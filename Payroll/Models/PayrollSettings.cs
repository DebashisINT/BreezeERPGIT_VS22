using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Payroll.Models
{
    public class PayrollSettings
    {
        public string PayHeadId { get; set; }
        public List<Payheads> Payheads { get; set; }

        public string MainAccountCode { get; set; }
        public List<MainAccount> MainAccounts { get; set; }



        internal DataSet GetAllData(string StructureID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("PRC_PAYROLLSETTINGS");
            proc.AddVarcharPara("@ACTION", 100, "GETALLDROPDOWN");
            proc.AddPara("@StructureID", StructureID);
            ds = proc.GetDataSet();
            return ds;
        }

        internal string Save(InputModal input)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("PRC_PAYROLLSETTINGS");
            proc.AddVarcharPara("@ACTION", 100, input.Action);
            proc.AddVarcharPara("@AccountCode", 100, input.AccountCode);
            proc.AddVarcharPara("@AccountMapCode", 100, input.AccountMapCode);
            proc.AddVarcharPara("@PostingType", 100, input.PostingType);
            proc.AddVarcharPara("@Subaacount", 100, input.Subaacount);
            proc.AddPara("@USER_ID", HttpContext.Current.Session["userid"]);
            proc.AddPara("@StructureID", HttpContext.Current.Session["StructureID"]);
            proc.AddPara("@Payheadids", input.Payheadids);
            ds = proc.GetDataSet();
            return "";
        }

        internal DataSet Edit(InputModal input)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("PRC_PAYROLLSETTINGS");
            proc.AddVarcharPara("@ACTION", 100, "EDIT");
            proc.AddPara("@AccountMapCode", input.AccountMapCode);
            ds = proc.GetDataSet();
            return ds;
        }

        internal string Delete(InputModal input)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("PRC_PAYROLLSETTINGS");
            proc.AddVarcharPara("@ACTION", 100, "Delete");
            proc.AddPara("@AccountMapCode", input.AccountMapCode);
            ds = proc.GetDataSet();
            return "";
        }
    }
    public class Payheads
    {
        public string PayHeadID { get; set; }
        public string PayHeadName { get; set; }

    }
    public class MainAccount
    {
        public string Code { get; set; }
        public string Name { get; set; }

    }

    public class NumberingSchema
    {
        public string ID { get; set; }
        public string SchemaName { get; set; }

    }

    public class InputModal
    {
        public string Action { get; set; }
        public string AccountMapCode { get; set; }
        public string AccountCode { get; set; }
        public string Subaacount { get; set; }
        public string PostingType { get; set; }
        public string Payheadids { get; set; }
    }
}