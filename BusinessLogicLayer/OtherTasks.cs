using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessLayer;
using System.Data;

namespace BusinessLogicLayer
{
    public partial class OtherTasks
    {
        public DataSet InsertJournalVoucherEntry(string JournalVoucherXML, string createuser, string finyear, string compID, string JournalVoucherDetail_TransactionDate,
            string JournalVoucher_SettlementNumber, string JournalVoucher_SettlementType, string JournalVoucher_BillNumber, string JournalVoucher_Prefix,
            string segmentid, string WhichTypeItemsExist, string IBRef, string EntryUserProfile, string FormMode, string OldIBRef, string OldVoucherNumber,
            string OldWhichTypeItem, int CurrencyID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Insert_JournalVoucherEntry");
            proc.AddVarcharPara("@JournalVoucherXML", -1, JournalVoucherXML);
            proc.AddVarcharPara("@createuser", 100, createuser);
            proc.AddVarcharPara("@finyear", 100, finyear);
            proc.AddVarcharPara("@compID",15, compID); 
            proc.AddVarcharPara("@JournalVoucherDetail_TransactionDate", 100, JournalVoucherDetail_TransactionDate);
           // proc.AddVarcharPara("@JournalVoucher_SettlementNumber", 100, JournalVoucher_SettlementNumber);
            proc.AddVarcharPara("@JournalVoucher_SettlementNumber", 100, "");
            //proc.AddVarcharPara("@JournalVoucher_SettlementType", 100, JournalVoucher_SettlementType);
            proc.AddVarcharPara("@JournalVoucher_SettlementType", 100, "");
            proc.AddVarcharPara("@JournalVoucher_BillNumber", 100, JournalVoucher_BillNumber); 
            proc.AddVarcharPara("@JournalVoucher_Prefix", 100, JournalVoucher_Prefix);
            proc.AddVarcharPara("@segmentid", 100, segmentid);
            proc.AddVarcharPara("@WhichTypeItemsExist", 100, WhichTypeItemsExist);
            proc.AddVarcharPara("@IBRef",100, IBRef); 
            proc.AddVarcharPara("@EntryUserProfile", 100, EntryUserProfile);
            proc.AddVarcharPara("@FormMode", 100, FormMode);
            proc.AddVarcharPara("@OldIBRef", 100, OldIBRef);
            proc.AddVarcharPara("@OldVoucherNumber",100, OldVoucherNumber); 
            proc.AddVarcharPara("@OldWhichTypeItem", 100, OldWhichTypeItem);
            proc.AddIntegerPara("@CurrencyID", CurrencyID);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet Sp_ExpiryTradeGenerationCOMM(string segment, string expirydate, string MasterSegment, string Companyid,
            string instrumenttype, string Finyear, string creatuser, string Settno, string SettType)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("[Sp_ExpiryTradeGenerationCOMM]");
            proc.AddVarcharPara("@segment", 100, segment);
            proc.AddVarcharPara("@expirydate", 100, expirydate);
            proc.AddVarcharPara("@MasterSegment", 100, MasterSegment);
            proc.AddVarcharPara("@Companyid", 100, Companyid);
            proc.AddVarcharPara("@instrumenttype", 100, instrumenttype);
            proc.AddVarcharPara("@Finyear", 100, Finyear);
            proc.AddVarcharPara("@creatuser", 100, creatuser);
            proc.AddVarcharPara("@Settno", 100, Settno);
            proc.AddVarcharPara("@SettType", 100, SettType);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet ExpiryTradeGenerationOPTCommCurrency(string segment, string MasterSegment, string companyid, string expirydate,
            string Finyear, string GENERATETYPE, string UNDERLYING, string OPTIONTYPE, string Expiry, string INSTRUMENT)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("[ExpiryTradeGenerationOPTCommCurrency]");
            proc.AddVarcharPara("@segment", 100, segment);
            proc.AddVarcharPara("@MasterSegment", 100, MasterSegment);
            proc.AddVarcharPara("@companyid", 100, companyid);
            proc.AddVarcharPara("@expirydate", 100, expirydate);
            proc.AddVarcharPara("@Finyear", 100, Finyear);
            proc.AddVarcharPara("@GENERATETYPE", 100, GENERATETYPE);
            proc.AddVarcharPara("@UNDERLYING", 100, UNDERLYING);
            proc.AddVarcharPara("@OPTIONTYPE", 100, OPTIONTYPE);
            proc.AddVarcharPara("@Expiry", 100, Expiry);
            proc.AddVarcharPara("@INSTRUMENT", 100, INSTRUMENT);
            ds = proc.GetDataSet();
            return ds;
        }

        public int ExpiryTradeGenerationOPTCommCurrency_INSERT(string TABLEReport, string segment, string MasterSegment, string Companyid,
            string Finyear, string creatuser, string Settno, string SettType)
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            try
            {
                using (proc = new ProcedureExecute("[ExpiryTradeGenerationOPTCommCurrency_INSERT]"))
                {
                    proc.AddNTextPara("@TABLEReport", TABLEReport);
                    proc.AddVarcharPara("@segment", 100, segment);
                    proc.AddVarcharPara("@MasterSegment", 50, MasterSegment);
                    proc.AddVarcharPara("@Companyid", 50, Companyid);
                    proc.AddVarcharPara("@Finyear", 50, Finyear);
                    proc.AddVarcharPara("@creatuser", 50, creatuser);
                    proc.AddVarcharPara("@Settno",100, Settno);
                    proc.AddVarcharPara("@SettType", 50, SettType);

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
