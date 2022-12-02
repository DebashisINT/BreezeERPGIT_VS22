using Payroll.Models;
using Payroll.Repostiory.payrollClassGeneration;
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
    public class PayrollClassController : Controller
    {
        PClassGenerationEngine objModel = new PClassGenerationEngine();
        private IClassGenrationLogic _ClassGeneration;

        public ActionResult DashBoard()
        {
            return View("~/Views/HRPayroll/PayrollClass/Dashboard.cshtml");
        }
        public ActionResult Index(string ActionType, string PClassId = "")
        {
            try
            {
             
   
                if (ActionType == "ADD" && PClassId == "")
                {
                    ViewBag.title = "Class-Add";
                    ViewBag.classheadername = "Add Payroll Class";

                }
                else if (ActionType == "EDIT" && PClassId!="")
                {
                    string IsGenerate = string.Empty;
                    string GenerateLastDate = string.Empty;
                    _ClassGeneration = new ClassGenerationLogic();
                    objModel = _ClassGeneration.GetClassById(PClassId, ref IsGenerate, ref GenerateLastDate);
                    ViewBag.title = "Class-Edit";
                    ViewBag.classheadername = "Edit Payroll Class";
                    ViewBag.IsGenerate = IsGenerate;
                    ViewBag.GenerateLastDate = GenerateLastDate;
                }

                objModel._PClassUnit = objModel.PopulateClassUnit();
                objModel._PClassGen = objModel.PopulateClassGen();
                objModel._PClassBranchUnit = objModel.PopulateBranchUnit();
                objModel._PHoliDayBind = objModel.PopulateHoliDay();
            }
            catch(Exception ex)
            {

            }
           
            return View("~/Views/HRPayroll/PayrollClass/Index.cshtml",objModel);
        }
        public PartialViewResult PartialClassGrid()
        {
            return PartialView("~/Views/HRPayroll/PayrollClass/PartialClassGrid.cshtml", GetFormulaList());
        }
        public IEnumerable GetFormulaList()
        {
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            Payroll.Models.DataContext.PayRollDataClassDataContext dc = new Payroll.Models.DataContext.PayRollDataClassDataContext(connectionString);
            var q = from d in dc.v_proll_ClassGenerationLists
                    orderby d.CreatedDateTime descending
                    select d;
            return q;
        }
        [HttpPost]
        public JsonResult ClassDelete(string ActionType, string id)
        {

            string output_msg = string.Empty;
            int ReturnCode = 0;
            _ClassGeneration = new ClassGenerationLogic();
            Msg _msg = new Msg();
            try
            {
                output_msg = _ClassGeneration.Delete(ActionType, id, ref ReturnCode);
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
        [HttpPost]
        public JsonResult SaveProllClass(PClassGenerationEngine apply)
        {

            int ReturnCode = 0;
            string ReturnMsg = "";
            Msg _msg = new Msg();
            _ClassGeneration = new ClassGenerationLogic();
            try
            {
                _ClassGeneration.save(apply, ref ReturnCode, ref ReturnMsg);
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
    }
}