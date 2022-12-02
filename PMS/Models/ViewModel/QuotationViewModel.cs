using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMS.Models.ViewModel
{
    public class QuotationViewModel
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
}