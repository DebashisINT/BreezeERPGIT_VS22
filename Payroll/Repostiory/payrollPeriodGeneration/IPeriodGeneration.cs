using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Payroll.Repostiory.payrollPeriodGeneration
{
    public interface IPeriodGeneration
    {
        string setActivePrevNext(string ActionType, string PayClassID, ref string ActiveYYMM, ref int ReturnCode);
    }
}