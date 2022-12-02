using BusinessLogicLayer;
using CRM.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UtilityLayer;

namespace CRM.Controllers.CRMS
{
    public class NotificationController : Controller
    {
        //
        // GET: /Notification/
        DBEngine odbengine = new DBEngine();
        NotificationBL notificationbl = new NotificationBL();
        //
        // GET: /MYSHOP/Notification/
        public ActionResult NotificationSettings(string notificationid, string notificationame)
        {
            NotificationClass notification = new NotificationClass();

            DataTable dt = odbengine.GetDataTable("SELECT '0' ID,'All' Name Union all select CAST(deg_id as VARCHAR(20)) ID,deg_designation Name FROM tbl_master_designation WHERE deg_id IN (SELECT desg.deg_id FROM tbl_master_employee EMP INNER JOIN (SELECT cnt.emp_cntId,desg.deg_designation,MAX(emp_id) as emp_id,desg.deg_id FROM tbl_trans_employeeCTC as cnt LEFT OUTER JOIN tbl_master_designation desg ON desg.deg_id=cnt.emp_Designation GROUP BY emp_cntId,desg.deg_designation,desg.deg_id) DESG ON DESG.emp_cntId=EMP.emp_contactId )");

            DataTable dtuser = notificationbl.FetchNotificationSP(notificationid, "", "GetFirstTimeUser", "");
            DataTable dtselecteduser = notificationbl.FetchNotificationSP(notificationid, "", "Getslecteduser", "");
            DataTable dtstate = notificationbl.FetchNotificationSP(notificationid, "", "GetDesignationByState", "");
            DataTable dtedit = notificationbl.FetchNotificationSP(notificationid, "", "GetEditdetails", "");



            if (dtedit == null || dtedit.Rows.Count == 0)
            {
                ViewBag.Recur = "1";
                ViewBag.dtstart = DateTime.Now;
                notification.ddlAction = "1";
                ViewBag.starttime = "00:00 AM";
                ViewBag.endtime = "12:00 PM";
                notification.IsActive = true;
                ViewBag.Action = "";
            }
            else
            {
                ViewBag.Recur = Convert.ToString(dtedit.Rows[0]["INTERVAL"]);
                ViewBag.dtstart = Convert.ToDateTime(dtedit.Rows[0]["START_DATETIME"]);
                notification.ddlAction = Convert.ToString(dtedit.Rows[0]["ACTION"]);
                ViewBag.starttime = Convert.ToString(dtedit.Rows[0]["START_TIME"]);
                ViewBag.endtime = Convert.ToString(dtedit.Rows[0]["END_TIME"]);
                notification.IsActive = Convert.ToBoolean(dtedit.Rows[0]["IsActive"]);
                ViewBag.Action = Convert.ToString(dtedit.Rows[0]["ACTION"]);
            }

            ViewBag.notificationname = notificationame;
            ViewBag.NOTIFICATION_ID = notificationid;


            List<ActionLists> ACTIONLIST = new List<ActionLists>();
            ACTIONLIST.Add(new ActionLists { ActionID = "1", actionname = "Hourly" });
            if (notificationid != "1" && notificationid != "2")
            {
                ACTIONLIST.Add(new ActionLists { ActionID = "2", actionname = "Weekly" });
                ACTIONLIST.Add(new ActionLists { ActionID = "3", actionname = "Monthly" });
                ACTIONLIST.Add(new ActionLists { ActionID = "4", actionname = "Yearly" });
                ACTIONLIST.Add(new ActionLists { ActionID = "5", actionname = "Minutes" });
            }

            notification.ActionList = ACTIONLIST;

            ViewBag.notificationname = notificationame;
            notification.SupervisorList = APIHelperMethods.ToModelList<SupervisorList>(dt);
            notification.UserList = APIHelperMethods.ToModelList<UserNotificationList>(dtuser);
            notification.SelectedUser = APIHelperMethods.ToModelList<UserNotificationList>(dtselecteduser);
            notification.StateList = APIHelperMethods.ToModelList<StateList>(dtstate);


            return View(@"~\Views\CRMS\Notification\NotificationSettings.cshtml",notification);
        }

        public JsonResult GetUserList(string designationid, string notificationId, string stateid)
        {
            //DataTable dtuser = odbengine.GetDataTable("SELECT TMU.user_id UserID,TMU.user_name username FROM tbl_master_employee EMP INNER JOIN  TBL_MASTER_USER  TMU on TMU.user_contactId=EMP.emp_contactId  INNER JOIN (SELECT cnt.emp_cntId,desg.deg_designation,MAX(emp_id) as emp_id,desg.deg_id FROM tbl_trans_employeeCTC as cnt  LEFT OUTER JOIN tbl_master_designation desg ON desg.deg_id=cnt.emp_Designation GROUP BY emp_cntId,desg.deg_designation,desg.deg_id)  DESG ON DESG.emp_cntId=EMP.emp_contactId where DESG.deg_id='" + Convert.ToString(designationid) + "'");


            DataTable dtuser = notificationbl.FetchNotificationSP(notificationId, designationid, "GetUserByNotificationanddesignationId", stateid);


            return Json(APIHelperMethods.ToModelList<UserNotificationList>(dtuser));

        }
        public ActionResult NotificationList()
        {
            DataTable ListNotification = notificationbl.FetchNotificationSP("", "", "GetNotificationListData", "");
            NotificationListClass notiList = new NotificationListClass();
            notiList.notificationList = APIHelperMethods.ToModelList<NotificationGridProperty>(ListNotification);           
            return View(@"~\Views\CRMS\Notification\NotificationList.cshtml",notiList);
        }



        public PartialViewResult NotificationGrid()
        {
            DataTable ListNotification = notificationbl.FetchNotificationSP("", "", "GetNotificationListData", "");
            NotificationListClass notiList = new NotificationListClass();
            notiList.notificationList = APIHelperMethods.ToModelList<NotificationGridProperty>(ListNotification);
            
            return PartialView(@"~\Views\CRMS\Notification\NotificationGrid.cshtml",notiList.notificationList);
        }

        [HttpPost]
        public JsonResult SaveNotificationSetting(NotificationData data)
        {
            DataTable dtselected = CreateDataTable(data.Selecteduser);
            string OutputId = notificationbl.SaveNotificationSP("InsertSchedule", dtselected, data.Action, data.every, Convert.ToDateTime(data.Notidication_date), data.NOTIFICATION_ID, data.starttime, data.endtime, Convert.ToBoolean(data.IsActive));
            return Json(OutputId);
        }

        public static DataTable CreateDataTable<T>(IEnumerable<T> list)
        {
            Type type = typeof(T);
            var properties = type.GetProperties();

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("SelectedUser", typeof(Int32));

            foreach (T entity in list)
            {
                dataTable.Rows.Add(entity);
            }

            return dataTable;
        }
    }
}
