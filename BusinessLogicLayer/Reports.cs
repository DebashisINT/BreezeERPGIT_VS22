using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public partial class Reports
    {

        public DataTable PendingTaskReport(DateTime FromDate, DateTime ToDate, int? salesman)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("sp_PendingTask_Report");
            proc.AddDateTimePara("@Fromdate", FromDate);
            proc.AddDateTimePara("@ToDate", ToDate);
            proc.AddPara("@salesman", salesman);
           
            dt = proc.GetTable();    

            return dt;
        }
        public DataSet FETCH_USER_ACCESSGROUP(string AccType, string IDS)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("FETCH_USER_ACCESSGROUP");

            proc.AddVarcharPara("@AccType", 50, AccType);
            proc.AddVarcharPara("@IDS", -1, IDS);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet Fetch_TransEmail(DateTime FromDate, DateTime ToDate, string Segment, string Status, string ContactID, string chk, string systmmails)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Fetch_TransEmail");

            proc.AddDateTimePara("@FromDate",FromDate);
            proc.AddDateTimePara("@ToDate",ToDate);
            proc.AddVarcharPara("@Segment", 12, Segment);
            proc.AddVarcharPara("@Status", 10, Status);
            proc.AddVarcharPara("@ContactID", 20, ContactID);
            proc.AddVarcharPara("@chk", 10, chk);
            proc.AddCharPara("@systmmails", 10,Convert.ToChar(systmmails));
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet ExportPosition_CM_New(string date, string segment, string companyid, string MasterSegment, string ClientsID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("ExportPosition_CM_New");

            proc.AddVarcharPara("@FromDate", 30, date);
            proc.AddVarcharPara("@segment", 50, segment);
            proc.AddVarcharPara("@companyid", 30, companyid);
            proc.AddVarcharPara("@MasterSegment", 50, MasterSegment);
            proc.AddVarcharPara("@ClientsID", -1, ClientsID);
            ds = proc.GetDataSet();
            return ds;
        }

        public int sp_Insert_ExportFiles(string segid, string file_type, string file_name, string userid, string batch_number, string file_path)
        {
            int ret = 0;
            ProcedureExecute proc = new ProcedureExecute("sp_Insert_ExportFiles");

            proc.AddVarcharPara("@segid", 10, segid);
            proc.AddVarcharPara("@file_type", 20, file_type);
            proc.AddVarcharPara("@file_name", 150, file_name);
            proc.AddVarcharPara("@userid", 30, userid);
            proc.AddVarcharPara("@batch_number", 10, batch_number);
            proc.AddVarcharPara("@file_path", 200, file_path);
            ret = proc.RunActionQuery();
            return ret;
        }

        public DataTable sp_fetch_report_sms_email(string startdate, string enddate, string contact, string branchid, string segment)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("sp_fetch_report_sms_email");

            proc.AddVarcharPara("@startdate", 20, startdate);
            proc.AddVarcharPara("@enddate", 20, enddate);
            proc.AddVarcharPara("@contact", 20, contact);
            proc.AddVarcharPara("@branchid", 2000, branchid);
            proc.AddVarcharPara("@segment", 20, segment);
            dt = proc.GetTable();
            return dt;
        }


        public DataSet Report_MarginStocksInwardOutwardRegister(string ForDate, string ClientId,
            string GrpType, string Grpid, string Segment, string CompanyID,
            string FinYear,string Productid,string BranchHierchy,
            string Accountid, string CheckPur, string CheckSale,
            string CheckDp, string Chkledgerbaln, string Chkcashmargin,
            string ReportFor, string PendingPurSalevalueMethod, string Stocknegative,
            string SecurityType, string AppMrgnOrVarMrgn, string CorpAcTypeChk,
            string CorpAc)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Report_MarginStocksInwardOutwardRegister");

            proc.AddVarcharPara("@ForDate",35, ForDate);
            proc.AddVarcharPara("@ClientId", -1, ClientId);
            proc.AddVarcharPara("@GrpType", 25, GrpType);
            proc.AddVarcharPara("@Grpid", -1, Grpid);
            proc.AddVarcharPara("@Segment", 50, Segment);
            proc.AddVarcharPara("@CompanyID", 50,CompanyID);
            proc.AddVarcharPara("@FinYear", 50, FinYear);
            proc.AddVarcharPara("@Productid", -1, Productid);
            proc.AddVarcharPara("@BranchHierchy", -1, BranchHierchy);
            proc.AddVarcharPara("@Accountid", 20, Accountid);
            proc.AddVarcharPara("@CheckPur", 6, CheckPur);
            proc.AddVarcharPara("@CheckSale", 6, CheckSale);
            proc.AddVarcharPara("@CheckDp", 6, CheckDp);
            proc.AddVarcharPara("@Chkledgerbaln", 6, Chkledgerbaln);
            proc.AddVarcharPara("@Chkcashmargin", 10, Chkcashmargin);
            proc.AddVarcharPara("@ReportFor", 5, ReportFor);
            proc.AddVarcharPara("@PendingPurSalevalueMethod", 5, PendingPurSalevalueMethod);
            proc.AddVarcharPara("@Stocknegative", 6, Stocknegative);
            proc.AddVarcharPara("@SecurityType", 15, SecurityType);
            proc.AddVarcharPara("@AppMrgnOrVarMrgn", 10, AppMrgnOrVarMrgn);
            proc.AddVarcharPara("@CorpAcTypeChk", 8, CorpAcTypeChk);
            proc.AddVarcharPara("@CorpAc", 50, CorpAc);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataTable DematTransactionReport(string WhereClause, int startIndex, int endIndex, 
            string OwnAccSource, string OpenWhereClause,
            string Export, string SettNumTypeFr)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("DematTransactionReport");

            proc.AddVarcharPara("@WhereClause", -1, WhereClause);
            proc.AddIntegerPara("@startIndex", startIndex);
            proc.AddIntegerPara("@endIndex", endIndex);
            proc.AddVarcharPara("@OwnAccSource", 10, OwnAccSource);
            proc.AddVarcharPara("@OpenWhereClause", -1, OpenWhereClause);
            proc.AddVarcharPara("@Export", 5, Export);
            proc.AddVarcharPara("@SettNumTypeFr", 15, SettNumTypeFr);
            dt = proc.GetTable();

            return dt;
        }

        public DataSet Report_Holding(string Companyid,
          string Finyear, string HoldingDate,
          string Accountid, string Asset,
            string SettNoS,string HoldingValue,string SecurityType)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Report_Holding");

            proc.AddVarcharPara("@Companyid", 15, Companyid);
            proc.AddVarcharPara("@Finyear", 15, Finyear);
            proc.AddVarcharPara("@HoldingDate", 35, HoldingDate);
            proc.AddVarcharPara("@Accountid", 20, Accountid);
            proc.AddVarcharPara("@Asset", 500, Asset);
            proc.AddVarcharPara("@SettNoS", 20, SettNoS);
            proc.AddVarcharPara("@HoldingValue", 20, HoldingValue);
            proc.AddVarcharPara("@SecurityType", 15, SecurityType);

            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet Sp_DematChargesStatement(string segmentid, string companyID, string FromDate,
            string ToDate, string MasterSegment,
            string CLEINT, string finyear, string REPORTTYPE, string GEOUPBRANCH, string grouptype)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Sp_DematChargesStatement");

            proc.AddVarcharPara("@segmentid", 20, segmentid);
            proc.AddVarcharPara("@companyID",100, companyID);
            proc.AddVarcharPara("@FromDate", 50, FromDate);
            proc.AddVarcharPara("@ToDate", 50, ToDate);
            proc.AddVarcharPara("@MasterSegment", 50, MasterSegment);
            proc.AddVarcharPara("@CLEINT", -1, CLEINT);
            proc.AddVarcharPara("@finyear", 50, finyear);
            proc.AddVarcharPara("@REPORTTYPE", 10, REPORTTYPE);
            proc.AddVarcharPara("@GEOUPBRANCH", 10, GEOUPBRANCH);
            proc.AddVarcharPara("@grouptype", 100, grouptype);
            ds = proc.GetDataSet();
            return ds;
        }


        public DataSet Sp_Fetch_QueryReport_NSECM_QBR_ClientRealted(string Segments, string Company,
            int DayDormant, string DateFrom,string DateTo,
            string LegalStatus, int WhichQuery)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Sp_Fetch_QueryReport_NSECM_QBR_ClientRealted");

            proc.AddVarcharPara("@Segments", 3, Segments);
            proc.AddVarcharPara("@Company", 10, Company);
            proc.AddIntegerPara("@DayDormant",DayDormant);
            proc.AddVarcharPara("@DateFrom", 25, DateFrom);
            proc.AddVarcharPara("@DateTo", 25, DateTo);
            proc.AddVarcharPara("@LegalStatus", 50, LegalStatus);
            proc.AddIntegerPara("@WhichQuery", WhichQuery);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet Report_UnprocessTrades_Comm(string CommandText, string FromDate, string ToDate,
           string Broker, string ClientId,
           string Instrument, string TerminalId,string TradeCode,
            string Segment, string Companyid, string GrpType, string GrpId,
            string BranchHierchy, string RptView, string GeneRationType)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute(CommandText);

            proc.AddVarcharPara("@FromDate", 35, FromDate);
            proc.AddVarcharPara("@ToDate", 35, ToDate);
            proc.AddVarcharPara("@Broker", 5, Broker);
            proc.AddVarcharPara("@ClientId", -1, ClientId);
            proc.AddVarcharPara("@Instrument", -1, Instrument);
            proc.AddVarcharPara("@TerminalId", -1, TerminalId);
            proc.AddVarcharPara("@TradeCode",-1, TradeCode);
            proc.AddVarcharPara("@Segment", 20, Segment);
            proc.AddVarcharPara("@Companyid", 15, Companyid);
            proc.AddVarcharPara("@GrpType", 40, GrpType);
            proc.AddVarcharPara("@GrpId", -1, GrpId);
            proc.AddVarcharPara("@BranchHierchy", -1, BranchHierchy);
            proc.AddVarcharPara("@RptView", 2, RptView);
            proc.AddVarcharPara("@GeneRationType", 1, GeneRationType);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet Report_CorporateAction(string FromDate, string ToDate, string Type, string Products)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Report_CorporateAction");

            proc.AddVarcharPara("@FromDate", 100, FromDate);
            proc.AddVarcharPara("@ToDate", 100, ToDate);
            proc.AddVarcharPara("@Type", 100, Type);
            proc.AddVarcharPara("@Products", -1, Products);
            ds = proc.GetDataSet();
            return ds;
        }
        public DataSet Sp_ContractRegister(string fromdate, string todate, string clients_internalId, string segment, string Companyid)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Sp_ContractRegister");

            proc.AddVarcharPara("@fromdate", 50, fromdate);
            proc.AddVarcharPara("@todate", 50, todate);
            proc.AddVarcharPara("@clients_internalId", -1, clients_internalId);
            proc.AddVarcharPara("@segment", 50, segment);
            proc.AddVarcharPara("@Companyid", 50, Companyid);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet cdslTransctionShowwithDematandPledgeSingleClient(string stdate, string eddate, string compID, string dp,
            string BoID, string isin, string SettlementID, string branchid)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("cdslTransctionShowwithDematandPledgeSingleClient");

            proc.AddVarcharPara("@stdate", 30, stdate);
            proc.AddVarcharPara("@eddate", 30, eddate);
            proc.AddVarcharPara("@compID", 30, compID);
            proc.AddVarcharPara("@dp",30, dp);
            proc.AddVarcharPara("@BoID", 900, BoID);
            proc.AddVarcharPara("@isin", 30, isin);
            proc.AddVarcharPara("@SettlementID", 30, SettlementID);
            proc.AddVarcharPara("@branchid", 3000, branchid);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataTable cdslTransctionShowListSingleClient(string stdate, string eddate, string companyId,
          string BoID, string isin, string SettlementID,string userid, string branchid)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("cdslTransctionShowListSingleClient");

            proc.AddVarcharPara("@stdate", 30, stdate);
            proc.AddVarcharPara("@eddate", 30, eddate);
            proc.AddVarcharPara("@companyId", 30, companyId);
            proc.AddVarcharPara("@BoID", 20, BoID);
            proc.AddVarcharPara("@isin", 30, isin);
            proc.AddVarcharPara("@SettlementID", 30, SettlementID);
            proc.AddVarcharPara("@userid", 20, userid);
            proc.AddVarcharPara("@branchid", 2000, branchid);
            dt= proc.GetTable();

            return dt;
        }

        public DataTable cdslTransctionDisplayupdated(string stdate, string eddate,
         string BoID, string isin, string SettlementID, string userbranchHierarchy)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("cdslTransctionDisplayupdated");

            proc.AddVarcharPara("@stdate", 30, stdate);
            proc.AddVarcharPara("@eddate", 30, eddate);
            proc.AddVarcharPara("@BoID", 30, BoID);
            proc.AddVarcharPara("@isin", 30, isin);
            proc.AddVarcharPara("@SettlementID", 30, SettlementID);
            proc.AddVarcharPara("@userbranchHierarchy", -1, userbranchHierarchy);
            ds = proc.GetTable();
            return ds;
        }

        public DataTable DematTransactionRegisterSecurityForPOOLAccount(string WhereClause, int startIndex,
      int endIndex, string OwnAccSource, string OpenWhereClause, string Export, string SettNumTypeFr,
            string ReptType, string SegmentID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("DematTransactionRegisterSecurityForPOOLAccount");

            proc.AddVarcharPara("@WhereClause", -1, WhereClause);
            proc.AddIntegerPara("@startIndex", startIndex);
            proc.AddIntegerPara("@endIndex", endIndex);
            proc.AddVarcharPara("@OwnAccSource", 500, OwnAccSource);
            proc.AddVarcharPara("@OpenWhereClause", -1, OpenWhereClause);
            proc.AddVarcharPara("@Export", 5, Export);
            proc.AddVarcharPara("@SettNumTypeFr", 15, SettNumTypeFr);
            proc.AddVarcharPara("@ReptType",5, ReptType);
            proc.AddVarcharPara("@SegmentID", 5, SegmentID);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable DematTransactionRegisterSecurity(string WhereClause, int startIndex,
   int endIndex, string OwnAccSource, string OpenWhereClause, string Export, string SettNumTypeFr,
         string ReptType, string SegmentID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("DematTransactionRegisterSecurity");

            proc.AddVarcharPara("@WhereClause", -1, WhereClause);
            proc.AddIntegerPara("@startIndex", startIndex);
            proc.AddIntegerPara("@endIndex", endIndex);
            proc.AddVarcharPara("@OwnAccSource", 500, OwnAccSource);
            proc.AddVarcharPara("@OpenWhereClause", -1, OpenWhereClause);
            proc.AddVarcharPara("@Export", 5, Export);
            proc.AddVarcharPara("@SettNumTypeFr", 15, SettNumTypeFr);
            proc.AddVarcharPara("@ReptType", 5, ReptType);
            proc.AddVarcharPara("@SegmentID", 5, SegmentID);
            dt = proc.GetTable();
            return dt;
        }

        public DataSet sp_NoTransOnlyHolding(string startDate, string endDate,
 string dp, string contactid, string lastsegment, string lastcompany, string duplex,
       string GenerationOrder, string Header1, string Footer1, string group,
            string grouptype, string outputtype)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("sp_NoTransOnlyHolding");

            proc.AddVarcharPara("@startDate", 30, startDate);
            proc.AddVarcharPara("@endDate", 30, endDate);
            proc.AddVarcharPara("@dp", 4, dp);
            proc.AddVarcharPara("@contactid",30, contactid);
            proc.AddVarcharPara("@lastsegment", 5, lastsegment);
            proc.AddVarcharPara("@lastcompany", 30, lastcompany);
            proc.AddVarcharPara("@duplex", 1, duplex);
            proc.AddVarcharPara("@GenerationOrder", 10, GenerationOrder);
            proc.AddVarcharPara("@Header1", 20, Header1);
            proc.AddVarcharPara("@Footer1", 20, Footer1);
            proc.AddVarcharPara("@group", -1, group);
            proc.AddVarcharPara("@grouptype", 50, grouptype);
            proc.AddVarcharPara("@outputtype", 5, outputtype);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet Fetch_GeneralTrialSubAccount(string MainAccountID, string FromDate,
 string ToDate, string Segment, string FinancialYr, string Company, string ReportType,
       string ZeroBal, string ActiveCurrency, string TradeCurrency)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Fetch_GeneralTrialSubAccount");

            proc.AddVarcharPara("@MainAccountID", 50, MainAccountID);
            proc.AddVarcharPara("@FromDate", 50, FromDate);
            proc.AddVarcharPara("@ToDate", 50, ToDate);
            proc.AddVarcharPara("@Segment", -1, Segment);
            proc.AddVarcharPara("@FinancialYr", 50, FinancialYr);
            proc.AddVarcharPara("@Company", 50, Company);
            proc.AddCharPara("@ReportType", 1,Convert.ToChar(ReportType));
            proc.AddCharPara("@ZeroBal", 1, Convert.ToChar(ZeroBal));
            proc.AddVarcharPara("@ActiveCurrency", 3, ActiveCurrency);
            proc.AddVarcharPara("@TradeCurrency", 3, TradeCurrency);
           
            ds = proc.GetDataSet();
            return ds;
        }


        public DataSet CheckingForInstrumentNumber(string bankId, string ChkNumber)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("CheckingForInstrumentNumber");

            proc.AddVarcharPara("@bankId", 20, bankId);
            proc.AddVarcharPara("@ChkNumber", -1, ChkNumber);
            ds = proc.GetDataSet();
            return ds;
        }


        public int xmlCashBankUpdate(string cashBankData, string createuser,
 string finyear, string compID, string CashBankName, string OldDate, string TDate,
       string cashBankName1, string NewSegment, string OldSegment, int BRS)
        {
            int ret = 0;
            ProcedureExecute proc = new ProcedureExecute("xmlCashBankUpdate");

            proc.AddNTextPara("@cashBankData", cashBankData);
            proc.AddVarcharPara("@createuser", 10, createuser);
            proc.AddVarcharPara("@finyear", 12, finyear);
            proc.AddVarcharPara("@compID", 15, compID);
            proc.AddVarcharPara("@CashBankName", 500, CashBankName);
            proc.AddDateTimePara("@OldDate", Convert.ToDateTime(OldDate));
            proc.AddDateTimePara("@TDate", Convert.ToDateTime(TDate));
            proc.AddVarcharPara("@cashBankName1", 20, cashBankName1);
            proc.AddVarcharPara("@NewSegment", 20, NewSegment);
            proc.AddVarcharPara("@OldSegment", 20, OldSegment);
            proc.AddIntegerPara("@BRS",BRS);
            ret = proc.RunActionQuery();
            return ret;
        }

        public DataSet CashBankVoucherDelete(DateTime Date, string VoucherNumber, string CompID, int SegmentID, string Finyear)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("CashBankVoucherDelete");

            proc.AddDateTimePara("@Date", Date);
            proc.AddVarcharPara("@VoucherNumber", 20, VoucherNumber);
            proc.AddVarcharPara("@CompID", 15, CompID);
            proc.AddIntegerPara("@SegmentID", SegmentID);
            proc.AddVarcharPara("@Finyear", 10, Finyear);
            ds = proc.GetDataSet();
            return ds;
        }


        public int delBillingSPOT(int segmentid, string Date, string companyID, string finYear)
        {
            int ret = 0;
            ProcedureExecute proc = new ProcedureExecute("delBillingSPOT");

            proc.AddIntegerPara("@segmentid", segmentid);
            proc.AddVarcharPara("@Date", 50, Date);
            proc.AddVarcharPara("@companyID", 100, companyID);
            proc.AddVarcharPara("@finYear", 10, finYear);
            ret = proc.RunActionQuery();
            return ret;
        }

        public int ExchangeObligationSPOT(string date, string segment, string masterSegment,
            string companyID, string settlementNumber, string settlementtype,
            int createUser, string finYear)
        {
            int ret = 0;
            ProcedureExecute proc = new ProcedureExecute("ExchangeObligationSPOT");


            proc.AddDateTimePara("@date",Convert.ToDateTime(date));
            proc.AddVarcharPara("@segment", 5, segment);
            proc.AddVarcharPara("@masterSegment", 5, masterSegment);
            proc.AddVarcharPara("@companyID", 10, companyID);
            proc.AddVarcharPara("@settlementNumber", 10, settlementNumber);
            proc.AddVarcharPara("@settlementtype", 5, settlementtype);
            proc.AddIntegerPara("@createUser", createUser);
            proc.AddVarcharPara("@finYear", 5, finYear);
            
            ret = proc.RunActionQuery();
            return ret;
        }
        public int ClientObligationSPOT(string date, string segment, string masterSegment,
           string companyID, string settlementNumber, string settlementtype,
           int createUser, string ClientsID, string Instrument, string finYear)
        {
            int ret = 0;
            ProcedureExecute proc = new ProcedureExecute("ClientObligationSPOT");


            proc.AddDateTimePara("@date", Convert.ToDateTime(date));
            proc.AddVarcharPara("@segment", 5, segment);
            proc.AddVarcharPara("@masterSegment", 5, masterSegment);
            proc.AddVarcharPara("@companyID", 10, companyID);
            proc.AddVarcharPara("@settlementNumber", 10, settlementNumber);
            proc.AddVarcharPara("@settlementtype", 5, settlementtype);
            proc.AddIntegerPara("@createUser", createUser);
            proc.AddVarcharPara("@ClientsID", -1, ClientsID);
            proc.AddVarcharPara("@Instrument", -1, Instrument);
            proc.AddVarcharPara("@finYear", 5, finYear);

            ret = proc.RunActionQuery();
            return ret;
        }

        public int InsertExchComOblJV_SPOT(int segmentid, int createUser, string Date,
         string companyID, string COMPosition_SettlementNumber, string COMPosition_SettlementType,
         string COMPosition_FinYear, int masterSegment)
        {
            int ret = 0;
            ProcedureExecute proc = new ProcedureExecute("InsertExchComOblJV_SPOT");

            proc.AddIntegerPara("@segmentid", segmentid);
            proc.AddIntegerPara("@createUser", createUser);
            proc.AddVarcharPara("@Date",50, Date);
            proc.AddVarcharPara("@companyID", 100, companyID);
            proc.AddVarcharPara("@COMPosition_SettlementNumber", 50, COMPosition_SettlementNumber);
            proc.AddVarcharPara("@COMPosition_SettlementType", 50, COMPosition_SettlementType);
            proc.AddVarcharPara("@COMPosition_FinYear", 50, COMPosition_FinYear);
            proc.AddIntegerPara("@masterSegment", masterSegment);

            ret = proc.RunActionQuery();
            return ret;
        }
        public int AccountsLedgerBillingForSPOT(int segmentid, int createUser, string Date,
        string companyID, string COMPosition_SettlementNumber, string COMPosition_SettlementType,
        string COMPosition_FinYear)
        {
            int ret = 0;
            ProcedureExecute proc = new ProcedureExecute("AccountsLedgerBillingForSPOT");

            proc.AddIntegerPara("@segmentid", segmentid);
            proc.AddIntegerPara("@createUser", createUser);
            proc.AddVarcharPara("@Date", 50, Date);
            proc.AddVarcharPara("@companyID", 100, companyID);
            proc.AddVarcharPara("@COMPosition_SettlementNumber", 50, COMPosition_SettlementNumber);
            proc.AddVarcharPara("@COMPosition_SettlementType", 50, COMPosition_SettlementType);
            proc.AddVarcharPara("@COMPosition_FinYear", 50, COMPosition_FinYear);

            ret = proc.RunActionQuery();
            return ret;
        }



        public DataSet Report_DematCentreCommCurrency(string segment, string companyID, string finYear,
            string ExchangeSegment, string DdlType,
            string DateType, string FromDate, string ToDate, string ForClient, string Clients,
            string SettNo, string SettType, string Product, string Movement, string Nature, string OrderByType
            )
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Report_DematCentreCommCurrency");

            proc.AddVarcharPara("@segment", 20, segment);
            proc.AddVarcharPara("@companyID", 20, companyID);
            proc.AddVarcharPara("@finYear", 20, finYear);
            proc.AddVarcharPara("@ExchangeSegment", 20, ExchangeSegment);
            proc.AddVarcharPara("@DdlType", 20, DdlType);
            proc.AddVarcharPara("@DateType", 20, DateType);
            proc.AddVarcharPara("@FromDate", 40, FromDate);
            proc.AddVarcharPara("@ToDate", 40, ToDate);
            proc.AddVarcharPara("@ForClient", 20, ForClient);
            proc.AddVarcharPara("@Clients", -1, Clients);
            proc.AddVarcharPara("@SettNo", 20, SettNo);
            proc.AddVarcharPara("@SettType", 20, SettType);
            proc.AddVarcharPara("@Product", -1, Product);
            proc.AddVarcharPara("@Movement", 20, Movement);
            proc.AddVarcharPara("@Nature", 20, Nature);
            proc.AddVarcharPara("@OrderByType", 10, OrderByType);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet Delete_TransationCommCurrency(int CreateUser, int TranID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Delete_TransationCommCurrency");

            proc.AddIntegerPara("@CreateUser", CreateUser);
            proc.AddIntegerPara("@TranID",TranID);
            ds = proc.GetDataSet();
            return ds;
        }
        public DataTable cdslTransctionShowListExport(string stdate, string eddate,
              string BoID, string isin, string SettlementID, string boStatus, string companyId, string userid, string branchid)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("cdslTransctionShowListExport");

            proc.AddVarcharPara("@stdate", 30, stdate);
            proc.AddVarcharPara("@eddate", 30, eddate);           
            proc.AddVarcharPara("@BoID", 20, BoID);
            proc.AddVarcharPara("@isin", 30, isin);
            proc.AddVarcharPara("@SettlementID", 30, SettlementID);
            proc.AddVarcharPara("@boStatus", 40, boStatus);
            proc.AddVarcharPara("@companyId", 30, companyId);
            proc.AddVarcharPara("@userid", 20, userid);
            proc.AddVarcharPara("@branchid", 2000, branchid);
            dt = proc.GetTable();

            return dt;
        }

        public DataTable IndustryMapReport()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("sp_IndustryMap_Report");         
            dt = proc.GetTable();

            return dt;
        }
        //added By:Subhabrata
        public DataTable DailySalesReport(DateTime FromDate,DateTime ToDate,int? salesman)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("sp_DailySales_Report");
            proc.AddDateTimePara("@Fromdate", FromDate);
            proc.AddDateTimePara("@ToDate", ToDate);
            proc.AddPara("@salesman", salesman);
            dt = proc.GetTable();

            return dt;
        }

        #region Debashis
        //Print Common Header
        public string CommonReportHeader(string Cmp_Id, string FinYear, bool Comp_Name, bool Comp_Add, bool Comp_Oth, bool Comp_PhNo, bool Comp_FaxNo, bool AC_Period)
        {
            string Common_Header = "";
            DataTable dt = new DataTable();

            ProcedureExecute proc = new ProcedureExecute("PROC_Common_Report_Header");

            proc.AddVarcharPara("@Comp_ID", 200, Cmp_Id);
            proc.AddVarcharPara("@FINYEAR", 10, FinYear);

            dt = proc.GetTable();

            if (dt.Rows.Count > 0)
            {
                if (Comp_Name == true)
                {
                    Common_Header = Convert.ToString(dt.Rows[0]["Col_Comp_Name"]) + Environment.NewLine;
                }
                if (Comp_Add == true)
                {
                    Common_Header = Common_Header + Convert.ToString(dt.Rows[0]["Col_Comp_Add"]) + Environment.NewLine;
                }
                if (Comp_Oth == true)
                {
                    Common_Header = Common_Header + Convert.ToString(dt.Rows[0]["Col_Comp_Oth"]) + Environment.NewLine;
                }
                if (Comp_PhNo == true)
                {
                    Common_Header = Common_Header + Convert.ToString(dt.Rows[0]["Col_Comp_PhNo"]) + Environment.NewLine;
                }
                if (Comp_FaxNo == true)
                {
                    Common_Header = Common_Header + Convert.ToString(dt.Rows[0]["Col_Comp_FaxNo"]) + Environment.NewLine;
                }
                if (AC_Period == true)
                {
                    Common_Header = Common_Header + Convert.ToString(dt.Rows[0]["Col_AC_Period"]);
                }
            }
            return Common_Header;
        }
        #endregion

    }
}
