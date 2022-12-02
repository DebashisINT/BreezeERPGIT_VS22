using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesmanTrack
{
    public  class UserList
    {
        public DataTable GetUserList(string userid)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("API_Salesman_Getuserslist");

            proc.AddPara("@userreportto", userid);
            
            ds = proc.GetTable();
            return ds;
        }



        public DataTable GetUserList(string userid, string Type)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("API_Salesman_Getuserslist");

            proc.AddPara("@userreportto", userid);
            proc.AddPara("@Type", Type);
            ds = proc.GetTable();
            return ds;
        }


        public DataTable GetDesignationList()
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("SP_API_Designation_Userwise");


            ds = proc.GetTable();
            return ds;
        }



        public DataTable GetUserLocationList(string user)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("API_SalesmanLocationcurrent");

            proc.AddPara("@userid", user);

            ds = proc.GetTable();
            return ds;
        }

    }
}
