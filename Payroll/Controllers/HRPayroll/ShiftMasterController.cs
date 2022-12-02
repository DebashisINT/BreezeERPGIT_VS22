using Payroll.Models;
using Payroll.Repostiory.ShiftMaster;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Payroll.Controllers
{
    [Payroll.Models.Attributes.SessionTimeout]
    public class ShiftMasterController : Controller
    {
        IShiftMasterLogic objIShiftMasterLogic;
        ShiftMasterEngine objShiftMasterEngine = new ShiftMasterEngine();
        public ActionResult Dashboard()
        {
            return View("~/Views/HRPayroll/ShiftMaster/Dashboard.cshtml");
        }

        public ActionResult PartialShiftEntry()
        {
            ShiftMasterEngine model = new ShiftMasterEngine();
            return PartialView("~/Views/HRPayroll/ShiftMaster/PartialShiftEntry.cshtml", model);
        }

        public ActionResult PartialShiftByID(string ShiftId)
        {
            ShiftMasterEngine model = new ShiftMasterEngine();
            if (ShiftId != "")
            {
                int strIsComplete = 0;
                string strMessage = "";

                objIShiftMasterLogic = new ShiftMasterLogic();
                model = objIShiftMasterLogic.GetShiftById(ShiftId, ref strIsComplete, ref strMessage);
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
            return PartialView("~/Views/HRPayroll/ShiftMaster/PartialShiftEntry.cshtml", model);
        }


        public PartialViewResult PartialShiftGrid()
        {
            return PartialView("~/Views/HRPayroll/ShiftMaster/PartialShiftGrid.cshtml", GetShiftList());
        }
        public IEnumerable GetShiftList()
        {
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            Payroll.Models.DataContext.PayRollDataClassDataContext dc = new Payroll.Models.DataContext.PayRollDataClassDataContext(connectionString);
            var q = from d in dc.v_proll_ShiftLists
                    orderby d.ShiftID descending
                    select d;
            return q;
        }




        [HttpPost]
        public JsonResult ShiftMasterSubmit(ShiftMasterEngine model)
        {
            if (Convert.ToString(model.Shift_Code) == "")
            {
                model.ResponseCode = "Error";
                model.ResponseMessage = "Shift Code  is mandatory";
            }
            else if (Convert.ToString(model.Shift_Name) == "")
            {
                model.ResponseCode = "Error";
                model.ResponseMessage = "Shift Name is mandatory";
            }
            else if (Convert.ToString(model.Shift_Start) == "" && model.Shift_Type == "0")
            {
                model.ResponseCode = "Error";
                model.ResponseMessage = "Shift Start is mandatory";
            }
            else if (Convert.ToString(model.Shift_End) == "" && model.Shift_Type == "0")
            {
                model.ResponseCode = "Error";
                model.ResponseMessage = "Shift End is mandatory";
            }
            else
            {
                int strIsComplete = 0;
                string strMessage = "";

                DataTable dtLateRule = new DataTable();
                if (model.LateRules != null)
                {
                    dtLateRule = ToDataTable(model.LateRules);
                }

                DataTable dtEarlyLeavingRule = new DataTable();
                if (model.EarlyLeavingRules != null)
                {
                    dtEarlyLeavingRule = ToDataTable(model.EarlyLeavingRules);
                }

                DataTable dtRotationalShiftRule = new DataTable();
                if (model.RotationalShift != null)
                {
                    if (model.RotationalShift.Count > 0)
                    {
                        dtRotationalShiftRule = ToDataTable(model.RotationalShift);
                    }
                }

                objIShiftMasterLogic = new ShiftMasterLogic();
                objIShiftMasterLogic.ShiftMasterSubmit(model, dtLateRule, dtEarlyLeavingRule,dtRotationalShiftRule, ref strIsComplete, ref strMessage);
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

        public DataTable ToDataTable<T>(List<T> items)
        {

            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties

            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in Props)
            {

                //Setting column names as Property names

                dataTable.Columns.Add(prop.Name);

            }

            foreach (T item in items)
            {

                var values = new object[Props.Length];

                for (int i = 0; i < Props.Length; i++)
                {

                    //inserting property values to datatable rows

                    values[i] = Props[i].GetValue(item, null);

                }

                dataTable.Rows.Add(values);

            }

            //put a breakpoint here and check datatable

            return dataTable;

        }

        [HttpPost]
        public JsonResult ShiftDelete(string ActionType, string id)
        {

            string output_msg = string.Empty;
            int strIsComplete = 0;
            objIShiftMasterLogic = new ShiftMasterLogic(); ;
            Msg _msg = new Msg();
            try
            {
                output_msg = objIShiftMasterLogic.Delete(ActionType, id, ref strIsComplete);
                if (output_msg == "Success" && strIsComplete == 1)
                {
                    _msg.response_code = "Success";
                    _msg.response_msg = "Success";
                }
                else if (output_msg != "Success" && strIsComplete == -1)
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

        public JsonResult LeavingLateShiftByID(string id)
        {
            ShiftMasterEngine objModel = new ShiftMasterEngine();
            int strIsComplete = 0;
            string strMessage = "";
            objIShiftMasterLogic = new ShiftMasterLogic();
            objModel = objIShiftMasterLogic.LeavingLateShiftByID(id, ref strIsComplete, ref strMessage);
            return Json(objModel);
        }

        public JsonResult RotationalShiftShiftByID(string id)
        {
            ShiftMasterEngine objModel = new ShiftMasterEngine();
            int strIsComplete = 0;
            string strMessage = "";
            objIShiftMasterLogic = new ShiftMasterLogic();
            objModel = objIShiftMasterLogic.RotationalShiftShiftByID(id, ref strIsComplete, ref strMessage);
            return Json(objModel);
        }

    }
}