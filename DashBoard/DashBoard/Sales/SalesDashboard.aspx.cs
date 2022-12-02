using BusinessLogicLayer;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DashBoard.DashBoard.Sales
{
    public partial class SalesDashboard : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        protected void Page_Load(object sender, EventArgs e)
        {
            FormDate.Date = DateTime.Now;
            toDate.Date = DateTime.Now;

            if (!IsPostBack)
            {
                PopulateBranchByHierchy(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
                //var A = Session["LocalCurrency"].ToString().Split('~')[1].Trim();

                if (Session["LocalCurrency"] != null)
                {
                    lblCurrencySymbol.InnerHtml = Session["LocalCurrency"].ToString().Split('~')[1].Trim();
                    lblCurrencySymbol2.InnerHtml = Session["LocalCurrency"].ToString().Split('~')[1].Trim();
                    lblCurrencySymbol3.InnerHtml = Session["LocalCurrency"].ToString().Split('~')[1].Trim();
                    lblCurrencySymbol4.InnerHtml = Session["LocalCurrency"].ToString().Split('~')[1].Trim();
                }


                DataTable dt = oDBEngine.GetDataTable(@"select TotSaleDue,TotSale ,TotAdvRcv ,TotOrder ,
                                            TopNSalesman ,TopNCustomer,NewCust from tbl_master_dashboard_setting_details 
                                            where user_id=" + Convert.ToString(Session["userid"]));
                if (dt.Rows.Count > 0)
                {
                    TotDuebtn.Visible = Convert.ToBoolean(dt.Rows[0]["TotSaleDue"]);
                    TotSalebtn.Visible = Convert.ToBoolean(dt.Rows[0]["TotSale"]);
                    TotReceiptbtn.Visible = Convert.ToBoolean(dt.Rows[0]["TotAdvRcv"]);
                    TotOrdbtn.Visible = Convert.ToBoolean(dt.Rows[0]["TotOrder"]);
                    top10Smanbtn.Visible = Convert.ToBoolean(dt.Rows[0]["TopNSalesman"]);
                    top10Custbtn.Visible = Convert.ToBoolean(dt.Rows[0]["TopNCustomer"]);
                    newCustbtn.Visible = Convert.ToBoolean(dt.Rows[0]["NewCust"]);

                }

            }
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