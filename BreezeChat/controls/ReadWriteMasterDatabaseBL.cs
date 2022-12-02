using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;


namespace BreezeChat.controls
{
    public class ReadWriteMasterDatabaseBL
    {
        DBEngine db = new DBEngine();
        public string WriteNewCompanyToMasterDatabase(string companyName, string dbName, DateTime YearStartDate, DateTime CutoffDate)
        {
            try
            {
                string masterDbanem = ConfigurationSettings.AppSettings["MasterDBName"];

                DataTable dtCon = CompanyRead();

                int maxColumn = Convert.ToInt32(dtCon.Compute("max([id])", string.Empty));

                maxColumn = maxColumn + 1;

                int i = db.ExeInteger("insert into " + masterDbanem + ".dbo.ERP_Company_List(Id,Name,DbName,IsDefault,Fin_Start,Fin_End) values('" + maxColumn + "','" + companyName + "','" + dbName + "',0,'" + YearStartDate + "','" + CutoffDate + "')");

                return i.ToString();
            }

            catch (Exception e)
            {
                throw e;
            }
        }
        public DataTable CompanyRead()
        {
            try
            {
                string masterDbanem = ConfigurationSettings.AppSettings["MasterDBName"];
                DataTable dt = GetDataTable("select cast(Id as Bigint) Id,Name+'   [ '+ Convert(varchar(10),Fin_Start,105) + ' - ' + Convert(varchar(10),Fin_End,105) + ' ]' Name,DbName,IsDefault from " + masterDbanem + ".dbo.ERP_Company_List");
                return dt;
            }
            catch (Exception es)
            {
                throw es;
            }
        }
        public DataTable CompanyRead(string LoginId)
        {
            try
            {
                string masterDbanem = ConfigurationSettings.AppSettings["MasterDBName"];

                string str = "select cast(ECL.Id as Bigint) Id,Name+'    [ '+ Convert(varchar(10),Fin_Start,105) + ' - ' + Convert(varchar(10),Fin_End,105) + ' ]' Name,DbName,IsDefault from " + masterDbanem + ".dbo.ERP_Company_List ECL inner join " + masterDbanem + ".dbo.User_Company_Map UCM on ECL.Id=UCM.CompanyId where LoginId='" + Convert.ToString(LoginId) + "'";

                DataTable dt = GetDataTable(str);

                //if (dt == null || dt.Rows.Count == 0)
                //{
                //    dt = CompanyRead();
                //}


                return dt;
            }
            catch (Exception es)
            {
                throw es;
            }
        }




        public int GetDataRowCount()
        {
            DataTable dtCount = CompanyRead();
            return dtCount.Rows.Count;
        }

        public string GetConnectionStringById(int id)
        {
            string output = "";
            try
            {
                string masterDbanem = ConfigurationSettings.AppSettings["MasterDBName"];
                DataTable dt = GetDataTable("select DbName from " + masterDbanem + ".dbo.ERP_Company_List  where id='" + Convert.ToString(id) + "'");

                output = GetConnectionString(Convert.ToString(dt.Rows[0]["dbName"].ToString()));

            }

            catch (Exception es)
            {
                throw es;
            }
            return output;
        }


        public string GetDefaultConnectionString()
        {
            string output = "";
            try
            {


                string masterDbanem = ConfigurationSettings.AppSettings["MasterDBName"];
                System.Web.HttpContext.Current.Session["ErpConnection"] = GetConnectionString(masterDbanem);

                DataTable dt = GetDataTable("select DbName from " + masterDbanem + ".dbo.ERP_Company_List where isdefault=1");

                output = GetConnectionString(Convert.ToString(dt.Rows[0]["dbName"].ToString()));

            }

            catch (Exception es)
            {
                throw es;
            }
            return output;
        }


        public string GetDefaultConnectionStringWithoutSession()
        {
            string output = "";
            try
            {


                string masterDbanem = ConfigurationSettings.AppSettings["MasterDBName"];

                DataTable dt = GetDataTableWithoutSession("select DbName from " + masterDbanem + ".dbo.ERP_Company_List where isdefault=1");

                output = GetConnectionString(Convert.ToString(dt.Rows[0]["dbName"].ToString()));

            }

            catch (Exception es)
            {
                throw es;
            }
            return output;
        }

        private DataTable GetDataTableWithoutSession(string lcSql)
        {
            string masterDbanem = ConfigurationSettings.AppSettings["MasterDBName"];
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

            //string connectionString="Data Source=LABSERVER\MSSQLSERVERR2;Initial Catalog=PK24092018;Persist Security Info=True;User ID=sa;Password=sql@123";


            //if (IntSq != "Windows")
            //{
            //    Conn = "Data Source=" + DtSource + ";Initial Catalog=" + dbName + ";Persist Security Info=True;User ID=" + UserId + ";Password=" + Pwd + ";pooling=" + ispool + ";Max Pool Size=" + poolsize;
            //}
            //else
            //{
            //    Conn = "Data Source=" + DtSource + ";Initial Catalog=" + dbName + ";Integrated Security=SSPI" + ";pooling=" + ispool + ";Max Pool Size=" + poolsize;
            //}


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


        private DataTable GetDataTable(string lcSql)
        {
            string oSql = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlConnection oSqlConnection = new SqlConnection(oSql);
            SqlDataAdapter lda = new SqlDataAdapter(lcSql, oSqlConnection);
            DataTable GetTable = new DataTable();
            lda.SelectCommand.CommandTimeout = 0;
            lda.Fill(GetTable);
            oSqlConnection.Close();
            return GetTable;
        }
    }
}