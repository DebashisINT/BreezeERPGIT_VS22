using DataAccessLayer;
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

namespace ERP.OMS.Management.Master
{
    public partial class view_version_features : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public static string addversionfeatures(string VersionNumber, string FeaturesMarkup, string flag)
        {

            try
            {
                DataTable dt = GerVersionFeatures(VersionNumber, FeaturesMarkup, flag);
                if (dt.Rows.Count > 0)
                {
                    return EncodeToString((byte[])(dt.Rows[0]["CurrentDBVersion_WhatsNew"]));
                }
                else
                {
                    return "0";
                }


                      
            }
            catch (Exception ex)
            {
                return "Error occured";
            }
        }
        public static string EncodeToString(byte[] bytes)
        {
            bool even = (bytes.Length % 2 == 0);
            char[] chars = new char[1 + bytes.Length / sizeof(char) + (even ? 0 : 1)];
            System.Buffer.BlockCopy(bytes, 0, chars, 2, bytes.Length);

            return new string(chars);
        }
        public static DataTable GerVersionFeatures(string VersionNumber, string FeaturesMarkup, string flag)
        {
            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;MULTI
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand myCMD = new SqlCommand("sp_Version_Features", con))
                {
                    myCMD.Parameters.Add("@Action", SqlDbType.VarChar).Value = flag;
                    myCMD.Parameters.Add("@CurrentDBVersion_Number", SqlDbType.VarChar).Value = VersionNumber;
                    myCMD.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter(myCMD);
                    da.Fill(ds);
                    if ((null != ds) && (ds.Tables.Count > 0))
                    {
                        dt = ds.Tables[0];
                    }
                    con.Close();
                }
            }
            return dt;
        }
    }
}