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
using System.Configuration;

namespace BusinessLogicLayer
{
  public   class ProjectPurchaseReturnBL
  {
      public bool IsPRExistINAdjtPR(string Poid)
      {
          bool IsExist = false;
          if (Poid != "" && Convert.ToString(Poid).Trim() != "")
          {
              DataTable dt = new DataTable();
              dt = ModifyCheck(Poid);
              if (dt.Rows.Count > 0 && Convert.ToString(dt.Rows[0]["isexist"]) != "0")
              {
                  IsExist = true;
              }
          }
          return IsExist;
      }

      public DataTable ModifyCheck(string poid)
      {
          DataTable dt = new DataTable();
          ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseReturn_Details");
          proc.AddVarcharPara("@Action", 100, "ModifyCheck");
          proc.AddVarcharPara("@PurchaseReturnID", 20, poid);
          dt = proc.GetTable();
          return dt;
      }
      public int UpdateEWayBillForPurchaseReturn(string ReturnID, string EWayBillNumber, string EWayBillDate, string EWayBillValue)
      {
          int i;
          int rtrnvalue = 0;
          ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseReturn_Details");
          proc.AddVarcharPara("@Action", 100, "UpdateEWayBill");
          proc.AddBigIntegerPara("@Return_ID", Convert.ToInt32(ReturnID));
          proc.AddVarcharPara("@EWayBillNumber", 50, EWayBillNumber);
          proc.AddVarcharPara("@EWayBillDate", 50, EWayBillDate);
          proc.AddVarcharPara("@EWayBillValue", 50, EWayBillValue);
          proc.AddVarcharPara("@Return_Value", 50, "0", QueryParameterDirection.Output);
          i = proc.RunActionQuery();
          rtrnvalue = Convert.ToInt32(proc.GetParaValue("@Return_Value"));
          return rtrnvalue;

      }

      public DataTable GetPurchaseReturnListGridData(string userbranchlist, string lastCompany, string ComponentType, string StartDate, string EndDate)
      {

          DataTable dt = new DataTable();
          ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseReturn_Details");
          proc.AddVarcharPara("@Action", 500, "GetPurchaseReturnListGridData");
          proc.AddNTextPara("@BranchList", userbranchlist);
          proc.AddVarcharPara("@CompanyID", 50, lastCompany);
          proc.AddVarcharPara("@ComponentType ", 50, ComponentType);
          proc.AddDateTimePara("@FinYearStartdate", Convert.ToDateTime(StartDate));
          proc.AddDateTimePara("@FinYearEnddate", Convert.ToDateTime(EndDate));
          dt = proc.GetTable();
          return dt;
      }


      public DataTable PopulatePrOnDemand(string filter, int startindex, int EndIndex, string companyid, string FinYear)
      {
          DataTable dt = new DataTable();
          ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseReturn_Details");
          proc.AddVarcharPara("@Action", 500, "ProductLoadOnDemandDetails");
          //proc.AddVarcharPara("@branchId", 20, branchId);
          proc.AddVarcharPara("@filter", 20, filter);
          proc.AddIntegerPara("@startindex", startindex);
          proc.AddIntegerPara("@EndIndex", EndIndex);
          proc.AddVarcharPara("@CompanyId", 10, companyid);
          proc.AddVarcharPara("@FinYear", 10, FinYear);


          dt = proc.GetTable();
          return dt;
      }
      public DataTable GetPRListGridData(string userbranchlist, string lastCompany, string ComponentType, string StartDate, string EndDate, string Fiyear, string userbranchID)
      {

          DataTable dt = new DataTable();
          ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseReturn_Details");
          proc.AddVarcharPara("@Action", 500, "GetPurchaseReturnListGridData");
          proc.AddNTextPara("@BranchList", userbranchlist);
          proc.AddVarcharPara("@CompanyID", 50, lastCompany);
          proc.AddVarcharPara("@ComponentType ", 50, ComponentType);
          proc.AddDateTimePara("@FinYearStartdate", Convert.ToDateTime(StartDate));
          proc.AddDateTimePara("@FinYearEnddate", Convert.ToDateTime(EndDate));
          proc.AddVarcharPara("@FinYear", 50, Fiyear);

          proc.AddVarcharPara("@Branch", 300, userbranchID);
          dt = proc.GetTable();
          return dt;
      }

      public DataTable GetPurchaseReturnIssueListGridData(string userbranchlist, string lastCompany, string ComponentType, string StartDate, string EndDate)
      {

          DataTable dt = new DataTable();
          ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseReturn_Details");
          proc.AddVarcharPara("@Action", 500, "GetPurchaseReturnIssueListGridData");
          proc.AddNTextPara("@BranchList", userbranchlist);
          proc.AddVarcharPara("@CompanyID", 50, lastCompany);
          proc.AddVarcharPara("@ComponentType ", 50, ComponentType);
          proc.AddDateTimePara("@FinYearStartdate", Convert.ToDateTime(StartDate));
          proc.AddDateTimePara("@FinYearEnddate", Convert.ToDateTime(EndDate));
          dt = proc.GetTable();
          return dt;
      }


      public DataTable GetCustomerNewGSTIN(string Customer, string BranchId)
      {
          DataTable dt = new DataTable();
          ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseReturn_Details");
          proc.AddVarcharPara("@Action", 500, "GetGstnDetails");
          proc.AddVarcharPara("@InternalId", 500, Customer);
          proc.AddIntegerPara("@branchId", Convert.ToInt32(BranchId));
          dt = proc.GetTable();
          return dt;
      }
      public DataTable GetReferredByVendor(string InternalId)
      {

          DataTable dt = new DataTable();
          ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseReturn_Details");
          proc.AddVarcharPara("@Action", 500, "GetReferredByVendor");
          proc.AddVarcharPara("@InternalId", 50, InternalId);

          dt = proc.GetTable();
          return dt;
      }

      public DataTable GetProjectCode(string Proj_Code)
      {
          DataTable dt = new DataTable();
          ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseReturn_Details");
          proc.AddVarcharPara("@Action", 500, "GetProjectCode");
          proc.AddVarcharPara("@Proj_Code", 200, Proj_Code);
          dt = proc.GetTable();
          return dt;
      }
    

      public DataTable GetSchemaLengthPurchaseReturn()
      {
          DataTable dt = new DataTable();
          ProcedureExecute proc = new ProcedureExecute("[prc_ProjectPurchaseReturn_Details]");

          proc.AddVarcharPara("@Action", 100, "GetSchemaLengthPurchaseReturn");

          dt = proc.GetTable();
          return dt;
      }

      public DataTable GetSchemaLengthPurchaseReturnIssue()
      {
          DataTable dt = new DataTable();
          ProcedureExecute proc = new ProcedureExecute("[prc_ProjectPurchaseReturn_Details]");

          proc.AddVarcharPara("@Action", 100, "GetSchemaLengthPurchaseReturnIssue");

          dt = proc.GetTable();
          return dt;
      }
      public int DeletePurchaseReturn(string strPurchaseReturnID, string comapanyid, string finyear, string branch)
      {
          int i;
          int rtrnvalue = 0;

          if (!string.IsNullOrEmpty(strPurchaseReturnID) && !string.IsNullOrEmpty(comapanyid) && !string.IsNullOrEmpty(finyear))
          {
              DataTable dt = new DataTable();
              ProcedureExecute proc = new ProcedureExecute("prc_CRMProjectPurchaseReturn");
              proc.AddVarcharPara("@Action", 100, "DeletePurchaseReturnDetails");
              proc.AddVarcharPara("@DeletePurchaseReturnId ", 100, strPurchaseReturnID);
              proc.AddVarcharPara("@CompanyID", 100, comapanyid);
              proc.AddVarcharPara("@FinYear", 100, finyear);
              proc.AddVarcharPara("@BranchID", 100, branch);

              proc.AddVarcharPara("@IsEditidMode", 10, "N");
              dt = proc.GetTable();
          }

          return rtrnvalue;

      }


      public int DeletePReturn(string strPurchaseReturnID, string comapanyid, string finyear, string branch, string ComponentType)
      {
          int i = 0;
          int rtrnvalue = 0;

          if (!string.IsNullOrEmpty(strPurchaseReturnID) && !string.IsNullOrEmpty(comapanyid) && !string.IsNullOrEmpty(finyear))
          {
              DataTable dt = new DataTable();
              ProcedureExecute proc = new ProcedureExecute("prc_CRMProjectPurchaseReturn");
              proc.AddVarcharPara("@Action", 100, "DeletePurchaseReturnDetails");
              proc.AddVarcharPara("@DeletePurchaseReturnId ", 100, strPurchaseReturnID);
              proc.AddVarcharPara("@CompanyID", 100, comapanyid);
              proc.AddVarcharPara("@FinYear", 100, finyear);
              proc.AddVarcharPara("@BranchID", 100, branch);
              proc.AddVarcharPara("@ComponentType", 50, ComponentType);
              proc.AddVarcharPara("@IsEditidMode", 10, "N");
              proc.AddIntegerPara("@UserID", Convert.ToInt32(HttpContext.Current.Session["userid"]));
              proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
              i = proc.RunActionQuery();
              rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));

              //dt = proc.GetTable();
          }

          return rtrnvalue;

      }
      public DataSet GetAllDropDownDetailForSalesInvoice(string userbranch, string CompanyID, string BranchID, string Year)
      {
          DataSet ds = new DataSet();
          ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseReturn_Details");
          proc.AddVarcharPara("@Action", 100, "GetAllDropDownDetailForSalesQuotation");
          proc.AddNTextPara("@BranchList", userbranch);
          proc.AddVarcharPara("@CompanyID", 100, CompanyID);
          proc.AddVarcharPara("@BrID", 100, BranchID);
          proc.AddVarcharPara("@FinYear", 100, Year);
          ds = proc.GetDataSet();
          return ds;
      }
      public DataTable PopulateCustomerDetail()
      {
          DataTable dt = new DataTable();
          ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseReturn_Details");
          proc.AddVarcharPara("@Action", 100, "PopulateCustomerDetail");
          dt = proc.GetTable();
          return dt;
      }
      public DataTable GetCustomerDetails_InvoiceRelated(string strCustomerID)
      {
          DataTable dt = new DataTable();
          ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseReturn_Details");
          proc.AddVarcharPara("@Action", 100, "GetCustomerInvoiceDetails");
          proc.AddVarcharPara("@CustomerID", 1000, strCustomerID);
          dt = proc.GetTable();
          return dt;
      }

      public bool CheckUniqueRefCreditNoteNo(string vendorid, string RefCreditNoteno, string action, string mode, string strPurchaseReturnID)
      {
          ProcedureExecute proc;

          try
          {
              using (proc = new ProcedureExecute("prc_ProjectPurchaseReturn_Details"))
              {
                  proc.AddVarcharPara("@action", 50, action);
                  proc.AddBooleanPara("@ReturnValue", false, QueryParameterDirection.Output);
                  proc.AddVarcharPara("@CustomerID", 100, vendorid);
                  proc.AddVarcharPara("@RefCreditNoteno", 100, RefCreditNoteno);
                  proc.AddVarcharPara("@mode", 5, mode);
                  proc.AddVarcharPara("@PurchaseReturnID", 100, strPurchaseReturnID);

                  int i = proc.RunActionQuery();
                  var retData = Convert.ToBoolean(proc.GetParaValue("@ReturnValue"));
                  return retData;
              }
          }

          catch (Exception ex)
          {
              return false;
          }

          finally
          {
              proc = null;
          }

      }
      public DataTable GetPurchaseReturnEditData(string PurchaseReturnID)
      {
          DataTable dt = new DataTable();
          ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseReturn_Details");
          proc.AddVarcharPara("@Action", 500, "PurchaseReturnEditDetails");
          proc.AddVarcharPara("@PurchaseReturnID ", 500, PurchaseReturnID);
          dt = proc.GetTable();
          return dt;
      }
      public DataTable PurchaseReturnTotalAmountDetails(string PurchaseReturnID)
      {
          DataTable dt = new DataTable();
          ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseReturn_Details");
          proc.AddVarcharPara("@Action", 500, "EditedProductAmountDetails");
          proc.AddVarcharPara("@PurchaseReturnID ", 500, PurchaseReturnID);
          dt = proc.GetTable();
          return dt;
      }
      public DataSet GetPurchaseReturnTaggingProductData(string strPurchaseReturnID, string LastFinYear, string LastCompany)
      {
          DataSet ds = new DataSet();
          ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseReturn_Details");
          proc.AddVarcharPara("@Action", 50, "PurchaseReturnTaggingProductDetails");
          proc.AddVarcharPara("@PurchaseReturnID", 500, strPurchaseReturnID);
          proc.AddVarcharPara("@FinYear", 100, LastFinYear);
          proc.AddVarcharPara("@campany_Id", 100, LastCompany);
          ds = proc.GetDataSet();
          return ds;
      }

      public DataSet GetPurchaseReturnIssueTaggingProductData(string strPurchaseReturnID, string LastFinYear, string LastCompany)
      {
          DataSet ds = new DataSet();
          ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseReturn_Details");
          proc.AddVarcharPara("@Action", 50, "PurchaseReturnIssueTaggingProductDetails");
          proc.AddVarcharPara("@PurchaseReturnID", 500, strPurchaseReturnID);
          proc.AddVarcharPara("@FinYear", 100, LastFinYear);
          proc.AddVarcharPara("@campany_Id", 100, LastCompany);
          ds = proc.GetDataSet();
          return ds;
      }

      public DataSet GetPurchaseReturnTransitIssueTaggingProductData(string strPurchaseReturnID, string LastFinYear, string LastCompany)
      {
          DataSet ds = new DataSet();
          ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseReturn_Details");
          proc.AddVarcharPara("@Action", 50, "PurchaseReturnTransitIssueTaggingProductDetails");
          proc.AddVarcharPara("@PurchaseReturnID", 500, strPurchaseReturnID);
          proc.AddVarcharPara("@FinYear", 100, LastFinYear);
          proc.AddVarcharPara("@campany_Id", 100, LastCompany);
          ds = proc.GetDataSet();
          return ds;
      }


      public DataSet GetPurchaseReturnProductData(string strPurchaseReturnID, string LastFinYear, string LastCompany)
      {
          DataSet ds = new DataSet();
          ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseReturn_Details");
          proc.AddVarcharPara("@Action", 50, "PurchaseReturnProductDetails");
          proc.AddVarcharPara("@PurchaseReturnID", 500, strPurchaseReturnID);
          proc.AddVarcharPara("@FinYear", 100, LastFinYear);
          proc.AddVarcharPara("@campany_Id", 100, LastCompany);
          ds = proc.GetDataSet();
          return ds;
      }

      public DataTable GetPurchaseReturnBillingAddress(string strPurchaseReturnID)
      {
          DataTable dt = new DataTable();
          ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseReturn_Details");
          proc.AddVarcharPara("@Action", 50, "PurchaseReturnBillingAddress");
          proc.AddVarcharPara("@PurchaseReturnID", 500, strPurchaseReturnID);
          dt = proc.GetTable();
          return dt;
      }


      public int PurchaseSalesReturn(string strPurchaseReturnID, string comapanyid, string finyear)
      {
          int i;
          int rtrnvalue = 0;

          if (!string.IsNullOrEmpty(strPurchaseReturnID) && !string.IsNullOrEmpty(comapanyid) && !string.IsNullOrEmpty(finyear))
          {
              DataTable dt = new DataTable();
              ProcedureExecute proc = new ProcedureExecute("prc_CRMProjectPurchaseReturn");
              proc.AddVarcharPara("@Action", 100, "DeletePurchaseReturnDetails");
              proc.AddVarcharPara("@DeletePurchaseReturnId", 100, strPurchaseReturnID);
              proc.AddVarcharPara("@CompanyID", 100, comapanyid);
              proc.AddVarcharPara("@FinYear", 100, finyear);
              proc.AddVarcharPara("@IsEditidMode", 10, "N");
              dt = proc.GetTable();
          }

          return rtrnvalue;

      }

      public DataTable GetComponent(string Customer, string Date, string strPurchaseReturnID, string BranchId, string ComponentType)
      {
          DataTable dt = new DataTable();
          ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseReturn_Details");
          proc.AddVarcharPara("@Action", 500, "GetPurchaseInvoice");
          proc.AddVarcharPara("@CustomerID", 500, Customer);
          proc.AddVarcharPara("@PurchaseReturnID", 20, strPurchaseReturnID);
          proc.AddIntegerPara("@branchId", Convert.ToInt32(BranchId));
          proc.AddVarcharPara("@ComponentType", 10, ComponentType);
          // proc("@Date",  Convert.ToDateTime(Date).ToString("YYYY-mm-dd"));
          proc.AddVarcharPara("@Date", 30, Date);
          dt = proc.GetTable();
          return dt;
      }
      public DataTable GetTransitComponent(string Customer, string Date, string strPurchaseReturnID, string BranchId, string ComponentType)
      {
          DataTable dt = new DataTable();
          ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseReturn_Details");
          proc.AddVarcharPara("@Action", 500, "GetTransitPurchaseInvoice");
          proc.AddVarcharPara("@CustomerID", 500, Customer);
          proc.AddVarcharPara("@PurchaseReturnID", 20, strPurchaseReturnID);
          proc.AddIntegerPara("@branchId", Convert.ToInt32(BranchId));
          proc.AddVarcharPara("@ComponentType", 10, ComponentType);
          // proc("@Date",  Convert.ToDateTime(Date).ToString("YYYY-mm-dd"));
          proc.AddVarcharPara("@Date", 30, Date);
          dt = proc.GetTable();
          return dt;
      }

      public DataTable GetPurchaseRDEVInvoice(string Customer, string Date, string strPurchaseReturnID, string BranchId, string ComponentType)
      {
          DataTable dt = new DataTable();
          ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseReturn_Details");
          proc.AddVarcharPara("@Action", 500, "GetPurchaseRDEVInvoice");
          proc.AddVarcharPara("@CustomerID", 500, Customer);
          proc.AddVarcharPara("@PurchaseReturnID", 20, strPurchaseReturnID);
          proc.AddIntegerPara("@branchId", Convert.ToInt32(BranchId));
          proc.AddVarcharPara("@ComponentType", 10, ComponentType);
          // proc("@Date",  Convert.ToDateTime(Date).ToString("YYYY-mm-dd"));
          proc.AddVarcharPara("@Date", 30, Date);
          dt = proc.GetTable();
          return dt;
      }


      public DataTable GetTransitPurchaseRDEVInvoice(string Customer, string Date, string strPurchaseReturnID, string BranchId, string ComponentType)
      {
          DataTable dt = new DataTable();
          ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseReturn_Details");
          proc.AddVarcharPara("@Action", 500, "GetTransitPurchaseRDEVInvoice");
          proc.AddVarcharPara("@CustomerID", 500, Customer);
          proc.AddVarcharPara("@PurchaseReturnID", 20, strPurchaseReturnID);
          proc.AddIntegerPara("@branchId", Convert.ToInt32(BranchId));
          proc.AddVarcharPara("@ComponentType", 10, ComponentType);
          // proc("@Date",  Convert.ToDateTime(Date).ToString("YYYY-mm-dd"));
          proc.AddVarcharPara("@Date", 30, Date);
          dt = proc.GetTable();
          return dt;
      }

      public DataTable GetComponentByRequest(string Customer, string Date, string strPurchaseReturnID, string BranchId, string ComponentType, string RequestID)
      {
          DataTable dt = new DataTable();
          ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseReturn_Details");
          proc.AddVarcharPara("@Action", 500, "GetPurchaseInvoiceByRequest");
          proc.AddVarcharPara("@CustomerID", 500, Customer);
          proc.AddVarcharPara("@PurchaseReturnID", 20, strPurchaseReturnID);
          proc.AddIntegerPara("@branchId", Convert.ToInt32(BranchId));
          proc.AddVarcharPara("@ComponentType", 10, ComponentType);
          proc.AddVarcharPara("@Date", 30, Date);
          proc.AddVarcharPara("@RequestID", 30, RequestID);
          dt = proc.GetTable();
          return dt;
      }

      public DataTable GetBranchIdByRequest(string RequestID)
      {
          DataTable dt = new DataTable();
          ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseReturn_Details");
          proc.AddVarcharPara("@Action", 500, "GetBranchIdByRequest");

          proc.AddVarcharPara("@RequestID", 30, RequestID);
          dt = proc.GetTable();
          return dt;
      }


      public DataTable GetTaxDetailsPI(string InvoiceID)
      {
          DataTable dt = new DataTable();
          ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseReturn_Details");
          proc.AddVarcharPara("@Action", 500, "TaxDetailsPI");
          proc.AddVarcharPara("@Purchaseinvoice_ID", 500, InvoiceID);
          dt = proc.GetTable();
          return dt;
      }
      public DataTable GetTaxDetailsPC(string ChallanID)
      {
          DataTable dt = new DataTable();
          ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseReturn_Details");
          proc.AddVarcharPara("@Action", 500, "TaxDetailsPC");
          proc.AddVarcharPara("@PurchaseChallan_ID", 500, ChallanID);
          dt = proc.GetTable();
          return dt;
      }
      public DataTable GetPurchaseChallanComponent(string Customer, string Date, string strPurchaseReturnID, string BranchId)
      {
          DataTable dt = new DataTable();
          ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseReturn_Details");
          proc.AddVarcharPara("@Action", 500, "GetPurchaseChallan");
          proc.AddVarcharPara("@CustomerID", 500, Customer);
          proc.AddVarcharPara("@PurchaseReturnID", 20, strPurchaseReturnID);

          proc.AddIntegerPara("@branchId", Convert.ToInt32(BranchId));
          // proc("@Date",  Convert.ToDateTime(Date).ToString("YYYY-mm-dd"));
          proc.AddVarcharPara("@Date", 30, Date);
          dt = proc.GetTable();
          return dt;
      }

      public DataTable GetCustomerOutStanding(string Customer)
      {
          DataTable dt = new DataTable();
          ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseReturn_Details");
          proc.AddVarcharPara("@Action", 500, "GetCustomerOutStandingString");
          proc.AddVarcharPara("@CustomerID", 500, Customer);

          dt = proc.GetTable();
          return dt;
      }

      #region Get PurchaseInvoice  Date
      public DataTable GetPurchaseInvoiceDate(string PurchaseInvoiceNo)
      {
          ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseReturn_Details");
          proc.AddVarcharPara("@PurchaseInvoice_Number", 100, PurchaseInvoiceNo);
          proc.AddVarcharPara("@Action", 100, "GetPurchaseInvoiceDate");

          return proc.GetTable();
      }
      #endregion  Get PurchaseInvoice  Date

      #region Get TransitPurchaseInvoice  Date
      public DataTable GetTransitPurchaseInvoiceDate(string TransitPurchaseInvoiceNo)
      {
          ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseReturn_Details");
          proc.AddVarcharPara("@PurchaseInvoice_Number", 100, TransitPurchaseInvoiceNo);
          proc.AddVarcharPara("@Action", 100, "GetTransitPurchaseInvoiceDate");

          return proc.GetTable();
      }
      #endregion  Get TransitPurchaseInvoice  Date

      #region Get Purchasechallan  Date
      public DataTable GetPurchaseChallanDate(string PurchaseInvoiceNo)
      {
          ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseReturn_Details");
          proc.AddVarcharPara("@PurchaseInvoice_Number", 100, PurchaseInvoiceNo);
          proc.AddVarcharPara("@Action", 100, "GetPurchaseChallanDate");

          return proc.GetTable();
      }
      #endregion  Get Purchasechallan  Date

      public DataTable GetPurchaseInvoiceDetailsOnly(string Indent_Id, string Type)
      {
          ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseReturn_Details");
          proc.AddVarcharPara("@Action", 100, "GetPurchaseInvoiceDetailsOnly");
          proc.AddVarcharPara("@Purchaseinvoice_ID", 4000, Indent_Id);
          proc.AddVarcharPara("@Type", 10, Type);
          return proc.GetTable();
      }
      public DataTable GetTransitPurchaseInvoiceProducts(string Indent_Id, string Type)
      {
          ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseReturn_Details");
          proc.AddVarcharPara("@Action", 100, "GetTransitPurchaseInvoiceProducts");
          proc.AddVarcharPara("@Purchaseinvoice_ID", 4000, Indent_Id);
          proc.AddVarcharPara("@Type", 10, Type);
          return proc.GetTable();
      }
      public DataTable GetTransitPurchaseInvoiceDetailsOnly(string Indent_Id, string Type)
      {
          ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseReturn_Details");
          proc.AddVarcharPara("@Action", 100, "GetTransitPurchaseInvoiceDetailsOnly");
          proc.AddVarcharPara("@Purchaseinvoice_ID", 4000, Indent_Id);
          proc.AddVarcharPara("@Type", 10, Type);
          return proc.GetTable();
      }


      public DataTable PopulateContactPersonPhone(string InternalId)
      {
          DataTable ds = new DataTable();
          ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseReturn_Details");
          proc.AddVarcharPara("@Action", 100, "PopulateContactPersonPhone");
          proc.AddVarcharPara("@InternalId", 100, InternalId);
          ds = proc.GetTable();
          return ds;
      }
      public DataTable GetPurchaseChallanDetailsOnly(string Indent_Id, string Type)
      {
          ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseReturn_Details");
          proc.AddVarcharPara("@Action", 100, "GetPurchaseChallanDetailsOnly");
          proc.AddVarcharPara("@PurchaseChallan_ID", 4000, Indent_Id);
          proc.AddVarcharPara("@Type", 10, Type);
          return proc.GetTable();
      }
      public DataTable GetPurchaseChallanDetailsByRequest(string Indent_Id, string Type, string RequestNo)
      {
          ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseReturn_Details");
          proc.AddVarcharPara("@Action", 100, "GetPurchaseChallanDetailsByRequest");
          proc.AddVarcharPara("@PurchaseChallan_ID", 4000, Indent_Id);
          proc.AddVarcharPara("@Type", 10, Type);
          proc.AddVarcharPara("@RequestID", 100, RequestNo);
          return proc.GetTable();
      }
      public DataSet GetIndentDetailsForPOGridBind(string Indent_Id, string Order_Key, string Product_Ids, string comapanyid, string finyear)
      {
          DataSet dtp = new DataSet();
          ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseReturn_Details");
          proc.AddVarcharPara("@Action", 100, "GetPurchaseInvoiceforPR");
          proc.AddVarcharPara("@Purchaseinvoice_ID", 4000, Indent_Id);
          proc.AddVarcharPara("@OrderID_Key", 1000, Order_Key);
          proc.AddVarcharPara("@Product_Id", 1000, Product_Ids);
          proc.AddVarcharPara("@campany_Id", 100, comapanyid);
          proc.AddVarcharPara("@FinYear", 100, finyear);
          dtp = proc.GetDataSet();
          return dtp;
      }
      public DataTable GetTPIDetailsForGridBind(string Indent_Id, string Order_Key, string Product_Ids, string comapanyid, string finyear)
      {
          ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseReturn_Details");
          proc.AddVarcharPara("@Action", 100, "GetPurchaseInvoiceforTPR");
          proc.AddVarcharPara("@Purchaseinvoice_ID", 4000, Indent_Id);
          proc.AddVarcharPara("@OrderID_Key", 1000, Order_Key);
          proc.AddVarcharPara("@Product_Id", 1000, Product_Ids);
          proc.AddVarcharPara("@campany_Id", 100, comapanyid);
          proc.AddVarcharPara("@FinYear", 100, finyear);

          return proc.GetTable();
      }



      public DataTable GetIndentDetailsForTPRGridBind(string Indent_Id, string Order_Key, string Product_Ids, string comapanyid, string finyear)
      {
          ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseReturn_Details");
          proc.AddVarcharPara("@Action", 100, "GetPurchaseInvoiceforTPR");
          proc.AddVarcharPara("@Purchaseinvoice_ID", 4000, Indent_Id);
          proc.AddVarcharPara("@OrderID_Key", 1000, Order_Key);
          proc.AddVarcharPara("@Product_Id", 1000, Product_Ids);
          proc.AddVarcharPara("@campany_Id", 100, comapanyid);
          proc.AddVarcharPara("@FinYear", 100, finyear);

          return proc.GetTable();
      }



      public DataSet GetIndentDetailsForDSPOGridBind(string Indent_Id, string Order_Key, string Product_Ids, string comapanyid, string finyear)
      {
          DataSet ds = new DataSet();
          ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseReturn_Details");
          proc.AddVarcharPara("@Action", 100, "GetPurchaseInvoiceforPR");
          proc.AddVarcharPara("@Purchaseinvoice_ID", 4000, Indent_Id);
          proc.AddVarcharPara("@OrderID_Key", 1000, Order_Key);
          proc.AddVarcharPara("@Product_Id", 1000, Product_Ids);
          proc.AddVarcharPara("@campany_Id", 100, comapanyid);
          proc.AddVarcharPara("@FinYear", 100, finyear);

          ds = proc.GetDataSet();
          return ds;
      }



      public DataSet GetIndentDetailsForDSTPRGridBind(string Indent_Id, string Order_Key, string Product_Ids, string comapanyid, string finyear)
      {
          DataSet ds = new DataSet();
          ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseReturn_Details");
          proc.AddVarcharPara("@Action", 100, "GetPurchaseInvoiceforTPR");
          proc.AddVarcharPara("@Purchaseinvoice_ID", 4000, Indent_Id);
          proc.AddVarcharPara("@OrderID_Key", 1000, Order_Key);
          proc.AddVarcharPara("@Product_Id", 1000, Product_Ids);
          proc.AddVarcharPara("@campany_Id", 100, comapanyid);
          proc.AddVarcharPara("@FinYear", 100, finyear);

          ds = proc.GetDataSet();
          return ds;
      }
      public DataTable GetIndentDetailsForPSGridBind(string Indent_Id, string Order_Key, string Product_Ids, string comapanyid, string finyear)
      {
          ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseReturn_Details");
          proc.AddVarcharPara("@Action", 100, "GetPurchaseChallanforPC");
          proc.AddVarcharPara("@PurchaseChallan_ID", 4000, Indent_Id);
          proc.AddVarcharPara("@OrderID_Key", 1000, Order_Key);
          proc.AddVarcharPara("@Product_Id", 1000, Product_Ids);
          proc.AddVarcharPara("@campany_Id", 100, comapanyid);
          proc.AddVarcharPara("@FinYear", 100, finyear);

          return proc.GetTable();
      }
      public DataTable GetIndentDetailsForPSGridBindByRequest(string Indent_Id, string Order_Key, string Product_Ids, string comapanyid, string finyear, string RequestNo)
      {
          ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseReturn_Details");
          proc.AddVarcharPara("@Action", 100, "GetPurchaseChallanforPCByRequest");
          proc.AddVarcharPara("@PurchaseChallan_ID", 4000, Indent_Id);
          proc.AddVarcharPara("@OrderID_Key", 1000, Order_Key);
          proc.AddVarcharPara("@Product_Id", 1000, Product_Ids);
          proc.AddVarcharPara("@campany_Id", 100, comapanyid);
          proc.AddVarcharPara("@FinYear", 100, finyear);
          proc.AddVarcharPara("@RequestID", 100, RequestNo);

          return proc.GetTable();
      }
      public DataTable GetVendorGSTIN(string Vendor)
      {
          DataTable dt = new DataTable();
          ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseReturn_Details");
          proc.AddVarcharPara("@Action", 500, "GetGSTIN");
          proc.AddVarcharPara("@InternalId", 500, Vendor);

          dt = proc.GetTable();
          return dt;
      }
      public DataTable GetNecessaryData(string strComponentIDs, string strType)
      {
          DataTable dt = new DataTable();
          ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseReturn_Details");
          proc.AddVarcharPara("@Action", 100, "GetComponentSelectData");
          proc.AddVarcharPara("@SelectedComponentList", 1000, strComponentIDs);
          proc.AddVarcharPara("@ComponentType", 1000, strType);
          dt = proc.GetTable();
          return dt;
      }


      #region Issue To Customer Return

      public DataTable GetCustomerreturnComponent(string Customer, string Date, string strPurchaseReturnID, string BranchId)
      {
          DataTable dt = new DataTable();
          ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseReturn_Details");
          proc.AddVarcharPara("@Action", 500, "GeCustomerIssuereturn");
          proc.AddVarcharPara("@CustomerID", 500, Customer);
          proc.AddVarcharPara("@PurchaseReturnID", 20, strPurchaseReturnID);

          proc.AddIntegerPara("@branchId", Convert.ToInt32(BranchId));
          // proc("@Date",  Convert.ToDateTime(Date).ToString("YYYY-mm-dd"));
          proc.AddVarcharPara("@Date", 30, Date);
          dt = proc.GetTable();
          return dt;
      }
      #endregion



      public DataTable IsExistPurchaseChallanInPurchaseInvoice(string strPurchaseReturnID)
      {

          DataTable dt = new DataTable();
          ProcedureExecute proc = new ProcedureExecute("prc_ProjectPurchaseReturn_Details");
          proc.AddVarcharPara("@Action", 50, "IsExistPurchaseChallanInPurchaseInvoice");
          proc.AddVarcharPara("@PurchaseReturnID", 100, strPurchaseReturnID);
          dt = proc.GetTable();
          return dt;
      }


  }
}

