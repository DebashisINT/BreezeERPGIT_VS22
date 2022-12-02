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
   public class YearEndingPurchase
    {
        public string PurchaseCutOff(DateTime CutOffDate)
        {
            DataTable CutoffPurchase = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_YEARENDING_VENDOR");
            proc.AddVarcharPara("@Action", 100, "CurrentYear");
            proc.AddVarcharPara("@CompanyId", 100, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@FinYear", 100, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@CutOffdate", 10, CutOffDate.ToString("yyyy-MM-dd"));
            proc.AddIntegerPara("@userid", Convert.ToInt32(HttpContext.Current.Session["userid"]));
            CutoffPurchase = proc.GetTable();
            return "";
        }
    }
}
