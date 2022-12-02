using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Payroll.Controllers.HRPayroll
{
    public class PayrollKRAController : Controller
    {
        //
        // GET: /PayrollKRA/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult KRAEntry()
        {
            return View("~/Views/HRPayroll/PayrollKRA/KRAEntry.cshtml");
        }
	}
}