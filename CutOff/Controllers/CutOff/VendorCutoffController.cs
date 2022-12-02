using BusinessLogicLayer.YearEnding;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CutOff.Controllers.CutOff
{
    public class VendorCutoffController : Controller
    {
        //
        // GET: /VendorCutoff/
        public ActionResult CutOffVendorPurchase()
        {

            BusinessLogicLayer.DBEngine objDb = new BusinessLogicLayer.DBEngine();
            string MasterDbname = ConfigurationSettings.AppSettings["MasterDBName"];
            int UpdateStatus = objDb.ExeInteger("update " + MasterDbname + ".dbo.Master_YearEnding SET Custoff_StartPage='Vendor'");

            YearEndingBL yrBl = new YearEndingBL();
            DateTime Con = yrBl.GetCutOffDate();
            ViewBag.CutoffDate = Con;
            return View("~/Views/CutOff/VendorPurchase/CutoffVendorPurchase.cshtml");
        }
        public JsonResult StartPurchaseCutoff(DateTime Cutoffdate)
        {
            string msg = "";
            YearEndingPurchase obj = new YearEndingPurchase();

            string output = obj.PurchaseCutOff(Cutoffdate);

            return Json(msg, JsonRequestBehavior.AllowGet);
        }
	}
}