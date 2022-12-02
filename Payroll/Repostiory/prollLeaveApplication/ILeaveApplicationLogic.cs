using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Payroll.Models;
using System.Data;

namespace Payroll.Repostiory.prollLeaveApplication
{
    public interface ILeaveApplicationLogic
    {
        void LeaveApplicationModify(DataTable dtLeaveDetails, ref int strIsComplete, ref string strMessage);

        void ApproveLeave(string LEAVE_IDS, ref int strIsComplete, ref string strMessage);

        void RejectLeave(string LEAVE_IDS, ref int strIsComplete, ref string strMessage);

         //Ref Bapi
        void LeaveApplicationEdit(List<classLeaveApplicationEdit> model, ref int strIsComplete, ref string strMessage);

        void DeleteLeaves(string DocID, ref int strIsComplete, ref string strMessage);

        //End Ref
    }
}