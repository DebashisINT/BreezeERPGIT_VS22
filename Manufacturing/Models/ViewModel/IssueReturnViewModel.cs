using Manufacturing.Models.ViewModel.BOMEntryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manufacturing.Models.ViewModel
{
    public class IssueReturnViewModel
    {

        public Int64 IssueReturnID { get; set; }
        public Int64 ProductionIssueID { get; set; }

        public Int64 Issue_SchemaID { get; set; }
        public String OrderDate { get; set; }
        public String Issue_No { get; set; }

        public DateTime Issue_Date { get; set; }

        public Int64 WorkOrderID { get; set; }

        public Int64 ProductionOrderID { get; set; }

        public Int64 Details_ID { get; set; }

        public Decimal Issue_Qty { get; set; }

        public Int64 BRANCH_ID { get; set; }

        public Int64 WorkCenterID { get; set; }

        public String ProductionOrderNo { get; set; }

        public String ProductionOrderDate { get; set; }

        public DateTime dtProductionOrderDate { get; set; }

        public String BOMNo { get; set; }

        public String RevNo { get; set; }

        public String WorkOrderNo { get; set; }

        public DateTime? WorkOrderDate { get; set; }

        public String FinishedItem { get; set; }

        public String FinishedUom { get; set; }

        public String Warehouse { get; set; }

        public String WorkCenterDescription { get; set; }

        public DateTime BOM_Date { get; set; }

        public Decimal BalQty { get; set; }

        public Int64 Unit { get; set; }

        public String WorkCenterCode { get; set; }

        public DateTime? REV_Date { get; set; }


        public List<BranchUnit> UnitList { get; set; }

        public Decimal TotalCost { get; set; }


        public String CreatedBy { get; set; }

        public String ModifyBy { get; set; }


        public DateTime CreateDate { get; set; }

        public DateTime? ModifyDate { get; set; }

        public String OrderNo { get; set; }

        public Decimal Finished_Qty { get; set; }

        public String strRemarks { get; set; }

        public Decimal MaxQty { get; set; }

        public String ProductionIssueNo { get; set; }

        public DateTime ProductionIssueDate { get; set; }

        public String txtProductionIssueDate { get; set; }

        public String PartNo { get; set; }
        public String PartNoName { get; set; }
        public String DesignNo { get; set; }
        public String ItemRevNo { get; set; }
        public String Description { get; set; }
        public String Proj_Code { get; set; }
        public String Hierarchy { get; set; }

        public String Proj_Name { get; set; }

        // Rev Sanchita
        public Int64 ProjectID { get; set; }
        // End of Rev Sanchita
    }


    public class FGIssueReturnViewModel
    {

        public Int64 IssueReturnID { get; set; }
        public Int64 ProductionIssueID { get; set; }

        public Int64 Issue_SchemaID { get; set; }
        public String OrderDate { get; set; }
        public String Issue_No { get; set; }

        public DateTime Issue_Date { get; set; }

        public Int64 WorkOrderID { get; set; }

        public Int64 ProductionOrderID { get; set; }

        public Int64 Details_ID { get; set; }

        public Decimal Issue_Qty { get; set; }

        public Int64 BRANCH_ID { get; set; }

        public Int64 WorkCenterID { get; set; }

        public String ProductionOrderNo { get; set; }

        public String ProductionOrderDate { get; set; }

        public DateTime dtProductionOrderDate { get; set; }

        public String BOMNo { get; set; }

        public String RevNo { get; set; }

        public String WorkOrderNo { get; set; }

        public DateTime WorkOrderDate { get; set; }

        public String FinishedItem { get; set; }

        public String FinishedUom { get; set; }

        public String Warehouse { get; set; }
        public Int64 HeaderWarehouseId { get; set; }
        public Int64 WarehouseID { get; set; }
        public String WorkCenterDescription { get; set; }

        public DateTime BOM_Date { get; set; }

        public Decimal BalQty { get; set; }

        public Int64 Unit { get; set; }

        public String WorkCenterCode { get; set; }

        public DateTime? REV_Date { get; set; }


        public List<BranchUnit> UnitList { get; set; }

        public Decimal TotalCost { get; set; }


        public String CreatedBy { get; set; }

        public String ModifyBy { get; set; }


        public DateTime CreateDate { get; set; }

        public DateTime? ModifyDate { get; set; }

        public String OrderNo { get; set; }

        public Decimal Finished_Qty { get; set; }

        public String strRemarks { get; set; }

        public Decimal MaxQty { get; set; }

        public String ProductionIssueNo { get; set; }

        public DateTime ProductionIssueDate { get; set; }

        public String txtProductionIssueDate { get; set; }

        public String PartNo { get; set; }
        public String PartNoName { get; set; }
        public String DesignNo { get; set; }
        public String ItemRevNo { get; set; }
        public String Description { get; set; }
        public String Proj_Code { get; set; }
        public String Hierarchy { get; set; }

        public String Proj_Name { get; set; }

        // Rev Sanchita
        public Int64 ProjectID { get; set; }
        // End of Rev Sanchita
    }


    public class udtIssueReturnDetails
    {
        public Int64 BOMProductsID { get; set; }

        public Int64 ProductsID { get; set; }

        public Decimal Qty { get; set; }

        public Decimal Amount { get; set; }
    }

    public class IssueReturnStkWarehouse
    {
        public Int64 StkWarehouseID { get; set; }

        public Int64 ProductionIssueID { get; set; }

        public Int64 ProductionIssueDetailsID { get; set; }

        public String Batch { get; set; }

        public String IsOutStatus { get; set; }

        public String LoopID { get; set; }

        public String Product_SrlNo { get; set; }

        public String Quantity { get; set; }

        public String SalesQuantity { get; set; }

        public String SrlNo { get; set; }

        public String Status { get; set; }

        public String WarehouseID { get; set; }

        public String WarehouseName { get; set; }

        public String SerialNo { get; set; }

        public String ProductID { get; set; }

    }

   
}