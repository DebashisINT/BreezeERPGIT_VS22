using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessLayer;
using System.Data;

namespace BusinessLogicLayer
{
   public class FAReportsOther
   {
       public DataSet Fetch_LedgerView(string CompanyID, string FinYear, string FromDate, string ToDate, string MainAccount, string SubAccount, string Branch, string ReportType, string Segment, string UserID, string Settlement, string TranType, string CbPayment, string CbReceipt, string CbContract, string JvType, string SelectedJv, string ActiveCurrency, string TradeCurrency)
       {
           DataSet ds = new DataSet();
           ProcedureExecute proc = new ProcedureExecute("Fetch_LedgerView");
           proc.AddVarcharPara("@CompanyID", 50, CompanyID);
           proc.AddVarcharPara("@FinYear", 50, FinYear);
           proc.AddVarcharPara("@FromDate", 50, FromDate);
           proc.AddVarcharPara("@ToDate", 50, ToDate);
           proc.AddVarcharPara("@MainAccount", -1, MainAccount);
           proc.AddVarcharPara("@SubAccount", -1, SubAccount);
           proc.AddVarcharPara("@Branch", -1, Branch);
           proc.AddVarcharPara("@ReportType", 50, ReportType);
           proc.AddVarcharPara("@Segment", -1, Segment);
           proc.AddVarcharPara("@UserID", 50, UserID);
           proc.AddVarcharPara("@Settlement", -1, Settlement);
           proc.AddVarcharPara("@TranType", 20, TranType);
           proc.AddVarcharPara("@CbPayment", 20, CbPayment);
           proc.AddVarcharPara("@CbReceipt", 20, CbReceipt);
           proc.AddVarcharPara("@CbContract", 20, CbContract);
           proc.AddVarcharPara("@JvType", 20, JvType);
           proc.AddVarcharPara("@SelectedJv", 1000, SelectedJv);
           proc.AddVarcharPara("@ActiveCurrency", 10, ActiveCurrency);
           proc.AddVarcharPara("@TradeCurrency", 10, TradeCurrency);
           ds = proc.GetDataSet();
           return ds;

       }

       public DataSet Fetch_LedgerForCrystalReport(string CompanyID, string FinYear, string FromDate, string ToDate, string MainAccount, string SubAccount, string Branch, string ReportType, string Segment, string UserID, string Settlement, string SubledgerType, string TranType, string CbPayment, string CbReceipt, string CbContract, string JvType, string SelectedJv)
       {
           DataSet ds = new DataSet();
           ProcedureExecute proc = new ProcedureExecute("Fetch_LedgerForCrystalReport");
           proc.AddVarcharPara("@CompanyID", 50, CompanyID);
           proc.AddVarcharPara("@FinYear", 50, FinYear);
           proc.AddVarcharPara("@FromDate", 50, FromDate);
           proc.AddVarcharPara("@ToDate", 50, ToDate);
           proc.AddVarcharPara("@MainAccount", -1, MainAccount);
           proc.AddVarcharPara("@SubAccount", -1, SubAccount);
           proc.AddVarcharPara("@Branch", -1, Branch);
           proc.AddVarcharPara("@ReportType", 50, ReportType);
           proc.AddVarcharPara("@Segment", -1, Segment);
           proc.AddVarcharPara("@UserID", 50, UserID);
           proc.AddVarcharPara("@Settlement", -1, Settlement);
           proc.AddVarcharPara("@SubledgerType", 250, SubledgerType);
           proc.AddVarcharPara("@TranType", 20, TranType);
           proc.AddVarcharPara("@CbPayment", 20, CbPayment);
           proc.AddVarcharPara("@CbReceipt", 20, CbReceipt);
           proc.AddVarcharPara("@CbContract", 20, CbContract);
           proc.AddVarcharPara("@JvType", 20, JvType);
           proc.AddVarcharPara("@SelectedJv", 1000, SelectedJv);
          
           ds = proc.GetDataSet();
           return ds;




       }


       public DataSet Fetch_LedgerForSendEmail(string CompanyID, string FinYear, string FromDate, string ToDate, string MainAccount, string SubAccount, string Branch, string ReportType, string Segment, string UserID,string SubledgerType, string Settlement,string BranchGroupInd, string TranType, string CbPayment, string CbReceipt, string CbContract, string JvType, string SelectedJv, string ActiveCurrency, string TradeCurrency)
       {
           DataSet ds = new DataSet();
           ProcedureExecute proc = new ProcedureExecute("Fetch_LedgerForSendEmail");
           proc.AddVarcharPara("@CompanyID", 50, CompanyID);
           proc.AddVarcharPara("@FinYear", 50, FinYear);
           proc.AddVarcharPara("@FromDate", 50, FromDate);
           proc.AddVarcharPara("@ToDate", 50, ToDate);
           proc.AddVarcharPara("@MainAccount", -1, MainAccount);
           proc.AddVarcharPara("@SubAccount", -1, SubAccount);
           proc.AddVarcharPara("@Branch", -1, Branch);
           proc.AddVarcharPara("@ReportType", 50, ReportType);
           proc.AddVarcharPara("@Segment", -1, Segment);
           proc.AddVarcharPara("@UserID", 50, UserID);
           proc.AddVarcharPara("@SubledgerType", 250, SubledgerType);
           proc.AddVarcharPara("@Settlement", -1, Settlement);
           proc.AddVarcharPara("@BranchGroupInd", 50, BranchGroupInd);
          
           proc.AddVarcharPara("@TranType", 20, TranType);
           proc.AddVarcharPara("@CbPayment", 20, CbPayment);
           proc.AddVarcharPara("@CbReceipt", 20, CbReceipt);
           proc.AddVarcharPara("@CbContract", 20, CbContract);
           proc.AddVarcharPara("@JvType", 20, JvType);
           proc.AddVarcharPara("@SelectedJv", 1000, SelectedJv);
           proc.AddVarcharPara("@ActiveCurrency", 10, ActiveCurrency);
           proc.AddVarcharPara("@TradeCurrency", 10, TradeCurrency);
           ds = proc.GetDataSet();
           return ds;

           }


       public DataSet AccountsLedgerReport_Cryatal(string CompanyID, string segmentID, string DateFrom, string DateTo, string MainAccountSearch, string Branch,string SubAccountSearch, string Typeval, string MainAcID, string SubAcID, string FinYear, string SingleDouble, string SegmanetName)
       {
           DataSet ds = new DataSet();
           ProcedureExecute proc = new ProcedureExecute("AccountsLedgerReport_Cryatal");
           proc.AddVarcharPara("@CompanyID", 50, CompanyID);
           proc.AddVarcharPara("@segmentID", 100, segmentID);
           proc.AddVarcharPara("@DateFrom", 50, DateFrom);
           proc.AddVarcharPara("@DateTo", 50, DateTo);
           proc.AddVarcharPara("@MainAccountSearch", -1, MainAccountSearch);
           proc.AddVarcharPara("@Branch", -1, Branch);
           proc.AddVarcharPara("@SubAccountSearch", -1, SubAccountSearch);
           proc.AddVarcharPara("@Type", 20, Typeval);
           proc.AddVarcharPara("@MainAcID", 100, MainAcID);
           proc.AddVarcharPara("@SubAcID", -1, SubAcID);
           proc.AddVarcharPara("@FinYear", 10, FinYear);
           proc.AddVarcharPara("@SingleDouble", 10, SingleDouble);
           proc.AddVarcharPara("@SegmanetName", 150, SegmanetName);

           ds = proc.GetDataSet();
           return ds;

       }



       public DataSet Fetch_LedgerForopeningblnc1(string CompanyID, string FinYear, string FromDate, string ToDate, string MainAccount, string SubAccount, string Branch, string ReportType, string Segment, string UserID, string Settlement, string SubledgerType, string TranType, string CbPayment, string CbReceipt, string CbContract, string JvType, string SelectedJv,string ActiveCurrency,string TradeCurrency)
       {
           DataSet ds = new DataSet();
           ProcedureExecute proc = new ProcedureExecute("Fetch_LedgerForopeningblnc1");
           proc.AddVarcharPara("@CompanyID", 50, CompanyID);
           proc.AddVarcharPara("@FinYear", 50, FinYear);
           proc.AddVarcharPara("@FromDate", 50, FromDate);
           proc.AddVarcharPara("@ToDate", 50, ToDate);
           proc.AddVarcharPara("@MainAccount", -1, MainAccount);
           proc.AddVarcharPara("@SubAccount", -1, SubAccount);
           proc.AddVarcharPara("@Branch", -1, Branch);
           proc.AddVarcharPara("@ReportType", 50, ReportType);
           proc.AddVarcharPara("@Segment", -1, Segment);
           proc.AddVarcharPara("@UserID", 50, UserID);
           proc.AddVarcharPara("@Settlement", -1, Settlement);
           proc.AddVarcharPara("@SubledgerType", 250, SubledgerType);
           proc.AddVarcharPara("@TranType", 20, TranType);
           proc.AddVarcharPara("@CbPayment", 20, CbPayment);
           proc.AddVarcharPara("@CbReceipt", 20, CbReceipt);
           proc.AddVarcharPara("@CbContract", 20, CbContract);
           proc.AddVarcharPara("@JvType", 20, JvType);
           proc.AddVarcharPara("@SelectedJv", 1000, SelectedJv);
           proc.AddVarcharPara("@ActiveCurrency", 10, ActiveCurrency);
           proc.AddVarcharPara("@TradeCurrency", 10, TradeCurrency);
           ds = proc.GetDataSet();
           return ds;


       }


       public DataSet Fetch_LedgerFordosprintall(string CompanyID, string FinYear, string FromDate, string ToDate, string MainAccount, string SubAccount, string Branch, string ReportType, string Segment, string UserID, string Settlement, string SubledgerType, string TranType, string CbPayment, string CbReceipt, string CbContract, string JvType, string SelectedJv,string Header,string Footer)
       {
           DataSet ds = new DataSet();
           ProcedureExecute proc = new ProcedureExecute("Fetch_LedgerFordosprintall");
           proc.AddVarcharPara("@CompanyID", 50, CompanyID);
           proc.AddVarcharPara("@FinYear", 50, FinYear);
           proc.AddVarcharPara("@FromDate", 50, FromDate);
           proc.AddVarcharPara("@ToDate", 50, ToDate);
           proc.AddVarcharPara("@MainAccount", -1, MainAccount);
           proc.AddVarcharPara("@SubAccount", -1, SubAccount);
           proc.AddVarcharPara("@Branch", -1, Branch);
           proc.AddVarcharPara("@ReportType", 50, ReportType);
           proc.AddVarcharPara("@Segment", -1, Segment);
           proc.AddVarcharPara("@UserID", 50, UserID);
           proc.AddVarcharPara("@Settlement", -1, Settlement);
           proc.AddVarcharPara("@SubledgerType", 250, SubledgerType);
           proc.AddVarcharPara("@TranType", 20, TranType);
           proc.AddVarcharPara("@CbPayment", 20, CbPayment);
           proc.AddVarcharPara("@CbReceipt", 20, CbReceipt);
           proc.AddVarcharPara("@CbContract", 20, CbContract);
           proc.AddVarcharPara("@JvType", 20, JvType);
           proc.AddVarcharPara("@SelectedJv", 1000, SelectedJv);
           proc.AddVarcharPara("@Header", -1, Header);
           proc.AddVarcharPara("@Footer", -1, Footer);
           ds = proc.GetDataSet();
           return ds;

       }
       public DataSet InterestPenaltyStatement(string Fromdate, string ToDate, string Segment, string MainAccount, string SubAccount, string GenerateFor, string Debit, string Credit, string GracePeriod, string StatementTypeVal, string CompID, string branchID, string FinYear, string GrpBranchType, string GroupID, string SingleDouble, string SubLedgerType, string AddType)
       {
           DataSet ds = new DataSet();
           ProcedureExecute proc = new ProcedureExecute("InterestPenaltyStatement");
           proc.AddVarcharPara("@Fromdate", 50, Fromdate);
           proc.AddVarcharPara("@ToDate", 50, ToDate);
           proc.AddVarcharPara("@Segment", 50, Segment);
           proc.AddVarcharPara("@MainAccount", 50, MainAccount);
           proc.AddVarcharPara("@SubAccount", -1, SubAccount);
           proc.AddVarcharPara("@GenerateFor", 10, GenerateFor);
           proc.AddVarcharPara("@Debit", 50, Debit);
           proc.AddVarcharPara("@Credit", 50,Credit);
           proc.AddIntegerPara("@GracePeriod", Convert.ToInt32(GracePeriod));
           proc.AddVarcharPara("@StatementType", 10, StatementTypeVal);
           proc.AddVarcharPara("@CompID", 20, CompID);
           proc.AddVarcharPara("@branchID", -1, branchID);
           proc.AddVarcharPara("@FinYear", 20, FinYear);
           proc.AddVarcharPara("@GrpBranchType", 10, GrpBranchType);
           proc.AddVarcharPara("@GroupID", -1, GroupID);
           proc.AddVarcharPara("@SingleDouble", 10, SingleDouble);
           proc.AddVarcharPara("@SubLedgerType", 50, SubLedgerType);
           proc.AddVarcharPara("@AddType", 10, AddType);
         
           ds = proc.GetDataSet();
           return ds;


       }

       public DataSet Report_BRSNew(string ReportView, string CompanyId, string BanckAc, string ConsiderDate, string Date, string FinYear)
       {
           DataSet ds = new DataSet();
           ProcedureExecute proc = new ProcedureExecute("Report_BRSNew");
           proc.AddVarcharPara("@ReportView", 10, ReportView);
           proc.AddVarcharPara("@CompanyId", -1, CompanyId);
           proc.AddVarcharPara("@BanckAc", -1, BanckAc);
           proc.AddVarcharPara("@ConsiderDate", 10, ConsiderDate);
           proc.AddVarcharPara("@Date", 50, Date);
           proc.AddVarcharPara("@FinYear", 20, FinYear);         
           ds = proc.GetDataSet();
           return ds;


       }

       public DataSet Report_AgeingAnalysis(string ClientType, string ClientId, string Date, string BranchHierchy, string GrpType, string GrpId,
         string Segment, string Companyid, string FinYear, string AgeCalculationBasis, string AcType, string AgeGroupDays,
           string NoOfCol, string ApplyHaircut, decimal DebitMoreThan, string BalnType, string ColumnMarginDep, string ColumnCollateral,
            string ColumnDP, string ColumnPendgPur, string ColumnPendgSale)
       {
           DataSet ds = new DataSet();
           ProcedureExecute proc = new ProcedureExecute("Report_AgeingAnalysis");
           proc.AddVarcharPara("@ClientType", 10, ClientType);
           proc.AddVarcharPara("@ClientId", -1, ClientId);
           proc.AddVarcharPara("@Date", 50, Date);
           proc.AddVarcharPara("@BranchHierchy", -1, BranchHierchy);
           proc.AddVarcharPara("@GrpType", 50, GrpType);
           proc.AddVarcharPara("@GrpId", -1, GrpId);
           proc.AddVarcharPara("@Segment", 50, Segment);
           proc.AddVarcharPara("@Companyid", 50, Companyid);
           proc.AddVarcharPara("@FinYear", 50, FinYear);
           proc.AddVarcharPara("@AgeCalculationBasis", 50, AgeCalculationBasis);
           proc.AddVarcharPara("@AcType", 50, AcType);
           proc.AddVarcharPara("@AgeGroupDays", 50, AgeGroupDays);
           proc.AddIntegerPara("@NoOfCol",  Convert.ToInt32(NoOfCol));
           proc.AddVarcharPara("@ApplyHaircut", 50, ApplyHaircut);
           proc.AddIntegerPara("@DebitMoreThan", Convert.ToInt32(DebitMoreThan));
         
               //proc.AddDecimalPara("@DebitMoreThan", 28,6, Convert.ToDecimal(DebitMoreThan));
           proc.AddVarcharPara("@BalnType", 50, BalnType);
           proc.AddVarcharPara("@ColumnMarginDep", 50, ColumnMarginDep);
           proc.AddVarcharPara("@ColumnCollateral", 50, ColumnCollateral);
           proc.AddVarcharPara("@ColumnDP", 50, ColumnDP);
           proc.AddVarcharPara("@ColumnPendgPur", 50, ColumnPendgPur);
           proc.AddVarcharPara("@ColumnPendgSale", 20, ColumnPendgSale);
           ds = proc.GetDataSet();
           return ds;



       }

       public DataSet CashBankVoucherPrint(string BankName, string FromDate, string ToDate, string Typeval, string Debit, string Credit,
          string Contra, string Segment, string Mode, string FromChequeNumber, string ToChequeNumber, string CallFromCashBankEntry, string IBRefString, string CBDetailsIds)
       {
           DataSet ds = new DataSet();
           ProcedureExecute proc = new ProcedureExecute("CashBankVoucherPrint");
           proc.AddVarcharPara("@BankName", 50, BankName);
           proc.AddVarcharPara("@FromDate", 50, FromDate);
           proc.AddVarcharPara("@ToDate", 50, ToDate);
           proc.AddVarcharPara("@Type", 50, Typeval);
           proc.AddVarcharPara("@Debit", 50, Debit);
           proc.AddVarcharPara("@Credit", 50, Credit);
           proc.AddVarcharPara("@Contra", 50, Contra);
           proc.AddVarcharPara("@Segment", 50, Segment);
           proc.AddVarcharPara("@Mode", 50, Mode);
           proc.AddVarcharPara("@FromChequeNumber", 50, FromChequeNumber);
           proc.AddVarcharPara("@ToChequeNumber", 50, ToChequeNumber);
           proc.AddVarcharPara("@CallFromCashBankEntry", -1, CallFromCashBankEntry);
           proc.AddVarcharPara("@IBRefString", 50, IBRefString);
           proc.AddVarcharPara("@CBDetailsIds", -1, CBDetailsIds);   
           ds = proc.GetDataSet();
           return ds;



         
       }

       public DataSet JournalVoucherPrint(string FromDate, string ToDate, string SegmentID, string TypeVal, string Typedata)
       {
           DataSet ds = new DataSet();
           ProcedureExecute proc = new ProcedureExecute("JournalVoucherPrint");
           proc.AddVarcharPara("@FromDate", 50, FromDate);
           proc.AddVarcharPara("@ToDate", 50, ToDate);
           proc.AddIntegerPara("@SegmentID",Convert.ToInt32(SegmentID));
           proc.AddVarcharPara("@Type", 50, TypeVal);
           proc.AddVarcharPara("@Typedata", 50, Typedata);          
           ds = proc.GetDataSet();
           return ds;


       }
       public DataSet JournalVoucherPrintSelected(string FromDate, string ToDate, string SegmentID, string TypeVal, string SelectedJvs)
       {
           DataSet ds = new DataSet();
           ProcedureExecute proc = new ProcedureExecute("JournalVoucherPrint");
           proc.AddVarcharPara("@FromDate", 50, FromDate);
           proc.AddVarcharPara("@ToDate", 50, ToDate);
           proc.AddIntegerPara("@SegmentID", Convert.ToInt32(SegmentID));
           proc.AddVarcharPara("@Type", 50, TypeVal);
           proc.AddVarcharPara("@SelectedJvs", -1, SelectedJvs);
           ds = proc.GetDataSet();
           return ds;


       }

       public DataSet Fetch_CBE_DataSet(string UserID, string IBRef, string TransactionType, string TradeCurrency)
       {
           DataSet ds = new DataSet();
           ProcedureExecute proc = new ProcedureExecute("Fetch_CBE_DataSet");
           proc.AddVarcharPara("@UserID", 20, UserID);
           proc.AddVarcharPara("@IBRef", 50, IBRef);
           proc.AddVarcharPara("@TransactionType",10, TransactionType);
           proc.AddIntegerPara("@TradeCurrency", Convert.ToInt32(TradeCurrency));         
           ds = proc.GetDataSet();
           return ds;
         

       }

       public DataSet InsertCashBankVoucherEntry(string CashBankXML, string CashBankID, string CreateUser, string FinYear, string CompanyID, string TransactionDate,
            string ExchangeSegmentID, string TransactionType, string EntryUserProfile, string Narration, string FormMode, string OldIBRef, string IsVoucherNumberChange,
            int CurrencyID, out string OutIBRef)
       {
           DataSet ds = new DataSet();
           ProcedureExecute proc = new ProcedureExecute("Insert_CashBankVoucherEntry");
           proc.AddVarcharPara("@CashBankXML", -1, CashBankXML);
           proc.AddNVarcharPara("@CashBankID", 100, CashBankID);
           proc.AddNVarcharPara("@CreateUser", 100, CreateUser);
           proc.AddNVarcharPara("@FinYear", 100, FinYear);
           proc.AddVarcharPara("@CompanyID", 100, CompanyID);
           proc.AddNVarcharPara("@TransactionDate", 100, TransactionDate);
           proc.AddNVarcharPara("@ExchangeSegmentID", 100, ExchangeSegmentID);
           proc.AddNVarcharPara("@TransactionType", 100, TransactionType);
           proc.AddVarcharPara("@EntryUserProfile", 100, EntryUserProfile);
           proc.AddNVarcharPara("@Narration", 100, Narration);
           proc.AddNVarcharPara("@FormMode", 100, FormMode);
           proc.AddNVarcharPara("@OldIBRef", 100, OldIBRef);
           proc.AddVarcharPara("@IsVoucherNumberChange", 100, IsVoucherNumberChange);
           proc.AddIntegerPara("@CurrencyID", CurrencyID);
           proc.AddNVarcharPara("@TransactionType", 100, TransactionType);
           proc.AddVarcharPara("@OutIBRef", 100, "", QueryParameterDirection.Output);
           OutIBRef = proc.GetParaValue("@OutIBRef").ToString();
           ds = proc.GetDataSet();
           return ds;
       }

       public DataSet Search_JournalVoucher(string FinYear, string CompanyID, string ExchSegmentID, string QueryAdPart)
       {
           DataSet ds = new DataSet();
           ProcedureExecute proc = new ProcedureExecute("Search_JournalVoucher");
           proc.AddVarcharPara("@FinYear", 50, FinYear);
           proc.AddVarcharPara("@CompanyID", 50, CompanyID);
           proc.AddVarcharPara("@ExchSegmentID", 50, ExchSegmentID);
           proc.AddVarcharPara("@QueryAdPart", -1, QueryAdPart);
           ds = proc.GetDataSet();
           return ds;
          
       }

       public DataSet Fetch_JVE_DataSet(string CrgExchange, string UserID, string VoucherNumber, string IBRef, string TradeCurrency, string CompanyID)
       {
           DataSet ds = new DataSet();
           ProcedureExecute proc = new ProcedureExecute("Fetch_JVE_DataSet");
           proc.AddVarcharPara("@CrgExchange", 50, CrgExchange);
           proc.AddVarcharPara("@UserID", 50, UserID);
           proc.AddVarcharPara("@VoucherNumber", 50, VoucherNumber);
           proc.AddVarcharPara("@IBRef", 50, IBRef);
           proc.AddIntegerPara("@TradeCurrency", Convert.ToInt32(TradeCurrency));
           proc.AddVarcharPara("@CompanyID", 50, CompanyID);
           ds = proc.GetDataSet();
           return ds;

       }
       public int Delete_JV(string IBRef, string VoucherNumber)
       {
           int i;
           DataSet ds = new DataSet();
           ProcedureExecute proc = new ProcedureExecute("Delete_JV");
           proc.AddVarcharPara("@IBRef", 50, IBRef);
           proc.AddVarcharPara("@VoucherNumber", 50, VoucherNumber);          
           i = proc.RunActionQuery();
           return i;

         
       }
       public DataTable Sp_ObligationStatementCMCLIENT(string fordate, string segment, string Companyid, string BranchId, string SettNo, string SettType, string ClientsID)
       {
           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("Sp_ObligationStatementCMCLIENT");
           proc.AddVarcharPara("@fordate", 50, fordate);
           proc.AddVarcharPara("@segment", 50, segment);
           proc.AddVarcharPara("@Companyid", 50, Companyid);
           proc.AddVarcharPara("@BranchId", -1, BranchId);
           proc.AddVarcharPara("@SettNo", 50, SettNo);
           proc.AddVarcharPara("@SettType", 50, SettType);
           proc.AddVarcharPara("@ClientsID", -1, ClientsID);
           dt = proc.GetTable();
           return dt;

       }

       public DataSet Sp_ObligationStatementCM(string segment, string Companyid, string MasterSegment, string ClientsID, string fordate, string SettNo, string SettType)
       {
           DataSet ds = new DataSet();
           ProcedureExecute proc = new ProcedureExecute("Sp_ObligationStatementCM");
           proc.AddVarcharPara("@segment", 50, segment);
           proc.AddVarcharPara("@Companyid", 50, Companyid);
           proc.AddVarcharPara("@MasterSegment", 50, MasterSegment);
           proc.AddVarcharPara("@ClientsID", 50, ClientsID);
           proc.AddVarcharPara("@fordate", 50, fordate);
           proc.AddVarcharPara("@SettNo", 50, SettNo);
           proc.AddVarcharPara("@SettType", 50, SettType);
           ds = proc.GetDataSet();
           return ds;

       }


       public DataSet Sp_ObligationStatementCM_CRYSTAL(string segment, string Companyid, string MasterSegment, string ClientsID, string fordate, string SettNo, string SettType)
       {
           DataSet ds = new DataSet();
           ProcedureExecute proc = new ProcedureExecute("Sp_ObligationStatementCM_CRYSTAL");
           proc.AddVarcharPara("@segment", 50, segment);
           proc.AddVarcharPara("@Companyid", 50, Companyid);
           proc.AddVarcharPara("@MasterSegment", 50, MasterSegment);
           proc.AddVarcharPara("@ClientsID", 50, ClientsID);
           proc.AddVarcharPara("@fordate", 50, fordate);
           proc.AddVarcharPara("@SettNo", 50, SettNo);
           proc.AddVarcharPara("@SettType", 50, SettType);
           ds = proc.GetDataSet();
           return ds;



       }


       public DataSet ObligationStatementFO_NEW(string fromdate, string todate, string Broker, string ClientsID, string segment, string MasterSegment,
          string Companyid, string Finyear, string grptype, string grpid, string RESULTMODE, string userbranchHierarchy,
          string ChkCollateralDeposit, string Header, string Footer)
       {
           DataSet ds = new DataSet();
           ProcedureExecute proc = new ProcedureExecute("ObligationStatementCOMM_NEW");
           proc.AddVarcharPara("@fromdate", 50, fromdate);
           proc.AddVarcharPara("@todate", 50, todate);
           proc.AddVarcharPara("@Broker", 20, Broker);
           proc.AddVarcharPara("@ClientsID", -1, ClientsID);
           proc.AddVarcharPara("@segment", 50, segment);
           proc.AddVarcharPara("@MasterSegment", 50, MasterSegment);
           proc.AddVarcharPara("@Companyid", 50, Companyid);
           proc.AddVarcharPara("@Finyear", 50, Finyear);
           proc.AddVarcharPara("@grptype", 50, grptype);
           proc.AddVarcharPara("@grpid", -1, grpid);
           proc.AddVarcharPara("@RESULTMODE", 50, RESULTMODE);
           proc.AddVarcharPara("@userbranchHierarchy", -1, userbranchHierarchy);
           proc.AddVarcharPara("@ChkCollateralDeposit", 20, ChkCollateralDeposit);
           proc.AddVarcharPara("@Header", -1, Header);
           proc.AddVarcharPara("@Footer", -1, Footer);

           ds = proc.GetDataSet();
           return ds;

       }
       public DataSet ObligationStatementCOMM_NEW(string fromdate, string todate, string Broker, string ClientsID, string segment, string MasterSegment,
          string Companyid, string Finyear, string grptype, string grpid, string RESULTMODE, string userbranchHierarchy,
          string ChkCollateralDeposit, string Header, string Footer)
       {
           DataSet ds = new DataSet();
           ProcedureExecute proc = new ProcedureExecute("ObligationStatementCOMM_NEW");
           proc.AddVarcharPara("@fromdate", 50, fromdate);
           proc.AddVarcharPara("@todate", 50, todate);
           proc.AddVarcharPara("@Broker", 20, Broker);
           proc.AddVarcharPara("@ClientsID", -1, ClientsID);
           proc.AddVarcharPara("@segment", 50, segment);
           proc.AddVarcharPara("@MasterSegment", 50, MasterSegment);
           proc.AddVarcharPara("@Companyid", 50, Companyid);
           proc.AddVarcharPara("@Finyear", 50, Finyear);
           proc.AddVarcharPara("@grptype", 50, grptype);
           proc.AddVarcharPara("@grpid", -1, grpid);
           proc.AddVarcharPara("@RESULTMODE", 50, RESULTMODE);
           proc.AddVarcharPara("@userbranchHierarchy", -1, userbranchHierarchy);
           proc.AddVarcharPara("@ChkCollateralDeposit", 20, ChkCollateralDeposit);
           proc.AddVarcharPara("@Header", -1, Header);
           proc.AddVarcharPara("@Footer", -1, Footer);
        
           ds = proc.GetDataSet();
           return ds;


       }

       public DataSet Report_RateDateFetch(string segment, string Companyid)
       {
           DataSet ds = new DataSet();
           ProcedureExecute proc = new ProcedureExecute("Report_RateDateFetch");
           proc.AddVarcharPara("@segment", 50, segment);
           proc.AddVarcharPara("@Companyid", 50, Companyid);
          
           ds = proc.GetDataSet();
           return ds;

       }


       public DataTable Sp_ObligationStatementFOCLIENT1(string fromdate, string todate, string segment, string Companyid, string BranchId, string ExchangeSegmentID,
          string Clients, string grptype, string groupby)
       {
           DataTable dt= new DataTable();
           ProcedureExecute proc = new ProcedureExecute("Sp_ObligationStatementFOCLIENT1");
           proc.AddVarcharPara("@fromdate", 50, fromdate);
           proc.AddVarcharPara("@todate", 50, todate);           
           proc.AddVarcharPara("@segment", 50, segment);          
           proc.AddVarcharPara("@Companyid", 50, Companyid);
           proc.AddVarcharPara("@BranchId", -1, BranchId);         
           proc.AddVarcharPara("@ExchangeSegmentID", 50, ExchangeSegmentID);
           proc.AddVarcharPara("@Clients", -1, Clients);
           proc.AddVarcharPara("@grptype", 50, grptype);
           proc.AddVarcharPara("@groupby", 50, groupby);

           dt = proc.GetTable();
           return dt;

       }


       public DataSet Sp_ObligationStatementFO1(string fromdate, string todate, string segment, string Companyid, string MasterSegment, string ClientsID, string Finyear
           )
       {
           DataSet ds = new DataSet();
           ProcedureExecute proc = new ProcedureExecute("Sp_ObligationStatementFO1");
           proc.AddVarcharPara("@fromdate", 50, fromdate);
           proc.AddVarcharPara("@todate", 50, todate);
           proc.AddVarcharPara("@segment", 50, segment);
           proc.AddVarcharPara("@Companyid", 50, Companyid);
           proc.AddIntegerPara("@MasterSegment", Convert.ToInt32(MasterSegment));          
           proc.AddVarcharPara("@ClientsID", 50, ClientsID);
           proc.AddVarcharPara("@Finyear", 50, Finyear);

           ds = proc.GetDataSet();
           return ds;
       }
       public DataSet Sp_ObligationStatementFO_CRYSTAL(string fromdate, string todate, string segment, string Companyid, string MasterSegment, string ClientsID, string Finyear
            )
       {
           DataSet ds = new DataSet();
           ProcedureExecute proc = new ProcedureExecute("Sp_ObligationStatementFO_CRYSTAL");
           proc.AddVarcharPara("@fromdate", 50, fromdate);
           proc.AddVarcharPara("@todate", 50, todate);
           proc.AddVarcharPara("@segment", 50, segment);
           proc.AddVarcharPara("@Companyid", 50, Companyid);
           proc.AddVarcharPara("@MasterSegment", 50,MasterSegment);
           proc.AddVarcharPara("@ClientsID", -1, ClientsID);
           proc.AddVarcharPara("@Finyear", 50, Finyear);

           ds = proc.GetDataSet();
           return ds;
       }

       public DataSet JournalVoucherPrintFromInsert(string journalData, DateTime TransactionDate
          )
       {
           DataSet ds = new DataSet();
           ProcedureExecute proc = new ProcedureExecute("JournalVoucherPrintFromInsert");
           proc.AddNTextPara("@journalData", journalData);
           proc.AddDateTimePara("@TransactionDate", TransactionDate);
          
           ds = proc.GetDataSet();
           return ds;
       }
       public DataSet SelectInterSegmentTransfer(string SAccount, string Date, string Segment, string CompanyID,
           string Client, string FinYear, decimal Amount, string DrCr, string BranchID,
           string TargetSegment, string TargetAccount, string CDSL_NSDL, string ActiveCurrency, string TradeCurrency
         )
       {
           DataSet ds = new DataSet();
           ProcedureExecute proc = new ProcedureExecute("SelectInterSegmentTransfer");
           proc.AddNVarcharPara("@SAccount", 50, SAccount);
           proc.AddNVarcharPara("@Date", 50, Date);
           proc.AddNVarcharPara("@Segment", 50, Segment);
           proc.AddNVarcharPara("@CompanyID", 50, CompanyID);
           proc.AddNVarcharPara("@Client", -1, Client);
           proc.AddNVarcharPara("@FinYear", 50, FinYear);
           proc.AddNVarcharPara("@Amount", 50,Convert.ToString(Amount));
           //proc.AddDecimalPara("@Amount", 28,6, Amount);
           proc.AddNVarcharPara("@DrCr", 10, DrCr);
           proc.AddNVarcharPara("@BranchID", -1, BranchID);
           proc.AddNVarcharPara("@TargetSegment", 50, TargetSegment);
           proc.AddNVarcharPara("@TargetAccount", 50, TargetAccount);
           proc.AddNVarcharPara("@CDSL_NSDL", 50, CDSL_NSDL);
           proc.AddNVarcharPara("@ActiveCurrency", 10, ActiveCurrency);
           proc.AddNVarcharPara("@TradeCurrency", 10, TradeCurrency);

           ds = proc.GetDataSet();
           return ds;
       }


       public int xmlJournalVoucherInterSegmentInsert(string journalData, string createuser, string finyear, string compID, string JournalVoucher_Narration,
         string JournalVoucherDetail_TransactionDate, string JournalVoucher_SettlementNumber, string JournalVoucher_SettlementType, string JournalVoucher_BillNumber, string JournalVoucher_Prefix,
         string segmentid
       )
       {
           int i = 0; DataSet ds = new DataSet();
           ProcedureExecute proc = new ProcedureExecute("xmlJournalVoucherInterSegmentInsert");
           proc.AddNTextPara("@journalData",  journalData);
           proc.AddNVarcharPara("@createuser", 50, createuser);
           proc.AddNVarcharPara("@finyear", 50, finyear);
           proc.AddNVarcharPara("@compID", 50, compID);
           proc.AddNVarcharPara("@JournalVoucher_Narration", -1, JournalVoucher_Narration);
           proc.AddNVarcharPara("@JournalVoucherDetail_TransactionDate", 50, JournalVoucherDetail_TransactionDate);
           proc.AddNVarcharPara("@JournalVoucher_SettlementNumber", 50 ,JournalVoucher_SettlementNumber);
           proc.AddNVarcharPara("@JournalVoucher_SettlementType", 50, JournalVoucher_SettlementType);         
           proc.AddNVarcharPara("@JournalVoucher_BillNumber", -1, JournalVoucher_BillNumber);
           proc.AddNVarcharPara("@JournalVoucher_Prefix", 50, JournalVoucher_Prefix);
           proc.AddNVarcharPara("@segmentid", 50, segmentid);

           i = proc.RunActionQuery();
           return i;
       }


       public DataSet TradeChangeNSEBSEDISPLAY(string date, string ClientsID, string segment, string Companyid,
        string Instruments, string OrderNo, string Terminalid, string CTCLID, string TIME,
        string Settno, string Setttype, string MasterSegment, string tradetype, string actype,
           string actypeCli, string mktprice, string Sortorder, string TranType
      )
       {
           DataSet ds = new DataSet();
           ProcedureExecute proc = new ProcedureExecute("TradeChangeNSEBSEDISPLAY");
           proc.AddNVarcharPara("@date", 50, date);
           proc.AddNVarcharPara("@ClientsID", -1, ClientsID);
           proc.AddNVarcharPara("@segment", 50, segment);
           proc.AddNVarcharPara("@Companyid", 50, Companyid);
           proc.AddNVarcharPara("@Instruments", -1, Instruments);
           proc.AddNVarcharPara("@OrderNo", -1, OrderNo);
           proc.AddNVarcharPara("@Terminalid", -1, Terminalid);
           proc.AddNVarcharPara("@CTCLID", -1, CTCLID);
           proc.AddNVarcharPara("@TIME", -1, TIME);
           proc.AddNVarcharPara("@Settno", 50, Settno);
           proc.AddNVarcharPara("@Setttype", 50, Setttype);
           proc.AddNVarcharPara("@MasterSegment", 50, MasterSegment);
           proc.AddNVarcharPara("@tradetype", 50, tradetype);
           proc.AddNVarcharPara("@actype", 50, actype);
           proc.AddNVarcharPara("@actypeCli", -1, actypeCli);
           proc.AddNVarcharPara("@mktprice", -1, mktprice);
           proc.AddNVarcharPara("@Sortorder", 50, Sortorder);
           proc.AddNVarcharPara("@TranType", 50, TranType);
           ds = proc.GetDataSet();
           return ds;
       }

       public DataSet Report_ExchangeMarginReportingFileBSEFO(string segment, string Companyid, string Finyear, string branch,
             string date, string CValuationdate, string CHoldingdate, string SYSTM1date, string SYSTM2date,
             string chkhaircut, string chkunapproved, decimal unapprovedshares, string IntialMargin
           )
       {
           DataSet ds = new DataSet();
           ProcedureExecute proc = new ProcedureExecute("Report_ExchangeMarginReportingFileBSEFO");
           proc.AddNVarcharPara("@segment", 50, segment);
           proc.AddNVarcharPara("@Companyid", 50, Companyid);
           proc.AddNVarcharPara("@Finyear", 50, Finyear);
           proc.AddNVarcharPara("@branch", -1, branch);
           proc.AddNVarcharPara("@date", 50, date);
           proc.AddNVarcharPara("@CValuationdate", 50, CValuationdate);
           proc.AddNVarcharPara("@CHoldingdate", 50, CHoldingdate);
           proc.AddNVarcharPara("@SYSTM1date", 50, SYSTM1date);
           proc.AddNVarcharPara("@SYSTM2date", 50, SYSTM2date);
           proc.AddNVarcharPara("@chkhaircut", 50, chkhaircut);
           proc.AddNVarcharPara("@chkunapproved", 50, chkunapproved);
           proc.AddDecimalPara("@unapprovedshares", 28,2, unapprovedshares);
           proc.AddNVarcharPara("@IntialMargin", 20, IntialMargin);        
           ds = proc.GetDataSet();
           return ds;
       }

       public void sp_Insert_ExportFiles(string segid, string file_type, string file_name, string userid, string batch_number, string file_path
           )
       {
          
           ProcedureExecute proc = new ProcedureExecute("sp_Insert_ExportFiles");
           proc.AddVarcharPara("@segid", 50, segid);
           proc.AddVarcharPara("@file_type", 50, file_type);
           proc.AddVarcharPara("@file_name", 200, file_name);
           proc.AddVarcharPara("@userid", 50, userid);
           proc.AddVarcharPara("@batch_number",50,batch_number);
           proc.AddVarcharPara("@file_path", 200, file_path);

          proc.RunActionQuery();
          
       }


       public DataSet Contract_Report14(string CompanyID, string DpId, string dp, string tradedate,
          string AuthorizeName, string Mode, string SegmentExchangeID, string strFundPayoutDate, string BrkgFlag,
          string SettlementNumber, string SettlementType, string Branch, string Customer, string Group,
             string Groupbytext, string Reporttype, string Groupbyvalue, string sendtypeparameter,
             string Employeename, string netammountchk, string pdforhtml, string outputhtmltype
        )
       {
           DataSet ds = new DataSet();
           ProcedureExecute proc = new ProcedureExecute("Contract_Report14");
           proc.AddNVarcharPara("@CompanyID", 50, CompanyID);
           proc.AddNVarcharPara("@DpId", 50, DpId);
           proc.AddNVarcharPara("@dp", 50, dp);
           proc.AddNVarcharPara("@tradedate", 50, tradedate);
           proc.AddNVarcharPara("@AuthorizeName", -1, AuthorizeName);
           proc.AddNVarcharPara("@Mode", -1, Mode);
           proc.AddIntegerPara("@SegmentExchangeID",Convert.ToInt32(SegmentExchangeID));
           proc.AddNVarcharPara("@strFundPayoutDate", 50, strFundPayoutDate);
           proc.AddNVarcharPara("@BrkgFlag", 50, BrkgFlag);
           proc.AddNVarcharPara("@SettlementNumber", 50, SettlementNumber);
           proc.AddNVarcharPara("@SettlementType", 20, SettlementType);
           proc.AddNVarcharPara("@Branch", -1, Branch);
           proc.AddNVarcharPara("@Customer", -1, Customer);
           proc.AddNVarcharPara("@Group", -1, Group);
           proc.AddNVarcharPara("@Groupbytext", 100, Groupbytext);
           proc.AddNVarcharPara("@Reporttype", 20, Reporttype);
           proc.AddNVarcharPara("@Groupbyvalue", 50, Groupbyvalue);
           proc.AddNVarcharPara("@sendtypeparameter", 50, sendtypeparameter);
           proc.AddNVarcharPara("@Employeename",252, Employeename);
           proc.AddNVarcharPara("@netammountchk", 20, netammountchk);
           proc.AddNVarcharPara("@pdforhtml", 50, pdforhtml);
           proc.AddNVarcharPara("@outputhtmltype", 50, outputhtmltype);
           ds = proc.GetDataSet();

           return ds;
       }

   
     public string searchSignatureUser(string userID
          )
       {
           string str=string.Empty;
           ProcedureExecute proc = new ProcedureExecute("searchSignatureUser");
           proc.AddVarcharPara("@userID",100, userID);
     
           str = Convert.ToString(proc.GetScalar());
           return str;
       }



     public void InsertTransEmail(string Emails_SenderEmailID, string Emails_Subject, string Emails_Content, string Emails_HasAttachement,
          string Emails_CreateApplication, string Emails_CreateUser, string Emails_Type, string Emails_CompanyID, string Emails_Segment, out string result
        )
     {

         ProcedureExecute proc = new ProcedureExecute("InsertTransEmail");
         proc.AddNVarcharPara("@Emails_SenderEmailID", 50, Emails_SenderEmailID);
         proc.AddNVarcharPara("@Emails_Subject", 50, Emails_Subject);
         proc.AddNVarcharPara("@Emails_Content", 50, Emails_Content);
         proc.AddNVarcharPara("@Emails_HasAttachement", -1, Emails_HasAttachement);
         proc.AddNVarcharPara("@Emails_CreateApplication", 50, Emails_CreateApplication);
         proc.AddNVarcharPara("@Emails_CreateUser", 50, Emails_CreateUser);
         proc.AddNVarcharPara("@Emails_Type", 50, Emails_Type);
         proc.AddNVarcharPara("@Emails_CompanyID", 50, Emails_CompanyID);
         proc.AddNVarcharPara("@Emails_Segment", 50, Emails_Segment);
         proc.AddVarcharPara("@result", 100, "", QueryParameterDirection.Output);
         result = proc.GetParaValue("@result").ToString();
         proc.RunActionQuery();
       
     }


     public DataSet Debit_Note(string CompanyID, string FromDate, string ToDate, string SegmentExchangeID, string SelectedJvs, string TypeVal
            )
     {
         DataSet ds = new DataSet();
         ProcedureExecute proc = new ProcedureExecute("Debit_Note");
         proc.AddVarcharPara("@CompanyID", 50, CompanyID);
         proc.AddVarcharPara("@FromDate", 50, FromDate);
         proc.AddVarcharPara("@ToDate", 50, ToDate);
         proc.AddVarcharPara("@SegmentExchangeID", 50, SegmentExchangeID);
         proc.AddNVarcharPara("@SelectedJvs",-1,SelectedJvs);
         proc.AddVarcharPara("@Type", 20, TypeVal);

         ds = proc.GetDataSet();
         return ds;
     }



     public DataSet Report_AccountConfirmationSummary(string FromDate, string ToDate, string ClientId, string BranchHierchy, string GrpType, string GrpId,
       string Segment, string CompanyID, string Finyear, string HeaderId, string FooterId, byte[] Signature)
     {
       //  byte[] SignatureinByte;
         DataSet ds = new DataSet();
         ProcedureExecute proc = new ProcedureExecute("Report_AccountConfirmationSummary");
         proc.AddVarcharPara("@FromDate", -1, FromDate);
         proc.AddVarcharPara("@ToDate", -1, ToDate);
         proc.AddVarcharPara("@ClientId", -1, ClientId);
         proc.AddVarcharPara("@BranchHierchy",-1, BranchHierchy);
         proc.AddNVarcharPara("@GrpType", 50, GrpType);
         proc.AddVarcharPara("@GrpId", -1, GrpId);
         proc.AddVarcharPara("@Segment", 50, Segment);
         proc.AddVarcharPara("@CompanyID", 50, CompanyID);
         proc.AddVarcharPara("@Finyear", 50, Finyear);
         proc.AddVarcharPara("@HeaderId", 50, HeaderId);
         proc.AddVarcharPara("@FooterId", 50, FooterId);
         proc.AddImagePara("@Signature",  Signature);
         ds = proc.GetDataSet();
         return ds;
     }


     public DataSet Report_QuatarlyStatement(string FinYear, string COMPANYID, string CLIENTS, string GRPTYPE, string Groupby, string BRANCHID,
       string MasterSegment, string Header, string Footer, string ChkDoNotPrintSecurities, string dpid, string ChkConsiderEntirePeriod, string FORDATE, string Fromdatefrmpage, string Todatefrmpage)
     {
         //  byte[] SignatureinByte;
         DataSet ds = new DataSet();
         ProcedureExecute proc = new ProcedureExecute("Report_QuatarlyStatement");
         proc.AddVarcharPara("@FinYear", 50, FinYear);
         proc.AddVarcharPara("@COMPANYID", 50, COMPANYID);
         proc.AddVarcharPara("@CLIENTS", -1, CLIENTS);
         proc.AddVarcharPara("@GRPTYPE",50, GRPTYPE);
         proc.AddNVarcharPara("@Groupby", -1, Groupby);
         proc.AddVarcharPara("@BRANCHID", -1, BRANCHID);
         proc.AddIntegerPara("@MasterSegment",Convert.ToInt32(MasterSegment));
         proc.AddVarcharPara("@Header", -1, Header);
         proc.AddVarcharPara("@Footer",-1, Footer);
         proc.AddVarcharPara("@ChkDoNotPrintSecurities", 10, ChkDoNotPrintSecurities);
         proc.AddVarcharPara("@dpid", 20, dpid);
         proc.AddVarcharPara("@ChkConsiderEntirePeriod",10, ChkConsiderEntirePeriod);
         proc.AddVarcharPara("@FORDATE", 50, FORDATE);
         proc.AddVarcharPara("@Fromdatefrmpage", 50, Fromdatefrmpage);
         proc.AddVarcharPara("@Todatefrmpage", 50, Todatefrmpage);
        
         ds = proc.GetDataSet();
         return ds;
     }




     public DataSet cdsl_EmployeeName(string ID
     )
     {
         DataSet ds = new DataSet();
         ProcedureExecute proc = new ProcedureExecute("cdsl_EmployeeName");
         proc.AddVarcharPara("@ID",50, ID);
      
         ds = proc.GetDataSet();
         return ds;
     }




     public DataSet Fetch_SubSideryTrial(string MainAccountID, string SubAccount, string Branch, string FromDate, string ToDate, string Segment,
       string FinancialYr, string CompanyVal, string DrCrAmt, string Group, string ReportType, string BranchGroutType, string GroupType,
         string ShowStatus, string ZeroBal, string ActiveCurrency, string TradeCurrency, string IsSegmentWiseBreakUp, string IsTDayBal, string IsConsolidate, string IsAcConsolidate
         ,out string LType)
     {
         //  byte[] SignatureinByte;
         DataSet ds = new DataSet();
         ProcedureExecute proc = new ProcedureExecute("Fetch_SubSideryTrial");
         proc.AddVarcharPara("@MainAccountID", -1, MainAccountID);
         proc.AddVarcharPara("@SubAccount", -1, SubAccount);
         proc.AddVarcharPara("@Branch", -1, Branch);
         proc.AddVarcharPara("@FromDate", 50, FromDate);
         proc.AddNVarcharPara("@ToDate", 50, ToDate);
         proc.AddVarcharPara("@Segment", -1, Segment);
         proc.AddVarcharPara("@FinancialYr", 50,FinancialYr);
         proc.AddVarcharPara("@Company", 50, CompanyVal);
         proc.AddVarcharPara("@DrCrAmt", 100, DrCrAmt);
         proc.AddVarcharPara("@Group", -1, Group);
         proc.AddVarcharPara("@ReportType", 10, ReportType);
         proc.AddVarcharPara("@BranchGroutType", 10, BranchGroutType);
         proc.AddVarcharPara("@GroupType", 100, GroupType);
         proc.AddVarcharPara("@ShowStatus", 10, ShowStatus);
         proc.AddVarcharPara("@ZeroBal", 10, ZeroBal);
         proc.AddVarcharPara("@ActiveCurrency", 10, ActiveCurrency);
         proc.AddVarcharPara("@TradeCurrency", 10, TradeCurrency);
         proc.AddVarcharPara("@IsSegmentWiseBreakUp", 10, IsSegmentWiseBreakUp);
         proc.AddVarcharPara("@IsTDayBal", 10, IsTDayBal);
         proc.AddVarcharPara("@IsConsolidate", 10, IsConsolidate);
         proc.AddVarcharPara("@IsAcConsolidate", 10, IsAcConsolidate);
         LType = proc.GetParaValue("@LType").ToString();
         ds = proc.GetDataSet();
         return ds;
     }
   
   }
   
}
