using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class InstallationCouponBL
    {
        public DataTable GetBranch()
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("GetBranch");
            ds = proc.GetTable();
            return ds;
        }
        public DataTable GetInstallationInvoiceDetails(string branch_parentId, string userbranchlist, string Action)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesChallan_Details");
            proc.AddVarcharPara("@Action", 500, Action);
            proc.AddVarcharPara("@userbranchlist", 500, userbranchlist);
            proc.AddVarcharPara("@branch_parentId", 500, branch_parentId);
            dt = proc.GetTable();
            return dt;
        }

        public static string SaveVerification(string InvoiceDetails_Id, string IsInstallVerified)
        {
            try
            {
                string msg = "";
                ProcedureExecute proc = new ProcedureExecute("SP_InstallationCoupon_CRUD");

                proc.AddIntegerPara("@InvoiceDetails_Id", Convert.ToInt32(InvoiceDetails_Id));
                proc.AddVarcharPara("@IsInstallVerified", 10, IsInstallVerified);

                int _Ret = proc.RunActionQuery() > 0 ? 1 : -1;

                if (_Ret == 1)
                {
                    msg = "Verification done successfully";
                }
                else
                {
                    msg = "Error occured";
                }
                return msg;
            }
            catch (Exception ex)
            {
                return "Error occured";
            }
        }
    }
}
