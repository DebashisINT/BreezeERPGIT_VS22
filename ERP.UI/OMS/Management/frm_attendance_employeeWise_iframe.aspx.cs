using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;


namespace ERP.OMS.Management
{
    public partial class management_frm_attendance_employeeWise_iframe : System.Web.UI.Page
    {
        DataTable DT_AttendanceDetail = new DataTable();
        DataTable DT_employeeWorkingShedule = new DataTable();
        DataTable DT_WorkingHour = new DataTable();
        DataTable DT_Employee = new DataTable();
        DataTable DT_Attendance = new DataTable();
        //DataTable DT_AttendanceDetail = new DataTable();
        DataTable DT_Holiday = new DataTable();
        DataTable DT_LeaveApplication = new DataTable();
        DataTable DT_LeaveApplication_DayBefore = new DataTable();
        DataTable DT_LeaveApplication_DayAfter = new DataTable();
        DataTable DT_ODOV = new DataTable();
        DataTable DT_LeaveScheme = new DataTable();
        string attendanceStatus = "";
        string inTime = "";
        string outTime = "";
        string dayOfWeek = "";
        int statrNo = 0;
        int endno = 0;
        string LeaveType = "";
        string LeaveValue = "";
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        clsDropDownList ObjDrop = new clsDropDownList();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["SortExpr"] = "datetime ASC";
                Session["mode"] = "";
                cmbDate.MaxDate = oDBEngine.GetDate();
                cmbFromdate.MaxDate = oDBEngine.GetDate();
                cmbDate.Value = oDBEngine.GetDate();
                //________This script is for firing javascript when page load first___//
                if (!ClientScript.IsStartupScriptRegistered("Today"))
                    ClientScript.RegisterStartupScript(typeof(Page), "Today", "<script>PageLoad('n');</script>");
                //______________________________End Script____________________________//
                //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
                string[,] branches = oDBEngine.GetFieldValue(" tbl_master_branch ", " branch_id,(branch_description+'['+branch_code+']') as branch ", " branch_id in (" + HttpContext.Current.Session["userbranchHierarchy"] + ")", 2, " branch_description ");
                ObjDrop.AddDataToDropDownList(branches, cmbBranch, "All");
                cmbBranch.SelectedIndex = 0;
                string[,] companies = oDBEngine.GetFieldValue(" tbl_master_company ", " cmp_id,cmp_name ", null, 2);
                ObjDrop.AddDataToDropDownList(companies, cmbCompany, "All");
                cmbCompany.SelectedIndex = 0;
                txtName.Attributes.Add("onkeyup", "CallAjax(this,'employeeBranchHrchy',event)");

                btnSave.Visible = false;
            }
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        private void BindGrid()
        {
            if (txtName_hidden.Text != "")
            {
                //__preparing Query to fetch data
                string Query = "";
                for (int i = DateTime.Parse(cmbFromdate.Value.ToString()).Year; i <= DateTime.Parse(cmbDate.Value.ToString()).Year; i++)
                {
                    if (DateTime.Parse(cmbFromdate.Value.ToString()).Month - DateTime.Parse(cmbDate.Value.ToString()).Month == 0)
                    {
                        int j = DateTime.Parse(cmbFromdate.Value.ToString()).Month;
                        for (int k = DateTime.Parse(cmbFromdate.Value.ToString()).Day; k <= DateTime.Parse(cmbDate.Value.ToString()).Day; k++)
                        {
                            if (Query == "")
                                Query = "select convert(varchar(12),cast('" + j.ToString() + "/" + k.ToString() + "/" + i.ToString() + "' as datetime),113) as date, *,(select atd_intime from tbl_trans_AttendanceDetail where atd_atdid=cast(D.atd_id as numeric) and convert(varchar(11),atd_dateofattendance,113)=convert(varchar(11),cast('" + j.ToString() + "/" + k.ToString() + "/" + i.ToString() + "' as datetime),113)) as intime," +
                                        " (select atd_outtime from tbl_trans_AttendanceDetail where atd_atdid=D.atd_id and atd_dateofattendance='" + j.ToString() + "/" + k.ToString() + "/" + i.ToString() + "') as outtime,cast('" + j.ToString() + "/" + k.ToString() + "/" + i.ToString() + "' as datetime) as datetime " +
                                        " from (select atd_id,atd_statusday" + k + " as status from tbl_trans_attendance where atd_cntid='" + txtName_hidden.Text.Trim() + "' and atd_month=month('" + j.ToString() + "/" + k.ToString() + "/" + i.ToString() + "') and atd_year=year('" + j.ToString() + "/" + k.ToString() + "/" + i.ToString() + "')) as D ";
                            else
                                Query += " UNION ALL select convert(varchar(12),cast('" + j.ToString() + "/" + k.ToString() + "/" + i.ToString() + "' as datetime),113) as date, *,(select atd_intime from tbl_trans_AttendanceDetail where atd_atdid=cast(D.atd_id as numeric) and convert(varchar(11),atd_dateofattendance,113)=convert(varchar(11),cast('" + j.ToString() + "/" + k.ToString() + "/" + i.ToString() + "' as datetime),113)) as intime," +
                                        "(select atd_outtime from tbl_trans_AttendanceDetail where atd_atdid=D.atd_id and atd_dateofattendance='" + j.ToString() + "/" + k.ToString() + "/" + i.ToString() + "') as outtime,cast('" + j.ToString() + "/" + k.ToString() + "/" + i.ToString() + "' as datetime) as datetime " +
                                        " from (select atd_id,atd_statusday" + k + " as status from tbl_trans_attendance where atd_cntid='" + txtName_hidden.Text.Trim() + "' and atd_month=month('" + j.ToString() + "/" + k.ToString() + "/" + i.ToString() + "') and atd_year=year('" + j.ToString() + "/" + k.ToString() + "/" + i.ToString() + "')) as D ";
                        }
                    }
                    else
                    {
                        for (int j = DateTime.Parse(cmbFromdate.Value.ToString()).Month; j <= DateTime.Parse(cmbDate.Value.ToString()).Month; j++)
                        {
                            if (j == DateTime.Parse(cmbFromdate.Value.ToString()).Month)
                            {
                                int lastDayOfMonth = DateTime.Parse(cmbFromdate.Value.ToString()).AddDays(-(DateTime.Parse(cmbFromdate.Value.ToString()).Day - 1)).AddMonths(1).AddDays(-1).Day;
                                for (int k = DateTime.Parse(cmbFromdate.Value.ToString()).Day; k < lastDayOfMonth; k++)
                                {
                                    if (Query == "")
                                        Query = "select convert(varchar(12),cast('" + j.ToString() + "/" + k.ToString() + "/" + i.ToString() + "' as datetime),113) as date, *,(select atd_intime from tbl_trans_AttendanceDetail where atd_atdid=cast(D.atd_id as numeric) and convert(varchar(11),atd_dateofattendance,113)=convert(varchar(11),cast('" + j.ToString() + "/" + k.ToString() + "/" + i.ToString() + "' as datetime),113)) as intime," +
                                                " (select atd_outtime from tbl_trans_AttendanceDetail where atd_atdid=D.atd_id and atd_dateofattendance='" + j.ToString() + "/" + k.ToString() + "/" + i.ToString() + "') as outtime,cast('" + j.ToString() + "/" + k.ToString() + "/" + i.ToString() + "' as datetime) as datetime " +
                                                " from (select atd_id,atd_statusday" + k + " as status from tbl_trans_attendance where atd_cntid='" + txtName_hidden.Text.Trim() + "' and atd_month=month('" + j.ToString() + "/" + k.ToString() + "/" + i.ToString() + "') and atd_year=year('" + j.ToString() + "/" + k.ToString() + "/" + i.ToString() + "')) as D ";
                                    else
                                        Query += " UNION ALL select convert(varchar(12),cast('" + j.ToString() + "/" + k.ToString() + "/" + i.ToString() + "' as datetime),113) as date, *,(select atd_intime from tbl_trans_AttendanceDetail where atd_atdid=cast(D.atd_id as numeric) and convert(varchar(11),atd_dateofattendance,113)=convert(varchar(11),cast('" + j.ToString() + "/" + k.ToString() + "/" + i.ToString() + "' as datetime),113)) as intime," +
                                                "(select atd_outtime from tbl_trans_AttendanceDetail where atd_atdid=D.atd_id and atd_dateofattendance='" + j.ToString() + "/" + k.ToString() + "/" + i.ToString() + "') as outtime,cast('" + j.ToString() + "/" + k.ToString() + "/" + i.ToString() + "' as datetime) as datetime " +
                                                " from (select atd_id,atd_statusday" + k + " as status from tbl_trans_attendance where atd_cntid='" + txtName_hidden.Text.Trim() + "' and atd_month=month('" + j.ToString() + "/" + k.ToString() + "/" + i.ToString() + "') and atd_year=year('" + j.ToString() + "/" + k.ToString() + "/" + i.ToString() + "')) as D ";
                                }
                            }
                            else if (j < DateTime.Parse(cmbDate.Value.ToString()).Month)
                            {
                                int lastDayOfMonth = DateTime.Parse("1/" + j.ToString() + "/" + i.ToString()).AddMonths(1).AddDays(-1).Day;
                                for (int k = 1; k < lastDayOfMonth; k++)
                                {
                                    Query += " UNION ALL select convert(varchar(12),cast('" + j.ToString() + "/" + k.ToString() + "/" + i.ToString() + "' as datetime),113) as date, *,(select atd_intime from tbl_trans_AttendanceDetail where atd_atdid=cast(D.atd_id as numeric) and convert(varchar(11),atd_dateofattendance,113)=convert(varchar(11),cast('" + j.ToString() + "/" + k.ToString() + "/" + i.ToString() + "' as datetime),113)) as intime," +
                                                "(select atd_outtime from tbl_trans_AttendanceDetail where atd_atdid=D.atd_id and atd_dateofattendance='" + j.ToString() + "/" + k.ToString() + "/" + i.ToString() + "') as outtime,cast('" + j.ToString() + "/" + k.ToString() + "/" + i.ToString() + "' as datetime) as datetime " +
                                                " from (select atd_id,atd_statusday" + k + " as status from tbl_trans_attendance where atd_cntid='" + txtName_hidden.Text.Trim() + "' and atd_month=month('" + j.ToString() + "/" + k.ToString() + "/" + i.ToString() + "') and atd_year=year('" + j.ToString() + "/" + k.ToString() + "/" + i.ToString() + "')) as D ";
                                }
                            }
                            else if (j == DateTime.Parse(cmbDate.Value.ToString()).Month)
                            {
                                for (int k = 1; k < DateTime.Parse(cmbDate.Value.ToString()).Day; k++)
                                {
                                    Query += " UNION ALL select convert(varchar(12),cast('" + j.ToString() + "/" + k.ToString() + "/" + i.ToString() + "' as datetime),113) as date, *,(select atd_intime from tbl_trans_AttendanceDetail where atd_atdid=cast(D.atd_id as numeric) and convert(varchar(11),atd_dateofattendance,113)=convert(varchar(11),cast('" + j.ToString() + "/" + k.ToString() + "/" + i.ToString() + "' as datetime),113)) as intime," +
                                                "(select atd_outtime from tbl_trans_AttendanceDetail where atd_atdid=D.atd_id and atd_dateofattendance='" + j.ToString() + "/" + k.ToString() + "/" + i.ToString() + "') as outtime,cast('" + j.ToString() + "/" + k.ToString() + "/" + i.ToString() + "' as datetime) as datetime " +
                                                " from (select atd_id,atd_statusday" + k + " as status from tbl_trans_attendance where atd_cntid='" + txtName_hidden.Text.Trim() + "' and atd_month=month('" + j.ToString() + "/" + k.ToString() + "/" + i.ToString() + "') and atd_year=year('" + j.ToString() + "/" + k.ToString() + "/" + i.ToString() + "')) as D ";
                                }
                            }
                        }
                    }

                }
                //SqlConnection lcon = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
                SqlConnection lcon = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                AttendanceRelatedData(lcon);
                if (Query != "")
                {
                    btnSave.Visible = true;
                    DataTable dt = oDBEngine.GetDataTable(" (" + Query + ") as D", " * ", null);
                    dt = getAddedtable(dt);
                    Session["mode"] = dt.Rows.Count.ToString();
                    dt.DefaultView.Sort = ViewState["SortExpr"].ToString();
                    grdUserAttendace.DataSource = dt;
                    grdUserAttendace.DataBind();
                    Page.ClientScript.RegisterStartupScript(GetType(), "test", "<script language='javascript'>PageLoad('" + dt.Rows.Count + "');</script>");
                }
            }
        }
        private void AttendanceRelatedData(SqlConnection lcon)
        {
            lcon.Open();
            //DT_employeeWorkingShedule = oDBEngine.GetDataTable(" tbl_master_contact contact,tbl_trans_employeeCTC ectc,tbl_master_employee emp  ", " contact.cnt_internalId  as ID,ectc.emp_workinghours   as workingID, ectc.emp_totalLeavePA", " emp.emp_contactId=contact.cnt_internalId and emp.emp_dateofjoining<='" + cmbDate.Value + "' and contact.cnt_internalId like 'EM%' and (emp.emp_dateOfLeaving is null OR emp.emp_dateOfLeaving='1/1/1900 12:00:00 AM' OR emp.emp_dateOfLeaving='1/1/1900' OR emp.emp_dateOfLeaving >='" + cmbDate.Value + "' ) and contact.cnt_branchId in (" + HttpContext.Current.Session["userbranchHierarchy"] + ") and ectc.emp_cntId=contact.cnt_internalId and (ectc.emp_effectiveuntil is null or ectc.emp_effectiveuntil >='" + cmbDate.Value + "' ) ", " contact.cnt_branchId ");
            string lsSql = " select top 1 emp_workinghours   as workingID, emp_totalLeavePA,emp_cntID as ID from tbl_trans_employeeCTC where emp_cntId='" + txtName_hidden.Text + "' ";
            SqlDataAdapter lda = new SqlDataAdapter(lsSql, lcon);
            lda.Fill(DT_employeeWorkingShedule);

            DT_WorkingHour.Rows.Clear();
            lsSql = " select * from tbl_master_WorkingHours";
            lda = new SqlDataAdapter(lsSql, lcon);
            lda.Fill(DT_WorkingHour);

            string year = "";
            for (int i = DateTime.Parse(cmbFromdate.Value.ToString()).Year; i <= DateTime.Parse(cmbDate.Value.ToString()).Year; i++)
            {
                if (year == "")
                    year = i.ToString();
                else
                    year += "," + i.ToString();
            }
            lsSql = " select * from tbl_trans_attendance where atd_cntId='" + txtName_hidden.Text + "' and atd_year in(" + year + ") ";
            lda = new SqlDataAdapter(lsSql, lcon);
            lda.Fill(DT_Attendance);

            lsSql = " select hol_id,holw_workId,h.hol_DateOfHoliday from tbl_master_holiday h,tbl_master_holiday_workingSchdule w where h.hol_id=w.holw_hol_id and h.hol_DateOfHoliday between cast('" + cmbFromdate.Value.ToString() + "' as datetime) and cast('" + cmbDate.Value.ToString() + "' as datetime) ";
            lda = new SqlDataAdapter(lsSql, lcon);
            lda.Fill(DT_Holiday);

            //_leave apllication query for date range
            lsSql = "";
            for (DateTime dtt = DateTime.Parse(cmbFromdate.Value.ToString()); dtt < DateTime.Parse(cmbDate.Value.ToString()); dtt = dtt.AddDays(1))
            {
                if (lsSql == "")
                    lsSql = "select la_cntId,la_appType,la_startDateApr,la_EndDateApr,la_joinDateTime,la_consideration,cast('" + dtt + "' as datetime) as datetime from tbl_trans_LeaveApplication where cast('" + dtt + "' as datetime) between la_startDateApr and la_joinDateTime and la_appStatus in ('AP','AM') and la_cntId='" + txtName_hidden.Text + "' ";
                else
                    lsSql += "UNION ALL select la_cntId,la_appType,la_startDateApr,la_EndDateApr,la_joinDateTime,la_consideration,cast('" + dtt + "' as datetime) as datetime from tbl_trans_LeaveApplication where cast('" + dtt + "' as datetime) between la_startDateApr and la_joinDateTime and la_appStatus in ('AP','AM') and la_cntId='" + txtName_hidden.Text + "' ";
            }
            //lsSql = "select la_cntId,la_appType,la_startDateApr,la_EndDateApr,la_joinDateTime,la_consideration from tbl_trans_LeaveApplication where getdate() between la_startDateApr and la_joinDateTime and la_appStatus in ('AP','AM')";
            lda = new SqlDataAdapter(lsSql, lcon);
            lda.Fill(DT_LeaveApplication);

            //_a day before
            lsSql = "";
            for (DateTime dtt = DateTime.Parse(cmbFromdate.Value.ToString()); dtt < DateTime.Parse(cmbDate.Value.ToString()); dtt = dtt.AddDays(1))
            {
                if (lsSql == "")
                    lsSql = "select la_cntId,la_appType,cast('" + dtt.ToString() + "' as datetime) as datetime from tbl_trans_LeaveApplication where (convert(varchar(10),dateadd(dd,-1,cast('" + dtt.ToString() + "' as datetime)),103)=convert(varchar(10),la_enddateApr,103)) and la_appStatus in ('AP','AM') and la_cntId='" + txtName_hidden.Text.Trim() + "'";
                else
                    lsSql += "UNION ALL select la_cntId,la_appType,cast('" + dtt.ToString() + "' as datetime) as datetime from tbl_trans_LeaveApplication where (convert(varchar(10),dateadd(dd,-1,cast('" + dtt.ToString() + "' as datetime)),103)=convert(varchar(10),la_enddateApr,103)) and la_appStatus in ('AP','AM') and la_cntId='" + txtName_hidden.Text.Trim() + "'";
            }
            //lsSql = "select la_cntId,la_appType from tbl_trans_LeaveApplication where (convert(varchar(10),dateadd(dd,-1,cast('" + Convert.ToDateTime(cmbDate.Value.ToString()) + "' as datetime)),103)=convert(varchar(10),la_enddateApr,103)) and la_appStatus in ('AP','AM')";
            lda = new SqlDataAdapter(lsSql, lcon);
            lda.Fill(DT_LeaveApplication_DayBefore);

            //_a day after
            lsSql = "";
            for (DateTime dtt = DateTime.Parse(cmbFromdate.Value.ToString()); dtt < DateTime.Parse(cmbDate.Value.ToString()); dtt = dtt.AddDays(1))
            {
                if (lsSql == "")
                    lsSql = "select la_cntId,la_appType,cast('" + dtt.ToString() + "' as datetime) as datetime from tbl_trans_LeaveApplication where (dateadd(dd,1,cast('" + dtt.ToString() + "' as datetime)) between la_startDateApr and la_enddateApr)  and la_appStatus in ('AP','AM') and la_cntId='" + txtName_hidden.Text.Trim() + "'";
                else
                    lsSql += "UNION ALL select la_cntId,la_appType,cast('" + dtt.ToString() + "' as datetime) as datetime from tbl_trans_LeaveApplication where (dateadd(dd,1,cast('" + dtt.ToString() + "' as datetime)) between la_startDateApr and la_enddateApr)  and la_appStatus in ('AP','AM') and la_cntId='" + txtName_hidden.Text.Trim() + "'";
            }
            //lsSql = "select la_cntId,la_appType from tbl_trans_LeaveApplication where (dateadd(dd,1,cast('" + Convert.ToDateTime(cmbDate.Value.ToString()) + "' as datetime)) between la_startDateApr and la_enddateApr)  and la_appStatus in ('AP','AM')";
            lda = new SqlDataAdapter(lsSql, lcon);
            lda.Fill(DT_LeaveApplication_DayAfter);

            lsSql = "";
            for (DateTime dtt = DateTime.Parse(cmbFromdate.Value.ToString()); dtt < DateTime.Parse(cmbDate.Value.ToString()); dtt = dtt.AddDays(1))
            {
                if (lsSql == "")
                    lsSql = "select od_cntId,od_type,od_reportTime,cast('" + dtt.ToString() + "' as datetime) as datetime from tbl_trans_officialdelay where (cast('" + dtt.ToString() + "' as datetime) between od_from and od_to) or (convert(varchar(10),'" + dtt.ToString() + "',103)=convert(varchar(10),od_to,103)) and od_cntId='" + txtName_hidden.Text.Trim() + "'";
                else
                    lsSql += "UNION ALL select od_cntId,od_type,od_reportTime,cast('" + dtt.ToString() + "' as datetime) as datetime from tbl_trans_officialdelay where (cast('" + dtt.ToString() + "' as datetime) between od_from and od_to) or (convert(varchar(10),'" + dtt.ToString() + "',103)=convert(varchar(10),od_to,103)) and od_cntId='" + txtName_hidden.Text.Trim() + "'";
            }
            //lsSql = "select od_cntId,od_type,od_reportTime from tbl_trans_officialdelay where (cast('" + Convert.ToDateTime(cmbDate.Value.ToString()) + "' as datetime) between od_from and od_to) or (convert(varchar(10),'" + Convert.ToDateTime(cmbDate.Value.ToString()) + "',103)=convert(varchar(10),od_to,103))";
            lda = new SqlDataAdapter(lsSql, lcon);
            lda.Fill(DT_ODOV);

            lsSql = "select ls_id,ls_PLcount_PSWO_PH,ls_CLcount_PSWO_PH,ls_SLcount_PSWO_PH from tbl_master_LeaveScheme";
            lda = new SqlDataAdapter(lsSql, lcon);
            lda.Fill(DT_LeaveScheme);

            lsSql = "SELECT     atd_atdID, atd_dateOfAttendance, atd_InTime, atd_OutTime FROM  tbl_trans_AttendanceDetail WHERE     cast(atd_dateOfAttendance as datetime)  between cast('" + cmbFromdate.Value.ToString() + "' as datetime) and  cast('" + cmbDate.Value.ToString() + "' as datetime)";
            lda = new SqlDataAdapter(lsSql, lcon);
            lda.Fill(DT_AttendanceDetail);

            lcon.Close();
            lda.Dispose();

        }
        private int returnfieldNo(string day)
        {
            int dayno = 0;
            if (day == "Monday")
                dayno = 2;
            if (day == "Tuesday")
                dayno = 4;
            if (day == "Wednesday")
                dayno = 6;
            if (day == "Thursday")
                dayno = 8;
            if (day == "Friday")
                dayno = 10;
            if (day == "Saturday")
                dayno = 12;
            if (day == "Sunday")
                dayno = 14;
            return dayno;
        }
        BusinessLogicLayer.Converter Oconverter = new BusinessLogicLayer.Converter();
        private DataTable getAddedtable(DataTable dt)
        {
            DataTable dt_new = dt.Clone();

            for (int i = 0; i < dt.Rows.Count - 1; i++)
            {
                if (DateTime.Parse(dt.Rows[i]["date"].ToString()).AddDays(1) != DateTime.Parse(dt.Rows[i + 1]["date"].ToString()))
                {
                    for (DateTime dtt = DateTime.Parse(dt.Rows[i]["date"].ToString()).AddDays(1); dtt < DateTime.Parse(dt.Rows[i + 1]["date"].ToString()); dtt = dtt.AddDays(1))
                    {
                        DataRow dr = dt_new.NewRow();
                        dr["date"] = Oconverter.ArrangeDate1(dtt.ToString()).Substring(0, 12);
                        dr["atd_id"] = "0";
                        dr[2] = "";
                        dr[3] = "";
                        dr[4] = "";
                        dr[5] = dtt;
                        dt_new.Rows.Add(dr);//.ImportRow(dr);

                    }
                }
            }
            if (dt_new.Rows.Count > 0)
            {
                for (int i = 0; i < dt_new.Rows.Count; i++)
                    dt.ImportRow(dt_new.Rows[i]);
            }
            return dt;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

            for (int i = 0; i < int.Parse(Session["mode"].ToString()); i++)
            {
                Label date = (Label)grdUserAttendace.Rows[i].FindControl("lblId");
                string InsertFields = "atd_cntId,atd_year,atd_Month,atd_StatusDay" + DateTime.Parse(date.Text.ToString()).Day.ToString();
                string dayYear = Convert.ToDateTime(date.Text.ToString()).Year.ToString();
                string dayMonth = Convert.ToDateTime(date.Text.ToString()).Month.ToString();

                DropDownList cmbStatus = (DropDownList)grdUserAttendace.Rows[i].FindControl("cmbStatus");

                TextBox txtIntimel = (TextBox)grdUserAttendace.Rows[i].FindControl("txtINtime");

                TextBox txtOUTtimel = (TextBox)grdUserAttendace.Rows[i].FindControl("txtOUTtime");


                int noofrow = oDBEngine.SetFieldValue(" tbl_trans_attendance ", " atd_StatusDay" + DateTime.Parse(date.Text.ToString()).Day.ToString() + "='" + cmbStatus.SelectedValue + "'", " atd_cntId='" + txtName_hidden.Text + "' and atd_year='" + dayYear.Trim() + "' and atd_Month='" + dayMonth.Trim() + "'");
                if (noofrow == 0)
                    oDBEngine.InsurtFieldValue(" tbl_trans_attendance ", " atd_cntId,atd_year,atd_Month,atd_StatusDay" + DateTime.Parse(date.Text.ToString()).Day.ToString(), "'" + txtName_hidden.Text + "','" + dayYear.Trim() + "','" + dayMonth.Trim() + "','" + cmbStatus.SelectedValue.ToString().Trim() + "'");

                string[,] atd_id = oDBEngine.GetFieldValue(" tbl_trans_attendance ", " atd_id ", " atd_cntId='" + txtName_hidden.Text + "' and atd_Month='" + dayMonth + "' and atd_year='" + dayYear + "'", 1);
                //oDBEngine.InsurtFieldValue(" tbl_trans_AttendanceDetail ", " atd_dateOfAttendance, atd_InTime,atd_OutTime,CreateDate,CreateUser ", "'" + cmbDate.Value + "','" + txtIntimel.Text + "','" + txtOUTtimel.Text + "',getdate()," + HttpContext.Current.Session["userid"].ToString());
                if (atd_id[0, 0] != "n")
                {
                    BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
                    string date1 = oconverter.DateConverterFromMMtoDD(date.Text, "mm/dd/yyyy");
                    noofrow = oDBEngine.SetFieldValue(" tbl_trans_AttendanceDetail ", " atd_InTime='" + txtIntimel.Text + "',atd_OutTime='" + txtOUTtimel.Text + "' ", " convert(varchar(10), atd_dateOfAttendance,101)=convert(varchar(10),'" + date1 + "',101) and atd_atdID=" + atd_id[0, 0]);
                    if (noofrow == 0)
                        oDBEngine.InsurtFieldValue(" tbl_trans_AttendanceDetail ", " atd_atdId,atd_dateOfAttendance, atd_InTime,atd_OutTime,CreateDate,CreateUser ", atd_id[0, 0] + ",'" + date.Text + "','" + txtIntimel.Text + "','" + txtOUTtimel.Text + "',getdate()," + HttpContext.Current.Session["userid"].ToString());
                }
            }
            Session["mode"] = "";
            DataTable DT_dummy = new DataTable();
            grdUserAttendace.DataSource = DT_dummy;
            grdUserAttendace.DataBind();
            btnSave.Visible = false;
            Page.ClientScript.RegisterStartupScript(GetType(), "test", "<script language='javascript'>PageLoad('n');</script>");
        }
        protected void grdUserAttendace_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            attendanceStatus = "";
            inTime = "";
            outTime = "";
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LeaveType = "";
                LeaveValue = "";
                //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                TextBox txtINtime = (TextBox)e.Row.FindControl("txtINtime");
                TextBox txtOUTtime = (TextBox)e.Row.FindControl("txtOUTtime");
                Label date = (Label)e.Row.FindControl("lblId");
                dayOfWeek = Convert.ToDateTime(date.Text.ToString()).DayOfWeek.ToString();
                statrNo = returnfieldNo(dayOfWeek);
                //statrNo = statrNo + 2;
                endno = statrNo + 1;
                DropDownList drpStatus = (DropDownList)e.Row.FindControl("cmbStatus");

                //___first checking wether attendance for this day is available or not
                //___If available then show the available data else filter data.
                string expression = " atd_Month='" + DateTime.Parse(date.Text).Month.ToString() + "'";// and atd_StatusDay" + dayno.Trim() + " in ('P','OD','OV','PD','A','PL','CL','SL','HC','HS','WO','PH')";
                DataRow[] attdnce = DT_Attendance.Select(expression);
                string fieldName = "atd_StatusDay" + Convert.ToDateTime(date.Text).Day.ToString().Trim();
                //_if attendance is not marked!
                if (attdnce.Length <= 0 || attdnce[0][fieldName].ToString() == "")
                {
                    #region If Attendance is not marked
                    //_____Checking working schedule alloted to user is present in the list of working schedule
                    //_____or not. if not the consider default one.
                    expression = " ID='" + txtName_hidden.Text.Trim() + "'";
                    DataRow[] data_WorkingHourEmployee = DT_employeeWorkingShedule.Select(expression);
                    DataRow[] data_WorkingHour;
                    if (data_WorkingHourEmployee[0]["workingID"].ToString() == "")
                    {
                        expression = " wor_isThisTheDefaultWorkingHourSchedule='Y'";
                        data_WorkingHour = DT_WorkingHour.Select(expression);
                    }
                    else
                    {
                        expression = "wor_id='" + data_WorkingHourEmployee[0]["workingID"].ToString().Trim() + "'";

                        data_WorkingHour = DT_WorkingHour.Select(expression);
                        if (data_WorkingHour.Length > 0)
                            expression = "";
                        else
                        {
                            expression = " wor_isThisTheDefaultWorkingHourSchedule='Y'";
                            data_WorkingHour = DT_WorkingHour.Select(expression);
                        }
                    }
                    //___Checking for any leave application given by an employee for this day or not
                    //___If leave applicatin is there then take the status, intime and out time.
                    expression = " datetime='" + date.Text.Trim() + "'";
                    DataRow[] Leave = DT_LeaveApplication.Select(expression);
                    if (Leave.Length > 0)
                    {
                        if (Convert.ToDateTime(DateTime.Parse(date.Text).ToString("MM/dd/yyyy")) <= Convert.ToDateTime(Convert.ToDateTime(Leave[0]["la_EndDateApr"]).ToString("MM/dd/yyyy")))
                        {
                            if (Leave[0]["la_consideration"].ToString().Trim() == "L")
                            {
                                attendanceStatus = Leave[0]["la_appType"].ToString();
                                LeaveType = "L";
                            }
                            else
                            {
                                attendanceStatus = "P";
                                LeaveType = "C";
                                LeaveValue = Leave[0]["la_appType"].ToString();
                                inTime = data_WorkingHour[0][statrNo].ToString();
                                outTime = data_WorkingHour[0][endno].ToString();
                            }

                        }
                        else
                        {
                            #region //___Checking for Holiday on this date!
                            expression = " holw_workId=" + data_WorkingHourEmployee[0]["workingID"].ToString().Trim();
                            DataRow[] Holiday = DT_Holiday.Select(expression);

                            if (Holiday.Length > 0)
                            {
                                #region Holiday Logic
                                expression = " datetime='" + date.Text + "'";
                                DataRow[] LeaveApplAdaybefore = DT_LeaveApplication_DayBefore.Select(expression);
                                DataRow[] LeaveApplAdayAfter = DT_LeaveApplication_DayAfter.Select(expression);
                                expression = "ls_id=" + data_WorkingHourEmployee[0]["emp_totalLeavePA"].ToString();
                                DataRow[] LeaveScheme = DT_LeaveScheme.Select(expression);
                                if (LeaveApplAdaybefore.Length <= 0 && LeaveApplAdayAfter.Length <= 0)
                                {
                                    attendanceStatus = "PH";
                                    inTime = "";
                                    outTime = "";
                                }
                                else if (LeaveApplAdaybefore.Length > 0 && LeaveApplAdayAfter.Length > 0)
                                {
                                    if (LeaveApplAdaybefore[0][1].ToString().Trim() == "PL")
                                    {
                                        if (LeaveScheme[0][1].ToString() == "Y")
                                        {
                                            attendanceStatus = LeaveApplAdaybefore[0][1].ToString();
                                        }
                                        else if (LeaveScheme[0][1].ToString() == "O")
                                        {
                                            if (LeaveApplAdaybefore.Length > 0 && LeaveApplAdayAfter.Length > 0)
                                                attendanceStatus = LeaveApplAdaybefore[0][1].ToString();
                                            else
                                                attendanceStatus = "PH";
                                        }
                                        else
                                            attendanceStatus = "PH";
                                    }
                                    else if (LeaveApplAdaybefore[0][1].ToString().Trim() == "CL")
                                    {
                                        if (LeaveScheme[0][2].ToString() == "Y")
                                        {
                                            attendanceStatus = LeaveApplAdaybefore[0][1].ToString();
                                        }
                                        else if (LeaveScheme[0][2].ToString() == "O")
                                        {
                                            if (LeaveApplAdaybefore.Length > 0 && LeaveApplAdayAfter.Length > 0)
                                                attendanceStatus = LeaveApplAdaybefore[0][1].ToString();
                                            else
                                                attendanceStatus = "PH";
                                        }
                                        else
                                            attendanceStatus = "PH";
                                    }
                                    else if (LeaveApplAdaybefore[0][1].ToString().Trim() == "SL")
                                    {
                                        if (LeaveScheme[0][3].ToString() == "Y")
                                        {
                                            attendanceStatus = LeaveApplAdaybefore[0][1].ToString();
                                        }
                                        else if (LeaveScheme[0][3].ToString() == "O")
                                        {
                                            if (LeaveApplAdaybefore.Length > 0 && LeaveApplAdayAfter.Length > 0)
                                                attendanceStatus = LeaveApplAdaybefore[0][1].ToString();
                                            else
                                                attendanceStatus = "PH";
                                        }
                                        else
                                            attendanceStatus = "PH";
                                    }
                                    else if (LeaveApplAdaybefore[0][1].ToString().Trim() == "A")
                                    {
                                        attendanceStatus = "A";
                                    }
                                }
                                else if (LeaveApplAdaybefore.Length > 0 && LeaveApplAdayAfter.Length <= 0)
                                {
                                    if (LeaveApplAdaybefore[0][1].ToString().Trim() == "PL")
                                    {
                                        if (LeaveScheme[0][1].ToString() == "Y")
                                            attendanceStatus = LeaveApplAdaybefore[0][1].ToString();
                                        else
                                            attendanceStatus = "PH";
                                    }
                                    if (LeaveApplAdaybefore[0][1].ToString().Trim() == "CL")
                                    {
                                        if (LeaveScheme[0][2].ToString() == "Y")
                                            attendanceStatus = LeaveApplAdaybefore[0][1].ToString();
                                        else
                                            attendanceStatus = "PH";
                                    }
                                    if (LeaveApplAdaybefore[0][1].ToString().Trim() == "SL")
                                    {
                                        if (LeaveScheme[0][3].ToString() == "Y")
                                            attendanceStatus = LeaveApplAdaybefore[0][1].ToString();
                                        else
                                            attendanceStatus = "PH";
                                    }
                                    else if (LeaveApplAdaybefore[0][1].ToString().Trim() == "A")
                                    {
                                        attendanceStatus = "A";
                                    }
                                }
                                else if (LeaveApplAdaybefore.Length <= 0 && LeaveApplAdayAfter.Length > 0)
                                {
                                    if (LeaveApplAdayAfter[0][1].ToString().Trim() == "PL")
                                    {
                                        if (LeaveScheme[0][1].ToString() == "Y")
                                            attendanceStatus = LeaveApplAdayAfter[0][1].ToString();
                                        else
                                            attendanceStatus = "PH";
                                    }
                                    if (LeaveApplAdayAfter[0][1].ToString().Trim() == "CL")
                                    {
                                        if (LeaveScheme[0][2].ToString() == "Y")
                                            attendanceStatus = LeaveApplAdayAfter[0][1].ToString();
                                        else
                                            attendanceStatus = "PH";
                                    }
                                    if (LeaveApplAdayAfter[0][1].ToString().Trim() == "SL")
                                    {
                                        if (LeaveScheme[0][3].ToString() == "Y")
                                            attendanceStatus = LeaveApplAdayAfter[0][1].ToString();
                                        else
                                            attendanceStatus = "PH";
                                    }
                                    else if (LeaveApplAdayAfter[0][1].ToString().Trim() == "A")
                                    {
                                        attendanceStatus = "A";
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                //___Checking for any OD/OV
                                expression = " datetime='" + date.Text + "'";
                                DataRow[] ODOV = DT_ODOV.Select(expression);
                                if (ODOV.Length > 0)
                                {
                                    if (ODOV[0]["od_type"].ToString().Trim() == "OD")
                                    {
                                        attendanceStatus = "OD";
                                        inTime = ODOV[0]["od_reportTime"].ToString();
                                    }
                                    else
                                    {
                                        attendanceStatus = "OV";
                                        inTime = data_WorkingHour[0][statrNo].ToString();
                                    }
                                    outTime = data_WorkingHour[0][endno].ToString();
                                }
                                else
                                {

                                    inTime = data_WorkingHour[0][statrNo].ToString();
                                    outTime = data_WorkingHour[0][endno].ToString();
                                    if (inTime == "")
                                        attendanceStatus = "WO";
                                    else
                                        attendanceStatus = "P";
                                }
                            }
                            #endregion
                        }

                    }
                    else
                    {
                        //___Checking for Holiday on this date!
                        expression = " holw_workId=" + data_WorkingHourEmployee[0]["workingID"].ToString().Trim();
                        DataRow[] Holiday = DT_Holiday.Select(expression);

                        if (Holiday.Length > 0)
                        {
                            #region Holiday Logic
                            expression = " datetime='" + date.Text + "'";
                            DataRow[] LeaveApplAdaybefore = DT_LeaveApplication_DayBefore.Select(expression);
                            DataRow[] LeaveApplAdayAfter = DT_LeaveApplication_DayAfter.Select(expression);
                            expression = "ls_id=" + data_WorkingHourEmployee[0]["emp_totalLeavePA"].ToString();
                            DataRow[] LeaveScheme = DT_LeaveScheme.Select(expression);
                            if (LeaveApplAdaybefore.Length <= 0 && LeaveApplAdayAfter.Length <= 0)
                            {
                                attendanceStatus = "PH";
                                inTime = "";
                                outTime = "";
                            }
                            else if (LeaveApplAdaybefore.Length > 0 && LeaveApplAdayAfter.Length > 0)
                            {
                                if (LeaveApplAdaybefore[0][1].ToString().Trim() == "PL")
                                {
                                    if (LeaveScheme[0][1].ToString() == "Y")
                                    {
                                        attendanceStatus = LeaveApplAdaybefore[0][1].ToString();
                                    }
                                    else if (LeaveScheme[0][1].ToString() == "O")
                                    {
                                        if (LeaveApplAdaybefore.Length > 0 && LeaveApplAdayAfter.Length > 0)
                                            attendanceStatus = LeaveApplAdaybefore[0][1].ToString();
                                        else
                                            attendanceStatus = "PH";
                                    }
                                    else
                                        attendanceStatus = "PH";
                                }
                                else if (LeaveApplAdaybefore[0][1].ToString().Trim() == "CL")
                                {
                                    if (LeaveScheme[0][2].ToString() == "Y")
                                    {
                                        attendanceStatus = LeaveApplAdaybefore[0][1].ToString();
                                    }
                                    else if (LeaveScheme[0][2].ToString() == "O")
                                    {
                                        if (LeaveApplAdaybefore.Length > 0 && LeaveApplAdayAfter.Length > 0)
                                            attendanceStatus = LeaveApplAdaybefore[0][1].ToString();
                                        else
                                            attendanceStatus = "PH";
                                    }
                                    else
                                        attendanceStatus = "PH";
                                }
                                else if (LeaveApplAdaybefore[0][1].ToString().Trim() == "SL")
                                {
                                    if (LeaveScheme[0][3].ToString() == "Y")
                                    {
                                        attendanceStatus = LeaveApplAdaybefore[0][1].ToString();
                                    }
                                    else if (LeaveScheme[0][3].ToString() == "O")
                                    {
                                        if (LeaveApplAdaybefore.Length > 0 && LeaveApplAdayAfter.Length > 0)
                                            attendanceStatus = LeaveApplAdaybefore[0][1].ToString();
                                        else
                                            attendanceStatus = "PH";
                                    }
                                    else
                                        attendanceStatus = "PH";
                                }
                                else if (LeaveApplAdaybefore[0][1].ToString().Trim() == "A")
                                {
                                    attendanceStatus = "A";
                                }
                            }
                            else if (LeaveApplAdaybefore.Length > 0 && LeaveApplAdayAfter.Length <= 0)
                            {
                                if (LeaveApplAdaybefore[0][1].ToString().Trim() == "PL")
                                {
                                    if (LeaveScheme[0][1].ToString() == "Y")
                                        attendanceStatus = LeaveApplAdaybefore[0][1].ToString();
                                    else
                                        attendanceStatus = "PH";
                                }
                                if (LeaveApplAdaybefore[0][1].ToString().Trim() == "CL")
                                {
                                    if (LeaveScheme[0][2].ToString() == "Y")
                                        attendanceStatus = LeaveApplAdaybefore[0][1].ToString();
                                    else
                                        attendanceStatus = "PH";
                                }
                                if (LeaveApplAdaybefore[0][1].ToString().Trim() == "SL")
                                {
                                    if (LeaveScheme[0][3].ToString() == "Y")
                                        attendanceStatus = LeaveApplAdaybefore[0][1].ToString();
                                    else
                                        attendanceStatus = "PH";
                                }
                                else if (LeaveApplAdaybefore[0][1].ToString().Trim() == "A")
                                {
                                    attendanceStatus = "A";
                                }
                            }
                            else if (LeaveApplAdaybefore.Length <= 0 && LeaveApplAdayAfter.Length > 0)
                            {
                                if (LeaveApplAdayAfter[0][1].ToString().Trim() == "PL")
                                {
                                    if (LeaveScheme[0][1].ToString() == "Y")
                                        attendanceStatus = LeaveApplAdayAfter[0][1].ToString();
                                    else
                                        attendanceStatus = "PH";
                                }
                                if (LeaveApplAdayAfter[0][1].ToString().Trim() == "CL")
                                {
                                    if (LeaveScheme[0][2].ToString() == "Y")
                                        attendanceStatus = LeaveApplAdayAfter[0][1].ToString();
                                    else
                                        attendanceStatus = "PH";
                                }
                                if (LeaveApplAdayAfter[0][1].ToString().Trim() == "SL")
                                {
                                    if (LeaveScheme[0][3].ToString() == "Y")
                                        attendanceStatus = LeaveApplAdayAfter[0][1].ToString();
                                    else
                                        attendanceStatus = "PH";
                                }
                                else if (LeaveApplAdayAfter[0][1].ToString().Trim() == "A")
                                {
                                    attendanceStatus = "A";
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            //___Checking for any OD/OV
                            expression = " datetime='" + date.Text + "'";
                            DataRow[] ODOV = DT_ODOV.Select(expression);
                            if (ODOV.Length > 0)
                            {
                                if (ODOV[0]["od_type"].ToString().Trim() == "OD")
                                {
                                    attendanceStatus = "OD";
                                    inTime = ODOV[0]["od_reportTime"].ToString();
                                }
                                else
                                {
                                    attendanceStatus = "OV";
                                    inTime = data_WorkingHour[0][statrNo].ToString();
                                }
                                outTime = data_WorkingHour[0][endno].ToString();
                            }
                            else
                            {

                                inTime = data_WorkingHour[0][statrNo].ToString();
                                outTime = data_WorkingHour[0][endno].ToString();
                                if (inTime == "")
                                    attendanceStatus = "WO";
                                else
                                    attendanceStatus = "P";
                            }
                        }
                    }
                    drpStatus.SelectedValue = attendanceStatus;
                    txtINtime.Text = inTime;
                    txtOUTtime.Text = outTime;

                    #endregion
                }
                else
                {
                    //string expression = " atd_Month='" + DateTime.Parse(date.Text).Month.ToString() + "'";
                    //DataRow[] attdnce = DT_Attendance.Select(expression);
                    drpStatus.SelectedValue = attdnce[0][DateTime.Parse(date.Text).Day + 3].ToString().Trim();
                    expression = "  atd_atdID='" + attdnce[0]["atd_id"].ToString() + "'";
                    DataRow[] InOutTime = DT_AttendanceDetail.Select(expression);
                    for (int l = 0; l < InOutTime.Length; l++)
                    {
                        string dt = DateTime.Parse(InOutTime[l]["atd_dateOfAttendance"].ToString()).ToString("MM/dd/yyyy");
                        string dt1 = DateTime.Parse(date.Text).ToString("MM/dd/yyyy");
                        //DateTime dd = System.DateTime.ParseExact(DateTime.Parse(InOutTime[l]["atd_dateOfAttendance"].ToString()).ToString("MM/dd/yyyy"), "ddd dd MMM h:mm tt yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        //DateTime dd1 = System.DateTime.ParseExact(DateTime.Parse(date.Text).ToString("MM/dd/yyyy"), "ddd dd MMM h:mm tt yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        if (dt == dt1)
                        {
                            txtINtime.Text = InOutTime[l]["atd_InTime"].ToString();
                            txtOUTtime.Text = InOutTime[l]["atd_OutTime"].ToString();
                            continue;
                        }
                    }
                }
            }
        }
        protected void grdUserAttendace_Sorting(object sender, GridViewSortEventArgs e)
        {
            //______Sorting logic goes here
            // Check the DataTable itself for the previous sort expression.
            if (ViewState["SortExpr"].ToString() == (e.SortExpression.ToString() + " ASC"))
            {
                ViewState["SortExpr"] = e.SortExpression + " DESC";
            }
            else // Handles cases where the previous sort expression was the current expression descending, another expression, or none at all.
            {
                ViewState["SortExpr"] = e.SortExpression + " ASC";
            }
            BindGrid();
        }
    }
}
