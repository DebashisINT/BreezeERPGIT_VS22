using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PMS.Controllers
{
    public class DesignController : Controller
    {
        //
        // GET: /Design/
        public ActionResult DesignPage()
        {
            return View("~/Views/PMS/Master/Design.cshtml");
        }
	}
}