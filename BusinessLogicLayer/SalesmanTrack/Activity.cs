using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.SalesmanTrack
{
  public  class Activity
    {
        public DataSet GetDaywiseActivityList(string userid, string fromdate, string todate, string datespan)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Sp_API_DaywiseShop_Report");

            proc.AddPara("@date_span", datespan);
            proc.AddPara("@from_date", fromdate);
            proc.AddPara("@to_date", todate);
            proc.AddPara("@user_id", userid);
            proc.AddPara("@Action", "0");
            ds = proc.GetDataSet();

            return ds;
        }

    }
}
