using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public class DailyReports
    {
        public DataSet Reports_ComTrade_Register(string fromdate, string todate, string tradetype, string companyid, string clients, string Broker,
            string instruments, string segment, string grptype, string grp, string TERMINALID, string BRANCHHierarchy, string CTCLID, string rpttype,
            string exportType, string groupByFilter, string OrderBy, string Parameter)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Reports_ComTrade_Register");

            proc.AddVarcharPara("@fromdate", 30, fromdate);
            proc.AddVarcharPara("@todate", 50, todate);
            proc.AddVarcharPara("@tradetype", 50, tradetype);
            proc.AddVarcharPara("@companyid", 50, companyid);
            proc.AddVarcharPara("@clients", -1, clients);
            proc.AddVarcharPara("@Broker", 5, Broker);
            proc.AddVarcharPara("@instruments", -1, instruments);
            proc.AddVarcharPara("@segment", -1, segment);
            proc.AddVarcharPara("@grptype", 50, grptype);
            proc.AddVarcharPara("@grp", -1, grp);
            proc.AddVarcharPara("@TERMINALID", 50, TERMINALID);
            proc.AddVarcharPara("@BRANCHHierarchy", -1, BRANCHHierarchy);
            proc.AddVarcharPara("@CTCLID", 50, CTCLID);
            proc.AddVarcharPara("@rpttype", 10, rpttype);
            proc.AddVarcharPara("@exportType", 10, exportType);
            proc.AddVarcharPara("@groupByFilter", 20, groupByFilter);
            proc.AddVarcharPara("@OrderBy", 5, OrderBy);
            proc.AddVarcharPara("@Parameter", -1, Parameter);
            ds = proc.GetDataSet();
            return ds;
        }


        public DataSet Sp_ExchangeObligationCOMM(string date, string segment, int MASTERSEGMENT, string Companyid, string Branch)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Sp_ExchangeObligationCOMM");

            proc.AddVarcharPara("@date", 50, date);
            proc.AddVarcharPara("@segment", 50, segment);
            proc.AddIntegerPara("@MASTERSEGMENT", MASTERSEGMENT);
            proc.AddVarcharPara("@Companyid", 30, Companyid);
            proc.AddVarcharPara("@Branch", 10, Branch);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet PerformanceReportCommCurrency(string companyid, string segment, string fromdate, string todate, string clients,
            string Seriesid, string Expiry, string grptype, string GrpId, string BranchHierchy,
            string mtmcalbasis, string rpttype, string instrutype, string PRINTCHK,
            string CHKDISTRIBUTION, string ignorebfqty, string chkopen, string chkopenbfpositive, string chkclosepricezero,
            string chknetclients, string chkterminal, string valuebfposition, string ChkPremium, string rptview, string AcrossExchange,
            string ConsolidateExchSegment, string CalculateCharges, string CalculateInterest)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("PerformanceReportCommCurrency");

            proc.AddVarcharPara("@companyid", 15, companyid);
            proc.AddVarcharPara("@segment", 50, segment);
            proc.AddVarcharPara("@fromdate", 35, fromdate);
            proc.AddVarcharPara("@todate", 35, todate);
            proc.AddVarcharPara("@clients", -1, clients);
            proc.AddVarcharPara("@Seriesid", -1, Seriesid);
            proc.AddVarcharPara("@Expiry", -1, Expiry);
            proc.AddVarcharPara("@grptype", 30, grptype);
            proc.AddVarcharPara("@GrpId", -1, GrpId);
            proc.AddVarcharPara("@BranchHierchy", -1, BranchHierchy);
            proc.AddVarcharPara("@mtmcalbasis", 10, mtmcalbasis);
            proc.AddVarcharPara("@rpttype", 10, rpttype);
            proc.AddVarcharPara("@instrutype", 300, instrutype);
            proc.AddVarcharPara("@PRINTCHK", 200, PRINTCHK);
            proc.AddVarcharPara("@CHKDISTRIBUTION", 200, CHKDISTRIBUTION);
            proc.AddVarcharPara("@ignorebfqty", 10, ignorebfqty);
            proc.AddVarcharPara("@chkopen", 10, chkopen);
            proc.AddVarcharPara("@chkopenbfpositive", 10, chkopenbfpositive);
            proc.AddVarcharPara("@chkclosepricezero", 10, chkclosepricezero);
            proc.AddVarcharPara("@chknetclients", 10, chknetclients);
            proc.AddVarcharPara("@chkterminal", 10, chkterminal);
            proc.AddVarcharPara("@valuebfposition", 10, valuebfposition);
            proc.AddVarcharPara("@ChkPremium", 10, ChkPremium);
            proc.AddVarcharPara("@rptview", 10, rptview);
            proc.AddVarcharPara("@AcrossExchange", 10, AcrossExchange);
            proc.AddVarcharPara("@ConsolidateExchSegment", 20, ConsolidateExchSegment);
            proc.AddCharPara("@CalculateCharges", 1, Convert.ToChar(CalculateCharges));
            proc.AddCharPara("@CalculateInterest", 1, Convert.ToChar(CalculateInterest));
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet Sp_ContractRegisterComm(string fromdate, string todate, string clients_internalId, string branch_id, string segment, string Companyid)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Sp_ContractRegisterComm");

            proc.AddVarcharPara("@fromdate", 50, fromdate);
            proc.AddVarcharPara("@todate", 50, todate);
            proc.AddVarcharPara("@clients_internalId", -1, clients_internalId);
            proc.AddVarcharPara("@branch_id", -1, branch_id);
            proc.AddVarcharPara("@segment", 50, segment);
            proc.AddVarcharPara("@Companyid", 10, Companyid);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet Report_ComDailyBills(string segment, string Companyid, string date, string Finyear, string grpbranch, string ClientsID,
            string branch, string CalType, string MasterSegment)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Report_ComDailyBills");

            proc.AddVarcharPara("@segment", 20, segment);
            proc.AddVarcharPara("@Companyid", 30, Companyid);
            proc.AddVarcharPara("@date", 30, date);
            proc.AddVarcharPara("@Finyear", 15, Finyear);
            proc.AddVarcharPara("@grpbranch", 10, grpbranch);
            proc.AddVarcharPara("@ClientsID", -1, ClientsID);
            proc.AddVarcharPara("@branch", -1, branch);
            proc.AddVarcharPara("@CalType", 10, CalType);
            proc.AddVarcharPara("@MasterSegment", 10, MasterSegment);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet NetPositionCommCurrency(string fromdate, string todate, string segment, string MasterSegment, string Companyid, string NetOrMarketVal,
           string Broker, string ClientsID, string Instrument, string UNDERLYING, string Expiry, string BranchHierchy, string GRPTYPE,
            string GRPID, string OPENFUT, string ChkCharge, string Chksign, string rptview, string ExposureBuyCall, string ExposureBuyPut,
            string ParameterFeild)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("NetPositionCommCurrency");

            proc.AddVarcharPara("@fromdate", 35, fromdate);
            proc.AddVarcharPara("@todate", 35, todate);
            proc.AddVarcharPara("@segment", 10, segment);
            proc.AddVarcharPara("@MasterSegment", 5, MasterSegment);
            proc.AddVarcharPara("@Companyid", 30, Companyid);
            proc.AddVarcharPara("@NetOrMarketVal", 10, NetOrMarketVal);
            proc.AddVarcharPara("@Broker", 5, Broker);
            proc.AddVarcharPara("@ClientsID", -1, ClientsID);
            proc.AddVarcharPara("@Instrument", -1, Instrument);
            proc.AddVarcharPara("@UNDERLYING", -1, UNDERLYING);
            proc.AddVarcharPara("@Expiry", -1, Expiry);
            proc.AddVarcharPara("@BranchHierchy", -1, BranchHierchy);
            proc.AddVarcharPara("@GRPTYPE", 20, GRPTYPE);
            proc.AddVarcharPara("@GRPID", -1, GRPID);
            proc.AddVarcharPara("@OPENFUT", 10, OPENFUT);
            proc.AddVarcharPara("@ChkCharge", 10, ChkCharge);
            proc.AddVarcharPara("@Chksign", 10, Chksign);
            proc.AddVarcharPara("@rptview", 10, rptview);
            proc.AddVarcharPara("@ExposureBuyCall", 10, ExposureBuyCall);
            proc.AddVarcharPara("@ExposureBuyPut", 10, ExposureBuyPut);
            proc.AddVarcharPara("@ParameterFeild", -1, ParameterFeild);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet cdsl_EmployeeName(string ID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("cdsl_EmployeeName");

            proc.AddVarcharPara("@ID", 50, ID);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet DailyMarginReportToClient(string date, string clients, string segment, string grptype, string companyid, string Finyear,
            string chkappmargin, string CHKDETAILSEQURITY, string CHKPRINT, string AppMrgnOrVarMrgn)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("DailyMarginReportToClient");

            proc.AddVarcharPara("@date", 30, date);

            proc.AddVarcharPara("@clients", -1, clients);
            proc.AddVarcharPara("@segment", -1, segment);
            proc.AddVarcharPara("@grptype", 20, grptype);
            proc.AddVarcharPara("@companyid", 20, companyid);
            proc.AddVarcharPara("@Finyear", 50, Finyear);
            proc.AddVarcharPara("@chkappmargin", 10, chkappmargin);
            proc.AddVarcharPara("@CHKDETAILSEQURITY", 10, CHKDETAILSEQURITY);
            proc.AddVarcharPara("@CHKPRINT", 10, CHKPRINT);
            proc.AddVarcharPara("@AppMrgnOrVarMrgn", 10, AppMrgnOrVarMrgn);
            ds = proc.GetDataSet();
            return ds;
        }


        public DataSet TradeRegister(string Companyid, string FromDate, string ToDate, string TradeType, string RptType, string BranchHierchy,
            string GrpType, string GrpId, string Clients, string Broker, string groupbyvalue, string TradeCategory, string Terminalid, string CtClid,
            string Scrip, string Segment, string Settno, string SettType, string TradeTime, string Parameter, string OrderBy, string SecurityType, string Header, string Footer)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("TradeRegister");

            proc.AddVarcharPara("@Companyid", 15, Companyid);
            proc.AddVarcharPara("@FromDate", 35, FromDate);
            proc.AddVarcharPara("@ToDate", 35, ToDate);
            proc.AddVarcharPara("@TradeType", 5, TradeType);
            proc.AddVarcharPara("@RptType", 50, RptType);
            proc.AddVarcharPara("@BranchHierchy", -1, BranchHierchy);
            proc.AddVarcharPara("@GrpType", 20, GrpType);
            proc.AddVarcharPara("@GrpId", -1, GrpId);
            proc.AddVarcharPara("@Clients", -1, Clients);
            proc.AddVarcharPara("@Broker", 5, Broker);
            proc.AddVarcharPara("@groupbyvalue", 20, groupbyvalue);
            proc.AddVarcharPara("@TradeCategory", 20, TradeCategory);
            proc.AddVarcharPara("@Terminalid", 20, Terminalid);
            proc.AddVarcharPara("@CtClid", 20, CtClid);
            proc.AddVarcharPara("@Scrip", -1, Scrip);
            proc.AddVarcharPara("@Segment", 30, Segment);
            proc.AddVarcharPara("@Settno", 150, Settno);
            proc.AddVarcharPara("@SettType", 150, SettType);
            proc.AddVarcharPara("@TradeTime", 500, TradeTime);
            proc.AddVarcharPara("@Parameter", -1, Parameter);
            proc.AddVarcharPara("@OrderBy", 10, OrderBy);
            proc.AddVarcharPara("@SecurityType", 15, SecurityType);
            proc.AddVarcharPara("@Header", -1, Header);
            proc.AddVarcharPara("@Footer", -1, Footer);

            ds = proc.GetDataSet();
            return ds;
        }

        public string exchange_obligationtotal(int trades_segment, string trades_settlementno, string trades_obligationtype, string Companyid)
        {
            string rtrnvalue = "";
            ProcedureExecute proc;
            try
            {
                using (proc = new ProcedureExecute("exchange_obligationtotal"))
                {
                    proc.AddIntegerPara("@trades_segment", trades_segment);
                    proc.AddVarcharPara("@trades_settlementno", 10, trades_settlementno);
                    proc.AddVarcharPara("@trades_obligationtype", 50, trades_obligationtype);
                    proc.AddVarcharPara("@Companyid", 50, Companyid);
                    proc.AddVarcharPara("@total_obligance", 5000, "", QueryParameterDirection.Output);
                    int i = proc.RunActionQuery();
                    rtrnvalue = proc.GetParaValue("@total_obligance").ToString();
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

        public DataTable exchange_obligation(int trades_segment, string trades_settlementno, string trades_obligationtype, string Companyid)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("exchange_obligation");

            proc.AddIntegerPara("@trades_segment", trades_segment);
            proc.AddVarcharPara("@trades_settlementno", 10, trades_settlementno);
            proc.AddVarcharPara("@trades_obligationtype", 50, trades_obligationtype);
            proc.AddVarcharPara("@Companyid", 50, Companyid);
            ds = proc.GetDataSet();
            return ds.Tables[0];

        }

        public DataTable exchange_dateobligation(int trades_segment, string trades_datefrom, string trades_dateto,
             string trades_obligationtype, string MasterSegment, string Companyid)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("exchange_dateobligation");

            proc.AddIntegerPara("@trades_segment", trades_segment);
            proc.AddVarcharPara("@trades_datefrom", 50, trades_datefrom);
            proc.AddVarcharPara("@trades_dateto", 50, trades_dateto);
            proc.AddVarcharPara("@trades_obligationtype", 50, trades_obligationtype);
            proc.AddVarcharPara("@MasterSegment", 50, MasterSegment);
            proc.AddVarcharPara("@Companyid", 50, Companyid);
            ds = proc.GetDataSet();
            return ds.Tables[0];

        }

        public string exchange_dateobligationtotal(int trades_segment, string trades_datefrom, string trades_dateto,
           string trades_obligationtype, string MasterSegment, string Companyid)
        {
            string rtrnvalue = "";
            ProcedureExecute proc = new ProcedureExecute("exchange_dateobligationtotal");

            proc.AddIntegerPara("@trades_segment", trades_segment);
            proc.AddVarcharPara("@trades_datefrom", 50, trades_datefrom);
            proc.AddVarcharPara("@trades_dateto", 50, trades_dateto);
            proc.AddVarcharPara("@trades_obligationtype", 50, trades_obligationtype);
            proc.AddVarcharPara("@MasterSegment", 50, MasterSegment);
            proc.AddVarcharPara("@Companyid", 50, Companyid);
            proc.AddVarcharPara("@net_obligance", 5000, "", QueryParameterDirection.Output);
            int i = proc.RunActionQuery();
            rtrnvalue = proc.GetParaValue("@net_obligance").ToString();
            return rtrnvalue;

        }


        public DataSet NetPositionCM(string fromdate, string todate, string SettNo, string segment, string MasterSegment, string Companyid,
           string Broker, string ClientsID, string instrument, string settype, string Branch, string GRPTYPE, string GRPID, string openposition,
           string ChkCharge, string Chksign, string rptview, string AmntGreaterThan, string SecurityType)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("NetPositionCM");

            proc.AddVarcharPara("@fromdate", 50, fromdate);
            proc.AddVarcharPara("@todate", 50, todate);
            proc.AddVarcharPara("@SettNo", 50, SettNo);
            proc.AddVarcharPara("@segment", 50, segment);
            proc.AddVarcharPara("@MasterSegment", 50, MasterSegment);
            proc.AddVarcharPara("@Companyid", 50, Companyid);
            proc.AddVarcharPara("@Broker", 5, Broker);
            proc.AddVarcharPara("@ClientsID", -1, ClientsID);
            proc.AddVarcharPara("@instrument", -1, instrument);
            proc.AddVarcharPara("@settype", -1, settype);
            proc.AddVarcharPara("@Branch", -1, Branch);
            proc.AddVarcharPara("@GRPTYPE", 50, GRPTYPE);
            proc.AddVarcharPara("@GRPID", -1, GRPID);
            proc.AddVarcharPara("@openposition", 50, openposition);
            proc.AddVarcharPara("@ChkCharge", 50, ChkCharge);
            proc.AddVarcharPara("@Chksign", 50, Chksign);
            proc.AddVarcharPara("@rptview", 50, rptview);
            proc.AddDecimalPara("@AmntGreaterThan",  2,28, Convert.ToDecimal(AmntGreaterThan));
            proc.AddVarcharPara("@SecurityType", 15, SecurityType);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet Report_SettlementTrialNSECM(string date, string segment, string Companyid, string SettNo,
            string SetType, string MasterSegment, string rptview, string GrpCodeName)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Report_SettlementTrialNSECM");

            proc.AddVarcharPara("@date", 50, date);
            proc.AddVarcharPara("@segment", 20, segment);
            proc.AddVarcharPara("@Companyid", 30, Companyid);
            proc.AddVarcharPara("@SettNo", 50, SettNo);
            proc.AddVarcharPara("@SetType", -1, SetType);
            proc.AddVarcharPara("@MasterSegment", 10, MasterSegment);
            proc.AddVarcharPara("@rptview", 5, rptview);
            proc.AddVarcharPara("@GrpCodeName", 50, GrpCodeName);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet Sp_SettlementTrialNSECM1(string segment, string Companyid, string date, string SettNo,
           string SetType, int MasterSegment, string Type, string payout,
            string segmentname, string CLIENTS, string grptype, string groupby)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Sp_SettlementTrialNSECM1");

            proc.AddVarcharPara("@segment", 20, segment);
            proc.AddVarcharPara("@Companyid", 30, Companyid);
            proc.AddVarcharPara("@date", 50, date);
            proc.AddVarcharPara("@SettNo", 50, SettNo);
            proc.AddVarcharPara("@SetType", -1, SetType);
            proc.AddIntegerPara("@MasterSegment", MasterSegment);
            proc.AddVarcharPara("@Type", 20, Type);
            proc.AddVarcharPara("@payout", 50, payout);
            proc.AddVarcharPara("@segmentname", 20, segmentname);
            proc.AddVarcharPara("@CLIENTS", -1, CLIENTS);
            proc.AddVarcharPara("@grptype", 30, grptype);
            proc.AddVarcharPara("@groupby", 30, groupby);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet Sp_ExchangeObligationFO(string date, string segment, string MasterSegment, string Companyid,
         string underlying, string expiry, string Branch)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Sp_ExchangeObligationFO");

            proc.AddVarcharPara("@date", 50, date);
            proc.AddVarcharPara("@segment", 50, segment);
            proc.AddVarcharPara("@MasterSegment", 50, MasterSegment);
            proc.AddVarcharPara("@Companyid", 30, Companyid);
            proc.AddVarcharPara("@underlying", -1, underlying);
            proc.AddVarcharPara("@expiry", 50, expiry);
            proc.AddVarcharPara("@Branch", 10, Branch);

            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet SP_MarginReportNSEFO(string Module, string FromDate, string ToDate, string ClientsID,
       string grptype, string ExcSegmt, string CalType)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("SP_MarginReportNSEFO");

            proc.AddVarcharPara("@Module", 100, Module);
            proc.AddVarcharPara("@FromDate", 100, FromDate);
            proc.AddVarcharPara("@ToDate", 100, ToDate);
            proc.AddVarcharPara("@ClientsID", -1, ClientsID);
            proc.AddVarcharPara("@grptype", 100, grptype);
            proc.AddVarcharPara("@ExcSegmt", 100, ExcSegmt);
            proc.AddVarcharPara("@CalType", 10, CalType);

            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet PerformanceReportFO(string companyid, string segment, string fromdate, string todate, string clients, string MasterSegment,
            string Seriesid, string Expiry, string grptype, string mtmcalbasis, string rpttype, string instrutype, string PRINTCHK, string CHKDISTRIBUTION,
            string ignorebfqty, string chkopen, string chkopenbfpositive, string chkclosepricezero, string chknetclients, string chkterminal,
            string valuebfposition, string clubspot, string ChkPremium, string rptview)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("PerformanceReportFO");

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
            proc.AddVarcharPara("@chkclosepricezero", 10, chkclosepricezero);
            proc.AddVarcharPara("@chknetclients", 10, chknetclients);
            proc.AddVarcharPara("@chkterminal", 10, chkterminal);
            proc.AddVarcharPara("@valuebfposition", 10, valuebfposition);
            proc.AddVarcharPara("@clubspot", 10, clubspot);
            proc.AddVarcharPara("@ChkPremium", 10, ChkPremium);
            proc.AddVarcharPara("@rptview", 10, rptview);
            ds = proc.GetDataSet();
            return ds;
        }


        public DataSet Report_DailyBillsFO(string segment, string Companyid, string date, string Finyear, string Branch, string grpbranch, string GRPID,
            string Broker, string ClientsID, string CalType, string MasterSegment, string AppMargin, string AppMarginconsolidate)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Report_DailyBillsFO");

            proc.AddVarcharPara("@segment", 20, segment);
            proc.AddVarcharPara("@Companyid", 30, Companyid);
            proc.AddVarcharPara("@date", 100, date);
            proc.AddVarcharPara("@Finyear", 15, Finyear);
            proc.AddVarcharPara("@Branch", -1, Branch);
            proc.AddVarcharPara("@grpbranch", 10, grpbranch);
            proc.AddVarcharPara("@GRPID", -1, GRPID);
            proc.AddVarcharPara("@Broker", 5, Broker);
            proc.AddVarcharPara("@ClientsID", -1, ClientsID);
            proc.AddVarcharPara("@CalType", 10, CalType);
            proc.AddVarcharPara("@MasterSegment", 10, MasterSegment);
            proc.AddVarcharPara("@AppMargin", 10, AppMargin);
            proc.AddVarcharPara("@AppMarginconsolidate", 10, AppMarginconsolidate);

            ds = proc.GetDataSet();
            return ds;
        }


        public DataSet NetPositionFO(string fromdate, string todate, string segment, string Finyear, string MasterSegment, string Companyid,
          string NetOrMarketVal, string Broker, string ClientsID, string UNDERLYING, string Expiry, string Branch, string GRPTYPE, string GRPID,
            string OPENFUT, string OPENOPT, string ChkCharge, string Chksign, string rptview, string InstryType, string ExposureBuyCall,
            string ExposureBuyPut, string ExposurePrice)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("NetPositionFO");

            proc.AddVarcharPara("@fromdate", 30, fromdate);
            proc.AddVarcharPara("@todate", 30, todate);
            proc.AddVarcharPara("@segment", 10, segment);
            proc.AddVarcharPara("@Finyear", 150, Finyear);
            proc.AddVarcharPara("@MasterSegment", 50, MasterSegment);
            proc.AddVarcharPara("@Companyid", 30, Companyid);
            proc.AddVarcharPara("@NetOrMarketVal", 10, NetOrMarketVal);
            proc.AddVarcharPara("@Broker", 5, Broker);
            proc.AddVarcharPara("@ClientsID", -1, ClientsID);
            proc.AddVarcharPara("@UNDERLYING", -1, UNDERLYING);
            proc.AddVarcharPara("@Expiry", 30, Expiry);
            proc.AddVarcharPara("@Branch", -1, Branch);
            proc.AddVarcharPara("@GRPTYPE", 20, GRPTYPE);
            proc.AddVarcharPara("@GRPID", -1, GRPID);
            proc.AddVarcharPara("@OPENFUT", 10, OPENFUT);
            proc.AddVarcharPara("@OPENOPT", 10, OPENOPT);
            proc.AddVarcharPara("@ChkCharge", 10, ChkCharge);
            proc.AddVarcharPara("@Chksign", 10, Chksign);
            proc.AddVarcharPara("@rptview", 10, rptview);
            proc.AddVarcharPara("@InstryType", 2, InstryType);
            proc.AddVarcharPara("@ExposureBuyCall", 10, ExposureBuyCall);
            proc.AddVarcharPara("@ExposureBuyPut", 10, ExposureBuyPut);
            proc.AddVarcharPara("@ExposurePrice", 5, ExposurePrice);
            ds = proc.GetDataSet();
            return ds;
        }


        #region Statement of holding

        public int MailSend_ForISIN(string LastFinYear, string LastCompany, string usersegid, string txtDate,
           string RecipientContactID, string DocumentName, string DocumentPath, string userid,
            string RecipientEmailID, int EmailCreateAppMenuId)
        {

            int i;
            ProcedureExecute proc = new ProcedureExecute("Insert_UnSignedAttachDocuments");

            proc.AddVarcharPara("@FinancialYear", 50, LastFinYear);
            proc.AddVarcharPara("@CompanyID", 50, LastCompany);
            proc.AddVarcharPara("@Segment_OR_DPID", 50, usersegid);
            proc.AddVarcharPara("@Segment_Name", 50, "NSDL");
            proc.AddVarcharPara("@ContractDate", 50, Convert.ToDateTime(txtDate).ToString("dd-MMM-yyyy"));
            proc.AddVarcharPara("@BranchID", 50, "1");//==for digital Signed
            proc.AddVarcharPara("@ContactID_OR_BenAccNumber", -1, RecipientContactID);
            proc.AddVarcharPara("@DocumentType", 50, "21");
            proc.AddVarcharPara("@DocumentName", -1, DocumentName);
            proc.AddVarcharPara("@DocumentPath", 500, DocumentPath + "/" + DocumentName);
            proc.AddVarcharPara("@user", 50, userid);
            proc.AddVarcharPara("@RecipientEmailID", 200, RecipientEmailID);
            proc.AddIntegerPara("@EmailCreateAppMenuId", EmailCreateAppMenuId);
            i = proc.RunActionQuery();

            return i;


        }


        #endregion

        #region Statement of Transaction

        public DataTable ShowNsdlTransactionHeaderList(string stdate, string endDate, string cmpid,
           string isinid, string SettlementID, string CmbClientType, string userid, string userbranchHierarchy)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("sp_ShowNsdlTransactionHeaderList");

            proc.AddVarcharPara("@stdate", 30, stdate);
            proc.AddVarcharPara("@eddate", 30, endDate);
            if (cmpid != "")
            {
                proc.AddVarcharPara("@benAccNo", -1, cmpid);
            }

            if (isinid != "")
            {
                proc.AddVarcharPara("@isin", -1, isinid);
            }
            else
            {
                proc.AddVarcharPara("@isin", -1, "na");
            }

            if (SettlementID != "")
            {
                proc.AddVarcharPara("@settlement_id", 30, SettlementID);
            }
            else
            {
                proc.AddVarcharPara("@settlement_id", 30, "na");
            }
            if (CmbClientType == "All")
            {
                proc.AddVarcharPara("@bentype", 30, "na");
            }
            else
            {
                proc.AddVarcharPara("@bentype", 30, CmbClientType);
            }
            proc.AddVarcharPara("@userid", 20, userid);
            proc.AddVarcharPara("@branchid", 2000, userbranchHierarchy);


            ds = proc.GetDataSet();
            return ds.Tables[0];
        }
        #endregion

        #region NsdlTransaction_FetchTypeSubtype_totaltrans

        public DataTable NsdlTransaction_FetchTypeSubtype_totaltrans(string stdate, string endDate, string BenAccNoClientId, int BenType)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("sp_NsdlTransaction_FetchTypeSubtype_totaltrans");

            proc.AddVarcharPara("@stdate", 30, stdate);
            proc.AddVarcharPara("@eddate", 30, endDate);

            proc.AddVarcharPara("@BenAccNo", 30, BenAccNoClientId);

            proc.AddIntegerPara("@BenType", BenType);



            ds = proc.GetDataSet();
            return ds.Tables[0];
        }

        #endregion

        #region ShowNsdlTransaction

        public DataTable ShowNsdlTransaction(string stdate, string endDate, string BenAccNoClientId, int BenType, string isinid, string SettlementID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("sp_ShowNsdlTransaction");

            proc.AddVarcharPara("@stdate", 30, stdate);
            proc.AddVarcharPara("@eddate", 30, endDate);

            proc.AddVarcharPara("@BenAccNo", 30, BenAccNoClientId);

            proc.AddIntegerPara("@BenType", BenType);

            if (isinid != "")
            {
                proc.AddVarcharPara("@isin", 30, isinid);
            }
            else
            {
                proc.AddVarcharPara("@isin", 30, "na");
            }
            if (SettlementID != "")
            {
                proc.AddVarcharPara("@settlement_id", 50, SettlementID);
            }
            else
            {
                proc.AddVarcharPara("@settlement_id", 50, "na");
            }

            ds = proc.GetDataSet();
            return ds.Tables[0];
        }

        #endregion

        #region ShowNsdlTransaction

        public DataTable Nsdl_FetchTransaction(string userid, int startIndex, int endIndex)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("sp_Nsdl_FetchTransaction");

            proc.AddVarcharPara("@userid", 10, userid);
            proc.AddIntegerPara("@startRowIndex", startIndex);
            proc.AddIntegerPara("@endIndex", endIndex);

            ds = proc.GetDataSet();
            return ds.Tables[0];
        }

        #endregion

        #region ShowNsdlTransaction

        public DataSet ShowNsdlTransactionReport(string stdate, string endDate, string companyId, string dp,
            string cmpid, string isinid, string SettlementID, string CmbClientType, string branchid)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("sp_ShowNsdlTransactionReport");
            //proc.AddVarcharPara("@userid", 10, userid);
            //proc.AddIntegerPara("@startRowIndex", startIndex);
            //proc.AddIntegerPara("@endIndex", endIndex);

            proc.AddVarcharPara("@stdate", 30, stdate);
            proc.AddVarcharPara("@eddate", 30, endDate);
            proc.AddVarcharPara("@compID", 30, companyId);
            proc.AddVarcharPara("@dp", 30, dp);

            if (cmpid != "")
            {
                proc.AddVarcharPara("@benAccNo", -1, cmpid);
            }
            else
            {
                proc.AddVarcharPara("@benAccNo", -1, "na");
            }
            if (isinid != "")
            {
                proc.AddVarcharPara("@isin", 30, isinid);
            }
            else
            {
                proc.AddVarcharPara("@isin", 30, "na");
            }
            if (SettlementID != "")
            {
                proc.AddVarcharPara("@settlement_id", 30, SettlementID);
            }
            else
            {
                proc.AddVarcharPara("@settlement_id", 30, "na");
            }
            if (CmbClientType == "All")
            {
                proc.AddVarcharPara("@bentype", 30, "na");
            }
            else
            {
                proc.AddVarcharPara("@bentype", 30, CmbClientType);
            }

            proc.AddNVarcharPara("@branchid", -1, branchid);

            ds = proc.GetDataSet();
            return ds;
        }

        #endregion

        #region ShowNsdlTransaction
        public DataSet ShowNsdlTransactionHoldingReport(string stdate, string endDate, string cmpid)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("sp_ShowNsdlTransactionHoldingReport");
            proc.AddVarcharPara("@stdate", 30, stdate);
            proc.AddVarcharPara("@eddate", 30, endDate);
            if (cmpid != "")
            {
                proc.AddVarcharPara("@benAccNo", -1, cmpid);
            }
            else
            {
                proc.AddVarcharPara("@benAccNo", -1, "na");
            }


            ds = proc.GetDataSet();
            return ds;
        }
        #endregion

        #region ShowNsdlTransaction
        public DataSet Fetch_DPNetworthStatement(int NoOfRows, bool rbSpecific, string hidClients, string SelDate, bool chkConsiderAccounts,
            string txtNetworthPercentage, string Segment)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Fetch_DPNetworthStatement");

            proc.AddIntegerPara("@NoOfRow", NoOfRows);

            if (rbSpecific == true)
            {
                if (hidClients != "")
                    proc.AddVarcharPara("@AccountTypes", 2000, hidClients);
            }

            proc.AddVarcharPara("@SelDate", 10, SelDate);

            if (chkConsiderAccounts == true)
            {
                proc.AddCharPara("@ConsiderPercent", 1, 'Y');

                string PercentValue;
                if (txtNetworthPercentage != "")
                    PercentValue = txtNetworthPercentage;
                else
                    PercentValue = "5";
                proc.AddIntegerPara("@PercentValue", Convert.ToInt32(PercentValue));

            }

            proc.AddVarcharPara("@Segment", 10, Segment);


            ds = proc.GetDataSet();
            return ds;
        }
        #endregion

        #region SRVTAXSTATEMENT
        public DataSet Sp_SRVTAXSTATEMENT_DP(string FromDate, string ToDate, string DPid, string Companyid,
            string REPORTTYPE, string userbranchHierarchy)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Sp_SRVTAXSTATEMENT_DP");


            proc.AddVarcharPara("@FromDate", 50, FromDate);
            proc.AddVarcharPara("@ToDate", 50, ToDate);
            proc.AddVarcharPara("@DPid", -1, DPid);
            proc.AddVarcharPara("@Companyid", 100, Companyid);
            proc.AddVarcharPara("@REPORTTYPE", 10, REPORTTYPE);
            proc.AddVarcharPara("@BRANCH", -1, userbranchHierarchy);

            ds = proc.GetDataSet();
            return ds;
        }
        #endregion

        public DataSet ExchangeObligationCDX(string date, string segment, string MasterSegment, string Companyid)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("ExchangeObligationCDX");

            proc.AddVarcharPara("@date", 50, date);
            proc.AddVarcharPara("@segment", 50, segment);
            proc.AddVarcharPara("@MasterSegment", 50, MasterSegment);
            proc.AddVarcharPara("@Companyid", 30, Companyid);
            ds = proc.GetDataSet();
            return ds;
        }

    }
}
