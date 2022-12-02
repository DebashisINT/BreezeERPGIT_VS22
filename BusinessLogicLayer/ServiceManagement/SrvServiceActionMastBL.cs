using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.ServiceManagement
{
    public class SrvServiceActionMastBL : IDisposable
    {
        public void Dispose()
        {

        }

        public DataTable GetServiceActionList()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PROC_InsertUpdateMasterServiceAction");
            proc.AddVarcharPara("@ACTION", 500, "SELECT");
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetServiceActionEdit(String SrvActionID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PROC_InsertUpdateMasterServiceAction");
            proc.AddVarcharPara("@ACTION", 500, "EDIT");
            proc.AddVarcharPara("@SrvActionID", 80, SrvActionID);
            dt = proc.GetTable();
            return dt;
        }

        public int UpdateServiceActionMaster(string SrvActionDesc, string USER, string SrvActionID)
        {
            ProcedureExecute proc;
            int retval = 0;
            try
            {
                using (proc = new ProcedureExecute("PROC_InsertUpdateMasterServiceAction"))
                {
                    proc.AddVarcharPara("@Action", 500, "UPDATE");
                    proc.AddVarcharPara("@SrvActionDesc", 500, SrvActionDesc);
                    proc.AddVarcharPara("@USER", 150, USER);
                    proc.AddVarcharPara("@SrvActionID", 150, SrvActionID);
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

        public int InsertServiceActionMaster(string SrvActionDesc, string USER)
        {
            ProcedureExecute proc;
            int retval = 0;
            try
            {
                using (proc = new ProcedureExecute("PROC_InsertUpdateMasterServiceAction"))
                {
                    proc.AddVarcharPara("@Action", 500, "INSERT");
                    proc.AddVarcharPara("@SrvActionDesc", 500, SrvActionDesc);
                    proc.AddVarcharPara("@USER", 150, USER);
                    retval = proc.RunActionQuery();
                }
            }
            catch (Exception ex)
            { }
            finally
            { }
            return retval;
        }

        public DataTable GetServiceActionChecking(String SrvActionDesc)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PROC_InsertUpdateMasterServiceAction");
            proc.AddVarcharPara("@ACTION", 500, "CHECKING");
            proc.AddVarcharPara("@SrvActionDesc", 500, SrvActionDesc);
            dt = proc.GetTable();
            return dt;
        }

        public int DeletePServiceActionMaster(string SrvActionID)
        {
            ProcedureExecute proc;
            int retval = 0;
            try
            {
                using (proc = new ProcedureExecute("PROC_InsertUpdateMasterServiceAction"))
                {
                    proc.AddVarcharPara("@Action", 500, "DELETE");
                    proc.AddVarcharPara("@SrvActionID", 150, SrvActionID);
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
    }
}
