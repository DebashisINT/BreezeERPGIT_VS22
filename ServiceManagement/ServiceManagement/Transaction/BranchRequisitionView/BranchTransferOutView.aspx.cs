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

namespace ServiceManagement.ServiceManagement.Transaction.BranchRequisitionView
{
    public partial class BranchTransferOutView : System.Web.UI.Page
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
        public static object GetData(string TransferOut_Id, string dbname)
        {

            DataTable dt = new DataTable();
            List<TransferOutHeaderData> lHeaderData = new List<TransferOutHeaderData>();

            string oSql = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlConnection oSqlConnection = new SqlConnection(oSql);


            oSqlConnection.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd = new SqlCommand("prc_CRMBranchStockOut", oSqlConnection);
            sqlcmd.Parameters.Add("@ACTION", "BranchTransferOutHeader");
            sqlcmd.Parameters.Add("@BranchTransferOutId", TransferOut_Id);

            sqlcmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
            da.Fill(dt);
            oSqlConnection.Close();




            DataTable CallData = dt;
            //var random = new Random();
            lHeaderData = (from DataRow dr in CallData.Rows
                          select new TransferOutHeaderData()
                          {
                              Stk_Id = Convert.ToString(dr["Stk_Id"]),
                              Stk_TransferNumber = Convert.ToString(dr["Stk_TransferNumber"]),
                              Stk_TransferDate = Convert.ToString(dr["Stk_TransferDate"]),
                              Stk_Purpose = Convert.ToString(dr["Stk_Purpose"]),
                              Proj_Name = Convert.ToString(dr["Proj_Name"]),
                              Stk_TransferToBranch = Convert.ToString(dr["Stk_TransferToBranch"]),
                              Indent_RequisitionNumber = Convert.ToString(dr["Indent_RequisitionNumber"])
                             
                          }).ToList();
            return lHeaderData;
        }
        [WebMethod]
        //for Activities
        public static object GetListData(string TransferOut_Id)
        {
            DataTable dt = new DataTable();
            List<TransferOutDetailData> lDetailData = new List<TransferOutDetailData>();

            string oSql = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlConnection oSqlConnection = new SqlConnection(oSql);


            oSqlConnection.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd = new SqlCommand("prc_CRMBranchStockOut", oSqlConnection);
            sqlcmd.Parameters.Add("@ACTION", "BranchTransferOutDetails");
            sqlcmd.Parameters.Add("@BranchTransferOutId", TransferOut_Id);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
            da.Fill(dt);
            oSqlConnection.Close();
            DataTable CallData = dt;
            lDetailData = (from DataRow dr in CallData.Rows
                          select new TransferOutDetailData()
                          {
                              StkDetails_Id = Convert.ToString(dr["StkDetails_Id"]),
                              ProductDescription = Convert.ToString(dr["ProductDescription"]),
                              Quantity = Convert.ToString(dr["Quantity"]),
                              UOM = Convert.ToString(dr["UOM"])
                          }).ToList();
            return lDetailData;
        }
    }

    public class TransferOutHeaderData
    {
        public string Stk_Id { get; set; }
        public string Stk_TransferNumber { get; set; }
        public string Stk_TransferDate { get; set; }
        public string Stk_Purpose { get; set; }
        public string Proj_Name { get; set; }
        public string Stk_TransferToBranch { get; set; }
        public string Indent_RequisitionNumber { get; set; }
    }
    public class TransferOutDetailData
    {
        public string StkDetails_Id { get; set; }
        public string ProductDescription { get; set; }
        public string Quantity { get; set; }
        public string UOM { get; set; }
    }
}