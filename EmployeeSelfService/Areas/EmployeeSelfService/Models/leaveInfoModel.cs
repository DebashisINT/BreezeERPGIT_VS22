using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeSelfService.Areas.EmployeeSelfService.Models
{
    public class LeaveInfoModelInput
    {
        public string UserId { get; set; }
        public string EMPCODE { get; set; } 
        
    }
    public class fetchTimesheetInput
    {
        public string UserId { get; set; }
        public string mm { get; set; }
        public string yyyy { get; set; } 
    }
    public class LeaveInfoModelOutpur
    {
        public string status { get; set; }
        public string message { get; set; }

        public leaveInformation leaveinfo { get; set; }
    }

    public class inputEmailClass
    {
        public string ToEmail { get; set; }
        public string CCEmail { get; set; }

        public string BCCEmail { get; set; }
        public string Subject { get; set; }
        public string EmailBody { get; set; }

    }
    public class leaveInformation
    {
        public string AvlPL { get; set; }
        public string  AvlCL	{ get; set; }
        public string  ConsumedCL	{ get; set; }
        public string  ConsumedPL { get; set; }

    }


    public class leaveApplyModel
    {
        public string UserId { get; set; }
        public string LEAVE_START_DATE { get; set; }
        public string LEAVE_END_DATE { get; set; }
        public string LEAVE_TYPE { get; set; }
        public string LEAVE_REASON { get; set; }
        public string CREATED_DATE { get; set; }

    }
    public class ApplyOut
    {
        public string status { get; set; }
        public string message { get; set; }

    }

    public class leaveType
    {
        public string LeaveID { get; set; }
        public string LeaveName { get; set; }

    }
    public class onLeaveTodayOut
    {
        public string status { get; set; }
        public string message { get; set; }

        public List<onLeaveToday> leaveList { get; set; }
    }
    public class onLeaveToday
    {	
        public string user_name { get; set; }
        public string USER_ID { get; set; }

    } 
    public class superVisors
    {
        public string userId { get; set; }
        public string spName { get; set; }
        public string spId { get; set; }
        public string spEmail { get; set; }
        

    }
    					
    public class HolidayList
    {
        public string HOLIDAYID { get; set; }
        public string HOLIDAY_NAME { get; set; }
        public string STARTDAY { get; set; }
        public string ENDDAY { get; set; } 


    }

    public class EMP_ATTSTATUSClass
    {
        public string ID { get; set; }
        public string EMP_CODE { get; set; }
        public string ATT_DATE { get; set; }
        public string IS_IN { get; set; }
        public string IS_OUT { get; set; }

    }
    public class LeaveListOutput
    { 
        public string status { get; set; }
        public string message { get; set; }
         
        public List<LeaveListClass> leaveList { get; set; }
    }
    public class LeaveListClass
    {
        public string ID	{ get; set; }
        public string USER_ID	{ get; set; }
        public string LEAVE_START_DATE	{ get; set; }
        public string LEAVE_END_DATE	{ get; set; }
        public string LEAVE_TYPE	{ get; set; }
        public string LEAVE_REASON	{ get; set; }
        public string CREATED_DATE	{ get; set; }
        public string CURRENT_STATUS	{ get; set; }
        public string SUPERVISOR_NAME	{ get; set; }
        public string SUPERVISOR_EMAIL { get; set; }
        public string SUPERVISOR_ID { get; set; }
        public string LeaveType { get; set; }

        //rev Pratik
        public string isDelete { get; set; }
        public string isEdit { get; set; }
        //End of rev Pratik
    }

    public class WFHListClass 
    { 
     public string ID	{ get; set; }
     public string USER_ID	{ get; set; }
     public string WFH_STARTDATE	{ get; set; }
     public string WFH_ENDDATE	{ get; set; }
     public string WFH_REASON	{ get; set; }
     public string WORK_PLAN	{ get; set; }
     public string APPLIED_DATE	{ get; set; }
     public string CURRENT_STATUS{ get; set; }	
     public string SUPERVISOR_NAME	{ get; set; }
     public string SUPERVISOR_EMAIL{ get; set; }	
     public string SUPERVISOR_ID { get; set; }

     //rev Pratik
     public string isDelete { get; set; }
     public string isEdit { get; set; }

     //end of rev Pratik
    
    }





}




	