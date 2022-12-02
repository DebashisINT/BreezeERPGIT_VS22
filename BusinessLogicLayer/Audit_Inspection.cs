using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessLayer;
using System.Data;

namespace BusinessLogicLayer
{
    public class Audit_Inspection
    {
        public DataSet Inpersonverification(string done, string GRPTYPE, string Groupby, string CLIENTS)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Inpersonverification");

            proc.AddVarcharPara("@done", 5, done);
            proc.AddVarcharPara("@GRPTYPE", 15, GRPTYPE);
            proc.AddVarcharPara("@Groupby", -1, Groupby);
            proc.AddVarcharPara("@CLIENTS", -1, CLIENTS);
            ds = proc.GetDataSet();
            return ds;
        }
        public DataSet specialcategory(string Special)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("specialcategory");

            proc.AddVarcharPara("@Special", 20, Special);

            ds = proc.GetDataSet();
            return ds;
        }
        public DataSet Riskcategory(string Risk)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Riskcategory");

            proc.AddVarcharPara("@Risk", 20, Risk);

            ds = proc.GetDataSet();
            return ds;
        }
    }
}
