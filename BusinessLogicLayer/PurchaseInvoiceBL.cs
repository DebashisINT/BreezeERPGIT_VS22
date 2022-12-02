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
   public class PurchaseInvoiceBL
   {
       //.GetSystemSettingData("PurchaseOrdTagInPurchaseInvoice");

       #region SaleBill 
       public DataTable PopulateSalePurchaseBillByTransporterID(string InternalId)
       {
           DataTable ds = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 100, "PopulateSalePurchaseBillByTransporterID");
           proc.AddVarcharPara("@InternalId", 100, InternalId);
           ds = proc.GetTable();
           return ds;
       }

       #endregion SaleBill




       public DataSet PopulatePurchaseBillByTransporterIDWithTaggedPBid(string InternalId, string vendorbranchid, string InvoiceID)
       {
           DataSet ds = new DataSet();
           ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 100, "PopulatePurchaseBillByTransporterIDWithTaggedPBid");
           proc.AddVarcharPara("@InternalId", 100, InternalId);
           proc.AddVarcharPara("@branchId", 100, vendorbranchid);
           proc.AddVarcharPara("@InvoiceID", 100, InvoiceID);
           ds = proc.GetDataSet();
           return ds;
       }      
       public DataTable PopulatePurchaseBillByTransporterID(string InternalId,string schemabranchid)
       {
           DataTable ds = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 100, "PopulatePurchaseBillByTransporterID");
           proc.AddVarcharPara("@InternalId", 100, InternalId);
           proc.AddVarcharPara("@branchId", 100, schemabranchid);          
           ds = proc.GetTable();
           return ds;
       }


       public DataSet PopulateTaggedPBByTransporterID(string transporterID, string TaggedInvoiceID)
       {
           DataSet dst = new DataSet();
           ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 100, "PopulateTaggedPBByTransporterID");
           proc.AddVarcharPara("@InternalId", 10, transporterID);
           proc.AddVarcharPara("@InvoiceID", 15, TaggedInvoiceID);
           dst = proc.GetDataSet();
           return dst;
       }
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

        

       #region PB List Function

       public DataTable PopulateProductDtlByInvoiceId(string strPurchaseInvoiceID)
       {
           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 500, "PopulateProductDtlByInvoiceId");
           proc.AddIntegerPara("@InvoiceID", Convert.ToInt32(strPurchaseInvoiceID));
           dt = proc.GetTable();
           return dt;
       }

       public DataTable GetPurchaseInvoiceListGridData(string userbranchlist, string lastCompany, string lastfinyear, string invoicefor)
       {

           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceList");
           proc.AddVarcharPara("@Action", 500, "GetPurchaseInvoiceListGridData");
           proc.AddVarcharPara("@userbranchlist", 5000, userbranchlist);
           proc.AddVarcharPara("@lastCompany", 50, lastCompany);
           proc.AddVarcharPara("@FinYear", 50, lastfinyear);
           proc.AddVarcharPara("@invoicefor", 5, invoicefor);
           dt = proc.GetTableModified();
           // dt = proc.GetTable();
           return dt;
       }
       public DataTable getBranchListByHierchy(string userbranchhierchy)
       {
           DataTable ds = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 100, "getBranchListbyHierchy");
           proc.AddVarcharPara("@BranchList", 1000, userbranchhierchy);
           ds = proc.GetTable();
           return ds;
       }
       public DataTable getBranchListByHierchyProjectPInv(string userbranchhierchy)
       {
           DataTable ds = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 100, "getBranchListByHierchyProjectPInv");
           proc.AddVarcharPara("@BranchList", 1000, userbranchhierchy);
           ds = proc.GetTable();
           return ds;
       }
       //public DataTable PopulateAccessPages(string userid, string userlastsegment)
       //{
       //    DataTable dt = new DataTable();
       //    ProcedureExecute proc = new ProcedureExecute("prc_MasterDetail");
       //    proc.AddVarcharPara("@Action", 100, "PopulateAccessPages");
       //    proc.AddVarcharPara("@userid", 7, userid);
       //    proc.AddVarcharPara("@userlastsegment", 10, userlastsegment);

       //    dt = proc.GetTable();
       //    return dt;
       //}

       public DataTable GetInvoiceListGridDataByDate(string userbranchlist, string lastCompany, string fromdate, string todate, string branch, string Invicefor)
       {

           DataTable dt = new DataTable();
           DataTable dt1 = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceList");
           //proc.AddVarcharPara("@Action", 500, "GetQuotationListGridDataByDate");
           proc.AddVarcharPara("@Action", 500, "GetInvoiceListGridDataByDate");
           proc.AddVarcharPara("@BranchList", 500, userbranchlist);
           proc.AddVarcharPara("@CompanyID", 50, lastCompany);
           proc.AddVarcharPara("@fromdate", 50, fromdate);
           proc.AddVarcharPara("@todate", 50, todate);
           proc.AddVarcharPara("@branchId", 50, branch);
           proc.AddVarcharPara("@invoicefor", 5, Invicefor);
           dt = proc.GetTableModified();
           //dt1 = proc.GetTable();
           return dt;
       }

       #endregion PB List Function


       #region Purchase Invoice Section Detail Start

       public int UpdateEWayBill(string InvoiceID, string EWayBillNumber, string EWayBillDate, string EWayBillValue, string TransporterGSTIN
                    , string TransporterName, string TransportationMode, string TransportationDistance, string TransporterDocNo
                    , string TransporterDocDate, string VehicleNo, string VehicleType)
       {
           int i;
           int rtrnvalue = 0;
           ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 100, "UpdateEWayBill");
           proc.AddBigIntegerPara("@Invoice_ID",Convert.ToInt32(InvoiceID));
           proc.AddVarcharPara("@EWayBillNumber", 50, EWayBillNumber);
           proc.AddVarcharPara("@EWayBillDate", 50, EWayBillDate);
           proc.AddVarcharPara("@EWayBillValue", 50, EWayBillValue);

           proc.AddPara("@TransporterGSTIN", TransporterGSTIN);
           proc.AddPara("@TransporterName", TransporterName);
           proc.AddPara("@TransportationMode", TransportationMode);
           proc.AddPara("@TransportationDistance", TransportationDistance);
           proc.AddPara("@TransporterDocNo", TransporterDocNo);
           proc.AddPara("@TransporterDocDate", TransporterDocDate);
           proc.AddPara("@VehicleNo", VehicleNo);
           proc.AddPara("@VehicleType", VehicleType);

           proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
           i = proc.RunActionQuery();
           rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
           return rtrnvalue;

       }


       public int UpdateEWayBillInvCumChallan(string InvoiceID, string EWayBillNumber, string EWayBillDate, string EWayBillValue, string TransporterGSTIN
                   , string TransporterName, string TransportationMode, string TransportationDistance, string TransporterDocNo
                   , string TransporterDocDate, string VehicleNo, string VehicleType)
       {
           int i;
           int rtrnvalue = 0;
           ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 100, "UpdateEWayBillInvCumChallan");
           proc.AddBigIntegerPara("@Invoice_ID", Convert.ToInt32(InvoiceID));
           proc.AddVarcharPara("@EWayBillNumber", 50, EWayBillNumber);
           proc.AddVarcharPara("@EWayBillDate", 50, EWayBillDate);
           proc.AddVarcharPara("@EWayBillValue", 50, EWayBillValue);

           proc.AddPara("@TransporterGSTIN", TransporterGSTIN);
           proc.AddPara("@TransporterName", TransporterName);
           proc.AddPara("@TransportationMode", TransportationMode);
           proc.AddPara("@TransportationDistance", TransportationDistance);
           proc.AddPara("@TransporterDocNo", TransporterDocNo);
           proc.AddPara("@TransporterDocDate", TransporterDocDate);
           proc.AddPara("@VehicleNo", VehicleNo);
           proc.AddPara("@VehicleType", VehicleType);

           proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
           i = proc.RunActionQuery();
           rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
           return rtrnvalue;

       }

       public int UpdateEWayBillTransitPB(string InvoiceID, string EWayBillNumber, string EWayBillDate, string EWayBillValue, string TransporterGSTIN
                    , string TransporterName, string TransportationMode, string TransportationDistance, string TransporterDocNo
                    , string TransporterDocDate, string VehicleNo, string VehicleType)
       {
           int i;
           int rtrnvalue = 0;
           ProcedureExecute proc = new ProcedureExecute("prc_TransitPurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 100, "UpdateEWayBill");
           proc.AddBigIntegerPara("@Invoice_ID",Convert.ToInt32(InvoiceID));
           proc.AddVarcharPara("@EWayBillNumber", 50, EWayBillNumber);
           proc.AddVarcharPara("@EWayBillDate", 50, EWayBillDate);
           proc.AddVarcharPara("@EWayBillValue", 50, EWayBillValue);

           proc.AddPara("@TransporterGSTIN", TransporterGSTIN);
           proc.AddPara("@TransporterName", TransporterName);
           proc.AddPara("@TransportationMode", TransportationMode);
           proc.AddPara("@TransportationDistance", TransportationDistance);
           proc.AddPara("@TransporterDocNo", TransporterDocNo);
           proc.AddPara("@TransporterDocDate", TransporterDocDate);
           proc.AddPara("@VehicleNo", VehicleNo);
           proc.AddPara("@VehicleType", VehicleType);

           proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
           i = proc.RunActionQuery();
           rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
           return rtrnvalue;

       }
       public int UpdateEWayBillTransitSB(string InvoiceID, string EWayBillNumber, string EWayBillDate, string EWayBillValue, string TransporterGSTIN
                    , string TransporterName, string TransportationMode, string TransportationDistance, string TransporterDocNo
                    , string TransporterDocDate, string VehicleNo, string VehicleType)
       {
           int i;
           int rtrnvalue = 0;
           ProcedureExecute proc = new ProcedureExecute("prc_TransitSalesInvoice_Details");
           proc.AddVarcharPara("@Action", 100, "UpdateEWayBill");
           proc.AddBigIntegerPara("@Invoice_ID",Convert.ToInt32(InvoiceID));
           proc.AddVarcharPara("@EWayBillNumber", 50, EWayBillNumber);
           proc.AddVarcharPara("@EWayBillDate", 50, EWayBillDate);
           proc.AddVarcharPara("@EWayBillValue", 50, EWayBillValue);

           proc.AddPara("@TransporterGSTIN", TransporterGSTIN);
           proc.AddPara("@TransporterName", TransporterName);
           proc.AddPara("@TransportationMode", TransportationMode);
           proc.AddPara("@TransportationDistance", TransportationDistance);
           proc.AddPara("@TransporterDocNo", TransporterDocNo);
           proc.AddPara("@TransporterDocDate", TransporterDocDate);
           proc.AddPara("@VehicleNo", VehicleNo);
           proc.AddPara("@VehicleType", VehicleType);

           proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
           i = proc.RunActionQuery();
           rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
           return rtrnvalue;

       }
       
       public DataTable SHowHidePanelVendorWise(int docid,string doctype)
       {
           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("[prc_PBHeaderDetail]");
           proc.AddVarcharPara("@Action", 100, "SHowHidePanelVendorWise");           
           proc.AddIntegerPara("@docid", docid);
           proc.AddVarcharPara("@doctype", 50, doctype);
           dt = proc.GetTableModified();
           //dt1 = proc.GetTable();
           return dt;
       }      
       public DataTable BindBranchByParentID(string branchhierchy)
       {
           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("[prc_PBHeaderDetail]");
           proc.AddVarcharPara("@Action", 100, "BindBranchByParentID");
           proc.AddVarcharPara("@userbranch", 5000, branchhierchy);
           dt = proc.GetTableModified();
           //dt1 = proc.GetTable();
           return dt;
        }
       public DataSet PopulatePBHeaderDetail(string userbranch, int branch, string companyid,string finyear, string POTaggingApplicable, string PB_Branch_Selection, string Option)
       {
           DataSet ds = new DataSet();
           ProcedureExecute proc = new ProcedureExecute("[prc_PBHeaderDetail]");
           proc.AddVarcharPara("@Action", 100, "PopulatePBHeaderDetail");
           proc.AddVarcharPara("@userbranch", 5000, userbranch);
           proc.AddIntegerPara("@branch", branch);
           proc.AddVarcharPara("@CompanyId", 100, companyid);
           proc.AddVarcharPara("@FinYear", 20, finyear);
           proc.AddVarcharPara("@POTaggingApplicable", 500, POTaggingApplicable);
           proc.AddVarcharPara("@PBBranchSelection", 500, PB_Branch_Selection);
           proc.AddVarcharPara("@Option", 500, Option);
           ds = proc.GetDataSet();
           return ds;
       }

       //PopulateVendorProductTaxDataOnLoad(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear1"]), Convert.ToString(Session["userbranchID"]), Convert.ToString(ddlInventory.SelectedItem.Value), DateTime.Now.ToString("yyyy-MM-dd"), Convert.ToString(ddl_TdsScheme.Value));
       public DataSet PopulateVendorProductTaxDataOnLoad(string companyid, string FinYear, string branchid,string inventorytype,string curdate,string tdssection)
       {
           try
           {
               DataSet dst = new DataSet();
               ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceList");
               proc.AddVarcharPara("@Action", 100, "PopulateVendorProductTaxDataOnLoad");
               proc.AddVarcharPara("@CompanyId", 10, companyid);
               proc.AddVarcharPara("@FinYear", 10, FinYear);
               proc.AddVarcharPara("@branchId", 10, branchid);
               proc.AddVarcharPara("@IsInventory", 2, inventorytype);
               proc.AddVarcharPara("@CurrenteDate",12, curdate);
               proc.AddVarcharPara("@SchemeID", 10, tdssection);
               dst = proc.GetDataSet();
               return dst;
           }
           catch
           {
               return null;
           }
       }
       public int UpdatePartyInvoiceNoDate(string PBid, string InvoiceNo, string InvoiceDate)
       {
           int i;
           int retValue = 0;

           ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 500, "UpdatePartyInvoiceNoDate");
           proc.AddVarcharPara("@InvoiceID", 50, PBid);
           proc.AddVarcharPara("@partyinviceNo", 50, InvoiceNo);
           proc.AddVarcharPara("@partyinviceDate", 50, InvoiceDate);
           proc.AddVarcharPara("@result", 50, "", QueryParameterDirection.Output);
           i = proc.RunActionQuery();
           retValue = Convert.ToInt32(proc.GetParaValue("@result"));
           return retValue;
       }       
       public DataTable CheckShippingAddressOfVendor(string Vendorid)
       {
           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 100, "CheckShippingAddressOfVendor");
           proc.AddVarcharPara("@VendorID", 50, Vendorid); 
           dt = proc.GetTable();
           return dt;
       }
       public DataTable PopulateWarehouseByBranchList(string branchlist)
       {
           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 500, "PopulateWarehouseByBranchList");
           proc.AddVarcharPara("@userbranchlist", 1000, branchlist);
           //proc.AddDecimalPara("@amount", 2, 18, proamt);
           dt = proc.GetTable();
           return dt;
       }
       public DataTable PopulateNonInventoryChargesForVendor(string schemeid, decimal proamt)
       {
           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 500, "PopulateNonInventoryChargesForVendor");
           proc.AddVarcharPara("@SchemeID", 50, schemeid);
           proc.AddDecimalPara("@amount", 2, 18, proamt);
           dt = proc.GetTable();
           return dt;
       }       
       public DataTable PopulateLimitcrossDtlofVendorBySchemeId(string Vendorid, string schemeid,decimal totalamt)
       {
           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetail");
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
           ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetail");
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
           ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 100, "PopulateTDSSchemeForNonInventoryItem");           
           ds = proc.GetTable();
           return ds;
       }
       public DataTable GetApplicableAmtDetail(string Vendorid, string SchemeID)
       {
           DataTable ds = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 100, "GetApplicableAmtDetail");
           proc.AddVarcharPara("@Vendorid", 20, Vendorid);
           proc.AddVarcharPara("@SchemeID", 15, SchemeID); 
           ds = proc.GetTable();
           return ds;
       }

       //public DataTable getBranchListByHierchy(string userbranchhierchy)
       //{
       //    DataTable ds = new DataTable();
       //    ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetail");
       //    proc.AddVarcharPara("@Action", 100, "getBranchListbyHierchy");
       //    proc.AddVarcharPara("@BranchList", 1000, userbranchhierchy);
       //    ds = proc.GetTable();
       //    return ds;
       //}
       //public DataTable GetInvoiceListGridDataByDate(string userbranchlist, string lastCompany, string fromdate, string todate, string branch, string Invicefor)
       //{

       //    DataTable dt = new DataTable();
       //    ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetail");
       //    //proc.AddVarcharPara("@Action", 500, "GetQuotationListGridDataByDate");
       //    proc.AddVarcharPara("@Action", 500, "GetInvoiceListGridDataByDate");
       //    proc.AddVarcharPara("@BranchList", 500, userbranchlist);
       //    proc.AddVarcharPara("@CompanyID", 50, lastCompany);
       //    proc.AddVarcharPara("@fromdate", 50, fromdate);
       //    proc.AddVarcharPara("@todate", 50, todate);
       //    proc.AddVarcharPara("@branchId", 50, branch);
       //    proc.AddVarcharPara("@invoicefor", 5, Invicefor);

       //    dt = proc.GetTable();
       //    return dt;
       //}

       #region System Setting Section Start

       public DataSet SystemSettingChecking(string POTaggingApplicable, string PB_Branch_Selection, string Option)
       {
           try
           {
               DataSet dst = new DataSet();
               ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetail");
               proc.AddVarcharPara("@Action", 100, "SystemSettingChecking");
               proc.AddVarcharPara("@POTaggingApplicable", 500, POTaggingApplicable);
               proc.AddVarcharPara("@PBBranchSelection", 500, PB_Branch_Selection);
               proc.AddVarcharPara("@Option", 500, Option);
               dst = proc.GetDataSet();
               return dst;
           }
           catch
           {
               return null;
           }
       }
       //public DataTable PurchaseOrdTagInPurchaseInvoice(string POTaggingApplicable)
       //{
       //    try
       //    {
       //        DataTable dt = new DataTable();
       //        ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetail");
       //        proc.AddVarcharPara("@Action", 500, "PurchaseOrdTagInPurchaseInvoice");
       //        proc.AddVarcharPara("@POTaggingApplicable", 500, POTaggingApplicable);
                
       //        dt = proc.GetTable();

       //        return dt;
       //    }
       //    catch
       //    {
       //        return null;
       //    }
       //}

       //public DataTable BranchEditableInPurchaseInvoice(string PB_Branch_Selection)
       //{
       //    try
       //    {
       //        DataTable dt = new DataTable();
       //        ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetail");
       //        proc.AddVarcharPara("@Action", 500, "BranchEditableInPurchaseInvoice");
       //        proc.AddVarcharPara("@PBBranchSelection", 500, PB_Branch_Selection);
                
       //        dt = proc.GetTable();

       //        return dt;
       //    }
       //    catch
       //    {
       //        return null;
       //    }
       //}

       #endregion System Setting Section Start
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
           ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 100, "GetVendorInvoiceDetails");
           proc.AddVarcharPara("@CustomerID", 100, strCustomerID);
           proc.AddIntegerPara("@branchId",Convert.ToInt32(branchid));
           dst = proc.GetDataSet();
           return dst;
       }
       #region Optimized By Sam on 16102017
       public DataSet PopulateVendorRelatedInfo(string strCustomerID, string branchid)
       {
           DataSet dst = new DataSet();
           ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 100, "PopulateVendorRelatedInfo");
           proc.AddVarcharPara("@CustomerID", 100, strCustomerID);
           proc.AddIntegerPara("@branchId", Convert.ToInt32(branchid));
           dst = proc.GetDataSet();
           return dst;
       }

       #endregion Optimized By Sam on 16102017
       public DataTable GetTdsMainIdByInvoiceID(string invoiceid)
       {
           DataTable ds = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 100, "GetTdsMainIdByInvoiceID");
           proc.AddVarcharPara("@InvoiceID",100, invoiceid);
           ds = proc.GetTable();
           return ds;
       }       
       public DataTable PopulateContactPersonOfCustomer(string InternalId)
       {
           DataTable ds = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 100, "PopulateContactPersonOfCustomer");
           proc.AddVarcharPara("@InternalId", 100, InternalId);
           ds = proc.GetTable();
           return ds;
       }
       //public DataTable PopulateProductDtlByInvoiceId(string strPurchaseInvoiceID)
       //{
       //    DataTable dt = new DataTable();
       //    ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetail");
       //    proc.AddVarcharPara("@Action", 500, "PopulateProductDtlByInvoiceId");
       //    proc.AddIntegerPara("@InvoiceID", Convert.ToInt32(strPurchaseInvoiceID));
       //    dt = proc.GetTable();
       //    return dt;
       //}
       public DataSet GetFirstPOPCHeaderDtl(string doctype, string docNo)  
       {
           DataSet dst = new DataSet();
           ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 500, "GetFirstPOPCHeaderDtl");
           proc.AddVarcharPara("@docno", 50, docNo);
           proc.AddVarcharPara("@doctype", 5, doctype);

           dst = proc.GetDataSet();
           return dst;
       }
       public DataTable GetBranchIdByInvoiceID(string strPurchaseInvoiceID)
       {
           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 500, "GetBranchIdByInvoiceID");
           proc.AddIntegerPara("@InvoiceID", Convert.ToInt32(strPurchaseInvoiceID)); 
           dt = proc.GetTable();
           return dt;
       }      

       public DataTable PopulateTdsChargesByID(string strPurchaseInvoiceID)
       {
           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 500, "PopulateTdsChargesByID");
           proc.AddIntegerPara("@PurchaseInvoiceId", Convert.ToInt32(strPurchaseInvoiceID)); 
           dt = proc.GetTable();
           return dt;
       }
       public DataTable PopulateBranchForNonInventoryCharges(string childbranchlist)
       {
           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 500, "PopulateBranchForNonInventoryCharges");
           proc.AddVarcharPara("@userbranchlist", 1000, childbranchlist); 
           dt = proc.GetTable();
           return dt;
       }
       public DataTable PopulateNonInventoryCharges(string ProductID,decimal proamt,string vendorId,DateTime trans_Date)
       {
           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 500, "PopulateNonInventoryCharges");
           proc.AddVarcharPara("@ProductID", 2000, ProductID);
           proc.AddDecimalPara("@amount", 2, 18, proamt);
           proc.AddVarcharPara("@VendorID", 2000, vendorId);
           proc.AddPara("@Date", trans_Date);
           dt = proc.GetTable();
           return dt;
       }

       public DataTable PopulateNonInventoryChargesBasedOnEntryTDS(string ProductID, decimal proamt, string vendorId, DateTime trans_Date, Int64 EntryTDSId)
       {
           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 500, "PopulateNonInventoryChargesBasedOnEntryTDS");
           proc.AddVarcharPara("@ProductID", 2000, ProductID);
           proc.AddDecimalPara("@amount", 2, 18, proamt);
           proc.AddVarcharPara("@VendorID", 2000, vendorId);
           proc.AddPara("@Date", trans_Date);
           proc.AddBigIntegerPara("@EntryTDSId", EntryTDSId);
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
           ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 500, "PopulateInvoiceID");
           proc.AddVarcharPara("@invoicedetailid", 50, invoicedetailid);
           //proc.AddVarcharPara("@FinYear", 50, FinYear);
           dt = proc.GetTable();
           return dt;
       }
       public DataTable PopulateSchemaTypeAfterInsertion(string finyear,string userbranch,string LastCompany )
       {
           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetail");
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
           ProcedureExecute proc = new ProcedureExecute("[prc_PurchaseInvoiceDetail]");
           proc.AddVarcharPara("@Action", 100, "GetAllDropDownDetailForPurchaseInvoice");
           proc.AddVarcharPara("@userbranch", 5000, userbranch);
           proc.AddVarcharPara("@CompanyId", 100, companyid);
           proc.AddIntegerPara("@branch",  branch);
           ds = proc.GetDataSet();
           return ds;
       }

       //public DataTable GetPurchaseInvoiceListGridData(string userbranchlist, string lastCompany, string lastfinyear, string invoicefor)
       //{

       //    DataTable dt = new DataTable();
       //    ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetail");
       //    proc.AddVarcharPara("@Action", 500, "GetPurchaseInvoiceListGridData");
       //    proc.AddVarcharPara("@userbranchlist", 5000, userbranchlist);
       //    proc.AddVarcharPara("@lastCompany", 50, lastCompany);
       //    proc.AddVarcharPara("@FinYear", 50, lastfinyear);
       //    proc.AddVarcharPara("@invoicefor", 5, invoicefor);
       //    dt = proc.GetTable();
       //    return dt;
       //}
       //public DataTable GetPurchaseInvoiceListGridData(string userbranchlist, string lastCompany, string lastfinyear)
       //{

       //    DataTable dt = new DataTable();
       //    ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetail");
       //    proc.AddVarcharPara("@Action", 500, "GetPurchaseInvoiceListGridData");
       //     proc.AddVarcharPara("@userbranchlist", 5000, userbranchlist);
       //     proc.AddVarcharPara("@lastCompany", 50, lastCompany);
       //     proc.AddVarcharPara("@FinYear", 50, lastfinyear);
       //     dt = proc.GetTable();
       //     return dt;
       //}
      

       public DataTable GetPurchaseInvoiceListGridDataOpening(string userbranchlist, string lastCompany, string finYear)
       {

           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 500, "GetPurchaseInvoiceListGridDataOpening");
           proc.AddVarcharPara("@userbranchlist", 5000, userbranchlist);
           proc.AddVarcharPara("@lastCompany", 50, lastCompany);
           proc.AddVarcharPara("@FinYear", 50, finYear);
           dt = proc.GetTable();
           return dt;
       }



       public DataTable PopulateVendorInEditMode(string VendorId)
       {

           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 100, "PopulateVendorInEditMode");
           proc.AddVarcharPara("@VendorID", 50, VendorId); 
           dt = proc.GetTable();
           return dt;
       }
       public DataTable GetComponent(string invoiceid,string Customer, string Date, string ComponentType, string Action, string branchlist, string company, string FinYear,string invtype )
       {
           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 500, Action);
           proc.AddVarcharPara("@CustomerID", 500, Customer);
           proc.AddVarcharPara("@InvoiceID", 500, invoiceid);
           proc.AddDateTimePara("@Date", Convert.ToDateTime(Date));
           proc.AddVarcharPara("@ComponentType", 10, ComponentType);
           proc.AddVarcharPara("@userbranchlist", 5000, branchlist);
           proc.AddVarcharPara("@lastCompany", 50, company);
           proc.AddVarcharPara("@FinYear", 50, FinYear);
           proc.AddVarcharPara("@InventoryType", 5, invtype);
           
           dt = proc.GetTable();
           return dt;
       }

       public DataTable GetComponentForOPPB(string Customer, string Date, string ComponentType, string Action)
       {
           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetail");
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
           ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 500, Action);
           proc.AddVarcharPara("@ComponentList", 2000, ComponentList);
           proc.AddVarcharPara("@InvoiceID", 20, InvoiceID);
           dt = proc.GetTable();
           return dt;
       }
       //Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear1"]), Convert.ToString(Session["userbranchID"]), Convert.ToString(ddlInventory.SelectedItem.Value), DateTime.Now.ToString("yyyy-MM-dd"), Convert.ToString(ddl_TdsScheme.Value)
       public DataTable PopulateProductOnDemand(string filter, int startindex, int EndIndex, string companyid, string FinYear, string branchid, string inventorytype, string curdate, string tdssection)
       {
           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceList");
           proc.AddVarcharPara("@Action", 500, "PopulateProductOnDemand");
           //proc.AddVarcharPara("@branchId", 20, branchId);
           proc.AddVarcharPara("@filter", 20, filter); 
           proc.AddIntegerPara("@startindex", startindex);
           proc.AddIntegerPara("@EndIndex",EndIndex);
           proc.AddVarcharPara("@CompanyId", 10, companyid);
           proc.AddVarcharPara("@FinYear", 10, FinYear);
           proc.AddVarcharPara("@branchId", 10, branchid);
           proc.AddVarcharPara("@IsInventory", 2, inventorytype);
           proc.AddVarcharPara("@CurrenteDate", 12, curdate);
           proc.AddVarcharPara("@SchemeID", 10, tdssection); 
           dt = proc.GetTable();
           return dt;
       }

       public DataTable PopulateVendorOnDemand(string branchId, string filter, int startindex, int EndIndex)  
       {
           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetail"); 
           proc.AddVarcharPara("@Action", 500, "PopulateVendorOnDemand");
           proc.AddVarcharPara("@branchId", 20, branchId);
           proc.AddVarcharPara("@filter", 20, filter);
           proc.AddIntegerPara("@startindex", startindex);
           proc.AddIntegerPara("@EndIndex", EndIndex);
           dt = proc.GetTable();
           return dt;
       }


       
       #region optimization By sam on 16102017
       public DataSet GetComponentProductListDetail(string Action, string ComponentList, string InvoiceID, string docno)
       {
           DataSet dst = new DataSet();
           ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 500, Action);
           proc.AddVarcharPara("@docno", 50, docno);
           proc.AddVarcharPara("@ComponentList", 2000, ComponentList);
           proc.AddVarcharPara("@InvoiceID", 20, InvoiceID);
           dst = proc.GetDataSet();
           return dst;
       }

       public DataSet PopulateTaggingAllDetailInfo(string Action, string ComponentList, string InvoiceID, string strType)
       {
           DataSet dst = new DataSet();
           ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceList");
           proc.AddVarcharPara("@Action", 500, Action);
           proc.AddVarcharPara("@SelectedComponentList", 2000, ComponentList);
           proc.AddVarcharPara("@InvoiceID", 20, InvoiceID);
           proc.AddVarcharPara("@ComponentType", 20, strType);
           dst = proc.GetDataSet();
           return dst;
       }

       public DataSet PopulateTaggingAllDetailInfoForInvChallan(string Action, string ComponentList, string InvoiceID, string strType)
       {
           DataSet dst = new DataSet();
           ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceList");
           proc.AddVarcharPara("@Action", 500, Action);
           proc.AddVarcharPara("@SelectedComponentList", 2000, ComponentList);
           proc.AddVarcharPara("@InvoiceID", 20, InvoiceID);
           proc.AddVarcharPara("@ComponentType", 20, strType);
           dst = proc.GetDataSet();
           return dst;
       }

       public DataTable PopulateMultiUOMTaggingDetailInfo(string ComponentList, string ChallanComponent)
       {
           DataTable dst = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetail");
           //Rev Manties 24428
           //proc.AddVarcharPara("@action", 500, "GetChallanWiseMultiUOMData");
           proc.AddVarcharPara("@action", 500, "GetChallanWiseMultiUOMData_New");
           //End Rev Mantis 24428
           proc.AddVarcharPara("@SelectedComponentList", 2000, ChallanComponent);
           proc.AddVarcharPara("@SelectedComponentDetailsList", 2000, ComponentList);
           dst = proc.GetTable();
           return dst;
       }

       // Rev Sanchita
       public DataTable PopulateMultiUOMTaggingDetailInfoForInvoice_New(string ComponentList, string ChallanComponent, string strType)
       {
           DataTable dst = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetail");
           proc.AddVarcharPara("@action", 500, "GetOrderChallanWiseMultiUOMData");
           proc.AddVarcharPara("@SelectedComponentList", 2000, ChallanComponent);
           proc.AddVarcharPara("@SelectedComponentDetailsList", 2000, ComponentList);
           proc.AddVarcharPara("@ComponentType", 20, strType);
           dst = proc.GetTable();
           return dst;
       }

       public DataSet PopulateTaggingAllDetailInfo_New(string Action, string ComponentList, string InvoiceID, string strType)
       {
           DataSet dst = new DataSet();
           ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceList");
           proc.AddVarcharPara("@Action", 500, Action);
           proc.AddVarcharPara("@SelectedComponentList", 2000, ComponentList);
           proc.AddVarcharPara("@InvoiceID", 20, InvoiceID);
           proc.AddVarcharPara("@ComponentType", 20, strType);
           dst = proc.GetDataSet();
           return dst;
       }
       // End of Rev Sanchita
       public DataSet PurchaseInvoicePopulateTaggingAllDetailInfo_New(string Action, string ComponentList, string InvoiceID, string strType,string CustomerId,string Branch,string strDate)
       {
           DataSet dst = new DataSet();
           ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceList");
           proc.AddVarcharPara("@Action", 500, Action);
           proc.AddVarcharPara("@SelectedComponentList", 2000, ComponentList);
           proc.AddVarcharPara("@InvoiceID", 20, InvoiceID);
           proc.AddVarcharPara("@ComponentType", 20, strType);
           proc.AddVarcharPara("@CustomerId", 20, CustomerId);
           proc.AddVarcharPara("@Branch", 20, Branch);
           proc.AddVarcharPara("@PostingDate", 100, strDate);
           dst = proc.GetDataSet();
           return dst;
       }
       

       #endregion optimization By sam on 16102017
       public DataTable GetSelectedComponentProductList(string Action, string SelectedComponentList, string InvoiceID)
       {
           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 500, Action);
           proc.AddVarcharPara("@SelectedComponentList", 2000, SelectedComponentList);
           proc.AddVarcharPara("@InvoiceID", 20, InvoiceID);
           dt = proc.GetTable();
           return dt;
       }

       public int DeletePurchaseInvoice(string PurchaseInvoiceid,int Userid)
       {
           int i;
           int rtrnvalue = 0;
           ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoice");
           proc.AddVarcharPara("@Action", 100, "DeletePurchaseInvoice");
           proc.AddVarcharPara("@DeleteInvoiceId", 20, PurchaseInvoiceid);
           proc.AddIntegerPara("@UserID", Userid);
           proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
           i = proc.RunActionQuery();
           rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
           return rtrnvalue;

       }

       public int DeletePurchaseInvoiceDetails(string PurchaseInvoiceid, int Userid)
       {
           int i;
           int rtrnvalue = 0;
           ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoice_AddEdit");
           proc.AddVarcharPara("@Action", 100, "DeletePurchaseInvoice");
           proc.AddVarcharPara("@DeleteInvoiceId", 20, PurchaseInvoiceid);
           proc.AddIntegerPara("@UserID", Userid);
           proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
           i = proc.RunActionQuery();
           rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
           return rtrnvalue;

       }

       public int DeletePurchaseInvoiceInvoiceCumGRNDetails(string PurchaseInvoiceid, int Userid)
       {
           int i;
           int rtrnvalue = 0;
           ProcedureExecute proc = new ProcedureExecute("prc_InvoiceCumGRN_AddEdit");
           proc.AddVarcharPara("@Action", 100, "DeletePurchaseInvoice");
           proc.AddVarcharPara("@DeleteInvoiceId", 20, PurchaseInvoiceid);
           proc.AddIntegerPara("@UserID", Userid);
           proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
           i = proc.RunActionQuery();
           rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
           return rtrnvalue;

       }

       public int DeleteSelfInvoice(string PurchaseInvoiceid)
       {
           int i;
           int rtrnvalue = 0;
           ProcedureExecute proc = new ProcedureExecute("prc_SelfPurchaseInvoice");
           proc.AddVarcharPara("@Action", 100, "DeleteSelfInvoice");
           proc.AddVarcharPara("@DeleteInvoiceId", 20, PurchaseInvoiceid);
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
           ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetail");
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
           ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetail");
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
           ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 100, "PopulateVendorsDetail");
           dt = proc.GetTable();
           return dt;
       }

       public DataTable PopulateVendorsDetailByInventoryItem(string invtype)
       {
           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 100, "PopulateVendorsDetailByInventoryItem");
           proc.AddVarcharPara("@InventoryType", 2, invtype);
           dt = proc.GetTable();
           return dt;
       }

       public DataTable GetGRNOverhead(string Vendor, string Date, string branch, string ProjectID)
       {
           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_GRNOverheadCostDetail");
           proc.AddPara("@Vendor", Vendor);
           proc.AddPara("@ProjectID", ProjectID);
           proc.AddPara("@PostingDate", Convert.ToDateTime(Date));
           proc.AddPara("@BranchId", branch);
           dt = proc.GetTable();
           return dt;
       }

       public DataTable GetTdsSectionByID(string strPurchaseInvoiceID)
       {
           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetail");
           proc.AddVarcharPara("@Action", 500, "GetTdsSectionByID");
           proc.AddIntegerPara("@PurchaseInvoiceId", Convert.ToInt32(strPurchaseInvoiceID));
           dt = proc.GetTable();
           return dt;
       }
       /*Mantise work 24702 04.03.2022*/
       public int UpdatePartyINV(string InvoiceID, string PartyInvValue, string PartyInvDate)
       {
           int i;
           int rtrnvalue = 0;
           ProcedureExecute proc = new ProcedureExecute("prc_UpdatePartyInvNoDate");
           proc.AddVarcharPara("@Action", 100, "UpdatePartyINvDT");
           proc.AddBigIntegerPara("@DocId", Convert.ToInt32(InvoiceID));
           proc.AddPara("@PartyInvoiceNo", PartyInvValue);
           proc.AddVarcharPara("@PartyInvoiceDate", 50, PartyInvDate);
           proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
           i = proc.RunActionQuery();
           rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
           return rtrnvalue;
       }
       /*Clsoe of mantise work 24702 04.03.2022*/
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

       #endregion Transit Purchase Invoice






   }
}
