//=================================================Revision History=========================================================
//Rev 1.0      Priti      V2.0.38   30-05-2023     0026258:The Search Panel to be made available in the Product Popup in Purchase Order entry Module
//====================================================End Revision History=====================================================================
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BusinessLogicLayer
{
    public class PurchaseOrderBL
    {
        public DataTable GetComponantProduct(string poid)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList");
            proc.AddVarcharPara("@Action", 100, "LinkedProduct");
            proc.AddVarcharPara("@POID", 10, poid);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetSerialBatchID(string strBatchNo, string strSerialNo, string StockID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("proc_GetCentralized_BatchSerialID");
            proc.AddVarcharPara("@Action", 100, "");
            proc.AddVarcharPara("@Serial_Number", 100, strSerialNo);
            proc.AddVarcharPara("@Batch_Number", 100, strBatchNo);
            proc.AddVarcharPara("@StockID", 100, StockID);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetBranchIdByPOID(string poid)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList");
            proc.AddVarcharPara("@Action", 100, "GetBranchIdByPOID");
            proc.AddVarcharPara("@POID",10, poid);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable CheckPOTraanaction(string poid)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList");
            proc.AddVarcharPara("@Action", 100, "PurchaseOrderTagOrNotInPurchaseChallan");
            proc.AddIntegerPara("@DeletePOId", Convert.ToInt32(poid));
            dt = proc.GetTable();
            return dt;
        }
        public DataTable CheckPOTraanactionForPoINV(string poid)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList");
            proc.AddVarcharPara("@Action", 100, "PurchaseOrderTagOrNotInPurchaseInvoice");
            proc.AddIntegerPara("@DeletePOId", Convert.ToInt32(poid));
            dt = proc.GetTable();
            return dt;
        } 
        public DataTable GetVendorOutStanding(string Vendor)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList");
            proc.AddVarcharPara("@Action", 500, "GetVendorOutStanding");
            proc.AddVarcharPara("@VendorID", 500, Vendor);

            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetIndentTaggingMendatory()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList");
            proc.AddVarcharPara("@Action", 500, "GetIndentTaggingMendatory");           
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetVendorGSTIN(string Vendor)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList");
            proc.AddVarcharPara("@Action", 500, "GetGSTIN");
            proc.AddVarcharPara("@VendorID", 500, Vendor);

            dt = proc.GetTable();
            return dt;
        }
        public DataSet GetVendorDetails_CDRelated(string strVendorID)
        {
            DataSet dt = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList");
            proc.AddVarcharPara("@Action", 100, "GetCustomerInvoiceDetails");
            proc.AddVarcharPara("@InternalId", 1000, strVendorID);
            dt = proc.GetDataSet();
            return dt;
        }
        public DataSet GetAllDropDownDetailForPurchaseOrder()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList");
            proc.AddVarcharPara("@Action", 100, "GetAllDropDownDetailForPurchaseOrder");           
            ds = proc.GetDataSet();
            return ds;
        }
        public DataSet GetAllDropDownDetailForPurchaseOrder(string branchHierchy)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList");
            proc.AddVarcharPara("@Action", 100, "GetAllDropDownDetailForPurchaseOrder");
            proc.AddVarcharPara("@userbranchlist", -1, branchHierchy);
            ds = proc.GetDataSet();
            return ds;
        }
        public DataTable PopulateContactPersonOfCustomer(string InternalId)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList");
            proc.AddVarcharPara("@Action", 100, "PopulateContactPersonOfCustomer");
            proc.AddVarcharPara("@InternalId", 100, InternalId);
            ds = proc.GetTable();
            return ds;
        }
        public DataTable PopulateVendorCountryID(string InternalId)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList");
            proc.AddVarcharPara("@Action", 100, "VendorCountryID");
            proc.AddVarcharPara("@InternalId", 100, InternalId);
            ds = proc.GetTable();
            return ds;
        }
        public DataTable PopulateContactPersonPhone(string InternalId)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList");
            proc.AddVarcharPara("@Action", 100, "PopulateContactPersonPhone");
            proc.AddVarcharPara("@InternalId", 100, InternalId);
            ds = proc.GetTable();
            return ds;
        }
        public DataTable PopulateGSTCSTVAT(string quoteDate)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList");
            proc.AddVarcharPara("@Action", 100, "PopulateGSTCSTVATbyDate");
            proc.AddVarcharPara("@S_quoteDate", 10, quoteDate);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable PopulateGSTCSTVAT()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList");
            proc.AddVarcharPara("@Action", 100, "PopulateGSTCSTVAT");
            dt = proc.GetTable();
            return dt;
        }

        public int DeletePurchaseOrder(string poid)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList");
            proc.AddVarcharPara("@Action", 100, "DeletePurchaseOrderAll");
            proc.AddIntegerPara("@DeletePOId", Convert.ToInt32(poid));
            proc.AddVarcharPara("@ReturnValueDelete", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValueDelete"));
            return rtrnvalue;

        }

        public DataTable PopulateVendorsDetail()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList");
            proc.AddVarcharPara("@Action", 100, "PopulateVendorsDetail");
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetIndentOnPO(string OrderDate, string Status,string branch)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_GetIndentOnPurchaseOrder");           
            proc.AddDateTimePara("@OrderDate", Convert.ToDateTime(OrderDate));
            proc.AddVarcharPara("@Status", 50, Status);
           // proc.AddIntegerPara("@branch",Convert.ToInt32(branch));
            proc.AddVarcharPara("@branch", 500, branch);
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@campany_Id", 500, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            dt = proc.GetTable();
            return dt;
        }

        // Rev Sanchita
        public DataTable GetIndentOnPOVendoewise(string OrderDate, string Status, string branch, String strVendor)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_GetIndentOnPurchaseOrderVendoewise");
            proc.AddDateTimePara("@OrderDate", Convert.ToDateTime(OrderDate));
            proc.AddVarcharPara("@Status", 50, Status);
            proc.AddVarcharPara("@strVendor", 500, strVendor);
            proc.AddVarcharPara("@branch", 500, branch);
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@campany_Id", 500, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            dt = proc.GetTable();
            return dt;
        }
        // End of Rev Sanchita
        public DataTable GetQuotationOnPO(string OrderDate, string Status, string branch)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_GetQuotationOnPurchaseOrder");
            proc.AddDateTimePara("@OrderDate", Convert.ToDateTime(OrderDate));
            proc.AddVarcharPara("@Status", 50, Status);
            // proc.AddIntegerPara("@branch",Convert.ToInt32(branch));
            proc.AddVarcharPara("@branch", 500, branch);
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@campany_Id", 500, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            dt = proc.GetTable();
            return dt;
        }

        #region Get IndentRequisition  Date
        public DataTable GetIndentRequisitionDate(string Indent_No)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList");
            proc.AddVarcharPara("@Indent_RequisitionNumber", 50, Indent_No);
            proc.AddVarcharPara("@Action", 100, "GetIndentRequisitionDate");

            return proc.GetTable();
        }
        #endregion  Get IndentRequisition  Date

        public DataTable GetIndentDetailsFromPO(string Indent_Id, string Order_Key, string Product_Ids, string Action)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList");
            proc.AddVarcharPara("@Action", 100, "GetIndentDetailsOnly");
            proc.AddVarcharPara("@Indent_Id",4000,Indent_Id);
            proc.AddIntegerPara("@PurchaseOrder_Id", Convert.ToInt32(Order_Key));
            proc.AddVarcharPara("@Mode", 10, Action);
            return proc.GetTable();
        }
        //Rev 1.0
        public DataTable GetIndentDetails(string Indent_Id, string Order_Key, string Product_Ids, string Action)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList");
            proc.AddVarcharPara("@Action", 100, "GetIndentDetails");
            proc.AddVarcharPara("@Indent_Id", 4000, Indent_Id);
            proc.AddIntegerPara("@PurchaseOrder_Id", Convert.ToInt32(Order_Key));
            proc.AddVarcharPara("@Mode", 10, Action);
            return proc.GetTable();
        }
        public DataTable GetQuotationDetails(string Indent_Id, string Order_Key, string Product_Ids, string Action)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList");
            proc.AddVarcharPara("@Action", 100, "GetPurchaseQuotationDetails");
            proc.AddVarcharPara("@Indent_Id", 4000, Indent_Id);
            proc.AddIntegerPara("@PurchaseOrder_Id", Convert.ToInt32(Order_Key));
            proc.AddVarcharPara("@Mode", 10, Action);
            return proc.GetTable();
        }
        //Rev 1.0 End
        public DataTable GetQuotationDetailsFromPO(string Indent_Id, string Order_Key, string Product_Ids, string Action)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList");
            proc.AddVarcharPara("@Action", 100, "GetPurchaseQuotationDetailsOnly");
            proc.AddVarcharPara("@Indent_Id", 4000, Indent_Id);
            proc.AddIntegerPara("@PurchaseOrder_Id", Convert.ToInt32(Order_Key));
            proc.AddVarcharPara("@Mode", 10, Action);
            return proc.GetTable();
        }


        public DataSet GetIndentDetailsForPOGridBind(string Indent_Id, string Order_Key, string Product_Ids, string Action)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList");
            proc.AddVarcharPara("@Action", 100, "GetIndentDetailsForGridBind");
            proc.AddVarcharPara("@Indent_Id", 4000, Indent_Id);
            proc.AddVarcharPara("@IndentDetails_Id", 1000, Order_Key);
            proc.AddVarcharPara("@Product_Id", 1000, Product_Ids);
            proc.AddVarcharPara("@Mode", 10, Action);
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@campany_Id", 500, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            return proc.GetDataSet();
        }
        public DataSet GetPQDetailsForPOGridBind(string Indent_Id, string Order_Key, string Product_Ids, string Action)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList");
            proc.AddVarcharPara("@Action", 100, "GetPQDetailsForGridBind");
            proc.AddVarcharPara("@Indent_Id", 4000, Indent_Id);
            proc.AddVarcharPara("@IndentDetails_Id", 1000, Order_Key);
            proc.AddVarcharPara("@Product_Id", 1000, Product_Ids);
            proc.AddVarcharPara("@Mode", 10, Action);
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@campany_Id", 500, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            return proc.GetDataSet();
        }

        public DataTable GetIndentDetailsForMUltiUOMGridBind(string Indent_Id, string Order_Key, string Product_Ids)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList");
            proc.AddVarcharPara("@Action", 100, "GetIndentDetailsForMultiUOMGridBind");
            proc.AddVarcharPara("@SelectedQuotationtList", 4000, Indent_Id);
            proc.AddVarcharPara("@SelectedQuotationtdetailsList", 1000, Order_Key);
            proc.AddVarcharPara("@SelectedProductList", 1000, Product_Ids);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetPQDetailsForMUltiUOMGridBind(string Indent_Id, string Order_Key, string Product_Ids)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList");
            proc.AddVarcharPara("@Action", 100, "GetPQDetailsForMultiUOMGridBind");
            proc.AddVarcharPara("@SelectedQuotationtList", 4000, Indent_Id);
            proc.AddVarcharPara("@SelectedQuotationtdetailsList", 1000, Order_Key);
            proc.AddVarcharPara("@SelectedProductList", 1000, Product_Ids);
            dt = proc.GetTable();
            return dt;
        }

        // Rev Sanchita
        public DataSet GetIndentDetailsForPOGridBind_New(string Indent_Id, string Order_Key, string Product_Ids, string Action, string Vendor, string TaxOption, string shippingStateCode, string strBranch, string strDate)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList");
            proc.AddVarcharPara("@Action", 100, "GetIndentDetailsForGridBind_New");
            proc.AddVarcharPara("@Indent_Id", 4000, Indent_Id);
            proc.AddVarcharPara("@IndentDetails_Id", 1000, Order_Key);
            proc.AddVarcharPara("@Product_Id", 1000, Product_Ids);
            proc.AddVarcharPara("@Mode", 10, Action);
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@campany_Id", 500, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@ENTITY_ID", 20, Vendor);
            proc.AddVarcharPara("@TaxOption", 20, TaxOption);
            proc.AddVarcharPara("@SupplyState", 15, Convert.ToString(shippingStateCode));
            proc.AddVarcharPara("@branchId", 10, Convert.ToString(strBranch));
            proc.AddVarcharPara("@TaxDATE", 100, Convert.ToString(strDate));
            return proc.GetDataSet();
        }

        public DataSet GetPQDetailsForPOGridBind_New(string Indent_Id, string Order_Key, string Product_Ids, string Action)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList");
            proc.AddVarcharPara("@Action", 100, "GetPQDetailsForGridBind_New");
            proc.AddVarcharPara("@Indent_Id", 4000, Indent_Id);
            proc.AddVarcharPara("@IndentDetails_Id", 1000, Order_Key);
            proc.AddVarcharPara("@Product_Id", 1000, Product_Ids);
            proc.AddVarcharPara("@Mode", 10, Action);
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@campany_Id", 500, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            return proc.GetDataSet();
        }

        public DataTable GetIndentDetailsForMUltiUOMGridBind_New(string Indent_Id, string Order_Key, string Product_Ids)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList");
            proc.AddVarcharPara("@Action", 100, "GetIndentDetailsForMultiUOMGridBind_New");
            proc.AddVarcharPara("@SelectedQuotationtList", 4000, Indent_Id);
            proc.AddVarcharPara("@SelectedQuotationtdetailsList", 1000, Order_Key);
            proc.AddVarcharPara("@SelectedProductList", 1000, Product_Ids);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetPQDetailsForMUltiUOMGridBind_New(string Indent_Id, string Order_Key, string Product_Ids)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList");
            proc.AddVarcharPara("@Action", 100, "GetPQDetailsForMultiUOMGridBind_New");
            proc.AddVarcharPara("@SelectedQuotationtList", 4000, Indent_Id);
            proc.AddVarcharPara("@SelectedQuotationtdetailsList", 1000, Order_Key);
            proc.AddVarcharPara("@SelectedProductList", 1000, Product_Ids);
            dt = proc.GetTable();
            return dt;
        }
        // End of Rev Sanchita
        public DataTable GetCurrentConvertedRate(int BaseCurrencyId, int ConvertedCurrencyId, string CompID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList");
            proc.AddVarcharPara("@Action", 100, "GetCurrentConvertedRate");
            proc.AddIntegerPara("@BaseCurrencyId", BaseCurrencyId);
            proc.AddIntegerPara("@ConvertedCurrencyId", ConvertedCurrencyId);
            proc.AddVarcharPara("@campany_Id", 10, CompID);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable PopulatePODocApprovalByUserLevel(int userid, string doctype)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList");
            proc.AddVarcharPara("@Action", 500, "PopulatePODocApprovalByUserLevel");
            proc.AddIntegerPara("@UserId", userid);
            proc.AddVarcharPara("@DocType", 5, doctype);
            dt = proc.GetTable();
            return dt;
        }
    }
}
