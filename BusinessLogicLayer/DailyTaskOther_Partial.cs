using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public partial class DailyTaskOther
    {
        public DataTable CDSLBill1(string startDate, string endDate, string BillDate, string companyId, string DpId, string user, string DPChargeMembers_SegmentID,
            string param)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("cdslBill1");
            proc.AddVarcharPara("@startDate", 100, startDate);
            proc.AddNVarcharPara("@endDate", 100, endDate); 
            proc.AddVarcharPara("@BillDate", 100, BillDate);
            proc.AddNVarcharPara("@companyId", 100, companyId); 
            proc.AddVarcharPara("@DpId", 100, DpId);
            proc.AddNVarcharPara("@user", 100, user); 
            proc.AddVarcharPara("@DPChargeMembers_SegmentID", 100, DPChargeMembers_SegmentID);
            proc.AddNVarcharPara("@param", 100, param);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable CDSLBillDelete(string date, string dp, string financialYear) 
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("cdslBill_Delete");
            proc.AddVarcharPara("@date", 100, date);
            proc.AddNVarcharPara("@dp", 100, dp);
            proc.AddVarcharPara("@financialYear", 100, financialYear);
            dt = proc.GetTable();
            return dt;
        }

        public void InsertSettlement(string dp, string doc)
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            try
            {
                using (proc = new ProcedureExecute("AdressDummyInsert"))
                {
                    proc.AddVarcharPara("@dp", 50, dp);
                    proc.AddVarcharPara("@doc", -1, doc);
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

        public DataSet UpdateTransNSDLOfflineAccount(string dp, string doc, int userid, string DataTableName)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Update_Trans_NSDLOfflineAccount");
            proc.AddVarcharPara("@dp", 100, dp);
            proc.AddNVarcharPara("@doc", -1, doc);
            proc.AddIntegerPara("@userid", userid);
            proc.AddVarcharPara("@DataTableName", 100, DataTableName);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet FetchMatchedUnMatchedRecordVerification(string date, string WhichRecord, string WhichDp)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Sp_Fetch_Matched_UnMatchedRecord_Verification");
            proc.AddVarcharPara("@date", 100, date);
            proc.AddNVarcharPara("@WhichRecord", 200, WhichRecord);
            proc.AddVarcharPara("@WhichDp", 200, WhichDp);
            ds = proc.GetDataSet();
            return ds;
        }

        public int VerifyDeliveryInstructionVerification(string doc, string WhichDP, int VerifierUserID, string WhichQuery, string EntryUserRole)
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            try
            {
                using (proc = new ProcedureExecute("Verify_DeliveryInstruction_Verification"))
                {
                    proc.AddNVarcharPara("@doc", -1, doc);
                    proc.AddVarcharPara("@WhichDP", 100, WhichDP);
                    proc.AddIntegerPara("@VerifierUserID", VerifierUserID); 
                    proc.AddVarcharPara("@WhichQuery", 100, WhichQuery);
                    proc.AddVarcharPara("@EntryUserRole", 100, EntryUserRole); 
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

        public int INSDELUnMatchRecordDoubleCapturing(long RecordID, string WhichQuery, string WhichDP)
        {
            ProcedureExecute proc;
            try
            {
                using (proc = new ProcedureExecute("INSDEL_UnMatchRecord_DoubleCapturing"))
                {
                    proc.AddBigIntegerPara("@RecordID", RecordID);
                    proc.AddVarcharPara("@WhichQuery", 100, WhichQuery);
                    proc.AddVarcharPara("@WhichDP", 100, WhichDP);
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

        public DataSet FetchDelDoubleCapturingExludeRecord(int RecordID, string WhichDP, string WhichQuery)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("FetchDel_DoubleCapturing_ExludeRecord");
            proc.AddIntegerPara("@RecordID", RecordID);
            proc.AddNVarcharPara("@WhichDP", 200, WhichDP);
            proc.AddVarcharPara("@WhichQuery", 200, WhichQuery);
            ds = proc.GetDataSet();
            return ds;
        }



        public DataSet Sp_Tdays(string date, string days, string exchsegment)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Sp_Tdays");
            proc.AddNVarcharPara("@date", 50, date);
            proc.AddIntegerPara("@days", Convert.ToInt32(days));
            proc.AddIntegerPara("@exchsegment", Convert.ToInt32(exchsegment));
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet Process_ExcessMarginRefundStatement(string fordate, string ledgerbalndate, string clients,
          string segment, string CompanyID, string Finyear, 
          string WrkingDays, decimal markupappmrgn, string maintaincashcomp,
          string checkpartialrelease, string grptype, string branch,
              string grpval, string stockreleaseorder, decimal AmountCondition,
          string PaymentMode, string PostingbankId, string PostingSegment,
             string CreateUser, string InterAcNarration, string CaskBankNarration,
          string DematTransNarration, string GenerationType)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Process_ExcessMarginRefundStatement");
            proc.AddNVarcharPara("@fordate", 50, fordate);
            proc.AddNVarcharPara("@ledgerbalndate", 50, ledgerbalndate);
            proc.AddNVarcharPara("@clients", -1, clients);
            proc.AddNVarcharPara("@segment", 50, segment);
            proc.AddNVarcharPara("@CompanyID", 50, CompanyID);
            proc.AddNVarcharPara("@Finyear", 50, Finyear);
            proc.AddIntegerPara("@WrkingDays", Convert.ToInt32(WrkingDays));
            proc.AddDecimalPara("@markupappmrgn", 2,28, markupappmrgn);
            proc.AddNVarcharPara("@maintaincashcomp", 50, maintaincashcomp);
            proc.AddNVarcharPara("@checkpartialrelease", 50, checkpartialrelease);
            proc.AddNVarcharPara("@grptype", 50, grptype);
            proc.AddNVarcharPara("@branch", -1, branch);
            proc.AddNVarcharPara("@grpval", -1, grpval);
            proc.AddNVarcharPara("@stockreleaseorder", 50, stockreleaseorder);
            proc.AddDecimalPara("@AmountCondition", 6, 28,AmountCondition);
            proc.AddNVarcharPara("@PaymentMode", 50, PaymentMode);
            proc.AddNVarcharPara("@PostingbankId", 50, PostingbankId);
            proc.AddNVarcharPara("@PostingSegment", 50, PostingSegment);
            proc.AddNVarcharPara("@CreateUser", 50, CreateUser);
            proc.AddNVarcharPara("@InterAcNarration", 50, InterAcNarration);
            proc.AddNVarcharPara("@CaskBankNarration", 50, CaskBankNarration);
            proc.AddNVarcharPara("@DematTransNarration", 50, DematTransNarration);
            proc.AddNVarcharPara("@GenerationType", 50, GenerationType);
            
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet Process_ExcessMarginRefundStatementAuthorize(string fordate, string CompanyID, string Segment,
            string Finyear, string Grptype, string BranchHierchy,
            string Grpval, string ClientId, string LoginUser)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Process_ExcessMarginRefundStatementAuthorize");
            proc.AddNVarcharPara("@fordate", 50, fordate);
            proc.AddNVarcharPara("@CompanyID", 50, CompanyID);
            proc.AddNVarcharPara("@Segment", 50, Segment);
            proc.AddNVarcharPara("@Finyear", 50, Finyear);
            proc.AddNVarcharPara("@Grptype", 50, Grptype);
            proc.AddNVarcharPara("@BranchHierchy", -1, BranchHierchy);
            proc.AddNVarcharPara("@Grpval", -1, Grpval);
            proc.AddNVarcharPara("@ClientId", -1, ClientId);
            proc.AddNVarcharPara("@LoginUser", 50, LoginUser);
            ds = proc.GetDataSet();
            return ds;
        }

        public int Process_ExcessMarginRefundAuthorize(string ExcessMarginRefundData, string Finyear, string CompanyID, string Segment, string Fordate, string CreateUser)
        {
            int i = 0;
            ProcedureExecute proc = new ProcedureExecute("Process_ExcessMarginRefundAuthorize");
            proc.AddNTextPara("@ExcessMarginRefundData",  ExcessMarginRefundData);
            proc.AddNVarcharPara("@Finyear",50,Finyear);
            proc.AddNVarcharPara("@CompanyID", 50,CompanyID);
            proc.AddNVarcharPara("@Segment", 20, Segment);
            proc.AddNVarcharPara("@Fordate", 50,Fordate);
            proc.AddIntegerPara("@CreateUser", Convert.ToInt32(CreateUser));
            i = proc.RunActionQuery();
            return i;
        }

        public DataSet Process_geneRateQtrTransfersFetch(string fordate, string BankId, string Typeval,
          string Finyear, string MenuId)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Process_geneRateQtrTransfersFetch");
            proc.AddNVarcharPara("@fordate", 50, fordate);
            proc.AddNVarcharPara("@BankId", 50, BankId);
            proc.AddNVarcharPara("@Type", 10, Typeval);
            proc.AddNVarcharPara("@Finyear", 50, Finyear);
            proc.AddNVarcharPara("@MenuId", 50, MenuId);
          
            ds = proc.GetDataSet();
            return ds;
        }

        public int DeliveryProcessingMarginHoldBack(string ExcessMarginRefundData, string Finyear, string CompanyID, string Segment, string Fordate, string CreateUser)
        {
            int i = 0;
            ProcedureExecute proc = new ProcedureExecute("DeliveryProcessingMarginHoldBack");
            proc.AddNTextPara("@clientPayoutData", ExcessMarginRefundData);
            proc.AddNVarcharPara("@Finyear", 50, Finyear);
            proc.AddNVarcharPara("@compID", 50, CompanyID);
            proc.AddNVarcharPara("@segmentid", 20, Segment);
            proc.AddNVarcharPara("@TransferDate", 50, Fordate);
            proc.AddNVarcharPara("@CreateUser", 50, CreateUser);
            i = proc.RunActionQuery();
            return i;
        }
        public int Process_GeneRateQtrTransfers(string ExcessMarginRefundData, string Fordate, string Finyear, string CompanyId, string Typedata, string CreateUser)
        {
            int i = 0;
            ProcedureExecute proc = new ProcedureExecute("Process_GeneRateQtrTransfers");
            proc.AddNTextPara("@@ALLData", ExcessMarginRefundData);
            proc.AddNVarcharPara("@Fordate", 50, Fordate);
            proc.AddNVarcharPara("@Finyear", 50, Finyear);
            proc.AddNVarcharPara("@CompanyId", 20, CompanyId);
            proc.AddNVarcharPara("@Type", 10, Typedata);
            proc.AddNVarcharPara("@CreateUser", 50, CreateUser);
            i = proc.RunActionQuery();
            return i;
        }
        public int xmlCashBankInsertPayout(string cashBankData, string createuser, string finyear, string compID, string CashBankName, string TDate,
            string BRS, string Mode)
        {
            int i = 0;
            ProcedureExecute proc = new ProcedureExecute("xmlCashBankInsertPayout");
            proc.AddNTextPara("@cashBankData", cashBankData);
            proc.AddNVarcharPara("@createuser", 50, createuser);
            proc.AddNVarcharPara("@finyear", 50, finyear);
            proc.AddNVarcharPara("@compID", 20, compID);
            proc.AddNVarcharPara("@CashBankName", 50, CashBankName);
            proc.AddNVarcharPara("@TDate",50,TDate);
            proc.AddIntegerPara("@BRS",Convert.ToInt32(BRS));
            proc.AddNVarcharPara("@Mode", 10, Mode);
            i = proc.RunActionQuery();
            return i;
        }
        public int Process_InterAcTransFerYearEnd(string Finyear, string CompanyID, string Segment, string Fordate)
        {
            int i = 0;
            ProcedureExecute proc = new ProcedureExecute("Process_InterAcTransFerYearEnd");        
            proc.AddNVarcharPara("@Fordate", 50, Finyear);
            proc.AddNVarcharPara("@Finyear", 50, CompanyID);
            proc.AddNVarcharPara("@CompanyId", 50, Segment);
            proc.AddNVarcharPara("@CreateUser", 50, Fordate);           
            i = proc.RunActionQuery();
            return i;
        }

        public DataSet YearEndMarginCF(string CompanyId, string FinYear, string CreatUser)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("YearEndMarginCF");
            proc.AddNVarcharPara("@CompanyId", 50, CompanyId);
            proc.AddNVarcharPara("@FinYear", 50, FinYear);
            proc.AddIntegerPara("@CreatUser",Convert.ToInt32(CreatUser));          
            ds = proc.GetDataSet();
            return ds;
        }
        public DataSet YearEndStcks(string CompanyId, string FinYear, string CreatUser,
           string ClientId, string SegmentId)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("YearEndStcks");
            proc.AddNVarcharPara("@CompanyId", 50, CompanyId);
            proc.AddNVarcharPara("@FinYear", 50, FinYear);
            proc.AddIntegerPara("@CreatUser",Convert.ToInt32(CreatUser));
            proc.AddNVarcharPara("@ClientId", 50, ClientId);
            proc.AddNVarcharPara("@SegmentId", 50, SegmentId);

            ds = proc.GetDataSet();
            return ds;
        }
        public DataSet YearEndObligationCF(string fordate, string BankId, string CreatUser)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("YearEndObligationCF");
            proc.AddNVarcharPara("@CompanyId", 50, fordate);
            proc.AddNVarcharPara("@FinYear", 50, BankId);
            proc.AddIntegerPara("@CreatUser", Convert.ToInt32(CreatUser));
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet SelectInterAccountTransfer(string SAccount, string Date, string Segment,
         string CompanyID, string Client, string FinYear,
            decimal Amount, string DrCr, string ActiveCurrency,
            string TradeCurrency)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("SelectInterAccountTransfer");
            proc.AddNVarcharPara("@SAccount", 50, SAccount);
            proc.AddNVarcharPara("@Date", 50, Date);
            proc.AddIntegerPara("@Segment", Convert.ToInt32(Segment));
            proc.AddNVarcharPara("@CompanyID", 50, CompanyID);
            proc.AddNVarcharPara("@Client", -1, Client);
            proc.AddNVarcharPara("@FinYear", 50, FinYear);
            proc.AddDecimalPara("@Amount", 6,28, Amount);
            proc.AddNVarcharPara("@DrCr", 50, DrCr);
            proc.AddNVarcharPara("@ActiveCurrency", 10, ActiveCurrency);
            proc.AddNVarcharPara("@TradeCurrency", 10, TradeCurrency);
            ds = proc.GetDataSet();
            return ds;
        }
        public int CreateJVForInterAccountTransfer(string clientPayoutData, string CreateUser, string TDate,
            string segmentid, string compID, string Typedata, string ActiveCurrency)
        {
            int i = 0;
            ProcedureExecute proc = new ProcedureExecute("CreateJVForInterAccountTransfer");
            proc.AddNTextPara("@clientPayoutData",clientPayoutData);
            proc.AddIntegerPara("@CreateUser", Convert.ToInt32(CreateUser));
            proc.AddIntegerPara("@TDate", Convert.ToInt32(TDate));
            proc.AddIntegerPara("@Segment", Convert.ToInt32(segmentid));
            proc.AddNVarcharPara("@compID", 50, compID);
            proc.AddNVarcharPara("@Type", 50, Typedata);
            proc.AddNVarcharPara("@ActiveCurrency", 50, ActiveCurrency);
            i = proc.RunActionQuery();
            return i;
        }



        public DataSet CorporateActionCheckforSecondTime(string compID, string segment, string productID,
              string ISinNew, string ISinOld, string account,
              string finyear, string date, string AdjType)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("CorporateActionCheckforSecondTime");
            proc.AddNVarcharPara("@compID", 50, compID);
            proc.AddIntegerPara("@segment",Convert.ToInt32(segment));
            proc.AddIntegerPara("@productID",Convert.ToInt32(productID));
            proc.AddNVarcharPara("@ISinNew", 50, ISinNew);
            proc.AddNVarcharPara("@ISinOld", 50, ISinOld);
            proc.AddIntegerPara("@account", Convert.ToInt32(account));
            proc.AddNVarcharPara("@finyear", -1, finyear);
            proc.AddNVarcharPara("@date", 50, date);
            proc.AddNVarcharPara("@AdjType", 50, AdjType);
            ds = proc.GetDataSet();
            return ds;
        }


        public int CorporateActionAdjustment(string ActionAdjData, string createuser, string finyear,
              string compID, string date, string segment, string TypeVal, string TranDate, string ExDate, string ExSegmentID)
        {
            int i = 0;
            ProcedureExecute proc = new ProcedureExecute("CorporateActionAdjustment");
            proc.AddNTextPara("@ActionAdjData", ActionAdjData);
            proc.AddNVarcharPara("@createuser",50,createuser);
            proc.AddNVarcharPara("@finyear", 50,finyear);
            proc.AddIntegerPara("@compID", Convert.ToInt32(compID));
            proc.AddNVarcharPara("@date", 50, date);
            proc.AddIntegerPara("@segment",Convert.ToInt32(segment));
            proc.AddNVarcharPara("@Type", 50, TypeVal);
            proc.AddNVarcharPara("@TranDate", 50, TranDate);
            proc.AddNVarcharPara("@ExDate", 50, ExDate);
            proc.AddIntegerPara("@ExSegmentID",Convert.ToInt32(ExSegmentID));
            i = proc.RunActionQuery();
            return i;
        }
        public int CorporateActionDelete(string compID, string segment, string productID,
           string ISinNew, string ISinOld, string account, string finyear, string date, string createuser, string AdjType)
        {
            int i = 0;
            ProcedureExecute proc = new ProcedureExecute("CorporateActionDelete");
            proc.AddNVarcharPara("@compID",50, compID);
            proc.AddIntegerPara("@segment",Convert.ToInt32(segment));
            proc.AddIntegerPara("@productID",Convert.ToInt32(productID));
            proc.AddNVarcharPara("@ISinNew",50,ISinNew);
            proc.AddNVarcharPara("@ISinOld", 50, ISinOld);
            proc.AddIntegerPara("@account", Convert.ToInt32(account));
            proc.AddNVarcharPara("@finyear", 50, finyear);
            proc.AddNVarcharPara("@date", 50, date);
            proc.AddIntegerPara("@createuser", Convert.ToInt32(createuser));
            proc.AddNVarcharPara("@AdjType",20, AdjType);
            i = proc.RunActionQuery();
            return i;
        }


        public DataSet CorporateActionAdjShow(string fromdate, string segment, string Companyid,
              string Finyear, string productid, string accountid,
              string ISIN, decimal rate1, decimal rate2, string Typeval)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("CorporateActionAdjShow");
            proc.AddNVarcharPara("@fromdate", 50, fromdate);
            proc.AddNVarcharPara("@segment",-1,segment);
            proc.AddNVarcharPara("@Companyid", 50,Companyid);
            proc.AddNVarcharPara("@Finyear", 50, Finyear);
            proc.AddNVarcharPara("@productid", -1, productid);
            proc.AddNVarcharPara("@accountid", 50,accountid);
            proc.AddNVarcharPara("@ISIN", 50, ISIN);
            proc.AddDecimalPara("@rate1", 1,10, rate1);
            proc.AddDecimalPara("@rate2",  1,10, rate2);
            proc.AddNVarcharPara("@Type", 50, Typeval);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet DividendPostingShow(string fromdate, string segment, string Companyid,
                 string Finyear, string productid, string accountid,
                 decimal BonusAmt)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("DividendPostingShow");
            proc.AddNVarcharPara("@fromdate", 50, fromdate);
            proc.AddNVarcharPara("@segment", -1, segment);
            proc.AddNVarcharPara("@Companyid", 50, Companyid);
            proc.AddNVarcharPara("@Finyear", 50, Finyear);
            proc.AddNVarcharPara("@productid", -1, productid);
            proc.AddNVarcharPara("@accountid", 50, accountid);
            proc.AddDecimalPara("@BonusAmt", 4,28, BonusAmt);
          
            ds = proc.GetDataSet();
            return ds;
        }


        public int DividendPostingGenerate(string DividendPostingData, string createuser, string finyear,
                string compID, string date, string segment,
                string TranDate, string rdate)
        {
            int i = 0;
            ProcedureExecute proc = new ProcedureExecute("DividendPostingGenerate");
            proc.AddNTextPara("@DividendPostingData",  DividendPostingData);
            proc.AddNVarcharPara("@createuser", 50, createuser);
            proc.AddNVarcharPara("@finyear", 50, finyear);
            proc.AddNVarcharPara("@compID", 50, compID);
            proc.AddNVarcharPara("@date", -1, date);
            proc.AddIntegerPara("@segment",Convert.ToInt32(segment));
            proc.AddNVarcharPara("@TranDate", 50, TranDate);
            proc.AddNVarcharPara("@rdate", 50, rdate);
          
            i = proc.RunActionQuery();
            return i;
        }
        public int DividendPostingDelete(string compID, string segment, string productID,
              string finyear, string date)
        {
            int i = 0;
            ProcedureExecute proc = new ProcedureExecute("DividendPostingDelete");
            proc.AddNVarcharPara("@compID",50, compID);
            proc.AddIntegerPara("@segment",  Convert.ToInt32(segment));
            proc.AddIntegerPara("@productID", Convert.ToInt32(productID));
            proc.AddNVarcharPara("@finyear", 50, finyear);
            proc.AddNVarcharPara("@date", -1, date);
            i = proc.RunActionQuery();
            return i;
        }


        public DataSet Sp_GenerateDematCharges(string segmentid, string companyID, string FromDate,
               string ToDate, string createuser, string finyear,
               string action)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Sp_GenerateDematCharges");
            proc.AddNVarcharPara("@segmentid", 50, segmentid);
            proc.AddNVarcharPara("@companyID", 100, companyID);
            proc.AddNVarcharPara("@FromDate", 50, FromDate);
            proc.AddNVarcharPara("@ToDate", 50, ToDate);
            proc.AddNVarcharPara("@createuser", 50, createuser);
            proc.AddNVarcharPara("@finyear", 50, finyear);
            proc.AddNVarcharPara("@action", 50, action);
          
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet xmlJournalVoucherInsert_Update(string journalData, string createuser, string finyear,
             string compID, string JournalVoucher_Narration, string JournalVoucherDetail_TransactionDate,
             string JournalVoucher_SettlementNumber, string JournalVoucher_SettlementType, string JournalVoucher_BillNumber, string JournalVoucher_Prefix, string segmentid)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("xmlJournalVoucherInsert_Update");
            proc.AddNTextPara("@journalData", journalData);
            proc.AddNVarcharPara("@createuser", 100, createuser);
            proc.AddNVarcharPara("@finyear", 50, finyear);
            proc.AddNVarcharPara("@compID", 50, compID);
            proc.AddNVarcharPara("@JournalVoucher_Narration", -1, JournalVoucher_Narration);
            proc.AddNVarcharPara("@JournalVoucherDetail_TransactionDate", 50, JournalVoucherDetail_TransactionDate);
            proc.AddNVarcharPara("@JournalVoucher_SettlementNumber", 50, JournalVoucher_SettlementNumber);
            proc.AddNVarcharPara("@JournalVoucher_SettlementType", 50, JournalVoucher_SettlementType);
            proc.AddNVarcharPara("@JournalVoucher_BillNumber", 50, JournalVoucher_BillNumber);
            proc.AddNVarcharPara("@JournalVoucher_Prefix", 50, JournalVoucher_Prefix);
            proc.AddNVarcharPara("@segmentid", 50, segmentid);
            ds = proc.GetDataSet();
            return ds;
        }



        public DataSet Delete_DematCharges(string Segmentid, string FromDate, string ToDate,
                string CompanyID, string Finyear)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Delete_DematCharges");
            proc.AddNVarcharPara("@Segmentid", 50, Segmentid);
            proc.AddNVarcharPara("@FromDate", 50, FromDate);
            proc.AddNVarcharPara("@ToDate", 50, ToDate);
            proc.AddNVarcharPara("@CompanyID", 100, CompanyID);
            proc.AddNVarcharPara("@Finyear", 50, Finyear);
       

            ds = proc.GetDataSet();
            return ds;
        }
        public DataSet Update_DematCharges(string Segmentid, string FromDate, string ToDate,
            string CompanyID, string Finyear)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Update_DematCharges");
            proc.AddNVarcharPara("@Segmentid", 50, Segmentid);
            proc.AddNVarcharPara("@FromDate", 50, FromDate);
            proc.AddNVarcharPara("@ToDate", 50, ToDate);
            proc.AddNVarcharPara("@CompanyID", 100, CompanyID);
            proc.AddNVarcharPara("@Finyear", 50, Finyear);


            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet JournalVoucherPrintFromInsert(string journalData, string TransactionDate)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("JournalVoucherPrintFromInsert");
            proc.AddNTextPara("@journalData", journalData);
            proc.AddNVarcharPara("@TransactionDate", 50, TransactionDate);
          
            ds = proc.GetDataSet();
            return ds;
        }

        public int Delete_JV(string IBRef, string VoucherNumber, int userid)
        {
            int i = 0;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("Delete_JV");
            proc.AddNVarcharPara("@IBRef", 50, IBRef);
            proc.AddNVarcharPara("@VoucherNumber", 50, VoucherNumber);
            proc.AddIntegerPara("@CreateUser", userid);
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;
            //i = proc.RunActionQuery();
            //return i;
        }

        public DataSet Fetch_JVE_DataSet(string CrgExchange, string UserID, string VoucherNumber, string IBRef, string TradeCurrency, string CompanyID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Fetch_JVE_DataSet");
            proc.AddNVarcharPara("@CrgExchange", 50, CrgExchange);
            proc.AddNVarcharPara("@UserID", 50, UserID);
            proc.AddNVarcharPara("@VoucherNumber", 50, VoucherNumber);
            proc.AddNVarcharPara("@IBRef", 50, IBRef);
            proc.AddNVarcharPara("@TradeCurrency", 50, TradeCurrency);
            proc.AddIntegerPara("@CompanyID", Convert.ToInt32(CompanyID));
            ds = proc.GetDataSet();
            return ds;
        }
        public DataSet Sp_DailyBillsFO(string segment, string Companyid, string date, string Finyear, string grpbranch, string ClientsID, string branch)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Sp_DailyBillsFO");
            proc.AddNVarcharPara("@segment", 50, segment);
            proc.AddNVarcharPara("@Companyid", 50, Companyid);
            proc.AddNVarcharPara("@date", 50, date);
            proc.AddNVarcharPara("@Finyear", 50, Finyear);
            proc.AddNVarcharPara("@grpbranch", 50, grpbranch);
            proc.AddNVarcharPara("@ClientsID", -1,ClientsID);
            proc.AddNVarcharPara("@branch", -1, branch);
            ds = proc.GetDataSet();
            return ds;
        }
        public DataSet xmlCashBankInsert(string cashBankData, string createuser, string finyear, string compID, string CashBankName, string TDate, string BRS)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("xmlCashBankInsert");
            proc.AddNTextPara("@cashBankData", cashBankData);
            proc.AddNVarcharPara("@createuser", 50, createuser);
            proc.AddNVarcharPara("@finyear", 50, finyear);
            proc.AddNVarcharPara("@compID", 50, compID);
            proc.AddNVarcharPara("@CashBankName", 512, CashBankName);
            proc.AddNVarcharPara("@TDate", 50, TDate);
            proc.AddIntegerPara("@BRS", Convert.ToInt32(BRS));
            ds = proc.GetDataSet();
            return ds;
        }
        public void xmlCashBankInsertMCX(string cashBankData, string createuser, string finyear, string compID, string CashBankName, string TDate, string BRS)
        {
           
            ProcedureExecute proc = new ProcedureExecute("xmlCashBankInsert");
            proc.AddNTextPara("@cashBankData", cashBankData);
            proc.AddNVarcharPara("@createuser", 50, createuser);
            proc.AddNVarcharPara("@finyear", 50, finyear);
            proc.AddNVarcharPara("@compID", 50, compID);
            proc.AddNVarcharPara("@CashBankName", 512, CashBankName);
            proc.AddNVarcharPara("@TDate", 50, TDate);
            proc.AddIntegerPara("@BRS", Convert.ToInt32(BRS));
            proc.RunActionQuery();
          
        }
        public DataSet ExportPosition_MCXCOMM(string date, string segment, string companyid, string MasterSegment, string ClientsID, string GrpType)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("ExportPosition_MCXCOMM");
            proc.AddNVarcharPara("@date",50, date);
            proc.AddNVarcharPara("@segment", 50, segment);
            proc.AddNVarcharPara("@companyid", 50, companyid);
            proc.AddNVarcharPara("@MasterSegment", 50, MasterSegment);
            proc.AddNVarcharPara("@ClientsID", -1, ClientsID);
            proc.AddNVarcharPara("@GrpType", 50, GrpType);
          
            ds = proc.GetDataSet();
            return ds;
        }
        public void sp_Insert_ExportFiles(string segid, string file_type, string file_name, string userid, string batch_number, string file_path)
        {

            ProcedureExecute proc = new ProcedureExecute("sp_Insert_ExportFiles");
            proc.AddNVarcharPara("@segid", 50, segid);
            proc.AddNVarcharPara("@file_type", 50, file_type);
            proc.AddNVarcharPara("@file_name",200, file_name);
            proc.AddNVarcharPara("@userid", 50, userid);
            proc.AddNVarcharPara("@batch_number", 50, batch_number);
            proc.AddNVarcharPara("@file_path", 200, file_path);

            proc.RunActionQuery();
            
        }



        public DataSet TradeChangeCommCurrencyDISPLAY(string date, string ClientsID, string segment, string Companyid, string Instruments, string OrderNo,
             string Terminalid, string CTCLID, string TIME, string Settno, string Setttype, string MasterSegment, string tradetype, string actype
                 , string actypeCli, string mktprice, string Sortorder, string TranType)
        {

            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("TradeChangeCommCurrencyDISPLAY");

            proc.AddVarcharPara("@date", 100, date);
            proc.AddVarcharPara("@ClientsID", 5000, ClientsID);
            proc.AddVarcharPara("@segment", 100, segment);
            proc.AddVarcharPara("@Companyid", 100, Companyid);
            proc.AddVarcharPara("@Instruments", 5000, Instruments);
            proc.AddVarcharPara("@OrderNo", 5000, OrderNo);
            proc.AddVarcharPara("@Terminalid", 5000, Terminalid);
            proc.AddVarcharPara("@CTCLID", 5000, CTCLID);
            proc.AddVarcharPara("@TIME", 5000, TIME);
            proc.AddVarcharPara("@Settno", 100, Settno);
            proc.AddVarcharPara("@Setttype", 100, Setttype);
            proc.AddVarcharPara("@MasterSegment", 100, MasterSegment);
            proc.AddVarcharPara("@tradetype", 100, tradetype);
            proc.AddVarcharPara("@actype", 100, actype);
            proc.AddVarcharPara("@actypeCli", 5000, actypeCli);
            proc.AddVarcharPara("@mktprice", 5000, mktprice);
            proc.AddVarcharPara("@Sortorder", 100, Sortorder);
            proc.AddVarcharPara("@TranType", 100, TranType);


            ds = proc.GetDataSet();
            return ds;
        }


        public int TRADECHANGECommCurrency(string TABLEReport, string date, string segment, string Companyid, string createuser, string RPTTOEXCHANGE,
            string SETTNO, string SETTTYPE, string FROMCLIENT, string TOCLIENT)
        {
            int i = 0;
            ProcedureExecute proc = new ProcedureExecute("TRADECHANGECommCurrency");
            proc.AddNTextPara("@TABLEReport", TABLEReport);
            proc.AddNVarcharPara("@date", 50, date);
            proc.AddNVarcharPara("@segment", 50, segment);
            proc.AddNVarcharPara("@Companyid", 50, Companyid);
            proc.AddNVarcharPara("@createuser", 50, createuser);
            proc.AddNVarcharPara("@RPTTOEXCHANGE", 50, RPTTOEXCHANGE);
            proc.AddNVarcharPara("@SETTNO", 50, SETTNO);
            proc.AddNVarcharPara("@SETTTYPE", 50, SETTTYPE);
            proc.AddNVarcharPara("@FROMCLIENT", 50, FROMCLIENT);
            proc.AddNVarcharPara("@TOCLIENT", 50, TOCLIENT);
           i= proc.RunActionQuery();
           return i;
        }
        public DataSet TRADEDELETE(string SEGMENT, string FROMDATE, string TODATE, string COMPANYID, string SETTNO, string SETTTYPE,
                                      string TRADETYPE, string EXCHANGESEGMENT, string BRANCHID, string TERMINALID, string CLIENTID, string SCRIPID)
        {

            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("TRADEDELETE");

            proc.AddVarcharPara("@SEGMENT", 100, SEGMENT);
            proc.AddVarcharPara("@FROMDATE", 100, FROMDATE);
            proc.AddVarcharPara("@TODATE", 100, TODATE);
            proc.AddVarcharPara("@COMPANYID", 100, COMPANYID);
            proc.AddVarcharPara("@SETTNO", 100, SETTNO);
            proc.AddVarcharPara("@SETTTYPE", 100, SETTTYPE);
            proc.AddVarcharPara("@TRADETYPE", 100, TRADETYPE);
            proc.AddVarcharPara("@EXCHANGESEGMENT", 5000, EXCHANGESEGMENT);
            proc.AddVarcharPara("@BRANCHID", 5000, BRANCHID);
            proc.AddVarcharPara("@TERMINALID", 5000, TERMINALID);
            proc.AddVarcharPara("@CLIENTID", 5000, CLIENTID);
            proc.AddVarcharPara("@SCRIPID", 5000, SCRIPID);


            ds = proc.GetDataSet();
            return ds;
        }
        public DataSet Sp_SplitTradesComm(string segment, string Companyid, string date, string client, string product, string MasterSegment,
                                     string SettNo, string SetType, string type, string custid)
        {

            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Sp_SplitTradesComm");

            proc.AddVarcharPara("@segment", 50, segment);
            proc.AddVarcharPara("@Companyid", 50, Companyid);
            proc.AddVarcharPara("@date", 50, date);
            proc.AddVarcharPara("@client", 50, client);
            proc.AddVarcharPara("@product", 50, product);
            proc.AddIntegerPara("@MasterSegment",Convert.ToInt32(MasterSegment));
            proc.AddVarcharPara("@SettNo", 50, SettNo);
            proc.AddVarcharPara("@SetType", 50, SetType);
            proc.AddVarcharPara("@type", 50, type);
            proc.AddVarcharPara("@custid", 50, custid);
           

            ds = proc.GetDataSet();
            return ds;
        }


        public DataSet ICEXCOMM_TRADE_PROCESS(string trades_segment, string trades_settlementno, string tradedate, string CreateUser, string companyid, string ClientsID,
                                       string Instrument)
        {

            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("ICEXCOMM_TRADE_PROCESS");

            proc.AddVarcharPara("@trades_segment", 50, trades_segment);
            proc.AddVarcharPara("@trades_settlementno", 50, trades_settlementno);
            proc.AddVarcharPara("@tradedate", 50, tradedate);
            proc.AddIntegerPara("@CreateUser", Convert.ToInt32(CreateUser));
            proc.AddVarcharPara("@companyid", 50, companyid);
            proc.AddVarcharPara("@ClientsID", -1, ClientsID);
            proc.AddVarcharPara("@Instrument", -1, Instrument);
        
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet CheckContractNoForProcessing(string trades_segment, string trades_settlementno, string tradedate, string companyid, string ClientsID, string Instrument,
                                       string SettType, string EXCHANGESEGMENT)
        {

            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("CheckContractNoForProcessing");

            proc.AddVarcharPara("@trades_segment", 50, trades_segment);
            proc.AddVarcharPara("@trades_settlementno", 50, trades_settlementno);
            proc.AddVarcharPara("@tradedate", 50, tradedate);
            proc.AddVarcharPara("@companyid", 50, companyid);
            proc.AddVarcharPara("@ClientsID", -1, ClientsID);
            proc.AddVarcharPara("@Instrument",-1,Instrument);
            proc.AddVarcharPara("@SettType", 50, SettType);
            proc.AddVarcharPara("@EXCHANGESEGMENT", 50, EXCHANGESEGMENT);
          
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet BillingCommCurrencyDelete(string FromDate, string ToDate, string Segment, string Companyid)
        {

            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("BillingCommCurrencyDelete");

            proc.AddVarcharPara("@FromDate", 50, FromDate);
            proc.AddVarcharPara("@ToDate", 50, ToDate);
            proc.AddVarcharPara("@Segment", 50, Segment);
            proc.AddVarcharPara("@Companyid", 50, Companyid);
          
            ds = proc.GetDataSet();
            return ds;
        }
        public DataSet BillingCommCurrencyCheck(string FromDate, string ToDate, string Segment, string Companyid,
            string MasterSegment, string Finyear, string SettNo, string SettType, string CreateUser)
        {

            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("BillingCommCurrencyCheck");

            proc.AddVarcharPara("@FromDate", 50, FromDate);
            proc.AddVarcharPara("@ToDate", 50, ToDate);
            proc.AddVarcharPara("@Segment", 50, Segment);
            proc.AddVarcharPara("@Companyid", 50, Companyid);
            proc.AddVarcharPara("@MasterSegment", 50, MasterSegment);
            proc.AddVarcharPara("@Finyear", 50, Finyear);
            proc.AddVarcharPara("@SettNo", 50, SettNo);
            proc.AddVarcharPara("@SettType", 50, SettType);
            proc.AddVarcharPara("@CreateUser", 50, CreateUser);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet BillingCommCurrencyDateWise(string FromDate, string ToDate, string Segment, string Companyid,
           string MasterSegment, string Finyear, string SettNo, string SettType, string CreateUser)
        {

            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("BillingCommCurrencyDateWise");

            proc.AddVarcharPara("@FromDate", 50, FromDate);
            proc.AddVarcharPara("@ToDate", 50, ToDate);
            proc.AddVarcharPara("@Segment", 50, Segment);
            proc.AddVarcharPara("@Companyid", 50, Companyid);
            proc.AddVarcharPara("@MasterSegment", 50, MasterSegment);
            proc.AddVarcharPara("@Finyear", 50, Finyear);
            proc.AddVarcharPara("@SettNo", 50, SettNo);
            proc.AddVarcharPara("@SettType", 50, SettType);
            proc.AddVarcharPara("@CreateUser", 50, CreateUser);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet MarginCalculationCommodity(string date, string segment, string Companyid, string MasterSegment, string ClientsID, string ModifyUser, string FinancialYear)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("MarginCalculationCommodity");
            proc.AddNVarcharPara("@date", 50, date);
            proc.AddNVarcharPara("@segment", 50, segment);
            proc.AddNVarcharPara("@Companyid", 50, Companyid);
            proc.AddNVarcharPara("@MasterSegment", 50, MasterSegment);
            proc.AddNVarcharPara("@ClientsID", -1, ClientsID);
            proc.AddNVarcharPara("@ModifyUser",50,ModifyUser);
            proc.AddNVarcharPara("@FinancialYear", 50, FinancialYear);
            ds = proc.GetDataSet();
            return ds;
        }



        public DataSet Sp_CTTGenerationCOMM(string segment, string date, string Companyid, string ClientsID, string MasterSegment, string createuser, string SetNo, string SetType, string forcntrnogenerate)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Sp_CTTGenerationCOMM");
            proc.AddNVarcharPara("@segment", 50, segment);
            proc.AddNVarcharPara("@date", 50, date);
            proc.AddNVarcharPara("@Companyid", 50, Companyid);
            proc.AddNVarcharPara("@ClientsID", -1, ClientsID);
            proc.AddNVarcharPara("@MasterSegment", 50, MasterSegment);
            proc.AddNVarcharPara("@createuser", 50, createuser);
            proc.AddNVarcharPara("@SetNo", 50, SetNo);
            proc.AddNVarcharPara("@SetType", 50, SetType);
            proc.AddNVarcharPara("@forcntrnogenerate", 50, forcntrnogenerate);
            ds = proc.GetDataSet();
            return ds;
        }


        public void SP_INSUP_NSECMClosingRates(string Module, string FilePath, string Date)
        {
           
            ProcedureExecute proc = new ProcedureExecute("SP_INSUP_NSECMClosingRates");
            proc.AddNVarcharPara("@Module",100, Module);
            proc.AddNVarcharPara("@FilePath",255, FilePath);
            proc.AddNVarcharPara("@Date", 50, Date);
        
      proc.RunActionQuery();
          
        }



        public void XML_NSEApprovedSecurities(string XML, string Date, string CompanyID, string ModifyUser)
        {
           
            ProcedureExecute proc = new ProcedureExecute("XML_NSEApprovedSecurities");
            proc.AddNTextPara("@XML",  XML);
            proc.AddNVarcharPara("@Date", 100, Date);
            proc.AddNVarcharPara("@CompanyID", 50, CompanyID);
            proc.AddIntegerPara("@ModifyUser", Convert.ToInt32(ModifyUser));

            proc.RunActionQuery();
           
        }
        public DataSet Fetch_BankTransactionDetails(string BankID, string TransactionDate, string ExchSeg, string PrintDateTime, string Checknull, string BankName, string Narration, string Companyid)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Fetch_BankTransactionDetails");
            proc.AddNVarcharPara("@BankID", 50, BankID);
            proc.AddNVarcharPara("@TransactionDate", 50, TransactionDate);
            proc.AddNVarcharPara("@ExchSeg", 50, ExchSeg);
            proc.AddNVarcharPara("@PrintDateTime", -1, PrintDateTime);
            proc.AddNVarcharPara("@Checknull", 10, Checknull);
            proc.AddNVarcharPara("@BankName", 50, BankName);
            proc.AddNVarcharPara("@Narration", -1, Narration);
            proc.AddNVarcharPara("@Companyid", 50, Companyid);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet Fetch_BankTransaction(string BankID, string ExchSeg, string TransactionDate, string BankName)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Fetch_BankTransaction");
            proc.AddNVarcharPara("@BankID", 50, BankID);
            proc.AddNVarcharPara("@ExchSeg", 50, ExchSeg);
            proc.AddNVarcharPara("@TransactionDate", 50, TransactionDate);
            proc.AddNVarcharPara("@BankName", 50, BankName);
          
            ds = proc.GetDataSet();
            return ds;
        }
        public DataSet YearEndOpeningStocks(string Companyid, string Segment, string FinYear, string ClientType,
            string PostClient, string CloseMethod, string Client, string PostSegment, string CreatUser, string ValuationDate)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("YearEndOpeningStocks");
            proc.AddNVarcharPara("@Companyid", 50, Companyid);
            proc.AddNVarcharPara("@Segment", 50, Segment);
            proc.AddNVarcharPara("@FinYear", 50, FinYear);
            proc.AddNVarcharPara("@ClientType", 100, ClientType);
            proc.AddNVarcharPara("@PostClient", 50, PostClient);
            proc.AddNVarcharPara("@CloseMethod", 50, CloseMethod);
            proc.AddNVarcharPara("@Client", -1, Client);
            proc.AddNVarcharPara("@PostSegment", 50, PostSegment);
            proc.AddNVarcharPara("@CreatUser", 50, CreatUser);
            proc.AddNVarcharPara("@ValuationDate", 50, ValuationDate);
            ds = proc.GetDataSet();
            return ds;
        }


        public DataSet Report_Exchangemarginreportingfile_MCXFO(string segment, string Companyid, string Finyear, string branch, string date, string CValuationdate, string CHoldingdate,
            string SYSTM1date, string SYSTM2date, string Considercmseg, string chkhaircut, string chkunapproved, string unapprovedshares, string IntialMargin)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("[Report_ExchangeMarginReportingFile_Com]");
            proc.AddVarcharPara("@segment", 300, segment);
            proc.AddVarcharPara("@Companyid", 100, Companyid);
            proc.AddVarcharPara("@Finyear", 300, Finyear);
            proc.AddVarcharPara("@branch", -1, branch);
            proc.AddVarcharPara("@date", 300, date);
            proc.AddVarcharPara("@CValuationdate", 100, CValuationdate);
            proc.AddVarcharPara("@CHoldingdate", 300, CHoldingdate);
            proc.AddVarcharPara("@SYSTM1date", 100, SYSTM1date);
            proc.AddVarcharPara("@SYSTM2date", 300, SYSTM2date);
            proc.AddVarcharPara("@Considercmseg", 100, Considercmseg);
            proc.AddVarcharPara("@chkhaircut", 300, chkhaircut);
            proc.AddVarcharPara("@chkunapproved", 300, chkunapproved);
            proc.AddDecimalPara("@unapprovedshares", 2, 28, Convert.ToDecimal(unapprovedshares));
            proc.AddNVarcharPara("@IntialMargin", 100, IntialMargin);


            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet Report_Exchangemarginreportingfile_MCXFOCM(string segment, string Companyid, string Finyear, string branch, string date, string CValuationdate, string CHoldingdate,
          string SYSTM1date, string SYSTM2date, string Considercmseg, string chkhaircut, string chkunapproved, string unapprovedshares, string IntialMargin)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("[Report_Exchangemarginreportingfile_MCXFOCM]");
            proc.AddVarcharPara("@segment", 300, segment);
            proc.AddVarcharPara("@Companyid", 100, Companyid);
            proc.AddVarcharPara("@Finyear", 300, Finyear);
            proc.AddVarcharPara("@branch", -1, branch);
            proc.AddVarcharPara("@date", 300, date);
            proc.AddVarcharPara("@CValuationdate", 100, CValuationdate);
            proc.AddVarcharPara("@CHoldingdate", 300, CHoldingdate);
            proc.AddVarcharPara("@SYSTM1date", 100, SYSTM1date);
            proc.AddVarcharPara("@SYSTM2date", 300, SYSTM2date);
            proc.AddVarcharPara("@Considercmseg", 100, Considercmseg);
            proc.AddVarcharPara("@chkhaircut", 300, chkhaircut);
            proc.AddVarcharPara("@chkunapproved", 300, chkunapproved);
            proc.AddDecimalPara("@unapprovedshares",  2,28, Convert.ToDecimal(unapprovedshares));
            proc.AddNVarcharPara("@IntialMargin", 100, IntialMargin);


            ds = proc.GetDataSet();
            return ds;
        }
        public DataSet ExportPosition_NCDXCOMM(string date, string segment, string companyid, string MasterSegment, string ClientsID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("ExportPosition_NCDXCOMM");
            proc.AddNVarcharPara("@date", 50, date);
            proc.AddNVarcharPara("@segment", 50, segment);
            proc.AddNVarcharPara("@companyid", 50, companyid);
            proc.AddNVarcharPara("@MasterSegment", 50, MasterSegment);
            proc.AddNVarcharPara("@ClientsID", -1, ClientsID);
          
            ds = proc.GetDataSet();
            return ds;
        }


        public int ICEXCOMM_trade_processing(string trades_segment, string trades_settlementno, string tradedate, string CreateUser, string Companyid, string ClientsID,
                                      string Instrument)
        {

            int ret = 0;
            ProcedureExecute proc = new ProcedureExecute("ICEXCOMM_trade_processing");

            proc.AddVarcharPara("@trades_segment", 50, trades_segment);
            proc.AddVarcharPara("@trades_settlementno", 50, trades_settlementno);
            proc.AddVarcharPara("@tradedate", 50, tradedate);
            proc.AddIntegerPara("@CreateUser", Convert.ToInt32(CreateUser));
            proc.AddVarcharPara("@Companyid", 50, Companyid);
            proc.AddVarcharPara("@ClientsID", -1, ClientsID);
            proc.AddVarcharPara("@Instrument", -1, Instrument);

            ret = proc.RunActionQuery();
            return ret;
        }

        public DataSet CheckContractNo_Trade_TEST(string trades_segment, string trades_settlementno,string trades_settlementtype, string tradedate, string Companyid, string ClientsID,
                                     int MasterSegment, string Type)
        {

            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("CheckContractNo_Trade_TEST");

            proc.AddVarcharPara("@trades_segment", 50, trades_segment);
            proc.AddVarcharPara("@trades_settlementno", 30, trades_settlementno);
            proc.AddVarcharPara("@trades_settlementtype", 20, trades_settlementtype);
            proc.AddVarcharPara("@tradedate", 50, tradedate);
            proc.AddVarcharPara("@Companyid", 50, Companyid);
            proc.AddVarcharPara("@ClientsID", -1, ClientsID);
            proc.AddIntegerPara("@MasterSegment", MasterSegment);
            proc.AddVarcharPara("@Type", -1, Type);
            ds = proc.GetDataSet();
            return ds;
        }
    }
}

