using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;
using BusinessLogicLayer.EmailTemplate;
using Newtonsoft.Json;
using DataAccessLayer;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace ERP.OMS.Management.Master
{
    public partial class add_new_versions_features : System.Web.UI.Page
    {
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        public EntityLayer.CommonELS.UserRightsForPage rights = new EntityLayer.CommonELS.UserRightsForPage();
        string data = "";
        public string pageAccess = "";

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               

                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);


                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("sp_Version_Features");
                proc.AddVarcharPara("@Action", 30, "3");
                proc.AddVarcharPara("@CurrentDBVersion_Number", 30, "");
                dt = proc.GetTable();
                hiddnversion1.Value = dt.Rows[0][0].ToString();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }




        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public static string addversionfeatures(string VersionNumber, string FeaturesMarkup,string flag)
        {
           
            try
            {
                DataTable dt =  GerVersionFeatures(VersionNumber, FeaturesMarkup, flag);
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
        static byte[] EncodeToBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
        
        
        
        public static DataTable GerVersionFeatures(string VersionNumber, string FeaturesMarkup,string flag)
        {
            DataTable dt = new DataTable();
            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString; MULTI
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            byte[] bytes = EncodeToBytes(FeaturesMarkup);
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand myCMD = new SqlCommand("sp_Version_Features", con))
                {
                    myCMD.Parameters.Add("@Action", SqlDbType.VarChar).Value = flag;
                    myCMD.Parameters.Add("@CurrentDBVersion_Number", SqlDbType.VarChar).Value = VersionNumber;
                    myCMD.Parameters.Add("@CurrentDBVersion_WhatsNew", SqlDbType.Image).Value = bytes;

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