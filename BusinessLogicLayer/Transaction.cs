using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public partial class Transaction
    {
        public void InsertDematnNSDL(string NsdlOffline_ID, string NsdlOffline_DPID, string NsdlOffline_FinYear, string NsdlOffline_TransactionDate,
            string NsdlOffline_ExecutionDate, string NsdlOffline_TransactionType, string NsdlOffline_ClientID, string NsdlOffline_ISIN,
            string NsdlOffline_Quantity, string NsdlOffline_SlipNumber, string NsdlOffline_Certificates, string NsdlOffline_EnteredBy,
            string NsdlOffline_EntryUserRole, string NsdlOffline_EntryDateTime, string NsdlOffline_BranchID)
        {
            ProcedureExecute proc;
            try
            {


                using (proc = new ProcedureExecute("sp_insert_demat_nsdl"))
                {

                    proc.AddVarcharPara("@NsdlOffline_ID", 100, NsdlOffline_ID);
                    proc.AddVarcharPara("@NsdlOffline_DPID", 100, NsdlOffline_DPID);
                    proc.AddVarcharPara("@NsdlOffline_FinYear", 100, NsdlOffline_FinYear); 
                    proc.AddVarcharPara("@NsdlOffline_TransactionDate", 100, NsdlOffline_TransactionDate);
                    proc.AddVarcharPara("@NsdlOffline_ExecutionDate", 100, NsdlOffline_ExecutionDate);
                    proc.AddVarcharPara("@NsdlOffline_TransactionType", 100, NsdlOffline_TransactionType); 
                    proc.AddVarcharPara("@NsdlOffline_ClientID", 100, NsdlOffline_ClientID);
                    proc.AddVarcharPara("@NsdlOffline_ISIN", 100, NsdlOffline_ISIN);
                    proc.AddVarcharPara("@NsdlOffline_Quantity", 100, NsdlOffline_Quantity); 
                    proc.AddVarcharPara("@NsdlOffline_SlipNumber", 100, NsdlOffline_SlipNumber);
                    proc.AddVarcharPara("@NsdlOffline_Certificates", 100, NsdlOffline_Certificates);
                    proc.AddVarcharPara("@NsdlOffline_EnteredBy", 100, NsdlOffline_EnteredBy); 
                    proc.AddVarcharPara("@NsdlOffline_EntryUserRole", 100, NsdlOffline_EntryUserRole);
                    proc.AddVarcharPara("@NsdlOffline_EntryDateTime", 100, NsdlOffline_EntryDateTime);
                    proc.AddVarcharPara("@NsdlOffline_BranchID", 100, NsdlOffline_BranchID);
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

        public DataTable FetchDoublecaptureEnteredSlipDetail(string dp, string dpId, string slipType, string slipNo, string usertype) 
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("sp_Fetch_doublecapture_EnteredSlipDetail");
            proc.AddVarcharPara("@dp", 100, dp);
            proc.AddNVarcharPara("@dpId", 100, dpId);
            proc.AddVarcharPara("@slipType", 100, slipType);
            proc.AddNVarcharPara("@slipNo", 100, slipNo);
            proc.AddNVarcharPara("@usertype", 100, usertype);
            dt = proc.GetTable();
            return dt;
        }

        public int InsertSettlementDoublecapture(string dp, string doc, string user, string dpid, int sliptype, string IsAcDormant)
        {
            ProcedureExecute proc;
            try
            {


                using (proc = new ProcedureExecute("sp_insert_settlement_doublecapture"))
                {

                    proc.AddVarcharPara("@dp", 100, dp);
                    proc.AddVarcharPara("@doc", -1, doc);
                    proc.AddVarcharPara("@user", 100, user);
                    proc.AddVarcharPara("@dpid", 100, dpid);
                    proc.AddIntegerPara("@sliptype", sliptype);
                    proc.AddVarcharPara("@IsAcDormant", 100, IsAcDormant);
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

        public DataTable FetchDetailsForAccountTransfer(string dp, string dpId, string benaccno)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Fetch_DetailsForAccountTransfer");
            proc.AddVarcharPara("@dp", 100, dp);
            proc.AddNVarcharPara("@dpId", 100, dpId);
            proc.AddVarcharPara("@benaccno", 100, benaccno);
            dt = proc.GetTable();
            return dt;
        }

        public DataSet FetchAccountTransfer(string Seg, string DpId, string BenAccNo, string ClientId, string CompanyId, string FinYear,
            string Transactiondate, string ExecutionDate, int TransactionType, string SlipNo, string OtherDpId, string OtherClientId, int SlipType,
            string EnteredBy, string HoldingDatetime)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Fetch_AccountTransfer");
            proc.AddVarcharPara("@Seg", 100, Seg);
            proc.AddNVarcharPara("@DpId", 100, DpId);
            proc.AddVarcharPara("@BenAccNo", 100, BenAccNo);

            proc.AddVarcharPara("@ClientId", 100, ClientId);
            proc.AddNVarcharPara("@CompanyId", 100, CompanyId);
            proc.AddVarcharPara("@FinYear", 100, FinYear); 
            proc.AddVarcharPara("@Transactiondate", 100, Transactiondate);
            proc.AddNVarcharPara("@ExecutionDate", 100, ExecutionDate);
            proc.AddIntegerPara("@TransactionType", TransactionType); 
            proc.AddVarcharPara("@SlipNo", 100, SlipNo);
            proc.AddNVarcharPara("@OtherDpId", 100, OtherDpId);
            proc.AddVarcharPara("@OtherClientId", 100, OtherClientId); 
            proc.AddIntegerPara("@SlipType", SlipType);
            proc.AddNVarcharPara("@EnteredBy", 100, EnteredBy);
            proc.AddVarcharPara("@HoldingDatetime", 100, HoldingDatetime);
            ds = proc.GetDataSet();
            return ds;
        }

        public int InsertAccountTransfer(string seg, int TransactionType, string SlipNo, int SlipType, string doc)
        {
            ProcedureExecute proc;
            try
            {


                using (proc = new ProcedureExecute("Insert_AccountTransfer"))
                {

                    proc.AddVarcharPara("@seg", 100, seg);
                    proc.AddIntegerPara("@TransactionType", TransactionType);
                    proc.AddVarcharPara("@SlipNo", 100, SlipNo);
                    proc.AddIntegerPara("@SlipType", SlipType);
                    proc.AddVarcharPara("@doc", -1, doc);
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

        public int InsertDematTransfer(string seg, int TransactionType, string SlipNo, int SlipType, string doc)
        {
            ProcedureExecute proc;
            try
            {


                using (proc = new ProcedureExecute("Insert_DematTransfer"))
                {

                    proc.AddVarcharPara("@seg", 100, seg);
                    proc.AddIntegerPara("@TransactionType", TransactionType);
                    proc.AddVarcharPara("@SlipNo", 100, SlipNo);
                    proc.AddIntegerPara("@SlipType", SlipType);
                    proc.AddVarcharPara("@doc", -1, doc);
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

        public void TRansactionInsert(string TranCharge_CompanyID, long TranCharge_ExchangeSegmentID, string TranCharge_DateFrom,
            decimal TranCharge_Rate1, string TranCharge_Time, decimal TranCharge_PoRate, decimal TranCharge_RateFut, decimal TranCharge_RateFutPO,
            decimal TranCharge_RateFutStk, decimal TranCharge_RateFutStkPO, decimal TranCharge_RateFutIdx, decimal TranCharge_RateFutIdxPO,
            decimal TranCharge_RateFutExp, decimal TranCharge_RateFutExpPO, string TranCharge_OptBasis, decimal TranCharge_RateOpt,
            decimal TranCharge_RateOptPO, decimal TranCharge_RateOptStk, decimal TranCharge_RateOptStkPO, decimal TranCharge_RateOptIdx, 
            decimal TranCharge_RateOptIdxPO, string TranCharge_OptFSBasis, decimal TranCharge_RateOptFS, decimal TranCharge_RateOptFSPO, 
            decimal TranCharge_RateOptStkFS, decimal TranCharge_RateOptStkFSPO, decimal TranCharge_RateOptIdxFS, decimal TranCharge_RateOptIdxFSPO,
            decimal TranCharge_RateIPC, string TranCharge_STApplicable, string TranCharge_ApplicableFor, string TranCharge_GnAcCodeClients,
            string TranCharge_SbAcCodeClients, string TranCharge_GnAcCodeExch, string TranCharge_SbAcCodeExch, string TranCharge_GnAcCodeST,
            string TranCharge_SbAcCodeST, int TranCharge_CreateUser, string TranCharge_CreateDateTime, int TranCharge_ModifyUser,
            string TranCharge_ChargeGroupID)
        {
            ProcedureExecute proc;
            try
            {


                using (proc = new ProcedureExecute("TRansactionInsert"))
                {

                    proc.AddVarcharPara("@TranCharge_CompanyID", 100, TranCharge_CompanyID);
                    proc.AddBigIntegerPara("@TranCharge_ExchangeSegmentID", TranCharge_ExchangeSegmentID);
                    proc.AddVarcharPara("@TranCharge_DateFrom", 100, TranCharge_DateFrom);
                    proc.AddDecimalPara("@TranCharge_Rate1", 2,28, TranCharge_Rate1);
                    proc.AddVarcharPara("@TranCharge_Time", 100, TranCharge_Time);
                    proc.AddDecimalPara("@TranCharge_PoRate",  2,28, TranCharge_PoRate); 
                    proc.AddDecimalPara("@TranCharge_RateFut", 2,28,  TranCharge_RateFut);
                    proc.AddDecimalPara("@TranCharge_RateFutPO", 2,28,  TranCharge_RateFutPO);
                    proc.AddDecimalPara("@TranCharge_RateFutStk",2, 28, TranCharge_RateFutStk);
                    proc.AddDecimalPara("@TranCharge_RateFutStkPO", 2,28, TranCharge_RateFutStkPO);
                    proc.AddDecimalPara("@TranCharge_RateFutIdx", 2,28,  TranCharge_RateFutIdx);
                    proc.AddDecimalPara("@TranCharge_RateFutIdxPO", 2,28,  TranCharge_RateFutIdxPO); 
                    proc.AddDecimalPara("@TranCharge_RateFutExp", 2,28,  TranCharge_RateFutExp);
                    proc.AddDecimalPara("@TranCharge_RateFutExpPO", 2,28,  TranCharge_RateFutExpPO);
                    proc.AddVarcharPara("@TranCharge_OptBasis", 100, TranCharge_OptBasis);
                    proc.AddDecimalPara("@TranCharge_RateOpt", 2,28,  TranCharge_RateOpt);
                    proc.AddDecimalPara("@TranCharge_RateOptPO", 2,28,  TranCharge_RateOptPO);
                    proc.AddDecimalPara("@TranCharge_RateOptStk", 2,28,  TranCharge_RateOptStk); 
                    proc.AddDecimalPara("@TranCharge_RateOptStkPO",  2,28, TranCharge_RateOptStkPO);
                    proc.AddDecimalPara("@TranCharge_RateOptIdx",  2,28, TranCharge_RateOptIdx);
                    proc.AddDecimalPara("@TranCharge_RateOptIdxPO",  2,28, TranCharge_RateOptIdxPO);
                    proc.AddVarcharPara("@TranCharge_OptFSBasis", 100, TranCharge_OptFSBasis); 
                    proc.AddDecimalPara("@TranCharge_RateOptFS",  2,28, TranCharge_RateOptFS); 
                    proc.AddDecimalPara("@TranCharge_RateOptFSPO", 2,28,  TranCharge_RateOptFSPO);
                    proc.AddDecimalPara("@TranCharge_RateOptStkFS",  2,28, TranCharge_RateOptStkFS);
                    proc.AddDecimalPara("@TranCharge_RateOptStkFSPO",  2,28, TranCharge_RateOptStkFSPO);
                    proc.AddDecimalPara("@TranCharge_RateOptIdxFS", 2,28,  TranCharge_RateOptIdxFS);
                    proc.AddDecimalPara("@TranCharge_RateOptIdxFSPO", 2,28,  TranCharge_RateOptIdxFSPO);
                    proc.AddDecimalPara("@TranCharge_RateIPC",  2,28, TranCharge_RateIPC); 
                    proc.AddVarcharPara("@TranCharge_STApplicable", 100, TranCharge_STApplicable);
                    proc.AddVarcharPara("@TranCharge_ApplicableFor", 100, TranCharge_ApplicableFor);
                    proc.AddVarcharPara("@TranCharge_GnAcCodeClients", 100, TranCharge_GnAcCodeClients);
                    proc.AddVarcharPara("@TranCharge_SbAcCodeClients", 100, TranCharge_SbAcCodeClients);
                    proc.AddVarcharPara("@TranCharge_GnAcCodeExch", 100, TranCharge_GnAcCodeExch);
                    proc.AddVarcharPara("@TranCharge_SbAcCodeExch", 100, TranCharge_SbAcCodeExch); 
                    proc.AddVarcharPara("@TranCharge_GnAcCodeST", 100, TranCharge_GnAcCodeST);
                    proc.AddVarcharPara("@TranCharge_SbAcCodeST", 100, TranCharge_SbAcCodeST);
                    proc.AddIntegerPara("@TranCharge_CreateUser", TranCharge_CreateUser);
                    proc.AddVarcharPara("@TranCharge_CreateDateTime", 100, TranCharge_CreateDateTime);
                    proc.AddIntegerPara("@TranCharge_ModifyUser", TranCharge_ModifyUser);
                    proc.AddVarcharPara("@TranCharge_ChargeGroupID", 100, TranCharge_ChargeGroupID);
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
    }
}
