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
        public DataTable CdslWorkingDaysInMonth(string currentDate, string dpId, string dp) 
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Cdsl_WorkingDays_in_Month");
            proc.AddVarcharPara("@currentDate", 100, currentDate);
            proc.AddNVarcharPara("@dpId", 100, dpId);
            proc.AddVarcharPara("@dp", 100, dp);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable NsdlBill(string startDate, string endDate, string BillDate, string companyId, string DpId, string user, string DPChargeMembers_SegmentID, string param, string DueDate)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("sp_NsdlBill");
            proc.AddVarcharPara("@startDate", 100, startDate);
            proc.AddNVarcharPara("@endDate", 100, endDate);
            proc.AddVarcharPara("@BillDate", 100, BillDate); 
            proc.AddVarcharPara("@companyId", 100, companyId);
            proc.AddNVarcharPara("@DpId", 100, DpId);
            proc.AddVarcharPara("@user", 100, user); 
            proc.AddVarcharPara("@DPChargeMembers_SegmentID", 100, DPChargeMembers_SegmentID);
            proc.AddNVarcharPara("@param", 100, param);
            proc.AddVarcharPara("@DueDate", 100, DueDate);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable NsdlBillDelete(string startdate, string enddate, string dp, string financialYear)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("sp_NsdlBill_Delete");
            proc.AddVarcharPara("@startdate", 100, startdate);
            proc.AddNVarcharPara("@enddate", 100, enddate);
            proc.AddVarcharPara("@dp", 100, dp);
            proc.AddVarcharPara("@financialYear", 100, financialYear);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable CdslDpPriceWorkingDaysInMonth(string currentDate, string dp)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Cdsl_DpPrice_WorkingDays_in_Month");
            proc.AddVarcharPara("@currentDate", 100, currentDate);
            proc.AddNVarcharPara("@dp", 100, dp); 
            dt = proc.GetTable();
            return dt;
        }

        public DataSet FetchSlipsinwardsGenerated(string FromDate, string ToDate)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Fetch_SlipsinwardsGenerated");
            proc.AddVarcharPara("@FromDate", 100, FromDate);
            proc.AddNVarcharPara("@ToDate", 100, ToDate);
            ds = proc.GetDataSet();
            return ds;
        }


        public DataSet TRADE_PROCESS_TEST(string trades_segment, string trades_settlementno, string tradedate, string CreateUser,
            string companyid,string ClientsID,string Instrument,string SettType,string EXCHANGESEGMENT)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("TRADE_PROCESS_TEST");
            proc.AddVarcharPara("@trades_segment", 100, trades_segment);
            proc.AddNVarcharPara("@trades_settlementno", 100, trades_settlementno);
            proc.AddVarcharPara("@tradedate", 100, tradedate);
            proc.AddIntegerPara("@CreateUser",Convert.ToInt32(CreateUser));
            proc.AddVarcharPara("@companyid", 100, companyid);
            proc.AddNVarcharPara("@ClientsID", -1, ClientsID);
            proc.AddVarcharPara("@Instrument", -1, Instrument);
            proc.AddNVarcharPara("@SettType", 100, SettType);
            proc.AddVarcharPara("@EXCHANGESEGMENT", 100, EXCHANGESEGMENT);                  

            ds = proc.GetDataSet();
            return ds;
        }



        public DataSet Sp_SttGenerationAndOther( string segment,string date,string Companyid,string ClientsID,string MasterSegment,
            string createuser,string forcntrnogenerate,string spname)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute(spname);
            proc.AddVarcharPara("@segment", 100, segment);
            proc.AddNVarcharPara("@date", 100, date);
            proc.AddVarcharPara("@Companyid", 100, Companyid);
            proc.AddNVarcharPara("@ClientsID", -1, ClientsID);
            proc.AddVarcharPara("@MasterSegment", 100, MasterSegment);
            proc.AddNVarcharPara("@createuser", 100, createuser);
            proc.AddNVarcharPara("@forcntrnogenerate", 100, forcntrnogenerate);
            ds = proc.GetDataSet();
            return ds;
        }


        public DataSet Sp_SttGenerationFO(string segment, string date, string Companyid, string ClientsID, string MasterSegment,
            string createuser,string SetNo,string SetType,string forcntrnogenerate)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Sp_SttGenerationFO");
            proc.AddVarcharPara("@segment", 100, segment);
            proc.AddNVarcharPara("@date", 100, date);
            proc.AddVarcharPara("@Companyid", 100, Companyid);
            proc.AddNVarcharPara("@ClientsID", -1, ClientsID);
            proc.AddVarcharPara("@MasterSegment", 100, MasterSegment);
            proc.AddNVarcharPara("@createuser", 100, createuser);
            proc.AddVarcharPara("@SetNo", 100, SetNo);
            proc.AddNVarcharPara("@SetType", 100, SetType);
            proc.AddNVarcharPara("@forcntrnogenerate", 100, forcntrnogenerate);
            ds = proc.GetDataSet();
            return ds;
        }


        public DataSet Sp_CONTRACTMODIFICATION( string FROMDATE,string TODATE,string SEGMENT,string COMPANYID,string CLIENTS,string TYPE)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Sp_CONTRACTMODIFICATION");
            proc.AddVarcharPara("@FROMDATE", 100, FROMDATE);
            proc.AddNVarcharPara("@TODATE", 100, TODATE);
            proc.AddVarcharPara("@SEGMENT", 100, SEGMENT);
            proc.AddNVarcharPara("@COMPANYID", 100, COMPANYID);
            proc.AddVarcharPara("@CLIENTS", -1, CLIENTS);
            proc.AddNVarcharPara("@TYPE", 100, TYPE);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet ShowIssuedSlipStatus(long DpSlipsIssuedID, string RunQuery)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Sp_ShowIssuedSlipStatus");
            proc.AddBigIntegerPara("@DpSlipsIssuedID", DpSlipsIssuedID);
            proc.AddNVarcharPara("@RunQuery", 100, RunQuery);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataTable InsertMasterDPSlipsStockIssue(string DpId, string SlipType, string Account, string Poa, string IsLoosed,
            string DisFormat, string IssueDate, string SlipNoPrefix, string BookNoFrom, string SlipNoFrom, string SlipNoTo,string BookNoTo,
            string TotalNoOfBooks, string Remarks, string user)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("insertMaster_DPSlipsStockIssue");
            proc.AddVarcharPara("@DpId", 100, DpId);
            proc.AddNVarcharPara("@SlipType", 100, SlipType); 
            proc.AddVarcharPara("@Account", 100, Account);
            proc.AddNVarcharPara("@Poa", 100, Poa); 
            proc.AddVarcharPara("@IsLoosed", 100, IsLoosed);
            proc.AddNVarcharPara("@DisFormat", 100, DisFormat); 
            proc.AddVarcharPara("@IssueDate", 100, IssueDate);
            proc.AddNVarcharPara("@SlipNoPrefix", 100, SlipNoPrefix); 
            proc.AddVarcharPara("@BookNoFrom", 100, BookNoFrom);
            proc.AddNVarcharPara("@SlipNoFrom", 100, SlipNoFrom);
            proc.AddVarcharPara("@SlipNoTo", 100, SlipNoTo);
            proc.AddNVarcharPara("@BookNoTo", 100, BookNoTo); 
            proc.AddVarcharPara("@TotalNoOfBooks", 100, TotalNoOfBooks);
            proc.AddNVarcharPara("@Remarks", 100, Remarks);
            proc.AddVarcharPara("@user", 100, user); 
            proc.AddNVarcharPara("@BookNoTo", 100, BookNoTo);
            dt = proc.GetTable();
            return dt;
        }
        public DataSet FetchSlipsinwardregisterData(int BenAccountID, string exchangesegmentid, string branchid)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Sp_Fetch_slipsinwardregister_Data");
            proc.AddIntegerPara("@BenAccountID", BenAccountID);
            proc.AddVarcharPara("@exchangesegmentid", 100, exchangesegmentid);
            proc.AddVarcharPara("@branchid", 300, branchid);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet FetchSlipsinwardregisterDataCDSL(string CompID, string CdslClients_BOID, string exchangesegmentid, string CdslClients_BranchID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Fetch_slipsinwardregister_Data_CDSL");
            proc.AddVarcharPara("@CompID",100, CompID);
            proc.AddVarcharPara("@CdslClients_BOID", 100, CdslClients_BOID);
            proc.AddVarcharPara("@exchangesegmentid", 300, exchangesegmentid);
            proc.AddVarcharPara("@CdslClients_BranchID", 300, CdslClients_BranchID);
            ds = proc.GetDataSet();
            return ds;
        }

        public int InsertSlipinwardRegister(string SlipsInwardRegister_FinYear, string SlipsInwardRegister_CompanyID, int SlipsInwardRegister_BranchID,
            string SlipsInwardRegister_DPID, int SlipsInwardRegister_ClientID, string SlipsInwardRegister_ReceiptDateTime, int SlipsInwardRegister_SlipType,
            string SlipsInwardRegister_SlipNumber, int SlipsInwardRegister_Instructions, string SlipsInwardRegister_Status, string SlipsInwardRegister_Reason,
            string SlipsInwardRegister_TransactionDate, string SlipsInwardRegister_ExecutionDate, string SlipsInwardRegister_BatchNumber, string SlipsInwardRegister_BatchDateTime,
            int SlipsInwardRegister_CreateUser, string SlipsInwardRegister_CreateDateTime, int SlipsInwardRegister_ModifyUser, string SlipsInwardRegister_ModifyDateTime)
        {
            ProcedureExecute proc;
            try
            {


                using (proc = new ProcedureExecute("sp_insert_slipinwardregister"))
                {

                    proc.AddVarcharPara("@SlipsInwardRegister_FinYear", 100, SlipsInwardRegister_FinYear);
                    proc.AddVarcharPara("@SlipsInwardRegister_CompanyID", 100, SlipsInwardRegister_CompanyID);
                    proc.AddIntegerPara("@SlipsInwardRegister_BranchID", SlipsInwardRegister_BranchID);
                    proc.AddVarcharPara("@SlipsInwardRegister_DPID", 100, SlipsInwardRegister_DPID);
                    proc.AddIntegerPara("@SlipsInwardRegister_ClientID", SlipsInwardRegister_ClientID);
                    proc.AddVarcharPara("@SlipsInwardRegister_ReceiptDateTime", 100, SlipsInwardRegister_ReceiptDateTime); 
                    proc.AddIntegerPara("@SlipsInwardRegister_SlipType", SlipsInwardRegister_SlipType);
                    proc.AddVarcharPara("@SlipsInwardRegister_SlipNumber", 100, SlipsInwardRegister_SlipNumber);
                    proc.AddIntegerPara("@SlipsInwardRegister_Instructions", SlipsInwardRegister_Instructions);
                    proc.AddVarcharPara("@SlipsInwardRegister_Status",100, SlipsInwardRegister_Status);
                    proc.AddVarcharPara("@SlipsInwardRegister_Reason",100, SlipsInwardRegister_Reason);
                    proc.AddVarcharPara("@SlipsInwardRegister_TransactionDate", 100, SlipsInwardRegister_TransactionDate); 
                    proc.AddVarcharPara("@SlipsInwardRegister_ExecutionDate", 100, SlipsInwardRegister_ExecutionDate);
                    proc.AddVarcharPara("@SlipsInwardRegister_BatchNumber", 100, SlipsInwardRegister_BatchNumber);
                    proc.AddVarcharPara("@SlipsInwardRegister_BatchDateTime", 100, SlipsInwardRegister_BatchDateTime);
                    proc.AddIntegerPara("@SlipsInwardRegister_CreateUser", SlipsInwardRegister_CreateUser);
                    proc.AddVarcharPara("@SlipsInwardRegister_CreateDateTime",100, SlipsInwardRegister_CreateDateTime);
                    proc.AddIntegerPara("@SlipsInwardRegister_ModifyUser", SlipsInwardRegister_ModifyUser); 
                    proc.AddVarcharPara("@SlipsInwardRegister_ModifyDateTime", 100, SlipsInwardRegister_ModifyDateTime); 
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

        public DataTable CDSLUpdateDematCertificates(string time)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("cdsl_UpdateDematCertificates");
            proc.AddVarcharPara("@time", 100, time);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable CDSLUpdateDematCertificatesDuringBill(string startDate, string endDate)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("cdsl_UpdateDematCertificates_DuringBill");
            proc.AddVarcharPara("@startDate", 100, startDate);
            proc.AddVarcharPara("@endDate", 100, endDate);
            dt = proc.GetTable();
            return dt;
        }

        public int CDSLUpdateTransactionCertificates(string TransactionId)
        {
            ProcedureExecute proc;
            try
            {


                using (proc = new ProcedureExecute("cdsl_Update_Transaction_Certificates"))
                {

                    proc.AddVarcharPara("@TransactionId", 100, TransactionId);
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

        public DataTable EmailNsdlreport(string BenAccId)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Email_Nsdl_report");
            proc.AddVarcharPara("@BenAccId", 100, BenAccId);
            dt = proc.GetTable();
            return dt;
        }

        public int InsertManualDematTransaction(string FinYear, string TranDate, string CompanyID, string SegID, string CustomerID, string ISIN,
            string SettNumS, string SettTypeS, string SettNumT, string SettTypeT, string Quantity, string AccountS, string CustomerS, string AccountT,
            string CustomerT, string SourceDPID, string SourceClientID, string TargetDPID, string TargetClientID, string CreateUser, string DematType,
            string Scrip, string DematTransactions_SlipNumber, string DematTransactions_Remarks, string Type, string SourceSegment, string SourceCMBPID,
            string TargetCMBPID)
        {
            ProcedureExecute proc;
            try
            {


                using (proc = new ProcedureExecute("InsertManualDematTransaction"))
                {

                    proc.AddVarcharPara("@FinYear", 100, FinYear);
                    proc.AddVarcharPara("@TranDate", 100, TranDate);
                    proc.AddVarcharPara("@CompanyID", 100, CompanyID);
                    proc.AddVarcharPara("@SegID", 100, SegID);
                    proc.AddVarcharPara("@CustomerID",100, CustomerID);
                    proc.AddVarcharPara("@ISIN", 100, ISIN);
                    proc.AddVarcharPara("@SettNumS",100, SettNumS);
                    proc.AddVarcharPara("@SettTypeS", 100, SettTypeS);
                    proc.AddVarcharPara("@SettNumT",100, SettNumT);
                    proc.AddVarcharPara("@SettTypeT", 100, SettTypeT);
                    proc.AddVarcharPara("@Quantity", 100, Quantity);
                    proc.AddVarcharPara("@AccountS", 100, AccountS);
                    proc.AddVarcharPara("@CustomerS", 100, CustomerS);
                    proc.AddVarcharPara("@AccountT", 100, AccountT);
                    proc.AddVarcharPara("@CustomerT", 100, CustomerT);
                    proc.AddVarcharPara("@SourceDPID",100, SourceDPID);
                    proc.AddVarcharPara("@SourceClientID", 100, SourceClientID);
                    proc.AddVarcharPara("@TargetDPID",100, TargetDPID);
                    proc.AddVarcharPara("@TargetClientID", 100, TargetClientID); 
                    proc.AddVarcharPara("@CreateUser", 100, CreateUser);
                    proc.AddVarcharPara("@DematType",100, DematType);
                    proc.AddVarcharPara("@Scrip", 100, Scrip);
                    proc.AddVarcharPara("@DematTransactions_SlipNumber",100, DematTransactions_SlipNumber);
                    proc.AddVarcharPara("@DematTransactions_Remarks", -1, DematTransactions_Remarks); 
                    proc.AddVarcharPara("@Type", 100, Type);
                    proc.AddVarcharPara("@SourceSegment",100, SourceSegment);
                    proc.AddVarcharPara("@SourceCMBPID", 100, SourceCMBPID); 
                    proc.AddVarcharPara("@TargetCMBPID", 100, TargetCMBPID);
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

        public DataSet FetchInterExchange(string ISIN, int ExchSegmentID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("FetchInterExchange");
            proc.AddVarcharPara("@ISIN",100, ISIN);
            proc.AddIntegerPara("@ExchSegmentID", ExchSegmentID);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet Report_CheckDematOutGoingObligation(string Clients, string CompanyID, string Finyear, string Productid, string settlementnom,
            string settlementtype)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("[Report_CheckDematOutGoingObligation]");
            proc.AddVarcharPara("@Clients", -1, Clients);
            proc.AddVarcharPara("@CompanyID", 100, CompanyID);
            proc.AddVarcharPara("@Finyear", 100, Finyear);
            proc.AddVarcharPara("@Productid", 100, Productid); 
            proc.AddVarcharPara("@settlementnom", 100, settlementnom);
            proc.AddVarcharPara("@settlementtype", 100, settlementtype);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet Sp_MarginStocksInward_OutwardRegister(string fromdate, string todate, string clients, string segment, string Companyid,
            string Finyear, string productid, string accountid, string checkacc, string checksale, string checkpur, string checkdp, string chkledgerbaln,
            string chkcashmargin, string reporttype, string branch, string pursalevaluemethod, string negativeqty)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("[Sp_MarginStocksInward/OutwardRegister]");
            proc.AddVarcharPara("@fromdate", 100, fromdate);
            proc.AddVarcharPara("@todate", 100, todate);
            proc.AddVarcharPara("@clients", 100, clients);
            proc.AddVarcharPara("@segment", 100, segment);
            proc.AddVarcharPara("@Companyid", 100, Companyid);
            proc.AddVarcharPara("@Finyear", 100, Finyear); 
            proc.AddVarcharPara("@productid", 100, productid);
            proc.AddVarcharPara("@accountid", 100, accountid);
            proc.AddVarcharPara("@checkacc", 100, checkacc);
            proc.AddVarcharPara("@checksale", 100, checksale);
            proc.AddVarcharPara("@checkpur", 100, checkpur);
            proc.AddVarcharPara("@checkdp", 100, checkdp); 
            proc.AddVarcharPara("@chkledgerbaln", 100, chkledgerbaln);
            proc.AddVarcharPara("@chkcashmargin", 100, chkcashmargin); 
            proc.AddVarcharPara("@reporttype", 100, reporttype);
            proc.AddVarcharPara("@branch", -1, branch);
            proc.AddVarcharPara("@pursalevaluemethod", 100, pursalevaluemethod);
            proc.AddVarcharPara("@negativeqty", 100, negativeqty);
            ds = proc.GetDataSet();
            return ds;
        }

        public int xmlInterExchange(string ExchangeData, string createuser)
        {
            ProcedureExecute proc;
            try
            {


                using (proc = new ProcedureExecute("xmlInterExchange"))
                {

                    proc.AddNTextPara("@ExchangeData", ExchangeData);
                    proc.AddVarcharPara("@createuser", 100, createuser);
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
