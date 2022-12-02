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
   public class ProjectPurchaseChallanBL
    {

        public int UpdateEWayBill(string InvoiceID, string EWayBillNumber, string EWayBillDate, string EWayBillValue)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchasechallan_Details");
            proc.AddVarcharPara("@Action", 100, "UpdateEWayBill");
            proc.AddBigIntegerPara("@PurchaseChallan_Id", Convert.ToInt32(InvoiceID));
            proc.AddVarcharPara("@EWayBillNumber", 50, EWayBillNumber);
            proc.AddVarcharPara("@EWayBillDate", 50, EWayBillDate);
            proc.AddVarcharPara("@EWayBillValue", 50, EWayBillValue);
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }
        public DataTable GetBranchIdByPCID(string pcid)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetail");
            proc.AddVarcharPara("@Action", 100, "GetBranchIdByPCID");
            proc.AddVarcharPara("@PCID", 10, pcid);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetFinacialYearBasedQouteDate(string FinYear)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchasechallan_Details");
            proc.AddVarcharPara("@Action", 100, "GetFinacialYearBasedQouteDate");
            proc.AddVarcharPara("@FinYear", 15, FinYear);
            dt = proc.GetTable();
            return dt;
        }

        public DataSet GetAllDropDownDetailForPurchaseChallan()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchasechallan_Details");
            proc.AddVarcharPara("@Action", 100, "GetAllDropDownDetailForPurchaseOrder");
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet GetNewAllDropDownDetailForPurchaseChallan(string branchHierchy)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchasechallan_Details");
            proc.AddVarcharPara("@Action", 100, "GetAllDropDownDetailForPurchaseOrder");
            proc.AddVarcharPara("@userbranchlist", -1, branchHierchy);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataTable GetPurchaseChallanListGridData(string Branch, string company, string finyear, string BranchID, DateTime FromDate, DateTime ToDate, string Entity_Type)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchasechallan_Details");
            proc.AddVarcharPara("@Action", 500, "PurchaseChallan");
            proc.AddVarcharPara("@userbranchlist", 500, Branch);
            proc.AddVarcharPara("@campany_Id", 500, company);
            proc.AddVarcharPara("@FinYear", 50, finyear);
            proc.AddVarcharPara("@branchid", 3000, BranchID);
            proc.AddDateTimePara("@FromDate", FromDate);
            proc.AddDateTimePara("@ToDate", ToDate);
            proc.AddVarcharPara("@Entity_Type", 2, Entity_Type);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable PopulateContactPersonOfCustomer(string InternalId)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchasechallan_Details");
            proc.AddVarcharPara("@Action", 100, "PopulateContactPersonOfCustomer");
            proc.AddVarcharPara("@InternalId", 100, InternalId);
            ds = proc.GetTable();
            return ds;
        }
        public DataTable PopulateGSTCSTVAT()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchasechallan_Details");
            proc.AddVarcharPara("@Action", 100, "PopulateGSTCSTVAT");
            dt = proc.GetTable();
            return dt;
        }


        public DataTable PopulateGSTCSTVAT(string challanDate)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchasechallan_Details");
            proc.AddVarcharPara("@Action", 100, "PopulateGSTCSTVATbyDate");
            proc.AddVarcharPara("@S_ChallanDate", 10, challanDate);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetCurrentConvertedRate(int BaseCurrencyId, int ConvertedCurrencyId, string CompID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchasechallan_Details");
            proc.AddVarcharPara("@Action", 100, "GetCurrentConvertedRate");
            proc.AddIntegerPara("@BaseCurrencyId", BaseCurrencyId);
            proc.AddIntegerPara("@ConvertedCurrencyId", ConvertedCurrencyId);
            proc.AddVarcharPara("@campany_Id", 10, CompID);
            dt = proc.GetTable();
            return dt;
        }
        public int DeletePurchaseChallan(string pcid, string comapanyid, string finyear)
        {
            int i;
            int rtrnvalue = 0;

            if (!string.IsNullOrEmpty(pcid) && !string.IsNullOrEmpty(comapanyid) && !string.IsNullOrEmpty(finyear))
            {
                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchasechallan_Details");
                proc.AddVarcharPara("@Action", 100, "DeletePCAll");
                proc.AddIntegerPara("@PurchaseChallan_Id", Convert.ToInt32(pcid));
                proc.AddVarcharPara("@campany_Id", 100, comapanyid);
                proc.AddVarcharPara("@FinYear", 100, finyear);
                dt = proc.GetTable();
            }

            return rtrnvalue;

        }

        public DataTable PopulateVendorsDetail()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchasechallan_Details");
            proc.AddVarcharPara("@Action", 100, "PopulateVendorsDetail");
            dt = proc.GetTable();
            return dt;
        }
        public DataTable PopulateVendorsDetailByBranch(string strBranch)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchasechallan_Details");
            proc.AddVarcharPara("@Action", 100, "PopulateVendorsDetailByBranch");
            proc.AddVarcharPara("@branchid", 100, strBranch);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetPO(string vendorid, string OrderDate, string Status)
        {
            DataTable dt = new DataTable();
            //      string PCdate=Convert.ToDateTime(OrderDate).ToString("yyyy/MM/dd");
            ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchasechallan_Details");
            proc.AddVarcharPara("@Action", 100, "GetPObyChallanDate");
            //proc.AddVarcharPara("@POrderDate", 200, OrderDate);@PoDateDatetime
            proc.AddDateTimePara("@PoDateDatetime", Convert.ToDateTime(OrderDate));
            proc.AddVarcharPara("@Status", 50, Status);
            proc.AddVarcharPara("@Customer_Id", 10, vendorid);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable CheckPCTraanaction(string vendorid)
        {
            DataTable dt = new DataTable();

            ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchasechallan_Details");
            proc.AddVarcharPara("@Action", 100, "ChallanTagedOrNotInInvoice");
            proc.AddVarcharPara("@PurchaseChallan_Id", 200, vendorid);

            dt = proc.GetTable();
            return dt;
        }
        public DataTable CheckPCTranslation(string vendorid)
        {
            DataTable dt = new DataTable();

            ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchasechallan_Details");
            proc.AddVarcharPara("@Action", 100, "IS_PC_Translation");
            proc.AddVarcharPara("@PurchaseChallan_Id", 200, vendorid);

            dt = proc.GetTable();
            return dt;
        }

        #region Get PO  Date
        public DataTable GetIndentRequisitionDate(string Indent_No)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchasechallan_Details");
            proc.AddVarcharPara("@PurchaseOrder_ID", 50, Indent_No);
            proc.AddVarcharPara("@Action", 100, "GetPODate");

            return proc.GetTable();
        }
        #endregion  Get PO  Date

        public DataTable GetIndentDetailsFromPO(string Indent_Id, string Order_Key, string Product_Ids)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchasechallan_Details");
            proc.AddVarcharPara("@Action", 100, "GetPODetailsOnly");
            proc.AddVarcharPara("@PurchaseOrder_ID", 4000, Indent_Id);
            // proc.AddVarcharPara("@OrderID_Key", 1000, Order_Key);
            return proc.GetTable();
        }
        public DataTable GetIndentDetailsForPOGridBind(string Indent_Id, string Order_Key, string Product_Ids, string comapanyid, string finyear)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchasechallan_Details");
            proc.AddVarcharPara("@Action", 100, "GetPOforPC");
            proc.AddVarcharPara("@PurchaseOrder_ID", 4000, Indent_Id);
            proc.AddVarcharPara("@OrderID_Key", 1000, Order_Key);
            proc.AddVarcharPara("@Product_Id", 1000, Product_Ids);
            proc.AddVarcharPara("@campany_Id", 100, comapanyid);
            proc.AddVarcharPara("@FinYear", 100, finyear);

            return proc.GetTable();
        }

        public DataSet PopulateBillingandShippingDetailByCustomerID(string customerid)
        {

            DataSet dst = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchasechallan_Details");
            proc.AddNVarcharPara("@Action", 150, "PopulateBillingandShippingDetailByCustomerID");
            proc.AddNVarcharPara("@Customer_Id", 10, customerid);
            dst = proc.GetDataSet();
            return dst;
        }

        public DataTable PopulateAddressDetailByAddressId(int Addressid)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchasechallan_Details");
            proc.AddNVarcharPara("@Action", 150, "PopulateAddressDetailByAddressId");
            proc.AddIntegerPara("@Product_Id", Addressid);
            dt = proc.GetTable();
            return dt;
        }

        #region Count Pending Approval Section Start
        public DataTable PopulateERPDocApprovalPendingCountByUserLevel(int userid, string doctype)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_ERPDocPendingApprovalDetail");
            proc.AddVarcharPara("@Action", 500, "PopulateERPDocApprovalPendingCountByUserLevel");
            proc.AddIntegerPara("@UserId", userid);
            proc.AddVarcharPara("@DocType", 5, doctype);
            dt = proc.GetTable();
            return dt;
        }

        #endregion Count Pending Approval Section End

        public DataTable GetPurchaseGRNPendingDeliveryListByDateFiltering(string companyID, string branchID, string frmDate = "", string toDate = "")
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_GetPendingDeliveryGRN");

            proc.AddVarcharPara("@BranchID", -1, (branchID == "0") ? Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]) : branchID);
            proc.AddVarcharPara("@CompanyID", 50, companyID.Trim());
            proc.AddVarcharPara("@FromDelivDate", 10, frmDate.Trim());
            proc.AddVarcharPara("@ToDelivDate", 10, toDate.Trim());
            proc.AddVarcharPara("@Action", -1, "PendingDeliveryGRNList");

            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetComponentProductList(string Action, string ComponentList, string InvoiceID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("_p_ProjectCRMTagging_Details");
            proc.AddVarcharPara("@Action", 500, Action);
            proc.AddVarcharPara("@ComponentList", 2000, ComponentList);
            proc.AddVarcharPara("@InvoiceID", 20, InvoiceID);
            dt = proc.GetTable();
            return dt;
        }

    }
}
