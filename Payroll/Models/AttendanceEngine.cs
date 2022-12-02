using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Payroll.Models
{
    public class AttendanceEngine
    {
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ResponseCode { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ResponseMessage { get; set; }
        public List<dynamic> EmployeeAttendanceList { get; set; }
        public List<EmployeeAttendance> AttendanceDetails { get; set; }
    }

    public class AttendanceApprovalEngine
    {
        public string Date { get; set; }
        public string Status { get; set; }
        public string curStatus { get; set; }

    }

    public class EmployeeAttendance
    {
        public string EmployeeID { get; set; }
        public string AttendanceDate { get; set; }
        public string InTime { get; set; }
        public string OutTime { get; set; }
        public string Status { get; set; }
    }

    public class ImportAttendance
    {
        public string ReturnValue { get; set; }
        public string ReturnMessage { get; set; }
        public int HasLog { get; set; }
    }
    public class approvaldata
    {
        public string Date { get; set; }
        public string newStatus { get; set; }
        

    }

    public class Approval
    {
        public List<approvaldata> data { get; set; }
        public string EmployeeId { get; set; }
        public string yymm { get; set; }
    }

}