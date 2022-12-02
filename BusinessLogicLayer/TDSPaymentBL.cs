using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class TDSPaymentBL
    {
        public DataTable GetTDSPayment(DateTime TDSPaymentDate, string TDSCode, string TDSQuarter, string TDSYear,string Type)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_TDSPayment");
            proc.AddVarcharPara("@Action", 100, "GETTDSDOCDETAILS");
            proc.AddPara("@TDSPaymentDate", TDSPaymentDate);
            proc.AddPara("@TDSCode", TDSCode);
            proc.AddPara("@Quater", TDSQuarter);
            proc.AddPara("@Year", TDSYear);
            proc.AddPara("@Type", Type);

            ds = proc.GetTable();
            return ds; 
        }


        public DataSet GetTDSPaymentEdit(string doc_id)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_TDSPayment");
            proc.AddVarcharPara("@Action", 100, "GETTDSDETAILSEDIT");
            proc.AddPara("@CashBankID", doc_id);
            ds = proc.GetDataSet();
            return ds; 
        }
       
    }
}
