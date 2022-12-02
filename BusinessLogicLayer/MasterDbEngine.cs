using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BusinessLogicLayer
{
    public class MasterDbEngine
    {
        string strAppConnection = String.Empty;
        ProcedureExecute proc = new ProcedureExecute();
        // Internal reference to the Connection that is created.
        SqlConnection oSqlConnection = new SqlConnection();



        private string list_of_ids;     //______This is in use of message delete____//

        SQLProcedures oSqlProcedures = new SQLProcedures();
        GenericMethod oGenericMethod = null;

        public void GetConnection()
        {
            if (oSqlConnection.State.Equals(ConnectionState.Open))
            {
            }
            else
            {
                string masterdbname = Convert.ToString(ConfigurationSettings.AppSettings["MasterDBName"]);
                oSqlConnection.ConnectionString = GetConnectionString(masterdbname);
            }
        }

     
        public void CloseConnection()
        {
            if (oSqlConnection.State.Equals(ConnectionState.Open))
            {
                oSqlConnection.Close();
            }
        }

        public SqlDataReader GetReader(String cSql)
        {
            // Now we create a Command object and execute the command.

            GetConnection();
            SqlDataReader lsdr;

            SqlCommand lcmd = new SqlCommand(cSql, oSqlConnection);

            lsdr = lcmd.ExecuteReader();
            return lsdr;
        }
       
        #region Data Table creation

        //-------------------------------------------------------------------------------------------//
        // This will returm a DATATABLE frompassed parameter
        // Example Usage:
        // GetDataTable("tbl_trans_menu", "mnu_id, mnu_menuName, mnu_menuLink, mun_parentId, mnu_segmentId, mnu_image", null);
        //-------------------------------------------------------------------------------------------//
        public DataTable GetDataTable(
                            String cTableName,     // TableName from which the field value is to be fetched
                            String cFieldName,     // The name if the field whose value needs to be returned
                            String cWhereClause)   // WHERE condition [if any]
        {
            // Now we construct a SQL command that will fetch fields from the Suplied table

            String lcSql;
           lcSql = "Select " + cFieldName + " from " + cTableName;
            if (cWhereClause != null)
            {
                lcSql += " WHERE " + cWhereClause;
            }
            GetConnection();
            SqlDataAdapter lda = new SqlDataAdapter(lcSql, oSqlConnection);
            // createing an object of datatable
            DataTable GetTable = new DataTable();
            lda.SelectCommand.CommandTimeout = 0;
            lda.Fill(GetTable);
            oSqlConnection.Close();
            return GetTable;
        }

        //////////////////////////11/02/2012 developed by Subhadeep/////////

        public DataTable GetDataTable(
                           String query)    // TableName from which the field value is to be fetched
        // The name if the field whose value needs to be returned
        // WHERE condition [if any]
        {
            // Now we construct a SQL command that will fetch fields from the Suplied table

            String lcSql;
            lcSql = query;

            //SqlConnection lcon = GetConnection();
            GetConnection();
            SqlDataAdapter lda = new SqlDataAdapter(lcSql, oSqlConnection);
            // createing an object of datatable
            DataTable getquery = new DataTable();
            lda.SelectCommand.CommandTimeout = 0;
            lda.Fill(getquery);
            oSqlConnection.Close();
            return getquery;
        }

        //////////////////////11/02/2012 developed/////////////////////////////
        public DataTable GetDataTable(
                            String cTableName,     // TableName from which the field value is to be fetched
                            String cFieldName,     // The name if the field whose value needs to be returned
                            String cWhereClause,   // WHERE condition [if any]
                            string cOrderBy)       // Order by condition
        {
            // Now we construct a SQL command that will fetch fields from the Suplied table

            String lcSql;
            lcSql = "Select " + cFieldName + " from " + cTableName;
            if (cWhereClause != null)
            {
                lcSql += " WHERE " + cWhereClause;
            }
            if (cOrderBy != null)
            {
                lcSql += " Order By " + cOrderBy;
            }

            GetConnection();
            SqlDataAdapter lda = new SqlDataAdapter(lcSql, oSqlConnection);
            // createing an object of datatable
            DataTable GetTable = new DataTable();
            lda.SelectCommand.CommandTimeout = 0;
            lda.Fill(GetTable);
            oSqlConnection.Close();
            return GetTable;
        }

        public DataTable GetDataTableGroup(
                            String cTableName,     // TableName from which the field value is to be fetched
                            String cFieldName,     // The name if the field whose value needs to be returned
                            String cWhereClause,   // WHERE condition [if any]
                            string groupBy)       // Group by condition
        {
            // Now we construct a SQL command that will fetch fields from the Suplied table

            String lcSql;
            lcSql = "Select " + cFieldName + " from " + cTableName;
            if (cWhereClause != null)
            {
                lcSql += " WHERE " + cWhereClause;
            }
            if (groupBy != null)
            {
                lcSql += " group By " + groupBy;
            }

            GetConnection();
            SqlDataAdapter lda = new SqlDataAdapter(lcSql, oSqlConnection);
            // createing an object of datatable
            DataTable GetTable = new DataTable();
            lda.SelectCommand.CommandTimeout = 0;
            lda.Fill(GetTable);
            oSqlConnection.Close();
            return GetTable;
        }

        public DataTable GetDataTable(
                            String cTableName,     // TableName from which the field value is to be fetched
                            String cFieldName,     // The name if the field whose value needs to be returned
                            String cWhereClause,   // WHERE condition [if any]
                            string groupBy,         // Gropu by condition
                            string cOrderBy)        // Order by condition
        {
            // Now we construct a SQL command that will fetch fields from the Suplied table

            String lcSql;
            lcSql = "Select " + cFieldName + " from " + cTableName;
            if (cWhereClause != null)
            {
                lcSql += " WHERE " + cWhereClause;
            }
            if (groupBy != null)
            {
                lcSql += " group By " + groupBy;
            }
            if (cOrderBy != null)
            {
                lcSql += " Order By " + cOrderBy;
            }
            GetConnection();
            SqlDataAdapter lda = new SqlDataAdapter(lcSql, oSqlConnection);
            // createing an object of datatable
            DataTable GetTable = new DataTable();
            lda.SelectCommand.CommandTimeout = 0;
            lda.Fill(GetTable);
            oSqlConnection.Close();
            return GetTable;
        }

        #endregion Data Table creation

        public string ExeSclar(string query)
        {

            GetConnection();

            SqlCommand cmd = new SqlCommand(query, oSqlConnection);

            cmd.CommandText = query;
            string retval = (string)cmd.ExecuteScalar();



            return retval;
        }

        public int ExeInteger(string query)
        {

            GetConnection();
            SqlCommand cmd = new SqlCommand(query, oSqlConnection);
            cmd.CommandText = query;
            int countDis = Convert.ToInt32(cmd.ExecuteScalar());
            return countDis;
        }

        private string GetConnectionString(string dbName)
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
            connectionString.Pooling = Convert.ToBoolean(ispool);
            connectionString.MaxPoolSize = Convert.ToInt32(poolsize);



            string str = connectionString.ConnectionString;



            return str;
        }


       
     
    }
}
