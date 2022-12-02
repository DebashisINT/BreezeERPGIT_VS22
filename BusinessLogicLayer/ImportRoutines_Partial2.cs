using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public partial class ImportRoutines
    {
        public DataSet ImportNsdlCA1(string file_path, string userid, string create_modify_datetime, string version)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("sp_Import_NsdlCA1");
            proc.AddVarcharPara("@file_path", 300, file_path);
            proc.AddVarcharPara("@userid", 100, userid);
            proc.AddVarcharPara("@create_modify_datetime", 100, create_modify_datetime);
            proc.AddNVarcharPara("@version", 100, version);
            ds = proc.GetDataSet();
            return ds;
        }

        public void ImportNsdlPriceFile(string file_path, string userid, string create_modify_datetime)
        {
            ProcedureExecute proc;
            try
            {


                using (proc = new ProcedureExecute("sp_Import_NsdlPriceFile"))
                {

                    proc.AddVarcharPara("@file_path", 500, file_path);
                    proc.AddVarcharPara("@userid", 100, userid);
                    proc.AddVarcharPara("@create_modify_datetime", 100, create_modify_datetime);
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

        public DataSet ImportNsdlHoldings1(string file_path, string userid, string create_modify_datetime, string version)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("sp_Import_NsdlHoldings1");
            proc.AddVarcharPara("@file_path", 500, file_path);
            proc.AddVarcharPara("@userid", 100, userid);
            proc.AddVarcharPara("@create_modify_datetime", 100, create_modify_datetime);
            proc.AddNVarcharPara("@version", 100, version);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet ImportNsdlTransactions1(string file_path, string userid, string create_modify_datetime, string version)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("sp_Import_NsdlTransactions1");
            proc.AddVarcharPara("@file_path", 500, file_path);
            proc.AddVarcharPara("@userid", 100, userid);
            proc.AddVarcharPara("@create_modify_datetime", 100, create_modify_datetime);
            proc.AddNVarcharPara("@version", 100, version);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet ImportNsdlCOD1(string file_path, string userid, string create_modify_datetime, string version)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("sp_Import_NsdlCOD1");
            proc.AddVarcharPara("@file_path", 500, file_path);
            proc.AddVarcharPara("@userid", 100, userid);
            proc.AddVarcharPara("@create_modify_datetime", 100, create_modify_datetime);
            proc.AddNVarcharPara("@version", 100, version);
            ds = proc.GetDataSet();
            return ds;
        }

        public string ImportCDSLISINCD03(string FilePath, string Module, string ModifyUser, string Commodities)
        {
            ProcedureExecute proc;
            try
            {


                using (proc = new ProcedureExecute("SP_Import_CDSLISINCD03"))
                {

                    proc.AddVarcharPara("@FilePath", -1, FilePath);
                    proc.AddVarcharPara("@Module", 100, Module);
                    proc.AddVarcharPara("@ModifyUser", 100, ModifyUser);
                    proc.AddVarcharPara("@Commodities", 100, Commodities);
                    string i = Convert.ToString(proc.RunActionQuery());
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

        public int cdslImportSattlementnew(string Module, string FilePath, string ModifyUser, string startdate, string enddate)
        {
            ProcedureExecute proc;
            try
            {


                using (proc = new ProcedureExecute("cdslImportSattlementnew"))
                {

                    proc.AddVarcharPara("@Module", 100, Module);
                    proc.AddVarcharPara("@FilePath", -1, FilePath);
                    proc.AddVarcharPara("@ModifyUser", 100, ModifyUser);
                    proc.AddVarcharPara("@startdate", 100, startdate);
                    proc.AddVarcharPara("@enddate", 100, enddate);
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

        public DataTable cdslISBilledMonth(string currentDate, string dpid)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("cdsl_ISBilledMonth");
            proc.AddVarcharPara("@currentDate", 500, currentDate);
            proc.AddVarcharPara("@dpid", 100, dpid);
            dt = proc.GetTable();
            return dt;
        }

        public int ImportCdslOfflineBatch(string Module, string CompanyId, int EnteredBy, string EntryUserRole, string UserDpId, string TransactionDate)
        {
            ProcedureExecute proc;
            try
            {


                using (proc = new ProcedureExecute("Import_CdslOfflineBatch"))
                {

                    proc.AddVarcharPara("@Module", 100, Module);
                    proc.AddVarcharPara("@CompanyId", 100, CompanyId);
                    proc.AddIntegerPara("@EnteredBy", EnteredBy);
                    proc.AddVarcharPara("@EntryUserRole", 100, EntryUserRole);
                    proc.AddVarcharPara("@UserDpId", 100, UserDpId);
                    proc.AddVarcharPara("@TransactionDate", 100, TransactionDate);
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

        public void INSUPNSEFOClosingRates(string Module, string FilePath, string Date)
        {
            ProcedureExecute proc;
            try
            {


                using (proc = new ProcedureExecute("[SP_INSUP_NSEFOClosingRates]"))
                {

                    proc.AddVarcharPara("@Module", 100, Module);
                    proc.AddVarcharPara("@FilePath", 255, FilePath);
                    proc.AddVarcharPara("@Date", 100, Date);
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

        public void XMLCashBankInsert(string cashBankData, string createuser, string finyear, string compID, string CashBankName, string TDate, int BRS)
        {
            ProcedureExecute proc;
            try
            {


                using (proc = new ProcedureExecute("xmlCashBankInsert"))
                {

                    proc.AddNTextPara("@cashBankData", cashBankData);
                    proc.AddVarcharPara("@createuser", 255, createuser);
                    proc.AddVarcharPara("@finyear", 100, finyear); 
                    proc.AddVarcharPara("@compID", 100, compID);
                    proc.AddVarcharPara("@CashBankName", 255, CashBankName);
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
    }
}
