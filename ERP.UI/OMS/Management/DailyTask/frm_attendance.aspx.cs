using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;
namespace ERP.OMS.Management.DailyTask
{

    public partial class management_DailyTask_frm_attendance : ERP.OMS.ViewState_class.VSPage
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
        //Converter Oconverter = new Converter();
        BusinessLogicLayer.Converter Oconverter = new BusinessLogicLayer.Converter();
        public string pageAccess = "";
        //DBEngine oDBEngine = new DBEngine(string.Empty);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        clsDropDownList oclsDropDownList = new clsDropDownList();
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
               //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (!IsPostBack)
            {
                cmbDate.EditFormatString = Oconverter.GetDateFormat("Date");
                //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);

               // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                Session["mode"] = "";
                Session["KeyVal"] = "";
                string[,] data = oDBEngine.GetFieldValue(" tbl_master_branch", " branch_id ", " branch_id='" + HttpContext.Current.Session["userbranchID"] + "' and  branch_parentId<>0", 1);
                if (data[0, 0] != "n")
                {
                    cmbDate.MinDate = oDBEngine.GetDate().AddDays(-1);
                }
                cmbDate.MaxDate = oDBEngine.GetDate();
                cmbDate.Value = oDBEngine.GetDate();
                //________This script is for firing javascript when page load first___//
                if (!ClientScript.IsStartupScriptRegistered("Today"))
                    ClientScript.RegisterStartupScript(typeof(Page), "Today", "<script>PageLoad();</script>");
                //______________________________End Script____________________________//

                string[,] branches = oDBEngine.GetFieldValue(" tbl_master_branch ", " branch_id,(branch_description+'['+branch_code+']') as branch ", " branch_id in (" + HttpContext.Current.Session["userbranchHierarchy"] + ")", 2, " branch_description ");
                //oDBEngine.AddDataToDropDownList(branches, cmbBranch, "All");
                oclsDropDownList.AddDataToDropDownList(branches, cmbBranch, "All");
                cmbBranch.SelectedIndex = 0;
                string[,] companies = oDBEngine.GetFieldValue(" tbl_master_company ", " cmp_id,cmp_name ", null, 2);
                //oDBEngine.AddDataToDropDownList(companies, cmbCompany, "All");
                oclsDropDownList.AddDataToDropDownList(companies, cmbCompany, "All");
                cmbCompany.SelectedIndex = 0;
                txtName.Attributes.Add("onkeyup", "CallAjax(this,'employeeBranchHrchy',event)");
                MarkDefaultAttendance();
                btnSave.Visible = false;
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            }

        }

        private void MarkDefaultAttendance()
        {
           // SqlConnection lcon = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]);

            SqlConnection lcon = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

            lcon.Open();
            string lsSql = " select contact.cnt_internalId  as ID, ctc.emp_workinghours as workingID,(select emp_totalleavePA from tbl_trans_employeeCTC where emp_cntId=contact.cnt_internalId and emp_effectiveuntil is null) as LeaveScheme from tbl_master_contact contact, tbl_trans_employeeCTC ctc, tbl_master_employee emp where emp.emp_contactId=contact.cnt_internalId and emp.emp_dateofjoining<getdate() and (emp.emp_dateofLeaving is null or emp.emp_dateofLeaving='1/1/1900 12:00:00 AM' OR emp.emp_dateofLeaving >=getdate()) and contact.cnt_internalId like 'EM%' and contact.cnt_branchId in (" + HttpContext.Current.Session["userbranchHierarchy"] + ") and ctc.emp_cntId=contact.cnt_internalId and (ctc.emp_effectiveuntil is null OR ctc.emp_effectiveuntil >=getdate()) and ctc.emp_effectiveDate is not null order by contact.cnt_branchId";
            SqlDataAdapter lda = new SqlDataAdapter(lsSql, lcon);
            lda.Fill(DT_Employee);

            lsSql = " select * from tbl_master_WorkingHours";
            lda = new SqlDataAdapter(lsSql, lcon);
            lda.Fill(DT_WorkingHour);

            lsSql = " select atd_id,atd_cntId,atd_Year,atd_Month,atd_StatusDay" + oDBEngine.GetDate().Day.ToString().Trim() + " from tbl_master_contact,tbl_trans_attendance where cnt_internalId=atd_cntId and cnt_internalId like 'EM%' and cnt_branchId in (" + HttpContext.Current.Session["userbranchHierarchy"] + ") and atd_year='" + oDBEngine.GetDate().Year.ToString() + "' and atd_Month='" + oDBEngine.GetDate().Month.ToString() + "'";
            lda = new SqlDataAdapter(lsSql, lcon);
            lda.Fill(DT_Attendance);

            lsSql = " select hol_id,holw_workId from tbl_master_holiday h,tbl_master_holiday_workingSchdule w where h.hol_id=w.holw_hol_id and convert(varchar(10),h.hol_DateOfHoliday,103) = convert(varchar(10),getdate(),103)";
            lda = new SqlDataAdapter(lsSql, lcon);
            lda.Fill(DT_Holiday);

            //lsSql = "select la_cntId,la_appType from tbl_trans_LeaveApplication where ((getdate() between la_startDateApr and la_enddateApr) or (convert(varchar(10),getdate(),103)=convert(varchar(10),la_enddateApr,103))) and la_appStatus in ('AP','AM')";
            lsSql = "select la_cntId,la_appType,la_startDateApr,la_EndDateApr,la_joinDateTime,la_consideration from tbl_trans_LeaveApplication where getdate() between la_startDateApr and la_joinDateTime and la_appStatus in ('AP','AM')";
            lda = new SqlDataAdapter(lsSql, lcon);
            lda.Fill(DT_LeaveApplication);

            //_a day before
            lsSql = "select la_cntId,la_appType from tbl_trans_LeaveApplication where (convert(varchar(10),dateadd(dd,-1,getdate()),103)=convert(varchar(10),la_enddateApr,103)) and la_appStatus in ('AP','AM')";
            lda = new SqlDataAdapter(lsSql, lcon);
            lda.Fill(DT_LeaveApplication_DayBefore);

            //_a day after
            lsSql = "select la_cntId,la_appType from tbl_trans_LeaveApplication where (dateadd(dd,1,getdate()) between la_startDateApr and la_enddateApr)  and la_appStatus in ('AP','AM')";
            lda = new SqlDataAdapter(lsSql, lcon);
            lda.Fill(DT_LeaveApplication_DayAfter);

            lsSql = "select od_cntId,od_type,od_reportTime from tbl_trans_officialdelay where (getdate() between od_from and od_to) or (convert(varchar(10),getdate(),103)=convert(varchar(10),od_to,103))";
            lda = new SqlDataAdapter(lsSql, lcon);
            lda.Fill(DT_ODOV);

            lsSql = "select ls_id,ls_PLcount_PSWO_PH,ls_CLcount_PSWO_PH,ls_SLcount_PSWO_PH from tbl_master_LeaveScheme";
            lda = new SqlDataAdapter(lsSql, lcon);
            lda.Fill(DT_LeaveScheme);

            string expression = "";
            string dayno = oDBEngine.GetDate().Day.ToString();
            dayOfWeek = oDBEngine.GetDate().DayOfWeek.ToString();
            statrNo = returnfieldNo(dayOfWeek);
            endno = statrNo + 1;
            string updatefileds = "atd_StatusDay" + dayno.Trim();
            string InsertFields = "atd_cntId,atd_year,atd_Month,atd_StatusDay" + dayno.Trim();
            string AttendanceExistForAnEmployeeOrNot = "";

            string atd_id = "";
            for (int i = 0; i < DT_Employee.Rows.Count; i++)
            {
                LeaveType = "";
                LeaveValue = "";
                //_____Checking for attandance of this year, month and day for an employee
                //_____if exists the return else mark attendance!
                expression = " atd_cntId='" + DT_Employee.Rows[i]["ID"] + "' and atd_year='" + oDBEngine.GetDate().Year.ToString() + "' and atd_Month='" + oDBEngine.GetDate().Month.ToString() + "' and atd_StatusDay" + dayno.Trim() + " in ('P','OD','OV','PD','A','PL','CL','SL','HC','HS','WO','PH')";
                DataRow[] attdnce = DT_Attendance.Select(expression);
                if (attdnce.Length <= 0)
                {
                    //_____Checking for attandance of this year and month for an employee
                    //_____if exists the assign YES to variable else NO.
                    expression = " atd_cntId='" + DT_Employee.Rows[i]["ID"] + "' and atd_year='" + oDBEngine.GetDate().Year.ToString() + "' and atd_Month='" + oDBEngine.GetDate().Month.ToString() + "'";
                    attdnce = DT_Attendance.Select(expression);
                    if (attdnce.Length > 0)
                        AttendanceExistForAnEmployeeOrNot = "YES";
                    else
                        AttendanceExistForAnEmployeeOrNot = "NO";
                    //_____Checking working schedule alloted to user is present in the list of working schedule
                    //_____or not. if not the consider default one.
                    DataRow[] data_WorkingHour;
                    if (DT_Employee.Rows[i]["workingID"].ToString() == "")
                    {
                        expression = " wor_isThisTheDefaultWorkingHourSchedule='Y'";
                        data_WorkingHour = DT_WorkingHour.Select(expression);
                    }
                    else
                    {
                        expression = "wor_id=" + DT_Employee.Rows[i]["workingID"];

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
                    expression = " la_cntId='" + DT_Employee.Rows[i]["ID"] + "'";
                    DataRow[] Leave = DT_LeaveApplication.Select(expression);
                    if (Leave.Length > 0)
                    {
                        if (Convert.ToDateTime(oDBEngine.GetDate().ToString("MM/dd/yyyy")) <= Convert.ToDateTime(Convert.ToDateTime(Leave[0]["la_EndDateApr"]).ToString("MM/dd/yyyy")))
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
                            expression = " holw_workId=" + DT_Employee.Rows[i]["workingID"].ToString().Trim();
                            DataRow[] Holiday = DT_Holiday.Select(expression);

                            if (Holiday.Length > 0)
                            {
                                #region Holiday Logic
                                expression = " la_cntId='" + DT_Employee.Rows[i]["ID"] + "'";
                                DataRow[] LeaveApplAdaybefore = DT_LeaveApplication_DayBefore.Select(expression);
                                DataRow[] LeaveApplAdayAfter = DT_LeaveApplication_DayAfter.Select(expression);
                                expression = "ls_id=" + DT_Employee.Rows[i]["LeaveScheme"];
                                DataRow[] LeaveScheme = DT_LeaveScheme.Select(expression);
                                if (LeaveApplAdaybefore.Length <= 0 && LeaveApplAdayAfter.Length <= 0)
                                {
                                    attendanceStatus = "PH";
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
                                    if (LeaveApplAdaybefore[0][1].ToString().Trim() == "PL")
                                    {
                                        if (LeaveScheme[0][1].ToString() == "Y")
                                            attendanceStatus = LeaveApplAdayAfter[0][1].ToString();
                                        else
                                            attendanceStatus = "PH";
                                    }
                                    if (LeaveApplAdaybefore[0][1].ToString().Trim() == "CL")
                                    {
                                        if (LeaveScheme[0][2].ToString() == "Y")
                                            attendanceStatus = LeaveApplAdayAfter[0][1].ToString();
                                        else
                                            attendanceStatus = "PH";
                                    }
                                    if (LeaveApplAdaybefore[0][1].ToString().Trim() == "SL")
                                    {
                                        if (LeaveScheme[0][3].ToString() == "Y")
                                            attendanceStatus = LeaveApplAdayAfter[0][1].ToString();
                                        else
                                            attendanceStatus = "PH";
                                    }
                                    else if (LeaveApplAdaybefore[0][1].ToString().Trim() == "A")
                                    {
                                        attendanceStatus = "A";
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                //___Checking for any OD/OV
                                expression = " od_cntId='" + DT_Employee.Rows[i]["ID"] + "'";
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
                        expression = " holw_workId=" + DT_Employee.Rows[i]["workingID"].ToString().Trim();
                        DataRow[] Holiday = DT_Holiday.Select(expression);

                        if (Holiday.Length > 0)
                        {
                            #region Holiday Logic
                            expression = " la_cntId='" + DT_Employee.Rows[i]["ID"] + "'";
                            DataRow[] LeaveApplAdaybefore = DT_LeaveApplication_DayBefore.Select(expression);
                            DataRow[] LeaveApplAdayAfter = DT_LeaveApplication_DayAfter.Select(expression);
                            expression = "ls_id=" + DT_Employee.Rows[i]["LeaveScheme"];
                            DataRow[] LeaveScheme = DT_LeaveScheme.Select(expression);
                            if (LeaveApplAdaybefore.Length <= 0 && LeaveApplAdayAfter.Length <= 0)
                            {
                                attendanceStatus = "PH";
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
                                if (LeaveApplAdaybefore[0][1].ToString().Trim() == "PL")
                                {
                                    if (LeaveScheme[0][1].ToString() == "Y")
                                        attendanceStatus = LeaveApplAdayAfter[0][1].ToString();
                                    else
                                        attendanceStatus = "PH";
                                }
                                if (LeaveApplAdaybefore[0][1].ToString().Trim() == "CL")
                                {
                                    if (LeaveScheme[0][2].ToString() == "Y")
                                        attendanceStatus = LeaveApplAdayAfter[0][1].ToString();
                                    else
                                        attendanceStatus = "PH";
                                }
                                if (LeaveApplAdaybefore[0][1].ToString().Trim() == "SL")
                                {
                                    if (LeaveScheme[0][3].ToString() == "Y")
                                        attendanceStatus = LeaveApplAdayAfter[0][1].ToString();
                                    else
                                        attendanceStatus = "PH";
                                }
                                else if (LeaveApplAdaybefore[0][1].ToString().Trim() == "A")
                                {
                                    attendanceStatus = "A";
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            //___Checking for any OD/OV
                            expression = " od_cntId='" + DT_Employee.Rows[i]["ID"] + "'";
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
                    //___Now updating attendance table
                    SqlCommand lcmd;
                    int NoRowsEffected;
                    if (AttendanceExistForAnEmployeeOrNot == "YES")
                    {
                        //__Update
                        lsSql = " update tbl_trans_attendance set " + updatefileds + "='" + attendanceStatus + "' where atd_cntId='" + DT_Employee.Rows[i]["ID"] + "' and atd_year='" + oDBEngine.GetDate().Year.ToString() + "' and atd_Month='" + oDBEngine.GetDate().Month.ToString() + "'";
                        lcmd = new SqlCommand(lsSql, lcon);
                        NoRowsEffected = lcmd.ExecuteNonQuery();
                        atd_id = attdnce[0][0].ToString();
                    }
                    else
                    {
                        //__Insert
                        lsSql = " Insert into tbl_trans_attendance (" + InsertFields + ") values('" + DT_Employee.Rows[i]["ID"].ToString() + "','" + oDBEngine.GetDate().Year.ToString() + "','" + oDBEngine.GetDate().Month.ToString() + "','" + attendanceStatus + "')";
                        lcmd = new SqlCommand(lsSql, lcon);
                        NoRowsEffected = lcmd.ExecuteNonQuery();
                        lsSql = "select atd_id from tbl_trans_attendance where atd_cntId='" + DT_Employee.Rows[i]["ID"] + "' and atd_Month='" + oDBEngine.GetDate().Month.ToString() + "' and atd_year='" + oDBEngine.GetDate().Year.ToString() + "'";
                        SqlDataReader lsdr;
                        lcmd = new SqlCommand(lsSql, lcon);
                        lsdr = lcmd.ExecuteReader();
                        if (lsdr.HasRows)
                        {
                            while (lsdr.Read())
                                atd_id = lsdr[0].ToString();
                        }
                        lsdr.Close();
                    }
                    if (atd_id != "")
                    {
                        lsSql = " insert into tbl_trans_AttendanceDetail (atd_atdId,atd_dateOfAttendance, atd_InTime,atd_OutTime,CreateDate,CreateUser) values(" + atd_id + ",getdate(),'" + inTime + "','" + outTime + "',getdate()," + HttpContext.Current.Session["userid"].ToString() + ")";
                        lcmd = new SqlCommand(lsSql, lcon);
                        NoRowsEffected = lcmd.ExecuteNonQuery();
                    }
                    lsSql = "";
                    if (LeaveType != "")
                    {
                        string finanYear = returnFinancialYear(oDBEngine.GetDate());
                        if (LeaveType == "L")
                        {
                            if (attendanceStatus == "PL")
                                lsSql = "update tbl_trans_LeaveAccountBalance set lab_PLtotalAvailedThisYear=(lab_PLtotalAvailedThisYear+1) where lab_contactId='" + DT_Employee.Rows[i]["ID"] + "' and lab_FinancialYear='" + finanYear + "'";

                            else if (attendanceStatus == "CL")
                                lsSql = "update tbl_trans_LeaveAccountBalance set lab_CLtotalAvailedThisYear=(lab_CLtotalAvailedThisYear+1) where lab_contactId='" + DT_Employee.Rows[i]["ID"] + "' and lab_FinancialYear='" + finanYear + "'";

                            else if (attendanceStatus == "SL")
                                lsSql = "update tbl_trans_LeaveAccountBalance set lab_SLtotalAvailedThisYear=(lab_SLtotalAvailedThisYear+1) where lab_contactId='" + DT_Employee.Rows[i]["ID"] + "' and lab_FinancialYear='" + finanYear + "'";

                            else if (attendanceStatus == "ML")
                                lsSql = "update tbl_trans_LeaveAccountBalance set lab_MLCurrentYear=(lab_MLCurrentYear+1) where lab_contactId='" + DT_Employee.Rows[i]["ID"] + "' and lab_FinancialYear='" + finanYear + "'";

                        }
                        else if (LeaveType == "C")
                        {
                            if (LeaveValue == "PL")
                                lsSql = "update tbl_trans_LeaveAccountBalance set lab_PLtotalEncashedThisYear=(lab_PLtotalEncashedThisYear+1) where lab_contactId='" + DT_Employee.Rows[i]["ID"] + "' and lab_FinancialYear='" + finanYear + "'";

                            else if (LeaveValue == "CL")
                                lsSql = "update tbl_trans_LeaveAccountBalance set lab_CLtotalEncashedThisYear=(lab_CLtotalEncashedThisYear+1) where lab_contactId='" + DT_Employee.Rows[i]["ID"] + "' and lab_FinancialYear='" + finanYear + "'";

                            else if (LeaveValue == "SL")
                                lsSql = "update tbl_trans_LeaveAccountBalance set lab_SLtotalEncashedThisYear=(lab_SLtotalEncashedThisYear+1) where lab_contactId='" + DT_Employee.Rows[i]["ID"] + "' and lab_FinancialYear='" + finanYear + "'";
                        }
                        if (lsSql != "")
                        {
                            lcmd = new SqlCommand(lsSql, lcon);
                            NoRowsEffected = lcmd.ExecuteNonQuery();
                        }
                    }
                    lcmd.Dispose();
                }

            }
            DT_LeaveApplication_DayAfter.Dispose();
            DT_LeaveApplication_DayBefore.Dispose();
            DT_LeaveScheme.Dispose();
            DT_ODOV.Dispose();
            DT_LeaveApplication.Dispose();
            DT_Holiday.Dispose();
            DT_Attendance.Dispose();
            DT_WorkingHour.Dispose();

            lda.Dispose();
            DT_Employee.Dispose();
            lcon.Close();
            lcon.Dispose();


        }
        //This function hepl to find place in array respective to the day!
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
        private string returnFinancialYear(DateTime date)
        {
            string retrn = "";
            int year = date.Year;
            int month = date.Month;
            if (month <= 3)
                retrn = (year - 1) + "-" + year;
            else
                retrn = year + "-" + (year + 1);
            return retrn;
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            btnSave.Visible = true;
            //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
           // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();


           // SqlConnection lcon = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
            SqlConnection lcon = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));



            DataTable DT = new DataTable();
            string dayNo = Convert.ToDateTime(cmbDate.Value.ToString()).Day.ToString();
            string fields = "atd.atd_cntId, atd.atd_statusDay" + dayNo.Trim() + ",atddet.atd_InTime,atddet.atd_OutTime ";
            string dayYear = Convert.ToDateTime(cmbDate.Value.ToString()).Year.ToString();
            string dayMonth = Convert.ToDateTime(cmbDate.Value.ToString()).Month.ToString();
            DT_WorkingHour = oDBEngine.GetDataTable(" tbl_master_WorkingHours ", " * ", null);
            //__condition attendance of month & year is locked or not
            string AttdLockCondition = "  and LTRIM(RTRIM(contact.cnt_branchId))+LTRIM(RTRIM(ectc.emp_organization)) not in (select LTRIM(RTRIM(al_branch))+(select LTRIM(RTRIM(cmp_id)) from tbl_master_company where cmp_internalid =al_company) from tbl_trans_AttendanceLock where al_month=month(cast('" + cmbDate.Value.ToString() + "' as datetime)) and al_year=year(cast('" + cmbDate.Value.ToString() + "' as datetime)) and al_lock='Y') ";
            //                         " and ectc.emp_organization not in (select cmp_id from tbl_master_company where cmp_internalid in (select al_company from tbl_trans_AttendanceLock where al_month=month(cast('" + cmbDate.Value.ToString() + "' as datetime)) and al_year=year(cast('" + cmbDate.Value.ToString() + "' as datetime)) and al_lock='Y') ))";
            if (rbUser.SelectedItem.Value.ToString() == "A")
            {
                if (cmbBranch.SelectedItem.Value.ToString() == "All" && cmbCompany.SelectedItem.Value.ToString() == "All")
                {
                    DT = oDBEngine.GetDataTable(" tbl_master_contact contact,tbl_trans_employeeCTC ectc, tbl_master_employee emp  ", " (contact.cnt_firstName +' '+contact.cnt_middleName+' '+contact.cnt_lastName) as name,contact.cnt_shortName as EMPID ,contact.cnt_internalId  as ID, (select branch_description from tbl_master_branch where branch_id=contact.cnt_branchId) as branch ", " emp.emp_contactId=contact.cnt_internalId and emp.emp_dateOfjoining <='" + cmbDate.Value + "' and contact.cnt_internalId like 'EM%' and (emp.emp_dateOfLeaving is null OR emp.emp_dateOfLeaving='1/1/1900 12:00:00 AM' OR emp.emp_dateOfLeaving='1/1/1900' OR emp.emp_dateOfLeaving >='" + cmbDate.Value + "' ) and contact.cnt_branchId in (" + HttpContext.Current.Session["userbranchHierarchy"] + ") and ectc.emp_cntId=contact.cnt_internalId and (ectc.emp_effectiveuntil is null or (ectc.emp_effectiveuntil >'" + cmbDate.Value + "' and ectc.emp_effectiveuntil in (select top 1 emp_effectiveuntil from tbl_trans_employeeCTC  where emp_cntId=ectc.emp_cntId order by createdate desc)) ) " + AttdLockCondition, " contact.cnt_branchId ");
                    DT_employeeWorkingShedule = oDBEngine.GetDataTable(" tbl_master_contact contact,tbl_trans_employeeCTC ectc,tbl_master_employee emp  ", " contact.cnt_internalId  as ID,ectc.emp_workinghours   as workingID, ectc.emp_totalLeavePA", " emp.emp_contactId=contact.cnt_internalId and emp.emp_dateofjoining<='" + cmbDate.Value + "' and contact.cnt_internalId like 'EM%' and (emp.emp_dateOfLeaving is null OR emp.emp_dateOfLeaving='1/1/1900 12:00:00 AM' OR emp.emp_dateOfLeaving='1/1/1900' OR emp.emp_dateOfLeaving >='" + cmbDate.Value + "' ) and contact.cnt_branchId in (" + HttpContext.Current.Session["userbranchHierarchy"] + ") and ectc.emp_cntId=contact.cnt_internalId and (ectc.emp_effectiveuntil is null or ectc.emp_effectiveuntil >='" + cmbDate.Value + "' ) ", " contact.cnt_branchId ");
                }
                if (cmbBranch.SelectedItem.Value.ToString() != "All" && cmbCompany.SelectedItem.Value.ToString() == "All")
                {
                    DT = oDBEngine.GetDataTable(" tbl_master_contact contact,tbl_trans_employeeCTC ectc, tbl_master_employee emp ", " (contact.cnt_firstName +' '+contact.cnt_middleName+' '+contact.cnt_lastName) as name,contact.cnt_shortName as EMPID ,contact.cnt_internalId  as ID, (select branch_description from tbl_master_branch where branch_id=contact.cnt_branchId) as branch", " emp.emp_contactId=contact.cnt_internalId and emp.emp_dateOfjoining <='" + cmbDate.Value + "' and contact.cnt_internalId like 'EM%' and contact.cnt_internalId=ectc.emp_cntId and (ectc.emp_effectiveuntil is null or (ectc.emp_effectiveuntil >'" + cmbDate.Value + "' and ectc.emp_effectiveuntil in (select top 1 emp_effectiveuntil from tbl_trans_employeeCTC  where emp_cntId=ectc.emp_cntId order by createdate desc)) ) and contact.cnt_branchId in (" + cmbBranch.SelectedItem.Value.ToString().Trim() + ") " + AttdLockCondition, " contact.cnt_branchId ");
                    DT_employeeWorkingShedule = oDBEngine.GetDataTable(" tbl_master_contact contact,tbl_trans_employeeCTC ectc ", " contact.cnt_internalId  as ID,ectc.emp_workinghours   as workingID, ectc.emp_totalLeavePA", " contact.cnt_internalId like 'EM%' and contact.cnt_branchId in (" + cmbBranch.SelectedItem.Value.ToString().Trim() + ") and ectc.emp_cntId=contact.cnt_internalId and (ectc.emp_effectiveuntil is null or ectc.emp_effectiveuntil >='" + cmbDate.Value + "' ) ", " contact.cnt_branchId ");
                }
                if (cmbBranch.SelectedItem.Value.ToString() == "All" && cmbCompany.SelectedItem.Value.ToString() != "All")
                {
                    DT = oDBEngine.GetDataTable(" tbl_master_contact contact,tbl_trans_employeeCTC ectc, tbl_master_employee emp ", " (contact.cnt_firstName +' '+contact.cnt_middleName+' '+contact.cnt_lastName) as name,contact.cnt_shortName as EMPID ,contact.cnt_internalId  as ID, (select branch_description from tbl_master_branch where branch_id=contact.cnt_branchId) as branch", " emp.emp_contactId=contact.cnt_internalId and emp.emp_dateOfjoining <='" + cmbDate.Value + "' and contact.cnt_internalId like 'EM%' and contact.cnt_internalId=ectc.emp_cntId and (ectc.emp_effectiveuntil is null or (ectc.emp_effectiveuntil >'" + cmbDate.Value + "' and ectc.emp_effectiveuntil in (select top 1 emp_effectiveuntil from tbl_trans_employeeCTC  where emp_cntId=ectc.emp_cntId order by createdate desc)) ) and ectc.emp_Organization =" + cmbCompany.SelectedItem.Value.ToString() + " and contact.cnt_branchId in (" + HttpContext.Current.Session["userbranchHierarchy"] + ") " + AttdLockCondition, " contact.cnt_branchId ");
                    DT_employeeWorkingShedule = oDBEngine.GetDataTable(" tbl_master_contact contact,tbl_trans_employeeCTC ectc ", " contact.cnt_internalId  as ID,ectc.emp_workinghours   as workingID, ectc.emp_totalLeavePA", " contact.cnt_internalId like 'EM%' and contact.cnt_branchId in (" + HttpContext.Current.Session["userbranchHierarchy"] + ") and ectc.emp_cntId=contact.cnt_internalId and (ectc.emp_effectiveuntil is null or ectc.emp_effectiveuntil >='" + cmbDate.Value + "' ) ", " contact.cnt_branchId ");
                }
                if (cmbBranch.SelectedItem.Value.ToString() != "All" && cmbCompany.SelectedItem.Value.ToString() != "All")
                {
                    DT = oDBEngine.GetDataTable(" tbl_master_contact contact,tbl_trans_employeeCTC ectc, tbl_master_employee emp ", " (contact.cnt_firstName +' '+contact.cnt_middleName+' '+contact.cnt_lastName) as name,contact.cnt_shortName as EMPID ,contact.cnt_internalId  as ID, (select branch_description from tbl_master_branch where branch_id=contact.cnt_branchId) as branch", " emp.emp_contactId=contact.cnt_internalId and emp.emp_dateOfjoining <='" + cmbDate.Value + "' and contact.cnt_internalId like 'EM%' and contact.cnt_internalId=ectc.emp_cntId and (ectc.emp_effectiveuntil is null or (ectc.emp_effectiveuntil >'" + cmbDate.Value + "' and ectc.emp_effectiveuntil in (select top 1 emp_effectiveuntil from tbl_trans_employeeCTC  where emp_cntId=ectc.emp_cntId order by createdate desc))) and ectc.emp_Organization =" + cmbCompany.SelectedItem.Value.ToString() + " and contact.cnt_branchId in (" + cmbBranch.SelectedItem.Value.ToString().Trim() + ") " + AttdLockCondition, " contact.cnt_branchId ");
                    DT_employeeWorkingShedule = oDBEngine.GetDataTable(" tbl_master_contact contact,tbl_trans_employeeCTC ectc ", " contact.cnt_internalId  as ID,ectc.emp_workinghours   as workingID, ectc.emp_totalLeavePA", " contact.cnt_internalId like 'EM%' and contact.cnt_branchId in (" + cmbBranch.SelectedItem.Value.ToString().Trim() + ") and ectc.emp_cntId=contact.cnt_internalId and (ectc.emp_effectiveuntil is null or ectc.emp_effectiveuntil >='" + cmbDate.Value + "' ) ", " contact.cnt_branchId ");
                }
                int noOfRows = DT.Rows.Count;
                Session["mode"] = noOfRows;
                Page.ClientScript.RegisterStartupScript(GetType(), "Today", "<script>ShowEmployeeFilterForm('A');NoOfRows(" + noOfRows + ");</script>");

                //___Getting table that contains attandance related data

                AttendanceRelatedData(lcon);

                grdUserAttendace.DataSource = DT.DefaultView;
                grdUserAttendace.DataBind();
                if (DT.Rows.Count == 0)
                {
                    btnSave.Visible = false;
                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>alert('Attendance of this Month has been locked!')</script>");
                }
            }
            else
            {
                if (txtName_hidden.Text != "")
                {
                    DT = oDBEngine.GetDataTable(" tbl_master_contact contact,tbl_trans_employeeCTC ectc ", " (contact.cnt_firstName +' '+contact.cnt_middleName+' '+contact.cnt_lastName) as name,contact.cnt_shortName as EMPID ,contact.cnt_internalId  as ID, (select branch_description from tbl_master_branch where branch_id=contact.cnt_branchId) as branch", " contact.cnt_internalId ='" + txtName_hidden.Text + "' and ectc.emp_effectiveDate<='" + cmbDate.Value + "' and contact.cnt_internalId=ectc.emp_cntId and (ectc.emp_effectiveuntil is null or (ectc.emp_effectiveuntil >'" + cmbDate.Value + "' and ectc.emp_effectiveuntil in (select top 1 emp_effectiveuntil from tbl_trans_employeeCTC  where emp_cntId=ectc.emp_cntId order by createdate desc))) and contact.cnt_branchId in (" + HttpContext.Current.Session["userbranchHierarchy"] + ") " + AttdLockCondition, " contact.cnt_branchId ");
                    //DT_AttendanceDetail = oDBEngine.GetDataTable(" tbl_trans_attendance atd,tbl_trans_AttendanceDetail atddet,tbl_master_contact contact  ", fields, " atd.atd_Year='" + dayYear.Trim() + "' and atd.atd_Month='" + dayMonth.Trim() + "' and atddet.atd_atdID=atd.atd_id and contact.cnt_internalId = '" + txtName_hidden.Text + "' and contact.cnt_branchId in (" + HttpContext.Current.Session["userbranchHierarchy"] + ") and atd.atd_cntId=contact.cnt_internalId and convert(varchar(10),convert(datetime,atd_dateofattendance,101),101) = convert(varchar(10),convert(datetime,'" + cmbDate.Value.ToString().Trim() + "',101),101)");
                    DT_employeeWorkingShedule = oDBEngine.GetDataTable(" tbl_master_contact contact,tbl_trans_employeeCTC ectc ", " contact.cnt_internalId  as ID,ectc.emp_workinghours   as workingID, ectc.emp_totalLeavePA", " contact.cnt_internalId like 'EM%' and contact.cnt_branchId in (" + HttpContext.Current.Session["userbranchHierarchy"] + ") and ectc.emp_cntId=contact.cnt_internalId and ectc.emp_effectiveuntil is null", " contact.cnt_branchId ");
                }
                int noOfRows = DT.Rows.Count;
                Session["mode"] = noOfRows;
                Page.ClientScript.RegisterStartupScript(GetType(), "Today", "<script>ShowEmployeeFilterForm('S');NoOfRows(" + noOfRows + ");</script>");
                AttendanceRelatedData(lcon);

                grdUserAttendace.DataSource = DT.DefaultView;
                grdUserAttendace.DataBind();
                if (DT.Rows.Count == 0)
                {
                    btnSave.Visible = false;
                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>alert('This Employee is Not An Employee on this Date OR Attendance of this month has been locked!')</script>");
                }
            }
            this.Page.ClientScript.RegisterStartupScript(GetType(), "height", "<script>height();</script>");
        }
        private void AttendanceRelatedData(SqlConnection lcon)
        {
            lcon.Open();

            DT_WorkingHour.Rows.Clear();
            string lsSql = " select * from tbl_master_WorkingHours";
            SqlDataAdapter lda = new SqlDataAdapter(lsSql, lcon);
            lda.Fill(DT_WorkingHour);

            lsSql = " select atd_id,atd_cntId,atd_Year,atd_Month,atd_StatusDay" + Convert.ToDateTime(cmbDate.Value.ToString()).Day.ToString().Trim() + " from tbl_master_contact,tbl_trans_attendance where cnt_internalId=atd_cntId and cnt_internalId like 'EM%' and cnt_branchId in (" + HttpContext.Current.Session["userbranchHierarchy"] + ") and atd_year='" + Convert.ToDateTime(cmbDate.Value.ToString()).Year.ToString() + "' and atd_Month='" + Convert.ToDateTime(cmbDate.Value.ToString()).Month.ToString() + "'";
            lda = new SqlDataAdapter(lsSql, lcon);
            lda.Fill(DT_Attendance);

            lsSql = " select hol_id,holw_workId from tbl_master_holiday h,tbl_master_holiday_workingSchdule w where h.hol_id=w.holw_hol_id and convert(varchar(10),h.hol_DateOfHoliday,103) = convert(varchar(10),cast('" + Convert.ToDateTime(cmbDate.Value.ToString()) + "' as datetime),103)";
            lda = new SqlDataAdapter(lsSql, lcon);
            lda.Fill(DT_Holiday);

            //lsSql = "select la_cntId,la_appType from tbl_trans_LeaveApplication where (('" + Convert.ToDateTime(cmbDate.Value.ToString()) + "' between la_startDateApr and la_enddateApr) or (convert(varchar(10),cast('" + Convert.ToDateTime(cmbDate.Value.ToString()) + "' as datetime),103)=convert(varchar(10),la_enddateApr,103))) and la_appStatus in ('AP','AM')";
            lsSql = "select la_cntId,la_appType,la_startDateApr,la_EndDateApr,la_joinDateTime,la_consideration from tbl_trans_LeaveApplication where getdate() between la_startDateApr and la_joinDateTime and la_appStatus in ('AP','AM')";
            lda = new SqlDataAdapter(lsSql, lcon);
            lda.Fill(DT_LeaveApplication);

            //_a day before
            lsSql = "select la_cntId,la_appType from tbl_trans_LeaveApplication where (convert(varchar(10),dateadd(dd,-1,cast('" + Convert.ToDateTime(cmbDate.Value.ToString()) + "' as datetime)),103)=convert(varchar(10),la_enddateApr,103)) and la_appStatus in ('AP','AM')";
            lda = new SqlDataAdapter(lsSql, lcon);
            lda.Fill(DT_LeaveApplication_DayBefore);

            //_a day after
            lsSql = "select la_cntId,la_appType from tbl_trans_LeaveApplication where (dateadd(dd,1,cast('" + Convert.ToDateTime(cmbDate.Value.ToString()) + "' as datetime)) between la_startDateApr and la_enddateApr)  and la_appStatus in ('AP','AM')";
            lda = new SqlDataAdapter(lsSql, lcon);
            lda.Fill(DT_LeaveApplication_DayAfter);

            lsSql = "select od_cntId,od_type,od_reportTime from tbl_trans_officialdelay where (cast('" + Convert.ToDateTime(cmbDate.Value.ToString()) + "' as datetime) between od_from and od_to) or (convert(varchar(10),'" + Convert.ToDateTime(cmbDate.Value.ToString()) + "',103)=convert(varchar(10),od_to,103))";
            lda = new SqlDataAdapter(lsSql, lcon);
            lda.Fill(DT_ODOV);

            lsSql = "select ls_id,ls_PLcount_PSWO_PH,ls_CLcount_PSWO_PH,ls_SLcount_PSWO_PH from tbl_master_LeaveScheme";
            lda = new SqlDataAdapter(lsSql, lcon);
            lda.Fill(DT_LeaveScheme);

            lsSql = "SELECT     atd_atdID, atd_dateOfAttendance, atd_InTime, atd_OutTime FROM  tbl_trans_AttendanceDetail WHERE     convert(varchar(10),atd_dateOfAttendance ,103) = convert(varchar(10),cast('" + Convert.ToDateTime(cmbDate.Value.ToString()) + "' as datetime),103)";
            lda = new SqlDataAdapter(lsSql, lcon);
            lda.Fill(DT_AttendanceDetail);

            lcon.Close();
            lda.Dispose();
            dayOfWeek = Convert.ToDateTime(cmbDate.Value.ToString()).DayOfWeek.ToString();
            statrNo = returnfieldNo(dayOfWeek);
            endno = statrNo + 1;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);

            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

            //string[,] thismonth = oDBEngine.GetFieldValue(" tbl_trans_attendance ", " count(*) ", " atd_year='" + oDBEngine.GetDate().Year.ToString() + "' and atd_Month='" + oDBEngine.GetDate().Month.ToString() + "'", 1);
            string[,] values = new string[int.Parse(Session["mode"].ToString()), 4];
            string day = Convert.ToDateTime(cmbDate.Value.ToString()).Day.ToString();
            string InsertFields = "atd_cntId,atd_year,atd_Month,atd_StatusDay" + day.Trim();
            string dayYear = Convert.ToDateTime(cmbDate.Value.ToString()).Year.ToString();
            string dayMonth = Convert.ToDateTime(cmbDate.Value.ToString()).Month.ToString();
            for (int i = 0; i < int.Parse(Session["mode"].ToString()); i++)
            {
                Label lblInternalId = (Label)grdUserAttendace.Rows[i].FindControl("lblId");
                values[i, 0] = lblInternalId.Text;
                DropDownList cmbStatus = (DropDownList)grdUserAttendace.Rows[i].FindControl("cmbStatus");
                values[i, 1] = cmbStatus.SelectedValue;
                TextBox txtIntimel = (TextBox)grdUserAttendace.Rows[i].FindControl("txtINtime");
                values[i, 2] = txtIntimel.Text;
                TextBox txtOUTtimel = (TextBox)grdUserAttendace.Rows[i].FindControl("txtOUTtime");
                values[i, 3] = txtOUTtimel.Text;

                int noofrow = oDBEngine.SetFieldValue(" tbl_trans_attendance ", " atd_StatusDay" + day.Trim() + "='" + cmbStatus.SelectedValue + "'", " atd_cntId='" + lblInternalId.Text + "' and atd_year='" + dayYear.Trim() + "' and atd_Month='" + dayMonth.Trim() + "'");
                if (noofrow == 0)
                    oDBEngine.InsurtFieldValue(" tbl_trans_attendance ", " atd_cntId,atd_year,atd_Month,atd_StatusDay" + day.Trim(), "'" + lblInternalId.Text + "','" + dayYear.Trim() + "','" + dayMonth.Trim() + "','" + cmbStatus.SelectedValue.ToString().Trim() + "'");

                string[,] atd_id = oDBEngine.GetFieldValue(" tbl_trans_attendance ", " atd_id ", " atd_cntId='" + lblInternalId.Text + "' and atd_Month='" + dayMonth + "' and atd_year='" + dayYear + "'", 1);
                //oDBEngine.InsurtFieldValue(" tbl_trans_AttendanceDetail ", " atd_dateOfAttendance, atd_InTime,atd_OutTime,CreateDate,CreateUser ", "'" + cmbDate.Value + "','" + txtIntimel.Text + "','" + txtOUTtimel.Text + "',getdate()," + HttpContext.Current.Session["userid"].ToString());
                if (atd_id[0, 0] != "n")
                {
                    //Converter oconverter = new Converter();
                    BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
                    string date = oconverter.DateConverterFromMMtoDD(cmbDate.Value.ToString(), "mm/dd/yyyy");
                    noofrow = oDBEngine.SetFieldValue(" tbl_trans_AttendanceDetail ", " atd_InTime='" + txtIntimel.Text + "',atd_OutTime='" + txtOUTtimel.Text + "' ", " convert(varchar(10), atd_dateOfAttendance,101)=convert(varchar(10),'" + date + "',101) and atd_atdID=" + atd_id[0, 0]);
                    if (noofrow == 0)
                        oDBEngine.InsurtFieldValue(" tbl_trans_AttendanceDetail ", " atd_atdId,atd_dateOfAttendance, atd_InTime,atd_OutTime,CreateDate,CreateUser ", atd_id[0, 0] + ",'" + cmbDate.Value + "','" + txtIntimel.Text + "','" + txtOUTtimel.Text + "',getdate()," + HttpContext.Current.Session["userid"].ToString());
                }
            }

            //Page.ClientScript.RegisterStartupScript(GetType(), "Today", "<script>aftersave('" + rbUser.SelectedItem.Value + "');height();</script>");
            Session["mode"] = null;
            DataTable DT_dummy = new DataTable();
            grdUserAttendace.DataSource = DT_dummy;
            grdUserAttendace.DataBind();
            btnSave.Visible = false;
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
                //DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
               // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                TextBox txtINtime = (TextBox)e.Row.FindControl("txtINtime");
                TextBox txtOUTtime = (TextBox)e.Row.FindControl("txtOUTtime");
                Label lblInternalId = (Label)e.Row.FindControl("lblId");
                DropDownList drpStatus = (DropDownList)e.Row.FindControl("cmbStatus");

                //___first checking wether attendance for this day is available or not
                //___If available then show the available data else filter data.
                string expression = " atd_cntId='" + lblInternalId.Text + "'";// and atd_StatusDay" + dayno.Trim() + " in ('P','OD','OV','PD','A','PL','CL','SL','HC','HS','WO','PH')";
                DataRow[] attdnce = DT_Attendance.Select(expression);
                string fieldName = "atd_StatusDay" + Convert.ToDateTime(cmbDate.Value.ToString()).Day.ToString().Trim();
                //_if attendance is not marked!
                if (attdnce.Length <= 0 || attdnce[0][fieldName].ToString() == "")
                {
                    #region If Attendance is not marked
                    //_____Checking working schedule alloted to user is present in the list of working schedule
                    //_____or not. if not the consider default one.
                    expression = " ID='" + lblInternalId.Text.Trim() + "'";
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
                    expression = " la_cntId='" + lblInternalId.Text.Trim() + "'";
                    DataRow[] Leave = DT_LeaveApplication.Select(expression);
                    if (Leave.Length > 0)
                    {
                        if (Convert.ToDateTime(oDBEngine.GetDate().ToString("MM/dd/yyyy")) <= Convert.ToDateTime(Convert.ToDateTime(Leave[0]["la_EndDateApr"]).ToString("MM/dd/yyyy")))
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
                            expression = " holw_workId=" + DT_Employee.Rows[0]["workingID"].ToString().Trim();
                            DataRow[] Holiday = DT_Holiday.Select(expression);

                            if (Holiday.Length > 0)
                            {
                                #region Holiday Logic
                                expression = " la_cntId='" + lblInternalId.Text.Trim() + "'";
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
                                expression = " od_cntId='" + lblInternalId.Text.Trim() + "'";
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
                            expression = " la_cntId='" + lblInternalId.Text.Trim() + "'";
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
                            expression = " od_cntId='" + lblInternalId.Text.Trim() + "'";
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
                    int noofrow = oDBEngine.SetFieldValue(" tbl_trans_attendance ", " atd_StatusDay" + Convert.ToDateTime(cmbDate.Value.ToString()).Day.ToString() + "='" + drpStatus.SelectedValue + "'", " atd_cntId='" + lblInternalId.Text + "' and atd_year='" + Convert.ToDateTime(cmbDate.Value.ToString()).Year.ToString() + "' and atd_Month='" + Convert.ToDateTime(cmbDate.Value.ToString()).Month.ToString() + "'");
                    if (noofrow == 0)
                        noofrow = oDBEngine.InsurtFieldValue(" tbl_trans_attendance ", " atd_cntId,atd_year,atd_Month,atd_StatusDay" + Convert.ToDateTime(cmbDate.Value.ToString()).Day.ToString(), "'" + lblInternalId.Text + "','" + Convert.ToDateTime(cmbDate.Value.ToString()).Year.ToString() + "','" + Convert.ToDateTime(cmbDate.Value.ToString()).Month.ToString() + "','" + drpStatus.SelectedValue.ToString().Trim() + "'");
                    if (noofrow > 0)
                    {
                        string[,] atd_id = oDBEngine.GetFieldValue(" tbl_trans_attendance ", " atd_id ", " atd_cntId='" + lblInternalId.Text + "' and atd_Month='" + Convert.ToDateTime(cmbDate.Value.ToString()).Month.ToString() + "' and atd_year='" + Convert.ToDateTime(cmbDate.Value.ToString()).Year.ToString() + "'", 1);
                        if (atd_id[0, 0] != "n")
                        {
                            oDBEngine.InsurtFieldValue(" tbl_trans_AttendanceDetail ", " atd_atdId,atd_dateOfAttendance, atd_InTime,atd_OutTime,CreateDate,CreateUser ", atd_id[0, 0] + ",'" + Convert.ToDateTime(cmbDate.Value.ToString()) + "','" + txtINtime.Text + "','" + txtOUTtime.Text + "',getdate()," + HttpContext.Current.Session["userid"].ToString());
                        }
                    }
                    string lsSql = "";
                    if (LeaveType != "")
                    {
                        string finanYear = returnFinancialYear(oDBEngine.GetDate());
                        if (LeaveType == "L")
                        {
                            if (attendanceStatus == "PL")
                                lsSql = "update tbl_trans_LeaveAccountBalance set lab_PLtotalAvailedThisYear=(lab_PLtotalAvailedThisYear+1) where lab_contactId='" + lblInternalId.Text + "' and lab_FinancialYear='" + finanYear + "'";

                            else if (attendanceStatus == "CL")
                                lsSql = "update tbl_trans_LeaveAccountBalance set lab_CLtotalAvailedThisYear=(lab_CLtotalAvailedThisYear+1) where lab_contactId='" + lblInternalId.Text + "' and lab_FinancialYear='" + finanYear + "'";

                            else if (attendanceStatus == "SL")
                                lsSql = "update tbl_trans_LeaveAccountBalance set lab_SLtotalAvailedThisYear=(lab_SLtotalAvailedThisYear+1) where lab_contactId='" + lblInternalId.Text + "' and lab_FinancialYear='" + finanYear + "'";

                            else if (attendanceStatus == "ML")
                                lsSql = "update tbl_trans_LeaveAccountBalance set lab_MLCurrentYear=(lab_MLCurrentYear+1) where lab_contactId='" + lblInternalId.Text + "' and lab_FinancialYear='" + finanYear + "'";

                        }
                        else if (LeaveType == "C")
                        {
                            if (LeaveValue == "PL")
                                lsSql = "update tbl_trans_LeaveAccountBalance set lab_PLtotalEncashedThisYear=(lab_PLtotalEncashedThisYear+1) where lab_contactId='" + lblInternalId.Text + "' and lab_FinancialYear='" + finanYear + "'";

                            else if (LeaveValue == "CL")
                                lsSql = "update tbl_trans_LeaveAccountBalance set lab_CLtotalEncashedThisYear=(lab_CLtotalEncashedThisYear+1) where lab_contactId='" + lblInternalId.Text + "' and lab_FinancialYear='" + finanYear + "'";

                            else if (LeaveValue == "SL")
                                lsSql = "update tbl_trans_LeaveAccountBalance set lab_SLtotalEncashedThisYear=(lab_SLtotalEncashedThisYear+1) where lab_contactId='" + lblInternalId.Text + "' and lab_FinancialYear='" + finanYear + "'";
                        }
                        if (lsSql != "")
                        {
                          //  SqlConnection lcon = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]);

                            SqlConnection lcon = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

                            lcon.Open();
                            SqlCommand lcmd = new SqlCommand(lsSql, lcon);
                            int NoRowsEffected = lcmd.ExecuteNonQuery();
                            lcon.Close();
                            lcmd.Dispose();
                        }
                    }
                    #endregion
                }
                else
                {
                    drpStatus.SelectedValue = attdnce[0][4].ToString().Trim();
                    expression = "atd_atdID=" + attdnce[0]["atd_id"].ToString();
                    DataRow[] InOutTime = DT_AttendanceDetail.Select(expression);
                    txtINtime.Text = InOutTime[0]["atd_InTime"].ToString();
                    txtOUTtime.Text = InOutTime[0]["atd_OutTime"].ToString();
                }

            }
        }
        protected void grdUserAttendace_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (txtName_hidden.Text == "")
            {
                //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);

               // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();


               // SqlConnection lcon = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]);

                SqlConnection lcon = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

                DataTable DT = new DataTable();
                string dayNo = Convert.ToDateTime(cmbDate.Value.ToString()).Day.ToString();
                string fields = "atd.atd_cntId, atd.atd_statusDay" + dayNo.Trim() + ",atddet.atd_InTime,atddet.atd_OutTime ";
                string dayYear = Convert.ToDateTime(cmbDate.Value.ToString()).Year.ToString();
                string dayMonth = Convert.ToDateTime(cmbDate.Value.ToString()).Month.ToString();
                DT_WorkingHour = oDBEngine.GetDataTable(" tbl_master_WorkingHours ", " * ", null);
                string AttdLockCondition = "  and LTRIM(RTRIM(contact.cnt_branchId))+LTRIM(RTRIM(ectc.emp_organization)) not in (select LTRIM(RTRIM(al_branch))+(select LTRIM(RTRIM(cmp_id)) from tbl_master_company where cmp_internalid =al_company) from tbl_trans_AttendanceLock where al_month=month(cast('" + cmbDate.Value.ToString() + "' as datetime)) and al_year=year(cast('" + cmbDate.Value.ToString() + "' as datetime)) and al_lock='Y') ";
                //                     " and ectc.emp_organization not in (select cmp_id from tbl_master_company where cmp_internalid in (select al_company from tbl_trans_AttendanceLock where al_month=month(cast('" + cmbDate.Value.ToString() + "' as datetime)) and al_year=year(cast('" + cmbDate.Value.ToString() + "' as datetime)) and al_lock='Y') )";
                if (rbUser.SelectedItem.Value.ToString() == "A")
                {
                    if (cmbBranch.SelectedItem.Value.ToString() == "All" && cmbCompany.SelectedItem.Value.ToString() == "All")
                    {
                        DT = oDBEngine.GetDataTable(" tbl_master_contact contact,tbl_trans_employeeCTC ectc, tbl_master_employee emp  ", " (contact.cnt_firstName +' '+contact.cnt_middleName+' '+contact.cnt_lastName) as name,contact.cnt_shortName as EMPID ,contact.cnt_internalId  as ID, (select branch_description from tbl_master_branch where branch_id=contact.cnt_branchId) as branch ", " emp.emp_contactId=contact.cnt_internalId and emp.emp_dateOfjoining <='" + cmbDate.Value + "' and contact.cnt_internalId like 'EM%' and (emp.emp_dateOfLeaving is null OR emp.emp_dateOfLeaving='1/1/1900 12:00:00 AM' OR emp.emp_dateOfLeaving='1/1/1900' OR emp.emp_dateOfLeaving >='" + cmbDate.Value + "' ) and contact.cnt_branchId in (" + HttpContext.Current.Session["userbranchHierarchy"] + ") and ectc.emp_cntId=contact.cnt_internalId and (ectc.emp_effectiveuntil is null or (ectc.emp_effectiveuntil >'" + cmbDate.Value + "' and ectc.emp_effectiveuntil in (select top 1 emp_effectiveuntil from tbl_trans_employeeCTC  where emp_cntId=ectc.emp_cntId order by createdate desc)) ) " + AttdLockCondition, " contact.cnt_branchId ");
                        DT_employeeWorkingShedule = oDBEngine.GetDataTable(" tbl_master_contact contact,tbl_trans_employeeCTC ectc,tbl_master_employee emp  ", " contact.cnt_internalId  as ID,ectc.emp_workinghours   as workingID, ectc.emp_totalLeavePA", " emp.emp_contactId=contact.cnt_internalId and emp.emp_dateofjoining<='" + cmbDate.Value + "' and contact.cnt_internalId like 'EM%' and (emp.emp_dateOfLeaving is null OR emp.emp_dateOfLeaving='1/1/1900 12:00:00 AM' OR emp.emp_dateOfLeaving='1/1/1900' OR emp.emp_dateOfLeaving >='" + cmbDate.Value + "' ) and contact.cnt_branchId in (" + HttpContext.Current.Session["userbranchHierarchy"] + ") and ectc.emp_cntId=contact.cnt_internalId and (ectc.emp_effectiveuntil is null or ectc.emp_effectiveuntil >='" + cmbDate.Value + "' ) ", " contact.cnt_branchId ");
                    }
                    if (cmbBranch.SelectedItem.Value.ToString() != "All" && cmbCompany.SelectedItem.Value.ToString() == "All")
                    {
                        DT = oDBEngine.GetDataTable(" tbl_master_contact contact,tbl_trans_employeeCTC ectc, tbl_master_employee emp ", " (contact.cnt_firstName +' '+contact.cnt_middleName+' '+contact.cnt_lastName) as name,contact.cnt_shortName as EMPID ,contact.cnt_internalId  as ID, (select branch_description from tbl_master_branch where branch_id=contact.cnt_branchId) as branch", " emp.emp_contactId=contact.cnt_internalId and emp.emp_dateOfjoining <='" + cmbDate.Value + "' and contact.cnt_internalId like 'EM%' and contact.cnt_internalId=ectc.emp_cntId and (ectc.emp_effectiveuntil is null or (ectc.emp_effectiveuntil >'" + cmbDate.Value + "' and ectc.emp_effectiveuntil in (select top 1 emp_effectiveuntil from tbl_trans_employeeCTC  where emp_cntId=ectc.emp_cntId order by createdate desc)) ) and contact.cnt_branchId in (" + cmbBranch.SelectedItem.Value.ToString().Trim() + ") " + AttdLockCondition, " contact.cnt_branchId ");
                        DT_employeeWorkingShedule = oDBEngine.GetDataTable(" tbl_master_contact contact,tbl_trans_employeeCTC ectc ", " contact.cnt_internalId  as ID,ectc.emp_workinghours   as workingID, ectc.emp_totalLeavePA", " contact.cnt_internalId like 'EM%' and contact.cnt_branchId in (" + cmbBranch.SelectedItem.Value.ToString().Trim() + ") and ectc.emp_cntId=contact.cnt_internalId and (ectc.emp_effectiveuntil is null or ectc.emp_effectiveuntil >='" + cmbDate.Value + "' ) ", " contact.cnt_branchId ");
                    }
                    if (cmbBranch.SelectedItem.Value.ToString() == "All" && cmbCompany.SelectedItem.Value.ToString() != "All")
                    {
                        DT = oDBEngine.GetDataTable(" tbl_master_contact contact,tbl_trans_employeeCTC ectc, tbl_master_employee emp ", " (contact.cnt_firstName +' '+contact.cnt_middleName+' '+contact.cnt_lastName) as name,contact.cnt_shortName as EMPID ,contact.cnt_internalId  as ID, (select branch_description from tbl_master_branch where branch_id=contact.cnt_branchId) as branch", " emp.emp_contactId=contact.cnt_internalId and emp.emp_dateOfjoining <='" + cmbDate.Value + "' and contact.cnt_internalId like 'EM%' and contact.cnt_internalId=ectc.emp_cntId and (ectc.emp_effectiveuntil is null or (ectc.emp_effectiveuntil >'" + cmbDate.Value + "' and ectc.emp_effectiveuntil in (select top 1 emp_effectiveuntil from tbl_trans_employeeCTC  where emp_cntId=ectc.emp_cntId order by createdate desc)) ) and ectc.emp_Organization =" + cmbCompany.SelectedItem.Value.ToString() + " and contact.cnt_branchId in (" + HttpContext.Current.Session["userbranchHierarchy"] + ") " + AttdLockCondition, " contact.cnt_branchId ");
                        DT_employeeWorkingShedule = oDBEngine.GetDataTable(" tbl_master_contact contact,tbl_trans_employeeCTC ectc ", " contact.cnt_internalId  as ID,ectc.emp_workinghours   as workingID, ectc.emp_totalLeavePA", " contact.cnt_internalId like 'EM%' and contact.cnt_branchId in (" + HttpContext.Current.Session["userbranchHierarchy"] + ") and ectc.emp_cntId=contact.cnt_internalId and (ectc.emp_effectiveuntil is null or ectc.emp_effectiveuntil >='" + cmbDate.Value + "' ) ", " contact.cnt_branchId ");
                    }
                    if (cmbBranch.SelectedItem.Value.ToString() != "All" && cmbCompany.SelectedItem.Value.ToString() != "All")
                    {
                        DT = oDBEngine.GetDataTable(" tbl_master_contact contact,tbl_trans_employeeCTC ectc, tbl_master_employee emp ", " (contact.cnt_firstName +' '+contact.cnt_middleName+' '+contact.cnt_lastName) as name,contact.cnt_shortName as EMPID ,contact.cnt_internalId  as ID, (select branch_description from tbl_master_branch where branch_id=contact.cnt_branchId) as branch", " emp.emp_contactId=contact.cnt_internalId and emp.emp_dateOfjoining <='" + cmbDate.Value + "' and contact.cnt_internalId like 'EM%' and contact.cnt_internalId=ectc.emp_cntId and (ectc.emp_effectiveuntil is null or (ectc.emp_effectiveuntil >'" + cmbDate.Value + "' and ectc.emp_effectiveuntil in (select top 1 emp_effectiveuntil from tbl_trans_employeeCTC  where emp_cntId=ectc.emp_cntId order by createdate desc))) and ectc.emp_Organization =" + cmbCompany.SelectedItem.Value.ToString() + " and contact.cnt_branchId in (" + cmbBranch.SelectedItem.Value.ToString().Trim() + ") " + AttdLockCondition, " contact.cnt_branchId ");
                        DT_employeeWorkingShedule = oDBEngine.GetDataTable(" tbl_master_contact contact,tbl_trans_employeeCTC ectc ", " contact.cnt_internalId  as ID,ectc.emp_workinghours   as workingID, ectc.emp_totalLeavePA", " contact.cnt_internalId like 'EM%' and contact.cnt_branchId in (" + cmbBranch.SelectedItem.Value.ToString().Trim() + ") and ectc.emp_cntId=contact.cnt_internalId and (ectc.emp_effectiveuntil is null or ectc.emp_effectiveuntil >='" + cmbDate.Value + "' ) ", " contact.cnt_branchId ");
                    }
                    int noOfRows = DT.Rows.Count;
                    Session["mode"] = noOfRows;
                    Page.ClientScript.RegisterStartupScript(GetType(), "Today", "<script>ShowEmployeeFilterForm('A');NoOfRows(" + noOfRows + ");</script>");

                    //___Getting table that contains attandance related data

                    AttendanceRelatedData(lcon);

                    //______Sorting logic goes here
                    // Check the DataTable itself for the previous sort expression.
                    //if (DT.DefaultView.Sort.ToString() == (e.SortExpression.ToString() + " ASC"))
                    if (Session["KeyVal"].ToString() == (e.SortExpression.ToString() + " ASC"))
                    {
                        DT.DefaultView.Sort = e.SortExpression + " DESC";
                        Session["KeyVal"] = e.SortExpression + " DESC";
                    }
                    else // Handles cases where the previous sort expression was the current expression descending, another expression, or none at all.
                    {
                        DT.DefaultView.Sort = e.SortExpression + " ASC";
                        Session["KeyVal"] = e.SortExpression + " ASC";
                    }


                    grdUserAttendace.DataSource = DT.DefaultView;
                    grdUserAttendace.DataBind();

                }
            }

        }
    }

}