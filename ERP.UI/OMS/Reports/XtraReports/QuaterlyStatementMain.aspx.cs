using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace ERP.OMS.Reports.XtraReports
{
    public partial class QuaterlyStatementMain : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection("Data Source=10.0.1.71;Initial Catalog=Nakamichi;User ID=sa; Password=sql123;pooling='true';Max Pool Size=200");
            SqlCommand com = new SqlCommand("Report_QuatarlyStatement", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@FINYEAR","2012-2013");
            com.Parameters.AddWithValue("@FORDATE","30 Jun 2012");
            com.Parameters.AddWithValue("@COMPANYID","COR0000002");
            com.Parameters.AddWithValue("@CLIENTS","ALL");
            com.Parameters.AddWithValue("@BRANCHID","130,129,178");
            com.Parameters.AddWithValue("@GRPTYPE", "BRANCH");
            com.Parameters.AddWithValue("@Groupby", "130,129,178");
            com.Parameters.AddWithValue("@MASTERSEGMENT", 1);
            com.Parameters.AddWithValue("@Header", "");
            com.Parameters.AddWithValue("@Footer", "");
            com.Parameters.AddWithValue("@ChkConsiderEntirePeriod", "N");
            com.Parameters.AddWithValue("@ChkDoNotPrintSecurities", "N");
            com.Parameters.AddWithValue("@dpid", "24");
            com.Parameters.AddWithValue("@Fromdatefrmpage", "");
            com.Parameters.AddWithValue("@Todatefrmpage", "");
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            da.Fill(ds);
            con.Close();

        }
    }
}