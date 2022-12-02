using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Payroll.Models;
using System.Data;

namespace Payroll.Repostiory.P_Formula
{
    public interface IPayrole_formula
    {
        void save(FormulaApply apply, ref string tblformulaid, ref int ReturnCode, ref string ReturnMsg);
        FormulaApply getFormulaDetailsById(string _formulaCode, string EditFlag, int TableBreakup_ID);
           

        string Delete(string ActionType, string id, ref int ReturnCode);
    }
}