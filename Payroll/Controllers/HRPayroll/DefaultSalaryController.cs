using Payroll.Models;
using Payroll.Models.DataContext;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Payroll.Controllers.HRPayroll
{
    public class DefaultSalaryController : Controller
    {
         [Payroll.Models.Attributes.SessionTimeout]
        // GET: DefaultSalary
        public ActionResult DashBoard()
        {
            return View("~/Views/HRPayroll/DefaultSalary/DashBoard.cshtml");
        }

        public JsonResult GetActivePeriodGeneration(string ID)
        {

            var jsontable = (String)null; ;
            Msg _msg = new Msg();
            try
            {
                BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
                DataTable dt = objEngine.GetDataTable(@"select proll_PayStructureMaster.StructureID,proll_PayrollClass_Master.PayrollClassID,proll_PayrollClass_Master.PeriodFrom,proll_PayrollClass_Master.PeriodTo,a.YYMM,a.Period from proll_PayStructureMaster
            inner join proll_PayrollClass_Master
            on proll_PayStructureMaster.ClassId=proll_PayrollClass_Master.PayrollClassID
            left join(select proll_PeriodGeneration.PayrollClassID,proll_PeriodGeneration.YYMM,proll_PeriodGeneration.Period  from proll_PeriodGeneration where IsActive=1)a

            on a.PayrollClassID=proll_PayrollClass_Master.PayrollClassID

            where proll_PayStructureMaster.StructureID='" + ID + "'");
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

        public PartialViewResult PartialEmployeeSalaryGrid(string StructureCode,string yymm)
        {
            return PartialView("~/Views/HRPayroll/DefaultSalary/PartialEmployeeSalaryGrid.cshtml", GetEmployeeList(StructureCode, yymm));
        }
        public IEnumerable GetEmployeeList(string StructureCode, string yymm)
        {
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            PayRollDataClassDataContext dc = new PayRollDataClassDataContext(connectionString);
            var q = from d in dc.v_proll_Dflt_Salaries
                    where d.PayStructureCode == StructureCode && Convert.ToInt32(d.FrmDt) <= Convert.ToInt32(yymm) && Convert.ToInt32(d.ToDt) >= Convert.ToInt32(yymm)
                    orderby d.EmployeeID descending
                    select d;
            return q;
        }

        public PartialViewResult PartialAllowanceGrid(string PayStructureCode, string EmployeeCode, string yymm)
        {
            return PartialView("~/Views/HRPayroll/DefaultSalary/PartialAllowanceGrid.cshtml", GetAllowanceList(PayStructureCode, EmployeeCode, yymm));
        }
        public IEnumerable GetAllowanceList(string PayStructureCode, string EmployeeCode, string yymm)
        {
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            PayRollDataClassDataContext dc = new PayRollDataClassDataContext(connectionString);
            var q = from d in dc.v_proll_Salary_AllowanceDeductions
                    where (d.EmployeeCode == EmployeeCode) && (d.StructureID == PayStructureCode) && (d.YYMM == yymm)
                    orderby d.serial descending
                    select d;
            return q;
        }

        //public PartialViewResult PartialDeductionGrid()
        //{
        //    return PartialView("~/Views/HRPayroll/DefaultSalary/PartialDeductionGrid.cshtml", GetDeductionList());
        //}
        //public IEnumerable GetDeductionList()
        //{
        //    string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        //    PayRollDataClassDataContext dc = new PayRollDataClassDataContext(connectionString);
        //    var q = from d in dc.v_proll_Dflt_Salaries

        //            orderby d.EmployeeID descending
        //            select d;
        //    return q;
        //}

    }
}