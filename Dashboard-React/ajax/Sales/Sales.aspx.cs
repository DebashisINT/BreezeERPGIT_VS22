using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Dashboard_React.ajax.Sales
{
    public partial class Sales : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        [WebMethod]
        public static object LoadTotalSale(string FromDtae, string toDate, string branchid, string ProdClass, string Prodid)
        {

            ValueWithGraph valueWithGraph = new ValueWithGraph();
            List<KeyvalueAxis> lKeyAxis = new List<KeyvalueAxis>();


            ProcedureExecute proc = new ProcedureExecute("Prc_salesDb");
            proc.AddPara("@action", "GetTotalSale");
            proc.AddPara("@fromdate", FromDtae);
            proc.AddPara("@toDate", toDate);
            proc.AddPara("@BranchId", branchid);
            proc.AddPara("@ProdClass", ProdClass);
            proc.AddPara("@Prodid", Prodid);
            DataSet TotSaleDs = proc.GetDataSet();

            valueWithGraph.totValue = Convert.ToString(TotSaleDs.Tables[0].Rows[0][0]);


            lKeyAxis = (from DataRow dr in TotSaleDs.Tables[1].Rows
                        select new KeyvalueAxis()
                        {
                            xAxis = Convert.ToString(dr["Amount"]),
                            yAxis = Convert.ToString(dr["date"])
                        }).ToList();

            valueWithGraph.AxisValue = lKeyAxis;
            return valueWithGraph;
        }


        [WebMethod]
        public static object LoadTotalDue(string FromDtae, string toDate, string branchid, string ProdClass, string Prodid)
        {

            ValueWithGraph valueWithGraph = new ValueWithGraph();
            List<KeyvalueAxis> lKeyAxis = new List<KeyvalueAxis>();


            ProcedureExecute proc = new ProcedureExecute("Prc_salesDb");
            proc.AddPara("@action", "GetTotalDue");
            proc.AddPara("@fromdate", FromDtae);
            proc.AddPara("@toDate", toDate);
            proc.AddPara("@BranchId", branchid);
            proc.AddPara("@ProdClass", ProdClass);
            proc.AddPara("@Prodid", Prodid);
            DataSet TotSaleDs = proc.GetDataSet();

            valueWithGraph.totValue = Convert.ToString(TotSaleDs.Tables[0].Rows[0][0]);


            lKeyAxis = (from DataRow dr in TotSaleDs.Tables[1].Rows
                        select new KeyvalueAxis()
                        {
                            xAxis = Convert.ToString(dr["Amount"]),
                            yAxis = Convert.ToString(dr["date"])
                        }).ToList();

            valueWithGraph.AxisValue = lKeyAxis;
            return valueWithGraph;
        }


        [WebMethod]
        public static object LoadTotalOrder(string FromDtae, string toDate, string branchid, string ProdClass, string Prodid)
        {

            ValueForOrder valueWithGraph = new ValueForOrder();
            List<OrderValue> lKeyAxis = new List<OrderValue>();


            ProcedureExecute proc = new ProcedureExecute("Prc_salesDb");
            proc.AddPara("@action", "GetOrderDetails");
            proc.AddPara("@fromdate", FromDtae);
            proc.AddPara("@toDate", toDate);
            proc.AddPara("@BranchId", branchid);
            proc.AddPara("@ProdClass", ProdClass);
            proc.AddPara("@Prodid", Prodid);
            DataSet TotSaleDs = proc.GetDataSet();

            valueWithGraph.totValue = Convert.ToString(TotSaleDs.Tables[0].Rows[0][0]);


            lKeyAxis = (from DataRow dr in TotSaleDs.Tables[1].Rows
                        select new OrderValue()
                        {
                            Date = Convert.ToString(dr["date"]),
                            OrderAmt = Convert.ToString(dr["OrdAmount"]),
                            InvoiceAmt = Convert.ToString(dr["InvAmount"])
                        }).ToList();

            valueWithGraph.AxisValue = lKeyAxis;
            return valueWithGraph;
        }



        [WebMethod]
        public static object LoadTotalReceipt(string FromDtae, string toDate, string branchid, string ProdClass, string Prodid)
        {

            ValueWithGraph valueWithGraph = new ValueWithGraph();
            List<KeyvalueAxis> lKeyAxis = new List<KeyvalueAxis>();


            ProcedureExecute proc = new ProcedureExecute("Prc_salesDb");
            proc.AddPara("@action", "GetAdvanceReceived");
            proc.AddPara("@fromdate", FromDtae);
            proc.AddPara("@toDate", toDate);
            proc.AddPara("@BranchId", branchid);
            proc.AddPara("@ProdClass", ProdClass);
            proc.AddPara("@Prodid", Prodid);
            DataSet TotSaleDs = proc.GetDataSet();

            valueWithGraph.totValue = Convert.ToString(TotSaleDs.Tables[0].Rows[0][0]);


            lKeyAxis = (from DataRow dr in TotSaleDs.Tables[1].Rows
                        select new KeyvalueAxis()
                        {
                            xAxis = Convert.ToString(dr["Amount"]),
                            yAxis = Convert.ToString(dr["date"])
                        }).ToList();

            valueWithGraph.AxisValue = lKeyAxis;
            return valueWithGraph;
        }



        [WebMethod]
        public static object LoadnewCust(string FromDtae, string toDate, string branchid, string ProdClass, string Prodid)
        {

            ValueWithGraph valueWithGraph = new ValueWithGraph();
            List<KeyvalueAxis> lKeyAxis = new List<KeyvalueAxis>();


            ProcedureExecute proc = new ProcedureExecute("Prc_salesDb");
            proc.AddPara("@action", "GetNewCustomerCount");
            proc.AddPara("@fromdate", FromDtae);
            proc.AddPara("@toDate", toDate);
            proc.AddPara("@BranchId", branchid);
            proc.AddPara("@ProdClass", ProdClass);
            proc.AddPara("@Prodid", Prodid);
            DataSet TotSaleDs = proc.GetDataSet();

            valueWithGraph.totValue = Convert.ToString(TotSaleDs.Tables[0].Rows[0][0]);


            lKeyAxis = (from DataRow dr in TotSaleDs.Tables[1].Rows
                        select new KeyvalueAxis()
                        {
                            xAxis = Convert.ToString(dr["cnt"]),
                            yAxis = Convert.ToString(dr["ondate"])
                        }).ToList();

            valueWithGraph.AxisValue = lKeyAxis;
            return valueWithGraph;
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
        public static object GetTopNCustomer(string FromDtae, string toDate, string branchid, string ProdClass, string Prodid)
        {
            List<NameValue> CustList = new List<NameValue>();
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand com = new SqlCommand("Prc_salesDb", con);
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
        public class NameValue
        {
            public string Name { get; set; }
            public string AmtVal { get; set; }

        }
        public class ValueWithGraph
        {
            public string totValue { get; set; }
            public List<KeyvalueAxis> AxisValue { get; set; }
        }

        public class KeyvalueAxis
        {
            public string xAxis { get; set; }
            public string yAxis { get; set; }

        }


        public class ValueForOrder
        {
            public string totValue { get; set; }
            public List<OrderValue> AxisValue { get; set; }
        }

        public class OrderValue
        {
            public string Date { get; set; }
            public string OrderAmt { get; set; }
            public string InvoiceAmt { get; set; }

        }
    }
}