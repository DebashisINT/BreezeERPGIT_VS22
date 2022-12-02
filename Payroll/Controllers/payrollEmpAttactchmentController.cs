using BusinessLogicLayer;
using Payroll.Models;
using Payroll.Repostiory.EmpAttachment;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Payroll.Controllers
{
    [Payroll.Models.Attributes.SessionTimeout]
    public class payrollEmpAttactchmentController : Controller
    {
        IAttachmentLogic objIAttachmentLogic;

        public ActionResult Dashboard()
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
        }
        [HttpPost]
        public JsonResult AttactchmentSubmit(p_EmpAttactchmentEngine model)
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
                if (model.EmployeeList != "")
                {
                    string[] EmployeeList = model.EmployeeList.Split(',');
                    for (int i = 0; i < EmployeeList.Length; i++)
                    {
                        table.Rows.Add(new object[] { EmployeeList[i] });
                    }
                }

                objIAttachmentLogic = new AttachmentLogic();
                objIAttachmentLogic.AttachmentModify(model, table, ref strIsComplete, ref strMessage);
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
    }
}