using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessLayer;
using System.Data;

namespace BusinessLogicLayer
{
    public class MainAccountHead_BL
    {
        public int MainAccountGridUpdating(string MainAccount_ReferenceID, string AccountType, string BankCompany,
            string BankCashType, string BankAccountType, string ExchengSegment, string AccountCode, string AccountGroup,
            string AccountName, string BankAccountNo, string SubLedgerType, string TDSRate, bool FBTApplicable,
            decimal FBTRate, decimal RateOfIntrest, decimal Depreciation, string CreateUser, string branch, string paymentType, string BranchList, string strEditcmbOldUnitLedger, string strReverseApplicable = "0")
        {
            ProcedureExecute proc;

            try
            {
                using (proc = new ProcedureExecute("UpdateMainAccountHead"))
                {
                    int rtrnvalue = 0;
                    proc.AddIntegerPara("@MainAccount_ReferenceID", Convert.ToInt32(MainAccount_ReferenceID));
                    proc.AddVarcharPara("@AccountType", 100, AccountType);
                    proc.AddVarcharPara("@BankCompany", 20, BankCompany);
                    proc.AddVarcharPara("@BankCashType", 100, BankCashType);
                    proc.AddVarcharPara("@BankAccountType", 100, BankAccountType);
                    proc.AddVarcharPara("@ExchengSegment", 100, ExchengSegment);
                    proc.AddVarcharPara("@AccountCode", 50, AccountCode);
                    proc.AddVarcharPara("@AccountGroup", 100, AccountGroup);
                    proc.AddVarcharPara("@AccountName", 100, AccountName);
                    proc.AddVarcharPara("@BankAccountNo", 100, BankAccountNo);
                    proc.AddVarcharPara("@SubLedgerType", 100, SubLedgerType);
                    // .............................Code Commented and Added by Sam on 15122016 to change the parameter length.from 10 to 50 ..................
                    proc.AddVarcharPara("@TDSRate", 50, TDSRate);
                    //proc.AddVarcharPara("@TDSRate", 10, TDSRate);
                    // .............................Code Above Commented and Added by Sam on 15122016...................................... 
                    proc.AddBooleanPara("@FBTApplicable", FBTApplicable);
                    proc.AddDecimalPara("@FBTRate", 2, 6, FBTRate);

                    proc.AddDecimalPara("@RateOfIntrest", 2, 6, RateOfIntrest);
                    proc.AddDecimalPara("@Depreciation", 2, 6, Depreciation);

                    proc.AddVarcharPara("@LedgerBranchList", 4000, BranchList);

                    proc.AddIntegerPara("@CreateUser", Convert.ToInt32(CreateUser));
                    if (branch != null && branch != "")
                    {
                        proc.AddIntegerPara("@branchId", Convert.ToInt32(branch));
                    }
                    proc.AddVarcharPara("@MainAccount_PaymentType", 10, paymentType);
                    proc.AddIntegerPara("@oldUnitLedger", Convert.ToInt32(strEditcmbOldUnitLedger));
                    proc.AddIntegerPara("@ReverseApplicable", Convert.ToInt32(strReverseApplicable));

                    proc.AddVarcharPara("@ReturnValue", 200, "0", QueryParameterDirection.Output);
                    int NoOfRowEffected = proc.RunActionQuery();

                    rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
                    return rtrnvalue;
                    // return NoOfRowEffected;
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

        public int MainAccountGridInserting(string AccountType, string BankCompany,
           string BankCashType, string BankAccountType, string ExchengSegment, string AccountCode, string AccountGroup,
           string AccountName, string BankAccountNo, string SubLedgerType, string TDSRate, bool FBTApplicable,
           decimal FBTRate, decimal RateOfIntrest, decimal Depreciation, string CreateUser, string branch, string paymentTypevalue, string LedgerBranchList, string strOldUnitLedger, string strReverseApplicable = "0")
        {
            ProcedureExecute proc;

            try
            {
                using (proc = new ProcedureExecute("MainAccountInsert"))
                {
                    int rtrnvalue = 0;
                    proc.AddVarcharPara("@AccountType", 100, AccountType);
                    proc.AddVarcharPara("@BankCompany", 20, BankCompany);
                    proc.AddVarcharPara("@BankCashType", 100, BankCashType);
                    proc.AddVarcharPara("@BankAccountType", 100, BankAccountType);
                    proc.AddVarcharPara("@ExchengSegment", 100, ExchengSegment);
                    proc.AddVarcharPara("@AccountCode", 50, AccountCode);
                    proc.AddVarcharPara("@AccountGroup", 100, AccountGroup);
                    proc.AddVarcharPara("@AccountName", 100, AccountName);
                    proc.AddVarcharPara("@BankAccountNo", 100, BankAccountNo);
                    proc.AddVarcharPara("@SubLedgerType", 100, SubLedgerType);
                    // .............................Code Commented and Added by Sam on 15122016 to change the parameter length. ..................
                    proc.AddVarcharPara("@TDSRate", 50, TDSRate);
                    //proc.AddVarcharPara("@TDSRate", 10, TDSRate);
                    // .............................Code Above Commented and Added by Sam on 15122016...................................... 
                    proc.AddBooleanPara("@FBTApplicable", FBTApplicable);
                    proc.AddDecimalPara("@FBTRate", 2, 6, FBTRate);

                    proc.AddDecimalPara("@RateOfIntrest", 2, 6, RateOfIntrest);
                    proc.AddDecimalPara("@Depreciation", 2, 6, Depreciation);
                    proc.AddIntegerPara("@CreateUser", Convert.ToInt32(CreateUser));

                    proc.AddVarcharPara("@LedgerBranchList", 4000, LedgerBranchList);
                    if (branch != null && branch != "")
                    {
                        proc.AddIntegerPara("@branchId", Convert.ToInt32(branch));
                    }

                    proc.AddVarcharPara("@MainAccount_PaymentType", 1000, paymentTypevalue);
                    proc.AddIntegerPara("@oldUnitLedger", Convert.ToInt32(strOldUnitLedger));
                    proc.AddIntegerPara("@ReverseApplicable", Convert.ToInt32(strReverseApplicable));

                    proc.AddVarcharPara("@ReturnValue", 200, "0", QueryParameterDirection.Output);

                    int NoOfRowEffected = proc.RunActionQuery();
                    //return NoOfRowEffected;
                    rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
                    return rtrnvalue;
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

        public int MainAccountGridDeleteCustomMain(string AccountCode)
        {
            ProcedureExecute proc;

            try
            {
                using (proc = new ProcedureExecute("DeleteCustomMain"))
                {

                    proc.AddVarcharPara("@mainAc", 50, AccountCode);

                    int NoOfRowEffected = proc.RunActionQuery();

                    return NoOfRowEffected;
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

        public int MainAccountGridDeleteMainAccountHead(string AccountCode)
        {
            ProcedureExecute proc;

            try
            {
                using (proc = new ProcedureExecute("DeleteMainAccountHead"))
                {

                    proc.AddVarcharPara("@AcountCode", 50, AccountCode);

                    int NoOfRowEffected = proc.RunActionQuery();

                    return NoOfRowEffected;
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

        public int InsertInTransMainAccount(string FinancialYear, Int64 MainAccount_ReferenceID, string CompanyName,
            string SegmentName, string BranchName, decimal openingCR, decimal openingDR, string ActiveCurrency)
        {
            ProcedureExecute proc;

            try
            {

                using (proc = new ProcedureExecute("InsertTransMainAccountSummary"))
                {
                    proc.AddVarcharPara("@FinancialYear", 100, FinancialYear);

                    proc.AddIntegerPara("@MainAccount_ReferenceID", Convert.ToInt32(MainAccount_ReferenceID));
                    proc.AddVarcharPara("@CompanyName", 10, CompanyName);
                    proc.AddVarcharPara("@SegmentName", 10, SegmentName);
                    //proc.AddCharPara("@CompanyName", 10, Convert.ToChar(CompanyName));

                    //proc.AddCharPara("@SegmentName", 10, Convert.ToChar(SegmentName));

                    proc.AddCharPara("@BranchName", 10, Convert.ToChar(BranchName));

                    proc.AddDecimalPara("@openingCR", 8, 28, openingCR);

                    proc.AddDecimalPara("@openingDR", 8, 28, openingDR);

                    proc.AddCharPara("@ActiveCurrency", 10, Convert.ToChar(ActiveCurrency));

                    int NoOfRowEffected = proc.RunActionQuery();

                    return NoOfRowEffected;
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

        public int updateMainAccount(string Module, string FinancialYear, Int64 AccountRefID, string CompanyName, string SegmentName,
            string BranchName, decimal openingCR, decimal openingDR, string ActiveCurrency, string TradeCurrency)
        {
            ProcedureExecute proc;

            try
            {
                using (proc = new ProcedureExecute("UpDateAccountSummary"))
                {
                    proc.AddVarcharPara("@Module", 100, Module);
                    proc.AddVarcharPara("@FinancialYear", 100, FinancialYear);

                    proc.AddIntegerPara("@AccountRefID", Convert.ToInt32(AccountRefID));
                    proc.AddVarcharPara("@CompanyName", 10, CompanyName);
                    proc.AddVarcharPara("@SegmentName", 10, SegmentName);

                    //proc.AddCharPara("@CompanyName", 10, Convert.ToChar(CompanyName));

                    //proc.AddCharPara("@SegmentName", 10, Convert.ToChar(SegmentName));

                    // .............................Code Commented and Added by Sam on 29112016. ..................................... 

                    proc.AddVarcharPara("@BranchName", 10, BranchName);

                    //proc.AddCharPara("@BranchName", 10, Convert.ToChar(BranchName));


                    // .............................Code Above Commented and Added by Sam on 29112016...................................... 

                    proc.AddDecimalPara("@openingCR", 8, 28, openingCR);

                    proc.AddDecimalPara("@openingDR", 8, 28, openingDR);

                    proc.AddVarcharPara("@ActiveCurrency", 3, ActiveCurrency);

                    proc.AddVarcharPara("@TradeCurrency", 3, TradeCurrency);

                    int NoOfRowEffected = proc.RunActionQuery();

                    return NoOfRowEffected;
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


        public DataSet GetHSNSACNoList()
        {
            ProcedureExecute proc;
            DataSet ds;

            try
            {
                using (proc = new ProcedureExecute("prc_Accounts_GetHSNSACNo"))
                {
                    ds = proc.GetDataSet();

                    return ds;
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

        //public int MappedHSNSACToLedger(string ledgerID, string hsn_sacID, string type, bool fobFlag)
        public int MappedHSNSACToLedger(string ledgerID, string hsn_sacID, string type, bool fobFlag, string GSTIN, string PAN, string Deductee_Type, string NameAsPerPan)
        {
            ProcedureExecute proc;

            try
            {
                using (proc = new ProcedureExecute("prc_Accounts_MappedHSNSAC"))
                {
                    proc.AddIntegerPara("@LedgerID", Convert.ToInt32(ledgerID));
                    proc.AddIntegerPara("@HSN_SAC_ID", (string.IsNullOrEmpty(hsn_sacID) ? 0 : Convert.ToInt32(hsn_sacID)));
                    proc.AddVarcharPara("@HSN_SAC_Type", 5, type);
                    proc.AddBooleanPara("@Furtherance_Of_Busines_Flag", Convert.ToBoolean(Convert.ToInt16(fobFlag)));
                    // Code Added By Sam on 05082017 for adding the GSTIN on 05082017
                    proc.AddVarcharPara("@GSTIN", 20, GSTIN);
                    //REv Rajdip
                    proc.AddVarcharPara("@Pan", 20, PAN);
                    proc.AddVarcharPara("@DeducteeType", 20, Deductee_Type);
                    //End Rev Rajdip
                 
                    // Code Added By Sam on 05082017 for adding the GSTIN on 05082017
                    proc.AddVarcharPara("@MainAccount_NameFromPAN", 300, NameAsPerPan);
                    int NoOfRowEffected = proc.RunActionQuery();

                    return NoOfRowEffected;
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

        public DataSet GetMappedHSNSACNoList(string ledgerID)
        {
            ProcedureExecute proc;
            DataSet ds;

            try
            {
                using (proc = new ProcedureExecute("prc_Accounts_GetMAppedHSNSACNo"))
                {
                    proc.AddIntegerPara("@LedgerID", Convert.ToInt32(ledgerID));
                    ds = proc.GetDataSet();

                    return ds;
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
