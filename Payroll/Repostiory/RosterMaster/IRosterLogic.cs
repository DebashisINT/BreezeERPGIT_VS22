using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Payroll.Models;
using System.Data;

namespace Payroll.Repostiory.RosterMaster
{
    public interface IRosterLogic
    {
        //void save(Roster RosterHeaderDetails,DataTable dtLateRule, DataTable dtEarlyLeavingRule, ref int strIsComplete, ref string strMessage);
        void save(Roster RosterHeaderDetails, DataTable dtLateRule, DataTable dtEarlyLeavingRule, DataTable dtHalfDayINOUTRules, ref int strIsComplete, ref string strMessage);
        Roster getRosterDetailsById(string RosterId);

        RosterModify RosterActionByID(String ActionType, String ID);

        ShiftMasterEngine LeavingLateShiftByID(String ID, ref int strIsComplete, ref string strMessage);
    }
}