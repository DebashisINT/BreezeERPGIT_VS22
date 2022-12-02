using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PMS.Controllers
{
    public class QuestionController : Controller
    {
        //
        // GET: /Question/
        public ActionResult Index()
        {
            return View("~/Views/PMS/Question_PMSHRMS/View1.cshtml");
        }
        public PartialViewResult questionGrid()
        {
            return PartialView("~/Views/PMS/Question_PMSHRMS/questionGrid.cshtml");
        }
	}
}