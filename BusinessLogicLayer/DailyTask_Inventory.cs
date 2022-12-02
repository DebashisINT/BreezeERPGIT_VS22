using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public class DailyTask_Inventory
    {
        public DataTable GetStockTable(int StockPositionId)
        {
            DataTable dt = new DataTable();
            try
            {
                ProcedureExecute proc = new ProcedureExecute("Select_StockTable");
                proc.AddIntegerPara("@StockPositionId", StockPositionId);
                dt = proc.GetTable();
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int InsertTransaction(string mode, int InventoryId, string Company, string Inventory_FinYear, string OrderType, string Date, string OrderNo, string OrderDate, 
            string CustomerorVendor, long ProductID, string Brand, int BestBeforeMonth, int BestBeforeYear, int Size, int Color, string ExpiryDate, 
            int Currency, decimal UnitPrice, int PriceLot, int PriceLotUnit, decimal QuantityIn, decimal QuantityOut, int QuantityUnit, string BatchNo, 
            string ManufactureDate, string ProductDescription, string DeliveryReferance, string Remarks,int Inventory_OwnLocationS, int Inventory_OwnLocationT,
            int Inventory_ContactLocationS, int Inventory_ContactLocationT, string Inventory_ContacOthertLocationS, 
            string Inventory_ContactOtherLocationT, int CreateUser,string RecvDate,string PieceNo)
        {
            ProcedureExecute proc;
            int rtrnvalue = 0;
            using (proc = new ProcedureExecute("Insert_OpeningStock_Trans"))
            {
                proc.AddVarcharPara("@mode", 50, mode);
                proc.AddIntegerPara("@Inventory_ID", InventoryId);
                proc.AddVarcharPara("@Inventory_Company", 50, Company);
                proc.AddIntegerPara("@Inventory_Branch",1);
                proc.AddVarcharPara("@Inventory_FinYear", 9, Inventory_FinYear);
                proc.AddCharPara("@Inventory_Type", 1, Convert.ToChar(OrderType));
                proc.AddVarcharPara("@Inventory_Date", 20, Date);
                proc.AddVarcharPara("@Inventory_OrderNumber", 50, OrderNo);
                proc.AddVarcharPara("@Inventory_OrderDate", 50, OrderDate);
                proc.AddVarcharPara("@Inventory_ContactID", 10, CustomerorVendor);
                proc.AddBigIntegerPara("@Inventory_ProductID", ProductID);
                proc.AddVarcharPara("@Inventory_Brand", 50, Brand);
                proc.AddIntegerPara("@Inventory_BestBeforeMonth", BestBeforeMonth);
                proc.AddIntegerPara("@Inventory_BestBeforeYear", BestBeforeYear);
                proc.AddIntegerPara("@Inventory_Size", Size);
                proc.AddIntegerPara("@Inventory_Color", Color);
                proc.AddVarcharPara("@Inventory_ExpiryDate", 50, ExpiryDate);
                proc.AddIntegerPara("@Inventory_QuoteCurrency", Currency);
                proc.AddVarcharPara("@Inventory_UnitPrice", 50, Convert.ToString(UnitPrice));
                proc.AddIntegerPara("@Inventory_PriceLot", PriceLot);
                proc.AddIntegerPara("@Inventory_PriceLotUnit", PriceLotUnit);
                proc.AddVarcharPara("@Inventory_QuantityIn",50, Convert.ToString(QuantityIn));
                proc.AddVarcharPara("@Inventory_QuantityOut", 50, Convert.ToString(QuantityOut));
                proc.AddIntegerPara("@Inventory_QuantityUnit", QuantityUnit);
                proc.AddVarcharPara("@Inventory_BatchNumber", 30, BatchNo);
                proc.AddVarcharPara("@Inventory_Mdate", 50, ManufactureDate);
                proc.AddVarcharPara("@Inventory_ProductDescription", 200, ProductDescription);
                proc.AddVarcharPara("@Inventory_DeliveryReference", 200, DeliveryReferance);
                proc.AddVarcharPara("@Inventory_Remarks", 200, Remarks);
                proc.AddIntegerPara("@Inventory_OwnLocationS", Inventory_OwnLocationS);
                proc.AddIntegerPara("@Inventory_OwnLocationT", Inventory_OwnLocationT);
                proc.AddIntegerPara("@Inventory_ContactLocationS", Inventory_ContactLocationS);
                proc.AddIntegerPara("@Inventory_ContactLocationT", Inventory_ContactLocationT);
                proc.AddVarcharPara("@Inventory_ContacOthertLocationS", -1, Inventory_ContacOthertLocationS);
                proc.AddVarcharPara("@Inventory_ContactOtherLocationT", -1, Inventory_ContactOtherLocationT);
                proc.AddIntegerPara("@Inventory_CreateUser", CreateUser);
                proc.AddDateTimePara("@Inventory_CreateTime", System.DateTime.Now);
                proc.AddDateTimePara("@Inventory_Received_Date", Convert.ToDateTime(RecvDate));
                proc.AddVarcharPara("@PieceNo", 50, PieceNo);
                

                rtrnvalue = proc.RunActionQuery();
            }
            return rtrnvalue;
        }   

        public DataTable FetchInventoryTransaction(int id)
        {
            DataTable dt = new DataTable();
            try
            {
                ProcedureExecute proc = new ProcedureExecute("SP_Select_Inventory_Transaction");
                proc.AddIntegerPara("@PositionId", id);
                dt = proc.GetTable();
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int DeleteTransaction(string mode, int InventoryId)
        {
            ProcedureExecute proc;
            int rtrnvalue = 0;
            using (proc = new ProcedureExecute("Insert_OpeningStock_Trans"))
            {
                proc.AddVarcharPara("@mode", 50, mode);
                proc.AddIntegerPara("@Inventory_ID", InventoryId);
                rtrnvalue = proc.RunActionQuery();
            }
            return rtrnvalue;
        }
        public DataTable GetInvTransactionPrintData(string FromDate, string ToDate, string Mode)
        {
            DataTable dt = new DataTable();
            try
            {
                ProcedureExecute proc = new ProcedureExecute("sp_InvTransactionPrintTable");
                proc.AddVarcharPara("@Mode", 20, Mode);
                proc.AddVarcharPara("@FromDate",100, FromDate);
                proc.AddVarcharPara("@ToDate", 100, @ToDate);
                dt = proc.GetTable();
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetInvTransactionPrintData(int InventoryId, string Mode, string IsContactnamePrint)
        {
            DataTable dt = new DataTable();
            try
            {
                ProcedureExecute proc = new ProcedureExecute("sp_InvTransactionPrintTable");
                proc.AddVarcharPara("@Mode", 20, Mode);
                proc.AddIntegerPara("@StockPositionId", InventoryId);
                proc.AddVarcharPara("@IsContactnamePrint", 100, IsContactnamePrint);
                dt = proc.GetTable();
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}
