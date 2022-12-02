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
    public partial class BranchRequisitionView : System.Web.UI.Page
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
            sqlcmd.Parameters.Add("@ACTION", "BranchRequisitionHeader");
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
                              Indent_RequisitionDate = Convert.ToString(dr["Indent_RequisitionDate"]),
                              Indent_Purpose = Convert.ToString(dr["Indent_Purpose"]),
                              Proj_Name = Convert.ToString(dr["Proj_Name"]),
                              branch_description = Convert.ToString(dr["branch_description"]),
                              // Mantis Issue 25237
                              BranchRequisition_ApproveStatus = Convert.ToString(dr["ApprovalStatus"])
                               // End of Mantis Issue 25237
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
            sqlcmd.Parameters.Add("@ACTION", "BranchRequisitionDetails");
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
                              Remarks = Convert.ToString(dr["Remarks"])
                          }).ToList();
            return lEfficency;
        }
        // Mantis Issue 25237
        [WebMethod]
        public static object JobStatusUpdate(String AppRej, String[][] udtIndentDetailsAction, String Indent_Id)
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
                sqlcmd.Parameters.Add("@ACTION", "SaveBranchReqApproveReject");
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
        // End of Mantis Issue 25237
        
    }
    public class TopBoxDataClass
    {
        public string Indent_Id { get; set; }
        public string Indent_RequisitionNumber { get; set; }
        public string Indent_RequisitionDate { get; set; }
        public string Indent_Purpose { get; set; }
        public string ApprovalSettings { get; set; }
        public string branch_description { get; set; }
        public string Proj_Name { get; set; }
        // Mantis Issue 25237
        public string BranchRequisition_ApproveStatus { get; set; }
        // End of Mantis Issue 25237
    }
    public class TabbleAction
    {

        public string IndentDetailsId { get; set; }
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
    }
}