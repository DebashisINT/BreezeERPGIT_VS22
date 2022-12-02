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
    public class SalesInvoiceBL
    {

        public int UpdateEWayBillForChallan(string ChallanID, string EWayBillNumber, string EWayBillDate, string EWayBillValue, string TransporterGSTIN
                    , string TransporterName, string TransportationMode, string TransportationDistance, string TransporterDocNo
                    , string TransporterDocDate, string VehicleNo, string VehicleType)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_SalesChallan_Details");
            proc.AddVarcharPara("@Action", 100, "UpdateEWayBill");
            proc.AddBigIntegerPara("@ChallanId", Convert.ToInt32(ChallanID));
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



        public int UpdateEWayBill(string InvoiceID, string EWayBillNumber, string EWayBillDate, string EWayBillValue, string TransporterGSTIN
                    , string TransporterName, string TransportationMode, string TransportationDistance, string TransporterDocNo
                    , string TransporterDocDate, string VehicleNo, string VehicleType)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesInvoice_Details");
            proc.AddVarcharPara("@Action", 100, "UpdateEWayBill");
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

            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesInvoice_Details");
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
        public DataSet GetAllDropDownDetailForSalesInvoice(string userbranch, string CompanyID, string BranchID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesInvoice_Details");
            proc.AddVarcharPara("@Action", 100, "GetAllDropDownDetailForSalesQuotation");
            proc.AddVarcharPara("@BranchList", 3000, userbranch);
            proc.AddVarcharPara("@CompanyID", 100, CompanyID);
            proc.AddVarcharPara("@BranchID", 100, BranchID);
            ds = proc.GetDataSet();
            return ds;
        }

        //public DataSet GetAllDropDownDetailForSalesInvoice(string userbranch, string CompanyID, string BranchID)
        //{
        //    DataSet ds = new DataSet();
        //    ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesInvoice_Details");
        //    proc.AddVarcharPara("@Action", 100, "GetAllDropDownDetailForSalesQuotation");
        //    proc.AddVarcharPara("@BranchList", 3000, userbranch);
        //    proc.AddVarcharPara("@CompanyID", 100, CompanyID);
        //    proc.AddVarcharPara("@BranchID", 100, BranchID);
        //    ds = proc.GetDataSet();
        //    return ds;
        //}
        public DataSet GetAllDropDownDetailForInvoiceDelvChallan(string userbranch, string CompanyID, string BranchID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_InvoiceDeliveryChallan_Details");
            proc.AddVarcharPara("@Action", 100, "GetAllDropDownDetailForSalesQuotation");
            proc.AddVarcharPara("@BranchList", 3000, userbranch);
            proc.AddVarcharPara("@CompanyID", 100, CompanyID);
            proc.AddVarcharPara("@BranchID", 100, BranchID);
            ds = proc.GetDataSet();
            return ds;
        }
        public DataTable EInvoiceBranchDetails(string BranchId)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SaveEInvoice");
            proc.AddVarcharPara("@Action", 100, "GetEInvoiceBranchDet");
            proc.AddIntegerPara("@BranchID", Convert.ToInt32(BranchId));
            ds = proc.GetTable();
            return ds;
        }
        public DataTable PopulateCustomerDetail()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesInvoice_Details");
            proc.AddVarcharPara("@Action", 100, "PopulateCustomerDetail");
            dt = proc.GetTable();
            return dt;
        }
        public DataTable PopulateCustomerDetailForTransitSalesInvoice()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMTransitSalesInvoice_Details");
            proc.AddVarcharPara("@Action", 100, "PopulateCustomerDetailForTransitSalesInvoice");
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetCustomerDetails_InvoiceRelated(string strCustomerID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesInvoice_Details");
            proc.AddVarcharPara("@Action", 100, "GetCustomerInvoiceDetails");
            proc.AddVarcharPara("@CustomerID", 1000, strCustomerID);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetCustomerDetails_InvoiceRelated_Days(string strCustomerID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesInvoice_Details");
            proc.AddVarcharPara("@Action", 100, "GetCustomerInvoiceDetailsDays");
            proc.AddVarcharPara("@CustomerID", 1000, strCustomerID);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetCustomerDetails_InvDelvChallanRelated_Days(string strCustomerID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_InvoiceDeliveryChallan_Details");
            proc.AddVarcharPara("@Action", 100, "GetCustomerInvoiceDetailsDays");
            proc.AddVarcharPara("@CustomerID", 1000, strCustomerID);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetCustomerTotalDues(string strCustomerID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesInvoice_Details");
            proc.AddVarcharPara("@Action", 100, "GetCustomerTotalDues");
            proc.AddVarcharPara("@CustomerID", 1000, strCustomerID);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetCustomerInvDelvChallanTotalDues(string strCustomerID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_InvoiceDeliveryChallan_Details");
            proc.AddVarcharPara("@Action", 100, "GetCustomerTotalDues");
            proc.AddVarcharPara("@CustomerID", 1000, strCustomerID);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetNecessaryData(string strComponentIDs, string strType)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesInvoice_Details");
            proc.AddVarcharPara("@Action", 100, "GetComponentSelectData");
            proc.AddVarcharPara("@SelectedComponentList", 1000, strComponentIDs);
            proc.AddVarcharPara("@ComponentType", 1000, strType);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetInvDelvChallanNecessaryData(string strComponentIDs, string strType)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_InvoiceDeliveryChallan_Details");
            proc.AddVarcharPara("@Action", 100, "GetComponentSelectData");
            proc.AddVarcharPara("@SelectedComponentList", 1000, strComponentIDs);
            proc.AddVarcharPara("@ComponentType", 1000, strType);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetQuotationList_GridData(string userbranchlist, string lastCompany, string Fiyear, string userbranchID, DateTime FromDate, DateTime ToDate, string invoicefor)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesInvoice_Details");
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
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesInvoice_Details");
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
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesInvoice_Details");
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
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesInvoice_Details");
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



        public DataTable GetTransitSalesInvoiceListGridData(string userbranchlist, string lastCompany, string Fiyear)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMTransitSalesInvoice_Details");
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
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesInvoice_Details");
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
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesInvoice_Details");
            proc.AddVarcharPara("@Action", 500, "GetTotalDues");
            proc.AddVarcharPara("@BranchList", 3000, userbranchlist);
            proc.AddVarcharPara("@CompanyID", 50, lastCompany);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetInvoiceEditData(string InvoiceID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesInvoice_Details");
            proc.AddVarcharPara("@Action", 500, "InvoiceEditDetails");
            proc.AddVarcharPara("@InvoiceID", 500, InvoiceID);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetProjectCode(string Proj_Code)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesInvoice_Details");
            proc.AddVarcharPara("@Action", 500, "GetProjectCode");
            proc.AddVarcharPara("@Proj_Code", 200, Proj_Code);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetInvDelvChallanProjectCode(string Proj_Code)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_InvoiceDeliveryChallan_Details");
            proc.AddVarcharPara("@Action", 500, "GetProjectCode");
            proc.AddVarcharPara("@Proj_Code", 200, Proj_Code);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetProjectEditData(string SalesInvoiceId)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesInvoice_Details");
            proc.AddVarcharPara("@Action", 500, "ProjectEditdata");
            proc.AddVarcharPara("@InvoiceID", 200, SalesInvoiceId);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetProjectInvoiceDelvEditData(string SalesInvoiceId)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_InvoiceDeliveryChallan_Details");
            proc.AddVarcharPara("@Action", 500, "ProjectEditdata");
            proc.AddVarcharPara("@InvoiceID", 200, SalesInvoiceId);
            dt = proc.GetTable();
            return dt;
        }

        public DataSet GetDetailsOfInvoicedata(string stInvoiceID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesInvoice_Details");
            proc.AddVarcharPara("@Action", 500, "ModifyOfInvoiceEditDetails");
            proc.AddVarcharPara("@InvoiceID", 500, stInvoiceID);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet GetDetailsOfDraftInvoicedata(string stInvoiceID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesInvoice_Details");
            proc.AddVarcharPara("@Action", 500, "DraftInvoiceDetails");
            proc.AddVarcharPara("@InvoiceID", 500, stInvoiceID);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet GetDetailsOfInvoiceDeliveryChallandata(string stInvoiceID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_InvoiceDeliveryChallan_Details");
            proc.AddVarcharPara("@Action", 500, "ModifyOfInvoiceEditDetails");
            proc.AddVarcharPara("@InvoiceID", 500, stInvoiceID);
            ds = proc.GetDataSet();
            return ds;
        }
        public int DeleteInvoice(string Invoiceid)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesInvoice_AddEdit");
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
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesInvoice");
            proc.AddVarcharPara("@Action", 100, "CheckInvoice");
            proc.AddVarcharPara("@DeleteInvoiceId", 20, Invoiceid);
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }

        public int CheckInvoiceDelvChallan(string Invoiceid,ref string Number)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesInvoice");
            proc.AddVarcharPara("@Action", 100, "CHECKINVOICECumChallan");
            proc.AddVarcharPara("@DeleteInvoiceId", 20, Invoiceid);
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            proc.AddVarcharPara("@RETURNVALUENumBer", 4000, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            Number = Convert.ToString(proc.GetParaValue("@RETURNVALUENumBer"));
            return rtrnvalue;

        }
        public DataSet GetInvoiceProductData(string strInvoiceID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesInvoice_Details");
            proc.AddVarcharPara("@Action", 500, "ProductDetails");
            proc.AddVarcharPara("@InvoiceID", 500, strInvoiceID);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet GetSalesInvoiceProductData(string strInvoiceID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesInvoice_Details");
            proc.AddVarcharPara("@Action", 500, "ProductDetailsForSalesInvoice");
            proc.AddVarcharPara("@InvoiceID", 500, strInvoiceID);
            ds = proc.GetDataSet();
            return ds;
        }
        // Rev Sanchita
        public DataSet GetSalesInvoiceProductData_New(string strInvoiceID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesInvoice_Details");
            proc.AddVarcharPara("@Action", 500, "ProductDetailsForSalesInvoice_New");
            proc.AddVarcharPara("@InvoiceID", 500, strInvoiceID);
            ds = proc.GetDataSet();
            return ds;
        }
        // End of REv Sanchita
        public DataSet GetDraftSalesInvoiceProductData(string strInvoiceID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesInvoice_Details");
            proc.AddVarcharPara("@Action", 500, "ProductDetailsForDraftSalesInvoice");
            proc.AddVarcharPara("@InvoiceID", 500, strInvoiceID);
            ds = proc.GetDataSet();
            return ds;
        }
        public DataSet GetInvoiceDelvChallanProductData(string strInvoiceID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_InvoiceDeliveryChallan_Details");
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
            ProcedureExecute proc = new ProcedureExecute("_p_CRMTagging_Details");
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

        public DataTable GetComponentInvoicedeliveryChallan(string Customer, string Date, string ComponentType, string FinYear, string BranchID, string Action, string strInvoiceID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("InvoiceDeliveryChallan_Tagging_Details");
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
        public DataTable GetComponentForPurchasechallan(string Customer, string Date, string ComponentType, string FinYear, string BranchID, string Action, string strInvoiceID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("_p_ProjectCRMTagging_Details");
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
            ProcedureExecute proc = new ProcedureExecute("_p_CRMTagging_Details");
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
        public DataTable GetComponentInvoicedeliveryChallan(string Customer, string Date, string ComponentType, string FinYear, string BranchID, string Action, string strInvoiceID, string inventory = "", string Entry_type = "")
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("InvoiceDeliveryChallan_Tagging_Details");
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
        public DataTable GetComponentInvDElvChallan(string Customer, string Date, string ComponentType, string FinYear, string BranchID, string Action, string strInvoiceID, string inventory = "", string Entry_type = "")
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("InvoiceDeliveryChallan_Tagging_Details");
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
            ProcedureExecute proc = new ProcedureExecute("prc_CRMTransitSalesInvoice_Details");
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
            ProcedureExecute proc = new ProcedureExecute("_p_CRMTagging_Details");
            proc.AddVarcharPara("@Action", 500, Action);
            proc.AddVarcharPara("@ComponentList", 2000, ComponentList);
            proc.AddVarcharPara("@InvoiceID", 20, InvoiceID);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetInvDelvChallanProductList(string Action, string ComponentList, string InvoiceID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("InvoiceDeliveryChallan_Tagging_Details");
            proc.AddVarcharPara("@Action", 500, Action);
            proc.AddVarcharPara("@ComponentList", 2000, ComponentList);
            proc.AddVarcharPara("@InvoiceID", 20, InvoiceID);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetTransitPurchaseInvoiceProducts(string Action, string ComponentList, string InvoiceID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMTransitSalesInvoice_Details");
            proc.AddVarcharPara("@Action", 500, "GetTransitPurchaseInvoiceProducts");
            proc.AddVarcharPara("@ComponentList", 2000, ComponentList);
            proc.AddVarcharPara("@InvoiceID", 20, InvoiceID);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetSelectedComponentProductList(string Action, string SelectedComponentList, string InvoiceID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("_p_CRMTagging_Details");
            proc.AddVarcharPara("@Action", 500, Action);
            proc.AddVarcharPara("@SelectedComponentList", 2000, SelectedComponentList);
            proc.AddVarcharPara("@InvoiceID", 20, InvoiceID);
            dt = proc.GetTable();
            return dt;
        }
        public DataSet GetSelectedSalesInvoiceComponentProductList(string Action, string SelectedComponentList, string InvoiceID)
        {
            DataSet dt = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("_p_CRMTagging_Details");
            proc.AddVarcharPara("@Action", 500, Action);
            proc.AddVarcharPara("@SelectedComponentList", 2000, SelectedComponentList);
            proc.AddVarcharPara("@InvoiceID", 20, InvoiceID);
            dt = proc.GetDataSet();
            return dt;
        }
        // Rev Sanchita
        public DataSet GetSelectedSalesInvoiceComponentProductList_New(string Action, string SelectedComponentList, string InvoiceID)
        {
            DataSet dt = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("_p_CRMTagging_Details");
            proc.AddVarcharPara("@Action", 500, Action);
            proc.AddVarcharPara("@SelectedComponentList", 2000, SelectedComponentList);
            proc.AddVarcharPara("@InvoiceID", 20, InvoiceID);
            dt = proc.GetDataSet();
            return dt;
        }
        // End of Rev Sanchita
        public DataSet GetSalesOrderProductsTaggedInInvoice(string InvoiceID)
        {
            DataSet dt = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_InvoiceDeliveryChallan_Details");
            proc.AddVarcharPara("@Action", 500, "GetOrderProductsTaggedInInvoice");
            proc.AddVarcharPara("@InvoiceID", 20, InvoiceID);
            dt = proc.GetDataSet();
            return dt;
        }
        public DataSet GetSelectedInvoiceDElvChalanProductList(string Action, string SelectedComponentList, string InvoiceID)
        {
            DataSet dt = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("InvoiceDeliveryChallan_Tagging_Details");
            proc.AddVarcharPara("@Action", 500, Action);
            proc.AddVarcharPara("@SelectedComponentList", 2000, SelectedComponentList);
            proc.AddVarcharPara("@InvoiceID", 20, InvoiceID);
            dt = proc.GetDataSet();
            return dt;
        }
        public DataTable GetSelectedProductwiseMultiUOMList(string Action, string SelectedComponentList, string InvoiceID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("_p_CRMTagging_Details");
            proc.AddVarcharPara("@Action", 500, Action);
            proc.AddVarcharPara("@SelectedComponentList", 2000, SelectedComponentList);
            proc.AddVarcharPara("@InvoiceID", 20, InvoiceID);
            dt = proc.GetTable();
            return dt;
        }
        // Rev Sanchita
        public DataTable GetSelectedProductwiseMultiUOMList_New (string Action, string SelectedComponentList, string InvoiceID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("_p_CRMTagging_Details");
            proc.AddVarcharPara("@Action", 500, Action);
            proc.AddVarcharPara("@SelectedComponentList", 2000, SelectedComponentList);
            proc.AddVarcharPara("@InvoiceID", 20, InvoiceID);
            dt = proc.GetTable();
            return dt;
        }
        // End of Rev Sanchita
        public DataTable GetInvDelvchallanProductwiseMultiUOMList(string Action, string SelectedComponentList, string InvoiceID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("InvoiceDeliveryChallan_Tagging_Details");
            proc.AddVarcharPara("@Action", 500, Action);
            proc.AddVarcharPara("@SelectedComponentList", 2000, SelectedComponentList);
            proc.AddVarcharPara("@InvoiceID", 20, InvoiceID);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetInvDelvchallanProductwiseSalesOrderMultiUOMList(string InvoiceID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("InvoiceDeliveryChallan_Tagging_Details");
            proc.AddVarcharPara("@Action", 500, "GetSalesOrderMultiUOMData");
            proc.AddVarcharPara("@InvoiceID", 20, InvoiceID);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetSeletedTransitPurchaseInvoiceProducts(string Action, string SelectedComponentList, string InvoiceID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMTransitSalesInvoice_Details");
            proc.AddVarcharPara("@Action", 500, "GetSeletedTransitPurchaseInvoiceProducts");
            proc.AddVarcharPara("@SelectedComponentList", 2000, SelectedComponentList);
            proc.AddVarcharPara("@InvoiceID", 20, InvoiceID);
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
        public DataTable GetLinkedInvDelvChallanProductList(string Action, string ProductID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_InvoiceDeliveryChallan_Details");
            proc.AddVarcharPara("@Action", 500, Action);
            proc.AddVarcharPara("@ProductID", 2000, ProductID);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable IsMinSalePriceOk(string ProductList)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesInvoice_Details");
            proc.AddVarcharPara("@Action", 500, "IsMinSalePriceOk");
            proc.AddVarcharPara("@ProductSalePriceList", 5000, ProductList);
            DataTable ReturnTable = proc.GetTable();
            return ReturnTable;
        }
        public DataTable IsInvDelvChallanMinSalePriceOk(string ProductList)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_InvoiceDeliveryChallan_Details");
            proc.AddVarcharPara("@Action", 500, "IsMinSalePriceOk");
            proc.AddVarcharPara("@ProductSalePriceList", 5000, ProductList);
            DataTable ReturnTable = proc.GetTable();
            return ReturnTable;
        }
        public DataTable EditEWayBill(string DocID, string Action)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_EwayBillListingValue_Edit");
            proc.AddPara("@ACTION", Action);
            proc.AddBigIntegerPara("@DocId", Convert.ToInt32(DocID));
            dt = proc.GetTable();
            return dt;
        }
        /*Mantise work 24702 04.03.2022*/
        public DataTable EditPartyInvDT(string DocID, string Action)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_UpdatePartyInvNoDate");
            proc.AddPara("@ACTION", Action);
            proc.AddBigIntegerPara("@DocId", Convert.ToInt32(DocID));
            dt = proc.GetTable();
            return dt;
        }
        /*Clsoe of mantise work 24702 04.03.2022*/
        public DataTable GetTdsSectionByID(string strInvoiceID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_InvoiceDeliveryChallan_Details");
            proc.AddVarcharPara("@Action", 500, "GetTdsSectionByID");
            proc.AddVarcharPara("@InvoiceID", 500, strInvoiceID);
            dt = proc.GetTable();
            return dt;
        }
    }
}
