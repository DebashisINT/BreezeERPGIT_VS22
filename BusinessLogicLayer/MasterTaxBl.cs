using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace BusinessLogicLayer
{
   public class MasterTaxBl
    {
       public DataTable GetTaxxDetails( )
       {
           DataTable Dt;
           ProcedureExecute proc;
           try
           {

                
               using (proc = new ProcedureExecute("prc_Taxtype"))
               {
                 //  proc.RunActionQuery();
                   Dt = proc.GetTable();
               }
           }

           catch (Exception ex)
           {
               throw ex;
           }

          
           return Dt;
       }


    }
}
