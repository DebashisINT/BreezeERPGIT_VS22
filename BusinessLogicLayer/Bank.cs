using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public class Bank
    {



        public DataSet HDFC_BANK(string vModule, string vBank, string vModifyuser, string vExcSegmt, string vFilePath, string vBankCode, string vCompanyID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("SP_INSUP_BANKSTATEMENT1");
            proc.AddVarcharPara("@Module", 100, vModule);
            proc.AddVarcharPara("@Bank", 100, vBank);
            proc.AddVarcharPara("@ModifyUser", 10, vModifyuser);
            proc.AddIntegerPara("@ExcSegmt", Convert.ToInt32(vExcSegmt));
            proc.AddVarcharPara("@FilePath", 250, vFilePath);
            proc.AddNVarcharPara("@BankCode", 250, vBankCode);
            proc.AddVarcharPara("@ExchangeTrades_CompanyID", 10, vCompanyID);
            ds = proc.GetDataSet();
            return ds;
        }


        public DataSet HDFC_BANK_EasyView(string vModule, string vBank, string vModifyuser, string vExcSegmt, string vFilePath, string vBankCode, string vCompanyID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("SP_INSUP_BANKSTATEMENT1");
            proc.AddVarcharPara("@Module", 100, vModule);
            proc.AddVarcharPara("@Bank", 100, vBank);
            proc.AddVarcharPara("@ModifyUser", 10, vModifyuser);
            proc.AddIntegerPara("@ExcSegmt", Convert.ToInt32(vExcSegmt));
            proc.AddVarcharPara("@FilePath", 250, vFilePath);
            proc.AddNVarcharPara("@BankCode", 250, vBankCode);
            proc.AddVarcharPara("@ExchangeTrades_CompanyID", 10, vCompanyID);
            ds = proc.GetDataSet();
            return ds;
        }


        public DataSet ICICIBankStatement(string vModifyuser, string vExcSegmt, string vFilePath, string vBankCode, string vCompanyID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Import_ICICIBankStatement");
            //proc.AddVarcharPara("@Module", 100, vModule);
            //proc.AddVarcharPara("@Bank", 100, vBank);
            proc.AddVarcharPara("@ModifyUser", 10, vModifyuser);
            proc.AddIntegerPara("@ExcSegmt", Convert.ToInt32(vExcSegmt));
            proc.AddVarcharPara("@FilePath", 250, vFilePath);
            proc.AddVarcharPara("@BankCode", 250, vBankCode);
            proc.AddVarcharPara("@ExchangeTrades_CompanyID", 10, vCompanyID);
            ds = proc.GetDataSet();
            return ds;
        }


        public DataSet Bind_Data(string vModule, string vBank, string vModifyuser, string vExcSegmt, string vFilePath, string vBankCode, string vCompanyID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("SP_INSUP_BANKSTATEMENT");
            proc.AddVarcharPara("@Module", 100, vModule);
            proc.AddVarcharPara("@Bank", 100, vBank);
            proc.AddVarcharPara("@ModifyUser", 10, vModifyuser);
            proc.AddIntegerPara("@ExcSegmt", Convert.ToInt32(vExcSegmt));
            proc.AddVarcharPara("@FilePath", 250, vFilePath);
            proc.AddVarcharPara("@BankCode", 250, vBankCode);
            proc.AddVarcharPara("@ExchangeTrades_CompanyID", 10, vCompanyID);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet UpdateBankAccountsLedgerByInstrumentNumber(DateTime Date, string NarrationDescription, string InstrumentNumber, DateTime ValueDt, Decimal Withdrawal, Decimal Deposit, Decimal ClosingBalance, Decimal bankamount, String AccountCode)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_BankAccountsReconciliationByInstrumentNumber_Update");
            proc.AddDateTimePara("@DATE", Date);
            proc.AddVarcharPara("@NARRATIONDESCRIPTION", 200, NarrationDescription);
            proc.AddVarcharPara("@INSTRUMENTNUMBER", 200, InstrumentNumber);
            proc.AddDateTimePara("@VALUEDATE", ValueDt);
            proc.AddDecimalPara("@WITHDRAWALAMT", 2, 18, Withdrawal);
            proc.AddDecimalPara("@DEPOSITAMT", 2, 18, Deposit);
            proc.AddDecimalPara("@CLOSINGBALANCE", 2, 18, ClosingBalance);
            proc.AddDecimalPara("@BANKAMOUNT", 2, 18, bankamount);

            proc.AddVarcharPara("@BANKACCOUNTCODE",50, AccountCode);

            ds = proc.GetDataSet();
            return ds;
        }


        public DataSet GetBankAccountsReconciliationLog()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_BankAccountsReconciliationLog_Get");
            ds = proc.GetDataSet();
            return ds;
        }


    }
}
