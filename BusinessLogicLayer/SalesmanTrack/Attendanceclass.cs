using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.SalesmanTrack
{
    public class Attendanceclass
    {

        public DataTable GetAttendanceist(string userid, string fromdate, string todate)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("SP_API_Attendance");

            proc.AddPara("@user_id", userid);
            proc.AddPara("@start_date", fromdate);
            proc.AddPara("@end_date", todate);
        
            ds = proc.GetTable();
            return ds;
        }

        
    }
}
