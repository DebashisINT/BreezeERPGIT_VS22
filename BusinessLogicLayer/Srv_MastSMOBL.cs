using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class Srv_MastSMOBL
    {
        public void Dispose()
        {

        }

        public DataTable GetEmployee()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_FetchMSO");
            proc.AddVarcharPara("@Action", 500, "EmployeeList");
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetBranch()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_FetchMSO");
            proc.AddVarcharPara("@Action", 500, "BRANCH");
            dt = proc.GetTable();
            return dt;
        }


        public DataTable GetMSO(String internal_id)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_FetchMSO");
            proc.AddVarcharPara("@Action", 500, "EmployeeCheck");
            proc.AddVarcharPara("@cnt_internalId", 100, internal_id);
            dt = proc.GetTable();
            return dt;
        }


        public string InsertMSOMaster(string contacttype, string cnt_ucc, string cnt_firstName, int cnt_branchId, string cnt_contactNO, bool Is_Active, string lastModifyUser, string cnt_contactType, string MOD, String EmploeeCode, String cnt_ContactPerson)
        {
            ProcedureExecute proc;
            string retval = "";
            try
            {
                using (proc = new ProcedureExecute("PRC_InsertUpdateMSO"))
                {
                    proc.AddVarcharPara("@Action", 500, "MSOInsertUpdate");
                    proc.AddVarcharPara("@contacttype", 50, contacttype);
                    proc.AddVarcharPara("@cnt_ucc", 80, cnt_ucc);
                    proc.AddVarcharPara("@cnt_firstName", 150, cnt_firstName);
                    proc.AddIntegerPara("@cnt_branchId", cnt_branchId);
                    proc.AddPara("@cnt_ContactPerson", cnt_ContactPerson);
                    proc.AddVarcharPara("@cnt_contactNO", 150, cnt_contactNO);
                    proc.AddBooleanPara("@Is_Active", Is_Active);
                    proc.AddVarcharPara("@lastModifyUser", 20, lastModifyUser);
                    proc.AddVarcharPara("@cnt_contactType", 5, cnt_contactType);
                    proc.AddVarcharPara("@Mod", 3, MOD);
                    proc.AddVarcharPara("@cnt_mainAccount", 100, EmploeeCode);
                    proc.AddVarcharPara("@result", 50, "", QueryParameterDirection.Output);
                    int i = proc.RunActionQuery();
                    retval = Convert.ToString(proc.GetParaValue("@result"));
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

        public int UpdateMSOMaster(string cnt_ucc, string cnt_firstName, int cnt_branchId, string cnt_contactNO, bool Is_Active, string lastModifyUser, string MOD, String EmploeeCode, int cnt_id, String cnt_ContactPerson)
        {
            ProcedureExecute proc;
            int retval = 0;
            try
            {
                using (proc = new ProcedureExecute("PRC_InsertUpdateMSO"))
                {
                    proc.AddVarcharPara("@Action", 500, "MSOInsertUpdate");
                    proc.AddVarcharPara("@cnt_ucc", 80, cnt_ucc);
                    proc.AddVarcharPara("@cnt_firstName", 150, cnt_firstName);
                    proc.AddIntegerPara("@cnt_branchId", cnt_branchId);
                    proc.AddPara("@cnt_ContactPerson", cnt_ContactPerson);
                    proc.AddVarcharPara("@cnt_contactNO", 150, cnt_contactNO);
                    proc.AddBooleanPara("@Is_Active", Is_Active);
                    proc.AddVarcharPara("@lastModifyUser", 20, lastModifyUser);
                    proc.AddVarcharPara("@Mod", 5, MOD);
                    proc.AddVarcharPara("@cnt_mainAccount", 100, EmploeeCode);
                    proc.AddIntegerPara("@cnt_id", cnt_id);
                    // retval = proc.RunActionQuery();
                    proc.AddVarcharPara("@result", 50, "", QueryParameterDirection.Output);
                    retval = proc.RunActionQuery();
                    //  retval = Convert.ToString(proc.GetParaValue("@result"));

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

        public int DeleteMSOMaster(String EmploeeCode)
        {
            ProcedureExecute proc;
            int retval = 0;
            try
            {
                using (proc = new ProcedureExecute("PRC_FetchMSO"))
                {
                    proc.AddVarcharPara("@Action", 500, "DeleteMSO");
                    proc.AddVarcharPara("@cnt_internalId", 100, EmploeeCode);
                    retval = proc.RunActionQuery();
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

        public int insertMSOBranchMap(string BranchIdList, string Tech_Internalid, int BranchId)
        {
            ProcedureExecute proc;
            try
            {
                using (proc = new ProcedureExecute("Prc_TechnicianBranchMap"))
                {
                    proc.AddVarcharPara("@Technician_InternalId", 50, Tech_Internalid);
                    proc.AddVarcharPara("@BranchIdList", 500, BranchIdList);
                    proc.AddIntegerPara("@BranchId", BranchId);

                    int i = proc.RunActionQuery();
                    return i;
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

        public int ImportEntitylMaster(string ComanyDbName)
        {
            ProcedureExecute proc;
            try
            {
                using (proc = new ProcedureExecute("PRC_FetchMSO"))
                {
                    proc.AddPara("@Action", "IMPORT");
                    proc.AddVarcharPara("@ComanyDbName", 500, ComanyDbName);
                    int i = proc.RunActionQuery();
                    return i;
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
    }
}
