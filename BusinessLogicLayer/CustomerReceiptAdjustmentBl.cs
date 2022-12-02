using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BusinessLogicLayer
{
   public class CustomerReceiptAdjustmentBl
    {
       public DataSet PopulateCustomerReceiptAdjustmentDetails()
       {
           ProcedureExecute proc = new ProcedureExecute("Prc_CustomerReceiptAdjustment_details");
           proc.AddVarcharPara("@Action", 50, "GetLoadDetails");
           proc.AddVarcharPara("@companyId", 20, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
           proc.AddVarcharPara("@FinYear", 10, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
           proc.AddVarcharPara("@BranchList", -1, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
           return proc.GetDataSet();
       }

       public DataTable GetAdvanceListAdd(string date, string customerId,string Branch)
       {
           ProcedureExecute proc = new ProcedureExecute("Prc_CustomerReceiptAdjustment_details");
           proc.AddVarcharPara("@Action", 50, "GetAdvanceListAdd");
           proc.AddVarcharPara("@CustomerId", 15, customerId);
           proc.AddVarcharPara("@tranDate", 10, date);
           proc.AddIntegerPara("@BranchId", Convert.ToInt32(Branch));
           return proc.GetTable();
       }

       public DataTable GetDocumentList(string Mode, string ReceiptId, string customerId, string TransDate,string AdjId,string BranchId,
           string AdvType, string ProjectId) 
       {
           if (Convert.ToInt16(ProjectId) == 0)
           {
               ProcedureExecute proc = new ProcedureExecute("Prc_CustomerReceiptAdjustment_details");
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
           else
           {
               ProcedureExecute proc = new ProcedureExecute("Prc_CustomerReceiptAdjustment_details");
               proc.AddVarcharPara("@Action", 50, "GetDocListWithProject");
               proc.AddVarcharPara("@CustomerId", 15, customerId);
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
           ProcedureExecute proc = new ProcedureExecute("Prc_CustomerReceiptAdjustment_details");
           proc.AddVarcharPara("@Action", 50, "Delete");
           proc.AddVarcharPara("@AdjId", 10, AdjId);
          return proc.RunActionQuery();
       }

       public DataSet GetEditedData(string AdjId)
       {
           ProcedureExecute proc = new ProcedureExecute("Prc_CustomerReceiptAdjustment_details");
           proc.AddVarcharPara("@Action", 50, "GetEditedData");
           proc.AddVarcharPara("@AdjId", 10, AdjId);
           proc.AddVarcharPara("@companyId", 20, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
           proc.AddVarcharPara("@FinYear", 10, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
           proc.AddVarcharPara("@BranchList", -1, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
           return proc.GetDataSet();
       }
       public DataSet GetEditedDataOnaccount(string AdjId)
       {
           ProcedureExecute proc = new ProcedureExecute("Prc_CustomerCrNoteAdjustment_details");
           proc.AddVarcharPara("@Action", 50, "GetEditedData");
           proc.AddVarcharPara("@AdjId", 10, AdjId);
           proc.AddVarcharPara("@companyId", 20, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
           proc.AddVarcharPara("@FinYear", 10, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
           proc.AddVarcharPara("@BranchList", -1, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
           return proc.GetDataSet();
       }
       public void AddEditAdvanceAdjustment(string Mode, string SchemeId, string Adjustment_No, string Adjustment_Date, string Branch, string Customer_id,
           string Adjusted_doc_id, string Adjusted_Doc_no, string Adjusted_DocAmt, string ExchangeRate, string Adjusted_DocAmt_inBaseCur,
           string Remarks, string Adjusted_DocOSAmt, string Adjusted_Amount, string userId, ref int AdjustedId, ref string ReturnNumber,
           DataTable AdjustmentTable, ref int ErrorCode, string Adj_id, string AdvType, string Project_Id)
       {
           DataTable dsInst = new DataTable();


                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);   MULTI

                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));


                SqlCommand cmd = new SqlCommand("prc_CustomerReceiptAdjustment_AddEdit", con);
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
                cmd.Parameters.AddWithValue("@Project_Id", Project_Id);
                cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 50);
                cmd.Parameters.Add("@ReturnId", SqlDbType.VarChar,10);
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

                cmd.Dispose();
                con.Dispose();
       }



       public void AddEditOnAccountAdjustment(string Mode, string SchemeId, string Adjustment_No, string Adjustment_Date, string Branch, string Customer_id,
           string Adjusted_doc_id, string Adjusted_Doc_no, string Adjusted_DocAmt, string ExchangeRate, string Adjusted_DocAmt_inBaseCur,
           string Remarks, string Adjusted_DocOSAmt, string Adjusted_Amount, string userId,Int64 ProjId, ref int AdjustedId, ref string ReturnNumber,
           DataTable AdjustmentTable, ref int ErrorCode, string Adj_id, string CrNoteType)
       {
           DataTable dsInst = new DataTable();

        //   SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);

           SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

           SqlCommand cmd = new SqlCommand("prc_CustomerCrNoteAdjustment_AddEdit", con);
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
           cmd.Parameters.AddWithValue("@FinYear", HttpContext.Current.Session["LastFinYear"].ToString());
           cmd.Parameters.AddWithValue("@CompanyID", HttpContext.Current.Session["LastCompany"].ToString());
           cmd.Parameters.AddWithValue("@CrNoteType", CrNoteType);
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


       public DataTable GetOnAccountList(string customerId, string TransDate,string BranchId)
       {
           ProcedureExecute proc = new ProcedureExecute("Prc_CustomerCrNoteAdjustment_details");
           proc.AddVarcharPara("@Action", 50, "GetOnAccountList");
           proc.AddVarcharPara("@CustomerId", 15, customerId);
         //  proc.AddIntegerPara("@recPayId", Convert.ToInt32(ReceiptId));
           proc.AddVarcharPara("@tranDate", 10, TransDate);
           //proc.AddVarcharPara("@Mode", 10, Mode);
           //proc.AddVarcharPara("@AdjId", 10, AdjId);
           proc.AddIntegerPara("@EnterBranchId", Convert.ToInt32(BranchId));
           return proc.GetTable();
       }

       public DataSet PopulateCustomerOnAccountAdjustmentDetails()
       {
           ProcedureExecute proc = new ProcedureExecute("Prc_CustomerCrNoteAdjustment_details");
           proc.AddVarcharPara("@Action", 50, "GetLoadDetails");
           proc.AddVarcharPara("@companyId", 20, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
           proc.AddVarcharPara("@FinYear", 10, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
           proc.AddVarcharPara("@BranchList", -1, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
           return proc.GetDataSet();
       }
       public DataTable GetOnAccountDocumentList(string Mode, string ReceiptId, string customerId, string TransDate, string AdjId, string BranchId, string ProjectId)
       {
           ProcedureExecute proc = new ProcedureExecute("Prc_CustomerCrNoteAdjustment_details");

           if(Convert.ToInt16(ProjectId)==0)
           {
               
               proc.AddVarcharPara("@Action", 50, "GetDocList");
               proc.AddVarcharPara("@CustomerId", 15, customerId);
               proc.AddIntegerPara("@recPayId", Convert.ToInt32(ReceiptId));
               proc.AddVarcharPara("@tranDate", 10, TransDate);
               proc.AddVarcharPara("@Mode", 10, Mode);
               proc.AddVarcharPara("@AdjId", 10, AdjId);
               proc.AddIntegerPara("@BranchId", Convert.ToInt32(BranchId));
              
           }
           else
           {
               proc.AddVarcharPara("@Action", 50, "GetDocListWithProject");
               proc.AddVarcharPara("@CustomerId", 15, customerId);
               proc.AddIntegerPara("@recPayId", Convert.ToInt32(ReceiptId));
               proc.AddVarcharPara("@tranDate", 10, TransDate);
               proc.AddVarcharPara("@Mode", 10, Mode);
               proc.AddVarcharPara("@AdjId", 10, AdjId);
               proc.AddIntegerPara("@BranchId", Convert.ToInt32(BranchId));
               proc.AddIntegerPara("@ProjectId", Convert.ToInt32(ProjectId));
           }
           return proc.GetTable();
          
       }
       public int DeleteAdjForCrNote(string AdjId)
       {
           ProcedureExecute proc = new ProcedureExecute("Prc_CustomerCrNoteAdjustment_details");
           proc.AddVarcharPara("@Action", 50, "Delete");
           proc.AddVarcharPara("@AdjId", 10, AdjId);
           return proc.RunActionQuery();
       }


       #region Adjustment of Crdit Note With Debit Note

       public DataSet GetEditedDataCrNOteWithDrNote(string AdjId)
       {
           ProcedureExecute proc = new ProcedureExecute("Prc_CustomerCrNoteAdjustmentDrNote_details");
           proc.AddVarcharPara("@Action", 50, "GetEditedData");
           proc.AddVarcharPara("@AdjId", 10, AdjId);
           proc.AddVarcharPara("@companyId", 20, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
           proc.AddVarcharPara("@FinYear", 10, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
           proc.AddVarcharPara("@BranchList", -1, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
           return proc.GetDataSet();
       }

       public DataSet PopulateCrNoteAdjustmentDrNoteDetails()
       {
           ProcedureExecute proc = new ProcedureExecute("Prc_CustomerCrNoteAdjustmentDrNote_details");
           proc.AddVarcharPara("@Action", 50, "GetLoadDetails");
           proc.AddVarcharPara("@companyId", 20, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
           proc.AddVarcharPara("@FinYear", 10, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
           proc.AddVarcharPara("@BranchList", -1, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
           return proc.GetDataSet();
       }
       public DataTable GetDeNoteDocumentList(string Mode, string ReceiptId, string customerId, string TransDate, string AdjId, string BranchId, string ProjectId)
       {
           if (Convert.ToInt16(ProjectId) == 0)
           {
               ProcedureExecute proc = new ProcedureExecute("Prc_CustomerCrNoteAdjustmentDrNote_details");
               proc.AddVarcharPara("@Action", 50, "GetDocList");
               proc.AddVarcharPara("@CustomerId", 15, customerId);
               proc.AddIntegerPara("@recPayId", Convert.ToInt32(ReceiptId));
               proc.AddVarcharPara("@tranDate", 10, TransDate);
               proc.AddVarcharPara("@Mode", 10, Mode);
               proc.AddVarcharPara("@AdjId", 10, AdjId);
               proc.AddIntegerPara("@BranchId", Convert.ToInt32(BranchId));
               return proc.GetTable();
           }
           else
           {
               ProcedureExecute proc = new ProcedureExecute("Prc_CustomerCrNoteAdjustmentDrNote_details");
               proc.AddVarcharPara("@Action", 50, "GetDocListWithProject");
               proc.AddVarcharPara("@CustomerId", 15, customerId);
               proc.AddIntegerPara("@recPayId", Convert.ToInt32(ReceiptId));
               proc.AddVarcharPara("@tranDate", 10, TransDate);
               proc.AddVarcharPara("@Mode", 10, Mode);
               proc.AddVarcharPara("@AdjId", 10, AdjId);
               proc.AddIntegerPara("@BranchId", Convert.ToInt32(BranchId));
               proc.AddIntegerPara("@ProjectId", Convert.ToInt32(ProjectId));
               return proc.GetTable();
           }
       }

       public int DeleteAdjForCrNoteWithDrNote(string AdjId)
       {
           ProcedureExecute proc = new ProcedureExecute("Prc_CustomerCrNoteAdjustmentDrNote_details");
           proc.AddVarcharPara("@Action", 50, "Delete");
           proc.AddVarcharPara("@AdjId", 10, AdjId);
           return proc.RunActionQuery();
       }

       public void AddEditCrNoteAdjustmentDrNote(string Mode, string SchemeId, string Adjustment_No, string Adjustment_Date, string Branch, string Customer_id,
    string Adjusted_doc_id, string Adjusted_Doc_no, string Adjusted_DocAmt, string ExchangeRate, string Adjusted_DocAmt_inBaseCur,
    string Remarks, string Adjusted_DocOSAmt, string Adjusted_Amount, string userId, ref int AdjustedId, ref string ReturnNumber,
    DataTable AdjustmentTable, ref int ErrorCode, string Adj_id, string CrNoteType, string Project_Id)
       {
           DataTable dsInst = new DataTable();         

           SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
           SqlCommand cmd = new SqlCommand("prc_CustomerCrNoteAdjustmentDrNote_AddEdit", con);
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
           cmd.Parameters.AddWithValue("@FinYear", HttpContext.Current.Session["LastFinYear"].ToString());
           cmd.Parameters.AddWithValue("@CompanyID", HttpContext.Current.Session["LastCompany"].ToString());
           cmd.Parameters.AddWithValue("@CrNoteType", CrNoteType);
           cmd.Parameters.AddWithValue("@Project_Id", Project_Id);

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
       #endregion


       #region Adjustment of Advance With Debit Note
       public DataSet PopulateCustomerAdvanceAdjustmentDrNoteDetails()
       {
           ProcedureExecute proc = new ProcedureExecute("Prc_CustomerDrNoteAdjustment_details");
           proc.AddVarcharPara("@Action", 50, "GetLoadDetails");
           proc.AddVarcharPara("@companyId", 20, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
           proc.AddVarcharPara("@FinYear", 10, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
           proc.AddVarcharPara("@BranchList", -1, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
           return proc.GetDataSet();
       }
       public DataTable GetDrNoteDocumentList(string Mode, string ReceiptId, string customerId, string TransDate, string AdjId, string BranchId, string ProjectId)
       {
           if (Convert.ToInt16(ProjectId) == 0)
           {
               ProcedureExecute proc = new ProcedureExecute("Prc_CustomerDrNoteAdjustment_details");
               proc.AddVarcharPara("@Action", 50, "GetDocList");
               proc.AddVarcharPara("@CustomerId", 15, customerId);
               proc.AddIntegerPara("@recPayId", Convert.ToInt32(ReceiptId));
               proc.AddVarcharPara("@tranDate", 10, TransDate);
               proc.AddVarcharPara("@Mode", 10, Mode);
               proc.AddVarcharPara("@AdjId", 10, AdjId);
               proc.AddIntegerPara("@BranchId", Convert.ToInt32(BranchId));
               return proc.GetTable();
           }
           else
           {
               ProcedureExecute proc = new ProcedureExecute("Prc_CustomerDrNoteAdjustment_details");
               proc.AddVarcharPara("@Action", 50, "GetDocListWithProject");
               proc.AddVarcharPara("@CustomerId", 15, customerId);
               proc.AddIntegerPara("@recPayId", Convert.ToInt32(ReceiptId));
               proc.AddVarcharPara("@tranDate", 10, TransDate);
               proc.AddVarcharPara("@Mode", 10, Mode);
               proc.AddVarcharPara("@AdjId", 10, AdjId);
               proc.AddIntegerPara("@BranchId", Convert.ToInt32(BranchId));
               proc.AddIntegerPara("@ProjectId", Convert.ToInt32(ProjectId));
               return proc.GetTable();
           }
         
       }
       public void AddEditDrNoteAdvanceAdjustment(string Mode, string SchemeId, string Adjustment_No, string Adjustment_Date, string Branch, string Customer_id,
          string Adjusted_doc_id, string Adjusted_Doc_no, string Adjusted_DocAmt, string ExchangeRate, string Adjusted_DocAmt_inBaseCur,
          string Remarks, string Adjusted_DocOSAmt, string Adjusted_Amount, string userId, ref int AdjustedId, ref string ReturnNumber,
          DataTable AdjustmentTable, ref int ErrorCode, string Adj_id, string CrNoteType, string Project_Id)
       {
           try
           {
               DataTable dsInst = new DataTable();


              // SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
               SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

               SqlCommand cmd = new SqlCommand("prc_CustomerAdvanceAdstDrNote_AddEdit", con);
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
               cmd.Parameters.AddWithValue("@FinYear", HttpContext.Current.Session["LastFinYear"].ToString());
               cmd.Parameters.AddWithValue("@CompanyID", HttpContext.Current.Session["LastCompany"].ToString());
               cmd.Parameters.AddWithValue("@CrNoteType", CrNoteType);
               cmd.Parameters.AddWithValue("@Project_Id", Project_Id);
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

       public DataSet GetEditedDrNoteData(string AdjId)
       {
           ProcedureExecute proc = new ProcedureExecute("Prc_CustomerDrNoteAdjustment_details");
           proc.AddVarcharPara("@Action", 50, "GetEditedData");
           proc.AddVarcharPara("@AdjId", 10, AdjId);
           proc.AddVarcharPara("@companyId", 20, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
           proc.AddVarcharPara("@FinYear", 10, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
           proc.AddVarcharPara("@BranchList", -1, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
           return proc.GetDataSet();
       }
       public int DeleteDrNoteAdj(string AdjId)
       {
           ProcedureExecute proc = new ProcedureExecute("Prc_CustomerDrNoteAdjustment_details");
           proc.AddVarcharPara("@Action", 50, "Delete");
           proc.AddVarcharPara("@AdjId", 10, AdjId);
           return proc.RunActionQuery();
       }
       public DataTable GetAdvanceListAddForDrNote(string date, string customerId, string Branch)
       {
           ProcedureExecute proc = new ProcedureExecute("Prc_CustomerDrNoteAdjustment_details");
           proc.AddVarcharPara("@Action", 50, "GetAdvanceListAdd");
           proc.AddVarcharPara("@CustomerId", 15, customerId);
           proc.AddVarcharPara("@tranDate", 10, date);
           proc.AddIntegerPara("@BranchId", Convert.ToInt32(Branch));
           return proc.GetTable();
       }

       #endregion

        #region Adjustment of Journal With Sale Invoice
       public DataSet PopulateJournalDebtorsAdjustmentDetails()
       {

           ProcedureExecute proc = new ProcedureExecute("Prc_JournalDebtorsAdjustmentInvoice_details");
           proc.AddVarcharPara("@Action", 50, "GetLoadDetails");
           proc.AddVarcharPara("@companyId", 20, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
           proc.AddVarcharPara("@FinYear", 10, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
           proc.AddVarcharPara("@BranchList", -1, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
           return proc.GetDataSet();
       }

       public DataTable GetJournalListAdd(string date, string customerId, string Branch)
       {
           ProcedureExecute proc = new ProcedureExecute("Prc_JournalDebtorsAdjustmentInvoice_details");
           proc.AddVarcharPara("@Action", 50, "GetAdvanceListAdd");
           proc.AddVarcharPara("@CustomerId", 15, customerId);
           proc.AddVarcharPara("@tranDate", 10, date);
           proc.AddIntegerPara("@BranchId", Convert.ToInt32(Branch));
           return proc.GetTable();
       }


       public void AddEditJournalAdjustmentInvoice(string Mode, string SchemeId, string Adjustment_No, string Adjustment_Date, string Branch, string Customer_id,
          string Adjusted_doc_id, string Adjusted_Doc_no, string Adjusted_DocAmt, string ExchangeRate, string Adjusted_DocAmt_inBaseCur,
          string Remarks, string Adjusted_DocOSAmt, string Adjusted_Amount, string userId, ref int AdjustedId, ref string ReturnNumber,
          DataTable AdjustmentTable, ref int ErrorCode, string Adj_id, string AdvType, string Project_Id)
       {
           DataTable dsInst = new DataTable();


           //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);   MULTI

           SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));


           SqlCommand cmd = new SqlCommand("prc_JournalDebtorsAdjustmentInvoice_AddEdit", con);
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
           cmd.Parameters.AddWithValue("@Project_Id", Project_Id);
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


       public DataSet GetEditedJounalDebtorsData(string AdjId)
       {
           ProcedureExecute proc = new ProcedureExecute("Prc_JournalDebtorsAdjustmentInvoice_details");
           proc.AddVarcharPara("@Action", 50, "GetEditedData");
           proc.AddVarcharPara("@AdjId", 10, AdjId);
           proc.AddVarcharPara("@companyId", 20, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
           proc.AddVarcharPara("@FinYear", 10, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
           proc.AddVarcharPara("@BranchList", -1, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
           return proc.GetDataSet();
       }

       public int DeleteJournalSaleInvoiceAdj(string AdjId)
       {
           ProcedureExecute proc = new ProcedureExecute("Prc_JournalDebtorsAdjustmentInvoice_details");
           proc.AddVarcharPara("@Action", 50, "Delete");
           proc.AddVarcharPara("@AdjId", 10, AdjId);
           return proc.RunActionQuery();
       }

        #endregion



       #region Adjustment of Journal With Purchasee Invoice
       public DataSet PopulateJournalCreditorsAdjustmentDetails()
       {

           ProcedureExecute proc = new ProcedureExecute("Prc_JournalCreditorsAdjustmentInvoice_details");
           proc.AddVarcharPara("@Action", 50, "GetLoadDetails");
           proc.AddVarcharPara("@companyId", 20, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
           proc.AddVarcharPara("@FinYear", 10, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
           proc.AddVarcharPara("@BranchList", -1, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
           return proc.GetDataSet();
       }
       public int DeleteJournalPurchaseInvoiceAdj(string AdjId)
       {
           ProcedureExecute proc = new ProcedureExecute("Prc_JournalCreditorsAdjustmentInvoice_details");
           proc.AddVarcharPara("@Action", 50, "Delete");
           proc.AddVarcharPara("@AdjId", 10, AdjId);
           return proc.RunActionQuery();
       }

       public DataSet GetEditedJounalCreditorData(string AdjId)
       {
           ProcedureExecute proc = new ProcedureExecute("Prc_JournalCreditorsAdjustmentInvoice_details");
           proc.AddVarcharPara("@Action", 50, "GetEditedData");
           proc.AddVarcharPara("@AdjId", 10, AdjId);
           proc.AddVarcharPara("@companyId", 20, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
           proc.AddVarcharPara("@FinYear", 10, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
           proc.AddVarcharPara("@BranchList", -1, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
           return proc.GetDataSet();
       }

       public void AddEditJournalAdjustmentPurchaseInvoice(string Mode, string SchemeId, string Adjustment_No, string Adjustment_Date, string Branch, string Customer_id,
        string Adjusted_doc_id, string Adjusted_Doc_no, string Adjusted_DocAmt, string ExchangeRate, string Adjusted_DocAmt_inBaseCur,
        string Remarks, string Adjusted_DocOSAmt, string Adjusted_Amount, string userId, Int64 ProjId, ref int AdjustedId, ref string ReturnNumber,
        DataTable AdjustmentTable, ref int ErrorCode, string Adj_id, string AdvType)
       {
           DataTable dsInst = new DataTable();


           //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);   MULTI

           SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));


           SqlCommand cmd = new SqlCommand("prc_JournalCreditorsAdjustmentInvoice_AddEdit", con);
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

       public DataTable GetJournalCreditosListAdd(string date, string customerId, string Branch)
       {
           ProcedureExecute proc = new ProcedureExecute("Prc_JournalCreditorsAdjustmentInvoice_details");
           proc.AddVarcharPara("@Action", 50, "GetAdvanceListAdd");
           proc.AddVarcharPara("@CustomerId", 15, customerId);
           proc.AddVarcharPara("@tranDate", 10, date);
           proc.AddIntegerPara("@BranchId", Convert.ToInt32(Branch));
           return proc.GetTable();
       }
       public DataTable GetPurchaseInvoiceDocumentList(string Mode, string ReceiptId, string customerId, string TransDate, string AdjId, string BranchId,
         string AdvType, string ProjectId)
       {
           if (Convert.ToInt16(ProjectId) == 0)
           {
               ProcedureExecute proc = new ProcedureExecute("Prc_JournalCreditorsAdjustmentInvoice_details");
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
           else
           {
               ProcedureExecute proc = new ProcedureExecute("Prc_JournalCreditorsAdjustmentInvoice_details");
               proc.AddVarcharPara("@Action", 50, "GetDocListWithProjectPurchaseInvoice");
               proc.AddVarcharPara("@CustomerId", 15, customerId);
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
       #endregion

       #region Adjustment of Journal With Purchasee Return
       public DataSet PopulateJournalCreditorsAdjustmentPReturnDetails()
       {

           ProcedureExecute proc = new ProcedureExecute("Prc_JournalCreditorsAdjustmentPReturn_details");
           proc.AddVarcharPara("@Action", 50, "GetLoadDetails");
           proc.AddVarcharPara("@companyId", 20, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
           proc.AddVarcharPara("@FinYear", 10, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
           proc.AddVarcharPara("@BranchList", -1, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
           return proc.GetDataSet();
       }
       public int DeleteJournalPurchaseReturnAdj(string AdjId)
       {
           ProcedureExecute proc = new ProcedureExecute("Prc_JournalCreditorsAdjustmentPReturn_details");
           proc.AddVarcharPara("@Action", 50, "Delete");
           proc.AddVarcharPara("@AdjId", 10, AdjId);
           return proc.RunActionQuery();
       }

       public DataSet GetEditedJounalCreditorPReturnData(string AdjId)
       {
           ProcedureExecute proc = new ProcedureExecute("Prc_JournalCreditorsAdjustmentPReturn_details");
           proc.AddVarcharPara("@Action", 50, "GetEditedData");
           proc.AddVarcharPara("@AdjId", 10, AdjId);
           proc.AddVarcharPara("@companyId", 20, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
           proc.AddVarcharPara("@FinYear", 10, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
           proc.AddVarcharPara("@BranchList", -1, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
           return proc.GetDataSet();
       }

       public void AddEditJournalAdjustmentPurchaseReturn(string Mode, string SchemeId, string Adjustment_No, string Adjustment_Date, string Branch, string Customer_id,
        string Adjusted_doc_id, string Adjusted_Doc_no, string Adjusted_DocAmt, string ExchangeRate, string Adjusted_DocAmt_inBaseCur,
        string Remarks, string Adjusted_DocOSAmt, string Adjusted_Amount, string userId, Int64 ProjId, ref int AdjustedId, ref string ReturnNumber,
        DataTable AdjustmentTable, ref int ErrorCode, string Adj_id, string AdvType)
       {
           DataTable dsInst = new DataTable();


           //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);   MULTI

           SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));


           SqlCommand cmd = new SqlCommand("prc_JournalCreditorsAdjustmentPReturn_AddEdit", con);
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

       public DataTable GetJournalCreditosPRListAdd(string date, string customerId, string Branch)
       {
           ProcedureExecute proc = new ProcedureExecute("Prc_JournalCreditorsAdjustmentPReturn_details");
           proc.AddVarcharPara("@Action", 50, "GetAdvanceListAdd");
           proc.AddVarcharPara("@CustomerId", 15, customerId);
           proc.AddVarcharPara("@tranDate", 10, date);
           proc.AddIntegerPara("@BranchId", Convert.ToInt32(Branch));
           return proc.GetTable();
       }
       public DataTable GetPurchaseReturnDocumentList(string Mode, string ReceiptId, string customerId, string TransDate, string AdjId, string BranchId,
         string AdvType, string ProjectId)
       {
           if (Convert.ToInt16(ProjectId) == 0)
           {
               ProcedureExecute proc = new ProcedureExecute("Prc_JournalCreditorsAdjustmentPReturn_details");
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
           else
           {
               ProcedureExecute proc = new ProcedureExecute("Prc_JournalCreditorsAdjustmentPReturn_details");
               proc.AddVarcharPara("@Action", 50, "GetDocListWithProject");
               proc.AddVarcharPara("@CustomerId", 15, customerId);
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
       #endregion

       #region Adjustment of Journal With Vendor Payment
       public DataSet PopulateJournalCreditorsAdjustmentVendorPayDetails()
       {

           ProcedureExecute proc = new ProcedureExecute("Prc_JournalCreditorsAdjustmentVendorPayment_details");
           proc.AddVarcharPara("@Action", 50, "GetLoadDetails");
           proc.AddVarcharPara("@companyId", 20, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
           proc.AddVarcharPara("@FinYear", 10, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
           proc.AddVarcharPara("@BranchList", -1, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
           return proc.GetDataSet();
       }
       public int DeleteJournalVendorPayAdj(string AdjId)
       {
           ProcedureExecute proc = new ProcedureExecute("Prc_JournalCreditorsAdjustmentVendorPayment_details");
           proc.AddVarcharPara("@Action", 50, "Delete");
           proc.AddVarcharPara("@AdjId", 10, AdjId);
           return proc.RunActionQuery();
       }

       public DataSet GetEditedJounalCreditorVendorPayData(string AdjId)
       {
           ProcedureExecute proc = new ProcedureExecute("Prc_JournalCreditorsAdjustmentVendorPayment_details");
           proc.AddVarcharPara("@Action", 50, "GetEditedData");
           proc.AddVarcharPara("@AdjId", 10, AdjId);
           proc.AddVarcharPara("@companyId", 20, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
           proc.AddVarcharPara("@FinYear", 10, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
           proc.AddVarcharPara("@BranchList", -1, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
           return proc.GetDataSet();
       }

       public void AddEditJournalAdjustmentVendorPay(string Mode, string SchemeId, string Adjustment_No, string Adjustment_Date, string Branch, string Customer_id,
        string Adjusted_doc_id, string Adjusted_Doc_no, string Adjusted_DocAmt, string ExchangeRate, string Adjusted_DocAmt_inBaseCur,
        string Remarks, string Adjusted_DocOSAmt, string Adjusted_Amount, string userId, Int64 ProjId, ref int AdjustedId, ref string ReturnNumber,
        DataTable AdjustmentTable, ref int ErrorCode, string Adj_id, string AdvType)
       {
           DataTable dsInst = new DataTable();


           //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);   MULTI

           SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));


           SqlCommand cmd = new SqlCommand("prc_JournalCreditorsAdjustmentVenPay_AddEdit", con);
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
           cmd.Parameters.AddWithValue("@Project_Id", ProjId);
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

       public DataTable GetJournalCreditosVendorPayListAdd(string date, string customerId, string Branch)
       {
           ProcedureExecute proc = new ProcedureExecute("Prc_JournalCreditorsAdjustmentVendorPayment_details");
           proc.AddVarcharPara("@Action", 50, "GetAdvanceListAdd");
           proc.AddVarcharPara("@CustomerId", 15, customerId);
           proc.AddVarcharPara("@tranDate", 10, date);
           proc.AddIntegerPara("@BranchId", Convert.ToInt32(Branch));
           return proc.GetTable();
       }
       public DataTable GetVendorPayDocumentList(string Mode, string ReceiptId, string customerId, string TransDate, string AdjId, string BranchId,
         string AdvType, string ProjectId)
       {
           if (Convert.ToInt16(ProjectId) == 0)
           {
               ProcedureExecute proc = new ProcedureExecute("Prc_JournalCreditorsAdjustmentVendorPayment_details");
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
           else
           {
               ProcedureExecute proc = new ProcedureExecute("Prc_JournalCreditorsAdjustmentVendorPayment_details");
               proc.AddVarcharPara("@Action", 50, "GetDocListWithProject");
               proc.AddVarcharPara("@CustomerId", 15, customerId);
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
       #endregion

       #region Adjustment of Journal With Vendor Payment
       public DataSet PopulateJournalDetorsAdjustmentVendorRecDetails()
       {

           ProcedureExecute proc = new ProcedureExecute("Prc_JournalDEtorsAdjVendorOnAccount_details");
           proc.AddVarcharPara("@Action", 50, "GetLoadDetails");
           proc.AddVarcharPara("@companyId", 20, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
           proc.AddVarcharPara("@FinYear", 10, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
           proc.AddVarcharPara("@BranchList", -1, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
           return proc.GetDataSet();
       }

       public DataTable GetJournalDetorsVendorRecListAdd(string date, string customerId, string Branch)
       {
           ProcedureExecute proc = new ProcedureExecute("Prc_JournalDEtorsAdjVendorOnAccount_details");
           proc.AddVarcharPara("@Action", 50, "GetAdvanceListAdd");
           proc.AddVarcharPara("@CustomerId", 15, customerId);
           proc.AddVarcharPara("@tranDate", 10, date);
           proc.AddIntegerPara("@BranchId", Convert.ToInt32(Branch));
           return proc.GetTable();
       }

       public DataTable GetVendorRecDocumentList(string Mode, string ReceiptId, string customerId, string TransDate, string AdjId, string BranchId,
         string AdvType, string ProjectId)
       {
           if (Convert.ToInt16(ProjectId) == 0)
           {
               ProcedureExecute proc = new ProcedureExecute("Prc_JournalDEtorsAdjVendorOnAccount_details");
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
           else
           {
               ProcedureExecute proc = new ProcedureExecute("Prc_JournalDEtorsAdjVendorOnAccount_details");
               proc.AddVarcharPara("@Action", 50, "GetDocListWithProject");
               proc.AddVarcharPara("@CustomerId", 15, customerId);
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
       public void AddEditJournalAdjustmentVendorRec(string Mode, string SchemeId, string Adjustment_No, string Adjustment_Date, string Branch, string Customer_id,
       string Adjusted_doc_id, string Adjusted_Doc_no, string Adjusted_DocAmt, string ExchangeRate, string Adjusted_DocAmt_inBaseCur,
       string Remarks, string Adjusted_DocOSAmt, string Adjusted_Amount, string userId,Int64 ProjId, ref int AdjustedId, ref string ReturnNumber,
       DataTable AdjustmentTable, ref int ErrorCode, string Adj_id, string AdvType)
       {
           DataTable dsInst = new DataTable();


           //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);   MULTI

           SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));


           SqlCommand cmd = new SqlCommand("prc_JournalDetorsAdjustmentVenRec_AddEdit", con);
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

       public DataSet GetEditedJounalCreditorVendorRecData(string AdjId)
       {
           ProcedureExecute proc = new ProcedureExecute("Prc_JournalDEtorsAdjVendorOnAccount_details");
           proc.AddVarcharPara("@Action", 50, "GetEditedData");
           proc.AddVarcharPara("@AdjId", 10, AdjId);
           proc.AddVarcharPara("@companyId", 20, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
           proc.AddVarcharPara("@FinYear", 10, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
           proc.AddVarcharPara("@BranchList", -1, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
           return proc.GetDataSet();
       }
       public int DeleteJournalVendorRecAdj(string AdjId)
       {
           ProcedureExecute proc = new ProcedureExecute("Prc_JournalDEtorsAdjVendorOnAccount_details");
           proc.AddVarcharPara("@Action", 50, "Delete");
           proc.AddVarcharPara("@AdjId", 10, AdjId);
           return proc.RunActionQuery();
       }
       #endregion


    }

}
