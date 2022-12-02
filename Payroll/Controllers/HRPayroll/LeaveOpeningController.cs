using Payroll.Models;
using Payroll.Repostiory.payrollLeaveOpening;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UtilityLayer;

namespace Payroll.Controllers.HRPayroll
{
    [Payroll.Models.Attributes.SessionTimeout]
    public class LeaveOpeningController : Controller
    {
        public ILeaveOpeningLogic objILeaveOpeningLogic;

        public ActionResult Dashboard()
        {
            return View("~/Views/HRPayroll/LeaveOpening/Dashboard.cshtml"); ;
        }
        public object GetOpeningBalanceList(string StructureID)
        {
            LeaveOpeningEngine model = new LeaveOpeningEngine();

            objILeaveOpeningLogic = new LeaveOpeningLogic();
            DataSet ds = objILeaveOpeningLogic.GetEmployeeLeaveOpening(StructureID);

            DataTable dt_Status = new DataTable();
            DataTable dt_LeaveDetailsList = new DataTable();
            DataTable dt_OpeningBalanceList = new DataTable();

            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0) dt_Status = ds.Tables[0];
            string ReturnValue = Convert.ToString(dt_Status.Rows[0]["ReturnValue"]);
            string ReturnMessage = Convert.ToString(dt_Status.Rows[0]["ReturnMessage"]);

            if (ReturnValue == "Success")
            {
                if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0) dt_LeaveDetailsList = ds.Tables[1];
                if (ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0) dt_OpeningBalanceList = ds.Tables[2];

                List<LeaveDetailsList> oLeaveDetailsList = new List<LeaveDetailsList>();
                List<dynamic> oOpeningBalanceList = new List<dynamic>();

                if (dt_LeaveDetailsList != null && dt_LeaveDetailsList.Rows.Count > 0)
                {
                    oLeaveDetailsList = APIHelperMethods.ToModelList<LeaveDetailsList>(dt_LeaveDetailsList);
                }

                if (dt_OpeningBalanceList != null && dt_OpeningBalanceList.Rows.Count > 0)
                {
                    oOpeningBalanceList = ToDynamicList(dt_OpeningBalanceList);
                }

                model.ResponseCode = "Success";
                model.LeaveDetailsList = oLeaveDetailsList;
                model.OpeningBalanceList = oOpeningBalanceList;
            }
            else
            {
                model.ResponseCode = "Error";
                model.ResponseMessage = ReturnMessage;
            }

            return Json(model);
        }
        public object SaveEmployeeOpeningBalance(LeaveHead TableDetails)
        {
            LeaveOpeningEngine model = new LeaveOpeningEngine();

            DataTable LeaveDetails_dt = new DataTable();
            LeaveDetails_dt.Columns.Add("EmployeeCode", typeof(string));
            LeaveDetails_dt.Columns.Add("LeaveID", typeof(string));
            LeaveDetails_dt.Columns.Add("Amount", typeof(string));
            LeaveDetails_dt.Columns.Add("Value", typeof(string));

            for (var i = 0; i < TableDetails.MainArray.Count; i++)
            {
                string EmployeeCode = "";

                for (var j = 0; j < TableDetails.MainArray[i].classLeaveHead.Count; j++)
                {
                    classLeaveHead objclassPayHead = new classLeaveHead();

                    objclassPayHead = TableDetails.MainArray[i].classLeaveHead[j];
                    string _Key = objclassPayHead.Keys;
                    string _Amount = objclassPayHead.Amount;
                    string _Values = objclassPayHead.Values;

                    if (_Key == "EmployeeCode") EmployeeCode = _Values;

                    if (_Key != "EmployeeCode" && _Key != "EmpName" && _Key != "Employee_Name")
                    {
                        LeaveDetails_dt.Rows.Add(EmployeeCode, _Key, _Amount, _Values);
                    }
                }
            }

            int strIsComplete = 0;
            string strMessage = "";

            objILeaveOpeningLogic = new LeaveOpeningLogic();
            objILeaveOpeningLogic.SaveOpeningBalance(LeaveDetails_dt, ref strIsComplete, ref strMessage);
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

            return Json(model);
        }
        public List<dynamic> ToDynamicList(DataTable dt)
        {
            var dynamicDt = new List<dynamic>();
            foreach (DataRow row in dt.Rows)
            {
                dynamic dyn = new ExpandoObject();
                dynamicDt.Add(dyn);
                foreach (DataColumn column in dt.Columns)
                {
                    var dic = (IDictionary<string, object>)dyn;
                    dic[column.ColumnName] = row[column];
                }
            }
            return dynamicDt;
        }
    }
}