using DataAccessLayer;
using Manufacturing.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Manufacturing.Models
{
    public class StockReturnModel
    {
        string ConnectionString = String.Empty;
        public StockReturnModel()
        {
            ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }

        public DataTable GetStockReturnData(string Action = null, Int64 ID = 0, Int64 DetailsID = 0)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("usp_StockReturnDataGet");
            proc.AddVarcharPara("@ACTION", 100, Action);
            proc.AddPara("@ID", ID);
            proc.AddPara("@DetailsID", DetailsID);
            ds = proc.GetTable();
            return ds;
        }

        public DataTable StockReturnInsertUpdate(string Action = null, StockReturnViewModel obj = null, String LastCompany = null, String LastFinYear = null, DataTable Warehouse = null, DataTable WarehouseFresh = null, DataTable dtWarehouseWC = null)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("usp_StockReturnInsertUpdate");
            proc.AddVarcharPara("@ACTION", 100, Action);
            proc.AddPara("@StockReturnID", obj.StockReturnID);
            proc.AddPara("@StockReceiptID", obj.StockReceiptID);
            // proc.AddPara("@Product_ID", obj.ProductID);
            proc.AddPara("@SchemaID", obj.ReturnSchemaID);
            proc.AddPara("@Return_No", obj.StockReturn_No);
            proc.AddPara("@Return_Date", Convert.ToDateTime(obj.StockReturnDate));

            proc.AddPara("@Fresh_ReturnQty", obj.FreshQuantity);
            proc.AddPara("@Rejected_ReturnQty", obj.RejectedQuantity);
            proc.AddPara("@UserID", obj.UserID);

            proc.AddPara("@Product_ID", obj.ProductID);
            proc.AddPara("@CompanyID", LastCompany);
            proc.AddPara("@FinYear", LastFinYear);
            proc.AddPara("@DetailsID", obj.Details_ID);
            if (Warehouse != null)
            {
                if (Warehouse.Rows.Count > 0)
                {
                    proc.AddPara("@UDTWAREHOUSE_DETAILS", Warehouse);
                }
            }

            if (WarehouseFresh != null)
            {
                if (WarehouseFresh.Rows.Count > 0)
                {
                    proc.AddPara("@UDTWAREHOUSE_DETAILSFresh", WarehouseFresh);
                }
            }

            if (dtWarehouseWC != null)
            {
                if (dtWarehouseWC.Rows.Count > 0)
                {
                    proc.AddPara("@UDTWAREHOUSE_DETAILSWC", dtWarehouseWC);
                }
            }
           
            

            ds = proc.GetTable();
            return ds;
        }
    }
}