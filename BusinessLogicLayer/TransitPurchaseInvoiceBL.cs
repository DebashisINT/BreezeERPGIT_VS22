using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using DataAccessLayer;
using System.Web;
using EntityLayer;

namespace BusinessLogicLayer
{
   public class TransitPurchaseInvoiceBL
    {
         #region Purchase Invoice Section Detail Start

       public int CheckBillingShippingofVendor(string Vendorid)
       {
           int i;
           int retValue = 0;

           ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceList");
           proc.AddVarcharPara("@Action", 500, "CheckBillingShippingofVendor");
           proc.AddVarcharPara("@Vendorid", 50, Vendorid);
           proc.AddVarcharPara("@ReturnValue", 50, "", QueryParameterDirection.Output);
           i = proc.RunActionQuery();
           retValue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
           return retValue;
       }
        //prc_TransitPurchaseInvoice
       public DataTable PopulateWarehouseByBranchList(string branchlist)
       {
           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_TransitPurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 500, "PopulateWarehouseByBranchList");
           proc.AddVarcharPara("@userbranchlist", 1000, branchlist);
           //proc.AddDecimalPara("@amount", 2, 18, proamt);
           dt = proc.GetTable();
           return dt;
       }
       public DataTable PopulateNonInventoryChargesForVendor(string schemeid, decimal proamt)
       {
           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_TransitPurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 500, "PopulateNonInventoryChargesForVendor");
           proc.AddVarcharPara("@SchemeID", 50, schemeid);
           proc.AddDecimalPara("@amount", 2, 18, proamt);
           dt = proc.GetTable();
           return dt;
       }

       
        public DataTable PopulateLimitcrossDtlofVendorBySchemeId(string Vendorid, string schemeid,decimal totalamt)
       {
           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_TransitPurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 100, "PopulateLimitcrossDtlofVendorBySchemeId");
           proc.AddVarcharPara("@VendorID", 50, Vendorid);
           proc.AddVarcharPara("@SchemeID", 50, schemeid);
           proc.AddDecimalPara("@TDSAmt",2,18,  totalamt);
            
           
           dt = proc.GetTable();
           return dt;
       }
       
       public DataTable PopulateNonInvProductBySchemeId(string schemeid, string company,string finyear)
       {
           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_TransitPurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 100, "PopulateNonInvProductBySchemeId");
           proc.AddVarcharPara("@SchemeID", 50, schemeid);
           proc.AddVarcharPara("@campany_Id", 50, company);
           proc.AddVarcharPara("@FinYear", 50, finyear);
           proc.AddVarcharPara("@IsInventory", 2, "N");
           
           dt = proc.GetTable();
           return dt;
       }

       public DataTable PopulateTDSSchemeForNonInventoryItem()
       {
           DataTable ds = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_TransitPurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 100, "PopulateTDSSchemeForNonInventoryItem");           
           ds = proc.GetTable();
           return ds;
       }

       public DataTable GetApplicableAmtDetail(string Vendorid, string SchemeID)
       {
           DataTable ds = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_TransitPurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 100, "GetApplicableAmtDetail");
           proc.AddVarcharPara("@Vendorid", 20, Vendorid);
           proc.AddVarcharPara("@SchemeID", 15, SchemeID); 
           ds = proc.GetTable();
           return ds;
       }

       public DataTable getBranchListByHierchy(string userbranchhierchy)
       {
           DataTable ds = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_TransitPurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 100, "getBranchListbyHierchy");
           proc.AddVarcharPara("@BranchList", 1000, userbranchhierchy);
           ds = proc.GetTable();
           return ds;
       }
       public DataTable GetInvoiceListGridDataByDate(string userbranchlist, string lastCompany, string fromdate, string todate, string branch)
       {

           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_TransitPurchaseInvoiceDetail");
           //proc.AddVarcharPara("@Action", 500, "GetQuotationListGridDataByDate");
           proc.AddVarcharPara("@Action", 500, "GetInvoiceListGridDataByDate");
           proc.AddVarcharPara("@BranchList", 500, userbranchlist);
           proc.AddVarcharPara("@CompanyID", 50, lastCompany);
           proc.AddVarcharPara("@fromdate", 50, fromdate);
           proc.AddVarcharPara("@todate", 50, todate); 
           proc.AddVarcharPara("@branchId", 50, branch);
          
           dt = proc.GetTable();
           return dt;
       }

       
       public DataTable PurchaseOrdTagInPurchaseInvoice(string POTaggingApplicable)
       {
           try
           {
               DataTable dt = new DataTable();
               ProcedureExecute proc = new ProcedureExecute("prc_TransitPurchaseInvoiceDetail");
               proc.AddVarcharPara("@Action", 500, "PurchaseOrdTagInPurchaseInvoice");
               proc.AddVarcharPara("@POTaggingApplicable", 500, POTaggingApplicable);
                
               dt = proc.GetTable();

               return dt;
           }
           catch
           {
               return null;
           }
       }
       public DataTable CheckDuplicateSerial(string SerialNo, string ProductID, string BranchID, string Action)
       {
           try
           {
               DataTable dt = new DataTable();
               ProcedureExecute proc = new ProcedureExecute("proc_CheckDuplicateProduct");
               proc.AddVarcharPara("@Action", 500, Action);
               proc.AddVarcharPara("@SerialNo", 500, SerialNo);
               proc.AddVarcharPara("@ProductID", 500, ProductID);
               proc.AddVarcharPara("@BranchID", 500, BranchID);
               dt = proc.GetTable();

               return dt;
           }
           catch
           {
               return null;
           }
       }
       public DataSet GetCustomerDetails_InvoiceRelated(string strCustomerID,string branchid)
       {
           DataSet dst = new DataSet();
           ProcedureExecute proc = new ProcedureExecute("prc_TransitPurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 100, "GetVendorInvoiceDetails");
           proc.AddVarcharPara("@CustomerID", 100, strCustomerID);
           proc.AddIntegerPara("@branchId",Convert.ToInt32(branchid));
           dst = proc.GetDataSet();
           return dst;
       }

       public DataTable GetTdsMainIdByInvoiceID(string invoiceid)
       {
           DataTable ds = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_TransitPurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 100, "GetTdsMainIdByInvoiceID");
           proc.AddVarcharPara("@InvoiceID",100, invoiceid);
           ds = proc.GetTable();
           return ds;
       }

       
       public DataTable PopulateContactPersonOfCustomer(string InternalId)
       {
           DataTable ds = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_TransitPurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 100, "PopulateContactPersonOfCustomer");
           proc.AddVarcharPara("@InternalId", 100, InternalId);
           ds = proc.GetTable();
           return ds;
       }
       public DataTable PopulateProductDtlByInvoiceId(string strPurchaseInvoiceID)
       {
           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_TransitPurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 500, "PopulateProductDtlByInvoiceId");
           proc.AddIntegerPara("@InvoiceID", Convert.ToInt32(strPurchaseInvoiceID));
           dt = proc.GetTable();
           return dt;
       }
       public DataSet GetFirstPOPCHeaderDtl(string doctype, string docNo)  
       {
           DataSet dst = new DataSet();
           ProcedureExecute proc = new ProcedureExecute("prc_TransitPurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 500, "GetFirstPOPCHeaderDtl");
           proc.AddVarcharPara("@docno", 50, docNo);
           proc.AddVarcharPara("@doctype", 5, doctype);

           dst = proc.GetDataSet();
           return dst;
       }
       public DataTable GetBranchIdByInvoiceID(string strPurchaseInvoiceID)
       {
           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_TransitPurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 500, "GetBranchIdByInvoiceID");
           proc.AddIntegerPara("@InvoiceID", Convert.ToInt32(strPurchaseInvoiceID)); 
           dt = proc.GetTable();
           return dt;
       }
        

       public DataTable PopulateTdsChargesByID(string strPurchaseInvoiceID)
       {
           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_TransitPurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 500, "PopulateTdsChargesByID");
           proc.AddIntegerPara("@PurchaseInvoiceId", Convert.ToInt32(strPurchaseInvoiceID)); 
           dt = proc.GetTable();
           return dt;
       }
       public DataTable PopulateBranchForNonInventoryCharges(string childbranchlist)
       {
           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_TransitPurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 500, "PopulateBranchForNonInventoryCharges");
           proc.AddVarcharPara("@userbranchlist", 1000, childbranchlist); 
           dt = proc.GetTable();
           return dt;
       }
       public DataTable PopulateNonInventoryCharges(string ProductID,decimal proamt)
       {
           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_TransitPurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 500, "PopulateNonInventoryCharges");
           proc.AddVarcharPara("@ProductID", 2000, ProductID);
           proc.AddDecimalPara("@amount", 2, 18, proamt);
           dt = proc.GetTable();
           return dt;
       }

        
       public DataTable GetLinkedProductList(string Action, string ProductID)
       {
           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesInvoice_Details");
           proc.AddVarcharPara("@Action", 500, Action);
           proc.AddVarcharPara("@ProductID", 2000, ProductID);
           dt = proc.GetTable();
           return dt;
       }
       public DataTable PopulateInvoiceID(string invoicedetailid)
       {
           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_TransitPurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 500, "PopulateInvoiceID");
           proc.AddVarcharPara("@invoicedetailid", 50, invoicedetailid);
           //proc.AddVarcharPara("@FinYear", 50, FinYear);
           dt = proc.GetTable();
           return dt;
       }
       public DataTable PopulateSchemaTypeAfterInsertion(string finyear,string userbranch,string LastCompany )
       {
           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_TransitPurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 500, "PopulateSchemaTypeAfterInsertion");
           proc.AddVarcharPara("@FinYear", 500, finyear);
           proc.AddVarcharPara("@userbranchlist", 5000, userbranch);
           proc.AddVarcharPara("@lastCompany", 50, LastCompany);
           //proc.AddVarcharPara("@FinYear", 50, FinYear);
           dt = proc.GetTable();
           return dt;
       }
       public DataSet GetAllDropDownDetailForPurchaseInvoice(string userbranch ,int branch,string companyid)
       {
           DataSet ds = new DataSet();
           ProcedureExecute proc = new ProcedureExecute("[prc_TransitPurchaseInvoiceDetail]");
           proc.AddVarcharPara("@Action", 100, "GetAllDropDownDetailForPurchaseInvoice");
           proc.AddVarcharPara("@userbranch", 5000, userbranch);
           proc.AddVarcharPara("@CompanyId", 100, companyid);
           proc.AddIntegerPara("@branch",  branch);
           ds = proc.GetDataSet();
           return ds;
       }

       public DataTable GetPurchaseInvoiceListGridData(string userbranchlist, string lastCompany, string lastfinyear)
       {

           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_TransitPurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 500, "GetPurchaseInvoiceListGridData");
            proc.AddVarcharPara("@userbranchlist", 5000, userbranchlist);
            proc.AddVarcharPara("@lastCompany", 50, lastCompany);
            proc.AddVarcharPara("@FinYear", 50, lastfinyear);
            dt = proc.GetTable();
            return dt;
       }
      

       public DataTable GetPurchaseInvoiceListGridDataOpening(string userbranchlist, string lastCompany, string finYear)
       {

           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_TransitPurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 500, "GetPurchaseInvoiceListGridDataOpening");
           proc.AddVarcharPara("@userbranchlist", 5000, userbranchlist);
           proc.AddVarcharPara("@lastCompany", 50, lastCompany);
           proc.AddVarcharPara("@FinYear", 50, finYear);
           dt = proc.GetTable();
           return dt;
       }

       public DataTable GetComponent(string invoiceid,string Customer, string Date, string ComponentType, string Action, string branchlist, string company, string FinYear)
       {
           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_TransitPurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 500, Action);
           proc.AddVarcharPara("@CustomerID", 500, Customer);
           proc.AddVarcharPara("@InvoiceID", 500, invoiceid);
           proc.AddDateTimePara("@Date", Convert.ToDateTime(Date));
           proc.AddVarcharPara("@ComponentType", 10, ComponentType);
           proc.AddVarcharPara("@userbranchlist", 5000, branchlist);
           proc.AddVarcharPara("@lastCompany", 50, company);
           proc.AddVarcharPara("@FinYear", 50, FinYear);
           dt = proc.GetTable();
           return dt;
       }

       public DataTable GetComponentForOPPB(string Customer, string Date, string ComponentType, string Action)
       {
           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_TransitPurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 500, Action);
           proc.AddVarcharPara("@CustomerID", 500, Customer);
           proc.AddDateTimePara("@Date", Convert.ToDateTime(Date));
           proc.AddVarcharPara("@ComponentType", 10, ComponentType); 
           dt = proc.GetTable();
           return dt;
       }


       public DataTable GetComponentProductList(string Action, string ComponentList, string InvoiceID)
       {
           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_TransitPurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 500, Action);
           proc.AddVarcharPara("@ComponentList", 2000, ComponentList);
           proc.AddVarcharPara("@InvoiceID", 20, InvoiceID);
           dt = proc.GetTable();
           return dt;
       }
       public DataTable GetSelectedComponentProductList(string Action, string SelectedComponentList, string InvoiceID)
       {
           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_TransitPurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 500, Action);
           proc.AddVarcharPara("@SelectedComponentList", 2000, SelectedComponentList);
           proc.AddVarcharPara("@InvoiceID", 20, InvoiceID);
           dt = proc.GetTable();
           return dt;
       }

       public int DeletePurchaseInvoice(string PurchaseInvoiceid, string UserID)
       {
           int i;
           int rtrnvalue = 0;
           ProcedureExecute proc = new ProcedureExecute("prc_TransitPurchaseInvoice");
           proc.AddVarcharPara("@Action", 100, "DeletePurchaseInvoice");
           proc.AddVarcharPara("@DeleteInvoiceId", 20, PurchaseInvoiceid);
           proc.AddVarcharPara("@UserID",500, UserID);
           proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
           i = proc.RunActionQuery();
           rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
           return rtrnvalue;

       }
       public int DeleteTransitPurchaseInvoice(string PurchaseInvoiceid)
       {
           int i;
           int rtrnvalue = 0;
           ProcedureExecute proc = new ProcedureExecute("prc_TransitPurchaseInvoice");
           proc.AddVarcharPara("@Action", 100, "DeleteTransitPurchaseInvoice");
           proc.AddVarcharPara("@DeleteInvoiceId", 20, PurchaseInvoiceid);
           proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
           i = proc.RunActionQuery();
           rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
           return rtrnvalue;

       }

       public DataTable PopulateProductWiseWarehouseDetail(int productid, string invoiceid, string companyid, int branchid)
       {

           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_TransitPurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 500, "PopulateProductWiseWarehouseDetail");
           proc.AddVarcharPara("@InvoiceID", 50, invoiceid);
           proc.AddIntegerPara("@EditProductID",  productid);
           //proc.AddVarcharPara("@userbranch", 500, branchid);
           proc.AddVarcharPara("@lastCompany", 50, companyid);
           dt = proc.GetTable();
           return dt;
       }

       public DataTable PopulateWareHouseByInvoiceId( string invoiceid)
       {

           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_TransitPurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 500, "PopulateWareHouseByInvoiceId");
           proc.AddVarcharPara("@InvoiceID", 50, invoiceid); 
           //proc.AddVarcharPara("@userbranch", 500, branchid);
           //proc.AddVarcharPara("@lastCompany", 50, companyid);
           dt = proc.GetTable();
           return dt;
       }

       public DataTable PopulateVendorsDetail()
       {
           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_TransitPurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 100, "PopulateVendorsDetail");
           dt = proc.GetTable();
           return dt;
       }

       public DataTable PopulateVendorsDetailByInventoryItem(string invtype)
       {
           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_TransitPurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 100, "PopulateVendorsDetailByInventoryItem");
           proc.AddVarcharPara("@InventoryType", 2, invtype);
           dt = proc.GetTable();
           return dt;
       }

#endregion Purchase Invoice Section Detail End

       #region Transit Purchase Invoice Section Detail
       public DataTable GetTransitInvoiceListGridDataByDate(string userbranchlist, string lastCompany, string fromdate, string todate, string branch)
       {

           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_TransitPurchaseInvoiceDetail");
           //proc.AddVarcharPara("@Action", 500, "GetQuotationListGridDataByDate");
           proc.AddVarcharPara("@Action", 500, "GetTransitInvoiceListGridDataByDate");
           proc.AddVarcharPara("@BranchList", 500, userbranchlist);
           proc.AddVarcharPara("@CompanyID", 50, lastCompany);
           proc.AddVarcharPara("@fromdate", 50, fromdate);
           proc.AddVarcharPara("@todate", 50, todate);
           proc.AddVarcharPara("@branchId", 50, branch);
           dt = proc.GetTable();
           return dt;
       }

       public DataTable GetTransitPurchaseInvoiceListGridData(string userbranchlist, string lastCompany, string lastfinyear)
       {

           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_TransitPurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 500, "GetTransitPurchaseInvoiceListGridData");
           proc.AddVarcharPara("@userbranchlist", 5000, userbranchlist);
           proc.AddVarcharPara("@lastCompany", 50, lastCompany);
           proc.AddVarcharPara("@FinYear", 50, lastfinyear);
           dt = proc.GetTable();
           return dt;
       }


       public DataTable GetTdsSectionByID(string strPurchaseInvoiceID)
       {
           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_TransitPurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 500, "GetTdsSectionByID");
           proc.AddIntegerPara("@PurchaseInvoiceId", Convert.ToInt32(strPurchaseInvoiceID));
           dt = proc.GetTable();
           return dt;
       }

       #endregion Transit Purchase Invoice


       public string Session { get; set; }
    }
}
