using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Dashboard_React.ajax.purchase
{
    public partial class purchaseDb : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        [WebMethod]
        public static object GetPurchaseBalance(string FromDtae, string toDate, string branchid, string ProdClass, string Prodid)
        {
            PurchaseBalance PurchaseBalance = new PurchaseBalance();
            //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["crmConnectionString"].ToString());
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand com = new SqlCommand("Prc_PurchaseDb", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@action", "getHeaderDetail");
            com.Parameters.AddWithValue("@fromdate", FromDtae);
            com.Parameters.AddWithValue("@toDate", toDate);
            com.Parameters.AddWithValue("@BranchId", branchid);
            com.Parameters.AddWithValue("@ProdClass", ProdClass);
            com.Parameters.AddWithValue("@Prodid", Prodid);
            con.Open();
            SqlDataReader dr = com.ExecuteReader();
            if (dr.Read())
            {
                PurchaseBalance.TotalSale = dr.GetDecimal(0).ToString();
                PurchaseBalance.TotDue = dr.GetDecimal(1).ToString();
            }
            dr.NextResult();
            if (dr.Read())
            {
                PurchaseBalance.totAdvance = dr.GetDecimal(0).ToString();
            }
            dr.NextResult();
            if (dr.Read())
            {
                PurchaseBalance.totOrder = dr.GetDecimal(0).ToString();
            }
            con.Close();
            return PurchaseBalance;
        }


        [WebMethod]
        public static object GetTopNPurchaseMan(string FromDtae, string toDate, string branchid, string ProdClass, string Prodid)
        {
            List<NameValue> PurchaseManList = new List<NameValue>();
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand com = new SqlCommand("Prc_PurchaseDb", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@action", "getTopNSaleman");
            com.Parameters.AddWithValue("@fromdate", FromDtae);
            com.Parameters.AddWithValue("@toDate", toDate);
            com.Parameters.AddWithValue("@BranchId", branchid);
            com.Parameters.AddWithValue("@ProdClass", ProdClass);
            com.Parameters.AddWithValue("@Prodid", Prodid);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(com);
            da.Fill(dt);
            PurchaseManList = (from DataRow dr in dt.Rows
                               select new NameValue()
                               {
                                   Name = dr["Name"].ToString(),
                                   AmtVal = dr["Amount"].ToString()
                               }).ToList();


            return PurchaseManList;
        }

        [WebMethod]
        public static object GetTopNCustomer(string FromDtae, string toDate, string branchid, string ProdClass, string Prodid)
        {
            List<NameValue> CustList = new List<NameValue>();
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand com = new SqlCommand("Prc_PurchaseDb", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@action", "GetTopNCustomer");
            com.Parameters.AddWithValue("@fromdate", FromDtae);
            com.Parameters.AddWithValue("@toDate", toDate);
            com.Parameters.AddWithValue("@BranchId", branchid);
            com.Parameters.AddWithValue("@ProdClass", ProdClass);
            com.Parameters.AddWithValue("@Prodid", Prodid);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(com);
            da.Fill(dt);
            CustList = (from DataRow dr in dt.Rows
                        select new NameValue()
                        {
                            Name = dr["Name"].ToString(),
                            AmtVal = dr["Amount"].ToString()
                        }).ToList();


            return CustList;
        }


        public class PurchaseBalance
        {
            public string TotalSale { get; set; }
            public string TotDue { get; set; }
            public string totAdvance { get; set; }
            public string totOrder { get; set; }

        }

        public class NameValue
        {
            public string Name { get; set; }
            public string AmtVal { get; set; }

        }
    }
}