using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class MainAccountBal
    {
        public DataSet PopulateMainAccountDtls(ref string retmsg, int userId)
        {
            DataSet ds = new DataSet();
            string output = string.Empty;
            try
            {
                ProcedureExecute proc = new ProcedureExecute("prc_mainaccountdetails");
                proc.AddVarcharPara("@Action", 20, "get_acnt_dtls");
                proc.AddIntegerPara("@userId", userId);
                proc.AddVarcharPara("@is_success", 500, "", QueryParameterDirection.Output);
                ds = proc.GetDataSet();
                output = Convert.ToString(proc.GetParaValue("@is_success"));

            }
            catch (Exception ex)
            {
                output = ex.Message.ToString();
            }
            
            
                retmsg = output;
                return ds;
            

        }

        public DataTable GetAccountGroup(ref string retmsg, string asset_type)
        {
            DataTable dt = new DataTable();
            string output = string.Empty;
            try
            {
                ProcedureExecute proc = new ProcedureExecute("prc_mainaccountdetails");
                proc.AddVarcharPara("@Action", 20, "get_acnt_grp_dtls");
                proc.AddVarcharPara("@asset_type", 25, asset_type);
                proc.AddVarcharPara("@is_success", 500, "", QueryParameterDirection.Output);
                dt = proc.GetTable();
                output = Convert.ToString(proc.GetParaValue("@is_success"));

            }
            catch (Exception ex)
            {

            }
            if (output == "true")
            {
                return dt;
            }
            else
            {
                retmsg = output;
                return null;
            }
        }

        public string MainAccountSave(long acnt_id,string action,string AccountType, string BankCompany,
           string BankCashType, string AccountCode, string AccountGroup,
           string AccountName, string BankAccountNo, string SubLedgerType, string TDSRate,
            decimal Depreciation, string CreateUser, string paymentTypevalue, string strOldUnitLedger, string strReverseApplicable,DataTable dt,string modify_user,ref string NewId
            , string BalLimit, string NegStock, string DailyLimit, string DailyLimitExceed,bool Isparty,string DeductStatus=null,string TaxEntityType=null
            , string ddlModule=null)
        {
            ProcedureExecute proc;
            string output = string.Empty;
            int NoOfRowEffected = 0;
            try
            {
               
               using (proc = new ProcedureExecute("prc_mainaccountdetails"))
                {

                    proc.AddVarcharPara("@Action", 20, action);
                    proc.AddBigIntegerPara("@main_acnt_id", acnt_id);
                    proc.AddVarcharPara("@AccountType", 100, AccountType);
                    proc.AddVarcharPara("@BankCompany", 20, BankCompany);
                    proc.AddVarcharPara("@BankCashType", 100, BankCashType);
                    proc.AddVarcharPara("@AccountCode", 50, AccountCode);
                    proc.AddVarcharPara("@AccountGroup", 100, AccountGroup);
                    proc.AddVarcharPara("@AccountName", 100, AccountName);
                    proc.AddVarcharPara("@BankAccountNo", 100, BankAccountNo);
                    proc.AddVarcharPara("@SubLedgerType", 100, SubLedgerType);
                    proc.AddVarcharPara("@TDSRate", 100, TDSRate);
                    proc.AddDecimalPara("@Depreciation", 2, 6, Depreciation);
                    proc.AddIntegerPara("@CreateUser", Convert.ToInt32(CreateUser));
                    proc.AddVarcharPara("@MainAccount_PaymentType", 1000, paymentTypevalue);
                    proc.AddIntegerPara("@oldUnitLedger", Convert.ToInt32(strOldUnitLedger));
                    proc.AddIntegerPara("@ReverseApplicable", Convert.ToInt32(strReverseApplicable));
                    proc.AddVarcharPara("@BalLimit", 100, BalLimit);
                    proc.AddVarcharPara("@NegStock", 100, NegStock);

                    proc.AddVarcharPara("@DailyLimit", 100, DailyLimit);
                    proc.AddVarcharPara("@DailyLimitExceed", 100, DailyLimitExceed);
                    proc.AddBooleanPara("@IsParty", Isparty);
                    proc.AddVarcharPara("@MainAccount_DeducteeStatus",20 ,DeductStatus);
                    proc.AddPara("@PARAMTABLE", dt);
                    proc.AddIntegerPara("@lastmodifyuser", Convert.ToInt32(modify_user));
                    proc.AddVarcharPara("@MainAccount_TaxEntityType", 10, TaxEntityType);
                    proc.AddVarcharPara("@ModuleSet", 500, ddlModule);


                    proc.AddVarcharPara("@is_success", 500, "", QueryParameterDirection.Output);
                    proc.AddVarcharPara("@returnId", 500, "", QueryParameterDirection.Output);
                    NoOfRowEffected = proc.RunActionQuery();
                    
                    output = Convert.ToString(proc.GetParaValue("@is_success"));
                    NewId = Convert.ToString(proc.GetParaValue("@returnId"));
                   
                   
                }
            }

            catch (Exception ex)
            {
                output = ex.Message.ToString();
            }

            finally
            {
                proc = null;
            }
            return output;
        }
        public DataSet PopulateMainAccountDtlsById(ref string retmsg, string acnt_reference_id)
        {
            DataSet ds = new DataSet();
            string output = string.Empty;
            try
            {
                ProcedureExecute proc = new ProcedureExecute("prc_mainaccountdetails");
                proc.AddVarcharPara("@Action", 20, "Select");
                proc.AddBigIntegerPara("@main_acnt_id", Convert.ToInt64(acnt_reference_id));
                proc.AddVarcharPara("@is_success", 500, "", QueryParameterDirection.Output);
                ds = proc.GetDataSet();
                output = Convert.ToString(proc.GetParaValue("@is_success"));

            }
            catch (Exception ex)
            {
                output = ex.Message.ToString();
            }
            retmsg = output;
            return ds;

        }
        public DataTable TransactionCheck(ref string Trans_msg, string acnt_reference_id)
        {
            DataTable ds = new DataTable();
            string output = string.Empty;
            try
            {
                ProcedureExecute proc = new ProcedureExecute("prc_mainaccountdetails");
                proc.AddVarcharPara("@Action", 30, "TransactionExistCheck");
                proc.AddBigIntegerPara("@main_acnt_id", Convert.ToInt64(acnt_reference_id));
                proc.AddVarcharPara("@returnId", 500, "", QueryParameterDirection.Output);
                proc.AddVarcharPara("@is_success", 500, "", QueryParameterDirection.Output);
                ds = proc.GetTable();
                output = Convert.ToString(proc.GetParaValue("@returnId"));

            }
            catch (Exception ex)
            {
                output = ex.Message.ToString();
            }
            Trans_msg = output;
            return ds;

        }
    }
}
