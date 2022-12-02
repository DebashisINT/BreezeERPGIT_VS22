using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_frmReport_BillingSpot : System.Web.UI.Page
    {
        BusinessLogicLayer.Reports rep = new BusinessLogicLayer.Reports();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        public string Instruments = null;
        public string Clients = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dtDate.EditFormatString = oconverter.GetDateFormat("Date");
                dtDate.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
            }
        }
        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            DataTable DtCount = oDBEngine.GetDataTable("Trans_ComDailyStat", "count(ComDailyStat_DateTime)", " cast(DATEADD(dd, 0, DATEDIFF(dd, 0,ComDailyStat_DateTime)) as datetime)=cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtDate.Value + "')) as datetime) and ComDailyStat_ExchangeSegmentID=" + HttpContext.Current.Session["ExchangeSegmentID"].ToString() + "");
            if (DtCount.Rows.Count > 0)
            {
                string settNo = HttpContext.Current.Session["LastSettNo"].ToString();
                string SettmentNo = settNo.Substring(0, 7);
                string SettType = settNo.Substring(7);
                DataTable dtInstruments = oDBEngine.GetDataTable("Master_Commodity,Trans_ComCustomerTrades", "distinct Commodity_ProductSeriesID", "ComCustomerTrades_ProductSeriesID=Commodity_ProductSeriesID and Commodity_ExchangeSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "'");
                if (dtInstruments.Rows.Count > 0)
                {
                    for (int i = 0; i < dtInstruments.Rows.Count; i++)
                    {
                        if (Instruments == null)
                            Instruments = dtInstruments.Rows[i][0].ToString();
                        else
                            Instruments += "," + dtInstruments.Rows[i][0].ToString();
                    }
                }
                String conn = ConfigurationSettings.AppSettings["DBConnectionDefault"];
                SqlConnection con = new SqlConnection(conn);
                if (ddlGenerate.SelectedItem.Value == "1")
                {

                    rep.delBillingSPOT(Convert.ToInt32(Session["usersegid"].ToString()), dtDate.Value.ToString(), Session["LastCompany"].ToString(), Session["LastFinYear"].ToString());

                    oDBEngine.DeleteValue("Trans_COMPosition", " COMPosition_CompanyID='" + Session["LastCompany"].ToString() + "' and COMPosition_SegmentID='" + Session["usersegid"].ToString() + "' and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,COMPosition_Date)) as datetime)=cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtDate.Value + "')) as datetime)");
                    oDBEngine.DeleteValue("Trans_COMPositionSummary", " COMPositionSummary_CompanyID='" + Session["LastCompany"].ToString() + "' and COMPositionSummary_SegmentID='" + Session["usersegid"].ToString() + "' and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,COMPositionSummary_Date)) as datetime)=cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtDate.Value + "')) as datetime)");

                    rep.ExchangeObligationSPOT(dtDate.Value.ToString(), Session["usersegid"].ToString(), HttpContext.Current.Session["ExchangeSegmentID"].ToString(),
                         Session["LastCompany"].ToString(), SettmentNo, SettType, Convert.ToInt32(Session["userid"].ToString()), Session["LastFinYear"].ToString());

                    DataTable dtClients = oDBEngine.GetDataTable("tbl_master_contact contact,Trans_ComCustomerTrades,master_commodity",
                            "distinct contact.cnt_internalId,(isnull(rtrim(contact.cnt_firstName),'') +' '+" +
                            "isnull(rtrim(contact.cnt_middleName),'')+' '+isnull(rtrim(contact.cnt_lastName),''))" +
                            "+'['+isnull(rtrim(contact.cnt_UCC),'')+']' as Name", "ComCustomerTrades_CompanyID='" +
                            Session["LastCompany"].ToString() + "' and ComCustomerTrades_ExchangeSegment='" +
                            Convert.ToInt32(Session["usersegid"].ToString()) + "' and ComCustomerTrades_TradeDate<='" +
                            dtDate.Value + "'  and ComCustomerTrades_BranchID in(" + Session["userbranchHierarchy"].ToString() +
                            ") and ComCustomerTrades_ProductSeriesID in(" + Instruments + ")  and " +
                            "contact.cnt_internalId = ComCustomerTrades_CustomerID " +
                            "and  Commodity_ProductSeriesID=ComCustomerTrades_ProductSeriesID", " Name");
                    if (dtClients.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtClients.Rows.Count; i++)
                        {
                            oDBEngine.DeleteValue("Trans_OtherCharges", " OtherCharges_CompanyID='" + Session["LastCompany"].ToString() + "' and OtherCharges_SegmentID='" + Session["usersegid"].ToString() + "' and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,OtherCharges_Date)) as datetime)=cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtDate.Value + "')) as datetime) and OtherCharges_CustomerID='" + dtClients.Rows[i][0].ToString() + "'");

                            rep.ClientObligationSPOT(dtDate.Value.ToString(), Session["usersegid"].ToString(), HttpContext.Current.Session["ExchangeSegmentID"].ToString(),
                                Session["LastCompany"].ToString(), SettmentNo, SettType, Convert.ToInt32(Session["userid"].ToString()), dtClients.Rows[i][0].ToString(),
                                Instruments, Session["LastFinYear"].ToString());
                        }
                    }

                    rep.InsertExchComOblJV_SPOT(Convert.ToInt32(Session["usersegid"].ToString()), Convert.ToInt32(Session["userid"].ToString()), dtDate.Value.ToString(),
                        Session["LastCompany"].ToString(), SettmentNo, SettType, Session["LastFinYear"].ToString(), Convert.ToInt32(Session["ExchangeSegmentID"].ToString()));


                    rep.AccountsLedgerBillingForSPOT(Convert.ToInt32(Session["usersegid"].ToString()), Convert.ToInt32(Session["userid"].ToString()), dtDate.Value.ToString(),
                      Session["LastCompany"].ToString(), SettmentNo, SettType, Session["LastFinYear"].ToString());

                    ScriptManager.RegisterStartupScript(this, GetType(), "JS", "alert('Generate Successfully');", true);
                }
                else
                {

                    rep.delBillingSPOT(Convert.ToInt32(Session["usersegid"].ToString()), dtDate.Value.ToString(), Session["LastCompany"].ToString(), Session["LastFinYear"].ToString());
                    oDBEngine.DeleteValue("Trans_COMPosition", " COMPosition_CompanyID='" + Session["LastCompany"].ToString() + "' and COMPosition_SegmentID='" + Session["usersegid"].ToString() + "' and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,COMPosition_Date)) as datetime)=cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtDate.Value + "')) as datetime)");
                    oDBEngine.DeleteValue("Trans_COMPositionSummary", " COMPositionSummary_CompanyID='" + Session["LastCompany"].ToString() + "' and COMPositionSummary_SegmentID='" + Session["usersegid"].ToString() + "' and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,COMPositionSummary_Date)) as datetime)=cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtDate.Value + "')) as datetime)");
                    ScriptManager.RegisterStartupScript(this, GetType(), "JS", "alert('Deleted Successfully');", true);
                }
            }

        }
    }
}