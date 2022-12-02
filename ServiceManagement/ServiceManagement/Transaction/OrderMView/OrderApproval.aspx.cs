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

namespace ServiceManagement.ServiceManagement.Transaction.OrderMView
{
    public partial class OrderApproval : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["UniqueKey"] != null)
            {
                string masterDbanem = Request.QueryString["UniqueKey"];
                Session["ErpConnection"] = GetDefaultConnectionString(masterDbanem);
            }
        }

        public string GetDefaultConnectionString(string masterDbanem)
        {
            string output = "";
            try
            {
                output = GetConnectionString(Convert.ToString(masterDbanem));
            }

            catch (Exception es)
            {
                throw es;
            }
            return output;
        }

        public string GetConnectionString(string dbName)
        {
            string Conn = "";
            string DtSource = ConfigurationSettings.AppSettings["sqlDatasource"];
            string UserId = ConfigurationSettings.AppSettings["sqlUserId"];
            string Pwd = ConfigurationSettings.AppSettings["sqlPassword"];
            string IntSq = ConfigurationSettings.AppSettings["sqlAuth"];
            string ispool = ConfigurationSettings.AppSettings["isPool"];
            string poolsize = ConfigurationSettings.AppSettings["PoolSize"];


            SqlConnectionStringBuilder connectionString = new SqlConnectionStringBuilder();
            connectionString.DataSource = DtSource;
            connectionString.InitialCatalog = dbName;
            if (IntSq == "Windows")
            {
                connectionString.IntegratedSecurity = true;
            }
            else
            {
                connectionString.PersistSecurityInfo = true;
                connectionString.IntegratedSecurity = false;
                connectionString.UserID = UserId;
                connectionString.Password = Pwd;

            }
            connectionString.ConnectTimeout = 0;
            connectionString.Pooling = Convert.ToBoolean(ispool);
            connectionString.MaxPoolSize = Convert.ToInt32(poolsize);



            string str = connectionString.ConnectionString;
            return str;
        }

        [WebMethod]
        //for Activities
        public static object GetData(string Order_Id, string dbname)
        {

            DataTable dt = new DataTable();
            List<TopBoxDataClass> lEfficency = new List<TopBoxDataClass>();

            string oSql = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlConnection oSqlConnection = new SqlConnection(oSql);


            oSqlConnection.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd = new SqlCommand("prc_PurchaseOrderDetailsList", oSqlConnection);
            sqlcmd.Parameters.Add("@ACTION", "PurchaseOrderHeader");
            sqlcmd.Parameters.Add("@PurchaseOrder_Id", Order_Id);

            sqlcmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
            da.Fill(dt);
            oSqlConnection.Close();




            DataTable CallData = dt;
            //var random = new Random();
            lEfficency = (from DataRow dr in CallData.Rows
                          select new TopBoxDataClass()
                          {
                              PurchaseOrder_Id = Convert.ToString(dr["PurchaseOrder_Id"]),
                              PurchaseOrder_Number = Convert.ToString(dr["PurchaseOrder_Number"]),
                              PurchaseOrder_Date = Convert.ToString(dr["PurchaseOrder_Date"]),
                              Indent_Num = Convert.ToString(dr["Indent_Num"]),
                              branch_description = Convert.ToString(dr["branch_description"]),
                              ProjectPurchase_ApproveStatus = Convert.ToString(dr["ProjectPurchase_ApproveStatus"]),
                              ProjectPurchase_ApprovalRemarks = Convert.ToString(dr["ProjectPurchase_ApprovalRemarks"])
                          }).ToList();
            return lEfficency;
        }
        [WebMethod]
        //for Activities
        public static object GetListData(string Order_Id)
        {
            DataTable dt = new DataTable();
            List<TabbleAction> lEfficency = new List<TabbleAction>();

            string oSql = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlConnection oSqlConnection = new SqlConnection(oSql);


            oSqlConnection.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd = new SqlCommand("prc_PurchaseOrderDetailsList", oSqlConnection);
            sqlcmd.Parameters.Add("@ACTION", "PurchaseOrdDetails");
            sqlcmd.Parameters.Add("@PurchaseOrder_Id", Order_Id);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
            da.Fill(dt);
            oSqlConnection.Close();
            DataTable CallData = dt;
            lEfficency = (from DataRow dr in CallData.Rows
                          select new TabbleAction()
                          {

                              OrderDetailsId = Convert.ToString(dr["OrderDetails_Id"]),
                              Description = Convert.ToString(dr["Description"]),
                              Quantity = Convert.ToString(dr["Quantity"]),
                              UOM = Convert.ToString(dr["UOM"])
                              //Rate = Convert.ToString(dr["Rate"]),
                              //ValueInBaseCurrency = Convert.ToString(dr["ValueInBaseCurrency"]),
                              //ExpectedDeliveryDate = Convert.ToString(dr["ExpectedDeliveryDate"]),
                              //Status = Convert.ToString(dr["Status"]),
                              //AvailableStock = Convert.ToString(dr["AvailableStock"]),
                              //ProductName = Convert.ToString(dr["ProductName"]),
                              //Remarks = Convert.ToString(dr["Remarks"]),
                              //ServiceTempDetails_ID = Convert.ToString(dr["ServiceTempDetails_ID"]),
                              //ServiceTemplate_ID = Convert.ToString(dr["ServiceTemplate_ID"]),
                              //Order_AltQuantity = Convert.ToString(dr["Order_AltQuantity"]),
                              //Order_AltUOM = Convert.ToString(dr["Order_AltUOM"]),
                              //BalanceQty = Convert.ToString(dr["BalanceQty"])
                          }).ToList();
            return lEfficency;
        }

        [WebMethod]
        //public static string JobStatusUpdate(String AppRej, String ReceiptChallan_ID, String[] myTableArray)
        public static object JobStatusUpdate(String AppRej, String Order_ApprovalRemarks, String[][] udtOrderDetailsAction, String Order_Id)
        //public JsonResult JobStatusUpdate(String AppRej, String ReceiptChallan_ID, String[][] myTableArray)
        {
            string output = string.Empty;
            DataTable dtable = new DataTable();
            int OrderDetailsId = 0;
            decimal Quantity = 0;
            try
            {
                DataTable dt = new DataTable();
                if (udtOrderDetailsAction[0][0] != null)
                {
                    dtable.Clear();
                    dtable.Columns.Add("OrderDetailsId", typeof(System.String));
                    dtable.Columns.Add("Quantity", typeof(System.String));
                }
                for (int i = 0; i < udtOrderDetailsAction.Length; i++)
                {
                    OrderDetailsId = Convert.ToInt32(udtOrderDetailsAction[i][0]);

                    if (udtOrderDetailsAction[i][2] == "")
                    {
                        udtOrderDetailsAction[i][2] = "0.00";
                    }
                    Quantity = Convert.ToDecimal(udtOrderDetailsAction[i][2]);
                    dtable.Rows.Add(OrderDetailsId, Quantity);
                }


                string oSql = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                SqlConnection oSqlConnection = new SqlConnection(oSql);


                oSqlConnection.Open();
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd = new SqlCommand("prc_PurchaseOrderDetailsList", oSqlConnection);
                sqlcmd.Parameters.Add("@ACTION", "SaveApproveReject");
                sqlcmd.Parameters.Add("@Order_ApprovalRemarks", Convert.ToString(Order_ApprovalRemarks));
                sqlcmd.Parameters.Add("@AppRej", Convert.ToString(AppRej));
                sqlcmd.Parameters.Add("@udtOrderDetailsAction", dtable);
                sqlcmd.Parameters.Add("@POID", Order_Id);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                da.Fill(dt);
                oSqlConnection.Close();
                output = "Success";
                //End of rev Pratik
            }
            catch (Exception ex)
            {
                output = ex.Message.ToString();
            }
            return output;
            //return Json(output, JsonRequestBehavior);
        }

    }
    public class TopBoxDataClass
    {
        public string PurchaseOrder_Id { get; set; }
        public string PurchaseOrder_Number { get; set; }
        public string PurchaseOrder_Date { get; set; }
        public string Indent_Num { get; set; }
        public string ApprovalSettings { get; set; }
        public string branch_description { get; set; }
        public string ProjectPurchase_ApproveStatus { get; set; }
        public string ProjectPurchase_ApprovalRemarks { get; set; }

    }
    public class TabbleAction
    {

        public string OrderDetailsId { get; set; }
        public string Description { get; set; }
        public string Quantity { get; set; }
        public string UOM { get; set; }
        public string Rate { get; set; }
        public string ValueInBaseCurrency { get; set; }
        public string ExpectedDeliveryDate { get; set; }
        public string Status { get; set; }
        public string AvailableStock { get; set; }
        public string ProductName { get; set; }
        public string Remarks { get; set; }
        public string ServiceTempDetails_ID { get; set; }
        public string ServiceTemplate_ID { get; set; }
        public string Order_AltQuantity { get; set; }
        public string Order_AltUOM { get; set; }
        public string BalanceQty { get; set; }
    }

    public class DetailsStatus
    {
        public string ReceiptChallan_ID { get; set; }
        public string Status { get; set; }
    }
}