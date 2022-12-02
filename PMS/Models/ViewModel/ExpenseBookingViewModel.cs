using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PMS.Models.ViewModel
{
    public class ExpenseBookingViewModel
    {
        public String ExpenseBooking_Id { get; set; }
        public String Action_type { get; set; }
        public String Contact { get; set; }
        public String resourceName { get; set; }
        public String Projects { get; set; }
        public String Branch { get; set; }
        public String Currency { get; set; }
        public String ExpenseCategory { get; set; }
        public String ExpensePurpose { get; set; }
        public String Basis { get; set; }
        public String BranchID { get; set; }

        public string response_code { get; set; }
        public string response_msg { get; set; }

        [RegularExpression(@"^(((\d{1,3})(,\d{3})*)|(\d+))(.\d+)?$", ErrorMessage = "The Value format must be 00.00")]
        public String Quantity { get; set; }
        [RegularExpression(@"^(((\d{1,3})(,\d{3})*)|(\d+))(.\d+)?$", ErrorMessage = "The Value format must be 00.00")]
        public String Price { get; set; }
        [RegularExpression(@"^(((\d{1,3})(,\d{3})*)|(\d+))(.\d+)?$", ErrorMessage = "The Value format must be 00.00")]
        public String Amount { get; set; }
        [RegularExpression(@"^(((\d{1,3})(,\d{3})*)|(\d+))(.\d+)?$", ErrorMessage = "The Value format must be 00.00")]
        public String SalesTaxAmount { get; set; }
        public String ExternalComments { get; set; }
        public string TransactionDate { get; set; }
        public List<Units> UnitsList { get; set; }
        public List<Projects> ProjectsList { get; set; }
        public List<Currency> CurrencyList { get; set; }
        public List<ExpenseCategory> ExpenseCategoryList { get; set; }

        public DateTime Transaction_Date { get; set; }
    }

    public class Currency
    {
        public String Currency_id { get; set; }
        public String Currency_Name { get; set; }
    }

    public class ExpenseCategory
    {
        public String ExpenseCategory_id { get; set; }
        public String ExpenseCategory_Name { get; set; }
    }

    public class ExpenseBookingView
    {
        public long ExpenseBooking_Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public string ExpensePurpose { get; set; }
        public string Basis { get; set; }
        public string Quantity { get; set; }
        public string Price { get; set; }
        public string Amount { get; set; }
        public string SalesTaxAmount { get; set; }
        public string ExternalComments { get; set; }
        public string branch_description { get; set; }
        public string Proj_Name { get; set; }
        public string Expense_Name { get; set; }
        public string Currency_Name { get; set; }
        public string CREATE_USER { get; set; }
        public DateTime CREATED_ON { get; set; }
        public string UPDATE_USER { get; set; }
        public DateTime? UPDATED_ON { get; set; }
    }
}