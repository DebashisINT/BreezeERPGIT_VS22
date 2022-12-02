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
    public class RoleMasterBL
    {
        public DataSet DropDownDetailForRole()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("PRC_PMSROLEMASTER_MASTERVVALUE");
            ds = proc.GetDataSet();
            return ds;
        }

        public int InsertRoleMaster(string ROLE_ID, string ROLE_NAME, string DESCRIPTION, string BILLING_TYPE, string UNIT, string SKILL_CATEGORY, string SKILL_SET, string CREATE_BY, string UPDATE_BY)
        {
            int ret = 0;
            ProcedureExecute proc = new ProcedureExecute("PRC_PMSROLEMASTER_INSERTUPDATE");
            proc.AddNVarcharPara("@ROLE_ID",10, ROLE_ID);
            proc.AddNVarcharPara("@ROLE_NAME",100, ROLE_NAME);
            proc.AddNVarcharPara("@DESCRIPTION", 500, DESCRIPTION);
            proc.AddNVarcharPara("@BILLING_TYPE",10, BILLING_TYPE);
            proc.AddNVarcharPara("@UNIT",10, UNIT);
            proc.AddNVarcharPara("@SKILL_CATEGORY",10, SKILL_CATEGORY);
            proc.AddNVarcharPara("@SKILL_SET", 100, SKILL_SET);
            proc.AddNVarcharPara("@CREATE_BY", 10, Convert.ToString(HttpContext.Current.Session["userid"]));
            proc.AddNVarcharPara("@UPDATE_BY", 10, Convert.ToString(HttpContext.Current.Session["userid"]));

            ret = proc.RunActionQuery();
            return ret;
        }

        public DataTable GetRoleMasterList()
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_PMSROLEMASTER_VIEW");
            ds = proc.GetTable();
            return ds;
        }

        public DataTable ViewRoleMaster(string ROLE_ID)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_PMSROLEMASTER_EDIT");
            proc.AddNVarcharPara("@ROLE_ID", 10, ROLE_ID);
            ds = proc.GetTable();
            return ds;
        }

        public void DeleteRoleMaster(string ROLE_ID, ref String ReturnMsg)
        {
            int ret = 0;
            ProcedureExecute proc = new ProcedureExecute("PRC_PMSROLEMASTER_DELETE");
            proc.AddNVarcharPara("@ROLE_ID", 10, ROLE_ID);
            proc.AddNVarcharPara("@ReturnMsg", 500, ReturnMsg, QueryParameterDirection.Output); 
            ret = proc.RunActionQuery();
            ReturnMsg = Convert.ToString(proc.GetParaValue("@ReturnMsg"));
            //return ret;
        }

        public DataTable ViewSkillSetr(string skillID)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_PMS_SKILLSETVIEW");
            proc.AddNVarcharPara("@SkillMaster_ID", 10, skillID);
            ds = proc.GetTable();
            return ds;
        }
    }
}
