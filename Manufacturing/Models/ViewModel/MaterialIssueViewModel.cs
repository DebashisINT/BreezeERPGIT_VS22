
using Manufacturing.Models.ViewModel.BOMEntryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manufacturing.Models.ViewModel
{
    public class MaterialIssueViewModel
    {
        public Int64 ProductionIssueID { get; set; }

        public Int64 Issue_SchemaID { get; set; }

        public String Issue_No { get; set; }

        public DateTime? Issue_Date { get; set; }

        public Int64 WorkOrderID { get; set; }

        public Int64 ProductionOrderID { get; set; }

        public Int64 Details_ID { get; set; }

        public Decimal Issue_Qty { get; set; }

        public Int64 BRANCH_ID { get; set; }

        public Int64 WorkCenterID { get; set; }

        public String ProductionOrderNo { get; set; }

        public String ProductionOrderDate { get; set; }

        public DateTime? dtProductionOrderDate { get; set; }

        public String BOMNo { get; set; }

        public String RevNo { get; set; }

        public String WorkOrderNo { get; set; }

        public DateTime? WorkOrderDate { get; set; }

        public String FinishedItem { get; set; }

        public String FinishedUom { get; set; }

        public String Warehouse { get; set; }

        public String WorkCenterDescription { get; set; }

        public DateTime? BOM_Date { get; set; }

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

        public String PartNo { get; set; }
        public String PartNoName { get; set; }
        public String DesignNo { get; set; }
        public String ItemRevNo { get; set; }
        public String Description { get; set; }
        List<MaterialsFinishItemDetails> ListMaterialsFinishItemDetails { get; set; }
        public String Proj_Code { get; set; }
        public String Hierarchy { get; set; }

        public String Proj_Name { get; set; }

        public String Doctype { get; set; }
        public Int64 WorkordrWarehouseId { get; set; }
        public Int64 Proj_Id { get; set; }
        public string ClosedStatus { get; set; }
        public string TaggedStatus { get; set; }
        public String JobWorkRate { get; set; }
    }

    public class MaterialsFinishItemDetails
    {
        public String Guids { get; set; }
        public String SrlNO { get; set; }

        public String FinishItemName { get; set; }

        public String FinishItemDescription { get; set; }

        public String FinishDrawingNo { get; set; }

        public String FinishItemRevNo { get; set; }

        public String Qty { get; set; }

        public String FinishUOM { get; set; }

        public String FinishPrice { get; set; }

        public String FinishAmount { get; set; }

        public String FinishUpdateEdit { get; set; }

        public String JobWorkID { get; set; }
        public String MaterialIssueID { get; set; }
        public String FinishUOMId { get; set; }

        public String FinishProductsID { get; set; }
        public String OldFGQuantity { get; set; }

        public String MaxBalFGQuantity { get; set; }
    }
    public class udtMaterialProductionIssueDetails
    {
        public Int64 BOMProductsID { get; set; }

        public Int64 ProductsID { get; set; }

        public Decimal Qty { get; set; }

        public Decimal Amount { get; set; }
    }


    public class udt_MaterialsDetails
    {
        public Int64 DetProductId { get; set; }

        public string DetDrawingNo { get; set; }

        public string DetDrawingRevNO { get; set; }
        public Decimal DetQty { get; set; }
        public Int64 Uom { get; set; }
        public Decimal Price { get; set; }

        public Decimal Amount { get; set; }


        public string Remarks { get; set; }
    }


    public class MaterialIssueStkWarehouse
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

    public class udtMaterialStockProduct
    {
        public String Batch { get; set; }

        public String IsOutStatus { get; set; }

        public Int32 LoopID { get; set; }

        public String Product_SrlNo { get; set; }

        public String Quantity { get; set; }

        public String SalesQuantity { get; set; }

        public Int32 SrlNo { get; set; }

        public String Status { get; set; }

        public String WarehouseID { get; set; }

        public String WarehouseName { get; set; }

        public String SerialNo { get; set; }

        public String ProductID { get; set; }
    }
    public class FGFinishudtMaterialStockProduct
    {
        public String Batch { get; set; }

        public String IsOutStatus { get; set; }

        public Int32 LoopID { get; set; }

        public String Product_SrlNo { get; set; }

        public String Quantity { get; set; }

        public String SalesQuantity { get; set; }

        public Int32 SrlNo { get; set; }

        public String Status { get; set; }

        public String WarehouseID { get; set; }

        public String WarehouseName { get; set; }

        public String SerialNo { get; set; }

        public String ProductID { get; set; }
        public String MfgDate { get; set; }
        public String ExpiryDate { get; set; }
    }
}