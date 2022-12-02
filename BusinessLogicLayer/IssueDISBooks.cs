using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public partial class IssueDISBooks
    {
        public DataTable DpStockUsageFetch(string Dp)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("[DpStockUsageFetch]");
            proc.AddVarcharPara("@Dp", 100, Dp);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable DpStockUsageFetch2(string Dp,string Mode)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("[DpStockUsageFetch]");
            proc.AddVarcharPara("@Dp", 100, Dp);
            proc.AddVarcharPara("@Mode", 100, Mode);
            dt = proc.GetTable();
            return dt;
        }

        public DataSet DpStockUsageFetchPOA(string Dp)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("[DpStockUsageFetchPOA]");
            proc.AddVarcharPara("@Dp", 100, Dp);
            ds = proc.GetDataSet();
            return ds;
        }

        public int DpStockUsageDeletePOA(int DpSlipsIssued_ID)
        {
            ProcedureExecute proc;
            try
            {


                using (proc = new ProcedureExecute("[DpStockUsageDeletePOA]"))
                {

                    proc.AddIntegerPara("@DpSlipsIssued_ID", DpSlipsIssued_ID); 
                    int i = proc.RunActionQuery();
                    return i;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                proc = null;
            }
        }
    }
}
