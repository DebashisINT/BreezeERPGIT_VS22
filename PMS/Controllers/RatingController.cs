using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PMS.Controllers
{
    public class RatingController : Controller
    {
        //
        // GET: /Rating/
        public ActionResult Index()
        {
            return View("~/Views/PMS/Rating_PMSHRMS/View1.cshtml");
        }
	}
}