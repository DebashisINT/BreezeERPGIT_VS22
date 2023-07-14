//@*==================================================== Revision History =========================================================================
// 1.0  Priti  V2.0.38    19-06-2023  0026367:In Production Order Qty:  1.A New field required in Production Order Module called 'BOMProductionQty'
// 2.0  Priti  V2.0.39    14-07-2023  0026384:Show valuation rate feature is required in Production Order module

//====================================================End Revision History=====================================================================*@

using Manufacturing.Models.ViewModel.BOMEntryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manufacturing.Models.ViewModel
{
    public class ProductionOrderViewModel
    {
        public Int64 ProductionOrderID { get; set; }

        public Int64 Production_ID { get; set; }

        public Int64 Details_ID { get; set; }

        public String OrderNo { get; set; }

        public String BOMNo { get; set; }

        public String RevNo { get; set; }

        public String FinishedItem { get; set; }

        public Int64 Order_SchemaID { get; set; }

        public String OrderDate { get; set; }

        public DateTime dtOrderDate { get; set; }

        public Int64 BRANCH_ID { get; set; }

        public Int64 WarehouseID { get; set; }

        public Decimal Order_Qty { get; set; }

        public Decimal BalQty { get; set; }

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

        public List<BranchUnit> UnitList { get; set; }

        public List<BranchWarehouse> WarehouseList { get; set; }

        public DateTime BOM_Date { get; set; }

        public DateTime? REV_Date { get; set; }


        public String CreatedBy { get; set; }

        public String ModifyBy { get; set; }


        public DateTime CreateDate { get; set; }

        public DateTime? ModifyDate { get; set; }

        public String strRemarks { get; set; }

        public String Status { get; set; }
        public String ClosedRemarks { get; set; }
        public String TotalResourceCost { get; set; }

        public String PartNo { get; set; }
        public String PartNoName { get; set; }
        public String DesignNo { get; set; }
        public String ItemRevNo { get; set; }
        public String Description { get; set; }
        public String Proj_Code { get; set; }
        public String Hierarchy { get; set; }
        public String Proj_Name { get; set; }



    }

    public class udtProductionOrderDetails
    {
        public Int64 BOMProductsID { get; set; }
        public Decimal Qty { get; set; }
        public Decimal Amount { get; set; }
        //Rev 1.0
        public Decimal BOMProductionQty { get; set; }
        public Decimal sProduct_packageqty { get; set; }
        //Rev 1.0 End

        //REV 2.0
        public Decimal Price { get; set; }
        //REV 2.0 End
    }


    public class udtFGREceivedDetails
    {
        public Int64 BOMProductsID { get; set; }
        public Decimal Qty { get; set; }
        public Int64 UomId { get; set; }
        public Decimal Price { get; set; }
       

        public Decimal Amount { get; set; }
        public string Remarks { get; set; }


        

    }
    public class udtJobProductionOrderDetails
    {
        public string SlNO { get; set; }
        public string JobWorkID { get; set; }
        public string Details_ID { get; set; }
        public string ProductsID { get; set; }
	public string Description { get; set; }
	public string DrawingNo { get; set; }
	public string DrawingRevNo { get; set; }
    public string UOM { get; set; }
    public string Warehouse { get; set; }
	public string Price { get; set; }
    public string Qty { get; set; }
    public string Amount { get; set; }
    public string Remarks { get; set; }
    }

}