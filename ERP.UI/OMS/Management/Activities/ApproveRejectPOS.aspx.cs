using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Activities
{
    public partial class ApproveRejectPOS : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            string InvoiceId = "";
            string DBName = "";
            string Status = "";
            string ApproveBy = "";

            if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString["key"])))
                InvoiceId = Convert.ToString(Request.QueryString["key"]);

            if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString["dbname"])))
                DBName = Convert.ToString(Request.QueryString["dbname"]);

            if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString["Status"])))
                Status = Convert.ToString(Request.QueryString["Status"]);

            if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString["ApproveBy"])))
                ApproveBy = Convert.ToString(Request.QueryString["ApproveBy"]);

            string msg = "";
            string stat = "";
            DataTable dt = GetDataTableWithoutSession("select case when isApproved=1 THEN 'App' WHEN isRejected=1 THEN 'Rej'  else 'Pen' END Stat from  tbl_trans_SalesInvoice where Invoice_Id='" + InvoiceId + "'", DBName);
            if (dt != null && dt.Columns.Count > 0)
            {
                stat = Convert.ToString(dt.Rows[0][0]);
            }

            if (stat == "App")
            {
                msg = "Already Approved.";
            }
            else if (stat == "Rej")
            {
                msg = "Already Rejected.";
            }
            else
            {               

                if (Status == "1")
                {
                    var dtCmb = GetDataTableWithoutSession("UPDATE tbl_trans_SalesInvoice set isApproved=1,ApprovedRejected_By='" + ApproveBy + "',ApprovedRejected_On=GETDATE() where Invoice_Id='" + InvoiceId + "'", DBName);
                    msg = "Approved Successfully.";
                }
                else
                {
                    var dtCmb = GetDataTableWithoutSession("UPDATE tbl_trans_SalesInvoice set isRejected=1,ApprovedRejected_By='" + ApproveBy + "',ApprovedRejected_On=GETDATE() where Invoice_Id='" + InvoiceId + "'", DBName);
                    msg = "Rejected Successfully.";

                }
            }
            ClientScript.RegisterStartupScript(Page.GetType(), "ClosePage", "alert('" + msg + "'); window.close();", true);
        }

        private DataTable GetDataTableWithoutSession(string lcSql, string DBname)
        {
            string masterDbanem = DBname;
            string oSql = Convert.ToString(GetConnectionString(masterDbanem));
            SqlConnection oSqlConnection = new SqlConnection(oSql);
            SqlDataAdapter lda = new SqlDataAdapter(lcSql, oSqlConnection);
            DataTable GetTable = new DataTable();
            lda.SelectCommand.CommandTimeout = 0;
            lda.Fill(GetTable);
            oSqlConnection.Close();
            return GetTable;


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
            connectionString.ConnectTimeout = 950;
            connectionString.Pooling = Convert.ToBoolean(ispool);
            connectionString.MaxPoolSize = Convert.ToInt32(poolsize);



            string str = connectionString.ConnectionString;



            return str;
        }

    }
}