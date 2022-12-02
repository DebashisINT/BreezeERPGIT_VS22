using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BusinessLogicLayer
{
    public class CustomerNoteBL
    {
        public DataSet CustCrNoteBranch(string @userbranch)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("proc_DebitCreditNoteDetails");
            proc.AddVarcharPara("@Action", 100, "GetAllDropDownBranchForVendorDrCrNote");
            proc.AddVarcharPara("@userbranchlist", 4000, @userbranch);
            ds = proc.GetDataSet();
            return ds; 
        }

        public DataSet GetAllDropDownDataByVoucherType(string VoucherType)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prdn_CustomerNoteDetails");
            proc.AddVarcharPara("@Action", 100, "AllDropDownDataCNote");
            proc.AddVarcharPara("@userbranchlist", 4000, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            proc.AddVarcharPara("@FinYear", 50, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@VoucherType", 10, VoucherType);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataTable GetAllInvoiceForCustomer(string customerId, DateTime TransDate, string BranchID)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prdn_CustomerNoteDetails");
            proc.AddVarcharPara("@Action", 100, "GetAllInvoiceForCustomer");
            proc.AddVarcharPara("@InternalId", 4000, customerId);
            proc.AddDateTimePara("@TransDate", TransDate);
            proc.AddVarcharPara("@BranchId", 100, BranchID);            
            ds = proc.GetTable();
            return ds; 
        }

        public DataTable GetAllInvoiceForCustomerWithProject(string customerId, DateTime TransDate, string BranchID, Int64 ProjectId)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prdn_CustomerNoteDetails");
            proc.AddVarcharPara("@Action", 100, "GetAllInvoiceForCustomerWithProject");
            proc.AddVarcharPara("@InternalId", 4000, customerId);
            proc.AddDateTimePara("@TransDate", TransDate);
            proc.AddVarcharPara("@BranchId", 100, BranchID);
            proc.AddBigIntegerPara("@ProjectId", ProjectId);
            ds = proc.GetTable();
            return ds;
        }

        public DataTable GetProjectCode(string Proj_Code)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prdn_CustomerNoteDetails");
            proc.AddVarcharPara("@Action", 500, "GetProjectCode");
            proc.AddVarcharPara("@Proj_Code", 200, Proj_Code);
            dt = proc.GetTable();
            return dt;
        }

        //Rev v1.0.101  subhra  07-01-2019  0019425 
        //public string AddEditPayment(ref string OutputId, string ActionType, string NoteId, string NoteBranchID, DateTime TransactionDate, string TransactionType, string CustomerId, string Currency, string rate, DataTable BatchGridData, DataTable BillAddress, string CompanyId, string LastFinYear, string userid, string SCHEMEID, string Doc_No, DataTable TaxRecord, string InvoiceId, string Narration, string PartyInvoiceNo, string ReasonID,DateTime? PartyInvoiceDate)
        public string AddEditPayment(ref string OutputId, ref string OutputNoteId, string ActionType, string NoteId, string NoteBranchID, DateTime TransactionDate, string TransactionType, string CustomerId, string Currency, string rate, DataTable BatchGridData, DataTable BillAddress, string CompanyId, string LastFinYear, string userid, string SCHEMEID, string Doc_No, DataTable TaxRecord, string InvoiceId, string Narration, string PartyInvoiceNo, string ReasonID, DateTime? PartyInvoiceDate, Int64 ProjId
            , string Segment1, string Segment2, string Segment3, string Segment4, string Segment5
            )
        //End of Rev
        {

            try
            {

                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("prc_AddEditCustomerNote", con);
                DataTable NoteTable = new DataTable();
                NoteTable = GetReceiptDataSource();
                foreach (DataRow dr in BatchGridData.Rows)
                {

                    NoteTable.Rows.Add(Convert.ToString(dr["Note_Id"]), Convert.ToString(dr["gvColMainAccount"]), Convert.ToString(dr["gvColSubAccount"]),
                       Convert.ToDecimal(dr["btnRecieve"]), Convert.ToDecimal(dr["TaxAmount"]), Convert.ToDecimal(dr["NetAmount"]), Convert.ToString(dr["btnLineNarration"]));
                }

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", ActionType);
                cmd.Parameters.AddWithValue("@NoteId", NoteId);
                cmd.Parameters.AddWithValue("@CompanyID", CompanyId);
                cmd.Parameters.AddWithValue("@FinYear", LastFinYear);
                cmd.Parameters.AddWithValue("@CreateUser", userid);
                cmd.Parameters.AddWithValue("@BranchID", NoteBranchID);
                cmd.Parameters.AddWithValue("@TransactionDate", TransactionDate);
                cmd.Parameters.AddWithValue("@CurrencyID", Currency);
                cmd.Parameters.AddWithValue("@rate", rate);
                cmd.Parameters.AddWithValue("@CustomerID", CustomerId);
                cmd.Parameters.AddWithValue("@Details", NoteTable);
                cmd.Parameters.AddWithValue("@TransactionType", TransactionType);
                cmd.Parameters.AddWithValue("@TaxRecord", TaxRecord);
                cmd.Parameters.AddWithValue("@BillAddress", BillAddress);
                cmd.Parameters.AddWithValue("@SCHEMEID", SCHEMEID);
                cmd.Parameters.AddWithValue("@Doc_No", Doc_No);
                cmd.Parameters.AddWithValue("@InvoiceId", InvoiceId);

                cmd.Parameters.AddWithValue("@Narration", Narration);
               // cmd.Parameters.AddWithValue("@Narration", "");
                cmd.Parameters.AddWithValue("@PartyInvoiceNo", PartyInvoiceNo);
                if (PartyInvoiceDate!=null)
                cmd.Parameters.AddWithValue("@PartyInvoiceDate", PartyInvoiceDate);
                cmd.Parameters.AddWithValue("@Project_Id", ProjId);
                cmd.Parameters.AddWithValue("@ReasonID", ReasonID);


                cmd.Parameters.AddWithValue("@SegmentID1", Segment1);
                cmd.Parameters.AddWithValue("@SegmentID2", Segment2);
                cmd.Parameters.AddWithValue("@SegmentID3", Segment3);
                cmd.Parameters.AddWithValue("@SegmentID4", Segment4);
                cmd.Parameters.AddWithValue("@SegmentID5", Segment5);

                SqlParameter output = new SqlParameter("@ReturnValue", SqlDbType.VarChar,200);
                output.Direction = ParameterDirection.Output;
                SqlParameter outputText = new SqlParameter("@ReturnText", SqlDbType.VarChar, 200);
                outputText.Direction = ParameterDirection.Output;
                //Rev v1.0.101  subhra  07-01-2019  0019425 
                SqlParameter outputNId = new SqlParameter("@ReturnNoteId", SqlDbType.BigInt);
                outputNId.Direction = ParameterDirection.Output;
                //End of Rev
                cmd.Parameters.Add(output);
                cmd.Parameters.Add(outputText);
                //Rev v1.0.101  subhra  07-01-2019  0019425 
                cmd.Parameters.Add(outputNId);
                //End of Rev
                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                con.Dispose();
                OutputId = Convert.ToString(cmd.Parameters["@ReturnValue"].Value.ToString());
                string strCPRID = Convert.ToString(cmd.Parameters["@ReturnText"].Value.ToString());
                //Rev v1.0.101  subhra  07-01-2019  0019425 
                OutputNoteId = Convert.ToString(cmd.Parameters["@ReturnNoteId"].Value.ToString());
                //End of Rev
                return Convert.ToString(strCPRID);
            }
            catch (Exception ex)
            {
                return "Please try again later.";
            }




            
        }

        private DataTable GetReceiptDataSource()
        {
            DataTable NoteTable = new DataTable();
            NoteTable.Columns.Add("Note_Id", typeof(System.String));
            NoteTable.Columns.Add("MainAccountId", typeof(System.String));
            NoteTable.Columns.Add("SubAccountId", typeof(System.String));
            NoteTable.Columns.Add("Amount", typeof(System.Decimal));
            NoteTable.Columns.Add("Charges", typeof(System.Decimal));
            NoteTable.Columns.Add("NetAmount", typeof(System.Decimal));
            NoteTable.Columns.Add("Remarks", typeof(System.String));
            return NoteTable;
        }

        public DataSet GetEditDetails(string id,string userbranchHierarchy,string LastFinYear,string userid)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prdn_CustomerNoteDetails");
            proc.AddVarcharPara("@Action", 100, "GetEditDetails");
            proc.AddVarcharPara("@userbranchlist", 4000, userbranchHierarchy);
            proc.AddVarcharPara("@FinYear", 50, LastFinYear);
            proc.AddVarcharPara("@Note_Id", 10, id);
            proc.AddVarcharPara("@UserID", 10, userid);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataTable GetProjectDetails(string id)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prdn_CustomerNoteDetails");
            proc.AddVarcharPara("@Action", 100, "GetProjectDataForEdit");
            proc.AddVarcharPara("@Note_Id", 10, id);
           
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetExactTable(DataTable TaxTable)
        {
            DataSet dsInst = new DataSet();
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("prdn_CustomerNoteDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Action", "GetExactTable");
            cmd.Parameters.AddWithValue("@TaxRecord", TaxTable);
            cmd.CommandTimeout = 0;
            SqlDataAdapter Adap = new SqlDataAdapter();
            Adap.SelectCommand = cmd;
            Adap.Fill(dsInst);
            cmd.Dispose();
            con.Dispose();
            return dsInst.Tables[0];

        }





    }
}
