#region//====================================================Revision History=========================================================================
// 1.0   Priti     V2.0.37    02-03-2023      0025706: Mfg Date & Exp date & Alt Qty is not showing in modify mode of Sales return
// 2.0   Sanchita  V2.0.39    14-07-2023      Multi UOM EVAC Issues status modulewise - Sales Return. Mantis : 26524
#endregion//====================================================End Revision History=====================================================================


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
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using System.Web.Hosting;
using System.IO;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json.Linq;

namespace ERP.OMS.Management.Activities
{
    public partial class SalesReturn : ERP.OMS.ViewState_class.VSPage// System.Web.UI.Page
    {
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        GSTtaxDetails gstTaxDetails = new GSTtaxDetails();
        clsDropDownList oclsDropDownList = new clsDropDownList();
        BusinessLogicLayer.Contact oContactGeneralBL = new BusinessLogicLayer.Contact();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        static string ForJournalDate = null;
        SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
        SalesReturnBL objSalesReturnBL = new SalesReturnBL();
        CRMSalesDtlBL objCRMSalesDtlBL = new CRMSalesDtlBL();
        PurchaseOrderBL objPurchaseOrderBL = new PurchaseOrderBL();
        ERPDocPendingApprovalBL objERPDocPendingApproval = new ERPDocPendingApprovalBL();
        string UniqueQuotation = string.Empty;
        public string pageAccess = "";
        string userbranch = "";
        BusinessLogicLayer.RemarkCategoryBL reCat = new BusinessLogicLayer.RemarkCategoryBL();
        DataTable Remarks = new DataTable();
        #region Code Checked and Modified
        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {
            if (Request.QueryString.AllKeys.Contains("status") || Request.QueryString.AllKeys.Contains("IsTagged"))
            {
                this.Page.MasterPageFile = "~/OMS/MasterPage/PopUp.Master";

            }
            else
            {
                this.Page.MasterPageFile = "~/OMS/MasterPage/ERP.Master";

            }
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);


                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {

            CountrySelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            StateSelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SelectCity.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SelectArea.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SelectPin.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            sqltaxDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            dsCustomer.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            UomSelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            AltUomSelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }

        #endregion Code Checked and Modified  By Sam on 17022016

        public void IsExistsDocumentInERPDocApproveStatus(string strQuotationId)
        {
            string editable = "";
            string status = "";
            DataTable dt = new DataTable();
            int quoteid = Convert.ToInt32(strQuotationId);
            dt = objERPDocPendingApproval.IsExistsDocumentInERPDocApproveStatus(quoteid, 1);
            if (dt.Rows.Count > 0)
            {
                editable = Convert.ToString(dt.Rows[0]["editable"]);
                if (editable == "0")
                {
                    lbl_quotestatusmsg.Visible = true;
                    status = Convert.ToString(dt.Rows[0]["Status"]);
                    if (status == "Approved")
                    {
                        lbl_quotestatusmsg.Text = "Document already Approved";
                    }
                    if (status == "Rejected")
                    {
                        lbl_quotestatusmsg.Text = "Document already Rejected";
                    }
                    btn_SaveRecords.Visible = false;
                    ASPxButton1.Visible = false;
                }
                else
                {
                    lbl_quotestatusmsg.Visible = false;
                    btn_SaveRecords.Visible = true;
                    ASPxButton1.Visible = true;
                }
            }
        }
        public void GetEditablePermission()
        {
            if (Request.QueryString["Permission"] != null)
            {
                if (Convert.ToString(Request.QueryString["Permission"]) == "1")
                {

                    btn_SaveRecords.Visible = false;
                    ASPxButton1.Visible = false;
                }
                else if (Convert.ToString(Request.QueryString["Permission"]) == "2")
                {

                    btn_SaveRecords.Visible = true;
                    ASPxButton1.Visible = true;
                }
                else if (Convert.ToString(Request.QueryString["Permission"]) == "3")
                {

                    btn_SaveRecords.Visible = true;
                    ASPxButton1.Visible = true;
                }
            }
        }

        protected int getUdfCount()
        {
            DataTable udfCount = oDBEngine.GetDataTable("select 1 from tbl_master_remarksCategory rc where cat_applicablefor='SR'  and ( exists (select * from tbl_master_udfGroup where id=rc.cat_group_id and grp_isVisible=1) or rc.cat_group_id=0)");
            return udfCount.Rows.Count;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/SalesReturn.aspx");
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
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
                    grid.Columns[11].Width = 0;

                    // Mantis Issue 24429
                    grid.Columns[12].Width = 0;
                    grid.Columns[13].Width = 0;
                    // End of Mantis Issue 24429

                }
            }



            if (!IsPostBack)
            {

                
                string BackDatedEntryPurchaseReturnManual = ComBL.GetSystemSettingsResult("BackDatedEntrySalesReturn");
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

                Session["SR_ProductDetails"] = null;
                this.Session["LastCompanySR"] = Session["LastCompany"];
                this.Session["LastFinYearSR"] = Session["LastFinYear"];
                ddl_numberingScheme.Focus();
                #region Sandip Section For Checking User Level for Allow Edit to logging User or Not
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
                    ApprovalCross.Visible = true;
                    ddl_Branch.Enabled = false;
                }
                else
                {
                    divcross.Visible = true;
                    btn_SaveRecords.Visible = true;
                    ApprovalCross.Visible = false;
                    ddl_Branch.Enabled = false; // Change Due to Numbering Schema
                }
                dt_PlQuoteExpiry.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
                string finyear = Convert.ToString(Session["LastFinYear"]);
                IsUdfpresent.Value = Convert.ToString(getUdfCount());
                hdnAddressDtl.Value = "0";
                #endregion Sandip Section

                #region Session Null Initialization Start
                Session["SR_BillingAddressLookup"] = null;
                Session["SR_ShippingAddressLookup"] = null;
                #endregion Session Null Initialization End

                //Tanmoy Hierarchy
                bindHierarchy();
                ddlHierarchy.Enabled = false;
                //Tanmoy Hierarchy End

                //Purpose : Binding Batch Edit Grid
                //Name : Sudip 
                // Dated : 21-01-2017

                Session["SR_ReturnID"] = "";
                Session["SR_CustomerDetail"] = null;
                Session["SR_QuotationDetails"] = null;
                Session["SR_WarehouseData"] = null;
                Session["SR_QuotationTaxDetails"] = null;
                Session["LoopSRWarehouse"] = 1;
                Session["SR_TaxDetails"] = null;
                Session["SR_ActionType"] = "";
                Session["SR_ComponentData"] = null;
                Session["TaggingInvoce"] = "";
                Session["MultiUOMData"] = null;
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

                Session["SR_QuotationAddressDtl"] = null;

                if (Request.QueryString["key"] != null)
                {
                    if (Convert.ToString(Request.QueryString["key"]) != "ADD")
                    {
                        lookup_Project.ClientEnabled = false;
                        dt_PLQuote.ClientEnabled = false;
                        lblHeadTitle.Text = "Modify Sales Return";
                        hdnPageStatus.Value = "update";
                        divScheme.Style.Add("display", "none");
                        strSalesReturnId = Convert.ToString(Request.QueryString["key"]);
                        Session["SR_KeyVal_InternalID"] = "SRQUOTE" + strSalesReturnId;

                        rdl_SalesInvoice.Enabled = false;
                        rdl_SalesInvoice.SelectedValue = "SIN";
                        Keyval_internalId.Value = "SaleReturn" + strSalesReturnId;

                        #region Subhra Section
                        Session["SR_key_QutId"] = strSalesReturnId;

                        #endregion End Section

                        #region Product, Quotation Tax, Warehouse

                        Session["SR_ReturnID"] = strSalesReturnId;
                        Session["SR_ActionType"] = "Edit";
                        SetSalesReturnDetails(strSalesReturnId);
                        Session["SR_TaxDetails"] = GetTaxDataWithGST(GetTaxData(dt_PLQuote.Date.ToString("yyyy-MM-dd")));

                        DataTable Productdt = new DataTable();
                        if (Session["TaggingInvoce"] == "1")
                        {
                            // Rev Sanchita
                            //DataSet dtProduct = objSalesReturnBL.GetSalesReturnTaggingProductData(strSalesReturnId, Convert.ToString(Session["LastFinYear"]), Convert.ToString(Session["LastCompany"]));
                            DataSet dtProduct = objSalesReturnBL.GetSalesReturnTaggingProductData_New(strSalesReturnId, Convert.ToString(Session["LastFinYear"]), Convert.ToString(Session["LastCompany"]));
                            // End of Rev Sanchita
                            Productdt = dtProduct.Tables[0];
                            Session["InlineRemarks"] = dtProduct.Tables[1];
                        }
                        else
                        {
                            // Rev Sanchita
                            //DataSet dtProduct = objSalesReturnBL.GetSalesReturnProductData(strSalesReturnId, Convert.ToString(Session["LastFinYear"]), Convert.ToString(Session["LastCompany"]));
                            DataSet dtProduct = objSalesReturnBL.GetSalesReturnProductData_New(strSalesReturnId, Convert.ToString(Session["LastFinYear"]), Convert.ToString(Session["LastCompany"]));
                            // End of Rev Sanchita
                            Productdt = dtProduct.Tables[0];
                            Session["InlineRemarks"] = dtProduct.Tables[1];
                        }
                        //Rev Rajdip For Running Total
                        DataTable Orderdt = Productdt.Copy();
                        decimal TotalQty = 0;
                        decimal TotalAmt = 0;
                        decimal TaxAmount = 0;
                        decimal Amount = 0;
                        decimal SalePrice = 0;
                        decimal AmountWithTaxValue = 0;
                        decimal othercharges = 0;
                        decimal netamount = 0;
                        for (int i = 0; i < Orderdt.Rows.Count; i++)
                        {
                            TotalQty = TotalQty + Convert.ToDecimal(Orderdt.Rows[i]["TotalQty"]);
                            Amount = Amount + Convert.ToDecimal(Orderdt.Rows[i]["Amount"]);
                            TaxAmount = TaxAmount + Convert.ToDecimal(Orderdt.Rows[i]["TaxAmount"]);
                            SalePrice = SalePrice + Convert.ToDecimal(Orderdt.Rows[i]["SalePrice"]);
                            TotalAmt = TotalAmt + Convert.ToDecimal(Orderdt.Rows[i]["TotalAmount"]);

                        }
                        othercharges = Convert.ToDecimal(bnrOtherChargesvalue.Text.ToString());
                        AmountWithTaxValue = TaxAmount + Amount;

                        ASPxLabel12.Text = TotalQty.ToString();
                        bnrLblTaxableAmtval.Text = Amount.ToString();
                        bnrLblTaxAmtval.Text = TaxAmount.ToString();
                        bnrlblAmountWithTaxValue.Text = AmountWithTaxValue.ToString();
                        netamount = TotalAmt + othercharges;
                        bnrLblInvValue.Text = netamount.ToString();

                        //grid.JSProperties["cpRunningTotal"] = TotalQty + "~" + Amount + "~" + TaxAmount + "~" + AmountWithTaxValue + "~" + TotalAmt;
                        //end rev rajdip
                        Session["SR_QuotationDetails"] = Productdt;
                        Session["MultiUOMData"] = GetMultiUOMData();
                        grid.DataSource = GetQuotation(Productdt);
                        grid.DataBind();
                        DataTable dt = new DataTable();
                        dt = GetQuotationWarehouseData();
                        if (dt == null || dt.Rows.Count == 0)
                        {
                            Session["SR_WarehouseData"] = null;
                        }
                        else
                        {
                            Session["SR_WarehouseData"] = dt;
                        }
                        Session["SR_QuotationAddressDtl"] = objSalesReturnBL.GetSalesReturnBillingAddress(strSalesReturnId); ;

                        #endregion

                        #region Debjyoti Get Tax Details in Edit Mode

                        if (GetQuotationTaxData().Tables[0] != null)
                            Session["SR_QuotationTaxDetails"] = GetQuotationTaxData().Tables[0];

                        if (GetQuotationEditedTaxData().Tables[0] != null)
                        {
                            DataTable quotetable = GetQuotationEditedTaxData().Tables[0];
                            if (quotetable == null)
                            {
                                CreateDataTaxTable();
                            }
                            else
                            {
                                Session["SR_FinalTaxRecord"] = quotetable;
                            }
                        }
                        #endregion Debjyoti Get Tax Details in Edit Mode

                        #region Samrat Roy -- Hide Save Button in Edit Mode
                        if (Request.QueryString["req"] != null && Request.QueryString["req"] == "V")
                        {
                            lblHeadTitle.Text = "View Sales Return";
                            lbl_quotestatusmsg.Text = "*** View Mode Only";
                            btn_SaveRecords.Visible = false;
                            ASPxButton1.Visible = false;
                            lbl_quotestatusmsg.Visible = true;
                        }
                        #endregion

                        ddl_AmountAre.ClientEnabled = false;
                    }
                    else
                    {
                        #region To Show By Default Cursor after SAVE AND NEW
                        if (Session["SR_SaveMode"] != null)  // it has been removed from coding side of Quotation list 
                        {
                            if (Session["SR_schemavalue"] != null)  // it has been removed from coding side of Quotation list 
                            {
                                ddl_numberingScheme.SelectedValue = Convert.ToString(Session["SR_schemavalue"]); // it has been removed from coding side of Quotation list 
                                ddl_Branch.SelectedValue = Convert.ToString(Session["SR_schemavalue"]).Split('~')[3];
                            }
                            if (Convert.ToString(Session["SR_SaveMode"]) == "A")
                            {
                                dt_PLQuote.Focus();
                                txt_PLQuoteNo.Enabled = false;
                                txt_PLQuoteNo.Text = "Auto";
                            }
                            else if (Convert.ToString(Session["SR_SaveMode"]) == "M")
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

                        DataTable dtposTime = oDBEngine.GetDataTable("SELECT  top 1 convert(varchar(50),Lock_Fromdate,110) LockCon_Fromdate,convert(varchar(50),Lock_Todate,110) LockCon_Todate,convert(varchar(10),Lock_Fromdate,105) DataFreeze_Fromdate,convert(varchar(10),Lock_Todate,105) DataFreeze_Todate FROM Trans_LockConfigouration_Details WHERE  Type='Add' and Module_Id=12");

                        if (dtposTime != null && dtposTime.Rows.Count > 0)
                        {
                            hdnLockFromDate.Value = Convert.ToString(dtposTime.Rows[0]["LockCon_Fromdate"]);
                            hdnLockToDate.Value = Convert.ToString(dtposTime.Rows[0]["LockCon_Todate"]);
                            hdnLockFromDateCon.Value = Convert.ToString(dtposTime.Rows[0]["DataFreeze_Fromdate"]);
                            hdnLockToDateCon.Value = Convert.ToString(dtposTime.Rows[0]["DataFreeze_Todate"]);
                        }

                        Keyval_internalId.Value = "Add";
                        Session["SR_ActionType"] = "Add";
                        Session["SR_TaxDetails"] = null;
                        CreateDataTaxTable();
                        lblHeadTitle.Text = "Add Sales Return";
                        ddl_AmountAre.Value = "1";
                        ddl_VatGstCst.SelectedIndex = 0;
                        ddl_VatGstCst.ClientEnabled = false;
                        ddl_AmountAre.ClientEnabled = false;

                    }
                }


                if (Request.QueryString.AllKeys.Contains("IsTagged"))
                {
                    ApprovalCross.Visible = false;
                    divcross.Visible = false;


                }
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "GridCallBack()", true);
                GetSalesReturnSchemaLength();


                MasterSettings objmaster = new MasterSettings(); // Surojit 12-03-2019
                hdnConvertionOverideVisible.Value = objmaster.GetSettings("ConvertionOverideVisible");// Surojit 12-03-2019
                hdnShowUOMConversionInEntry.Value = objmaster.GetSettings("ShowUOMConversionInEntry");// Surojit 12-03-2019


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

            }

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

                txtCustName.Value = Customer_Id;
                hdnCustomerId.Value = Customer_Id;

                hdfLookupCustomer.Value = Customer_Id;
                TabPage page = ASPxPageControl1.TabPages.FindByName("[B]illing/Shipping");
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
                ddl_AmountAre.ClientEnabled = false;
                ddl_VatGstCst.ClientEnabled = false;

                dt_PLQuote.ClientEnabled = false;

                txtCustName.ClientEnabled = false;

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

        public class MultiUOMPacking
        {
            public decimal packing_quantity { get; set; }
            public decimal sProduct_quantity { get; set; }

            public Int32 AltUOMId { get; set; }
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
            public string IsInventory { get; set; }
            public string DetailsId { get; set; }
            public string CloseRate { get; set; }
            public string CloseRateFlag { get; set; }
            public string Remarks {get;set; }
            // Rev  Mantis Issue 24428
            public string Order_AltQuantity { get; set; }
            public string Order_AltUOM { get; set; }
            // End of Rev  Mantis Issue 24428
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
            for (int i = 0; i < DT.Rows.Count; i++)
            {
                Product Products = new Product();
                Products.ProductID = Convert.ToString(DT.Rows[i]["Products_ID"]);
                Products.ProductName = Convert.ToString(DT.Rows[i]["Products_Name"]);
                ProductList.Add(Products);
            }

            return ProductList;
        }
        public DataTable GetMultiUOMData()
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesReturn_Details");
            // Rev Sanchita
            //proc.AddVarcharPara("@Action", 500, "MultiUOMQuotationDetails");
            proc.AddVarcharPara("@Action", 500, "MultiUOMQuotationDetails_New");
            // End of Rev Sanchita
            proc.AddVarcharPara("@SalesReturnID", 500, Convert.ToString(Session["SR_ReturnID"]));
            ds = proc.GetTable();
            return ds;
        }
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

        #endregion Code Checked and Modified  By Sam on 17022016


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
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesReturn_Details");
            proc.AddVarcharPara("@Action", 500, "GetBatchByProductIDWarehouse");
            proc.AddVarcharPara("@ProdID", 500, Convert.ToString(hdfProductID.Value));
            proc.AddVarcharPara("@WarehouseID", 500, WarehouseID);
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            proc.AddVarcharPara("@branchId", 500, Convert.ToString(ddl_Branch.SelectedValue));
            proc.AddVarcharPara("@companyId", 500, Convert.ToString(Session["LastCompany"]));
            proc.AddVarcharPara("@Component_ID", 500, Convert.ToString(hdfComponentID.Value));
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetProjectCode(string Proj_Code)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesReturn_Details");
            proc.AddVarcharPara("@Action", 500, "GetProjectCode");
            proc.AddVarcharPara("@Proj_Code", 200, Proj_Code);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetProjectEditData()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesReturn_Details");
            proc.AddVarcharPara("@Action", 500, "ProjectEditdata");
            proc.AddVarcharPara("@SalesReturnID", 500, Convert.ToString(Session["SR_ReturnID"]));
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetSerialata(string WarehouseID, string BatchID, string BranchSerialNumber)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesReturn_Details");
            proc.AddVarcharPara("@Action", 500, "GetSerialByProductIDSRWarehouseBatch");
            proc.AddVarcharPara("@ProdID", 500, Convert.ToString(hdfProductID.Value));
            proc.AddVarcharPara("@BatchID", 500, BatchID);
            proc.AddVarcharPara("@WarehouseID", 500, WarehouseID);
            proc.AddVarcharPara("@Stock_BranchSerialNumber", 50, BranchSerialNumber);
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            proc.AddVarcharPara("@branchId", 500, Convert.ToString(ddl_Branch.SelectedValue));
            proc.AddVarcharPara("@companyId", 500, Convert.ToString(Session["LastCompany"]));
            proc.AddVarcharPara("@Component_ID", 500, Convert.ToString(hdfComponentID.Value));
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
            DataColumnCollection dtc=Quotationdt.Columns;
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
                    Quotations.IsInventory = Convert.ToString(Quotationdt.Rows[i]["IsInventory"]);
                    Quotations.IsInventory = Convert.ToString(Quotationdt.Rows[i]["IsInventory"]);
                    Quotations.DetailsId = Convert.ToString(Quotationdt.Rows[i]["DetailsId"]);
                    Quotations.CloseRate = Convert.ToString(Quotationdt.Rows[i]["CloseRate"]);
                    Quotations.CloseRateFlag = Convert.ToString(Quotationdt.Rows[i]["CloseRateFlag"]);
                    if (dtc.Contains("Remarks"))
                    {
                        Quotations.Remarks = Convert.ToString(Quotationdt.Rows[i]["Remarks"]);
                    }

                    // Rev  Mantis Issue 24428
                     Quotations.Order_AltQuantity = Convert.ToString(Quotationdt.Rows[i]["Order_AltQuantity"]);
                     Quotations.Order_AltUOM = Convert.ToString(Quotationdt.Rows[i]["Order_AltUOM"]);
                    // End of Rev  Mantis Issue 24428

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
            proc.AddVarcharPara("@QuotationID", 500, Convert.ToString(Session["SR_ReturnID"]));
            ds = proc.GetDataSet();
            return ds;
        }
        protected void grid_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            CommonBL ComBL = new CommonBL();

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
                    grid.Columns[11].Width = 0;

                    // Mantis Issue 24429
                    grid.Columns[12].Width = 0;
                    grid.Columns[13].Width = 0;
                    // End of Mantis Issue 24429

                }
            }
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

            else if (e.Column.FieldName == "SalePrice")
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
            else if (e.Column.FieldName == "Product")
            {
                e.Editor.Enabled = true;
            }
                // Rev Bapi
            //else if (e.Column.FieldName == "Quantity")
            //{
            //    e.Editor.Enabled = true;
            //}
                // End of Rev Bapi
            else if (e.Column.FieldName == "Discount")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "CloseRate")
            {
                e.Editor.Enabled = true;
            }
                // Rev Bapi
            else if (e.Column.FieldName == "Order_AltQuantity")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "Order_AltUOM")
            {
                e.Editor.Enabled = true;
            }
            else if (hddnMultiUOMSelection.Value == "0" && e.Column.FieldName == "Quantity")
            {
                e.Editor.Enabled = true;
            }
                // End of Rev Bapi
            else
            {
                e.Editor.ReadOnly = true;
            }
        }
        protected void grid_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            DataTable Quotationdt = new DataTable();
            string IsDeleteFrom = Convert.ToString(hdfIsDelete.Value);

            string validate = "", SchemaID = "";
            grid.JSProperties["cpQuotationNo"] = "";
            grid.JSProperties["cpSaveSuccessOrFail"] = "";

            if (Session["SR_QuotationDetails"] != null)
            {
                Quotationdt = (DataTable)Session["SR_QuotationDetails"];
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
                Quotationdt.Columns.Add("IsInventory", typeof(string));
                Quotationdt.Columns.Add("DetailsId", typeof(string));
                Quotationdt.Columns.Add("CloseRate", typeof(string));
                Quotationdt.Columns.Add("CloseRateFlag", typeof(string));
                Quotationdt.Columns.Add("Remarks", typeof(string));

                // Rev  Mantis Issue 24428
                Quotationdt.Columns.Add("Order_AltQuantity", typeof(string));
                Quotationdt.Columns.Add("Order_AltUOM", typeof(string));
                // End of Rev  Mantis Issue 24428

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


                    // Rev  Mantis Issue 24428
                    string Order_AltQuantity = Convert.ToString(args.NewValues["Order_AltQuantity"]);
                    string Order_AltUOM = Convert.ToString(args.NewValues["Order_AltUOM"]);
                    // End of Rev  Mantis Issue 24428
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
                    string IsInventory = (Convert.ToString(args.NewValues["IsInventory"]) != "") ? Convert.ToString(args.NewValues["IsInventory"]) : "0";
                    string DetailsId = Convert.ToString(args.NewValues["DetailsId"]);
                    string CloseRate = Convert.ToString(args.NewValues["CloseRate"]);
                    string CloseRateFlag = Convert.ToString(args.NewValues["CloseRateFlag"]);
                    string Remarks = (Convert.ToString(args.NewValues["Remarks"]) != "") ? Convert.ToString(args.NewValues["Remarks"]) : "";
                    // Rev  Mantis Issue 24428
                  //  Quotationdt.Rows.Add(SrlNo, "0", ProductDetails, Description, Quantity, UOM, Warehouse, StockQuantity, StockUOM, SalePrice, Discount, Amount, TaxAmount, TotalAmount, "I", ProductName, ComponentID, ComponentNumber, TotalQty, BalanceQty, IsComponentProduct, ProductDisID, Product, IsInventory, DetailsId, CloseRate, CloseRateFlag, Remarks);
                    Quotationdt.Rows.Add(SrlNo, "0", ProductDetails, Description, Quantity, UOM, Warehouse, StockQuantity, StockUOM, SalePrice, Discount, Amount, TaxAmount, TotalAmount, "I", ProductName, ComponentID, ComponentNumber, TotalQty, BalanceQty, IsComponentProduct, ProductDisID, Product, IsInventory, DetailsId, CloseRate, CloseRateFlag, Remarks, Order_AltQuantity, Order_AltUOM);
                    // End of Rev  Mantis Issue 24428
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

                        // Rev  Mantis Issue 24428
                        string Order_AltQuantity = Convert.ToString(args.NewValues["Order_AltQuantity"]);
                        string Order_AltUOM = Convert.ToString(args.NewValues["Order_AltUOM"]);
                        // End of Rev  Mantis Issue 24428

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
                        string[] ProductDList = ProductDisID.Split(new string[] { "||@||" }, StringSplitOptions.None);
                        ProductDisID = Convert.ToString(ProductDList[0]);
                        string Product = (Convert.ToString(args.NewValues["Product"]) != "") ? Convert.ToString(args.NewValues["Product"]) : "N";
                        string IsInventory = (Convert.ToString(args.NewValues["IsInventory"]) != "") ? Convert.ToString(args.NewValues["IsInventory"]) : "N";
                        string DetailsId = Convert.ToString(args.NewValues["DetailsId"]);
                        string CloseRate = Convert.ToString(args.NewValues["CloseRate"]);
                        string CloseRateFlag = Convert.ToString(args.NewValues["CloseRateFlag"]);
                        string Remarks = (Convert.ToString(args.NewValues["Remarks"]) != "") ? Convert.ToString(args.NewValues["Remarks"]) : "N";
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
                                drr["IsInventory"] = IsInventory;
                                drr["DetailsId"] = DetailsId;
                                drr["CloseRate"] = CloseRate;
                                drr["CloseRateFlag"] = CloseRateFlag;
                                drr["Remarks"] = Remarks;
                                // Rev  Mantis Issue 24428
                                drr["Order_AltQuantity"] = Order_AltQuantity;
                                drr["Order_AltUOM"] = Order_AltUOM;
                                // End of Rev  Mantis Issue 24428
                                break;
                            }
                        }

                        if (Isexists == false)
                        {
                            // Rev  Mantis Issue 24428
                           // Quotationdt.Rows.Add(SrlNo, QuotationID, ProductDetails, Description, Quantity, UOM, Warehouse, StockQuantity, StockUOM, SalePrice, Discount, Amount, TaxAmount, TotalAmount, "U", ProductName, ComponentID, ComponentNumber, TotalQty, BalanceQty, IsComponentProduct, ProductDisID, Product, IsInventory, DetailsId, CloseRate, CloseRateFlag, Remarks);
                            Quotationdt.Rows.Add(SrlNo, QuotationID, ProductDetails, Description, Quantity, UOM, Warehouse, StockQuantity, StockUOM, SalePrice, Discount, Amount, TaxAmount, TotalAmount, "U", ProductName, ComponentID, ComponentNumber, TotalQty, BalanceQty, IsComponentProduct, ProductDisID, Product, IsInventory, DetailsId, CloseRate, CloseRateFlag, Remarks, Order_AltQuantity, Order_AltUOM);

                            // End of Rev  Mantis Issue 24428
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
                    Quotationdt.Rows.Add("0", QuotationID, "0", "", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "D", "", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0","");
                }
            }

            ///////////////////////

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

                //if (Status != "D")
                //{
                //    string CloseRate = Convert.ToString(dr["CloseRate"]);
                //    string CloseRateFlag = Convert.ToString(dr["CloseRateFlag"]);

                //    if(CloseRate==CloseRateFlag)
                //    {
                //        dr["CloseRateFlag"] = 0;
                //    }
                //    else if (CloseRate != CloseRateFlag)
                //    {
                //        dr["CloseRateFlag"] = 1;
                //    }

                //}




            }
            Quotationdt.AcceptChanges();

            Session["SR_QuotationDetails"] = Quotationdt;

            //////////////////////


            if (IsDeleteFrom != "D")
            {
                string InvoiceComponentDate = string.Empty, InvoiceComponent = "";

                string ActionType = Convert.ToString(Session["SR_ActionType"]);
                string MainInvoiceID = Convert.ToString(Session["SR_ReturnID"]);

                string strSchemeType = Convert.ToString(ddl_numberingScheme.SelectedValue);
                string strInvoiceNo = Convert.ToString(txt_PLQuoteNo.Text);
                string strInvoiceDate = Convert.ToString(dt_PLQuote.Date);
                string strBranch = Convert.ToString(ddl_Branch.SelectedValue);

                string strCustomer = Convert.ToString(hdnCustomerId.Value);
                string strContactName = Convert.ToString(cmbContactPerson.Value);
                string Reference = Convert.ToString(txt_Refference.Text);
                string PosForGst = Convert.ToString(ddlPosGstSReturn.Value);
                string strAgents = Convert.ToString(ddl_SalesAgent.SelectedValue);
                string ReasonforReturn = Convert.ToString(txtReasonforChange.Text);

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
                    if (dr["CloseRate"] == "0.00")
                    {
                        dr["CloseRateFlag"] = "0.00";
                    }


                }
                tempQuotation.AcceptChanges();

                DataTable TaxDetailTable = new DataTable();
                if (Session["SR_FinalTaxRecord"] != null)
                {
                    TaxDetailTable = (DataTable)Session["SR_FinalTaxRecord"];
                }

                // DataTable of Warehouse


                DataTable tempWarehousedt = new DataTable();
                if (Session["SR_WarehouseData"] != null)
                {
                    DataTable Warehousedt = (DataTable)Session["SR_WarehouseData"];

                    tempWarehousedt.Columns.Add("Product_SrlNo", typeof(string));
                    tempWarehousedt.Columns.Add("SrlNo", typeof(string));
                    tempWarehousedt.Columns.Add("WarehouseID", typeof(string));
                    tempWarehousedt.Columns.Add("TotalQuantity", typeof(string));
                    tempWarehousedt.Columns.Add("BatchID", typeof(string));
                    tempWarehousedt.Columns.Add("SerialID", typeof(string));
                    tempWarehousedt.Columns.Add("AltQty", typeof(string));
                    tempWarehousedt.Columns.Add("AltUOM", typeof(string));
                    //Rev 1.0
                    tempWarehousedt.Columns.Add("MfgDate", typeof(string));
                    tempWarehousedt.Columns.Add("ExpiryDate", typeof(string));
                    //Rev 1.0 end
                    for (int i = 0; i < Warehousedt.Rows.Count; i++)
                    {
                        tempWarehousedt.Rows.Add(Warehousedt.Rows[i]["Product_SrlNo"],
                    Convert.ToString(Warehousedt.Rows[i]["LoopID"]),
                    Convert.ToString(Warehousedt.Rows[i]["WarehouseID"]),
                    Convert.ToString(Warehousedt.Rows[i]["TotalQuantity"]),
                    Convert.ToString(Warehousedt.Rows[i]["BatchID"]),
                    Convert.ToString(Warehousedt.Rows[i]["SerialID"]),
                    Convert.ToString(Warehousedt.Rows[i]["AltQty"]),
                    Convert.ToString(Warehousedt.Rows[i]["AltUOM"]),
                    //Rev 1.0
                    Convert.ToString(Warehousedt.Rows[i]["MfgDate"]),
                    Convert.ToString(Warehousedt.Rows[i]["ExpiryDate"])
                    //Rev 1.0 end
                    );
                    }

                }
                else
                {
                    tempWarehousedt.Columns.Add("Product_SrlNo", typeof(string));
                    tempWarehousedt.Columns.Add("SrlNo", typeof(string));
                    tempWarehousedt.Columns.Add("WarehouseID", typeof(string));
                    tempWarehousedt.Columns.Add("Quantity", typeof(string));
                    tempWarehousedt.Columns.Add("BatchID", typeof(string));
                    tempWarehousedt.Columns.Add("SerialID", typeof(string));
                    tempWarehousedt.Columns.Add("AltQty", typeof(string));
                    tempWarehousedt.Columns.Add("AltUOM", typeof(string));
                    tempWarehousedt.Columns.Add("MfgDate", typeof(string));
                    tempWarehousedt.Columns.Add("ExpiryDate", typeof(string));
                }

                // End
                //Rev 1.0 
                if (tempWarehousedt != null && tempWarehousedt.Rows.Count > 0)
                {
                    for (int i = 0; i < tempWarehousedt.Rows.Count; i++)
                    {
                        DataRow dr = tempWarehousedt.Rows[i];
                        string strMfgDate = Convert.ToString(dr["MfgDate"]);
                        string strExpiryDate = Convert.ToString(dr["ExpiryDate"]);
                        if (strMfgDate != "")
                        {
                            string DD = strMfgDate.Substring(0, 2);
                            string MM = strMfgDate.Substring(3, 2);
                            string YYYY = strMfgDate.Substring(6, 4);
                            string Date = YYYY + '-' + MM + '-' + DD;

                            dr["MfgDate"] = Date;
                        }

                        if (strExpiryDate != "")
                        {
                            string DD = strExpiryDate.Substring(0, 2);
                            string MM = strExpiryDate.Substring(3, 2);
                            string YYYY = strExpiryDate.Substring(6, 4);
                            string Date = YYYY + '-' + MM + '-' + DD;

                            dr["ExpiryDate"] = Date;
                        }

                    }
                    tempWarehousedt.AcceptChanges();
                }
                //Rev 1.0 end
                //datatable for MultiUOm start chinmoy 14-01-2020
                DataTable MultiUOMDetails = new DataTable();

                if (Session["MultiUOMData"] != null)
                {
                    DataTable MultiUOM = (DataTable)Session["MultiUOMData"];
                    // Mantis Issue 24428
                   // MultiUOMDetails = MultiUOM.DefaultView.ToTable(false, "SrlNo", "Quantity", "UOM", "AltUOM", "AltQuantity", "UomId", "AltUomId", "ProductId", "DetailsId");
                    MultiUOMDetails = MultiUOM.DefaultView.ToTable(false, "SrlNo", "Quantity", "UOM", "AltUOM", "AltQuantity", "UomId", "AltUomId", "ProductId", "DetailsId", "BaseRate", "AltRate", "UpdateRow");
                    // End of Mantis Issue 24428
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


                    // Mantis Issue 24428
                    MultiUOMDetails.Columns.Add("BaseRate", typeof(Decimal));
                    MultiUOMDetails.Columns.Add("AltRate", typeof(Decimal));
                    MultiUOMDetails.Columns.Add("UpdateRow", typeof(string));
                    // End of Mantis Issue 24428
                }
                //End


                // DataTable Of Quotation Tax Details 
                //  Session["SR_TaxDetails"]
                DataTable TaxDetailsdt = new DataTable();
                if (Session["SR_TaxDetails"] != null)
                {
                    TaxDetailsdt = (DataTable)Session["SR_TaxDetails"];
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
                tempTaxDetailsdt.Columns["SlNo"].SetOrdinal(0);
                tempTaxDetailsdt.Columns["Taxes_ID"].SetOrdinal(1);
                tempTaxDetailsdt.Columns["AltTax_Code"].SetOrdinal(2);
                tempTaxDetailsdt.Columns["Percentage"].SetOrdinal(3);
                tempTaxDetailsdt.Columns["Amount"].SetOrdinal(4);

                foreach (DataRow d in tempTaxDetailsdt.Rows)
                {
                    d["SlNo"] = "0";

                }

                // End

                // DataTable Of Billing Address
                #region ##### Added By : Samrat Roy -- to get BillingShipping user control data
                DataTable tempBillAddress = new DataTable();
                tempBillAddress = Purchase_BillingShipping.GetBillingShippingTable();

                #region #### Old_Process ####

                #endregion

                #endregion

                // End

                string approveStatus = "";
                if (Request.QueryString["status"] != null)
                {
                    approveStatus = Convert.ToString(Request.QueryString["status"]);
                }

                //if (ActionType == "Add")
                //{
                //    string[] SchemeList = strSchemeType.Split(new string[] { "~" }, StringSplitOptions.None);
                //    validate = checkNMakeJVCode(strInvoiceNo, Convert.ToInt32(SchemeList[0]));
                //}
                if (tempBillAddress != null && tempBillAddress.Rows.Count == 0)
                {
                    validate = "checkAddress";
                }

                foreach (DataRow dr in tempQuotation.Rows)
                {
                    string strSrlNo = Convert.ToString(dr["SrlNo"]);
                    decimal strProductQuantity = Convert.ToDecimal(dr["Quantity"]);

                    decimal strWarehouseQuantity = 0;
                    GetQuantityBaseOnProduct(strSrlNo, ref strWarehouseQuantity);
                    if (Convert.ToString(dr["IsInventory"]) == "Yes")
                    {
                        if (Convert.ToString(Request.QueryString["key"]) != "ADD")
                        {
                            if (Session["SR_WarehouseData"] != null)
                            {
                                if (strProductQuantity != strWarehouseQuantity)
                                {
                                    validate = "checkWarehouse";
                                    grid.JSProperties["cpProductSrlIDCheck"] = strSrlNo;
                                    break;
                                }
                            }
                        }
                        else
                        {

                            if (strProductQuantity != strWarehouseQuantity)
                            {
                                validate = "checkWarehouse";
                                grid.JSProperties["cpProductSrlIDCheck"] = strSrlNo;
                                break;
                            }

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
                        GetQuantityBaseOnProductforDetailsId(strDetailsId, ref strUOMQuantity);


                        if (Convert.ToString(dr["IsInventory"]) == "Yes")
                        {
                            DataTable dtb = new DataTable();
                            dtb = (DataTable)Session["MultiUOMData"];
                            if (dtb.Rows.Count > 0)
                            {
                                // Mantis Issue 24428
                                //if (strUOMQuantity != null)
                                //{
                                //    if (strProductQuantity != strUOMQuantity)
                                //    {
                                //        validate = "checkMultiUOMData";
                                //        grid.JSProperties["cpcheckMultiUOMData"] = strSrlNo;
                                //        break;
                                //    }
                                //}
                                // End of Mantis Issue 24428

                                // Rev 2.0
                                DataRow[] MultiUoMresult;

                                MultiUoMresult = dtb.Select("SrlNo ='" + strSrlNo + "' and UpdateRow ='True'");

                                if (MultiUoMresult.Length > 0)
                                {
                                    if ((Convert.ToDecimal(MultiUoMresult[0]["Quantity"]) != Convert.ToDecimal(dr["Quantity"])) ||
                                        (Math.Round(Convert.ToDecimal(MultiUoMresult[0]["AltQuantity"]), 2) != Math.Round(Convert.ToDecimal(dr["Order_AltQuantity"]), 2)) ||
                                        (Math.Round(Convert.ToDecimal(MultiUoMresult[0]["BaseRate"]), 2) != Math.Round(Convert.ToDecimal(dr["SalePrice"]), 2))
                                        )
                                    {
                                        validate = "checkMultiUOMData_QtyMismatch";
                                        grid.JSProperties["cpcheckMultiUOMData"] = strSrlNo;
                                        break;
                                    }
                                }
                                else
                                {
                                    validate = "checkMultiUOMData_NotFound";
                                    grid.JSProperties["cpcheckMultiUOMData"] = strSrlNo;
                                    break;
                                }
                                // End of Rev 2.0
                            }
                            else if (dtb.Rows.Count < 1)
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
                if (PosForGst == "")
                {
                    validate = "EmptyPlaceOfsupply";
                }

                //// ############# Added By : Samrat Roy -- 02/05/2017 -- To check Transporter Mandatory Control 
                //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

                BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();

                DataTable DT = objEngine.GetDataTable("Config_SystemSettings", " Variable_Value ", " Variable_Name='Transporter_SRMandatory' AND IsActive=1");
                if (DT != null && DT.Rows.Count > 0)
                {
                    string IsMandatory = Convert.ToString(DT.Rows[0]["Variable_Value"]).Trim();

                    if (Convert.ToString(Session["TransporterVisibilty"]).Trim() == "Yes")
                    {
                        if (IsMandatory == "Yes")
                        {
                            if (hfControlData.Value.Trim() == "")
                            {
                                validate = "transporteMandatory";
                            }
                        }
                    }
                }

                string placeState = Convert.ToString(hdnPlaceOfSupply.Value);
                string SupplyState = "";

                if (!string.IsNullOrEmpty(placeState))
                {
                    string[] POSValue = (placeState.Split('~'));
                    SupplyState = Convert.ToString(POSValue[2]);
                }


                CommonBL ComBL = new CommonBL();
                string GSTRateTaxMasterMandatory = ComBL.GetSystemSettingsResult("GSTRateTaxMasterMandatory");
                if (!String.IsNullOrEmpty(GSTRateTaxMasterMandatory))
                {
                    if (GSTRateTaxMasterMandatory == "Yes")
                    {
                        // Mantis Issue 24429
                        DataTable temp_Quotation = tempQuotation.Copy();
                        if (temp_Quotation.Columns.Contains("Order_AltQuantity"))
                        {
                            temp_Quotation.Columns.Remove("Order_AltQuantity");
                        }
                        if (temp_Quotation.Columns.Contains("Order_AltUOM"))
                        {
                            temp_Quotation.Columns.Remove("Order_AltUOM");
                        }
                        // End of Mantis Issue 24429
                        DataTable dtTaxDetails = new DataTable();
                        ProcedureExecute procT = new ProcedureExecute("prc_CRMSalesReturn_Details");
                        procT.AddVarcharPara("@Action", 500, "GetTaxDetailsByProductID");
                        // Mantis Issue 24429
                        //procT.AddPara("@ProductDetails", tempQuotation);
                        procT.AddPara("@ProductDetails", temp_Quotation);
                        // End of Mantis Issue 24429
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
                                decimal _TaxAmount = Math.Round(Convert.ToDecimal(dr["TaxAmount"]), 2);
                                string ProductName = Convert.ToString(dr["ProductName"]);
                                DataRow[] rows;
                                if (TaxDetailTable != null && TaxDetailTable.Rows.Count > 0)
                                {
                                    rows = TaxDetailTable.Select("SlNo = '" + SerialID + "' and TaxCode='" + TaxID + "'");

                                }
                                else
                                {
                                    string ShippingStateValue = Purchase_BillingShipping.GetShippingStateId();

                                    if (hdnPlaceOfSupply.Value != null)
                                        ShippingStateValue = hdnPlaceOfSupply.Value.Split('~')[1].ToString();

                                    string strTaxTypesValue = "";
                                    if (ddl_AmountAre.Value.ToString() == "1")
                                        strTaxTypesValue = "E";
                                    else if (ddl_AmountAre.Value.ToString() == "2")
                                        strTaxTypesValue = "I";

                                    DataTable TaxDetailTableExcep = new DataTable();
                                    TaxDetailTableExcep = gstTaxDetails.SetTaxTableDataWithProductSerialRoundOff(ref tempQuotation, "SrlNo", "ProductID", "Amount", "TaxAmount", TaxDetailTable, "S", dt_PLQuote.Date.ToString("yyyy-MM-dd"), strBranch, ShippingStateValue, strTaxTypesValue);
                                    rows = TaxDetailTableExcep.Select("SlNo = '" + SerialID + "' and TaxCode='" + TaxID + "'");
                                }
                                if (rows != null && rows.Length > 0)
                                {
                                    //decimal EntryTaxAmount = Math.Round(Convert.ToDecimal(rows[0]["Amount"]), 2);
                                    decimal EntryTaxAmount = Math.Round(Convert.ToDecimal(rows[0]["Amount"]), 2);

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

                //// ############# Added By : Samrat Roy -- 02/05/2017 -- To check Transporter Mandatory Control 
                
                // Rev 2.0 [validate == "checkMultiUOMData_QtyMismatch", "checkMultiUOMData_NotFound" added]    
                if (validate == "outrange" || validate == "duplicate" || validate == "checkWarehouse" || validate == "duplicateProduct" 
                    || validate == "nullAmount" || validate == "nullQuantity" || validate == "transporteMandatory" || validate == "checkAddress"
                    || validate == "EmptyPlaceOfsupply" || validate == "checkMultiUOMData" || validate == "checkAcurateTaxAmount"
                    || validate == "checkMultiUOMData_QtyMismatch" || validate == "checkMultiUOMData_NotFound"
                    )
                {
                    grid.JSProperties["cpSaveSuccessOrFail"] = validate;
                }
                else
                {
                    //grid.JSProperties["cpQuotationNo"] = UniqueQuotation;

                    #region To Show By Default Cursor after SAVE AND NEW
                    if (Convert.ToString(Session["SR_ActionType"]) == "Add") // session has been removed from quotation list page working good
                    {
                        string[] schemaid = new string[] { };
                        string schemavalue = ddl_numberingScheme.SelectedValue;
                        Session["SR_schemavalue"] = schemavalue;        // session has been removed from quotation list page working good
                        schemaid = ddl_numberingScheme.SelectedValue.Split('~');

                        string schematype = schemaid[1];
                        if (hdnRefreshType.Value == "N")
                        {
                            if (schematype == "1")
                            {
                                Session["SR_SaveMode"] = "A";
                            }
                            else
                            {
                                Session["SR_SaveMode"] = "M";
                            }
                        }
                    }

                    #endregion

                    #region Subhabrata Section Start

                    int strIsComplete = 0, strQuoteID = 0;


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

                    string ShippingState = Purchase_BillingShipping.GetShippingStateId();

                    if (hdnPlaceOfSupply.Value != null)
                        ShippingState = hdnPlaceOfSupply.Value.Split('~')[1].ToString();

                    string strTaxTypes = "";
                    if (ddl_AmountAre.Value.ToString() == "1")
                        strTaxTypes = "E";
                    else if (ddl_AmountAre.Value.ToString() == "2")
                        strTaxTypes = "I";


                    CommonBL cbl = new CommonBL();
                    string ProjectSelectcashbankModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");
                    Int64 ProjId = 0;
                    if (lookup_Project.Text != "")
                    {
                        string projectCode = lookup_Project.Text;
                        DataTable dtSlOrd = GetProjectCode(projectCode);
                        //oDbEngine.GetDataTable("select Proj_Id from Master_ProjectManagement where Proj_Code='" + projectCode + "'");
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
                    TaxDetailTable = gstTaxDetails.SetTaxTableDataWithProductSerialRoundOff(ref tempQuotation, "SrlNo", "ProductID", "Amount", "TaxAmount", TaxDetailTable, "S", dt_PLQuote.Date.ToString("yyyy-MM-dd"), strBranch, ShippingState, strTaxTypes);

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

                    DataTable dtAddlDesc = (DataTable)Session["InlineRemarks"];
                    //ModifySalesReturn(MainInvoiceID, strSchemeType, UniqueQuotation, strInvoiceDate, strCustomer, strContactName, ProjId,
                    //                Reference, PosForGst, strBranch, strAgents, strCurrency, strRate, strTaxType, strTaxCode,
                    //                InvoiceComponent, InvoiceComponentDate, strCashBank, strDueDate,
                    //                tempQuotation, TaxDetailTable, tempWarehousedt, tempTaxDetailsdt, tempBillAddress,
                    //                approveStatus, ActionType, ref strIsComplete, ref strQuoteID, ReasonforReturn, duplicatedt2, MultiUOMDetails);

                    ModifySalesReturn(MainInvoiceID, strSchemeType, SchemaID, txt_PLQuoteNo.Text, strInvoiceDate, strCustomer, strContactName, ProjId,
                                    Reference, PosForGst, strBranch, strAgents, strCurrency, strRate, strTaxType, strTaxCode,
                                    InvoiceComponent, InvoiceComponentDate, strCashBank, strDueDate,
                                    tempQuotation, TaxDetailTable, tempWarehousedt, tempTaxDetailsdt, tempBillAddress,
                                    approveStatus, ActionType, ref strIsComplete, ref strQuoteID, ReasonforReturn, duplicatedt2, MultiUOMDetails, dtAddlDesc, drdTransCategory.SelectedValue);


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

                        grid.JSProperties["cpSaleRID"] = strQuoteID;
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

                        if (!string.IsNullOrEmpty(ReceiverEmail))
                        {

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
                    else if (strIsComplete == -12)
                    {
                        grid.JSProperties["cpSaveSuccessOrFail"] = "EmptyProject";
                    }
                    else if (strIsComplete == 2)
                    {
                        grid.JSProperties["cpSaveSuccessOrFail"] = "quantityTagged";
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
            proc.AddVarcharPara("@Action", 500, "GetAddLockForSalesREturn");

            dt = proc.GetTable();
            return dt;

        }

        public void GetQuantityBaseOnProduct(string strProductSrlNo, ref decimal WarehouseQty)
        {
            decimal sum = 0;

            DataTable Warehousedt = new DataTable();
            if (Session["SR_WarehouseData"] != null)
            {
                Warehousedt = (DataTable)Session["SR_WarehouseData"];
                for (int i = 0; i < Warehousedt.Rows.Count; i++)
                {
                    DataRow dr = Warehousedt.Rows[i];
                    string Product_SrlNo = Convert.ToString(dr["Product_SrlNo"]);

                    if (strProductSrlNo == Product_SrlNo)
                    {
                        string strQuantity = (Convert.ToString(dr["Quantity"]) != "") ? Convert.ToString(dr["Quantity"]) : "0";
                        var weight = Decimal.Parse(Regex.Match(strQuantity, "[0-9]*\\.*[0-9]*").Value);

                        sum = sum + Convert.ToDecimal(weight);
                    }
                }
            }

            WarehouseQty = sum;
        }


        public void GetQuantityBaseOnProductforDetailsId(string strDetailsId, ref decimal strUOMQuantity)
        {
            decimal sum = 0;

            DataTable MultiUOMData = new DataTable();
            if (Session["MultiUOMData"] != null)
            {
                MultiUOMData = (DataTable)Session["MultiUOMData"];
                for (int i = 0; i < MultiUOMData.Rows.Count; i++)
                {
                    DataRow dr = MultiUOMData.Rows[i];
                    string UomDetailsid = Convert.ToString(dr["DetailsId"]);

                    if (strDetailsId == UomDetailsid)
                    {
                        string strQuantity = (Convert.ToString(dr["Quantity"]) != "") ? Convert.ToString(dr["Quantity"]) : "0";
                        var weight = Decimal.Parse(Regex.Match(strQuantity, "[0-9]*\\.*[0-9]*").Value);

                        sum = Convert.ToDecimal(weight);
                    }
                }
            }

            strUOMQuantity = sum;

        }

        protected void grid_DataBinding(object sender, EventArgs e)
        {
            if (Session["SR_QuotationDetails"] != null)
            {
                DataTable Quotationdt = (DataTable)Session["SR_QuotationDetails"];
                if (Quotationdt.Rows.Count > 0)
                {
                    DataView dvData = new DataView(Quotationdt);
                    dvData.RowFilter = "Status <> 'D'";
                    grid.DataSource = GetQuotation(dvData.ToTable());
                }
                else
                {
                    grid.DataSource = null;
                }
            }
            else
            {
                grid.DataSource = null;
            }
        }
        protected void MultiUOM_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string SpltCmmd = e.Parameters.Split('~')[0];

            grid_MultiUOM.JSProperties["cpDuplicateAltUOM"] = "";
            grid_MultiUOM.JSProperties["cpOpenFocus"] = "";
            // Rev Sanchita
            grid_MultiUOM.JSProperties["cpSetBaseQtyRateInGrid"] = "";
            // End of Rev Sanchita

            if (SpltCmmd == "MultiUOMDisPlay")
            {

                DataTable MultiUOMData = new DataTable();

                if (Session["MultiUOMData"] != null)
                {
                    MultiUOMData = (DataTable)Session["MultiUOMData"];
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
                    //if (DetailsId != "" && DetailsId != null && DetailsId != "null")
                    //{
                    //    //dvData.RowFilter = "SrlNo = '" + SrlNo + "' and Doc_DetailsId='" + DetailsId + "'";
                    //    dvData.RowFilter = "DetailsId='" + DetailsId + "'";
                    //}
                    // if
                    //{
                        dvData.RowFilter = "SrlNo = '" + SrlNo + "'";
                    //}
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
                // Mantis Issue 24428
                int MultiUOMSR = 1;
                //End Mantis Issue 24428
                string SrlNo = Convert.ToString(e.Parameters.Split('~')[1]);
                string Quantity = Convert.ToString(e.Parameters.Split('~')[2]);
                string UOM = Convert.ToString(e.Parameters.Split('~')[3]);
                string AltUOM = Convert.ToString(e.Parameters.Split('~')[4]);
                string AltQuantity = Convert.ToString(e.Parameters.Split('~')[5]);
                string UomId = Convert.ToString(e.Parameters.Split('~')[6]);
                string AltUomId = Convert.ToString(e.Parameters.Split('~')[7]);
                string ProductId = Convert.ToString(e.Parameters.Split('~')[8]);
                string DetailsId = Convert.ToString(e.Parameters.Split('~')[9]);

                // Mantis Issue 24428
                string BaseRate = Convert.ToString(e.Parameters.Split('~')[10]);
                string AltRate = Convert.ToString(e.Parameters.Split('~')[11]);
                string UpdateRow = Convert.ToString(e.Parameters.Split('~')[12]);
                // End of Mantis Issue 24428

                DataTable allMultidataDetails = (DataTable)Session["MultiUOMData"];



                DataRow[] MultiUoMresult;

                if (allMultidataDetails != null && allMultidataDetails.Rows.Count > 0)
                {

                    //Rev Mantis 24428 add to DetailsId != "0"
                    if (DetailsId != "" && DetailsId != null && DetailsId != "null" && DetailsId != "0")
                    {
                     //End Mantis 24428
                        //MultiUoMresult = allMultidataDetails.Select("SrlNo ='" + SrlNo + "' and Doc_DetailsId='" + DetailsId + "'");
                        MultiUoMresult = allMultidataDetails.Select("DetailsId='" + DetailsId + "'");
                    }
                    else
                    {
                        MultiUoMresult = allMultidataDetails.Select("SrlNo ='" + SrlNo + "'");
                    }
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
                        // Mantis Issue 24428  [ if "Update Row" checkbox is checked, then all existing Update Row in the grid will be set to False]
                        if (UpdateRow == "True")
                        {
                            item["UpdateRow"] = "False";
                        }
                        // End of Mantis Issue 24428 
                    }
                }

                if (Validcheck != "DuplicateUOM")
                {
                    if (Session["MultiUOMData"] != null)
                    {

                        MultiUOMSaveData = (DataTable)Session["MultiUOMData"];

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
                    DataRow thisRow;
                    if (MultiUOMSaveData.Rows.Count > 0)
                    {
                        // Rev Sanchita
                        //thisRow = (DataRow)MultiUOMSaveData.Rows[MultiUOMSaveData.Rows.Count - 1];
                        //MultiUOMSaveData.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId, DetailsId, BaseRate, AltRate, UpdateRow, (Convert.ToInt16(thisRow["MultiUOMSR"]) + 1));
                        MultiUOMSR = Convert.ToInt32(MultiUOMSaveData.Compute("max([MultiUOMSR])", string.Empty)) + 1;
                        MultiUOMSaveData.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId, DetailsId, BaseRate, AltRate, UpdateRow, MultiUOMSR);
                        // End of Rev Sanchita
                    }
                    else
                    {
                        MultiUOMSaveData.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId, DetailsId, BaseRate, AltRate, UpdateRow, MultiUOMSR);
                    }
                    // Mantis Issue 24428
                    //if (DetailsId != "" && DetailsId != null && DetailsId != "null")
                    //{
                    //    // Mantis Issue 24428
                    //    //MultiUOMSaveData.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId, DetailsId);
                    //    MultiUOMSaveData.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId, DetailsId, BaseRate, AltRate, UpdateRow);
                    //    // End of Mantis Issue 24428
                    //}
                    //else
                    //{
                    //    // Mantis Issue 24428
                    //    //MultiUOMSaveData.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId, DetailsId);
                    //    MultiUOMSaveData.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId, DetailsId, BaseRate, AltRate, UpdateRow);
                    //    // End of Mantis Issue 24428
                    //}
                    // End of Mantis Issue 24428
                    MultiUOMSaveData.AcceptChanges();
                    Session["MultiUOMData"] = MultiUOMSaveData;

                    if (MultiUOMSaveData != null && MultiUOMSaveData.Rows.Count > 0)
                    {
                        DataView dvData = new DataView(MultiUOMSaveData);

                        //Rev Mantis 24428 add con DetailsId != "0"
                        if (DetailsId != "" && DetailsId != null && DetailsId != "null" && DetailsId != "0")
                        {

                            //End Mantis 24428
                            //dvData.RowFilter = "SrlNo = '" + SrlNo + "' and Doc_DetailsId='" + DetailsId + "'";
                            dvData.RowFilter = "DetailsId='" + DetailsId + "'";
                        }
                        else
                        {
                            dvData.RowFilter = "SrlNo = '" + SrlNo + "'";
                        }
                        grid_MultiUOM.DataSource = dvData;
                        grid_MultiUOM.DataBind();
                    }
                    else
                    {
                        //MultiUOMSaveData.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId);
                        //Session["MultiUOMData"] = MultiUOMSaveData;
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
                DataTable dt = (DataTable)Session["MultiUOMData"];

                if (dt != null && dt.Rows.Count > 0)
                {
                    if (DetailsId != "" && DetailsId != null && DetailsId != "null")
                    {
                        //MultiUoMresult = dt.Select("SrlNo ='" + SrlNo + "' and Doc_DetailsId='" + DetailsId + "'");
                        MultiUoMresult = dt.Select("DetailsId='" + DetailsId + "'");
                    }
                    else
                    {
                        MultiUoMresult = dt.Select("SrlNo ='" + SrlNo + "'");
                    }

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
                Session["MultiUOMData"] = dt;
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataView dvData = new DataView(dt);
                    if (DetailsId != "" && DetailsId != null && DetailsId != "null")
                    {
                        //dvData.RowFilter = "SrlNo = '" + SrlNo + "'and Doc_DetailsId='" + DetailsId + "'";
                        dvData.RowFilter = "DetailsId='" + DetailsId + "'";
                    }
                    else
                    {
                        dvData.RowFilter = "SrlNo = '" + SrlNo + "'";
                    }
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
                DataTable dt = (DataTable)Session["MultiUOMData"];
                string detailsId = Convert.ToString(e.Parameters.Split('~')[2]);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow[] MultiUoMresult;
                    if (detailsId != null && detailsId != "" && detailsId != "null")
                    {
                        MultiUoMresult = dt.Select("DetailsId ='" + detailsId + "'");
                    }
                    else
                    {
                        MultiUoMresult = dt.Select("SrlNo ='" + SrlNo + "'");
                    }
                    foreach (DataRow item in MultiUoMresult)
                    {
                        item.Table.Rows.Remove(item);
                    }
                }
                Session["MultiUOMData"] = dt;
            }
            // Mantis Issue 24428
            else if (SpltCmmd == "EditData")
            {
                string AltUOMKeyValuewithqnty = e.Parameters.Split('~')[1];
                string AltUOMKeyValue = e.Parameters.Split('~')[2];
                string AltUOMKeyqnty = AltUOMKeyValuewithqnty.Split('|')[1];

                string SrlNo = Convert.ToString(e.Parameters.Split('~')[1]);
                DataTable dt = (DataTable)Session["MultiUOMData"];

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
                Session["MultiUOMData"] = dt;
            }



            else if (SpltCmmd == "UpdateRow")
            {


                string SrlNoR = e.Parameters.Split('~')[1];
                string AltUOMKeyValue = e.Parameters.Split('~')[7];
                string AltUOMKeyqnty = e.Parameters.Split('~')[5];
                string muid = e.Parameters.Split('~')[13];
                // Rev Sanchita
                string SrlNo = "0";
                // End of Rev Sanchita

                string Validcheck = "";
                DataTable MultiUOMSaveData = new DataTable();

                DataTable dt = (DataTable)Session["MultiUOMData"];

                // Rev Sanchita
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow[] MultiUoMresult = dt.Select("MultiUOMSR ='" + muid + "'");
                    foreach (DataRow item in MultiUoMresult)
                    {
                        // Rev SAnchita
                        SrlNo = Convert.ToString(item["SrlNo"]);
                        // End of Rev Sanchita
                        //item.Table.Rows.Remove(item);
                        //break;

                    }
                }
                // End of Rev Sanchita

                // Rev Sanchita
                //string SrlNo = Convert.ToString(e.Parameters.Split('~')[1]);
                // End of Rev Sanchita
                string Quantity = Convert.ToString(e.Parameters.Split('~')[2]);
                string UOM = Convert.ToString(e.Parameters.Split('~')[3]);
                string AltUOM = Convert.ToString(e.Parameters.Split('~')[4]);
                string AltQuantity = Convert.ToString(e.Parameters.Split('~')[5]);
                string UomId = Convert.ToString(e.Parameters.Split('~')[6]);
                string AltUomId = Convert.ToString(e.Parameters.Split('~')[7]);
                string ProductId = Convert.ToString(e.Parameters.Split('~')[8]);
                string DetailsId = Convert.ToString(e.Parameters.Split('~')[9]);
                // Mantis Issue 24428
                string BaseRate = Convert.ToString(e.Parameters.Split('~')[10]);
                string AltRate = Convert.ToString(e.Parameters.Split('~')[11]);
                string UpdateRow = Convert.ToString(e.Parameters.Split('~')[12]);
             
                // Rev Sanchita
                //dt.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId,DetailsId, BaseRate, AltRate, UpdateRow, muid);
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
                    // Mantis Issue 24428  [ if "Update Row" checkbox is checked, then all existing Update Row in the grid will be set to False]
                    if (UpdateRow == "True")
                    {
                        item["UpdateRow"] = "False";
                    }
                    // End of Mantis Issue 24428 
                }


                if (Validcheck != "DuplicateUOM")
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        DataRow[] MultiUoMresult = dt.Select("MultiUOMSR ='" + muid + "'");
                        foreach (DataRow item in MultiUoMresult)
                        {
                            //// Rev SAnchita
                            //SrlNo = Convert.ToString(item["SrlNo"]);
                            //// End of Rev Sanchita
                            item.Table.Rows.Remove(item);
                            break;

                        }
                    }
                    dt.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId, DetailsId, BaseRate, AltRate, UpdateRow, muid);
                }
                //End Rev Sanchita

                // End of Mantis Issue 24428
                Session["MultiUOMData"] = dt;

                MultiUOMSaveData = (DataTable)Session["MultiUOMData"];

                MultiUOMSaveData.AcceptChanges();
                Session["MultiUOMData"] = MultiUOMSaveData;

                if (MultiUOMSaveData != null && MultiUOMSaveData.Rows.Count > 0)
                {
                    DataView dvData = new DataView(MultiUOMSaveData);
                    // dvData.RowFilter = "SrlNo = '" + SrlNo + "'";
                    // Rev Sanchita
                    dvData.RowFilter = "SrlNo = '" + SrlNo + "'";
                    // End of Rev Sanchita

                    grid_MultiUOM.DataSource = dvData;
                    grid_MultiUOM.DataBind();
                }




                //if (dt != null && dt.Rows.Count > 0)
                //{
                //    DataView dvData = new DataView(dt);
                //    dvData.RowFilter = "SrlNo = '" + SrlNo + "'";

                //    grid_MultiUOM.DataSource = dvData;
                //    grid_MultiUOM.DataBind();
                //}
                //else
                //{
                //    //MultiUOMSaveData.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId);
                //    //Session["MultiUOMData"] = MultiUOMSaveData;
                //    grid_MultiUOM.DataSource = dt.DefaultView;
                //    grid_MultiUOM.DataBind();
                //}

            }








            // End of Mantis Issue 24428




            // Mantis Issue 24428
            else if (SpltCmmd == "SetBaseQtyRateInGrid")
            {
                DataTable dt = new DataTable();

                if (Session["MultiUOMData"] != null)
                {
                    string SrlNo = Convert.ToString(e.Parameters.Split('~')[1]);
                    dt = (DataTable)HttpContext.Current.Session["MultiUOMData"];
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


                    //if (Session["OrderDetails"] != null)
                    //{
                    //    DataTable SalesOrderdt = (DataTable)Session["OrderDetails"];

                    //    DataRow[] drSalesOrder = SalesOrderdt.Select("SrlNo ='" + SelNo + "'");
                    //    if (drSalesOrder.Length > 0)
                    //    {
                    //        drSalesOrder[0]["Quantity"] = BaseQty;
                    //        drSalesOrder[0]["SalePrice"] = BaseRate;
                    //    }

                    //}

                }
            }
            // End of Mantis Issue 24428


        }
        protected void MultiUOM_DataBinding(object sender, EventArgs e)
        {
            //DataTable dt = (DataTable)Session["MultiUOMData"];
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
                    DataTable Quotationdt = (DataTable)Session["SR_QuotationDetails"];
                    grid.DataSource = GetQuotation(Quotationdt);
                    grid.DataBind();
                }
            }
            else if (strSplitCommand == "RemoveDisplay")
            {
                DataTable salesReturndt = new DataTable();
                if (Session["SR_QuotationDetails"] != null)
                {
                    salesReturndt = (DataTable)Session["SR_QuotationDetails"];
                }

                DataRow[] dr = salesReturndt.Select();
                foreach (DataRow row in dr)
                {
                    salesReturndt.Rows.Remove(row);
                }

                salesReturndt.AcceptChanges();
                Session["SR_QuotationDetails"] = salesReturndt;
                grid.DataBind();

                grid.JSProperties["cpRemoveProductInvoice"] = "valid";


            }
            else if (strSplitCommand == "CurrencyChangeDisplay")
            {
                if (Session["SR_QuotationDetails"] != null)
                {
                    DataTable Quotationdt = (DataTable)Session["SR_QuotationDetails"];

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

                    Session["SR_QuotationDetails"] = Quotationdt;

                    DataView dvData = new DataView(Quotationdt);
                    dvData.RowFilter = "Status <> 'D'";

                    grid.DataSource = GetQuotation(dvData.ToTable());
                    grid.DataBind();
                }
            }
            else if (strSplitCommand == "DateChangeDisplay")
            {
                if (Session["SR_QuotationDetails"] != null)
                {
                    DataTable Quotationdt = (DataTable)Session["SR_QuotationDetails"];

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

                    Session["SR_QuotationDetails"] = Quotationdt;

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
                InvoiceDetails_Id = InvoiceDetails_Id.TrimStart(',');
                Product_id1 = Product_id1.TrimStart(',');
                string Quote_Nos = Convert.ToString(e.Parameters.Split('~')[1]);
                string companyId = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                Session["ReturnInvoiceIds"] = QuoComponent1;
                string fin_year = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);
                if (Quote_Nos != "$")
                {
                    DataSet dt_Quotationtaggeddata = new DataSet();

                    DataTable dt_QuotationDetails = new DataTable();
                    DataTable dtMUltiUOM = new DataTable();
                    string IdKey = Convert.ToString(Request.QueryString["key"]);
                   // Rev Sanchita
                    //if (!string.IsNullOrEmpty(IdKey))
                    //{
                    //    if (IdKey != "ADD")
                    //    {
                    //        // dt_QuotationDetails = objSalesReturnBL.GetIndentDetailsForPOGridBind(QuoComponent1, IdKey, Product_id1, companyId, fin_year);

                    //        dt_Quotationtaggeddata = objSalesReturnBL.GetSIDetailsForSRGridBind(QuoComponent1, IdKey, Product_id1, companyId, fin_year, dt_PLQuote.Date.ToString("yyyy-MM-dd"), ddl_Branch.SelectedValue, InvoiceDetails_Id);
                    //        dt_QuotationDetails = dt_Quotationtaggeddata.Tables[0];
                    //    }
                    //    else
                    //    {
                    //        // dt_QuotationDetails = objSalesReturnBL.GetIndentDetailsForPOGridBind(QuoComponent1, IdKey, Product_id1, companyId, fin_year);
                    //        dt_Quotationtaggeddata = objSalesReturnBL.GetSIDetailsForSRGridBind(QuoComponent1, IdKey, Product_id1, companyId, fin_year, dt_PLQuote.Date.ToString("yyyy-MM-dd"), ddl_Branch.SelectedValue, InvoiceDetails_Id);
                    //        dt_QuotationDetails = dt_Quotationtaggeddata.Tables[0];
                    //        //chinmoy added for multiuom

                    //        dtMUltiUOM = objSalesReturnBL.GetMultiUOMDetailsForSRGridBind(QuoComponent1, InvoiceDetails_Id, Product_id1, companyId, fin_year);
                    //        Session["MultiUOMData"] = dtMUltiUOM;
                    //    }

                    //}
                    //else
                    //{
                    //    // dt_QuotationDetails = objSalesReturnBL.GetIndentDetailsForPOGridBind(QuoComponent1, IdKey, Product_id1, companyId, fin_year);
                    //    dt_Quotationtaggeddata = objSalesReturnBL.GetSIDetailsForSRGridBind(QuoComponent1, IdKey, Product_id1, companyId, fin_year, dt_PLQuote.Date.ToString("yyyy-MM-dd"), ddl_Branch.SelectedValue, InvoiceDetails_Id);
                    //    dt_QuotationDetails = dt_Quotationtaggeddata.Tables[0];
                    //}

                    if (!string.IsNullOrEmpty(IdKey))
                    {
                        if (IdKey != "ADD")
                        {
                            // dt_QuotationDetails = objSalesReturnBL.GetIndentDetailsForPOGridBind(QuoComponent1, IdKey, Product_id1, companyId, fin_year);

                            dt_Quotationtaggeddata = objSalesReturnBL.GetSIDetailsForSRGridBind_New(QuoComponent1, IdKey, Product_id1, companyId, fin_year, dt_PLQuote.Date.ToString("yyyy-MM-dd"), ddl_Branch.SelectedValue, InvoiceDetails_Id);
                            dt_QuotationDetails = dt_Quotationtaggeddata.Tables[0];
                        }
                        else
                        {
                            // dt_QuotationDetails = objSalesReturnBL.GetIndentDetailsForPOGridBind(QuoComponent1, IdKey, Product_id1, companyId, fin_year);
                            dt_Quotationtaggeddata = objSalesReturnBL.GetSIDetailsForSRGridBind_New(QuoComponent1, IdKey, Product_id1, companyId, fin_year, dt_PLQuote.Date.ToString("yyyy-MM-dd"), ddl_Branch.SelectedValue, InvoiceDetails_Id);
                            dt_QuotationDetails = dt_Quotationtaggeddata.Tables[0];
                            //chinmoy added for multiuom

                            dtMUltiUOM = objSalesReturnBL.GetMultiUOMDetailsForSRGridBind_New(QuoComponent1, InvoiceDetails_Id, Product_id1, companyId, fin_year);
                            Session["MultiUOMData"] = dtMUltiUOM;
                        }

                    }
                    else
                    {
                        // dt_QuotationDetails = objSalesReturnBL.GetIndentDetailsForPOGridBind(QuoComponent1, IdKey, Product_id1, companyId, fin_year);
                        dt_Quotationtaggeddata = objSalesReturnBL.GetSIDetailsForSRGridBind_New(QuoComponent1, IdKey, Product_id1, companyId, fin_year, dt_PLQuote.Date.ToString("yyyy-MM-dd"), ddl_Branch.SelectedValue, InvoiceDetails_Id);
                        dt_QuotationDetails = dt_Quotationtaggeddata.Tables[0];
                    }
                    // End of Rev Sanchita
                    //rev rajdip for running data on edit mode

                    DataTable Orderdt = dt_QuotationDetails.Copy();

                    decimal TotalQty = 0;
                    decimal TotalAmt = 0;
                    decimal TaxAmount = 0;
                    decimal Amount = 0;
                    decimal SalePrice = 0;
                    decimal AmountWithTaxValue = 0;
                    decimal othercharges = 0;
                    for (int i = 0; i < Orderdt.Rows.Count; i++)
                    {
                        TotalQty = TotalQty + Convert.ToDecimal(Orderdt.Rows[i]["Quantity"]);
                        Amount = Amount + Convert.ToDecimal(Orderdt.Rows[i]["Amount"]);
                        TaxAmount = TaxAmount + Convert.ToDecimal(Orderdt.Rows[i]["TaxAmount"]);
                        SalePrice = SalePrice + Convert.ToDecimal(Orderdt.Rows[i]["SalePrice"]);
                        TotalAmt = TotalAmt + Convert.ToDecimal(Orderdt.Rows[i]["TotalAmount"]);

                    }
                    AmountWithTaxValue = TaxAmount + Amount;

                    othercharges = Convert.ToDecimal(bnrOtherChargesvalue.Text.ToString());

                    ASPxLabel12.Text = TotalQty.ToString();
                    bnrLblTaxableAmtval.Text = Amount.ToString();
                    bnrLblTaxAmtval.Text = TaxAmount.ToString();
                    bnrlblAmountWithTaxValue.Text = AmountWithTaxValue.ToString();
                    bnrLblInvValue.Text = TotalAmt.ToString();

                    TotalAmt = TotalAmt + othercharges;
                    string PosExistscheck = ""; 
                    DataTable dtPoscheck = oDBEngine.GetDataTable("select ISNULL(Invoice_Id,0) SalesmanId from tbl_trans_SalesInvoice where isFromPos=1 and Invoice_Id in(" + QuoComponent1 + ")");
                    if(dtPoscheck !=null &&  dtPoscheck.Rows.Count>0)
                    {
                        hdnTaggedDoctype.Value = "PosAvailable";
                        PosExistscheck = "PosAvailable";
                     
                    }
                    else
                    {
                        hdnTaggedDoctype.Value = "PosNotAvailable";
                        PosExistscheck = "PosNotAvailable";
                    }
                    string Salesmanvalue = "";
                    DataTable dtsalesman = oDBEngine.GetDataTable("select ISNULL(Invoice_SalesmanId,'') SalesmanId from tbl_trans_SalesInvoice where isFromPos=1 and Invoice_Id='" + Convert.ToString(grid_Products.GetSelectedFieldValues("Quotation_No")[0]) + "'");
                    if (dtsalesman != null && dtsalesman.Rows.Count > 0)
                    {
                        ddl_SalesAgent.SelectedValue = Convert.ToString(dtsalesman.Rows[0]["SalesmanId"]);
                        Salesmanvalue = Convert.ToString(dtsalesman.Rows[0]["SalesmanId"]);
                    }

                    grid.JSProperties["cpRunningTotal"] = TotalQty + "~" + Amount + "~" + TaxAmount + "~" + AmountWithTaxValue + "~" + TotalAmt + "~" + Salesmanvalue + "~" + PosExistscheck;
                    //end rev rajdip

                    Session["SR_QuotationDetails"] = null;
                    Session["InlineRemarks"] = dt_Quotationtaggeddata.Tables[1];
                    grid.DataSource = GetInvoiceInfo(dt_QuotationDetails, IdKey);
                    grid.DataBind();
                }
                else
                {
                    grid.DataSource = null;
                    grid.DataBind();
                }
            }

            else if (strSplitCommand == "GridBlank")
            {

                lookup_quotation.GridView.Selection.UnselectAll();
                lookup_quotation.DataSource = null;
                lookup_quotation.DataBind();
                Session["SR_QuotationDetails"] = null;
                Session["SR_QuotationAddressDtl"] = null;
                Session["SR_WarehouseData"] = null;
                Session["SRwarehousedetailstemp"] = null;
                Session["SR_QuotationTaxDetails"] = null;
                grid.DataSource = null;
                grid.DataBind();
                grid.JSProperties["cpGridBlank"] = "1";
            }

            else if (strSplitCommand == "EInvoice")
            {
                string IrnOrgId = ConfigurationManager.AppSettings["IRNOrgID"];
                if (IrnOrgId == "1000687")
                {
                    UploadEinvoiceWelTel(e.Parameters.Split('~')[1]);
                }
                else
                {
                    UploadEinvoice(e.Parameters.Split('~')[1]);
                }
            }

        }
        private void UploadEinvoiceWelTel(string id)
        {
            grid.JSProperties["cpSucessIRN"] = null;
            CommonBL objBL = new CommonBL();
            string setting = objBL.GetSystemSettingsResult("IsBasicEInvoice");


            if (setting.ToUpper() == "YES")
            {
                List<EinvoiceModelWebtel> obj = new List<EinvoiceModelWebtel>();
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
                // string Branch_id = Convert.ToString(objDBEngineCredential.GetDataTable("SELECT Invoice_BranchId FROM TBL_TRANS_SALESINVOICE WHERE INVOICE_ID='" + id.ToString() + "'").Rows[0][0]);
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



                EinvoiceModelWebtel objInvoiceWebtel = new EinvoiceModelWebtel("1.1");


                objInvoiceWebtel.CDKey = IrnOrgId;
                objInvoiceWebtel.EInvUserName = IRN_API_UserId;
                objInvoiceWebtel.EInvPassword = IRN_API_Password;

                objInvoiceWebtel.EFUserName = IrnUser;
                objInvoiceWebtel.EFPassword = IrnPassword;
                objInvoiceWebtel.GSTIN = IRN_API_GSTIN;
                objInvoiceWebtel.GetQRImg = "1";
                objInvoiceWebtel.GetSignedInvoice = "1";

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
                objInvoiceWebtel.TranDtls = objTransporter;


                DocumentsDetails objDoc = new DocumentsDetails();
                objDoc.Dt = Convert.ToDateTime(Header.Rows[0]["Invoice_Date"]).ToString("dd/MM/yyyy");     // Form table invoice_Date DD/MM/YYYY format
                objDoc.No = Convert.ToString(Header.Rows[0]["Invoice_Number"]);   // Form table invoice_Number
                objDoc.Typ = "INV";  //INV-Invoice ,CRN-Credit Note, DBN-Debit Note
                objInvoiceWebtel.DocDtls = objDoc;


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
                objInvoiceWebtel.SellerDtls = objSeller;


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
                objInvoiceWebtel.BuyerDtls = objBuyer;


                objInvoiceWebtel.DispDtls = null;  // for now 
                objInvoiceWebtel.ShipDtls = null; ///Shipping details from invoice i.e tbl_trans_salesinvoiceadddress

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
                objInvoiceWebtel.ValDtls = objValue;

                ShipToDetails objShip = new ShipToDetails();
                objShip.Addr1 = Convert.ToString(ShipDetails.Rows[0]["Addr1"]); ;
                objShip.Addr2 = Convert.ToString(ShipDetails.Rows[0]["Addr2"]); ;
                objShip.Loc = Convert.ToString(ShipDetails.Rows[0]["Addr2"]); ;
                objShip.Gstin = Convert.ToString(ShipDetails.Rows[0]["Gstin"]); ;
                objShip.LglNm = Convert.ToString(ShipDetails.Rows[0]["LglNm"]); ;
                objShip.TrdNm = Convert.ToString(ShipDetails.Rows[0]["TrdNm"]); ;
                objShip.Pin = Convert.ToInt32(ShipDetails.Rows[0]["Pin"]); ;
                objShip.Stcd = Convert.ToString(ShipDetails.Rows[0]["Stcd"]); ;
                objInvoiceWebtel.ShipDtls = objShip;



                List<ProductList> objListProd = new List<ProductList>();

                foreach (DataRow dr in Products.Rows)
                {
                    ProductList objProd = new ProductList();

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

                    objProd.UnitPrice = Convert.ToDecimal(Convert.ToDecimal(dr["InvoiceDetails_SalePrice"]).ToString("0.00")); ;
                    objListProd.Add(objProd);
                }
                objInvoiceWebtel.ItemList = objListProd;

                obj.Add(objInvoiceWebtel);

                string success = "";
                string error = "";


                string IRNsuccess = "";
                string IRNerror = "";
                try
                {
                    IRN objIRN = new IRN();
                    using (var client = new HttpClient())
                    {
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                        SecurityProtocolType.Tls11 |
                        SecurityProtocolType.Tls12;

                        var json = JsonConvert.SerializeObject(objInvoiceWebtel, Formatting.Indented);
                        var stringContent = new StringContent(json);
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));




                        var content = new StringContent(json, Encoding.UTF8, "application/json");
                        var response = client.PostAsync(IrnGenerationUrl, content).Result;

                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            string jsonString = response.Content.ReadAsStringAsync().Result;
                            DBEngine objDb = new DBEngine();
                            JArray jsonResponse = JArray.Parse(jsonString);

                            var AckNo = "";
                            var AckDate = "";
                            var Irn = "";

                            foreach (var item in jsonResponse)
                            {

                                AckNo = item["AckNo"].ToString();
                                AckDate = item["AckDate"].ToString();
                                Irn = item["Irn"].ToString();
                                var SignedInvoice = item["SignedInvoice"].ToString();
                                var SignedQRCode = item["SignedQRCode"].ToString();
                                var EwbNo = item["EwbNo"].ToString();
                                var EwbDt = item["EwbDt"].ToString();
                                var IrnStatus = item["IrnStatus"].ToString();
                                var EwbValidTill = item["EwbValidTill"].ToString();
                                var ErrorCode = item["ErrorCode"].ToString();
                                var ErrorMessage = item["ErrorMessage"].ToString();
                                if (ErrorCode == "2150")
                                {
                                    JArray jRaces = (JArray)item["InfoDtls"];
                                    foreach (var rItem in jRaces)
                                    {
                                        AckNo = rItem["AckNo"].ToString();
                                        AckDate = rItem["AckDate"].ToString();
                                        Irn = rItem["Irn"].ToString();
                                    }
                                }
                                if (Convert.ToString(AckNo) != "0" && AckNo != null)
                                {

                                    objDb.GetDataTable("update TBL_TRANS_SALESRETURN SET AckNo='" + AckNo + "',AckDt='" + AckDate + "',Irn='" + Irn + "',SignedInvoice='" + SignedInvoice + "',SignedQRCode='" + SignedQRCode + "',Status='" + IrnStatus + "',EWayBillNumber = '" + EwbNo + "',EWayBillDate='" + EwbDt + "',EwayBill_ValidTill='" + EwbValidTill + "' where Return_Id='" + id.ToString() + "'");

                                    //IRNsuccess = IRNsuccess + "," + objInvoice.DocDtls.No;
                                    //success = success + "," + objInvoice.DocDtls.No;

                                    grid.JSProperties["cpSucessIRN"] = "Yes";
                                    grid.JSProperties["cpSucessIRNNumber"] = Irn;
                                }

                                else
                                {
                                    objDb.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SR' AND ERROR_TYPE='IRN_GEN'");

                                    // objDb.GetDataTable("update TBL_TRANS_SALESINVOICE SET AckNo='" + AckNo + "',AckDt='" + AckDate + "',Irn='" + Irn + "',SignedInvoice='" + SignedInvoice + "',SignedQRCode='" + SignedQRCode + "',Status='" + IrnStatus + "' where invoice_id='" + id.ToString() + "'");
                                    objDb.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SR','IRN_GEN','" + ErrorCode + "','" + ErrorMessage.Replace("'", "''") + "')");

                                    //error = error + "," + objInvoice.DocDtls.No;
                                    //IRNerror = IRNerror + "," + objInvoice.DocDtls.No;
                                    grid.JSProperties["cpSucessIRN"] = "No";
                                }


                                //success = success + "," + objInvoice.DocDtls.No;


                            }

                        }
                        else
                        {


                            EinvoiceError err = new EinvoiceError();
                            var jsonString = response.Content.ReadAsStringAsync().Result;

                            //err = response.Content.ReadAsAsync<EinvoiceError>().Result;
                            JArray jsonResponse = JArray.Parse(jsonString);

                            DBEngine objDB = new DBEngine();
                            objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SI' AND ERROR_TYPE='IRN_GEN'");

                            foreach (var item in jsonResponse)
                            {
                                var ErrorCode = item["ErrorCode"].ToString();
                                var ErrorMessage = item["ErrorMessage"].ToString();

                                objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','IRN_GEN','" + ErrorCode + "','" + ErrorMessage.Replace("'", "''") + "')");

                            }

                            grid.JSProperties["cpSucessIRN"] = "No";


                        }
                        //  }

                    }
                }
                catch (AggregateException err)
                {
                    DBEngine objDB = new DBEngine();
                    objDB.GetDataTable("DELETE FROM EInvoice_ErrorLog WHERE DOC_ID='" + id.ToString() + "' and DOC_TYPE='SI' AND ERROR_TYPE='IRN_GEN'");

                    foreach (var errInner in err.InnerExceptions)
                    {
                        objDB.GetDataTable("INSERT INTO EInvoice_ErrorLog(DOC_ID,DOC_TYPE,ERROR_TYPE,ERROR_CODE,ERROR_MSG) VALUES ('" + id.ToString() + "','SI','IRN_GEN','0','" + err.Message + "')");
                    }
                    error = error + "," + objInvoiceWebtel.DocDtls.No;
                }
            }
            else
            {
                Enrich objEnrich = new Enrich();
                meta objMeta = new meta();
                List<string> lstEmail = new List<string>();
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
                objDoc.Typ = "INV";  //INV-Invoice ,CRN-Credit Note, DBN-Debit Note
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
                objDoc.Typ = "CRN";  //INV-Invoice ,CRN-Credit Note, DBN-Debit Note
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
                    objProd.GstRt =Convert.ToDecimal(Convert.ToDecimal(dr["IGSTAmount"]).ToString("0.00"));
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
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-USER-TOKEN", authObj.data.token);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-ORG-ID", IrnOrgId);
                        client.DefaultRequestHeaders.Add("X-FLYNN-N-IRP-GSTIN",IRN_API_GSTIN);
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
                                objDb.GetDataTable("update TBL_TRANS_SALESRETURN SET AckNo='" + objIRNDetails.AckNo + "',AckDt='" + objIRNDetails.AckDt + "',Irn='" + objIRNDetails.Irn + "',SignedInvoice='" + objIRNDetails.SignedInvoice + "',SignedQRCode='" + objIRNDetails.SignedQRCode + "',Status='" + objIRNDetails.Status + "' where Return_id='" + id.ToString() + "'");
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
        public void ModifySalesReturn(string QuotationID, string strSchemeType, string SchemeId, string Adjustment_No, string strQuoteDate, string strCustomer, string strContactName, Int64 ProjId,
                                    string Reference, string PosForGst, string strBranch, string strAgents, string strCurrency, string strRate, string strTaxType, string strTaxCode,
                                    string strInvoiceComponent, string strInvoiceComponentDate, string strCashBank, string strDueDate,
                                    DataTable Productdt, DataTable TaxDetailTable, DataTable Warehousedt, DataTable SalesReturnTaxdt, DataTable BillAddressdt, string approveStatus, string ActionType,
                                    ref int strIsComplete, ref int strInvoiceID, string ReasonforReturn, DataTable PackingDetailsdt, DataTable MultiUOMDetails, DataTable dtAddlDesc,string drdTransCategory)
        {
            try
            {

                string DriverName = string.Empty;
                string PhoneNo = string.Empty;
                string VehicleNo = string.Empty;
                DataSet dsInst = new DataSet();
                //  SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                //Rev Tanmoy 08-08-2019 sp change
                // SqlCommand cmd = new SqlCommand("prc_CRMSalesReturn_AddEdit", con);

                 //Mantis Issue 24428
                //SqlCommand cmd = new SqlCommand("prc_CRMSalesReturn_AddEditNew", con);
               SqlCommand cmd = new SqlCommand("prc_CRMSalesReturn_AddEditNewAlt", con);

                //End Mantis Issue 24428

                //END Rev Tanmoy 08-08-2019 sp change
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", ActionType);
                cmd.Parameters.AddWithValue("@approveStatus", approveStatus);
                cmd.Parameters.AddWithValue("@SalesReturnID", QuotationID);
                //cmd.Parameters.AddWithValue("@SalesReturnNo", strQuoteNo);
                cmd.Parameters.AddWithValue("@SchemeId", SchemeId);
                cmd.Parameters.AddWithValue("@Adjustment_No", Adjustment_No);
                cmd.Parameters.AddWithValue("@SalesReturnDate", Convert.ToDateTime(strQuoteDate));
                cmd.Parameters.AddWithValue("@BranchID", strBranch);
                cmd.Parameters.AddWithValue("@CustomerID", strCustomer);
                cmd.Parameters.AddWithValue("@ContactPerson", strContactName);
                cmd.Parameters.AddWithValue("@Reference", Reference);
                cmd.Parameters.AddWithValue("@PosForGst", PosForGst);
                cmd.Parameters.AddWithValue("@Agents", strAgents);
                if (String.IsNullOrEmpty(strInvoiceComponent))
                { cmd.Parameters.AddWithValue("@ComponentType", ""); }
                else { cmd.Parameters.AddWithValue("@ComponentType", "SI"); }
                cmd.Parameters.AddWithValue("@InvoiceComponent", strInvoiceComponent);
                if (String.IsNullOrEmpty(strInvoiceComponentDate))
                { cmd.Parameters.AddWithValue("@InvoiceComponentDate", strInvoiceComponentDate); }
                else
                {
                    DateTime dt = DateTime.ParseExact(strInvoiceComponentDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    cmd.Parameters.AddWithValue("@InvoiceComponentDate", Convert.ToDateTime(dt).ToString("yyyy-MM-dd"));
                }
                DataTable Quotationdt = new DataTable();
                Quotationdt.Columns.Add("SrlNo", typeof(string));
                Quotationdt.Columns.Add("BatchWarehouseID", typeof(string));
                Quotationdt.Columns.Add("BatchWarehousedetailsID", typeof(string));
                Quotationdt.Columns.Add("BatchID", typeof(string));
                Quotationdt.Columns.Add("SerialID", typeof(string));
                Quotationdt.Columns.Add("WarehouseID", typeof(string));
                Quotationdt.Columns.Add("WarehouseName", typeof(string));
                Quotationdt.Columns.Add("BatchNo", typeof(string));
                Quotationdt.Columns.Add("SerialNo", typeof(string));
                Quotationdt.Columns.Add("MFGDate", typeof(string));
                Quotationdt.Columns.Add("ExpiryDate", typeof(string));
                Quotationdt.Columns.Add("Quantitysum", typeof(string));
                Quotationdt.Columns.Add("productid", typeof(string));
                Quotationdt.Columns.Add("Inventrytype", typeof(string));
                Quotationdt.Columns.Add("StockID", typeof(string));
                Quotationdt.Columns.Add("isnew", typeof(string));
                DataTable temtable = Quotationdt.DefaultView.ToTable(false, "SrlNo", "BatchWarehouseID", "BatchWarehousedetailsID", "BatchID", "SerialID", "WarehouseID", "WarehouseName", "BatchNo", "SerialNo", "MFGDate", "ExpiryDate", "Quantitysum", "productid", "Inventrytype", "StockID", "isnew");
                cmd.Parameters.AddWithValue("@udt_StockOpeningwarehousentrie", temtable);
                cmd.Parameters.AddWithValue("@CashBank", strCashBank);
                // cmd.Parameters.AddWithValue("@DueDate", Convert.ToDateTime(strDueDate));
                cmd.Parameters.AddWithValue("@ReturnPackingDetails", PackingDetailsdt); //Surojit 12-03-2019               
                cmd.Parameters.AddWithValue("@Currency", strCurrency);
                cmd.Parameters.AddWithValue("@Rate", strRate);
                cmd.Parameters.AddWithValue("@TaxType", strTaxType);
                cmd.Parameters.AddWithValue("@TaxCode", strTaxCode);
                cmd.Parameters.AddWithValue("@CompanyID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FinYear", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(Session["userid"]));
                cmd.Parameters.AddWithValue("@ProductDetails", Productdt);
                cmd.Parameters.AddWithValue("@TaxDetail", TaxDetailTable);
                cmd.Parameters.AddWithValue("@WarehouseDetail", Warehousedt);
                cmd.Parameters.AddWithValue("@SalesReturnTax", SalesReturnTaxdt);
                cmd.Parameters.AddWithValue("@BillAddress", BillAddressdt);
                cmd.Parameters.AddWithValue("@VehicleNo", VehicleNo);
                cmd.Parameters.AddWithValue("@DriverName", DriverName);
                cmd.Parameters.AddWithValue("@PhoneNo", PhoneNo);
                cmd.Parameters.AddWithValue("@ReasonforReturn", ReasonforReturn);
                cmd.Parameters.AddWithValue("@Project_Id", ProjId);
                cmd.Parameters.AddWithValue("@MultiUOMDetails", MultiUOMDetails);
                cmd.Parameters.AddWithValue("@udt_AddlDesc", dtAddlDesc);
                cmd.Parameters.AddWithValue("@TransacCategory", drdTransCategory);
                if (String.IsNullOrEmpty(strInvoiceComponent))
                { cmd.Parameters.AddWithValue("@ComponenyType", ""); }
                else { cmd.Parameters.AddWithValue("@ComponenyType", "SI"); }

                cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 50);
                cmd.Parameters.Add("@ReturnSalesReturnID", SqlDbType.VarChar, 50);
                cmd.Parameters.Add("@isEinvoice", SqlDbType.VarChar, 50);

                cmd.Parameters.AddWithValue("@IsManual", 0);
                cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;
                cmd.Parameters["@ReturnSalesReturnID"].Direction = ParameterDirection.Output;
                cmd.Parameters["@isEinvoice"].Direction = ParameterDirection.Output;

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
                    grid.JSProperties["cpQuotationId"] = strInvoiceID;

                }
                #region warehouse Update and delete

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
            //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

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
                                Taxes.calCulatedOn = Math.Round(finalCalCulatedOn);
                            }
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
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesReturn_Details");
            proc.AddVarcharPara("@Action", 500, "TaxDetails");
            proc.AddVarcharPara("@SalesReturnID", 500, Convert.ToString(Session["SR_ReturnID"]));
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
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
                if (Session["SR_TaxDetails"] == null)
                {
                    Session["SR_TaxDetails"] = GetTaxData(dt_PLQuote.Date.ToString("yyyy-MM-dd"));
                }

                if (Session["SR_TaxDetails"] != null)
                {
                    TaxDetailsdt = (DataTable)Session["SR_TaxDetails"];

                    #region Delete Igst,Cgst,Sgst respectively
                    //Get Company Gstin 09032017
                    string CompInternalId = Convert.ToString(Session["LastCompany"]);
                    string[] compGstin = oDBEngine.GetFieldValue1("tbl_master_company", "cmp_gstin", "cmp_internalid='" + CompInternalId + "'", 1);
                    string ShippingState = "";

                    #region ##### Added By : Samrat Roy -- For BillingShippingUserControl ######
                    string sstateCode = "";
                    //if (ddlPosGstSReturn.Value.ToString()=="")
                    //{
                    //if (ddlPosGstSReturn.Value.ToString() == "S")
                    //{
                    //    sstateCode = Purchase_BillingShipping.GeteShippingStateCode();
                    //}
                    //else
                    //{
                    //    sstateCode = Purchase_BillingShipping.GetBillingStateCode();
                    //}
                    // }
                    //Chinmoy stared
                    // sstateCode = Purchase_BillingShipping.GeteShippingStateCode();
                    string placeStateDetail = Convert.ToString(hdnPlaceOfSupply.Value);
                    string PosstateCode = "";

                    if (!string.IsNullOrEmpty(placeStateDetail))
                    {
                        string[] POSValue = (placeStateDetail.Split('~'));
                        PosstateCode = Convert.ToString(POSValue[2]);
                    }

                    sstateCode = PosstateCode;
                    //End

                    ShippingState = sstateCode;
                    if (ShippingState.Trim() != "")
                    {
                        ShippingState = ShippingState;
                    }
                    #region ##### Old Code -- BillingShipping ######
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
                    #endregion

                    #endregion

                    if (ShippingState.Trim() != "" && compGstin[0].Trim() != "")
                    {
                        if (compGstin.Length > 0)
                        {
                            if (compGstin[0].Substring(0, 2) == ShippingState)
                            {
                                //Check if the state is in union territories then only UTGST will apply
                                //   Chandigarh     Andaman and Nicobar Islands    DADRA & NAGAR HAVELI    DAMAN & DIU    Lakshadweep              PONDICHERRY
                                if (ShippingState == "4" || ShippingState == "35" || ShippingState == "26" || ShippingState == "25" || ShippingState == "31" || ShippingState == "34")
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
                if (Session["SR_TaxDetails"] != null)
                {
                    TaxDetailsdt = (DataTable)Session["SR_TaxDetails"];
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

                    Session["SR_TaxDetails"] = TaxDetailsdt;
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
            if (Session["SR_TaxDetails"] != null)
            {
                TaxDetailsdt = (DataTable)Session["SR_TaxDetails"];
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

            Session["SR_TaxDetails"] = TaxDetailsdt;

            gridTax.DataSource = GetTaxes(TaxDetailsdt);
            gridTax.DataBind();
        }
        protected void gridTax_DataBinding(object sender, EventArgs e)
        {
            if (Session["SR_TaxDetails"] != null)
            {
                DataTable TaxDetailsdt = (DataTable)Session["SR_TaxDetails"];
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
                        sqlQuery = "SELECT max(tjv.Return_Number) FROM tbl_trans_salesreturn tjv WHERE dbo.RegexMatch('";
                        if (prefLen > 0)
                            sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                        else if (scheme_type == 2)
                            sqlQuery += "^";
                        sqlQuery += "[0-9]{" + paddCounter + "}";
                        if (sufxLen > 0)
                            sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                        sqlQuery += "?$', LTRIM(RTRIM(tjv.Return_Number))) = 1 and Return_Number like '" + prefCompCode + "%'";
                        if (scheme_type == 2)
                            sqlQuery += " AND CONVERT(DATE, Return_Date) = CONVERT(DATE, GETDATE())";
                        dtC = oDBEngine.GetDataTable(sqlQuery);
                    }
                    else
                    {
                        //sqlQuery = "SELECT max(tjv.Return_Number) FROM tbl_trans_salesreturn tjv WHERE dbo.RegexMatch('";
                        //if (prefLen > 0)
                        //    sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                        //sqlQuery += "[0-9]*";
                        //if (sufxLen > 0)
                        //    sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                        ////sqlQuery += "?$', LTRIM(RTRIM(tjv.Return_Number))) = 1";
                        //sqlQuery += "?$', LTRIM(RTRIM(tjv.Return_Number))) = 1 and Return_Number like '" + prefCompCode + "%'";
                        //dtC = oDBEngine.GetDataTable(sqlQuery);



                        int i = startNo.Length;
                        while (i < paddCounter)
                        {
                            sqlQuery = "SELECT max(tjv.Return_Number) FROM tbl_trans_salesreturn tjv WHERE dbo.RegexMatch('";
                            if (prefLen > 0)
                                sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                            sqlQuery += "[0-9]{" + i + "}";
                            if (sufxLen > 0)
                                sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                            //sqlQuery += "?$', LTRIM(RTRIM(tjv.Return_Number))) = 1";
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
                            //sqlQuery += "?$', LTRIM(RTRIM(tjv.Return_Number))) = 1";
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
            proc.AddVarcharPara("@SalesReturnID", 500, Convert.ToString(Session["SR_ReturnID"]));
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
                DataTable MainTaxDataTable = (DataTable)Session["SR_FinalTaxRecord"];

                DataRow[] deletedRow = MainTaxDataTable.Select("SlNo=" + performpara.Split('~')[1]);
                if (deletedRow.Length > 0)
                {
                    foreach (DataRow dr in deletedRow)
                    {
                        MainTaxDataTable.Rows.Remove(dr);
                    }

                }

                Session["SR_FinalTaxRecord"] = MainTaxDataTable;
                GetStock(Convert.ToString(performpara.Split('~')[1]));
                // DeleteWarehouse(Convert.ToString(performpara.Split('~')[1]));
                DataTable taxDetails = (DataTable)Session["SR_TaxDetails"];
                if (taxDetails != null)
                {
                    foreach (DataRow dr in taxDetails.Rows)
                    {
                        dr["Amount"] = "0.00";
                    }
                    Session["SR_TaxDetails"] = taxDetails;
                }
            }
            else if (performpara.Split('~')[0] == "DeleteAllTax")
            {
                CreateDataTaxTable();

                DataTable taxDetails = (DataTable)Session["SR_TaxDetails"];

                if (taxDetails != null)
                {
                    foreach (DataRow dr in taxDetails.Rows)
                    {
                        dr["Amount"] = "0.00";
                    }
                    Session["SR_TaxDetails"] = taxDetails;
                }
            }
            else if (performpara.Split('~')[0] == "DelQtybySl")
            {
                DataTable MainTaxDataTable = (DataTable)Session["SR_FinalTaxRecord"];

                DataRow[] deletedRow = MainTaxDataTable.Select("SlNo=" + performpara.Split('~')[1]);
                if (deletedRow.Length > 0)
                {
                    foreach (DataRow dr in deletedRow)
                    {
                        dr.Delete();
                    }

                }
                MainTaxDataTable.AcceptChanges();
                Session["SR_FinalTaxRecord"] = MainTaxDataTable;
            }
            else
            {
                DataTable MainTaxDataTable = (DataTable)Session["SR_FinalTaxRecord"];

                DataRow[] deletedRow = MainTaxDataTable.Select("SlNo=" + performpara.Split('~')[1]);
                if (deletedRow.Length > 0)
                {
                    foreach (DataRow dr in deletedRow)
                    {
                        dr["Amount"] = "0.00";
                    }

                }

                Session["SR_FinalTaxRecord"] = MainTaxDataTable;

                DataTable taxDetails = (DataTable)Session["SR_TaxDetails"];

                if (taxDetails != null)
                {
                    foreach (DataRow dr in taxDetails.Rows)
                    {
                        dr["Amount"] = "0.00";
                    }
                    Session["SR_TaxDetails"] = taxDetails;
                }
            }

        }
        public double ReCalculateTaxAmount(string slno, double amount)
        {
            DataTable MainTaxDataTable = (DataTable)Session["SR_FinalTaxRecord"];
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
            Session["SR_FinalTaxRecord"] = MainTaxDataTable;

            return totalSum;

        }

        public void PopulateGSTCSTVATCombo(string quoteDate)
        {
            string LastCompany = "";
            if (Convert.ToString(Session["LastCompany"]) != null)
            {
                LastCompany = Convert.ToString(Session["LastCompany"]);
            }

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
            Session["SR_FinalTaxRecord"] = TaxRecord;
        }
        public static void CreateDataTaxTableForAjax()
        {
            DataTable TaxRecord = new DataTable();

            TaxRecord.Columns.Add("SlNo", typeof(System.Int32));
            TaxRecord.Columns.Add("TaxCode", typeof(System.String));
            TaxRecord.Columns.Add("AltTaxCode", typeof(System.String));
            TaxRecord.Columns.Add("Percentage", typeof(System.Decimal));
            TaxRecord.Columns.Add("Amount", typeof(System.Decimal));
            HttpContext.Current.Session["SR_FinalTaxRecord"] = TaxRecord;
        }
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
            proc.AddVarcharPara("@SalesReturnID", 500, Convert.ToString(Session["SR_ReturnID"]));
            ds = proc.GetDataSet();
            return ds;
        }
        public DataSet GetQuotationEditedTaxData()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesReturn_Details");
            proc.AddVarcharPara("@Action", 500, "ProductEditedTaxDetails");
            proc.AddVarcharPara("@SalesReturnID", 500, Convert.ToString(Session["SR_ReturnID"]));
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
            string retMsg = "";
            if (e.Parameters.Split('~')[0] == "SaveGST")
            {
                DataTable TaxRecord = (DataTable)Session["SR_FinalTaxRecord"];
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

                Session["SR_FinalTaxRecord"] = TaxRecord;
            }
            else
            {




                #region fetch All data For Tax

                //DataTable MainTaxDataTable = (DataTable)Session["SR_FinalTaxRecord"];


                //int slNo = Convert.ToInt32(HdSerialNo.Value);

                ////Get Gross Amount and Net Amount 
                //decimal ProdGrossAmt = Convert.ToDecimal(HdProdGrossAmt.Value);
                //decimal ProdNetAmt = Convert.ToDecimal(HdProdNetAmt.Value);

                //List<TaxDetails> TaxDetailsDetails = new List<TaxDetails>();

                ////Debjyoti 09032017
                //decimal totalParcentage = 0;

                DataTable ReturnTaxDetailsTable = new DataTable();








                DataTable taxDetail = new DataTable();
                DataTable MainTaxDataTable = (DataTable)Session["SR_FinalTaxRecord"];
                DataTable databaseReturnTable = (DataTable)Session["SR_QuotationTaxDetails"];

                //if (Convert.ToInt32(e.Parameters.Split('~')[1]) == 1)
                //    taxDetail = oDBEngine.GetDataTable("select TaxRates_ID as Taxes_ID,TaxRatesSchemeName as Taxes_Name,TaxCalculateMethods,tm.Taxes_Name as taxCodeName,tm.Taxes_ApplicableOn as 'ApplicableOn',isnull((select TaxRatesSchemeName from Config_TaxRates where TaxRates_ID=tm.Taxes_OtherTax),'')  as dependOn from Master_taxes tm inner join Config_TaxRates ct on tm.Taxes_ID=ct.TaxRates_TaxCode where tm.TaxTypeCode not in('G','V','C') and tm.TaxItemlevel='Y'");
                //else if (Convert.ToInt32(e.Parameters.Split('~')[1]) == 2)
                //taxDetail = oDBEngine.GetDataTable("select TaxRates_ID as Taxes_ID,TaxRatesSchemeName as Taxes_Name,TaxCalculateMethods,tm.Taxes_Name as taxCodeName,tm.Taxes_ApplicableOn as 'ApplicableOn',isnull((select TaxRatesSchemeName from Config_TaxRates where TaxRates_ID=tm.Taxes_OtherTax),'')  as dependOn from Master_taxes tm inner join Config_TaxRates ct on tm.Taxes_ID=ct.TaxRates_TaxCode where tm.TaxTypeCode not in('G','V','C') and tm.TaxItemlevel='Y'");

                ProcedureExecute proc = new ProcedureExecute("prc_PosSalesCRM_Details");
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

                //ShippingState = lblShippingStateText.Value;
                ShippingState = Purchase_BillingShipping.GeteShippingStateCode();
                //if (hdnPlaceOfSupply.Value != null && hdnPlaceOfSupply.Value != "")
                //{
                //    ShippingState = hdnPlaceOfSupply.Value.Split('~')[2].ToString();

                //}
                    
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

                        obj.Amount = Convert.ToDouble(obj.calCulatedOn * (Convert.ToDecimal(obj.TaxField) / 100));


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

                    DataTable TaxRecord = (DataTable)Session["SR_FinalTaxRecord"];


                    foreach (DataRow dr in taxDetail.Rows)
                    {
                        TaxDetails obj = new TaxDetails();
                        obj.Taxes_ID = Convert.ToInt32(dr["Taxes_ID"]);
                        obj.taxCodeName = Convert.ToString(dr["taxCodeName"]);

                        if (Convert.ToString(dr["TaxCalculateMethods"]) == "A")
                            obj.Taxes_Name = Convert.ToString(dr["Taxes_Name"]) + "(+)";
                        else
                            obj.Taxes_Name = Convert.ToString(dr["Taxes_Name"]) + "(-)";
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
                                obj.Amount = Math.Round(Convert.ToDouble((finalCalCulatedOn * Convert.ToDecimal(obj.TaxField)) / 100), 2);
                            }

                        }
                        else if (Convert.ToString(ddl_AmountAre.Value) == "1")
                        {

                            if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST")
                            {
                                decimal finalCalCulatedOn = 0;
                                finalCalCulatedOn = obj.calCulatedOn;
                                obj.calCulatedOn = finalCalCulatedOn;
                                obj.Amount = Math.Round(Convert.ToDouble((finalCalCulatedOn * Convert.ToDecimal(obj.TaxField)) / 100),2);
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
                            obj.Amount = Math.Round(Convert.ToDouble(filtronexsisting1[0]["Amount"]), 2);
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
                    Session["SR_FinalTaxRecord"] = TaxRecord;

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

            //int slNo = Convert.ToInt32(HdSerialNo.Value);
            //DataTable TaxRecord = (DataTable)Session["SR_FinalTaxRecord"];
            //foreach (var args in e.UpdateValues)
            //{

            //    string TaxCodeDes = Convert.ToString(args.NewValues["Taxes_Name"]);
            //    decimal Percentage = 0;
            //    Percentage = Convert.ToDecimal(args.NewValues["TaxField"]);
            //    decimal Amount = Convert.ToDecimal(args.NewValues["Amount"]);
            //    string TaxCode = "0";
            //    if (!Convert.ToString(args.Keys[0]).Contains('~'))
            //    {
            //        TaxCode = Convert.ToString(args.Keys[0]);
            //    }



            //    DataRow[] finalRow = TaxRecord.Select("SlNo=" + Convert.ToString(slNo) + " and TaxCode='" + TaxCode + "'");
            //    if (finalRow.Length > 0)
            //    {
            //        finalRow[0]["Percentage"] = Percentage;
            //        finalRow[0]["Amount"] = Amount;
            //        finalRow[0]["TaxCode"] = args.Keys[0];
            //        finalRow[0]["AltTaxCode"] = "0";

            //    }
            //    else
            //    {
            //        DataRow newRow = TaxRecord.NewRow();
            //        newRow["slNo"] = slNo;
            //        newRow["Percentage"] = Percentage;
            //        newRow["TaxCode"] = TaxCode;
            //        newRow["AltTaxCode"] = "0";
            //        newRow["Amount"] = Amount;
            //        TaxRecord.Rows.Add(newRow);
            //    }


            //}

            ////For GST/CST/VAT
            //if (cmbGstCstVat.Value != null)
            //{

            //    DataRow[] finalRow = TaxRecord.Select("SlNo=" + Convert.ToString(slNo) + " and TaxCode='0'");
            //    if (finalRow.Length > 0)
            //    {
            //        finalRow[0]["Percentage"] = Convert.ToString(cmbGstCstVat.Value).Split('~')[1];
            //        finalRow[0]["Amount"] = txtGstCstVat.Text;
            //        finalRow[0]["AltTaxCode"] = Convert.ToString(cmbGstCstVat.Value).Split('~')[0];

            //    }
            //    else
            //    {
            //        DataRow newRowGST = TaxRecord.NewRow();
            //        newRowGST["slNo"] = slNo;
            //        newRowGST["Percentage"] = Convert.ToString(cmbGstCstVat.Value).Split('~')[1];
            //        newRowGST["TaxCode"] = "0";
            //        newRowGST["AltTaxCode"] = Convert.ToString(cmbGstCstVat.Value).Split('~')[0];
            //        newRowGST["Amount"] = txtGstCstVat.Text;
            //        TaxRecord.Rows.Add(newRowGST);
            //    }
            //}
            ////End Here


            //Session["SR_FinalTaxRecord"] = TaxRecord;

            int slNo = Convert.ToInt32(HdSerialNo.Value);
            DataTable TaxRecord = (DataTable)Session["SR_FinalTaxRecord"];
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


            Session["SR_FinalTaxRecord"] = TaxRecord;



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

                //Rev Tanmoy 28-08-2019
                if (dsInst.Tables[0] != null && dsInst.Tables[0].Rows.Count > 0)
                {
                    aspxGridTax.JSProperties["cpTotalGST"] = dsInst.Tables[0].Rows[0][0];
                    aspxGridTax.JSProperties["cpGSTType"] = dsInst.Tables[0].Rows[0][1];
                }
                //Rev END
            }


        }
        protected void cmbGstCstVat_Callback(object sender, CallbackEventArgsBase e)
        {
            DateTime quoteDate = Convert.ToDateTime(dt_PLQuote.Date.ToString("yyyy-MM-dd"));
            PopulateGSTCSTVATCombo(quoteDate.ToString("yyyy-MM-dd"));
            CreateDataTaxTable();
        }

        protected void cmbGstCstVatcharge_Callback(object sender, CallbackEventArgsBase e)
        {
            Session["SR_TaxDetails"] = null;
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
            if (Session["sR_FinalTaxRecord"] != null)
            {
                TaxDetailTable = (DataTable)Session["SR_FinalTaxRecord"];

                var rows = TaxDetailTable.Select("SlNo ='" + SrlNo + "'");
                foreach (var row in rows)
                {
                    row.Delete();
                }
                TaxDetailTable.AcceptChanges();

                Session["SR_FinalTaxRecord"] = TaxDetailTable;
            }
        }
        public void UpdateTaxDetails(string oldSrlNo, string newSrlNo)
        {
            DataTable TaxDetailTable = new DataTable();
            if (Session["SR_FinalTaxRecord"] != null)
            {
                TaxDetailTable = (DataTable)Session["SR_FinalTaxRecord"];

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

                Session["SR_FinalTaxRecord"] = TaxDetailTable;
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



        [WebMethod]
        public static bool CheckUniqueCode(string ReturnNo)
        {
            bool flag = false;
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
            try
            {
                MShortNameCheckingBL objShortNameChecking = new MShortNameCheckingBL();
                flag = objShortNameChecking.CheckUnique(ReturnNo, "0", "SalesReturn");
            }
            catch (Exception ex)
            {

            }
            finally
            {
            }
            return flag;
        }
        #endregion Subhra Section end

        #region Sam Section Start

        #region PrePopulated Data If Page is not Post Back Section Start
        public void SetFinYearCurrentDate()
        {
            dt_PLQuote.EditFormatString = objConverter.GetDateFormat("Date");
            string fDate = null;
            string[] FinYEnd = Convert.ToString(Session["FinYearEnd"]).Split(' ');
            string FinYearEnd = FinYEnd[0];
            DateTime date3 = DateTime.ParseExact(FinYearEnd, "M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            ForJournalDate = Convert.ToString(date3);
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
                    }
                    else if (oDBEngine.GetDate().Date <= Convert.ToDateTime(Convert.ToString(Session["FinYearStartDate"])) && oDBEngine.GetDate().Date <= Convert.ToDateTime(Convert.ToString(Session["FinYearEndDate"])))
                    {
                        setdate = Convert.ToString(Convert.ToDateTime(Session["FinYearStartDate"]).Month) + "/" + Convert.ToString(Convert.ToDateTime(Session["FinYearStartDate"]).Day) + "/" + Convert.ToString(Convert.ToDateTime(Session["FinYearStartDate"]).Year);
                        dt_PLQuote.Value = DateTime.ParseExact(setdate, @"M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    }
                }
            }

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


            SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
            DataTable Schemadt = objSlaesActivitiesBL.GetNumberingSchema(strCompanyID, userbranchHierarchy, FinYear, "13", "Y");
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



        #region Header Portion Detail of the Page By Sam

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
            Session["SR_QuotationAddressDtl"] = null;
            Session["SR_BillingAddressLookup"] = null;
            Session["SR_ShippingAddressLookup"] = null;
            string WhichCall = e.Parameter.Split('~')[0];
            if (WhichCall == "BindContactPerson")
            {
                string InternalId = Convert.ToString(Convert.ToString(e.Parameter.Split('~')[1]));
                PopulateContactPersonOfCustomer(InternalId);

                DataTable dtDeuDate = objSalesReturnBL.GetCustomerDetails_InvoiceRelated(InternalId);
                foreach (DataRow dr in dtDeuDate.Rows)
                {
                    string strDueDate = Convert.ToString(dr["DueDate"]);
                    cmbContactPerson.JSProperties["cpDueDate"] = strDueDate;

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
                string strGSTN = string.Empty;
                DataTable GSTNTable = objSalesReturnBL.GetCustomerGSTIN(InternalId);

                if (GSTNTable.Rows.Count > 0)
                { strGSTN = Convert.ToString(GSTNTable.Rows[0]["CNT_GSTIN"]).Trim(); }

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
        protected void PopulateContactPersonOfCustomer(string InternalId)
        {
            string ContactPerson = "";
            DataTable dtContactPerson = new DataTable();
            SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
            dtContactPerson = objSlaesActivitiesBL.PopulateMultipleContactPersonOfCustomer(InternalId);
            if (dtContactPerson != null && dtContactPerson.Rows.Count > 0)
            {
                cmbContactPerson.TextField = "cp_name";
                cmbContactPerson.ValueField = "cp_id";
                cmbContactPerson.DataSource = dtContactPerson;
                cmbContactPerson.DataBind();
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

                #region ##### Added By : Samrat Roy -- For BillingShippingUserControl ######
                string sstateCode = "";
                //if (ddlPosGstSReturn.Value.ToString() == "S")
                //{
                //    sstateCode = Convert.ToString(Purchase_BillingShipping.GetShippingStateId());
                //}
                //else
                //{
                //    sstateCode = Convert.ToString(Purchase_BillingShipping.GetBillingStateId());
                //}
                //Chinmoy stared

                string placeStateDetail = Convert.ToString(hdnPlaceOfSupply.Value);
                // Tanmoy Start placeStateDetail blanck checking
                if (placeStateDetail != "")
                {
                    string[] POSValue = (placeStateDetail.Split('~'));
                    string PosstateId = Convert.ToString(POSValue[1]);
                    sstateCode = PosstateId;
                }
                // Tanmoy End
                // Chinmoy End

                ShippingState = sstateCode;
                if (ShippingState.Trim() != "")
                {
                    ShippingState = ShippingState;
                }


                #endregion

                if (ShippingState.Trim() != "" && compGstin[0].Trim() != "")
                {

                    if (compGstin.Length > 0)
                    {
                        if (compGstin[0].Substring(0, 2) == ShippingState)
                        {
                            //Check if the state is in union territories then only UTGST will apply
                            //   Chandigarh     Andaman and Nicobar Islands    DADRA & NAGAR HAVELI    DAMAN & DIU   Lakshadweep              PONDICHERRY
                            if (ShippingState == "4" || ShippingState == "35" || ShippingState == "26" || ShippingState == "25" || ShippingState == "31" || ShippingState == "34")
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
            proc.AddVarcharPara("@QuotationID", 500, Convert.ToString(Session["SR_ReturnID"]));
            dt = proc.GetTable();
            return dt;
        }

        #endregion Sam Section Start


        protected void EntityServerModeDataSalesReturn_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
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

            //BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

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
                hdnCustomerId.Value = Customer_Id;
                txtCustName.Text = Convert.ToString(SalesReturnEditdt.Rows[0]["Name"]);
                string Contact_Person_Id = Convert.ToString(SalesReturnEditdt.Rows[0]["Contact_Person_Id"]);
                string Quote_Reference = Convert.ToString(SalesReturnEditdt.Rows[0]["Return_Reference"]);
                string Currency_Id = Convert.ToString(SalesReturnEditdt.Rows[0]["Currency_Id"]);
                string Currency_Conversion_Rate = Convert.ToString(SalesReturnEditdt.Rows[0]["Currency_Conversion_Rate"]);
                string Tax_Option = Convert.ToString(SalesReturnEditdt.Rows[0]["Tax_Option"]);
                string Tax_Code = Convert.ToString(SalesReturnEditdt.Rows[0]["Tax_Code"]);
                string Quote_SalesmanId = Convert.ToString(SalesReturnEditdt.Rows[0]["Return_SalesmanId"]);
                string IsUsed = Convert.ToString(SalesReturnEditdt.Rows[0]["IsUsed"]);
                string TransCategory = Convert.ToString(SalesReturnEditdt.Rows[0]["TransCategory"]);
                drdTransCategory.SelectedValue = TransCategory;
                
                string CashBank_Code = Convert.ToString(SalesReturnEditdt.Rows[0]["CashBank_Code"]);
                string InvoiceCreatedFromDoc = Convert.ToString(SalesReturnEditdt.Rows[0]["ReturnCreatedFromDoc"]);
                string InvoiceCreatedFromDoc_Ids = Convert.ToString(SalesReturnEditdt.Rows[0]["ReturnCreatedFromDoc_Ids"]);
                string InvoiceCreatedFromDocDate = Convert.ToString(SalesReturnEditdt.Rows[0]["ReturnCreatedFromDocDate"]);
                string DueDate = Convert.ToString(SalesReturnEditdt.Rows[0]["DueDate"]);

                string VehicleNumber = Convert.ToString(SalesReturnEditdt.Rows[0]["VehicleNumber"]);
                string TransporterName = Convert.ToString(SalesReturnEditdt.Rows[0]["TransporterName"]);
                string TransporterPhone = Convert.ToString(SalesReturnEditdt.Rows[0]["TransporterPhone"]);
                string ReasonforReturn = Convert.ToString(SalesReturnEditdt.Rows[0]["ReasonforReturn"]);



                string placeOfSupply = Convert.ToString(PlaceOfSupplyData.Rows[0]["Name"]);
                int placeOfSupplyStateId = Convert.ToInt32(PlaceOfSupplyData.Rows[0]["StateId"]);
                string StateCode = Convert.ToString(PlaceOfSupplyData.Rows[0]["StateCode"]);

                ddlPosGstSReturn.DataSource = SalesReturnEdit.Tables[2];

                ddlPosGstSReturn.ValueField = "StateId";
                ddlPosGstSReturn.TextField = "Name";
                ddlPosGstSReturn.DataBind();

                string PosForGst = Convert.ToString(SalesReturnEditdt.Rows[0]["PosForGst"]);
                ddlPosGstSReturn.Value = placeOfSupply;
                hdnPlaceOfSupply.Value = placeOfSupply + "~" + placeOfSupplyStateId + "~" + StateCode;
                Purchase_BillingShipping.SetBillingShippingTable(SalesReturnEdit.Tables[1]);
                txtReasonforChange.Text = ReasonforReturn;


                txt_InvoiceDate.Text = InvoiceCreatedFromDocDate;
                //dt_SaleInvoiceDue.Date = Convert.ToDateTime(DueDate);


                DataTable dtt = GetProjectEditData();
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

                }
                else
                {
                    pageheaderContent.Attributes.Add("style", "display:block");
                    divOutstanding.Attributes.Add("style", "display:block");
                    lblOutstanding.Text = "0.0";
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
                // SetCustomerDDbyValue(Customer_Id);
                hdnCustomerId.Value = Customer_Id;
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
                dt_PLQuote.ClientEnabled = false;
                txtCustName.ClientEnabled = false;

            }
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

                string strReturnID = Convert.ToString(Session["SR_ReturnID"]);

                if (e.Parameter.Split('~')[3] == "DateCheck")
                {
                    lookup_quotation.GridView.Selection.UnselectAll();


                    ComponentQuotationPanel.JSProperties["cpDetails"] = Reference + "~" + Currency_Id + "~" + SalesmanId + "~" + ExpiryDate + "~" + CurrencyRate + "~" + Contact_person_id + "~" + Tax_option + "~" + Tax_Code;
                }
                DataTable ComponentTable = objSalesReturnBL.GetSRComponent(Customer, SalesReturnDate, strReturnID, branchId);
                lookup_quotation.GridView.Selection.CancelSelection();
                Session["SR_ComponentData"] = ComponentTable;
                lookup_quotation.DataSource = ComponentTable;
                lookup_quotation.DataBind();


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
                    if (lookup_quotation.GridView.GetSelectedFieldValues("ComponentDate").Count() != 0)
                    {
                        lookup_quotation.GridView.Selection.UnselectAll();
                        lookup_quotation.DataSource = null;
                        lookup_quotation.DataBind();
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
            if (Session["SR_ComponentData"] != null)
            {
                lookup_quotation.DataSource = (DataTable)Session["SR_ComponentData"];
            }
        }

        //protected void CustomerComboBox_Callback(object sender, CallbackEventArgsBase e)
        //{
        //    ASPxComboBox comboBox = (ASPxComboBox)sender;
        //    string Customerid = e.Parameter.Split('~')[0];
        //    DataTable dt = objSalesReturnBL.PopulateCustomerInEditMode(Convert.ToString(Customerid));
        //    comboBox.DataSource = dt;
        //    comboBox.DataBind();
        //    comboBox.Value = Customerid;

        //}
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
                DataTable invoicenumber = objSalesReturnBL.GetInvoiceNumber(Invoice_No);
                if (invoicenumber != null && invoicenumber.Rows.Count > 0)
                {
                    string placeOfSupply = Convert.ToString(invoicenumber.Rows[0]["Name"]);
                    int placeOfSupplyStateId = Convert.ToInt32(invoicenumber.Rows[0]["StateId"]);
                    string StateCode = Convert.ToString(invoicenumber.Rows[0]["StateCode"]);
                    if (!string.IsNullOrEmpty(placeOfSupply))
                    {
                        hdnPlaceOfSupply.Value = placeOfSupply + "~" + placeOfSupplyStateId + "~" + StateCode;


                    }
                }
            }


        }

        protected void grid_Products_DataBinding(Object sender, EventArgs e)
        {

            if (Session["SR_ProductDetails"] != null)
            {
                DataTable Quotationdt = (DataTable)Session["SR_ProductDetails"];
                DataView dvData = new DataView(Quotationdt);
                //dvData.RowFilter = "Status <> 'D'";
                grid_Products.DataSource = GetProductsInfo(dvData.ToTable());
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
                    DataTable dtTaxInvoice = new DataTable();
                    string IdKey = Convert.ToString(Request.QueryString["key"]);
                    if (!string.IsNullOrEmpty(IdKey))
                    {
                        if (IdKey != "ADD")
                        {
                            dt_QuotationDetails = objSalesReturnBL.GetInvoiceDetailsOnly(QuoComponent, "Edit");
                            dtTaxInvoice = objSalesReturnBL.GetTransitSalesInvoiceTaxType(QuoComponent, "Edit");
                        }
                        else
                        {
                            dt_QuotationDetails = objSalesReturnBL.GetInvoiceDetailsOnly(QuoComponent, "Add");
                            dtTaxInvoice = objSalesReturnBL.GetTransitSalesInvoiceTaxType(QuoComponent, "Add");
                        }

                    }
                    else
                    {
                        dt_QuotationDetails = objSalesReturnBL.GetInvoiceDetailsOnly(QuoComponent, "Add");
                        dtTaxInvoice = objSalesReturnBL.GetTransitSalesInvoiceTaxType(QuoComponent, "Add");
                    }
                    // Session["OrderDetails"] = null;
                    bool valproduct = true;
                    if (dtTaxInvoice != null && dtTaxInvoice.Rows.Count > 0)
                    {
                        string fstTaxtType = "";
                        foreach (DataRow item in dtTaxInvoice.Rows)
                        {
                            if (fstTaxtType == "")
                            {
                                fstTaxtType = item["TaxType"].ToString();
                                grid_Products.JSProperties["cpTaxType"] = "";
                            }
                            else
                            {
                                if (fstTaxtType != item["TaxType"].ToString())
                                {
                                    lookup_quotation.Text = "";
                                    lookup_quotation.GridView.Selection.CancelSelection();
                                    lookup_quotation.DataSource = (DataTable)Session["SR_ComponentData"];
                                    lookup_quotation.DataBind();
                                    grid_Products.JSProperties["cpTaxType"] = "refreshGrid";

                                    valproduct = false;
                                    break;
                                }
                                else
                                {
                                    grid_Products.JSProperties["cpTaxType"] = "";
                                }
                            }
                        }
                        ddl_AmountAre.ClientEnabled = true;
                        ddl_AmountAre.Value = fstTaxtType;
                        grid_Products.JSProperties["cpTaxTypeid"] = fstTaxtType;
                        // ddl_AmountAre.ClientEnabled = false;
                    }

                    if (valproduct)
                    {
                        Session["SR_ProductDetails"] = dt_QuotationDetails;
                        grid_Products.DataSource = GetProductsInfo(dt_QuotationDetails);
                        grid_Products.DataBind();
                    }
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
            public string IsInventory { get; set; }
            public string DetailsId { get; set; }
            public string CloseRate { get; set; }
            public string CloseRateFlag { get; set; }
           public string Remarks { get; set; }
        }
        public bool ImplementTaxonTagging(int id, int slno)
        {
            Boolean inUse = true;
            DataTable taxTable = (DataTable)Session["SR_FinalTaxRecord"];
            ProcedureExecute proc = new ProcedureExecute("prc_taxReturnTable");
            proc.AddVarcharPara("@Module", 20, "SalesReturn");
            proc.AddIntegerPara("@id", id);
            proc.AddIntegerPara("@slno", slno);
            proc.AddBooleanPara("@inUse", true, QueryParameterDirection.Output);
            DataTable returnedTable = proc.GetTable();
            inUse = Convert.ToBoolean(proc.GetParaValue("@inUse"));
            if (returnedTable != null)
                taxTable.Merge(returnedTable);

            Session["SR_FinalTaxRecord"] = taxTable;
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
                Orders.IsInventory = Convert.ToString(SalesInvoicedt1.Rows[i]["IsInventory"]);
                Orders.DetailsId = Convert.ToString(SalesInvoicedt1.Rows[i]["DetailsId"]);
                Orders.CloseRate = Convert.ToString(SalesInvoicedt1.Rows[i]["CloseRate"]);
                Orders.CloseRateFlag = Convert.ToString(SalesInvoicedt1.Rows[i]["CloseRate"]);
                if(dtC.Contains("Remarks"))
                {
                    Orders.Remarks = Convert.ToString(SalesInvoicedt1.Rows[i]["Remarks"]);
                }

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

                    DataRow[] existingRow = tempMainTax.Select("Taxes_ID='0'");
                    if (tempMainTax.Rows.Count == 1 && existingRow.Count() == 1)
                    {
                        if (existingRow.Length > 0)
                        {
                            tempMainTax.Rows.Remove(existingRow[0]);
                        }
                    }
                    tempMainTax.AcceptChanges();

                    if (tempMainTax != null && tempMainTax.Rows.Count > 0)
                    {
                        Session["SR_TaxDetails"] = tempMainTax;

                    }

                }
                else { Session["SR_TaxDetails"] = null; }

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
            SlsOreturnDT.Columns.Add("IsInventory", typeof(string));
            SlsOreturnDT.Columns.Add("DetailsId", typeof(string));
            SlsOreturnDT.Columns.Add("CloseRate", typeof(string));
            SlsOreturnDT.Columns.Add("CloseRateFlag", typeof(string));
            SlsOreturnDT.Columns.Add("Remarks", typeof(string));
            // Rev Bapi
            SlsOreturnDT.Columns.Add("Order_AltQuantity", typeof(string));
            SlsOreturnDT.Columns.Add("Order_AltUOM", typeof(string));

            //End Rev Bapi


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IsSuccess = true;
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
                      Convert.ToString(dt.Rows[i]["ProductName"]),
                        Convert.ToString(dt.Rows[i]["IsInventory"]),
                        Convert.ToString(dt.Rows[i]["DetailsId"]),
                        Convert.ToString(dt.Rows[i]["CloseRate"]),
                         Convert.ToString(dt.Rows[i]["CloseRate"]), Convert.ToString(dt.Rows[i]["Remarks"]),
                             // Rev Bapi
                              Convert.ToString(dt.Rows[i]["Order_AltQuantity"]), Convert.ToString(dt.Rows[i]["Order_AltUOM"])
            

            //End Rev Bapi
                              );
            }

            Session["SR_QuotationDetails"] = SlsOreturnDT;

            return IsSuccess;
        }//End
        #endregion
        protected void BindLookUp(string Customer, string SalesReturnDate, string branchId)
        {
            string strReturnID = Convert.ToString(Session["SR_ReturnID"]);
            DataTable ComponentTable = objSalesReturnBL.GetSRComponent(Customer, SalesReturnDate, strReturnID, branchId);
            lookup_quotation.GridView.Selection.CancelSelection();
            Session["SR_ComponentData"] = ComponentTable;
            lookup_quotation.DataSource = ComponentTable;
            lookup_quotation.DataBind();

        }
        protected void acbpCrpUdf_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            // BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();

            if (Request.QueryString["key"] == "ADD")
            {
                if (reCat.isAllMandetoryDone((DataTable)Session["UdfDataOnAdd"], "SR") == false)
                {
                    acbpCrpUdf.JSProperties["cpUDF"] = "false";

                }
                else
                {
                    acbpCrpUdf.JSProperties["cpUDF"] = "true";
                }


                acbpCrpUdf.JSProperties["cpTransport"] = "true";
                DataTable DT = objEngine.GetDataTable("Config_SystemSettings", " Variable_Value ", " Variable_Name='Transporter_SRMandatory' AND IsActive=1");
                if (DT != null && DT.Rows.Count > 0)
                {
                    string IsMandatory = Convert.ToString(DT.Rows[0]["Variable_Value"]).Trim();

                    if (Convert.ToString(Session["TransporterVisibilty"]).Trim() == "Yes")
                    {
                        if (IsMandatory == "Yes")
                        {
                            if (hfControlData.Value.Trim() == "")
                            {
                                acbpCrpUdf.JSProperties["cpTransport"] = "false";
                            }

                            else { acbpCrpUdf.JSProperties["cpTransport"] = "true"; }
                        }
                    }
                    else { acbpCrpUdf.JSProperties["cpTransport"] = "true"; }
                }

            }
            else
            {
                acbpCrpUdf.JSProperties["cpUDF"] = "true";
                acbpCrpUdf.JSProperties["cpTransport"] = "true";

                DataTable DT = objEngine.GetDataTable("Config_SystemSettings", " Variable_Value ", " Variable_Name='Transporter_SRMandatory' AND IsActive=1");
                if (DT != null && DT.Rows.Count > 0)
                {
                    string IsMandatory = Convert.ToString(DT.Rows[0]["Variable_Value"]).Trim();

                    if (Convert.ToString(Session["TransporterVisibilty"]).Trim() == "Yes")
                    {
                        if (IsMandatory == "Yes")
                        {
                            if (VehicleDetailsControl.GetControlValue("cmbTransporter") == "")
                            {
                                acbpCrpUdf.JSProperties["cpTransport"] = "false";
                            }

                            else { acbpCrpUdf.JSProperties["cpTransport"] = "true"; }
                        }
                    }
                    else { acbpCrpUdf.JSProperties["cpTransport"] = "true"; }
                }

            }
        }
        #region Warehouse Details

        public DataTable GetQuotationWarehouseData()
        {
            try
            {
                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesReturn_Details");
                proc.AddVarcharPara("@Action", 500, "SalesReturnWarehouse");
                proc.AddVarcharPara("@SalesReturnID", 500, Convert.ToString(Session["SR_ReturnID"]));
                proc.AddVarcharPara("@Type", 100, "SR");
                dt = proc.GetTable();

                if (Session["SR_QuotationDetails"] != null)
                {
                    DataTable SalesInvoicedt1 = (DataTable)Session["SR_QuotationDetails"];
                    for (int i = 0; i < SalesInvoicedt1.Rows.Count; i++)
                    {
                        string strSrNo = Convert.ToString(SalesInvoicedt1.Rows[i]["SrlNo"]);
                        string QuotationID = Convert.ToString(SalesInvoicedt1.Rows[i]["QuotationID"]);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            var rows = dt.Select("QuotationID ='" + QuotationID + "'");
                            foreach (var row in rows)
                            {
                                row["Product_SrlNo"] = strSrNo;
                            }
                            dt.AcceptChanges();
                        }
                    }

                    dt.Columns.Remove("QuotationID");
                }

                string strNewVal = "0", strOldVal = "";
                DataTable tempdt = new DataTable();

                if (dt != null && dt.Rows.Count > 0)
                {
                    tempdt = dt.Copy();
                    foreach (DataRow drr in tempdt.Rows)
                    {
                        strNewVal = Convert.ToString(drr["QuoteWarehouse_Id"]);

                        if (strNewVal == strOldVal)
                        {
                            drr["WarehouseName"] = "";
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

                    tempdt.Columns.Remove("QuoteWarehouse_Id");
                }

                Session["LoopSRWarehouse"] = Convert.ToString(Convert.ToInt32(strNewVal) + 1);
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

                Session["LoopSRWarehouse"] = Convert.ToString(Convert.ToInt32(strNewVal) + 1);
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
                // Changes
                string defaultWarehouse = "";
                bool IsExists = false;
                DataTable dtW = GetBranchWarehouseData();
                if (dtW != null && dtW.Rows.Count > 0)
                {
                    defaultWarehouse = Convert.ToString(dtW.Rows[0]["WarehouseID"]);
                }

                DataTable dt = GetWarehouseData();
                CmbWarehouse.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (defaultWarehouse == Convert.ToString(dt.Rows[i]["WarehouseID"]))
                    {
                        IsExists = true;
                    }

                    CmbWarehouse.Items.Add(Convert.ToString(dt.Rows[i]["WarehouseName"]), Convert.ToString(dt.Rows[i]["WarehouseID"]));
                }


            }
        }

        public DataTable GetBranchWarehouseData()
        {
            string strBranch = Convert.ToString(ddl_Branch.SelectedValue);

            DataTable dt = new DataTable();
            dt = oDBEngine.GetDataTable("select top 1 tmb.bui_id as WarehouseID,bui_Name as WarehouseName from tbl_master_building tmb inner join Master_Warehouse_Branchmap mwb on tmb.bui_id=mwb.Bui_id  Where isnull(mwb.Branch_id,0) in ('" + strBranch + "') order by bui_Name");
            return dt;
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

        [WebMethod]
        public static string GetSerialId(string id, string wareHouseStr, string BatchID, string ProducttId)
        {
            CRMSalesOrderDtlBL objCRMSalesOrderDtlBL = new CRMSalesOrderDtlBL();

            string[] Serials = id.Split(';');
            string Serial = Serials[0].TrimStart(';');
            string ispermission = string.Empty;
            string LastSerial = string.Empty;
            for (int i = 0; i < Serials.Length; i++)
            {
                LastSerial = Serials[Serials.Length - 1].TrimStart(';');

            }

            DataTable dt = new DataTable();
            if (!string.IsNullOrEmpty(LastSerial))
            {
                dt = objCRMSalesOrderDtlBL.GetSerialataOnFIFOBasis(wareHouseStr, BatchID, Serial, ProducttId, id, LastSerial);
            }
            else
            {
                dt = objCRMSalesOrderDtlBL.GetSerialataOnFIFOBasis(wareHouseStr, BatchID, Serial, ProducttId, id, Serial);
            }


            ispermission = Convert.ToString(dt.Rows[0].Field<Int32>("ResturnVal"));
            return Convert.ToString(ispermission);

        }

        protected void CmbSerial_Callback(object sender, CallbackEventArgsBase e)
        {
            string WhichCall = e.Parameter.Split('~')[0];
            if (WhichCall == "BindSerial")
            {
                string WarehouseID = Convert.ToString(e.Parameter.Split('~')[1]);

                if (WarehouseID != "null")
                {
                    string BatchID = Convert.ToString(e.Parameter.Split('~')[2]);
                    DataTable dt = GetSerialata(WarehouseID, BatchID, "");

                    if (Session["SR_WarehouseData"] != null)
                    {
                        DataTable Warehousedt = (DataTable)Session["SR_WarehouseData"];
                        DataTable tempdt = Warehousedt.DefaultView.ToTable(false, "SerialID");

                        foreach (DataRow dr in dt.Rows)
                        {
                            string SerialID = Convert.ToString(dr["SerialID"]);
                            DataRow[] rows = tempdt.Select("SerialID = '" + SerialID + "' AND SerialID<>'0'");

                            if (rows != null && rows.Length > 0)
                            {
                                dr.Delete();
                            }



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
            else if (WhichCall == "EditSerial")
            {
                string WarehouseID = Convert.ToString(e.Parameter.Split('~')[1]);
                string BatchID = Convert.ToString(e.Parameter.Split('~')[2]);
                string editSerialID = Convert.ToString(e.Parameter.Split('~')[3]);
                DataTable dt = GetSerialata(WarehouseID, BatchID, editSerialID);

                if (Session["SR_WarehouseData"] != null)
                {
                    DataTable Warehousedt = (DataTable)Session["SR_WarehouseData"];
                    DataTable tempdt = Warehousedt.DefaultView.ToTable(false, "SerialID");

                    foreach (DataRow dr in dt.Rows)
                    {
                        string SerialID = Convert.ToString(dr["SerialID"]);
                        DataRow[] rows = tempdt.Select("SerialID = '" + SerialID + "' AND SerialID not in ('0','" + editSerialID + "')");

                        if (rows != null && rows.Length > 0)
                        {
                            dr.Delete();
                        }

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
            DataTable dt = GetSerialata("", "", "");
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
                if (Session["SR_WarehouseData"] != null)
                {
                    Warehousedt = (DataTable)Session["SR_WarehouseData"];

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
                changeGridOrder();
            }
            else if (strSplitCommand == "SaveDisplay")
            {
                int loopId = Convert.ToInt32(Session["LoopSRWarehouse"]);

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
                string AltUOM = Convert.ToString(e.Parameters.Split('~')[10]);
                string AltUOMName = Convert.ToString(e.Parameters.Split('~')[11]);




                string Sales_UOM_Name = "", Sales_UOM_Code = "", Stk_UOM_Name = "", Stk_UOM_Code = "", Conversion_Multiplier = "", Trans_Stock = "0", MfgDate = "", ExpiryDate = "";
                GetProductType(ref Type);

                DataTable Warehousedt = new DataTable();
                if (Session["SR_WarehouseData"] != null)
                {
                    Warehousedt = (DataTable)Session["SR_WarehouseData"];
                    Warehousedt = Warehousedt.DefaultView.ToTable(false, "Product_SrlNo", "SrlNo", "WarehouseID", "WarehouseName",
                                "Quantity", "BatchID", "BatchNo", "SerialID", "SerialNo",
                                "SalesUOMName", "SalesUOMCode", "SalesQuantity", "StkUOMName", "StkUOMCode",
                                 "StkQuantity", "ConversionMultiplier", "AvailableQty", "BalancrStk", "MfgDate",
                                 "ExpiryDate", "LoopID", "TotalQuantity", "Status", "AltQty", "AltUOM", "AltUOMName");

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
                    Warehousedt.Columns.Add("AltUOM", typeof(string));
                    Warehousedt.Columns.Add("AltUOMName", typeof(string));
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

                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, SerialIDList.Length, BatchID, BatchName, strSrlID, strSrlName, Sales_UOM_Name, Sales_UOM_Code, SerialIDList.Length + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, SerialIDList.Length, "D", AltQty, AltUOM, AltUOMName);
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

                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, SerialIDList.Length, BatchID, BatchName, strSrlID, strSrlName, Sales_UOM_Name, Sales_UOM_Code, SerialIDList.Length + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, strLoopID, SerialIDList.Length, repute, AltQty, AltUOM, AltUOMName);
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
                            Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, Qty, BatchID, BatchName, "0", "", Sales_UOM_Name, Sales_UOM_Code, Convert.ToDecimal(Qty) + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, Qty, "D", AltQty, AltUOM, AltUOMName);
                        }
                        else
                        {
                            foreach (var row in updaterows)
                            {
                                decimal oldQuantity = Convert.ToDecimal(row["Quantity"]);
                                row["Quantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                decimal oldAltQty = Convert.ToDecimal(row["AltQty"]);
                                row["AltQty"] = (oldAltQty + Convert.ToDecimal(AltQty));
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
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, Qty, BatchID, BatchName, "0", "", Sales_UOM_Name, Sales_UOM_Code, Convert.ToDecimal(Qty) + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, Qty, "D", AltQty, AltUOM, AltUOMName);

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
                            Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, Qty, BatchID, BatchName, "0", "", Sales_UOM_Name, Sales_UOM_Code, Convert.ToDecimal(Qty) + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, Qty, "D", AltQty, AltUOM, AltUOMName);
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
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, Qty, BatchID, BatchName, "0", "", Sales_UOM_Name, Sales_UOM_Code, Convert.ToDecimal(Qty) + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, Qty, "D", AltQty, AltUOM, AltUOMName);
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
                            Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, Qty, BatchID, BatchName, "0", "", Sales_UOM_Name, Sales_UOM_Code, Convert.ToDecimal(Qty) + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, Qty, "D");
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
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, Qty, BatchID, BatchName, "0", "", Sales_UOM_Name, Sales_UOM_Code, Convert.ToDecimal(Qty) + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, Qty, "D", AltQty, AltUOM, AltUOMName);
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
                    Qty = "1";
                    decimal ConversionMultiplier = Convert.ToDecimal(Conversion_Multiplier);
                    string stkqtn = Convert.ToString(Math.Round((Convert.ToDecimal(Qty) * ConversionMultiplier), 2));
                    decimal BalanceStk = Convert.ToDecimal(Trans_Stock) - Convert.ToDecimal(stkqtn);
                    WarehouseID = "0";
                    BatchID = "0";

                    if (editWarehouseID != "0")
                    {
                        DataRow[] delResult = Warehousedt.Select("SrlNo ='" + editWarehouseID + "'");
                        foreach (DataRow delrow in delResult)
                        {
                            delrow.Delete();
                        }
                        Warehousedt.AcceptChanges();

                        bool isfirstRow = false;
                        var updateDeleterows = Warehousedt.Select("Product_SrlNo='" + ProductSerialID + "'");
                        if (updateDeleterows.Length > 0)
                        {
                            foreach (var row in updateDeleterows)
                            {
                                decimal oldQuantity = Convert.ToDecimal(row["Quantity"]);

                                row["Quantity"] = (oldQuantity - Convert.ToDecimal(1));
                                row["TotalQuantity"] = (oldQuantity - Convert.ToDecimal(1));
                                if (Convert.ToString(row["SalesQuantity"]) != "")
                                {
                                    isfirstRow = true;
                                    row["SalesQuantity"] = (oldQuantity - Convert.ToDecimal(1)) + " " + Sales_UOM_Name;
                                }
                            }

                            if (isfirstRow == false)
                            {
                                foreach (var row in updateDeleterows)
                                {
                                    decimal oldQuantity = Convert.ToDecimal(row["Quantity"]);
                                    row["SalesQuantity"] = oldQuantity + " " + Sales_UOM_Name;
                                }
                            }
                        }
                    }

                    for (int i = 0; i < SerialIDList.Length; i++)
                    {
                        string strSrlID = SerialIDList[i];
                        string strSrlName = SerialNameList[i];

                        if (editWarehouseID == "0")
                        {
                            var updaterows = Warehousedt.Select("Product_SrlNo='" + ProductSerialID + "'");
                            string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";

                            decimal oldQuantity = 0;
                            string whID = "1";

                            if (updaterows.Length > 0)
                            {
                                foreach (var row in updaterows)
                                {
                                    oldQuantity = Convert.ToDecimal(row["Quantity"]);
                                    whID = Convert.ToString(row["LoopID"]);

                                    row["Quantity"] = (oldQuantity + Convert.ToDecimal(1));
                                    row["TotalQuantity"] = (oldQuantity + Convert.ToDecimal(1));
                                    if (Convert.ToString(row["SalesQuantity"]) != "")
                                    {
                                        row["SalesQuantity"] = (oldQuantity + Convert.ToDecimal(1)) + " " + Sales_UOM_Name;
                                    }
                                }

                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, (oldQuantity + Convert.ToDecimal(1)), BatchID, BatchName, strSrlID, strSrlName, Sales_UOM_Name, Sales_UOM_Code, "", Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, whID, (oldQuantity + Convert.ToDecimal(1)), "D", AltQty, AltUOM, AltUOMName);
                            }
                            else
                            {
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, "1", BatchID, BatchName, strSrlID, strSrlName, Sales_UOM_Name, Sales_UOM_Code, "1" + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, "1", "D");
                            }
                        }
                        else
                        {
                            var rows = Warehousedt.Select("SerialID ='" + strSrlID + "' AND SrlNo='" + editWarehouseID + "'");
                            if (rows.Length == 0)
                            {

                                var updaterows = Warehousedt.Select("Product_SrlNo='" + ProductSerialID + "'");
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";

                                decimal oldQuantity = 0;
                                string whID = "1";

                                if (updaterows.Length > 0)
                                {
                                    foreach (var row in updaterows)
                                    {
                                        oldQuantity = Convert.ToDecimal(row["Quantity"]);
                                        whID = Convert.ToString(row["LoopID"]);

                                        row["Quantity"] = (oldQuantity + Convert.ToDecimal(1));
                                        row["TotalQuantity"] = (oldQuantity + Convert.ToDecimal(1));
                                        if (Convert.ToString(row["SalesQuantity"]) != "")
                                        {
                                            row["SalesQuantity"] = (oldQuantity + Convert.ToDecimal(1)) + " " + Sales_UOM_Name;
                                        }
                                    }

                                    Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, (oldQuantity + Convert.ToDecimal(1)), BatchID, BatchName, strSrlID, strSrlName, Sales_UOM_Name, Sales_UOM_Code, "", Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, whID, (oldQuantity + Convert.ToDecimal(1)), "D", AltQty, AltUOM, AltUOMName);
                                }
                                else
                                {
                                    Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, "1", BatchID, BatchName, strSrlID, strSrlName, Sales_UOM_Name, Sales_UOM_Code, "1" + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, "1", "D");
                                }
                            }
                        }
                    }
                }
                else if (Type == "WS")
                {
                    GetTotalStock(ref Trans_Stock, WarehouseID);
                    GetProductUOM(ref Sales_UOM_Name, ref Sales_UOM_Code, ref Stk_UOM_Name, ref Stk_UOM_Code, ref Conversion_Multiplier, ProductID);

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

                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, SerialIDList.Length, "0", BatchName, strSrlID, strSrlName, Sales_UOM_Name, Sales_UOM_Code, SerialIDList.Length + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, SerialIDList.Length, "D", AltQty, AltUOM, AltUOMName);
                            }
                            else
                            {
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, "", "", "0", "", strSrlID, strSrlName, "", Sales_UOM_Code, "", "", Stk_UOM_Code, "", "", "", "", "", "", loopId, SerialIDList.Length, "D", AltQty, AltUOM);
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

                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, SerialIDList.Length, "0", BatchName, strSrlID, strSrlName, Sales_UOM_Name, Sales_UOM_Code, SerialIDList.Length + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, SerialIDList.Length, repute, AltQty, AltUOM, AltUOMName);
                            }
                            else
                            {
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, "", "", "0", "", strSrlID, strSrlName, "", Sales_UOM_Code, "", "", Stk_UOM_Code, "", "", "", "", "", "", loopId, SerialIDList.Length, repute, AltQty, AltUOM);
                            }
                        }
                    }
                }
                else if (Type == "BS")
                {

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

                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, SerialIDList.Length, BatchID, BatchName, strSrlID, strSrlName, Sales_UOM_Name, Sales_UOM_Code, SerialIDList.Length + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, SerialIDList.Length, "D", AltQty, AltUOM, AltUOMName);
                            }
                            else
                            {
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, "", "", BatchID, "", strSrlID, strSrlName, "", Sales_UOM_Code, "", "", Stk_UOM_Code, "", "", "", "", "", "", loopId, SerialIDList.Length, "D", AltQty, AltUOM);
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

                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, SerialIDList.Length, BatchID, BatchName, strSrlID, strSrlName, Sales_UOM_Name, Sales_UOM_Code, SerialIDList.Length + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, SerialIDList.Length, repute, AltQty, AltUOM, AltUOMName);
                            }
                            else
                            {
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, "", "", BatchID, "", strSrlID, strSrlName, "", Sales_UOM_Code, "", "", Stk_UOM_Code, "", "", "", "", "", "", loopId, SerialIDList.Length, repute, AltQty, AltUOM);
                            }
                        }
                    }
                }


                if (IsDelete == true)
                {
                    DataRow[] delResult = Warehousedt.Select("SrlNo ='" + editWarehouseID + "'");
                    foreach (DataRow delrow in delResult)
                    {
                        delrow.Delete();
                    }
                    Warehousedt.AcceptChanges();
                }

                Session["SR_WarehouseData"] = Warehousedt;
                changeGridOrder();

                GrdWarehouse.DataSource = Warehousedt.DefaultView;
                GrdWarehouse.DataBind();

                Session["LoopSRWarehouse"] = loopId + 1;

                CmbWarehouse.SelectedIndex = -1;
                CmbBatch.SelectedIndex = -1;
            }
            else if (strSplitCommand == "Delete")
            {
                string strKey = e.Parameters.Split('~')[1];
                string strLoopID = "", strPreLoopID = "";

                DataTable Warehousedt = new DataTable();
                if (Session["SR_WarehouseData"] != null)
                {
                    Warehousedt = (DataTable)Session["SR_WarehouseData"];
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


                        }
                        else
                        {
                            if (delLoopID == strLoopID)
                            {
                                if (strKey == delSrlID)
                                {

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

                Session["SR_WarehouseData"] = Warehousedt;
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
                if (Session["SR_WarehouseData"] != null)
                {
                    DataTable Warehousedt = (DataTable)Session["SR_WarehouseData"];
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

                    Session["SR_WarehouseData"] = Warehousedt;
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

                if (Session["SR_WarehouseData"] != null)
                {
                    DataTable Warehousedt = (DataTable)Session["SR_WarehouseData"];

                    string strWarehouse = "", strBatchID = "", strSrlID = "", strQuantity = "0";
                    var rows = Warehousedt.Select(string.Format("SrlNo ='{0}'", SrlNo));
                    foreach (var dr in rows)
                    {
                        strWarehouse = (Convert.ToString(dr["WarehouseID"]) != "") ? Convert.ToString(dr["WarehouseID"]) : "0";
                        strBatchID = (Convert.ToString(dr["BatchID"]) != "") ? Convert.ToString(dr["BatchID"]) : "0";
                        strSrlID = (Convert.ToString(dr["SerialID"]) != "") ? Convert.ToString(dr["SerialID"]) : "0";
                        strQuantity = (Convert.ToString(dr["TotalQuantity"]) != "") ? Convert.ToString(dr["TotalQuantity"]) : "0";
                    }

                    CmbBatch.DataSource = GetBatchData(strWarehouse);
                    CmbBatch.DataBind();

                    CallbackPanel.JSProperties["cpEdit"] = strWarehouse + "~" + strBatchID + "~" + strSrlID + "~" + strQuantity;
                }
            }
        }
        public void changeGridOrder()
        {
            string Type = "";
            GetProductType(ref Type);
            if (Type == "W")
            {
                GrdWarehouse.Columns["WarehouseName"].Visible = true;
                GrdWarehouse.Columns["AvailableQty"].Visible = false;
                GrdWarehouse.Columns["BatchNo"].Visible = false;
                GrdWarehouse.Columns["MfgDate"].Visible = false;
                GrdWarehouse.Columns["ExpiryDate"].Visible = false;
                GrdWarehouse.Columns["SerialNo"].Visible = false;
            }
            else if (Type == "WB")
            {
                GrdWarehouse.Columns["WarehouseName"].Visible = true;
                GrdWarehouse.Columns["AvailableQty"].Visible = false;
                GrdWarehouse.Columns["BatchNo"].Visible = true;
                GrdWarehouse.Columns["MfgDate"].Visible = true;
                GrdWarehouse.Columns["ExpiryDate"].Visible = true;
                GrdWarehouse.Columns["SerialNo"].Visible = false;
            }
            else if (Type == "WBS")
            {
                GrdWarehouse.Columns["WarehouseName"].Visible = true;
                GrdWarehouse.Columns["AvailableQty"].Visible = false;
                GrdWarehouse.Columns["BatchNo"].Visible = true;
                GrdWarehouse.Columns["MfgDate"].Visible = true;
                GrdWarehouse.Columns["ExpiryDate"].Visible = true;
                GrdWarehouse.Columns["SerialNo"].Visible = true;
            }
            else if (Type == "B")
            {
                GrdWarehouse.Columns["WarehouseName"].Visible = false;
                GrdWarehouse.Columns["AvailableQty"].Visible = false;
                GrdWarehouse.Columns["BatchNo"].Visible = true;
                GrdWarehouse.Columns["MfgDate"].Visible = true;
                GrdWarehouse.Columns["ExpiryDate"].Visible = true;
                GrdWarehouse.Columns["SerialNo"].Visible = false;
            }
            else if (Type == "S")
            {
                GrdWarehouse.Columns["WarehouseName"].Visible = false;
                GrdWarehouse.Columns["AvailableQty"].Visible = false;
                GrdWarehouse.Columns["BatchNo"].Visible = false;
                GrdWarehouse.Columns["MfgDate"].Visible = false;
                GrdWarehouse.Columns["ExpiryDate"].Visible = false;
                GrdWarehouse.Columns["SerialNo"].Visible = true;
            }
            else if (Type == "WS")
            {
                GrdWarehouse.Columns["WarehouseName"].Visible = true;
                GrdWarehouse.Columns["AvailableQty"].Visible = false;
                GrdWarehouse.Columns["BatchNo"].Visible = false;
                GrdWarehouse.Columns["MfgDate"].Visible = false;
                GrdWarehouse.Columns["ExpiryDate"].Visible = false;
                GrdWarehouse.Columns["SerialNo"].Visible = true;
            }
            else if (Type == "BS")
            {
                GrdWarehouse.Columns["WarehouseName"].Visible = false;
                GrdWarehouse.Columns["AvailableQty"].Visible = false;
                GrdWarehouse.Columns["BatchNo"].Visible = true;
                GrdWarehouse.Columns["MfgDate"].Visible = true;
                GrdWarehouse.Columns["ExpiryDate"].Visible = true;
                GrdWarehouse.Columns["SerialNo"].Visible = true;
            }
        }
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
            if (Session["SR_WarehouseData"] != null)
            {
                string Type = "";
                GetProductType(ref Type);
                string SerialID = Convert.ToString(hdfProductSerialID.Value);
                DataTable Warehousedt = (DataTable)Session["SR_WarehouseData"];

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

        public void DeleteWarehouse(string SrlNo)
        {
            DataTable Warehousedt = new DataTable();
            if (Session["SR_WarehouseData"] != null)
            {
                Warehousedt = (DataTable)Session["SR_WarehouseData"];

                var rows = Warehousedt.Select(string.Format("Product_SrlNo ='{0}'", SrlNo));
                foreach (var row in rows)
                {
                    row.Delete();
                }
                Warehousedt.AcceptChanges();

                Session["SR_WarehouseData"] = Warehousedt;
            }
        }
        public void DeleteUnsaveWarehouse(string SrlNo)
        {
            DataTable Warehousedt = new DataTable();
            if (Session["SR_WarehouseData"] != null)
            {
                Warehousedt = (DataTable)Session["SR_WarehouseData"];

                var rows = Warehousedt.Select("Product_SrlNo ='" + SrlNo + "' AND Status='D'");
                foreach (var row in rows)
                {
                    row.Delete();
                }
                Warehousedt.AcceptChanges();

                Session["SR_WarehouseData"] = Warehousedt;
            }
        }
        public DataTable DeleteWarehouseBySrl(string strKey)
        {
            string strLoopID = "", strPreLoopID = "";

            DataTable Warehousedt = new DataTable();
            if (Session["SR_WarehouseData"] != null)
            {
                Warehousedt = (DataTable)Session["SR_WarehouseData"];
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

                    }
                    else
                    {
                        if (delLoopID == strLoopID)
                        {
                            if (strKey == delSrlID)
                            {

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
            if (Session["SR_WarehouseData"] != null)
            {
                Warehousedt = (DataTable)Session["SR_WarehouseData"];

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

                Session["SR_WarehouseData"] = Warehousedt;
            }
        }

        public DataTable GetWarehouseData()
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesReturn_Details");
            proc.AddVarcharPara("@Action", 500, "GetWareHouseDtlByProductID_From_RSC");
            proc.AddVarcharPara("@PID", 500, Convert.ToString(hdfProductID.Value));


            proc.AddVarcharPara("@Component_ID", 500, Convert.ToString(hdfComponentID.Value));
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetInvoiceWarehouse(string strInvoiceList)
        {
            try
            {
                string strCustomer = Convert.ToString(hdfLookupCustomer.Value);
                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesReturn_Details");
                proc.AddVarcharPara("@Action", 500, "SalesChallanWarehouseBySalesReturn");
                proc.AddVarcharPara("@InvoiceList", 3000, strInvoiceList);

                dt = proc.GetTable();

                string strNewVal = "0", strOldVal = "";
                DataTable tempdt = new DataTable();

                if (dt != null && dt.Rows.Count > 0)
                {
                    tempdt = dt.Copy();
                    foreach (DataRow drr in tempdt.Rows)
                    {
                        strNewVal = Convert.ToString(drr["InvoiceWarehouse_Id"]);

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
                    tempdt.Columns.Remove("InvoiceWarehouse_Id");
                }

                Session["LoopSRWarehouse"] = Convert.ToString(Convert.ToInt32(strNewVal) + 1);
                return tempdt;
            }
            catch
            {
                return null;
            }
        }
        #endregion
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
        public static object SetProjectCode(string OrderId, string TagDocType)
        {
            List<ProjectDetails> Detail = new List<ProjectDetails>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesReturn_Details");
                proc.AddVarcharPara("@Action", 500, "SalesInvoicetaggingProjectdata");
                proc.AddVarcharPara("@SalesReturnID", 500, OrderId);
                proc.AddVarcharPara("@TagDocType", 500, TagDocType);
                DataTable address = proc.GetTable();



                Detail = (from DataRow dr in address.Rows
                          select new ProjectDetails()

                          {
                              ProjectId = Convert.ToInt64(dr["ProjectId"]),
                              ProjectCode = Convert.ToString(dr["ProjectCode"])
                          }).ToList();
                return Detail;

            }
            return null;

        }


        [WebMethod]
        public static object GetCustomerStateCode(string CustomerID)
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

        public class SateCodeList
        {
            public string id { get; set; }
            public string state { get; set; }
            public string StateCode { get; set; }
            public string add_addressType { get; set;}
            public string TransactionType { get; set; }

        }

        public class ProjectDetails
        {
            public Int64 ProjectId { get; set; }
            public string ProjectCode { get; set; }
        }

        //#region predictive Customer
        //protected void ASPxComboBox_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        //{
        //    if (e.Filter != "")
        //    {
        //        ASPxComboBox comboBox = (ASPxComboBox)source;
        //        CustomerDataSource.SelectCommand =
        //               @"select cnt_internalid,uniquename,Name,Billing from (SELECT cnt_internalid,uniquename,Name,Billing, row_number()over(order by t.[Name]) as [rn]  from v_pos_customerDetails  as t where (([uniquename] + ' ' + [Name] ) LIKE @filter)) as st where st.[rn] between @startIndex and @endIndex";

        //        CustomerDataSource.SelectParameters.Clear();
        //        CustomerDataSource.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
        //        CustomerDataSource.SelectParameters.Add("startIndex", TypeCode.Int64, (e.BeginIndex + 1).ToString());
        //        CustomerDataSource.SelectParameters.Add("endIndex", TypeCode.Int64, (e.EndIndex + 1).ToString());
        //        comboBox.DataSource = CustomerDataSource;
        //        comboBox.DataBind();
        //    }
        //}


        //protected void ASPxComboBox_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e)
        //{
        //    long value = 0;
        //    if (e.Value == null || !Int64.TryParse(e.Value.ToString(), out value))
        //        return;
        //    ASPxComboBox comboBox = (ASPxComboBox)source;
        //    CustomerDataSource.SelectCommand = @"SELECT cnt_internalid,uniquename,Name,Billing FROM v_pos_customerDetails WHERE (cnt_internalid = @ID) ";

        //    CustomerDataSource.SelectParameters.Clear();
        //    CustomerDataSource.SelectParameters.Add("ID", TypeCode.String, e.Value.ToString());
        //    comboBox.DataSource = CustomerDataSource;
        //    comboBox.DataBind();
        //}

        //protected void SetCustomerDDbyValue(string customerId)
        //{

        //    CustomerDataSource.SelectCommand = @"SELECT cnt_internalid,uniquename,Name,Billing FROM v_pos_customerDetails WHERE (cnt_internalid = @ID) ";

        //    CustomerDataSource.SelectParameters.Clear();
        //    CustomerDataSource.SelectParameters.Add("ID", TypeCode.String, customerId);
        //    CustomerComboBox.DataSource = CustomerDataSource;
        //    CustomerComboBox.DataBind();
        //    CustomerComboBox.Value = customerId;

        //}

        //#endregion


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
        //Tanmoy Hierarchy End
        [WebMethod]
        public static object GetEINvDetails(string Id, string CustId)
        {
            List<EInvDEtailsSR> Detail = new List<EInvDEtailsSR>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                DataTable dtEInvoice = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_EInvoiceSaveCheck");
                proc.AddVarcharPara("@Action", 100, "EInvoicecheckForReturnModule");
                proc.AddIntegerPara("@BranchId", Convert.ToInt32(Id));
                proc.AddVarcharPara("@CustomerID", 200, CustId);
                dtEInvoice = proc.GetTable();
                Detail = (from DataRow dr in dtEInvoice.Rows
                          select new EInvDEtailsSR()

                          {
                              BranchCompany = Convert.ToString(dr["DefaultAddress"]),
                              CustomerId = Convert.ToString(dr["custId"]),
                              BillingStatus = Convert.ToString(dr["BillingStatus"]),
                              ShippingStatus = Convert.ToString(dr["ShippingStatus"])
                          }).ToList();
                return Detail;

            }
            return null;

        }
        public class EInvDEtailsSR
        {
            public string BranchCompany { get; set; }
            public string CustomerId { get; set; }
            public string BillingStatus { get; set; }
            public string ShippingStatus { get; set; }

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
        public static Int32 GetQuantityfromSL(string SLNo, string val)
        {

            DataTable dt = new DataTable();
            int SLVal = 0;
            if (HttpContext.Current.Session["MultiUOMData"] != null)
            {
                DataRow[] MultiUoMresult;
                dt = (DataTable)HttpContext.Current.Session["MultiUOMData"];
                if (val == "1")
                {
                    // Mantis Issue 24428
                    //MultiUoMresult = dt.Select("DetailsId ='" + SLNo + "'");
                    MultiUoMresult = dt.Select("DetailsId ='" + SLNo + "' and UpdateRow ='True'");
                    // End of Mantis Issue 24428
                }
                else
                {
                    // Mantis Issue 24428
                   // MultiUoMresult = dt.Select("SrlNo ='" + SLNo + "'");
                    MultiUoMresult = dt.Select("SrlNo ='" + SLNo + "' and UpdateRow ='True'");
                    // End of Mantis Issue 24428
                }
                SLVal = MultiUoMresult.Length;


            }

            return SLVal;
        }

        //Tanmoy Hierarchy
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

        [WebMethod(EnableSession = true)]
        public static object taxUpdatePanel_Callback(string Action, string srl, string prodid)
        {
            string output = "200";
            try
            {
                //string performpara = e.Parameter;
                if (Action == "DelProdbySl")
                {
                    DataTable MainTaxDataTable = (DataTable)HttpContext.Current.Session["SR_FinalTaxRecord"];

                    DataRow[] deletedRow = MainTaxDataTable.Select("SlNo=" + srl);
                    if (deletedRow.Length > 0)
                    {
                        foreach (DataRow dr in deletedRow)
                        {
                            MainTaxDataTable.Rows.Remove(dr);
                        }

                    }

                    HttpContext.Current.Session["SR_FinalTaxRecord"] = MainTaxDataTable;
                    // GetStock(Convert.ToString(srl));
                    // DeleteWarehouse(Convert.ToString(performpara.Split('~')[1]));
                    DataTable taxDetails = (DataTable)HttpContext.Current.Session["SR_TaxDetails"];
                    if (taxDetails != null)
                    {
                        foreach (DataRow dr in taxDetails.Rows)
                        {
                            dr["Amount"] = "0.00";
                        }
                        HttpContext.Current.Session["SR_TaxDetails"] = taxDetails;
                    }
                }
                else if (Action == "DeleteAllTax")
                {
                    CreateDataTaxTableForAjax();



                    DataTable taxDetails = (DataTable)HttpContext.Current.Session["SR_TaxDetails"];

                    if (taxDetails != null)
                    {
                        foreach (DataRow dr in taxDetails.Rows)
                        {
                            dr["Amount"] = "0.00";
                        }
                        HttpContext.Current.Session["SR_TaxDetails"] = taxDetails;
                    }
                }
                else if (Action == "DelQtybySl")
                {
                    DataTable MainTaxDataTable = (DataTable)HttpContext.Current.Session["SR_FinalTaxRecord"];

                    DataRow[] deletedRow = MainTaxDataTable.Select("SlNo=" + srl);
                    if (deletedRow.Length > 0)
                    {
                        foreach (DataRow dr in deletedRow)
                        {
                            dr.Delete();
                        }

                    }
                    MainTaxDataTable.AcceptChanges();
                    HttpContext.Current.Session["SR_FinalTaxRecord"] = MainTaxDataTable;
                }
                else
                {
                    DataTable MainTaxDataTable = (DataTable)HttpContext.Current.Session["SR_FinalTaxRecord"];

                    DataRow[] deletedRow = MainTaxDataTable.Select("SlNo=" + srl);
                    if (deletedRow.Length > 0)
                    {
                        foreach (DataRow dr in deletedRow)
                        {
                            dr["Amount"] = "0.00";
                        }

                    }

                    HttpContext.Current.Session["SR_FinalTaxRecord"] = MainTaxDataTable;

                    DataTable taxDetails = (DataTable)HttpContext.Current.Session["SR_TaxDetails"];

                    if (taxDetails != null)
                    {
                        foreach (DataRow dr in taxDetails.Rows)
                        {
                            dr["Amount"] = "0.00";
                        }
                        HttpContext.Current.Session["SR_TaxDetails"] = taxDetails;
                    }
                }
            }
            catch
            {
                output = "201";

            }


            return output;

        }

        [WebMethod]
        public static object DocWiseSimilarProjectCheck(string quote_Id, string Doctype)
        {
            string returnValue = "0";
            if (HttpContext.Current.Session["userid"] != null)
            {
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                //quote_Id = quote_Id.Replace("'", "''");

                DataTable dtMainAccount = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_ProjectSalesReturn_Details");
                proc.AddVarcharPara("@Action", 100, "GetSimilarProjectCheckforReturn");
                proc.AddVarcharPara("@TagDocType", 100, Doctype);
                proc.AddVarcharPara("@SelectedComponentList", 500, quote_Id);
                proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
                proc.RunActionQuery();
                returnValue = Convert.ToString(proc.GetParaValue("@ReturnValue"));

            }
            return returnValue;

        }
        //End Tanmoy Hierarchy

        [WebMethod]
        public static object GetContactPerson(string CustomerID)
        {
            PurchaseOrderBL objPurchaseOrderBL = new PurchaseOrderBL();
            List<ddlContactPerson> listCotact = new List<ddlContactPerson>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                DataTable dtContactPerson = new DataTable();
                SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
                dtContactPerson = objSlaesActivitiesBL.PopulateContactPersonOfCustomer(CustomerID);
                if (dtContactPerson != null && dtContactPerson.Rows.Count > 0)
                {
                    DataView dvData = new DataView(dtContactPerson);
                    listCotact = (from DataRow dr in dvData.ToTable().Rows
                                  select new ddlContactPerson()
                                  {
                                      Id = dr["add_id"].ToString(),
                                      Name = dr["contactperson"].ToString(),

                                  }).ToList();
                }
            }

            return listCotact;
        }


        public class ddlContactPerson
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }
    }
}