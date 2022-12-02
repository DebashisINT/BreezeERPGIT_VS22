using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using DataAccessLayer;
using System.Web;
using EntityLayer;

namespace BusinessLogicLayer
{
    public class PurchaseIndentBL
    {
        #region Approval Section Start By Sam
        public int PurchaseIndentEditablePermission(int userid)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseIndentDetailsList");
            proc.AddVarcharPara("@Action", 100, "PurchaseIndentEditablePermission");
            proc.AddIntegerPara("@userid", userid);
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }

        public int GetUserLevelByUserID(int userid)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseIndentDetailsList");
            proc.AddVarcharPara("@Action", 100, "GetUserLevelByUserID");
            proc.AddIntegerPara("@userid", userid);
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }

        public DataTable PopulateApprovalPendingCountByUserLevel(int userlevel)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseIndentDetailsList");
            proc.AddVarcharPara("@Action", 500, "PopulateApprovalPendingCountByUserLevel");
            proc.AddIntegerPara("@userlevel", userlevel);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetPendingPurchaseIndentListByUserLevel(int userlevel)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseIndentDetailsList");
            proc.AddVarcharPara("@Action", 500, "GetPendingPurchaseIndentListByUserLevel");
            proc.AddIntegerPara("@userlevel", userlevel);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable PopulateUserWisePurchaseIndentStatus(int userid)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseIndentDetailsList");
            proc.AddVarcharPara("@Action", 100, "PopulateUserWisePurchaseIndentStatus");
            proc.AddIntegerPara("@userid", userid);
            dt = proc.GetTable();
            return dt;
        }

        #endregion Approval Section End By Sam
        public DataTable CheckPITraanaction(string piid)
        {
            DataTable dt = new DataTable();

            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseIndentDetailsList");
            proc.AddVarcharPara("@Action", 100, "PurchaseIndentTagOrNotInPurchaseOrder");
            proc.AddVarcharPara("@Indent_Id", 200, piid);

            dt = proc.GetTable();
            return dt;
        }

        public static int ClosedPurchaseIndent(string KeyVal, string Reason)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseIndentDetailsList");
            proc.AddVarcharPara("@Action", 100, "ClosedPurchaseIndent");
            proc.AddVarcharPara("@Document_Id", 50, KeyVal);
            //proc.AddVarcharPara("@CompanyID", 50, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            //proc.AddVarcharPara("@FinYear", 50, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            proc.AddVarcharPara("@Reason", 50, Convert.ToString(Reason));
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }

        public DataTable CheckProjectPITraanaction(string piid)
        {
            DataTable dt = new DataTable();

            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseIndentDetailsList");
            proc.AddVarcharPara("@Action", 100, "ProjPurchaseIndentTagOrNotInPurchaseOrder");
            proc.AddVarcharPara("@Indent_Id", 200, piid);

            dt = proc.GetTable();
            return dt;
        }
        public DataTable CheckBRTraanaction(string Brid)
        {
            DataTable dt = new DataTable();

            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseIndentDetailsList");
            proc.AddVarcharPara("@Action", 100, "BranchRequisitionTagOrNotInStockOut");
            proc.AddVarcharPara("@BranchRequisitiont_Id", 200, Brid);

            dt = proc.GetTable();
            return dt;
        }
        public int CheckBRTraanactionCancel(string Brid)
        {
            int rtrnvalue = 0;
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseIndentDetailsList");
            proc.AddVarcharPara("@Action", 100, "BranchRequisitionFullCancleOrPartial");
            proc.AddVarcharPara("@Indent_Id", 200, Brid);
            proc.AddVarcharPara("@ReturnValue", 200, "0", QueryParameterDirection.Output);
            proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;
        }
    }
}
