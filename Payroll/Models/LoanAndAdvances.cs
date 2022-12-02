using Payroll.Models.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Payroll.Models
{
    public class LoanAndAdvances
    {
        public string Action { get; set; }
        public string LoanId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Emp_Code { get; set; }
        public List<v_proll_Dflt_Salary> Emp_List { get; set; }
        public string Period { get; set; }
        public List<v_proll_PeriodGenerationList> Period_List { get; set; }

        public string TYPE { get; set; }
        public string Amount { get; set; }
        public string Installment { get; set; }
        public string ins_Amount { get; set; }
        public string Max_Amount { get; set; }
        public string Max_Based_On { get; set; }
        public string Max_Check { get; set; }
        public string Min_Amount { get; set; }
        public string Min_Check { get; set; }
        public string Disb_Date { get; set; }
        public string Deduction_Start { get; set; }
        public string Deduction_Starts_Period { get; set; }
        public string IsFreeze { get; set; }
        public string Freeze_Upto { get; set; }



    }
}