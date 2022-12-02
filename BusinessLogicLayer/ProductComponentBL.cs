using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
   public class ProductComponentBL
    {
       public Boolean SetProductComponent(string internalId, string executiveName)
       {
           ProcedureExecute proc;
           bool retval = true;
           try
           {
               using (proc = new ProcedureExecute("InsProductComponent"))
               {
                   proc.AddVarcharPara("@prodId", 10, internalId);
                   proc.AddVarcharPara("@ProdComponent", 1000, executiveName);


                   int i = proc.RunActionQuery();

               }
           }
           catch (Exception ex)
           {
               retval = false;
           }
           finally
           {
               proc = null;
           }
           return retval;
       }
       public int InsertHSN(string HSNCode, string HSNDescription, string Action)
       {
           ProcedureExecute proc;
           int retValue = 0;
           try
           {
               using (proc = new ProcedureExecute("InsertHSN_SAC"))
               {
                   proc.AddNVarcharPara("@Action", 50, Action);
                   proc.AddNVarcharPara("@HSNCode", 50, HSNCode);
                   proc.AddNVarcharPara("@HSNDescription", 4000, HSNDescription);
                   proc.AddVarcharPara("@ReturnValue", 50, "", QueryParameterDirection.Output);
                   int i = proc.RunActionQuery();
                   retValue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
                   return retValue;
               }
           }
           catch (Exception ex)
           {
               retValue = 0;
           }
           finally
           {
               proc = null;
           }
           return retValue;
       }

       public int InsertSAC(string Service_Category_Code, string Service_tax_Name, string Group_ID, string Group_Name, string Action)
       {
           ProcedureExecute proc;
           int retValue = 0;
           try
           {
               using (proc = new ProcedureExecute("InsertHSN_SAC"))
               {
                   proc.AddNVarcharPara("@Action", 50, Action);
                   proc.AddNVarcharPara("@Service_Category_Code", 50, Service_Category_Code);
                   proc.AddNVarcharPara("@Service_tax_Name", 4000, Service_tax_Name);
                   proc.AddNVarcharPara("@Group_ID", 50, Group_ID);
                   proc.AddNVarcharPara("@Group_Name", 100, Group_Name);
                   proc.AddVarcharPara("@ReturnValue", 50, "", QueryParameterDirection.Output);
                   int i = proc.RunActionQuery();

                   retValue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
                   return retValue;
               }
           }
           catch (Exception ex)
           {
               retValue = 0;
           }
           finally
           {
               proc = null;
           }
           return retValue;
       }


       public int UpdateHSN(string HSNCode, string HSNDescription, string Action)
       {
           ProcedureExecute proc;
           int retValue = 0;
           try
           {
               using (proc = new ProcedureExecute("InsertHSN_SAC"))
               {
                   proc.AddNVarcharPara("@Action", 50, Action);
                   proc.AddNVarcharPara("@HSNCode", 50, HSNCode);
                   proc.AddNVarcharPara("@HSNDescription", 4000, HSNDescription);
                   proc.AddVarcharPara("@ReturnValue", 50, "", QueryParameterDirection.Output);
                   int i = proc.RunActionQuery();
                   retValue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
                   return retValue;
               }
           }
           catch (Exception ex)
           {
               retValue = 0;
           }
           finally
           {
               proc = null;
           }
           return retValue;
       }

       public int DeleteHSN(string HSNCode, string Action)
       {
           ProcedureExecute proc;
           int retValue = 0;
           try
           {
               using (proc = new ProcedureExecute("InsertHSN_SAC"))
               {
                   proc.AddNVarcharPara("@Action", 50, Action);
                   proc.AddNVarcharPara("@HSNCode", 50, HSNCode);                  
                   proc.AddVarcharPara("@ReturnValue", 50, "", QueryParameterDirection.Output);
                   int i = proc.RunActionQuery();
                   retValue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
                   return retValue;
               }
           }
           catch (Exception ex)
           {
               retValue = 0;
           }
           finally
           {
               proc = null;
           }
           return retValue;
       }
    }
}
