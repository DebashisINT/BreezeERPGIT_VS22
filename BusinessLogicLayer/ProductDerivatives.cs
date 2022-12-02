using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public class ProductDerivatives
    {
        public DataSet FoProductsDerivatives()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Fetch_FoProductsDerivatives");
           
            ds = proc.GetDataSet();
            return ds;
        }
        public DataSet FoExchProductsDetails(string vModule, string ProductID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Fetch_FoExchProductsDetails");

            proc.AddVarcharPara("@Module", 100, vModule);
            proc.AddIntegerPara("@ProductID", Convert.ToInt32(ProductID));
           
            ds = proc.GetDataSet();
            return ds;
        }
    }
}
