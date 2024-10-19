using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace ServiceManagement.Controllers
{
    public class DecryptShortURLController : Controller
    {
        public ActionResult Index(int id)
        {
            string LongURL = "";
            string ky = "GTPL";

            Session["ErpConnection"] = GetDefaultConnectionString(ky);

            string oSql = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlConnection oSqlConnection = new SqlConnection(oSql);

            DataTable dt = new DataTable();

            oSqlConnection.Open();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd = new SqlCommand("PRC_AssignJobDetails", oSqlConnection);
            sqlcmd.Parameters.Add("@Action", "GetLongURL");
            sqlcmd.Parameters.Add("@URLid", id);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
            da.Fill(dt);
            oSqlConnection.Close();

            if (dt.Rows.Count > 0)
            {
                LongURL = dt.Rows[0]["LONG_URL"].ToString();
            }

            return Redirect(LongURL);
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

    }
}