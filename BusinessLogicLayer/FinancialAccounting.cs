using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public partial class FinancialAccounting
    {
        public DataSet ReturnChequeEntry(string BankID, string ChequeNumber, decimal Amount, string TranDate, int CreateUser,
            string FinYear, int SegmentID, string CompanyID, string type, string MNarration) 
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("ReturnChequeEntry");
            proc.AddVarcharPara("@BankID", 100, BankID);
            proc.AddNVarcharPara("@ChequeNumber", 100, ChequeNumber);
            proc.AddIntegerPara("@Amount",Convert.ToInt32(Convert.ToDecimal(Amount))); 
            proc.AddVarcharPara("@TranDate", 100, TranDate);
            proc.AddIntegerPara("@CreateUser", CreateUser);
            proc.AddVarcharPara("@FinYear", 100, FinYear); 
            proc.AddIntegerPara("@SegmentID", SegmentID);
            proc.AddNVarcharPara("@CompanyID", 100, CompanyID);
            proc.AddVarcharPara("@type", 100, type); 
            proc.AddVarcharPara("@MNarration", 100, MNarration);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet ReturnChequeEntryForReceived(string BankID, string ChequeNumber, decimal Amount, string TranDate, int CreateUser,
            string FinYear, int SegmentID, string CompanyID, string type, string MNarration)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("ReturnChequeEntryForReceived");
            proc.AddVarcharPara("@BankID", 100, BankID);
            proc.AddNVarcharPara("@ChequeNumber", 100, ChequeNumber);
            proc.AddDecimalPara("@Amount", 28, 6, Amount);
            proc.AddVarcharPara("@TranDate", 100, TranDate);
            proc.AddIntegerPara("@CreateUser", CreateUser);
            proc.AddVarcharPara("@FinYear", 100, FinYear);
            proc.AddIntegerPara("@SegmentID", SegmentID);
            proc.AddNVarcharPara("@CompanyID", 100, CompanyID);
            proc.AddVarcharPara("@type", 100, type);
            proc.AddVarcharPara("@MNarration", 100, MNarration);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet FetchManualBRSData(string TdateTo, string TdateFrom, string WhichRecord, string AccountID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Fetch_ManualBRS_Data");
            proc.AddVarcharPara("@TdateTo", 100, TdateTo);
            proc.AddNVarcharPara("@TdateFrom", 100, TdateFrom);
            proc.AddVarcharPara("@WhichRecord", 100, WhichRecord);
            proc.AddVarcharPara("@AccountID", 100, AccountID);
            ds = proc.GetDataSet();
            return ds;
        }

        public void UpdateManualBRS(string Doc, string UserID)
        {
            ProcedureExecute proc;
            try
            {


                using (proc = new ProcedureExecute("Update_ManualBRS"))
                {

                    proc.AddVarcharPara("@Doc", -1, Doc);
                    proc.AddVarcharPara("@UserID", 100, UserID);
                    int i = proc.RunActionQuery();
                    //return i;
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

        public DataSet FetchCBEDataSet(string UserID, string IBRef, string TransactionType, int TradeCurrency)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Fetch_CBE_DataSet");
            proc.AddVarcharPara("@UserID", 100, UserID);
            proc.AddNVarcharPara("@IBRef", 100, IBRef);
            proc.AddNVarcharPara("@TransactionType", 100, TransactionType);
            proc.AddIntegerPara("@TradeCurrency", TradeCurrency);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet InsertCashBankVoucherEntry(string CashBankXML, string CashBankID, string CreateUser, string FinYear, string CompanyID, string TransactionDate,
            string ExchangeSegmentID, string TransactionType, string EntryUserProfile, string Narration, string FormMode, string OldIBRef, string IsVoucherNumberChange,
            int CurrencyID,out string OutIBRef,string voucherNo,string rate)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Insert_CashBankVoucherEntry");
            proc.AddXMLPara("@CashBankXML", CashBankXML);
            proc.AddNVarcharPara("@CashBankID", 100, CashBankID);
            proc.AddNVarcharPara("@CreateUser", 100, CreateUser);
            proc.AddNVarcharPara("@FinYear",100, FinYear); 
            proc.AddVarcharPara("@CompanyID", 100, CompanyID);
            proc.AddNVarcharPara("@TransactionDate", 100, TransactionDate);
            proc.AddNVarcharPara("@ExchangeSegmentID", 100, ExchangeSegmentID);
            proc.AddNVarcharPara("@TransactionType",100, TransactionType); 
            proc.AddVarcharPara("@EntryUserProfile", 100, EntryUserProfile);
            proc.AddNVarcharPara("@Narration", 100, Narration);
            proc.AddNVarcharPara("@FormMode", 100, FormMode);
            proc.AddNVarcharPara("@OldIBRef",100, OldIBRef); 
            proc.AddVarcharPara("@IsVoucherNumberChange", 100, IsVoucherNumberChange);
            proc.AddIntegerPara("@CurrencyID", CurrencyID);
            //proc.AddNVarcharPara("@TransactionType", 100, TransactionType);
            //priti added on 19-01-2017
            proc.AddVarcharPara("@VoucherNumber", 15, voucherNo);         
            proc.AddDecimalPara("@rate",5,18,Convert.ToDecimal(rate));
            proc.AddVarcharPara("@OutIBRef", 100,"", QueryParameterDirection.Output);
            OutIBRef = proc.GetParaValue("@OutIBRef").ToString();
            ds = proc.GetDataSet();
            return ds;
        }

        public int DeleteCB(string IBRef, int userid)
        {
            ProcedureExecute proc;
            try
            {


                using (proc = new ProcedureExecute("Delete_CB"))
                { 
                    proc.AddVarcharPara("@IBRef", 100, IBRef);
                    proc.AddIntegerPara("@CreateUser", userid);
                    
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



        public DataSet xmlCashBankInsertPayout( string cashBankData,string createuser,string finyear,string compID,
                                               string CashBankName,string TDate,string BRS,string Mode)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("xmlCashBankInsertPayout");
            proc.AddVarcharPara("@cashBankData", 100, cashBankData);
            proc.AddVarcharPara("@createuser", 100, createuser);
            proc.AddVarcharPara("@finyear", 100, finyear);
            proc.AddVarcharPara("@compID",100, compID);
            proc.AddVarcharPara("@CashBankName", 100, CashBankName);
            proc.AddDateTimePara("@TDate",Convert.ToDateTime(TDate));
            proc.AddIntegerPara("@BRS", Convert.ToInt32(BRS));
            proc.AddVarcharPara("@Mode",100, Mode);
            ds = proc.GetDataSet();
            return ds;
        }
    }
}
