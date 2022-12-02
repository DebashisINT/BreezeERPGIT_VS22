using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
  public class FinancerExecutiveBL
    {

      public Boolean InsertFinancerExecutive(string internalId, string executiveName)
      {
          ProcedureExecute proc;
          bool retval = true;
          try
          {
              using (proc = new ProcedureExecute("instFinExecutive"))
              {
                  proc.AddVarcharPara("@internalid", 10, internalId);
                  proc.AddVarcharPara("@executiveName", 5000, executiveName);
                   

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

      public string InsertFinancerMaster(string contacttype, string cnt_ucc, string cnt_firstName, int cnt_branchId, string cnt_VerifcationRemarks, bool Is_Active, string lastModifyUser, string cnt_contactType, string MOD)
      {
          ProcedureExecute proc;
          string retval = "";
          try
          {
              using (proc = new ProcedureExecute("ContactInsert_Driver"))
              {
                  proc.AddVarcharPara("@contacttype", 50, contacttype);
                  proc.AddVarcharPara("@cnt_ucc", 80, cnt_ucc);
                  proc.AddVarcharPara("@cnt_firstName", 150, cnt_firstName);
                  proc.AddIntegerPara("@cnt_branchId", cnt_branchId);
                  proc.AddVarcharPara("@cnt_VerifcationRemarks", 150, cnt_VerifcationRemarks);
                  proc.AddBooleanPara("@Is_Active", Is_Active);
                  proc.AddVarcharPara("@lastModifyUser", 20, lastModifyUser);
                  proc.AddVarcharPara("@cnt_contactType", 5, cnt_contactType);
                  proc.AddVarcharPara("@Mod", 3, MOD);
                  proc.AddVarcharPara("@result", 50, "", QueryParameterDirection.Output);
                  int i = proc.RunActionQuery();
                  retval =Convert.ToString( proc.GetParaValue("@result"));
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
