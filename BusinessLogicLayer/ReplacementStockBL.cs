using DataAccessLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class ReplacementStockBL
    {
        public DataTable GetReplacementListProductList(string BranchId, string ComapnyId, string Finyear, string Action)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_ReplacementNoteOutList");
            proc.AddPara("@Action", Action);
            proc.AddPara("@BranchList", BranchId);
            proc.AddPara("@companyId", ComapnyId);
            proc.AddPara("@finyear", Finyear);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable ProductList(string ChallanID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_ReplacementNoteOutList");
            proc.AddPara("@Action", "ProductDetails");
            proc.AddPara("@ChallanID", ChallanID);         
            dt = proc.GetTable();
            return dt;
        }
        public void ModifyReplacementStock(string ReplacementStockOutID, string ReplacementNumber,string ReplacementID, string InvoiceID, string ChallanID, string StockType, DataTable Warehousedt,
                                            string CompanyID, string FinYear, string BranchID, string StockOutID, string UserID, string ActionType,
                                            ref int strIsComplete, ref int strInvoiceID)
        {
            try
            {
                DataSet dsInst = new DataSet();

                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

                SqlCommand cmd = new SqlCommand("prc_ModifyReplacementNote", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", ActionType);
                cmd.Parameters.AddWithValue("@ReplacementStockOutID", ReplacementStockOutID);
                cmd.Parameters.AddWithValue("@ReplacementNumber", ReplacementNumber);
                cmd.Parameters.AddWithValue("@ReplacementID", ReplacementID);
                cmd.Parameters.AddWithValue("@InvoiceID", InvoiceID);
                cmd.Parameters.AddWithValue("@ChallanID", ChallanID);
                cmd.Parameters.AddWithValue("@StockType", StockType);
                cmd.Parameters.AddWithValue("@CompanyID", CompanyID);
                cmd.Parameters.AddWithValue("@FinYear", FinYear);
                cmd.Parameters.AddWithValue("@BranchID", BranchID);
                cmd.Parameters.AddWithValue("@StockOutID", StockOutID);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@WarehouseDetail", Warehousedt);

                cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 50);
                cmd.Parameters.Add("@ReturnID", SqlDbType.VarChar, 50);

                cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;
                cmd.Parameters["@ReturnID"].Direction = ParameterDirection.Output;

                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);

                strIsComplete = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value.ToString());
                strInvoiceID = Convert.ToInt32(cmd.Parameters["@ReturnID"].Value.ToString());
                
                cmd.Dispose();
                con.Dispose();
            }
            catch (Exception ex)
            {
            }
        }
        public DataTable GetReplacementOutDetails(string ReplacementStkID, string Action)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_ReplacementNoteOutList");
            proc.AddPara("@Action", Action);
            proc.AddPara("@ReplacementStkID", ReplacementStkID);
            dt = proc.GetTable();
            return dt;
        }
    }
}
