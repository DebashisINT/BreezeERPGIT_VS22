using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Payroll.Models;
using Payroll.Repostiory.P_Formula;

namespace Payroll.Controllers
{
    public class p_formulaController : Controller
    {
        // GET: p_formula
        private IPayrole_formula _formula;
        public ActionResult p_FormulaAddEdit()
        {
            return View();
        }
        public JsonResult Apply(FormulaApply apply)
        {
            string output_msg = string.Empty;
            _formula = new P_formulaBal();
            try
            {
                output_msg = _formula.save(apply);
                if(output_msg=="true")
                {
                    apply.response_code = "Success";
                    apply.response_msg = "Success";
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

            return Json(apply,JsonRequestBehavior.AllowGet);
        }
    }
}