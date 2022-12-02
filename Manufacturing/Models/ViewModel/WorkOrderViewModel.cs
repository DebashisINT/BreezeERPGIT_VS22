using Manufacturing.Models.ViewModel.BOMEntryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manufacturing.Models.ViewModel
{
    public class WorkOrderViewModel
    {
        public Int64 WorkOrderID { get; set; }

        public Int64 Production_ID { get; set; }

        public Int64 Details_ID { get; set; }

        public String WorkOrderNo { get; set; }

        public String BOMNo { get; set; }

        public String RevNo { get; set; }

        public String FinishedItem { get; set; }

        public Int64 Order_SchemaID { get; set; }

        public String OrderDate { get; set; }

        public DateTime dtOrderDate { get; set; }

        public Int64 BRANCH_ID { get; set; }

        //public Int64 WarehouseID { get; set; }

        public Decimal Order_Qty { get; set; }

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

        public DateTime? REV_Date { get; set; }

        public String WorkCenterID { get; set; }

        public String OrderNo { get; set; }

        public List<BranchUnit> UnitList { get; set; }

        public String WorkCenterCode { get; set; }

        public String WorkCenterDescription { get; set; }

        public String ProductionOrderNo { get; set; }

        public DateTime ProductionOrderDate { get; set; }

        public Int64 ProductionOrderID { get; set; }

        public String CreatedBy { get; set; }

        public String ModifyBy { get; set; }
        public DateTime CreateDate { get; set; }

        public DateTime? ModifyDate { get; set; }

        public String strRemarks { get; set; }
        public String TotalResourceCost { get; set; }

        public String PartNo { get; set; }

        public String PartNoName { get; set; }
        public String DesignNo { get; set; }
        public String ItemRevNo { get; set; }
        public String Description { get; set; }

        public String Proj_Code { get; set; }
        public String Hierarchy { get; set; }
        public String Proj_Name { get; set; }
        public Int64 WorkordrWarehouseId { get; set; }
        public Int64 Proj_Id { get; set; }

        public String FinishedItemID { get; set; }
        public String WarehouseID { get; set; }
    }
}