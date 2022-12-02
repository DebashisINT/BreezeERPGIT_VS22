using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PMS.Models
{
    public class ExpenseBookingModel
    {
        public DataSet DropDownDetailForExpenseBooking()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("PRC_ExpenseBooking_Details");
            proc.AddNVarcharPara("@Action", 100, "GetAllLoadDetais");
            proc.AddVarcharPara("@BranchList", 1000, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet DropDownDetailForProject(String Branchid)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("PRC_ExpenseBooking_Details");
            proc.AddNVarcharPara("@Action", 100, "GetProjectDetais");
            proc.AddVarcharPara("@BranchID", 100, Branchid);
            ds = proc.GetDataSet();
            return ds;
        }

        public void AddEditExpenseBooking(string Action_type, string TransactionDate,string BranchID, string Projects, string ExpensePurpose, string ExpenseCategory,
                    string Basis, string Quantity, string Price, string Currency,string Amount, string SalesTaxAmount,string ExternalComments,
                    string ExpenseBooking_Id, ref int ReturnCode, ref string ReturnMsg)
        {
            if (HttpContext.Current.Session["userid"] != null)
            {
                int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);

                ProcedureExecute proc = new ProcedureExecute("PRC_ExpenseBooking_InsertUpdate");
                proc.AddPara("@ACTION", Action_type);
                proc.AddPara("@ExpenseBooking_Id", ExpenseBooking_Id);
                proc.AddPara("@TransactionDate", Convert.ToString(TransactionDate));
                proc.AddPara("@Branch_Id", BranchID);
                proc.AddPara("@Project_Id", Projects);
                proc.AddPara("@ExpensePurpose", ExpensePurpose);
                proc.AddPara("@ExpenseCategory_Id",ExpenseCategory);
                proc.AddPara("@Basis", Basis);
                proc.AddPara("@Quantity", Quantity);
                proc.AddPara("@Price", Price);
                proc.AddPara("@Currency", Currency);
                proc.AddPara("@Amount", Amount);
                proc.AddPara("@SalesTaxAmount", SalesTaxAmount);
                proc.AddPara("@ExternalComments", ExternalComments);
                proc.AddPara("@User_id", Convert.ToString(user_id));
                proc.AddPara("@FinYear", HttpContext.Current.Session["LastFinYear"].ToString());
                proc.AddPara("@CompanyID", HttpContext.Current.Session["LastCompany"].ToString());

                proc.AddNVarcharPara("@ReturnCode", 10, Convert.ToString(ReturnCode), QueryParameterDirection.Output);
                proc.AddNVarcharPara("@ReturnMsg",100, ReturnMsg, QueryParameterDirection.Output);

                proc.RunActionQuery();

                ReturnCode = Convert.ToInt32(proc.GetParaValue("@ReturnCode"));
                ReturnMsg = Convert.ToString(proc.GetParaValue("@ReturnMsg"));
            }
        }

        public DataTable GetExpenseBookingList(string BranchID, DateTime FromDate, DateTime ToDate)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_PMSExpenseBooking_VIEW");
            proc.AddPara("@BranchID", BranchID);
            proc.AddPara("@FROM_DATE", FromDate);
            proc.AddPara("@TO_DATE", ToDate);
            proc.AddPara("@USER_ID", Convert.ToString(HttpContext.Current.Session["userid"]));
            ds = proc.GetTable();
            return ds;
        }

        public DataTable ViewExpenseBooking(string ExpenseBooking_id)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_ExpenseBooking_InsertUpdate");
            proc.AddPara("@ACTION", "ViewExpenseBooking");
            proc.AddNVarcharPara("@ExpenseBooking_Id", 10, ExpenseBooking_id);
            ds = proc.GetTable();
            return ds;
        }

        public void DeleteExpenseBooking(string ExpenseBooking_id, ref string ReturnMsg)
        {
            int ReturnCode = 0;
            int ret = 0;
            ProcedureExecute proc = new ProcedureExecute("PRC_ExpenseBooking_InsertUpdate");
            proc.AddPara("@ACTION", "DeleteExpenseBooking");
            proc.AddNVarcharPara("@ExpenseBooking_Id", 10, ExpenseBooking_id);
            proc.AddNVarcharPara("@ReturnMsg", 500, ReturnMsg, QueryParameterDirection.Output);
            proc.AddNVarcharPara("@ReturnCode", 10, Convert.ToString(ReturnCode), QueryParameterDirection.Output);
            ret = proc.RunActionQuery();
            //proc.RunActionQuery();
            ReturnMsg = Convert.ToString(proc.GetParaValue("@ReturnMsg"));
            ReturnCode = Convert.ToInt32(proc.GetParaValue("@ReturnCode"));
            // return ret;
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
    }
}