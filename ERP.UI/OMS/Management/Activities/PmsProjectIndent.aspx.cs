using BusinessLogicLayer;
using DataAccessLayer;
using DevExpress.Web;
using DevExpress.Web.Data;
using ERP.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Activities
{
    public partial class PmsProjectIndent : ERP.OMS.ViewState_class.VSPage//System.Web.UI.Page
    {
        PurchaseIndentBL objPurchaseIndentBL = new PurchaseIndentBL();
        BusinessLogicLayer.RemarkCategoryBL reCat = new BusinessLogicLayer.RemarkCategoryBL();
        DataTable Remarks = null;
        SalesInvoiceBL objSalesInvoiceBL = new SalesInvoiceBL();
        string QuotationIds = string.Empty;

        //CRMSalesOrderDtlBL objCRMSalesOrderDtlBL = new CRMSalesOrderDtlBL();
        //CRMSalesDtlBL objPurchaseIndentBL = new CRMSalesDtlBL();

        #region Sandip Section For Approval Section Start
        ERPDocPendingApprovalBL objERPDocPendingApproval = new ERPDocPendingApprovalBL();
        int KeyValue = 0;
        #endregion Sandip Section For Approval Dtl Section End

        #region LocalVariable
        SqlDataSource Obj_Sds;
        BusinessLogicLayer.DBEngine oDbEngine;
        string strCon;
        string CurrentSegment;
        DataTable DtCurrentSegment;
        DataTable dtXML = new DataTable();
        BusinessLogicLayer.GenericLogSystem oGenericLogSystem;
        // PurchaseIndentCS objPurchaseIndentBL = new PurchaseIndentCS();
        string CashBankVoucherFile_XMLPATH = null;
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();


        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();


        static string ForJournalDate = null;
        string JVNumStr = string.Empty;
        public EntityLayer.CommonELS.UserRightsForPage rights = new EntityLayer.CommonELS.UserRightsForPage();
        #endregion

        CommonBL cbl = new CommonBL();

        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {
            #region Sandip Section For Approval Section to Change Master Page Dyanamically Start
            //Rev Debashis
            //if (Request.QueryString.AllKeys.Contains("status"))
            if (Request.QueryString.AllKeys.Contains("status") || Request.QueryString.AllKeys.Contains("IsTagged"))
            //End of Rev Debashis
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
        protected void Page_Init(object sender, EventArgs e)
        {
            SqlSchematype.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            dsBranch.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            dsHierarchy.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlCurrency.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlCurrencyBind.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            String finyear = "";
            finyear = Convert.ToString(Session["LastFinYear"]).Trim();
            SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
            DataTable dtFinYear = objSlaesActivitiesBL.GetFinacialYearBasedQouteDate(finyear);
            Session["FinYearStartDate"] = Convert.ToString(dtFinYear.Rows[0]["finYearStartDate"]);
            Session["FinYearEndDate"] = Convert.ToString(dtFinYear.Rows[0]["finYearEndDate"]);

            FormDate.MaxDate = Convert.ToDateTime(Session["FinYearEndDate"]);
            FormDate.MinDate = Convert.ToDateTime(Session["FinYearStartDate"]);
            toDate.MaxDate = Convert.ToDateTime(Session["FinYearEndDate"]);
            toDate.MinDate = Convert.ToDateTime(Session["FinYearStartDate"]);

            //Rev Tanmoy
            string ProjectSelectInEntryModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");
            if (!String.IsNullOrEmpty(ProjectSelectInEntryModule))
            {
                if (ProjectSelectInEntryModule == "Yes")
                {
                    Grid_PurchaseIndent.Columns[10].Visible = true;
                }
                else if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
                {
                    Grid_PurchaseIndent.Columns[10].Visible = false;
                }
            }


            string ProjectMandatoryInEntry = cbl.GetSystemSettingsResult("ProjectMandatoryInEntry");

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


            //string ProjectSelectInEntryModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");
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
            //End Rev Tanmoy

            //For Hierarchy Start Tanmoy
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
            //For Hierarchy End Tanmoy

            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/PmsProjectIndent.aspx");
            if (!IsPostBack)
            {


                //................Cookies..................
                if (this.Page.MasterPageFile != "/OMS/MasterPage/PopUp.Master")
                {
                    Grid_PurchaseIndent.SettingsCookies.CookiesID = "BreeezeErpGridCookiesGrid_PurchaseIndentPagePurchaseIndent";
                    this.Page.ClientScript.RegisterStartupScript(GetType(), "setCookieOnStorage", "<script>addCookiesKeyOnStorage('BreeezeErpGridCookiesGrid_PurchaseIndentPagePurchaseIndent');</script>");
                }
                //...........Cookies End............... 
                string userbranchHierachy = Convert.ToString(Session["userbranchHierarchy"]);
                string FinYear = Convert.ToString(Session["LastFinYear"]);
                PopulateDropDown(userbranchHierachy, FinYear);
                IsUdfpresent.Value = Convert.ToString(getUdfCount());
                BindHierarchy();
                BindBranchFrom();

                SetFinYearCurrentDate();
                Session["ProjectIndateDetails"] = null;
                Session["Indent_Id"] = null;
                Session["IndentIdPO"] = null;
                Session["InlineRemarks"] = null;
                Session["Indent_TagID"] = null;
                // Session["ProjectIndateDetails"] = null;
                Session["SI_InvoiceID"] = null;
                this.Page.ClientScript.RegisterStartupScript(GetType(), "CS", "<script>PageLoad();</script>");
                if (!String.IsNullOrEmpty(Convert.ToString(Session["userbranchID"])))
                {
                    string strdefaultBranch = Convert.ToString(Session["userbranchID"]);
                    ddlBranch.SelectedValue = strdefaultBranch;
                }
                if (!String.IsNullOrEmpty(Convert.ToString(Session["LocalCurrency"])))
                {
                    string LocalCurrency = Convert.ToString(Session["LocalCurrency"]);
                    string basedCurrency = Convert.ToString(LocalCurrency.Split('~')[0]);
                    string CurrencyId = Convert.ToString(basedCurrency[0]);
                    CmbCurrency.Value = CurrencyId;
                }
                #region Sandip Section For Approval Section Start
                #region Session Remove Section Start
                Session.Remove("INPendingApproval");
                Session.Remove("INUserWiseERPDocCreation");
                #endregion Session Remove Section End
                ConditionWiseShowStatusButton();

                if (Request.QueryString.AllKeys.Contains("status"))
                {
                    string indentid = Convert.ToString(Request.QueryString["key"]);
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "StartingSetupForApproval(" + indentid + ")", true);
                    btncross.Visible = false;
                    btnnew.Visible = false;
                    ApprovalCross.Visible = true;
                    ddlBranch.Enabled = false;
                }
                //Rev Debashis
                else if (Request.QueryString.AllKeys.Contains("IsTagged"))
                {
                    string indentid = Convert.ToString(Request.QueryString["key"]);
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "StartingSetupForApproval(" + indentid + ")", true);
                    btncross.Visible = false;
                    btnnew.Visible = false;
                    ApprovalCross.Visible = true;
                    ddlBranch.Enabled = false;
                    Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>zoompmspurchaseindent(" + Request.QueryString["key"] + ", '" + Request.QueryString["req"] + "');</script>");
                }
                //End of Rev Debashis
                else
                {
                    btncross.Visible = true;
                    btnnew.Visible = true;
                    ApprovalCross.Visible = false;
                    ddlBranch.Enabled = true;
                }
                #endregion Sandip Section For Approval Dtl Section End

                //Subhra 28-02-2019

                MasterSettings objmaster = new MasterSettings();
                hdnConvertionOverideVisible.Value = objmaster.GetSettings("ConvertionOverideVisible");
                hdnShowUOMConversionInEntry.Value = objmaster.GetSettings("ShowUOMConversionInEntry");

                //Subhra 28-02-2019
            }
            //FillGrid();
            Bind_NumberingScheme();
            // Code Commented By Sam TO avoid rebind again and again by server 
            // It reflects always branchid 1 as head office during save
            // Version 1.0.0.1
            //BindBranchFrom();
            // Code End
            #region Sandip Section For Approval Section Start
            if (divPendingWaiting.Visible == true)
            {
                PopulateUserWiseERPDocCreation();
                PopulateApprovalPendingCountByUserLevel();
                PopulateERPDocApprovalPendingListByUserLevel();
                if (Request.QueryString.AllKeys.Contains("status"))
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "StartingSetupForApproval()", true);
                }
                //else
                //{
                //    if (hdn_Mode.Value == "Edit" && hdngridkeyval.Value!="")
                //    {
                //        IsExistsDocumentInERPDocApproveStatus(hdngridkeyval.Value);
                //    }
                //    else
                //    {
                //        btnnew.Visible = true;
                //        btnSaveExit.Visible = true;
                //    }
                //}

            }
            #endregion Sandip Section For Approval Dtl Section End
            if (hdn_Mode.Value == "Entry")
            {
                Keyval_internalId.Value = "Add";
            }

        }

        public void SetFinYearCurrentDate()
        {
            tDate.EditFormatString = objConverter.GetDateFormat("Date");
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

            tDate.Value = DateTime.ParseExact(fDate, @"M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        }
        protected int getUdfCount()
        {
            DataTable udfCount = oDBEngine.GetDataTable("select 1 from tbl_master_remarksCategory rc where cat_applicablefor='PI' and ( exists (select * from tbl_master_udfGroup where id=rc.cat_group_id and grp_isVisible=1) or rc.cat_group_id=0)");
            return udfCount.Rows.Count;
        }
        public void Bind_NumberingScheme()
        {
            string strCompanyID = Convert.ToString(Session["LastCompany"]);
            string strBranchID = Convert.ToString(Session["userbranchID"]);
            string FinYear = Convert.ToString(Session["LastFinYear"]);
            string userbranchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);
            SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
            DataTable Schemadt = objSlaesActivitiesBL.GetNumberingSchema(strCompanyID, userbranchHierarchy, FinYear, "120", "N");
            if (Schemadt != null && Schemadt.Rows.Count > 0)
            {
                CmbScheme.TextField = "SchemaName";
                CmbScheme.ValueField = "Id";
                CmbScheme.DataSource = Schemadt;
                CmbScheme.DataBind();
            }
        }
        //rev Tanmoy for Hierarchy
        public void BindHierarchy()
        {
            dsHierarchy.SelectCommand = "select 0 as Hierarchy_ID ,'Select' as HIERARCHY_NAME union select ID as Hierarchy_ID,H_Name as HIERARCHY_NAME from V_HIERARCHY ";
            ddlHierarchy.DataBind();
        }
        //End rev Tanmoy for Hierarchy
        public void BindBranchFrom()
        {
            //dsBranch.SelectCommand = "SELECT BRANCH_id AS BANKBRANCH_ID , RTRIM(BRANCH_DESCRIPTION)+' ['+ISNULL(RTRIM(BRANCH_CODE),'')+']' AS BANKBRANCH_NAME  FROM TBL_MASTER_BRANCH where BRANCH_id in(" + Convert.ToString(Session["userbranchHierarchy"]) + ")";
            dsBranch.SelectCommand = "SELECT BRANCH_id AS BANKBRANCH_ID , RTRIM(BRANCH_DESCRIPTION)+' ['+ISNULL(RTRIM(BRANCH_CODE),'')+']' AS BANKBRANCH_NAME  FROM TBL_MASTER_BRANCH ";
            ddlBranch.DataBind();
        }
        public void bindexport(int Filter)
        {
            //Code  Added and Commented By Priti on 20122016 to use Export Header,date
            //exporter.GridView = Grid_ContraVoucher;

            exporter.GridViewID = "Grid_PurchaseIndent";
            string filename = "PurchaseIndentRequisition";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Purchase Indent Requisition";
            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";
            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();
                    break;
                case 2:
                    exporter.WriteXlsToResponse();
                    break;
                case 3:
                    exporter.WriteRtfToResponse();
                    break;
                case 4:
                    exporter.WriteCsvToResponse();
                    break;
            }
        }
        protected void drdExport_SelectedIndexChanged(object sender, EventArgs e)
        {

            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));

            if (Filter != 0)
            {
                if (Session["exportval"] == null)
                {
                    Session["exportval"] = Filter;
                    bindexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportval"]) != Filter)
                {
                    Session["exportval"] = Filter;
                    bindexport(Filter);
                }
            }

        }

        //**Abhisek**//
        public void PopulateDropDown(string branchHierchy, string FinYear)
        {
            DataSet dst = GetAllDropDownDetail(branchHierchy, FinYear);

            FormDate.Date = DateTime.Now;
            toDate.Date = DateTime.Now;
            //dtBilldate.Date = DateTime.Now;

            if (dst.Tables[0] != null && dst.Tables[0].Rows.Count > 0)
            {
                cmbBranchfilter.DataSource = dst.Tables[0];
                cmbBranchfilter.ValueField = "branch_id";
                cmbBranchfilter.TextField = "branch_description";
                cmbBranchfilter.DataBind();

                cmbBranchfilter.Value = Convert.ToString(Session["userbranchID"]);
            }


        }
        public DataSet GetAllDropDownDetail(string branchList, string FinYear)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Prc_DownPaymentEntry_Details");
            proc.AddVarcharPara("@Action", 100, "GetAllDropDownDetail");
            proc.AddVarcharPara("@BranchList", 4000, branchList);
            proc.AddVarcharPara("@FinYear", 10, FinYear);
            ds = proc.GetDataSet();
            return ds;
        }
        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Indent_Id";

            // string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);



            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);


            List<int> branchidlist;
            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);

            if (IsFilter == "Y")
            {
                if (strBranchID == "0")
                {
                    string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));

                    var q = from d in dc.V_ProjectPurchaseIndentLists
                            where d.Indent_RequisitionDateTimeFormat >= Convert.ToDateTime(strFromDate) && d.Indent_RequisitionDateTimeFormat <= Convert.ToDateTime(strToDate) &&
                            d.Indent_FinYear == Convert.ToString(Session["LastFinYear"]) &&
                            d.Indent_Company == Convert.ToString(Session["LastCompany"]) &&
                            branchidlist.Contains(Convert.ToInt32(d.Indent_BranchIdFor))
                            orderby d.Indent_RequisitionDateTimeFormat descending
                            select d;
                    e.QueryableSource = q;
                }
                else
                {
                    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));
                    var q = from d in dc.V_ProjectPurchaseIndentLists
                            where
                            d.Indent_RequisitionDateTimeFormat >= Convert.ToDateTime(strFromDate) && d.Indent_RequisitionDateTimeFormat <= Convert.ToDateTime(strToDate) &&
                            branchidlist.Contains(Convert.ToInt32(d.Indent_BranchIdFor)) &&
                            d.Indent_FinYear == Convert.ToString(Session["LastFinYear"]) &&
                            d.Indent_Company == Convert.ToString(Session["LastCompany"])
                            orderby d.Indent_RequisitionDateTimeFormat descending
                            select d;
                    e.QueryableSource = q;
                }
            }
            else
            {
                var q = from d in dc.V_ProjectPurchaseIndentLists
                        where d.Indent_FinYear == Convert.ToString(Session["LastFinYear"]) &&
                                d.Indent_Company == Convert.ToString(Session["LastCompany"]) &&
                                d.Indent_BranchIdFor == 0
                        orderby d.Indent_RequisitionDateTimeFormat descending
                        select d;
                e.QueryableSource = q;
            }
        }

        public void FillGrid()
        {
            //DataTable dtdata = GetGridData();
            DataTable dtdata = GetPurchaseIndentListGridData();


            if (dtdata != null && dtdata.Rows.Count > 0)
            {
                Grid_PurchaseIndent.DataSource = dtdata;
                Grid_PurchaseIndent.DataBind();
            }
            else
            {
                Grid_PurchaseIndent.DataSource = null;
                Grid_PurchaseIndent.DataBind();
            }
        }
        public DataTable GetPurchaseIndentListGridData()
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseIndentDetailsList");
            proc.AddVarcharPara("@Action", 500, "PurchaseIndent");
            proc.AddVarcharPara("@CampanyID", 500, Convert.ToString(Session["LastCompany"]));
            proc.AddVarcharPara("@userbranchHierarchy", 500, Convert.ToString(Session["userbranchHierarchy"]));
            // proc.AddVarcharPara("@Companybranch", 500, Convert.ToString(Session["userbranchID"]));
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            dt = proc.GetTable();
            return dt;
        }

        public class Product
        {
            public string ProductID { get; set; }
            public string ProductName { get; set; }
        }
        public DataSet GetProductData()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseIndentDetailsList");
            proc.AddVarcharPara("@Action", 500, "ProductDetailsPurchaseIndent");
            ds = proc.GetDataSet();
            return ds;
        }
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

        [WebMethod]
        public static string getSchemeType(string sel_scheme_id)
        {
            //BusinessLogicLayer.DBEngine oDbEngine1 = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            //string[] scheme = oDbEngine1.GetFieldValue1("tbl_master_Idschema", "schema_type", "Id = " + Convert.ToInt32(sel_scheme_id), 1);
            //return Convert.ToString(scheme[0]);

            string strschematype = "", strschemalength = "", strschemavalue = "", strschemaBranch = "", Valid_From = "", Valid_Upto = "";


            // BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();



            DataTable DT = objEngine.GetDataTable("tbl_master_Idschema", " schema_type,length,Branch,Valid_From,Valid_Upto ", " Id = " + Convert.ToInt32(sel_scheme_id));

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                strschematype = Convert.ToString(DT.Rows[i]["schema_type"]);
                strschemalength = Convert.ToString(DT.Rows[i]["length"]);
                strschemaBranch = Convert.ToString(DT.Rows[i]["Branch"]);
                Valid_From = Convert.ToDateTime(DT.Rows[i]["Valid_From"]).ToString("MM-dd-yyyy");
                Valid_Upto = Convert.ToDateTime(DT.Rows[i]["Valid_Upto"]).ToString("MM-dd-yyyy");
                strschemavalue = strschematype + "~" + strschemalength + "~" + strschemaBranch + "~" + Valid_From + "~" + Valid_Upto;
            }
            return Convert.ToString(strschemavalue);
        }
        [WebMethod]
        public static String GetRate(string basedCurrency, string Currency_ID, string Campany_ID)
        {

            SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
            DataTable dt = objSlaesActivitiesBL.GetCurrentConvertedRate(Convert.ToInt16(basedCurrency), Convert.ToInt16(Currency_ID), Campany_ID);
            string SalesRate = "";
            if (dt.Rows.Count > 0)
            {
                SalesRate = Convert.ToString(dt.Rows[0]["SalesRate"]);
            }

            return SalesRate;
        }
        public class BratchGridLIST
        {
            public string SrlNo { get; set; }
            public string PurchaseIndentID { get; set; }
            public string gvColProduct { get; set; }
            public string gvColDiscription { get; set; }
            public string gvColQuantity { get; set; }
            public string gvColUOM { get; set; }
            public string gvColRate { get; set; }
            public string gvColValue { get; set; }
            public Nullable<DateTime> ExpectedDeliveryDate { get; set; }

            public string Status { get; set; }
            public string AvailableStock { get; set; }
            public string ProductName { get; set; }
            public string gvColIndentDetailsId { get; set; }
            public string Remarks { get; set; }

            public string gvProdDetailsId { get; set; }
            public string gvDetailsId { get; set; }
            public string gvPRODUCTFROM { get; set; }

            //chinmoy added for balqty start 05-12-2019
            //public string BalQty { get; set; }
            //End

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


        protected void gridBatch_DataBinding(object sender, EventArgs e)
        {
            if (Session["ProjectIndateDetails"] != null)
            {
                DataTable dvData = (DataTable)Session["ProjectIndateDetails"];
                DataView dvDataView = new DataView(dvData);
                dvDataView.RowFilter = "Status <> 'D'";

                gridBatch.DataSource = GetPurchaseIndent(dvDataView.ToTable());
            }
            else
            {
                gridBatch.DataSource = BindBratchGrid();
            }
        }
        protected void gridBatch_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (e.Column.FieldName == "SrlNo")
            {
                e.Editor.ReadOnly = true;
            }
            else
                if (e.Column.FieldName == "gvColDiscription")
                {
                    e.Editor.ReadOnly = true;
                }
                else if (e.Column.FieldName == "gvColUOM")
                {
                    e.Editor.ReadOnly = true;
                }
                //else if (e.Column.FieldName == "gvColRate")
                //{
                //    e.Editor.ReadOnly = true;
                //}
                else if (e.Column.FieldName == "gvColAvailableStock")
                {
                    e.Editor.ReadOnly = true;
                }
                else if (e.Column.FieldName == "gvColValue")
                {
                    e.Editor.ReadOnly = true;
                }
                else if (e.Column.FieldName == "ProductName")
                {
                    e.Editor.ReadOnly = true;
                }
                else
                {
                    e.Editor.ReadOnly = false;
                }

        }

        protected void gridBatch_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            DataTable Indentdt = new DataTable();
            string IsDeleteFrom = Convert.ToString(hdfIsDelete.Value);
            string IndentComponentDate = "", IndentComponent = "";

            if (Session["ProjectIndateDetails"] != null)
            {
                DataTable dts = new DataTable();
                Indentdt = (DataTable)Session["ProjectIndateDetails"];

                //chinmoy added for balqty 05-12-2019
                //dts = Indentdt.Copy();
                //foreach (DataRow row in dts.Rows)
                //{
                //    DataColumnCollection dtC = dts.Columns;


                //    if (dtC.Contains("BalQty"))
                //    { 
                //        dts.Columns.Remove("BalQty"); 
                //    }
                //    break;
                //}
                //Indentdt = dts;

                //End

            }
            else
            {
                Indentdt.Columns.Add("SrlNo", typeof(string));//1
                Indentdt.Columns.Add("IndentDetailsId", typeof(string));//2
                Indentdt.Columns.Add("ProductID", typeof(string));//3
                Indentdt.Columns.Add("Description", typeof(string));//4
                Indentdt.Columns.Add("Quantity", typeof(string));//5
                Indentdt.Columns.Add("UOM", typeof(string));//6
                Indentdt.Columns.Add("Rate", typeof(string));//7
                Indentdt.Columns.Add("ValueInBaseCurrency", typeof(string));//8
                Indentdt.Columns.Add("ExpectedDeliveryDate", typeof(string));//9

                Indentdt.Columns.Add("Status", typeof(string));//10
                Indentdt.Columns.Add("AvailableStock", typeof(string));//11
                Indentdt.Columns.Add("ProductName", typeof(string));//12
                Indentdt.Columns.Add("Remarks", typeof(string));//13

                Indentdt.Columns.Add("gvProdDetailsId", typeof(string));
                Indentdt.Columns.Add("gvDetailsId", typeof(string));
                Indentdt.Columns.Add("gvPRODUCTFROM", typeof(string));
            }

            foreach (var args in e.InsertValues)
            {
                string ProductDetails = Convert.ToString(args.NewValues["gvColProduct"]);

                if (ProductDetails != "" && ProductDetails != "0")
                {
                    string SrlNo = Convert.ToString(args.NewValues["SrlNo"]);

                    string[] ProductDetailsList = ProductDetails.Split(new string[] { "||@||" }, StringSplitOptions.None);
                    string ProductID = ProductDetailsList[0];

                    string ProductName = Convert.ToString(args.NewValues["ProductName"]);
                    string Description = Convert.ToString(args.NewValues["gvColDiscription"]);
                    string Quantity = (Convert.ToString(args.NewValues["gvColQuantity"]) != "") ? Convert.ToString(args.NewValues["gvColQuantity"]) : "0";
                    string UOM = Convert.ToString(args.NewValues["gvColUOM"]);
                    //string UOM = Convert.ToString(ProductDetailsList[3]);
                    string Rate = Convert.ToString(args.NewValues["gvColRate"]);
                    // string StockQuantity = Convert.ToString(args.NewValues["gvColValue"]);
                    //string Rate = Convert.ToString(ProductDetailsList[7]);
                    string Amount = (Convert.ToString(args.NewValues["gvColValue"]) != "") ? Convert.ToString(args.NewValues["gvColValue"]) : "0";
                    string Date = Convert.ToString(args.NewValues["ExpectedDeliveryDate"]);

                    string Remarks = Convert.ToString(args.NewValues["Remarks"]);
                    //if (Date=="")
                    //{
                    //    Date = "1900-01-01";
                    //}
                    Indentdt.Rows.Add(SrlNo, "0", ProductDetails, Description, Quantity, UOM, Rate, Amount, Date, "I", 0, ProductName, Remarks, null, null, null);
                    //Indentdt.Rows.Add(SrlNo,"0", ProductID,"",Quantity, UOM, Rate, Amount, Date, "I",0);
                }
            }
            foreach (var args in e.UpdateValues)
            {
                string SrlNo = Convert.ToString(args.NewValues["SrlNo"]);
                string IndentDetailsId = Convert.ToString(args.Keys["PurchaseIndentID"]);
                string ProductDetails = Convert.ToString(args.NewValues["gvColProduct"]);

                bool isDeleted = false;
                foreach (var arg in e.DeleteValues)
                {
                    string DeleteID = Convert.ToString(arg.Keys["PurchaseIndentID"]);
                    if (DeleteID == IndentDetailsId)
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
                        string Description = Convert.ToString(args.NewValues["gvColDiscription"]);
                        string Quantity = Convert.ToString(args.NewValues["gvColQuantity"]);
                        // string UOM = Convert.ToString(ProductDetailsList[3]);
                        string UOM = Convert.ToString(args.NewValues["gvColUOM"]);
                        string Rate = Convert.ToString(args.NewValues["gvColRate"]);
                        // string StockQuantity = Convert.ToString(args.NewValues["gvColValue"]);
                        // string Rate = Convert.ToString(ProductDetailsList[7]);
                        string Amount = (Convert.ToString(args.NewValues["gvColValue"]) != "") ? Convert.ToString(args.NewValues["gvColValue"]) : "0";
                        string Date = Convert.ToString(args.NewValues["ExpectedDeliveryDate"]);
                        string Remarks = Convert.ToString(args.NewValues["Remarks"]);

                        bool Isexists = false;
                        foreach (DataRow drr in Indentdt.Rows)
                        {
                            string OldQuotationID = Convert.ToString(drr["IndentDetailsId"]);

                            if (OldQuotationID == IndentDetailsId)
                            {
                                Isexists = true;
                                drr["SrlNo"] = SrlNo;
                                drr["IndentDetailsId"] = OldQuotationID;
                                drr["ProductID"] = ProductDetails;
                                drr["Description"] = Description;
                                drr["Quantity"] = Quantity;
                                drr["UOM"] = UOM;
                                drr["Rate"] = Rate;
                                drr["ValueInBaseCurrency"] = Amount;
                                drr["ExpectedDeliveryDate"] = Date;
                                drr["Status"] = "U";
                                drr["AvailableStock"] = "0";
                                drr["ProductName"] = ProductName;
                                drr["Remarks"] = Remarks;
                                break;
                            }
                        }

                        if (Isexists == false)
                        {
                            // Indentdt.Rows.Add(SrlNo, IndentDetailsId, ProductID,"",Quantity, UOM, Rate, Amount, Date, "U",0);
                            Indentdt.Rows.Add(SrlNo, IndentDetailsId, ProductDetails, "", Quantity, UOM, Rate, Amount, Date, "U", 0, ProductName, Remarks, null, null, null);
                        }
                    }
                }
            }
            foreach (var args in e.DeleteValues)
            {
                string IndentDetailsID = Convert.ToString(args.Keys["PurchaseIndentID"]);

                for (int i = Indentdt.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = Indentdt.Rows[i];
                    string delQuotationID = Convert.ToString(dr["IndentDetailsID"]);

                    if (delQuotationID == IndentDetailsID)
                        dr.Delete();
                }
                Indentdt.AcceptChanges();

                //if (IndentDetailsID.Contains("~") != true)
                //{
                //    Indentdt.Rows.Add(0, IndentDetailsID, 0, "", 0, 0, 0, 0, DateTime.Now, "D", 0, "","");
                //}
            }
            int j = 1;
            foreach (DataRow dr in Indentdt.Rows)
            {
                string Status = Convert.ToString(dr["Status"]);
                dr["SrlNo"] = j.ToString();

                if (Status != "D")
                {
                    if (Status == "I")
                    {
                        string strID = Convert.ToString(j);//"Q~" +
                        dr["IndentDetailsID"] = strID;
                    }
                    j++;
                }
            }
            Indentdt.AcceptChanges();

            Session["ProjectIndateDetails"] = Indentdt;
            if (IsDeleteFrom != "D")
            {
                string ActionType = "";
                if (hdn_Mode.Value == "Entry")
                {
                    ActionType = "ADD";
                }
                else if (hdn_Mode.Value == "Edit")
                {
                    ActionType = "Edit";
                }

                string strIndentType = "PI";
                string strRequisitionNumber = Convert.ToString(txtVoucherNo.Text.Trim());
                string strIndent_Id = Convert.ToString(hdnEditIndentID.Value);
                string strRequisitionDate = Convert.ToString(tDate.Date);
                string strBranch1 = Convert.ToString(ddlBranch.SelectedValue);
                string strBranch = Convert.ToString(ddlBranch.SelectedItem.Value);
                string strPurpose = Convert.ToString(txtMemoPurpose.Text);
                string strCurrency = Convert.ToString(CmbCurrency.SelectedItem.Value);
                string strExchangeRate = Convert.ToString(txtRate.Text);
                string currency = Convert.ToString(HttpContext.Current.Session["LocalCurrency"]);
                string[] ActCurrency = currency.Split('~');
                int BaseCurrencyId = Convert.ToInt32(ActCurrency[0]);


                string strComponenyType = Convert.ToString(rdl_SaleInvoice.SelectedValue);
                List<object> QuoList = lookup_quotation.GridView.GetSelectedFieldValues("Details_ID");
                foreach (object Quo in QuoList)
                {
                    IndentComponent += "," + Quo;
                }
                IndentComponent = IndentComponent.TrimStart(',');
                string[] eachInvoice = IndentComponent.Split(',');
                if (eachInvoice.Length == 1)
                {
                    IndentComponentDate = Convert.ToString(txt_InvoiceDate.Text);
                }
                else
                {
                    IndentComponentDate = "";
                }

                DataTable tempQuotation = Indentdt.Copy();

                foreach (DataRow dr in tempQuotation.Rows)
                {
                    string Status = Convert.ToString(dr["Status"]);

                    if (Status == "I")
                    {
                        dr["IndentDetailsId"] = "0";

                        string[] list = Convert.ToString(dr["ProductID"]).Split(new string[] { "||@||" }, StringSplitOptions.None);
                        dr["ProductID"] = Convert.ToString(list[0]);
                        dr["UOM"] = Convert.ToString(list[3]);
                        string rate = Convert.ToString(dr["Rate"]);
                        dr["Rate"] = Convert.ToString(Math.Round(Convert.ToDecimal(rate), 2));
                        dr["Description"] = "";
                    }
                    else if (Status == "U" || Status == "")
                    {
                        string[] list = Convert.ToString(dr["ProductID"]).Split(new string[] { "||@||" }, StringSplitOptions.None);
                        dr["ProductID"] = Convert.ToString(list[0]);
                        dr["UOM"] = Convert.ToString(list[3]);
                        string rate = Convert.ToString(dr["Rate"]);
                        dr["Rate"] = Convert.ToString(Math.Round(Convert.ToDecimal(rate), 2));
                        dr["Description"] = "";
                        string IndentDetailsID = Convert.ToString(dr["IndentDetailsId"]);
                        if (IndentDetailsID.Contains("~") == true)
                        {
                            dr["IndentDetailsId"] = "0";
                        }
                    }
                }
                tempQuotation.AcceptChanges();

                string approveStatus = "";
                if (Request.QueryString["status"] != null)
                {
                    approveStatus = Convert.ToString(Request.QueryString["status"]);
                }

                string validate = "";

                if (hdn_Mode.Value == "Entry")
                {
                    validate = checkNMakeJVCode(strRequisitionNumber.Trim(), Convert.ToInt32(CmbScheme.Value));
                }
                DataView dvData = new DataView(tempQuotation);
                dvData.RowFilter = "Status<>'D'";
                DataTable dt_tempQuotation = dvData.ToTable();

                //var duplicateRecords = dt_tempQuotation.AsEnumerable()
                //.GroupBy(r => r["ProductID"]) //coloumn name which has the duplicate values
                //.Where(gr => gr.Count() > 1)
                // .Select(g => g.Key);

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

                if (Keyval_internalId.Value == "Add")
                {

                    if (lookup_quotation.GridView.GetSelectedFieldValues("Details_ID").Count > 0)
                    {
                        if (tempQuotation.Rows.Count > 0)
                        {
                            foreach (DataRow dr in tempQuotation.Rows)
                            {
                                Int64 DetailsId = 0;
                                string ProductID = Convert.ToString(dr["ProductID"]);
                                decimal ProductQuantity = Convert.ToDecimal(dr["Quantity"]);
                                string Status = Convert.ToString(dr["Status"]);
                                //QuoteOrderDetails_id = Convert.ToString(Session["QuoteOrderDetails_id"]);
                                //Int64 i = 0;
                                //for ( i =(Convert.ToInt64(dr["SrlNo"])-1); i <Convert.ToInt64(dr["SrlNo"]); i++)
                                //{
                                //     DetailsId = Convert.ToInt64(QuoteOrderDetails_id.Split(',')[i]);
                                //}
                                if (rdl_SaleInvoice.SelectedValue == "ES")
                                {
                                    DataTable dtq = oDBEngine.GetDataTable("select isnull(BALANCE_QTY,0) TotQty from PMS_EstimateBalance where  ESTIMATE_ID='" + Convert.ToInt64(dr["DetailsId"]) + "' and  PRODUCT_ID='" + Convert.ToInt64(dr["ProductID"]) + "'and  ESTIMATE_DETAILSID='" + Convert.ToInt64(dr["ProdDetailsId"]) + "'");

                                    if (ProductID != "" && dtq.Rows.Count > 0)
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


                if (Keyval_internalId.Value != "Add")
                {

                    if (lookup_quotation.GridView.GetSelectedFieldValues("Details_ID").Count > 0)
                    {
                        if (tempQuotation.Rows.Count > 0)
                        {
                            foreach (DataRow dr in tempQuotation.Rows)
                            {
                                Int64 DetailsId = 0;
                                decimal mainqty = 0, baltoalQty = 0;

                                string ProductID = Convert.ToString(dr["ProductID"]);
                                decimal ProductQuantity = Convert.ToDecimal(dr["Quantity"]);
                                string Status = Convert.ToString(dr["Status"]);

                                if (rdl_SaleInvoice.SelectedValue == "ES")
                                {
                                    DataTable dtmainqty = oDBEngine.GetDataTable("select IndentDetails_Quantity from tbl_trans_IndentDetails where IndentDetails_Id='" + Convert.ToInt64(dr["IndentDetailsId"]) + "'");
                                    DataTable dtq = oDBEngine.GetDataTable("select isnull(BALANCE_QTY,0) TotQty from PMS_EstimateBalance where   ESTIMATE_ID='" + Convert.ToInt64(dr["DetailsId"]) + "' and  PRODUCT_ID='" + Convert.ToInt64(dr["ProductID"]) + "'and  ESTIMATE_DETAILSID='" + Convert.ToInt64(dr["ProdDetailsId"]) + "'");

                                    if (ProductID != "" && dtq.Rows.Count > 0 && dtmainqty.Rows.Count > 0)
                                    {
                                        baltoalQty = (Convert.ToDecimal(dtq.Rows[0]["TotQty"]) + Convert.ToDecimal(dtmainqty.Rows[0]["IndentDetails_Quantity"]));
                                        if (ProductQuantity > Convert.ToDecimal(baltoalQty))
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



                //if (tempQuotation.Rows.Count > 0)
                //{
                //    foreach (DataRow dr in tempQuotation.Rows)
                //    {
                //        string ProductID = Convert.ToString(dr["ProductID"]);
                //        decimal ProductQuantity = Convert.ToDecimal(dr["Quantity"]);
                //        string Status = Convert.ToString(dr["Status"]);
                //        if (Convert.ToInt32(dr["IndentDetailsId"]) != 0)
                //        {
                //            DataTable dtq = oDBEngine.GetDataTable("select ISNULL(TotalQty,0) TotalQty from tbl_trans_RequisitionBalanceMapForPurchaseOrder where  RequisitionId='" + Convert.ToInt32(dr["IndentDetailsId"]) + "' and ProductId='" + ProductID + "'");
                //            if (Status != "D")
                //            {
                //                if (ProductQuantity < Convert.ToDecimal(dtq.Rows[0]["TotalQty"]))
                //                {
                //                    validate = "ExceedQuantity";
                //                    break;
                //                }
                //            }
                //        }
                //    }
                //}

                if (validate == "outrange" || validate == "duplicate" || validate == "nullQuantity" || validate == "ExceedQuantity") // || validate == "duplicateProduct"
                {
                    gridBatch.JSProperties["cpSaveSuccessOrFail"] = validate;
                }
                else
                {
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
                    DataTable dtAddlDesc = (DataTable)Session["InlineRemarks"];

                    //if (Save_Record(strIndentType, strIndent_Id, JVNumStr, strRequisitionDate, strBranch, strPurpose, strCurrency, strExchangeRate, BaseCurrencyId, tempQuotation, ActionType, approveStatus) == false)
                    int id = Save_Record(strIndentType, strIndent_Id, JVNumStr, strRequisitionDate, strBranch, strPurpose, strCurrency, strExchangeRate, BaseCurrencyId, tempQuotation, ActionType, approveStatus, duplicatedt2,
                              ProjId, dtAddlDesc, strComponenyType, IndentComponent, IndentComponentDate);
                    //End of Rev
                    if (id == -10)
                    {
                        gridBatch.JSProperties["cpSaveSuccessOrFail"] = "errorInsert";
                    }
                    else if (id == -12)
                    {
                        gridBatch.JSProperties["cpSaveSuccessOrFail"] = "ProjectError";
                    }

                    else
                    {
                        gridBatch.JSProperties["cpVouvherNo"] = JVNumStr;
                    }

                    if (approveStatus != "" && id != -10 && id != -12)
                    {
                        gridBatch.JSProperties["cpApproverStatus"] = "approve";
                    }
                    e.Handled = true;
                }
            }
            else
            {
                DataView dvData = new DataView(Indentdt);
                dvData.RowFilter = "Status <> 'D'";

                gridBatch.DataSource = GetPurchaseIndent(dvData.ToTable());
                gridBatch.DataBind();
            }
        }

        public DataTable GetProjectEditData(string VendorePayID, String Action)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CustomerPaymentReciptProjectID");
            proc.AddIntegerPara("@Receipt_ID", Convert.ToInt32(VendorePayID));
            proc.AddVarcharPara("@Action", 100, Action);
            dt = proc.GetTable();
            return dt;
        }

        public IEnumerable GetPurchaseIndent(DataTable PurchaseIndentdt)
        {
            List<BratchGridLIST> BratchGridLists = new List<BratchGridLIST>();

            for (int i = 0; i < PurchaseIndentdt.Rows.Count; i++)
            {
                BratchGridLIST BratchGrid_LIST = new BratchGridLIST();
                BratchGrid_LIST.SrlNo = Convert.ToString(PurchaseIndentdt.Rows[i]["SrlNo"]);
                BratchGrid_LIST.PurchaseIndentID = Convert.ToString(PurchaseIndentdt.Rows[i]["IndentDetailsId"]);
                BratchGrid_LIST.gvColProduct = Convert.ToString(PurchaseIndentdt.Rows[i]["ProductID"]);
                BratchGrid_LIST.gvColDiscription = Convert.ToString(PurchaseIndentdt.Rows[i]["Description"]);
                BratchGrid_LIST.gvColQuantity = Convert.ToString(PurchaseIndentdt.Rows[i]["Quantity"]);
                BratchGrid_LIST.gvColUOM = Convert.ToString(PurchaseIndentdt.Rows[i]["UOM"]);
                BratchGrid_LIST.gvColRate = Convert.ToString(PurchaseIndentdt.Rows[i]["Rate"]);
                BratchGrid_LIST.gvColIndentDetailsId = Convert.ToString(PurchaseIndentdt.Rows[i]["IndentDetailsId"]);
                BratchGrid_LIST.gvColValue = Convert.ToString(PurchaseIndentdt.Rows[i]["ValueInBaseCurrency"]);
                string ExpectedDeliveryDate = Convert.ToString(PurchaseIndentdt.Rows[i]["ExpectedDeliveryDate"]);
                if (!String.IsNullOrEmpty(ExpectedDeliveryDate))
                {
                    BratchGrid_LIST.ExpectedDeliveryDate = Convert.ToDateTime(PurchaseIndentdt.Rows[i]["ExpectedDeliveryDate"]);//8
                }
                else
                {
                    BratchGrid_LIST.ExpectedDeliveryDate = null;
                }
                BratchGrid_LIST.Status = Convert.ToString(PurchaseIndentdt.Rows[i]["Status"]);
                BratchGrid_LIST.AvailableStock = Convert.ToString(PurchaseIndentdt.Rows[i]["AvailableStock"]);
                BratchGrid_LIST.ProductName = Convert.ToString(PurchaseIndentdt.Rows[i]["ProductName"]);//12
                BratchGrid_LIST.Remarks = Convert.ToString(PurchaseIndentdt.Rows[i]["Remarks"]);
                //chinmoy added for balqty start 05-12-2019

                //BratchGrid_LIST.BalQty = Convert.ToString(PurchaseIndentdt.Rows[i]["BalQty"]);
                //End

                BratchGridLists.Add(BratchGrid_LIST);
            }

            return BratchGridLists;
        }

        protected string checkNMakeJVCode(string manual_str, int sel_schema_Id)
        {
            //oDbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            oDbEngine = new BusinessLogicLayer.DBEngine();



            DataTable dtSchema = new DataTable();
            DataTable dtC = new DataTable();
            string prefCompCode = string.Empty, sufxCompCode = string.Empty, startNo, paddedStr, sqlQuery = string.Empty;
            int EmpCode, prefLen, sufxLen, paddCounter;

            if (sel_schema_Id > 0)
            {
                dtSchema = oDbEngine.GetDataTable("tbl_master_idschema", "prefix, suffix, digit, startno, schema_type", "id=" + sel_schema_Id);
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

                    sqlQuery = "SELECT max(tjv.Indent_RequisitionNumber) FROM tbl_trans_Indent tjv WHERE dbo.RegexMatch('";
                    if (prefLen > 0)
                        sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                    else if (scheme_type == 2)
                        sqlQuery += "^";
                    sqlQuery += "[0-9]{" + paddCounter + "}";
                    if (sufxLen > 0)
                        sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                    // sqlQuery += "?$', LTRIM(RTRIM(tjv.Indent_RequisitionNumber))) = 1";
                    sqlQuery += "?$', LTRIM(RTRIM(tjv.Indent_RequisitionNumber))) = 1 and Indent_RequisitionNumber like '" + prefCompCode + "%'";
                    if (scheme_type == 2)
                        sqlQuery += " AND CONVERT(DATE, Indent_CreatedDate) = CONVERT(DATE, GETDATE())";
                    dtC = oDbEngine.GetDataTable(sqlQuery);

                    if (dtC.Rows[0][0].ToString() == "")
                    {
                        sqlQuery = "SELECT max(tjv.Indent_RequisitionNumber) FROM tbl_trans_Indent tjv WHERE dbo.RegexMatch('";
                        if (prefLen > 0)
                            sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                        else if (scheme_type == 2)
                            sqlQuery += "^";
                        sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                        if (sufxLen > 0)
                            sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                        //sqlQuery += "?$', LTRIM(RTRIM(tjv.Indent_RequisitionNumber))) = 1";
                        sqlQuery += "?$', LTRIM(RTRIM(tjv.Indent_RequisitionNumber))) = 1 and Indent_RequisitionNumber like '" + prefCompCode + "%'";
                        if (scheme_type == 2)
                            sqlQuery += " AND CONVERT(DATE, Indent_CreatedDate) = CONVERT(DATE, GETDATE())";
                        dtC = oDbEngine.GetDataTable(sqlQuery);
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
                            JVNumStr = prefCompCode + paddedStr + sufxCompCode;
                            return "ok";
                        }
                    }
                    else
                    {
                        JVNumStr = startNo.PadLeft(paddCounter, '0');
                        JVNumStr = prefCompCode + paddedStr + sufxCompCode;
                        return "ok";
                    }
                }
                else
                {
                    sqlQuery = "SELECT Indent_RequisitionNumber FROM tbl_trans_Indent WHERE Indent_RequisitionNumber LIKE '" + manual_str.Trim() + "'";
                    dtC = oDbEngine.GetDataTable(sqlQuery);
                    // duplicate manual entry check
                    if (dtC.Rows.Count > 0 && dtC.Rows[0][0].ToString().Trim() != "")
                    {
                        return "duplicate";
                    }

                    JVNumStr = manual_str.Trim();
                    return "ok";
                }
            }
            else
            {
                return "noid";
            }
        }

        public int Save_Record(string strIndentType, string strIndent_Id, string strRequisitionNumber, string strRequisitionDate, string strBranch, string strPurpose, string strCurrency,
            string strExchangeRate, int BaseCurrencyId,
            DataTable Indentdt, string ActionType, string approveStatus, DataTable PackingDetailsdt, Int64 ProjId, DataTable dtAddlDesc, String strComponenyType,
            String strIndentComponent, String strIndentComponentDate)
        {
            try
            {
                gridBatch.JSProperties["cpExitNew"] = null;
                gridBatch.JSProperties["cpVouvherNo"] = null;

                DataSet dsInst = new DataSet();

                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

                SqlCommand cmd = new SqlCommand("prc_ProjectPurchaseIndent", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", ActionType);
                cmd.Parameters.AddWithValue("@Indent_EditId", strIndent_Id);
                cmd.Parameters.AddWithValue("@IndentType", strIndentType);
                cmd.Parameters.AddWithValue("@Indent_RequisitionNumber", strRequisitionNumber);
                cmd.Parameters.AddWithValue("@Indent_RequisitionDate", strRequisitionDate);
                cmd.Parameters.AddWithValue("@Indent_BranchIdFor", strBranch);
                cmd.Parameters.AddWithValue("@Indent_BranchIdTo", "0");
                cmd.Parameters.AddWithValue("@Indent_Purpose", strPurpose);
                cmd.Parameters.AddWithValue("@Indent_baseCurrencyId", BaseCurrencyId);
                cmd.Parameters.AddWithValue("@Indent_CurrencyId", strCurrency);
                cmd.Parameters.AddWithValue("@Indent_ExchangeRtae", strExchangeRate);
                cmd.Parameters.AddWithValue("@Indent_Company", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@Indent_FinYear", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@Indent_CreatedBy", Convert.ToString(Session["userid"]));

                cmd.Parameters.AddWithValue("@PurchaseIndentDetails", Indentdt);
                cmd.Parameters.AddWithValue("@udt_Addldesc", dtAddlDesc);
                cmd.Parameters.AddWithValue("@Project_Id", ProjId);

                cmd.Parameters.AddWithValue("@ComponenyType", strComponenyType);
                cmd.Parameters.AddWithValue("@IndentComponent", strIndentComponent);
                cmd.Parameters.AddWithValue("@IndentComponentDate", strIndentComponentDate);
                cmd.Parameters.AddWithValue("@PackingDetails", PackingDetailsdt);

                SqlParameter output = new SqlParameter("@ReturnValueID", SqlDbType.Int);
                output.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(output);

                cmd.Parameters.AddWithValue("@approveStatus", approveStatus);
                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                int strIsComplete = Convert.ToInt32(cmd.Parameters["@ReturnValueID"].Value.ToString());

                cmd.Dispose();
                con.Dispose();
                gridBatch.JSProperties["cpExitNew"] = "YES";


                //Udf Add mode
                DataTable udfTable = (DataTable)Session["UdfDataOnAdd"];
                if (udfTable != null)
                {
                    Session["UdfDataOnAdd"] = reCat.insertRemarksCategoryAddMode("PI", "PurchaseIndent" + Convert.ToString(output.Value), udfTable, Convert.ToString(Session["userid"]));
                }
                return strIsComplete;
            }
            catch (Exception ex)
            {
                return 0;
            }
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
                status = objMShortNameCheckingBL.CheckUnique(Convert.ToString(VoucherNo).Trim(), "0", "PurchaseIndent_Check");
            }
            return status;
        }


        protected void gridBatch_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string command = e.Parameters.Split('~')[0];


            if (command == "Edit" || command == "View")
            {

                int RowIndex = Convert.ToInt32(e.Parameters.Split('~')[1]);
                //Rev Debashis
                //string Indent_Id = Grid_PurchaseIndent.GetRowValues(RowIndex, "Indent_Id").ToString();
                string Indent_Id = "";
                if (Request.QueryString.AllKeys.Contains("IsTagged"))
                {
                    Indent_Id = Convert.ToString(RowIndex);
                }
                else
                {
                    Indent_Id = Grid_PurchaseIndent.GetRowValues(RowIndex, "Indent_Id").ToString();  //Convert.ToString(RowIndex);
                }
                //End of Rev Debashis
                // ViewState["Indent_Id"] = Indent_Id;
                Session["Indent_Id"] = Indent_Id;
                Keyval_internalId.Value = "PurchaseIndent" + Indent_Id;
                string Indent_TagFrom = "";
                string Indent_TagID = "";
                DataTable PurchaseIndentEditdt = GetPurchaseIndentEditData();
                if (PurchaseIndentEditdt != null && PurchaseIndentEditdt.Rows.Count > 0)
                {
                    string Indent_RequisitionNumber = Convert.ToString(PurchaseIndentEditdt.Rows[0]["Indent_RequisitionNumber"]);//0
                    string Indent_RequisitionDate = Convert.ToString(PurchaseIndentEditdt.Rows[0]["Indent_RequisitionDate"]);//1
                    string Indent_BranchIdFor = Convert.ToString(PurchaseIndentEditdt.Rows[0]["Indent_BranchIdFor"]);//2
                    ddlBranch.SelectedValue = Indent_BranchIdFor;
                    string Indent_Purpose = Convert.ToString(PurchaseIndentEditdt.Rows[0]["Indent_Purpose"]);//3
                    string Indent_CurrencyId = Convert.ToString(PurchaseIndentEditdt.Rows[0]["Indent_CurrencyId"]);//4
                    string Indent_ExchangeRtae = Convert.ToString(PurchaseIndentEditdt.Rows[0]["Indent_ExchangeRtae"]);//5
                    string Indent_ProjID = Convert.ToString(PurchaseIndentEditdt.Rows[0]["Proj_Id"]);//6

                    Indent_TagFrom = Convert.ToString(PurchaseIndentEditdt.Rows[0]["IndentCreatedFromDoc"]);
                    Indent_TagID = Convert.ToString(PurchaseIndentEditdt.Rows[0]["IndentCreatedFromDoc_Ids"]);

                    string Estimate_isLastEntry = Convert.ToString(PurchaseIndentEditdt.Rows[0]["Estimate_isLastEntry"]);

                    Session["Indent_TagID"] = Indent_TagID;

                    txtVoucherNo.Text = Indent_RequisitionNumber.Trim();

                    if (Indent_TagFrom != "")
                    {
                        //rdl_SaleInvoice.Items.FindByValue(Indent_TagFrom).Selected = true;
                        // rdl_SaleInvoice.SelectedValue = Indent_TagFrom;
                        // lookup_quotation.ClientEnabled = false;
                    }

                    if (Indent_TagID != "")
                    {
                        lookup_Project.ClientEnabled = false;
                    }



                    //string ProjectSelectEntryModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");
                    //lookup_Project.GridView.Selection.SelectRowByKey(Convert.ToInt64(PurchaseIndentEditdt.Rows[0]["Proj_Id"]));

                    gridBatch.JSProperties["cpEdit"] = Indent_RequisitionNumber + "~" + Indent_RequisitionDate + "~" + Indent_BranchIdFor + "~" + Indent_Purpose + "~" + Indent_CurrencyId
                        + "~" + Indent_ExchangeRtae + "~" + Indent_Id + "~" + Indent_ProjID + "~" + Indent_TagFrom + "~" + Indent_TagID + "~" + Estimate_isLastEntry;
                    gridBatch.JSProperties["cpView"] = (command.ToUpper() == "VIEW") ? "1" : "0";

                    //if (!String.IsNullOrEmpty(Indent_TagID))
                    //{
                    //    string[] eachQuo = Indent_TagID.Split(',');
                    //    if (eachQuo.Length > 1)//More tha one quotation
                    //    {
                    //        BindLookUp(Indent_RequisitionDate, Indent_TagFrom, Indent_BranchIdFor, Indent_TagID);
                    //        foreach (string val in eachQuo)
                    //        {
                    //            lookup_quotation.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));
                    //        }
                    //        lookup_quotation.ClientEnabled = false;
                    //    }
                    //    else if (eachQuo.Length == 1)//Single Quotation
                    //    {
                    //        BindLookUp(Indent_RequisitionDate, Indent_TagFrom, Indent_BranchIdFor, Indent_TagID);
                    //        foreach (string val in eachQuo)
                    //        {
                    //            lookup_quotation.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));
                    //        }
                    //        lookup_quotation.ClientEnabled = false;
                    //    }
                    //    else//No Quotation selected
                    //    {
                    //        BindLookUp(Indent_RequisitionDate, Indent_TagFrom, Indent_BranchIdFor, Indent_TagID);
                    //    }
                    //}
                }

                gridBatch.DataSource = BindBratchGrid();
                gridBatch.DataBind();

                Session["ProjectIndateDetails"] = GetPurchaseIndentData().Tables[0];
                Session["InlineRemarks"] = GetPurchaseIndentData().Tables[1];


                string result = "";
                result = IsExistsDocumentInERPDocApproveStatus(Indent_Id);
                if (result != "")
                {
                    if (result == "A")
                    {
                        gridBatch.JSProperties["cpApproval"] = "A";
                    }
                    else if (result == "R")
                    {
                        gridBatch.JSProperties["cpApproval"] = "R";
                    }
                }
                else
                {
                    gridBatch.JSProperties["cpApproval"] = null;
                }


                if (IsPITransactionExist(Indent_Id))
                {
                    gridBatch.JSProperties["cpBtnVisible"] = "false";
                }

                //DataTable Quotationdt = (DataTable)Session["ProjectIndateDetails"];
                // gridBatch.DataSource = GetPurchaseIndent(Quotationdt);
                // gridBatch.DataBind();

                rdl_SaleInvoice.Enabled = false;
                rdl_SaleInvoice.Attributes.Add("style", "color:#999;");


                string strAction = "";


                if (Indent_TagFrom == "ES")
                {
                    strAction = "GetEstimateProducts";
                    grid_Products.Columns["ComponentNumber"].Caption = "Estimate No";
                }
                else if (Indent_TagFrom == "BQ")
                {
                    strAction = "GetBOQProducts";
                    grid_Products.Columns["ComponentNumber"].Caption = "BOQ No";
                }
                else if (Indent_TagFrom == "SC")
                {
                    strAction = "GetChallanProducts";
                    grid_Products.Columns["ComponentNumber"].Caption = "Challan No";
                }

                string strInvoiceID = Convert.ToString(Session["Indent_Id"]);
                DataTable dtDetails = GetComponentProductList(strAction, Indent_TagID, strInvoiceID);

                grid_Products.DataSource = dtDetails;
                grid_Products.DataBind();

                DataTable Transaction_dt = (DataTable)Session["ProductIndentDetails"];

                for (int i = 0; i < grid_Products.VisibleRowCount; i++)
                {
                    string ESTIMATE_DETAILSID = Convert.ToString(grid_Products.GetRowValues(i, "ESTIMATE_DETAILSID"));
                    string ProductList = Convert.ToString(grid_Products.GetRowValues(i, "ProductID"));

                    string[] list = ProductList.Split(new string[] { "||@||" }, StringSplitOptions.None);
                    string ProductID = Convert.ToString(list[0]) + "||@||%";

                    var Checkdt = Transaction_dt.Select("ProdDetailsId='" + ESTIMATE_DETAILSID + "' AND ProductID LIKE '" + ProductID + "'");
                    if (Checkdt.Length > 0)
                    {
                        grid_Products.Selection.SelectRow(i);
                    }
                }

            }

            if (command == "ApprovalEdit")
            {
                int Indent_Id = Convert.ToInt32(e.Parameters.Split('~')[1]);

                //string Indent_Id = Grid_PurchaseIndent.GetRowValues(RowIndex, "Indent_Id").ToString();
                // ViewState["Indent_Id"] = Indent_Id;
                Session["Indent_Id"] = Indent_Id;

                DataTable PurchaseIndentEditdt = GetPurchaseIndentEditData();
                if (PurchaseIndentEditdt != null && PurchaseIndentEditdt.Rows.Count > 0)
                {
                    string Indent_RequisitionNumber = Convert.ToString(PurchaseIndentEditdt.Rows[0]["Indent_RequisitionNumber"]);//0
                    string Indent_RequisitionDate = Convert.ToString(PurchaseIndentEditdt.Rows[0]["Indent_RequisitionDate"]);//1
                    string Indent_BranchIdFor = Convert.ToString(PurchaseIndentEditdt.Rows[0]["Indent_BranchIdFor"]);//2
                    ddlBranch.SelectedValue = Indent_BranchIdFor;
                    string Indent_Purpose = Convert.ToString(PurchaseIndentEditdt.Rows[0]["Indent_Purpose"]);//3
                    string Indent_CurrencyId = Convert.ToString(PurchaseIndentEditdt.Rows[0]["Indent_CurrencyId"]);//4
                    string Indent_ExchangeRtae = Convert.ToString(PurchaseIndentEditdt.Rows[0]["Indent_ExchangeRtae"]);//5

                    txtVoucherNo.Text = Indent_RequisitionNumber.Trim();
                    gridBatch.JSProperties["cpEdit"] = Indent_RequisitionNumber + "~" + Indent_RequisitionDate + "~" + Indent_BranchIdFor + "~" + Indent_Purpose + "~" + Indent_CurrencyId
                        + "~" + Indent_ExchangeRtae + "~" + Indent_Id;

                }

                gridBatch.DataSource = BindBratchGrid();
                gridBatch.DataBind();

                Session["ProjectIndateDetails"] = GetPurchaseIndentData().Tables[0];
                Session["InlineRemarks"] = GetPurchaseIndentData().Tables[1];

                //DataTable Quotationdt = (DataTable)Session["ProjectIndateDetails"];
                // gridBatch.DataSource = GetPurchaseIndent(Quotationdt);
                // gridBatch.DataBind();
            }

            if (command == "Display")
            {
                string IsDeleteFrom = Convert.ToString(hdfIsDelete.Value);
                if (IsDeleteFrom == "D")
                {
                    DataTable Quotationdt = (DataTable)Session["ProjectIndateDetails"];
                    gridBatch.DataSource = GetPurchaseIndent(Quotationdt);
                    gridBatch.DataBind();

                    gridBatch.JSProperties["cpAddNewRow"] = "AddNewRow";
                }
            }

            #region Tag Product

            if (command == "BindGridOnQuotation")
            {
                // string command = e.Parameters.ToString();
                string State = Convert.ToString(e.Parameters.Split('~')[1]);
                string strType = Convert.ToString(rdl_SaleInvoice.SelectedValue);
                string ComponentDetailsIDs = string.Empty;
                string strAction = "";
                string strTaxCountAction = "";

                for (int i = 0; i < grid_Products.GetSelectedFieldValues("ComponentDetailsID").Count; i++)
                {
                    ComponentDetailsIDs += "," + Convert.ToString(grid_Products.GetSelectedFieldValues("ComponentDetailsID")[i]);
                }
                ComponentDetailsIDs = ComponentDetailsIDs.TrimStart(',');


                if (strType == "ES")
                {
                    strAction = "GetGridEstimateProducts";
                    strTaxCountAction = "GetSeletedQuotationTaxCount";
                }
                else if (strType == "BQ")
                {
                    strAction = "GetGridBOQProducts";
                    strTaxCountAction = "GetSeletedOrderTaxCount";
                }
                else if (strType == "SC")
                {
                    strAction = "GetSeletedChallanProducts";
                    strTaxCountAction = "GetSeletedChallanTaxCount";
                }

                string strInvoiceID = Convert.ToString(Session["SI_InvoiceID"]);

                //if (objSalesInvoiceBL.GetSelectedComponentProductList(strTaxCountAction, ComponentDetailsIDs, strInvoiceID).Rows.Count <= 2)
                //{
                DataSet dt_QuotationDetails = GetSelectedComponentProductList(strAction, ComponentDetailsIDs, strInvoiceID);
                Session["SI_QuotationDetails"] = dt_QuotationDetails.Tables[0];

                Session["ProjectIndateDetails"] = dt_QuotationDetails.Tables[0];
                Session["InlineRemarks"] = dt_QuotationDetails.Tables[1];

                gridBatch.DataSource = GetQuotation(dt_QuotationDetails.Tables[0]);
                gridBatch.DataBind();

                //Session["SI_WarehouseData"] = GetTaggingWarehouseData(ComponentDetailsIDs, strType);
                //Session["SI_FinalTaxRecord"] = GetComponentEditedTaxData(ComponentDetailsIDs, strType);

                //}
                //else
                //{
                //    gridBatch.JSProperties["cpSaveSuccessOrFail"] = "MultiTax";
                //}
            }

            #endregion

        }

        public IEnumerable GetQuotation(DataTable dt)
        {
            DataSet DsOnLoad = new DataSet();
            DataTable tempdt = new DataTable();
            DBEngine objEngine = new DBEngine();
            List<BratchGridLIST> BratchGridLists = new List<BratchGridLIST>();
            DataTable PurchaseIndentdt = dt;
            // Session["InlineRemarks"] = GetPurchaseIndentData().Tables[1];
            for (int i = 0; i < PurchaseIndentdt.Rows.Count; i++)
            {
                BratchGridLIST BratchGrid_LIST = new BratchGridLIST();
                BratchGrid_LIST.SrlNo = Convert.ToString(PurchaseIndentdt.Rows[i]["SrlNo"]);
                BratchGrid_LIST.PurchaseIndentID = Convert.ToString(PurchaseIndentdt.Rows[i]["IndentDetailsId"]);
                //Rev 1.0 Subhra 01.03.2019
                BratchGrid_LIST.gvColIndentDetailsId = Convert.ToString(PurchaseIndentdt.Rows[i]["IndentDetailsId"]);
                //End of Rev
                BratchGrid_LIST.gvColProduct = Convert.ToString(PurchaseIndentdt.Rows[i]["ProductID"]);
                BratchGrid_LIST.gvColDiscription = Convert.ToString(PurchaseIndentdt.Rows[i]["Description"]);
                BratchGrid_LIST.gvColQuantity = Convert.ToString(PurchaseIndentdt.Rows[i]["Quantity"]);
                BratchGrid_LIST.gvColUOM = Convert.ToString(PurchaseIndentdt.Rows[i]["UOM"]);
                BratchGrid_LIST.gvColRate = Convert.ToString(PurchaseIndentdt.Rows[i]["Rate"]);

                BratchGrid_LIST.gvColValue = Convert.ToString(PurchaseIndentdt.Rows[i]["ValueInBaseCurrency"]);
                string ExpectedDeliveryDate = Convert.ToString(PurchaseIndentdt.Rows[i]["ExpectedDeliveryDate"]);
                if (!String.IsNullOrEmpty(ExpectedDeliveryDate))
                {
                    BratchGrid_LIST.ExpectedDeliveryDate = Convert.ToDateTime(PurchaseIndentdt.Rows[i]["ExpectedDeliveryDate"]);
                }
                else
                {
                    BratchGrid_LIST.ExpectedDeliveryDate = null;
                }
                BratchGrid_LIST.Status = Convert.ToString(PurchaseIndentdt.Rows[i]["Status"]);
                BratchGrid_LIST.AvailableStock = Convert.ToString(PurchaseIndentdt.Rows[i]["AvailableStock"]);
                BratchGrid_LIST.ProductName = Convert.ToString(PurchaseIndentdt.Rows[i]["ProductName"]);
                BratchGrid_LIST.Remarks = Convert.ToString(PurchaseIndentdt.Rows[i]["Remarks"]);

                BratchGrid_LIST.gvProdDetailsId = Convert.ToString(PurchaseIndentdt.Rows[i]["ProdDetailsId"]);
                BratchGrid_LIST.gvDetailsId = Convert.ToString(PurchaseIndentdt.Rows[i]["DetailsId"]);
                BratchGrid_LIST.gvPRODUCTFROM = Convert.ToString(PurchaseIndentdt.Rows[i]["PRODUCTFROM"]);
                BratchGridLists.Add(BratchGrid_LIST);
            }
            return BratchGridLists;
        }

        private bool IsPITransactionExist(string PIid)
        {
            bool IsExist = false;
            if (PIid != "" && Convert.ToString(PIid).Trim() != "")
            {
                DataTable dt = new DataTable();
                dt = objPurchaseIndentBL.CheckProjectPITraanaction(PIid);
                if (dt.Rows.Count > 0 && Convert.ToString(dt.Rows[0]["isexist"]) != "0")
                {
                    IsExist = true;
                }
            }

            return IsExist;
        }

        protected void Grid_PurchaseIndent_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string command = e.Parameters.Split('~')[0];
            Grid_PurchaseIndent.JSProperties["cpDelete"] = null;
            string PurIndentID = null;
            if (Convert.ToString(e.Parameters).Contains("~"))
            {
                if (Convert.ToString(e.Parameters).Split('~')[1] != "")
                {
                    //PurIndentID = Convert.ToString(e.Parameters).Split('~')[1];
                    int RowIndex = Convert.ToInt32(e.Parameters.Split('~')[1]);
                    PurIndentID = Grid_PurchaseIndent.GetRowValues(RowIndex, "Indent_Id").ToString();
                }
            }
            if (command == "Delete")
            {
                if (!IsPITransactionExist(PurIndentID))
                {
                    int RowIndex = Convert.ToInt32(e.Parameters.Split('~')[1]);
                    string Indent_Id = Grid_PurchaseIndent.GetRowValues(RowIndex, "Indent_Id").ToString();
                    // ViewState["Indent_Id"] = Indent_Id;
                    //Session["Indent_Id"] = Indent_Id;
                    int val = GetPurchaseIndentDeleteData(Indent_Id);
                    if (val == 1)
                    {
                        Grid_PurchaseIndent.JSProperties["cpDelete"] = "Succesfully Deleted";
                    }
                }
                else
                {

                    Grid_PurchaseIndent.JSProperties["cpDelete"] = "Transaction exist. Cannot Delete.";
                }

            }
            //FillGrid();
        }

        public int GetPurchaseIndentDeleteData(string Indent_Id)
        {
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseIndentDetailsList");
            proc.AddVarcharPara("@Action", 500, "PurchaseIndentDeleteDetails");
            // Int32 ID = Convert.ToInt32(ViewState["Indent_Id"]);
            //Int32 ID = Convert.ToInt32(Session["Indent_Id"]);
            Int32 ID = Convert.ToInt32(Indent_Id);
            proc.AddIntegerPara("@Indent_Id", ID);
            proc.AddVarcharPara("@ReturnValue", 200, "0", QueryParameterDirection.Output);
            proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;
        }

        public IEnumerable BindBratchGrid()
        {
            DataSet DsOnLoad = new DataSet();
            DataTable tempdt = new DataTable();
            DBEngine objEngine = new DBEngine();
            List<BratchGridLIST> BratchGridLists = new List<BratchGridLIST>();
            DataTable PurchaseIndentdt = GetPurchaseIndentData().Tables[0];
            Session["ProductIndentDetails"] = PurchaseIndentdt;
            Session["InlineRemarks"] = GetPurchaseIndentData().Tables[1];
            for (int i = 0; i < PurchaseIndentdt.Rows.Count; i++)
            {
                BratchGridLIST BratchGrid_LIST = new BratchGridLIST();
                BratchGrid_LIST.SrlNo = Convert.ToString(PurchaseIndentdt.Rows[i]["SrlNo"]);//1
                BratchGrid_LIST.PurchaseIndentID = Convert.ToString(PurchaseIndentdt.Rows[i]["IndentDetailsId"]);//2
                //Rev 1.0 Subhra 01.03.2019
                BratchGrid_LIST.gvColIndentDetailsId = Convert.ToString(PurchaseIndentdt.Rows[i]["IndentDetailsId"]);//2
                //End of Rev
                BratchGrid_LIST.gvColProduct = Convert.ToString(PurchaseIndentdt.Rows[i]["ProductID"]);//3
                BratchGrid_LIST.gvColDiscription = Convert.ToString(PurchaseIndentdt.Rows[i]["Description"]);//4
                BratchGrid_LIST.gvColQuantity = Convert.ToString(PurchaseIndentdt.Rows[i]["Quantity"]);//5
                BratchGrid_LIST.gvColUOM = Convert.ToString(PurchaseIndentdt.Rows[i]["UOM"]);//6
                BratchGrid_LIST.gvColRate = Convert.ToString(PurchaseIndentdt.Rows[i]["Rate"]);//7

                BratchGrid_LIST.gvColValue = Convert.ToString(PurchaseIndentdt.Rows[i]["ValueInBaseCurrency"]);//9
                string ExpectedDeliveryDate = Convert.ToString(PurchaseIndentdt.Rows[i]["ExpectedDeliveryDate"]);
                if (!String.IsNullOrEmpty(ExpectedDeliveryDate))
                {
                    BratchGrid_LIST.ExpectedDeliveryDate = Convert.ToDateTime(PurchaseIndentdt.Rows[i]["ExpectedDeliveryDate"]);//8
                }
                else
                {
                    BratchGrid_LIST.ExpectedDeliveryDate = null;
                }
                BratchGrid_LIST.Status = Convert.ToString(PurchaseIndentdt.Rows[i]["Status"]);
                BratchGrid_LIST.AvailableStock = Convert.ToString(PurchaseIndentdt.Rows[i]["AvailableStock"]);//11
                BratchGrid_LIST.ProductName = Convert.ToString(PurchaseIndentdt.Rows[i]["ProductName"]);//12
                BratchGrid_LIST.Remarks = Convert.ToString(PurchaseIndentdt.Rows[i]["Remarks"]);
                BratchGridLists.Add(BratchGrid_LIST);
            }
            return BratchGridLists;
        }

        public DataSet GetPurchaseIndentData()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseIndentDetailsList");
            proc.AddVarcharPara("@Action", 500, "BindBratchGridPurChaseIndent");
            //proc.AddIntegerPara("@Indent_Id", Convert.ToInt32(ViewState["Indent_Id"]));
            proc.AddIntegerPara("@Indent_Id", Convert.ToInt32(Session["Indent_Id"]));
            ds = proc.GetDataSet();
            return ds;
        }

        public DataTable GetPurchaseIndentEditData()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseIndentDetailsList");
            proc.AddVarcharPara("@Action", 500, "PurchaseIndentEditDetails");
            //proc.AddIntegerPara("@Indent_Id", Convert.ToInt32(ViewState["Indent_Id"]));
            proc.AddIntegerPara("@Indent_Id", Convert.ToInt32(Session["Indent_Id"]));
            //proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            //proc.AddVarcharPara("@branchId", 500, Convert.ToString(Session["userbranchID"]));
            //proc.AddVarcharPara("@companyId", 500, Convert.ToString(Session["LastCompany"]));
            dt = proc.GetTable();
            return dt;
        }

        protected void Grid_PurchaseIndent_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
        {
            if (!rights.CanDelete)
            {
                if (e.ButtonID == "CustomBtnDelete")
                {
                    e.Visible = DevExpress.Utils.DefaultBoolean.False;
                }
            }


            if (!rights.CanEdit)
            {
                if (e.ButtonID == "CustomBtnEdit")
                {
                    e.Visible = DevExpress.Utils.DefaultBoolean.False;
                }
            }
            if (!rights.CanView)
            {
                if (e.ButtonID == "CustomBtnView")
                {
                    e.Visible = DevExpress.Utils.DefaultBoolean.False;
                }
            }
            if (!rights.CanPrint)
            {
                if (e.ButtonID == "CustomBtnPrint")
                {
                    e.Visible = DevExpress.Utils.DefaultBoolean.False;
                }
            }
        }

        [WebMethod]
        public static String getAvilableStock(string ProductID, string LastFinYear, string Campany_ID, string BranchFor)
        {


            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();



            //            string query = @"select  (Stock_Open+Stock_ID)-(ISNULL(Stock_Out,0))as Available_stock,
            //            (select UOM_Name from Master_UOM where UOM_ID=Stock_QuantityUnit)as UOM_Name
            //            from Trans_Stock where Stock_ProductID='" + ProductID + "' and Stock_Company='" + Campany_ID + "' and Stock_FinYear='" + LastFinYear + "'";
            //            DataTable dt = oDBEngine.GetDataTable(query);
            string Available_Stock = "", UOM_Name = "";

            DataTable dt2 = oDBEngine.GetDataTable("Select dbo.fn_CheckAvailableQuotation(" + BranchFor + ",'" + Convert.ToString(Campany_ID) + "','" + Convert.ToString(LastFinYear) + "'," + ProductID + ") as branchopenstock");

            if (dt2.Rows.Count > 0)
            {
                Available_Stock = Convert.ToString(Math.Round(Convert.ToDecimal(dt2.Rows[0]["branchopenstock"]), 2));
                return Available_Stock;
            }
            else
            {
                Available_Stock = "0.00";
                return Available_Stock;
            }

            //            if (dt != null && dt.Rows.Count > 0)
            //            {
            //                Available_Stock = Convert.ToString(dt.Rows[0]["Available_stock"]);
            //                UOM_Name = Convert.ToString(dt.Rows[0]["UOM_Name"]);
            //            }
            //            return Available_Stock + " " + UOM_Name;
        }

        #region Sandip Section For Approval Section Start

        #region Approval Waiting or Pending User Level Wise Section Start

        public void PopulateERPDocApprovalPendingListByUserLevel() // Checked and Modified By Sandip
        {
            DataTable dtdata = new DataTable();
            if (Session["userid"] != null)
            {
                if (Session["userbranchID"] != null)
                {
                    int userid = 0;
                    userid = Convert.ToInt32(Session["userid"]);

                    dtdata = objERPDocPendingApproval.PopulateERPDocApprovalPendingListByUserLevel(userid, "PI");
                    if (dtdata != null && dtdata.Rows.Count > 0)
                    {
                        gridPendingApproval.DataSource = dtdata;
                        gridPendingApproval.DataBind();
                        Session["INPendingApproval"] = dtdata;  // Commented For Temporary Purpose
                    }
                    else
                    {
                        gridPendingApproval.DataSource = null;
                        gridPendingApproval.DataBind();
                    }
                }
            }
        }

        public void PopulateApprovalPendingCountByUserLevel()  // Checked and Modified By Sandip 
        {
            int userid = 0;
            if (Session["userid"] != null)
            {
                if (Session["userbranchID"] != null)
                {

                    userid = Convert.ToInt32(Session["userid"]);
                }
            }
            DataTable dtdata = new DataTable();
            dtdata = objERPDocPendingApproval.PopulateERPDocApprovalPendingCountByUserLevel(userid, "PI");
            if (dtdata != null && dtdata.Rows.Count > 0)
            {
                lblWaiting.Text = "(" + Convert.ToString(dtdata.Rows[0]["ID"]) + ")";
            }
            else
            {
                lblWaiting.Text = "";
            }
        }


        protected void gridPendingApproval_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e) // Checked and Modified By Sandip
        {
            gridPendingApproval.JSProperties["cpinsert"] = null;
            gridPendingApproval.JSProperties["cpEdit"] = null;
            gridPendingApproval.JSProperties["cpUpdate"] = null;
            gridPendingApproval.JSProperties["cpDelete"] = null;
            gridPendingApproval.JSProperties["cpExists"] = null;
            gridPendingApproval.JSProperties["cpUpdateValid"] = null;
            int userid = 0;
            if (Session["userid"] != null)
            {
                Session.Remove("INPendingApproval");
                userid = Convert.ToInt32(Session["userid"]);
                PopulateERPDocApprovalPendingListByUserLevel();
                gridPendingApproval.JSProperties["cpEdit"] = "F";
                Session.Remove("INUserWiseERPDocCreation");
            }
            if (Session["KeyValue"] != null)
            {
                Session.Remove("KeyValue");
            }

        }

        protected void chkapprove_Init(object sender, EventArgs e)  // Checked and Modified By Sandip
        {
            ASPxCheckBox Dcheckbox = (ASPxCheckBox)sender;
            int itemindex = (((ASPxCheckBox)sender).NamingContainer as GridViewDataItemTemplateContainer).ItemIndex;
            KeyValue = Convert.ToInt32((((ASPxCheckBox)sender).NamingContainer as GridViewDataItemTemplateContainer).KeyValue);
            Session["KeyValue"] = KeyValue;
            Dcheckbox.ClientSideEvents.CheckedChanged = String.Format("function(s, e) {{ GetApprovedQuoteId(s, e, {0}) }}", itemindex);

        }


        protected void chkreject_Init(object sender, EventArgs e) // Checked and Modified By Sandip
        {
            ASPxCheckBox Dcheckbox = (ASPxCheckBox)sender;
            int itemindex = (((ASPxCheckBox)sender).NamingContainer as GridViewDataItemTemplateContainer).ItemIndex;
            KeyValue = Convert.ToInt32((((ASPxCheckBox)sender).NamingContainer as GridViewDataItemTemplateContainer).KeyValue);
            Session["KeyValue"] = KeyValue;
            Dcheckbox.ClientSideEvents.CheckedChanged = String.Format("function(s, e) {{ GetRejectedQuoteId(s, e, {0}) }}", itemindex);

        }

        #endregion Approval Waiting or Pending User Level Wise Section End
        #region Created User Wise List Quotation after Clicking on Status Button Section Start  (call in page load)

        protected void gridUserWiseQuotation_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            PopulateUserWiseERPDocCreation();
        }

        public void PopulateUserWiseERPDocCreation()
        {
            int userid = 0;
            if (Session["userid"] != null)
            {
                if (Session["userbranchID"] != null)
                {
                    userid = Convert.ToInt32(Session["userid"]);
                }
            }
            DataTable dtdata = new DataTable();
            if (Session["INUserWiseERPDocCreation"] == null)
            {

                dtdata = objERPDocPendingApproval.PopulateUserWiseERPDocCreation(userid, "PI");
            }
            else
            {
                dtdata = (DataTable)Session["INUserWiseERPDocCreation"];
            }
            if (dtdata != null && dtdata.Rows.Count > 0)
            {
                gridUserWiseQuotation.DataSource = dtdata;
                gridUserWiseQuotation.DataBind();
                Session["INUserWiseERPDocCreation"] = dtdata;
            }
            else
            {
                gridUserWiseQuotation.DataSource = null;
                gridUserWiseQuotation.DataBind();
            }

        }

        #endregion #region Created User Wise List Quotation after Clicking on Status Button Section End


        #region To Show Hide Status and Pending Approval Button Configuration Wise Start

        public void ConditionWiseShowStatusButton()
        {
            int i = 0;
            int j = 0;
            int k = 0;
            int branchid = 0;
            if (Session["userbranchID"] != null)
            {
                branchid = Convert.ToInt32(Session["userbranchID"]);
            }
            //Session["userbranchHierarchy"])

            #region Sam Section For Showing Status and Approval waiting Button on 22052017
            j = objERPDocPendingApproval.ConditionWiseShowApprovalStatusButton(6, Convert.ToString(Session["userbranchHierarchy"]), Convert.ToString(Session["userid"]), "PI");

            if (j == 1)
            {
                spanStatus.Visible = true;
            }
            else
            {
                spanStatus.Visible = false;
            }


            k = objERPDocPendingApproval.ConditionWiseShowApprovalPendingButton(6, Convert.ToString(Session["userbranchHierarchy"]), Convert.ToString(Session["userid"]), "PI");

            if (k == 1)
            {
                divPendingWaiting.Visible = true;
            }
            else
            {
                divPendingWaiting.Visible = false;
            }



            #endregion Sam Section For Showing Status and Approval waiting Button on 22052017
            // Cross Branch Section by Sam on 10052017 Start  
            //i = objERPDocPendingApproval.ConditionWiseShowApprovalDtlStatusButton(8, branchid, Convert.ToString(Session["userid"]), "PB");  // Entity Id 8 For Purchase Invoice
            ////i = objERPDocPendingApproval.ConditionWiseShowApprovalDtlStatusButton(8, branchid, Convert.ToString(Session["userid"]), "PB");  // 7 for Purchase Order Module 
            ////i = objERPDocPendingApproval.ConditionWiseShowStatusButton(8, branchid, Convert.ToString(Session["userid"])); //Entity Id 8 For Purchase Invoice
            //// Cross Branch Section by Sam on 10052017 End 
            //if (i == 1)
            //{
            //    spanStatus.Visible = true;
            //    divPendingWaiting.Visible = true;
            //}
            //else if (i == 2)
            //{
            //    spanStatus.Visible = false;
            //    divPendingWaiting.Visible = true;
            //}
            //else
            //{
            //    spanStatus.Visible = false;
            //    divPendingWaiting.Visible = false;
            //}
        }

        #endregion To Show Hide Status and Pending Approval Button Configuration Wise End

        //#region To Show Hide Status and Pending Approval Button Configuration Wise Start
        //public void ConditionWiseShowStatusButton()
        //{
        //    int i = 0;
        //    int branchid = 0;
        //    if (Session["userbranchID"] != null)
        //    {
        //        branchid = Convert.ToInt32(Session["userbranchID"]);
        //    }
        //    // Cross Branch Section by Sam on 10052017 Start  
        //    i = objERPDocPendingApproval.ConditionWiseShowApprovalDtlStatusButton(6, branchid, Convert.ToString(Session["userid"]), "PI");  // 7 for Purchase Order Module 
        //    //i = objERPDocPendingApproval.ConditionWiseShowStatusButton(7, branchid, Convert.ToString(Session["userid"]));  // 7 for Purchase Order Module 
        //    // Cross Branch Section by Sam on 10052017 End 
        //    //i = objERPDocPendingApproval.ConditionWiseShowStatusButton(6, branchid, Convert.ToString(Session["userid"]));  // 6 for Purchase Indent Module 
        //    if (i == 1)
        //    {
        //        spanStatus.Visible = true;
        //        divPendingWaiting.Visible = true;
        //    }
        //    else if (i == 2)
        //    {
        //        spanStatus.Visible = false;
        //        divPendingWaiting.Visible = true;
        //    }
        //    else 
        //    {
        //        spanStatus.Visible = false;
        //        divPendingWaiting.Visible = false;
        //    }
        //}

        //#endregion To Show Hide Status and Pending Approval Button Configuration Wise End

        #region After Approval Or rejected Number to reflect of Pending Approval Section  Start

        [WebMethod]
        public static string GetPendingCase()
        {
            string strPending = "(0)";

            ERPDocPendingApprovalBL objERPDocPendingApproval = new ERPDocPendingApprovalBL();
            int userid = Convert.ToInt32(HttpContext.Current.Session["userid"]);
            //int userlevel = objCRMSalesDtlBL.GetUserLevelByUserID(userid);

            DataTable dtdata = new DataTable();
            dtdata = objERPDocPendingApproval.PopulateERPDocApprovalPendingCountByUserLevel(userid, "PI");
            if (dtdata != null && dtdata.Rows.Count > 0)
            {
                strPending = "(" + Convert.ToString(dtdata.Rows[0]["ID"]) + ")";
            }

            return strPending;
        }

        #endregion After Approval Or rejected Number to reflect of Pending Approval Section  End

        public string IsExistsDocumentInERPDocApproveStatus(string id)
        {
            string result = "";

            string editable = "";
            string status = "";
            DataTable dt = new DataTable();
            int quoteid = Convert.ToInt32(id);
            dt = objERPDocPendingApproval.IsExistsDocumentInERPDocApproveStatus(quoteid, 6);  // 6 For Purchase Indent
            if (dt.Rows.Count > 0)
            {
                editable = Convert.ToString(dt.Rows[0]["editable"]);
                if (editable == "0")
                {

                    lbl_quotestatusmsg.Visible = true;
                    status = Convert.ToString(dt.Rows[0]["Status"]);
                    if (status == "Approved")
                    {
                        result = "A";
                        lbl_quotestatusmsg.Text = "Document already Approved.";

                    }
                    if (status == "Rejected")
                    {
                        result = "R";
                        lbl_quotestatusmsg.Text = "Document already Rejected.";

                    }
                    btnnew.Visible = false;
                    btnSaveExit.Visible = false;
                }
                else
                {
                    result = "X";
                    lbl_quotestatusmsg.Visible = false;
                    // btnnew.Visible = true;
                    // btnSaveExit.Visible = true;
                }
                return result;
            }
            else
            {
                return result;
            }
        }

        #endregion Sandip Section For Approval Dtl Section End

        protected void acpAvailableStock_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string performpara = e.Parameter;
            string strProductID = Convert.ToString(performpara.Split('~')[0]);
            string strBranch = Convert.ToString(ddlBranch.SelectedValue);

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

        protected void acbpCrpUdf_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (hdn_Mode.Value == "Entry")
            {
                if (reCat.isAllMandetoryDone((DataTable)Session["UdfDataOnAdd"], "PI") == false)
                {
                    acbpCrpUdf.JSProperties["cpUDFPI"] = "false";

                }
                else
                {
                    acbpCrpUdf.JSProperties["cpUDFPI"] = "true";
                }
            }
            else
            {
                acbpCrpUdf.JSProperties["cpUDFPI"] = "true";
            }
        }

        protected void gridPendingApproval_PageIndexChanged(object sender, EventArgs e)
        {
            PopulateERPDocApprovalPendingListByUserLevel();
        }


        public void FillPOLIstGrid(string Indent_Id)
        {

            DataTable dtdata = GetPOListGridData(Indent_Id);


            if (dtdata != null && dtdata.Rows.Count > 0)
            {
                gridPOLIst.DataSource = dtdata;
                gridPOLIst.DataBind();
            }
            else
            {
                gridPOLIst.DataSource = null;
                gridPOLIst.DataBind();
            }
        }

        public DataTable GetPOListGridData(string Indent_Id)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseIndentDetailsList");
            proc.AddVarcharPara("@Action", 500, "GetPurchaseOrderListByIndentID");
            proc.AddVarcharPara("@CampanyID", 500, Convert.ToString(Session["LastCompany"]));
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            //proc.AddBigIntegerPara("@Indent_Id",Convert.ToInt32(Indent_Id));
            proc.AddBigIntegerPara("@Indent_Id", Convert.ToInt32(Session["IndentIdPO"]));

            dt = proc.GetTable();
            return dt;
        }

        protected void gridPOLIst_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string Indent_Id = Convert.ToString(e.Parameters.Split('~')[1]);
            // string Indent_Id = Grid_PurchaseIndent.GetRowValues(RowIndex, "Indent_Id").ToString();
            Session["IndentIdPO"] = Indent_Id;
            string command = e.Parameters.Split('~')[0];
            if (command == "BindPOLIst")
            {
                FillPOLIstGrid(Indent_Id);
            }

        }

        protected void gridPOLIst_DataBinding(object sender, EventArgs e)
        {

            DataTable dtdata = GetPOListGridData("0");
            if (dtdata != null && dtdata.Rows.Count > 0)
            {
                gridPOLIst.DataSource = dtdata;
            }

        }

        protected void gridUserWiseQuotation_PageIndexChanged(object sender, EventArgs e)
        {
            PopulateUserWiseERPDocCreation();
        }

        //Rev Subhra 0019337  23-01-2019 
        protected void SelectPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            string strSplitCommand = e.Parameter.Split('~')[0];
            if (strSplitCommand == "Bindalldesignes")
            {
                string[] filePaths = new string[] { };
                string DesignPath = "";
                if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                {
                    DesignPath = @"Reports\Reports\RepxReportDesign\PurchaseIndent\DocDesign\Designes";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\PurchaseIndent\DocDesign\Designes";
                }
                string fullpath = Server.MapPath("~");
                fullpath = fullpath.Replace("ERP.UI\\", "");
                string DesignFullPath = fullpath + DesignPath;
                filePaths = System.IO.Directory.GetFiles(DesignFullPath, "*.repx");

                foreach (string filename in filePaths)
                {
                    string reportname = Path.GetFileNameWithoutExtension(filename);
                    string name = "";
                    if (reportname.Split('~').Length > 1)
                    {
                        name = reportname.Split('~')[0];
                    }
                    else
                    {
                        name = reportname;
                    }
                    string reportValue = reportname;
                    CmbDesignName.Items.Add(name, reportValue);
                }
                CmbDesignName.SelectedIndex = 0;
            }
            else
            {
                string DesignPath = @"Reports\Reports\REPXReports";
                string fullpath = Server.MapPath("~");
                fullpath = fullpath.Replace("ERP.UI\\", "");
                string filename = @"\RepxReportViewer.aspx";
                string DesignFullPath = fullpath + DesignPath + filename;
                string reportName = Convert.ToString(CmbDesignName.Value);
                SelectPanel.JSProperties["cpSuccess"] = "Success";
            }
        }

        //End of Rev Subhra 23-01-2019 
        //Rev Subhra 01-03-2019

        [WebMethod]
        public static string SetSessionPacking(string list)
        {
            System.Web.Script.Serialization.JavaScriptSerializer jsSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            List<ProductQuantity> finalResult = jsSerializer.Deserialize<List<ProductQuantity>>(list);
            //var result = JsonConvert.DeserializeObject<ProductQuantity>(list);
            HttpContext.Current.Session["SessionPackingDetails"] = finalResult;
            return null;

        }

        //End of Rev

        //Rev Tanmoy 23-09-2019
        protected void ProjectServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Proj_Id";
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            string branch = ddlBranch.SelectedValue;
            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);

            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);

            var q = from d in dc.V_ProjectLists
                    where d.ProjectStatus == "Approved" && d.ProjBracnchid == Convert.ToInt32(branch)
                    orderby d.Proj_Id descending
                    select d;

            e.QueryableSource = q;
        }
        //End Rev

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
        }

        #region Component Tagging

        protected void ComponentQuotation_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string status = string.Empty;
            string Customer = string.Empty;
            string OrderDate = string.Empty;
            string ComponentType = string.Empty;
            string Action = string.Empty;
            string BranchID = string.Empty;
            string inventory = string.Empty;
            string inventoryType = string.Empty;
            string FinYear = Convert.ToString(Session["LastFinYear"]);

            if (e.Parameter.Split('~')[0] == "BindComponentGrid")
            {
                BranchID = Convert.ToString(ddlBranch.SelectedValue);
                // if (e.Parameter.Split('~')[1] != null) Customer = e.Parameter.Split('~')[1];
                if (e.Parameter.Split('~')[1] != null) OrderDate = e.Parameter.Split('~')[1];
                if (e.Parameter.Split('~')[3] != null) ComponentType = e.Parameter.Split('~')[3];
                //if (e.Parameter.Split('~')[5] != null) inventory = e.Parameter.Split('~')[5];
                //if (e.Parameter.Split('~')[6] != null) inventoryType = e.Parameter.Split('~')[6];

                if (ComponentType == "ES")
                {
                    Action = "Estimate";
                    lbl_InvoiceNO.Text = "Estimate Date";
                }
                else if (ComponentType == "BQ")
                {
                    Action = "BOQ";
                    lbl_InvoiceNO.Text = "BOQ Date";
                }

                if (e.Parameter.Split('~')[2] == "DateCheck")
                {
                    // lookup_quotation.GridView.Selection.UnselectAll();
                }


                string strInvoiceID = Convert.ToString(Session["Indent_TagID"]);
                DataTable ComponentTable = new DataTable();
                //if (Session["SI_ComponentData"] != null)
                //{
                //    ComponentTable = (DataTable)Session["SI_ComponentData"];
                //}
                //else
                //{
                ComponentTable = GetComponent(OrderDate, BranchID, Action, strInvoiceID);
                lookup_quotation.GridView.Selection.CancelSelection();
                // }






                Session["SI_ComponentData"] = ComponentTable;
                lookup_quotation.DataSource = ComponentTable;
                lookup_quotation.DataBind();

                if (strInvoiceID != null && strInvoiceID != "")
                {
                    string[] eachQuo = strInvoiceID.Split(',');
                    foreach (string val in eachQuo)
                    {
                        lookup_quotation.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));
                    }
                }


            }

            else if (e.Parameter.Split('~')[0] == "RebindGridQuote")//Subhabrata for binding quotation
            {
                QuotationIds = OldSelectedKeyvalue.Value.TrimStart(',');
                if (!String.IsNullOrEmpty(QuotationIds))
                {
                    string[] eachQuo = QuotationIds.Split(',');
                    if (eachQuo.Length > 1)//More tha one quotation
                    {
                        ComponentQuotationPanel.JSProperties["cpRebindGridQuote"] = "Multiple Select Quotation Dates";
                        lookup_quotation.GridView.Selection.UnselectAll();
                        foreach (string val in eachQuo)
                        {
                            lookup_quotation.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));
                        }
                    }
                    else if (eachQuo.Length == 1)//Single Quotation
                    {
                        lookup_quotation.GridView.Selection.UnselectAll();
                        foreach (string val in eachQuo)
                        {
                            lookup_quotation.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));
                        }
                        ComponentQuotationPanel.JSProperties["cpRebindGridQuote"] = Convert.ToString(lookup_quotation.GridView.GetSelectedFieldValues("ComponentDate")[0]);
                    }
                    else//No Quotation selected
                    {
                        lookup_quotation.GridView.Selection.UnselectAll();
                    }
                }



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
                    string SalesmanName = Convert.ToString(dt.Rows[0]["SalesmanName"]);
                    string ExpiryDate = Convert.ToString(dt.Rows[0]["ExpiryDate"]);
                    string CurrencyRate = Convert.ToString(dt.Rows[0]["CurrencyRate"]);
                    string Type = Convert.ToString(dt.Rows[0]["ComponentType"]);
                    string CreditDays = Convert.ToString(dt.Rows[0]["CreditDays"]);
                    string DueDate = Convert.ToString(dt.Rows[0]["DueDate"]);

                    ComponentQuotationPanel.JSProperties["cpDetails"] = Reference + "~" + Currency_Id + "~" + SalesmanId + "~" + ExpiryDate + "~" + CurrencyRate + "~" + Type + "~" + CreditDays + "~" + DueDate + "~" + SalesmanName;
                }
            }
            else if (e.Parameter.Split('~')[0] == "DateCheckOnChanged")//Subhabrata for binding quotation
            {
                if (grid_Products.GetSelectedFieldValues("Quotation_No").Count != 0)
                {
                    DateTime SalesOrderDate = Convert.ToDateTime(e.Parameter.Split('~')[1]);
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
            if (Session["SI_ComponentData"] != null)
            {
                lookup_quotation.DataSource = (DataTable)Session["SI_ComponentData"];
            }
        }

        protected void ComponentDatePanel_Callback(object sender, CallbackEventArgsBase e)
        {
            string strSplitCommand = e.Parameter.Split('~')[0];
            if (strSplitCommand == "BindComponentDate")
            {
                string ComponentNo = Convert.ToString(e.Parameter.Split('~')[1]);
                string type = Convert.ToString(e.Parameter.Split('~')[2]);

                DataTable dtDetails = GetComponentDate(ComponentNo, type);
                if (dtDetails != null && dtDetails.Rows.Count > 0)
                {
                    string Date = Convert.ToString(dtDetails.Rows[0]["ComponentDate"]);
                    if (!string.IsNullOrEmpty(Date))
                    {
                        txt_InvoiceDate.Text = Convert.ToString(Date);
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
                string strType = Convert.ToString(rdl_SaleInvoice.SelectedValue);
                String QuoComponent = "";
                String QuoComponentProj = "";
                List<object> QuoList = lookup_quotation.GridView.GetSelectedFieldValues("Details_ID");
                foreach (object Quo in QuoList)
                {
                    QuoComponent += "," + Quo;
                }
                QuoComponent = QuoComponent.TrimStart(',');

                if (Quote_Nos != "$")
                {
                    string strAction = "";


                    if (strType == "ES")
                    {
                        strAction = "GetEstimateProducts";
                        grid_Products.Columns["ComponentNumber"].Caption = "Estimate No";
                    }
                    else if (strType == "BQ")
                    {
                        strAction = "GetBOQProducts";
                        grid_Products.Columns["ComponentNumber"].Caption = "BOQ No";
                    }
                    else if (strType == "SC")
                    {
                        strAction = "GetChallanProducts";
                        grid_Products.Columns["ComponentNumber"].Caption = "Challan No";
                    }

                    string strInvoiceID = Convert.ToString(Session["Indent_Id"]);
                    DataTable dtDetails = GetComponentProductList(strAction, QuoComponent, strInvoiceID);

                    grid_Products.DataSource = dtDetails;
                    grid_Products.DataBind();

                    DataTable Transaction_dt = (DataTable)Session["ProductIndentDetails"];
                    if (Transaction_dt != null && Transaction_dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < grid_Products.VisibleRowCount; i++)
                        {
                            string ESTIMATE_DETAILSID = Convert.ToString(grid_Products.GetRowValues(i, "ESTIMATE_DETAILSID"));
                            string ProductList = Convert.ToString(grid_Products.GetRowValues(i, "ProductID"));

                            string[] list = ProductList.Split(new string[] { "||@||" }, StringSplitOptions.None);
                            string ProductID = Convert.ToString(list[0]) + "||@||%";

                            var Checkdt = Transaction_dt.Select("ProdDetailsId='" + ESTIMATE_DETAILSID + "' AND ProductID LIKE '" + ProductID + "'");
                            if (Checkdt.Length > 0)
                            {
                                grid_Products.Selection.SelectRow(i);
                            }
                        }
                    }
                }
                else
                {
                    grid_Products.DataSource = null;
                    grid_Products.DataBind();
                }
                String Action = "";
                if (strType == "ES")
                {
                    Action = "Estimate";
                }
                else if (strType == "BQ")
                {
                    Action = "BOQ";
                }
                QuoComponentProj = QuoComponent.Split(',')[0].ToString();
                Int64 ProjId = 0;
                if (QuoComponentProj != "")
                {
                    DataTable dtproj = GetProjectEditData(QuoComponentProj, Action);
                    if (dtproj != null && dtproj.Rows.Count > 0)
                    {
                        ProjId = Convert.ToInt64(dtproj.Rows[0]["Proj_Id"]);
                    }
                    else
                    {
                        ProjId = 0;
                    }
                }

                grid_Products.JSProperties["cpComponentDetails"] = ProjId;
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

        public DataTable GetComponentDate(string Component_ID, string Type)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesInvoice_Details");
            proc.AddVarcharPara("@SelectedComponentList", 100, Component_ID);
            proc.AddVarcharPara("@ComponentType", 100, Type);
            proc.AddVarcharPara("@Action", 100, "GetComponentDateAddEdit");

            return proc.GetTable();
        }


        public DataTable GetComponent(string Date, string BranchID, string Action, string ComponentList)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_IndentTaggingDetails");
            proc.AddVarcharPara("@Action", 500, Action);
            proc.AddVarcharPara("@ComponentList", 50, ComponentList);
            proc.AddDateTimePara("@Date", Convert.ToDateTime(Date));
            proc.AddVarcharPara("@BranchID", 3000, BranchID);
            dt = proc.GetTable();
            return dt;
        }


        public DataTable GetComponentProductList(string Action, string ComponentList, string InvoiceID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_IndentTaggingDetails");
            proc.AddVarcharPara("@Action", 500, Action);
            proc.AddVarcharPara("@ComponentList", 2000, ComponentList);
            proc.AddVarcharPara("@InvoiceID", 20, InvoiceID);
            dt = proc.GetTable();
            return dt;
        }

        protected void BindLookUp(string OrderDate, string ComponentType, string BranchID, String ComponentList)
        {
            string Action = "";
            if (ComponentType == "ES")
            {
                Action = "Estimate";
            }
            else if (ComponentType == "BQ")
            {
                Action = "BOQ";
            }
            else if (ComponentType == "SC")
            {
                Action = "GetChallan";
            }

            string strIndentID = Convert.ToString(Session["Indent_Id"]);
            string FinYear = Convert.ToString(Session["LastFinYear"]);
            DataTable ComponentTable = GetComponent(OrderDate, BranchID, Action, ComponentList);
            lookup_quotation.GridView.Selection.CancelSelection();

            lookup_quotation.GridView.Selection.CancelSelection();
            lookup_quotation.DataSource = ComponentTable;
            lookup_quotation.DataBind();

            Session["SI_ComponentData"] = ComponentTable;
        }
        #endregion

        public DataSet GetSelectedComponentProductList(string Action, string SelectedComponentList, string InvoiceID)
        {
            DataSet dt = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("PRC_IndentTaggingDetails");
            proc.AddVarcharPara("@Action", 500, Action);
            proc.AddVarcharPara("@SelectedComponentList", 2000, SelectedComponentList);
            proc.AddVarcharPara("@InvoiceID", 20, InvoiceID);
            dt = proc.GetDataSet();
            return dt;
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
                ProcedureExecute proc = new ProcedureExecute("prc_ProjectOrder_Details");
                proc.AddVarcharPara("@Action", 100, "GetProjectCheckINIndent");
                proc.AddVarcharPara("@Doc_Type", 100, Doctype);
                proc.AddVarcharPara("@ProjectDetails", 500, quote_Id);
                proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
                proc.RunActionQuery();
                returnValue = Convert.ToString(proc.GetParaValue("@ReturnValue"));

            }
            return returnValue;

        }

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
    }
}