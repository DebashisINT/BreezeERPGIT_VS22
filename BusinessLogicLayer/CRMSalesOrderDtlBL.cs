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
    public class CRMSalesOrderDtlBL
    {
        public DataTable GetBranchIdBySOID(string SOID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
            proc.AddVarcharPara("@Action", 500, "GetBranchIdBySOID");
            proc.AddIntegerPara("@Order_Id", Convert.ToInt32(SOID));
            dt = proc.GetTable();
            return dt;
        }

        #region Sales Order List Section Start
        public DataTable GetOrderListGridData(string Branch, string company,string StartDate,string EndDate)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
            proc.AddVarcharPara("@Action", 500, "SalesOrder");
            proc.AddVarcharPara("@userbranchlist", 500, Branch);
            proc.AddVarcharPara("@lastCompany", 500, company);
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddDateTimePara("@FinYearStartdate", Convert.ToDateTime(StartDate));
            proc.AddDateTimePara("@FinYearEnddate", Convert.ToDateTime(EndDate));
            dt = proc.GetTable();
            return dt;
        }

        public bool GetUserwiseDocumentFiltered(string user_Id)
        {

            DataTable dt = new DataTable();
            bool IsUserwise = false;
            ProcedureExecute proc = new ProcedureExecute("prc_GetIsUserwiseFiltered");
            proc.AddNVarcharPara("@User_Id", 20, user_Id);
            dt = proc.GetTable();
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dt.Rows[0]["user_IsUserwise"]))
                {
                    IsUserwise = true;
                }
            }

            return IsUserwise;
        }

        public DataTable GetOrderListGridDataBydate(string Branch, string company, DateTime StartDate, DateTime EndDate, string BranchId)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
            proc.AddVarcharPara("@Action", 500, "SalesOrderFilteredByDate");
            proc.AddVarcharPara("@userbranchlist", 500, Branch);
            proc.AddVarcharPara("@lastCompany", 500, company);
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddDateTimePara("@FromDate", StartDate);
            proc.AddDateTimePara("@ToDate", EndDate);
            proc.AddIntegerPara("@branchId", Convert.ToInt32(BranchId));
            dt = proc.GetTable();
            return dt;
        }


        public DataTable GetOrderListGridData(string Branch, string company,string Finyear)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
            proc.AddVarcharPara("@Action", 500, "SalesOrderOpening");
            proc.AddVarcharPara("@userbranchlist", 500, Branch);
            proc.AddVarcharPara("@lastCompany", 500, company);
            proc.AddVarcharPara("@FinYear", 500, Finyear);

            dt = proc.GetTable();
            return dt;
        } 

        public DataTable GetProductFifoValuation(int Product_id)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("GetFIFOValuation");
            proc.AddIntegerPara("@ProductId", Product_id);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetValueForProductFifoValuation(int Product_id, decimal Qty, string Val_Type,string Fromdate,string Todate,string Branch_Id)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_ProductValuation_Report");
            proc.AddIntegerPara("@PRODUCT_ID", Product_id);
            proc.AddVarcharPara("@COMPANYID", 50, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@USERID", 50, Convert.ToString(HttpContext.Current.Session["userbranchID"]));
            proc.AddVarcharPara("@FINYEAR", 50, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            //proc.AddVarcharPara("@FROMDATE", 10, dt_BTOut.Date.ToString("yyyy-MM-dd"));
            proc.AddVarcharPara("@FROMDATE", 10, Fromdate);
            proc.AddVarcharPara("@TODATE", 10, Todate);
            proc.AddVarcharPara("@BRANCHID", 10, Convert.ToString(Branch_Id));
            proc.AddVarcharPara("@VAL_TYPE", 10, Val_Type);
            proc.AddDecimalPara("@QTY", 3, 10, Qty);
            proc.AddIntegerPara("@CONSLANDEDCOST",1);
            proc.AddIntegerPara("@OWMASTERVALTECH",1);
            proc.AddIntegerPara("@CONSOVERHEADCOST", 1);
            dt = proc.GetTable();
            return dt;
        }


        public DataTable GetSalesChallanListGridData(string Branch, string company, string StartDate, string EndDate)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesChallan_Details");
            proc.AddVarcharPara("@Action", 500, "SalesChallan");
            proc.AddVarcharPara("@userbranchlist", 500, Branch);
            proc.AddVarcharPara("@lastCompany", 500, company);
            proc.AddDateTimePara("@FinYearStartdate", Convert.ToDateTime(StartDate));
            proc.AddDateTimePara("@FinYearEnddate", Convert.ToDateTime(EndDate));
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetSalesChallanListGridDataByDate(string Branch, string company, DateTime StartDate, DateTime EndDate, string BranchId)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesChallan_Details");
            proc.AddVarcharPara("@Action", 500, "SalesChallanFilteredByDate");
            proc.AddVarcharPara("@userbranchlist", 500, Branch);
            proc.AddVarcharPara("@lastCompany", 500, company);
            proc.AddDateTimePara("@FromDate", StartDate);
            proc.AddDateTimePara("@ToDate", EndDate);
            proc.AddIntegerPara("@branchId", Convert.ToInt32(BranchId));
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetSalesInvoiceOnCustomerDeliveryPending(string Branch, string company,string DlvType)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesChallan_Details");
            proc.AddVarcharPara("@Action", 500, DlvType);
            proc.AddVarcharPara("@userbranchlist", 2000, Branch);
            //proc.AddIntegerPara("@branchId", Convert.ToInt32(Branch));
            proc.AddVarcharPara("@lastCompany", 500, company);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetSalesInvoiceOnCustomerDeliveryPendingDatewise(string Branch, string company, string DlvType, string BranchID, DateTime FromDate, DateTime ToDate)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesChallan_Details");
            proc.AddVarcharPara("@Action", 500, DlvType);
            proc.AddVarcharPara("@userbranchlist", 3000, Branch);
            proc.AddIntegerPara("@branchId", Convert.ToInt32(BranchID));
            proc.AddDateTimePara("@FromDate", FromDate);
            proc.AddDateTimePara("@ToDate", ToDate);
            proc.AddVarcharPara("@lastCompany", 500, company);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetSalesInvoiceOnPendingDeliveryList(string Branch, string company)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesChallan_Details");
            proc.AddVarcharPara("@Action", 500, "PendingDeliveryList");
            proc.AddVarcharPara("@userbranchlist", 500, Branch);
            proc.AddVarcharPara("@lastCompany", 500, company);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetSalesInvoiceOnPendingDeliveryListByDateFiltering(string Branch, string company, string BranchID, DateTime FromDate, DateTime ToDate)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesChallan_Details");
            proc.AddVarcharPara("@Action", 500, "PendingDeliveryListByDate");
            proc.AddVarcharPara("@userbranchlist", 500, Branch);
            proc.AddVarcharPara("@branchId", 500, BranchID);
            proc.AddVarcharPara("@lastCompany", 500, company);
            proc.AddDateTimePara("@FromDate", FromDate);
            proc.AddDateTimePara("@ToDate", ToDate);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetSalesInvoiceOnUnDeliveryListByDateFiltering(string Branch, string company, string BranchID, DateTime FromDate, DateTime ToDate)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesChallan_Details");
            proc.AddVarcharPara("@Action", 500, "UndeliverySalesChllanByDate");
            proc.AddVarcharPara("@userbranchlist", 500, Branch);
            proc.AddVarcharPara("@branchId", 500, BranchID);
            proc.AddVarcharPara("@lastCompany", 500, company);
            proc.AddDateTimePara("@FromDate", FromDate);
            proc.AddDateTimePara("@ToDate", ToDate);
            dt = proc.GetTable();
            return dt;
        }
       

        public DataTable GetSerialataOnFIFOBasis(string WarehouseID, string BatchID, string Serial,string ProductId,string TotalId,string LastSerial)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("GetSerialOnFIFOBasis");
            proc.AddVarcharPara("@ProductID", 500, Convert.ToString(ProductId));
            proc.AddVarcharPara("@BatchID", 500, BatchID);
            proc.AddVarcharPara("@WarehouseID", 500, WarehouseID);
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@companyId", 500, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@Serial", 500, Serial);
            proc.AddVarcharPara("@LastSerial", 500, LastSerial);
            proc.AddVarcharPara("@TotalIds", 500, TotalId);
            dt = proc.GetTable();
            return dt;
        }

  

        public DataTable GetSerialataOnFIFOBasisForChallan(string WarehouseID, string BatchID, string Serial, string ProductId, string TotalId, string LastSerial)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("GetSerialOnFIFOBasisForChallan");
            proc.AddVarcharPara("@ProductID", 500, Convert.ToString(ProductId));
            proc.AddVarcharPara("@BatchID", 500, BatchID);
            proc.AddVarcharPara("@WarehouseID", 500, WarehouseID);
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@companyId", 500, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@Serial", 500, Serial);
            proc.AddVarcharPara("@LastSerial", 500, LastSerial);
            proc.AddVarcharPara("@TotalIds", 500, TotalId);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetSerialataOnFIFOBasisForBTOut(string WarehouseID, string BatchID, string Serial, string ProductId, string TotalId, string LastSerial)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("GetSerialOnFIFOBasisForBTOut");
            proc.AddVarcharPara("@ProductID", 500, Convert.ToString(ProductId));
            proc.AddVarcharPara("@BatchID", 500, BatchID);
            proc.AddVarcharPara("@WarehouseID", 500, WarehouseID);
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@companyId", 500, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@Serial", 500, Serial);
            proc.AddVarcharPara("@LastSerial", 500, LastSerial);
            proc.AddVarcharPara("@TotalIds", 500, TotalId);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetSerialataOnFIFOBasisForBTIn(string WarehouseID, string BatchID, string Serial, string ProductId, string TotalId, string LastSerial)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("GetSerialOnFIFOBasisForBTIn");
            proc.AddVarcharPara("@ProductID", 500, Convert.ToString(ProductId));
            proc.AddVarcharPara("@BatchID", 500, BatchID);
            proc.AddVarcharPara("@WarehouseID", 500, WarehouseID);
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@companyId", 500, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@Serial", 500, Serial);
            proc.AddVarcharPara("@LastSerial", 500, LastSerial);
            proc.AddVarcharPara("@TotalIds", 500, TotalId);
            dt = proc.GetTable();
            return dt;
        }

        public string ApproveRejectProject(string ApproveRemarks, int ApproveRejStatus, string OrderId)
        {
            string returnValue = "";
            ProcedureExecute proc = new ProcedureExecute("prc_ProjectOrder_Details");
              proc.AddVarcharPara("@Action", 200, "ApproveRejectDetails");
            proc.AddVarcharPara("@Project_ApproveRejectREmarks", 5000, ApproveRemarks);
            proc.AddIntegerPara("@Project_ApproveStatus", ApproveRejStatus);
            proc.AddVarcharPara("@Order_Id", 10, OrderId);
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            proc.RunActionQuery();
            returnValue = Convert.ToString(proc.GetParaValue("@ReturnValue"));
            return returnValue;
        }
        public string SalesInquiryApproveRejectProject(string ApproveRemarks, int ApproveRejStatus, string OrderId)
        {
            string returnValue = "";
            ProcedureExecute proc = new ProcedureExecute("Prc_SalesInquiryManupulation");
            proc.AddVarcharPara("@Action", 200, "ApproveRejectDetails");
            proc.AddVarcharPara("@Project_ApproveRejectREmarks", 5000, ApproveRemarks);
            proc.AddIntegerPara("@Project_ApproveStatus", ApproveRejStatus);
            proc.AddVarcharPara("@InquiryId", 10, OrderId);
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            proc.RunActionQuery();
            returnValue = Convert.ToString(proc.GetParaValue("@ReturnValue"));
            return returnValue;
        }

        public string PurchaseQuotationApproveRejectProject(string ApproveRemarks, int ApproveRejStatus, string PurchaseQuotationId)
        {
            string returnValue = "";
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseQuotation_Details");
            proc.AddVarcharPara("@Action", 200, "ApproveRejectDetails");
            proc.AddVarcharPara("@Project_ApproveRejectREmarks", 5000, ApproveRemarks);
            proc.AddIntegerPara("@Project_ApproveStatus", ApproveRejStatus);
            proc.AddVarcharPara("@InquiryId", 10, PurchaseQuotationId);
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            proc.RunActionQuery();
            returnValue = Convert.ToString(proc.GetParaValue("@ReturnValue"));
            return returnValue;
        }


        public DataTable DuplicateRevisionNumber(string RevNo,string Ordernumber)
        {
            DataTable DtRevNo = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_ProjectOrder_Details");
            proc.AddVarcharPara("@Action", 200, "RevisionNumberCheck");
            proc.AddVarcharPara("@Rev", 5000, RevNo);
            proc.AddVarcharPara("@OrderNumber", 100, Ordernumber);
            proc.RunActionQuery();
            DtRevNo = proc.GetTable();
            return DtRevNo;
        }


        public DataTable ApproveRejectProjectStatus(string OrderId)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_ProjectOrder_Details");
            proc.AddVarcharPara("@Action", 200, "GetApproveDetails");

           
            proc.AddVarcharPara("@Order_Id", 10, OrderId);

            dt = proc.GetTable();
            return dt;
        }
        public DataTable ApproveRejectSalesInquiryStatus(string InquiryId)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_SalesInquiryManupulation");
            proc.AddVarcharPara("@Action", 200, "GetApproveDetails");


            proc.AddVarcharPara("@InquiryId", 10, InquiryId);

            dt = proc.GetTable();
            return dt;
        }


        public DataTable ApproveRejectPurchaseQuoteStatus(string InquiryId)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseQuotation_Details");
            proc.AddVarcharPara("@Action", 200, "GetApproveDetails");


            proc.AddVarcharPara("@InquiryId", 10, InquiryId);

            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetStockOutListGridData(string Branch, string company)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_stockOut_Details");
            proc.AddVarcharPara("@Action", 500, "StockOut");
            proc.AddVarcharPara("@userbranchlist", 500, Branch);
            proc.AddVarcharPara("@lastCompany", 500, company);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetStockOutListGridDataByDate(string Branch, string company,DateTime FromDate,DateTime ToDate,string BranchId)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_stockOut_Details");
            proc.AddVarcharPara("@Action", 500, "StockOutFilteredBydate");
            proc.AddVarcharPara("@userbranchlist", 500, Branch);
            proc.AddVarcharPara("@lastCompany", 500, company);
            proc.AddDateTimePara("@FromDate", FromDate);
            proc.AddDateTimePara("@ToDate", ToDate);
            proc.AddVarcharPara("@branchId", 500, BranchId);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetIssueToServiceCenterListGridData(string Branch, string company)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_stockOut_Details");
            proc.AddVarcharPara("@Action", 500, "IssueToServiceCenter");
            proc.AddVarcharPara("@userbranchlist", 500, Branch);
            proc.AddVarcharPara("@lastCompany", 500, company);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetReceiveFromServiceCentreListGridData(string Branch, string company)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_stockOut_Details");
            proc.AddVarcharPara("@Action", 500, "ReceiveFromServiceCenter");
            proc.AddVarcharPara("@userbranchlist", 500, Branch);
            proc.AddVarcharPara("@lastCompany", 500, company);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetStockInListGridData(string Branch, string company)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_stockOut_Details");
            proc.AddVarcharPara("@Action", 500, "StockIn");
            proc.AddVarcharPara("@userbranchlist", 500, Branch);
            proc.AddVarcharPara("@lastCompany", 500, company);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetStockInListGridDataBydate(string Branch, string company, DateTime FromDate, DateTime ToDate, string BranchId)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_stockOut_Details");
            proc.AddVarcharPara("@Action", 500, "StockInFilteredBydate");
            proc.AddVarcharPara("@userbranchlist", 500, Branch);
            proc.AddVarcharPara("@lastCompany", 500, company);
            proc.AddDateTimePara("@FromDate", FromDate);
            proc.AddDateTimePara("@ToDate", ToDate);
            proc.AddVarcharPara("@branchId", 500, BranchId);
            dt = proc.GetTable();
            return dt;
        }
        public int DeleteSalesOrder(string SalesOrderid)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesOrder");
            proc.AddVarcharPara("@Action", 100, "SalesOrderDelete");
            proc.AddVarcharPara("@Order_Id",50, SalesOrderid);
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }


        public int DeleteProjectOrder(string SalesOrderid)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_ProjectOrder");
            proc.AddVarcharPara("@Action", 100, "SalesOrderDelete");
            proc.AddVarcharPara("@Order_Id", 50, SalesOrderid);
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }


        //Subhabrata on 01-03-2017
        public int DeleteSalesChallan(string SalesChallanid)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesChallan");
            proc.AddVarcharPara("@Action", 100, "SalesChallanDelete");
            proc.AddVarcharPara("@Challan_Id", 50, SalesChallanid);
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@CompanyID", 500, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;
            
        }//End

        public int DeleteBranchStockOut(string SalesChallanid,string Company,string FinYear)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_CRMBranchStockOut");
            proc.AddVarcharPara("@Action", 100, "BranchStockOutDelete");
            proc.AddVarcharPara("@Challan_Id", 50, SalesChallanid);
            proc.AddVarcharPara("@CompanyID", 50, Company);
            proc.AddVarcharPara("@FinYear", 50, FinYear);
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }

        public int UpdateReasonForCancellationOfBTO(string KeyVal,string Reason)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("Prc_InsertReasonForCancel");
            proc.AddVarcharPara("@Action", 100, "BTOCancelReason");
            proc.AddVarcharPara("@KeyVal", 50, KeyVal);
            proc.AddVarcharPara("@Reason", 50, Reason);
            proc.AddVarcharPara("@CancelledBy", 50, Convert.ToString(HttpContext.Current.Session["userid"]));
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }

        public int CancelBranchStockOut(string KeyVal)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("Prc_CancelBranchTransferOut");
            proc.AddVarcharPara("@Action", 100, "CancelBTO");
            proc.AddVarcharPara("@Document_Id", 50, KeyVal);
            proc.AddVarcharPara("@CompanyID", 50, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@FinYear", 50, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }

        public int CancelSalesOrder(string KeyVal,string Reason)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("Prc_CancelBranchTransferOut");
            proc.AddVarcharPara("@Action", 100, "CancelSalesOrder");
            proc.AddVarcharPara("@Document_Id", 50, KeyVal);
            proc.AddVarcharPara("@CompanyID", 50, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@FinYear", 50, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            proc.AddVarcharPara("@Reason", 50, Convert.ToString(Reason));
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }

        public int CancelPurchaseQuotation(string KeyVal, string Reason)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseQuotation_Details");
            proc.AddVarcharPara("@Action", 100, "CancelPurchaseQuotation");
            proc.AddVarcharPara("@Document_Id", 50, KeyVal);
            proc.AddVarcharPara("@companyId", 50, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@FinYear", 50, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            proc.AddVarcharPara("@Reason", 50, Convert.ToString(Reason));
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }

        public int CancelSalesInquiry(string KeyVal, string Reason)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("Prc_CancelBranchTransferOut");
            proc.AddVarcharPara("@Action", 100, "CancelSalesInquiry");
            proc.AddVarcharPara("@Document_Id", 50, KeyVal);
            proc.AddVarcharPara("@CompanyID", 50, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@FinYear", 50, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            proc.AddVarcharPara("@Reason", 50, Convert.ToString(Reason));
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }
        public int CancelProjectSalesOrder(string KeyVal, string Reason)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("Prc_CancelBranchTransferOut");
            proc.AddVarcharPara("@Action", 100, "CancelProjectSalesOrder");
            proc.AddVarcharPara("@Document_Id", 50, KeyVal);
            proc.AddVarcharPara("@CompanyID", 50, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@FinYear", 50, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            proc.AddVarcharPara("@Reason", 50, Convert.ToString(Reason));
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }

        public int CancelPurchaseOrder(string KeyVal, string Reason)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("Prc_CancelBranchTransferOut");
            proc.AddVarcharPara("@Action", 100, "CancelPurchaseOrder");
            proc.AddVarcharPara("@Document_Id", 50, KeyVal);
            proc.AddVarcharPara("@CompanyID", 50, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@FinYear", 50, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            proc.AddVarcharPara("@Reason", 50, Convert.ToString(Reason));
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }

        public int CancelProjectPurchaseOrder(string KeyVal, string Reason)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("Prc_CancelBranchTransferOut");
            proc.AddVarcharPara("@Action", 100, "CancelProjectPurchaseOrder");
            proc.AddVarcharPara("@Document_Id", 50, KeyVal);
            proc.AddVarcharPara("@CompanyID", 50, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@FinYear", 50, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            proc.AddVarcharPara("@Reason", 50, Convert.ToString(Reason));
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }

        public int ClosedSalesOrder(string KeyVal, string Reason)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("Prc_CancelBranchTransferOut");
            proc.AddVarcharPara("@Action", 100, "ClosedSalesOrder");
            proc.AddVarcharPara("@Document_Id", 50, KeyVal);
            proc.AddVarcharPara("@CompanyID", 50, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@FinYear", 50, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            proc.AddVarcharPara("@Reason", 50, Convert.ToString(Reason));
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }

        public int ClosedPurchaseOrder(string KeyVal, string Reason)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("Prc_CancelBranchTransferOut");
            proc.AddVarcharPara("@Action", 100, "ClosedPurchaseOrder");
            proc.AddVarcharPara("@Document_Id", 50, KeyVal);
            proc.AddVarcharPara("@CompanyID", 50, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@FinYear", 50, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            proc.AddVarcharPara("@Reason", 50, Convert.ToString(Reason));
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }

        public int ClosedProjectPurchaseOrder(string KeyVal, string Reason)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("Prc_CancelBranchTransferOut");
            proc.AddVarcharPara("@Action", 100, "ClosedProjectPurchaseOrder");
            proc.AddVarcharPara("@Document_Id", 50, KeyVal);
            proc.AddVarcharPara("@CompanyID", 50, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@FinYear", 50, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            proc.AddVarcharPara("@Reason", 50, Convert.ToString(Reason));
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }


        public int UpdateManualBRSList(string VoucherNo, string Type, string ValueDate, string InstrumentNo, string InstrumentDate, string VoucherDate)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("UpdateManualBRSList");

            proc.AddVarcharPara("@VoucherNumber", 50, VoucherNo);
            proc.AddVarcharPara("@Module_Type", 50, Type);
            proc.AddVarcharPara("@CreatedBy", 50, Convert.ToString(HttpContext.Current.Session["userid"]));
            proc.AddDateTimePara("@ValueDate", DateTime.ParseExact(ValueDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture));
            proc.AddVarcharPara("@InstrumentNo", 150, InstrumentNo);
            proc.AddDateTimePara("@Instrumentdate", DateTime.ParseExact(InstrumentDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture));
            proc.AddDateTimePara("@VoucherDate", DateTime.ParseExact(VoucherDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture));
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }

        public int IsBankValueDateValid(string VoucherNo, string ValueDate, string Type)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_GetIsBankValueDateIsValid");

            proc.AddVarcharPara("@VoucherNumber", 50, VoucherNo);
            proc.AddVarcharPara("@Module_Type", 50, Type);
            proc.AddDateTimePara("@ValueDate", DateTime.ParseExact(ValueDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture));
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }


        public int DeleteIssueToService(string SalesChallanid, string Company, String Finyear)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_CRMIssueToServiceCenter");
            proc.AddVarcharPara("@Action", 100, "DeleteIssueToServiceCenter");
            proc.AddVarcharPara("@Challan_Id", 50, SalesChallanid);
            proc.AddVarcharPara("@CompanyID", 50, Company);
            proc.AddVarcharPara("@FinYear", 50, Finyear);
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }

        public int DeleteReceivedFromService(string SalesChallanid,string Company, String Finyear)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_CRMIssueToServiceCenter");
            proc.AddVarcharPara("@Action", 100, "ReceivedFromServiceCentre");
            proc.AddVarcharPara("@Challan_Id", 50, SalesChallanid);
            proc.AddVarcharPara("@CompanyID", 50, Company);
            proc.AddVarcharPara("@FinYear", 50, Finyear);
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }
        public int DeleteBranchStockIn(string SalesChallanid, string Company, String Finyear)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_CRMBranchStockOut");
            proc.AddVarcharPara("@Action", 100, "BranchStockInDelete");
            proc.AddVarcharPara("@Challan_Id", 50, SalesChallanid);
            proc.AddVarcharPara("@CompanyID", 50, Company);
            proc.AddVarcharPara("@FinYear", 50, Finyear);
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;
        }


        #endregion Sales Order List Section
        #region Sales Order Quotation Section Start
        public int SalesOrderEditablePermission(int userid)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
            proc.AddVarcharPara("@Action", 100, "SalesOrderEditablePermission");
            proc.AddIntegerPara("@userid", userid);
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }

        public int GetIdFromSalesInvoiceExists(string ChallanID)
        {
            DataTable dt = new DataTable();
            int i = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_GetChallanInvoiceDetails");
            proc.AddVarcharPara("@Action", 500, "IsChallanIdExistInInvoice");
            proc.AddVarcharPara("@Order_Id", 500, Convert.ToString(ChallanID));
            dt = proc.GetTable();
            if (dt!=null && dt.Rows.Count>0)
            {
                if (Convert.ToInt32(dt.Rows[0]["InvoiceDetails_Id"]) > 0)
                {
                    i = 1;
                }
            }
        
            return i;
        }

        public int GetIdFromInvoiceOrChallan(string ChallanID)
        {
            DataTable dt = new DataTable();
            int i = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_GetChallanInvoiceDetails");
            proc.AddVarcharPara("@Action", 500, "IsSalesOrderIdExistInChallanOrInvoice");
            proc.AddVarcharPara("@Order_Id", 500, Convert.ToString(ChallanID));
            dt = proc.GetTable();
            if (dt != null && dt.Rows.Count > 0)
            {
                if (Convert.ToInt32(dt.Rows[0]["MatchQty"]) > 0)
                {
                    i = 1;
                }
            }

            return i;
        }

        public int GetIdForCustomerDeliveryPendingExists(string ChallanID)
        {
            DataTable dt = new DataTable();
            int i = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_GetChallanInvoiceDetails");
            proc.AddVarcharPara("@Action", 500, "IsCustomerDeliveryPendingDelivered");
            proc.AddVarcharPara("@Order_Id", 500, Convert.ToString(ChallanID));
            dt = proc.GetTable();
            if (dt != null && dt.Rows.Count > 0)
            {
                if (Convert.ToInt32(dt.Rows[0]["OUTPUT"]) > 0)
                {
                    i = 1;
                }
            }

            return i;
        }

        public int GetIdFromReceivedFromServiceExists(string ServiceId)
        {
            DataTable dt = new DataTable();
            int i = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_GetChallanInvoiceDetails");
            proc.AddVarcharPara("@Action", 500, "IsReceivedFromServiceExists");
            proc.AddVarcharPara("@Order_Id", 500, Convert.ToString(ServiceId));
            dt = proc.GetTable();
            if (dt != null && dt.Rows.Count > 0)
            {
                if (Convert.ToInt32(dt.Rows[0]["ServiceDetails_Id"]) > 0)
                {
                    i = 1;
                }
            }

            return i;
        }

        public int GetIdFromSalesInvoiceExistsInOrder(string ServiceId)
        {
            DataTable dt = new DataTable();
            int i = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_GetChallanInvoiceDetails");
            proc.AddVarcharPara("@Action", 500, "IsOrderExistsInInvoice");
            proc.AddVarcharPara("@Order_Id", 500, Convert.ToString(ServiceId));
            dt = proc.GetTable();
            if (dt != null && dt.Rows.Count > 0)
            {
                if (Convert.ToInt32(dt.Rows[0]["InvoiceCreatedFromDocID"]) > 0)
                {
                    i = 1;
                }
            }

            return i;
        }

        public int GetIdFromBIExists(string ServiceId)
        {
            DataTable dt = new DataTable();
            int i = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_GetChallanInvoiceDetails");
            proc.AddVarcharPara("@Action", 500, "IsBOFromBIExists");
            proc.AddVarcharPara("@Order_Id", 500, Convert.ToString(ServiceId));
            dt = proc.GetTable();
            if (dt != null && dt.Rows.Count > 0)
            {
                if (Convert.ToInt32(dt.Rows[0]["StkDetails_Id"]) > 0)
                {
                    i = 1;
                }
            }

            return i;
        }

        public string GetCustomerDormantOrNot(string SalesID)
        {

            DataTable dt = new DataTable();
            string statusType = string.Empty;
            ProcedureExecute proc = new ProcedureExecute("getCustomerDormant");
            proc.AddVarcharPara("@SalesID", 500, Convert.ToString(SalesID));
            dt = proc.GetTable();
            if (dt != null && dt.Rows.Count > 0)
            {
                statusType = Convert.ToString(dt.Rows[0]["Statustype"]);
            }

            return statusType;
        }

        public string GetInvoiceCustomerId(int KeyVal)
        {
            string Cust_Id = string.Empty;
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("GetCustomerId");
            proc.AddVarcharPara("@Action", 100, "SalesOrderEditablePermission");
            proc.AddIntegerPara("@Key_Val", KeyVal);
            
            dt = proc.GetTable();
            Cust_Id = dt.Rows[0].Field<string>("Customer_Id") + "~" + Convert.ToString(dt.Rows[0].Field<DateTime>("Invoice_Date"));
            return Cust_Id;

        }

        public DataTable GetSalesOrderStatusByOrderID(string SalesOrder_Id)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
            proc.AddNVarcharPara("@action", 150, "GetSalesOrderStatusByOrderID");
            proc.AddIntegerPara("@SalesOrder_Id", Convert.ToInt32(SalesOrder_Id));
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetCustVendHistoryId(string Cnt_Id)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_getCustVendHistoryDetails");
            proc.AddNVarcharPara("@ACTION", 150, "CustVendHistoryDetails");
            proc.AddIntegerPara("@cnt_Id", Convert.ToInt32(Cnt_Id));
            dt = proc.GetTable();
            return dt;
        }


        #endregion Sales Order Section End

        #region Sales Order Address Section
        public int InsertProduct(string Action, int OrderAdd_OrderId, string companyId, int @S_OrderAdd_BranchId, string fin_year, string contactperson, string AddressType, string address1, string address2, string address3, string landmark, int country, int State, int city, string pin, int area)
        {

            ProcedureExecute proc;
            try
            {
                using (proc = new ProcedureExecute("prc_SalesOrder_Details"))
                {
                    proc.AddVarcharPara("@Action", 100, Action);
                    proc.AddIntegerPara("@S_SalesOrderAdd_OrderId", OrderAdd_OrderId);
                    proc.AddVarcharPara("@S_SalesOrderAdd_CompanyID", 10, companyId);
                    proc.AddIntegerPara("@S_SalesOrderAdd_BranchId", @S_OrderAdd_BranchId);
                    proc.AddVarcharPara("@S_SalesOrderAdd_FinYear", 1, fin_year);
                    proc.AddVarcharPara("@S_SalesOrderAdd_ContactPerson", 50, contactperson);
                    proc.AddVarcharPara("@S_SalesOrderAdd_addressType", 50, AddressType);
                    proc.AddVarcharPara("@S_SalesOrderAdd_address1", 500, address1);
                    proc.AddVarcharPara("@S_SalesOrderAdd_address2", 500, address2);
                    proc.AddVarcharPara("@S_SalesOrderAdd_address3", 500, address3);
                    proc.AddVarcharPara("@S_SalesOrderAdd_landMark", 500, landmark);

                    proc.AddIntegerPara("@S_SalesOrderAdd_countryId", country);
                    proc.AddIntegerPara("@S_SalesOrderAdd_stateId", State);
                    proc.AddIntegerPara("@S_SalesOrderAdd_cityId", city);
                    proc.AddVarcharPara("@S_SalesOrderAdd_pin", 12, pin);
                    proc.AddIntegerPara("@S_SalesOrderAdd_areaId", area);
                    proc.AddIntegerPara("@S_SalesOrderAdd_CreatedUser", Convert.ToInt32(HttpContext.Current.Session["userid"]));

                    //End here 04-01-2017

                    int NoOfRowEffected = proc.RunActionQuery();
                    return NoOfRowEffected;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                proc = null;
            }
        }

        #endregion Address Section

        #region Sales invoice number
        public DataTable PopulateSalesInvoiceNumberByOrderId(string strOrderID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
            proc.AddVarcharPara("@Action", 50, "PopulateSalesInvoiceNumberByOrderId");
            proc.AddVarcharPara("@Order_Id", 20, strOrderID);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable PendingDeliveryProductDetailsOngrid(string InvoiceID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesChallan_Details");
            proc.AddVarcharPara("@Action", 50, "PendingDeliveryProductDetails");
            proc.AddVarcharPara("@InvoiceId", 20, InvoiceID);
            dt = proc.GetTable();
            return dt;
        }

        #endregion Sales invoice number

        public void DeleteBasketDetailsFromtable(string basketId, int userId)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_PosSalesInvoice");
            proc.AddVarcharPara("@Action", 100, "DeleteBasketDetails");
            proc.AddIntegerPara("@SalesOrder_Id", Convert.ToInt32(basketId));
            proc.AddIntegerPara("@UserID", userId);
            proc.GetTable();

        }
        public int GetorderCount(string branchList)
        {
            ProcedureExecute proc;
            DataTable basketWaitingTable = new DataTable();
            int count = 0;
            try
            {


                using (proc = new ProcedureExecute("prc_posListingDetails"))
                {

                    proc.AddVarcharPara("@Action", 50, "GetOrderWaitingCount");
                    proc.AddVarcharPara("@BranchList", 1000, branchList);
                    basketWaitingTable = proc.GetTable();
                    if (basketWaitingTable.Rows.Count > 0)
                    {
                        count = Convert.ToInt32(basketWaitingTable.Rows[0][0]);
                    }
                    return count;

                }
            }

            catch (Exception ex)
            {
                return count;
            }

            finally
            {
                proc = null;
            }

        }


        public DataSet GetOrderBusketById(int id)
        {
            DataSet basketDetails = new DataSet();
            ProcedureExecute proc;
            try
            {


                using (proc = new ProcedureExecute("prc_SalesOrder_Details"))
                {
                    proc.AddVarcharPara("@Action", 50, "GetBasketProductDetails");
                    proc.AddIntegerPara("@SalesOrder_Id", id);
                    basketDetails = proc.GetDataSet();
                    return basketDetails;

                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                proc = null;
            }

        }




        public DataTable SalesOrderBasketDetails(string branchList)
        {
            ProcedureExecute proc;
            DataTable basketTable = new DataTable();
            try
            {


                using (proc = new ProcedureExecute("prc_OrderBasketDetail"))
                {
                    //  int i = proc.RunActionQuery();
                    proc.AddVarcharPara("@branchHierchy", 1000, branchList);
                    basketTable = proc.GetTable();

                    return basketTable;

                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                proc = null;
            }
        }
        //Rev work start 12.07.2022 mantise no :0025011: Update E-way Bill
        public int UpdateEWayBillFor(string Stk_Id, string EWayBillNumber, string EWayBillDate)
        {
            try
            {
                int i;
                int rtrnvalue = 0;
                ProcedureExecute proc = new ProcedureExecute("prc_UpdateEwayBillNo");
                proc.AddVarcharPara("@Action", 100, "UpdateEWayBill");
                proc.AddVarcharPara("@Module", 100, "BranchtransferOut");
                proc.AddBigIntegerPara("@Stk_Id", Convert.ToInt32(Stk_Id));
                proc.AddVarcharPara("@EWayBillNumber", 50, EWayBillNumber);
                proc.AddVarcharPara("@EWayBillDate", 50, EWayBillDate);
                proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
                i = proc.RunActionQuery();
                rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
                return rtrnvalue;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        //Rev work close 12.07.2022 mantise no :0025011: Update E-way Bill
    }
}
