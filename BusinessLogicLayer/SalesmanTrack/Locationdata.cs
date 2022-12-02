using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SalesmanTrack
{
    public  class Locationdata
    {
        public DataTable GetLocationList(string userid, string fromdate, string todate, string datespan)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("[Sp_API_Locationfetch_report]");

            proc.AddPara("@user_id", userid);
            proc.AddPara("@date_span", datespan);
            proc.AddPara("@from_date", fromdate);
            proc.AddPara("@to_date", todate);
            ds = proc.GetTable();
            return ds;
        }

    }
}
