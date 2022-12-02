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
    public class DebitCreditNoteBL
    {

        public DataTable GetNoteDetailsInEditMode(string NoteID)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("proc_VendorDrCrNoteDetails");
            proc.AddVarcharPara("@Action", 500, "VendorNoteDetails");
            proc.AddVarcharPara("@NoteID", 200, NoteID);
            ds = proc.GetTable();
            return ds;
        }

        //Project Code Tanmoy
        public DataTable GetProjectDetails(string id)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("proc_VendorDrCrNoteDetails");
            proc.AddVarcharPara("@Action", 100, "GetProjectDataForEdit");
            proc.AddVarcharPara("@NoteID", 10, id);

            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetProjectCode(string Proj_Code)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("proc_VendorDrCrNoteDetails");
            proc.AddVarcharPara("@Action", 500, "GetProjectCode");
            proc.AddVarcharPara("@Proj_Code", 200, Proj_Code);
            dt = proc.GetTable();
            return dt;
        }
        //Project Code Tanmoy

        public DataTable GetNoteDetailsAddressInEditMode(string NoteID)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("proc_VendorDrCrNoteDetails");
            proc.AddVarcharPara("@Action", 500, "VendorNoteAddressDetails");
            proc.AddVarcharPara("@NoteID", 200, NoteID);
            ds = proc.GetTable();
            return ds;
        }


        public DataSet AllDropDownDetailForCashBank(string NoteType)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("proc_VendorDrCrNoteDetails");
            proc.AddVarcharPara("@Action", 100, "GetAllDropDownData");
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@CompanyID", 500, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@userbranchlist", 5000, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            proc.AddVarcharPara("@NoteType", 10, NoteType);
            ds = proc.GetDataSet();
            return ds;
        }

        public void VendorModifyDrCrNote(string ActionType, string SchemaID,string BillNo, string FinYear, string CompanyID, string BranchID, string NoteDate,
                                string CurrencyID, string Narration, string NoteType, string CustomerName, string InvoiceNo, string Currency, decimal Rate, string UserID
                                , DataTable VendorDrCrNoteDetails,
                                string strPartyInvoice, string strPartyDate, DataTable tempBillAddress, DataTable TaxDetailTable,Int64 ProjId, ref int strIsComplete, ref string strVoucharNo,
             ref int strOutNotelNo, string NoteID)
        {
            try
            {

                DataSet dsInst = new DataSet();
                
                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);  MULTI
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                
                SqlCommand cmd = new SqlCommand("proc_VendorDrCrNoteInsertUpdate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", ActionType);
                cmd.Parameters.AddWithValue("@SchemaID", SchemaID);
                cmd.Parameters.AddWithValue("@BillNo", BillNo);
                cmd.Parameters.AddWithValue("@FinYear", FinYear);
                cmd.Parameters.AddWithValue("@CompanyID", CompanyID);
                cmd.Parameters.AddWithValue("@BranchID", BranchID);
                cmd.Parameters.AddWithValue("@NoteDate", Convert.ToDateTime(NoteDate));
                cmd.Parameters.AddWithValue("@CurrencyID", CurrencyID);
                cmd.Parameters.AddWithValue("@Narration", Narration);
                cmd.Parameters.AddWithValue("@NoteType", NoteType);
                cmd.Parameters.AddWithValue("@CustomerName", CustomerName);
                cmd.Parameters.AddWithValue("@InvoiceNo", InvoiceNo);
                cmd.Parameters.AddWithValue("@DCNote_CurrencyId", Currency);
                cmd.Parameters.AddWithValue("@Rate", Rate);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@VendorDrCrNoteDetails", VendorDrCrNoteDetails);

                cmd.Parameters.AddWithValue("@PartyInvoiceNo", strPartyInvoice);
                cmd.Parameters.AddWithValue("@PartyInvoiceDate", strPartyDate);
                cmd.Parameters.AddWithValue("@BillAddress", tempBillAddress);
                cmd.Parameters.AddWithValue("@TaxDetails", TaxDetailTable);

                cmd.Parameters.AddWithValue("@NoteID", NoteID);

                cmd.Parameters.Add("@OutNoteID", SqlDbType.VarChar, 50);
                cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 50);
                cmd.Parameters.Add("@ReturnVoucharNo", SqlDbType.VarChar, 50);
                //Proje code Tanmoy 10-03-2020
                cmd.Parameters.AddWithValue("@Project_Id", ProjId);
                //Project Code Tanmoy
                cmd.Parameters["@OutNoteID"].Direction = ParameterDirection.Output;
                cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;
                cmd.Parameters["@ReturnVoucharNo"].Direction = ParameterDirection.Output;

                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);

                strIsComplete = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value.ToString());
                strOutNotelNo = Convert.ToInt32(cmd.Parameters["@OutNoteID"].Value.ToString());
                strVoucharNo= cmd.Parameters["@ReturnVoucharNo"].Value.ToString();
                cmd.Dispose();
                con.Dispose();
            }
            catch (Exception ex)
            {
            }
        }


        public DataTable GetSearchGridData(string BranchList, string CompanyID, string FinYear, string Action)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("proc_DebitCreditNoteDetails");
            proc.AddVarcharPara("@BranchList", 500, BranchList);
            proc.AddVarcharPara("@CompanyID", 50, CompanyID);
            proc.AddVarcharPara("@FinYear", 50, FinYear);
            proc.AddVarcharPara("@Action", 500, Action);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetNoteDetails(string Action, string NoteID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("proc_DebitCreditNoteDetails");
            proc.AddVarcharPara("@Action", 500, Action);
            proc.AddVarcharPara("@NoteID", 500, NoteID);
            dt = proc.GetTable();
            return dt;
        }

        public void ModifyDrCrNote(string ActionType, string SchemaID, string VoucharNo, string FinYear, string CompanyID, string BranchID, string NoteDate,
                                   string CurrencyID, string Narration, string NoteType, string CustomerName, string InvoiceNo, string Currency, decimal Rate, string UserID, DataTable JournalDetails,
                                   ref int strIsComplete, ref int strOutNotelNo, string strPartyInvoice, string strPartyDate, DataTable tempBillAddress, DataTable TaxDetailTable, string strReason)
        {
            try
            {

                DataSet dsInst = new DataSet();
                
               //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                
                SqlCommand cmd = new SqlCommand("prc_InsertDebitCreditNoteEntry", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", ActionType);
                cmd.Parameters.AddWithValue("@BillNo", VoucharNo);
                cmd.Parameters.AddWithValue("@NoteId", SchemaID);
                
                cmd.Parameters.AddWithValue("@FinYear", FinYear);
                cmd.Parameters.AddWithValue("@CompanyID", CompanyID);
                cmd.Parameters.AddWithValue("@BranchID", BranchID);
                cmd.Parameters.AddWithValue("@NoteDate", Convert.ToDateTime(NoteDate));
                cmd.Parameters.AddWithValue("@CurrencyID", CurrencyID);
                cmd.Parameters.AddWithValue("@Narration", Narration);
                cmd.Parameters.AddWithValue("@NoteType", NoteType);
                cmd.Parameters.AddWithValue("@CustomerName", CustomerName);
                cmd.Parameters.AddWithValue("@InvoiceNo", InvoiceNo);
                cmd.Parameters.AddWithValue("@DCNote_CurrencyId", Currency);
                cmd.Parameters.AddWithValue("@Rate", Rate);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@JournalDetails", JournalDetails);

                cmd.Parameters.AddWithValue("@PartyInvoiceNo", strPartyInvoice);
                cmd.Parameters.AddWithValue("@PartyInvoiceDate", strPartyDate);
                cmd.Parameters.AddWithValue("@BillAddress", tempBillAddress);
                cmd.Parameters.AddWithValue("@TaxDetail", TaxDetailTable);

                cmd.Parameters.AddWithValue("@ReasonID", strReason);

                cmd.Parameters.Add("@OutNoteID", SqlDbType.VarChar, 50);
                cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 50);

                cmd.Parameters["@OutNoteID"].Direction = ParameterDirection.Output;
                cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;

                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);

                strIsComplete = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value.ToString());
                strOutNotelNo = Convert.ToInt32(cmd.Parameters["@OutNoteID"].Value.ToString());

                cmd.Dispose();
                con.Dispose();
            }
            catch (Exception ex)
            {
            }
        }

        //public void ModifyDrCrNote(string ActionType, string NotelNo, string BillNo, string FinYear, string CompanyID, string BranchID, string NoteDate,
        //                            string CurrencyID, string Narration, string NoteType, string CustomerName, string InvoiceNo, string Currency, decimal Rate, string UserID, DataTable JournalDetails,
        //                            ref int strIsComplete, ref int strOutNotelNo, string strPartyInvoice, string strPartyDate, DataTable tempBillAddress)
        //{
        //    try
        //    {
        //        DataSet dsInst = new DataSet();
        //        SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        //        SqlCommand cmd = new SqlCommand("prc_InsertDebitCreditNoteEntry", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@Action", ActionType);
        //        cmd.Parameters.AddWithValue("@NoteID", NotelNo);
        //        cmd.Parameters.AddWithValue("@BillNo", BillNo);
        //        cmd.Parameters.AddWithValue("@FinYear", FinYear);
        //        cmd.Parameters.AddWithValue("@CompanyID", CompanyID);
        //        cmd.Parameters.AddWithValue("@BranchID", BranchID);
        //        cmd.Parameters.AddWithValue("@NoteDate", Convert.ToDateTime(NoteDate));
        //        cmd.Parameters.AddWithValue("@CurrencyID", CurrencyID);
        //        cmd.Parameters.AddWithValue("@Narration", Narration);
        //        cmd.Parameters.AddWithValue("@NoteType", NoteType);
        //        cmd.Parameters.AddWithValue("@CustomerName", CustomerName);
        //        cmd.Parameters.AddWithValue("@InvoiceNo", InvoiceNo);
        //        cmd.Parameters.AddWithValue("@DCNote_CurrencyId", Currency);
        //        cmd.Parameters.AddWithValue("@Rate", Rate);
        //        cmd.Parameters.AddWithValue("@UserID", UserID);
        //        cmd.Parameters.AddWithValue("@JournalDetails", JournalDetails);

        //        cmd.Parameters.AddWithValue("@PartyInvoiceNo", strPartyInvoice);
        //        cmd.Parameters.AddWithValue("@PartyInvoiceDate", strPartyDate);
        //        cmd.Parameters.AddWithValue("@BillAddress", tempBillAddress);

        //        cmd.Parameters.Add("@OutNoteID", SqlDbType.VarChar, 50);
        //        cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 50);

        //        cmd.Parameters["@OutNoteID"].Direction = ParameterDirection.Output;
        //        cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;

        //        cmd.CommandTimeout = 0;
        //        SqlDataAdapter Adap = new SqlDataAdapter();
        //        Adap.SelectCommand = cmd;
        //        Adap.Fill(dsInst);

        //        strIsComplete = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value.ToString());
        //        strOutNotelNo = Convert.ToInt32(cmd.Parameters["@OutNoteID"].Value.ToString());

        //        cmd.Dispose();
        //        con.Dispose();
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}
        public void DeleteDrCrNote(string ActionType, string NotelNo, ref int strIsComplete)
        {
            try
            {
                DataSet dsInst = new DataSet();


               // SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

                SqlCommand cmd = new SqlCommand("prc_InsertDebitCreditNoteEntry", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", ActionType);
                cmd.Parameters.AddWithValue("@UserID", HttpContext.Current.Session["userid"]);
                cmd.Parameters.AddWithValue("@NoteID", NotelNo);

                cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 50);
                cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;

                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);

                strIsComplete = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value.ToString());

                cmd.Dispose();
                con.Dispose();
            }
            catch (Exception ex)
            {
            }
        }
        public DataTable GetInvoiceDetails(string Action, string CustVenID, string FinYear, string BranchList)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("proc_DebitCreditNoteDetails");
            proc.AddVarcharPara("@Action", 500, Action);
            proc.AddVarcharPara("@CustVenID", 500, CustVenID);
            proc.AddVarcharPara("@FinYear", 500, FinYear);
            proc.AddVarcharPara("@BranchList", 500, BranchList);
            dt = proc.GetTable();
            return dt;
        }

    }
}
