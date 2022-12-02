using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.SalesmanTrack
{
   public  class Dashboard
    {

       public DataTable GetFtsDashboardyList(string userid)
       {
           DataTable ds = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("FTS_Dashboard");

           proc.AddPara("@UseriD", userid);
           ds = proc.GetTable();

           return ds;
       }
    }
}
