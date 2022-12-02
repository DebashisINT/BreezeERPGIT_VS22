using Payroll.Models;
using Payroll.Repostiory.payrollLeaveStructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Payroll.Controllers.HRPayroll
{
     [Payroll.Models.Attributes.SessionTimeout]
    public class LeaveStructureController : Controller
    {
        LeaveStructureEngine objModel = new LeaveStructureEngine();
        public ILeaveStructureLogic objILeaveStructureLogic;
        public ActionResult Dashboard()
        {
            return View("~/Views/HRPayroll/LeaveStructure/Dashboard.cshtml");
        }
        public ActionResult Index(string ActionType, string LeaveStructureID)
        {
            try
            {
                if (ActionType == "ADD")
                {
                    objModel.StructureHeaderName = "Add Leave Structure";
                    Session["LeaveStructureID"] = null;
                }
                else if (ActionType == "EDIT")
                {
                    Session["LeaveStructureID"] = LeaveStructureID;

                    objILeaveStructureLogic = new LeaveStructureLogic();
                    objModel = objILeaveStructureLogic.GetLeaveStructureDefination(LeaveStructureID);
                    objModel.StructureHeaderName = "Edit Leave Structure";
                }
            }
           catch(Exception ex)
            {

            }
            return View("~/Views/HRPayroll/LeaveStructure/Index.cshtml", objModel);
        }
        [HttpPost]
        public JsonResult EditLeaveDefination(string LeaveStructureID, string LeaveId)
        {
            objILeaveStructureLogic = new LeaveStructureLogic();
             int strIsComplete = 0;
             string strMessage = "";
            try
            {
                objModel = objILeaveStructureLogic.EditLeaveDefination(LeaveStructureID, LeaveId, ref strIsComplete, ref strMessage);
                if (strIsComplete == 1)
                {
                    objModel.ResponseCode = "Success";
                    objModel.ResponseMessage = "Success";


                }

                else
                {
                    objModel.ResponseCode = "Error";
                    objModel.ResponseMessage = "Please try again later";
                }
            }
            catch(Exception ex)
            {
                objModel.ResponseCode = "Catch Error";
                objModel.ResponseMessage = "Please try again later";
            }
            return Json(objModel);
        }
        public PartialViewResult StructureOfLeave(LeaveStructureEngine objModel)
        {
            return PartialView("~/Views/HRPayroll/LeaveStructure/StructureOfLeave.cshtml", objModel);
        }
        public PartialViewResult LeaveDefinition(string LeaveStructureId)
        {
            objModel.ApplicableForList = objModel.PopulateLeaveDropdown("AF");
            objModel.LeaveTypeList = objModel.PopulateLeaveDropdown("LT");
            objModel.HolidayCheckList = objModel.PopulateLeaveDropdown("HC");
            objModel.HolidayRuleList = objModel.PopulateLeaveDropdown("HR");
            objModel.EligibilityTypeList = objModel.PopulateLeaveDropdown("EL");
            objModel.BasicList = objModel.PopulateLeaveDropdown("BA");
            objModel.StructureID = LeaveStructureId;
            objModel.WeeklyOffDaysList = objModel.PopulateWeeklyOffDays();
            objModel._PHoliDayBind = objModel.PopulateHoliDay();
            //objModel._NumberBind = objModel.PopulateNumber();
            //objModel._PayHeadBind = objModel.PopulatePayHead();
            return PartialView("~/Views/HRPayroll/LeaveStructure/LeaveDefinition.cshtml", objModel);
        }

        public PartialViewResult PartialLeaveStructureListGrid()
        {
            return PartialView("~/Views/HRPayroll/LeaveStructure/PartialLeaveStructureListGrid.cshtml", GetFormulaList());
        }

        public IEnumerable GetFormulaList()
        {
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            Payroll.Models.DataContext.PayRollDataClassDataContext dc = new Payroll.Models.DataContext.PayRollDataClassDataContext(connectionString);
            var q = from d in dc.v_proll_LeaveStructureLists
                    orderby d.LeaveStructureID descending
                    select d;
            return q;
        }

        public PartialViewResult PartialLeaveDefinationGrid(String LeaveStructureId)
        {
            LeaveStructureId= Convert.ToString(Session["LeaveStructureID"]);
            ViewData["LeaveStructureId"] = LeaveStructureId;
            return PartialView("~/Views/HRPayroll/LeaveStructure/PartialLeaveDefinationGrid.cshtml", GetDefinationList(LeaveStructureId));
        }

        public IEnumerable GetDefinationList(String LeaveStructureId)
        {
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            Payroll.Models.DataContext.PayRollDataClassDataContext dc = new Payroll.Models.DataContext.PayRollDataClassDataContext(connectionString);
            var q = from d in dc.v_proll_LeaveDefinationLists
                    where d.LeaveStructureID ==LeaveStructureId
                    orderby d.LeaveID descending
                    select d;
            return q;
        }


        [HttpPost]
        public JsonResult LeaveStructureSubmit(LeaveStructureEngine model)
        {
            if (Convert.ToString(model.StructureName) == "")
            {
                model.ResponseCode = "Error";
                model.ResponseMessage = "Leave Structure Name is mandatory";
            }
            else if (Convert.ToString(model.StructureCode) == "")
            {
                model.ResponseCode = "Error";
                model.ResponseMessage = "Leave Structure Short Name is mandatory";
            }
            else if (Convert.ToString(model.FromDate) == "")
            {
                model.ResponseCode = "Error";
                model.ResponseMessage = "Leave Period is mandatory";
            }
            else if (Convert.ToString(model.ToDate) == "")
            {
                model.ResponseCode = "Error";
                model.ResponseMessage = "Leave Period is mandatory";
            }
            else
            {
                int strIsComplete = 0;
                string strMessage = "";
                string StructureID = "";

                objILeaveStructureLogic = new LeaveStructureLogic();
                objILeaveStructureLogic.LeaveStructureModify(model, ref strIsComplete, ref strMessage, ref StructureID);
                if (strIsComplete == 1)
                {
                    model.ResponseCode = "Success";
                    model.ResponseMessage = "Success";

                    Session["LeaveStructureID"] = StructureID;
                }
                else
                {
                    model.ResponseCode = "Error";
                    model.ResponseMessage = strMessage;
                }
            }

            return Json(model);
        }

        [HttpPost]
        public JsonResult LeaveDefinationSubmit(LeaveStructureEngine model)
         {

            try
            {
                int strIsComplete = 0;
                string strMessage = "";
                if (model.StructureID=="")
                {
                    model.StructureID = Convert.ToString(Session["LeaveStructureID"]);
                }
                
                objILeaveStructureLogic = new LeaveStructureLogic();
                objILeaveStructureLogic.LeaveDefinationModify(model, ref strIsComplete, ref strMessage);
                if (strIsComplete == 1)
                {
                    model.ResponseCode = "Success";
                    model.ResponseMessage = "Success";


                }
                else if (strMessage != "Success" && strIsComplete == -1)
                {
                    model.ResponseCode = "Error";
                    model.ResponseMessage = strMessage;
                }
                else
                {
                    model.ResponseCode = "Error";
                    model.ResponseMessage = "Please try again later";
                }

            }
            catch (Exception ex)
            {
                model.ResponseCode = "CatchError";
                model.ResponseMessage = "Please try again later";
            }


            return Json(model);
        }

        [HttpPost]

        public JsonResult StructureDelete(string ActionType, string StructureID)
        {
             string output_msg = string.Empty;
            int ReturnCode = 0;
            objILeaveStructureLogic = new LeaveStructureLogic();
            Msg _msg = new Msg();
            try
            {
                output_msg = objILeaveStructureLogic.Delete(ActionType, StructureID, ref ReturnCode);
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

        public JsonResult LeaveDefinationDelete(string ActionType, string LeaveID)
        {
            string output_msg = string.Empty;
            int ReturnCode = 0;
            objILeaveStructureLogic = new LeaveStructureLogic();
            Msg _msg = new Msg();
            try
            {
                output_msg = objILeaveStructureLogic.Delete(ActionType, LeaveID, ref ReturnCode);
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