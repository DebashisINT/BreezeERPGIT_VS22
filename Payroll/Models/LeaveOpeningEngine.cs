using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Payroll.Models
{
    public class LeaveOpeningEngine
    {
        string ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string StructureID { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ResponseCode { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ResponseMessage { get; set; }

        public List<LeaveDetailsList> LeaveDetailsList { get; set; }
        public List<dynamic> OpeningBalanceList { get; set; }
    }
    public class LeaveDetailsList
    {
        public string LeaveID { get; set; }
        public string LeaveName { get; set; }
    }
    public class LeaveHead
    {
        public List<Leave> MainArray { get; set; }
    }
    public class Leave
    {
        public List<classLeaveHead> classLeaveHead { get; set; }
    }
    public class classLeaveHead
    {
        public string Keys { get; set; }
        public string Amount { get; set; }
        public string Values { get; set; }
    }
}