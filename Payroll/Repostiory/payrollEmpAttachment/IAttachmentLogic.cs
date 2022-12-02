using Payroll.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Payroll.Repostiory.payrollEmpAttachment
{
    public interface IAttachmentLogic
    {
        void AttachmentModify(EmployeeAttachmentEngine model, DataTable dt, int SundayCount, int MondayCount, int TuesdayCount, int WednesdayCount, int ThursdayCount,
            int FridayCount, int SaturdayCount, ref int strIsComplete, ref string strMessage);
        void AttachmentUpdate(EmployeeAttachmentEngine model, ref int strIsComplete, ref string strMessage);
        void DeleteStructure(string ID, ref int strIsComplete, ref string strMessage);

        DataTable EditAttachment(string AttachmentID);
    }
}