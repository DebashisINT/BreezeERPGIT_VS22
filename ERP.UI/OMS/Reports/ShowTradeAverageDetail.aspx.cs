using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace ERP.OMS.Reports
{
    public partial class Reports_ShowTradeAverageDetail : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        string id = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                    id = Request.QueryString["id"].ToString();
                DataTable dt = new DataTable();
                dt = oDBEngine.GetDataTable("Select convert(varchar(11),ExchangeTrades_TradeDate,106) TradeDate,isnull(ExchangeTrades_SecuritySymbol,'')+' ['+isnull(ExchangeTrades_SecuritySeries,'')+']' Scrip,ExchangeTrades_OrderNumber OrderNumber,convert(varchar(8),cast(ExchangeTrades_OrderEntryTime as datetime),114) OrderTime,ExchangeTrades_TradeNumber TradeNumber,convert(varchar(8),cast(ExchangeTrades_TradeEntryTime as datetime),114) TradeTime,case When ExchangeTrades_BuySellFlag=1 Then 'Buy' Else 'Sell' End [Buy/Sell],ExchangeTrades_QuantityLots Quantity,ExchangeTrades_UnitPrice UnitPrice,ExchangeTrades_MarketValue MarketValue	 From Trans_ExchangeTrades where ExchangeTrades_CustTransactionID=" + id + " Order By ExchangeTrades_TradeNumber");

                if (dt.Rows.Count > 0)
                {
                    rptExchAvgTrade.DataSource = dt;
                    rptExchAvgTrade.DataBind();
                }
            }
        }
    }
}