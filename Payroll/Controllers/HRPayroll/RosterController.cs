using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Payroll.Models;
using System.Data;
using UtilityLayer;
using Payroll.Repostiory.RosterMaster;
using System.Reflection;

namespace Payroll.Controllers.HRPayroll
{
    [Payroll.Models.Attributes.SessionTimeout]
    public class RosterController : Controller
    {
        IRosterLogic objIRosterMasterLogic;

        // GET: Roster
        public ActionResult Dashboard()
        {
            return View("~/Views/HRPayroll/Roster/Dashboard.cshtml");
        }


        public PartialViewResult PartialRosterGrid()
        {
            return PartialView("~/Views/HRPayroll/Roster/PartialRosterGrid.cshtml", GetRosterList());
        }
        public IEnumerable GetRosterList()
        {
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            Payroll.Models.DataContext.PayRollDataClassDataContext dc = new Payroll.Models.DataContext.PayRollDataClassDataContext(connectionString);
            var q = from d in dc.v_proll_RosterMasterLists
                    orderby d.RosterID descending
                    select d;
            return q;
        }


        [HttpPost]
        public JsonResult GetPeriod(string classId)
        {
            Roster objModel = new Roster();
            objModel._PeriodName = objModel.PopulatePeriodName(classId);
            return Json(objModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index(string ActionType, string RosterId = "")
        {
            Roster objModel = new Roster();
            objIRosterMasterLogic = new RosterLogic();
            if (ActionType == "ADD" && RosterId == "")
            {
                objModel._PeriodName = objModel.PopulatePeriodName("");
                ViewBag.mode = "Roster-Add";
            }
            else if (ActionType == "EDIT" && RosterId != "")
            {


                objModel = objIRosterMasterLogic.getRosterDetailsById(RosterId);
                string ClassId = objModel._PClassId.ToString();
                string rosterType = objModel._WeekDay.ToString();
                if (rosterType == "W")
                {
                    objModel._PeriodName = objModel.PopulatePeriodName("");
                }

                else
                {
                    objModel._PeriodName = objModel.PopulatePeriodName(ClassId);
                }


                ViewBag.mode = "Roster-Edit";

            }
            else if (ActionType == "Copy" && RosterId != "")
            {
                objModel = objIRosterMasterLogic.getRosterDetailsById(RosterId);
                string ClassId = objModel._PClassId.ToString();
                string rosterType = objModel._WeekDay.ToString();
                objModel.RosterCode = "";
                //objModel.RosterID="";
                objModel.RosterName = "";
                objModel._PClassId="";
                objModel._PeriodName = objModel.PopulatePeriodName("");
                ViewBag.mode = "RosterCopy";

            }
            objModel._PClassName = objModel.PopulateClassName();
            // objModel._PeriodName = objModel.PopulatePeriodName("");
            objModel._ShiftName = objModel.PopulateShiftName();
            return View("~/Views/HRPayroll/Roster/Index.cshtml", objModel);

        }
        [HttpPost]
        public JsonResult Apply(Roster RosterHeaderDetails)
        {
            string output_msg = string.Empty;
            string tblformulaid = string.Empty;
            int strIsComplete = 0;
            string strMessage = string.Empty;
            objIRosterMasterLogic = new RosterLogic();
            try
            {

                DataTable dtLateRule = new DataTable();
                if (RosterHeaderDetails.LateRules != null)
                {
                    dtLateRule = ToDataTable(RosterHeaderDetails.LateRules);
                }

                DataTable dtEarlyLeavingRule = new DataTable();
                if (RosterHeaderDetails.EarlyLeavingRules != null)
                {
                    dtEarlyLeavingRule = ToDataTable(RosterHeaderDetails.EarlyLeavingRules);
                }

                DataTable dtHalfDayINOUTRules = new DataTable();
                if (RosterHeaderDetails.HalfDayINOUTRules != null)
                {
                    dtHalfDayINOUTRules = ToDataTable(RosterHeaderDetails.HalfDayINOUTRules);
                }

                //objIRosterMasterLogic.save(RosterHeaderDetails, dtLateRule, dtEarlyLeavingRule, ref strIsComplete, ref strMessage);
                objIRosterMasterLogic.save(RosterHeaderDetails, dtLateRule, dtEarlyLeavingRule,dtHalfDayINOUTRules, ref strIsComplete, ref strMessage);
                if (strMessage == "Success" && strIsComplete == 1)
                {
                    RosterHeaderDetails.ResponseCode = "Success";
                    RosterHeaderDetails.ResponseMessage = "Success";


                }
                else if (strMessage != "Success" && strIsComplete == -1)
                {
                    RosterHeaderDetails.ResponseCode = "Error";
                    RosterHeaderDetails.ResponseMessage = strMessage;
                }
                else
                {
                    RosterHeaderDetails.ResponseCode = "Error";
                    RosterHeaderDetails.ResponseMessage = "Please try again later";
                }

            }

            catch (Exception ex)
            {
                RosterHeaderDetails.ResponseCode = "CatchError";
                RosterHeaderDetails.ResponseMessage = "Please try again later";
            }

            return Json(RosterHeaderDetails, JsonRequestBehavior.AllowGet);
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

        public JsonResult DeleteRoster(String ActionType, String id)
        {
            RosterModify objModel = new RosterModify();
            objIRosterMasterLogic = new RosterLogic();
            try
            {
                objModel = objIRosterMasterLogic.RosterActionByID("DeleteRoster", id);
            }
            catch { }
            return Json(objModel, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LeavingLateShiftByID(string id)
        {
            ShiftMasterEngine objModel = new ShiftMasterEngine();
            int strIsComplete = 0;
            string strMessage = "";
            objIRosterMasterLogic = new RosterLogic();
            objModel = objIRosterMasterLogic.LeavingLateShiftByID(id, ref strIsComplete, ref strMessage);
            return Json(objModel);
        }
    }
}