using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Payroll.Controllers
{
    public class p_StructureListController : Controller
    {
        // GET: p_StructureList
        public ActionResult Index()
        {
            //return View();
            return View(GetPaystructure());
        }
        public ActionResult GridViewPart()
        {
            return PartialView("PartialPayStructure", GetPaystructure());
            
        }

        public IEnumerable GetPaystructure()
        {
            //string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[HttpContext.Current.Session["CurConString"].ToString().Trim()].ConnectionString;
            //Payroll.Models.DataContext.PayRollDataClassDataContext dc = new Payroll.Models.DataContext.PayRollDataClassDataContext(connectionString);
            //var q = from d in dc.v_proll_PayStructureMasterLists
            //        orderby d.StructureID descending
            //        select d;
            //return q;

            Payroll.Models.DataContext.PayRollDataClassDataContext DB = new Payroll.Models.DataContext.PayRollDataClassDataContext();
            return from v_proll_PayStructureMasterList in DB.v_proll_PayStructureMasterLists select v_proll_PayStructureMasterList;
        }

        public ActionResult GridViewPartialAddNew()
        {
            return RedirectToAction("Index", "Structure");

        }
        public ActionResult GridViewPartialUpdate()
        {
            return RedirectToAction("Index", "Structure");

        }

    }
}