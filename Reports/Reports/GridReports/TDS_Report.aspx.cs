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
    public partial class TDS_Report : System.Web.UI.Page
    {
        //DataTable DTIndustry = new DataTable();
        DateTime dtFrom;
        DateTime dtTo;
        BusinessLogicLayer.Reports objReport = new BusinessLogicLayer.Reports();
        //string data = "";

        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        DataTable dtDocumentTotal = null;
        string TotDocCount = "";

        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            //SalesDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            //EntityDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/GridReports/TDS_Report.aspx");
            DateTime dtFrom;
            DateTime dtTo;

            if (!IsPostBack)
            {
                DataTable dtBranchSelection = new DataTable();
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "TDS Report";
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

                //ASPxddlmonth.Items.Insert(0, new ListEditItem("All", "All-All"));

                Session["exportval"] = null;
                //Session["dtLedger"] = null;
                Session["SI_ComponentData_Financer"] = null;
                Session["SI_ComponentData_Branch"] = null;
                Session["IsTDSFilter"] = null;
                dtFrom = DateTime.Now;
                dtTo = DateTime.Now;
                ASPxFromDate.Text = dtFrom.ToString("dd-MM-yyyy");
                ASPxToDate.Text = dtTo.ToString("dd-MM-yyyy");
                ASPxFromDate.Value = DateTime.Now;
                ASPxToDate.Value = DateTime.Now;

                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);

                //ShowGrid.Columns[7].Visible = false;
                ShowGrid.Columns[9].Visible = false;

                string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
                string TODATE = dtTo.ToString("yyyy-MM-dd");
                Date_finyearwise(Convert.ToString(Session["LastFinYear"]));
                //   BindDropDownList();
                Getsectionpopulate();
                //Rev Subhra 20-12-2018   0017670
                Session["BranchNames"] = null;
                //End Rev
                dtBranchSelection = oDBEngine.GetDataTable("select Variable_Value from Config_SystemSettings where Variable_Name='ReportsBranchSelection'");
                hdnBranchSelection.Value = dtBranchSelection.Rows[0][0].ToString();
            }
            else
            {
                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);
            }

            if (!IsPostBack)
            {
                //Session.Remove("dtLedger");
                //ShowGrid.JSProperties["cpSave"] = null;
                //  string[] CallVal = e.Parameters.ToString().Split('~');
                //string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                //string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);

                string BRANCH_ID = "";
                if (hdnSelectedBranches.Value != "")
                {
                    BRANCH_ID = hdnSelectedBranches.Value;
                }

                //string CASHBANKTYPE = "";
                //string CASHBANKID = "";
                //string UserId = "";
                string CUSTVENDID = "";
                if (hdnSelectedCustomerVendor.Value != "")
                {
                    CUSTVENDID = hdnSelectedCustomerVendor.Value;
                }

                //string LEDGERID = "";
                //ASPxddlmonth.DataSource = GetMonthTable();
                //ASPxddlmonth.TextField = "MonthName";
                //ASPxddlmonth.ValueField = "MonthId";
                //ASPxddlmonth.DataBind();
                //ASPxddlmonth.Items.Insert(0, new ListEditItem("All", "All-All"));
                //ASPxddlmonth.SelectedIndex = 0;

                //ddlmonth.DataSource = GetMonthTable();
                //ddlmonth.DataTextField = "MonthName";
                //ddlmonth.DataValueField = "MonthId";
                //ddlmonth.DataBind();
                //ddlmonth.Items.Insert(0, new ListItem("All", "All-All"));

                /// GetTdSReport(BRANCH_ID, CUSTVENDID,ddlmonth.SelectedValue);

                //Task PopulateStockTrialDataTask = new Task(() => GetTdSReport(BRANCH_ID, CUSTVENDID, ddlmonth.SelectedValue));
                //PopulateStockTrialDataTask.RunSynchronously(); 

            }
        }


        //public DataTable GetMonthTable()
        //{
        //    CommonBL cbl = new CommonBL();
        //    DataTable tcbl = new DataTable();
        //    tcbl = cbl.GetDateFinancila(Convert.ToString(Session["LastFinYear"]));

        //    System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();

        //    DataTable Month = new DataTable();
        //    Month.Columns.Add("MonthId", typeof(System.String));
        //    Month.Columns.Add("MonthName", typeof(System.String));
        //    DataRow MonthRow = Month.NewRow();
        //    //DateTime startMonth = ErpUser.FinYear_StartDate;
        //    DateTime startMonth = Convert.ToDateTime((tcbl.Rows[0]["FinYear_StartDate"]));
        //    //Rev Debashis
        //    //for (int i = 0; i < 12; i++)
        //    for (int i = 0; i < 24; i++)
        //    //End of Rev Debashis
        //    {
        //        string strMonthName = mfi.GetMonthName(startMonth.Month).ToString();
        //        string strYear = Convert.ToString(startMonth.Year);
        //        MonthRow = Month.NewRow();
        //        MonthRow["MonthId"] = startMonth.Month+"-"+strYear;
        //        MonthRow["MonthName"] = strMonthName + "-" +strYear;
        //        Month.Rows.Add(MonthRow);
        //        startMonth = startMonth.AddMonths(1);
        //    }
        //    return Month;

        //}

        public void Date_finyearwise(string Finyear)
        {
            CommonBL stkledg = new CommonBL();
            DataTable dtstkledg = new DataTable();

            dtstkledg = stkledg.GetDateFinancila(Finyear);
            if (dtstkledg.Rows.Count > 0)
            {
                ASPxFromDate.MaxDate = Convert.ToDateTime((dtstkledg.Rows[0]["FinYear_EndDate"]));
                ASPxFromDate.MinDate = Convert.ToDateTime((dtstkledg.Rows[0]["FinYear_StartDate"]));

                ASPxToDate.MaxDate = Convert.ToDateTime((dtstkledg.Rows[0]["FinYear_EndDate"]));
                ASPxToDate.MinDate = Convert.ToDateTime((dtstkledg.Rows[0]["FinYear_StartDate"]));

                DateTime MaximumDate = Convert.ToDateTime((dtstkledg.Rows[0]["FinYear_EndDate"]));
                DateTime MinimumDate = Convert.ToDateTime((dtstkledg.Rows[0]["FinYear_StartDate"]));

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
                    //Session["exportval"] = Filter;
                    // BindDropDownList();
                    bindexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportval"]) != Filter)
                {
                    // Session["exportval"] = Filter;
                    // BindDropDownASPxddlmonthList();
                    bindexport(Filter);
                }

                drdExport.SelectedValue = "0";
            }

        }

        public void bindexport(int Filter)
        {
            string filename = "TDS_Report";
            exporter.FileName = filename;
            string FileHeader = "";
            //string MonthName = "";
            //MonthName = ASPxddlmonth.Text;
            //MonthName = MonthName.Split('-')[0];
            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

            ////////05-06-2018 Subhra
            //if (Convert.ToString(ddlmonth.SelectedValue) == "ALL")
            //{
            //    MonthName = "All";
            //}
            //else if (Convert.ToString(ddlmonth.SelectedValue) == "1")
            //{
            //    MonthName = "January";
            //}
            //else if (Convert.ToString(ddlmonth.SelectedValue) == "2")
            //{
            //    MonthName = "February";
            //}
            //else if (Convert.ToString(ddlmonth.SelectedValue) == "3")
            //{
            //    MonthName = "March";
            //}
            //else if (Convert.ToString(ddlmonth.SelectedValue) == "4")
            //{
            //    MonthName = "April";
            //}
            //else if (Convert.ToString(ddlmonth.SelectedValue) == "5")
            //{
            //    MonthName = "May";
            //}
            //else if (Convert.ToString(ddlmonth.SelectedValue) == "6")
            //{
            //    MonthName = "June";
            //}
            //else if (Convert.ToString(ddlmonth.SelectedValue) == "7")
            //{
            //    MonthName = "July";
            //}
            //else if (Convert.ToString(ddlmonth.SelectedValue) == "8")
            //{
            //    MonthName = "August";
            //}
            //else if (Convert.ToString(ddlmonth.SelectedValue) == "9")
            //{
            //    MonthName = "September";
            //}
            //else if (Convert.ToString(ddlmonth.SelectedValue) == "10")
            //{
            //    MonthName = "October";
            //}
            //else if (Convert.ToString(ddlmonth.SelectedValue) == "11")
            //{
            //    MonthName = "November";
            //}
            //else if (Convert.ToString(ddlmonth.SelectedValue) == "12")
            //{
            //    MonthName = "December";
            //}
            ////////05-06-2018 Subhra

            //FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true) + Environment.NewLine + "TDS Report";

            //if (MonthName == "All")
            //{
            //    FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "TDS Report" + Environment.NewLine + "For all the months";
            //}
            //else
            //{
            //    FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "TDS Report" + Environment.NewLine + "For the month of " + MonthName + "";
            //}
            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "TDS Report" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
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
            exporter.GridViewID = "ShowGrid";
            exporter.RenderBrick += exporter_RenderBrick;
            switch (Filter)
            {
                case 1:
                    exporter.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
                    break;
                case 2:
                    exporter.WritePdfToResponse();
                    break;
                case 3:
                    exporter.WriteCsvToResponse();
                    break;
                case 4:
                    exporter.WriteRtfToResponse();
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

        protected void CallbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {

            //Session.Remove("dtLedger");
            //ShowGrid.JSProperties["cpSave"] = null;
            //  string[] CallVal = e.Parameters.ToString().Split('~');
            //string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            //string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);

            string IsTDSFilter = Convert.ToString(hfIsTDSFilter.Value);
            Session["IsTDSFilter"] = IsTDSFilter;

            //DateTime dtFrom;
            //DateTime dtTo;
            //string TABLENAME = "Ledger Details";

            dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
            dtTo = Convert.ToDateTime(ASPxToDate.Date);

            string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
            string TODATE = dtTo.ToString("yyyy-MM-dd");

            string BRANCH_ID = "";

            string QuoComponent = "";
            List<object> QuoList = lookup_branch.GridView.GetSelectedFieldValues("ID");
            foreach (object Quo in QuoList)
            {
                QuoComponent += "," + Quo;
            }
            BRANCH_ID = QuoComponent.TrimStart(',');

            //string CASHBANKTYPE = "";
            //string CASHBANKID = "";
            //string UserId = "";
            string CUSTVENDID = "";

            string QuoComponent2 = "";
            List<object> QuoList2 = gridvendorLookup.GridView.GetSelectedFieldValues("ID");
            foreach (object Quo2 in QuoList2)
            {
                QuoComponent2 += "," + Quo2;
            }
            CUSTVENDID = QuoComponent2.TrimStart(',');

            //string LEDGERID = "";
            //string ISCREATEORPREVIEW = "P";
            /// GetTdSReport(BRANCH_ID, CUSTVENDID,ddlmonth.SelectedValue);

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
            //Task PopulateStockTrialDataTask = new Task(() => GetTdSReport(BRANCH_ID, CUSTVENDID,ASPxddlmonth.Value));
            Task PopulateStockTrialDataTask = new Task(() => GetTdSReport(BRANCH_ID, CUSTVENDID, FROMDATE, TODATE));
            PopulateStockTrialDataTask.RunSynchronously();
        }

        //public void GetTdSReport(string BRANCH_ID,string CUSTVENDID, object MonthYear)
        public void GetTdSReport(string BRANCH_ID, string CUSTVENDID, string FROMDATE, string TODATE)
        {
            try
            {
                //string DriverName = string.Empty;
                //string PhoneNo = string.Empty;
                //string VehicleNo = string.Empty;

                //string monthyear = Convert.ToString(MonthYear);
                //string[] monthyears = monthyear.Split('-');
                //string Month = Convert.ToString(monthyears[0]);
                //string Year = Convert.ToString(monthyears[1]);

                DataSet ds = new DataSet();
                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("prc_TDS_Report", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]).Trim());
                cmd.Parameters.AddWithValue("@BRANCHID", BRANCH_ID);
                cmd.Parameters.AddWithValue("@VENDOR", CUSTVENDID);
                //cmd.Parameters.AddWithValue("@MONTH", Month);
                //cmd.Parameters.AddWithValue("@YEAR", Year);
                cmd.Parameters.AddWithValue("@STARTDATE", FROMDATE);
                cmd.Parameters.AddWithValue("@ENDDATE", TODATE);
                cmd.Parameters.AddWithValue("@SELECTSECTION", ddlsection.SelectedValue);
                cmd.Parameters.AddWithValue("@TDSREGTYPE", ddlTDSRegSelection.SelectedValue);
                cmd.Parameters.AddWithValue("@CONSDEPOSITDATE", (chkConsDepsitOn.Checked) ? "1" : "0");
                cmd.Parameters.AddWithValue("@SHOWREMARKS", (chkRemarks.Checked) ? "1" : "0");
                cmd.Parameters.AddWithValue("@CONSIDEROP", (chkConsOP.Checked) ? "1" : "0");
                cmd.Parameters.AddWithValue("@USERID", Convert.ToInt32(Session["userid"]));

                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);

                cmd.Dispose();
                con.Dispose();

                //Session["dtLedger"] = ds.Tables[0];

                //ShowGrid.DataSource = ds.Tables[0];
                //ShowGrid.DataBind();
            }
            catch (Exception ex)
            {
            }
        }

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




        //protected void ShowGrid_DataBinding(object sender, EventArgs e)
        //{
        //    if (Session["dtLedger"] != null)
        //    {
        //        ShowGrid.DataSource = (DataTable)Session["dtLedger"];
        //        //  ShowGrid.DataBind();
        //    }

        //}
        protected void ShowGrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            //e.Text = string.Format("{0}", e.Value);
            if (Convert.ToString(Session["IsTDSFilter"]) == "Y")
            {
                dtDocumentTotal = oDBEngine.GetDataTable("SELECT COUNT(DISTINCT DOCUMENT_NO) FROM TDS_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " ");
                TotDocCount = dtDocumentTotal.Rows[0][0].ToString();
            }

            string summaryTAG = (e.Item as ASPxSummaryItem).Tag;
            if (e.IsTotalSummary == true)
            {
                switch (summaryTAG)
                {
                    case "DocCount":
                        e.Text = "No. of Document(s)="+ TotDocCount;
                        break;
                }
            }
            if (e.Item != ShowGrid.TotalSummary["DOCUMENT_NO"])
            {
                e.Text = string.Format("{0}", e.Value);
            }            
        }

        public void Getsectionpopulate()
        {
            DataTable dtbl = oDBEngine.GetDataTable("select distinct RTrim(TDSTCS_Code)  as TDSTCS_Code from Master_TDSTCS");
            ddlsection.DataSource = dtbl;
            ddlsection.DataTextField = "TDSTCS_Code";
            ddlsection.DataValueField = "TDSTCS_Code";
            ddlsection.DataBind();
            ddlsection.Items.Insert(0, new ListItem("All", ""));

        }

        #region Branch Populate

        protected void Componentbranch_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string FinYear = Convert.ToString(Session["LastFinYear"]);

            if (e.Parameter.Split('~')[0] == "BindComponentGrid")
            {
                DataTable ComponentTable = new DataTable();
                string Hoid = e.Parameter.Split('~')[1];
                if (Session["userbranchHierarchy"] != null)
                {
                    ComponentTable = oDBEngine.GetDataTable("select branch_id as ID,branch_description,branch_code from tbl_master_branch   where branch_id in(" + Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]) + ")   order by branch_description asc");
                }
                if (ComponentTable.Rows.Count > 0)
                {

                    Session["SI_ComponentData_Branch"] = ComponentTable;
                    lookup_branch.DataSource = ComponentTable;
                    lookup_branch.DataBind();
                }
                else
                {
                    Session["SI_ComponentData_Branch"] = ComponentTable;
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

        #endregion


        #region  vendor Populate

        protected void Componentvendor_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {

            string FinYear = Convert.ToString(Session["LastFinYear"]);

            if (e.Parameter.Split('~')[0] == "BindComponentGrid")
            {
                string type = e.Parameter.Split('~')[1];

                DataTable ComponentTable = new DataTable();

                //Rev Debashis
                //ComponentTable = oDBEngine.GetDataTable("select cnt_internalId AS ID,ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Name'  FROM tbl_master_contact WHERE cnt_contactType in('DV') ORDER BY ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'') ");
                // Rev Maynak 30-10-2019 0021156
                //ComponentTable = oDBEngine.GetDataTable("select cnt_internalId AS ID,ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Name'  FROM tbl_master_contact WHERE cnt_contactType in('DV','RA') ORDER BY ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'') ");
                //Rev Debashis
                //ComponentTable = oDBEngine.GetDataTable("select cnt_internalId AS ID,ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Name'  FROM tbl_master_contact WHERE cnt_contactType in('DV','TR','RA','EM') ORDER BY ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'') ");
                ComponentTable = oDBEngine.GetDataTable("SELECT ID,Name FROM(select cnt_internalId AS ID,ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Name'  FROM tbl_master_contact WHERE cnt_contactType in('DV','TR','RA','EM') UNION ALL SELECT MainAccount_AccountCode AS ID,MainAccount_Name AS 'Name' FROM Master_MainAccount ) AS DEDUCTEE ORDER BY Name ");
                //End of Rev Debashis
                // End of Rev Maynak
                //End of Rev Debashis

                if (ComponentTable.Rows.Count > 0)
                {
                    Session["SI_ComponentData"] = ComponentTable;
                    gridvendorLookup.DataSource = ComponentTable;
                    gridvendorLookup.DataBind();
                }
                else
                {
                    Session["SI_ComponentData"] = null;
                    gridvendorLookup.DataSource = null;
                    gridvendorLookup.DataBind();

                }
            }
        }

        protected void lookup_vendor_DataBinding(object sender, EventArgs e)
        {
            //Rev Debashis
            //DataTable ComponentTable = oDBEngine.GetDataTable("select cnt_internalId AS ID,ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Name'  FROM tbl_master_contact WHERE cnt_contactType in('DV') ORDER BY ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'') ");
            //Rev Maynak 05-11-2019 0021156
            //DataTable ComponentTable = oDBEngine.GetDataTable("select cnt_internalId AS ID,ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Name'  FROM tbl_master_contact WHERE cnt_contactType in('DV','RA') ORDER BY ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'') ");
            //Rev Debashis
            //DataTable ComponentTable = oDBEngine.GetDataTable("select cnt_internalId AS ID,ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Name'  FROM tbl_master_contact WHERE cnt_contactType in('DV','TR','RA','EM') ORDER BY ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'') ");
            DataTable ComponentTable = oDBEngine.GetDataTable("SELECT ID,Name FROM(select cnt_internalId AS ID,ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Name'  FROM tbl_master_contact WHERE cnt_contactType in('DV','TR','RA','EM') UNION ALL SELECT MainAccount_AccountCode AS ID,MainAccount_Name AS 'Name' FROM Master_MainAccount ) AS DEDUCTEE ORDER BY Name ");
            //End of Rev Debashis
            //End of Rev Maynak            
            //End of Rev Debashis
            gridvendorLookup.DataSource = ComponentTable;
        }

        #endregion

        #region Linq
        protected void GenerateEntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SEQ";
            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsTDSFilter"]) == "Y")
            {
                var q = from d in dc.TDS_REPORTs
                        where Convert.ToString(d.USERID) == Userid
                        orderby d.SEQ ascending
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.TDS_REPORTs
                        where Convert.ToString(d.SEQ) == "0"
                        orderby d.SEQ ascending
                        select d;
                e.QueryableSource = q;
            }

            string strremarkschk = (chkRemarks.Checked) ? "1" : "0";
            if (Convert.ToString(strremarkschk) == "0")
            {
                //ShowGrid.Columns[7].Visible = false;
                ShowGrid.Columns[9].Visible = false;
            }
            else
            {
                //ShowGrid.Columns[7].Visible = true;
                ShowGrid.Columns[9].Visible = true;
            }
        }
        #endregion
    }
}