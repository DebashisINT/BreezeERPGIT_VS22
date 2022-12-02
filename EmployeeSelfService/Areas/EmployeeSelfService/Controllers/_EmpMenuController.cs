using EntityLayer.CommonELS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeSelfService.Areas.EmployeeSelfService.Controllers
{
    public class _EmpMenuController : Controller
    {
        //
        // GET: /EmployeeSelfService/_EmpMenu/
        public EntityLayer.CommonELS.UserRightsForPage rightsDashboard = new UserRightsForPage();
        public ActionResult _EmpMenu()
        {
            rightsDashboard = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionESS("Dashboard");
            ViewBag.CanAddDashboard = rightsDashboard.CanAdd;

            rightsDashboard = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionESS("Doc Upload");
            ViewBag.CanDocUpload = rightsDashboard.CanAdd;
            rightsDashboard = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionESS("Work from Home");
            ViewBag.CanWFH = rightsDashboard.CanAdd;

            rightsDashboard = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionESS("Leave");
            ViewBag.CanLEAVE = rightsDashboard.CanAdd;

            rightsDashboard = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionESS("Timesheet");
            ViewBag.CanTIMESHEET = rightsDashboard.CanAdd;

            rightsDashboard = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionESS("Attendance Report");
            ViewBag.CanATTREPORT = rightsDashboard.CanAdd;

             rightsDashboard = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionESS("Help and Support");
            ViewBag.CanHELP = rightsDashboard.CanAdd;

            rightsDashboard = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionESS("Business Meetings");
            ViewBag.CanBM = rightsDashboard.CanAdd;

            rightsDashboard = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionESS("Reimbursment");
            ViewBag.CanRM = rightsDashboard.CanAdd;

            rightsDashboard = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionESS("Payslip");
            ViewBag.CanPayslip = rightsDashboard.CanAdd;

            rightsDashboard = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionESS("Biometric Approval");
            ViewBag.CanBIOAPPROVAL = rightsDashboard.CanAdd;

            rightsDashboard = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionESS("Leave Approval");
            ViewBag.CanLEAVEAPP = rightsDashboard.CanAdd;

            rightsDashboard = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionESS("Reimbursement Approval");
            ViewBag.CanRMAPP = rightsDashboard.CanAdd;

            rightsDashboard = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSessionESS("Business Meeting Approval");
            ViewBag.CanBMAPP = rightsDashboard.CanAdd;
            
            return View();
        }

    }
}
