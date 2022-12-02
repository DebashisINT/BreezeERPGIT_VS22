using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class eInvoice_ConfigurationBL
    {
        public DataSet BindCompanyBranch(String Type)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Prc_EInvoiceBranchCompanyBind");
            proc.AddPara("@Action", Convert.ToString(Type));
            ds = proc.GetDataSet();

            return ds;
        }

        public int InsertUpdateTaxCharges(String Action, string SalesDiscount, String SalesRoundOff, String SalesTCS, String purchaseDiscount, String purchaseRoundOff, String purchaseTCS, string USER_ID)
        {
            int i = 0;
            ProcedureExecute proc = new ProcedureExecute("PROC_Einvoice_ChargesMapInsertUpdate");
            proc.AddPara("@Action", Action);
            proc.AddPara("@SalesDiscount_id", SalesDiscount);
            proc.AddPara("@SalesRoundOff_id", SalesRoundOff);
            proc.AddPara("@SalesOtherCharges_id", SalesTCS);
            proc.AddPara("@PurchaseDiscount_id", purchaseDiscount);
            proc.AddPara("@PurchaseRoundOff_id", purchaseRoundOff);
            proc.AddPara("@PurchaseOtherCharges_id", purchaseTCS);
            proc.AddPara("@USER_ID", USER_ID);
            i = proc.RunActionQuery();

            return i;
        }

        public int InsertUpdateGroupUser(String Action, DataTable dt, string USER_ID,String user_group)
        {
            int i = 0;
            ProcedureExecute proc = new ProcedureExecute("Prc_Einvoice_UserGroupMapInsertUpdate");
            proc.AddPara("@Action", Action);
            proc.AddPara("@user_id", USER_ID);
            proc.AddPara("@UserGroupMap", dt);
            proc.AddPara("@user_group", user_group);
            i = proc.RunActionQuery();

            return i;
        }

        public int InsertUpdateeInvoiceActivation(String Action, DataTable dt, string USER_ID)
        {
            int i = 0;
            ProcedureExecute proc = new ProcedureExecute("Prc_Einvoice_ActivationMapInsertUpdate");
            proc.AddPara("@Action", Action);
            proc.AddPara("@user_id", USER_ID);
            proc.AddPara("@udtEinvoiceActivationMap", dt);
            i = proc.RunActionQuery();

            return i;
        }
    }
}
