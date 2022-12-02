using DevExpress.Web;
using DevExpress.Web.Mvc;
using EntityLayer.CommonELS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;
using System.Collections.Specialized;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Threading.Tasks;
using DevExpress.XtraPrinting;
using DevExpress.Export;
using System.Drawing;
using Reports.Model;

namespace Reports.Reports.GridReports
{
    public partial class GSTR_Vendorpaymetreceipt : System.Web.UI.Page
    {
        ReportData rpt = new ReportData();
        //DataTable DTIndustry = new DataTable();
        BusinessLogicLayer.Reports objReport = new BusinessLogicLayer.Reports();
        //string data = "";
        string totalbillCount;
        string totalrecpCount;
        DataTable dtDocumentTotal = null;
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Reports/master/GSTR_Vendorpaymetreceipt.aspx");
            DateTime dtFrom;
            DateTime dtTo;
            if (!IsPostBack)
            {
                DataTable dtBranchSelection = new DataTable();
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "GSTR Vendor Payment Receipt";
                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, false, false, false, false, false);
                CompName.Text = GridHeader;

                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, true, false, false, false, false);
                CompAdd.Text = GridHeader;

                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, false, true, false, false, false);
                CompOth.Text = GridHeader;

                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, false, false, true, false, false);
                CompPh.Text = GridHeader;

                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, false, false, false, false, true);
                CompAccPrd.Text = GridHeader;

                //Session["gridGstR1"] = null;
                //Session["dt_GSTRRpt"] = null;
                Session["IsGSTRVendRecPayRegFilter"] = null;
                dtFrom = DateTime.Now;
                dtTo = DateTime.Now;
                //   BindDropDownList();

                ASPxFromDate.Text = dtFrom.ToString("dd-MM-yyyy");
                ASPxToDate.Text = dtTo.ToString("dd-MM-yyyy");
                //Date_finyearwise(Convert.ToString(Session["LastFinYear"]));

                ASPxFromDate.Value = DateTime.Now;
                ASPxToDate.Value = DateTime.Now;

                BranchpopulateGSTN();
                Date_finyearwise(Convert.ToString(Session["LastFinYear"]));
                //Rev Subhra 20-12-2018   0017670
                Session["BranchNames"] = null;
                //End Rev
                dtBranchSelection = oDBEngine.GetDataTable("select Variable_Value from Config_SystemSettings where Variable_Name='ReportsBranchSelection'");
                hdnBranchSelection.Value = dtBranchSelection.Rows[0][0].ToString();
            }
            else
            {

            }
        }

        public void Date_finyearwise(string Finyear)
        {
            //CommonBL bll1 = new CommonBL();
            //DataTable stbill = new DataTable();
            //stbill = bll1.GetDateFinancila(Finyear);
            //if (stbill.Rows.Count > 0)
            //{
            //    ASPxFromDate.Text = Convert.ToDateTime(stbill.Rows[0]["FinYear_StartDate"]).ToString("dd-MM-yyyy");
            //    ASPxToDate.Text = Convert.ToDateTime(stbill.Rows[0]["FinYear_EndDate"]).ToString("dd-MM-yyyy");
            //}

            CommonBL cmbl = new CommonBL();
            DataTable dtfnyear = new DataTable();

            dtfnyear = cmbl.GetDateFinancila(Finyear);
            if (dtfnyear.Rows.Count > 0)
            {
                ASPxFromDate.MaxDate = Convert.ToDateTime((dtfnyear.Rows[0]["FinYear_EndDate"]));
                ASPxFromDate.MinDate = Convert.ToDateTime((dtfnyear.Rows[0]["FinYear_StartDate"]));

                ASPxToDate.MaxDate = Convert.ToDateTime((dtfnyear.Rows[0]["FinYear_EndDate"]));
                ASPxToDate.MinDate = Convert.ToDateTime((dtfnyear.Rows[0]["FinYear_StartDate"]));

                DateTime MaximumDate = Convert.ToDateTime((dtfnyear.Rows[0]["FinYear_EndDate"]));
                DateTime MinimumDate = Convert.ToDateTime((dtfnyear.Rows[0]["FinYear_StartDate"]));

                DateTime TodayDate = Convert.ToDateTime(DateTime.Now);
                DateTime FinYearEndDate = MaximumDate;

                if (TodayDate > FinYearEndDate)
                {
                    ASPxToDate.Date = FinYearEndDate;
                    ASPxFromDate.Date = MinimumDate;
                }
                else
                {
                    ASPxToDate.Date = TodayDate;
                    ASPxFromDate.Date = MinimumDate;
                }
            }
        }

        public void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                if (Session["exportval"] == null)
                {
                    //  Session["exportval"] = Filter;
                    // BindDropDownList();
                    bindexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportval"]) != Filter)
                {
                    // Session["exportval"] = Filter;
                    // BindDropDownList();
                    bindexport(Filter);
                }
            }
        }
       
        public void BindDropDownList()
        {
            // Declare a Dictionary to hold all the Options with Value and Text.
            Dictionary<string, string> options = new Dictionary<string, string>();
            options.Add("0", "Export to");
            options.Add("1", "PDF");
            options.Add("2", "XLS");
            options.Add("3", "RTF");
            options.Add("4", "CSV");

            // Bind the Dictionary to the DropDownList.
            drdExport.DataSource = options;
            drdExport.DataTextField = "value";
            drdExport.DataValueField = "key";
            drdExport.DataBind();
            drdExport.SelectedValue = "0";
        }
        public void bindexport(int Filter)
        {
            string filename = "GSTR Vendor payment Receipt";
            exporter.FileName = filename;
            string FileHeader = "";

            exporter.FileName = filename;

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true,true, true, true) + Environment.NewLine + "GSTR Vendor payment Receipt" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
            //Rev Subhra 20-12-2018   0017670
            FileHeader = ReplaceFirst(FileHeader, "\r\n", Convert.ToString(Session["BranchNames"]));
            //End of Rev
            exporter.PageHeader.Left = FileHeader;
            exporter.PageHeader.Font.Size = 10;
            exporter.PageHeader.Font.Name = "Tahoma";

            //exporter.PageFooter.Center = "[Page # of Pages #]";
            //exporter.PageFooter.Right = "[Date Printed]";
            exporter.Landscape = true;
            exporter.MaxColumnWidth = 100;
            exporter.RenderBrick += exporter_RenderBrick;
            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();

                    break;
                case 2:
                    exporter.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
                    break;
                case 3:
                    exporter.WriteRtfToResponse();
                    break;
                case 4:
                    exporter.WriteCsvToResponse();
                    break;
            }

        }

        //Rev Subhra 20-12-2018   0017670
        public string ReplaceFirst(string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + Environment.NewLine + replace + Environment.NewLine + text.Substring(pos + search.Length);
        }
        //End of Rev

        protected void exporter_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        {
            e.BrickStyle.BackColor = Color.White;
            e.BrickStyle.ForeColor = Color.Black;
        }

        //protected void BindLedgerPosting()
        //{
        //    try
        //    {
        //        if (Session["dtLedger"] != null)
        //        {
        //            ShowGrid.DataSource = (DataTable)Session["dtLedger"];
        //            ShowGrid.DataBind();
        //        }
        //    }
        //    catch { }
        //}

        //protected void Grid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        protected void CallbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            //Session.Remove("dt_GSTRRpt");
            //ShowGrid.JSProperties["cpSave"] = null;
            //string returnPara = Convert.ToString(e.Parameters);

            string returnPara = Convert.ToString(e.Parameter);

            string IsGSTRVendRecPayRegFilter = Convert.ToString(hfIsGSTRVendRecPayRegFilter.Value);
            Session["IsGSTRVendRecPayRegFilter"] = IsGSTRVendRecPayRegFilter;

            string WhichCall = returnPara.Split('~')[0];
            if (WhichCall == "ListData")
            {
                string WhichCall2 = returnPara.Split('~')[1];
                string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);


                string BRANCH_ID = "";
                //if (hdnSelectedBranches.Value != "")
                //{
                //    BRANCH_ID = hdnSelectedBranches.Value;
                //}

                string BranchComponent = "";
                List<object> BranchList = lookup_branch.GridView.GetSelectedFieldValues("branch_id");
                foreach (object branch in BranchList)
                {
                    BranchComponent += "," + branch;
                }
                BRANCH_ID = BranchComponent.TrimStart(',');

                //Rev Subhra 20-12-2018   0017670

                string BRANCH_NAME = "";
                string BranchNameComponent = "";
                List<object> BranchNameList = lookup_branch.GridView.GetSelectedFieldValues("branch_description");
                foreach (object BranchName in BranchNameList)
                {
                    BranchNameComponent += "," + BranchName;
                }
                if (BranchNameList.Count > 1)
                {
                    BRANCH_NAME = "Multiple Branch Selected";
                    Session["BranchNames"] = BRANCH_NAME;
                }
                else
                {
                    BRANCH_NAME = BranchNameComponent.TrimStart(',');
                    Session["BranchNames"] = "For Unit : " + BRANCH_NAME + " ";
                }
                CallbackPanel.JSProperties["cpBranchNames"] = Convert.ToString(Session["BranchNames"]);

                //End of Rev

                //GetSalesRegisterdata(WhichCall2, BRANCH_ID);
                Task PopulateStockTrialDataTask = new Task(() => GetGSTRCustVendRecPayRegisterdata(WhichCall2, BRANCH_ID));
                PopulateStockTrialDataTask.RunSynchronously();
            }
        }

        //protected void grid_DataBinding(object sender, EventArgs e)
        //{
        //    if (Session["gridGstR1"] != null)
        //    {
        //        ShowGrid.DataSource = (DataTable)Session["gridGstR1"];
        //    }
        //}

        public void GetGSTRCustVendRecPayRegisterdata(string Gstn, string BRANCH_ID)
        {
            try
            {
                //string DriverName = string.Empty;
                //string PhoneNo = string.Empty;
                //string VehicleNo = string.Empty;
                //DataTable dttab = new DataTable();

                DateTime dtFrom;
                DateTime dtTo;

                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);

                string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
                string TODATE = dtTo.ToString("yyyy-MM-dd");


                //dttab = GetBranchGestn(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]).Trim(), "sale", Gstn, FROMDATE, TODATE, BRANCH_ID);

                //if (dttab.Rows.Count > 0)
                //{
                //    Session["gridGstR1"] = dttab;
                //    ShowGrid.DataSource = dttab;
                //    ShowGrid.DataBind();
                //}
                //else
                //{
                //    Session["gridGstR1"] = null;
                //    ShowGrid.DataSource = null;
                //    ShowGrid.DataBind();
                //}

                DataSet ds = new DataSet();
                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("prc_CustVendRecPayRegister_Report", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@BRANCHID", BRANCH_ID);
                cmd.Parameters.AddWithValue("@GSTIN", Gstn);
                cmd.Parameters.AddWithValue("@FROMDATE", FROMDATE);
                cmd.Parameters.AddWithValue("@TODATE", TODATE);
                cmd.Parameters.AddWithValue("@TYPE", "DV");
                cmd.Parameters.AddWithValue("@USERID", Convert.ToInt32(Session["userid"]));

                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);

                cmd.Dispose();
                con.Dispose();
            }
            catch (Exception ex)
            {
            }
        }
        //public DataTable GetBranchGestn(string Company, string Finyear, string Action, string Gstn, string FROMDATE, string TODATE, string BRANCH_ID)
        //{
        //    DataTable ds = new DataTable();
        //    ProcedureExecute proc = new ProcedureExecute("prc_CustVendRecPayRegister_Report");
        //    proc.AddPara("@COMPANYID", Company);
        //    proc.AddPara("@FINYEAR", Finyear);
        //    proc.AddPara("@TYPE", "DV");
        //    proc.AddPara("@FROMDATE", FROMDATE);
        //    proc.AddPara("@TODATE", TODATE);
        //    proc.AddPara("@GSTIN", Gstn);
        //    proc.AddPara("@BRANCHID", BRANCH_ID);
        //    ds = proc.GetTable();
        //    return ds;
        //}

        //[WebMethod]
        //public static List<string> GetBranchesList(String NoteId)
        //{
        //    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        //    StringBuilder filter = new StringBuilder();
        //    StringBuilder Supervisorfilter = new StringBuilder();
        //    BusinessLogicLayer.Others objbl = new BusinessLogicLayer.Others();
        //    DataTable dtbl = new DataTable();
        //    if (NoteId.Trim() == "")
        //    {
        //        dtbl = oDBEngine.GetDataTable("select branch_id,branch_description from tbl_master_branch where branch_id in(" + Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]) + ")  order by branch_description asc");

        //    }

        //    List<string> obj = new List<string>();

        //    foreach (DataRow dr in dtbl.Rows)
        //    {

        //        obj.Add(Convert.ToString(dr["branch_description"]) + "|" + Convert.ToString(dr["branch_id"]));
        //    }
        //    return obj;
        //}


        //[WebMethod]
        //public static List<string> BindLedgerType(String Ids)
        //{
        //    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);

        //    DataTable dtbl = new DataTable();
        //    if (Ids.Trim() != "")
        //    {
        //        dtbl = oDBEngine.GetDataTable("select A.MainAccount_ReferenceID AS ID,A.MainAccount_Name as 'AccountName' FROM Master_MainAccount A WHERE A.MainAccount_AccountCode IN(SELECT RTRIM(B.AccountsLedger_MainAccountID) FROM Trans_AccountsLedger B WHERE B.AccountsLedger_BranchId in(" + Ids + ")) ORDER BY A.MainAccount_Name ");

        //    }

        //    List<string> obj = new List<string>();

        //    foreach (DataRow dr in dtbl.Rows)
        //    {

        //        obj.Add(Convert.ToString(dr["AccountName"]) + "|" + Convert.ToString(dr["Id"]));
        //    }
        //    return obj;
        //}

        //[WebMethod]
        //public static List<string> BindCustomerVendor()
        //{
        //    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        //    DataTable dtbl = new DataTable();
        //    //dtbl = oDBEngine.GetDataTable("select cnt_id AS ID,ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Name'  FROM tbl_master_contact WHERE cnt_contactType in('CL','DV') AND cnt_branchid IN("+ Ids +") ORDER BY ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'') ");
        //    dtbl = oDBEngine.GetDataTable("select cnt_id AS ID,ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Name'  FROM tbl_master_contact WHERE cnt_contactType in('CL','DV') ORDER BY ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'') ");
        //    List<string> obj = new List<string>();
        //    foreach (DataRow dr in dtbl.Rows)
        //    {
        //        obj.Add(Convert.ToString(dr["Name"]) + "|" + Convert.ToString(dr["Id"]));
        //    }
        //    return obj;
        //}

        protected void ShowGrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            //  e.Text = string.Format("{0}", Convert.ToDecimal(e.Value));
            e.Text = string.Format("{0}", e.Value);

            if (Convert.ToString(Session["IsGSTRVendRecPayRegFilter"]) == "Y")
            {
                dtDocumentTotal = oDBEngine.GetDataTable("SELECT COUNT(DISTINCT ReceiptPayment_VoucherNumber) AS ReceiptPayment_VoucherNumber FROM GSTRCUSTVENDRECPAYREGISTER_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND REPORTTYPE='DV'");
                totalrecpCount = dtDocumentTotal.Rows[0][0].ToString();
            }

            string summaryTAG = (e.Item as ASPxSummaryItem).Tag;
            if (e.IsTotalSummary == true)
            {
                switch (summaryTAG)
                {
                    case "Item_RecPayNumber":
                        e.Text = "Doc. Count = " + totalrecpCount;
                        break;
                }
            }
        }

        #region ########  Branch GRN Populate  #######
        protected void BranchpopulateGSTN()
        {
            // DataTable dst = new DataTable();
            string userbranchID = Convert.ToString(Session["userbranchID"]);

            DataSet ds = new DataSet();
            //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("GetGSTNfetch", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Company", Convert.ToString(Session["LastCompany"]));
            cmd.Parameters.AddWithValue("@Branchlist", Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);

            cmd.Dispose();
            con.Dispose();

            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlgstn.DataSource = ds.Tables[0];
                ddlgstn.DataTextField = "branch_GSTIN";
                ddlgstn.DataValueField = "branch_GSTIN";
                ddlgstn.DataBind();
                ddlgstn.Items.Insert(0, "");
            }
        }
        #endregion

        //protected void Showgrid_Datarepared(object sender, EventArgs e)
        //{
        //    ASPxGridView grid = (ASPxGridView)sender;
            //if (ddlgstrtype.SelectedValue == "7" )
            //{
            //    grid.Columns["GSTIN/UIN"].Visible = false;
            //    grid.Columns["Date"].Visible = false;
            //    grid.Columns["Value"].Visible = false;
            //    grid.Columns["Taxable value"].Visible = false;
            //    grid.Columns["POS"].Visible = false;
            //    grid.Columns["Reverse Charge"].Visible = false;
            //    grid.Columns["GSTIN E-Commerce"].Visible = false;
            //}

            ////else 
            ////{
            ////}
            //    //grid.Columns["GSTIN/UIN"].Visible = true;
            //    //grid.Columns["Date"].Visible = true;
            //    //grid.Columns["Value"].Visible = true;
            //    //grid.Columns["Taxable value"].Visible = false;
            //    //grid.Columns["POS"].Visible = true;
            //    //grid.Columns["Reverse Charge"].Visible = true;
            //    //grid.Columns["GSTIN E-Commerce"].Visible = true;

            //else if (ddlgstrtype.SelectedValue == "7A")
            //{
            //    grid.Columns["GSTIN/UIN"].Visible = false;
            //    grid.Columns["Date"].Visible = false;
            //    grid.Columns["Value"].Visible = false;
            //    grid.Columns["Taxable value"].Visible = false;
            //    grid.Columns["POS"].Visible = false;
            //    grid.Columns["Reverse Charge"].Visible = false;
            //    grid.Columns["GSTIN E-Commerce"].Visible = false;
            //}
        //}

        //private int totalCount;
        //private int totalCountrecp;
        //private List<string> ReceiptPayment_VoucherNumber = new List<string>();
        //private List<string> GSTINUIN = new List<string>();

        //protected void ASPxGridView1_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        //{
        //    ASPxSummaryItem item = e.Item as ASPxSummaryItem;

        //    if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Start)
        //    {
        //        ReceiptPayment_VoucherNumber.Clear();
        //        GSTINUIN.Clear();
        //        totalCount = 0;
        //        totalCountrecp = 0;
        //    }
        //    if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Calculate)
        //    {
        //        string val = Convert.ToString(e.FieldValue);
        //        if (!ReceiptPayment_VoucherNumber.Contains(val))
        //        {
        //            totalCount++;
        //            ReceiptPayment_VoucherNumber.Add(val);
        //        }
        //        if (!GSTINUIN.Contains(val))
        //        {
        //            totalCountrecp++;
        //            GSTINUIN.Add(val);
        //        }
        //    }
        //    if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Finalize)
        //    {
        //        if (e.Item == ShowGrid.TotalSummary["ReceiptPayment_VoucherNumber"])
        //        {
        //            e.TotalValue = string.Format("Doc. Count={0}", totalCount);
        //        }
        //        if (e.Item == ShowGrid.TotalSummary["GSTINUIN"])
        //        {
        //            e.TotalValue = string.Format("No. of Recipients={0}", totalCountrecp);
        //        }
        //        //if (e.Item == ShowGrid.TotalSummary["Value"])
        //        //{
        //        //    e.TotalValue = "Total Invoice Value={0}" + e.TotalValue;
        //        //}
        //        //if (e.Item == ShowGrid.TotalSummary["Taxable value"])
        //        //{
        //        //    e.TotalValue = "Total Taxable Value={0}" + e.TotalValue;
        //        //}
        //    }
        //}

        protected void ASPxCallbackPanel1_Callback(object sender, CallbackEventArgsBase e)
        {
            //string FinYear = Convert.ToString(Session["LastFinYear"]);

            //if (e.Parameter.Split('~')[0] == "BindComponentGrid")
            //{
            //    DataTable ComponentTable = new DataTable();
            //    string Hoid = e.Parameter.Split('~')[1];
            //    if (Session["userbranchHierarchy"] != null)
            //    {
            //        ComponentTable = oDBEngine.GetDataTable("select branch_id as ID,branch_description,branch_code from tbl_master_branch where branch_id in(" + Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]) + ") order by branch_description asc");
            //    }
            //    if (ComponentTable.Rows.Count > 0)
            //    {
            //        Session["SI_ComponentData_Branch"] = ComponentTable;
            //        lookup_branch.DataSource = ComponentTable;
            //        lookup_branch.DataBind();
            //    }
            //    else
            //    {
            //        lookup_branch.DataSource = null;
            //        lookup_branch.DataBind();
            //    }
            //}

            string FinYear = Convert.ToString(Session["LastFinYear"]);

            if (e.Parameter.Split('~')[0] == "BindComponentGrid")
            {
                DataTable ComponentTable = new DataTable();
                string Hoid = e.Parameter.Split('~')[1];

                if (Hoid != "0")
                {
                    DataSet ds = new DataSet();
                    //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                    SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                    SqlCommand cmd = new SqlCommand("Getbranchlist_Gsitnwise", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@GstinId", ddlgstn.SelectedValue);
                    cmd.Parameters.AddWithValue("@Branch", Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
                    cmd.Parameters.AddWithValue("@GstinId", Hoid);
                    cmd.CommandTimeout = 0;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(ds);

                    cmd.Dispose();
                    con.Dispose();


                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Session["SI_ComponentData_Branch"] = ds.Tables[0];
                        lookup_branch.DataSource = ds.Tables[0];
                        lookup_branch.DataBind();
                    }
                    else
                    {
                        Session["SI_ComponentData_Branch"] = ds.Tables[0];
                        lookup_branch.DataSource = null;
                        lookup_branch.DataBind();
                    }

                }
                else
                {
                    Session["SI_ComponentData_Branch"] = null;
                    lookup_branch.DataSource = null;
                    lookup_branch.DataBind();
                }
            }
        }

        protected void lookup_branch_DataBinding(object sender, EventArgs e)
        {
            if (Session["SI_ComponentData_Branch"] != null)
            {
                //    DataTable ComponentTable = oDBEngine.GetDataTable("select branch_id,branch_description,branch_code from tbl_master_branch where branch_parentId='" + Hoid + "' order by branch_description asc");
                lookup_branch.DataSource = (DataTable)Session["SI_ComponentData_Branch"];
            }
        }

        #region LinQ
        protected void GenerateEntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SrlNo";
            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsGSTRVendRecPayRegFilter"]) == "Y")
            {
                var q = from d in dc.GSTRCUSTVENDRECPAYREGISTER_REPORTs
                        where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.REPORTTYPE) == "DV"
                        orderby d.SrlNo
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.GSTRCUSTVENDRECPAYREGISTER_REPORTs
                        where Convert.ToString(d.SrlNo) == "0"
                        orderby d.SrlNo
                        select d;
                e.QueryableSource = q;
            }
        }
        #endregion

    }
}