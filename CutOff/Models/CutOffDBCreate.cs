using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataAccessLayer;
using System.Data;

namespace CutOff.Models
{
    public class CutOffDBCreate
    {
        string ConnectionString = String.Empty;
        public CutOffDBCreate()
        {
            ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        public DataSet InsertCompany(string dbnm)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("USP_CREATECOMPANY");
            proc.AddVarcharPara("@DBNAME", 150, dbnm);
            ds = proc.GetDataSet();
            return ds;
        }
        public DataSet DropTBLSchema()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("PRC_DROPDBTABLE");
            ds = proc.GetDataSet();
            return ds;
        }
        public DataSet CreateTBLSchema()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("PRC_ALLCREATETABLE");
            ds = proc.GetDataSet();
            return ds;
        }
        public DataTable GetDBType(string dbnm)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Usp_SelectDbType");
            proc.AddVarcharPara("@DBNAME", 150, dbnm);
            dt = proc.GetTable();
            return dt;
        }
        public DataSet InsertFinancialYear()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("USP_CREATEFINANCIALYEAR");
            ds = proc.GetDataSet();
            return ds;
        }
    }
}