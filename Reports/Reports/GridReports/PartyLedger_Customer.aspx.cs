#region ==========================Revision History====================================================================================================
//1.0   v2 .0.38    Debashis    15/09/2023      Opening Breakup required in Party Ledger.Refer: 0026804
//2.0   v2 .0.42    Debashis    26/03/2024      Customer code column is required in various reports.Refer: 0027273
#endregion =======================End Revision History================================================================================================
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
using DevExpress.Data;
using System.Threading.Tasks;
using DevExpress.XtraPrinting;
using DevExpress.Export;
using Reports.Model;
using System.Drawing;

namespace Reports.Reports.GridReports
{
    public partial class PartyLedger_Customer : System.Web.UI.Page
    {
        BusinessLogicLayer.Reports objReport = new BusinessLogicLayer.Reports();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        CommonBL cbl = new CommonBL();

        decimal TotalDebit_OP = 0, TotalCredit_OP = 0, TotalCredit_PR = 0, TotalDebit_PR = 0;
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
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/GridReports/PartyLedger_Customer.aspx");
            DateTime dtFrom;
            DateTime dtTo;
            string ProjectSelectInEntryModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");
            if (!String.IsNullOrEmpty(ProjectSelectInEntryModule))
            {
                if (ProjectSelectInEntryModule == "Yes")
                {
                    lookup_project.Visible = true;
                    lblProj.Visible = true;
                    //Rev 2.0 Mantis: 0027273
                    //ShowGrid.Columns[3].Visible = true;
                    ShowGrid.Columns[4].Visible = true;
                    //End of Rev 2.0 Mantis: 0027273
                    hdnProjectSelection.Value = "1";

                }
                else if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
                {
                    lookup_project.Visible = false;
                    lblProj.Visible = false;
                    //Rev 2.0 Mantis: 0027273
                    //ShowGrid.Columns[3].Visible = false;
                    ShowGrid.Columns[4].Visible = false;
                    //End of Rev 2.0 Mantis: 0027273
                    hdnProjectSelection.Value = "0";
                }
            }
            if (!IsPostBack)
            {
                DataTable dtBranchSelection = new DataTable();
                DataTable dtLedgerSelection = new DataTable();
                DataTable dtProjectSelection = new DataTable();
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "Party Ledger - Customer";
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
                Session["exportval"] = null;
                dtFrom = DateTime.Now;
                dtTo = DateTime.Now;
                ASPxFromDate.Text = dtFrom.ToString("dd-MM-yyyy");
                ASPxToDate.Text = dtTo.ToString("dd-MM-yyyy");

                Session["IsPLedgCustFilter"] = null;
                ASPxFromDate.Value = DateTime.Now;
                ASPxToDate.Value = DateTime.Now;
                BranchHoOffice();
                Date_finyearwise(Convert.ToString(Session["LastFinYear"]));
                chkallpartyledg.Attributes.Add("OnClick", "PartyLedgAll('allpartyledg')");
                Session["BranchNames"] = null;
                dtBranchSelection = oDBEngine.GetDataTable("select Variable_Value from Config_SystemSettings where Variable_Name='ReportsBranchSelection'");
                hdnBranchSelection.Value = dtBranchSelection.Rows[0][0].ToString();

                dtLedgerSelection = oDBEngine.GetDataTable("SELECT A.MainAccount_ReferenceID AS ID FROM Master_MainAccount A WHERE MainAccount_AccountCode='SYSTM00003'");
                hdnSelectedLedger.Value = dtLedgerSelection.Rows[0][0].ToString();

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
                ShowGrid.JSProperties["cpSave"] = null;

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

                string LEDGERID = "";

                if (hdnSelectedLedger.Value != "")
                {
                    LEDGERID = hdnSelectedLedger.Value;
                }
            }
        }
        public void Date_finyearwise(string Finyear)
        {
            CommonBL pledgcust = new CommonBL();
            DataTable dtpledgcust = new DataTable();

            dtpledgcust = pledgcust.GetDateFinancila(Finyear);
            if (dtpledgcust.Rows.Count > 0)
            {

                ASPxFromDate.MaxDate = Convert.ToDateTime((dtpledgcust.Rows[0]["FinYear_EndDate"]));
                ASPxFromDate.MinDate = Convert.ToDateTime((dtpledgcust.Rows[0]["FinYear_StartDate"]));

                ASPxToDate.MaxDate = Convert.ToDateTime((dtpledgcust.Rows[0]["FinYear_EndDate"]));
                ASPxToDate.MinDate = Convert.ToDateTime((dtpledgcust.Rows[0]["FinYear_StartDate"]));

                DateTime MaximumDate = Convert.ToDateTime((dtpledgcust.Rows[0]["FinYear_EndDate"]));
                DateTime MinimumDate = Convert.ToDateTime((dtpledgcust.Rows[0]["FinYear_StartDate"]));

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
            CommonBL pledgcust = new CommonBL();
            DataTable dtpledgcust = new DataTable();
            DataTable dtBranchChild = new DataTable();
            dtpledgcust = pledgcust.GetBranchheadoffice(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), "HO");
            if (dtpledgcust.Rows.Count > 0)
            {
                ddlbranchHO.DataSource = dtpledgcust;
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

        public void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                BranchHoOffice();
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

        public void BindDropDownList()
        {
            // Declare a Dictionary to hold all the Options with Value and Text.
            Dictionary<string, string> options = new Dictionary<string, string>();
            options.Add("0", "Export to");
            options.Add("1", "PDF");
            options.Add("2", "XLSX");
            options.Add("3", "RTF");
            options.Add("4", "CSV");

            // Bind the Dictionary to the DropDownList.
            drdExport.DataSource = options;
            drdExport.DataTextField = "value";
            drdExport.DataValueField = "key";
            drdExport.DataBind();
        }

        public void bindexport(int Filter)
        {
            string filename = "Party Ledger - Customer";
            exporter.FileName = filename;
            string FileHeader = "";

            exporter.FileName = filename;

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "Party Ledger - Customer" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
            FileHeader = ReplaceFirst(FileHeader, "\r\n", Convert.ToString(Session["BranchNames"]));
            exporter.PageHeader.Left = FileHeader;
            exporter.PageHeader.Font.Size = 10;
            exporter.PageHeader.Font.Name = "Tahoma";
            exporter.Landscape = true;
            exporter.MaxColumnWidth = 100;
            exporter.GridViewID = "ShowGrid";
            exporter.RenderBrick += exporter_RenderBrick;
            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();

                    break;
                case 2:
                    //exporter.WriteXlsxToResponse();
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

        public string ReplaceFirst(string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + Environment.NewLine + replace + Environment.NewLine + text.Substring(pos + search.Length);
        }

        protected void exporter_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        {
            e.BrickStyle.BackColor = Color.White;
            e.BrickStyle.ForeColor = Color.Black;
        }

        protected void CallbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string[] CallVal = e.Parameter.ToString().Split('~');
            string type = CallVal[1];
            string code = CallVal[2];
            if (CallVal[1] == "null")
            {
                type = "";
            }
            if (CallVal[2] == "null")
            {
                code = "";
            }
            string IsPLedgCustFilter = Convert.ToString(hfIsPLedgCustFilter.Value);
            Session["IsPLedgCustFilter"] = IsPLedgCustFilter;

            DateTime dtFrom;
            DateTime dtTo;
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

            string LEDGERIDS = hdnSelectedLedger.Value;
            string PARTYLEDGERIDS = "";
            string ALLPARTYLEDGER = (chkallpartyledg.Checked) ? "1" : "0";
            if (Convert.ToString(ALLPARTYLEDGER) == "1")
            {
                PARTYLEDGERIDS = "";
            }
            else
            {
                PARTYLEDGERIDS = hdnSelectedPartyLedger.Value;
            }

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

            Task PopulateStockTrialDataTask = new Task(() => GetPartyLedgerdata(FROMDATE, TODATE, BRANCH_ID, LEDGERIDS, PARTYLEDGERIDS, PROJECT_ID));
            PopulateStockTrialDataTask.RunSynchronously();
            ShowGrid.ExpandAll();
        }
        public void GetPartyLedgerdata(string FROMDATE, string TODATE, string BRANCH_ID, string LEDGERIDS, string SUBLEDGERIDS, string PROJECT_ID)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_PARTYLEDGERCUSTOMERVENDOR_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@FROMDATE", FROMDATE);
                cmd.Parameters.AddWithValue("@TODATE", TODATE);
                cmd.Parameters.AddWithValue("@BRANCH_ID", BRANCH_ID);
                cmd.Parameters.AddWithValue("@PARTYID", SUBLEDGERIDS);
                cmd.Parameters.AddWithValue("@PARTY_TYPE", "C");
                cmd.Parameters.AddWithValue("@PROJECT_ID", PROJECT_ID);
                //Rev 1.0 Mantis: 0026804
                cmd.Parameters.AddWithValue("@SHOWOPBREAKUP", (chkShowOPBreakUp.Checked) ? "1" : "0");
                //End of Rev 1.0 Mantis: 0026804
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

        protected void ShowGrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            TotalDebit_OP = Convert.ToDecimal(ShowGrid.GetTotalSummaryValue(ShowGrid.TotalSummary["OP_DEBIT"]));
            TotalCredit_OP = Convert.ToDecimal(ShowGrid.GetTotalSummaryValue(ShowGrid.TotalSummary["OP_CREDIT"]));
            TotalDebit_PR = Convert.ToDecimal(ShowGrid.GetTotalSummaryValue(ShowGrid.TotalSummary["PR_DEBIT"]));
            TotalCredit_PR = Convert.ToDecimal(ShowGrid.GetTotalSummaryValue(ShowGrid.TotalSummary["PR_CREDIT"]));

            if (e.Item.FieldName == "Particulars")
            {
                e.Text = "Net Total";
            }
            else if (e.Item.FieldName == "CL_DEBIT")
            {
                if ((TotalDebit_OP + TotalDebit_PR) - (TotalCredit_OP + TotalCredit_PR) > 0)
                {
                    e.Text = Convert.ToString(Math.Abs((TotalDebit_OP + TotalDebit_PR) - (TotalCredit_OP + TotalCredit_PR)));
                }
                else
                {
                    e.Text = Convert.ToString(Math.Abs(0.00));
                }
            }
            else if (e.Item.FieldName == "CL_CREDIT")
            {
                if ((TotalDebit_OP + TotalDebit_PR) - (TotalCredit_OP + TotalCredit_PR) < 0)
                {
                    e.Text = Convert.ToString(Math.Abs((TotalDebit_OP + TotalDebit_PR) - (TotalCredit_OP + TotalCredit_PR)));
                }
                else
                {
                    e.Text = Convert.ToString(Math.Abs(0.00));
                }
            }
            else
            {
                e.Text = string.Format("{0}", e.Value);
            }
        }

        #region LinQ
        protected void GenerateEntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SEQ";
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsPLedgCustFilter"]) == "Y")
            {
                var q = from d in dc.PARTYLEDGERCUSTVEND_REPORTs
                        where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.PARTY_TYPE)=="C"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.PARTYLEDGERCUSTVEND_REPORTs
                        where Convert.ToString(d.SEQ) == "0"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
        }
        #endregion

        #region Branch Populate

        protected void Componentbranch_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
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
                    ComponentTable = oDBEngine.GetDataTable("select * from(select branch_id as ID,branch_description,branch_code from tbl_master_branch a where a.branch_id=1 union all select branch_id as ID,branch_description,branch_code from tbl_master_branch b where b.branch_parentId=1) a order by branch_description");
                    Session["SI_ComponentData_Branch"] = ComponentTable;
                    lookup_branch.DataSource = ComponentTable;
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