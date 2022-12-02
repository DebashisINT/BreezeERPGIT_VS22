using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.ServiceManagement
{
    public class STBSchemeMastBL
    {
        public void Dispose()
        {

        }

        public DataTable GetSTBSchemeList()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PROC_InsertUpdateSTBSchemeMaster");
            proc.AddVarcharPara("@ACTION", 500, "SELECT");
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetSTBSchemeEdit(String STBSchemeID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PROC_InsertUpdateSTBSchemeMaster");
            proc.AddVarcharPara("@ACTION", 500, "EDIT");
            proc.AddVarcharPara("@STBSchemeID", 80, STBSchemeID);
            dt = proc.GetTable();
            return dt;
        }

        public int UpdateSTBSchemeMaster(string STBSchemeDesc, string USER, string STBSchemeID)
        {
            ProcedureExecute proc;
            int retval = 0;
            try
            {
                using (proc = new ProcedureExecute("PROC_InsertUpdateSTBSchemeMaster"))
                {
                    proc.AddVarcharPara("@Action", 500, "UPDATE");
                    proc.AddVarcharPara("@STBSchemeDesc", 500, STBSchemeDesc);
                    proc.AddVarcharPara("@USER", 150, USER);
                    proc.AddVarcharPara("@STBSchemeID", 150, STBSchemeID);
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

        public int InsertSTBSchemeMaster(string STBSchemeDesc, string USER)
        {
            ProcedureExecute proc;
            int retval = 0;
            try
            {
                using (proc = new ProcedureExecute("PROC_InsertUpdateSTBSchemeMaster"))
                {
                    proc.AddVarcharPara("@Action", 500, "INSERT");
                    proc.AddVarcharPara("@STBSchemeDesc", 500, STBSchemeDesc);
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

        public DataTable GetSTBSchemeChecking(String STBSchemeDesc)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PROC_InsertUpdateSTBSchemeMaster");
            proc.AddVarcharPara("@ACTION", 500, "CHECKING");
            proc.AddVarcharPara("@STBSchemeDesc", 500, STBSchemeDesc);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable DeleteSTBSchemeMaster(string STBSchemeID)
        {
            ProcedureExecute proc;
            DataTable dt = new DataTable();
            try
            {
                using (proc = new ProcedureExecute("PROC_InsertUpdateSTBSchemeMaster"))
                {
                    proc.AddVarcharPara("@Action", 500, "DELETE");
                    proc.AddVarcharPara("@STBSchemeID", 150, STBSchemeID);
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

        public int ImportSTBSchemeMaster(string DBNAME)
        {
            int i = 0;
            ProcedureExecute proc;
            try
            {
                using (proc = new ProcedureExecute("PROC_InsertUpdateSTBSchemeMaster"))
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
