using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UtilityLayer;

namespace Payroll.Controllers
{
    public class payLoginController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult login(string user_id, string password)
        {
            //Session["ErpConnection"] = @"Data Source=10.0.8.251\MSSQLSERVER17;Initial Catalog=PK29042019;User ID=sa; Password=sql@123";
            Session["ErpConnection"] = @"Data Source=52.66.59.156,1480\SQLEXPRESS;Initial Catalog=Peekay;User ID=sa; Password=4yU29211*HE";
            //Session["ErpConnection"] = @"Data Source=10.0.8.251\MSSQLSERVERR2;Initial Catalog=PK07012019;User ID=sa; Password=sql@123";

            Encryption epasswrd = new Encryption();
            string Encryptpass = epasswrd.Encrypt(password);
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(@"Data Source=10.0.8.251\MSSQLSERVER17;Initial Catalog=PK29042019;User ID=sa; Password=sql@123;pooling='true';Max Pool Size=200");
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(@"Data Source=52.66.59.156,1480\SQLEXPRESS;Initial Catalog=Peekay;User ID=sa; Password=4yU29211*HE;pooling='true';Max Pool Size=200");
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(@"Data Source=10.0.8.251\MSSQLSERVERR2;Initial Catalog=PK07012019;User ID=sa; Password=sql@123;pooling='true';Max Pool Size=200");

            string Validuser = oDBEngine.AuthenticateUser(user_id, Encryptpass).ToString();
            string msg = string.Empty;

            try
            {
                if (Validuser == "Y")
                {
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