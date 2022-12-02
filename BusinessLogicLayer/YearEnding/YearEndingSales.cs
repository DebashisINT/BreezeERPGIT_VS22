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
    public  class YearEndingSales
    {

        public string SalesCutOff(DateTime CutOffDate)
        {
            DataTable CutoffValue = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_YEARENDING_CUSTOMER");
            proc.AddVarcharPara("@Action", 100, "CurrentYear");
            proc.AddVarcharPara("@CompanyId", 100, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@FinYear", 100, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@CutOffdate",10, CutOffDate.ToString("yyyy-MM-dd"));
            proc.AddIntegerPara("@userid", Convert.ToInt32(HttpContext.Current.Session["userid"]));
            CutoffValue = proc.GetTable();
            return "";
        }
    }
}
