using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DashBoard.DashBoard.Sales
{
    public partial class SalesDb : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        protected void Page_Load(object sender, EventArgs e)
        {

            //New Changes
            
            FormDate.Date = DateTime.Now;
            toDate.Date = DateTime.Now;

            if (!IsPostBack)
            {
                DataTable dt = oDBEngine.GetDataTable(@"select TotSaleDue,TotSale ,TotAdvRcv ,TotOrder ,
                                            TopNSalesman ,TopNCustomer from tbl_master_dashboard_setting_details 
                                            where user_id="+ Convert.ToString(Session["userid"]));
                if (dt.Rows.Count > 0)
                {
                    TotSaleDue.Visible = Convert.ToBoolean(dt.Rows[0]["TotSaleDue"]);
                    TotSale.Visible = Convert.ToBoolean(dt.Rows[0]["TotSale"]);
                    TotAdvRcv.Visible = Convert.ToBoolean(dt.Rows[0]["TotAdvRcv"]);
                    TotOrder.Visible = Convert.ToBoolean(dt.Rows[0]["TotOrder"]);
                    TopNSalesman.Visible = Convert.ToBoolean(dt.Rows[0]["TopNSalesman"]);
                    TopNCustomer.Visible = Convert.ToBoolean(dt.Rows[0]["TopNCustomer"]); 
                
                }
            
            }

                                              
        }

        [WebMethod]
        public static object GetSalesBalance(string FromDtae, string toDate, string branchid, string ProdClass, string Prodid)
        {
            SalesBalance salesBalance = new SalesBalance();
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand com = new SqlCommand("Prc_salesDb", con);
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
                salesBalance.TotalSale = dr.GetDecimal(0).ToString();
                salesBalance.TotDue = dr.GetDecimal(1).ToString();
            }
            dr.NextResult();
            if (dr.Read())
            {
                salesBalance.totAdvance = dr.GetDecimal(0).ToString();
            }
            dr.NextResult();
            if (dr.Read())
            {
                salesBalance.totOrder = dr.GetDecimal(0).ToString();
            }
            con.Close();
            return salesBalance;
        }


        [WebMethod]
        public static object GetTopNSalesMan(string FromDtae, string toDate, string branchid, string ProdClass, string Prodid)
        {
            List<NameValue> SalesManList = new List<NameValue>();
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand com = new SqlCommand("Prc_salesDb", con);
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
            SalesManList = (from DataRow dr in dt.Rows
                            select new NameValue()
                        {
                            Name = dr["Name"].ToString(),
                            AmtVal = dr["Amount"].ToString()
                        }).ToList();


            return SalesManList;
        }

        [WebMethod]
        public static object GetTopNCustomer(string FromDtae, string toDate, string branchid, string Prodid)
        {
            List<NameValue> CustList = new List<NameValue>();
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand com = new SqlCommand("Prc_salesDb", con);
            com.CommandType = System.Data.CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@action", "GetTopNCustomer");
            com.Parameters.AddWithValue("@fromdate", FromDtae);
            com.Parameters.AddWithValue("@toDate", toDate);
            com.Parameters.AddWithValue("@BranchId", branchid);
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


        public class SalesBalance 
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