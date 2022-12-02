using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Management.ToolsUtilities
{
    public partial class management_utilities_frm_attendance_PD_calculation : System.Web.UI.Page
    {
        //DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
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

            if (!IsPostBack)
            {
                ViewState["sort"] = "";
                cmbLockI.Enabled = false;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "heigh", "<script language='javascript'>height();</script>");
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            int obj = oDBEngine.getCount(" tbl_trans_AttendanceLock ", " al_company='" + cmbCompanyI.SelectedValue + "' and al_branch=" + cmbBranchI.SelectedValue + " and al_month=" + cmbMonthI.SelectedValue + " and al_year=" + cmbYearI.SelectedValue);// + " and la_PD_Lock is not NULL");
            if (obj == 0)
            {
                PDcalculation("+", cmbCompanyI.SelectedValue, cmbBranchI.SelectedValue, cmbMonthI.SelectedValue, cmbYearI.SelectedValue);

                oDBEngine.InsurtFieldValue("tbl_trans_AttendanceLock", " al_company,al_branch,al_month,al_year,al_lock,la_PD_Lock,al_CreateDate,al_CreateUser", "'" + cmbCompanyI.SelectedValue + "'," + cmbBranchI.SelectedValue + "," + cmbMonthI.SelectedValue + "," + cmbYearI.SelectedValue + ",'" + cmbLockI.SelectedValue + "','" + cmbLockI.SelectedValue + "',getdate(),'" + HttpContext.Current.Session["userid"].ToString() + "'");
                GrdAttendanceLock.DataBind();
                Page.ClientScript.RegisterStartupScript(this.GetType(), "height", "<script language='javascript'>height();</script>");
            }
            else
                Page.ClientScript.RegisterStartupScript(this.GetType(), "heightt", "<script language='javascript'>height();alert('Data already present for selected Company,Branch,month and Year. Edit it!');</script>");
        }

        private void PDcalculation(string Operator, string Company, string branch, string month, string year)
        {
            string[,] IDs = oDBEngine.GetFieldValue(" tbl_trans_attendance inner join tbl_trans_employeeCTC on emp_cntId=atd_cntID and atd_month=" + month + " and atd_year=" + year + " and (emp_effectiveuntil is null or month(emp_effectiveuntil) >=" + month + ") and emp_organization=(select cmp_id from tbl_master_company where cmp_internalid='" + Company + "') and emp_branch=" + branch, " DISTINCT atd_cntid ", null, 1);
            if (IDs[0, 0] != "n")
            {
                for (int j = 0; j < IDs.Length; j++)
                {
                    if (IDs[j, 0] != null)
                    {
                        String PDPLCLSLcount = "(select";
                        String PLcount = string.Empty;
                        String CLcount = string.Empty;
                        String SLcount = string.Empty;
                        String HCcount = string.Empty;
                        String HScount = string.Empty;
                        String pdCount = string.Empty;
                        for (int i = 1; i <= 31; i++)
                        {
                            if (pdCount == string.Empty)
                            {
                                pdCount = " ( (SELECT (case when ATD_STATUSDAY" + i.ToString() + "='Pd' then 1 else 0 end)";
                                PLcount = " ( (SELECT (case when ATD_STATUSDAY" + i.ToString() + "='PL' then 1 else 0 end)";
                                CLcount = " ( (SELECT (case when ATD_STATUSDAY" + i.ToString() + "='CL' then 1 else 0 end)";
                                SLcount = " ( (SELECT (case when ATD_STATUSDAY" + i.ToString() + "='SL' then 1 else 0 end)";
                                HCcount = " ( (SELECT (case when ATD_STATUSDAY" + i.ToString() + "='HC' then 1 else 0 end)";
                                HScount = " ( (SELECT (case when ATD_STATUSDAY" + i.ToString() + "='HS' then 1 else 0 end)";

                            }
                            else
                            {
                                pdCount += " +(case when ATD_STATUSDAY" + i.ToString() + "='Pd' then 1 else 0 end)";
                                PLcount += " +(case when ATD_STATUSDAY" + i.ToString() + "='PL' then 1 else 0 end)";
                                CLcount += " +(case when ATD_STATUSDAY" + i.ToString() + "='CL' then 1 else 0 end)";
                                SLcount += " +(case when ATD_STATUSDAY" + i.ToString() + "='SL' then 1 else 0 end)";
                                HCcount += " +(case when ATD_STATUSDAY" + i.ToString() + "='HC' then 1 else 0 end)";
                                HScount += " +(case when ATD_STATUSDAY" + i.ToString() + "='HS' then 1 else 0 end)";
                            }
                        }
                        string TableWcond = " FROM TBL_TRANS_ATTENDANCE WHERE ATD_CNTID='" + IDs[j, 0] + "'  and atd_month=" + month + " and atd_year=" + year;
                        pdCount += TableWcond + ")) as PD";
                        PLcount += TableWcond + ")) as PL";
                        CLcount += TableWcond + ")) as CL";
                        SLcount += TableWcond + ")) as SL";
                        HCcount += TableWcond + ")) as HC";
                        HScount += TableWcond + ")) as HS";

                        PDPLCLSLcount += pdCount + "," + PLcount + "," + CLcount + "," + SLcount + "," + HCcount + "," + HScount;
                        PDPLCLSLcount += ") as D";
                        string[,] PDcountTotal = oDBEngine.GetFieldValue(PDPLCLSLcount, " * ", null, 6);
                        if (PDcountTotal[0, 0] != "n")
                        {
                            string fieldWvalue = "";
                            if (int.Parse(PDcountTotal[0, 0]) > 3)
                            {
                                decimal totalpd = (decimal)(int.Parse(PDcountTotal[0, 0]) - 3) / 2;
                                fieldWvalue += " lab_CLtotalAvailedThisYear=lab_CLtotalAvailedThisYear" + Operator + totalpd;
                                if (int.Parse(PDcountTotal[0, 2]) > 0)
                                    fieldWvalue += Operator + int.Parse(PDcountTotal[0, 2]);
                                if (int.Parse(PDcountTotal[0, 4]) > 0)
                                    fieldWvalue += Operator + int.Parse(PDcountTotal[0, 4]);

                            }
                            else if (int.Parse(PDcountTotal[0, 2]) > 0)
                            {
                                if (fieldWvalue == "")
                                    fieldWvalue += " lab_CLtotalAvailedThisYear=lab_CLtotalAvailedThisYear" + Operator + int.Parse(PDcountTotal[0, 2]);
                                else
                                    fieldWvalue += ", lab_CLtotalAvailedThisYear=lab_CLtotalAvailedThisYear" + Operator + int.Parse(PDcountTotal[0, 2]);
                                if (int.Parse(PDcountTotal[0, 4]) > 0)
                                    fieldWvalue += Operator + int.Parse(PDcountTotal[0, 4]);
                            }
                            else if (int.Parse(PDcountTotal[0, 4]) > 0)
                            {
                                if (fieldWvalue == "")
                                    fieldWvalue += " lab_CLtotalAvailedThisYear=lab_CLtotalAvailedThisYear" + Operator + int.Parse(PDcountTotal[0, 4]);
                                else
                                    fieldWvalue += ", lab_CLtotalAvailedThisYear=lab_CLtotalAvailedThisYear" + Operator + int.Parse(PDcountTotal[0, 4]);
                            }

                            if (int.Parse(PDcountTotal[0, 1]) > 0)
                            {
                                if (fieldWvalue == "")
                                    fieldWvalue += " lab_PLtotalAvailedThisYear=lab_PLtotalAvailedThisYear" + Operator + int.Parse(PDcountTotal[0, 1]);
                                else
                                    fieldWvalue += ", lab_PLtotalAvailedThisYear=lab_PLtotalAvailedThisYear" + Operator + int.Parse(PDcountTotal[0, 1]);
                            }

                            if (int.Parse(PDcountTotal[0, 3]) > 0)
                            {
                                if (fieldWvalue == "")
                                    fieldWvalue += " lab_SLtotalAvailedThisYear=lab_SLtotalAvailedThisYear" + Operator + int.Parse(PDcountTotal[0, 3]);
                                else
                                    fieldWvalue += ", lab_SLtotalAvailedThisYear=lab_SLtotalAvailedThisYear" + Operator + int.Parse(PDcountTotal[0, 3]);
                                if (int.Parse(PDcountTotal[0, 5]) > 0)
                                    fieldWvalue += Operator + int.Parse(PDcountTotal[0, 5]);
                            }
                            else if (int.Parse(PDcountTotal[0, 5]) > 0)
                            {
                                if (fieldWvalue == "")
                                    fieldWvalue += " lab_SLtotalAvailedThisYear=lab_SLtotalAvailedThisYear" + Operator + int.Parse(PDcountTotal[0, 5]);
                                else
                                    fieldWvalue += ", lab_SLtotalAvailedThisYear=lab_SLtotalAvailedThisYear" + Operator + int.Parse(PDcountTotal[0, 5]);
                            }
                            if (fieldWvalue != "")
                                oDBEngine.SetFieldValue(" tbl_trans_LeaveAccountBalance ", fieldWvalue, " lab_contactId='" + IDs[j, 0] + "'");

                        }
                    }
                }
            }
        }
        protected void GrdAttendanceLock_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            DropDownList cmbcompany = (DropDownList)GrdAttendanceLock.Rows[e.RowIndex].FindControl("cmbCompanyE");
            DropDownList cmbBranch = (DropDownList)GrdAttendanceLock.Rows[e.RowIndex].FindControl("cmbBranchE");
            DropDownList cmbmonth = (DropDownList)GrdAttendanceLock.Rows[e.RowIndex].FindControl("cmbMonthE");
            DropDownList cmbYear = (DropDownList)GrdAttendanceLock.Rows[e.RowIndex].FindControl("cmbYearE");
            DropDownList cmbLock = (DropDownList)GrdAttendanceLock.Rows[e.RowIndex].FindControl("cmbLockE");
            //__checking for previous record is PD was calculated then we have to reverse the effecr by deducting PDs from CL
            string[,] PDlast = oDBEngine.GetFieldValue(" tbl_trans_AttendanceLock ", " la_PD_lock ", " al_id=" + GrdAttendanceLock.DataKeys[e.RowIndex].Value.ToString(), 1);
            int noofrowseffected = 0;
            if (PDlast[0, 0] != "n")
            {
                if (PDlast[0, 0] != "" && PDlast[0, 0] == "Y")
                {
                    if (cmbLock.SelectedValue.Trim() != "Y")    //if we open the lock!
                    {
                        PDcalculation("-", cmbcompany.SelectedValue, cmbBranch.SelectedValue, cmbmonth.SelectedValue, cmbYear.SelectedValue);
                        noofrowseffected = oDBEngine.InsertDataFromAnotherTable(" tbl_trans_attendanceLock_log ", " tbl_trans_attendanceLock ", " al_id,al_company,al_branch,al_month,al_year,al_lock,la_PD_Lock,'M',al_createDate,al_CreateUser,al_lastmodifiedDate,al_lastmodifiedUser," + HttpContext.Current.Session["userid"].ToString() + ",getdate() ", " al_id=" + GrdAttendanceLock.DataKeys[e.RowIndex].Value.ToString());
                        if (noofrowseffected > 0)
                            oDBEngine.SetFieldValue(" tbl_trans_AttendanceLock ", " [al_lock] = '" + cmbLock.SelectedValue + "',[la_PD_Lock]= '" + cmbLock.SelectedValue + "',[al_LastModifiedDate]=getdate(),[al_LastModifiedUser]='" + HttpContext.Current.Session["userid"].ToString() + "'", " al_id=" + GrdAttendanceLock.DataKeys[e.RowIndex].Value.ToString());
                    }
                }
                else
                {
                    if (cmbLock.SelectedValue.Trim() == "Y")    //if we lock the lock!
                    {
                        PDcalculation("+", cmbcompany.SelectedValue, cmbBranch.SelectedValue, cmbmonth.SelectedValue, cmbYear.SelectedValue);
                        noofrowseffected = oDBEngine.InsertDataFromAnotherTable(" tbl_trans_attendanceLock_log ", " tbl_trans_attendanceLock ", " al_id,al_company,al_branch,al_month,al_year,al_lock,la_PD_Lock,'M',al_createDate,al_CreateUser,al_lastmodifiedDate,al_lastmodifiedUser," + HttpContext.Current.Session["userid"].ToString() + ",getdate() ", " al_id=" + GrdAttendanceLock.DataKeys[e.RowIndex].Value.ToString());
                        if (noofrowseffected > 0)
                            oDBEngine.SetFieldValue(" tbl_trans_AttendanceLock ", " [al_lock] = '" + cmbLock.SelectedValue + "',[la_PD_Lock]= '" + cmbLock.SelectedValue + "',[al_LastModifiedDate]=getdate(),[al_LastModifiedUser]='" + HttpContext.Current.Session["userid"].ToString() + "'", " al_id=" + GrdAttendanceLock.DataKeys[e.RowIndex].Value.ToString());
                    }
                }
            }
            else
            {
                if (cmbLock.SelectedValue.Trim() == "Y")    //if we lock the lock!
                {
                    PDcalculation("+", cmbcompany.SelectedValue, cmbBranch.SelectedValue, cmbmonth.SelectedValue, cmbYear.SelectedValue);
                    oDBEngine.InsurtFieldValue("tbl_trans_AttendanceLock", " al_company,al_branch,al_month,al_year,al_lock,la_PD_Lock,al_CreateDate,al_CreateUser", "'" + cmbCompanyI.SelectedValue + "'," + cmbBranchI.SelectedValue + "," + cmbMonthI.SelectedValue + "," + cmbYearI.SelectedValue + ",'" + cmbLockI.SelectedValue + "','" + cmbLockI.SelectedValue + "',getdate(),'" + HttpContext.Current.Session["userid"].ToString() + "'");
                    GrdAttendanceLock.DataBind();
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "heighttt", "<script language='javascript'>height();</script>");
                }
            }
        }

        protected void GrdAttendanceLock_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (ViewState["sort"].ToString() == (e.SortExpression.ToString() + " ASC"))
            {
                ViewState["sort"] = e.SortExpression + " DESC";
                SqlDataSource1.SelectCommand = "SELECT [al_id], [al_company],(select ltrim(rtrim(cmp_name)) from tbl_master_company where cmp_internalid=al_company) as al_companyD, [al_branch],(select branch_description from tbl_master_branch where branch_id=al_branch) as al_branchD, [al_month],(case when al_month=1 then 'Jan' when al_month=2 then 'Feb' when al_month=3 then 'Mar' when al_month=4 then 'Apr' when al_month=5 then 'May' when al_month=6 then 'Jun' when al_month=7 then 'Jul' when al_month=8 then 'Aug' when al_month=9 then 'Sep' when al_month=10 then 'Oct' when al_month=11 then 'Nov' when al_month=12 then 'Dec' else '' end) as al_monthD, [al_year], ISNULL(la_PD_Lock,'N') as la_PD_Lock,(case when la_PD_Lock='Y' then 'Locked' else 'Open' end) as al_lockD FROM [tbl_trans_AttendanceLock]  ORDER BY " + ViewState["sort"].ToString();
            }
            else
            {
                ViewState["sort"] = e.SortExpression + " ASC";
                SqlDataSource1.SelectCommand = "SELECT [al_id], [al_company],(select ltrim(rtrim(cmp_name)) from tbl_master_company where cmp_internalid=al_company) as al_companyD, [al_branch],(select branch_description from tbl_master_branch where branch_id=al_branch) as al_branchD, [al_month],(case when al_month=1 then 'Jan' when al_month=2 then 'Feb' when al_month=3 then 'Mar' when al_month=4 then 'Apr' when al_month=5 then 'May' when al_month=6 then 'Jun' when al_month=7 then 'Jul' when al_month=8 then 'Aug' when al_month=9 then 'Sep' when al_month=10 then 'Oct' when al_month=11 then 'Nov' when al_month=12 then 'Dec' else '' end) as al_monthD, [al_year],  ISNULL(la_PD_Lock,'N') as la_PD_Lock,(case when la_PD_Lock='Y' then 'Locked' else 'Open' end) as al_lockD FROM [tbl_trans_AttendanceLock]  ORDER BY " + ViewState["sort"].ToString();
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
                SqlDataSource1.SelectCommand = " Select * from ( SELECT [al_id], [al_company],(select ltrim(rtrim(cmp_name)) from tbl_master_company where cmp_internalid=al_company) as al_companyD, [al_branch],(select branch_description from tbl_master_branch where branch_id=al_branch) as al_branchD, [al_month],(case when al_month=1 then 'Jan' when al_month=2 then 'Feb' when al_month=3 then 'Mar' when al_month=4 then 'Apr' when al_month=5 then 'May' when al_month=6 then 'Jun' when al_month=7 then 'Jul' when al_month=8 then 'Aug' when al_month=9 then 'Sep' when al_month=10 then 'Oct' when al_month=11 then 'Nov' when al_month=12 then 'Dec' else '' end) as al_monthD, [al_year], [al_lock],(case when al_lock='Y' then 'Locked' else 'Open' end) as al_lockD FROM [tbl_trans_AttendanceLock] where la_PD_Lock is not NULL ) as D where  " + wherecondition;
            GrdAttendanceLock.DataBind();
            txtCompany.Text = "Company";
            txtBranch.Text = "Branch";
            txtMonth.Text = "Month";
            txtYear.Text = "Year";
            txtLock.Text = "Status";
        }
    }
}