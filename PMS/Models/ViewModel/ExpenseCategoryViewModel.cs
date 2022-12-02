using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMS.Models.ViewModel
{
    public class ExpenseCategoryViewModel
    {
        public string ExpenseID { get; set; }
        public string Expense_Name { get; set; }
        public string Expense_Type { get; set; }
        public string TransactionCategory { get; set; }
        public string ReceiptReq { get; set; }
        public string BRANCH { get; set; }
        public string CREATE_BY { get; set; }
        public string CREATE_DATE { get; set; }
        public string UPDATE_BY { get; set; }
        public string UPDATE_DATE { get; set; }
        public List<Units> BranchList { get; set; }
        public List<ExpenseTypes> ExpenseTypeList { get; set; }
        public List<TransactionCategorys> TransactionCategoryList { get; set; }
        public IList<ReceiptReqs> ReceiptReqList { get; set; }
    }

    public class ExpenseTypes
    {
        public string Expense_TypeID { get; set; }
        public string Expense_Type_Name { get; set; }
    }

    public class TransactionCategorys
    {
        public string TRANS_ID { get; set; }
        public string TRANS_NAME { get; set; }
    }

    public class ReceiptReqs
    {
        public string ReceiptRequiredID { get; set; }
        public string ReceiptRequiredName { get; set; }
    }

    public class ExpenseCategoryList
    {
        public long ExpenseID { get; set; }
        public string Expense_Name { get; set; }
        public long Expense_Type { get; set; }
        public long TransactionCategory { get; set; }
        public long ReceiptReq { get; set; }
        public long BRANCH { get; set; }
        public long Create_By { get; set; }
        public DateTime? Create_Date { get; set; }
        public long Update_BY { get; set; }
        public DateTime? Update_Date { get; set; }
        public string BRANCH_NAME { get; set; }
        public string CREATE_NAME { get; set; }
        public string UPDATE_NAME { get; set; }
        public string ReceiptRequiredName { get; set; }
        public string Expense_Type_Name { get; set; }
        public string TRANS_NAME { get; set; }
    }
}