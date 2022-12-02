using BusinessLogicLayer.YearEnding;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CutOff.Controllers.CutOff
{
    public class StockCutoffController : Controller
    {
        public ActionResult CutOffStockValue()
        {

            BusinessLogicLayer.DBEngine objDb = new BusinessLogicLayer.DBEngine();
            string MasterDbname = ConfigurationSettings.AppSettings["MasterDBName"];
            int UpdateStatus = objDb.ExeInteger("update " + MasterDbname + ".dbo.Master_YearEnding SET Custoff_StartPage='Stock'");

            YearEndingBL yrBl = new YearEndingBL();
            DateTime Con = yrBl.GetCutOffDate();
            ViewBag.CutoffDate = Con;
            return View("~/Views/CutOff/StockValue/CutOffStockValue.cshtml");
        }
        public JsonResult StartStockCutoff(DateTime Cutoffdate)
        {
            string msg = "";
            StockCutoff obj = new StockCutoff();
            YearEndingBL yearBl = new YearEndingBL();
            string output = obj.StockCutoffValueCutOff(Cutoffdate);
            string ChallanStatus = yearBl.YearendingChallan(Cutoffdate);
            //string lastStatus = yearBl.YearendingLastStage();
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
	}
}