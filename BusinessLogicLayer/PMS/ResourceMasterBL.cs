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
   public class ResourceMasterBL
    {
       public DataSet DropDownDetailForResource()
       {
           DataSet ds = new DataSet();
           ProcedureExecute proc = new ProcedureExecute("PRC_RESOURCELOAD");
           ds = proc.GetDataSet();
           return ds;
       }

       public DataTable GetResourceList()
       {
           DataTable ds = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("PRC_RESOURCE_VIEW");
           ds = proc.GetTable();
           return ds;
       }

       public DataTable ViewResourceMaster(string RESOURCE_ID)
       {
           DataTable ds = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("PRC_VIEWRESOURCE");
           proc.AddNVarcharPara("@RESOURCE_ID", 10, RESOURCE_ID);
           ds = proc.GetTable();
           return ds;
       }

       public int DeleteResoureceMaster(string RESOURCE_ID)
       {
           int ret = 0;
           ProcedureExecute proc = new ProcedureExecute("PRC_DELETERESOURCE");
           proc.AddNVarcharPara("@RESOURCE_ID", 10, RESOURCE_ID);
           ret = proc.RunActionQuery();
           return ret;
       }

       public int InsertRoleMaster(string RESOURCE_ID, string RESOURCE_TypeID, string CONTACT, string RESOURCE_NAME, string BRANCH)
       {
           int ret = 0;
           ProcedureExecute proc = new ProcedureExecute("PRC_SAVERESOURCELOAD");
           proc.AddNVarcharPara("@RESOURCE_ID", 10, RESOURCE_ID);
           proc.AddNVarcharPara("@RESOURCE_TypeID", 100, RESOURCE_TypeID);
           proc.AddNVarcharPara("@CONTACT", 500, CONTACT);
           proc.AddNVarcharPara("@RESOURCE_NAME", 500, RESOURCE_NAME);
           proc.AddNVarcharPara("@BRANCH", 10, BRANCH);
           proc.AddNVarcharPara("@CREATE_BY", 10, Convert.ToString(HttpContext.Current.Session["userid"]));
           proc.AddNVarcharPara("@UPDATE_BY", 10, Convert.ToString(HttpContext.Current.Session["userid"]));

           ret = proc.RunActionQuery();
           return ret;
       }
    }
}
