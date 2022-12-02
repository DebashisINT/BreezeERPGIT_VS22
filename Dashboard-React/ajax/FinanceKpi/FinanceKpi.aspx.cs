using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Dashboard_React.ajax.FinanceKpi
{
    public partial class FinanceKpi : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        [WebMethod]
        //for Activities
        public static object GetTopBoxData()
        {


            List<TopBoxDataClass> lEfficency = new List<TopBoxDataClass>();
            ProcedureExecute proc = new ProcedureExecute("PRC_FINANCIALKPI");
            proc.AddVarcharPara("@ACTION", 100, "BOXDATA");
            proc.AddVarcharPara("@USERID", 1000, Convert.ToString(HttpContext.Current.Session["userid"]));

            DataTable CallData = proc.GetTable();
            //var random = new Random();
            lEfficency = (from DataRow dr in CallData.Rows
                          select new TopBoxDataClass()
                          {
                              Reveneu = Convert.ToString(dr["Reveneu"]),
                              cogs = Convert.ToString(dr["cogs"]),
                              Expenses = Convert.ToString(dr["Expenses"]),
                              Gross_Profit = Convert.ToString(dr["Gross_Profit"]),
                              Gross_Profit_percent = Convert.ToString(dr["Gross_Profit_percent"]),
                              Other_income = Convert.ToString(dr["Other_income"]),
                              Other_Expenses = Convert.ToString(dr["Other_Expenses"]),
                              Nett_Profit = Convert.ToString(dr["Nett_Profit"]),
                              Nett_Profit_percent = Convert.ToString(dr["Nett_Profit_percent"])
                          }).ToList();
            return lEfficency;
        }

        [WebMethod]
        //for Activities
        public static object GetchartsData()
        {
            List<GetchartsData> lEfficency = new List<GetchartsData>();
            ProcedureExecute proc = new ProcedureExecute("PRC_FINANCIALKPI");
            proc.AddVarcharPara("@ACTION", 100, "CHARTDATA");
            proc.AddVarcharPara("@USERID", 1000, Convert.ToString(HttpContext.Current.Session["userid"]));

            DataTable CallData = proc.GetTable();
            //var random = new Random();
            lEfficency = (from DataRow dr in CallData.Rows
                          select new GetchartsData()
                          {
                              ID = Convert.ToString(dr["ID"]),
                              GROSS_PROFIT = Convert.ToString(dr["GROSS_PROFIT"]),
                              COGS = Convert.ToString(dr["COGS"]),
                              REVENEU = Convert.ToString(dr["REVENEU"]),
                              NETT_PROFIT = Convert.ToString(dr["NETT_PROFIT"]),
                              NETT_PROFIT_PERCENTAGE = Convert.ToString(dr["NETT_PROFIT_PERCENTAGE"]),
                              MONTHYEAR = Convert.ToString(dr["MONTHYEAR"])
                          }).ToList();
            return lEfficency;
        }

        [WebMethod]
        //for Activities
        public static object GetAssetsData()
        {
            List<GetAssetsData> lEfficency = new List<GetAssetsData>();
            ProcedureExecute proc = new ProcedureExecute("PRC_FINANCIALKPI");
            proc.AddVarcharPara("@ACTION", 100, "ASSETBOXDATA");
            proc.AddVarcharPara("@USERID", 1000, Convert.ToString(HttpContext.Current.Session["userid"]));

            DataTable CallData = proc.GetTable();
            //var random = new Random();
            lEfficency = (from DataRow dr in CallData.Rows
                          select new GetAssetsData()
                          {
                              Total_Asset = Convert.ToString(dr["Total_Asset"]),
                              Fixed_Asset = Convert.ToString(dr["Fixed_Asset"]),
                              Current_Asset = Convert.ToString(dr["Current_Asset"]),
                              Cashbank_Amount = Convert.ToString(dr["Cashbank_Amount"]),
                              Closing_Stock = Convert.ToString(dr["Closing_Stock"]),
                              Loan_Advance = Convert.ToString(dr["Loan_Advance"]),
                              Deposit = Convert.ToString(dr["Deposit"]),
                              LIABILITY = Convert.ToString(dr["LIABILITY"]),
                              CUR_LIABILITY = Convert.ToString(dr["CUR_LIABILITY"]),
                              NON_CUR_LIABILITY = Convert.ToString(dr["NON_CUR_LIABILITY"]),
                              EQUITY = Convert.ToString(dr["EQUITY"]),
                              PROVISION = Convert.ToString(dr["PROVISION"]),
                              RELATED_PARTY = Convert.ToString(dr["RELATED_PARTY"]),
                              TOTAL_REC = Convert.ToString(dr["TOTAL_REC"]),
                              TOTAL_PAY = Convert.ToString(dr["TOTAL_PAY"])

                          }).ToList();
            return lEfficency;
        }
    }

    public class GetAssetsData
    {
        public string Total_Asset { get; set; }
        public string Fixed_Asset { get; set; }
        public string Current_Asset { get; set; }
        public string Cashbank_Amount { get; set; }
        public string Closing_Stock { get; set; }
        public string Loan_Advance { get; set; }
        public string Deposit { get; set; }
        public string LIABILITY { get; set; }
        public string CUR_LIABILITY { get; set; }
        public string NON_CUR_LIABILITY { get; set; }
        public string EQUITY { get; set; }
        public string PROVISION { get; set; }
        public string RELATED_PARTY { get; set; }
        public string TOTAL_REC { get; set; }
        public string TOTAL_PAY { get; set; }

    }
    public class GetchartsData
    {
        public string ID { get; set; }
        public string GROSS_PROFIT { get; set; }
        public string COGS { get; set; }
        public string REVENEU { get; set; }
        public string NETT_PROFIT { get; set; }
        public string NETT_PROFIT_PERCENTAGE { get; set; }
        public string MONTHYEAR { get; set; }
    }
    public class TopBoxDataClass
    {
        public string Reveneu { get; set; }
        public string cogs { get; set; }
        public string Expenses { get; set; }
        public string Gross_Profit { get; set; }
        public string Gross_Profit_percent { get; set; }
        public string Other_income { get; set; }
        public string Other_Expenses { get; set; }
        public string Nett_Profit { get; set; }
        public string Nett_Profit_percent { get; set; }

    }
}