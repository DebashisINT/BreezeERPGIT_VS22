using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeSelfService.Areas.EmployeeSelfService.Models
{
    public class leaveApplyInputs
    {
        public string UserId { get; set; }
        public string LeaveType { get; set; }
        public string LeaveFrom { get; set; }
        public string LeaveTo { get; set; }
        public string SupervisorName { get; set; }
        public string SupervisorId { get; set; }
        public string SupervisorEmail { get; set; }
        public string LeaveReason { get; set; }
        public string EMPCODE { get; set; }

    }
    public class leaveApplyOutput
    {
        public string status { get; set; }

        public string message { get; set; }
    }



    public class WFHApplyInputs
    {
        public string USER_ID { get; set; }
        public string EMP_CODE { get; set; } 
        public string WFH_STARTDATE { get; set; }
        public string WFH_ENDDATE { get; set; }
        public string WFH_REASON { get; set; }
        public string WORK_PLAN { get; set; }
        public string SUPERVISOR_NAME { get; set; }
        public string SUPERVISOR_ID { get; set; }
        public string SUPERVISOR_EMAIL { get; set; }

    }

    public class InOutInputs
    { 
        public string UserId { get; set; }
        public string EmpId { get; set; }
    }

    public class timesheetInputs
    {
        public string USER_ID { get; set; }
        public string EMP_CODE { get; set; }
        public string TIMESHEET_PROJECT_ID { get; set; }
        public string TIMESHEET_DATE { get; set; }
        public string TIMESHEET_HOUR { get; set; }
        public string TIMESHEET_MINUTE { get; set; }
        public string TIMESHEET_COMMENT { get; set; }

    }
    public class timesheetOutput 
    {
        public string USER_ID { get; set; }
       

    }
    public class timesheetOutputList  
    {
        public string status { get; set; }
        public string message { get; set; } 
        public  List<timesheetTopLevelList> dt { get; set; }
    }
    public class timesheetTopLevelList
    {
        public string TIMESHEET_DATE { get; set; }
        public List<timesheetDownLevelList> info { get; set; }
    }
    public class timesheetDownLevelList
    {
        public string TIMESHEET_ID	{ get; set; }
        public string TIMESHEET_DATE	{ get; set; }
        public string TIMESHEET_COMMENT	{ get; set; }
        public string TIMESHEET_HOUR{ get; set; }	
        public string TIMESHEET_MINUTE { get; set; }
        public string isDelete { get; set; }
        public string isEdit { get; set; } 
    }

    public class biometricApplyInputs
    {
        public string EMP_CODE { get; set; }
        public string REQ_INTIME { get; set; }
        public string REQ_OUTTIME { get; set; }
        public string APPLIED_FOR_DATE { get; set; }
        public string BioReason { get; set; }

    }
    public class appBiometricReqClass
    {
        public string BIOMETRIC_IDS { get; set; }
    }
    public class appLeaveReqClass
    {
        public string LEAVE_IDS { get; set; }
    }
    /* for Reimbursement approval  */
    public class appRMReqClass
    {
        public string RM_IDS { get; set; } 
    }
    /* Business meeting section */
    public class BMInputs  
    {
        public string ID { get; set; }
        public string EMP_CODE { get; set; }
        public string FROMDATE { get; set; }
        public string TODATE { get; set; }
        public string VISITIN { get; set; }
        public string CLIENT_DETAILS { get; set; }
        public string AGENDA { get; set; }
        public string ACTION { get; set; }
        public string STATUS { get; set; }
        public string MM { get; set; }
        public string YYYY { get; set; } 
    }
    public class BMApplyOutput 
    {
        public string status { get; set; }

        public string message { get; set; }
    }
    public class BMOutput
    {			
        public string ID { get; set; }
        public string FROMDATE { get; set; }
        public string TODATE { get; set; }
        public string VISITIN { get; set; }
        public string CLIENT_DETAILS { get; set; }
        public string AGENDA { get; set; }
        public string STATUS { get; set; }
        public string CREATED_DATE { get; set; }
        public string MODIFIED_DATE { get; set; }
        public string EMPCODE { get; set; }
        public string USERNAME { get; set; } 
        //rev Pratik
        public string isDelete { get; set; }
        public string isEdit { get; set; } 
        //End of rev Pratik

    }
    public class appBMReqClass
    { 
        public string BM_IDS { get; set; }
    }

    /* Reimbursement section */
    public class RMInputs
    {
        public string ID { get; set; }
        public string EMP_CODE { get; set; }
        public string RIMDATE { get; set; }
        public string RIMTODATE { get; set; }
        public string RIMTYPE { get; set; }
        public string RIMCAT { get; set; }
        public string FARE_AMT { get; set; }

        public string REFRESH_AMT { get; set; }
        public string TOTAL_AMT { get; set; }
        public string REMARKS { get; set; }
        public string ACTION { get; set; }
        public string STATUS { get; set; }
        public string MM { get; set; }
        public string YYYY { get; set; }
    }
    public class RMApplyOutput
    {
        public string status { get; set; }

        public string message { get; set; }

        public List<RMOutput> Rminfo { get; set; }
    }									
    public class RMOutput  
    {
        public string ID { get; set; }
        public string RIMDATE { get; set; }
        public string RIMTYPE { get; set; }
        public string RIMCAT { get; set; }
        public string FARE_AMT { get; set; }
        public string REFRESH_AMT { get; set; }
        public string TOTAL_AMT { get; set; }
        public string REMARKS { get; set; }
        public string EMPCODE { get; set; }
        public string STATUS { get; set; }
        public string CREATED_DATE { get; set; }
        public string MODIFIED_DATE { get; set; }
        public string RIMTODATE { get; set; } 
        public string username { get; set; }
        //rev Pratik
        public string isDelete { get; set; }
        public string isEdit { get; set; }

        //End of Rev Pratik
    }
    //rev Pratik
    public class DeleteReq
    {
        public string ID { get; set; }
        public string Action { get; set; }
    }
    //End of rev Pratik
    public class RMCATOUTPUT  
    {
        public string ID { get; set; }
        public string CATNAME { get; set; }
    }
    public class RMTYPEOUTPUT
    { 
        public string ID { get; set; }
        public string TYPENAME { get; set; }
    }


    /* User */
    public class userOutput
    {
        public string user_id { get; set; }
        public string user_contactId { get; set; }
        public string user_name { get; set; } 

    }

    public class AttchartOutput
    {
        public string YYMM { get; set; }
        public string Emp_status { get; set; }
        public string NoOfPresent { get; set; }
    }
}

