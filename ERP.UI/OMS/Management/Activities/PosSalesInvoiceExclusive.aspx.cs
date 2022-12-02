using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;
using EntityLayer.CommonELS;
using System.Web.Services;
using System.Collections.Generic;
using System.Collections;
using DevExpress.Web.Data;
using DataAccessLayer;
using System.ComponentModel;
using System.Linq;
using EntityLayer;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Drawing;
using ERP.OMS.Tax_Details.ClassFile;
using System.Net;
using ERP.OMS.ViewState_class;
using ERP.Models;
using System.Web.Script.Services;

namespace ERP.OMS.Management.Activities
{
    public partial class PosSalesInvoiceExclusive : System.Web.UI.Page
    {
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();


        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        public static string IsLighterCustomePage = string.Empty;
        clsDropDownList oclsDropDownList = new clsDropDownList();
        BusinessLogicLayer.Contact oContactGeneralBL = new BusinessLogicLayer.Contact();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        static string ForJournalDate = null;
        SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
        SalesInvoiceBL objSalesInvoiceBL = new SalesInvoiceBL();
        CRMSalesDtlBL objCRMSalesDtlBL = new CRMSalesDtlBL();
        ERPDocPendingApprovalBL objERPDocPendingApproval = new ERPDocPendingApprovalBL();
        BusinessLogicLayer.RemarkCategoryBL reCat = new BusinessLogicLayer.RemarkCategoryBL();
        PosSalesInvoiceBl PosData = new PosSalesInvoiceBl();
        GSTtaxDetails gstTaxDetails = new GSTtaxDetails();

        string UniqueQuotation = string.Empty;
        DataTable dtPartyTotal = null;
        string UniqueChallan = string.Empty;
        public string pageAccess = "";
        string userbranch = "";
        string QuotationIds = string.Empty;
        decimal TotalTaggingAmount = 0;
        string InvoiceId = "";
        string PartyTotalBalAmt = "";
        string PartyTotalBalDesc = "";

        #region Product pop up


        //GenericMethod oGenericMethod;
        //DBEngine oDBEngine = new DBEngine(string.Empty);

        BusinessLogicLayer.GenericMethod oGenericMethod;

        public EntityLayer.CommonELS.UserRightsForPage rightsProd = new UserRightsForPage();
        public ProductComponentBL prodComp = new ProductComponentBL();
        BusinessLogicLayer.MasterDataCheckingBL masterChecking = new BusinessLogicLayer.MasterDataCheckingBL();

        BusinessLogicLayer.MasterSettings objmaster = new BusinessLogicLayer.MasterSettings();

        #endregion



        protected void Page_Init(object sender, EventArgs e)
        {
            PaymentDetails.doc_type = "POS";
            if (Request.QueryString["key"] != "ADD")
            {
                PaymentDetails.StorePaymentDetailsToHiddenfield(Convert.ToInt32(Request.QueryString["key"]));
            }

            oldUnitDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            tdstcs.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlClassSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlHSNDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            ProductDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            HsnDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }


        //Chinmoy commented 02-08-2019
        //Start
        //protected int getUdfCount()
        //{
        //    DataTable udfCount = oDBEngine.GetDataTable("select 1 from tbl_master_remarksCategory rc where cat_applicablefor='POS'  and ( exists (select * from tbl_master_udfGroup where id=rc.cat_group_id and grp_isVisible=1) or rc.cat_group_id=0)");
        //    return udfCount.Rows.Count;
        //}
        //End
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/CustomerMasterList.aspx");
            rightsProd = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/store/Master/sProducts.aspx");
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }

            if (Convert.ToString(Request.QueryString["key"]) != "ADD")
                InvoiceId = Convert.ToString(Request.QueryString["key"]);


            //Start Rev set TDS type OLD/NEW Tanmoy 12-08-2019
            SendSmsBL objTdsTcsBL = new SendSmsBL();
            DataTable dt = objTdsTcsBL.GetSMSSettings("Show_SendSMS_CheckBox");
            if (dt != null && dt.Rows.Count > 0)
            {
                hdnsendsmsSettings.Value = dt.Rows[0]["Variable_Value"].ToString();
            }
            if (hdnsendsmsSettings.Value == "No")
            {
                divSendSMS.Attributes["class"] = "hidden";
            }
            //End Rev Tanmoy

            if (!IsPostBack)
            {
                #region Rajdip
                //For Credit & due date show
                if (Convert.ToString(Request.QueryString["type"]) != "Crd")
                {
                    Creditdatediv.Visible = false;
                    DueDatediv.Visible = false;
                }
                else
                {
                    Creditdatediv.Visible = true;
                    DueDatediv.Visible = true;
                }
                CommonBL ComBL = new CommonBL();
                string AltCrDateMandatoryInSI = ComBL.GetSystemSettingsResult("AltCrDateMandatoryInPOS");
                if (!String.IsNullOrEmpty(AltCrDateMandatoryInSI))
                {
                    if (AltCrDateMandatoryInSI == "Yes")
                    {
                        hdnCrDateMandatory.Value = "1";

                    }
                    else if (AltCrDateMandatoryInSI.ToUpper().Trim() == "NO")
                    {
                        hdnCrDateMandatory.Value = "0";
                    }
                }
                #endregion Rajdip
                CommonBL cbl = new CommonBL();
                ISAllowBackdatedEntry.Value = cbl.GetSystemSettingsResult("BackDatedEntryInPOS");
                oldUnitDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

                //To Get An UniqueId
                uniqueId.Value = Guid.NewGuid().ToString();

                //  #region NewTaxblock
                //  string ItemLevelTaxDetails = string.Empty; string HSNCodewisetaxSchemid = string.Empty; string BranchWiseStateTax = string.Empty; string StateCodeWiseStateIDTax = string.Empty;
                //  gstTaxDetails.GetTaxData(ref ItemLevelTaxDetails, ref HSNCodewisetaxSchemid, ref BranchWiseStateTax, ref StateCodeWiseStateIDTax, "S"); 
                ////  HDItemLevelTaxDetails.Value = ItemLevelTaxDetails;
                //  HDHSNCodewisetaxSchemid.Value = HSNCodewisetaxSchemid;
                //  HDBranchWiseStateTax.Value = BranchWiseStateTax;
                //  HDStateCodeWiseStateIDTax.Value = StateCodeWiseStateIDTax;


                //  #endregion
                hidIsLigherContactPage.Value = "0";
                IsLighterCustomePage = "";

                string ISLigherpage = cbl.GetSystemSettingsResult("LighterCustomerEntryPage");
                if (!String.IsNullOrEmpty(ISLigherpage))
                {
                    if (ISLigherpage == "Yes")
                    {
                        hidIsLigherContactPage.Value = "1";
                        IsLighterCustomePage = "1";
                    }
                }




                SetFinYearCurrentDate();
                GetFinacialYearBasedQouteDate();
                deliveryDate.Date = dt_PLQuote.Date;
                dtChallandate.Date = dt_PLQuote.Date;

                if (Session["userbranchHierarchy"] != null)
                {
                    userbranch = Convert.ToString(Session["userbranchHierarchy"]);

                }
                GetAllDropDownDetailForSalesQuotation(userbranch);

                if (Request.QueryString.AllKeys.Contains("status"))
                {
                    divcross.Visible = false;
                    btn_SaveRecords.Visible = false;
                    ApprovalCross.Visible = true;
                    ddl_Branch.Enabled = false;
                }
                else
                {
                    divcross.Visible = true;
                    btn_SaveRecords.Visible = true;
                    ApprovalCross.Visible = false;
                    ddl_Branch.Enabled = true;
                }
                if (Request.QueryString["IsTagged"] != null)
                {
                    divcross.Visible = false;
                    ApprovalCross.Visible = false;
                }



                //DataTable ComponentTable = objSalesInvoiceBL.GetVehicle();
                //poupVehicles.GridView.Selection.CancelSelection();
                //poupVehicles.DataSource = ComponentTable;
                //poupVehicles.DataBind();
                //Session["SI_VehicleDetails"] = ComponentTable;

                dt_PlQuoteExpiry.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
                IsUdfpresent.Value = Convert.ToString(getUdfCount());
                hdnAddressDtl.Value = "0";


                #region Session Null Initialization Start
                Session["SI_BillingAddressLookup"] = null;
                Session["SI_ShippingAddressLookup"] = null;
                #endregion Session Null Initialization End




                Session["SI_InvoiceID"] = "";
                Session["SI_CustomerDetail"] = null;
                Session["SI_QuotationDetails" + Convert.ToString(uniqueId.Value)] = null;
                Session["SI_WarehouseData" + Convert.ToString(uniqueId.Value)] = null;
                Session["SI_QuotationTaxDetails"] = null;
                Session["SI_LoopWarehouse" + Convert.ToString(uniqueId.Value)] = 1;
                Session["SI_TaxDetails" + Convert.ToString(uniqueId.Value)] = null;
                Session["SI_ActionType"] = "";
                Session["SI_ComponentData"] = null;
                //PopulateGSTCSTVATCombo(DateTime.Now.ToString("yyyy-MM-dd"));
                //PopulateChargeGSTCSTVATCombo(DateTime.Now.ToString("yyyy-MM-dd"));
                string strQuotationId = "";
                hdnCustomerId.Value = "";
                hdnPageStatus.Value = "first";
                createOldUnittable();
                //LoadGSTCSTVATCombo();
                Session["SI_QuotationAddressDtl"] = null;



                if (Request.QueryString["key"] != null)
                {
                    if (Convert.ToString(Request.QueryString["key"]) != "ADD")
                    {
                        lblHeadTitle.Text = "Modify Sales Invoice";
                        hdnPageStatus.Value = "update";
                        hdAddOrEdit.Value = "Edit";
                        ASPxButton1.ClientVisible = false;
                        divScheme.Style.Add("display", "none");
                        btn_SaveRecords.ClientVisible = false;
                        //divSchemelbl.Style.Add("display", "none");
                        hdBasketId.Value = "";
                        //  ChallanNoSchmLbl.Style.Add("display", "none");
                        challanNoSchemedd.Style.Add("display", "none");

                        strQuotationId = Convert.ToString(Request.QueryString["key"]);
                        Session["SI_KeyVal_InternalID"] = "PIQUOTE" + strQuotationId;

                        Keyval_internalId.Value = "SaleInvoice" + strQuotationId;

                        #region Subhra Section
                        Session["SI_key_QutId"] = strQuotationId;


                        #endregion End Section

                        #region Product, Quotation Tax, Warehouse

                        Session["SI_InvoiceID"] = strQuotationId;
                        Session["SI_ActionType"] = "Edit";
                        SetInvoiceDetails(strQuotationId);
                        SetOldUnitDetails(strQuotationId);
                        SetCreditInvoiceDetails(strQuotationId);
                        //  DateTime quoteDate = Convert.ToDateTime(dt_PLQuote.Date.ToString("dd/MM/yyyy"));
                        Session["SI_TaxDetails" + Convert.ToString(uniqueId.Value)] = GetTaxDataWithGST(GetTaxData(dt_PLQuote.Date.ToString("yyyy-MM-dd")));

                        Session["SI_WarehouseData" + Convert.ToString(uniqueId.Value)] = GetQuotationWarehouseData();

                        DataTable Productdt = objSalesInvoiceBL.GetInvoiceProductData(strQuotationId).Tables[0];
                        string Quantity = GetTotalSumValue(Productdt, "Quantity").ToString();
                        string taxableAmount = GetTotalSumValue(Productdt, "TotalAmount").ToString();
                        string totalAmount = GetTotalSumValue(Productdt, "Amount").ToString();
                        string taxAmount = GetTotalSumValue(Productdt, "taxAmount").ToString();
                        ASPxLabel12.Text = Quantity;
                        bnrlblAmountWithTaxValue.Text = totalAmount;
                        bnrLblInvValue.Text = totalAmount;
                        bnrLblTaxAmtval.Text = taxAmount;
                        bnrLblTaxableAmtval.Text = taxableAmount;
                        SetOtherChargesBanaerValue();
                        Session["SI_QuotationDetails" + Convert.ToString(uniqueId.Value)] = Productdt;
                        grid.DataSource = GetQuotation(Productdt);
                        grid.DataBind();

                        PaymentDetails.Setbranchvalue(Convert.ToString(ddl_Branch.SelectedValue));

                        Session["SI_QuotationAddressDtl"] = objSalesInvoiceBL.GetInvoiceBillingAddress(strQuotationId); ;
                        PopulateShippingLabelDetailsbyPin(Convert.ToString(((DataTable)Session["SI_QuotationAddressDtl"]).Rows[1]["QuoteAdd_pin"]));
                        txtsAddress1.Text = Convert.ToString(((DataTable)Session["SI_QuotationAddressDtl"]).Rows[1]["QuoteAdd_address1"]);
                        txtsAddress2.Text = Convert.ToString(((DataTable)Session["SI_QuotationAddressDtl"]).Rows[1]["QuoteAdd_address2"]);
                        txtsAddress3.Text = Convert.ToString(((DataTable)Session["SI_QuotationAddressDtl"]).Rows[1]["QuoteAdd_address3"]);
                        txtslandmark.Text = Convert.ToString(((DataTable)Session["SI_QuotationAddressDtl"]).Rows[1]["QuoteAdd_landMark"]);


                        populateBillingLabelDetailsbyPin(Convert.ToString(((DataTable)Session["SI_QuotationAddressDtl"]).Rows[0]["QuoteAdd_pin"]));
                        txtAddress1.Text = Convert.ToString(((DataTable)Session["SI_QuotationAddressDtl"]).Rows[0]["QuoteAdd_address1"]);
                        txtAddress2.Text = Convert.ToString(((DataTable)Session["SI_QuotationAddressDtl"]).Rows[0]["QuoteAdd_address2"]);
                        txtAddress3.Text = Convert.ToString(((DataTable)Session["SI_QuotationAddressDtl"]).Rows[0]["QuoteAdd_address3"]);
                        txtlandmark.Text = Convert.ToString(((DataTable)Session["SI_QuotationAddressDtl"]).Rows[0]["QuoteAdd_landMark"]);

                        #endregion

                        #region Debjyoti Get Tax Details in Edit Mode

                        Session["SI_QuotationTaxDetails"] = GetQuotationTaxData().Tables[0];
                        DataTable quotetable = GetQuotationEditedTaxData().Tables[0];
                        if (quotetable == null)
                        {
                            CreateDataTaxTable();
                        }
                        else
                        {
                            Session["SI_FinalTaxRecord" + Convert.ToString(uniqueId.Value)] = quotetable;
                        }

                        #endregion Debjyoti Get Tax Details in Edit Mode

                        SetControlVissibility(Convert.ToString(HdPosType.Value));
                        #region viewMode
                        if (Request.QueryString["Viemode"] != null && Request.QueryString["Viemode"] == "1")
                        {

                            HdViewmode.Value = "Yes";
                            //dt_PLQuote.ClientEnabled = false;
                            cmbDeliveryType.ClientEnabled = false;
                            txt_Refference.ClientEnabled = false;
                            txtChallanNo.ClientEnabled = false;
                            dtChallandate.ClientEnabled = false;
                            cmbFinancer.ClientEnabled = false;
                            cmbExecName.ClientEnabled = false;
                            txtEmiDetails.ClientEnabled = false;
                            txtFinanceAmt.ClientEnabled = false;
                            txtDBD.ClientEnabled = false;
                            txtDbdPercen.ClientEnabled = false;
                            txtprocFee.ClientEnabled = false;
                            txtSfCode.ClientEnabled = false;
                            grid.Enabled = false;
                            cmbOldUnit.ClientEnabled = false;
                            txtunitValue.ClientEnabled = false;
                            txtRemarks.ClientEnabled = false;
                            txtAdvnceReceipt.ClientEnabled = false;

                            lblHeadTitle.Text = "View Invoice";
                        }
                        else
                        {
                            // Else part denote only theedit mode
                            grid.Enabled = false;
                            // cmbDeliveryType.ClientEnabled = false;
                            cmbDeliveryType.Items.RemoveAt(2);
                            oldunitPopupSaveAndClickClick.ClientVisible = false;
                            ASPxButton8.ClientVisible = false;
                            //  OldUnitGrid.Enabled = false;
                            oldUnitGridAdd.ClientVisible = false;
                            txtFinanceAmt.ClientEnabled = false;
                            lookup_quotation.ClientEnabled = false;
                            rdl_SaleInvoice.Enabled = false;
                            if (HdPosType.Value == "Cash")
                                lblHeadTitle.Text = lblHeadTitle.Text + " (Cash)";
                            else if (HdPosType.Value == "Fin")
                                lblHeadTitle.Text = lblHeadTitle.Text + " (Finance)";
                            else if (HdPosType.Value == "Crd")
                                lblHeadTitle.Text = lblHeadTitle.Text + " (Credit)";
                            else if (HdPosType.Value == "IST")
                                lblHeadTitle.Text = lblHeadTitle.Text + " (Interstate Stock Transfer)";
                            dt_PLQuote.ClientEnabled = false;
                        }
                        #endregion

                    }
                    else
                    {
                        DataTable dtposTime = oDBEngine.GetDataTable("SELECT  top 1 convert(varchar(50),Lock_Fromdate,110) LockCon_Fromdate,convert(varchar(50),Lock_Todate,110) LockCon_Todate FROM Trans_LockConfigouration_Details WHERE  Type='Add' and Module_Id=44");

                        if (dtposTime != null && dtposTime.Rows.Count > 0)
                        {
                            hdnLockFromDate.Value = Convert.ToString(dtposTime.Rows[0]["LockCon_Fromdate"]);
                            hdnLockToDate.Value = Convert.ToString(dtposTime.Rows[0]["LockCon_Todate"]);
                        }

                        #region To Show By Default Cursor after SAVE AND NEW
                        if (Session["SI_SaveMode"] != null)  // it has been removed from coding side of Quotation list 
                        {
                            if (Session["SI_schemavalue"] != null)  // it has been removed from coding side of Quotation list 
                            {
                                string itemValue = Convert.ToString(Session["SI_schemavalue"]); // it has been removed from coding side of Quotation list 
                                ddl_numberingScheme.SelectedValue = itemValue;
                                ddl_Branch.SelectedValue = itemValue.Split('~')[3];
                                bidfinancerByBranch(Convert.ToString(itemValue.Split('~')[3]));
                                bindChallanNumeringScheme(Convert.ToString(itemValue.Split('~')[3]));

                                // GetCashBalanceByBranch(Convert.ToString(itemValue.Split('~')[3]));
                            }
                            if (Convert.ToString(Session["SI_SaveMode"]) == "A")
                            {
                                dt_PLQuote.Focus();
                                txt_PLQuoteNo.ClientEnabled = false;
                                txt_PLQuoteNo.Text = "Auto";
                            }
                            else if (Convert.ToString(Session["SI_SaveMode"]) == "M")
                            {
                                txt_PLQuoteNo.ClientEnabled = true;
                                txt_PLQuoteNo.Text = "";
                                txt_PLQuoteNo.Focus();

                            }
                        }
                        else
                        {
                            ddl_numberingScheme.Focus();
                            for (int itmCnt = 1; itmCnt < ddl_numberingScheme.Items.Count; itmCnt++)
                            {
                                string itemValue = ddl_numberingScheme.Items[itmCnt].Value;
                                if (itemValue.Split('~')[1] == "1")
                                {
                                    ddl_numberingScheme.SelectedIndex = itmCnt;
                                    txt_PLQuoteNo.Text = "Auto";
                                    ddl_Branch.SelectedValue = itemValue.Split('~')[3];
                                    txt_PLQuoteNo.ClientEnabled = false;
                                    // bidfinancerByBranch(Convert.ToString(itemValue.Split('~')[3]));
                                    // bindChallanNumeringScheme(Convert.ToString(itemValue.Split('~')[3])); 
                                    break;
                                }
                            }
                        }
                        // bindSalesmanByBranch(Convert.ToString(ddl_Branch.SelectedValue));
                        GetCashBalanceByBranch(Convert.ToString(ddl_Branch.SelectedValue));
                        ASPxButton7.ClientVisible = false;
                        #endregion To Show By Default Cursor after SAVE AND NEW
                        hdAddOrEdit.Value = "Add";

                        if (ISAllowBackdatedEntry.Value == "No")
                        {
                            dt_PLQuote.ClientEnabled = false;
                        }


                        Keyval_internalId.Value = "Add";
                        Session["SI_ActionType"] = "Add";
                        //ASPxButton2.Enabled = false;
                        Session["SI_TaxDetails" + Convert.ToString(uniqueId.Value)] = null;
                        CreateDataTaxTable();
                        lblHeadTitle.Text = "Add Sales Invoice (POS)";
                        PaymentDetails.Setbranchvalue(Convert.ToString(ddl_Branch.SelectedValue));
                        ddl_VatGstCst.SelectedIndex = 0;
                        ddl_VatGstCst.ClientEnabled = false;
                        SetHeaderText("Add", Convert.ToString(Request.QueryString["type"]));
                        if (Request.QueryString["BasketId"] != null)
                        {
                            string basketId = Convert.ToString(Request.QueryString["BasketId"]);
                            fillRecordFromBasket(basketId);
                            hdBasketId.Value = basketId;
                        }
                        else
                        {
                            hdBasketId.Value = "";
                        }
                        if (Request.QueryString["type"] != null)
                        {
                            HdPosType.Value = Convert.ToString(Request.QueryString["type"]);
                        }
                        hdnPageStatus.Value = "first";
                        bindOldUnitGrid();
                        SetControlVissibility(Convert.ToString(HdPosType.Value));

                    }
                }

                //   ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "GridCallBack()", true);


                MasterSettings objmaster = new MasterSettings(); // Surojit 15-03-2019
                hdnConvertionOverideVisible.Value = objmaster.GetSettings("ConvertionOverideVisible");// Surojit 15-03-2019
                hdnShowUOMConversionInEntry.Value = objmaster.GetSettings("ShowUOMConversionInEntry");// Surojit 15-03-2019

                //Rev Subhra 23-04-2019
                hdnShowOldUnitInPOS.Value = objmaster.GetSettings("ShowOldUnitInPOS");
                //End of Rev Subhra 23-04-2019

                //Tanmoy 26-04-2019
                string ShowDeliveryTypeInPos = objmaster.GetSettings("ShowDeliveryTypeInPos");

                if (ShowDeliveryTypeInPos == "0")
                {
                    string DefaultDeliveryTypeInPos = objmaster.GetSettings("DefaultDeliveryTypeInPos");
                    tdDeliveryType.Attributes.Add("Style", "Display:none;");
                    cmbDeliveryType.Value = DefaultDeliveryTypeInPos;
                }
                //Rev Subhra 22-05-2019
                MasterSettings objmasterposprint = new MasterSettings();
                hdnPosDocPrintDesignBasedOnTaxCategory.Value = objmasterposprint.GetSettings("PosDocPrintDesignBasedOnTaxCategory");
                //End of Rev Subhra 22-05-2019



                #region product product pop up

                string InstSetting = objmaster.GetSettings("ShowPOSAttributeinProductMaster");

                if (InstSetting == "1")
                {
                    divPosInstallation.Style.Add("display", "block");
                    divPosOldUnit.Style.Add("display", "block");
                }
                else
                {
                    divPosInstallation.Style.Add("display", "none");
                    divPosOldUnit.Style.Add("display", "none");
                }


                //BindCountry();
                //BindState(1); 
                IsUdfpresent.Value = Convert.ToString(getUdfCount());
                Session["exportval"] = null;

                BindProType();
                BindProductSize();
                //BindProClassCode();
                BindProductColor();
                BindQuoteCurrency();
                BindTradingLotUnits();
                BindBarCodeType();
                BindTaxCode("S", CmbTaxCodeSale);
                BindTaxCode("P", CmbTaxCodePur);
                BindTaxScheme();
                BindServiceTax();
                BindBrand();
                bindMainAccounts();

                #endregion



            }

        }

        #region product pop up
        protected void BindProType()
        {
            //  / oGenericMethod = new GenericMethod();
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

            DataTable dtCmb = new DataTable();
            dtCmb = oGenericMethod.GetDataTable("SELECT name,sProducts_Type FROM ( SELECT 'Raw Material' AS name, 'A' AS sProducts_Type UNION SELECT 'Work-In-Process' AS name, 'B' AS sProducts_Type UNION SELECT 'Finished Goods' AS name, 'C' AS sProducts_Type) X order by sProducts_Type  ");
            AspxHelper oAspxHelper = new AspxHelper();
            if (dtCmb.Rows.Count > 0)
            {
                oAspxHelper.Bind_Combo(CmbProType, dtCmb, "name", "sProducts_Type", "");
            }

        }


        protected void bindMainAccounts()
        {
            //BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

            //DataTable dtCmb = new DataTable();
            //ProcedureExecute proc = new ProcedureExecute("prc_ProductMaster_bindData");
            //proc.AddVarcharPara("@action", 20, "GetMainAccount"); 
            //dtCmb = proc.GetTable();



            //cmbsalesInvoice.DataSource = dtCmb;
            //cmbsalesInvoice.TextField = "MainAccount_Name";
            //cmbsalesInvoice.ValueField = "MainAccount_AccountCode";
            //cmbsalesInvoice.DataBind();

            //cmbPurInvoice.DataSource = dtCmb;
            //cmbPurInvoice.TextField = "MainAccount_Name";
            //cmbPurInvoice.ValueField = "MainAccount_AccountCode";
            //cmbPurInvoice.DataBind();

            //cmbSalesReturn.DataSource = dtCmb;
            //cmbSalesReturn.TextField = "MainAccount_Name";
            //cmbSalesReturn.ValueField = "MainAccount_AccountCode";
            //cmbSalesReturn.DataBind();

            //cmbPurReturn.DataSource = dtCmb;
            //cmbPurReturn.TextField = "MainAccount_Name";
            //cmbPurReturn.ValueField = "MainAccount_AccountCode";
            //cmbPurReturn.DataBind();


        }



        protected void BindServiceTax()
        {
            DataTable serviceTax = oDBEngine.GetDataTable("SELECT TAX_ID,SERVICE_CATEGORY_CODE,SERVICE_TAX_NAME,ACCOUNT_HEAD_TAX_RECEIPTS,ACCOUNT_HEAD_OTHERS_RECEIPTS,ACCOUNT_HEAD_PENALTIES,ACCOUNT_HEAD_DeductRefund FROM TBL_MASTER_SERVICE_TAX");
            AspxServiceTax.DataSource = serviceTax;
            AspxServiceTax.DataBind();
        }

        //chinmoy commented 02-08-2019
        //start
        //protected void BindProClassCode()
        //{
        //    //  / oGenericMethod = new GenericMethod();
        //    BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

        //    DataTable dtCmb = new DataTable();
        //    dtCmb = oGenericMethod.GetDataTable("SELECT ProductClass_ID,ProductClass_Name FROM Master_ProductClass order by ProductClass_Name");
        //    AspxHelper oAspxHelper = new AspxHelper();
        //    if (dtCmb.Rows.Count > 0)
        //    {
        //        oAspxHelper.Bind_Combo(CmbProClassCode, dtCmb, "ProductClass_Name", "ProductClass_ID", "");
        //    }

        //}
        //End 02-08-2019
        //Tax Code bind here Debjyoti 05-01-2017
        protected void BindTaxCode(string taxType, ASPxComboBox cmb)
        {

            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

            DataTable dtCmb = new DataTable();
            //dtCmb = oGenericMethod.GetDataTable("select 0  Taxes_ID,'-Select-' Taxes_Name union all select Taxes_ID,Taxes_Name from Master_Taxes where Taxes_ApplicableFor in('B','" + taxType.Trim() + "')");
            dtCmb = oGenericMethod.GetDataTable("select 0  Taxes_SchemeID,'--Select--' Taxes_SchemeName union all select TaxRates_ID,TaxRatesSchemeName from Config_TaxRates ct inner join Master_Taxes mt on ct.TaxRates_TaxCode=mt.Taxes_ID where TaxRates_TaxCode in (select Taxes_ID from Master_Taxes where Taxes_ApplicableFor in ('B','" + taxType + "')) and mt.TaxTypeCode<>'O' order by Taxes_SchemeName");

            AspxHelper oAspxHelper = new AspxHelper();

            if (dtCmb.Rows.Count > 0)
            {
                oAspxHelper.Bind_Combo(cmb, dtCmb, "Taxes_SchemeName", "Taxes_SchemeID");
            }



        }

        //tax Scheme bind here debjyoti 05-01-2017

        protected void BindTaxScheme()
        {

            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

            DataTable dtCmb = new DataTable();

            dtCmb = oGenericMethod.GetDataTable("select 0  TaxRates_ID,'-Select-' TaxRates_Scheme union all select TaxRates_ID,isnull(TaxRatesSchemeName,'') from Config_TaxRates");
            AspxHelper oAspxHelper = new AspxHelper();

            if (dtCmb.Rows.Count > 0)
            {
                oAspxHelper.Bind_Combo(CmbTaxScheme, dtCmb, "TaxRates_Scheme", "TaxRates_ID");
            }



        }

        //BarCode tye added here Debjyoti 30-12-2016
        protected void BindBarCodeType()
        {
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

            DataTable dtCmb = new DataTable();
            dtCmb = oGenericMethod.GetDataTable("select id,Symbology from tbl_master_BarCodeSymbology where isActive=1");
            AspxHelper oAspxHelper = new AspxHelper();
            if (dtCmb.Rows.Count > 0)
            {
                oAspxHelper.Bind_Combo(CmbBarCodeType, dtCmb, "Symbology", "id");
            }

        }


        protected void BindHsnCode()
        {
            //BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

            //DataTable dtCmb = new DataTable();
            //dtCmb = oGenericMethod.GetDataTable("select HSN_id,Code+'  ['+Description+']' as Description  from  tbl_HSN_Master");
            //aspxHsnCode.DataSource = dtCmb;
            //aspxHsnCode.ValueField = "HSN_id";
            //aspxHsnCode.TextField = "Description";
            //aspxHsnCode.DataBind();

        }


        protected void BindTradingLotUnits()
        {
            //  / oGenericMethod = new GenericMethod();
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

            DataTable dtCmb = new DataTable();
            dtCmb = oGenericMethod.GetDataTable("select UOM_ID,UOM_Name from Master_UOM  order by UOM_Name");
            AspxHelper oAspxHelper = new AspxHelper();
            if (dtCmb.Rows.Count > 0)
            {
                oAspxHelper.Bind_Combo(CmbTradingLotUnits, dtCmb, "UOM_Name", "UOM_ID", "");

                oAspxHelper.Bind_Combo(CmbQuoteLotUnit, dtCmb, "UOM_Name", "UOM_ID", "");

                oAspxHelper.Bind_Combo(CmbDeliveryLotUnit, dtCmb, "UOM_Name", "UOM_ID", "");

                //added for stock uom
                oAspxHelper.Bind_Combo(cmbStockUom, dtCmb, "UOM_Name", "UOM_ID", "");

                //Added for packing uom
                oAspxHelper.Bind_Combo(cmbPackingUomPro, dtCmb, "UOM_Name", "UOM_ID", "");

                // oAspxHelper.Bind_Combo(ddlCovgUOM, dtCmb, "UOM_Name", "UOM_ID", "");


                oAspxHelper.Bind_Combo(ddlSize, dtCmb, "UOM_Name", "UOM_ID", "");



            }

        }


        protected void BindQuoteCurrency()
        {
            //  / oGenericMethod = new GenericMethod();
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

            DataTable dtCmb = new DataTable();
            dtCmb = oGenericMethod.GetDataTable("select Currency_ID, Currency_Name  from Master_Currency order by Currency_Name");
            AspxHelper oAspxHelper = new AspxHelper();
            if (dtCmb.Rows.Count > 0)
            {
                oAspxHelper.Bind_Combo(CmbQuoteCurrency, dtCmb, "Currency_Name", "Currency_ID", "");
            }

        }

        protected void BindProductColor()
        {
            //  / oGenericMethod = new GenericMethod();
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

            DataTable dtCmb = new DataTable();
            //................CODE UPDATED BY sAM ON 18102016.................................................
            //dtCmb = oGenericMethod.GetDataTable("SELECT [Color_ID],[Color_Name] FROM [dbo].[Master_Color] UNION SELECT 0 AS [Color_ID],'None' AS [Color_Name] UNION SELECT NULL AS [Color_ID],'' AS [Color_Name] ORDER BY [Color_ID]");
            dtCmb = oGenericMethod.GetDataTable("SELECT [Color_ID],[Color_Name] FROM [dbo].[Master_Color] UNION SELECT 0 AS [Color_ID],'Select' AS [Color_Name] ");

            //................CODE ABOVE UPDATED BY sAM ON 18102016.................................................
            AspxHelper oAspxHelper = new AspxHelper();
            if (dtCmb.Rows.Count > 0)
            {
                oAspxHelper.Bind_Combo(CmbProductColor, dtCmb, "Color_Name", "Color_ID", "");
                CmbProductColor.SelectedIndex = 0;
            }

        }

        protected void BindBrand()
        {
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

            DataTable dtCmb = new DataTable();
            dtCmb = oGenericMethod.GetDataTable("select Brand_Id ,Brand_Name from tbl_master_brand where Brand_IsActive=1");

            AspxHelper oAspxHelper = new AspxHelper();
            if (dtCmb.Rows.Count > 0)
            {
                oAspxHelper.Bind_Combo(cmbBrand, dtCmb, "Brand_Name", "Brand_Id", "");
            }

        }



        protected void BindProductSize()
        {
            //  / oGenericMethod = new GenericMethod();
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();

            DataTable dtCmb = new DataTable();
            //................CODE UPDATED BY sAM ON 18102016.................................................
            //dtCmb = oGenericMethod.GetDataTable("SELECT [Size_ID],[Size_Name] FROM [dbo].[Master_Size] UNION SELECT 0 AS [Size_ID],'None' AS [Size_Name] UNION SELECT NULL AS [Size_ID],'' AS [Size_Name]");
            dtCmb = oGenericMethod.GetDataTable("SELECT [Size_ID],[Size_Name] FROM [dbo].[Master_Size] UNION SELECT 0 AS [Size_ID],'Select' AS [Size_Name]");
            //................CODE aBOVE UPDATED BY sAM ON 18102016.................................................
            AspxHelper oAspxHelper = new AspxHelper();
            if (dtCmb.Rows.Count > 0)
            {
                oAspxHelper.Bind_Combo(CmbProductSize, dtCmb, "Size_Name", "Size_ID", "");
                CmbProductSize.SelectedIndex = 0;
            }

        }

        #endregion

        protected decimal GetTotalSumValue(DataTable recvTable, string columName)
        {
            decimal tempValue = 0;
            foreach (DataRow dr in recvTable.Rows)
            {
                tempValue += Convert.ToDecimal(dr[columName]);
            }

            return tempValue;
        }

        private void SetCreditInvoiceDetails(string stringInvId)
        {
            if (Convert.ToString(HdPosType.Value) == "Crd" || Convert.ToString(HdPosType.Value) == "Fin")
            {
                DataTable CustomerRecPayDtExists = PosData.GetCustomerReceiptDetailsByInvoiceId(stringInvId);
                Session["PosCustomerReceiptDetails" + Convert.ToString(uniqueId.Value)] = CustomerRecPayDtExists;
                aspxCustomerReceiptGridview.DataBind();

                for (int i = 0; i < aspxCustomerReceiptGridview.VisibleRowCount; i++)
                {
                    aspxCustomerReceiptGridview.Selection.SelectRow(i);
                }
            }
        }


        public void SetControlVissibility(string requestedtype)
        {
            if (requestedtype == "Cash")
            {
                idclsbnrLblLessAdvance.Style.Add("display", "none");
                FinancerTable.Style.Add("display", "none");
                lblAdvnceRecptNovalue.Style.Add("display", "none");
                lblAdvnceRecptNo.Style.Add("display", "none");
            }
            else if (requestedtype == "Crd")
            {
                FinancerTable.Style.Add("display", "none");
            }
            else if (requestedtype == "Fin")
            {
                //idclsbnrLblLessAdvance.Style.Add("display", "none");
                //lblAdvnceRecptNovalue.Style.Add("display", "none");
                //lblAdvnceRecptNo.Style.Add("display", "none");
            }
            else if (requestedtype == "IST")
            {
                idclsbnrLblLessAdvance.Style.Add("display", "none");
                lblAdvnceRecptNovalue.Style.Add("display", "none");
                lblAdvnceRecptNo.Style.Add("display", "none");
                FinancerTable.Style.Add("display", "none");
                unitValueID.Style.Add("display", "none");
                UnitValueCombo.Style.Add("display", "none");
                oldunitButton.Style.Add("display", "none");
                unitvaluelbl.Style.Add("display", "none");
                unitValueText.Style.Add("display", "none");
                oldUnitBanerLbl.Style.Add("display", "none");
                lbl_SaleInvoiceNo.Text = "Document Number";
                PaymentDetails.Visible = false;
            }

        }

        public void SetHeaderText(string mode, string code)
        {
            if (mode == "Add" && code != "")
            {

                if (code == "Cash")
                    lblHeadTitle.Text = "Add Sales Invoice (Cash)";
                else if (code == "Crd")
                    lblHeadTitle.Text = "Add Credit Invoice(POS)";
                else if (code == "Fin")
                    lblHeadTitle.Text = "Add Finance Invoice(POS)";
                else if (code == "IST")
                    lblHeadTitle.Text = "Interstate Stock Transfer";
            }
        }

        protected void grid_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "Number")
            {
                e.Value = string.Format("Item #{0}", e.ListSourceRowIndex);
            }
            if (e.Column.FieldName == "Warehouse")
            {
            }
        }

        #region Code Checked and Modified
        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {
            if (Request.QueryString.AllKeys.Contains("Viemode") || Request.QueryString.AllKeys.Contains("Icoupon") || Request.QueryString.AllKeys.Contains("IsTagged"))
            {
                this.Page.MasterPageFile = "~/OMS/MasterPage/PopUp.Master";
                //divcross.Visible = false;
            }
            else
            {
                this.Page.MasterPageFile = "~/OMS/MasterPage/ERP.Master";
                //divcross.Visible = true;
            }
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);


                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }

        #endregion
        //Chinmoy added  for Product Popup  02-08-2019
        //Start

        protected void CmbState_Callback(object source, CallbackEventArgsBase e)
        {
            string WhichCall = e.Parameter.Split('~')[0];
            if (WhichCall == "BindState")
            {
                int countryID = Convert.ToInt32(Convert.ToString(e.Parameter.Split('~')[1]));
                BindState(countryID);
            }
        }

        protected void CmbCity_Callback(object source, CallbackEventArgsBase e)
        {
            string WhichCall = e.Parameter.Split('~')[0];
            if (WhichCall == "BindCity")
            {
                int countryID = Convert.ToInt32(Convert.ToString(e.Parameter.Split('~')[1]));
                BindCity(countryID);
            }
        }


        protected void cityGrid_CustomCallback(object sender, CallbackEventArgsBase e)
        {
            gridPro.JSProperties["cpinsert"] = null;
            gridPro.JSProperties["cpEdit"] = null;
            gridPro.JSProperties["cpUpdate"] = null;
            gridPro.JSProperties["cpDelete"] = null;
            gridPro.JSProperties["cpExists"] = null;
            gridPro.JSProperties["cpUpdateValid"] = null;

            int insertcount = 0;
            int updtcnt = 0;
            int deletecnt = 0;

            // oGenericMethod = new GenericMethod();
            oGenericMethod = new BusinessLogicLayer.GenericMethod();

            string WhichCall = Convert.ToString(e.Parameter).Split('~')[0];
            string WhichType = null;
            if (Convert.ToString(e.Parameter).Contains("~"))
                if (Convert.ToString(e.Parameter).Split('~')[1] != "")
                    WhichType = Convert.ToString(e.Parameter).Split('~')[1];


            if (WhichCall == "savecity")
            {
                oGenericMethod = new BusinessLogicLayer.GenericMethod();
                Store_MasterBL oStore_MasterBL = new Store_MasterBL();
                int TradingLot = 0;
                int QuoteLot = 0;
                int DeliveryLot = 0;
                int productSize = 0;
                int ProductColor = 0;
                string strMsg = "fail";
                //-----Arindam
                txtQuoteLot.Text = "1";
                txtTradingLot.Text = "1";
                txtDeliveryLot.Text = "1";

                string lenght = txtHeight.Text;
                string width = txtWidth.Text;
                string Thickness = txtThickness.Text;
                string size = ddlSize.Value.ToString();
                string SUOM = SizeUOM.Value;
                string series = txtSeries.Text;
                string Finish = txtFinish.Text;
                string LeadTime = txtLeadtime.Text;
                string Coverage = txtCoverage.Value;
                string covuom = dvCovg.InnerText;
                string volume = txtVolumn.Value;
                string volumeuom = dvvolume.InnerText;
                string wight = txtWeight.Text;
                string subcat = txtSubCat.Text;

                //--Arindam for tryparse
                /*insertcount = oStore_MasterBL.InsertProduct(txtPro_Code.Text, txtPro_Name.Text, txtPro_Description.Text,
                    Convert.ToString(CmbProType.SelectedItem.Value), Convert.ToInt32(CmbProClassCode.SelectedItem.Value), txtGlobalCode.Text,
                    TradingLot, Convert.ToInt32(CmbTradingLotUnits.SelectedItem.Value));*/
                if (!string.IsNullOrEmpty(txtPro_Code.Text.Trim()) && !string.IsNullOrEmpty(txtPro_Name.Text.Trim()))
                {

                    if (int.TryParse(txtQuoteLot.Text, out QuoteLot))
                    {
                        if (int.TryParse(txtTradingLot.Text, out TradingLot))
                        {
                            if (int.TryParse(txtDeliveryLot.Text, out DeliveryLot))
                            {
                                //if (CmbProductSize.SelectedItem.Value != "") //28.12.2016 commented by Subhra because it's getting error
                                if (CmbProductSize.Text != "")
                                {
                                    productSize = Convert.ToInt32(CmbProductSize.SelectedItem.Value);
                                }
                                if (CmbProductColor.Text != "")
                                //if (CmbProductColor.SelectedItem.Value != "") //28.12.2016 commented by Subhra because it's getting error
                                {
                                    ProductColor = Convert.ToInt32(CmbProductColor.SelectedItem.Value);
                                }
                                Boolean sizeapplicable = false;
                                Boolean colorapplicable = false;
                                Boolean isInventory = false;
                                Boolean autoApply = false;
                                Boolean isInstall = false;
                                Boolean isOldUnit = false;
                                Boolean IsServiceItem = false;
                                Boolean FurtheranceToBusiness = false;//Subhabrata
                                Boolean OverideConvertion = false; //Surojit 08-02-2019
                                Boolean IsMandatory = false; //Surojit 11-02-2019
                                decimal saleprice = 0;
                                decimal MinSaleprice = 0;
                                decimal purPrice = 0;
                                decimal MRP = 0;
                                decimal minLvl = 0;
                                decimal maxLvl = 0;
                                decimal reorderLvl = 0;
                                decimal reorder_qty = 0;
                                Boolean isCapitalGoods = false;
                                int tdscode = 0;

                                if (cmb_tdstcs.Value != null)
                                {
                                    tdscode = Convert.ToInt32(cmb_tdstcs.Value);
                                }

                                if (chkFurtherance.Checked)
                                {
                                    FurtheranceToBusiness = true;
                                }

                                if (ChkAutoApply.Checked)
                                    autoApply = true;

                                if (chkOverideConvertion.Checked) //Surojit 08-02-2019
                                    OverideConvertion = true;

                                if (chkIsMandatory.Checked) //Surojit 11-02-2019
                                    IsMandatory = true;

                                if (txtReorderLvl.Text.Trim() != "")
                                {
                                    reorderLvl = Convert.ToDecimal(txtReorderLvl.Text.Trim());
                                }

                                if (txtReorderQty.Text.Trim() != "")
                                {
                                    reorder_qty = Convert.ToDecimal(txtReorderQty.Text.Trim());

                                }

                                if (txtMinLvl.Text.Trim() != "")
                                {
                                    minLvl = Convert.ToDecimal(txtMinLvl.Text.Trim());
                                }


                                if (txtMaxLvl.Text.Trim() != "")
                                {
                                    maxLvl = Convert.ToDecimal(txtMaxLvl.Text.Trim());
                                }




                                if (txtMrp.Text.Trim() != "")
                                {
                                    MRP = Convert.ToDecimal(txtMrp.Text.Trim());
                                }

                                if (txtPurPrice.Text.Trim() != "")
                                {
                                    purPrice = Convert.ToDecimal(txtPurPrice.Text.Trim());
                                }

                                if (txtMinSalePrice.Text.Trim() != "")
                                {
                                    MinSaleprice = Convert.ToDecimal(txtMinSalePrice.Text.Trim());
                                }

                                if (txtSalePrice.Text.Trim() != "")
                                {
                                    saleprice = Convert.ToDecimal(txtSalePrice.Text.Trim());
                                }
                                if (rdblappColor.Items[0].Selected)
                                {
                                    colorapplicable = true;
                                }
                                else
                                {
                                    colorapplicable = false;
                                }


                                if (rdblapp.Items[0].Selected)
                                {
                                    sizeapplicable = true;
                                }
                                else
                                {
                                    sizeapplicable = false;
                                }

                                if (Convert.ToString(cmbIsInventory.SelectedItem.Value) == "1")
                                    isInventory = true;

                                //Get Product Componnet details
                                String ProdComponent = "";
                                List<object> ComponentList = GridLookup.GridView.GetSelectedFieldValues("sProducts_ID");
                                foreach (object Pobj in ComponentList)
                                {
                                    ProdComponent += "," + Pobj;
                                }
                                ProdComponent = ProdComponent.TrimStart(',');

                                if (Convert.ToString(aspxInstallation.SelectedItem.Value) == "1")
                                    isInstall = true;

                                if (Convert.ToString(cmbOldUnit.SelectedItem.Value) == "1")
                                    isOldUnit = true;


                                if (Convert.ToString(cmbIsCapitalGoods.SelectedItem.Value) == "1")
                                    isCapitalGoods = true;

                                if (Convert.ToString(cmbServiceItem.SelectedItem.Value) == "1")
                                    IsServiceItem = true;

                                if (!reCat.isAllMandetoryDone((DataTable)Session["UdfDataOnAdd"], "Prd"))
                                {
                                    gridPro.JSProperties["cpinsert"] = "UDFManddratory";
                                    return;

                                }

                                if (HttpContext.Current.Session["userid"] != null)
                                {

                                    insertcount = oStore_MasterBL.InsertProduct(txtPro_Code.Text, txtPro_Name.Text, txtPro_Description.Text,
                                     Convert.ToString(CmbProType.Value == null ? 0 : CmbProType.SelectedItem.Value),

                                     //chinmoy cooment 19-07-2019
                                        // Convert.ToInt32(CmbProClassCode.Value == null ? 0 : CmbProClassCode.SelectedItem.Value), 
                                     (ClassId.Value == "" ? 0 : Convert.ToInt32(ClassId.Value)),
                                        //End

                                     txtGlobalCode.Text,
                                     1, Convert.ToInt32(CmbTradingLotUnits.Value == null ? 0 : CmbTradingLotUnits.SelectedItem.Value),
                                     1, 1, 1, 1, Convert.ToInt32(CmbDeliveryLotUnit.Value == null ? 0 : CmbDeliveryLotUnit.SelectedItem.Value), ProductColor,
                                     productSize, Convert.ToInt32(HttpContext.Current.Session["userid"]), sizeapplicable, colorapplicable,
                                     Convert.ToInt32(CmbBarCodeType.Value == null ? 0 : CmbBarCodeType.SelectedItem.Value), txtBarCodeNo.Text.Trim(),
                                        // Rev Srijeeta mantis issue 0024515
                                     //isInventory, Convert.ToString(CmbStockValuation.SelectedItem.Value), saleprice, MinSaleprice, purPrice, MRP,
                                     isInventory, Convert.ToString(CmbStockValuation.SelectedItem.Value), saleprice, MinSaleprice, purPrice,0,  MRP,
                                        // End of Rev Srijeeta mantis issue 0024515
                                     Convert.ToInt32(cmbStockUom.Value == null ? 0 : cmbStockUom.SelectedItem.Value), minLvl, reorderLvl,
                                     Convert.ToString(cmbNegativeStk.SelectedItem.Value), Convert.ToInt32(CmbTaxCodeSale.Value == null ? 0 : CmbTaxCodeSale.SelectedItem.Value),
                                     Convert.ToInt32(CmbTaxCodePur.Value == null ? 0 : CmbTaxCodePur.SelectedItem.Value), Convert.ToInt32(CmbTaxScheme.Value == null ? 0 : CmbTaxScheme.SelectedItem.Value),
                                     autoApply, Convert.ToString(fileName.Value), ProdComponent, Convert.ToString(CmbStatus.SelectedItem.Value),

                                      //chinmoy edited 22-07-2019
                                        //start
                                        //Convert.ToString(HsnLookUp.Text).Trim(),
                                     Convert.ToString(hdnHSN.Value).Trim(),
                                        //end


                                     Convert.ToInt32(AspxServiceTax.Value == null ? 0 : AspxServiceTax.Value),
                                     Convert.ToDecimal(txtPackingQty.Text.Trim()), Convert.ToDecimal(txtpacking.Text.Trim()), Convert.ToInt32(cmbPackingUomPro.Value != null ? cmbPackingUomPro.Value : 0),
                                     OverideConvertion, //Surojit 08-02-2019
                                     IsMandatory, //Surojit 11-02-2019
                                     isInstall, Convert.ToInt32(cmbBrand.Value == null ? 0 : cmbBrand.Value), isCapitalGoods, tdscode, Convert.ToString(Session["LastFinYear"]), isOldUnit,
                                     hdnSIMainAccount.Value == null ? "" : Convert.ToString(hdnSIMainAccount.Value), hdnSRMainAccount.Value == null ? "" : Convert.ToString(hdnSRMainAccount.Value),
                                     hdnPIMainAccount.Value == null ? "" : Convert.ToString(hdnPIMainAccount.Value), hdnPRMainAccount.Value == null ? "" : Convert.ToString(hdnPRMainAccount.Value), FurtheranceToBusiness, IsServiceItem, reorder_qty
                                     , maxLvl, lenght, width, Thickness, size, SUOM, series, Finish, LeadTime, Coverage, covuom, volume, volumeuom, wight, txtPro_Printname.Text, subcat);


                                    //Udf Add mode
                                    DataTable udfTable = (DataTable)Session["UdfDataOnAdd"];
                                    if (udfTable != null)
                                        Session["UdfDataOnAdd"] = reCat.insertRemarksCategoryAddMode("Prd", "ProductMaster" + Convert.ToString(insertcount), udfTable, Convert.ToString(Session["userid"]));

                                    //insertcount = oStore_MasterBL.InsertProduct(txtPro_Code.Text, txtPro_Name.Text, txtPro_Description.Text,
                                    //Convert.ToString(CmbProType.SelectedItem.Value), Convert.ToInt32(CmbProClassCode.SelectedItem.Value), txtGlobalCode.Text,
                                    //TradingLot, Convert.ToInt32(CmbTradingLotUnits.SelectedItem.Value),
                                    //Convert.ToInt32(CmbQuoteCurrency.SelectedItem.Value), QuoteLot,
                                    //Convert.ToInt32(CmbQuoteLotUnit.SelectedItem.Value), DeliveryLot,
                                    //Convert.ToInt32(CmbDeliveryLotUnit.SelectedItem.Value), ProductColor,
                                    //productSize, Convert.ToInt32(HttpContext.Current.Session["userid"]));


                                    strMsg = "Success";

                                }
                                else
                                {
                                    strMsg = "Your session is end";
                                }

                            }

                        }


                    }

                }



                if (insertcount > 0)
                {
                    gridPro.JSProperties["cpinsert"] = "Success";

                }
                else
                {
                    gridPro.JSProperties["cpinsert"] = strMsg;
                }
            }





        }



        protected void BindState(int countryID)
        {
            //CmbState.Items.Clear();

            // oGenericMethod = new GenericMethod();
            oGenericMethod = new BusinessLogicLayer.GenericMethod();

            DataTable dtCmb = new DataTable();
            dtCmb = oGenericMethod.GetDataTable("Select id,state as name From tbl_master_STATE Where countryID=" + countryID + " Order By Name");//+ " Order By state "
            AspxHelper oAspxHelper = new AspxHelper();
            if (dtCmb.Rows.Count > 0)
            {
                //CmbState.Enabled = true;
                //oAspxHelper.Bind_Combo(CmbState, dtCmb, "name", "id", 0);
            }
            else
            {
                //CmbState.Enabled = false;
            }
        }
        protected void BindCity(int stateID)
        {
            //CmbCity.Items.Clear();

            // oGenericMethod = new GenericMethod();
            oGenericMethod = new BusinessLogicLayer.GenericMethod();

            DataTable dtCmb = new DataTable();
            dtCmb = oGenericMethod.GetDataTable("Select city_id,city_name From tbl_master_city Where state_id=" + stateID + " Order By city_name");//+ " Order By state "
            AspxHelper oAspxHelper = new AspxHelper();
            if (dtCmb.Rows.Count > 0)
            {
                //CmbState.Enabled = true;
                // oAspxHelper.Bind_Combo(CmbCity, dtCmb, "city_name", "city_id", 0);
            }
            else
            {
                //CmbCity.Enabled = false;
            }
        }
        //chinmoy commented 02-08-2019
        //Start
        //[WebMethod]
        //public static bool CheckUniqueCode(string MarketsCode)
        //{
        //    bool flag = false;
        //    BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
        //    try
        //    {
        //        DataTable dtCmb = new DataTable();
        //        dtCmb = oGenericMethod.GetDataTable("SELECT * FROM [dbo].[Master_Markets] WHERE [Markets_Code] = " + "'" + MarketsCode + "'");
        //        int cnt = dtCmb.Rows.Count;
        //        if (cnt > 0)
        //        {
        //            flag = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    finally
        //    {
        //    }
        //    return flag;
        //}
        //End 02-08-2019
        protected void ASPxCallback1_Callback(object sender, CallbackEventArgsBase e)
        {
            ASPxCallbackPanel sendrPanel = sender as ASPxCallbackPanel;
            FileUpload fp = (FileUpload)sendrPanel.FindControl("FileUpload1");
            //   fp.SaveAs(Server.MapPath("~/OMS/") + fp.PostedFile..FileName);
        }
        protected void ASPxUploadControl1_FileUploadComplete(object sender, DevExpress.Web.FileUploadCompleteEventArgs e)
        {
            ASPxUploadControl uploader = sender as ASPxUploadControl;
            string fileName = uploader.FileName;
            string name = fileName.Substring(0, fileName.IndexOf('.'));
            string exten = fileName.Substring(fileName.IndexOf('.'), fileName.Length - fileName.IndexOf('.'));


            string ProductFilePath = "/CommonFolderErpCRM/ProductImages/" + name + Guid.NewGuid() + exten;
            uploader.SaveAs(Server.MapPath(ProductFilePath));
            e.CallbackData = ProductFilePath;


        }

        protected void Component_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string componet = Convert.ToString(e.Parameter);
            ASPxCallbackPanel cbPanl = source as ASPxCallbackPanel;
            ASPxGridLookup LookUp = (ASPxGridLookup)cbPanl.FindControl("GridLookup");

            string[] eachComponet = componet.Split(',');
            LookUp.GridView.Selection.UnselectAll();
            LookUp.GridView.Selection.BeginSelection();

            foreach (string val in eachComponet)
            {
                LookUp.GridView.Selection.SelectRowByKey(val);
            }
            LookUp.GridView.Selection.EndSelection();
        }

        protected int getUdfCount()
        {
            DataTable udfCount = oDBEngine.GetDataTable("select 1 from tbl_master_remarksCategory rc where cat_applicablefor='Prd' and ( exists (select * from tbl_master_udfGroup where id=rc.cat_group_id and grp_isVisible=1) or rc.cat_group_id=0)");
            return udfCount.Rows.Count;
        }

        //protected void SetHSnPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        //{

        //    string ClassId = e.Parameter;
        //    DataTable dt = oDBEngine.GetDataTable("select isnull(ProductClass_HSNCode,'') ProductClass_HSNCode  from Master_ProductClass where ProductClass_ID=" + ClassId);
        //    if (dt.Rows.Count > 0)
        //    {



        //        if (Convert.ToString(dt.Rows[0]["ProductClass_HSNCode"]) != "")
        //        {
        //            HsnLookUp.GridView.Selection.SelectRowByKey(Convert.ToString(dt.Rows[0]["ProductClass_HSNCode"]));
        //            SetHSnPanel.JSProperties["cpHsnCode"] = Convert.ToString(dt.Rows[0]["ProductClass_HSNCode"]);
        //            HsnLookUp.ClientEnabled = false;
        //        }
        //        else
        //        {
        //            HsnLookUp.GridView.Selection.SelectRowByKey("");
        //            HsnLookUp.ClientEnabled = true;
        //        }
        //    }

        //}






        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object GetMainAccount(string SearchKey)
        {
            List<MainAccount> listMainAccount = new List<MainAccount>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable MainAccount = oDBEngine.GetDataTable("Select top 10 * from (select '' MainAccount_AccountCode,'--Select--' MainAccount_Name union all  select MainAccount_AccountCode,MainAccount_Name from Master_MainAccount where MainAccount_BankCashType not in ('Cash','Bank') and " +
                                                               "MainAccount_AccountCode not in (select distinct SubAccount_MainAcReferenceID from Master_SubAccount where SubAccount_MainAcReferenceID is not null) " +
                                                               "and MainAccount_AccountCode not like 'SYSTM%' and MainAccount_AccountCode like '%" + SearchKey + "%' or MainAccount_Name like '%" + SearchKey + "%' ) TblMA  order by Len(MainAccount_AccountCode) ");


                listMainAccount = (from DataRow dr in MainAccount.Rows
                                   select new MainAccount()
                                   {
                                       MainAccount_Name = Convert.ToString(dr["MainAccount_Name"]),
                                       MainAccount_AccountCode = Convert.ToString(dr["MainAccount_AccountCode"]),

                                   }).ToList();
            }

            return listMainAccount;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object GetHSNDetails(string SearchKey)
        {
            List<HSNModel> listMainAccount = new List<HSNModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable MainAccount = oDBEngine.GetDataTable("select top 10  Code , isnull(Code,'') HSNCode,isnull(Description,'') HSNDesc from tbl_HSN_Master where Code like '%" + SearchKey + "%' or Description like '%" + SearchKey + "%'   order by Description");


                listMainAccount = (from DataRow dr in MainAccount.Rows
                                   select new HSNModel()
                                   {
                                       HSNId = Convert.ToInt32(dr["Code"]),
                                       id = Convert.ToString(dr["HSNCode"]),
                                       Name = Convert.ToString(dr["HSNDesc"]),

                                   }).ToList();
            }

            return listMainAccount;
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object GetClassDetails(string SearchKey)
        {
            List<ClassModel> listClass = new List<ClassModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable classes = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PROC_CLASSBIND_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@filtertext", SearchKey);

                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(classes);

                cmd.Dispose();
                con.Dispose();

                listClass = (from DataRow dr in classes.Rows
                             select new ClassModel()
                             {
                                 id = dr["ID"].ToString(),
                                 Name = dr["Name"].ToString(),
                             }).ToList();
            }

            return listClass;
        }
        class MainAccount
        {
            public string MainAccount_AccountCode { get; set; }
            public string MainAccount_Name { get; set; }
        }

        public class ClassModel
        {
            public string id { get; set; }
            public string Name { get; set; }
        }

        public class HSNModel
        {
            public int HSNId { get; set; }
            public string id { get; set; }
            public string Name { get; set; }
        }
        //End  02-08-2019
        #region Product Details, Warehouse

        protected void CmbProduct_Init(object sender, EventArgs e)
        {
            ASPxComboBox cityCombo = sender as ASPxComboBox;
            cityCombo.DataSource = GetProduct();
        }
        public void fillRecordFromBasket(string basketId)
        {
            PosSalesInvoiceBl pos = new PosSalesInvoiceBl();
            DataSet busketSet = pos.GetBusketDetailsById(Convert.ToInt32(basketId));

            #region SetHeader
            if (busketSet.Tables[0].Rows.Count > 0)
            {
                DataTable headerTable = busketSet.Tables[0];
                string customerId = Convert.ToString(headerTable.Rows[0]["CustomerId"]);
                txtCustName.Text = Convert.ToString(headerTable.Rows[0]["cnt_firstName"]);
                // PopulateBillingShippingByCustomerId(customerId);
                if (ddl_SalesAgent.Items.FindByValue(Convert.ToString(headerTable.Rows[0]["SalesmanInternalId"])) != null)
                {
                    ddl_SalesAgent.Value = Convert.ToString(headerTable.Rows[0]["SalesmanInternalId"]);
                }
                //  dt_PLQuote.Date = DateTime.Now;
                sessionBranch.Value = Convert.ToString(Session["userbranchID"]);
                //     ddl_Branch.SelectedValue = Convert.ToString(Session["userbranchID"]);
                ddDeliveredFrom.SelectedValue = Convert.ToString(Session["userbranchID"]);

                hdnCustomerId.Value = customerId;

                //PopulateGSTCSTVAT("2");
                ddl_VatGstCst.SelectedIndex = 0;
                txtoldUnitValue.Text = Convert.ToString(headerTable.Rows[0]["DiscountAmount"]);
                HdDiscountAmount.Value = Convert.ToString(headerTable.Rows[0]["DiscountAmount"]);
                txtunitValue.Text = Convert.ToString(headerTable.Rows[0]["DiscountAmount"]);
                #region set delivery type
                if (Convert.ToString(headerTable.Rows[0]["delivery_option_type"]) == "our")
                {
                    cmbDeliveryType.Value = "O";
                }
                else if (Convert.ToString(headerTable.Rows[0]["delivery_option_type"]) == "intimation_approx")
                {
                    cmbDeliveryType.Value = "I";
                }
                else if (Convert.ToString(headerTable.Rows[0]["delivery_option_type"]) == "self")
                {
                    cmbDeliveryType.Value = "S";
                }
                if (Convert.ToString(headerTable.Rows[0]["delivery_date"]) != "")
                    deliveryDate.Date = Convert.ToDateTime(Convert.ToString(headerTable.Rows[0]["delivery_date"]));

                #region setPosttype
                string Postype = Convert.ToString(headerTable.Rows[0]["Paymenttype"]).ToUpper();
                if (Postype == "CASH")
                    HdPosType.Value = "Cash";
                else if (Postype == "CREDIT")
                    HdPosType.Value = "Crd";
                else if (Postype == "FINANCE")
                    HdPosType.Value = "Fin";

                #endregion
                if (HdPosType.Value == "Fin")
                {
                    txtFinanceAmt.Text = Convert.ToString(headerTable.Rows[0]["Fin_Loanamt"]);
                    txtprocFee.Text = Convert.ToString(headerTable.Rows[0]["Fin_processingFee"]);
                    txtEmiDetails.Text = Convert.ToString(headerTable.Rows[0]["Fin_no_of_emi"]);
                    cmbFinancer.Value = Convert.ToString(headerTable.Rows[0]["financerInternalId"]);
                    populateExecutive(Convert.ToString(headerTable.Rows[0]["financerInternalId"]));
                    txtEmiOtherCharges.Text = Convert.ToString(headerTable.Rows[0]["Fin_Othercharge"]);
                    txtdownPayment.Text = Convert.ToString(headerTable.Rows[0]["Fin_downpayment"]);
                    txtDBD.Text = Convert.ToString(headerTable.Rows[0]["Fin_dbd_no"]);
                    txtTotDpAmt.Text = Convert.ToString(headerTable.Rows[0]["Fin_Emi_amount"]);
                }
                #region Set Financer Details


                #endregion

                #endregion

                SetBillingShippingForTab(customerId);
            }
            #endregion

            DataTable DetailGrid = busketSet.Tables[1];
            DataTable GrdpackQty = busketSet.Tables[4];

            #region SetShippingState
            string ShippingCode = "";
            if (busketSet.Tables[3].Rows.Count > 0)
            {
                ShippingCode = Convert.ToString(busketSet.Tables[3].Rows[0][0]);
            }
            #endregion
            DetailGrid = gstTaxDetails.SetTaxAmountWithGSTonDetailsTable(DetailGrid, "sProductsID", "TaxAmount", "Amount", "TotalAmount", dt_PLQuote.Date.ToString("yyyy-MM-dd"), "S", Convert.ToString(ddl_Branch.SelectedValue), ShippingCode, "I");
            CreateDataTaxTable();
            DataTable taxtable = (DataTable)Session["SI_FinalTaxRecord" + Convert.ToString(uniqueId.Value)];
            taxtable = gstTaxDetails.SetTaxTableDataWithProductSerial(DetailGrid, "SrlNo", "sProductsID", "Amount", "TaxAmount", taxtable, "S", dt_PLQuote.Date.ToString("yyyy-MM-dd"), Convert.ToString(ddl_Branch.SelectedValue), ShippingCode, "I");
            Session["SI_FinalTaxRecord" + Convert.ToString(uniqueId.Value)] = taxtable;
            DetailGrid.Columns.Remove("sProductsID");
            string totalAmount = Convert.ToString(DetailGrid.Compute("SUM(TotalAmount)", string.Empty));
            bnrlblAmountWithTaxValue.Text = totalAmount;
            bnrLblInvValue.Text = totalAmount;

            DataTable duplicatedt2 = new DataTable();
            duplicatedt2.Columns.Add("productid", typeof(Int64));
            duplicatedt2.Columns.Add("pslno", typeof(Int32));
            duplicatedt2.Columns.Add("pQuantity", typeof(Decimal));
            duplicatedt2.Columns.Add("packing", typeof(Decimal));
            duplicatedt2.Columns.Add("PackingUom", typeof(Int32));
            duplicatedt2.Columns.Add("PackingSelectUom", typeof(Int32));
            HttpContext.Current.Session["SessionPackingDetails"] = GetWaitingProductDetails(GrdpackQty);
            if (HttpContext.Current.Session["SessionPackingDetails"] != null)
            {
                List<WaitingProductQuantity> obj = new List<WaitingProductQuantity>();
                obj = (List<WaitingProductQuantity>)HttpContext.Current.Session["SessionPackingDetails"];
                foreach (var item in obj)
                {
                    duplicatedt2.Rows.Add(item.productid, item.pslno, item.pQuantity, item.packing, item.PackingUom, item.PackingSelectUom);
                }
            }
            grid.DataSource = GetWaitingProductDetails(GrdpackQty);
            grid.DataSource = GetQuotation(DetailGrid);
            grid.DataBind();
            Session["SI_QuotationDetails" + Convert.ToString(uniqueId.Value)] = DetailGrid;

            #region setComponentProduct
            if (busketSet.Tables[2].Rows.Count > 0)
            {
                isBasketContainComponent.Value = "yes";
            }
            #endregion
        }


        public void PopulateBillingShippingByCustomerId(string customerId)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PosSalesInvoice");
            proc.AddVarcharPara("@Action", 500, "GetCustomerBillingAddress");
            proc.AddVarcharPara("@customerId", 100, customerId);
            ds = proc.GetTable();

            txtsAddress1.Text = Convert.ToString(ds.Rows[0]["add_landMark"]);
            txtAddress1.Text = Convert.ToString(ds.Rows[0]["add_landMark"]);
        }
        public void SetInvoiceDetails(string strInvoiceId)
        {
            DataSet dsQuotationEditdt = PosData.GetInvoiceEditData(strInvoiceId);
            DataTable QuotationEditdt = dsQuotationEditdt.Tables[0];
            if (QuotationEditdt != null && QuotationEditdt.Rows.Count > 0)
            {
                string Branch_Id = Convert.ToString(QuotationEditdt.Rows[0]["Invoice_BranchId"]);
                string Quote_Number = Convert.ToString(QuotationEditdt.Rows[0]["Invoice_Number"]);
                string Quote_Date = Convert.ToString(QuotationEditdt.Rows[0]["Invoice_Date"]);
                string Customer_Id = Convert.ToString(QuotationEditdt.Rows[0]["Customer_Id"]);
                string Contact_Person_Id = Convert.ToString(QuotationEditdt.Rows[0]["Contact_Person_Id"]);
                string Quote_Reference = Convert.ToString(QuotationEditdt.Rows[0]["Invoice_Reference"]);
                string Currency_Id = Convert.ToString(QuotationEditdt.Rows[0]["Currency_Id"]);
                string Currency_Conversion_Rate = Convert.ToString(QuotationEditdt.Rows[0]["Currency_Conversion_Rate"]);
                string Tax_Option = Convert.ToString(QuotationEditdt.Rows[0]["Tax_Option"]);
                string Tax_Code = Convert.ToString(QuotationEditdt.Rows[0]["Tax_Code"]);
                string Quote_SalesmanId = Convert.ToString(QuotationEditdt.Rows[0]["Invoice_SalesmanId"]);
                string IsUsed = Convert.ToString(QuotationEditdt.Rows[0]["IsUsed"]);

                string CashBank_Code = Convert.ToString(QuotationEditdt.Rows[0]["CashBank_Code"]);
                string InvoiceCreatedFromDoc = Convert.ToString(QuotationEditdt.Rows[0]["InvoiceCreatedFromDoc"]);
                string InvoiceCreatedFromDoc_Ids = Convert.ToString(QuotationEditdt.Rows[0]["InvoiceCreatedFromDoc_Ids"]);
                string InvoiceCreatedFromDocDate = Convert.ToString(QuotationEditdt.Rows[0]["InvoiceCreatedFromDocDate"]);
                string DueDate = Convert.ToString(QuotationEditdt.Rows[0]["DueDate"]);
                string PosType = Convert.ToString(QuotationEditdt.Rows[0]["Pos_EntryType"]);
                string DeliveryDate = Convert.ToString(QuotationEditdt.Rows[0]["pos_deliveryDate"]);
                string Pos_Scheme = Convert.ToString(QuotationEditdt.Rows[0]["Pos_Scheme"]);
                string Advance = Convert.ToString(QuotationEditdt.Rows[0]["Adjusted"]);
                string CustomerName = Convert.ToString(QuotationEditdt.Rows[0]["custName"]);




                if (InvoiceCreatedFromDoc != "") rdl_SaleInvoice.SelectedValue = InvoiceCreatedFromDoc;
                txt_InvoiceDate.Text = InvoiceCreatedFromDocDate;


                if (!String.IsNullOrEmpty(InvoiceCreatedFromDoc_Ids))
                {
                    string[] eachQuo = InvoiceCreatedFromDoc_Ids.Split(',');
                    if (eachQuo.Length > 1)//More tha one quotation
                    {
                        BindLookUp(Customer_Id, Quote_Date, InvoiceCreatedFromDoc, Branch_Id);
                        foreach (string val in eachQuo)
                        {
                            lookup_quotation.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));
                        }
                    }
                    else if (eachQuo.Length == 1)//Single Quotation
                    {
                        BindLookUp(Customer_Id, Quote_Date, InvoiceCreatedFromDoc, Branch_Id);
                        foreach (string val in eachQuo)
                        {
                            lookup_quotation.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));
                        }
                    }
                    else//No Quotation selected
                    {
                        BindLookUp(Customer_Id, Quote_Date, InvoiceCreatedFromDoc, Branch_Id);
                    }
                }

                txt_PLQuoteNo.Text = Quote_Number;
                if (DeliveryDate.Trim() != "")
                    deliveryDate.Date = Convert.ToDateTime(DeliveryDate);

                // dt_PLQuote.Date = Convert.ToDateTime(Quote_Date);
                dt_PLQuote.Date = Convert.ToDateTime(Convert.ToDateTime(Quote_Date).ToString("yyyy-MM-dd"));
                PopulateGSTCSTVATCombo(Convert.ToDateTime(Quote_Date).ToString("yyyy-MM-dd"));
                PopulateChargeGSTCSTVATCombo(Convert.ToDateTime(Quote_Date).ToString("yyyy-MM-dd"));
                //CustomerComboBox.Value = Customer_Id;  
                hdnCustomerId.Value = Customer_Id;
                TabPage page = ASPxPageControl1.TabPages.FindByName("[B]illing/Shipping");
                page.ClientEnabled = true;

                txtCustName.Text = CustomerName;
                cmbContactPerson.SelectedItem = cmbContactPerson.Items.FindByValue(Contact_Person_Id);
                txt_Refference.Text = Quote_Reference;
                ddl_Branch.SelectedValue = Branch_Id;
                ddl_SalesAgent.Value = Quote_SalesmanId;
                ddl_Currency.SelectedValue = Currency_Id;
                txt_Rate.Value = Currency_Conversion_Rate;
                txt_Rate.Text = Currency_Conversion_Rate;
                bnrLblLessAdvanceValue.Text = string.Format("{0:0.00}", Convert.ToDouble(Advance == "" ? "0" : Advance));
                txtVehicles.Text = Convert.ToString(QuotationEditdt.Rows[0]["VECHICLE_NO"]);


                if (Tax_Option != "0") ddl_AmountAre.Value = Tax_Option;
                if (Tax_Code != "0")
                {
                    //    PopulateGSTCSTVAT("2");
                    setValueForHeaderGST(ddl_VatGstCst, Tax_Code);
                }
                else
                {
                    //   PopulateGSTCSTVAT("2");
                    ddl_VatGstCst.SelectedIndex = 0;
                }
                ddl_AmountAre.ClientEnabled = false;
                ddl_VatGstCst.ClientEnabled = false;

                if (IsUsed == "Y")
                {
                    dt_PLQuote.ClientEnabled = false;
                }
                else
                {
                    dt_PLQuote.ClientEnabled = true;
                }


                if (dsQuotationEditdt != null && dsQuotationEditdt.Tables[1] != null && dsQuotationEditdt.Tables[1].Rows.Count > 0)
                {
                    ddl_PosGst.DataSource = dsQuotationEditdt.Tables[1];
                    ddl_PosGst.ValueField = "ID";
                    ddl_PosGst.TextField = "Name";
                    ddl_PosGst.DataBind();
                }

                ddl_PosGst.Value = Convert.ToString(QuotationEditdt.Rows[0]["PosForGst"]);

                #region PosExtra Details
                if (Convert.ToString(QuotationEditdt.Rows[0]["pos_oldUnit"]) == "True")
                    cmbOldUnit.Value = "1";
                else
                    cmbOldUnit.Value = "0";
                txtunitValue.Text = Convert.ToString(QuotationEditdt.Rows[0]["pos_unitValue"]);
                bnrLblLessOldMainVal.Text = string.Format("{0:0.00}", Convert.ToDouble(QuotationEditdt.Rows[0]["pos_unitValue"]));
                txtRemarks.Text = Convert.ToString(QuotationEditdt.Rows[0]["pos_Unitremarks"]);
                txtAdvnceReceipt.Text = Convert.ToString(QuotationEditdt.Rows[0]["pos_advncRecptNo"]);
                //bnrLblLessAdvanceValue.Text = string.Format("{0:0.00}", Convert.ToDouble(QuotationEditdt.Rows[0]["pos_advncRecptNo"]==""?"0":QuotationEditdt.Rows[0]["pos_advncRecptNo"]));
                txtChallanNo.Text = Convert.ToString(QuotationEditdt.Rows[0]["Pos_ChallanNo"]);
                string challanDate = Convert.ToString(QuotationEditdt.Rows[0]["Pos_ChallanDate"]);
                if (challanDate.Trim() != "")
                    dtChallandate.Date = Convert.ToDateTime(challanDate);
                txtChallanNo.ClientEnabled = false;
                dtChallandate.ClientEnabled = false;
                cmbDeliveryType.Value = Convert.ToString(QuotationEditdt.Rows[0]["Pos_DeliveryType"]);
                //Financer
                bidfinancerByBranch(Branch_Id);
                cmbFinancer.Value = Convert.ToString(QuotationEditdt.Rows[0]["Pos_Financer"]);
                populateExecutive(Convert.ToString(QuotationEditdt.Rows[0]["Pos_Financer"]));

                bindSalesmanByBranch(Branch_Id);
                ddl_SalesAgent.Value = Quote_SalesmanId;

                cmbExecName.Value = Convert.ToString(QuotationEditdt.Rows[0]["Pos_Executive"]);
                txtEmiDetails.Text = Convert.ToString(QuotationEditdt.Rows[0]["Pos_EMIDetails"]);
                txtFinanceAmt.Text = Convert.ToString(QuotationEditdt.Rows[0]["Pos_FinanceAmt"]);
                txtDBD.Text = Convert.ToString(QuotationEditdt.Rows[0]["Pos_DBD"]);
                txtDbdPercen.Text = Convert.ToString(QuotationEditdt.Rows[0]["Pos_DBDPercent"]);
                txtprocFee.Text = Convert.ToString(QuotationEditdt.Rows[0]["Pos_ProcFee"]);
                txtSfCode.Text = Convert.ToString(QuotationEditdt.Rows[0]["Pos_SFCode"]);
                HdPosType.Value = PosType;

                txtEmiOtherCharges.Text = Convert.ToString(QuotationEditdt.Rows[0]["Pos_EmiOther_charges"]);
                txtdownPayment.Text = Convert.ToString(QuotationEditdt.Rows[0]["Pos_downPayment"]);
                txtTotDpAmt.Text = Convert.ToString(QuotationEditdt.Rows[0]["Pos_total_dp_Amt"]);
                txtScheme.Text = Convert.ToString(QuotationEditdt.Rows[0]["Pos_Scheme"]);
                txtfinChallanNo.Text = Convert.ToString(QuotationEditdt.Rows[0]["Pos_FinChallanNo"]);
                if (Convert.ToString(QuotationEditdt.Rows[0]["Pos_FinChallanDate"]).Trim() != "")
                    finChallandate.Date = Convert.ToDateTime(Convert.ToString(QuotationEditdt.Rows[0]["Pos_FinChallanDate"]));
                #endregion

            }
        }
        protected void BindLookUp(string Customer, string OrderDate, string ComponentType, string BranchID)
        {
            string Action = "";
            if (ComponentType == "QO")
            {
                Action = "GetQuotation";
            }
            else if (ComponentType == "SO")
            {
                Action = "GetOrder";
            }
            else if (ComponentType == "SC")
            {
                Action = "GetChallan";
            }

            string strInvoiceID = InvoiceId;// Convert.ToString(Session["SI_InvoiceID"]);
            string FinYear = Convert.ToString(Session["LastFinYear"]);
            DataTable ComponentTable = objSalesInvoiceBL.GetComponent(Customer, OrderDate, ComponentType, FinYear, BranchID, Action, strInvoiceID);
            lookup_quotation.GridView.Selection.CancelSelection();

            lookup_quotation.GridView.Selection.CancelSelection();
            lookup_quotation.DataSource = ComponentTable;
            lookup_quotation.DataBind();

            Session["SI_ComponentData" + Convert.ToString(uniqueId.Value)] = ComponentTable;
        }

        #region Page Classes
        public class Product
        {
            public string ProductID { get; set; }
            public string ProductName { get; set; }
        }
        public class Taxes
        {
            public string TaxID { get; set; }
            public string TaxName { get; set; }
            public string Percentage { get; set; }
            public string Amount { get; set; }
            public decimal calCulatedOn { get; set; }
        }
        public class Quotation
        {
            public string SrlNo { get; set; }
            public string QuotationID { get; set; }
            public string ProductID { get; set; }
            public string Description { get; set; }
            public string Quantity { get; set; }
            public string UOM { get; set; }
            public string Warehouse { get; set; }
            public string StockQuantity { get; set; }
            public string StockUOM { get; set; }
            public string SalePrice { get; set; }
            public string Discount { get; set; }
            public string Old_Amount { get; set; }
            public string Amount { get; set; }
            public string TaxAmount { get; set; }
            public string TotalAmount { get; set; }
            public string ProductName { get; set; }
            public string ComponentID { get; set; }
            public string ComponentNumber { get; set; }
            public string TotalQty { get; set; }
            public string BalanceQty { get; set; }
            public string IsComponentProduct { get; set; }
            public string IsLinkedProduct { get; set; }
        }
        #endregion

        #region Product Details



        #region Code Checked and Modified

        public IEnumerable GetProduct()
        {
            List<Product> ProductList = new List<Product>();

            //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();

            DataTable DT = GetProductData();
            //DataTable DT = GetProductData().Tables[0];
            for (int i = 0; i < DT.Rows.Count; i++)
            {
                Product Products = new Product();
                Products.ProductID = Convert.ToString(DT.Rows[i]["Products_ID"]);
                Products.ProductName = Convert.ToString(DT.Rows[i]["Products_Name"]);
                ProductList.Add(Products);
            }

            return ProductList;
        }


        public class WaitingProductQuantity
        {
            public Int64 productid { get; set; }
            public Int32 pslno { get; set; }
            public Decimal pQuantity { get; set; }
            public Decimal packing { get; set; }

            public Int32 PackingUom { get; set; }
            public Int32 PackingSelectUom { get; set; }
        }
        public IEnumerable GetWaitingProductDetails(DataTable Quotationdt)
        {
            List<WaitingProductQuantity> QuotationList = new List<WaitingProductQuantity>();

            if (Quotationdt != null && Quotationdt.Rows.Count > 0)
            {
                for (int i = 0; i < Quotationdt.Rows.Count; i++)
                {
                    WaitingProductQuantity Quotations = new WaitingProductQuantity();

                    Quotations.productid = Convert.ToInt64(Quotationdt.Rows[i]["productid"]);
                    Quotations.pslno = Convert.ToInt32(Quotationdt.Rows[i]["pslno"]);
                    Quotations.pQuantity = Convert.ToDecimal(Quotationdt.Rows[i]["pQuantity"]);
                    Quotations.packing = Convert.ToDecimal(Quotationdt.Rows[i]["packing"]);
                    Quotations.PackingUom = Convert.ToInt32(Quotationdt.Rows[i]["PackingUom"]);
                    Quotations.PackingSelectUom = Convert.ToInt32(Quotationdt.Rows[i]["PackingSelectUom"]);

                    QuotationList.Add(Quotations);
                }
            }

            return QuotationList;
        }


        //public IEnumerable GetFilterProduct(string filter)
        //{
        //    List<Product> ProductList = new List<Product>();
        //    BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        //    DataTable DT = GetFilterProductData(filter);
        //    //DataTable DT = GetProductData().Tables[0];
        //    for (int i = 0; i < DT.Rows.Count; i++)
        //    {
        //        Product Products = new Product();
        //        Products.ProductID = Convert.ToString(DT.Rows[i]["Products_ID"]);
        //        Products.ProductName = Convert.ToString(DT.Rows[i]["Products_Name"]);
        //        ProductList.Add(Products);
        //    }

        //    return ProductList;
        //}
        public DataTable GetProductData()
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 500, "ProductDetails");
            ds = proc.GetTable();
            return ds;
        }
        //public DataTable GetFilterProductData(string filter)
        //{
        //    DataTable ds = new DataTable();
        //    ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
        //    proc.AddVarcharPara("@Action", 500, "ProductFilterDetails");
        //    proc.AddVarcharPara("@Filter", 2000, filter);
        //    ds = proc.GetTable();
        //    return ds;
        //}

        #endregion Code Checked and Modified


        //public DataSet GetProductData()
        //{
        //    DataSet ds = new DataSet();
        //    ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
        //    proc.AddVarcharPara("@Action", 500, "ProductDetails");
        //    ds = proc.GetDataSet();
        //    return ds;
        //}
        public DataTable GetWarehouseData()
        {

            MasterSettings masterBl = new MasterSettings();
            string multiwarehouse = Convert.ToString(masterBl.GetSettings("IaMultilevelWarehouse"));

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 500, "GetWareHouseDtlByProductID");
            proc.AddVarcharPara("@QuotationID", 500, InvoiceId);
            proc.AddVarcharPara("@ProductID", 500, Convert.ToString(hdfProductID.Value));
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            proc.AddVarcharPara("@branchId", 500, Convert.ToString(ddl_Branch.SelectedValue));
            proc.AddVarcharPara("@companyId", 500, Convert.ToString(Session["LastCompany"]));
            proc.AddVarcharPara("@Multiwarehouse", 500, Convert.ToString(multiwarehouse));
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetProductDetailsData(string ProductID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 500, "ProductDetailsSearch");
            proc.AddVarcharPara("@ProductID", 500, ProductID);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetBatchData(string WarehouseID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 500, "GetBatchByProductIDWarehouse");
            proc.AddVarcharPara("@QuotationID", 500, InvoiceId);
            proc.AddVarcharPara("@ProductID", 500, Convert.ToString(hdfProductID.Value));
            proc.AddVarcharPara("@WarehouseID", 500, WarehouseID);
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            proc.AddVarcharPara("@branchId", 500, Convert.ToString(ddl_Branch.SelectedValue));
            proc.AddVarcharPara("@companyId", 500, Convert.ToString(Session["LastCompany"]));
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetSerialata(string WarehouseID, string BatchID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 500, "GetSerialByProductIDWarehouseBatch");
            proc.AddVarcharPara("@QuotationID", 500, InvoiceId);
            proc.AddVarcharPara("@ProductID", 500, Convert.ToString(hdfProductID.Value));
            proc.AddVarcharPara("@BatchID", 500, BatchID);
            proc.AddVarcharPara("@WarehouseID", 500, WarehouseID);
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            proc.AddVarcharPara("@branchId", 500, Convert.ToString(ddl_Branch.SelectedValue));
            proc.AddVarcharPara("@companyId", 500, Convert.ToString(Session["LastCompany"]));
            dt = proc.GetTable();
            return dt;
        }
        public IEnumerable GetQuotation(DataTable Quotationdt)
        {
            List<Quotation> QuotationList = new List<Quotation>();

            if (Quotationdt != null && Quotationdt.Rows.Count > 0)
            {
                for (int i = 0; i < Quotationdt.Rows.Count; i++)
                {
                    Quotation Quotations = new Quotation();

                    Quotations.SrlNo = Convert.ToString(Quotationdt.Rows[i]["SrlNo"]);
                    Quotations.QuotationID = Convert.ToString(Quotationdt.Rows[i]["QuotationID"]);
                    Quotations.ProductID = Convert.ToString(Quotationdt.Rows[i]["ProductID"]);
                    Quotations.Description = Convert.ToString(Quotationdt.Rows[i]["Description"]);
                    string quantity = Convert.ToString(Quotationdt.Rows[i]["Quantity"]);

                    //Mantis issue number 20058
                    //if (quantity.Contains('.'))
                    //{
                    //    quantity = quantity.Substring(0, quantity.IndexOf('.'));
                    //}                    
                    Quotations.Quantity = quantity;
                    Quotations.UOM = Convert.ToString(Quotationdt.Rows[i]["UOM"]);
                    Quotations.Warehouse = "";
                    Quotations.StockQuantity = Convert.ToString(Quotationdt.Rows[i]["StockQuantity"]);
                    Quotations.StockUOM = Convert.ToString(Quotationdt.Rows[i]["StockUOM"]);
                    Quotations.SalePrice = Convert.ToString(Quotationdt.Rows[i]["SalePrice"]);
                    Quotations.Discount = Convert.ToString(Quotationdt.Rows[i]["Discount"]);


                    Quotations.Old_Amount = Convert.ToString(Quotationdt.Rows[i]["Old_Amount"]);

                    Quotations.Amount = Convert.ToString(Quotationdt.Rows[i]["TotalAmount"]);

                    Quotations.TaxAmount = Convert.ToString(Quotationdt.Rows[i]["TaxAmount"]);



                    //Quotations.TotalAmount = Convert.ToString(Quotationdt.Rows[i]["TotalAmount"]);
                    Quotations.TotalAmount = Convert.ToString(Quotationdt.Rows[i]["Amount"]);


                    Quotations.ProductName = Convert.ToString(Quotationdt.Rows[i]["ProductName"]);
                    Quotations.ComponentID = Convert.ToString(Quotationdt.Rows[i]["ComponentID"]);
                    Quotations.ComponentNumber = Convert.ToString(Quotationdt.Rows[i]["ComponentNumber"]);
                    Quotations.TotalQty = Convert.ToString(Quotationdt.Rows[i]["TotalQty"]);
                    Quotations.BalanceQty = Convert.ToString(Quotationdt.Rows[i]["BalanceQty"]);
                    Quotations.IsComponentProduct = Convert.ToString(Quotationdt.Rows[i]["IsComponentProduct"]);
                    Quotations.IsLinkedProduct = Convert.ToString(Quotationdt.Rows[i]["IsLinkedProduct"]);


                    TotalTaggingAmount += Convert.ToDecimal(Convert.ToString(Quotationdt.Rows[i]["TotalAmount"]));

                    QuotationList.Add(Quotations);
                }
            }

            return QuotationList;
        }
        protected void grid_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (e.Column.FieldName == "Description")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "UOM")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "StkUOM")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "TaxAmount")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "ComponentNumber")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "Amount")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "TotalAmount")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "SrlNo")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "ProductName")
            {
                e.Editor.Enabled = true;
            }
            else
            {
                e.Editor.ReadOnly = false;
            }
        }
        protected void grid_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            SendSmsBL ObjSms = new SendSmsBL();
            string msgBody = ObjSms.getMsgbody(1);
            msgBody = msgBody.Replace("@Customer_name", txtCustName.Text.ToString().Trim());


            PaymentDetails.GetPaymentTable();

            DataTable Quotationdt = new DataTable();
            string IsDeleteFrom = Convert.ToString(hdfIsDelete.Value);

            string validate = "";
            string Challanvalidate = "";
            grid.JSProperties["cpQuotationNo"] = "";
            grid.JSProperties["cpSaveSuccessOrFail"] = "";

            if (Session["SI_QuotationDetails" + Convert.ToString(uniqueId.Value)] != null)
            {
                Quotationdt = (DataTable)Session["SI_QuotationDetails" + Convert.ToString(uniqueId.Value)];
            }
            else
            {
                Quotationdt.Columns.Add("SrlNo", typeof(string));
                Quotationdt.Columns.Add("QuotationID", typeof(string));
                Quotationdt.Columns.Add("ProductID", typeof(string));
                Quotationdt.Columns.Add("Description", typeof(string));
                Quotationdt.Columns.Add("Quantity", typeof(string));
                Quotationdt.Columns.Add("UOM", typeof(string));
                Quotationdt.Columns.Add("Warehouse", typeof(string));
                Quotationdt.Columns.Add("StockQuantity", typeof(string));
                Quotationdt.Columns.Add("StockUOM", typeof(string));
                Quotationdt.Columns.Add("SalePrice", typeof(string));
                Quotationdt.Columns.Add("Discount", typeof(string));
                Quotationdt.Columns.Add("Old_Amount", typeof(string));
                Quotationdt.Columns.Add("Amount", typeof(string));
                Quotationdt.Columns.Add("TaxAmount", typeof(string));
                Quotationdt.Columns.Add("TotalAmount", typeof(string));
                Quotationdt.Columns.Add("Status", typeof(string));
                Quotationdt.Columns.Add("ProductName", typeof(string));
                Quotationdt.Columns.Add("ComponentID", typeof(string));
                Quotationdt.Columns.Add("ComponentNumber", typeof(string));
                Quotationdt.Columns.Add("TotalQty", typeof(string));
                Quotationdt.Columns.Add("BalanceQty", typeof(string));
                Quotationdt.Columns.Add("IsComponentProduct", typeof(string));
                Quotationdt.Columns.Add("IsLinkedProduct", typeof(string));
            }

            foreach (var args in e.InsertValues)
            {
                string ProductDetails = Convert.ToString(args.NewValues["ProductID"]);

                if (ProductDetails != "" && ProductDetails != "0")
                {
                    string SrlNo = Convert.ToString(args.NewValues["SrlNo"]);
                    string[] ProductDetailsList = ProductDetails.Split(new string[] { "||@||" }, StringSplitOptions.None);
                    string ProductID = ProductDetailsList[0];

                    string ProductName = Convert.ToString(args.NewValues["ProductName"]);
                    string Description = Convert.ToString(args.NewValues["Description"]);
                    string Quantity = Convert.ToString(args.NewValues["Quantity"]);
                    string UOM = Convert.ToString(args.NewValues["UOM"]);
                    string Warehouse = Convert.ToString(args.NewValues["Warehouse"]);

                    decimal strMultiplier = Convert.ToDecimal(ProductDetailsList[7]);
                    string StockQuantity = Convert.ToString(Convert.ToDecimal(Quantity) * strMultiplier);
                    string StockUOM = Convert.ToString(ProductDetailsList[4]);
                    //string StockQuantity = Convert.ToString(args.NewValues["StockQuantity"]);
                    //string StockUOM = Convert.ToString(args.NewValues["StockUOM"]);

                    string SalePrice = Convert.ToString(args.NewValues["SalePrice"]);
                    string Discount = (Convert.ToString(args.NewValues["Discount"]) != "") ? Convert.ToString(args.NewValues["Discount"]) : "0";
                    string Amount = (Convert.ToString(args.NewValues["Amount"]) != "") ? Convert.ToString(args.NewValues["Amount"]) : "0";
                    string Old_Amount = (Convert.ToString(args.NewValues["Amount"]) != "") ? Convert.ToString(args.NewValues["Old_Amount"]) : "0";

                    string TaxAmount = (Convert.ToString(args.NewValues["TaxAmount"]) != "") ? Convert.ToString(args.NewValues["TaxAmount"]) : "0";
                    string TotalAmount = (Convert.ToString(args.NewValues["TotalAmount"]) != "") ? Convert.ToString(args.NewValues["TotalAmount"]) : "0";

                    string ComponentID = (Convert.ToString(args.NewValues["ComponentID"]) != "") ? Convert.ToString(args.NewValues["ComponentID"]) : "0";
                    string ComponentName = (Convert.ToString(args.NewValues["ComponentName"]) != "") ? Convert.ToString(args.NewValues["ComponentName"]) : "";
                    string TotalQty = (Convert.ToString(args.NewValues["TotalQty"]) != "") ? Convert.ToString(args.NewValues["TotalQty"]) : "0";
                    string BalanceQty = (Convert.ToString(args.NewValues["BalanceQty"]) != "") ? Convert.ToString(args.NewValues["BalanceQty"]) : "0";
                    string IsComponentProduct = (Convert.ToString(args.NewValues["IsComponentProduct"]) != "") ? Convert.ToString(args.NewValues["IsComponentProduct"]) : "N";
                    string IsLinkedProduct = (Convert.ToString(args.NewValues["IsLinkedProduct"]) != "") ? Convert.ToString(args.NewValues["IsLinkedProduct"]) : "N";



                    Quotationdt.Rows.Add(SrlNo, "0", ProductDetails, Description, Quantity, UOM, Warehouse, StockQuantity, StockUOM, SalePrice, Discount,Old_Amount, TotalAmount, TaxAmount, Amount, "I", ProductName, ComponentID, ComponentName, TotalQty, BalanceQty, "", IsLinkedProduct);

                    if (IsComponentProduct == "Y")
                    {
                        DataTable ComponentTable = PosData.GetLinkedProductList("LinkedProduct", ProductID);
                        foreach (DataRow drr in ComponentTable.Rows)
                        {
                            string sProductsID = Convert.ToString(drr["sProductsID"]);
                            string Products_Description = Convert.ToString(drr["Products_Description"]);
                            string Sales_UOM_Name = Convert.ToString(drr["Sales_UOM_Name"]);
                            string Conversion_Multiplier = Convert.ToString(drr["Conversion_Multiplier"]);
                            string Stk_UOM_Name = Convert.ToString(drr["Stk_UOM_Name"]);
                            string Product_SalePrice = Convert.ToString(drr["Product_SalePrice"]);
                            string Products_Name = Convert.ToString(drr["Products_Name"]);
                            string StkQty = Convert.ToString(Convert.ToDecimal(Quantity) * Convert.ToDecimal(Conversion_Multiplier));

                            Quotationdt.Rows.Add(Convert.ToString(Convert.ToInt32(SrlNo) + 1), "0", sProductsID, Products_Description, Quantity, Sales_UOM_Name, "", StkQty, Stk_UOM_Name, Product_SalePrice, "0.0","0.0", "0.0", "0.0", "0.0", "I", Products_Name, ComponentID, ComponentName, "0.0", "0.0", "", "Y");
                        }
                    }

                }
            }

            foreach (var args in e.UpdateValues)
            {
                string SrlNo = Convert.ToString(args.NewValues["SrlNo"]);
                string QuotationID = Convert.ToString(args.Keys["QuotationID"]);
                string ProductDetails = Convert.ToString(args.NewValues["ProductID"]);

                bool isDeleted = false;
                foreach (var arg in e.DeleteValues)
                {
                    string DeleteID = Convert.ToString(arg.Keys["QuotationID"]);
                    if (DeleteID == QuotationID)
                    {
                        isDeleted = true;
                        break;
                    }
                }

                if (isDeleted == false)
                {
                    if (ProductDetails != "" && ProductDetails != "0")
                    {
                        string[] ProductDetailsList = ProductDetails.Split(new string[] { "||@||" }, StringSplitOptions.None);
                        string ProductID = Convert.ToString(ProductDetailsList[0]);

                        string ProductName = Convert.ToString(args.NewValues["ProductName"]);
                        string Description = Convert.ToString(args.NewValues["Description"]);
                        string Quantity = Convert.ToString(args.NewValues["Quantity"]);
                        string UOM = Convert.ToString(args.NewValues["UOM"]);
                        string Warehouse = Convert.ToString(args.NewValues["Warehouse"]);

                        decimal strMultiplier = Convert.ToDecimal(ProductDetailsList[7]);
                        string StockQuantity = Convert.ToString(Convert.ToDecimal(Quantity) * strMultiplier);
                        string StockUOM = Convert.ToString(ProductDetailsList[4]);
                        //string StockQuantity = Convert.ToString(args.NewValues["StockQuantity"]);
                        //string StockUOM = Convert.ToString(args.NewValues["StockUOM"]);

                        string SalePrice = Convert.ToString(args.NewValues["SalePrice"]);
                        string Discount = (Convert.ToString(args.NewValues["Discount"]) != "") ? Convert.ToString(args.NewValues["Discount"]) : "0";
                        string Amount = (Convert.ToString(args.NewValues["Amount"]) != "") ? Convert.ToString(args.NewValues["Amount"]) : "0";
                        string Old_Amount = (Convert.ToString(args.NewValues["Old_Amount"]) != "") ? Convert.ToString(args.NewValues["Old_Amount"]) : "0";
                        string TaxAmount = (Convert.ToString(args.NewValues["TaxAmount"]) != "") ? Convert.ToString(args.NewValues["TaxAmount"]) : "0";
                        string TotalAmount = (Convert.ToString(args.NewValues["TotalAmount"]) != "") ? Convert.ToString(args.NewValues["TotalAmount"]) : "0";

                        string ComponentID = (Convert.ToString(args.NewValues["ComponentID"]) != "") ? Convert.ToString(args.NewValues["ComponentID"]) : "0";
                        string ComponentName = (Convert.ToString(args.NewValues["ComponentName"]) != "") ? Convert.ToString(args.NewValues["ComponentName"]) : "";
                        string TotalQty = (Convert.ToString(args.NewValues["TotalQty"]) != "") ? Convert.ToString(args.NewValues["TotalQty"]) : "0";
                        string BalanceQty = (Convert.ToString(args.NewValues["BalanceQty"]) != "") ? Convert.ToString(args.NewValues["BalanceQty"]) : "0";
                        string IsComponentProduct = (Convert.ToString(args.NewValues["IsComponentProduct"]) != "") ? Convert.ToString(args.NewValues["IsComponentProduct"]) : "N";
                        string IsLinkedProduct = (Convert.ToString(args.NewValues["IsLinkedProduct"]) != "") ? Convert.ToString(args.NewValues["IsLinkedProduct"]) : "N";

                        bool Isexists = false;
                        foreach (DataRow drr in Quotationdt.Rows)
                        {
                            string OldQuotationID = Convert.ToString(drr["QuotationID"]);

                            if (OldQuotationID == QuotationID)
                            {
                                Isexists = true;

                                drr["ProductID"] = ProductDetails;
                                drr["Description"] = Description;
                                drr["Quantity"] = Quantity;
                                drr["UOM"] = UOM;
                                drr["Warehouse"] = Warehouse;
                                drr["StockQuantity"] = StockQuantity;
                                drr["StockUOM"] = StockUOM;
                                drr["SalePrice"] = SalePrice;
                                drr["Discount"] = Discount;
                                drr["Amount"] = TotalAmount;
                                drr["Old_Amount"] = Old_Amount;
                                drr["TaxAmount"] = TaxAmount;
                                drr["TotalAmount"] = Amount;
                                drr["Status"] = "U";
                                drr["ProductName"] = ProductName;
                                drr["ComponentID"] = ComponentID;
                                drr["ComponentNumber"] = ComponentName;
                                drr["TotalQty"] = TotalQty;
                                drr["BalanceQty"] = BalanceQty;
                                drr["IsComponentProduct"] = IsComponentProduct;
                                drr["IsLinkedProduct"] = IsLinkedProduct;

                                break;
                            }
                        }

                        if (Isexists == false)
                        {
                            Quotationdt.Rows.Add(SrlNo, QuotationID, ProductDetails, Description, Quantity, UOM, Warehouse, StockQuantity, StockUOM, SalePrice, Discount, TotalAmount, TaxAmount, Old_Amount, Amount, "U", ProductName, ComponentID, ComponentName, TotalQty, BalanceQty, IsComponentProduct, IsLinkedProduct);
                        }

                        if (IsComponentProduct == "Y")
                        {
                            DataTable ComponentTable = PosData.GetLinkedProductList("LinkedProduct", ProductID);
                            foreach (DataRow drr in ComponentTable.Rows)
                            {
                                string sProductsID = Convert.ToString(drr["sProductsID"]);
                                string Products_Description = Convert.ToString(drr["Products_Description"]);
                                string Sales_UOM_Name = Convert.ToString(drr["Sales_UOM_Name"]);
                                string Conversion_Multiplier = Convert.ToString(drr["Conversion_Multiplier"]);
                                string Stk_UOM_Name = Convert.ToString(drr["Stk_UOM_Name"]);
                                string Product_SalePrice = Convert.ToString(drr["Product_SalePrice"]);
                                string Products_Name = Convert.ToString(drr["Products_Name"]);
                                string StkQty = Convert.ToString(Convert.ToDecimal(Quantity) * Convert.ToDecimal(Conversion_Multiplier));

                                Quotationdt.Rows.Add(Convert.ToString(Convert.ToInt32(SrlNo) + 1), "0", sProductsID, Products_Description, Quantity, Sales_UOM_Name, "", StkQty, Stk_UOM_Name, Product_SalePrice, "0.0","0.00", "0.0", "0.0", "0.0", "I", Products_Name, ComponentID, ComponentName, "0.0", "0.0", "", "Y");
                            }
                        }

                    }
                }
            }

            foreach (var args in e.DeleteValues)
            {
                string QuotationID = Convert.ToString(args.Keys["QuotationID"]);
                string SrlNo = "";

                for (int i = Quotationdt.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = Quotationdt.Rows[i];
                    string delQuotationID = Convert.ToString(dr["QuotationID"]);

                    if (delQuotationID == QuotationID)
                    {
                        SrlNo = Convert.ToString(dr["SrlNo"]);
                        dr.Delete();
                    }
                }
                Quotationdt.AcceptChanges();

                DeleteWarehouse(SrlNo);
                DeleteTaxDetails(SrlNo);

                if (QuotationID.Contains("~") != true)
                {
                    Quotationdt.Rows.Add("0", QuotationID, "0", "", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "I", "", "0", "", "0", "0", "", "");
                }
            }

            ///////////////////////

            string strDeleteSrlNo = Convert.ToString(hdnDeleteSrlNo.Value);
            if (strDeleteSrlNo != "" && strDeleteSrlNo != "0")
            {
                DeleteWarehouse(strDeleteSrlNo);
                DeleteTaxDetails(strDeleteSrlNo);

                hdnDeleteSrlNo.Value = "";
            }

            int j = 1;
            foreach (DataRow dr in Quotationdt.Rows)
            {
                string Status = Convert.ToString(dr["Status"]);
                string oldSrlNo = Convert.ToString(dr["SrlNo"]);
                string newSrlNo = j.ToString();

                dr["SrlNo"] = j.ToString();

                UpdateWarehouse(oldSrlNo, newSrlNo);
                UpdateTaxDetails(oldSrlNo, newSrlNo);

                if (Status != "D")
                {
                    if (Status == "I")
                    {
                        string strID = Convert.ToString("Q~" + j);
                        dr["QuotationID"] = strID;
                    }
                    j++;
                }
            }
            Quotationdt.AcceptChanges();

            //if (Quotationdt.Rows.Count == 0)
            //{
            //    Quotationdt.Rows.Add("1", "", "", "", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "I", "", "0", "", "0", "0", "", "");
            //}

            Session["SI_QuotationDetails" + Convert.ToString(uniqueId.Value)] = Quotationdt;

            //////////////////////


            if (IsDeleteFrom != "D" && IsDeleteFrom != "C")
            {
                string InvoiceComponentDate = "", InvoiceComponent = "";

                //string ActionType = Convert.ToString(Session["SI_ActionType"]);
                string ActionType = "";
                string MainInvoiceID = "";

                if (Request.QueryString["Key"] == "ADD")
                    ActionType = "Add";
                else
                {
                    ActionType = "Edit";
                    MainInvoiceID = Request.QueryString["Key"];
                }

                //MainInvoiceID = Convert.ToString(Session["SI_InvoiceID"]);

                string strSchemeType = Convert.ToString(ddl_numberingScheme.SelectedValue);
                string strInvoiceNo = Convert.ToString(txt_PLQuoteNo.Text);
                string strInvoiceDate = Convert.ToString(dt_PLQuote.Date);
                string strBranch = Convert.ToString(ddl_Branch.SelectedValue);

                string strCustomer = Convert.ToString(hdfLookupCustomer.Value);
                string strContactName = Convert.ToString(cmbContactPerson.Value);
                string Reference = Convert.ToString(txt_Refference.Text);
                string strAgents = Convert.ToString(ddl_SalesAgent.Value);

                string strComponenyType = Convert.ToString(rdl_SaleInvoice.SelectedValue);
                List<object> QuoList = lookup_quotation.GridView.GetSelectedFieldValues("ComponentID");
                foreach (object Quo in QuoList)
                {
                    InvoiceComponent += "," + Quo;
                }
                InvoiceComponent = InvoiceComponent.TrimStart(',');
                string[] eachInvoice = InvoiceComponent.Split(',');
                if (eachInvoice.Length == 1)
                {
                    InvoiceComponentDate = Convert.ToString(txt_InvoiceDate.Text);
                }
                string strCashBank = "";
                string strDueDate = null;
                #region Rajdip
                //Rev Rajdip
                if (dt_SaleInvoiceDue.Text != "")
                {
                    strDueDate = Convert.ToString(dt_SaleInvoiceDue.Date);
                }
                //End Rev Rajdip
                #endregion rajdip

                string strCurrency = Convert.ToString(ddl_Currency.SelectedValue);
                string strRate = Convert.ToString(txt_Rate.Value);
                string strTaxType = Convert.ToString(ddl_AmountAre.Value);
                string strTaxCode = Convert.ToString(ddl_VatGstCst.Value).Split('~')[0];

                string CompID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string currency = Convert.ToString(HttpContext.Current.Session["LocalCurrency"]);
                string[] ActCurrency = currency.Split('~');
                int BaseCurrencyId = Convert.ToInt32(ActCurrency[0]);
                int ConvertedCurrencyId = Convert.ToInt32(strCurrency);
                bool oldUnit = Convert.ToBoolean(Convert.ToInt32(cmbOldUnit.Value));

                //Financer 
                string financerId = cmbFinancer.Value == null ? "" : Convert.ToString(cmbFinancer.Value);
                string ExecutiveId = cmbExecName.Value == null ? "" : Convert.ToString(cmbExecName.Value);
                string EMIDetails = txtEmiDetails.Text;
                decimal financeAmount = Convert.ToDecimal(txtFinanceAmt.Text);
                decimal dbd = Convert.ToDecimal(txtDBD.Text);
                decimal dbdPercent = Convert.ToDecimal(txtDbdPercen.Text);
                decimal ProcFee = Convert.ToDecimal(txtprocFee.Text);
                string sfCode = txtSfCode.Text;
                string strDeliveryDate = Convert.ToString(deliveryDate.Date);
                string placeofsupply = "S";
                if (ddl_PosGst.Value != null)
                {
                    placeofsupply = ddl_PosGst.Value.ToString();
                }


                DataTable paymentDetails = PaymentDetails.GetPaymentTable();

                DataTable tempQuotation = Quotationdt.Copy();
                foreach (DataRow dr in tempQuotation.Rows)
                {
                    string Status = Convert.ToString(dr["Status"]);

                    if (Status == "I")
                    {
                        dr["QuotationID"] = "0";

                        string[] list = Convert.ToString(dr["ProductID"]).Split(new string[] { "||@||" }, StringSplitOptions.None);
                        dr["ProductID"] = Convert.ToString(list[0]);
                        dr["UOM"] = Convert.ToString(list[3]);
                        dr["StockUOM"] = Convert.ToString(list[5]);
                    }
                    else if (Status == "U" || Status == "")
                    {
                        if (Convert.ToString(dr["QuotationID"]).Contains("~") == true)
                        {
                            dr["QuotationID"] = 0;
                        }

                        string[] list = Convert.ToString(dr["ProductID"]).Split(new string[] { "||@||" }, StringSplitOptions.None);
                        dr["ProductID"] = Convert.ToString(list[0]);
                        dr["UOM"] = Convert.ToString(list[3]);
                        dr["StockUOM"] = Convert.ToString(list[5]);
                    }
                }
                tempQuotation.AcceptChanges();

                //DataTable TaxDetailTable = getAllTaxDetails(e);

                DataTable TaxDetailTable = new DataTable();
                if (Session["SI_FinalTaxRecord" + Convert.ToString(uniqueId.Value)] != null)
                {
                    TaxDetailTable = (DataTable)Session["SI_FinalTaxRecord" + Convert.ToString(uniqueId.Value)];
                }

                // DataTable of Warehouse

                DataTable tempWarehousedt = new DataTable();
                if (Session["SI_WarehouseData" + Convert.ToString(uniqueId.Value)] != null)
                {
                    DataTable Warehousedt = (DataTable)Session["SI_WarehouseData" + Convert.ToString(uniqueId.Value)];
                    tempWarehousedt = Warehousedt.DefaultView.ToTable(false, "Product_SrlNo", "LoopID", "WarehouseID", "TotalQuantity", "BatchID", "SerialID", "AltQty");
                }
                else
                {
                    tempWarehousedt.Columns.Add("Product_SrlNo", typeof(string));
                    tempWarehousedt.Columns.Add("SrlNo", typeof(string));
                    tempWarehousedt.Columns.Add("WarehouseID", typeof(string));
                    tempWarehousedt.Columns.Add("TotalQuantity", typeof(string));
                    tempWarehousedt.Columns.Add("BatchID", typeof(string));
                    tempWarehousedt.Columns.Add("SerialID", typeof(string));
                    tempWarehousedt.Columns.Add("AltQty", typeof(string));
                }

                // End

                // DataTable Of Quotation Tax Details 

                DataTable TaxDetailsdt = new DataTable();
                if (Session["SI_TaxDetails" + Convert.ToString(uniqueId.Value)] != null)
                {
                    TaxDetailsdt = (DataTable)Session["SI_TaxDetails" + Convert.ToString(uniqueId.Value)];
                }
                else
                {
                    TaxDetailsdt.Columns.Add("Taxes_ID", typeof(string));
                    TaxDetailsdt.Columns.Add("Taxes_Name", typeof(string));
                    TaxDetailsdt.Columns.Add("Percentage", typeof(string));
                    TaxDetailsdt.Columns.Add("Amount", typeof(string));
                    TaxDetailsdt.Columns.Add("AltTax_Code", typeof(string));
                }

                DataTable tempTaxDetailsdt = new DataTable();
                tempTaxDetailsdt = TaxDetailsdt.DefaultView.ToTable(false, "Taxes_ID", "Percentage", "Amount", "AltTax_Code");

                tempTaxDetailsdt.Columns.Add("SlNo", typeof(string));
                //    tempTaxDetailsdt.Columns.Add("AltTaxCode", typeof(string));

                tempTaxDetailsdt.Columns["SlNo"].SetOrdinal(0);
                tempTaxDetailsdt.Columns["Taxes_ID"].SetOrdinal(1);
                tempTaxDetailsdt.Columns["AltTax_Code"].SetOrdinal(2);
                tempTaxDetailsdt.Columns["Percentage"].SetOrdinal(3);
                tempTaxDetailsdt.Columns["Amount"].SetOrdinal(4);

                foreach (DataRow d in tempTaxDetailsdt.Rows)
                {
                    d["SlNo"] = "0";
                    //d["AltTaxCode"] = "0";
                }

                // End

                // DataTable Of Billing Address

                DataTable tempBillAddress = new DataTable();
                if (Session["SI_QuotationAddressDtl"] != null)
                {
                    tempBillAddress = (DataTable)Session["SI_QuotationAddressDtl"];
                }
                else
                {
                    tempBillAddress = StoreQuotationAddressDetail();
                }

                // End

                string approveStatus = "";
                if (Request.QueryString["status"] != null)
                {
                    approveStatus = Convert.ToString(Request.QueryString["status"]);
                }

                if (ActionType == "Add")
                {
                    string[] SchemeList = strSchemeType.Split(new string[] { "~" }, StringSplitOptions.None);
                    validate = checkNMakeJVCode(strInvoiceNo, Convert.ToInt32(SchemeList[0]));
                }

                if (Convert.ToString(cmbDeliveryType.Value) == "D")
                {
                    foreach (DataRow dr in tempQuotation.Rows)
                    {
                        string strSrlNo = Convert.ToString(dr["SrlNo"]);
                        string strproduct = Convert.ToString(dr["ProductID"]);
                        string strproductDescription = Convert.ToString(dr["Description"]);
                        decimal strProductQuantity = Convert.ToDecimal(dr["Quantity"]);

                        ProcedureExecute proc = new ProcedureExecute("prc_PosCRMSalesInvoice_Details");
                        proc.AddVarcharPara("@Action", 500, "ProductIsInventory");
                        proc.AddVarcharPara("@ProductID", 20, strproduct);
                        DataTable receiptTable = proc.GetTable();
                        if (receiptTable.Rows.Count > 0)
                        {
                            decimal strWarehouseQuantity = 0;
                            GetQuantityBaseOnProduct(strSrlNo, ref strWarehouseQuantity);


                            if (strProductQuantity != strWarehouseQuantity)
                            {
                                validate = "checkWarehouse";
                                grid.JSProperties["cpProductSrlIDCheck"] = strproductDescription + " [ Line Number :" + strSrlNo + " ]";
                                break;
                            }
                        }
                    }
                }

                DataView dvData = new DataView(tempQuotation);
                dvData.RowFilter = "Status<>'D'";
                DataTable _tempQuotation = dvData.ToTable();

               // var duplicateRecords = _tempQuotation.AsEnumerable()
               //.GroupBy(r => r["ProductID"]) //coloumn name which has the duplicate values
               //.Where(gr => gr.Count() > 1)
               //.Select(g => g.Key);

               // foreach (var d in duplicateRecords)
               // {
               //     validate = "duplicateProduct";
               // }

                foreach (DataRow dr in tempQuotation.Rows)
                {
                    decimal ProductQuantity = Convert.ToDecimal(dr["Quantity"]);
                    string Status = Convert.ToString(dr["Status"]);

                    if (Status != "D")
                    {
                        if (ProductQuantity == 0)
                        {
                            validate = "nullQuantity";
                            break;
                        }
                    }
                }

                //Old Unit Table 
                DataTable oldUnitTable = (DataTable)Session["PosOldUnittable" + Convert.ToString(uniqueId.Value)];
                if (oldUnitTable == null)
                {
                    createOldUnittable();
                    oldUnitTable = (DataTable)Session["PosOldUnittable" + Convert.ToString(uniqueId.Value)];
                }
                if (oldUnitTable.Columns["oldUnit_id"] != null)
                {
                    oldUnitTable.Columns.Remove("oldUnit_id");
                    oldUnitTable.Columns.Remove("Product_Des");
                    oldUnitTable.Columns.Remove("oldUnit_Uom");
                }

                if (ActionType == "Add")
                {
                    if (Convert.ToString(cmbDeliveryType.Value) == "D")
                    {
                        string challanSchemeId = Convert.ToString(challanNoScheme.Value).Split('~')[0];
                        string challanNo = txtChallanNo.Text;
                        Challanvalidate = checkNChallanCode(challanNo, Convert.ToInt32(challanSchemeId));
                    }
                }


                decimal ProductAmount = 0;
                foreach (DataRow dr in tempQuotation.Rows)
                {
                    ProductAmount = ProductAmount + Convert.ToDecimal(dr["Amount"]);
                }
                #region Rajdip
                if (dt_PLQuote.Value != null && dt_SaleInvoiceDue.Value != null)
                {
                    if (Convert.ToDateTime(dt_SaleInvoiceDue.Value) < Convert.ToDateTime(dt_PLQuote.Value))
                    {
                        Challanvalidate = "DueDateLess";
                        validate = "DueDateLess";
                    }
                }
                if (txtCreditDays.Text.ToString() == "0" && hdnCrDateMandatory.Value.ToString() == "1" && HdPosType.Value.ToString() == "Crd")
                {
                    Challanvalidate = "nullCredit";
                    validate = "nullCredit";
                }
                #endregion Rajdip
                //Checking for minimum Sale Price
                if (hdBasketId.Value.Trim() == "")
                {
                    string ProductMinSalePriceList = "";
                    foreach (DataRow dr in tempQuotation.Rows)
                    {
                        ProductMinSalePriceList = ProductMinSalePriceList + Convert.ToString(dr["ProductID"]) + ",";
                    }
                    ProductMinSalePriceList = ProductMinSalePriceList.TrimEnd(',');
                    string validateminSalePrice = IsMinSalePriceOk(ProductMinSalePriceList, tempQuotation);
                    if (validateminSalePrice == "MinSalePriceGreater")
                    {
                        validate = "minSalePriceMust";
                    }
                    if (validateminSalePrice == "MRPLess")
                    {
                        validate = "MRPLess";
                    }
                }

                if (tempBillAddress.Rows.Count == 0)
                {
                    validate = "BillingSHippingRequired";
                }


                if (!GetValidReceiptOrNot(tempQuotation))
                {
                    validate = "InValidReceipt";
                }

                if (ProductAmount == 0)
                {
                    validate = "nullAmount";
                }


                /// Commented by indranil for bypass negative stock check. Mantis issue number : ///

                //if (Convert.ToString(cmbDeliveryType.Value) == "D")
                //{
                //    string StockCheckMsg = PosData.GetAvailableStockCheckForOutModules(_tempQuotation, Convert.ToString(ddl_Branch.SelectedValue), Convert.ToString(strInvoiceDate));
                //    if (!string.IsNullOrEmpty(StockCheckMsg))
                //    {
                //        validate = StockCheckMsg;
                //    }
                //}

                /// End Commented by indranil for bypass negative stock check. Mantis issue number : ///



                if (Challanvalidate == "outrange" || Challanvalidate == "duplicate" || validate == "outrange" || validate == "duplicate" || validate == "checkWarehouse" 
                    || validate == "duplicateProduct" || validate == "nullAmount" || validate == "nullQuantity" || validate == "minSalePriceMust" || validate == "MRPLess" || 
                    validate == "InValidReceipt" || validate == "MoreThanStock" || validate == "BillingSHippingRequired"
                    //Rev Rajdip
                    || Challanvalidate == "DueDateLess" || Challanvalidate == "nullCredit")
                    //End Rev Rajdip         
                {
                    grid.JSProperties["cpSaveSuccessOrFail"] = validate;
                }
                else
                {
                    grid.JSProperties["cpQuotationNo"] = UniqueQuotation;

                    #region To Show By Default Cursor after SAVE AND NEW
                    if (Convert.ToString(Request.QueryString["Key"]) == "Add") // session has been removed from quotation list page working good
                    {
                        string[] schemaid = new string[] { };
                        string schemavalue = ddl_numberingScheme.SelectedValue;
                        Session["SI_schemavalue"] = schemavalue;        // session has been removed from quotation list page working good
                        schemaid = ddl_numberingScheme.SelectedValue.Split('~');

                        string schematype = schemaid[1];
                        if (hdnRefreshType.Value == "N")
                        {
                            if (schematype == "1")
                            {
                                Session["SI_SaveMode"] = "A";
                            }
                            else
                            {
                                Session["SI_SaveMode"] = "M";
                            }
                        }
                        else
                        {
                            Session["SI_SaveMode"] = null;
                        }
                    }

                    #endregion

                    #region Subhabrata Section Start

                    int strIsComplete = 0, strQuoteID = 0;

                    string ShippingState = "";


                    if (ddl_PosGst.Value != null)
                    {
                        if (ddl_PosGst.Value.ToString() == "S")
                        {
                            ShippingState = Convert.ToString(lblShippingStateValue.Value);
                        }
                        else
                        {
                            ShippingState = Convert.ToString(lblBillingStateValue.Value);
                        }
                    }


                    TaxDetailTable = gstTaxDetails.SetTaxTableDataWithProductSerialRoundOff(ref tempQuotation, "SrlNo", "ProductID", "TotalAmount", "TaxAmount", TaxDetailTable, "S", dt_PLQuote.Date.ToString("yyyy-MM-dd"), strBranch, ShippingState, "E");


                    #region Add New Filed To Check from Table - Surojit

                    DataTable duplicatedt2 = new DataTable();

                    if (Convert.ToString(hdBasketId.Value).Trim() == "")
                    {
                        duplicatedt2.Columns.Add("productid", typeof(Int64));
                        duplicatedt2.Columns.Add("slno", typeof(Int32));
                        duplicatedt2.Columns.Add("Quantity", typeof(Decimal));
                        duplicatedt2.Columns.Add("packing", typeof(Decimal));
                        duplicatedt2.Columns.Add("PackingUom", typeof(Int32));
                        duplicatedt2.Columns.Add("PackingSelectUom", typeof(Int32));

                        if (HttpContext.Current.Session["SessionPackingDetails"] != null)
                        {
                            List<ProductQuantity> obj = new List<ProductQuantity>();
                            obj = (List<ProductQuantity>)HttpContext.Current.Session["SessionPackingDetails"];
                            foreach (var item in obj)
                            {
                                duplicatedt2.Rows.Add(item.productid, item.slno, item.Quantity, item.packing, item.PackingUom, item.PackingSelectUom);
                            }
                        }
                    }
                    else
                    {
                        PosSalesInvoiceBl pos = new PosSalesInvoiceBl();
                        DataSet busketSet = pos.GetBusketDetailsById(Convert.ToInt32(hdBasketId.Value));
                        DataTable GrdpackQty = busketSet.Tables[4];

                        duplicatedt2.Columns.Add("productid", typeof(Int64));
                        duplicatedt2.Columns.Add("pslno", typeof(Int32));
                        duplicatedt2.Columns.Add("pQuantity", typeof(Decimal));
                        duplicatedt2.Columns.Add("packing", typeof(Decimal));
                        duplicatedt2.Columns.Add("PackingUom", typeof(Int32));
                        duplicatedt2.Columns.Add("PackingSelectUom", typeof(Int32));
                        HttpContext.Current.Session["SessionPackingDetails"] = GetWaitingProductDetails(GrdpackQty);
                        if (HttpContext.Current.Session["SessionPackingDetails"] != null)
                        {
                            List<WaitingProductQuantity> obj = new List<WaitingProductQuantity>();
                            obj = (List<WaitingProductQuantity>)HttpContext.Current.Session["SessionPackingDetails"];
                            foreach (var item in obj)
                            {
                                duplicatedt2.Rows.Add(item.productid, item.pslno, item.pQuantity, item.packing, item.PackingUom, item.PackingSelectUom);
                            }
                        }
                    }
                    HttpContext.Current.Session["SessionPackingDetails"] = null;
                    #endregion


                    #region  Swap Amount

                    foreach (DataRow DR in tempQuotation.Rows)
                    {
                        string Amount = Convert.ToString(DR["Amount"]);
                        string TotalAmount = Convert.ToString(DR["TotalAmount"]);
                        DR["TotalAmount"] = Amount;
                        DR["Amount"] = TotalAmount;
                    }
                    tempQuotation.AcceptChanges();




                    #endregion


                    // List<object> VehicleDetails = poupVehicles.GridView.GetSelectedFieldValues("vehicle_Id");



                    string Vehicles = txtVehicles.Text;
                    //int i = 0;
                    //foreach (object item in VehicleDetails)
                    //{
                    //    if (i == 0)
                    //    {
                    //        i = i + 1;
                    //        Vehicles = item.ToString();
                    //    }
                    //    else
                    //    {
                    //        Vehicles = Vehicles + "," + item.ToString();
                    //    }
                    //}




                    ModifyQuatation(MainInvoiceID, strSchemeType, UniqueQuotation, strInvoiceDate, strCustomer, strContactName,
                                    Reference, strBranch, strAgents, strCurrency, strRate, strTaxType, strTaxCode,
                                    strComponenyType, InvoiceComponent, InvoiceComponentDate, strCashBank, strDueDate,
                                    tempQuotation, TaxDetailTable, tempWarehousedt, tempTaxDetailsdt, tempBillAddress,
                                    approveStatus, ActionType, ref strIsComplete, ref strQuoteID, oldUnit,
                                    financerId, ExecutiveId, EMIDetails, financeAmount, dbd, dbdPercent, ProcFee, sfCode, paymentDetails, oldUnitTable, UniqueChallan, strDeliveryDate, duplicatedt2, Vehicles, placeofsupply);
                    //---------Rev Subhra 22-05-2019---------------------

                    BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
                    DataTable dtCmb = new DataTable();
                    dtCmb = oGenericMethod.GetDataTable("select dbo.fInvoiceGSTType(" + Convert.ToString(strQuoteID) + ")");
                    grid.JSProperties["cpIsCGSTorIGST"] = dtCmb.Rows[0][0].ToString();
                    //---------End of Rev--------------------------------

                    //Rev Tanmoy 12-08-2019 send SMS
                    String msgResponse = ObjSms.sendSMS(hdnCustMobile.Value, msgBody);
                    int stus = ObjSms.SmsStatusSave(msgBody, "POS Sales Invoice", msgResponse, hdnCustMobile.Value);
                    if (msgResponse == "")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('SMS Configuration Need.')", true);
                    }
                    //End Rev Tanmoy

                    grid.JSProperties["cpGeneratedInvoice"] = Convert.ToString(strQuoteID);

                    grid.JSProperties["cpIsInstallRequired"] = IsProductNeedInstall(Convert.ToString(strQuoteID));
                    //Udf Add mode
                    DataTable udfTable = (DataTable)Session["UdfDataOnAdd"];
                    if (udfTable != null)
                        Session["UdfDataOnAdd"] = reCat.insertRemarksCategoryAddMode("POS", "SaleInvoice" + Convert.ToString(strQuoteID), udfTable, Convert.ToString(Session["userid"]));

                    if (strIsComplete == 1)
                    {
                        if (approveStatus != "")
                        {
                            if (approveStatus == "2")
                            {
                                grid.JSProperties["cpApproverStatus"] = "approve";
                            }
                            else
                            {
                                grid.JSProperties["cpApproverStatus"] = "rejected";
                            }
                        }



                        Employee_BL objemployeebal = new Employee_BL();
                        int MailStatus = 0;
                        string AssignedTo_Email = string.Empty;
                        string ReceiverEmail = string.Empty;
                        decimal Level1_User = Convert.ToDecimal(Session["userid"]);
                        decimal Level2User = Convert.ToDecimal(Session["userid"]);
                        decimal Level3User = Convert.ToDecimal(Session["userid"]);
                        bool L1 = false;
                        bool L2 = false;
                        bool L3 = false;
                        string ActivityName = string.Empty;

                        DataTable dtbl_AssignedTo = new DataTable();
                        DataTable dtbl_AssignedBy = new DataTable();
                        DataTable dtEmail_To = new DataTable();
                        DataTable dt_EmailConfig = new DataTable();
                        DataTable dt_ActivityName = new DataTable();
                        DataTable Dt_LevelUser = new DataTable();

                        dt_EmailConfig = objemployeebal.GetEmailAccountConfigDetails("", 1); // Checked fetch datatatable of email setup with action 1 param
                        Dt_LevelUser = objemployeebal.GetAllLevelUsers("1", 12);

                        ActivityName = UniqueQuotation;

                        if (Dt_LevelUser != null && string.IsNullOrEmpty(approveStatus))
                        {
                            L1 = true;
                            dtEmail_To = objemployeebal.GetEmailLevelUsersWise(1, 11, 1);
                        }
                        else if (Dt_LevelUser != null && Dt_LevelUser.Rows[0].Field<decimal>("Level1_user_id") == Level1_User && approveStatus == "2")
                        {
                            L2 = true;
                            dtEmail_To = objemployeebal.GetEmailLevelUsersWise(1, 11, 2);
                        }
                        else if (Dt_LevelUser != null && Dt_LevelUser.Rows[0].Field<decimal>("Level2_user_id") == Level2User && approveStatus == "2")
                        {
                            L3 = true;
                            dtEmail_To = objemployeebal.GetEmailLevelUsersWise(1, 11, 3);
                        }

                        if (dtEmail_To != null && dtEmail_To.Rows.Count > 0)
                        {
                            if (!string.IsNullOrEmpty(dtEmail_To.Rows[0].Field<string>("Email_Id")))
                            {
                                ReceiverEmail = Convert.ToString(dtEmail_To.Rows[0].Field<string>("Email_Id"));
                            }
                            else
                            {
                                ReceiverEmail = "";
                            }
                        }



                        ListDictionary replacements = new ListDictionary();
                        if (dtEmail_To.Rows.Count > 0)
                        {
                            replacements.Add("<%Approver%>", Convert.ToString(dtEmail_To.Rows[0].Field<string>("Approver")));
                        }
                        else
                        {
                            replacements.Add("<%Approver%>", "");
                        }
                        replacements.Add("<%QuotationNo%>", UniqueQuotation);
                        //ExceptionLogging.SendExceptionMail(ex, Convert.ToInt32(lineNumber));

                        if (!string.IsNullOrEmpty(ReceiverEmail))
                        {
                            //ExceptionLogging.SendEmailToAssigneeByUser(ReceiverEmail, "", replacements, "~/OMS/EmailTemplates/EmailSendToAssigneeByUserID.html", dt_EmailConfig, ActivityName,4);
                            MailStatus = ExceptionLogging.SendEmailToAssigneeByUser(ReceiverEmail, "", replacements, dt_EmailConfig, ActivityName, 12);
                        }
                        SendPosSMSToUser(UniqueQuotation, strCustomer, strInvoiceDate);
                        RemoveSessionOnPage();
                    }
                    else if (strIsComplete == 2)
                    {
                        grid.JSProperties["cpSaveSuccessOrFail"] = "quantityTagged";
                    }
                    else if (strIsComplete == -9)
                    {
                        DataTable dts = new DataTable();
                        dts = GetAddLockStatus();
                        grid.JSProperties["cpSaveSuccessOrFail"] = "AddLock";
                        grid.JSProperties["cpAddLockStatus"] = (Convert.ToString(dts.Rows[0]["Lock_Fromdate"]) + " to " + Convert.ToString(dts.Rows[0]["Lock_Todate"]));
                    }
                    else
                    {
                        grid.JSProperties["cpSaveSuccessOrFail"] = "errorInsert";
                    }

                    #endregion Subhabrata Section End
                }
            }
            else
            {
                DataView dvData = new DataView(Quotationdt);
                dvData.RowFilter = "Status <> 'D'";

                grid.DataSource = GetQuotation(dvData.ToTable());
                grid.DataBind();
            }
        }
        public DataTable GetAddLockStatus()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesInvoice_Details");
            proc.AddVarcharPara("@Action", 500, "GetAddLockForSalesInvoicePOS");

            dt = proc.GetTable();
            return dt;

        }
        protected bool GetValidReceiptOrNot(DataTable productTable)
        {
            bool retvalue = true;
            string ProductIdList = "";
            string customerReceipt = Convert.ToString(hdAddvanceReceiptNo.Value);
            string receiptNo = "";
            string[] custReceipt = customerReceipt.Split(',');
            string[] recType;

            foreach (string recNo in custReceipt)
            {
                recType = recNo.Split('~');

                if (recType.Length > 1)
                {

                    if (recType[1] != "" && recType[1] == "Adv")
                        receiptNo = receiptNo + "," + recType[0];
                }
            }

            receiptNo = receiptNo.TrimStart(',');

            foreach (DataRow dr in productTable.Rows)
            {
                ProductIdList = ProductIdList + ',' + Convert.ToString(dr["ProductID"]);
            }
            ProductIdList = ProductIdList.TrimStart(',');

            ProcedureExecute proc = new ProcedureExecute("prc_PosCRMSalesInvoice_Details");
            proc.AddVarcharPara("@Action", 500, "validReceipt");
            proc.AddVarcharPara("@ProductList", 4000, ProductIdList);
            proc.AddVarcharPara("@receiptList", 4000, receiptNo);
            DataTable receiptTable = proc.GetTable();

            if (Convert.ToString(receiptTable.Rows[0][0]) == "1")
            {
                retvalue = true;
            }
            else
            {
                retvalue = false;
            }

            return retvalue;

        }

        protected void grid_DataBinding(object sender, EventArgs e)
        {
            //string IsDeleteFrom = Convert.ToString(hdfIsDelete.Value);
            //if (IsDeleteFrom == "D")
            //{
            //    DataTable Quotationdt = (DataTable)Session["SI_QuotationDetails"];
            //    grid.DataSource = GetQuotation(Quotationdt);
            //}
            //else
            //{
            //    grid.DataSource = GetQuotation();
            //}

            if (Session["SI_QuotationDetails" + Convert.ToString(uniqueId.Value)] != null)
            {
                DataTable Quotationdt = (DataTable)Session["SI_QuotationDetails" + Convert.ToString(uniqueId.Value)];
                DataView dvData = new DataView(Quotationdt);
                dvData.RowFilter = "Status <> 'D'";
                grid.DataSource = GetQuotation(dvData.ToTable());
            }
        }

        protected string IsMinSalePriceOk(string list, DataTable DetailsTable)
        {
            string validate = "";
            DataTable minSalePriceTable = PosData.IsMinSalePriceOk(list);

            if (minSalePriceTable != null)
            {

                foreach (DataRow dr in minSalePriceTable.Rows)
                {
                    DataRow[] productRow = DetailsTable.Select("ProductID='" + Convert.ToString(dr["sProducts_ID"]) + "'");
                    if (Convert.ToDecimal(dr["sProduct_MinSalePrice"]) > Convert.ToDecimal(productRow[0]["SalePrice"]))
                    {
                        validate = "MinSalePriceGreater";
                        break;
                    }
                    if (Convert.ToDecimal(dr["sProduct_MRP"]) != 0 && Convert.ToDecimal(dr["sProduct_MRP"]) < Convert.ToDecimal(productRow[0]["SalePrice"]))
                    {
                        validate = "MRPLess";
                        break;
                    }
                }

                //validate = "MinSalePriceGreater";

            }

            return validate;
        }

        protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string strSplitCommand = e.Parameters.Split('~')[0];
            if (strSplitCommand == "Display")
            {
                string IsDeleteFrom = Convert.ToString(hdfIsDelete.Value);
                if (IsDeleteFrom == "D" || IsDeleteFrom == "C")
                {
                    DataTable Quotationdt = (DataTable)Session["SI_QuotationDetails" + Convert.ToString(uniqueId.Value)];
                    grid.DataSource = GetQuotation(Quotationdt);
                    grid.DataBind();
                }

                if (e.Parameters.Split('~').Length > 1)
                {
                    if (e.Parameters.Split('~')[1] == "fromComponent")
                    {
                        grid.JSProperties["cpComponent"] = "true";
                    }
                }
            }
            else if (strSplitCommand == "GridBlank")
            {
                Session["SI_QuotationDetails" + Convert.ToString(uniqueId.Value)] = null;
                grid.DataSource = null;
                grid.DataBind();
            }
            else if (strSplitCommand == "rebindgridFromBasket")
            {
                string basketId = hdBasketId.Value;
                DataSet basketDetails = PosData.GetBasketDetailsOnly(Convert.ToInt32(basketId));
                #region SetShippingState
                string ShippingCode = "";
                if (basketDetails.Tables[1].Rows.Count > 0)
                {
                    ShippingCode = Convert.ToString(basketDetails.Tables[1].Rows[0][0]);
                }
                #endregion
                DataTable ProductDetails = basketDetails.Tables[0];
                CreateDataTaxTable();
                DataTable taxtable = (DataTable)Session["SI_FinalTaxRecord" + Convert.ToString(uniqueId.Value)];
                ProductDetails = gstTaxDetails.SetTaxAmountWithGSTonDetailsTable(ProductDetails, "sProductsID", "TaxAmount", "Amount", "TotalAmount", dt_PLQuote.Date.ToString("yyyy-MM-dd"), "S", Convert.ToString(ddl_Branch.SelectedValue), ShippingCode, "I");
                taxtable = gstTaxDetails.SetTaxTableDataWithProductSerial(ProductDetails, "SrlNo", "sProductsID", "Amount", "TaxAmount", taxtable, "S", dt_PLQuote.Date.ToString("yyyy-MM-dd"), Convert.ToString(ddl_Branch.SelectedValue), ShippingCode, "I");
                Session["SI_FinalTaxRecord" + Convert.ToString(uniqueId.Value)] = taxtable;

                ProductDetails.Columns.Remove("sProductsID");
                Session["SI_QuotationDetails" + Convert.ToString(uniqueId.Value)] = ProductDetails;
                grid.DataSource = GetQuotation(ProductDetails);
                grid.DataBind();

            }
            else if (strSplitCommand == "CurrencyChangeDisplay")
            {
                if (Session["SI_QuotationDetails" + Convert.ToString(uniqueId.Value)] != null)
                {
                    DataTable Quotationdt = (DataTable)Session["SI_QuotationDetails" + Convert.ToString(uniqueId.Value)];

                    string strCurrRate = Convert.ToString(txt_Rate.Text);
                    decimal strRate = 1;

                    if (strCurrRate != "")
                    {
                        strRate = Convert.ToDecimal(strCurrRate);
                        if (strRate == 0) strRate = 1;
                    }

                    foreach (DataRow dr in Quotationdt.Rows)
                    {
                        string Status = Convert.ToString(dr["Status"]);
                        if (Status != "D")
                        {
                            string ProductDetails = Convert.ToString(dr["ProductID"]);
                            string QuantityValue = Convert.ToString(dr["Quantity"]);
                            string Discount = Convert.ToString(dr["Discount"]);
                            string[] ProductDetailsList = ProductDetails.Split(new string[] { "||@||" }, StringSplitOptions.None);

                            string strMultiplier = ProductDetailsList[7];
                            string strFactor = ProductDetailsList[8];
                            string strSalePrice = ProductDetailsList[6];

                            decimal SalePrice = Convert.ToDecimal(strSalePrice) / strRate;
                            string Amount = Convert.ToString(Math.Round((Convert.ToDecimal(QuantityValue) * Convert.ToDecimal(strFactor) * SalePrice), 2));
                            string amountAfterDiscount = Convert.ToString(Math.Round((Convert.ToDecimal(Amount) - ((Convert.ToDecimal(Discount) * Convert.ToDecimal(Amount)) / 100)), 2));


                            dr["SalePrice"] = Convert.ToString(Math.Round(SalePrice, 2));
                            dr["Amount"] = amountAfterDiscount;
                            dr["TaxAmount"] = ReCalculateTaxAmount(Convert.ToString(dr["SrlNo"]), Convert.ToDouble(amountAfterDiscount));
                            dr["TotalAmount"] = Convert.ToDecimal(dr["Amount"]) + Convert.ToDecimal(dr["TaxAmount"]);
                        }
                    }

                    Session["SI_QuotationDetails" + Convert.ToString(uniqueId.Value)] = Quotationdt;

                    DataView dvData = new DataView(Quotationdt);
                    dvData.RowFilter = "Status <> 'D'";

                    grid.DataSource = GetQuotation(dvData.ToTable());
                    grid.DataBind();
                }
            }
            else if (strSplitCommand == "DateChangeDisplay")
            {
                if (Session["SI_QuotationDetails" + Convert.ToString(uniqueId.Value)] != null)
                {
                    DataTable Quotationdt = (DataTable)Session["SI_QuotationDetails" + Convert.ToString(uniqueId.Value)];

                    string strCurrRate = Convert.ToString(txt_Rate.Text);
                    decimal strRate = 1;

                    if (strCurrRate != "")
                    {
                        strRate = Convert.ToDecimal(strCurrRate);
                        if (strRate == 0) strRate = 1;
                    }

                    foreach (DataRow dr in Quotationdt.Rows)
                    {
                        string Status = Convert.ToString(dr["Status"]);
                        if (Status != "D")
                        {
                            dr["TaxAmount"] = 0;
                            dr["TotalAmount"] = dr["Amount"];
                        }
                    }

                    Session["SI_QuotationDetails" + Convert.ToString(uniqueId.Value)] = Quotationdt;

                    DataView dvData = new DataView(Quotationdt);
                    dvData.RowFilter = "Status <> 'D'";

                    grid.DataSource = GetQuotation(dvData.ToTable());
                    grid.DataBind();
                }
            }
            else if (strSplitCommand == "BindGridOnQuotation")
            {
                string command = e.Parameters.ToString();
                string State = Convert.ToString(e.Parameters.Split('~')[1]);
                string strType = Convert.ToString(rdl_SaleInvoice.SelectedValue);
                string ComponentDetailsIDs = string.Empty;
                string strAction = "";

                for (int i = 0; i < grid_Products.GetSelectedFieldValues("ComponentDetailsID").Count; i++)
                {
                    ComponentDetailsIDs += "," + Convert.ToString(grid_Products.GetSelectedFieldValues("ComponentDetailsID")[i]);
                }
                ComponentDetailsIDs = ComponentDetailsIDs.TrimStart(',');


                if (strType == "QO")
                {
                    // strAction = "GetSeletedQuotationProducts  mantis issue number 20058
                    strAction = "GetSeletedQuotationProductsPOS";

                }
                else if (strType == "SO")
                {
                    strAction = "GetSeletedOrderProducts";
                }
                else if (strType == "SC")
                {
                    strAction = "GetSeletedChallanProducts";
                }

                string strInvoiceID = InvoiceId;// Convert.ToString(Session["SI_InvoiceID"]);
                DataTable dt_QuotationDetails = objSalesInvoiceBL.GetSelectedComponentProductList(strAction, ComponentDetailsIDs, strInvoiceID);
                Session["SI_QuotationDetails" + Convert.ToString(uniqueId.Value)] = dt_QuotationDetails;
                TotalTaggingAmount = 0;
                grid.DataSource = GetQuotation(dt_QuotationDetails);
                grid.JSProperties["cpTaggingTotalAmount"] = Convert.ToString(TotalTaggingAmount);
                grid.DataBind();
                TotalTaggingAmount = 0;
                Session["SI_WarehouseData" + Convert.ToString(uniqueId.Value)] = GetTaggingWarehouseData(ComponentDetailsIDs, strType);
                Session["SI_FinalTaxRecord" + Convert.ToString(uniqueId.Value)] = GetComponentEditedTaxData(ComponentDetailsIDs, strType);
            }
            else if (strSplitCommand == "UpdateExistingData")
            {
                UpdatePosData();
                grid.JSProperties["cpReturnParameter"] = strSplitCommand;
            }
        }

        public void UpdatePosData()
        {
            try
            {
                string InvId = "";// Convert.ToString(Session["SI_InvoiceID"]);
                if (Request.QueryString["Key"] != "ADD")
                    InvId = Request.QueryString["Key"];


                DataTable invoiceExistTable = oDBEngine.GetDataTable("select 1 from tbl_trans_SalesInvoice where Invoice_Id<>" + InvId + " and Invoice_Number='" + Convert.ToString(txt_PLQuoteNo.Text) + "'");
                if (invoiceExistTable.Rows.Count == 0)
                {


                    DataSet dsInst = new DataSet();


                    //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                    SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

                    SqlCommand cmd = new SqlCommand("Prc_updatePosData", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@invoiceId", Convert.ToInt32(InvId));
                    cmd.Parameters.AddWithValue("@References", txt_Refference.Text);
                    cmd.Parameters.AddWithValue("@pos_Unitremarks", txtRemarks.Text);
                    cmd.Parameters.AddWithValue("@Pos_DBD", txtDBD.Text);
                    cmd.Parameters.AddWithValue("@Pos_DBDPercent", txtDbdPercen.Text);
                    cmd.Parameters.AddWithValue("@Pos_ProcFee", txtprocFee.Text);
                    cmd.Parameters.AddWithValue("@Pos_SFCode", txtSfCode.Text);
                    cmd.Parameters.AddWithValue("@Pos_Financer", cmbFinancer.Value == null ? "" : Convert.ToString(cmbFinancer.Value));
                    cmd.Parameters.AddWithValue("@Pos_Executive", cmbExecName.Value == null ? "" : Convert.ToString(cmbExecName.Value));
                    cmd.Parameters.AddWithValue("@EntryType", HdPosType.Value);
                    cmd.Parameters.AddWithValue("@updatedBy", Convert.ToString(Session["userid"]));
                    cmd.Parameters.AddWithValue("@Pos_DeliveryType", Convert.ToString(cmbDeliveryType.Value));
                    cmd.Parameters.AddWithValue("@Invoice_Number", Convert.ToString(txt_PLQuoteNo.Text));
                    cmd.Parameters.AddWithValue("@Invoice_SalesmanId", Convert.ToString(ddl_SalesAgent.Value));
                    cmd.Parameters.AddWithValue("@pos_deliveryDate", Convert.ToString(deliveryDate.Date.ToString("yyyy-MM-dd")));
                    cmd.CommandTimeout = 0;
                    SqlDataAdapter Adap = new SqlDataAdapter();
                    Adap.SelectCommand = cmd;
                    Adap.Fill(dsInst);
                    cmd.Dispose();
                    con.Dispose();

                    Page.ClientScript.RegisterStartupScript(GetType(), "updated", "<script>Updated();</script>");
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "updated", "<script>InvoiceExists();</script>");
                }

            }
            catch (Exception excp)
            {

            }

        }

        public void ModifyQuatation(string QuotationID, string strSchemeType, string strQuoteNo, string strQuoteDate, string strCustomer, string strContactName,
                                    string Reference, string strBranch, string strAgents, string strCurrency, string strRate, string strTaxType, string strTaxCode,
                                    string strComponenyType, string strInvoiceComponent, string strInvoiceComponentDate, string strCashBank, string strDueDate,
                                    DataTable Productdt, DataTable TaxDetailTable, DataTable Warehousedt, DataTable InvoiceTaxdt, DataTable BillAddressdt, string approveStatus, string ActionType,
                                    ref int strIsComplete, ref int strInvoiceID, bool oldUnit, string financerId, string ExecutiveId
                                    , string EMIDetails, decimal financeAmount, decimal dbd, decimal dbdPercent, decimal ProcFee, string sfCode, DataTable paymentDetails,
                                    DataTable oldUnitTable, string UniqueChallan, string strDeliveryDate, DataTable PackingDetailsdt, string Vehicles, string placeofsupply)
        {
            try
            {
                DataSet dsInst = new DataSet();



                if (Productdt.Columns.Contains("Old_Amount"))
                {
                    Productdt.Columns.Remove("Old_Amount");
                }





                // SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));


                SqlCommand cmd = new SqlCommand("prc_PosCRMSalesInvoice_Exclusive", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", ActionType);
                cmd.Parameters.AddWithValue("@approveStatus", approveStatus);

                cmd.Parameters.AddWithValue("@InvoiceID", QuotationID);
                cmd.Parameters.AddWithValue("@InvoiceNo", strQuoteNo);
                cmd.Parameters.AddWithValue("@InvoiceDate", strQuoteDate);
                cmd.Parameters.AddWithValue("@BranchID", strBranch);

                cmd.Parameters.AddWithValue("@CustomerID", strCustomer);
                cmd.Parameters.AddWithValue("@ContactPerson", strContactName);
                cmd.Parameters.AddWithValue("@Reference", Reference);
                cmd.Parameters.AddWithValue("@Agents", strAgents);

                cmd.Parameters.AddWithValue("@ComponenyType", strComponenyType);
                cmd.Parameters.AddWithValue("@InvoiceComponent", strInvoiceComponent);
                cmd.Parameters.AddWithValue("@InvoiceComponentDate", strInvoiceComponentDate);
                cmd.Parameters.AddWithValue("@CashBank", strCashBank);
                cmd.Parameters.AddWithValue("@DueDate", strDueDate);

                cmd.Parameters.AddWithValue("@Currency", strCurrency);
                cmd.Parameters.AddWithValue("@Rate", strRate);
                cmd.Parameters.AddWithValue("@TaxType", strTaxType);
                cmd.Parameters.AddWithValue("@TaxCode", strTaxCode);

                cmd.Parameters.AddWithValue("@CompanyID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FinYear", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@UserID", Convert.ToString(Session["userid"]));

                cmd.Parameters.AddWithValue("@ProductDetails", Productdt);
                cmd.Parameters.AddWithValue("@TaxDetail", TaxDetailTable);
                cmd.Parameters.AddWithValue("@WarehouseDetail", Warehousedt);
                cmd.Parameters.AddWithValue("@InvoiceTax", InvoiceTaxdt);
                cmd.Parameters.AddWithValue("@BillAddress", BillAddressdt);
                //Pos data
                cmd.Parameters.AddWithValue("@Pos_DeliveryType", Convert.ToString(cmbDeliveryType.Value));
                cmd.Parameters.AddWithValue("@Pos_DeliveredFrom", Convert.ToInt32(ddDeliveredFrom.SelectedValue));
                cmd.Parameters.AddWithValue("@pos_oldUnit", oldUnit);
                cmd.Parameters.AddWithValue("@pos_unitValue", Convert.ToDecimal(txtunitValue.Text));
                cmd.Parameters.AddWithValue("@pos_Unitremarks", txtRemarks.Text);
                cmd.Parameters.AddWithValue("@pos_advncRecptNo", txtAdvnceReceipt.Text);
                cmd.Parameters.AddWithValue("@Pos_advanceRecptValue", Convert.ToDecimal(txtAdvnceReceipt.Text.Trim() == "" ? "0" : txtAdvnceReceipt.Text));
                // financer
                cmd.Parameters.AddWithValue("@Pos_Financer", financerId);
                cmd.Parameters.AddWithValue("@Pos_Executive", ExecutiveId);
                cmd.Parameters.AddWithValue("@Pos_EMIDetails", EMIDetails);
                cmd.Parameters.AddWithValue("@Pos_FinanceAmt", financeAmount);
                cmd.Parameters.AddWithValue("@Pos_DBD", dbd);
                cmd.Parameters.AddWithValue("@Pos_DBDPercent", dbdPercent);
                cmd.Parameters.AddWithValue("@Pos_ProcFee", ProcFee);
                cmd.Parameters.AddWithValue("@Pos_SFCode", sfCode);
                cmd.Parameters.AddWithValue("@Pos_EmiOther_charges", txtEmiOtherCharges.Text);
                cmd.Parameters.AddWithValue("@Pos_downPayment", txtdownPayment.Text);
                cmd.Parameters.AddWithValue("@Pos_total_dp_Amt", txtTotDpAmt.Text);
                cmd.Parameters.AddWithValue("@Pos_Scheme", txtScheme.Text);
                cmd.Parameters.AddWithValue("@Pos_FinChallanNo", txtfinChallanNo.Text);
                if (finChallandate.Value != null)
                    cmd.Parameters.AddWithValue("@Pos_FinChallanDate", finChallandate.Date.ToString("yyyy-MM-dd"));
                else
                    cmd.Parameters.AddWithValue("@Pos_FinChallanDate", null);
                //Payement Details
                cmd.Parameters.AddWithValue("@paymentDetails", paymentDetails);
                //oldUnit details 
                cmd.Parameters.AddWithValue("@OldUnitDetails", oldUnitTable);
                cmd.Parameters.AddWithValue("@AdvanceReceiptNoList", Convert.ToString(hdAddvanceReceiptNo.Value));
                cmd.Parameters.AddWithValue("@Pos_EntryType", Convert.ToString(HdPosType.Value));
                cmd.Parameters.AddWithValue("@UniqueChallan", UniqueChallan);

                cmd.Parameters.AddWithValue("@Pos_deliveryDate", strDeliveryDate);


                cmd.Parameters.AddWithValue("@ReturnPackingDetails", PackingDetailsdt); //Surojit 15-03-2019
                cmd.Parameters.AddWithValue("@Vehicles", Vehicles);
                cmd.Parameters.AddWithValue("@placeofsupply", placeofsupply);

                if (Convert.ToString(hdBasketId.Value).Trim() != "")
                    cmd.Parameters.AddWithValue("@BasketId", Convert.ToInt32(hdBasketId.Value));

                cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 50);
                cmd.Parameters.Add("@ReturnInvoiceID", SqlDbType.VarChar, 50);



                cmd.Parameters.Add("@UniqueChallanOutput", SqlDbType.VarChar, 50);

                cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;
                cmd.Parameters["@ReturnInvoiceID"].Direction = ParameterDirection.Output;
                cmd.Parameters["@UniqueChallanOutput"].Direction = ParameterDirection.Output;


                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);

                strIsComplete = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value.ToString());
                strInvoiceID = Convert.ToInt32(cmd.Parameters["@ReturnInvoiceID"].Value.ToString());
                int UniqueChallanOutput = Convert.ToInt32(cmd.Parameters["@UniqueChallanOutput"].Value.ToString());
                if (hdnShowUOMConversionInEntry.Value == "1")
                {
                    if (HttpContext.Current.Session["SecondUOMDetails"] != null)
                    {
                        SecondUOMDetailsBL uomBL = new SecondUOMDetailsBL();
                        List<SecondUOMDetails> finalResult = (List<SecondUOMDetails>)HttpContext.Current.Session["SecondUOMDetails"];
                        DataTable dtoutput = uomBL.SaveSencondUOMDetails(finalResult, "SC", "OUT", Convert.ToString(UniqueChallanOutput));
                        HttpContext.Current.Session["SecondUOMDetails"] = null;
                    }

                }



                cmd.Dispose();
                con.Dispose();
            }
            catch (Exception ex)
            {
            }
        }
        protected void Grid_RowInserting(object sender, ASPxDataInsertingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void Grid_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void Grid_RowDeleting(object sender, ASPxDataDeletingEventArgs e)
        {
            e.Cancel = true;
        }

        protected void grid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data)
                return;

            string IsLinkedProduct = Convert.ToString(e.GetValue("IsLinkedProduct"));
            if (IsLinkedProduct == "Y")
                e.Row.ForeColor = Color.Blue;
        }

        #endregion

        #region Quotation Tax Details

        public IEnumerable GetTaxes()
        {
            List<Taxes> TaxList = new List<Taxes>();

            // BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();


            DataTable DT = GetTaxData(dt_PLQuote.Date.ToString("yyyy-MM-dd"));
            for (int i = 0; i < DT.Rows.Count; i++)
            {
                Taxes Taxes = new Taxes();
                Taxes.TaxID = Convert.ToString(DT.Rows[i]["Taxes_ID"]);
                Taxes.TaxName = Convert.ToString(DT.Rows[i]["Taxes_Name"]);
                Taxes.Percentage = Convert.ToString(DT.Rows[i]["Percentage"]);
                Taxes.Amount = Convert.ToString(DT.Rows[i]["Amount"]);
                TaxList.Add(Taxes);
            }

            return TaxList;
        }
        public IEnumerable GetTaxes(DataTable DT)
        {
            List<Taxes> TaxList = new List<Taxes>();

            decimal totalParcentage = 0;
            foreach (DataRow dr in DT.Rows)
            {
                if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST")
                {
                    totalParcentage += Convert.ToDecimal(dr["Percentage"]);
                }
            }




            for (int i = 0; i < DT.Rows.Count; i++)
            {
                if (Convert.ToString(DT.Rows[i]["Taxes_ID"]) != "0")
                {
                    Taxes Taxes = new Taxes();
                    Taxes.TaxID = Convert.ToString(DT.Rows[i]["Taxes_ID"]);
                    Taxes.TaxName = Convert.ToString(DT.Rows[i]["Taxes_Name"]);
                    Taxes.Percentage = Convert.ToString(DT.Rows[i]["Percentage"]);
                    Taxes.Amount = Convert.ToString(DT.Rows[i]["Amount"]);
                    if (Convert.ToString(DT.Rows[i]["ApplicableOn"]) == "G")
                    {
                        Taxes.calCulatedOn = Convert.ToDecimal(HdChargeProdAmt.Value);
                    }
                    else if (Convert.ToString(DT.Rows[i]["ApplicableOn"]) == "N")
                    {
                        Taxes.calCulatedOn = Convert.ToDecimal(HdChargeProdNetAmt.Value);
                    }
                    else
                    {
                        Taxes.calCulatedOn = 0;
                    }

                    //Set Amount Value 
                    if (Taxes.Amount == "0.00")
                    {
                        Taxes.Amount = Convert.ToString(Taxes.calCulatedOn * (Convert.ToDecimal(Taxes.Percentage) / 100));
                    }


                    if (Convert.ToString(ddl_AmountAre.Value) == "2")
                    {

                        if (Convert.ToString(DT.Rows[i]["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(DT.Rows[i]["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(DT.Rows[i]["TaxTypeCode"]).Trim() == "SGST")
                        {
                            decimal finalCalCulatedOn = 0;
                            decimal backProcessRate = (1 + (totalParcentage / 100));
                            finalCalCulatedOn = Taxes.calCulatedOn / backProcessRate;
                            Taxes.calCulatedOn = Math.Round(finalCalCulatedOn);
                        }

                    }


                    TaxList.Add(Taxes);
                }
            }

            return TaxList;
        }
        public DataTable GetTaxData(string quoteDate)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesInvoice_Details");
            proc.AddVarcharPara("@Action", 500, "TaxDetails");
            proc.AddVarcharPara("@InvoiceID", 500, InvoiceId);
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            proc.AddVarcharPara("@BranchID", 500, Convert.ToString(ddl_Branch.SelectedValue));
            proc.AddVarcharPara("@CompanyID", 500, Convert.ToString(Session["LastCompany"]));
            proc.AddVarcharPara("@S_quoteDate", 10, quoteDate);
            dt = proc.GetTable();
            return dt;
        }
        protected void gridTax_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string strSplitCommand = e.Parameters.Split('~')[0];
            if (strSplitCommand == "Display")
            {
                DataTable TaxDetailsdt = new DataTable();
                if (Session["SI_TaxDetails" + Convert.ToString(uniqueId.Value)] == null)
                {
                    Session["SI_TaxDetails" + Convert.ToString(uniqueId.Value)] = GetTaxData(dt_PLQuote.Date.ToString("yyyy-MM-dd"));
                }

                if (Session["SI_TaxDetails" + Convert.ToString(uniqueId.Value)] != null)
                {
                    TaxDetailsdt = (DataTable)Session["SI_TaxDetails" + Convert.ToString(uniqueId.Value)];

                    #region Delete Igst,Cgst,Sgst respectively
                    //Get Company Gstin 09032017
                    string CompInternalId = Convert.ToString(Session["LastCompany"]);
                    string[] compGstin = oDBEngine.GetFieldValue1("tbl_master_company", "cmp_gstin", "cmp_internalid='" + CompInternalId + "'", 1);
                    string ShippingState = "";
                    //Rev Subhra 09-05-2019
                    if (lblShippingStateText.Value != "" && lblBillingStateText.Value != "")
                    {
                        if (ddl_PosGst.Value == "S")
                        {
                            ShippingState = lblShippingStateText.Value;
                            ShippingState = ShippingState.Substring(ShippingState.IndexOf("(State Code:")).Replace("(State Code:", "").Replace(")", "");
                        }
                        else
                        {
                            ShippingState = lblBillingStateText.Value;
                            ShippingState = ShippingState.Substring(ShippingState.IndexOf("(State Code:")).Replace("(State Code:", "").Replace(")", "");
                        }
                    }
                    //End of Rev

                    if (ShippingState.Trim() != "" && compGstin[0].Trim() != "")
                    {
                        if (compGstin.Length > 0)
                        {
                            if (compGstin[0].Substring(0, 2) == ShippingState)
                            {
                                //Check if the state is in union territories then only UTGST will apply
                                //   Chandigarh     Andaman and Nicobar Islands    DADRA & NAGAR HAVELI    DAMAN & DIU             DELHI                  Lakshadweep              PONDICHERRY
                                if (ShippingState == "4" || ShippingState == "26" || ShippingState == "25" || ShippingState == "7" || ShippingState == "31" || ShippingState == "34")
                                {
                                    foreach (DataRow dr in TaxDetailsdt.Rows)
                                    {
                                        if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST")
                                        {
                                            dr.Delete();
                                        }
                                    }

                                }
                                else
                                {
                                    foreach (DataRow dr in TaxDetailsdt.Rows)
                                    {
                                        if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
                                        {
                                            dr.Delete();
                                        }
                                    }
                                }
                                TaxDetailsdt.AcceptChanges();
                            }
                            else
                            {
                                foreach (DataRow dr in TaxDetailsdt.Rows)
                                {
                                    if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
                                    {
                                        dr.Delete();
                                    }
                                }
                                TaxDetailsdt.AcceptChanges();

                            }

                        }
                    }

                    //If Company GSTIN is blank then Delete All CGST,UGST,IGST,CGST
                    if (compGstin[0].Trim() == "")
                    {
                        foreach (DataRow dr in TaxDetailsdt.Rows)
                        {
                            if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST")
                            {
                                dr.Delete();
                            }
                        }
                        TaxDetailsdt.AcceptChanges();
                    }

                    #endregion






                    //gridTax.DataSource = GetTaxes();
                    var taxlist = (List<Taxes>)GetTaxes(TaxDetailsdt);
                    var taxChargeDataSource = setChargeCalculatedOn(taxlist, TaxDetailsdt);
                    gridTax.DataSource = taxChargeDataSource;
                    gridTax.DataBind();
                    gridTax.JSProperties["cpJsonChargeData"] = createJsonForChargesTax(TaxDetailsdt);
                }
            }
            else if (strSplitCommand == "SaveGst")
            {
                DataTable TaxDetailsdt = new DataTable();
                if (Session["SI_TaxDetails" + Convert.ToString(uniqueId.Value)] != null)
                {
                    TaxDetailsdt = (DataTable)Session["SI_TaxDetails" + Convert.ToString(uniqueId.Value)];
                }
                else
                {
                    TaxDetailsdt.Columns.Add("Taxes_ID", typeof(string));
                    TaxDetailsdt.Columns.Add("Taxes_Name", typeof(string));
                    TaxDetailsdt.Columns.Add("Percentage", typeof(string));
                    TaxDetailsdt.Columns.Add("Amount", typeof(string));
                    //ForGst
                    TaxDetailsdt.Columns.Add("AltTax_Code", typeof(string));
                }
                DataRow[] existingRow = TaxDetailsdt.Select("Taxes_ID='0'");
                if (Convert.ToString(cmbGstCstVatcharge.Value) == "0")
                {
                    if (existingRow.Length > 0)
                    {
                        TaxDetailsdt.Rows.Remove(existingRow[0]);
                    }
                }
                else
                {
                    if (existingRow.Length > 0)
                    {
                        existingRow[0]["Percentage"] = (cmbGstCstVatcharge.Value != null) ? Convert.ToString(cmbGstCstVatcharge.Value).Split('~')[1] : "0";
                        existingRow[0]["Amount"] = txtGstCstVatCharge.Text;
                        existingRow[0]["AltTax_Code"] = (cmbGstCstVatcharge.Value != null) ? Convert.ToString(cmbGstCstVatcharge.Value).Split('~')[0] : "0";

                    }
                    else
                    {
                        string GstTaxId = (cmbGstCstVatcharge.Value != null) ? Convert.ToString(cmbGstCstVatcharge.Value).Split('~')[0] : "0";
                        string GstPerCentage = (cmbGstCstVatcharge.Value != null) ? Convert.ToString(cmbGstCstVatcharge.Value).Split('~')[1] : "0";

                        string GstAmount = txtGstCstVatCharge.Text;
                        DataRow gstRow = TaxDetailsdt.NewRow();
                        gstRow["Taxes_ID"] = 0;
                        gstRow["Taxes_Name"] = cmbGstCstVatcharge.Text;
                        gstRow["Percentage"] = GstPerCentage;
                        gstRow["Amount"] = GstAmount;
                        gstRow["AltTax_Code"] = GstTaxId;
                        TaxDetailsdt.Rows.Add(gstRow);
                    }

                    Session["SI_TaxDetails" + Convert.ToString(uniqueId.Value)] = TaxDetailsdt;
                }
            }
        }
        protected void gridTax_RowInserting(object sender, ASPxDataInsertingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void gridTax_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void gridTax_RowDeleting(object sender, ASPxDataDeletingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void gridTax_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            DataTable TaxDetailsdt = new DataTable();
            if (Session["SI_TaxDetails" + Convert.ToString(uniqueId.Value)] != null)
            {
                TaxDetailsdt = (DataTable)Session["SI_TaxDetails" + Convert.ToString(uniqueId.Value)];
            }
            else
            {
                TaxDetailsdt.Columns.Add("Taxes_ID", typeof(string));
                TaxDetailsdt.Columns.Add("Taxes_Name", typeof(string));
                TaxDetailsdt.Columns.Add("Percentage", typeof(string));
                TaxDetailsdt.Columns.Add("Amount", typeof(string));
                //ForGst
                TaxDetailsdt.Columns.Add("AltTax_Code", typeof(string));
            }

            foreach (var args in e.UpdateValues)
            {
                string TaxID = Convert.ToString(args.Keys["TaxID"]);
                string TaxName = Convert.ToString(args.NewValues["TaxName"]);
                string Percentage = Convert.ToString(args.NewValues["Percentage"]);
                string Amount = Convert.ToString(args.NewValues["Amount"]);

                bool Isexists = false;
                foreach (DataRow drr in TaxDetailsdt.Rows)
                {
                    string OldTaxID = Convert.ToString(drr["Taxes_ID"]);

                    if (OldTaxID == TaxID)
                    {
                        Isexists = true;

                        drr["Percentage"] = Percentage;
                        drr["Amount"] = Amount;

                        break;
                    }
                }

                if (Isexists == false)
                {
                    TaxDetailsdt.Rows.Add(TaxID, TaxName, Percentage, Amount, 0);
                }
            }

            if (cmbGstCstVatcharge.Value != null)
            {
                DataRow[] existingRow = TaxDetailsdt.Select("Taxes_ID='0'");
                if (Convert.ToString(cmbGstCstVatcharge.Value) == "0")
                {
                    if (existingRow.Length > 0)
                    {
                        TaxDetailsdt.Rows.Remove(existingRow[0]);
                    }
                }
                else
                {
                    if (existingRow.Length > 0)
                    {

                        existingRow[0]["Percentage"] = Convert.ToString(cmbGstCstVatcharge.Value).Split('~')[1];
                        existingRow[0]["Amount"] = txtGstCstVatCharge.Text;
                        existingRow[0]["AltTax_Code"] = Convert.ToString(cmbGstCstVatcharge.Value).Split('~')[0]; ;

                    }
                    else
                    {
                        string GstTaxId = Convert.ToString(cmbGstCstVatcharge.Value).Split('~')[0];
                        string GstPerCentage = Convert.ToString(cmbGstCstVatcharge.Value).Split('~')[1];
                        string GstAmount = txtGstCstVatCharge.Text;
                        DataRow gstRow = TaxDetailsdt.NewRow();
                        gstRow["Taxes_ID"] = 0;
                        gstRow["Taxes_Name"] = cmbGstCstVatcharge.Text;
                        gstRow["Percentage"] = GstPerCentage;
                        gstRow["Amount"] = GstAmount;
                        gstRow["AltTax_Code"] = GstTaxId;
                        TaxDetailsdt.Rows.Add(gstRow);
                    }
                }
            }

            Session["SI_TaxDetails" + Convert.ToString(uniqueId.Value)] = TaxDetailsdt;

            gridTax.DataSource = GetTaxes(TaxDetailsdt);
            gridTax.DataBind();
        }



        protected void gridTax_DataBinding(object sender, EventArgs e)
        {
            if (Session["SI_TaxDetails" + Convert.ToString(uniqueId.Value)] != null)
            {
                DataTable TaxDetailsdt = (DataTable)Session["SI_TaxDetails" + Convert.ToString(uniqueId.Value)];

                //gridTax.DataSource = GetTaxes();
                var taxlist = (List<Taxes>)GetTaxes(TaxDetailsdt);
                var taxChargeDataSource = setChargeCalculatedOn(taxlist, TaxDetailsdt);
                gridTax.DataSource = taxChargeDataSource;
            }
        }


        #endregion

        #region Warehouse Details

        public DataTable GetQuotationWarehouseData()
        {
            try
            {
                MasterSettings masterBl = new MasterSettings();
                string multiwarehouse = Convert.ToString(masterBl.GetSettings("IaMultilevelWarehouse"));
                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesInvoice_Details");
                proc.AddVarcharPara("@Action", 500, "InvoiceWarehouse");
                proc.AddVarcharPara("@InvoiceID", 500, InvoiceId);
                proc.AddVarcharPara("@Multiwarehouse", 500, Convert.ToString(multiwarehouse));
                dt = proc.GetTable();

                string strNewVal = "", strOldVal = "";
                DataTable tempdt = dt.Copy();
                foreach (DataRow drr in tempdt.Rows)
                {
                    strNewVal = Convert.ToString(drr["QuoteWarehouse_Id"]);

                    if (strNewVal == strOldVal)
                    {
                        drr["WarehouseName"] = "";
                        drr["Quantity"] = "";
                        drr["BatchNo"] = "";
                        drr["SalesUOMName"] = "";
                        drr["SalesQuantity"] = "";
                        drr["StkUOMName"] = "";
                        drr["StkQuantity"] = "";
                        drr["ConversionMultiplier"] = "";
                        drr["AvailableQty"] = "";
                        drr["BalancrStk"] = "";
                        drr["MfgDate"] = "";
                        drr["ExpiryDate"] = "";
                    }

                    strOldVal = strNewVal;
                }

                Session["SI_LoopWarehouse" + Convert.ToString(uniqueId.Value)] = Convert.ToString(Convert.ToInt32(strNewVal) + 1);
                tempdt.Columns.Remove("QuoteWarehouse_Id");
                return tempdt;
            }
            catch
            {
                return null;
            }
        }
        public DataTable GetTaggingWarehouseData(string ComponentDetailsIDs, string strType)
        {
            try
            {
                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesInvoice_Details");
                proc.AddVarcharPara("@Action", 500, "ComponentWarehouse");
                proc.AddVarcharPara("@SelectedComponentList", 2000, ComponentDetailsIDs);
                proc.AddVarcharPara("@ComponentType", 10, strType);
                dt = proc.GetTable();

                string strNewVal = "", strOldVal = "";
                DataTable tempdt = dt.Copy();
                foreach (DataRow drr in tempdt.Rows)
                {
                    strNewVal = Convert.ToString(drr["QuoteWarehouse_Id"]);

                    if (strNewVal == strOldVal)
                    {
                        drr["WarehouseName"] = "";
                        drr["Quantity"] = "";
                        drr["BatchNo"] = "";
                        drr["SalesUOMName"] = "";
                        drr["SalesQuantity"] = "";
                        drr["StkUOMName"] = "";
                        drr["StkQuantity"] = "";
                        drr["ConversionMultiplier"] = "";
                        drr["AvailableQty"] = "";
                        drr["BalancrStk"] = "";
                        drr["MfgDate"] = "";
                        drr["ExpiryDate"] = "";
                    }

                    strOldVal = strNewVal;
                }

                Session["SI_LoopWarehouse" + Convert.ToString(uniqueId.Value)] = Convert.ToString(Convert.ToInt32(strNewVal) + 1);
                tempdt.Columns.Remove("QuoteWarehouse_Id");
                return tempdt;
            }
            catch
            {
                return null;
            }
        }
        protected void CmbWarehouse_Callback(object sender, CallbackEventArgsBase e)
        {
            string WhichCall = e.Parameter.Split('~')[0];
            if (WhichCall == "BindWarehouse")
            {
                DataTable dt = GetWarehouseData();

                CmbWarehouse.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    CmbWarehouse.Items.Add(Convert.ToString(dt.Rows[i]["WarehouseName"]), Convert.ToString(dt.Rows[i]["WarehouseID"]));
                }
            }
        }
        protected void CmbBatch_Callback(object sender, CallbackEventArgsBase e)
        {
            string WhichCall = e.Parameter.Split('~')[0];
            if (WhichCall == "BindBatch")
            {
                string WarehouseID = Convert.ToString(e.Parameter.Split('~')[1]);
                DataTable dt = GetBatchData(WarehouseID);

                CmbBatch.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    CmbBatch.Items.Add(Convert.ToString(dt.Rows[i]["BatchName"]), Convert.ToString(dt.Rows[i]["BatchID"]));
                }
            }
        }
        protected void CmbSerial_Callback(object sender, CallbackEventArgsBase e)
        {
            string WhichCall = e.Parameter.Split('~')[0];
            if (WhichCall == "BindSerial")
            {
                string WarehouseID = Convert.ToString(e.Parameter.Split('~')[1]);
                string BatchID = Convert.ToString(e.Parameter.Split('~')[2]);
                //DataTable dt = GetSerialata(WarehouseID, BatchID);
                DataTable dt = GetSerialataNew(WarehouseID, BatchID);
                if (Session["SI_WarehouseData" + Convert.ToString(uniqueId.Value)] != null)
                {
                    DataTable Warehousedt = (DataTable)Session["SI_WarehouseData" + Convert.ToString(uniqueId.Value)];
                    DataTable tempdt = Warehousedt.DefaultView.ToTable(false, "SerialID");

                    foreach (DataRow dr in dt.Rows)
                    {
                        string SerialID = Convert.ToString(dr["SerialID"]);
                        DataRow[] rows = tempdt.Select("SerialID = '" + SerialID + "' AND SerialID<>'0'");

                        if (rows != null && rows.Length > 0)
                        {
                            dr.Delete();
                        }

                        //foreach (DataRow drr in tempdt.Rows)
                        //{
                        //    string oldSerialID = Convert.ToString(drr["SerialID"]);
                        //    if (newSerialID == oldSerialID)
                        //    {
                        //        dr.Delete();
                        //    }
                        //}

                    }
                    dt.AcceptChanges();

                }

                ASPxListBox lb = sender as ASPxListBox;
                lb.DataSource = dt;
                lb.ValueField = "SerialID";
                lb.TextField = "SerialName";
                lb.ValueType = typeof(string);
                lb.DataBind();
            }
            else if (WhichCall == "EditSerial")
            {
                string WarehouseID = Convert.ToString(e.Parameter.Split('~')[1]);
                string BatchID = Convert.ToString(e.Parameter.Split('~')[2]);
                string editSerialID = Convert.ToString(e.Parameter.Split('~')[3]);
                //DataTable dt =  GetSerialata(WarehouseID, BatchID);
                DataTable dt = GetSerialataNew(WarehouseID, BatchID);
                if (Session["SI_WarehouseData" + Convert.ToString(uniqueId.Value)] != null)
                {
                    DataTable Warehousedt = (DataTable)Session["SI_WarehouseData" + Convert.ToString(uniqueId.Value)];
                    DataTable tempdt = Warehousedt.DefaultView.ToTable(false, "SerialID");

                    foreach (DataRow dr in dt.Rows)
                    {
                        string SerialID = Convert.ToString(dr["SerialID"]);
                        DataRow[] rows = tempdt.Select("SerialID = '" + SerialID + "' AND SerialID not in ('0','" + editSerialID + "')");

                        if (rows != null && rows.Length > 0)
                        {
                            dr.Delete();
                        }

                        //foreach (DataRow drr in tempdt.Rows)
                        //{
                        //    string oldSerialID = Convert.ToString(drr["SerialID"]);
                        //    if (newSerialID == oldSerialID)
                        //    {
                        //        dr.Delete();
                        //    }
                        //}

                    }
                    dt.AcceptChanges();

                }

                ASPxListBox lb = sender as ASPxListBox;
                lb.DataSource = dt;
                lb.ValueField = "SerialID";
                lb.TextField = "SerialName";
                lb.ValueType = typeof(string);
                lb.DataBind();
            }
        }
        protected void listBox_Init(object sender, EventArgs e)
        {
            ASPxListBox lb = sender as ASPxListBox;
            DataTable dt = GetSerialata("", "");

            lb.DataSource = dt;
            lb.ValueField = "SerialID";
            lb.TextField = "SerialName";
            lb.ValueType = typeof(string);
            lb.DataBind();
        }
        protected void GrdWarehouse_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GrdWarehouse.JSProperties["cpIsSave"] = "";
            string strSplitCommand = e.Parameters.Split('~')[0];
            string Type = "";

            if (strSplitCommand == "Display")
            {
                GetProductType(ref Type);
                string ProductID = Convert.ToString(hdfProductID.Value);
                string SerialID = Convert.ToString(e.Parameters.Split('~')[1]);

                DataTable Warehousedt = new DataTable();
                if (Session["SI_WarehouseData" + Convert.ToString(uniqueId.Value)] != null)
                {
                    Warehousedt = (DataTable)Session["SI_WarehouseData" + Convert.ToString(uniqueId.Value)];
                }
                else
                {
                    Warehousedt.Columns.Add("Product_SrlNo", typeof(string));
                    Warehousedt.Columns.Add("SrlNo", typeof(string));
                    Warehousedt.Columns.Add("WarehouseID", typeof(string));
                    Warehousedt.Columns.Add("WarehouseName", typeof(string));
                    Warehousedt.Columns.Add("Quantity", typeof(string));
                    Warehousedt.Columns.Add("BatchID", typeof(string));
                    Warehousedt.Columns.Add("BatchNo", typeof(string));
                    Warehousedt.Columns.Add("SerialID", typeof(string));
                    Warehousedt.Columns.Add("SerialNo", typeof(string));
                    Warehousedt.Columns.Add("SalesUOMName", typeof(string));
                    Warehousedt.Columns.Add("SalesUOMCode", typeof(string));
                    Warehousedt.Columns.Add("SalesQuantity", typeof(string));
                    Warehousedt.Columns.Add("StkUOMName", typeof(string));
                    Warehousedt.Columns.Add("StkUOMCode", typeof(string));
                    Warehousedt.Columns.Add("StkQuantity", typeof(string));
                    Warehousedt.Columns.Add("ConversionMultiplier", typeof(string));
                    Warehousedt.Columns.Add("AvailableQty", typeof(string));
                    Warehousedt.Columns.Add("BalancrStk", typeof(string));
                    Warehousedt.Columns.Add("MfgDate", typeof(string));
                    Warehousedt.Columns.Add("ExpiryDate", typeof(string));
                    Warehousedt.Columns.Add("LoopID", typeof(string));
                    Warehousedt.Columns.Add("TotalQuantity", typeof(string));
                    Warehousedt.Columns.Add("Status", typeof(string));
                    Warehousedt.Columns.Add("AltQty", typeof(string));
                }

                if (Warehousedt != null && Warehousedt.Rows.Count > 0)
                {
                    DataView dvData = new DataView(Warehousedt);
                    dvData.RowFilter = "Product_SrlNo = '" + SerialID + "'";

                    GrdWarehouse.DataSource = dvData;
                    GrdWarehouse.DataBind();
                }
                else
                {
                    GrdWarehouse.DataSource = Warehousedt.DefaultView;
                    GrdWarehouse.DataBind();
                }
                // changeGridOrder();
            }
            else if (strSplitCommand == "SaveDisplay")
            {
                int loopId = Convert.ToInt32(Session["SI_LoopWarehouse" + Convert.ToString(uniqueId.Value)]);

                string WarehouseID = Convert.ToString(e.Parameters.Split('~')[1]);
                string WarehouseName = Convert.ToString(e.Parameters.Split('~')[2]);
                string BatchID = Convert.ToString(e.Parameters.Split('~')[3]);
                string BatchName = Convert.ToString(e.Parameters.Split('~')[4]);
                string SerialID = Convert.ToString(e.Parameters.Split('~')[5]);
                string SerialName = Convert.ToString(e.Parameters.Split('~')[6]);
                string ProductID = Convert.ToString(hdfProductID.Value);
                string ProductSerialID = Convert.ToString(hdfProductSerialID.Value);
                string Qty = Convert.ToString(e.Parameters.Split('~')[7]);
                string editWarehouseID = Convert.ToString(e.Parameters.Split('~')[8]);
                string AltQty = Convert.ToString(e.Parameters.Split('~')[9]);


                string Sales_UOM_Name = "", Sales_UOM_Code = "", Stk_UOM_Name = "", Stk_UOM_Code = "", Conversion_Multiplier = "", Trans_Stock = "0", MfgDate = "", ExpiryDate = "";
                GetProductType(ref Type);

                DataTable Warehousedt = new DataTable();
                if (Session["SI_WarehouseData" + Convert.ToString(uniqueId.Value)] != null)
                {
                    Warehousedt = (DataTable)Session["SI_WarehouseData" + Convert.ToString(uniqueId.Value)];
                }
                else
                {
                    Warehousedt.Columns.Add("Product_SrlNo", typeof(string));
                    Warehousedt.Columns.Add("SrlNo", typeof(string));
                    Warehousedt.Columns.Add("WarehouseID", typeof(string));
                    Warehousedt.Columns.Add("WarehouseName", typeof(string));
                    Warehousedt.Columns.Add("Quantity", typeof(string));
                    Warehousedt.Columns.Add("BatchID", typeof(string));
                    Warehousedt.Columns.Add("BatchNo", typeof(string));
                    Warehousedt.Columns.Add("SerialID", typeof(string));
                    Warehousedt.Columns.Add("SerialNo", typeof(string));
                    Warehousedt.Columns.Add("SalesUOMName", typeof(string));
                    Warehousedt.Columns.Add("SalesUOMCode", typeof(string));
                    Warehousedt.Columns.Add("SalesQuantity", typeof(string));
                    Warehousedt.Columns.Add("StkUOMName", typeof(string));
                    Warehousedt.Columns.Add("StkUOMCode", typeof(string));
                    Warehousedt.Columns.Add("StkQuantity", typeof(string));
                    Warehousedt.Columns.Add("ConversionMultiplier", typeof(string));
                    Warehousedt.Columns.Add("AvailableQty", typeof(string));
                    Warehousedt.Columns.Add("BalancrStk", typeof(string));
                    Warehousedt.Columns.Add("MfgDate", typeof(string));
                    Warehousedt.Columns.Add("ExpiryDate", typeof(string));
                    Warehousedt.Columns.Add("LoopID", typeof(string));
                    Warehousedt.Columns.Add("TotalQuantity", typeof(string));
                    Warehousedt.Columns.Add("Status", typeof(string));
                    Warehousedt.Columns.Add("AltQty", typeof(string));
                }

                bool IsDelete = false;

                if (Type == "WBS")
                {
                    GetTotalStock(ref Trans_Stock, WarehouseID);
                    GetProductUOM(ref Sales_UOM_Name, ref Sales_UOM_Code, ref Stk_UOM_Name, ref Stk_UOM_Code, ref Conversion_Multiplier, ProductID);
                    GetBatchDetails(ref MfgDate, ref ExpiryDate, BatchID);

                    string[] SerialIDList = SerialID.Split(new string[] { "||@||" }, StringSplitOptions.None);
                    string[] SerialNameList = SerialName.Split(new string[] { "||@||" }, StringSplitOptions.None);

                    if (editWarehouseID == "0")
                    {
                        for (int i = 0; i < SerialIDList.Length; i++)
                        {
                            string strSrlID = SerialIDList[i];
                            string strSrlName = SerialNameList[i];


                            if (i == 0)
                            {
                                decimal ConversionMultiplier = Convert.ToDecimal(Conversion_Multiplier);
                                string stkqtn = Convert.ToString(Math.Round((SerialIDList.Length * ConversionMultiplier), 2));
                                decimal BalanceStk = Convert.ToDecimal(Trans_Stock) - Convert.ToDecimal(stkqtn);
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";

                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, SerialIDList.Length, BatchID, BatchName, strSrlID, strSrlName, Sales_UOM_Name, Sales_UOM_Code, SerialIDList.Length + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, SerialIDList.Length, "D", AltQty);
                            }
                            else
                            {
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, "", "", BatchID, "", strSrlID, strSrlName, "", Sales_UOM_Code, "", "", Stk_UOM_Code, "", "", "", "", "", "", loopId, SerialIDList.Length, "D");
                            }
                        }
                    }
                    else
                    {
                        string strLoopID = "0", strSerial = "0";

                        DataRow[] result = Warehousedt.Select("SrlNo ='" + editWarehouseID + "'");
                        foreach (DataRow row in result)
                        {
                            strSerial = Convert.ToString(row["SerialID"]);
                            strLoopID = Convert.ToString(row["LoopID"]);
                        }

                        int count = 0;
                        DataRow[] dr = Warehousedt.Select("LoopID ='" + strLoopID + "'");
                        int value = (dr.Length + SerialIDList.Length - 1);

                        string[] temp_SerialIDList = new string[value];
                        string[] temp_SerialNameList = new string[value];

                        string[] check_SerialIDList = new string[value];
                        string[] check_SerialNameList = new string[value];

                        foreach (DataRow rows in dr)
                        {
                            if (strSerial != Convert.ToString(rows["SerialID"]))
                            {
                                temp_SerialIDList[count] = Convert.ToString(rows["SerialID"]);
                                temp_SerialNameList[count] = Convert.ToString(rows["SerialNo"]);

                                check_SerialIDList[count] = Convert.ToString(rows["SerialID"]);
                                check_SerialNameList[count] = Convert.ToString(rows["SerialNo"]);

                                count++;
                            }
                        }

                        for (int i = 0; i < SerialIDList.Length; i++)
                        {
                            temp_SerialIDList[count] = Convert.ToString(SerialIDList[i]);
                            temp_SerialNameList[count] = Convert.ToString(SerialNameList[i]);

                            count++;
                        }

                        DataRow[] delResult = Warehousedt.Select("LoopID ='" + strLoopID + "'");
                        foreach (DataRow delrow in delResult)
                        {
                            delrow.Delete();
                        }
                        Warehousedt.AcceptChanges();

                        SerialIDList = temp_SerialIDList;
                        SerialNameList = temp_SerialNameList;

                        for (int i = 0; i < SerialIDList.Length; i++)
                        {
                            string strSrlID = SerialIDList[i];
                            string strSrlName = SerialNameList[i];
                            string repute = "D";
                            if (check_SerialIDList.Contains(strSrlID)) repute = "I";

                            if (i == 0)
                            {
                                decimal ConversionMultiplier = Convert.ToDecimal(Conversion_Multiplier);
                                string stkqtn = Convert.ToString(Math.Round((SerialIDList.Length * ConversionMultiplier), 2));
                                decimal BalanceStk = Convert.ToDecimal(Trans_Stock) - Convert.ToDecimal(stkqtn);
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";

                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, SerialIDList.Length, BatchID, BatchName, strSrlID, strSrlName, Sales_UOM_Name, Sales_UOM_Code, SerialIDList.Length + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, strLoopID, SerialIDList.Length, repute, AltQty);
                            }
                            else
                            {
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, "", "", BatchID, "", strSrlID, strSrlName, "", Sales_UOM_Code, "", "", Stk_UOM_Code, "", "", "", "", "", "", strLoopID, SerialIDList.Length, repute);
                            }
                        }
                    }
                }
                else if (Type == "W")
                {
                    GetTotalStock(ref Trans_Stock, WarehouseID);
                    GetProductUOM(ref Sales_UOM_Name, ref Sales_UOM_Code, ref Stk_UOM_Name, ref Stk_UOM_Code, ref Conversion_Multiplier, ProductID);

                    decimal ConversionMultiplier = Convert.ToDecimal(Conversion_Multiplier);
                    string stkqtn = Convert.ToString(Math.Round((Convert.ToDecimal(Qty) * ConversionMultiplier), 2));
                    decimal BalanceStk = Convert.ToDecimal(Trans_Stock) - Convert.ToDecimal(stkqtn);
                    BatchID = "0";

                    if (editWarehouseID == "0")
                    {
                        string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";
                        var updaterows = Warehousedt.Select("WarehouseID ='" + WarehouseID + "' AND Product_SrlNo='" + ProductSerialID + "'");

                        if (updaterows.Length == 0)
                        {
                            Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, Qty, BatchID, BatchName, "0", "", Sales_UOM_Name, Sales_UOM_Code, Convert.ToDecimal(Qty) + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, Qty, "D", AltQty);
                        }
                        else
                        {
                            foreach (var row in updaterows)
                            {
                                decimal oldQuantity = Convert.ToDecimal(row["Quantity"]);
                                decimal oldAltQty = Convert.ToDecimal(row["AltQty"]);
                                row["AltQty"] = (oldAltQty + Convert.ToDecimal(AltQty));
                                row["Quantity"] = (oldQuantity + Convert.ToDecimal(Qty));

                                row["TotalQuantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                row["SalesQuantity"] = (oldQuantity + Convert.ToDecimal(Qty)) + " " + Sales_UOM_Name;
                            }
                        }
                    }
                    else
                    {
                        var rows = Warehousedt.Select("WarehouseID ='" + WarehouseID + "' AND Convert(TotalQuantity, 'System.Decimal')='" + Qty + "' AND SrlNo='" + editWarehouseID + "'");

                        if (rows.Length == 0)
                        {
                            string whID = "";
                            string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";

                            var updaterows = Warehousedt.Select("WarehouseID ='" + WarehouseID + "' AND Product_SrlNo='" + ProductSerialID + "'");
                            foreach (var row in updaterows)
                            {
                                whID = Convert.ToString(row["SrlNo"]);
                            }

                            if (updaterows.Length == 0)
                            {
                                IsDelete = true;
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, Qty, BatchID, BatchName, "0", "", Sales_UOM_Name, Sales_UOM_Code, Convert.ToDecimal(Qty) + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, Qty, "D", AltQty);

                            }
                            else if (editWarehouseID == whID)
                            {
                                foreach (var row in updaterows)
                                {
                                    whID = Convert.ToString(row["SrlNo"]);
                                    decimal oldQuantity = Convert.ToDecimal(row["Quantity"]);
                                    decimal oldAltQty = Convert.ToDecimal(row["AltQty"]);
                                    row["AltQty"] = (oldAltQty + Convert.ToDecimal(AltQty));
                                    row["Quantity"] = Qty;
                                    row["TotalQuantity"] = Qty;
                                    row["SalesQuantity"] = Qty + " " + Sales_UOM_Name;
                                }
                            }
                            else if (editWarehouseID != whID)
                            {
                                IsDelete = true;
                                foreach (var row in updaterows)
                                {
                                    ID = Convert.ToString(row["SrlNo"]);
                                    decimal oldQuantity = Convert.ToDecimal(row["Quantity"]);
                                    decimal oldAltQty = Convert.ToDecimal(row["AltQty"]);
                                    row["AltQty"] = (oldAltQty + Convert.ToDecimal(AltQty));
                                    row["Quantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                    row["TotalQuantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                    row["SalesQuantity"] = (oldQuantity + Convert.ToDecimal(Qty)) + " " + Sales_UOM_Name;
                                }
                            }
                        }
                    }
                }
                else if (Type == "WB")
                {
                    GetTotalStock(ref Trans_Stock, WarehouseID);
                    GetProductUOM(ref Sales_UOM_Name, ref Sales_UOM_Code, ref Stk_UOM_Name, ref Stk_UOM_Code, ref Conversion_Multiplier, ProductID);
                    GetBatchDetails(ref MfgDate, ref ExpiryDate, BatchID);

                    decimal ConversionMultiplier = Convert.ToDecimal(Conversion_Multiplier);
                    string stkqtn = Convert.ToString(Math.Round((Convert.ToDecimal(Qty) * ConversionMultiplier), 2));
                    decimal BalanceStk = Convert.ToDecimal(Trans_Stock) - Convert.ToDecimal(stkqtn);

                    if (editWarehouseID == "0")
                    {
                        string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";
                        var updaterows = Warehousedt.Select("WarehouseID ='" + WarehouseID + "' AND BatchID='" + BatchID + "' AND Product_SrlNo='" + ProductSerialID + "'");

                        if (updaterows.Length == 0)
                        {
                            Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, Qty, BatchID, BatchName, "0", "", Sales_UOM_Name, Sales_UOM_Code, Convert.ToDecimal(Qty) + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, Qty, "D", AltQty);
                        }
                        else
                        {
                            foreach (var row in updaterows)
                            {
                                decimal oldQuantity = Convert.ToDecimal(row["Quantity"]);
                                decimal oldAltQty = Convert.ToDecimal(row["AltQty"]);
                                row["AltQty"] = (oldAltQty + Convert.ToDecimal(AltQty));
                                row["Quantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                row["TotalQuantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                row["SalesQuantity"] = (oldQuantity + Convert.ToDecimal(Qty)) + " " + Sales_UOM_Name;
                            }
                        }
                    }
                    else
                    {
                        var rows = Warehousedt.Select("WarehouseID ='" + WarehouseID + "' AND BatchID='" + BatchID + "' AND Convert(TotalQuantity, 'System.Decimal')='" + Qty + "' AND SrlNo='" + editWarehouseID + "'");
                        if (rows.Length == 0)
                        {
                            string whID = "";
                            string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";

                            var updaterows = Warehousedt.Select("WarehouseID ='" + WarehouseID + "' AND BatchID='" + BatchID + "' AND Product_SrlNo='" + ProductSerialID + "'");
                            foreach (var row in updaterows)
                            {
                                whID = Convert.ToString(row["SrlNo"]);
                            }

                            if (updaterows.Length == 0)
                            {
                                IsDelete = true;
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, Qty, BatchID, BatchName, "0", "", Sales_UOM_Name, Sales_UOM_Code, Convert.ToDecimal(Qty) + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, Qty, "D", AltQty);
                            }
                            else if (editWarehouseID == whID)
                            {
                                foreach (var row in updaterows)
                                {
                                    whID = Convert.ToString(row["SrlNo"]);
                                    decimal oldQuantity = Convert.ToDecimal(row["Quantity"]);

                                    row["Quantity"] = Qty;
                                    row["TotalQuantity"] = Qty;
                                    row["SalesQuantity"] = Qty + " " + Sales_UOM_Name;
                                }
                            }
                            else if (editWarehouseID != whID)
                            {
                                IsDelete = true;
                                foreach (var row in updaterows)
                                {
                                    ID = Convert.ToString(row["SrlNo"]);
                                    decimal oldQuantity = Convert.ToDecimal(row["Quantity"]);
                                    decimal oldAltQty = Convert.ToDecimal(row["AltQty"]);
                                    row["AltQty"] = (oldAltQty + Convert.ToDecimal(AltQty));
                                    row["Quantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                    row["TotalQuantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                    row["SalesQuantity"] = (oldQuantity + Convert.ToDecimal(Qty)) + " " + Sales_UOM_Name;
                                }
                            }
                        }
                    }
                }
                else if (Type == "B")
                {
                    GetProductUOM(ref Sales_UOM_Name, ref Sales_UOM_Code, ref Stk_UOM_Name, ref Stk_UOM_Code, ref Conversion_Multiplier, ProductID);
                    GetBatchDetails(ref MfgDate, ref ExpiryDate, BatchID);

                    decimal ConversionMultiplier = Convert.ToDecimal(Conversion_Multiplier);
                    string stkqtn = Convert.ToString(Math.Round((Convert.ToDecimal(Qty) * ConversionMultiplier), 2));
                    decimal BalanceStk = Convert.ToDecimal(Trans_Stock) - Convert.ToDecimal(stkqtn);
                    WarehouseID = "0";


                    if (editWarehouseID == "0")
                    {
                        string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";
                        var updaterows = Warehousedt.Select("BatchID ='" + BatchID + "' AND Product_SrlNo='" + ProductSerialID + "'");

                        if (updaterows.Length == 0)
                        {
                            Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, Qty, BatchID, BatchName, "0", "", Sales_UOM_Name, Sales_UOM_Code, Convert.ToDecimal(Qty) + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, Qty, "D", AltQty);
                        }
                        else
                        {
                            foreach (var row in updaterows)
                            {
                                decimal oldQuantity = Convert.ToDecimal(row["Quantity"]);
                                decimal oldAltQty = Convert.ToDecimal(row["AltQty"]);
                                row["AltQty"] = (oldAltQty + Convert.ToDecimal(AltQty));
                                row["Quantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                row["TotalQuantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                row["SalesQuantity"] = (oldQuantity + Convert.ToDecimal(Qty)) + " " + Sales_UOM_Name;
                            }
                        }
                    }
                    else
                    {
                        var rows = Warehousedt.Select("BatchID='" + BatchID + "' AND Convert(TotalQuantity, 'System.Decimal')='" + Qty + "' AND SrlNo='" + editWarehouseID + "'");
                        if (rows.Length == 0)
                        {
                            string whID = "";
                            string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";

                            var updaterows = Warehousedt.Select("BatchID ='" + BatchID + "' AND Product_SrlNo='" + ProductSerialID + "'");
                            foreach (var row in updaterows)
                            {
                                whID = Convert.ToString(row["SrlNo"]);
                            }

                            if (updaterows.Length == 0)
                            {
                                IsDelete = true;
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, Qty, BatchID, BatchName, "0", "", Sales_UOM_Name, Sales_UOM_Code, Convert.ToDecimal(Qty) + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, Qty, "D", AltQty);
                            }
                            else if (editWarehouseID == whID)
                            {
                                foreach (var row in updaterows)
                                {
                                    whID = Convert.ToString(row["SrlNo"]);
                                    decimal oldQuantity = Convert.ToDecimal(row["Quantity"]);

                                    row["Quantity"] = Qty;
                                    row["TotalQuantity"] = Qty;
                                    row["SalesQuantity"] = Qty + " " + Sales_UOM_Name;
                                }
                            }
                            else if (editWarehouseID != whID)
                            {
                                IsDelete = true;
                                foreach (var row in updaterows)
                                {
                                    ID = Convert.ToString(row["SrlNo"]);
                                    decimal oldQuantity = Convert.ToDecimal(row["Quantity"]);
                                    decimal oldAltQty = Convert.ToDecimal(row["AltQty"]);
                                    row["AltQty"] = (oldAltQty + Convert.ToDecimal(AltQty));
                                    row["Quantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                    row["TotalQuantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                    row["SalesQuantity"] = (oldQuantity + Convert.ToDecimal(Qty)) + " " + Sales_UOM_Name;
                                }
                            }
                        }
                    }
                }
                else if (Type == "S")
                {
                    GetProductUOM(ref Sales_UOM_Name, ref Sales_UOM_Code, ref Stk_UOM_Name, ref Stk_UOM_Code, ref Conversion_Multiplier, ProductID);

                    string[] SerialIDList = SerialID.Split(new string[] { "||@||" }, StringSplitOptions.None);
                    string[] SerialNameList = SerialName.Split(new string[] { "||@||" }, StringSplitOptions.None);

                    //Qty = Convert.ToString(SerialIDList.Length);
                    Qty = "1";
                    decimal ConversionMultiplier = Convert.ToDecimal(Conversion_Multiplier);
                    string stkqtn = Convert.ToString(Math.Round((Convert.ToDecimal(Qty) * ConversionMultiplier), 2));
                    decimal BalanceStk = Convert.ToDecimal(Trans_Stock) - Convert.ToDecimal(stkqtn);
                    WarehouseID = "0";
                    BatchID = "0";

                    for (int i = 0; i < SerialIDList.Length; i++)
                    {
                        string strSrlID = SerialIDList[i];
                        string strSrlName = SerialNameList[i];

                        if (editWarehouseID == "0")
                        {
                            string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";
                            Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, SerialIDList.Length, BatchID, BatchName, strSrlID, strSrlName, Sales_UOM_Name, Sales_UOM_Code, SerialIDList.Length + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, SerialIDList.Length, "D", AltQty);
                        }
                        else
                        {
                            var rows = Warehousedt.Select("SerialID ='" + strSrlID + "' AND SrlNo='" + editWarehouseID + "'");
                            if (rows.Length == 0)
                            {
                                IsDelete = true;
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, SerialIDList.Length, BatchID, BatchName, strSrlID, strSrlName, Sales_UOM_Name, Sales_UOM_Code, SerialIDList.Length + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, SerialIDList.Length, "D", AltQty);
                            }
                        }
                    }
                }
                else if (Type == "WS")
                {
                    GetTotalStock(ref Trans_Stock, WarehouseID);
                    GetProductUOM(ref Sales_UOM_Name, ref Sales_UOM_Code, ref Stk_UOM_Name, ref Stk_UOM_Code, ref Conversion_Multiplier, ProductID);
                    //GetBatchDetails(ref MfgDate, ref ExpiryDate, BatchID);

                    string[] SerialIDList = SerialID.Split(new string[] { "||@||" }, StringSplitOptions.None);
                    string[] SerialNameList = SerialName.Split(new string[] { "||@||" }, StringSplitOptions.None);

                    if (editWarehouseID == "0")
                    {
                        for (int i = 0; i < SerialIDList.Length; i++)
                        {
                            string strSrlID = SerialIDList[i];
                            string strSrlName = SerialNameList[i];


                            if (i == 0)
                            {
                                decimal ConversionMultiplier = Convert.ToDecimal(Conversion_Multiplier);
                                string stkqtn = Convert.ToString(Math.Round((SerialIDList.Length * ConversionMultiplier), 2));
                                decimal BalanceStk = Convert.ToDecimal(Trans_Stock) - Convert.ToDecimal(stkqtn);
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";

                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, SerialIDList.Length, "0", BatchName, strSrlID, strSrlName, Sales_UOM_Name, Sales_UOM_Code, SerialIDList.Length + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, SerialIDList.Length, "D", AltQty);
                            }
                            else
                            {
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, "", "", "0", "", strSrlID, strSrlName, "", Sales_UOM_Code, "", "", Stk_UOM_Code, "", "", "", "", "", "", loopId, SerialIDList.Length, "D", AltQty);
                            }
                        }
                    }
                    else
                    {
                        string strLoopID = "0", strSerial = "0";

                        DataRow[] result = Warehousedt.Select("SrlNo ='" + editWarehouseID + "'");
                        foreach (DataRow row in result)
                        {
                            strSerial = Convert.ToString(row["SerialID"]);
                            strLoopID = Convert.ToString(row["LoopID"]);
                        }

                        int count = 0;
                        DataRow[] dr = Warehousedt.Select("LoopID ='" + strLoopID + "'");
                        int value = (dr.Length + SerialIDList.Length - 1);

                        string[] temp_SerialIDList = new string[value];
                        string[] temp_SerialNameList = new string[value];

                        string[] check_SerialIDList = new string[value];
                        string[] check_SerialNameList = new string[value];

                        foreach (DataRow rows in dr)
                        {
                            if (strSerial != Convert.ToString(rows["SerialID"]))
                            {
                                temp_SerialIDList[count] = Convert.ToString(rows["SerialID"]);
                                temp_SerialNameList[count] = Convert.ToString(rows["SerialNo"]);

                                check_SerialIDList[count] = Convert.ToString(rows["SerialID"]);
                                check_SerialNameList[count] = Convert.ToString(rows["SerialNo"]);

                                count++;
                            }
                        }

                        for (int i = 0; i < SerialIDList.Length; i++)
                        {
                            temp_SerialIDList[count] = Convert.ToString(SerialIDList[i]);
                            temp_SerialNameList[count] = Convert.ToString(SerialNameList[i]);

                            count++;
                        }

                        DataRow[] delResult = Warehousedt.Select("LoopID ='" + strLoopID + "'");
                        foreach (DataRow delrow in delResult)
                        {
                            delrow.Delete();
                        }
                        Warehousedt.AcceptChanges();

                        SerialIDList = temp_SerialIDList;
                        SerialNameList = temp_SerialNameList;

                        for (int i = 0; i < SerialIDList.Length; i++)
                        {
                            string strSrlID = SerialIDList[i];
                            string strSrlName = SerialNameList[i];
                            string repute = "D";
                            if (check_SerialIDList.Contains(strSrlID)) repute = "I";

                            if (i == 0)
                            {
                                decimal ConversionMultiplier = Convert.ToDecimal(Conversion_Multiplier);
                                string stkqtn = Convert.ToString(Math.Round((SerialIDList.Length * ConversionMultiplier), 2));
                                decimal BalanceStk = Convert.ToDecimal(Trans_Stock) - Convert.ToDecimal(stkqtn);
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";

                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, SerialIDList.Length, "0", BatchName, strSrlID, strSrlName, Sales_UOM_Name, Sales_UOM_Code, SerialIDList.Length + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, SerialIDList.Length, repute, AltQty);
                            }
                            else
                            {
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, "", "", "0", "", strSrlID, strSrlName, "", Sales_UOM_Code, "", "", Stk_UOM_Code, "", "", "", "", "", "", loopId, SerialIDList.Length, repute, AltQty);
                            }
                        }
                    }
                }
                else if (Type == "BS")
                {
                    // GetTotalStock(ref Trans_Stock, WarehouseID);
                    GetProductUOM(ref Sales_UOM_Name, ref Sales_UOM_Code, ref Stk_UOM_Name, ref Stk_UOM_Code, ref Conversion_Multiplier, ProductID);
                    GetBatchDetails(ref MfgDate, ref ExpiryDate, BatchID);

                    string[] SerialIDList = SerialID.Split(new string[] { "||@||" }, StringSplitOptions.None);
                    string[] SerialNameList = SerialName.Split(new string[] { "||@||" }, StringSplitOptions.None);

                    if (editWarehouseID == "0")
                    {
                        for (int i = 0; i < SerialIDList.Length; i++)
                        {
                            string strSrlID = SerialIDList[i];
                            string strSrlName = SerialNameList[i];
                            WarehouseID = "0";

                            if (i == 0)
                            {
                                decimal ConversionMultiplier = Convert.ToDecimal(Conversion_Multiplier);
                                string stkqtn = Convert.ToString(Math.Round((SerialIDList.Length * ConversionMultiplier), 2));
                                decimal BalanceStk = Convert.ToDecimal(Trans_Stock) - Convert.ToDecimal(stkqtn);
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";

                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, SerialIDList.Length, BatchID, BatchName, strSrlID, strSrlName, Sales_UOM_Name, Sales_UOM_Code, SerialIDList.Length + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, SerialIDList.Length, "D", AltQty);
                            }
                            else
                            {
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, "", "", BatchID, "", strSrlID, strSrlName, "", Sales_UOM_Code, "", "", Stk_UOM_Code, "", "", "", "", "", "", loopId, SerialIDList.Length, "D", AltQty);
                            }
                        }
                    }
                    else
                    {
                        string strLoopID = "0", strSerial = "0";

                        DataRow[] result = Warehousedt.Select("SrlNo ='" + editWarehouseID + "'");
                        foreach (DataRow row in result)
                        {
                            strSerial = Convert.ToString(row["SerialID"]);
                            strLoopID = Convert.ToString(row["LoopID"]);
                        }

                        int count = 0;
                        DataRow[] dr = Warehousedt.Select("LoopID ='" + strLoopID + "'");
                        int value = (dr.Length + SerialIDList.Length - 1);

                        string[] temp_SerialIDList = new string[value];
                        string[] temp_SerialNameList = new string[value];

                        string[] check_SerialIDList = new string[value];
                        string[] check_SerialNameList = new string[value];

                        foreach (DataRow rows in dr)
                        {
                            if (strSerial != Convert.ToString(rows["SerialID"]))
                            {
                                temp_SerialIDList[count] = Convert.ToString(rows["SerialID"]);
                                temp_SerialNameList[count] = Convert.ToString(rows["SerialNo"]);

                                check_SerialIDList[count] = Convert.ToString(rows["SerialID"]);
                                check_SerialNameList[count] = Convert.ToString(rows["SerialNo"]);

                                count++;
                            }
                        }

                        for (int i = 0; i < SerialIDList.Length; i++)
                        {
                            temp_SerialIDList[count] = Convert.ToString(SerialIDList[i]);
                            temp_SerialNameList[count] = Convert.ToString(SerialNameList[i]);

                            count++;
                        }

                        DataRow[] delResult = Warehousedt.Select("LoopID ='" + strLoopID + "'");
                        foreach (DataRow delrow in delResult)
                        {
                            delrow.Delete();
                        }
                        Warehousedt.AcceptChanges();

                        SerialIDList = temp_SerialIDList;
                        SerialNameList = temp_SerialNameList;

                        for (int i = 0; i < SerialIDList.Length; i++)
                        {
                            string strSrlID = SerialIDList[i];
                            string strSrlName = SerialNameList[i];
                            WarehouseID = "0";
                            string repute = "D";
                            if (check_SerialIDList.Contains(strSrlID)) repute = "I";

                            if (i == 0)
                            {
                                decimal ConversionMultiplier = Convert.ToDecimal(Conversion_Multiplier);
                                string stkqtn = Convert.ToString(Math.Round((SerialIDList.Length * ConversionMultiplier), 2));
                                decimal BalanceStk = Convert.ToDecimal(Trans_Stock) - Convert.ToDecimal(stkqtn);
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";

                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, SerialIDList.Length, BatchID, BatchName, strSrlID, strSrlName, Sales_UOM_Name, Sales_UOM_Code, SerialIDList.Length + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, SerialIDList.Length, repute, AltQty);
                            }
                            else
                            {
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, "", "", BatchID, "", strSrlID, strSrlName, "", Sales_UOM_Code, "", "", Stk_UOM_Code, "", "", "", "", "", "", loopId, SerialIDList.Length, repute, AltQty);
                            }
                        }
                    }
                }

                //Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, WarehouseID, WarehouseName, "1", BatchID, BatchName, SerialID, SerialName);
                //string sortExpression = string.Format("{0} {1}", colName, direction);
                //dt.DefaultView.Sort = sortExpression;
                //Warehousedt.DefaultView.Sort = "SrlNo Asc";
                //Warehousedt = Warehousedt.DefaultView.ToTable(true);

                if (IsDelete == true)
                {
                    DataRow[] delResult = Warehousedt.Select("SrlNo ='" + editWarehouseID + "'");
                    foreach (DataRow delrow in delResult)
                    {
                        delrow.Delete();
                    }
                    Warehousedt.AcceptChanges();
                }

                Session["SI_WarehouseData" + Convert.ToString(uniqueId.Value)] = Warehousedt;
                //    changeGridOrder();

                GrdWarehouse.DataSource = Warehousedt.DefaultView;
                GrdWarehouse.DataBind();

                Session["SI_LoopWarehouse" + Convert.ToString(uniqueId.Value)] = loopId + 1;

                CmbWarehouse.SelectedIndex = -1;
                CmbBatch.SelectedIndex = -1;
            }
            else if (strSplitCommand == "Delete")
            {
                string strKey = e.Parameters.Split('~')[1];
                string strLoopID = "", strPreLoopID = "";

                DataTable Warehousedt = new DataTable();
                if (Session["SI_WarehouseData" + Convert.ToString(uniqueId.Value)] != null)
                {
                    Warehousedt = (DataTable)Session["SI_WarehouseData" + Convert.ToString(uniqueId.Value)];
                }

                DataRow[] result = Warehousedt.Select("SrlNo ='" + strKey + "'");
                foreach (DataRow row in result)
                {
                    strLoopID = row["LoopID"].ToString();
                }

                if (Warehousedt != null && Warehousedt.Rows.Count > 0)
                {
                    int count = 0;
                    bool IsFirst = false, IsAssign = false;
                    string WarehouseName = "", Quantity = "", BatchNo = "", SalesUOMName = "", SalesQuantity = "", StkUOMName = "", StkQuantity = "", ConversionMultiplier = "", AvailableQty = "", BalancrStk = "", MfgDate = "", ExpiryDate = "";


                    for (int i = 0; i < Warehousedt.Rows.Count; i++)
                    {
                        DataRow dr = Warehousedt.Rows[i];
                        string delSrlID = Convert.ToString(dr["SrlNo"]);
                        string delLoopID = Convert.ToString(dr["LoopID"]);

                        if (strPreLoopID != delLoopID)
                        {
                            count = 0;
                        }

                        if ((delLoopID == strLoopID) && (strKey == delSrlID) && count == 0)
                        {
                            IsFirst = true;

                            WarehouseName = Convert.ToString(dr["WarehouseName"]);
                            Quantity = Convert.ToString(dr["Quantity"]);
                            BatchNo = Convert.ToString(dr["BatchNo"]);
                            SalesUOMName = Convert.ToString(dr["SalesUOMName"]);
                            SalesQuantity = Convert.ToString(dr["SalesQuantity"]);
                            StkUOMName = Convert.ToString(dr["StkUOMName"]);
                            StkQuantity = Convert.ToString(dr["StkQuantity"]);
                            ConversionMultiplier = Convert.ToString(dr["ConversionMultiplier"]);
                            AvailableQty = Convert.ToString(dr["AvailableQty"]);
                            BalancrStk = Convert.ToString(dr["BalancrStk"]);
                            MfgDate = Convert.ToString(dr["MfgDate"]);
                            ExpiryDate = Convert.ToString(dr["ExpiryDate"]);

                            //dr.Delete();
                        }
                        else
                        {
                            if (delLoopID == strLoopID)
                            {
                                if (strKey == delSrlID)
                                {
                                    //dr.Delete();
                                }
                                else
                                {
                                    decimal S_Quantity = Convert.ToDecimal(dr["TotalQuantity"]);
                                    dr["Quantity"] = S_Quantity - 1;
                                    dr["TotalQuantity"] = S_Quantity - 1;

                                    if (IsFirst == true && IsAssign == false)
                                    {
                                        IsAssign = true;

                                        dr["WarehouseName"] = WarehouseName;
                                        dr["BatchNo"] = BatchNo;
                                        dr["SalesUOMName"] = SalesUOMName;
                                        dr["SalesQuantity"] = (S_Quantity - 1) + " " + SalesUOMName;//SalesQuantity;
                                        dr["StkUOMName"] = StkUOMName;
                                        dr["StkQuantity"] = StkQuantity;
                                        dr["ConversionMultiplier"] = ConversionMultiplier;
                                        dr["AvailableQty"] = AvailableQty;
                                        dr["BalancrStk"] = BalancrStk;
                                        dr["MfgDate"] = MfgDate;
                                        dr["ExpiryDate"] = ExpiryDate;
                                    }
                                    else
                                    {
                                        if (IsAssign == false)
                                        {
                                            IsAssign = true;
                                            SalesUOMName = Convert.ToString(dr["SalesUOMName"]);
                                            dr["SalesQuantity"] = (S_Quantity - 1) + " " + SalesUOMName;//SalesQuantity;
                                        }
                                    }
                                }
                            }
                        }

                        strPreLoopID = delLoopID;
                        count++;
                    }
                    Warehousedt.AcceptChanges();


                    for (int i = 0; i < Warehousedt.Rows.Count; i++)
                    {
                        DataRow dr = Warehousedt.Rows[i];
                        string delSrlID = Convert.ToString(dr["SrlNo"]);
                        if (strKey == delSrlID)
                        {
                            dr.Delete();
                        }
                    }
                    Warehousedt.AcceptChanges();
                }

                Session["SI_WarehouseData" + Convert.ToString(uniqueId.Value)] = Warehousedt;
                GrdWarehouse.DataSource = Warehousedt.DefaultView;
                GrdWarehouse.DataBind();
            }
            else if (strSplitCommand == "WarehouseDelete")
            {
                string ProductID = Convert.ToString(hdfProductSerialID.Value);
                DeleteUnsaveWarehouse(ProductID);
            }
            else if (strSplitCommand == "WarehouseFinal")
            {
                if (Session["SI_WarehouseData" + Convert.ToString(uniqueId.Value)] != null)
                {
                    DataTable Warehousedt = (DataTable)Session["SI_WarehouseData" + Convert.ToString(uniqueId.Value)];
                    string ProductID = Convert.ToString(hdfProductSerialID.Value);
                    string strPreLoopID = "";
                    decimal sum = 0;

                    for (int i = 0; i < Warehousedt.Rows.Count; i++)
                    {
                        DataRow dr = Warehousedt.Rows[i];
                        string delLoopID = Convert.ToString(dr["LoopID"]);
                        string Product_SrlNo = Convert.ToString(dr["Product_SrlNo"]);

                        if (ProductID == Product_SrlNo)
                        {
                            string strQuantity = (Convert.ToString(dr["SalesQuantity"]) != "") ? Convert.ToString(dr["SalesQuantity"]) : "0";
                            var weight = Decimal.Parse(Regex.Match(strQuantity, "[0-9]*\\.*[0-9]*").Value);
                            //string resultString = Regex.Match(strQuantity, @"[^0-9\.]+").Value;

                            sum = sum + Convert.ToDecimal(weight);
                        }
                    }

                    if (Convert.ToDecimal(sum) == Convert.ToDecimal(hdnProductQuantity.Value))
                    {
                        GrdWarehouse.JSProperties["cpIsSave"] = "Y";
                        for (int i = 0; i < Warehousedt.Rows.Count; i++)
                        {
                            DataRow dr = Warehousedt.Rows[i];
                            string Product_SrlNo = Convert.ToString(dr["Product_SrlNo"]);
                            if (ProductID == Product_SrlNo)
                            {
                                dr["Status"] = "I";
                            }
                        }
                        Warehousedt.AcceptChanges();
                    }
                    else
                    {
                        GrdWarehouse.JSProperties["cpIsSave"] = "N";
                    }

                    Session["SI_WarehouseData" + Convert.ToString(uniqueId.Value)] = Warehousedt;
                }
            }
        }
        protected void CallbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string performpara = e.Parameter;
            if (performpara.Split('~')[0] == "EditWarehouse")
            {
                string SrlNo = performpara.Split('~')[1];
                string ProductType = Convert.ToString(hdfProductType.Value);

                if (Session["SI_WarehouseData" + Convert.ToString(uniqueId.Value)] != null)
                {
                    DataTable Warehousedt = (DataTable)Session["SI_WarehouseData" + Convert.ToString(uniqueId.Value)];

                    string strWarehouse = "", strBatchID = "", strSrlID = "", strQuantity = "0", strAltQuantity = "0";
                    var rows = Warehousedt.Select(string.Format("SrlNo ='{0}'", SrlNo));
                    foreach (var dr in rows)
                    {
                        strWarehouse = (Convert.ToString(dr["WarehouseID"]) != "") ? Convert.ToString(dr["WarehouseID"]) : "0";
                        strBatchID = (Convert.ToString(dr["BatchID"]) != "") ? Convert.ToString(dr["BatchID"]) : "0";
                        strSrlID = (Convert.ToString(dr["SerialID"]) != "") ? Convert.ToString(dr["SerialID"]) : "0";
                        strQuantity = (Convert.ToString(dr["TotalQuantity"]) != "") ? Convert.ToString(dr["TotalQuantity"]) : "0";
                        strAltQuantity = (Convert.ToString(dr["AltQty"]) != "") ? Convert.ToString(dr["AltQty"]) : "0";
                    }

                    //CmbWarehouse.DataSource = GetWarehouseData();
                    CmbBatch.DataSource = GetBatchData(strWarehouse);
                    CmbBatch.DataBind();

                    CallbackPanel.JSProperties["cpEdit"] = strWarehouse + "~" + strBatchID + "~" + strSrlID + "~" + strQuantity + "~" + strAltQuantity;
                }
            }
        }
        //public void changeGridOrder()
        //{
        //    string Type = "";
        //    GetProductType(ref Type);
        //    if (Type == "W")
        //    {
        //        GrdWarehouse.Columns["WarehouseName"].Visible = true;
        //        GrdWarehouse.Columns["AvailableQty"].Visible = false;
        //        GrdWarehouse.Columns["BatchNo"].Visible = false;
        //        GrdWarehouse.Columns["MfgDate"].Visible = false;
        //        GrdWarehouse.Columns["ExpiryDate"].Visible = false;
        //        GrdWarehouse.Columns["SerialNo"].Visible = false;
        //    }
        //    else if (Type == "WB")
        //    {
        //        GrdWarehouse.Columns["WarehouseName"].Visible = true;
        //        GrdWarehouse.Columns["AvailableQty"].Visible = false;
        //        GrdWarehouse.Columns["BatchNo"].Visible = true;
        //        GrdWarehouse.Columns["MfgDate"].Visible = true;
        //        GrdWarehouse.Columns["ExpiryDate"].Visible = true;
        //        GrdWarehouse.Columns["SerialNo"].Visible = false;
        //    }
        //    else if (Type == "WBS")
        //    {
        //        GrdWarehouse.Columns["WarehouseName"].Visible = true;
        //        GrdWarehouse.Columns["AvailableQty"].Visible = false;
        //        GrdWarehouse.Columns["BatchNo"].Visible = true;
        //        GrdWarehouse.Columns["MfgDate"].Visible = true;
        //        GrdWarehouse.Columns["ExpiryDate"].Visible = true;
        //        GrdWarehouse.Columns["SerialNo"].Visible = true;
        //    }
        //    else if (Type == "B")
        //    {
        //        GrdWarehouse.Columns["WarehouseName"].Visible = false;
        //        GrdWarehouse.Columns["AvailableQty"].Visible = false;
        //        GrdWarehouse.Columns["BatchNo"].Visible = true;
        //        GrdWarehouse.Columns["MfgDate"].Visible = true;
        //        GrdWarehouse.Columns["ExpiryDate"].Visible = true;
        //        GrdWarehouse.Columns["SerialNo"].Visible = false;
        //    }
        //    else if (Type == "S")
        //    {
        //        GrdWarehouse.Columns["WarehouseName"].Visible = false;
        //        GrdWarehouse.Columns["AvailableQty"].Visible = false;
        //        GrdWarehouse.Columns["BatchNo"].Visible = false;
        //        GrdWarehouse.Columns["MfgDate"].Visible = false;
        //        GrdWarehouse.Columns["ExpiryDate"].Visible = false;
        //        GrdWarehouse.Columns["SerialNo"].Visible = true;
        //    }
        //    else if (Type == "WS")
        //    {
        //        GrdWarehouse.Columns["WarehouseName"].Visible = true;
        //        GrdWarehouse.Columns["AvailableQty"].Visible = false;
        //        GrdWarehouse.Columns["BatchNo"].Visible = false;
        //        GrdWarehouse.Columns["MfgDate"].Visible = false;
        //        GrdWarehouse.Columns["ExpiryDate"].Visible = false;
        //        GrdWarehouse.Columns["SerialNo"].Visible = true;
        //    }
        //    else if (Type == "BS")
        //    {
        //        GrdWarehouse.Columns["WarehouseName"].Visible = false;
        //        GrdWarehouse.Columns["AvailableQty"].Visible = false;
        //        GrdWarehouse.Columns["BatchNo"].Visible = true;
        //        GrdWarehouse.Columns["MfgDate"].Visible = true;
        //        GrdWarehouse.Columns["ExpiryDate"].Visible = true;
        //        GrdWarehouse.Columns["SerialNo"].Visible = true;
        //    }
        //}


        public void GetTotalStock(ref string Trans_Stock, string WarehouseID)
        {
            string ProductID = Convert.ToString(hdfProductID.Value);

            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 500, "GetBatchDetails");
            proc.AddVarcharPara("@ProductID", 100, Convert.ToString(ProductID));
            proc.AddVarcharPara("@WarehouseID", 100, Convert.ToString(WarehouseID));
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            proc.AddVarcharPara("@branchId", 500, Convert.ToString(Session["userbranchID"]));
            proc.AddVarcharPara("@companyId", 500, Convert.ToString(Session["LastCompany"]));
            DataTable dt = proc.GetTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                Trans_Stock = Convert.ToString(dt.Rows[0]["Trans_Stock"]);
            }
        }
        public void GetProductUOM(ref string Sales_UOM_Name, ref string Sales_UOM_Code, ref string Stk_UOM_Name, ref string Stk_UOM_Code, ref string Conversion_Multiplier, string ProductID)
        {
            DataTable Productdt = GetProductDetailsData(ProductID);
            if (Productdt != null && Productdt.Rows.Count > 0)
            {
                Sales_UOM_Name = Convert.ToString(Productdt.Rows[0]["Sales_UOM_Name"]);
                Sales_UOM_Code = Convert.ToString(Productdt.Rows[0]["Sales_UOM_Code"]);
                Stk_UOM_Name = Convert.ToString(Productdt.Rows[0]["Stk_UOM_Name"]);
                Stk_UOM_Code = Convert.ToString(Productdt.Rows[0]["Stk_UOM_Code"]);
                Conversion_Multiplier = Convert.ToString(Productdt.Rows[0]["Conversion_Multiplier"]);
            }
        }
        public void GetBatchDetails(ref string MfgDate, ref string ExpiryDate, string BatchID)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 500, "GetBatchDetails");
            proc.AddVarcharPara("@BatchID", 100, Convert.ToString(BatchID));
            DataTable Batchdt = proc.GetTable();

            if (Batchdt != null && Batchdt.Rows.Count > 0)
            {
                MfgDate = Convert.ToString(Batchdt.Rows[0]["MfgDate"]);
                ExpiryDate = Convert.ToString(Batchdt.Rows[0]["ExpiryDate"]);
            }
        }
        public void GetProductType(ref string Type)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 500, "GetSchemeType");
            proc.AddVarcharPara("@ProductID", 100, Convert.ToString(hdfProductID.Value));
            DataTable dt = proc.GetTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                Type = Convert.ToString(dt.Rows[0]["Type"]);
            }
        }
        protected void GrdWarehouse_DataBinding(object sender, EventArgs e)
        {
            if (Session["SI_WarehouseData" + Convert.ToString(uniqueId.Value)] != null)
            {
                string Type = "";
                GetProductType(ref Type);
                string SerialID = Convert.ToString(hdfProductSerialID.Value);
                DataTable Warehousedt = (DataTable)Session["SI_WarehouseData" + Convert.ToString(uniqueId.Value)];

                if (Warehousedt != null && Warehousedt.Rows.Count > 0)
                {
                    DataView dvData = new DataView(Warehousedt);
                    dvData.RowFilter = "Product_SrlNo = '" + SerialID + "'";

                    GrdWarehouse.DataSource = dvData;
                }
                else
                {
                    GrdWarehouse.DataSource = Warehousedt.DefaultView;
                }
            }
        }
        [WebMethod]
        public static string getSchemeType(string Products_ID)
        {
            string Type = "";
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 500, "GetSchemeType");
            proc.AddVarcharPara("@ProductID", 100, Convert.ToString(Products_ID));
            DataTable dt = proc.GetTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                Type = Convert.ToString(dt.Rows[0]["Type"]);
            }

            return Convert.ToString(Type);
        }
        [WebMethod]
        public object getGroupCustomer()
        {
            string Name = "";
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 500, "GroupCustomerDetails");

            DataTable dt = proc.GetTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                Name = Convert.ToString(dt.Rows[0]["Name"]);
            }

            return Convert.ToString(Name);
        }
        public void GetQuantityBaseOnProduct(string strProductSrlNo, ref decimal WarehouseQty)
        {
            decimal sum = 0;

            DataTable Warehousedt = new DataTable();
            if (Session["SI_WarehouseData" + Convert.ToString(uniqueId.Value)] != null)
            {
                Warehousedt = (DataTable)Session["SI_WarehouseData" + Convert.ToString(uniqueId.Value)];
                for (int i = 0; i < Warehousedt.Rows.Count; i++)
                {
                    DataRow dr = Warehousedt.Rows[i];
                    string Product_SrlNo = Convert.ToString(dr["Product_SrlNo"]);

                    if (strProductSrlNo == Product_SrlNo)
                    {
                        string strQuantity = (Convert.ToString(dr["SalesQuantity"]) != "") ? Convert.ToString(dr["SalesQuantity"]) : "0";
                        var weight = Decimal.Parse(Regex.Match(strQuantity, "[0-9]*\\.*[0-9]*").Value);

                        sum = sum + Convert.ToDecimal(weight);
                    }
                }
            }

            WarehouseQty = sum;
        }
        public void DeleteWarehouse(string SrlNo)
        {
            DataTable Warehousedt = new DataTable();
            if (Session["SI_WarehouseData" + Convert.ToString(uniqueId.Value)] != null)
            {
                Warehousedt = (DataTable)Session["SI_WarehouseData" + Convert.ToString(uniqueId.Value)];

                var rows = Warehousedt.Select(string.Format("Product_SrlNo ='{0}'", SrlNo));
                foreach (var row in rows)
                {
                    row.Delete();
                }
                Warehousedt.AcceptChanges();

                Session["SI_WarehouseData" + Convert.ToString(uniqueId.Value)] = Warehousedt;
            }
        }
        public void DeleteUnsaveWarehouse(string SrlNo)
        {
            DataTable Warehousedt = new DataTable();
            if (Session["SI_WarehouseData" + Convert.ToString(uniqueId.Value)] != null)
            {
                Warehousedt = (DataTable)Session["SI_WarehouseData" + Convert.ToString(uniqueId.Value)];

                var rows = Warehousedt.Select("Product_SrlNo ='" + SrlNo + "' AND Status='D'");
                foreach (var row in rows)
                {
                    row.Delete();
                }
                Warehousedt.AcceptChanges();

                Session["SI_WarehouseData" + Convert.ToString(uniqueId.Value)] = Warehousedt;
            }
        }
        public DataTable DeleteWarehouseBySrl(string strKey)
        {
            string strLoopID = "", strPreLoopID = "";

            DataTable Warehousedt = new DataTable();
            if (Session["SI_WarehouseData" + Convert.ToString(uniqueId.Value)] != null)
            {
                Warehousedt = (DataTable)Session["SI_WarehouseData" + Convert.ToString(uniqueId.Value)];
            }

            DataRow[] result = Warehousedt.Select("SrlNo ='" + strKey + "'");
            foreach (DataRow row in result)
            {
                strLoopID = row["LoopID"].ToString();
            }

            if (Warehousedt != null && Warehousedt.Rows.Count > 0)
            {
                int count = 0;
                bool IsFirst = false, IsAssign = false;
                string WarehouseName = "", Quantity = "", BatchNo = "", SalesUOMName = "", SalesQuantity = "", StkUOMName = "", StkQuantity = "", ConversionMultiplier = "", AvailableQty = "", BalancrStk = "", MfgDate = "", ExpiryDate = "";


                for (int i = 0; i < Warehousedt.Rows.Count; i++)
                {
                    DataRow dr = Warehousedt.Rows[i];
                    string delSrlID = Convert.ToString(dr["SrlNo"]);
                    string delLoopID = Convert.ToString(dr["LoopID"]);

                    if (strPreLoopID != delLoopID)
                    {
                        count = 0;
                    }

                    if ((delLoopID == strLoopID) && (strKey == delSrlID) && count == 0)
                    {
                        IsFirst = true;

                        WarehouseName = Convert.ToString(dr["WarehouseName"]);
                        Quantity = Convert.ToString(dr["Quantity"]);
                        BatchNo = Convert.ToString(dr["BatchNo"]);
                        SalesUOMName = Convert.ToString(dr["SalesUOMName"]);
                        SalesQuantity = Convert.ToString(dr["SalesQuantity"]);
                        StkUOMName = Convert.ToString(dr["StkUOMName"]);
                        StkQuantity = Convert.ToString(dr["StkQuantity"]);
                        ConversionMultiplier = Convert.ToString(dr["ConversionMultiplier"]);
                        AvailableQty = Convert.ToString(dr["AvailableQty"]);
                        BalancrStk = Convert.ToString(dr["BalancrStk"]);
                        MfgDate = Convert.ToString(dr["MfgDate"]);
                        ExpiryDate = Convert.ToString(dr["ExpiryDate"]);

                        //dr.Delete();
                    }
                    else
                    {
                        if (delLoopID == strLoopID)
                        {
                            if (strKey == delSrlID)
                            {
                                //dr.Delete();
                            }
                            else
                            {
                                int S_Quantity = Convert.ToInt32(dr["TotalQuantity"]);
                                dr["Quantity"] = S_Quantity - 1;
                                dr["TotalQuantity"] = S_Quantity - 1;

                                if (IsFirst == true && IsAssign == false)
                                {
                                    IsAssign = true;

                                    dr["WarehouseName"] = WarehouseName;
                                    dr["BatchNo"] = BatchNo;
                                    dr["SalesUOMName"] = SalesUOMName;
                                    dr["SalesQuantity"] = (S_Quantity - 1) + " " + SalesUOMName;//SalesQuantity;
                                    dr["StkUOMName"] = StkUOMName;
                                    dr["StkQuantity"] = StkQuantity;
                                    dr["ConversionMultiplier"] = ConversionMultiplier;
                                    dr["AvailableQty"] = AvailableQty;
                                    dr["BalancrStk"] = BalancrStk;
                                    dr["MfgDate"] = MfgDate;
                                    dr["ExpiryDate"] = ExpiryDate;
                                }
                                else
                                {
                                    if (IsAssign == false)
                                    {
                                        IsAssign = true;
                                        SalesUOMName = Convert.ToString(dr["SalesUOMName"]);
                                        dr["SalesQuantity"] = (S_Quantity - 1) + " " + SalesUOMName;//SalesQuantity;
                                    }
                                }
                            }
                        }
                    }

                    strPreLoopID = delLoopID;
                    count++;
                }
                Warehousedt.AcceptChanges();


                for (int i = 0; i < Warehousedt.Rows.Count; i++)
                {
                    DataRow dr = Warehousedt.Rows[i];
                    string delSrlID = Convert.ToString(dr["SrlNo"]);
                    if (strKey == delSrlID)
                    {
                        dr.Delete();
                    }
                }
                Warehousedt.AcceptChanges();
            }

            return Warehousedt;
        }
        public void UpdateWarehouse(string oldSrlNo, string newSrlNo)
        {
            DataTable Warehousedt = new DataTable();
            if (Session["SI_WarehouseData" + Convert.ToString(uniqueId.Value)] != null)
            {
                Warehousedt = (DataTable)Session["SI_WarehouseData" + Convert.ToString(uniqueId.Value)];

                for (int i = 0; i < Warehousedt.Rows.Count; i++)
                {
                    DataRow dr = Warehousedt.Rows[i];
                    string Product_SrlNo = Convert.ToString(dr["Product_SrlNo"]);
                    if (oldSrlNo == Product_SrlNo)
                    {
                        dr["Product_SrlNo"] = newSrlNo;
                    }
                }
                Warehousedt.AcceptChanges();

                Session["SI_WarehouseData" + Convert.ToString(uniqueId.Value)] = Warehousedt;
            }
        }
        protected void acpAvailableStock_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string performpara = e.Parameter;
            string strProductID = Convert.ToString(performpara.Split('~')[0]);
            string strBranch = Convert.ToString(ddl_Branch.SelectedValue);
            acpAvailableStock.JSProperties["cpstock"] = "0.00";

            try
            {
                DataTable dt2 = oDBEngine.GetDataTable("Select dbo.fn_CheckAvailableQuotationStatewise(" + strBranch + ",'" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]) + "'," + strProductID + ") as branchopenstock");

                if (dt2.Rows.Count > 0)
                {
                    acpAvailableStock.JSProperties["cpstock"] = Convert.ToString(Math.Round(Convert.ToDecimal(dt2.Rows[0]["branchopenstock"]), 2));
                }
                else
                {
                    acpAvailableStock.JSProperties["cpstock"] = "0.00";
                }
                GetActualStockOnFocus(strProductID, Convert.ToInt32(ddl_Branch.SelectedValue));

            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        #endregion

        #region Unique Code Generated Section Start

        public string checkNMakeJVCode(string manual_str, int sel_schema_Id)
        {

            // oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            oDBEngine = new BusinessLogicLayer.DBEngine();


            DataTable dtSchema = new DataTable();
            DataTable dtC = new DataTable();
            string prefCompCode = string.Empty, sufxCompCode = string.Empty, startNo, paddedStr, sqlQuery = string.Empty;
            int EmpCode, prefLen, sufxLen, paddCounter;
            bool suppressZero = false;

            if (sel_schema_Id > 0)
            {
                dtSchema = oDBEngine.GetDataTable("tbl_master_idschema", "prefix, suffix, digit, startno, schema_type,suppressZero", "id=" + sel_schema_Id);
                int scheme_type = Convert.ToInt32(dtSchema.Rows[0]["schema_type"]);

                if (scheme_type != 0)
                {
                    startNo = dtSchema.Rows[0]["startno"].ToString();
                    paddCounter = Convert.ToInt32(dtSchema.Rows[0]["digit"]);
                    paddedStr = startNo.PadLeft(paddCounter, '0');

                    prefCompCode = dtSchema.Rows[0]["prefix"].ToString();
                    sufxCompCode = dtSchema.Rows[0]["suffix"].ToString();
                    prefLen = Convert.ToInt32(prefCompCode.Length);
                    sufxLen = Convert.ToInt32(sufxCompCode.Length);
                    suppressZero = Convert.ToBoolean(dtSchema.Rows[0]["suppressZero"]);

                    if (!suppressZero)
                    {
                        sqlQuery = "SELECT max(tjv.Invoice_Number) FROM tbl_trans_SalesInvoice tjv WHERE dbo.RegexMatch('";
                        if (prefLen > 0)
                            sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                        sqlQuery += "[0-9]{" + paddCounter + "}";
                        if (sufxLen > 0)
                            sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                        //sqlQuery += "?$', LTRIM(RTRIM(tjv.Invoice_Number))) = 1";
                        sqlQuery += "?$', LTRIM(RTRIM(tjv.Invoice_Number))) = 1 and Invoice_Number like '" + prefCompCode + "%'";
                        dtC = oDBEngine.GetDataTable(sqlQuery);
                    }
                    else
                    {
                        int i = startNo.Length;
                        while (i < paddCounter)
                        {
                            sqlQuery = "SELECT max(tjv.Invoice_Number) FROM tbl_trans_SalesInvoice tjv WHERE dbo.RegexMatch('";
                            if (prefLen > 0)
                                sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                            sqlQuery += "[0-9]{" + i + "}";
                            if (sufxLen > 0)
                                sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                            //sqlQuery += "?$', LTRIM(RTRIM(tjv.Invoice_Number))) = 1";
                            sqlQuery += "?$', LTRIM(RTRIM(tjv.Invoice_Number))) = 1 and Invoice_Number like '" + prefCompCode + "%'";

                            if (prefLen == 0 && sufxLen == 0)
                            {
                                sqlQuery += " and LEN(tjv.Invoice_Number)=" + i;
                            }

                            dtC = oDBEngine.GetDataTable(sqlQuery);
                            if (dtC.Rows[0][0].ToString() == "")
                            {
                                break;
                            }
                            i++;


                        }
                        if (i != 1)
                        {
                            sqlQuery = "SELECT max(tjv.Invoice_Number) FROM tbl_trans_SalesInvoice tjv WHERE dbo.RegexMatch('";
                            if (prefLen > 0)
                                sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                            sqlQuery += "[0-9]{" + (i - 1) + "}";
                            if (sufxLen > 0)
                                sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                            //sqlQuery += "?$', LTRIM(RTRIM(tjv.Invoice_Number))) = 1";
                            sqlQuery += "?$', LTRIM(RTRIM(tjv.Invoice_Number))) = 1 and Invoice_Number like '" + prefCompCode + "%'";

                            if (prefLen == 0 && sufxLen == 0)
                            {
                                sqlQuery += " and LEN(tjv.Invoice_Number)=" + (i - 1);
                            }
                            dtC = oDBEngine.GetDataTable(sqlQuery);
                        }
                    }



                    if (dtC.Rows[0][0].ToString() == "")
                    {

                        sqlQuery = "SELECT max(tjv.Invoice_Number) FROM tbl_trans_SalesInvoice tjv WHERE dbo.RegexMatch('";
                        if (prefLen > 0)
                            sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                        //else if (scheme_type == 2)
                        //    sqlQuery += "^";
                        sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                        if (sufxLen > 0)
                            sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                        //sqlQuery += "?$', LTRIM(RTRIM(tjv.Invoice_Number))) = 1";
                        sqlQuery += "?$', LTRIM(RTRIM(tjv.Invoice_Number))) = 1 and Invoice_Number like '" + prefCompCode + "%'";
                        if (prefLen == 0 && sufxLen == 0)
                        {
                            sqlQuery += " and LEN(tjv.Invoice_Number)=" + (paddCounter - 1);
                        }
                        dtC = oDBEngine.GetDataTable(sqlQuery);
                    }

                    if (dtC.Rows.Count > 0 && dtC.Rows[0][0].ToString().Trim() != "")
                    {
                        string uccCode = dtC.Rows[0][0].ToString().Trim();
                        int UCCLen = uccCode.Length;
                        int decimalPartLen = UCCLen - (prefCompCode.Length + sufxCompCode.Length);
                        string uccCodeSubstring = uccCode.Substring(prefCompCode.Length, decimalPartLen);
                        EmpCode = Convert.ToInt32(uccCodeSubstring) + 1;

                        // out of range journal scheme
                        if (EmpCode.ToString().Length > paddCounter)
                        {
                            return "outrange";
                        }
                        else
                        {
                            if (!suppressZero)
                                paddedStr = EmpCode.ToString().PadLeft(paddCounter, '0');
                            else
                                paddedStr = EmpCode.ToString();
                            UniqueQuotation = prefCompCode + paddedStr + sufxCompCode;
                            return "ok";
                        }
                    }
                    else
                    {

                        if (!suppressZero)
                            paddedStr = startNo.PadLeft(paddCounter, '0');
                        else
                            paddedStr = startNo;
                        UniqueQuotation = prefCompCode + paddedStr + sufxCompCode;
                        return "ok";
                    }
                }
                else
                {
                    sqlQuery = "SELECT Invoice_Number FROM tbl_trans_SalesInvoice WHERE Invoice_Number LIKE '" + manual_str.Trim() + "'";
                    dtC = oDBEngine.GetDataTable(sqlQuery);
                    // duplicate manual entry check
                    if (dtC.Rows.Count > 0 && dtC.Rows[0][0].ToString().Trim() != "")
                    {
                        return "duplicate";
                    }

                    UniqueQuotation = manual_str.Trim();
                    return "ok";
                }
            }
            else
            {
                return "noid";
            }
        }

        public string checkNChallanCode(string manual_str, int sel_schema_Id)
        {
            //oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            oDBEngine = new BusinessLogicLayer.DBEngine();


            DataTable dtSchema = new DataTable();
            DataTable dtC = new DataTable();
            string prefCompCode = string.Empty, sufxCompCode = string.Empty, startNo, paddedStr, sqlQuery = string.Empty;
            int EmpCode, prefLen, sufxLen, paddCounter;

            if (sel_schema_Id > 0)
            {
                dtSchema = oDBEngine.GetDataTable("tbl_master_idschema", "prefix, suffix, digit, startno, schema_type", "id=" + sel_schema_Id);
                int scheme_type = Convert.ToInt32(dtSchema.Rows[0]["schema_type"]);

                if (scheme_type != 0)
                {
                    startNo = dtSchema.Rows[0]["startno"].ToString();
                    paddCounter = Convert.ToInt32(dtSchema.Rows[0]["digit"]);
                    paddedStr = startNo.PadLeft(paddCounter, '0');
                    prefCompCode = dtSchema.Rows[0]["prefix"].ToString();
                    sufxCompCode = dtSchema.Rows[0]["suffix"].ToString();
                    prefLen = Convert.ToInt32(prefCompCode.Length);
                    sufxLen = Convert.ToInt32(sufxCompCode.Length);

                    sqlQuery = "SELECT max(tjv.Challan_Number) FROM tbl_trans_SalesChallan tjv WHERE dbo.RegexMatch('";
                    if (prefLen > 0)
                        sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                    else if (scheme_type == 2)
                        sqlQuery += "^";
                    sqlQuery += "[0-9]{" + paddCounter + "}";
                    if (sufxLen > 0)
                        sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                    // sqlQuery += "?$', LTRIM(RTRIM(tjv.Challan_Number))) = 1";
                    sqlQuery += "?$', LTRIM(RTRIM(tjv.Challan_Number))) = 1 and Challan_Number like '" + prefCompCode + "%'";
                    if (scheme_type == 2)
                        sqlQuery += " AND CONVERT(DATE, CreatedDate) = CONVERT(DATE, GETDATE())";
                    dtC = oDBEngine.GetDataTable(sqlQuery);

                    if (dtC.Rows[0][0].ToString() == "")
                    {
                        sqlQuery = "SELECT max(tjv.Challan_Number) FROM tbl_trans_SalesChallan tjv WHERE dbo.RegexMatch('";
                        if (prefLen > 0)
                            sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                        else if (scheme_type == 2)
                            sqlQuery += "^";
                        sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                        if (sufxLen > 0)
                            sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                        //sqlQuery += "?$', LTRIM(RTRIM(tjv.Challan_Number))) = 1";
                        sqlQuery += "?$', LTRIM(RTRIM(tjv.Challan_Number))) = 1 and Challan_Number like '" + prefCompCode + "%'";
                        if (scheme_type == 2)
                            sqlQuery += " AND CONVERT(DATE, CreatedDate) = CONVERT(DATE, GETDATE())";
                        dtC = oDBEngine.GetDataTable(sqlQuery);
                    }

                    if (dtC.Rows.Count > 0 && dtC.Rows[0][0].ToString().Trim() != "")
                    {
                        string uccCode = dtC.Rows[0][0].ToString().Trim();
                        int UCCLen = uccCode.Length;
                        int decimalPartLen = UCCLen - (prefCompCode.Length + sufxCompCode.Length);
                        string uccCodeSubstring = uccCode.Substring(prefCompCode.Length, decimalPartLen);
                        EmpCode = Convert.ToInt32(uccCodeSubstring) + 1;
                        // out of range journal scheme
                        if (EmpCode.ToString().Length > paddCounter)
                        {
                            return "outrange";
                        }
                        else
                        {
                            paddedStr = EmpCode.ToString().PadLeft(paddCounter, '0');
                            UniqueChallan = prefCompCode + paddedStr + sufxCompCode;
                            return "ok";
                        }
                    }
                    else
                    {
                        UniqueChallan = startNo.PadLeft(paddCounter, '0');
                        UniqueChallan = prefCompCode + paddedStr + sufxCompCode;
                        return "ok";
                    }
                }
                else
                {
                    sqlQuery = "SELECT Challan_Number FROM tbl_trans_SalesChallan WHERE Challan_Number LIKE '" + manual_str.Trim() + "'";
                    dtC = oDBEngine.GetDataTable(sqlQuery);
                    // duplicate manual entry check
                    if (dtC.Rows.Count > 0 && dtC.Rows[0][0].ToString().Trim() != "")
                    {
                        return "duplicate";
                    }

                    UniqueChallan = manual_str.Trim();
                    return "ok";
                }
            }
            else
            {
                return "noid";
            }
        }

        #endregion Unique Code Generated Section End

        #region Product Tax

        public void setValueForHeaderGST(ASPxComboBox aspxcmb, string taxId)
        {
            for (int i = 0; i < aspxcmb.Items.Count; i++)
            {
                if (Convert.ToString(aspxcmb.Items[i].Value).Split('~')[0] == taxId.Split('~')[0])
                {
                    aspxcmb.Items[i].Selected = true;
                    break;
                }
            }

        }

        protected DataTable GetTaxDataWithGST(DataTable existing)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesInvoice_Details");
            proc.AddVarcharPara("@Action", 500, "TaxDetailsForGst");
            proc.AddVarcharPara("@InvoiceID", 500, InvoiceId);
            dt = proc.GetTable();
            if (dt.Rows.Count > 0)
            {
                DataRow gstRow = existing.NewRow();
                gstRow["Taxes_ID"] = 0;
                gstRow["Taxes_Name"] = dt.Rows[0]["TaxRatesSchemeName"];
                gstRow["Percentage"] = dt.Rows[0]["QuoteTax_Percentage"];
                gstRow["Amount"] = dt.Rows[0]["QuoteTax_Amount"];
                gstRow["AltTax_Code"] = dt.Rows[0]["Gst"];

                UpdateGstForCharges(Convert.ToString(dt.Rows[0]["Gst"]));
                txtGstCstVatCharge.Value = gstRow["Amount"];
                existing.Rows.Add(gstRow);
            }
            SetTotalCharges(existing);
            return existing;
        }

        public void SetTotalCharges(DataTable taxTableFinal)
        {
            decimal totalCharges = 0;
            foreach (DataRow dr in taxTableFinal.Rows)
            {
                if (Convert.ToString(dr["Taxes_ID"]) != "0")
                {
                    if (Convert.ToString(dr["Taxes_Name"]).Contains("(+)"))
                    {
                        totalCharges += Convert.ToDecimal(dr["Amount"]);
                    }
                    else
                    {
                        totalCharges -= Convert.ToDecimal(dr["Amount"]);
                    }
                }
                else
                {//Else part For Gst 
                    totalCharges += Convert.ToDecimal(dr["Amount"]);
                }
            }
            txtQuoteTaxTotalAmt.Value = totalCharges;

        }

        protected void UpdateGstForCharges(string data)
        {
            for (int i = 0; i < cmbGstCstVatcharge.Items.Count; i++)
            {
                if (Convert.ToString(cmbGstCstVatcharge.Items[i].Value).Split('~')[0] == data)
                {
                    cmbGstCstVatcharge.Items[i].Selected = true;
                    break;
                }
            }
        }
        public void GetStock(string strProductID)
        {
            string strBranch = Convert.ToString(ddl_Branch.SelectedValue);
            acpAvailableStock.JSProperties["cpstock"] = "0.00";

            try
            {
                DataTable dt2 = oDBEngine.GetDataTable("Select dbo.fn_CheckAvailableQuotationStatewise(" + strBranch + ",'" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]) + "'," + strProductID + ") as branchopenstock");

                if (dt2.Rows.Count > 0)
                {
                    taxUpdatePanel.JSProperties["cpstock"] = Convert.ToString(Math.Round(Convert.ToDecimal(dt2.Rows[0]["branchopenstock"]), 2));
                }
                else
                {
                    taxUpdatePanel.JSProperties["cpstock"] = "0.00";
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void GetActualStock(string ProductId, int branchId)
        {
            DataTable ActualStockTable = PosData.GetProductActualStock(branchId, ProductId);

            if (ActualStockTable.Rows.Count > 0)
            {
                taxUpdatePanel.JSProperties["cpActualStock"] = Convert.ToString(Math.Round(Convert.ToDecimal(ActualStockTable.Rows[0][0]), 2));
            }
            else
            {
                taxUpdatePanel.JSProperties["cpActualStock"] = "0.00";
            }

            taxUpdatePanel.JSProperties["cpbalanceStock"] = Convert.ToDecimal(taxUpdatePanel.JSProperties["cpstock"]) - Convert.ToDecimal(taxUpdatePanel.JSProperties["cpActualStock"]);
        }
        public void GetActualStockOnFocus(string ProductId, int branchId)
        {
            DataTable ActualStockTable = PosData.GetProductActualStock(branchId, ProductId);

            if (ActualStockTable.Rows.Count > 0)
            {
                acpAvailableStock.JSProperties["cpActualStock"] = Convert.ToString(Math.Round(Convert.ToDecimal(ActualStockTable.Rows[0][0]), 2));
            }
            else
            {
                acpAvailableStock.JSProperties["cpActualStock"] = "0.00";
            }

            acpAvailableStock.JSProperties["cpbalanceStock"] = Convert.ToDecimal(acpAvailableStock.JSProperties["cpstock"]) - Convert.ToDecimal(acpAvailableStock.JSProperties["cpActualStock"]);
        }
        protected void taxUpdatePanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {

            string performpara = e.Parameter;
            if (performpara.Split('~')[0] == "DelProdbySl")
            {
                DataTable MainTaxDataTable = (DataTable)Session["SI_FinalTaxRecord" + Convert.ToString(uniqueId.Value)];

                DataRow[] deletedRow = MainTaxDataTable.Select("SlNo=" + performpara.Split('~')[1]);
                if (deletedRow.Length > 0)
                {
                    foreach (DataRow dr in deletedRow)
                    {
                        MainTaxDataTable.Rows.Remove(dr);
                    }

                }

                Session["SI_FinalTaxRecord" + Convert.ToString(uniqueId.Value)] = MainTaxDataTable;
                GetStock(Convert.ToString(performpara.Split('~')[2]));
                GetActualStock(Convert.ToString(performpara.Split('~')[2]), Convert.ToInt32(ddl_Branch.SelectedValue));
                DeleteWarehouse(Convert.ToString(performpara.Split('~')[1]));
                DataTable taxDetails = (DataTable)Session["SI_TaxDetails" + Convert.ToString(uniqueId.Value)];
                if (taxDetails != null)
                {
                    foreach (DataRow dr in taxDetails.Rows)
                    {
                        dr["Amount"] = "0.00";
                    }
                    Session["SI_TaxDetails" + Convert.ToString(uniqueId.Value)] = taxDetails;
                }
            }
            else if (performpara.Split('~')[0] == "DeleteAllTax")
            {
                CreateDataTaxTable();

                DataTable taxDetails = (DataTable)Session["SI_TaxDetails" + Convert.ToString(uniqueId.Value)];

                if (taxDetails != null)
                {
                    foreach (DataRow dr in taxDetails.Rows)
                    {
                        dr["Amount"] = "0.00";
                    }
                    Session["SI_TaxDetails" + Convert.ToString(uniqueId.Value)] = taxDetails;
                }
            }
            else
            {
                DataTable MainTaxDataTable = (DataTable)Session["SI_FinalTaxRecord" + Convert.ToString(uniqueId.Value)];

                DataRow[] deletedRow = MainTaxDataTable.Select("SlNo=" + performpara.Split('~')[1]);
                if (deletedRow.Length > 0)
                {
                    foreach (DataRow dr in deletedRow)
                    {
                        MainTaxDataTable.Rows.Remove(dr);
                    }

                }

                Session["SI_FinalTaxRecord" + Convert.ToString(uniqueId.Value)] = MainTaxDataTable;
                DataTable taxDetails = (DataTable)Session["SI_TaxDetails" + Convert.ToString(uniqueId.Value)];
                if (taxDetails != null)
                {
                    foreach (DataRow dr in taxDetails.Rows)
                    {
                        dr["Amount"] = "0.00";
                    }
                    Session["SI_TaxDetails" + Convert.ToString(uniqueId.Value)] = taxDetails;
                }
            }
        }
        public double ReCalculateTaxAmount(string slno, double amount)
        {
            DataTable MainTaxDataTable = (DataTable)Session["SI_FinalTaxRecord" + Convert.ToString(uniqueId.Value)];
            double totalSum = 0.0;
            //Get The Existing datatable
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 500, "PopulateAllTax");
            DataTable TaxDt = proc.GetTable();

            DataRow[] filterRow = MainTaxDataTable.Select("SlNo=" + slno);

            if (filterRow.Length > 0)
            {
                foreach (DataRow dr in filterRow)
                {
                    if (Convert.ToString(dr["TaxCode"]) != "0")
                    {
                        DataRow[] taxrow = TaxDt.Select("Taxes_ID=" + dr["TaxCode"]);
                        if (taxrow.Length > 0)
                        {
                            if (taxrow[0]["TaxCalculateMethods"] == "A")
                            {
                                dr["Amount"] = (amount * Convert.ToDouble(dr["Percentage"])) / 100;
                                totalSum += (amount * Convert.ToDouble(dr["Percentage"])) / 100;
                            }
                            else
                            {
                                dr["Amount"] = (amount * Convert.ToDouble(dr["Percentage"])) / 100;
                                totalSum -= (amount * Convert.ToDouble(dr["Percentage"])) / 100;
                            }
                        }
                    }
                    else
                    {
                        DataRow[] taxrow = TaxDt.Select("Taxes_ID=" + dr["AltTaxCode"]);
                        if (taxrow.Length > 0)
                        {
                            if (taxrow[0]["TaxCalculateMethods"] == "A")
                            {
                                dr["Amount"] = (amount * Convert.ToDouble(dr["Percentage"])) / 100;
                                totalSum += (amount * Convert.ToDouble(dr["Percentage"])) / 100;
                            }
                            else
                            {
                                dr["Amount"] = (amount * Convert.ToDouble(dr["Percentage"])) / 100;
                                totalSum -= (amount * Convert.ToDouble(dr["Percentage"])) / 100;
                            }
                        }
                    }
                }

            }
            Session["SI_FinalTaxRecord" + Convert.ToString(uniqueId.Value)] = MainTaxDataTable;

            return totalSum;

        }

        public void PopulateGSTCSTVATCombo(string quoteDate)
        {
            string LastCompany = "";
            if (Convert.ToString(Session["LastCompany"]) != null)
            {
                LastCompany = Convert.ToString(Session["LastCompany"]);
            }
            //DataTable dt = new DataTable();
            //dt = objCRMSalesDtlBL.PopulateGSTCSTVATCombo();
            //DataTable DT = oDBEngine.GetDataTable("select cast(td.TaxRates_ID as varchar(5))+'~'+ cast (td.TaxRates_Rate as varchar(25)) 'Taxes_ID',td.TaxRatesSchemeName 'Taxes_Name',th.Taxes_Name as 'TaxCodeName' from Master_Taxes th inner join Config_TaxRates td on th.Taxes_ID=td.TaxRates_TaxCode where (td.TaxRates_Country=0 or td.TaxRates_Country=(select add_country from tbl_master_address where add_cntId ='" + Convert.ToString(Session["LastCompany"]) + "' ))  and th.Taxes_ApplicableFor in ('B','S') and th.TaxTypeCode in('G','V','C')");

            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 500, "LoadGSTCSTVATCombo");
            proc.AddVarcharPara("@S_QuoteAdd_CompanyID", 10, Convert.ToString(LastCompany));
            proc.AddVarcharPara("@S_quoteDate", 10, quoteDate);
            DataTable DT = proc.GetTable();
            cmbGstCstVat.DataSource = DT;
            cmbGstCstVat.TextField = "Taxes_Name";
            cmbGstCstVat.ValueField = "Taxes_ID";
            cmbGstCstVat.DataBind();
        }
        public void CreateDataTaxTable()
        {
            DataTable TaxRecord = new DataTable();

            TaxRecord.Columns.Add("SlNo", typeof(System.Int32));
            TaxRecord.Columns.Add("TaxCode", typeof(System.String));
            TaxRecord.Columns.Add("AltTaxCode", typeof(System.String));
            TaxRecord.Columns.Add("Percentage", typeof(System.Decimal));
            TaxRecord.Columns.Add("Amount", typeof(System.Decimal));
            Session["SI_FinalTaxRecord" + Convert.ToString(uniqueId.Value)] = TaxRecord;
        }

        //public IEnumerable GetTaxCode()
        //{
        //    List<taxCode> TaxList = new List<taxCode>();
        //    BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        //    // DataTable DT = objEngine.GetDataTable("select Taxes_ID,Taxes_Name from dbo.Master_Taxes");
        //    DataTable DT = objEngine.GetDataTable("select cast(th.Taxes_ID as varchar(5))+'~'+ cast (td.TaxRates_Rate as varchar(25)) 'Taxes_ID',th.Taxes_Name 'Taxes_Name' from Master_Taxes th inner join Config_TaxRates td on th.Taxes_ID=td.TaxRates_TaxCode where (td.TaxRates_Country=0 or td.TaxRates_Country=(select add_country from tbl_master_address where add_cntId ='" + Convert.ToString(Session["LastCompany"]) + "' ))  and th.Taxes_ApplicableFor in ('B','S')");


        //    for (int i = 0; i < DT.Rows.Count; i++)
        //    {
        //        taxCode tax = new taxCode();
        //        tax.Taxes_ID = Convert.ToString(DT.Rows[i]["Taxes_ID"]);
        //        tax.Taxes_Name = Convert.ToString(DT.Rows[i]["Taxes_Name"]);
        //        TaxList.Add(tax);
        //    }

        //    return TaxList;
        //}

        public string GetTaxName(int id)
        {
            string taxName = "";
            string[] arr = oDBEngine.GetFieldValue1("Master_taxes", "Taxes_Name", "Taxes_ID=" + Convert.ToString(id), 1);
            if (arr[0] != "n")
            {
                taxName = arr[0];
            }
            return taxName;
        }
        public DataSet GetQuotationTaxData()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 500, "ProductTaxDetails");
            proc.AddVarcharPara("@QuotationID", 500, InvoiceId);
            ds = proc.GetDataSet();
            return ds;
        }
        public DataSet GetQuotationEditedTaxData()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesInvoice_Details");
            proc.AddVarcharPara("@Action", 500, "ProductEditedTaxDetails");
            proc.AddVarcharPara("@InvoiceID", 500, InvoiceId);
            ds = proc.GetDataSet();
            return ds;
        }
        public DataTable GetComponentEditedTaxData(string ComponentDetailsIDs, string strType)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesInvoice_Details");
            proc.AddVarcharPara("@Action", 500, "ComponentProductTax");
            proc.AddVarcharPara("@SelectedComponentList", 500, ComponentDetailsIDs);
            proc.AddVarcharPara("@ComponentType", 500, strType);
            ds = proc.GetTable();
            return ds;
        }
        public double GetTotalTaxAmount(List<TaxDetails> tax)
        {
            double sum = 0;
            foreach (TaxDetails td in tax)
            {
                if (td.Taxes_Name.Substring(td.Taxes_Name.Length - 3, 3) == "(+)")
                    sum += td.Amount;
                else
                    sum -= td.Amount;

            }
            return sum;
        }
        protected void cgridTax_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string retMsg = "";
            if (e.Parameters.Split('~')[0] == "SaveGST")
            {
                DataTable TaxRecord = (DataTable)Session["SI_FinalTaxRecord" + Convert.ToString(uniqueId.Value)];
                int slNo = Convert.ToInt32(HdSerialNo.Value);
                //For GST/CST/VAT
                if (cmbGstCstVat.Value != null)
                {

                    DataRow[] finalRow = TaxRecord.Select("SlNo=" + Convert.ToString(slNo) + " and TaxCode='0'");
                    if (finalRow.Length > 0)
                    {
                        finalRow[0]["Percentage"] = Convert.ToString(cmbGstCstVat.Value).Split('~')[1];
                        finalRow[0]["Amount"] = txtGstCstVat.Text;
                        finalRow[0]["AltTaxCode"] = Convert.ToString(cmbGstCstVat.Value).Split('~')[0];

                    }
                    else
                    {
                        DataRow newRowGST = TaxRecord.NewRow();
                        newRowGST["slNo"] = slNo;
                        newRowGST["Percentage"] = Convert.ToString(cmbGstCstVat.Value).Split('~')[1];
                        newRowGST["TaxCode"] = "0";
                        newRowGST["AltTaxCode"] = Convert.ToString(cmbGstCstVat.Value).Split('~')[0];
                        newRowGST["Amount"] = txtGstCstVat.Text;
                        TaxRecord.Rows.Add(newRowGST);
                    }
                }
                //End Here

                aspxGridTax.JSProperties["cpUpdated"] = "";

                Session["SI_FinalTaxRecord" + Convert.ToString(uniqueId.Value)] = TaxRecord;
            }
            else
            {
                #region fetch All data For Tax

                DataTable taxDetail = new DataTable();
                DataTable MainTaxDataTable = (DataTable)Session["SI_FinalTaxRecord" + Convert.ToString(uniqueId.Value)];
                DataTable databaseReturnTable = (DataTable)Session["SI_QuotationTaxDetails"];

                //if (Convert.ToInt32(e.Parameters.Split('~')[1]) == 1)
                //    taxDetail = oDBEngine.GetDataTable("select TaxRates_ID as Taxes_ID,TaxRatesSchemeName as Taxes_Name,TaxCalculateMethods,tm.Taxes_Name as taxCodeName,tm.Taxes_ApplicableOn as 'ApplicableOn',isnull((select TaxRatesSchemeName from Config_TaxRates where TaxRates_ID=tm.Taxes_OtherTax),'')  as dependOn from Master_taxes tm inner join Config_TaxRates ct on tm.Taxes_ID=ct.TaxRates_TaxCode where tm.TaxTypeCode not in('G','V','C') and tm.TaxItemlevel='Y'");
                //else if (Convert.ToInt32(e.Parameters.Split('~')[1]) == 2)
                //taxDetail = oDBEngine.GetDataTable("select TaxRates_ID as Taxes_ID,TaxRatesSchemeName as Taxes_Name,TaxCalculateMethods,tm.Taxes_Name as taxCodeName,tm.Taxes_ApplicableOn as 'ApplicableOn',isnull((select TaxRatesSchemeName from Config_TaxRates where TaxRates_ID=tm.Taxes_OtherTax),'')  as dependOn from Master_taxes tm inner join Config_TaxRates ct on tm.Taxes_ID=ct.TaxRates_TaxCode where tm.TaxTypeCode not in('G','V','C') and tm.TaxItemlevel='Y'");

                ProcedureExecute proc = new ProcedureExecute("prc_PosSalesCRM_Details_Exclusive");
                proc.AddVarcharPara("@Action", 500, "LoadOtherTaxDetails");
                proc.AddVarcharPara("@ProductID", 10, Convert.ToString(setCurrentProdCode.Value));
                proc.AddVarcharPara("@S_quoteDate", 10, dt_PLQuote.Date.ToString("yyyy-MM-dd"));
                taxDetail = proc.GetTable();


                //Get Company Gstin 09032017
                string CompInternalId = Convert.ToString(Session["LastCompany"]);
                string[] compGstin = oDBEngine.GetFieldValue1("tbl_master_company", "cmp_gstin", "cmp_internalid='" + CompInternalId + "'", 1);


                //Get BranchStateCode
                string BranchStateCode = "", BranchGSTIN = "";
                DataTable BranchTable = oDBEngine.GetDataTable("select StateCode,branch_GSTIN   from tbl_master_branch branch inner join tbl_master_state st on branch.branch_state=st.id where branch_id=" + Convert.ToString(ddl_Branch.SelectedValue));
                if (BranchTable != null)
                {
                    BranchStateCode = Convert.ToString(BranchTable.Rows[0][0]);
                    BranchGSTIN = Convert.ToString(BranchTable.Rows[0][1]);
                }



                string ShippingState = "";
                if (ddl_PosGst.Value != null)
                {
                    if (ddl_PosGst.Value.ToString() == "S")
                    {
                        ShippingState = lblShippingStateText.Value;
                        ShippingState = ShippingState.Substring(ShippingState.IndexOf("(State Code:")).Replace("(State Code:", "").Replace(")", "");
                    }
                    else
                    {
                        ShippingState = lblBillingStateText.Value;
                        ShippingState = ShippingState.Substring(ShippingState.IndexOf("(State Code:")).Replace("(State Code:", "").Replace(")", "");
                    }
                }
                if (ShippingState.Trim() != "" && BranchStateCode != "")
                {

                    if (BranchStateCode != "")
                    {
                        if (BranchStateCode == ShippingState)
                        {
                            //Check if the state is in union territories then only UTGST will apply
                            //   Chandigarh     Andaman and Nicobar Islands    DADRA & NAGAR HAVELI    DAMAN & DIU             DELHI                  Lakshadweep              PONDICHERRY
                            if (ShippingState == "4" || ShippingState == "26" || ShippingState == "25" || ShippingState == "35" || ShippingState == "31" || ShippingState == "34")
                            {
                                foreach (DataRow dr in taxDetail.Rows)
                                {
                                    if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST")
                                    {
                                        dr.Delete();
                                    }
                                }

                            }
                            else
                            {
                                foreach (DataRow dr in taxDetail.Rows)
                                {
                                    if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
                                    {
                                        dr.Delete();
                                    }
                                }
                            }
                            taxDetail.AcceptChanges();
                        }
                        else
                        {
                            foreach (DataRow dr in taxDetail.Rows)
                            {
                                if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
                                {
                                    dr.Delete();
                                }
                            }
                            taxDetail.AcceptChanges();

                        }

                    }
                }

                //If Company GSTIN is blank then Delete All CGST,UGST,IGST,CGST
                if (compGstin[0].Trim() == "" && BranchGSTIN == "")
                {
                    foreach (DataRow dr in taxDetail.Rows)
                    {
                        if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST")
                        {
                            dr.Delete();
                        }
                    }
                    taxDetail.AcceptChanges();
                }



                //string[] schemeIDViaProdID = oDBEngine.GetFieldValue1("master_sproducts", "isnull(sProduct_TaxSchemeSale,0)sProduct_TaxSchemeSale", "sProducts_ID='" + Convert.ToString(setCurrentProdCode.Value) + "'", 1);

                //if (schemeIDViaProdID.Length > 0)
                //{

                //    if (taxDetail.Select("Taxes_ID='" + schemeIDViaProdID[0] + "'").Length > 0)
                //    {
                //        foreach (DataRow dr in taxDetail.Rows)
                //        {
                //            if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST")
                //            {
                //                if (Convert.ToString(dr["Taxes_ID"]).Trim() != schemeIDViaProdID[0].Trim())
                //                    dr["TaxRates_Rate"] = 0;
                //            }
                //        }
                //    }
                //}



                int slNo = Convert.ToInt32(HdSerialNo.Value);

                //Get Gross Amount and Net Amount 
                decimal ProdGrossAmt = Convert.ToDecimal(HdProdGrossAmt.Value);
                decimal ProdNetAmt = Convert.ToDecimal(HdProdNetAmt.Value);

                List<TaxDetails> TaxDetailsDetails = new List<TaxDetails>();

                //Debjyoti 09032017
                decimal totalParcentage = 0;
                foreach (DataRow dr in taxDetail.Rows)
                {
                    if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
                    {
                        totalParcentage += Convert.ToDecimal(dr["TaxRates_Rate"]);
                    }
                }

                if (e.Parameters.Split('~')[0] == "New")
                {
                    foreach (DataRow dr in taxDetail.Rows)
                    {
                        TaxDetails obj = new TaxDetails();
                        obj.Taxes_ID = Convert.ToInt32(dr["Taxes_ID"]);
                        obj.taxCodeName = Convert.ToString(dr["taxCodeName"]);
                        obj.TaxField = Convert.ToString(dr["TaxRates_Rate"]);
                        obj.Amount = 0.0;

                        #region set calculated on
                        //Check Tax Applicable on and set to calculated on
                        if (Convert.ToString(dr["ApplicableOn"]) == "G")
                        {
                            obj.calCulatedOn = ProdGrossAmt;
                        }
                        else if (Convert.ToString(dr["ApplicableOn"]) == "N")
                        {
                            obj.calCulatedOn = ProdNetAmt;
                        }
                        else
                        {
                            obj.calCulatedOn = 0;
                        }
                        //End Here
                        #endregion

                        //Debjyoti 09032017
                        if (Convert.ToString(ddl_AmountAre.Value) == "2")
                        {

                            if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST")
                            {
                                decimal finalCalCulatedOn = 0;
                                decimal backProcessRate = (1 + (totalParcentage / 100));
                                finalCalCulatedOn = obj.calCulatedOn / backProcessRate;
                                obj.calCulatedOn = finalCalCulatedOn;
                            }

                        }

                        if (Convert.ToString(dr["TaxCalculateMethods"]) == "A")
                        {
                            obj.Taxes_Name = Convert.ToString(dr["Taxes_Name"]) + "(+)";

                        }
                        else
                        {
                            obj.Taxes_Name = Convert.ToString(dr["Taxes_Name"]) + "(-)";
                        }

                        

                        if (Convert.ToString(dr["ApplicableOn"]) == "G")
                        {
                            obj.Amount = Convert.ToDouble(ProdGrossAmt * (Convert.ToDecimal(obj.TaxField) / 100)); 
                        }
                        else if (Convert.ToString(dr["ApplicableOn"]) == "N")
                        {
                            obj.Amount = Convert.ToDouble(ProdNetAmt * (Convert.ToDecimal(obj.TaxField) / 100)); 
                        }
                        else
                        {
                            obj.Amount = 0;
                        }



                        if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
                            obj.Amount = Math.Round(obj.Amount, 2);



                        DataRow[] filtr = MainTaxDataTable.Select("TaxCode ='" + obj.Taxes_ID + "' and slNo=" + Convert.ToString(slNo));
                        if (filtr.Length > 0)
                        {
                            obj.Amount = Convert.ToDouble(filtr[0]["Amount"]);
                            if (obj.Taxes_ID == 0)
                            {
                                //   obj.TaxField = GetTaxName(Convert.ToInt32(Convert.ToString(filtr[0]["AltTaxCode"])));
                                aspxGridTax.JSProperties["cpComboCode"] = Convert.ToString(filtr[0]["AltTaxCode"]);
                            }
                            else
                                obj.TaxField = Convert.ToString(filtr[0]["Percentage"]);
                        }

                        TaxDetailsDetails.Add(obj);
                    }
                }
                else
                {
                    string keyValue = e.Parameters.Split('~')[0];

                    DataTable TaxRecord = (DataTable)Session["SI_FinalTaxRecord" + Convert.ToString(uniqueId.Value)];


                    foreach (DataRow dr in taxDetail.Rows)
                    {
                        TaxDetails obj = new TaxDetails();
                        obj.Taxes_ID = Convert.ToInt32(dr["Taxes_ID"]);
                        obj.taxCodeName = Convert.ToString(dr["taxCodeName"]);

                        if (Convert.ToString(dr["TaxCalculateMethods"]) == "A")
                            obj.Taxes_Name = Convert.ToString(dr["Taxes_Name"]) + "(+)";
                        else
                            obj.Taxes_Name = Convert.ToString(dr["Taxes_Name"]) + "(-)";
                        obj.TaxField = "";
                        obj.Amount = 0.0;

                        #region set calculated on
                        //Check Tax Applicable on and set to calculated on
                        if (Convert.ToString(dr["ApplicableOn"]) == "G")
                        {
                            obj.calCulatedOn = ProdGrossAmt;
                        }
                        else if (Convert.ToString(dr["ApplicableOn"]) == "N")
                        {
                            obj.calCulatedOn = ProdNetAmt;
                        }
                        else
                        {
                            obj.calCulatedOn = 0;
                        }
                        //End Here
                        #endregion

                        //Debjyoti 09032017
                        if (Convert.ToString(ddl_AmountAre.Value) == "2")
                        {

                            if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST")
                            {
                                decimal finalCalCulatedOn = 0;
                                decimal backProcessRate = (1 + (totalParcentage / 100));
                                finalCalCulatedOn = obj.calCulatedOn / backProcessRate;
                                obj.calCulatedOn = finalCalCulatedOn;
                            }

                        }

                        DataRow[] filtronexsisting1 = TaxRecord.Select("TaxCode=" + obj.Taxes_ID + " and SlNo=" + Convert.ToString(slNo));
                        if (filtronexsisting1.Length > 0)
                        {
                            if (obj.Taxes_ID == 0)
                            {
                                obj.TaxField = "0";
                            }
                            else
                            {
                                obj.TaxField = Convert.ToString(filtronexsisting1[0]["Percentage"]);
                            }
                            obj.Amount = Convert.ToDouble(filtronexsisting1[0]["Amount"]);
                        }
                        else
                        {
                            #region checkingFordb


                            //DataRow[] filtr = databaseReturnTable.Select("ProductTax_ProductId ='" + keyValue + "' and ProductTax_QuoteId=" +Convert.ToString( Session["SI_InvoiceID"] )+ " and ProductTax_TaxTypeId=" + obj.Taxes_ID);
                            //if (filtr.Length > 0)
                            //{
                            //    obj.Amount = Convert.ToDouble(filtr[0]["ProductTax_Amount"]);
                            //    if (obj.Taxes_ID == 0)
                            //    {
                            //        //obj.TaxField = GetTaxName();
                            //        obj.TaxField = "0";
                            //    }
                            //    else
                            //    {
                            //        obj.TaxField = Convert.ToString(filtr[0]["ProductTax_Percentage"]);
                            //    }


                            //    DataRow[] filtronexsisting = TaxRecord.Select("TaxCode=" + obj.Taxes_ID + " and SlNo=" + Convert.ToString(slNo));
                            //    if (filtronexsisting.Length > 0)
                            //    {
                            //        filtronexsisting[0]["Amount"] = obj.Amount;
                            //        if (obj.Taxes_ID == 0)
                            //        {
                            //            filtronexsisting[0]["Percentage"] = 0;
                            //        }
                            //        else
                            //        {
                            //            filtronexsisting[0]["Percentage"] = obj.TaxField;
                            //        }

                            //    }
                            //    else
                            //    {

                            //        DataRow taxRecordNewRow = TaxRecord.NewRow();
                            //        taxRecordNewRow["SlNo"] = slNo;
                            //        taxRecordNewRow["TaxCode"] = obj.Taxes_ID;
                            //        taxRecordNewRow["AltTaxCode"] = "0";
                            //        taxRecordNewRow["Percentage"] = obj.TaxField;
                            //        taxRecordNewRow["Amount"] = obj.Amount;

                            //        TaxRecord.Rows.Add(taxRecordNewRow);
                            //    }

                            //}
                            //else
                            //{
                            //    DataRow[] filtronexsisting = TaxRecord.Select("TaxCode=" + obj.Taxes_ID + " and SlNo=" + Convert.ToString(slNo));
                            //    if (filtronexsisting.Length > 0)
                            //    {
                            //        DataRow taxRecordNewRow = TaxRecord.NewRow();
                            //        taxRecordNewRow["SlNo"] = slNo;
                            //        taxRecordNewRow["TaxCode"] = obj.Taxes_ID;
                            //        taxRecordNewRow["AltTaxCode"] = "0";
                            //        taxRecordNewRow["Percentage"] = 0.0;
                            //        taxRecordNewRow["Amount"] = "0";

                            //        TaxRecord.Rows.Add(taxRecordNewRow);
                            //    }
                            //}




                            #endregion


                            DataRow[] filtronexsisting = TaxRecord.Select("TaxCode=" + obj.Taxes_ID + " and SlNo=" + Convert.ToString(slNo));
                            if (filtronexsisting.Length > 0)
                            {
                                DataRow taxRecordNewRow = TaxRecord.NewRow();
                                taxRecordNewRow["SlNo"] = slNo;
                                taxRecordNewRow["TaxCode"] = obj.Taxes_ID;
                                taxRecordNewRow["AltTaxCode"] = "0";
                                taxRecordNewRow["Percentage"] = 0.0;
                                taxRecordNewRow["Amount"] = "0";

                                TaxRecord.Rows.Add(taxRecordNewRow);
                            }

                        }
                        TaxDetailsDetails.Add(obj);

                        //      DataRow[] filtrIndex = databaseReturnTable.Select("ProductTax_ProductId ='" + keyValue + "' and ProductTax_QuoteId=" + Session["SI_InvoiceID"] + " and ProductTax_TaxTypeId=0");
                        DataRow[] filtrIndex = TaxRecord.Select("SlNo=" + Convert.ToString(slNo) + " and TaxCode=0");
                        if (filtrIndex.Length > 0)
                        {
                            aspxGridTax.JSProperties["cpComboCode"] = Convert.ToString(filtrIndex[0]["AltTaxCode"]);
                        }
                    }
                    Session["SI_FinalTaxRecord" + Convert.ToString(uniqueId.Value)] = TaxRecord;

                }
                //New Changes 170217
                //GstCode should fetch everytime
                DataRow[] finalFiltrIndex = MainTaxDataTable.Select("SlNo=" + Convert.ToString(slNo) + " and TaxCode=0");
                if (finalFiltrIndex.Length > 0)
                {
                    aspxGridTax.JSProperties["cpComboCode"] = Convert.ToString(finalFiltrIndex[0]["AltTaxCode"]);
                }

                aspxGridTax.JSProperties["cpJsonData"] = createJsonForDetails(taxDetail);

                retMsg = Convert.ToString(GetTotalTaxAmount(TaxDetailsDetails));
                aspxGridTax.JSProperties["cpUpdated"] = "ok~" + retMsg;

                TaxDetailsDetails = setCalculatedOn(TaxDetailsDetails, taxDetail);
                aspxGridTax.DataSource = TaxDetailsDetails;
                aspxGridTax.DataBind();

                #endregion
            }
        }
        public string createJsonForDetails(DataTable lstTaxDetails)
        {
            List<TaxSetailsJson> jsonList = new List<TaxSetailsJson>();
            TaxSetailsJson jsonObj;
            int visIndex = 0;
            foreach (DataRow taxObj in lstTaxDetails.Rows)
            {
                jsonObj = new TaxSetailsJson();

                jsonObj.SchemeName = Convert.ToString(taxObj["Taxes_Name"]);
                jsonObj.vissibleIndex = visIndex;
                jsonObj.applicableOn = Convert.ToString(taxObj["ApplicableOn"]);
                if (jsonObj.applicableOn == "G" || jsonObj.applicableOn == "N")
                {
                    jsonObj.applicableBy = "None";
                }
                else
                {
                    jsonObj.applicableBy = Convert.ToString(taxObj["dependOn"]);
                }

                jsonObj.Formula = Convert.ToString(taxObj["Formula"]);
                visIndex++;
                jsonList.Add(jsonObj);
            }

            var oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            return oSerializer.Serialize(jsonList);
        }
        public List<TaxDetails> setCalculatedOn(List<TaxDetails> gridSource, DataTable taxDt)
        {
            foreach (TaxDetails taxObj in gridSource)
            {
                DataRow[] dependOn = taxDt.Select("dependOn='" + taxObj.Taxes_Name.Replace("(+)", "").Replace("(-)", "") + "'");
                if (dependOn.Length > 0)
                {
                    foreach (DataRow dr in dependOn)
                    {
                        //  List<TaxDetails> dependObj = gridSource.Where(r => r.Taxes_Name.Replace("(+)", "").Replace("(-)", "") == Convert.ToString(dependOn[0]["Taxes_Name"])).ToList();
                        foreach (var setCalObj in gridSource.Where(r => r.Taxes_Name.Replace("(+)", "").Replace("(-)", "") == Convert.ToString(dependOn[0]["Taxes_Name"])))
                        {
                            setCalObj.calCulatedOn = Convert.ToDecimal(taxObj.Amount);
                        }
                    }

                }

            }
            return gridSource;
        }

        public class TaxSetailsJson
        {
            public string SchemeName { get; set; }
            public int vissibleIndex { get; set; }
            public string applicableOn { get; set; }
            public string applicableBy { get; set; }
            public string Formula { get; set; }
        }
        public class TaxDetails
        {
            public int Taxes_ID { get; set; }
            public string Taxes_Name { get; set; }

            public double Amount { get; set; }
            public string TaxField { get; set; }

            public string taxCodeName { get; set; }

            public decimal calCulatedOn { get; set; }

        }
        class taxCode
        {
            public string Taxes_ID { get; set; }
            public string Taxes_Name { get; set; }
        }
        protected void aspxGridTax_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridViewEditorEventArgs e)
        {

            if (e.Column.FieldName == "Taxes_Name")
            {
                e.Editor.ReadOnly = true;
            }
            if (e.Column.FieldName == "taxCodeName")
            {
                e.Editor.ReadOnly = true;
            }
            if (e.Column.FieldName == "calCulatedOn")
            {
                e.Editor.ReadOnly = true;
            }
            //else if (e.Column.FieldName == "Amount")
            //{
            //    e.Editor.ReadOnly = true;
            //}
            else
            {
                e.Editor.ReadOnly = false;
            }
        }
        protected void aspxGridTax_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {

        }
        protected void taxgrid_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {

            int slNo = Convert.ToInt32(HdSerialNo.Value);
            DataTable TaxRecord = (DataTable)Session["SI_FinalTaxRecord" + Convert.ToString(uniqueId.Value)];
            foreach (var args in e.UpdateValues)
            {

                string TaxCodeDes = Convert.ToString(args.NewValues["Taxes_Name"]);
                decimal Percentage = 0;

                Percentage = Convert.ToDecimal(args.NewValues["TaxField"]);

                decimal Amount = Convert.ToDecimal(args.NewValues["Amount"]);
                string TaxCode = "0";
                if (!Convert.ToString(args.Keys[0]).Contains('~'))
                {
                    TaxCode = Convert.ToString(args.Keys[0]);
                }



                DataRow[] finalRow = TaxRecord.Select("SlNo=" + Convert.ToString(slNo) + " and TaxCode='" + TaxCode + "'");
                if (finalRow.Length > 0)
                {
                    finalRow[0]["Percentage"] = Percentage;
                    // finalRow[0]["TaxCode"] = args.NewValues["TaxField"]; 
                    finalRow[0]["Amount"] = Amount;

                    finalRow[0]["TaxCode"] = args.Keys[0];
                    finalRow[0]["AltTaxCode"] = "0";

                }
                else
                {
                    DataRow newRow = TaxRecord.NewRow();
                    newRow["slNo"] = slNo;
                    newRow["Percentage"] = Percentage;
                    newRow["TaxCode"] = TaxCode;
                    newRow["AltTaxCode"] = "0";
                    newRow["Amount"] = Amount;
                    TaxRecord.Rows.Add(newRow);
                }


            }

            //For GST/CST/VAT
            if (cmbGstCstVat.Value != null)
            {

                DataRow[] finalRow = TaxRecord.Select("SlNo=" + Convert.ToString(slNo) + " and TaxCode='0'");
                if (finalRow.Length > 0)
                {
                    finalRow[0]["Percentage"] = Convert.ToString(cmbGstCstVat.Value).Split('~')[1];
                    finalRow[0]["Amount"] = txtGstCstVat.Text;
                    finalRow[0]["AltTaxCode"] = Convert.ToString(cmbGstCstVat.Value).Split('~')[0];

                }
                else
                {
                    DataRow newRowGST = TaxRecord.NewRow();
                    newRowGST["slNo"] = slNo;
                    newRowGST["Percentage"] = Convert.ToString(cmbGstCstVat.Value).Split('~')[1];
                    newRowGST["TaxCode"] = "0";
                    newRowGST["AltTaxCode"] = Convert.ToString(cmbGstCstVat.Value).Split('~')[0];
                    newRowGST["Amount"] = txtGstCstVat.Text;
                    TaxRecord.Rows.Add(newRowGST);
                }
            }
            //End Here


            Session["SI_FinalTaxRecord" + Convert.ToString(uniqueId.Value)] = TaxRecord;



            DataRow[] finalRowGSTTotal = TaxRecord.Select("SlNo=" + Convert.ToString(slNo));
            if (finalRowGSTTotal.Length > 0)
            {
                DataTable TaxTable = finalRowGSTTotal.CopyToDataTable();
                DataSet dsInst = new DataSet();


                //  SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);

                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

                SqlCommand cmd = new SqlCommand("prc_PosSalesInvoice", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "GetTotalGSTVALUE");
                cmd.Parameters.AddWithValue("@TaxDetail", TaxTable);

                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                con.Dispose();

                aspxGridTax.JSProperties["cpTotalGST"] = dsInst.Tables[0].Rows[0][0];
                aspxGridTax.JSProperties["cpGSTType"] = dsInst.Tables[0].Rows[0][1];
            }


            #region oldpart


            //DataTable taxdtByProductCode = new DataTable();
            //if (Session["ProdTax" + "_" + Convert.ToString(HdSerialNo.Value)] == null)
            //{

            //    taxdtByProductCode.TableName = "ProdTax"  + "_" + Convert.ToString(HdSerialNo.Value);


            //    taxdtByProductCode.Columns.Add("TaxCode", typeof(System.String));
            //    taxdtByProductCode.Columns.Add("TaxCodeDescription", typeof(System.String));
            //    taxdtByProductCode.Columns.Add("Percentage", typeof(System.Decimal));
            //    taxdtByProductCode.Columns.Add("Amount", typeof(System.Decimal));
            //    DataRow dr;
            //    foreach (var args in e.UpdateValues)
            //    {
            //        dr = taxdtByProductCode.NewRow();
            //        string TaxCodeDes = Convert.ToString(args.NewValues["Taxes_Name"]);
            //        decimal Percentage = 0;
            //        if (TaxCodeDes == "GST/CST/VAT")
            //        {
            //            Percentage = Convert.ToDecimal(Convert.ToString(args.NewValues["TaxField"]).Split('~')[1]);
            //        }
            //        else
            //        {
            //            Percentage = Convert.ToDecimal(args.NewValues["TaxField"]);
            //        }
            //        decimal Amount = Convert.ToDecimal(args.NewValues["Amount"]);

            //        dr["TaxCodeDescription"] = TaxCodeDes;
            //        if (Convert.ToString(args.Keys[0]) == "0")
            //        {
            //            dr["TaxCode"] = "0~" + Convert.ToString(args.NewValues["TaxField"]).Split('~')[0];
            //        }
            //        else
            //        {
            //            dr["TaxCode"] = args.Keys[0];
            //        }
            //        dr["Percentage"] = Percentage;
            //        dr["Amount"] = Amount;

            //        taxdtByProductCode.Rows.Add(dr);
            //    }
            //}
            //else
            //{
            //    taxdtByProductCode = (DataTable)Session["ProdTax"  +"_"+ Convert.ToString(HdSerialNo.Value)];

            //    foreach (var args in e.UpdateValues)
            //    {
            //        DataRow[] filtr ;

            //        if (Convert.ToString(args.Keys[0]) == "0")
            //        {
            //            filtr = taxdtByProductCode.Select("TaxCode like '%0~%'"); ;
            //        }
            //        else
            //        {
            //            filtr = taxdtByProductCode.Select("TaxCode='" + args.Keys[0]+"'");
            //        }

            //        if (filtr.Length > 0)
            //        {
            //            string TaxCodeDes = Convert.ToString(args.NewValues["Taxes_Name"]);
            //        filtr[0]["TaxCodeDescription"] = TaxCodeDes;
            //        if (Convert.ToString(args.Keys[0]) == "0")
            //        {
            //            filtr[0]["TaxCode"] = "0~" + Convert.ToString(args.NewValues["TaxField"]).Split('~')[0];
            //        }
            //        else
            //        {
            //            filtr[0]["TaxCode"] = args.Keys[0];
            //        }

            //        decimal Percentage = 0;
            //        if (TaxCodeDes == "GST/CST/VAT")
            //        {
            //            Percentage = Convert.ToDecimal(Convert.ToString(args.NewValues["TaxField"]).Split('~')[1]);
            //        }
            //        else
            //        {
            //            Percentage = Convert.ToDecimal(args.NewValues["TaxField"]);
            //        }
            //        decimal Amount = Convert.ToDecimal(args.NewValues["Amount"]);
            //        filtr[0]["Percentage"] = Percentage;
            //        filtr[0]["Amount"] = Amount;

            //        }
            //    }


            //}

            #endregion
            //  Session[taxdtByProductCode.TableName] = taxdtByProductCode;

        }
        protected void cmbGstCstVat_Callback(object sender, CallbackEventArgsBase e)
        {
            DateTime quoteDate = Convert.ToDateTime(dt_PLQuote.Date.ToString("yyyy-MM-dd"));

            PopulateGSTCSTVATCombo(quoteDate.ToString("yyyy-MM-dd"));
            CreateDataTaxTable();
            //DataTable taxTableItemLvl = (DataTable)Session["SI_FinalTaxRecord"];
            //foreach (DataRow dr in taxTableItemLvl.Rows)
            //    dr.Delete();
            //taxTableItemLvl.AcceptChanges();
            //Session["SI_FinalTaxRecord"] = taxTableItemLvl;
        }
        protected void cmbGstCstVatcharge_Callback(object sender, CallbackEventArgsBase e)
        {
            Session["SI_TaxDetails" + Convert.ToString(uniqueId.Value)] = null;
            DateTime quoteDate = Convert.ToDateTime(dt_PLQuote.Date.ToString("yyyy-MM-dd"));
            PopulateChargeGSTCSTVATCombo(quoteDate.ToString("yyyy-MM-dd"));
        }

        protected void taxgrid_RowInserting(object sender, ASPxDataInsertingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void taxgrid_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void taxgrid_RowDeleting(object sender, ASPxDataDeletingEventArgs e)
        {
            e.Cancel = true;
        }
        public void DeleteTaxDetails(string SrlNo)
        {
            DataTable TaxDetailTable = new DataTable();
            if (Session["SI_FinalTaxRecord" + Convert.ToString(uniqueId.Value)] != null)
            {
                TaxDetailTable = (DataTable)Session["SI_FinalTaxRecord" + Convert.ToString(uniqueId.Value)];

                var rows = TaxDetailTable.Select("SlNo ='" + SrlNo + "'");
                foreach (var row in rows)
                {
                    row.Delete();
                }
                TaxDetailTable.AcceptChanges();

                Session["SI_FinalTaxRecord" + Convert.ToString(uniqueId.Value)] = TaxDetailTable;
            }
        }
        public void UpdateTaxDetails(string oldSrlNo, string newSrlNo)
        {
            DataTable TaxDetailTable = new DataTable();
            if (Session["SI_FinalTaxRecord" + Convert.ToString(uniqueId.Value)] != null)
            {
                TaxDetailTable = (DataTable)Session["SI_FinalTaxRecord" + Convert.ToString(uniqueId.Value)];

                for (int i = 0; i < TaxDetailTable.Rows.Count; i++)
                {
                    DataRow dr = TaxDetailTable.Rows[i];
                    string Product_SrlNo = Convert.ToString(dr["SlNo"]);
                    if (oldSrlNo == Product_SrlNo)
                    {
                        dr["SlNo"] = newSrlNo;
                    }
                }
                TaxDetailTable.AcceptChanges();

                Session["SI_FinalTaxRecord" + Convert.ToString(uniqueId.Value)] = TaxDetailTable;
            }
        }


        public string createJsonForChargesTax(DataTable lstTaxDetails)
        {
            List<TaxSetailsJson> jsonList = new List<TaxSetailsJson>();
            TaxSetailsJson jsonObj;
            int visIndex = 0;
            foreach (DataRow taxObj in lstTaxDetails.Rows)
            {
                jsonObj = new TaxSetailsJson();

                jsonObj.SchemeName = Convert.ToString(taxObj["Taxes_Name"]);
                jsonObj.vissibleIndex = visIndex;
                jsonObj.applicableOn = Convert.ToString(taxObj["ApplicableOn"]);
                if (jsonObj.applicableOn == "G" || jsonObj.applicableOn == "N")
                {
                    jsonObj.applicableBy = "None";
                }
                else
                {
                    jsonObj.applicableBy = Convert.ToString(taxObj["dependOn"]);
                }
                visIndex++;
                jsonList.Add(jsonObj);
            }

            var oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            return oSerializer.Serialize(jsonList);
        }
        public List<Taxes> setChargeCalculatedOn(List<Taxes> gridSource, DataTable taxDt)
        {
            foreach (Taxes taxObj in gridSource)
            {
                DataRow[] dependOn = taxDt.Select("dependOn='" + taxObj.TaxName.Replace("(+)", "").Replace("(-)", "").Trim() + "'");
                if (dependOn.Length > 0)
                {
                    foreach (DataRow dr in dependOn)
                    {
                        //  List<TaxDetails> dependObj = gridSource.Where(r => r.Taxes_Name.Replace("(+)", "").Replace("(-)", "") == Convert.ToString(dependOn[0]["Taxes_Name"])).ToList();
                        foreach (var setCalObj in gridSource.Where(r => r.TaxName.Replace("(+)", "").Replace("(-)", "").Trim() == Convert.ToString(dependOn[0]["Taxes_Name"]).Replace("(+)", "").Replace("(-)", "").Trim()))
                        {
                            setCalObj.calCulatedOn = Convert.ToDecimal(taxObj.Amount);
                        }
                    }

                }

            }
            return gridSource;
        }

        public void PopulateChargeGSTCSTVATCombo(string quoteDate)
        {
            string LastCompany = "";
            if (Convert.ToString(Session["LastCompany"]) != null)
            {
                LastCompany = Convert.ToString(Session["LastCompany"]);
            }
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 500, "LoadChargeGSTCSTVATCombo");
            proc.AddVarcharPara("@S_QuoteAdd_CompanyID", 10, Convert.ToString(LastCompany));
            proc.AddVarcharPara("@S_quoteDate", 10, quoteDate);
            DataTable DT = proc.GetTable();
            cmbGstCstVatcharge.DataSource = DT;
            cmbGstCstVatcharge.TextField = "Taxes_Name";
            cmbGstCstVatcharge.ValueField = "Taxes_ID";
            cmbGstCstVatcharge.DataBind();
        }

        #endregion

        #region  Billing and Shipping DetailSection Start


        public DataTable StoreQuotationAddressDetail()
        {
            //QuoteAdd_id, QuoteAdd_QuoteId, QuoteAdd_CompanyID, QuoteAdd_BranchId, QuoteAdd_FinYear,
            //QuoteAdd_ContactPerson, QuoteAdd_addressType, QuoteAdd_address1, QuoteAdd_address2, QuoteAdd_address3, 
            //QuoteAdd_landMark, QuoteAdd_countryId, QuoteAdd_stateId, QuoteAdd_cityId, QuoteAdd_areaId, 
            //QuoteAdd_pin, QuoteAdd_CreatedDate, QuoteAdd_CreatedUser, QuoteAdd_LastModifyDate, QuoteAdd_LastModifyUser

            DataTable AddressDetaildt = new DataTable();

            AddressDetaildt.Columns.Add("QuoteAdd_QuoteId", typeof(System.Int32));
            AddressDetaildt.Columns.Add("QuoteAdd_CompanyID", typeof(System.String));
            AddressDetaildt.Columns.Add("QuoteAdd_BranchId", typeof(System.Int32));
            AddressDetaildt.Columns.Add("QuoteAdd_FinYear", typeof(System.String));

            AddressDetaildt.Columns.Add("QuoteAdd_ContactPerson", typeof(System.String));
            AddressDetaildt.Columns.Add("QuoteAdd_addressType", typeof(System.String));
            AddressDetaildt.Columns.Add("QuoteAdd_address1", typeof(System.String));
            AddressDetaildt.Columns.Add("QuoteAdd_address2", typeof(System.String));
            AddressDetaildt.Columns.Add("QuoteAdd_address3", typeof(System.String));


            AddressDetaildt.Columns.Add("QuoteAdd_landMark", typeof(System.String));
            AddressDetaildt.Columns.Add("QuoteAdd_countryId", typeof(System.Int32));
            AddressDetaildt.Columns.Add("QuoteAdd_stateId", typeof(System.Int32));
            AddressDetaildt.Columns.Add("QuoteAdd_cityId", typeof(System.Int32));
            AddressDetaildt.Columns.Add("QuoteAdd_areaId", typeof(System.Int32));


            AddressDetaildt.Columns.Add("QuoteAdd_pin", typeof(System.String));
            AddressDetaildt.Columns.Add("QuoteAdd_CreatedDate", typeof(System.DateTime));
            AddressDetaildt.Columns.Add("QuoteAdd_CreatedUser", typeof(System.Int32));
            AddressDetaildt.Columns.Add("QuoteAdd_LastModifyDate", typeof(System.DateTime));
            AddressDetaildt.Columns.Add("QuoteAdd_LastModifyUser", typeof(System.Int32));
            return AddressDetaildt;


        }
        protected void ComponentPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            #region Addresss Lookup Section Start
            DataSet dst = new DataSet();
            POSCRMSalesDtlBL objCRMSalesDtlBL = new POSCRMSalesDtlBL();
            dst = objCRMSalesDtlBL.PopulateBillingandShippingDetailByCustomerID(hdnCustomerId.Value);

            if (dst.Tables[0].Rows.Count > 0)
            {
                Session["SI_BillingAddressLookup"] = dst.Tables[0];
            }

            if (dst.Tables[1].Rows.Count > 0)
            {
                Session["SI_ShippingAddressLookup"] = dst.Tables[1];
            }


            #endregion Addresss Lookup Section End

            #region Variable Declaration to send value using jsproperties Start
            string add_addressType = "";
            string add_address1 = "";
            string add_address2 = "";
            string add_address3 = "";
            string add_landMark = "";
            string add_country = "";
            string add_state = "";
            string add_city = "";
            string add_pin = "";
            string add_area = "";

            ///// shipping variable

            string add_saddressType = "";
            string add_saddress1 = "";
            string add_saddress2 = "";
            string add_saddress3 = "";
            string add_slandMark = "";
            string add_scountry = "";
            string add_sstate = "";
            string add_scity = "";
            string add_spin = "";
            string add_sarea = "";

            #endregion Variable Declaration to send value using jsproperties Start
            ComponentPanel.JSProperties["cpshow"] = null;
            ComponentPanel.JSProperties["cpshowShip"] = null;
            string WhichCall = e.Parameter.Split('~')[0];

            ComponentPanel.JSProperties["cpEParameter"] = WhichCall;


            #region Edit Section of Address Start

            if (WhichCall == "Edit")
            {
                if (Convert.ToString(DeleteCustomer.Value) == "yes")
                {
                    Session["SI_QuotationAddressDtl"] = null;
                }
                //DataTable dtaddress=(DataTable)
                //string AddressStatus = Convert.ToString(e.Parameter.Split('~')[1]);
                if (Session["SI_QuotationAddressDtl"] == null)
                {
                    string customerid = hdnCustomerId.Value;
                    #region Billing Detail fillup
                    DataTable dtaddbill = oDBEngine.GetDataTable("select add_addressType,add_address1,add_address2,add_address3,add_landMark,add_country,add_state,add_city,add_pin,add_area from tbl_master_address where add_cntId='" + customerid + "' and add_addressType='Billing' and Isdefault='1' ");

                    #region Function To get All Detail

                    if (dtaddbill.Rows.Count > 0 && dtaddbill != null)
                    {

                        //for (int m = 0; m < dtaddbill.Rows.Count; m++)
                        //{
                        add_addressType = Convert.ToString(dtaddbill.Rows[0]["add_addressType"]);
                        add_address1 = Convert.ToString(dtaddbill.Rows[0]["add_address1"]);
                        add_address2 = Convert.ToString(dtaddbill.Rows[0]["add_address2"]);
                        add_address3 = Convert.ToString(dtaddbill.Rows[0]["add_address3"]);
                        add_landMark = Convert.ToString(dtaddbill.Rows[0]["add_landMark"]);
                        add_country = Convert.ToString(dtaddbill.Rows[0]["add_country"]);
                        add_state = Convert.ToString(dtaddbill.Rows[0]["add_state"]);
                        add_city = Convert.ToString(dtaddbill.Rows[0]["add_city"]);
                        add_pin = Convert.ToString(dtaddbill.Rows[0]["add_pin"]);
                        add_area = Convert.ToString(dtaddbill.Rows[0]["add_area"]);


                        populateNewAddressDetailsByPIN(add_address1, add_address2, add_address3, add_landMark, add_country, add_state, add_city, add_pin, add_area);


                        ComponentPanel.JSProperties["cpshow"] = add_addressType + "~"
                                                           + add_address1 + "~"
                                                           + add_address2 + "~"
                                                           + add_address3 + "~"
                                                           + add_landMark + "~"
                                                           + add_country + "~"
                                                           + add_state + "~"
                                                           + add_city + "~"
                                                           + add_pin + "~"
                                                           + add_area + "~"
                                                           + "Y" + "~";

                    }
                    else
                    {

                        txtAddress1.Text = "";
                        txtAddress2.Text = "";
                        txtAddress3.Text = "";
                        txtlandmark.Text = "";
                    }
                    #endregion Function Calling End
                    #endregion Billing Detail fillup end
                    #region Shipping Detail fillup
                    DataTable dtaship = oDBEngine.GetDataTable("select add_addressType,add_address1,add_address2,add_address3,add_landMark,add_country,add_state,add_city,add_pin,add_area from tbl_master_address where add_cntId='" + customerid + "' and add_addressType='Shipping' and Isdefault='1' ");
                    if (dtaship.Rows.Count > 0 && dtaship != null)
                    {
                        add_saddressType = Convert.ToString(dtaship.Rows[0]["add_addressType"]);
                        add_saddress1 = Convert.ToString(dtaship.Rows[0]["add_address1"]);
                        add_saddress2 = Convert.ToString(dtaship.Rows[0]["add_address2"]);
                        add_saddress3 = Convert.ToString(dtaship.Rows[0]["add_address3"]);
                        add_slandMark = Convert.ToString(dtaship.Rows[0]["add_landMark"]);
                        add_scountry = Convert.ToString(dtaship.Rows[0]["add_country"]);
                        add_sstate = Convert.ToString(dtaship.Rows[0]["add_state"]);
                        add_scity = Convert.ToString(dtaship.Rows[0]["add_city"]);
                        add_spin = Convert.ToString(dtaship.Rows[0]["add_pin"]);
                        add_sarea = Convert.ToString(dtaship.Rows[0]["add_area"]);

                        populateNewAddressDetailsShippingByPIN(add_saddress1, add_saddress2, add_saddress3, add_slandMark, add_scountry, add_sstate, add_scity, add_spin, add_sarea);

                        ComponentPanel.JSProperties["cpshowShip"] = add_saddressType + "~"
                                                          + add_saddress1 + "~"
                                                          + add_saddress2 + "~"
                                                          + add_saddress3 + "~"
                                                          + add_slandMark + "~"
                                                          + add_scountry + "~"
                                                          + add_sstate + "~"
                                                          + add_scity + "~"
                                                          + add_spin + "~"
                                                          + add_sarea + "~"
                                                          + "Y" + "~";

                    }
                    else
                    {
                        txtShippingPin.Text = "";
                        txtsAddress1.Text = "";
                        txtsAddress2.Text = "";
                        txtsAddress3.Text = "";
                        txtslandmark.Text = "";
                    }
                    #endregion Shipping detail Fillup
                }
                else if (Session["SI_QuotationAddressDtl"] != null)
                {
                    DataTable dt = (DataTable)Session["SI_QuotationAddressDtl"];
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows.Count == 2) // when 2 row  data found in edit mode
                        {
                            #region billing Address Dtl using session
                            add_addressType = Convert.ToString(dt.Rows[0]["QuoteAdd_addressType"]);
                            add_address1 = Convert.ToString(dt.Rows[0]["QuoteAdd_address1"]);
                            add_address2 = Convert.ToString(dt.Rows[0]["QuoteAdd_address2"]);
                            add_address3 = Convert.ToString(dt.Rows[0]["QuoteAdd_address3"]);
                            add_landMark = Convert.ToString(dt.Rows[0]["QuoteAdd_landMark"]);
                            add_country = Convert.ToString(dt.Rows[0]["QuoteAdd_countryId"]);
                            add_state = Convert.ToString(dt.Rows[0]["QuoteAdd_stateId"]);
                            add_city = Convert.ToString(dt.Rows[0]["QuoteAdd_cityId"]);
                            add_pin = Convert.ToString(dt.Rows[0]["QuoteAdd_pin"]);
                            add_area = Convert.ToString(dt.Rows[0]["QuoteAdd_areaId"]);

                            populateNewAddressDetailsByPIN(add_address1, add_address2, add_address3, add_landMark, add_country, add_state, add_city, add_pin, add_area);

                            ComponentPanel.JSProperties["cpshow"] = add_addressType + "~"
                                                                  + add_address1 + "~"
                                                                  + add_address2 + "~"
                                                                  + add_address3 + "~"
                                                                  + add_landMark + "~"
                                                                  + add_country + "~"
                                                                  + add_state + "~"
                                                                  + add_city + "~"
                                                                  + add_pin + "~"
                                                                  + add_area + "~"
                                                                  + "Y" + "~";
                            #endregion billing Address Dtl using session
                            #region Shipping Address Dtl using session
                            add_saddressType = Convert.ToString(dt.Rows[1]["QuoteAdd_addressType"]);
                            add_saddress1 = Convert.ToString(dt.Rows[1]["QuoteAdd_address1"]);
                            add_saddress2 = Convert.ToString(dt.Rows[1]["QuoteAdd_address2"]);
                            add_saddress3 = Convert.ToString(dt.Rows[1]["QuoteAdd_address3"]);
                            add_slandMark = Convert.ToString(dt.Rows[1]["QuoteAdd_landMark"]);
                            add_scountry = Convert.ToString(dt.Rows[1]["QuoteAdd_countryId"]);
                            add_sstate = Convert.ToString(dt.Rows[1]["QuoteAdd_stateId"]);
                            add_scity = Convert.ToString(dt.Rows[1]["QuoteAdd_cityId"]);
                            add_spin = Convert.ToString(dt.Rows[1]["QuoteAdd_pin"]);
                            add_sarea = Convert.ToString(dt.Rows[1]["QuoteAdd_areaId"]);

                            populateNewAddressDetailsShippingByPIN(add_saddress1, add_saddress2, add_saddress3, add_slandMark, add_scountry, add_sstate, add_scity, add_spin, add_sarea);

                            ComponentPanel.JSProperties["cpshowShip"] = add_saddressType + "~"
                                                                 + add_saddress1 + "~"
                                                                 + add_saddress2 + "~"
                                                                 + add_saddress3 + "~"
                                                                 + add_slandMark + "~"
                                                                 + add_scountry + "~"
                                                                 + add_sstate + "~"
                                                                 + add_scity + "~"
                                                                 + add_spin + "~"
                                                                 + add_sarea + "~"
                                                                 + "Y" + "~";
                            #endregion Shipping Address Dtl using session end

                        }
                        else if (dt.Rows.Count == 1) // when 1 row  data found in edit mode
                        {
                            if (Convert.ToString(dt.Rows[0]["QuoteAdd_addressType"]) == "Billing")
                            {
                                #region billing Address Dtl using session
                                add_addressType = Convert.ToString(dt.Rows[0]["QuoteAdd_addressType"]);
                                add_address1 = Convert.ToString(dt.Rows[0]["QuoteAdd_address1"]);
                                add_address2 = Convert.ToString(dt.Rows[0]["QuoteAdd_address2"]);
                                add_address3 = Convert.ToString(dt.Rows[0]["QuoteAdd_address3"]);
                                add_landMark = Convert.ToString(dt.Rows[0]["QuoteAdd_landMark"]);
                                add_country = Convert.ToString(dt.Rows[0]["QuoteAdd_countryId"]);
                                add_state = Convert.ToString(dt.Rows[0]["QuoteAdd_stateId"]);
                                add_city = Convert.ToString(dt.Rows[0]["QuoteAdd_cityId"]);
                                add_pin = Convert.ToString(dt.Rows[0]["QuoteAdd_pin"]);
                                add_area = Convert.ToString(dt.Rows[0]["QuoteAdd_areaId"]);

                                populateNewAddressDetailsByPIN(add_address1, add_address2, add_address3, add_landMark, add_country, add_state, add_city, add_pin, add_area);

                                ComponentPanel.JSProperties["cpshow"] = add_addressType + "~"
                                                                      + add_address1 + "~"
                                                                      + add_address2 + "~"
                                                                      + add_address3 + "~"
                                                                      + add_landMark + "~"
                                                                      + add_country + "~"
                                                                      + add_state + "~"
                                                                      + add_city + "~"
                                                                      + add_pin + "~"
                                                                      + add_area + "~"
                                                                      + "Y" + "~";

                                ComponentPanel.JSProperties["cpshowShip"] = add_saddressType + "~"
                                                     + add_saddress1 + "~"
                                                     + add_saddress2 + "~"
                                                     + add_saddress3 + "~"
                                                     + add_slandMark + "~"
                                                     + add_scountry + "~"
                                                     + add_sstate + "~"
                                                     + add_scity + "~"
                                                     + add_spin + "~"
                                                     + add_sarea + "~"
                                                      + "N" + "~";

                                #endregion billing Address Dtl using session
                            }
                            if (Convert.ToString(dt.Rows[0]["QuoteAdd_addressType"]) == "Shipping")
                            {
                                #region Shipping Address Dtl using session
                                ComponentPanel.JSProperties["cpshow"] = add_addressType + "~"
                                                     + add_address1 + "~"
                                                     + add_address2 + "~"
                                                     + add_address3 + "~"
                                                     + add_landMark + "~"
                                                     + add_country + "~"
                                                     + add_state + "~"
                                                     + add_city + "~"
                                                     + add_pin + "~"
                                                     + add_area + "~"
                                                      + "N" + "~";

                                add_saddressType = Convert.ToString(dt.Rows[0]["QuoteAdd_addressType"]);
                                add_saddress1 = Convert.ToString(dt.Rows[0]["QuoteAdd_address1"]);
                                add_saddress2 = Convert.ToString(dt.Rows[0]["QuoteAdd_address2"]);
                                add_saddress3 = Convert.ToString(dt.Rows[0]["QuoteAdd_address3"]);
                                add_slandMark = Convert.ToString(dt.Rows[0]["QuoteAdd_landMark"]);
                                add_scountry = Convert.ToString(dt.Rows[0]["QuoteAdd_countryId"]);
                                add_sstate = Convert.ToString(dt.Rows[0]["QuoteAdd_stateId"]);
                                add_scity = Convert.ToString(dt.Rows[0]["QuoteAdd_cityId"]);
                                add_spin = Convert.ToString(dt.Rows[0]["QuoteAdd_pin"]);
                                add_sarea = Convert.ToString(dt.Rows[0]["QuoteAdd_areaId"]);

                                populateNewAddressDetailsShippingByPIN(add_saddress1, add_saddress2, add_saddress3, add_slandMark, add_scountry, add_sstate, add_scity, add_spin, add_sarea);
                                ComponentPanel.JSProperties["cpshowShip"] = add_saddressType + "~"
                                                                     + add_saddress1 + "~"
                                                                     + add_saddress2 + "~"
                                                                     + add_saddress3 + "~"
                                                                     + add_slandMark + "~"
                                                                     + add_scountry + "~"
                                                                     + add_sstate + "~"
                                                                     + add_scity + "~"
                                                                     + add_spin + "~"
                                                                     + add_sarea + "~"
                                                                     + "Y" + "~";
                                #endregion Shipping Address Dtl using session end
                            }
                        }
                        else // when no data found in edit mode
                        {
                            #region billing Address Dtl using session

                            ComponentPanel.JSProperties["cpshow"] = add_addressType + "~"
                                                                  + add_address1 + "~"
                                                                  + add_address2 + "~"
                                                                  + add_address3 + "~"
                                                                  + add_landMark + "~"
                                                                  + add_country + "~"
                                                                  + add_state + "~"
                                                                  + add_city + "~"
                                                                  + add_pin + "~"
                                                                  + add_area + "~"
                                                                  + "Y" + "~";
                            #endregion billing Address Dtl using session
                            #region Shipping Address Dtl using session

                            ComponentPanel.JSProperties["cpshowShip"] = add_saddressType + "~"
                                                                 + add_saddress1 + "~"
                                                                 + add_saddress2 + "~"
                                                                 + add_saddress3 + "~"
                                                                 + add_slandMark + "~"
                                                                 + add_scountry + "~"
                                                                 + add_sstate + "~"
                                                                 + add_scity + "~"
                                                                 + add_spin + "~"
                                                                 + add_sarea + "~"
                                                                 + "Y" + "~";

                            #endregion Shipping Address Dtl using session end

                        }
                    }
                }
            }
            #endregion Edit Section of Address End

            #region Save Section of Address Start
            if (WhichCall == "save")
            {
                #region Global Data for Address Start
                string companyId = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                int branchid = Convert.ToInt32(HttpContext.Current.Session["userbranchID"]);
                string fin_year = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);
                int userid = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                #endregion Global Data for Address End
                string AddressStatus = e.Parameter.Split('~')[1];
                if (AddressStatus == "1") // Both Billing and Shipping Address Available
                {
                    #region Billing Address Detail Start
                    string contactperson = "";


                    //int insertcount = 0;

                    //string AddressType = Convert.ToString(CmbAddressType.SelectedItem.Value);
                    string AddressType = "Billing";
                    string address1 = txtAddress1.Text;
                    string address2 = txtAddress2.Text;
                    string address3 = txtAddress3.Text;
                    string landmark = txtlandmark.Text;
                    int country = Convert.ToInt32(lblBillingCountryValue.Value);
                    int State = Convert.ToInt32(lblBillingStateValue.Value);
                    int city = Convert.ToInt32(lblBillingCityValue.Value);
                    int area = Convert.ToInt32(CmbArea.Value);
                    int pin = Convert.ToInt32(hdBillingPin.Value);
                    DataTable dt = StoreQuotationAddressDetail();
                    dt.Rows.Add(0, companyId, branchid, fin_year, "", AddressType, address1, address2, address3, landmark, country, State, city, area, pin, System.DateTime.Now, userid, System.DateTime.Now, userid);




                    #endregion Billing Address Detail Start end

                    #region Shipping Address Detail Start
                    // CRMSalesAddressEL objCRMSalesSAddress  = new CRMSalesAddressEL();
                    string scontactperson = "";
                    //string sAddressType = Convert.ToString(CmbAddressType1.SelectedItem.Value);
                    string sAddressType = "Shipping";
                    string saddress1 = txtsAddress1.Text;
                    string saddress2 = txtsAddress2.Text;
                    string saddress3 = txtsAddress3.Text;
                    string slandmark = txtslandmark.Text;
                    int scountry = Convert.ToInt32(lblShippingCountryValue.Value);
                    int sState = Convert.ToInt32(lblShippingStateValue.Value);
                    int scity = Convert.ToInt32(lblShippingCityValue.Value);
                    int sarea = Convert.ToInt32(CmbArea1.Value);
                    string spin = Convert.ToString(hdShippingPin.Value);
                    dt.Rows.Add(0, companyId, branchid, fin_year, "", sAddressType, saddress1, saddress2, saddress3, slandmark, scountry, sState, scity, sarea, spin, System.DateTime.Now, userid, System.DateTime.Now, userid);


                    Session["SI_QuotationAddressDtl"] = dt;
                    #endregion Shipping Address Detail Start end
                }

                lblShippingCountry.Text = Convert.ToString(hdlblShippingCountry.Value);
                lblShippingState.Text = Convert.ToString(hdlblShippingState.Value);
                lblShippingCity.Text = Convert.ToString(hdlblShippingCity.Value);
                lblBillingCountry.Text = Convert.ToString(hdlblBillingCountry.Value);
                lblBillingState.Text = Convert.ToString(hdlblBillingState.Value);
                lblBillingCity.Text = Convert.ToString(hdlblBillingCity.Value);

            }

            #endregion Save Section of Address Start

        }

        private void populateNewAddressDetailsByPIN(string add_address1, string add_address2, string add_address3, string add_landMark, string add_country, string add_state, string add_city, string add_pin, string add_area)
        {
            DataSet PopulateAllAddressDetails = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_posCustomerBillingShipping");
            proc.AddVarcharPara("@Action", 50, "PopulateLabelDetailsByPin");
            proc.AddVarcharPara("@pinId", 10, add_pin.ToString());
            PopulateAllAddressDetails = proc.GetDataSet();

            //set Country 
            lblBillingCountryValue.Value = Convert.ToString(PopulateAllAddressDetails.Tables[0].Rows[0]["cou_id"]);
            lblBillingCountry.Text = Convert.ToString(PopulateAllAddressDetails.Tables[0].Rows[0]["cou_country"]);

            //Set State
            lblBillingStateValue.Value = Convert.ToString(PopulateAllAddressDetails.Tables[0].Rows[0]["stateId"]);
            lblBillingState.Text = Convert.ToString(PopulateAllAddressDetails.Tables[0].Rows[0]["state"]);
            lblBillingStateText.Value = Convert.ToString(PopulateAllAddressDetails.Tables[0].Rows[0]["state"]);
            //Set city
            lblBillingCityValue.Value = Convert.ToString(PopulateAllAddressDetails.Tables[0].Rows[0]["city_id"]);
            lblBillingCity.Text = Convert.ToString(PopulateAllAddressDetails.Tables[0].Rows[0]["city_name"]);



            //set Area
            CmbArea.DataSource = PopulateAllAddressDetails.Tables[1];
            CmbArea.TextField = "area_name";
            CmbArea.ValueField = "area_id";
            CmbArea.DataBind();
            CmbArea.Value = add_area;

            txtAddress1.Text = add_address1;
            txtAddress2.Text = add_address2;
            txtAddress3.Text = add_address3;
            txtlandmark.Text = add_landMark;
        }

        #endregion





        #region populate Shipping Address
        private void populateNewAddressDetailsShippingByPIN(string add_address1, string add_address2, string add_address3, string add_landMark, string add_country, string add_state, string add_city, string add_pin, string add_area)
        {
            DataSet PopulateAllAddressDetails = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_posCustomerBillingShipping");
            proc.AddVarcharPara("@Action", 50, "PopulateLabelDetailsByPin");
            proc.AddVarcharPara("@pinId", 10, add_pin.ToString());
            PopulateAllAddressDetails = proc.GetDataSet();

            //set Country 
            lblShippingCountryValue.Value = Convert.ToString(PopulateAllAddressDetails.Tables[0].Rows[0]["cou_id"]);
            lblShippingCountry.Text = Convert.ToString(PopulateAllAddressDetails.Tables[0].Rows[0]["cou_country"]);

            //Set State
            lblShippingStateValue.Value = Convert.ToString(PopulateAllAddressDetails.Tables[0].Rows[0]["stateId"]);
            lblShippingState.Text = Convert.ToString(PopulateAllAddressDetails.Tables[0].Rows[0]["state"]);
            lblShippingStateText.Value = Convert.ToString(PopulateAllAddressDetails.Tables[0].Rows[0]["state"]);

            //Set city
            lblShippingCityValue.Value = Convert.ToString(PopulateAllAddressDetails.Tables[0].Rows[0]["city_id"]);
            lblShippingCity.Text = Convert.ToString(PopulateAllAddressDetails.Tables[0].Rows[0]["city_name"]);
            hdShippingPin.Value = add_pin;

            //set Area
            CmbArea1.DataSource = PopulateAllAddressDetails.Tables[1];
            CmbArea1.TextField = "area_name";
            CmbArea1.ValueField = "area_id";
            CmbArea1.DataBind();
            CmbArea1.Value = add_area;

            txtsAddress1.Text = add_address1;
            txtsAddress2.Text = add_address2;
            txtsAddress3.Text = add_address3;
            txtslandmark.Text = add_landMark;
        }


        #endregion




        #region Header Portion Detail of the Page

        #region Check Billing and Shipping Address

        [WebMethod]
        public static int CheckCustomerBillingShippingAddress(string Customerid)
        {
            int addressStatus = 0;
            try
            {
                CRMSalesDtlBL objCRMSalesDtlBL = new CRMSalesDtlBL();
                addressStatus = objCRMSalesDtlBL.CheckCustomerBillingShippingAddress(Customerid);
            }
            catch (Exception ex)
            {

            }
            finally
            {
            }
            return addressStatus;
        }

        #endregion Check Billing and Shipping Address
        //chinmoy commented
        [WebMethod]
        public static bool CheckUniqueCode(string QuoteNo)
        {
            bool flag = false;
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
            try
            {
                MShortNameCheckingBL objShortNameChecking = new MShortNameCheckingBL();
                flag = objShortNameChecking.CheckUnique(QuoteNo, "0", "Quotation");
            }
            catch (Exception ex)
            {

            }
            finally
            {
            }
            return flag;
        }
        [WebMethod]
        public static string GetCurrentConvertedRate(string CurrencyId)
        {

            string[] ActCurrency = new string[] { };

            string CompID = "";
            if (HttpContext.Current.Session["LastCompany"] != null)
            {
                CompID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);


            }
            string currentRate = "";
            if (HttpContext.Current.Session["ActiveCurrency"] != null)
            {
                string currency = Convert.ToString(HttpContext.Current.Session["ActiveCurrency"]);
                ActCurrency = currency.Split('~');
                int BaseCurrencyId = Convert.ToInt32(ActCurrency[0]);
                int ConvertedCurrencyId = Convert.ToInt32(CurrencyId);
                SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
                DataTable dt = objSlaesActivitiesBL.GetCurrentConvertedRate(BaseCurrencyId, ConvertedCurrencyId, CompID);
                if (dt != null && dt.Rows.Count > 0)
                {
                    currentRate = Convert.ToString(dt.Rows[0]["SalesRate"]);
                    return currentRate;
                }
            }
            return null;

        }
        protected void cmbContactPerson_Callback(object sender, CallbackEventArgsBase e)
        {
            Session["SI_QuotationAddressDtl"] = null;
            Session["SI_BillingAddressLookup"] = null;
            Session["SI_ShippingAddressLookup"] = null;
            string WhichCall = e.Parameter.Split('~')[0];
            if (WhichCall == "BindContactPerson")
            {
                string InternalId = Convert.ToString(Convert.ToString(e.Parameter.Split('~')[1]));


                DataTable dtDeuDate = objSalesInvoiceBL.GetCustomerDetails_InvoiceRelated(InternalId);
                foreach (DataRow dr in dtDeuDate.Rows)
                {
                    string strDueDate = Convert.ToString(dr["DueDate"]);
                    cmbContactPerson.JSProperties["cpDueDate"] = strDueDate;
                }

                DataTable dtTotalDues = objSalesInvoiceBL.GetCustomerTotalDues(InternalId);
                if (dtTotalDues != null && dtTotalDues.Rows.Count > 0)
                {
                    string totalDues = Convert.ToString(dtTotalDues.Rows[0]["NetOutstanding"]);
                    cmbContactPerson.JSProperties["cpTotalDue"] = totalDues;
                }
                else
                {
                    cmbContactPerson.JSProperties["cpTotalDue"] = "0.00";
                }


            }
        }

        protected void ddl_VatGstCst_Callback(object sender, CallbackEventArgsBase e)
        {
            string type = e.Parameter.Split('~')[0];
            PopulateGSTCSTVAT(type);
        }
        protected void PopulateGSTCSTVAT(string type)
        {
            DataTable dtGSTCSTVAT = new DataTable();
            SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
            if (type == "2")
            {
                dtGSTCSTVAT = objSlaesActivitiesBL.PopulateGSTCSTVAT(dt_PLQuote.Date.ToString("yyyy-MM-dd"));

                #region Delete Igst,Cgst,Sgst respectively

                string CompInternalId = Convert.ToString(Session["LastCompany"]);
                string[] compGstin = oDBEngine.GetFieldValue1("tbl_master_company", "cmp_gstin", "cmp_internalid='" + CompInternalId + "'", 1);

                string ShippingState = "";

                if (ddl_PosGst.Value != null)
                {
                    if (ddl_PosGst.Value.ToString() == "S")
                    {
                        ShippingState = lblShippingStateText.Value;
                        ShippingState = ShippingState.Substring(ShippingState.IndexOf("(State Code:")).Replace("(State Code:", "").Replace(")", "");
                    }
                    else
                    {
                        ShippingState = lblBillingStateText.Value;
                        ShippingState = ShippingState.Substring(ShippingState.IndexOf("(State Code:")).Replace("(State Code:", "").Replace(")", "");
                    }
                }


                if (ShippingState.Trim() != "" && compGstin[0].Trim() != "")
                {

                    if (compGstin.Length > 0)
                    {
                        if (compGstin[0].Substring(0, 2) == ShippingState)
                        {
                            //Check if the state is in union territories then only UTGST will apply
                            //   Chandigarh     Andaman and Nicobar Islands    DADRA & NAGAR HAVELI    DAMAN & DIU             DELHI                  Lakshadweep              PONDICHERRY
                            if (ShippingState == "4" || ShippingState == "26" || ShippingState == "25" || ShippingState == "7" || ShippingState == "31" || ShippingState == "34")
                            {
                                foreach (DataRow dr in dtGSTCSTVAT.Rows)
                                {
                                    if (Convert.ToString(dr["TaxRates_GSTtype"]).Trim() == "I" || Convert.ToString(dr["TaxRates_GSTtype"]).Trim() == "C" || Convert.ToString(dr["TaxRates_GSTtype"]).Trim() == "S")
                                    {
                                        dr.Delete();
                                    }
                                }

                            }
                            else
                            {
                                foreach (DataRow dr in dtGSTCSTVAT.Rows)
                                {
                                    if (Convert.ToString(dr["TaxRates_GSTtype"]).Trim() == "I" || Convert.ToString(dr["TaxRates_GSTtype"]).Trim() == "U")
                                    {
                                        dr.Delete();
                                    }
                                }
                            }
                            dtGSTCSTVAT.AcceptChanges();
                        }
                        else
                        {
                            foreach (DataRow dr in dtGSTCSTVAT.Rows)
                            {
                                if (Convert.ToString(dr["TaxRates_GSTtype"]).Trim() == "C" || Convert.ToString(dr["TaxRates_GSTtype"]).Trim() == "S" || Convert.ToString(dr["TaxRates_GSTtype"]).Trim() == "U")
                                {
                                    dr.Delete();
                                }
                            }
                            dtGSTCSTVAT.AcceptChanges();

                        }

                    }
                }

                //If Company GSTIN is blank then Delete All CGST,UGST,IGST,CGST
                if (compGstin[0].Trim() == "")
                {
                    foreach (DataRow dr in dtGSTCSTVAT.Rows)
                    {
                        if (Convert.ToString(dr["TaxRates_GSTtype"]).Trim() == "C" || Convert.ToString(dr["TaxRates_GSTtype"]).Trim() == "S" || Convert.ToString(dr["TaxRates_GSTtype"]).Trim() == "U" || Convert.ToString(dr["TaxRates_GSTtype"]).Trim() == "I")
                        {
                            dr.Delete();
                        }
                    }
                    dtGSTCSTVAT.AcceptChanges();
                }


                #endregion



                if (dtGSTCSTVAT != null && dtGSTCSTVAT.Rows.Count > 0)
                {
                    ddl_VatGstCst.TextField = "Taxes_Name";
                    ddl_VatGstCst.ValueField = "Taxes_ID";
                    ddl_VatGstCst.DataSource = dtGSTCSTVAT;
                    ddl_VatGstCst.DataBind();
                    ddl_VatGstCst.SelectedIndex = 0;
                }
            }
            else
            {
                ddl_VatGstCst.DataSource = null;
                ddl_VatGstCst.DataBind();
            }
        }

        #region PrePopulated Data If Page is not Post Back Section Start
        public void SetFinYearCurrentDate()
        {
            dt_PLQuote.EditFormatString = objConverter.GetDateFormat("Date");
            string fDate = null;

            string[] FinYEnd = Convert.ToString(Session["FinYearEnd"]).Split(' ');
            string FinYearEnd = FinYEnd[0];

            DateTime date3 = DateTime.ParseExact(FinYearEnd, "M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);

            ForJournalDate = Convert.ToString(date3);



            if (date3 < oDBEngine.GetDate().Date)
            {
                fDate = Convert.ToString(Convert.ToDateTime(ForJournalDate).Month) + "/" + Convert.ToString(Convert.ToDateTime(ForJournalDate).Day) + "/" + Convert.ToString(Convert.ToDateTime(ForJournalDate).Year);
            }
            else
            {
                fDate = Convert.ToString(Convert.ToDateTime(oDBEngine.GetDate().Date).Month) + "/" + Convert.ToString(Convert.ToDateTime(oDBEngine.GetDate().Date).Day) + "/" + Convert.ToString(Convert.ToDateTime(oDBEngine.GetDate().Date).Year);
            }

            dt_PLQuote.Value = DateTime.ParseExact(fDate, @"M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        }
        public void GetFinacialYearBasedQouteDate()
        {
            String finyear = "";
            string setdate = null;
            if (Session["LastFinYear"] != null)
            {
                finyear = Convert.ToString(Session["LastFinYear"]).Trim();
                DataTable dtFinYear = objSlaesActivitiesBL.GetFinacialYearBasedQouteDate(finyear);
                if (dtFinYear != null && dtFinYear.Rows.Count > 0)
                {
                    Session["FinYearStartDate"] = Convert.ToString(dtFinYear.Rows[0]["finYearStartDate"]);
                    Session["FinYearEndDate"] = Convert.ToString(dtFinYear.Rows[0]["finYearEndDate"]);
                    if (Session["FinYearStartDate"] != null)
                    {
                        dt_PLQuote.MinDate = Convert.ToDateTime(Convert.ToString(Session["FinYearStartDate"]));
                    }
                    if (Session["FinYearEndDate"] != null)
                    {
                        dt_PLQuote.MaxDate = Convert.ToDateTime(Convert.ToString(Session["FinYearEndDate"]));
                    }
                    if (oDBEngine.GetDate().Date >= Convert.ToDateTime(Convert.ToString(Session["FinYearStartDate"])) && oDBEngine.GetDate().Date <= Convert.ToDateTime(Convert.ToString(Session["FinYearEndDate"])))
                    {

                    }
                    else if (oDBEngine.GetDate().Date >= Convert.ToDateTime(Convert.ToString(Session["FinYearStartDate"])) && oDBEngine.GetDate().Date >= Convert.ToDateTime(Convert.ToString(Session["FinYearEndDate"])))
                    {
                        setdate = Convert.ToString(Convert.ToDateTime(Session["FinYearEndDate"]).Month) + "/" + Convert.ToString(Convert.ToDateTime(Session["FinYearEndDate"]).Day) + "/" + Convert.ToString(Convert.ToDateTime(Session["FinYearEndDate"]).Year);
                        dt_PLQuote.Value = DateTime.ParseExact(setdate, @"M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        //dt_PLQuote.Value = DateTime.ParseExact(Convert.ToString(Session["FinYearStartDate"]), @"M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    }
                    else if (oDBEngine.GetDate().Date <= Convert.ToDateTime(Convert.ToString(Session["FinYearStartDate"])) && oDBEngine.GetDate().Date <= Convert.ToDateTime(Convert.ToString(Session["FinYearEndDate"])))
                    {
                        setdate = Convert.ToString(Convert.ToDateTime(Session["FinYearStartDate"]).Month) + "/" + Convert.ToString(Convert.ToDateTime(Session["FinYearStartDate"]).Day) + "/" + Convert.ToString(Convert.ToDateTime(Session["FinYearStartDate"]).Year);
                        dt_PLQuote.Value = DateTime.ParseExact(setdate, @"M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                        //dt_PLQuote.Value = DateTime.ParseExact(Convert.ToString(Session["FinYearEndDate"]), @"M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    }
                }
            }
            //dt_PLQuote.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
        }

        protected void cmbExecName_Callback(object sender, CallbackEventArgsBase e)
        {
            string financerInternalId = e.Parameter;
            // populateExecutive(financerInternalId);
            cmbExecName.JSProperties["cpFinancerHasLedger"] = populateExecutive(financerInternalId); //IsFinancerHasLedger(financerInternalId);
        }

        public string IsFinancerHasLedger(string financerId)
        {
            string IsLedgerPresent = "0";
            DataTable execData = PosData.IsLedgerExistsForFinancer(financerId);
            if (execData != null)
            {
                if (Convert.ToString(execData.Rows[0][0]) != "")
                {
                    IsLedgerPresent = "1";
                }
            }
            return IsLedgerPresent;
        }
        public string populateExecutive(string financerId)
        {
            DataSet execData = PosData.GetExecutive(financerId);
            cmbExecName.DataSource = execData.Tables[0];
            cmbExecName.TextField = "ExecName";
            cmbExecName.ValueField = "cnt_internalId";
            cmbExecName.DataBind();

            string IsLedgerPresent = "0";
            if (execData != null)
            {
                if (execData.Tables[1].Rows.Count > 0 && Convert.ToString(execData.Tables[1].Rows[0][0]) != "")
                {
                    IsLedgerPresent = "1";
                }
            }
            return IsLedgerPresent;
        }

        public void GetAllDropDownDetailForSalesQuotation(string userbranch)
        {
            #region Schema Drop Down Start
            DataSet dst = new DataSet();
            string strCompanyID = Convert.ToString(Session["LastCompany"]);
            string strBranchID = Convert.ToString(Session["userbranchID"]);
            string FinYear = Convert.ToString(Session["LastFinYear"]);
            string userbranchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);

            dst = PosData.GetAllDropDownDetailForSalesInvoice(userbranch, strCompanyID, strBranchID);

            SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
            DataTable Schemadt = objSlaesActivitiesBL.GetNumberingSchema(strCompanyID, userbranchHierarchy, FinYear, "12", "Y");
            if (Schemadt != null && Schemadt.Rows.Count > 0)
            {
                ddl_numberingScheme.DataTextField = "SchemaName";
                ddl_numberingScheme.DataValueField = "Id";
                ddl_numberingScheme.DataSource = Schemadt;
                ddl_numberingScheme.DataBind();
            }
            #endregion Schema Drop Down Start

            #region Branch Drop Down Start
            if (dst.Tables[1] != null && dst.Tables[1].Rows.Count > 0)
            {
                ddl_Branch.DataTextField = "branch_description";
                ddl_Branch.DataValueField = "branch_id";
                ddl_Branch.DataSource = dst.Tables[1];
                ddl_Branch.DataBind();
                //ddl_Branch.Items.Insert(0, new ListItem("Select", "0"));
            }
            if (Session["userbranchID"] != null)
            {
                if (ddl_Branch.Items.Count > 0)
                {
                    int branchindex = 0;
                    int cnt = 0;
                    foreach (ListItem li in ddl_Branch.Items)
                    {
                        if (li.Value == Convert.ToString(Session["userbranchID"]))
                        {
                            cnt = 1;
                            break;
                        }
                        else
                        {
                            branchindex += 1;
                        }
                    }
                    if (cnt == 1)
                    {
                        ddl_Branch.SelectedIndex = branchindex;
                    }
                    else
                    {
                        ddl_Branch.SelectedIndex = cnt;
                    }
                }
            }

            #endregion Branch Drop Down End

            #region Delivered from Drop Down Start
            if (dst.Tables[1] != null && dst.Tables[1].Rows.Count > 0)
            {
                ddDeliveredFrom.DataTextField = "branch_description";
                ddDeliveredFrom.DataValueField = "branch_id";
                ddDeliveredFrom.DataSource = dst.Tables[1];
                ddDeliveredFrom.DataBind();
            }
            if (Session["userbranchID"] != null)
            {
                if (ddDeliveredFrom.Items.Count > 0)
                {
                    int branchindex = 0;
                    int cnt = 0;
                    foreach (ListItem li in ddDeliveredFrom.Items)
                    {
                        if (li.Value == Convert.ToString(Session["userbranchID"]))
                        {
                            cnt = 1;
                            break;
                        }
                        else
                        {
                            branchindex += 1;
                        }
                    }
                    if (cnt == 1)
                    {
                        ddDeliveredFrom.SelectedIndex = branchindex;
                    }
                    else
                    {
                        ddDeliveredFrom.SelectedIndex = cnt;
                    }
                }
            }

            #endregion Branch Drop Down End

            #region Saleman DropDown Start
            //if (dst.Tables[2] != null && dst.Tables[2].Rows.Count > 0)
            //{
            //    ddl_SalesAgent.TextField = "Name";
            //    ddl_SalesAgent.ValueField= "cnt_internalId";
            //    ddl_SalesAgent.DataSource = dst.Tables[2];
            //    ddl_SalesAgent.DataBind();
            //}
            //ddl_SalesAgent.Items.Insert(0, new ListItem("Select", "0"));
            #endregion Saleman DropDown End

            #region Currency Drop Down Start

            if (dst.Tables[3] != null && dst.Tables[3].Rows.Count > 0)
            {
                ddl_Currency.DataTextField = "Currency_Name";
                ddl_Currency.DataValueField = "Currency_ID";
                ddl_Currency.DataSource = dst.Tables[3];
                ddl_Currency.DataBind();
            }
            int currencyindex = 1;
            int no = 0;
            if (Session["ActiveCurrency"] != null)
            {
                if (ddl_Currency.Items.Count > 0)
                {
                    string[] ActCurrency = new string[] { };
                    string currency = Convert.ToString(HttpContext.Current.Session["ActiveCurrency"]);
                    ActCurrency = currency.Split('~');
                    foreach (ListItem li in ddl_Currency.Items)
                    {
                        if (li.Value == Convert.ToString(ActCurrency[0]))
                        {
                            //ddl_Currency.Items.Remove(li);
                            no = 1;
                            break;
                        }
                        else
                        {
                            currencyindex += 1;
                        }
                    }
                }
                ddl_Currency.Items.Insert(0, new ListItem("Select", "0"));
                if (no == 1)
                {
                    ddl_Currency.SelectedIndex = currencyindex;
                }
                else
                {
                    ddl_Currency.SelectedIndex = no;
                }
            }

            #endregion Currency Drop Down End

            #region TaxGroupType DropDown Start
            if (dst.Tables[4] != null && dst.Tables[4].Rows.Count > 0)
            {
                ddl_AmountAre.TextField = "taxGrp_Description";
                ddl_AmountAre.ValueField = "taxGrp_Id";
                ddl_AmountAre.DataSource = dst.Tables[4];
                ddl_AmountAre.DataBind();
                ddl_AmountAre.Value = "1";
            }
            #endregion TaxGroupType DropDown Start




            #region Financer DropDown Start
            //if (dst.Tables[5] != null && dst.Tables[5].Rows.Count > 0)
            //{
            //    cmbFinancer.TextField = "cnt_firstName";
            //    cmbFinancer.ValueField = "cnt_internalId";
            //    cmbFinancer.DataSource = dst.Tables[7];
            //    cmbFinancer.DataBind();
            //}
            #endregion Financer DropDown Start

            #region  productDD
            if (Session["ProductDetailsListPOS"] == null)
                Session["ProductDetailsListPOS"] = dst.Tables[7];

            #endregion Product


            #region  productDD
            if (Session["CustomerDetailsListPOS"] == null)
                Session["CustomerDetailsListPOS"] = dst.Tables[8];

            #endregion Product

        }

        #endregion PrePopulated Data If Page is not Post Back Section End

        #region PrePopulated Data in Page Load Due to use Searching Functionality Section Start

        #endregion PrePopulated Data in Page Load Due to use Searching Functionality Section End

        #endregion



        #region Component Tagging

        protected void ComponentQuotation_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string status = string.Empty;
            string Customer = string.Empty;
            string OrderDate = string.Empty;
            string ComponentType = string.Empty;
            string Action = string.Empty;
            string BranchID = string.Empty;
            string FinYear = Convert.ToString(Session["LastFinYear"]);

            if (e.Parameter.Split('~')[0] == "BindComponentGrid")
            {
                BranchID = Convert.ToString(ddl_Branch.SelectedValue);
                if (e.Parameter.Split('~')[1] != null) Customer = e.Parameter.Split('~')[1];
                if (e.Parameter.Split('~')[2] != null) OrderDate = e.Parameter.Split('~')[2];
                if (e.Parameter.Split('~')[4] != null) ComponentType = e.Parameter.Split('~')[4];

                if (ComponentType == "QO")
                {
                    Action = "GetQuotation";
                    lbl_InvoiceNO.Text = "PI/Quotation Date";
                }
                else if (ComponentType == "SO")
                {
                    Action = "GetOrder";
                    lbl_InvoiceNO.Text = "Sales Order Date";
                }
                else if (ComponentType == "SC")
                {
                    Action = "GetChallan";
                    lbl_InvoiceNO.Text = "Sales Challan Date";
                }

                if (e.Parameter.Split('~')[3] == "DateCheck")
                {
                    lookup_quotation.GridView.Selection.UnselectAll();
                }

                string strInvoiceID = InvoiceId;// Convert.ToString(Session["SI_InvoiceID"]);
                DataTable ComponentTable = PosData.GetComponent(Customer, OrderDate, ComponentType, FinYear, BranchID, Action, strInvoiceID, Convert.ToString(Session["userbranchHierarchy"]));
                lookup_quotation.GridView.Selection.CancelSelection();
                lookup_quotation.DataSource = ComponentTable;
                lookup_quotation.DataBind();

                Session["SI_ComponentData" + Convert.ToString(uniqueId.Value)] = ComponentTable;
                lookup_quotation.Focus();
            }
            else if (e.Parameter.Split('~')[0] == "BindComponentGridOnSelection")//Subhabrata for binding quotation
            {
                if (grid_Products.GetSelectedFieldValues("ComponentDetailsID").Count != 0)
                {
                    for (int i = 0; i < grid_Products.GetSelectedFieldValues("ComponentDetailsID").Count; i++)
                    {
                        QuotationIds += "," + grid_Products.GetSelectedFieldValues("ComponentID")[i];
                    }
                    QuotationIds = QuotationIds.TrimStart(',');
                    lookup_quotation.GridView.Selection.UnselectAll();
                    if (!String.IsNullOrEmpty(QuotationIds))
                    {
                        string[] eachQuo = QuotationIds.Split(',');
                        if (eachQuo.Length > 1)//More tha one quotation
                        {
                            txt_InvoiceDate.Text = "Multiple Select Quotation Dates";

                            foreach (string val in eachQuo)
                            {
                                lookup_quotation.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));
                            }
                        }
                        else if (eachQuo.Length == 1)//Single Quotation
                        {
                            foreach (string val in eachQuo)
                            {
                                lookup_quotation.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));
                            }
                        }
                        else//No Quotation selected
                        {
                            lookup_quotation.GridView.Selection.UnselectAll();
                        }
                    }
                }
                else if (grid_Products.GetSelectedFieldValues("ComponentDetailsID").Count == 0)
                {
                    lookup_quotation.GridView.Selection.UnselectAll();
                }

                string strType = Convert.ToString(rdl_SaleInvoice.SelectedValue);
                DataTable dt = objSalesInvoiceBL.GetNecessaryData(QuotationIds, strType);
                if (dt != null && dt.Rows.Count > 0)
                {
                    string Reference = Convert.ToString(dt.Rows[0]["Reference"]);
                    string Currency_Id = Convert.ToString(dt.Rows[0]["Currency_Id"]);
                    string SalesmanId = Convert.ToString(dt.Rows[0]["SalesmanId"]);
                    string ExpiryDate = Convert.ToString(dt.Rows[0]["ExpiryDate"]);
                    string CurrencyRate = Convert.ToString(dt.Rows[0]["CurrencyRate"]);

                    ComponentQuotationPanel.JSProperties["cpDetails"] = Reference + "~" + Currency_Id + "~" + SalesmanId + "~" + ExpiryDate + "~" + CurrencyRate;
                }
            }
            else if (e.Parameter.Split('~')[0] == "DateCheckOnChanged")//Subhabrata for binding quotation
            {
                if (grid_Products.GetSelectedFieldValues("Quotation_No").Count != 0)
                {
                    DateTime SalesOrderDate = Convert.ToDateTime(e.Parameter.Split('~')[2]);
                    if (lookup_quotation.GridView.GetSelectedFieldValues("Date").Count() != 0)
                    {
                        DateTime QuotationDate = Convert.ToDateTime(lookup_quotation.GridView.GetSelectedFieldValues("Date")[0]);
                        if (SalesOrderDate < QuotationDate)
                        {
                            lookup_quotation.GridView.Selection.UnselectAll();
                        }
                    }
                }
            }
        }
        protected void lookup_quotation_DataBinding(object sender, EventArgs e)
        {
            if (Session["SI_ComponentData" + Convert.ToString(uniqueId.Value)] != null)
            {
                lookup_quotation.DataSource = (DataTable)Session["SI_ComponentData" + Convert.ToString(uniqueId.Value)];
            }
        }
        protected void ComponentDatePanel_Callback(object sender, CallbackEventArgsBase e)
        {
            string strSplitCommand = e.Parameter.Split('~')[0];
            if (strSplitCommand == "BindComponentDate")
            {
                string Quote_No = Convert.ToString(e.Parameter.Split('~')[1]);

                DataTable dt_QuotationDetails = objSlaesActivitiesBL.GetQuotationDate(Quote_No);
                if (dt_QuotationDetails != null && dt_QuotationDetails.Rows.Count > 0)
                {
                    string quotationdate = Convert.ToString(dt_QuotationDetails.Rows[0]["Quote_Date"]);
                    if (!string.IsNullOrEmpty(quotationdate))
                    {
                        txt_InvoiceDate.Text = Convert.ToString(quotationdate);
                    }
                }
            }
        }
        protected void cgridProducts_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string strSplitCommand = e.Parameters.Split('~')[0];
            if (strSplitCommand == "BindProductsDetails")
            {
                string Quote_Nos = Convert.ToString(e.Parameters.Split('~')[1]);

                String QuoComponent = "";
                List<object> QuoList = lookup_quotation.GridView.GetSelectedFieldValues("ComponentID");
                foreach (object Quo in QuoList)
                {
                    QuoComponent += "," + Quo;
                }
                QuoComponent = QuoComponent.TrimStart(',');

                if (Quote_Nos != "$")
                {
                    string strAction = "";
                    string strType = Convert.ToString(rdl_SaleInvoice.SelectedValue);

                    if (strType == "QO")
                    {
                        strAction = "GetQuotationProducts";
                        grid_Products.Columns["ComponentNumber"].Caption = "Quotation No";
                    }
                    else if (strType == "SO")
                    {
                        strAction = "GetOrderProducts";
                        grid_Products.Columns["ComponentNumber"].Caption = "Order No";
                    }
                    else if (strType == "SC")
                    {
                        strAction = "GetChallanProducts";
                        grid_Products.Columns["ComponentNumber"].Caption = "Challan No";
                    }

                    string strInvoiceID = InvoiceId;// Convert.ToString(Session["SI_InvoiceID"]);
                    DataTable dtDetails = objSalesInvoiceBL.GetComponentProductList(strAction, QuoComponent, strInvoiceID);

                    grid_Products.DataSource = dtDetails;
                    grid_Products.DataBind();
                }
                else
                {
                    grid_Products.DataSource = null;
                    grid_Products.DataBind();
                }

            }
            if (strSplitCommand == "SelectAndDeSelectProducts")
            {
                ASPxGridView gv = sender as ASPxGridView;
                string command = e.Parameters.ToString();
                string State = Convert.ToString(e.Parameters.Split('~')[1]);
                if (State == "SelectAll")
                {
                    for (int i = 0; i < gv.VisibleRowCount; i++)
                    {
                        gv.Selection.SelectRow(i);
                    }
                }
                if (State == "UnSelectAll")
                {
                    for (int i = 0; i < gv.VisibleRowCount; i++)
                    {
                        gv.Selection.UnselectRow(i);
                    }
                }
                if (State == "Revart")
                {
                    for (int i = 0; i < gv.VisibleRowCount; i++)
                    {
                        if (gv.Selection.IsRowSelected(i))
                            gv.Selection.UnselectRow(i);
                        else
                            gv.Selection.SelectRow(i);
                    }
                }
            }
        }

        #endregion


        #region oldUnitValue
        private void createOldUnittable()
        {
            DataTable oldUnitTable = new DataTable();
            oldUnitTable.Columns.Add("oldUnit_id", typeof(System.Guid));
            oldUnitTable.Columns.Add("Product_id", typeof(System.Int32));
            oldUnitTable.Columns.Add("Product_Des", typeof(System.String));
            oldUnitTable.Columns.Add("oldUnit_Uom", typeof(System.String));
            oldUnitTable.Columns.Add("oldUnit_qty", typeof(System.Decimal));
            oldUnitTable.Columns.Add("oldUnit_value", typeof(System.Decimal));

            Session["PosOldUnittable" + Convert.ToString(uniqueId.Value)] = oldUnitTable;
        }
        protected void OldUnitGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string requestedString = e.Parameters.Split('~')[0];
            OldUnitGrid.JSProperties["cpReturnString"] = e.Parameters;
            if (requestedString == "DisplayOldUnit")
            {
                DataTable oldUnitTable = (DataTable)Session["PosOldUnittable" + Convert.ToString(uniqueId.Value)];
                if (oldUnitTable != null)
                {
                    OldUnitGrid.DataSource = oldUnitTable;
                    OldUnitGrid.DataBind();
                }
            }
            else if (requestedString == "AddDataToTable")
            {
                DataTable oldUnitTable = (DataTable)Session["PosOldUnittable" + Convert.ToString(uniqueId.Value)];
                if (oldUnitTable == null)
                {
                    createOldUnittable();
                }
                oldUnitTable = (DataTable)Session["PosOldUnittable" + Convert.ToString(uniqueId.Value)];
                DataRow oldUnitRow = oldUnitTable.NewRow();
                oldUnitRow["oldUnit_id"] = Guid.NewGuid();
                string productId = Convert.ToString(oldUnitProductLookUp.Value == null ? 0 : oldUnitProductLookUp.Value);
                oldUnitRow["Product_id"] = productId.Split(new string[] { "|@|" }, StringSplitOptions.None)[0];
                oldUnitRow["Product_Des"] = oldUnitProductLookUp.Text;
                oldUnitRow["oldUnit_Uom"] = txtOldUnitUom.Text;
                oldUnitRow["oldUnit_qty"] = Convert.ToDecimal(txtOldUnitqty.Text);
                oldUnitRow["oldUnit_value"] = Convert.ToDecimal(txtoldUnitValue.Text);
                oldUnitTable.Rows.Add(oldUnitRow);
                Session["PosOldUnittable" + Convert.ToString(uniqueId.Value)] = oldUnitTable;
                bindOldUnitGrid();
            }
            else if (requestedString == "DeleteFromTable")
            {
                string id = e.Parameters.Split('~')[1];
                DataTable oldUnitTable = (DataTable)Session["PosOldUnittable" + Convert.ToString(uniqueId.Value)];
                if (oldUnitTable != null)
                {
                    DataRow[] existingRow = oldUnitTable.Select("oldUnit_id='" + id + "'");
                    if (existingRow.Length > 0)
                    {
                        foreach (DataRow dr in existingRow)
                        {
                            oldUnitTable.Rows.Remove(dr);
                        }

                    }
                    Session["PosOldUnittable" + Convert.ToString(uniqueId.Value)] = oldUnitTable;
                    bindOldUnitGrid();
                }

            }
            else if (requestedString == "DeleteAllRecord")
            {
                createOldUnittable();
            }

            OldUnitGrid.JSProperties["cpTotalOldUnit"] = GetTotalOldUnitValue();

        }

        public string GetTotalOldUnitValue()
        {
            string ReturnValue = "0";
            DataTable oldUnitTable = (DataTable)Session["PosOldUnittable" + Convert.ToString(uniqueId.Value)];
            if (oldUnitTable != null)
            {
                ReturnValue = Convert.ToString(oldUnitTable.Compute("SUM(oldUnit_value)", string.Empty));
            }
            if (ReturnValue.Trim() == "")
            {
                ReturnValue = "0";
            }
            return ReturnValue;
        }

        private void bindOldUnitGrid()
        {
            DataTable oldUnitTable = (DataTable)Session["PosOldUnittable" + Convert.ToString(uniqueId.Value)];
            if (oldUnitTable != null)
            {
                OldUnitGrid.DataSource = oldUnitTable;
                OldUnitGrid.DataBind();
            }
        }

        protected void oldUnitUpdatePanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string updatedId = e.Parameter;
            DataTable oldUnitTable = (DataTable)Session["PosOldUnittable" + Convert.ToString(uniqueId.Value)];
            DataRow[] existingRow = oldUnitTable.Select("oldUnit_id='" + updatedId + "'");
            if (existingRow.Length > 0)
            {
                oldUnitProductLookUp.GridView.Selection.SelectRowByKey(Convert.ToString(existingRow[0]["Product_id"]) + "|@|" + Convert.ToString(existingRow[0]["oldUnit_Uom"]));
                txtOldUnitUom.Text = Convert.ToString(existingRow[0]["oldUnit_Uom"]);
                txtOldUnitqty.Text = Convert.ToString(existingRow[0]["oldUnit_qty"]);
                txtoldUnitValue.Text = Convert.ToString(existingRow[0]["oldUnit_value"]);
            }
        }

        private void SetOldUnitDetails(string invid)
        {
            DataTable QuotationEditdt = PosData.GetOldUnitDetails(invid);
            Session["PosOldUnittable" + Convert.ToString(uniqueId.Value)] = QuotationEditdt;
        }

        #endregion

        #region Customer Receipt
        protected void aspxCustomerReceiptGridview_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string receivedString = e.Parameters;

            if (receivedString.Split('~')[0] == "BindCustomerGridByInternalId")
            {
                string customerInternalId = receivedString.Split('~')[1];
                string posInvoiceDtae = receivedString.Split('~')[2];

                string hsnList = Convert.ToString(hdHsnList.Value).TrimStart(',');

                DataTable customerReceiptDetails = PosData.GetCustomerReceiptDetails(customerInternalId, Convert.ToString(Session["LastFinYear"]), Convert.ToString(Session["LastCompany"]), posInvoiceDtae, Convert.ToString(ddl_Branch.SelectedValue), hsnList);
                if (customerReceiptDetails != null)
                {
                    Session["PosCustomerReceiptDetails" + Convert.ToString(uniqueId.Value)] = customerReceiptDetails;
                    aspxCustomerReceiptGridview.DataBind();
                }

                DataTable totalAmount = PosData.GetCustomerTotalAmountOnSingleDay(customerInternalId, posInvoiceDtae, Convert.ToInt32(ddl_Branch.SelectedValue));
                if (totalAmount != null && totalAmount.Rows.Count > 0)
                    aspxCustomerReceiptGridview.JSProperties["cpTotalTransectionAmount"] = Convert.ToString(totalAmount.Rows[0][0]);
                else
                    aspxCustomerReceiptGridview.JSProperties["cpTotalTransectionAmount"] = "0";

            }
            else if (receivedString.Split('~')[0] == "SelectAllRecords")
            {
                for (int i = 0; i < aspxCustomerReceiptGridview.VisibleRowCount; i++)
                {
                    aspxCustomerReceiptGridview.Selection.SelectRow(i);
                }
            }
            else if (receivedString.Split('~')[0] == "UnSelectAllRecords")
            {
                for (int i = 0; i < aspxCustomerReceiptGridview.VisibleRowCount; i++)
                {
                    aspxCustomerReceiptGridview.Selection.UnselectRow(i);
                }
            }
            else if (receivedString.Split('~')[0] == "Revert")
            {
                for (int i = 0; i < aspxCustomerReceiptGridview.VisibleRowCount; i++)
                {
                    if (aspxCustomerReceiptGridview.Selection.IsRowSelected(i))
                        aspxCustomerReceiptGridview.Selection.UnselectRow(i);
                    else
                        aspxCustomerReceiptGridview.Selection.SelectRow(i);
                }
            }
            else if (receivedString.Split('~')[0] == "SaveCustomerReceiptGridview")
            {
                string receipt = "";
                List<object> receiptList = aspxCustomerReceiptGridview.GetSelectedFieldValues("ReceiptPayment_ID");

                foreach (object Robj in receiptList)
                {
                    receipt += "," + Robj;
                }
                receipt = receipt.TrimStart(',');
                hdAddvanceReceiptNo.Value = receipt;
                aspxCustomerReceiptGridview.JSProperties["cpReceiptList"] = receipt;
                DataTable totalAmount = PosData.GetCustomerReceiptTotalAmount(receipt);
                if (totalAmount != null)
                {
                    aspxCustomerReceiptGridview.JSProperties["cpCustomerReceiptTotalAmount"] = Convert.ToString(totalAmount.Rows[0][0]);
                }
            }

        }

        protected void aspxCustomerReceiptGridview_DataBinding(object sender, EventArgs e)
        {
            DataTable customerReceiptDetails = (DataTable)Session["PosCustomerReceiptDetails" + Convert.ToString(uniqueId.Value)];
            if (customerReceiptDetails != null)
            {
                aspxCustomerReceiptGridview.DataSource = customerReceiptDetails;
            }
        }
        #endregion

        protected void challanNoScheme_Callback(object sender, CallbackEventArgsBase e)
        {
            string type = e.Parameter.Split('~')[0];
            if (type == "BindChallanScheme")
            {
                string branchId = e.Parameter.Split('~')[1];

                DataSet dst = new DataSet();
                string strCompanyID = Convert.ToString(Session["LastCompany"]);
                string FinYear = Convert.ToString(Session["LastFinYear"]);
                dst = objSlaesActivitiesBL.GetAllDropDownDetailForSalesChallan(branchId);
                SlaesActivitiesBL objSlaesActivitiesBL1 = new SlaesActivitiesBL();
                DataTable Schemadt = objSlaesActivitiesBL1.GetNumberingSchema(strCompanyID, branchId, FinYear, "10", "Y");


                if (Schemadt != null && Schemadt.Rows.Count > 0)
                {
                    challanNoScheme.TextField = "SchemaName";
                    challanNoScheme.ValueField = "Id";
                    challanNoScheme.DataSource = Schemadt;
                    challanNoScheme.DataBind();
                }
            }
        }

        protected void Edit_Click(object sender, EventArgs e)
        {
            UpdatePosData();
        }


        private void SetBillingShippingForTab(string CustomerId)
        {
            #region Addresss Lookup Section Start
            DataSet dst = new DataSet();
            POSCRMSalesDtlBL objCRMSalesDtlBL = new POSCRMSalesDtlBL();
            dst = objCRMSalesDtlBL.PopulateBillingandShippingDetailByCustomerID(CustomerId);



            if (dst.Tables[0].Rows.Count > 0)
            {
                Session["SI_BillingAddressLookup"] = dst.Tables[0];
            }

            if (dst.Tables[1].Rows.Count > 0)
            {
                Session["SI_ShippingAddressLookup"] = dst.Tables[1];
            }

            #endregion

            #region Variable Declaration to send value using jsproperties Start
            string add_addressType = "";
            string add_address1 = "";
            string add_address2 = "";
            string add_address3 = "";
            string add_landMark = "";
            string add_country = "";
            string add_state = "";
            string add_city = "";
            string add_pin = "";
            string add_area = "";

            ///// shipping variable

            string add_saddressType = "";
            string add_saddress1 = "";
            string add_saddress2 = "";
            string add_saddress3 = "";
            string add_slandMark = "";
            string add_scountry = "";
            string add_sstate = "";
            string add_scity = "";
            string add_spin = "";
            string add_sarea = "";

            #endregion Variable Declaration to send value using jsproperties Start




            //DataTable dtaddress=(DataTable)
            //string AddressStatus = Convert.ToString(e.Parameter.Split('~')[1]);
            if (Session["SI_QuotationAddressDtl"] == null)
            {
                string customerid = hdnCustomerId.Value;
                #region Billing Detail fillup
                DataTable dtaddbill = oDBEngine.GetDataTable("select add_addressType,add_address1,add_address2,add_address3,add_landMark,add_country,add_state,add_city,add_pin,add_area from tbl_master_address where add_cntId='" + customerid + "' and add_addressType='Billing' and Isdefault='1' ");

                #region Function To get All Detail

                if (dtaddbill.Rows.Count > 0 && dtaddbill != null)
                {

                    add_addressType = Convert.ToString(dtaddbill.Rows[0]["add_addressType"]);
                    add_address1 = Convert.ToString(dtaddbill.Rows[0]["add_address1"]);
                    add_address2 = Convert.ToString(dtaddbill.Rows[0]["add_address2"]);
                    add_address3 = Convert.ToString(dtaddbill.Rows[0]["add_address3"]);
                    add_landMark = Convert.ToString(dtaddbill.Rows[0]["add_landMark"]);
                    add_country = Convert.ToString(dtaddbill.Rows[0]["add_country"]);
                    add_state = Convert.ToString(dtaddbill.Rows[0]["add_state"]);
                    add_city = Convert.ToString(dtaddbill.Rows[0]["add_city"]);
                    add_pin = Convert.ToString(dtaddbill.Rows[0]["add_pin"]);
                    add_area = Convert.ToString(dtaddbill.Rows[0]["add_area"]);

                    populateNewAddressDetailsByPIN(add_address1, add_address2, add_address3, add_landMark, add_country, add_state, add_city, add_pin, add_area);
                    ComponentPanel.JSProperties["cpshow"] = add_addressType + "~"
                                                       + add_address1 + "~"
                                                       + add_address2 + "~"
                                                       + add_address3 + "~"
                                                       + add_landMark + "~"
                                                       + add_country + "~"
                                                       + add_state + "~"
                                                       + add_city + "~"
                                                       + add_pin + "~"
                                                       + add_area + "~"
                                                       + "Y" + "~";

                }
                else
                {
                    ComponentPanel.JSProperties["cpshow"] = add_addressType + "~"
                                                      + add_address1 + "~"
                                                      + add_address2 + "~"
                                                      + add_address3 + "~"
                                                      + add_landMark + "~"
                                                      + add_country + "~"
                                                      + add_state + "~"
                                                      + add_city + "~"
                                                      + add_pin + "~"
                                                      + add_area + "~"
                                                       + "N" + "~";
                }
                #endregion Function Calling End
                #endregion Billing Detail fillup end
                #region Shipping Detail fillup
                DataTable dtaship = oDBEngine.GetDataTable("select add_addressType,add_address1,add_address2,add_address3,add_landMark,add_country,add_state,add_city,add_pin,add_area from tbl_master_address where add_cntId='" + customerid + "' and add_addressType='Shipping' and Isdefault='1' ");
                if (dtaship.Rows.Count > 0 && dtaship != null)
                {
                    add_saddressType = Convert.ToString(dtaship.Rows[0]["add_addressType"]);
                    add_saddress1 = Convert.ToString(dtaship.Rows[0]["add_address1"]);
                    add_saddress2 = Convert.ToString(dtaship.Rows[0]["add_address2"]);
                    add_saddress3 = Convert.ToString(dtaship.Rows[0]["add_address3"]);
                    add_slandMark = Convert.ToString(dtaship.Rows[0]["add_landMark"]);
                    add_scountry = Convert.ToString(dtaship.Rows[0]["add_country"]);
                    add_sstate = Convert.ToString(dtaship.Rows[0]["add_state"]);
                    add_scity = Convert.ToString(dtaship.Rows[0]["add_city"]);
                    add_spin = Convert.ToString(dtaship.Rows[0]["add_pin"]);
                    add_sarea = Convert.ToString(dtaship.Rows[0]["add_area"]);

                    populateNewAddressDetailsShippingByPIN(add_saddress1, add_saddress2, add_saddress3, add_slandMark, add_scountry, add_sstate, add_scity, add_spin, add_sarea);

                    ComponentPanel.JSProperties["cpshowShip"] = add_saddressType + "~"
                                                      + add_saddress1 + "~"
                                                      + add_saddress2 + "~"
                                                      + add_saddress3 + "~"
                                                      + add_slandMark + "~"
                                                      + add_scountry + "~"
                                                      + add_sstate + "~"
                                                      + add_scity + "~"
                                                      + add_spin + "~"
                                                      + add_sarea + "~"
                                                      + "Y" + "~";

                }
                else
                {
                    ComponentPanel.JSProperties["cpshowShip"] = add_saddressType + "~"
                                                      + add_saddress1 + "~"
                                                      + add_saddress2 + "~"
                                                      + add_saddress3 + "~"
                                                      + add_slandMark + "~"
                                                      + add_scountry + "~"
                                                      + add_sstate + "~"
                                                      + add_scity + "~"
                                                      + add_spin + "~"
                                                      + add_sarea + "~"
                                                       + "N" + "~";
                }
                #endregion Shipping detail Fillup
            }
            else if (Session["SI_QuotationAddressDtl"] != null)
            {
                DataTable dt = (DataTable)Session["SI_QuotationAddressDtl"];
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows.Count == 2) // when 2 row  data found in edit mode
                    {
                        #region billing Address Dtl using session
                        add_addressType = Convert.ToString(dt.Rows[0]["QuoteAdd_addressType"]);
                        add_address1 = Convert.ToString(dt.Rows[0]["QuoteAdd_address1"]);
                        add_address2 = Convert.ToString(dt.Rows[0]["QuoteAdd_address2"]);
                        add_address3 = Convert.ToString(dt.Rows[0]["QuoteAdd_address3"]);
                        add_landMark = Convert.ToString(dt.Rows[0]["QuoteAdd_landMark"]);
                        add_country = Convert.ToString(dt.Rows[0]["QuoteAdd_countryId"]);
                        add_state = Convert.ToString(dt.Rows[0]["QuoteAdd_stateId"]);
                        add_city = Convert.ToString(dt.Rows[0]["QuoteAdd_cityId"]);
                        add_pin = Convert.ToString(dt.Rows[0]["QuoteAdd_pin"]);
                        add_area = Convert.ToString(dt.Rows[0]["QuoteAdd_areaId"]);

                        populateNewAddressDetailsByPIN(add_address1, add_address2, add_address3, add_landMark, add_country, add_state, add_city, add_pin, add_area);

                        ComponentPanel.JSProperties["cpshow"] = add_addressType + "~"
                                                              + add_address1 + "~"
                                                              + add_address2 + "~"
                                                              + add_address3 + "~"
                                                              + add_landMark + "~"
                                                              + add_country + "~"
                                                              + add_state + "~"
                                                              + add_city + "~"
                                                              + add_pin + "~"
                                                              + add_area + "~"
                                                              + "Y" + "~";
                        #endregion billing Address Dtl using session
                        #region Shipping Address Dtl using session
                        add_saddressType = Convert.ToString(dt.Rows[1]["QuoteAdd_addressType"]);
                        add_saddress1 = Convert.ToString(dt.Rows[1]["QuoteAdd_address1"]);
                        add_saddress2 = Convert.ToString(dt.Rows[1]["QuoteAdd_address2"]);
                        add_saddress3 = Convert.ToString(dt.Rows[1]["QuoteAdd_address3"]);
                        add_slandMark = Convert.ToString(dt.Rows[1]["QuoteAdd_landMark"]);
                        add_scountry = Convert.ToString(dt.Rows[1]["QuoteAdd_countryId"]);
                        add_sstate = Convert.ToString(dt.Rows[1]["QuoteAdd_stateId"]);
                        add_scity = Convert.ToString(dt.Rows[1]["QuoteAdd_cityId"]);
                        add_spin = Convert.ToString(dt.Rows[1]["QuoteAdd_pin"]);
                        add_sarea = Convert.ToString(dt.Rows[1]["QuoteAdd_areaId"]);

                        populateNewAddressDetailsShippingByPIN(add_saddress1, add_saddress2, add_saddress3, add_slandMark, add_scountry, add_sstate, add_scity, add_spin, add_sarea);

                        ComponentPanel.JSProperties["cpshowShip"] = add_saddressType + "~"
                                                             + add_saddress1 + "~"
                                                             + add_saddress2 + "~"
                                                             + add_saddress3 + "~"
                                                             + add_slandMark + "~"
                                                             + add_scountry + "~"
                                                             + add_sstate + "~"
                                                             + add_scity + "~"
                                                             + add_spin + "~"
                                                             + add_sarea + "~"
                                                             + "Y" + "~";
                        #endregion Shipping Address Dtl using session end

                    }
                    else if (dt.Rows.Count == 1) // when 1 row  data found in edit mode
                    {
                        if (Convert.ToString(dt.Rows[0]["QuoteAdd_addressType"]) == "Billing")
                        {
                            #region billing Address Dtl using session
                            add_addressType = Convert.ToString(dt.Rows[0]["QuoteAdd_addressType"]);
                            add_address1 = Convert.ToString(dt.Rows[0]["QuoteAdd_address1"]);
                            add_address2 = Convert.ToString(dt.Rows[0]["QuoteAdd_address2"]);
                            add_address3 = Convert.ToString(dt.Rows[0]["QuoteAdd_address3"]);
                            add_landMark = Convert.ToString(dt.Rows[0]["QuoteAdd_landMark"]);
                            add_country = Convert.ToString(dt.Rows[0]["QuoteAdd_countryId"]);
                            add_state = Convert.ToString(dt.Rows[0]["QuoteAdd_stateId"]);
                            add_city = Convert.ToString(dt.Rows[0]["QuoteAdd_cityId"]);
                            add_pin = Convert.ToString(dt.Rows[0]["QuoteAdd_pin"]);
                            add_area = Convert.ToString(dt.Rows[0]["QuoteAdd_areaId"]);

                            populateNewAddressDetailsByPIN(add_address1, add_address2, add_address3, add_landMark, add_country, add_state, add_city, add_pin, add_area);

                            ComponentPanel.JSProperties["cpshow"] = add_addressType + "~"
                                                                  + add_address1 + "~"
                                                                  + add_address2 + "~"
                                                                  + add_address3 + "~"
                                                                  + add_landMark + "~"
                                                                  + add_country + "~"
                                                                  + add_state + "~"
                                                                  + add_city + "~"
                                                                  + add_pin + "~"
                                                                  + add_area + "~"
                                                                  + "Y" + "~";

                            ComponentPanel.JSProperties["cpshowShip"] = add_saddressType + "~"
                                                 + add_saddress1 + "~"
                                                 + add_saddress2 + "~"
                                                 + add_saddress3 + "~"
                                                 + add_slandMark + "~"
                                                 + add_scountry + "~"
                                                 + add_sstate + "~"
                                                 + add_scity + "~"
                                                 + add_spin + "~"
                                                 + add_sarea + "~"
                                                  + "N" + "~";

                            #endregion billing Address Dtl using session
                        }
                        if (Convert.ToString(dt.Rows[0]["QuoteAdd_addressType"]) == "Shipping")
                        {
                            #region Shipping Address Dtl using session
                            ComponentPanel.JSProperties["cpshow"] = add_addressType + "~"
                                                 + add_address1 + "~"
                                                 + add_address2 + "~"
                                                 + add_address3 + "~"
                                                 + add_landMark + "~"
                                                 + add_country + "~"
                                                 + add_state + "~"
                                                 + add_city + "~"
                                                 + add_pin + "~"
                                                 + add_area + "~"
                                                  + "N" + "~";

                            add_saddressType = Convert.ToString(dt.Rows[0]["QuoteAdd_addressType"]);
                            add_saddress1 = Convert.ToString(dt.Rows[0]["QuoteAdd_address1"]);
                            add_saddress2 = Convert.ToString(dt.Rows[0]["QuoteAdd_address2"]);
                            add_saddress3 = Convert.ToString(dt.Rows[0]["QuoteAdd_address3"]);
                            add_slandMark = Convert.ToString(dt.Rows[0]["QuoteAdd_landMark"]);
                            add_scountry = Convert.ToString(dt.Rows[0]["QuoteAdd_countryId"]);
                            add_sstate = Convert.ToString(dt.Rows[0]["QuoteAdd_stateId"]);
                            add_scity = Convert.ToString(dt.Rows[0]["QuoteAdd_cityId"]);
                            add_spin = Convert.ToString(dt.Rows[0]["QuoteAdd_pin"]);
                            add_sarea = Convert.ToString(dt.Rows[0]["QuoteAdd_areaId"]);
                            populateNewAddressDetailsShippingByPIN(add_saddress1, add_saddress2, add_saddress3, add_slandMark, add_scountry, add_sstate, add_scity, add_spin, add_sarea);
                            ComponentPanel.JSProperties["cpshowShip"] = add_saddressType + "~"
                                                                 + add_saddress1 + "~"
                                                                 + add_saddress2 + "~"
                                                                 + add_saddress3 + "~"
                                                                 + add_slandMark + "~"
                                                                 + add_scountry + "~"
                                                                 + add_sstate + "~"
                                                                 + add_scity + "~"
                                                                 + add_spin + "~"
                                                                 + add_sarea + "~"
                                                                 + "Y" + "~";
                            #endregion Shipping Address Dtl using session end
                        }
                    }
                    else // when no data found in edit mode
                    {
                        #region billing Address Dtl using session
                        //add_addressType = Convert.ToString(dt.Rows[0]["QuoteAdd_addressType"]);
                        //add_address1 = Convert.ToString(dt.Rows[0]["QuoteAdd_address1"]);
                        //add_address2 = Convert.ToString(dt.Rows[0]["QuoteAdd_address2"]);
                        //add_address3 = Convert.ToString(dt.Rows[0]["QuoteAdd_address3"]);
                        //add_landMark = Convert.ToString(dt.Rows[0]["QuoteAdd_landMark"]);
                        //add_country = Convert.ToString(dt.Rows[0]["QuoteAdd_countryId"]);
                        //add_state = Convert.ToString(dt.Rows[0]["QuoteAdd_stateId"]);
                        //add_city = Convert.ToString(dt.Rows[0]["QuoteAdd_cityId"]);
                        //add_pin = Convert.ToString(dt.Rows[0]["QuoteAdd_pin"]);
                        //add_area = Convert.ToString(dt.Rows[0]["QuoteAdd_areaId"]);
                        ComponentPanel.JSProperties["cpshow"] = add_addressType + "~"
                                                              + add_address1 + "~"
                                                              + add_address2 + "~"
                                                              + add_address3 + "~"
                                                              + add_landMark + "~"
                                                              + add_country + "~"
                                                              + add_state + "~"
                                                              + add_city + "~"
                                                              + add_pin + "~"
                                                              + add_area + "~"
                                                              + "Y" + "~";
                        #endregion billing Address Dtl using session
                        #region Shipping Address Dtl using session
                        //add_saddressType = Convert.ToString(dt.Rows[1]["QuoteAdd_addressType"]);
                        //add_saddress1 = Convert.ToString(dt.Rows[1]["QuoteAdd_address1"]);
                        //add_saddress2 = Convert.ToString(dt.Rows[1]["QuoteAdd_address2"]);
                        //add_saddress3 = Convert.ToString(dt.Rows[1]["QuoteAdd_address3"]);
                        //add_slandMark = Convert.ToString(dt.Rows[1]["QuoteAdd_landMark"]);
                        //add_scountry = Convert.ToString(dt.Rows[1]["QuoteAdd_countryId"]);
                        //add_sstate = Convert.ToString(dt.Rows[1]["QuoteAdd_stateId"]);
                        //add_scity = Convert.ToString(dt.Rows[1]["QuoteAdd_cityId"]);
                        //add_spin = Convert.ToString(dt.Rows[1]["QuoteAdd_pin"]);
                        //add_sarea = Convert.ToString(dt.Rows[1]["QuoteAdd_areaId"]);
                        ComponentPanel.JSProperties["cpshowShip"] = add_saddressType + "~"
                                                             + add_saddress1 + "~"
                                                             + add_saddress2 + "~"
                                                             + add_saddress3 + "~"
                                                             + add_slandMark + "~"
                                                             + add_scountry + "~"
                                                             + add_sstate + "~"
                                                             + add_scity + "~"
                                                             + add_spin + "~"
                                                             + add_sarea + "~"
                                                             + "Y" + "~";

                        #endregion Shipping Address Dtl using session end

                    }
                }
            }



        }



        protected void CustomerCallBackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {

            string action = e.Parameter.Split('~')[0];

            if (action == "SetCustomer")
            {
                string CustomerInternalId = e.Parameter.Split('~')[1];
                string CustomerName = e.Parameter.Split('~')[2];
                //Session["CustomerDetailsListPOS"] = PosData.PopulateCustomer();
                //lookup_Customer.DataBind();

                //lookup_Customer.GridView.Selection.SelectRowByKey(CustomerInternalId);
            }
        }

        protected void ddl_SalesAgent_Callback(object sender, CallbackEventArgsBase e)
        {
            string BranchId = e.Parameter;
            bindSalesmanByBranch(BranchId);
        }

        protected void bindSalesmanByBranch(string branchId)
        {
            DataTable salsemanData = PosData.GetSalesmanByBranch(branchId);
            if (salsemanData != null)
            {
                ddl_SalesAgent.DataSource = salsemanData;
                ddl_SalesAgent.TextField = "Name";
                ddl_SalesAgent.ValueField = "cnt_internalId";
                ddl_SalesAgent.DataBind();
                ddl_SalesAgent.SelectedIndex = 0;
            }
        }


        public bool IsProductNeedInstall(string invoiceId)
        {
            bool returnValue = false;
            ProcedureExecute proc = new ProcedureExecute("prc_PosCRMSalesInvoice_Details");
            proc.AddVarcharPara("@Action", 500, "IsProductNeedInstall");
            proc.AddVarcharPara("@InvoiceID", 500, invoiceId);
            DataTable dt = proc.GetTable();
            if (dt.Rows.Count > 0)
            {
                returnValue = true;
            }

            return returnValue;
        }

        [WebMethod]
        public static string GetCurrentBankBalance(string MainAccountID, string BranchId)
        {
            if (BranchId != "")
            {
                BusinessLogicLayer.Converter oConverter = new BusinessLogicLayer.Converter();
                string MainAccountValID = string.Empty;
                string strColor = string.Empty;
                DataTable DT = new DataTable();
                DBEngine objEngine = new DBEngine();

                DT = objEngine.GetDataTable("Select Sum(AccountsLedger_AmountDr-AccountsLedger_AmountCr) TotalAmount from Trans_AccountsLedger WHERE AccountsLedger_MainAccountID='" + MainAccountID + "' and AccountsLedger_BranchId=" + BranchId);
                if (DT.Rows.Count != 0)
                {

                    if (!String.IsNullOrEmpty(Convert.ToString(DT.Rows[0]["TotalAmount"])))
                    {
                        MainAccountValID = oConverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(DT.Rows[0]["TotalAmount"]));
                        strColor = Convert.ToDecimal(MainAccountValID) > 0 ? "White" : "Red";
                    }


                }

                return MainAccountValID + "~" + strColor;
            }
            else
            {
                return "0.00" + "White";
            }
        }


        public static void SendPosSMSToUser(string InvoiceNo, string CustomerId, string datetime)
        {
            try
            {
                DBEngine odbeng = new DBEngine();
                string Provider = ConfigurationManager.AppSettings["SmsProvider"];
                string userName = ConfigurationManager.AppSettings["sSMSLiveUser"];
                string password = ConfigurationManager.AppSettings["sSMSLivePass"];
                string type = ConfigurationManager.AppSettings["sSMSType"];
                string senderId = ConfigurationManager.AppSettings["sSMSSenderID"];
                string SmsMsg = "";

                DataTable ShouldSendSmsDt = odbeng.GetDataTable("select Variable_Value from Config_SystemSettings where Variable_Name='POS_SendSMS'");
                if (Convert.ToString(ShouldSendSmsDt.Rows[0][0]) == "Yes")
                {


                    DataTable smstemplateTable = odbeng.GetDataTable("select SMSTemplate from tbl_master_SMSTemplate where ModuleName='POS'");
                    if (smstemplateTable.Rows.Count > 0)
                    {
                        SmsMsg = Convert.ToString(smstemplateTable.Rows[0][0]);
                    }


                    DataTable customerNamedt = odbeng.GetDataTable("select isnull(sal_Name,'')+space(1)+cnt_firstName+SPACE(1)+isnull(cnt_lastName,'') Name  from tbl_master_contact cnt left outer join tbl_master_salutation salutation  on cnt.cnt_salutation=salutation.sal_id  where cnt_internalId ='" + CustomerId + "'");
                    string customerName = "Customer,";
                    if (customerNamedt.Rows.Count > 0)
                    {
                        customerName = Convert.ToString(customerNamedt.Rows[0][0]);
                    }
                    string MobileNo = "";
                    DataTable MobilenUMBERdT = odbeng.GetDataTable("select top 1 phf_phoneNumber from  tbl_master_phonefax where phf_type='Mobile' and phf_cntId='" + CustomerId + "'");
                    if (MobilenUMBERdT.Rows.Count > 0)
                    {
                        MobileNo = Convert.ToString(MobilenUMBERdT.Rows[0][0]);
                    }

                    SmsMsg = SmsMsg.Replace("@@invoice_number", InvoiceNo);
                    SmsMsg = SmsMsg.Replace("@@customer_name", customerName);
                    //  string url = "http://5.189.187.82/sendsms/sendsms.php?username=itgeapl&password=DW3xGORV&type=TEXT&sender=GRTEST&mobile=8944835508&message=Testing%20Invoice%20" + InvoiceNo;
                    string url = Provider + "?username=" + userName + "&password=" + password + "&type=" + type + "&sender=" + senderId + "&mobile=" + MobileNo + "&message=" + SmsMsg;
                    if (MobileNo.Trim() != "")
                    {
                        try
                        {
                            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                            odbeng.InsurtFieldValue("tbl_trans_SMSMessage", "TextSent,FromModule,SMSdate,deliveryStatus", "'" + SmsMsg + "','POS','" + datetime + "','" + httpResponse.StatusCode + "'");
                        }
                        catch
                        {

                        }
                    }

                }
            }
            catch { }
        }

        public void bidfinancerByBranch(string branchId)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_PosCRMSalesInvoice_Details");
            proc.AddVarcharPara("@Action", 500, "GetFinancerBranch");
            proc.AddVarcharPara("@BranchID", 100, branchId);
            DataTable FinancerDt = proc.GetTable();

            cmbFinancer.TextField = "cnt_firstName";
            cmbFinancer.ValueField = "cnt_internalId";
            cmbFinancer.DataSource = FinancerDt;
            cmbFinancer.DataBind();
        }


        protected void cmbFinancer_Callback(object sender, CallbackEventArgsBase e)
        {
            string branchId = e.Parameter;
            if (branchId != "")
            {
                bidfinancerByBranch(branchId);
            }
            else
            {
                bidfinancerByBranch("0");
            }
        }

        protected void bindChallanNumeringScheme(string branchId)
        {
            DataSet dst = new DataSet();
            string strCompanyID = Convert.ToString(Session["LastCompany"]);
            string FinYear = Convert.ToString(Session["LastFinYear"]);
            dst = objSlaesActivitiesBL.GetAllDropDownDetailForSalesChallan(branchId);
            SlaesActivitiesBL objSlaesActivitiesBL1 = new SlaesActivitiesBL();
            DataTable Schemadt = objSlaesActivitiesBL1.GetNumberingSchema(strCompanyID, branchId, FinYear, "10", "Y");


            if (Schemadt != null && Schemadt.Rows.Count > 0)
            {
                challanNoScheme.TextField = "SchemaName";
                challanNoScheme.ValueField = "Id";
                challanNoScheme.DataSource = Schemadt;
                challanNoScheme.DataBind();
            }

        }


        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {

            #region Block By Sudip
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            string CustomerId = Convert.ToString(hdnCustomerId.Value);
            string strToDate = Convert.ToString(hddnAsOnDate.Value);
            e.KeyExpression = "SLNO";

            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);

            if (Convert.ToString(hddnOutStandingBlock.Value) == "1")
            {
                var q = from d in dc.PARTYOUTSTANDINGDET_REPORTs
                        where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.SLNO) != "999999999" && Convert.ToString(d.PARTYTYPE) == "C"
                        orderby d.SEQ ascending
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.PARTYOUTSTANDINGDET_REPORTs
                        where Convert.ToString(d.SLNO) == "0"
                        orderby d.SEQ ascending
                        select d;
                e.QueryableSource = q;
            }


            #endregion
            CustomerOutstanding.ExpandAll();

        }
        protected void ShowGridCustOut_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            string CustomerId = Convert.ToString(hdnCustomerId.Value);
            if (Convert.ToString(hddnOutStandingBlock.Value) == "1")
            {
                dtPartyTotal = oDBEngine.GetDataTable("Select DOC_TYPE,BAL_AMOUNT from PARTYOUTSTANDINGDET_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND SLNO=999999999 AND DOC_TYPE='Gross Outstanding:' AND PARTYTYPE='C'");
                PartyTotalBalDesc = dtPartyTotal.Rows[0][0].ToString();
                PartyTotalBalAmt = dtPartyTotal.Rows[0][1].ToString();

            }
            string summaryTAG = (e.Item as ASPxSummaryItem).Tag;
            if (e.IsTotalSummary == true)
            {
                switch (summaryTAG)
                {
                    case "Item_BalAmt":
                        e.Text = PartyTotalBalAmt;
                        break;
                    case "Item_DocType":
                        e.Text = PartyTotalBalDesc;
                        break;
                }
            }

        }
        protected void cCustomerOutstanding_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string strAsOnDate = Convert.ToString(e.Parameters.Split('~')[3]);
            string strCustomerId = Convert.ToString(e.Parameters.Split('~')[1]);
            string BranchId = e.Parameters.Split('~')[2];
            string strCommand = Convert.ToString(e.Parameters.Split('~')[0]);
            DataTable dtOutStanding = new DataTable();
            if (strCommand == "BindOutStanding")
            {
                dtOutStanding = objSlaesActivitiesBL.GetCustomerOutstandingRecords(strAsOnDate, strCustomerId, BranchId);

                //CustomerOutstanding.DataSource = dtOutStanding;
                //CustomerOutstanding.DataBind();
                CustomerOutstanding.JSProperties["cpOutStanding"] = "OutStanding";


            }
        }

        protected void ShowGridCustOut_HtmlFooterCellPrepared(object sender, ASPxGridViewTableFooterCellEventArgs e)
        {
            if (e.Column.Caption != "Doc. Type")
            {
                e.Cell.Style["text-align"] = "right";
            }

        }
        protected void ShowGridCustOut_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {

            if (Convert.ToString(e.CellValue) == "Party Outstanding:" || Convert.ToString(e.CellValue) == "Total:")
            {
                Session["chk_presenttotal"] = 1;
            }
            if (Convert.ToInt32(Session["chk_presenttotal"]) == 1)
            {
                e.Cell.Font.Bold = true;
                e.Cell.BackColor = System.Drawing.Color.DarkSeaGreen;
            }

            if (e.DataColumn.FieldName == "BAL_AMOUNT")
            {
                Session["chk_presenttotal"] = 0;
            }
        }
        protected void cmbExport1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                if (Session["exportval1"] == null)
                {
                    Session["exportval1"] = Filter;
                    bindexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportval1"]) != Filter)
                {
                    Session["exportval1"] = Filter;
                    bindexport(Filter);
                }
            }
        }
        public void bindexport(int Filter)
        {
            //CustomerOutstanding.Columns[5].Visible = false;
            string filename = "CustomerOutStanding";
            exporter1.FileName = filename;
            exporter1.FileName = "GrdCustomerOutstanding";

            exporter1.PageHeader.Left = "CustomerOutStanding";
            exporter1.PageFooter.Center = "[Page # of Pages #]";
            exporter1.PageFooter.Right = "[Date Printed]";

            switch (Filter)
            {
                case 1:
                    exporter1.WritePdfToResponse();
                    break;
                case 2:
                    exporter1.WriteXlsToResponse();
                    break;
                case 3:
                    exporter1.WriteRtfToResponse();
                    break;
                case 4:
                    exporter1.WriteCsvToResponse();
                    break;
            }
        }

        protected void SetTooltip()
        {
            // txtFinanceAmt.ToolTip = "Hello";
            //txtDBD.ToolTip = "Suppose total DBD is 2.5% on the product and we allow waiver of maximum of 2% so here we will write maximum upto 2%. The remaining amount (0.5%) will be written in EMI Card/Other Charges to calculate total DP.";
            //txtdownPayment.ToolTip = "Here we will write downpayment related to the product only. No extra charges like PF,DBD or EMI card charges.";
            //txtprocFee.ToolTip = "Here only applicable Processing Fees totalling for all the below mentioned products to be mentioned.";
            //txtEmiOtherCharges.ToolTip = "Here all the extra charges except PF to be collected from the customer along with downpayment like DBD,EMI Card Charges,ECS Charges etc.";
            //txtTotDpAmt.ToolTip = "Total DP will be Downpayment + PF + EMI Card/Other Charges will come automatically.";
            //txtFinanceAmt.ToolTip = "Financer Due = Invoice Value net of Old unit if applicable - Total DP Amt. as above";

        }

        protected void AvailableStockgrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            Session["AvailableStockPos"] = PosData.GetProductStockBranchWise(Convert.ToInt32(ddl_Branch.SelectedValue), Convert.ToString(HDSelectedProduct.Value));
            AvailableStockgrid.DataBind();
        }
        protected void AvailableStockgrid_DataBinding(object sender, EventArgs e)
        {
            DataTable availablestock = (DataTable)Session["AvailableStockPos"];
            if (availablestock != null)
            {
                AvailableStockgrid.DataSource = availablestock;

            }
        }

        protected void AvailableStockgrid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data)
                return;

            Decimal available = Convert.ToDecimal(e.GetValue("Available"));
            if (available < 0)
                e.Row.ForeColor = Color.Red;
            else if (available > 0)
                e.Row.ForeColor = Color.Blue;
            else
                e.Row.ForeColor = Color.Gray;
        }
        public void SetOtherChargesBanaerValue()
        {
            DataTable dt = (DataTable)Session["SI_TaxDetails" + Convert.ToString(uniqueId.Value)];
            string taxname = "";
            decimal totalTaxAmount = 0;
            foreach (DataRow dr in dt.Rows)
            {
                taxname = Convert.ToString(dr["Taxes_Name"]).Trim();
                if (taxname != "")
                {
                    if (taxname.Substring(taxname.Length - 2, 1) == "-")
                    {
                        totalTaxAmount = totalTaxAmount - Convert.ToDecimal(dr["Amount"]);
                    }
                    else
                    {

                        totalTaxAmount = totalTaxAmount + Convert.ToDecimal(dr["Amount"]);
                    }
                }

            }

            bnrOtherChargesvalue.Text = string.Format("{0:0.00}", totalTaxAmount);

        }

        public void RemoveSessionOnPage()
        {
            Session.Remove("SI_QuotationDetails" + Convert.ToString(uniqueId.Value));
            Session.Remove("SI_TaxDetails" + Convert.ToString(uniqueId.Value));
            Session.Remove("SI_FinalTaxRecord" + Convert.ToString(uniqueId.Value));
            Session.Remove("SI_WarehouseData" + Convert.ToString(uniqueId.Value));
            Session.Remove("SI_LoopWarehouse" + Convert.ToString(uniqueId.Value));
            Session.Remove("PosOldUnittable" + Convert.ToString(uniqueId.Value));
            Session.Remove("PosCustomerReceiptDetails" + Convert.ToString(uniqueId.Value));
            Session.Remove("SI_QuotationAddressDtl");
        }

        #region NEW warehouse
        public DataTable GetSerialataNew(string WarehouseID, string BatchID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
            proc.AddVarcharPara("@Action", 500, "GetSerialByProductIDWarehouseBatch");
            proc.AddVarcharPara("@Order_Id", 500, "0");
            proc.AddVarcharPara("@ProductID", 500, Convert.ToString(hdfProductID.Value));
            proc.AddVarcharPara("@BatchID", 500, BatchID);
            proc.AddVarcharPara("@WarehouseID", 500, WarehouseID);
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            proc.AddVarcharPara("@branchId", 2000, Convert.ToString(ddl_Branch.SelectedValue));
            proc.AddVarcharPara("@companyId", 500, Convert.ToString(Session["LastCompany"]));
            proc.AddVarcharPara("@Doc_Type", 500, "SC");
            proc.AddVarcharPara("@SC_Date", 10, Convert.ToDateTime(dt_PLQuote.Value).ToString("yyyy-MM-dd"));
            dt = proc.GetTable();
            return dt;
        }
        #endregion

        #region lookupBind


        protected void productLookUp_DataBinding(object sender, EventArgs e)
        {
            productLookUp.DataSource = (DataTable)Application.Get("POSPRODUCTLISTDATA");
        }

        #endregion

        #region PopulateBillingCountry State area city by pin
        protected void updateBillingDetailsByPin_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string pin = e.Parameter;
            if (pin != null && pin != "" && pin != "0")
            {

                DataSet PopulateAllAddressDetails = new DataSet();
                ProcedureExecute proc = new ProcedureExecute("prc_posCustomerBillingShipping");
                proc.AddVarcharPara("@Action", 50, "PopulateLabelDetailsByPin");
                proc.AddVarcharPara("@pinId", 10, pin);
                PopulateAllAddressDetails = proc.GetDataSet();

                //set Country 
                lblBillingCountryValue.Value = Convert.ToString(PopulateAllAddressDetails.Tables[0].Rows[0]["cou_id"]);
                lblBillingCountry.Text = Convert.ToString(PopulateAllAddressDetails.Tables[0].Rows[0]["cou_country"]);

                //Set State
                lblBillingStateValue.Value = Convert.ToString(PopulateAllAddressDetails.Tables[0].Rows[0]["stateId"]);
                lblBillingState.Text = Convert.ToString(PopulateAllAddressDetails.Tables[0].Rows[0]["state"]);
                lblBillingStateText.Value = Convert.ToString(PopulateAllAddressDetails.Tables[0].Rows[0]["state"]);
                //Set city
                lblBillingCityValue.Value = Convert.ToString(PopulateAllAddressDetails.Tables[0].Rows[0]["city_id"]);
                lblBillingCity.Text = Convert.ToString(PopulateAllAddressDetails.Tables[0].Rows[0]["city_name"]);



                //set Area
                CmbArea.DataSource = PopulateAllAddressDetails.Tables[1];
                CmbArea.TextField = "area_name";
                CmbArea.ValueField = "area_id";
                CmbArea.DataBind();

            }

        }

        public void populateBillingLabelDetailsbyPin(string pin)
        {
            if (pin != "" && pin != "0")
            {
                DataSet PopulateAllAddressDetails = new DataSet();
                ProcedureExecute proc = new ProcedureExecute("prc_posCustomerBillingShipping");
                proc.AddVarcharPara("@Action", 50, "PopulateLabelDetailsByPin");
                proc.AddVarcharPara("@pinId", 10, pin);
                PopulateAllAddressDetails = proc.GetDataSet();

                //set Country 
                lblBillingCountryValue.Value = Convert.ToString(PopulateAllAddressDetails.Tables[0].Rows[0]["cou_id"]);
                lblBillingCountry.Text = Convert.ToString(PopulateAllAddressDetails.Tables[0].Rows[0]["cou_country"]);
                txtbillingPin.Text = Convert.ToString(PopulateAllAddressDetails.Tables[0].Rows[0]["pin_code"]);
                //Set State
                lblBillingStateValue.Value = Convert.ToString(PopulateAllAddressDetails.Tables[0].Rows[0]["stateId"]);
                lblBillingState.Text = Convert.ToString(PopulateAllAddressDetails.Tables[0].Rows[0]["state"]);
                lblBillingStateText.Value = Convert.ToString(PopulateAllAddressDetails.Tables[0].Rows[0]["state"]);
                //Set city
                lblBillingCityValue.Value = Convert.ToString(PopulateAllAddressDetails.Tables[0].Rows[0]["city_id"]);
                lblBillingCity.Text = Convert.ToString(PopulateAllAddressDetails.Tables[0].Rows[0]["city_name"]);
            }

        }

        #endregion


        #region PopulateShipping Country State area city by pin


        public void PopulateShippingLabelDetailsbyPin(string pinId)
        {
            if (pinId != "" && pinId != "0")
            {

                DataSet PopulateAllAddressDetails = new DataSet();
                ProcedureExecute proc = new ProcedureExecute("prc_posCustomerBillingShipping");
                proc.AddVarcharPara("@Action", 50, "PopulateLabelDetailsByPin");
                proc.AddVarcharPara("@pinId", 10, pinId);
                PopulateAllAddressDetails = proc.GetDataSet();

                //set Country 
                lblShippingCountryValue.Value = Convert.ToString(PopulateAllAddressDetails.Tables[0].Rows[0]["cou_id"]);
                lblShippingCountry.Text = Convert.ToString(PopulateAllAddressDetails.Tables[0].Rows[0]["cou_country"]);
                txtShippingPin.Text = Convert.ToString(PopulateAllAddressDetails.Tables[0].Rows[0]["pin_code"]);
                //Set State
                lblShippingStateValue.Value = Convert.ToString(PopulateAllAddressDetails.Tables[0].Rows[0]["stateId"]);
                lblShippingState.Text = Convert.ToString(PopulateAllAddressDetails.Tables[0].Rows[0]["state"]);
                lblShippingStateText.Value = Convert.ToString(PopulateAllAddressDetails.Tables[0].Rows[0]["state"]);

                //Set city
                lblShippingCityValue.Value = Convert.ToString(PopulateAllAddressDetails.Tables[0].Rows[0]["city_id"]);
                lblShippingCity.Text = Convert.ToString(PopulateAllAddressDetails.Tables[0].Rows[0]["city_name"]);
            }
        }


        #endregion


        public void GetCashBalanceByBranch(string BranchId)
        {
            DataTable DT = oDBEngine.GetDataTable("Select Convert(decimal(18,2),Sum(AccountsLedger_AmountDr-AccountsLedger_AmountCr)) TotalAmount from Trans_AccountsLedger WHERE AccountsLedger_MainAccountID='Cash' and AccountsLedger_BranchId=" + BranchId);
            if (DT.Rows.Count != 0)
            {

                if (!String.IsNullOrEmpty(Convert.ToString(DT.Rows[0]["TotalAmount"])))
                {
                    B_BankBalance.InnerHtml = Convert.ToString(DT.Rows[0]["TotalAmount"]);
                }


            }

        }


        #region populate all data with Branch
        [WebMethod]
        public static object GetAllDetailsByBranch(string BranchId, string EntryType)
        {
            PosSalesInvoiceBl PosData = new PosSalesInvoiceBl();
            string strCompanyID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            string FinYear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);

            DataSet AllDetailsByBranch = PosData.GetAllDetailsByBranch(BranchId, strCompanyID, FinYear);
            //SlaesActivitiesBL objSlaesActivitiesBL1 = new SlaesActivitiesBL();

            AllDetailsByBranch allDetailsByBranch = new AllDetailsByBranch();

            DataTable salsemanData = AllDetailsByBranch.Tables[0];

            List<KeyValueClass> SalesMan = new List<KeyValueClass>();
            SalesMan = (from DataRow dr in salsemanData.Rows
                        select new KeyValueClass()
                        {
                            Id = dr["cnt_internalId"].ToString(),
                            Name = dr["Name"].ToString()
                        }).ToList();

            allDetailsByBranch.SalesMan = SalesMan;





            DataTable Schemadt = AllDetailsByBranch.Tables[1]; // objSlaesActivitiesBL1.GetNumberingSchema(strCompanyID, BranchId, FinYear, "10", "Y");

            List<KeyValueClass> ChallanNumberScheme = new List<KeyValueClass>();
            ChallanNumberScheme = (from DataRow dr in Schemadt.Rows
                                   select new KeyValueClass()
                                   {
                                       Id = dr["Id"].ToString(),
                                       Name = dr["SchemaName"].ToString()
                                   }).ToList();

            allDetailsByBranch.ChallanNumberScheme = ChallanNumberScheme;

            DataTable Financer = AllDetailsByBranch.Tables[2];

            List<KeyValueClass> financer = new List<KeyValueClass>();
            financer = (from DataRow dr in Financer.Rows
                        select new KeyValueClass()
                        {
                            Id = dr["cnt_internalId"].ToString(),
                            Name = dr["cnt_firstName"].ToString(),
                            otherDetails = Convert.ToString(dr["cnt_mainAccount"])
                        }).ToList();

            allDetailsByBranch.Financer = financer;


            DataTable Executive = AllDetailsByBranch.Tables[3];
            List<KeyValueClass> executive = new List<KeyValueClass>();
            executive = (from DataRow dr in Executive.Rows
                         select new KeyValueClass()
                         {
                             Id = dr["cnt_internalId"].ToString(),
                             Name = dr["ExecName"].ToString(),
                             otherDetails = Convert.ToString(dr["Fin_InternalId"])
                         }).ToList();

            allDetailsByBranch.Executive = executive;


            return allDetailsByBranch;
        }

        public class AllDetailsByBranch
        {
            public List<KeyValueClass> SalesMan { get; set; }
            public List<KeyValueClass> ChallanNumberScheme { get; set; }
            public List<KeyValueClass> Financer { get; set; }
            public List<KeyValueClass> Executive { get; set; }
        }
        public class KeyValueClass
        {
            public string Id { get; set; }
            public String Name { get; set; }
            public string otherDetails { get; set; }
        }



        #endregion

        #region New Billing Shipping
        [WebMethod]
        public static object SaveCustomer(string UniqueID, string Name, string BillingAddress1, string BillingAddress2, string BillingPin, string shippingAddress1, string shippingAddress2, string shippingPin, string GSTIN, string BillingPhone, string ShippingPhone, string contactperson)
        {
            BusinessLogicLayer.Contact oContactGeneralBL = new BusinessLogicLayer.Contact();
            MShortNameCheckingBL mshort = new MShortNameCheckingBL();
            try
            {
                string entityName = "";
                if (!mshort.CheckUniqueWithtypeContactMaster(UniqueID, "", "MasterContactType", "CL", ref entityName))
                {
                    //rev srijeeta mantis issue 0024515
                    //string InternalId = oContactGeneralBL.Insert_ContactGeneral("Customer/Client", UniqueID, "1",
                    //                                    Name, "", "",
                    //                                    "", Convert.ToString(HttpContext.Current.Session["userbranchID"]), "2",
                    //                                    "0", new DateTime(1900, 1, 1, 0, 0, 0, 0), new DateTime(1900, 1, 1, 0, 0, 0, 0), "1",
                    //                                    "1", "1", "",
                    //                                    "0", "0", "0",
                    //                                    "1", "", "", "CL",
                    //                                    "1", DateTime.Now, "1", "",
                    //                                    "1", "No", "", Convert.ToString(HttpContext.Current.Session["userid"]), "",
                    //                                    DateTime.Now, "", "78", false, 0,
                    //                                    0, "A",
                    //                                    "", "");
                    string InternalId = oContactGeneralBL.Insert_ContactGeneral("Customer/Client", UniqueID, "1","1",
                                                        Name, "", "",
                                                        "", Convert.ToString(HttpContext.Current.Session["userbranchID"]), "2",
                                                        "0", new DateTime(1900, 1, 1, 0, 0, 0, 0), new DateTime(1900, 1, 1, 0, 0, 0, 0), "1",
                                                        "1", "1", "",
                                                        "0", "0", "0",
                                                        "1", "", "", "CL",
                                                        "1", DateTime.Now, "1", "",
                                                        "1", "No", "", Convert.ToString(HttpContext.Current.Session["userid"]), "",
                                                        DateTime.Now, "", "78", false, 0,
                                                        0, "A",
                                                        "", "");
                    //end of rev srijeeta mantis issue 0024515

                    ProcedureExecute proc = new ProcedureExecute("prc_GetSetCustomerPopup");
                    proc.AddVarcharPara("@action", 500, "SaveBillingShipping");
                    proc.AddVarcharPara("@pin", 10, BillingPin);
                    proc.AddVarcharPara("@billingAddress1", 100, BillingAddress1);
                    proc.AddVarcharPara("@billingAddress2", 100, BillingAddress2);
                    proc.AddVarcharPara("@shippingpin", 10, shippingPin);
                    proc.AddVarcharPara("@shippingbillingAddress1", 100, shippingAddress1);
                    proc.AddVarcharPara("@shippingbillingAddress2", 100, shippingAddress2);
                    proc.AddVarcharPara("@customerInternalId", 10, InternalId);
                    proc.AddVarcharPara("@cntUcc", 50, UniqueID);
                    proc.AddVarcharPara("@GSTIN", 20, GSTIN);
                    proc.AddVarcharPara("@BillingPhone", 20, BillingPhone);
                    proc.AddVarcharPara("@ShippingPhone", 20, ShippingPhone);
                    proc.AddVarcharPara("@contactperson", 40, contactperson);
                    proc.RunActionQuery();


                    return new { status = "Ok", InternalId = InternalId };
                }
                else
                {
                    return new { status = "Error", Msg = "Already Exists" };
                }
            }
            catch (Exception ex)
            {
                return new { status = "Error", Msg = ex.Message };
            }
        }

        #endregion



        #region billing Shipping service

        [WebMethod]
        public static object GetAddressbyPin(string PinCode)
        {
            if (HttpContext.Current.Session["userid"] != null)
            {
                ProcedureExecute proc = new ProcedureExecute("prc_posCustomerBillingShipping");
                proc.AddVarcharPara("@Action", 50, "PopulateAddbyPincode");
                proc.AddVarcharPara("@Pincode", 50, PinCode);
                DataTable Address = proc.GetTable();

                if (Address.Rows.Count > 0)
                {
                    AddressDetails details = new AddressDetails();
                    details.pinId = Convert.ToInt32(Address.Rows[0]["pin_id"]);
                    details.ConId = Convert.ToInt32(Address.Rows[0]["cou_id"]);
                    details.ConName = Convert.ToString(Address.Rows[0]["cou_country"]);
                    details.stId = Convert.ToInt32(Address.Rows[0]["stateId"]);
                    details.stName = Convert.ToString(Address.Rows[0]["state"]);
                    details.cityId = Convert.ToInt32(Address.Rows[0]["city_id"]);
                    details.cityName = Convert.ToString(Address.Rows[0]["city_name"]);
                    return details;

                }

            }
            return null;
        }
        public class AddressDetails
        {
            public int pinId { get; set; }
            public int ConId { get; set; }
            public string ConName { get; set; }
            public int stId { get; set; }
            public string stName { get; set; }
            public int cityId { get; set; }
            public string cityName { get; set; }
        }

        #endregion


        #region Save session For UOM packing Surojit
        [WebMethod]
        public static string SetSessionPacking(string list)
        {
            System.Web.Script.Serialization.JavaScriptSerializer jsSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            List<ProductQuantity> finalResult = jsSerializer.Deserialize<List<ProductQuantity>>(list);
            //var result = JsonConvert.DeserializeObject<ProductQuantity>(list);
            HttpContext.Current.Session["SessionPackingDetails"] = finalResult;
            return null;

        }
        #endregion

        protected void ASPxCallbackPanelVehicle_Callback(object sender, CallbackEventArgsBase e)
        {
            //string strInvoiceID = Convert.ToString(Session["SI_InvoiceID"]);
            //DataTable ComponentTable = objSalesInvoiceBL.GetVehicle();
            //poupVehicles.GridView.Selection.CancelSelection();
            //poupVehicles.DataSource = ComponentTable;
            //poupVehicles.DataBind();
            //string inv = Session["Invoices"].ToString();
            //PosSalesInvoiceBl ibl = new PosSalesInvoiceBl();
            //DataSet dtdata = new DataSet();
            //dtdata = ibl.GetInvoiceDeleviryData(inv);
            //DataTable headTable = dtdata.Tables[0];
            //if (headTable != null && headTable.Rows.Count > 0)
            //{
            //    foreach (DataRow item in headTable.Rows)
            //    {
            //        poupVehicles.GridView.Selection.SelectRowByKey(Convert.ToInt32(item["VECHICLE_ID"].ToString()));
            //    }
            //}
        }

        //chinmoy added for product popup
        //Start 02-08-2019
        //protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        //{
        //    e.KeyExpression = "Products_ID";

        //    //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
        //    string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);



        //    //string IsFilter = Convert.ToString(hfIsFilter.Value);
        //    //string strFromDate = Convert.ToString(hfFromDate.Value);
        //    //string strToDate = Convert.ToString(hfToDate.Value);
        //    //string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);

        //    List<int> branchidlist;

        //    //if (IsFilter == "Y")
        //    //{
        //    //    if (strBranchID == "0")
        //    //    {
        //    string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
        //    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));

        //    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
        //    var q = from d in dc.v_Prods
        //            //where branchidlist.Contains(Convert.ToInt32(d.BranchID))
        //            //&& d.NoteDate >= Convert.ToDateTime(strFromDate) && d.NoteDate <= Convert.ToDateTime(strToDate)

        //            orderby d.Products_Name descending
        //            select d;
        //    e.QueryableSource = q;
        //    //    }
        //    //    else
        //    //    {
        //    //        branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));

        //    //        ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
        //    //        var q = from d in dc.v_CustomerNoteLists
        //    //                where
        //    //                branchidlist.Contains(Convert.ToInt32(d.BranchID))
        //    //                && d.NoteDate >= Convert.ToDateTime(strFromDate) && d.NoteDate <= Convert.ToDateTime(strToDate)
        //    //                orderby d.NoteDate descending
        //    //                select d;
        //    //        e.QueryableSource = q;
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
        //    //    var q = from d in dc.v_CustomerNoteLists
        //    //            where d.BranchID == '0'
        //    //            orderby d.NoteDate descending
        //    //            select d;
        //    //    e.QueryableSource = q;
        //    //}
        //}
        //End 02-08-2019

        protected void poupVehicles_DataBinding(object sender, EventArgs e)
        {
            if (Session["SI_VehicleDetails"] != null)
            {
                //lookup_quotation.DataSource = (DataTable)Session["SI_VehicleDetails"];
            }
        }

        #region Tanmoy For SMS

        [WebMethod]
        public static string GetPhnNoForSMS(string InternalID)
        {
            string phoneno = string.Empty;
            DataTable DT = new DataTable();
            DBEngine objEngine = new DBEngine();
            DT = objEngine.GetDataTable("select phf_phoneNumber from tbl_master_phonefax where phf_cntId='" + InternalID + "' and phf_type='Mobile'");
            if (DT != null && DT.Rows.Count > 0)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(DT.Rows[0]["phf_phoneNumber"])))
                {
                    phoneno = Convert.ToString(DT.Rows[0]["phf_phoneNumber"]);
                }
            }
            return phoneno;
        }

        #endregion
    }
}