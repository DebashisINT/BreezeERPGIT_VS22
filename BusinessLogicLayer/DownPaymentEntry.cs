using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccessLayer;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;

namespace BusinessLogicLayer
{
    public class DownPaymentEntry
    {
        public DataSet GetAllDropDownDetail(string branchList, string FinYear)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Prc_DownPaymentEntry_Details");
            proc.AddVarcharPara("@Action", 100, "GetAllDropDownDetail");
            proc.AddVarcharPara("@BranchList", 4000, branchList);
            proc.AddVarcharPara("@FinYear", 10, FinYear);
            ds = proc.GetDataSet();
            return ds;
        }
        public DataTable GetInvoiceListGridDataByDate(string userbranchlist, string lastCompany, string fromdate, string todate, string branch)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_DownPaymentEntry_Details");
            proc.AddVarcharPara("@Action", 50, "GetDownPaymentList");
            proc.AddVarcharPara("@BranchList", 4000, userbranchlist);
            proc.AddVarcharPara("@fromdate", 50, fromdate);
            proc.AddVarcharPara("@todate", 50, todate);
            proc.AddVarcharPara("@branchId", 50, branch);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetFinInvoiceListGridDataByDate(string userbranchlist, string lastCompany, string fromdate, string todate, string branch)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_DownPaymentEntry_Details");
            proc.AddVarcharPara("@Action", 50, "GetFinanceInvoice");
            proc.AddVarcharPara("@BranchList", 4000, userbranchlist);
            proc.AddVarcharPara("@fromdate", 50, fromdate);
            proc.AddVarcharPara("@todate", 50, todate);
            proc.AddVarcharPara("@branchId", 50, branch);
            dt = proc.GetTable();
            return dt;
        }       
        public DataTable GetDownPaymentDetails(string ID)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_DownPaymentEntry_Details");
            proc.AddVarcharPara("@Action", 50, "GetDownPaymentDetails");
            proc.AddVarcharPara("@DownpaymentID", 10, ID);
            DataTable Returntable = proc.GetTable();
            proc.Dispose();
            return Returntable;
        }
        public DataTable GetFinInvoiceDetails(string ID)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_DownPaymentEntry_Details");
            proc.AddVarcharPara("@Action", 50, "GetFinInvoiceDetails");
            proc.AddVarcharPara("@InvoiceID", 10, ID);
            DataTable Returntable = proc.GetTable();
            proc.Dispose();
            return Returntable;
        }
        public void AddOpening(string DownPaymentNumber, string EntryDate, string DownPaymentDate, string Financer, string Branch, string BillNumber, 
            decimal BillAmount, string BillDate, string ChallanNo,string Customer, decimal FinanceAmount, string SFCode, string ModeofPayment,
            decimal DownPay1, string AdjmntNo, string AdjmntDt, decimal DownPay2, string FinalMr, decimal FinalPayment,
            decimal DbdParcentage, decimal DbdAmount, decimal MbdParcentage, decimal MbdAmount, decimal ProcessFee, decimal TotalPay, decimal Balance,
            string DpStatus, string Narration,string DivestmentNo1, string DivestmentDt1, decimal DivestmentAmt1, string DivestmentNo2, string DivestmentDt2,
            decimal DivestmentAmt2, string DivestmentNo3, string DivestmentDt3, decimal DivestmentAmt3, string product, string DownPaymentID, string InvoiceID, string Action)
        {
            SqlConnection con = null;
            SqlCommand cmd = null;
            try
            {
                DataSet dsInst = new DataSet();
                
                //con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);

                con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                
                cmd = new SqlCommand("prc_AddEditDownPayment", con);

                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", Action);
                cmd.Parameters.AddWithValue("@DownPaymentID", DownPaymentID);
                cmd.Parameters.AddWithValue("@DownPaymentNumber", DownPaymentNumber);
                cmd.Parameters.AddWithValue("@EntryDate", EntryDate);
                cmd.Parameters.AddWithValue("@DownPaymentDate", DownPaymentDate);
                cmd.Parameters.AddWithValue("@Financer", Financer);
                cmd.Parameters.AddWithValue("@Branch", Branch);
                cmd.Parameters.AddWithValue("@BillNumber", BillNumber);
                cmd.Parameters.AddWithValue("@BillAmount", BillAmount);
                cmd.Parameters.AddWithValue("@BillDate", BillDate);
                cmd.Parameters.AddWithValue("@ChallanNo", ChallanNo);
                cmd.Parameters.AddWithValue("@Customer", Customer);
                cmd.Parameters.AddWithValue("@FinanceAmount", FinanceAmount);
                cmd.Parameters.AddWithValue("@SFCode", SFCode);
                cmd.Parameters.AddWithValue("@ModeOfPayment", ModeofPayment);
                cmd.Parameters.AddWithValue("@DownPay1", DownPay1);
                cmd.Parameters.AddWithValue("@AdjustmentNo", AdjmntNo);
                cmd.Parameters.AddWithValue("@AdjustmentDate", AdjmntDt);

                cmd.Parameters.AddWithValue("@DownPay2", DownPay2);
                cmd.Parameters.AddWithValue("@FinalMr", FinalMr);
                cmd.Parameters.AddWithValue("@FinalPayment", FinalPayment);
                cmd.Parameters.AddWithValue("@DbdParcentage", DbdParcentage);
                cmd.Parameters.AddWithValue("@DbdAmount", DbdAmount);
                cmd.Parameters.AddWithValue("@MbdParcentage", MbdParcentage);
                cmd.Parameters.AddWithValue("@MbdAmount", MbdAmount);
                cmd.Parameters.AddWithValue("@ProcessFee", ProcessFee);
                cmd.Parameters.AddWithValue("@TotalPay", TotalPay);
                cmd.Parameters.AddWithValue("@Balance", Balance);
                cmd.Parameters.AddWithValue("@DpStatus", DpStatus);
                cmd.Parameters.AddWithValue("@Narration", Narration);
                cmd.Parameters.AddWithValue("@User", HttpContext.Current.Session["userid"].ToString());

                cmd.Parameters.AddWithValue("@DivestmentNo1", DivestmentNo1);
                cmd.Parameters.AddWithValue("@DivestmentDt1", DivestmentDt1);
                cmd.Parameters.AddWithValue("@DivestmentAmt1", DivestmentAmt1);

                cmd.Parameters.AddWithValue("@DivestmentNo2", DivestmentNo2);
                cmd.Parameters.AddWithValue("@DivestmentDt2", DivestmentDt2);
                cmd.Parameters.AddWithValue("@DivestmentAmt2", DivestmentAmt2);

                cmd.Parameters.AddWithValue("@DivestmentNo3", DivestmentNo3);
                cmd.Parameters.AddWithValue("@DivestmentDt3", DivestmentDt3);
                cmd.Parameters.AddWithValue("@DivestmentAmt3", DivestmentAmt3);
                cmd.Parameters.AddWithValue("@Product", product);
                cmd.Parameters.AddWithValue("@InvoiceID", InvoiceID);

                cmd.ExecuteNonQuery();
                con.Close();
                con.Dispose();
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                con.Close();
                con.Dispose();
                cmd.Dispose();
                throw ex;
            }
        }
        public void DeleteOpening(string DownPaymentID)
        {
            SqlConnection con = null;
            SqlCommand cmd = null;
            try
            {
                DataSet dsInst = new DataSet();

               // con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                
                cmd = new SqlCommand("prc_AddEditDownPayment", con);

                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "DeleteOP");
                cmd.Parameters.AddWithValue("@DownPaymentID", DownPaymentID);
                cmd.ExecuteNonQuery();
                con.Close();
                con.Dispose();
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                con.Close();
                con.Dispose();
                cmd.Dispose();
                throw ex;
            }
        }
        public DataTable GetBranchByParent(string ID)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_DownPaymentEntry_Details");
            proc.AddVarcharPara("@Action", 50, "GetBranchByParent");
            proc.AddVarcharPara("@branchId", 10, ID);
            DataTable Returntable = proc.GetTable();
            proc.Dispose();
            return Returntable;
        }
    }
}
