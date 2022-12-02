using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMS.Models.ViewModel
{
    public class EstimateViewModel
    {
        public String EstimateNo { get; set; }

        public String strEstimateNo { get; set; }

        public String Estimate_SCHEMAID { get; set; }

        public String EstimateDate { get; set; }

        public DateTime dtEstimateDate { get; set; }

        public String FinishedItem { get; set; }

        public String FinishedItemName { get; set; }

        public String FinishedQty { get; set; }

        public String FinishedUom { get; set; }

        public String EstimateType { get; set; }

        public String RevisionNo { get; set; }

        public DateTime? dtREVDate { get; set; }

        public String strREVDate { get; set; }

        public String RevisionDate { get; set; }

        public String Unit { get; set; }

        public String Warehouse { get; set; }

        public String WarehouseID { get; set; }

        public List<BranchUnit> UnitList { get; set; }

        public List<BranchWarehouse> WarehouseList { get; set; }

        public String ActualAdditionalCost { get; set; }

        public List<EstimateProduct> ListEstimateProducts { get; set; }

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

        public String Proposal { get; set; }

        public String Quotation { get; set; }

        public String Proposal_ID { get; set; }

        public String Quotation_ID { get; set; }

        public String HeadRemarks { get; set; }

        public String Customer { get; set; }

        public String Customer_ID { get; set; }

        public List<String> ContractNo { get; set; }

        public String ProjectID { get; set; }

        public String TaxID { get; set; }

        public String Proj_Code { get; set; }

        public String ReOpen { get; set; }

        public String Cancel { get; set; }

        public String Approve { get; set; }

        public String EstStatus { get; set; }

        public String EstCalcen { get; set; }

        public String ApproveRemarks { get; set; }

        public String ApprvRejct { get; set; }

        public String APPROVE_NAME { get; set; }

        public String Reject { get; set; }

        public String OrderCode { get; set; }

        public List<HierarchyList> Hierarchy_List { get; set; }

        public String Hierarchy { get; set; }

        public String ProductQty { get; set; }

        public String ProductPrice { get; set; }

        public String ProductDiscount { get; set; }

        public String ProductAmount { get; set; }

        public String ProductNetAmount { get; set; }

        public String ProductBudgetedPrice { get; set; }

        public String ProductSellable { get; set; }

        public String ProductCharges { get; set; }

        public String ResQty { get; set; }

        public String ResPrice { get; set; }

        public String ResDiscount { get; set; }

        public String ResAmount { get; set; }

        public String ResNetAmount { get; set; }

        public String ResBudgetedPrice { get; set; }

        public String ResSellable { get; set; }

        public String ResCharges { get; set; }

        public List<String> ProjectIDs { get; set; }

        public String ApprovRevSettings { get; set; }

        public String RevisionRequiredEveryAfterApproval { get; set; }

        public String isFirstApprove { get; set; }
    }

    public class EstimateProduct
    {
        public String SlNO { get; set; }

        public String EstimateroductsID { get; set; }

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

        public String EstimateNo { get; set; }

        public String RevNo { get; set; }

        public String OLDQty { get; set; }

        public String OLDAmount { get; set; }

        public String RevDate { get; set; }

        public String Remarks { get; set; }

        public String UpdateEdit { get; set; }

        public String Tag_Details_ID { get; set; }

        public String Tag_Production_ID { get; set; }

        public String Charges { get; set; }
        public String Discount { get; set; }

        public String NetAmount { get; set; }
        public String BudgetedPrice { get; set; }
        public String TaxTypeID { get; set; }
        public String TaxType { get; set; }
        public String ProdHSN { get; set; }
        public String AddlDesc { get; set; }

        public String Guids { get; set; }

        public String Sellable { get; set; }

        public String SellableID { get; set; }
        public String BalQty { get; set; }
        public String ProductDetailsID { get; set; }
    }

    public class EstimateResource
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

        public String Discount { get; set; }

        public String Amount { get; set; }

        public String BOMNo { get; set; }

        public String RevNo { get; set; }

        public String RevDate { get; set; }

        public String Remarks { get; set; }

        public String UpdateEdit { get; set; }

        public String ResourceCharges { get; set; }

        public String NetAmount { get; set; }

        public String BudgetedPrice { get; set; }

        public String TaxTypeID { get; set; }

        public String TaxType { get; set; }

        public String ProdHSN { get; set; }

        public String AddlDesc { get; set; }

        public String Guids { get; set; }

        public String Sellable { get; set; }

        public String SellableID { get; set; }
        public String ProductDetailsID { get; set; }
        public String BalQty { get; set; }
      
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


    public class ContractList
    {
        public String ContractNo { get; set; }
        public DateTime ContractDate { get; set; }
        public String RevNo { get; set; }
        public DateTime RevDate { get; set; }
        public String CostomerName { get; set; }
        public String Details_ID { get; set; }
    }

    public class AmountFor
    {
        public string taxGrp_Description { get; set; }
        public string taxGrp_Id { get; set; }
    }

    public class udtEstimateProducts
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

        public String Charges { get; set; }

        public String Discount { get; set; }

        public String NetAmount { get; set; }

        public String BudgetedPrice { get; set; }

        public String TaxTypeID { get; set; }

        public String TaxType { get; set; }

        public String AddlDesc { get; set; }
    }

    public class TaxSchemeItemLabel
    {
        public int TaxRates_ID { get; set; }
        public int TaxRates_TaxCode { get; set; }
        public string TaxRatesSchemeName { get; set; }
        public string Taxes_Code { get; set; }
        public string Taxes_ApplicableOn { get; set; }
        public string Taxes_ApplicableFor { get; set; }
        public string TaxCalculateMethods { get; set; }
        public double TaxRates_Rate { get; set; }

    }

    public class TaxDetailsforEntry
    {
        public List<TaxSchemeItemLabel> ItemLevelTaxDetails { get; set; }
        public List<HSNListwithTaxes> HSNCodewiseTaxSchem { get; set; }
        public List<BranchWiseState> BranchWiseStateTax { get; set; }
        public List<StateCodeWiseStateID> StateCodeWiseStateIDTax { get; set; }

    }

    public class HSNListwithTaxes
    {
        public string HSNCODE { get; set; }
        public List<Config_TaxRatesID> config_TaxRatesIDs { get; set; }

    }

    public class Config_TaxRatesID
    {
        public int TaxRates_ID { get; set; }
        public decimal Rate { get; set; }
        public string Taxes_ApplicableOn { get; set; }
        public string TaxTypeCode { get; set; }
        public Config_TaxRatesID(int id, decimal rate, string Taxes_ApplicableOn, string TaxTypeCode)
        {
            this.TaxRates_ID = id;
            this.Rate = rate;
            this.Taxes_ApplicableOn = Taxes_ApplicableOn;
            this.TaxTypeCode = TaxTypeCode;
        }

    }

    public class BranchWiseState
    {
        public int branch_id { get; set; }
        public int branch_state { get; set; }
        public string BranchGSTIN { get; set; }
        public string CompanyGSTIN { get; set; }
    }

    public class StateCodeWiseStateID
    {
        public int id { get; set; }
        public string StateCode { get; set; }
    }

    public class udtEstimateProduct
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

        public String Charges { get; set; }

        public String Discount { get; set; }

        public String NetAmount { get; set; }

        public String BudgetedPrice { get; set; }

        public String TaxTypeID { get; set; }

        public String TaxType { get; set; }

        public String SrlNo { get; set; }

        public String AddlDesc { get; set; }

        public String SellableID { get; set; }
        public String ProductDetailsID { get; set; }
    }

    public class udtEstmtEntryResources
    {
        public Int64 ProductID { get; set; }

        public Decimal StkQty { get; set; }

        public String StkUOM { get; set; }

        public Int64 WarehouseID { get; set; }

        public Decimal Price { get; set; }

        public Decimal Amount { get; set; }

        public String Remarks { get; set; }

        public String SlNo { get; set; }

        public Decimal ResourceCharges { get; set; }

        public Decimal NetAmount { get; set; }

        public Decimal BudgetedPrice { get; set; }

        public String TaxTypeID { get; set; }

        public Decimal Discount { get; set; }

        public String TaxType { get; set; }

        public String EstBalID { get; set; }

        public String AddlDesc { get; set; }
    }

    public class udtEstimateResources
    {
        public Int64 ProductID { get; set; }

        public Decimal StkQty { get; set; }

        public String StkUOM { get; set; }

        public Int64 WarehouseID { get; set; }

        public Decimal Price { get; set; }

        public Decimal Amount { get; set; }

        public String Remarks { get; set; }

        public Decimal Charges { get; set; }

        public Decimal NetAmount { get; set; }

        public Decimal BudgetedPrice { get; set; }

        public String TaxTypeID { get; set; }

        public Decimal Discount { get; set; }

        public String TaxType { get; set; }

        public String SrlNo { get; set; }

        public String AddlDesc { get; set; }
        public String SellableID { get; set; }
        public String ProductDetailsID { get; set; }
    }

    public class DesignList
    {
        public String name { get; set; }

        public String reportValue { get; set; }
    }

}