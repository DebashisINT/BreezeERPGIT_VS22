using BusinessLogicLayer.YearEnding;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CutOff.Controllers.CutOff
{
    public class CustomerCutoffController : Controller
    {
        //
        // GET: /CustomerCutoff/
        public ActionResult CutOffCustomerSales()
        {
            BusinessLogicLayer.DBEngine objDb = new BusinessLogicLayer.DBEngine();
            string MasterDbname = ConfigurationSettings.AppSettings["MasterDBName"];
            int UpdateStatus = objDb.ExeInteger("update " + MasterDbname + ".dbo.Master_YearEnding SET Custoff_StartPage='Customer'");


            YearEndingBL yrBl=new YearEndingBL();
            DateTime Con = yrBl.GetCutOffDate();
            ViewBag.CutoffDate = Con;
            return View("~/Views/CutOff/CustomerSales/CutoffCustomerSales.cshtml");
        }
        public JsonResult StartSalesCutoff(DateTime Cutoffdate)
        {
            string msg = "";
            YearEndingSales obj = new YearEndingSales();

            string output = obj.SalesCutOff(Cutoffdate);

            return Json(msg, JsonRequestBehavior.AllowGet);
        }
	}
}