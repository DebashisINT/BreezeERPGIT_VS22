using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Payroll.Models
{
    public class LeaveApplicationEngine
    {
        string ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string LeaveStructureID { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string EmployeeID { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string LeaveApplicationID { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string EmployeeName { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ResponseCode { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ResponseMessage { get; set; }

        public List<SelectListItem> _PLeaveType { get; set; }
        public string PLeaveID { get; set; }

        public List<SelectListItem> PopulateLeaveType(string EMPCODE, string _LeaveStructureCode)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            DataTable DT = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_PAYROLLSETTINGS");
            proc.AddVarcharPara("@ACTION", 100, "GETLEAVETYPE");
            proc.AddVarcharPara("@LEAVESTRUCTURECODE", 50, _LeaveStructureCode);
            proc.AddVarcharPara("@EMPLOYEECODE", 50, EMPCODE);
            DT = proc.GetTable();

            if (DT != null && DT.Rows.Count > 0)
            {
                foreach (DataRow row in DT.Rows)
                {
                    items.Add(new SelectListItem
                    {
                        Text = row["LeaveID"].ToString(),
                        Value = row["LeaveName"].ToString()

                    });
                }
            }
            return items;

        }

        /////newly added
        public Int64 USERID { get; set; }

        public String REPORTTYPE { get; set; }

        public Int64 SLNO { get; set; }

        public String EMPLOYEECODE { get; set; }

        public String EMPLOYEENAME { get; set; }

        public String LEAVENAME { get; set; }

        public DateTime LEV_DATE_FROM { get; set; }

        public DateTime LEV_DATE_TO { get; set; }

        public Int64 LEAVEDAYS { get; set; }

        public DateTime LEAVEAPPLIEDON { get; set; }

        public String STATUS { get; set; }

        public String USER_NAME { get; set; }
    }

    public class classLeaveApplication
    {
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ApplicationID { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string EmployeeID { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string LeaveStructureID { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string LeaveApplicationNo { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string LeaveApplicationDetails { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string LeaveID { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string DayPart { get; set; }
        public DateTime LeaveFromDate { get; set; }
        public DateTime LeaveToDate { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ApplyDays { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string LeaveReason { get; set; }
    }


    public class classLeaveApplicationEdit
    {
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ApplicationID { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string EmployeeID { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string LeaveStructureID { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string LeaveApplicationNo { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string LeaveApplicationDetails { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string LeaveID { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string DayPart { get; set; }
        public string LeaveFromDate { get; set; }
        public string LeaveToDate { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ApplyDays { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string LeaveReason { get; set; }
    }

    

}