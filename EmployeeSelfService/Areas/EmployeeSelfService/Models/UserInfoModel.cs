using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeSelfService.Areas.EmployeeSelfService.Models
{
    public class UserInfoModelInput
    {
        public string UserId { get; set; }
    }
    //rev Pratik
    public class UserBMeetingInfoModelInput
    {
        public string UserId { get; set; }
        public string EmpCode { get; set; }
    }
    //End of rev Pratik

    public class leaveTypeModel
    {
        public string EMPCODE { get; set; }
    }
    public class UserInfoModelOutpur
    {
        public string status { get; set; }
        public string message { get; set; }

        public userInformation userinfo { get; set; }
    }
    //rev Pratik
    public class UserLeaveInfoModel
    {
        public string status { get; set; }
        public string message { get; set; }

        //public userInformation userinfo { get; set; }
        public LeaveDetailsByIdClass leavedetinfo { get; set; }
    }
    public class LeaveDetailsByIdClass
    {
        public string ID { get; set; }
        public string USER_ID { get; set; }
        public string LEAVE_START_DATE { get; set; }
        public string LEAVE_END_DATE { get; set; }
        public string LEAVE_TYPE { get; set; }
        public string LEAVE_REASON { get; set; }
        public string CREATED_DATE { get; set; }
        public string CURRENT_STATUS { get; set; }
        public string SUPERVISOR_NAME { get; set; }
        public string SUPERVISOR_EMAIL { get; set; }
        public string SUPERVISOR_ID { get; set; }
        public string user_name { get; set; }

        public string ERPLeave_ID { get; set; }

        public string Name { get; set; }
        public string deg_designation { get; set; }
        public string Date_Of_Confirmation { get; set; }
        public string emp_dateofJoining { get; set; }

        public string empCode { get; set; }
        public string EMPCODE { get; set; }
        public string phoneNo { get; set; }
        public string reportingManager { get; set; }
        public string dateOfBirth { get; set; }
        public string profileImage { get; set; }
        public string AddressInfo { get; set; }
        public string PanNo { get; set; }
        public string emailId { get; set; }
        public string brachName { get; set; }
        public string fathersName { get; set; }

    }

    public class UserWfhInfoModel
    {
        public string status { get; set; }
        public string message { get; set; }
        public WFHInfoById WFHdetinfo { get; set; }
    }
    public class WFHInfoById
    {
        public string ID { get; set; }
        public string USER_ID { get; set; }
        public string WFH_STARTDATE { get; set; }
        public string WFH_ENDDATE { get; set; }
        public string WFH_REASON { get; set; }
        public string WORK_PLAN { get; set; }
        public string APPLIED_DATE { get; set; }
        public string CURRENT_STATUS { get; set; }
        public string SUPERVISOR_NAME { get; set; }
        public string SUPERVISOR_EMAIL { get; set; }
        public string SUPERVISOR_ID { get; set; }
        public string isDelete { get; set; }
        public string isEdit { get; set; }

        public string Name { get; set; }
        public string deg_designation { get; set; }
        public string Date_Of_Confirmation { get; set; }
        public string emp_dateofJoining { get; set; }

        public string empCode { get; set; }
        public string EMPCODE { get; set; }
        public string phoneNo { get; set; }
        public string reportingManager { get; set; }
        public string dateOfBirth { get; set; }
        public string profileImage { get; set; }
        public string AddressInfo { get; set; }
        public string PanNo { get; set; }
        public string emailId { get; set; }
        public string brachName { get; set; }
        public string fathersName { get; set; }

    }

    public class UserBMeetingInfoModel
    {
        public string status { get; set; }
        public string message { get; set; }
        public BMeetingInfoById BMeetingdetinfo { get; set; }
    }
    public class BMeetingInfoById
    {
        public string ID { get; set; }
        public string FROMDATE { get; set; }
        public string TODATE { get; set; }
        public string VISIT_IN { get; set; }
        public string CLIENT_DETAILS { get; set; }
        public string AGENDA { get; set; }
        public string SUPERVISOR_NAME { get; set; }
        public string SUPERVISOR_EMAIL { get; set; }
        public string SUPERVISOR_ID { get; set; }
        public string STATUS { get; set; }

        public string Name { get; set; }
        public string deg_designation { get; set; }
        public string Date_Of_Confirmation { get; set; }
        public string emp_dateofJoining { get; set; }

        public string empCode { get; set; }
        public string EMPCODE { get; set; }
        public string phoneNo { get; set; }
        public string reportingManager { get; set; }
        public string dateOfBirth { get; set; }
        public string profileImage { get; set; }
        public string AddressInfo { get; set; }
        public string PanNo { get; set; }
        public string emailId { get; set; }
        public string brachName { get; set; }
        public string fathersName { get; set; }

    }

    public class UserReimbursmentInfoModel
    {
        public string status { get; set; }
        public string message { get; set; }
        public ReimbursmentInfoById Reimbursmentdetinfo { get; set; }
    }
    public class ReimbursmentInfoById
    {
        public string ID { get; set; }
        public string RIMDATE { get; set; }
        public string RIMTYPE { get; set; }
        public string RIMCAT { get; set; }
        public string FARE_AMT { get; set; }
        public string REFRESH_AMT { get; set; }
        public string TOTAL_AMT { get; set; }
        public string REMARKS { get; set; }
        //public string EMPCODE { get; set; }
        public string STATUS { get; set; }
        public string CREATED_DATE { get; set; }
        public string MODIFIED_DATE { get; set; }
        public string RIMTODATE { get; set; }

        public string Name { get; set; }
        public string deg_designation { get; set; }
        public string Date_Of_Confirmation { get; set; }
        public string emp_dateofJoining { get; set; }

        public string empCode { get; set; }
        public string EMPCODE { get; set; }
        public string phoneNo { get; set; }
        public string reportingManager { get; set; }
        public string dateOfBirth { get; set; }
        public string profileImage { get; set; }
        public string AddressInfo { get; set; }
        public string PanNo { get; set; }
        public string emailId { get; set; }
        public string brachName { get; set; }
        public string fathersName { get; set; }

    }

    public class UserTaskInfoModel
    {
        public string status { get; set; }
        public string message { get; set; }
        public TaskInfoById Taskdetinfo { get; set; }
    }
    public class TaskInfoById
    {
        public string TIMESHEET_ID { get; set; }
        public string TIMESHEET_USER_ID { get; set; }
        public string TIMESHEET_PROJECT_ID { get; set; }
        public string TIMESHEET_DATE { get; set; }
        public string TIMESHEET_COMMENT { get; set; }
        public string TIMESHEET_ISAPPROVED { get; set; }
        public string TIMESHEET_HOUR { get; set; }
        public string TIMESHEET_MINUTE { get; set; }

        public string Name { get; set; }
        public string deg_designation { get; set; }
        public string Date_Of_Confirmation { get; set; }
        public string emp_dateofJoining { get; set; }
        public string empCode { get; set; }
        public string EMPCODE { get; set; }
        public string phoneNo { get; set; }
        public string reportingManager { get; set; }
        public string dateOfBirth { get; set; }
        public string profileImage { get; set; }
        public string AddressInfo { get; set; }
        public string PanNo { get; set; }
        public string emailId { get; set; }
        public string brachName { get; set; }
        public string fathersName { get; set; }

    }
    //end of rev Pratik
    public class userInformation
    {
        public string Name	 { get; set; }
        public string deg_designation	 { get; set; }
        public string Date_Of_Confirmation	 { get; set; }
        public string 	emp_dateofJoining { get; set; }
        public string empCode	{ get; set; }
        public string phoneNo	{ get; set; }
        public string reportingManager	{ get; set; }
        public string dateOfBirth	{ get; set; }
        public string profileImage	{ get; set; }
        public string AddressInfo	{ get; set; }
        public string PanNo	{ get; set; }
        public string emailId	{ get; set; }
        public string brachName	{ get; set; }
        public string fathersName { get; set; }
    }

    public class attModelInput
    {
        public string EMP_CODE { get; set; }
        public string YYMM { get; set; }
    }

    public class attModelOutput
    {
        public string status { get; set; }
        public string message { get; set; }

        public List<attReportData> attinfo { get; set; }
    }

    public class attReportData
    {
       public string EMP_CODE	{ get; set; }
       public string DATE	{ get; set; }
       public string IN_TIME	{ get; set; }       
       public string OUT_TIME	{ get; set; }
       public string DIFF	{ get; set; }
       public string TIMESHEET_HOUR	{ get; set; }
       public string SHOW_ACTION { get; set; }
    }

    public class bioOutput
    {
        public string status { get; set; }
        public string message { get; set; }

        public List<bioOutputData> attinfo { get; set; }
    }

    public class bioOutputData
    {
        public string user_name { get; set; }
        public string INTIME { get; set; }
        public string OUTTIME { get; set; }
        public string ID { get; set; }
        public string EMPCODE { get; set; }
        public string APPLIED_DATE { get; set; }
        public string ISAPPROVED { get; set; }
        public string BioReason { get; set; }
        public string APPLIED_FOR_DATE { get; set; }
    }

    public class PayslipPassInput
    {
        public string EmployeeCode { get; set; }
        public string YYMM { get; set; } 

    }
    public class PayslipPassOutput
    {
        public string status { get; set; }
        public string message { get; set; }

        public string Palslip_Password { get; set; }
    }
}