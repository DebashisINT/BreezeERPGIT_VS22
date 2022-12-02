using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.ServiceManagement
{
    public class SrvProbMastBL : IDisposable
    {
        public void Dispose()
        {

        }

       public DataTable GetProbList()
       {
           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("PROC_InsertUpdateProblemMaster");
           proc.AddVarcharPara("@ACTION", 500, "SELECT");
           dt = proc.GetTable();
           return dt;
       }

       public DataTable GetProbEdit(String ProblemID)
       {
           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("PROC_InsertUpdateProblemMaster");
           proc.AddVarcharPara("@ACTION", 500, "EDIT");
           proc.AddVarcharPara("@ProblemID", 80, ProblemID);
           dt = proc.GetTable();
           return dt;
       }

       public int UpdateProblemMaster(string ProblemDesc, string USER, string ProblemID)
       {
           ProcedureExecute proc;
           int retval = 0;
           try
           {
               using (proc = new ProcedureExecute("PROC_InsertUpdateProblemMaster"))
               {
                   proc.AddVarcharPara("@Action", 500, "UPDATE");
                   proc.AddVarcharPara("@ProblemDesc", 500, ProblemDesc);
                   proc.AddVarcharPara("@USER", 150, USER);
                   proc.AddVarcharPara("@ProblemID", 150, ProblemID);
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

       public int InsertProblemMaster(string ProblemDesc, string USER)
       {
           ProcedureExecute proc;
           int retval = 0;
           try
           {
               using (proc = new ProcedureExecute("PROC_InsertUpdateProblemMaster"))
               {
                   proc.AddVarcharPara("@Action", 500, "INSERT");
                   proc.AddVarcharPara("@ProblemDesc", 500, ProblemDesc);
                   proc.AddVarcharPara("@USER", 150, USER);
                   retval = proc.RunActionQuery();
               }
           }
           catch (Exception ex)
           {  }
           finally
           {   }
           return retval;
       }

       public DataTable GetProbChecking(String ProblemDesc)
       {
           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("PROC_InsertUpdateProblemMaster");
           proc.AddVarcharPara("@ACTION", 500, "CHECKING");
           proc.AddVarcharPara("@ProblemDesc", 500, ProblemDesc);
           dt = proc.GetTable();
           return dt;
       }

       public int DeleteProblemMaster(string ProblemID)
       {
           ProcedureExecute proc;
           int retval = 0;
           try
           {
               using (proc = new ProcedureExecute("PROC_InsertUpdateProblemMaster"))
               {
                   proc.AddVarcharPara("@Action", 500, "DELETE");
                   proc.AddVarcharPara("@ProblemID", 150, ProblemID);
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
