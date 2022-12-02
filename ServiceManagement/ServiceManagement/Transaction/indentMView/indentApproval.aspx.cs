using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLogicLayer;
using DataAccessLayer;
using System.Data;
using System.Linq;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.Mvc;

namespace ServiceManagement.ServiceManagement.Transaction.indentMView
{
    public partial class indentApproval : System.Web.UI.Page
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
        public static object GetData(string Indent_Id, string dbname)
        {

            DataTable dt = new DataTable();
            List<TopBoxDataClass> lEfficency = new List<TopBoxDataClass>();

            string oSql = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlConnection oSqlConnection = new SqlConnection(oSql);


            oSqlConnection.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd = new SqlCommand("prc_PurchaseIndentDetailsList", oSqlConnection);
            sqlcmd.Parameters.Add("@ACTION", "PurchaseIndentHeader");
            sqlcmd.Parameters.Add("@Indent_Id", Indent_Id);
            
            sqlcmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
            da.Fill(dt);
            oSqlConnection.Close();




            DataTable CallData = dt;
            //var random = new Random();
            lEfficency = (from DataRow dr in CallData.Rows
                          select new TopBoxDataClass()
                          {
                              Indent_Id = Convert.ToString(dr["Indent_Id"]),
                              Indent_RequisitionNumber = Convert.ToString(dr["Indent_RequisitionNumber"]),
                              Indent_RequisitionDateTimeFormat = Convert.ToString(dr["Indent_RequisitionDateTimeFormat"]),
                              Indent_Purpose = Convert.ToString(dr["Indent_Purpose"]),
                              ApprovalSettings = Convert.ToString(dr["ApprovalSettings"]),
                              Indent_branch = Convert.ToString(dr["Indent_branch"]),
                              PurchaseIndent_ApproveStatus = Convert.ToString(dr["PurchaseIndent_ApproveStatus"])
                          }).ToList();
            return lEfficency;
        }
        [WebMethod]
        //for Activities
        public static object GetListData(string Indent_Id)
        {
            DataTable dt = new DataTable();
            List<TabbleAction> lEfficency = new List<TabbleAction>();

            string oSql = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlConnection oSqlConnection = new SqlConnection(oSql);


            oSqlConnection.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd = new SqlCommand("prc_PurchaseIndentDetailsList", oSqlConnection);
            sqlcmd.Parameters.Add("@ACTION", "PurchaseIndentDetails");
            sqlcmd.Parameters.Add("@Indent_Id", Indent_Id);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
            da.Fill(dt);
            oSqlConnection.Close();
            DataTable CallData = dt;
            lEfficency = (from DataRow dr in CallData.Rows
                          select new TabbleAction()
                          {
                              
                              IndentDetailsId = Convert.ToString(dr["IndentDetailsId"]),
                              Description = Convert.ToString(dr["Description"]),
                              Quantity = Convert.ToString(dr["Quantity"]),
                              UOM = Convert.ToString(dr["UOM"]),
                              Rate = Convert.ToString(dr["Rate"]),
                              ValueInBaseCurrency = Convert.ToString(dr["ValueInBaseCurrency"]),
                              ExpectedDeliveryDate = Convert.ToString(dr["ExpectedDeliveryDate"]),
                              Status = Convert.ToString(dr["Status"]),
                              AvailableStock = Convert.ToString(dr["AvailableStock"]),
                              ProductName = Convert.ToString(dr["ProductName"]),
                              Remarks = Convert.ToString(dr["Remarks"]),
                              ServiceTempDetails_ID = Convert.ToString(dr["ServiceTempDetails_ID"]),
                              ServiceTemplate_ID = Convert.ToString(dr["ServiceTemplate_ID"]),
                              Order_AltQuantity = Convert.ToString(dr["Order_AltQuantity"]),
                              Order_AltUOM = Convert.ToString(dr["Order_AltUOM"]),
                              BalanceQty = Convert.ToString(dr["BalanceQty"])
                          }).ToList();
            return lEfficency;
        }

        [WebMethod]
        //public static string JobStatusUpdate(String AppRej, String ReceiptChallan_ID, String[] myTableArray)
        public static object JobStatusUpdate(String AppRej, String Indent_ApprovalRemarks, String[][] udtIndentDetailsAction, String Indent_Id)
        //public JsonResult JobStatusUpdate(String AppRej, String ReceiptChallan_ID, String[][] myTableArray)
        {
            string output = string.Empty;
            DataTable dtable = new DataTable();
            int IndentDetailsId = 0;
            decimal Quantity = 0;
            try
            {
                DataTable dt = new DataTable();
                if (udtIndentDetailsAction[0][0] != null)
                {
                    dtable.Clear();
                    dtable.Columns.Add("IndentDetailsId", typeof(System.String));
                    dtable.Columns.Add("Quantity", typeof(System.String));
                }
                for (int i = 0; i < udtIndentDetailsAction.Length; i++)
                {
                    IndentDetailsId = Convert.ToInt32(udtIndentDetailsAction[i][0]);

                    if (udtIndentDetailsAction[i][2] == "")
                    {
                        udtIndentDetailsAction[i][2] = "0.00";
                    }
                    Quantity = Convert.ToDecimal(udtIndentDetailsAction[i][2]);
                    dtable.Rows.Add(IndentDetailsId, Quantity);
                }
               

                string oSql = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                SqlConnection oSqlConnection = new SqlConnection(oSql);


                oSqlConnection.Open();
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd = new SqlCommand("prc_PurchaseIndentDetailsList", oSqlConnection);
                sqlcmd.Parameters.Add("@ACTION", "SaveApproveReject");
                sqlcmd.Parameters.Add("@Indent_ApprovalRemarks", Convert.ToString(Indent_ApprovalRemarks));
                sqlcmd.Parameters.Add("@AppRej", Convert.ToString(AppRej));
                sqlcmd.Parameters.Add("@udtIndentDetailsAction", dtable);
                sqlcmd.Parameters.Add("@Indent_Id", Indent_Id);
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
        public string Indent_Id { get; set; }
        public string Indent_RequisitionNumber { get; set; }
        public string Indent_RequisitionDateTimeFormat { get; set; }
        public string Indent_Purpose { get; set; }
        public string ApprovalSettings { get; set; }
        public string Indent_branch { get; set; }
        public string PurchaseIndent_ApproveStatus { get; set; }
       
    }
    public class TabbleAction
    {
        
        public string IndentDetailsId	{ get; set; }
        public string Description	{ get; set; }
        public string Quantity	{ get; set; }
        public string UOM	{ get; set; }
        public string     Rate	{ get; set; }
        public string ValueInBaseCurrency	{ get; set; }
        public string ExpectedDeliveryDate	{ get; set; }
        public string Status	{ get; set; }
        public string AvailableStock	{ get; set; }
        public string ProductName	{ get; set; }
        public string Remarks	{ get; set; }
        public string ServiceTempDetails_ID	{ get; set; }
        public string ServiceTemplate_ID	{ get; set; }
        public string Order_AltQuantity	{ get; set; }
        public string Order_AltUOM	{ get; set; }
        public string BalanceQty { get; set; }
    }

    public class DetailsStatus
    {
        public string ReceiptChallan_ID { get; set; }
        public string Status { get; set; }
    }
}