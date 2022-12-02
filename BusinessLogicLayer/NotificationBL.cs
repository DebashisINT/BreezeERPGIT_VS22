using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BusinessLogicLayer
{
   public class NotificationBL
    {
        public DataTable FetchNotificationSP(string notificationid, string designationid, string action, string stateid)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_FTSNotification");

            proc.AddPara("@designationid", designationid);
            proc.AddPara("@notificationId", notificationid);
            proc.AddPara("@action", action);
            proc.AddPara("@stateid", stateid);
            ds = proc.GetTable();
            return ds;
        }

        public string SaveNotificationSP(string ACTION, DataTable USERTABLE, string NOTIFICATION_ACTION, string recur, DateTime starts, string NOTIFICATION_ID, string strattime, string endtime, bool IsActive)
        {
            //DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_NOTIFICATION_ADDEDIT");

            proc.AddPara("@ACTION", ACTION);
            proc.AddPara("@USERTABLE", USERTABLE);
            proc.AddPara("@NOTIFICATION_ACTION", NOTIFICATION_ACTION);

            proc.AddPara("@Recur", recur);
            proc.AddPara("@starts", starts);
            proc.AddPara("@strattime", strattime);
            proc.AddPara("@endtime", endtime);
            proc.AddPara("@NOTIFICATION_ID", NOTIFICATION_ID);
            proc.AddBooleanPara("@IsActive", IsActive);
            proc.AddPara("@USER_ID", Convert.ToString(HttpContext.Current.Session["userid"]));
            proc.AddPara("@returntext", SqlDbType.VarChar, 500, ParameterDirection.Output);
            //ds = proc.GetTable();
            proc.GetScalar();
            string OutputId = Convert.ToString(proc.GetParaValue("@returntext"));
            return OutputId;
        }

        public int Savenotification(string mobiles, string amessagetext)
        {
            int s = 0;
            ProcedureExecute proc = new ProcedureExecute("Proc_FCM_NotificationManage");
            proc.AddPara("@Mobiles", mobiles);
            proc.AddPara("@Message", amessagetext);


            s = proc.RunActionQuery();

            return s;
        }
    }
}
