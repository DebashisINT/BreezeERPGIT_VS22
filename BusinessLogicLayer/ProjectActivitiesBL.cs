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
    public class ProjectActivitiesBL
    {
        #region Sale Quotation

        public DataSet GetAllDropDownDetailForSalesQuotation(string @userbranch)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesActivity");
            proc.AddVarcharPara("@Action", 100, "GetAllDropDownDetailForSalesQuotation");
            proc.AddVarcharPara("@userbranch", 4000, @userbranch);
            ds = proc.GetDataSet();
            return ds;
        }

        //Added By:Subhabrata on 02-02-2017

        //Subhabrata:For Sales order
        public DataSet GetAllDropDownDetailForSalesOrder(string UserBranch)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesActivity");
            proc.AddVarcharPara("@Action", 100, "GetAllDropDownDetailForSalesOrder");
            proc.AddVarcharPara("@userbranch", 4000, UserBranch);
            ds = proc.GetDataSet();
            return ds;
        }
        //Subhabrata:For sales Challan
        public DataSet GetAllDropDownDetailForProjectChallan(string UserBranch)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesActivity");
            proc.AddVarcharPara("@Action", 100, "GetAllDropDownDetailForProjectChallan");
            proc.AddVarcharPara("@userbranch", 4000, UserBranch);
            ds = proc.GetDataSet();
            return ds;
        }
        //End
        public DataTable PopulateCustomerDetail()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesActivity");
            proc.AddVarcharPara("@Action", 100, "PopulateCustomerDetail");
            dt = proc.GetTable();
            return dt;
        }

        public DataTable PopulateCustomerDetailOD(string CustomerId)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesActivity");
            proc.AddVarcharPara("@Action", 100, "PopulateCustomerDetailForODSD");
            proc.AddVarcharPara("@CustomerInternalId", 100, CustomerId);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable PopulateCustomerDetailForCRM()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesActivity");
            proc.AddVarcharPara("@Action", 100, "PopulateCustomerDetailCRM");
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetCustomerbudgetData(string AssignedID, string FinYear)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("sp_Sales");
            proc.AddVarcharPara("@Mode", 100, "GetSalesmanBudgetInfo");
            proc.AddDecimalPara("@AssignedID", 2, 18, Convert.ToDecimal(AssignedID));
            proc.AddVarcharPara("@Finyear", 100, FinYear);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable PopulateContactPersonOfCustomer(string InternalId)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesActivity");
            proc.AddVarcharPara("@Action", 100, "PopulateContactPersonOfCustomer");
            proc.AddVarcharPara("@InternalId", 100, InternalId);
            ds = proc.GetTable();
            return ds;
        }
        public DataTable PopulateSalesManAgent(string InternalId)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesActivity");
            proc.AddVarcharPara("@Action", 100, "PopulateSalesManAgentById");
            proc.AddVarcharPara("@InternalId", 100, InternalId);
            ds = proc.GetTable();
            return ds;
        }

        public DataTable PopulateBranchOnOD(string Branch)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesActivity");
            proc.AddVarcharPara("@Action", 100, "PopulateBranchOnOD");
            proc.AddVarcharPara("@BranchID", 100, Branch);
            ds = proc.GetTable();
            return ds;
        }



        public DataTable PopulateContactPersonForCustomerOrLead(string internalId)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesActivity");
            proc.AddVarcharPara("@Action", 100, "PopulateContactPersonForCustomerOrLead");
            proc.AddVarcharPara("@LeadContactId", 10, internalId);
            dt = proc.GetTable();
            return dt;
        }



        public DataTable GetFinacialYearBasedQouteDate(string FinYear)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesActivity");
            proc.AddVarcharPara("@Action", 100, "GetFinacialYearBasedQouteDate");
            proc.AddVarcharPara("@FinYear", 15, FinYear);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetCurrentConvertedRate(int BaseCurrencyId, int ConvertedCurrencyId, string CompID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesActivity");
            proc.AddVarcharPara("@Action", 100, "GetCurrentConvertedRate");
            proc.AddIntegerPara("@BaseCurrencyId", BaseCurrencyId);
            proc.AddIntegerPara("@ConvertedCurrencyId", ConvertedCurrencyId);
            proc.AddVarcharPara("@CompanyId", 10, CompID);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetChallanIdOnTransporter(string Doc_id)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("GetTransporterDetailsOnChallanId");
            proc.AddVarcharPara("@Action", 100, "PendingDelivery");
            proc.AddIntegerPara("@Doc_Id", Convert.ToInt32(Doc_id));
            proc.AddVarcharPara("@Doc_type", 100, "SC");
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetConfigSettingsFIFOWise(string VariableName)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("GetConfigSettingsSerialOnFIFOBasis");
            proc.AddVarcharPara("@Variable_Name", 200, VariableName);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetConfigSettingsOnCreditDaysBasis(string VariableName)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("GetConfigSettingsCreditDays");
            proc.AddVarcharPara("@Variable_Name", 200, VariableName);
            dt = proc.GetTable();
            return dt;
        }


        public DataTable GetBranchDetailsOFReq(string ReqNo)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("GetBranchDetails");
            proc.AddVarcharPara("@Action", 100, "BO"); //@DocumentNo
            proc.AddIntegerPara("@DocumentNo", Convert.ToInt32(ReqNo));
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetSameBranchIds(string ReqNo)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("GetSameBranchId");
            proc.AddVarcharPara("@Action", 100, "BR"); //@DocumentNo
            proc.AddVarcharPara("@ReqIds", 100, ReqNo);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetBranchDetailsOFBranchOut(string BranchOut)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("GetBranchDetails");
            proc.AddVarcharPara("@Action", 100, "BI");
            proc.AddIntegerPara("@DocumentNo", Convert.ToInt32(BranchOut));
            dt = proc.GetTable();
            return dt;
        }




        public DataTable GetConatctReferenceSalesManInfo(string KeyVal)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
            proc.AddVarcharPara("@Action", 100, "GetSalesManHistory");
            proc.AddVarcharPara("@OrderNumber", 50, KeyVal);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetConatctReferenceSalesManInfoInvoice(string KeyVal)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
            proc.AddVarcharPara("@Action", 100, "GetSalesManHistoryForInvoice");
            proc.AddVarcharPara("@BillNumber", 50, KeyVal);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetDriverNamePhNo(int cnt_Id)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("GetPhoneNoAndDriverName");
            proc.AddIntegerPara("@cnt_Id", cnt_Id);

            dt = proc.GetTable();
            return dt;
        }

        public DataTable PopulateGSTCSTVAT(string quoteDate)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesActivity");
            proc.AddVarcharPara("@Action", 100, "PopulateGSTCSTVATbyDate");
            proc.AddVarcharPara("@S_quoteDate", 10, quoteDate);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable PopulateGSTCSTVAT()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesActivity");
            proc.AddVarcharPara("@Action", 100, "PopulateGSTCSTVAT");
            dt = proc.GetTable();
            return dt;
        }

        #endregion Sale Quotation
        public DataTable PopulateCallDispositionDetailForPhoneCallOfSalesActivities()
        {
            ProcedureExecute proc = new ProcedureExecute("prc_SalesActivity");
            proc.AddVarcharPara("@Action", 100, "PopulateCallDispositionDetailForPhoneCallOfSalesActivities");
            return proc.GetTable();
        }

        public DataTable GetRateDetails(string productid)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesOrder");
            proc.AddVarcharPara("@Action", 100, "LastSalesRate");
            proc.AddVarcharPara("@ProdID", 100, productid);
            return proc.GetTable();
        }


        #region Sales Visit
        public DataTable PopulateBranchList()
        {
            ProcedureExecute proc = new ProcedureExecute("prc_SalesActivity");
            proc.AddVarcharPara("@Action", 100, "PopulateBranchList");
            return proc.GetTable();
        }

        #endregion Sales Visit

        public DataTable GetPhoneCallIdToShowDetail(int transSaleId)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_SalesActivity");
            proc.AddVarcharPara("@Action", 100, "GetPhoneCallIdToShowDetail");
            proc.AddIntegerPara("@trsid", transSaleId);
            return proc.GetTable();
        }

        public DataSet GetCustomerDetails_CDRelated(string strCustomerID)
        {
            DataSet dt = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesActivity");
            proc.AddVarcharPara("@Action", 100, "GetCustomerDetails");
            proc.AddVarcharPara("@InternalId", 1000, strCustomerID);
            dt = proc.GetDataSet();
            return dt;
        }

        public DataTable GetEditablePermissionOfSupervisorInActivity(int transSaleId, string cntid)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_SalesActivity");
            proc.AddVarcharPara("@Action", 100, "GetEditablePermissionOfSupervisorInActivity");
            proc.AddIntegerPara("@trsid", transSaleId);

            proc.AddDecimalPara("@Cnt_id", 2, 18, Convert.ToDecimal(cntid));
            return proc.GetTable();
        }

        public DataTable GetQuotationDetailsFromSalesOrder(string Quote_Nos, string Order_Key, string Product_Ids, string Action)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_GetQuotationDetails");
            proc.AddVarcharPara("@Mode", 100, "GetQuotationDetailsFromSalesOrder");
            proc.AddVarcharPara("@Quote_Id", 4000, Quote_Nos);
            proc.AddVarcharPara("@QuoteDetails_Id", 1000, Order_Key);
            proc.AddVarcharPara("@Product_Id", 1000, Product_Ids);
            proc.AddVarcharPara("@Action", 10, Action);
            return proc.GetTable();
        }


        public DataSet GetQuotationDetailsFromProjectOrder(string Quote_Nos, string Order_Key, string Product_Ids, string Action)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Prc_GetProjectQuotationDetails");
            proc.AddVarcharPara("@Mode", 100, "GetQuotationDetailsFromProjectOrder");
            proc.AddVarcharPara("@Quote_Id", 4000, Quote_Nos);
            proc.AddVarcharPara("@QuoteDetails_Id", 1000, Order_Key);
            proc.AddVarcharPara("@Product_Id", 1000, Product_Ids);
            proc.AddVarcharPara("@Action", 10, Action);
            ds = proc.GetDataSet();
            return ds;
        }



        //Subhabrata
        public DataTable GetBalQuantityForQuantiyCheck(string Id, string Pro_Id, string Status)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_getBalQuantityForQuantityCheck");
            proc.AddVarcharPara("@Action", 100, Status);
            proc.AddVarcharPara("@Id", 4000, Id);
            proc.AddVarcharPara("@Product_Id", 1000, Pro_Id);
            return proc.GetTable();
        }
        //End

        public DataTable PopulateSupervisorHistory(string ActivityId)
        {
            try
            {
                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("sp_Sales");
                proc.AddVarcharPara("@Mode", 50, "SupervisorHistory");
                proc.AddIntegerPara("@ActivityId", Convert.ToInt32(ActivityId));

                dt = proc.GetTable();

                return dt;
            }
            catch
            {
                return null;
            }
        }
        public DataTable GetQuotationDetailsFromSalesOrder_PhoneCall(string Product_Ids)
        {
            ProcedureExecute proc = new ProcedureExecute("Sp_GetProductList_frmSales");


            proc.AddVarcharPara("@Products", 1000, Product_Ids);
            return proc.GetTable();
        }

        public DataTable GetQuotationDetailsFromSalesOrderonly(string Quote_Nos, string Order_Key, string Product_Ids)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_GetQuotationDetails");
            proc.AddVarcharPara("@Mode", 100, "GetQuotationDetailsOnly");
            proc.AddVarcharPara("@Quote_Id", 4000, Quote_Nos);
            proc.AddVarcharPara("@OrderID_Key", 1000, Order_Key);
            return proc.GetTable();
        }


        public DataTable GetCustomerOutstandingAmount(string Datetime, string CustomerId, string BranchId)
        {
            ProcedureExecute proc = new ProcedureExecute("PRC_PARTYWISEOUTSTANDINGENTRY_REPORT");
            proc.AddVarcharPara("@COMPANYID", 100, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@FINYEAR", 4000, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@ASONDATE", 1000, Datetime);
            proc.AddVarcharPara("@BRANCHID", 4000, "");
            proc.AddVarcharPara("@PARTY_CODE", 1000, CustomerId);
            proc.AddVarcharPara("@PARTY_TYPE", 4000, "C");
            //proc.AddIntegerPara("@INCLUDECB", 0);
            //proc.AddIntegerPara("@INCLUDEJV", 0);
            proc.AddIntegerPara("@INCLUDECB", 1);
            proc.AddIntegerPara("@INCLUDEJV", 1);
            return proc.GetTable();
        }

        public DataTable GetCustomerOutstandingRecords(string Datetime, string CustomerId, string BranchId)
        {
            ProcedureExecute proc = new ProcedureExecute("PRC_PARTYWISEOUTSTANDINGDET_REPORT");
            proc.AddVarcharPara("@COMPANYID", 100, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@FINYEAR", 4000, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@ASONDATE", 1000, Datetime);
            proc.AddVarcharPara("@ONDATE", 100, "B");
            proc.AddIntegerPara("@DUEDATE", 1);
            proc.AddIntegerPara("@ALLPARTY", 0);
            proc.AddIntegerPara("@PRINTDAYS", 1);
            proc.AddIntegerPara("@SHOWSALESMAN", 1);
            proc.AddVarcharPara("@BRANCHID", 4000, "");
            proc.AddVarcharPara("@PARTY_CODE", 1000, CustomerId);
            proc.AddVarcharPara("@PARTY_TYPE", 4000, "C");
            //proc.AddIntegerPara("@INCLUDECB",0);
            //proc.AddIntegerPara("@INCLUDEJV",0);
            proc.AddIntegerPara("@INCLUDECB", 1);
            proc.AddIntegerPara("@INCLUDEJV", 1);
            proc.AddIntegerPara("@EXCLUDEDNCN", 0);
            proc.AddVarcharPara("@USERID", 1000, Convert.ToString(HttpContext.Current.Session["userid"]));
            return proc.GetTable();
        }
        //Subhabrata for sales Challan
        public DataTable GetSalesOrderDetailsFromProjectChallanonly(string Order_nos, string Order_Key, string Product_Ids)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_GetOrderDetailsForSaleChallan");
            proc.AddVarcharPara("@Mode", 100, "GetOrderProductDetailsOnly");
            proc.AddVarcharPara("@Order_Id", 4000, Order_nos);
            proc.AddVarcharPara("@OrderID_Key", 1000, Order_Key);
            return proc.GetTable();
        }
        public DataTable GetSalesInvoiceDetailsFromProjectChallanonly(string Order_nos, string Order_Key, string Product_Ids)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_GetSalesInvoiceForSaleChallan");
            proc.AddVarcharPara("@Mode", 100, "GetOrderProductDetailsOnly");
            proc.AddVarcharPara("@Order_Id", 4000, Order_nos);
            proc.AddVarcharPara("@OrderID_Key", 1000, Order_Key);
            return proc.GetTable();
        }
        public DataTable GetIndentRequisitionToBindCGridProductPopUp(string Order_nos, string Order_Key, string Product_Ids, string Action)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_GetRequistionDetails");
            proc.AddVarcharPara("@Mode", 100, "GetOrderProductDetailsOnly");
            proc.AddVarcharPara("@Requisition_Id", 4000, Order_nos);
            proc.AddVarcharPara("@OrderID_Key", 1000, Order_Key);
            proc.AddVarcharPara("@Action", 10, Action);
            return proc.GetTable();
        }

        public DataTable GetStockOutToBindCGridProductPopUp(string Order_nos, string Order_Key, string Product_Ids)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_GetStockOutProductDetailsForStockIn");
            proc.AddVarcharPara("@Mode", 100, "GetOrderProductDetailsOnly");
            proc.AddVarcharPara("@Requisition_Id", 4000, Order_nos);
            proc.AddVarcharPara("@OrderID_Key", 1000, Order_Key);
            return proc.GetTable();
        }

        public DataTable GetIssueToServiceCentreToBindCGridProductPopUp(string Order_nos, string Order_Key, string Product_Ids)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_GetIssueToServiceProductDetailsForRecvService");
            proc.AddVarcharPara("@Mode", 100, "GetOrderProductDetailsOnly");
            proc.AddVarcharPara("@Requisition_Id", 4000, Order_nos);
            proc.AddVarcharPara("@OrderID_Key", 1000, Order_Key);
            return proc.GetTable();
        }
        public DataTable GetIndentRequisitionToBindCGrid(string Quote_Nos, string Order_Key, string Product_Ids, string Action)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_GetRequistionDetails");
            proc.AddVarcharPara("@Mode", 100, "GetOrderInfoFromChallanToBindBatchGrid");
            proc.AddVarcharPara("@Requisition_Id", 4000, Quote_Nos);
            proc.AddVarcharPara("@ORder_Details_Id", 1000, Order_Key);
            proc.AddVarcharPara("@Product_Id", 1000, Product_Ids);
            proc.AddVarcharPara("@Action", 10, Action);
            return proc.GetTable();
        }

        public DataTable GetTransferOutDetailsToBindCGrid(string Quote_Nos, string Order_Key, string Product_Ids, string Action)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_GetStockOutProductDetailsForStockIn");
            proc.AddVarcharPara("@Mode", 100, "GetOrderInfoFromChallanToBindBatchGridQuoteDetails");
            proc.AddVarcharPara("@Requisition_Id", 4000, Quote_Nos);
            proc.AddVarcharPara("@ORder_Details_Id", 1000, Order_Key);
            proc.AddVarcharPara("@Product_Id", 1000, Product_Ids);
            proc.AddVarcharPara("@Action", 10, Action);
            return proc.GetTable();
        }

        public DataTable GetIssueToServiceDetailsToBindCGrid(string Quote_Nos, string Order_Key, string Product_Ids, string Action)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_GetIssueToServiceProductDetailsForRecvService");
            proc.AddVarcharPara("@Mode", 100, "GetOrderInfoFromChallanToBindBatchGridQuoteDetails");
            proc.AddVarcharPara("@Requisition_Id", 4000, Quote_Nos);
            proc.AddVarcharPara("@ORder_Details_Id", 1000, Order_Key);
            proc.AddVarcharPara("@Product_Id", 1000, Product_Ids);
            proc.AddVarcharPara("@Action", 10, Action);
            return proc.GetTable();
        }
        public DataSet GetSalesOrderDetailsFromProjectChallan(string Quote_Nos, string Order_Key, string Product_Ids, string Action)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_GetOrderDetailsForSaleChallan");
            proc.AddVarcharPara("@Mode", 100, "GetOrderInfoFromChallanToBindBatchGrid");
            proc.AddVarcharPara("@Order_Id", 4000, Quote_Nos);
            proc.AddVarcharPara("@ORder_Details_Id", 1000, Order_Key);
            proc.AddVarcharPara("@Product_Id", 1000, Product_Ids);
            proc.AddVarcharPara("@Action", 10, Action);
            return proc.GetDataSet();
        }
        public DataSet GetSalesInvoiceFromProjectChallan(string Quote_Nos, string Order_Key, string Product_Ids, string Action)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_GetSalesInvoiceForSaleChallan");
            proc.AddVarcharPara("@Mode", 100, "GetOrderInfoFromProjectChallanToBindBatchGrid");
            proc.AddVarcharPara("@Order_Id", 4000, Quote_Nos);
            proc.AddVarcharPara("@ORder_Details_Id", 1000, Order_Key);
            proc.AddVarcharPara("@Product_Id", 1000, Product_Ids);
            proc.AddVarcharPara("@Action", 10, Action);
            return proc.GetDataSet();
        }
        //End


        public DataSet GetQuotationDetailsUOMInfo(string UOM, string StockUOM)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Prc_GetUOMInfo");
            proc.AddVarcharPara("@UOMId", 20, UOM);
            proc.AddVarcharPara("@StockUOmId", 20, StockUOM);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataTable GetOtherActivityIdToShowDetail(int transSaleId, int ActivityTypeId)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_SalesActivity");
            proc.AddVarcharPara("@Action", 100, "GetOtherActivityIdToShowDetail");
            proc.AddIntegerPara("@trsid", transSaleId);
            proc.AddIntegerPara("@ActivityTypeId", ActivityTypeId);
            return proc.GetTable();
        }



        public DataTable GetSaleVisitDtlToShowDetail(int transSaleId)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_SalesActivity");
            proc.AddVarcharPara("@Action", 100, "GetSaleVisitDtlToShowDetail");
            proc.AddIntegerPara("@trsid", transSaleId);
            return proc.GetTable();
        }

        public DataTable PopulateSaleDispositionDetailForSaleVisitOfSalesActivities(int cnt)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_SalesActivity");
            proc.AddVarcharPara("@Action", 100, "PopulateSaleDispositionDetailForSaleVisitOfSalesActivities");
            proc.AddIntegerPara("@cnt", cnt);
            return proc.GetTable();
        }

        public DataTable PopulateSalesActivitiesDropDownForSalesVisit(string AllUser)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_SalesActivity");
            proc.AddVarcharPara("@Action", 100, "PopulateSalesActivitiesDropDownForSalesVisit");
            proc.AddVarcharPara("@AllUser", 500, AllUser);
            return proc.GetTable();
        }

        public DataSet GetSalesActvityViewDetails(string ActivityId)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesActivity");
            proc.AddVarcharPara("@Action", 100, "GetSalesActvityViewDetails");
            proc.AddIntegerPara("@ActivityId", Convert.ToInt32(ActivityId));
            ds = proc.GetDataSet();
            return ds;
        }


        public DataTable GetContactLeadCustomerId(string slsid)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_SalesActivity");
            proc.AddVarcharPara("@Action", 100, "GetContactId");
            proc.AddIntegerPara("@trsid", Convert.ToInt32(slsid));
            return proc.GetTable();
        }

        public DataTable GetSaleActivitydtl(int slsid)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_SalesActivity");
            proc.AddVarcharPara("@Action", 100, "GetSaleActivitydtl");
            proc.AddIntegerPara("@trsid", slsid);
            return proc.GetTable();
        }

        #region GetCustomerLeadName
        public DataTable GetLeadCustomerName(string Internald)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_SalesActivity");
            proc.AddVarcharPara("@Action", 100, "GetLeadCustomerName");
            proc.AddVarcharPara("@LeadContactId", 30, Internald);
            return proc.GetTable();
        }

        #endregion Sales Visit


        #region GetPhoneStatus
        public DataTable GetPhoneStatus(string Sid)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_SalesActivity");
            proc.AddVarcharPara("@Action", 100, "GetPhoneStatus");
            proc.AddIntegerPara("@trsid", Convert.ToInt32(Sid));
            return proc.GetTable();
        }

        #endregion Sales Visit

        #region Get Industry from lead
        public DataTable GetIndustry(string internalId)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_SalesActivity");
            proc.AddVarcharPara("@Action", 100, "GetIndustry");
            proc.AddVarcharPara("@LeadContactId", 100, internalId);
            return proc.GetTable();
        }
        #endregion Get Industry


        #region Get Quotation/SalesDate  Date
        public DataTable GetQuotationDate(string Quote_Nos)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_GetQuotationDetails");
            proc.AddVarcharPara("@Quote_Number", 100, Quote_Nos);
            proc.AddVarcharPara("@Mode", 100, "GetActivityTypeList");

            return proc.GetTable();
        }

        public DataTable GetStockOutDate(string Quote_Nos)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_GetQuotationDetails");
            proc.AddVarcharPara("@Quote_Number", 100, Quote_Nos);
            proc.AddVarcharPara("@Mode", 100, "GetStockTransferDate");

            return proc.GetTable();
        }

        public DataTable GetIssueToServiceCentreDate(string Quote_Nos)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_GetQuotationDetails");
            proc.AddVarcharPara("@Quote_Number", 100, Quote_Nos);
            proc.AddVarcharPara("@Mode", 100, "GetIssueToServiceCentreDate");

            return proc.GetTable();
        }


        public DataTable GetIndentDate(string Quote_Nos)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_GetQuotationDetails");
            proc.AddVarcharPara("@Quote_Number", 100, Quote_Nos);
            proc.AddVarcharPara("@Mode", 100, "GetIndentDate");

            return proc.GetTable();
        }
        public DataTable GetBranchRequisitionDate(string Quote_Nos)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_GetQuotationDetails");
            proc.AddVarcharPara("@Quote_Number", 100, Quote_Nos);
            proc.AddVarcharPara("@Mode", 100, "GetBRDate");

            return proc.GetTable();
        }
        #endregion  Get Quotation  Date

        public DataTable GetNumberingSchema(string strCompanyID, string strBranchID, string strFinYear, string strType, string strIsSplit)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesActivity");
            proc.AddVarcharPara("@Action", 100, "GetNumberingSchema");
            proc.AddVarcharPara("@CompanyID", 100, strCompanyID);
            //proc.AddVarcharPara("@BranchID", 100, strBranchID);
            proc.AddVarcharPara("@BranchID", 4000, strBranchID);
            proc.AddVarcharPara("@FinYear", 100, strFinYear);
            proc.AddVarcharPara("@Type", 100, strType);
            proc.AddVarcharPara("@IsSplit", 100, strIsSplit);
            ds = proc.GetTable();
            return ds;
        }
        public DataTable GetNumberingSchemaPendingDelivery(string strCompanyID, string strBranchID, string strFinYear, string strType, string strIsSplit)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesActivity");
            proc.AddVarcharPara("@Action", 100, "GetNmbrngSchmForPendingDeliveryList");
            proc.AddVarcharPara("@CompanyID", 100, strCompanyID);
            //proc.AddVarcharPara("@BranchID", 100, strBranchID);
            proc.AddVarcharPara("@BranchID", 4000, strBranchID);
            proc.AddVarcharPara("@FinYear", 100, strFinYear);
            proc.AddVarcharPara("@Type", 100, strType);
            proc.AddVarcharPara("@IsSplit", 100, strIsSplit);
            ds = proc.GetTable();
            return ds;
        }

        public DataTable GetVehicleMasterNumber(string strBranchID)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesActivity");
            proc.AddVarcharPara("@Action", 100, "GetVehicleMasterData");
            proc.AddVarcharPara("@BranchID", 4000, strBranchID);
            ds = proc.GetTable();
            return ds;
        }

        public DataTable SolveRepeatSubAccount(string FromDate, string Todate, string Bank_Id)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_SolveRepeatSubaccount");
            proc.AddVarcharPara("@TdateFrom", 15, FromDate);
            proc.AddVarcharPara("@TdateTo", 15, Todate);
            proc.AddVarcharPara("@AccountID", 50, Bank_Id);
            ds = proc.GetTable();
            return ds;
        }


        public DataTable GetUserBranchHeirarchy(string strBranchID)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("CallGetBranchHierarchyList");
            //proc.AddVarcharPara("@Action", 100, "GetNumberingSchema");
            proc.AddIntegerPara("@BranchId", Convert.ToInt32(strBranchID));
            ds = proc.GetTable();
            return ds;
        }



        public DataTable GetNumberingSchemaCustomerDeliverypending(string strCompanyID, string strBranchID, string strFinYear, string strType, string strIsSplit)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesActivity");
            proc.AddVarcharPara("@Action", 100, "GetNumberingSchema");
            proc.AddVarcharPara("@CompanyID", 100, strCompanyID);
            //proc.AddVarcharPara("@BranchID", 100, strBranchID);
            proc.AddTextPara("@BranchID", strBranchID);
            proc.AddVarcharPara("@FinYear", 100, strFinYear);
            proc.AddVarcharPara("@Type", 100, strType);
            proc.AddVarcharPara("@IsSplit", 100, strIsSplit);
            ds = proc.GetTable();
            return ds;
        }


        public DataTable GetNumberingSchemaOpening(string strCompanyID, string strBranchID, string strFinYear, string strType, string strIsSplit)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesActivity");
            proc.AddVarcharPara("@Action", 100, "GetNumberingSchemaOpening");
            proc.AddVarcharPara("@CompanyID", 100, strCompanyID);
            proc.AddVarcharPara("@BranchID", 4000, strBranchID);
            proc.AddVarcharPara("@FinYear", 100, strFinYear);
            proc.AddVarcharPara("@Type", 100, strType);
            proc.AddVarcharPara("@IsSplit", 100, strIsSplit);
            ds = proc.GetTable();
            return ds;
        }


        public string GetFeedbackNote(string DetailsId, string Tid)
        {
            string FeedbackNote = string.Empty;
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesActivity");
            proc.AddVarcharPara("@action", 100, "GetFeedbackNote");
            proc.AddVarcharPara("@DetailsId", 200, DetailsId);
            proc.AddVarcharPara("@TId", 10, Tid);
            dt = proc.GetTable();
            if (dt != null && dt.Rows.Count > 0)
            {
                FeedbackNote = Convert.ToString(dt.Rows[0]["Note"]);

            }
            return FeedbackNote;
        }



        public DataTable GetsalesActivity(string cnt_internalId, string ActivityTypeId, string FromDate, string ToDate)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("sp_Sales");
            proc.AddVarcharPara("@Mode", 50, "GetSalesActivityDetails");
            proc.AddVarcharPara("@cnt_internalId", 100, cnt_internalId);
            proc.AddVarcharPara("@ActivityTypeId", 50, ActivityTypeId);
            proc.AddVarcharPara("@Fromdate", 50, FromDate);
            proc.AddVarcharPara("@ToDate", 50, ToDate);
            dt = proc.GetTable();
            return dt;
        }

        public DataSet GetSalesInvoiceFromMaterialIsuue(string Quote_Nos, string Order_Key, string Product_Ids, string Action)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_GetSalesInvoiceForSaleChallan");
            proc.AddVarcharPara("@Mode", 100, "GetOrderInfoFromProjectChallanToBindBatchGridMaterialIssue");
            proc.AddVarcharPara("@Order_Id", 4000, Quote_Nos);
            proc.AddVarcharPara("@ORder_Details_Id", 1000, Order_Key);
            proc.AddVarcharPara("@Product_Id", 1000, Product_Ids);
            proc.AddVarcharPara("@Action", 10, Action);
            return proc.GetDataSet();
        }

        public DataTable GetSalesInvoiceDetailsFromIssueMaterialOnly(string Order_nos, string Order_Key, string Product_Ids)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_GetSalesInvoiceForSaleChallan");
            proc.AddVarcharPara("@Mode", 100, "GetOrderProductDetailsOnly");
            proc.AddVarcharPara("@Order_Id", 4000, Order_nos);
            proc.AddVarcharPara("@OrderID_Key", 1000, Order_Key);
            return proc.GetTable();
            //GetInvoiceProductDetailsOnlyForIssueMaterial
        }
    }
}
