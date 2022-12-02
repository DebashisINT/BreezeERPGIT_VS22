using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Manufacturing.Models
{
    public class ReturnMaterialIssueModel
    {

         string ConnectionString = String.Empty;
         public ReturnMaterialIssueModel()
        {
            ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }


        public DataTable GetWorkOrderData(string Action = null, Int64 WorkOrderID = 0, Int64 DetailsID = 0)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("USP_ReturnMaterialsIssueDATAGET");
            proc.AddVarcharPara("@ACTION", 100, Action);
            proc.AddPara("@WorkOrderID", WorkOrderID);
            proc.AddPara("@DetailsID", DetailsID);
            ds = proc.GetTable();
            return ds;
        }
        public DataTable GetJobWorkOrderMultipleFinishdata(string Action = null, Int64 WorkOrderID = 0)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("usp_WorkOrderDataGet");
            proc.AddVarcharPara("@ACTION", 100, Action);
            proc.AddPara("@WorkOrderID", WorkOrderID);
            // proc.AddPara("@DetailsID", DetailsID);
            ds = proc.GetTable();
            return ds;
        }
        public DataTable ClosedMateriuals(string Action, Int64 WorkOrderID, string ClosedJobRemarks)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("USP_ReturnMaterialsIssueDATAGET");
            proc.AddVarcharPara("@ACTION", 100, Action);
            proc.AddPara("@WorkOrderID", WorkOrderID);
            proc.AddVarcharPara("@ClosedRemarks", 500, ClosedJobRemarks);
            ds = proc.GetTable();
            return ds;
        }

        public DataTable GetJobWorkOrderData(string Action, Int64 WorkOrderID, DateTime WorkOrderDate)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("USP_ReturnMaterialsIssueDATAGET");
            proc.AddVarcharPara("@ACTION", 100, Action);
            proc.AddBigIntegerPara("@WorkOrderID", WorkOrderID);
            if (Convert.ToString(WorkOrderDate) != "" && Convert.ToString(WorkOrderDate) != "01-01-0001 00:00:00")
                proc.AddVarcharPara("@MatissuedtOrderDate", 10, WorkOrderDate.ToString("yyyy-MM-dd"));
            ds = proc.GetTable();
            return ds;
        }

        public DataTable GetProductionIssueData(string Action = null, Int64 ProductionIssueID = 0, Int64 DetailsID = 0, DataTable dtWarehouse = null)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("usp_mfc_ReturnMaterialIssue");
            proc.AddVarcharPara("@ACTION", 100, Action);
            proc.AddPara("@ProductionIssueID", ProductionIssueID);
            proc.AddPara("@DetailsID", DetailsID);
            if (dtWarehouse != null)
            {
                if (dtWarehouse.Rows.Count > 0)
                {
                    proc.AddPara("@UDTWAREHOUSE_DETAILS", dtWarehouse);
                }
            }
            ds = proc.GetTable();
            return ds;
        }

        public DataTable GetReturnmatIssueDeleteData(string Action = null, Int64 ProductionIssueID = 0, Int64 DetailsID = 0, DataTable dtWarehouse = null,Int64 UserId=0)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("usp_mfc_ReturnMaterialIssue");
            proc.AddVarcharPara("@ACTION", 100, Action);
            proc.AddPara("@ProductionIssueID", ProductionIssueID);
            proc.AddPara("@DetailsID", DetailsID);
            proc.AddPara("@UserId", UserId);
            if (dtWarehouse != null)
            {
                if (dtWarehouse.Rows.Count > 0)
                {
                    proc.AddPara("@UDTWAREHOUSE_DETAILS", dtWarehouse);
                }
            }
            ds = proc.GetTable();
            return ds;
        }


        public DataTable GetManufacturingProductionIssue(string Action = null, String ProductID = null, String LastFinYear = null, String Branch = null, String LastCompany = null, String multiwarehouse = null, String warehouseid = null, String BatchID = null, String Row_No = null, String SC_Date = null)
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
            ds = proc.GetTable();
            return ds;
        }
        public DataTable GetManufacturingProductionIssueWarehousedetails(string Action = null, String ProductID = null, String LastFinYear = null, String Branch = null, String LastCompany = null, String multiwarehouse = null, String warehouseid = null, String BatchID = null, String Row_No = null, String SC_Date = null, string MaterialsIssue=null)
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
            proc.AddVarcharPara("@MaterialsIssueId", 500, MaterialsIssue);
            ds = proc.GetTable();
            return ds;
        }

        public DataSet ProductionIssueBOMProductInsertUpdate(String action, Int64 ProductionIssueID, Int64 WorkOrderID, Int64 ProductionOrderID, Int64 Details_ID, Int64 WorkCenterID, String Issue_No, Int64 Issue_SchemaID, DateTime Issue_Date,
        Decimal Issue_Qty, Decimal TotalCost, Int64 BRANCH_ID, Int64 userid, String CompanyID, String FinYear, String Remarks, string PartNo, DataTable dtBOM_PRODUCTS, DataTable dtWarehouseFresh, DataTable dtWarehouse, string DocType
            , DataTable dt_PRODUCTS)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("usp_ReturnMaterialIssueInsertUpdate");
            proc.AddVarcharPara("@ACTION", 150, action);
            proc.AddPara("@ProductionIssueID", ProductionIssueID);
            proc.AddPara("@WorkOrderID", WorkOrderID);
            proc.AddPara("@ProductionOrderID", ProductionOrderID);
            proc.AddPara("@Issue_No", Issue_No);
            proc.AddPara("@Issue_SchemaID", Issue_SchemaID);
            proc.AddPara("@WorkCenterID", WorkCenterID);
            proc.AddPara("@Issue_Date", Issue_Date);
            proc.AddPara("@Issue_Qty", Issue_Qty);
            proc.AddPara("@Details_ID", Details_ID);
            proc.AddPara("@TotalCost", TotalCost);
            proc.AddPara("@BRANCH_ID", BRANCH_ID);
            proc.AddPara("@Remarks", Remarks);
            proc.AddPara("@UserID", userid);
            proc.AddPara("@CompanyID", CompanyID);
            proc.AddPara("@FinYear", FinYear);
            proc.AddPara("@PartNo", PartNo);
            proc.AddPara("@UDTPRODUCTIONORDER_DETAILS", dtBOM_PRODUCTS);
            if (dtWarehouse.Rows.Count > 0)
            {
                proc.AddPara("@UDTWAREHOUSE_DETAILS", dtWarehouse);
            }
            if (dtWarehouseFresh.Rows.Count > 0)
            {
                proc.AddPara("@UDTWAREHOUSE_DETAILSFresh", dtWarehouseFresh);
            }
            proc.AddPara("@DocType", DocType);
            proc.AddPara("@Udt_FinishItemDetails", dt_PRODUCTS);
            ds = proc.GetDataSet();
            return ds;
        }
    }
}