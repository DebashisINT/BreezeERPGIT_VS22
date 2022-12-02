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
    public class CRMSalesDtlBL
    {
        #region Quotation List Section Start
        public DataTable GetQuotationListGridData(string userbranchlist, string lastCompany, string finyear, string BranchID, DateTime FromDate, DateTime ToDate)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 500, "GetQuotationListGridData");
            proc.AddVarcharPara("@userbranchlist", 500, userbranchlist);
            proc.AddVarcharPara("@lastCompany", 50, lastCompany);
            proc.AddVarcharPara("@FinYear", 50, finyear);
            proc.AddVarcharPara("@branchId", 3000, BranchID);
            proc.AddDateTimePara("@FromDate", FromDate);
            proc.AddDateTimePara("@ToDate", ToDate);
            dt = proc.GetTable();
            return dt;
        }


        public DataTable GetQuotationListGridData(string userbranchlist, string lastCompany, string Finyear)
        {
            DataTable dt = new DataTable();

            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 500, "GetQuotationListGridDataOpening");
            proc.AddVarcharPara("@userbranchlist", 500, userbranchlist);
            proc.AddVarcharPara("@lastCompany", 50, lastCompany);
            proc.AddVarcharPara("@FinYear", 50, Finyear);

            dt = proc.GetTable();

            return dt;
        }


        public int GetUserLevelByUserID(int userid)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 100, "GetUserLevelByUserID");
            proc.AddIntegerPara("@userid", userid);
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }

        public DataTable PopulateApprovalPendingCountByUserLevel(int userlevel)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 500, "PopulateApprovalPendingCountByUserLevel");
            proc.AddIntegerPara("@userlevel", userlevel);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetPendingQuotationListByUserLevel(int userlevel)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 500, "GetPendingQuotationListByUserLevel");
            proc.AddIntegerPara("@userlevel", userlevel);
            dt = proc.GetTable();
            return dt;
        }

        public Int64 UpdatePendingQuotationListByUserLevel(string chkbox, int keyval, int userlevel)
        {
            object result = -1;
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 500, "UpdateTblTransQuoStatus");
            proc.AddIntegerPara("@userlevel", userlevel);
            proc.AddIntegerPara("@QuotationID", keyval);
            proc.AddVarcharPara("@S_QuoteAdd_addressType", 100, chkbox);
            result = proc.GetScalar();
            return Convert.ToInt64(result);
        }

        public int UpdateQuotationStatusByCustomer(int Qouteid, int QuoteStatus, string remarks)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 100, "UpdateQuotationStatusByCustomer");
            proc.AddVarcharPara("@remarks", 500, remarks);
            proc.AddIntegerPara("@Quote_Id", Qouteid);
            proc.AddIntegerPara("@QuoteStatus", QuoteStatus);
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }
        public int UpdateQuotationOpenByCustomer(int Qouteid, string OpenReason, string status, string OpenCloseRemarks)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 100, "UpdateQuotationOpenByCustomer");
            proc.AddVarcharPara("@OpenReason", 100, OpenReason);
            proc.AddIntegerPara("@Quote_Id", Qouteid);
            proc.AddVarcharPara("@StatusType",50, status);
            proc.AddVarcharPara("@remarks", 500, OpenCloseRemarks);
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }
        public int UpdateQuotationCloseByCustomer(int Qouteid, string CloseReason, string OpenCloseRemarks)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 100, "UpdateQuotationCloseByCustomer");
            proc.AddVarcharPara("@OpenReason", 100, CloseReason);
            proc.AddIntegerPara("@Quote_Id", Qouteid);
            proc.AddVarcharPara("@remarks", 500, OpenCloseRemarks);
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }
        public int UpdatePurchaseQuotationStatusByCustomer(int Qouteid, int QuoteStatus, string remarks)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseQuotation_Details");
            proc.AddVarcharPara("@Action", 100, "UpdatePurchaseQuotationStatusByCustomer");
            proc.AddVarcharPara("@Remarks", 500, remarks);
            proc.AddIntegerPara("@Quote_Id", Qouteid);
            proc.AddIntegerPara("@QuoteStatus", QuoteStatus);
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }
        public int UpdateInquiryStatusByCustomer(int Qouteid, int QuoteStatus, string remarks)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 100, "UpdateInquiryStatusByCustomer");
            proc.AddVarcharPara("@remarks", 500, remarks);
            proc.AddIntegerPara("@Quote_Id", Qouteid);
            proc.AddIntegerPara("@QuoteStatus", QuoteStatus);
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }

        public int GetIdFromSalesOrder(string Quote_Id)
        {
            DataTable dt = new DataTable();
            int i = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 500, "IsSalesOrderIdExistInQuatation");
            proc.AddVarcharPara("@Quote_Id", 500, Convert.ToString(Quote_Id));
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
        public int IsPurchaseQuotationExistsInPurchaseOrder(string Quote_Id)
        {
            DataTable dt = new DataTable();
            int i = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseQuotation_Details");
            proc.AddVarcharPara("@Action", 500, "IsPurchaseQuotationExistsInPurchaseOrder");
            proc.AddVarcharPara("@Quote_Id", 500, Convert.ToString(Quote_Id));
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


        public int DeleteCheckForSalesInquiry(string Quote_Id)
        {
            DataTable dt = new DataTable();
            int i = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 500, "IsSalesInquiryExistInQuatation");
            proc.AddVarcharPara("@Quote_Id", 500, Convert.ToString(Quote_Id));
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

        public int DeleteQuotation(string Quoteid)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSales");
            proc.AddVarcharPara("@Action", 100, "DeleteQuotation");
            proc.AddVarcharPara("@DeleteQuoteId", 20, Quoteid);
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }
        public int DeletePurchaseQuotation(string Quoteid)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("Prc_PurchaseQuotation");
            proc.AddVarcharPara("@Action", 100, "DeleteQuotation");
            proc.AddVarcharPara("@DeleteQuoteId", 20, Quoteid);
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }
        public int DeleteSalesInquiry(string Quoteid)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_SalesInquirySave");
            proc.AddVarcharPara("@Action", 100, "DeleteQuotation");
            proc.AddVarcharPara("@DeleteQuoteId", 20, Quoteid);
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }
        #region New Code Added By Sam on 25022017 Start
        public DataTable PopulateUserWiseQuotationStatus(int userid)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 100, "PopulateUserWiseQuotationStatus");
            proc.AddIntegerPara("@userid", userid);
            dt = proc.GetTable();
            return dt;
        }

        #endregion New Code Added By Sam on 25022017 Start
        public DataTable IsExistsQuotationInApproveModule(int quoteid)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddNVarcharPara("@action", 150, "IsExistsQuotationInApproveModule");
            proc.AddIntegerPara("@Quote_Id", quoteid);
            dt = proc.GetTable();
            return dt;
        }

        #endregion Quotation List Section

        #region Quotation Section Start

        public int QuotationEditablePermission(int userid)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 100, "QuotationEditablePermission");
            proc.AddIntegerPara("@userid", userid);
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }

        public string EditablePermission(int SalesInvoiceID,string Action)
        {
            int i;
            string rtrnvalue = "";
            ProcedureExecute proc = new ProcedureExecute("Prc_EInvoiceCheckingSales_details");
            proc.AddVarcharPara("@Action", 100, Action);
            proc.AddVarcharPara("@Module", 100, "SalesInvoice");
            proc.AddIntegerPara("@DocumentId", SalesInvoiceID);
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToString(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }
        public string EditablePermissionInvoiceChallan(int SalesInvoiceID, string Action)
        {
            int i;
            string rtrnvalue = "";
            ProcedureExecute proc = new ProcedureExecute("Prc_EInvoiceCheckingSales_details");
            proc.AddVarcharPara("@Action", 100, Action);
            proc.AddVarcharPara("@Module", 100, "SalesInvoiceCumChallan");
            proc.AddIntegerPara("@DocumentId", SalesInvoiceID);
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToString(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }
        public int PurchaseQuotationEditablePermission(int userid)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseQuotation_Details");
            proc.AddVarcharPara("@Action", 100, "PurchaseQuotationEditablePermission");
            proc.AddIntegerPara("@userid", userid);
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }
        public int PurchaseQuotationEditablePermission(int userid, int SalesDocId)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseQuotation_Details");
            proc.AddVarcharPara("@Action", 100, "PurchaseQuotationEditablePermission");
            proc.AddIntegerPara("@userid", userid);
            proc.AddIntegerPara("@SalesDocId", SalesDocId);
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }
        public int SalesInquiryEditablePermission(int userid)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 100, "SalesInquiryEditablePermission");
            proc.AddIntegerPara("@userid", userid);
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }

        //REV RAJDIP
        public int QuotationEditablePermissions(int userid, int SalesDocId)
        //END REV RAJDIP
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 100, "QuotationEditablePermission");
            proc.AddIntegerPara("@userid", userid);
            proc.AddIntegerPara("@SalesDocId", SalesDocId);
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }


        public int SalesInquiryEditablePermissions(int userid, int SalesDocId)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 100, "SalesInquiryEditablePermission");
            proc.AddIntegerPara("@userid", userid);
            proc.AddIntegerPara("@SalesDocId", SalesDocId);
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }


        public DataTable GetQuotationStatusByQuotationID(string quoteid)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddNVarcharPara("@action", 150, "GetQuotationStatusByQuotationID");
            proc.AddIntegerPara("@Quote_Id", Convert.ToInt32(quoteid));
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetPurchaseQuotationStatusByQuotationID(string quoteid)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseQuotation_Details");
            proc.AddNVarcharPara("@action", 150, "GetPurchaseQuotationStatusByQuotationID");
            proc.AddIntegerPara("@Quote_Id", Convert.ToInt32(quoteid));
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetSalesInquiryStatusByInquiryID(string quoteid)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddNVarcharPara("@action", 150, "GetSalesInquiryStatusByInquiryID");
            proc.AddIntegerPara("@Quote_Id", Convert.ToInt32(quoteid));
            dt = proc.GetTable();
            return dt;
        }

        public int CheckCustomerBillingShippingAddress(string customerid)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddNVarcharPara("@action", 150, "CheckCustomerBillingShippingAddress");
            proc.AddVarcharPara("@customerid", 10, customerid);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;
        }

        public DataSet PopulateBillingandShippingDetailByCustomerID(string customerid)
        {

            DataSet dst = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddNVarcharPara("@action", 150, "PopulateBillingandShippingDetailByCustomerID");
            proc.AddNVarcharPara("@customerid", 10, customerid);
            dst = proc.GetDataSet();
            return dst;
        }

        public DataTable PopulateAddressDetailByAddressId(int Addressid)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddNVarcharPara("@action", 150, "PopulateAddressDetailByAddressId");
            proc.AddIntegerPara("@Addressid", Addressid);
            dt = proc.GetTable();
            return dt;
        }
        #endregion Quotation Section End

        #region Subhra Address Section
        public int InsertProduct(string Action, int QuoteAdd_QuoteId, string companyId, int @S_QuoteAdd_BranchId, string fin_year, string contactperson, string AddressType, string address1, string address2, string address3, string landmark, int country, int State, int city, string pin, int area)
        {

            ProcedureExecute proc;
            try
            {
                using (proc = new ProcedureExecute("prc_SalesCRM_Details"))
                {
                    proc.AddVarcharPara("@Action", 100, Action);
                    proc.AddIntegerPara("@S_QuoteAdd_QuoteId", QuoteAdd_QuoteId);
                    proc.AddVarcharPara("@S_QuoteAdd_CompanyID", 10, companyId);
                    proc.AddIntegerPara("@S_QuoteAdd_BranchId", @S_QuoteAdd_BranchId);
                    proc.AddVarcharPara("@S_QuoteAdd_FinYear", 1, fin_year);
                    proc.AddVarcharPara("@S_QuoteAdd_ContactPerson", 50, contactperson);
                    proc.AddVarcharPara("@S_QuoteAdd_addressType", 50, AddressType);
                    proc.AddVarcharPara("@S_QuoteAdd_address1", 500, address1);
                    proc.AddVarcharPara("@S_QuoteAdd_address2", 500, address2);
                    proc.AddVarcharPara("@S_QuoteAdd_address3", 500, address3);
                    proc.AddVarcharPara("@S_QuoteAdd_landMark", 500, landmark);

                    proc.AddIntegerPara("@S_QuoteAdd_countryId", country);
                    proc.AddIntegerPara("@S_QuoteAdd_stateId", State);
                    proc.AddIntegerPara("@S_QuoteAdd_cityId", city);
                    proc.AddVarcharPara("@S_QuoteAdd_pin", 12, pin);
                    proc.AddIntegerPara("@S_QuoteAdd_areaId", area);
                    proc.AddIntegerPara("@S_QuoteAdd_CreatedUser", Convert.ToInt32(HttpContext.Current.Session["userid"]));

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

        #endregion Subhra Address Section


        #region Priti Section Start
        #region Purchase Indent
        public DataTable GetPurchaseIndentListGridData()
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 500, "PurchaseIndent");
            dt = proc.GetTable();
            return dt;
        }
        #endregion
        #endregion


        public string SavePOSMainBill(string SchemaID, string DocNo, string invoice_id, string Challan_SchemaID, string Challan_Doc)
        {

            string output = "";
            try
            {
                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_ApprovalSave");
                proc.AddVarcharPara("@Action", 500, "SavePOSMainBill");
                proc.AddVarcharPara("@SchemaID", 500, SchemaID.Split('~')[0]);
                proc.AddVarcharPara("@DocNo", 500, DocNo);
                proc.AddVarcharPara("@invoice_id", 500, invoice_id);
                proc.AddVarcharPara("@Challan_SchemaID", 500, Challan_SchemaID.Split('~')[0]);
                proc.AddVarcharPara("@Challan_Doc", 500, Challan_Doc);

                dt = proc.GetTable();

                if (dt != null && dt.Rows.Count > 0)
                {
                    if (Convert.ToString(dt.Rows[0][0]) == "Document number already exists. Can not proceed.")
                    {
                        output = "0~" + Convert.ToString(dt.Rows[0][0]) + "~~";
                    }
                    else
                    {
                        output = "1~" + Convert.ToString(dt.Rows[0][0]) + "~" + Convert.ToString(dt.Rows[0][1]) + "~" + Convert.ToString(dt.Rows[0][2]);
                    }
                }
            }
            catch (Exception e)
            {
                output = "0~" + Convert.ToString(e.InnerException.Message) + "~~";
            }
            return output;

        }

        public DataTable GetWarehouseDataTable(string invoice_id)
        {
            try
            {
                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_ApprovalSave");
                proc.AddVarcharPara("@Action", 500, "GetPOSMainBill");
                proc.AddVarcharPara("@invoice_id", 500, invoice_id);
                dt = proc.GetTable();
                return dt;
            }
            catch
            {
                return null;
            }
        }



    }
}
