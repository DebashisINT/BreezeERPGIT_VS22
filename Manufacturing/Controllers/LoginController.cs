using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UtilityLayer;

namespace Manufacturing.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult login(string user_id,string password)
        {
            Session["ErpConnection"] = @"Data Source=3.7.30.86,1480\SQLEXPRESS;Initial Catalog=AMIAMOFOODS;User ID=sa; Password=5dc57YITWh";
            Encryption epasswrd = new Encryption();
            string Encryptpass = epasswrd.Encrypt(password);
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(@"Data Source=3.7.30.86,1480\SQLEXPRESS;Initial Catalog=AMIAMOFOODS;User ID=sa; Password=5dc57YITWh;pooling='true';Max Pool Size=200");

           string  Validuser = oDBEngine.AuthenticateUser( user_id, Encryptpass).ToString();




            string msg=string.Empty;
            try
            {
                if (Validuser == "Y")
                {
                    Session["date"] = DateTime.Today.ToString("dd/MM/yyy");
                    Session["date1"] = DateTime.Today.AddDays(10).ToString("dd/MM/yyy");
                    msg = "1";
                }
                else {
                    msg = "0";
                }
            }
            catch(Exception ex)
            {
                msg = ex.Message.ToString();
            }


            return Json(msg, JsonRequestBehavior.AllowGet);

        }
    }
}