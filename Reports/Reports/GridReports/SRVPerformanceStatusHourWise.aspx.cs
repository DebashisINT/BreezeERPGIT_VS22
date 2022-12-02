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
    public partial class SRVPerformanceStatusHourWise : System.Web.UI.Page
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
        string Lessthan1hrtot = "";
        string hr1tot = "";
        string hrs2tot = "";
        string hrs3tot = "";
        string hrs4tot = "";
        string hrs5tot = "";
        string hrs6tot = "";
        string hrs7tot = "";
        string hrs8tot = "";
        string hrs9tot = "";
        string hrs10tot = "";
        string hrs11tot = "";
        string hrs12tot = "";
        string morethan12hrstot = "";
        string Totalhrs = "";

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
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/GridReports/SRVPerformanceStatusHourWise.aspx");
            DateTime dtFrom;
            DateTime dtTo;
            if (!IsPostBack)
            {
                DataTable dtBranchSelection = new DataTable();
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "TAT - Hour(s) Wise";
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
                Session["IsSrvPerformanceStatHrWiseFilter"] = null;
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
            if (Convert.ToString(Session["IsSrvPerformanceStatHrWiseFilter"]) == "Y")
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
            string filename = "TATHrWise";
            exporter.FileName = filename;

            if (Filter == 1 || Filter == 2)
            {
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                con.Open();
                string selectQuery = "SELECT BRANCHDESC,LESSTHAN1HR,HR1,HRS2,HRS3,HRS4,HRS5,HRS6,HRS7,HRS8,HRS9,HRS10,HRS11,HRS12,MORETHANHRS12,TOTALHRS FROM SRVPERFORMANCESTATHRSWISE_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND BRANCHDESC<>'Total :' AND BRANCHDESCORDBY<>'ZZZZZZZZZZZZZZ' order by SEQ";
                SqlDataAdapter myCommand = new SqlDataAdapter(selectQuery, con);

                // Create and fill a DataSet.
                DataSet ds = new DataSet();
                myCommand.Fill(ds, "Main");
                myCommand = new SqlDataAdapter("SELECT BRANCHDESC,LESSTHAN1HR,HR1,HRS2,HRS3,HRS4,HRS5,HRS6,HRS7,HRS8,HRS9,HRS10,HRS11,HRS12,MORETHANHRS12,TOTALHRS FROM SRVPERFORMANCESTATHRSWISE_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND BRANCHDESC='Total :' AND BRANCHDESCORDBY='ZZZZZZZZZZZZZZ' ", con);
                myCommand.Fill(ds, "GrossTotal");
                myCommand.Dispose();
                con.Dispose();
                Session["exportperformancestathrwdataset"] = ds;

                dtExport = ds.Tables[0].Copy();
                dtExport.Clear();
                dtExport.Columns.Add(new DataColumn("Unit", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Less Than 1 (< 1) Hour", typeof(Int32)));
                dtExport.Columns.Add(new DataColumn("1 Hour", typeof(Int32)));
                dtExport.Columns.Add(new DataColumn("2 Hrs.", typeof(Int32)));
                dtExport.Columns.Add(new DataColumn("3 Hrs.", typeof(Int32)));
                dtExport.Columns.Add(new DataColumn("4 Hrs.", typeof(Int32)));
                dtExport.Columns.Add(new DataColumn("5 Hrs.", typeof(Int32)));
                dtExport.Columns.Add(new DataColumn("6 Hrs.", typeof(Int32)));
                dtExport.Columns.Add(new DataColumn("7 Hrs.", typeof(Int32)));
                dtExport.Columns.Add(new DataColumn("8 Hrs.", typeof(Int32)));
                dtExport.Columns.Add(new DataColumn("9 Hrs.", typeof(Int32)));
                dtExport.Columns.Add(new DataColumn("10 Hrs.", typeof(Int32)));
                dtExport.Columns.Add(new DataColumn("11 Hrs.", typeof(Int32)));
                dtExport.Columns.Add(new DataColumn("12 Hrs.", typeof(Int32)));
                dtExport.Columns.Add(new DataColumn("More Than 12 (>12) Hrs.", typeof(Int32)));
                dtExport.Columns.Add(new DataColumn("Total Hrs.", typeof(Int32)));

                foreach (DataRow dr1 in ds.Tables[0].Rows)
                {
                    DataRow row2 = dtExport.NewRow();

                    row2["Unit"] = dr1["BRANCHDESC"];
                    row2["Less Than 1 (< 1) Hour"] = dr1["LESSTHAN1HR"];
                    row2["1 Hour"] = dr1["HR1"];
                    row2["2 Hrs."] = dr1["HRS2"];
                    row2["3 Hrs."] = dr1["HRS3"];
                    row2["4 Hrs."] = dr1["HRS4"];
                    row2["5 Hrs."] = dr1["HRS5"];
                    row2["6 Hrs."] = dr1["HRS6"];
                    row2["7 Hrs."] = dr1["HRS7"];
                    row2["8 Hrs."] = dr1["HRS8"];
                    row2["9 Hrs."] = dr1["HRS9"];
                    row2["10 Hrs."] = dr1["HRS10"];
                    row2["11 Hrs."] = dr1["HRS11"];
                    row2["12 Hrs."] = dr1["HRS12"];
                    row2["More Than 12 (>12) Hrs."] = dr1["MORETHANHRS12"];
                    row2["Total Hrs."] = dr1["TOTALHRS"];

                    dtExport.Rows.Add(row2);
                }

                dtExport.Columns.Remove("BRANCHDESC");
                dtExport.Columns.Remove("LESSTHAN1HR");
                dtExport.Columns.Remove("HR1");
                dtExport.Columns.Remove("HRS2");
                dtExport.Columns.Remove("HRS3");
                dtExport.Columns.Remove("HRS4");
                dtExport.Columns.Remove("HRS5");
                dtExport.Columns.Remove("HRS6");
                dtExport.Columns.Remove("HRS7");
                dtExport.Columns.Remove("HRS8");
                dtExport.Columns.Remove("HRS9");
                dtExport.Columns.Remove("HRS10");
                dtExport.Columns.Remove("HRS11");
                dtExport.Columns.Remove("HRS12");
                dtExport.Columns.Remove("MORETHANHRS12");
                dtExport.Columns.Remove("TOTALHRS");

                DataRow row3 = dtExport.NewRow();
                row3["Unit"] = ds.Tables[1].Rows[0]["BRANCHDESC"].ToString();
                row3["Less Than 1 (< 1) Hour"] = ds.Tables[1].Rows[0]["LESSTHAN1HR"].ToString();
                row3["1 Hour"] = ds.Tables[1].Rows[0]["HR1"].ToString();
                row3["2 Hrs."] = ds.Tables[1].Rows[0]["HRS2"].ToString();
                row3["3 Hrs."] = ds.Tables[1].Rows[0]["HRS3"].ToString();
                row3["4 Hrs."] = ds.Tables[1].Rows[0]["HRS4"].ToString();
                row3["5 Hrs."] = ds.Tables[1].Rows[0]["HRS5"].ToString();
                row3["6 Hrs."] = ds.Tables[1].Rows[0]["HRS6"].ToString();
                row3["7 Hrs."] = ds.Tables[1].Rows[0]["HRS7"].ToString();
                row3["8 Hrs."] = ds.Tables[1].Rows[0]["HRS8"].ToString();
                row3["9 Hrs."] = ds.Tables[1].Rows[0]["HRS9"].ToString();
                row3["10 Hrs."] = ds.Tables[1].Rows[0]["HRS10"].ToString();
                row3["11 Hrs."] = ds.Tables[1].Rows[0]["HRS11"].ToString();
                row3["12 Hrs."] = ds.Tables[1].Rows[0]["HRS12"].ToString();
                row3["More Than 12 (>12) Hrs."] = ds.Tables[1].Rows[0]["MORETHANHRS12"].ToString();
                row3["Total Hrs."] = ds.Tables[1].Rows[0]["TOTALHRS"].ToString();
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
                HeaderRow6[0] = "TAT - Hour(s) Wise";
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
                    objExcel.ExportToExcelforExcel(dtExport, "TATHrWise", "ZZZZZZZZZZZZZZZZZZZZ", "ZZZZZZZZZZZZZZZZZZZZ", dtReportHeader, dtReportFooter);
                    break;
                case 2:
                    objExcel.ExportToPDF(dtExport, "TATHrWise", "ZZZZZZZZZZZZZZZZZZZZ", "ZZZZZZZZZZZZZZZZZZZZ", dtReportHeader, dtReportFooter);
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

            string IsSrvPerformanceStatHrWiseFilter = Convert.ToString(hfIsSrvPerformanceStatHrWiseFilter.Value);
            Session["IsSrvPerformanceStatHrWiseFilter"] = IsSrvPerformanceStatHrWiseFilter;

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

            Task PopulateStockTrialDataTask = new Task(() => GetPerformanceStatHourWisedata(FROMDATE, TODATE, BRANCH_ID, HEAD_BRANCH));
            PopulateStockTrialDataTask.RunSynchronously();
        }
        public void GetPerformanceStatHourWisedata(string FROMDATE, string TODATE, string BRANCH_ID, string HEAD_BRANCH)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_SRVPERFORMANCESTATHRSWISE_REPORT", con);
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

            if (Convert.ToString(Session["IsSrvPerformanceStatHrWiseFilter"]) == "Y")
            {
                var q = from d in dc.SRVPERFORMANCESTATHRSWISE_REPORTs
                        where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.BRANCHDESC) != "Total :" && Convert.ToString(d.BRANCHDESCORDBY) != "ZZZZZZZZZZZZZZ"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.SRVPERFORMANCESTATHRSWISE_REPORTs
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
            if (Convert.ToString(Session["IsSrvPerformanceStatHrWiseFilter"]) == "Y")
            {
                dtBranchTotal = oDBEngine.GetDataTable("Select BRANCHDESC,LESSTHAN1HR,HR1,HRS2,HRS3,HRS4,HRS5,HRS6,HRS7,HRS8,HRS9,HRS10,HRS11,HRS12,MORETHANHRS12,TOTALHRS from SRVPERFORMANCESTATHRSWISE_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND BRANCHDESC='Total :' AND BRANCHDESCORDBY='ZZZZZZZZZZZZZZ'");
                BranchDesc = dtBranchTotal.Rows[0][0].ToString();
                Lessthan1hrtot = dtBranchTotal.Rows[0][1].ToString();
                hr1tot = dtBranchTotal.Rows[0][2].ToString();
                hrs2tot = dtBranchTotal.Rows[0][3].ToString();
                hrs3tot = dtBranchTotal.Rows[0][4].ToString();
                hrs4tot = dtBranchTotal.Rows[0][5].ToString();
                hrs5tot = dtBranchTotal.Rows[0][6].ToString();
                hrs6tot = dtBranchTotal.Rows[0][7].ToString();
                hrs7tot = dtBranchTotal.Rows[0][8].ToString();
                hrs8tot = dtBranchTotal.Rows[0][9].ToString();
                hrs9tot = dtBranchTotal.Rows[0][10].ToString();
                hrs10tot = dtBranchTotal.Rows[0][11].ToString();
                hrs11tot = dtBranchTotal.Rows[0][12].ToString();
                hrs12tot = dtBranchTotal.Rows[0][13].ToString();
                morethan12hrstot = dtBranchTotal.Rows[0][14].ToString();
                Totalhrs = dtBranchTotal.Rows[0][15].ToString();
            }

            string summaryTAG = (e.Item as ASPxSummaryItem).Tag;
            if (e.IsTotalSummary == true)
            {
                switch (summaryTAG)
                {
                    case "Item_Branch":
                        e.Text = BranchDesc;
                        break;
                    case "Item_Less1Hr":
                        e.Text = Lessthan1hrtot;
                        break;
                    case "Item_Hr1":
                        e.Text = hr1tot;
                        break;
                    case "Item_Hrs2":
                        e.Text = hrs2tot;
                        break;
                    case "Item_Hrs3":
                        e.Text = hrs3tot;
                        break;
                    case "Item_Hrs4":
                        e.Text = hrs4tot;
                        break;
                    case "Item_Hrs5":
                        e.Text = hrs5tot;
                        break;
                    case "Item_Hrs6":
                        e.Text = hrs6tot;
                        break;
                    case "Item_Hrs7":
                        e.Text = hrs7tot;
                        break;
                    case "Item_Hrs8":
                        e.Text = hrs8tot;
                        break;
                    case "Item_Hrs9":
                        e.Text = hrs9tot;
                        break;
                    case "Item_Hrs10":
                        e.Text = hrs10tot;
                        break;
                    case "Item_Hrs11":
                        e.Text = hrs11tot;
                        break;
                    case "Item_Hrs12":
                        e.Text = hrs12tot;
                        break;
                    case "Item_MorethanHrs12":
                        e.Text = morethan12hrstot;
                        break;
                    case "Item_TotHrs":
                        e.Text = Totalhrs;
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