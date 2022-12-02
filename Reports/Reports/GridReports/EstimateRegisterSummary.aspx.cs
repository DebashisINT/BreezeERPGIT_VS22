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
    public partial class EstimateRegisterSummary : System.Web.UI.Page
    {
        BusinessLogicLayer.Reports objReport = new BusinessLogicLayer.Reports();
        CommonBL cbl = new CommonBL();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        ExcelFile objExcel = new ExcelFile();
        DataTable dtExport = new DataTable();
        DataTable dtReportHeader = new DataTable();
        DataTable dtReportFooter = new DataTable();

        DataTable dtEMTotal = null;
        string EMTotalBalDesc = "";
        string EMIMCost = "";
        string EMISCost = "";
        string EMITCost = "";
        string EMIMNetAmt = "";
        string EMISNetAmt = "";

        string EMRMCost = "";
        string EMRSCost = "";
        string EMRTCost = "";
        string EMRMNetAmt = "";
        string EMRSNetAmt = "";
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
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/GridReports/EstimateRegisterSummary.aspx");
            DateTime dtFrom;
            DateTime dtTo;
            string ProjectSelectInEntryModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");
            if (!String.IsNullOrEmpty(ProjectSelectInEntryModule))
            {
                if (ProjectSelectInEntryModule == "Yes")
                {
                    lookup_project.Visible = true;
                    Label2.Visible = true;
                    ShowGridList.Columns[1].Visible = true;
                    hdnProjectSelection.Value = "1";

                }
                else if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
                {
                    lookup_project.Visible = false;
                    Label2.Visible = false;
                    ShowGridList.Columns[1].Visible = false;
                    hdnProjectSelection.Value = "0";
                }
            }
            if (!IsPostBack)
            {
                DataTable dtBranchSelection = new DataTable();
                DataTable dtProjectSelection = new DataTable();
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "Estimate Register - Summary";
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

                Session["IsEstimateRegSumFilter"] = null;
                Session["SI_ComponentData_Branch"] = null;
                dtFrom = DateTime.Now;
                dtTo = DateTime.Now;
                ASPxFromDate.Text = dtFrom.ToString("dd-MM-yyyy");
                ASPxToDate.Text = dtTo.ToString("dd-MM-yyyy");

                ASPxFromDate.Value = DateTime.Now;
                ASPxToDate.Value = DateTime.Now;
                BranchHoOffice();
                Date_finyearwise(Convert.ToString(Session["LastFinYear"]));
                Session["BranchNames"] = null;
                dtBranchSelection = oDBEngine.GetDataTable("select Variable_Value from Config_SystemSettings where Variable_Name='ReportsBranchSelection'");
                hdnBranchSelection.Value = dtBranchSelection.Rows[0][0].ToString();

                dtProjectSelection = oDBEngine.GetDataTable("select Variable_Value from Config_SystemSettings where Variable_Name='ReportsProjectSelection'");
                hdnProjectSelectionInReport.Value = dtProjectSelection.Rows[0][0].ToString();
            }
            else
            {
                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);
            }

            if (!IsPostBack)
            {
                string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);
                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);

                string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
                string TODATE = dtTo.ToString("yyyy-MM-dd");
                string BRANCH_ID = "";
                if (hdnSelectedBranches.Value != "")
                {
                    BRANCH_ID = hdnSelectedBranches.Value;
                }
            }
        }

        public void Date_finyearwise(string Finyear)
        {
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
        public void BranchHoOffice()
        {
            CommonBL blbranch = new CommonBL();
            DataTable dtbranch = new DataTable();
            DataTable dtBranchChild = new DataTable();
            dtbranch = blbranch.GetBranchheadoffice(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), "HO");
            if (dtbranch.Rows.Count > 0)
            {
                ddlbranchHO.DataSource = dtbranch;
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
                }
                else
                {
                    ComponentTable = oDBEngine.GetDataTable("select * from (select branch_id as ID,branch_description,branch_code from tbl_master_branch a where a.branch_id=1  union all select branch_id as ID,branch_description,branch_code from tbl_master_branch b where b.branch_parentId=1) a order by branch_description");
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
        #region Export
        public void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Convert.ToString(Session["IsEstimateRegSumFilter"]) == "Y")
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

            string filename = "EstimateRegisterSummary";
            exporter.FileName = filename;

            if (Filter == 1 || Filter == 2)
            {
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                con.Open();
                string selectQuery = "SELECT BRANCHDESC,PROJ_NAME,PARTYNAME,DOCNO,DOCDATE,REV_NO,REV_DATE,REFORDER,REFQO,REFINDENT,REFSI,INITIALMCOST,INITIALSCOST,INITIALTCOST,INITIALMNETAMT,INITIALSNETAMT,REVMCOST,REVSCOST,REVTCOST,REVMNETAMT,REVSNETAMT FROM ESTIMATEREGISTERSUMDET_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND BRANCH_ID<>9999999999 AND PARTYNAME<>'Total :' AND REPORTTYPE='Summary' order by SEQ";
                SqlDataAdapter myCommand = new SqlDataAdapter(selectQuery, con);

                // Create and fill a DataSet.
                DataSet ds = new DataSet();
                myCommand.Fill(ds, "Main");
                myCommand = new SqlDataAdapter("Select PARTYNAME,INITIALMCOST,INITIALSCOST,INITIALTCOST,INITIALMNETAMT,INITIALSNETAMT,REVMCOST,REVSCOST,REVTCOST,REVMNETAMT,REVSNETAMT from ESTIMATEREGISTERSUMDET_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND BRANCH_ID=9999999999 AND PARTYNAME='Total :' AND REPORTTYPE='Summary'", con);
                myCommand.Fill(ds, "GrossTotal");
                myCommand.Dispose();
                con.Dispose();
                Session["exportemregsumdataset"] = ds;

                string ProjectSelectInEntryModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");

                dtExport = ds.Tables[0].Copy();
                dtExport.Clear();
                dtExport.Columns.Add(new DataColumn("Unit", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Project Name", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Customer Name", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Document No.", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Document Date", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Revision No.", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Revision Date", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Ref. Order", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Ref. Quotation", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Ref. Indent", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Ref. Invoice", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Initial Material Cost", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("Initial Service Cost", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("Initial Total Cost", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("Initial Material Net Amount", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("Initial Service Net Amount", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("Revised Material Cost", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("Revised Service Cost", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("Revised Total Cost", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("Revised Material Net Amount", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("Revised Service Net Amount", typeof(decimal)));

                foreach (DataRow dr1 in ds.Tables[0].Rows)
                {
                    DataRow row2 = dtExport.NewRow();

                    row2["Unit"] = dr1["BRANCHDESC"];
                    row2["Project Name"] = dr1["PROJ_NAME"];
                    row2["Customer Name"] = dr1["PARTYNAME"];
                    row2["Document No."] = dr1["DOCNO"];
                    row2["Document Date"] = dr1["DOCDATE"];
                    row2["Revision No."] = dr1["REV_NO"];
                    row2["Revision Date"] = dr1["REV_DATE"];
                    row2["Ref. Order"] = dr1["REFORDER"];
                    row2["Ref. Quotation"] = dr1["REFQO"];
                    row2["Ref. Indent"] = dr1["REFINDENT"];
                    row2["Ref. Invoice"] = dr1["REFSI"];
                    row2["Initial Material Cost"] = dr1["INITIALMCOST"];
                    row2["Initial Service Cost"] = dr1["INITIALSCOST"];
                    row2["Initial Total Cost"] = dr1["INITIALTCOST"];
                    row2["Initial Material Net Amount"] = dr1["INITIALMNETAMT"];
                    row2["Initial Service Net Amount"] = dr1["INITIALSNETAMT"];
                    row2["Revised Material Cost"] = dr1["REVMCOST"];
                    row2["Revised Service Cost"] = dr1["REVSCOST"];
                    row2["Revised Total Cost"] = dr1["REVTCOST"];
                    row2["Revised Material Net Amount"] = dr1["REVMNETAMT"];
                    row2["Revised Service Net Amount"] = dr1["REVSNETAMT"];

                    dtExport.Rows.Add(row2);
                }

                if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
                    dtExport.Columns.Remove("Project Name");

                dtExport.Columns.Remove("BRANCHDESC");
                dtExport.Columns.Remove("PROJ_NAME");
                dtExport.Columns.Remove("PARTYNAME");
                dtExport.Columns.Remove("DOCNO");
                dtExport.Columns.Remove("DOCDATE");
                dtExport.Columns.Remove("REV_NO");
                dtExport.Columns.Remove("REV_DATE");
                dtExport.Columns.Remove("REFORDER");
                dtExport.Columns.Remove("REFQO");
                dtExport.Columns.Remove("REFINDENT");
                dtExport.Columns.Remove("REFSI");
                dtExport.Columns.Remove("INITIALMCOST");
                dtExport.Columns.Remove("INITIALSCOST");
                dtExport.Columns.Remove("INITIALTCOST");
                dtExport.Columns.Remove("INITIALMNETAMT");
                dtExport.Columns.Remove("INITIALSNETAMT");
                dtExport.Columns.Remove("REVMCOST");
                dtExport.Columns.Remove("REVSCOST");
                dtExport.Columns.Remove("REVTCOST");
                dtExport.Columns.Remove("REVMNETAMT");
                dtExport.Columns.Remove("REVSNETAMT");

                DataRow row3 = dtExport.NewRow();
                row3["Customer Name"] = ds.Tables[1].Rows[0]["PARTYNAME"].ToString();
                row3["Initial Material Cost"] = ds.Tables[1].Rows[0]["INITIALMCOST"].ToString();
                row3["Initial Service Cost"] = ds.Tables[1].Rows[0]["INITIALSCOST"].ToString();
                row3["Initial Total Cost"] = ds.Tables[1].Rows[0]["INITIALTCOST"].ToString();
                row3["Initial Material Net Amount"] = ds.Tables[1].Rows[0]["INITIALMNETAMT"].ToString();
                row3["Initial Service Net Amount"] = ds.Tables[1].Rows[0]["INITIALSNETAMT"].ToString();
                row3["Revised Material Cost"] = ds.Tables[1].Rows[0]["REVMCOST"].ToString();
                row3["Revised Service Cost"] = ds.Tables[1].Rows[0]["REVSCOST"].ToString();
                row3["Revised Total Cost"] = ds.Tables[1].Rows[0]["REVTCOST"].ToString();
                row3["Revised Material Net Amount"] = ds.Tables[1].Rows[0]["REVMNETAMT"].ToString();
                row3["Revised Service Net Amount"] = ds.Tables[1].Rows[0]["REVSNETAMT"].ToString();
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
                HeaderRow6[0] = "Estimate Register - Summary";
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
                exporter.GridViewID = "ShowGridList";
            }
            switch (Filter)
            {
                case 1:
                    objExcel.ExportToExcelforExcel(dtExport, "EstimateRegisterSummary", "ZZZZZZZZZZZZZZZZ", "ZZZZZZZZZZZZZZZZ", dtReportHeader, dtReportFooter);
                    break;
                case 2:
                    objExcel.ExportToPDF(dtExport, "EstimateRegisterSummary", "ZZZZZZZZZZZZZZZZ", "ZZZZZZZZZZZZZZZZ", dtReportHeader, dtReportFooter);
                    break;
                case 3:
                    exporter.WriteCsvToResponse();
                    break;
                default:
                    return;
            }

        }
        #endregion

        #region Estimate Register Summary grid
        protected void CallbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string IsEstimateRegSumFilter = Convert.ToString(hfIsEstimateRegSumFilter.Value);
            Session["IsEstimateRegSumFilter"] = IsEstimateRegSumFilter;

            DateTime dtFrom;
            DateTime dtTo;

            dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
            dtTo = Convert.ToDateTime(ASPxToDate.Date);
            string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
            string TODATE = dtTo.ToString("yyyy-MM-dd");


            string BRANCH_ID = "";

            string BranchComponent = "";
            List<object> BranchList = lookup_branch.GridView.GetSelectedFieldValues("ID");
            foreach (object branch in BranchList)
            {
                BranchComponent += "," + branch;
            }
            BRANCH_ID = BranchComponent.TrimStart(',');

            string PROJECT_ID = "";
            string Projects = "";
            List<object> ProjectList = lookup_project.GridView.GetSelectedFieldValues("ID");
            foreach (object Project in ProjectList)
            {
                Projects += "," + Project;
            }
            PROJECT_ID = Projects.TrimStart(',');

            string BRANCH_NAME = "";
            string BranchNameComponent = "";
            List<object> BranchNameList = lookup_branch.GridView.GetSelectedFieldValues("branch_description");
            foreach (object BranchName in BranchNameList)
            {
                BranchNameComponent += "," + BranchName;
            }
            if (BranchNameList.Count > 1 || BranchNameList.Count==0)
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
            Task PopulateStockTrialDataTask = new Task(() => GetESMRegSumdata(BRANCH_ID, FROMDATE, TODATE, PROJECT_ID));
            PopulateStockTrialDataTask.RunSynchronously();
        }
        public void GetESMRegSumdata(string BRANCH_ID, string FROMDATE, string TODATE, string PROJECT_ID)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_ESTIMATEREGISTERSUMDET_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@FROMDATE", FROMDATE);
                cmd.Parameters.AddWithValue("@TODATE", TODATE);
                cmd.Parameters.AddWithValue("@BRANCHID", BRANCH_ID);
                cmd.Parameters.AddWithValue("@PARTY_CODE", hdnCustomerId.Value);
                cmd.Parameters.AddWithValue("@PROJECT_ID", PROJECT_ID);
                cmd.Parameters.AddWithValue("@REPORTTYPE", "Summary");                
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

        protected void ShowGridList_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            if (Convert.ToString(Session["IsEstimateRegSumFilter"]) == "Y")
            {
                dtEMTotal = oDBEngine.GetDataTable("Select PARTYNAME,INITIALMCOST,INITIALSCOST,INITIALTCOST,INITIALMNETAMT,INITIALSNETAMT,REVMCOST,REVSCOST,REVTCOST,REVMNETAMT,REVSNETAMT from ESTIMATEREGISTERSUMDET_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND BRANCH_ID=9999999999 AND PARTYNAME='Total :' AND REPORTTYPE='Summary'");
                EMTotalBalDesc = dtEMTotal.Rows[0][0].ToString();
                EMIMCost = dtEMTotal.Rows[0][1].ToString();
                EMISCost = dtEMTotal.Rows[0][2].ToString();
                EMITCost = dtEMTotal.Rows[0][3].ToString();
                EMIMNetAmt = dtEMTotal.Rows[0][4].ToString();
                EMISNetAmt = dtEMTotal.Rows[0][5].ToString();
                EMRMCost = dtEMTotal.Rows[0][6].ToString();
                EMRSCost = dtEMTotal.Rows[0][7].ToString();
                EMRTCost = dtEMTotal.Rows[0][8].ToString();
                EMRMNetAmt = dtEMTotal.Rows[0][9].ToString();
                EMRSNetAmt = dtEMTotal.Rows[0][10].ToString();
            }

            string summaryTAG = (e.Item as ASPxSummaryItem).Tag;
            if (e.IsTotalSummary == true)
            {
                switch (summaryTAG)
                {
                    case "EM_Party":
                        e.Text = EMTotalBalDesc;
                        break;
                    case "EM_IMC":
                        e.Text = EMIMCost;
                        break;
                    case "EM_ISC":
                        e.Text = EMISCost;
                        break;
                    case "EM_ITC":
                        e.Text = EMITCost;
                        break;
                    case "EM_IMNetAmt":
                        e.Text = EMIMNetAmt;
                        break;
                    case "EM_ISNetAmt":
                        e.Text = EMISNetAmt;
                        break;
                    case "EM_RMC":
                        e.Text = EMRMCost;
                        break;
                    case "EM_RSC":
                        e.Text = EMRSCost;
                        break;
                    case "EM_RTC":
                        e.Text = EMRTCost;
                        break;
                    case "EM_RMNetAmt":
                        e.Text = EMRMNetAmt;
                        break;
                    case "EM_RSNetAmt":
                        e.Text = EMRSNetAmt;
                        break;
                }
            }
        }

        #endregion

        #region LinQ
        protected void GenerateEntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SEQ";
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsEstimateRegSumFilter"]) == "Y")
            {
                var q = from d in dc.ESTIMATEREGISTERSUMDET_REPORTs
                        where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.BRANCH_ID) != "9999999999" && Convert.ToString(d.PARTYNAME) != "Total :" && Convert.ToString(d.REPORTTYPE) == "Summary"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.ESTIMATEREGISTERSUMDET_REPORTs
                        where Convert.ToString(d.SEQ) == "0"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
            ShowGridList.ExpandAll();
        }
        #endregion

        #region Project Populate
        protected void Project_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter.Split('~')[0] == "BindProjectGrid")
            {
                DataTable ProjectTable = new DataTable();
                ProjectTable = GetProject();

                if (ProjectTable.Rows.Count > 0)
                {
                    Session["ProjectData"] = ProjectTable;
                    lookup_project.DataSource = ProjectTable;
                    lookup_project.DataBind();
                }
                else
                {
                    Session["ProjectData"] = ProjectTable;
                    lookup_project.DataSource = null;
                    lookup_project.DataBind();
                }
            }
        }

        public DataTable GetProject()
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("PRC_FETCHPROJECTS_REPORT", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dt);
            cmd.Dispose();
            con.Dispose();

            return dt;
        }

        protected void lookup_project_DataBinding(object sender, EventArgs e)
        {
            if (Session["ProjectData"] != null)
            {
                lookup_project.DataSource = (DataTable)Session["ProjectData"];
            }
        }
        #endregion
    }
}