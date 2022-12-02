using Payroll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Payroll.Repostiory.payrollClassGeneration
{
    public interface IClassGenrationLogic
    {
        void save(PClassGenerationEngine model, ref int ReturnCode, ref string ReturnMsg);
        string Delete(string ActionType, string id, ref int ReturnCode);


        PClassGenerationEngine GetClassById(string PClassId, ref string IsGenerate, ref string GenerateLastDate);
    }
}