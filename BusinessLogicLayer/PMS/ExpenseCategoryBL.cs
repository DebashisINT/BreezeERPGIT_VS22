using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BusinessLogicLayer.PMS
{
    public class ExpenseCategoryBL
    {
        public DataSet DropDownDetailForExpe()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("PRC_EXPENSECATEGORYLOAD");
            ds = proc.GetDataSet();
            return ds;
        }

        public string SaveExpenseData(string ExpenseID, string Expense_Name, string Expense_Type, string TransactionCategory, string ReceiptReq, string BRANCH)
        {
            try
            {
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_ExpenseCategory_ADDEDIT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ExpenseID", ExpenseID);
                cmd.Parameters.AddWithValue("@Expense_Name", Expense_Name);
                cmd.Parameters.AddWithValue("@Expense_Type", Expense_Type);
                cmd.Parameters.AddWithValue("@TransactionCategory", TransactionCategory);
                cmd.Parameters.AddWithValue("@ReceiptReq", ReceiptReq);
                cmd.Parameters.AddWithValue("@BRANCH", BRANCH);
                cmd.Parameters.AddWithValue("@Create_By", Convert.ToInt32(HttpContext.Current.Session["userid"]));
                cmd.Parameters.AddWithValue("@Update_BY", Convert.ToInt32(HttpContext.Current.Session["userid"]));
                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                con.Dispose();

                return Convert.ToString("Data save");
            }
            catch (Exception ex)
            {
                return "Please try again later.";
            }
        }

        public DataTable GetExpenseList()
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_EXPENSECATEGORYLIST");
            ds = proc.GetTable();
            return ds;
        }

        public DataTable ViewExpense(string ExpenseID)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_ExpenseCategory_VIEW");
            proc.AddNVarcharPara("@ExpenseID", 10, ExpenseID);
            ds = proc.GetTable();
            return ds;
        }

        public int DeleteExpense(string ExpenseID)
        {
            int ret = 0;
            ProcedureExecute proc = new ProcedureExecute("PRC_ExpenseCategory_DELETE");
            proc.AddNVarcharPara("@ExpenseID", 10, ExpenseID);
            ret = proc.RunActionQuery();
            return ret;
        }

    }
}
