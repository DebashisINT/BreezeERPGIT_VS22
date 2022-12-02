using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using DataAccessLayer;
using System.Web;
using EntityLayer;

namespace BusinessLogicLayer
{
   public class UserBL
    {
       
       public DataTable PopulateAssociatedEmployee(int userid,string action)
       {

           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_UserDetail");
          
           proc.AddVarcharPara("@Action", 500, action);
           proc.AddIntegerPara("@userid", userid);
           
           dt = proc.GetTable();
           return dt;
       }
       public int UpdateUserStatus(string UserId)
       {
           DataSet dsInst = new DataSet();
           ProcedureExecute proc = new ProcedureExecute("prc_UserDetail");
           proc.AddPara("@userid", UserId);
           proc.AddPara("@Action", "UserLoggoff");
           return proc.RunActionQuery();
       }

    }
}
