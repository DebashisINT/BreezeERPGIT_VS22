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
    public class ProjectInvoiceBL
    {
        public int UpdateEWayBillForChallan(string ChallanID, string EWayBillNumber, string EWayBillDate, string EWayBillValue)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_SalesChallan_Details");
            proc.AddVarcharPara("@Action", 100, "UpdateEWayBill");
            proc.AddBigIntegerPara("@ChallanId", Convert.ToInt32(ChallanID));
            proc.AddVarcharPara("@EWayBillNumber", 50, EWayBillNumber);
            proc.AddVarcharPara("@EWayBillDate", 50, EWayBillDate);
            proc.AddVarcharPara("@EWayBillValue", 50, EWayBillValue);
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }



        public int UpdateEWayBill(string InvoiceID, string EWayBillNumber, string EWayBillDate, string EWayBillValue)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_CRMProjectInvoice_Details");
            proc.AddVarcharPara("@Action", 100, "UpdateEWayBill");
            proc.AddBigIntegerPara("@Invoice_ID", Convert.ToInt32(InvoiceID));
            proc.AddVarcharPara("@EWayBillNumber", 50, EWayBillNumber);
            proc.AddVarcharPara("@EWayBillDate", 50, EWayBillDate);
            proc.AddVarcharPara("@EWayBillValue", 50, EWayBillValue);
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }
        public int UpdateShipBill(string actionname, DateTime? shipdate, string ShippingBill_Number, string ShippingBill_PortId, string ShippingBill_InvoiceId, string CreatedBy)
        {
            //Int64 shinumber=0;
            int i;
            int rtrnvalue = 0;
            //if (ShippingBill_Number != "")
            //{
            //    shinumber = Convert.ToInt64(ShippingBill_Number);
            //}
            //else
            //{
            //    shinumber = 0;
            //}

            ProcedureExecute proc = new ProcedureExecute("prc_CRMProjectInvoice_Details");
            proc.AddVarcharPara("@Action", 100, actionname);
            proc.AddPara("@ShippingBill_Date", shipdate);
            proc.AddVarcharPara("@ShippingBill_Number", 20, ShippingBill_Number);
            proc.AddIntegerPara("@ShippingBill_PortId", Convert.ToInt32(ShippingBill_PortId));
            proc.AddIntegerPara("@ShippingBill_InvoiceId", Convert.ToInt32(ShippingBill_InvoiceId));
            proc.AddVarcharPara("@CreatedBy", 50, CreatedBy);
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }
        public DataSet GetAllDropDownDetailForProjectInvoice(string userbranch, string CompanyID, string BranchID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMProjectInvoice_Details");
            proc.AddVarcharPara("@Action", 100, "GetAllDropDownDetailForSalesQuotation");
            proc.AddVarcharPara("@BranchList", 3000, userbranch);
            proc.AddVarcharPara("@CompanyID", 100, CompanyID);
            proc.AddVarcharPara("@BranchID", 100, BranchID);
            ds = proc.GetDataSet();
            return ds;
        }
        public DataTable PopulateCustomerDetail()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMProjectInvoice_Details");
            proc.AddVarcharPara("@Action", 100, "PopulateCustomerDetail");
            dt = proc.GetTable();
            return dt;
        }
        public DataTable PopulateCustomerDetailForTransitProjectInvoice()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMTransitProjectInvoice_Details");
            proc.AddVarcharPara("@Action", 100, "PopulateCustomerDetailForTransitProjectInvoice");
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetCustomerDetails_InvoiceRelated(string strCustomerID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMProjectInvoice_Details");
            proc.AddVarcharPara("@Action", 100, "GetCustomerInvoiceDetails");
            proc.AddVarcharPara("@CustomerID", 1000, strCustomerID);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetCustomerDetails_InvoiceRelated_Days(string strCustomerID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMProjectInvoice_Details");
            proc.AddVarcharPara("@Action", 100, "GetCustomerInvoiceDetailsDays");
            proc.AddVarcharPara("@CustomerID", 1000, strCustomerID);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetCustomerTotalDues(string strCustomerID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMProjectInvoice_Details");
            proc.AddVarcharPara("@Action", 100, "GetCustomerTotalDues");
            proc.AddVarcharPara("@CustomerID", 1000, strCustomerID);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetNecessaryData(string strComponentIDs, string strType)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMProjectInvoice_Details");
            proc.AddVarcharPara("@Action", 100, "GetComponentSelectData");
            proc.AddVarcharPara("@SelectedComponentList", 1000, strComponentIDs);
            proc.AddVarcharPara("@ComponentType", 1000, strType);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetQuotationList_GridData(string userbranchlist, string lastCompany, string Fiyear, string userbranchID, DateTime FromDate, DateTime ToDate, string invoicefor)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMProjectInvoice_Details");
            proc.AddVarcharPara("@Action", 500, "GetQuotationListGridData");
            proc.AddVarcharPara("@BranchList", 3000, userbranchlist);
            proc.AddVarcharPara("@CompanyID", 50, lastCompany);
            proc.AddVarcharPara("@FinYear", 50, Fiyear);
            proc.AddVarcharPara("@BranchID", 3000, userbranchID);
            proc.AddDateTimePara("@FromDate", FromDate);
            proc.AddDateTimePara("@ToDate", ToDate);
            proc.AddVarcharPara("@invoicefor", 50, invoicefor);

            dt = proc.GetTable();
            return dt;
        }


        public DataTable GetQuotationList_GridDataForVendor(string userbranchlist, string lastCompany, string Fiyear, string userbranchID, DateTime FromDate, DateTime ToDate, string invoicefor)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMProjectInvoice_Details");
            proc.AddVarcharPara("@Action", 500, "GetQuotationListGridDataForVendor");
            proc.AddVarcharPara("@BranchList", 3000, userbranchlist);
            proc.AddVarcharPara("@CompanyID", 50, lastCompany);
            proc.AddVarcharPara("@FinYear", 50, Fiyear);
            proc.AddVarcharPara("@BranchID", 3000, userbranchID);
            proc.AddDateTimePara("@FromDate", FromDate);
            proc.AddDateTimePara("@ToDate", ToDate);
            proc.AddVarcharPara("@invoicefor", 50, invoicefor);

            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetQuotationList_GridData_OldUnit(string userbranchlist, string lastCompany, string Fiyear, string userbranchID, DateTime FromDate, DateTime ToDate)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMProjectInvoice_Details");
            proc.AddVarcharPara("@Action", 500, "GetQuotationListGridData_OldUnit");
            proc.AddVarcharPara("@BranchList", 3000, userbranchlist);
            proc.AddVarcharPara("@CompanyID", 50, lastCompany);
            proc.AddVarcharPara("@FinYear", 50, Fiyear);
            proc.AddVarcharPara("@BranchID", 3000, userbranchID);
            proc.AddDateTimePara("@FromDate", FromDate);
            proc.AddDateTimePara("@ToDate", ToDate);
            dt = proc.GetTable();
            return dt;
        }


        public DataTable GetChallanGridData_OldUnit(string userbranchlist, string lastCompany, string Fiyear, string userbranchID, DateTime FromDate, DateTime ToDate)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMProjectInvoice_Details");
            proc.AddVarcharPara("@Action", 500, "GetChallanGridData_OldUnit");
            proc.AddVarcharPara("@BranchList", 3000, userbranchlist);
            proc.AddVarcharPara("@CompanyID", 50, lastCompany);
            proc.AddVarcharPara("@FinYear", 50, Fiyear);
            proc.AddVarcharPara("@BranchID", 3000, userbranchID);
            proc.AddDateTimePara("@FromDate", FromDate);
            proc.AddDateTimePara("@ToDate", ToDate);
            dt = proc.GetTable();
            return dt;
        }



        public DataTable GetTransitProjectInvoiceListGridData(string userbranchlist, string lastCompany, string Fiyear)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMTransitProjectInvoice_Details");
            proc.AddVarcharPara("@Action", 500, "GetTransitSalesInvoiceListGridData");
            proc.AddVarcharPara("@BranchList", 3000, userbranchlist);
            proc.AddVarcharPara("@CompanyID", 50, lastCompany);
            proc.AddVarcharPara("@FinYear", 50, Fiyear);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetQuotationListGridData(string userbranchlist, string lastCompany, string Fiyear)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMProjectInvoice_Details");
            proc.AddVarcharPara("@Action", 500, "GetQuotationListGridDataOpening");
            proc.AddVarcharPara("@BranchList", 3000, userbranchlist);
            proc.AddVarcharPara("@CompanyID", 50, lastCompany);
            proc.AddVarcharPara("@FinYear", 50, Fiyear);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetTotalDuesData(string userbranchlist, string lastCompany)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMProjectInvoice_Details");
            proc.AddVarcharPara("@Action", 500, "GetTotalDues");
            proc.AddVarcharPara("@BranchList", 3000, userbranchlist);
            proc.AddVarcharPara("@CompanyID", 50, lastCompany);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetInvoiceEditData(string InvoiceID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMProjectInvoice_Details");
            proc.AddVarcharPara("@Action", 500, "InvoiceEditDetails");
            proc.AddVarcharPara("@InvoiceID", 500, InvoiceID);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetProjectCode(string Proj_Code)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMProjectInvoice_Details");
            proc.AddVarcharPara("@Action", 500, "GetProjectCode");
            proc.AddVarcharPara("@Proj_Code", 200, Proj_Code);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetProjectEditData(string ProjectInvoiceId)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMProjectInvoice_Details");
            proc.AddVarcharPara("@Action", 500, "ProjectEditdata");
            proc.AddVarcharPara("@InvoiceID", 200, ProjectInvoiceId);
            dt = proc.GetTable();
            return dt;
        }

        public DataSet GetDetailsOfInvoicedata(string stInvoiceID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMProjectInvoice_Details");
            proc.AddVarcharPara("@Action", 500, "ModifyOfInvoiceEditDetails");
            proc.AddVarcharPara("@InvoiceID", 500, stInvoiceID);
            ds = proc.GetDataSet();
            return ds;
        }
        public int DeleteInvoice(string Invoiceid)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_CRMProjectInvoice_AddEdit");
            proc.AddVarcharPara("@Action", 100, "DeleteInvoice");
            proc.AddVarcharPara("@DeleteInvoiceId", 20, Invoiceid);
            proc.AddVarcharPara("@UserID", 20, Convert.ToString(HttpContext.Current.Session["userid"]));
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }
        public int CheckInvoice(string Invoiceid)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_CRMProjectInvoice");
            proc.AddVarcharPara("@Action", 100, "CheckInvoice");
            proc.AddVarcharPara("@DeleteInvoiceId", 20, Invoiceid);
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }

        public DataTable GetRetentionDetails(string Invoiceid)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMProjectInvoice");
            proc.AddVarcharPara("@Action", 100, "GetRetentionDetails");
            proc.AddVarcharPara("@InvoiceId", 20, Invoiceid);
            dt = proc.GetTable();            
            return dt;

        }

        public DataTable SaveRetentionDetails(string Invoiceid, string retAmount, string schema_id, string doc_no, string trans_date)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMProjectInvoice");
            proc.AddVarcharPara("@Action", 100, "SaveRetentionDetails");
            proc.AddVarcharPara("@InvoiceId", 20, Invoiceid);
            proc.AddVarcharPara("@schema_id", 200, schema_id);
            proc.AddVarcharPara("@doc_no", 200, doc_no);
            proc.AddVarcharPara("@trans_date", 20, trans_date);

            proc.AddVarcharPara("@RETENTIOn_RETURN_AMOUNT", 25, retAmount);
            dt = proc.GetTable();
            return dt;

        }

        public DataTable SaveRetentionDetailsOpening(string Invoiceid, string retAmount, string schema_id, string doc_no, string trans_date)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMProjectInvoice");
            proc.AddVarcharPara("@Action", 100, "SaveRetentionDetailsOpening");
            proc.AddVarcharPara("@InvoiceId", 20, Invoiceid);
            proc.AddVarcharPara("@schema_id", 200, schema_id);
            proc.AddVarcharPara("@doc_no", 200, doc_no);
            proc.AddVarcharPara("@trans_date", 20, trans_date);

            proc.AddVarcharPara("@RETENTIOn_RETURN_AMOUNT", 25, retAmount);
            dt = proc.GetTable();
            return dt;

        }
        public DataTable SaveRetentionDetailsOpeningPI(string Invoiceid, string retAmount, string schema_id, string doc_no, string trans_date)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMProjectInvoice");
            proc.AddVarcharPara("@Action", 100, "SaveRetentionDetailsOpeningPI");
            proc.AddVarcharPara("@InvoiceId", 20, Invoiceid);
            proc.AddVarcharPara("@schema_id", 200, schema_id);
            proc.AddVarcharPara("@doc_no", 200, doc_no);
            proc.AddVarcharPara("@trans_date", 20, trans_date);

            proc.AddVarcharPara("@RETENTIOn_RETURN_AMOUNT", 25, retAmount);
            dt = proc.GetTable();
            return dt;

        }
        public DataSet GetInvoiceProductData(string strInvoiceID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMProjectInvoice_Details");
            proc.AddVarcharPara("@Action", 500, "ProductDetails");
            proc.AddVarcharPara("@InvoiceID", 500, strInvoiceID);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet GetProjectInvoiceProductData(string strInvoiceID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMProjectInvoice_Details");
            proc.AddVarcharPara("@Action", 500, "ProductDetailsForSalesInvoice");
            proc.AddVarcharPara("@InvoiceID", 500, strInvoiceID);
            ds = proc.GetDataSet();
            return ds;
        }
        public DataSet GetOldUnitInvoiceProductData(string strInvoiceID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_OldUnitCRMSalesInvoice_Details");
            proc.AddVarcharPara("@Action", 500, "ProductDetails");
            proc.AddVarcharPara("@InvoiceID", 500, strInvoiceID);
            ds = proc.GetDataSet();
            return ds;
        }


        public DataTable GetInvoiceBillingAddress(string strInvoiceID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesInvoice_Details");
            proc.AddVarcharPara("@Action", 500, "InvoiceBillingAddress");
            proc.AddVarcharPara("@InvoiceID", 500, strInvoiceID);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetComponent(string Customer, string Date, string ComponentType, string FinYear, string BranchID, string Action, string strInvoiceID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("_p_ProjectTagging_Details");
            proc.AddVarcharPara("@Action", 500, Action);
            proc.AddVarcharPara("@CustomerID", 500, Customer);
            proc.AddDateTimePara("@Date", Convert.ToDateTime(Date));
            proc.AddVarcharPara("@ComponentType", 10, ComponentType);
            proc.AddVarcharPara("@FinYear", 10, FinYear);
            proc.AddVarcharPara("@BranchID", 3000, BranchID);
            proc.AddVarcharPara("@InvoiceID", 20, strInvoiceID);

            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetVehicle()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_GetAllVehicles");
            proc.AddVarcharPara("@OrderByMod", 10, "");
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetComponent(string Customer, string Date, string ComponentType, string FinYear, string BranchID, string Action, string strInvoiceID, string inventory = "", string Entry_type = "")
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("_p_ProjectTagging_Details");
            proc.AddVarcharPara("@Action", 500, Action);
            proc.AddVarcharPara("@CustomerID", 500, Customer);
            proc.AddDateTimePara("@Date", Convert.ToDateTime(Date));
            proc.AddVarcharPara("@ComponentType", 10, ComponentType);
            proc.AddVarcharPara("@FinYear", 10, FinYear);
            proc.AddVarcharPara("@BranchID", 3000, BranchID);
            proc.AddVarcharPara("@InvoiceID", 20, strInvoiceID);
            proc.AddVarcharPara("@Inventory", 20, inventory);
            proc.AddVarcharPara("@Entry_type", 20, Entry_type);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetComponentForTransitPurchaseInvoice(string Customer, string Date, string ComponentType, string FinYear, string BranchID, string Action, string strInvoiceID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMTransitProjectInvoice_Details");
            proc.AddVarcharPara("@Action", 500, "GetComponentForTransitPurchaseInvoice");
            proc.AddVarcharPara("@CustomerID", 500, Customer);
            proc.AddDateTimePara("@Date", Convert.ToDateTime(Date));
            proc.AddVarcharPara("@ComponentType", 10, "GetComponentForTransitPurchaseInvoice");
            proc.AddVarcharPara("@FinYear", 10, FinYear);
            proc.AddVarcharPara("@BranchID", 3000, BranchID);
            proc.AddVarcharPara("@InvoiceID", 20, strInvoiceID);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetComponentProductList(string Action, string ComponentList, string InvoiceID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("_p_ProjectTagging_Details");
            proc.AddVarcharPara("@Action", 500, Action);
            proc.AddVarcharPara("@ComponentList", 2000, ComponentList);
            proc.AddVarcharPara("@InvoiceID", 20, InvoiceID);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetTransitPurchaseInvoiceProducts(string Action, string ComponentList, string InvoiceID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMTransitProjectInvoice_Details");
            proc.AddVarcharPara("@Action", 500, "GetTransitPurchaseInvoiceProducts");
            proc.AddVarcharPara("@ComponentList", 2000, ComponentList);
            proc.AddVarcharPara("@InvoiceID", 20, InvoiceID);
            dt = proc.GetTable();
            return dt;
        }
        public DataSet GetSelectedComponentProductList(string Action, string SelectedComponentList, string InvoiceID)
        {
            DataSet dt = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("_p_ProjectTagging_Details");
            proc.AddVarcharPara("@Action", 500, Action);
            proc.AddVarcharPara("@SelectedComponentList", 2000, SelectedComponentList);
            proc.AddVarcharPara("@InvoiceID", 20, InvoiceID);
            dt = proc.GetDataSet();
            return dt;
        }
        public DataTable GetSeletedTransitPurchaseInvoiceProducts(string Action, string SelectedComponentList, string InvoiceID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMTransitProjectInvoice_Details");
            proc.AddVarcharPara("@Action", 500, "GetSeletedTransitPurchaseInvoiceProducts");
            proc.AddVarcharPara("@SelectedComponentList", 2000, SelectedComponentList);
            proc.AddVarcharPara("@InvoiceID", 20, InvoiceID);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetLinkedProductList(string Action, string ProductID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMProjectInvoice_Details");
            proc.AddVarcharPara("@Action", 500, Action);
            proc.AddVarcharPara("@ProductID", 2000, ProductID);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable IsMinSalePriceOk(string ProductList)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_CRMProjectInvoice_Details");
            proc.AddVarcharPara("@Action", 500, "IsMinSalePriceOk");
            proc.AddVarcharPara("@ProductSalePriceList", 5000, ProductList);
            DataTable ReturnTable = proc.GetTable();
            return ReturnTable;
        }
    }
}
