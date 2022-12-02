using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Management.ToolsUtilities
{
    public partial class management_utilities_frm_attendance_Lock_iframe : System.Web.UI.Page
    {
        //DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        public string pageAccess = "";
        protected void Page_PreInit(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            dataCompany.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            databranch.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            dataYear.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //------- For Read Only User in SQL Datasource Connection String   Start-----------------

            if (HttpContext.Current.Session["EntryProfileType"] != null)
            {
                if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) == "R")
                {
                    //SqlDataSource1.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"];MULTI
                    SqlDataSource1.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
                else
                {
                    //SqlDataSource1.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
                    SqlDataSource1.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
            }

            //------- For Read Only User in SQL Datasource Connection String   End-----------------

            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            if (!IsPostBack)
            {
                ViewState["sort"] = "";
                cmbLockI.Enabled = false;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "height", "<script language='javascript'>height();</script>");
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            oDBEngine.InsurtFieldValue("tbl_trans_AttendanceLock", " al_company,al_branch,al_month,al_year,al_lock,al_CreateDate,al_CreateUser", "'" + cmbCompanyI.SelectedValue + "'," + cmbBranchI.SelectedValue + "," + cmbMonthI.SelectedValue + "," + cmbYearI.SelectedValue + ",'" + cmbLockI.SelectedValue + "',getdate(),'" + HttpContext.Current.Session["userid"].ToString() + "'");
            GrdAttendanceLock.DataBind();
            Page.ClientScript.RegisterStartupScript(this.GetType(), "height", "<script language='javascript'>height();</script>");
        }
        protected void GrdAttendanceLock_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            DropDownList cmbcompany = (DropDownList)GrdAttendanceLock.Rows[e.RowIndex].FindControl("cmbCompanyE");
            DropDownList cmbBranch = (DropDownList)GrdAttendanceLock.Rows[e.RowIndex].FindControl("cmbBranchE");
            DropDownList cmbmonth = (DropDownList)GrdAttendanceLock.Rows[e.RowIndex].FindControl("cmbMonthE");
            DropDownList cmbYear = (DropDownList)GrdAttendanceLock.Rows[e.RowIndex].FindControl("cmbYearE");
            DropDownList cmbLock = (DropDownList)GrdAttendanceLock.Rows[e.RowIndex].FindControl("cmbLockE");
            int noofrowseffected = oDBEngine.InsertDataFromAnotherTable(" tbl_trans_attendanceLock_log ", " tbl_trans_attendanceLock ", " al_id,al_company,al_branch,al_month,al_year,al_lock,la_PD_Lock,'M',al_createDate,al_CreateUser,al_lastmodifiedDate,al_lastmodifiedUser," + HttpContext.Current.Session["userid"].ToString() + ",getdate() ", " al_id=" + GrdAttendanceLock.DataKeys[e.RowIndex].Value.ToString());
            if (noofrowseffected > 0)
                oDBEngine.SetFieldValue(" tbl_trans_AttendanceLock ", " [al_company] = '" + cmbcompany.SelectedValue + "', [al_branch] = '" + cmbBranch.SelectedValue + "', [al_month] = '" + cmbmonth.SelectedValue + "', [al_year] = '" + cmbYear.SelectedValue + "', [al_lock] = '" + cmbLock.SelectedValue + "',[al_LastModifiedDate]=getdate(),[al_LastModifiedUser]='" + HttpContext.Current.Session["userid"].ToString() + "'", " al_id=" + GrdAttendanceLock.DataKeys[e.RowIndex].Value.ToString());
        }
        protected void GrdAttendanceLock_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (ViewState["sort"].ToString() == (e.SortExpression.ToString() + " ASC"))
            {
                ViewState["sort"] = e.SortExpression + " DESC";
                SqlDataSource1.SelectCommand = "SELECT [al_id], [al_company],(select ltrim(rtrim(cmp_name)) from tbl_master_company where cmp_internalid=al_company) as al_companyD, [al_branch],(select branch_description from tbl_master_branch where branch_id=al_branch) as al_branchD, [al_month],(case when al_month=1 then 'Jan' when al_month=2 then 'Feb' when al_month=3 then 'Mar' when al_month=4 then 'Apr' when al_month=5 then 'May' when al_month=6 then 'Jun' when al_month=7 then 'Jul' when al_month=8 then 'Aug' when al_month=9 then 'Sep' when al_month=10 then 'Oct' when al_month=11 then 'Nov' when al_month=12 then 'Dec' else '' end) as al_monthD, [al_year], [al_lock],(case when al_lock='Y' then 'Locked' else 'Open' end) as al_lockD FROM [tbl_trans_AttendanceLock] ORDER BY " + ViewState["sort"].ToString();
            }
            else
            {
                ViewState["sort"] = e.SortExpression + " ASC";
                SqlDataSource1.SelectCommand = "SELECT [al_id], [al_company],(select ltrim(rtrim(cmp_name)) from tbl_master_company where cmp_internalid=al_company) as al_companyD, [al_branch],(select branch_description from tbl_master_branch where branch_id=al_branch) as al_branchD, [al_month],(case when al_month=1 then 'Jan' when al_month=2 then 'Feb' when al_month=3 then 'Mar' when al_month=4 then 'Apr' when al_month=5 then 'May' when al_month=6 then 'Jun' when al_month=7 then 'Jul' when al_month=8 then 'Aug' when al_month=9 then 'Sep' when al_month=10 then 'Oct' when al_month=11 then 'Nov' when al_month=12 then 'Dec' else '' end) as al_monthD, [al_year], [al_lock],(case when al_lock='Y' then 'Locked' else 'Open' end) as al_lockD FROM [tbl_trans_AttendanceLock] ORDER BY " + ViewState["sort"].ToString();
            }
            GrdAttendanceLock.DataBind();
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string wherecondition = "";
            if (txtCompany.Text != "Company")
                wherecondition = " al_companyD like '" + txtCompany.Text + "%'";
            if (txtBranch.Text != "Branch")
            {
                if (wherecondition == "")
                    wherecondition = " al_branchD like '" + txtBranch.Text + "%'";
                else
                    wherecondition += " and al_branchD like '" + txtBranch.Text + "%'";
            }
            if (txtMonth.Text != "Month")
            {
                if (wherecondition == "")
                    wherecondition = " al_monthD like '" + txtMonth.Text + "%'";
                else
                    wherecondition += " and al_monthD like '" + txtMonth.Text + "%'";
            }
            if (txtYear.Text != "Year")
            {
                if (wherecondition == "")
                    wherecondition = " al_yearD like '" + txtYear.Text + "%'";
                else
                    wherecondition += " and al_yearD like '" + txtYear.Text + "%'";
            }
            if (txtLock.Text != "Status")
            {
                if (wherecondition == "")
                    wherecondition = " al_lockD like '" + txtLock.Text + "%'";
                else
                    wherecondition += " and al_lockD like '" + txtLock.Text + "%'";
            }
            if (wherecondition != "")
                SqlDataSource1.SelectCommand = " Select * from ( SELECT [al_id], [al_company],(select ltrim(rtrim(cmp_name)) from tbl_master_company where cmp_internalid=al_company) as al_companyD, [al_branch],(select branch_description from tbl_master_branch where branch_id=al_branch) as al_branchD, [al_month],(case when al_month=1 then 'Jan' when al_month=2 then 'Feb' when al_month=3 then 'Mar' when al_month=4 then 'Apr' when al_month=5 then 'May' when al_month=6 then 'Jun' when al_month=7 then 'Jul' when al_month=8 then 'Aug' when al_month=9 then 'Sep' when al_month=10 then 'Oct' when al_month=11 then 'Nov' when al_month=12 then 'Dec' else '' end) as al_monthD, [al_year], [al_lock],(case when al_lock='Y' then 'Locked' else 'Open' end) as al_lockD FROM [tbl_trans_AttendanceLock]) as D where " + wherecondition;
            GrdAttendanceLock.DataBind();
            txtCompany.Text = "Company";
            txtBranch.Text = "Branch";
            txtMonth.Text = "Month";
            txtYear.Text = "Year";
            txtLock.Text = "Status";
        }
    }
}