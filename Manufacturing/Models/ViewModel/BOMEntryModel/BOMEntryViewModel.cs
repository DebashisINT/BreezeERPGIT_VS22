//@*==================================================== Revision History =========================================================================
//1.0  Priti V2.0.38    06-06-2023  0026257: Excess Qty for an Item to be Stock Transferred automatically to a specific Warehouse while making Issue for Prod
//2.0  Priti V2.0.38    19-06-2023  0026367:In Production Order Qty:  1.A New field required in Production Order Module called 'BOMProductionQty'
//====================================================End Revision History=====================================================================*@

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manufacturing.Models.ViewModel.BOMEntryModel
{
    public class BOMEntryViewModel
    {
        public String BOMNo { get; set; }

        public String strBOMNo { get; set; }

        public String BOM_SCHEMAID { get; set; }

        public String BOMDate { get; set; }

        public DateTime dtBOMDate { get; set; }

        public String FinishedItem { get; set; }

        public String FinishedItemName { get; set; }

        public String FinishedQty { get; set; }

        public String FinishedUom { get; set; }

        public String BOMType { get; set; }

        public String RevisionNo { get; set; }

        public DateTime? dtREVDate { get; set; }

        public String RevisionDate { get; set; }

        public String Unit { get; set; }

        public String Warehouse { get; set; }

        public String WarehouseID { get; set; }

        public List<BranchUnit> UnitList { get; set; }

        public List<BranchWarehouse> WarehouseList { get; set; }

        public String ActualAdditionalCost { get; set; }

        public List<BOMProduct> ListBOMProducts { get; set; }

        public String ProductionID { get; set; }

        public String DetailsID { get; set; }

        public String CreatedBy { get; set; }

        public String ModifyBy { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? ModifyDate { get; set; }

        public String ActualComponentCost { get; set; }

        public String ActualProductCost { get; set; }

        public String ProductionOrderQty { get; set; }

        public String FGReceiptQty { get; set; }
        public String Status { get; set; }
        public String strRemarks { get; set; }
        public Boolean IsActive { get; set; }
        public String TotalResourceCost1 { get; set; }
        public String PartNo { get; set; }
        public String PartNoName { get; set; }
        public String DesignNo { get; set; }
        public String ItemRevNo { get; set; }
        public String Description { get; set; }

        public String ProjectID { get; set; }
        public String Proj_Code { get; set; }
        public List<HierarchyList> Hierarchy_List { get; set; }
        public String Hierarchy { get; set; }

        public String Proj_Name { get; set; }

        public String MPS_ID { get; set; }
        public String MPS_No { get; set; }
        public String MPSDate { get; set; }
        
    }
    public class HierarchyList
    {
        public string Hierarchy_id { get; set; }
        public string Hierarchy_Name { get; set; }
    }
    public class SchemaNumber
    {
        public string SchemaID { get; set; }

        public string SchemaName { get; set; }
    }

    public class UnitList
    {
        public string ID { get; set; }

        public string Name { get; set; }
    }

    public class BranchUnit
    {
        public string BranchID { get; set; }

        public string BankBranchName { get; set; }
    }

    public class BranchWarehouse
    {
        public string WarehouseID { get; set; }

        public string WarehouseName { get; set; }
    }

    public class ProductSerial
    {
        public string SerialID { get; set; }

        public string SerialValue { get; set; }
    }


    public class BatchWarehouse
    {
        public string BatchID { get; set; }

        public string BatchName { get; set; }
    }
    public class BatchWarehouseForFGFinishItem
    {
        public string BatchID { get; set; }

        public string BatchName { get; set; }
        public string MfgDate { get; set; }
        public string ExpiryDate { get; set; }
    }

    public class BOMProduct
    {
        public String SlNO { get; set; }
        public string JobWorkID { get; set; }
        public String BOMProductsID { get; set; }

        public String DetailsID { get; set; }
        public String Details_ID { get; set; }

        public String ProductName { get; set; }

        public String ProductId { get; set; }

        public String ProductDescription { get; set; }

        public String DesignNo { get; set; }
        public String ItemRevisionNo { get; set; }

        public String ProductQty { get; set; }

        public string OrderNo { get; set; }

        public String ProductUOM { get; set; }

        public String StockQty { get; set; }

        public String StockUOM { get; set; }

        public String Warehouse { get; set; }

        public Int64 UOmId { get; set; }

        public Int64 GridWarehouseId { get; set; }

        public String ProductsWarehouseID { get; set; }

        public String Price { get; set; }

        public String Amount { get; set; }

        public String BOMNo { get; set; }

        public String RevNo { get; set; }

        public String OLDQty { get; set; }

        public String OLDAmount { get; set; }

        public String RevDate { get; set; }

        public String Remarks { get; set; }

        public String UpdateEdit { get; set; }

        public String Tag_Details_ID { get; set; }

        public String Tag_Production_ID { get; set; }

        public String BalQty { get; set; }

        public String InventoryType { get; set; }

        public Boolean IsActive { get; set; }

        public String Product_NegativeStock { get; set; }

        public String AvlStk { get; set; }

        public String StkMsg { get; set; }

        public String IsInventory { get; set; }

        //rev Pratik
        public String AltQuantity { get; set; }

        public String AltUom { get; set; }
        public String MultiUOMSelectionForManufacturing { get; set; }
        //End of rev Pratik
        //Rev 1.0
        public String ExcessQty { get; set; }
        //Rev 1.0 End
        //Rev 2.0
        public String BOMProductionQty { get; set; }
        public String sProduct_packageqty { get; set; }
        //Rev 2.0 End
    }


    public class FGReceivedProduct
    {
        public String SlNO { get; set; }
        public string JobWorkID { get; set; }
        public String BOMProductsID { get; set; }
        public Int64 StockUOMId { get; set; }
        public String Details_ID { get; set; }

        public String ProductName { get; set; }

        public String ProductId { get; set; }

        public String ProductDescription { get; set; }

        public String DesignNo { get; set; }
        public String ItemRevisionNo { get; set; }

        public String ProductQty { get; set; }

        public string OrderNo { get; set; }

        public String ProductUOM { get; set; }

        public String StockQty { get; set; }

        public String StockUOM { get; set; }

        public String Warehouse { get; set; }

        public Int64 UOmId { get; set; }

        public Int64 GridWarehouseId { get; set; }

        public String ProductsWarehouseID { get; set; }

        public String Price { get; set; }

        public String Amount { get; set; }

        public String BOMNo { get; set; }

        public String RevNo { get; set; }

        public String OLDQty { get; set; }

        public String OLDAmount { get; set; }

        public String RevDate { get; set; }

        public String Remarks { get; set; }

        public String UpdateEdit { get; set; }

        public String Tag_Details_ID { get; set; }

        public String Tag_Production_ID { get; set; }

        public String BalQty { get; set; }

        public String InventoryType { get; set; }

        public Boolean IsActive { get; set; }

        public String Product_NegativeStock { get; set; }

        public String AvlStk { get; set; }

        public String StkMsg { get; set; }

        public String IsInventory { get; set; }

    }
    public class MateialIssueProduct
    {
        public String SlNO { get; set; }
        public string WorkOrderID { get; set; }
        public string JobWorkID { get; set; }
        public String BOMProductsID { get; set; }

        public String Details_ID { get; set; }

        public String ProductName { get; set; }

        public String ProductId { get; set; }

        public String ProductDescription { get; set; }

        public String DesignNo { get; set; }
        public String ItemRevisionNo { get; set; }

        public String ProductQty { get; set; }

        public string OrderNo { get; set; }

        public String ProductUOM { get; set; }

        public String StockQty { get; set; }

        public String StockUOM { get; set; }

        public String Warehouse { get; set; }

        public Int64 UOmId { get; set; }
        public Int64 ProductUOMId { get; set; }

        public Int64 GridWarehouseId { get; set; }

        public String ProductsWarehouseID { get; set; }

        public String Price { get; set; }

        public String Amount { get; set; }

        public String BOMNo { get; set; }

        public String RevNo { get; set; }

        public String OLDQty { get; set; }

        public String OLDAmount { get; set; }

        public String RevDate { get; set; }

        public String Remarks { get; set; }

        public String UpdateEdit { get; set; }

        public String Tag_Details_ID { get; set; }

        public String Tag_Production_ID { get; set; }

        public String BalQty { get; set; }

        public String InventoryType { get; set; }

        public Boolean IsActive { get; set; }

        public String Product_NegativeStock { get; set; }

        public String AvlStk { get; set; }

        public String StkMsg { get; set; }

        public String IsInventory { get; set; }

    }

    public class BOMDetails
    {
        public String Details_ID { get; set; }

        //public String Production_ID { get; set; }

        public String BOM_No { get; set; }

        public String BOM_Date { get; set; }

        public String REV_Date { get; set; }

        public String REV_No { get; set; }

    }

    public class POSSales
    {
        public String SlNO { get; set; }

        public String ProductName { get; set; }

        public String ProductId { get; set; }

        public String ProductDescription { get; set; }

        public String ProductQty { get; set; }

        public String ProductUOM { get; set; }

        public String StockQty { get; set; }

        public String StockUOM { get; set; }

        public String Warehouse { get; set; }

        public String ProductsWarehouseID { get; set; }

        public String Price { get; set; }

        public String Amount { get; set; }

        public String BOMNo { get; set; }

        public String RevNo { get; set; }

        public String RevDate { get; set; }

        public String Remarks { get; set; }

        public String UpdateEdit { get; set; }
    }

    public class udtEntryProducts
    {
        public Int64 ProductID { get; set; }

        public Decimal StkQty { get; set; }

        public String StkUOM { get; set; }

        public Decimal IssuesQty { get; set; }

        public String IssuesUOM { get; set; }

        public Int64 WarehouseID { get; set; }

        public Decimal Price { get; set; }

        public Decimal Amount { get; set; }

        public Int64 Tag_Details_ID { get; set; }

        public Int64 Tag_Production_ID { get; set; }

        public String Tag_REV_No { get; set; }

        public String Remarks { get; set; }

        public String SlNo { get; set; }

        //rev Pratik
        public decimal AltQuantity { get; set; }

        public string AltUom { get; set; }
        //End of rev Pratik
    }


    public class udtProducts
    {
        public Int64 ProductID { get; set; }

        public Decimal StkQty { get; set; }
        public String BOMProductsID { get; set; }
        public String StkUOM { get; set; }

        public Decimal IssuesQty { get; set; }

        public String IssuesUOM { get; set; }

        public Int64 WarehouseID { get; set; }

        public Decimal Price { get; set; }

        public Decimal Amount { get; set; }

        public Int64 Tag_Details_ID { get; set; }

        public Int64 Tag_Production_ID { get; set; }

        public String Tag_REV_No { get; set; }

        public String Remarks { get; set; }

        //rev Pratik
        public decimal AltQuantity { get; set; }

        public string AltUom { get; set; }
        //End of rev Pratik
        //Rev work start 03.08.2022 mantise no:0025098 code retification
        public int SlNo { get; set; }
        //Rev work close 03.08.2022 mantise no:0025098 code retification
    }

    public class udtEntryResources
    {
        public Int64 ProductID { get; set; }

        public Decimal StkQty { get; set; }

        public String StkUOM { get; set; }

        public Int64 WarehouseID { get; set; }

        public Decimal Price { get; set; }

        public Decimal Amount { get; set; }

        public String Remarks { get; set; }

        public String SlNo { get; set; }
    }

    public class udtResources
    {
        public Int64 ProductID { get; set; }

        public Decimal StkQty { get; set; }

        public String StkUOM { get; set; }

        public Int64 WarehouseID { get; set; }

        public Decimal Price { get; set; }

        public Decimal Amount { get; set; }

        public String Remarks { get; set; }
    }

    public class ReturnData
    {
        public Boolean Success { get; set; }

        public String Message { get; set; }
    }

    public class DesignList
    {
        public String name { get; set; }

        public String reportValue { get; set; }
    }

    public class ProjectList
    {
        public long Proj_Id { get; set; }
        public String ProjectCode { get; set; }
        public String ProjectName { get; set; }
        public String CostomerName { get; set; }
        public String Hierarchy_ID { get; set; }
        public String Hierarchy_Name { get; set; }
    }

    public class MPSNOList
    {
        public long MPS_ID { get; set; }
        public String MPS_No { get; set; }
        public String MPS_Date { get; set; }
        
    }
}

