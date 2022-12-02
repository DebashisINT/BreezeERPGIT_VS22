using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.ServiceManagement
{
    public class SrvModelMastBL : IDisposable
    {
        public void Dispose()
        {

        }

        public DataTable GetModelList()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PROC_InsertUpdateModelMaster");
            proc.AddVarcharPara("@ACTION", 500, "SELECT");
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetModelEdit(String ModelID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PROC_InsertUpdateModelMaster");
            proc.AddVarcharPara("@ACTION", 500, "EDIT");
            proc.AddVarcharPara("@ModelID", 80, ModelID);
            dt = proc.GetTable();
            return dt;
        }

        public int UpdateModelMaster(string ModelDesc, string USER, string ModelID)
        {
            ProcedureExecute proc;
            int retval = 0;
            try
            {
                using (proc = new ProcedureExecute("PROC_InsertUpdateModelMaster"))
                {
                    proc.AddVarcharPara("@Action", 500, "UPDATE");
                    proc.AddVarcharPara("@ModelDesc", 500, ModelDesc);
                    proc.AddVarcharPara("@USER", 150, USER);
                    proc.AddVarcharPara("@ModelID", 150, ModelID);
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

        public int InsertModelMaster(string ModelDesc, string USER)
        {
            ProcedureExecute proc;
            int retval = 0;
            try
            {
                using (proc = new ProcedureExecute("PROC_InsertUpdateModelMaster"))
                {
                    proc.AddVarcharPara("@Action", 500, "INSERT");
                    proc.AddVarcharPara("@ModelDesc", 500, ModelDesc);
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

        public DataTable GetModelChecking(String ModelDesc)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PROC_InsertUpdateModelMaster");
            proc.AddVarcharPara("@ACTION", 500, "CHECKING");
            proc.AddVarcharPara("@ModelDesc", 500, ModelDesc);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable DeleteModelMaster(string ModelID)
        {
            ProcedureExecute proc;
            DataTable dt = new DataTable();
            try
            {
                using (proc = new ProcedureExecute("PROC_InsertUpdateModelMaster"))
                {
                    proc.AddVarcharPara("@Action", 500, "DELETE");
                    proc.AddVarcharPara("@ModelID", 150, ModelID);
                    dt = proc.GetTable();
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
            }
            return dt;
        }

        public int ImportModelMaster(string DBNAME)
        {
            int i = 0;
            ProcedureExecute proc;
            try
            {
                using (proc = new ProcedureExecute("PROC_InsertUpdateModelMaster"))
                {
                    proc.AddVarcharPara("@Action", 500, "IMPORT");
                    proc.AddVarcharPara("@DBNAME", 200, DBNAME);
                    i = proc.RunActionQuery();
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
            }
            return i;
        }
    }

}
