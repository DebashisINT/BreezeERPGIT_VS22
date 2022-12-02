using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DataAccessLayer;
namespace BusinessLogicLayer
{
    public class ReportData
    {
        public DataSet reportDataSet(string reportKey)
        {
            DataSet reportData = new DataSet();
            DBEngine oDBEngine = new DBEngine();
            DataTable dt = new DataTable();

            switch (reportKey)
            {  
                case "BNKMAST":
                    
                    dt = oDBEngine.GetDataTable("select *  from tbl_master_Bank");
                    reportData.DataSetName = "ReportDataSource";
                    dt.TableName = "Header Table";
                    reportData.Tables.Add(dt);
                    break;
                case "MProd":
                    string reportString = @"SELECT MP.sProducts_ID,MP.sProducts_Code  ,MP.sProducts_Name,MP.sProducts_Description,MP.sProducts_Type,CASE WHEN MP.sProducts_Type ='A' THEN 'Raw Material' WHEN MP.sProducts_Type ='B' THEN 'Work-In-Process'    
                                     WHEN  MP.sProducts_Type ='C' THEN 'Finished Goods' END AS sProducts_TypeFull  ,MP.ProductClass_Code ,MPC.ProductClass_Name   ,MP.sProducts_GlobalCode,MP.sProducts_TradingLot, MP.sProducts_TradingLotUnit,MP.sProducts_QuoteCurrency  
                                    ,MP.sProducts_QuoteLot, MP.sProducts_QuoteLotUnit, MP.sProducts_DeliveryLot, MP.sProducts_DeliveryLotUnit  ,MP.sProducts_Color ,MP.sProducts_Size  
                                    ,MP.sProducts_CreateUser  ,MP.sProducts_CreateTime ,MP.sProducts_ModifyUser  ,MP.sProducts_ModifyTime  FROM Master_sProducts MP left join Master_ProductClass MPC  on MP.ProductClass_Code=MPC.ProductClass_ID ";

                    dt = oDBEngine.GetDataTable(reportString);
                    reportData.DataSetName = "ReportDataSource";
                    dt.TableName = "Header Table";
                    reportData.Tables.Add(dt);
                    break;
            }
            return reportData;
        }

        //public string GenerateSqlDataSource(string reportKey, string Module)
        //{ 
        //    string reportString="";
        //        switch (Module)
        //        {
        //            case "PIQUOTATION":
        //                switch (reportKey)
        //                {
        //                    case "Header":
        //                        reportString = @"SELECT Quote_Id,Quote_CompanyID,Quote_BranchId,Quote_FinYear,Quote_Number,Quote_Date,Quote_Expiry,Quote_Code,Quote_TotalAmount,Customer_Id,Contact_Person_Id,Quote_Reference,Currency_Id,Currency_Conversion_Rate,Tax_Option,Tax_Code,Sls_ActivityId,CreatedBy,ModifiedBy,CreatedDate,ModifiedDate,Quote_BillingAddress_Id,Quote_ShippingAddress_Id,Quote_SalesmanId,Quote_Status,Quote_Remarks FROM tbl_trans_Quotation ";
        //                        break;
        //                    case "Details":
        //                        reportString = @"SELECT QuoteDetails_Id,QuoteDetails_QuoteId,QuoteDetails_ProductId as Product_Id,QuoteDetails_ProductDescription,QuoteDetails_Quantity,QuoteDetails_UOMId,QuoteDetails_StockQty,QuoteDetails_StockUOMId,QuoteDetails_SalePrice,QuoteDetails_Discount,QuoteDetails_GrossAmount,QuoteDetails_TaxableAmount,QuoteDetails_TaxOnGrossAmount,QuoteDetails_TaxAmount,QuoteDetails_TotalAmountInBaseCurrency,QuoteDetails_CreateBy,QuoteDetails_CreatedDate,QuoteDetails_ModifiedBy,QuoteDetails_ModifiedDate,QuoteDetails_CompanyId,QuoteDetails_BranchId,QuoteDetails_FinYear,QuoteDetails_PackingUOM,QuoteDetails_PackingQty FROM tbl_trans_QuotationProducts ";
        //                        break;
        //                    case "Address":
        //                        reportString = @"SELECT QuoteAdd_id,QuoteAdd_QuoteId,QuoteAdd_CompanyID,QuoteAdd_BranchId,QuoteAdd_FinYear,QuoteAdd_ContactPerson,QuoteAdd_addressType,QuoteAdd_address1,QuoteAdd_address2,QuoteAdd_address3,QuoteAdd_landMark,QuoteAdd_countryId,QuoteAdd_stateId,QuoteAdd_cityId,QuoteAdd_areaId,QuoteAdd_pin,QuoteAdd_CreatedDate,QuoteAdd_CreatedUser,QuoteAdd_LastModifyDate,QuoteAdd_LastModifyUser FROM tbl_trans_QuotationAddress ";
        //                        break;
        //                    case "ProductWiseTax":
        //                        reportString = @"SELECT ProductTax_Id,ProductTax_QuoteId,cast(ProductTax_ProductId as bigint) as ProductId,ProductTax_TaxTypeId,ProductTax_Percentage,ProductTax_Amount,ProductTax_CompanyID,ProductTax_Branch,ProductTax_FinYear,ProductTax_CreatedBy,ProductTax_CreatedDate,ProductTax_ModifiedBy,ProductTax_ModifiedDate,ProductTax_VatGstCstId,ProductTax_CalculationMethod FROM tbl_trans_QuotationProductTax ";
        //                        break;
        //                    case "HeaderMainTax":
        //                        reportString = @"SELECT QuoteTax_Id,QuoteTax_QuoteId,QuoteTax_TaxTypeId,QuoteTax_Percentage,QuoteTax_Amount,QuoteTax_CompanyID,QuoteTax_Branch,QuoteTax_FinYear,QuoteTax_CreatedBy,QuoteTax_CreatedDate,QuoteTax_ModifiedBy,QuoteTax_ModifiedDate,QuoteTax_CalculationMethod,ProductTax_VatGstCstId FROM tbl_trans_QuotationTax ";
        //                        break;
        //                    case "CompanyMaster":
        //                        reportString = @"SELECT cmp_id,cmp_internalid,cmp_Name,cmp_parentid,cmp_natureOfBusiness,cmp_directors,cmp_authorizedSignatories,cmp_exchange,cmp_registrationNo,cmp_sebiRegnNo,cmp_panNo,cmp_serviceTaxNo,cmp_salesTaxNo,CreateDate,CreateUser,LastModifyDate,LastModifyUser,cmp_DateIncorporation,cmp_CIN,cmp_CINdt,cmp_VregisNo,cmp_VPanNo,cmp_OffRoleShortName,cmp_OnRoleShortName,com_Add,com_logopath,cmp_currencyid,cmp_KYCPrefix,cmp_KRAIntermediaryID,cmp_LedgerView,cmp_CombinedCntrDate,cmp_CombCntrNumber,cmp_CombCntrReset,cmp_CombCntrOrder,cmp_vat_no,cmp_EPFRegistrationNo,cmp_EPFRegistrationNoValidfrom,cmp_EPFRegistrationNoValidupto,cmp_ESICRegistrationNo,cmp_ESICRegistrationNoValidfrom,cmp_ESICRegistrationNoValidupto,onrole_schema_id,offrole_schema_id,cmp_bigLogo,cmp_smallLogo,cmp_gstin FROM tbl_master_company ";
        //                        break;
        //                    case "ProdWiseTaxWiseAmnt":
        //                        reportString = @"SELECT ProductTax_Id,ProductTax_QuoteId,ProductTax_ProductId,ProductTax_TaxTypeId,ProductTax_Percentage,ProductTax_Amount,ProductTax_CompanyID,ProductTax_Branch,ProductTax_FinYear,ProductTax_CreatedBy,ProductTax_CreatedDate,ProductTax_ModifiedBy,ProductTax_ModifiedDate,ProductTax_VatGstCstId,ProductTax_CalculationMethod FROM tbl_trans_QuotationProductTax ";
        //                        break;
        //                    case "productwisebarcode":
        //                        reportString = @"SELECT Product_SrlNo,SrlNo,Quote_Id,QuoteWarehouse_Id,QuoteDetails_Id,ProductId,WarehouseID,WarehouseName,Quantity,BatchID,BatchNo,SerialID,SerialNo,SalesUOMName,SalesUOMCode,SalesQuantity,StkUOMName,StkUOMCode,StkQuantity,ConversionMultiplier,AvailableQty,BalancrStk,MfgDate,ExpiryDate,LoopID,TotalQuantity,Inventory_type FROM v_Quotation_WarehouseDetails ";
        //                        break;
        //                }
        //            break;
        //        }
        //        return reportString;
        //}
        public string GenerateSqlDataSource(string reportKey)
        {
            string reportString = "";
            switch (reportKey)
            {
                case "Header":
                    reportString = @"SELECT Quote_Id,Quote_CompanyID,Quote_BranchId,Quote_FinYear,Quote_Number,Quote_Date,Quote_Expiry,Quote_Code,Quote_TotalAmount,Customer_Id,Contact_Person_Id,Quote_Reference,Currency_Id,Currency_Conversion_Rate,Tax_Option,Tax_Code,Sls_ActivityId,CreatedBy,ModifiedBy,CreatedDate,ModifiedDate,Quote_BillingAddress_Id,Quote_ShippingAddress_Id,Quote_SalesmanId,Quote_Status,Quote_Remarks FROM tbl_trans_Quotation ";
                    break;
                case "Details":
                    reportString = @"SELECT QuoteDetails_Id,QuoteDetails_QuoteId,QuoteDetails_ProductId as Product_Id,QuoteDetails_ProductDescription,QuoteDetails_Quantity,QuoteDetails_UOMId,QuoteDetails_StockQty,QuoteDetails_StockUOMId,QuoteDetails_SalePrice,QuoteDetails_Discount,QuoteDetails_GrossAmount,QuoteDetails_TaxableAmount,QuoteDetails_TaxOnGrossAmount,QuoteDetails_TaxAmount,QuoteDetails_TotalAmountInBaseCurrency,QuoteDetails_CreateBy,QuoteDetails_CreatedDate,QuoteDetails_ModifiedBy,QuoteDetails_ModifiedDate,QuoteDetails_CompanyId,QuoteDetails_BranchId,QuoteDetails_FinYear,QuoteDetails_PackingUOM,QuoteDetails_PackingQty FROM tbl_trans_QuotationProducts ";
                    break;
                case "Address":
                    reportString = @"SELECT QuoteAdd_id,QuoteAdd_QuoteId,QuoteAdd_CompanyID,QuoteAdd_BranchId,QuoteAdd_FinYear,QuoteAdd_ContactPerson,QuoteAdd_addressType,QuoteAdd_address1,QuoteAdd_address2,QuoteAdd_address3,QuoteAdd_landMark,QuoteAdd_countryId,QuoteAdd_stateId,QuoteAdd_cityId,QuoteAdd_areaId,QuoteAdd_pin,QuoteAdd_CreatedDate,QuoteAdd_CreatedUser,QuoteAdd_LastModifyDate,QuoteAdd_LastModifyUser FROM tbl_trans_QuotationAddress ";
                    break;
                case "ProductWiseTax":
                    reportString = @"SELECT ProductTax_Id,ProductTax_QuoteId,cast(ProductTax_ProductId as bigint) as ProductId,ProductTax_TaxTypeId,ProductTax_Percentage,ProductTax_Amount,ProductTax_CompanyID,ProductTax_Branch,ProductTax_FinYear,ProductTax_CreatedBy,ProductTax_CreatedDate,ProductTax_ModifiedBy,ProductTax_ModifiedDate,ProductTax_VatGstCstId,ProductTax_CalculationMethod FROM tbl_trans_QuotationProductTax ";
                    break;
                case "HeaderMainTax":
                    reportString = @"SELECT QuoteTax_Id,QuoteTax_QuoteId,QuoteTax_TaxTypeId,QuoteTax_Percentage,QuoteTax_Amount,QuoteTax_CompanyID,QuoteTax_Branch,QuoteTax_FinYear,QuoteTax_CreatedBy,QuoteTax_CreatedDate,QuoteTax_ModifiedBy,QuoteTax_ModifiedDate,QuoteTax_CalculationMethod,ProductTax_VatGstCstId FROM tbl_trans_QuotationTax ";
                    break;
                case "CompanyMaster":
                    reportString = @"SELECT cmp_id,cmp_internalid,cmp_Name,cmp_parentid,cmp_natureOfBusiness,cmp_directors,cmp_authorizedSignatories,cmp_exchange,cmp_registrationNo,cmp_sebiRegnNo,cmp_panNo,cmp_serviceTaxNo,cmp_salesTaxNo,CreateDate,CreateUser,LastModifyDate,LastModifyUser,cmp_DateIncorporation,cmp_CIN,cmp_CINdt,cmp_VregisNo,cmp_VPanNo,cmp_OffRoleShortName,cmp_OnRoleShortName,com_Add,com_logopath,cmp_currencyid,cmp_KYCPrefix,cmp_KRAIntermediaryID,cmp_LedgerView,cmp_CombinedCntrDate,cmp_CombCntrNumber,cmp_CombCntrReset,cmp_CombCntrOrder,cmp_vat_no,cmp_EPFRegistrationNo,cmp_EPFRegistrationNoValidfrom,cmp_EPFRegistrationNoValidupto,cmp_ESICRegistrationNo,cmp_ESICRegistrationNoValidfrom,cmp_ESICRegistrationNoValidupto,onrole_schema_id,offrole_schema_id,cmp_bigLogo,cmp_smallLogo,cmp_gstin FROM tbl_master_company ";
                    break;
                case "ProdWiseTaxWiseAmnt":
                    reportString = @"SELECT ProductTax_Id,ProductTax_QuoteId,ProductTax_ProductId,ProductTax_TaxTypeId,ProductTax_Percentage,ProductTax_Amount,ProductTax_CompanyID,ProductTax_Branch,ProductTax_FinYear,ProductTax_CreatedBy,ProductTax_CreatedDate,ProductTax_ModifiedBy,ProductTax_ModifiedDate,ProductTax_VatGstCstId,ProductTax_CalculationMethod FROM tbl_trans_QuotationProductTax ";
                    break;
                case "productwisebarcode":
                    reportString = @"SELECT Product_SrlNo,SrlNo,Quote_Id,QuoteWarehouse_Id,QuoteDetails_Id,ProductId,WarehouseID,WarehouseName,Quantity,BatchID,BatchNo,SerialID,SerialNo,SalesUOMName,SalesUOMCode,SalesQuantity,StkUOMName,StkUOMCode,StkQuantity,ConversionMultiplier,AvailableQty,BalancrStk,MfgDate,ExpiryDate,LoopID,TotalQuantity,Inventory_type FROM v_Quotation_WarehouseDetails ";
                    break;
            }
            return reportString;
        }


        public DataTable GetBranchGestn(string  Company, string Finyear,string Action,string Gstn,string branch)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_GSTR1_Report");
            proc.AddPara("@COMPANYID", Company);
            proc.AddPara("@FINYEAR", Finyear);
            proc.AddPara("@Action", Action);
            proc.AddPara("@GSTIN", Gstn);
            proc.AddPara("@MONTH", branch);

            ds = proc.GetTable();
            return ds;
        }
       
    }
}
