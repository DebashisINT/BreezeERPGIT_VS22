
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Payroll.Models
{
    public class LeaveRegisterViewModel
    {
        public Int64 USERID { get; set; }

        public String REPORTTYPE { get; set; }

        public Int64 SLNO { get; set; }

        public String EMPLOYEECODE { get; set; }

        public String STRUCTURECODE { get; set; }

        public String EMPLOYEENAME { get; set; }

        public String LEAVENAME { get; set; }

        public String LEV_DATE_FROM { get; set; }

        public String LEV_DATE_TO { get; set; }

        public Int64 LEAVEDAYS { get; set; }

        public DateTime LEAVEAPPLIEDON { get; set; }

        public String STATUS { get; set; }

        public String USER_NAME { get; set; }

        public String ApplicationNo { get; set; }

    
        public String ApplicationDetails { get; set; }

        public String ReasonForLeave { get; set; }

        public String LeaveID { get; set; }

        public String Day_Part { get; set; }

        public String Response { get; set; }

        public String DocID { get; set; }


        public Int64 Balance { get; set; }

      

    }

  

}