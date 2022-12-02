using Manufacturing.Models.ViewModel.BOMEntryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manufacturing.Models.ViewModel
{
    public class JobWorkOrderViewModel
    {
        public string SlNO { get; set; }
        public string WorkOrderID { get; set; }

        public string Production_ID { get; set; }
        public string PartProductId { get; set; }
        public string warehouseId { get; set; }

        public string Details_ID { get; set; }

        public String WorkOrderNo { get; set; }

        public String BOMNo { get; set; }

        public String RevNo { get; set; }

        public String FinishedItemName { get; set; }
        public String FinishedItem { get; set; }

        public string Order_SchemaID { get; set; }

        public String OrderDate { get; set; }

        public DateTime dtOrderDate { get; set; }

        public string BRANCH_ID { get; set; }

       // public string WarehouseID { get; set; }

        public String Order_Qty { get; set; }
        public String JobWorkRate { get; set; }

        public String FinishedUom { get; set; }

        public String Finished_Qty { get; set; }

        public String ActualAdditionalCost { get; set; }

        public String ActualComponentCost { get; set; }

        public String ActualProductCost { get; set; }

        public String ProductionOrderQty { get; set; }

        public String FGReceiptQty { get; set; }

        public String TotalCost { get; set; }

        public string UserID { get; set; }

        public String Unit { get; set; }

        public String Warehouse { get; set; }

        public DateTime BOM_Date { get; set; }

        public DateTime? REV_Date { get; set; }

        public String WorkCenterID { get; set; }

        public String OrderNo { get; set; }

        public List<FinishItemDetails> ListFinishItemDetails { get; set; }
        public List<BranchUnit> UnitList { get; set; }

        public String WorkCenterCode { get; set; }

        public String WorkCenterDescription { get; set; }

        public String ProductionOrderNo { get; set; }

        public DateTime? ProductionOrderDate { get; set; }

        public string ProductionOrderID { get; set; }
        public String BOMProductsID { get; set; }
        public String CreatedBy { get; set; }

        public String ModifyBy { get; set; }
        public DateTime? CreateDate { get; set; }

        public DateTime? ModifyDate { get; set; }

        public String strRemarks { get; set; }
        public String TotalResourceCost { get; set; }

        public string PartNo { get; set; }

        public String PartNoName { get; set; }
        public string DesignNo { get; set; }
        public string DrawingheaderNo { get; set; }
        public String ItemRevNo { get; set; }
        public String Description { get; set; }
        public String ProjectID { get; set; }
        public List<HierarchyList> Hierarchy_List { get; set; }
        public String Proj_Code { get; set; }
        public String Hierarchy { get; set; }
        public String Proj_Name { get; set; }
    }

    public class FinishItemDetails
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

        public String FinishUOMId { get; set; }

        public String FinishProductsID { get; set; }

       

    }
    public class ReturnDataClosed
    {
        public Int64 Success { get; set; }

        public String Message { get; set; }
    }
}