/*************************************************************************************************************************************************
 *  Rev 1.0     Priti    V2.0.40     05-10-2023      Data Freeze Required for Project Sale Invoice & Project Purchase Invoice. Mantis:26854
 *************************************************************************************************************************************************/

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
using UtilityLayer;
using EntityLayer.MailingSystem;
using BusinessLogicLayer.EmailDetails;
using System.Drawing;
using ERP.OMS.Tax_Details.ClassFile;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using ERP.Models;
using System.Globalization;

namespace ERP.OMS.Management.Activities
{
    public partial class ProjectPurchaseOrder : System.Web.UI.Page
    {
        Export.ExportToPDF exportToPDF = new Export.ExportToPDF();
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        clsDropDownList oclsDropDownList = new clsDropDownList();
        BusinessLogicLayer.Contact oContactGeneralBL = new BusinessLogicLayer.Contact();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        string UniquePurchaseNumber = string.Empty;
        SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
        ProjectPurchaseOrderBL objPurchaseOrderBL = new ProjectPurchaseOrderBL();
        BusinessLogicLayer.RemarkCategoryBL reCat = new BusinessLogicLayer.RemarkCategoryBL();
        GSTtaxDetails gstTaxDetails = new GSTtaxDetails();
        public string pageAccess = "";
        static string ForJournalDate = null;
        DataTable Remarks = null;
        CommonBL cbl = new CommonBL();
        string Doctype = string.Empty;
        #region Sandip Section For Approval Section Start
        ERPDocPendingApprovalBL objERPDocPendingApproval = new ERPDocPendingApprovalBL();
        #endregion Sandip Section For Approval Dtl Section End

        #region Sandip Section For Approval Section Start
        public void IsExistsDocumentInERPDocApproveStatus(string orderId)
        {
            string editable = "";
            string status = "";
            DataTable dt = new DataTable();
            int Id = Convert.ToInt32(orderId);
            dt = objERPDocPendingApproval.IsExistsDocumentInERPDocApproveStatus(Id, 7); // 7 for Purchase Order
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
                    btn_SaveRecords.Visible = false;
                }
                else
                {
                    lbl_quotestatusmsg.Visible = false;
                    btn_SaveRecords.Visible = true;
                    btn_SaveRecords.Visible = true;
                }
            }
        }

        //public void GetEditablePermission()
        //{
        //    if (Request.QueryString["Permission"] != null)
        //    {
        //        if (Convert.ToString(Request.QueryString["Permission"]) == "1")
        //        {

        //            btn_SaveRecords.Visible = false;
        //            btn_SaveRecords.Visible = false;
        //        }
        //        else if (Convert.ToString(Request.QueryString["Permission"]) == "2")
        //        {

        //            btn_SaveRecords.Visible = true;
        //            btn_SaveRecords.Visible = true;
        //        }
        //        else if (Convert.ToString(Request.QueryString["Permission"]) == "3")
        //        {

        //            btn_SaveRecords.Visible = true;
        //            ASPxButton1.Visible = true;
        //        }
        //    }
        //}

        #endregion Sandip Section For Approval Dtl Section End
        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {
            #region Sandip Section For Approval Section Start
            //Rev Debashis
            //if (Request.QueryString.AllKeys.Contains("status"))
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
            #endregion Sandip Section For Approval Dtl Section End
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Init(object sender, EventArgs e) // lead add
        {

            //SqlSchematype.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            //SqlIndentRequisitionNo.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
           // Sqlvendor.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlCurrency.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
           // DS_AmountAre.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            //CountrySelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            //StateSelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            //SelectCity.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            //SelectArea.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            //SelectPin.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            DS_Branch.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void grid_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (e.Column.FieldName == "SrlNo")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "Indent_Num")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "ProductName")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "gvColDiscription")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "gvColUOM")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "gvColTaxAmount")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "gvColTotalAmountINR")
            {
                e.Editor.Enabled = true;
            }
            else
            {
                e.Editor.ReadOnly = false;
            }


        }
        protected void Page_Load(object sender, EventArgs e)
        {

            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/PurchaseOrder.aspx");
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }       

            if (ddlInventory.SelectedValue == "Y")
            {                
                taggingList.ClientEnabled = true;
            }
            else
            {                
                taggingList.ClientEnabled = true;
            }
            if (Request.QueryString["key"] != null && Request.QueryString["key"] != "ADD")
            {
                string strOrderId1 = Convert.ToString(Request.QueryString["key"]);
                #region Sandip Section For Approval Dtl Section Start
                if (Request.QueryString["status"] == null)
                {
                    IsExistsDocumentInERPDocApproveStatus(strOrderId1);
                }
                #endregion Sandip Section For Approval Dtl Section End

            }
            string ProjectMandatoryInEntry = cbl.GetSystemSettingsResult("ProjectMandatoryInEntry");
            string ApproveProjPurchaseOrder = cbl.GetSystemSettingsResult("ApproveProjPurchaseOrder");
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


            if (!String.IsNullOrEmpty(ApproveProjPurchaseOrder))
            {
                if (ApproveProjPurchaseOrder == "Yes")
                {
                    hdnApprovalReqInq.Value = "1";
                }
                else if (ApproveProjPurchaseOrder.ToUpper().Trim() == "NO")
                {
                    hdnApprovalReqInq.Value = "0";
                    dvAppRejRemarks.Style.Add("display", "none");
                    dvRevision.Style.Add("display", "none");
                    dvRevisionDate.Style.Add("display", "none");
                    dvApprove.Style.Add("display", "none");
                    dvReject.Style.Add("display", "none");
                }
            }

            //Tanmoy Hierarchy
            string HierarchySelectInEntryModule = cbl.GetSystemSettingsResult("Show_Hierarchy");
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
            //Tanmoy Hierarchy End
            //Rev start Tanmoy
            string ProjectSelectInEntryModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");
            if (!String.IsNullOrEmpty(ProjectSelectInEntryModule))
            {
                if (ProjectSelectInEntryModule == "Yes")
                {
                    hdnProjectSelectInEntryModule.Value = "1";
                    lookup_Project.ClientVisible = true;
                    lblProject.Visible = true;

                    taggingGrid.Columns[4].Visible = true;
                    taggingGrid.Columns[5].Visible = true;
                }
                else if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
                {
                    hdnProjectSelectInEntryModule.Value = "0";
                    lookup_Project.ClientVisible = false;
                    lblProject.Visible = false;

                    taggingGrid.Columns[4].Width = 0;
                    taggingGrid.Columns[5].Width = 0;
                }
            }
            //End Rev Tanmoy
            string ProjectTermsCondition = cbl.GetSystemSettingsResult("ShowProjectTCProjectPurchaseOrder");
            if (!String.IsNullOrEmpty(ProjectTermsCondition))
            {
                if (ProjectTermsCondition == "Yes")
                {
                    dvSOTC.Style.Add("display", "inline-block");
                    hdnProjectSOTC.Value = "1";
                }
                else if (ProjectTermsCondition.ToUpper().Trim() == "NO")
                {
                    dvSOTC.Style.Add("display", "none");
                    hdnProjectSOTC.Value = "0";
                }
            }

            string RevisionRequiredEveryAfterApproval = cbl.GetSystemSettingsResult("RevisionRequiredEveryAfterApproval");
            hdnRevisionRequiredEveryAfterApproval.Value = RevisionRequiredEveryAfterApproval;
            // Rev 1.0
            DataTable dtposTime = oDBEngine.GetDataTable("SELECT  top 1 convert(varchar(50),Lock_Fromdate,110) LockCon_Fromdate,convert(varchar(50),Lock_Todate,110) LockCon_Todate,convert(varchar(10),Lock_Fromdate,105) Data_Fromdate,convert(varchar(10),Lock_Todate,105) Data_Todate FROM Trans_LockConfigouration_Details WHERE  Type='Add' and Module_Id=64");
            if (dtposTime != null && dtposTime.Rows.Count > 0)
            {
                hdnLockFromDate.Value = Convert.ToString(dtposTime.Rows[0]["LockCon_Fromdate"]);
                hdnLockToDate.Value = Convert.ToString(dtposTime.Rows[0]["LockCon_Todate"]);
                hdnDatafrom.Value = Convert.ToString(dtposTime.Rows[0]["Data_Fromdate"]);
                hdnDatato.Value = Convert.ToString(dtposTime.Rows[0]["Data_Todate"]);
            }
            // End of Rev 1.0
            if (!IsPostBack)
            {

                #region NewTaxblock
                string ItemLevelTaxDetails = string.Empty; string HSNCodewisetaxSchemid = string.Empty; string BranchWiseStateTax = string.Empty; string StateCodeWiseStateIDTax = string.Empty;
                gstTaxDetails.GetTaxData(ref ItemLevelTaxDetails, ref HSNCodewisetaxSchemid, ref BranchWiseStateTax, ref StateCodeWiseStateIDTax, "P");
                HDItemLevelTaxDetails.Value = ItemLevelTaxDetails;
                HDHSNCodewisetaxSchemid.Value = HSNCodewisetaxSchemid;
                HDBranchWiseStateTax.Value = BranchWiseStateTax;
                HDStateCodeWiseStateIDTax.Value = StateCodeWiseStateIDTax;

                #endregion

                // Cross Branch Section by Sam on 10052017 Start
                string branchid = "";
                string branch = "";
                if (Request.QueryString.AllKeys.Contains("status"))
                {
                    DataTable dt = objPurchaseOrderBL.GetBranchIdByPOID(Convert.ToString(Request.QueryString["key"]));
                    branchid = Convert.ToString(dt.Rows[0]["br"]);
                    branch = oDBEngine.getBranch(branchid, "") + branchid;

                    HttpContext.Current.Session["userbranchHierarchy"] = branch;
                    Session["LastCompany"] = Convert.ToString(dt.Rows[0]["comp"]);
                    Session["LastFinYear"] = Convert.ToString(dt.Rows[0]["finyear"]);

                }
                else
                {
                    branchid = Convert.ToString(Session["userbranchID"]);
                    branch = oDBEngine.getBranch(branchid, "") + branchid;
                }
                // Cross Branch Section by Sam on 10052017 End
                #region Sandip Section For Checking User Level for Allow Edit to logging User or Not
                //GetEditablePermission();

                //CommonBL cbl = new CommonBL();
                //string ISLigherpage = cbl.GetSystemSettingsResult("LighterVendorEntryPage");
                //if (!String.IsNullOrEmpty(ISLigherpage))
                //{
                //    if (ISLigherpage == "Yes")
                //    {
                //        hidIsLigherContactPage.Value = "1";
                //    }
                //} 

                



                if (Request.QueryString.AllKeys.Contains("status"))
                {
                    if (Convert.ToString(Request.QueryString["status"]) == "PO")
                    {
                        divcross.Visible = false;
                        btn_SaveRecords.Visible = false;
                        ApprovalCross.Visible = false;
                        ddl_Branch.Enabled = false;
                    }
                    else
                    {
                        divcross.Visible = false;
                        btn_SaveRecords.Visible = false;
                        ApprovalCross.Visible = true;
                        ddl_Branch.Enabled = false;
                    }

                }
                else
                {
                    divcross.Visible = true;
                    btn_SaveRecords.Visible = true;
                    ApprovalCross.Visible = false;
                    ddl_Branch.Enabled = true;
                }
                #endregion Sandip Section
                if (Request.QueryString["op"] == "yes")
                {
                    hdnOpening.Value = "Opening";
                    taggingList.ClientEnabled = true;
                }
                this.Session["LastCompany1"] = Session["LastCompany"];
                this.Session["LastFinYear1"] = Session["LastFinYear"];
                this.Session["userbranch"] = Session["userbranchID"];
                SetFinYearCurrentDate();
                string branchHierchy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                Task PopulateStockTrialDataTask = new Task(() => GetAllDropDownDetailForPurchaseOrder(branchHierchy));
                PopulateStockTrialDataTask.RunSynchronously();
                BindBranch();
                bindHierarchy();
                ddlHierarchy.Enabled = false;
                string finyear = Convert.ToString(Session["LastFinYear"]);
                Session["CustomerDetail"] = null;
                Session["PurchaseOrder_Id"] = null;
                Session["ProductOrderDetails"] = null;
                Session["LoopWarehouse"] = 1;
                Session["EditQuoteId"] = null;
                Session["POTaxDetails"] = null;
                Session["PurchaseOrderAddressDtl"] = null;
                Session["POwarehousedetailstemp"] = null;
                Session["POwarehousedetailstempUpdate"] = null;
                Session["POwarehousedetailstempDelete"] = null;
                Session["InlineRemarks"] = null;
                PopulateGSTCSTVATCombo(DateTime.Now.ToString("yyyy-MM-dd"));
                PopulateChargeGSTCSTVATCombo(DateTime.Now.ToString("yyyy-MM-dd"));
                if (Request.QueryString["key"] == "ADD")
                {
                    ddlInventory.SelectedValue = Convert.ToString(Request.QueryString["InvType"]);
                    ddlInventory.Enabled = false;

                    //Rev Tanmoy allow indent tag for all case

                    //if (Convert.ToString(Request.QueryString["InvType"]) == "Y") taggingList.ClientEnabled = true;
                    //else taggingList.ClientEnabled = false;

                    if (Convert.ToString(Request.QueryString["InvType"]) == "Y") taggingList.ClientEnabled = true;
                    else taggingList.ClientEnabled = true;

                    ////Rev end Tanmoy allow indent tag for all case

                    if (!String.IsNullOrEmpty(Convert.ToString(Session["userbranchID"])))
                    {
                        string strdefaultBranch = Convert.ToString(Session["userbranchID"]);
                        ddl_Branch.SelectedValue = strdefaultBranch;
                    }

                    #region To Show By Default Cursor after SAVE AND NEW

                    if (Session["SaveModePO"] != null)  // it has been removed from coding side of Quotation list 
                    {
                        if (Session["schemavaluePO"] != null)  // it has been removed from coding side of Quotation list 
                        {
                            ddl_numberingScheme.SelectedValue = Convert.ToString(Session["schemavaluePO"]); // it has been removed from coding side of Quotation list 
                            string branchId = Convert.ToString(ddl_numberingScheme.SelectedValue);
                            string strbranchID = branchId.Split('~')[3];
                            ddl_Branch.SelectedValue = strbranchID;
                            ddl_Branch.Enabled = false;
                            ddlInventory.SelectedValue = Convert.ToString(Session["POType"]);

                        }
                        if (Convert.ToString(Session["SaveModePO"]) == "A")
                        {
                            dt_PLQuote.Focus();
                            txtVoucherNo.Enabled = false;
                            txtVoucherNo.Text = "Auto";
                        }
                        else if (Convert.ToString(Session["SaveModePO"]) == "M")
                        {
                            txtVoucherNo.Enabled = true;
                            txtVoucherNo.Text = "";
                            txtVoucherNo.Focus();
                        }
                    }
                    else
                    {
                        ddlInventory.Focus();
                    }
                    #endregion To Show By Default Cursor after SAVE AND NEW

                    IndentTaggingMendatory();
                    ViewState["ActionType"] = "Add";
                    Keyval_internalId.Value = "Add";
                    hdnPageStatus1.Value = "update";

                    if (!String.IsNullOrEmpty(Convert.ToString(Session["LocalCurrency"])))
                    {
                        string CompID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                        string LocalCurrency = Convert.ToString(Session["LocalCurrency"]);
                        string basedCurrency = Convert.ToString(LocalCurrency.Split('~')[0]);
                        string CurrencyId = Convert.ToString(basedCurrency[0]);
                        ddl_Currency.SelectedValue = CurrencyId;
                        int ConvertedCurrencyId = Convert.ToInt32(CurrencyId);

                        DataTable dt = objPurchaseOrderBL.GetCurrentConvertedRate(Convert.ToInt16(CurrencyId), ConvertedCurrencyId, CompID);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            txt_Rate.Text = Convert.ToString(dt.Rows[0]["PurchaseRate"]);
                        }
                    }
                    hdnPageStatus.Value = "first";
                    ddl_AmountAre.Value = "3";
                    ddl_VatGstCst.ClientEnabled = false;
                    //.................Tax..............
                    Session["POTaxDetails"] = null;
                    CreateDataTaxTable();
                    //..........End Tax-----------


                    DataTable Transactiondt = CreateTempTable("Transaction");
                    Transactiondt.Rows.Add("1", "1", "", "", "0.0000", "", "", "0.0000", "", "0.000", "0.00", "0.00", "0.00", "0.00", "0", "0", "", "", "I");

                    Session["ProductOrderDetails"] = Transactiondt;
                    grid.DataSource = GetPurchaseOrderBatch(Transactiondt);
                    grid.DataBind();

                }
                else
                {
                    txtVoucherNo.Enabled = false;
                    ddlInventory.Enabled = false;
                    ddl_Branch.Enabled = false;
                    txtVendorName.ClientEnabled = false;
                    dt_PLQuote.ClientEnabled = false;
                    Keyval_internalId.Value = "PurchaseOrder" + Request.QueryString["key"];
                    hdnPageStatus.Value = "Quoteupdate";
                    hdnEditPageStatus.Value = "Quoteupdate";
                    lblHeading.Text = "Modify Project Purchase Order";
                    divNumberingScheme.Style.Add("display", "none");
                    lbl_NumberingScheme.Visible = false;
                    ddl_numberingScheme.Visible = false;
                    hdnEditOrderId.Value=Convert.ToString(Request.QueryString["key"]);
                    Session["PurchaseOrder_Id"] = Request.QueryString["key"];
                    ViewState["ActionType"] = "Edit";
                    btn_SaveRecords.Visible = false;
                    // chinmoy edited below code
                    if (hdnProjectSOTC.Value == "1")
                    {
                        Doctype = "ProjPO";
                        ProjectTermsConditionsControl.SetEditTermsCoditionData(Convert.ToString(hdnEditOrderId.Value), Doctype);
                    }


                    DataSet Ds = GetPurchaseOrderData();
                    Session["ProductOrderDetails"] = Ds.Tables[0];
                    Session["InlineRemarks"]=Ds.Tables[3];
                    Purchase_BillingShipping.SetBillingShippingTable(Ds.Tables[1]);

                    FillGrid();
                    //.................Tax..............
                    Session["POTaxDetails"] = GetTaxDataWithGST(GetTaxData(dt_PLQuote.Date.ToString("yyyy-MM-dd")));
                    DataTable quotetable = GetQuotationEditedTaxData().Tables[0];
                    if (quotetable == null)
                    {
                        CreateDataTaxTable();
                    }
                    else
                    {
                        Session["PurchaseOrderFinalTaxRecord"] = quotetable;
                    }
                    //..........End Tax-----------
                    grid.DataSource = GetPurchaseOrderBatch();
                    grid.DataBind();

                    if (Request.QueryString["AppStat"] != null && Request.QueryString["AppStat"] == "ProjApprove")
                    {
                        lblHeading.Text = "Approve Project Purchase Order";
                        hdnProjectApproval.Value = "ProjApprove";
                        btn_SaveRecords.Visible = false;
                        btnSaveExit.Visible = false;
                      
                    }

                    //Chinmoy comment for rev dicument number start 03-12-2019
                    //if (IsPITransactionExist(Request.QueryString["key"]))
                    //{
                    //    grid.JSProperties["cpBtnVisible"] = "false";

                    //    btn_SaveRecords.Visible = false;
                    //    btnSaveExit.Visible = false;
                    //    tagged.Style.Add("display", "block");
                    //}
                    //end

                    #region Samrat Roy -- Hide Save Button in Edit Mode
                    if (Request.QueryString["req"] != null && Request.QueryString["req"] == "V")
                    {
                        //chinmoy comment for Rev start 03-12-2019
                        lblHeading.Text = "View Project Purchase Order";
                        lbl_quotestatusmsg.Text = "*** View Mode Only";
                        btn_SaveRecords.Visible = false;
                        btnSaveExit.Visible = false;
                        lbl_quotestatusmsg.Visible = true;
                        //End
                    }
                    #endregion

                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "GridCallBack()", true);
                }

                ///Mail Visibility
                VisiblitySendEmail();


                //Surojit 26-02-2019

                MasterSettings objmaster = new MasterSettings();
                hdnConvertionOverideVisible.Value = objmaster.GetSettings("ConvertionOverideVisible");
                hdnShowUOMConversionInEntry.Value = objmaster.GetSettings("ShowUOMConversionInEntry");

                //Surojit 26-02-2019

            }

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
        //End Tanmoy Hierarchy

        public DataTable CreateTempTable(string Type)
        {

            DataTable PurchaseOrderdt = new DataTable();

            if (Type == "Transaction")
            {
                PurchaseOrderdt.Columns.Add("SrlNo", typeof(string));
                PurchaseOrderdt.Columns.Add("OrderDetails_Id", typeof(string));
                PurchaseOrderdt.Columns.Add("ProductID", typeof(string));
                PurchaseOrderdt.Columns.Add("Description", typeof(string));
                PurchaseOrderdt.Columns.Add("Quantity", typeof(string));
                PurchaseOrderdt.Columns.Add("UOM", typeof(string));
                PurchaseOrderdt.Columns.Add("Warehouse", typeof(string));
                PurchaseOrderdt.Columns.Add("StockQuantity", typeof(string));
                PurchaseOrderdt.Columns.Add("StockUOM", typeof(string));
                PurchaseOrderdt.Columns.Add("PurchasePrice", typeof(string));
                PurchaseOrderdt.Columns.Add("Discount", typeof(string));
                PurchaseOrderdt.Columns.Add("Amount", typeof(string));
                PurchaseOrderdt.Columns.Add("TaxAmount", typeof(string));
                PurchaseOrderdt.Columns.Add("TotalAmount", typeof(string));
                PurchaseOrderdt.Columns.Add("Status", typeof(string));
                PurchaseOrderdt.Columns.Add("Indent_No", typeof(string));
                PurchaseOrderdt.Columns.Add("ProductName", typeof(string));
                PurchaseOrderdt.Columns.Add("IsComponentProduct", typeof(string));
                PurchaseOrderdt.Columns.Add("IsLinkedProduct", typeof(string));
                PurchaseOrderdt.Columns.Add("PurchaseOrder_InlineRemarks", typeof(string));
                PurchaseOrderdt.Columns.Add("BalQty", typeof(string));
                PurchaseOrderdt.Columns.Add("Document_DetailsID", typeof(string));

            }
            return PurchaseOrderdt;
        }
        public void IndentTaggingMendatory()
        {
            DataTable TaggingMen = objPurchaseOrderBL.GetIndentTaggingMendatory();
            if (TaggingMen.Rows.Count > 0)
            {
                string strVariableName = Convert.ToString(TaggingMen.Rows[0]["Variable_Value"]).Trim();
                if (strVariableName == "Yes")
                {
                    hdfTagMendatory.Value = "Yes";
                }
                else
                {
                    hdfTagMendatory.Value = "No";
                }
            }
        }
        public void BindBranch()
        {
            DS_Branch.SelectCommand = "select BANKBRANCH_ID,BANKBRANCH_NAME from ( SELECT BRANCH_id AS BANKBRANCH_ID , RTRIM(BRANCH_DESCRIPTION)+' ['+ISNULL(RTRIM(BRANCH_CODE),'')+']' AS BANKBRANCH_NAME  FROM TBL_MASTER_BRANCH where BRANCH_id in(" + Convert.ToString(Session["userbranchHierarchy"]) + "))tbl" +
            " order by BANKBRANCH_NAME asc";
            ddl_Branch.DataBind();
        }
        private bool IsPITransactionExist(string Poid)
        {
            bool IsExist = false;
            if (Poid != "" && Convert.ToString(Poid).Trim() != "")
            {
                DataTable dt = new DataTable();
                dt = objPurchaseOrderBL.CheckPOTraanaction(Poid);
                if (dt.Rows.Count > 0 && Convert.ToString(dt.Rows[0]["isexist"]) != "0")
                {
                    IsExist = true;
                }
            }

            return IsExist;
        }
        public void Bind_NumberingScheme()
        {
            string strCompanyID = Convert.ToString(Session["LastCompany"]);
            string strBranchID = Convert.ToString(Session["userbranchID"]);
            string FinYear = Convert.ToString(Session["LastFinYear"]);
            string userbranchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);
            SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
            DataTable Schemadt = objSlaesActivitiesBL.GetNumberingSchema(strCompanyID, userbranchHierarchy, FinYear, "117", "N");
            if (Schemadt != null && Schemadt.Rows.Count > 0)
            {
                ddl_numberingScheme.DataTextField = "SchemaName";
                ddl_numberingScheme.DataValueField = "Id";
                ddl_numberingScheme.DataSource = Schemadt;
                ddl_numberingScheme.DataBind();
            }
        }
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
        public void PopulateCustomerDetail()
        {
            //if (Session["POVendorsDetail"] == null)
            //{
            //    DataTable dtCustomer = new DataTable();
            //    dtCustomer = objPurchaseOrderBL.PopulateVendorsDetail();

            //    if (dtCustomer != null && dtCustomer.Rows.Count > 0)
            //    {
            //        lookup_Customer.DataSource = dtCustomer;
            //        lookup_Customer.DataBind();
            //        Session["POVendorsDetail"] = dtCustomer;
            //    }
            //}
            //else
            //{
            //    lookup_Customer.DataSource = (DataTable)Session["POVendorsDetail"];
            //    lookup_Customer.DataBind();
            //}
            DataTable dtCustomer = new DataTable();
            dtCustomer = objPurchaseOrderBL.PopulateVendorsDetail();

            if (dtCustomer != null && dtCustomer.Rows.Count > 0)
            {
                //lookup_Customer.DataSource = dtCustomer;
                //lookup_Customer.DataBind();
                Session["POVendorsDetail"] = dtCustomer;
            }

        }



        #region  Class
        public class PurchaseOrderList
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
           // public string Quotation_No { get; set; }
            public string ProductName { get; set; }
            public string Indent_Num { get; set; }
            public Int64 Indent { get; set; }
            public string IsComponentProduct { get; set; }
            public string IsLinkedProduct { get; set; }
            public string PurchaseOrder_InlineRemarks { get; set; }
            //chinmoy added 03-12-2019 start
            public string BalQty { get; set; }
            //End
            public string Document_DetailsID { get; set; }
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
            public string Key_UniqueId { get; set; }
            public string Product_Shortname { get; set; }
            public string ProductName { get; set; }
            public string QuoteDetails_Id { get; set; }
            public string Indent_Num { get; set; }
            public Int64 Indent { get; set; }
            public string Document_DetailsID { get; set; }
            public string PurchaseOrder_InlineRemarks { get; set; }

        }
        public class Product
        {
            public string ProductID { get; set; }
            public string ProductName { get; set; }
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
        public class Taxes
        {
            public string TaxID { get; set; }
            public string TaxName { get; set; }
            public string Percentage { get; set; }
            public string Amount { get; set; }
            public decimal calCulatedOn { get; set; }
        }
        #endregion
        public void FillGrid()
        {
            DataTable PurchaseOrderEditdt = GetPurchaseOrderEditData();
            DataTable PurchaseOrderDocType = PurchaseOrderDocTypeEditDetails();
            if (PurchaseOrderEditdt != null && PurchaseOrderEditdt.Rows.Count > 0)
            {
                string PurchaseOrder_Number = Convert.ToString(PurchaseOrderEditdt.Rows[0]["PurchaseOrder_Number"]);//0
                string PurchaseOrder_IndentId = Convert.ToString(PurchaseOrderEditdt.Rows[0]["PurchaseOrder_IndentIds"]);//1
                string PurchaseOrder_BranchId = Convert.ToString(PurchaseOrderEditdt.Rows[0]["PurchaseOrder_BranchId"]);//2
                TermsConditionsControl.SetBranchId(PurchaseOrder_BranchId);  // Mantis Issue #16920
                string PurchaseOrder_Date = Convert.ToString(PurchaseOrderEditdt.Rows[0]["PurchaseOrder_Date"]);//3

                string Customer_Id = Convert.ToString(PurchaseOrderEditdt.Rows[0]["PurchaseOrder_VendorId"]);//5
                string Contact_Person_Id = Convert.ToString(PurchaseOrderEditdt.Rows[0]["Contact_Person_Id"]);//6
                string PurchaseOrder_Reference = Convert.ToString(PurchaseOrderEditdt.Rows[0]["PurchaseOrder_Reference"]);//7
                string PurchaseOrder_Currency_Id = Convert.ToString(PurchaseOrderEditdt.Rows[0]["PurchaseOrder_Currency_Id"]);//8
                string Currency_Conversion_Rate = Convert.ToString(PurchaseOrderEditdt.Rows[0]["Currency_Conversion_Rate"]);//9
                string Tax_Option = Convert.ToString(PurchaseOrderEditdt.Rows[0]["Tax_Option"]);//10
                string Tax_Code = Convert.ToString(PurchaseOrderEditdt.Rows[0]["Tax_Code"]);//11
                string PurchaseOrder_SalesmanId = Convert.ToString(PurchaseOrderEditdt.Rows[0]["PurchaseOrder_SalesmanId"]);//12
                string PurchaseOrder_IndentDate = Convert.ToString(PurchaseOrderEditdt.Rows[0]["IndentDate"]);//13
                string IsInventory = Convert.ToString(PurchaseOrderEditdt.Rows[0]["IsInventory"]);//13
                string PurchaseOrderDueDate = Convert.ToString(PurchaseOrderEditdt.Rows[0]["PODueDate"]);//13
                string CreditDays = Convert.ToString(PurchaseOrderEditdt.Rows[0]["CreditDays"]);//13
                string VendorName = Convert.ToString(PurchaseOrderEditdt.Rows[0]["VendorName"]);//13
                string PosForGst = Convert.ToString(PurchaseOrderEditdt.Rows[0]["PosForGst"]);

                PurchaseOrderPosGst.Value = PosForGst;
                if (PurchaseOrderDocType.Rows.Count > 0)
                {
                    rdl_Salesquotation.SelectedValue = Convert.ToString(PurchaseOrderDocType.Rows[0]["CreatedTagDocType"]);
                }
                //if (IsInventory == "Y")
                //{
                //    //lookup_quotation.ClientEnabled = true;
                //    taggingList.ClientEnabled = true;
                //}
                //else
                //{
                //    //lookup_quotation.ClientEnabled = false;
                //    taggingList.ClientEnabled = false;
                //}
                taggingList.ClientEnabled = false;
                ddlInventory.SelectedValue = IsInventory;
                txtVoucherNo.Text = PurchaseOrder_Number;
                dt_PLQuote.Date = Convert.ToDateTime(PurchaseOrder_Date);
                // ddl_IndentRequisitionNo.SelectedValue = PurchaseOrder_IndentId;
                if (!string.IsNullOrEmpty(PurchaseOrder_IndentDate))
                {
                    //txtDateIndentRequis.Date = Convert.ToDateTime(PurchaseOrder_IndentDate); 
                    dt_Quotation.Text = PurchaseOrder_IndentDate;
                }
                // ddl_Vendor.SelectedValue = Customer_Id;
                //lookup_Customer.GridView.Selection.SelectRowByKey(Customer_Id);
                if (Customer_Id != "")
                {
                    hdfLookupCustomer.Value = Customer_Id;
                    DataTable GSTNTable = objPurchaseOrderBL.GetVendorGSTIN(Customer_Id);
                    string strGSTN = Convert.ToString(GSTNTable.Rows[0]["CNT_GSTIN"]).Trim();
                    if (strGSTN != "")
                    {
                        pageheaderContent.Attributes.Add("style", "display:block");
                        divGSTIN.Attributes.Add("style", "display:block");
                        lblGSTIN.Text = "Yes";
                    }
                    else
                    {
                        pageheaderContent.Attributes.Add("style", "display:block");
                        divGSTIN.Attributes.Add("style", "display:block");
                        lblGSTIN.Text = "No";
                    }
                    DataTable OutStandingTable = objPurchaseOrderBL.GetVendorOutStanding(Customer_Id);
                    if (OutStandingTable.Rows.Count > 0)
                    {
                        pageheaderContent.Attributes.Add("style", "display:block");
                        divOutstanding.Attributes.Add("style", "display:block");

                        var convertDecimal = Convert.ToDecimal(Convert.ToString(OutStandingTable.Rows[0]["NetOutstanding"]));
                        if (convertDecimal > 0)
                        {
                            lblTotalPayable.Text = Convert.ToString(convertDecimal) + "(Db)";
                        }
                        else
                        {
                            lblTotalPayable.Text = Convert.ToString(convertDecimal * -1).ToString() + "(Cr)";
                        }
                    }
                    else
                    {
                        cmbContactPerson.JSProperties["cpOutstanding"] = "0.0";
                    }
                }
                hdnCustomerId.Value = Customer_Id;
                ProcedureExecute GetVendorGstin = new ProcedureExecute("prc_GstTaxDetails");
                GetVendorGstin.AddVarcharPara("@Action", 500, "GetVendorGSTINByBranch");
                GetVendorGstin.AddVarcharPara("@branchId", 10, Convert.ToString(ddl_Branch.SelectedValue));
                GetVendorGstin.AddVarcharPara("@entityId", 10, Convert.ToString(Customer_Id));
                DataTable VendorGstin = GetVendorGstin.GetTable();

                if (VendorGstin.Rows.Count > 0)
                {
                    if (Convert.ToString(VendorGstin.Rows[0][0]).Trim() != "")
                    {
                        //Edited By Chinmoy
                        // this.Purchase_BillingShipping.hfVendorGSTIN.Value = Convert.ToString(VendorGstin.Rows[0][0]).Trim();
                        string VendorGst = Convert.ToString(VendorGstin.Rows[0][0]).Trim();
                        this.Purchase_BillingShipping.GetGSTIN(VendorGst);
                    }
                }
                txtVendorName.Text = VendorName.Trim();
                TabPage page = ASPxPageControl1.TabPages.FindByName("Billing/Shipping");
                page.ClientEnabled = true;
                PopulateContactPersonOfCustomer(Customer_Id);
                if (Contact_Person_Id != "0")
                {
                    cmbContactPerson.Value = Contact_Person_Id;

                    DataTable dtContactPersonPhone = new DataTable();
                    dtContactPersonPhone = objPurchaseOrderBL.PopulateContactPersonPhone(Contact_Person_Id);
                    if (dtContactPersonPhone != null && dtContactPersonPhone.Rows.Count > 0)
                    {
                        pageheaderContent.Attributes.Add("style", "display:block");
                        divContactPhone.Attributes.Add("style", "display:block");

                        foreach (DataRow dr in dtContactPersonPhone.Rows)
                        {
                            string phone = Convert.ToString(dr["phf_phoneNumber"]);
                            if (!string.IsNullOrEmpty(phone))
                            {
                                lblContactPhone.Text = phone;
                                break;
                            }
                        }

                    }

                }

                txt_Refference.Text = PurchaseOrder_Reference;
                ddl_Branch.SelectedValue = PurchaseOrder_BranchId;
                //ddl_SalesAgent.SelectedValue = PurchaseOrder_SalesmanId;
                ddl_Currency.SelectedValue = PurchaseOrder_Currency_Id;
                txt_Rate.Text = Currency_Conversion_Rate;


                string ProjectValidFrom = Convert.ToString(PurchaseOrderEditdt.Rows[0]["PO_ProjectValidFrom"]);
                string ProjectValidUpto = Convert.ToString(PurchaseOrderEditdt.Rows[0]["PO_ProjectValidUpto"]);

                if (!string.IsNullOrEmpty(Convert.ToString(PurchaseOrderEditdt.Rows[0]["PO_ProjectValidFrom"])))
                {
                    dtProjValidFrom.Date = Convert.ToDateTime(PurchaseOrderEditdt.Rows[0]["PO_ProjectValidFrom"]);

                }
                if (!string.IsNullOrEmpty(Convert.ToString(PurchaseOrderEditdt.Rows[0]["PO_ProjectValidUpto"])))
                {
                    dtProjValidUpto.Date = Convert.ToDateTime(PurchaseOrderEditdt.Rows[0]["PO_ProjectValidUpto"]);

                 }

                string ApproveProjectRem = Convert.ToString(PurchaseOrderEditdt.Rows[0]["ProjectPurchase_ApprovalRemarks"]);

                txtAppRejRemarks.Text = ApproveProjectRem;

                hdnApproveStatus.Value = Convert.ToString(PurchaseOrderEditdt.Rows[0]["ProjectPurchase_ApproveStatus"]);
                if(hdnApproveStatus.Value=="1")
                {
                    lookup_Project.ClientEnabled = false;
                }

                string POLastEntry = Convert.ToString(PurchaseOrderEditdt.Rows[0]["POLastEntry"]);

                string RevisionNo = Convert.ToString(PurchaseOrderEditdt.Rows[0]["ProjPOrder_RevisionNo"]);
                txtRevisionNo.Text = RevisionNo;

                if (!string.IsNullOrEmpty(Convert.ToString(PurchaseOrderEditdt.Rows[0]["ProjPOrder_RevisionDate"])))
                {
                    txtRevisionDate.Date = Convert.ToDateTime(PurchaseOrderEditdt.Rows[0]["ProjPOrder_RevisionDate"]);

                    txtRevisionDate.MinDate = Convert.ToDateTime(Convert.ToDateTime(PurchaseOrderEditdt.Rows[0]["ProjPOrder_RevisionDate"]).ToShortDateString());
                }
                else
                {
                    txtRevisionDate.Date = Convert.ToDateTime(PurchaseOrder_Date);
                    txtRevisionDate.MinDate = Convert.ToDateTime(Convert.ToDateTime(PurchaseOrder_Date).ToShortDateString());
                }


                #region Indent Tagging & Product Tagging
                string Quoids = Convert.ToString(PurchaseOrderEditdt.Rows[0]["PurchaseOrder_IndentIds"]);
                Session["EditQuoteId"] = Quoids;

                if (!String.IsNullOrEmpty(Quoids) && Quoids.Split(',')[0]!="0")
                {
                    lookup_Project.ClientEnabled = false;
                    BindLookUp(PurchaseOrder_Date, PurchaseOrder_BranchId);
                    string[] eachQuo = Quoids.Split(',');
                    string QuoComponent = "", QuoComponentNumber = "", QuoComponentDate = "";

                    for (int i = 0; i < taggingGrid.VisibleRowCount; i++)
                    {
                        string PurchaseOrder_Id = Convert.ToString(taggingGrid.GetRowValues(i, "Indent_Id"));
                        if (eachQuo.Contains(PurchaseOrder_Id))
                        {
                            QuoComponent += "," + Convert.ToString(taggingGrid.GetRowValues(i, "Indent_Id"));
                            QuoComponentNumber += "," + Convert.ToString(taggingGrid.GetRowValues(i, "Indent_RequisitionNumber"));
                            QuoComponentDate += "," + Convert.ToString(taggingGrid.GetRowValues(i, "Indent_RequisitionDate"));

                            taggingGrid.Selection.SelectRow(i);
                        }
                    }
                    QuoComponent = QuoComponent.TrimStart(',');
                    QuoComponentNumber = QuoComponentNumber.TrimStart(',');
                    QuoComponentDate = QuoComponentDate.TrimStart(',');

                    if (taggingGrid.GetSelectedFieldValues("Indent_Id").Count > 0)
                    {
                        if (taggingGrid.GetSelectedFieldValues("Indent_Id").Count > 1)
                        {
                            QuoComponentDate = "Multiple Project Purchase  Order Dates";
                        }
                    }
                    else
                    {
                        QuoComponentDate = "";
                    }

                    dt_Quotation.Text = Convert.ToString(PurchaseOrderEditdt.Rows[0]["Indent_RequisitionDate"]);
                    taggingList.Text = Convert.ToString(PurchaseOrderEditdt.Rows[0]["Indent_Numbers"]);



                    //if(taggingList.Text!="")
                    //{
                    //    taggingList.ClientEnabled =false;
                    //}
                    //else
                    //{
                    //    taggingList.ClientEnabled =true;
                    //}
                    string doctypeForTag = "";
                    string InventoryType = ddlInventory.SelectedValue;
                    DataTable dt_QuotationDetails = new DataTable();
                    string IdKey = Convert.ToString(Request.QueryString["key"]);
                    ProjectTermsBL ProjectTermsBL = new ProjectTermsBL();
                    if (PurchaseOrderDocType.Rows.Count > 0)
                    {
                        doctypeForTag = Convert.ToString(PurchaseOrderDocType.Rows[0]["CreatedTagDocType"]);
                    }
                    if (doctypeForTag == "Quotation")
                    {
                        dt_QuotationDetails = ProjectTermsBL.GetQuotationDetailsFromPO(QuoComponent, IdKey, "", "Edit");
                    }
                    //if (doctypeForTag == "Indent")
                    else
                    {
                        dt_QuotationDetails = ProjectTermsBL.GetIndentDetailsFromPO(QuoComponent, IdKey, "", "Edit");
                    }


                   // dt_QuotationDetails = objPurchaseOrderBL.GetIndentDetailsFromPO(QuoComponent, IdKey, "", "Edit", InventoryType);

                    grid_Products.DataSource = GetProductsInfo(dt_QuotationDetails);
                    grid_Products.DataBind();

                    DataTable Transaction_dt = (DataTable)Session["ProductOrderDetails"];

                    for (int i = 0; i < grid_Products.VisibleRowCount; i++)
                    {
                        string ComponentID = Convert.ToString(grid_Products.GetRowValues(i, "Quotation_No"));
                        string ProductList = Convert.ToString(grid_Products.GetRowValues(i, "gvColProduct"));

                        string[] list = ProductList.Split(new string[] { "||@||" }, StringSplitOptions.None);
                        string ProductID = Convert.ToString(list[0]) + "||@||%";

                        var Checkdt = Transaction_dt.Select("Indent_No='" + ComponentID + "' AND ProductID LIKE '" + ProductID + "'");
                        if (Checkdt.Length > 0)
                        {
                            grid_Products.Selection.SelectRow(i);
                        }
                    }
                }
                #endregion


                //Rev Tanmoy for Project
                string ProjectSelectEntryModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");

                lookup_Project.GridView.Selection.SelectRowByKey(Convert.ToInt64(PurchaseOrderEditdt.Rows[0]["ProjectId"]));

                //chinmoy started 03-12-2019

                DataTable BalNum = GetPurchaseOrderBalQtyDetails();
                if (BalNum != null && BalNum.Rows.Count > 0)
                {
                    int BalQty = Convert.ToInt32(BalNum.Rows[0]["BalQty"]);
                    if (BalQty == 0)
                    {
                        hddnDocumentIdTagged.Value = "1";
                    }
                }



                //End




                //Tanmoy  Hierarchy
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable dt2 = oDBEngine.GetDataTable("Select Hierarchy_ID from V_ProjectList where Proj_Id='" + Convert.ToInt64(PurchaseOrderEditdt.Rows[0]["ProjectId"]) + "'");
                if (dt2.Rows.Count > 0)
                {
                    ddlHierarchy.SelectedValue = Convert.ToString(dt2.Rows[0]["Hierarchy_ID"]);
                }
                //Tanmoy  Hierarchy End

                if (Convert.ToString(PurchaseOrderEditdt.Rows[0]["PurchaseOrder_IndentIds"]).Trim() != "")
                {
                    lookup_Project.ClientEnabled = false;
                }


                //End Rev

                hdnisFirstApprove.Value = Convert.ToString(PurchaseOrderEditdt.Rows[0]["FirstApprove"]);

                //Subhabrata
                //string Quoids = Convert.ToString(PurchaseOrderEditdt.Rows[0]["PurchaseOrder_IndentIds"]);

                //if (!String.IsNullOrEmpty(Quoids))
                //{
                //    string[] eachQuo = Quoids.Split(',');
                //    if (eachQuo.Length > 1)//More tha one quotation
                //    {
                //        dt_Quotation.Text = "Multiple Select Indent Dates";
                //        BindLookUp(Convert.ToString(dt_PLQuote.Date), PurchaseOrder_BranchId);
                //        foreach (string val in eachQuo)
                //        {
                //            lookup_quotation.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));
                //        }

                //    }
                //    else if (eachQuo.Length == 1)//Single Quotation
                //    {
                //        BindLookUp(Convert.ToString(dt_PLQuote.Date), PurchaseOrder_BranchId);
                //        foreach (string val in eachQuo)
                //        {
                //            lookup_quotation.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));
                //        }
                //    }
                //    else//No Quotation selected
                //    {
                //        BindLookUp(Convert.ToString(dt_PLQuote.Date), PurchaseOrder_BranchId);
                //    }
                //}
                //End

                if (Tax_Option != "0")
                {
                    ddl_AmountAre.Value = Tax_Option;
                }
                if (Tax_Option == "3")
                {
                    btn_SaveRecordTaxs.ClientEnabled = false;
                }
                PopulateGSTCSTVAT("2");
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
                txtCreditDays.Text = CreditDays;
                if (PurchaseOrderDueDate != "")
                {
                    dt_PODue.Date = Convert.ToDateTime(PurchaseOrderDueDate);
                }
            }
        }
        public IEnumerable GetPurchaseOrderBatch()
        {
            List<PurchaseOrderList> PurchaseOrderList = new List<PurchaseOrderList>();
            DataTable Quotationdt = GetPurchaseOrderData().Tables[0];


            for (int i = 0; i < Quotationdt.Rows.Count; i++)
            {
                PurchaseOrderList PurchaseOrders = new PurchaseOrderList();

                PurchaseOrders.SrlNo = Convert.ToString(Quotationdt.Rows[i]["SrlNo"]);
                PurchaseOrders.OrderDetails_Id = Convert.ToString(Quotationdt.Rows[i]["OrderDetails_Id"]);
                PurchaseOrders.gvColProduct = Convert.ToString(Quotationdt.Rows[i]["ProductID"]);
                PurchaseOrders.gvColDiscription = Convert.ToString(Quotationdt.Rows[i]["Description"]);
                PurchaseOrders.gvColQuantity = Convert.ToString(Quotationdt.Rows[i]["Quantity"]);
                PurchaseOrders.gvColUOM = Convert.ToString(Quotationdt.Rows[i]["UOM"]);
                PurchaseOrders.Warehouse = "";
                PurchaseOrders.gvColStockQty = Convert.ToString(Quotationdt.Rows[i]["StockQuantity"]);
                PurchaseOrders.gvColStockUOM = Convert.ToString(Quotationdt.Rows[i]["StockUOM"]);
                PurchaseOrders.gvColStockPurchasePrice = Convert.ToString(Quotationdt.Rows[i]["PurchasePrice"]);
                PurchaseOrders.gvColDiscount = Convert.ToString(Quotationdt.Rows[i]["Discount"]);
                PurchaseOrders.gvColAmount = Convert.ToString(Quotationdt.Rows[i]["Amount"]);
                PurchaseOrders.gvColTaxAmount = Convert.ToString(Quotationdt.Rows[i]["TaxAmount"]);
                PurchaseOrders.gvColTotalAmountINR = Convert.ToString(Quotationdt.Rows[i]["TotalAmount"]);
                //PurchaseOrders.Quotation_No = Convert.ToString(0);
                PurchaseOrders.ProductName = Convert.ToString(Quotationdt.Rows[i]["ProductName"]);
                PurchaseOrders.IsComponentProduct = Convert.ToString(Quotationdt.Rows[i]["IsComponentProduct"]);
                PurchaseOrders.PurchaseOrder_InlineRemarks = Convert.ToString(Quotationdt.Rows[i]["PurchaseOrder_InlineRemarks"]);
                PurchaseOrders.BalQty = Convert.ToString(Quotationdt.Rows[i]["BalQty"]);
                PurchaseOrders.Document_DetailsID = Convert.ToString(Quotationdt.Rows[i]["Document_DetailsID"]);
                PurchaseOrderList.Add(PurchaseOrders);
            }
            PurchaseOrderPosGst.DataSource = GetPurchaseOrderData().Tables[2];
            PurchaseOrderPosGst.ValueField = "ID";
            PurchaseOrderPosGst.TextField = "Name";
            PurchaseOrderPosGst.DataBind();

            return PurchaseOrderList;
        }
        public DataSet GetPurchaseOrderData()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc__Project_PurchaseOrderDetailsList");
            proc.AddVarcharPara("@Action", 500, "PoBatchEditDetails");
            proc.AddVarcharPara("@PurchaseOrder_Id", 500, Convert.ToString(Session["PurchaseOrder_Id"]));
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            proc.AddVarcharPara("@campany_Id", 500, Convert.ToString(Session["LastCompany"]));
            ds = proc.GetDataSet();
            return ds;
        }
        public DataTable GetPurchaseOrderEditData()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc__Project_PurchaseOrderDetailsList");
            proc.AddVarcharPara("@Action", 500, "PurchaseOrderEditDetails");
            proc.AddIntegerPara("@PurchaseOrder_Id", Convert.ToInt32(Session["PurchaseOrder_Id"]));
            //proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            //proc.AddVarcharPara("@branchId", 500, Convert.ToString(Session["userbranchID"]));
            //proc.AddVarcharPara("@companyId", 500, Convert.ToString(Session["LastCompany"]));
            dt = proc.GetTable();
            return dt;
        }

        public DataTable PurchaseOrderDocTypeEditDetails()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList");
            proc.AddVarcharPara("@Action", 500, "PurchaseOrderDocTypeEditDetails");
            proc.AddIntegerPara("@PurchaseOrder_Id", Convert.ToInt32(Session["PurchaseOrder_Id"]));
          
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetPurchaseOrderBalQtyDetails()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc__Project_PurchaseOrderDetailsList");
            proc.AddVarcharPara("@Action", 500, "PurchaseOrderBalQty");
            proc.AddIntegerPara("@PurchaseOrder_Id", Convert.ToInt32(Session["PurchaseOrder_Id"]));
           
            dt = proc.GetTable();
            return dt;
        }


        public DataSet GetProductData()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc__Project_PurchaseOrderDetailsList");
            proc.AddVarcharPara("@Action", 500, "ProductDetails");
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            proc.AddVarcharPara("@campany_Id", 500, Convert.ToString(Session["LastCompany"]));
            ds = proc.GetDataSet();
            return ds;
        }
        public IEnumerable GetProduct()
        {
            List<Product> ProductList = new List<Product>();


            //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

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
        //protected void Page_Init(object sender, EventArgs e)
        //{
        //    ((GridViewDataComboBoxColumn)grid.Columns["gvColProduct"]).PropertiesComboBox.DataSource = GetProduct();
        //}

        #region  WebMethod

        [WebMethod]
        public static string SetApproveReject(string ApproveRemarks, int ApproveRejStatus, string OrderId)
        {
            ProjectPurchaseOrderBL objCRMSalesOrderDtlBL = new ProjectPurchaseOrderBL();
            string val = objCRMSalesOrderDtlBL.ApproveRejectProject(ApproveRemarks, ApproveRejStatus, OrderId);
            return val;
        }


        [WebMethod]
        public static Int64 GetTermsConditionsDataFromProjectCode(string ProjectCode)
        {

            HttpContext.Current.Session["BGDetails"] = null;
            Int64 ProjId = 0;
            DBEngine odb = new DBEngine();
            DataTable dbt = odb.GetDataTable("select isnull(Proj_Id,0) Proj_Id from Master_ProjectManagement where Proj_Code='" + ProjectCode + "'");

            if (dbt.Rows.Count > 0)
            {
                ProjId = Convert.ToInt64(dbt.Rows[0]["Proj_Id"]);
            }
            return ProjId;
        }


        [WebMethod]
        public static int Duplicaterevnumbercheck(string RevNo, string Order)
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            int returnVal = 0;
            CRMSalesOrderDtlBL objCRMSalesOrderDtlBL = new CRMSalesOrderDtlBL();
            DataTable dtrev = new DataTable();
            DataTable OrderNumber = new DataTable();
            OrderNumber = oDBEngine.GetDataTable("select ISNULL(PurchaseOrder_Number,'') PurchaseOrder_Number from tbl_trans_PurchaseOrder  where PurchaseOrder_Id='" + Order + "'");
            string orderno;
            if (OrderNumber.Rows.Count > 0)
            {
                orderno = Convert.ToString(OrderNumber.Rows[0]["PurchaseOrder_Number"]);
            }
            else
            {
                orderno = "";
            }
            //dtrev = objCRMSalesOrderDtlBL.DuplicateRevisionNumber(RevNo, Convert.ToString(OrderNumber.Rows[0]["Order_Number"]));
            //dtrev = objCRMSalesOrderDtlBL.DuplicateRevisionNumber(RevNo, Convert.ToString(OrderNumber.Rows[0]["Order_Number"]));
            dtrev = oDBEngine.GetDataTable("select ProjPOrder_RevisionNo from tbl_trans_PurchaseOrder where  PurchaseOrder_Number='" + orderno + "'");
            //if (dtrev.Rows.Count > 0)
            //{
            //    returnVal = 1;
            //}
            if (dtrev.Rows.Count > 0)
            {
                for (int i = 0; i < dtrev.Rows.Count; i++)
                {
                    if (dtrev.Rows[i]["ProjPOrder_RevisionNo"].ToString() == RevNo)
                    {
                        returnVal = 1;
                    }
                }
            }
            return returnVal;
        }


        [WebMethod]
        public static bool CheckUniqueName(string VoucherNo)
        {
            MShortNameCheckingBL objMShortNameCheckingBL = new MShortNameCheckingBL();
            DataTable dt = new DataTable();
            Boolean status = false;
            BusinessLogicLayer.GenericMethod oGeneric = new BusinessLogicLayer.GenericMethod();

            if (VoucherNo != "" && Convert.ToString(VoucherNo).Trim() != "")
            {
                status = objMShortNameCheckingBL.CheckUnique(Convert.ToString(VoucherNo).Trim(), "0", "PurchaseOrder_Check");
            }
            return status;
        }
        [WebMethod]
        public static string getSchemeType(string sel_scheme_id)
        {
            //BusinessLogicLayer.DBEngine oDbEngine1 = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            //string[] scheme = oDbEngine1.GetFieldValue1("tbl_master_Idschema", "schema_type", "Id = " + Convert.ToInt32(sel_scheme_id), 1);
            //return Convert.ToString(scheme[0]);
            string strschematype = "", strschemalength = "", strschemavalue = "", strschemaBranch = "";


            //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();



            DataTable DT = objEngine.GetDataTable("tbl_master_Idschema", " schema_type,length,Branch ", " Id = " + Convert.ToInt32(sel_scheme_id));

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                strschematype = Convert.ToString(DT.Rows[i]["schema_type"]);
                strschemalength = Convert.ToString(DT.Rows[i]["length"]);
                strschemaBranch = Convert.ToString(DT.Rows[i]["Branch"]);
                strschemavalue = strschematype + "~" + strschemalength + "~" + strschemaBranch;
            }
            return Convert.ToString(strschemavalue);
        }
        [WebMethod]
        public static string getIndentRequisitionDate(string IndentRequisitionNo)
        {
            // BusinessLogicLayer.DBEngine oDbEngine1 = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

            BusinessLogicLayer.DBEngine oDbEngine1 = new BusinessLogicLayer.DBEngine();

            string[] RequisitionDate = oDbEngine1.GetFieldValue1("tbl_trans_Indent", "Indent_RequisitionDate", "Indent_Id = " + Convert.ToInt32(IndentRequisitionNo), 1);
            return Convert.ToString(RequisitionDate[0]);
        }
        [WebMethod]
        public static String GetRate(string basedCurrency, string Currency_ID, string Campany_ID)
        {

            PurchaseOrderBL objPurchaseOrderBL = new PurchaseOrderBL();
            DataTable dt = objPurchaseOrderBL.GetCurrentConvertedRate(Convert.ToInt16(basedCurrency), Convert.ToInt16(Currency_ID), Campany_ID);
            string SalesRate = "";
            if (dt.Rows.Count > 0)
            {
                SalesRate = Convert.ToString(dt.Rows[0]["PurchaseRate"]);
            }

            return SalesRate;
        }
        [WebMethod]
        public static string GetVendorReletedData(string VendorId)
        {
            PurchaseOrderBL objPurchaseOrderBL = new PurchaseOrderBL();
            DataSet dtVendor = objPurchaseOrderBL.GetVendorDetails_CDRelated(VendorId);

            string strDueDate = "";
            if (dtVendor.Tables[0] != null && dtVendor.Tables[0].Rows.Count > 0)
            {
                strDueDate = Convert.ToString(dtVendor.Tables[0].Rows[0]["DueDate"]);
            }

            string strcountryId = "";
            if (dtVendor.Tables[1] != null && dtVendor.Tables[1].Rows.Count > 0)
            {
                strcountryId = Convert.ToString(dtVendor.Tables[1].Rows[0]["add_country"]);
            }
            else
            {
                strcountryId = "0";
            }

            string strOutstanding = "";
            if (dtVendor.Tables[2] != null && dtVendor.Tables[2].Rows.Count > 0)
            {
                var convertDecimal = Convert.ToDecimal(Convert.ToString(dtVendor.Tables[2].Rows[0]["NetOutstanding"]));
                if (convertDecimal > 0)
                {
                    strOutstanding = Convert.ToString(convertDecimal) + "(Db)";
                }
                else
                {
                    strOutstanding = Convert.ToString(convertDecimal * -1).ToString() + "(Cr)";
                }
            }
            else
            {
                strOutstanding = "0.0";
            }

            string strGSTN = "";
            if (dtVendor.Tables[3] != null && dtVendor.Tables[3].Rows.Count > 0)
            {
                strGSTN = Convert.ToString(dtVendor.Tables[3].Rows[0]["CNT_GSTIN"]).Trim();
                if (strGSTN != "")
                {
                    strGSTN = "Yes";
                }
                else
                {
                    strGSTN = "No";
                }
            }

            return strDueDate + "~" + strcountryId + "~" + strOutstanding + "~" + strGSTN;
        }



        #endregion Class
        public void GetAllDropDownDetailForPurchaseOrder(string branchHierchy)
        {
            DataSet dst = new DataSet();
            dst = objPurchaseOrderBL.GetAllDropDownDetailForPurchaseOrder(branchHierchy);


            //if (dst.Tables[1] != null && dst.Tables[1].Rows.Count > 0)
            //{
            //    ddl_SalesAgent.DataTextField = "Name";
            //    ddl_SalesAgent.DataValueField = "cnt_id";
            //    ddl_SalesAgent.DataSource = dst.Tables[1];
            //    ddl_SalesAgent.DataBind();
            //}

            if (dst.Tables[3] != null && dst.Tables[3].Rows.Count > 0)
            {

                ddl_AmountAre.TextField = "taxGrp_Description";
                ddl_AmountAre.ValueField = "taxGrp_Id";
                ddl_AmountAre.DataSource = dst.Tables[3];
                ddl_AmountAre.DataBind();

                ListEditItem li = new ListEditItem();
                li.Text = "Import";
                li.Value = "4";
                ddl_AmountAre.Items.Insert(3, li);
            }
            if (dst.Tables[4] != null && dst.Tables[4].Rows.Count > 0)
            {
                ddl_numberingScheme.DataSource = dst.Tables[4];
                ddl_numberingScheme.DataBind();
            }

            //ddl_SalesAgent.Items.Insert(0, new ListItem("Select", "0"));

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
                        Remarks.Columns.Add("PurchaseOrder_AddiDesc", typeof(string));

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
                        txtInlineRemarks.Text = dvData[0]["PurchaseOrder_AddiDesc"].ToString();
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

        protected void cmbContactPerson_Callback(object sender, CallbackEventArgsBase e)
        {
            string WhichCall = e.Parameter.Split('~')[0];
            if (WhichCall == "BindContactPerson")
            {
                string InternalId = Convert.ToString(Convert.ToString(e.Parameter.Split('~')[1]));
                PopulateContactPersonOfCustomer(InternalId);

                //DataTable dtDeuDate = objPurchaseOrderBL.GetVendorDetails_CDRelated(InternalId);
                //foreach (DataRow dr in dtDeuDate.Rows)
                //{
                //    string strDueDate = Convert.ToString(dr["DueDate"]);
                //    cmbContactPerson.JSProperties["cpDueDate"] = strDueDate;

                //}
                //DataTable dtVenCountryID = objPurchaseOrderBL.PopulateVendorCountryID(InternalId);
                //foreach (DataRow dr in dtVenCountryID.Rows)
                //{
                //    string strcountryId = Convert.ToString(dr["add_country"]);
                //    cmbContactPerson.JSProperties["cpVendorCountryID"] = strcountryId;

                //}
                //DataTable OutStandingTable = objPurchaseOrderBL.GetVendorOutStanding(InternalId);
                //if (OutStandingTable.Rows.Count > 0)
                //{
                //    var convertDecimal = Convert.ToDecimal(Convert.ToString(OutStandingTable.Rows[0]["NetOutstanding"]));
                //    if (convertDecimal > 0)
                //    {
                //        cmbContactPerson.JSProperties["cpOutstanding"] = Convert.ToString(convertDecimal) + "(Db)";
                //    }
                //    else
                //    {
                //        cmbContactPerson.JSProperties["cpOutstanding"] = Convert.ToString(convertDecimal * -1).ToString() + "(Cr)";
                //    }
                //}
                //else
                //{
                //    cmbContactPerson.JSProperties["cpOutstanding"] = "0.0";
                //}

                //DataTable GSTNTable = objPurchaseOrderBL.GetVendorGSTIN(InternalId);
                //string strGSTN = Convert.ToString(GSTNTable.Rows[0]["CNT_GSTIN"]).Trim();
                //if (strGSTN != "")
                //{
                //    cmbContactPerson.JSProperties["cpGSTN"] = "Yes";
                //}
                //else
                //{
                //    cmbContactPerson.JSProperties["cpGSTN"] = "No";
                //}
            }

        }
        protected void acpContactPersonPhone_Callback(object sender, CallbackEventArgsBase e)
        {
            string WhichCall = e.Parameter.Split('~')[0];
            if (WhichCall == "ContactPersonPhone")
            {
                string InternalId = Convert.ToString(Convert.ToString(e.Parameter.Split('~')[1]));
                DataTable dtContactPersonPhone = new DataTable();
                dtContactPersonPhone = objPurchaseOrderBL.PopulateContactPersonPhone(InternalId);
                if (dtContactPersonPhone != null && dtContactPersonPhone.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtContactPersonPhone.Rows)
                    {
                        string phone = Convert.ToString(dr["phf_phoneNumber"]);
                        if (!string.IsNullOrEmpty(phone))
                        {
                            acpContactPersonPhone.JSProperties["cpPhone"] = phone;
                            break;
                        }
                    }

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

            if (type == "2")
            {
                dtGSTCSTVAT = objPurchaseOrderBL.PopulateGSTCSTVAT(dt_PLQuote.Date.ToString("yyyy-MM-dd"));


                #region Delete Igst,Cgst,Sgst respectively

                string CompInternalId = Convert.ToString(Session["LastCompany"]);
                string[] compGstin = oDBEngine.GetFieldValue1("tbl_master_company", "cmp_gstin", "cmp_internalid='" + CompInternalId + "'", 1);

                string ShippingState = "";
                #region ##### Added By : Samrat Roy -- For BillingShippingUserControl ######
                //Edited By chinmoy
                string sstateCode = "";
                if (PurchaseOrderPosGst.Value != null)
                {
                    if (PurchaseOrderPosGst.Value.ToString() == "S")
                    {
                        sstateCode = Convert.ToString(Purchase_BillingShipping.GetShippingStateId());
                    }
                    else
                    {
                        sstateCode = Convert.ToString(Purchase_BillingShipping.GetBillingStateId());
                    }
                }
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
                            //   Chandigarh     Andaman and Nicobar Islands    DADRA & NAGAR HAVELI    DAMAN & DIU      Lakshadweep              PONDICHERRY
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
        protected void PopulateContactPersonOfCustomer(string InternalId)
        {
            //string ContactPerson = "";
            DataTable dtContactPerson = new DataTable();
            dtContactPerson = objPurchaseOrderBL.PopulateContactPersonOfCustomer(InternalId);
            if (dtContactPerson != null && dtContactPerson.Rows.Count > 0)
            {
                cmbContactPerson.TextField = "cp_name";
                cmbContactPerson.ValueField = "cp_contactId";
                cmbContactPerson.DataSource = dtContactPerson;
                cmbContactPerson.DataBind();

                cmbContactPerson.Value = cmbContactPerson.Items[0].Value;

                //foreach (DataRow dr in dtContactPerson.Rows)
                //{
                //    if (Convert.ToString(dr["Isdefault"]) == "True")
                //    {
                //        ContactPerson = Convert.ToString(dr["add_id"]);
                //        break;
                //    }
                //}
                //cmbContactPerson.SelectedItem = cmbContactPerson.Items.FindByValue(ContactPerson);

            }
        }
        #region Shipping DropDown Call Back Function End
        #region All address Dropdown Populated Data and function


        string[,] GetState(int country)
        {
            StateSelect.SelectParameters[0].DefaultValue = Convert.ToString(country);
            DataView view = (DataView)StateSelect.Select(DataSourceSelectArguments.Empty);
            string[,] DATA = new string[view.Count, 2];
            for (int i = 0; i < view.Count; i++)
            {
                DATA[i, 0] = Convert.ToString(view[i][0]);
                DATA[i, 1] = Convert.ToString(view[i][1]);
            }
            return DATA;

        }

        protected void FillStateCombo(ASPxComboBox cmb, int country)
        {

            string[,] state = GetState(country);
            cmb.Items.Clear();

            for (int i = 0; i < state.GetLength(0); i++)
            {
                cmb.Items.Add(state[i, 1], state[i, 0]);
            }
            //cmb.SelectedIndex = -1;
            // Code Commented By Sandip on 08032017 To avoid Select Option By default End
            //cmb.Items.Insert(0, new ListEditItem("Select", "0"));
            // Code Above Commented By Sandip on 08032017 To avoid Select Option By default End
        }


        string[,] GetCities(int state)
        {


            SelectCity.SelectParameters[0].DefaultValue = Convert.ToString(state);
            DataView view = (DataView)SelectCity.Select(DataSourceSelectArguments.Empty);
            string[,] DATA = new string[view.Count, 2];
            for (int i = 0; i < view.Count; i++)
            {
                DATA[i, 0] = Convert.ToString(view[i][0]);
                DATA[i, 1] = Convert.ToString(view[i][1]);
            }
            return DATA;

        }
        protected void FillCityCombo(ASPxComboBox cmb, int state)
        {

            string[,] cities = GetCities(state);
            cmb.Items.Clear();

            for (int i = 0; i < cities.GetLength(0); i++)
            {
                cmb.Items.Add(cities[i, 1], cities[i, 0]);
            }
            //cmb.SelectedIndex = -1;
            // Code Commented By Sandip on 08032017 To avoid Select Option By default End
            //cmb.Items.Insert(0, new ListEditItem("Select", "0"));
            // Code Above Commented By Sandip on 08032017 To avoid Select Option By default End
        }
        protected void FillPinCombo(ASPxComboBox cmb, int city)
        {
            string[,] pin = GetPin(city);
            cmb.Items.Clear();

            for (int i = 0; i < pin.GetLength(0); i++)
            {
                cmb.Items.Add(pin[i, 1], pin[i, 0]);
            }
            //cmb.SelectedIndex = -1;
        }
        string[,] GetPin(int city)
        {
            SelectPin.SelectParameters[0].DefaultValue = Convert.ToString(city);
            DataView view = (DataView)SelectPin.Select(DataSourceSelectArguments.Empty);
            string[,] DATA = new string[view.Count, 2];
            for (int i = 0; i < view.Count; i++)
            {
                DATA[i, 0] = Convert.ToString(view[i][0]);
                DATA[i, 1] = Convert.ToString(view[i][1]);
            }
            return DATA;
        }
        string[,] GetArea(int city)
        {
            SelectArea.SelectParameters[0].DefaultValue = Convert.ToString(city);
            DataView view = (DataView)SelectArea.Select(DataSourceSelectArguments.Empty);
            string[,] DATA = new string[view.Count, 2];
            for (int i = 0; i < view.Count; i++)
            {
                DATA[i, 0] = Convert.ToString(view[i][0]);
                DATA[i, 1] = Convert.ToString(view[i][1]);
            }
            return DATA;
        }

        protected void FillAreaCombo(ASPxComboBox cmb, int city)
        {
            string[,] area = GetArea(city);
            cmb.Items.Clear();

            for (int i = 0; i < area.GetLength(0); i++)
            {
                cmb.Items.Add(area[i, 1], area[i, 0]);
            }
            //cmb.SelectedIndex = -1;
            // Code Commented By Sandip on 08032017 To avoid Select Option By default Start
            //cmb.Items.Insert(0, new ListEditItem("Select", "0"));
            // Code Above Commented By Sandip on 08032017 To avoid Select Option By default End
        }

        #endregion

        #region Shipping DropDown Call Back Function End
        protected void cmbState_OnCallback(object source, CallbackEventArgsBase e)
        {
            if (e.Parameter != "")
            {
                FillStateCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
            }
            //else
            //{
            //    FillStateCombo(source as ASPxComboBox, 0);
            //}
        }
        protected void cmbCity_OnCallback(object source, CallbackEventArgsBase e)
        {
            if (e.Parameter != "")
            {
                FillCityCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
            }
            //else
            //{
            //    FillCityCombo(source as ASPxComboBox, 0);
            //}
        }
        protected void cmbPin_OnCallback(object source, CallbackEventArgsBase e)
        {
            if (e.Parameter != "")
            {
                FillPinCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
            }
            //else
            //{
            //    FillPinCombo(source as ASPxComboBox, 0);
            //}
        }
        protected void cmbArea_OnCallback(object source, CallbackEventArgsBase e)
        {
            if (e.Parameter != "")
            {
                FillAreaCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
            }
            //else
            //{
            //    FillAreaCombo(source as ASPxComboBox, 0);
            //}
        }

        #endregion Billing DropDown Call Back Function End

        #region Shipping DropDown Call Back Function Start
        protected void cmbState1_OnCallback(object source, CallbackEventArgsBase e)
        {
            if (e.Parameter != "")
            {
                FillStateCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
            }
            //else
            //{
            //    FillStateCombo(source as ASPxComboBox, 0);
            //}
        }

        protected void cmbCity1_OnCallback(object source, CallbackEventArgsBase e)
        {
            if (e.Parameter != "")
            {
                FillCityCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
            }
            //else
            //{
            //    FillCityCombo(source as ASPxComboBox, 0);
            //}
        }
        protected void cmbArea1_OnCallback(object source, CallbackEventArgsBase e)
        {
            if (e.Parameter != "")
            {
                FillAreaCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
            }
            //else
            //{
            //    FillAreaCombo(source as ASPxComboBox, 0);
            //}
        }
        protected void cmbPin1_OnCallback(object source, CallbackEventArgsBase e)
        {
            if (e.Parameter != "")
            {
                FillPinCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
            }
            //else
            //{
            //    FillPinCombo(source as ASPxComboBox, 0);
            //}
        }

        #endregion Shipping DropDown Call Back Function End


        #endregion Billing DropDown Call Back Function End
        #region  Batch Grid
        protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string strSplitCommand = e.Parameters.Split('~')[0];
            //if (strSplitCommand == "Display")
            if (strSplitCommand == "Display")
            {
                string IsDeleteFrom = Convert.ToString(hdfIsDelete.Value);
                if (IsDeleteFrom == "D" || IsDeleteFrom == "C")
                {
                    DataTable POdt = (DataTable)Session["ProductOrderDetails"];
                    grid.DataSource = GetPurchaseOrderBatch(POdt);
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
                Session["ProductOrderDetails"] = null;
                grid.DataSource = null;
                grid.DataBind();
            }
            else if (strSplitCommand == "BindGridOnQuotation")
            {
                ASPxGridView gv = sender as ASPxGridView;
                string command = e.Parameters.ToString();
                string State = Convert.ToString(e.Parameters.Split('~')[1]);
                String QuoComponent1 = "";
                string Product_id1 = "";
                string QuoteDetails_Id = "";
                string strPOID = "";
                if (grid_Products.GetSelectedFieldValues("Quotation_No").Count > 0)
                {
                    for (int i = 0; i < grid_Products.GetSelectedFieldValues("Quotation_No").Count; i++)
                    {

                        QuoComponent1 += "," + Convert.ToString(grid_Products.GetSelectedFieldValues("Quotation_No")[i]);
                        Product_id1 += "," + Convert.ToString(grid_Products.GetSelectedFieldValues("gvColProduct")[i]);
                        QuoteDetails_Id += "," + Convert.ToString(grid_Products.GetSelectedFieldValues("QuoteDetails_Id")[i]);

                    }
                    QuoComponent1 = QuoComponent1.TrimStart(',');
                    Product_id1 = Product_id1.TrimStart(',');
                    QuoteDetails_Id = QuoteDetails_Id.TrimStart(',');


                    string Quote_Nos = Convert.ToString(e.Parameters.Split('~')[1]);

                    string DocProType = Convert.ToString(rdl_Salesquotation.SelectedValue);
                    ProjectTermsBL ProjectTermsBL=new ProjectTermsBL();
                    if (Quote_Nos != "$")
                    {
                        string InventoryType = ddlInventory.SelectedValue;
                        DataSet  dt_Quotationfortagged=new DataSet();
                        DataTable dt_QuotationDetails = new DataTable();
                        string IdKey = Convert.ToString(Request.QueryString["key"]);
                        if (!string.IsNullOrEmpty(IdKey))
                        {
                            if (IdKey != "ADD")
                            {
                                strPOID=Convert.ToString(Session["PurchaseOrder_Id"]);
                               // dt_QuotationDetails = objPurchaseOrderBL.GetIndentDetailsForPOGridBind(QuoComponent1, QuoteDetails_Id, Product_id1, "Edit", InventoryType);
                                if (DocProType == "Indent")
                                {
                                    dt_Quotationfortagged = ProjectTermsBL.GetIndentDetailsForPOGridBind(QuoComponent1, QuoteDetails_Id, Product_id1, "Edit", strPOID);
                                   dt_QuotationDetails= dt_Quotationfortagged.Tables[2];
                                }
                                else if (DocProType == "Quotation")
                                {
                                    dt_Quotationfortagged = ProjectTermsBL.GetPQDetailsForPOGridBind(QuoComponent1, QuoteDetails_Id, Product_id1, "Edit");
                                    dt_QuotationDetails = dt_Quotationfortagged.Tables[0];
                                }
                            }
                            else
                            {
                               
                                 if (DocProType == "Indent")
                                {
                                    dt_Quotationfortagged = ProjectTermsBL.GetIndentDetailsForPOGridBind(QuoComponent1, QuoteDetails_Id, Product_id1, "Add", strPOID);
                                    dt_QuotationDetails= dt_Quotationfortagged.Tables[2];
                                    //MultiUOMDet = objPurchaseOrderBL.GetIndentDetailsForMUltiUOMGridBind(QuoComponent1, QuoteDetails_Id, Product_id1);
                                }
                                else if(DocProType == "Quotation")
                                {
                                    dt_Quotationfortagged = ProjectTermsBL.GetPQDetailsForPOGridBind(QuoComponent1, QuoteDetails_Id, Product_id1, "Add");
                                    dt_QuotationDetails = dt_Quotationfortagged.Tables[0];
                                   // MultiUOMDet = objPurchaseOrderBL.GetPQDetailsForMUltiUOMGridBind(QuoComponent1, QuoteDetails_Id, Product_id1);
                                }
                            }

                            Session["InlineRemarks"] = dt_Quotationfortagged.Tables[1];

                        }                       

                        Session["ProductOrderDetails"] = null;
                        grid.DataSource = GetSalesOrderInfo(dt_QuotationDetails, IdKey);
                        grid.DataBind();                       
                    }
                    else
                    {
                        grid.DataSource = null;
                        grid.DataBind();
                        grid.JSProperties["cpProjectID"] = 0;
                    }
                }
                else
                {
                    DataTable Transactiondt = CreateTempTable("Transaction");
                    Transactiondt.Rows.Add("1", "1", "", "", "", "", "", "0.0000", "", "0.00", "0.00", "0.00", "0.00", "0.00", "0", "0", "", "","", "I");

                    Session["ProductOrderDetails"] = Transactiondt;
                    grid.DataSource = null;
                    grid.DataBind();

                    grid.JSProperties["cpBindNullGrid"] = "Y";
                }

            }
        }
        public IEnumerable GetSalesOrderInfo(DataTable SalesOrderdt1, string Order_Id)
        {
            List<SalesOrder> OrderList = new List<SalesOrder>();
            DataColumnCollection dtC = SalesOrderdt1.Columns;
            for (int i = 0; i < SalesOrderdt1.Rows.Count; i++)
            {
                SalesOrder Orders = new SalesOrder();

                Orders.SrlNo = Convert.ToString(i + 1);
                Orders.OrderDetails_Id = Convert.ToString(i + 1);
                Orders.gvColProduct = Convert.ToString(SalesOrderdt1.Rows[i]["QuoteDetails_ProductId"]);
                Orders.gvColDiscription = Convert.ToString(SalesOrderdt1.Rows[i]["QuoteDetails_ProductDescription"]);
                Orders.gvColQuantity = Convert.ToString(SalesOrderdt1.Rows[i]["QuoteDetails_Quantity"]);
                Orders.gvColUOM = Convert.ToString(SalesOrderdt1.Rows[i]["UOM_ShortName"]);
                Orders.Warehouse = "";
                Orders.gvColStockQty = Convert.ToString(SalesOrderdt1.Rows[i]["QuoteDetails_StockQty"]);
                Orders.gvColStockUOM = Convert.ToString(SalesOrderdt1.Rows[i]["UOM_ShortName"]);
                Orders.gvColStockPurchasePrice = Convert.ToString(SalesOrderdt1.Rows[i]["SalePrice"]);
                Orders.gvColDiscount = Convert.ToString(SalesOrderdt1.Rows[i]["Discount"]);
                Orders.gvColAmount = Convert.ToString(SalesOrderdt1.Rows[i]["Amount"]);
                Orders.gvColTaxAmount = Convert.ToString(SalesOrderdt1.Rows[i]["TaxAmount"]);
                Orders.gvColTotalAmountINR = Convert.ToString(SalesOrderdt1.Rows[i]["TotalAmount"]);
                if (!string.IsNullOrEmpty(Convert.ToString(SalesOrderdt1.Rows[i]["Indent_No"])))//Added on 15-02-2017
                { Orders.Indent = Convert.ToInt64(SalesOrderdt1.Rows[i]["Indent_No"]); }
                else
                { Orders.Indent = 0; }
                if (dtC.Contains("Indent"))
                {
                    Orders.Indent_Num = Convert.ToString(SalesOrderdt1.Rows[i]["Indent"]);//subhabrata on 21-02-2017
                }
                if (!string.IsNullOrEmpty(Convert.ToString(SalesOrderdt1.Rows[i]["Products_Name"])))
                { Orders.ProductName = Convert.ToString(SalesOrderdt1.Rows[i]["Products_Name"]); }
                else
                {
                    Orders.ProductName = "";
                }

                if(dtC.Contains("PurchaseOrder_InlineRemarks"))
                {
                    Orders.PurchaseOrder_InlineRemarks = Convert.ToString(SalesOrderdt1.Rows[i]["PurchaseOrder_InlineRemarks"]);
                }
                else
                {
                    Orders.PurchaseOrder_InlineRemarks = "";
                }
                // Mapping With Warehouse with Product Srl No
                string strQuoteDetails_Id = Convert.ToString(SalesOrderdt1.Rows[i]["QuoteDetails_Id"]).Trim();
                // End

                Orders.Document_DetailsID = Convert.ToString(SalesOrderdt1.Rows[i]["QuoteDetails_Id"]).Trim();


                OrderList.Add(Orders);
            }

            BindSessionByDatatable(SalesOrderdt1);

            return OrderList;
        }

        #region Subhabrata/SessionBind
        //Subhabrata on 02-03-2017
        public bool BindSessionByDatatable(DataTable dt)
        {
            //if (Session["InlineRemarks"] != null)
            //{
            //    Remarks = (DataTable)Session["InlineRemarks"];
            //}
            //else
            //{
            //    if (Remarks == null || Remarks.Rows.Count == 0)
            //    {
            //        Remarks = new DataTable();
            //        Remarks.Columns.Add("SrlNo", typeof(string));
            //        Remarks.Columns.Add("PurchaseOrder_AddiDesc", typeof(string));
            //    }
            //}

            bool IsSuccess = false;
            DataTable purChaseDT = new DataTable();
            purChaseDT.Columns.Add("SrlNo", typeof(string));
            purChaseDT.Columns.Add("OrderDetails_Id", typeof(string));
            purChaseDT.Columns.Add("ProductID", typeof(string));
            purChaseDT.Columns.Add("Description", typeof(string));
            //SalesOrderdt.Columns.Add("Quotation", typeof(string));//Added By:subhabrata on 21-02-2017               
            purChaseDT.Columns.Add("Quantity", typeof(string));
            purChaseDT.Columns.Add("UOM", typeof(string));
            purChaseDT.Columns.Add("Warehouse", typeof(string));
            purChaseDT.Columns.Add("StockQuantity", typeof(string));
            purChaseDT.Columns.Add("StockUOM", typeof(string));
            //purChaseDT.Columns.Add("SalePrice", typeof(string));
            purChaseDT.Columns.Add("PurchasePrice", typeof(string));
            purChaseDT.Columns.Add("Discount", typeof(string));
            purChaseDT.Columns.Add("Amount", typeof(string));
            purChaseDT.Columns.Add("TaxAmount", typeof(string));
            purChaseDT.Columns.Add("TotalAmount", typeof(string));
            purChaseDT.Columns.Add("Status", typeof(string));
            purChaseDT.Columns.Add("Indent_No", typeof(string));
            purChaseDT.Columns.Add("Indent_Num", typeof(string));
            purChaseDT.Columns.Add("ProductName", typeof(string));
            purChaseDT.Columns.Add("IsComponentProduct", typeof(string));
            purChaseDT.Columns.Add("IsLinkedProduct", typeof(string));
            purChaseDT.Columns.Add("PurchaseOrder_InlineRemarks", typeof(string));
            purChaseDT.Columns.Add("BalQty", typeof(string));
            purChaseDT.Columns.Add("Document_DetailsID", typeof(string));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IsSuccess = true;
                DataColumnCollection dtC = dt.Columns;
                string SalePrice, Discount, Amount, TaxAmount, TotalAmount, Order_Num1, ProductName, IsComponent, IsLinkedProduct, PurchaseOrder_InlineRemarks, PurchaseOrder_AddlRemarks, Document_DetailsID;
                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["SalePrice"])))
                { SalePrice = Convert.ToString(dt.Rows[i]["SalePrice"]); }
                else
                { SalePrice = ""; }
                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Discount"])))
                { Discount = Convert.ToString(dt.Rows[i]["Discount"]); }
                else
                { Discount = ""; }
                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Amount"])))
                { Amount = Convert.ToString(dt.Rows[i]["Amount"]); }
                else { Amount = ""; }
                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["TaxAmount"])))
                { TaxAmount = Convert.ToString(dt.Rows[i]["TaxAmount"]); }
                else { TaxAmount = ""; }
                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["TotalAmount"])))
                { TotalAmount = Convert.ToString(dt.Rows[i]["TotalAmount"]); }
                else { TotalAmount = ""; }
                if (dtC.Contains("Indent"))
                {
                    Order_Num1 = Convert.ToString(dt.Rows[i]["Indent"]);//subhabrata on 21-02-2017
                }
                else
                {
                    Order_Num1 = "";
                }
                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Products_Name"])))
                {
                    ProductName = Convert.ToString(dt.Rows[i]["Products_Name"]);
                }
                else
                {
                    ProductName = "";
                }
                //Subhabrata
                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["IsComponentProduct"])))
                {
                    IsComponent = Convert.ToString(dt.Rows[i]["IsComponentProduct"]);
                }
                else
                {
                    IsComponent = "";
                }
                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["IsLinkedProduct"])))
                {
                    IsLinkedProduct = Convert.ToString(dt.Rows[i]["IsLinkedProduct"]);
                }
                else
                {
                    IsLinkedProduct = "";
                }//End
                if (dt.Columns.Contains("PurchaseOrder_InlineRemarks"))
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["PurchaseOrder_InlineRemarks"])))
                    {
                        PurchaseOrder_InlineRemarks = Convert.ToString(dt.Rows[i]["PurchaseOrder_InlineRemarks"]);
                    }
                    else
                    {
                        PurchaseOrder_InlineRemarks = "";
                    }
                }
                else
                {
                    PurchaseOrder_InlineRemarks = "";
                }

                if (dt.Columns.Contains("PurchaseOrder_AddiDesc"))
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["PurchaseOrder_AddlRemarks"])))
                    {
                        PurchaseOrder_AddlRemarks = Convert.ToString(dt.Rows[i]["PurchaseOrder_AddlRemarks"]);
                    }
                    else
                    {
                        PurchaseOrder_AddlRemarks = "";
                    }
                }
                else
                {
                    PurchaseOrder_AddlRemarks = "";
                }
                string BalQty = "0";
                if (dt.Columns.Contains("BalQty"))
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["BalQty"])))
                    {
                        BalQty = Convert.ToString(dt.Rows[i]["BalQty"]);
                    }
                    else
                    {
                        BalQty = "99999999";
                    }
                }
                else
                {
                    BalQty = "99999999";
                }

                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["QuoteDetails_Id"])))
                {
                    Document_DetailsID = Convert.ToString(dt.Rows[i]["QuoteDetails_Id"]);
                }
                else
                {
                    Document_DetailsID = "";
                }


                purChaseDT.Rows.Add(Convert.ToString(i + 1), Convert.ToString(i + 1), Convert.ToString(dt.Rows[i]["QuoteDetails_ProductId"]), Convert.ToString(dt.Rows[i]["QuoteDetails_ProductDescription"]),
                    Convert.ToString(dt.Rows[i]["QuoteDetails_Quantity"]), Convert.ToString(dt.Rows[i]["UOM_ShortName"]), "", Convert.ToString(dt.Rows[i]["QuoteDetails_StockQty"]), Convert.ToString(dt.Rows[i]["UOM_ShortName"]),
                               SalePrice, Discount, Amount, TaxAmount, TotalAmount, "U", Convert.ToInt64(dt.Rows[i]["Indent_No"]), Order_Num1, ProductName, IsComponent, IsLinkedProduct, PurchaseOrder_InlineRemarks, BalQty, Document_DetailsID);

                //Remarks.Rows.Add(Convert.ToString(i + 1), PurchaseOrder_AddlRemarks);                
            }

            Session["ProductOrderDetails"] = purChaseDT;
            //Session["InlineRemarks"] = Remarks;
            return IsSuccess;
        }//End
        #endregion

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
        protected void grid_DataBinding(object sender, EventArgs e)
        {
            //grid.DataSource = GetVoucher();
            if (Session["ProductOrderDetails"] != null)
            {
                DataTable POdt = (DataTable)Session["ProductOrderDetails"];
                DataView dvData = new DataView(POdt);
                dvData.RowFilter = "Status <> 'D'";
                grid.DataSource = GetPurchaseOrderBatch(dvData.ToTable());
            }
        }
        protected void grid_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            grid.JSProperties["cpSaveSuccessOrFail"] = null;
            grid.JSProperties["cpPurchaseOrderNo"] = null;
            DataTable PurchaseOrderdt = new DataTable();
            string IsDeleteFrom = Convert.ToString(hdfIsDelete.Value);
            DataTable AdditionalDetails = new DataTable();
            AdditionalDetails = (DataTable)Session["InlineRemarks"];
            if (Session["ProductOrderDetails"] != null)
            {
                DataTable dt = new DataTable();
                dt = (DataTable)Session["ProductOrderDetails"];
                //foreach (DataRow row in dt.Rows)
                //{
                //    DataColumnCollection dtC = dt.Columns;

                //    if (dtC.Contains("Indent_Num"))
                //    { dt.Columns.Remove("Indent_Num"); }

                //    if (dtC.Contains("BalQty"))
                //    { dt.Columns.Remove("BalQty"); }

                //    break;
                //}//End
                PurchaseOrderdt = dt;
            }
            else
            {
                PurchaseOrderdt.Columns.Add("SrlNo", typeof(string));
                PurchaseOrderdt.Columns.Add("OrderDetails_Id", typeof(string));
                PurchaseOrderdt.Columns.Add("ProductID", typeof(string));
                PurchaseOrderdt.Columns.Add("Description", typeof(string));
                PurchaseOrderdt.Columns.Add("Quantity", typeof(string));
                PurchaseOrderdt.Columns.Add("UOM", typeof(string));
                PurchaseOrderdt.Columns.Add("Warehouse", typeof(string));
                PurchaseOrderdt.Columns.Add("StockQuantity", typeof(string));
                PurchaseOrderdt.Columns.Add("StockUOM", typeof(string));
                PurchaseOrderdt.Columns.Add("PurchasePrice", typeof(string));
                PurchaseOrderdt.Columns.Add("Discount", typeof(string));
                PurchaseOrderdt.Columns.Add("Amount", typeof(string));
                PurchaseOrderdt.Columns.Add("TaxAmount", typeof(string));
                PurchaseOrderdt.Columns.Add("TotalAmount", typeof(string));
                PurchaseOrderdt.Columns.Add("Status", typeof(string));
                PurchaseOrderdt.Columns.Add("Indent_No", typeof(string));
                PurchaseOrderdt.Columns.Add("ProductName", typeof(string));
                PurchaseOrderdt.Columns.Add("IsComponentProduct", typeof(string));
                PurchaseOrderdt.Columns.Add("IsLinkedProduct", typeof(string));
                PurchaseOrderdt.Columns.Add("PurchaseOrder_InlineRemarks", typeof(string));
                PurchaseOrderdt.Columns.Add("Document_DetailsID", typeof(string));
                PurchaseOrderdt.Columns.Add("BalQty", typeof(string));
            }

            foreach (var args in e.InsertValues)
            {
                string ProductDetails = Convert.ToString(args.NewValues["gvColProduct"]);

                if (ProductDetails != "" && ProductDetails != "0")
                {
                    string SrlNo = Convert.ToString(args.NewValues["SrlNo"]);
                    string[] ProductDetailsList = ProductDetails.Split(new string[] { "||@||" }, StringSplitOptions.None);
                    string ProductID = ProductDetailsList[0];
                    string Description = ProductDetailsList[1];
                    string ProductName = Convert.ToString(args.NewValues["ProductName"]);
                    string Quantity = Convert.ToString(args.NewValues["gvColQuantity"]);                   
                    string UOM = Convert.ToString(args.NewValues["gvColUOM"]);
                    string Warehouse = Convert.ToString(args.NewValues["Warehouse"]);
                    string StockQuantity = Convert.ToString(args.NewValues["gvColStockQty"]);
                    string StockUOM = Convert.ToString(args.NewValues["gvColStockUOM"]);                  
                    string PurchasePrice = Convert.ToString(args.NewValues["gvColStockPurchasePrice"]);
                    string PurchaseOrder_InlineRemarks = Convert.ToString(args.NewValues["PurchaseOrder_InlineRemarks"]);                   
                    string Discount = Convert.ToString(args.NewValues["gvColDiscount"]);
                    string Amount = (Convert.ToString(args.NewValues["gvColAmount"]) != "") ? Convert.ToString(args.NewValues["gvColAmount"]) : "0";
                    string TaxAmount = (Convert.ToString(args.NewValues["gvColTaxAmount"]) != "") ? Convert.ToString(args.NewValues["gvColTaxAmount"]) : "0";
                    string TotalAmount = (Convert.ToString(args.NewValues["gvColTotalAmountINR"]) != "") ? Convert.ToString(args.NewValues["gvColTotalAmountINR"]) : "0";
                    string Indent_Id = (Convert.ToString(args.NewValues["Indent"]) != "") ? Convert.ToString(args.NewValues["Indent"]) : "0";
                    string IsComponentProduct = (Convert.ToString(args.NewValues["IsComponentProduct"]) != "") ? Convert.ToString(args.NewValues["IsComponentProduct"]) : "N";
                    string IsLinkedProduct = (Convert.ToString(args.NewValues["IsLinkedProduct"]) != "") ? Convert.ToString(args.NewValues["IsLinkedProduct"]) : "N";
                    string Document_DetailsID = (Convert.ToString(args.NewValues["Document_DetailsID"]) != "") ? Convert.ToString(args.NewValues["Document_DetailsID"]) : "0";

                    PurchaseOrderdt.Rows.Add(SrlNo, "0", ProductDetails, Description, Quantity, UOM, Warehouse, StockQuantity, StockUOM, PurchasePrice, Discount,
                        Amount, TaxAmount, TotalAmount, "I", Indent_Id, ProductName, "", IsLinkedProduct, PurchaseOrder_InlineRemarks);

                    if (IsComponentProduct == "Y")
                    {
                        DataTable ComponentTable = objPurchaseOrderBL.GetComponantProduct(ProductID);
                        foreach (DataRow drr in ComponentTable.Rows)
                        {
                            string sProductsID = Convert.ToString(drr["sProductsID"]);
                            string Products_Description = Convert.ToString(drr["Products_Description"]);
                            string Sales_UOM_Name = Convert.ToString(drr["Sales_UOM_Name"]);
                            string Conversion_Multiplier = Convert.ToString(drr["Conversion_Multiplier"]);
                            string Stk_UOM_Name = Convert.ToString(drr["Stk_UOM_Name"]);
                            string Product_PurPrice = Convert.ToString(drr["Product_PurPrice"]);
                            string Products_Name = Convert.ToString(drr["Products_Name"]);
                            string StkQty = Convert.ToString(Convert.ToDecimal(Quantity) * Convert.ToDecimal(Conversion_Multiplier));

                         
                            PurchaseOrderdt.Rows.Add(Convert.ToString(Convert.ToInt32(SrlNo) + 1), "0", sProductsID, Products_Description, Quantity, Sales_UOM_Name,
                                Warehouse, StkQty, Stk_UOM_Name, Product_PurPrice, "0.0",
                       "0.0", "0.0", "0.0", "I", Indent_Id, Products_Name, "", "Y", "", Document_DetailsID);
                        }
                    }
                }
            }
            foreach (var args in e.UpdateValues)
            {
                string SrlNo = Convert.ToString(args.NewValues["SrlNo"]);
                string OrderDetails_Id = Convert.ToString(args.Keys["OrderDetails_Id"]);
                string ProductDetails = Convert.ToString(args.NewValues["gvColProduct"]);

                bool isDeleted = false;
                foreach (var arg in e.DeleteValues)
                {
                    string DeleteID = Convert.ToString(arg.Keys["OrderDetails_Id"]);
                    if (DeleteID == OrderDetails_Id)
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

                        //string Description = ProductDetailsList[1];
                        string Description = Convert.ToString(args.NewValues["gvColDiscription"]); 
                        
                        string Quantity = Convert.ToString(args.NewValues["gvColQuantity"]);                       
                        string ProductName = Convert.ToString(args.NewValues["ProductName"]);
                        string UOM = Convert.ToString(args.NewValues["gvColUOM"]);
                        string Warehouse = Convert.ToString(args.NewValues["Warehouse"]);
                        string StockQuantity = Convert.ToString(args.NewValues["gvColStockQty"]);
                        string StockUOM = Convert.ToString(args.NewValues["gvColStockUOM"]);                    
                        string PurchasePrice = Convert.ToString(args.NewValues["gvColStockPurchasePrice"]);                      
                        string PurchaseOrder_InlineRemarks = Convert.ToString(args.NewValues["PurchaseOrder_InlineRemarks"]);
                        string Discount = Convert.ToString(args.NewValues["gvColDiscount"]);
                        string Amount = (Convert.ToString(args.NewValues["gvColAmount"]) != "") ? Convert.ToString(args.NewValues["gvColAmount"]) : "0";
                        string TaxAmount = (Convert.ToString(args.NewValues["gvColTaxAmount"]) != "") ? Convert.ToString(args.NewValues["gvColTaxAmount"]) : "0";
                        string TotalAmount = (Convert.ToString(args.NewValues["gvColTotalAmountINR"]) != "") ? Convert.ToString(args.NewValues["gvColTotalAmountINR"]) : "0";
                        string Indent_Id = (Convert.ToString(args.NewValues["Indent"]) != "") ? Convert.ToString(args.NewValues["Indent"]) : "0";
                        string IsComponentProduct = (Convert.ToString(args.NewValues["IsComponentProduct"]) != "") ? Convert.ToString(args.NewValues["IsComponentProduct"]) : "N";
                        string IsLinkedProduct = (Convert.ToString(args.NewValues["IsLinkedProduct"]) != "") ? Convert.ToString(args.NewValues["IsLinkedProduct"]) : "N";
                        string Document_DetailsID = (Convert.ToString(args.NewValues["Document_DetailsID"]) != "") ? Convert.ToString(args.NewValues["Document_DetailsID"]) : "0";
                        string BalQty = (Convert.ToString(args.NewValues["BalQty"]) != "") ? Convert.ToString(args.NewValues["BalQty"]) : "0";
                        bool Isexists = false;
                        foreach (DataRow drr in PurchaseOrderdt.Rows)
                        {
                            string OldOrderDetailsId = Convert.ToString(drr["OrderDetails_Id"]);

                            if (OldOrderDetailsId == OrderDetails_Id)
                            {
                                Isexists = true;

                                drr["ProductID"] = ProductDetails;
                                drr["Description"] = Description;
                                drr["Quantity"] = Quantity;
                                drr["UOM"] = UOM;
                                drr["Warehouse"] = Warehouse;
                                drr["StockQuantity"] = StockQuantity;
                                drr["StockUOM"] = StockUOM;
                                drr["PurchasePrice"] = PurchasePrice;
                                drr["Discount"] = Discount;
                                drr["Amount"] = Amount;
                                drr["TaxAmount"] = TaxAmount;
                                drr["TotalAmount"] = TotalAmount;
                                drr["Status"] = "U";
                                if (!string.IsNullOrEmpty(Indent_Id))
                                { drr["Indent_No"] = Indent_Id; }
                                drr["ProductName"] = ProductName;
                                drr["IsComponentProduct"] = IsComponentProduct;
                                drr["IsLinkedProduct"] = IsLinkedProduct;
                                drr["PurchaseOrder_InlineRemarks"] = PurchaseOrder_InlineRemarks;
                                drr["Document_DetailsID"] = Document_DetailsID;
                                drr["BalQty"] = BalQty;
                                break;
                            }
                        }

                        if (Isexists == false)
                        {
                            PurchaseOrderdt.Rows.Add(SrlNo, OrderDetails_Id, ProductDetails, Description, Quantity, UOM, Warehouse, StockQuantity, StockUOM,
                                PurchasePrice, Discount, Amount, TaxAmount, TotalAmount, "U", Indent_Id, ProductName, IsComponentProduct, IsLinkedProduct, PurchaseOrder_InlineRemarks, Document_DetailsID, BalQty);
                        }
                        if (IsComponentProduct == "Y")
                        {
                            DataTable ComponentTable = objPurchaseOrderBL.GetComponantProduct(ProductID);
                            foreach (DataRow drr in ComponentTable.Rows)
                            {
                                string sProductsID = Convert.ToString(drr["sProductsID"]);
                                string Products_Description = Convert.ToString(drr["Products_Description"]);
                                string Sales_UOM_Name = Convert.ToString(drr["Sales_UOM_Name"]);
                                string Conversion_Multiplier = Convert.ToString(drr["Conversion_Multiplier"]);
                                string Stk_UOM_Name = Convert.ToString(drr["Stk_UOM_Name"]);
                                string Product_PurPrice = Convert.ToString(drr["Product_PurPrice"]);
                                string Products_Name = Convert.ToString(drr["Products_Name"]);
                                string StkQty = Convert.ToString(Convert.ToDecimal(Quantity) * Convert.ToDecimal(Conversion_Multiplier));

                               
                                PurchaseOrderdt.Rows.Add(Convert.ToString(Convert.ToInt32(SrlNo) + 1), "0", sProductsID, Products_Description, Quantity, Sales_UOM_Name,
                                    Warehouse, StkQty, Stk_UOM_Name, Product_PurPrice, "0.0",
                           "0.0", "0.0", "0.0", "I", Indent_Id, ProductName, "", "Y", "", Document_DetailsID, BalQty);
                            }
                        }
                    }
                }
            }

            foreach (var args in e.DeleteValues)
            {
                string OrderDetails_Id = Convert.ToString(args.Keys["OrderDetails_Id"]);

                for (int i = AdditionalDetails.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = PurchaseOrderdt.Rows[i];
                    string delOrderDetailsId = Convert.ToString(dr["OrderDetails_Id"]);
                    DataRow daddr = AdditionalDetails.Rows[i];
                    if (delOrderDetailsId == OrderDetails_Id)
                        daddr.Delete();
                }

                AdditionalDetails.AcceptChanges();

                for (int i = PurchaseOrderdt.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = PurchaseOrderdt.Rows[i];
                    string delOrderDetailsId = Convert.ToString(dr["OrderDetails_Id"]);
                    if (delOrderDetailsId == OrderDetails_Id)
                        dr.Delete();
                }
                PurchaseOrderdt.AcceptChanges();

                if (OrderDetails_Id.Contains("~") != true)
                {
                    PurchaseOrderdt.Rows.Add("0", OrderDetails_Id, "0", "", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "D", "0", "", "", "","","0");                    
                }
            }        

            int j = 1;
            foreach (DataRow dr in PurchaseOrderdt.Rows)
            {
                string Status = Convert.ToString(dr["Status"]);
                dr["SrlNo"] = j.ToString();             
               
                if (Status != "D")
                {
                    if (Status == "I")
                    {
                        string strID = Convert.ToString("Q~" + j);
                        dr["OrderDetails_Id"] = strID;
                    }
                    j++;
                }
            }
            PurchaseOrderdt.AcceptChanges();          
            Session["InlineRemarks"] = AdditionalDetails;
            Session["ProductOrderDetails"] = PurchaseOrderdt;
            if (IsDeleteFrom != "D" && IsDeleteFrom != "C")
            {
                string ActionType = Convert.ToString(ViewState["ActionType"]);
                string IsInventory = Convert.ToString(ddlInventory.SelectedValue);
                string PurchaseOrder_Id = Convert.ToString(Session["PurchaseOrder_Id"]);
                string strSchemeType = Convert.ToString(ddl_numberingScheme.SelectedValue);
                string strPurchaseNumber = Convert.ToString(txtVoucherNo.Text);
                string strPurchaseDate = Convert.ToString(dt_PLQuote.Date);
                String IndentRequisitionNo = "";
                string IndentRequisitionDate = string.Empty;
                for (int i = 0; i < taggingGrid.GetSelectedFieldValues("Indent_Id").Count; i++)
                {
                    IndentRequisitionNo += "," + Convert.ToString(taggingGrid.GetSelectedFieldValues("Indent_Id")[i]);
                }
                IndentRequisitionNo = IndentRequisitionNo.TrimStart(',');

                if (taggingGrid.GetSelectedFieldValues("Indent_Id").Count == 1)
                {
                    IndentRequisitionDate = Convert.ToString(dt_Quotation.Text);
                }                
                string strVendor = Convert.ToString(hdfLookupCustomer.Value);
                string strContactName = Convert.ToString(cmbContactPerson.Value);
                string Reference = Convert.ToString(txt_Refference.Text);
                string strBranch = Convert.ToString(ddl_Branch.SelectedValue);            
                string strAgents = Convert.ToString("0");
                string strCurrency = Convert.ToString(ddl_Currency.SelectedValue);
                string PosForGst = Convert.ToString(PurchaseOrderPosGst.Value);
                string strRate = Convert.ToString(txt_Rate.Value);
                string strTaxOption = Convert.ToString(ddl_AmountAre.Value);
                string strTaxCode = Convert.ToString(ddl_VatGstCst.Value).Split('~')[0];
                string CompanyID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string currency = Convert.ToString(HttpContext.Current.Session["LocalCurrency"]);
                string[] ActCurrency = currency.Split('~');
                int BaseCurrencyId = Convert.ToInt32(ActCurrency[0]);
                string strCreditDays = Convert.ToString(txtCreditDays.Text);
                string strPoDate = Convert.ToString(dt_PODue.Date);
                string AppRejRemarks = Convert.ToString(txtAppRejRemarks.Text);
                string projectValidFrom = Convert.ToString(dtProjValidFrom.Text);
                string projectValidUpto = Convert.ToString(dtProjValidUpto.Text);
                string RevisionNo = Convert.ToString(txtRevisionNo.Text);
                string RevisionDate = "";
                if (hdnApprovalReqInq.Value == "1")
                {
                     RevisionDate = Convert.ToString(txtRevisionDate.Text);
                }
                DataTable tempQuotation = PurchaseOrderdt.Copy();
                foreach (DataRow dr in tempQuotation.Rows)
                {
                    string Status = Convert.ToString(dr["Status"]);
                    if (Status == "I")
                    {
                        dr["OrderDetails_Id"] = "0";
                        string[] list = Convert.ToString(dr["ProductID"]).Split(new string[] { "||@||" }, StringSplitOptions.None);
                        dr["ProductID"] = Convert.ToString(list[0]);
                        dr["UOM"] = Convert.ToString(list[3]);
                        dr["StockUOM"] = Convert.ToString(list[5]);
                    }
                    else if (Status == "U" || Status == "")
                    {
                        string[] list = Convert.ToString(dr["ProductID"]).Split(new string[] { "||@||" }, StringSplitOptions.None);
                        dr["ProductID"] = Convert.ToString(list[0]);
                        dr["UOM"] = Convert.ToString(list[3]);
                        dr["StockUOM"] = Convert.ToString(list[5]);
                        string OrderDetails_Id = Convert.ToString(dr["OrderDetails_Id"]);
                        if (OrderDetails_Id.Contains("~") == true)
                        {
                            dr["OrderDetails_Id"] = "0";
                        }
                    }
                }
                tempQuotation.AcceptChanges();

                #region DataTable of Inline Tax

                DataTable TaxDetaildt = new DataTable();
                if (Session["PurchaseOrderFinalTaxRecord"] != null)
                {
                    TaxDetaildt = (DataTable)Session["PurchaseOrderFinalTaxRecord"];
                }

                #endregion Inline Tax End
                #region DataTable Of Purchase Order Tax Details

                DataTable POTaxDetailsdt = new DataTable();
                if (Session["POTaxDetails"] != null)
                {
                    POTaxDetailsdt = (DataTable)Session["POTaxDetails"];
                }
                else
                {
                    POTaxDetailsdt.Columns.Add("Taxes_ID", typeof(string));
                    POTaxDetailsdt.Columns.Add("Taxes_Name", typeof(string));
                    POTaxDetailsdt.Columns.Add("Percentage", typeof(string));
                    POTaxDetailsdt.Columns.Add("Amount", typeof(string));
                    POTaxDetailsdt.Columns.Add("AltTax_Code", typeof(string));
                }

                DataTable tempTaxDetailsdt = new DataTable();
                tempTaxDetailsdt = POTaxDetailsdt.DefaultView.ToTable(false, "Taxes_ID", "Percentage", "Amount", "AltTax_Code");

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

                #endregion DataTable Of Purchase Order Tax Details
                #region DataTable of Warehouse

                DataTable tempWarehousedt = new DataTable();
                //if (Session["POWarehouseData"] != null)
                //{
                //    DataTable Warehousedt = (DataTable)Session["POWarehouseData"];
                //    tempWarehousedt = Warehousedt.DefaultView.ToTable(false, "Product_SrlNo", "LoopID", "WarehouseID", "TotalQuantity", "BatchID", "SerialID");
                //}
                //else
                //{
                //    tempWarehousedt.Columns.Add("Product_SrlNo", typeof(string));
                //    tempWarehousedt.Columns.Add("SrlNo", typeof(string));
                //    tempWarehousedt.Columns.Add("WarehouseID", typeof(string));
                //    tempWarehousedt.Columns.Add("TotalQuantity", typeof(string));
                //    tempWarehousedt.Columns.Add("BatchID", typeof(string));
                //    tempWarehousedt.Columns.Add("SerialID", typeof(string));
                //}

                #endregion Warehouse End
                string validate = "";

                // DataTable Of Billing Address
                #region ##### Added By : Samrat Roy -- to get BillingShipping user control data
                DataTable tempBillAddress = new DataTable();

                //Edited vbelow line by chinmoy
                tempBillAddress = Purchase_BillingShipping.GetBillingShippingTable();

                #region #### Old_Process ####
                ////DataTable tempBillAddress = new DataTable();
                ////if (Session["PurchaseOrderAddressDtl"] != null)
                ////{
                ////    tempBillAddress = (DataTable)Session["PurchaseOrderAddressDtl"];
                ////}
                ////else
                ////{
                ////    tempBillAddress = StoreSalesOrderAddressDetail();
                ////}

                #endregion

                #endregion
                // End
                #region Sandip Section For Approval Section Start
                string approveStatus = "";
                if (Request.QueryString["status"] != null)
                {
                    approveStatus = Convert.ToString(Request.QueryString["status"]);
                }

                #endregion Sandip Section For Approval Section end
                if (ActionType == "Add")
                {
                    string[] SchemeList = strSchemeType.Split(new string[] { "~" }, StringSplitOptions.None);
                    validate = checkNMakeJVCode(strPurchaseNumber, Convert.ToInt32(SchemeList[0]));
                    if (!reCat.isAllMandetoryDone((DataTable)Session["UdfDataOnAdd"], "PO"))
                    {
                        validate = "UdfMandetory";
                    }
                }
                DataView dvData = new DataView(tempQuotation);
                dvData.RowFilter = "Status<>'D'";
                DataTable dt_tempQuotation = dvData.ToTable();

                var duplicateRecords = dt_tempQuotation.AsEnumerable()
               .GroupBy(r => r["ProductID"]) //coloumn name which has the duplicate values
               .Where(gr => gr.Count() > 1)
                .Select(g => g.Key);

                //foreach (var d in duplicateRecords)
                //{
                //    validate = "duplicateProduct";
                //}
                if (ddlInventory.SelectedValue != "N")
                {
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
                }
                string pageTyper = Convert.ToString(Request.QueryString["key"]);


                if (pageTyper.ToUpper() == "ADD")
                {
                    if (taggingGrid.GetSelectedFieldValues("Indent_Id").Count > 0)
                    {
                        if (tempQuotation.Rows.Count > 0)
                        {
                            foreach (DataRow dr in tempQuotation.Rows)
                            {
                                DataTable dtq = new DataTable();
                                string ProductID = Convert.ToString(dr["ProductID"]);
                                decimal ProductQuantity = Convert.ToDecimal(dr["Quantity"]);
                                string Status = Convert.ToString(dr["Status"]);
                                string Document_DetailsID = Convert.ToString(dr["Document_DetailsID"]);
                                if (rdl_Salesquotation.SelectedValue == "Indent")
                                {
                                    dtq = oDBEngine.GetDataTable("select isnull(BalanceQty,0) TotQty from tbl_trans_RequisitionBalanceMapForPurchaseOrder where  RequisitionId='" + Convert.ToInt32(dr["Indent_No"]) + "' and ProductId='" + ProductID + "' and Requisition_Details_ID='" + Document_DetailsID + "'");
                                    if (Status != "D" && dtq.Rows.Count > 0)
                                    {
                                        if (ProductQuantity > Convert.ToDecimal(dtq.Rows[0]["TotQty"]))
                                        {
                                            validate = "ExceedQuantity";
                                            break;
                                        }
                                    }
                                }
                                if (rdl_Salesquotation.SelectedValue == "Quotation")
                                {
                                    dtq = oDBEngine.GetDataTable("select isnull(BalanceQty,0) TotQty from tbl_trans_PurchaseQuotationBalancemap where  PurchaseQuotationDocId='" + Convert.ToInt32(dr["Indent_No"]) + "' and ProductId='" + ProductID + "' and Details_id='" + Document_DetailsID + "'");
                                    if (Status != "D" && dtq.Rows.Count > 0)
                                    {
                                        if (ProductQuantity > Convert.ToDecimal(dtq.Rows[0]["TotQty"]))
                                        {
                                            validate = "ExceedQuantity";
                                            break;
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
                else
                  {
                    if(tempQuotation.Rows.Count>0)
                    {
                        foreach (DataRow dr in tempQuotation.Rows)
                        {
                            DataTable dtq = new DataTable();
                            string ProductID = Convert.ToString(dr["ProductID"]);
                            decimal ProductQuantity = Convert.ToDecimal(dr["Quantity"]);
                            string Status = Convert.ToString(dr["Status"]);
                            string Document_DetailsID = Convert.ToString(dr["Document_DetailsID"]);
                            if (rdl_Salesquotation.SelectedValue == "Indent")
                            {
                                dtq = oDBEngine.GetDataTable("select (ISNULL(TotalQty,0)+isnull(BalanceQty,0)) TotQty from tbl_trans_RequisitionBalanceMapForPurchaseOrder where  RequisitionId='" + Convert.ToInt32(dr["Indent_No"]) + "' and ProductId='" + ProductID + "' and Requisition_Details_ID='" + Document_DetailsID + "'");
                                if (Status != "D" && dtq.Rows.Count>0) 
                                {
                                    if (ProductQuantity > Convert.ToDecimal(dtq.Rows[0]["TotQty"]))
                                    {
                                        validate = "ExceedQuantity";
                                        break;
                                    }
                                }
                            }
                            if (rdl_Salesquotation.SelectedValue == "Quotation")
                            {
                                dtq = oDBEngine.GetDataTable("select (ISNULL(TotalQty,0)+isnull(BalanceQty,0)) TotQty from tbl_trans_PurchaseQuotationBalancemap where  PurchaseQuotationDocId='" + Convert.ToInt32(dr["Indent_No"]) + "' and ProductId='" + ProductID + "'and Details_id='" + Document_DetailsID + "'");
                                if (Status != "D" && dtq.Rows.Count > 0)
                                {
                                    if (ProductQuantity > Convert.ToDecimal(dtq.Rows[0]["TotQty"]))
                                    {
                                        validate = "ExceedQuantity";
                                        break;
                                    }
                                }
                            }

                        }
                    }
                
                }




                //// ############# Added By : Samrat Roy -- 02/05/2017 -- To check Transporter Mandatory Control 
                //  BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

                BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();

                DataTable DT = objEngine.GetDataTable("Config_SystemSettings", " Variable_Value ", " Variable_Name='Transporter_POMandatory' AND IsActive=1");
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

                #region Terms & Conditions
                DataTable DT_TC = objEngine.GetDataTable("Config_SystemSettings", " Variable_Value ", " Variable_Name='TC_POMandatory' AND IsActive=1");
                if (DT_TC != null && DT_TC.Rows.Count > 0)
                {
                    string IsMandatory = Convert.ToString(DT_TC.Rows[0]["Variable_Value"]).Trim();

                    //objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

                    objEngine = new BusinessLogicLayer.DBEngine();

                    DataTable DTVisible = objEngine.GetDataTable("Config_SystemSettings", " Variable_Value ", " Variable_Name='Show_TC_PO' AND IsActive=1");
                    if (Convert.ToString(DTVisible.Rows[0]["Variable_Value"]).Trim() == "Yes")
                    {
                        if (IsMandatory == "Yes")
                        {
                            //if (TermsConditionsControl.GetControlValue("dtDeliveryDate") == "" || TermsConditionsControl.GetControlValue("dtDeliveryDate") == "@")
                            if (TermsConditionsControl.GetControlVisibility("dtDeliveryDate") && (TermsConditionsControl.GetControlValue("dtDeliveryDate") == "" || TermsConditionsControl.GetControlValue("dtDeliveryDate") == "@"))
                            {
                                validate = "TCMandatory";
                            }
                        }
                    }
                }
                #endregion

                #region Section For Stock Section

                if (IsBarcodeGeneratete() == true)
                {
                    if (ddlInventory.SelectedValue != "N")
                    {
                        if (Session["POwarehousedetailstemp"] != null)
                        {
                            foreach (DataRow dr in tempQuotation.Rows)
                            {
                                string ProductID = Convert.ToString(dr["ProductID"]);
                                string strSrlNo = Convert.ToString(dr["SrlNo"]);
                                decimal strProductQuantity = Convert.ToDecimal(dr["Quantity"]);
                                string Status = Convert.ToString(dr["Status"]);
                                string IsLinkedProduct = Convert.ToString(dr["IsLinkedProduct"]);

                                decimal strWarehouseQuantity = 0;
                                DataTable _Stockdt = (DataTable)Session["POwarehousedetailstemp"];
                                GetQuantityBaseOnProduct(_Stockdt, ProductID, strSrlNo, ref strWarehouseQuantity);

                                if (ProductID != "")
                                {
                                    if (CheckProduct_IsInventory(ProductID) == true)
                                    {
                                        if (Status != "D")
                                        {
                                            if (IsLinkedProduct != "Y")
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
                                }
                            }
                        }
                        else
                        {
                            validate = "nullWarehouse";
                        }
                    }
                }

                #endregion
                //Added By chinmoy
                string _ShippingState = "";

                if (PurchaseOrderPosGst.Value.ToString() == "S")
                {
                    _ShippingState = Convert.ToString(Purchase_BillingShipping.GetShippingStateId());
                }
                else
                {
                    _ShippingState = Convert.ToString(Purchase_BillingShipping.GetBillingStateId());
                }
                if (_ShippingState == "0")
                {
                    validate = "BillingShippingNotLoaded";
                }

                #region Checking of Registered Billing/Shipping of Vendor if not exist will not allow to save

                PurchaseInvoiceBL objPurchaseInvoice = new PurchaseInvoiceBL();
                int cnt = objPurchaseInvoice.CheckBillingShippingofVendor(Convert.ToString(hdnCustomerId.Value));
                if (cnt != 1)
                {
                    validate = "VendorAddressProblem";
                }
                //chinmoy comment start 03-12-2019
                //else
                //{
                //    validate = "VendorAddressSuccess";
                //}
                //end
                #endregion Checking of Registered Billing/Shipping of Vendor if not exist will not allow to save
                if (validate == "outrange" || validate == "BillingShippingNotLoaded" || validate == "duplicate" || validate == "UdfMandetory" || validate == "transporteMandatory" || validate == "TCMandatory" || validate == "nullQuantity" || validate == "duplicateProduct" || validate == "checkWarehouse" || validate == "nullWarehouse" || validate == "VendorAddressProblem" || validate=="ExceedQuantity")
                {
                    grid.JSProperties["cpSaveSuccessOrFail"] = validate;
                }
                else
                {
                    if (IsPOExist(IndentRequisitionNo))
                    {

                        string TaxType = "", ShippingState = "";
                        //Edited By chinmoy
                        // ShippingState = Convert.ToString(Purchase_BillingShipping.GetShippingStateId());
                        if (PurchaseOrderPosGst.Value.ToString() == "S")
                        {
                            ShippingState = Convert.ToString(Purchase_BillingShipping.GetShippingStateId());
                        }
                        else
                        {
                            ShippingState = Convert.ToString(Purchase_BillingShipping.GetBillingStateId());
                        }

                        if (ddl_AmountAre.Value == "1") TaxType = "E";
                        else if (ddl_AmountAre.Value == "2") TaxType = "I";

                        #region Delete Product Tax

                        string TaxDetails = Convert.ToString(hdnJsonProductTax.Value);
                        JavaScriptSerializer ser = new JavaScriptSerializer();
                        ser.MaxJsonLength = Int32.MaxValue;
                        List<ProductTaxDetails> TaxEntryJson = ser.Deserialize<List<ProductTaxDetails>>(TaxDetails);


                        foreach (DataRow productRow in tempQuotation.Rows)
                        {
                            string _ProductID = Convert.ToString(productRow["ProductID"]);
                            string _SrlNo = Convert.ToString(productRow["SrlNo"]);
                            string _IsEntry = "";

                            if (_ProductID != "")
                            {
                                var TaxValue = TaxEntryJson.Where(x => x.SrlNo == _SrlNo).ToList().SingleOrDefault();

                                if (TaxValue != null)
                                {
                                    _IsEntry = TaxValue.IsTaxEntry;

                                    if (_IsEntry == "N")
                                    {
                                        DataRow[] deletedRow = TaxDetaildt.Select("SlNo=" + _SrlNo);
                                        if (deletedRow.Length > 0)
                                        {
                                            foreach (DataRow dr in deletedRow)
                                            {
                                                TaxDetaildt.Rows.Remove(dr);
                                            }
                                            TaxDetaildt.AcceptChanges();
                                        }
                                    }
                                }
                            }
                        }

                        #endregion

                      //  TaxDetaildt = gstTaxDetails.SetTaxTableDataWithProductSerialForPurchaseRoundOff(ref tempQuotation, "SrlNo", "ProductID", "Amount", "TaxAmount", "TotalAmount", TaxDetaildt, "P", dt_PLQuote.Date.ToString("yyyy-MM-dd"), strBranch, ShippingState, TaxType, Convert.ToString(hdnCustomerId.Value));

                        TaxDetaildt = gstTaxDetails.SetTaxTableDataWithProductSerialForPurchaseRoundOffWithException(ref tempQuotation, "SrlNo", "ProductID", "Amount", "TaxAmount", "TotalAmount", TaxDetaildt, "P", dt_PLQuote.Date.ToString("yyyy-MM-dd"), strBranch, ShippingState, TaxType, Convert.ToString(hdnCustomerId.Value), "Quantity", "PO");

                        #region Add New Filed To Check from Table

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


                        //Rev Tanmoy Project
                        CommonBL cbl = new CommonBL();
                        string ProjectSelectcashbankModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");

                        Int64 ProjId = 0;
                        if (lookup_Project.Text != "")
                        {
                            string projectCode = lookup_Project.Text;
                            DataTable dt = oDBEngine.GetDataTable("select Proj_Id from Master_ProjectManagement where Proj_Code='" + projectCode + "'");
                            ProjId = Convert.ToInt64(dt.Rows[0]["Proj_Id"]);
                        }
                        else if (lookup_Project.Text == "")
                        {
                            ProjId = 0;
                        }
                        //End Rev Tanmoy Project

                        DataTable addrDesc = new DataTable();
                        addrDesc = (DataTable)Session["InlineRemarks"];


                            if (hdnApproveStatus.Value == "")
                             {
                                 hdnApproveStatus.Value = "0";
                             }
                             int ApproveRejectstatus = Convert.ToInt32(hdnApproveStatus.Value);
                             string TagDocType = Convert.ToString(rdl_Salesquotation.SelectedValue);
                             int firstApprove = 0;
                             if (hdnApproveStatus.Value == "1")
                             {
                                 if (hdnisFirstApprove.Value == "No" && hdnRevisionRequiredEveryAfterApproval.Value == "Yes")
                                 {
                                     firstApprove = 1;
                                 }
                             }
                             

                         int id=(  AddModifyPurchaseOrder(PurchaseOrder_Id, UniquePurchaseNumber, strPurchaseDate, IndentRequisitionNo, IndentRequisitionDate, strVendor, strContactName, TagDocType, addrDesc,
                        Reference, strBranch, strAgents, strCurrency, strRate, strTaxOption, strTaxCode, CompanyID, BaseCurrencyId, dt_tempQuotation, ActionType, IsInventory,
                        tempWarehousedt, TaxDetaildt, tempTaxDetailsdt, tempBillAddress, approveStatus, strCreditDays, strPoDate, PosForGst, duplicatedt2, ProjId, AppRejRemarks, projectValidFrom
                        , projectValidUpto, RevisionNo, RevisionDate, ApproveRejectstatus, firstApprove));
                         if (id <= 0)
                        {
                            if (id == 0)
                            {
                                grid.JSProperties["cpSaveSuccessOrFail"] = "errorInsert";
                            }
                            else if(id == -12)
                            {
                                grid.JSProperties["cpSaveSuccessOrFail"] = "EmptyProject";
                            }
                                 
                        }
                        else
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

                            grid.JSProperties["cpPurchaseOrderNo"] = UniquePurchaseNumber;

                            #region To Show By Default Cursor after SAVE AND NEW
                            if (ActionType == "Add")
                            {
                                string schemavalue = ddl_numberingScheme.SelectedValue;
                                Session["schemavaluePO"] = schemavalue;
                                Session["POType"] = IsInventory;
                                string schematype = strPurchaseNumber.Trim();
                                if (schematype == "Auto")
                                {
                                    Session["SaveModePO"] = "A";
                                }
                                else
                                {
                                    Session["SaveModePO"] = "M";
                                }
                            }

                            if (Session["ProductOrderDetails"] != null)
                            {
                                Session["ProductOrderDetails"] = null;
                            }
                            #endregion
                        }

                        
                    }
                    else
                    {
                        grid.JSProperties["cpSaveSuccessOrFail"] = "Ponotexist";
                    }
                }
            }
            else
            {
                DataView dvData = new DataView(PurchaseOrderdt);
                dvData.RowFilter = "Status <> 'D'";
                grid.DataSource = GetPurchaseOrderBatch(dvData.ToTable());
                grid.DataBind();
            }
        }

        public DataTable GetProjectEditData(string VendorePayID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CustomerPaymentReciptProjectID");
            proc.AddIntegerPara("@Receipt_ID", Convert.ToInt32(VendorePayID));
            proc.AddVarcharPara("@Action", 100, "Purchase_Order");
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetProjectEditDataIndent(string VendorePayID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CustomerPaymentReciptProjectID");
            proc.AddIntegerPara("@Receipt_ID", Convert.ToInt32(VendorePayID));
            proc.AddVarcharPara("@Action", 100, "Project_Indent");
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetProjectEditDataQuatation(string VendorePayID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CustomerPaymentReciptProjectID");
            proc.AddIntegerPara("@Receipt_ID", Convert.ToInt32(VendorePayID));
            proc.AddVarcharPara("@Action", 100, "ProjPurchase_Quotation");
            dt = proc.GetTable();
            return dt;
        }
        public void GetQuantityBaseOnProduct(DataTable Warehousedt, string ProductID, string strProductSrlNo, ref decimal WarehouseQty)
        {
            decimal sum = 0;
            string Type = "";
            GetProductType(ProductID, ref Type);

            if (Warehousedt != null)
            {
                if (Type == "WS" || Type == "WBS" || Type == "BS" || Type == "S")
                {
                    for (int i = 0; i < Warehousedt.Rows.Count; i++)
                    {
                        DataRow dr = Warehousedt.Rows[i];
                        string Product_SrlNo = Convert.ToString(dr["pcslno"]);

                        if (strProductSrlNo == Product_SrlNo)
                        {
                            sum = sum + 1;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < Warehousedt.Rows.Count; i++)
                    {
                        DataRow dr = Warehousedt.Rows[i];
                        string Product_SrlNo = Convert.ToString(dr["pcslno"]);

                        if (strProductSrlNo == Product_SrlNo)
                        {
                            string strQuantity = (Convert.ToString(dr["Quantitysum"]) != "") ? Convert.ToString(dr["Quantitysum"]) : "0";
                            sum = sum + Convert.ToDecimal(strQuantity);
                        }
                    }
                }
            }


            WarehouseQty = sum;
        }
        public void GetProductType(string ProductID, ref string Type)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 500, "GetSchemeType");
            proc.AddVarcharPara("@ProductID", 100, Convert.ToString(ProductID));
            DataTable dt = proc.GetTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                Type = Convert.ToString(dt.Rows[0]["Type"]);
            }
        }
        public bool CheckProduct_IsInventory(string ProductID)
        {
            bool IsInventory = true;

            //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();

            DataTable DT = objEngine.GetDataTable("Master_sProducts", " sProduct_IsInventory ", " sProducts_ID='" + ProductID + "'");
            if (DT != null && DT.Rows.Count > 0)
            {
                string strIsInventory = Convert.ToString(DT.Rows[0]["sProduct_IsInventory"]).Trim();

                if (strIsInventory == "True") IsInventory = true;
                else IsInventory = false;
            }

            return IsInventory;
        }
        public bool IsBarcodeGeneratete()
        {
            bool IsGeneratete = false;

            try
            {
                //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

                BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();

                DataTable DT_TC = objEngine.GetDataTable("tbl_Master_SystemControl", " BarcodeGeneration ", null);
                if (DT_TC != null && DT_TC.Rows.Count > 0)
                {
                    IsGeneratete = Convert.ToBoolean(DT_TC.Rows[0]["BarcodeGeneration"]);
                }

                return IsGeneratete;
            }
            catch
            {
                return IsGeneratete;
            }
        }
        public bool IsPOExist(string pcid)
        {
            bool IsExist = false;
            if (pcid != "" && Convert.ToString(pcid).Trim() != "")
            {
                DataTable dt = new DataTable();

                var elements = pcid.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
                foreach (string items in elements)
                {
                    if (rdl_Salesquotation.SelectedValue == "Indent")
                    {
                        dt = oDBEngine.GetDataTable("select Count(*) as isexist from tbl_trans_Indent where Indent_Id=" + items + "");
                    }
                    else if (rdl_Salesquotation.SelectedValue == "Quotation")
                    {
                        dt = oDBEngine.GetDataTable("select Count(*) as isexist from tbl_trans_PurchaseQuotation where PurchaseQuotation_Id=" + items + "");
                    }
                    if (dt.Rows.Count > 0 && Convert.ToString(dt.Rows[0]["isexist"]) != "0")
                    {
                        IsExist = true;
                    }
                }

            }
            else
            {
                IsExist = true;
            }

            return IsExist;
        }
        public IEnumerable GetPurchaseOrderBatch(DataTable Quotationdt)
        {
            List<PurchaseOrderList> PurchaseOrderList = new List<PurchaseOrderList>();
            //DataTable Quotationdt = GetPurchaseOrderData().Tables[0];
            DataColumnCollection dtC = Quotationdt.Columns;
            for (int i = 0; i < Quotationdt.Rows.Count; i++)
            {
                PurchaseOrderList PurchaseOrders = new PurchaseOrderList();

                PurchaseOrders.SrlNo = Convert.ToString(Quotationdt.Rows[i]["SrlNo"]);
                PurchaseOrders.OrderDetails_Id = Convert.ToString(Quotationdt.Rows[i]["OrderDetails_Id"]);
                PurchaseOrders.gvColProduct = Convert.ToString(Quotationdt.Rows[i]["ProductID"]);
                PurchaseOrders.gvColDiscription = Convert.ToString(Quotationdt.Rows[i]["Description"]);
                PurchaseOrders.gvColQuantity = Convert.ToString(Quotationdt.Rows[i]["Quantity"]);
                PurchaseOrders.gvColUOM = Convert.ToString(Quotationdt.Rows[i]["UOM"]);
                PurchaseOrders.Warehouse = "";
                PurchaseOrders.gvColStockQty = Convert.ToString(Quotationdt.Rows[i]["StockQuantity"]);
                PurchaseOrders.gvColStockUOM = Convert.ToString(Quotationdt.Rows[i]["StockUOM"]);
                PurchaseOrders.gvColStockPurchasePrice = Convert.ToString(Quotationdt.Rows[i]["PurchasePrice"]);
                PurchaseOrders.gvColDiscount = Convert.ToString(Quotationdt.Rows[i]["Discount"]);
                PurchaseOrders.gvColAmount = Convert.ToString(Quotationdt.Rows[i]["Amount"]);
                PurchaseOrders.gvColTaxAmount = Convert.ToString(Quotationdt.Rows[i]["TaxAmount"]);
                PurchaseOrders.gvColTotalAmountINR = Convert.ToString(Quotationdt.Rows[i]["TotalAmount"]);
                if (!string.IsNullOrEmpty(Convert.ToString(Quotationdt.Rows[i]["Indent_No"])))
                { PurchaseOrders.Indent = Convert.ToInt64(Quotationdt.Rows[i]["Indent_No"]); }
                else
                { PurchaseOrders.Indent = 0; }
                if (dtC.Contains("Indent_Num"))
                {
                    PurchaseOrders.Indent_Num = Convert.ToString(Quotationdt.Rows[i]["Indent_Num"]);
                }
                else
                {
                    PurchaseOrders.Indent_Num = "";
                }
                PurchaseOrders.ProductName = Convert.ToString(Quotationdt.Rows[i]["ProductName"]);
                PurchaseOrders.IsComponentProduct = Convert.ToString(Quotationdt.Rows[i]["IsComponentProduct"]);
                PurchaseOrders.IsLinkedProduct = Convert.ToString(Quotationdt.Rows[i]["IsLinkedProduct"]);
                PurchaseOrders.PurchaseOrder_InlineRemarks = Convert.ToString(Quotationdt.Rows[i]["PurchaseOrder_InlineRemarks"]);
                if (dtC.Contains("BalQty"))
                {
                    PurchaseOrders.BalQty = Convert.ToString(Quotationdt.Rows[i]["BalQty"]);
                }
                else
                {
                    PurchaseOrders.BalQty = "0.0000";
                }
                PurchaseOrders.Document_DetailsID = Convert.ToString(Quotationdt.Rows[i]["Document_DetailsID"]).Trim();
                PurchaseOrderList.Add(PurchaseOrders);
            }

            return PurchaseOrderList;
        }
        public int AddModifyPurchaseOrder(string PurchaseOrder_Id, string UniquePurchaseNumber, string strPurchaseDate, string IndentRequisitionNo, string IndentRequisitionDate,

        string strVendor, string strContactName, string TagDocType, DataTable addrDesc,
        string Reference, string strBranch, string strAgents, string strCurrency, string strRate, string strTaxOption,
        string strTaxCode, string CompanyID, int BaseCurrencyId, DataTable PurchaseOrderdt,
        string ActionType, string IsInventory, DataTable Warehousedt, DataTable TaxDetaildt, DataTable PurchaseOrderTaxdt, DataTable tempBillAddress,
        string approveStatus, string strCreditDays, string strPoDate, string PosForGst, DataTable QuotationPackingDetailsdt, Int64 ProjId, string AppRejRemarks, string projectValidFrom, string projectValidUpto, string RevisionNo, string RevisionDate, int ApproveRejectstatus
            , int firstApprove)
        {
            try
            {
                if (PurchaseOrderdt.Rows.Count > 0)  // Cross Branch Section by Sam on 10052017 Start only Condition to prevent Twice Firing
                {


                    foreach (DataRow row in PurchaseOrderdt.Rows)
                    {
                        DataColumnCollection dtC = PurchaseOrderdt.Columns;

                        if (dtC.Contains("Indent_Num"))
                        { PurchaseOrderdt.Columns.Remove("Indent_Num"); }

                        if (dtC.Contains("BalQty"))
                        { PurchaseOrderdt.Columns.Remove("BalQty"); }

                        break;
                    }





                    DataSet dsInst = new DataSet();
                    // SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                    SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

                    SqlCommand cmd = new SqlCommand("prc_ProjectPurchaseOrder", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (hdnApprovalReqInq.Value == "0")
                    {
                        ApproveRejectstatus = 1;
                    }

                    if ((ApproveRejectstatus == 1) && (hdnProjectApproval.Value == "") && (hdnApprovalReqInq.Value == "1") && (hdnRevisionRequiredEveryAfterApproval.Value=="No"))
                    {
                        cmd.Parameters.AddWithValue("@Action", "Add");
                        cmd.Parameters.AddWithValue("@PurchaseOrder_Number", txtVoucherNo.Text);
                    }
                    else if ((hdnProjectApproval.Value == "ProjApprove") && (hdnisFirstApprove.Value == "Yes") && (hdnRevisionRequiredEveryAfterApproval.Value == "Yes"))
                    {
                        cmd.Parameters.AddWithValue("@Action", "Add");
                        cmd.Parameters.AddWithValue("@PurchaseOrder_Number", txtVoucherNo.Text);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Action", ActionType);
                        cmd.Parameters.AddWithValue("@PurchaseOrder_Number", UniquePurchaseNumber);
                    }
                    cmd.Parameters.AddWithValue("@IsInventory", IsInventory);
                    cmd.Parameters.AddWithValue("@PurchaseOrderEdit_Id", PurchaseOrder_Id);
                    cmd.Parameters.AddWithValue("@approveStatus", approveStatus);
                    cmd.Parameters.AddWithValue("@PurchaseOrder_CompanyID", Convert.ToString(Session["LastCompany"]));
                    cmd.Parameters.AddWithValue("@PurchaseOrder_BranchId", strBranch);
                    if (Request.QueryString["op"] == "yes")
                    {
                        cmd.Parameters.AddWithValue("@PurchaseOrder_FinYear", "");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@PurchaseOrder_FinYear", Convert.ToString(Session["LastFinYear"]));
                    }
                   
                    cmd.Parameters.AddWithValue("@PurchaseOrder_IndentIds", IndentRequisitionNo);
                    if (!String.IsNullOrEmpty(IndentRequisitionDate))
                    { cmd.Parameters.AddWithValue("@PurchaseOrder_IndentDate", IndentRequisitionDate); }
                    cmd.Parameters.AddWithValue("@PurchaseOrder_Date", strPurchaseDate);
                    cmd.Parameters.AddWithValue("@PurchaseOrder_VendorId", strVendor);
                    cmd.Parameters.AddWithValue("@Contact_Person_Id", strContactName);
                    cmd.Parameters.AddWithValue("@TagDocType", TagDocType);
                    cmd.Parameters.AddWithValue("@PurchaseOrder_Reference", Reference);
                    cmd.Parameters.AddWithValue("@PurchaseOrder_Currency_Id", Convert.ToInt32(strCurrency));
                    cmd.Parameters.AddWithValue("@Currency_Conversion_Rate", strRate);
                    cmd.Parameters.AddWithValue("@Tax_Option", strTaxOption);
                    //cmd.Parameters.AddWithValue("@Tax_Code", strTaxCode);
                    cmd.Parameters.AddWithValue("@Tax_Code", 0);
                    cmd.Parameters.AddWithValue("@PurchaseOrder_SalesmanId", strAgents);
                    cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(Session["userid"]));
                    cmd.Parameters.AddWithValue("@PurchaseOrderDetails", PurchaseOrderdt);
                    cmd.Parameters.AddWithValue("@TaxDetail", TaxDetaildt);
                    cmd.Parameters.AddWithValue("@AddDescDetails", addrDesc);
                    cmd.Parameters.AddWithValue("@PurchaseOrderTax", PurchaseOrderTaxdt);
                    cmd.Parameters.AddWithValue("@PurchaseOrderAddress", tempBillAddress);
                    cmd.Parameters.AddWithValue("@CreditDays", strCreditDays);
                    cmd.Parameters.AddWithValue("@PoDate", strPoDate);
                    cmd.Parameters.AddWithValue("@PosForGst", PosForGst);
                    cmd.Parameters.AddWithValue("@Project_Id", ProjId);

                    if (projectValidFrom != "1/1/0001 12:00:00 AM")
                    {
                        if (!String.IsNullOrEmpty(projectValidFrom))
                            //cmd.Parameters.AddWithValue("@ProjectValidfromDate", Convert.ToDateTime(projectValidFrom));
                            cmd.Parameters.AddWithValue("@ProjectValidfromDate", DateTime.ParseExact(projectValidFrom, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"));
                    }
                    if (projectValidUpto != "1/1/0001 12:00:00 AM")
                    {
                        if (!String.IsNullOrEmpty(projectValidUpto))
                            //cmd.Parameters.AddWithValue("@ProjectValidUptoDate", Convert.ToDateTime(projectValidUpto));
                            cmd.Parameters.AddWithValue("@ProjectValidUptoDate", DateTime.ParseExact(projectValidUpto, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"));
                    }

                    if (RevisionDate != "1/1/0001 12:00:00 AM")
                    {
                        if (!String.IsNullOrEmpty(RevisionDate))
                            //cmd.Parameters.AddWithValue("@ProjectValidfromDate", Convert.ToDateTime(projectValidFrom));
                            cmd.Parameters.AddWithValue("@RevisionDate", DateTime.ParseExact(RevisionDate, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"));
                    }

                    cmd.Parameters.AddWithValue("@RevisionNo", RevisionNo);
                    cmd.Parameters.AddWithValue("@ProjRemarks", AppRejRemarks);
                    cmd.Parameters.AddWithValue("@ApproveRejectStatus", ApproveRejectstatus);

                    cmd.Parameters.AddWithValue("@QuotationPackingDetails", QuotationPackingDetailsdt); //Surojit 26-02-2019

                    cmd.Parameters.AddWithValue("@IsFirstApprove", firstApprove);
                    cmd.Parameters.AddWithValue("@IsfromApproval", hdnIsfromApproval.Value);

                    if (Session["POwarehousedetailstemp"] != null)
                    {
                        DataTable temtable = new DataTable();
                        DataTable Warehousedtssss = (DataTable)Session["POwarehousedetailstemp"];

                        temtable = Warehousedtssss.DefaultView.ToTable(false, "SrlNo", "BatchWarehouseID", "BatchWarehousedetailsID", "BatchID", "SerialID", "WarehouseID", "WarehouseName", "BatchNo", "SerialNo", "MFGDate", "ExpiryDate", "Quantitysum", "productid", "Inventrytype", "StockID", "isnew");
                        cmd.Parameters.AddWithValue("@udt_StockOpeningwarehousentrie", temtable);
                    }

                    SqlParameter output = new SqlParameter("@ReturnValue", SqlDbType.Int);
                    output.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(output);



                    cmd.CommandTimeout = 0;
                    SqlDataAdapter Adap = new SqlDataAdapter();
                    Adap.SelectCommand = cmd;
                    Adap.Fill(dsInst);

                    int idFromString = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value.ToString());

                    Session["Insert"] = "Y";
                    cmd.Dispose();
                    con.Dispose();

                    grid.JSProperties["cpPurchaseOrderID"] = Convert.ToString(output.Value);
                    //Udf Add mode
                    if(idFromString>0)
                    {
                    DataTable udfTable = (DataTable)Session["UdfDataOnAdd"];
                    if (udfTable != null)
                    {
                        Session["UdfDataOnAdd"] = reCat.insertRemarksCategoryAddMode("PO", "PurchaseOrder" + Convert.ToString(output.Value), udfTable, Convert.ToString(Session["userid"]));
                    }

                    if (!string.IsNullOrEmpty(hfControlData.Value))
                    {
                        CommonBL objCommonBL = new CommonBL();
                        objCommonBL.InsertTransporterControlDetails(Convert.ToInt32(output.Value), "PO".ToUpper(), hfControlData.Value, Convert.ToString(HttpContext.Current.Session["userid"]));
                    }
                    
                        if (hdnProjectSOTC.Value == "1")
                        {
                            ProjectTermsConditionsControl.Save_ProjectTerms(Convert.ToString(output.Value), "ProjPO");
                        }

                        if (!string.IsNullOrEmpty(hfTermsConditionData.Value))
                        {
                            TermsConditionsControl.SaveTC(hfTermsConditionData.Value, Convert.ToString(output.Value), "PO");
                        }
                    
                    if (chkmail.Checked)
                    {
                        exportToPDF.ExportToPdfforEmail("PO-Default~D", "Porder", Server.MapPath("~"), "", Convert.ToString(output.Value));

                        int retval = 0;
                        if (!string.IsNullOrEmpty(Convert.ToString(output.Value)))
                        {
                            retval = Sendmail_Purchaseorder(output.Value.ToString());
                        }
                    }
                    #region warehouse Update and delete

                    updatewarehouse();
                    deleteALL();
                }
                    #endregion

                    return idFromString;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public string checkNMakeJVCode(string manual_str, int sel_schema_Id)
        {

            //   oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

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

                    sqlQuery = "SELECT max(tjv.PurchaseOrder_Number) FROM tbl_trans_PurchaseOrder tjv WHERE dbo.RegexMatch('";
                    if (prefLen > 0)
                        sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                    else if (scheme_type == 2)
                        sqlQuery += "^";
                    sqlQuery += "[0-9]{" + paddCounter + "}";
                    if (sufxLen > 0)
                        sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                    //sqlQuery += "?$', LTRIM(RTRIM(tjv.PurchaseOrder_Number))) = 1";
                    sqlQuery += "?$', LTRIM(RTRIM(tjv.PurchaseOrder_Number))) = 1 and PurchaseOrder_Number like '" + prefCompCode + "%'";
                    if (scheme_type == 2)
                        sqlQuery += " AND CONVERT(DATE, CreatedDate) = CONVERT(DATE, GETDATE())";
                    dtC = oDBEngine.GetDataTable(sqlQuery);

                    if (dtC.Rows[0][0].ToString() == "")
                    {
                        sqlQuery = "SELECT max(tjv.PurchaseOrder_Number) FROM tbl_trans_PurchaseOrder tjv WHERE dbo.RegexMatch('";
                        if (prefLen > 0)
                            sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                        else if (scheme_type == 2)
                            sqlQuery += "^";
                        sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                        if (sufxLen > 0)
                            sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                        //sqlQuery += "?$', LTRIM(RTRIM(tjv.PurchaseOrder_Number))) = 1";
                        sqlQuery += "?$', LTRIM(RTRIM(tjv.PurchaseOrder_Number))) = 1 and PurchaseOrder_Number like '" + prefCompCode + "%'";
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
                            UniquePurchaseNumber = prefCompCode + paddedStr + sufxCompCode;
                            return "ok";
                        }
                    }
                    else
                    {
                        UniquePurchaseNumber = startNo.PadLeft(paddCounter, '0');
                        UniquePurchaseNumber = prefCompCode + paddedStr + sufxCompCode;
                        return "ok";
                    }
                }
                else
                {
                    sqlQuery = "SELECT PurchaseOrder_Number FROM tbl_trans_PurchaseOrder WHERE PurchaseOrder_Number LIKE '" + manual_str.Trim() + "'";
                    dtC = oDBEngine.GetDataTable(sqlQuery);
                    // duplicate manual entry check
                    if (dtC.Rows.Count > 0 && dtC.Rows[0][0].ToString().Trim() != "")
                    {
                        return "duplicate";
                    }

                    UniquePurchaseNumber = manual_str.Trim();
                    return "ok";
                }
            }
            else
            {
                return "noid";
            }
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

                bool isserialunique = false;
                if (hdnisserial.Value == "true")
                {
                    isserialunique = CheckUniqueSerial(SerialName, "new");
                }
                if (Convert.ToDecimal(openingstock) > totalopeining)
                {
                    GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("You have entered Quantity greater than Opening Quantity. Cannot Proceed.");

                }
                else if (isserialunique == false)
                {
                    DataTable Warehousedt = new DataTable();
                    DataTable Warehousedtnew = new DataTable();
                    if (Session["POCWarehouseData"] != null)
                    {
                        Warehousedt = (DataTable)Session["POCWarehouseData"];
                    }
                    else
                    {
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


                    if (Session["POCWarehouseData"] != null)
                    {
                        DataTable Warehousedts = (DataTable)Session["POCWarehouseData"];

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
                                if (Convert.ToString(dtmgh) != "1/1/0001 12:00:00 AM" && Convert.ToString(dtexp) != "1/1/0001 12:00:00 AM")
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

                                if (Convert.ToString(dtmgh) != "1/1/0001 12:00:00 AM" && Convert.ToString(dtexp) != "1/1/0001 12:00:00 AM")
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
                                if (Convert.ToString(dtmgh) != "1/1/0001 12:00:00 AM" && Convert.ToString(dtexp) != "1/1/0001 12:00:00 AM")
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
                                if (Convert.ToString(dtmgh) != "1/1/0001 12:00:00 AM" && Convert.ToString(dtexp) != "1/1/0001 12:00:00 AM")
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
                                if (Convert.ToString(dtmgh) != "1/1/0001 12:00:00 AM" && Convert.ToString(dtexp) != "1/1/0001 12:00:00 AM")
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


                            Session["POCWarehouseData"] = Warehousedt;


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
                string openingstock = Convert.ToString(hdfopeningstockPC.Value);

                if (Session["POCWarehouseData"] == null && hdnisolddeleted.Value == "false")
                {
                    if (!string.IsNullOrEmpty(hdnvalue.Value) && !string.IsNullOrEmpty(hdnrate.Value))
                    {

                    }
                    else
                    {
                        GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("Nothing to Saved .");
                    }

                }
                else if (Session["POCWarehouseData"] == null && openingstock != "0.0")
                {

                    //deleteALL();

                    GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("You can not save blank data.");
                }
                else
                {
                    string ProductID = Convert.ToString(hdfProductIDPC.Value);
                    string stockid = Convert.ToString(hdfstockidPC.Value);
                    openingstock = Convert.ToString(hdfopeningstockPC.Value);
                    string branchid = Convert.ToString(hdbranchIDPC.Value);
                    string isolddeletd = hdnisolddeleted.Value;

                    int oldrowcount = Convert.ToInt32(hdnoldrowcount.Value);
                    int newrowcount = 0;
                    int updaterows = 0;
                    int deleted = 0;
                    decimal hdntotalqntyPCs = Convert.ToDecimal(hdntotalqntyPC.Value);

                    DataTable Warehousedts = (DataTable)Session["POCWarehouseData"];

                    newrowcount = Warehousedts.Select("isnew = 'new'").Count<DataRow>();
                    updaterows = Warehousedts.Select("isnew = 'Updated'").Count<DataRow>();


                    if (newrowcount != 0 || updaterows != 0 || Session["POWarehouseDataDelete"] != null)
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
                                            Session["POCWarehouseData"] = Warehousedt;

                                            GrdWarehousePC.DataSource = Warehousedt.DefaultView;
                                            GrdWarehousePC.DataBind();
                                        }
                                        else
                                        {

                                            Session["POCWarehouseData"] = null;

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
                                            Session["POCWarehouseData"] = Warehousedt;

                                            GrdWarehousePC.DataSource = Warehousedt.DefaultView;
                                            GrdWarehousePC.DataBind();
                                        }
                                        else
                                        {

                                            Session["POCWarehouseData"] = null;

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
                                            Session["POCWarehouseData"] = Warehousedt;

                                            GrdWarehousePC.DataSource = Warehousedt.DefaultView;
                                            GrdWarehousePC.DataBind();
                                        }
                                        else
                                        {

                                            Session["POCWarehouseData"] = null;

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
                                            Session["POCWarehouseData"] = Warehousedt;

                                            GrdWarehousePC.DataSource = Warehousedt.DefaultView;
                                            GrdWarehousePC.DataBind();
                                        }
                                        else
                                        {

                                            Session["POCWarehouseData"] = null;

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
                                            Session["POCWarehouseData"] = Warehousedt;

                                            GrdWarehousePC.DataSource = Warehousedt.DefaultView;
                                            GrdWarehousePC.DataBind();
                                        }
                                        else
                                        {

                                            Session["POCWarehouseData"] = null;

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
                                            Session["POCWarehouseData"] = Warehousedt;

                                            GrdWarehousePC.DataSource = Warehousedt.DefaultView;
                                            GrdWarehousePC.DataBind();
                                        }
                                        else
                                        {

                                            Session["POCWarehouseData"] = null;

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
                Session["POCWarehouseData"] = null;

                GrdWarehousePC.DataSource = null;

                string strProductname = Convert.ToString(e.Parameters.Split('~')[4]);
                string name = strProductname;
                name = name.Replace("squot", "'");
                name = name.Replace("coma", ",");
                name = name.Replace("slash", "/");

                Session["WarehouseUpdatedData"] = null;
                Session["POWarehouseDataDelete"] = null;

                GrdWarehousePC.JSProperties["cpproductname"] = Convert.ToString(name);


                string strProductID = Convert.ToString(e.Parameters.Split('~')[1]);

                string stockids = Convert.ToString(e.Parameters.Split('~')[2]);

                string branchid = Convert.ToString(e.Parameters.Split('~')[3]);

                DataTable Warehousedt = new DataTable();
                Warehousedt = GetRecord(stockids);
                if (Warehousedt.Rows.Count > 0)
                {
                    Session["POCWarehouseData"] = Warehousedt;

                    GrdWarehousePC.DataSource = Warehousedt.DefaultView;
                    GrdWarehousePC.DataBind();
                }
                else
                {

                    Session["POCWarehouseData"] = null;

                    GrdWarehousePC.DataSource = null;
                    GrdWarehousePC.DataBind();

                }


            }
            //if (strSplitCommand == "UpdatedataReffresh")
            //{
            //    if (Session["POCWarehouseData"] != null)
            //    {

            //        GrdWarehousePC.DataSource = Session["POCWarehouseData"];
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

                bool isserialunique = false;
                if (hdnisserial.Value == "true")
                {
                    isserialunique = CheckUniqueSerial(SerialName, slno);
                }

                if (Session["POCWarehouseData"] != null && isserialunique == false)
                {
                    Warehousedt = (DataTable)Session["POCWarehouseData"];
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

                            if (Convert.ToString(expdate) != "1/1/0001 12:00:00 AM" && Convert.ToString(mkgdate) != "1/1/0001 12:00:00 AM")
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

                            if (Convert.ToString(expdate) != "1/1/0001 12:00:00 AM" && Convert.ToString(mkgdate) != "1/1/0001 12:00:00 AM")
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
                            if (Convert.ToString(expdate) != "1/1/0001 12:00:00 AM" && Convert.ToString(mkgdate) != "1/1/0001 12:00:00 AM")
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
                    Session["POCWarehouseData"] = Warehousedt;

                    Session["WarehouseUpdatedData"] = Warehousedt;

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
                bool isserialunique = false;
                if (hdnisserial.Value == "true")
                {
                    isserialunique = CheckUniqueSerial(SerialName, slno);
                }


                if (Session["POCWarehouseData"] != null && isserialunique == false)
                {
                    Warehousedt = (DataTable)Session["POCWarehouseData"];
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
                    Session["POCWarehouseData"] = Warehousedt;

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
                if (Session["POCWarehouseData"] != null)
                {
                    Warehousedt = (DataTable)Session["POCWarehouseData"];
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
                        if (Session["POWarehouseDataDelete"] != null)
                        {
                            setdeletedate = drs.CopyToDataTable();
                            DataTable dtmp = (DataTable)Session["POWarehouseDataDelete"];
                            dtmp.Merge(setdeletedate);
                            Session["POWarehouseDataDelete"] = dtmp;
                        }
                        else
                        {
                            setdeletedate = drs.CopyToDataTable();
                            Session["POWarehouseDataDelete"] = setdeletedate;
                        }


                    }


                    DataTable Warehousedts = dataSet1.Tables[0];
                    DataRow[] dr = Warehousedts.Select("isnew = 'new' OR isnew = 'Updated' OR isnew = 'old'");
                    if (dr.Count() > 0)
                    {

                        Warehousedt = dr.CopyToDataTable();
                        Warehousedt.DefaultView.Sort = "SrlNo asc";
                        Warehousedt = Warehousedt.DefaultView.ToTable(true);

                        Session["POCWarehouseData"] = Warehousedt;

                        GrdWarehousePC.DataSource = Session["POCWarehouseData"];
                        GrdWarehousePC.DataBind();
                    }
                    else
                    {
                        Session["POCWarehouseData"] = null;

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
        public bool CheckUniqueSerial(string serial, string slno)
        {
            bool ISexist = false;

            if (Session["POCWarehouseData"] != null && hdnisserial.Value == "true")
            {
                DataTable Warehousedts = (DataTable)Session["POCWarehouseData"];
                //
                DataRow[] dr = Warehousedts.Select(" productid = '" + Convert.ToString(hdfProductIDPC.Value) + "'and isnew = 'new' OR isnew = 'Updated' OR isnew = 'old'");

                if (dr.Count() > 0)
                {
                    DataTable dt = dr.CopyToDataTable();
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dt.Rows.Count - 1; i++)
                        {
                            if (slno == "new")
                            {
                                if (Convert.ToString(dt.Rows[i]["viewSerialNo"]).ToUpper() == serial.ToUpper())
                                {
                                    ISexist = true;
                                    break;
                                }
                            }
                            else
                            {
                                if (Convert.ToString(dt.Rows[i]["viewSerialNo"]).ToUpper() == serial.ToUpper() && Convert.ToString(dt.Rows[i]["SrlNo"]) != slno)
                                {
                                    ISexist = true;
                                    break;
                                }
                            }


                        }

                    }
                }


            }
            return ISexist;

        }
        public DataTable GetRecord(string stockids)
        {
            Session["POCWarehouseData"] = null;
            DataTable Warehousedt = new DataTable();

            if (Session["POCWarehouseData"] != null)
            {
                Warehousedt = (DataTable)Session["POCWarehouseData"];
            }
            else
            {
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
                    if (!string.IsNullOrEmpty(Convert.ToString(dts.Rows[i]["BatchWarehouseID"])) && Convert.ToString(dts.Rows[i]["BatchWarehouseID"]) != "0")
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
                            if (Convert.ToString(dts.Rows[i]["MfgDate"]) == "1/1/1900 12:00:00 AM")
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
                            if (Convert.ToString(dts.Rows[i]["MfgDate"]) == "1/1/1900 12:00:00 AM")
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
                            if (Convert.ToString(dts.Rows[i]["MfgDate"]) == "1/1/1900 12:00:00 AM")
                            {
                                Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToDecimal(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), "", Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["viewQuantity"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToString(dts.Rows[i]["viewQuantity"]), isoldornew, Convert.ToString(dts.Rows[i]["productid"]), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                            }
                            else if (string.IsNullOrEmpty(Convert.ToString(dts.Rows[i]["MfgDate"])))
                            {
                                Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToDecimal(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), "", Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["viewQuantity"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToString(dts.Rows[i]["viewQuantity"]), isoldornew, Convert.ToString(dts.Rows[i]["productid"]), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                            }
                            else
                            {

                                Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), Convert.ToDateTime(dts.Rows[i]["MfgDate"]), Convert.ToDateTime(dts.Rows[i]["ExpiryDate"]), Convert.ToDecimal(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), "", Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["viewQuantity"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToString(dts.Rows[i]["Quantitysum"]), isoldornew, Convert.ToString(dts.Rows[i]["productid"]), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                            }
                        }
                        else
                        {
                            if (Convert.ToString(dts.Rows[i]["MfgDate"]) == "1/1/1900 12:00:00 AM")
                            {
                                Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToDecimal(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["viewQuantity"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToString(dts.Rows[i]["Quantitysum"]), isoldornew, Convert.ToString(dts.Rows[i]["productid"]), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                            }
                            else if (string.IsNullOrEmpty(Convert.ToString(dts.Rows[i]["MfgDate"])))
                            {
                                Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToString(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["viewQuantity"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToString(dts.Rows[i]["Quantitysum"]), isoldornew, Convert.ToString(dts.Rows[i]["productid"]), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                            }
                            else
                            {

                                Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), Convert.ToDateTime(dts.Rows[i]["MfgDate"]), Convert.ToDateTime(dts.Rows[i]["ExpiryDate"]), Convert.ToDecimal(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["viewQuantity"]), Convert.ToString(dts.Rows[i]["serialno"]), Convert.ToDateTime(dts.Rows[i]["MfgDate"]), Convert.ToDateTime(dts.Rows[i]["ExpiryDate"]), Convert.ToString(dts.Rows[i]["Quantitysum"]), isoldornew, Convert.ToString(dts.Rows[i]["productid"]), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                            }
                        }

                    }
                    else
                    {

                        if (Convert.ToString(dts.Rows[i]["MfgDate"]) == "1/1/1900 12:00:00 AM")
                        {
                            Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToDecimal(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["viewQuantity"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToString(dts.Rows[i]["Quantitysum"]), isoldornew, Convert.ToString(hdfProductIDPC.Value), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                        }
                        else if (string.IsNullOrEmpty(Convert.ToString(dts.Rows[i]["MfgDate"])))
                        {
                            Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToDecimal(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["viewQuantity"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToString(dts.Rows[i]["Quantitysum"]), isoldornew, Convert.ToString(hdfProductIDPC.Value), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                        }
                        else
                        {

                            Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), Convert.ToDateTime(dts.Rows[i]["MfgDate"]), Convert.ToDateTime(dts.Rows[i]["ExpiryDate"]), Convert.ToDecimal(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["viewQuantity"]), Convert.ToString(dts.Rows[i]["serialno"]), Convert.ToDateTime(dts.Rows[i]["MfgDate"]), Convert.ToDateTime(dts.Rows[i]["ExpiryDate"]), Convert.ToString(dts.Rows[i]["Quantitysum"]), isoldornew, Convert.ToString(dts.Rows[i]["productid"]), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
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
            if (Session["POCWarehouseData"] != null)
            {
                DataTable Warehousedt = (DataTable)Session["POCWarehouseData"];

                temtable = Warehousedt.DefaultView.ToTable(false, "SrlNo", "BatchWarehouseID", "BatchWarehousedetailsID", "BatchID", "SerialID", "WarehouseID", "WarehouseName", "BatchNo", "SerialNo", "MFGDate", "ExpiryDate", "Quantitysum", "isnew", "viewQuantity", "productid", "Inventrytype", "StockID", "pcslno");

                if (Session["POwarehousedetailstemp"] != null)
                {
                    temtable23 = (DataTable)Session["POwarehousedetailstemp"];
                    DataRow[] rows;
                    rows = temtable23.Select("pcslno = '" + hdnpcslno.Value + "'");  //'UserName' is ColumnName
                    foreach (DataRow row in rows)
                        temtable23.Rows.Remove(row);

                    temtable23.Merge(temtable);
                    Session["POwarehousedetailstemp"] = temtable23;
                }
                else
                {
                    Session["POwarehousedetailstemp"] = temtable;
                }

            }


            #endregion
            #region Update
            if (Session["POCWarehouseData"] != null)
            {
                DataTable dt = new DataTable();
                DataTable Warehousedtups = (DataTable)Session["POCWarehouseData"];
                DataRow[] dr = Warehousedtups.Select("isnew = 'Updated'");
                if (dr.Count() != 0)
                {
                    dt = dr.CopyToDataTable();
                    if (Session["POwarehousedetailstempUpdate"] != null)
                    {
                        temtable23 = (DataTable)Session["POwarehousedetailstempUpdate"];
                        DataRow[] rows;
                        rows = temtable23.Select("pcslno = '" + hdnpcslno.Value + "'");  //'UserName' is ColumnName
                        foreach (DataRow row in rows)
                            temtable23.Rows.Remove(row);

                        temtable23.Merge(dt);
                        Session["POwarehousedetailstempUpdate"] = temtable23;
                    }
                    else
                    {
                        Session["POwarehousedetailstempUpdate"] = dt;
                    }


                }
            }

            #endregion
            #region delete

            if (Session["POWarehouseDataDelete"] != null)
            {
                DataTable dt = new DataTable();
                DataTable Warehousedtups = (DataTable)Session["POWarehouseDataDelete"];
                DataRow[] dr = Warehousedtups.Select("isnew = 'DeleteWHBT' OR isnew = 'DeleteWH' OR isnew = 'DeleteWHBT' OR isnew = 'DeleteBTSL' OR isnew = 'DeleteSL' OR isnew = 'DeleteWHSL' OR isnew = 'DeleteWHBTSL'");
                if (dr.Count() != 0)
                {
                    dt = dr.CopyToDataTable();
                    if (Session["POwarehousedetailstempDelete"] != null)
                    {
                        temtable23 = (DataTable)Session["POwarehousedetailstempDelete"];
                        DataRow[] rows;
                        rows = temtable23.Select("pcslno = '" + hdnpcslno.Value + "'");  //'UserName' is ColumnName
                        foreach (DataRow row in rows)
                            temtable23.Rows.Remove(row);

                        temtable23.Merge(dt);
                        Session["POwarehousedetailstempDelete"] = temtable23;
                    }
                    else
                    {
                        Session["POwarehousedetailstempDelete"] = dt;
                    }


                }

                //deleteALL(stockid);

            }
            #endregion

            return 1;


        }
        protected void GrdWarehousePC_DataBinding(object sender, EventArgs e)
        {
            if (Session["POCWarehouseData"] != null)
            {
                DataTable Warehousedt = (DataTable)Session["POCWarehouseData"];

                GrdWarehousePC.DataSource = Warehousedt;
            }
        }
        protected void CmbWarehouse_Callback(object sender, CallbackEventArgsBase e)
        {
            string WhichCall = e.Parameter.Split('~')[0];

            if (WhichCall == "BindWarehouse")
            {
                string strBranch = ddl_Branch.SelectedValue;
                DataTable dt = GetWarehouseData(strBranch);
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
        public DataTable GetWarehouseData(string strBranch)
        {
            DataTable dt = new DataTable();
            //dt = oDBEngine.GetDataTable("select  b.bui_id as WarehouseID,b.bui_Name as WarehouseName from tbl_master_building b order by b.bui_Name");

            //dt = oDBEngine.GetDataTable("select  bui_id as WarehouseID,bui_Name as WarehouseName from tbl_master_building Where IsNull(bui_BranchId,0) in ('0','" + strBranch + "') order by bui_Name");


            MasterSettings masterBl = new MasterSettings();
            string multiwarehouse = Convert.ToString(masterBl.GetSettings("IaMultilevelWarehouse"));

            if (multiwarehouse != "1")
            {
                dt = oDBEngine.GetDataTable("select  bui_id as WarehouseID,bui_Name as WarehouseName from tbl_master_building Where IsNull(bui_BranchId,0) in ('0','" + strBranch + "') order by bui_Name");
            }
            else
            {
                dt = oDBEngine.GetDataTable("EXEC [GET_BRANCHWISEWAREHOUSE] '1','" + strBranch + "'");
            }



            return dt;
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
            if (Convert.ToString(ViewState["ActionType"]) == "Edit")
            {
                DataSet dsInst = new DataSet();
                MasterSettings masterBl = new MasterSettings();
                string multiwarehouse = Convert.ToString(masterBl.GetSettings("IaMultilevelWarehouse"));

                //  SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

                SqlCommand cmd = new SqlCommand("prc_POGetwarehousentry", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ProductID", Convert.ToInt32(hdfProductIDPC.Value));
                cmd.Parameters.AddWithValue("@branchID", Convert.ToInt32(hdbranchIDPC.Value));
                cmd.Parameters.AddWithValue("@compnay", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@Finyear", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@PONumber", Convert.ToString(Session["PurchaseOrder_Id"]));
                cmd.Parameters.AddWithValue("@Multiwarehouse", Convert.ToString(multiwarehouse));
                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                con.Dispose();

                if (dsInst.Tables != null)
                {
                    if (Session["POwarehousedetailstemp"] != null)
                    {
                        DataTable Warehousedtss = (DataTable)Session["POwarehousedetailstemp"];

                        DataRow[] dr = Warehousedtss.Select("productid = '" + Convert.ToString(hdfProductIDPC.Value) + "' AND pcslno = '" + hdnpcslno.Value + "'");
                        if (dr.Count() > 0)
                        {

                            Warehousedtss = dr.CopyToDataTable();
                            Warehousedtss.DefaultView.Sort = "SrlNo asc";
                            Warehousedtss = Warehousedtss.DefaultView.ToTable(true);

                            Session["PCWarehouseDatabyslno"] = Warehousedtss;

                            dt2 = (DataTable)Session["PCWarehouseDatabyslno"];
                            DataTable dtmp = dsInst.Tables[0];

                            if (Session["POwarehousedetailstempUpdate"] != null)
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
            else if (Session["POwarehousedetailstemp"] != null)
            {
                DataTable Warehousedtss = (DataTable)Session["POwarehousedetailstemp"];

                DataRow[] dr = Warehousedtss.Select("productid = '" + Convert.ToString(hdfProductIDPC.Value) + "' AND pcslno = '" + hdnpcslno.Value + "'");
                if (dr.Count() > 0)
                {

                    Warehousedtss = dr.CopyToDataTable();
                    Warehousedtss.DefaultView.Sort = "SrlNo asc";
                    Warehousedtss = Warehousedtss.DefaultView.ToTable(true);

                    Session["PCWarehouseDatabyslno"] = Warehousedtss;

                    dt = (DataTable)Session["PCWarehouseDatabyslno"];

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

            if (Session["POwarehousedetailstempDelete"] != null)
            {
                string stockid = string.Empty;
                DataTable Warehousedtups = (DataTable)Session["POwarehousedetailstempDelete"];
                for (int i = 0; i <= Warehousedtups.Rows.Count - 1; i++)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(Warehousedtups.Rows[i]["BatchWarehouseID"])) && Convert.ToString(Warehousedtups.Rows[i]["BatchWarehouseID"]) != "0")
                    {
                        stockid = Convert.ToString(Warehousedtups.Rows[i]["StockID"]);

                        if (Convert.ToString(Warehousedtups.Rows[i]["isnew"]) == "DeleteSL")
                        {
                            string sqls = "delete Trans_StockSerialMapping where PurchaseOrder_MapId=" + Convert.ToString(Warehousedtups.Rows[i]["SerialID"]);

                            oDBEngine.GetDataTable(sqls);
                        }
                        else if (Convert.ToString(Warehousedtups.Rows[i]["isnew"]) == "DeleteWHBTSL")
                        {

                            string sqls = "delete from tbl_trans_PurchaseOrderWarehouseDetails where PurchaseOrderWarehouseDetail_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehousedetailsID"]) + " " +
                                          " delete from tbl_trans_PurchaseOrderWarehouse where PurchaseOrderWarehouse_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehouseID"]) + " " +
                                          " delete from tbl_trans_PurchaseOrderBatch where PurchaseOrderBatch_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehousedetailsID"]) + " " +
                                          " delete from tbl_trans_PurchaseOrderSerialMapping where PurchaseOrder_BatchId=" + Convert.ToString(Warehousedtups.Rows[i]["BatchID"]);


                            oDBEngine.GetDataTable(sqls);


                        }
                        else if (Convert.ToString(Warehousedtups.Rows[i]["isnew"]) == "DeleteWHSL")
                        {

                            string sqls = "delete from tbl_trans_PurchaseOrderWarehouseDetails where PurchaseOrderWarehouseDetail_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehousedetailsID"]) + " " +
                                          " delete from Trans_StockBranchWarehouse where PurchaseOrderWarehouse_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehouseID"]) + " " +
                                          " delete from tbl_trans_PurchaseOrderBatch where PurchaseOrderBatch_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehousedetailsID"]) + " " +
                                          " delete from tbl_trans_PurchaseOrderSerialMapping where PurchaseOrder_BatchId=" + Convert.ToString(Warehousedtups.Rows[i]["BatchID"]);


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

                                // String conn = ConfigurationSettings.AppSettings["DBConnectionDefault"];

                                String conn = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);


                                SqlConnection con = new SqlConnection(conn);
                                SqlCommand cmd3 = new SqlCommand("prc_POwarehousbatchDeleteWHBT", con);
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



                            string sqls = "delete from tbl_trans_PurchaseOrderWarehouseDetails where PurchaseOrderWarehouseDetail_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehousedetailsID"]) + " " +
                                          " delete from tbl_trans_PurchaseOrderWarehouse where PurchaseOrderWarehouse_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehouseID"]) + " " +
                                          " delete from tbl_trans_PurchaseOrderBatch where PurchaseOrderBatch_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehousedetailsID"]);

                            oDBEngine.GetDataTable(sqls);

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
            if (Session["POCWarehouseData"] != null)
            {
                DataTable Warehousedt = (DataTable)Session["POCWarehouseData"];

                temtable = Warehousedt.DefaultView.ToTable(false, "SrlNo", "BatchWarehouseID", "BatchWarehousedetailsID", "BatchID", "SerialID", "WarehouseID", "WarehouseName", "BatchNo", "SerialNo", "MFGDate", "ExpiryDate", "Quantitysum", "productid", "Inventrytype", "StockID", "isnew");
                cmd3.Parameters.AddWithValue("@udt_StockOpeningwarehousentried", temtable);
            }
            if (Session["POWarehouseDataDelete"] != null)
            {
                DataTable Warehousedts = (DataTable)Session["POWarehouseDataDelete"];

                temtable2 = Warehousedts.DefaultView.ToTable(false, "SrlNo", "BatchWarehouseID", "BatchWarehousedetailsID", "BatchID", "SerialID", "WarehouseID", "WarehouseName", "BatchNo", "SerialNo", "MFGDate", "ExpiryDate", "Quantity", "isnew");
                cmd3.Parameters.AddWithValue("@udt_StockOpeningwarehousentrieDelete", null);
            }

            #endregion

            return 1;


        }
        private int updatewarehouse()
        {
            if (Session["POwarehousedetailstempUpdate"] != null)
            {
                //DataTable dt = new DataTable();
                DataTable dtOLD = new DataTable();
                DataTable Warehousedtups = (DataTable)Session["POwarehousedetailstempUpdate"];
                DataRow[] dr = Warehousedtups.Select("isnew = 'Updated'");
                if (dr.Count() != 0)
                {
                    //dt = dr.CopyToDataTable();
                    dtOLD = dr.CopyToDataTable();
                    foreach (DataRow drNew in dtOLD.Rows)
                    {
                        string strBatchNo = Convert.ToString(drNew["BatchNo"]);
                        string strSerialNo = Convert.ToString(drNew["SerialNo"]);
                        string strStockID = Convert.ToString(drNew["StockID"]);
                        DataTable GetBatchSerial = objPurchaseOrderBL.GetSerialBatchID(strBatchNo, strSerialNo, strStockID);
                        if (GetBatchSerial.Rows.Count > 0)
                        {
                            string BatchNo = Convert.ToString(GetBatchSerial.Rows[0]["BatchNo"]);
                            string SerialNo = Convert.ToString(GetBatchSerial.Rows[0]["SerialNo"]);

                            drNew["BatchNo"] = BatchNo;
                            drNew["SerialNo"] = SerialNo;
                            drNew["viewBatchNo"] = BatchNo;
                            drNew["viewSerialNo"] = SerialNo;
                        }

                    }
                    dtOLD.AcceptChanges();
                    DataTable dt = dtOLD.Copy();

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
                                if ((Convert.ToString(dt.Rows[i]["viewMFGDate"]) != "1/1/0001 12:00:00 AM" && Convert.ToString(dt.Rows[i]["viewExpiryDate"]) != "1/1/0001 12:00:00 AM"))
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

                                        sql = "update tbl_trans_PurchaseOrderWarehouse set PurchaseOrderWarehouse_StockIn=" + Convert.ToDecimal(updateqnty) + ",PurchaseOrderWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where PurchaseOrderWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +

                                                             "   update tbl_trans_PurchaseOrderBatch set   PurchaseOrderBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                           "PurchaseOrderBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +


                                                           "PurchaseOrderBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                           "PurchaseOrderBatch_ModifiedDate=GETDATE() " +
                                                      "where PurchaseOrderBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]) + "	  " +
                                                      "  update  tbl_trans_PurchaseOrderSerialMapping" +
                                                      " Set PurchaseOrder_SerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
                                                      " where PurchaseOrder_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";
                                    }
                                    else
                                    {
                                        sql = "update tbl_trans_PurchaseOrderWarehouse set PurchaseOrderWarehouse_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + ",PurchaseOrderWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where PurchaseOrderWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +

                                                               "   update tbl_trans_PurchaseOrderBatch set   PurchaseOrderBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                             "PurchaseOrderBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +


                                                             "PurchaseOrderBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                             "PurchaseOrderBatch_ModifiedDate=GETDATE() " +
                                                        "where PurchaseOrderBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]) + "	  " +
                                                        "  update  tbl_trans_PurchaseOrderSerialMapping" +
                                                        " Set PurchaseOrder_SerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
                                                        " where PurchaseOrder_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";
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
                                        sql = "update tbl_trans_PurchaseOrderWarehouse set PurchaseOrderWarehouse_StockIn=" + Convert.ToDecimal(updateqnty) + ",PurchaseOrderWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where PurchaseOrderWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +

                                                                 "   update tbl_trans_PurchaseOrderBatch set   PurchaseOrderBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                               "PurchaseOrderBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +

                                                               "StockBranchBatch_MfgDate='" + mfgex + "'," +
                                                               "StockBranchBatch_ExpiryDate='" + exdate + "'," +
                                                               "PurchaseOrderBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                               "PurchaseOrderBatch_ModifiedDate=GETDATE() " +
                                                          "where PurchaseOrderBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]) + "	  " +
                                                          "  update  tbl_trans_PurchaseOrderSerialMapping" +
                                                          " Set PurchaseOrder_SerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
                                                          " where PurchaseOrder_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";
                                    }
                                    else
                                    {
                                        sql = "update tbl_trans_PurchaseOrderWarehouse set PurchaseOrderWarehouse_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + ",PurchaseOrderWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where PurchaseOrderWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +

                                                            "   update tbl_trans_PurchaseOrderBatch set   PurchaseOrderBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                          "PurchaseOrderBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +

                                                          "StockBranchBatch_MfgDate='" + mfgex + "'," +
                                                          "StockBranchBatch_ExpiryDate='" + exdate + "'," +
                                                          "PurchaseOrderBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                          "PurchaseOrderBatch_ModifiedDate=GETDATE() " +
                                                          "where PurchaseOrderBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]) + "	  " +
                                                          "  update  tbl_trans_PurchaseOrderSerialMapping" +
                                                          " Set PurchaseOrder_SerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
                                                          " where PurchaseOrder_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";
                                    }


                                }


                                oDBEngine.GetDataTable(sql);

                            }
                            else
                            {
                                //var updateqnty = dt.Compute("sum(Quantitysum)", "isnew = 'Updated' AND WarehouseID='" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + "' AND BatchNo='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'");


                                string sql = "update tbl_trans_PurchaseOrderWarehouse set PurchaseOrderWarehouse_StockIn=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where PurchaseOrderWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +


                                                  "  update  tbl_trans_PurchaseOrderSerialMapping" +
                                        " Set PurchaseOrder_SerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
                                        " where PurchaseOrder_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";

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
                                if ((Convert.ToString(dt.Rows[i]["viewMFGDate"]) != "1/1/0001 12:00:00 AM" && Convert.ToString(dt.Rows[i]["viewExpiryDate"]) != "1/1/0001 12:00:00 AM"))
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
                                    sql = "update tbl_trans_PurchaseOrderWarehouse set PurchaseOrderWarehouse_StockIn=" + Convert.ToDecimal(updateqnty) + ",PurchaseOrderWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where PurchaseOrderWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +

                                                         "   update tbl_trans_PurchaseOrderBatch set   PurchaseOrderBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                       "PurchaseOrderBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +


                                                       "PurchaseOrderBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                       "PurchaseOrderBatch_ModifiedDate=GETDATE() " +
                                                  "where PurchaseOrderBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]);


                                }
                                else
                                {
                                    sql = "update tbl_trans_PurchaseOrderWarehouse set PurchaseOrderWarehouse_StockIn=" + Convert.ToDecimal(updateqnty) + ",PurchaseOrderWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where PurchaseOrderWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +

                                                             "   update tbl_trans_PurchaseOrderBatch set   PurchaseOrderBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                           "PurchaseOrderBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +

                                                           "StockBranchBatch_MfgDate='" + mfgex + "'," +
                                                           "StockBranchBatch_ExpiryDate='" + exdate + "'," +
                                                           "PurchaseOrderBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                           "PurchaseOrderBatch_ModifiedDate=GETDATE() " +
                                                      "where PurchaseOrderBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]);
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
                                if ((Convert.ToString(dt.Rows[i]["viewMFGDate"]) != "1/1/0001 12:00:00 AM" && Convert.ToString(dt.Rows[i]["viewExpiryDate"]) != "1/1/0001 12:00:00 AM"))
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
                                    //sql = "update tbl_trans_PurchaseOrderWarehouse set PurchaseOrderWarehouse_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + ",PurchaseOrderWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where PurchaseOrderWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
                                    //          "  update tbl_trans_PurchaseOrderWarehouseDetails set Stock_OpeningRate=" + Convert.ToDecimal(hdnrate.Value) + "," +
                                    //                     "Stock_OpeningValue=ISNULL(isnull(" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + ",0)*isnull(" + Convert.ToDecimal(hdnrate.Value) + ",0),0)," +
                                    //                     "ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                    //                     "ModifiedDate=GETDATE()" +
                                    //                     "where PurchaseOrderWarehouseDetail_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehousedetailsID"]) + "" +
                                    //                     "   update tbl_trans_PurchaseOrderBatch set   PurchaseOrderBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                    //                   "PurchaseOrderBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +


                                    //                   "PurchaseOrderBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                    //                   "PurchaseOrderBatch_ModifiedDate=GETDATE()" +
                                    //              "where PurchaseOrderBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]) + "	  " +
                                    //              "  update  tbl_trans_PurchaseOrderSerialMapping" +
                                    //    " Set PurchaseOrder_SerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
                                    //    " where PurchaseOrder_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";

                                    sql = "update tbl_trans_PurchaseOrderWarehouse set PurchaseOrderWarehouse_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + ",PurchaseOrderWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where PurchaseOrderWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
                                             "  update tbl_trans_PurchaseOrderWarehouseDetails set " +
                                                        " ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                        "ModifiedDate=GETDATE()" +
                                                        "where PurchaseOrderWarehouseDetail_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehousedetailsID"]) + "" +
                                                        "   update tbl_trans_PurchaseOrderBatch set   PurchaseOrderBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                      "PurchaseOrderBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +


                                                      "PurchaseOrderBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                      "PurchaseOrderBatch_ModifiedDate=GETDATE()" +
                                                 "where PurchaseOrderBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]) + "	  " +
                                                 "  update  tbl_trans_PurchaseOrderSerialMapping" +
                                       " Set PurchaseOrder_SerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
                                       " where PurchaseOrder_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";

                                }
                                else
                                {
                                    sql = "update tbl_trans_PurchaseOrderWarehouse set PurchaseOrderWarehouse_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + ",PurchaseOrderWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where PurchaseOrderWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
                                                  "  update tbl_trans_PurchaseOrderWarehouseDetails set " +//Stock_OpeningRate=" + Convert.ToDecimal(hdnrate.Value) + "," +
                                        // "Stock_OpeningValue=ISNULL(isnull(" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + ",0)*isnull(" + Convert.ToDecimal(hdnrate.Value) + ",0),0)," +
                                                             " ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                             "ModifiedDate=GETDATE()" +
                                                             "where StockBranchWarehouseDetail_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehousedetailsID"]) + "" +
                                                             "   update tbl_trans_PurchaseOrderBatch set   PurchaseOrderBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                           "PurchaseOrderBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +

                                                           "StockBranchBatch_MfgDate='" + mfgex + "'," +
                                                           "StockBranchBatch_ExpiryDate='" + exdate + "'," +
                                                           "PurchaseOrderBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                           "PurchaseOrderBatch_ModifiedDate=GETDATE()" +
                                                      "where PurchaseOrderBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]) + "	  " +
                                                      "  update  tbl_trans_PurchaseOrderSerialMapping" +
                                            " Set PurchaseOrder_SerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
                                            " where PurchaseOrder_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";
                                }


                                oDBEngine.GetDataTable(sql);

                            }
                            else
                            {
                                //var updateqnty = dt.Compute("sum(Quantitysum)", "isnew = 'Updated' AND WarehouseID='" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + "' AND BatchNo='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'");


                                //string sql = "update tbl_trans_PurchaseOrderWarehouse set StockBranchWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where PurchaseOrderWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
                                //              "  update tbl_trans_PurchaseOrderWarehouseDetails set " +
                                //                         "ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                //                         "ModifiedDate=GETDATE()" +
                                //                         "where StockBranchWarehouseDetail_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehousedetailsID"]) + "" +

                                //                  "  update  tbl_trans_PurchaseOrderSerialMapping" +
                                //        " Set PurchaseOrder_SerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
                                //        " where PurchaseOrder_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";
                                string sql = "update tbl_trans_PurchaseOrderWarehouse set PurchaseOrderWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where PurchaseOrderWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
                                              "  update tbl_trans_PurchaseOrderWarehouseDetails set " +
                                                         "ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                         "ModifiedDate=GETDATE()" +
                                                         "where PurchaseOrderWarehouseDetail_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehousedetailsID"]) + "" +

                                                  "  update  tbl_trans_PurchaseOrderSerialMapping" +
                                        " Set PurchaseOrder_SerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
                                        " where PurchaseOrder_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";

                                oDBEngine.GetDataTable(sql);
                            }
                        }
                        #endregion

                        #region WH
                        if (Convert.ToString(dt.Rows[i]["Inventrytype"]) == "WH")
                        {
                            if (Convert.ToString(dt.Rows[i]["viewQuantity"]) != "0" && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Quantity"])))
                            {

                                string sql = "update tbl_trans_PurchaseOrderWarehouse set PurchaseOrderWarehouse_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantity"]) + ",PurchaseOrderWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where PurchaseOrderWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +

                                                       "   update tbl_trans_PurchaseOrderBatch set   PurchaseOrderBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                     "PurchaseOrderBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantity"]) + "," +


                                                     "PurchaseOrderBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                     "PurchaseOrderBatch_ModifiedDate=GETDATE() " +
                                                " where PurchaseOrderBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]);




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
                                if ((Convert.ToString(dt.Rows[i]["viewMFGDate"]) != "1/1/0001 12:00:00 AM" && Convert.ToString(dt.Rows[i]["viewExpiryDate"]) != "1/1/0001 12:00:00 AM"))
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

                                        sql = "update tbl_trans_PurchaseOrderWarehouse set PurchaseOrderWarehouse_StockIn=" + Convert.ToDecimal(updateqnty) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where PurchaseOrderWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +

                                                             "   update tbl_trans_PurchaseOrderBatch set   PurchaseOrderBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                           "PurchaseOrderBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +


                                                           "PurchaseOrderBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                           "PurchaseOrderBatch_ModifiedDate=GETDATE() " +
                                                      "where PurchaseOrderBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]) + "	  " +
                                                      "  update  tbl_trans_PurchaseOrderSerialMapping" +
                                            " Set PurchaseOrder_SerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
                                            " where PurchaseOrder_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";
                                    }
                                    else
                                    {
                                        sql = "update tbl_trans_PurchaseOrderWarehouse set PurchaseOrderWarehouse_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where PurchaseOrderWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +

                                                               "   update tbl_trans_PurchaseOrderBatch set   PurchaseOrderBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                             "PurchaseOrderBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +


                                                             "PurchaseOrderBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                             "PurchaseOrderBatch_ModifiedDate=GETDATE() " +
                                                        "where PurchaseOrderBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]) + "	  " +
                                                        "  update  tbl_trans_PurchaseOrderSerialMapping" +
                                              " Set PurchaseOrder_SerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
                                              " where PurchaseOrder_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";
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
                                        sql = "update tbl_trans_PurchaseOrderWarehouse set PurchaseOrderWarehouse_StockIn=" + Convert.ToDecimal(updateqnty) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where PurchaseOrderWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +

                                                                 "   update tbl_trans_PurchaseOrderBatch set   PurchaseOrderBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                               "PurchaseOrderBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +

                                                               "StockBranchBatch_MfgDate='" + mfgex + "'," +
                                                               "StockBranchBatch_ExpiryDate='" + exdate + "'," +
                                                               "PurchaseOrderBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                               "PurchaseOrderBatch_ModifiedDate=GETDATE() " +
                                                          "where PurchaseOrderBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]) + "	  " +
                                                          "  update  tbl_trans_PurchaseOrderSerialMapping" +
                                                " Set PurchaseOrder_SerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
                                                " where PurchaseOrder_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";
                                    }
                                    else
                                    {
                                        sql = "update tbl_trans_PurchaseOrderWarehouse set PurchaseOrderWarehouse_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where PurchaseOrderWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +

                                                            "   update tbl_trans_PurchaseOrderBatch set   PurchaseOrderBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                          "PurchaseOrderBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +

                                                          "StockBranchBatch_MfgDate='" + mfgex + "'," +
                                                          "StockBranchBatch_ExpiryDate='" + exdate + "'," +
                                                          "PurchaseOrderBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                          "PurchaseOrderBatch_ModifiedDate=GETDATE() " +
                                                     "where PurchaseOrderBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]) + "	  " +
                                                     "  update  tbl_trans_PurchaseOrderSerialMapping" +
                                           " Set PurchaseOrder_SerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
                                           " where PurchaseOrder_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";
                                    }


                                }


                                oDBEngine.GetDataTable(sql);

                            }
                            else
                            {
                                //var updateqnty = dt.Compute("sum(Quantitysum)", "isnew = 'Updated' AND WarehouseID='" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + "' AND BatchNo='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'");


                                string sql = "update tbl_trans_PurchaseOrderWarehouse set PurchaseOrderWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where PurchaseOrderWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
                                              "  update tbl_trans_PurchaseOrderWarehouseDetails set " +
                                                         "ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                         "ModifiedDate=GETDATE() " +
                                                         "where PurchaseOrderWarehouseDetail_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehousedetailsID"]) + "" +

                                                  "  update  tbl_trans_PurchaseOrderSerialMapping" +
                                        " Set PurchaseOrder_SerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
                                        " where PurchaseOrder_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";

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
                                if ((Convert.ToString(dt.Rows[i]["viewMFGDate"]) != "1/1/0001 12:00:00 AM" && Convert.ToString(dt.Rows[i]["viewExpiryDate"]) != "1/1/0001 12:00:00 AM"))
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
                                    sql = "update tbl_trans_PurchaseOrderWarehouse set PurchaseOrderWarehouse_StockIn=" + Convert.ToDecimal(updateqnty) + ",PurchaseOrderWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where PurchaseOrderWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +

                                                         "   update tbl_trans_PurchaseOrderBatch set   PurchaseOrderBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                       "PurchaseOrderBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +


                                                       "PurchaseOrderBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                       "PurchaseOrderBatch_ModifiedDate=GETDATE() " +
                                                  "where PurchaseOrderBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]);


                                }
                                else
                                {
                                    sql = "update tbl_trans_PurchaseOrderWarehouse set PurchaseOrderWarehouse_StockIn=" + Convert.ToDecimal(updateqnty) + ",PurchaseOrderWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where PurchaseOrderWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +

                                                             "   update tbl_trans_PurchaseOrderBatch set   PurchaseOrderBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                           "PurchaseOrderBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +

                                                           "StockBranchBatch_MfgDate='" + mfgex + "'," +
                                                           "StockBranchBatch_ExpiryDate='" + exdate + "'," +
                                                           "PurchaseOrderBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                           "PurchaseOrderBatch_ModifiedDate=GETDATE() " +
                                                      "where PurchaseOrderBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]);
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

        #endregion

        #region Taxpopup
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
            ProcedureExecute proc = new ProcedureExecute("prc__Project_PurchaseOrderDetailsList");
            proc.AddVarcharPara("@Action", 500, "TaxDetailsForGst");
            proc.AddIntegerPara("@PurchaseOrder_Id", Convert.ToInt32(Session["PurchaseOrder_Id"]));
            dt = proc.GetTable();
            if (dt.Rows.Count > 0)
            {
                DataRow gstRow = existing.NewRow();
                gstRow["Taxes_ID"] = 0;
                gstRow["Taxes_Name"] = dt.Rows[0]["TaxRatesSchemeName"];
                gstRow["Percentage"] = dt.Rows[0]["OrderTax_Percentage"];
                gstRow["Amount"] = dt.Rows[0]["OrderTax_Amount"];
                gstRow["AltTax_Code"] = dt.Rows[0]["Gst"];

                UpdateGstForCharges(Convert.ToString(dt.Rows[0]["Gst"]));
                // txtGstCstVatCharge.Value = gstRow["Amount"];
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
            //for (int i = 0; i < cmbGstCstVatcharge.Items.Count; i++)
            //{
            //    if (Convert.ToString(cmbGstCstVatcharge.Items[i].Value).Split('~')[0] == data)
            //    {
            //        cmbGstCstVatcharge.Items[i].Selected = true;
            //        break;
            //    }
            //}
        }
        protected void taxUpdatePanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {

            string performpara = e.Parameter;
            if (performpara.Split('~')[0] == "DelProdbySl")
            {
                DataTable MainTaxDataTable = (DataTable)Session["PurchaseOrderFinalTaxRecord"];

                DataRow[] deletedRow = MainTaxDataTable.Select("SlNo=" + performpara.Split('~')[1]);
                if (deletedRow.Length > 0)
                {
                    foreach (DataRow dr in deletedRow)
                    {
                        MainTaxDataTable.Rows.Remove(dr);
                    }

                }

                Session["PurchaseOrderFinalTaxRecord"] = MainTaxDataTable;
                GetStock(Convert.ToString(performpara.Split('~')[2]));
                // DeleteWarehouse(Convert.ToString(performpara.Split('~')[1]));
                DataTable taxDetails = (DataTable)Session["POTaxDetails"];
                if (taxDetails != null)
                {
                    foreach (DataRow dr in taxDetails.Rows)
                    {
                        dr["Amount"] = "0.00";
                    }
                    Session["POTaxDetails"] = taxDetails;
                }
            }
            else if (performpara.Split('~')[0] == "DeleteAllTax")
            {
                CreateDataTaxTable();

                DataTable taxDetails = (DataTable)Session["POTaxDetails"];

                if (taxDetails != null)
                {
                    foreach (DataRow dr in taxDetails.Rows)
                    {
                        dr["Amount"] = "0.00";
                    }
                    Session["POTaxDetails"] = taxDetails;
                }
            }
            else
            {
                DataTable MainTaxDataTable = (DataTable)Session["PurchaseOrderFinalTaxRecord"];

                DataRow[] deletedRow = MainTaxDataTable.Select("SlNo=" + performpara.Split('~')[1]);
                if (deletedRow.Length > 0)
                {
                    foreach (DataRow dr in deletedRow)
                    {
                        MainTaxDataTable.Rows.Remove(dr);
                    }

                }

                Session["PurchaseOrderFinalTaxRecord"] = MainTaxDataTable;

                DataTable taxDetails = (DataTable)Session["POTaxDetails"];

                if (taxDetails != null)
                {
                    foreach (DataRow dr in taxDetails.Rows)
                    {
                        dr["Amount"] = "0.00";
                    }
                    Session["POTaxDetails"] = taxDetails;
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
        public void DeleteWarehouse(string SrlNo)
        {
            DataTable Warehousedt = new DataTable();
            if (Session["WarehouseData"] != null)
            {
                Warehousedt = (DataTable)Session["WarehouseData"];

                var rows = Warehousedt.Select(string.Format("Product_SrlNo ='{0}'", SrlNo));
                foreach (var row in rows)
                {
                    row.Delete();
                }
                Warehousedt.AcceptChanges();

                Session["WarehouseData"] = Warehousedt;
            }
        }
        public double ReCalculateTaxAmount(string slno, double amount)
        {
            DataTable MainTaxDataTable = (DataTable)Session["PurchaseOrderFinalTaxRecord"];
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
            Session["PurchaseOrderFinalTaxRecord"] = MainTaxDataTable;

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
            Session["PurchaseOrderFinalTaxRecord"] = TaxRecord;
        }
        public static void CreateDataTaxTableByAjax()
        {
            DataTable TaxRecord = new DataTable();

            TaxRecord.Columns.Add("SlNo", typeof(System.Int32));
            TaxRecord.Columns.Add("TaxCode", typeof(System.String));
            TaxRecord.Columns.Add("AltTaxCode", typeof(System.String));
            TaxRecord.Columns.Add("Percentage", typeof(System.Decimal));
            TaxRecord.Columns.Add("Amount", typeof(System.Decimal));
            HttpContext.Current.Session["PurchaseOrderFinalTaxRecord"] = TaxRecord;
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
            proc.AddVarcharPara("@QuotationID", 500, Convert.ToString(Session["QuotationID"]));
            ds = proc.GetDataSet();
            return ds;
        }
        public DataSet GetQuotationEditedTaxData()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc__Project_PurchaseOrderDetailsList");
            proc.AddVarcharPara("@Action", 500, "ProductEditedTaxDetailsForPo");
            proc.AddIntegerPara("@PurchaseOrder_Id", Convert.ToInt32(Session["PurchaseOrder_Id"]));
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
                DataTable TaxRecord = (DataTable)Session["PurchaseOrderFinalTaxRecord"];
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

                Session["PurchaseOrderFinalTaxRecord"] = TaxRecord;
            }
            else
            {
                #region fetch All data For Tax

                DataTable taxDetail = new DataTable();
                DataTable MainTaxDataTable = (DataTable)Session["PurchaseOrderFinalTaxRecord"];

                if (Convert.ToString(ddl_AmountAre.Value).Trim() != "4")
                {
                    //ProcedureExecute proc = new ProcedureExecute("prc__Project_PurchaseOrderDetailsList");
                    //proc.AddVarcharPara("@Action", 500, "LoadOtherTaxDetails");
                    //proc.AddVarcharPara("@ProductsID", 10, Convert.ToString(setCurrentProdCode.Value));
                    //proc.AddVarcharPara("@S_quoteDate", 10, dt_PLQuote.Date.ToString("yyyy-MM-dd"));
                    //taxDetail = proc.GetTable();

                    ProcedureExecute proc = new ProcedureExecute("prc_TaxExceptionFind");
                    proc.AddVarcharPara("@Action", 500, "PO");
                    proc.AddVarcharPara("@ProductID", 10, Convert.ToString(setCurrentProdCode.Value));
                    proc.AddVarcharPara("@ENTITY_ID", 100, hdnCustomerId.Value);
                    proc.AddVarcharPara("@Date", 10, dt_PLQuote.Date.ToString("yyyy-MM-dd"));
                    proc.AddVarcharPara("@Amount", 100, HdProdGrossAmt.Value);
                    proc.AddVarcharPara("@Qty", 100, hdnQty.Value);
                    taxDetail = proc.GetTable();
                }
                else
                {
                    ProcedureExecute proc = new ProcedureExecute("prc__Project_PurchaseOrderDetailsList");
                    proc.AddVarcharPara("@Action", 500, "LoadImportTaxDetails");
                    proc.AddVarcharPara("@S_quoteDate", 10, dt_PLQuote.Date.ToString("yyyy-MM-dd"));
                    taxDetail = proc.GetTable();
                }

                //Get Company Gstin 09032017
                string CompInternalId = Convert.ToString(Session["LastCompany"]);
                string[] compGstin = oDBEngine.GetFieldValue1("tbl_master_company", "cmp_gstin", "cmp_internalid='" + CompInternalId + "'", 1);

                //Get BranchStateCode
                string BrancgStateCode = "", BranchGSTIN = "";
                DataTable BranchTable = oDBEngine.GetDataTable("select StateCode,branch_GSTIN   from tbl_master_branch branch inner join tbl_master_state st on branch.branch_state=st.id where branch_id=" + Convert.ToString(ddl_Branch.SelectedValue));
                if (BranchTable != null)
                {
                    BrancgStateCode = Convert.ToString(BranchTable.Rows[0][0]);
                    BranchGSTIN = Convert.ToString(BranchTable.Rows[0][1]);
                    if (BranchGSTIN.Trim() != "")
                    {
                        BrancgStateCode = BranchGSTIN.Substring(0, 2);
                    }
                }
                if (BranchGSTIN.Trim() == "")
                {
                    BrancgStateCode = compGstin[0].Substring(0, 2);
                }
                string VendorState = "";

                ProcedureExecute GetVendorGstin = new ProcedureExecute("prc_GstTaxDetails");
                GetVendorGstin.AddVarcharPara("@Action", 500, "GetVendorGSTINByBranch");
                GetVendorGstin.AddVarcharPara("@branchId", 10, Convert.ToString(ddl_Branch.SelectedValue));
                GetVendorGstin.AddVarcharPara("@entityId", 10, Convert.ToString(hdnCustomerId.Value));
                DataTable VendorGstin = GetVendorGstin.GetTable();

                if (VendorGstin.Rows.Count > 0)
                {
                    if (Convert.ToString(VendorGstin.Rows[0][0]).Trim() != "")
                    {
                        VendorState = Convert.ToString(VendorGstin.Rows[0][0]).Substring(0, 2);
                    }

                }
                #endregion
                if (Convert.ToString(ddl_AmountAre.Value).Trim() != "4")
                {
                    if (VendorState.Trim() != "" && BrancgStateCode != "")
                    {
                        if (compGstin.Length > 0)
                        {
                            if (BrancgStateCode == VendorState)
                            {
                                //Check if the state is in union territories then only UTGST will apply
                                //   Chandigarh     Andaman and Nicobar Islands    DADRA & NAGAR HAVELI    DAMAN & DIU          Lakshadweep              PONDICHERRY
                                if (VendorState == "4" || VendorState == "26" || VendorState == "25" || VendorState == "7" || VendorState == "31" || VendorState == "34")
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
                    if ((compGstin[0].Trim() == "" && BranchGSTIN == "") || VendorState == "")
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
                }
                else
                {
                    if ((compGstin[0].Trim() == "" && BranchGSTIN == "") || VendorState == "")
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
                            if (Convert.ToString(ddl_VatGstCst.Value) == "0~0~X")
                            {
                                if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST")
                                {
                                    decimal finalCalCulatedOn = 0;
                                    decimal backProcessRate = (1 + (totalParcentage / 100));
                                    finalCalCulatedOn = obj.calCulatedOn / backProcessRate;
                                    //obj.calCulatedOn = Math.Round(finalCalCulatedOn);
                                    obj.calCulatedOn = finalCalCulatedOn;
                                }
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

                        DataRow[] filtr = MainTaxDataTable.Select("TaxCode ='" + obj.Taxes_ID + "' and slNo=" + Convert.ToString(slNo));
                        if (filtr.Length > 0)
                        {
                            obj.Amount = Convert.ToDouble(filtr[0]["Amount"]);
                            if (obj.Taxes_ID == 0)
                            {
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
                    DataTable TaxRecord = (DataTable)Session["PurchaseOrderFinalTaxRecord"];
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
                            if (Convert.ToString(ddl_VatGstCst.Value) == "0~0~X")
                            {
                                if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST")
                                {
                                    decimal finalCalCulatedOn = 0;
                                    decimal backProcessRate = (1 + (totalParcentage / 100));
                                    finalCalCulatedOn = obj.calCulatedOn / backProcessRate;
                                    //obj.calCulatedOn = Math.Round(finalCalCulatedOn);
                                    obj.calCulatedOn = finalCalCulatedOn;
                                }
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
                        DataRow[] filtrIndex = TaxRecord.Select("SlNo=" + Convert.ToString(slNo) + " and TaxCode=0");
                        if (filtrIndex.Length > 0)
                        {
                            aspxGridTax.JSProperties["cpComboCode"] = Convert.ToString(filtrIndex[0]["AltTaxCode"]);
                        }
                    }
                    Session["PurchaseOrderFinalTaxRecord"] = TaxRecord;
                }
                //New Changes 170217
                //GstCode should fetch everytime
                DataRow[] finalFiltrIndex = MainTaxDataTable.Select("SlNo=" + Convert.ToString(slNo) + " and TaxCode=0");
                if (finalFiltrIndex.Length > 0)
                {
                    aspxGridTax.JSProperties["cpComboCode"] = Convert.ToString(finalFiltrIndex[0]["AltTaxCode"]);
                }
                aspxGridTax.JSProperties["cpJsonData"] = createJsonForDetails(taxDetail);
                for (var i = 0; i < TaxDetailsDetails.Count; i++)
                {
                    decimal _Amount = Convert.ToDecimal(TaxDetailsDetails[i].Amount);
                    decimal _calCulatedOn = Convert.ToDecimal(TaxDetailsDetails[i].calCulatedOn);

                    TaxDetailsDetails[i].Amount = GetRoundOfValue(_Amount);
                    TaxDetailsDetails[i].calCulatedOn = Convert.ToDecimal(GetRoundOfValue(_calCulatedOn));
                }

                retMsg = Convert.ToString(GetTotalTaxAmount(TaxDetailsDetails));
                aspxGridTax.JSProperties["cpUpdated"] = "ok~" + retMsg;
                TaxDetailsDetails = setCalculatedOn(TaxDetailsDetails, taxDetail);
                aspxGridTax.DataSource = TaxDetailsDetails;
                aspxGridTax.DataBind();

        #endregion
            }
        }
        public double GetRoundOfValue(decimal Value)
        {
            double RoundOfValue = 0;

            if (Value > 0)
            {
                decimal _Value = Convert.ToDecimal(Value);
                _Value = Math.Round(Value, 2);

                RoundOfValue = Convert.ToDouble(_Value);
            }

            return RoundOfValue;
        }
        #region  Tax Details
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
            DataTable TaxRecord = (DataTable)Session["PurchaseOrderFinalTaxRecord"];
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


            Session["PurchaseOrderFinalTaxRecord"] = TaxRecord;


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

        }
        protected void cmbGstCstVatcharge_Callback(object sender, CallbackEventArgsBase e)
        {
            Session["POTaxDetails"] = null;
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
            if (Session["PurchaseOrderFinalTaxRecord"] != null)
            {
                TaxDetailTable = (DataTable)Session["PurchaseOrderFinalTaxRecord"];

                var rows = TaxDetailTable.Select("SlNo ='" + SrlNo + "'");
                foreach (var row in rows)
                {
                    row.Delete();
                }
                TaxDetailTable.AcceptChanges();

                Session["PurchaseOrderFinalTaxRecord"] = TaxDetailTable;
            }
        }
        public void UpdateTaxDetails(string oldSrlNo, string newSrlNo)
        {
            DataTable TaxDetailTable = new DataTable();
            if (Session["PurchaseOrderFinalTaxRecord"] != null)
            {
                TaxDetailTable = (DataTable)Session["PurchaseOrderFinalTaxRecord"];

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

                Session["PurchaseOrderFinalTaxRecord"] = TaxDetailTable;
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
        #region Inline Tax Details

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
            ProcedureExecute proc = new ProcedureExecute("prc__Project_PurchaseOrderDetailsList");
            proc.AddVarcharPara("@Action", 500, "TaxDetails");
            proc.AddIntegerPara("@PurchaseOrder_Id", Convert.ToInt32(Session["PurchaseOrder_Id"]));
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            //proc.AddVarcharPara("@branchId", 500, Convert.ToString(Session["userbranchID"])); 
            proc.AddVarcharPara("@branchId", 500, Convert.ToString(ddl_Branch.SelectedValue));
            proc.AddVarcharPara("@campany_Id", 500, Convert.ToString(Session["LastCompany"]));
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
                if (Session["POTaxDetails"] == null)
                {
                    Session["POTaxDetails"] = GetTaxData(dt_PLQuote.Date.ToString("yyyy-MM-dd"));
                }

                if (Session["POTaxDetails"] != null)
                {
                    TaxDetailsdt = (DataTable)Session["POTaxDetails"];


                    #region Delete Igst,Cgst,Sgst respectively
                    //Get Company Gstin 09032017
                    string CompInternalId = Convert.ToString(Session["LastCompany"]);
                    string[] compGstin = oDBEngine.GetFieldValue1("tbl_master_company", "cmp_gstin", "cmp_internalid='" + CompInternalId + "'", 1);
                    string ShippingState = "";
                    #region ##### Added By : Samrat Roy -- For BillingShippingUserControl ######
                    //edited by chinmoy
                    string sstateCode = "";
                    if (PurchaseOrderPosGst.Value != null)
                    {
                        if (PurchaseOrderPosGst.Value.ToString() == "S")
                        {
                            sstateCode = Purchase_BillingShipping.GeteShippingStateCode();
                        }
                        else
                        {
                            sstateCode = Purchase_BillingShipping.GetBillingStateCode();
                        }
                    }
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
                                //   Chandigarh     Andaman and Nicobar Islands    DADRA & NAGAR HAVELI    DAMAN & DIU      Lakshadweep              PONDICHERRY
                                if (ShippingState == "4" || ShippingState == "26" || ShippingState == "25" || ShippingState == "35" || ShippingState == "31" || ShippingState == "34")
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
                if (Session["POTaxDetails"] != null)
                {
                    TaxDetailsdt = (DataTable)Session["POTaxDetails"];
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

                    Session["POTaxDetails"] = TaxDetailsdt;
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
            if (Session["POTaxDetails"] != null)
            {
                TaxDetailsdt = (DataTable)Session["POTaxDetails"];
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

            Session["POTaxDetails"] = TaxDetailsdt;

            gridTax.DataSource = GetTaxes(TaxDetailsdt);
            gridTax.DataBind();
        }
        protected void gridTax_DataBinding(object sender, EventArgs e)
        {
            if (Session["POTaxDetails"] != null)
            {
                DataTable TaxDetailsdt = (DataTable)Session["POTaxDetails"];

                //gridTax.DataSource = GetTaxes();
                var taxlist = (List<Taxes>)GetTaxes(TaxDetailsdt);
                var taxChargeDataSource = setChargeCalculatedOn(taxlist, TaxDetailsdt);
                gridTax.DataSource = taxChargeDataSource;
            }
        }


        #endregion
        #region   Indent/Requisition Number
        //protected void lookup_quotation_DataBinding(object sender, EventArgs e)
        //{
        //    if (Session["IndentRequiData"] != null)
        //    {
        //        lookup_quotation.DataSource = (DataTable)Session["IndentRequiData"];
        //    }
        //}

        protected void BindLookUp(string OrderDate, string BranchId)
        {
            ProjectTermsBL ProjectTermsBL = new ProjectTermsBL();
            string status = string.Empty;
            //Subhabrata
            if (Convert.ToString(Request.QueryString["key"]) != "ADD")
            {
                status = "DONE";
            }
            else
            {
                status = "NOT-DONE";
            }//End            
            String prodtype = ddlInventory.SelectedValue;
            string strBranch = Convert.ToString(Session["userbranchHierarchy"]);
            //DataTable IndentTable = objPurchaseOrderBL.GetIndentOnPO(OrderDate, status, strBranch);
            DataTable IndentTable = new DataTable();
            if (rdl_Salesquotation.SelectedValue == "Indent")
            {
                IndentTable = ProjectTermsBL.GetIndentOnPO(OrderDate, status, strBranch);
            }
            else if (rdl_Salesquotation.SelectedValue == "Quotation")
            {
                IndentTable = ProjectTermsBL.GetQuotationOnPO(OrderDate, status, strBranch);  
            }
                
               // GetIndentOnPO(OrderDate, status, strBranch, prodtype);  //Tanmoy Search for Project Indent
            if (IndentTable.Rows.Count > 0)
            {
                taggingGrid.DataSource = IndentTable;
                taggingGrid.DataBind();
            }
            else
            {
                taggingGrid.DataSource = IndentTable;
                taggingGrid.DataBind();
            }
            Session["IndentRequiData"] = IndentTable;
        }
        //protected void ComponentQuotation_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        //{
        //    string status = string.Empty;
        //    string POrderDate = string.Empty;
        //    string customer = string.Empty;
        //    string IndentIds = string.Empty;
        //    if (e.Parameter.Split('~')[0] == "BindNullGrid")
        //    {
        //        lookup_quotation.GridView.Selection.UnselectAll();
        //        lookup_quotation.DataSource = null;
        //        lookup_quotation.DataBind();
        //        dt_Quotation.Text = "";
        //        ComponentQuotationPanel.JSProperties["cpNullGrid"] = "YES";
        //    }
        //    if (e.Parameter.Split('~')[0] == "BindIndentGrid")
        //    {

        //        if (Convert.ToString(Request.QueryString["key"]) != "ADD")
        //        {
        //            status = "DONE";
        //        }
        //        else
        //        {
        //            status = "NOT-DONE";
        //        }

        //        lookup_quotation.GridView.Selection.UnselectAll();
        //        POrderDate = e.Parameter.Split('~')[1];               
        //        string strBranch = Convert.ToString(Session["userbranchHierarchy"]);
        //        DataTable IndentTable = objPurchaseOrderBL.GetIndentOnPO(POrderDate, status, strBranch);
        //        if (IndentTable.Rows.Count > 0)
        //        {
        //            lookup_quotation.GridView.Selection.CancelSelection();
        //            lookup_quotation.DataSource = IndentTable;
        //            lookup_quotation.DataBind();
        //        }
        //        else
        //        {                    
        //            lookup_quotation.DataSource = null;
        //            lookup_quotation.DataBind();
        //        }
        //        Session["IndentRequiData"] = IndentTable;
        //    }
        //    else if (e.Parameter.Split('~')[0] == "BindQuotationGridOnSelection")
        //    {
        //        if (grid_Products.GetSelectedFieldValues("Quotation_No").Count != 0)
        //        {
        //            for (int i = 0; i < grid_Products.GetSelectedFieldValues("Quotation_No").Count; i++)
        //            {
        //                IndentIds += "," + grid_Products.GetSelectedFieldValues("Quotation_No")[i];
        //            }
        //            IndentIds = IndentIds.TrimStart(',');
        //            lookup_quotation.GridView.Selection.UnselectAll();
        //            if (!String.IsNullOrEmpty(IndentIds))
        //            {
        //                string[] eachQuo = IndentIds.Split(',');
        //                if (eachQuo.Length > 1)//More tha one quotation
        //                {
        //                    dt_Quotation.Text = "Multiple Select Quotation Dates";
        //                    //BindLookUp(Customer_Id, Order_Date);
        //                    foreach (string val in eachQuo)
        //                    {
        //                        lookup_quotation.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));
        //                    }
        //                }
        //                else if (eachQuo.Length == 1)//Single Quotation
        //                {
        //                    foreach (string val in eachQuo)
        //                    {
        //                        lookup_quotation.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));
        //                    }
        //                }
        //                else//No Quotation selected
        //                {
        //                    dt_Quotation.Text = "";
        //                    lookup_quotation.GridView.Selection.UnselectAll();
        //                }
        //            }
        //        }
        //        else if (grid_Products.GetSelectedFieldValues("Quotation_No").Count == 0)
        //        {
        //            lookup_quotation.GridView.Selection.UnselectAll();
        //            dt_Quotation.Text = "";
        //        }
        //    }

        //}
        protected void ComponentDatePanel_Callback(object sender, CallbackEventArgsBase e)
        {
            string strSplitCommand = e.Parameter.Split('~')[0];
            if (strSplitCommand == "BindQuotationDate")
            {
                string Indent_No = Convert.ToString(e.Parameter.Split('~')[1]);
                DataTable dt_indentDetails = objPurchaseOrderBL.GetIndentRequisitionDate(Indent_No);
                if (dt_indentDetails != null && dt_indentDetails.Rows.Count > 0)
                {
                    string quotationdate = Convert.ToString(dt_indentDetails.Rows[0]["Indent_RequisitionDate"]);
                    if (!string.IsNullOrEmpty(quotationdate))
                    {
                        dt_Quotation.Text = Convert.ToString(quotationdate);

                    }
                }
            }
        }
        protected void taggingGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string strSplitCommand = Convert.ToString(e.Parameters.Split('~')[0]);

            if (strSplitCommand == "BindComponentGrid")
            {

                // string BranchID = Convert.ToString(ddl_Branch.SelectedValue);
                string strBranch = Convert.ToString(Session["userbranchHierarchy"]);
                string FinYear = Convert.ToString(Session["LastFinYear"]);
                string POrderDate = dt_PLQuote.Date.ToString("yyyy-MM-dd");
                string Vendor = Convert.ToString(hdnCustomerId.Value);
                string status = string.Empty;

                if (Convert.ToString(Request.QueryString["key"]) != "ADD")
                {
                    status = "DONE";
                }
                else
                {
                    status = "NOT-DONE";
                }
                ProjectTermsBL ProjectTermsBL = new ProjectTermsBL();
                //DataTable IndentTable = objPurchaseOrderBL.GetIndentOnPO(POrderDate, status, strBranch);
                String prodtype = ddlInventory.SelectedValue;
                DataTable IndentTable = new DataTable();
                if (rdl_Salesquotation.SelectedValue == "Indent")
                {
                    IndentTable = ProjectTermsBL.GetIndentOnPO(POrderDate, status, strBranch);
                }
                else if (rdl_Salesquotation.SelectedValue == "Quotation")
                {
                    IndentTable = ProjectTermsBL.GetQuotationOnPO(POrderDate, status, strBranch);
                }
                Session["IndentRequiData"] = IndentTable;
                taggingGrid.DataSource = IndentTable;
                taggingGrid.DataBind();

            }
            else if (strSplitCommand == "SelectAndDeSelectProducts")
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
        protected void taggingGrid_DataBinding(object sender, EventArgs e)
        {
            if (Session["IndentRequiData"] != null)
            {
                taggingGrid.DataSource = (DataTable)Session["IndentRequiData"];
            }
        }
        protected void cgridProducts_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string strSplitCommand = e.Parameters.Split('~')[0];
            if (strSplitCommand == "BindProductsDetails")
            {
                String QuoComponent = "", QuoComponentNumber = "", QuoComponentDate = "";
                if (taggingGrid.GetSelectedFieldValues("Indent_Id").Count > 0)
                {
                    for (int i = 0; i < taggingGrid.GetSelectedFieldValues("Indent_Id").Count; i++)
                    {
                        QuoComponent += "," + Convert.ToString(taggingGrid.GetSelectedFieldValues("Indent_Id")[i]);
                        QuoComponentDate += "," + Convert.ToString(taggingGrid.GetSelectedFieldValues("Indent_RequisitionDate")[i]);
                        QuoComponentNumber += "," + Convert.ToString(taggingGrid.GetSelectedFieldValues("Indent_RequisitionNumber")[i]);
                    }

                    QuoComponent = QuoComponent.TrimStart(',');
                    QuoComponentDate = QuoComponentDate.TrimStart(',');
                    QuoComponentNumber = QuoComponentNumber.TrimStart(',');
                    if (taggingGrid.GetSelectedFieldValues("Indent_Id").Count > 0)
                    {
                        if (taggingGrid.GetSelectedFieldValues("Indent_Id").Count > 1)
                        {
                           // QuoComponentDate = "Multiple Project Purchase Order Dates";
                            QuoComponentDate = "Multiple Project Indent/Quotation  Dates";
                        }
                    }
                    else
                    {
                        QuoComponentDate = "";
                    }


                    string IndentID = Convert.ToString(QuoComponent.Split(',')[0]);

                    Int64 ProjId = 0;
                    if (rdl_Salesquotation.SelectedValue == "Indent")
                    {
                       
                        DataTable dtproj = GetProjectEditDataIndent(IndentID);
                        if (dtproj != null && dtproj.Rows.Count > 0)
                        {
                            ProjId = Convert.ToInt64(dtproj.Rows[0]["Proj_Id"]);
                        }
                        else
                        {
                            ProjId = 0;
                        }
                    }
                    else if (rdl_Salesquotation.SelectedValue == "Quotation")
                    {
                        DataTable dtproj = GetProjectEditDataQuatation(IndentID);
                        if (dtproj != null && dtproj.Rows.Count > 0)
                        {
                            ProjId = Convert.ToInt64(dtproj.Rows[0]["Proj_Id"]);
                        }
                        else
                        {
                            ProjId = 0;
                        }
                    }


                    string ProjectSelectEntryModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");

                    string DocType = Convert.ToString(rdl_Salesquotation.SelectedValue);
                    //string Quote_Nos = Convert.ToString(e.Parameters.Split('~')[1]);
                    //String QuoComponent = "";
                    //List<object> QuoList = lookup_quotation.GridView.GetSelectedFieldValues("Indent_Id");
                    //foreach (object Quo in QuoList)
                    //{
                    //    QuoComponent += "," + Quo;
                    //}
                    //QuoComponent = QuoComponent.TrimStart(',');
                    //if (Quote_Nos != "$")
                    //{
                    ProjectTermsBL ProjectTermsBL = new ProjectTermsBL();
                    string InventoryType = ddlInventory.SelectedValue;
                    DataTable dt_QuotationDetails = new DataTable();
                    string IdKey = Convert.ToString(Request.QueryString["key"]);
                    if (!string.IsNullOrEmpty(IdKey))
                    {
                        if (IdKey != "ADD")
                        {
                            //dt_QuotationDetails = objPurchaseOrderBL.GetIndentDetailsFromPO(QuoComponent, IdKey, "", "Edit", InventoryType);
                            if (DocType == "Indent")
                            {
                                dt_QuotationDetails = ProjectTermsBL.GetIndentDetailsFromPO(QuoComponent, IdKey, "", "Edit");
                            }
                            else if (DocType == "Quotation")
                            {
                                dt_QuotationDetails = ProjectTermsBL.GetQuotationDetailsFromPO(QuoComponent, IdKey, "", "Edit");
                            }
                        }
                        else
                        {
                           // dt_QuotationDetails = objPurchaseOrderBL.GetIndentDetailsFromPO(QuoComponent, "0", "", "Add", InventoryType);
                            if (DocType == "Indent")
                            {
                                dt_QuotationDetails = ProjectTermsBL.GetIndentDetailsFromPO(QuoComponent, "0", "", "Add");
                            }
                            else if (DocType == "Quotation")
                            {
                                dt_QuotationDetails = ProjectTermsBL.GetQuotationDetailsFromPO(QuoComponent, "0", "", "Add");
                            }
                        }

                    }
                    //else
                    //{
                    //    dt_QuotationDetails = objPurchaseOrderBL.GetIndentDetailsFromPO(QuoComponent, "", "");
                    //}
                    //Session["ProductOrderDetails"] = null;

                    grid_Products.DataSource = GetProductsInfo(dt_QuotationDetails);
                    grid_Products.DataBind();
                    grid_Products.JSProperties["cpComponentDetails"] = QuoComponentNumber + "~" + QuoComponentDate + "~" + ProjId;
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


        public IEnumerable GetProductsInfo(DataTable SalesOrderdt1)
        {
            List<SalesOrder> OrderList = new List<SalesOrder>();
            for (int i = 0; i < SalesOrderdt1.Rows.Count; i++)
            {
                SalesOrder Orders = new SalesOrder();

                Orders.SrlNo = Convert.ToString(i + 1);
                Orders.Key_UniqueId = Convert.ToString(i + 1);
                if (!string.IsNullOrEmpty(Convert.ToString(SalesOrderdt1.Rows[i]["Indent_No"])))
                { Orders.Quotation_No = Convert.ToInt64(SalesOrderdt1.Rows[i]["Indent_No"]); }
                else
                { Orders.Quotation_No = 0; }
                Orders.gvColProduct = Convert.ToString(SalesOrderdt1.Rows[i]["QuoteDetails_ProductId"]);
                Orders.gvColDiscription = Convert.ToString(SalesOrderdt1.Rows[i]["QuoteDetails_ProductDescription"]);
                Orders.gvColQuantity = Convert.ToString(SalesOrderdt1.Rows[i]["QuoteDetails_Quantity"]);
                Orders.Quotation_Num = Convert.ToString(SalesOrderdt1.Rows[i]["Indent"]);
                Orders.Product_Shortname = Convert.ToString(SalesOrderdt1.Rows[i]["Product_Name"]);
                Orders.QuoteDetails_Id = Convert.ToString(SalesOrderdt1.Rows[i]["QuoteDetails_Id"]);

                OrderList.Add(Orders);
            }

            return OrderList;
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
        #endregion
        #region  Available Stock
        protected void acpAvailableStock_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string performpara = e.Parameter;
            string strProductID = Convert.ToString(performpara.Split('~')[0]);
            string strBranch = Convert.ToString(ddl_Branch.SelectedValue);
            acpAvailableStock.JSProperties["cpstock"] = "0.00";
            acpAvailableStock.JSProperties["cpafterproductFocus"] = "";
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
            acpAvailableStock.JSProperties["cpafterproductFocus"] = "afterproductFocus";
        }
        #endregion Available Stock
        #region Purchase Order Mail
        public int Sendmail_Purchaseorder(string Output)
        {
            int stat = 0;
            if (chkmail.Checked)
            {
                if (cmbContactPerson.Value != null)
                {
                    Employee_BL objemployeebal = new Employee_BL();
                    ExceptionLogging mailobj = new ExceptionLogging();
                    EmailSenderHelperEL emailSenderSettings = new EmailSenderHelperEL();
                    DataTable dt_EmailConfig = new DataTable();
                    DataTable dt_EmailConfigpurchase = new DataTable();

                    DataTable dt_Emailbodysubject = new DataTable();
                    PurchaseorderEmailTags fetchModel = new PurchaseorderEmailTags();
                    string Subject = "";
                    string Body = "";
                    string emailTo = "";
                    int MailStatus = 0;

                    var customerid = cmbContactPerson.Value.ToString();
                    dt_EmailConfig = objemployeebal.Getemailids(customerid);
                    string FilePath = Server.MapPath("~/Reports/RepxReportDesign/PurchaseOrder/DocDesign/PDFFiles/" + "PO-Default-" + Output + ".pdf");
                    string FileName = FilePath;
                    if (dt_EmailConfig.Rows.Count > 0)
                    {
                        emailTo = Convert.ToString(dt_EmailConfig.Rows[0]["eml_email"]);
                        dt_Emailbodysubject = objemployeebal.Getemailtemplates("13");  //for purchase order
                        if (dt_Emailbodysubject.Rows.Count > 0)
                        {
                            Body = Convert.ToString(dt_Emailbodysubject.Rows[0]["body"]);
                            Subject = Convert.ToString(dt_Emailbodysubject.Rows[0]["subjct"]);

                            dt_EmailConfigpurchase = objemployeebal.Getemailtagsforpurchase(Output, "PurchaseOrderEmailTags");  //For Purchase Order Get all Tags Value

                            if (dt_EmailConfigpurchase.Rows.Count > 0)
                            {
                                fetchModel = DbHelpers.ToModel<PurchaseorderEmailTags>(dt_EmailConfigpurchase);
                                Body = Employee_BL.GetFormattedString<PurchaseorderEmailTags>(fetchModel, Body);
                                Subject = Employee_BL.GetFormattedString<PurchaseorderEmailTags>(fetchModel, Subject);
                            }

                            emailSenderSettings = mailobj.GetEmailSettingsforAllreport(emailTo, "", "", FilePath, Body, Subject);
                            if (emailSenderSettings.IsSuccess)
                            {
                                string Message = "";
                                EmailSenderEL obj2 = new EmailSenderEL();
                                stat = SendEmailUL.sendMailInHtmlFormat(emailSenderSettings.ModelCast<EmailSenderEL>(), out Message);

                            }
                        }
                    }
                }

            }
            return stat;
        }
        #endregion
        #region Visibility of Send Mail

        public void VisiblitySendEmail()
        {
            Employee_BL objemployeebal = new Employee_BL();
            chkmail.Visible = true;
            string userid = "";
            string branchId = "";
            string userfetch = "";
            if (HttpContext.Current.Session["userid"] != null)
            {
                userid = HttpContext.Current.Session["userid"].ToString();

            }

            if (HttpContext.Current.Session["userbranchID"] != null)
            {

                branchId = HttpContext.Current.Session["userbranchID"].ToString();
            }

            if (!Request.QueryString.AllKeys.Contains("status") && Request.QueryString["key"] != "ADD")
            {
                ////if (Request.QueryString["key"] != "ADD" && Request.QueryString.AllKeys.Contains("status") ==false )
                //{
                chkmail.Visible = false;
            }
            else
            {
                DataTable dt2 = new DataTable();
                dt2 = objemployeebal.GetSystemsettingmail("Show Email in PO");
                if (Convert.ToString(dt2.Rows[0]["Variable_Value"]) == "Yes")
                {

                    DataTable dt = new DataTable();
                    dt = objemployeebal.GetApporvalMail(branchId, "PO");
                    if (dt.Rows.Count > 0)
                    {

                        userfetch = Convert.ToString(dt.Rows[0]["userid"]);
                        if (userfetch != "0")
                        {

                            if (userid != userfetch)
                            {
                                chkmail.Visible = false;

                            }
                        }
                    }
                }
                else
                {
                    chkmail.Visible = false;

                }

            }
        }

        #endregion

        #region Set session For Packing Quantity
        [WebMethod]
        public static string SetSessionPacking(string list)
        {
            System.Web.Script.Serialization.JavaScriptSerializer jsSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            List<ProductQuantity> finalResult = jsSerializer.Deserialize<List<ProductQuantity>>(list);
            HttpContext.Current.Session["SessionPackingDetails"] = finalResult;
            return null;

        }
        #endregion
        public class ProductTaxDetails
        {
            public string SrlNo { get; set; }
            public string IsTaxEntry { get; set; }
        }

        //Rev Tanmoy 23-09-2019
        protected void ProjectServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Proj_Id";
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            string branch = ddl_Branch.SelectedValue;
            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);

            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);

            var q = from d in dc.V_ProjectLists
                    where d.ProjectStatus == "Approved" && d.ProjBracnchid == Convert.ToInt32(branch)
                    orderby d.Proj_Id descending
                    select d;

            e.QueryableSource = q;
        }
        //End Rev

        //Rev Tanmoy 12-11-2019 Search from Project Indent
        public DataTable GetIndentOnPO(string OrderDate, string Status, string branch, String prodtype)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_GetProjectIndentOnPurchaseOrder");
            proc.AddVarcharPara("@OrderDate",10, OrderDate);
            proc.AddVarcharPara("@Status", 50, Status);
            // proc.AddIntegerPara("@branch",Convert.ToInt32(branch));
            proc.AddVarcharPara("@branch", 500, branch);
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@campany_Id", 500, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@ProdType", 500, prodtype);
            dt = proc.GetTable();
            return dt;
        }
        //End Rev
        [WebMethod]
        public static object SetProjectCode(string OrderId, string TagDocType)
        {
            List<DocumentDetails> Detail = new List<DocumentDetails>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList");
                proc.AddVarcharPara("@Action", 500, "PurchaseOrdertaggingProjectdata");
                proc.AddVarcharPara("@PurchaseOrder_Id", 100, OrderId);
                proc.AddVarcharPara("@TagDocType", 500, TagDocType);
                DataTable address = proc.GetTable();



                Detail = (from DataRow dr in address.Rows
                          select new DocumentDetails()

                          {
                              ProjectId = Convert.ToInt64(dr["ProjectId"]),
                              ProjectCode = Convert.ToString(dr["ProjectCode"])
                          }).ToList();
                return Detail;

            }
            return null;

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
                    DataTable MainTaxDataTable = (DataTable)HttpContext.Current.Session["PurchaseOrderFinalTaxRecord"];

                    DataRow[] deletedRow = MainTaxDataTable.Select("SlNo=" + srl);
                    if (deletedRow.Length > 0)
                    {
                        foreach (DataRow dr in deletedRow)
                        {
                            MainTaxDataTable.Rows.Remove(dr);
                        }

                    }

                    HttpContext.Current.Session["PurchaseOrderFinalTaxRecord"] = MainTaxDataTable;
                   // GetStock(Convert.ToString(performpara.Split('~')[2]));
                    // DeleteWarehouse(Convert.ToString(performpara.Split('~')[1]));
                    DataTable taxDetails = (DataTable)HttpContext.Current.Session["POTaxDetails"];
                    if (taxDetails != null)
                    {
                        foreach (DataRow dr in taxDetails.Rows)
                        {
                            dr["Amount"] = "0.00";
                        }
                        HttpContext.Current.Session["POTaxDetails"] = taxDetails;
                    }
                }
                else if (Action == "DeleteAllTax")
                {

                    CreateDataTaxTableByAjax();

                    DataTable taxDetails = (DataTable)HttpContext.Current.Session["POTaxDetails"];

                    if (taxDetails != null)
                    {
                        foreach (DataRow dr in taxDetails.Rows)
                        {
                            dr["Amount"] = "0.00";
                        }
                        HttpContext.Current.Session["POTaxDetails"] = taxDetails;
                    }

                }
                else
                {
                    DataTable MainTaxDataTable = (DataTable)HttpContext.Current.Session["PurchaseOrderFinalTaxRecord"];

                    DataRow[] deletedRow = MainTaxDataTable.Select("SlNo=" + srl);
                    if (deletedRow.Length > 0)
                    {
                        foreach (DataRow dr in deletedRow)
                        {
                            MainTaxDataTable.Rows.Remove(dr);
                        }

                    }

                    HttpContext.Current.Session["PurchaseOrderFinalTaxRecord"] = MainTaxDataTable;

                    DataTable taxDetails = (DataTable)HttpContext.Current.Session["POTaxDetails"];

                    if (taxDetails != null)
                    {
                        foreach (DataRow dr in taxDetails.Rows)
                        {
                            dr["Amount"] = "0.00";
                        }
                        HttpContext.Current.Session["POTaxDetails"] = taxDetails;
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
                ProcedureExecute proc = new ProcedureExecute("prc__Project_PurchaseOrderDetailsList");
                proc.AddVarcharPara("@Action", 100, "GetSimilarProjectCheckforProjPurcOrder");
                proc.AddVarcharPara("@TagDocType", 100, Doctype);
                proc.AddVarcharPara("@SelectedComponentList", 500, quote_Id);
                proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
                proc.RunActionQuery();
                returnValue = Convert.ToString(proc.GetParaValue("@ReturnValue"));

            }
            return returnValue;

        }

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

        // Rev 1.0
        [WebMethod]
        public static string GetAddLock(DateTime LockDate)
        {
            string rtrnvalue = "0";
            ProcedureExecute proc = new ProcedureExecute("prc__Project_PurchaseOrderDetailsList");
            proc.AddVarcharPara("@Action", 500, "GetAddLockForProjectPurchaseOrderStatus");
            proc.AddDateTimePara("@TransactionDate", LockDate);
            proc.AddVarcharPara("@ReturnValue", 200, "0", QueryParameterDirection.Output);
            proc.RunActionQuery();
            rtrnvalue = Convert.ToString(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;
        }
        // End of Rev 1.0
    }
}