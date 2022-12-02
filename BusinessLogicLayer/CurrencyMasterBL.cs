using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
   public class CurrencyMasterBL
    {

       public int InsertCurrencyMaster( string cmp_intrnlid, int basecurr_id, int lastModifyUser, string MOD)
       {
           ProcedureExecute proc;
           int retval = 0;
           try
           {
               using (proc = new ProcedureExecute("sp_CurrencyMaster"))
               {
                   proc.AddVarcharPara("@cmp_internalid", 50, cmp_intrnlid);
                   proc.AddIntegerPara("@BaseCurrency_ID", basecurr_id);
                   proc.AddIntegerPara("@CreatedBY", lastModifyUser);
                   proc.AddVarcharPara("@Mod", 3, MOD);
                   proc.AddVarcharPara("@result", 50, "", QueryParameterDirection.Output);
                   int i = proc.RunActionQuery();
                   retval = Convert.ToInt32(proc.GetParaValue("@result"));
               }
           }
           catch (Exception ex)
           {

           }
           finally
           {

           }
           return retval;
       }
       public int InsertCurrencyRateDateWise(string cmp_intrnlid, int basecurr_id, int lastModifyUser, string MOD, string basecurr,int CRID)
       {
           ProcedureExecute proc;
           int retval = 0;
           try
           {
               using (proc = new ProcedureExecute("sp_CurrencyMasterDateWise"))
               {
                   proc.AddVarcharPara("@cmp_internalid", 50, cmp_intrnlid);
                   proc.AddIntegerPara("@BaseCurrency_ID", basecurr_id);
                   proc.AddIntegerPara("@CreatedBY", lastModifyUser);
                   proc.AddVarcharPara("@Mod", 3, MOD);
                   proc.AddVarcharPara("@basecurr", 2000, basecurr);
                   proc.AddIntegerPara("@CRID", CRID);
                   proc.AddVarcharPara("@result", 50, "", QueryParameterDirection.Output);
                   int i = proc.RunActionQuery();
                   retval = Convert.ToInt32(proc.GetParaValue("@result"));
               }
           }
           catch (Exception ex)
           {

           }
           finally
           {

           }
           return retval;
       }
    }
}
