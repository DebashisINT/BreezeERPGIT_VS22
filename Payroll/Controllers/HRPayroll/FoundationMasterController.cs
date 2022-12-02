using Payroll.Models;
using Payroll.Repostiory.payrollFoundationMaster;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Payroll.Controllers.HRPayroll
{
     [Payroll.Models.Attributes.SessionTimeout]
    public class FoundationMasterController : Controller
    {
         private IFoundationMaster _IFoundationMaster;
        public ActionResult DashBoard()
        {
            return View("~/Views/HRPayroll/FoundationMaster/DashBoard.cshtml");
        }

        public PartialViewResult partialFoundationMasterOuterGrid()
        {
            return PartialView("~/Views/HRPayroll/FoundationMaster/partialFoundationMasterOuterGrid.cshtml", GetOuterGridList());
        }

        public IEnumerable GetOuterGridList()
        {
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            Payroll.Models.DataContext.PayRollDataClassDataContext dc = new Payroll.Models.DataContext.PayRollDataClassDataContext(connectionString);
            var q = from d in dc.v_proll_FoundationMasterOuterLists
                    orderby d.CODE descending
                    select d;
            return q;
        }

        public PartialViewResult partialFoundationMasterInnerGrid(string CODE)
        {
            ViewData["RID"] = CODE;
            return PartialView("~/Views/HRPayroll/FoundationMaster/partialFoundationMasterInnerGrid.cshtml", GetInnerGridList(CODE));
        }

        public IEnumerable GetInnerGridList(string CODE)
        {
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            Payroll.Models.DataContext.PayRollDataClassDataContext dc = new Payroll.Models.DataContext.PayRollDataClassDataContext(connectionString);
            var q = from d in dc.v_proll_FoundationMasterInnerLists
                    where d.RID == CODE
                    orderby d.CODE descending
                    select d;
            return q;
        }

        [HttpPost]
        public JsonResult SaveProllMaster(string desc, string code)
        {

            int ReturnCode = 0;
            string ReturnMsg = "";
            Msg _msg = new Msg();
            _IFoundationMaster = new FoundationMaster();
            try
            {
                _IFoundationMaster.save(desc, code, ref ReturnCode, ref ReturnMsg);
                if (ReturnMsg == "Success" && ReturnCode == 1)
                {
                    _msg.response_code = "Success";
                    _msg.response_msg = "Success";


                }
                else if (ReturnMsg != "Success" && ReturnCode == -1)
                {
                    _msg.response_code = "Error";
                    _msg.response_msg = ReturnMsg;
                }
                else
                {
                    _msg.response_code = "Error";
                    _msg.response_msg = "Please try again later";
                }

            }

            catch (Exception ex)
            {
                _msg.response_code = "CatchError";
                _msg.response_msg = "Please try again later";
            }

            return Json(_msg, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteMaster(string ActionType, string code)
        {

            string output_msg = string.Empty;
            int ReturnCode = 0;
            _IFoundationMaster = new FoundationMaster();
            Msg _msg = new Msg();
            try
            {
                output_msg = _IFoundationMaster.Delete(ActionType, code, ref ReturnCode);
                if (output_msg == "Success" && ReturnCode == 1)
                {
                    _msg.response_code = "Success";
                    _msg.response_msg = "Success";
                }
                else if (output_msg != "Success" && ReturnCode == -1)
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
                _msg.response_code = "CatchError";
                _msg.response_msg = "Please try again later";
            }

            return Json(_msg, JsonRequestBehavior.AllowGet);
        }
    }
}