using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeSelfService.Areas.EmployeeSelfService.Controllers
{
    public class BiometricApprovalPageController : Controller
    {
        //
        // GET: /EmployeeSelfService/BiometricApprovalPage/

        public ActionResult BiometricApproval()
        {
            return View();  
        }

    }
}
