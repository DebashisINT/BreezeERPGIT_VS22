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

namespace ServiceManagement.ServiceManagement.Transaction
{
    public partial class TechnicianAssign : System.Web.UI.Page
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
        public static object GetData(string ReceiptChallan_ID, string technician_Id, string dbname)
        {
            
            DataTable dt = new DataTable();
            List<TopBoxDataClass> lEfficency = new List<TopBoxDataClass>();

            string oSql = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlConnection oSqlConnection = new SqlConnection(oSql);

            
            oSqlConnection.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd = new SqlCommand("PRC_RepairPendingDetails", oSqlConnection);
            sqlcmd.Parameters.Add("@Action", "Details");
            sqlcmd.Parameters.Add("@Technician_Id", technician_Id);
            sqlcmd.Parameters.Add("@ReceiptChallan_ID", ReceiptChallan_ID);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
            da.Fill(dt);
            oSqlConnection.Close();


            

            DataTable CallData = dt;
            //var random = new Random();
            lEfficency = (from DataRow dr in CallData.Rows
                          select new TopBoxDataClass()
                          {
                              RepPendingDetails_ID = Convert.ToString(dr["RepPendingDetails_ID"]),
                              SerialNo = Convert.ToString(dr["SerialNo"]),
                              Problem = Convert.ToString(dr["Problem"]),
                              ServiceAction = Convert.ToString(dr["ServiceAction"]),
                              RepPending_ID = Convert.ToString(dr["RepPending_ID"]),
                              ReceiptChallan_ID = Convert.ToString(dr["ReceiptChallan_ID"]),
                              ChallanNo = Convert.ToString(dr["ChallanNo"]),
                              AssignedOn = Convert.ToString(dr["AssignedOn"]),
                              cnt_firstName = Convert.ToString(dr["cnt_firstName"]),
                              ProblemDesc = Convert.ToString(dr["ProblemDesc"])
                          }).ToList();
            return lEfficency;
        }
        [WebMethod]
        //for Activities
        public static object GetListData(string ReceiptChallan_ID, string technician_Id)
        {
            DataTable dt = new DataTable();
            List<TabbleAction> lEfficency = new List<TabbleAction>();

            string oSql = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlConnection oSqlConnection = new SqlConnection(oSql);


            oSqlConnection.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd = new SqlCommand("PRC_RepairPendingDetails", oSqlConnection);
            sqlcmd.Parameters.Add("@Action", "ServiceAction");
            sqlcmd.Parameters.Add("@Technician_Id", technician_Id);
            sqlcmd.Parameters.Add("@ReceiptChallan_ID", ReceiptChallan_ID);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
            da.Fill(dt);
            oSqlConnection.Close();
            DataTable CallData = dt;
            lEfficency = (from DataRow dr in CallData.Rows
                          select new TabbleAction()
                          {
                              ServiceMaster_ID = Convert.ToString(dr["ServiceMaster_ID"]),
                              RepairingServiceDesc = Convert.ToString(dr["RepairingServiceDesc"]),
                              
                          }).ToList();
            return lEfficency;
        }

        [WebMethod]
        //public static string JobStatusUpdate(String AppRej, String ReceiptChallan_ID, String[] myTableArray)
        public static object JobStatusUpdate(String AppRej, String ReceiptChallan_ID, String[][] myTableArray, String user_id)
        //public JsonResult JobStatusUpdate(String AppRej, String ReceiptChallan_ID, String[][] myTableArray)
        {
            string output = string.Empty;
            DataTable dtable = new DataTable();
            int RepPendingDetails_ID = 0;
            int ServiceAction = 0;
            try
            {
                DataTable dt = new DataTable();
                if (myTableArray[0][0] != null)
                {
                    dtable.Clear();
                    dtable.Columns.Add("RepPendingDetails_ID", typeof(System.String));
                    dtable.Columns.Add("ServiceAction", typeof(System.String));
                }
                for (int i = 0; i < myTableArray.Length; i++)
                {
                    RepPendingDetails_ID = Convert.ToInt32(myTableArray[i][0]);

                    if (myTableArray[i][3]==""){
                        myTableArray[i][3] = "0";
                    }
                    ServiceAction = Convert.ToInt32(myTableArray[i][3]);
                    dtable.Rows.Add(RepPendingDetails_ID, ServiceAction);
                }
                //rev Pratik
                //int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                //End of rev Pratik
                //ProcedureExecute proc = new ProcedureExecute("PRC_RepairPendingDetails");
                //proc.AddVarcharPara("@ACTION", 500, "UpdateStatus");
                //proc.AddPara("@ReceiptChallan_ID", Convert.ToString(ReceiptChallan_ID));
                //proc.AddPara("@AppRej", Convert.ToString(AppRej));
                //proc.AddPara("@udtServiceAction", dtable);
                //proc.AddPara("@USER_ID", user_id);
                //dt = proc.GetTable();
                //output = "Success";

                string oSql = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                SqlConnection oSqlConnection = new SqlConnection(oSql);


                oSqlConnection.Open();
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd = new SqlCommand("PRC_RepairPendingDetails", oSqlConnection);
                sqlcmd.Parameters.Add("@ACTION", "UpdateStatus");
                sqlcmd.Parameters.Add("@ReceiptChallan_ID", Convert.ToString(ReceiptChallan_ID));
                sqlcmd.Parameters.Add("@AppRej", Convert.ToString(AppRej));
                sqlcmd.Parameters.Add("@udtServiceAction", dtable);
                sqlcmd.Parameters.Add("@USER_ID", user_id);
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

        [WebMethod]
        public static object GetResponseStatus(string ReceiptChallan_ID, string technician_Id, string dbname)
        {

            DataTable dt = new DataTable();
            List<DetailsStatus> lEfficency = new List<DetailsStatus>();

            string oSql = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlConnection oSqlConnection = new SqlConnection(oSql);


            oSqlConnection.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd = new SqlCommand("PRC_RepairPendingDetails", oSqlConnection);
            sqlcmd.Parameters.Add("@Action", "StatusDetails");
            sqlcmd.Parameters.Add("@Technician_Id", technician_Id);
            sqlcmd.Parameters.Add("@ReceiptChallan_ID", ReceiptChallan_ID);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
            da.Fill(dt);
            oSqlConnection.Close();

            DataTable CallData = dt;
            var sts = "";
            //var random = new Random();
            //lEfficency = (from DataRow dr in CallData.Rows
            //              select new DetailsStatus()
            //              {
            //                  ReceiptChallan_ID = Convert.ToString(dr["ReceiptChallan_ID"]),
            //                  Status = Convert.ToString(dr["Status"])
            //              }).ToList();
            foreach (DataRow item in dt.Rows)
            {
                sts = item["Status"].ToString();
            }
            return sts;
        }

    }



    public class TopBoxDataClass
    {
        public string RepPendingDetails_ID	{ get; set; }
        public string SerialNo{ get; set; }	
        public string Problem	{ get; set; }
        public string ServiceAction	{ get; set; }
        public string RepPending_ID { get; set; }	
        public string ReceiptChallan_ID	{ get; set; }
        public string ChallanNo	{ get; set; }
        public string AssignedOn	{ get; set; }
        public string cnt_firstName { get; set; }

        public string ProblemDesc { get; set; }
    } 
    public class TabbleAction
    {
        public string ServiceMaster_ID { get; set; }
        public string RepairingServiceDesc	{ get; set; }
    }

    public class DetailsStatus
    {
        public string ReceiptChallan_ID { get; set; }
        public string Status { get; set; }
    }
}
