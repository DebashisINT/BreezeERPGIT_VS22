using BusinessLogicLayer.PMS;
using DataAccessLayer;
using EntityLayer.CommonELS;
using PMS.Models.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PMS.Controllers
{
    public class PeriodListController : Controller
    {
        //
        // GET: /PeriodList/
        public ActionResult Index()
        {
            return View("~/Views/PMS/Period_PMSHRMS/PeriodList_PMSHRMS.cshtml");
        }
        public PartialViewResult periodGrid()
        {
            return PartialView("~/Views/PMS/Period_PMSHRMS/periodGrid.cshtml");
        }
	}
}