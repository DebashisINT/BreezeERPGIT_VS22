using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.ServiceManagement
{
    public class SrvReturnReasonBL : IDisposable
    {
        public void Dispose()
        {

        }

        public DataTable GetReasonList()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PROC_InsertUpdateReasonMaster");
            proc.AddVarcharPara("@ACTION", 500, "SELECT");
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetReasonEdit(String ReasonID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PROC_InsertUpdateReasonMaster");
            proc.AddVarcharPara("@ACTION", 500, "EDIT");
            proc.AddVarcharPara("@ReasonID", 80, ReasonID);
            dt = proc.GetTable();
            return dt;
        }

        public int UpdateReasonMaster(string ReasonDesc, string USER, string ReasonID)
        {
            ProcedureExecute proc;
            int retval = 0;
            try
            {
                using (proc = new ProcedureExecute("PROC_InsertUpdateReasonMaster"))
                {
                    proc.AddVarcharPara("@Action", 500, "UPDATE");
                    proc.AddVarcharPara("@ReasonDesc", 500, ReasonDesc);
                    proc.AddVarcharPara("@USER", 150, USER);
                    proc.AddVarcharPara("@ReasonID", 150, ReasonID);
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

        public int InsertReasonMaster(string ReasonDesc, string USER)
        {
            ProcedureExecute proc;
            int retval = 0;
            try
            {
                using (proc = new ProcedureExecute("PROC_InsertUpdateReasonMaster"))
                {
                    proc.AddVarcharPara("@Action", 500, "INSERT");
                    proc.AddVarcharPara("@ReasonDesc", 500, ReasonDesc);
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

        public DataTable GetReasonChecking(String ReasonDesc)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PROC_InsertUpdateReasonMaster");
            proc.AddVarcharPara("@ACTION", 500, "CHECKING");
            proc.AddVarcharPara("@ReasonDesc", 500, ReasonDesc);
            dt = proc.GetTable();
            return dt;
        }

        public int DeleteReasonMaster(string ReasonID)
        {
            ProcedureExecute proc;
            int retval = 0;
            DataTable dt = new DataTable();
            try
            {
                using (proc = new ProcedureExecute("PROC_InsertUpdateReasonMaster"))
                {
                    proc.AddVarcharPara("@Action", 500, "DELETE");
                    proc.AddVarcharPara("@ReasonID", 150, ReasonID);
                    dt = proc.GetTable();
                    if (dt!=null && dt.Rows.Count>0)
                    {
                        if (dt.Rows[0]["MSG"].ToString()=="DELETE")
                        {
                            retval = 1;
                        }
                    }
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
