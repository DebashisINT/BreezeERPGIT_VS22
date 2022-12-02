using BusinessLogicLayer;
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

namespace DashBoard.DashBoard.Purchase
{
    public partial class PurchaseDb : System.Web.UI.Page
    {
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["crmConnectionString"]);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        protected void Page_Load(object sender, EventArgs e)
        {
            
            FormDate.Date = DateTime.Now;
            toDate.Date = DateTime.Now;
            if (!IsPostBack)
            {
                PopulateBranchByHierchy(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));

                DataTable dt = oDBEngine.GetDataTable(@"select TotPurchase,TotDue,TotPayment,
                                            TotReturn,TopNItemByPurchase,TopNVendor 
                                            from tbl_master_dashboard_setting_details 
                                            where user_id=" + Convert.ToString(Session["userid"]));
                if (dt.Rows.Count > 0)
                {
                    TotPurchase.Visible = Convert.ToBoolean(dt.Rows[0]["TotPurchase"]);
                    TotDue.Visible = Convert.ToBoolean(dt.Rows[0]["TotDue"]);
                    TotPayment.Visible = Convert.ToBoolean(dt.Rows[0]["TotPayment"]);
                    TotReturn.Visible = Convert.ToBoolean(dt.Rows[0]["TotReturn"]);
                    TopNItemByPurchase.Visible = Convert.ToBoolean(dt.Rows[0]["TopNItemByPurchase"]);
                    TopNVendor.Visible = Convert.ToBoolean(dt.Rows[0]["TopNVendor"]);

                }

            } 
                                              
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

        private void PopulateBranchByHierchy(string userbranchhierchy)
        {
            PosSalesInvoiceBl posSale = new PosSalesInvoiceBl();
            DataTable branchtable = posSale.getBranchListByHierchy(userbranchhierchy);
            ddlBranch.DataSource = branchtable;
            ddlBranch.ValueField = "branch_id";
            ddlBranch.TextField = "branch_description";
            ddlBranch.DataBind();
            ddlBranch.SelectedIndex = 0;
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