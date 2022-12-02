using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class TaxSchemeBl
    {
        public Boolean InsertProdTaxWise(int TaxRates_TaxCode, string TaxRatesSchemeName, string ProdList,DataTable dt_List, int Id)
        {
            ProcedureExecute proc;
            bool retval = true;
            try
            {
                DataSet dsInst = new DataSet();

                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);

                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

                SqlCommand cmd = new SqlCommand("InsProductTaxRate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@taxCode", TaxRates_TaxCode);
                cmd.Parameters.AddWithValue("@taxname", TaxRatesSchemeName);
                cmd.Parameters.AddWithValue("@ProdList", ProdList);
                cmd.Parameters.AddWithValue("@action", "Ins");
                cmd.Parameters.AddWithValue("@IDList", dt_List);
                cmd.Parameters.AddWithValue("@taxRatesId", Id);
                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);

                retval = true;

                //using (proc = new ProcedureExecute("InsProductTaxRate"))
                //{
                //    proc.AddIntegerPara("@taxCode", TaxRates_TaxCode);
                //    proc.AddVarcharPara("@taxname", 100, TaxRatesSchemeName);
                //    //proc.AddVarcharPara("@ProdList", 1000, ProdList);
                //    proc.AddVarcharPara("@ProdList", -1, ProdList);
                //    proc.AddVarcharPara("@action", 5, "Ins");
                //    int i = proc.RunActionQuery();
                //}
            }
            catch (Exception ex)
            {
                retval = false;
            }
            finally
            {
                proc = null;
            }
            return retval;
        }
        public Boolean UpdateProdTaxWise(int TaxRates_TaxCode, string TaxRatesSchemeName, string ProdList, DataTable dt_List,int Id)
        {
            ProcedureExecute proc;
            bool retval = true;
            try
            {
                DataSet dsInst = new DataSet();
               // SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI

                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("InsProductTaxRate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@taxCode", TaxRates_TaxCode);
                cmd.Parameters.AddWithValue("@taxname", TaxRatesSchemeName);
                cmd.Parameters.AddWithValue("@ProdList", ProdList);
                cmd.Parameters.AddWithValue("@action", "Upd");
                cmd.Parameters.AddWithValue("@IDList", dt_List);
                cmd.Parameters.AddWithValue("@taxRatesId", Id);
                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);

                retval = true;

                //using (proc = new ProcedureExecute("InsProductTaxRate"))
                //{
                //    proc.AddIntegerPara("@taxCode", TaxRates_TaxCode);
                //    proc.AddVarcharPara("@taxname", 100, TaxRatesSchemeName);
                //    //proc.AddVarcharPara("@ProdList", 1000, ProdList);
                //    proc.AddVarcharPara("@ProdList", -1, ProdList);
                //    proc.AddVarcharPara("@action", 5, "Upd");
                //    int i = proc.RunActionQuery();
                //}
            }
            catch (Exception ex)
            {
                retval = false;
            }
            finally
            {
                proc = null;
            }
            return retval;
        }
        public int DelProdTaxWise(int TaxRates_TaxCode)
        {
            ProcedureExecute proc;
            int retval;
            try
            {
                using (proc = new ProcedureExecute("InsProductTaxRate"))
                {
                    proc.AddIntegerPara("@taxCode", TaxRates_TaxCode);
                    proc.AddVarcharPara("@action", 5, "Del");
                    retval = proc.RunActionQuery();

                }
            }
            catch (Exception ex)
            {
                retval = 0;
            }
            finally
            {
                proc = null;
            }
            return retval;
        }


        public Boolean InsertLedgerTaxWise(long TaxConfigID, string LedgerList)
        {
            ProcedureExecute proc;
            bool retval = true;
            try
            {
                using (proc = new ProcedureExecute("prc_LedgerTaxRate"))
                {
                    proc.AddBigIntegerPara("@TaxRate_ID", TaxConfigID);
                    proc.AddVarcharPara("@LedgerList", -1, LedgerList);
                    proc.AddVarcharPara("@action", 5, "Ins");
                    int i = proc.RunActionQuery();

                }
            }
            catch (Exception ex)
            {
                retval = false;
            }
            finally
            {
                proc = null;
            }
            return retval;
        }

        public Boolean UpdateLedgerTaxWise(long TaxConfigID, string LedgerList)
        {
            ProcedureExecute proc;
            bool retval = true;
            try
            {
                using (proc = new ProcedureExecute("prc_LedgerTaxRate"))
                {
                    proc.AddBigIntegerPara("@TaxRate_ID", TaxConfigID);
                    proc.AddVarcharPara("@LedgerList", -1, LedgerList);
                    proc.AddVarcharPara("@action", 5, "Upd");
                    int i = proc.RunActionQuery();

                }
            }
            catch (Exception ex)
            {
                retval = false;
            }
            finally
            {
                proc = null;
            }
            return retval;
        }


        public DataTable GetProductList(int taxId)
        {
            ProcedureExecute proc;
            DataTable returntable = new DataTable();
            bool retval = true;
            try
            {
                using (proc = new ProcedureExecute("GetProductDetailsForTaxScheme"))
                {
                    proc.AddIntegerPara("@taxId", taxId);
                    returntable = proc.GetTable();
                }
            }
            catch (Exception ex)
            {
                retval = false;
            }
            finally
            {
                proc = null;
            }
            return returntable;
        }
        public DataTable GetProductListForScheme(int SchemeId)
        {
            ProcedureExecute proc;
            DataTable returntable = new DataTable();
            bool retval = true;
            try
            {
                using (proc = new ProcedureExecute("GetProductDetailsForTaxScheme"))
                {
                    proc.AddIntegerPara("@schemeId", SchemeId);
                    returntable = proc.GetTable();
                }
            }
            catch (Exception ex)
            {
                retval = false;
            }
            finally
            {
                proc = null;
            }
            return returntable;
        }

        public DataTable GetLedgerForScheme(int SchemeId)
        {
            ProcedureExecute proc;
            DataTable returntable = new DataTable();
            bool retval = true;
            try
            {
                using (proc = new ProcedureExecute("GetLedgerDetailsForTaxScheme"))
                {
                    //proc.AddIntegerPara("@schemeId", SchemeId);
                    proc.AddIntegerPara("@taxId", SchemeId);
                    returntable = proc.GetTable();
                }
            }
            catch (Exception ex)
            {
                retval = false;
            }
            finally
            {
                proc = null;
            }
            return returntable;
        }

        public int ChekTaxSchemeinUse(int taxSchemeId)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_checkTaxSchemeinUse");
            proc.AddIntegerPara("@id", taxSchemeId);
            proc.AddVarcharPara("@ReturnValue", 200, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;
        }
        public bool InsertProductWiseHSN(int TaxRates_TaxCode, string TaxRatesSchemeName, string HsnCodeList, int TaxRates_ID)
        {
            ProcedureExecute proc;
            bool retval = true;
            try
            {
                using (proc = new ProcedureExecute("SP_UpdateProductWiseHSN"))
                {
                    proc.AddIntegerPara("@TaxRates_TaxCode", TaxRates_TaxCode);
                    proc.AddVarcharPara("@TaxRatesSchemeName", 100, TaxRatesSchemeName);
                    proc.AddVarcharPara("@HsnCodeList", 5000, HsnCodeList);
                    proc.AddIntegerPara("@TaxRates_ID", TaxRates_ID);
                    int i = proc.RunActionQuery();

                }
            }
            catch (Exception ex)
            {
                retval = false;
            }
            finally
            {
                proc = null;
            }
            return retval;


        }

        public bool InsertHSNSACMappedWithLedger(int TaxRates_TaxCode, string TaxRatesSchemeName, string HsnSacCodeList, int TaxRates_ID)
        {
            ProcedureExecute proc;
            bool retval = true;
            try
            {
                using (proc = new ProcedureExecute("prc_InsertUpdateHSNSACWiseLedger"))
                {
                    proc.AddIntegerPara("@TaxRates_TaxCode", TaxRates_TaxCode);
                    proc.AddVarcharPara("@TaxRatesSchemeName", 100, TaxRatesSchemeName);
                    proc.AddVarcharPara("@HsnCodeList", -1, HsnSacCodeList);
                    proc.AddIntegerPara("@TaxRates_ID", TaxRates_ID);
                    int i = proc.RunActionQuery();

                }
            }
            catch (Exception ex)
            {
                retval = false;
            }
            finally
            {
                proc = null;
            }
            return retval;


        }
        public int GetTaxRates_ID(int TaxRates_TaxCode, string TaxRatesSchemeName)
        {
            try
            {
               // BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

                BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();

                DataTable DT = objEngine.GetDataTable("Config_TaxRates", " TaxRates_ID ", " TaxRates_TaxCode=" + TaxRates_TaxCode + " AND TaxRatesSchemeName = '" + TaxRatesSchemeName + "' ");
                if (DT != null && DT.Rows.Count > 0)
                {
                    int TaxRates_ID = Convert.ToInt32(DT.Rows[0]["TaxRates_ID"]);

                    if (TaxRates_ID > 0)
                    {
                        return TaxRates_ID;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex) { return 0; }
        }


        public void GetSchemeMaxupdateDate()
        {
            ProcedureExecute proc;
            using (proc = new ProcedureExecute("prc_taxschemeMaxdate"))
                {

                   DataTable maxdt = proc.GetTable();
                   System.Web.HttpContext.Current.Cache["SchemeMaxDate"] = maxdt.Rows[0][0];

                }
        }

        public Boolean SetHsnTaxRate(string Hsncode, int PurchaseCGST_TaxRatesId,int PurchaseIGST_TaxRatesId, int PurchaseSGST_TaxRatesId, int PurchaseUTGST_TaxRatesId,
          int SaleCGST_TaxRatesId, int SaleIGST_TaxRatesId, int SaleSGST_TaxRatesId, int SaleUTGST_TaxRatesId,int SacType,string moduleType)
        {
          
            ProcedureExecute proc;
            bool retval = true;
            try
            {
                using (proc = new ProcedureExecute("SetHsnTaxRate"))
                {
                    proc.AddVarcharPara("@hsncode", 20, Hsncode);
                    proc.AddIntegerPara("@PurchaseCGST_TaxRatesId", PurchaseCGST_TaxRatesId);
                    proc.AddIntegerPara("@PurchaseIGST_TaxRatesId", PurchaseIGST_TaxRatesId);
                    proc.AddIntegerPara("@PurchaseSGST_TaxRatesId", PurchaseSGST_TaxRatesId);
                    proc.AddIntegerPara("@PurchaseUTGST_TaxRatesId", PurchaseUTGST_TaxRatesId);
                    proc.AddIntegerPara("@SaleCGST_TaxRatesId", SaleCGST_TaxRatesId);
                    proc.AddIntegerPara("@SaleIGST_TaxRatesId", SaleIGST_TaxRatesId);
                    proc.AddIntegerPara("@SaleSGST_TaxRatesId", SaleSGST_TaxRatesId);
                    proc.AddIntegerPara("@SaleUTGST_TaxRatesId", SaleUTGST_TaxRatesId);
                    proc.AddIntegerPara("@SacType", SacType);
                    proc.AddVarcharPara("@moduleType", 20, moduleType);
                    int i = proc.RunActionQuery();
                    
                }
            }
            catch (Exception ex)
            {
                retval = false;
            }
            finally
            {
                proc = null;
            }
            return retval;
        }
        public DataTable GetHSNTaxrates(string HSNCode)
        {
            ProcedureExecute proc;
            DataTable returntable = new DataTable();
            bool retval = true;
            try
            {
                using (proc = new ProcedureExecute("InsertHSN_SAC"))
                {
                    proc.AddVarcharPara("@Action",50, "GetHsnTaxDetails");
                    proc.AddVarcharPara("@HSNCode", 50, HSNCode);
                    returntable = proc.GetTable();
                }
            }
            catch (Exception ex)
            {
                retval = false;
            }
            finally
            {
                proc = null;
            }
            return returntable;
        }

        public DataTable GetHSNSacType(string HSNCode,String ModuleName)
        {
            ProcedureExecute proc;
            DataTable returntable = new DataTable();
            bool retval = true;
            try
            {
                using (proc = new ProcedureExecute("InsertHSN_SAC"))
                {
                    proc.AddVarcharPara("@Action", 50, "GetHsnSactype");
                    proc.AddVarcharPara("@HSNCode", 50, HSNCode);
                    proc.AddVarcharPara("@modulename", 50, ModuleName);
                    returntable = proc.GetTable();
                }
            }
            catch (Exception ex)
            {
                retval = false;
            }
            finally
            {
                proc = null;
            }
            return returntable;
        }

    }
}
