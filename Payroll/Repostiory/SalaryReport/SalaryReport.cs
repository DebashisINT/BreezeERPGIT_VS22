using DataAccessLayer;
using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Payroll.Repostiory.SalaryReport
{
    public class SalaryReport:ISalaryReport
    {
        public DataTable PopulateSalaryReport(string StructureCode, string YYMM, ref string ReturnMsg)
        {
            DataTable _SalaryReport = new DataTable();
            try
            {
                ExecProcedure execProc = new ExecProcedure();
                List<KeyObj> paramList = new List<KeyObj>();
                execProc.ProcedureName = "proll_SalaryReport_Details";
                paramList.Add(new KeyObj("@Action", "SalaryReport"));
                paramList.Add(new KeyObj("@StructureID", StructureCode));
                paramList.Add(new KeyObj("@YYMM", YYMM));
                execProc.param = paramList;
                _SalaryReport = execProc.ExecuteProcedureGetTable();
                paramList.Clear();
            }
            catch (Exception ex)
            {
            }
            return _SalaryReport;
        }
    }
}