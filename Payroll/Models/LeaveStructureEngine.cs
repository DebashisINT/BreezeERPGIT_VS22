using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Payroll.Models
{
    public class LeaveStructureEngine
    {
        string ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ActionType { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string StructureID { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string StructureHeaderName { get; set; }

        
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string StructureName { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string StructureCode { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string LeaveID { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string LeaveName { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string LeaveCode { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ApplicableForCode { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public bool is_IncludePublicHoliday { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public bool is_WeeklyOff { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string LeaveTypeCode { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string HolidayRuleCode { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public bool is_onconfirmation { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public bool is_noticeperiod { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public bool is_probationperiod { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public bool is_periodbasis { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public bool is_weeklyoffperperiod { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public bool is_publicholidayinperiod { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public String WeeklyOffDays { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public bool is_carryforwardtonextperiod { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public bool is_encashable { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public bool is_adjstleave { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public bool is_chkEntitlement { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ddlEntitlementBase { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public bool is_carryforward { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public bool is_lengthofservice { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string LeaveDaysAdd { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string DaysPer { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string DaysForMaximum { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Years { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ServiceYears { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ServiceMonths { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ServiceDays { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ResponseCode { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ResponseMessage { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string BasicCode { get; set; }
        public string Unavailed { get; set; }
        public string _PHoliday { get; set; }
        public string _SamePeriod { get; set; }
        public string _EndLeavePeriod { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }



        public bool chkFixed { get; set; }
        public string txtNumber { get; set; }
        public bool ChkProrataBasis { get; set; }
        public string dtCredited { get; set; }
        public bool chkNotCrWeekoffDays { get; set; }
        public bool chkNotCrPublicHolidays { get; set; }
        public bool chkEntitled { get; set; }
        public string ddlEntitled { get; set; }
        public bool chkEligeableAvail { get; set; }
        public string ddlEligeableAvail { get; set; }
        public bool chkApplyFor { get; set; }
        public string txtApplyForMin { get; set; }
        public string txtApplyForMax { get; set; }
        public bool chkFixdCF { get; set; }
        public string ddlCarryUpto { get; set; }
        public bool chkMaximumBal { get; set; }
        public string txtMaximumNumber { get; set; }
        public bool chkAllowLeaveZeroBalance { get; set; }
        public string txtUptoDays { get; set; }
        public string txtUptoMonths { get; set; }
        public string txtUptoYears { get; set; }
        public bool FixedEncashable { get; set; }
        public string ddlEncashableAt { get; set; }
        public string ddlEncashableBalance { get; set; }
        public bool ConsiderBeforeCF { get; set; }
        public bool ConjuncAllowedOtherLeaves { get; set; }
        public bool chkAdjustableOtherLeaves { get; set; }













        public List<SelectListItem> _PHoliDayBind { get; set; }
        public List<SelectListItem> ApplicableForList { get; set; }
        public List<SelectListItem> LeaveTypeList { get; set; }
        public List<SelectListItem> HolidayCheckList { get; set; }
        public List<SelectListItem> HolidayRuleList { get; set; }
        public List<SelectListItem> EligibilityTypeList { get; set; }
        public List<SelectListItem> WeeklyOffDaysList { get; set; }
        public List<SelectListItem> BasicList { get; set; }
        public List<SelectListItem> _NumberBind { get; set; }
        public List<SelectListItem> _PayHeadBind { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public bool is_AutoAdjusted { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public bool is_AdjustLeaveApproval { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public bool is_AdjustLeaveConsider { get; set; }
        

        public List<SelectListItem> PopulateLeaveDropdown(string RID)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConnectionString);
            DataTable DT = objEngine.GetDataTable(" proll_Main_Master ", " CODE,[DESC] ", " RID='" + RID + "' ");
            if (DT != null && DT.Rows.Count > 0)
            {
                foreach (DataRow row in DT.Rows)
                {
                    items.Add(new SelectListItem
                    {
                        Text = row["DESC"].ToString(),
                        Value = row["CODE"].ToString()
                    });
                }
            }

            return items;
        }

        public List<SelectListItem> PopulateWeeklyOffDays()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            SelectListItem obj = new SelectListItem();
            obj.Text = "Monday";
            obj.Value = "Monday";
            items.Add(obj);
            obj = new SelectListItem();
            obj.Text = "Tuesday";
            obj.Value = "Tuesday";
            items.Add(obj);
            obj = new SelectListItem();
            obj.Text = "Wednesday";
            obj.Value = "Wednesday";
            items.Add(obj);
            obj = new SelectListItem();
            obj.Text = "Thursday";
            obj.Value = "Thursday";
            items.Add(obj);
            obj = new SelectListItem();
            obj.Text = "Friday";
            obj.Value = "Friday";
            items.Add(obj);
            obj = new SelectListItem();
            obj.Text = "Saturday";
            obj.Value = "Saturday";
            items.Add(obj);
            obj = new SelectListItem();
            obj.Text = "Sunday";
            obj.Value = "Sunday";
            items.Add(obj);            
            return items;
        }

        public List<SelectListItem> PopulateHoliDay()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConnectionString);

            DataTable DT = objEngine.GetDataTable("Select '0' as HId,'Select' as HDesc Union All select HOLIDAYID as HId,HOLIDAY_DESC as HDesc from ERP_HOLIDAYMAIN");
            if (DT != null && DT.Rows.Count > 0)
            {
                foreach (DataRow row in DT.Rows)
                {
                    items.Add(new SelectListItem
                    {
                        Text = row["HDesc"].ToString(),
                        Value = row["HId"].ToString()
                    });
                }
            }

            return items;
        }

        public List<SelectListItem> PopulateNumber()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConnectionString);

            DataTable DT = objEngine.GetDataTable("Select 0 as NumberId,'Select' as NumberValue Union All select NumberId,NumberValue from Number ");
            if (DT != null && DT.Rows.Count > 0)
            {
                foreach (DataRow row in DT.Rows)
                {
                    items.Add(new SelectListItem
                    {
                        Text = row["NumberValue"].ToString(),
                        Value = row["NumberId"].ToString()
                    });
                }
            }

            return items;
        }

        public List<SelectListItem> PopulatePayHead()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConnectionString);

            DataTable DT = objEngine.GetDataTable("Select '0' as PayHeadID,'Select' as PayHeadName Union All select PayHeadID,PayHeadName from proll_PayHeadMaster ");
            if (DT != null && DT.Rows.Count > 0)
            {
                foreach (DataRow row in DT.Rows)
                {
                    items.Add(new SelectListItem
                    {
                        Text = row["PayHeadName"].ToString(),
                        Value = row["PayHeadID"].ToString()
                    });
                }
            }

            return items;
        }

    }
}