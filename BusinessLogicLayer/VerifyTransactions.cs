using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public partial class VerifyTransactions
    {
        public DataSet FetchSingleCapturingVerification(string date, string WhichRecord, string WhichDp)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Fetch_SingleCapturing_Verification");
            proc.AddVarcharPara("@date", 100, date);
            proc.AddNVarcharPara("@WhichRecord", 100, WhichRecord);
            proc.AddVarcharPara("@WhichDp", 100, WhichDp);
            ds = proc.GetDataSet();
            return ds;
        }

        public int UpdateDeliveryInstructionVerification(string DP, int RecordID, string MarketFromValue, string SettlementFrom, int CMBPID, string Exchange, string OtherDPID,
            string OtherClientID,string MarketToValue,string SettlementTo,string ISIN,string QTY,string TransactionDate,string ExecutionDate,string CounterClientId,
            string NSDLDPID,string NSDLClientid,string NSDLSettlement, string EntryProfileType,string EntryUser,string CMID,string ClearingHouseID,string AcceptReject,
            int RejectedUser,string RejectedReason,string NoofCertificate,string DispatchDocID,string DispatchName,string DispatchDate, string LockinStatus,
            string LockinCode,string LockinRemark,string LockinExpirydate, string TransactionType)
        {
            ProcedureExecute proc;
            try
            {
                using (proc = new ProcedureExecute("Update_DeliveryInstruction_Verification"))
                { 
                    proc.AddVarcharPara("@DP", 100, DP);
                    proc.AddIntegerPara("@RecordID", RecordID);
                    proc.AddVarcharPara("@MarketFromValue", 100, MarketFromValue);
                    proc.AddVarcharPara("@SettlementFrom", 100, SettlementFrom);
                    proc.AddIntegerPara("@CMBPID", CMBPID);
                    proc.AddVarcharPara("@Exchange", 100, Exchange); 
                    proc.AddVarcharPara("@OtherDPID", 100, OtherDPID);
                    proc.AddVarcharPara("@OtherClientID", 100, OtherClientID);
                    proc.AddVarcharPara("@MarketToValue", 100, MarketToValue);
                    proc.AddVarcharPara("@SettlementTo", 100, SettlementTo);
                    proc.AddVarcharPara("@ISIN", 100, ISIN);
                    proc.AddVarcharPara("@QTY", 100, QTY); 
                    proc.AddVarcharPara("@TransactionDate", 100, TransactionDate);
                    proc.AddVarcharPara("@ExecutionDate", 100, ExecutionDate);
                    proc.AddVarcharPara("@CounterClientId", 100, CounterClientId);
                    proc.AddVarcharPara("@NSDLDPID", 100, NSDLDPID);
                    proc.AddVarcharPara("@NSDLClientid", 100, NSDLClientid);
                    proc.AddVarcharPara("@EntryProfileType", 100, EntryProfileType); 
                    proc.AddVarcharPara("@EntryUser", 100, EntryUser);
                    proc.AddVarcharPara("@CMID", 100, CMID);
                    proc.AddVarcharPara("@ClearingHouseID", 100, ClearingHouseID);
                    proc.AddVarcharPara("@AcceptReject", 100, AcceptReject);
                    proc.AddIntegerPara("@RejectedUser", RejectedUser);
                    proc.AddVarcharPara("@RejectedReason", 100, RejectedReason); 
                    proc.AddVarcharPara("@NoofCertificate", 100, NoofCertificate);
                    proc.AddVarcharPara("@DispatchDocID", 100, DispatchDocID);
                    proc.AddVarcharPara("@DispatchName", 100, DispatchName);
                    proc.AddVarcharPara("@DispatchDate", 100, DispatchDate);
                    proc.AddVarcharPara("@LockinStatus", 100, LockinStatus);
                    proc.AddVarcharPara("@LockinCode", 100, LockinCode); 
                    proc.AddVarcharPara("@LockinRemark", 100, LockinRemark);
                    proc.AddVarcharPara("@LockinExpirydate", 100, LockinExpirydate);
                    proc.AddVarcharPara("@TransactionType", 100, TransactionType);
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

        public int VerifyDeliveryInstructionVerification(string doc, string WhichDP, int VerifierUserID, string WhichQuery,string EntryUserRole)
        {
            ProcedureExecute proc;
            try
            {
                using (proc = new ProcedureExecute("Verify_DeliveryInstruction_Verification"))
                {
                    proc.AddVarcharPara("@doc", -1, doc);
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

        public DataSet FetchVerifyDeliveryInstructionVerification(string doc, string WhichDP, int VerifierUserID, string WhichQuery, string EntryUserRole)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Verify_DeliveryInstruction_Verification");
            proc.AddVarcharPara("@doc", -1, doc);
            proc.AddNVarcharPara("@WhichDP", 100, WhichDP);
            proc.AddIntegerPara("@VerifierUserID", VerifierUserID); 
            proc.AddNVarcharPara("@WhichQuery", 100, WhichQuery);
            proc.AddVarcharPara("@EntryUserRole", 100, EntryUserRole);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet FetchAuthoriseVerifiedInstructionSlips(string TransDateFrom, string TransDateTo, string DpID, string WhichDp, int AuthUserId)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Fetch_Authorise_VerifiedInstructionSlips");
            proc.AddVarcharPara("@TransDateFrom", 100, TransDateFrom);
            proc.AddNVarcharPara("@TransDateTo", 100, TransDateTo);
            proc.AddNVarcharPara("@DpID",100, DpID);
            proc.AddNVarcharPara("@WhichDp", 100, WhichDp);
            proc.AddIntegerPara("@AuthUserId", AuthUserId);
            ds = proc.GetDataSet();
            return ds;
        }

        public int UpdateAuthoriseVerifiedInstructionSlips(string TransDate, string DpID, string SlipNumber, string ClientID, int SlipType, string WhichDp, int AuthUserId)
        {
            ProcedureExecute proc;
            try
            {
                using (proc = new ProcedureExecute("Update_Authorise_VerifiedInstructionSlips"))
                {
                    proc.AddVarcharPara("@TransDate", 100, TransDate);
                    proc.AddVarcharPara("@DpID", 100, DpID);
                    proc.AddVarcharPara("@SlipNumber", 100, SlipNumber);
                    proc.AddVarcharPara("@ClientID", 100, ClientID);
                    proc.AddIntegerPara("@SlipType", SlipType);
                    proc.AddVarcharPara("@WhichDp", 100, WhichDp);
                    proc.AddIntegerPara("@AuthUserId", AuthUserId);
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

        public int UpdateDeliveryInstructionVerification(string DP, int RecordID, string MarketFromValue, string SettlementFrom, int CMBPID, string Exchange, string OtherDPID,
            string OtherClientID, string MarketToValue, string SettlementTo, string ISIN, string QTY, string TransactionDate, string ExecutionDate, string CounterClientId,
            string NSDLDPID, string NSDLClientid, string NSDLSettlement, string EntryProfileType, string EntryUser, string CMID, string ClearingHouseID, string AcceptReject,
            int RejectedUser, string RejectedReason, string TransferRsn, string RsnPurpose, string Consideration, string NoofCertificate, string DispatchDocID, string DispatchName, string DispatchDate, string LockinStatus,
            string LockinCode, string LockinRemark, string LockinExpirydate, string TransactionType)
        {
            ProcedureExecute proc;
            try
            {
                using (proc = new ProcedureExecute("Update_DeliveryInstruction_Verification"))
                {
                    proc.AddVarcharPara("@DP", 100, DP);
                    proc.AddIntegerPara("@RecordID", RecordID);
                    proc.AddVarcharPara("@MarketFromValue", 100, MarketFromValue);
                    proc.AddVarcharPara("@SettlementFrom", 100, SettlementFrom);
                    proc.AddIntegerPara("@CMBPID", CMBPID);
                    proc.AddVarcharPara("@Exchange", 100, Exchange);
                    proc.AddVarcharPara("@OtherDPID", 100, OtherDPID);
                    proc.AddVarcharPara("@OtherClientID", 100, OtherClientID);
                    proc.AddVarcharPara("@MarketToValue", 100, MarketToValue);
                    proc.AddVarcharPara("@SettlementTo", 100, SettlementTo);
                    proc.AddVarcharPara("@ISIN", 100, ISIN);
                    proc.AddVarcharPara("@QTY", 100, QTY);
                    proc.AddVarcharPara("@TransactionDate", 100, TransactionDate);
                    proc.AddVarcharPara("@ExecutionDate", 100, ExecutionDate);
                    proc.AddVarcharPara("@CounterClientId", 100, CounterClientId);
                    proc.AddVarcharPara("@NSDLDPID", 100, NSDLDPID); 
                    proc.AddVarcharPara("@NSDLClientid", 100, NSDLClientid);
                    proc.AddVarcharPara("@NSDLSettlement", 100, NSDLSettlement);
                    proc.AddVarcharPara("@EntryProfileType", 100, EntryProfileType);
                    proc.AddVarcharPara("@EntryUser", 100, EntryUser);
                    proc.AddVarcharPara("@CMID", 100, CMID);
                    proc.AddVarcharPara("@ClearingHouseID", 100, ClearingHouseID);
                    proc.AddVarcharPara("@AcceptReject", 100, AcceptReject);
                    proc.AddIntegerPara("@RejectedUser", RejectedUser);
                    proc.AddVarcharPara("@RejectedReason", 100, RejectedReason);
                    proc.AddVarcharPara("@TransferRsn", 100, TransferRsn);
                    proc.AddVarcharPara("@RsnPurpose", 100, RsnPurpose);
                    proc.AddVarcharPara("@Consideration", 100, RejectedReason);
                    proc.AddVarcharPara("@NoofCertificate", 100, NoofCertificate);
                    proc.AddVarcharPara("@DispatchDocID", 100, DispatchDocID);
                    proc.AddVarcharPara("@DispatchName", 100, DispatchName);
                    proc.AddVarcharPara("@DispatchDate", 100, DispatchDate);
                    proc.AddVarcharPara("@LockinStatus", 100, LockinStatus);
                    proc.AddVarcharPara("@LockinCode", 100, LockinCode);
                    proc.AddVarcharPara("@LockinRemark", 100, LockinRemark);
                    proc.AddVarcharPara("@LockinExpirydate", 100, LockinExpirydate);
                    proc.AddVarcharPara("@TransactionType", 100, TransactionType);
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
