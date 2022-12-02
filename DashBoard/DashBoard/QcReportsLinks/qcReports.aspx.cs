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
using UtilityLayer;

namespace DashBoard.DashBoard.QcReportsLinks
{
    public partial class qcReports : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        [WebMethod]
        public static object GetReportsLink()
        {
            DataTable dsInst = new DataTable();
            //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["crmConnectionString"].ToString());
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("prc_GetQuickLink", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            
            cmd.Parameters.AddWithValue("@groupid", HttpContext.Current.Session["usergoup"]);

            cmd.CommandTimeout = 0;
            SqlDataAdapter Adap = new SqlDataAdapter();
            Adap.SelectCommand = cmd;
            Adap.Fill(dsInst);
            cmd.Dispose();
            con.Dispose();

            List<QuickMenuLink> lstQuickMenuLink = APIHelperMethods.ToModelList<QuickMenuLink>(dsInst);

            return lstQuickMenuLink;
        }


        public class QuickMenuLink
        {
            public string Link { get; set; }
            public string Name { get; set; }
        }
    }
}