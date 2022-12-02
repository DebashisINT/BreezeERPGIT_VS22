using BusinessLogicLayer;
using CRM.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRM.Controllers.CRMS
{
    public class CustomSMSController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        DBEngine odbengine = new DBEngine();
       // NotificationBL notificationbl = new NotificationBL();
        SendSmsBL ObjSendSMS = new SendSmsBL();

        // GET: /MYSHOP/CustomSMS/
        public ActionResult CustomSMS()
        {

            CustomSMSClass notification = new CustomSMSClass();

            //DataTable dt = odbengine.GetDataTable("SELECT '0' ID,'All' Name Union all select CAST(deg_id as VARCHAR(20)) ID,deg_designation Name FROM tbl_master_designation WHERE deg_id IN (SELECT desg.deg_id FROM tbl_master_employee EMP INNER JOIN (SELECT cnt.emp_cntId,desg.deg_designation,MAX(emp_id) as emp_id,desg.deg_id FROM tbl_trans_employeeCTC as cnt LEFT OUTER JOIN tbl_master_designation desg ON desg.deg_id=cnt.emp_Designation GROUP BY emp_cntId,desg.deg_designation,desg.deg_id) DESG ON DESG.emp_cntId=EMP.emp_contactId )");
            //DataTable dtuser = notificationbl.FetchNotificationSP("", "", "GetFirstTimeUserWithLoginID", "");
            //DataTable dtstate = notificationbl.FetchNotificationSP("", "", "GetDesignationByState", "");
            //notification.SupervisorList = APIHelperMethods.ToModelList<SupervisorList>(dt);
            //notification.UserList = APIHelperMethods.ToModelList<UserNotificationList>(dtuser);
            //notification.StateList = APIHelperMethods.ToModelList<StateList>(dtstate);


            return View(notification);
        }
        //public JsonResult GetUserList(string designationid, string notificationId, string stateid)
        //{
        //    DataTable dtuser = notificationbl.FetchNotificationSP("", designationid, "GetUserByNotificationanddesignationIdWithLoginID", stateid);
        //    return Json(APIHelperMethods.ToModelList<UserNotificationList>(dtuser));
        //}

        public JsonResult SendSMS(string Mobiles, string messagetext)
        {
            string status = string.Empty;
            try
            {

                //String tokenmatch = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                //String username = System.Configuration.ConfigurationSettings.AppSettings["username"];
                //String password = System.Configuration.ConfigurationSettings.AppSettings["password"];
                //String Provider = System.Configuration.ConfigurationSettings.AppSettings["Provider"];
                //String sender = System.Configuration.ConfigurationSettings.AppSettings["sender"];

                String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];


                string res = ObjSendSMS.sendSMS(Mobiles, messagetext);
                //string res = SmsSent(username, password, Provider, sender, Mobiles, messagetext, "Text");
                if (res != "0")
                {
                    status = "200";
                    return Json(status, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    status = "300";
                    return Json(status, JsonRequestBehavior.AllowGet);
                }
                return Json(status, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                status = "300";
                return Json(status, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SendSMSToShop(string StateID, string messagetext)
        {
            string status = string.Empty;
            try
            {

                //String tokenmatch = System.Configuration.ConfigurationSettings.AppSettings["AuthToken"];
                //String username = System.Configuration.ConfigurationSettings.AppSettings["username"];
                //String password = System.Configuration.ConfigurationSettings.AppSettings["password"];
                //String Provider = System.Configuration.ConfigurationSettings.AppSettings["Provider"];
                //String sender = System.Configuration.ConfigurationSettings.AppSettings["sender"];

                String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];

                string res = "1";
                DataTable dt = new DataTable();
                dt = odbengine.GetDataTable("exec Proc_ShopDetails_Type  1,'" + StateID + "','Shop'");

                foreach (DataRow item in dt.Rows)
                {
                    string mobile = Convert.ToString(item[0]); //+","+Convert.ToString(item[1]);
                   // res = SmsSent(username, password, Provider, sender, mobile, messagetext, "Text");
                    res = ObjSendSMS.sendSMS(mobile, messagetext);
                }

                dt = odbengine.GetDataTable("exec Proc_ShopDetails_Type  1,'" + StateID + "','User'");
                foreach (DataRow item in dt.Rows)
                {
                    string mobile = Convert.ToString(item[0]); //+","+Convert.ToString(item[1]);
                    res = ObjSendSMS.sendSMS(mobile, messagetext);
                   // res = SmsSent(username, password, Provider, sender, mobile, messagetext, "Text");
                }
                if (res != "0")
                {
                    status = "200";
                    return Json(status, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    status = "300";
                    return Json(status, JsonRequestBehavior.AllowGet);
                }
                return Json(status, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                status = "300";
                return Json(status, JsonRequestBehavior.AllowGet);
            }
        }

        //public string SmsSent(string username, string password, string Provider, string senderId, string mobile, string message, string type)
        //{

        //    //  http://5.189.187.82/sendsms/sendsms.php?username=QHEkaruna&password=baj8piv3&type=Text&sender=KARUNA&mobile=9831892083&message=HELO
        //    string response = "";
        //    string url = Provider + "?username=" + username + "&password=" + password + "&type=" + type + "&sender=" + senderId + "&mobile=" + mobile + "&message=" + message;
        //    if (mobile.Trim() != "")
        //    {
        //        try
        //        {
        //            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
        //            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        //            response = httpResponse.StatusCode.ToString();
        //        }
        //        catch
        //        {
        //            return "0";

        //        }
        //    }
        //    return response;
        //}
	}
}