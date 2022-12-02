using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;

namespace BusinessLogicLayer
{
    public class PosSalesInvoiceBl
    {
        public DataTable GetBasketDetails(string branchList)
        {
            ProcedureExecute proc;
            DataTable basketTable = new DataTable();
            try
            {


                using (proc = new ProcedureExecute("prc_getBasketDetail"))
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


        public DataTable GetApprovalList(string branchList)
        {
            ProcedureExecute proc;
            DataTable basketTable = new DataTable();
            try
            {


                using (proc = new ProcedureExecute("prc_getPosApprovalDetails"))
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


        public DataTable SalesBasketDetails(string branchList)
        {
            ProcedureExecute proc;
            DataTable basketTable = new DataTable();
            try
            {


                using (proc = new ProcedureExecute("prc_SalesBasketDetail"))
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


        public decimal getCustomerCurrentOutStanding(decimal CurrentBillAmount, string CustomerId)
        {
            ProcedureExecute proc;
            DataTable dtDueAmount = new DataTable();
            decimal count = 0;
            try
            {


                using (proc = new ProcedureExecute("prc_posListingDetails"))
                {

                    proc.AddVarcharPara("@Action", 50, "GetTotalDue");
                    proc.AddPara("@CurrentAmount", CurrentBillAmount);
                    proc.AddPara("@CustomerId", CustomerId);

                    dtDueAmount = proc.GetTable();
                    if (dtDueAmount != null && dtDueAmount.Rows.Count > 0)
                    {
                        count = Convert.ToDecimal(dtDueAmount.Rows[0][0]);
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

        public decimal getActualBillAmount(decimal pos_unitvalue, decimal pos_finamount, decimal pos_advancereceipt, DataTable detailstdt, DataTable invoicetaxdt, DataTable paymentdt)
        {

            ProcedureExecute proc;
            DataTable dtDueAmount = new DataTable();
            DataTable dtDet = detailstdt.Copy();
            decimal count = 0;
            try
            {


                using (proc = new ProcedureExecute("prc_posListingDetails"))
                {
                    if (dtDet.Columns.Contains("DocDetailsID"))
                    {
                        dtDet.Columns.Remove("DocDetailsID");
                    }

                    proc.AddVarcharPara("@Action", 50, "GetActualBillAmount");
                    proc.AddPara("@pos_unitValue", pos_unitvalue);
                    proc.AddPara("@Pos_FinanceAmt", pos_finamount);

                    proc.AddPara("@Pos_advanceRecptValue", pos_advancereceipt);
                    proc.AddPara("@ProductDetails", dtDet);

                    proc.AddPara("@InvoiceTax", invoicetaxdt);
                    proc.AddPara("@paymentDetails", paymentdt);

                    dtDueAmount = proc.GetTable();
                    if (dtDueAmount != null && dtDueAmount.Rows.Count > 0)
                    {
                        count = Convert.ToDecimal(dtDueAmount.Rows[0][0]);
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

        public int GetWaitingCount(string branchList)
        {
            ProcedureExecute proc;
            DataTable basketWaitingTable = new DataTable();
            int count = 0;
            try
            {


                using (proc = new ProcedureExecute("prc_posListingDetails"))
                {

                    proc.AddVarcharPara("@Action", 50, "GetWaitingCount");
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


        public int GetQuotationCount(string branchList)
        {
            ProcedureExecute proc;
            DataTable basketWaitingTable = new DataTable();
            int count = 0;
            try
            {


                using (proc = new ProcedureExecute("prc_posListingDetails"))
                {

                    proc.AddVarcharPara("@Action", 50, "GetQuoteWaitingCount");
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




































        public DataSet GetBusketDetailsById(int id)
        {
            DataSet basketDetails = new DataSet();
            ProcedureExecute proc;
            try
            {


                using (proc = new ProcedureExecute("prc_PosSalesInvoice"))
                {
                    proc.AddVarcharPara("@action", 50, "GetbasketDetails");
                    proc.AddIntegerPara("@id", id);
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


        public DataSet GetQuotationBusketById(int id)
        {
            DataSet basketDetails = new DataSet();
            ProcedureExecute proc;
            try
            {


                using (proc = new ProcedureExecute("prc_SalesCRM_Details"))
                {
                    proc.AddVarcharPara("@Action", 50, "GetBasketProductDetails");
                    proc.AddIntegerPara("@SBMain_Id", id);
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


























        public DataTable GetInvoiceListGridData(string userbranchlist, string lastCompany)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_posListingDetails");
            proc.AddVarcharPara("@Action", 500, "GetQuotationListGridData");
            proc.AddVarcharPara("@BranchList", 500, userbranchlist);
            proc.AddVarcharPara("@CompanyID", 50, lastCompany);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetInvoiceListGridDataByDate(string userbranchlist, string lastCompany, string fromdate, string todate, string branch)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_PosListGridData");
            proc.AddVarcharPara("@BranchList", 500, userbranchlist);
            proc.AddVarcharPara("@CompanyID", 50, lastCompany);
            proc.AddVarcharPara("@fromdate", 50, fromdate);
            proc.AddVarcharPara("@todate", 50, todate);
            proc.AddVarcharPara("@branchId", 50, branch);
            dt = proc.GetTable();
            return dt;
        }


        public DataTable GetISTInvoiceListGridDataByDate(string userbranchlist, string lastCompany, string fromdate, string todate, string branch)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_posListingDetails");
            proc.AddVarcharPara("@Action", 500, "GetISTListGridDataByDate");
            proc.AddVarcharPara("@BranchList", 500, userbranchlist);
            proc.AddVarcharPara("@CompanyID", 50, lastCompany);
            proc.AddVarcharPara("@fromdate", 50, fromdate);
            proc.AddVarcharPara("@todate", 50, todate);
            proc.AddVarcharPara("@branchId", 50, branch);
            dt = proc.GetTable();
            return dt;
        }


        public DataTable GetDuplicateInvoiceListGridDataByDate(string userbranchlist, string lastCompany, string fromdate, string todate, string branch)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_posListingDetails");
            proc.AddVarcharPara("@Action", 500, "GetDuplicateQuotationListGridDataByDate");
            proc.AddVarcharPara("@BranchList", 500, userbranchlist);
            proc.AddVarcharPara("@CompanyID", 50, lastCompany);
            proc.AddVarcharPara("@fromdate", 50, fromdate);
            proc.AddVarcharPara("@todate", 50, todate);
            proc.AddVarcharPara("@branchId", 50, branch);
            dt = proc.GetTable();
            return dt;
        }

        public DataSet GetAllDropDownDetailForSalesInvoice(string userbranch, string CompanyID, string BranchID)
        {
            string prodLoad = "Y", CustLoad = "Y";
            //if (HttpContext.Current.Session["ProductDetailsListPOS"] != null)
            //{
            //    prodLoad = "N";
            //}
            //if (HttpContext.Current.Session["CustomerDetailsListPOS"] != null)
            //{
            //    CustLoad = "N";
            //}

            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_PosSalesInvoice");
            proc.AddVarcharPara("@Action", 100, "GetAllDropDownDetailForSalesQuotation");
            proc.AddVarcharPara("@BranchList", 4000, userbranch);
            proc.AddVarcharPara("@CompanyID", 100, CompanyID);
            proc.AddVarcharPara("@BranchID", 4000, BranchID);
            proc.AddVarcharPara("@ShouldLoadProduct", 5, prodLoad);
            proc.AddVarcharPara("@ShouldLoadCustomer", 5, CustLoad);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataTable PopulateCustomer()
        {
            DataTable customerTable = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_POSCRMSalesInvoice_Details");
            proc.AddVarcharPara("@Action", 100, "PopulateCustomerDetail");
            customerTable = proc.GetTable();
            return customerTable;
        }

        public DataSet GetExecutive(string InternalId)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_PosSalesInvoice");
            proc.AddVarcharPara("@Action", 100, "GetExecutive");
            proc.AddVarcharPara("@cnt_internalId ", 100, InternalId);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataTable IsLedgerExistsForFinancer(string InternalId)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PosSalesInvoice");
            proc.AddVarcharPara("@Action", 100, "GetFinancerLedger");
            proc.AddVarcharPara("@cnt_internalId ", 100, InternalId);
            ds = proc.GetTable();
            return ds;
        }


        public DataSet GetInvoiceEditData(string InvoiceID)
        {
            DataSet dt = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_POSCRMSalesInvoice_Details");
            proc.AddVarcharPara("@Action", 500, "InvoiceEditDetails");
            proc.AddVarcharPara("@InvoiceID", 500, InvoiceID);
            dt = proc.GetDataSet();
            return dt;
        }


        public DataTable GetOldUnitDetails(string invoiceId)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_POSCRMSalesInvoice_Details");
            proc.AddVarcharPara("@Action", 500, "GetOldUnitDetails");
            proc.AddVarcharPara("@InvoiceID", 500, invoiceId);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetCustomerReceiptDetails(string cnt_internalId, string FinYear, string CompanyID, string posInvoiceDtae, string BranchId, string HSNList)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PosSalesInvoice");
            proc.AddVarcharPara("@action", 500, "CustomerReceiptDetails");
            proc.AddVarcharPara("@CompanyID", 100, CompanyID);
            proc.AddVarcharPara("@cnt_internalId", 100, cnt_internalId);
            proc.AddVarcharPara("@FinYear", 100, FinYear);
            proc.AddVarcharPara("@posInvoiceDtae", 10, posInvoiceDtae);
            proc.AddIntegerPara("@userBranch", Convert.ToInt32(BranchId));
            proc.AddVarcharPara("@HSNlist", 2000, HSNList);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetCustomerTotalAmountOnSingleDay(string cnt_internalId, string posInvoiceDtae, int branchId)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_GetCustomerTotalAmountOnSingleDay");
            proc.AddVarcharPara("@action", 100, "GetCustomerTotalAmt");
            proc.AddVarcharPara("@doc_date", 30, posInvoiceDtae);
            proc.AddVarcharPara("@customer_id", 10, cnt_internalId);
            proc.AddIntegerPara("@branchId", branchId);
            ds = proc.GetTable();
            return ds;

        }

        public DataTable GetCustomerReceiptTotalAmount(string customerReceipt)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PosSalesInvoice");
            proc.AddVarcharPara("@action", 500, "GetTotalCustomerReceiptAmount");
            proc.AddVarcharPara("@customerReceiptList", 2000, customerReceipt);
            dt = proc.GetTable();
            return dt;

        }
        public DataTable GetCustomerReceiptDetailsByInvoiceId(string InvoiceId)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PosSalesInvoice");
            proc.AddVarcharPara("@action", 500, "GetCustRecByInvoiceId");
            proc.AddIntegerPara("@id", Convert.ToInt32(InvoiceId));
            dt = proc.GetTable();
            return dt;
        }


        public DataTable getBranchListByBranchList(string userbranchhierchy, string userBranch)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PosSalesInvoice");
            proc.AddVarcharPara("@Action", 100, "getBranchList");
            proc.AddVarcharPara("@BranchList", 1000, userbranchhierchy);
            proc.AddIntegerPara("@branch", Convert.ToInt32(userBranch));
            ds = proc.GetTable();
            return ds;
        }

        public DataTable getBranchListByBranchListForMassBranch(string userbranchhierchy, string userBranch)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PosSalesInvoice");
            proc.AddVarcharPara("@Action", 100, "getBranchListforMassBranch");
            proc.AddVarcharPara("@BranchList", 1000, userbranchhierchy);
            proc.AddIntegerPara("@branch", Convert.ToInt32(userBranch));
            ds = proc.GetTable();
            return ds;
        }

        public DataTable getBranchListByHierchy(string userbranchhierchy)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PosSalesInvoice");
            proc.AddVarcharPara("@Action", 100, "getBranchListbyHierchy");
            proc.AddVarcharPara("@BranchList", 1000, userbranchhierchy);
            ds = proc.GetTable();
            return ds;
        }

        public DataTable getBranchListByHierchyEInvoice(string userbranchhierchy)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PosSalesInvoice");
            proc.AddVarcharPara("@Action", 100, "GetBranchListByHierchyEInvoice");
            proc.AddVarcharPara("@BranchList", 1000, userbranchhierchy);
            ds = proc.GetTable();
            return ds;
        }

        public DataTable getBankForManualBRS(string userbranchhierchy)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("GetBankForBRS");
            //proc.AddVarcharPara("@Action", 100, "getBranchListbyHierchy");
            proc.AddVarcharPara("@CompanyId", 1000, userbranchhierchy);
            ds = proc.GetTable();
            return ds;
        }

        public DataTable GetBranchAssignmentDetails(int InvoiceId, string companyId, string finYear, int branchId)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_branchAssignmentFetch");
            proc.AddVarcharPara("@companyId", 100, companyId);
            proc.AddVarcharPara("@finYear", 50, finYear);
            proc.AddIntegerPara("@invoiceId", InvoiceId);
            proc.AddIntegerPara("@branchId", branchId);
            ds = proc.GetTable();
            return ds;
        }

        public DataTable GetProductActualStock(int branchId, string ProdId)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_GetProductActualStock");
            proc.AddVarcharPara("@companyId", 100, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@finYear", 50, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddIntegerPara("@branchId", branchId);
            proc.AddVarcharPara("@prodId", 10, ProdId);
            ds = proc.GetTable();
            return ds;
        }


        public DataTable getWareHouseByBranch(int userbranch)
        {

            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PosSalesInvoice");
            proc.AddVarcharPara("@Action", 100, "GetWareHouseByBranch");
            proc.AddIntegerPara("@branch", userbranch);
            ds = proc.GetTable();
            return ds;
        }

        public DataTable UpdateAssignBranch(int pos_assignBranch, int pos_wareHouse, int Invoice_Id)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PosSalesInvoice");
            proc.AddVarcharPara("@Action", 100, "AssignBranchTo");
            proc.AddIntegerPara("@pos_assignBranch", pos_assignBranch);
            proc.AddIntegerPara("@pos_wareHouse", pos_wareHouse);
            proc.AddIntegerPara("@Invoice_Id", Invoice_Id);
            proc.AddIntegerPara("@UserID", Convert.ToInt32(HttpContext.Current.Session["userid"]));
            ds = proc.GetTable();
            return ds;
        }


        public DataTable GetComponent(string Customer, string Date, string ComponentType, string FinYear, string BranchID, string Action, string strInvoiceID, string branchHierchy)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("_p_POSTagging_Details");
            proc.AddVarcharPara("@Action", 500, Action);
            proc.AddVarcharPara("@CustomerID", 500, Customer);
            proc.AddDateTimePara("@Date", Convert.ToDateTime(Date));
            proc.AddVarcharPara("@ComponentType", 10, ComponentType);
            proc.AddVarcharPara("@FinYear", 10, FinYear);
            proc.AddVarcharPara("@BranchID", 10, BranchID);
            proc.AddVarcharPara("@InvoiceID", 20, strInvoiceID);
            proc.AddVarcharPara("@branchlist", 500, branchHierchy);
            dt = proc.GetTable();
            return dt;
        }


        public void DeleteBasketDetailsFromtable(string basketId, int userId)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_PosSalesInvoice");
            proc.AddVarcharPara("@Action", 100, "DeleteBasketDetails");
            proc.AddIntegerPara("@id", Convert.ToInt32(basketId));
            proc.AddIntegerPara("@UserID", userId);
            proc.GetTable();

        }


        public int DeleteInvoice(string Invoiceid)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_DeletePOSWithAdjustment");
            proc.AddVarcharPara("@invoiceId", 10, Invoiceid);
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            proc.AddIntegerPara("@UserID", Convert.ToInt32(HttpContext.Current.Session["userid"]));
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;

        }

        public DataTable IsMinSalePriceOk(string ProductList)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_posListingDetails");
            proc.AddVarcharPara("@Action", 500, "IsMinSalePriceOk");
            proc.AddVarcharPara("@ProductSalePriceList", 5000, ProductList);
            DataTable ReturnTable = proc.GetTable();
            return ReturnTable;
        }


        public DataTable GetMassbranchPosDetails(string userbranchlist, string lastCompany)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_posListingDetails");
            proc.AddVarcharPara("@Action", 500, "GetGridMassBranchDetails");
            proc.AddVarcharPara("@BranchList", 500, userbranchlist);
            proc.AddVarcharPara("@CompanyID", 50, lastCompany);
            dt = proc.GetTable();
            return dt;


        }

        public void SetMassAssignBranch(int InvoiceId, int BranchId)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_posListingDetails");
            proc.AddVarcharPara("@Action", 500, "SetMassAssignBranch");
            proc.AddIntegerPara("@InvoiceId", InvoiceId);
            proc.AddIntegerPara("@AssignBranchId", BranchId);

            proc.RunActionQuery();
        }

        public void CancelBranchAssignment(int InvoiceId)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_posListingDetails");
            proc.AddVarcharPara("@Action", 500, "CancelBranchAssignment");
            proc.AddIntegerPara("@InvoiceId", InvoiceId);

            proc.RunActionQuery();
        }

        public DataTable GetCustomerReceipttable(string BranchList)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_posListingDetails");
            proc.AddVarcharPara("@Action", 500, "GetCustomerReceiptData");
            proc.AddVarcharPara("@BranchList", 1000, BranchList);
            DataTable ReturnTable = proc.GetTable();
            return ReturnTable;
        }

        public DataTable GetCustomerReceipttableByDateBranch(string userbranchlist, string lastCompany, string fromdate, string todate, string branch)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_posListingDetails");
            proc.AddVarcharPara("@Action", 500, "GetCustomerReceiptDataByBranch");
            proc.AddVarcharPara("@BranchList", 500, userbranchlist);
            proc.AddVarcharPara("@CompanyID", 50, lastCompany);
            proc.AddVarcharPara("@fromdate", 50, fromdate);
            proc.AddVarcharPara("@todate", 50, todate);
            proc.AddVarcharPara("@branchId", 50, branch);
            DataTable ReturnTable = proc.GetTable();
            return ReturnTable;
        }

        public DataTable GetSalesmanByBranch(string BranchID)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PosSalesInvoice");
            proc.AddVarcharPara("@Action", 100, "GetSalesmanByBranch");
            proc.AddVarcharPara("@branch", 4000, BranchID);
            ds = proc.GetTable();
            return ds;
        }

        public DataTable GetLinkedProductList(string Action, string ProductID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_POSCRMSalesInvoice_Details");
            proc.AddVarcharPara("@Action", 500, Action);
            proc.AddVarcharPara("@ProductID", 2000, ProductID);
            dt = proc.GetTable();
            return dt;
        }


        public DataSet GetBasketDetailsOnly(int id)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_PosSalesInvoice");
            proc.AddVarcharPara("@Action", 100, "GetBasketDetailsOnly");
            proc.AddIntegerPara("@id", id);
            ds = proc.GetDataSet();
            return ds;
        }

        public string getProductIsInventoryExists(string ProductId)
        {
            string IsInventory = string.Empty;
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("GetIsInventoryFlagByProductID");
            proc.AddVarcharPara("@ProductId", 500, ProductId);
            dt = proc.GetTable();
            if (dt != null && dt.Rows.Count > 0)
            {
                if (Convert.ToString(dt.Rows[0]["sProduct_IsInventory"]).ToUpper() == "TRUE")
                {
                    IsInventory = "Y";
                }
                else
                {
                    IsInventory = "N";
                }
            }
            return IsInventory;
        }

        public string GetAvailableStockCheckForOutModules(DataTable duplicatedt, string BranchId, string Date)
        {
            string StockCheck = string.Empty;
            string StockCheckMsg = string.Empty;
            //For Zero stock:Subhabrata
            if (duplicatedt != null && duplicatedt.Rows.Count > 0)
            {
                foreach (DataRow row in duplicatedt.Rows)
                {
                    Int64 ProductId = Convert.ToInt64(row["ProductID"]);
                    string IsInventory = getProductIsInventoryExists(Convert.ToString(row["ProductID"]));
                    if (IsInventory != "N")
                    {


                        // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
                        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                        //DataTable dtAvailableStockCheck = oDBEngine.GetDataTable("Select dbo.fn_CheckAvailableStockSCBOIST(" + BranchId + ",'" + Convert.ToString(HttpContext.Current.Session["LastCompany"]) + "','" + Convert.ToString(HttpContext.Current.Session["LastFinYear"]) + "'," + ProductId + "'," + Convert.ToDateTime(Date) + ") as branchopenstock");
                        DataTable dtAvailableStockCheck = oDBEngine.GetDataTable("Select dbo.fn_CheckAvailableStockForAlreadyDelivered(" + BranchId + ",'" + Convert.ToString(HttpContext.Current.Session["LastCompany"]) + "','" + Convert.ToString(HttpContext.Current.Session["LastFinYear"]) + "','" + ProductId + "','" + Convert.ToDateTime(Date).ToString("yyyy-MM-dd") + "') as branchopenstock");

                        if (dtAvailableStockCheck.Rows.Count > 0)
                        {
                            StockCheck = Convert.ToString(Math.Round(Convert.ToDecimal(dtAvailableStockCheck.Rows[0]["branchopenstock"]), 2));

                            if (Convert.ToDecimal(row["Quantity"]) > Convert.ToDecimal(StockCheck))
                            {
                                StockCheckMsg = "MoreThanStock";
                                break;
                            }
                            if (StockCheck == "0.00")
                            {
                                StockCheckMsg = "ZeroStock";
                                break;
                            }

                        }
                    }
                }
            }//End
            return StockCheckMsg;
        }
        public string GetAvailableStockCheckForPOSWarehouseWise(DataTable duplicatedt, DataTable warehousedt, string BranchId, string Date)
        {
            string StockCheck = string.Empty;
            string StockCheckMsg = string.Empty;
            //For Zero stock:Subhabrata
            if (duplicatedt != null && duplicatedt.Rows.Count > 0)
            {
                foreach (DataRow row in duplicatedt.Rows)
                {
                    string warehouseid = "";
                    string Product_Id = Convert.ToString(row["ProductID"]);
                    Int32 ProductId = Convert.ToInt32(row["ProductID"]);
                    decimal Quantity = Convert.ToDecimal(row["Quantity"]);
                    decimal TotalQuantity = 0;
                    // string validate = "";

                    // var duplicateRecords = warehousedt.AsEnumerable() 
                    //     .Select(x =>
                    //     new
                    //     {
                    //         SrlNo = x["ProductID"],
                    //         WarehouseID = x["WarehouseID"],                                                    
                    //     }
                    //  )
                    // .GroupBy(s => new { s.SrlNo, s.WarehouseID })                    

                    //.Where(gr => gr.Count() > 1)
                    //.Select(g => g.Key);

                    // foreach (var d in duplicateRecords)
                    // {
                    //     validate = "duplicateWarehouse";
                    // }

                    //  var duplicateProductRecords = duplicatedt.AsEnumerable()
                    //.GroupBy(r => r["ProductID"]) //coloumn name which has the duplicate values
                    //.Where(gr => gr.Count() > 1)
                    //.Select(g => g.Key);


                    //  foreach (var d in duplicateProductRecords)
                    //  {
                    //      validate = "duplicateProduct";
                    //  }




                    //Int32 TotalQuantity = Convert.ToInt32(duplicatedt.Compute("SUM(Quantity)", "ProductID=" + ProductId)); 

                    string IsInventory = getProductIsInventoryExists(Convert.ToString(row["ProductID"]));
                    if (IsInventory != "N")
                    {

                        DataRow[] drr = warehousedt.Select("Product_SrlNo='" + Convert.ToInt64(row["SrlNo"]) + "'");

                        foreach (DataRow item in drr)
                        {
                            warehouseid = Convert.ToString(item["WarehouseID"]);

                            //Mantis 24428
                            //TotalQuantity = warehousedt.AsEnumerable().Where(row1 => row1.Field<string>("ProductID") == Product_Id && row1.Field<string>("WarehouseID") == warehouseid)
                            // .Sum(row1 => Math.Round(Convert.ToDecimal(row1.Field<String>("TotalQuantity")), 4));

                            TotalQuantity = warehousedt.AsEnumerable().Where(row1 => row1.Field<string>("Product_SrlNo") == Convert.ToString(row["SrlNo"]) && row1.Field<string>("WarehouseID") == warehouseid)
                              .Sum(row1 => Math.Round(Convert.ToDecimal(row1.Field<String>("TotalQuantity")), 4));

                            //if (validate == "duplicateWarehouse")
                            //{
                            //    TotalQuantity = warehousedt.AsEnumerable().Where(row1 => row1.Field<string>("ProductID") == Product_Id && ("WarehouseID") == warehouseid)
                            //   .Sum(row1 => Math.Round(Convert.ToDecimal(row1.Field<String>("Quantity")), 4));
                            //}
                            ////else if (validate == "duplicateProduct")
                            ////{
                            ////    TotalQuantity = duplicatedt.AsEnumerable().Where(row1 => row1.Field<string>("ProductID") == Product_Id)
                            ////   .Sum(row1 => Math.Round(Convert.ToDecimal(row1.Field<String>("Quantity")), 4));
                            ////}
                            //else
                            //{
                            //    TotalQuantity = Convert.ToDecimal(item["TotalQuantity"]);
                            //}

                            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                            //DataTable dtAvailableStockCheck = oDBEngine.GetDataTable("Select dbo.fn_CheckAvailableStockSCBOIST(" + BranchId + ",'" + Convert.ToString(HttpContext.Current.Session["LastCompany"]) + "','" + Convert.ToString(HttpContext.Current.Session["LastFinYear"]) + "'," + ProductId + "'," + Convert.ToDateTime(Date) + ") as branchopenstock");
                            DataTable dtAvailableStockCheck = oDBEngine.GetDataTable("Select dbo.fn_GetWarehousewiseStock(" + BranchId + ",'" + Convert.ToString(HttpContext.Current.Session["LastCompany"]) + "','" + Convert.ToString(HttpContext.Current.Session["LastFinYear"]) + "','" + ProductId + "','" + warehouseid + "','" + Convert.ToDateTime(Date).ToString("yyyy-MM-dd") + "') as branchopenstock");

                            if (dtAvailableStockCheck.Rows.Count > 0)
                            {
                                StockCheck = Convert.ToString(Math.Round(Convert.ToDecimal(dtAvailableStockCheck.Rows[0]["branchopenstock"]), 4));

                                //if (Convert.ToDecimal(item["TotalQuantity"]) > Convert.ToDecimal(StockCheck))
                                if (Convert.ToDecimal(TotalQuantity) > Convert.ToDecimal(StockCheck))
                                {
                                    StockCheckMsg = "MoreThanStock";
                                    break;
                                }
                                if (StockCheck == "0.00")
                                {
                                    StockCheckMsg = "ZeroStock";
                                    break;
                                }

                            }

                        }

                        if (StockCheckMsg == "MoreThanStock" || StockCheckMsg == "ZeroStock")
                        {
                            break;
                        }

                        // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI

                    }
                }
            }//End
            return StockCheckMsg;
        }
        public string GetAvailableStockCheckForOutModulesWarehouseWise(DataTable duplicatedt, DataTable warehousedt, string BranchId, string Date)
        {
            string StockCheck = string.Empty;
            string StockCheckMsg = string.Empty;
            //For Zero stock:Subhabrata
            if (duplicatedt != null && duplicatedt.Rows.Count > 0)
            {
                foreach (DataRow row in duplicatedt.Rows)
                {
                    string warehouseid = "";
                    string Product_Id = Convert.ToString(row["ProductID"]);
                    Int32 ProductId = Convert.ToInt32(row["ProductID"]);
                    decimal Quantity = Convert.ToDecimal(row["Quantity"]);
                   // decimal TotalQuantity = 0;                
                  
                    //TotalQuantity = Convert.ToInt32(duplicatedt.Compute("SUM(Quantity)", "ProductID=" + ProductId)); 

                    string IsInventory = getProductIsInventoryExists(Convert.ToString(row["ProductID"]));
                    if (IsInventory != "N")
                    {

                        DataRow[] drr = warehousedt.Select("Product_SrlNo='" + Convert.ToInt64(row["SrlNo"]) + "'");

                        foreach (DataRow item in drr)
                        {
                            warehouseid = Convert.ToString(item["WarehouseID"]);

                           
                            
                            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                            //DataTable dtAvailableStockCheck = oDBEngine.GetDataTable("Select dbo.fn_CheckAvailableStockSCBOIST(" + BranchId + ",'" + Convert.ToString(HttpContext.Current.Session["LastCompany"]) + "','" + Convert.ToString(HttpContext.Current.Session["LastFinYear"]) + "'," + ProductId + "'," + Convert.ToDateTime(Date) + ") as branchopenstock");
                            DataTable dtAvailableStockCheck = oDBEngine.GetDataTable("Select dbo.fn_GetWarehousewiseStock(" + BranchId + ",'" + Convert.ToString(HttpContext.Current.Session["LastCompany"]) + "','" + Convert.ToString(HttpContext.Current.Session["LastFinYear"]) + "','" + ProductId + "','" + warehouseid + "','" + Convert.ToDateTime(Date).ToString("yyyy-MM-dd") + "') as branchopenstock");

                            if (dtAvailableStockCheck.Rows.Count > 0)
                            {
                                StockCheck = Convert.ToString(Math.Round(Convert.ToDecimal(dtAvailableStockCheck.Rows[0]["branchopenstock"]), 4));

                                if (Convert.ToDecimal(row["Quantity"]) > Convert.ToDecimal(StockCheck))                            
                                {
                                    StockCheckMsg = "MoreThanStock";
                                    break;
                                }
                                if (StockCheck == "0.00")
                                {
                                    StockCheckMsg = "ZeroStock";
                                    break;
                                }

                            }

                        }

                        if (StockCheckMsg == "MoreThanStock" || StockCheckMsg == "ZeroStock")
                        {
                            break;
                        }

                        // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI

                    }
                }
            }//End
            return StockCheckMsg;
        }
        public DataTable GetProductStockBranchWise(int branchId, string ProdId)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_GetProductAvailableListStatewise");
            proc.AddVarcharPara("@companyId", 100, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@finYear", 50, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddIntegerPara("@branchId", branchId);
            proc.AddVarcharPara("@productId", 10, ProdId);
            ds = proc.GetTable();
            return ds;
        }

        public DataSet GetAllDetailsByBranch(string BranchID, String strCompanyID, string strFinYear)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_PosSalesInvoice");
            proc.AddVarcharPara("@Action", 100, "GetAllDetailsByBranch");
            proc.AddVarcharPara("@branch", 4000, BranchID);
            proc.AddVarcharPara("@CompanyID", 100, strCompanyID);
            proc.AddVarcharPara("@BranchID", 4000, BranchID);
            proc.AddVarcharPara("@FinYear", 100, strFinYear);
            proc.AddVarcharPara("@Type", 100, "10");
            ds = proc.GetDataSet();
            return ds;
        }
        public DataSet GetAllDetailsByBranchForInvChallan(string BranchID, String strCompanyID, string strFinYear)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_PosSalesInvoice");
            proc.AddVarcharPara("@Action", 100, "GETALLDETAILSBYBRANCHFORINVCHALLAN");
            proc.AddVarcharPara("@branch", 4000, BranchID);
            proc.AddVarcharPara("@CompanyID", 100, strCompanyID);
            proc.AddVarcharPara("@BranchID", 4000, BranchID);
            proc.AddVarcharPara("@FinYear", 100, strFinYear);
            proc.AddVarcharPara("@Type", 100, "10");
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet GetInfluencerDetails(string InvoiceId)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_PosInfluencer");
            proc.AddVarcharPara("@Action", 100, "GetAllDetailsById");
            proc.AddVarcharPara("@InvoiceId", 50, InvoiceId);
            ds = proc.GetDataSet();
            return ds;
        }


        public DataSet GetInfluencerSchemeDetails(string InvoiceId)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_PosInfluencer");
            proc.AddVarcharPara("@Action", 100, "GetAllDetailsByIdScheme");
            proc.AddVarcharPara("@InvoiceId", 50, InvoiceId);
            ds = proc.GetDataSet();
            return ds;
        }
        public DataSet GetInfluencerReturnDetails(string influencerId)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_PosInfluencer");
            proc.AddVarcharPara("@Action", 100, "GetAllReturnDetailsById");
            proc.AddVarcharPara("@influencerId", 50, influencerId);
            ds = proc.GetDataSet();
            return ds;
        }

        public void AssignOldunitBranch(string invoiceId, string branchId)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_PosSalesInvoice");
            proc.AddVarcharPara("@Action", 100, "AssignOldunitBranch");
            proc.AddIntegerPara("@Invoice_Id", Convert.ToInt32(invoiceId));
            proc.AddIntegerPara("@branch", Convert.ToInt32(branchId));
            proc.AddIntegerPara("@UserID", Convert.ToInt32(HttpContext.Current.Session["userid"]));
            proc.RunActionQuery();
        }

        public string SaveVehicleData(DeleviryDetails Delsave, DataTable prod)
        {
            try
            {
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_INSETRT_VEHICLEMAP_DETAILS", con);
                DataTable dtReceipt = new DataTable();
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter paramProd = cmd.Parameters.Add("@VICHLELIST", SqlDbType.Structured);
                paramProd.Value = prod;
                cmd.Parameters.AddWithValue("@INVOICE_ID", Delsave.Invoice_Id);
                cmd.Parameters.AddWithValue("@Payment_Terms", Delsave.cmbPaymentTrms);
                cmd.Parameters.AddWithValue("@Other_Charges", Delsave.CmbOtehrChrgs);
                cmd.Parameters.AddWithValue("@EWAY_BILL", Delsave.ENo);
                cmd.Parameters.AddWithValue("@EWAY_DATE", Delsave.PostingDate);
                cmd.Parameters.AddWithValue("@EWAY_VALUE", Delsave.EwayValu);
                cmd.Parameters.AddWithValue("@OTHER_REMARKS", Delsave.Remarks);
                cmd.Parameters.AddWithValue("@USER", Convert.ToInt32(HttpContext.Current.Session["userid"]));
                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                con.Dispose();

                return Convert.ToString("Data save successfully");
            }
            catch (Exception ex)
            {
                return "Please try again later.";
            }
        }

        public DataSet GetInvoiceDeleviryData(string INVOICE_ID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("PRC_VIEW_VEHICLEMAP_DETAILS");
            proc.AddVarcharPara("@INVOICE_ID ", 100, INVOICE_ID);
            ds = proc.GetDataSet();
            return ds;
        }
    }
    public class DeleviryDetails
    {
        public string Invoice_Id { get; set; }
        public DateTime? PostingDate { get; set; }
        public string Remarks { get; set; }
        public string EwayValu { get; set; }
        public string ENo { get; set; }
        public string CmbOtehrChrgs { get; set; }
        public string cmbPaymentTrms { get; set; }
        public List<GridList> Grid { get; set; }
    }
    public class GridList
    {
        public string VECHICLE_ID { get; set; }
        public string VECHICLE_NO { get; set; }
    }
}
