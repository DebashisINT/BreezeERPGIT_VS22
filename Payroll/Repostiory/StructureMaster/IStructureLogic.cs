using Payroll.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Payroll.Repostiory.StructureMaster
{
    public interface IStructureLogic
    {
        void StructureModify(PayStructureEngine model, ref int strIsComplete, ref string strMessage, ref string StructureID);
        void DeleteStructure(PayStructureEngine model, ref int strIsComplete, ref string strMessage);
        void PayheadSaveModify(PayStructureEngine model, ref int strIsComplete, ref string strMessage, ref string StructureID,ref DataTable dt);
        DataTable PopulatePayHead(string strStructureID);
        DataTable CheckFormula(string strFormula);
        DataSet GetStructureDetails(string strStructureID);
    }
}