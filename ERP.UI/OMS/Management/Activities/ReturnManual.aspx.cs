/*****************************************************************************************
 * Rev 1.0   Sanchita   V2.0.39     22-09-2023      GST is showing Zero in the TAX Window whereas GST in the Grid calculated. 
 *                                                  Mantis: 26843
 *                                                  Session["MultiUOMData"] has been renamed to Session["MultiUOMDataRETM"]
 * *****************************************************************************************/
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
using System.Globalization;
using ERP.OMS.Tax_Details.ClassFile;
using ERP.Models;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using System.Web.Hosting;
using System.IO;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;

namespace ERP.OMS.Management.Activities
{
    public partial class ReturnManual : ERP.OMS.ViewState_class.VSPage// System.Web.UI.Page
    {
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();


       // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        clsDropDownList oclsDropDownList = new clsDropDownList();
        BusinessLogicLayer.Contact oContactGeneralBL = new BusinessLogicLayer.Contact();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        static string ForJournalDate = null;
        SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
        SalesReturnBL objSalesReturnBL = new SalesReturnBL();
        CRMSalesDtlBL objCRMSalesDtlBL = new CRMSalesDtlBL();
        ERPDocPendingApprovalBL objERPDocPendingApproval = new ERPDocPendingApprovalBL();
        GSTtaxDetails gstTaxDetails = new GSTtaxDetails();
        string UniqueQuotation = string.Empty;
        public string pageAccess = "";
        string userbranch = "";
        BusinessLogicLayer.RemarkCategoryBL reCat = new BusinessLogicLayer.RemarkCategoryBL();
        DataTable Remarks = null;
        #region Code Checked and Modified
        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {
            if (Request.QueryString.AllKeys.Contains("status") || Request.QueryString.AllKeys.Contains("IsTagged"))
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

        protected void Page_Init(object sender, EventArgs e)
        {

           // CountrySelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
           // StateSelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
           // SelectCity.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
           // SelectArea.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
           // SelectPin.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
           // sqltaxDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
           // dsCustomer.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            UomSelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            AltUomSelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            //if (!IsPostBack)
            //{
            //    ((GridViewDataComboBoxColumn)grid.Columns["ProductID"]).PropertiesComboBox.DataSource = GetProduct();
            //}
        }

        #endregion Code Checked and Modified  By Sam on 17022016

        public void IsExistsDocumentInERPDocApproveStatus(string strQuotationId)
        {
            
        }
        public void GetEditablePermission()
        {
            
        }

        protected int getUdfCount()
        {
            DataTable udfCount = oDBEngine.GetDataTable("select 1 from tbl_master_remarksCategory rc where cat_applicablefor='SR'  and ( exists (select * from tbl_master_udfGroup where id=rc.cat_group_id and grp_isVisible=1) or rc.cat_group_id=0)");
            return udfCount.Rows.Count;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/ReturnManual.aspx");
            if (HttpContext.Current.Session["userid"] == null)
            {
                //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }

            CommonBL ComBL = new CommonBL();
            //Rev work start 24.06.2022 mantise no:0024987            
            string SalesmanCaption = ComBL.GetSystemSettingsResult("ShowCoordinator");

            if (!String.IsNullOrEmpty(SalesmanCaption))
            {
                if (SalesmanCaption.ToUpper().Trim() == "NO")
                {
                    ASPxLabel3.Text = "Salesman/Agents";

                }
                else if (SalesmanCaption.ToUpper().Trim() == "YES")
                {
                    ASPxLabel3.Text = "Coordinator";
                }
            }
            //Rev work close 24.06.2022 mantise no:0024987
            string ProjectSelectInEntryModule = ComBL.GetSystemSettingsResult("ProjectSelectInEntryModule");
            if (!String.IsNullOrEmpty(ProjectSelectInEntryModule))
            {
                if (ProjectSelectInEntryModule == "Yes")
                {
                    hdnProjectSelectInEntryModule.Value = "1";
                    lookup_Project.ClientVisible = true;
                    lblProject.Visible = true;
                }
                else if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
                {
                    hdnProjectSelectInEntryModule.Value = "0";
                    lookup_Project.ClientVisible = false;
                    lblProject.Visible = false;
                }
            }

            string ProjectMandatoryInEntry = ComBL.GetSystemSettingsResult("ProjectMandatoryInEntry");

            if (!String.IsNullOrEmpty(ProjectMandatoryInEntry))
            {
                if (ProjectMandatoryInEntry == "Yes")
                {
                    hdnProjectMandatory.Value = "1";



                }
                else if (ProjectMandatoryInEntry.ToUpper().Trim() == "NO")
                {
                    hdnProjectMandatory.Value = "0";


                }
            }

            string MultiUOMSelection = ComBL.GetSystemSettingsResult("MultiUOMSelection");
            if (!String.IsNullOrEmpty(MultiUOMSelection))
            {
                if (MultiUOMSelection.ToUpper().Trim() == "YES")
                {
                    hddnMultiUOMSelection.Value = "1";

                }
                else if (MultiUOMSelection.ToUpper().Trim() == "NO")
                {
                    hddnMultiUOMSelection.Value = "0";
                    grid.Columns[7].Width = 0;
                    // Mantis Issue 24831
                    grid.Columns[8].Width = 0;
                    grid.Columns[9].Width = 0;
                    // Mantis Issue 24831
                }
            }

            //For Hierarchy Start Tanmoy
            string HierarchySelectInEntryModule = ComBL.GetSystemSettingsResult("Show_Hierarchy");
            if (!String.IsNullOrEmpty(HierarchySelectInEntryModule))
            {
                if (HierarchySelectInEntryModule.ToUpper().Trim() == "YES")
                {
                    ddlHierarchy.Visible = true;
                    lblHierarchy.Visible = true;
                    lookup_Project.Columns[3].Visible = true;
                }
                else if (HierarchySelectInEntryModule.ToUpper().Trim() == "NO")
                {
                    ddlHierarchy.Visible = false;
                    lblHierarchy.Visible = false;
                    lookup_Project.Columns[3].Visible = false;
                }
            }
            //For Hierarchy End Tanmoy

            if (!IsPostBack)
            {

                //lookup_Project.GridView.DataBind();



                string BackDatedEntryPurchaseReturnManual = ComBL.GetSystemSettingsResult("BackDatedEntrySalesReturnManual");
                if (!String.IsNullOrEmpty(BackDatedEntryPurchaseReturnManual))
                {
                    if (BackDatedEntryPurchaseReturnManual.ToUpper().Trim() == "YES")
                    {
                        HdnBackDatedEntryPurchaseGRN.Value = "1";

                    }
                    else if (BackDatedEntryPurchaseReturnManual.ToUpper().Trim() == "NO")
                    {
                        HdnBackDatedEntryPurchaseGRN.Value = "0";

                    }
                }


                #region New Tax Block

                string ItemLevelTaxDetails = string.Empty; string HSNCodewisetaxSchemid = string.Empty; string BranchWiseStateTax = string.Empty; string StateCodeWiseStateIDTax = string.Empty;
                gstTaxDetails.GetTaxData(ref ItemLevelTaxDetails, ref HSNCodewisetaxSchemid, ref BranchWiseStateTax, ref StateCodeWiseStateIDTax, "S");
                HDItemLevelTaxDetails.Value = ItemLevelTaxDetails;
                HDHSNCodewisetaxSchemid.Value = HSNCodewisetaxSchemid;
                HDBranchWiseStateTax.Value = BranchWiseStateTax;
                HDStateCodeWiseStateIDTax.Value = StateCodeWiseStateIDTax;

                #endregion


                this.Session["LastCompanySRM"] = Session["LastCompany"];
                this.Session["LastFinYearSRM"] = Session["LastFinYear"];
                //18-10-2017
                //   GetProductData();
                //18-10-2017
                //txt_PLQuoteNo.Enabled = false;
                ddl_numberingScheme.Focus();
                #region Sandip Section For Checking User Level for Allow Edit to logging User or Not
                //   GetEditablePermission();
                //   PopulateCustomerDetail();
                SetFinYearCurrentDate();
                GetFinacialYearBasedQouteDate();
                if (Session["userbranchHierarchy"] != null)
                {
                    userbranch = Convert.ToString(Session["userbranchHierarchy"]);
                }
                GetAllDropDownDetailForSalesQuotation(userbranch);

                if (Request.QueryString.AllKeys.Contains("status"))
                {
                    divcross.Visible = false;
                    btn_SaveRecords.Visible = false;
                   
                    ddl_Branch.Enabled = false;
                }
                else
                {
                    divcross.Visible = true;
                    btn_SaveRecords.Visible = true;
                  
                    ddl_Branch.Enabled = false; // Change Due to Numbering Schema
                }
                dt_PlQuoteExpiry.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
                string finyear = Convert.ToString(Session["LastFinYear"]);
                IsUdfpresent.Value = Convert.ToString(getUdfCount());
                hdnAddressDtl.Value = "0";
                #endregion Sandip Section

                #region Session Null Initialization Start
                //Session["QuotationAddressDtl"] = null;
                Session["SRM_BillingAddressLookup"] = null;
                Session["SRM_ShippingAddressLookup"] = null;
                #endregion Session Null Initialization End


                //Purpose : Binding Batch Edit Grid
                //Name : Sudip 
                // Dated : 21-01-2017
                Session["RM_ComponentData"] = null;
                Session["SRM_ReturnID"] = "";
                Session["SRM_CustomerDetail"] = null;
                Session["SRM_QuotationDetails"] = null;
                Session["SRM_WarehouseData"] = null;
                Session["SRM_QuotationTaxDetails"] = null;
                Session["LoopSRWarehouse"] = 1;
                Session["SRM_TaxDetails"] = null;
                Session["SRM_ActionType"] = "";
                Session["MultiUOMDataRETM"] = null;
                Session["TaggingInvoce"] = "";
                Session["SRM_ComponentData"] = null;
                Session["SRwarehousedetailstemp"] = null;
                Session["SRwarehousedetailstempUpdate"] = null;
                Session["SRwarehousedetailstempDelete"] = null;
                Session["SalesReturnAddressDtl"] = null;
                Session["InlineRemarks"] = null;
                PopulateGSTCSTVATCombo(DateTime.Now.ToString("yyyy-MM-dd"));
                PopulateChargeGSTCSTVATCombo(DateTime.Now.ToString("yyyy-MM-dd"));
                string strQuotationId = "";
                string strSalesReturnId = "";
                hdnCustomerId.Value = "";
                hdnPageStatus.Value = "first";

                //LoadGSTCSTVATCombo();
                //Session["SRQuotationAddressDtl"] = null;
                Session["SRM_QuotationAddressDtl"] = null;

                //string strLocalCurrency = Convert.ToString(Session["LocalCurrency"]);
                //if(strLocalCurrency!="")
                //{
                //    string[] currencyList = strLocalCurrency.Split(new string[] { "~" }, StringSplitOptions.None);
                //    string currency = currencyList[1];
                //    grid.Columns["TotalAmount"].Caption = "Total Amount in " + currency;
                //}

                //Tanmoy Hierarchy
                bindHierarchy();
                ddlHierarchy.Enabled = false;
                //Tanmoy Hierarchy End

                if (Request.QueryString["key"] != null)
                {
                    if (Convert.ToString(Request.QueryString["key"]) != "ADD")
                    {
                        dt_PLQuote.ClientEnabled = false;
                        lblHeadTitle.Text = "Modify Sale Return Manual";
                        hdnPageStatus.Value = "update";
                        divScheme.Style.Add("display", "none");
                        strSalesReturnId = Convert.ToString(Request.QueryString["key"]);
                        Session["SRM_KeyVal_InternalID"] = "SRQUOTE" + strSalesReturnId;


                        Keyval_internalId.Value = "ManualSaleReturn" + strSalesReturnId;
                        #region Subhra Section
                        Session["SRM_key_QutId"] = strSalesReturnId;
                        //if (Request.QueryString["status"] == null)
                        //{
                        //    IsExistsDocumentInERPDocApproveStatus(strSalesReturnId);
                        //}

                        #endregion End Section

                        #region Product, Quotation Tax, Warehouse

                        Session["SRM_ReturnID"] = strSalesReturnId;
                        Session["SRM_ActionType"] = "Edit";
                        SetSalesReturnDetails(strSalesReturnId);
                        //  DateTime quoteDate = Convert.ToDateTime(dt_PLQuote.Date.ToString("dd/MM/yyyy"));
                        Session["SRM_TaxDetails"] = GetTaxDataWithGST(GetTaxData(dt_PLQuote.Date.ToString("yyyy-MM-dd")));

                        //  Session["SRM_WarehouseData"] = GetQuotationWarehouseData();
                        DataSet allproductdetails = new DataSet();
                        DataTable Productdt = new DataTable();
                        if (Session["TaggingInvoce"] == "1")
                        {

                            allproductdetails = GetSalesReturnManualTaggingProductData(strSalesReturnId, Convert.ToString(Session["LastFinYear"]), Convert.ToString(Session["LastCompany"]));
                            Productdt = allproductdetails.Tables[0];
                        }
                        else
                        {
                            allproductdetails = GetSalesReturnManualProductData(strSalesReturnId, Convert.ToString(Session["LastFinYear"]), Convert.ToString(Session["LastCompany"]));
                            Productdt = allproductdetails.Tables[0];
                        }
                        if (allproductdetails.Tables[1].Rows.Count>0)
                        {
                            Session["InlineRemarks"]=allproductdetails.Tables[1];
                        }
                        Session["MultiUOMDataRETM"] = GetMultiUOMData();
                        Session["SRM_QuotationDetails"] = Productdt;
                        //rev rajdip for running data on edit mode

                        DataTable Orderdt = Productdt.Copy();
                       
                        decimal TotalQty = 0;
                        decimal TotalAmt = 0;
                        decimal TaxAmount = 0;
                        decimal Amount = 0;
                        decimal SalePrice = 0;
                        decimal AmountWithTaxValue = 0;
                        for (int i = 0; i < Orderdt.Rows.Count; i++)
                        {
                            TotalQty = TotalQty + Convert.ToDecimal(Orderdt.Rows[i]["TotalQty"]);
                            Amount = Amount + Convert.ToDecimal(Orderdt.Rows[i]["Amount"]);
                            TaxAmount = TaxAmount + Convert.ToDecimal(Orderdt.Rows[i]["TaxAmount"]);
                            SalePrice = SalePrice + Convert.ToDecimal(Orderdt.Rows[i]["SalePrice"]);
                            TotalAmt = TotalAmt + Convert.ToDecimal(Orderdt.Rows[i]["TotalAmount"]);
                          
                        }
                        AmountWithTaxValue = TaxAmount + Amount;

                        ASPxLabel12.Text = TotalQty.ToString();
                        bnrLblTaxableAmtval.Text = Amount.ToString();
                        bnrLblTaxAmtval.Text = TaxAmount.ToString();
                        bnrlblAmountWithTaxValue.Text = AmountWithTaxValue.ToString();
                       // bnrLblInvValue.Text = TotalAmt.ToString();


                        decimal amount = 0;
                        decimal addcharge = 0;
                        decimal totamnt = 0;
                        amount = Math.Round(Convert.ToDecimal(bnrlblAmountWithTaxValue.Text.ToString()), 2);
                        addcharge = Math.Round(Convert.ToDecimal(bnrOtherChargesvalue.Text.ToString()), 2);
                        totamnt = addcharge + amount;
                        bnrLblInvValue.Text = Convert.ToString(Math.Round(Convert.ToDecimal(totamnt.ToString()), 2));


                        //end rev rajdip
                        grid.DataSource = GetQuotation(Productdt);
                        grid.DataBind();

                        Session["SRM_QuotationAddressDtl"] = objSalesReturnBL.GetSalesReturnBillingAddress(strSalesReturnId); ;

                        #endregion

                        #region Debjyoti Get Tax Details in Edit Mode


                        if (GetQuotationTaxData().Tables[0] != null)
                            Session["SRM_QuotationTaxDetails"] = GetQuotationTaxData().Tables[0];

                        if (GetQuotationEditedTaxData().Tables[0] != null)
                        {
                            DataTable quotetable = GetQuotationEditedTaxData().Tables[0];
                            if (quotetable == null)
                            {
                                CreateDataTaxTable();
                            }
                            else
                            {
                                Session["SRM_FinalTaxRecord"] = quotetable;
                            }
                        }
                        #endregion Debjyoti Get Tax Details in Edit Mode

                        #region Samrat Roy -- Hide Save Button in Edit Mode
                        if (Request.QueryString["req"] != null && Request.QueryString["req"] == "V")
                        {
                            lblHeadTitle.Text = "View Sale Return Manual";
                            lbl_quotestatusmsg.Text = "*** View Mode Only";
                            btn_SaveRecords.Visible = false;
                            ASPxButton2.Visible = false;
                            lbl_quotestatusmsg.Visible = true;
                        }
                        #endregion

                        if (IsSRTransactionExist(Request.QueryString["key"]))
                        {

                            tdSaveButtonNew.Style.Add("display", "none");
                            tdSaveButton.Style.Add("display", "none");
                            tagged.Style.Add("display", "block");
                        }
                    }
                    else
                    {

                        DataTable dtposTime = oDBEngine.GetDataTable("SELECT  top 1 convert(varchar(50),Lock_Fromdate,110) LockCon_Fromdate,convert(varchar(50),Lock_Todate,110) LockCon_Todate,convert(varchar(10),Lock_Fromdate,105) DataFreeze_Fromdate,convert(varchar(10),Lock_Todate,105) DataFreeze_Todate FROM Trans_LockConfigouration_Details WHERE  Type='Add' and Module_Id=13");

                        if (dtposTime != null && dtposTime.Rows.Count > 0)
                        {
                            hdnLockFromDate.Value = Convert.ToString(dtposTime.Rows[0]["LockCon_Fromdate"]);
                            hdnLockToDate.Value = Convert.ToString(dtposTime.Rows[0]["LockCon_Todate"]);
                            hdnLockFromDateCon.Value = Convert.ToString(dtposTime.Rows[0]["DataFreeze_Fromdate"]);
                            hdnLockToDateCon.Value = Convert.ToString(dtposTime.Rows[0]["DataFreeze_Todate"]);
                        }

                        #region To Show By Default Cursor after SAVE AND NEW
                        if (Session["SRM_SaveMode"] != null)  // it has been removed from coding side of Quotation list 
                        {
                            if (Session["SRM_schemavalue"] != null)  // it has been removed from coding side of Quotation list 
                            {
                                ddl_numberingScheme.SelectedValue = Convert.ToString(Session["SRM_schemavalue"]); // it has been removed from coding side of Quotation list 
                                ddl_Branch.SelectedValue = Convert.ToString(Session["SRM_schemavalue"]).Split('~')[3];
                            }
                            if (Convert.ToString(Session["SRM_SaveMode"]) == "A")
                            {
                                dt_PLQuote.Focus();
                                txt_PLQuoteNo.Enabled = false;
                                txt_PLQuoteNo.Text = "Auto";
                            }
                            else if (Convert.ToString(Session["SRM_SaveMode"]) == "M")
                            {
                                txt_PLQuoteNo.Enabled = true;
                                txt_PLQuoteNo.Text = "";
                                txt_PLQuoteNo.Focus();

                            }
                        }
                        else
                        {
                            ddl_numberingScheme.Focus();
                        }
                        #endregion To Show By Default Cursor after SAVE AND NEW



                        Keyval_internalId.Value = "Add";
                        Session["SRM_ActionType"] = "Add";
                        //ASPxButton2.Enabled = false;
                        Session["SRM_TaxDetails"] = null;
                        CreateDataTaxTable();
                        lblHeadTitle.Text = "Add Sale Return Manual";
                        ddl_AmountAre.Value = "1";
                        ddl_VatGstCst.SelectedIndex = 0;
                        ddl_VatGstCst.ClientEnabled = false;


                    }
                }
                if (Request.QueryString.AllKeys.Contains("IsTagged"))
                {
                   
                    divcross.Visible = false;


                }
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "GridCallBack()", true);
                GetSalesReturnSchemaLength();


                MasterSettings objmaster = new MasterSettings(); // Surojit 14-03-2019
                hdnConvertionOverideVisible.Value = objmaster.GetSettings("ConvertionOverideVisible");// Surojit 14-03-2019
                hdnShowUOMConversionInEntry.Value = objmaster.GetSettings("ShowUOMConversionInEntry");// Surojit 14-03-2019

                //20-05-2019 Surojit
                hdnPostingDateDisable.Value = Convert.ToString(0);
                BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
                DataTable DT = objEngine.GetDataTable("Config_SystemSettings", " Variable_Value ", " Variable_Name='PostingDateDisable' AND IsActive=1");
                if (DT != null && DT.Rows.Count > 0)
                {
                    string IsVisible = Convert.ToString(DT.Rows[0]["Variable_Value"]).Trim();

                    if (IsVisible == "Yes")
                    {
                        hdnPostingDateDisable.Value = Convert.ToString(1);
                    }
                    else
                    {
                        hdnPostingDateDisable.Value = Convert.ToString(0);
                    }
                }
                //20-05-2019
            }
            else
            {
                // PopulateCustomerDetail();
            }
           // 
        }
        private bool IsSRTransactionExist(string SRid)
        {
            bool IsExist = false;
            if (SRid != "" && Convert.ToString(SRid).Trim() != "")
            {
                DataTable dt = new DataTable();
                dt = objSalesReturnBL.CheckSRTraanaction(SRid);
                if (dt.Rows.Count > 0 && Convert.ToString(dt.Rows[0]["isexist"]) != "0")
                {
                    IsExist = true;
                }
            }

            return IsExist;
        }

        public void GetSalesReturnSchemaLength()
        {
            DataTable Dt = new DataTable();
            Dt = objSalesReturnBL.GetSchemaLengthSalesReturn();
            if (Dt != null && Dt.Rows.Count > 0)
            {
                hdnSchemaLength.Value = Convert.ToString(Dt.Rows[0]["length"]);

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

        #region Product Details, Warehouse


        public void SetQuotationDetails()
        {
            DataTable QuotationEditdt = GetQuotationEditData();
            if (QuotationEditdt != null && QuotationEditdt.Rows.Count > 0)
            {
                string Branch_Id = Convert.ToString(QuotationEditdt.Rows[0]["Quote_BranchId"]);
                string Quote_Number = Convert.ToString(QuotationEditdt.Rows[0]["Quote_Number"]);
                string Quote_Date = Convert.ToString(QuotationEditdt.Rows[0]["Quote_Date"]);
                string Quote_Expiry = Convert.ToString(QuotationEditdt.Rows[0]["Quote_Expiry"]);
                string Customer_Id = Convert.ToString(QuotationEditdt.Rows[0]["Customer_Id"]);
                string Contact_Person_Id = Convert.ToString(QuotationEditdt.Rows[0]["Contact_Person_Id"]);
                string Quote_Reference = Convert.ToString(QuotationEditdt.Rows[0]["Quote_Reference"]);
                string Currency_Id = Convert.ToString(QuotationEditdt.Rows[0]["Currency_Id"]);
                string Currency_Conversion_Rate = Convert.ToString(QuotationEditdt.Rows[0]["Currency_Conversion_Rate"]);
                string Tax_Option = Convert.ToString(QuotationEditdt.Rows[0]["Tax_Option"]);
                string Tax_Code = Convert.ToString(QuotationEditdt.Rows[0]["Tax_Code"]);
                string Quote_SalesmanId = Convert.ToString(QuotationEditdt.Rows[0]["Quote_SalesmanId"]);
                string IsUsed = Convert.ToString(QuotationEditdt.Rows[0]["IsUsed"]);

                txt_PLQuoteNo.Text = Quote_Number;
                dt_PLQuote.Date = Convert.ToDateTime(Quote_Date);
                PopulateGSTCSTVATCombo(Convert.ToDateTime(Quote_Date).ToString("yyyy-MM-dd"));
                PopulateChargeGSTCSTVATCombo(Convert.ToDateTime(Quote_Date).ToString("yyyy-MM-dd"));
                dt_PlQuoteExpiry.Date = Convert.ToDateTime(Quote_Expiry);
                //lookup_Customer.GridView.Selection.SelectRowByKey(Customer_Id);
                // CustomerComboBox.Value = Customer_Id;

                hdnCustomerId.Value = Customer_Id;
                // txtCustName.Text = Convert.ToString(QuotationEditdt.Rows[0]["Name"]);

                TabPage page = ASPxPageControl1.TabPages.FindByName("[B]illing/Shipping");
                page.ClientEnabled = true;
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Jc", "<script  language='javascript'>SettingTabStatus();</script>", true);
                PopulateContactPersonOfCustomer(Customer_Id);
                cmbContactPerson.SelectedItem = cmbContactPerson.Items.FindByValue(Contact_Person_Id);
                txt_Refference.Text = Quote_Reference;
                ddl_Branch.SelectedValue = Branch_Id;
                ddl_SalesAgent.SelectedValue = Quote_SalesmanId;
                ddl_Currency.SelectedValue = Currency_Id;
                txt_Rate.Value = Currency_Conversion_Rate;
                txt_Rate.Text = Currency_Conversion_Rate;
                if (Tax_Option != "0") ddl_AmountAre.Value = Tax_Option;
                if (Tax_Code != "0")
                {
                    //ddl_VatGstCst.Value = Tax_Code;
                    PopulateGSTCSTVAT("2");
                    setValueForHeaderGST(ddl_VatGstCst, Tax_Code);
                }
                //    ddl_AmountAre.ClientEnabled = true;
                ddl_VatGstCst.ClientEnabled = false;

                dt_PLQuote.ClientEnabled = false;
                txtCustName.ClientEnabled = false;

                //if (IsUsed == "Y")
                //{
                //    dt_PLQuote.ClientEnabled = false;
                //    lookup_Customer.ClientEnabled = false;
                //}
                //else
                //{
                //    dt_PLQuote.ClientEnabled = true;
                //    lookup_Customer.ClientEnabled = true;
                //}
            }
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
            public string Amount { get; set; }
            public string TaxAmount { get; set; }
            public string TotalAmount { get; set; }
            public string ProductName { get; set; }
            public string ComponentID { get; set; }
            public string ComponentNumber { get; set; }
            public string TotalQty { get; set; }
            public string BalanceQty { get; set; }
            public string IsComponentProduct { get; set; }
            public string ProductDisID { get; set; }
            public string Product { get; set; }
            public string DetailsId { get; set; }
            public string CloseRate { get; set; }
            public string CloseRateFlag { get; set; }
            public string Remarks { get; set; }
           
            //Mantis Issue 24831
            public string Order_AltQuantity { get; set; }
            public string Order_AltUOM { get; set; }
            // End of Mantis Issue 24831
        }
        #endregion

        #region Product Details



        #region Code Checked and Modified  By Sam on 17022016

        public IEnumerable GetProduct()
        {
            List<Product> ProductList = new List<Product>();
           // BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();

            DataTable DT = GetProductData().Tables[0];
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
        //public DataTable GetProductData()
        //{
        //    DataTable ds = new DataTable();
        //    ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
        //    proc.AddVarcharPara("@Action", 500, "ProductDetails");
        //    ds = proc.GetTable();
        //    return ds;
        //}


        public DataSet GetProductData()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_Purchasechallan_Details");
            proc.AddVarcharPara("@Action", 500, "ProductDetails");
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            proc.AddVarcharPara("@branchid", 500, Convert.ToString(Session["userbranchID"]));
            proc.AddVarcharPara("@campany_Id", 500, Convert.ToString(Session["LastCompany"]));
            ds = proc.GetDataSet();
            return ds;
        }

        public DataTable GetMultiUOMData()
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesReturn_Details");
            // Mantis Issue 24831
            //proc.AddVarcharPara("@Action", 500, "MultiUOMQuotationDetailsForManualSR");
            proc.AddVarcharPara("@Action", 500, "MultiUOMQuotationDetailsForManualSR_New");
            // End of Mantis Issue 24831
            proc.AddVarcharPara("@SalesReturnID", 500, Convert.ToString(Session["SRM_ReturnID"]));
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

        #endregion Code Checked and Modified  By Sam on 17022016


        //public DataSet GetProductData()
        //{
        //    DataSet ds = new DataSet();
        //    ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
        //    proc.AddVarcharPara("@Action", 500, "ProductDetails");
        //    ds = proc.GetDataSet();
        //    return ds;
        //}

        public DataTable GetQuotationEditData()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 500, "QuotationEditDetails");
            proc.AddVarcharPara("@QuotationID", 500, Convert.ToString(Session["QuotationID"]));
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            proc.AddVarcharPara("@branchId", 500, Convert.ToString(Session["userbranchID"]));
            proc.AddVarcharPara("@companyId", 500, Convert.ToString(Session["LastCompany"]));
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
            proc.AddVarcharPara("@QuotationID", 500, Convert.ToString(Session["QuotationID"]));
            proc.AddVarcharPara("@ProductID", 500, Convert.ToString(hdfProductID.Value));
            proc.AddVarcharPara("@WarehouseID", 500, WarehouseID);
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            proc.AddVarcharPara("@branchId", 500, Convert.ToString(Session["userbranchID"]));
            proc.AddVarcharPara("@companyId", 500, Convert.ToString(Session["LastCompany"]));
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetSerialata(string WarehouseID, string BatchID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 500, "GetSerialByProductIDWarehouseBatch");
            proc.AddVarcharPara("@QuotationID", 500, Convert.ToString(Session["QuotationID"]));
            proc.AddVarcharPara("@ProductID", 500, Convert.ToString(hdfProductID.Value));
            proc.AddVarcharPara("@BatchID", 500, BatchID);
            proc.AddVarcharPara("@WarehouseID", 500, WarehouseID);
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            proc.AddVarcharPara("@branchId", 500, Convert.ToString(Session["userbranchID"]));
            proc.AddVarcharPara("@companyId", 500, Convert.ToString(Session["LastCompany"]));
            dt = proc.GetTable();
            return dt;
        }
        public IEnumerable GetQuotation()
        {
            List<Quotation> QuotationList = new List<Quotation>();
            DataTable Quotationdt = GetQuotationData().Tables[0];

            for (int i = 0; i < Quotationdt.Rows.Count; i++)
            {
                Quotation Quotations = new Quotation();

                Quotations.SrlNo = Convert.ToString(Quotationdt.Rows[i]["SrlNo"]);
                Quotations.QuotationID = Convert.ToString(Quotationdt.Rows[i]["QuotationID"]);
                Quotations.ProductID = Convert.ToString(Quotationdt.Rows[i]["ProductID"]);
                Quotations.Description = Convert.ToString(Quotationdt.Rows[i]["Description"]);
                Quotations.Quantity = Convert.ToString(Quotationdt.Rows[i]["Quantity"]);
                Quotations.UOM = Convert.ToString(Quotationdt.Rows[i]["UOM"]);
                Quotations.Warehouse = "";
                Quotations.StockQuantity = Convert.ToString(Quotationdt.Rows[i]["StockQuantity"]);
                Quotations.StockUOM = Convert.ToString(Quotationdt.Rows[i]["StockUOM"]);
                Quotations.SalePrice = Convert.ToString(Quotationdt.Rows[i]["SalePrice"]);
                Quotations.Discount = Convert.ToString(Quotationdt.Rows[i]["Discount"]);
                Quotations.Amount = Convert.ToString(Quotationdt.Rows[i]["Amount"]);
                Quotations.TaxAmount = Convert.ToString(Quotationdt.Rows[i]["TaxAmount"]);
                Quotations.TotalAmount = Convert.ToString(Quotationdt.Rows[i]["TotalAmount"]);
                Quotations.ProductName = Convert.ToString(Quotationdt.Rows[i]["ProductName"]);
                QuotationList.Add(Quotations);
            }

            return QuotationList;
        }
        public IEnumerable GetQuotation(DataTable Quotationdt)        
        {
            List<Quotation> QuotationList = new List<Quotation>();
            DataColumnCollection dtc = Quotationdt.Columns;
            if (Quotationdt != null && Quotationdt.Rows.Count > 0)
            {
                for (int i = 0; i < Quotationdt.Rows.Count; i++)
                {
                    Quotation Quotations = new Quotation();

                    Quotations.SrlNo = Convert.ToString(Quotationdt.Rows[i]["SrlNo"]);
                    Quotations.QuotationID = Convert.ToString(Quotationdt.Rows[i]["QuotationID"]);
                    Quotations.ProductID = Convert.ToString(Quotationdt.Rows[i]["ProductID"]);
                    Quotations.Description = Convert.ToString(Quotationdt.Rows[i]["Description"]);
                    Quotations.Quantity = Convert.ToString(Quotationdt.Rows[i]["Quantity"]);
                    //  Quotations.UOM = Convert.ToString(Quotationdt.Rows[i]["UOM"]);
                    Quotations.UOM = Convert.ToString(Quotationdt.Rows[i]["StockUOM"]);
                    Quotations.Warehouse = "";
                    Quotations.StockQuantity = Convert.ToString(Quotationdt.Rows[i]["StockQuantity"]);
                    Quotations.StockUOM = Convert.ToString(Quotationdt.Rows[i]["StockUOM"]);
                    Quotations.SalePrice = Convert.ToString(Quotationdt.Rows[i]["SalePrice"]);
                    Quotations.Discount = Convert.ToString(Quotationdt.Rows[i]["Discount"]);
                    Quotations.Amount = Convert.ToString(Quotationdt.Rows[i]["Amount"]);
                    Quotations.TaxAmount = Convert.ToString(Quotationdt.Rows[i]["TaxAmount"]);
                    Quotations.TotalAmount = Convert.ToString(Quotationdt.Rows[i]["TotalAmount"]);
                    Quotations.ProductName = Convert.ToString(Quotationdt.Rows[i]["ProductName"]);
                    Quotations.ComponentID = Convert.ToString(Quotationdt.Rows[i]["ComponentID"]);
                    Quotations.ComponentNumber = Convert.ToString(Quotationdt.Rows[i]["ComponentNumber"]);
                    Quotations.TotalQty = Convert.ToString(Quotationdt.Rows[i]["TotalQty"]);
                    Quotations.BalanceQty = Convert.ToString(Quotationdt.Rows[i]["BalanceQty"]);
                    Quotations.IsComponentProduct = Convert.ToString(Quotationdt.Rows[i]["IsComponentProduct"]);
                    Quotations.ProductDisID = Convert.ToString(Quotationdt.Rows[i]["ProductDisID"]);
                    Quotations.Product = Convert.ToString(Quotationdt.Rows[i]["Product"]);
                    Quotations.DetailsId = Convert.ToString(Quotationdt.Rows[i]["DetailsId"]);
                    Quotations.CloseRate = Convert.ToString(Quotationdt.Rows[i]["CloseRate"]);
                    Quotations.CloseRateFlag = Convert.ToString(Quotationdt.Rows[i]["CloseRateFlag"]);
                    if (dtc.Contains("Remarks"))
                    {
                        Quotations.Remarks = Convert.ToString(Quotationdt.Rows[i]["Remarks"]);
                    }
                    // Mantis Issue 24831
                    Quotations.Order_AltQuantity = Convert.ToString(Quotationdt.Rows[i]["Order_AltQuantity"]);
                    Quotations.Order_AltUOM = Convert.ToString(Quotationdt.Rows[i]["Order_AltUOM"]);
                    // End of Mantis Issue 24831
                        QuotationList.Add(Quotations);
                }
            }

            return QuotationList;
        }
        public DataSet GetQuotationData()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 500, "QuotationDetails");
            proc.AddVarcharPara("@QuotationID", 500, Convert.ToString(Session["SRM_ReturnID"]));
            ds = proc.GetDataSet();
            return ds;
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
            //else if (e.Column.FieldName == "CloseRate")
            //{
            //    e.Editor.Enabled = true;
            //}
            //else if (e.Column.FieldName == "SalePrice")
            //{
            //    e.Editor.Enabled = true;
            //}
            //else if (e.Column.FieldName == "Amount")
            //{
            //    e.Editor.Enabled = true;
            //}
            //else if (e.Column.FieldName == "TotalAmount")
            //{
            //    e.Editor.Enabled = true;
            //}
            else if (e.Column.FieldName == "SrlNo")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "ProductName")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "Product")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "TaxAmount")
            {
                e.Editor.Enabled = true;
            }
            //else if (e.Column.FieldName == "Quantity")
            //{
            //    e.Editor.Enabled = true;
            //}
            // Mantis Issue 24831
            else if (hddnMultiUOMSelection.Value == "1" && e.Column.FieldName == "Quantity")
            {
                e.Editor.Enabled = true;
            }
            // End of Mantis Issue 24831
            // Mantis Issue 25377
            else if (hddnMultiUOMSelection.Value == "1" && e.Column.FieldName == "SalePrice" )
            {
                e.Editor.Enabled = true;
            }
            else if (hddnMultiUOMSelection.Value == "1" && e.Column.FieldName == "Amount" )
            {
                e.Editor.Enabled = true;
            }
            else if (hddnMultiUOMSelection.Value == "1" && e.Column.FieldName == "TotalAmount")
            {
                e.Editor.Enabled = true;
            }
            // End of Mantis Issue 25377
            else
            {
                e.Editor.ReadOnly = false;
            }
        }


        protected void EntityServerModeDataSalesReturnManual_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Proj_Id";

            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);

            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            BusinessLogicLayer.DBEngine BEngine = new BusinessLogicLayer.DBEngine();



            // Mantis Issue 24976
            //var q = from d in dc.V_ProjectLists
            //        where d.ProjectStatus == "Approved" && d.ProjBracnchid == Convert.ToInt64(ddl_Branch.SelectedValue) && d.CustId == hdnCustomerId.Value
            //        orderby d.Proj_Id descending
            //        select d;

            //e.QueryableSource = q;

            CommonBL cbl = new CommonBL();
            string ISProjectIndependentOfBranch = cbl.GetSystemSettingsResult("AllowProjectIndependentOfBranch");

            if (ISProjectIndependentOfBranch == "No")
            {
                var q = from d in dc.V_ProjectLists
                        where d.ProjectStatus == "Approved" && d.ProjBracnchid == Convert.ToInt64(ddl_Branch.SelectedValue) && d.CustId == hdnCustomerId.Value
                        orderby d.Proj_Id descending
                        select d;

                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.V_ProjectLists
                        where d.ProjectStatus == "Approved" && d.CustId == hdnCustomerId.Value
                        orderby d.Proj_Id descending
                        select d;

                e.QueryableSource = q;
            }
            // End of Mantis Issue 24976

        }

        protected void grid_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            DataTable Quotationdt = new DataTable();
            string IsDeleteFrom = Convert.ToString(hdfIsDelete.Value);

            string validate = "",SchemaID = "";
            grid.JSProperties["cpQuotationNo"] = "";
            grid.JSProperties["cpSaveSuccessOrFail"] = "";

            if (Session["SRM_QuotationDetails"] != null)
            {
                Quotationdt = (DataTable)Session["SRM_QuotationDetails"];
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
                Quotationdt.Columns.Add("ProductDisID", typeof(string));
                Quotationdt.Columns.Add("Product", typeof(string));
                Quotationdt.Columns.Add("DetailsId", typeof(string));
                Quotationdt.Columns.Add("CloseRate", typeof(string));
                Quotationdt.Columns.Add("CloseRateFlag", typeof(string));
                Quotationdt.Columns.Add("Remarks", typeof(string));
                // Mantis Issue 24831
                Quotationdt.Columns.Add("Order_AltQuantity", typeof(string));
                Quotationdt.Columns.Add("Order_AltUOM", typeof(string));
                // End of Mantis Issue 24831
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
                    string Description = ProductDetailsList[1];
                    string Quantity = Convert.ToString(args.NewValues["Quantity"]);
                    string UOM = Convert.ToString(args.NewValues["UOM"]);
                    string Warehouse = Convert.ToString(args.NewValues["Warehouse"]);
                    decimal strMultiplier = Convert.ToDecimal(ProductDetailsList[7]);
                    string StockQuantity = Convert.ToString(Convert.ToDecimal(Quantity) * strMultiplier);
                    string StockUOM = Convert.ToString(ProductDetailsList[4]);                 

                    // Mantis Issue 24831
                    string Order_AltQuantity = Convert.ToString(args.NewValues["Order_AltQuantity"]);
                    string Order_AltUOM = Convert.ToString(args.NewValues["Order_AltUOM"]);
                    // End of Mantis Issue 24831
                    string SalePrice = Convert.ToString(args.NewValues["SalePrice"]);
                    string Discount = (Convert.ToString(args.NewValues["Discount"]) != "") ? Convert.ToString(args.NewValues["Discount"]) : "0";
                    string Amount = (Convert.ToString(args.NewValues["Amount"]) != "") ? Convert.ToString(args.NewValues["Amount"]) : "0";
                    string TaxAmount = (Convert.ToString(args.NewValues["TaxAmount"]) != "") ? Convert.ToString(args.NewValues["TaxAmount"]) : "0";
                    string TotalAmount = (Convert.ToString(args.NewValues["TotalAmount"]) != "") ? Convert.ToString(args.NewValues["TotalAmount"]) : "0";
                    string ComponentID = (Convert.ToString(args.NewValues["ComponentID"]) != "") ? Convert.ToString(args.NewValues["ComponentID"]) : "0";
                    string ComponentNumber = (Convert.ToString(args.NewValues["ComponentNumber"]) != "") ? Convert.ToString(args.NewValues["ComponentNumber"]) : "0";
                    string TotalQty = (Convert.ToString(args.NewValues["TotalQty"]) != "") ? Convert.ToString(args.NewValues["TotalQty"]) : "0";
                    string BalanceQty = (Convert.ToString(args.NewValues["BalanceQty"]) != "" && Convert.ToString(args.NewValues["BalanceQty"]) != "NaN") ? Convert.ToString(args.NewValues["BalanceQty"]) : "0";
                    string IsComponentProduct = (Convert.ToString(args.NewValues["IsComponentProduct"]) != "") ? Convert.ToString(args.NewValues["IsComponentProduct"]) : "0";
                    string ProductDisID = (Convert.ToString(args.NewValues["ProductDisID"]) != "") ? Convert.ToString(args.NewValues["ProductDisID"]) : "0";
                    string Product = (Convert.ToString(args.NewValues["Product"]) != "") ? Convert.ToString(args.NewValues["Product"]) : "0";
                    string DetailsId = Convert.ToString(args.NewValues["DetailsId"]);
                    string CloseRate = Convert.ToString(args.NewValues["CloseRate"]);
                    string CloseRateFlag = (Convert.ToString(args.NewValues["CloseRateFlag"]) != "") ? Convert.ToString(args.NewValues["CloseRateFlag"]) : "0";
                    string Remarks = (Convert.ToString(args.NewValues["Remarks"]) != "") ? Convert.ToString(args.NewValues["Remarks"]) : "";
                    // Mantis Issue 24831
                    //Quotationdt.Rows.Add(SrlNo, "0", ProductDetails, Description, Quantity, UOM, Warehouse, StockQuantity, StockUOM, SalePrice, Discount, Amount, TaxAmount, TotalAmount, "I", ProductName, ComponentID, ComponentNumber, TotalQty, BalanceQty, IsComponentProduct, ProductDisID, Product, DetailsId, CloseRate, CloseRateFlag, Remarks);
                    Quotationdt.Rows.Add(SrlNo, "0", ProductDetails, Description, Quantity, UOM, Warehouse, StockQuantity, StockUOM, SalePrice, Discount, Amount, TaxAmount, TotalAmount, "I", ProductName, ComponentID, ComponentNumber, TotalQty, BalanceQty, IsComponentProduct, ProductDisID, Product, DetailsId, CloseRate, CloseRateFlag, Remarks, Order_AltQuantity, Order_AltUOM);
                    // End of Mantis Issue 24831
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
                        string Description = Convert.ToString(ProductDetailsList[1]);
                        string Quantity = Convert.ToString(args.NewValues["Quantity"]);
                        string UOM = Convert.ToString(args.NewValues["UOM"]);
                        string Warehouse = Convert.ToString(args.NewValues["Warehouse"]);
                        decimal strMultiplier = Convert.ToDecimal(ProductDetailsList[7]);
                        string StockQuantity = Convert.ToString(Convert.ToDecimal(Quantity) * strMultiplier);
                        string StockUOM = Convert.ToString(ProductDetailsList[4]);

                        // Rev  Sanchita
                        string Order_AltQuantity = Convert.ToString(args.NewValues["Order_AltQuantity"]);
                        string Order_AltUOM = Convert.ToString(args.NewValues["Order_AltUOM"]);
                        // End of Mantis Issue 24831

                        string SalePrice = Convert.ToString(args.NewValues["SalePrice"]);
                        string Discount = (Convert.ToString(args.NewValues["Discount"]) != "") ? Convert.ToString(args.NewValues["Discount"]) : "0";
                        string Amount = (Convert.ToString(args.NewValues["Amount"]) != "") ? Convert.ToString(args.NewValues["Amount"]) : "0";
                        string TaxAmount = (Convert.ToString(args.NewValues["TaxAmount"]) != "") ? Convert.ToString(args.NewValues["TaxAmount"]) : "0";
                        string TotalAmount = (Convert.ToString(args.NewValues["TotalAmount"]) != "") ? Convert.ToString(args.NewValues["TotalAmount"]) : "0";
                        string ComponentID = (Convert.ToString(args.NewValues["ComponentID"]) != "") ? Convert.ToString(args.NewValues["ComponentID"]) : "0";
                        string ComponentNumber = (Convert.ToString(args.NewValues["ComponentNumber"]) != "") ? Convert.ToString(args.NewValues["ComponentNumber"]) : "0";
                        string TotalQty = (Convert.ToString(args.NewValues["TotalQty"]) != "") ? Convert.ToString(args.NewValues["TotalQty"]) : "0";
                        string BalanceQty = (Convert.ToString(args.NewValues["BalanceQty"]) != "" && Convert.ToString(args.NewValues["BalanceQty"]) != "NAN") ? Convert.ToString(args.NewValues["BalanceQty"]) : "0";
                        string IsComponentProduct = (Convert.ToString(args.NewValues["IsComponentProduct"]) != "") ? Convert.ToString(args.NewValues["IsComponentProduct"]) : "N";
                        string ProductDisID = (Convert.ToString(args.NewValues["ProductDisID"]) != "") ? Convert.ToString(args.NewValues["ProductDisID"]) : "0";
                        string DetailsId = Convert.ToString(args.NewValues["DetailsId"]);
                        string CloseRate = Convert.ToString(args.NewValues["CloseRate"]);
                        string[] ProductDList = ProductDisID.Split(new string[] { "||@||" }, StringSplitOptions.None);
                        ProductDisID = Convert.ToString(ProductDList[0]);
                        string CloseRateFlag = (Convert.ToString(args.NewValues["CloseRateFlag"]) != "") ? Convert.ToString(args.NewValues["CloseRateFlag"]) : "0";
                        string Remarks = (Convert.ToString(args.NewValues["Remarks"]) != "") ? Convert.ToString(args.NewValues["Remarks"]) : "";

                        string Product = (Convert.ToString(args.NewValues["Product"]) != "") ? Convert.ToString(args.NewValues["Product"]) : "N";
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
                                drr["Amount"] = Amount;
                                drr["TaxAmount"] = TaxAmount;
                                drr["TotalAmount"] = TotalAmount;
                                drr["Status"] = "U";
                                drr["ProductName"] = ProductName;
                                drr["ComponentID"] = ComponentID;
                                drr["ComponentNumber"] = ComponentNumber;
                                drr["TotalQty"] = TotalQty;
                                drr["BalanceQty"] = BalanceQty;
                                drr["IsComponentProduct"] = IsComponentProduct;
                                drr["ProductDisID"] = ProductDisID;
                                drr["Product"] = Product;
                                drr["DetailsId"] = DetailsId;
                                drr["CloseRate"] = CloseRate;
                                drr["CloseRateFlag"] = CloseRateFlag;
                                drr["Remarks"] = Remarks;
                                // Mantis Issue 24831
                                drr["Order_AltQuantity"] = Order_AltQuantity;
                                drr["Order_AltUOM"] = Order_AltUOM;
                                // End of Mantis Issue 24831
                                break;
                            }
                        }

                        if (Isexists == false)
                        {
                            // Mantis Issue 24831
                            //Quotationdt.Rows.Add(SrlNo, QuotationID, ProductDetails, Description, Quantity, UOM, Warehouse, StockQuantity, StockUOM, SalePrice, Discount, Amount, TaxAmount, TotalAmount, "U", ProductName, ComponentID, ComponentNumber, TotalQty, BalanceQty, IsComponentProduct, ProductDisID, Product, DetailsId, CloseRate, CloseRateFlag, Remarks);
                            Quotationdt.Rows.Add(SrlNo, QuotationID, ProductDetails, Description, Quantity, UOM, Warehouse, StockQuantity, StockUOM, SalePrice, Discount, Amount, TaxAmount, TotalAmount, "U", ProductName, ComponentID, ComponentNumber, TotalQty, BalanceQty, IsComponentProduct, ProductDisID, Product, DetailsId, CloseRate, CloseRateFlag, Remarks, Order_AltQuantity, Order_AltUOM);
                            // End of Mantis Issue 24831
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
                DeleteTaxDetails(SrlNo);
                if (QuotationID.Contains("~") != true)
                {
                    Quotationdt.Rows.Add("0", QuotationID, "0", "", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "D", "", "0", "0", "0", "0", "0", "0", "0","0","0","0","");
                }
            }           

            string strDeleteSrlNo = Convert.ToString(hdnDeleteSrlNo.Value);
            if (strDeleteSrlNo != "" && strDeleteSrlNo != "0")
            {                
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
            Session["SRM_QuotationDetails"] = Quotationdt;         


            if (IsDeleteFrom != "D")
            {
                string InvoiceComponentDate = string.Empty, InvoiceComponent = "";
                string ActionType = Convert.ToString(Session["SRM_ActionType"]);
                string MainInvoiceID = Convert.ToString(Session["SRM_ReturnID"]);
                string strSchemeType = Convert.ToString(ddl_numberingScheme.SelectedValue);
                string strInvoiceNo = Convert.ToString(txt_PLQuoteNo.Text);
                string strInvoiceDate = Convert.ToString(dt_PLQuote.Date);
                string strBranch = Convert.ToString(ddl_Branch.SelectedValue);
                string strCustomer = Convert.ToString(hdfLookupCustomer.Value);
                string strContactName = Convert.ToString(cmbContactPerson.Value);
                string Reference = Convert.ToString(txt_Refference.Text);
                string PosForGst = Convert.ToString(ddlPosGstReturnManual.Value);
                string strAgents = Convert.ToString(ddl_SalesAgent.SelectedValue);


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
                string strDueDate = Convert.ToString(dt_SaleInvoiceDue.Date);
                string strCurrency = Convert.ToString(ddl_Currency.SelectedValue);
                string strRate = Convert.ToString(txt_Rate.Value);
                string strTaxType = Convert.ToString(ddl_AmountAre.Value);
                string strTaxCode = Convert.ToString(ddl_VatGstCst.Value).Split('~')[0];
                string CompID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string currency = Convert.ToString(HttpContext.Current.Session["LocalCurrency"]);
                string[] ActCurrency = currency.Split('~');
                int BaseCurrencyId = Convert.ToInt32(ActCurrency[0]);
                int ConvertedCurrencyId = Convert.ToInt32(strCurrency);
                string ReasonforReturn = Convert.ToString(txtReasonforChange.Text);
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

                DataTable TaxDetailTable = new DataTable();
                if (Session["SRM_FinalTaxRecord"] != null)
                {
                    TaxDetailTable = (DataTable)Session["SRM_FinalTaxRecord"];
                }
                // DataTable of Warehouse

                DataTable tempWarehousedt = new DataTable();
                if (Session["SRM_WarehouseData"] != null)
                {
                    DataTable Warehousedt = (DataTable)Session["SRM_WarehouseData"];
                    tempWarehousedt = Warehousedt.DefaultView.ToTable();
                    // tempWarehousedt = Warehousedt.DefaultView.ToTable(false, "LoopID", "WarehouseID", "TotalQuantity", "BatchID", "SerialID");
                }
                else
                {
                    tempWarehousedt.Columns.Add("SrlNo", typeof(string));
                    tempWarehousedt.Columns.Add("WarehouseID", typeof(string));
                    tempWarehousedt.Columns.Add("TotalQuantity", typeof(string));
                    tempWarehousedt.Columns.Add("BatchID", typeof(string));
                    tempWarehousedt.Columns.Add("SerialID", typeof(string));
                }

                // End
                //datatable for MultiUOm start chinmoy 14-01-2020
                DataTable MultiUOMDetails = new DataTable();
                if (Session["MultiUOMDataRETM"] != null)
                {
                    DataTable MultiUOM = (DataTable)Session["MultiUOMDataRETM"];
                    // Mantis Issue 24831
                    //MultiUOMDetails = MultiUOM.DefaultView.ToTable(false, "SrlNo", "Quantity", "UOM", "AltUOM", "AltQuantity", "UomId", "AltUomId", "ProductId", "DetailsId");
                    MultiUOMDetails = MultiUOM.DefaultView.ToTable(false, "SrlNo", "Quantity", "UOM", "AltUOM", "AltQuantity", "UomId", "AltUomId", "ProductId", "DetailsId", "BaseRate", "AltRate", "UpdateRow");
                    // End of Mantis Issue 24831  
                }
                else
                {
                    MultiUOMDetails.Columns.Add("SrlNo", typeof(string));
                    MultiUOMDetails.Columns.Add("Quantity", typeof(Decimal));
                    MultiUOMDetails.Columns.Add("UOM", typeof(string));
                    MultiUOMDetails.Columns.Add("AltUOM", typeof(string));
                    MultiUOMDetails.Columns.Add("AltQuantity", typeof(Decimal));
                    MultiUOMDetails.Columns.Add("UomId", typeof(Int64));
                    MultiUOMDetails.Columns.Add("AltUomId", typeof(Int64));
                    MultiUOMDetails.Columns.Add("ProductId", typeof(Int64));
                    MultiUOMDetails.Columns.Add("DetailsId", typeof(string));

                    // Mantis Issue 24831
                    MultiUOMDetails.Columns.Add("BaseRate", typeof(Decimal));
                    MultiUOMDetails.Columns.Add("AltRate", typeof(Decimal));
                    MultiUOMDetails.Columns.Add("UpdateRow", typeof(string));
                    // End of Mantis Issue 24831
                }


                //End


                // DataTable Of Quotation Tax Details 

                DataTable TaxDetailsdt = new DataTable();
                if (Session["SRM_TaxDetails"] != null)
                {
                    TaxDetailsdt = (DataTable)Session["SRM_TaxDetails"];
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

                DataTable tempBillAddress = new DataTable();                
                tempBillAddress = Purchase_BillingShipping.GetBillingShippingTable();           

                string approveStatus = "";
                if (Request.QueryString["status"] != null)
                {
                    approveStatus = Convert.ToString(Request.QueryString["status"]);
                }                
                if (tempBillAddress != null && tempBillAddress.Rows.Count == 0)
                {
                    validate = "checkAddress";
                }
                foreach (DataRow dr in tempQuotation.Rows)
                {
                    string strSrlNo = Convert.ToString(dr["SrlNo"]);
                    decimal strProductQuantity = Convert.ToDecimal(dr["Quantity"]);
                    decimal strWarehouseQuantity = 0;                  

                    if (strWarehouseQuantity != 0)
                    {
                        if (strProductQuantity != strWarehouseQuantity)
                        {
                            validate = "checkWarehouse";
                            grid.JSProperties["cpProductSrlIDCheck"] = strSrlNo;
                            break;
                        }
                    }
                }

                if (hddnMultiUOMSelection.Value == "1")
                {
                    foreach (DataRow dr in tempQuotation.Rows)
                    {
                        string strSrlNo = Convert.ToString(dr["SrlNo"]);
                        string strDetailsId = Convert.ToString(dr["DetailsId"]);
                        string StockUOM = Convert.ToString(dr["StockUOM"]);
                        decimal strProductQuantity = Convert.ToDecimal(dr["Quantity"]);
                        decimal strUOMQuantity = 0;

                        if (StockUOM != "0")
                        {
                            GetQuantityBaseOnProductforDetailsId(strSrlNo, ref strUOMQuantity);
                            if (Session["MultiUOMDataRETM"] != null)
                            {
                               // Mantis Issue 24831
                                //if (strUOMQuantity != null)
                                //{
                                //    if (strProductQuantity != strUOMQuantity)
                                //    {
                                //        validate = "checkMultiUOMData";
                                //        grid.JSProperties["cpcheckMultiUOMData"] = strSrlNo;
                                //        break;
                                //    }
                                //}
                                // End of Mantis Issue 24831
                            }
                            else if (Session["MultiUOMDataRETM"] == null)
                            {
                                validate = "checkMultiUOMData";
                                grid.JSProperties["cpcheckMultiUOMData"] = strSrlNo;
                                break;
                            }
                        }
                    }
             }
                var duplicateRecords = tempQuotation.AsEnumerable()
               .GroupBy(r => r["ProductID"]) //coloumn name which has the duplicate values
               .Where(gr => gr.Count() > 1)
               .Select(g => g.Key);

                //foreach (var d in duplicateRecords)
                //{
                //    validate = "duplicateProduct";
                //}
                // Mantis Issue 25118
                if (hddnMultiUOMSelection.Value == "1")
                {
                    foreach (var d in duplicateRecords)
                    {
                        validate = "duplicateProduct";
                    }
                }
                // End of Mantis Issue 25118

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
                decimal ProductAmount = 0;
                foreach (DataRow dr in tempQuotation.Rows)
                {
                    ProductAmount = ProductAmount + Convert.ToDecimal(dr["Amount"]);
                }
                if (ProductAmount == 0)
                {
                    validate = "nullAmount";
                }
                if (PosForGst=="")
                {
                    validate = "EmptyPlaceOfsupply";
                }
                string SupplyState = "";
                if (ddlPosGstReturnManual.Value.ToString() == "S")
                {
                    SupplyState = hdnPOSShippingStateCode.Value;
                }
                else
                {
                    SupplyState = hdnPOSBillingStateCode.Value;
                }
                if (TaxDetailTable.Rows.Count<0 || TaxDetailTable == null)
                {
                    string TaxType = "", ShippingState = "";

                    if (ddlPosGstReturnManual.Value.ToString() == "S")
                    {
                        ShippingState = hdnPOSShippingStateCode.Value;
                    }
                    else
                    {
                        ShippingState = hdnPOSBillingStateCode.Value;
                    }

                    DataTable DTSTATEID = oDBEngine.GetDataTable("SELECT TOP 1 ID FROM TBL_MASTER_STATE WHERE STATECODE='" + ShippingState + "'");
                    ShippingState = Convert.ToString(DTSTATEID.Rows[0]["ID"]);

                    if (ddl_AmountAre.Value.ToString() == "1") TaxType = "E";
                    else if (ddl_AmountAre.Value.ToString() == "2") TaxType = "I";

                    TaxDetailTable = gstTaxDetails.SetTaxTableDataWithProductSerialRoundOff(ref tempQuotation, "SrlNo", "ProductID", "Amount", "TaxAmount", TaxDetailTable, "S", dt_PLQuote.Date.ToString("yyyy-MM-dd"), strBranch, ShippingState, TaxType);
                }

                CommonBL ComBL = new CommonBL();
                string GSTRateTaxMasterMandatory = ComBL.GetSystemSettingsResult("GSTRateTaxMasterMandatory");
                if (!String.IsNullOrEmpty(GSTRateTaxMasterMandatory))
                {
                    if (GSTRateTaxMasterMandatory == "Yes")
                    {


                        DataTable dtTaxDetails = new DataTable();
                        ProcedureExecute procT = new ProcedureExecute("prc_CRMSalesReturn_Details");
                        procT.AddVarcharPara("@Action", 500, "GetTaxDetailsByProductIDForSRM");
                        procT.AddPara("@SRMProductDetails", tempQuotation);
                        procT.AddVarcharPara("@TaxOption", 10, Convert.ToString(strTaxType));
                        procT.AddVarcharPara("@SupplyState", 10, SupplyState);
                        procT.AddIntegerPara("@branchId", Convert.ToInt32(strBranch));
                        procT.AddVarcharPara("@COMPANYID", 500, Convert.ToString(Session["LastCompany"]));
                        procT.AddVarcharPara("@ENTITY_ID", 100, Convert.ToString(strCustomer));
                        procT.AddVarcharPara("@TaxDATE", 100, Convert.ToString(dt_PLQuote.Date.ToString("yyyy-MM-dd")));
                        dtTaxDetails = procT.GetTable();

                        if (dtTaxDetails != null && dtTaxDetails.Rows.Count > 0)
                        {

                            foreach (DataRow dr in dtTaxDetails.Rows)
                            {
                                string SerialID = Convert.ToString(dr["SrlNo"]);
                                string TaxID = Convert.ToString(dr["TaxCode"]);
                                Double d = Convert.ToDouble(dr["TaxAmount"]);
                                Math.Round(d);
                                decimal _TaxAmount = Math.Round(Convert.ToDecimal(dr["TaxAmount"]), 2);
                                string ProductName = Convert.ToString(dr["ProductName"]);
                                DataRow[] rows = TaxDetailTable.Select("SlNo = '" + SerialID + "' and TaxCode='" + TaxID + "'");

                                if (rows != null && rows.Length > 0)
                                {

                                    decimal EntryTaxAmount = Math.Round(Convert.ToDecimal(rows[0]["Amount"]), 2);

                                    //decimal EntryTaxAmount = Math.Round(Convert.ToDecimal(rows[0]["Amount"]), 2);


                                    if (EntryTaxAmount != _TaxAmount)
                                    {
                                        validate = "checkAcurateTaxAmount";
                                        grid.JSProperties["cpSerialNo"] = SerialID;
                                        grid.JSProperties["cpProductName"] = ProductName;
                                        break;
                                    }


                                }

                            }
                        }
                    }
                }


                if (validate == "outrange" || validate == "duplicate" || validate == "checkWarehouse" || validate == "duplicateProduct" || validate == "nullAmount" || validate == "checkAddress" || validate == "EmptyPlaceOfsupply" || validate == "checkMultiUOMData" || validate == "checkAcurateTaxAmount")
                {
                    grid.JSProperties["cpSaveSuccessOrFail"] = validate;
                }
                else
                {
                    grid.JSProperties["cpQuotationNo"] = UniqueQuotation;
                    #region To Show By Default Cursor after SAVE AND NEW
                    if (Convert.ToString(Session["SRM_ActionType"]) == "Add") // session has been removed from quotation list page working good
                    {
                        string[] schemaid = new string[] { };
                        string schemavalue = ddl_numberingScheme.SelectedValue;
                        Session["SRM_schemavalue"] = schemavalue;        // session has been removed from quotation list page working good
                        schemaid = ddl_numberingScheme.SelectedValue.Split('~');

                        string schematype = schemaid[1];
                        if (hdnRefreshType.Value == "N")
                        {
                            if (schematype == "1")
                            {
                                Session["SRM_SaveMode"] = "A";
                            }
                            else
                            {
                                Session["SRM_SaveMode"] = "M";
                            }
                        }
                    }
                    #endregion

                    #region Subhabrata Section Start
                    int strIsComplete = 0, strQuoteID = 0;                    
                    string TaxType = "", ShippingState = "";                  

                    if (ddlPosGstReturnManual.Value.ToString() == "S")
                    {                       
                        ShippingState = hdnPOSShippingStateCode.Value;
                    }
                    else
                    {                       
                        ShippingState = hdnPOSBillingStateCode.Value;
                    }

                    DataTable DTSTATEID = oDBEngine.GetDataTable("SELECT TOP 1 ID FROM TBL_MASTER_STATE WHERE STATECODE='" + ShippingState + "'");
                    ShippingState = Convert.ToString(DTSTATEID.Rows[0]["ID"]);

                    if (ddl_AmountAre.Value.ToString() == "1") TaxType = "E";
                    else if (ddl_AmountAre.Value.ToString() == "2") TaxType = "I";

                    TaxDetailTable = gstTaxDetails.SetTaxTableDataWithProductSerialRoundOff(ref tempQuotation, "SrlNo", "ProductID", "Amount", "TaxAmount", TaxDetailTable, "S", dt_PLQuote.Date.ToString("yyyy-MM-dd"), strBranch, ShippingState, TaxType);

                    #region Add New Filed To Check from Table - Surojit

                    DataTable duplicatedt2 = new DataTable();
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
                    HttpContext.Current.Session["SessionPackingDetails"] = null;
                    #endregion

                    CommonBL cbl = new CommonBL();
                    string ProjectSelectcashbankModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");
                    Int64 ProjId = 0;
                    if (lookup_Project.Text != "")
                    {
                        string projectCode = lookup_Project.Text;
                        DataTable dtSlOrd = GetSalesReturnManualProjectCode(projectCode);                       
                        ProjId = Convert.ToInt64(dtSlOrd.Rows[0]["Proj_Id"]);
                    }
                    else if (lookup_Project.Text == "")
                    {
                        ProjId = 0;
                    }

                    else
                    {
                        ProjId = 0;
                    }
                    if (ActionType == "Add")
                    {
                        string[] SchemeList = strSchemeType.Split(new string[] { "~" }, StringSplitOptions.None);
                        if (SchemeList[0] != "")
                        {
                            SchemaID = Convert.ToString(SchemeList[0]);
                        }
                    }
                    else
                    {
                        SchemaID = "";
                    }
                    

                    //ModifySalesReturn(MainInvoiceID, strSchemeType, UniqueQuotation, strInvoiceDate, strCustomer, strContactName,ProjId,
                    //                Reference, PosForGst, strBranch, strAgents, strCurrency, strRate, strTaxType, strTaxCode,
                    //                InvoiceComponent, InvoiceComponentDate, strCashBank, strDueDate,
                    //                tempQuotation, TaxDetailTable, tempWarehousedt, tempTaxDetailsdt, tempBillAddress,
                    //                approveStatus, ActionType, ref strIsComplete, ref strQuoteID, ReasonforReturn, duplicatedt2, MultiUOMDetails);

                    DataTable dtAddlDesc = (DataTable)Session["InlineRemarks"];
                    ModifySalesReturn(MainInvoiceID, strSchemeType, SchemaID, txt_PLQuoteNo.Text, strInvoiceDate, strCustomer, strContactName, ProjId,
                                    Reference, PosForGst, strBranch, strAgents, strCurrency, strRate, strTaxType, strTaxCode,dtAddlDesc,
                                    InvoiceComponent, InvoiceComponentDate, strCashBank, strDueDate,
                                    tempQuotation, TaxDetailTable, tempWarehousedt, tempTaxDetailsdt, tempBillAddress,
                                    approveStatus, ActionType, ref strIsComplete, ref strQuoteID, ReasonforReturn, duplicatedt2, MultiUOMDetails, drdTransCategory.SelectedValue);


                    //####### Coded By Samrat Roy For Custom Control Data Process #########

                    if (!string.IsNullOrEmpty(hfControlData.Value))
                    {
                        CommonBL objCommonBL = new CommonBL();
                        objCommonBL.InsertTransporterControlDetails(Convert.ToInt32(strQuoteID), "SR", hfControlData.Value, Convert.ToString(HttpContext.Current.Session["userid"]));
                    }
                    //Udf Add mode
                    DataTable udfTable = (DataTable)Session["UdfDataOnAdd"];
                    if (udfTable != null)
                        Session["UdfDataOnAdd"] = reCat.insertRemarksCategoryAddMode("SR", "SaleReturn" + Convert.ToString(strQuoteID), udfTable, Convert.ToString(Session["userid"]));


                    if (strIsComplete == 1)
                    {

                        DataTable dts = oDBEngine.GetDataTable("select ISNULL(CONVERT (Decimal(18,2) , Return_TotalAmount),0) as TotalAmt,ISNULL(Irn,'') IRN from tbl_trans_SalesReturn where Return_Id=" + Convert.ToInt64(strQuoteID) + "");

                        if (dts != null && dts.Rows.Count > 0)
                        {
                            grid.JSProperties["cpToalAmountDEt"] = Convert.ToString(dts.Rows[0]["TotalAmt"]); ;
                            grid.JSProperties["cpIRN"] = Convert.ToString(dts.Rows[0]["IRN"]);
                        }
                        grid.JSProperties["cpRecetId"] = Convert.ToInt32(strQuoteID);
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
                    }
                    else if (strIsComplete == -9)
                    {
                        DataTable dts = new DataTable();
                        dts = GetAddLockStatus();
                        grid.JSProperties["cpSaveSuccessOrFail"] = "AddLock";
                        grid.JSProperties["cpAddLockStatus"] = (Convert.ToString(dts.Rows[0]["Lock_Fromdate"]) + " to " + Convert.ToString(dts.Rows[0]["Lock_Todate"]));
                    }
                    else if (strIsComplete == 2)
                    {
                        grid.JSProperties["cpSaveSuccessOrFail"] = "quantityTagged";
                    }
                    else if (strIsComplete == -12)
                    {
                        grid.JSProperties["cpSaveSuccessOrFail"] = "EmptyProject";
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
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesReturn_Details");
            proc.AddVarcharPara("@Action", 500, "GetAddLockForSalesREturnManual");

            dt = proc.GetTable();
            return dt;

        }
        protected void grid_DataBinding(object sender, EventArgs e)
        {


            if (Session["SRM_QuotationDetails"] != null)
            {
                DataTable Quotationdt = (DataTable)Session["SRM_QuotationDetails"];

                if (Quotationdt.Rows.Count > 0)
                {
                    DataView dvData = new DataView(Quotationdt);
                    dvData.RowFilter = "Status <> 'D'";
                    grid.DataSource = GetQuotation(dvData.ToTable());
                }
                else
                {
                    grid.DataSource = null;
                    // grid.DataBind();
                }

            }
            else
            {
                grid.DataSource = null;
                // grid.DataBind();
            }
        }


        protected void MultiUOM_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string SpltCmmd = e.Parameters.Split('~')[0];

            grid_MultiUOM.JSProperties["cpDuplicateAltUOM"] = "";
            grid_MultiUOM.JSProperties["cpOpenFocus"] = "";
            // Mantis Issue 24831
            grid_MultiUOM.JSProperties["cpSetBaseQtyRateInGrid"] = "";
            // End of Mantis Issue 24831

            if (SpltCmmd == "MultiUOMDisPlay")
            {
                
                DataTable MultiUOMData = new DataTable();

                if (Session["MultiUOMDataRETM"] != null)
                {
                    MultiUOMData = (DataTable)Session["MultiUOMDataRETM"];
                }
                else
                {
                    MultiUOMData.Columns.Add("SrlNo", typeof(string));
                    MultiUOMData.Columns.Add("Quantity", typeof(Decimal));
                    MultiUOMData.Columns.Add("UOM", typeof(string));
                    MultiUOMData.Columns.Add("AltUOM", typeof(string));
                    MultiUOMData.Columns.Add("AltQuantity", typeof(Decimal));
                    MultiUOMData.Columns.Add("UomId", typeof(Int64));
                    MultiUOMData.Columns.Add("AltUomId", typeof(Int64));
                    MultiUOMData.Columns.Add("ProductId", typeof(Int64));
                    MultiUOMData.Columns.Add("DetailsId", typeof(string));

                }
                if (MultiUOMData != null && MultiUOMData.Rows.Count > 0)
                {
                    string SrlNo = e.Parameters.Split('~')[1];
                    string DetailsId = e.Parameters.Split('~')[2];


                    DataView dvData = new DataView(MultiUOMData);
                    //dvData.RowFilter = "Product_SrlNo = '" + SerialID + "'";
                    // Mantis Issue 24831
                    //if (DetailsId != "" && DetailsId != null && DetailsId != "null")
                    //{
                    //    //dvData.RowFilter = "SrlNo = '" + SrlNo + "' and Doc_DetailsId='" + DetailsId + "'";
                    //    dvData.RowFilter = "DetailsId='" + DetailsId + "'";
                    //}
                    //else
                    //{
                    //    dvData.RowFilter = "SrlNo = '" + SrlNo + "'";
                    //}
                    dvData.RowFilter = "SrlNo = '" + SrlNo + "'";
                    // End of Mantis Issue 24831
                    grid_MultiUOM.DataSource = dvData;
                    grid_MultiUOM.DataBind();
                }
                else
                {
                    grid_MultiUOM.DataSource = MultiUOMData.DefaultView;
                    grid_MultiUOM.DataBind();
                }
                grid_MultiUOM.JSProperties["cpOpenFocus"] = "OpenFocus";
            }

            else if (SpltCmmd == "SaveDisplay")
            {

                string Validcheck = "";
                DataTable MultiUOMSaveData = new DataTable();
                // Mantis Issue 24831
                int MultiUOMSR = 1;
                //End of Mantis Issue 24831

                string SrlNo = Convert.ToString(e.Parameters.Split('~')[1]);
                string Quantity = Convert.ToString(e.Parameters.Split('~')[2]);
                string UOM = Convert.ToString(e.Parameters.Split('~')[3]);
                string AltUOM = Convert.ToString(e.Parameters.Split('~')[4]);
                string AltQuantity = Convert.ToString(e.Parameters.Split('~')[5]);
                string UomId = Convert.ToString(e.Parameters.Split('~')[6]);
                string AltUomId = Convert.ToString(e.Parameters.Split('~')[7]);
                string ProductId = Convert.ToString(e.Parameters.Split('~')[8]);
                string DetailsId = Convert.ToString(e.Parameters.Split('~')[9]);

                // Mantis Issue 24831
                string BaseRate = Convert.ToString(e.Parameters.Split('~')[10]);
                string AltRate = Convert.ToString(e.Parameters.Split('~')[11]);
                string UpdateRow = Convert.ToString(e.Parameters.Split('~')[12]);
                // End of Mantis Issue 24831

                DataTable allMultidataDetails = (DataTable)Session["MultiUOMDataRETM"];



                DataRow[] MultiUoMresult;

                if (allMultidataDetails != null && allMultidataDetails.Rows.Count > 0)
                {

                    // Mantis Issue 24831
                    //if (DetailsId != "" && DetailsId != null && DetailsId != "null")
                    //{
                    //    //MultiUoMresult = allMultidataDetails.Select("SrlNo ='" + SrlNo + "' and Doc_DetailsId='" + DetailsId + "'");
                    //    MultiUoMresult = allMultidataDetails.Select("DetailsId='" + DetailsId + "'");
                    //}
                    //else
                    //{
                    //    MultiUoMresult = allMultidataDetails.Select("SrlNo ='" + SrlNo + "'");
                    //}
                    MultiUoMresult = allMultidataDetails.Select("SrlNo ='" + SrlNo + "'");
                    // End of Mantis Issue 24831
                    foreach (DataRow item in MultiUoMresult)
                    {
                        if ((AltUomId == item["AltUomId"].ToString()) || (UomId == item["AltUomId"].ToString()))
                        {
                            if (AltQuantity == item["AltQuantity"].ToString())
                            {
                                Validcheck = "DuplicateUOM";
                                grid_MultiUOM.JSProperties["cpDuplicateAltUOM"] = "DuplicateAltUOM";
                                break;
                            }
                        }
                        // Mantis Issue 24831  [ if "Update Row" checkbox is checked, then all existing Update Row in the grid will be set to False]
                        if (UpdateRow == "True")
                        {
                            item["UpdateRow"] = "False";
                        }
                        // End of Mantis Issue 24831
                    }
                }

                if (Validcheck != "DuplicateUOM")
                {
                    if (Session["MultiUOMDataRETM"] != null)
                    {

                        MultiUOMSaveData = (DataTable)Session["MultiUOMDataRETM"];

                    }
                    else
                    {
                        MultiUOMSaveData.Columns.Add("SrlNo", typeof(string));
                        MultiUOMSaveData.Columns.Add("Quantity", typeof(Decimal));
                        MultiUOMSaveData.Columns.Add("UOM", typeof(string));
                        MultiUOMSaveData.Columns.Add("AltUOM", typeof(string));
                        MultiUOMSaveData.Columns.Add("AltQuantity", typeof(Decimal));
                        MultiUOMSaveData.Columns.Add("UomId", typeof(Int64));
                        MultiUOMSaveData.Columns.Add("AltUomId", typeof(Int64));
                        MultiUOMSaveData.Columns.Add("ProductId", typeof(Int64));
                        MultiUOMSaveData.Columns.Add("DetailsId", typeof(string));

                        // Mantis Issue 24428
                        MultiUOMSaveData.Columns.Add("BaseRate", typeof(Decimal));
                        MultiUOMSaveData.Columns.Add("AltRate", typeof(Decimal));
                        MultiUOMSaveData.Columns.Add("UpdateRow", typeof(string));
                        MultiUOMSaveData.Columns.Add("MultiUOMSR", typeof(int));
                        // End of Mantis Issue 24428
                    }

                    // Mantis Issue 24831
                    //if (DetailsId != "" && DetailsId != null && DetailsId != "null")
                    //{
                    //    MultiUOMSaveData.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId, DetailsId);
                    //}
                    //else
                    //{
                    //    MultiUOMSaveData.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId, DetailsId);
                    //}
                    DataRow thisRow;
                    if (MultiUOMSaveData.Rows.Count > 0)
                    {
                        MultiUOMSR = Convert.ToInt32(MultiUOMSaveData.Compute("max([MultiUOMSR])", string.Empty)) + 1;
                        MultiUOMSaveData.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId, DetailsId, BaseRate, AltRate, UpdateRow, MultiUOMSR);
                    }
                    else
                    {
                        MultiUOMSaveData.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId, DetailsId, BaseRate, AltRate, UpdateRow, MultiUOMSR);
                    }
                    // End of Mantis Issue 24831
                    MultiUOMSaveData.AcceptChanges();
                    Session["MultiUOMDataRETM"] = MultiUOMSaveData;

                    if (MultiUOMSaveData != null && MultiUOMSaveData.Rows.Count > 0)
                    {
                        DataView dvData = new DataView(MultiUOMSaveData);
                        // Mantis Issue 24831
                        //if (DetailsId != "" && DetailsId != null && DetailsId != "null" && DetailsId != "0")
                        //{
                        //    //dvData.RowFilter = "SrlNo = '" + SrlNo + "' and Doc_DetailsId='" + DetailsId + "'";
                        //    dvData.RowFilter = "DetailsId='" + DetailsId + "'";
                        //}
                        //else
                        //{
                        //    dvData.RowFilter = "SrlNo = '" + SrlNo + "'";
                        //}
                        dvData.RowFilter = "SrlNo = '" + SrlNo + "'";
                        // End of Mantis Issue 24831
                        grid_MultiUOM.DataSource = dvData;
                        grid_MultiUOM.DataBind();
                    }
                    else
                    {
                        //MultiUOMSaveData.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId);
                        //Session["MultiUOMDataRETM"] = MultiUOMSaveData;
                        grid_MultiUOM.DataSource = MultiUOMSaveData.DefaultView;
                        grid_MultiUOM.DataBind();
                    }
                }
            }

            else if (SpltCmmd == "MultiUomDelete")
            {
                string AltUOMKeyValuewithqnty = e.Parameters.Split('~')[1];
                string AltUOMKeyValue = AltUOMKeyValuewithqnty.Split('|')[0];
                string AltUOMKeyqnty = AltUOMKeyValuewithqnty.Split('|')[1];

                string SrlNo = Convert.ToString(e.Parameters.Split('~')[2]);
                string DetailsId = Convert.ToString(e.Parameters.Split('~')[3]);

                DataRow[] MultiUoMresult;
                DataTable dt = (DataTable)Session["MultiUOMDataRETM"];

                if (dt != null && dt.Rows.Count > 0)
                {
                    // Mantis Issue 24831
                    //if (DetailsId != "" && DetailsId != null && DetailsId != "null")
                    //{
                    //    //MultiUoMresult = dt.Select("SrlNo ='" + SrlNo + "' and Doc_DetailsId='" + DetailsId + "'");
                    //    MultiUoMresult = dt.Select("DetailsId='" + DetailsId + "'");
                    //}
                    //else
                    //{
                    //    MultiUoMresult = dt.Select("SrlNo ='" + SrlNo + "'");
                    //}
                    MultiUoMresult = dt.Select("SrlNo ='" + SrlNo + "'");
                    // End of Mantis Issue 24831

                    foreach (DataRow item in MultiUoMresult)
                    {
                        if (AltUOMKeyValue.ToString() == item["AltUomId"].ToString())
                        {
                            //dt.Rows.Remove(item);
                            if (AltUOMKeyqnty.ToString() == item["AltQuantity"].ToString())
                            {
                                item.Table.Rows.Remove(item);
                                break;
                            }
                        }
                    }
                }
                Session["MultiUOMDataRETM"] = dt;
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataView dvData = new DataView(dt);
                    // Mantis Issue 24831
                    //if (DetailsId != "" && DetailsId != null && DetailsId != "null")
                    //{
                    //    //dvData.RowFilter = "SrlNo = '" + SrlNo + "'and Doc_DetailsId='" + DetailsId + "'";
                    //    dvData.RowFilter = "DetailsId='" + DetailsId + "'";
                    //}
                    //else
                    //{
                    //    dvData.RowFilter = "SrlNo = '" + SrlNo + "'";
                    //}

                    dvData.RowFilter = "SrlNo = '" + SrlNo + "'";
                    // End of Mantis Issue 24831

                    grid_MultiUOM.DataSource = dvData;
                    grid_MultiUOM.DataBind();
                }
                else
                {
                    grid_MultiUOM.DataSource = null;
                    grid_MultiUOM.DataBind();
                }
            }


            else if (SpltCmmd == "CheckMultiUOmDetailsQuantity")
            {
                string SrlNo = Convert.ToString(e.Parameters.Split('~')[1]);
                DataTable dt = (DataTable)Session["MultiUOMDataRETM"];
                string detailsId = Convert.ToString(e.Parameters.Split('~')[2]);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow[] MultiUoMresult;
                    // Mantis Issue 24831
                    //if (detailsId != null && detailsId != "" && detailsId != "null")
                    //{
                    //    MultiUoMresult = dt.Select("DetailsId ='" + detailsId + "'");
                    //}
                    //else
                    //{
                    //    MultiUoMresult = dt.Select("SrlNo ='" + SrlNo + "'");
                    //}
                    MultiUoMresult = dt.Select("SrlNo ='" + SrlNo + "'");
                    // End of Mantis Issue 24831

                    foreach (DataRow item in MultiUoMresult)
                    {
                        item.Table.Rows.Remove(item);
                    }
                }
                Session["MultiUOMDataRETM"] = dt;
            }

            // Mantis Issue 24831
            else if (SpltCmmd == "EditData")
            {
                string AltUOMKeyValuewithqnty = e.Parameters.Split('~')[1];
                string AltUOMKeyValue = e.Parameters.Split('~')[2];
                string AltUOMKeyqnty = AltUOMKeyValuewithqnty.Split('|')[1];

                string SrlNo = Convert.ToString(e.Parameters.Split('~')[1]);
                DataTable dt = (DataTable)Session["MultiUOMDataRETM"];

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow[] MultiUoMresult = dt.Select("MultiUOMSR ='" + AltUOMKeyValue + "'");

                    Decimal BaseQty = Convert.ToDecimal(MultiUoMresult[0]["Quantity"]);
                    Decimal BaseRate = Convert.ToDecimal(MultiUoMresult[0]["BaseRate"]);

                    Decimal AltQty = Convert.ToDecimal(MultiUoMresult[0]["AltQuantity"]);
                    Decimal AltRate = Convert.ToDecimal(MultiUoMresult[0]["AltRate"]);
                    Decimal AltUom = Convert.ToDecimal(MultiUoMresult[0]["AltUomId"]);
                    bool UpdateRow = Convert.ToBoolean(MultiUoMresult[0]["UpdateRow"]);

                    grid_MultiUOM.JSProperties["cpAllDetails"] = "EditData";

                    grid_MultiUOM.JSProperties["cpBaseQty"] = BaseQty;
                    grid_MultiUOM.JSProperties["cpBaseRate"] = BaseRate;


                    grid_MultiUOM.JSProperties["cpAltQty"] = AltQty;
                    grid_MultiUOM.JSProperties["cpAltUom"] = AltUom;
                    grid_MultiUOM.JSProperties["cpAltRate"] = AltRate;
                    grid_MultiUOM.JSProperties["cpUpdatedrow"] = UpdateRow;
                    grid_MultiUOM.JSProperties["cpuomid"] = AltUOMKeyValue;
                }
                Session["MultiUOMDataRETM"] = dt;
            }



            else if (SpltCmmd == "UpdateRow")
            {


                string SrlNoR = e.Parameters.Split('~')[1];
                string AltUOMKeyValue = e.Parameters.Split('~')[7];
                string AltUOMKeyqnty = e.Parameters.Split('~')[5];
                string muid = e.Parameters.Split('~')[13];
                string SrlNo = "0";
                
                string Validcheck = "";
                DataTable MultiUOMSaveData = new DataTable();

                DataTable dt = (DataTable)Session["MultiUOMDataRETM"];

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow[] MultiUoMresult = dt.Select("MultiUOMSR ='" + muid + "'");
                    foreach (DataRow item in MultiUoMresult)
                    {
                        SrlNo = Convert.ToString(item["SrlNo"]);
                
                    }
                }
                
                string Quantity = Convert.ToString(e.Parameters.Split('~')[2]);
                string UOM = Convert.ToString(e.Parameters.Split('~')[3]);
                string AltUOM = Convert.ToString(e.Parameters.Split('~')[4]);
                string AltQuantity = Convert.ToString(e.Parameters.Split('~')[5]);
                string UomId = Convert.ToString(e.Parameters.Split('~')[6]);
                string AltUomId = Convert.ToString(e.Parameters.Split('~')[7]);
                string ProductId = Convert.ToString(e.Parameters.Split('~')[8]);
                string DetailsId = Convert.ToString(e.Parameters.Split('~')[9]);
                string BaseRate = Convert.ToString(e.Parameters.Split('~')[10]);
                string AltRate = Convert.ToString(e.Parameters.Split('~')[11]);
                string UpdateRow = Convert.ToString(e.Parameters.Split('~')[12]);

                DataRow[] MultiUoMresultResult = dt.Select("SrlNo ='" + SrlNo + "' and MultiUOMSR <>'" + muid + "'");

                foreach (DataRow item in MultiUoMresultResult)
                {
                    if ((AltUomId == item["AltUomId"].ToString()) || (UomId == item["AltUomId"].ToString()))
                    {
                        if (AltQuantity == item["AltQuantity"].ToString())
                        {
                            Validcheck = "DuplicateUOM";
                            grid_MultiUOM.JSProperties["cpDuplicateAltUOM"] = "DuplicateAltUOM";
                            break;
                        }
                    }
                    if (UpdateRow == "True")
                    {
                        item["UpdateRow"] = "False";
                    }
                   
                }


                if (Validcheck != "DuplicateUOM")
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        DataRow[] MultiUoMresult = dt.Select("MultiUOMSR ='" + muid + "'");
                        foreach (DataRow item in MultiUoMresult)
                        {
                     
                            item.Table.Rows.Remove(item);
                            break;

                        }
                    }
                    dt.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId, DetailsId, BaseRate, AltRate, UpdateRow, muid);
                }
             
                Session["MultiUOMDataRETM"] = dt;

                MultiUOMSaveData = (DataTable)Session["MultiUOMDataRETM"];

                MultiUOMSaveData.AcceptChanges();
                Session["MultiUOMDataRETM"] = MultiUOMSaveData;

                if (MultiUOMSaveData != null && MultiUOMSaveData.Rows.Count > 0)
                {
                    DataView dvData = new DataView(MultiUOMSaveData);
                    dvData.RowFilter = "SrlNo = '" + SrlNo + "'";
                   
                    grid_MultiUOM.DataSource = dvData;
                    grid_MultiUOM.DataBind();
                }

            }

            else if (SpltCmmd == "SetBaseQtyRateInGrid")
            {
                DataTable dt = new DataTable();

                if (Session["MultiUOMDataRETM"] != null)
                {
                    string SrlNo = Convert.ToString(e.Parameters.Split('~')[1]);
                    dt = (DataTable)HttpContext.Current.Session["MultiUOMDataRETM"];
                    DataRow[] MultiUoMresult = dt.Select("SrlNo ='" + SrlNo + "' and UpdateRow ='True'");

                    Int64 SelNo = Convert.ToInt64(MultiUoMresult[0]["SrlNo"]);
                    Decimal BaseQty = Convert.ToDecimal(MultiUoMresult[0]["Quantity"]);
                    Decimal BaseRate = Convert.ToDecimal(MultiUoMresult[0]["BaseRate"]);

                    Decimal AltQty = Convert.ToDecimal(MultiUoMresult[0]["AltQuantity"]);
                    string AltUom = Convert.ToString(MultiUoMresult[0]["AltUOM"]);

                    grid_MultiUOM.JSProperties["cpSetBaseQtyRateInGrid"] = "1";
                    grid_MultiUOM.JSProperties["cpBaseQty"] = BaseQty;
                    grid_MultiUOM.JSProperties["cpBaseRate"] = BaseRate;


                    grid_MultiUOM.JSProperties["cpAltQty"] = AltQty;
                    grid_MultiUOM.JSProperties["cpAltUom"] = AltUom;


                }
            }
            // End of Mantis Issue 24831
        }
        protected void MultiUOM_DataBinding(object sender, EventArgs e)
        {
            //DataTable dt = (DataTable)Session["MultiUOMDataRETM"];
            //if(dt !=null && dt.Rows.Count >0 )
            //{
            //    DataView dvData = new DataView(dt);
            //    //dvData.RowFilter = "Product_SrlNo = '" + SerialID + "'";

            //    grid_MultiUOM.DataSource = dvData;
            //    grid_MultiUOM.DataBind();
            //}
            //else
            //{
            //    grid_MultiUOM.DataSource = null;
            //    grid_MultiUOM.DataBind();
            //}
        }

        protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string strSplitCommand = e.Parameters.Split('~')[0];
            if (strSplitCommand == "Display")
            {
                string IsDeleteFrom = Convert.ToString(hdfIsDelete.Value);
                if (IsDeleteFrom == "D")
                {
                    DataTable Quotationdt = (DataTable)Session["SRM_QuotationDetails"];
                    grid.DataSource = GetQuotation(Quotationdt);
                    grid.DataBind();
                }
            }
            else if (strSplitCommand == "RemoveDisplay")
            {
                DataTable salesReturndt = new DataTable();
                if (Session["SRM_QuotationDetails"] != null)
                {
                    salesReturndt = (DataTable)Session["SRM_QuotationDetails"];
                }

                DataRow[] dr = salesReturndt.Select();
                foreach (DataRow row in dr)
                {
                    salesReturndt.Rows.Remove(row);
                }

                salesReturndt.AcceptChanges();
                Session["SRM_QuotationDetails"] = salesReturndt;
                grid.DataBind();

                grid.JSProperties["cpRemoveProductInvoice"] = "valid";
                //salesReturndt.AcceptChanges();
                //Session["SRM_QuotationDetails"] = salesReturndt;
                //grid.DataSource = salesReturndt;
                //grid.DataBind();


            }
            else if (strSplitCommand == "CurrencyChangeDisplay")
            {
                if (Session["SRM_QuotationDetails"] != null)
                {
                    DataTable Quotationdt = (DataTable)Session["SRM_QuotationDetails"];

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

                            //if (Convert.ToDecimal(dr["SalePrice"]) == 0)
                            //{
                            //    strSalePrice = ProductDetailsList[6];
                            //}
                            //else
                            //{
                            //    strSalePrice = Convert.ToDecimal(dr["SalePrice"]);
                            //}
                            //strSalePrice = ProductDetailsList[6];

                            decimal SalePrice = Convert.ToDecimal(strSalePrice) / strRate;
                            string Amount = Convert.ToString(Math.Round((Convert.ToDecimal(QuantityValue) * Convert.ToDecimal(strFactor) * SalePrice), 2));
                            string amountAfterDiscount = Convert.ToString(Math.Round((Convert.ToDecimal(Amount) - ((Convert.ToDecimal(Discount) * Convert.ToDecimal(Amount)) / 100)), 2));


                            dr["SalePrice"] = Convert.ToString(Math.Round(SalePrice, 2));
                            dr["Amount"] = amountAfterDiscount;
                            //dr["TaxAmount"] = amountAfterDiscount;
                            //dr["TotalAmount"] = TotalAmount;
                            dr["TaxAmount"] = ReCalculateTaxAmount(Convert.ToString(dr["SrlNo"]), Convert.ToDouble(amountAfterDiscount));
                            dr["TotalAmount"] = Convert.ToDecimal(dr["Amount"]) + Convert.ToDecimal(dr["TaxAmount"]);
                        }
                    }

                    Session["SRM_QuotationDetails"] = Quotationdt;

                    DataView dvData = new DataView(Quotationdt);
                    dvData.RowFilter = "Status <> 'D'";

                    grid.DataSource = GetQuotation(dvData.ToTable());
                    grid.DataBind();
                }
            }
            else if (strSplitCommand == "DateChangeDisplay")
            {
                if (Session["SRM_QuotationDetails"] != null)
                {
                    DataTable Quotationdt = (DataTable)Session["SRM_QuotationDetails"];

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

                    Session["SRM_QuotationDetails"] = Quotationdt;

                    DataView dvData = new DataView(Quotationdt);
                    dvData.RowFilter = "Status <> 'D'";

                    grid.DataSource = GetQuotation(dvData.ToTable());
                    grid.DataBind();
                }
            }
            else if (strSplitCommand == "BindGridOnQuotation")
            {
                ASPxGridView gv = sender as ASPxGridView;
                string command = e.Parameters.ToString();
                string State = Convert.ToString(e.Parameters.Split('~')[1]);
                String QuoComponent1 = "";
                string Product_id1 = "";
                string InvoiceDetails_Id = "";
                for (int i = 0; i < grid_Products.GetSelectedFieldValues("Quotation_No").Count; i++)
                {

                    QuoComponent1 += "," + Convert.ToString(grid_Products.GetSelectedFieldValues("Quotation_No")[i]);
                    Product_id1 += "," + Convert.ToString(grid_Products.GetSelectedFieldValues("gvColProduct")[i]);
                    InvoiceDetails_Id += "," + Convert.ToString(grid_Products.GetSelectedFieldValues("ComponentDetailsID")[i]);
                }
                QuoComponent1 = QuoComponent1.TrimStart(',');
                Product_id1 = Product_id1.TrimStart(',');
                string Quote_Nos = Convert.ToString(e.Parameters.Split('~')[1]);
                string companyId = Convert.ToString(HttpContext.Current.Session["LastCompany"]);

                string fin_year = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);
                if (Quote_Nos != "$")
                {

                    DataTable dt_QuotationDetails = new DataTable();
                    string IdKey = Convert.ToString(Request.QueryString["key"]);
                    if (!string.IsNullOrEmpty(IdKey))
                    {
                        if (IdKey != "ADD")
                        {
                            dt_QuotationDetails = objSalesReturnBL.GetIndentDetailsForPOGridBind(QuoComponent1, IdKey, Product_id1, companyId, fin_year);
                        }
                        else
                        {
                            dt_QuotationDetails = objSalesReturnBL.GetIndentDetailsForPOGridBind(QuoComponent1, IdKey, Product_id1, companyId, fin_year);
                        }

                    }
                    else
                    {
                        dt_QuotationDetails = objSalesReturnBL.GetIndentDetailsForPOGridBind(QuoComponent1, IdKey, Product_id1, companyId, fin_year);
                    }
                    // Session["OrderDetails"] = null;



                    Session["SRM_QuotationDetails"] = null;
                    grid.DataSource = GetInvoiceInfo(dt_QuotationDetails, IdKey);
                    grid.DataBind();


                }
                else
                {
                    grid.DataSource = null;
                    grid.DataBind();
                }

                //   Session["PRI_QuotationAddressDtl"]


                //Session["SRM_QuotationAddressDtl"] = GetComponentEditedAddressData(InvoiceDetails_Id, "SI");
                //if (Session["SRM_QuotationAddressDtl"] != null)
                //{
                //    DataTable dt = (DataTable)Session["SRM_QuotationAddressDtl"];
                //    if (dt != null && dt.Rows.Count > 0)
                //    {
                //        if (dt.Rows.Count == 2)
                //        {
                //            if (Convert.ToString(dt.Rows[0]["ReturnAdd_addressType"]) == "Shipping")
                //            {
                //                string countryid = Convert.ToString(dt.Rows[0]["ReturnAdd_countryId"]);
                //                CmbCountry1.Value = countryid;
                //                FillStateCombo(CmbState1, Convert.ToInt32(countryid));
                //                string stateid = Convert.ToString(dt.Rows[0]["ReturnAdd_stateId"]);
                //                CmbState1.Value = stateid;
                //            }
                //            else if (Convert.ToString(dt.Rows[1]["ReturnAdd_addressType"]) == "Shipping")
                //            {
                //                string countryid = Convert.ToString(dt.Rows[0]["ReturnAdd_countryId"]);
                //                CmbCountry1.Value = countryid;
                //                FillStateCombo(CmbState1, Convert.ToInt32(countryid));
                //                string stateid = Convert.ToString(dt.Rows[0]["ReturnAdd_stateId"]);
                //                CmbState1.Value = stateid;
                //            }
                //        }
                //        else if (dt.Rows.Count == 1)
                //        {
                //            if (Convert.ToString(dt.Rows[0]["ReturnAdd_addressType"]) == "Shipping")
                //            {
                //                string countryid = Convert.ToString(dt.Rows[0]["ReturnAdd_countryId"]);
                //                CmbCountry1.Value = countryid;
                //                FillStateCombo(CmbState1, Convert.ToInt32(countryid));
                //                string stateid = Convert.ToString(dt.Rows[0]["ReturnAdd_stateId"]);
                //                CmbState1.Value = stateid;
                //            }

                //        }
                //    }
                //}



            }

            else if (strSplitCommand == "GridBlank")
            {

                lookup_quotation.GridView.Selection.UnselectAll();
                lookup_quotation.DataSource = null;
                lookup_quotation.DataBind();

                // txt_InvoiceDate.Text = "";
                Session["SRM_QuotationDetails"] = null;
                Session["SRM_QuotationAddressDtl"] = null;
                Session["SRM_WarehouseData"] = null;
                Session["SRwarehousedetailstemp"] = null;
                Session["SRM_QuotationTaxDetails"] = null;
                grid.DataSource = null;
                grid.DataBind();
                grid.JSProperties["cpGridBlank"] = "1";
            }

            else if (strSplitCommand == "EInvoice")
            {
                UploadEinvoice(e.Parameters.Split('~')[1]);
            }

        }


        private static DataSet GetInvoiceDetails(string id)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_Einvoice");
            proc.AddVarcharPara("@Action", 100, "GetSRDetails");
            proc.AddVarcharPara("@id", 4000, id);
            ds = proc.GetDataSet();
            return ds;
        }
        private void UploadEinvoice(string id)
        {
            grid.JSProperties["cpSucessIRN"] = null;
            CommonBL objBL = new CommonBL();
            string setting = objBL.GetSystemSettingsResult("IsBasicEInvoice");


            if (setting.ToUpper() == "YES")
            {
                List<EinvoiceModel> obj = new List<EinvoiceModel>();
                DataSet ds = GetInvoiceDetails(id.ToString());


                DataTable Header = ds.Tables[0];
                DataTable SellerDetails = ds.Tables[1];
                DataTable BuyerDetails = ds.Tables[2];
                DataTable ValueDetails = ds.Tables[3];
                DataTable Products = ds.Tables[4];
                DataTable ShipDetails = ds.Tables[5];
                DataTable DispatchFrom = ds.Tables[6];

                DBEngine objDBEngineCredential = new DBEngine();
                string Branch_id = Convert.ToString(Header.Rows[0]["Return_BranchId"]);
                DataTable dt = objDBEngineCredential.GetDataTable("select EwayBill_Userid,EwayBill_Password,EwayBill_GSTIN,EInvoice_UserId,EInvoice_Password,branch_GSTIN from tbl_master_branch where branch_id='" + Branch_id + "'");
                string IRN_API_UserId = Convert.ToString(dt.Rows[0]["EInvoice_UserId"]);
                string IRN_API_Password = Convert.ToString(dt.Rows[0]["EInvoice_Password"]);
                string IRN_API_GSTIN = Convert.ToString(dt.Rows[0]["branch_GSTIN"]);


                string IrnUser = ConfigurationManager.AppSettings["IRNUserId"];
                string IrnPassword = ConfigurationManager.AppSettings["IRNPasswod"];
                string IrnBaseURL = ConfigurationManager.AppSettings["IRNBaseURL"];
                string IrnOrgId = ConfigurationManager.AppSettings["IRNOrgID"];
                string IrnCancelUrl = ConfigurationManager.AppSettings["IRNCancelURL"];
                string IrnEwaybillUrl = ConfigurationManager.AppSettings["IrnEwaybillUrl"];
                string IrnGenerationUrl = ConfigurationManager.AppSettings["IrnGenerationUrl"];


                EinvoiceModel objInvoice = new EinvoiceModel("1.1");

                TrasporterDetails objTransporter = new TrasporterDetails();
                objTransporter.EcmGstin = null;
                objTransporter.IgstOnIntra = "N";
                if (Convert.ToBoolean(Header.Rows[0]["IsReverseCharge"]))
                {
                    objTransporter.RegRev = "Y";     /// From table mantis id 23407
                }
                else
                {
                    objTransporter.RegRev = "N";
                }
                if (Convert.ToString(Header.Rows[0]["TransCategory"]) != "" && Convert.ToString(Header.Rows[0]["TransCategory"]) != "0")
                    objTransporter.SupTyp = Convert.ToString(Header.Rows[0]["TransCategory"]);   /// From table mantis id 23406
                else
                    objTransporter.SupTyp = "B2B";
                objTransporter.TaxSch = "GST";
                objInvoice.TranDtls = objTransporter;


                DocumentsDetails objDoc = new DocumentsDetails();
                objDoc.Dt = Convert.ToDateTime(Header.Rows[0]["Invoice_Date"]).ToString("dd/MM/yyyy");     // Form table invoice_Date DD/MM/YYYY format
                objDoc.No = Convert.ToString(Header.Rows[0]["Invoice_Number"]);   // Form table invoice_Number
                objDoc.Typ = "CRN";  //INV-Invoice ,CRN-Credit Note, DBN-Debit Note//Mantis:0024685
                objInvoice.DocDtls = objDoc;


                SellerDetails objSeller = new SellerDetails();
                objSeller.Addr1 = Convert.ToString(SellerDetails.Rows[0]["Addr1"]);   /// Based on settings Branch/Company master
                objSeller.Addr2 = Convert.ToString(SellerDetails.Rows[0]["Addr2"]); ;   /// Based on settings Branch/Company master 
                if (Convert.ToString(SellerDetails.Rows[0]["Em"]) != "")
                    objSeller.Em = Convert.ToString(SellerDetails.Rows[0]["Em"]); ;      /// Based on settings Branch/Company master 
                //objSeller.Gstin = Convert.ToString(SellerDetails.Rows[0]["Gstin"]); ;   /// Based on settings Branch/Company master

                objSeller.Gstin = IRN_API_GSTIN;//Sandbox
                objSeller.LglNm = Convert.ToString(SellerDetails.Rows[0]["LglNm"]); ;  /// Based on settings Branch/Company master 
                if (Convert.ToString(SellerDetails.Rows[0]["Loc"]) != "")
                    objSeller.Loc = Convert.ToString(SellerDetails.Rows[0]["Loc"]);     /// Based on settings Branch/Company master
                else
                    objSeller.Loc = Convert.ToString(SellerDetails.Rows[0]["Addr2"]);
                /// 
                if (Convert.ToString(SellerDetails.Rows[0]["Ph"]) != "")
                    objSeller.Ph = Convert.ToString(SellerDetails.Rows[0]["Ph"]).Replace(",", "");      /// Based on settings Branch/Company master
                objSeller.Pin = Convert.ToInt32(SellerDetails.Rows[0]["Pin"]); ;     /// Based on settings Branch/Company master
                objSeller.Stcd = Convert.ToString(SellerDetails.Rows[0]["Stcd"]); ;    /// Based on settings Branch/Company master
                objSeller.TrdNm = Convert.ToString(SellerDetails.Rows[0]["TrdNm"]); ;   /// Based on settings Branch/Company master
                objInvoice.SellerDtls = objSeller;


                BuyerDetails objBuyer = new BuyerDetails();
                objBuyer.Addr1 = Convert.ToString(BuyerDetails.Rows[0]["Addr1"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                objBuyer.Addr2 = Convert.ToString(BuyerDetails.Rows[0]["Addr2"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                if (Convert.ToString(BuyerDetails.Rows[0]["Em"]) != "")
                    objBuyer.Em = Convert.ToString(BuyerDetails.Rows[0]["Em"]);    ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                objBuyer.Gstin = Convert.ToString(BuyerDetails.Rows[0]["Gstin"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                objBuyer.LglNm = Convert.ToString(BuyerDetails.Rows[0]["LglNm"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                if (Convert.ToString(BuyerDetails.Rows[0]["Loc"]) != "")
                    objBuyer.Loc = Convert.ToString(BuyerDetails.Rows[0]["Loc"]);   ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                else
                    objBuyer.Loc = Convert.ToString(BuyerDetails.Rows[0]["Addr2"]);
                if (Convert.ToString(BuyerDetails.Rows[0]["Ph"]) != "")
                    objBuyer.Ph = Convert.ToString(BuyerDetails.Rows[0]["Ph"]);    ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                objBuyer.Pin = Convert.ToInt32(BuyerDetails.Rows[0]["Pin"]);   ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                objBuyer.Stcd = Convert.ToString(BuyerDetails.Rows[0]["Stcd"]);  ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                objBuyer.Pos = Convert.ToString(BuyerDetails.Rows[0]["Stcd"]);
                objBuyer.TrdNm = Convert.ToString(BuyerDetails.Rows[0]["TrdNm"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                objInvoice.BuyerDtls = objBuyer;


                objInvoice.DispDtls = null;  // for now 
                objInvoice.ShipDtls = null; ///Shipping details from invoice i.e tbl_trans_salesinvoiceadddress

                ValueDetails objValue = new ValueDetails();
                objValue.AssVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Taxable"]).ToString("0.00"));   // Taxable value
                objValue.CesVal = 0.00M;
                objValue.CgstVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Total_CGST"]).ToString("0.00"));
                objValue.Discount = 0.00M;
                objValue.IgstVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Total_IGST"]).ToString("0.00"));
                objValue.OthChrg = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Other_Charge"]).ToString("0.00"));   // Global Tax
                objValue.RndOffAmt = 0.00M;
                objValue.SgstVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Total_SGST"]).ToString("0.00"));
                objValue.StCesVal = 0.00M;
                objValue.TotInvVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["TotalAmount"]).ToString("0.00"));
                objValue.TotInvValFc = 0.00M;
                objInvoice.ValDtls = objValue;


                //ExportDetails objExport = new ExportDetails();
                //objExport.CntCode = ""; ///optional for now
                //objExport.ExpDuty = 0;  ///optional for now
                //objExport.ForCur = "";  ///optional for now
                //objExport.Port = "";    ///optional for now
                //objExport.RefClm = "";  ///optional for now
                //objExport.ShipBDt = ""; ///optional for now
                //objExport.ShipBNo = ""; ///optional for now
                //objInvoice.ExpDtls = objExport;

                //EwayBillDetails objEway = new EwayBillDetails();
                //if (Header.Rows[0]["Trans_Distance"] != null && Convert.ToDecimal(Header.Rows[0]["Trans_Distance"]) != 0)
                //    objEway.Distance = Convert.ToInt32(Header.Rows[0]["Trans_Distance"]);    ///from table Mantis id 23408 
                //else
                //    objEway.Distance = 0;
                /////
                //if (Header.Rows[0]["Transporter_DocDate"] != DBNull.Value && Header.Rows[0]["Transporter_DocDate"] != null)
                //    objEway.TransDocDt = Convert.ToDateTime(Header.Rows[0]["Transporter_DocDate"]).ToString("dd/MM/yyy"); ///from table Mantis id 23408 
                //if (Header.Rows[0]["Transporter_DocNo"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                //    objEway.TransDocNo = Convert.ToString(Header.Rows[0]["Transporter_DocNo"]); ///from table Mantis id 23408 
                //if (Header.Rows[0]["Transporter_GSTIN"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                //    objEway.TransId = Convert.ToString(Header.Rows[0]["Transporter_GSTIN"]);    ///from table Mantis id 23408 
                //if (Header.Rows[0]["Transporter_Mode"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                //    objEway.TransMode = Convert.ToString(Header.Rows[0]["Transporter_Mode"]);  ///from table Mantis id 23408 
                //if (Header.Rows[0]["Transporter_Name"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                //    objEway.TransName = Convert.ToString(Header.Rows[0]["Transporter_Name"]);  ///from table Mantis id 23408 
                //if (Header.Rows[0]["Vehicle_No"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                //    objEway.VehNo = Convert.ToString(Header.Rows[0]["Vehicle_No"]);      ///from table Mantis id 23408 
                //if (Header.Rows[0]["Vehicle_Type"] != DBNull.Value && Header.Rows[0]["Transporter_DocNo"] != null)
                //    objEway.VehType = Convert.ToString(Header.Rows[0]["Vehicle_Type"]);    ///from table Mantis id 23408 
                //objInvoice.EwbDtls = objEway;



                DispatchDetails objDisp = new DispatchDetails();
                objDisp.Addr1 = Convert.ToString(DispatchFrom.Rows[0]["Addr1"]);
                objDisp.Addr2 = Convert.ToString(DispatchFrom.Rows[0]["Addr2"]);
                objDisp.Loc = Convert.ToString(DispatchFrom.Rows[0]["Addr2"]);
                objDisp.Nm = Convert.ToString(DispatchFrom.Rows[0]["Nm"]);
                objDisp.Pin = Convert.ToInt32(DispatchFrom.Rows[0]["Pin"]);
                objDisp.Stcd = Convert.ToString(DispatchFrom.Rows[0]["Stcd"]);
                objInvoice.DispDtls = objDisp;



                ShipToDetails objShip = new ShipToDetails();
                objShip.Addr1 = Convert.ToString(ShipDetails.Rows[0]["Addr1"]); ;
                objShip.Addr2 = Convert.ToString(ShipDetails.Rows[0]["Addr2"]); ;
                objShip.Loc = Convert.ToString(ShipDetails.Rows[0]["Addr2"]); ;
                objShip.Gstin = Convert.ToString(ShipDetails.Rows[0]["Gstin"]); ;
                objShip.LglNm = Convert.ToString(ShipDetails.Rows[0]["LglNm"]); ;
                objShip.TrdNm = Convert.ToString(ShipDetails.Rows[0]["TrdNm"]); ;
                objShip.Pin = Convert.ToInt32(ShipDetails.Rows[0]["Pin"]); ;
                objShip.Stcd = Convert.ToString(ShipDetails.Rows[0]["Stcd"]); ;
                objInvoice.ShipDtls = objShip;


                //PaymentDetails objPayment = new PaymentDetails();
                //objPayment.AccDet = "";   ///Optional For Now
                //objPayment.CrDay = 0;     ///Optional For Now
                //objPayment.CrTrn = "";    ///Optional For Now
                //objPayment.DirDr = "";    ///Optional For Now
                //objPayment.FinInsBr = ""; ///Optional For Now
                //objPayment.Mode = "";     ///Optional For Now
                //objPayment.Nm = "";       ///Optional For Now
                //objPayment.PaidAmt = 0;   ///Optional For Now
                //objPayment.PayInstr = ""; ///Optional For Now
                //objPayment.PaymtDue = 0;  ///Optional For Now
                //objPayment.PayTerm = "";  ///Optional For Now
                //objInvoice.PayDtls = objPayment;


                //ReferenceDetails objRef = new ReferenceDetails();

                //List<ContractDetails> onjListContact = new List<ContractDetails>();
                //for (int i = 0; i < 1; i++)
                //{
                //    ContractDetails onjContact = new ContractDetails();
                //    onjContact.ContrRefr = "";
                //    onjContact.ExtRefr = "";
                //    onjContact.PORefDt = "";
                //    onjContact.PORefr = "";
                //    onjContact.ProjRefr = "";
                //    onjContact.RecAdvDt = "";
                //    onjContact.RecAdvRefr = "";
                //    onjContact.TendRefr = "";
                //    onjListContact.Add(onjContact);
                //}
                //objRef.ContrDtls = onjListContact;


                //List<PrecDocumentDetails> onjListPrecDoc = new List<PrecDocumentDetails>();
                //for (int i = 0; i < 1; i++)
                //{
                //    PrecDocumentDetails onjPrecDoc = new PrecDocumentDetails();
                //    onjPrecDoc.InvDt = "";
                //    onjPrecDoc.InvNo = "";
                //    onjPrecDoc.OthRefNo = "";
                //    onjListPrecDoc.Add(onjPrecDoc);
                //}
                //objRef.PrecDocDtls = onjListPrecDoc;

                //DocumentPerdDetails objdocPre = new DocumentPerdDetails();
                //objdocPre.InvEndDt = "";
                //objdocPre.InvStDt = "";
                //objRef.DocPerdDtls = objdocPre;

                //objRef.InvRm = "";  // Remarks from invoice
                //objInvoice.RefDtls = objRef;   ///////////// Optional For now



                //List<AdditionalDocumentDetails> objListAddl = new List<AdditionalDocumentDetails>();
                //for (int i = 0; i < 1; i++)
                //{
                //    AdditionalDocumentDetails objAddl = new AdditionalDocumentDetails();
                //    objAddl.Docs = "";
                //    objAddl.Info = "";
                //    objAddl.Url = "";
                //    objListAddl.Add(objAddl);
                //}
                //objInvoice.AddlDocDtls = objListAddl;    /// Optional for now


                List<ProductList> objListProd = new List<ProductList>();

                foreach (DataRow dr in Products.Rows)
                {
                    ProductList objProd = new ProductList();
                    // objProd.AssAmt = 0.00M;

                    //**************Commented for now -- This is foer Attribute adding ********************************//

                    //List<AttributeDetails> objListAttr = new List<AttributeDetails>();
                    //for (int j = 0; j < 1; j++)
                    //{
                    //    AttributeDetails objAttr = new AttributeDetails();
                    //    objAttr.Nm = "";
                    //    objAttr.Val = "";
                    //    objListAttr.Add(objAttr);
                    //}
                    //objProd.AttribDtls = objListAttr;

                    //**************End Commented for now -- This is foer Attribute adding ******************************//

                    objProd.AssAmt = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Amount"]).ToString("0.00"));
                    objProd.Barcde = null;
                    objProd.BchDtls = null;
                    objProd.CesAmt = 0.00M;
                    objProd.CesNonAdvlAmt = 0.00M;
                    objProd.CesRt = 0.00M;
                    objProd.CgstAmt = Convert.ToDecimal(Convert.ToDecimal(dr["CGSTAmount"]).ToString("0.00"));
                    objProd.Discount = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Discount"]).ToString("0.00"));
                    objProd.FreeQty = 0.00M;
                    objProd.GstRt = Convert.ToDecimal(Convert.ToDecimal(dr["GST_RATE"]).ToString("0.00")); 
                    objProd.HsnCd = Convert.ToString(dr["sProducts_HsnCode"]);
                    objProd.IgstAmt = Convert.ToDecimal(Convert.ToDecimal(dr["IGSTAmount"]).ToString("0.00"));
                    if (!Convert.ToBoolean(dr["Is_ServiceItem"]))
                        objProd.IsServc = "N";
                    else
                        objProd.IsServc = "Y";
                    objProd.OrdLineRef = null;
                    objProd.OrgCntry = null;
                    objProd.OthChrg = Convert.ToDecimal(Convert.ToDecimal(dr["OtherAmount"]).ToString("0.00"));
                    objProd.PrdDesc = Convert.ToString(dr["InvoiceDetails_ProductDescription"]);
                    objProd.PrdSlNo = null;
                    objProd.PreTaxVal = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Amount"]).ToString("0.00"));
                    objProd.Qty = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Quantity"]).ToString("0.000"));
                    objProd.SgstAmt = Convert.ToDecimal(Convert.ToDecimal(dr["SGSTAmount"]).ToString("0.00"));
                    objProd.SlNo = Convert.ToString(dr["SL"]);
                    objProd.StateCesAmt = 0.00M;
                    objProd.StateCesNonAdvlAmt = 0.00M;
                    objProd.StateCesRt = 0.00M;
                    objProd.TotAmt = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Amount"]).ToString("0.00"));
                    objProd.TotItemVal = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_TotalAmountInBaseCurrency"]).ToString("0.00")); ;
                    if (Convert.ToString(dr["GST_Print_Name"]) != "")
                        objProd.Unit = Convert.ToString(dr["GST_Print_Name"]);
                    //else
                    //    objProd.Unit = "BAG";
                    objProd.UnitPrice = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_SalePrice"]).ToString("0.00")); ;
                    objListProd.Add(objProd);
                }
                objInvoice.ItemList = objListProd;

                obj.Add(objInvoice);

                authtokensOutput authObj = new authtokensOutput();
                if (DateTime.Now > EinvoiceToken.Expiry)
                {
                    try
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                               SecurityProtocolType.Tls11 |
                                               SecurityProtocolType.Tls12;
                            authtokensInput objI = new authtokensInput(IrnUser,IrnPassword);
                            var json = JsonConvert.SerializeObject(objI, Formatting.Indented);
                            var stringContent = new StringContent(json);
                            var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                            var response = client.PostAsync(IrnBaseURL, stringContent).Result;

                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                var jsonString = response;
                                var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
                                authObj = response.Content.ReadAsAsync<authtokensOutput>().Result;

                                EinvoiceToken.token = authObj.data.token;
                                long unixDate = authObj.data.expiry;
                                DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                                DateTime date = start.AddMilliseconds(unixDate).ToLocalTime();
                                EinvoiceToken.Expiry = date;
                            }
                        }
                    }
                    catch (AggregateException err)
                    {
                        foreach (var errInner in err.InnerExceptions)
                        {

                        }
                    }
                }

                try
                {
                    IRN objIRN = new IRN();
                    using (var client = new HttpClient())
                    {
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                        SecurityProtocolType.Tls11 |
                        SecurityProtocolType.Tls12;
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var json = JsonConvert.SerializeObject(objInvoice, Formatting.Indented);
                        var stringContent = new StringContent(json);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-USER-TOKEN", EinvoiceToken.token);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-ORG-ID", IrnOrgId);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSTIN", IRN_API_GSTIN);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-USERNAME", IRN_API_UserId);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-PWD", IRN_API_Password);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSP-CODE", "clayfin");
                        var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                        var response = client.PostAsync(IrnGenerationUrl, stringContent).Result;
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var jsonString = response.Content.ReadAsStringAsync().Result;
                            objIRN = response.Content.ReadAsAsync<IRN>().Result;

                            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(objIRN.data)))
                            {
                                // Deserialization from JSON  
                                DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(IRNDetails));
                                IRNDetails objIRNDetails = (IRNDetails)deserializer.ReadObject(ms);

                                DBEngine objDb = new DBEngine();
                                objDb.GetDataTable("update TBL_TRANS_SALESRETURN SET AckNo='" + objIRNDetails.AckNo + "',AckDt='" + objIRNDetails.AckDt + "',Irn='" + objIRNDetails.Irn + "',SignedInvoice='" + objIRNDetails.SignedInvoice + "',SignedQRCode='" + objIRNDetails.SignedQRCode + "',Status='" + objIRNDetails.Status + "' where return_id='" + id.ToString() + "'");
                                grid.JSProperties["cpSucessIRN"] = "Yes";
                                grid.JSProperties["cpSucessIRNNumber"] = objIRNDetails.Irn;
                            }
                        }
                        else
                        {
                            EinvoiceError err = new EinvoiceError();
                            var jsonString = response.Content.ReadAsStringAsync().Result;
                            // var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
                            err = response.Content.ReadAsAsync<EinvoiceError>().Result;


                            DBEngine objDB = new DBEngine();
                            objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SR'");
                            if (err.error.type != "ClientRequest")
                            {
                                foreach (errorlog item in err.error.args.irp_error.details)
                                {
                                    objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SR','IRN_GEN','" + item.ErrorCode + "','" + item.ErrorMessage.Replace("'", "''") + "')");
                                }
                            }
                            else
                            {
                                ClientEinvoiceError cErr = new ClientEinvoiceError();
                                cErr = JsonConvert.DeserializeObject<ClientEinvoiceError>(response.Content.ReadAsStringAsync().Result);
                                foreach (string item in cErr.error.args.errors)
                                {
                                    objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SR','IRN_GEN','" + "0" + "','" + item + "')");
                                }
                            }
                            grid.JSProperties["cpSucessIRN"] = "No";
                        }


                    }
                }
                catch (AggregateException err)
                {
                    foreach (var errInner in err.InnerExceptions)
                    {
                        grid.JSProperties["cpSucessIRN"] = "No";
                    }
                }
            }
            else
            {
                Enrich objEnrich = new Enrich();
                meta objMeta = new meta();
                List<string> lstEmail = new List<string>();
                //lstEmail.Add("bhaskar.chatterjee@indusnet.co.in ");
                //lstEmail.Add("pijushk.bhattacharya@indusnet.co.in");
                //lstEmail.Add("indranil.dey@indusnet.co.in");
                objMeta.emailRecipientList = lstEmail;
                objMeta.generatePdf = "Y";
                objEnrich.meta = objMeta;

                List<EinvoiceModelEnrich> obj = new List<EinvoiceModelEnrich>();
                DataSet ds = GetInvoiceDetails(id.ToString());


                DataTable Header = ds.Tables[0];
                DataTable SellerDetails = ds.Tables[1];
                DataTable BuyerDetails = ds.Tables[2];
                DataTable ValueDetails = ds.Tables[3];
                DataTable Products = ds.Tables[4];
                DataTable ShipDetails = ds.Tables[5];




                EinvoiceModelEnrich objInvoice = new EinvoiceModelEnrich("1.1");

                TrasporterDetailsEnrich objTransporter = new TrasporterDetailsEnrich();
                objTransporter.IgstOnIntra = "N";
                if (Convert.ToBoolean(Header.Rows[0]["IsReverseCharge"]))
                {
                    objTransporter.RegRev = "Y";     /// From table mantis id 23407
                }
                else
                {
                    objTransporter.RegRev = "N";
                }
                if (Convert.ToString(Header.Rows[0]["TransCategory"]) != "" && Convert.ToString(Header.Rows[0]["TransCategory"]) != "0")
                    objTransporter.SupTyp = Convert.ToString(Header.Rows[0]["TransCategory"]);   /// From table mantis id 23406
                else
                    objTransporter.SupTyp = "B2B";
                objTransporter.TaxSch = "GST";
                objInvoice.TranDtls = objTransporter;


                DocumentsDetailsEnrich objDoc = new DocumentsDetailsEnrich();
                objDoc.Dt = Convert.ToDateTime(Header.Rows[0]["Invoice_Date"]).ToString("dd/MM/yyyy");     // Form table invoice_Date DD/MM/YYYY format
                objDoc.No = Convert.ToString(Header.Rows[0]["Invoice_Number"]);   // Form table invoice_Number
                objDoc.Typ = "CRN";  //INV-Invoice ,CRN-Credit Note, DBN-Debit Note
                objInvoice.DocDtls = objDoc;


                SellerDetailsEnrich objSeller = new SellerDetailsEnrich();
                objSeller.Addr1 = Convert.ToString(SellerDetails.Rows[0]["Addr1"]);   /// Based on settings Branch/Company master               

                //objSeller.Gstin = Convert.ToString(SellerDetails.Rows[0]["Gstin"]); ;   /// Based on settings Branch/Company master

                objSeller.Gstin = "19AABCP5428M1Z0";//Sandbox
                objSeller.LglNm = Convert.ToString(SellerDetails.Rows[0]["LglNm"]); ;  /// Based on settings Branch/Company master 
                if (Convert.ToString(SellerDetails.Rows[0]["Loc"]) != "")
                    objSeller.Loc = Convert.ToString(SellerDetails.Rows[0]["Loc"]);     /// Based on settings Branch/Company master
                else
                    objSeller.Loc = Convert.ToString(SellerDetails.Rows[0]["Addr2"]);

                objSeller.Pin = Convert.ToInt32(SellerDetails.Rows[0]["Pin"]); ;     /// Based on settings Branch/Company master
                objSeller.Stcd = Convert.ToString(SellerDetails.Rows[0]["Stcd"]); ;    /// Based on settings Branch/Company master
                objSeller.TrdNm = Convert.ToString(SellerDetails.Rows[0]["TrdNm"]); ;   /// Based on settings Branch/Company master
                objInvoice.SellerDtls = objSeller;


                BuyerDetailsEnrich objBuyer = new BuyerDetailsEnrich();
                objBuyer.Addr1 = Convert.ToString(BuyerDetails.Rows[0]["Addr1"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress


                objBuyer.Gstin = Convert.ToString(BuyerDetails.Rows[0]["Gstin"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                objBuyer.LglNm = Convert.ToString(BuyerDetails.Rows[0]["LglNm"]); ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                if (Convert.ToString(BuyerDetails.Rows[0]["Loc"]) != "")
                    objBuyer.Loc = Convert.ToString(BuyerDetails.Rows[0]["Loc"]);   ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                else
                    objBuyer.Loc = Convert.ToString(BuyerDetails.Rows[0]["Addr2"]);

                objBuyer.Pin = Convert.ToInt32(BuyerDetails.Rows[0]["Pin"]);   ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                objBuyer.Stcd = Convert.ToString(BuyerDetails.Rows[0]["Stcd"]);  ///Billing details from invoice i.e tbl_trans_salesinvoiceadddress
                objBuyer.Pos = Convert.ToString(BuyerDetails.Rows[0]["Stcd"]);
                objInvoice.BuyerDtls = objBuyer;


                objInvoice.DispDtls = null;  // for now 
                objInvoice.ShipDtls = null; ///Shipping details from invoice i.e tbl_trans_salesinvoiceadddress

                ValueDetailsEnrich objValue = new ValueDetailsEnrich();
                objValue.AssVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Taxable"]).ToString("0.00"));   // Taxable value                
                objValue.CgstVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Total_CGST"]).ToString("0.00"));
                objValue.IgstVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Total_IGST"]).ToString("0.00"));
                objValue.SgstVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["Total_SGST"]).ToString("0.00"));
                objValue.TotInvVal = Convert.ToDecimal(Convert.ToDecimal(ValueDetails.Rows[0]["TotalAmount"]).ToString("0.00"));

                objInvoice.ValDtls = objValue;


                //ExportDetails objExport = new ExportDetails();
                //objExport.CntCode = ""; ///optional for now
                //objExport.ExpDuty = 0;  ///optional for now
                //objExport.ForCur = "";  ///optional for now
                //objExport.Port = "";    ///optional for now
                //objExport.RefClm = "";  ///optional for now
                //objExport.ShipBDt = ""; ///optional for now
                //objExport.ShipBNo = ""; ///optional for now
                //objInvoice.ExpDtls = objExport;




                //DispatchDetailsEnrich objDisp = new DispatchDetailsEnrich();
                //objDisp.Addr1 = Convert.ToString(ShipDetails.Rows[0]["Addr1"]);
                //objDisp.Addr2 = Convert.ToString(ShipDetails.Rows[0]["Addr2"]);
                //objDisp.Loc = Convert.ToString(ShipDetails.Rows[0]["Addr2"]);
                //objDisp.Nm = Convert.ToString(ShipDetails.Rows[0]["Nm"]);
                //objDisp.Pin = Convert.ToInt32(ShipDetails.Rows[0]["Pin"]);
                //objDisp.Stcd = Convert.ToString(ShipDetails.Rows[0]["Stcd"]);
                //objInvoice.DispDtls = objDisp;



                ShipToDetailsEnrich objShip = new ShipToDetailsEnrich();
                objShip.Addr1 = Convert.ToString(ShipDetails.Rows[0]["Addr1"]); ;
                objShip.Addr2 = Convert.ToString(ShipDetails.Rows[0]["Addr2"]); ;
                objShip.Loc = Convert.ToString(ShipDetails.Rows[0]["Addr2"]); ;
                objShip.Gstin = Convert.ToString(ShipDetails.Rows[0]["Gstin"]); ;
                objShip.LglNm = Convert.ToString(ShipDetails.Rows[0]["LglNm"]); ;
                objShip.TrdNm = Convert.ToString(ShipDetails.Rows[0]["TrdNm"]); ;
                objShip.Pin = Convert.ToInt32(ShipDetails.Rows[0]["Pin"]); ;
                objShip.Stcd = Convert.ToString(ShipDetails.Rows[0]["Stcd"]); ;
                objInvoice.ShipDtls = objShip;


                //PaymentDetails objPayment = new PaymentDetails();
                //objPayment.AccDet = "";   ///Optional For Now
                //objPayment.CrDay = 0;     ///Optional For Now
                //objPayment.CrTrn = "";    ///Optional For Now
                //objPayment.DirDr = "";    ///Optional For Now
                //objPayment.FinInsBr = ""; ///Optional For Now
                //objPayment.Mode = "";     ///Optional For Now
                //objPayment.Nm = "";       ///Optional For Now
                //objPayment.PaidAmt = 0;   ///Optional For Now
                //objPayment.PayInstr = ""; ///Optional For Now
                //objPayment.PaymtDue = 0;  ///Optional For Now
                //objPayment.PayTerm = "";  ///Optional For Now
                //objInvoice.PayDtls = objPayment;


                //ReferenceDetails objRef = new ReferenceDetails();

                //List<ContractDetails> onjListContact = new List<ContractDetails>();
                //for (int i = 0; i < 1; i++)
                //{
                //    ContractDetails onjContact = new ContractDetails();
                //    onjContact.ContrRefr = "";
                //    onjContact.ExtRefr = "";
                //    onjContact.PORefDt = "";
                //    onjContact.PORefr = "";
                //    onjContact.ProjRefr = "";
                //    onjContact.RecAdvDt = "";
                //    onjContact.RecAdvRefr = "";
                //    onjContact.TendRefr = "";
                //    onjListContact.Add(onjContact);
                //}
                //objRef.ContrDtls = onjListContact;


                //List<PrecDocumentDetails> onjListPrecDoc = new List<PrecDocumentDetails>();
                //for (int i = 0; i < 1; i++)
                //{
                //    PrecDocumentDetails onjPrecDoc = new PrecDocumentDetails();
                //    onjPrecDoc.InvDt = "";
                //    onjPrecDoc.InvNo = "";
                //    onjPrecDoc.OthRefNo = "";
                //    onjListPrecDoc.Add(onjPrecDoc);
                //}
                //objRef.PrecDocDtls = onjListPrecDoc;

                //DocumentPerdDetails objdocPre = new DocumentPerdDetails();
                //objdocPre.InvEndDt = "";
                //objdocPre.InvStDt = "";
                //objRef.DocPerdDtls = objdocPre;

                //objRef.InvRm = "";  // Remarks from invoice
                //objInvoice.RefDtls = objRef;   ///////////// Optional For now



                //List<AdditionalDocumentDetails> objListAddl = new List<AdditionalDocumentDetails>();
                //for (int i = 0; i < 1; i++)
                //{
                //    AdditionalDocumentDetails objAddl = new AdditionalDocumentDetails();
                //    objAddl.Docs = "";
                //    objAddl.Info = "";
                //    objAddl.Url = "";
                //    objListAddl.Add(objAddl);
                //}
                //objInvoice.AddlDocDtls = objListAddl;    /// Optional for now


                List<ProductListEnrich> objListProd = new List<ProductListEnrich>();

                foreach (DataRow dr in Products.Rows)
                {
                    ProductListEnrich objProd = new ProductListEnrich();
                    // objProd.AssAmt = 0.00M;

                    //**************Commented for now -- This is foer Attribute adding ********************************//

                    //List<AttributeDetails> objListAttr = new List<AttributeDetails>();
                    //for (int j = 0; j < 1; j++)
                    //{
                    //    AttributeDetails objAttr = new AttributeDetails();
                    //    objAttr.Nm = "";
                    //    objAttr.Val = "";
                    //    objListAttr.Add(objAttr);
                    //}
                    //objProd.AttribDtls = objListAttr;

                    //**************End Commented for now -- This is foer Attribute adding ******************************//

                    objProd.AssAmt = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Amount"]).ToString("0.00"));
                    objProd.Barcde = null;
                    objProd.BchDtls = null;
                    objProd.CesAmt = 0.00M;
                    objProd.CesNonAdvlAmt = 0.00M;
                    objProd.CesRt = 0.00M;
                    objProd.CgstAmt = Convert.ToDecimal(Convert.ToDecimal(dr["CGSTAmount"]).ToString("0.00"));
                    objProd.Discount = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Discount"]).ToString("0.00"));
                    objProd.FreeQty = 0.00M;
                    objProd.GstRt = Convert.ToDecimal(Convert.ToDecimal(dr["IGSTAmount"]).ToString("0.00"));
                    objProd.HsnCd = Convert.ToString(dr["sProducts_HsnCode"]);
                    objProd.IgstAmt = Convert.ToDecimal(Convert.ToDecimal(dr["IGSTAmount"]).ToString("0.00"));
                    objProd.IsServc = "N";
                    objProd.OrdLineRef = null;
                    objProd.OrgCntry = null;
                    objProd.OthChrg = Convert.ToDecimal(Convert.ToDecimal(dr["OtherAmount"]).ToString("0.00"));
                    objProd.PrdDesc = Convert.ToString(dr["InvoiceDetails_ProductDescription"]);
                    objProd.PrdSlNo = null;
                    objProd.PreTaxVal = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Amount"]).ToString("0.00"));
                    objProd.Qty = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Quantity"]).ToString("0.000"));
                    objProd.SgstAmt = Convert.ToDecimal(Convert.ToDecimal(dr["SGSTAmount"]).ToString("0.00"));
                    objProd.SlNo = Convert.ToString(dr["SL"]);
                    objProd.StateCesAmt = 0.00M;
                    objProd.StateCesNonAdvlAmt = 0.00M;
                    objProd.StateCesRt = 0.00M;
                    objProd.TotAmt = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_Amount"]).ToString("0.00"));
                    objProd.TotItemVal = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_TotalAmountInBaseCurrency"]).ToString("0.00")); ;
                    if (Convert.ToString(dr["GST_Print_Name"]) != "")
                        objProd.Unit = Convert.ToString(dr["GST_Print_Name"]);
                    else
                        objProd.Unit = "BAG";
                    objProd.UnitPrice = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_SalePrice"]).ToString("0.00")); ;
                    objListProd.Add(objProd);
                }
                objInvoice.ItemList = objListProd;

                obj.Add(objInvoice);
                objEnrich.payload = obj;
                authtokensOutput authObj = new authtokensOutput();

                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                           SecurityProtocolType.Tls11 |
                                           SecurityProtocolType.Tls12;
                        authtokensInput objI = new authtokensInput("shivkumar@peekay.co.in", "PeekaY@.!_123");
                        var json = JsonConvert.SerializeObject(objI, Formatting.Indented);
                        var stringContent = new StringContent(json);
                        var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                        var response = client.PostAsync("https://sandbox.services.vayananet.com/theodore/apis/v1/authtokens", stringContent).Result;

                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var jsonString = response;
                            var data = JsonConvert.DeserializeObject<authtokensOutput>(response.Content.ReadAsStringAsync().Result);
                            authObj = response.Content.ReadAsAsync<authtokensOutput>().Result;

                        }
                    }
                }
                catch (AggregateException err)
                {
                    foreach (var errInner in err.InnerExceptions)
                    {

                    }
                }

                try
                {
                    IRNEnrich objIRN = new IRNEnrich();
                    using (var client = new HttpClient())
                    {
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                        SecurityProtocolType.Tls11 |
                        SecurityProtocolType.Tls12;
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/zip"));
                        var json = JsonConvert.SerializeObject(objEnrich, Formatting.Indented);
                        var stringContent = new StringContent(json);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-USER-TOKEN", authObj.data.token);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-ORG-ID", "d16184ae-1699-495c-b276-14c4408e76ba");
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSTIN", "19AABCP5428M1Z0");
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-USERNAME", "PEEKAYAGENCIES");
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-PWD", "Shiv@2709");
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSP-CODE", "clayfin");
                        var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                        var response = client.PostAsync("https://solo.enriched-api.vayana.com/enriched/einv/v1.0/nic/invoices", stringContent).Result;
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var jsonString = response.Content.ReadAsStringAsync().Result;

                            objIRN = response.Content.ReadAsAsync<IRNEnrich>().Result;
                            //TaskModel objIRNDetails = new TaskModel();
                            //using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(objIRN.data)))
                            //{
                            //    // Deserialization from JSON  
                            //    DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(IRNDetails));
                            //    objIRNDetails = (TaskModel)deserializer.ReadObject(ms);
                            //}
                            client.DefaultRequestHeaders.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-USER-TOKEN", authObj.data.token);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-ORG-ID", "d16184ae-1699-495c-b276-14c4408e76ba");
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSTIN", "19AABCP5428M1Z0");
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-USERNAME", "PEEKAYAGENCIES");
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-PWD", "Shiv@2709");
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSP-CODE", "clayfin");
                            var response2 = client.GetStringAsync("https://solo.enriched-api.vayana.com/enriched/tasks/v1.0/status/" + objIRN.data.task_id).Result;
                            client.DefaultRequestHeaders.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/zip"));
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-USER-TOKEN", authObj.data.token);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-ORG-ID", "d16184ae-1699-495c-b276-14c4408e76ba");
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSTIN", "19AABCP5428M1Z0");
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-USERNAME", "PEEKAYAGENCIES");
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-PWD", "Shiv@2709");
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSP-CODE", "clayfin");
                            var response1 = client.GetAsync("https://solo.enriched-api.vayana.com/enriched/tasks/v1.0/download/" + objIRN.data.task_id).Result;
                            //var file = client.GetStreamAsync("https://solo.enriched-api.vayana.com/enriched/tasks/v1.0/download/" + objIRN.data.task_id).Result;
                            //var response = await client.GetAsync(uri);
                            using (var fs = new FileStream(
                                HostingEnvironment.MapPath(string.Format("~/Commonfolder/{0}.zip", id.ToString())),
                                FileMode.CreateNew))
                            {
                                response1.Content.CopyToAsync(fs);
                            }
                            client.DefaultRequestHeaders.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-USER-TOKEN", authObj.data.token);
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-ORG-ID", "d16184ae-1699-495c-b276-14c4408e76ba");
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSTIN", "19AABCP5428M1Z0");
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-USERNAME", "PEEKAYAGENCIES");
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-PWD", "Shiv@2709");
                            client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSP-CODE", "clayfin");
                            var response3 = client.GetStringAsync("https://solo.enriched-api.vayana.com/enriched/tasks/v1.0/result/" + objIRN.data.task_id).Result;

                            grid.JSProperties["cpSucessIRN"] = "Yes";
                        }
                        else
                        {
                            var jsonString = response.Content.ReadAsStringAsync().Result;
                            grid.JSProperties["cpSucessIRN"] = "No";
                        }


                    }
                }
                catch (AggregateException err)
                {
                    foreach (var errInner in err.InnerExceptions)
                    {
                        grid.JSProperties["cpSucessIRN"] = "No";
                    }
                }
            }

        }
        //public void ModifySalesReturn(string QuotationID, string strSchemeType, string strQuoteNo, string strQuoteDate, string strCustomer, string strContactName, Int64 ProjId,
        //                            string Reference, string PosForGst, string strBranch, string strAgents, string strCurrency, string strRate, string strTaxType, string strTaxCode,
        //                            string strInvoiceComponent, string strInvoiceComponentDate, string strCashBank, string strDueDate,
        //                            DataTable Productdt, DataTable TaxDetailTable, DataTable Warehousedt, DataTable SalesReturnTaxdt, DataTable BillAddressdt, string approveStatus, string ActionType,
        //                            ref int strIsComplete, ref int strInvoiceID, string ReasonforReturn, DataTable PackingDetailsdt, DataTable MultiUOMDetails)
        public void ModifySalesReturn(string QuotationID, string strSchemeType,string SchemeId, string Adjustment_No, string strQuoteDate, string strCustomer, string strContactName, Int64 ProjId,
                                  string Reference, string PosForGst, string strBranch, string strAgents, string strCurrency, string strRate, string strTaxType, string strTaxCode,DataTable dtAddlDesc,
                                  string strInvoiceComponent, string strInvoiceComponentDate, string strCashBank, string strDueDate,
                                  DataTable Productdt, DataTable TaxDetailTable, DataTable Warehousedt, DataTable SalesReturnTaxdt, DataTable BillAddressdt, string approveStatus, string ActionType,
                                  ref int strIsComplete, ref int strInvoiceID, string ReasonforReturn, DataTable PackingDetailsdt, DataTable MultiUOMDetails, string drdTransCategory)
        {
            try
            {                
                string DriverName = string.Empty;
                string PhoneNo = string.Empty;
                string VehicleNo = string.Empty;
                DataSet dsInst = new DataSet();              
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                // Mantis Issue 24831
                //SqlCommand cmd = new SqlCommand("prc_CRMSalesReturnManual_AddEdit", con);
                SqlCommand cmd = new SqlCommand("prc_SalesReturnManual_AddEdit_New", con);
                // End of Mantis Issue 24831
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", ActionType);
                cmd.Parameters.AddWithValue("@approveStatus", approveStatus);
                cmd.Parameters.AddWithValue("@SalesReturnID", QuotationID);               
                cmd.Parameters.AddWithValue("@SchemeId", SchemeId);
                cmd.Parameters.AddWithValue("@Adjustment_No", Adjustment_No);
                cmd.Parameters.AddWithValue("@SalesReturnDate", Convert.ToDateTime(strQuoteDate));
                cmd.Parameters.AddWithValue("@BranchID", strBranch);
                cmd.Parameters.AddWithValue("@CustomerID", strCustomer);
                cmd.Parameters.AddWithValue("@ContactPerson", strContactName);
                cmd.Parameters.AddWithValue("@Reference", Reference);
                cmd.Parameters.AddWithValue("@PosForGst", PosForGst);
                cmd.Parameters.AddWithValue("@Agents", strAgents);
                cmd.Parameters.AddWithValue("@ComponentType", "SRM");              
                cmd.Parameters.AddWithValue("@InvoiceComponent", strInvoiceComponent);
                if (String.IsNullOrEmpty(strInvoiceComponentDate))
                { cmd.Parameters.AddWithValue("@InvoiceComponentDate", strInvoiceComponentDate); }
                else
                {
                    DateTime dt = DateTime.ParseExact(strInvoiceComponentDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    cmd.Parameters.AddWithValue("@InvoiceComponentDate", Convert.ToDateTime(dt).ToString("yyyy-MM-dd"));
                }
                cmd.Parameters.AddWithValue("@CashBank", strCashBank);
                cmd.Parameters.AddWithValue("@DueDate", Convert.ToDateTime(strDueDate));
                cmd.Parameters.AddWithValue("@Currency", strCurrency);
                cmd.Parameters.AddWithValue("@Rate", strRate);
                cmd.Parameters.AddWithValue("@TaxType", strTaxType);
                cmd.Parameters.AddWithValue("@TaxCode", strTaxCode);
                cmd.Parameters.AddWithValue("@CompanyID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FinYear", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(Session["userid"]));
                cmd.Parameters.AddWithValue("@ProductDetails", Productdt);
                cmd.Parameters.AddWithValue("@TaxDetail", TaxDetailTable);
                if (Session["SRwarehousedetailstemp"] != null)
                {
                    DataTable temtable = new DataTable();
                    DataTable Warehousedtssss = (DataTable)Session["SRwarehousedetailstemp"];
                    temtable = Warehousedtssss.DefaultView.ToTable(false, "SrlNo", "BatchWarehouseID", "BatchWarehousedetailsID", "BatchID", "SerialID", "WarehouseID", "WarehouseName", "BatchNo", "SerialNo", "MFGDate", "ExpiryDate", "Quantitysum", "productid", "Inventrytype", "StockID", "isnew");
                    cmd.Parameters.AddWithValue("@udt_StockOpeningwarehousentrie", temtable);
                }

                cmd.Parameters.AddWithValue("@ReturnPackingDetails", PackingDetailsdt); //Surojit 14-03-2019               
                cmd.Parameters.AddWithValue("@SalesReturnTax", SalesReturnTaxdt);
                cmd.Parameters.AddWithValue("@BillAddress", BillAddressdt);
                cmd.Parameters.AddWithValue("@udt_AddlDesc", dtAddlDesc);
                cmd.Parameters.AddWithValue("@VehicleNo", VehicleNo);
                cmd.Parameters.AddWithValue("@DriverName", DriverName);
                cmd.Parameters.AddWithValue("@PhoneNo", PhoneNo);
                cmd.Parameters.AddWithValue("@Project_Id", ProjId);
                cmd.Parameters.AddWithValue("@MultiUOMDetails", MultiUOMDetails);
                cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 50);
                cmd.Parameters.Add("@ReturnSalesReturnID", SqlDbType.VarChar, 50);
                cmd.Parameters.Add("@isEinvoice", SqlDbType.VarChar, 50);

                cmd.Parameters.AddWithValue("@IsManual", 0);
                cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;
                cmd.Parameters["@ReturnSalesReturnID"].Direction = ParameterDirection.Output;
                cmd.Parameters["@isEinvoice"].Direction = ParameterDirection.Output;


                cmd.Parameters.AddWithValue("@ReasonforReturn", ReasonforReturn);
                cmd.Parameters.AddWithValue("@TransacCategory", drdTransCategory);
                SqlParameter rtnSaleReturnNo = new SqlParameter("@rtnSaleReturnNo", SqlDbType.VarChar, -1);
                rtnSaleReturnNo.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(rtnSaleReturnNo);
                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);

                strIsComplete = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value.ToString());
                strInvoiceID = Convert.ToInt32(cmd.Parameters["@ReturnSalesReturnID"].Value.ToString());
                grid.JSProperties["cpisEinvoice"] = Convert.ToString(cmd.Parameters["@isEinvoice"].Value.ToString());
                cmd.Dispose();
                con.Dispose();
                if (strIsComplete == 1)
                {
                    grid.JSProperties["cpQuotationNo"] = rtnSaleReturnNo.Value;
                }
                    #region warehouse Update and delete

                updatewarehouse();
                deleteALL();

                #endregion
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

        #endregion

        #region Quotation Tax Details

        public IEnumerable GetTaxes()
        {
            List<Taxes> TaxList = new List<Taxes>();
         //   BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
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
                        if (Convert.ToString(ddl_VatGstCst.Value) == "0~0~X")
                        {
                            if (Convert.ToString(DT.Rows[i]["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(DT.Rows[i]["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(DT.Rows[i]["TaxTypeCode"]).Trim() == "SGST")
                            {
                                decimal finalCalCulatedOn = 0;
                                decimal backProcessRate = (1 + (totalParcentage / 100));
                                finalCalCulatedOn = Taxes.calCulatedOn / backProcessRate;
                                Taxes.calCulatedOn = Math.Round(finalCalCulatedOn, 2);
                            }
                        }
                    }


                    TaxList.Add(Taxes);
                }
            }

            return TaxList;
        }

        public DataTable GetSalesReturnManualProjectCode(string Proj_Code)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesReturn_Details");
            proc.AddVarcharPara("@Action", 500, "GetProjectCode");
            proc.AddVarcharPara("@Proj_Code", 200, Proj_Code);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetReturnManualProjectEditData()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesReturn_Details");
            proc.AddVarcharPara("@Action", 500, "ProjectReturnManualEditdata");
            proc.AddVarcharPara("@SalesReturnID", 500, Convert.ToString(Session["SRM_ReturnID"]));
            dt = proc.GetTable();
            return dt;
        }


        public DataTable GetTaxData(string quoteDate)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesReturn_Details");
            proc.AddVarcharPara("@Action", 500, "TaxDetails");
            proc.AddVarcharPara("@SalesReturnID", 500, Convert.ToString(Session["SRM_ReturnID"]));
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            // proc.AddVarcharPara("@branchId", 500, Convert.ToString(Session["userbranchID"]));
            proc.AddVarcharPara("@branchId", 500, Convert.ToString(ddl_Branch.SelectedValue));
            proc.AddVarcharPara("@companyId", 500, Convert.ToString(Session["LastCompany"]));
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
                if (Session["SRM_TaxDetails"] == null)
                {
                    Session["SRM_TaxDetails"] = GetTaxData(dt_PLQuote.Date.ToString("yyyy-MM-dd"));
                }

                if (Session["SRM_TaxDetails"] != null)
                {
                    TaxDetailsdt = (DataTable)Session["SRM_TaxDetails"];

                    #region Delete Igst,Cgst,Sgst respectively
                    //Get Company Gstin 09032017
                    string CompInternalId = Convert.ToString(Session["LastCompany"]);
                    string[] compGstin = oDBEngine.GetFieldValue1("tbl_master_company", "cmp_gstin", "cmp_internalid='" + CompInternalId + "'", 1);
                    string ShippingState = "";





                    // Date: 31-05-2017    Author: Kallol Samanta  [START] 
                    // Details: Billing/Shipping user control integration 

                    //New Code
                    //chinmoy change below code 12-03-2019
                    string sstateCode = "";

                    //if (ddlPosGstReturnManual.Value.ToString() == "")
                    //{
                        if (ddlPosGstReturnManual.Value.ToString() == "S")
                        {
                         //   sstateCode = Purchase_BillingShipping.GeteShippingStateCode();
                            sstateCode = hdnPOSShippingStateCode.Value;
                        }
                        else
                        {
                           // sstateCode = Purchase_BillingShipping.GetBillingStateCode();
                            sstateCode = hdnPOSBillingStateCode.Value;
                        }
                 //   }
                    ShippingState = sstateCode;
                    if (ShippingState.Trim() != "")
                    {
                        ShippingState = ShippingState;
                    }

                    //Old Code               
                    //if (chkBilling.Checked)
                    //{
                    //    if (CmbState.Value != null)
                    //    {
                    //        ShippingState = CmbState.Text;
                    //        ShippingState = ShippingState.Substring(ShippingState.IndexOf("(State Code:")).Replace("(State Code:", "").Replace(")", "");
                    //    }
                    //}
                    //else
                    //{
                    //    if (CmbState1.Value != null)
                    //    {
                    //        ShippingState = CmbState1.Text;
                    //        ShippingState = ShippingState.Substring(ShippingState.IndexOf("(State Code:")).Replace("(State Code:", "").Replace(")", "");
                    //    }
                    //}

                    // Date: 31-05-2017    Author: Kallol Samanta  [END]











                    if (ShippingState.Trim() != "" && compGstin[0].Trim() != "")
                    {
                        if (compGstin.Length > 0)
                        {
                            if (compGstin[0].Substring(0, 2) == ShippingState)
                            {
                                //Check if the state is in union territories then only UTGST will apply
                                //   Chandigarh     Andaman and Nicobar Islands    DADRA & NAGAR HAVELI    DAMAN & DIU    Lakshadweep              PONDICHERRY
                                if (ShippingState == "4" || ShippingState == "26" || ShippingState == "25" || ShippingState == "35" || ShippingState == "31" || ShippingState == "34")
                                {
                                    foreach (DataRow dr in TaxDetailsdt.Rows)
                                    {
                                        if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST")
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
                    gridTax.JSProperties["cpTotalCharges"] = ClculatedTotalCharge(taxChargeDataSource);
                }
            }
            else if (strSplitCommand == "SaveGst")
            {
                DataTable TaxDetailsdt = new DataTable();
                if (Session["SRM_TaxDetails"] != null)
                {
                    TaxDetailsdt = (DataTable)Session["SRM_TaxDetails"];
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

                    Session["SRM_TaxDetails"] = TaxDetailsdt;
                }
            }
        }
        protected decimal ClculatedTotalCharge(List<Taxes> taxChargeDataSource)
        {
            decimal totalCharges = 0;
            foreach (Taxes txObj in taxChargeDataSource)
            {

                if (Convert.ToString(txObj.TaxName).Contains("(+)"))
                {
                    totalCharges += Convert.ToDecimal(txObj.Amount);
                }
                else
                {
                    totalCharges -= Convert.ToDecimal(txObj.Amount);
                }

            }
            totalCharges += Convert.ToDecimal(txtGstCstVatCharge.Text);

            return totalCharges;

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
            if (Session["SRM_TaxDetails"] != null)
            {
                TaxDetailsdt = (DataTable)Session["SRM_TaxDetails"];
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

            Session["SRM_TaxDetails"] = TaxDetailsdt;

            gridTax.DataSource = GetTaxes(TaxDetailsdt);
            gridTax.DataBind();
        }
        protected void gridTax_DataBinding(object sender, EventArgs e)
        {
            if (Session["SRM_TaxDetails"] != null)
            {
                DataTable TaxDetailsdt = (DataTable)Session["SRM_TaxDetails"];

                //gridTax.DataSource = GetTaxes();
                var taxlist = (List<Taxes>)GetTaxes(TaxDetailsdt);
                var taxChargeDataSource = setChargeCalculatedOn(taxlist, TaxDetailsdt);
                gridTax.DataSource = taxChargeDataSource;
            }
        }


        #endregion


        #region Subhabrata-Products
        protected void Productgrid_RowInserting(object sender, ASPxDataInsertingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void Productgrid_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void Productgrid_RowDeleting(object sender, ASPxDataDeletingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void aspxGridProduct_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {

        }
        #endregion
        #region warehousepopup
        protected void GrdWarehousePC_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string strSplitCommand = e.Parameters.Split('~')[0];


            if (hdniswarehouse.Value == "true")
            {
                GrdWarehousePC.Columns["viewWarehouseName"].Visible = true;
                GrdWarehousePC.Columns["Quantity"].Visible = true;
                GrdWarehousePC.Columns["viewQuantity"].Visible = false;

            }
            else
            {
                GrdWarehousePC.Columns["viewWarehouseName"].Visible = false;
                GrdWarehousePC.Columns["Quantity"].Visible = false;
                GrdWarehousePC.Columns["viewQuantity"].Visible = true;
            }
            if (hdnisbatch.Value == "true")
            {
                GrdWarehousePC.Columns["viewBatchNo"].Visible = true;
                GrdWarehousePC.Columns["viewMFGDate"].Visible = true;
                GrdWarehousePC.Columns["viewExpiryDate"].Visible = true;
                GrdWarehousePC.Columns["Quantity"].Visible = false;
                GrdWarehousePC.Columns["viewQuantity"].Visible = true;

            }
            else
            {
                GrdWarehousePC.Columns["viewBatchNo"].Visible = false;
                GrdWarehousePC.Columns["viewMFGDate"].Visible = false;
                GrdWarehousePC.Columns["viewExpiryDate"].Visible = false;
                GrdWarehousePC.Columns["Quantity"].Visible = true;
                GrdWarehousePC.Columns["viewQuantity"].Visible = false;
            }
            if (hdnisserial.Value == "true")
            {
                GrdWarehousePC.Columns["viewSerialNo"].Visible = true;
                GrdWarehousePC.Columns["Quantity"].Visible = false;
                GrdWarehousePC.Columns["viewQuantity"].Visible = true;
            }
            else
            {
                GrdWarehousePC.Columns["viewSerialNo"].Visible = false;
            }


            #region Savenew
            if (strSplitCommand == "Display")
            {
                string WarehouseName = Convert.ToString(e.Parameters.Split('~')[2]);

                string WarehouseID = Convert.ToString(e.Parameters.Split('~')[1]).Trim();

                string BatchName = Convert.ToString(e.Parameters.Split('~')[3]);

                string SerialName = Convert.ToString(e.Parameters.Split('~')[4]);
                string ProductID = Convert.ToString(hdfProductIDPC.Value);
                string stockid = Convert.ToString(hdfstockidPC.Value);
                decimal openingstock = Convert.ToDecimal(txtqnty.Text);
                string branchid = Convert.ToString(hdbranchIDPC.Value);
                string qnty = Convert.ToString(e.Parameters.Split('~')[5]);

                decimal totalopeining = Convert.ToDecimal(hdfopeningstockPC.Value);
                decimal oldtotalopeining = Convert.ToDecimal(oldopeningqntity.Value);

                if (qnty == "0.0000" && openingstock <= 0)
                {
                    qnty = batchqnty.Text;
                    openingstock = Convert.ToDecimal(qnty);
                }

                if (!string.IsNullOrEmpty(BatchName))
                {
                    BatchName = BatchName.Trim();
                }
                if (!string.IsNullOrEmpty(SerialName))
                {
                    SerialName = SerialName.Trim();
                }
                if (WarehouseID == "null")
                {
                    WarehouseID = "0";
                }

                if (Convert.ToDecimal(openingstock) > totalopeining)
                {
                    GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("You have entered Quantity greater than Opening Quantity. Cannot Proceed.");

                }
                else
                {
                    DataTable Warehousedt = new DataTable();
                    DataTable Warehousedtnew = new DataTable();
                    if (Session["SRM_WarehouseData"] != null)
                    {
                        Warehousedt = (DataTable)Session["SRM_WarehouseData"];
                    }
                    else
                    {
                        //  Warehousedt.Columns.Add("Product_SrlNo", typeof(string));
                        Warehousedt.Columns.Add("SrlNo", typeof(Int32));
                        Warehousedt.Columns.Add("WarehouseID", typeof(string));
                        Warehousedt.Columns.Add("WarehouseName", typeof(string));

                        Warehousedt.Columns.Add("BatchNo", typeof(string));
                        Warehousedt.Columns.Add("SerialNo", typeof(string));

                        Warehousedt.Columns.Add("MFGDate", typeof(DateTime));
                        Warehousedt.Columns.Add("ExpiryDate", typeof(DateTime));
                        Warehousedt.Columns.Add("Quantity", typeof(decimal));

                        Warehousedt.Columns.Add("BatchWarehouseID", typeof(string));
                        Warehousedt.Columns.Add("BatchWarehousedetailsID", typeof(string));
                        Warehousedt.Columns.Add("BatchID", typeof(string));
                        Warehousedt.Columns.Add("SerialID", typeof(string));


                        Warehousedt.Columns.Add("viewWarehouseName", typeof(string));

                        Warehousedt.Columns.Add("viewBatchNo", typeof(string));
                        Warehousedt.Columns.Add("viewQuantity", typeof(string));
                        Warehousedt.Columns.Add("viewSerialNo", typeof(string));



                        Warehousedt.Columns.Add("viewMFGDate", typeof(DateTime));
                        Warehousedt.Columns.Add("viewExpiryDate", typeof(DateTime));

                        Warehousedt.Columns.Add("Quantitysum", typeof(decimal));
                        Warehousedt.Columns.Add("isnew", typeof(string));
                        Warehousedt.Columns.Add("productid", typeof(string));
                        Warehousedt.Columns.Add("Inventrytype", typeof(string));
                        Warehousedt.Columns.Add("StockID", typeof(string));
                        Warehousedt.Columns.Add("pcslno", typeof(Int32));

                    }



                    DateTime dtmgh = txtmkgdate.Date;
                    DateTime dtexp = txtexpirdate.Date;

                    string isedited = hdnisedited.Value;
                    decimal inputqnty = 0;
                    int noofserial = 0;
                    int oldrowcount = Convert.ToInt32(hdnoldrowcount.Value);
                    int newrowcount = 0;
                    decimal hdntotalqntyPCs = Convert.ToDecimal(hdntotalqntyPC.Value);


                    if (Session["SRM_WarehouseData"] != null)
                    {
                        DataTable Warehousedts = (DataTable)Session["SRM_WarehouseData"];

                        newrowcount = Warehousedts.Select("isnew = 'new'").Count<DataRow>();

                        if (newrowcount != null && hdnisserial.Value == "true")
                        {

                            var inputqntys = Warehousedts.Select("WarehouseName= '" + WarehouseName + "' AND BatchNo = '" + BatchName + "' AND isnew = 'new'").Count<DataRow>();
                            if (inputqntys != null && !string.IsNullOrEmpty(Convert.ToString(inputqntys)))
                            {
                                noofserial = Convert.ToInt32(inputqntys + 1);

                            }
                            else
                            {
                                noofserial = 0;
                            }
                        }
                    }

                    if (hidencountforserial.Value != "2")
                    {
                        if (Warehousedt.Rows.Count > 0)
                        {
                            var inputqntys = Warehousedt.Compute("sum(Quantitysum)", "isnew = 'new'");
                            if (inputqntys != null && !string.IsNullOrEmpty(Convert.ToString(inputqntys)))
                            {
                                inputqnty = Convert.ToDecimal(inputqntys);

                            }
                            else
                            {
                                inputqnty = 0;
                            }

                        }
                        //commentout below line for it should not add.
                        if (hdnisserial.Value == "false" && hdniswarehouse.Value == "true" && hdnisbatch.Value == "false")
                        {
                            inputqnty = inputqnty + Convert.ToDecimal(openingstock);
                        }
                        if (hdnisserial.Value == "false" && hdniswarehouse.Value == "true" && hdnisbatch.Value == "true")
                        {
                            inputqnty = inputqnty + Convert.ToDecimal(openingstock);
                        }
                        if (hdnisserial.Value == "false" && hdniswarehouse.Value == "false" && hdnisbatch.Value == "true")
                        {
                            inputqnty = inputqnty + Convert.ToDecimal(openingstock);
                        }


                    }

                    //checking quantity only for warehouse is true.
                    if (inputqnty == 0 && hdnisserial.Value == "false")
                    {
                        inputqnty = Convert.ToDecimal(openingstock);
                    }

                    if (hidencountforserial.Value == "1" && hdnisserial.Value == "true" && noofserial != openingstock && hdnisbatch.Value == "true" && hdniswarehouse.Value == "true")
                    {
                        GrdWarehousePC.JSProperties["cpupdatemssgserialsetdisblebatch"] = Convert.ToString("disable");
                    }
                    if (hidencountforserial.Value == "2" && hdnisserial.Value == "true" && noofserial == openingstock && hdnisbatch.Value == "true" && hdniswarehouse.Value == "true")
                    {
                        GrdWarehousePC.JSProperties["cpupdatemssgserialsetenablebatch"] = Convert.ToString("enable");
                    }
                    if (hidencountforserial.Value == "1" && hdnisserial.Value == "true" && noofserial != openingstock && hdnisbatch.Value == "false" && hdniswarehouse.Value == "true")
                    {
                        GrdWarehousePC.JSProperties["cpupdatemssgserialsetdisblebatch"] = Convert.ToString("disable");
                    }
                    if (hidencountforserial.Value == "2" && hdnisserial.Value == "true" && noofserial == openingstock && hdnisbatch.Value == "false" && hdniswarehouse.Value == "true")
                    {
                        GrdWarehousePC.JSProperties["cpupdatemssgserialsetenablebatch"] = Convert.ToString("enable");
                    }

                    ///batch and serial only
                    if (hidencountforserial.Value == "1" && hdnisserial.Value == "true" && noofserial != openingstock && hdnisbatch.Value == "true" && hdniswarehouse.Value == "false")
                    {
                        GrdWarehousePC.JSProperties["cpupdatemssgserialsetdisblebatch"] = Convert.ToString("disable");
                    }

                    if (hidencountforserial.Value == "2" && hdnisserial.Value == "true" && noofserial == openingstock && hdnisbatch.Value == "true" && hdniswarehouse.Value == "false")
                    {
                        GrdWarehousePC.JSProperties["cpupdatemssgserialsetenablebatch"] = Convert.ToString("enable");
                    }

                    ///end batch and serial only



                    //

                    //if (hdnisserial.Value == "true" && ((Convert.ToDecimal(Convert.ToString((oldrowcount - newrowcount)).Replace('-', '0')) == hdntotalqntyPCs) || (Convert.ToDecimal(Convert.ToString((oldrowcount - newrowcount)).Replace('-', '0')) == (hdntotalqntyPCs * Convert.ToDecimal(hdnbatchchanged.Value)))) && ((hdniswarehouse.Value == "true" && hdnisbatch.Value == "true") && (hdnoldwarehousname.Value == WarehouseName && hdnoldbatchno.Value == BatchName)) || (hdniswarehouse.Value == "true" && hdnisbatch.Value == "false") && (hdnoldwarehousname.Value == WarehouseName) || (hdniswarehouse.Value == "false" && hdnisbatch.Value == "true") && (hdnoldbatchno.Value == BatchName))
                    //{
                    //    GrdWarehousePC.JSProperties["cpupdatemssgserial"] = Convert.ToString("Please make sure quantity and no of Serial are equal or not.");
                    //}
                    if (hdnisserial.Value == "true" && noofserial > openingstock)
                    {
                        GrdWarehousePC.JSProperties["cpupdatemssgserial"] = Convert.ToString("Please make sure quantity and no of Serial are equal or not.");
                    }
                    else
                    {

                        if (inputqnty <= totalopeining || isedited == "true")
                        {

                            if (hdniswarehouse.Value == "true" && hdnisbatch.Value == "true" && hidencountforserial.Value == "2" && hdnisserial.Value == "true" && hdnoldwarehousname.Value == WarehouseName && hdnoldbatchno.Value == BatchName)
                            {
                                if (Convert.ToString(dtmgh) != "1/1/0001 12:00:00 AM" && Convert.ToString(dtexp) != "1/1/0001 12:00:00 AM" && Convert.ToString(dtmgh) != "01-01-0001 12:00:00 AM" && Convert.ToString(dtexp) != "01-01-0001 12:00:00 AM")
                                {

                                    Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, WarehouseID, WarehouseName, BatchName, SerialName, dtmgh, dtexp, openingstock, "0", "0", "0", "0", "", "", "", SerialName, null, null, 0, "new", Convert.ToString(hdfProductIDPC.Value), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                                    GrdWarehousePC.JSProperties["cpinsertmssgserial"] = Convert.ToString("Inserted.");

                                }
                                else
                                {
                                    Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, WarehouseID, WarehouseName, BatchName, SerialName, null, null, openingstock, "0", "0", "0", "0", "", "", "", SerialName, null, null, 0, "new", Convert.ToString(hdfProductIDPC.Value), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));

                                    GrdWarehousePC.JSProperties["cpinsertmssgserial"] = Convert.ToString("Inserted.");

                                }
                            }
                            else if (hdniswarehouse.Value == "false" && hdnisbatch.Value == "false" && hdnisserial.Value == "true")
                            {

                                if (Convert.ToString(dtmgh) != "1/1/0001 12:00:00 AM" && Convert.ToString(dtexp) != "1/1/0001 12:00:00 AM" && Convert.ToString(dtmgh) != "01-01-0001 12:00:00 AM" && Convert.ToString(dtexp) != "01-01-0001 12:00:00 AM")
                                {

                                    Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, WarehouseID, WarehouseName, BatchName, SerialName, dtmgh, dtexp, openingstock, "0", "0", "0", "0", WarehouseName, BatchName, openingstock, SerialName, dtmgh, dtexp, openingstock, "new", Convert.ToString(hdfProductIDPC.Value), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                                    GrdWarehousePC.JSProperties["cpinsertmssgserial"] = Convert.ToString("Inserted.");
                                }
                                else
                                {
                                    Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, WarehouseID, WarehouseName, BatchName, SerialName, null, null, openingstock, "0", "0", "0", "0", WarehouseName, BatchName, openingstock, SerialName, null, null, openingstock, "new", Convert.ToString(hdfProductIDPC.Value), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                                    GrdWarehousePC.JSProperties["cpinsertmssgserial"] = Convert.ToString("Inserted.");

                                }

                            }
                            else if (hdniswarehouse.Value == "true" && hidencountforserial.Value == "2" && hdnisbatch.Value == "false" && hdnisserial.Value == "true")
                            {

                                if (Convert.ToString(dtmgh) != "1/1/0001 12:00:00 AM" && Convert.ToString(dtexp) != "1/1/0001 12:00:00 AM" && Convert.ToString(dtmgh) != "01-01-0001 12:00:00 AM" && Convert.ToString(dtexp) != "01-01-0001 12:00:00 AM")
                                {

                                    Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, WarehouseID, WarehouseName, BatchName, SerialName, dtmgh, dtexp, openingstock, "0", "0", "0", "0", "", "", "", SerialName, null, null, 0, "new", Convert.ToString(hdfProductIDPC.Value), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                                    GrdWarehousePC.JSProperties["cpinsertmssgserial"] = Convert.ToString("Inserted.");

                                }
                                else
                                {
                                    Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, WarehouseID, WarehouseName, BatchName, SerialName, null, null, openingstock, "0", "0", "0", "0", "", "", "", SerialName, null, null, 0, "new", Convert.ToString(hdfProductIDPC.Value), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));

                                    GrdWarehousePC.JSProperties["cpinsertmssgserial"] = Convert.ToString("Inserted.");

                                }
                            }
                            //batch only with serial

                            else if (hdniswarehouse.Value == "false" && hidencountforserial.Value == "2" && hdnisbatch.Value == "true" && hdnisserial.Value == "true")
                            {

                                if (Convert.ToString(dtmgh) != "1/1/0001 12:00:00 AM" && Convert.ToString(dtexp) != "1/1/0001 12:00:00 AM" && Convert.ToString(dtmgh) != "01-01-0001 12:00:00 AM" && Convert.ToString(dtexp) != "01-01-0001 12:00:00 AM")
                                {

                                    Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, WarehouseID, WarehouseName, BatchName, SerialName, dtmgh, dtexp, openingstock, "0", "0", "0", "0", "", "", "", SerialName, null, null, 0, "new", Convert.ToString(hdfProductIDPC.Value), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                                    GrdWarehousePC.JSProperties["cpinsertmssgserial"] = Convert.ToString("Inserted.");

                                }
                                else
                                {
                                    Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, WarehouseID, WarehouseName, BatchName, SerialName, null, null, openingstock, "0", "0", "0", "0", "", "", "", SerialName, null, null, 0, "new", Convert.ToString(hdfProductIDPC.Value), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));

                                    GrdWarehousePC.JSProperties["cpinsertmssgserial"] = Convert.ToString("Inserted.");

                                }
                            }
                            //batch only.
                            else
                            {

                                if (Convert.ToString(dtmgh) != "1/1/0001 12:00:00 AM" && Convert.ToString(dtexp) != "1/1/0001 12:00:00 AM" && Convert.ToString(dtmgh) != "01-01-0001 12:00:00 AM" && Convert.ToString(dtexp) != "01-01-0001 12:00:00 AM")
                                {

                                    Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, WarehouseID, WarehouseName, BatchName, SerialName, dtmgh, dtexp, openingstock, "0", "0", "0", "0", WarehouseName, BatchName, openingstock, SerialName, dtmgh, dtexp, openingstock, "new", Convert.ToString(hdfProductIDPC.Value), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));

                                }
                                else
                                {
                                    Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, WarehouseID, WarehouseName, BatchName, SerialName, null, null, openingstock, "0", "0", "0", "0", WarehouseName, BatchName, openingstock, SerialName, null, null, openingstock, "new", Convert.ToString(hdfProductIDPC.Value), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));


                                }
                                if (hdnisserial.Value == "true")
                                {
                                    GrdWarehousePC.JSProperties["cpinsertmssg"] = Convert.ToString("Inserted.");
                                }
                                if (hdnisserial.Value == "false" && hdniswarehouse.Value == "true" && hdnisbatch.Value == "true")
                                {
                                    GrdWarehousePC.JSProperties["cpbatchinsertmssg"] = Convert.ToString("Inserted.");
                                }

                            }


                            Session["SRM_WarehouseData"] = Warehousedt;


                            GrdWarehousePC.DataSource = Warehousedt.DefaultView;
                            GrdWarehousePC.DataBind();
                        }
                        else
                        {
                            GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("You have entered Quantity greater than Opening Quantity. Cannot Proceed.");
                        }
                    }
                }
                //
            }
            #endregion
            #region SaveAll
            if (strSplitCommand == "Saveall")
            {
                if (Session["SRM_WarehouseData"] == null && hdnisolddeleted.Value == "false")
                {
                    if (!string.IsNullOrEmpty(hdnvalue.Value) && !string.IsNullOrEmpty(hdnrate.Value))
                    {

                    }
                    else
                    {
                        GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("Nothing to Saved .");
                    }

                }
                else if (Session["SRM_WarehouseData"] == null && hdnisolddeleted.Value == "true")
                {

                    deleteALL();
                    GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("Saved Successfully.");
                }
                else
                {
                    string ProductID = Convert.ToString(hdfProductIDPC.Value);
                    string stockid = Convert.ToString(hdfstockidPC.Value);
                    string openingstock = Convert.ToString(hdfopeningstockPC.Value);
                    string branchid = Convert.ToString(hdbranchIDPC.Value);
                    string isolddeletd = hdnisolddeleted.Value;

                    int oldrowcount = Convert.ToInt32(hdnoldrowcount.Value);
                    int newrowcount = 0;
                    int updaterows = 0;
                    int deleted = 0;
                    decimal hdntotalqntyPCs = Convert.ToDecimal(hdntotalqntyPC.Value);

                    DataTable Warehousedts = (DataTable)Session["SRM_WarehouseData"];

                    newrowcount = Warehousedts.Select("isnew = 'new'").Count<DataRow>();
                    updaterows = Warehousedts.Select("isnew = 'Updated'").Count<DataRow>();


                    if (newrowcount != 0 || updaterows != 0 || Session["SRWarehouseDataDelete"] != null)
                    {
                        decimal inputqnty = 0;
                        decimal totalopeining = Convert.ToDecimal(hdfopeningstockPC.Value);
                        decimal oldtotalopeining = Convert.ToDecimal(oldopeningqntity.Value);

                        var inputqntys = Warehousedts.Compute("sum(Quantitysum)", "isnew = 'new'");
                        var updateqnty = Warehousedts.Compute("sum(Quantitysum)", "isnew = 'Updated'");
                        var oldeqnty = Warehousedts.Compute("sum(Quantitysum)", "isnew = 'old'");
                        decimal deletd = Convert.ToDecimal(hdndeleteqnity.Value);

                        if (inputqntys != null && !string.IsNullOrEmpty(Convert.ToString(inputqntys)))
                        {
                            inputqnty = Convert.ToDecimal(inputqntys);

                        }
                        if (updateqnty != null && !string.IsNullOrEmpty(Convert.ToString(updateqnty)))
                        {
                            inputqnty = inputqnty + Convert.ToDecimal(updateqnty);


                        }
                        if (oldeqnty != null && !string.IsNullOrEmpty(Convert.ToString(oldeqnty)))
                        {
                            inputqnty = inputqnty + Convert.ToDecimal(oldeqnty);


                        }
                        //commnet for allowing reduce from main page
                        //if ((oldeqnty != null && !string.IsNullOrEmpty(Convert.ToString(oldeqnty))) || (updateqnty != null && !string.IsNullOrEmpty(Convert.ToString(updateqnty))))
                        //{

                        //    //totalopeining = totalopeining + oldtotalopeining; //commnet for allowing reduce from main page
                        //    //if (hdnisreduing.Value == "true")
                        //    //{
                        //    //    totalopeining = oldtotalopeining;
                        //    //}
                        //    //else
                        //    //{
                        //    //    totalopeining = totalopeining + oldtotalopeining;
                        //    //}
                        //    totalopeining = oldtotalopeining;
                        //}
                        //if (deletd > 0 & isolddeletd == "true")
                        //{
                        //    totalopeining = Convert.ToDecimal(oldopeningqntity.Value) - deletd;
                        //}

                        if (inputqnty == totalopeining)
                        {
                            //if (hdnisserial.Value == "true" && ((Convert.ToDecimal(Convert.ToString((oldrowcount - newrowcount)).Replace('-', '0')) == hdntotalqntyPCs) || (Convert.ToDecimal(Convert.ToString((oldrowcount - newrowcount)).Replace('-', '0')) == (hdntotalqntyPCs * Convert.ToDecimal(hdnbatchchanged.Value)))))
                            if (hdnisserial.Value == "true" && hdnisbatch.Value == "true" && hdniswarehouse.Value == "true")
                            {

                                if (ProductID != "0" && branchid != "0")
                                {
                                    int output = Insertupdatedata(ProductID, stockid, branchid);
                                    if (output > 0)
                                    {
                                        DataTable Warehousedt = new DataTable();
                                        Warehousedt = GetRecord(stockid);
                                        if (Warehousedt.Rows.Count > 0)
                                        {
                                            Session["SRM_WarehouseData"] = Warehousedt;

                                            GrdWarehousePC.DataSource = Warehousedt.DefaultView;
                                            GrdWarehousePC.DataBind();
                                        }
                                        else
                                        {

                                            Session["SRM_WarehouseData"] = null;

                                            GrdWarehousePC.DataSource = null;
                                            GrdWarehousePC.DataBind();

                                        }

                                        GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("Saved Successfully.");

                                    }

                                }
                                else
                                {
                                    GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("Please try again null parameters.");
                                }
                            }
                            else if (hdnisserial.Value != "true" && hdnisbatch.Value != "true" && hdniswarehouse.Value == "true")
                            {
                                if (ProductID != "0" && branchid != "0")
                                {
                                    int output = Insertupdatedata(ProductID, stockid, branchid);
                                    if (output > 0)
                                    {
                                        DataTable Warehousedt = new DataTable();
                                        Warehousedt = GetRecord(stockid);
                                        if (Warehousedt.Rows.Count > 0)
                                        {
                                            Session["SRM_WarehouseData"] = Warehousedt;

                                            GrdWarehousePC.DataSource = Warehousedt.DefaultView;
                                            GrdWarehousePC.DataBind();
                                        }
                                        else
                                        {

                                            Session["SRM_WarehouseData"] = null;

                                            GrdWarehousePC.DataSource = null;
                                            GrdWarehousePC.DataBind();

                                        }

                                        GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("Saved Successfully.");

                                    }

                                }
                                else
                                {
                                    GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("Please try again null parameters.");
                                }
                            }
                            else if (hdnisserial.Value != "true" && hdnisbatch.Value == "true")
                            {
                                if (ProductID != "0" && branchid != "0")
                                {
                                    int output = Insertupdatedata(ProductID, stockid, branchid);
                                    if (output > 0)
                                    {
                                        DataTable Warehousedt = new DataTable();
                                        Warehousedt = GetRecord(stockid);
                                        if (Warehousedt.Rows.Count > 0)
                                        {
                                            Session["SRM_WarehouseData"] = Warehousedt;

                                            GrdWarehousePC.DataSource = Warehousedt.DefaultView;
                                            GrdWarehousePC.DataBind();
                                        }
                                        else
                                        {

                                            Session["SRM_WarehouseData"] = null;

                                            GrdWarehousePC.DataSource = null;
                                            GrdWarehousePC.DataBind();

                                        }

                                        GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("Saved Successfully.");

                                    }

                                }
                                else
                                {
                                    GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("Please try again null parameters.");
                                }

                            }
                            else if (hdnisserial.Value == "true" && hdnisbatch.Value != "true" && hdniswarehouse.Value != "true")
                            {
                                if (ProductID != "0" && branchid != "0")
                                {
                                    int output = Insertupdatedata(ProductID, stockid, branchid);
                                    if (output > 0)
                                    {
                                        DataTable Warehousedt = new DataTable();
                                        Warehousedt = GetRecord(stockid);
                                        if (Warehousedt.Rows.Count > 0)
                                        {
                                            Session["SRM_WarehouseData"] = Warehousedt;

                                            GrdWarehousePC.DataSource = Warehousedt.DefaultView;
                                            GrdWarehousePC.DataBind();
                                        }
                                        else
                                        {

                                            Session["SRM_WarehouseData"] = null;

                                            GrdWarehousePC.DataSource = null;
                                            GrdWarehousePC.DataBind();

                                        }

                                        GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("Saved Successfully.");

                                    }

                                }
                                else
                                {
                                    GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("Please try again null parameters.");
                                }
                            }
                            else if (hdnisserial.Value == "true" && hdnisbatch.Value != "true" && hdniswarehouse.Value == "true")
                            {

                                if (ProductID != "0" && branchid != "0")
                                {
                                    int output = Insertupdatedata(ProductID, stockid, branchid);
                                    if (output > 0)
                                    {
                                        DataTable Warehousedt = new DataTable();
                                        Warehousedt = GetRecord(stockid);
                                        if (Warehousedt.Rows.Count > 0)
                                        {
                                            Session["SRM_WarehouseData"] = Warehousedt;

                                            GrdWarehousePC.DataSource = Warehousedt.DefaultView;
                                            GrdWarehousePC.DataBind();
                                        }
                                        else
                                        {

                                            Session["SRM_WarehouseData"] = null;

                                            GrdWarehousePC.DataSource = null;
                                            GrdWarehousePC.DataBind();

                                        }

                                        GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("Saved Successfully.");

                                    }

                                }
                                else
                                {
                                    GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("Please try again null parameters.");
                                }
                            }
                            else if (hdnisserial.Value == "true" && hdnisbatch.Value == "true" && hdniswarehouse.Value != "true")
                            {

                                if (ProductID != "0" && branchid != "0")
                                {
                                    int output = Insertupdatedata(ProductID, stockid, branchid);
                                    if (output > 0)
                                    {
                                        DataTable Warehousedt = new DataTable();
                                        Warehousedt = GetRecord(stockid);
                                        if (Warehousedt.Rows.Count > 0)
                                        {
                                            Session["SRM_WarehouseData"] = Warehousedt;

                                            GrdWarehousePC.DataSource = Warehousedt.DefaultView;
                                            GrdWarehousePC.DataBind();
                                        }
                                        else
                                        {

                                            Session["SRM_WarehouseData"] = null;

                                            GrdWarehousePC.DataSource = null;
                                            GrdWarehousePC.DataBind();

                                        }

                                        GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("Saved Successfully.");

                                    }

                                }
                                else
                                {
                                    GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("Please try again null parameters.");
                                }
                            }
                            else
                            {
                                GrdWarehousePC.JSProperties["cpupdatemssgserial"] = Convert.ToString("Please make sure quantity and no of Serial are equal or not.");

                            }

                        }
                        else
                        {
                            GrdWarehousePC.JSProperties["cpupdatemssgserial"] = Convert.ToString("You have entered Quantity greater than Opening Quantity. Cannot Proceed.");
                        }

                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(hdnvalue.Value) && !string.IsNullOrEmpty(hdnrate.Value))
                        {
                            int olfdatterows = Warehousedts.Select("isnew = 'old'").Count<DataRow>();
                            if (olfdatterows != 0)
                            {
                                var oldeqnty = Warehousedts.Compute("sum(Quantitysum)", "isnew = 'old'");
                                if (Convert.ToDecimal(hdfopeningstockPC.Value) < Convert.ToDecimal(oldeqnty))
                                {
                                    GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("Opening quantity and enterd quantity are mismatch.");
                                }
                                else
                                {

                                }

                            }
                            else
                            {

                            }

                        }
                        else
                        {
                            GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("Nothing to Saved .");
                        }

                    }
                }

            }

            #endregion
            #region CheckDataExist
            if (strSplitCommand == "checkdataexist")
            {
                Session["SRM_WarehouseData"] = null;

                GrdWarehousePC.DataSource = null;

                string strProductname = Convert.ToString(e.Parameters.Split('~')[4]);
                string name = strProductname;
                name = name.Replace("squot", "'");
                name = name.Replace("coma", ",");
                name = name.Replace("slash", "/");

                Session["SRWarehouseUpdatedData"] = null;
                Session["SRWarehouseDataDelete"] = null;

                GrdWarehousePC.JSProperties["cpproductname"] = Convert.ToString(name);


                string strProductID = Convert.ToString(e.Parameters.Split('~')[1]);

                string stockids = Convert.ToString(e.Parameters.Split('~')[2]);

                string branchid = Convert.ToString(e.Parameters.Split('~')[3]);

                DataTable Warehousedt = new DataTable();
                Warehousedt = GetRecord(stockids);
                if (Warehousedt.Rows.Count > 0)
                {
                    Session["SRM_WarehouseData"] = Warehousedt;

                    GrdWarehousePC.DataSource = Warehousedt.DefaultView;
                    GrdWarehousePC.DataBind();
                }
                else
                {

                    Session["SRM_WarehouseData"] = null;

                    GrdWarehousePC.DataSource = null;
                    GrdWarehousePC.DataBind();

                }


            }
            //if (strSplitCommand == "UpdatedataReffresh")
            //{
            //    if (Session["PCWarehouseData"] != null)
            //    {

            //        GrdWarehousePC.DataSource = Session["PCWarehouseData"];
            //        GrdWarehousePC.DataBind();
            //    }
            //}
            #endregion
            #region updatedata
            if (strSplitCommand == "Updatedata")
            {
                string WarehouseID = Convert.ToString(e.Parameters.Split('~')[1]);

                string WarehouseName = Convert.ToString(e.Parameters.Split('~')[2]);

                string BatchName = Convert.ToString(e.Parameters.Split('~')[3]);

                string SerialName = Convert.ToString(e.Parameters.Split('~')[4]);

                string slno = Convert.ToString(e.Parameters.Split('~')[5]);
                string qntity = Convert.ToString(e.Parameters.Split('~')[6]);
                string isviewqntitynull = hdnisviewqntityhas.Value;
                string isserialenable = hdnisserial.Value;
                DateTime expdate = txtexpirdate.Date;
                DateTime mkgdate = txtmkgdate.Date;
                DataTable Warehousedt = new DataTable();

                decimal openingstock = Convert.ToDecimal(txtqnty.Text);
                if (qntity == "0.0000" && openingstock <= 0)
                {
                    qntity = batchqnty.Text;
                    openingstock = Convert.ToDecimal(qntity);
                }

                if (WarehouseID == "null")
                {
                    WarehouseID = "0";
                    WarehouseName = "";
                }

                if (Session["SRM_WarehouseData"] != null)
                {
                    Warehousedt = (DataTable)Session["SRM_WarehouseData"];
                    DataSet dataSet1 = new DataSet();
                    DataTable dt1 = new DataTable();
                    dt1 = Warehousedt.Copy();
                    dataSet1.Tables.Add(dt1);

                    if (dataSet1.Tables[0].Rows.Count > 0)
                    {
                        DataRow[] customerRow = dataSet1.Tables[0].Select("SrlNo ='" + slno + "'");
                        if (isserialenable == "false" && isviewqntitynull != "false")
                        {
                            customerRow[0]["WarehouseID"] = WarehouseID;
                            customerRow[0]["WarehouseName"] = WarehouseName;
                            customerRow[0]["BatchNo"] = BatchName;
                            customerRow[0]["SerialNo"] = SerialName;
                            customerRow[0]["Quantity"] = openingstock;

                            if (Convert.ToString(expdate) != "1/1/0001 12:00:00 AM" && Convert.ToString(mkgdate) != "1/1/0001 12:00:00 AM" && Convert.ToString(expdate) != "01-01-0001 12:00:00 AM" && Convert.ToString(mkgdate) != "01-01-0001 12:00:00 AM")
                            {
                                customerRow[0]["viewMFGDate"] = mkgdate;
                                customerRow[0]["viewExpiryDate"] = expdate;
                            }

                            customerRow[0]["viewWarehouseName"] = WarehouseName;
                            customerRow[0]["viewBatchNo"] = BatchName;
                            customerRow[0]["viewSerialNo"] = SerialName;
                            customerRow[0]["viewQuantity"] = qntity;
                            customerRow[0]["Quantitysum"] = openingstock;
                            customerRow[0]["isnew"] = "Updated";
                        }
                        if (isserialenable == "true" && isviewqntitynull == "false")
                        {
                            customerRow[0]["WarehouseID"] = WarehouseID;
                            customerRow[0]["WarehouseName"] = WarehouseName;
                            customerRow[0]["BatchNo"] = BatchName;
                            customerRow[0]["SerialNo"] = SerialName;
                            customerRow[0]["Quantity"] = openingstock;

                            if (Convert.ToString(expdate) != "1/1/0001 12:00:00 AM" && Convert.ToString(mkgdate) != "1/1/0001 12:00:00 AM" && Convert.ToString(expdate) != "01-01-0001 12:00:00 AM" && Convert.ToString(mkgdate) != "01-01-0001 12:00:00 AM")
                            {
                                customerRow[0]["viewMFGDate"] = mkgdate;
                                customerRow[0]["viewExpiryDate"] = expdate;
                            }

                            customerRow[0]["viewWarehouseName"] = WarehouseName;
                            customerRow[0]["viewBatchNo"] = BatchName;
                            customerRow[0]["viewSerialNo"] = SerialName;
                            customerRow[0]["viewQuantity"] = openingstock;
                            customerRow[0]["Quantitysum"] = openingstock;
                            customerRow[0]["isnew"] = "Updated";
                        }
                        if (isserialenable == "true" && isviewqntitynull == "true")
                        {
                            customerRow[0]["WarehouseID"] = WarehouseID;
                            customerRow[0]["WarehouseName"] = WarehouseName;
                            customerRow[0]["BatchNo"] = BatchName;
                            customerRow[0]["SerialNo"] = SerialName;
                            customerRow[0]["Quantity"] = openingstock;



                            customerRow[0]["viewSerialNo"] = SerialName;

                            customerRow[0]["Quantitysum"] = 0;
                            customerRow[0]["isnew"] = "Updated";
                        }
                        if (isserialenable == "false" && isviewqntitynull == "false")
                        {
                            customerRow[0]["WarehouseID"] = WarehouseID;
                            customerRow[0]["WarehouseName"] = WarehouseName;
                            customerRow[0]["BatchNo"] = BatchName;
                            customerRow[0]["SerialNo"] = SerialName;
                            customerRow[0]["Quantity"] = openingstock;

                            customerRow[0]["SerialNo"] = SerialName;
                            customerRow[0]["Quantity"] = openingstock;
                            if (Convert.ToString(expdate) != "1/1/0001 12:00:00 AM" && Convert.ToString(mkgdate) != "1/1/0001 12:00:00 AM" && Convert.ToString(expdate) != "01-01-0001 12:00:00 AM" && Convert.ToString(mkgdate) != "01-01-0001 12:00:00 AM")
                            {
                                customerRow[0]["viewMFGDate"] = mkgdate;
                                customerRow[0]["viewExpiryDate"] = expdate;
                            }
                            customerRow[0]["viewWarehouseName"] = WarehouseName;
                            customerRow[0]["viewBatchNo"] = BatchName;
                            customerRow[0]["viewSerialNo"] = SerialName;
                            customerRow[0]["viewQuantity"] = openingstock;
                            customerRow[0]["Quantitysum"] = openingstock;
                            customerRow[0]["isnew"] = "Updated";
                        }

                        GrdWarehousePC.JSProperties["cpupdateexistingdata"] = Convert.ToString("Saved.");
                    }
                    Warehousedt = null;
                    Warehousedt = dataSet1.Tables[0];
                    Session["SRM_WarehouseData"] = Warehousedt;

                    Session["SRWarehouseUpdatedData"] = Warehousedt;

                    GrdWarehousePC.DataSource = Warehousedt.DefaultView;
                    GrdWarehousePC.DataBind();

                }
            }

            #endregion
            #region newrowupdate
            if (strSplitCommand == "NewUpdatedata")
            {
                string WarehouseID = Convert.ToString(e.Parameters.Split('~')[1]);

                string WarehouseName = Convert.ToString(e.Parameters.Split('~')[2]);

                string BatchName = Convert.ToString(e.Parameters.Split('~')[3]);

                string SerialName = Convert.ToString(e.Parameters.Split('~')[4]);

                string slno = Convert.ToString(e.Parameters.Split('~')[5]);
                string qntity = Convert.ToString(e.Parameters.Split('~')[6]);
                string isviewqntitynull = hdnisviewqntityhas.Value;
                string isserialenable = hdnisserial.Value;
                DataTable Warehousedt = new DataTable();
                decimal openingstock = Convert.ToDecimal(txtqnty.Text);
                if (qntity == "0.0000" && openingstock <= 0)
                {
                    qntity = batchqnty.Text;
                    openingstock = Convert.ToDecimal(qntity);
                }

                if (Session["SRM_WarehouseData"] != null)
                {
                    Warehousedt = (DataTable)Session["SRM_WarehouseData"];
                    DataSet dataSet1 = new DataSet();
                    DataTable dt1 = new DataTable();
                    dt1 = Warehousedt.Copy();
                    dataSet1.Tables.Add(dt1);

                    if (dataSet1.Tables[0].Rows.Count > 0)
                    {
                        DataRow[] customerRow = dataSet1.Tables[0].Select("SrlNo ='" + slno + "'");
                        if (isserialenable == "false" && isviewqntitynull != "false")
                        {
                            customerRow[0]["WarehouseID"] = WarehouseID;
                            customerRow[0]["WarehouseName"] = WarehouseName;
                            customerRow[0]["BatchNo"] = BatchName;
                            customerRow[0]["SerialNo"] = SerialName;
                            customerRow[0]["Quantity"] = openingstock;

                            customerRow[0]["viewWarehouseName"] = WarehouseName;
                            customerRow[0]["viewBatchNo"] = BatchName;
                            customerRow[0]["viewSerialNo"] = SerialName;
                            customerRow[0]["viewQuantity"] = openingstock;
                            customerRow[0]["Quantitysum"] = openingstock;
                            customerRow[0]["isnew"] = "new";
                        }
                        if (isserialenable == "true" && isviewqntitynull == "false")
                        {
                            customerRow[0]["WarehouseID"] = WarehouseID;
                            customerRow[0]["WarehouseName"] = WarehouseName;
                            customerRow[0]["BatchNo"] = BatchName;
                            customerRow[0]["SerialNo"] = SerialName;
                            customerRow[0]["Quantity"] = openingstock;

                            customerRow[0]["viewWarehouseName"] = WarehouseName;
                            customerRow[0]["viewBatchNo"] = BatchName;
                            customerRow[0]["viewSerialNo"] = SerialName;
                            customerRow[0]["viewQuantity"] = openingstock;
                            customerRow[0]["Quantitysum"] = openingstock;
                            customerRow[0]["isnew"] = "new";
                        }
                        if (isserialenable == "true" && isviewqntitynull == "true")
                        {
                            customerRow[0]["WarehouseID"] = WarehouseID;
                            customerRow[0]["WarehouseName"] = WarehouseName;
                            customerRow[0]["BatchNo"] = BatchName;
                            customerRow[0]["SerialNo"] = SerialName;
                            customerRow[0]["Quantity"] = openingstock;



                            customerRow[0]["viewSerialNo"] = SerialName;

                            customerRow[0]["Quantitysum"] = 0;
                            customerRow[0]["isnew"] = "new";
                        }
                        if (isserialenable == "false" && isviewqntitynull == "false")
                        {
                            customerRow[0]["WarehouseID"] = WarehouseID;
                            customerRow[0]["WarehouseName"] = WarehouseName;
                            customerRow[0]["BatchNo"] = BatchName;
                            customerRow[0]["SerialNo"] = SerialName;
                            customerRow[0]["Quantity"] = openingstock;

                            customerRow[0]["viewWarehouseName"] = WarehouseName;
                            customerRow[0]["viewBatchNo"] = BatchName;
                            customerRow[0]["viewSerialNo"] = SerialName;
                            customerRow[0]["viewQuantity"] = openingstock;
                            customerRow[0]["Quantitysum"] = openingstock;
                            customerRow[0]["isnew"] = "new";
                        }

                        GrdWarehousePC.JSProperties["cpupdatenewdata"] = Convert.ToString("Saved.");
                    }
                    Warehousedt = null;
                    Warehousedt = dataSet1.Tables[0];
                    Session["SRM_WarehouseData"] = Warehousedt;

                    GrdWarehousePC.DataSource = Warehousedt.DefaultView;
                    GrdWarehousePC.DataBind();

                }


            }
            #endregion
            #region Delete
            if (strSplitCommand == "Delete")
            {
                string slno = Convert.ToString(e.Parameters.Split('~')[1]);

                string viewQuantity = Convert.ToString(e.Parameters.Split('~')[2]);

                string Quantity = Convert.ToString(e.Parameters.Split('~')[3]);
                string warehouseid = Convert.ToString(e.Parameters.Split('~')[4]);
                string batch = Convert.ToString(e.Parameters.Split('~')[5]);
                DataTable Warehousedt = new DataTable();

                // string isviewqntitynull = hdnisviewqntityhas.Value;
                string isserialenable = hdnisserial.Value;
                string isbatch = hdnisbatch.Value;
                string iswarehouse = hdniswarehouse.Value;
                string isolddeletd = hdnisolddeleted.Value;
                if (Session["SRM_WarehouseData"] != null)
                {
                    Warehousedt = (DataTable)Session["SRM_WarehouseData"];
                    DataSet dataSet1 = new DataSet();

                    DataTable dt1 = new DataTable();
                    dt1 = Warehousedt.Copy();



                    dataSet1.Tables.Add(dt1);

                    if (dataSet1.Tables[0].Rows.Count > 0)
                    {

                        if (isserialenable == "true" && (viewQuantity == "" || viewQuantity == "0") && isbatch == "true" && iswarehouse == "true")
                        {
                            DataRow[] customerRows = dataSet1.Tables[0].Select("WarehouseID= '" + warehouseid + "' AND BatchNo='" + batch + "'");

                            for (int i = 0; i <= customerRows.Count() - 1; i++)
                            {
                                if (Convert.ToString(customerRows[i]["SrlNo"]) == slno && string.IsNullOrEmpty(Convert.ToString(customerRows[i]["viewQuantity"])))
                                {
                                    customerRows[i]["isnew"] = "DeleteSL";
                                }
                                else if (Convert.ToString(customerRows[i]["SrlNo"]) == slno && !string.IsNullOrEmpty(Convert.ToString(customerRows[i]["viewQuantity"])))
                                {

                                    customerRows[i]["isnew"] = "DeleteWHBTSL";

                                }

                                if (!string.IsNullOrEmpty(Convert.ToString(customerRows[i]["viewQuantity"])))
                                {
                                    customerRows[i]["viewQuantity"] = Convert.ToDecimal(customerRows[i]["Quantitysum"]) - 1;
                                    customerRows[i]["Quantity"] = Convert.ToDecimal(customerRows[i]["Quantity"]) - 1;
                                    customerRows[i]["Quantitysum"] = Convert.ToDecimal(customerRows[i]["Quantitysum"]) - 1;

                                    if (Convert.ToString(customerRows[i]["isnew"]) != "new")
                                    {
                                        customerRows[i]["isnew"] = "Updated";
                                    }
                                    else
                                    {
                                        customerRows[i]["isnew"] = "new";
                                    }

                                }
                                //else
                                //{
                                //    customerRows[i]["Quantity"] = 0;
                                //    customerRows[i]["Quantitysum"] = 0;
                                //}
                            }


                            GrdWarehousePC.JSProperties["cpdeletedata"] = Convert.ToString(1);

                        }
                        else if (isserialenable == "true" && (viewQuantity == "" || viewQuantity == "0") && isbatch != "true" && iswarehouse == "true")
                        {
                            DataRow[] customerRows = dataSet1.Tables[0].Select("WarehouseID= '" + warehouseid + "'");

                            for (int i = 0; i <= customerRows.Count() - 1; i++)
                            {
                                if (Convert.ToString(customerRows[i]["SrlNo"]) == slno && string.IsNullOrEmpty(Convert.ToString(customerRows[i]["viewQuantity"])))
                                {
                                    customerRows[i]["isnew"] = "DeleteSL";
                                }
                                else if (Convert.ToString(customerRows[i]["SrlNo"]) == slno && !string.IsNullOrEmpty(Convert.ToString(customerRows[i]["viewQuantity"])))
                                {

                                    customerRows[i]["isnew"] = "DeleteWHSL";


                                }

                                if (!string.IsNullOrEmpty(Convert.ToString(customerRows[i]["viewQuantity"])))
                                {
                                    customerRows[i]["viewQuantity"] = Convert.ToDecimal(customerRows[i]["Quantitysum"]) - 1;
                                    customerRows[i]["Quantity"] = Convert.ToDecimal(customerRows[i]["Quantity"]) - 1;
                                    customerRows[i]["Quantitysum"] = Convert.ToDecimal(customerRows[i]["Quantitysum"]) - 1;

                                    if (Convert.ToString(customerRows[i]["isnew"]) != "new")
                                    {
                                        customerRows[i]["isnew"] = "Updated";
                                    }
                                    else
                                    {
                                        customerRows[i]["isnew"] = "new";
                                    }

                                }
                                else
                                {
                                    customerRows[i]["Quantity"] = 0;
                                    customerRows[i]["Quantitysum"] = 0;
                                }
                            }


                            GrdWarehousePC.JSProperties["cpdeletedata"] = Convert.ToString(1);

                        }

                        else if (isserialenable == "true" && (viewQuantity == "" || viewQuantity == "0") && isbatch == "true" && iswarehouse != "true")
                        {
                            DataRow[] customerRows = dataSet1.Tables[0].Select("BatchNo= '" + batch + "'");

                            for (int i = 0; i <= customerRows.Count() - 1; i++)
                            {
                                if (Convert.ToString(customerRows[i]["SrlNo"]) == slno && string.IsNullOrEmpty(Convert.ToString(customerRows[i]["viewQuantity"])))
                                {
                                    customerRows[i]["isnew"] = "DeleteSL";
                                }
                                else if (Convert.ToString(customerRows[i]["SrlNo"]) == slno && !string.IsNullOrEmpty(Convert.ToString(customerRows[i]["viewQuantity"])))
                                {

                                    customerRows[i]["isnew"] = "DeleteWHBTSL";

                                    //customerRows[i]["isnew"] = "DeleteWHBTSL";
                                }
                                if (!string.IsNullOrEmpty(Convert.ToString(customerRows[i]["viewQuantity"])))
                                {
                                    customerRows[i]["viewQuantity"] = Convert.ToDecimal(customerRows[i]["Quantitysum"]) - 1;
                                    customerRows[i]["Quantity"] = Convert.ToDecimal(customerRows[i]["Quantity"]) - 1;
                                    customerRows[i]["Quantitysum"] = Convert.ToDecimal(customerRows[i]["Quantitysum"]) - 1;

                                    if (Convert.ToString(customerRows[i]["isnew"]) != "new")
                                    {
                                        customerRows[i]["isnew"] = "Updated";
                                    }
                                    else
                                    {
                                        customerRows[i]["isnew"] = "new";
                                    }

                                }
                                else
                                {

                                    customerRows[i]["Quantity"] = 0;
                                    customerRows[i]["Quantitysum"] = 0;

                                }

                            }

                            GrdWarehousePC.JSProperties["cpdeletedata"] = Convert.ToString(1);


                        }
                        else
                        {

                            DataRow[] customerRow = dataSet1.Tables[0].Select("SrlNo ='" + slno + "'");
                            if (isserialenable != "true" && isbatch == "true" && iswarehouse == "true")
                            {
                                customerRow[0]["isnew"] = "DeleteWHBT";
                                GrdWarehousePC.JSProperties["cpdeletedata"] = Convert.ToString(viewQuantity);
                            }
                            if (isserialenable != "true" && isbatch != "true" && iswarehouse == "true")
                            {
                                customerRow[0]["isnew"] = "DeleteWH";
                                GrdWarehousePC.JSProperties["cpdeletedata"] = Convert.ToString(Quantity);
                            }
                            if (isserialenable != "true" && isbatch == "true" && iswarehouse != "true")
                            {
                                customerRow[0]["isnew"] = "DeleteWHBT";
                                GrdWarehousePC.JSProperties["cpdeletedata"] = Convert.ToString(viewQuantity);
                            }
                            if (isserialenable == "true" && isbatch == "true" && iswarehouse == "true")
                            {
                                customerRow[0]["isnew"] = "DeleteWHBTSL";
                                GrdWarehousePC.JSProperties["cpdeletedata"] = Convert.ToString(Quantity);
                            }
                            if (isserialenable == "true" && isbatch != "true" && iswarehouse == "true")
                            {
                                customerRow[0]["isnew"] = "DeleteWHBTSL";
                                GrdWarehousePC.JSProperties["cpdeletedata"] = Convert.ToString(1);
                            }
                            if (isserialenable == "true" && isbatch == "true" && iswarehouse != "true")
                            {
                                customerRow[0]["isnew"] = "DeleteWHBTSL";
                                GrdWarehousePC.JSProperties["cpdeletedata"] = Convert.ToString(1);
                            }
                        }

                    }

                    Warehousedt = null;
                    DataTable setdeletedate = new DataTable();
                    DataTable getdeletedate = dataSet1.Tables[0];

                    DataRow[] drs = getdeletedate.Select("isnew = 'DeleteWHBT' OR isnew = 'DeleteWH' OR isnew = 'DeleteWHBT' OR isnew = 'DeleteBTSL' OR isnew = 'DeleteSL' OR isnew = 'DeleteWHSL' OR isnew = 'DeleteWHBTSL'");
                    if (drs.Count() != 0)
                    {
                        if (Session["SRWarehouseDataDelete"] != null)
                        {
                            setdeletedate = drs.CopyToDataTable();
                            DataTable dtmp = (DataTable)Session["SRWarehouseDataDelete"];
                            dtmp.Merge(setdeletedate);
                            Session["SRWarehouseDataDelete"] = dtmp;
                        }
                        else
                        {
                            setdeletedate = drs.CopyToDataTable();
                            Session["SRWarehouseDataDelete"] = setdeletedate;
                        }


                    }


                    DataTable Warehousedts = dataSet1.Tables[0];
                    DataRow[] dr = Warehousedts.Select("isnew = 'new' OR isnew = 'Updated' OR isnew = 'old'");
                    if (dr.Count() > 0)
                    {

                        Warehousedt = dr.CopyToDataTable();
                        Warehousedt.DefaultView.Sort = "SrlNo asc";
                        Warehousedt = Warehousedt.DefaultView.ToTable(true);

                        Session["SRM_WarehouseData"] = Warehousedt;

                        GrdWarehousePC.DataSource = Session["SRM_WarehouseData"];
                        GrdWarehousePC.DataBind();
                    }
                    else
                    {
                        Session["SRM_WarehouseData"] = null;

                        //Warehousedt.Columns.Add("SrlNo", typeof(string));
                        //Warehousedt.Columns.Add("WarehouseID", typeof(string));
                        //Warehousedt.Columns.Add("WarehouseName", typeof(string));

                        //Warehousedt.Columns.Add("BatchNo", typeof(string));
                        //Warehousedt.Columns.Add("SerialNo", typeof(string));

                        //Warehousedt.Columns.Add("MFGDate", typeof(DateTime));
                        //Warehousedt.Columns.Add("ExpiryDate", typeof(DateTime));
                        //Warehousedt.Columns.Add("Quantity", typeof(decimal));

                        //Warehousedt.Columns.Add("BatchWarehouseID", typeof(string));
                        //Warehousedt.Columns.Add("BatchWarehousedetailsID", typeof(string));
                        //Warehousedt.Columns.Add("BatchID", typeof(string));
                        //Warehousedt.Columns.Add("SerialID", typeof(string));


                        //Warehousedt.Columns.Add("viewWarehouseName", typeof(string));

                        //Warehousedt.Columns.Add("viewBatchNo", typeof(string));
                        //Warehousedt.Columns.Add("viewQuantity", typeof(string));
                        //Warehousedt.Columns.Add("viewSerialNo", typeof(string));



                        //Warehousedt.Columns.Add("viewMFGDate", typeof(DateTime));
                        //Warehousedt.Columns.Add("viewExpiryDate", typeof(DateTime));

                        //Warehousedt.Columns.Add("Quantitysum", typeof(decimal));
                        //Warehousedt.Columns.Add("isnew", typeof(string));

                        GrdWarehousePC.DataSource = Warehousedt;
                        GrdWarehousePC.DataBind();



                    }


                }
            #endregion
            }


        }

        public DataTable GetRecord(string stockids)
        {
            Session["SRM_WarehouseData"] = null;
            DataTable Warehousedt = new DataTable();

            if (Session["SRM_WarehouseData"] != null)
            {
                Warehousedt = (DataTable)Session["SRM_WarehouseData"];
            }
            else
            {
                //Warehousedt.Columns.Add("Product_SrlNo", typeof(string));
                Warehousedt.Columns.Add("SrlNo", typeof(Int32));
                Warehousedt.Columns.Add("WarehouseID", typeof(string));
                Warehousedt.Columns.Add("WarehouseName", typeof(string));

                Warehousedt.Columns.Add("BatchNo", typeof(string));
                Warehousedt.Columns.Add("SerialNo", typeof(string));

                Warehousedt.Columns.Add("MFGDate", typeof(DateTime));
                Warehousedt.Columns.Add("ExpiryDate", typeof(DateTime));
                Warehousedt.Columns.Add("Quantity", typeof(decimal));

                Warehousedt.Columns.Add("BatchWarehouseID", typeof(string));
                Warehousedt.Columns.Add("BatchWarehousedetailsID", typeof(string));
                Warehousedt.Columns.Add("BatchID", typeof(string));
                Warehousedt.Columns.Add("SerialID", typeof(string));


                Warehousedt.Columns.Add("viewWarehouseName", typeof(string));

                Warehousedt.Columns.Add("viewBatchNo", typeof(string));
                Warehousedt.Columns.Add("viewQuantity", typeof(string));
                Warehousedt.Columns.Add("viewSerialNo", typeof(string));



                Warehousedt.Columns.Add("viewMFGDate", typeof(DateTime));
                Warehousedt.Columns.Add("viewExpiryDate", typeof(DateTime));

                Warehousedt.Columns.Add("Quantitysum", typeof(decimal));
                Warehousedt.Columns.Add("isnew", typeof(string));

                Warehousedt.Columns.Add("productid", typeof(string));
                Warehousedt.Columns.Add("Inventrytype", typeof(string));
                Warehousedt.Columns.Add("StockID", typeof(string));
                Warehousedt.Columns.Add("pcslno", typeof(Int32));
            }


            DataTable dts = GetStockWarehouseData(stockids);

            string oldwarehousename = string.Empty;
            string oldbatchname = string.Empty;
            string oldquantity = string.Empty;
            string isoldornew = string.Empty;
            string BatchWarehouseID = string.Empty;

            if (dts.Rows.Count > 0)
            {
                for (int i = 0; i <= dts.Rows.Count - 1; i++)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(dts.Rows[i]["BatchWarehouseID"])) && Convert.ToString(dts.Rows[i]["BatchWarehouseID"]) != "0" && Convert.ToString(Session["SRM_ActionType"]) == "Edit")
                    {
                        isoldornew = "old";
                    }
                    else
                    {
                        isoldornew = "new";
                    }

                    if (hdnisserial.Value == "true" && i != 0)
                    {
                        oldwarehousename = Convert.ToString(dts.Rows[i - 1]["warehouseID"]);
                        oldbatchname = Convert.ToString(dts.Rows[i - 1]["batchNO"]);
                        oldquantity = Convert.ToString(dts.Rows[i - 1]["Quantitysum"]);
                        BatchWarehouseID = Convert.ToString(dts.Rows[i - 1]["BatchWarehouseID"]);

                        if (oldwarehousename == Convert.ToString(dts.Rows[i]["warehouseID"]) && oldbatchname == Convert.ToString(dts.Rows[i]["batchNO"]) && (oldquantity == Convert.ToString(dts.Rows[i]["Quantitysum"]) || Convert.ToString(dts.Rows[i]["Quantitysum"]) == "0") && BatchWarehouseID == Convert.ToString(dts.Rows[i]["BatchWarehouseID"]))
                        {

                            if (Convert.ToString(dts.Rows[i]["MfgDate"]) == "1/1/1900 12:00:00 AM" || Convert.ToString(dts.Rows[i]["MfgDate"]) == "01-01-0001 12:00:00 AM")
                            {
                                Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToDecimal(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), "", "", "", Convert.ToString(dts.Rows[i]["serialno"]), null, null, 0, isoldornew, Convert.ToString(dts.Rows[i]["productid"]), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                            }
                            else if (string.IsNullOrEmpty(Convert.ToString(dts.Rows[i]["MfgDate"])))
                            {
                                Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToDecimal(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), "", "", "", Convert.ToString(dts.Rows[i]["serialno"]), null, null, 0, isoldornew, Convert.ToString(dts.Rows[i]["productid"]), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                            }
                            else
                            {

                                Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), Convert.ToDateTime(dts.Rows[i]["MfgDate"]), Convert.ToDateTime(dts.Rows[i]["ExpiryDate"]), Convert.ToDecimal(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), "", "", "", Convert.ToString(dts.Rows[i]["serialno"]), null, null, 0, isoldornew, Convert.ToString(dts.Rows[i]["productid"]), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                            }
                        }
                        //else if (oldwarehousename == Convert.ToString(dts.Rows[i]["warehouseID"]) && oldbatchname == Convert.ToString(dts.Rows[i]["batchNO"]))
                        //{
                        //    if (Convert.ToString(dts.Rows[i]["MfgDate"]) == "1/1/1900 12:00:00 AM")
                        //    {
                        //        Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToDecimal(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), "", "", "", Convert.ToString(dts.Rows[i]["serialno"]), null, null, 0, "old");
                        //    }
                        //    else if (string.IsNullOrEmpty(Convert.ToString(dts.Rows[i]["MfgDate"])))
                        //    {
                        //        Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToDecimal(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), "", "", "", Convert.ToString(dts.Rows[i]["serialno"]), null, null, 0, "old");
                        //    }
                        //    else
                        //    {

                        //        Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), Convert.ToDateTime(dts.Rows[i]["MfgDate"]), Convert.ToDateTime(dts.Rows[i]["ExpiryDate"]), Convert.ToDecimal(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), "", "", "", Convert.ToString(dts.Rows[i]["serialno"]), null, null, 0, "old");
                        //    }
                        //}
                        else
                        {
                            //string viewqunatity = (Convert.ToString(dts.Rows[i]["viewQuantity"])!=string.Empty)? Convert.ToString(dts.Rows[i]["viewQuantity"]):"0";
                            string viewqunatity = Convert.ToString(dts.Rows[i]["viewQuantity"]);
                            string sumquantiy = (Convert.ToString(dts.Rows[i]["Quantitysum"]) != string.Empty) ? Convert.ToString(dts.Rows[i]["Quantitysum"]) : "0";
                            //if (Convert.ToDateTime(dts.Rows[i]["MfgDate"]) != default(DateTime))
                            if (Convert.ToString(dts.Rows[i]["MfgDate"]) == "1/1/1900 12:00:00 AM" || Convert.ToString(dts.Rows[i]["MfgDate"]) == "01-01-0001 12:00:00 AM")
                            {
                                Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToDecimal(sumquantiy), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), viewqunatity, Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToString(sumquantiy), isoldornew, Convert.ToString(dts.Rows[i]["productid"]), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                            }
                            else if (string.IsNullOrEmpty(Convert.ToString(dts.Rows[i]["MfgDate"])))
                            {
                                Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToString(sumquantiy), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), viewqunatity, Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToString(sumquantiy), isoldornew, Convert.ToString(dts.Rows[i]["productid"]), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                            }
                            else
                            {

                                Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), Convert.ToDateTime(dts.Rows[i]["MfgDate"]), Convert.ToDateTime(dts.Rows[i]["ExpiryDate"]), Convert.ToDecimal(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), viewqunatity, Convert.ToString(dts.Rows[i]["serialno"]), Convert.ToDateTime(dts.Rows[i]["MfgDate"]), Convert.ToDateTime(dts.Rows[i]["ExpiryDate"]), Convert.ToString(sumquantiy), isoldornew, Convert.ToString(dts.Rows[i]["productid"]), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                            }

                        }

                    }
                    else if (hdnisserial.Value == "false" && hdniswarehouse.Value == "true" && hdnisbatch.Value == "true")
                    {
                        //string oldquantity=string.Empty;
                        if (i != 0)
                        {
                            oldwarehousename = Convert.ToString(dts.Rows[i - 1]["warehouseID"]);
                            oldquantity = Convert.ToString(dts.Rows[i - 1]["viewQuantity"]);
                        }



                        if (oldwarehousename == Convert.ToString(dts.Rows[i]["warehouseID"]))
                        {
                            //if (Convert.ToDateTime(dts.Rows[i]["MfgDate"]) != default(DateTime))
                            if (Convert.ToString(dts.Rows[i]["MfgDate"]) == "1/1/1900 12:00:00 AM" || Convert.ToString(dts.Rows[i]["MfgDate"]) == "01-01-0001 12:00:00 AM")
                            {
                                Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToDecimal(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), "", Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["viewQuantity"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToString(dts.Rows[i]["viewQuantity"]), isoldornew, Convert.ToString(dts.Rows[i]["productid"]), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                            }
                            else if (string.IsNullOrEmpty(Convert.ToString(dts.Rows[i]["MfgDate"])))
                            {
                                Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToDecimal(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), "", Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["viewQuantity"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToString(dts.Rows[i]["viewQuantity"]), isoldornew, Convert.ToString(dts.Rows[i]["productid"]), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                            }
                            else
                            {

                                Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), Convert.ToDateTime(dts.Rows[i]["MfgDate"]), Convert.ToDateTime(dts.Rows[i]["ExpiryDate"]), Convert.ToDecimal(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), "", Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["viewQuantity"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToString(dts.Rows[i]["viewQuantity"]), isoldornew, Convert.ToString(dts.Rows[i]["productid"]), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                            }
                        }
                        else
                        {
                            //  if (Convert.ToDateTime(dts.Rows[i]["MfgDate"]) != default(DateTime))
                            if (Convert.ToString(dts.Rows[i]["MfgDate"]) == "1/1/1900 12:00:00 AM" || Convert.ToString(dts.Rows[i]["MfgDate"]) == "01-01-0001 12:00:00 AM")
                            {
                                Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToDecimal(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["viewQuantity"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToString(dts.Rows[i]["viewQuantity"]), isoldornew, Convert.ToString(dts.Rows[i]["productid"]), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                            }
                            else if (string.IsNullOrEmpty(Convert.ToString(dts.Rows[i]["MfgDate"])))
                            {
                                Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToString(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["viewQuantity"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToString(dts.Rows[i]["viewQuantity"]), isoldornew, Convert.ToString(dts.Rows[i]["productid"]), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                            }
                            else
                            {

                                Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), Convert.ToDateTime(dts.Rows[i]["MfgDate"]), Convert.ToDateTime(dts.Rows[i]["ExpiryDate"]), Convert.ToDecimal(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["viewQuantity"]), Convert.ToString(dts.Rows[i]["serialno"]), Convert.ToDateTime(dts.Rows[i]["MfgDate"]), Convert.ToDateTime(dts.Rows[i]["ExpiryDate"]), Convert.ToString(dts.Rows[i]["viewQuantity"]), isoldornew, Convert.ToString(dts.Rows[i]["productid"]), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                            }
                        }

                    }
                    else
                    {
                        if (Convert.ToString(dts.Rows[i]["MfgDate"]) == "1/1/1900 12:00:00 AM" || Convert.ToString(dts.Rows[i]["MfgDate"]) == "01-01-0001 12:00:00 AM")
                        {
                            Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToDecimal(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["viewQuantity"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToString(dts.Rows[i]["viewQuantity"]), isoldornew, Convert.ToString(hdfProductIDPC.Value), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                        }
                        else if (string.IsNullOrEmpty(Convert.ToString(dts.Rows[i]["MfgDate"])))
                        {
                            Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToDecimal(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["viewQuantity"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToString(dts.Rows[i]["viewQuantity"]), isoldornew, Convert.ToString(hdfProductIDPC.Value), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                        }
                        else
                        {

                            Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), Convert.ToDateTime(dts.Rows[i]["MfgDate"]), Convert.ToDateTime(dts.Rows[i]["ExpiryDate"]), Convert.ToDecimal(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["viewQuantity"]), Convert.ToString(dts.Rows[i]["serialno"]), Convert.ToDateTime(dts.Rows[i]["MfgDate"]), Convert.ToDateTime(dts.Rows[i]["ExpiryDate"]), Convert.ToString(dts.Rows[i]["viewQuantity"]), isoldornew, Convert.ToString(dts.Rows[i]["productid"]), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                        }

                    }


                    //Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, WarehouseID, WarehouseName, BatchName, SerialName, "", "", openingstock, "isnew");

                }
            }

            hdnoldrowcount.Value = Convert.ToString(Warehousedt.Rows.Count);

            return Warehousedt;
        }

        private int Insertupdatedata(string ProductID, string stockid, string branchid)
        {
            DataSet dsEmail = new DataSet();

            string IsserialActive = "false";

            int newrowcount = 0;

            #region Insert

            DataTable temtable = new DataTable();
            DataTable temtable2 = new DataTable();
            DataTable temtable23 = new DataTable();
            if (Session["SRM_WarehouseData"] != null)
            {
                DataTable Warehousedt = (DataTable)Session["SRM_WarehouseData"];

                temtable = Warehousedt.DefaultView.ToTable(false, "SrlNo", "BatchWarehouseID", "BatchWarehousedetailsID", "BatchID", "SerialID", "WarehouseID", "WarehouseName", "BatchNo", "SerialNo", "MFGDate", "ExpiryDate", "Quantitysum", "isnew", "viewQuantity", "productid", "Inventrytype", "StockID", "pcslno");

                if (Session["SRwarehousedetailstemp"] != null)
                {
                    temtable23 = (DataTable)Session["SRwarehousedetailstemp"];
                    DataRow[] rows;
                    rows = temtable23.Select("pcslno = '" + hdnpcslno.Value + "'");  //'UserName' is ColumnName
                    foreach (DataRow row in rows)
                        temtable23.Rows.Remove(row);

                    temtable23.Merge(temtable);
                    Session["SRwarehousedetailstemp"] = temtable23;
                }
                else
                {
                    Session["SRwarehousedetailstemp"] = temtable;
                }

            }


            #endregion
            #region Update
            if (Session["SRM_WarehouseData"] != null)
            {
                DataTable dt = new DataTable();
                DataTable Warehousedtups = (DataTable)Session["SRM_WarehouseData"];
                DataRow[] dr = Warehousedtups.Select("isnew = 'Updated'");
                if (dr.Count() != 0)
                {
                    dt = dr.CopyToDataTable();
                    if (Session["SRwarehousedetailstempUpdate"] != null)
                    {
                        temtable23 = (DataTable)Session["SRwarehousedetailstempUpdate"];
                        DataRow[] rows;
                        rows = temtable23.Select("pcslno = '" + hdnpcslno.Value + "'");  //'UserName' is ColumnName
                        foreach (DataRow row in rows)
                            temtable23.Rows.Remove(row);

                        temtable23.Merge(dt);
                        Session["SRwarehousedetailstempUpdate"] = temtable23;
                    }
                    else
                    {
                        Session["SRwarehousedetailstempUpdate"] = dt;
                    }


                }
            }

            #endregion
            #region delete

            if (Session["SRWarehouseDataDelete"] != null)
            {
                DataTable dt = new DataTable();
                DataTable Warehousedtups = (DataTable)Session["SRWarehouseDataDelete"];
                DataRow[] dr = Warehousedtups.Select("isnew = 'DeleteWHBT' OR isnew = 'DeleteWH' OR isnew = 'DeleteWHBT' OR isnew = 'DeleteBTSL' OR isnew = 'DeleteSL' OR isnew = 'DeleteWHSL' OR isnew = 'DeleteWHBTSL'");
                if (dr.Count() != 0)
                {
                    dt = dr.CopyToDataTable();
                    if (Session["SRwarehousedetailstempDelete"] != null)
                    {
                        temtable23 = (DataTable)Session["SRwarehousedetailstempDelete"];
                        DataRow[] rows;
                        rows = temtable23.Select("pcslno = '" + hdnpcslno.Value + "'");  //'UserName' is ColumnName
                        foreach (DataRow row in rows)
                            temtable23.Rows.Remove(row);

                        temtable23.Merge(dt);
                        Session["SRwarehousedetailstempDelete"] = temtable23;
                    }
                    else
                    {
                        Session["SRwarehousedetailstempDelete"] = dt;
                    }


                }

                //deleteALL(stockid);

            }
            #endregion

            return 1;


        }
        protected void GrdWarehousePC_DataBinding(object sender, EventArgs e)
        {
            if (Session["SRM_WarehouseData"] != null)
            {
                DataTable Warehousedt = (DataTable)Session["SRM_WarehouseData"];

                GrdWarehousePC.DataSource = Warehousedt;
            }
        }

        protected void CmbWarehouse_Callback(object sender, CallbackEventArgsBase e)
        {
            string WhichCall = e.Parameter.Split('~')[0];

            if (WhichCall == "BindWarehouse")
            {
                DataTable dt = GetWarehouseData();
                GetAvailableStock();

                CmbWarehouse.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    CmbWarehouse.Items.Add(Convert.ToString(dt.Rows[i]["WarehouseName"]), Convert.ToString(dt.Rows[i]["WarehouseID"]));
                }
                if (hdndefaultID != null || hdndefaultID.Value != "")
                    CmbWarehouse.Value = hdndefaultID.Value;
            }

        }


        public DataTable GetWarehouseData()
        {
            string strBranch = Convert.ToString(ddl_Branch.SelectedValue);

            DataTable dt = new DataTable();
            dt = oDBEngine.GetDataTable("select  bui_id as WarehouseID,bui_Name as WarehouseName from tbl_master_building Where IsNull(bui_BranchId,0) in ('0','" + strBranch + "') order by bui_Name");
            return dt;
        }
        //public DataTable GetWarehouseData()
        //{
        //    DataTable dt = new DataTable();
        //    dt = oDBEngine.GetDataTable("select  b.bui_id as WarehouseID,b.bui_Name as WarehouseName from tbl_master_building b order by b.bui_Name");
        //    return dt;
        //}

        public void GetQuantityBaseOnProductforDetailsId(string strSrlNo, ref decimal strUOMQuantity)
        {
            decimal sum = 0;

            DataTable MultiUOMData = new DataTable();
            if (Session["MultiUOMDataRETM"] != null)
            {
                MultiUOMData = (DataTable)Session["MultiUOMDataRETM"];
                for (int i = 0; i < MultiUOMData.Rows.Count; i++)
                {
                    DataRow dr = MultiUOMData.Rows[i];
                    string SrlNo = Convert.ToString(dr["SrlNo"]);

                    if (strSrlNo == SrlNo)
                    {
                        string strQuantity = (Convert.ToString(dr["Quantity"]) != "") ? Convert.ToString(dr["Quantity"]) : "0";
                        var weight = Decimal.Parse(Regex.Match(strQuantity, "[0-9]*\\.*[0-9]*").Value);

                        sum = Convert.ToDecimal(weight);
                    }
                }
            }

            strUOMQuantity = sum;

        }


        public void GetAvailableStock()
        {
            //DataTable dt2 = oDBEngine.GetDataTable("select ISnull(ISNULL(tblwarehous.StockBranchWarehouse_StockIn,0)- isnull(tblwarehous.StockBranchWarehouse_StockOut,0),0) as branchopenstock from Trans_StockBranchWarehouse tblwarehous where tblwarehous.StockBranchWarehouse_StockId=" + hdfstockidPC.Value + " and tblwarehous.StockBranchWarehouse_CompanyId='" + Convert.ToString(Session["LastCompany"]) + "' and tblwarehous.StockBranchWarehouse_FinYear='" + Convert.ToString(Session["LastFinYear"]) + "' and tblwarehous.StockBranchWarehouse_BranchId=" + hdbranchIDPC.Value + "");
            try
            {
                DataTable dt2 = oDBEngine.GetDataTable("Select dbo.fn_CheckAvailableStockOpening(" + hdbranchIDPC.Value + ",'" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]) + "'," + hdfstockidPC.Value + ") as branchopenstock");

                if (dt2.Rows.Count > 0)
                {
                    CmbWarehouse.JSProperties["cpstock"] = Convert.ToString(dt2.Rows[0]["branchopenstock"]);
                }
                else
                {

                    CmbWarehouse.JSProperties["cpstock"] = "0.0000";
                }
            }
            catch (Exception ex)
            {

            }
        }
        public DataTable GetStockWarehouseData(string stockid)
        {
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();

            if (Convert.ToString(Session["SRM_ActionType"]) == "Edit")
            {
                DataSet dsInst = new DataSet();

                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);

                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

                SqlCommand cmd = new SqlCommand("prc_StockGetPCwarehousentry", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@StockID", Convert.ToInt32(stockid));
                cmd.Parameters.AddWithValue("@ProductID", Convert.ToInt32(hdfProductIDPC.Value));
                cmd.Parameters.AddWithValue("@branchID", Convert.ToInt32(hdbranchIDPC.Value));
                cmd.Parameters.AddWithValue("@compnay", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@Finyear", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@PCNumber", Convert.ToString(Session["SRM_ReturnID"]));
                cmd.Parameters.AddWithValue("@ISfromPO", "MainSalesReturnWarehouse");
                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                con.Dispose();

                if (dsInst.Tables != null)
                {


                    if (Session["SRwarehousedetailstemp"] != null)
                    {
                        DataTable Warehousedtss = (DataTable)Session["SRwarehousedetailstemp"];

                        DataRow[] dr = Warehousedtss.Select("productid = '" + Convert.ToString(hdfProductIDPC.Value) + "' AND pcslno = '" + hdnpcslno.Value + "'");
                        if (dr.Count() > 0)
                        {

                            Warehousedtss = dr.CopyToDataTable();
                            Warehousedtss.DefaultView.Sort = "SrlNo asc";
                            Warehousedtss = Warehousedtss.DefaultView.ToTable(true);

                            Session["SRWarehouseDatabyslno"] = Warehousedtss;

                            dt2 = (DataTable)Session["SRWarehouseDatabyslno"];
                            DataTable dtmp = dsInst.Tables[0];

                            if (Session["SRwarehousedetailstempUpdate"] != null)
                            {
                                DataRow[] drs = Warehousedtss.Select("productid = '" + Convert.ToString(hdfProductIDPC.Value) + "' AND pcslno = '" + hdnpcslno.Value + "' AND isnew='Updated'");
                                if (drs.Count() <= 0)
                                {
                                    dt2.Merge(dtmp);
                                }
                            }

                            dt = dt2;

                        }
                        else
                        {
                            dt = dsInst.Tables[0];
                        }

                    }
                    else
                    {
                        dt = dsInst.Tables[0];
                    }

                }
            }


            else if (Convert.ToString(Session["SRM_ActionType"]) != "Edit")
            {
                DataSet dsInst = new DataSet();

               // SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));



                SqlCommand cmd = new SqlCommand("prc_StockGetPCwarehousentry", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@StockID", Convert.ToInt32(stockid));
                cmd.Parameters.AddWithValue("@ProductID", Convert.ToInt32(hdfProductIDPC.Value));
                cmd.Parameters.AddWithValue("@branchID", Convert.ToInt32(hdbranchIDPC.Value));
                cmd.Parameters.AddWithValue("@compnay", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@Finyear", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@PCNumber", Convert.ToString(hdnInnumber.Value));
                cmd.Parameters.AddWithValue("@ISfromPO", "MainSalesInvoiceReturnWarehouse");
                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                con.Dispose();

                if (dsInst.Tables != null)
                {

                    if (Session["SRwarehousedetailstemp"] != null)
                    {
                        DataTable Warehousedtss = (DataTable)Session["SRwarehousedetailstemp"];

                        DataRow[] dr = Warehousedtss.Select("productid = '" + Convert.ToString(hdfProductIDPC.Value) + "' AND pcslno = '" + hdnpcslno.Value + "'");
                        if (dr.Count() > 0)
                        {

                            Warehousedtss = dr.CopyToDataTable();
                            Warehousedtss.DefaultView.Sort = "SrlNo asc";
                            Warehousedtss = Warehousedtss.DefaultView.ToTable(true);

                            Session["sRWarehouseDatabyslno"] = Warehousedtss;

                            dt2 = (DataTable)Session["SRWarehouseDatabyslno"];
                            DataTable dtmp = dsInst.Tables[0];

                            if (Session["SRwarehousedetailstempUpdate"] != null)
                            {
                                DataRow[] drs = Warehousedtss.Select("productid = '" + Convert.ToString(hdfProductIDPC.Value) + "' AND pcslno = '" + hdnpcslno.Value + "' AND isnew='Updated'");
                                if (drs.Count() <= 0)
                                {
                                    dt2.Merge(dtmp);
                                }
                            }

                            dt = dt2;

                        }
                        else
                        {
                            dt = dsInst.Tables[0];
                        }

                    }
                    else
                    {
                        dt = dsInst.Tables[0];
                    }

                }
            }
            else if (Session["SRwarehousedetailstemp"] != null)
            {
                DataTable Warehousedtss = (DataTable)Session["SRwarehousedetailstemp"];

                DataRow[] dr = Warehousedtss.Select("productid = '" + Convert.ToString(hdfProductIDPC.Value) + "' AND pcslno = '" + hdnpcslno.Value + "'");
                if (dr.Count() > 0)
                {

                    Warehousedtss = dr.CopyToDataTable();
                    Warehousedtss.DefaultView.Sort = "SrlNo asc";
                    Warehousedtss = Warehousedtss.DefaultView.ToTable(true);

                    Session["SRWarehouseDatabyslno"] = Warehousedtss;

                    dt = (DataTable)Session["SRWarehouseDatabyslno"];

                }

            }

            return dt;
        }

        private string getinventry()
        {

            string inventryType = string.Empty;
            if (hdniswarehouse.Value == "true")
            {
                inventryType = "WH";

            }
            if (hdnisbatch.Value == "true")
            {

                inventryType += "BT";
            }
            if (hdnisserial.Value == "true")
            {
                inventryType += "SL";

            }
            return inventryType;
        }

        public string SetVisibilityStock(object container)
        {
            string vs = string.Empty;
            GridViewDataItemTemplateContainer c = container as GridViewDataItemTemplateContainer;
            if (!string.IsNullOrEmpty(Convert.ToString(c.KeyValue)))
            {
                //if (isexist)
                //{
                vs = "display:block";
                //}
                //else
                //{
                //    vs = "display:none";
                //}

            }
            else
            {
                vs = "display:block";
            }

            return vs;


        }

        public void deleteALL()
        {

            #region delete

            if (Session["SRwarehousedetailstempDelete"] != null)
            {
                string stockid = string.Empty;
                DataTable Warehousedtups = (DataTable)Session["SRwarehousedetailstempDelete"];
                for (int i = 0; i <= Warehousedtups.Rows.Count - 1; i++)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(Warehousedtups.Rows[i]["BatchWarehouseID"])) && Convert.ToString(Warehousedtups.Rows[i]["BatchWarehouseID"]) != "0")
                    {
                        stockid = Convert.ToString(Warehousedtups.Rows[i]["StockID"]);

                        if (Convert.ToString(Warehousedtups.Rows[i]["isnew"]) == "DeleteSL")
                        {
                            string sqls = "delete Trans_StockSerialMapping where Stock_MapId=" + Convert.ToString(Warehousedtups.Rows[i]["SerialID"]) + " " +
                                               " update Trans_Stock set Stock_In=isnull(Stock_In,0)-1," +
                                       " Stock_ModifiedDate=GETDATE() WHERE Stock_ID=" + stockid + "	";

                            oDBEngine.GetDataTable(sqls);
                        }
                        else if (Convert.ToString(Warehousedtups.Rows[i]["isnew"]) == "DeleteWHBTSL")
                        {
                            var updateqnty = Warehousedtups.Compute("sum(Quantitysum)", "isnew = 'DeleteWHBTSL' AND BatchWarehouseID='" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehouseID"]) + "'");

                            string sqls = "delete from Trans_StockBranchWarehouseDetails where StockBranchWarehouseDetail_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehousedetailsID"]) + " " +
                                          " delete from Trans_StockBranchWarehouse where StockBranchWarehouse_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehouseID"]) + " " +
                                          " delete from Trans_StockBranchBatch where StockBranchWarehouseDetail_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehousedetailsID"]) + " " +
                                          " delete from Trans_StockSerialMapping where Stock_BranchBatchId=" + Convert.ToString(Warehousedtups.Rows[i]["BatchID"]) + " update Trans_Stock set Stock_In=isnull(Stock_In,0)-" + updateqnty + "," +
                                       " Stock_ModifiedDate=GETDATE() WHERE Stock_ID=" + stockid + "	";
                            if (!string.IsNullOrEmpty(Convert.ToString(updateqnty)))
                            {
                                oDBEngine.GetDataTable(sqls);
                            }

                        }
                        else if (Convert.ToString(Warehousedtups.Rows[i]["isnew"]) == "DeleteWHSL")
                        {

                            var updateqnty = Warehousedtups.Compute("sum(Quantitysum)", "isnew = 'DeleteWHSL' AND BatchWarehouseID='" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehouseID"]) + "'");

                            string sqls = "delete from Trans_StockBranchWarehouseDetails where StockBranchWarehouseDetail_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehousedetailsID"]) + " " +
                                          " delete from Trans_StockBranchWarehouse where StockBranchWarehouse_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehouseID"]) + " " +
                                          " delete from Trans_StockBranchBatch where StockBranchWarehouseDetail_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehousedetailsID"]) + " " +
                                          " delete from Trans_StockSerialMapping where Stock_BranchBatchId=" + Convert.ToString(Warehousedtups.Rows[i]["BatchID"]) + " update Trans_Stock set Stock_In=isnull(Stock_In,0)-" + updateqnty + "," +
                                       " Stock_ModifiedDate=GETDATE() WHERE Stock_ID=" + stockid + "	";
                            if (!string.IsNullOrEmpty(Convert.ToString(updateqnty)))
                            {
                                oDBEngine.GetDataTable(sqls);
                            }


                        }
                        else if (Convert.ToString(Warehousedtups.Rows[i]["isnew"]) == "DeleteWHBT")
                        {
                            string sqls = string.Empty;
                            var updateqnty = Warehousedtups.Compute("sum(Quantitysum)", "isnew = 'DeleteWHBT' AND BatchWarehouseID='" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehouseID"]) + "'");

                            if (string.IsNullOrEmpty(Convert.ToString(updateqnty)))
                            { updateqnty = 0.0; }


                            if (stockid != "0")
                            {
                                DataSet dsEmail = new DataSet();

                                //String conn = ConfigurationSettings.AppSettings["DBConnectionDefault"];
                                String conn = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);




                                SqlConnection con = new SqlConnection(conn);
                                SqlCommand cmd3 = new SqlCommand("prc_StockPchallanwarehousbatchDelete", con);
                                cmd3.CommandType = CommandType.StoredProcedure;

                                cmd3.Parameters.AddWithValue("@compnay", Convert.ToString(Session["LastCompany"]));
                                cmd3.Parameters.AddWithValue("@Finyear", Convert.ToString(Session["LastFinYear"]));
                                cmd3.Parameters.AddWithValue("@StockID", Convert.ToInt32(stockid));

                                cmd3.Parameters.AddWithValue("@BatchWarehouseID", Convert.ToInt32(Warehousedtups.Rows[i]["BatchWarehouseID"]));
                                cmd3.Parameters.AddWithValue("@BatchWarehousedetailsID", Convert.ToInt32(Warehousedtups.Rows[i]["BatchWarehousedetailsID"]));
                                cmd3.Parameters.AddWithValue("@BatchID", Convert.ToInt32(Warehousedtups.Rows[i]["BatchID"]));


                                //cmd3.Parameters.AddWithValue("@branchID", Convert.ToInt32(hdnselectedbranch.Value));
                                cmd3.Parameters.AddWithValue("@modifiedBy", Convert.ToInt32(HttpContext.Current.Session["userid"]));
                                cmd3.Parameters.AddWithValue("@rate", Convert.ToDecimal(hdnrate.Value));
                                cmd3.Parameters.AddWithValue("@updateqnty", Convert.ToDecimal(updateqnty));

                                SqlParameter output = new SqlParameter("@ReturnValue", SqlDbType.Int);
                                output.Direction = ParameterDirection.Output;
                                cmd3.Parameters.Add(output);
                                cmd3.CommandTimeout = 0;
                                SqlDataAdapter Adap = new SqlDataAdapter();
                                Adap.SelectCommand = cmd3;
                                Adap.Fill(dsEmail);
                                dsEmail.Clear();
                                cmd3.Dispose();
                                con.Dispose();
                                GC.Collect();
                            }
                        }
                        else if (Convert.ToString(Warehousedtups.Rows[i]["isnew"]) == "DeleteWH")
                        {

                            var updateqnty = Warehousedtups.Compute("sum(Quantity)", "isnew = 'DeleteWH' AND BatchWarehouseID='" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehouseID"]) + "'");

                            string sqls = "delete from Trans_StockBranchWarehouseDetails where StockBranchWarehouseDetail_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehousedetailsID"]) + " " +
                                          " delete from Trans_StockBranchWarehouse where StockBranchWarehouse_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehouseID"]) + " " +
                                          " delete from Trans_StockBranchBatch where StockBranchWarehouseDetail_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehousedetailsID"]) + "  update Trans_Stock set Stock_In=isnull(Stock_In,0)-" + updateqnty + "," +
                                       " Stock_ModifiedDate=GETDATE() WHERE Stock_ID=" + stockid + "	";
                            if (!string.IsNullOrEmpty(Convert.ToString(updateqnty)))
                            {
                                oDBEngine.GetDataTable(sqls);
                            }
                        }
                    }


                }


            }
            #endregion

        }





        private int Insertwaredataalldata(string ProductID, string stockid, string branchid)
        {
            DataSet dsEmail = new DataSet();
            string inventryType = string.Empty;
            string IsserialActive = "false";

            int newrowcount = 0;

            #region Insert
           // String conn = ConfigurationSettings.AppSettings["DBConnectionDefault"];

            String conn = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);


            SqlConnection con = new SqlConnection(conn);
            SqlCommand cmd3 = new SqlCommand("prc_StockPChallanwarehousentry", con);
            cmd3.CommandType = CommandType.StoredProcedure;

            cmd3.Parameters.AddWithValue("@compnay", Convert.ToString(Session["LastCompany"]));
            cmd3.Parameters.AddWithValue("@Finyear", Convert.ToString(Session["LastFinYear"]));
            cmd3.Parameters.AddWithValue("@StockID", Convert.ToInt32(stockid));
            cmd3.Parameters.AddWithValue("@ProductID", Convert.ToInt32(ProductID));
            cmd3.Parameters.AddWithValue("@branchID", Convert.ToInt32(branchid));
            cmd3.Parameters.AddWithValue("@createdBy", Convert.ToInt32(HttpContext.Current.Session["userid"]));
            cmd3.Parameters.AddWithValue("@PCNumber", Convert.ToInt32(HttpContext.Current.Session["userid"]));
            cmd3.Parameters.AddWithValue("@totalqntity", Convert.ToString(hdntotalqntyPC.Value));


            if (hdniswarehouse.Value == "true")
            {
                inventryType = "WH";

            }
            if (hdnisbatch.Value == "true")
            {

                inventryType += "BT";
            }
            if (hdnisserial.Value == "true")
            {
                inventryType += "SL";
                IsserialActive = "true";
            }

            cmd3.Parameters.AddWithValue("@IsSerialActive", IsserialActive);


            cmd3.Parameters.AddWithValue("@Inventrytype", inventryType);

            DataTable temtable = new DataTable();
            DataTable temtable2 = new DataTable();
            DataTable temtable23 = new DataTable();
            if (Session["SRM_WarehouseData"] != null)
            {
                DataTable Warehousedt = (DataTable)Session["SRM_WarehouseData"];

                temtable = Warehousedt.DefaultView.ToTable(false, "SrlNo", "BatchWarehouseID", "BatchWarehousedetailsID", "BatchID", "SerialID", "WarehouseID", "WarehouseName", "BatchNo", "SerialNo", "MFGDate", "ExpiryDate", "Quantitysum", "productid", "Inventrytype", "StockID", "isnew");
                cmd3.Parameters.AddWithValue("@udt_StockOpeningwarehousentried", temtable);
            }
            if (Session["SRWarehouseDataDelete"] != null)
            {
                DataTable Warehousedts = (DataTable)Session["SRWarehouseDataDelete"];

                temtable2 = Warehousedts.DefaultView.ToTable(false, "SrlNo", "BatchWarehouseID", "BatchWarehousedetailsID", "BatchID", "SerialID", "WarehouseID", "WarehouseName", "BatchNo", "SerialNo", "MFGDate", "ExpiryDate", "Quantity", "isnew");
                cmd3.Parameters.AddWithValue("@udt_StockOpeningwarehousentrieDelete", null);
            }

            #endregion

            return 1;


        }
        #endregion

        #endregion

        #region Unique Code Generated Section Start

        public string checkNMakeJVCode(string manual_str, int sel_schema_Id)
        {

          //  oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
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

                    //sqlQuery = "SELECT max(tjv.Return_Number) FROM tbl_trans_salesreturn tjv WHERE dbo.RegexMatch('";
                    //if (prefLen > 0)
                    //    sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";

                    //sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                    //if (sufxLen > 0)
                    //    sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                    //sqlQuery += "?$', LTRIM(RTRIM(tjv.Return_Number))) = 1";

                    //dtC = oDBEngine.GetDataTable(sqlQuery);

                    if (!suppressZero)
                    {
                        sqlQuery = "SELECT max(tjv.Return_Number) FROM tbl_trans_salesreturn tjv WHERE dbo.RegexMatch('";
                        if (prefLen > 0)
                            sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                        else if (scheme_type == 2)
                            sqlQuery += "^";
                        sqlQuery += "[0-9]{" + paddCounter + "}";
                        if (sufxLen > 0)
                            sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                        //sqlQuery += "?$', LTRIM(RTRIM(tjv.Return_Number))) = 1";
                        sqlQuery += "?$', LTRIM(RTRIM(tjv.Return_Number))) = 1 and Return_Number like '" + prefCompCode + "%'";
                        if (scheme_type == 2)
                            sqlQuery += " AND CONVERT(DATE, Return_Date) = CONVERT(DATE, GETDATE())";
                        dtC = oDBEngine.GetDataTable(sqlQuery);
                    }
                    else
                    {

                        int i = startNo.Length;
                        while (i < paddCounter)
                        {


                            sqlQuery = "SELECT max(tjv.Return_Number) FROM tbl_trans_salesreturn tjv WHERE dbo.RegexMatch('";
                            if (prefLen > 0)
                                sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                            sqlQuery += "[0-9]{" + i + "}";
                            if (sufxLen > 0)
                                sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                            //sqlQuery += "?$', LTRIM(RTRIM(tjv.Invoice_Number))) = 1";
                            sqlQuery += "?$', LTRIM(RTRIM(tjv.Return_Number))) = 1 and Return_Number like '" + prefCompCode + "%'";

                            if (prefLen == 0 && sufxLen == 0)
                            {
                                sqlQuery += " and LEN(tjv.Return_Number)=" + i;
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
                            sqlQuery = "SELECT max(tjv.Return_Number) FROM tbl_trans_salesreturn tjv WHERE dbo.RegexMatch('";
                            if (prefLen > 0)
                                sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                            sqlQuery += "[0-9]{" + (i - 1) + "}";
                            if (sufxLen > 0)
                                sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                            //sqlQuery += "?$', LTRIM(RTRIM(tjv.Invoice_Number))) = 1";
                            sqlQuery += "?$', LTRIM(RTRIM(tjv.Return_Number))) = 1 and Return_Number like '" + prefCompCode + "%'";

                            if (prefLen == 0 && sufxLen == 0)
                            {
                                sqlQuery += " and LEN(tjv.Return_Number)=" + (i - 1);
                            }
                            dtC = oDBEngine.GetDataTable(sqlQuery);
                        }
                    }

                    if (dtC.Rows[0][0].ToString() == "")
                    {
                        sqlQuery = "SELECT max(tjv.Return_Number) FROM tbl_trans_salesreturn tjv WHERE dbo.RegexMatch('";
                        if (prefLen > 0)
                            sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                        else if (scheme_type == 2)
                            sqlQuery += "^";
                        sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                        if (sufxLen > 0)
                            sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                        //sqlQuery += "?$', LTRIM(RTRIM(tjv.Return_Number))) = 1";
                        sqlQuery += "?$', LTRIM(RTRIM(tjv.Return_Number))) = 1 and Return_Number like '" + prefCompCode + "%'";
                        if (scheme_type == 2)
                            sqlQuery += " AND CONVERT(DATE, Return_Date) = CONVERT(DATE, GETDATE())";
                        if (prefLen == 0 && sufxLen == 0)
                        {
                            sqlQuery += " and LEN(tjv.Return_Number)=" + (paddCounter - 1);
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
                    sqlQuery = "SELECT Return_Number FROM tbl_trans_salesreturn WHERE Return_Number LIKE '" + manual_str.Trim() + "'";
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

        #endregion Unique Code Generated Section End

        #region Debu


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
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesReturn_Details");
            proc.AddVarcharPara("@Action", 500, "TaxDetailsForGst");
            proc.AddVarcharPara("@SalesReturnID", 500, Convert.ToString(Session["SRM_ReturnID"]));
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
            bnrOtherChargesvalue.Text = Convert.ToString(Math.Round(Convert.ToDecimal(totalCharges.ToString()), 2));
           
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
                DataTable dt2 = oDBEngine.GetDataTable("Select dbo.fn_CheckAvailableQuotation(" + strBranch + ",'" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]) + "'," + strProductID + ") as branchopenstock");

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
        protected void taxUpdatePanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {

            string performpara = e.Parameter;
            if (performpara.Split('~')[0] == "DelProdbySl")
            {
                DataTable MainTaxDataTable = (DataTable)Session["SRM_FinalTaxRecord"];

                DataRow[] deletedRow = MainTaxDataTable.Select("SlNo=" + performpara.Split('~')[1]);
                if (deletedRow.Length > 0)
                {
                    foreach (DataRow dr in deletedRow)
                    {
                        MainTaxDataTable.Rows.Remove(dr);
                    }

                }

                Session["SRM_FinalTaxRecord"] = MainTaxDataTable;
                GetStock(Convert.ToString(performpara.Split('~')[1]));
                // DeleteWarehouse(Convert.ToString(performpara.Split('~')[1]));
                DataTable taxDetails = (DataTable)Session["SRM_TaxDetails"];
                if (taxDetails != null)
                {
                    foreach (DataRow dr in taxDetails.Rows)
                    {
                        dr["Amount"] = "0.00";
                    }
                    Session["SRM_TaxDetails"] = taxDetails;
                }
            }
            else if (performpara.Split('~')[0] == "DeleteAllTax")
            {
                CreateDataTaxTable();

                DataTable taxDetails = (DataTable)Session["SRM_TaxDetails"];

                if (taxDetails != null)
                {
                    foreach (DataRow dr in taxDetails.Rows)
                    {
                        dr["Amount"] = "0.00";
                    }
                    Session["SRM_TaxDetails"] = taxDetails;
                }
            }
            else
            {
                DataTable MainTaxDataTable = (DataTable)Session["SRM_FinalTaxRecord"];

                DataRow[] deletedRow = MainTaxDataTable.Select("SlNo=" + performpara.Split('~')[1]);
                if (deletedRow.Length > 0)
                {
                    foreach (DataRow dr in deletedRow)
                    {
                        MainTaxDataTable.Rows.Remove(dr);
                    }

                }

                Session["SRM_FinalTaxRecord"] = MainTaxDataTable;

                DataTable taxDetails = (DataTable)Session["SRM_TaxDetails"];

                if (taxDetails != null)
                {
                    foreach (DataRow dr in taxDetails.Rows)
                    {
                        dr["Amount"] = "0.00";
                    }
                    Session["SRM_TaxDetails"] = taxDetails;
                }
            }
        }
        public double ReCalculateTaxAmount(string slno, double amount)
        {
            DataTable MainTaxDataTable = (DataTable)Session["SRM_FinalTaxRecord"];
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
            Session["SRM_FinalTaxRecord"] = MainTaxDataTable;

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
            Session["SRM_FinalTaxRecord"] = TaxRecord;
        }

        public static void CreateDataTaxTableByajax()
        {
            DataTable TaxRecord = new DataTable();

            TaxRecord.Columns.Add("SlNo", typeof(System.Int32));
            TaxRecord.Columns.Add("TaxCode", typeof(System.String));
            TaxRecord.Columns.Add("AltTaxCode", typeof(System.String));
            TaxRecord.Columns.Add("Percentage", typeof(System.Decimal));
            TaxRecord.Columns.Add("Amount", typeof(System.Decimal));
            HttpContext.Current.Session["SRM_FinalTaxRecord"] = TaxRecord;
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
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesReturn_Details");
            proc.AddVarcharPara("@Action", 500, "ProductTaxDetails");
            proc.AddVarcharPara("@SalesReturnID", 500, Convert.ToString(Session["SRM_ReturnID"]));
            ds = proc.GetDataSet();
            return ds;
        }
        public DataSet GetQuotationEditedTaxData()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesReturn_Details");
            proc.AddVarcharPara("@Action", 500, "ProductEditedTaxDetails");
            proc.AddVarcharPara("@SalesReturnID", 500, Convert.ToString(Session["SRM_ReturnID"]));
            ds = proc.GetDataSet();
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
            decimal TaxCalAmt = 0;
            string retMsg = "";
            if (e.Parameters.Split('~')[0] == "SaveGST")
            {
                DataTable TaxRecord = (DataTable)Session["SRM_FinalTaxRecord"];
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

                Session["SRM_FinalTaxRecord"] = TaxRecord;
            }
            else
            {
                string CustomerStateCode = string.Empty;
                #region fetch All data For Tax

                DataTable taxDetail = new DataTable();

                DataTable CustomerStateCodeDT = new DataTable();
                DataTable MainTaxDataTable = (DataTable)Session["SRM_FinalTaxRecord"];
                DataTable databaseReturnTable = (DataTable)Session["SRM_QuotationTaxDetails"];

                //if (Convert.ToInt32(e.Parameters.Split('~')[1]) == 1)
                //    taxDetail = oDBEngine.GetDataTable("select TaxRates_ID as Taxes_ID,TaxRatesSchemeName as Taxes_Name,TaxCalculateMethods,tm.Taxes_Name as taxCodeName,tm.Taxes_ApplicableOn as 'ApplicableOn',isnull((select TaxRatesSchemeName from Config_TaxRates where TaxRates_ID=tm.Taxes_OtherTax),'')  as dependOn from Master_taxes tm inner join Config_TaxRates ct on tm.Taxes_ID=ct.TaxRates_TaxCode where tm.TaxTypeCode not in('G','V','C') and tm.TaxItemlevel='Y'");
                //else if (Convert.ToInt32(e.Parameters.Split('~')[1]) == 2)
                //taxDetail = oDBEngine.GetDataTable("select TaxRates_ID as Taxes_ID,TaxRatesSchemeName as Taxes_Name,TaxCalculateMethods,tm.Taxes_Name as taxCodeName,tm.Taxes_ApplicableOn as 'ApplicableOn',isnull((select TaxRatesSchemeName from Config_TaxRates where TaxRates_ID=tm.Taxes_OtherTax),'')  as dependOn from Master_taxes tm inner join Config_TaxRates ct on tm.Taxes_ID=ct.TaxRates_TaxCode where tm.TaxTypeCode not in('G','V','C') and tm.TaxItemlevel='Y'");

                ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
                proc.AddVarcharPara("@Action", 500, "LoadOtherTaxDetails");
                proc.AddVarcharPara("@ProductID", 10, Convert.ToString(setCurrentProdCode.Value));
                proc.AddVarcharPara("@S_quoteDate", 10, dt_PLQuote.Date.ToString("yyyy-MM-dd"));
                taxDetail = proc.GetTable();





                //Get Company Gstin 09032017
                string CompInternalId = Convert.ToString(Session["LastCompany"]);
                string[] compGstin = oDBEngine.GetFieldValue1("tbl_master_company", "cmp_gstin", "cmp_internalid='" + CompInternalId + "'", 1);


                //Get CustomerStateCode

                // string CustomerStateCode = Convert.ToString(hdnCustomerStateCodeId.Value);

                //chinmoy comment below line for cgst & sgst mismatch 12-03-2019

                //start

  //              if (hdnCustomerStateCodeId.Value != null && hdnCustomerStateCodeId.Value != "")
  //              {
  //                  CustomerStateCode = Convert.ToString(hdnCustomerStateCodeId.Value);
  //              }
  //              else
  //              {
  //ProcedureExecute procCustomerStateCode = new ProcedureExecute("prc_CRMSalesReturn_Details");
  //                  procCustomerStateCode.AddVarcharPara("@Action", 500, "GetCustomerStateCode");
  //                  procCustomerStateCode.AddVarcharPara("@CustomerID", 100, Convert.ToString(hdnCustomerId.Value));

  //                  CustomerStateCodeDT = procCustomerStateCode.GetTable();
  //                  if (CustomerStateCodeDT != null && CustomerStateCodeDT.Rows.Count > 0)
  //                  {
  //                      CustomerStateCode = Convert.ToString(CustomerStateCodeDT.Rows[0]["StateCode"]);

  //                  }
                 
  //              }

                //End

                //Get BranchStateCode
                string BranchStateCode = "", BranchGSTIN = "";
                DataTable BranchTable = oDBEngine.GetDataTable("select StateCode,branch_GSTIN   from tbl_master_branch branch inner join tbl_master_state st on branch.branch_state=st.id where branch_id=" + Convert.ToString(ddl_Branch.SelectedValue));
                if (BranchTable != null)
                {
                    BranchStateCode = Convert.ToString(BranchTable.Rows[0][0]);
                    BranchGSTIN = Convert.ToString(BranchTable.Rows[0][1]);

                    if (BranchGSTIN.Trim() != "")
                    {
                        BranchStateCode = BranchGSTIN.Substring(0, 2);
                    }

                }

                if (BranchGSTIN.Trim() == "")
                {
                    BranchStateCode = compGstin[0].Substring(0, 2);
                }

                //if (CustomerStateCode == null || CustomerStateCode == "")
                //{
                //    CustomerStateCode = BranchStateCode;
                //}


                string VendorState = "";
                string sstateCode = "";
                if (ddlPosGstReturnManual.Value.ToString() == "S")
                {
                  //  sstateCode = Purchase_BillingShipping.GeteShippingStateCode();
                    CustomerStateCode = hdnPOSShippingStateCode.Value;
                }
                else
                {
                    //sstateCode = Purchase_BillingShipping.GetBillingStateCode();
                    CustomerStateCode = hdnPOSBillingStateCode.Value;
                }


               // VendorState = sstateCode;
                VendorState = BranchStateCode;
                if (VendorState.Trim() != "")
                {
                    VendorState = VendorState;
                }



                if (VendorState.Trim() != "" && CustomerStateCode != "")
                {


                    if (CustomerStateCode == VendorState)
                    {
                        //Check if the state is in union territories then only UTGST will apply
                        //   Chandigarh     Andaman and Nicobar Islands    DADRA & NAGAR HAVELI    DAMAN & DIU                  Lakshadweep              PONDICHERRY
                        if (VendorState == "4" || VendorState == "26" || VendorState == "25" || VendorState == "35" || VendorState == "31" || VendorState == "34")
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
                //if ((compGstin[0].Trim() == "" && BranchGSTIN == "") || VendorState == "")
                //{
                //    foreach (DataRow dr in taxDetail.Rows)
                //    {
                //        if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST")
                //        {
                //            dr.Delete();
                //        }
                //    }
                //    taxDetail.AcceptChanges();
                //}

                if (CustomerStateCode == "" || VendorState == "")
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

                string ShippingState = "";


                #region oldtax
                //string ShippingState = "";






                //// Date: 31-05-2017    Author: Kallol Samanta  [START] 
                //// Details: Billing/Shipping user control integration 

                ////New Code
                //string sstateCode = BillingShippingControl.GetShippingStateCode(Request.QueryString["key"]);
                //ShippingState = sstateCode;
                //if (ShippingState.Trim() != "")
                //{
                //    ShippingState = ShippingState.Substring(ShippingState.IndexOf("(State Code:")).Replace("(State Code:", "").Replace(")", "");
                //}

                ////Old Code
                ////if (chkBilling.Checked)
                ////{
                ////    if (CmbState.Value != null)
                ////    {
                ////        ShippingState = CmbState.Text;
                ////        ShippingState = ShippingState.Substring(ShippingState.IndexOf("(State Code:")).Replace("(State Code:", "").Replace(")", "");
                ////    }
                ////}
                ////else
                ////{
                ////    if (CmbState1.Value != null)
                ////    {
                ////        ShippingState = CmbState1.Text;
                ////        ShippingState = ShippingState.Substring(ShippingState.IndexOf("(State Code:")).Replace("(State Code:", "").Replace(")", "");
                ////    }
                ////}

                //// Date: 31-05-2017    Author: Kallol Samanta  [END]











                //if (ShippingState.Trim() != "" && compGstin[0].Trim() != "")
                //{

                //    if (compGstin.Length > 0)
                //    {
                //        if (compGstin[0].Substring(0, 2) == ShippingState)
                //        {
                //            //Check if the state is in union territories then only UTGST will apply
                //            //   Chandigarh     Andaman and Nicobar Islands    DADRA & NAGAR HAVELI    DAMAN & DIU    Lakshadweep              PONDICHERRY
                //            if (ShippingState == "4" || ShippingState == "26" || ShippingState == "25" || ShippingState == "35" || ShippingState == "31" || ShippingState == "34")
                //            {
                //                foreach (DataRow dr in taxDetail.Rows)
                //                {

                //                    //debu 7-7-2017
                //                    if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST"  || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST")
                //                    {
                //                        dr.Delete();
                //                    }
                //                }

                //            }
                //            else
                //            {
                //                foreach (DataRow dr in taxDetail.Rows)
                //                {
                //                    if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
                //                    {
                //                        dr.Delete();
                //                    }
                //                }
                //            }
                //            taxDetail.AcceptChanges();
                //        }
                //        else
                //        {
                //            foreach (DataRow dr in taxDetail.Rows)
                //            {
                //                if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
                //                {
                //                    dr.Delete();
                //                }
                //            }
                //            taxDetail.AcceptChanges();

                //        }

                //    }
                //}

                ////If Company GSTIN is blank then Delete All CGST,UGST,IGST,CGST
                //if (compGstin[0].Trim() == "")
                //{
                //    foreach (DataRow dr in taxDetail.Rows)
                //    {
                //        if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST")
                //        {
                //            dr.Delete();
                //        }
                //    }
                //    taxDetail.AcceptChanges();
                //}

                ////Check If any TaxScheme Set Against that Product Then update there rate 22-03-2017 and rate
                //string[] schemeIDViaProdID = oDBEngine.GetFieldValue1("master_sproducts", "isnull(sProduct_TaxSchemeSale,0)sProduct_TaxSchemeSale", "sProducts_ID='" + Convert.ToString(setCurrentProdCode.Value) + "'", 1);
                ////&& schemeIDViaProdID[0] != ""
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

                #endregion

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
                aspxGridTax.JSProperties["cpTotalGST"] = totalParcentage;
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
                            //if (Convert.ToString(ddl_VatGstCst.Value) == "0~0~X")
                            //{
                                if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST")
                                {
                                    decimal finalCalCulatedOn = 0;
                                    decimal backProcessRate = (1 + (totalParcentage / 100));
                                    //if (Convert.ToString(dr["ApplicableOn"]) == "G")
                                    //{
                                        finalCalCulatedOn = obj.calCulatedOn / backProcessRate;
                                   // }
                                    //else if (Convert.ToString(dr["ApplicableOn"]) == "N")
                                    //{
                                    //    finalCalCulatedOn = obj.calCulatedOn;
                                    //}
                                    obj.calCulatedOn = Math.Round(finalCalCulatedOn, 2);
                                }
                            //}
                        }

                        if (Convert.ToString(dr["TaxCalculateMethods"]) == "A")
                        {
                            obj.Taxes_Name = Convert.ToString(dr["Taxes_Name"]) + "(+)";

                        }
                        else
                        {
                            obj.Taxes_Name = Convert.ToString(dr["Taxes_Name"]) + "(-)";
                        }

                        obj.Amount = Convert.ToDouble(Math.Round(Convert.ToDecimal(obj.calCulatedOn * (Convert.ToDecimal(obj.TaxField) / 100)), 2));

                        // obj.Amount = Math.Round(Convert.ToDecimal(obj.calCulatedOn * (Convert.ToDecimal(obj.TaxField) / 100)), 2);


                        DataRow[] filtr = MainTaxDataTable.Select("TaxCode ='" + obj.Taxes_ID + "' and slNo=" + Convert.ToString(slNo));
                        if (filtr.Length > 0)
                        {
                           // obj.Amount = Convert.ToDouble(filtr[0]["Amount"]);
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

                    DataTable TaxRecord = (DataTable)Session["SRM_FinalTaxRecord"];


                    foreach (DataRow dr in taxDetail.Rows)
                    {
                        TaxDetails obj = new TaxDetails();
                        obj.Taxes_ID = Convert.ToInt32(dr["Taxes_ID"]);
                        obj.taxCodeName = Convert.ToString(dr["taxCodeName"]);

                        if (Convert.ToString(dr["TaxCalculateMethods"]) == "A")
                            obj.Taxes_Name = Convert.ToString(dr["Taxes_Name"]) + "(+)";
                        else
                            obj.Taxes_Name = Convert.ToString(dr["Taxes_Name"]) + "(-)";
                      //  obj.TaxField = "";
                        obj.TaxField= Convert.ToString(dr["TaxRates_Rate"]);
                        obj.Amount = 0.0;

                        #region set calculated on

                        //Check Tax Applicable on and set to calculated on
                        if (Convert.ToString(dr["ApplicableOn"]) == "G")
                        {
                            obj.calCulatedOn = ProdGrossAmt;
                            TaxCalAmt = ProdGrossAmt;
                        }
                        else if (Convert.ToString(dr["ApplicableOn"]) == "N")
                        {
                            obj.calCulatedOn = ProdNetAmt;
                            TaxCalAmt = ProdNetAmt;
                        }
                        else
                        {
                            obj.calCulatedOn = 0;
                        }
                        obj.Amount = Convert.ToDouble(Math.Round(Convert.ToDecimal(obj.calCulatedOn * (Convert.ToDecimal(obj.TaxField) / 100)), 2));
                        //End Here
                        #endregion

                        //Debjyoti 09032017
                        if (Convert.ToString(ddl_AmountAre.Value) == "2")
                        {
                            if (Convert.ToString(ddl_VatGstCst.Value) == "0~0~X")
                            {
                                if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST")
                                {
                                    //decimal finalCalCulatedOn = 0;
                                    //decimal backProcessRate = (1 + (totalParcentage / 100));
                                    //finalCalCulatedOn = Math.Round(obj.calCulatedOn / backProcessRate, 2);
                                    //obj.calCulatedOn = Math.Round(finalCalCulatedOn, 2);


                                    decimal finalCalCulatedOn = 0;
                                    decimal backProcessRate = (1 + (totalParcentage / 100));
                                    //if (Convert.ToString(dr["ApplicableOn"]) == "G")
                                    //{
                                        finalCalCulatedOn = obj.calCulatedOn / backProcessRate;
                                    //}
                                    //else if (Convert.ToString(dr["ApplicableOn"]) == "N")
                                    //{
                                    //    finalCalCulatedOn = obj.calCulatedOn;
                                    //}
                                    obj.calCulatedOn = Math.Round(finalCalCulatedOn, 2);
                                    obj.Amount = Convert.ToDouble(Math.Round(Convert.ToDecimal(obj.calCulatedOn * (Convert.ToDecimal(obj.TaxField) / 100)), 2));
                                }
                            }
                        }
                       // obj.Amount = Convert.ToDouble(Math.Round(Convert.ToDecimal(obj.calCulatedOn * (Convert.ToDecimal(obj.TaxField) / 100)), 2));
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


                            //DataRow[] filtr = databaseReturnTable.Select("ProductTax_ProductId ='" + keyValue + "' and ProductTax_QuoteId=" +Convert.ToString( Session["QuotationID"] )+ " and ProductTax_TaxTypeId=" + obj.Taxes_ID);
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

                        //      DataRow[] filtrIndex = databaseReturnTable.Select("ProductTax_ProductId ='" + keyValue + "' and ProductTax_QuoteId=" + Session["QuotationID"] + " and ProductTax_TaxTypeId=0");
                        DataRow[] filtrIndex = TaxRecord.Select("SlNo=" + Convert.ToString(slNo) + " and TaxCode=0");
                        if (filtrIndex.Length > 0)
                        {
                            aspxGridTax.JSProperties["cpComboCode"] = Convert.ToString(filtrIndex[0]["AltTaxCode"]);
                        }
                    }
                    Session["SRM_FinalTaxRecord"] = TaxRecord;

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

                //decimal taxAmount=0;
                //foreach (var item in TaxDetailsDetails)
                //{
                //    taxAmount = taxAmount + Convert.ToDecimal(item.Amount);

                //}


                //foreach (var item in TaxDetailsDetails)
                //{
                //    item.calCulatedOn = TaxCalAmt-taxAmount;

                //}

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
            DataTable TaxRecord = (DataTable)Session["SRM_FinalTaxRecord"];
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


            Session["SRM_FinalTaxRecord"] = TaxRecord;


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
            //DataTable taxTableItemLvl = (DataTable)Session["FinalTaxRecord"];
            //foreach (DataRow dr in taxTableItemLvl.Rows)
            //    dr.Delete();
            //taxTableItemLvl.AcceptChanges();
            //Session["FinalTaxRecord"] = taxTableItemLvl;
        }

        protected void cmbGstCstVatcharge_Callback(object sender, CallbackEventArgsBase e)
        {
            Session["SRM_TaxDetails"] = null;
            DateTime quoteDate = Convert.ToDateTime(dt_PLQuote.Date.ToString("yyyy-MM-dd"));
            PopulateChargeGSTCSTVATCombo(quoteDate.ToString("yyyy-MM-dd"));
        }
        public DataTable getAllTaxDetails(DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            DataTable FinalTable = new DataTable();
            FinalTable.Columns.Add("SlNo", typeof(System.Int32));
            FinalTable.Columns.Add("TaxCode", typeof(System.String));
            FinalTable.Columns.Add("AltTaxCode", typeof(System.String));
            FinalTable.Columns.Add("Percentage", typeof(System.Decimal));
            FinalTable.Columns.Add("Amount", typeof(System.Decimal));

            //for insert
            foreach (var args in e.InsertValues)
            {
                string Slno = Convert.ToString(args.NewValues["SrlNo"]);
                DataRow existsRow;
                if (Session["ProdTax_" + Slno] != null)
                {
                    DataTable sessiontable = (DataTable)Session["ProdTax_" + Slno];
                    foreach (DataRow dr in sessiontable.Rows)
                    {
                        existsRow = FinalTable.NewRow();

                        existsRow["SlNo"] = Slno;
                        if (Convert.ToString(dr["taxCode"]).Contains("~"))
                        {
                            existsRow["TaxCode"] = "0";
                            existsRow["AltTaxCode"] = Convert.ToString(dr["taxCode"]).Split('~')[1];
                        }
                        else
                        {
                            existsRow["TaxCode"] = Convert.ToString(dr["taxCode"]);
                            existsRow["AltTaxCode"] = "0";
                        }

                        existsRow["Percentage"] = Convert.ToString(dr["Percentage"]);
                        existsRow["Amount"] = Convert.ToString(dr["Amount"]);

                        FinalTable.Rows.Add(existsRow);
                    }
                    Session.Remove("ProdTax_" + Slno);
                }
            }

            return FinalTable;
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
            if (Session["SRM_FinalTaxRecord"] != null)
            {
                TaxDetailTable = (DataTable)Session["SRM_FinalTaxRecord"];

                var rows = TaxDetailTable.Select("SlNo ='" + SrlNo + "'");
                foreach (var row in rows)
                {
                    row.Delete();
                }
                TaxDetailTable.AcceptChanges();

                Session["SRM_FinalTaxRecord"] = TaxDetailTable;
            }
        }
        public void UpdateTaxDetails(string oldSrlNo, string newSrlNo)
        {
            DataTable TaxDetailTable = new DataTable();
            if (Session["SRM_FinalTaxRecord"] != null)
            {
                TaxDetailTable = (DataTable)Session["SRM_FinalTaxRecord"];

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

                Session["SRM_FinalTaxRecord"] = TaxDetailTable;
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

        #region  Billing and Shipping Detail Subhra Section Start






        //Date: 31-05-2017    Author: Kallol Samanta  [START]
        //Details: Billing/Shipping user control integration

        /*
        //#region All address Dropdown Populated Data and function
        //public void PopulateBillingShippingDetails()
        //{
        //    DataTable dtaddbill = oDBEngine.GetDataTable("select add_addressType,add_address1,add_address2,add_address3,add_landMark,add_country,add_state,add_city,add_pin,add_area from tbl_master_address where add_cntId='LDP0000002' and add_addressType='Billing'");
        //    if (dtaddbill.Rows.Count > 0 && dtaddbill != null)
        //    {

        //        //for (int m = 0; m < dtaddbill.Rows.Count; m++)
        //        //{
        //        string add_addressType = Convert.ToString(dtaddbill.Rows[0]["add_addressType"]);
        //        string add_address1 = Convert.ToString(dtaddbill.Rows[0]["add_address1"]);
        //        string add_address2 = Convert.ToString(dtaddbill.Rows[0]["add_address2"]);
        //        string add_address3 = Convert.ToString(dtaddbill.Rows[0]["add_address3"]);
        //        string add_landMark = Convert.ToString(dtaddbill.Rows[0]["add_landMark"]);
        //        string add_country = Convert.ToString(dtaddbill.Rows[0]["add_country"]);
        //        string add_state = Convert.ToString(dtaddbill.Rows[0]["add_state"]);
        //        string add_city = Convert.ToString(dtaddbill.Rows[0]["add_city"]);
        //        string add_pin = Convert.ToString(dtaddbill.Rows[0]["add_pin"]);
        //        string add_area = Convert.ToString(dtaddbill.Rows[0]["add_area"]);
        //        CmbCountry.Value = add_country;

        //        //}



        //    }

        //    DataTable dtaship = oDBEngine.GetDataTable("select add_addressType,add_address1,add_address2,add_address3,add_landMark,add_country,add_state,add_city,add_pin,add_area from tbl_master_address where add_cntId='LDP0000002' and add_addressType='Shipping'");
        //    if (dtaship.Rows.Count > 0 && dtaship != null)
        //    {
        //        string add_saddressType = Convert.ToString(dtaship.Rows[0]["add_addressType"]);
        //        string add_saddress1 = Convert.ToString(dtaship.Rows[0]["add_address1"]);
        //        string add_saddress2 = Convert.ToString(dtaship.Rows[0]["add_address2"]);
        //        string add_saddress3 = Convert.ToString(dtaship.Rows[0]["add_address3"]);
        //        string add_slandMark = Convert.ToString(dtaship.Rows[0]["add_landMark"]);
        //        string add_scountry = Convert.ToString(dtaship.Rows[0]["add_country"]);
        //        string add_sstate = Convert.ToString(dtaship.Rows[0]["add_state"]);
        //        string add_scity = Convert.ToString(dtaship.Rows[0]["add_city"]);
        //        string add_spin = Convert.ToString(dtaship.Rows[0]["add_pin"]);
        //        string add_sarea = Convert.ToString(dtaship.Rows[0]["add_area"]);

        //        //combo.ClientSideEvents.SelectedIndexChanged = "OnSelectedIndexChanged";
        //    }
        //}

        //string[,] GetState(int country)
        //{
        //    StateSelect.SelectParameters[0].DefaultValue = Convert.ToString(country);
        //    DataView view = (DataView)StateSelect.Select(DataSourceSelectArguments.Empty);
        //    string[,] DATA = new string[view.Count, 2];
        //    for (int i = 0; i < view.Count; i++)
        //    {
        //        DATA[i, 0] = Convert.ToString(view[i][0]);
        //        DATA[i, 1] = Convert.ToString(view[i][1]);
        //    }
        //    return DATA;

        //}

        //protected void FillStateCombo(ASPxComboBox cmb, int country)
        //{

        //    string[,] state = GetState(country);
        //    cmb.Items.Clear();

        //    for (int i = 0; i < state.GetLength(0); i++)
        //    {
        //        cmb.Items.Add(state[i, 1], state[i, 0]);
        //    }
        //    //cmb.SelectedIndex = -1;
        //    // Code Commented By Sandip on 08032017 To avoid Select Option By default End
        //    //cmb.Items.Insert(0, new ListEditItem("Select", "0"));
        //    // Code Above Commented By Sandip on 08032017 To avoid Select Option By default End
        //}


        //string[,] GetCities(int state)
        //{


        //    SelectCity.SelectParameters[0].DefaultValue = Convert.ToString(state);
        //    DataView view = (DataView)SelectCity.Select(DataSourceSelectArguments.Empty);
        //    string[,] DATA = new string[view.Count, 2];
        //    for (int i = 0; i < view.Count; i++)
        //    {
        //        DATA[i, 0] = Convert.ToString(view[i][0]);
        //        DATA[i, 1] = Convert.ToString(view[i][1]);
        //    }
        //    return DATA;

        //}
        //protected void FillCityCombo(ASPxComboBox cmb, int state)
        //{

        //    string[,] cities = GetCities(state);
        //    cmb.Items.Clear();

        //    for (int i = 0; i < cities.GetLength(0); i++)
        //    {
        //        cmb.Items.Add(cities[i, 1], cities[i, 0]);
        //    }
        //    //cmb.SelectedIndex = -1;
        //    // Code Commented By Sandip on 08032017 To avoid Select Option By default End
        //    //cmb.Items.Insert(0, new ListEditItem("Select", "0"));
        //    // Code Above Commented By Sandip on 08032017 To avoid Select Option By default End
        //}
        //protected void FillPinCombo(ASPxComboBox cmb, int city)
        //{
        //    string[,] pin = GetPin(city);
        //    cmb.Items.Clear();

        //    for (int i = 0; i < pin.GetLength(0); i++)
        //    {
        //        cmb.Items.Add(pin[i, 1], pin[i, 0]);
        //    }
        //    //cmb.SelectedIndex = -1;
        //}
        //string[,] GetPin(int city)
        //{
        //    SelectPin.SelectParameters[0].DefaultValue = Convert.ToString(city);
        //    DataView view = (DataView)SelectPin.Select(DataSourceSelectArguments.Empty);
        //    string[,] DATA = new string[view.Count, 2];
        //    for (int i = 0; i < view.Count; i++)
        //    {
        //        DATA[i, 0] = Convert.ToString(view[i][0]);
        //        DATA[i, 1] = Convert.ToString(view[i][1]);
        //    }
        //    return DATA;
        //}
        //string[,] GetArea(int city)
        //{
        //    SelectArea.SelectParameters[0].DefaultValue = Convert.ToString(city);
        //    DataView view = (DataView)SelectArea.Select(DataSourceSelectArguments.Empty);
        //    string[,] DATA = new string[view.Count, 2];
        //    for (int i = 0; i < view.Count; i++)
        //    {
        //        DATA[i, 0] = Convert.ToString(view[i][0]);
        //        DATA[i, 1] = Convert.ToString(view[i][1]);
        //    }
        //    return DATA;
        //}

        //protected void FillAreaCombo(ASPxComboBox cmb, int city)
        //{
        //    string[,] area = GetArea(city);
        //    cmb.Items.Clear();

        //    for (int i = 0; i < area.GetLength(0); i++)
        //    {
        //        cmb.Items.Add(area[i, 1], area[i, 0]);
        //    }
        //    //cmb.SelectedIndex = -1;
        //    // Code Commented By Sandip on 08032017 To avoid Select Option By default Start
        //    //cmb.Items.Insert(0, new ListEditItem("Select", "0"));
        //    // Code Above Commented By Sandip on 08032017 To avoid Select Option By default End
        //}

        //#endregion
        */
        //Date: 31-05-2017    Author: Kallol Samanta  [END]









        //Date: 31-05-2017    Author: Kallol Samanta  [START]
        //Details: Billing/Shipping user control integration

        /*
        //#region Shipping DropDown Call Back Function End
        //protected void cmbState_OnCallback(object source, CallbackEventArgsBase e)
        //{
        //    if (e.Parameter != "")
        //    {
        //        FillStateCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
        //    }
        //    //else
        //    //{
        //    //    FillStateCombo(source as ASPxComboBox, 0);
        //    //}
        //}
        //protected void cmbCity_OnCallback(object source, CallbackEventArgsBase e)
        //{
        //    if (e.Parameter != "")
        //    {
        //        FillCityCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
        //    }
        //    //else
        //    //{
        //    //    FillCityCombo(source as ASPxComboBox, 0);
        //    //}
        //}
        //protected void cmbPin_OnCallback(object source, CallbackEventArgsBase e)
        //{
        //    if (e.Parameter != "")
        //    {
        //        FillPinCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
        //    }
        //    //else
        //    //{
        //    //    FillPinCombo(source as ASPxComboBox, 0);
        //    //}
        //}
        //protected void cmbArea_OnCallback(object source, CallbackEventArgsBase e)
        //{
        //    if (e.Parameter != "")
        //    {
        //        FillAreaCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
        //    }
        //    //else
        //    //{
        //    //    FillAreaCombo(source as ASPxComboBox, 0);
        //    //}
        //}

        //#endregion Billing DropDown Call Back Function End
        */

        //Date: 31-05-2017    Author: Kallol Samanta  [END]










        //Date: 31-05-2017    Author: Kallol Samanta  [START]
        //Details: Billing/Shipping user control integration
        /*
        //#region Shipping DropDown Call Back Function Start
        //protected void cmbState1_OnCallback(object source, CallbackEventArgsBase e)
        //{
        //    if (e.Parameter != "")
        //    {
        //        FillStateCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
        //    }
        //    //else
        //    //{
        //    //    FillStateCombo(source as ASPxComboBox, 0);
        //    //}
        //}
        //protected void cmbCity1_OnCallback(object source, CallbackEventArgsBase e)
        //{
        //    if (e.Parameter != "")
        //    {
        //        FillCityCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
        //    }
        //    //else
        //    //{
        //    //    FillCityCombo(source as ASPxComboBox, 0);
        //    //}
        //}
        //protected void cmbArea1_OnCallback(object source, CallbackEventArgsBase e)
        //{
        //    if (e.Parameter != "")
        //    {
        //        FillAreaCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
        //    }
        //    //else
        //    //{
        //    //    FillAreaCombo(source as ASPxComboBox, 0);
        //    //}
        //}
        //protected void cmbPin1_OnCallback(object source, CallbackEventArgsBase e)
        //{
        //    if (e.Parameter != "")
        //    {
        //        FillPinCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
        //    }
        //    //else
        //    //{
        //    //    FillPinCombo(source as ASPxComboBox, 0);
        //    //}
        //}

        //#endregion Shipping DropDown Call Back Function End
        */
        //Date: 31-05-2017    Author: Kallol Samanta  [END]








        public DataTable StoreQuotationAddressDetail()
        {

            DataTable AddressDetaildt = new DataTable();

            AddressDetaildt.Columns.Add("QuoteAdd_QuoteId", typeof(System.Int32));
            AddressDetaildt.Columns.Add("QuoteAdd_CompanyID", typeof(System.String));
            AddressDetaildt.Columns.Add("QuoteAdd_BranchId", typeof(System.Int32));
            AddressDetaildt.Columns.Add("QuoteAdd_FinYear", typeof(System.String));

            AddressDetaildt.Columns.Add("QuoteAdd_ContactPerson", typeof(System.String));
            AddressDetaildt.Columns.Add("ReturnAdd_addressType", typeof(System.String));
            AddressDetaildt.Columns.Add("ReturnAdd_address1", typeof(System.String));
            AddressDetaildt.Columns.Add("ReturnAdd_address2", typeof(System.String));
            AddressDetaildt.Columns.Add("ReturnAdd_address3", typeof(System.String));


            AddressDetaildt.Columns.Add("ReturnAdd_landMark", typeof(System.String));
            AddressDetaildt.Columns.Add("ReturnAdd_countryId", typeof(System.Int32));
            AddressDetaildt.Columns.Add("ReturnAdd_stateId", typeof(System.Int32));
            AddressDetaildt.Columns.Add("ReturnAdd_cityId", typeof(System.Int32));
            AddressDetaildt.Columns.Add("ReturnAdd_areaId", typeof(System.Int32));


            AddressDetaildt.Columns.Add("ReturnAdd_pin", typeof(System.String));
            AddressDetaildt.Columns.Add("QuoteAdd_CreatedDate", typeof(System.DateTime));
            AddressDetaildt.Columns.Add("QuoteAdd_CreatedUser", typeof(System.Int32));
            AddressDetaildt.Columns.Add("QuoteAdd_LastModifyDate", typeof(System.DateTime));
            AddressDetaildt.Columns.Add("QuoteAdd_LastModifyUser", typeof(System.Int32));
            return AddressDetaildt;


        }


        [WebMethod]
        public static bool CheckUniqueCode(string ReturnNo)
        {
            bool flag = false;
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
            try
            {
                MShortNameCheckingBL objShortNameChecking = new MShortNameCheckingBL();
                flag = objShortNameChecking.CheckUnique(ReturnNo, "0", "SalesReturnManual");
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
        public static Int32 GetQuantityfromSL(string SLNo, string val)
        {

            DataTable dt = new DataTable();
            int SLVal = 0;
            if (HttpContext.Current.Session["MultiUOMDataRETM"] != null)
            {
                DataRow[] MultiUoMresult;
                dt = (DataTable)HttpContext.Current.Session["MultiUOMDataRETM"];
                if (val == "1")
                {
                    // Mantis Issue 24831
                    //MultiUoMresult = dt.Select("DetailsId ='" + SLNo + "'");
                    MultiUoMresult = dt.Select("DetailsId ='" + SLNo + "' and UpdateRow ='True'");
                    // End of Mantis Issue 24831
                }
                else
                {
                    // Mantis Issue 24831
                    //MultiUoMresult = dt.Select("SrlNo ='" + SLNo + "'");
                    MultiUoMresult = dt.Select("SrlNo ='" + SLNo + "' and UpdateRow ='True'");
                    // End of Mantis Issue 24831
                }
                SLVal = MultiUoMresult.Length;


            }

            return SLVal;
        }


        [WebMethod]
        public static object GetPackingQuantity(Int32 UomId, Int32 AltUomId, Int64 ProductID)
        {
            List<MultiUOMPacking> RateLists = new List<MultiUOMPacking>();

            decimal packing_quantity = 0;
            decimal sProduct_quantity = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesReturn_Details");
            proc.AddVarcharPara("@Action", 500, "PackingQuantityDetails");
            proc.AddIntegerPara("@UomId", UomId);
            proc.AddIntegerPara("@AltUomId", AltUomId);
            proc.AddBigIntegerPara("@PackingProductId", ProductID);
            DataTable dt = proc.GetTable();
            RateLists = DbHelpers.ToModelList<MultiUOMPacking>(dt);
            if (dt != null && dt.Rows.Count > 0)
            {
                packing_quantity = Convert.ToDecimal(dt.Rows[0]["packing_quantity"]);
                sProduct_quantity = Convert.ToDecimal(dt.Rows[0]["sProduct_quantity"]);
            }
            //return packing_quantity + '~' + sProduct_quantity;
            return RateLists;
        }

        [WebMethod]
        public static object AutoPopulateAltQuantity(Int64 ProductID)
        {
            List<MultiUOMPacking> RateLists = new List<MultiUOMPacking>();

            decimal packing_quantity = 0;
            decimal sProduct_quantity = 0;
            Int32 AltUOMId = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesReturn_Details");
            proc.AddVarcharPara("@Action", 500, "AutoPopulateAltQuantityDetails");
            proc.AddBigIntegerPara("@PackingProductId", ProductID);
            DataTable dt = proc.GetTable();
            RateLists = DbHelpers.ToModelList<MultiUOMPacking>(dt);
            if (dt != null && dt.Rows.Count > 0)
            {
                packing_quantity = Convert.ToDecimal(dt.Rows[0]["packing_quantity"]);
                sProduct_quantity = Convert.ToDecimal(dt.Rows[0]["sProduct_quantity"]);
                AltUOMId = Convert.ToInt32(dt.Rows[0]["AltUOMId"]);
            }
            //return packing_quantity + '~' + sProduct_quantity;
            return RateLists;
        }

        //Date: 31-05-2017    Author: Kallol Samanta  [START]
        //Details: Billing/Shipping user control integration
        protected void ComponentPanel_Callback(object sender, CallbackEventArgsBase e)
        {

            /*

            Populatecountry();
            #region Addresss Lookup Section Start
            DataSet dst = new DataSet();
            CRMSalesDtlBL objCRMSalesDtlBL = new CRMSalesDtlBL();
            dst = objCRMSalesDtlBL.PopulateBillingandShippingDetailByCustomerID(hdnCustomerId.Value);
            billingAddress.DataSource = dst.Tables[0];
            billingAddress.DataBind();
            if (dst.Tables[0].Rows.Count > 0)
            {
                Session["SRM_BillingAddressLookup"] = dst.Tables[0];
            }
            shippingAddress.DataSource = dst.Tables[1];
            shippingAddress.DataBind();
            if (dst.Tables[1].Rows.Count > 0)
            {
                Session["SRM_ShippingAddressLookup"] = dst.Tables[1];
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

            #region BillingLookup Edit Section Start
            if (WhichCall == "BlookupEdit")
            {
                int BillingAddressID = Convert.ToInt32(e.Parameter.Split('~')[1]);
                DataTable dt = objCRMSalesDtlBL.PopulateAddressDetailByAddressId(BillingAddressID);
                if (dt.Rows.Count > 0 && dt != null)
                {
                    //for (int m = 0; m < dtaddbill.Rows.Count; m++)
                    //{
                    add_addressType = Convert.ToString(dt.Rows[0]["add_addressType"]);
                    add_address1 = Convert.ToString(dt.Rows[0]["add_address1"]);
                    add_address2 = Convert.ToString(dt.Rows[0]["add_address2"]);
                    add_address3 = Convert.ToString(dt.Rows[0]["add_address3"]);
                    add_landMark = Convert.ToString(dt.Rows[0]["add_landMark"]);
                    add_country = Convert.ToString(dt.Rows[0]["add_country"]);
                    add_state = Convert.ToString(dt.Rows[0]["add_state"]);
                    add_city = Convert.ToString(dt.Rows[0]["add_city"]);
                    add_pin = Convert.ToString(dt.Rows[0]["add_pin"]);
                    add_area = Convert.ToString(dt.Rows[0]["add_area"]);

                    PopulateBilling(add_address1, add_address2, add_address3, add_landMark, add_country, add_state, add_city, add_pin, add_area);

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

            }



            #endregion BillingLookup Edit Section Start

            #region ShippingLookup Edit Section Start
            if (WhichCall == "SlookupEdit")
            {
                int AddressID = Convert.ToInt32(e.Parameter.Split('~')[1]);
                DataTable dt = objCRMSalesDtlBL.PopulateAddressDetailByAddressId(AddressID);
                if (dt.Rows.Count > 0 && dt != null)
                {
                    add_saddressType = Convert.ToString(dt.Rows[0]["add_addressType"]);
                    add_saddress1 = Convert.ToString(dt.Rows[0]["add_address1"]);
                    add_saddress2 = Convert.ToString(dt.Rows[0]["add_address2"]);
                    add_saddress3 = Convert.ToString(dt.Rows[0]["add_address3"]);
                    add_slandMark = Convert.ToString(dt.Rows[0]["add_landMark"]);
                    add_scountry = Convert.ToString(dt.Rows[0]["add_country"]);
                    add_sstate = Convert.ToString(dt.Rows[0]["add_state"]);
                    add_scity = Convert.ToString(dt.Rows[0]["add_city"]);
                    add_spin = Convert.ToString(dt.Rows[0]["add_pin"]);
                    add_sarea = Convert.ToString(dt.Rows[0]["add_area"]);

                    PopulateShipping(add_saddress1, add_saddress2, add_saddress3, add_slandMark, add_scountry, add_sstate, add_scity, add_spin, add_sarea);

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

            }

            #endregion ShippingLookup Edit Section End
            #region Edit Section of Address Start

            if (WhichCall == "Edit")
            {
                if (Session["SRM_QuotationAddressDtl"] == null)
                {
                    string customerid = Convert.ToString(HttpContext.Current.Session["LastCompany"]);

                    string branchid = Convert.ToString(ddl_Branch.SelectedValue);
                    #region Billing Detail fillup
                    DataTable dtaddbill = oDBEngine.GetDataTable("select '' as 'add_addressType',branch_address1 as add_address1,branch_address2 as add_address2,branch_address3 as add_address3,'' as add_landMark,branch_country as add_country ,branch_state as add_state,branch_city as add_city,branch_pin as add_pin,branch_area as add_area from tbl_master_branch where branch_id='" + branchid + "'");

                    if (Convert.ToString(dtaddbill.Rows[0]["add_address1"]) == "" || Convert.ToString(dtaddbill.Rows[0]["add_address2"]) == "")
                    {
                        dtaddbill = oDBEngine.GetDataTable("select add_addressType,add_address1,add_address2,add_address3,add_landMark,add_country,add_state,add_city,add_pin,add_area from tbl_master_address where add_cntId='" + customerid + "' and add_addressType='Office'");



                    }
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

                        //}

                        PopulateBilling(add_address1, add_address2, add_address3, add_landMark, add_country, add_state, add_city, add_pin, add_area);

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
                    DataTable dtaship = oDBEngine.GetDataTable("select '' as 'add_addressType',branch_address1 as add_address1,branch_address2 as add_address2,branch_address3 as add_address3,'' as add_landMark,branch_country as add_country ,branch_state as add_state,branch_city as add_city,branch_pin as add_pin,branch_area as add_area from tbl_master_branch where branch_id='" + branchid + "'");
                    if (Convert.ToString(dtaship.Rows[0]["add_address1"]) == "" || Convert.ToString(dtaship.Rows[0]["add_address2"]) == "")
                    {
                        dtaship = oDBEngine.GetDataTable("select add_addressType,add_address1,add_address2,add_address3,add_landMark,add_country,add_state,add_city,add_pin,add_area from tbl_master_address where add_cntId='" + customerid + "' and add_addressType='Office'");



                    }

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

                        PopulateShipping(add_saddress1, add_saddress2, add_saddress3, add_slandMark, add_scountry, add_sstate, add_scity, add_spin, add_sarea);

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

                else if (Session["SRM_QuotationAddressDtl"] != null)
                {
                    DataTable dt = (DataTable)Session["SRM_QuotationAddressDtl"];
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows.Count == 2) // when 2 row  data found in edit mode
                        {
                            #region billing Address Dtl using session
                            add_addressType = Convert.ToString(dt.Rows[0]["ReturnAdd_addressType"]);
                            add_address1 = Convert.ToString(dt.Rows[0]["ReturnAdd_address1"]);
                            add_address2 = Convert.ToString(dt.Rows[0]["ReturnAdd_address2"]);
                            add_address3 = Convert.ToString(dt.Rows[0]["ReturnAdd_address3"]);
                            add_landMark = Convert.ToString(dt.Rows[0]["ReturnAdd_landMark"]);
                            add_country = Convert.ToString(dt.Rows[0]["ReturnAdd_countryId"]);
                            add_state = Convert.ToString(dt.Rows[0]["ReturnAdd_stateId"]);
                            add_city = Convert.ToString(dt.Rows[0]["ReturnAdd_cityId"]);
                            add_pin = Convert.ToString(dt.Rows[0]["ReturnAdd_pin"]);
                            add_area = Convert.ToString(dt.Rows[0]["ReturnAdd_areaId"]);
                            PopulateBilling(add_address1, add_address2, add_address3, add_landMark, add_country, add_state, add_city, add_pin, add_area);
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
                            add_saddressType = Convert.ToString(dt.Rows[1]["ReturnAdd_addressType"]);
                            add_saddress1 = Convert.ToString(dt.Rows[1]["ReturnAdd_address1"]);
                            add_saddress2 = Convert.ToString(dt.Rows[1]["ReturnAdd_address2"]);
                            add_saddress3 = Convert.ToString(dt.Rows[1]["ReturnAdd_address3"]);
                            add_slandMark = Convert.ToString(dt.Rows[1]["ReturnAdd_landMark"]);
                            add_scountry = Convert.ToString(dt.Rows[1]["ReturnAdd_countryId"]);
                            add_sstate = Convert.ToString(dt.Rows[1]["ReturnAdd_stateId"]);
                            add_scity = Convert.ToString(dt.Rows[1]["ReturnAdd_cityId"]);
                            add_spin = Convert.ToString(dt.Rows[1]["ReturnAdd_pin"]);
                            add_sarea = Convert.ToString(dt.Rows[1]["ReturnAdd_areaId"]);
                            PopulateShipping(add_saddress1, add_saddress2, add_saddress3, add_slandMark, add_scountry, add_sstate, add_scity, add_spin, add_sarea);
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
                            if (Convert.ToString(dt.Rows[0]["ReturnAdd_addressType"]) == "Billing")
                            {
                                #region billing Address Dtl using session
                                add_addressType = Convert.ToString(dt.Rows[0]["ReturnAdd_addressType"]);
                                add_address1 = Convert.ToString(dt.Rows[0]["ReturnAdd_address1"]);
                                add_address2 = Convert.ToString(dt.Rows[0]["ReturnAdd_address2"]);
                                add_address3 = Convert.ToString(dt.Rows[0]["ReturnAdd_address3"]);
                                add_landMark = Convert.ToString(dt.Rows[0]["ReturnAdd_landMark"]);
                                add_country = Convert.ToString(dt.Rows[0]["ReturnAdd_countryId"]);
                                add_state = Convert.ToString(dt.Rows[0]["ReturnAdd_stateId"]);
                                add_city = Convert.ToString(dt.Rows[0]["ReturnAdd_cityId"]);
                                add_pin = Convert.ToString(dt.Rows[0]["ReturnAdd_pin"]);
                                add_area = Convert.ToString(dt.Rows[0]["ReturnAdd_areaId"]);
                                PopulateBilling(add_address1, add_address2, add_address3, add_landMark, add_country, add_state, add_city, add_pin, add_area);
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

                                add_saddressType = Convert.ToString(dt.Rows[0]["ReturnAdd_addressType"]);
                                add_saddress1 = Convert.ToString(dt.Rows[0]["ReturnAdd_address1"]);
                                add_saddress2 = Convert.ToString(dt.Rows[0]["ReturnAdd_address2"]);
                                add_saddress3 = Convert.ToString(dt.Rows[0]["ReturnAdd_address3"]);
                                add_slandMark = Convert.ToString(dt.Rows[0]["ReturnAdd_landMark"]);
                                add_scountry = Convert.ToString(dt.Rows[0]["ReturnAdd_countryId"]);
                                add_sstate = Convert.ToString(dt.Rows[0]["ReturnAdd_stateId"]);
                                add_scity = Convert.ToString(dt.Rows[0]["ReturnAdd_cityId"]);
                                add_spin = Convert.ToString(dt.Rows[0]["ReturnAdd_pin"]);
                                add_sarea = Convert.ToString(dt.Rows[0]["ReturnAdd_areaId"]);
                                PopulateShipping(add_saddress1, add_saddress2, add_saddress3, add_slandMark, add_scountry, add_sstate, add_scity, add_spin, add_sarea);
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
                    int country = Convert.ToInt32(Convert.ToString(CmbCountry.Value).TrimStart(','));
                    int State = Convert.ToInt32(Convert.ToString(CmbState.Value).TrimStart(','));
                    int city = Convert.ToInt32(Convert.ToString(CmbCity.Value).TrimStart(','));
                    int area = Convert.ToInt32(Convert.ToString(CmbArea.Value).TrimStart(',') == "" ? "0" : Convert.ToString(CmbArea.Value).TrimStart(','));
                    string pin = Convert.ToString(Convert.ToString(CmbPin.Value).TrimStart(',') == "" ? "0" : Convert.ToString(CmbPin.Value).TrimStart(','));
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
                    int scountry = Convert.ToInt32(Convert.ToString(CmbCountry1.Value).TrimStart(','));
                    int sState = Convert.ToInt32(Convert.ToString(CmbState1.Value).TrimStart(','));
                    int scity = Convert.ToInt32(Convert.ToString(CmbCity1.Value).TrimStart(','));
                    int sarea = Convert.ToInt32(Convert.ToString(CmbArea1.Value).TrimStart(',') == "" ? "0" : Convert.ToString(CmbArea1.Value).TrimStart(','));
                    string spin = Convert.ToString(Convert.ToString(CmbPin1.Value).TrimStart(',') == "" ? "0" : Convert.ToString(CmbPin1.Value).TrimStart(','));
                    dt.Rows.Add(0, companyId, branchid, fin_year, "", sAddressType, saddress1, saddress2, saddress3, slandmark, scountry, sState, scity, sarea, spin, System.DateTime.Now, userid, System.DateTime.Now, userid);


                    Session["SRM_QuotationAddressDtl"] = dt;
                    #endregion Shipping Address Detail Start end
                }
                else if (AddressStatus == "2") // Copy Billing to Shipping Address
                {
                    //string AddressType = Convert.ToString(CmbAddressType.SelectedItem.Value);
                    string AddressType = "Billing";
                    string address1 = txtAddress1.Text;
                    string address2 = txtAddress2.Text;
                    string address3 = txtAddress3.Text;
                    string landmark = txtlandmark.Text;
                    int country = Convert.ToInt32(Convert.ToString(CmbCountry.Value).TrimStart(','));
                    int State = Convert.ToInt32(Convert.ToString(CmbState.Value).TrimStart(','));
                    int city = Convert.ToInt32(Convert.ToString(CmbCity.Value).TrimStart(','));
                    int area = Convert.ToInt32(Convert.ToString(CmbArea.Value).TrimStart(',') == "" ? "0" : Convert.ToString(CmbArea.Value).TrimStart(','));
                    string pin = Convert.ToString(Convert.ToString(CmbPin.Value).TrimStart(',') == "" ? "0" : Convert.ToString(CmbPin.Value).TrimStart(','));
                    DataTable dt = StoreQuotationAddressDetail();
                    dt.Rows.Add(0, companyId, branchid, fin_year, "", AddressType, address1, address2, address3, landmark, country, State, city, area, pin, System.DateTime.Now, userid, System.DateTime.Now, userid);
                    dt.Rows.Add(0, companyId, branchid, fin_year, "", "Shipping", address1, address2, address3, landmark, country, State, city, area, pin, System.DateTime.Now, userid, System.DateTime.Now, userid);
                    Session["SRM_QuotationAddressDtl"] = dt;
                }
                else if (AddressStatus == "3") // Copy  Shipping to Billing  Address
                {
                    string scontactperson = "";
                    //string sAddressType = Convert.ToString(CmbAddressType1.SelectedItem.Value);
                    string sAddressType = "Shipping";
                    string saddress1 = txtsAddress1.Text;
                    string saddress2 = txtsAddress2.Text;
                    string saddress3 = txtsAddress3.Text;
                    string slandmark = txtslandmark.Text;
                    int scountry = Convert.ToInt32(Convert.ToString(CmbCountry1.Value).TrimStart(','));
                    int sState = Convert.ToInt32(Convert.ToString(CmbState1.Value).TrimStart(','));
                    int scity = Convert.ToInt32(Convert.ToString(CmbCity1.Value).TrimStart(','));
                    int sarea = Convert.ToInt32(Convert.ToString(CmbArea1.Value).TrimStart(',') == "" ? "0" : Convert.ToString(CmbArea1.Value).TrimStart(','));
                    string spin = Convert.ToString(Convert.ToString(CmbPin1.Value).TrimStart(',') == "" ? "0" : Convert.ToString(CmbPin1.Value).TrimStart(','));
                    DataTable dt = StoreQuotationAddressDetail();
                    dt.Rows.Add(0, companyId, branchid, fin_year, "", "Billing", saddress1, saddress2, saddress3, slandmark, scountry, sState, scity, sarea, spin, System.DateTime.Now, userid, System.DateTime.Now, userid);
                    dt.Rows.Add(0, companyId, branchid, fin_year, "", sAddressType, saddress1, saddress2, saddress3, slandmark, scountry, sState, scity, sarea, spin, System.DateTime.Now, userid, System.DateTime.Now, userid);
                    Session["SRM_QuotationAddressDtl"] = dt;
                }

            }

            #endregion Save Section of Address Start

            */


        }

        //Date: 31-05-2017    Author: Kallol Samanta  [END]


        #endregion Subhra Section end

        //Date: 31-05-2017    Author: Kallol Samanta  [START]
        //Details: Billing/Shipping user control integration

        /*
        //#region populate Billing Address
        //public void PopulateBilling(string add_address1, string add_address2, string add_address3, string add_landMark, string add_country, string add_state, string add_city, string add_pin, string add_area)
        //{
        //    DataTable dtcountry = new DataTable();

        //    dtcountry = oDBEngine.GetDataTable("SELECT cou_id, cou_country as Country FROM tbl_master_country order by cou_country");

        //    CmbCountry.DataSource = dtcountry;
        //    CmbCountry.TextField = "Country";
        //    CmbCountry.ValueField = "cou_id";
        //    CmbCountry.DataBind();
        //    if (!string.IsNullOrEmpty(add_country))
        //    {
        //        CmbCountry.Value = add_country;
        //    }
        //    DataTable dtstate = new DataTable();

        //    dtstate = oDBEngine.GetDataTable("SELECT s.id as ID,s.state+' (State Code:' +s.StateCode+')' as State from tbl_master_state s where (s.countryId = " + add_country + ") ORDER BY s.state");



        //    CmbState.DataSource = dtstate;
        //    CmbState.TextField = "State";
        //    CmbState.ValueField = "ID";
        //    CmbState.DataBind();
        //    if (!string.IsNullOrEmpty(add_state))
        //    {
        //        CmbState.Value = add_state;
        //    }
        //    DataTable dtcity = new DataTable();

        //    dtcity = oDBEngine.GetDataTable("SELECT c.city_id AS CityId, c.city_name AS City FROM tbl_master_city c where c.state_id=" + add_state + " order by c.city_name");


        //    CmbCity.DataSource = dtcity;
        //    CmbCity.TextField = "City";
        //    CmbCity.ValueField = "CityId";
        //    CmbCity.DataBind();

        //    if (!string.IsNullOrEmpty(add_city))
        //    {
        //        CmbCity.Value = add_city;

        //    }
        //    DataTable dtpin = new DataTable();
        //    dtpin = oDBEngine.GetDataTable("select pin_id,pin_code from tbl_master_pinzip where city_id=" + add_city + " order by pin_code");


        //    CmbPin.DataSource = dtpin;
        //    CmbPin.TextField = "pin_code";
        //    CmbPin.ValueField = "pin_id";
        //    CmbPin.DataBind();

        //    if (!string.IsNullOrEmpty(add_pin))
        //    {
        //        CmbPin.Value = add_pin;
        //    }

        //    DataTable dtarea = new DataTable();
        //    dtarea = oDBEngine.GetDataTable("SELECT area_id, area_name from tbl_master_area where (city_id = " + add_city + ") ORDER BY area_name");

        //    CmbArea.DataSource = dtarea;
        //    CmbArea.TextField = "area_name";
        //    CmbArea.ValueField = "area_id";
        //    CmbArea.DataBind();

        //    CmbArea.Value = add_area;



        //    txtAddress1.Text = add_address1;
        //    txtAddress2.Text = add_address2;
        //    txtAddress3.Text = add_address3;
        //    txtlandmark.Text = add_landMark;

        //}
        //#endregion

        //#region populate Shipping Address
        //public void PopulateShipping(string add_saddress1, string add_saddress2, string add_saddress3, string add_slandMark, string add_scountry, string add_sstate, string add_scity, string add_spin, string add_sarea)
        //{
        //    DataTable dtcountry1 = new DataTable();

        //    dtcountry1 = oDBEngine.GetDataTable("SELECT cou_id, cou_country as Country FROM tbl_master_country order by cou_country");

        //    CmbCountry1.DataSource = dtcountry1;
        //    CmbCountry1.TextField = "Country";
        //    CmbCountry1.ValueField = "cou_id";
        //    CmbCountry1.DataBind();
        //    if (!string.IsNullOrEmpty(add_scountry))
        //    {
        //        CmbCountry1.Value = add_scountry;
        //    }
        //    DataTable dtstate1 = new DataTable();

        //    dtstate1 = oDBEngine.GetDataTable("SELECT s.id as ID,s.state+' (State Code:' +s.StateCode+')' as State from tbl_master_state s where (s.countryId = " + add_scountry + ") ORDER BY s.state");


        //    CmbState1.DataSource = dtstate1;
        //    CmbState1.TextField = "State";
        //    CmbState1.ValueField = "ID";
        //    CmbState1.DataBind();
        //    if (!string.IsNullOrEmpty(add_sstate))
        //    {
        //        CmbState1.Value = add_sstate;
        //    }

        //    DataTable dtcity1 = new DataTable();
        //    dtcity1 = oDBEngine.GetDataTable("SELECT c.city_id AS CityId, c.city_name AS City FROM tbl_master_city c where c.state_id=" + add_sstate + " order by c.city_name");

        //    CmbCity1.DataSource = dtcity1;
        //    CmbCity1.TextField = "City";
        //    CmbCity1.ValueField = "CityId";
        //    CmbCity1.DataBind();
        //    if (!string.IsNullOrEmpty(add_scity))
        //    {
        //        CmbCity1.Value = add_scity;
        //    }


        //    DataTable dtpin1 = new DataTable();
        //    dtpin1 = oDBEngine.GetDataTable("select pin_id,pin_code from tbl_master_pinzip where city_id=" + add_scity + " order by pin_code");


        //    CmbPin1.DataSource = dtpin1;
        //    CmbPin1.TextField = "pin_code";
        //    CmbPin1.ValueField = "pin_id";
        //    CmbPin1.DataBind();
        //    if (!string.IsNullOrEmpty(add_spin))
        //    {
        //        CmbPin1.Value = add_spin;
        //    }

        //    DataTable dtarea1 = new DataTable();
        //    dtarea1 = oDBEngine.GetDataTable("SELECT area_id, area_name from tbl_master_area where (city_id = " + add_scity + ") ORDER BY area_name");

        //    CmbArea1.DataSource = dtarea1;
        //    CmbArea1.TextField = "area_name";
        //    CmbArea1.ValueField = "area_id";
        //    CmbArea1.DataBind();

        //    if (!string.IsNullOrEmpty(add_sarea))
        //    {
        //        CmbArea1.Value = add_sarea;
        //    }


        //    txtsAddress1.Text = add_saddress1;
        //    txtsAddress2.Text = add_saddress2;
        //    txtsAddress3.Text = add_saddress3;
        //    txtslandmark.Text = add_slandMark;

        //}
        //#endregion

        //#region Populate Country
        //public void Populatecountry()
        //{
        //    DataTable dtcountry = new DataTable();
        //    dtcountry = oDBEngine.GetDataTable("SELECT cou_id, cou_country as Country FROM tbl_master_country order by cou_country");
        //    CmbCountry.DataSource = dtcountry;
        //    CmbCountry.TextField = "Country";
        //    CmbCountry.ValueField = "cou_id";
        //    CmbCountry.DataBind();

        //    CmbCountry1.DataSource = dtcountry;
        //    CmbCountry1.TextField = "Country";
        //    CmbCountry1.ValueField = "cou_id";
        //    CmbCountry1.DataBind();
        //}

        //#endregion
        */

        //Date: 31-05-2017    Author: Kallol Samanta  [END]
        #region Sam Section Start

        #region PrePopulated Data If Page is not Post Back Section Start
        public void SetFinYearCurrentDate()
        {
            dt_PLQuote.EditFormatString = objConverter.GetDateFormat("Date");
            string fDate = null;

            //DateTime dt = DateTime.ParseExact("3/31/2016", "MM/dd/yyy", CultureInfo.InvariantCulture);
            string[] FinYEnd = Convert.ToString(Session["FinYearEnd"]).Split(' ');
            string FinYearEnd = FinYEnd[0];

            DateTime date3 = DateTime.ParseExact(FinYearEnd, "M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);

            ForJournalDate = Convert.ToString(date3);

            //ForJournalDate =Session["FinYearEnd"].ToString();
            int month = oDBEngine.GetDate().Month;
            int date = oDBEngine.GetDate().Day;
            int Year = oDBEngine.GetDate().Year;

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
        public void GetAllDropDownDetailForSalesQuotation(string userbranch)
        {
            #region Schema Drop Down Start
            DataSet dst = new DataSet();
            string strCompanyID = Convert.ToString(Session["LastCompany"]);
            string strBranchID = Convert.ToString(Session["userbranchID"]);
            string FinYear = Convert.ToString(Session["LastFinYear"]);
            string userbranchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);

            dst = objSalesReturnBL.GetAllDropDownDetailForSalesInvoice(userbranchHierarchy, strCompanyID, strBranchID, FinYear);

            //if (dst.Tables[0] != null && dst.Tables[0].Rows.Count > 0)
            //{
            //    ddl_numberingScheme.DataTextField = "SchemaName";
            //    ddl_numberingScheme.DataValueField = "Id";
            //    ddl_numberingScheme.DataSource = dst.Tables[0];
            //    ddl_numberingScheme.DataBind();
            //}

            SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
            DataTable Schemadt = objSlaesActivitiesBL.GetNumberingSchema(strCompanyID, userbranchHierarchy, FinYear, "39", "Y");
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

            #region Saleman DropDown Start
            if (dst.Tables[2] != null && dst.Tables[2].Rows.Count > 0)
            {
                ddl_SalesAgent.DataTextField = "Name";
                ddl_SalesAgent.DataValueField = "cnt_id";
                ddl_SalesAgent.DataSource = dst.Tables[2];
                ddl_SalesAgent.DataBind();
            }
            ddl_SalesAgent.Items.Insert(0, new ListItem("Select", "0"));
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
                    string currency = Convert.ToString(HttpContext.Current.Session["LocalCurrency"]);
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
            }
            #endregion TaxGroupType DropDown Start

        }

        #endregion PrePopulated Data If Page is not Post Back Section End

        #region PrePopulated Data in Page Load Due to use Searching Functionality Section Start
        //public void PopulateCustomerDetail()
        //{
        //    if (Session["SRM_CustomerDetail"] == null)
        //    {
        //        DataTable dtCustomer = new DataTable();
        //        dtCustomer = objSalesReturnBL.PopulateCustomerDetail();

        //        if (dtCustomer != null && dtCustomer.Rows.Count > 0)
        //        {
        //            lookup_Customer.DataSource = dtCustomer;
        //            lookup_Customer.DataBind();
        //            Session["SRM_CustomerDetail"] = dtCustomer;
        //        }
        //    }
        //    else
        //    {
        //        lookup_Customer.DataSource = (DataTable)Session["SRM_CustomerDetail"];
        //        lookup_Customer.DataBind();
        //    }

        //}
        #endregion PrePopulated Data in Page Load Due to use Searching Functionality Section End

        #region Header Portion Detail of the Page By Sam

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
            if (HttpContext.Current.Session["LocalCurrency"] != null)
            {
                string currency = Convert.ToString(HttpContext.Current.Session["LocalCurrency"]);
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
            Session["SRM_QuotationAddressDtl"] = null;
            Session["SRM_BillingAddressLookup"] = null;
            Session["SRM_ShippingAddressLookup"] = null;
            string WhichCall = e.Parameter.Split('~')[0];
            if (WhichCall == "BindContactPerson")
            {
                string InternalId = Convert.ToString(Convert.ToString(e.Parameter.Split('~')[1]));
                string BranchId = Convert.ToString(Convert.ToString(e.Parameter.Split('~')[2]));
                PopulateContactPersonOfCustomer(InternalId);

                DataTable dtDeuDate = objSalesReturnBL.GetCustomerDetails_InvoiceRelated(InternalId);
                foreach (DataRow dr in dtDeuDate.Rows)
                {
                    string strDueDate = Convert.ToString(dr["DueDate"]);
                    cmbContactPerson.JSProperties["cpDueDate"] = strDueDate;
                    //dt_SaleInvoiceDue.Date = Convert.ToDateTime(strDeuDate);
                }

                DataTable OutStandingTable = objSalesReturnBL.GetCustomerOutStanding(InternalId);
                if (OutStandingTable.Rows.Count > 0)
                {

                    var convertDecimal = Convert.ToDecimal(Convert.ToString(OutStandingTable.Rows[0]["NetOutstanding"]));
                    if (convertDecimal > 0)
                    {
                        cmbContactPerson.JSProperties["cpOutstanding"] = Convert.ToString(convertDecimal) + "(Db)";
                    }
                    else
                    {

                        cmbContactPerson.JSProperties["cpOutstanding"] = Convert.ToString(convertDecimal * -1).ToString() + "(Cr)";
                    }


                }
                else
                {
                    cmbContactPerson.JSProperties["cpOutstanding"] = "0.0";
                }
                DataTable GSTNTable = objSalesReturnBL.GetCustomerNewGSTIN(InternalId, BranchId);


                if (GSTNTable == null || GSTNTable.Rows.Count == 0)
                { cmbContactPerson.JSProperties["cpGSTN"] = "No"; }
                else
                {

                    string strGSTN = Convert.ToString(GSTNTable.Rows[0]["CNT_GSTIN"]).Trim();
                    if (strGSTN != "")
                    {
                        cmbContactPerson.JSProperties["cpGSTN"] = "Yes";
                    }
                    else
                    {
                        cmbContactPerson.JSProperties["cpGSTN"] = "No";
                    }
                }
            }
            else if (WhichCall == "GetCustomerStateCode")
            {

                string CustomerInternalId = Convert.ToString(Convert.ToString(e.Parameter.Split('~')[1]));
                DataTable CustomerStateCodeDT = new DataTable();

                ProcedureExecute procCustomerStateCode = new ProcedureExecute("prc_CRMSalesReturn_Details");
                procCustomerStateCode.AddVarcharPara("@Action", 500, "GetCustomerStateCode");
                procCustomerStateCode.AddVarcharPara("@CustomerID", 100, Convert.ToString(CustomerInternalId));

                CustomerStateCodeDT = procCustomerStateCode.GetTable();

                string CustomerStateCode = "";

                if (CustomerStateCodeDT != null && CustomerStateCodeDT.Rows.Count > 0)
                {
                    CustomerStateCode = Convert.ToString(CustomerStateCodeDT.Rows[0]["StateCode"]);

                    cmbContactPerson.JSProperties["cpCustomerStateCode"] = CustomerStateCode;

                }

            }


        }
        protected void PopulateContactPersonOfCustomer(string InternalId)
        {
            string ContactPerson = "";
            DataTable dtContactPerson = new DataTable();
            SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
            dtContactPerson = objSlaesActivitiesBL.PopulateContactPersonOfCustomer(InternalId);
            if (dtContactPerson != null && dtContactPerson.Rows.Count > 0)
            {
                cmbContactPerson.TextField = "contactperson";
                cmbContactPerson.ValueField = "add_id";
                cmbContactPerson.DataSource = dtContactPerson;
                cmbContactPerson.DataBind();
                foreach (DataRow dr in dtContactPerson.Rows)
                {
                    if (Convert.ToString(dr["Isdefault"]) == "True")
                    {
                        ContactPerson = Convert.ToString(dr["add_id"]);
                        break;
                    }
                }
                cmbContactPerson.SelectedItem = cmbContactPerson.Items.FindByValue(ContactPerson);
            }
        }
        protected void ddl_VatGstCst_Callback(object sender, CallbackEventArgsBase e)
        {
            string type = e.Parameter.Split('~')[0];
            if (type == "Tax-code")
            {
                string Tax_Code = e.Parameter.Split('~')[1];

                if (Tax_Code != "0")
                {
                    PopulateGSTCSTVAT("2");
                    setValueForHeaderGST(ddl_VatGstCst, Tax_Code);
                }
                else
                {
                    PopulateGSTCSTVAT("2");
                    ddl_VatGstCst.SelectedIndex = 0;
                }
            }
            else { PopulateGSTCSTVAT(type); }
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








                // Date: 31-05-2017    Author: Kallol Samanta  [START]
                // Details: Billing/Shipping user control integration

                //New Code
                string sstateCode = "";
                if (ddlPosGstReturnManual.Value.ToString() == "S")
                {
                    //sstateCode = Convert.ToString(Purchase_BillingShipping.GetShippingStateId());
                    sstateCode = hdnPOSShippingStateId.Value;
                }
                else
                {
                    //sstateCode = Convert.ToString(Purchase_BillingShipping.GetBillingStateId());
                    sstateCode = hdnPOSBillingStateId.Value;
                }
                ShippingState = sstateCode;
                if (ShippingState.Trim() != "")
                {
                    ShippingState = ShippingState;
                }

                //Old Code
                //if (chkBilling.Checked)
                //{
                //    if (CmbState.Value != null)
                //    {
                //        ShippingState = CmbState.Text;
                //        ShippingState = ShippingState.Substring(ShippingState.IndexOf("(State Code:")).Replace("(State Code:", "").Replace(")", "");
                //    }
                //}
                //else
                //{
                //    if (CmbState1.Value != null)
                //    {
                //        ShippingState = CmbState1.Text;
                //        ShippingState = ShippingState.Substring(ShippingState.IndexOf("(State Code:")).Replace("(State Code:", "").Replace(")", "");
                //    }
                //}

                // Date: 31-05-2017    Author: Kallol Samanta  [END]










                if (ShippingState.Trim() != "" && compGstin[0].Trim() != "")
                {

                    if (compGstin.Length > 0)
                    {
                        if (compGstin[0].Substring(0, 2) == ShippingState)
                        {
                            //Check if the state is in union territories then only UTGST will apply
                            //   Chandigarh     Andaman and Nicobar Islands    DADRA & NAGAR HAVELI    DAMAN & DIU     Lakshadweep              PONDICHERRY
                            if (ShippingState == "4" || ShippingState == "26" || ShippingState == "25" || ShippingState == "35" || ShippingState == "31" || ShippingState == "34")
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

        #endregion Header Portion Detail of the Page By Sam

        public DataTable GetBillingAddress()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 500, "QuotationBillingAddress");
            proc.AddVarcharPara("@QuotationID", 500, Convert.ToString(Session["SRM_ReturnID"]));
            dt = proc.GetTable();
            return dt;
        }

        #endregion Sam Section Start

        #region Trash Code Start

        //protected void aspxGridTax_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridViewEditorEventArgs e)
        //{

        //    if (e.Column.FieldName == "TaxName")
        //    {
        //        e.Editor.ReadOnly = true;
        //    }
        //    else if (e.Column.FieldName == "Amount")
        //    {
        //        e.Editor.ReadOnly = true;
        //    }
        //    else
        //    {
        //        e.Editor.ReadOnly = false;
        //    }
        //}

        //protected void Popup_SalesQuote_WindowCallback(object source, PopupWindowCallbackArgs e)
        //{
        //    Popup_SalesQuote.JSProperties["cpshow"] = "";
        //    Popup_SalesQuote.JSProperties["cpshowShip"] = "";
        //    DataTable dtaddbill = oDBEngine.GetDataTable("select add_addressType,add_address1,add_address2,add_address3,add_landMark,add_country,add_state,add_city,add_pin,add_area from tbl_master_address where add_cntId='LDP0000002' and add_addressType='Billing'");
        //    if (dtaddbill.Rows.Count > 0 && dtaddbill != null)
        //    {

        //        //for (int m = 0; m < dtaddbill.Rows.Count; m++)
        //        //{
        //        string add_addressType = Convert.ToString(dtaddbill.Rows[0]["add_addressType"]);
        //        string add_address1 = Convert.ToString(dtaddbill.Rows[0]["add_address1"]);
        //        string add_address2 = Convert.ToString(dtaddbill.Rows[0]["add_address2"]);
        //        string add_address3 = Convert.ToString(dtaddbill.Rows[0]["add_address3"]);
        //        string add_landMark = Convert.ToString(dtaddbill.Rows[0]["add_landMark"]);
        //        string add_country = Convert.ToString(dtaddbill.Rows[0]["add_country"]);
        //        string add_state = Convert.ToString(dtaddbill.Rows[0]["add_state"]);
        //        string add_city = Convert.ToString(dtaddbill.Rows[0]["add_city"]);
        //        string add_pin = Convert.ToString(dtaddbill.Rows[0]["add_pin"]);
        //        string add_area = Convert.ToString(dtaddbill.Rows[0]["add_area"]);

        //        //}

        //        Popup_SalesQuote.JSProperties["cpshow"] = add_addressType + "~"
        //                                           + add_address1 + "~"
        //                                           + add_address2 + "~"
        //                                           + add_address3 + "~"
        //                                           + add_landMark + "~"
        //                                           + add_country + "~"
        //                                           + add_state + "~"
        //                                           + add_city + "~"
        //                                           + add_pin + "~"
        //                                           + add_area + "~";

        //    }

        //    DataTable dtaship = oDBEngine.GetDataTable("select add_addressType,add_address1,add_address2,add_address3,add_landMark,add_country,add_state,add_city,add_pin,add_area from tbl_master_address where add_cntId='LDP0000002' and add_addressType='Shipping'");
        //    if (dtaship.Rows.Count > 0 && dtaship != null)
        //    {
        //        string add_saddressType = Convert.ToString(dtaship.Rows[0]["add_addressType"]);
        //        string add_saddress1 = Convert.ToString(dtaship.Rows[0]["add_address1"]);
        //        string add_saddress2 = Convert.ToString(dtaship.Rows[0]["add_address2"]);
        //        string add_saddress3 = Convert.ToString(dtaship.Rows[0]["add_address3"]);
        //        string add_slandMark = Convert.ToString(dtaship.Rows[0]["add_landMark"]);
        //        string add_scountry = Convert.ToString(dtaship.Rows[0]["add_country"]);
        //        string add_sstate = Convert.ToString(dtaship.Rows[0]["add_state"]);
        //        string add_scity = Convert.ToString(dtaship.Rows[0]["add_city"]);
        //        string add_spin = Convert.ToString(dtaship.Rows[0]["add_pin"]);
        //        string add_sarea = Convert.ToString(dtaship.Rows[0]["add_area"]);

        //        Popup_SalesQuote.JSProperties["cpshowShip"] = add_saddressType + "~"
        //                                          + add_saddress1 + "~"
        //                                          + add_saddress2 + "~"
        //                                          + add_saddress3 + "~"
        //                                          + add_slandMark + "~"
        //                                          + add_scountry + "~"
        //                                          + add_sstate + "~"
        //                                          + add_scity + "~"
        //                                          + add_spin + "~"
        //                                          + add_sarea + "~";

        //    }





        //}

        //public void GetEditablePermission()
        //{
        //    if (Request.QueryString["Permission"] != null)
        //    {
        //        if (Convert.ToString(Request.QueryString["Permission"]) == "1")
        //        {
        //            //pnl_quotation.Enabled = false;
        //            btn_SaveRecords.Visible = false;
        //            ASPxButtonmanual.Visible = false;
        //        }
        //        else if (Convert.ToString(Request.QueryString["Permission"]) == "2")
        //        {
        //            //pnl_quotation.Enabled = true;
        //            btn_SaveRecords.Visible = true;
        //            ASPxButtonmanual.Visible = true;
        //        }
        //        else if (Convert.ToString(Request.QueryString["Permission"]) == "3")
        //        {
        //            //pnl_quotation.Enabled = false;
        //            btn_SaveRecords.Visible = false;
        //            ASPxButtonmanual.Visible = false;
        //        }
        //    }
        //}

        #endregion Trash Code End

        protected void CmbProduct_Init(object sender, EventArgs e)
        {
            ASPxComboBox cityCombo = sender as ASPxComboBox;
            cityCombo.DataSource = GetProduct();
        }
        #region  Available Stock
        protected void acpAvailableStock_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string performpara = e.Parameter;
            string strProductID = Convert.ToString(performpara.Split('~')[0]);
            string strBranch = Convert.ToString(ddl_Branch.SelectedValue);
            acpAvailableStock.JSProperties["cpstock"] = "0.00";

            try
            {
                DataTable dt2 = oDBEngine.GetDataTable("Select dbo.fn_CheckAvailableQuotation(" + strBranch + ",'" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]) + "'," + strProductID + ") as branchopenstock");

                if (dt2.Rows.Count > 0)
                {
                    acpAvailableStock.JSProperties["cpstock"] = Convert.ToString(Math.Round(Convert.ToDecimal(dt2.Rows[0]["branchopenstock"]), 2));
                }
                else
                {
                    acpAvailableStock.JSProperties["cpstock"] = "0.00";
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion Available Stock


        protected void callback_InlineRemarks_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string strSplitCommand = e.Parameter.Split('~')[0];
            string SrlNo = e.Parameter.Split('~')[1];
            string RemarksVal = e.Parameter.Split('~')[2];
            Remarks = new DataTable();


            callback_InlineRemarks.JSProperties["cpDisplayFocus"] = "";

            if (strSplitCommand == "RemarksFinal")
            {
                if (Session["InlineRemarks"] != null)
                {
                    Remarks = (DataTable)Session["InlineRemarks"];
                }
                else
                {
                    if (Remarks == null || Remarks.Rows.Count == 0)
                    {
                        Remarks.Columns.Add("SrlNo", typeof(string));
                        Remarks.Columns.Add("ProjectAdditionRemarks", typeof(string));

                    }


                }

                DataRow[] deletedRow = Remarks.Select("SrlNo=" + SrlNo);

                if (deletedRow.Length > 0)
                {
                    foreach (DataRow dr in deletedRow)
                    {
                        Remarks.Rows.Remove(dr);
                    }
                    Remarks.AcceptChanges();
                }


                Remarks.Rows.Add(SrlNo, RemarksVal);

                Session["InlineRemarks"] = Remarks;
            }


            //else if(strSplitCommand=="BindRemarks")
            //{

            //    DataTable dt = GetOrderData().Tables[1];

            //   //txtInlineRemarks.Text=


            //}

            else if (strSplitCommand == "DisplayRemarks")
            {
                DataTable Remarksdt = (DataTable)Session["InlineRemarks"];
                if (Remarksdt != null)
                {
                    DataView dvData = new DataView(Remarksdt);
                    dvData.RowFilter = "SrlNo = '" + SrlNo + "'";
                    if (dvData.Count > 0)
                        txtInlineRemarks.Text = dvData[0]["ProjectAdditionRemarks"].ToString();
                    else
                        txtInlineRemarks.Text = "";
                }

                callback_InlineRemarks.JSProperties["cpDisplayFocus"] = "DisplayRemarksFocus";
            }
            //else if (strSplitCommand=="RemarksDelete")
            //{
            //    DeleteUnsaveaddRemarks(SrlNo, RemarksVal);

            //}


        }

        protected void acpContactPersonPhone_Callback(object sender, CallbackEventArgsBase e)
        {
            string WhichCall = e.Parameter.Split('~')[0];
            if (WhichCall == "ContactPersonPhone")
            {
                string InternalId = Convert.ToString(Convert.ToString(e.Parameter.Split('~')[1]));
                DataTable dtContactPersonPhone = new DataTable();
                dtContactPersonPhone = objSalesReturnBL.PopulateContactPersonPhone(InternalId);
                if (dtContactPersonPhone != null && dtContactPersonPhone.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtContactPersonPhone.Rows)
                    {
                        string phone = Convert.ToString(dr["add_phone"]);
                        if (!string.IsNullOrEmpty(phone))
                        {
                            acpContactPersonPhone.JSProperties["cpPhone"] = phone;
                            break;
                        }
                    }

                }
            }
        }
        [WebMethod]
        public static string getProductType(string Products_ID)
        {
            string Type = "";
            string query = @"Select
                           (Case When Isnull(Is_active_warehouse,'0')='0' AND Isnull(Is_active_Batch,'0')='0' AND Isnull(Is_active_serialno,'0')='0' Then ''
                           When Isnull(Is_active_warehouse,'0')='1' AND Isnull(Is_active_Batch,'0')='0' AND Isnull(Is_active_serialno,'0')='0' Then 'W'
                           When Isnull(Is_active_warehouse,'0')='0' AND Isnull(Is_active_Batch,'0')='1' AND Isnull(Is_active_serialno,'0')='0' Then 'B'
                           When Isnull(Is_active_warehouse,'0')='0' AND Isnull(Is_active_Batch,'0')='0' AND Isnull(Is_active_serialno,'0')='1' Then 'S'
                           When Isnull(Is_active_warehouse,'0')='1' AND Isnull(Is_active_Batch,'0')='1' AND Isnull(Is_active_serialno,'0')='0' Then 'WB'
                           When Isnull(Is_active_warehouse,'0')='1' AND Isnull(Is_active_Batch,'0')='0' AND Isnull(Is_active_serialno,'0')='1' Then 'WS'
                           When Isnull(Is_active_warehouse,'0')='1' AND Isnull(Is_active_Batch,'0')='1' AND Isnull(Is_active_serialno,'0')='1' Then 'WBS'
                           When Isnull(Is_active_warehouse,'0')='0' AND Isnull(Is_active_Batch,'0')='1' AND Isnull(Is_active_serialno,'0')='1' Then 'BS'
                           END) as Type
                           from Master_sProducts
                           where sProducts_ID='" + Products_ID + "'";

           // BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine();


            DataTable dt = oDbEngine.GetDataTable(query);
            if (dt != null && dt.Rows.Count > 0)
            {
                Type = Convert.ToString(dt.Rows[0]["Type"]);
            }

            return Convert.ToString(Type);
        }


        public void SetSalesReturnDetails(string strSalesReturnId)
        {
            DataSet SalesReturnEdit = objSalesReturnBL.SalesReturnDetailsForEdit(strSalesReturnId);
            DataTable SalesReturnEditdt = SalesReturnEdit.Tables[0];
            DataTable PlaceOfSupplyData = SalesReturnEdit.Tables[2];
            if (SalesReturnEditdt != null && SalesReturnEditdt.Rows.Count > 0)
            {
                string Branch_Id = Convert.ToString(SalesReturnEditdt.Rows[0]["Return_BranchId"]);
                string Quote_Number = Convert.ToString(SalesReturnEditdt.Rows[0]["Return_Number"]);
                string Quote_Date = Convert.ToString(SalesReturnEditdt.Rows[0]["Return_Date"]);
                string Customer_Id = Convert.ToString(SalesReturnEditdt.Rows[0]["Customer_Id"]);
                string Contact_Person_Id = Convert.ToString(SalesReturnEditdt.Rows[0]["Contact_Person_Id"]);
                string Quote_Reference = Convert.ToString(SalesReturnEditdt.Rows[0]["Return_Reference"]);
                string Currency_Id = Convert.ToString(SalesReturnEditdt.Rows[0]["Currency_Id"]);
                string Currency_Conversion_Rate = Convert.ToString(SalesReturnEditdt.Rows[0]["Currency_Conversion_Rate"]);
                string Tax_Option = Convert.ToString(SalesReturnEditdt.Rows[0]["Tax_Option"]);
                string Tax_Code = Convert.ToString(SalesReturnEditdt.Rows[0]["Tax_Code"]);
                string Quote_SalesmanId = Convert.ToString(SalesReturnEditdt.Rows[0]["Return_SalesmanId"]);
                string IsUsed = Convert.ToString(SalesReturnEditdt.Rows[0]["IsUsed"]);

                string CashBank_Code = Convert.ToString(SalesReturnEditdt.Rows[0]["CashBank_Code"]);
                string InvoiceCreatedFromDoc = Convert.ToString(SalesReturnEditdt.Rows[0]["ReturnCreatedFromDoc"]);
                string InvoiceCreatedFromDoc_Ids = Convert.ToString(SalesReturnEditdt.Rows[0]["ReturnCreatedFromDoc_Ids"]);
                string InvoiceCreatedFromDocDate = Convert.ToString(SalesReturnEditdt.Rows[0]["ReturnCreatedFromDocDate"]);
                string DueDate = Convert.ToString(SalesReturnEditdt.Rows[0]["DueDate"]);

                string VehicleNumber = Convert.ToString(SalesReturnEditdt.Rows[0]["VehicleNumber"]);
                string TransporterName = Convert.ToString(SalesReturnEditdt.Rows[0]["TransporterName"]);
                string TransporterPhone = Convert.ToString(SalesReturnEditdt.Rows[0]["TransporterPhone"]);
                string ReasonforReturn = Convert.ToString(SalesReturnEditdt.Rows[0]["ReasonforReturn"]);

                string ID = Convert.ToString(PlaceOfSupplyData.Rows[0]["ID"]);
                string Name = Convert.ToString(PlaceOfSupplyData.Rows[0]["Name"]);
                string StateId = Convert.ToString(PlaceOfSupplyData.Rows[0]["StateId"]);
                string StateCode = Convert.ToString(PlaceOfSupplyData.Rows[0]["StateCode"]);
                ddlPosGstReturnManual.DataSource = SalesReturnEdit.Tables[2];

                string TransCategory = Convert.ToString(SalesReturnEditdt.Rows[0]["TransCategory"]);
                drdTransCategory.SelectedValue = TransCategory;

                ddlPosGstReturnManual.ValueField = "ID";
                ddlPosGstReturnManual.TextField = "Name";
                ddlPosGstReturnManual.DataBind();

                string PosForGst = Convert.ToString(SalesReturnEditdt.Rows[0]["PosForGst"]);
                ddlPosGstReturnManual.Value = PosForGst;
                if (PosForGst=="S")
                {
                    hdnPOSShippingStateId.Value = StateId;
                    hdnPOSShippingStateCode.Value = StateCode;
                }
                else if (PosForGst == "B")
                {
                    hdnPOSBillingStateId.Value = StateId;
                    hdnPOSBillingStateCode.Value = StateCode;
                }

                Purchase_BillingShipping.SetBillingShippingTable(SalesReturnEdit.Tables[1]);
                txtReasonforChange.Text = ReasonforReturn;
                //ASPxTextBox txtDriverName = (ASPxTextBox)VehicleDetailsControl.FindControl("txtDriverName");
                //ASPxTextBox txtPhoneNo = (ASPxTextBox)VehicleDetailsControl.FindControl("txtPhoneNo");
                //DropDownList ddl_VehicleNo = (DropDownList)VehicleDetailsControl.FindControl("ddl_VehicleNo");

                //  txtDriverName.Text = TransporterName;
                //txtPhoneNo.Text = TransporterPhone;
                //ddl_VehicleNo.SelectedValue = VehicleNumber;

                //  ddlCashBank.Value = "";
                //if (InvoiceCreatedFromDoc != "") rdl_SaleInvoice.SelectedValue = InvoiceCreatedFromDoc;
                txt_InvoiceDate.Text = InvoiceCreatedFromDocDate;
                dt_SaleInvoiceDue.Date = Convert.ToDateTime(DueDate);



                DataTable dtt = GetReturnManualProjectEditData();
                if (dtt != null)
                {
                    lookup_Project.GridView.Selection.SelectRowByKey(Convert.ToInt64(dtt.Rows[0]["Proj_Id"]));

                    //Tanmoy  Hierarchy
                    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                    DataTable dt2 = oDBEngine.GetDataTable("Select Hierarchy_ID from V_ProjectList where Proj_Id='" + Convert.ToInt64(dtt.Rows[0]["Proj_Id"]) + "'");
                    if (dt2.Rows.Count > 0)
                    {
                        ddlHierarchy.SelectedValue = Convert.ToString(dt2.Rows[0]["Hierarchy_ID"]);
                    }
                    //Tanmoy  Hierarchy End
                }


                if (Contact_Person_Id != "0")
                {
                    cmbContactPerson.Value = Contact_Person_Id;

                    DataTable dtContactPersonPhone = new DataTable();
                    dtContactPersonPhone = objSalesReturnBL.PopulateContactPersonPhone(Contact_Person_Id);
                    if (dtContactPersonPhone != null && dtContactPersonPhone.Rows.Count > 0)
                    {
                        pageheaderContent.Attributes.Add("style", "display:block");
                        divContactPhone.Attributes.Add("style", "display:block");

                        foreach (DataRow dr in dtContactPersonPhone.Rows)
                        {
                            string phone = Convert.ToString(dr["add_phone"]);
                            if (!string.IsNullOrEmpty(phone))
                            {
                                lblContactPhone.Text = phone;
                                break;
                            }
                        }

                    }



                }
                //Outstanding

                DataTable OutStandingTable = objSalesReturnBL.GetCustomerOutStanding(Customer_Id);
                if (OutStandingTable.Rows.Count > 0)
                {
                    pageheaderContent.Attributes.Add("style", "display:block");
                    divOutstanding.Attributes.Add("style", "display:block");

                    var convertDecimal = Convert.ToDecimal(Convert.ToString(OutStandingTable.Rows[0]["NetOutstanding"]));


                    if (convertDecimal > 0)
                    {
                        lblOutstanding.Text = Convert.ToString(convertDecimal) + "(Db)";
                    }
                    else
                    {

                        lblOutstanding.Text = Convert.ToString(convertDecimal * -1).ToString() + "(Cr)";
                    }


                    // cmbContactPerson.JSProperties["cpOutstanding"] = Convert.ToString(OutStandingTable.Rows[0]["NetOutstanding"]);
                }
                else
                {
                    pageheaderContent.Attributes.Add("style", "display:block");
                    divOutstanding.Attributes.Add("style", "display:block");
                    lblOutstanding.Text = "0.0";
                    // cmbContactPerson.JSProperties["cpOutstanding"] = null;
                }

                //Outstanding


                //gstn
                DataTable GSTNTable = objSalesReturnBL.GetCustomerGSTIN(Customer_Id);
                if (GSTNTable != null && GSTNTable.Rows.Count > 0)
                {
                    pageheaderContent.Attributes.Add("style", "display:block");
                    divGSTN.Attributes.Add("style", "display:block");

                    var ghstnval = Convert.ToString(Convert.ToString(GSTNTable.Rows[0]["CNT_GSTIN"]));

                    if (ghstnval != "")
                    { lblGSTIN.Text = "Yes"; }
                    else { lblGSTIN.Text = "No"; }





                    // cmbContactPerson.JSProperties["cpOutstanding"] = Convert.ToString(OutStandingTable.Rows[0]["NetOutstanding"]);
                }
                //gstn
                if (!String.IsNullOrEmpty(InvoiceCreatedFromDoc_Ids))
                {
                    string[] eachQuo = InvoiceCreatedFromDoc_Ids.Split(',');
                    if (eachQuo.Length > 1)//More tha one quotation
                    {
                        BindLookUp(Customer_Id, Convert.ToDateTime(Quote_Date).ToString("yyyy-MM-dd"), Branch_Id);
                        foreach (string val in eachQuo)
                        {
                            lookup_quotation.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));
                        }
                        Session["TaggingInvoce"] = "1";
                    }
                    else if (eachQuo.Length == 1)//Single Quotation
                    {
                        BindLookUp(Customer_Id, Convert.ToDateTime(Quote_Date).ToString("yyyy-MM-dd"), Branch_Id);
                        foreach (string val in eachQuo)
                        {
                            lookup_quotation.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));
                        }
                        Session["TaggingInvoce"] = "1";
                    }
                    else//No Quotation selected
                    {
                        BindLookUp(Customer_Id, Convert.ToDateTime(Quote_Date).ToString("yyyy-MM-dd"), Branch_Id);
                        Session["TaggingInvoce"] = "0";
                    }
                }


                txt_PLQuoteNo.Text = Quote_Number;
                dt_PLQuote.Date = Convert.ToDateTime(Quote_Date);
                PopulateGSTCSTVATCombo(Convert.ToDateTime(Quote_Date).ToString("yyyy-MM-dd"));
                PopulateChargeGSTCSTVATCombo(Convert.ToDateTime(Quote_Date).ToString("yyyy-MM-dd"));
                //lookup_Customer.GridView.Selection.SelectRowByKey(Customer_Id);
                // SetCustomerDDbyValue(Customer_Id);
                hdnCustomerId.Value = Customer_Id;

                txtCustName.Text = Convert.ToString(SalesReturnEditdt.Rows[0]["Name"]);
                TabPage page = ASPxPageControl1.TabPages.FindByName("Billing/Shipping");
                page.ClientEnabled = true;

                PopulateContactPersonOfCustomer(Customer_Id);
                cmbContactPerson.SelectedItem = cmbContactPerson.Items.FindByValue(Contact_Person_Id);
                txt_Refference.Text = Quote_Reference;
                ddl_Branch.SelectedValue = Branch_Id;
                ddl_SalesAgent.SelectedValue = Quote_SalesmanId;
                ddl_Currency.SelectedValue = Currency_Id;
                txt_Rate.Value = Currency_Conversion_Rate;
                txt_Rate.Text = Currency_Conversion_Rate;
                if (Tax_Option != "0") ddl_AmountAre.Value = Tax_Option;
                if (Tax_Code != "0")
                {
                    PopulateGSTCSTVAT("2");
                    setValueForHeaderGST(ddl_VatGstCst, Tax_Code);
                }
                else
                {
                    PopulateGSTCSTVAT("2");
                    ddl_VatGstCst.SelectedIndex = 0;
                }
                ddl_AmountAre.ClientEnabled = false;
                ddl_VatGstCst.ClientEnabled = false;

                if (IsUsed == "Y")
                {
                    dt_PLQuote.ClientEnabled = false;
                    txtCustName.ClientEnabled = false;
                }
                else
                {
                    dt_PLQuote.ClientEnabled = true;
                    txtCustName.ClientEnabled = true;
                }




                ProcedureExecute procCustomerStateCode = new ProcedureExecute("prc_CRMSalesReturn_Details");
                procCustomerStateCode.AddVarcharPara("@Action", 500, "GetCustomerStateCode");
                procCustomerStateCode.AddVarcharPara("@CustomerID", 100, Convert.ToString(Customer_Id));

                DataTable CustomerStateCodeDT = procCustomerStateCode.GetTable();

                string CustomerStateCode = "";

                if (CustomerStateCodeDT != null && CustomerStateCodeDT.Rows.Count > 0)
                {
                    CustomerStateCode = Convert.ToString(CustomerStateCodeDT.Rows[0]["StateCode"]);

                    hdnCustomerStateCodeId.Value = CustomerStateCode;

                }





            }
        }

        private int updatewarehouse()
        {
            if (Session["SRwarehousedetailstempUpdate"] != null)
            {
                DataTable dt = new DataTable();
                DataTable Warehousedtups = (DataTable)Session["SRwarehousedetailstempUpdate"];
                DataRow[] dr = Warehousedtups.Select("isnew = 'Updated'");
                if (dr.Count() != 0)
                {
                    dt = dr.CopyToDataTable();
                    //if (Session["PcwarehousedetailstempUpdate"] != null)
                    //{
                    //    temtable23 = (DataTable)Session["PcwarehousedetailstempUpdate"];
                    //    DataRow[] rows;
                    //    rows = temtable23.Select("pcslno = '" + hdnpcslno.Value + "'");  //'UserName' is ColumnName
                    //    foreach (DataRow row in rows)
                    //        temtable23.Rows.Remove(row);

                    //    temtable23.Merge(dt);
                    //    Session["PcwarehousedetailstempUpdate"] = temtable23;
                    //}
                    //else
                    //{
                    //    Session["PcwarehousedetailstempUpdate"] = dt;
                    //}

                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        string iswarehouse = string.Empty;
                        string isbatch = string.Empty;
                        string isserial = string.Empty;



                        #region WHBTSL
                        if (Convert.ToString(dt.Rows[i]["Inventrytype"]) == "WHBTSL")
                        {
                            DateTime? mfgex;
                            DateTime? exdate;
                            if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewMFGDate"])) && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewExpiryDate"])))
                            {
                                if ((Convert.ToString(dt.Rows[i]["viewMFGDate"]) != "1/1/0001 12:00:00 AM" && Convert.ToString(dt.Rows[i]["viewExpiryDate"]) != "1/1/0001 12:00:00 AM" && Convert.ToString(dt.Rows[i]["viewMFGDate"]) != "01-01-0001 12:00:00 AM" && Convert.ToString(dt.Rows[i]["viewExpiryDate"]) != "01-01-0001 12:00:00 AM"))
                                {
                                    mfgex = Convert.ToDateTime(dt.Rows[i]["viewMFGDate"]);
                                    exdate = Convert.ToDateTime(dt.Rows[i]["viewExpiryDate"]);
                                }
                                else
                                {
                                    mfgex = null;
                                    exdate = null;
                                }

                            }
                            else
                            {
                                mfgex = null;
                                exdate = null;
                            }

                            if (Convert.ToString(dt.Rows[i]["viewQuantity"]) != "0" && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewQuantity"])))
                            {
                                string sql = string.Empty;
                                if (mfgex == null && exdate == null)
                                {
                                    if (hdnisolddeleted.Value == "true")
                                    {

                                        var updateqnty = dt.Compute("sum(Quantitysum)", "isnew = 'Updated' AND WarehouseID='" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + "' AND BatchNo='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'");
                                        if (string.IsNullOrEmpty(Convert.ToString(updateqnty)))
                                        {
                                            updateqnty = Convert.ToDecimal(dt.Rows[i]["Quantitysum"]);
                                        }

                                        sql = "update Trans_StockBranchWarehouse set StockBranchWarehouse_StockIn=" + Convert.ToDecimal(updateqnty) + ",StockBranchWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where StockBranchWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
                                                  "  update Trans_StockBranchWarehouseDetails set Stock_OpeningRate=" + Convert.ToDecimal(hdnrate.Value) + "," +
                                                             "Stock_OpeningValue=ISNULL(isnull(" + Convert.ToDecimal(updateqnty) + ",0)*isnull(" + Convert.ToDecimal(hdnrate.Value) + ",0),0)," +
                                                             "ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                             "ModifiedDate=GETDATE()" +
                                                             "where StockBranchWarehouseDetail_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehousedetailsID"]) + "" +
                                                             "   update Trans_StockBranchBatch set   StockBranchBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                           "StockBranchBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +


                                                           "StockBranchBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                           "StockBranchBatch_ModifiedDate=GETDATE() " +
                                                      "where StockBranchBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]) + "	  " +
                                                      "  update Trans_StockSerialMapping" +
                                            " Set Stock_BranchSerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
                                            " where Stock_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";
                                    }
                                    else
                                    {
                                        sql = "update Trans_StockBranchWarehouse set StockBranchWarehouse_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + ",StockBranchWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where StockBranchWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
                                                    "  update Trans_StockBranchWarehouseDetails set Stock_OpeningRate=" + Convert.ToDecimal(hdnrate.Value) + "," +
                                                               "Stock_OpeningValue=ISNULL(isnull(" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + ",0)*isnull(" + Convert.ToDecimal(hdnrate.Value) + ",0),0)," +
                                                               "ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                               "ModifiedDate=GETDATE() " +
                                                               "where StockBranchWarehouseDetail_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehousedetailsID"]) + "" +
                                                               "   update Trans_StockBranchBatch set   StockBranchBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                             "StockBranchBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +


                                                             "StockBranchBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                             "StockBranchBatch_ModifiedDate=GETDATE() " +
                                                        "where StockBranchBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]) + "	  " +
                                                        "  update Trans_StockSerialMapping" +
                                              " Set Stock_BranchSerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
                                              " where Stock_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";
                                    }
                                }
                                else
                                {
                                    if (hdnisolddeleted.Value == "true")
                                    {

                                        var updateqnty = dt.Compute("sum(Quantitysum)", "isnew = 'Updated' AND WarehouseID='" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + "' AND BatchNo='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'");
                                        if (string.IsNullOrEmpty(Convert.ToString(updateqnty)))
                                        {
                                            updateqnty = Convert.ToDecimal(dt.Rows[i]["Quantitysum"]);
                                        }
                                        sql = "update Trans_StockBranchWarehouse set StockBranchWarehouse_StockIn=" + Convert.ToDecimal(updateqnty) + ",StockBranchWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where StockBranchWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
                                                      "  update Trans_StockBranchWarehouseDetails set Stock_OpeningRate=" + Convert.ToDecimal(hdnrate.Value) + "," +
                                                                 "Stock_OpeningValue=ISNULL(isnull(" + Convert.ToDecimal(updateqnty) + ",0)*isnull(" + Convert.ToDecimal(hdnrate.Value) + ",0),0)," +
                                                                 "ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                                 "ModifiedDate=GETDATE()" +
                                                                 "where StockBranchWarehouseDetail_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehousedetailsID"]) + "" +
                                                                 "   update Trans_StockBranchBatch set   StockBranchBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                               "StockBranchBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +

                                                               "StockBranchBatch_MfgDate='" + mfgex + "'," +
                                                               "StockBranchBatch_ExpiryDate='" + exdate + "'," +
                                                               "StockBranchBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                               "StockBranchBatch_ModifiedDate=GETDATE() " +
                                                          "where StockBranchBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]) + "	  " +
                                                          "  update Trans_StockSerialMapping" +
                                                " Set Stock_BranchSerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
                                                " where Stock_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";
                                    }
                                    else
                                    {
                                        sql = "update Trans_StockBranchWarehouse set StockBranchWarehouse_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + ",StockBranchWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where StockBranchWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
                                                 "  update Trans_StockBranchWarehouseDetails set Stock_OpeningRate=" + Convert.ToDecimal(hdnrate.Value) + "," +
                                                            "Stock_OpeningValue=ISNULL(isnull(" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + ",0)*isnull(" + Convert.ToDecimal(hdnrate.Value) + ",0),0)," +
                                                            "ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                            "ModifiedDate=GETDATE()" +
                                                            "where StockBranchWarehouseDetail_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehousedetailsID"]) + "" +
                                                            "   update Trans_StockBranchBatch set   StockBranchBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                          "StockBranchBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +

                                                          "StockBranchBatch_MfgDate='" + mfgex + "'," +
                                                          "StockBranchBatch_ExpiryDate='" + exdate + "'," +
                                                          "StockBranchBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                          "StockBranchBatch_ModifiedDate=GETDATE() " +
                                                     "where StockBranchBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]) + "	  " +
                                                     "  update Trans_StockSerialMapping" +
                                           " Set Stock_BranchSerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
                                           " where Stock_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";
                                    }


                                }


                                oDBEngine.GetDataTable(sql);

                            }
                            else
                            {
                                //var updateqnty = dt.Compute("sum(Quantitysum)", "isnew = 'Updated' AND WarehouseID='" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + "' AND BatchNo='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'");


                                string sql = "update Trans_StockBranchWarehouse set StockBranchWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where StockBranchWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
                                              "  update Trans_StockBranchWarehouseDetails set " +
                                                         "ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                         "ModifiedDate=GETDATE() " +
                                                         "where StockBranchWarehouseDetail_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehousedetailsID"]) + "" +

                                                  "  update Trans_StockSerialMapping" +
                                        " Set Stock_BranchSerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
                                        " where Stock_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";

                                oDBEngine.GetDataTable(sql);
                            }
                        }
                        #endregion
                        #region WHBT
                        if (Convert.ToString(dt.Rows[i]["Inventrytype"]) == "WHBT")
                        {
                            DateTime? mfgex;
                            DateTime? exdate;

                            var updateqnty = dt.Compute("sum(Quantitysum)", "isnew = 'Updated' AND BatchWarehouseID='" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "'");
                            var OLDSAMEwarehouse = Warehousedtups.Compute("sum(Quantitysum)", "isnew = 'old' AND BatchWarehouseID='" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "'");
                            if (!string.IsNullOrEmpty(Convert.ToString(OLDSAMEwarehouse)) && !string.IsNullOrEmpty(Convert.ToString(updateqnty)))
                            {

                                updateqnty = Convert.ToDecimal(updateqnty) + Convert.ToDecimal(OLDSAMEwarehouse);
                            }

                            if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewMFGDate"])) && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewExpiryDate"])))
                            {
                                if ((Convert.ToString(dt.Rows[i]["viewMFGDate"]) != "1/1/0001 12:00:00 AM" && Convert.ToString(dt.Rows[i]["viewExpiryDate"]) != "1/1/0001 12:00:00 AM" && Convert.ToString(dt.Rows[i]["viewMFGDate"]) != "01-01-0001 12:00:00 AM" && Convert.ToString(dt.Rows[i]["viewExpiryDate"]) != "01-01-0001 12:00:00 AM"))
                                {
                                    mfgex = Convert.ToDateTime(dt.Rows[i]["viewMFGDate"]);
                                    exdate = Convert.ToDateTime(dt.Rows[i]["viewExpiryDate"]);
                                }
                                else
                                {
                                    mfgex = null;
                                    exdate = null;
                                }

                            }
                            else
                            {
                                mfgex = null;
                                exdate = null;
                            }

                            if (Convert.ToString(dt.Rows[i]["viewQuantity"]) != "0" && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewQuantity"])) && !string.IsNullOrEmpty(Convert.ToString(updateqnty)))
                            {
                                string sql = string.Empty;
                                if (mfgex == null && exdate == null)
                                {
                                    sql = "update Trans_StockBranchWarehouse set StockBranchWarehouse_StockIn=" + Convert.ToDecimal(updateqnty) + ",StockBranchWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where StockBranchWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
                                              "  update Trans_StockBranchWarehouseDetails set Stock_OpeningRate=" + Convert.ToDecimal(hdnrate.Value) + "," +
                                                         "Stock_OpeningValue=ISNULL(isnull(" + Convert.ToDecimal(updateqnty) + ",0)*isnull(" + Convert.ToDecimal(hdnrate.Value) + ",0),0)," +
                                                         "ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                         "ModifiedDate=GETDATE() " +
                                                         "where StockBranchWarehouseDetail_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehousedetailsID"]) + "" +
                                                         "   update Trans_StockBranchBatch set   StockBranchBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                       "StockBranchBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +


                                                       "StockBranchBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                       "StockBranchBatch_ModifiedDate=GETDATE() " +
                                                  "where StockBranchBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]);


                                }
                                else
                                {
                                    sql = "update Trans_StockBranchWarehouse set StockBranchWarehouse_StockIn=" + Convert.ToDecimal(updateqnty) + ",StockBranchWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where StockBranchWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
                                                  "  update Trans_StockBranchWarehouseDetails set Stock_OpeningRate=" + Convert.ToDecimal(hdnrate.Value) + "," +
                                                             "Stock_OpeningValue=ISNULL(isnull(" + Convert.ToDecimal(updateqnty) + ",0)*isnull(" + Convert.ToDecimal(hdnrate.Value) + ",0),0)," +
                                                             "ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                             "ModifiedDate=GETDATE() " +
                                                             "where StockBranchWarehouseDetail_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehousedetailsID"]) + "" +
                                                             "   update Trans_StockBranchBatch set   StockBranchBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                           "StockBranchBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +

                                                           "StockBranchBatch_MfgDate='" + mfgex + "'," +
                                                           "StockBranchBatch_ExpiryDate='" + exdate + "'," +
                                                           "StockBranchBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                           "StockBranchBatch_ModifiedDate=GETDATE() " +
                                                      "where StockBranchBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]);
                                }


                                oDBEngine.GetDataTable(sql);

                            }


                        }
                        #endregion
                        #region WHSL
                        if (Convert.ToString(dt.Rows[i]["Inventrytype"]) == "WHSL")
                        {
                            DateTime? mfgex;
                            DateTime? exdate;
                            if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewMFGDate"])) && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewExpiryDate"])))
                            {
                                if ((Convert.ToString(dt.Rows[i]["viewMFGDate"]) != "1/1/0001 12:00:00 AM" && Convert.ToString(dt.Rows[i]["viewExpiryDate"]) != "1/1/0001 12:00:00 AM" && Convert.ToString(dt.Rows[i]["viewMFGDate"]) != "01-01-0001 12:00:00 AM" && Convert.ToString(dt.Rows[i]["viewExpiryDate"]) != "01-01-0001 12:00:00 AM"))
                                {
                                    mfgex = Convert.ToDateTime(dt.Rows[i]["viewMFGDate"]);
                                    exdate = Convert.ToDateTime(dt.Rows[i]["viewExpiryDate"]);
                                }
                                else
                                {
                                    mfgex = null;
                                    exdate = null;
                                }

                            }
                            else
                            {
                                mfgex = null;
                                exdate = null;
                            }

                            if (Convert.ToString(dt.Rows[i]["viewQuantity"]) != "0" && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewQuantity"])))
                            {
                                string sql = string.Empty;
                                if (mfgex == null && exdate == null)
                                {
                                    sql = "update Trans_StockBranchWarehouse set StockBranchWarehouse_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + ",StockBranchWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where StockBranchWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
                                              "  update Trans_StockBranchWarehouseDetails set Stock_OpeningRate=" + Convert.ToDecimal(hdnrate.Value) + "," +
                                                         "Stock_OpeningValue=ISNULL(isnull(" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + ",0)*isnull(" + Convert.ToDecimal(hdnrate.Value) + ",0),0)," +
                                                         "ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                         "ModifiedDate=GETDATE()" +
                                                         "where StockBranchWarehouseDetail_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehousedetailsID"]) + "" +
                                                         "   update Trans_StockBranchBatch set   StockBranchBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                       "StockBranchBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +


                                                       "StockBranchBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                       "StockBranchBatch_ModifiedDate=GETDATE()" +
                                                  "where StockBranchBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]) + "	  " +
                                                  "  update Trans_StockSerialMapping" +
                                        " Set Stock_BranchSerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
                                        " where Stock_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";

                                }
                                else
                                {
                                    sql = "update Trans_StockBranchWarehouse set StockBranchWarehouse_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + ",StockBranchWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where StockBranchWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
                                                  "  update Trans_StockBranchWarehouseDetails set Stock_OpeningRate=" + Convert.ToDecimal(hdnrate.Value) + "," +
                                                             "Stock_OpeningValue=ISNULL(isnull(" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + ",0)*isnull(" + Convert.ToDecimal(hdnrate.Value) + ",0),0)," +
                                                             "ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                             "ModifiedDate=GETDATE()" +
                                                             "where StockBranchWarehouseDetail_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehousedetailsID"]) + "" +
                                                             "   update Trans_StockBranchBatch set   StockBranchBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                           "StockBranchBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +

                                                           "StockBranchBatch_MfgDate='" + mfgex + "'," +
                                                           "StockBranchBatch_ExpiryDate='" + exdate + "'," +
                                                           "StockBranchBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                           "StockBranchBatch_ModifiedDate=GETDATE()" +
                                                      "where StockBranchBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]) + "	  " +
                                                      "  update Trans_StockSerialMapping" +
                                            " Set Stock_BranchSerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
                                            " where Stock_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";
                                }


                                oDBEngine.GetDataTable(sql);

                            }
                            else
                            {
                                //var updateqnty = dt.Compute("sum(Quantitysum)", "isnew = 'Updated' AND WarehouseID='" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + "' AND BatchNo='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'");


                                string sql = "update Trans_StockBranchWarehouse set StockBranchWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where StockBranchWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
                                              "  update Trans_StockBranchWarehouseDetails set " +
                                                         "ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                         "ModifiedDate=GETDATE()" +
                                                         "where StockBranchWarehouseDetail_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehousedetailsID"]) + "" +

                                                  "  update Trans_StockSerialMapping" +
                                        " Set Stock_BranchSerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
                                        " where Stock_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";

                                oDBEngine.GetDataTable(sql);
                            }
                        }
                        #endregion

                        #region WH
                        if (Convert.ToString(dt.Rows[i]["Inventrytype"]) == "WH")
                        {
                            if (Convert.ToString(dt.Rows[i]["viewQuantity"]) != "0" && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Quantity"])))
                            {

                                string sql = "update Trans_StockBranchWarehouse set StockBranchWarehouse_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantity"]) + ",StockBranchWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where StockBranchWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
                                            "  update Trans_StockBranchWarehouseDetails set Stock_OpeningRate=" + Convert.ToDecimal(hdnrate.Value) + "," +
                                                       " Stock_OpeningValue=ISNULL(isnull(" + Convert.ToDecimal(dt.Rows[i]["Quantity"]) + ",0)*isnull(" + Convert.ToDecimal(hdnrate.Value) + ",0),0)," +
                                                       " ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                       " ModifiedDate=GETDATE() " +
                                                       " where StockBranchWarehouseDetail_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehousedetailsID"]) + "" +
                                                       "   update Trans_StockBranchBatch set   StockBranchBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                     "StockBranchBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantity"]) + "," +


                                                     "StockBranchBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                     "StockBranchBatch_ModifiedDate=GETDATE() " +
                                                " where StockBranchBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]);




                                oDBEngine.GetDataTable(sql);

                            }


                        }
                        #endregion

                        #region BTSL
                        if (Convert.ToString(dt.Rows[i]["Inventrytype"]) == "BTSL")
                        {
                            DateTime? mfgex;
                            DateTime? exdate;
                            if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewMFGDate"])) && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewExpiryDate"])))
                            {
                                if ((Convert.ToString(dt.Rows[i]["viewMFGDate"]) != "1/1/0001 12:00:00 AM" && Convert.ToString(dt.Rows[i]["viewExpiryDate"]) != "1/1/0001 12:00:00 AM" && Convert.ToString(dt.Rows[i]["viewMFGDate"]) != "01-01-0001 12:00:00 AM" && Convert.ToString(dt.Rows[i]["viewExpiryDate"]) != "01-01-0001 12:00:00 AM"))
                                {
                                    mfgex = Convert.ToDateTime(dt.Rows[i]["viewMFGDate"]);
                                    exdate = Convert.ToDateTime(dt.Rows[i]["viewExpiryDate"]);
                                }
                                else
                                {
                                    mfgex = null;
                                    exdate = null;
                                }

                            }
                            else
                            {
                                mfgex = null;
                                exdate = null;
                            }

                            if (Convert.ToString(dt.Rows[i]["viewQuantity"]) != "0" && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewQuantity"])))
                            {
                                string sql = string.Empty;
                                if (mfgex == null && exdate == null)
                                {
                                    if (hdnisolddeleted.Value == "true")
                                    {

                                        var updateqnty = dt.Compute("sum(Quantitysum)", "isnew = 'Updated'  AND BatchNo='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'");
                                        if (string.IsNullOrEmpty(Convert.ToString(updateqnty)))
                                        {
                                            updateqnty = Convert.ToDecimal(dt.Rows[i]["Quantitysum"]);
                                        }

                                        sql = "update Trans_StockBranchWarehouse set StockBranchWarehouse_StockIn=" + Convert.ToDecimal(updateqnty) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where StockBranchWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
                                                  "  update Trans_StockBranchWarehouseDetails set Stock_OpeningRate=" + Convert.ToDecimal(hdnrate.Value) + "," +
                                                             "Stock_OpeningValue=ISNULL(isnull(" + Convert.ToDecimal(updateqnty) + ",0)*isnull(" + Convert.ToDecimal(hdnrate.Value) + ",0),0)," +
                                                             "ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                             "ModifiedDate=GETDATE()" +
                                                             "where StockBranchWarehouseDetail_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehousedetailsID"]) + "" +
                                                             "   update Trans_StockBranchBatch set   StockBranchBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                           "StockBranchBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +


                                                           "StockBranchBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                           "StockBranchBatch_ModifiedDate=GETDATE() " +
                                                      "where StockBranchBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]) + "	  " +
                                                      "  update Trans_StockSerialMapping" +
                                            " Set Stock_BranchSerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
                                            " where Stock_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";
                                    }
                                    else
                                    {
                                        sql = "update Trans_StockBranchWarehouse set StockBranchWarehouse_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where StockBranchWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
                                                    "  update Trans_StockBranchWarehouseDetails set Stock_OpeningRate=" + Convert.ToDecimal(hdnrate.Value) + "," +
                                                               "Stock_OpeningValue=ISNULL(isnull(" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + ",0)*isnull(" + Convert.ToDecimal(hdnrate.Value) + ",0),0)," +
                                                               "ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                               "ModifiedDate=GETDATE() " +
                                                               "where StockBranchWarehouseDetail_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehousedetailsID"]) + "" +
                                                               "   update Trans_StockBranchBatch set   StockBranchBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                             "StockBranchBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +


                                                             "StockBranchBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                             "StockBranchBatch_ModifiedDate=GETDATE() " +
                                                        "where StockBranchBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]) + "	  " +
                                                        "  update Trans_StockSerialMapping" +
                                              " Set Stock_BranchSerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
                                              " where Stock_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";
                                    }
                                }
                                else
                                {
                                    if (hdnisolddeleted.Value == "true")
                                    {

                                        var updateqnty = dt.Compute("sum(Quantitysum)", "isnew = 'Updated' AND BatchNo='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'");
                                        if (string.IsNullOrEmpty(Convert.ToString(updateqnty)))
                                        {
                                            updateqnty = Convert.ToDecimal(dt.Rows[i]["Quantitysum"]);
                                        }
                                        sql = "update Trans_StockBranchWarehouse set StockBranchWarehouse_StockIn=" + Convert.ToDecimal(updateqnty) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where StockBranchWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
                                                      "  update Trans_StockBranchWarehouseDetails set Stock_OpeningRate=" + Convert.ToDecimal(hdnrate.Value) + "," +
                                                                 "Stock_OpeningValue=ISNULL(isnull(" + Convert.ToDecimal(updateqnty) + ",0)*isnull(" + Convert.ToDecimal(hdnrate.Value) + ",0),0)," +
                                                                 "ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                                 "ModifiedDate=GETDATE()" +
                                                                 "where StockBranchWarehouseDetail_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehousedetailsID"]) + "" +
                                                                 "   update Trans_StockBranchBatch set   StockBranchBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                               "StockBranchBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +

                                                               "StockBranchBatch_MfgDate='" + mfgex + "'," +
                                                               "StockBranchBatch_ExpiryDate='" + exdate + "'," +
                                                               "StockBranchBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                               "StockBranchBatch_ModifiedDate=GETDATE() " +
                                                          "where StockBranchBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]) + "	  " +
                                                          "  update Trans_StockSerialMapping" +
                                                " Set Stock_BranchSerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
                                                " where Stock_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";
                                    }
                                    else
                                    {
                                        sql = "update Trans_StockBranchWarehouse set StockBranchWarehouse_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where StockBranchWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
                                                 "  update Trans_StockBranchWarehouseDetails set Stock_OpeningRate=" + Convert.ToDecimal(hdnrate.Value) + "," +
                                                            "Stock_OpeningValue=ISNULL(isnull(" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + ",0)*isnull(" + Convert.ToDecimal(hdnrate.Value) + ",0),0)," +
                                                            "ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                            "ModifiedDate=GETDATE()" +
                                                            "where StockBranchWarehouseDetail_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehousedetailsID"]) + "" +
                                                            "   update Trans_StockBranchBatch set   StockBranchBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                          "StockBranchBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +

                                                          "StockBranchBatch_MfgDate='" + mfgex + "'," +
                                                          "StockBranchBatch_ExpiryDate='" + exdate + "'," +
                                                          "StockBranchBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                          "StockBranchBatch_ModifiedDate=GETDATE() " +
                                                     "where StockBranchBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]) + "	  " +
                                                     "  update Trans_StockSerialMapping" +
                                           " Set Stock_BranchSerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
                                           " where Stock_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";
                                    }


                                }


                                oDBEngine.GetDataTable(sql);

                            }
                            else
                            {
                                //var updateqnty = dt.Compute("sum(Quantitysum)", "isnew = 'Updated' AND WarehouseID='" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + "' AND BatchNo='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'");


                                string sql = "update Trans_StockBranchWarehouse set StockBranchWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where StockBranchWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
                                              "  update Trans_StockBranchWarehouseDetails set " +
                                                         "ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                         "ModifiedDate=GETDATE() " +
                                                         "where StockBranchWarehouseDetail_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehousedetailsID"]) + "" +

                                                  "  update Trans_StockSerialMapping" +
                                        " Set Stock_BranchSerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
                                        " where Stock_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";

                                oDBEngine.GetDataTable(sql);
                            }
                        }
                        #endregion

                        #region BT
                        if (Convert.ToString(dt.Rows[i]["Inventrytype"]) == "BTSL")
                        {
                            DateTime? mfgex;
                            DateTime? exdate;

                            var updateqnty = dt.Compute("sum(Quantitysum)", "isnew = 'Updated' AND BatchWarehouseID='" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "'");
                            var OLDSAMEwarehouse = Warehousedtups.Compute("sum(Quantitysum)", "isnew = 'old' AND BatchWarehouseID='" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "'");
                            if (!string.IsNullOrEmpty(Convert.ToString(OLDSAMEwarehouse)) && !string.IsNullOrEmpty(Convert.ToString(updateqnty)))
                            {

                                updateqnty = Convert.ToDecimal(updateqnty) + Convert.ToDecimal(OLDSAMEwarehouse);
                            }

                            if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewMFGDate"])) && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewExpiryDate"])))
                            {
                                if ((Convert.ToString(dt.Rows[i]["viewMFGDate"]) != "1/1/0001 12:00:00 AM" && Convert.ToString(dt.Rows[i]["viewExpiryDate"]) != "1/1/0001 12:00:00 AM" && Convert.ToString(dt.Rows[i]["viewMFGDate"]) != "01-01-0001 12:00:00 AM" && Convert.ToString(dt.Rows[i]["viewExpiryDate"]) != "01-01-0001 12:00:00 AM"))
                                {
                                    mfgex = Convert.ToDateTime(dt.Rows[i]["viewMFGDate"]);
                                    exdate = Convert.ToDateTime(dt.Rows[i]["viewExpiryDate"]);
                                }
                                else
                                {
                                    mfgex = null;
                                    exdate = null;
                                }

                            }
                            else
                            {
                                mfgex = null;
                                exdate = null;
                            }

                            if (Convert.ToString(dt.Rows[i]["viewQuantity"]) != "0" && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewQuantity"])) && !string.IsNullOrEmpty(Convert.ToString(updateqnty)))
                            {
                                string sql = string.Empty;
                                if (mfgex == null && exdate == null)
                                {
                                    sql = "update Trans_StockBranchWarehouse set StockBranchWarehouse_StockIn=" + Convert.ToDecimal(updateqnty) + ",StockBranchWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where StockBranchWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
                                              "  update Trans_StockBranchWarehouseDetails set Stock_OpeningRate=" + Convert.ToDecimal(hdnrate.Value) + "," +
                                                         "Stock_OpeningValue=ISNULL(isnull(" + Convert.ToDecimal(updateqnty) + ",0)*isnull(" + Convert.ToDecimal(hdnrate.Value) + ",0),0)," +
                                                         "ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                         "ModifiedDate=GETDATE() " +
                                                         "where StockBranchWarehouseDetail_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehousedetailsID"]) + "" +
                                                         "   update Trans_StockBranchBatch set   StockBranchBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                       "StockBranchBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +


                                                       "StockBranchBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                       "StockBranchBatch_ModifiedDate=GETDATE() " +
                                                  "where StockBranchBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]);


                                }
                                else
                                {
                                    sql = "update Trans_StockBranchWarehouse set StockBranchWarehouse_StockIn=" + Convert.ToDecimal(updateqnty) + ",StockBranchWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where StockBranchWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
                                                  "  update Trans_StockBranchWarehouseDetails set Stock_OpeningRate=" + Convert.ToDecimal(hdnrate.Value) + "," +
                                                             "Stock_OpeningValue=ISNULL(isnull(" + Convert.ToDecimal(updateqnty) + ",0)*isnull(" + Convert.ToDecimal(hdnrate.Value) + ",0),0)," +
                                                             "ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                             "ModifiedDate=GETDATE() " +
                                                             "where StockBranchWarehouseDetail_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehousedetailsID"]) + "" +
                                                             "   update Trans_StockBranchBatch set   StockBranchBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                           "StockBranchBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +

                                                           "StockBranchBatch_MfgDate='" + mfgex + "'," +
                                                           "StockBranchBatch_ExpiryDate='" + exdate + "'," +
                                                           "StockBranchBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                           "StockBranchBatch_ModifiedDate=GETDATE() " +
                                                      "where StockBranchBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]);
                                }


                                oDBEngine.GetDataTable(sql);

                            }


                        }
                        #endregion

                    }
                }
            }

            return 0;
        }

        public DataTable GetComponentEditedAddressData(string ComponentDetailsIDs, string strType)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesReturn_Details");
            proc.AddVarcharPara("@Action", 500, "ComponentBillingAddress");
            proc.AddVarcharPara("@SelectedComponentList", 500, ComponentDetailsIDs);
            proc.AddVarcharPara("@ComponentType", 500, strType);
            ds = proc.GetTable();
            return ds;
        }
        public DataSet GetSalesReturnManualTaggingProductData(string strSalesReturnID, string LastFinYear, string LastCompany)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesReturn_Details");
            proc.AddVarcharPara("@Action", 50, "SalesReturnManualTaggingProductDetails");
            proc.AddVarcharPara("@SalesReturnID", 500, strSalesReturnID);
            proc.AddVarcharPara("@FinYear", 100, LastFinYear);
            proc.AddVarcharPara("@campany_Id", 100, LastCompany);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet GetSalesReturnManualProductData(string strSalesReturnID, string LastFinYear, string LastCompany)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesReturn_Details");
            // Mantis Issue 24831
            //proc.AddVarcharPara("@Action", 50, "SalesReturnManualProductDetails");
            proc.AddVarcharPara("@Action", 50, "SalesReturnManualProductDetails_New");
            // End of Mantis Issue 24831
            proc.AddVarcharPara("@SalesReturnID", 500, strSalesReturnID);
            proc.AddVarcharPara("@FinYear", 100, LastFinYear);
            proc.AddVarcharPara("@campany_Id", 100, LastCompany);
            ds = proc.GetDataSet();
            return ds;
        }

        protected void ComponentQuotation_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {

            string Reference = string.Empty;
            string Currency_Id = string.Empty;
            string SalesmanId = string.Empty;
            string ExpiryDate = string.Empty;
            string CurrencyRate = string.Empty;
            string Contact_person_id = string.Empty;
            string Tax_option = string.Empty;
            string Tax_Code = string.Empty;

            string InvoiceIds = string.Empty;
            string Customer = string.Empty;
            string SalesReturnDate = string.Empty;
            string ComponentType = string.Empty;
            string Action = string.Empty;
            string branchId = Convert.ToString(ddl_Branch.SelectedValue);
            if (e.Parameter.Split('~')[0] == "BindComponentGrid")
            {
                if (e.Parameter.Split('~')[1] != null) Customer = e.Parameter.Split('~')[1];
                if (e.Parameter.Split('~')[2] != null) SalesReturnDate = e.Parameter.Split('~')[2];

                string strReturnID = Convert.ToString(Session["SRM_ReturnID"]);

                if (e.Parameter.Split('~')[3] == "DateCheck")
                {
                    lookup_quotation.GridView.Selection.UnselectAll();


                    ComponentQuotationPanel.JSProperties["cpDetails"] = Reference + "~" + Currency_Id + "~" + SalesmanId + "~" + ExpiryDate + "~" + CurrencyRate + "~" + Contact_person_id + "~" + Tax_option + "~" + Tax_Code;
                }
                DataTable ComponentTable = objSalesReturnBL.GetComponent(Customer, SalesReturnDate, strReturnID, branchId);
                lookup_quotation.GridView.Selection.CancelSelection();
                lookup_quotation.DataSource = ComponentTable;
                lookup_quotation.DataBind();

                Session["SRM_ComponentData"] = ComponentTable;



            }


            else if (e.Parameter.Split('~')[0] == "BindComponentGridOnSelection")//Subhabrata for binding quotation
            {
                if (grid_Products.GetSelectedFieldValues("ComponentDetailsID").Count != 0)
                {
                    for (int i = 0; i < grid_Products.GetSelectedFieldValues("ComponentDetailsID").Count; i++)
                    {
                        InvoiceIds += "," + grid_Products.GetSelectedFieldValues("ComponentID")[i];
                    }
                    InvoiceIds = InvoiceIds.TrimStart(',');
                    lookup_quotation.GridView.Selection.UnselectAll();
                    if (!String.IsNullOrEmpty(InvoiceIds))
                    {
                        string[] eachQuo = InvoiceIds.Split(',');
                        if (eachQuo.Length > 1)//More tha one quotation
                        {
                            txt_InvoiceDate.Text = "Multiple Select Invoice Dates";

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

                string strType = "SI";


                DataTable dt = objSalesReturnBL.GetNecessaryData(InvoiceIds, strType);
                if (dt != null && dt.Rows.Count > 0)
                {
                    Reference = Convert.ToString(dt.Rows[0]["Reference"]);
                    Currency_Id = Convert.ToString(dt.Rows[0]["Currency_Id"]);
                    SalesmanId = Convert.ToString(dt.Rows[0]["SalesmanId"]);
                    ExpiryDate = Convert.ToString(dt.Rows[0]["ExpiryDate"]);
                    CurrencyRate = Convert.ToString(dt.Rows[0]["CurrencyRate"]);
                    Contact_person_id = Convert.ToString(dt.Rows[0]["Contact_Person_Id"]);
                    Tax_option = Convert.ToString(dt.Rows[0]["Tax_Option"]);
                    Tax_Code = Convert.ToString(dt.Rows[0]["Tax_Code"]);
                }
                ComponentQuotationPanel.JSProperties["cpDetails"] = Reference + "~" + Currency_Id + "~" + SalesmanId + "~" + ExpiryDate + "~" + CurrencyRate + "~" + Contact_person_id + "~" + Tax_option + "~" + Tax_Code;
            }
            else if (e.Parameter.Split('~')[0] == "RemoveComponentGridOnSelection")//Subhabrata for binding quotation
            {

                ComponentQuotationPanel.JSProperties["cpDetails"] = Reference + "~" + Currency_Id + "~" + SalesmanId + "~" + ExpiryDate + "~" + CurrencyRate + "~" + Contact_person_id + "~" + Tax_option + "~" + Tax_Code;
            }
            else if (e.Parameter.Split('~')[0] == "DateCheckOnChanged")//Subhabrata for binding quotation
            {

                if (grid_Products.GetSelectedFieldValues("Quotation_No").Count != 0)
                {

                    // DateTime SalesOrderDate = Convert.ToDateTime(e.Parameter.Split('~')[2]);
                    if (lookup_quotation.GridView.GetSelectedFieldValues("ComponentDate").Count() != 0)
                    {
                        //DateTime QuotationDate = DateTime.Parse(Convert.ToString(lookup_quotation.GridView.GetSelectedFieldValues("Quote_Date")[0]));
                        //if (SalesOrderDate < QuotationDate)
                        //{
                        lookup_quotation.GridView.Selection.UnselectAll();
                        lookup_quotation.DataSource = null;
                        lookup_quotation.DataBind();
                        //}
                    }


                    //Quote_Date

                }
            }

            else if (e.Parameter.Split('~')[0] == "BranchCheckOnChanged")
            {

                if (grid_Products.GetSelectedFieldValues("Quotation_No").Count != 0)
                {
                    if (lookup_quotation.GridView.GetSelectedFieldValues("ComponentDate").Count() != 0)
                    {
                        lookup_quotation.GridView.Selection.UnselectAll();
                        lookup_quotation.DataSource = null;
                        lookup_quotation.DataBind();
                    }

                }

            }
        }

        protected void lookup_quotation_DataBinding(object sender, EventArgs e)
        {
            if (Session["SRM_ComponentData"] != null)
            {
                lookup_quotation.DataSource = (DataTable)Session["SRM_ComponentData"];
            }
        }

        protected void ComponentDatePanel_Callback(object sender, CallbackEventArgsBase e)
        {
            string strSplitCommand = e.Parameter.Split('~')[0];
            if (strSplitCommand == "BindComponentDate")
            {
                string Invoice_No = Convert.ToString(e.Parameter.Split('~')[1]);

                DataTable dt_QuotationDetails = objSalesReturnBL.GetInvoiceDate(Invoice_No);
                if (dt_QuotationDetails != null && dt_QuotationDetails.Rows.Count > 0)
                {
                    string quotationdate = Convert.ToString(dt_QuotationDetails.Rows[0]["Invoice_Date"]);
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
                    DataTable dt_QuotationDetails = new DataTable();
                    string IdKey = Convert.ToString(Request.QueryString["key"]);
                    if (!string.IsNullOrEmpty(IdKey))
                    {
                        if (IdKey != "ADD")
                        {
                            dt_QuotationDetails = objSalesReturnBL.GetInvoiceDetailsOnly(QuoComponent, "Edit");
                            // dt_QuotationDetails = objPurchaseChallanBL.GetInvoiceDetailsOnly(QuoComponent, IdKey, "");
                        }
                        else
                        {
                            dt_QuotationDetails = objSalesReturnBL.GetInvoiceDetailsOnly(QuoComponent, "Add");
                        }

                    }
                    else
                    {
                        dt_QuotationDetails = objSalesReturnBL.GetInvoiceDetailsOnly(QuoComponent, "Add");
                    }
                    // Session["OrderDetails"] = null;

                    grid_Products.DataSource = GetProductsInfo(dt_QuotationDetails);
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
        public IEnumerable GetProductsInfo(DataTable SalesInvoicedt1)
        {
            List<SalesOrder> OrderList = new List<SalesOrder>();
            for (int i = 0; i < SalesInvoicedt1.Rows.Count; i++)
            {
                SalesOrder Orders = new SalesOrder();

                Orders.SrlNo = Convert.ToString(i + 1);
                // Orders.Key_UniqueId = Convert.ToString(i + 1);
                Orders.ComponentDetailsID = Convert.ToString(SalesInvoicedt1.Rows[i]["ComponentDetailsID"]);
                if (!string.IsNullOrEmpty(Convert.ToString(SalesInvoicedt1.Rows[i]["Quotation_No"])))
                { Orders.Quotation_No = Convert.ToInt64(SalesInvoicedt1.Rows[i]["Quotation_No"]); }
                else
                { Orders.Quotation_No = 0; }
                Orders.gvColProduct = Convert.ToString(SalesInvoicedt1.Rows[i]["QuoteDetails_ProductId"]);
                Orders.gvColDiscription = Convert.ToString(SalesInvoicedt1.Rows[i]["Description"]);
                Orders.gvColQuantity = Convert.ToString(SalesInvoicedt1.Rows[i]["QuoteDetails_Quantity"]);
                Orders.Quotation_Num = Convert.ToString(SalesInvoicedt1.Rows[i]["Quotation"]);
                Orders.Product_Shortname = Convert.ToString(SalesInvoicedt1.Rows[i]["Product_Name"]);
                Orders.ComponentID = Convert.ToString(SalesInvoicedt1.Rows[i]["ComponentID"]);
                OrderList.Add(Orders);
            }

            return OrderList;
        }
        public class SalesOrder
        {
            public string SrlNo { get; set; }
            public string OrderDetails_Id { get; set; }
            public string gvColProduct { get; set; }
            public string gvColDiscription { get; set; }
            public string gvColQuantity { get; set; }
            public string gvColUOM { get; set; }
            public string Warehouse { get; set; }
            public string gvColStockQty { get; set; }
            public string gvColStockUOM { get; set; }
            public string gvColStockPurchasePrice { get; set; }
            public string gvColDiscount { get; set; }
            public string gvColAmount { get; set; }
            public string gvColTaxAmount { get; set; }
            public string gvColTotalAmountINR { get; set; }
            public Int64 Quotation_No { get; set; }
            public string Quotation_Num { get; set; }
            public string ComponentDetailsID { get; set; }
            public string Product_Shortname { get; set; }
            public string ProductName { get; set; }
            public string ComponentID { get; set; }
            public string ComponentNumber { get; set; }
            public string TotalQty { get; set; }
            public string BalanceQty { get; set; }
            public string IsComponentProduct { get; set; }
            public string ProductDisID { get; set; }
            public string Product { get; set; }
        }
        public class MultiUOMPacking
        {
            public decimal packing_quantity { get; set; }
            public decimal sProduct_quantity { get; set; }

            public Int32 AltUOMId { get; set; }
        }
        public class SR
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
            public string gvColStockPurchasePrice { get; set; }
            public string Discount { get; set; }
            public string Amount { get; set; }
            public string TaxAmount { get; set; }
            public string TotalAmount { get; set; }

            public string gvColTotalAmountINR { get; set; }
            public Int64 Quotation_No { get; set; }
            public string Quotation_Num { get; set; }
            public string ComponentDetailsID { get; set; }
            public string Product_Shortname { get; set; }
            public string ProductName { get; set; }
            public string ComponentID { get; set; }
            public string ComponentNumber { get; set; }
            public string TotalQty { get; set; }
            public string BalanceQty { get; set; }
            public string IsComponentProduct { get; set; }
            public string ProductDisID { get; set; }
            public string Product { get; set; }
            public string DetailsId { get; set; }
        }
        public bool ImplementTaxonTagging(int id, int slno)
        {
            Boolean inUse = false;
            DataTable taxTable = (DataTable)Session["SRM_FinalTaxRecord"];
            ProcedureExecute proc = new ProcedureExecute("prc_taxReturnTable");
            proc.AddVarcharPara("@Module", 20, "SalesReturn");
            proc.AddIntegerPara("@id", id);
            proc.AddIntegerPara("@slno", slno);
            proc.AddBooleanPara("@inUse", true, QueryParameterDirection.Output);

            DataTable returnedTable = proc.GetTable();
            inUse = Convert.ToBoolean(proc.GetParaValue("@inUse"));

            if (returnedTable != null)
                taxTable.Merge(returnedTable);


            Session["SRM_FinalTaxRecord"] = taxTable;

            return inUse;
        }
        public IEnumerable GetInvoiceInfo(DataTable SalesInvoicedt1, string Order_Id)
        {
            List<SR> OrderList = new List<SR>();
            DataColumnCollection dtC = SalesInvoicedt1.Columns;
            CreateDataTaxTable();



            for (int i = 0; i < SalesInvoicedt1.Rows.Count; i++)
            {
                SR Orders = new SR();

                Orders.SrlNo = Convert.ToString(i + 1);
                Orders.QuotationID = Convert.ToString(i + 1);
                Orders.ProductID = Convert.ToString(SalesInvoicedt1.Rows[i]["ProductID"]);
                Orders.Description = Convert.ToString(SalesInvoicedt1.Rows[i]["Description"]);
                Orders.Quantity = Convert.ToString(SalesInvoicedt1.Rows[i]["Quantity"]);
                // Orders.Quantity = Convert.ToString(SalesInvoicedt1.Rows[i]["TotalQty"]);
                Orders.UOM = Convert.ToString(SalesInvoicedt1.Rows[i]["UOM"]);
                Orders.Warehouse = "";
                Orders.StockQuantity = Convert.ToString(SalesInvoicedt1.Rows[i]["StockQuantity"]);
                Orders.StockUOM = Convert.ToString(SalesInvoicedt1.Rows[i]["StockUOM"]);
                Orders.gvColStockPurchasePrice = "";
                Orders.Discount = Convert.ToString(SalesInvoicedt1.Rows[i]["Discount"]);
                Orders.Amount = Convert.ToString(SalesInvoicedt1.Rows[i]["Amount"]);
                Orders.TotalAmount = Convert.ToString(SalesInvoicedt1.Rows[i]["TotalAmount"]);
                Orders.TaxAmount = Convert.ToString(SalesInvoicedt1.Rows[i]["TaxAmount"]);
                Orders.gvColTotalAmountINR = "";
                Orders.SalePrice = Convert.ToString(SalesInvoicedt1.Rows[i]["SalePrice"]);

                Orders.ProductName = Convert.ToString(SalesInvoicedt1.Rows[i]["ProductName"]);


                //22-03-2017

                Orders.ComponentID = Convert.ToString(SalesInvoicedt1.Rows[i]["ComponentID"]);
                Orders.ComponentNumber = Convert.ToString(SalesInvoicedt1.Rows[i]["ComponentNumber"]);
                Orders.TotalQty = Convert.ToString(SalesInvoicedt1.Rows[i]["TotalQty"]);
                Orders.BalanceQty = Convert.ToString(SalesInvoicedt1.Rows[i]["BalanceQty"]);
                Orders.IsComponentProduct = Convert.ToString(SalesInvoicedt1.Rows[i]["IsComponentProduct"]);


                Orders.ProductDisID = Convert.ToString(SalesInvoicedt1.Rows[i]["ProductID"]);
                Orders.Product = Convert.ToString(SalesInvoicedt1.Rows[i]["ProductName"]);

                //22-03-2017

                // Mapping With Warehouse with Product Srl NoGetInvoiceWarehouse

                String QuoComponent = "";
                List<object> QuoList = lookup_quotation.GridView.GetSelectedFieldValues("ComponentID");
                foreach (object Quo in QuoList)
                {
                    QuoComponent += "," + Quo;
                }
                QuoComponent = QuoComponent.TrimStart(',');

                string[] eachInvoice = QuoComponent.Split(',');
                string strQuoteDetails_Id = Convert.ToString(SalesInvoicedt1.Rows[i]["QuotationID"]).Trim();

                if (ImplementTaxonTagging(Convert.ToInt32(strQuoteDetails_Id), Convert.ToInt32(Orders.SrlNo)))
                {
                    Orders.TaxAmount = "0.00";
                }

                // main tax integrate with respect to first tag  invoice
                if (eachInvoice.Length == 1)
                {
                    DataTable tempMainTax = objSalesReturnBL.GetTaxDetailsSI(QuoComponent);
                    if (tempMainTax != null && tempMainTax.Rows.Count > 0)
                    {
                        Session["SRM_TaxDetails"] = tempMainTax;

                    }

                }
                else { Session["SRM_TaxDetails"] = null; }
                //  main tax integrate with respect to first tag  invoice
                // warehouse

                string commaSeparatedString = String.Join(",", SalesInvoicedt1.AsEnumerable().Select(x => x.Field<Int64>("QuotationID").ToString()).ToArray());
                DataTable tempWarehouse = GetInvoiceWarehouse(commaSeparatedString);
                if (tempWarehouse != null && tempWarehouse.Rows.Count > 0)
                {
                    var rows = tempWarehouse.Select("InvoiceDetails_Id ='" + strQuoteDetails_Id + "'");
                    foreach (var row in rows)
                    {
                        row["Product_SrlNo"] = Convert.ToString(i + 1);
                    }
                    tempWarehouse.AcceptChanges();
                }

                if (tempWarehouse != null && tempWarehouse.Rows.Count > 0)
                {
                    tempWarehouse.Columns.Remove("InvoiceDetails_Id");
                    Session["SRM_WarehouseData"] = tempWarehouse;

                    // GrdWarehousePC.DataSource = tempWarehouse.DefaultView;
                    // GrdWarehousePC.DataBind();
                }
                else
                {
                    Session["SRM_WarehouseData"] = null;
                    GrdWarehousePC.DataSource = null;
                    GrdWarehousePC.DataBind();
                }
                //warehouse

                //if (QuoComponent =="")
                //{






                //    if (!string.IsNullOrEmpty(Convert.ToString(SalesInvoicedt1.Rows[i]["Quotation_No"])))//Added on 15-02-2017
                //    { Orders.Quotation_No = Convert.ToInt64(SalesInvoicedt1.Rows[i]["Quotation_No"]); }
                //    else
                //    { Orders.Quotation_No = 0; }
                //    if (dtC.Contains("Quotation"))
                //    {
                //        Orders.Quotation_Num = Convert.ToString(SalesInvoicedt1.Rows[i]["Quotation"]);//subhabrata on 21-02-2017
                //    }

                //}




                OrderList.Add(Orders);
            }


            BindSessionByDatatable(SalesInvoicedt1);
            return OrderList;
        }
        #region kaushik/SessionBind
        //
        public bool BindSessionByDatatable(DataTable dt)
        {
            bool IsSuccess = false;
            DataTable SlsOreturnDT = new DataTable();


            SlsOreturnDT.Columns.Add("SrlNo", typeof(string));
            SlsOreturnDT.Columns.Add("QuotationID", typeof(string));
            SlsOreturnDT.Columns.Add("ProductID", typeof(string));
            SlsOreturnDT.Columns.Add("Description", typeof(string));
            SlsOreturnDT.Columns.Add("Quantity", typeof(string));
            SlsOreturnDT.Columns.Add("UOM", typeof(string));
            SlsOreturnDT.Columns.Add("Warehouse", typeof(string));
            SlsOreturnDT.Columns.Add("StockQuantity", typeof(string));
            SlsOreturnDT.Columns.Add("StockUOM", typeof(string));
            SlsOreturnDT.Columns.Add("SalePrice", typeof(string));
            SlsOreturnDT.Columns.Add("Discount", typeof(string));
            SlsOreturnDT.Columns.Add("Amount", typeof(string));
            SlsOreturnDT.Columns.Add("TaxAmount", typeof(string));
            SlsOreturnDT.Columns.Add("TotalAmount", typeof(string));
            SlsOreturnDT.Columns.Add("Status", typeof(string));
            SlsOreturnDT.Columns.Add("ProductName", typeof(string));
            SlsOreturnDT.Columns.Add("ComponentID", typeof(string));
            SlsOreturnDT.Columns.Add("ComponentNumber", typeof(string));
            SlsOreturnDT.Columns.Add("TotalQty", typeof(string));
            SlsOreturnDT.Columns.Add("BalanceQty", typeof(string));
            SlsOreturnDT.Columns.Add("IsComponentProduct", typeof(string));
            SlsOreturnDT.Columns.Add("ProductDisID", typeof(string));
            SlsOreturnDT.Columns.Add("Product", typeof(string));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IsSuccess = true;
                //  DataColumnCollection dtC = dt.Columns;




                // Quotationdt.Rows.Add(SrlNo, QuotationID, ProductDetails, Description, Quantity, UOM, Warehouse, StockQuantity, StockUOM, SalePrice, Discount, Amount, TaxAmount, TotalAmount, "U", ProductName, ComponentID, ComponentNumber, TotalQty, BalanceQty, IsComponentProduct, ProductDisID, Product);
                SlsOreturnDT.Rows.Add(Convert.ToString(i + 1),
                    Convert.ToString(dt.Rows[i]["QuotationID"]),
                    Convert.ToString(dt.Rows[i]["ProductID"]),
                    Convert.ToString(dt.Rows[i]["Description"]),
                    Convert.ToString(dt.Rows[i]["Quantity"]),
                    Convert.ToString(dt.Rows[i]["UOM"]),
                    "", Convert.ToString(dt.Rows[i]["StockQuantity"]),
                    Convert.ToString(dt.Rows[i]["StockUOM"]),
                                Convert.ToString(dt.Rows[i]["SalePrice"]),
                               Convert.ToString(dt.Rows[i]["Discount"]),
                    Convert.ToString(dt.Rows[i]["Amount"]),
                    Convert.ToString(dt.Rows[i]["TaxAmount"]),
                    Convert.ToString(dt.Rows[i]["TotalAmount"]), "U",
                    Convert.ToString(dt.Rows[i]["ProductName"]),
                    Convert.ToString(dt.Rows[i]["ComponentID"]),
                  Convert.ToString(dt.Rows[i]["ComponentNumber"]),
                   Convert.ToString(dt.Rows[i]["TotalQty"]),
                   Convert.ToString(dt.Rows[i]["BalanceQty"]),
                    Convert.ToString(dt.Rows[i]["IsComponentProduct"]),
                    Convert.ToString(dt.Rows[i]["ProductID"]),
                       Convert.ToString(dt.Rows[i]["ProductName"])


                               );
            }

            Session["SRM_QuotationDetails"] = SlsOreturnDT;

            return IsSuccess;
        }//End
        #endregion
        public DataTable GetInvoiceWarehouse(string strInvoiceList)
        {
            try
            {
                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesReturn_Details");
                proc.AddVarcharPara("@Action", 500, "InvoiceWarehouseByQuotation");
                proc.AddVarcharPara("@InvoiceList", 3000, strInvoiceList);
                dt = proc.GetTable();

                string strNewVal = "", strOldVal = "";
                DataTable tempdt = dt.Copy();
                foreach (DataRow drr in tempdt.Rows)
                {
                    strNewVal = Convert.ToString(drr["InvoiceWarehouse_Id"]);

                    if (strNewVal == strOldVal)
                    {
                        drr["WarehouseName"] = "";
                        drr["viewWarehouseName"] = "";
                        drr["Quantity"] = "";
                        drr["viewQuantity"] = "";
                        drr["BatchNo"] = "";
                        drr["viewBatchNo"] = "";
                        drr["SalesUOMName"] = "";
                        drr["SalesQuantity"] = "";
                        drr["StkUOMName"] = "";
                        drr["StkQuantity"] = "";
                        drr["ConversionMultiplier"] = "";
                        drr["AvailableQty"] = "";
                        drr["BalancrStk"] = "";
                        drr["MfgDate"] = "";
                        drr["viewMFGDate"] = "";
                        drr["ExpiryDate"] = "";
                        drr["viewExpiryDate"] = "";
                    }

                    strOldVal = strNewVal;
                }

                Session["LoopSRWarehouse"] = Convert.ToString(Convert.ToInt32(strNewVal) + 1);
                tempdt.Columns.Remove("InvoiceWarehouse_Id");
                return tempdt;
            }
            catch
            {
                return null;
            }
        }
        protected void BindLookUp(string Customer, string SalesReturnDate, string branchId)
        {

            string strReturnID = Convert.ToString(Session["SRM_ReturnID"]);
            // string branchId = Convert.ToString(ddl_Branch.SelectedValue);
            DataTable ComponentTable = objSalesReturnBL.GetComponent(Customer, SalesReturnDate, strReturnID, branchId);
            lookup_quotation.GridView.Selection.CancelSelection();
            Session["SRM_ComponentData"] = ComponentTable;
            lookup_quotation.DataSource = ComponentTable;
            lookup_quotation.DataBind();





        }
        protected void acbpCrpUdf_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            acbpCrpUdf.JSProperties["cpUDF"] = "true";
            acbpCrpUdf.JSProperties["cpTransport"] = "true";

        }
        protected void productLookUp_ItemsRequestedByFilterCondition(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            if (e.Filter != "")
            {
                ASPxComboBox comboBox = (ASPxComboBox)source;
                DataTable dt = new DataTable();
                string filter = "%" + Convert.ToString(e.Filter) + "%";
                int startindex = Convert.ToInt32(e.BeginIndex + 1);
                int EndIndex = Convert.ToInt32(e.EndIndex + 1);
                string branchId = ddl_Branch.SelectedItem.Value;
                dt = objSalesReturnBL.PopulatePrOnDemand(filter, startindex, EndIndex, Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear1"]));
                productLookUp.DataSource = dt;
                productLookUp.DataBind();
            }
        }
        protected void productLookUp_ItemRequestedByValue(object source, ListEditItemRequestedByValueEventArgs e)
        {

        }
        protected void productDisLookUp_ItemsRequestedByFilterCondition(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            if (e.Filter != "")
            {
                ASPxComboBox comboBox = (ASPxComboBox)source;
                DataTable dt = new DataTable();
                string filter = "%" + Convert.ToString(e.Filter) + "%";
                int startindex = Convert.ToInt32(e.BeginIndex + 1);
                int EndIndex = Convert.ToInt32(e.EndIndex + 1);
                string branchId = ddl_Branch.SelectedItem.Value;
                dt = objSalesReturnBL.PopulatePrOnDemand(filter, startindex, EndIndex, Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear1"]));
                productDisLookUp.DataSource = dt;
                productDisLookUp.DataBind();
            }
        }
        protected void productDisLookUp_ItemRequestedByValue(object source, ListEditItemRequestedByValueEventArgs e)
        {

        }      

        [WebMethod]
        public static bool CheckBillingShippingExist(string CustomerID)
        {


            bool flag = false;

            DataTable CustomerStateCodeDT = new DataTable();

            try
            {
                ProcedureExecute procCustomerStateCode = new ProcedureExecute("prc_CRMSalesReturn_Details");
                procCustomerStateCode.AddVarcharPara("@Action", 500, "GetCustomerStateCode");
                procCustomerStateCode.AddVarcharPara("@CustomerID", 100, Convert.ToString(CustomerID));

                CustomerStateCodeDT = procCustomerStateCode.GetTable();
                if (CustomerStateCodeDT != null && CustomerStateCodeDT.Rows.Count > 0)
                {
                    flag = true;

                }
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
        public static object GetCustomerStateCodeDetails(string CustomerID)
        {
            string ShippingStateName = "", BillingStateCode = "";
            List<SateCodeList> SateCodeList = new List<SateCodeList>();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesReturn_Details");
            proc.AddVarcharPara("@Action", 500, "GetCustomerStateCodeForPlaceOfSupply");
            proc.AddVarcharPara("@CustomerID", 100, CustomerID);
            DataTable address = proc.GetTable();

            SateCodeList = (from DataRow dr in address.Rows
                            select new SateCodeList()
                            {
                                id = Convert.ToString(dr["id"]),
                                state = Convert.ToString(dr["state"]),
                                StateCode = Convert.ToString(dr["StateCode"]),
                                add_addressType = Convert.ToString(dr["add_addressType"]),
                                TransactionType = Convert.ToString(dr["TransactionType"])
                            }).ToList();

            return SateCodeList;

            //for (int i = 0; i < address.Rows.Count; i++)
            //{
            //    string addressType=Convert.ToString(address.Rows[i]["add_addressType"]);
            //    if(addressType=="Billing")
            //    {
            //        BillingStateCode = Convert.ToString(address.Rows[i]["state"]);
            //    }
            //    else if(addressType=="Shipping")
            //    {
            //        ShippingStateName = Convert.ToString(address.Rows[i]["state"]);
            //    }
            //}


            //return ShippingStateName + "~" + BillingStateCode;
        }
        public class ProjectDetails
        {
            public Int64 ProjectId { get; set; }
            public string ProjectCode { get; set; }
        }
        public class SateCodeList
        {
            public string id { get; set; }
            public string state { get; set; }
            public string StateCode { get; set; }
            public string add_addressType { get; set; }
            public string TransactionType { get; set; }

        }
        protected void CustStateCallBackPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            string WhichCall = e.Parameter.Split('~')[0];
            if (WhichCall == "GetCustomerStateCode")
            {

                string CustomerInternalId = Convert.ToString(Convert.ToString(e.Parameter.Split('~')[1]));
                DataTable CustomerStateCodeDT = new DataTable();

                ProcedureExecute procCustomerStateCode = new ProcedureExecute("prc_CRMSalesReturn_Details");
                procCustomerStateCode.AddVarcharPara("@Action", 500, "GetCustomerStateCode");
                procCustomerStateCode.AddVarcharPara("@CustomerID", 100, Convert.ToString(CustomerInternalId));

                CustomerStateCodeDT = procCustomerStateCode.GetTable();

                string CustomerStateCode = "";

                if (CustomerStateCodeDT != null && CustomerStateCodeDT.Rows.Count > 0)
                {
                    CustomerStateCode = Convert.ToString(CustomerStateCodeDT.Rows[0]["StateCode"]);

                    CustStateCallBackPanel.JSProperties["cpCustomerStateCode"] = CustomerStateCode;

                }

            }

        }        
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
        //Tanmoy Hierarchy
        [WebMethod]
        public static String getHierarchyID(string ProjID)
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            string Hierarchy_ID = "";
            DataTable dt2 = oDBEngine.GetDataTable("Select Hierarchy_ID from V_ProjectList where Proj_Code='" + ProjID + "'");
            if (dt2.Rows.Count > 0)
            {
                Hierarchy_ID = Convert.ToString(dt2.Rows[0]["Hierarchy_ID"]);
                return Hierarchy_ID;
            }
            else
            {
                Hierarchy_ID = "0";
                return Hierarchy_ID;
            }
        }       
        public void bindHierarchy()
        {
            DataTable hierarchydt = oDBEngine.GetDataTable("select 0 as ID ,'Select' as H_Name union select ID,H_Name from V_HIERARCHY");
            if (hierarchydt != null && hierarchydt.Rows.Count > 0)
            {
                ddlHierarchy.DataTextField = "H_Name";
                ddlHierarchy.DataValueField = "ID";
                ddlHierarchy.DataSource = hierarchydt;
                ddlHierarchy.DataBind();
            }
        }
        //End Tanmoy Hierarchy

        [WebMethod]
        public static String GetStockValuation(string ProductId)
        {

            CRMSalesOrderDtlBL objCRMSalesOrderDtlBL = new CRMSalesOrderDtlBL();
            DataTable dt = objCRMSalesOrderDtlBL.GetProductFifoValuation(Convert.ToInt32(ProductId));
            string Stock_Valuation = "";
            if (dt.Rows.Count > 0)
            {
                Stock_Valuation = Convert.ToString(dt.Rows[0]["Stockvaluation"]);
            }

            return Stock_Valuation;
        }
        [WebMethod]
        public static String GetStockValuationAmount(string Pro_Id, string Qty, string Valuationsign, string Fromdate, string BranchId)
        {

            CRMSalesOrderDtlBL objCRMSalesOrderDtlBL = new CRMSalesOrderDtlBL();
            DataTable dt = objCRMSalesOrderDtlBL.GetValueForProductFifoValuation(Convert.ToInt32(Pro_Id),
                                                Convert.ToDecimal(Qty), Valuationsign, Fromdate,
                                                Fromdate, BranchId);
            string ValuationAmount = "0";
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    ValuationAmount = Convert.ToString(dt.Rows[0]["VALUE"]);
                }
            }

            return ValuationAmount;
        }

        protected void grid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.CellValue != null)
            {
                string CloseRate = e.GetValue("CloseRate").ToString();
                string CloseRateFlag = e.GetValue("CloseRateFlag").ToString();
                if (CloseRate != CloseRateFlag)
                {
                    if (e.DataColumn.FieldName == "CloseRate")
                    {
                        //e.Cell.Font = System.Text.Bold;
                        //e.Cell.BackColor = System.Drawing.Color.Red;
                        e.Cell.ForeColor = System.Drawing.Color.Red;
                        e.Cell.Font.Bold = true;
                    }
                }

            }
        }
        protected void lookup_Project_DataBinding(object sender, EventArgs e)
        {
            if (Session["RM_ComponentData"] != null)
            {
                lookup_quotation.DataSource = (DataTable)Session["RM_ComponentData"];
            }
        }
        protected void ProjectCallBackPanel_Callback(object source, CallbackEventArgsBase e)
        {
            Int64 branchId=Convert.ToInt64(ddl_Branch.SelectedValue);
            string CustId= hdnCustomerId.Value;

            DataTable ComponentTable = ProjectList(branchId,CustId);
            
            lookup_Project.DataSource = ComponentTable;
            lookup_Project.DataBind();
            lookup_Project.GridView.DataBind();
            Session["RM_ComponentData"] = ComponentTable;
        }
        public DataTable ProjectList(Int64  branchId, string CustId)
        {
            DataTable dt=new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_ProjectList");
            proc.AddVarcharPara("@CustomerId", 100, CustId);
            proc.AddBigIntegerPara("@BranchId",branchId);
            dt=proc.GetTable();

            return dt;
        }


        [WebMethod(EnableSession = true)]
        public static object GetCustomerStateCode(string Key)
        {
            string CustomerInternalId = Convert.ToString(Key);
            DataTable CustomerStateCodeDT = new DataTable();

            ProcedureExecute procCustomerStateCode = new ProcedureExecute("prc_CRMSalesReturn_Details");
            procCustomerStateCode.AddVarcharPara("@Action", 500, "GetCustomerStateCode");
            procCustomerStateCode.AddVarcharPara("@CustomerID", 100, Convert.ToString(CustomerInternalId));

            CustomerStateCodeDT = procCustomerStateCode.GetTable();

            string CustomerStateCode = "";

            if (CustomerStateCodeDT != null && CustomerStateCodeDT.Rows.Count > 0)
            {
                CustomerStateCode = Convert.ToString(CustomerStateCodeDT.Rows[0]["StateCode"]);

               // cmbContactPerson.JSProperties["cpCustomerStateCode"] = CustomerStateCode;

            }
            return CustomerStateCode;
        }


        [WebMethod(EnableSession = true)]
        public static object GetContactPersonafterBillingShipping(string Key)
        {

            DataTable dtContactPerson = new DataTable();
            SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
            List<ContactPerson> contactPerson = new List<ContactPerson>();
            dtContactPerson = objSlaesActivitiesBL.PopulateContactPersonOfCustomer(Key);
            if (dtContactPerson != null && dtContactPerson.Rows.Count > 0)
            {

                if (dtContactPerson != null && dtContactPerson.Rows.Count > 0)
                {
                    for (int i = 0; i < dtContactPerson.Rows.Count; i++)
                    {
                        contactPerson.Add(new ContactPerson
                        {
                            Id = Convert.ToInt32(dtContactPerson.Rows[i]["add_id"]),
                            Name = Convert.ToString(dtContactPerson.Rows[i]["contactperson"])
                        });
                    }
                }
            }
            return contactPerson;
        }

        [WebMethod(EnableSession = true)]
        public static string GetContactPersonRelatedData(string Key, string branchid)
        {


            SalesReturnBL objSalesReturnBL = new SalesReturnBL();
            string InternalId = Convert.ToString(Key);
            string BranchId = Convert.ToString(branchid);
           
            string  cpOutstanding="0.0";
            string cpGSTN = "";
            DataTable OutStandingTable = objSalesReturnBL.GetCustomerOutStanding(Key);
            if (OutStandingTable.Rows.Count > 0)
            {

                var convertDecimal = Convert.ToDecimal(Convert.ToString(OutStandingTable.Rows[0]["NetOutstanding"]));
                if (convertDecimal > 0)
                {
                     cpOutstanding = Convert.ToString(convertDecimal) + "(Db)";
                }
                else
                {

                     cpOutstanding = Convert.ToString(convertDecimal * -1).ToString() + "(Cr)";
                }


            }
            else
            {
                cpOutstanding = "0.0";
            }
            DataTable GSTNTable = objSalesReturnBL.GetCustomerNewGSTIN(InternalId, BranchId);


            if (GSTNTable == null || GSTNTable.Rows.Count == 0)
            { cpGSTN = "No"; }
            else
            {

                string strGSTN = Convert.ToString(GSTNTable.Rows[0]["CNT_GSTIN"]).Trim();
                if (strGSTN != "")
                {
                    cpGSTN = "Yes";
                }
                else
                {
                    cpGSTN = "No";
                }
            }
            string s=cpOutstanding + '@' + cpGSTN;

            return s;
        }
        [WebMethod(EnableSession = true)]
        public static object taxUpdatePanel_Callback(string Action, string srl, string prodid)
        {
            string output = "200";
            try
            {
                if (Action == "DelProdbySl")
                {
                    DataTable MainTaxDataTable = (DataTable)HttpContext.Current.Session["SRM_FinalTaxRecord"];

                    DataRow[] deletedRow = MainTaxDataTable.Select("SlNo=" + srl);
                    if (deletedRow.Length > 0)
                    {
                        foreach (DataRow dr in deletedRow)
                        {
                            MainTaxDataTable.Rows.Remove(dr);
                        }

                    }

                    HttpContext.Current.Session["SRM_FinalTaxRecord"] = MainTaxDataTable;
                    //GetStock(Convert.ToString(srl));
                    // DeleteWarehouse(Convert.ToString(performpara.Split('~')[1]));
                    DataTable taxDetails = (DataTable)HttpContext.Current.Session["SRM_TaxDetails"];
                    if (taxDetails != null)
                    {
                        foreach (DataRow dr in taxDetails.Rows)
                        {
                            dr["Amount"] = "0.00";
                        }
                        HttpContext.Current.Session["SRM_TaxDetails"] = taxDetails;
                    }
                }
                else if (Action == "DeleteAllTax")
                {
                    CreateDataTaxTableByajax();

                    DataTable taxDetails = (DataTable)HttpContext.Current.Session["SI_TaxDetails"];

                    if (taxDetails != null)
                    {
                        foreach (DataRow dr in taxDetails.Rows)
                        {
                            dr["Amount"] = "0.00";
                        }
                        HttpContext.Current.Session["SI_TaxDetails"] = taxDetails;
                    }
                }
                else
                {
                    DataTable MainTaxDataTable = (DataTable)HttpContext.Current.Session["SRM_FinalTaxRecord"];

                    DataRow[] deletedRow = MainTaxDataTable.Select("SlNo=" + srl);
                    if (deletedRow.Length > 0)
                    {
                        foreach (DataRow dr in deletedRow)
                        {
                            MainTaxDataTable.Rows.Remove(dr);
                        }

                    }

                    HttpContext.Current.Session["SRM_FinalTaxRecord"] = MainTaxDataTable;

                    DataTable taxDetails = (DataTable)HttpContext.Current.Session["SRM_TaxDetails"];

                    if (taxDetails != null)
                    {
                        foreach (DataRow dr in taxDetails.Rows)
                        {
                            dr["Amount"] = "0.00";
                        }
                        HttpContext.Current.Session["SRM_TaxDetails"] = taxDetails;
                    }
                }
            }
            catch
            {
                output = "201";

            }


            return output;

        }

        public class ContactPersonWithAnotherDetails
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public string InternalID { get; set; }
           

        }

      


    }

}