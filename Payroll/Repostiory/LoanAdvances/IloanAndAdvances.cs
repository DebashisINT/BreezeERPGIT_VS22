using Payroll.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payroll.Repostiory.LoanAdvances
{
    public interface IloanAndAdvances
    {
         string SaveLoanAndAdveances(LoanAndAdvances model,string Action);
         DataTable GetLoanAndAdveances(string LoanId, string Action);

    }
}
