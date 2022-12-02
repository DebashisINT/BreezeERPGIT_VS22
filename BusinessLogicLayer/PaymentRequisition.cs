using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public class PaymentRequisition
    {
        public DataSet getpaymentrequisitiondetails(string id)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_GetPaymentRequision");

            proc.AddVarcharPara("@Id", 30, id);
           
            ds = proc.GetDataSet();
            return ds;
        }



    }
}
