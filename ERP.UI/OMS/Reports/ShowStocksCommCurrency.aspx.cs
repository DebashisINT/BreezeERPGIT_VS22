using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;

namespace ERP.OMS.Reports
{
    public partial class Reports_ShowStocksCommCurrency : System.Web.UI.Page
    {
        BusinessLogicLayer.Reports oReports = new BusinessLogicLayer.Reports();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        DataSet ds = new DataSet();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Stocks();

            }
        }
        void Stocks()
        {
            ds = oReports.Report_StocksCommCurrency(
                Convert.ToString(HttpContext.Current.Session["LastFinYear"]),
                Convert.ToString(Request.QueryString["productid"])
                );
            ViewState["dataset"] = ds;

            if (ds.Tables[0].Rows.Count == 0)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "JS", "<script language='JavaScript'>alert('No Stocks Found !!'); parent.editwin.close();</script>");
            }
            else
            {
                String strHtml = String.Empty;
                string str = " <b> Product Name : </b>" + ds.Tables[0].Rows[0]["ProductName"].ToString().Trim();
                strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                strHtml += "<tr><td align=\"left\" nowrap=\"nowrap;\" style=\"color:Blue;\">" + str + "</td></tr></table>";
                display.InnerHtml = strHtml;

                grdStocks.DataSource = ds.Tables[0];
                grdStocks.DataBind();
            }


        }
    }

}