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
   public class TransitSalesInvoiceBL
   {

       public DataSet GetAllDropDownDetailForSalesInvoice(string userbranch, string CompanyID, string BranchID)
       {
           DataSet ds = new DataSet();
           ProcedureExecute proc = new ProcedureExecute("prc_TransitSalesInvoice_Details");
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
            ProcedureExecute proc = new ProcedureExecute("prc_TransitSalesInvoice_Details");
            proc.AddVarcharPara("@Action", 100, "PopulateCustomerDetail");
            dt = proc.GetTable();
            return dt;
        }
        public DataTable PopulateCustomerDetailForTransitSalesInvoice()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_TransitSalesInvoice_Details");
            proc.AddVarcharPara("@Action", 100, "PopulateCustomerDetailForTransitSalesInvoice");
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetCustomerDetails_InvoiceRelated(string strCustomerID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_TransitSalesInvoice_Details");
            proc.AddVarcharPara("@Action", 100, "GetCustomerInvoiceDetails");
            proc.AddVarcharPara("@CustomerID", 1000, strCustomerID);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetCustomerTotalDues(string strCustomerID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_TransitSalesInvoice_Details");
            proc.AddVarcharPara("@Action", 100, "GetCustomerTotalDues");
            proc.AddVarcharPara("@CustomerID", 1000, strCustomerID);
            dt = proc.GetTable();
            return dt;
        }
        public DataSet GetNecessaryData(string strComponentIDs, string strType, string CustomerID)
        {
            DataSet dt = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_TransitSalesInvoice_Details");
            proc.AddVarcharPara("@Action", 100, "GetComponentSelectData");
            proc.AddVarcharPara("@SelectedComponentList", 1000, strComponentIDs);
            proc.AddVarcharPara("@ComponentType", 1000, strType);
            proc.AddVarcharPara("@CustomerID", 12, CustomerID);
            dt = proc.GetDataSet();
            return dt;
        }
        public DataTable GetQuotationList_GridData(string userbranchlist, string lastCompany, string Fiyear)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_TransitSalesInvoice_Details");
            proc.AddVarcharPara("@Action", 500, "GetQuotationListGridData");
            proc.AddVarcharPara("@BranchList", 3000, userbranchlist);
            proc.AddVarcharPara("@CompanyID", 50, lastCompany);
            proc.AddVarcharPara("@FinYear", 50, Fiyear);
            dt = proc.GetTable();
            return dt;
        }


        //public DataTable GetTransitSalesInvoiceListGridData(string userbranchlist, string lastCompany, string Fiyear)
        //{

        //    DataTable dt = new DataTable();
        //    ProcedureExecute proc = new ProcedureExecute("prc_TransitSalesInvoice_Details");
        //    proc.AddVarcharPara("@Action", 500, "GetTransitSalesInvoiceListGridData");
        //    proc.AddVarcharPara("@BranchList", 3000, userbranchlist);
        //    proc.AddVarcharPara("@CompanyID", 50, lastCompany);
        //    proc.AddVarcharPara("@FinYear", 50, Fiyear);
        //    dt = proc.GetTable();
        //    return dt;
        //}


        public DataTable GetTransitSalesInvoiceListGridDataByDate(string userbranchlist, string lastCompany, string finyear, string branch, DateTime fromdate, DateTime todate)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_TransitSalesInvoice_Details");
            //proc.AddVarcharPara("@Action", 500, "GetQuotationListGridDataByDate");
            proc.AddVarcharPara("@Action", 500, "GetTransitSalesInvoiceListGridDataByDate");
            proc.AddVarcharPara("@BranchList", 1500, userbranchlist);
            proc.AddVarcharPara("@CompanyID", 50, lastCompany);
            proc.AddVarcharPara("@FinYear", 50, finyear); 
            proc.AddDateTimePara("@fromdate",  fromdate);
            proc.AddDateTimePara("@todate",  todate); 
            proc.AddVarcharPara("@branchId", 50, branch);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetTransitSalesInvoiceListGridData(string userbranchlist, string lastCompany, string Fiyear)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_TransitSalesInvoice_Details");
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
            ProcedureExecute proc = new ProcedureExecute("prc_TransitSalesInvoice_Details");
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
            ProcedureExecute proc = new ProcedureExecute("prc_TransitSalesInvoice_Details");
            proc.AddVarcharPara("@Action", 500, "GetTotalDues");
            proc.AddVarcharPara("@BranchList", 3000, userbranchlist);
            proc.AddVarcharPara("@CompanyID", 50, lastCompany);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetInvoiceEditData(string InvoiceID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_TransitSalesInvoice_Details");
            proc.AddVarcharPara("@Action", 500, "InvoiceEditDetails");
            proc.AddVarcharPara("@InvoiceID", 500, InvoiceID);
            dt = proc.GetTable();
            return dt;
        }


        public DataSet ModifyOfInvoiceEditData(string InvoiceID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_TransitSalesInvoice_Details");
            proc.AddVarcharPara("@Action", 500, "ModifyOfTransitInvoiceEditDetails");
            proc.AddVarcharPara("@InvoiceID", 500, InvoiceID);
            ds = proc.GetDataSet();
            return ds;
        }

        public int DeleteInvoice(string Invoiceid,string UserID)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_TransitSalesInvoice");
            proc.AddVarcharPara("@Action", 100, "DeleteInvoice");
            proc.AddVarcharPara("@DeleteInvoiceId", 20, Invoiceid);
            proc.AddVarcharPara("@UserID", 100, UserID);
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }
        public DataSet GetInvoiceProductData(string strInvoiceID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_TransitSalesInvoice_Details");
            proc.AddVarcharPara("@Action", 500, "ProductDetails");
            proc.AddVarcharPara("@InvoiceID", 500, strInvoiceID);
            ds = proc.GetDataSet();
            return ds;
        }
        public DataTable GetInvoiceBillingAddress(string strInvoiceID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_TransitSalesInvoice_Details");
            proc.AddVarcharPara("@Action", 500, "InvoiceBillingAddress");
            proc.AddVarcharPara("@InvoiceID", 500, strInvoiceID);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetComponent(string Customer, string Date, string ComponentType, string FinYear, string BranchID, string Action, string strInvoiceID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_TransitSalesInvoice_Details");
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
        public DataTable GetComponentForTransitPurchaseInvoice(string Customer, string Date, string ComponentType, string FinYear, string BranchID, string Action, string strInvoiceID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_TransitSalesInvoice_Details");
            proc.AddVarcharPara("@Action", 500, "GetComponentForTransitPurchaseInvoice");
            proc.AddVarcharPara("@CustomerID", 500, Customer);
            proc.AddDateTimePara("@Date", Convert.ToDateTime(Date));
            proc.AddVarcharPara("@ComponentType", 10, ComponentType);
            proc.AddVarcharPara("@FinYear", 10, FinYear);
            proc.AddVarcharPara("@BranchID", 3000, BranchID);
            proc.AddVarcharPara("@InvoiceID", 20, strInvoiceID);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetComponentForTransitPurchaseInvoiceWithProject(string Customer, string Date, string ComponentType, string FinYear, string BranchID, string Action, string strInvoiceID, Int64 Project_Id)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_TransitSalesInvoice_Details");
            proc.AddVarcharPara("@Action", 500, "GetComponentForTransitPurchaseInvoiceWithProject");
            proc.AddVarcharPara("@CustomerID", 500, Customer);
            proc.AddDateTimePara("@Date", Convert.ToDateTime(Date));
            proc.AddVarcharPara("@ComponentType", 10, ComponentType);
            proc.AddVarcharPara("@FinYear", 10, FinYear);
            proc.AddVarcharPara("@BranchID", 3000, BranchID);
            proc.AddVarcharPara("@InvoiceID", 20, strInvoiceID);
            proc.AddBigIntegerPara("@Project_Id", Project_Id);
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
        public DataTable GetTransitPurchaseInvoiceProducts(string Action, string ComponentList, string InvoiceID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_TransitSalesInvoice_Details");
            proc.AddVarcharPara("@Action", 500, Action);
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
        public DataTable GetSeletedTransitPurchaseInvoiceProducts(string Action, string SelectedComponentList, string InvoiceID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_TransitSalesInvoice_Details");
            proc.AddVarcharPara("@Action", 500, Action);
            proc.AddVarcharPara("@SelectedComponentList", 2000, SelectedComponentList);
            proc.AddVarcharPara("@InvoiceID", 20, InvoiceID);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetLinkedProductList(string Action, string ProductID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_TransitSalesInvoice_Details");
            proc.AddVarcharPara("@Action", 500, Action);
            proc.AddVarcharPara("@ProductID", 2000, ProductID);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable IsMinSalePriceOk(string ProductList)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_TransitSalesInvoice_Details");
            proc.AddVarcharPara("@Action", 500, "IsMinSalePriceOk");
            proc.AddVarcharPara("@ProductSalePriceList", 5000, ProductList);
            DataTable ReturnTable = proc.GetTable();
            return ReturnTable;
        }

        public DataTable GetTdsSectionByID(string strInvoiceID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_TransitSalesInvoice_Details");
            proc.AddVarcharPara("@Action", 500, "GetTdsSectionByID");
            proc.AddVarcharPara("@InvoiceID", 500, strInvoiceID);
            dt = proc.GetTable();
            return dt;
        }
    }
}
