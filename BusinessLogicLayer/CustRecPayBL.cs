using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BusinessLogicLayer
{
    public class CustRecPayBL
    {
        public DataSet GetAllDropDownDataByVoucherType(string VoucherType)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prdn_CustomerReceiptDetails");
            proc.AddVarcharPara("@Action", 100, "AllDropDownDataCRP");
            proc.AddVarcharPara("@userbranchlist", 4000, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            proc.AddVarcharPara("@FinYear", 50, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@VoucherType", 10, VoucherType);
            ds = proc.GetDataSet();
            return ds;
        }
        public DataTable GetCustomerCashBankCRP(string userbranch, string CompanyId, string RecPayId)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prdn_CustomerReceiptDetails");
            proc.AddVarcharPara("@Action", 100, "GetCashBankDataCRP");
            proc.AddVarcharPara("@BranchId", 100, userbranch);
            proc.AddVarcharPara("@CompanyId", 100, CompanyId);
            proc.AddVarcharPara("@Receipt_ID", 100, RecPayId);
            proc.AddBigIntegerPara("@UserId", Convert.ToInt64(HttpContext.Current.Session["userid"]));

            ds = proc.GetTable();
            return ds;
        }

        public DataTable PopulateContactPerson(string InternalId)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prdn_CustomerReceiptDetails");
            proc.AddVarcharPara("@Action", 100, "GetContactPerson");
            proc.AddVarcharPara("@InternalId", 100, InternalId);
            ds = proc.GetTable();
            return ds;
        }
        public DataTable PopulateProformaInvoice(string InternalId, string AsOnDate)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prdn_CustomerReceiptDetails");
            proc.AddVarcharPara("@Action", 100, "GetProformaInvoice");
            proc.AddVarcharPara("@InternalId", 100, InternalId);
            proc.AddVarcharPara("@PostingDate", 10, AsOnDate);
            ds = proc.GetTable();
            return ds;
        }
        public DataTable GetEditProformaInvoice(string id)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prdn_CustomerReceiptDetails");
            proc.AddVarcharPara("@Action", 100, "GetEditProformaInvoice");
            proc.AddVarcharPara("@Receipt_ID", 100, id);           
            ds = proc.GetTable();
            return ds;
        }
        
        public DataTable GetBranchStateCode(string BranchId)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prdn_CustomerReceiptDetails");
            proc.AddVarcharPara("@Action", 100, "GetBranchStateCode");
            proc.AddVarcharPara("@BranchId", 100, BranchId);
            ds = proc.GetTable();
            return ds;
        }

        public DataTable GetCurrentConvertedRate(int BaseCurrencyId, int ConvertedCurrencyId, string CompID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prdn_CustomerReceiptDetails");
            proc.AddVarcharPara("@Action", 100, "GetCurrentConvertedRate");
            proc.AddIntegerPara("@BaseCurrencyId", BaseCurrencyId);
            proc.AddIntegerPara("@ConvertedCurrencyId", ConvertedCurrencyId);
            proc.AddVarcharPara("@CompanyId", 10, CompID);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetAllDocumentPayment(string VoucherType, string CustomerId, string BranchId, string ReceiptPaymentId, string TransDate)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prdn_CustomerReceiptDetails");
            proc.AddVarcharPara("@Action", 100, "GetDocDetailsPayment");
            proc.AddVarcharPara("@InternalId", 100, CustomerId);
            proc.AddVarcharPara("@BranchId",50, BranchId);
            proc.AddVarcharPara("@Receipt_ID", 10, ReceiptPaymentId);
            proc.AddDateTimePara("@Receiptdate", Convert.ToDateTime(TransDate));
            dt = proc.GetTable();
            return dt;
        }


        public DataTable GetAllDocumentPaymentWithProject(string VoucherType, string CustomerId, string BranchId, string ReceiptPaymentId, string TransDate, string ProjectId)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prdn_CustomerReceiptDetails");
            proc.AddVarcharPara("@Action", 100, "GetDocDetailsPaymentWthProject");
            proc.AddVarcharPara("@InternalId", 100, CustomerId);
            proc.AddVarcharPara("@BranchId", 50, BranchId);
            proc.AddVarcharPara("@Receipt_ID", 10, ReceiptPaymentId);
            proc.AddDateTimePara("@Receiptdate", Convert.ToDateTime(TransDate));
            proc.AddBigIntegerPara("@Project_Id", Convert.ToInt64(ProjectId));
            dt = proc.GetTable();
            return dt;
        }


        public DataTable GetAllDocument(string VoucherType, string CustomerId, string BranchId, string ReceiptPaymentId, string TransDate)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prdn_CustomerReceiptDetails");
            proc.AddVarcharPara("@Action", 100, "GetDocDetails");
            proc.AddVarcharPara("@InternalId", 100, CustomerId);
            proc.AddVarcharPara("@BranchId",50, BranchId);
            proc.AddVarcharPara("@Receipt_ID", 10, ReceiptPaymentId);
            proc.AddDateTimePara("@Receiptdate",Convert.ToDateTime(TransDate));

            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetAllDocumentWithProject(string VoucherType, string CustomerId, string BranchId, string ReceiptPaymentId, string TransDate, string ProjectId)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prdn_CustomerReceiptDetails");
            proc.AddVarcharPara("@Action", 100, "GetDocDetailsWithProject");
            proc.AddVarcharPara("@InternalId", 100, CustomerId);
            proc.AddVarcharPara("@BranchId", 50, BranchId);
            proc.AddVarcharPara("@Receipt_ID", 10, ReceiptPaymentId);
            proc.AddDateTimePara("@Receiptdate", Convert.ToDateTime(TransDate));
            proc.AddBigIntegerPara("@Project_Id", Convert.ToInt64(ProjectId));

            dt = proc.GetTable();
            return dt;
        }

        public string AddEditReceipt(ref string OutputId,string ActionType, string strEditCashBankID, string strCashBankBranchID, string strTransactionDate,
            string strCashBankID,string strExchangeSegmentID, string strTransactionType, string strEntryUserProfile, string strVoucherAmount,
            string strCustomer, string strContactName, string strNarration, Int64 ProjId, string strCurrency, string strInstrumentType, string strInstrumentNumber,
            string strInstrumentDate, string strrate, string Product_IDS,DataTable strReceiptPaymentdt, DataTable tempBillAddress, Boolean GSTApplicable,
            string strEnterBranchID, string DrawnOn, string CompanyId, string LastFinYear, string userid, string paymenttype, DataTable dtMultiType,
            string SCHEMEID, string Doc_No, string ProformaInvoiceID, string strTCScode, string strTCSappl, string strTCSpercentage, string strTCSamout
            ,string Segment1,string Segment2,string Segment3,string Segment4,string Segment5
            )
        {
            try
            {
                
                DataSet dsInst = new DataSet();
               
                
                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);  MULTI

                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));                
                SqlCommand cmd = new SqlCommand("prc_AddEditCustomerReceipt", con);
                DataTable dtReceipt = new DataTable();

                dtReceipt = GetReceiptProjectDataSource();
                foreach (DataRow dr in strReceiptPaymentdt.Rows)
                {
                    dtReceipt.Rows.Add(Convert.ToString(dr["TypeID"]), Convert.ToString(dr["DocumentNo"]), Convert.ToDecimal(dr["Receipt"]),
                       Convert.ToString(dr["Remarks"]), Convert.ToString(dr["DocId"]), Convert.ToString(dr["IsOpening"]), Convert.ToInt64(dr["ProjectId"]), Convert.ToString(dr["Project_Code"]));
                }
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", ActionType);
                cmd.Parameters.AddWithValue("@ReceiptID", strEditCashBankID);
                cmd.Parameters.AddWithValue("@CompanyID", CompanyId);
                cmd.Parameters.AddWithValue("@FinYear", LastFinYear);
                cmd.Parameters.AddWithValue("@CreateUser", userid);
                cmd.Parameters.AddWithValue("@ForBranchID", strCashBankBranchID);
                cmd.Parameters.AddWithValue("@TransactionDate", DateTime.ParseExact(strTransactionDate, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@CashBankID", strCashBankID);
                cmd.Parameters.AddWithValue("@ExchangeSegmentID", strExchangeSegmentID);
                cmd.Parameters.AddWithValue("@TransactionType", strTransactionType);
                cmd.Parameters.AddWithValue("@Narration", strNarration);
                cmd.Parameters.AddWithValue("@CurrencyID", strCurrency);
                cmd.Parameters.AddWithValue("@rate", strrate);   
                cmd.Parameters.AddWithValue("@InstrumentType", strInstrumentType);
                cmd.Parameters.AddWithValue("@InstrumentNumber", strInstrumentNumber);
                if (!string.IsNullOrEmpty(strInstrumentDate) && strInstrumentDate != "01-01-1990")
                {
                    cmd.Parameters.AddWithValue("@InstrumentDate", DateTime.ParseExact(strInstrumentDate, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"));
                }
                else
                {
                    cmd.Parameters.AddWithValue("@InstrumentDate", null);
                }
                cmd.Parameters.AddWithValue("@CustomerID", strCustomer);
                cmd.Parameters.AddWithValue("@ContactPersonID", strContactName);
                cmd.Parameters.AddWithValue("@VoucherAmount", strVoucherAmount);
                cmd.Parameters.AddWithValue("@Product_IDS", Product_IDS);
                cmd.Parameters.AddWithValue("@Details", dtReceipt);
                cmd.Parameters.AddWithValue("@PaymentType", paymenttype);
                cmd.Parameters.AddWithValue("@paymentDetails", dtMultiType);
                cmd.Parameters.AddWithValue("@BillAddress", tempBillAddress); 
                cmd.Parameters.AddWithValue("@GSTApplicable", GSTApplicable); 
                cmd.Parameters.AddWithValue("@EnterBranchID", strEnterBranchID);
                cmd.Parameters.AddWithValue("@DrawnOn", Convert.ToString(DrawnOn));
                cmd.Parameters.AddWithValue("@SCHEMEID", SCHEMEID);
                cmd.Parameters.AddWithValue("@Doc_No", Doc_No);
                cmd.Parameters.AddWithValue("@Project_Id", ProjId);
                cmd.Parameters.AddWithValue("@ProformaInvoiceID", ProformaInvoiceID);

                cmd.Parameters.AddWithValue("@SegmentID1", Segment1);
                cmd.Parameters.AddWithValue("@SegmentID2", Segment2);
                cmd.Parameters.AddWithValue("@SegmentID3", Segment3);
                cmd.Parameters.AddWithValue("@SegmentID4", Segment4);
                cmd.Parameters.AddWithValue("@SegmentID5", Segment5);

                ////////////// TCS /////////////////////////////
                cmd.Parameters.AddWithValue("@TCScode", strTCScode);
                cmd.Parameters.AddWithValue("@TCSappAmount", strTCSappl);
                cmd.Parameters.AddWithValue("@TCSpercentage", strTCSpercentage);
                cmd.Parameters.AddWithValue("@TCSamount", strTCSamout);
                /////////////////////////////////////////////////////

                SqlParameter output = new SqlParameter("@ReturnValue", SqlDbType.Int);
                output.Direction = ParameterDirection.Output;

                SqlParameter outputText = new SqlParameter("@ReturnText", SqlDbType.VarChar,200);
                outputText.Direction = ParameterDirection.Output;


                cmd.Parameters.Add(output);
                cmd.Parameters.Add(outputText);

                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                con.Dispose();


                OutputId = Convert.ToString(cmd.Parameters["@ReturnValue"].Value.ToString());
                
                string strCPRID = Convert.ToString(cmd.Parameters["@ReturnText"].Value.ToString());

                return Convert.ToString(strCPRID);


            }
            catch (Exception ex)
            {
                return "Please try again later.";
            }
        }


        public string AddEditPayment(ref string OutputId, string ActionType, string strEditCashBankID, string strCashBankBranchID, string strTransactionDate,
            string strCashBankID, string strExchangeSegmentID, string strTransactionType, string strEntryUserProfile, string strVoucherAmount,
            string strCustomer, string strContactName, string strNarration, Int64 ProjId, string strCurrency, string strInstrumentType, string strInstrumentNumber,
            string strInstrumentDate, string strrate, string Product_IDS, DataTable strReceiptPaymentdt, DataTable tempBillAddress, Boolean GSTApplicable,
            string strEnterBranchID, string DrawnOn, string CompanyId, string LastFinYear, string userid, string paymenttype, DataTable dtMultiType,
            string SCHEMEID, string Doc_No, string ProformaInvoiceID)
        {
            try
            {
                DataSet dsInst = new DataSet();
              //  SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI

                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("prc_AddEditCustomerPayment", con);              

                DataTable dtPayment = new DataTable();
                DataTable OlddtPayment=new DataTable();
                dtPayment = GetPaymentProjectDataSource();
                foreach (DataRow dr in strReceiptPaymentdt.Rows)
                {
                    dtPayment.Rows.Add(Convert.ToString(dr["TypeID"]), Convert.ToString(dr["DocumentNo"]), Convert.ToDecimal(dr["Payment"]),
                       Convert.ToString(dr["Remarks"]), Convert.ToString(dr["DocId"]), Convert.ToString(dr["IsOpening"]), Convert.ToInt64(dr["ProjectId"]), Convert.ToString(dr["Project_Code"]));
                }

                OlddtPayment = GetReceiptDataSource();
                foreach (DataRow dr in strReceiptPaymentdt.Rows)
                {
                    OlddtPayment.Rows.Add(Convert.ToString(dr["TypeID"]), Convert.ToString(dr["DocumentNo"]), Convert.ToDecimal(dr["Payment"]),
                       Convert.ToString(dr["Remarks"]), Convert.ToString(dr["DocId"]), Convert.ToString(dr["IsOpening"]));
                }
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", ActionType);
                cmd.Parameters.AddWithValue("@ReceiptID", strEditCashBankID);
                cmd.Parameters.AddWithValue("@CompanyID", CompanyId);
                cmd.Parameters.AddWithValue("@FinYear", LastFinYear);
                cmd.Parameters.AddWithValue("@CreateUser", userid);
                cmd.Parameters.AddWithValue("@ForBranchID", strCashBankBranchID);
                cmd.Parameters.AddWithValue("@TransactionDate", DateTime.ParseExact(strTransactionDate, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@CashBankID", strCashBankID);
                cmd.Parameters.AddWithValue("@ExchangeSegmentID", strExchangeSegmentID);
                cmd.Parameters.AddWithValue("@TransactionType", strTransactionType);
                cmd.Parameters.AddWithValue("@Narration", strNarration);
                cmd.Parameters.AddWithValue("@CurrencyID", strCurrency);
                cmd.Parameters.AddWithValue("@rate", strrate);
                cmd.Parameters.AddWithValue("@InstrumentType", strInstrumentType);
                cmd.Parameters.AddWithValue("@InstrumentNumber", strInstrumentNumber);

                if (!string.IsNullOrEmpty(strInstrumentDate) && strInstrumentDate != "01-01-1990")
                {
                    cmd.Parameters.AddWithValue("@InstrumentDate", DateTime.ParseExact(strInstrumentDate, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"));
                }
                else
                {
                    cmd.Parameters.AddWithValue("@InstrumentDate", null);
                }
                cmd.Parameters.AddWithValue("@CustomerID", strCustomer);
                cmd.Parameters.AddWithValue("@ContactPersonID", strContactName);
                cmd.Parameters.AddWithValue("@VoucherAmount", strVoucherAmount);
                cmd.Parameters.AddWithValue("@Product_IDS", Product_IDS);
                cmd.Parameters.AddWithValue("@Details", dtPayment);
                cmd.Parameters.AddWithValue("@OldDetails", OlddtPayment);
                cmd.Parameters.AddWithValue("@PaymentType", paymenttype);
                cmd.Parameters.AddWithValue("@paymentDetails", dtMultiType);
                cmd.Parameters.AddWithValue("@BillAddress", tempBillAddress);
                cmd.Parameters.AddWithValue("@GSTApplicable", GSTApplicable);
                cmd.Parameters.AddWithValue("@EnterBranchID", strEnterBranchID);
                cmd.Parameters.AddWithValue("@DrawnOn", Convert.ToString(DrawnOn));
                cmd.Parameters.AddWithValue("@SCHEMEID", SCHEMEID);
                cmd.Parameters.AddWithValue("@Doc_No", Doc_No);
                cmd.Parameters.AddWithValue("@Project_Id", ProjId);
                cmd.Parameters.AddWithValue("@ProformaInvoiceID", ProformaInvoiceID);

                SqlParameter output = new SqlParameter("@ReturnValue", SqlDbType.Int);
                output.Direction = ParameterDirection.Output;

                SqlParameter outputText = new SqlParameter("@ReturnText", SqlDbType.VarChar, 200);
                outputText.Direction = ParameterDirection.Output;


                cmd.Parameters.Add(output);
                cmd.Parameters.Add(outputText);

                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                con.Dispose();

                OutputId = Convert.ToString(cmd.Parameters["@ReturnValue"].Value.ToString());
                string strCPRID = Convert.ToString(cmd.Parameters["@ReturnText"].Value.ToString());
                return Convert.ToString(strCPRID);

            }
            catch (Exception ex)
            {
                return "Please try again later.";
            }
        }


        public DataTable GetReceiptDataSource()
        {
            DataTable RcpDt = new DataTable();
            RcpDt.Columns.Add("TypeID", typeof(System.String));
            RcpDt.Columns.Add("DocNo", typeof(System.String));
            RcpDt.Columns.Add("Receipt", typeof(System.Decimal));
            RcpDt.Columns.Add("Remarks", typeof(System.String));
            RcpDt.Columns.Add("DocId", typeof(System.String));
            RcpDt.Columns.Add("IsOpening", typeof(System.String));
            
            return RcpDt;
        }

        public DataTable GetReceiptProjectDataSource()
        {
            DataTable RcpDt = new DataTable();
            RcpDt.Columns.Add("TypeID", typeof(System.String));
            RcpDt.Columns.Add("DocNo", typeof(System.String));
            RcpDt.Columns.Add("Receipt", typeof(System.Decimal));
            RcpDt.Columns.Add("Remarks", typeof(System.String));
            RcpDt.Columns.Add("DocId", typeof(System.String));
            RcpDt.Columns.Add("IsOpening", typeof(System.String));
            RcpDt.Columns.Add("ProjectId", typeof(System.Int64));
            RcpDt.Columns.Add("Project_Code", typeof(System.String));
            return RcpDt;
        }

        public DataTable GetPaymentProjectDataSource()
        {
            DataTable RcpDt = new DataTable();
            RcpDt.Columns.Add("TypeID", typeof(System.String));
            RcpDt.Columns.Add("DocNo", typeof(System.String));
            RcpDt.Columns.Add("Receipt", typeof(System.Decimal));
            RcpDt.Columns.Add("Remarks", typeof(System.String));
            RcpDt.Columns.Add("DocId", typeof(System.String));
            RcpDt.Columns.Add("IsOpening", typeof(System.String));
            RcpDt.Columns.Add("ProjectId", typeof(System.Int64));
            RcpDt.Columns.Add("Project_Code", typeof(System.String));
            return RcpDt;
        }
        public DataTable CreatePaymentDataTable()
        {
            DataTable paymentDetails = new DataTable();
            paymentDetails.Columns.Add("Doc_type", typeof(System.String));
            paymentDetails.Columns.Add("Payment_type", typeof(System.String));
            paymentDetails.Columns.Add("PaymentType_details", typeof(System.String));
            paymentDetails.Columns.Add("cardType", typeof(System.String));
            paymentDetails.Columns.Add("AuthNo", typeof(System.String));
            paymentDetails.Columns.Add("payment_remarks", typeof(System.String));
            paymentDetails.Columns.Add("paymentAmount", typeof(System.String));
            paymentDetails.Columns.Add("payment_date", typeof(System.String));
            paymentDetails.Columns.Add("Drawee_date", typeof(System.String));
            paymentDetails.Columns.Add("Payment_mainAccount", typeof(System.String));

            return paymentDetails;
        }

        public DataSet GetEditDetails(string id, string userbranchlist, string FinYear, string userid)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prdn_CustomerReceiptDetails");
            proc.AddVarcharPara("@Action", 100, "GetEditDetails");
            proc.AddVarcharPara("@Receipt_ID", 100, id);
            proc.AddVarcharPara("@userbranchlist", -1, userbranchlist);
            proc.AddVarcharPara("@FinYear", 100, FinYear);
            proc.AddVarcharPara("@userid", 100, userid);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet GetEditDetailsPayment(string id, string userbranchlist, string FinYear, string userid)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prdn_CustomerReceiptDetails");
            proc.AddVarcharPara("@Action", 100, "GetEditDetailsPayment");
            proc.AddVarcharPara("@Receipt_ID", 100, id);
            proc.AddVarcharPara("@userbranchlist", -1, userbranchlist);
            proc.AddVarcharPara("@FinYear", 100, FinYear);
            proc.AddVarcharPara("@userid", 100, userid);
            ds = proc.GetDataSet();
            return ds;
        }


        public DataTable GetTaxTable(string hsnCodeTax, decimal Amount, string branchid, string customerstate)
        {

            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prdn_CustomerReceiptDetails");
            proc.AddVarcharPara("@Action", 100, "GetTaxTable");
            proc.AddVarcharPara("@hsnCodeTax", 100, hsnCodeTax);
            proc.AddPara("@Amount", Amount);
            proc.AddVarcharPara("@branchid", 100, branchid);
            proc.AddVarcharPara("@customerstate", 100, customerstate);

            ds = proc.GetTable();
            return ds;

        }


    }
}
