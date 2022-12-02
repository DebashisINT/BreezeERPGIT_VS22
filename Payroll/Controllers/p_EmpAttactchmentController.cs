using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Payroll.Controllers
{
    public class p_EmpAttactchmentController : Controller
    {
        public ActionResult Index()
        {
            return View(GetEmployeeList());
        }
        public ActionResult PartialEmployeeGrid()
        {
            return PartialView("PartialEmployeeGrid", GetEmployeeList());    
        }
        public IEnumerable GetEmployeeList()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            Payroll.Models.DataContext.PayRollDataClassDataContext dc = new Payroll.Models.DataContext.PayRollDataClassDataContext(connectionString);
            var q = from d in dc.v_proll_EmployeeLists
                    orderby d.Employee_ID descending
                    select d;
            return q;

            //Payroll.Models.DataContext.PayRollDataClassDataContext DB = new Payroll.Models.DataContext.PayRollDataClassDataContext();
            //return from v_proll_PayStructureMasterList in DB.v_proll_PayStructureMasterLists select v_proll_PayStructureMasterList;
        }
    }
}