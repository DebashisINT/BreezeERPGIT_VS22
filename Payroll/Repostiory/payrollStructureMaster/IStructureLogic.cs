using Payroll.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Payroll.Repostiory.payrollStructureMaster
{
    public interface IStructureLogic
    {
        void StructureModify(payrollStructureEngine model, ref int strIsComplete, ref string strMessage, ref string StructureID);
        void DeleteStructure(payrollStructureEngine model, ref int strIsComplete, ref string strMessage);
        void PayheadSaveModify(payrollStructureEngine model, ref int strIsComplete, ref string strMessage, ref string StructureID,ref DataTable dt);
        DataTable PopulatePayHead(string strStructureID);
        DataTable CheckFormula(string strFormula);
        DataTable CheckFormula(string strStructureID, string strFormula);
        DataSet GetStructureDetails(string strStructureID);
        DataSet GetOnceEmployeeHOPDetails(string strStructureID, string CalculationType);
        DataSet GetAlwaysEmployeeHOPDetails(string strStructureID, string CalculationType);
        void SavePayrollDetails(string Type,DataTable dt, ref int strIsComplete, ref string strMessage);
        string DeletePayHead(string ActionType, string id, ref int ReturnCode, string StructureID, ref DataTable dt);
        DataTable EditPayHead(string PayHeadID);
        void SaveReportWidgets(string StructureID, DataTable Allowance_dt, DataTable Deduction_dt, DataTable Others_dt, ref int strIsComplete, ref string strMessage);

        DataSet GetImportPayHead(DataTable dt, Int64 UserID, String PayHeadID, String StructureID);

        void SaveImportPayrollDetails(String Type, String PayStructureID, DataTable dt, ref int strIsComplete, ref string strMessage);

        DataSet SaveMappingExcel(String Action, DataTable dt, String SettingName);

    }
}