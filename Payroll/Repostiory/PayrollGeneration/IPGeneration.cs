using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Payroll.Repostiory.PayrollGeneration
{
    public interface IPGeneration
    {
        string PGenerate( string ClassId, ref int ReturnCode);
        string PGenerateEmployeeWise(string ClassId, string emp, ref int ReturnCode);
        void UndoSalaryGeneration(DataTable EmployeeCode, String ClassCode, String yymm, ref int ReturnCode, ref string ReturnMessage);
    }
}