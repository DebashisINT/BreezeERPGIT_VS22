using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeSelfService.Areas.EmployeeSelfService.Models
{

    public class LeaveInformationOutput
    {
        public string status { get; set; }
        public string message { get; set; }

        public List<LeaveInformationList> LeaveInformation { get; set; }
    }
    public class LeaveInformationList
    {
        public string EMPLOYEECODE { get; set; } 						
        public string LEAVESTRUCTURECODE { get; set; }
        public string LEAVEID { get; set; }
        public string LEAVENAME { get; set; }
        public string AVAILED { get; set; }
        public string BALANCE { get; set; }
        public string TotalLeave { get; set; }
    }
    public class LeaveApprovalOutput
    {
        public string status { get; set; }
        public string message { get; set; }

        public List<LeaveListClassForApp> leaveListforApp { get; set; }
    }

    public class LeaveListClassForApp 
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
    }
}