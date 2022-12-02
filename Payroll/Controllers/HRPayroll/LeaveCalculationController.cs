using DataAccessLayer;
using Payroll.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Payroll.Controllers.HRPayroll
{
    public class LeaveCalculationController : Controller
    {
        //
        // GET: /LeaveCalculation/
        PGenerationEngine objModel = new PGenerationEngine();
        public ActionResult EmpLeaveListing()
        {
            objModel._PClassName = objModel.PopulateClassName();
            return View("~/Views/HRPayroll/LeaveCalculation/EmpLeaveListing.cshtml", objModel);
        }
       
        public JsonResult PopulateEmployee()
        {
            List<UnitList> list = new List<UnitList>();        

            DataTable branchtable = getEmployee();

            if (branchtable.Rows.Count > 0)
            {
                UnitList obj = new UnitList();
                foreach (DataRow item in branchtable.Rows)
                {
                    obj = new UnitList();
                    obj.ID = Convert.ToString(item["EMPCODE"]);
                    obj.Name = Convert.ToString(item["EMPNAME"]);
                    list.Add(obj);
                }
            }

            return Json(list);
        }
        public DataTable getEmployee()
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("proll_LeaveStructureModify");
            proc.AddVarcharPara("@Action", 100, "SelectEmployee");           
            ds = proc.GetTable();
            return ds;
        }
        public class UnitList
        {
            public string ID { get; set; }

            public string Name { get; set; }
        }
        public JsonResult SetEmployeeCodeDateFilter(string EmployeeCode)
        {
            Boolean Success = false;
            try
            {
                TempData["EmployeeCode"] = EmployeeCode;                
                TempData.Keep();
                Success = true;
            }
            catch { }
            return Json(Success);
        }
        public PartialViewResult LeaveCalculationListingGrid()
        {           
            
            return PartialView("~/Views/HRPayroll/LeaveCalculation/PartialLeaveCalculationListing.cshtml", GetLeaveList());
        }
        public DataTable GetLeaveList()
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("proll_LeaveStructureModify");
            proc.AddVarcharPara("@Action", 100, "GetEmployeeLeave");
           // proc.AddVarcharPara("@EmployeeCode", 100, EmployeeCode);
            ds = proc.GetTable();
            return ds;
        }
        [HttpPost]
        public JsonResult MANUALLeaveCalculation(string ClassCode, String yymm)
        {
            string output_msg = string.Empty;
            int ReturnCode = 0;
            LeaveCalculation _LeaveCalculation = new LeaveCalculation();
            Msg _msg = new Msg();
            try
            {
                output_msg = _LeaveCalculation.ManualLeaveCalculation(ClassCode,yymm, ref ReturnCode);
                if (output_msg == "Payroll Generate successfully" && ReturnCode == 1)
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

        [HttpGet]
        public JsonResult GetPeriodName(string classId)
        {

            var jsontable = (String)null; ;
            Msg _msg = new Msg();
            try
            {
                BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
                DataTable dt = objEngine.GetDataTable(@"select proll_PeriodGeneration.Period,proll_PeriodGeneration.YYMM  from proll_PeriodGeneration where     proll_PeriodGeneration.PayrollClassID='" + classId + "' and IsActive=1");
                if (dt != null)
                {
                    _msg.response_code = "Success";
                    _msg.response_msg = "Success";

                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                    Dictionary<string, object> row;
                    foreach (DataRow dr in dt.Rows)
                    {
                        row = new Dictionary<string, object>();
                        foreach (DataColumn col in dt.Columns)
                        {
                            row.Add(col.ColumnName, dr[col]);
                        }
                        rows.Add(row);
                    }
                    jsontable = serializer.Serialize(rows);


                    //ViewData["PeriodFrm"] = dt.Rows[0]["PeriodFrom"].ToString();
                    //ViewData["PeriodTo"] = dt.Rows[0]["PeriodTo"].ToString();
                    //ViewData["Period"] = dt.Rows[0]["Period"].ToString();
                }

            }

            catch (Exception ex)
            {
                _msg.response_code = Convert.ToString(ex);
                _msg.response_msg = "Please try again later";
            }

            var result = new { data = jsontable, data2 = _msg };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

       
	}
}