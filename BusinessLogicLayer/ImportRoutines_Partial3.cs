using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public partial class ImportRoutines
    {

        public void Insert_MarginBSE( string Module,string FilePath,string FilePath1,string ModifyUser,string ExcSegmt,string ExchangeTrades_CompanyID)
        {
            ProcedureExecute proc;
            int rtrnvalue = 0;
            try
            {
                using (proc = new ProcedureExecute("SP_INSUP_MarginBSE"))
                {

                    proc.AddVarcharPara("@Module", 100, Module);
                    proc.AddVarcharPara("@FilePath", 255, FilePath);
                    proc.AddVarcharPara("@FilePath1", 255, FilePath1);
                    proc.AddIntegerPara("@ModifyUser", Convert.ToInt32(ModifyUser));
                    proc.AddIntegerPara("@ExcSegmt", Convert.ToInt32( ExcSegmt));
                    proc.AddVarcharPara("@ExchangeTrades_CompanyID", 100, ExchangeTrades_CompanyID);
                  

                   
                                       


                    rtrnvalue = proc.RunActionQuery();
                    //  rtrnvalue = proc.GetParaValue("@ResultCode").ToString();
                   // return rtrnvalue;


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


        public void xmlCashBankInsertFor_MultipleDate( string cashBankData,string createuser,string finyear,string compID,string CashBankName,string TDate,string BRS)
        {
            ProcedureExecute proc;
            int rtrnvalue = 0;
            try
            {
                using (proc = new ProcedureExecute("xmlCashBankInsertFor_MultipleDate"))
                {

                    proc.AddVarcharPara("@cashBankData", 100, cashBankData);
                    proc.AddVarcharPara("@createuser", 100, createuser);
                    proc.AddVarcharPara("@finyear", 100, finyear);
                    proc.AddVarcharPara("@compID", 100, compID);
                    proc.AddVarcharPara("@CashBankName",500,CashBankName);
                    proc.AddDateTimePara("@TDate", Convert.ToDateTime(TDate));
                    proc.AddIntegerPara("@BRS", Convert.ToInt32(BRS));

                                                 


                    rtrnvalue = proc.RunActionQuery();
                    //  rtrnvalue = proc.GetParaValue("@ResultCode").ToString();
                    // return rtrnvalue;


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
