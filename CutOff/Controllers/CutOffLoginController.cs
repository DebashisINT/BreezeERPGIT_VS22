using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UtilityLayer;

namespace CutOff.Controllers
{
    public class CutOffLoginController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult login(string user_id, string password)
        {
            Session["ErpConnection"] = ConfigurationManager.ConnectionStrings["ERP_ConnectionString"].ConnectionString;
            Encryption epasswrd = new Encryption();
            string Encryptpass = epasswrd.Encrypt(password);
            //Rev work start 02.06.2022
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(@"Data Source=3.7.30.86,1480\SQLEXPRESS;Initial Catalog=PK02122020;User ID=sa; Password=5dc57YITWh;pooling='true';Max Pool Size=200");
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(@"Data Source=3.7.30.86,1480\SQLEXPRESS;Initial Catalog=YEAREND_TEST_3_6_2022;User ID=sa; Password=5dc57YITWh;pooling='true';Max Pool Size=200");
            //Rev work close 02.06.2022
            string Validuser = oDBEngine.AuthenticateUser(user_id, Encryptpass).ToString();




            string msg = string.Empty;
            try
            {
                if (Validuser == "Y")
                {
                    Session["CurConString"] = "Con" + Convert.ToString(Session["LastFinYear"]).Replace("-", "").Trim();
                    Session["date"] = DateTime.Today.ToString("dd/MM/yyy");
                    Session["date1"] = DateTime.Today.AddDays(10).ToString("dd/MM/yyy");
                    msg = "1";
                }
                else
                {
                    msg = "0";
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
            }
            return Json(msg, JsonRequestBehavior.AllowGet);

        }
	}
}