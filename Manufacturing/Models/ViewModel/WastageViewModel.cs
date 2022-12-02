using Manufacturing.Models.ViewModel.BOMEntryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manufacturing.Models.ViewModel
{
    public class WastageViewModel
    {
        public Int64 WastageID { get; set; }

        public Int64 WastageWarehouseID { get; set; }

        public Decimal WarehouseQty { get; set; }

        public Int64 WastageSchemaID { get; set; }

        public String WastageDate { get; set; }

        public DateTime dtWastageDate { get; set; }

        public String Wastage_No { get; set; }

        public String StockReceipt_Date { get; set; }

        public String StockReturnDate { get; set; }

        public DateTime dtStockReturnDate { get; set; }

        public String StockReturn_No { get; set; }

        public Int64 ReturnSchemaID { get; set; }

        public Int64 StockReturnID { get; set; }

        public Decimal Fresh_ReceiptQty { get; set; }

        public Decimal Rejected_ReceiptQty { get; set; }

        public String ProductionReceiptNo { get; set; }

        public DateTime ProductionReceiptDate { get; set; }

        public Int64 ReceiptSchemaID { get; set; }

        public Int64 StockReceiptID { get; set; }

        public String StockReceipt_No { get; set; }

        public Int64 QualityControlID { get; set; }

        public String Product_NegativeStock { get; set; }

        public DateTime dtStockReceiptDate { get; set; }

        //public String StockReceiptDate { get; set; }

        public String QC_No { get; set; }

        public Decimal AvlStk { get; set; }

        public Int64 ProductID { get; set; }

        public String InventoryType { get; set; }

        public String ReceiptDate { get; set; }

        public Int64 ProductionReceiptID { get; set; }

        public Int64 ProductionIssueID { get; set; }

        public String ProductionIssueNo { get; set; }

        public Int64 WorkOrderID { get; set; }

        public String Receipt_No { get; set; }

        public DateTime Receipt_Date { get; set; }

        public DateTime ProductionIssueDate { get; set; }

        public Int64 Production_ID { get; set; }

        public Int64 Details_ID { get; set; }

        public String WorkOrderNo { get; set; }

        public String BOMNo { get; set; }

        public String RevNo { get; set; }

        public String FinishedItem { get; set; }

        public Int64 Order_SchemaID { get; set; }

        public Int64 QC_SchemaID { get; set; }

        public String OrderDate { get; set; }

        public DateTime dtOrderDate { get; set; }

        public Int64 BRANCH_ID { get; set; }

        public Int64 WarehouseID { get; set; }

        public Decimal FGQty { get; set; }

        public String FinishedUom { get; set; }

        public Decimal Finished_Qty { get; set; }

        public Decimal ActualAdditionalCost { get; set; }

        public Decimal ActualComponentCost { get; set; }

        public Decimal ActualProductCost { get; set; }

        public Decimal ProductionOrderQty { get; set; }

        public Decimal FGReceiptQty { get; set; }

        public Decimal TotalCost { get; set; }

        public Int64 UserID { get; set; }

        public String Unit { get; set; }

        public String Warehouse { get; set; }


        public DateTime BOM_Date { get; set; }

        public Decimal ProductionIssueQty { get; set; }

        public Decimal FGPrice { get; set; }

        public Decimal TotalAmount { get; set; }

        public String ProductDescription { get; set; }

        public DateTime? REV_Date { get; set; }

        public String WorkCenterID { get; set; }

        public String OrderNo { get; set; }

        public DateTime WorkOrderDate { get; set; }

        public List<BranchUnit> UnitList { get; set; }

        public String StockReceiptDate { get; set; }

        public String WorkCenterCode { get; set; }

        public String WorkCenterDescription { get; set; }

        public String ProductionOrderNo { get; set; }

        public String FinishedItemDescription { get; set; }

        public Decimal FreshQuantity { get; set; }

        public Decimal RejectedQuantity { get; set; }

        public Decimal BalFreshQuantity { get; set; }

        public Decimal BalRejectedQuantity { get; set; }

        public DateTime ProductionOrderDate { get; set; }

        public Int64 ProductionOrderID { get; set; }

        public String CreatedBy { get; set; }

        public String ModifyBy { get; set; }


        public DateTime CreateDate { get; set; }

        public DateTime? ModifyDate { get; set; }

        public String strRemarks { get; set; }

        public String IssueDate { get; set; }

        public String PartNoName { get; set; }
        public String DesignNo { get; set; }
        public String ItemRevNo { get; set; }

        public String Proj_Code { get; set; }
        public String Hierarchy { get; set; }

        public String Proj_Name { get; set; }
    }


    public class WastageStkWarehouse
    {
        public Int64 StkWarehouseID { get; set; }

        public Int64 WastageID { get; set; }

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