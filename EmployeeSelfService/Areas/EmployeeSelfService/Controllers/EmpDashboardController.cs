using EntityLayer.CommonELS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeSelfService.Areas.EmployeeSelfService.Controllers
{
    public class EmpDashboardController : Controller
    {
        //
        // GET: /EmployeeSelfService/EmpDashboard/
        //public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        public ActionResult EmpDashboard()
        {


            //rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionESS("Dashboard");
            //ViewBag.CanAdd = rights.CanAdd;
            if (Session["userid"] != null)
                return View();
            else
                return Redirect("/OMS/SignOff.aspx");
        }

    }
}
