using Payroll.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Payroll.Repostiory.ShiftMaster
{
    public interface IShiftMasterLogic
    {
        void ShiftMasterSubmit(ShiftMasterEngine model, DataTable dtLateRule, DataTable dtEarlyLeavingRule,DataTable dtRotationalShiftRule, ref int strIsComplete, ref string strMessage);

        ShiftMasterEngine GetShiftById(String ShiftId, ref int strIsComplete, ref string strMessage);
        string Delete(string ActionType, string id, ref int strIsComplete);

        ShiftMasterEngine LeavingLateShiftByID(String ShiftId, ref int strIsComplete, ref string strMessage);


        ShiftMasterEngine RotationalShiftShiftByID(String ShiftId, ref int strIsComplete, ref string strMessage);
    }
}