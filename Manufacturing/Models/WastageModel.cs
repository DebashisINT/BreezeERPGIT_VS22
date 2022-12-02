using DataAccessLayer;
using Manufacturing.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Manufacturing.Models
{
    public class WastageModel
    {
        string ConnectionString = String.Empty;
        public WastageModel()
        {
            ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }

        public DataTable GetWastageData(string Action = null, Int64 ID = 0, Int64 DetailsID = 0)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("usp_WastageDataGet");
            proc.AddVarcharPara("@ACTION", 100, Action);
            proc.AddPara("@ID", ID);
            proc.AddPara("@DetailsID", DetailsID);
            ds = proc.GetTable();
            return ds;
        }

        public DataTable WastageInsertUpdate(string Action = null, WastageViewModel obj = null, String LastCompany = null, String LastFinYear = null, DataTable Warehouse = null, DataTable WarehouseFresh = null)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("usp_WastageInsertUpdate");
            proc.AddVarcharPara("@ACTION", 100, Action);
            proc.AddPara("@WastageID", obj.WastageID);
            proc.AddPara("@StockReceiptID", obj.StockReceiptID);
            proc.AddPara("@WastageWarehouseID", obj.WastageWarehouseID);
            proc.AddPara("@SchemaID", obj.WastageSchemaID);
            proc.AddPara("@Wastage_No", obj.Wastage_No);
            proc.AddPara("@Wastage_Date", obj.WastageDate);

            proc.AddPara("@WastageQty", obj.WarehouseQty);
            proc.AddPara("@UserID", obj.UserID);

            proc.AddPara("@Product_ID", obj.ProductID);
            proc.AddPara("@CompanyID", LastCompany);
            proc.AddPara("@FinYear", LastFinYear);
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

            ds = proc.GetTable();
            return ds;
        }

    }
}