using Payroll.Models;
using Payroll.Repostiory.P_Formula;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UtilityLayer;

namespace Payroll.Controllers
{
    [Payroll.Models.Attributes.SessionTimeout]
    public class payrollTableFormulaController : Controller
    {
        private IPayrole_formula _formula;

        public ActionResult Dashboard()
        {
            return View("~/Views/HRPayroll/payrollTableFormula/Dashboard.cshtml");
        }
        public PartialViewResult PartialFormulaGrid()
        {
            return PartialView("~/Views/HRPayroll/payrollTableFormula/PartialFormulaGrid.cshtml", GetFormulaList());
        }
        public IEnumerable GetFormulaList()
        {
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            Payroll.Models.DataContext.PayRollDataClassDataContext dc = new Payroll.Models.DataContext.PayRollDataClassDataContext(connectionString);
            var q = from d in dc.v_proll_TableFormulaLists
                    orderby d.TableFormulaCode descending
                    select d;
            return q;
        }

        public PartialViewResult partialReferredGrid(string TblFormulaCode)
            
        {
            ViewData["TblFormulaCode"] = TblFormulaCode;
            return PartialView("~/Views/HRPayroll/payrollTableFormula/partialReferredGrid.cshtml", GetReferrerList(TblFormulaCode));
        }

        public IEnumerable GetReferrerList(string TblFormulaCode)
        {
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            Payroll.Models.DataContext.PayRollDataClassDataContext dc = new Payroll.Models.DataContext.PayRollDataClassDataContext(connectionString);
            if (TblFormulaCode != null)
            {
                var q = from d in dc.v_proll_TableFormulaOuterLists
                        where d.TableFormulaCode == TblFormulaCode
                        orderby d.CreatedDateTime descending
                        select d;
                return q;
            }
            else
            {
                var q = from d in dc.v_proll_TableFormulaLists
                        where d.TableName == ""
                        orderby d.TableFormulaCode descending
                        select d;
                return q;
            }


        }


        public PartialViewResult partialReferredInnerGrid(int TableBreakup_ID)
        {
            ViewData["TableBreakup_ID"] = TableBreakup_ID;
            return PartialView("~/Views/HRPayroll/payrollTableFormula/partialReferredInnerGrid.cshtml", GetRefferedInnerList(TableBreakup_ID));
        }

        public IEnumerable GetRefferedInnerList(int TableBreakup_ID)
        {
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            Payroll.Models.DataContext.PayRollDataClassDataContext dc = new Payroll.Models.DataContext.PayRollDataClassDataContext(connectionString);
            var q = from d in dc.v_proll_TableFormulaInnerLists
                    where d.TableBreakup_ID == TableBreakup_ID
                    orderby d.CreatedDateTime descending
                    select d;
            return q;
        }

        //[Route("payrollTableFormulaDetails/{id?}")]
        public ActionResult Index(string ActionType, int TableBreakUpId = 0, string _formulacode = "", string EditFlag = "")
        {
            _formula = new P_formulaBal();
            FormulaApply _apply = new FormulaApply();
            P_formula_header _header = new P_formula_header();
            _apply.header = _header;

            if (ActionType == "ADD" && _formulacode == "")
            {
                _apply.header.FormulaHeaderName = "Add Table Formula";
                ViewBag.title = "Table-Add";
            }
            else if (ActionType == "EDIT" && _formulacode != "")
            {


                _apply = _formula.getFormulaDetailsById(_formulacode, EditFlag, TableBreakUpId);
                // ViewBag.dtls = _apply.dtls;
                _apply.header.FormulaHeaderName = "Edit Table Formula";
                ViewBag.title = "Table-Edit";

            }
            return View("~/Views/HRPayroll/payrollTableFormula/Index.cshtml", _apply);

        }

        [HttpPost]
        public JsonResult Apply(FormulaApply apply)
        {
            string output_msg = string.Empty;
            string tblformulaid = string.Empty;
            int ReturnCode = 0;
            string ReturnMsg = "";
            _formula = new P_formulaBal();
            try
            {
                _formula.save(apply, ref tblformulaid, ref ReturnCode, ref ReturnMsg);
                if (ReturnMsg == "Success" && ReturnCode == 1)
                {
                    apply.response_code = "Success";
                    apply.response_msg = "Success";
                    apply.header.tableFormulaCode = tblformulaid;

                }
                else if (ReturnMsg != "Success" && ReturnCode == -1)
                {
                    apply.response_code = "Error";
                    apply.response_msg = ReturnMsg;
                }
                else
                {
                    apply.response_code = "Error";
                    apply.response_msg = "Please try again later";
                }

            }

            catch (Exception ex)
            {
                apply.response_code = "CatchError";
                apply.response_msg = "Please try again later";
            }

            return Json(apply, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult FormulaDelete(string ActionType, string id)
        {

            string output_msg = string.Empty;
            int ReturnCode = 0;
            _formula = new P_formulaBal();
            Msg _msg = new Msg();
            try
            {
                output_msg = _formula.Delete(ActionType, id, ref ReturnCode);
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