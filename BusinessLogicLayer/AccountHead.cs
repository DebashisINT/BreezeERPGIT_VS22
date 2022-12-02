using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public class AccountHead
    {
        public int Insert_TransSubAccount(string FinancialYear, string MainAccount_ReferenceID, string SubAccount_ReferenceID,
                                       string CompanyName, string SegmentName, string BranchName,
                                       string openingCR, string openingDR, string ActiveCurrency)
        {
            ProcedureExecute proc;
            try
            {


                using (proc = new ProcedureExecute("InsertTransSubAccount"))
                {

                    proc.AddVarcharPara("@FinancialYear", 100, FinancialYear);
                    proc.AddBigIntegerPara("@MainAccount_ReferenceID",Convert.ToInt64(MainAccount_ReferenceID));
                    proc.AddVarcharPara("@SubAccount_ReferenceID", 100, SubAccount_ReferenceID);
                    proc.AddVarcharPara("@CompanyName", 100, CompanyName);
                    proc.AddVarcharPara("@SegmentName", 100, SegmentName);
                    proc.AddVarcharPara("@BranchName", 100, BranchName);
                    proc.AddDecimalPara("@openingCR", 3, 10, Convert.ToDecimal(openingCR));
                    proc.AddDecimalPara("@openingDR", 3,10,  Convert.ToDecimal(openingDR));
                    proc.AddVarcharPara("@ActiveCurrency",50, ActiveCurrency);
                   
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

        public int UpDate_AccountSummary1(string Module, string FinancialYear, string AccountRefID, string SubAccountRefID,
                                     string CompanyName, string SegmentName, string BranchName,
                                     string openingCR, string openingDR, string ActiveCurrency)
        {
            ProcedureExecute proc;
            try
            {


                using (proc = new ProcedureExecute("UpDateAccountSummary1"))
                {
                    proc.AddVarcharPara("@Module", 100, Module);
                    proc.AddVarcharPara("@FinancialYear", 100, FinancialYear);
                    proc.AddBigIntegerPara("@AccountRefID", Convert.ToInt64(AccountRefID));
                    proc.AddVarcharPara("@SubAccountRefID", 100, SubAccountRefID);
                    proc.AddVarcharPara("@CompanyName", 100, CompanyName);
                    proc.AddVarcharPara("@SegmentName", 100, SegmentName);
                    proc.AddVarcharPara("@BranchName", 100, BranchName);
                    proc.AddDecimalPara("@openingCR",  3,10, Convert.ToDecimal(openingCR));
                    proc.AddDecimalPara("@openingDR", 3,10,  Convert.ToDecimal(openingDR));
                    proc.AddVarcharPara("@ActiveCurrency", 50, ActiveCurrency);

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
