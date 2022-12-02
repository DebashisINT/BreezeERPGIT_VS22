using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Payroll.Repostiory.SalaryReport
{
    public interface ISalaryReport
    {
        DataTable PopulateSalaryReport(string StructureCode, string YYMM, ref string ReturnMsg);
        //DataTable PopulateSalaryReport(string StructureCode, string YYMM, ref string ReturnMsg);
    }
}