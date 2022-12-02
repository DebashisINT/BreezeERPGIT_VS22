using Payroll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Payroll.Repostiory.payrollLeaveStructure
{
    public interface ILeaveStructureLogic
    {
        void LeaveStructureModify(LeaveStructureEngine model, ref int strIsComplete, ref string strMessage, ref string StructureID);
        void LeaveDefinationModify(LeaveStructureEngine model, ref int strIsComplete, ref string strMessage);
        string Delete(string ActionType, string id, ref int ReturnCode);

        LeaveStructureEngine GetLeaveStructureDefination(string LeaveStructureID);
        LeaveStructureEngine EditLeaveDefination(string LeaveStructureID, string LeaveId, ref int strIsComplete, ref string strMessage);

    }
}