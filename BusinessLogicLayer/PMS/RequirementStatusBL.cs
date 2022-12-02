using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BusinessLogicLayer.PMS
{
    public class RequirementStatusBL
    {
        public DataSet DropDownDetailForRole()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("PRC_PMSROLEMASTER_MASTERVVALUE");
            ds = proc.GetDataSet();
            return ds;
        }

        public DataTable GetRequirementList()
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_REQUIREMENTSTATUS_LIST");
            ds = proc.GetTable();
            return ds;
        }


        public DataTable ViewRequirement(string ReqID)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_REQUIREMENTSTATUS_VIEW");
            proc.AddNVarcharPara("@ReqID", 10, ReqID);
            ds = proc.GetTable();
            return ds;
        }

        public int DeleteRequirement(string ReqID)
        {
            int ret = 0;
            ProcedureExecute proc = new ProcedureExecute("PRC_REQUIREMENTSTATUS_DELETE");
            proc.AddNVarcharPara("@ReqID", 10, ReqID);
            ret = proc.RunActionQuery();
            return ret;
        }


        public DataTable InsertRequirement(string ReqID, string ReqName, string ReqStatus, string Branch)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_REQUIREMENTSTATUS_ADDEDIT");
            proc.AddNVarcharPara("@ReqID", 10, ReqID);
            proc.AddNVarcharPara("@ReqName", 100, ReqName);
            proc.AddNVarcharPara("@ReqStatus", 500, ReqStatus);
            proc.AddNVarcharPara("@Branch", 10, Branch);
            proc.AddNVarcharPara("@CREATE_BY", 10, Convert.ToString(HttpContext.Current.Session["userid"]));
            proc.AddNVarcharPara("@UPDATE_BY", 10, Convert.ToString(HttpContext.Current.Session["userid"]));

            ds = proc.GetTable();
            return ds;
        }
    }
}
