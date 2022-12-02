using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMS.Models.ViewModel
{
    public class ContractOrderViewModel
    {
        public String ContractNo { get; set; }

        public String strCotractNo { get; set; }

        public String Contract_SCHEMAID { get; set; }

        public String CotractDate { get; set; }

        public DateTime dtContarctDate { get; set; }

        public String RevisionNo { get; set; }

        public DateTime? dtREVDate { get; set; }

        public String RevisionDate { get; set; }

        public String Unit { get; set; }

        public List<BranchUnit> UnitList { get; set; }

        public List<BranchWarehouse> WarehouseList { get; set; }

        public String ActualAdditionalCost { get; set; }

        public List<ContractProduct> ListBOMProducts { get; set; }

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

        public String BOQ { get; set; }

        public String Estimate { get; set; }

        public String Proposal { get; set; }

        public String Quotation { get; set; }

        public String BOQ_ID { get; set; }

        public String Estimate_ID { get; set; }

        public String Proposal_ID { get; set; }

        public String Quotation_ID { get; set; }

        public String HeadRemarks { get; set; }
    }

    public class ContractDetails
    {
        public String Details_ID { get; set; }

        //public String Production_ID { get; set; }

        public String Contract_No { get; set; }

        public String Contract_Date { get; set; }

        public String REV_Date { get; set; }

        public String REV_No { get; set; }

    }

    public class ContractSales
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

        public String ResourceCharges { get; set; }
    }

    public class ContractProduct
    {
        public String SlNO { get; set; }

        public String ContractProductsID { get; set; }

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

        public String ContractNo { get; set; }

        public String RevNo { get; set; }

        public String OLDQty { get; set; }

        public String OLDAmount { get; set; }

        public String RevDate { get; set; }

        public String Remarks { get; set; }

        public String UpdateEdit { get; set; }

        public String Tag_Details_ID { get; set; }

        public String Tag_Production_ID { get; set; }

    }
}