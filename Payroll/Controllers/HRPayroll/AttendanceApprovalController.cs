using Payroll.Models;
using Payroll.Models.DataContext;
using Payroll.Repostiory.payrollAttendance;
using Payroll.Repostiory.PayrollGeneration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Payroll.Controllers.HRPayroll
{
    public class AttendanceApprovalController : Controller
    {
        public IAttendanceLogic objAttendanceLogic;
        PGenerationEngine objModel = new PGenerationEngine();
        private IPGeneration _PGeneration;
        //
        // GET: /AttendanceApproval/
        public ActionResult Index()
        {

            objModel._PClassName = objModel.PopulateClassName();
            return View("~/Views/HRPayroll/AttendanceApproval/Index.cshtml", objModel);
           
        }

        public object GetEmployeeAttendance(string PayClassID, string YYMM,string EmployeeId)
        { 
            List<AttendanceApprovalEngine> model = new List<AttendanceApprovalEngine>();

            objAttendanceLogic = new AttendanceLogic();
            DataSet ds = objAttendanceLogic.GetEmployeeAttendanceApproval(PayClassID, YYMM,EmployeeId);

            DataTable dt_Status = new DataTable();
            DataTable dt_AttendanceDetailsList = new DataTable();

            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0) 
                dt_Status = ds.Tables[0];

            if (dt_Status != null && dt_Status.Rows.Count > 0)
            {
                model = (from DataRow dr in dt_Status.Rows
                         select new AttendanceApprovalEngine()
                         {
                             Date=Convert.ToString(dr["Date"]),
                             Status = Convert.ToString(dr["Status"]),
                             curStatus = Convert.ToString(dr["cur_Status"])

                         }).ToList();
            }

            
            return Json(model);
        }
        [HttpPost]
        public JsonResult GetEmployeeList(string ClassCode, string yymm)
        {
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            PayRollDataClassDataContext dc = new PayRollDataClassDataContext(connectionString);
            var q = (from d in dc.v_proll_Dflt_Salaries
                    where d.ClassCode == ClassCode && Convert.ToInt32(d.FrmDt) <= Convert.ToInt32(yymm) && Convert.ToInt32(d.ToDt) >= Convert.ToInt32(yymm)
                    orderby d.EmployeeID descending
                    select d).ToList();
            return Json(q);
        }
        [HttpPost]
        public JsonResult SaveApprovalData(Approval model)
        {
            objAttendanceLogic = new AttendanceLogic();
            string output = objAttendanceLogic.SaveApprovalData(model);

            return Json(output);
        }

	}
}