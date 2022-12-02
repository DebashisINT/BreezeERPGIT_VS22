using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BusinessLogicLayer
{
    public class ApprovalBL
    {
        public DataTable GetListData(string ModuleId,string basedon)
        {
            DataTable Dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_Approval");
            proc.AddVarcharPara("@Action", 50, "GridData");
            proc.AddVarcharPara("@ModuleName",100, ModuleId);
            proc.AddVarcharPara("@Basedon", 100, basedon);

            try
            {
                Dt = proc.GetTable();
            }
            catch (Exception e)
            {

            }

            return Dt;
        }

        public DataTable GetListDataTransaction(string ModuleId, string basedon,string frmdt,string todt)
        {
            DataTable Dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_Approval");
            proc.AddVarcharPara("@Action", 50, "GridData");
            proc.AddVarcharPara("@ModuleName", 100, ModuleId);
            proc.AddVarcharPara("@Basedon", 100, basedon);
            proc.AddDateTimePara("@StatDate", Convert.ToDateTime(frmdt));
            proc.AddDateTimePara("@EndDate",  Convert.ToDateTime(todt));

            try
            {
                Dt = proc.GetTable();
            }
            catch (Exception e)
            {

            }

            return Dt;
        }

        public DataTable SaveApproval(string APPROVALID, string APPROVAL_TYPE, string Current_Status)
        {
            DataTable Dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_APPROVAL_ADDEDIT");
            proc.AddVarcharPara("@APPROVALID", -1, APPROVALID);
            proc.AddVarcharPara("@APPROVAL_TYPE", -1, APPROVAL_TYPE);
            proc.AddVarcharPara("@USER_ID", 100, Convert.ToString(HttpContext.Current.Session["userid"]));
            proc.AddVarcharPara("@Current_Status", 100, Current_Status);
            try
            {
                Dt = proc.GetTable();
            }
            catch (Exception e)
            {

            }

            return Dt;
        }

        public DataTable SaveApprovaltransaction(string APPROVALID, string APPROVAL_TYPE, string Current_Status)
        {
            DataTable Dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_APPROVAL_ADDEDIT_Transaction");
            proc.AddVarcharPara("@APPROVALID", -1, APPROVALID);
            proc.AddVarcharPara("@APPROVAL_TYPE", -1, APPROVAL_TYPE);
            proc.AddVarcharPara("@USER_ID", 100, Convert.ToString(HttpContext.Current.Session["userid"]));
            proc.AddVarcharPara("@Current_Status", 100, Current_Status);
            try
            {
                Dt = proc.GetTable();
            }
            catch (Exception e)
            {

            }

            return Dt;
        }
    }
}
