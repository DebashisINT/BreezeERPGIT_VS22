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
    public partial class SRVPerformanceStatusDayWise : System.Web.UI.Page
    {
        BusinessLogicLayer.Reports objReport = new BusinessLogicLayer.Reports();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        ExcelFile objExcel = new ExcelFile();
        DataTable dtExport = new DataTable();
        DataTable dtReportHeader = new DataTable();
        DataTable dtReportFooter = new DataTable();

        DataTable dtBranchTotal = null;
        string BranchDesc = "";
        string Lessthan1daytot = "";
        string day1tot = "";
        string day2tot = "";
        string day3tot = "";
        string day4tot = "";
        string day5tot = "";
        string day6tot = "";
        string day7tot = "";
        string day8tot = "";
        string day9tot = "";
        string day10tot = "";
        string day11tot = "";
        string day12tot = "";
        string morethan12daystot = "";
        string Totaldays = "";

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
            rights = new UserRightsForPage();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/GridReports/SRVPerformanceStatusDayWise.aspx");
            DateTime dtFrom;
            DateTime dtTo;
            if (!IsPostBack)
            {
                DataTable dtBranchSelection = new DataTable();
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "TAT - Day(s) Wise";
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

                Session["SI_ComponentData_Branch"] = null;
                Session["IsSrvPerformanceStatDayWiseFilter"] = null;
                BranchHoOffice();

                dtFrom = DateTime.Now;
                dtTo = DateTime.Now;

                ASPxFromDate.Value = DateTime.Now;
                ASPxToDate.Value = DateTime.Now;
                Date_finyearwise(Convert.ToString(Session["LastFinYear"]));
                Session["BranchNames"] = null;
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
                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);
                string BRANCH_ID = "";
                if (hdnSelectedBranches.Value != "")
                {
                    BRANCH_ID = hdnSelectedBranches.Value;
                }
            }
        }

        public void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Convert.ToString(Session["IsSrvPerformanceStatDayWiseFilter"]) == "Y")
            {
                if (Filter != 0)
                {
                    bindexport(Filter);
                }
            }
            else
            {
                BranchHoOffice();
            }
        }

        public void bindexport(int Filter)
        {
            string filename = "TATDayWise";
            exporter.FileName = filename;

            if (Filter == 1 || Filter == 2)
            {
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                con.Open();
                string selectQuery = "SELECT BRANCHDESC,LESSTHAN1DAY,DAY1,DAY2,DAY3,DAY4,DAY5,DAY6,DAY7,DAY8,DAY9,DAY10,DAY11,DAY12,MORETHANDAY12,TOTALDAYS FROM SRVPERFORMANCESTATDAYWISE_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND BRANCHDESC<>'Total :' AND BRANCHDESCORDBY<>'ZZZZZZZZZZZZZZ' order by SEQ";
                SqlDataAdapter myCommand = new SqlDataAdapter(selectQuery, con);

                // Create and fill a DataSet.
                DataSet ds = new DataSet();
                myCommand.Fill(ds, "Main");
                myCommand = new SqlDataAdapter("SELECT BRANCHDESC,LESSTHAN1DAY,DAY1,DAY2,DAY3,DAY4,DAY5,DAY6,DAY7,DAY8,DAY9,DAY10,DAY11,DAY12,MORETHANDAY12,TOTALDAYS FROM SRVPERFORMANCESTATDAYWISE_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND BRANCHDESC='Total :' AND BRANCHDESCORDBY='ZZZZZZZZZZZZZZ' ", con);
                myCommand.Fill(ds, "GrossTotal");
                myCommand.Dispose();
                con.Dispose();
                Session["exportperformancestatdwdataset"] = ds;

                dtExport = ds.Tables[0].Copy();
                dtExport.Clear();
                dtExport.Columns.Add(new DataColumn("Unit", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Less Than 1 (< 1) Day", typeof(Int32)));
                dtExport.Columns.Add(new DataColumn("1 Day", typeof(Int32)));
                dtExport.Columns.Add(new DataColumn("2 Days", typeof(Int32)));
                dtExport.Columns.Add(new DataColumn("3 Days", typeof(Int32)));
                dtExport.Columns.Add(new DataColumn("4 Days", typeof(Int32)));
                dtExport.Columns.Add(new DataColumn("5 Days", typeof(Int32)));
                dtExport.Columns.Add(new DataColumn("6 Days", typeof(Int32)));
                dtExport.Columns.Add(new DataColumn("7 Days", typeof(Int32)));
                dtExport.Columns.Add(new DataColumn("8 Days", typeof(Int32)));
                dtExport.Columns.Add(new DataColumn("9 Days", typeof(Int32)));
                dtExport.Columns.Add(new DataColumn("10 Days", typeof(Int32)));
                dtExport.Columns.Add(new DataColumn("11 Days", typeof(Int32)));
                dtExport.Columns.Add(new DataColumn("12 Days", typeof(Int32)));
                dtExport.Columns.Add(new DataColumn("More Than 12 (>12) Days", typeof(Int32)));
                dtExport.Columns.Add(new DataColumn("Total Days", typeof(Int32)));

                foreach (DataRow dr1 in ds.Tables[0].Rows)
                {
                    DataRow row2 = dtExport.NewRow();

                    row2["Unit"] = dr1["BRANCHDESC"];
                    row2["Less Than 1 (< 1) Day"] = dr1["LESSTHAN1DAY"];
                    row2["1 Day"] = dr1["DAY1"];
                    row2["2 Days"] = dr1["DAY2"];
                    row2["3 Days"] = dr1["DAY3"];
                    row2["4 Days"] = dr1["DAY4"];
                    row2["5 Days"] = dr1["DAY5"];
                    row2["6 Days"] = dr1["DAY6"];
                    row2["7 Days"] = dr1["DAY7"];
                    row2["8 Days"] = dr1["DAY8"];
                    row2["9 Days"] = dr1["DAY9"];
                    row2["10 Days"] = dr1["DAY10"];
                    row2["11 Days"] = dr1["DAY11"];
                    row2["12 Days"] = dr1["DAY12"];
                    row2["More Than 12 (>12) Days"] = dr1["MORETHANDAY12"];
                    row2["Total Days"] = dr1["TOTALDAYS"];

                    dtExport.Rows.Add(row2);
                }

                dtExport.Columns.Remove("BRANCHDESC");
                dtExport.Columns.Remove("LESSTHAN1DAY");
                dtExport.Columns.Remove("DAY1");
                dtExport.Columns.Remove("DAY2");
                dtExport.Columns.Remove("DAY3");
                dtExport.Columns.Remove("DAY4");
                dtExport.Columns.Remove("DAY5");
                dtExport.Columns.Remove("DAY6");
                dtExport.Columns.Remove("DAY7");
                dtExport.Columns.Remove("DAY8");
                dtExport.Columns.Remove("DAY9");
                dtExport.Columns.Remove("DAY10");
                dtExport.Columns.Remove("DAY11");
                dtExport.Columns.Remove("DAY12");
                dtExport.Columns.Remove("MORETHANDAY12");
                dtExport.Columns.Remove("TOTALDAYS");

                DataRow row3 = dtExport.NewRow();
                row3["Unit"] = ds.Tables[1].Rows[0]["BRANCHDESC"].ToString();
                row3["Less Than 1 (< 1) Day"] = ds.Tables[1].Rows[0]["LESSTHAN1DAY"].ToString();
                row3["1 Day"] = ds.Tables[1].Rows[0]["DAY1"].ToString();
                row3["2 Days"] = ds.Tables[1].Rows[0]["DAY2"].ToString();
                row3["3 Days"] = ds.Tables[1].Rows[0]["DAY3"].ToString();
                row3["4 Days"] = ds.Tables[1].Rows[0]["DAY4"].ToString();
                row3["5 Days"] = ds.Tables[1].Rows[0]["DAY5"].ToString();
                row3["6 Days"] = ds.Tables[1].Rows[0]["DAY6"].ToString();
                row3["7 Days"] = ds.Tables[1].Rows[0]["DAY7"].ToString();
                row3["8 Days"] = ds.Tables[1].Rows[0]["DAY8"].ToString();
                row3["9 Days"] = ds.Tables[1].Rows[0]["DAY9"].ToString();
                row3["10 Days"] = ds.Tables[1].Rows[0]["DAY10"].ToString();
                row3["11 Days"] = ds.Tables[1].Rows[0]["DAY11"].ToString();
                row3["12 Days"] = ds.Tables[1].Rows[0]["DAY12"].ToString();
                row3["More Than 12 (>12) Days"] = ds.Tables[1].Rows[0]["MORETHANDAY12"].ToString();
                row3["Total Days"] = ds.Tables[1].Rows[0]["TOTALDAYS"].ToString();
                dtExport.Rows.Add(row3);

                //For Excel/PDF Header
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String)));

                string GridHeader = "";
                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, false, false, false, false, false);
                DataRow HeaderRow = dtReportHeader.NewRow();
                HeaderRow[0] = GridHeader.ToString();
                dtReportHeader.Rows.Add(HeaderRow);
                DataRow HeaderRow1 = dtReportHeader.NewRow();
                HeaderRow1[0] = Convert.ToString(Session["BranchNames"]);
                dtReportHeader.Rows.Add(HeaderRow1);
                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, true, false, false, false, false);
                DataRow HeaderRow2 = dtReportHeader.NewRow();
                HeaderRow2[0] = GridHeader.ToString();
                dtReportHeader.Rows.Add(HeaderRow2);
                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, false, true, false, false, false);
                DataRow HeaderRow3 = dtReportHeader.NewRow();
                HeaderRow3[0] = GridHeader.ToString();
                dtReportHeader.Rows.Add(HeaderRow3);
                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, false, false, true, false, false);
                DataRow HeaderRow4 = dtReportHeader.NewRow();
                HeaderRow4[0] = GridHeader.ToString();
                dtReportHeader.Rows.Add(HeaderRow4);
                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, false, false, false, false, true);
                DataRow HeaderRow5 = dtReportHeader.NewRow();
                HeaderRow5[0] = GridHeader.ToString();
                dtReportHeader.Rows.Add(HeaderRow5);
                DataRow HeaderRow6 = dtReportHeader.NewRow();
                HeaderRow6[0] = "TAT - Day(s) Wise";
                dtReportHeader.Rows.Add(HeaderRow6);
                DataRow HeaderRow7 = dtReportHeader.NewRow();
                HeaderRow7[0] = "For the period: " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
                dtReportHeader.Rows.Add(HeaderRow7);

                //For Excel/PDF Footer
                dtReportFooter.Columns.Add(new DataColumn("Footer", typeof(String))); //0
                DataRow FooterRow1 = dtReportFooter.NewRow();
                dtReportFooter.Rows.Add(FooterRow1);
                DataRow FooterRow2 = dtReportFooter.NewRow();
                dtReportFooter.Rows.Add(FooterRow2);
                DataRow FooterRow = dtReportFooter.NewRow();
                FooterRow[0] = "* * *  End Of Report * * *   ";
                dtReportFooter.Rows.Add(FooterRow);
            }
            else
            {
                exporter.PageHeader.Font.Size = 10;
                exporter.PageHeader.Font.Name = "Tahoma";
                exporter.GridViewID = "ShowGrid";
            }

            switch (Filter)
            {
                case 1:
                    objExcel.ExportToExcelforExcel(dtExport, "TATDayWise", "ZZZZZZZZZZZZZZZZZZZZ", "ZZZZZZZZZZZZZZZZZZZZ", dtReportHeader, dtReportFooter);
                    break;
                case 2:
                    objExcel.ExportToPDF(dtExport, "TATDayWise", "ZZZZZZZZZZZZZZZZZZZZ", "ZZZZZZZZZZZZZZZZZZZZ", dtReportHeader, dtReportFooter);
                    break;
                case 3:
                    exporter.WriteCsvToResponse();
                    break;
                default:
                    return;
            }
        }

        protected void CallbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string returnPara = Convert.ToString(e.Parameter);
            string HEAD_BRANCH = returnPara.Split('~')[1];

            string IsSrvPerformanceStatDayWiseFilter = Convert.ToString(hfIsSrvPerformanceStatDayWiseFilter.Value);
            Session["IsSrvPerformanceStatDayWiseFilter"] = IsSrvPerformanceStatDayWiseFilter;

            DateTime dtFrom;
            DateTime dtTo;

            dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
            dtTo = Convert.ToDateTime(ASPxToDate.Date);

            string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
            string TODATE = dtTo.ToString("yyyy-MM-dd");
            string BRANCH_ID = "";

            string BranComponent = "";
            List<object> BranList = lookup_branch.GridView.GetSelectedFieldValues("ID");
            foreach (object Bran in BranList)
            {
                BranComponent += "," + Bran;
            }
            BRANCH_ID = BranComponent.TrimStart(',');

            string BRANCH_NAME = "";
            string BranchNameComponent = "";
            List<object> BranchNameList = lookup_branch.GridView.GetSelectedFieldValues("branch_description");
            foreach (object BranchName in BranchNameList)
            {
                BranchNameComponent += "," + BranchName;
            }
            if (BranchNameList.Count > 1 || BranchNameList.Count == 0)
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

            Task PopulateStockTrialDataTask = new Task(() => GetPerformanceStatDayWisedata(FROMDATE, TODATE, BRANCH_ID, HEAD_BRANCH));
            PopulateStockTrialDataTask.RunSynchronously();
        }
        public void GetPerformanceStatDayWisedata(string FROMDATE, string TODATE, string BRANCH_ID, string HEAD_BRANCH)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_SRVPERFORMANCESTATDAYWISE_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@FROMDATE", FROMDATE);
                cmd.Parameters.AddWithValue("@TODATE", TODATE);
                cmd.Parameters.AddWithValue("@HO", HEAD_BRANCH);
                cmd.Parameters.AddWithValue("@BRANCHID", BRANCH_ID);
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

        #region LinQ
        protected void GenerateEntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SEQ";
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsSrvPerformanceStatDayWiseFilter"]) == "Y")
            {
                var q = from d in dc.SRVPERFORMANCESTATDAYWISE_REPORTs
                        where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.BRANCHDESC) != "Total :" && Convert.ToString(d.BRANCHDESCORDBY) != "ZZZZZZZZZZZZZZ"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.SRVPERFORMANCESTATDAYWISE_REPORTs
                        where Convert.ToString(d.SEQ) == "0"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
        }

        #endregion

        public void Date_finyearwise(string Finyear)
        {
            CommonBL salereg = new CommonBL();
            DataTable dtsalereg = new DataTable();

            dtsalereg = salereg.GetDateFinancila(Finyear);
            if (dtsalereg.Rows.Count > 0)
            {
                ASPxFromDate.MaxDate = Convert.ToDateTime((dtsalereg.Rows[0]["FinYear_EndDate"]));
                ASPxFromDate.MinDate = Convert.ToDateTime((dtsalereg.Rows[0]["FinYear_StartDate"]));

                ASPxToDate.MaxDate = Convert.ToDateTime((dtsalereg.Rows[0]["FinYear_EndDate"]));
                ASPxToDate.MinDate = Convert.ToDateTime((dtsalereg.Rows[0]["FinYear_StartDate"]));

                DateTime MaximumDate = Convert.ToDateTime((dtsalereg.Rows[0]["FinYear_EndDate"]));
                DateTime MinimumDate = Convert.ToDateTime((dtsalereg.Rows[0]["FinYear_StartDate"]));

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

        public void BranchHoOffice()
        {
            CommonBL bll1 = new CommonBL();
            DataTable stbill = new DataTable();
            DataTable dtBranchChild = new DataTable();
            stbill = bll1.GetBranchheadoffice(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), "HO");
            if (stbill.Rows.Count > 0)
            {
                ddlbranchHO.DataSource = stbill;
                ddlbranchHO.DataTextField = "Code";
                ddlbranchHO.DataValueField = "branch_id";
                ddlbranchHO.DataBind();
                dtBranchChild = GetChildBranch(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
                if (dtBranchChild.Rows.Count > 0)
                {
                    ddlbranchHO.Items.Insert(0, new ListItem("All", "All"));
                }
            }
        }

        public DataTable GetChildBranch(string CHILDBRANCH)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("PRC_FINDCHILDBRANCH_REPORT", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CHILDBRANCH", CHILDBRANCH);
            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dt);
            cmd.Dispose();
            con.Dispose();
            return dt;
        }

        protected void ShowGrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            if (Convert.ToString(Session["IsSrvPerformanceStatDayWiseFilter"]) == "Y")
            {
                dtBranchTotal = oDBEngine.GetDataTable("Select BRANCHDESC,LESSTHAN1DAY,DAY1,DAY2,DAY3,DAY4,DAY5,DAY6,DAY7,DAY8,DAY9,DAY10,DAY11,DAY12,MORETHANDAY12,TOTALDAYS from SRVPERFORMANCESTATDAYWISE_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND BRANCHDESC='Total :' AND BRANCHDESCORDBY='ZZZZZZZZZZZZZZ'");
                BranchDesc = dtBranchTotal.Rows[0][0].ToString();
                Lessthan1daytot = dtBranchTotal.Rows[0][1].ToString();
                day1tot = dtBranchTotal.Rows[0][2].ToString();
                day2tot = dtBranchTotal.Rows[0][3].ToString();
                day3tot = dtBranchTotal.Rows[0][4].ToString();
                day4tot = dtBranchTotal.Rows[0][5].ToString();
                day5tot = dtBranchTotal.Rows[0][6].ToString();
                day6tot = dtBranchTotal.Rows[0][7].ToString();
                day7tot = dtBranchTotal.Rows[0][8].ToString();
                day8tot = dtBranchTotal.Rows[0][9].ToString();
                day9tot = dtBranchTotal.Rows[0][10].ToString();
                day10tot = dtBranchTotal.Rows[0][11].ToString();
                day11tot = dtBranchTotal.Rows[0][12].ToString();
                day12tot = dtBranchTotal.Rows[0][13].ToString();
                morethan12daystot = dtBranchTotal.Rows[0][14].ToString();
                Totaldays = dtBranchTotal.Rows[0][15].ToString();
            }

            string summaryTAG = (e.Item as ASPxSummaryItem).Tag;
            if (e.IsTotalSummary == true)
            {
                switch (summaryTAG)
                {
                    case "Item_Branch":
                        e.Text = BranchDesc;
                        break;
                    case "Item_Less1Day":
                        e.Text = Lessthan1daytot;
                        break;
                    case "Item_Day1":
                        e.Text = day1tot;
                        break;
                    case "Item_Day2":
                        e.Text = day2tot;
                        break;
                    case "Item_Day3":
                        e.Text = day3tot;
                        break;
                    case "Item_Day4":
                        e.Text = day4tot;
                        break;
                    case "Item_Day5":
                        e.Text = day5tot;
                        break;
                    case "Item_Day6":
                        e.Text = day6tot;
                        break;
                    case "Item_Day7":
                        e.Text = day7tot;
                        break;
                    case "Item_Day8":
                        e.Text = day8tot;
                        break;
                    case "Item_Day9":
                        e.Text = day9tot;
                        break;
                    case "Item_Day10":
                        e.Text = day10tot;
                        break;
                    case "Item_Day11":
                        e.Text = day11tot;
                        break;
                    case "Item_Day12":
                        e.Text = day12tot;
                        break;
                    case "Item_MorethanDay12":
                        e.Text = morethan12daystot;
                        break;
                    case "Item_TotDays":
                        e.Text = Totaldays;
                        break;
                }
            }
        }

        #region Branch Populate

        protected void Componentbranch_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string FinYear = Convert.ToString(Session["LastFinYear"]);

            if (e.Parameter.Split('~')[0] == "BindComponentGrid")
            {
                DataTable ComponentTable = new DataTable();
                string Hoid = e.Parameter.Split('~')[1];
                if (Hoid != "All")
                {
                    ComponentTable = GetBranch(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), Hoid);

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
                else
                {
                    ComponentTable = oDBEngine.GetDataTable("select * from (select branch_id as ID,branch_description,branch_code from tbl_master_branch a where a.branch_id=1  union all select branch_id as ID,branch_description,branch_code from tbl_master_branch b where b.branch_parentId=1) a order by branch_description");

                    if (ComponentTable.Rows.Count > 0)
                    {
                        Session["SI_ComponentData_Branch"] = ComponentTable;
                        lookup_branch.DataSource = ComponentTable;
                        lookup_branch.DataBind();
                    }
                }
            }
        }

        public DataTable GetBranch(string BRANCH_ID, string Ho)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("GetFinancerBranchfetchhowise", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Branch", BRANCH_ID);
            cmd.Parameters.AddWithValue("@Hoid", Ho);
            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dt);
            cmd.Dispose();
            con.Dispose();

            return dt;
        }

        protected void lookup_branch_DataBinding(object sender, EventArgs e)
        {
            if (Session["SI_ComponentData_Branch"] != null)
            {
                lookup_branch.DataSource = (DataTable)Session["SI_ComponentData_Branch"];
            }
        }

        #endregion
    }
}