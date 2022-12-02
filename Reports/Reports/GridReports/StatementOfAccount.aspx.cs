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
    public partial class StatementOfAccount : System.Web.UI.Page
    {
        BusinessLogicLayer.Reports objReport = new BusinessLogicLayer.Reports();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        CommonBL cbl = new CommonBL();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
            if (Request.QueryString.AllKeys.Contains("dashboard"))
            {
                this.Page.MasterPageFile = "~/OMS/MasterPage/PopUp.Master";
                //divcross.Visible = false;
            }
            else
            {
                this.Page.MasterPageFile = "~/OMS/MasterPage/ERP.Master";
                //divcross.Visible = true;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/GridReports/StatementOfAccount.aspx");
            DateTime dtFrom;
            DateTime dtTo;
            //string ProjectSelectInEntryModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");
            //if (!String.IsNullOrEmpty(ProjectSelectInEntryModule))
            //{
            //    if (ProjectSelectInEntryModule == "Yes")
            //    {
            //        lookup_project.Visible = true;
            //        lblProj.Visible = true;
            //        ShowGrid.Columns[2].Visible = true;
            //        hdnProjectSelection.Value = "1";

            //    }
            //    else if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
            //    {
            //        lookup_project.Visible = false;
            //        lblProj.Visible = false;
            //        ShowGrid.Columns[2].Visible = false;
            //        hdnProjectSelection.Value = "0";
            //    }
            //}
            if (!IsPostBack)
            {
                DataTable dtBranchSelection = new DataTable();
                //DataTable dtProjectSelection = new DataTable();
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "Statement Of Account";
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

                //dtProjectSelection = oDBEngine.GetDataTable("select Variable_Value from Config_SystemSettings where Variable_Name='ReportsProjectSelection'");
                //hdnProjectSelectionInReport.Value = dtProjectSelection.Rows[0][0].ToString();
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

        protected void CallbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            CallbackPanel.JSProperties["cpPreviewUrl"] = "";
            string reportName = "SOA-Details~D";
            string RptModuleName = "STATEMENTOFACCOUNT";
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

            DateTime dtFrom;
            DateTime dtTo;
            dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
            dtTo = Convert.ToDateTime(ASPxToDate.Date);

            string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
            string TODATE = dtTo.ToString("yyyy-MM-dd");

            string BRANCH_ID = "";

            string BranchComponent = "";
            List<object> BranList = lookup_branch.GridView.GetSelectedFieldValues("ID");
            foreach (object Quo in BranList)
            {
                BranchComponent += "," + Quo;
            }
            BRANCH_ID = BranchComponent.TrimStart(',');

            string PARTYLEDGERIDS = "";
            PARTYLEDGERIDS = hdnSelectedPartyLedger.Value;

            string PROJECT_ID = "";
            //string Projects = "";
            //List<object> ProjectList = lookup_project.GridView.GetSelectedFieldValues("ID");
            //foreach (object Project in ProjectList)
            //{
            //    Projects += "," + Project;
            //}
            //PROJECT_ID = Projects.TrimStart(',');

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
            
            var ShowAllParty = 0;

            if(chkAllParty.Checked==true)
            {
                ShowAllParty = 1;
            }
            else if(chkAllParty.Checked==false)
            {
                ShowAllParty = 0;
            }

            var ShowHeader = 0;
            if(chkShowHeader.Checked==true)
            {
                ShowHeader = 1;
            }
            else if (chkShowHeader.Checked == false)
            {
                ShowHeader = 0;
            }

            var ShowFooter = 0;
            if (chkShowFooter.Checked == true)
            {
                ShowFooter = 1;
            }
            else if (chkShowFooter.Checked == false)
            {
                ShowFooter = 0;
            }
            //Task PopulateStockTrialDataTask = new Task(() => GetPartyLedgerdata(FROMDATE, TODATE, BRANCH_ID, PARTYLEDGERIDS, PROJECT_ID));
            //Task PopulateStockTrialDataTask = new Task(() => GetPartyLedgerdata(FROMDATE, TODATE, BRANCH_ID, PARTYLEDGERIDS));
            //PopulateStockTrialDataTask.RunSynchronously();
            CallbackPanel.JSProperties["cpPreviewUrl"] = reportName + "\\" + FROMDATE + "\\" + TODATE + "\\" + BRANCH_ID + "\\" + PARTYLEDGERIDS + "\\" + ddlCriteria.SelectedValue + "\\" + ShowAllParty + "\\" + PROJECT_ID + "\\" + ShowHeader + "\\" + ShowFooter + "\\" + RptModuleName;
        }
        //public void GetPartyLedgerdata(string FROMDATE, string TODATE, string BRANCH_ID, string SUBLEDGERIDS, string PROJECT_ID)
        //public void GetPartyLedgerdata(string FROMDATE, string TODATE, string BRANCH_ID, string SUBLEDGERIDS)
        //{
        //    try
        //    {
        //        DataSet ds = new DataSet();
        //        SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
        //        SqlCommand cmd = new SqlCommand("PRC_PARTYLEDGERPOSTING_REPORT", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
        //        cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
        //        cmd.Parameters.AddWithValue("@FROMDATE", FROMDATE);
        //        cmd.Parameters.AddWithValue("@TODATE", TODATE);
        //        cmd.Parameters.AddWithValue("@BRANCH_ID", BRANCH_ID);
        //        cmd.Parameters.AddWithValue("@SUBLEDGERID", SUBLEDGERIDS);
        //        cmd.Parameters.AddWithValue("@CRITERIA", ddlCriteria.SelectedValue);
        //        cmd.Parameters.AddWithValue("@SHOWALLPARTY", (chkAllParty.Checked) ? "1" : "0");
        //        cmd.Parameters.AddWithValue("@PROJECT_ID", "");
        //        cmd.Parameters.AddWithValue("@USERID", Convert.ToInt32(Session["userid"]));

        //        cmd.CommandTimeout = 0;
        //        SqlDataAdapter da = new SqlDataAdapter();
        //        da.SelectCommand = cmd;
        //        da.Fill(ds);

        //        cmd.Dispose();
        //        con.Dispose();
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

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

        //#region Project Populate
        //protected void Project_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        //{
        //    if (e.Parameter.Split('~')[0] == "BindProjectGrid")
        //    {
        //        DataTable ProjectTable = new DataTable();
        //        ProjectTable = GetProject();

        //        if (ProjectTable.Rows.Count > 0)
        //        {
        //            Session["ProjectData"] = ProjectTable;
        //            lookup_project.DataSource = ProjectTable;
        //            lookup_project.DataBind();
        //        }
        //        else
        //        {
        //            Session["ProjectData"] = ProjectTable;
        //            lookup_project.DataSource = null;
        //            lookup_project.DataBind();
        //        }
        //    }
        //}

        //public DataTable GetProject()
        //{
        //    DataTable dt = new DataTable();
        //    SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
        //    SqlCommand cmd = new SqlCommand("PRC_FETCHPROJECTS_REPORT", con);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandTimeout = 0;
        //    SqlDataAdapter da = new SqlDataAdapter();
        //    da.SelectCommand = cmd;
        //    da.Fill(dt);
        //    cmd.Dispose();
        //    con.Dispose();

        //    return dt;
        //}

        //protected void lookup_project_DataBinding(object sender, EventArgs e)
        //{
        //    if (Session["ProjectData"] != null)
        //    {
        //        lookup_project.DataSource = (DataTable)Session["ProjectData"];
        //    }
        //}
        //#endregion
    }
}