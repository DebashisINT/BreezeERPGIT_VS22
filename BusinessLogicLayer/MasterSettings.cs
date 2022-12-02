using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public  class MasterSettings
    {
        public  string GetSettings(string Key)
        {
            CommonBL cbl = new CommonBL();
            string LocalSeting = cbl.GetSystemSettingsResult(Key);

            if (!string.IsNullOrEmpty(LocalSeting))
            {
                if (LocalSeting.ToUpper() == "YES")
                    return "1";
                else
                    return "0";
            }
            else
            {

                DataTable dtOutput = new DataTable();
                string output = "";
                SqlConnection con = new SqlConnection(Convert.ToString(GetConnectionString()));
                SqlCommand cmd = new SqlCommand("PRC_ERP_SETTINGS", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Key", Key);
                SqlParameter outputpara = new SqlParameter("@output", SqlDbType.VarChar, 200);
                outputpara.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputpara);
                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dtOutput);
                cmd.Dispose();
                con.Dispose();
                output = Convert.ToString(cmd.Parameters["@output"].Value.ToString());
                return Convert.ToString(output);

            }

        }
        private  string GetConnectionString()
        {
            string Conn = "";
            string dbName = ConfigurationSettings.AppSettings["MasterDBName"];
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
