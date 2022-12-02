using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class PurchaseReturnRequestBL
    {
        public int DeletePurchaseReturnRequest(string BR_Id)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseReturnRequestDetailsList");
            proc.AddVarcharPara("@Action", 100, "PurchaseReturnRequestDeleteDetails");
            proc.AddIntegerPara("@BR_Id", Convert.ToInt32(BR_Id));
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }
    }
}
