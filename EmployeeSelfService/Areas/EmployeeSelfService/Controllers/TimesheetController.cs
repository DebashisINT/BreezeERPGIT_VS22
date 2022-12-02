using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeSelfService.Areas.EmployeeSelfService.Controllers
{
    public class ESSTimesheetController : Controller
    {
        //
        // GET: /EmployeeSelfService/Timesheet/

        public ActionResult Timesheet()
        {
            return View();
        }

    }
}
