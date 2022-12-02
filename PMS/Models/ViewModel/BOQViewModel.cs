using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMS.Models.ViewModel
{
    public class BOQViewModel
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

    public class BOMProduct
    {
        public String SlNO { get; set; }

        public String BOMProductsID { get; set; }

        public String Details_ID { get; set; }

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

        public String OLDQty { get; set; }

        public String OLDAmount { get; set; }

        public String RevDate { get; set; }

        public String Remarks { get; set; }

        public String UpdateEdit { get; set; }

        public String Tag_Details_ID { get; set; }

        public String Tag_Production_ID { get; set; }

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
    }


    public class udtProducts
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
}