using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;

namespace ERP.OMS.Reports
{
    public partial class Reports_ShowTransactionCommCurrency : System.Web.UI.Page
    {
        BusinessLogicLayer.Reports oReports = new BusinessLogicLayer.Reports();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        DataSet ds = new DataSet();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Transaction();

            }
        }
        void Transaction()
        {
            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            //{
            ds = oReports.Report_TransationCommCurrency(
                Convert.ToString(Session["usersegid"]),
                Convert.ToString(Session["LastCompany"]),
                Convert.ToString(HttpContext.Current.Session["LastFinYear"]),
                Convert.ToString(Request.QueryString["clientid"]),
                Convert.ToString(Request.QueryString["productid"]),
                Convert.ToString(Request.QueryString["settno"]),
                "NA"
                );
            //SqlCommand cmd = new SqlCommand();
            //cmd.Connection = con;
            //cmd.CommandText = "Report_TransationCommCurrency";
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("@segment", Convert.ToInt32(Session["usersegid"].ToString()));
            //cmd.Parameters.AddWithValue("@companyid", Session["LastCompany"].ToString());
            //cmd.Parameters.AddWithValue("@finyear", HttpContext.Current.Session["LastFinYear"]);
            //cmd.Parameters.AddWithValue("@Client",Request.QueryString["clientid"].ToString().Trim());
            //cmd.Parameters.AddWithValue("@Product",Request.QueryString["productid"].ToString().Trim());
            //cmd.Parameters.AddWithValue("@SettNo",Request.QueryString["settno"].ToString().Trim());
            //cmd.Parameters.AddWithValue("@TranId", "NA");
            //SqlDataAdapter da = new SqlDataAdapter();
            //da.SelectCommand = cmd;
            //cmd.CommandTimeout = 0;
            //ds.Reset();
            //da.Fill(ds);
            //da.Dispose();
            ViewState["dataset"] = ds;

            if (ds.Tables[0].Rows.Count == 0)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "JS", "<script language='JavaScript'>alert('No Transaction Found !!'); parent.editwin.close();</script>");
            }
            else
            {
                String strHtml = String.Empty;
                string str = "<b> Client Name : </b> " + ds.Tables[0].Rows[0]["ClientName"].ToString().Trim() + " <b> Product Name : </b>" + ds.Tables[0].Rows[0]["ProductName"].ToString().Trim() + " <b> Sett No : </b>" + Request.QueryString["settno"].ToString().Trim();
                strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                strHtml += "<tr><td align=\"left\" nowrap=\"nowrap;\" style=\"color:Blue;\">" + str + "</td></tr></table>";
                display.InnerHtml = strHtml;

                grdTransaction.DataSource = ds.Tables[0];
                grdTransaction.DataBind();
            }
            //  }

        }
    }
}