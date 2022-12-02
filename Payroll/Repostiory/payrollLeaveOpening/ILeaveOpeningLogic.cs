using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Payroll.Repostiory.payrollLeaveOpening
{
    public interface ILeaveOpeningLogic
    {
        DataSet GetEmployeeLeaveOpening(string strStructureID);
        void SaveOpeningBalance(DataTable dt, ref int strIsComplete, ref string strMessage);
    }
}