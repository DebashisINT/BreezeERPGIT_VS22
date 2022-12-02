using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public partial class ImportRoutines
    {
        public void xmlCashBankInsert(string cashBankData, string createuser, string finyear, string compID,
            string CashBankName, string TDate, int BRS)
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            try
            {
                using (proc = new ProcedureExecute("AdressDummyInsert"))
                {
                    proc.AddVarcharPara("@cashBankData", 50, cashBankData);
                    proc.AddVarcharPara("@createuser", 100, createuser);
                    proc.AddVarcharPara("@finyear", 50, finyear);
                    proc.AddVarcharPara("@compID", 50, compID);
                    proc.AddVarcharPara("@CashBankName", 50, CashBankName);
                    proc.AddVarcharPara("@TDate", 50, TDate);
                    proc.AddIntegerPara("@BRS", BRS);

                    int i = proc.RunActionQuery();

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
