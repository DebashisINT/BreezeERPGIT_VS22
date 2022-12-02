using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PMS.Controllers
{
    public class Assessment_PMSHRMSController : Controller
    {
        //
        // GET: /Assessment_PMSHRMS/
        public ActionResult Index()
        {
            return View("~/Views/PMS/Assessment_PMSHRMS/ListAssessment_PMSHRMS.cshtml");
        }
        public PartialViewResult assesmentGrid()
        {
            return PartialView("~/Views/PMS/Assessment_PMSHRMS/assesmentGrid.cshtml");
        }
	}
}