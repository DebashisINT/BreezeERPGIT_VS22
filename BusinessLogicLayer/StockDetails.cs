using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public class StockDetails
    {

        public DataTable GetSp_Fetch_StockDetails(int Stock_ID, int @Inventory_OwnLocationT)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Fetch_StockDetails");


            proc.AddIntegerPara("@Stock_ID", Stock_ID);
            proc.AddIntegerPara("@Inventory_OwnLocationT", Inventory_OwnLocationT);

            ds = proc.GetTable();
            return ds;
        }

        public string Insert_StockDetails(string Inventory_Company, string Inventory_FinYear, Int64 Inventory_ProductID,
            string Inventory_Brand, int Inventory_BestBeforeMonth, int Inventory_BestBeforeYear, int Inventory_Size, int Inventory_Color,
            DateTime Inventory_ExpiryDate, int Inventory_QuoteCurrency, decimal Inventory_UnitPrice, int Inventory_PriceLot, int Inventory_PriceLotUnit,
            int Inventory_QuantityUnit, decimal Inventory_QuantityIn, int Inventory_OwnLocationT, string Inventory_BatchNumber, int @Inventory_CreateUser,
            DateTime Acquire_Date)
        {
            ProcedureExecute proc;
            string rtrnvalue = "1";
            try
            {
                using (proc = new ProcedureExecute("Insert_OpeningStock_Trans"))
                {


                    proc.AddVarcharPara("@Inventory_Company", 10, Inventory_Company);
                    proc.AddVarcharPara("@mode", 10, "Insert");
                    proc.AddVarcharPara("@Inventory_FinYear", 9, Inventory_FinYear);
                    proc.AddBigIntegerPara("@Inventory_ProductID", Inventory_ProductID);
                    proc.AddVarcharPara("@Inventory_Brand", 50, Inventory_Brand);
                    proc.AddIntegerPara("@Inventory_BestBeforeMonth", Inventory_BestBeforeMonth);
                    proc.AddIntegerPara("@Inventory_BestBeforeYear", Inventory_BestBeforeYear);
                    proc.AddIntegerPara("@Inventory_Size", Inventory_Size);
                    proc.AddIntegerPara("@Inventory_Color", Inventory_Color);
                    proc.AddCharPara("@Inventory_Type", 1, 'M');
                    proc.AddDateTimePara("@Inventory_ExpiryDate", Inventory_ExpiryDate);
                    proc.AddIntegerPara("@Inventory_QuoteCurrency", Inventory_QuoteCurrency);
                    proc.AddDecimalPara("@Inventory_UnitPrice", 5, 15, Inventory_UnitPrice);
                    proc.AddIntegerPara("@Inventory_PriceLot", Inventory_PriceLot);
                    proc.AddIntegerPara("@Inventory_PriceLotUnit", Inventory_PriceLotUnit);
                    proc.AddIntegerPara("@Inventory_QuantityUnit", Inventory_QuantityUnit);
                    proc.AddDecimalPara("@Inventory_QuantityIn", 4, 20, Inventory_QuantityIn);
                    proc.AddIntegerPara("@Inventory_OwnLocationT", Inventory_OwnLocationT);
                    proc.AddVarcharPara("@Inventory_BatchNumber", 30, Inventory_BatchNumber);
                    proc.AddIntegerPara("@Inventory_CreateUser", Inventory_CreateUser);
                    proc.AddDateTimePara("@Inventory_Date", Acquire_Date);

                    int i = proc.RunActionQuery();
                    rtrnvalue = Convert.ToString(i);
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

        public int Update_StockDetails(int Inventory_ID, string Inventory_Company, string Inventory_FinYear, Int64 Inventory_ProductID,
           string Inventory_Brand, int Inventory_BestBeforeMonth, int Inventory_BestBeforeYear, int Inventory_Size, int Inventory_Color,
           DateTime Inventory_ExpiryDate, int Inventory_QuoteCurrency, decimal Inventory_UnitPrice, int Inventory_PriceLot, int Inventory_PriceLotUnit,
           int Inventory_QuantityUnit, decimal Inventory_QuantityIn, int Inventory_OwnLocationT, string Inventory_BatchNumber, int @Inventory_CreateUser,
           DateTime Acquire_Date)
        {
            ProcedureExecute proc;
            string rtrnvalue = "1";
            try
            {
                using (proc = new ProcedureExecute("Insert_OpeningStock_Trans"))
                {

                    proc.AddIntegerPara("@Inventory_ID", Inventory_ID);
                    proc.AddVarcharPara("@Inventory_Company", 10, Inventory_Company);
                    proc.AddVarcharPara("@mode", 10, "Update");
                    proc.AddVarcharPara("@Inventory_FinYear", 9, Inventory_FinYear);
                    proc.AddBigIntegerPara("@Inventory_ProductID", Inventory_ProductID);
                    proc.AddVarcharPara("@Inventory_Brand", 50, Inventory_Brand);
                    proc.AddIntegerPara("@Inventory_BestBeforeMonth", Inventory_BestBeforeMonth);
                    proc.AddIntegerPara("@Inventory_BestBeforeYear", Inventory_BestBeforeYear);
                    proc.AddIntegerPara("@Inventory_Size", Inventory_Size);
                    proc.AddIntegerPara("@Inventory_Color", Inventory_Color);
                    proc.AddCharPara("@Inventory_Type", 1, 'M');
                    proc.AddDateTimePara("@Inventory_ExpiryDate", Inventory_ExpiryDate);
                    proc.AddIntegerPara("@Inventory_QuoteCurrency", Inventory_QuoteCurrency);
                    proc.AddDecimalPara("@Inventory_UnitPrice", 5, 15, Inventory_UnitPrice);
                    proc.AddIntegerPara("@Inventory_PriceLot", Inventory_PriceLot);
                    proc.AddIntegerPara("@Inventory_PriceLotUnit", Inventory_PriceLotUnit);
                    proc.AddIntegerPara("@Inventory_QuantityUnit", Inventory_QuantityUnit);
                    proc.AddDecimalPara("@Inventory_QuantityIn", 4, 20, Inventory_QuantityIn);
                    proc.AddIntegerPara("@Inventory_OwnLocationT", Inventory_OwnLocationT);
                    proc.AddVarcharPara("@Inventory_BatchNumber", 30, Inventory_BatchNumber);
                    proc.AddIntegerPara("@Inventory_CreateUser", Inventory_CreateUser);
                    proc.AddDateTimePara("@Inventory_Date", Acquire_Date);

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

        public string Delete_StockDetails(int Inventory_ID)
        {
            ProcedureExecute proc;
            string rtrnvalue = "1";
            try
            {
                using (proc = new ProcedureExecute("Insert_OpeningStock_Trans"))
                {
                    proc.AddIntegerPara("@Inventory_ID", Inventory_ID);
                    proc.AddVarcharPara("@mode", 10, "Delete");
                    int i = proc.RunActionQuery();

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

        public DataTable GetAllProduct()
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("AddNewProduct");
            proc.AddNVarcharPara("@Module", 100, "GetAllProducts");         
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetPreferredProduct(string IndustryID)
        {

            DataTable dt = new DataTable();
            if(!string.IsNullOrEmpty(IndustryID))
            { 
            ProcedureExecute proc = new ProcedureExecute("AddNewProduct");
            proc.AddNVarcharPara("@Module", 100, "GetEmployeePrefereedProducts");
            proc.AddIntegerPara("@IndustryID", Convert.ToInt32(IndustryID));
            dt = proc.GetTable();
            }
            return dt;
        }
        public DataTable GetPreferredProductClassGroup(string IndustryID)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("AddNewProduct");
            proc.AddNVarcharPara("@Module", 100, "GetEmployeePrefereedProductsClassGroup");
            proc.AddIntegerPara("@IndustryID", Convert.ToInt32(IndustryID));
            dt = proc.GetTable();
            return dt;
        }


        public DataTable GetEmployeePrefereedProductsByProductClass(string IndustryID, string ProductClassIds)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("AddNewProduct");
            proc.AddNVarcharPara("@Module", 100, "GetEmployeePrefereedProductsByProductClass");
            proc.AddIntegerPara("@IndustryID", Convert.ToInt32(IndustryID));
            proc.AddNVarcharPara("@ProductClassIds", 250, ProductClassIds);
            dt = proc.GetTable();
            return dt;
        }
    }
}
