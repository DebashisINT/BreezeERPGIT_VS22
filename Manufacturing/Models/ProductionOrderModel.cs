using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Manufacturing.Models
{
    public class ProductionOrderModel
    {
        string ConnectionString = String.Empty;
        public ProductionOrderModel()
        {
            ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }

        public DataTable GetProductionOrderData(string Action, Int64 productionorderid = 0, Int64 DetailsID = 0, String ClosedPORemarks = "")
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("usp_ProductionOrderDataGet");
            proc.AddVarcharPara("@ACTION", 100, Action);
            proc.AddPara("@ProductionOrderID", productionorderid);
            proc.AddPara("@DetailsID", DetailsID);
            proc.AddPara("@ClosedRemarks", ClosedPORemarks);
            ds = proc.GetTable();
            return ds;
        }
        
        public DataSet ProductionBOMProductInsertUpdate(String action, Int64 ProductionOrderID, Int64 Production_ID, Int64 Details_ID, String OrderNo, Int64 Order_SchemaID, DateTime OrderDate, Int64 BRANCH_ID, Int64 WarehouseID,
        Decimal Order_Qty, Decimal ActualAdditionalCost, Decimal TotalCost, Int64 userid, String Remarks, string PartNo, string TotalResourceCost, DataTable dtBOM_PRODUCTS)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("usp_ProductionOrderInsertUpdate");
            proc.AddVarcharPara("@ACTION", 150, action);
            proc.AddPara("@ProductionOrderID", ProductionOrderID);
            proc.AddPara("@Production_ID", Production_ID);
            proc.AddPara("@Details_ID", Details_ID);
            proc.AddPara("@OrderNo",  OrderNo);
            proc.AddPara("@Order_SchemaID",  Order_SchemaID);
            proc.AddPara("@OrderDate",  OrderDate);
            proc.AddPara("@BRANCH_ID",  BRANCH_ID);
            proc.AddPara("@WarehouseID", WarehouseID);
            proc.AddPara("@Order_Qty", Order_Qty);
            proc.AddPara("@ActualAdditionalCost", ActualAdditionalCost);
            proc.AddPara("@TotalCost", TotalCost);
            proc.AddPara("@UserID", userid);
            proc.AddPara("@Remarks", Remarks);
            proc.AddPara("@PartNo", PartNo);
            proc.AddPara("@UDTPRODUCTIONORDER_DETAILS", dtBOM_PRODUCTS);
            proc.AddPara("@TotalResourceCost", TotalResourceCost);
            ds = proc.GetDataSet();
            return ds;
        }
    }
}