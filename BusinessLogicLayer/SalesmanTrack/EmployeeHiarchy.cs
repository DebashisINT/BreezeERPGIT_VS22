using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.SalesmanTrack
{
    public class EmployeeHiarchy
    {

        public DataTable GetEmployeeist(string userid)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("SP_API_Userhiarchy");

            proc.AddPara("@user_id", userid);
          

            ds = proc.GetTable();

            return ds;
        }

    }
}
