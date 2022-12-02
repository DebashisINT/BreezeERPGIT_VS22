using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BusinessLogicLayer
{
    public  class ProjectTermsBL
    {
        
        public int ProjectTermsCoditionsSave(DateTime DefLiaPer, string DefLiaPerRemarks, string Liqdamageper, DateTime LiqDamageApplicableDt, string PaymentTerms, string OrderType, string NatureofWork)
        {
            
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("Prc_Project_TermsConditions");
            proc.AddVarcharPara("@Action", 500, "Add");
            //proc.AddVarcharPara("@Doc_id", Vendorid);
            proc.AddDateTimePara("@Terms_DefectLibilityPeriodDate", DefLiaPer);
            proc.AddVarcharPara("@Terms_DefectLibilityPeriodRemarks", 500, DefLiaPerRemarks);
            proc.AddVarcharPara("@Terms_LiqDamage", 100, Liqdamageper);
            proc.AddDateTimePara("@Terms_LiqDamageAppDate", LiqDamageApplicableDt);
            proc.AddVarcharPara("@Terms_Payment", 100, PaymentTerms);
            proc.AddVarcharPara("@Terms_OrderType", 100, OrderType);
            proc.AddVarcharPara("@Terms_NatureWork", 100, NatureofWork);
            
           // proc.AddIntegerPara("@Terms_CreatedBy", Convert.ToInt32(Session["userid"]));
            proc.AddVarcharPara("@ReturnValue", 50, "", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }

        public int IsPurchaseQuotationExistsInPurchaseOrder(string Quote_Id)
        {
            DataTable dt = new DataTable();
            int i = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseQuotation_Details");
            proc.AddVarcharPara("@Action", 500, "IsProjectPurchaseQuotationExistsInPurchaseOrder");
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

        public int CancelPurchaseQuotation(string KeyVal, string Reason)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseQuotation_Details");
            proc.AddVarcharPara("@Action", 100, "CancelProjectPurchaseQuotation");
            proc.AddVarcharPara("@Document_Id", 50, KeyVal);
            proc.AddVarcharPara("@companyId", 50, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@FinYear", 50, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            proc.AddVarcharPara("@Reason", 50, Convert.ToString(Reason));
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }
        public int PurchaseQuotationEditablePermission(int userid)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseQuotation_Details");
            proc.AddVarcharPara("@Action", 100, "ProjectPurchaseQuotationEditablePermission");
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
            proc.AddVarcharPara("@Action", 100, "ProjectPurchaseQuotationEditablePermission");
            proc.AddIntegerPara("@userid", userid);
            proc.AddIntegerPara("@SalesDocId", SalesDocId);
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }

        public DataTable GetPurchaseQuotationStatusByQuotationID(string quoteid)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseQuotation_Details");
            proc.AddNVarcharPara("@action", 150, "GetProjectPurchaseQuotationStatusByQuotationID");
            proc.AddIntegerPara("@Quote_Id", Convert.ToInt32(quoteid));
            dt = proc.GetTable();
            return dt;
        }
         public int UpdatePurchaseQuotationStatusByCustomer(int Qouteid, int QuoteStatus, string remarks)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseQuotation_Details");
            proc.AddVarcharPara("@Action", 100, "UpdateProjectPurchaseQuotationStatusByCustomer");
            proc.AddVarcharPara("@Remarks", 500, remarks);
            proc.AddIntegerPara("@Quote_Id", Qouteid);
            proc.AddIntegerPara("@QuoteStatus", QuoteStatus);
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }

         public int DeletePurchaseQuotation(string Quoteid)
         {
             int i;
             int rtrnvalue = 0;
             ProcedureExecute proc = new ProcedureExecute("Prc_ProjectPurchaseQuotation");
             proc.AddVarcharPara("@Action", 100, "DeleteQuotation");
             proc.AddVarcharPara("@DeleteQuoteId", 20, Quoteid);
             proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
             i = proc.RunActionQuery();
             rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
             return rtrnvalue;

         }


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

         public DataTable GetPindentDate(string Quote_Nos)
         {
             ProcedureExecute proc = new ProcedureExecute("Prc_GetQuotationDetails");
             proc.AddVarcharPara("@Quote_Number", 100, Quote_Nos);
             proc.AddVarcharPara("@Mode", 100, "GetPindentDate");

             return proc.GetTable();
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

         public DataTable GetQuotationDetailsFromPO(string Indent_Id, string Order_Key, string Product_Ids, string Action)
         {
             ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseOrderDetailsList");
             proc.AddVarcharPara("@Action", 100, "GetProjectPurchaseQuotationDetailsOnly");
             proc.AddVarcharPara("@Indent_Id", 4000, Indent_Id);
             proc.AddIntegerPara("@PurchaseOrder_Id", Convert.ToInt32(Order_Key));
             proc.AddVarcharPara("@Mode", 10, Action);
             return proc.GetTable();
         }
         public DataTable GetIndentDetailsFromPO(string Indent_Id, string Order_Key, string Product_Ids, string Action)
         {
             ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseOrderDetailsList");
             proc.AddVarcharPara("@Action", 100, "GetIndentDetailsOnly");
             proc.AddVarcharPara("@Indent_Id", 4000, Indent_Id);
             proc.AddIntegerPara("@PurchaseOrder_Id", Convert.ToInt32(Order_Key));
             proc.AddVarcharPara("@Mode", 10, Action);
             return proc.GetTable();
         }

         public DataSet GetIndentDetailsForPOGridBind(string Indent_Id, string Order_Key, string Product_Ids, string Action, string strPOID)
         {
             ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseOrderDetailsList");
             proc.AddVarcharPara("@Action", 100, "GetIndentDetailsForGridBind");
             proc.AddVarcharPara("@Indent_Id", 4000, Indent_Id);
             proc.AddVarcharPara("@IndentDetails_Id", 1000, Order_Key);
             proc.AddVarcharPara("@Product_Id", 1000, Product_Ids);
             proc.AddVarcharPara("@Mode", 10, Action);
             proc.AddVarcharPara("@FinYear", 500, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
             proc.AddVarcharPara("@campany_Id", 500, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
             proc.AddVarcharPara("@POID", 20, strPOID);
             return proc.GetDataSet();
         }
         public DataSet GetPQDetailsForPOGridBind(string Indent_Id, string Order_Key, string Product_Ids, string Action)
         {
             ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseOrderDetailsList");
             proc.AddVarcharPara("@Action", 100, "GetPQDetailsForGridBind");
             proc.AddVarcharPara("@Indent_Id", 4000, Indent_Id);
             proc.AddVarcharPara("@IndentDetails_Id", 1000, Order_Key);
             proc.AddVarcharPara("@Product_Id", 1000, Product_Ids);
             proc.AddVarcharPara("@Mode", 10, Action);
             proc.AddVarcharPara("@FinYear", 500, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
             proc.AddVarcharPara("@campany_Id", 500, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
             return proc.GetDataSet();
         }
         //public DataTable GetIndentDetailsForPOGridBind(string Indent_Id, string Order_Key, string Product_Ids, string Action)
         //{
         //    ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseOrderDetailsList");
         //    proc.AddVarcharPara("@Action", 100, "GetIndentDetailsForGridBind");
         //    proc.AddVarcharPara("@Indent_Id", 4000, Indent_Id);
         //    proc.AddVarcharPara("@IndentDetails_Id", 1000, Order_Key);
         //    proc.AddVarcharPara("@Product_Id", 1000, Product_Ids);
         //    proc.AddVarcharPara("@Mode", 10, Action);
         //    proc.AddVarcharPara("@FinYear", 500, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
         //    proc.AddVarcharPara("@campany_Id", 500, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
         //    return proc.GetTable();
         //}

         public DataTable GetQuotationOnPO(string OrderDate, string Status, string branch)
         {
             DataTable dt = new DataTable();
             ProcedureExecute proc = new ProcedureExecute("prc_GetProjectPurchaseQuotationOnPurchaseOrder");
             proc.AddDateTimePara("@OrderDate", Convert.ToDateTime(OrderDate));
             proc.AddVarcharPara("@Status", 50, Status);
             // proc.AddIntegerPara("@branch",Convert.ToInt32(branch));
             proc.AddVarcharPara("@branch", 500, branch);
             proc.AddVarcharPara("@FinYear", 500, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
             proc.AddVarcharPara("@campany_Id", 500, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
             dt = proc.GetTable();
             return dt;
         }

         public DataTable GetIndentOnPO(string OrderDate, string Status, string branch)
         {
             DataTable dt = new DataTable();
             ProcedureExecute proc = new ProcedureExecute("prc_GetIndentOnProjectPurchaseOrder");
             proc.AddDateTimePara("@OrderDate", Convert.ToDateTime(OrderDate));
             proc.AddVarcharPara("@Status", 50, Status);
             // proc.AddIntegerPara("@branch",Convert.ToInt32(branch));
             proc.AddVarcharPara("@branch", 500, branch);
             proc.AddVarcharPara("@FinYear", 500, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
             proc.AddVarcharPara("@campany_Id", 500, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
             dt = proc.GetTable();
             return dt;
         }

    }
}
