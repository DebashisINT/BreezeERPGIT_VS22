using Payroll.Models;
using Payroll.Models.DataContext;
using Payroll.Repostiory.payrollPeriodGeneration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Payroll.Controllers.HRPayroll
{
    public class PeriodGenerationController : Controller
    {
        private IPeriodGeneration _periodGeneration;
        string _PayClassID = "";
              
        public ActionResult DashBoard()
        {
            return View("~/Views/HRPayroll/PeriodGeneration/DashBoard.cshtml", GetPeriodList());
        }
        public PartialViewResult PartialPeriodGeneration(string PayClassID)
        {
            _PayClassID = PayClassID;
           
            return PartialView("~/Views/HRPayroll/PeriodGeneration/PartialPeriodGeneration.cshtml", GetPeriodList());
        }
        public IEnumerable GetPeriodList()
        {
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            PayRollDataClassDataContext dc = new PayRollDataClassDataContext(connectionString);
         var q = from d in dc.v_proll_PeriodGenerationLists
                    where d.PayrollClassID == Convert.ToString(_PayClassID)
                    orderby d.YYMM ascending
                    select d;
            return q;
        
        }
        public JsonResult setActivePrevNext(string ActionType, string PayClassID)
        {
            string output_msg = string.Empty;
            string output_ActiveYYMM = string.Empty;
            int ReturnCode = 0;
            _periodGeneration = new PeriodGenerationLogic();
            Msg _msg = new Msg();

            try
            {
                output_msg = _periodGeneration.setActivePrevNext(ActionType, PayClassID, ref output_ActiveYYMM, ref ReturnCode);
                if (output_msg == "Success" && ReturnCode == 1)
                {
                    _msg.response_code = "Success";
                    _msg.response_msg = "Success";
                }
                else if (output_msg != "Success" && ReturnCode == -10)
                {
                    _msg.response_code = "Error";
                    _msg.response_msg = output_msg;
                }
                else
                {
                    _msg.response_code = "Error";
                    _msg.response_msg = "Please try again later";
                }
            }
            catch (Exception ex)
            {
                _msg.response_code = Convert.ToString(ex);
                _msg.response_msg = "Please try again later";
            }

            return Json(_msg, JsonRequestBehavior.AllowGet);
        }
    }
}