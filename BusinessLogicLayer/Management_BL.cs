using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccessLayer;
using System.Configuration;
using System.Data.SqlClient;

namespace BusinessLogicLayer
{
    public partial class Management_BL
    {

        public int UpdateTransMainAccountSummary(int MainAccount_ReferenceID, decimal openingDR, decimal openingCR)
        {
            int ret = 0;
            ProcedureExecute proc = new ProcedureExecute("UpdateTransMainAccountSummary");

            proc.AddIntegerPara("@MainAccount_ReferenceID", MainAccount_ReferenceID);
            proc.AddDecimalPara("@openingDR", 8, 28, openingDR);
            proc.AddDecimalPara("@openingCR", 8, 28, openingCR);
            ret = proc.RunActionQuery();
            return ret;
        }

        public int InsertTransMainAccountSummary(string FinancialYear, int MainAccount_ReferenceID, decimal openingDR, decimal openingCR,
            string CompanyName, string SegmentName, string BranchName, string ActiveCurrency)
        {
            int ret = 0;
            ProcedureExecute proc = new ProcedureExecute("InsertTransMainAccountSummary");
            proc.AddVarcharPara("@FinancialYear", 100, FinancialYear);
            proc.AddIntegerPara("@MainAccount_ReferenceID", MainAccount_ReferenceID);
            proc.AddDecimalPara("@openingDR", 8, 28, openingDR);
            proc.AddDecimalPara("@openingCR", 8, 28, openingCR);
            proc.AddCharPara("@CompanyName", 10, Convert.ToChar(CompanyName));
            proc.AddCharPara("@SegmentName", 10, Convert.ToChar(SegmentName));
            proc.AddCharPara("@BranchName", 10, Convert.ToChar(BranchName));
            proc.AddCharPara("@ActiveCurrency", 1, Convert.ToChar(ActiveCurrency));

            ret = proc.RunActionQuery();
            return ret;
        }

        public string Insert_ArbCycle(string ArbCycle_FinYear, string ArbCycle_From, string ArbCycle_To, int ArbCycle_CreateUser)
        {
            string rtrnvalue = "";
            int ret = 0;
            ProcedureExecute proc = new ProcedureExecute("Insert_ArbCycle");

            proc.AddVarcharPara("@ArbCycle_FinYear", 15, ArbCycle_FinYear);
            proc.AddVarcharPara("@ArbCycle_From", 35, ArbCycle_From);
            proc.AddVarcharPara("@ArbCycle_To", 35, ArbCycle_To);
            proc.AddIntegerPara("@ArbCycle_CreateUser", ArbCycle_CreateUser);
            proc.AddVarcharPara("@ResultArbCycle", 20, "", QueryParameterDirection.Output);
            ret = proc.RunActionQuery();
            rtrnvalue = proc.GetParaValue("@ResultArbCycle").ToString();
            return rtrnvalue;
        }

        public DataSet Report_PayoutToClientsCommCurrency(string Finyear, string CompanyID, string SegmentID, string PositionOf,
            string Clients, string Scrip, string GrpId, string GrpType, string branchhiearchy, string StocksDetails)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Report_PayoutToClientsCommCurrency");

            proc.AddVarcharPara("@Finyear", 15, Finyear);
            proc.AddVarcharPara("@CompanyID", 15, CompanyID);
            proc.AddVarcharPara("@SegmentID", 30, SegmentID);
            proc.AddVarcharPara("@PositionOf", 15, PositionOf);
            proc.AddVarcharPara("@Clients", -1, Clients);
            proc.AddVarcharPara("@Scrip", -1, Scrip);
            proc.AddVarcharPara("@GrpId", -1, GrpId);
            proc.AddVarcharPara("@GrpType", 20, GrpType);
            proc.AddVarcharPara("@branchhiearchy", -1, branchhiearchy);
            proc.AddVarcharPara("@StocksDetails", 100, StocksDetails);
            ds = proc.GetDataSet();

            return ds;
        }
        public DataSet Processing_ClientPayoutCommCurrency(string PayoutData, string Finyear, string CompanyID,
            string StocksSettlementNumber, string StocksAcId, string TransferDate,
            string PositionSettlementNumber, string CreateUser, string Remarks)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Processing_ClientPayoutCommCurrency");
            proc.AddNTextPara("@PayoutData", PayoutData);
            proc.AddVarcharPara("@Finyear", 12, Finyear);
            proc.AddVarcharPara("@CompanyID", 15, CompanyID);
            proc.AddVarcharPara("@StocksSettlementNumber", 12, StocksSettlementNumber);
            proc.AddVarcharPara("@StocksAcId", 12, StocksAcId);
            proc.AddVarcharPara("@TransferDate", 35, TransferDate);
            proc.AddVarcharPara("@PositionSettlementNumber", 12, PositionSettlementNumber);
            proc.AddVarcharPara("@CreateUser", 10, CreateUser);
            proc.AddVarcharPara("@Remarks", 150, Remarks);
            ds = proc.GetDataSet();

            return ds;
        }

        public DataSet Report_PledgedStocks(string CompanyId, string FinYear, string Client, string PledgedStocksID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Report_PledgedStocks");

            proc.AddVarcharPara("@CompanyId", 15, CompanyId);
            proc.AddVarcharPara("@FinYear", 20, FinYear);
            proc.AddVarcharPara("@Client", 15, Client);
            proc.AddVarcharPara("@PledgedStocksID", 30, PledgedStocksID);

            ds = proc.GetDataSet();

            return ds;
        }

        public int Insert_PledgedStocks(string CompanyId, string Segmentid, string FinYear, string Client, string Productid,
            string PurchaseDate, string Quatity, string PledgeDate, string UnPledgeDate,
            string CreateUser, string UserSelection, string PledgedStocksID, string Pledgee)
        {
            int ret = 0;
            ProcedureExecute proc = new ProcedureExecute("Insert_PledgedStocks");

            proc.AddVarcharPara("@CompanyId", 15, CompanyId);
            proc.AddVarcharPara("@Segmentid", 30, Segmentid);
            proc.AddVarcharPara("@FinYear", 20, FinYear);
            proc.AddVarcharPara("@Client", 15, Client);
            proc.AddVarcharPara("@Productid", 30, Productid);
            proc.AddVarcharPara("@PurchaseDate", 35, PurchaseDate);
            proc.AddDecimalPara("@Quatity", 0, 28, Convert.ToDecimal(Quatity));
            proc.AddVarcharPara("@PledgeDate", 35, PledgeDate);
            proc.AddVarcharPara("@UnPledgeDate", 35, UnPledgeDate);
            proc.AddVarcharPara("@CreateUser", 30, CreateUser);
            proc.AddVarcharPara("@UserSelection", 10, UserSelection);
            proc.AddVarcharPara("@PledgedStocksID", 30, PledgedStocksID);
            proc.AddVarcharPara("@Pledgee", 100, Pledgee);

            ret = proc.RunActionQuery();
            return ret;
        }

        public int Insert_IntPlSlab(string PLSlab_Type, string PLSlab_Code, string PLSlab_AmntFrom, string PLSlab_AmntTo,
            string PLSlab_Rate, string PLSlab_CreateUser, out string ResultPLSlab, out string PLSlabmaxRange)
        {
            int ret = 0;
            ProcedureExecute proc = new ProcedureExecute("Insert_IntPlSlab");

            proc.AddCharPara("@PLSlab_Type", 4, Convert.ToChar(PLSlab_Type));
            proc.AddCharPara("@PLSlab_Code", 4, Convert.ToChar(PLSlab_Code));
            proc.AddDecimalPara("@PLSlab_AmntFrom", 0, 13, Convert.ToDecimal(PLSlab_AmntFrom));
            proc.AddDecimalPara("@PLSlab_AmntTo", 0, 13, Convert.ToDecimal(PLSlab_AmntTo));
            proc.AddDecimalPara("@PLSlab_Rate", 2, 5, Convert.ToDecimal(PLSlab_Rate));
            proc.AddIntegerPara("@PLSlab_CreateUser", Convert.ToInt32(PLSlab_CreateUser));
            proc.AddVarcharPara("@ResultPLSlab", 100, "", QueryParameterDirection.Output);
            ResultPLSlab = proc.GetParaValue("@ResultPLSlab").ToString();
            proc.AddVarcharPara("@PLSlabmaxRange", 100, "", QueryParameterDirection.Output);
            PLSlabmaxRange = proc.GetParaValue("@PLSlabmaxRange").ToString();
            ret = proc.RunActionQuery();
            return ret;
        }

        public DataSet Sp_PerformanceReportFO(string companyid, string segment, string fromdate, string todate,
            string clients, string MasterSegment, string Seriesid, string Expiry,
            string grptype, string mtmcalbasis, string rpttype, string instrutype,
            string PRINTCHK, string CHKDISTRIBUTION, string ignorebfqty, string chkopen,
            string chkopenbfpositive, string chkclosepricezero, string chknetclients, string chkterminal)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Sp_PerformanceReportFO");

            proc.AddVarcharPara("@companyid", -1, companyid);
            proc.AddVarcharPara("@segment", -1, segment);
            proc.AddVarcharPara("@fromdate", 50, fromdate);
            proc.AddVarcharPara("@todate", 50, todate);
            proc.AddVarcharPara("@clients", -1, clients);
            proc.AddVarcharPara("@MasterSegment", 50, MasterSegment);
            proc.AddVarcharPara("@Seriesid", -1, Seriesid);
            proc.AddVarcharPara("@Expiry", -1, Expiry);
            proc.AddVarcharPara("@grptype", 50, grptype);
            proc.AddVarcharPara("@mtmcalbasis", 10, mtmcalbasis);
            proc.AddVarcharPara("@rpttype", 10, rpttype);
            proc.AddVarcharPara("@instrutype", 200, instrutype);
            proc.AddVarcharPara("@PRINTCHK", 200, PRINTCHK);
            proc.AddVarcharPara("@CHKDISTRIBUTION", 200, CHKDISTRIBUTION);
            proc.AddVarcharPara("@ignorebfqty", 10, ignorebfqty);
            proc.AddVarcharPara("@chkopen", 10, chkopen);
            proc.AddVarcharPara("@chkopenbfpositive", 10, chkopenbfpositive);
            proc.AddVarcharPara("@chkclosepricezero", 200, chkclosepricezero);
            proc.AddVarcharPara("@chknetclients", 10, chknetclients);
            proc.AddVarcharPara("@chkterminal", 10, chkterminal);
            ds = proc.GetDataSet();

            return ds;
        }

        public DataSet ExportPosition_GREEK(string date, string segment, string companyid, string MasterSegment, string ClientsID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("ExportPosition_GREEK");

            proc.AddVarcharPara("@date", 30, date);
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

        public DataSet ExportPosition_GREEKCDX(string date, string segment, string companyid, string MasterSegment, string ClientsID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("ExportPosition_GREEKCDX");

            proc.AddVarcharPara("@date", 30, date);
            proc.AddVarcharPara("@segment", 50, segment);
            proc.AddVarcharPara("@companyid", 30, companyid);
            proc.AddVarcharPara("@MasterSegment", 50, MasterSegment);
            proc.AddVarcharPara("@ClientsID", -1, ClientsID);
            ds = proc.GetDataSet();

            return ds;
        }
        public DataSet ExportPosition_GREEKCM(string date, string segment, string companyid, string MasterSegment, string ClientsID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("ExportPosition_GREEKCM");

            proc.AddVarcharPara("@date", 30, date);
            proc.AddVarcharPara("@segment", 50, segment);
            proc.AddVarcharPara("@companyid", 30, companyid);
            proc.AddVarcharPara("@MasterSegment", 50, MasterSegment);
            proc.AddVarcharPara("@ClientsID", -1, ClientsID);
            ds = proc.GetDataSet();

            return ds;
        }

        public int UserActivityLog(string LastActivity, int LastSegment, int userid, string FinYear)
        {
            int ret = 0;
            ProcedureExecute proc = new ProcedureExecute("UserActivityLog");

            proc.AddVarcharPara("@LastActivity", 200, LastActivity);
            proc.AddIntegerPara("@LastSegment", LastSegment);
            proc.AddIntegerPara("@userid", userid);
            proc.AddVarcharPara("@FinYear", 15, FinYear);
            ret = proc.RunActionQuery();
            return ret;
        }

        public int Sp_SplitTradesInsert(string TABLEReport, int segment, string Companyid, string createuser)
        {
            int ret = 0;
            ProcedureExecute proc = new ProcedureExecute("Sp_SplitTradesInsert");
            proc.AddNTextPara("@TABLEReport", TABLEReport);
            proc.AddIntegerPara("@segment", segment);
            proc.AddVarcharPara("@Companyid", 50, Companyid);
            proc.AddVarcharPara("@createuser", 50, createuser);
            ret = proc.RunActionQuery();
            return ret;
        }

        public DataSet Sp_SplitTradesComm(string segment, string Companyid, string date, string client, string product,
                                                     string MasterSegment, string SettNo, string SetType, string type, string custid)
        {

            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Sp_SplitTradesComm");

            proc.AddVarcharPara("@segment", 100, segment);
            proc.AddVarcharPara("@Companyid", 100, Companyid);
            proc.AddVarcharPara("@date", 100, date);
            proc.AddVarcharPara("@client", 100, client);
            proc.AddVarcharPara("@product", 100, product);
            proc.AddIntegerPara("@MasterSegment", Convert.ToInt32(MasterSegment));
            proc.AddVarcharPara("@SettNo", 100, SettNo);
            proc.AddVarcharPara("@SetType", -1, SetType);
            proc.AddVarcharPara("@type", 100, type);
            proc.AddVarcharPara("@custid", 100, custid);

            ds = proc.GetDataSet();

            return ds;
        }

        public int Sp_SplitTradesInsertCOMM(string TABLEReport, int segment, string Companyid, string createuser)
        {
            int ret = 0;
            ProcedureExecute proc = new ProcedureExecute("Sp_SplitTradesInsertCOMM");
            proc.AddNTextPara("@TABLEReport", TABLEReport);
            proc.AddIntegerPara("@segment", segment);
            proc.AddVarcharPara("@Companyid", 50, Companyid);
            proc.AddVarcharPara("@createuser", 50, createuser);
            ret = proc.RunActionQuery();
            return ret;
        }

        public int StampInsert(string StampDuty_CompanyID, string StampDuty_ApplicableState, string StampDuty_ExchangeSegmentID,
            string StampDuty_DateFrom, string StampDuty_CalBasis, Decimal StampDuty_RateCLDel, Decimal StampDuty_MinCLDel,
            Decimal StampDuty_MaxCLDel, Decimal StampDuty_RateCLSqr, Decimal StampDuty_MinCLSqr, Decimal StampDuty_MaxCLSqr,
            Decimal StampDuty_RatePRODel, Decimal StampDuty_MinPRODel, Decimal StampDuty_MaxPRODel, Decimal StampDuty_RatePROSqr,
            Decimal StampDuty_MinPROSqr, Decimal StampDuty_MaxPROSqr, Decimal StampDuty_CntrSlabMultiple, Decimal StampDuty_SlabAmount,
            Decimal StampDuty_MinCntr, Decimal StampDuty_MaxCntr, Decimal StampDuty_RateCLFut, Decimal StampDuty_MinCLFut,
            Decimal StampDuty_MaxCLFut, Decimal StampDuty_RatePROFut, Decimal StampDuty_MinPROFut, Decimal StampDuty_MaxPROFut,
            Decimal StampDuty_RateCLOpt, Decimal StampDuty_MinCLOpt, Decimal StampDuty_MaxCLOpt, Decimal StampDuty_RatePROOpt,
            Decimal StampDuty_MinPROOpt, Decimal StampDuty_MaxPROOpt, string StampDuty_FSBasis, string StampDuty_OptBasis,
            Decimal StampDuty_RateCLFS, Decimal StampDuty_MinCLFS, Decimal StampDuty_MaxCLFS, Decimal StampDuty_RatePROFS,
            Decimal StampDuty_MinPROFS, Decimal StampDuty_MaxPROFS, string StampDuty_STApplicable, string StampDuty_ApplicableFor,
            string StampDuty_GnAcCode, string StampDuty_SbAcCode, string StampDuty_GnAcCodeST, string StampDuty_SbAcCodeST,
            int StampDuty_CreateUser, DateTime StampDuty_CreateDateTime, int StampDuty_ModifyUser, string StampDuty_ChargeGroupID,
            Decimal StampDuty_SQCntrSlabMultiple, Decimal StampDuty_SQSlabAmount, Decimal StampDuty_SQMinCntr, Decimal StampDuty_SQMaxCntr)
        {
            int ret = 0;
            ProcedureExecute proc = new ProcedureExecute("StampInsert");

            proc.AddCharPara("@StampDuty_CompanyID", 10, Convert.ToChar(StampDuty_CompanyID));
            proc.AddBigIntegerPara("@StampDuty_ApplicableState", Convert.ToInt64(StampDuty_ApplicableState));
            proc.AddBigIntegerPara("@StampDuty_ExchangeSegmentID", Convert.ToInt64(StampDuty_ExchangeSegmentID));
            proc.AddCharPara("@StampDuty_ChargeGroupID", 10, Convert.ToChar(StampDuty_ChargeGroupID));
            proc.AddDateTimePara("@StampDuty_DateFrom", Convert.ToDateTime(StampDuty_DateFrom));
            proc.AddVarcharPara("@StampDuty_CalBasis", 20, StampDuty_CalBasis);
            proc.AddDecimalPara("@StampDuty_RateCLDel", 6, 8, StampDuty_RateCLDel);
            proc.AddDecimalPara("@StampDuty_MinCLDel", 6, 8, StampDuty_MinCLDel);
            proc.AddDecimalPara("@StampDuty_MaxCLDel", 6, 8, StampDuty_MaxCLDel);
            proc.AddDecimalPara("@StampDuty_RateCLSqr", 6, 8, StampDuty_RateCLSqr);
            proc.AddDecimalPara("@StampDuty_MinCLSqr", 6, 8, StampDuty_MinCLSqr);
            proc.AddDecimalPara("@StampDuty_MaxCLSqr", 6, 8, StampDuty_MaxCLSqr);
            proc.AddDecimalPara("@StampDuty_RatePRODel", 6, 8, StampDuty_RatePRODel);
            proc.AddDecimalPara("@StampDuty_MinPRODel", 6, 8, StampDuty_MinPRODel);
            proc.AddDecimalPara("@StampDuty_MaxPRODel", 6, 8, StampDuty_MaxPRODel);
            proc.AddDecimalPara("@StampDuty_RatePROSqr", 6, 8, StampDuty_RatePROSqr);
            proc.AddDecimalPara("@StampDuty_MinPROSqr", 6, 8, StampDuty_MinPROSqr);
            proc.AddDecimalPara("@StampDuty_MaxPROSqr", 6, 8, StampDuty_MaxPROSqr);
            proc.AddDecimalPara("@StampDuty_CntrSlabMultiple", 6, 20, StampDuty_CntrSlabMultiple);
            proc.AddDecimalPara("@StampDuty_SlabAmount", 6, 8, StampDuty_SlabAmount);
            proc.AddDecimalPara("@StampDuty_MinCntr", 6, 10, StampDuty_MinCntr);
            proc.AddDecimalPara("@StampDuty_MaxCntr", 6, 10, StampDuty_MaxCntr);
            proc.AddDecimalPara("@StampDuty_RateCLFut", 6, 8, StampDuty_RateCLFut);
            proc.AddDecimalPara("@StampDuty_MinCLFut", 6, 8, StampDuty_MinCLFut);
            proc.AddDecimalPara("@StampDuty_MaxCLFut", 6, 8, StampDuty_MaxCLFut);
            proc.AddDecimalPara("@StampDuty_RatePROFut", 6, 8, StampDuty_RatePROFut);
            proc.AddDecimalPara("@StampDuty_MinPROFut", 6, 8, StampDuty_MinPROFut);
            proc.AddDecimalPara("@StampDuty_MaxPROFut", 6, 8, StampDuty_MaxPROFut);
            proc.AddDecimalPara("@StampDuty_RateCLOpt", 6, 8, StampDuty_RateCLOpt);
            proc.AddDecimalPara("@StampDuty_MinCLOpt", 6, 8, StampDuty_MinCLOpt);
            proc.AddDecimalPara("@StampDuty_MaxCLOpt", 6, 8, StampDuty_MaxCLOpt);
            proc.AddDecimalPara("@StampDuty_RatePROOpt", 6, 8, StampDuty_RatePROOpt);
            proc.AddDecimalPara("@StampDuty_MinPROOpt", 6, 8, StampDuty_MinPROOpt);
            proc.AddDecimalPara("@StampDuty_MaxPROOpt", 6, 8, StampDuty_MaxPROOpt);
            proc.AddVarcharPara("@StampDuty_FSBasis", 20, StampDuty_FSBasis);
            proc.AddVarcharPara("@StampDuty_OptBasis", 20, StampDuty_OptBasis);
            proc.AddDecimalPara("@StampDuty_RateCLFS", 6, 8, StampDuty_RateCLFS);
            proc.AddDecimalPara("@StampDuty_MinCLFS", 6, 8, StampDuty_MinCLFS);
            proc.AddDecimalPara("@StampDuty_MaxCLFS", 6, 8, StampDuty_MaxCLFS);
            proc.AddDecimalPara("@StampDuty_RatePROFS", 6, 8, StampDuty_RatePROFS);
            proc.AddDecimalPara("@StampDuty_MinPROFS", 6, 8, StampDuty_MinPROFS);
            proc.AddDecimalPara("@StampDuty_MaxPROFS", 6, 8, StampDuty_MaxPROFS);
            proc.AddCharPara("@StampDuty_STApplicable", 3, Convert.ToChar(StampDuty_STApplicable));
            proc.AddVarcharPara("@StampDuty_ApplicableFor", 20, StampDuty_ApplicableFor);
            proc.AddVarcharPara("@StampDuty_GnAcCode", 20, StampDuty_GnAcCode);
            proc.AddVarcharPara("@StampDuty_SbAcCode", 20, StampDuty_SbAcCode);
            proc.AddVarcharPara("@StampDuty_GnAcCodeST", 20, StampDuty_GnAcCodeST);
            proc.AddVarcharPara("@StampDuty_SbAcCodeST", 20, StampDuty_SbAcCodeST);
            proc.AddIntegerPara("@StampDuty_CreateUser", StampDuty_CreateUser);
            proc.AddDateTimePara("@StampDuty_CreateDateTime", StampDuty_CreateDateTime);
            proc.AddIntegerPara("@StampDuty_ModifyUser", StampDuty_ModifyUser);
            proc.AddBigIntegerPara("@StampDuty_ChargeGroupID", Convert.ToInt64(StampDuty_ChargeGroupID));
            proc.AddDecimalPara("@StampDuty_SQCntrSlabMultiple", 0, 13, StampDuty_SQCntrSlabMultiple);
            proc.AddDecimalPara("@StampDuty_SQSlabAmount", 2, 10, StampDuty_SQSlabAmount);
            proc.AddDecimalPara("@StampDuty_SQMinCntr", 2, 8, StampDuty_SQMinCntr);
            proc.AddDecimalPara("@StampDuty_SQMaxCntr", 2, 8, StampDuty_SQMaxCntr);
            ret = proc.RunActionQuery();
            return ret;
        }


        public DataSet Report_Uom_sp()
        {

            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Report_Uom");
            ds = proc.GetDataSet();
            return ds;
        }
        public int Sp_SubBrokerage_Delete(string SubBrokerageMain_ID,string Mode)
        {

            int ret = 0;
            ProcedureExecute proc = new ProcedureExecute("Sp_SubBrokerage_Delete");
            proc.AddVarcharPara("@SubBrokerageMain_ID", 20, SubBrokerageMain_ID);
            proc.AddVarcharPara("@Mode", 10, Mode);

            ret = proc.RunActionQuery();
            return ret;
        }

        public int Sp_SubBrokerageMainInsert(string SubBrokerageMain_contactid, string SubBrokerageMain_CompanyID, Int16 SubBrokerageMain_Segment,
            Int16 SubBrokerageMain_Decimals, int SubBrokerageMain_RoundPattern, decimal SubBrokerageMain_MinContractBrkg,
            decimal SubBrokerageMain_MinDelvBrkg, decimal SubBrokerageMain_MinSqrBrkg, decimal SubBrokerageMain_MinOrderBrkg,
            decimal SubBrokerageMain_MinMonthlyBrkg, int SubBrokerageMain_CalculationBasis, DateTime SubBrokerageMain_DateFrom,
            int SubBrokerageMain_CreateUser, DateTime SubBrokerageMain_CreateDateTime,out Int64 ResultId )
        {

            int ret = 0;
            ProcedureExecute proc = new ProcedureExecute("Sp_SubBrokerageMainInsert");
            proc.AddCharPara("@SubBrokerageMain_contactid", 10,Convert.ToChar(SubBrokerageMain_contactid));
            proc.AddCharPara("@SubBrokerageMain_CompanyID", 10, Convert.ToChar(SubBrokerageMain_CompanyID));
            proc.AddIntegerPara("@SubBrokerageMain_Segment",SubBrokerageMain_Segment);
            proc.AddIntegerPara("@SubBrokerageMain_Segment", SubBrokerageMain_Segment);
            proc.AddIntegerPara("@SubBrokerageMain_Decimals", SubBrokerageMain_Decimals);
            proc.AddIntegerPara("@SubBrokerageMain_RoundPattern", SubBrokerageMain_RoundPattern);
            proc.AddDecimalPara("@SubBrokerageMain_MinContractBrkg",6,18, SubBrokerageMain_MinContractBrkg);
            proc.AddDecimalPara("@SubBrokerageMain_MinDelvBrkg", 6, 18, SubBrokerageMain_MinDelvBrkg);
            proc.AddDecimalPara("@SubBrokerageMain_MinSqrBrkg", 6, 18, SubBrokerageMain_MinSqrBrkg);
            proc.AddDecimalPara("@SubBrokerageMain_MinOrderBrkg", 6, 18, SubBrokerageMain_MinOrderBrkg);
            proc.AddDecimalPara("@SubBrokerageMain_MinMonthlyBrkg", 6, 18, SubBrokerageMain_MinMonthlyBrkg);
            proc.AddIntegerPara("@SubBrokerageMain_CalculationBasis", SubBrokerageMain_CalculationBasis);
            proc.AddDateTimePara("@SubBrokerageMain_DateFrom", SubBrokerageMain_DateFrom);
            proc.AddIntegerPara("@SubBrokerageMain_CreateUser", SubBrokerageMain_CreateUser);
            proc.AddDateTimePara("@SubBrokerageMain_CreateDateTime", SubBrokerageMain_CreateDateTime);
            proc.AddBigIntegerPara("@ResultId",0, QueryParameterDirection.Output);
            ResultId =Convert.ToInt64(proc.GetParaValue("@ResultId").ToString());
            ret = proc.RunActionQuery();
            return ret;
        }

        public DataSet sp_Data_Del_Ins_Up(string sourceDB)
        {

            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("sp_Data_Del_Ins_Up");
            proc.AddNVarcharPara("@sourceDB", 500, sourceDB);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet sp_Import_NsdlDematTransactions_Rec(string file_path, string file_name, string userid, string dpid, string benid, string version)
        {

            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("sp_Import_NsdlDematTransactions_Rec");
            proc.AddVarcharPara("@file_path", 200, file_path);
            proc.AddVarcharPara("@file_name", 50, file_name);
            proc.AddVarcharPara("@userid", 10, userid);
            proc.AddVarcharPara("@dpid", 20, dpid);
            proc.AddVarcharPara("@benid", 20, benid);
            proc.AddVarcharPara("@version", 10, version);
            ds = proc.GetDataSet();
            return ds;
        }

      
        public DataSet SPOTTRADEPROCESSING_DATEWISE(string trades_segment, string trades_settlementno, string fromdate,
            string todate, int CreateUser, string Companyid, string ClientsID, string Instrument, string EXCHANGESEGMENT)
        {

            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("SPOTTRADEPROCESSING_DATEWISE");
            proc.AddVarcharPara("@trades_segment", 50, trades_segment);
            proc.AddVarcharPara("@trades_settlementno", 50, trades_settlementno);
            proc.AddVarcharPara("@fromdate", 50, fromdate);
            proc.AddVarcharPara("@todate", 50, todate);
            proc.AddIntegerPara("@CreateUser",CreateUser);
            proc.AddVarcharPara("@Companyid", 50, Companyid);
            proc.AddVarcharPara("@ClientsID", -1, ClientsID);
            proc.AddVarcharPara("@Instrument", -1, Instrument);
            proc.AddVarcharPara("@EXCHANGESEGMENT", 50, EXCHANGESEGMENT);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet SPOTTRADE_DELETE(string trades_segment, string trades_settlementno, string fromdate,
           string todate, string Companyid, string ClientsID, string Instrument)
        {

            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("SPOTTRADE_DELETE");
            proc.AddVarcharPara("@trades_segment", 50, trades_segment);
            proc.AddVarcharPara("@trades_settlementno", 50, trades_settlementno);
            proc.AddVarcharPara("@fromdate", 50, fromdate);
            proc.AddVarcharPara("@todate", 50, todate);
           // proc.AddIntegerPara("@CreateUser", CreateUser);
            proc.AddVarcharPara("@Companyid", 50, Companyid);
            proc.AddVarcharPara("@ClientsID", -1, ClientsID);
            proc.AddVarcharPara("@Instrument", -1, Instrument);
            //proc.AddVarcharPara("@EXCHANGESEGMENT", 50, EXCHANGESEGMENT);
            ds = proc.GetDataSet();
            return ds;
        }

        public int trade_processing_test(string trades_segment, string trades_settlementno, string tradedate,
             int CreateUser, string Companyid, string ClientsID, string Instrument, string EXCHANGESEGMENT)
        {

            int ret = 0;
            ProcedureExecute proc = new ProcedureExecute("trade_processing_test");
            proc.AddVarcharPara("@trades_segment", 50, trades_segment);
            proc.AddVarcharPara("@trades_settlementno", 50, trades_settlementno);
            proc.AddVarcharPara("@tradedate", 50, tradedate);
            proc.AddIntegerPara("@CreateUser", CreateUser);
            proc.AddVarcharPara("@Companyid", 50, Companyid);
            proc.AddVarcharPara("@ClientsID", -1, ClientsID);
            proc.AddVarcharPara("@Instrument", -1, Instrument);
            proc.AddVarcharPara("@EXCHANGESEGMENT", 50, EXCHANGESEGMENT);
            ret = proc.RunActionQuery();
            return ret;
        }

        public DataSet ICEXCOMM_TRADE_PROCESS_Print(string trades_segment, string trades_settlementno, string tradedate, 
            int CreateUser, string Companyid, string ClientsID, string Instrument)
        {

            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("ICEXCOMM_TRADE_PROCESS_Print");
            proc.AddVarcharPara("@trades_segment", 50, trades_segment);
            proc.AddVarcharPara("@trades_settlementno", 50, trades_settlementno);
            proc.AddVarcharPara("@tradedate", 50, tradedate);
            proc.AddIntegerPara("@CreateUser", CreateUser);
            proc.AddVarcharPara("@Companyid", 50, Companyid);
            proc.AddVarcharPara("@ClientsID", -1, ClientsID);
            proc.AddVarcharPara("@Instrument", -1, Instrument);
            ds = proc.GetDataSet();
            return ds;
        }

        public int TradesEntry(string date, string segment, string Companyid,
            string createuser, string finyear, string SETTNO, string SETTTYPE,
            string BUYCLIENT,string SELLCLIENT,string SCRIP,decimal QTY,
            decimal RATE, string ORDERNO, string TRADENO, string Remarks,
            string CATEGORY, string MASTERSEGMENT, string TYPE)
        {

            int ret = 0;
            ProcedureExecute proc = new ProcedureExecute("TradesEntry");
            proc.AddVarcharPara("@date", 50, date);
            proc.AddVarcharPara("@segment", 20, segment);
            proc.AddVarcharPara("@Companyid", 30, Companyid);
            proc.AddVarcharPara("@createuser",30, createuser);
            proc.AddVarcharPara("@finyear", 50, finyear);
            proc.AddVarcharPara("@SETTNO", 50, SETTNO);
            proc.AddVarcharPara("@SETTTYPE", 50, SETTTYPE);
            proc.AddVarcharPara("@BUYCLIENT", 50, BUYCLIENT);
            proc.AddVarcharPara("@SELLCLIENT", 50, SELLCLIENT);
            proc.AddVarcharPara("@SCRIP", 50, SCRIP);
            proc.AddDecimalPara("@QTY",6,28, QTY);
            proc.AddDecimalPara("@RATE", 6,28, RATE);
            proc.AddVarcharPara("@ORDERNO", 50, ORDERNO);
            proc.AddVarcharPara("@TRADENO", 50, TRADENO);
            proc.AddVarcharPara("@Remarks", 30, Remarks);
            proc.AddVarcharPara("@CATEGORY", 50, CATEGORY);
            proc.AddVarcharPara("@MASTERSEGMENT", 50, MASTERSEGMENT);
            proc.AddVarcharPara("@TYPE", 50, TYPE);
            ret = proc.RunActionQuery();
            return ret;
        }

        public int TradesEntryModify(string date, string segment, string Companyid,
         string finyear, string SETTNO, string SETTTYPE, string SCRIP, decimal QTY,
          decimal RATE, string ORDERNO, string TRADENO, string Remarks,
          string CATEGORY, string MASTERSEGMENT, string EXCHID)
        {

            int ret = 0;
            ProcedureExecute proc = new ProcedureExecute("TradesEntryModify");
            proc.AddVarcharPara("@date", 50, date);
            proc.AddVarcharPara("@segment", 20, segment);
            proc.AddVarcharPara("@Companyid", 30, Companyid);
            proc.AddVarcharPara("@finyear", 50, finyear);
            proc.AddVarcharPara("@SETTNO", 50, SETTNO);
            proc.AddVarcharPara("@SETTTYPE", 50, SETTTYPE);
            proc.AddVarcharPara("@SCRIP", 50, SCRIP);
            proc.AddDecimalPara("@QTY", 6, 28, QTY);
            proc.AddDecimalPara("@RATE", 6, 28, RATE);
            proc.AddVarcharPara("@ORDERNO", 50, ORDERNO);
            proc.AddVarcharPara("@TRADENO", 50, TRADENO);
            proc.AddVarcharPara("@Remarks", 30, Remarks);
            proc.AddVarcharPara("@CATEGORY", 50, CATEGORY);
            proc.AddVarcharPara("@MASTERSEGMENT", 50, MASTERSEGMENT);
            proc.AddVarcharPara("@EXCHID", 50, EXCHID);
            ret = proc.RunActionQuery();
            return ret;
        }

        public DataSet TradesEntryDisplayNSE(string Companyid, string date, string segment,
          string MasterSegment, string branchid, string settno, string setttype)
        {

            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("TradesEntryDisplayNSE");
            proc.AddVarcharPara("@Companyid", 50, Companyid);
            proc.AddVarcharPara("@date", 50, date);
            proc.AddVarcharPara("@segment", 20, segment);
            proc.AddVarcharPara("@MasterSegment", 20, MasterSegment);
            proc.AddVarcharPara("@branchid", -1, branchid);
            proc.AddVarcharPara("@settno",50, settno);
            proc.AddVarcharPara("@setttype", 50, setttype);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet TRADE_PROCESS_TEST__Print(string trades_segment, string trades_settlementno, string tradedate,
           int CreateUser, string Companyid, string ClientsID, string Instrument)
        {

            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("TRADE_PROCESS_TEST__Print");
            proc.AddVarcharPara("@trades_segment", 50, trades_segment);
            proc.AddVarcharPara("@trades_settlementno", 50, trades_settlementno);
            proc.AddVarcharPara("@tradedate", 50, tradedate);
            proc.AddIntegerPara("@CreateUser", CreateUser);
            proc.AddVarcharPara("@Companyid", 50, Companyid);
            proc.AddVarcharPara("@ClientsID", -1, ClientsID);
            proc.AddVarcharPara("@Instrument", -1, Instrument);
            ds = proc.GetDataSet();
            return ds;
        }

        public int Sp_SubBrokerageDetailInsert(byte SubBrokerageDetail_MainID, byte SubBrokerageDetail_MktSegment, byte SubBrokerageDetail_BrkgFor,
                                 byte SubBrokerageDetail_InstrType, string SubBrokerageDetail_ProductID, byte SubBrokerageDetail_BrkgType,
                                 byte SubBrokerageDetail_TranType, byte SubBrokerageDetail_CalculateOn, decimal SubBrokerageDetail_FlateRate,
            byte SubBrokerageDetail_FlatPer, decimal SubBrokerageDetail_Rate, decimal SubBrokerageDetail_MinAmount, byte SubBrokerageDetail_MinPer,
            decimal SubBrokerageDetail_MaxAmount, byte SubBrokerageDetail_MaxPer, int SubBrokerageDetail_CreateUser, DateTime SubBrokerageDetail_CreateDateTime,
            string SubBrokerageDetail_BrkgGroup, string SubBrokerageDetail_BrkgClient)
        {

            int ret = 0;
            ProcedureExecute proc = new ProcedureExecute("Sp_SubBrokerageDetailInsert");
            proc.AddIntegerPara("@SubBrokerageDetail_MainID",SubBrokerageDetail_MainID);
            proc.AddIntegerPara("@SubBrokerageDetail_MktSegment", SubBrokerageDetail_MktSegment);
            proc.AddIntegerPara("@SubBrokerageDetail_BrkgFor", SubBrokerageDetail_BrkgFor);
            proc.AddIntegerPara("@SubBrokerageDetail_InstrType", SubBrokerageDetail_InstrType);
            proc.AddVarcharPara("@SubBrokerageDetail_ProductID", 50, SubBrokerageDetail_ProductID);
            proc.AddIntegerPara("@SubBrokerageDetail_BrkgType", SubBrokerageDetail_BrkgType);
            proc.AddIntegerPara("@SubBrokerageDetail_TranType", SubBrokerageDetail_TranType);
            proc.AddIntegerPara("@SubBrokerageDetail_CalculateOn", SubBrokerageDetail_CalculateOn);
            proc.AddDecimalPara("@SubBrokerageDetail_FlateRate",6,28, SubBrokerageDetail_FlateRate);
            proc.AddIntegerPara("@SubBrokerageDetail_FlatPer", SubBrokerageDetail_FlatPer);
            proc.AddDecimalPara("@SubBrokerageDetail_Rate", 6, 28, SubBrokerageDetail_Rate);
            proc.AddDecimalPara("@SubBrokerageDetail_MinAmount", 6, 28, SubBrokerageDetail_MinAmount);
            proc.AddIntegerPara("@SubBrokerageDetail_MaxPer", SubBrokerageDetail_MaxPer);
            proc.AddIntegerPara("@SubBrokerageDetail_CreateUser", SubBrokerageDetail_CreateUser);
            proc.AddDateTimePara("@SubBrokerageDetail_CreateDateTime", SubBrokerageDetail_CreateDateTime);
            proc.AddVarcharPara("@SubBrokerageDetail_BrkgGroup", 50, SubBrokerageDetail_BrkgGroup);
            proc.AddVarcharPara("@SubBrokerageDetail_BrkgClient", 50, SubBrokerageDetail_BrkgClient);
            ret = proc.RunActionQuery();
            return ret;
        }



        public DataSet SPOTTRADEGENERATE(string trades_segment, string trades_settlementno, string tradedate, string CreateUser, string companyid,
        string EXCHANGESEGMENT, string FinYear)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("SPOTTRADEGENERATE");

            proc.AddVarcharPara("@trades_segment", 50, trades_segment);
            proc.AddVarcharPara("@trades_settlementno", 50, trades_settlementno);
            proc.AddVarcharPara("@tradedate", 50, tradedate);
            proc.AddIntegerPara("@CreateUser", Convert.ToInt32(CreateUser));
            proc.AddVarcharPara("@companyid", 50, companyid);
            proc.AddVarcharPara("@EXCHANGESEGMENT", 50, EXCHANGESEGMENT);
            proc.AddVarcharPara("@FinYear", 50, FinYear);

            ds = proc.GetDataSet();
            return ds;
        }
        public DataSet Processing_DeliveryPostingCommCurrency(string segment, string companyID, string createUser, string ExchangeSegment, string FromDate,
       string ToDate, string finYear)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Processing_DeliveryPostingCommCurrency");

            proc.AddVarcharPara("@segment", 50, segment);
            proc.AddVarcharPara("@companyID", 50, companyID);
            proc.AddIntegerPara("@createUser", Convert.ToInt32(createUser));
            proc.AddVarcharPara("@ExchangeSegment", 50, ExchangeSegment);
            proc.AddVarcharPara("@FromDate", 50, FromDate);
            proc.AddVarcharPara("@ToDate", 50, ToDate);
            proc.AddVarcharPara("@finYear", 50, finYear);

            ds = proc.GetDataSet();
            return ds;
        }


        public DataTable sp_Searchslip(string letter, string CdslHolding_BenAccountNumber, string CdslHolding_dpid, string dp, string settlement
     )
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("sp_Searchslip");

            proc.AddVarcharPara("@letter", 50, letter);
            proc.AddVarcharPara("@CdslHolding_BenAccountNumber", 50, CdslHolding_BenAccountNumber);
            proc.AddVarcharPara("@CdslHolding_dpid", 50, CdslHolding_dpid);
            proc.AddVarcharPara("@dp", 50, dp);
            proc.AddVarcharPara("@settlement", 50, settlement);

            dt = proc.GetTable();
            return dt;
        }

        public DataTable sp_edit_Searchslip(string letter, string CdslHolding_BenAccountNumber, string CdslHolding_dpid, string dp, string settlement
  )
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("sp_edit_Searchslip");

            proc.AddVarcharPara("@letter", 50, letter);
            proc.AddVarcharPara("@CdslHolding_BenAccountNumber", 50, CdslHolding_BenAccountNumber);
            proc.AddVarcharPara("@CdslHolding_dpid", 50, CdslHolding_dpid);
            proc.AddVarcharPara("@dp", 50, dp);
            proc.AddVarcharPara("@settlement", 50, settlement);

            dt = proc.GetTable();
            return dt;
        }


        public void sp_edit_verification_log(string id, string isin, decimal hold, decimal quan, string dpid,
            string clientid, string cmbpid, string ex, string settleto, string settlefrom,
            string dp, string mkt, string mktto, string sourceben, string trantype, string execdate
)
        {

            ProcedureExecute proc = new ProcedureExecute("sp_edit_verification_log");

            proc.AddVarcharPara("@id", 20, id);
            proc.AddVarcharPara("@isin", 20, isin);
            proc.AddDecimalPara("@hold", 16, 3, hold);
            proc.AddDecimalPara("@quan", 16, 3, quan);
            proc.AddVarcharPara("@dpid", 20, dpid);
            proc.AddVarcharPara("@clientid", 20, clientid);
            proc.AddVarcharPara("@cmbpid", 20, cmbpid);
            proc.AddVarcharPara("@ex", 20, ex);
            proc.AddVarcharPara("@settleto", 20, settleto);
            proc.AddVarcharPara("@settlefrom", 20, settlefrom);
            proc.AddVarcharPara("@dp", 20, dp);
            proc.AddVarcharPara("@mkt", 20, mkt);
            proc.AddVarcharPara("@mktto", 20, mktto);
            proc.AddVarcharPara("@sourceben", 20, sourceben);
            proc.AddIntegerPara("@trantype", Convert.ToInt32(trantype));
            proc.AddVarcharPara("@execdate", 20, execdate);
            proc.RunActionQuery();

        }



        public void SebiFeeInsert(string SebiFee_CompanyID, string SebiFee_ExchangeSegmentID, string SebiFee_DateFrom, string SebiFee_ApplicableFor, decimal SebiFee_Rate,
               string SebiFee_GnAcCode, string SebiFee_GnAcCodeST, string SebiFee_SbAcCode, string SebiFee_SbAcCodeST, string SebiFee_STApplicable,
               string SebiFee_CreateUser, string SebiFee_CreateDate, string SebiFee_ModifyUser, string SebiFee_ChargeGroupID, decimal SebiFee_OptRate
   )
        {

            ProcedureExecute proc = new ProcedureExecute("SebiFeeInsert");

            proc.AddVarcharPara("@SebiFee_CompanyID", 20, SebiFee_CompanyID);
            proc.AddBigIntegerPara("@SebiFee_ExchangeSegmentID", Convert.ToInt64(SebiFee_ExchangeSegmentID));
            proc.AddVarcharPara("@SebiFee_DateFrom", 16, SebiFee_DateFrom);
            proc.AddVarcharPara("@SebiFee_ApplicableFor", 20, SebiFee_ApplicableFor);
            proc.AddDecimalPara("@SebiFee_Rate", 8, 6, SebiFee_Rate);
            proc.AddVarcharPara("@SebiFee_GnAcCode", 20, SebiFee_GnAcCode);
            proc.AddVarcharPara("@SebiFee_GnAcCodeST", 20, SebiFee_GnAcCodeST);
            proc.AddVarcharPara("@SebiFee_SbAcCode", 20, SebiFee_SbAcCode);
            proc.AddVarcharPara("@SebiFee_SbAcCodeST", 20, SebiFee_SbAcCodeST);
            proc.AddVarcharPara("@SebiFee_STApplicable", 10, SebiFee_STApplicable);
            proc.AddIntegerPara("@SebiFee_CreateUser", Convert.ToInt32(SebiFee_CreateUser));
            proc.AddVarcharPara("@SebiFee_CreateDate", 20, SebiFee_CreateDate);
            proc.AddIntegerPara("@SebiFee_ModifyUser", Convert.ToInt32(SebiFee_ModifyUser));
            proc.AddVarcharPara("@SebiFee_ChargeGroupID", 20, SebiFee_ChargeGroupID);
            proc.AddDecimalPara("@SebiFee_OptRate", 8, 6, SebiFee_OptRate);
            proc.RunActionQuery();

        }


        public DataTable report_FetchBannedEntitiesNsdlCdsl(string Panno)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("report_FetchBannedEntitiesNsdlCdsl");

            proc.AddVarcharPara("@Panno", 50, Panno);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable report_FetchBannedEntitiesNsdlCdsl()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("report_FetchBannedEntitiesNsdlCdsl");



            dt = proc.GetTable();
            return dt;
        }


        public DataSet Fetch_MessageTemplateReservedWord(string SenderID, string RecipientID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Fetch_MessageTemplateReservedWord");

            proc.AddVarcharPara("@SenderID", 50, SenderID);
            proc.AddVarcharPara("@RecipientID", 50, RecipientID);
            ds = proc.GetDataSet();
            return ds;
        }


        public DataSet Fetch_AMCDueClients(string dp, string PageNum, string PageSize, string AsOnDate, string ToDate,
        string FinYear)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Fetch_AMCDueClients");

            proc.AddVarcharPara("@dp", 10, dp);
            proc.AddIntegerPara("@PageNum", Convert.ToInt32(PageNum));
            proc.AddIntegerPara("@PageSize", Convert.ToInt32(PageSize));
            proc.AddVarcharPara("@AsOnDate", 50, AsOnDate);
            proc.AddVarcharPara("@ToDate", 50, ToDate);
            proc.AddVarcharPara("@FinYear", 50, FinYear);

            ds = proc.GetDataSet();
            return ds;
        }





        public void DematInsert(string DematCharges_CompanyID, string DematCharges_ExchangeSegmentID, string DematCharges_DateFrom, string DematCharges_STApplicable, string DematCharges_ApplicableFor,
           string DematCharges_CalBasis, string DematCharges_CalFrequency, string DematCharges_CalOn, string DematCharges_GnAcCode, string DematCharges_SbAcCode,
           string DematCharges_GnAcCodeST, string DematCharges_SbAcCodeST, decimal DematCharges_RatePI, decimal DematCharges_MinPI, decimal DematCharges_MaxPI,
            decimal DematCharges_FlatPI, string DematCharges_FlatPIAdditive, decimal DematCharges_RatePIO, decimal DematCharges_MinPIO, decimal DematCharges_MaxPIO,
           decimal DematCharges_FlatPIO, string DematCharges_FlatPIOAdditive, decimal DematCharges_RateMPI, decimal DematCharges_MinMPI, decimal DematCharges_MaxMPI,
           decimal DematCharges_FlatMPI, string DematCharges_FlatMPIAdditive, decimal DematCharges_RateHPI, decimal DematCharges_MinHPI, decimal DematCharges_MaxHPI,
              decimal DematCharges_FlatHPI, string DematCharges_FlatHPIAdditive, decimal DematCharges_RatePO, decimal DematCharges_MinPO, decimal DematCharges_MaxPO,
           decimal DematCharges_FlatPO, string DematCharges_FlatPOAdditive, decimal DematCharges_RatePOO, decimal DematCharges_MinPOO, decimal DematCharges_MaxPOO,
           decimal DematCharges_FlatPOO, string DematCharges_FlatPOOAdditive, decimal DematCharges_RateMPO, decimal DematCharges_MinMPO, decimal DematCharges_MaxMPO,
               decimal DematCharges_FlatMPO, string DematCharges_FlatMPOAdditive, decimal DematCharges_RateHPO, decimal DematCharges_MinHPO, decimal DematCharges_MaxHPO,
           decimal DematCharges_FlatHPO, string DematCharges_FlatHPOAdditive, decimal DematCharges_RateMI, decimal DematCharges_MinMI, decimal DematCharges_MaxMI,
           decimal DematCharges_FlatMI, string DematCharges_FlatMIAdditive, decimal DematCharges_RateMO, decimal DematCharges_MinMO, decimal DematCharges_MaxMO,
            decimal DematCharges_FlatMO, string DematCharges_FlatMOAdditive, decimal DematCharges_RateHO, decimal DematCharges_MinHO, decimal DematCharges_MaxHO,
           decimal DematCharges_FlatHO, string DematCharges_FlatHOAdditive, decimal DematCharges_RateIS, decimal DematCharges_MinIS, decimal DematCharges_MaxIS,
           decimal DematCharges_FlatIS, string DematCharges_FlatISAdditive, string DematCharges_CreateUser, string DematCharges_CreateDateTime, string DematCharges_ModifyUser, string DematCharges_ChargeGroupID)
        {

            ProcedureExecute proc = new ProcedureExecute("DematInsert");

            proc.AddVarcharPara("@DematCharges_CompanyID", 20, DematCharges_CompanyID);
            proc.AddBigIntegerPara("@DematCharges_ExchangeSegmentID", Convert.ToInt64(DematCharges_ExchangeSegmentID));
            proc.AddVarcharPara("@DematCharges_DateFrom", 50, DematCharges_DateFrom);
            proc.AddVarcharPara("@DematCharges_STApplicable", 10, DematCharges_STApplicable);
            proc.AddVarcharPara("@DematCharges_ApplicableFor", 20, DematCharges_ApplicableFor);
            proc.AddVarcharPara("@DematCharges_CalBasis", 20, DematCharges_CalBasis);
            proc.AddVarcharPara("@DematCharges_CalFrequency", 20, DematCharges_CalFrequency);
            proc.AddVarcharPara("@DematCharges_CalOn", 20, DematCharges_CalOn);
            proc.AddVarcharPara("@DematCharges_GnAcCode", 20, DematCharges_GnAcCode);
            proc.AddVarcharPara("@DematCharges_SbAcCode", 10, DematCharges_SbAcCode);
            proc.AddVarcharPara("@DematCharges_GnAcCodeST", 20, DematCharges_GnAcCodeST);
            proc.AddVarcharPara("@DematCharges_SbAcCodeST", 20, DematCharges_SbAcCodeST);
            proc.AddDecimalPara("@DematCharges_RatePI", 8, 6, DematCharges_RatePI);
            proc.AddDecimalPara("@DematCharges_MinPI", 8, 6, DematCharges_MinPI);
            proc.AddDecimalPara("@DematCharges_MaxPI", 8, 6, DematCharges_MaxPI);
            proc.AddDecimalPara("@DematCharges_FlatPI", 8, 6, DematCharges_FlatPI);
            proc.AddVarcharPara("@DematCharges_FlatPIAdditive", 20, DematCharges_FlatPIAdditive);
            proc.AddDecimalPara("@DematCharges_RatePIO", 8, 6, DematCharges_RatePIO);
            proc.AddDecimalPara("@DematCharges_MinPIO", 8, 6, DematCharges_MinPIO);
            proc.AddDecimalPara("@DematCharges_MaxPIO", 8, 6, DematCharges_MaxPIO);
            proc.AddDecimalPara("@DematCharges_FlatPIO", 8, 6, DematCharges_FlatPIO);
            proc.AddVarcharPara("@DematCharges_FlatPIOAdditive", 20, DematCharges_FlatPIOAdditive);
            proc.AddDecimalPara("@DematCharges_RateMPI", 8, 6, DematCharges_RateMPI);
            proc.AddDecimalPara("@DematCharges_MinMPI", 8, 6, DematCharges_MinMPI);
            proc.AddDecimalPara("@DematCharges_MaxMPI", 8, 6, DematCharges_MaxMPI);
            proc.AddDecimalPara("@DematCharges_FlatMPI", 8, 6, DematCharges_FlatMPI);
            proc.AddVarcharPara("@DematCharges_FlatMPIAdditive", 20, DematCharges_FlatMPIAdditive);
            proc.AddDecimalPara("@DematCharges_RateHPI", 8, 6, DematCharges_RateHPI);
            proc.AddDecimalPara("@DematCharges_MinHPI", 8, 6, DematCharges_MinHPI);
            proc.AddDecimalPara("@DematCharges_MaxHPI", 8, 6, DematCharges_MaxHPI);
            proc.AddDecimalPara("@DematCharges_FlatHPI", 8, 6, DematCharges_FlatHPI);
            proc.AddVarcharPara("@DematCharges_FlatHPIAdditive", 20, DematCharges_FlatHPIAdditive);
            proc.AddDecimalPara("@DematCharges_RatePO", 8, 6, DematCharges_RatePO);
            proc.AddDecimalPara("@DematCharges_MinPO", 8, 6, DematCharges_MinPO);
            proc.AddDecimalPara("@DematCharges_MaxPO", 8, 6, DematCharges_MaxPO);
            proc.AddDecimalPara("@DematCharges_FlatPO", 8, 6, DematCharges_FlatPO);
            proc.AddVarcharPara("@DematCharges_FlatPOAdditive", 20, DematCharges_FlatPOAdditive);
            proc.AddDecimalPara("@DematCharges_RatePOO", 8, 6, DematCharges_RatePOO);
            proc.AddDecimalPara("@DematCharges_MinPOO", 8, 6, DematCharges_MinPOO);
            proc.AddDecimalPara("@DematCharges_MaxPOO", 8, 6, DematCharges_MaxPOO);
            proc.AddDecimalPara("@DematCharges_FlatPOO", 8, 6, DematCharges_FlatPOO);
            proc.AddVarcharPara("@DematCharges_FlatPOOAdditive", 20, DematCharges_FlatPOOAdditive);
            proc.AddDecimalPara("@DematCharges_RateMPO", 8, 6, DematCharges_RateMPO);
            proc.AddDecimalPara("@DematCharges_MinMPO", 8, 6, DematCharges_MinMPO);
            proc.AddDecimalPara("@DematCharges_MaxMPO", 8, 6, DematCharges_MaxMPO);
            proc.AddDecimalPara("@DematCharges_FlatMPO", 8, 6, DematCharges_FlatMPO);
            proc.AddVarcharPara("@DematCharges_FlatMPOAdditive", 20, DematCharges_FlatMPOAdditive);
            proc.AddDecimalPara("@DematCharges_RateHPO", 8, 6, DematCharges_RateHPO);
            proc.AddDecimalPara("@DematCharges_MinHPO", 8, 6, DematCharges_MinHPO);
            proc.AddDecimalPara("@DematCharges_MaxHPO", 8, 6, DematCharges_MaxHPO);
            proc.AddDecimalPara("@DematCharges_FlatHPO", 8, 6, DematCharges_FlatHPO);
            proc.AddVarcharPara("@DematCharges_FlatHPOAdditive", 20, DematCharges_FlatHPOAdditive);
            proc.AddDecimalPara("@DematCharges_RateMI", 8, 6, DematCharges_RateMI);
            proc.AddDecimalPara("@DematCharges_MinMI", 8, 6, DematCharges_MinMI);
            proc.AddDecimalPara("@DematCharges_MaxMI", 8, 6, DematCharges_MaxMI);
            proc.AddDecimalPara("@DematCharges_FlatMI", 8, 6, DematCharges_FlatMI);
            proc.AddVarcharPara("@DematCharges_FlatMIAdditive", 20, DematCharges_FlatMIAdditive);
            proc.AddDecimalPara("@DematCharges_RateMO", 8, 6, DematCharges_RateMO);
            proc.AddDecimalPara("@DematCharges_MinMO", 8, 6, DematCharges_MinMO);
            proc.AddDecimalPara("@DematCharges_MaxMO", 8, 6, DematCharges_MaxMO);
            proc.AddDecimalPara("@DematCharges_FlatMO", 8, 6, DematCharges_FlatMO);
            proc.AddVarcharPara("@DematCharges_FlatMOAdditive", 20, DematCharges_FlatMOAdditive);
            proc.AddDecimalPara("@DematCharges_RateHO", 8, 6, DematCharges_RateHO);
            proc.AddDecimalPara("@DematCharges_MinHO", 8, 6, DematCharges_MinHO);
            proc.AddDecimalPara("@DematCharges_MaxHO", 8, 6, DematCharges_MaxHO);
            proc.AddDecimalPara("@DematCharges_FlatHO", 8, 6, DematCharges_FlatHO);
            proc.AddVarcharPara("@DematCharges_FlatHOAdditive", 20, DematCharges_FlatHOAdditive);
            proc.AddDecimalPara("@DematCharges_RateIS", 8, 6, DematCharges_RateIS);
            proc.AddDecimalPara("@DematCharges_MinIS", 8, 6, DematCharges_MinIS);
            proc.AddDecimalPara("@DematCharges_MaxIS", 8, 6, DematCharges_MaxIS);
            proc.AddDecimalPara("@DematCharges_FlatIS", 8, 6, DematCharges_FlatIS);
            proc.AddVarcharPara("@DematCharges_FlatISAdditive", 20, DematCharges_FlatISAdditive);
            proc.AddVarcharPara("@DematCharges_CreateUser", 20, DematCharges_CreateUser);
            proc.AddVarcharPara("@DematCharges_CreateDateTime", 20, DematCharges_CreateDateTime);
            proc.AddIntegerPara("@DematCharges_ModifyUser", Convert.ToInt32(DematCharges_ModifyUser));
            proc.AddVarcharPara("@DematCharges_ChargeGroupID", 20, DematCharges_ChargeGroupID);

            proc.RunActionQuery();

        }




        public DataTable FetchClientNoHolding(string HoldingDate, string Finyear, string checkdt
)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("FetchClientNoHolding");
            proc.AddVarcharPara("@HoldingDate", 50, HoldingDate);
            proc.AddVarcharPara("@Finyear", 50, Finyear);
            proc.AddVarcharPara("@checkdt", 50, checkdt);


            dt = proc.GetTable();
            return dt;
        }



        public DataTable FetchClientNoHolding_NSDL(string HoldingDate, string Finyear, string checkdt
)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("FetchClientNoHolding_NSDL");
            proc.AddVarcharPara("@HoldingDate", 20, HoldingDate);
            proc.AddVarcharPara("@Finyear", 20, Finyear);
            proc.AddVarcharPara("@checkdt", 5, checkdt);

            dt = proc.GetTable();
            return dt;
        }




        public DataSet Sp_Fetch_QueryReport_NSECM_QBR_ClientRealted(string Segments, string CompanyVal, string DayDormant, string DateFrom, string DateTo,
      string LegalStatus, string WhichQuery)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Sp_Fetch_QueryReport_NSECM_QBR_ClientRealted");

            proc.AddVarcharPara("@Segments", 10, Segments);
            proc.AddVarcharPara("@Company", 10, CompanyVal);
            proc.AddIntegerPara("@DayDormant", Convert.ToInt32(DayDormant));
            proc.AddVarcharPara("@DateFrom", 50, DateFrom);
            proc.AddVarcharPara("@DateTo", 50, DateTo);
            proc.AddVarcharPara("@LegalStatus", 50, LegalStatus);
            proc.AddIntegerPara("@WhichQuery", Convert.ToInt32(WhichQuery));

            ds = proc.GetDataSet();
            return ds;
        }


        public DataSet Sp_Fetch_DormantAccData_ForGridView(string PageNum, string PageSize, string AsOnDate, string NoofMonth, string exchangesegmentid,
 string branchid, string MainAcId, string CompanyID, string FinYear, string where_NsdlHolding_HoldingDateTime, string user, string EndDate, string dp)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Sp_Fetch_DormantAccData_ForGridView");

            proc.AddIntegerPara("@PageNum", Convert.ToInt32(PageNum));
            proc.AddIntegerPara("@PageSize", Convert.ToInt32(PageSize));
            proc.AddVarcharPara("@AsOnDate", 30, AsOnDate);
            proc.AddIntegerPara("@NoofMonth", Convert.ToInt32(NoofMonth));
            proc.AddVarcharPara("@exchangesegmentid", 20, exchangesegmentid);
            proc.AddVarcharPara("@branchid", -1, branchid);
            proc.AddVarcharPara("@MainAcId", 50, MainAcId);
            proc.AddVarcharPara("@CompanyID", 50, CompanyID);
            proc.AddVarcharPara("@FinYear", 50, FinYear);
            proc.AddVarcharPara("@where_NsdlHolding_HoldingDateTime", 50, where_NsdlHolding_HoldingDateTime);
            proc.AddVarcharPara("@user", 50, user);
            proc.AddVarcharPara("@EndDate", 50, EndDate);
            proc.AddVarcharPara("@dp", 10, dp);
            ds = proc.GetDataSet();
            return ds;
        }



        public DataSet Sp_Fetch_DormantAccData(string AsOnDate, string NoofMonth, string exchangesegmentid, string branchid, string MainAcId,
 string CompanyID, string FinYear, string where_NsdlHolding_HoldingDateTime, string user)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Sp_Fetch_DormantAccData");

            proc.AddVarcharPara("@AsOnDate", 30, AsOnDate);
            proc.AddIntegerPara("@NoofMonth", Convert.ToInt32(NoofMonth));
            proc.AddVarcharPara("@exchangesegmentid", 20, exchangesegmentid);
            proc.AddVarcharPara("@branchid", -1, branchid);
            proc.AddVarcharPara("@MainAcId", 50, MainAcId);
            proc.AddVarcharPara("@CompanyID", 50, CompanyID);
            proc.AddVarcharPara("@FinYear", 50, FinYear);
            proc.AddVarcharPara("@where_NsdlHolding_HoldingDateTime", 50, where_NsdlHolding_HoldingDateTime);
            //  proc.AddVarcharPara("@EndDate", 50, EndDate);
            proc.AddVarcharPara("@user", 50, user);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet Sp_Fetch_DormantAccDataCDSL(string AsOnDate, string NoofMonth, string exchangesegmentid, string branchid, string MainAcId,
string CompanyID, string FinYear, string where_NsdlHolding_HoldingDateTime, string EndDate, string user)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Sp_Fetch_DormantAccDataCDSL");

            proc.AddVarcharPara("@AsOnDate", 30, AsOnDate);
            proc.AddIntegerPara("@NoofMonth", Convert.ToInt32(NoofMonth));
            proc.AddVarcharPara("@exchangesegmentid", 20, exchangesegmentid);
            proc.AddVarcharPara("@branchid", -1, branchid);
            proc.AddVarcharPara("@MainAcId", 50, MainAcId);
            proc.AddVarcharPara("@CompanyID", 50, CompanyID);
            proc.AddVarcharPara("@FinYear", 50, FinYear);
            proc.AddVarcharPara("@where_NsdlHolding_HoldingDateTime", 50, where_NsdlHolding_HoldingDateTime);
            proc.AddVarcharPara("@EndDate", 50, EndDate);
            proc.AddVarcharPara("@user", 50, user);
            ds = proc.GetDataSet();
            return ds;
        }


        public DataSet Sp_Fetch_DormantAccDataALL(string AsOnDate, string NoofMonth, string exchangesegmentid, string branchid, string MainAcId,
string CompanyID, string FinYear, string where_NsdlHolding_HoldingDateTime, string EndDate, string user)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Sp_Fetch_DormantAccDataALL");

            proc.AddVarcharPara("@AsOnDate", 30, AsOnDate);
            proc.AddIntegerPara("@NoofMonth", Convert.ToInt32(NoofMonth));
            proc.AddVarcharPara("@exchangesegmentid", 20, exchangesegmentid);
            proc.AddVarcharPara("@branchid", -1, branchid);
            proc.AddVarcharPara("@MainAcId", 50, MainAcId);
            proc.AddVarcharPara("@CompanyID", 50, CompanyID);
            proc.AddVarcharPara("@FinYear", 50, FinYear);
            proc.AddVarcharPara("@where_NsdlHolding_HoldingDateTime", 50, where_NsdlHolding_HoldingDateTime);
            proc.AddVarcharPara("@EndDate", 50, EndDate);
            proc.AddVarcharPara("@user", 50, user);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet SubsidiaryTrialPeriodic(string Fdate, string TDate, string SegmentID, string FinYear, string CompID, string WhereClause, string Branch,
string ForBarnch)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("SubsidiaryTrialPeriodic");
            proc.AddVarcharPara("@Fdate", 50, Fdate);
            proc.AddVarcharPara("@TDate", 50, TDate);
            proc.AddVarcharPara("@SegmentID", 20, SegmentID);
            proc.AddVarcharPara("@FinYear", 20, FinYear);
            proc.AddVarcharPara("@CompID", 20, CompID);
            proc.AddVarcharPara("@WhereClause", -1, WhereClause);
            proc.AddVarcharPara("@Branch", -1, Branch);
            proc.AddVarcharPara("@ForBarnch", 10, ForBarnch);
            ds = proc.GetDataSet();
            return ds;
        }


        public DataTable sp_Nsdl_FetchTransaction(string userid, string startRowIndex, string endIndex
)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("sp_Nsdl_FetchTransaction");
            proc.AddVarcharPara("@userid", 20, userid);
            proc.AddIntegerPara("@startRowIndex", Convert.ToInt32(startRowIndex));
            proc.AddIntegerPara("@endIndex", Convert.ToInt32(endIndex));

            dt = proc.GetTable();
            return dt;
        }

        public DataSet Sp_Fetch_Matched_UnMatchedRecord_Verification(string date, string WhichRecord, string WhichDp)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Sp_Fetch_Matched_UnMatchedRecord_Verification");


            proc.AddDateTimePara("@date", Convert.ToDateTime(date));
            proc.AddVarcharPara("@WhichRecord", 100, WhichRecord);
            proc.AddVarcharPara("@WhichDp", 100, WhichDp);



            ds = proc.GetDataSet();

            return ds;
        }


        public DataSet Fetch_UnMatchedRecord_Detail_ByID(string RecordID, string WhichDp)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Fetch_UnMatchedRecord_Detail_ByID");



            proc.AddVarcharPara("@RecordID", 100, RecordID);
            proc.AddVarcharPara("@WhichDp", 100, WhichDp);




            ds = proc.GetDataSet();

            return ds;
        }



        public DataSet Get_Download(string batch, string slip, string type, string ExchSegId, string AccountID, string TransDate, string Spname, string vCheck)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute(Spname);



            proc.AddVarcharPara("@batch", 100, batch);
            proc.AddVarcharPara("@slip", 100, slip);
            proc.AddVarcharPara("@type", 100, type);
            proc.AddVarcharPara("@ExchSegId", 100, ExchSegId);

            if (vCheck != "'Spot'")
            {
                proc.AddBigIntegerPara("@AccountID", Convert.ToInt64(AccountID));
                proc.AddDateTimePara("@TransDate", Convert.ToDateTime(TransDate));

            }



            ds = proc.GetDataSet();

            return ds;
        }

        public void Employee_Replacement(string ReplacementId, string Users, string modifyUser, string EffectiveFrom, string remarks
      )
        {

            ProcedureExecute proc = new ProcedureExecute("Employee_Replacement");
            proc.AddVarcharPara("@ReplacementId", 150, ReplacementId);
            proc.AddVarcharPara("@Users", -1, Users);
            proc.AddIntegerPara("@modifyUser", Convert.ToInt32(modifyUser));
            proc.AddVarcharPara("@EffectiveFrom", 50, EffectiveFrom);
            proc.AddVarcharPara("@remarks", 100, remarks);

            proc.RunActionQuery();

        }
        public void InsuranceCommissionSlab_Insert(string InsCommissionSlab_Code, string InsCommissionSlab_Type, decimal InsCommissionSlab_MinAmount, decimal InsCommissionSlab_MaxAmount, string InsCommissionSlab_MinPPT,
            string InsCommissionSlab_MaxPPT, decimal InsCommissionSlab_Rate, decimal InsCommissionSlab_FixedAmount, string InsCommissionSlab_CreateUser, string InsCommissionSlab_Notes,
            decimal InsCommissionSlab_ORC, string EntryType, out string ResultSlab
     )
        {

            ProcedureExecute proc = new ProcedureExecute("InsuranceCommissionSlab_Insert");
            proc.AddVarcharPara("@InsCommissionSlab_Code", 10, InsCommissionSlab_Code);
            proc.AddVarcharPara("@InsCommissionSlab_Type", 10, InsCommissionSlab_Type);
            proc.AddDecimalPara("@InsCommissionSlab_MinAmount", 18, 4, InsCommissionSlab_MinAmount);
            proc.AddDecimalPara("@InsCommissionSlab_MaxAmount", 18, 4, InsCommissionSlab_MaxAmount);
            proc.AddIntegerPara("@InsCommissionSlab_MinPPT", Convert.ToInt16(InsCommissionSlab_MinPPT));
            proc.AddIntegerPara("@InsCommissionSlab_MaxPPT", Convert.ToInt16(InsCommissionSlab_MaxPPT));
            proc.AddDecimalPara("@InsCommissionSlab_Rate", 6, 4, InsCommissionSlab_Rate);
            proc.AddDecimalPara("@InsCommissionSlab_FixedAmount", 18, 4, InsCommissionSlab_FixedAmount);
            proc.AddIntegerPara("@InsCommissionSlab_CreateUser", Convert.ToInt32(InsCommissionSlab_CreateUser));
            proc.AddVarcharPara("@InsCommissionSlab_Notes", -1, InsCommissionSlab_Notes);
            proc.AddDecimalPara("@InsCommissionSlab_ORC", 6, 4, InsCommissionSlab_ORC);
            proc.AddVarcharPara("@EntryType", 10, EntryType);
            proc.AddVarcharPara("@ResultSlab", 10, "", QueryParameterDirection.Output);
            ResultSlab = proc.GetParaValue("@ResultSlab").ToString();
            proc.RunActionQuery();

        }



        public void BrokerageDetailInsert(string BrokerageDetail_MainID, string BrokerageDetail_MktSegment, string BrokerageDetail_BrkgFor, string BrokerageDetail_InstrType, string BrokerageDetail_BrkgType,
        string BrokerageDetail_TranType, string BrokerageDetail_CalculateOn, string BrokerageDetail_MinPer, string BrokerageDetail_MaxPer, string BrokerageDetail_FlatPer,
        decimal BrokerageDetail_Rate, decimal BrokerageDetail_MinAmount, decimal BrokerageDetail_MaxAmount,
            decimal BrokerageDetail_FlatRate, string BrokerageDetail_ProductID, string BrokerageDetail_SlabCode,
            string BrokerageDetail_CreateUser, string BrokerageDetail_CreateDateTime, string BrokerageDetail_ModifyUser, decimal BrokerageDetail_FinMinPerLot, out int RT, out long dID
 )
        {

            ProcedureExecute proc = new ProcedureExecute("BrokerageDetailInsert");
            proc.AddBigIntegerPara("@BrokerageDetail_MainID", Convert.ToInt64(BrokerageDetail_MainID));
            proc.AddIntegerPara("@BrokerageDetail_MktSegment", Convert.ToInt16(BrokerageDetail_MktSegment));
            proc.AddIntegerPara("@BrokerageDetail_BrkgFor", Convert.ToInt16(BrokerageDetail_BrkgFor));
            proc.AddIntegerPara("@BrokerageDetail_InstrType", Convert.ToInt16(BrokerageDetail_InstrType));
            proc.AddIntegerPara("@BrokerageDetail_BrkgType", Convert.ToInt16(BrokerageDetail_BrkgType));
            proc.AddIntegerPara("@BrokerageDetail_TranType", Convert.ToInt16(BrokerageDetail_TranType));
            proc.AddIntegerPara("@BrokerageDetail_CalculateOn", Convert.ToInt16(BrokerageDetail_CalculateOn));
            proc.AddIntegerPara("@BrokerageDetail_MinPer", Convert.ToInt16(BrokerageDetail_MinPer));
            proc.AddIntegerPara("@BrokerageDetail_MaxPer", Convert.ToInt32(BrokerageDetail_MaxPer));
            proc.AddIntegerPara("@BrokerageDetail_FlatPer", Convert.ToInt32(BrokerageDetail_FlatPer));
            proc.AddDecimalPara("@BrokerageDetail_Rate", 18, 6, BrokerageDetail_Rate);
            proc.AddDecimalPara("@BrokerageDetail_MinAmount", 18, 6, BrokerageDetail_MinAmount);
            proc.AddDecimalPara("@BrokerageDetail_MaxAmount", 18, 6, BrokerageDetail_MaxAmount);
            proc.AddDecimalPara("@BrokerageDetail_FlatRate", 18, 4, BrokerageDetail_FlatRate);
            proc.AddBigIntegerPara("@BrokerageDetail_ProductID", Convert.ToInt64(BrokerageDetail_ProductID));
            proc.AddVarcharPara("@BrokerageDetail_SlabCode", 10, BrokerageDetail_SlabCode);
            proc.AddIntegerPara("@BrokerageDetail_CreateUser", Convert.ToInt32(BrokerageDetail_CreateUser));
            proc.AddVarcharPara("@BrokerageDetail_CreateDateTime", 50, BrokerageDetail_CreateDateTime);
            proc.AddIntegerPara("@BrokerageDetail_ModifyUser", Convert.ToInt32(BrokerageDetail_ModifyUser));
            proc.AddDecimalPara("@BrokerageDetail_FinMinPerLot", 16, 4, BrokerageDetail_FinMinPerLot);
            proc.AddIntegerPara("@RT", 0, QueryParameterDirection.Output);
            RT = Convert.ToInt32(proc.GetParaValue("@RT").ToString());
            proc.AddBigIntegerPara("@dID", 0, QueryParameterDirection.Output);
            dID = Convert.ToInt64(proc.GetParaValue("@dID").ToString());
            proc.RunActionQuery();

        }




        public int BrokerageChargeInsert(string ChargeSetup_MainID, string ChargeSetup_ServTaxBasis, string ChargeSetup_TranChargeBasis, string ChargeSetup_StampDutyBasis, string ChargeSetup_SEBIFeeBasis,
      string ChargeSetup_DematChargeBasis, string ChargeSetup_ServTaxGroup, string ChargeSetup_TranChargeGroup, string ChargeSetup_StampDutyGroup, string ChargeSetup_DematChargeGroupp,
      string ChargeSetup_SEBIFeeGroup, string ChargeSetup_CreateUser, string ChargeSetup_CreateDateTime, string ChargeSetup_ClearanceChargeBasis, string ChargeSetup_ClearanceChargeGroup,
            string ChargeSetup_CttBasis, string ChargeSetup_CttSchemeGroup, string ChargeSetup_STTGroup, string ChargeSetup_STTBasis, string ChargeSetup_segment
)
        {
            int i = 0;
            ProcedureExecute proc = new ProcedureExecute("BrokerageChargeInsert");
            proc.AddBigIntegerPara("@ChargeSetup_MainID", Convert.ToInt64(ChargeSetup_MainID));
            proc.AddIntegerPara("@ChargeSetup_ServTaxBasis", Convert.ToInt16(ChargeSetup_ServTaxBasis));
            proc.AddIntegerPara("@ChargeSetup_TranChargeBasis", Convert.ToInt16(ChargeSetup_TranChargeBasis));
            proc.AddIntegerPara("@ChargeSetup_StampDutyBasis", Convert.ToInt16(ChargeSetup_StampDutyBasis));
            proc.AddIntegerPara("@ChargeSetup_SEBIFeeBasis", Convert.ToInt16(ChargeSetup_SEBIFeeBasis));
            proc.AddIntegerPara("@ChargeSetup_DematChargeBasis", Convert.ToInt16(ChargeSetup_DematChargeBasis));
            proc.AddVarcharPara("@ChargeSetup_ServTaxGroup", 20, ChargeSetup_ServTaxGroup);
            proc.AddVarcharPara("@ChargeSetup_TranChargeGroup", 20, ChargeSetup_TranChargeGroup);
            proc.AddVarcharPara("@ChargeSetup_StampDutyGroup", 20, ChargeSetup_StampDutyGroup);
            proc.AddVarcharPara("@ChargeSetup_DematChargeGroupp", 20, ChargeSetup_DematChargeGroupp);
            proc.AddVarcharPara("@ChargeSetup_SEBIFeeGroup", 20, ChargeSetup_SEBIFeeGroup);
            proc.AddIntegerPara("@ChargeSetup_CreateUser", Convert.ToInt32(ChargeSetup_CreateUser));
            proc.AddVarcharPara("@ChargeSetup_CreateDateTime", 50, ChargeSetup_CreateDateTime);
            proc.AddIntegerPara("@ChargeSetup_ClearanceChargeBasis", Convert.ToInt16(ChargeSetup_ClearanceChargeBasis));
            proc.AddVarcharPara("@ChargeSetup_ClearanceChargeGroup", 20, ChargeSetup_ClearanceChargeGroup);
            proc.AddIntegerPara("@ChargeSetup_CttBasis", Convert.ToInt32(ChargeSetup_CttBasis));
            proc.AddVarcharPara("@ChargeSetup_CttSchemeGroup", 20, ChargeSetup_CttSchemeGroup);
            proc.AddVarcharPara("@ChargeSetup_STTGroup", 20, ChargeSetup_STTGroup);
            proc.AddIntegerPara("@ChargeSetup_STTBasis", Convert.ToInt32(ChargeSetup_STTBasis));
            proc.AddVarcharPara("@ChargeSetup_segment", 20, ChargeSetup_segment);

            i = proc.RunActionQuery();
            return i;
        }


        public void BrokerageChargeUpdate(string ChargeSetup_MainID, string ChargeSetup_ServTaxBasis, string ChargeSetup_STTBasis, string ChargeSetup_TranChargeBasis, string ChargeSetup_StampDutyBasis, string ChargeSetup_SEBIFeeBasis,
      string ChargeSetup_DematChargeBasis, string ChargeSetup_ServTaxGroup, string ChargeSetup_STTGroup, string ChargeSetup_TranChargeGroup, string ChargeSetup_StampDutyGroup, string ChargeSetup_DematChargeGroupp,
      string ChargeSetup_SEBIFeeGroup, string ChargeSetup_CreateUser, string ChargeSetup_CreateDateTime, string ChargeSetup_CttSchemeGroup, string ChargeSetup_ClearanceChargeGroup, string ChargeSetup_ClearanceChargeBasis,
            string ChargeSetup_CttBasis, string ChargeSetup_segment
)
        {

            ProcedureExecute proc = new ProcedureExecute("BrokerageChargeUpdate");
            proc.AddBigIntegerPara("@ChargeSetup_MainID", Convert.ToInt64(ChargeSetup_MainID));
            proc.AddIntegerPara("@ChargeSetup_ServTaxBasis", Convert.ToInt16(ChargeSetup_ServTaxBasis));
            proc.AddIntegerPara("@ChargeSetup_STTBasis", Convert.ToInt32(ChargeSetup_STTBasis));
            proc.AddIntegerPara("@ChargeSetup_TranChargeBasis", Convert.ToInt16(ChargeSetup_TranChargeBasis));
            proc.AddIntegerPara("@ChargeSetup_StampDutyBasis", Convert.ToInt16(ChargeSetup_StampDutyBasis));
            proc.AddIntegerPara("@ChargeSetup_SEBIFeeBasis", Convert.ToInt16(ChargeSetup_SEBIFeeBasis));
            proc.AddIntegerPara("@ChargeSetup_DematChargeBasis", Convert.ToInt16(ChargeSetup_DematChargeBasis));
            proc.AddVarcharPara("@ChargeSetup_ServTaxGroup", 20, ChargeSetup_ServTaxGroup);
            proc.AddVarcharPara("@ChargeSetup_STTGroup", 20, ChargeSetup_STTGroup);
            proc.AddVarcharPara("@ChargeSetup_TranChargeGroup", 20, ChargeSetup_TranChargeGroup);
            proc.AddVarcharPara("@ChargeSetup_StampDutyGroup", 20, ChargeSetup_StampDutyGroup);
            proc.AddVarcharPara("@ChargeSetup_DematChargeGroupp", 20, ChargeSetup_DematChargeGroupp);
            proc.AddVarcharPara("@ChargeSetup_SEBIFeeGroup", 20, ChargeSetup_SEBIFeeGroup);
            proc.AddIntegerPara("@ChargeSetup_CreateUser", Convert.ToInt32(ChargeSetup_CreateUser));
            proc.AddVarcharPara("@ChargeSetup_CreateDateTime", 50, ChargeSetup_CreateDateTime);
            proc.AddVarcharPara("@ChargeSetup_CttSchemeGroup", 20, ChargeSetup_CttSchemeGroup);
            proc.AddVarcharPara("@ChargeSetup_ClearanceChargeGroup", 20, ChargeSetup_ClearanceChargeGroup);
            proc.AddIntegerPara("@ChargeSetup_ClearanceChargeBasis", Convert.ToInt16(ChargeSetup_ClearanceChargeBasis));
            proc.AddIntegerPara("@ChargeSetup_CttBasis", Convert.ToInt32(ChargeSetup_CttBasis));

            proc.AddVarcharPara("@ChargeSetup_segment", 20, ChargeSetup_segment);

            proc.RunActionQuery();

        }


        public DataSet management_Report_ArbPLFinalSettlement(string Finyear, string CompanyID, string Cycleid)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Report_ArbPLFinalSettlement");


            proc.AddVarcharPara("@Companyid", 15, CompanyID);
            proc.AddVarcharPara("@FinYear", 15, Finyear);
            proc.AddVarcharPara("@Cycleid", 20, Cycleid);

            ds = proc.GetDataSet();

            return ds;
        }

        public DataSet management_Delete_ArbPLFinalSettlement(string Finyear, string CompanyID, string Cycleid, string Useid)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Delete_ArbPLFinalSettlement");

            proc.AddVarcharPara("@Companyid", 15, CompanyID);
            proc.AddVarcharPara("@FinYear", 15, Finyear);
            proc.AddVarcharPara("@Cycleid", 20, Cycleid);
            proc.AddVarcharPara("@Useid", 20, Useid);

            proc.RunActionQuery();

            return ds;
        }
        public void management_Process_ArbPLFinalSettlement(string tabledata, string CompanyID, string Finyear, string Cycleid, string Useid, string Segmantid)
        {

            ProcedureExecute proc = new ProcedureExecute("Process_ArbPLFinalSettlement");

            proc.AddNTextPara("@FinalSett", tabledata);
            proc.AddVarcharPara("@Companyid", 15, CompanyID);
            proc.AddVarcharPara("@FinYear", 15, Finyear);
            proc.AddVarcharPara("@Cycleid", 20, Cycleid);
            proc.AddVarcharPara("@Useid", 20, Useid);
            proc.AddVarcharPara("@Segmantid", 20, Segmantid);

            proc.RunActionQuery();


        }

        public DataSet management_Process_ArbPLFinalSettlementCOM(string Finyear, string CompanyID, string Cycleid, string tabledata, string Useid, string Segmantid)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Process_ArbPLFinalSettlementCOM");
            proc.AddVarcharPara("@Companyid", 15, CompanyID);
            proc.AddVarcharPara("@FinYear", 15, Finyear);
            proc.AddVarcharPara("@Cycleid", 20, Cycleid);

            if (tabledata != "" && tabledata != null)
                proc.AddNTextPara("@FinalSett", tabledata);

            if (Useid != "" && Useid != null)
                proc.AddVarcharPara("@Useid", 20, Useid);

            if (Segmantid != "" && Segmantid != null)
                proc.AddVarcharPara("@Segmantid", 20, Segmantid);

            proc.RunActionQuery();

            return ds;
        }

        public DataSet management_Delete_ArbPLFinalSettlementCOM(string Finyear, string CompanyID, string Cycleid, string Useid)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Delete_ArbPLFinalSettlementCOM");

            proc.AddVarcharPara("@Companyid", 15, CompanyID);
            proc.AddVarcharPara("@FinYear", 15, Finyear);
            proc.AddVarcharPara("@Cycleid", 20, Cycleid);
            proc.AddVarcharPara("@Useid", 20, Useid);

            proc.RunActionQuery();

            return ds;
        }

        public void management_Insert_ArbGroup(string txtGroupCode, string txtGroupName, decimal txtExposureLimit, decimal Txt_CMBuyExpMarginRate,
            decimal Txt_CMSellExpMarginRate, decimal Txt_FOExpMarginRate, string DdlIntCalOnCM, string ddl_CMBuyExpIntSlab, string ddl_CMSellExpIntSlab,
            string ddl_CMObligationIntSlab, string ddl_CMMArginIntSlab, string ddl_FOMarginIntSlab, string ddl_ProfitSlab, string ddl_LossSlab,
            string userid, string Mode, string ArbGroupID, string ddl_FOObligationIntSlab)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Insert_ArbGroup");

            proc.AddVarcharPara("@ArbGroup_Code", 10, txtGroupCode);
            proc.AddVarcharPara("@ArbGroup_Name", 250, txtGroupName);
            proc.AddDecimalPara("@ArbGroup_ExposureLimit", 0, 18, txtExposureLimit);
            proc.AddDecimalPara("@ArbGroup_CMBuyExpMarginRate", 0, 18, Txt_CMBuyExpMarginRate);
            proc.AddDecimalPara("@ArbGroup_CMSellExpMarginRate", 0, 18, Txt_CMSellExpMarginRate);
            proc.AddDecimalPara("@ArbGroup_FOExpMarginRate", 0, 18, Txt_FOExpMarginRate);

            if (DdlIntCalOnCM == "Exposure")
            {
                if (ddl_CMBuyExpIntSlab != "0")
                    proc.AddVarcharPara("@ArbGroup_CMBuyExpIntSlab", 10, ddl_CMBuyExpIntSlab);
                else
                    proc.AddVarcharPara("@ArbGroup_CMBuyExpIntSlab", 10, Convert.ToString(DBNull.Value));
                if (ddl_CMSellExpIntSlab != "0")
                    proc.AddVarcharPara("@ArbGroup_CMSellExpIntSlab", 10, ddl_CMSellExpIntSlab);
                else
                    proc.AddVarcharPara("@ArbGroup_CMSellExpIntSlab", 10, Convert.ToString(DBNull.Value));

                proc.AddVarcharPara("@ArbGroup_CMObligationIntSlab", 10, Convert.ToString(DBNull.Value));

            }
            else
            {
                proc.AddVarcharPara("@ArbGroup_CMBuyExpIntSlab", 10, Convert.ToString(DBNull.Value));
                proc.AddVarcharPara("@ArbGroup_CMSellExpIntSlab", 10, Convert.ToString(DBNull.Value));

                if (ddl_CMObligationIntSlab != "0")
                    proc.AddVarcharPara("@ArbGroup_CMObligationIntSlab", 10, ddl_CMObligationIntSlab);
                else
                    proc.AddVarcharPara("@ArbGroup_CMObligationIntSlab", 10, Convert.ToString(DBNull.Value));

            }

            if (ddl_CMMArginIntSlab != "0")
                proc.AddVarcharPara("@ArbGroup_CMMArginIntSlab", 10, ddl_CMMArginIntSlab);
            else
                proc.AddVarcharPara("@ArbGroup_CMMArginIntSlab", 10, Convert.ToString(DBNull.Value));

            if (ddl_FOMarginIntSlab != "0")
                proc.AddVarcharPara("@ArbGroup_FOMarginIntSlab", 10, ddl_FOMarginIntSlab);
            else
                proc.AddVarcharPara("@ArbGroup_FOMarginIntSlab", 10, Convert.ToString(DBNull.Value));

            if (ddl_ProfitSlab != "0")
                proc.AddVarcharPara("@ArbGroup_ProfitSlab", 10, ddl_ProfitSlab);
            else
                proc.AddVarcharPara("@ArbGroup_ProfitSlab", 10, Convert.ToString(DBNull.Value));

            if (ddl_LossSlab != "0")
                proc.AddVarcharPara("@ArbGroup_LossSlab", 10, ddl_LossSlab);
            else
                proc.AddVarcharPara("@ArbGroup_LossSlab", 10, Convert.ToString(DBNull.Value));

            proc.AddVarcharPara("@ArbGroup_CreateUser", 10, userid);

            if (Mode == "Edit")
            {
                proc.AddVarcharPara("@ArbGroupID", 30, ArbGroupID);

            }
            else
            {
                proc.AddVarcharPara("@ArbGroupID", 30, "ADD");

            }
            if (ddl_FOObligationIntSlab != "0")
                proc.AddVarcharPara("@ArbGroup_FOObligationIntSlab", 10, ddl_FOObligationIntSlab);
            else
                proc.AddVarcharPara("@ArbGroup_FOObligationIntSlab", 10, Convert.ToString(DBNull.Value));

            proc.RunActionQuery();


        }

        public DataSet management_Insert_ArbGroupMembers(string ArbGroupMembers_CustomerID, string ArbGroupMembers_GroupCode,
            decimal ArbGroupMembers_Deposits, string ArbGroupMembers_ProfitShareSlab, string ArbGroupMembers_LossShareSlab,
            string ArbGroupMembers_DateFrom, string ArbGroupMembers_CreateUser, string ArbGroupMembers_Mode, decimal ArbGroupMembers_FixedCostPerCycle)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Insert_ArbGroupMembers");

            //proc.AddVarcharPara("@Companyid", 15, CompanyID);
            //proc.AddVarcharPara("@FinYear", 15, Finyear);
            //proc.AddVarcharPara("@Cycleid", 20, Cycleid);
            //proc.AddVarcharPara("@Useid", 20, Useid);

            proc.AddVarcharPara("@ArbGroupMembers_CustomerID", 15, ArbGroupMembers_CustomerID);
            proc.AddVarcharPara("@ArbGroupMembers_GroupCode", 100, ArbGroupMembers_GroupCode);
            proc.AddDecimalPara("@ArbGroupMembers_Deposits", 0, 18, ArbGroupMembers_Deposits);
            proc.AddVarcharPara("@ArbGroupMembers_ProfitShareSlab", 30, ArbGroupMembers_ProfitShareSlab);
            proc.AddVarcharPara("@ArbGroupMembers_LossShareSlab", 30, ArbGroupMembers_LossShareSlab);
            proc.AddVarcharPara("@ArbGroupMembers_DateFrom", 35, ArbGroupMembers_DateFrom);
            proc.AddVarcharPara("@ArbGroupMembers_CreateUser", 20, ArbGroupMembers_CreateUser);
            proc.AddVarcharPara("@ArbGroupMembers_Mode", 15, ArbGroupMembers_Mode);
            proc.AddDecimalPara("@ArbGroupMembers_FixedCostPerCycle", 0, 18, ArbGroupMembers_FixedCostPerCycle);

            ds = proc.GetDataSet();

            return ds;
        }

        public DataSet management_Process_ExcessMarginRefundStatementAuthorizePopUp(string ClientId, string MainId, string StockType)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Process_ExcessMarginRefundStatementAuthorizePopUp");

            proc.AddVarcharPara("@ClientId", 15, ClientId);
            proc.AddVarcharPara("@MainId", 50, MainId);
            proc.AddVarcharPara("@StockType", 15, StockType);

            ds = proc.GetDataSet();

            return ds;
        }

        public void management_AccountsLedgerBillingOnlyExchange(int segmentid, string companyID, string SettmentNo, string SettType,
            DateTime Tdate, DateTime vdate, int createUser, string finYear, string masterSegment)
        {
            ProcedureExecute proc = new ProcedureExecute("AccountsLedgerBillingOnlyExchange");

            //proc.AddVarcharPara("@ClientId", 15, ClientId);
            //proc.AddVarcharPara("@MainId", 50, MainId);
            //proc.AddVarcharPara("@StockType", 15, StockType);

            proc.AddIntegerPara("@segmentid", segmentid);
            proc.AddVarcharPara("@companyID", 20, companyID);
            proc.AddVarcharPara("@settlementNumber", 15, SettmentNo);
            proc.AddVarcharPara("@settlementType", 5, SettType);

            proc.AddDateTimePara("@Tdate", Tdate);
            proc.AddDateTimePara("@vdate", vdate);
            proc.AddVarcharPara("@type", 5, "All");
            proc.AddIntegerPara("@createUser", createUser);
            proc.AddVarcharPara("@finYear", 12, finYear);
            proc.AddVarcharPara("@masterSegment", 10, masterSegment);

            proc.RunActionQuery();


        }

        public void management_delComobligationforNseCMOnlyExchange(int segmentid, string companyID, string SettmentNo, string SettType,
           DateTime Tdate, DateTime vdate)
        {
            ProcedureExecute proc = new ProcedureExecute("delComobligationforNseCMOnlyExchange");

            proc.AddIntegerPara("@segmentid", segmentid);
            proc.AddVarcharPara("@companyID", 100, companyID);
            proc.AddVarcharPara("@settlementNumber", 10, SettmentNo);
            proc.AddVarcharPara("@settlementType", 5, SettType);
            proc.AddDateTimePara("@Tdate", Tdate);
            proc.AddDateTimePara("@vdate", vdate);

            proc.RunActionQuery();


        }

        public DataSet management_CashBankDelete(string ID, string VNo, string BranchID, string SegmentID, DateTime TDate)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("CashBankDelete");

            proc.AddVarcharPara("@ID", 20, ID);
            proc.AddVarcharPara("@VoucherNo", 20, VNo);
            proc.AddVarcharPara("@BranchID", 50, BranchID);
            proc.AddVarcharPara("@SegmentID", 50, SegmentID);
            proc.AddDateTimePara("@TransactionDate", TDate);

            ds = proc.GetDataSet();

            return ds;
        }

        public void management_Delete_CFSettings(string ID)
        {
            ProcedureExecute proc = new ProcedureExecute("Delete_CFSettings");

            proc.AddVarcharPara("@CFMain_ID", 20, ID);
            proc.AddVarcharPara("@Mode", 10, "main");

            proc.RunActionQuery();
        }


        public void management_Insert_ScripDetailInsert(int ScripDetail_MainID, bool rdbAssetAll, string ScripDetail_Assetid, string ScripDetail_SlabCode,
            decimal ScripDetail_Rate, int CreateUser)
        {
            ProcedureExecute proc = new ProcedureExecute("Insert_ScripDetailInsert");


            proc.AddIntegerPara("@ScripDetail_MainID", ScripDetail_MainID);
            if (rdbAssetAll)
            {
                proc.AddVarcharPara("@ScripDetail_Assetid", 50, "0");
            }
            else
            {
                proc.AddVarcharPara("@ScripDetail_Assetid", 50, ScripDetail_Assetid);
            }
            proc.AddVarcharPara("@ScripDetail_SlabCode", 4, ScripDetail_SlabCode);
            proc.AddDecimalPara("@ScripDetail_Rate", 6, 28, ScripDetail_Rate);
            proc.AddIntegerPara("@CreateUser", CreateUser);

            proc.RunActionQuery();
        }


        public void management_Insert_CFSettingsMainInsert(char CFSettings_contactid, char CFSettings_CompanyID, int CFSettings_Segment,
            string CFSettings_Mode, string CFSettings_PositionType, DateTime CFSettings_DateFrom, int CFSettings_CreateUser,
            string CFSettings_ClientType, int CFSettings_Decimals, int CFSettings_RoundPattern, char CFSettings_PriceBasis,
            char CFSettings_MTM, bool ChkPostCharges, out Int64 ResultId)
        {
            string strReturn;

            ProcedureExecute proc = new ProcedureExecute("Insert_CFSettingsMainInsert");

            proc.AddCharPara("@CFSettings_contactid", 15, CFSettings_contactid);
            proc.AddCharPara("@CFSettings_CompanyID", 10, CFSettings_CompanyID);
            proc.AddIntegerPara("@CFSettings_Segment", CFSettings_Segment);
            proc.AddVarcharPara("@CFSettings_Mode", 10, CFSettings_Mode);
            proc.AddVarcharPara("@CFSettings_PositionType", 10, CFSettings_PositionType);
            proc.AddDateTimePara("@CFSettings_DateFrom", CFSettings_DateFrom);
            proc.AddIntegerPara("@CFSettings_CreateUser", CFSettings_CreateUser);

            proc.AddVarcharPara("@CFSettings_ClientType", 10, CFSettings_ClientType);

            proc.AddIntegerPara("@CFSettings_Decimals", CFSettings_Decimals);

            proc.AddIntegerPara("@CFSettings_RoundPattern", CFSettings_RoundPattern);

            proc.AddCharPara("@CFSettings_PriceBasis", 1, CFSettings_PriceBasis);

            proc.AddCharPara("@CFSettings_MTM", 1, CFSettings_MTM);

            if (ChkPostCharges)
            {
                proc.AddVarcharPara("@ScripMain_PostCharges", 1, "1");
            }
            else
            {
                proc.AddVarcharPara("@ScripMain_PostCharges", 1, "2");
            }

            proc.AddBigIntegerNullPara("@ResultId", QueryParameterDirection.Output);



            ResultId = Convert.ToInt64(proc.GetParaValue("@ResultId").ToString());

            proc.RunActionQuery();

        }


        public void management_CFSlabInsert(char CFSlab_Code, string CFSlab_Mode, decimal CFSlab_MinRange, decimal CFSlab_MaxRange,
            string txtflat, string txtrate, int createuser, DateTime createdate, int lastmodifyuser, out string ResultSlab, out decimal maxrange)
        {
            ProcedureExecute proc = new ProcedureExecute("CFSlabInsert");

            proc.AddCharPara("@CFSlab_Code", 4, CFSlab_Code);

            proc.AddVarcharPara("@CFSlab_Mode", 5, CFSlab_Mode);

            proc.AddDecimalPara("@CFSlab_MinRange", 6, 18, CFSlab_MinRange);

            proc.AddDecimalPara("@CFSlab_MaxRange", 2, 18, CFSlab_MaxRange);

            if (txtflat != "")
                proc.AddDecimalPara("@CFSlab_FlatRate", 2, 12, Convert.ToDecimal(txtflat));
            else
                proc.AddDecimalPara("@CFSlab_FlatRate", 2, 12, Convert.ToDecimal("0"));

            if (txtrate != "")
                proc.AddDecimalPara("@CFSlab_Rate", 4, 8, Convert.ToDecimal(txtrate));
            else
                proc.AddDecimalPara("@CFSlab_Rate", 4, 8, Convert.ToDecimal("0"));

            proc.AddIntegerPara("@CFSlab_CreateUser", createuser);
            proc.AddDateTimePara("@CFSlab_CreateDateTime", createdate);
            proc.AddIntegerPara("@CFSlab_ModifyUser", lastmodifyuser);

            proc.AddVarcharPara("@ResultSlab", 20, "", QueryParameterDirection.Output);
            proc.AddDecimalPara("@maxrange", 6, 18, 0, QueryParameterDirection.Output);

            ResultSlab = proc.GetParaValue("@ResultSlab").ToString();
            maxrange = Convert.ToDecimal(proc.GetParaValue("@maxrange").ToString());
            proc.RunActionQuery();
        }


        public DataSet management_Report_ClientRiskPopUp(string ClientId, string Segment, string Companyid, string FinYear,
            string RptType, string ApplyHaircut, string PendingPurSalevalueMethod)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Report_ClientRiskPopUp");

            proc.AddVarcharPara("@ClientId", 50, ClientId);
            proc.AddVarcharPara("@Segment", 30, Segment);
            proc.AddVarcharPara("@Companyid", 15, Companyid);
            proc.AddVarcharPara("@FinYear", 15, FinYear);
            proc.AddVarcharPara("@RptType", 20, RptType);
            proc.AddVarcharPara("@ApplyHaircut", 5, ApplyHaircut);
            proc.AddVarcharPara("@PendingPurSalevalueMethod", 10, PendingPurSalevalueMethod);

            ds = proc.GetDataSet();

            return ds;
        }


        public void management_insertCommission(string CommissionProfile_CntID, string CommissionProfile_CompanyID, string CommissionProfile_GroupCode,
            DateTime CommissionProfile_FromDate, int CommissionProfile_CreateUser, DateTime CommissionProfile_CreateDateTime)
        {
            ProcedureExecute proc = new ProcedureExecute("insertCommission");

            proc.AddVarcharPara("@CommissionProfile_CntID", 10, CommissionProfile_CntID);
            proc.AddVarcharPara("@CommissionProfile_CompanyID", 10, CommissionProfile_CompanyID);

            proc.AddIntegerPara("@CommissionProfile_Type", 1);

            proc.AddVarcharPara("@CommissionProfile_GroupCode", 50, CommissionProfile_GroupCode);

            proc.AddDateTimePara("@CommissionProfile_FromDate", CommissionProfile_FromDate);

            proc.AddIntegerPara("@CommissionProfile_CreateUser", CommissionProfile_CreateUser);

            proc.AddDateTimePara("@CommissionProfile_CreateDateTime", CommissionProfile_CreateDateTime);

            proc.RunActionQuery();
        }

        public DataSet management_Process_ArbPLDateRangeCOM(string ArbCompanyID, string ArbFinYear, string ArbCycleID,
            string ArbFromDate, string ArbToDate, string ArbCreateUser, string ArbType)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Process_ArbPLDateRangeCOM");

            proc.AddVarcharPara("@ArbCompanyID", 15, ArbCompanyID);
            proc.AddVarcharPara("@ArbFinYear", 15, ArbFinYear);
            proc.AddVarcharPara("@ArbCycleID", 15, ArbCycleID);
            proc.AddVarcharPara("@ArbFromDate", 35, ArbFromDate);
            proc.AddVarcharPara("@ArbToDate", 35, ArbToDate);
            proc.AddVarcharPara("@ArbCreateUser", 10, ArbCreateUser);
            proc.AddVarcharPara("@ArbType", 5, ArbType);

            ds = proc.GetDataSet();

            return ds;
        }

        public DataSet management_Process_ArbPLDateRange(string ArbCompanyID, string ArbFinYear, string ArbCycleID,
           string ArbFromDate, string ArbToDate, string ArbCreateUser, string ArbType)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Process_ArbPLDateRange");

            proc.AddVarcharPara("@ArbCompanyID", 15, ArbCompanyID);
            proc.AddVarcharPara("@ArbFinYear", 15, ArbFinYear);
            proc.AddVarcharPara("@ArbCycleID", 15, ArbCycleID);
            proc.AddVarcharPara("@ArbFromDate", 35, ArbFromDate);
            proc.AddVarcharPara("@ArbToDate", 35, ArbToDate);
            proc.AddVarcharPara("@ArbCreateUser", 10, ArbCreateUser);
            proc.AddVarcharPara("@ArbType", 5, ArbType);

            ds = proc.GetDataSet();

            return ds;
        }

        //public DataSet management_Process_ArbPLDateRange(string ArbCompanyID, string ArbFinYear, string ArbCycleID,
        //  string ArbFromDate, string ArbToDate, string ArbCreateUser, string ArbType)
        //{
        //    DataSet ds = new DataSet();
        //    ProcedureExecute proc = new ProcedureExecute("Process_ArbPLDateRange");

        //    proc.AddVarcharPara("@ArbCompanyID", 15, ArbCompanyID);
        //    proc.AddVarcharPara("@ArbFinYear", 15, ArbFinYear);
        //    proc.AddVarcharPara("@ArbCycleID", 15, ArbCycleID);
        //    proc.AddVarcharPara("@ArbFromDate", 35, ArbFromDate);
        //    proc.AddVarcharPara("@ArbToDate", 35, ArbToDate);
        //    proc.AddVarcharPara("@ArbCreateUser", 10, ArbCreateUser);
        //    proc.AddVarcharPara("@ArbType", 5, ArbType);

        //    ds = proc.GetDataSet();

        //    return ds;
        //}


        public void management_CttRateInsert(char ddlcmpname, int ddlsegmentCount, int ddlsegmentValue, string Dateedit, string txtdelpurchase,
            string txtdelsale, string txtsoff, string txtdelpurchaseETF, string txtdelsaleETF, string txtsoffETF, string ddlauction, string txtfpurchase,
            string txtfsale, string txtopurchase, string txtfsrate, string txtfexpiry, string txtosale, string ddlfinalbasis, string ddlbasis, string ddlcharge,
            string ddlrmainacc, string ddlunracc, string ddlproacc, int createuser, DateTime createdate, int lastmodifyuser, string txtGroup_hidden,
            out Int64 ResultId)
        {
            ProcedureExecute proc = new ProcedureExecute("CttRateInsert");

            if (ddlcmpname != null)
                proc.AddCharPara("@CTTRates_CompanyID", 10, ddlcmpname);
            else
                proc.AddCharPara("@CTTRates_CompanyID", 10, Convert.ToChar(""));
            if (ddlsegmentCount != 0)
                proc.AddIntegerPara("@CttRates_ExchangeSegmentID", ddlsegmentValue);
            else
                proc.AddIntegerNullPara("@CttRates_ExchangeSegmentID");//, "");
            if (Dateedit != null)
            {
                proc.AddDateTimePara("@CttRates_DateFrom", Convert.ToDateTime(Dateedit));
            }
            //else
            //{
            //    proc.AddDateTimenPara("@CTTRates_DateFrom", "");          //}
            if (txtdelpurchase != "")
                proc.AddDecimalPara("@CTTRates_RateDelBuy", 6, 8, Convert.ToDecimal(txtdelpurchase));
            else
                proc.AddDecimalPara("@CTTRates_RateDelBuy", 6, 8, Convert.ToDecimal("0"));

            if (txtdelsale != "")
                proc.AddDecimalPara("@CTTRates_RateDelSale", 6, 8, Convert.ToDecimal(txtdelsale));
            else
                proc.AddDecimalPara("@CTTRates_RateDelSale", 6, 8, Convert.ToDecimal("0"));

            if (txtsoff != "")
                proc.AddDecimalPara("@CTTRates_RateSqr", 6, 8, Convert.ToDecimal(txtsoff));
            else
                proc.AddDecimalPara("@CTTRates_RateSqr", 6, 8, Convert.ToDecimal("0"));

            //New Addition For ETF
            if (txtdelpurchaseETF != "")
                proc.AddDecimalPara("@CttRates_RateDelBuyETF", 6, 8, Convert.ToDecimal(txtdelpurchaseETF));
            else
                proc.AddDecimalPara("@CTTRates_RateDelBuyETF", 6, 8, Convert.ToDecimal("0"));

            if (txtdelsaleETF != "")
                proc.AddDecimalPara("@CTTRates_RateDelSaleETF", 6, 8, Convert.ToDecimal(txtdelsaleETF));
            else
                proc.AddDecimalPara("@CTTRates_RateDelSaleETF", 6, 8, Convert.ToDecimal("0"));

            if (txtsoffETF != "")
                proc.AddDecimalPara("@CTTRates_RateSqrETF", 6, 8, Convert.ToDecimal(txtsoffETF));
            else
                proc.AddDecimalPara("@CTTRates_RateSqrETF", 6, 8, Convert.ToDecimal("0"));
            //End New Addition For ETF
            if (ddlauction != null)
                proc.AddCharPara("@CTTRates_OnAuction", 3, Convert.ToChar(ddlauction));
            else
                proc.AddCharPara("@CTTRates_OnAuction", 3, Convert.ToChar(""));

            if (txtfpurchase != "")
                proc.AddDecimalPara("@CTTRates_RateFutBuy", 6, 8, Convert.ToDecimal(txtfpurchase));
            else
                proc.AddDecimalPara("@CTTRates_RateFutBuy", 6, 8, Convert.ToDecimal("0"));

            if (txtfsale != "")
                proc.AddDecimalPara("@CTTRates_RateFutSale", 6, 8, Convert.ToDecimal(txtfsale));
            else
                proc.AddDecimalPara("@CTTRates_RateFutSale", 6, 8, Convert.ToDecimal("0"));


            if (txtopurchase != "")
                proc.AddDecimalPara("@CTTRates_RateOptBuy", 6, 8, Convert.ToDecimal(txtopurchase));
            else
                proc.AddDecimalPara("@CTTRates_RateOptBuy", 6, 8, Convert.ToDecimal("0"));

            if (txtfsrate != "")
                proc.AddDecimalPara("@CTTRates_RateFS", 6, 8, Convert.ToDecimal(txtfsrate));
            else
                proc.AddDecimalPara("@CTTRates_RateFS", 6, 8, Convert.ToDecimal("0"));

            if (txtfexpiry != "")
                proc.AddDecimalPara("@CTTRates_RateFutExp", 6, 8, Convert.ToDecimal(txtfexpiry));
            else
                proc.AddDecimalPara("@CTTRates_RateFutExp", 6, 8, Convert.ToDecimal("0"));

            if (txtosale != "")
                proc.AddDecimalPara("@CTTRates_RateOptSale", 6, 8, Convert.ToDecimal(txtosale));
            else
                proc.AddDecimalPara("@CTTRates_RateOptSale", 6, 8, Convert.ToDecimal("0"));
            proc.AddVarcharPara("@CTTRates_FSBasis", 20, ddlfinalbasis);
            proc.AddVarcharPara("@CTTRates_OptBasis", 20, ddlbasis);
            if (ddlcharge != null)
                proc.AddVarcharPara("@CTTRates_ApplicableFor", 20, ddlcharge);
            else
                proc.AddVarcharPara("@CTTRates_ApplicableFor", 20, " ");
            if (ddlrmainacc != null)
                proc.AddVarcharPara("@CTTRates_GnAcCodeControl", 10, ddlrmainacc);
            else
                proc.AddVarcharPara("@CTTRates_GnAcCodeControl", 10, "");

            if (ddlunracc != null)
                proc.AddVarcharPara("@CTTRates_GnAcCodeUnrealised", 10, ddlunracc);
            else
                proc.AddVarcharPara("@CTTRates_GnAcCodeUnrealised", 10, "");
            if (ddlproacc != null)
                proc.AddVarcharPara("@CTTRates_GnAcCodePro", 10, ddlproacc);
            else
                proc.AddVarcharPara("@CTTRates_GnAcCodePro", 10, "");


            proc.AddIntegerPara("@CTTRates_CreateUser", createuser);
            proc.AddDateTimePara("@CTTRates_CreateDateTime", createdate);
            proc.AddIntegerPara("@CTTRates_ModifyUser", lastmodifyuser);
            if (txtGroup_hidden != "")
                proc.AddCharPara("@CTTRates_ChargeGroupID", 10, Convert.ToChar(txtGroup_hidden));
            else
                proc.AddCharPara("@CTTRates_ChargeGroupID", 10, Convert.ToChar(""));

            proc.AddBigIntegerNullPara("@ResultId", QueryParameterDirection.Output);

            ResultId = Convert.ToInt64(proc.GetParaValue("@ResultId").ToString());

            proc.RunActionQuery();
        }


        public void management_ClearingInsert(string ddlcmpnameSelectedItem,
            string ddlcmpnameSelectedItemValue,
            int ddlsegmentItemsCount,
            string ddlsegmentSelectedItemValue,
            string DateeditValue,
            string txtrate1Text,
            string txttimeText,
            string ASPxtxtratepassiveText,
            string txtfuturerateText,
            string txtfutureratePOText,
            string txtstockfutureText,
            string txtstockfuturePOText,
            string txtindexfuturerateText,
            string txtindexfutureratePOText,
            string txtratefutureexpiryText,
            string txtratefutureexpiryPOText,
            string ddlbasisSelectedItemValue,
            string txtallrateoptionText,
            string txtallrateoptionPOText,
            string txtratestockoptionText,
            string txtratestockoptionPOText,
            string txtallindexrateoptionText,
            string txtallindexrateoptionPOText,
            string ddlfinalbasisSelectedItemValue,
            string txtratefinalallstatementText,
            string txtratefinalallstatementPOText,
            string txtratefinalstockstatementText,
            string txtratefinalstockstatementPOText,
            string txtratefinalindexstatementText,
            string txtratefinalindexstatementPOText,
            string txtipcrateText,
            string ddltaxappSelectedItem,
            string ddltaxappSelectedItemValue,
            string ddlchargeSelectedItem,
            string ddlchargeSelectedItemValue,
            string createuser,
            string createdate,
            string lastmodifyuser,
            string txtGroup_hiddenText)
        {
          //  String con = ConfigurationSettings.AppSettings["DBConnectionDefault"]; MULTI

            String con = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlConnection lcon = new SqlConnection(con);
            lcon.Open();
            SqlCommand lcmdTransInsert = new SqlCommand("ClearingInsert", lcon);
            lcmdTransInsert.CommandType = CommandType.StoredProcedure;

            if (ddlcmpnameSelectedItem != null)
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_CompanyID", ddlcmpnameSelectedItemValue);
            else
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_CompanyID", "");
            if (ddlsegmentItemsCount != 0)
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_ExchangeSegmentID", ddlsegmentSelectedItemValue);
            else
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_ExchangeSegmentID", "");
            if (DateeditValue != null)
            {
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_DateFrom", Convert.ToDateTime(DateeditValue).ToString("MM-dd-yyyy HH:mm:ss"));
            }
            else
            {
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_DateFrom", "");
            }
            if (txtrate1Text != "")
            {
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_Rate1", Convert.ToDecimal(txtrate1Text));
            }
            else
            {
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_Rate1", Convert.ToDecimal("0"));
            }
            if (txttimeText != "")
            {
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_Time", txttimeText);
            }
            else
            {
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_Time", "");
            }
            if (ASPxtxtratepassiveText != "")
            {
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_PORate", Convert.ToDecimal(ASPxtxtratepassiveText));
            }
            else
            {
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_PORate", Convert.ToDecimal("0"));
            }
            if (txtfuturerateText != "")
            {
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_RateFut", Convert.ToDecimal(txtfuturerateText));
            }
            else
            {
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_RateFut", Convert.ToDecimal("0"));
            }
            if (txtfutureratePOText != "")
            {
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_RateFutPO", Convert.ToDecimal(txtfutureratePOText));
            }
            else
            {
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_RateFutPO", Convert.ToDecimal("0"));
            }
            if (txtstockfutureText != "")
            {
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_RateFutStk", Convert.ToDecimal(txtstockfutureText));
            }
            else
            {
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_RateFutStk", Convert.ToDecimal("0"));
            }
            if (txtstockfuturePOText != "")
            {
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_RateFutStkPO", Convert.ToDecimal(txtstockfuturePOText));
            }
            else
            {
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_RateFutStkPO", Convert.ToDecimal("0"));
            }
            if (txtindexfuturerateText != "")
            {
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_RateFutIdx", Convert.ToDecimal(txtindexfuturerateText));
            }
            else
            {
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_RateFutIdx", Convert.ToDecimal("0"));
            }
            if (txtindexfutureratePOText != "")
            {
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_RateFutIdxPO", Convert.ToDecimal(txtindexfutureratePOText));
            }
            else
            {
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_RateFutIdxPO", Convert.ToDecimal("0"));
            }
            if (txtratefutureexpiryText != "")
            {
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_RateFutExp", Convert.ToDecimal(txtratefutureexpiryText));
            }
            else
            {
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_RateFutExp", Convert.ToDecimal("0"));
            }
            if (txtratefutureexpiryPOText != "")
            {
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_RateFutExpPO", Convert.ToDecimal(txtratefutureexpiryPOText));
            }
            else
            {
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_RateFutExpPO", Convert.ToDecimal("0"));
            }
            lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_OptBasis", ddlbasisSelectedItemValue);
            if (txtallrateoptionText != "")
            {
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_RateOpt", Convert.ToDecimal(txtallrateoptionText));
            }
            else
            {
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_RateOpt", Convert.ToDecimal("0"));
            }
            if (txtallrateoptionPOText != "")
            {
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_RateOptPO", Convert.ToDecimal(txtallrateoptionPOText));
            }
            else
            {
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_RateOptPO", Convert.ToDecimal("0"));
            }
            if (txtratestockoptionText != "")
            {
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_RateOptStk", Convert.ToDecimal(txtratestockoptionText));
            }
            else
            {
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_RateOptStk", Convert.ToDecimal("0"));
            }
            if (txtratestockoptionPOText != "")
            {
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_RateOptStkPO", Convert.ToDecimal(txtratestockoptionPOText));
            }
            else
            {
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_RateOptStkPO", Convert.ToDecimal("0"));
            }
            if (txtallindexrateoptionText != "")
            {
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_RateOptIdx", Convert.ToDecimal(txtallindexrateoptionText));
            }
            else
            {
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_RateOptIdx", Convert.ToDecimal("0"));
            }
            if (txtallindexrateoptionPOText != "")
            {
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_RateOptIdxPO", Convert.ToDecimal(txtallindexrateoptionPOText));
            }
            else
            {
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_RateOptIdxPO", Convert.ToDecimal("0"));
            }
            lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_OptFSBasis", ddlfinalbasisSelectedItemValue);

            if (txtratefinalallstatementText != "")
            {
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_RateOptFS", Convert.ToDecimal(txtratefinalallstatementText));
            }
            else
            {
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_RateOptFS", Convert.ToDecimal("0"));
            }
            if (txtratefinalallstatementPOText != "")
            {
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_RateOptFSPO", Convert.ToDecimal(txtratefinalallstatementPOText));
            }
            else
            {
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_RateOptFSPO", Convert.ToDecimal("0"));
            }
            if (txtratefinalstockstatementText != "")
            {
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_RateOptStkFS", Convert.ToDecimal(txtratefinalstockstatementText));
            }
            else
            {
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_RateOptStkFS", Convert.ToDecimal("0"));
            }
            if (txtratefinalstockstatementPOText != "")
            {
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_RateOptStkFSPO", Convert.ToDecimal(txtratefinalstockstatementPOText));
            }
            else
            {
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_RateOptStkFSPO", Convert.ToDecimal("0"));
            }
            if (txtratefinalindexstatementText != "")
            {
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_RateOptIdxFS", Convert.ToDecimal(txtratefinalindexstatementText));
            }
            else
            {
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_RateOptIdxFS", Convert.ToDecimal("0"));
            }
            if (txtratefinalindexstatementPOText != "")
            {
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_RateOptIdxFSPO", Convert.ToDecimal(txtratefinalindexstatementPOText));
            }
            else
            {
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_RateOptIdxFSPO", Convert.ToDecimal("0"));
            }
            if (txtipcrateText != "")
            {
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_RateIPC", Convert.ToDecimal(txtipcrateText));
            }
            else
            {
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_RateIPC", Convert.ToDecimal("0"));
            }
            if (ddltaxappSelectedItem != null)
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_STApplicable", ddltaxappSelectedItemValue);
            else
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_STApplicable", "");
            if (ddlchargeSelectedItem != null)
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_ApplicableFor", ddlchargeSelectedItemValue);
            else
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_ApplicableFor", "");

            lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_GnAcCodeClients", "");

            lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_SbAcCodeClients", "");

            lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_GnAcCodeExch", "");

            lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_SbAcCodeExch", "");

            lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_GnAcCodeST", "");

            lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_SbAcCodeST", "");
            lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_CreateUser", createuser);
            lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_CreateDateTime", createdate);
            lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_ModifyUser", lastmodifyuser);
            if (txtGroup_hiddenText != "")
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_ChargeGroupID", txtGroup_hiddenText);
            else
                lcmdTransInsert.Parameters.AddWithValue("@ClearingCharge_ChargeGroupID", "");
            lcmdTransInsert.ExecuteNonQuery();
        }

        public void management_OtherChargeInsert(
            int ddlsegmentItemsCount,
            string ddlsegmentSelectedItemValue,
            string ddlcmpnameSelectedItemValue,
            string txtrateText,
            int ddlslabSelectedIndex,
            string ddlslabSelectedItemValue,
            string ddlchargemasterSelectedItemValue,
            string DateeditValue,
            string ddltaxappSelectedItemValue,
            string ddlchargeSelectedItemValue,
            string ddltransclientmainSelectedItemValue,
            string ddlotherchargemainSelectedItemValue,
            int ddlotherchargesubItemsCount,
            string ddlotherchargesubSelectedItemValue,
            string ddlservtaxmainSelectedItemValue,
            int ddlservtaxsubItemsCount,
            string ddlservtaxsubSelectedItemValue,
            string createuser,
            string createdate,
            string lastmodifyuser,
            string txtGroup_hiddenText
            )
        {

            //String con = ConfigurationSettings.AppSettings["DBConnectionDefault"]; MULTI
            String con = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            SqlConnection lcon = new SqlConnection(con);
            lcon.Open();
            SqlCommand lcmdOtherChargeInsert = new SqlCommand("OtherChargeInsert", lcon);
            lcmdOtherChargeInsert.CommandType = CommandType.StoredProcedure;

            SqlParameter parameter = new SqlParameter("@ResultId", SqlDbType.BigInt);
            parameter.Direction = ParameterDirection.Output;

            if (ddlsegmentItemsCount != 0)
                lcmdOtherChargeInsert.Parameters.AddWithValue("@OtherCharge_ExchangeSegmentID", ddlsegmentSelectedItemValue);
            else
                lcmdOtherChargeInsert.Parameters.AddWithValue("@OtherCharge_ExchangeSegmentID", "");
            if (ddlcmpnameSelectedItemValue != null)
                lcmdOtherChargeInsert.Parameters.AddWithValue("@OtherCharge_CompanyID", ddlcmpnameSelectedItemValue);
            else
                lcmdOtherChargeInsert.Parameters.AddWithValue("@OtherCharge_CompanyID", "");

            if (txtrateText.Trim() == "0.000000")
                lcmdOtherChargeInsert.Parameters.AddWithValue("@OtherCharge_SlabID", " ");
            else
            {
                if (ddlslabSelectedIndex > 0)
                    lcmdOtherChargeInsert.Parameters.AddWithValue("@OtherCharge_SlabID", ddlslabSelectedItemValue);
                else
                    lcmdOtherChargeInsert.Parameters.AddWithValue("@OtherCharge_SlabID", " ");
            }

            if (ddlchargemasterSelectedItemValue != null)
                lcmdOtherChargeInsert.Parameters.AddWithValue("@OtherCharge_MasterID", ddlchargemasterSelectedItemValue);
            else
                lcmdOtherChargeInsert.Parameters.AddWithValue("@OtherCharge_MasterID", "");

            if (DateeditValue != null)
            {
                lcmdOtherChargeInsert.Parameters.AddWithValue("@OtherCharge_ApplicableFrom", DateeditValue);
            }
            else
            {
                lcmdOtherChargeInsert.Parameters.AddWithValue("@OtherCharge_ApplicableFrom", "");
            }

            if (txtrateText != "")
            {
                lcmdOtherChargeInsert.Parameters.AddWithValue("@OtherCharge_Rate", Convert.ToDecimal(txtrateText));
            }
            else
            {
                lcmdOtherChargeInsert.Parameters.AddWithValue("@OtherCharge_Rate", Convert.ToDecimal("0"));
            }

            if (ddltaxappSelectedItemValue != null)
                lcmdOtherChargeInsert.Parameters.AddWithValue("@OtherCharge_STApplicable", ddltaxappSelectedItemValue);
            else
                lcmdOtherChargeInsert.Parameters.AddWithValue("@OtherCharge_STApplicable", "");
            if (ddlchargeSelectedItemValue != null)
                lcmdOtherChargeInsert.Parameters.AddWithValue("@OtherCharge_ApplicableFor", ddlchargeSelectedItemValue);
            else
                lcmdOtherChargeInsert.Parameters.AddWithValue("@OtherCharge_ApplicableFor", "");

            if (ddltransclientmainSelectedItemValue != null)
                lcmdOtherChargeInsert.Parameters.AddWithValue("@OtherCharge_GnAcCodeClient", ddltransclientmainSelectedItemValue);
            else
                lcmdOtherChargeInsert.Parameters.AddWithValue("@OtherCharge_GnAcCodeClient", "");


            if (ddlotherchargemainSelectedItemValue != null)
                lcmdOtherChargeInsert.Parameters.AddWithValue("@OtherCharge_GnAcCodeCharge", ddlotherchargemainSelectedItemValue);
            else
                lcmdOtherChargeInsert.Parameters.AddWithValue("@OtherCharge_GnAcCodeCharge", "");
            if (ddlotherchargesubItemsCount != 0)
                lcmdOtherChargeInsert.Parameters.AddWithValue("@OtherCharge_SbAcCodeCharge", ddlotherchargesubSelectedItemValue);
            else
                lcmdOtherChargeInsert.Parameters.AddWithValue("@OtherCharge_SbAcCodeCharge", "");

            if (ddlservtaxmainSelectedItemValue != null)
                lcmdOtherChargeInsert.Parameters.AddWithValue("@OtherCharge_GnAcCodeST", ddlservtaxmainSelectedItemValue);
            else
                lcmdOtherChargeInsert.Parameters.AddWithValue("@OtherCharge_GnAcCodeST", "");
            if (ddlservtaxsubItemsCount != 0)
                lcmdOtherChargeInsert.Parameters.AddWithValue("@OtherCharge_SbAcCodeST", ddlservtaxsubSelectedItemValue);
            else
                lcmdOtherChargeInsert.Parameters.AddWithValue("@OtherCharge_SbAcCodeST", "");

            lcmdOtherChargeInsert.Parameters.AddWithValue("@OtherCharge_CreateUser", createuser);
            lcmdOtherChargeInsert.Parameters.AddWithValue("@OtherCharge_CreateDateTime", createdate);
            lcmdOtherChargeInsert.Parameters.AddWithValue("@OtherCharge_ModifyUser", lastmodifyuser);
            if (txtGroup_hiddenText != "")
                lcmdOtherChargeInsert.Parameters.AddWithValue("@OtherCharge_ChargeGroupID", txtGroup_hiddenText);
            else
                lcmdOtherChargeInsert.Parameters.AddWithValue("@OtherCharge_ChargeGroupID", "");
            lcmdOtherChargeInsert.Parameters.Add(parameter);
            lcmdOtherChargeInsert.ExecuteNonQuery();
        }

        public DataSet management_CheckingForInstrumentNumber(string bankId, string ChkNumber)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("CheckingForInstrumentNumber");

            proc.AddVarcharPara("@bankId", 20, bankId);
            proc.AddVarcharPara("@ChkNumber", -1, ChkNumber);

            ds = proc.GetDataSet();

            return ds;
        }

        public void management_xmlCashBankInsert(string cashBankData, string createuser, string FinYear,
            string compID, string CashBankName, DateTime TDate)
        {

            ProcedureExecute proc = new ProcedureExecute("xmlCashBankInsert");

            proc.AddNTextPara("@cashBankData", cashBankData);

            proc.AddVarcharPara("@createuser", 10, createuser);

            proc.AddVarcharPara("@finyear", 12, FinYear);

            proc.AddVarcharPara("@compID", 15, compID);
            proc.AddVarcharPara("@CashBankName", 500, CashBankName);

            proc.AddDateTimePara("@TDate", TDate);

            proc.RunActionQuery();
        }

        public void management_xmlCashBankUpdate(string cashBankData, string createuser, string finyear,
           string compID, string CashBankName, DateTime Olddate, DateTime TDate, string cashBankName1, string NewSegment, string OldSegment)
        {

            ProcedureExecute proc = new ProcedureExecute("xmlCashBankUpdate");

            proc.AddNTextPara("@cashBankData", cashBankData);
            proc.AddVarcharPara("@createuser", 10, createuser);
            proc.AddVarcharPara("@finyear", 12, finyear);
            proc.AddVarcharPara("@compID", 15, compID);

            proc.AddVarcharPara("@CashBankName", 500, CashBankName);

            proc.AddDateTimePara("@OldDate", Olddate);

            proc.AddDateTimePara("@TDate", TDate);

            proc.AddVarcharPara("@cashBankName1", 20, cashBankName1);

            proc.AddVarcharPara("@NewSegment", 20, NewSegment);
            proc.AddVarcharPara("@OldSegment", 20, OldSegment);

            proc.RunActionQuery();
        }


        public DataSet FetchOfflineImportedDataVerification(string date, string WhichRecord, string WhichDp)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Fetch_OfflineImportedData_Verification");
            proc.AddVarcharPara("@date", 50, date);
            proc.AddVarcharPara("@WhichRecord", 50, WhichRecord);
            proc.AddVarcharPara("@WhichDp", 50, WhichDp);
            ds = proc.GetDataSet();

            return ds;
        }

        public int Verify_OfflineImport_Verification(string doc, string WhichDP, string VerifierUserID)
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            try
            {
                using (proc = new ProcedureExecute("Verify_OfflineImport_Verification"))
                {
                    proc.AddVarcharPara("@doc", -1, doc);
                    proc.AddVarcharPara("@WhichDP", 100, WhichDP);
                    proc.AddVarcharPara("@VerifierUserID", 50, VerifierUserID);
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

        public int Verify_DeliveryInstruction_Verification(string doc, string WhichDP, string VerifierUserID)
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            try
            {
                using (proc = new ProcedureExecute("Verify_DeliveryInstruction_Verification"))
                {
                    proc.AddVarcharPara("@doc", -1, doc);
                    proc.AddVarcharPara("@WhichDP", 100, WhichDP);
                    proc.AddVarcharPara("@VerifierUserID", 50, VerifierUserID);
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

        public DataSet Processing_GenerateCFFecth(string CompanyId, string Segment, string SettlementNumber, string SettlementType,
            string Date, string FinYear, string MasterSegment)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("[Processing_GenerateCFFecth]");
            proc.AddVarcharPara("@CompanyId", 50, CompanyId);
            proc.AddVarcharPara("@Segment", 50, Segment);
            proc.AddVarcharPara("@SettlementNumber", 50, SettlementNumber);
            proc.AddVarcharPara("@SettlementType", 50, SettlementType);
            proc.AddVarcharPara("@Date", 50, Date);
            proc.AddVarcharPara("@FinYear", 50, FinYear);
            proc.AddVarcharPara("@MasterSegment", 50, MasterSegment);
            ds = proc.GetDataSet();

            return ds;
        }

        public int Processing_GenerateCF(string AllData, string CompanyId, string Segment, string Date, string Finyear, int Creatuser,
            string MasterSegment)
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            try
            {
                using (proc = new ProcedureExecute("[Processing_GenerateCF]"))
                {
                    proc.AddNTextPara("@AllData", AllData);
                    proc.AddVarcharPara("@CompanyId", 100, CompanyId);
                    proc.AddVarcharPara("@Segment", 50, Segment);
                    proc.AddVarcharPara("@Date", 50, Date);
                    proc.AddVarcharPara("@Finyear", 100, Finyear);
                    proc.AddIntegerPara("@Creatuser", Creatuser);
                    proc.AddVarcharPara("@MasterSegment", 50, MasterSegment);
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

        public DataSet Processing_CFMTMDateRange(string CompanyId, string Segment, string FromDate, string ToDate, string FinYear, string MasterSegment,
            string SettlementType, int CreateUser)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("[Processing_CFMTMDateRange]");
            proc.AddVarcharPara("@CompanyId", 50, CompanyId);
            proc.AddVarcharPara("@Segment", 50, Segment);
            proc.AddVarcharPara("@FromDate", 50, FromDate);
            proc.AddVarcharPara("@ToDate", 50, ToDate);
            proc.AddVarcharPara("@FinYear", 50, FinYear);
            proc.AddVarcharPara("@MasterSegment", 50, MasterSegment);
            proc.AddVarcharPara("@SettlementType", 50, SettlementType);
            proc.AddIntegerPara("@CreateUser", CreateUser);
            ds = proc.GetDataSet();

            return ds;
        }

        public DataSet Process_ArbPLCloseOut(string ArbCompanyID, string ArbFinYear, string ArbCycleID, string ArbCreateUser)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("[Process_ArbPLCloseOut]");
            proc.AddVarcharPara("@ArbCompanyID", 50, ArbCompanyID);
            proc.AddVarcharPara("@ArbFinYear", 50, ArbFinYear);
            proc.AddVarcharPara("@ArbCycleID", 50, ArbCycleID);
            proc.AddVarcharPara("@ArbCreateUser", 50, ArbCreateUser);
            ds = proc.GetDataSet();

            return ds;
        }

        public DataSet Delete_ArbPLCloseOut(string ArbCompanyID, string ArbFinYear, string ArbCycleID, string Param)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("[Delete_ArbPLCloseOut]");
            proc.AddVarcharPara("@ArbCompanyID", 50, ArbCompanyID);
            proc.AddVarcharPara("@ArbFinYear", 50, ArbFinYear);
            proc.AddVarcharPara("@ArbCycleID", 50, ArbCycleID);
            proc.AddVarcharPara("@Param", 50, Param);
            ds = proc.GetDataSet();

            return ds;
        }

        public DataSet Process_ArbPLCloseOutCOM(string ArbCompanyID, string ArbFinYear, string ArbCycleID, string ArbCreateUser)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("[[Process_ArbPLCloseOutCOM]]");
            proc.AddVarcharPara("@ArbCompanyID", 50, ArbCompanyID);
            proc.AddVarcharPara("@ArbFinYear", 50, ArbFinYear);
            proc.AddVarcharPara("@ArbCycleID", 50, ArbCycleID);
            proc.AddVarcharPara("@ArbCreateUser", 50, ArbCreateUser);
            ds = proc.GetDataSet();

            return ds;
        }

        public DataSet Delete_ArbPLCloseOutCOM(string ArbCompanyID, string ArbFinYear, string ArbCycleID, string Param)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("[Delete_ArbPLCloseOutCOM]");
            proc.AddVarcharPara("@ArbCompanyID", 50, ArbCompanyID);
            proc.AddVarcharPara("@ArbFinYear", 50, ArbFinYear);
            proc.AddVarcharPara("@ArbCycleID", 50, ArbCycleID);
            proc.AddVarcharPara("@Param", 50, Param);
            ds = proc.GetDataSet();

            return ds;
        }

        public void SP_import_dpslips(string FilePath, string FilePath1, string user)
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            try
            {
                using (proc = new ProcedureExecute("[SP_import_dpslips]"))
                {
                    proc.AddVarcharPara("@FilePath", 500, FilePath);
                    proc.AddVarcharPara("@FilePath1", 500, FilePath1);
                    proc.AddVarcharPara("@user", 100, user);
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

        public DataSet InterestCalculation(string CompanyId, string SegmentID, string Segment, string FinYear, string FromDate,
            string ToDate, string MainAccount, string Client, int CreateUser, string GenType)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("[InterestCalculation]");
            proc.AddVarcharPara("@CompanyId", 50, CompanyId);
            proc.AddVarcharPara("@SegmentID", 50, SegmentID);
            proc.AddVarcharPara("@Segment", 50, Segment);
            proc.AddVarcharPara("@FinYear", 50, FinYear);
            proc.AddVarcharPara("@FromDate", 50, FromDate);
            proc.AddVarcharPara("@ToDate", 50, ToDate);
            proc.AddVarcharPara("@MainAccount", 50, MainAccount);
            proc.AddVarcharPara("@Client", 50, Client);
            proc.AddIntegerPara("@CreateUser", CreateUser);
            proc.AddVarcharPara("@GenType", 50, GenType);
            ds = proc.GetDataSet();

            return ds;
        }

        public int InterestCalculation_1(string AllData, string CompanyId, string Segment, string FromDate, string ToDate, string Finyear, int CreateUser,
            string MainAccountSubLedgerType)
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            try
            {
                using (proc = new ProcedureExecute("[InterestCalculation]"))
                {
                    proc.AddVarcharPara("@AllData", -1, AllData);
                    proc.AddVarcharPara("@CompanyId", 100, CompanyId);
                    proc.AddVarcharPara("@Segment", 50, Segment);
                    proc.AddVarcharPara("@FromDate", 50, FromDate);
                    proc.AddVarcharPara("@ToDate", 100, ToDate);
                    proc.AddVarcharPara("@Finyear", 50, Finyear);
                    proc.AddIntegerPara("@CreateUser", CreateUser);
                    proc.AddVarcharPara("@MainAccountSubLedgerType", 100, MainAccountSubLedgerType);
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

        public void Insert_IntSlab(out string ResultIntSlab, out decimal IntSlabmaxRange, string IntSlab_Code, decimal IntSlab_AmntFrom, decimal IntSlab_AmntTo,
            decimal IntSlab_Rate, int IntSlab_CreateUser)
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            try
            {
                using (proc = new ProcedureExecute("[Insert_IntSlab]"))
                {
                    proc.AddVarcharPara("@ResultIntSlab", 100, "", QueryParameterDirection.Output);
                    proc.AddDecimalPara("@IntSlabmaxRange", 28, 6, 0, QueryParameterDirection.Output);
                    proc.AddVarcharPara("@IntSlab_Code", 100, IntSlab_Code);
                    proc.AddDecimalPara("@IntSlab_AmntFrom", 13, 0, IntSlab_AmntFrom);
                    proc.AddDecimalPara("@IntSlab_AmntTo", 13, 0, IntSlab_AmntTo);
                    proc.AddDecimalPara("@IntSlab_Rate", 5, 2, IntSlab_Rate);
                    proc.AddIntegerPara("@IntSlab_CreateUser", IntSlab_CreateUser);
                    ResultIntSlab = proc.GetParaValue("@ResultIntSlab").ToString();
                    IntSlabmaxRange = Convert.ToDecimal(proc.GetParaValue("@IntSlabmaxRange"));
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

        public void InsertUpdateDeleteFetch_InterestParameterDetails(int Mode, string XmlDetails, string User, int MainId)
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            try
            {
                using (proc = new ProcedureExecute("InsertUpdateDeleteFetch_InterestParameterDetails"))
                {
                    proc.AddIntegerPara("@Mode", Mode);
                    proc.AddXMLPara("@XmlDetails", XmlDetails);
                    proc.AddVarcharPara("@User", 50, User);
                    proc.AddIntegerPara("@MainId", MainId);
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

        public DataSet JournalVoucherDelete(string ID, string TDate, int SegmentID, int BranchID, string VNo, string CompID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("JournalVoucherDelete");
            proc.AddVarcharPara("@ID", 50, ID);
            proc.AddVarcharPara("@TDate", 50, TDate);
            proc.AddIntegerPara("@SegmentID", SegmentID);
            proc.AddIntegerPara("@BranchID", BranchID);
            proc.AddVarcharPara("@VNo", 50, VNo);
            proc.AddVarcharPara("@CompID", 50, CompID);
            ds = proc.GetDataSet();

            return ds;
        }

        public int xmlJournalVoucherInterSegmentInsert(string journalData, string createuser, string finyear, string compID, string JournalVoucher_Narration,
            string JournalVoucherDetail_TransactionDate, string JournalVoucher_SettlementNumber, string JournalVoucher_SettlementType, string JournalVoucher_BillNumber,
            string JournalVoucher_Prefix, string segmentid)
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            try
            {
                using (proc = new ProcedureExecute("xmlJournalVoucherInterSegmentInsert"))
                {
                    proc.AddNTextPara("@journalData", journalData);
                    proc.AddVarcharPara("@createuser", 100, createuser);
                    proc.AddVarcharPara("@finyear", 100, finyear);
                    proc.AddVarcharPara("@compID", 100, compID);
                    proc.AddVarcharPara("@JournalVoucher_Narration", 100, JournalVoucher_Narration);
                    proc.AddVarcharPara("@JournalVoucherDetail_TransactionDate", 100, JournalVoucherDetail_TransactionDate);
                    proc.AddVarcharPara("@JournalVoucher_SettlementNumber", 100, JournalVoucher_SettlementNumber);
                    proc.AddVarcharPara("@JournalVoucher_SettlementType", 100, JournalVoucher_SettlementType);
                    proc.AddVarcharPara("@JournalVoucher_BillNumber", 100, JournalVoucher_BillNumber);
                    proc.AddVarcharPara("@JournalVoucher_Prefix", 100, JournalVoucher_Prefix);
                    proc.AddVarcharPara("@segmentid", 100, segmentid);
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


        public DataSet xmlJournalVoucherInsert(string journalData, string createuser, string finyear, string compID, string JournalVoucher_Narration,
            string JournalVoucherDetail_TransactionDate, string JournalVoucher_SettlementNumber, string JournalVoucher_SettlementType, string JournalVoucher_BillNumber,
            string JournalVoucher_Prefix, string segmentid)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("xmlJournalVoucherInsert");
            proc.AddNTextPara("@journalData", journalData);
            proc.AddVarcharPara("@createuser", 100, createuser);
            proc.AddVarcharPara("@finyear", 100, finyear);
            proc.AddVarcharPara("@compID", 100, compID);
            proc.AddVarcharPara("@JournalVoucher_Narration", 100, JournalVoucher_Narration);
            proc.AddVarcharPara("@JournalVoucherDetail_TransactionDate", 100, JournalVoucherDetail_TransactionDate);
            proc.AddVarcharPara("@JournalVoucher_SettlementNumber", 100, JournalVoucher_SettlementNumber);
            proc.AddVarcharPara("@JournalVoucher_SettlementType", 100, JournalVoucher_SettlementType);
            proc.AddVarcharPara("@JournalVoucher_BillNumber", 100, JournalVoucher_BillNumber);
            proc.AddVarcharPara("@JournalVoucher_Prefix", 100, JournalVoucher_Prefix);
            proc.AddVarcharPara("@segmentid", 100, segmentid);
            ds = proc.GetDataSet();

            return ds;
        }

        public int deleteJournalVoucher(string companyId, int VoucherID)
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            try
            {
                using (proc = new ProcedureExecute("deleteJournalVoucher"))
                {
                    proc.AddVarcharPara("@companyId", 12, companyId);
                    proc.AddIntegerPara("@VoucherID", VoucherID);
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

        public int xmlJournalVoucherUpdate(string journalData, string createuser, string finyear, string compID, string JournalVoucher_Narration,
            string JournalVoucherDetail_TransactionDate, string JournalVoucher_SettlementNumber, string JournalVoucher_SettlementType, string JournalVoucher_BillNumber,
            string JournalVoucher_Prefix, string segmentid)
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            try
            {
                using (proc = new ProcedureExecute("xmlJournalVoucherUpdate"))
                {
                    proc.AddNTextPara("@journalData", journalData);
                    proc.AddVarcharPara("@createuser", 100, createuser);
                    proc.AddVarcharPara("@finyear", 100, finyear);
                    proc.AddVarcharPara("@compID", 100, compID);
                    proc.AddVarcharPara("@JournalVoucher_Narration", 100, JournalVoucher_Narration);
                    proc.AddVarcharPara("@JournalVoucherDetail_TransactionDate", 100, JournalVoucherDetail_TransactionDate);
                    proc.AddVarcharPara("@JournalVoucher_SettlementNumber", 100, JournalVoucher_SettlementNumber);
                    proc.AddVarcharPara("@JournalVoucher_SettlementType", 100, JournalVoucher_SettlementType);
                    proc.AddVarcharPara("@JournalVoucher_BillNumber", 100, JournalVoucher_BillNumber);
                    proc.AddVarcharPara("@JournalVoucher_Prefix", 100, JournalVoucher_Prefix);
                    proc.AddVarcharPara("@segmentid", 100, segmentid);
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

        public DataSet JournalVoucherPrintFromInsert(string journalData, string TransactionDate)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("JournalVoucherPrintFromInsert");
            proc.AddNTextPara("@journalData", journalData);
            proc.AddVarcharPara("@TransactionDate", 50, TransactionDate);
            ds = proc.GetDataSet();

            return ds;
        }

        public void xmlCashBankInsert(string cashBankData, string createuser, string finyear, string compID, string CashBankName, string TDate,
            int BRS)
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            try
            {
                using (proc = new ProcedureExecute("xmlCashBankInsert"))
                {
                    proc.AddNTextPara("@cashBankData", cashBankData);
                    proc.AddVarcharPara("@createuser", 100, createuser);
                    proc.AddVarcharPara("@finyear", 100, finyear);
                    proc.AddVarcharPara("@compID", 100, compID);
                    proc.AddVarcharPara("@CashBankName", 100, CashBankName);
                    proc.AddVarcharPara("@TDate", 100, TDate);
                    proc.AddIntegerPara("@BRS", BRS);
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

        public DataSet MonthlyPerformanceReportFO(string companyid, string segment, string fromdate, string todate, string clients, string MasterSegment,
            string Seriesid, string Expiry, string grptype, string chkoptmtm)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("[MonthlyPerformanceReportFO]");
            proc.AddVarcharPara("@companyid", 100, companyid);
            proc.AddVarcharPara("@segment", 50, segment);
            proc.AddVarcharPara("@fromdate", 50, fromdate);
            proc.AddVarcharPara("@todate", 50, todate);
            proc.AddVarcharPara("@clients", -1, clients);
            proc.AddVarcharPara("@MasterSegment", 50, MasterSegment);
            proc.AddVarcharPara("@Seriesid", 50, Seriesid);
            proc.AddVarcharPara("@Expiry", 50, Expiry);
            proc.AddVarcharPara("@grptype", 50, grptype);
            proc.AddVarcharPara("@chkoptmtm", 50, chkoptmtm);
            ds = proc.GetDataSet();

            return ds;
        }

        public DataSet Import_NselSpot(string Mode, string FilePath, string FileName, string Accountid, string CreatUser)
        {
            DataSet ds = new DataSet();
            if (Mode == "[Import_NselSpotNsdlTrn]")
            {
                ProcedureExecute proc = new ProcedureExecute("[Import_NselSpotNsdlTrn]");
                proc.AddVarcharPara("@FilePath", 200, FilePath);
                proc.AddVarcharPara("@FileName", 100, FileName);
                proc.AddVarcharPara("@Accountid", 100, Accountid);
                proc.AddVarcharPara("@CreatUser", 100, CreatUser);
                ds = proc.GetDataSet();
            }
            if (Mode == "[Import_NselSpotCDSLDPC9]")
            {
                ProcedureExecute proc = new ProcedureExecute("[Import_NselSpotCDSLDPC9]");
                proc.AddVarcharPara("@FilePath", 200, FilePath);
                proc.AddVarcharPara("@FileName", 100, FileName);
                proc.AddVarcharPara("@Accountid", 100, Accountid);
                proc.AddVarcharPara("@CreatUser", 100, CreatUser);
                ds = proc.GetDataSet();
            }
            return ds;
        }

        public DataTable Sp_CommClientSelect(string fromdate, string todate, string segment, string Companyid, string BranchId, string ExchangeSegmentID,
            string ClientsID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("[Sp_CommClientSelect]");
            proc.AddVarcharPara("@fromdate", 100, fromdate);
            proc.AddVarcharPara("@todate", 100, todate);
            proc.AddVarcharPara("@segment", -1, segment);
            proc.AddVarcharPara("@Companyid", -1, Companyid);
            proc.AddVarcharPara("@BranchId", -1, BranchId);
            proc.AddVarcharPara("@ExchangeSegmentID", 100, ExchangeSegmentID);
            proc.AddVarcharPara("@ClientsID", 100, ClientsID);
            dt = proc.GetTable();
            return dt;
        }

        public DataSet Sp_ObligationStatement(string segment, string Companyid, int MasterSegment, string ClientsID, string fromdate, string todate)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Sp_ObligationStatement");
            proc.AddVarcharPara("@segment", -1, segment);
            proc.AddVarcharPara("@Companyid", 100, Companyid);
            proc.AddIntegerPara("@MasterSegment", MasterSegment);
            proc.AddVarcharPara("@ClientsID", -1, ClientsID);
            proc.AddVarcharPara("@fromdate", 100, fromdate);
            proc.AddVarcharPara("@todate", 100, todate);
            ds = proc.GetDataSet();

            return ds;
        }

        public DataSet Sp_ObligationStatement_CRYSTAL(string fromdate, string todate, string segment, string Companyid, string MasterSegment, string ClientsID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("[Sp_ObligationStatement_CRYSTAL]");
            proc.AddVarcharPara("@fromdate", 100, fromdate);
            proc.AddVarcharPara("@todate", 100, todate);
            proc.AddVarcharPara("@segment", -1, segment);
            proc.AddVarcharPara("@Companyid", 100, Companyid);
            proc.AddVarcharPara("@MasterSegment", 100, MasterSegment);
            proc.AddVarcharPara("@ClientsID", -1, ClientsID);
            ds = proc.GetDataSet();

            return ds;
        }

        public DataSet Report_RateDateFetch(string Segment, string Companyid)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("[Report_RateDateFetch]");
            proc.AddVarcharPara("@Segment", 50, Segment);
            proc.AddVarcharPara("@Companyid", 50, Companyid);
            ds = proc.GetDataSet();
            return ds;
        }

        public int sp_Insert_Trans_NSDLOfflineAccont(string dp, string doc, string userid)
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            try
            {
                using (proc = new ProcedureExecute("sp_Insert_Trans_NSDLOfflineAccont"))
                {
                    proc.AddVarcharPara("@dp", 100, dp);
                    proc.AddVarcharPara("@doc", -1, doc);
                    proc.AddVarcharPara("@userid", 100, userid);
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

        public DataSet Fetch_slipRegister(string Fromdate, string Todate, string UsersegId, int UserLastsegment)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Fetch_slipRegister");
            proc.AddVarcharPara("@Fromdate", 50, Fromdate);
            proc.AddVarcharPara("@Todate", 50, Todate);
            proc.AddVarcharPara("@UsersegId", 50, UsersegId);
            proc.AddIntegerPara("@UserLastsegment", UserLastsegment);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet SEARCH_CONTACT_BYEMAIL(string SEARCH_ENTITY, string SEARCH_TYPE, string EMAIL_LIKE)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("SEARCH_CONTACT_BYEMAIL");
            proc.AddVarcharPara("@SEARCH_ENTITY", 150, SEARCH_ENTITY);
            proc.AddVarcharPara("@SEARCH_TYPE", 150, SEARCH_TYPE);
            proc.AddVarcharPara("@EMAIL_LIKE", 150, EMAIL_LIKE);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet Fetch_EmailTemplateReservedWord(string UsedFor, string ContactID, string CompanyID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("[Fetch_EmailTemplateReservedWord]");
            proc.AddVarcharPara("@UsedFor", 50, UsedFor);
            proc.AddVarcharPara("@ContactID", 50, ContactID);
            proc.AddVarcharPara("@CompanyID", 50, CompanyID);
            ds = proc.GetDataSet();
            return ds;
        }

        public void InsertTransEmail(string Emails_SenderEmailID, string Emails_Subject, string Emails_Content, string Emails_HasAttachement, int Emails_CreateApplication,
            int Emails_CreateUser, string Emails_Type, string Emails_CompanyID, string Emails_Segment, out long result)
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            try
            {
                using (proc = new ProcedureExecute("InsertTransEmail"))
                {
                    proc.AddVarcharPara("@Emails_SenderEmailID", 100, Emails_SenderEmailID);
                    proc.AddVarcharPara("@Emails_Subject", 100, Emails_Subject);
                    proc.AddVarcharPara("@Emails_Content", -1, Emails_Content);
                    proc.AddVarcharPara("@Emails_HasAttachement", 10, Emails_HasAttachement);
                    proc.AddIntegerPara("@Emails_CreateApplication", Emails_CreateApplication);
                    proc.AddIntegerPara("@Emails_CreateUser", Emails_CreateUser);
                    proc.AddVarcharPara("@Emails_Type", 10, Emails_Type);
                    proc.AddVarcharPara("@Emails_CompanyID", -1, Emails_CompanyID);
                    proc.AddVarcharPara("@Emails_Segment", 100, Emails_Segment);
                    result = Convert.ToInt64(proc.GetParaValue("result"));
                    proc.AddBigIntegerPara("@result", result, QueryParameterDirection.Output);
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

        public int select_exchinternalid(string exch_compId, string seg_id, out int exch_internalid)
        {
            ProcedureExecute proc;
            int rtrnvalue = 0;
            try
            {
                using (proc = new ProcedureExecute("select_exchinternalid"))
                {
                    proc.AddVarcharPara("@exch_compId", 100, exch_compId);
                    proc.AddVarcharPara("@seg_id", 100, seg_id);
                    exch_internalid = Convert.ToInt32(proc.GetParaValue("@Emails_Content"));
                    proc.AddIntegerPara("@exch_internalid", exch_internalid, QueryParameterDirection.Output);
                    int i = proc.RunActionQuery();
                    //return i;
                    exch_internalid =Convert.ToInt32(proc.GetParaValue("@exch_internalid"));
                    return exch_internalid;
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

        //public void select_exchinternalid(string exch_compId, string seg_id, out int exch_internalid)
        //{
        //    ProcedureExecute proc;
        //    string rtrnvalue = "";
        //    try
        //    {
        //        using (proc = new ProcedureExecute("select_exchinternalid"))
        //        {
        //            proc.AddVarcharPara("@exch_compId", 100, exch_compId);
        //            proc.AddVarcharPara("@seg_id", 100, seg_id);
        //            exch_internalid = Convert.ToInt32(proc.GetParaValue("@Emails_Content"));
        //            proc.AddIntegerPara("@exch_internalid", exch_internalid, QueryParameterDirection.Output);
        //            int i = proc.RunActionQuery();
        //            //return i;
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    finally
        //    {
        //        proc = null;
        //    }
        //}

        public DataSet SEARCH_CONTACT_BYPAN(string SEARCH_TYPE, string SEARCH_ENTITY, string EMAIL_LIKE)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("[SEARCH_CONTACT_BYPAN]");
            proc.AddVarcharPara("@SEARCH_TYPE", 50, SEARCH_TYPE);
            proc.AddVarcharPara("@SEARCH_ENTITY", 110, SEARCH_ENTITY);
            proc.AddVarcharPara("@EMAIL_LIKE", 150, EMAIL_LIKE);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet ADDRESSSEARCH(string SearchOnly, string Param, string ADD1, string ADD2, string ADD3, string Landmark, string Country, string State,
            string City, string Area, string Pin)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("[ADDRESSSEARCH]");
            proc.AddVarcharPara("@SearchOnly", 20, SearchOnly);
            proc.AddVarcharPara("@Param", -1, Param);
            proc.AddVarcharPara("@ADD1", -1, ADD1);
            proc.AddVarcharPara("@ADD2", -1, ADD2);
            proc.AddVarcharPara("@ADD3", -1, ADD3);
            proc.AddVarcharPara("@Landmark", -1, Landmark);
            proc.AddVarcharPara("@Country", -1, Country);
            proc.AddVarcharPara("@State", -1, State);
            proc.AddVarcharPara("@City", -1, City);
            proc.AddVarcharPara("@Area", -1, Area);
            proc.AddVarcharPara("@Pin", -1, Pin);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet Report_BRSNew(string ReportView, string CompanyId, string BanckAc, string ConsiderDate, string Date, string FinYear)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("[Report_BRSNew]");
            proc.AddVarcharPara("@ReportView", 50, ReportView);
            proc.AddVarcharPara("@CompanyId", -1, CompanyId);
            proc.AddVarcharPara("@BanckAc", -1, BanckAc);
            proc.AddVarcharPara("@ConsiderDate", 50, ConsiderDate);
            proc.AddVarcharPara("@Date", 110, Date);
            proc.AddVarcharPara("@FinYear", 150, FinYear);
            ds = proc.GetDataSet();
            return ds;
        }

        public int InsertTransSubAccount(string FinancialYear, string MainAccount_ReferenceID, string SubAccount_ReferenceID, string CompanyName, string SegmentName, string BranchName,
            decimal openingCR, decimal openingDR, string ActiveCurrency)
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            try
            {
                using (proc = new ProcedureExecute("InsertTransSubAccount"))
                {
                    proc.AddVarcharPara("@FinancialYear", 100, FinancialYear);
                    proc.AddVarcharPara("@MainAccount_ReferenceID", 100, MainAccount_ReferenceID);
                    proc.AddVarcharPara("@SubAccount_ReferenceID", 100, SubAccount_ReferenceID);
                    proc.AddVarcharPara("@CompanyName", 100, CompanyName);
                    proc.AddVarcharPara("@SegmentName", 100, SegmentName);
                    proc.AddVarcharPara("@BranchName", 100, BranchName);
                    // .............................Code Commented and Added by Sam on 12122016 to pass the proper parameter. .....................................  
                    //proc.AddDecimalPara("@openingCR", 28, 8, openingCR);
                    //proc.AddDecimalPara("@openingDR", 28, 8, openingDR);
                    proc.AddDecimalPara("@openingCR", 8, 28, openingCR);
                    proc.AddDecimalPara("@openingDR", 8, 28, openingDR);
                    // .............................Code Above Commented and Added by Sam on 12122016...................................... 
                    proc.AddVarcharPara("@ActiveCurrency", 100, ActiveCurrency);
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

        public int CreateSubAccount(int SubAccount_MainAcReferenceID, string SubAccount_Name, string SubAccount_Code, out string id)
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            try
            {
                using (proc = new ProcedureExecute("CreateSubAccount"))
                {
                    proc.AddIntegerPara("@SubAccount_MainAcReferenceID", SubAccount_MainAcReferenceID);
                    proc.AddVarcharPara("@SubAccount_Name", 100, SubAccount_Name);
                    proc.AddVarcharPara("@SubAccount_Code", 100, SubAccount_Code);
                    //proc.AddVarcharPara("@id", 100, id, QueryParameterDirection.Output);
                    proc.AddVarcharPara("@id", 100, rtrnvalue, QueryParameterDirection.Output);
                    int i = proc.RunActionQuery();
                    id = proc.GetParaValue("@id").ToString();
                    
                   
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

        public int UpDateAccountSummaryCust(string Module, string FinancialYear, string MainAccount_ReferenceID, string SubAccount_ReferenceID, string CompanyName, string SegmentName, string BranchName,
            decimal openingCR, decimal openingDR, string ActiveCurrency, string TradeCurrency)
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            try
            {
                using (proc = new ProcedureExecute("UpDateAccountSummaryCust"))
                {
                    proc.AddVarcharPara("@FinancialYear", 100, FinancialYear);
                    proc.AddVarcharPara("@Module", 100, Module);

                    proc.AddVarcharPara("@AccountRefID", 100, MainAccount_ReferenceID);
                    //proc.AddVarcharPara("@MainAccount_ReferenceID", 100, MainAccount_ReferenceID);
                    
                    //proc.AddVarcharPara("@SubAccount_ReferenceID", 100, SubAccount_ReferenceID);
                    proc.AddVarcharPara("@SubAccountRefID", 100, SubAccount_ReferenceID);
                    proc.AddVarcharPara("@CompanyName", 100, CompanyName);
                    proc.AddVarcharPara("@SegmentName", 100, SegmentName);
                    proc.AddVarcharPara("@BranchName", 100, BranchName);
                    //proc.AddDecimalPara("@openingCR", 28, 8, openingCR);
                    //proc.AddDecimalPara("@openingDR", 28, 8, openingDR);
                    proc.AddDecimalPara("@openingCR", 8, 28, openingCR);
                    proc.AddDecimalPara("@openingDR", 8, 28, openingDR);
                    proc.AddVarcharPara("@ActiveCurrency", 100, ActiveCurrency);
                    proc.AddVarcharPara("@TradeCurrency", 30, TradeCurrency);
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

        public void BrokerageMainInsertForSecondTime(out long ResultId, string BrokerageMain_CustomerID, string BrokerageMain_CompanyID, long BrokerageMain_SegmentID,
            string BrokerageMain_FromDate, int BrokerageMain_BrkgDecimals, int BrokerageMain_AverageType, int BrokerageMain_MktDecimals,
            int BrokerageMain_NetDecimals, int BrokerageMain_BrkgRoundPattern, int BrokerageMain_MktRoundPattern, int BrokerageMain_NetRoundPattern,
            int BrokerageMain_ContractPattern, decimal BrokerageMain_MinDelPerShare, decimal BrokerageMain_MinSqrPerShare, decimal BrokerageMain_MinDailyBrkg,
            decimal BrokerageMain_MinBrkgPerOrder, decimal BrokerageMain_MaxDelPerShare, decimal BrokerageMain_MaxSqrPerShare, decimal BrokerageMain_MaxDailyBrkg,
            decimal BrokerageMain_MaxBrkgPerOrder, int BrokerageMain_CreateUser, string BrokerageMain_CreateDateTime, int BrokerageMain_ModifyUser,
            string BrokerageMain_Type, string BrokerageMain_ID, string MinBrokeragePerDay, string MainShareBrokeragePerShare)
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            try
            {
                using (proc = new ProcedureExecute("BrokerageMainInsertForSecondTime"))
                {
                    ResultId = Convert.ToInt64(proc.GetParaValue("@ResultId"));
                    proc.AddBigIntegerPara("@ResultId", ResultId, QueryParameterDirection.Output);
                    proc.AddVarcharPara("@BrokerageMain_CustomerID", 100, BrokerageMain_CustomerID);
                    proc.AddVarcharPara("@BrokerageMain_CompanyID", 100, BrokerageMain_CompanyID);
                    proc.AddBigIntegerPara("@BrokerageMain_SegmentID", BrokerageMain_SegmentID);
                    proc.AddVarcharPara("@BrokerageMain_FromDate", 100, BrokerageMain_FromDate);
                    proc.AddIntegerPara("@BrokerageMain_BrkgDecimals", BrokerageMain_BrkgDecimals);
                    proc.AddIntegerPara("@BrokerageMain_AverageType", BrokerageMain_AverageType);
                    proc.AddIntegerPara("@BrokerageMain_MktDecimals", BrokerageMain_MktDecimals);
                    proc.AddIntegerPara("@BrokerageMain_NetDecimals", BrokerageMain_NetDecimals);
                    proc.AddIntegerPara("@BrokerageMain_BrkgRoundPattern", BrokerageMain_BrkgRoundPattern);
                    proc.AddIntegerPara("@BrokerageMain_MktRoundPattern", BrokerageMain_MktRoundPattern);
                    proc.AddIntegerPara("@BrokerageMain_NetRoundPattern", BrokerageMain_NetRoundPattern);
                    proc.AddIntegerPara("@BrokerageMain_ContractPattern", BrokerageMain_ContractPattern);
                    proc.AddDecimalPara("@BrokerageMain_MinDelPerShare", 18, 2, BrokerageMain_MinDelPerShare);
                    proc.AddDecimalPara("@BrokerageMain_MinSqrPerShare", 18, 2, BrokerageMain_MinSqrPerShare);
                    proc.AddDecimalPara("@BrokerageMain_MinDailyBrkg", 18, 2, BrokerageMain_MinDailyBrkg);
                    proc.AddDecimalPara("@BrokerageMain_MinBrkgPerOrder", 18, 2, BrokerageMain_MinBrkgPerOrder);
                    proc.AddDecimalPara("@BrokerageMain_MaxDelPerShare", 18, 2, BrokerageMain_MaxDelPerShare);
                    proc.AddDecimalPara("@BrokerageMain_MaxSqrPerShare", 18, 2, BrokerageMain_MaxSqrPerShare);
                    proc.AddDecimalPara("@BrokerageMain_MaxDailyBrkg", 18, 2, BrokerageMain_MaxDailyBrkg);
                    proc.AddDecimalPara("@BrokerageMain_MaxBrkgPerOrder", 18, 2, BrokerageMain_MaxBrkgPerOrder);
                    proc.AddIntegerPara("@BrokerageMain_CreateUser", BrokerageMain_CreateUser);
                    proc.AddVarcharPara("@BrokerageMain_CreateDateTime", 100, BrokerageMain_CreateDateTime);
                    proc.AddIntegerPara("@BrokerageMain_ModifyUser", BrokerageMain_ModifyUser);
                    proc.AddVarcharPara("@BrokerageMain_Type", 100, BrokerageMain_Type);
                    proc.AddVarcharPara("@BrokerageMain_ID", 100, BrokerageMain_ID);
                    proc.AddVarcharPara("@MinBrokeragePerDay", 100, MinBrokeragePerDay);
                    proc.AddVarcharPara("@MainShareBrokeragePerShare", 100, MainShareBrokeragePerShare);
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

        public void BrokerageMainInsert(out long ResultId, string MinBrokeragePerDay, string MainShareBrokeragePerShare, string BrokerageMain_CustomerID,
            string BrokerageMain_CompanyID, long BrokerageMain_SegmentID, string BrokerageMain_FromDate, int BrokerageMain_BrkgDecimals,
            int BrokerageMain_AverageType, int BrokerageMain_MktDecimals, int BrokerageMain_NetDecimals, int BrokerageMain_BrkgRoundPattern,
            int BrokerageMain_MktRoundPattern, int BrokerageMain_NetRoundPattern, int BrokerageMain_ContractPattern, decimal BrokerageMain_MinDelPerShare,
            decimal BrokerageMain_MinSqrPerShare, decimal BrokerageMain_MinDailyBrkg, decimal BrokerageMain_MinBrkgPerOrder,
            decimal BrokerageMain_MaxDelPerShare, decimal BrokerageMain_MaxSqrPerShare, decimal BrokerageMain_MaxDailyBrkg,
            decimal BrokerageMain_MaxBrkgPerOrder, int BrokerageMain_CreateUser, string BrokerageMain_CreateDateTime, int BrokerageMain_ModifyUser,
            string BrokerageMain_Type)
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            try
            {
                using (proc = new ProcedureExecute("BrokerageMainInsert"))
                {
                    ResultId = Convert.ToInt64(proc.GetParaValue("@ResultId"));
                    proc.AddBigIntegerPara("@ResultId", ResultId, QueryParameterDirection.Output);
                    proc.AddVarcharPara("@MinBrokeragePerDay", 100, MinBrokeragePerDay);
                    proc.AddVarcharPara("@MainShareBrokeragePerShare", 100, MainShareBrokeragePerShare);
                    proc.AddVarcharPara("@BrokerageMain_CustomerID", 100, BrokerageMain_CustomerID);
                    proc.AddVarcharPara("@BrokerageMain_CompanyID", 100, BrokerageMain_CompanyID);
                    proc.AddBigIntegerPara("@BrokerageMain_SegmentID", BrokerageMain_SegmentID);
                    proc.AddVarcharPara("@BrokerageMain_FromDate", 100, BrokerageMain_FromDate);
                    proc.AddIntegerPara("@BrokerageMain_BrkgDecimals", BrokerageMain_BrkgDecimals);
                    proc.AddIntegerPara("@BrokerageMain_AverageType", BrokerageMain_AverageType);
                    proc.AddIntegerPara("@BrokerageMain_MktDecimals", BrokerageMain_MktDecimals);
                    proc.AddIntegerPara("@BrokerageMain_NetDecimals", BrokerageMain_NetDecimals);
                    proc.AddIntegerPara("@BrokerageMain_BrkgRoundPattern", BrokerageMain_BrkgRoundPattern);
                    proc.AddIntegerPara("@BrokerageMain_MktRoundPattern", BrokerageMain_MktRoundPattern);
                    proc.AddIntegerPara("@BrokerageMain_NetRoundPattern", BrokerageMain_NetRoundPattern);
                    proc.AddIntegerPara("@BrokerageMain_ContractPattern", BrokerageMain_ContractPattern);
                    proc.AddDecimalPara("@BrokerageMain_MinDelPerShare", 18, 2, BrokerageMain_MinDelPerShare);
                    proc.AddDecimalPara("@BrokerageMain_MinSqrPerShare", 18, 2, BrokerageMain_MinSqrPerShare);
                    proc.AddDecimalPara("@BrokerageMain_MinDailyBrkg", 18, 2, BrokerageMain_MinDailyBrkg);
                    proc.AddDecimalPara("@BrokerageMain_MinBrkgPerOrder", 18, 2, BrokerageMain_MinBrkgPerOrder);
                    proc.AddDecimalPara("@BrokerageMain_MaxDelPerShare", 18, 2, BrokerageMain_MaxDelPerShare);
                    proc.AddDecimalPara("@BrokerageMain_MaxSqrPerShare", 18, 2, BrokerageMain_MaxSqrPerShare);
                    proc.AddDecimalPara("@BrokerageMain_MaxDailyBrkg", 18, 2, BrokerageMain_MaxDailyBrkg);
                    proc.AddDecimalPara("@BrokerageMain_MaxBrkgPerOrder", 18, 2, BrokerageMain_MaxBrkgPerOrder);
                    proc.AddIntegerPara("@BrokerageMain_CreateUser", BrokerageMain_CreateUser);
                    proc.AddVarcharPara("@BrokerageMain_CreateDateTime", 100, BrokerageMain_CreateDateTime);
                    proc.AddIntegerPara("@BrokerageMain_ModifyUser", BrokerageMain_ModifyUser);
                    proc.AddVarcharPara("@BrokerageMain_Type", 100, BrokerageMain_Type);
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

        public int UpDateAccountSummary(int AccountRefID, decimal openingDR, decimal openingCR)
        {
            int ret = 0;
            ProcedureExecute proc = new ProcedureExecute("UpDateAccountSummary");

            proc.AddIntegerPara("@MainAccount_ReferenceID", AccountRefID);
            proc.AddDecimalPara("@openingDR", 8, 28, openingDR);
            proc.AddDecimalPara("@openingCR", 8, 28, openingCR);
            ret = proc.RunActionQuery();
            return ret;
        }

    }
}
