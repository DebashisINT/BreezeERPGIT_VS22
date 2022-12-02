using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BusinessLogicLayer.YearEnding
{
   public class StockCutoff
    {
        public string StockCutoffValueCutOff(DateTime CutOffDate)
        {
            DataTable CutoffValue = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_Yearending_Stock");
            proc.AddVarcharPara("@Action", 100, "CurrentYear");
            proc.AddVarcharPara("@CompanyID", 100, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@FinYear", 100, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@cutoffdate", 10, CutOffDate.ToString("yyyy-MM-dd"));
            // proc.AddIntegerPara("@userid", Convert.ToInt32(HttpContext.Current.Session["userid"]));
            CutoffValue = proc.GetTable();
            return "";
        }
    }
}
