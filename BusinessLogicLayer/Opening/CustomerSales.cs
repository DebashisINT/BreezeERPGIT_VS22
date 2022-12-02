using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using DataAccessLayer;

namespace BusinessLogicLayer
{
   public class CustomerSales
    {
       
       public DataTable GetCustomersalesFinancialyear(string FinYearcode)
       {
           DataTable ds = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("Opening_CustomerSales_GetDateFinbancialYear");
           proc.AddPara("@FinYearcode",  FinYearcode);
           proc.AddPara("@Mode", 1);
           ds = proc.GetTable();
           return ds;
       }

       public DataTable GetCustomersalesFinancialyearCode(DateTime FinYearDate)
       {
           DataTable ds = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("Opening_CustomerSales_GetDateFinbancialYear");
           proc.AddPara("@FinDatecode", FinYearDate);
           proc.AddPara("@Mode", 2);
           ds = proc.GetTable();
           return ds;
       }


    }
}
