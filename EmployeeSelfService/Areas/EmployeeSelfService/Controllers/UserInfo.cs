using EmployeeSelfService.Areas.EmployeeSelfService.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EmployeeSelfService.Areas.EmployeeSelfService.Controllers
{

    public class UserInfoController : ApiController
    {
        [HttpPost]
        
        public HttpResponseMessage getInfo(UserInfoModelInput model)
        {

            UserInfoModelOutpur oview = new UserInfoModelOutpur();

            try
            {
                if (!ModelState.IsValid)
                {
                    oview.status = "213";
                    oview.message = "Some input parameters are missing.";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, oview);
                }
                else
                {
                    DataSet dt = new DataSet();
                    String con = Convert.ToString(APIConnction.ApiConnction); ;
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("PRC_EMPLOYEESELFSERVICE", sqlcon);
                    sqlcmd.Parameters.Add("@Action", "EMP_DETAILS");
                    sqlcmd.Parameters.Add("@USER_ID", model.UserId);
                    //sqlcmd.Parameters.Add("@user_id", model.user_id);
                    //sqlcmd.Parameters.Add("@TASK_DATE", model.date);
                    //sqlcmd.Parameters.Add("@TASK", model.task);
                    //sqlcmd.Parameters.Add("@DETAILS", model.details);
                    //sqlcmd.Parameters.Add("@isCompleted", model.isCompleted);
                    //sqlcmd.Parameters.Add("@Event_Id", model.eventID);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    DataTable dts = dt.Tables[0];

                    userInformation obj = new userInformation();
                    obj.Name = Convert.ToString(dts.Rows[0]["Name"]);
                    obj.deg_designation = Convert.ToString(dts.Rows[0]["deg_designation"]);
                    obj.Date_Of_Confirmation = Convert.ToString(dts.Rows[0]["Date_Of_Confirmation"]);
                    obj.emp_dateofJoining = Convert.ToString(dts.Rows[0]["emp_dateofJoining"]);

                    obj.empCode = Convert.ToString(dts.Rows[0]["empCode"]);
                    obj.phoneNo = Convert.ToString(dts.Rows[0]["phoneNo"]);
                    obj.reportingManager = Convert.ToString(dts.Rows[0]["reportingManager"]);
                    obj.dateOfBirth = Convert.ToString(dts.Rows[0]["dateOfBirth"]);
                    obj.profileImage = Convert.ToString(dts.Rows[0]["profileImage"]);
                    obj.AddressInfo = Convert.ToString(dts.Rows[0]["AddressInfo"]);
                    obj.PanNo = Convert.ToString(dts.Rows[0]["PanNo"]);
                    obj.emailId = Convert.ToString(dts.Rows[0]["emailId"]);
                    obj.brachName = Convert.ToString(dts.Rows[0]["brachName"]);
                    obj.fathersName = Convert.ToString(dts.Rows[0]["fathersName"]);

                    oview.userinfo = obj;
                    oview.status = "200";
                    oview.message = "Successfully add task.";

                    var message = Request.CreateResponse(HttpStatusCode.OK, oview);
                    return message;
                }

            }
            catch (Exception ex)
            {
                oview.status = "209";
                oview.message = ex.Message;
                var message = Request.CreateResponse(HttpStatusCode.OK, oview);
                return message;
            }

        }

        //rev Pratik
        [HttpPost]

        public HttpResponseMessage getInfobyleaveid(UserInfoModelInput model)
        {

            UserLeaveInfoModel oview = new UserLeaveInfoModel();

            try
            {
                if (!ModelState.IsValid)
                {
                    oview.status = "213";
                    oview.message = "Some input parameters are missing.";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, oview);
                }
                else
                {
                    DataSet dt = new DataSet();
                    String con = Convert.ToString(APIConnction.ApiConnction); ;
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("PRC_EMPLOYEESELFSERVICE", sqlcon);
                    sqlcmd.Parameters.Add("@Action", "GET_MY_LEAVES_BY_ID");
                    sqlcmd.Parameters.Add("@USER_ID", model.UserId);
                    //sqlcmd.Parameters.Add("@user_id", model.user_id);
                    //sqlcmd.Parameters.Add("@TASK_DATE", model.date);
                    //sqlcmd.Parameters.Add("@TASK", model.task);
                    //sqlcmd.Parameters.Add("@DETAILS", model.details);
                    //sqlcmd.Parameters.Add("@isCompleted", model.isCompleted);
                    //sqlcmd.Parameters.Add("@Event_Id", model.eventID);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    DataTable dts = dt.Tables[0];

                    LeaveDetailsByIdClass obj = new LeaveDetailsByIdClass();
                    obj.Name = Convert.ToString(dts.Rows[0]["Name"]);
                    obj.deg_designation = Convert.ToString(dts.Rows[0]["deg_designation"]);
                    obj.Date_Of_Confirmation = Convert.ToString(dts.Rows[0]["Date_Of_Confirmation"]);
                    obj.emp_dateofJoining = Convert.ToString(dts.Rows[0]["emp_dateofJoining"]);

                    obj.empCode = Convert.ToString(dts.Rows[0]["empCode"]);
                    obj.phoneNo = Convert.ToString(dts.Rows[0]["phoneNo"]);
                    obj.reportingManager = Convert.ToString(dts.Rows[0]["reportingManager"]);
                    obj.dateOfBirth = Convert.ToString(dts.Rows[0]["dateOfBirth"]);
                    obj.profileImage = Convert.ToString(dts.Rows[0]["profileImage"]);
                    obj.AddressInfo = Convert.ToString(dts.Rows[0]["AddressInfo"]);
                    obj.PanNo = Convert.ToString(dts.Rows[0]["PanNo"]);
                    obj.emailId = Convert.ToString(dts.Rows[0]["emailId"]);
                    obj.brachName = Convert.ToString(dts.Rows[0]["brachName"]);
                    obj.fathersName = Convert.ToString(dts.Rows[0]["fathersName"]);

                    obj.ID = Convert.ToString(dts.Rows[0]["ID"]);
                    obj.USER_ID = Convert.ToString(dts.Rows[0]["USER_ID"]);
                    obj.LEAVE_START_DATE = Convert.ToString(dts.Rows[0]["LEAVE_START_DATE"]);
                    obj.LEAVE_END_DATE = Convert.ToString(dts.Rows[0]["LEAVE_END_DATE"]);
                    obj.LEAVE_TYPE = Convert.ToString(dts.Rows[0]["LEAVE_TYPE"]);
                    obj.LEAVE_REASON = Convert.ToString(dts.Rows[0]["LEAVE_REASON"]);
                    obj.CREATED_DATE = Convert.ToString(dts.Rows[0]["CREATED_DATE"]);
                    obj.CURRENT_STATUS = Convert.ToString(dts.Rows[0]["CURRENT_STATUS"]);
                    obj.SUPERVISOR_NAME = Convert.ToString(dts.Rows[0]["SUPERVISOR_NAME"]);
                    obj.SUPERVISOR_EMAIL = Convert.ToString(dts.Rows[0]["SUPERVISOR_EMAIL"]);
                    obj.SUPERVISOR_ID = Convert.ToString(dts.Rows[0]["SUPERVISOR_ID"]);
                    obj.EMPCODE = Convert.ToString(dts.Rows[0]["EMPCODE"]);
                    obj.ERPLeave_ID = Convert.ToString(dts.Rows[0]["ERPLeave_ID"]);

                    oview.leavedetinfo = obj;
                    oview.status = "200";
                    oview.message = "Successfully add task.";

                    var message = Request.CreateResponse(HttpStatusCode.OK, oview);
                    return message;
                }

            }
            catch (Exception ex)
            {
                oview.status = "209";
                oview.message = ex.Message;
                var message = Request.CreateResponse(HttpStatusCode.OK, oview);
                return message;
            }

        }

        [HttpPost]

        public HttpResponseMessage getInfobyWFHid(UserInfoModelInput model)
        {

            UserWfhInfoModel oview = new UserWfhInfoModel();

            try
            {
                if (!ModelState.IsValid)
                {
                    oview.status = "213";
                    oview.message = "Some input parameters are missing.";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, oview);
                }
                else
                {
                    DataSet dt = new DataSet();
                    String con = Convert.ToString(APIConnction.ApiConnction); ;
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("PRC_EMPLOYEESELFSERVICE", sqlcon);
                    sqlcmd.Parameters.Add("@Action", "GET_MY_WFH_BY_ID");
                    sqlcmd.Parameters.Add("@USER_ID", model.UserId);
                    //sqlcmd.Parameters.Add("@user_id", model.user_id);
                    //sqlcmd.Parameters.Add("@TASK_DATE", model.date);
                    //sqlcmd.Parameters.Add("@TASK", model.task);
                    //sqlcmd.Parameters.Add("@DETAILS", model.details);
                    //sqlcmd.Parameters.Add("@isCompleted", model.isCompleted);
                    //sqlcmd.Parameters.Add("@Event_Id", model.eventID);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    DataTable dts = dt.Tables[0];

                    WFHInfoById obj = new WFHInfoById();
                    obj.Name = Convert.ToString(dts.Rows[0]["Name"]);
                    obj.deg_designation = Convert.ToString(dts.Rows[0]["deg_designation"]);
                    obj.Date_Of_Confirmation = Convert.ToString(dts.Rows[0]["Date_Of_Confirmation"]);
                    obj.emp_dateofJoining = Convert.ToString(dts.Rows[0]["emp_dateofJoining"]);

                    obj.empCode = Convert.ToString(dts.Rows[0]["empCode"]);
                    obj.phoneNo = Convert.ToString(dts.Rows[0]["phoneNo"]);
                    obj.reportingManager = Convert.ToString(dts.Rows[0]["reportingManager"]);
                    obj.dateOfBirth = Convert.ToString(dts.Rows[0]["dateOfBirth"]);
                    obj.profileImage = Convert.ToString(dts.Rows[0]["profileImage"]);
                    obj.AddressInfo = Convert.ToString(dts.Rows[0]["AddressInfo"]);
                    obj.PanNo = Convert.ToString(dts.Rows[0]["PanNo"]);
                    obj.emailId = Convert.ToString(dts.Rows[0]["emailId"]);
                    obj.brachName = Convert.ToString(dts.Rows[0]["brachName"]);
                    obj.fathersName = Convert.ToString(dts.Rows[0]["fathersName"]);

                    obj.ID = Convert.ToString(dts.Rows[0]["ID"]);
                    obj.USER_ID = Convert.ToString(dts.Rows[0]["USER_ID"]);
                    obj.WFH_STARTDATE = Convert.ToString(dts.Rows[0]["WFH_STARTDATE"]);
                    obj.WFH_ENDDATE = Convert.ToString(dts.Rows[0]["WFH_ENDDATE"]);
                    obj.EMPCODE = Convert.ToString(dts.Rows[0]["EMPCODE"]);
                    obj.WFH_REASON = Convert.ToString(dts.Rows[0]["WFH_REASON"]);
                    obj.WORK_PLAN = Convert.ToString(dts.Rows[0]["WORK_PLAN"]);
                    obj.APPLIED_DATE = Convert.ToString(dts.Rows[0]["APPLIED_DATE"]);
                    obj.CURRENT_STATUS = Convert.ToString(dts.Rows[0]["CURRENT_STATUS"]);
                    obj.SUPERVISOR_NAME = Convert.ToString(dts.Rows[0]["SUPERVISOR_NAME"]);
                    obj.SUPERVISOR_EMAIL = Convert.ToString(dts.Rows[0]["SUPERVISOR_EMAIL"]);
                    obj.SUPERVISOR_ID = Convert.ToString(dts.Rows[0]["SUPERVISOR_ID"]);                    

                    oview.WFHdetinfo = obj;
                    oview.status = "200";
                    oview.message = "Successfully add task.";

                    var message = Request.CreateResponse(HttpStatusCode.OK, oview);
                    return message;
                }

            }
            catch (Exception ex)
            {
                oview.status = "209";
                oview.message = ex.Message;
                var message = Request.CreateResponse(HttpStatusCode.OK, oview);
                return message;
            }

        }

        [HttpPost]

        public HttpResponseMessage getInfobyBMeetingid(UserBMeetingInfoModelInput model)
        {

            UserBMeetingInfoModel oview = new UserBMeetingInfoModel();

            try
            {
                if (!ModelState.IsValid)
                {
                    oview.status = "213";
                    oview.message = "Some input parameters are missing.";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, oview);
                }
                else
                {
                    DataSet dt = new DataSet();
                    String con = Convert.ToString(APIConnction.ApiConnction); ;
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("PRC_EMPLOYEESELFSERVICE", sqlcon);
                    sqlcmd.Parameters.Add("@Action", "GET_MY_BMEETING_BY_ID");
                    sqlcmd.Parameters.Add("@USER_ID", model.UserId);
                    sqlcmd.Parameters.Add("@EMPCODE", model.EmpCode);
                    //sqlcmd.Parameters.Add("@user_id", model.user_id);
                    //sqlcmd.Parameters.Add("@TASK_DATE", model.date);
                    //sqlcmd.Parameters.Add("@TASK", model.task);
                    //sqlcmd.Parameters.Add("@DETAILS", model.details);
                    //sqlcmd.Parameters.Add("@isCompleted", model.isCompleted);
                    //sqlcmd.Parameters.Add("@Event_Id", model.eventID);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    DataTable dts = dt.Tables[0];

                    BMeetingInfoById obj = new BMeetingInfoById();
                    obj.Name = Convert.ToString(dts.Rows[0]["Name"]);
                    obj.deg_designation = Convert.ToString(dts.Rows[0]["deg_designation"]);
                    obj.Date_Of_Confirmation = Convert.ToString(dts.Rows[0]["Date_Of_Confirmation"]);
                    obj.emp_dateofJoining = Convert.ToString(dts.Rows[0]["emp_dateofJoining"]);

                    obj.empCode = Convert.ToString(dts.Rows[0]["empCode"]);
                    obj.phoneNo = Convert.ToString(dts.Rows[0]["phoneNo"]);
                    obj.reportingManager = Convert.ToString(dts.Rows[0]["reportingManager"]);
                    obj.dateOfBirth = Convert.ToString(dts.Rows[0]["dateOfBirth"]);
                    obj.profileImage = Convert.ToString(dts.Rows[0]["profileImage"]);
                    obj.AddressInfo = Convert.ToString(dts.Rows[0]["AddressInfo"]);
                    obj.PanNo = Convert.ToString(dts.Rows[0]["PanNo"]);
                    obj.emailId = Convert.ToString(dts.Rows[0]["emailId"]);
                    obj.brachName = Convert.ToString(dts.Rows[0]["brachName"]);
                    obj.fathersName = Convert.ToString(dts.Rows[0]["fathersName"]);

                    obj.ID = Convert.ToString(dts.Rows[0]["ID"]);
                    obj.FROMDATE = Convert.ToString(dts.Rows[0]["FROMDATE"]);
                    obj.TODATE = Convert.ToString(dts.Rows[0]["TODATE"]);
                    obj.EMPCODE = Convert.ToString(dts.Rows[0]["EMPCODE"]);
                    obj.VISIT_IN = Convert.ToString(dts.Rows[0]["VISIT_IN"]);
                    obj.CLIENT_DETAILS = Convert.ToString(dts.Rows[0]["CLIENT_DETAILS"]);
                    obj.AGENDA = Convert.ToString(dts.Rows[0]["AGENDA"]);
                    //obj.SUPERVISOR_NAME = Convert.ToString(dts.Rows[0]["SUPERVISOR_NAME"]);
                    //obj.SUPERVISOR_EMAIL = Convert.ToString(dts.Rows[0]["SUPERVISOR_EMAIL"]);
                    //obj.SUPERVISOR_ID = Convert.ToString(dts.Rows[0]["SUPERVISOR_ID"]);

                    oview.BMeetingdetinfo = obj;
                    oview.status = "200";
                    oview.message = "Successfully add task.";

                    var message = Request.CreateResponse(HttpStatusCode.OK, oview);
                    return message;
                }

            }
            catch (Exception ex)
            {
                oview.status = "209";
                oview.message = ex.Message;
                var message = Request.CreateResponse(HttpStatusCode.OK, oview);
                return message;
            }

        }

        [HttpPost]

        public HttpResponseMessage getInfobyReimbursementid(UserBMeetingInfoModelInput model)
        {

            UserReimbursmentInfoModel oview = new UserReimbursmentInfoModel();

            try
            {
                if (!ModelState.IsValid)
                {
                    oview.status = "213";
                    oview.message = "Some input parameters are missing.";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, oview);
                }
                else
                {
                    DataSet dt = new DataSet();
                    String con = Convert.ToString(APIConnction.ApiConnction); ;
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("PRC_EMPLOYEESELFSERVICE", sqlcon);
                    sqlcmd.Parameters.Add("@Action", "GET_MY_REIMBURSEMENT_BY_ID");
                    sqlcmd.Parameters.Add("@USER_ID", model.UserId);
                    sqlcmd.Parameters.Add("@EMPCODE", model.EmpCode);
                    //sqlcmd.Parameters.Add("@user_id", model.user_id);
                    //sqlcmd.Parameters.Add("@TASK_DATE", model.date);
                    //sqlcmd.Parameters.Add("@TASK", model.task);
                    //sqlcmd.Parameters.Add("@DETAILS", model.details);
                    //sqlcmd.Parameters.Add("@isCompleted", model.isCompleted);
                    //sqlcmd.Parameters.Add("@Event_Id", model.eventID);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    DataTable dts = dt.Tables[0];

                    ReimbursmentInfoById obj = new ReimbursmentInfoById();
                    obj.Name = Convert.ToString(dts.Rows[0]["Name"]);
                    obj.deg_designation = Convert.ToString(dts.Rows[0]["deg_designation"]);
                    obj.Date_Of_Confirmation = Convert.ToString(dts.Rows[0]["Date_Of_Confirmation"]);
                    obj.emp_dateofJoining = Convert.ToString(dts.Rows[0]["emp_dateofJoining"]);

                    obj.empCode = Convert.ToString(dts.Rows[0]["empCode"]);
                    obj.phoneNo = Convert.ToString(dts.Rows[0]["phoneNo"]);
                    obj.reportingManager = Convert.ToString(dts.Rows[0]["reportingManager"]);
                    obj.dateOfBirth = Convert.ToString(dts.Rows[0]["dateOfBirth"]);
                    obj.profileImage = Convert.ToString(dts.Rows[0]["profileImage"]);
                    obj.AddressInfo = Convert.ToString(dts.Rows[0]["AddressInfo"]);
                    obj.PanNo = Convert.ToString(dts.Rows[0]["PanNo"]);
                    obj.emailId = Convert.ToString(dts.Rows[0]["emailId"]);
                    obj.brachName = Convert.ToString(dts.Rows[0]["brachName"]);
                    obj.fathersName = Convert.ToString(dts.Rows[0]["fathersName"]);

                    obj.ID = Convert.ToString(dts.Rows[0]["ID"]);
                    obj.RIMDATE = Convert.ToString(dts.Rows[0]["RIMDATE"]);
                    obj.RIMTYPE = Convert.ToString(dts.Rows[0]["RIMTYPE"]);
                    obj.EMPCODE = Convert.ToString(dts.Rows[0]["EMPCODE"]);
                    obj.RIMCAT = Convert.ToString(dts.Rows[0]["RIMCAT"]);
                    obj.FARE_AMT = Convert.ToString(dts.Rows[0]["FARE_AMT"]);
                    obj.REFRESH_AMT = Convert.ToString(dts.Rows[0]["REFRESH_AMT"]);
                    obj.TOTAL_AMT = Convert.ToString(dts.Rows[0]["TOTAL_AMT"]);
                    obj.REMARKS = Convert.ToString(dts.Rows[0]["REMARKS"]);
                    obj.STATUS = Convert.ToString(dts.Rows[0]["STATUS"]);
                    obj.CREATED_DATE = Convert.ToString(dts.Rows[0]["CREATED_DATE"]);
                    obj.MODIFIED_DATE = Convert.ToString(dts.Rows[0]["MODIFIED_DATE"]);
                    obj.RIMTODATE = Convert.ToString(dts.Rows[0]["RIMTODATE"]);

                    oview.Reimbursmentdetinfo = obj;
                    oview.status = "200";
                    oview.message = "Successfully add task.";

                    var message = Request.CreateResponse(HttpStatusCode.OK, oview);
                    return message;
                }

            }
            catch (Exception ex)
            {
                oview.status = "209";
                oview.message = ex.Message;
                var message = Request.CreateResponse(HttpStatusCode.OK, oview);
                return message;
            }

        }

        [HttpPost]

        public HttpResponseMessage getInfobyTimeSheetid(UserBMeetingInfoModelInput model)
        {

            UserTaskInfoModel oview = new UserTaskInfoModel();

            try
            {
                if (!ModelState.IsValid)
                {
                    oview.status = "213";
                    oview.message = "Some input parameters are missing.";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, oview);
                }
                else
                {
                    DataSet dt = new DataSet();
                    String con = Convert.ToString(APIConnction.ApiConnction); ;
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("PRC_EMPLOYEESELFSERVICE", sqlcon);
                    sqlcmd.Parameters.Add("@Action", "GET_MY_TSHEET_BY_ID");
                    sqlcmd.Parameters.Add("@USER_ID", model.UserId);
                    sqlcmd.Parameters.Add("@EMPCODE", model.EmpCode);
                    //sqlcmd.Parameters.Add("@user_id", model.user_id);
                    //sqlcmd.Parameters.Add("@TASK_DATE", model.date);
                    //sqlcmd.Parameters.Add("@TASK", model.task);
                    //sqlcmd.Parameters.Add("@DETAILS", model.details);
                    //sqlcmd.Parameters.Add("@isCompleted", model.isCompleted);
                    //sqlcmd.Parameters.Add("@Event_Id", model.eventID);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    DataTable dts = dt.Tables[0];

                    TaskInfoById obj = new TaskInfoById();
                    obj.Name = Convert.ToString(dts.Rows[0]["Name"]);
                    obj.deg_designation = Convert.ToString(dts.Rows[0]["deg_designation"]);
                    obj.Date_Of_Confirmation = Convert.ToString(dts.Rows[0]["Date_Of_Confirmation"]);
                    obj.emp_dateofJoining = Convert.ToString(dts.Rows[0]["emp_dateofJoining"]);

                    obj.empCode = Convert.ToString(dts.Rows[0]["empCode"]);
                    obj.phoneNo = Convert.ToString(dts.Rows[0]["phoneNo"]);
                    obj.reportingManager = Convert.ToString(dts.Rows[0]["reportingManager"]);
                    obj.dateOfBirth = Convert.ToString(dts.Rows[0]["dateOfBirth"]);
                    obj.profileImage = Convert.ToString(dts.Rows[0]["profileImage"]);
                    obj.AddressInfo = Convert.ToString(dts.Rows[0]["AddressInfo"]);
                    obj.PanNo = Convert.ToString(dts.Rows[0]["PanNo"]);
                    obj.emailId = Convert.ToString(dts.Rows[0]["emailId"]);
                    obj.brachName = Convert.ToString(dts.Rows[0]["brachName"]);
                    obj.fathersName = Convert.ToString(dts.Rows[0]["fathersName"]);

                    obj.TIMESHEET_ID = Convert.ToString(dts.Rows[0]["TIMESHEET_ID"]);
                    obj.TIMESHEET_USER_ID = Convert.ToString(dts.Rows[0]["TIMESHEET_USER_ID"]);
                    obj.TIMESHEET_PROJECT_ID = Convert.ToString(dts.Rows[0]["TIMESHEET_PROJECT_ID"]);
                    obj.TIMESHEET_DATE = Convert.ToString(dts.Rows[0]["TIMESHEET_DATE"]);
                    obj.TIMESHEET_COMMENT = Convert.ToString(dts.Rows[0]["TIMESHEET_COMMENT"]);
                    obj.TIMESHEET_ISAPPROVED = Convert.ToString(dts.Rows[0]["TIMESHEET_ISAPPROVED"]);
                    obj.TIMESHEET_HOUR = Convert.ToString(dts.Rows[0]["TIMESHEET_HOUR"]);
                    obj.TIMESHEET_MINUTE = Convert.ToString(dts.Rows[0]["TIMESHEET_MINUTE"]);

                    oview.Taskdetinfo = obj;
                    oview.status = "200";
                    oview.message = "Successfully add task.";

                    var message = Request.CreateResponse(HttpStatusCode.OK, oview);
                    return message;
                }

            }
            catch (Exception ex)
            {
                oview.status = "209";
                oview.message = ex.Message;
                var message = Request.CreateResponse(HttpStatusCode.OK, oview);
                return message;
            }

        }

        [HttpPost]

        public HttpResponseMessage getSettingsESS(ESSInputs model)
        {

            SettingEssOutput oview = new SettingEssOutput();

            try
            {
                if (!ModelState.IsValid)
                {
                    oview.status = "213";
                    oview.message = "Some input parameters are missing.";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, oview);
                }
                else
                {
                    DataSet dt = new DataSet();
                    String con = Convert.ToString(APIConnction.ApiConnction); ;
                    SqlCommand sqlcmd = new SqlCommand();
                    SqlConnection sqlcon = new SqlConnection(con);
                    sqlcon.Open();
                    sqlcmd = new SqlCommand("PRC_EditDeleteDays", sqlcon);
                    sqlcmd.Parameters.Add("@Action", "GET_EDIT_DELETE_DAYS");
                    //sqlcmd.Parameters.Add("@ID", model.ID);
                    //sqlcmd.Parameters.Add("@user_id", model.user_id);
                    //sqlcmd.Parameters.Add("@TASK_DATE", model.date);
                    //sqlcmd.Parameters.Add("@TASK", model.task);
                    //sqlcmd.Parameters.Add("@DETAILS", model.details);
                    //sqlcmd.Parameters.Add("@isCompleted", model.isCompleted);
                    //sqlcmd.Parameters.Add("@Event_Id", model.eventID);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                    da.Fill(dt);
                    sqlcon.Close();

                    DataTable dts = dt.Tables[0];

                    SettingEssInputs obj = new SettingEssInputs();
                    obj.ID = Convert.ToString(dts.Rows[0]["ID"]);
                    obj.delete_days = Convert.ToString(dts.Rows[0]["delete_days"]);
                    obj.edit_days = Convert.ToString(dts.Rows[0]["edit_days"]);
                    
                    oview.EssInfo = obj;
                    oview.status = "200";
                    oview.message = "Successfully add task.";

                    var message = Request.CreateResponse(HttpStatusCode.OK, oview);
                    return message;
                }

            }
            catch (Exception ex)
            {
                oview.status = "209";
                oview.message = ex.Message;
                var message = Request.CreateResponse(HttpStatusCode.OK, oview);
                return message;
            }

        }

        //end of rev Pratik
        
    }
}