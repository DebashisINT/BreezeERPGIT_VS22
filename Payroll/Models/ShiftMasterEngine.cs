using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Payroll.Models
{
    public class ShiftMasterEngine
    {
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Shift_Code { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Shift_Id { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Shift_Name { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Shift_Start { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Shift_End { get; set; }

        //[DisplayFormat(ConvertEmptyStringToNull = false)]
        //public string Consider_LateAfter { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Consider_AttendanceAfter { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Consider_HalfDayAfter { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Break { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Consider_HalfDayBefore { get; set; }

         [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string OneLateAfter { get; set; }

         [DisplayFormat(ConvertEmptyStringToNull = false)]
         public string TwoLateAfter { get; set; }

         [DisplayFormat(ConvertEmptyStringToNull = false)]
         public string ThreeLateAfter { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ResponseCode { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ResponseMessage { get; set; }


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


        public List<EarlyLeaving> EarlyLeavingRules { get; set; }


        public List<HalfDayINOUT> HalfDayINOUTRules { get; set; }
        public List<LateRule> LateRules { get; set; }

        public String Shift_EndDay { get; set; }

        public String Shift_Type { get; set; }

        public String Shift_Time { get; set; }

        public List<RotationalShiftRule> RotationalShift { get; set; }

        public String Shift_Break_Time { get; set; }
        
    }

    public class EarlyLeaving
    {
        public string LeavingHours { get; set; }

        public int LeavingDays { get; set; }

        public string Condition { get; set; }
    }

    public class HalfDayINOUT
    {
        public string HalfDayInHours { get; set; }
        public string HalfDayOutHours { get; set; }
        public string AfterBefor1 { get; set; }
        public string AfterBefor2 { get; set; }
        public string slcondition1 { get; set; }
        public string slcondition2 { get; set; }        

       
    }
    public class RotationalShiftRule
    {
        public string RotationalShiftStart { get; set; }

        public string RotationalShiftEnd { get; set; }

        public string TimeDuration { get; set; }

        public string ShiftName { get; set; }
    }

    public class LateRule
    {
        public int LateCount { get; set; }

        public string InTimeAfter { get; set; }

    }

}