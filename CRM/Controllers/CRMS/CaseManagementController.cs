using CRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRM.Controllers.CRMS
{
     [CRM.Models.Attributes.SessionTimeout]
    public class CaseManagementController : Controller
    {
        //
        // GET: /CaseManagement/
        public ActionResult Index()
        {
            return View(@"~/Views/CRMS/Cases/cases.cshtml");
        }

        public ActionResult CaseManagementList()
        {
            return View();
        }

        public ActionResult CaseManagementEntry()
        {
            return View();
        }

        public ActionResult GetCaseManagementList()
        {
            List<CaseManagementViewModel> bomproductdata = new List<CaseManagementViewModel>();
            try
            {
                CaseManagementViewModel obj = new CaseManagementViewModel();
                //obj.CaseID = "1";
                bomproductdata.Add(obj);

            }
            catch { }
            return PartialView("_CaseManagementGrid", bomproductdata);
        }
	}
}