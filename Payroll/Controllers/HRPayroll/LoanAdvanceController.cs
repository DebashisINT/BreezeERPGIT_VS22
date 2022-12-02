using Payroll.Models;
using Payroll.Models.DataContext;
using Payroll.Repostiory.LoanAdvances;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Payroll.Controllers.HRPayroll
{
    public class LoanAdvanceController : Controller
    {
        public IloanAndAdvances objLoanAdvances;
        //
        // GET: /LoanAdvance/
        public ActionResult LoanIndex(string key)
        {
            LoanAndAdvances obj = new LoanAndAdvances();
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            PayRollDataClassDataContext dc = new PayRollDataClassDataContext(connectionString);
            var q = (from d in dc.v_proll_Dflt_Salaries
                     orderby d.EmployeeID descending
                     select d).ToList();
            obj.Emp_List = q;


            var qs = (from d in dc.v_proll_PeriodGenerationLists
                      orderby d.YYMM ascending
                      select d).ToList();

            obj.Period_List = qs;

            Session["Key"] = null;

            if (key != "Add")
            {

                ViewBag.Key = key;
                objLoanAdvances = new proll_LoanAndAdvances();
                DataTable dt = objLoanAdvances.GetLoanAndAdveances(key, "Edit");

                if (dt != null && dt.Rows.Count > 0)
                {
                    obj.Period = Convert.ToString(dt.Rows[0]["Deduction_Starts_Period"]);
                    obj.TYPE = Convert.ToString(dt.Rows[0]["TYPE"]);
                    obj.Name = Convert.ToString(dt.Rows[0]["NAME"]);
                    obj.Min_Check = Convert.ToString(dt.Rows[0]["Min_Check"]);
                    obj.Min_Amount = Convert.ToString(dt.Rows[0]["Min_Amount"]);
                    obj.Max_Check = Convert.ToString(dt.Rows[0]["Max_Check"]);
                    obj.Max_Based_On = Convert.ToString(dt.Rows[0]["Max_Based_On"]);
                    obj.Max_Amount = Convert.ToString(dt.Rows[0]["Max_Amount"]);
                    obj.IsFreeze = Convert.ToString(dt.Rows[0]["IsFreeze"]);
                    obj.Installment = Convert.ToString(dt.Rows[0]["Installment"]);
                    obj.ins_Amount = Convert.ToString(dt.Rows[0]["Ins_Amount"]);
                    if (Convert.ToString(dt.Rows[0]["Freeze_Upto"]) != "")
                        ViewBag.Freeze_Upto = Convert.ToDateTime(dt.Rows[0]["Freeze_Upto"]);


                    obj.Emp_Code = Convert.ToString(dt.Rows[0]["EMPLOYEECODE"]);
                    if (Convert.ToString(dt.Rows[0]["Disb_Date"]) != "")
                        ViewBag.Disb_Date = Convert.ToDateTime(dt.Rows[0]["Disb_Date"]);



                    obj.Deduction_Starts_Period = Convert.ToString(dt.Rows[0]["Deduction_Starts_Period"]);
                    obj.Deduction_Start = Convert.ToString(dt.Rows[0]["IsDeduction_Start_ImmediateLy"]);
                    obj.Code = Convert.ToString(dt.Rows[0]["CODE"]);
                    obj.Amount = Convert.ToString(dt.Rows[0]["Amount"]);



                }

            }



            return View("/Views/HRPayroll/LoanAdvance/LoanIndex.cshtml", obj);
        }

        public PartialViewResult PartialLoanGrid()
        {
            return PartialView("/Views/HRPayroll/LoanAdvance/_PartialLoanAndAdvanceGrid.cshtml", GetData());

        }

        public ActionResult LoanList()
        {
            return View("/Views/HRPayroll/LoanAdvance/LoanAndAdvanceListing.cshtml");

        }

        private object GetData()
        {
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            Payroll.Models.DataContext.PayRollDataClassDataContext dc = new Payroll.Models.DataContext.PayRollDataClassDataContext(connectionString);
            var q = from d in dc.V_LOANADVANCEs
                    select d;
            return q;
        }


        public JsonResult SaveLoan(LoanAndAdvances obj)
        {
            string strOutput="Saved Successfully";
            try
            {
                objLoanAdvances = new proll_LoanAndAdvances();

                strOutput = objLoanAdvances.SaveLoanAndAdveances(obj, obj.Action);

               

            }
            catch
            {
                strOutput = "100";
            }
            return Json( strOutput);

        }

    }
}