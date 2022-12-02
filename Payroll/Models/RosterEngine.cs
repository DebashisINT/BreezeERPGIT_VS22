using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Payroll.Models
{
    public class Roster
    {
        string ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string RosterCode { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string RosterID { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string RosterName { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string _PClassId { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string _PeriodID { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string _WeekDay { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string _ShiftID { get; set; }

        public List<RosterDetails> dtls { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ResponseCode { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string _Shift_Mon_ID { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string _Shift_Tue_ID { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string _Shift_Wed_ID { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string _Shift_Thur_ID { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string _Shift_Fri_ID { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string _Shift_Sat_ID { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string _Shift_Sun_ID { get; set; }


        public string Consider_AttendanceAfter { get; set; }
        public Boolean AdvanceShiftRules { get; set; }

        public Boolean IsInTimeAfter { get; set; }

        public String InTimeAfter { get; set; }

        public Boolean IsOutTimeAfter { get; set; }

        public String OutTimeAfter { get; set; }
        public Boolean IsInOutTimeDiff { get; set; }
        public String InOutTimeDiff { get; set; }

        public Boolean IsDeductLeave { get; set; }

        public String LateDayCount { get; set; }

        public String LateRuleCount { get; set; }

        public String LateRuleAfterCount { get; set; }

        public Boolean ShiftRule { get; set; }


        public List<EarlyLeaving> EarlyLeavingRules { get; set; }

        public List<LateRule> LateRules { get; set; }

        public List<HalfDayINOUT> HalfDayINOUTRules { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ResponseMessage { get; set; }
        public List<SelectListItem> _PClassName { get; set; }

        public List<SelectListItem> _PeriodName { get; set; }

        public List<SelectListItem> _ShiftName { get; set; }

        public List<SelectListItem> PopulateClassName()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConnectionString);
            DataTable DT = objEngine.GetDataTable(" proll_PayrollClass_Master ", " PayrollClassID,PayrollClassName ", "IsDeleted='N'");
            if (DT != null && DT.Rows.Count > 0)
            {
                foreach (DataRow row in DT.Rows)
                {
                    items.Add(new SelectListItem
                    {
                        Text = row["PayrollClassName"].ToString(),
                        Value = row["PayrollClassID"].ToString()
                    });
                }
            }

            items.Insert(0, new SelectListItem { Text = "--Select--", Value = "" });
            return items;
        }

        public List<SelectListItem> PopulatePeriodName(String payClassId,String Action="")
        {
            DataTable dt=new DataTable();
            List<SelectListItem> items = new List<SelectListItem>();
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConnectionString);
          
           
                 dt = objEngine.GetDataTable(" Select YYMM,Period From proll_PeriodGeneration Where PayrollClassID='" + payClassId + "'");
            
           
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    items.Add(new SelectListItem
                    {
                        Text = row["Period"].ToString(),
                        Value = row["YYMM"].ToString()
                    });
                }
            }

            return items;
        }

        public List<SelectListItem> PopulateShiftName()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConnectionString);
            //DataTable dt = objEngine.GetDataTable("select 'WO' 'ShiftID','Weekly Off' 'ShiftName' UNION ALL select ShiftID, ShiftName from proll_ShiftMaster");
            DataTable dt = objEngine.GetDataTable("SELECT * FROM ( select 'WO' 'ShiftID','Weekly Off' 'ShiftName',0 'RotationalShiftID' UNION ALL select SM.ShiftID,SM.ShiftName,0 'RotationalShiftID' from proll_ShiftMaster sm WHERE NOT EXISTS(SELECT ShiftName FROM proll_RotationalShiftMaster WHERE SM.ShiftName=ShiftName) union all select SM.ShiftID, (SM.ShiftName + ISNULL(' - '+RSM.ShiftName,'')) AS 'ShiftName',RSM.RotationalShiftID  from proll_ShiftMaster SM  INNER JOIN proll_RotationalShiftMaster RSM ON SM.ShiftID = RSM.ShiftID WHERE RSM.ShiftName IS NOT NULL ) AA ORDER BY ShiftName");
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    items.Add(new SelectListItem
                    {
                        Text = row["ShiftName"].ToString(),
                        Value = row["ShiftID"].ToString()  + (row["RotationalShiftID"].ToString() == "0" ? "" : "|" + row["RotationalShiftID"].ToString())
                        
                    });
                }
            }

            return items;
        }
    }

    public class RosterDetails
    {
        public string Day { get; set; }
        public string ShiftId { get; set; }
    }

    public class RosterModify
    {
        public Boolean Success { get; set; }
        public String Message { get; set; }
    }
    
}