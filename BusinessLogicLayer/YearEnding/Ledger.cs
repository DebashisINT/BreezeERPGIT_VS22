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
   public class Ledger
    {
       public string BranchWiseLedgerBalance(DateTime CutOffDate, int CloseStock)
       {
           DataTable CutoffValue = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("Prc_Yearending_MainAccount");
           proc.AddVarcharPara("@CutOffDATE", 10, CutOffDate.ToString("yyyy-MM-dd"));
           proc.AddVarcharPara("@COMPANYID", 100, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
           proc.AddVarcharPara("@FINYEAR", 100, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
           proc.AddIntegerPara("@CHKCLOSESTK", CloseStock);
           proc.AddIntegerPara("@USERID", Convert.ToInt32(HttpContext.Current.Session["userid"]));
           
          
           CutoffValue = proc.GetTable();
           return "";
       }

       public string BranchWiseSubLedgerBalance(DateTime CutOffDate)
       {
           DataTable CutoffValue = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("PRC_BRANCHWISESUBLEDGERWISECLOSEBAL_YE");
           //proc.AddVarcharPara("@Action", 100, "CurrentYear");
           proc.AddVarcharPara("@COMPANYID", 100, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
           proc.AddVarcharPara("@FINYEAR", 100, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
           proc.AddVarcharPara("@ASONDATE", 10, CutOffDate.ToString("yyyy-MM-dd"));
           // proc.AddIntegerPara("@userid", Convert.ToInt32(HttpContext.Current.Session["userid"]));
           CutoffValue = proc.GetTable();
           return "";
       }

    }
}
