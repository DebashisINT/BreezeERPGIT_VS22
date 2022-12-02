using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Payroll.Repostiory.payrollFoundationMaster
{
    public interface IFoundationMaster
    {
        void save(string desc, string code,ref int ReturnCode, ref string ReturnMsg);
        string Delete(string ActionType, string code, ref int ReturnCode);
    }
}