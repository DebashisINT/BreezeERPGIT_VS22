using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessLayer;
using System.Data;

namespace BusinessLogicLayer
{
    public class AccountGroup_BL
    {
        public int AccountGroupUpdating(string AccountGroupID, string AccountGroupType, string AccountGroupCode, string AccountGroupName, string AccountGroupParentID, string CreateUser)
        {
            ProcedureExecute proc;

            try
            {
                using (proc = new ProcedureExecute("UpdateAccountGroup"))
                {
                    // .............................Code Commented and Added by Sam on 06122016 to use Convert.tostring instead of tostring(). ................

                    proc.AddIntegerPara("@AccountGroupID", Convert.ToInt32(AccountGroupID));
                    proc.AddVarcharPara("@AccountGroupType", 50, AccountGroupType);
                    proc.AddVarcharPara("@AccountGroupCode", 80, AccountGroupCode);
                    proc.AddVarcharPara("@AccountGroupName", 100, AccountGroupName);

                    // .............................Code Above Commented and Added by Sam on 06122016 to use Convert.tostring instead of tostring(). ..................................... 
                   
                    //  proc.AddVarcharPara.Add("@AccountGroupName", SqlDbType.VarChar).Value = e.NewValues["AccountGroupName"].ToString();
                    if (AccountGroupParentID == null)
                    {
                        proc.AddIntegerPara("@AccountGroupParentID", 0);
                    }
                    else
                    { proc.AddIntegerPara("@AccountGroupParentID", Convert.ToInt32(AccountGroupParentID)); }

                    proc.AddIntegerPara("@CreateUser", Convert.ToInt32(CreateUser));

                    //if (e.NewValues["AccountGroupParentID"] == null)
                    //    proc.Parameters.Add("@AccountGroupParentID", SqlDbType.Int).Value = 0;
                    //else
                    //    proc.Parameters.Add("@AccountGroupParentID", SqlDbType.Int).Value = Convert.ToInt32(e.NewValues["AccountGroupParentID"].ToString());

                    //   proc.Parameters.Add("@CreateUser", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["userid"]);

                    int NoOfRowEffected = proc.RunActionQuery();

                    return NoOfRowEffected;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                proc = null;
            }
        }

        public Int64 GetAccountGroupParentID(string AccountGroup_Name)
        {
            ProcedureExecute proc;
            object result = -1;
            try
            {
                
                using (proc = new ProcedureExecute("getAccountGroupParentID"))
                {
                    proc.AddVarcharPara("@AccountGroup_Name", 24, AccountGroup_Name);
                    result = proc.GetScalar();
                }

                if (result == null)
                {
                    return -1;
                }
                else
                {
                    Convert.ToInt64(result);
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
            return Convert.ToInt64(result);
        }

    }
}
