using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class UdfGroupMasterBL
    {
        public Boolean InsertGroupMaster(string grp_description, string grp_applicablefor, bool grp_isVisible)
        {
             ProcedureExecute proc;
             bool retval = true;
            try
            {
                using (proc = new ProcedureExecute("prc_udfGroup"))
                {
                    proc.AddVarcharPara("@grp_description", 100, grp_description);
                    proc.AddVarcharPara("@grp_applicablefor", 100, grp_applicablefor);
                    proc.AddBooleanPara("@grp_isVisible", grp_isVisible);
                    proc.AddVarcharPara("@action", 5, "I");

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


        public Boolean UpdateGroupMaster(string grp_description, string grp_applicablefor, bool grp_isVisible,int id)
        {
            ProcedureExecute proc;
            bool retval = true;
            try
            {
                using (proc = new ProcedureExecute("prc_udfGroup"))
                {
                    proc.AddVarcharPara("@grp_description", 100, grp_description);
                    proc.AddVarcharPara("@grp_applicablefor", 100, grp_applicablefor);
                    proc.AddBooleanPara("@grp_isVisible", grp_isVisible);
                    proc.AddIntegerPara("@id", id);
                    proc.AddVarcharPara("@action", 5, "U");
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

    }
}
