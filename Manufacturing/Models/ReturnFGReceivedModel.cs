using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Manufacturing.Models
{
    public class ReturnFGReceivedModel
    {
    
         string ConnectionString = String.Empty;
         public ReturnFGReceivedModel()
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

        public DataTable GetManufacturingFGReturn(string Action = null, String ProductID = null, String LastFinYear = null, String Branch = null, String LastCompany = null, String multiwarehouse = null, String warehouseid = null, String BatchID = null, String Row_No = null, String SC_Date = null, string ProductionIssueID=null)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_ManufacturingProductionIssue_Get");
            proc.AddVarcharPara("@Action", 500, Action);
            proc.AddVarcharPara("@ProductID", 500, ProductID);
            proc.AddVarcharPara("@FinYear", 500, LastFinYear);
            proc.AddVarcharPara("@branchId", 2000, Branch);
            proc.AddVarcharPara("@companyId", 500, LastCompany);
            proc.AddVarcharPara("@Multiwarehouse", 500, multiwarehouse);
            proc.AddVarcharPara("@WarehouseID", 500, warehouseid);
            proc.AddVarcharPara("@BatchID", 10, BatchID);
            proc.AddVarcharPara("@Row_No", 100, Row_No);
            proc.AddVarcharPara("@SC_Date", 10, SC_Date);
            proc.AddVarcharPara("@ProductionIssueID", 100, ProductionIssueID);
            ds = proc.GetTable();
            return ds;
        }
        public DataTable GetREturnFGReceiptDeleteData(string Action = null, Int64 ProductionReceiptID = 0, Int64 DetailsID = 0,Int64 UserId=0)
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

        public DataTable GetProductionReceiptFinishData(string Action = null, Int64 ProductionReceiptID = 0)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("usp_FGReceivedDataGet");
            proc.AddVarcharPara("@ACTION", 100, Action);
            proc.AddPara("@ProductionReceiptID", ProductionReceiptID);
            ds = proc.GetTable();
            return ds;
        }
        public DataTable GetManufacturingProductionIssueWarehousedetailsForFGReturn(string Action = null, String ProductID = null, String LastFinYear = null, String Branch = null, String LastCompany = null, String multiwarehouse = null, String warehouseid = null, String BatchID = null, String Row_No = null, String SC_Date = null, string ProductionIssueID = null)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_ManufacturingProductionIssue_Get");
            proc.AddVarcharPara("@Action", 500, Action);
            proc.AddVarcharPara("@ProductID", 500, ProductID);
            proc.AddVarcharPara("@FinYear", 500, LastFinYear);
            proc.AddVarcharPara("@branchId", 2000, Branch);
            proc.AddVarcharPara("@companyId", 500, LastCompany);
            proc.AddVarcharPara("@Multiwarehouse", 500, multiwarehouse);
            proc.AddVarcharPara("@WarehouseID", 500, warehouseid);
            proc.AddVarcharPara("@BatchID", 10, BatchID);
            proc.AddVarcharPara("@Row_No", 100, Row_No);
            proc.AddVarcharPara("@SC_Date", 10, SC_Date);
            proc.AddVarcharPara("@MaterialsIssueId", 500, ProductionIssueID);
            ds = proc.GetTable();
            return ds;
        }
        
        // Rev Sanchita [ parameter ProjectID added]
        public DataSet ProductionReceiptBOMProductInsertUpdate(String action, Int64 ProductionReceiptID, Int64 ProductionIssueID, Int64 ProductionOrderID, Int64 Details_ID, Int64 WorkOrderID, Int64 Production_ID, Int64 WorkCenterID, String OrderNo, Int64 Order_SchemaID, DateTime OrderDate,
        Decimal Order_Qty, Decimal ActualAdditionalCost, Decimal TotalCost, Int64 BRANCH_ID, Int64 userid, String Remarks, Decimal FGPrice, Decimal TotalAmount,
            DataTable dtBOM_PRODUCTS, Int64 WarehouseId, String CompanyID, String FinYear, DataTable dt_PRODUCTS, DataTable dtWarehouse, DataTable dtFinishWarehouse,
            Int64 ProjectID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("usp_ReturnFGRecInsertUpdate");
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
            proc.AddPara("@UDTPRODUCTIONORDER_DETAILS", dtBOM_PRODUCTS);
            // Rev Sanchita
            proc.AddPara("@ProjectID", ProjectID);
            // End of Rev Sanchita
            if (dt_PRODUCTS != null && dt_PRODUCTS.Rows.Count>0)
            {
                proc.AddPara("@Udt_FinishItemDetails", dt_PRODUCTS);
            }
            if (dtWarehouse != null && dtWarehouse.Rows.Count > 0)
            {
                proc.AddPara("@UDTWAREHOUSE_DETAILS", dtWarehouse);
            }
            if (dtFinishWarehouse != null && dtFinishWarehouse.Rows.Count > 0)
            {
                proc.AddPara("@UDTFiNIshWAREHOUSE_DETAILS", dtFinishWarehouse);
            }
            proc.AddPara("@CompanyID", CompanyID);
            proc.AddPara("@FinYear", FinYear);
            ds = proc.GetDataSet();
            return ds;
        }
    }
}