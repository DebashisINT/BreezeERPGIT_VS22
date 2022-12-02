using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BusinessLogicLayer
{
    public class VendorPaymentAdjustmentBL
    {
        public DataSet PopulateVendorPaymentAdjustmentDetails()
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_VendorPaymentAdjustment_details");
            proc.AddVarcharPara("@Action", 50, "GetLoadDetails");
            proc.AddVarcharPara("@companyId", 20, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@FinYear", 10, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@BranchList", -1, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            return proc.GetDataSet();
        }

        public DataTable GetAdvanceListAdd(string date, string VendorId, string Branch)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_VendorPaymentAdjustment_details");
            proc.AddVarcharPara("@Action", 50, "GetAdvanceListAdd");
            proc.AddVarcharPara("@VendorId", 15, VendorId);
            proc.AddVarcharPara("@tranDate", 10, date);
            proc.AddIntegerPara("@BranchId", Convert.ToInt32(Branch));           
            return proc.GetTable();
        }

        public DataTable GetDocumentList(string Mode, string ReceiptId, string VendorId, string TransDate, string AdjId, string BranchId,
            string AdvType, string ProjectId)
        {
            if (Convert.ToInt16(ProjectId) == 0)
            {
                ProcedureExecute proc = new ProcedureExecute("Prc_VendorPaymentAdjustment_details");
                proc.AddVarcharPara("@Action", 50, "GetDocList");
                proc.AddVarcharPara("@VendorId", 15, VendorId);
                proc.AddIntegerPara("@recPayId", Convert.ToInt32(ReceiptId));
                proc.AddVarcharPara("@tranDate", 10, TransDate);
                proc.AddVarcharPara("@Mode", 10, Mode);
                proc.AddVarcharPara("@AdjId", 10, AdjId);
                proc.AddIntegerPara("@BranchId", Convert.ToInt32(BranchId));
                proc.AddVarcharPara("@AdvType", 20, AdvType);
                return proc.GetTable();
            }
            else
            {
                ProcedureExecute proc = new ProcedureExecute("Prc_VendorPaymentAdjustment_details");
                proc.AddVarcharPara("@Action", 50, "GetDocListWithProject");
                proc.AddVarcharPara("@VendorId", 15, VendorId);
                proc.AddIntegerPara("@recPayId", Convert.ToInt32(ReceiptId));
                proc.AddVarcharPara("@tranDate", 10, TransDate);
                proc.AddVarcharPara("@Mode", 10, Mode);
                proc.AddVarcharPara("@AdjId", 10, AdjId);
                proc.AddIntegerPara("@BranchId", Convert.ToInt32(BranchId));
                proc.AddVarcharPara("@AdvType", 20, AdvType);
                proc.AddIntegerPara("@ProjectId", Convert.ToInt32(ProjectId));
                return proc.GetTable();
            }
        }

        public int DeleteAdj(string AdjId)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_VendorPaymentAdjustment_details");
            proc.AddVarcharPara("@Action", 50, "Delete");
            proc.AddVarcharPara("@AdjId", 10, AdjId);
            return proc.RunActionQuery();
        }

        public DataSet GetEditedData(string AdjId)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_VendorPaymentAdjustment_details");
            proc.AddVarcharPara("@Action", 50, "GetEditedData");
            proc.AddVarcharPara("@AdjId", 10, AdjId);
            proc.AddVarcharPara("@companyId", 20, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@FinYear", 10, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@BranchList", -1, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            return proc.GetDataSet();
        }
        public DataSet GetEditedDataDebitNote(string AdjId)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_VendorCrNoteAdjustment_details");
            proc.AddVarcharPara("@Action", 50, "GetEditedData");
            proc.AddVarcharPara("@AdjId", 10, AdjId);
            proc.AddVarcharPara("@companyId", 20, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@FinYear", 10, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@BranchList", -1, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            return proc.GetDataSet();
        }
        public void AddEditAdvanceAdjustment(string Mode, string SchemeId, string Adjustment_No, string Adjustment_Date, string Branch, string Vendor_Id,
            string Adjusted_doc_id, string Adjusted_Doc_no, string Adjusted_DocAmt, string ExchangeRate, string Adjusted_DocAmt_inBaseCur,
            string Remarks, string Adjusted_DocOSAmt, string Adjusted_Amount, string userId, Int64 ProjId,ref int AdjustedId, ref string ReturnNumber,
            DataTable AdjustmentTable, ref int ErrorCode, string Adj_id, string AdvType)
        {
            DataTable dsInst = new DataTable();
            
            //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));


            SqlCommand cmd = new SqlCommand("prc_VendorPaymentAdjustment_AddEdit", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", Mode);
            cmd.Parameters.AddWithValue("@SchemeId", SchemeId);
            cmd.Parameters.AddWithValue("@Adjustment_No", Adjustment_No);
            cmd.Parameters.AddWithValue("@Adjustment_Date", Adjustment_Date);
            cmd.Parameters.AddWithValue("@Branch", Branch);
            cmd.Parameters.AddWithValue("@Vendor_id", Vendor_Id);
            cmd.Parameters.AddWithValue("@Adjusted_doc_id", Adjusted_doc_id);
            cmd.Parameters.AddWithValue("@Adjusted_Doc_no", Adjusted_Doc_no);
            cmd.Parameters.AddWithValue("@Adjusted_DocAmt", Adjusted_DocAmt);
            cmd.Parameters.AddWithValue("@ExchangeRate", ExchangeRate);
            cmd.Parameters.AddWithValue("@Adjusted_DocAmt_inBaseCur", Adjusted_DocAmt_inBaseCur);
            cmd.Parameters.AddWithValue("@Remarks", Remarks);
            cmd.Parameters.AddWithValue("@Adjusted_DocOSAmt", Adjusted_DocOSAmt);
            cmd.Parameters.AddWithValue("@Adjusted_Amount", Adjusted_Amount);
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Parameters.AddWithValue("@DetailTable", AdjustmentTable);
            cmd.Parameters.AddWithValue("@Adj_id", Adj_id);
            cmd.Parameters.AddWithValue("@AdvType", AdvType);
            cmd.Parameters.AddWithValue("@FinYear", HttpContext.Current.Session["LastFinYear"].ToString());
            cmd.Parameters.AddWithValue("@CompanyID", HttpContext.Current.Session["LastCompany"].ToString());
            cmd.Parameters.AddWithValue("@Project_Id", ProjId);

            cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 50);
            cmd.Parameters.Add("@ReturnId", SqlDbType.VarChar, 10);
            cmd.Parameters.Add("@ErrorCode", SqlDbType.Int);

            cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;
            cmd.Parameters["@ReturnId"].Direction = ParameterDirection.Output;
            cmd.Parameters["@ErrorCode"].Direction = ParameterDirection.Output;

            cmd.CommandTimeout = 0;
            SqlDataAdapter Adap = new SqlDataAdapter();
            Adap.SelectCommand = cmd;
            Adap.Fill(dsInst);

            AdjustedId = Convert.ToInt32(cmd.Parameters["@ReturnId"].Value);
            ReturnNumber = Convert.ToString(cmd.Parameters["@ReturnValue"].Value);
            ErrorCode = Convert.ToInt32(cmd.Parameters["@ErrorCode"].Value);
        }



        public void AddEditDebitNoteAdjustment(string Mode, string SchemeId, string Adjustment_No, string Adjustment_Date, string Branch, string Vendor_Id,
            string Adjusted_doc_id, string Adjusted_Doc_no, string Adjusted_DocAmt, string ExchangeRate, string Adjusted_DocAmt_inBaseCur,
            string Remarks, string Adjusted_DocOSAmt, string Adjusted_Amount, string userId, ref int AdjustedId, ref string ReturnNumber,
            DataTable AdjustmentTable, ref int ErrorCode, string Adj_id, string CrNoteType)
        {
            DataTable dsInst = new DataTable();


         //   SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI

            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));


            SqlCommand cmd = new SqlCommand("prc_VendorCrNoteAdjustment_AddEdit", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", Mode);
            cmd.Parameters.AddWithValue("@SchemeId", SchemeId);
            cmd.Parameters.AddWithValue("@Adjustment_No", Adjustment_No);
            cmd.Parameters.AddWithValue("@Adjustment_Date", Adjustment_Date);
            cmd.Parameters.AddWithValue("@Branch", Branch);
            cmd.Parameters.AddWithValue("@Vendor_Id", Vendor_Id);
            cmd.Parameters.AddWithValue("@Adjusted_doc_id", Adjusted_doc_id);
            cmd.Parameters.AddWithValue("@Adjusted_Doc_no", Adjusted_Doc_no);
            cmd.Parameters.AddWithValue("@Adjusted_DocAmt", Adjusted_DocAmt);
            cmd.Parameters.AddWithValue("@ExchangeRate", ExchangeRate);
            cmd.Parameters.AddWithValue("@Adjusted_DocAmt_inBaseCur", Adjusted_DocAmt_inBaseCur);
            cmd.Parameters.AddWithValue("@Remarks", Remarks);
            cmd.Parameters.AddWithValue("@Adjusted_DocOSAmt", Adjusted_DocOSAmt);
            cmd.Parameters.AddWithValue("@Adjusted_Amount", Adjusted_Amount);
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Parameters.AddWithValue("@DetailTable", AdjustmentTable);
            cmd.Parameters.AddWithValue("@Adj_id", Adj_id);
            cmd.Parameters.AddWithValue("@FinYear", HttpContext.Current.Session["LastFinYear"].ToString());
            cmd.Parameters.AddWithValue("@CompanyID", HttpContext.Current.Session["LastCompany"].ToString());
            cmd.Parameters.AddWithValue("@CrNoteType", CrNoteType);

            cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 50);
            cmd.Parameters.Add("@ReturnId", SqlDbType.VarChar, 10);
            cmd.Parameters.Add("@ErrorCode", SqlDbType.Int);

            cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;
            cmd.Parameters["@ReturnId"].Direction = ParameterDirection.Output;
            cmd.Parameters["@ErrorCode"].Direction = ParameterDirection.Output;

            cmd.CommandTimeout = 0;
            SqlDataAdapter Adap = new SqlDataAdapter();
            Adap.SelectCommand = cmd;
            Adap.Fill(dsInst);

            AdjustedId = Convert.ToInt32(cmd.Parameters["@ReturnId"].Value);
            ReturnNumber = Convert.ToString(cmd.Parameters["@ReturnValue"].Value);
            ErrorCode = Convert.ToInt32(cmd.Parameters["@ErrorCode"].Value);
        }


        public DataTable GetDebitNoteList(string VendorID, string TransDate, string BranchId)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_VendorDrNoteAdjustment_details");
            proc.AddVarcharPara("@Action", 50, "GetDebitNoteList");
            proc.AddVarcharPara("@VendorID", 15, VendorID);
            //  proc.AddIntegerPara("@recPayId", Convert.ToInt32(ReceiptId));
            proc.AddVarcharPara("@tranDate", 10, TransDate);
            //proc.AddVarcharPara("@Mode", 10, Mode);
            //proc.AddVarcharPara("@AdjId", 10, AdjId);
            proc.AddIntegerPara("@EnterBranchId", Convert.ToInt32(BranchId));
            return proc.GetTable();
        }

        public DataSet PopulateVendorDebitNoteAdjustmentDetails()
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_VendorDrNoteAdjustment_details");
            proc.AddVarcharPara("@Action", 50, "GetLoadDetails");
            proc.AddVarcharPara("@companyId", 20, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@FinYear", 10, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@BranchList", -1, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            return proc.GetDataSet();
        }
        public DataTable GetInvoiceDocumentList(string Mode, string ReceiptId, string VendorID, string TransDate, string AdjId, string BranchId)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_VendorDrNoteAdjustment_details");
            proc.AddVarcharPara("@Action", 50, "GetDocList");
            proc.AddVarcharPara("@VendorID", 15, VendorID);
            proc.AddIntegerPara("@recPayId", Convert.ToInt32(ReceiptId));
            proc.AddVarcharPara("@tranDate", 10, TransDate);
            proc.AddVarcharPara("@Mode", 10, Mode);
            proc.AddVarcharPara("@AdjId", 10, AdjId);
            proc.AddIntegerPara("@BranchId", Convert.ToInt32(BranchId));
            return proc.GetTable();
        }
        public int DeleteAdjForCrNote(string AdjId)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_VendorCrNoteAdjustment_details");
            proc.AddVarcharPara("@Action", 50, "Delete");
            proc.AddVarcharPara("@AdjId", 10, AdjId);
            return proc.RunActionQuery();
        }


        #region Adjustment of Advance With Credit Note
        public DataSet PopulateVendorAdvanceAdjustmentCrNoteDetails()
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_VendorCrNoteAdjustment_details");
            proc.AddVarcharPara("@Action", 50, "GetLoadDetails");
            proc.AddVarcharPara("@companyId", 20, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@FinYear", 10, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@BranchList", -1, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            return proc.GetDataSet();
        }
        public DataTable GetCrNoteCocumentList(string Mode, string ReceiptId, string VendorID, string TransDate, string AdjId, string BranchId, string ProjectId)
        {
            if (Convert.ToInt16(ProjectId) == 0)
            {
                ProcedureExecute proc = new ProcedureExecute("Prc_VendorCrNoteAdjustment_details");
                proc.AddVarcharPara("@Action", 50, "GetDocList");
                proc.AddVarcharPara("@VendorId", 15, VendorID);
                proc.AddIntegerPara("@recPayId", Convert.ToInt32(ReceiptId));
                proc.AddVarcharPara("@tranDate", 10, TransDate);
                proc.AddVarcharPara("@Mode", 10, Mode);
                proc.AddVarcharPara("@AdjId", 10, AdjId);
                proc.AddIntegerPara("@BranchId", Convert.ToInt32(BranchId));
                return proc.GetTable();
            }
            else
            {
                ProcedureExecute proc = new ProcedureExecute("Prc_VendorCrNoteAdjustment_details");
                proc.AddVarcharPara("@Action", 50, "GetDocListWithProject");
                proc.AddVarcharPara("@VendorId", 15, VendorID);
                proc.AddIntegerPara("@recPayId", Convert.ToInt32(ReceiptId));
                proc.AddVarcharPara("@tranDate", 10, TransDate);
                proc.AddVarcharPara("@Mode", 10, Mode);
                proc.AddVarcharPara("@AdjId", 10, AdjId);
                proc.AddIntegerPara("@BranchId", Convert.ToInt32(BranchId));
                proc.AddIntegerPara("@ProjectId", Convert.ToInt32(ProjectId));
                return proc.GetTable();
            }
            
        }
        public void AddEditCrNoteAdvanceAdjustment(string Mode, string SchemeId, string Adjustment_No, string Adjustment_Date, string Branch, string Vendor_Id,
           string Adjusted_doc_id, string Adjusted_Doc_no, string Adjusted_DocAmt, string ExchangeRate, string Adjusted_DocAmt_inBaseCur,
           string Remarks, string Adjusted_DocOSAmt, string Adjusted_Amount, string userId, ref int AdjustedId, ref string ReturnNumber,
           DataTable AdjustmentTable, ref int ErrorCode, string Adj_id, string CrNoteType)
        {
            try
            {
                DataTable dsInst = new DataTable();

                //Debugger.Launch();


                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

                SqlCommand cmd = new SqlCommand("prc_VendorAdvanceAdstCrNote_AddEdit", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Mode", Mode);
                cmd.Parameters.AddWithValue("@SchemeId", SchemeId);
                cmd.Parameters.AddWithValue("@Adjustment_No", Adjustment_No);
                cmd.Parameters.AddWithValue("@Adjustment_Date", Adjustment_Date);
                cmd.Parameters.AddWithValue("@Branch", Branch);
                cmd.Parameters.AddWithValue("@Vendor_id", Vendor_Id);
                cmd.Parameters.AddWithValue("@Adjusted_doc_id", Adjusted_doc_id);
                cmd.Parameters.AddWithValue("@Adjusted_Doc_no", Adjusted_Doc_no);
                cmd.Parameters.AddWithValue("@Adjusted_DocAmt", Adjusted_DocAmt);
                cmd.Parameters.AddWithValue("@ExchangeRate", ExchangeRate);
                cmd.Parameters.AddWithValue("@Adjusted_DocAmt_inBaseCur", Adjusted_DocAmt_inBaseCur);
                cmd.Parameters.AddWithValue("@Remarks", Remarks);
                cmd.Parameters.AddWithValue("@Adjusted_DocOSAmt", Adjusted_DocOSAmt);
                cmd.Parameters.AddWithValue("@Adjusted_Amount", Adjusted_Amount);
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@DetailTable", AdjustmentTable);
                cmd.Parameters.AddWithValue("@Adj_id", Adj_id);
                cmd.Parameters.AddWithValue("@FinYear", HttpContext.Current.Session["LastFinYear"].ToString());
                cmd.Parameters.AddWithValue("@CompanyID", HttpContext.Current.Session["LastCompany"].ToString());
                cmd.Parameters.AddWithValue("@CrNoteType", CrNoteType);
                cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 50);
                cmd.Parameters.Add("@ReturnId", SqlDbType.VarChar, 10);
                cmd.Parameters.Add("@ErrorCode", SqlDbType.Int);

                cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;
                cmd.Parameters["@ReturnId"].Direction = ParameterDirection.Output;
                cmd.Parameters["@ErrorCode"].Direction = ParameterDirection.Output;

                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);

                AdjustedId = Convert.ToInt32(cmd.Parameters["@ReturnId"].Value);
                ReturnNumber = Convert.ToString(cmd.Parameters["@ReturnValue"].Value);
                ErrorCode = Convert.ToInt32(cmd.Parameters["@ErrorCode"].Value);
            }
            catch (Exception ex)
            {

            }

        }

        public DataSet GetEditedCrNoteData(string AdjId)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_VendorCrNoteAdjustment_details");
            proc.AddVarcharPara("@Action", 50, "GetEditedData");
            proc.AddVarcharPara("@AdjId", 10, AdjId);
            proc.AddVarcharPara("@companyId", 20, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@FinYear", 10, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@BranchList", -1, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            return proc.GetDataSet();
        }
        public int DeleteCrNoteAdj(string AdjId)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_VendorCrNoteAdjustment_details");
            proc.AddVarcharPara("@Action", 50, "Delete");
            proc.AddVarcharPara("@AdjId", 10, AdjId);
            return proc.RunActionQuery();
        }
        public DataTable GetAdvanceListAddForCrNote(string date, string VendorID, string Branch)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_VendorCrNoteAdjustment_details");
            proc.AddVarcharPara("@Action", 50, "GetAdvanceListAdd");
            proc.AddVarcharPara("@VendorId", 15, VendorID);
            proc.AddVarcharPara("@tranDate", 10, date);
            proc.AddIntegerPara("@BranchId", Convert.ToInt32(Branch));
            return proc.GetTable();
        }

        #endregion

        #region Adjustment of Rate Difference With Invoice

        public DataSet PopulateVendorRateDifferenceAdjustDetails()
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_VendorRateDiffAdjustmentInvoice_details");
            proc.AddVarcharPara("@Action", 50, "GetLoadDetails");
            proc.AddVarcharPara("@companyId", 20, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@FinYear", 10, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@BranchList", -1, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            return proc.GetDataSet();
        }
        public DataSet GetRateDifferenceEditedData(string AdjId)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_VendorRateDiffAdjustmentInvoice_details");
            proc.AddVarcharPara("@Action", 50, "GetEditedData");
            proc.AddVarcharPara("@AdjId", 10, AdjId);
            proc.AddVarcharPara("@companyId", 20, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@FinYear", 10, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@BranchList", -1, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            return proc.GetDataSet();
        }
        public DataTable GetDocumentListAdd(string date, string VendorId, string Branch)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_VendorRateDiffAdjustmentInvoice_details");
            proc.AddVarcharPara("@Action", 50, "GetAdvanceListAdd");
            proc.AddVarcharPara("@VendorId", 15, VendorId);
            proc.AddVarcharPara("@tranDate", 10, date);
            proc.AddIntegerPara("@BranchId", Convert.ToInt32(Branch));
            return proc.GetTable();
        }
        public DataTable GetInvoiceDocumentRateDiffList(string Mode, string ReceiptId, string VendorId, string TransDate, string AdjId, string BranchId,
            string AdvType)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_VendorRateDiffAdjustmentInvoice_details");
            proc.AddVarcharPara("@Action", 50, "GetDocList");
            proc.AddVarcharPara("@VendorId", 15, VendorId);
            proc.AddIntegerPara("@recPayId", Convert.ToInt32(ReceiptId));
            proc.AddVarcharPara("@tranDate", 10, TransDate);
            proc.AddVarcharPara("@Mode", 10, Mode);
            proc.AddVarcharPara("@AdjId", 10, AdjId);
            proc.AddIntegerPara("@BranchId", Convert.ToInt32(BranchId));
            proc.AddVarcharPara("@AdvType", 20, AdvType);
            return proc.GetTable();
        }
        public void AddEditRateDifferenceAdjustment(string Mode, string SchemeId, string Adjustment_No, string Adjustment_Date, string Branch, string Vendor_Id,
           string Adjusted_doc_id, string Adjusted_Doc_no, string Adjusted_DocAmt, string ExchangeRate, string Adjusted_DocAmt_inBaseCur,
           string Remarks, string Adjusted_DocOSAmt, string Adjusted_Amount, string userId, ref int AdjustedId, ref string ReturnNumber,
           DataTable AdjustmentTable, ref int ErrorCode, string Adj_id, string AdvType)
        {
            DataTable dsInst = new DataTable();

            //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));


            SqlCommand cmd = new SqlCommand("prc_VendorRateDiffAdjustment_AddEdit", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", Mode);
            cmd.Parameters.AddWithValue("@SchemeId", SchemeId);
            cmd.Parameters.AddWithValue("@Adjustment_No", Adjustment_No);
            cmd.Parameters.AddWithValue("@Adjustment_Date", Adjustment_Date);
            cmd.Parameters.AddWithValue("@Branch", Branch);
            cmd.Parameters.AddWithValue("@Vendor_id", Vendor_Id);
            cmd.Parameters.AddWithValue("@Adjusted_doc_id", Adjusted_doc_id);
            cmd.Parameters.AddWithValue("@Adjusted_Doc_no", Adjusted_Doc_no);
            cmd.Parameters.AddWithValue("@Adjusted_DocAmt", Adjusted_DocAmt);
            cmd.Parameters.AddWithValue("@ExchangeRate", ExchangeRate);
            cmd.Parameters.AddWithValue("@Adjusted_DocAmt_inBaseCur", Adjusted_DocAmt_inBaseCur);
            cmd.Parameters.AddWithValue("@Remarks", Remarks);
            cmd.Parameters.AddWithValue("@Adjusted_DocOSAmt", Adjusted_DocOSAmt);
            cmd.Parameters.AddWithValue("@Adjusted_Amount", Adjusted_Amount);
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Parameters.AddWithValue("@DetailTable", AdjustmentTable);
            cmd.Parameters.AddWithValue("@Adj_id", Adj_id);
            cmd.Parameters.AddWithValue("@AdvType", AdvType);
            cmd.Parameters.AddWithValue("@FinYear", HttpContext.Current.Session["LastFinYear"].ToString());
            cmd.Parameters.AddWithValue("@CompanyID", HttpContext.Current.Session["LastCompany"].ToString());

            cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 50);
            cmd.Parameters.Add("@ReturnId", SqlDbType.VarChar, 10);
            cmd.Parameters.Add("@ErrorCode", SqlDbType.Int);

            cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;
            cmd.Parameters["@ReturnId"].Direction = ParameterDirection.Output;
            cmd.Parameters["@ErrorCode"].Direction = ParameterDirection.Output;

            cmd.CommandTimeout = 0;
            SqlDataAdapter Adap = new SqlDataAdapter();
            Adap.SelectCommand = cmd;
            Adap.Fill(dsInst);

            AdjustedId = Convert.ToInt32(cmd.Parameters["@ReturnId"].Value);
            ReturnNumber = Convert.ToString(cmd.Parameters["@ReturnValue"].Value);
            ErrorCode = Convert.ToInt32(cmd.Parameters["@ErrorCode"].Value);
        }

        public int DeleteRateDifferAdj(string AdjId)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_VendorRateDiffAdjustmentInvoice_details");
            proc.AddVarcharPara("@Action", 50, "Delete");
            proc.AddVarcharPara("@AdjId", 10, AdjId);
            return proc.RunActionQuery();
        }

        #endregion

        #region Adjustment of Debit Note With Purchasee Invoice
        public DataSet PopulatedebitNoteAdjustmentPInvoiceDetails()
        {

            ProcedureExecute proc = new ProcedureExecute("Prc_DebitNoteAdjustmentInvoice_details");
            proc.AddVarcharPara("@Action", 50, "GetLoadDetails");
            proc.AddVarcharPara("@companyId", 20, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@FinYear", 10, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@BranchList", -1, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            return proc.GetDataSet();
        }
        public int DeleteDebitNotePurchaseInvoiceAdj(string AdjId)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_DebitNoteAdjustmentInvoice_details");
            proc.AddVarcharPara("@Action", 50, "Delete");
            proc.AddVarcharPara("@AdjId", 10, AdjId);
            return proc.RunActionQuery();
        }

        public DataSet GetEditedDebitNoteData(string AdjId)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_DebitNoteAdjustmentInvoice_details");
            proc.AddVarcharPara("@Action", 50, "GetEditedData");
            proc.AddVarcharPara("@AdjId", 10, AdjId);
            proc.AddVarcharPara("@companyId", 20, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@FinYear", 10, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@BranchList", -1, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            return proc.GetDataSet();
        }

        public void AddEditDebitNoteAdjustmentPurchaseInvoice(string Mode, string SchemeId, string Adjustment_No, string Adjustment_Date, string Branch, string Customer_id,
         string Adjusted_doc_id, string Adjusted_Doc_no, string Adjusted_DocAmt, string ExchangeRate, string Adjusted_DocAmt_inBaseCur,
         string Remarks, string Adjusted_DocOSAmt, string Adjusted_Amount, string userId, Int64 ProjId, ref int AdjustedId, ref string ReturnNumber,
         DataTable AdjustmentTable, ref int ErrorCode, string Adj_id, string AdvType)
        {
            DataTable dsInst = new DataTable();


            //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);   MULTI

            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));


            SqlCommand cmd = new SqlCommand("prc_DebitNoteAdjustmentInvoice_AddEdit", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", Mode);
            cmd.Parameters.AddWithValue("@SchemeId", SchemeId);
            cmd.Parameters.AddWithValue("@Adjustment_No", Adjustment_No);
            cmd.Parameters.AddWithValue("@Adjustment_Date", Adjustment_Date);
            cmd.Parameters.AddWithValue("@Branch", Branch);
            cmd.Parameters.AddWithValue("@Customer_id", Customer_id);
            cmd.Parameters.AddWithValue("@Adjusted_doc_id", Adjusted_doc_id);
            cmd.Parameters.AddWithValue("@Adjusted_Doc_no", Adjusted_Doc_no);
            cmd.Parameters.AddWithValue("@Adjusted_DocAmt", Adjusted_DocAmt);
            cmd.Parameters.AddWithValue("@ExchangeRate", ExchangeRate);
            cmd.Parameters.AddWithValue("@Adjusted_DocAmt_inBaseCur", Adjusted_DocAmt_inBaseCur);
            cmd.Parameters.AddWithValue("@Remarks", Remarks);
            cmd.Parameters.AddWithValue("@Adjusted_DocOSAmt", Adjusted_DocOSAmt);
            cmd.Parameters.AddWithValue("@Adjusted_Amount", Adjusted_Amount);
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Parameters.AddWithValue("@DetailTable", AdjustmentTable);
            cmd.Parameters.AddWithValue("@Adj_id", Adj_id);
            cmd.Parameters.AddWithValue("@AdvType", AdvType);
            cmd.Parameters.AddWithValue("@FinYear", HttpContext.Current.Session["LastFinYear"].ToString());
            cmd.Parameters.AddWithValue("@CompanyID", HttpContext.Current.Session["LastCompany"].ToString());
            cmd.Parameters.AddWithValue("@Project_Id", ProjId);
            cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 50);
            cmd.Parameters.Add("@ReturnId", SqlDbType.VarChar, 10);
            cmd.Parameters.Add("@ErrorCode", SqlDbType.Int);

            cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;
            cmd.Parameters["@ReturnId"].Direction = ParameterDirection.Output;
            cmd.Parameters["@ErrorCode"].Direction = ParameterDirection.Output;

            cmd.CommandTimeout = 0;
            SqlDataAdapter Adap = new SqlDataAdapter();
            Adap.SelectCommand = cmd;
            Adap.Fill(dsInst);

            AdjustedId = Convert.ToInt32(cmd.Parameters["@ReturnId"].Value);
            ReturnNumber = Convert.ToString(cmd.Parameters["@ReturnValue"].Value);
            ErrorCode = Convert.ToInt32(cmd.Parameters["@ErrorCode"].Value);
        }

        public DataTable GetDebitNoteListAdd(string date, string VendorId, string Branch)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_DebitNoteAdjustmentInvoice_details");
            proc.AddVarcharPara("@Action", 50, "GetDebitNoteListAdd");
            proc.AddVarcharPara("@VendorId", 15, VendorId);
            proc.AddVarcharPara("@tranDate", 10, date);
            proc.AddIntegerPara("@BranchId", Convert.ToInt32(Branch));
            return proc.GetTable();
        }
        public DataTable GetPurchaseInvoiceDocumentList(string Mode, string ReceiptId, string customerId, string TransDate, string AdjId, string BranchId,
          string AdvType,string ProjectId)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_DebitNoteAdjustmentInvoice_details");
            proc.AddVarcharPara("@Action", 50, "GetDocList");
            proc.AddVarcharPara("@CustomerId", 15, customerId);
            proc.AddIntegerPara("@recPayId", Convert.ToInt32(ReceiptId));
            proc.AddVarcharPara("@tranDate", 10, TransDate);
            proc.AddVarcharPara("@Mode", 10, Mode);
            proc.AddVarcharPara("@AdjId", 10, AdjId);
            proc.AddIntegerPara("@BranchId", Convert.ToInt32(BranchId));
            proc.AddVarcharPara("@AdvType", 20, AdvType);
            return proc.GetTable();
        }
        #endregion

    }
}
