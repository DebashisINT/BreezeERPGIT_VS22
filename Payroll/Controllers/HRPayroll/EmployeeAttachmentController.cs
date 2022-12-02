using BusinessLogicLayer;
using Payroll.Models;
using Payroll.Models.DataContext;
using Payroll.Repostiory.payrollEmpAttachment;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Payroll.Controllers.HRPayroll
{
    [Payroll.Models.Attributes.SessionTimeout]
    public class EmployeeAttachmentController : Controller
    {
        IAttachmentLogic objIAttachmentLogic;
        EmployeeAttachmentEngine model = new EmployeeAttachmentEngine();
        public ActionResult Dashboard()
        {
            return View("~/Views/HRPayroll/EmployeeAttachment/Dashboard.cshtml", GetEmployeeList());
        }
        public ActionResult PartialEmployeeGrid()
        {
            return PartialView("~/Views/HRPayroll/EmployeeAttachment/PartialEmployeeGrid.cshtml", GetEmployeeList());
        }
        public IEnumerable GetEmployeeList()
        {
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            PayRollDataClassDataContext dc = new PayRollDataClassDataContext(connectionString);
            var q = from d in dc.v_proll_EmployeeLists
                    orderby d.Employee_ID descending
                    select d;
            return q;
        }
        public ActionResult PartialAttachmentGrid()
        {
            return PartialView("~/Views/HRPayroll/EmployeeAttachment/PartialAttachmentGrid.cshtml", GetAttachmentList());
        }
        public IEnumerable GetAttachmentList()
        {
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            PayRollDataClassDataContext dc = new PayRollDataClassDataContext(connectionString);
            var q = from d in dc.v_proll_EmployeeAttachments
                    orderby d.Employee_Code descending
                    select d;
            return q;
        }

        [HttpPost]
        public JsonResult AttactchmentSubmit(EmployeeAttachmentEngine model)
        {
            if (Convert.ToString(model.PayStructureCode) != "" && Convert.ToString(model.Pay_ApplicationFrom) == "")
            {
                model.ResponseCode = "Error";
                model.ResponseMessage = "Application From Date of Pay Structure is mandatory";
            }
            else if (Convert.ToString(model.PayStructureCode) != "" && Convert.ToString(model.Pay_ApplicationTo) == "")
            {
                model.ResponseCode = "Error";
                model.ResponseMessage = "Application To Date of Pay Structure is mandatory";
            }
            else if (Convert.ToString(model.LeaveStructureCode) != "" && Convert.ToString(model.Leave_ApplicationFrom) == "")
            {
                model.ResponseCode = "Error";
                model.ResponseMessage = "Application From Date of Leave Structure is mandatory";
            }
            else if (Convert.ToString(model.LeaveStructureCode) != "" && Convert.ToString(model.Leave_ApplicationTo) == "")
            {
                model.ResponseCode = "Error";
                model.ResponseMessage = "Application To Date of Leave Structure is mandatory";

            }
            else if (Convert.ToString(model.RosterID) != "" && Convert.ToString(model.RosterFrom) == "")
            {
                model.ResponseCode = "Error";
                model.ResponseMessage = "Application From Date of Roster is mandatory";
            }
            else if (Convert.ToString(model.RosterID) != "" && Convert.ToString(model.RosterTo) == "")
            {
                model.ResponseCode = "Error";
                model.ResponseMessage = "Application To Date of Roster is mandatory";
            }
            else if (Convert.ToString(model.EmployeeList) == "")
            {
                model.ResponseCode = "Error";
                model.ResponseMessage = "Please select an Employee";
            }
            else
            {
                int strIsComplete = 0;
                string strMessage = "";

                DataTable table = new DataTable();
                table.Columns.Add("EmployeeCode", typeof(string));
                //table.Columns.Add("Sunday", typeof(string));
                //table.Columns.Add("Monday", typeof(string));
                //table.Columns.Add("Tuesday", typeof(string));
                //table.Columns.Add("Wednesday", typeof(string));
                //table.Columns.Add("Thursday", typeof(string));
                //table.Columns.Add("Friday", typeof(string));
                //table.Columns.Add("Saturday", typeof(string));

                if (model.EmployeeList != "")
                {
                    string[] EmployeeList = model.EmployeeList.Split(',');
                    for (int i = 0; i < EmployeeList.Length; i++)
                    {
                        table.Rows.Add(new object[] { EmployeeList[i] });
                    }
                }
                //var firstDayOfMonth = new DateTime(Convert.ToDateTime(model.Leave_ApplicationFrom).Year, Convert.ToDateTime(model.Leave_ApplicationFrom).Month, 1);
                //var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                DateTime? firstDayOfMonth = null;
                //var firstDayOfMonth = DateTime.MinValue;
                DateTime? lastDayOfMonth = DateTime.MinValue;
                int Sunday = 0;
                int monday = 0;
                int tuesday = 0;
                int wednesday = 0;
                int thursday = 0;
                int friday = 0;
                int saturday = 0;

                if (model.Leave_ApplicationFrom != "" && model.Leave_ApplicationFrom!=null)
                { 
                 firstDayOfMonth = new DateTime(Convert.ToDateTime(model.Leave_ApplicationFrom).Year, Convert.ToDateTime(model.Leave_ApplicationFrom).Month, 1);
                 lastDayOfMonth = new DateTime(Convert.ToDateTime(model.Leave_ApplicationFrom).Year, Convert.ToDateTime(model.Leave_ApplicationFrom).Month, 1).AddMonths(1).AddDays(-1);
                 Sunday = CountDays(DayOfWeek.Sunday, new DateTime(Convert.ToDateTime(model.Leave_ApplicationFrom).Year, Convert.ToDateTime(model.Leave_ApplicationFrom).Month, 1), new DateTime(Convert.ToDateTime(model.Leave_ApplicationFrom).Year, Convert.ToDateTime(model.Leave_ApplicationFrom).Month, 1).AddMonths(1).AddDays(-1));
                 monday = CountDays(DayOfWeek.Monday, new DateTime(Convert.ToDateTime(model.Leave_ApplicationFrom).Year, Convert.ToDateTime(model.Leave_ApplicationFrom).Month, 1), new DateTime(Convert.ToDateTime(model.Leave_ApplicationFrom).Year, Convert.ToDateTime(model.Leave_ApplicationFrom).Month, 1).AddMonths(1).AddDays(-1));
                 tuesday = CountDays(DayOfWeek.Tuesday, new DateTime(Convert.ToDateTime(model.Leave_ApplicationFrom).Year, Convert.ToDateTime(model.Leave_ApplicationFrom).Month, 1), new DateTime(Convert.ToDateTime(model.Leave_ApplicationFrom).Year, Convert.ToDateTime(model.Leave_ApplicationFrom).Month, 1).AddMonths(1).AddDays(-1));
                 wednesday = CountDays(DayOfWeek.Wednesday, new DateTime(Convert.ToDateTime(model.Leave_ApplicationFrom).Year, Convert.ToDateTime(model.Leave_ApplicationFrom).Month, 1), new DateTime(Convert.ToDateTime(model.Leave_ApplicationFrom).Year, Convert.ToDateTime(model.Leave_ApplicationFrom).Month, 1).AddMonths(1).AddDays(-1));
                 thursday = CountDays(DayOfWeek.Thursday, new DateTime(Convert.ToDateTime(model.Leave_ApplicationFrom).Year, Convert.ToDateTime(model.Leave_ApplicationFrom).Month, 1), new DateTime(Convert.ToDateTime(model.Leave_ApplicationFrom).Year, Convert.ToDateTime(model.Leave_ApplicationFrom).Month, 1).AddMonths(1).AddDays(-1));
                 friday = CountDays(DayOfWeek.Friday, new DateTime(Convert.ToDateTime(model.Leave_ApplicationFrom).Year, Convert.ToDateTime(model.Leave_ApplicationFrom).Month, 1), new DateTime(Convert.ToDateTime(model.Leave_ApplicationFrom).Year, Convert.ToDateTime(model.Leave_ApplicationFrom).Month, 1).AddMonths(1).AddDays(-1));
                 saturday = CountDays(DayOfWeek.Saturday, new DateTime(Convert.ToDateTime(model.Leave_ApplicationFrom).Year, Convert.ToDateTime(model.Leave_ApplicationFrom).Month, 1), new DateTime(Convert.ToDateTime(model.Leave_ApplicationFrom).Year, Convert.ToDateTime(model.Leave_ApplicationFrom).Month, 1).AddMonths(1).AddDays(-1));
                }
                objIAttachmentLogic = new AttachmentLogic();
                objIAttachmentLogic.AttachmentModify(model, table,Sunday,monday,tuesday,wednesday,thursday,friday,saturday,ref strIsComplete, ref strMessage);
                if (strIsComplete == 1)
                {
                    model.ResponseCode = "Success";
                    model.ResponseMessage = "Success";
                }
                else
                {
                    model.ResponseCode = "Error";
                    model.ResponseMessage = strMessage;
                }
            }
            return Json(model);
        }


        public int CountDays(DayOfWeek day, DateTime start, DateTime end)
        {
            TimeSpan ts = end - start;                       // Total duration
            int count = (int)Math.Floor(ts.TotalDays / 7);   // Number of whole weeks
            int remainder = (int)(ts.TotalDays % 7);         // Number of remaining days
            int sinceLastDay = (int)(end.DayOfWeek - day);   // Number of days since last [day]
            if (sinceLastDay < 0) sinceLastDay += 7;         // Adjust for negative days since last [day]

            // If the days in excess of an even week are greater than or equal to the number days since the last [day], then count this one, too.
            if (remainder >= sinceLastDay) count++;

            return count;
        }

        [HttpPost]
        public JsonResult AttactchmentModify(EmployeeAttachmentEngine model)
        {
           
            if (Convert.ToString(model.Pay_ApplicationTo) == "")
            {
                model.ResponseCode = "Error";
                model.ResponseMessage = "Application To Date of Pay Structure is mandatory";
            }
            else if (Convert.ToString(model.Leave_ApplicationTo) == "")
            {
                model.ResponseCode = "Error";
                model.ResponseMessage = "Application To Date of Leave Structure is mandatory";

            }

            else if (Convert.ToString(model.RosterTo) == "")
            {
                model.ResponseCode = "Error";
                model.ResponseMessage = "Application To Date of Roster is mandatory";

            }
           
            else
            {
                int strIsComplete = 0;
                string strMessage = "";

                

                objIAttachmentLogic = new AttachmentLogic();
                objIAttachmentLogic.AttachmentUpdate(model,  ref strIsComplete, ref strMessage);
                if (strIsComplete == 1)
                {
                    model.ResponseCode = "Success";
                    model.ResponseMessage = "Success";
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
        public JsonResult DeleteAttachment(string ID)
        {
            int strIsComplete = 0;
            string strMessage = "";

            EmployeeAttachmentEngine model=new EmployeeAttachmentEngine();
            objIAttachmentLogic = new AttachmentLogic();
            objIAttachmentLogic.DeleteStructure(ID, ref strIsComplete, ref strMessage);
            if (strIsComplete == 1)
            {
                model.ResponseCode = "Success";
                model.ResponseMessage = "Success";
            }
            else if (strIsComplete == -99)
            {
                model.ResponseCode = "AllReadyUsed";
                model.ResponseMessage = "AllReadyUsed";
            }
            else
            {
                model.ResponseCode = "Error";
                model.ResponseMessage = strMessage;
            }

            return Json(model);
        }

        [HttpPost]
        public JsonResult EditAttachment (string ID)
        {
             objIAttachmentLogic = new AttachmentLogic();
             DataTable dtDetails = new DataTable();
             dtDetails = objIAttachmentLogic.EditAttachment(ID);
             if (dtDetails != null && dtDetails.Rows.Count > 0)
             {
                 model.Pay_ApplicationFrom = Convert.ToString(dtDetails.Rows[0]["Pay_ApplicationFrom"]);
                 model.Pay_ApplicationTo = Convert.ToString(dtDetails.Rows[0]["Pay_ApplicationTo"]);
                 model.Leave_ApplicationFrom = Convert.ToString(dtDetails.Rows[0]["Leave_ApplicationFrom"]);
                 model.Leave_ApplicationTo = Convert.ToString(dtDetails.Rows[0]["Leave_ApplicationTo"]);
                 model.RosterFrom = Convert.ToString(dtDetails.Rows[0]["Roster_ApplicationFrom"]);
                 model.RosterTo = Convert.ToString(dtDetails.Rows[0]["Roster_ApplicationTo"]);

                 model.LeaveStructureCode = Convert.ToString(dtDetails.Rows[0]["LeaveStructureCode"]);
                 model.LeaveStructureName = Convert.ToString(dtDetails.Rows[0]["LeaveStructureName"]);

                 model.RosterID = Convert.ToString(dtDetails.Rows[0]["RosterID"]);
                 model.RosterName = Convert.ToString(dtDetails.Rows[0]["RosterName"]);

              
                 model.ResponseCode = "Success";
             }
             else
             {
                 model.ResponseCode = "Error";
             }
             return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}