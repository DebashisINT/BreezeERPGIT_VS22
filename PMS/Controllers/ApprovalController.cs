using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PMS.Controllers
{
    public class ApprovalController : Controller
    {
        //
        // GET: /Approval/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ApprovalView()
        {
            return View();
        }
	}
}