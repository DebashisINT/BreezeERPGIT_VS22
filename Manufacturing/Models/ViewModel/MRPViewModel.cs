//==================================================== Revision History =========================================================================
// 1.0  Priti V2.0.36    23-01-2023  0025610:MRP Close Feature required
// 2.0  Priti V2.0.36    01-02-2023  0025634:Available Stock to be calculated in MRP product Wise
// 3.0  Priti V2.0.37    13-03-2023  save Avl Stk in table
//====================================================End Revision History=====================================================================


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Manufacturing.Models.ViewModel.BOMEntryModel;
namespace Manufacturing.Models.ViewModel
{
   
        public class MRPViewModel
        {
            public string MRP_ID { get; set; }
            public string MRPNo { get; set; }
            public String BOMNo { get; set; }
            public String strBOMNo { get; set; }

            public String BOM_SCHEMAID { get; set; }

            public String BOMDate { get; set; }

            public String MRPDate { get; set; }
            public DateTime dtMRPDate { get; set; }

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

           

            public List<BranchUnit> UnitList { get; set; }

            public String DetailsID { get; set; }

            public String CreatedBy { get; set; }

            public String ModifyBy { get; set; }

            public DateTime? CreateDate { get; set; }

            public DateTime? ModifyDate { get; set; }           
            
            public String strRemarks { get; set; }
           
            public String PartNo { get; set; }
            public String PartNoName { get; set; }
            public String DesignNo { get; set; }
            public String ItemRevNo { get; set; }
            public String Description { get; set; }

            public String BOMRelationshipNo { get; set; }

            public String BOMRelationshipID { get; set; }

            public String ParentBOMID { get; set; }
            public String UOM_ID { get; set; }
            public String UOM_Name { get; set; }

            public String MPSDate { get; set; }
            public String MPSNo { get; set; }
            public String MPS_ID { get; set; }

            public String FG_ID { get; set; }
            
            public String Status { get; set; }//REV 1.0

    }
     
        public class MRPProduct
        {
            public String SlNO { get; set; }
           
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
            public String UOmId { get; set; }         

            public String Price { get; set; }

            public String Amount { get; set; }

            public String BOMNo { get; set; }

            public String RevNo { get; set; }

            public String OLDQty { get; set; }

            public String OLDAmount { get; set; }

            public String RevDate { get; set; }

            public String Remarks { get; set; }

            public String UpdateEdit { get; set; }

            public String IndentQty { get; set; }

            public String PkgQty { get; set; }

            public String BalQty { get; set; }

            public String InventoryType { get; set; }

            public Boolean IsActive { get; set; }

            public String Product_NegativeStock { get; set; }

            public String AvlStk { get; set; }

            public String StkMsg { get; set; }

            public String IsInventory { get; set; }


            public String PurchaseQty { get; set; }

            public String AltQty { get; set; }
            public String OldAltQty { get; set; }
            public String AltUOM { get; set; }
            public String AltUOMID { get; set; }
            public String VendorName { get; set; }
            public String NewAvlStk { get; set; }//2.0
    }
        public class udtMRPEntryProducts
        {
            public Int64 ProductID { get; set; }
            public String ProductDescription { get; set; }
            public String DesignNo { get; set; }
            public String ItemRevisionNo { get; set; }
            public Decimal ProductQty { get; set; }
            public Int64 UOmId { get; set; }
            public Decimal StkQty { get; set; }       
            public Decimal Price { get; set; }
            public Decimal Amount { get; set; }
            public Decimal IndentQty { get; set; }
            public Decimal PkgQty { get; set; }
            public Decimal PurchaseQty { get; set; }
            public Decimal AltQty { get; set; }
            public Int64 AltUOMID { get; set; }
            public String SlNo { get; set; }
            public Decimal OLDQty { get; set; }
            public Decimal OldAltQty { get; set; }
            //Rev 3.0
            public Decimal NewAvlStk { get; set; }
            //Rev 3.0 End
    }

    public class udtMRPProducts
        {
            public Int64 ProductID { get; set; }
            public String ProductDescription { get; set; }
            public String DesignNo { get; set; }
            public String ItemRevisionNo { get; set; }
            public Decimal ProductQty { get; set; }
            public Int64 UOmId { get; set; }
            public Decimal StkQty { get; set; }
            public Decimal Price { get; set; }
            public Decimal Amount { get; set; }
            public Decimal IndentQty { get; set; }
            public Decimal PkgQty { get; set; }
            public Decimal PurchaseQty { get; set; }
            public Decimal AltQty { get; set; }
            public Int64 AltUOMID { get; set; }

            public Decimal OLDQty { get; set; }
            public Decimal OldAltQty { get; set; }
            //Rev 3.0
            public Decimal NewAvlStk { get; set; }
            //Rev 3.0 End
    }
}