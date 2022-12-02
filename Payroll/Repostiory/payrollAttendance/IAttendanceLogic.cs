using Payroll.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Payroll.Repostiory.payrollAttendance
{
    public interface IAttendanceLogic
    {

        DataSet GetEmployeeLeaveSummary(string strPayClassID, string strYYMM);
        DataSet SaveEmployeeLeaveSummary(string strPayClassID, string strYYMM);


        DataSet GetEmployeeAttendance(string strPayClassID, string strYYMM);
        void SaveAttendanceData(string PayClassID, string Period, DataTable dt, ref int strIsComplete, ref string strMessage);

        DataSet GetImportAttendance(DataTable dt, Int64 UserID, string payclassid, string periodid, String map);

        DataSet GetEmployeeAttendanceApproval(string PayClassID, string YYMM,string EmployeeID);

        string SaveApprovalData(Approval model);
    }
}