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
        public DataSet DropTBLSchema(string user_id, String _NewDBName)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("PRC_DROPDBTABLE");
            proc.AddVarcharPara("@user_id", 150, user_id);
            proc.AddVarcharPara("@NewDBName", 150, _NewDBName);
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
        // Rev Sanchita
        public DataTable GetSQLDataLocation()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Usp_SelectDbType");
            proc.AddVarcharPara("@Action", 150, "GetSQLDataLocation");
            dt = proc.GetTable();
            return dt;
        }
        // End of Rev Sanchita
    }
}