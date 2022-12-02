using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Manufacturing.Models
{
    public class FGReceivedModel
    {

         string ConnectionString = String.Empty;
         public FGReceivedModel()
        {
            ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }

        public DataTable GetProductionReceiptData(string Action = null, Int64 ProductionReceiptID = 0, Int64 DetailsID = 0)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("usp_FGReceivedDataGet");
            proc.AddVarcharPara("@ACTION", 100, Action);
            proc.AddPara("@ProductionReceiptID", ProductionReceiptID);
            proc.AddPara("@DetailsID", DetailsID);
            ds = proc.GetTable();
            return ds;
        }

        public DataTable GetProductionReceiptFinishData(string Action = null, Int64 ProductionReceiptID = 0)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("usp_FGReceivedDataGet");
            proc.AddVarcharPara("@ACTION", 100, Action);
            proc.AddPara("@ProductionReceiptID", ProductionReceiptID);
            ds = proc.GetTable();
            return ds;
        }

        public DataTable GetFGReceiptDatadelete(string Action = null, Int64 ProductionReceiptID = 0, Int64 DetailsID = 0,Int64 UserId=0 )
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("usp_FGReceivedDataGet");
            proc.AddVarcharPara("@ACTION", 100, Action);
            proc.AddPara("@ProductionReceiptID", ProductionReceiptID);
            proc.AddPara("@DetailsID", DetailsID);
            proc.AddPara("@UserId", UserId);
            ds = proc.GetTable();
            return ds;
        }

        // Revc Sanchita [ Int64 Proj_Code added]
        public DataSet ProductionReceiptBOMProductInsertUpdate(String action, Int64 ProductionReceiptID, Int64 ProductionIssueID, Int64 ProductionOrderID, Int64 Details_ID, Int64 WorkOrderID, Int64 Production_ID, Int64 WorkCenterID, String OrderNo, Int64 Order_SchemaID, DateTime OrderDate,
        string Order_Qty, Decimal ActualAdditionalCost, Decimal TotalCost, Int64 BRANCH_ID, Int64 userid, String Remarks, Decimal FGPrice, Decimal TotalAmount, DataTable dtBOM_PRODUCTS, Int64 WarehouseId, String CompanyID, String FinYear,
            DataTable dt_PRODUCTS, DataTable dtWarehouse, DataTable FinishWarehouseDetails, Decimal AddlPrice, Decimal ConsumePrice, Int64 ProjectID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("usp_FGReceivedInsertUpdate");
            proc.AddVarcharPara("@ACTION", 150, action);
            proc.AddPara("@ProductionReceiptID", ProductionReceiptID);
            proc.AddPara("@ProductionIssueID", ProductionIssueID);
            proc.AddPara("@ProductionOrderID", ProductionOrderID);
            proc.AddPara("@Details_ID", Details_ID);
            proc.AddPara("@WorkOrderID", WorkOrderID);
            proc.AddPara("@Production_ID", Production_ID);
            proc.AddPara("@OrderNo", OrderNo);
            proc.AddPara("@Order_SchemaID", Order_SchemaID);
            proc.AddPara("@WorkCenterID", WorkCenterID);
            proc.AddPara("@OrderDate", OrderDate);
            proc.AddPara("@Order_Qty", Order_Qty);
            proc.AddPara("@ActualAdditionalCost", ActualAdditionalCost);
            proc.AddPara("@TotalCost", TotalCost);
            proc.AddPara("@Remarks", Remarks);
            proc.AddPara("@BRANCH_ID", BRANCH_ID);
            proc.AddPara("@UserID", userid);
            proc.AddPara("@FGPrice", FGPrice);
            proc.AddPara("@TotalAmount", TotalAmount);
            proc.AddPara("@WarehouseId", WarehouseId);
            // Rev Sanchita 
            proc.AddPara("@ProjectID", ProjectID);
            // End of Rev Sanchita
            if (FinishWarehouseDetails != null && FinishWarehouseDetails.Rows.Count > 0)
            {
                proc.AddPara("@UDTFiNIshWAREHOUSE_DETAILS", FinishWarehouseDetails);
            }
            proc.AddPara("@Udt_FinishItemDetails", dt_PRODUCTS);
            proc.AddPara("@UDTPRODUCTIONORDER_DETAILS", dtBOM_PRODUCTS);
            if (dtWarehouse != null && dtWarehouse.Rows.Count > 0)
            {
                proc.AddPara("@UDTWAREHOUSE_DETAILS", dtWarehouse);
            }
            proc.AddPara("@CompanyID", CompanyID);
            proc.AddPara("@AddlPrice", AddlPrice);
            proc.AddPara("@ConsumePrice", ConsumePrice);
            proc.AddPara("@FinYear", FinYear);
            ds = proc.GetDataSet();
            return ds;
        }
    }
}