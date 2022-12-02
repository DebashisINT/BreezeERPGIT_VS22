using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;

namespace Payroll.Models
{
    public class p_EmpAttactchmentEngine
    {
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ActionType { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string PayStructureCode { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Pay_ApplicationFrom { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Pay_ApplicationTo { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string LeaveStructureCode { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Leave_ApplicationFrom { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Leave_ApplicationTo { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string EmployeeList { get; set; }


        //public DataTable dt_EmployeeList { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ResponseCode { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ResponseMessage { get; set; }
    }
}