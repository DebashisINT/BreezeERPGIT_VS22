using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;

namespace ERP.OMS.Reports
{
    public partial class Reports_EnterTransactionCommCurrency : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.Reports oReports = new BusinessLogicLayer.Reports();
        DataSet ds = new DataSet();
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (!IsPostBack)
            {

                if (Request.QueryString["clientid"].ToString().Trim().StartsWith("CL"))
                {
                    ViewState["variable"] = "client";
                }
                else
                {
                    ViewState["variable"] = "exchange";
                }
                Page.ClientScript.RegisterStartupScript(GetType(), "pageload", "<script>Page_Load('" + ViewState["variable"].ToString().Trim() + "');</script>");
                date();
                popupdatafetch();
            }



        }
        void date()
        {
            dtTransaction.EditFormatString = oconverter.GetDateFormat("Date");
            dtExecution.EditFormatString = oconverter.GetDateFormat("Date");
            dtTransaction.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
            dtExecution.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
        }
        void popupdatafetch()
        {
            clientnamefetch();
            productnamefetch();
            txtSettNumber.Text = Request.QueryString["settno"].ToString().Trim();
            txtQuantity.Text = Request.QueryString["qty"].ToString().Trim();
            Fn_ClientIncomingFetch();
            Fn_ClientOutgoingFetch();
            Fn_TragetAc();
        }
        void clientnamefetch()
        {
            DataTable DT = new DataTable();
            DT = oDBEngine.GetDataTable("tbl_master_contact,TBL_MASTER_CONTACTEXCHANGE", "isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(rtrim(cnt_ucc),'')+']' as clientname ,cnt_internalID", " cnt_internalId like 'CL%' AND crg_cntid=cnt_internalid AND  crg_exchange=(select exh_shortName+' '+ '-' +' '+exch_segmentId as SegmentName from tbl_master_companyExchange,tbl_master_Exchange where exch_internalId='" + Convert.ToInt32(Session["usersegid"].ToString()) + "' and exch_compid='" + Session["LastCompany"].ToString() + "' and exh_cntId=exch_exchId) AND cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ") AND isnull(crg_suspensiondate,'1900-01-01 00:00:00.000')='1900-01-01 00:00:00.000' and cnt_internalid='" + Request.QueryString["clientid"].ToString().Trim() + "'");
            if (DT.Rows.Count > 0)
            {
                txtClientName_hidden.Text = DT.Rows[0]["cnt_internalID"].ToString().Trim();
                txtClientName.Text = DT.Rows[0]["clientname"].ToString().Trim();
            }
            DT = new DataTable();
            DT = oDBEngine.GetDataTable("(select A.EXCH_INTERNALID AS SEGMENTID ,isnull((TME.EXH_ShortName + '-' + A.EXCH_SEGMENTID),exch_membershipType) AS EXCHANGENAME from (SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='" + Session["LastCompany"].ToString() + "' ) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID)as TAB", "EXCHANGENAME,SEGMENTID", "SEGMENTID='" + Session["usersegid"].ToString().Trim() + "'");
            if (DT.Rows.Count > 0)
            {
                txtExchange.Text = DT.Rows[0]["EXCHANGENAME"].ToString().Trim();

            }
        }
        void productnamefetch()
        {
            DataTable DT = new DataTable();
            DT = oDBEngine.GetDataTable("(select (case when isnull(commodity_StrikePrice,0)=0.0 and commodity_ExpiryDate is null then isnull(rtrim(ltrim(commodity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(commodity_Tickercode)),'') when isnull(commodity_StrikePrice,0)=0.0 and commodity_ExpiryDate is not null then isnull(rtrim(ltrim(commodity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(commodity_Tickercode)),'')+' '+isnull(convert(varchar(9),commodity_ExpiryDate,6),'') else isnull(rtrim(ltrim(commodity_TickerSymbol)),'')+' '+isnull(rtrim(ltrim(commodity_Tickercode)),'')+' '+isnull(convert(varchar(9),commodity_ExpiryDate,6),'')+' '+cast(cast(round(commodity_StrikePrice,2) as numeric(28,2)) as varchar) end) as TickerSymbol,Commodity_ProductSeriesID from Master_commodity  WHERE commodity_ExchangeSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and Commodity_ProductSeriesID='" + Request.QueryString["productid"].ToString().Trim() + "' )as tb", " distinct TickerSymbol,Commodity_ProductSeriesID as ProductSeriesID", "Commodity_ProductSeriesID='" + Request.QueryString["productid"].ToString().Trim() + "'");
            if (DT.Rows.Count > 0)
            {
                txtProduct_hidden.Text = DT.Rows[0]["ProductSeriesID"].ToString().Trim();
                txtProduct.Text = DT.Rows[0]["TickerSymbol"].ToString().Trim();
            }
        }
        void Fn_ClientIncomingFetch()
        {
            DataTable DT = new DataTable();
            ddlSAccount.Items.Clear();
            if (ddlClientSourceIncoming.SelectedItem.Value == "MH")
            {
                DT = new DataTable();
                DT = oDBEngine.GetDataTable("master_dpaccounts", "cast(DPAccounts_ID as varchar)+'~'+DPACCounts_DPID+'~'+DPACCounts_ClientID+'~'+isnull(DPACCounts_CMBPID,'')+'~'+isnull(cast(DPACCounts_ExchangeSegmentID as varchar),'') as ID,rtrim(DPACCounts_ShortName) as ShortName", " DPACCounts_AccountType in('[MRGIN]','[HOLDBK]') and DPACCounts_CompanyID='" + Session["LastCompany"].ToString() + "'");
                if (DT.Rows.Count > 0)
                {
                    ddlSAccount.DataTextField = "ShortName";
                    ddlSAccount.DataValueField = "ID";
                    ddlSAccount.DataSource = DT;
                    ddlSAccount.DataBind();
                }
            }
            else if (ddlClientSourceIncoming.SelectedItem.Value == "C")
            {
                DT = new DataTable();
                DT = oDBEngine.GetDataTable("tbl_master_contactdpdetails", "cast(dpd_id as varchar)+'~'+dpd_dpCode+'~'+dpd_ClientID+'~'+isnull(dpd_cmbpid,'N/A') AS id,(select rtrim(replace(dp_dpname,char(160),'')) from tbl_master_depositoryparticipants where substring(dp_dpID,1,8)=dpd_dpCode)+' ['+ rtrim(dpd_ClientID)+']' AS ShortName,rtrim(dpd_accountType) as dpd_accountType", " dpd_cntId='" + txtClientName_hidden.Text.ToString().Trim() + "' and dpd_accountType='CommodityDP'", " dpd_accountType ");
                if (DT.Rows.Count > 0)
                {
                    ddlSAccount.DataTextField = "ShortName";
                    ddlSAccount.DataValueField = "ID";
                    ddlSAccount.DataSource = DT;
                    ddlSAccount.DataBind();
                }
            }
            else
            {
                DT = new DataTable();
                DT = oDBEngine.GetDataTable("master_dpaccounts", "cast(DPAccounts_ID as varchar)+'~'+DPACCounts_DPID+'~'+DPACCounts_ClientID+'~'+isnull(DPACCounts_CMBPID,'')+'~'+isnull(cast(DPACCounts_ExchangeSegmentID as varchar),'') as ID,rtrim(DPACCounts_ShortName) as ShortName", " DPACCounts_AccountType like '%OWN%' and DPACCounts_CompanyID='" + Session["LastCompany"].ToString() + "'");
                if (DT.Rows.Count > 0)
                {
                    ddlSAccount.DataTextField = "ShortName";
                    ddlSAccount.DataValueField = "ID";
                    ddlSAccount.DataSource = DT;
                    ddlSAccount.DataBind();
                }
            }


        }
        void Fn_ClientOutgoingFetch()
        {
            DataTable DT = new DataTable();
            ddlTAccount.Items.Clear();
            if (ddlClientTargetOutgoing.SelectedItem.Value == "MH")
            {
                DT = oDBEngine.GetDataTable("master_dpaccounts", "cast(DPAccounts_ID as varchar)+'~'+DPACCounts_DPID+'~'+DPACCounts_ClientID+'~'+isnull(DPACCounts_CMBPID,'')+'~'+isnull(cast(DPACCounts_ExchangeSegmentID as varchar),'') as ID,rtrim(DPACCounts_ShortName) as ShortName", " DPACCounts_AccountType in('[MRGIN]','[HOLDBK]') and DPACCounts_CompanyID='" + Session["LastCompany"].ToString() + "'");
                if (DT.Rows.Count > 0)
                {
                    ddlTAccount.DataTextField = "ShortName";
                    ddlTAccount.DataValueField = "ID";
                    ddlTAccount.DataSource = DT;
                    ddlTAccount.DataBind();
                }
            }
            else if (ddlClientTargetOutgoing.SelectedItem.Value == "C")
            {
                DT = oDBEngine.GetDataTable("tbl_master_contactdpdetails", "cast(dpd_id as varchar)+'~'+dpd_dpCode+'~'+dpd_ClientID+'~'+isnull(dpd_cmbpid,'N/A') AS id,(select rtrim(replace(dp_dpname,char(160),'')) from tbl_master_depositoryparticipants where substring(dp_dpID,1,8)=dpd_dpCode)+' ['+ rtrim(dpd_ClientID)+']' AS ShortName,rtrim(dpd_accountType) as dpd_accountType", " dpd_cntId='" + txtClientName_hidden.Text.ToString().Trim() + "'  and dpd_accountType='CommodityDP'", " dpd_accountType ");
                if (DT.Rows.Count > 0)
                {

                    ddlTAccount.DataTextField = "ShortName";
                    ddlTAccount.DataValueField = "ID";
                    ddlTAccount.DataSource = DT;
                    ddlTAccount.DataBind();
                }
            }
            else
            {
                DT = oDBEngine.GetDataTable("master_dpaccounts", "cast(DPAccounts_ID as varchar)+'~'+DPACCounts_DPID+'~'+DPACCounts_ClientID+'~'+isnull(DPACCounts_CMBPID,'')+'~'+isnull(cast(DPACCounts_ExchangeSegmentID as varchar),'') as ID,rtrim(DPACCounts_ShortName) as ShortName", " DPACCounts_AccountType like '%OWN%' and DPACCounts_CompanyID='" + Session["LastCompany"].ToString() + "'");
                if (DT.Rows.Count > 0)
                {
                    ddlTAccount.DataTextField = "ShortName";
                    ddlTAccount.DataValueField = "ID";
                    ddlTAccount.DataSource = DT;
                    ddlTAccount.DataBind();
                }
            }


        }
        protected void ddlClientSourceIncoming_SelectedIndexChanged(object sender, EventArgs e)
        {
            Fn_ClientIncomingFetch();
        }
        void Fn_TragetAc()
        {
            DataTable DT = new DataTable();
            DT = oDBEngine.GetDataTable("master_dpaccounts", "cast(DPAccounts_ID as varchar)+'~'+DPACCounts_DPID+'~'+DPACCounts_ClientID+'~'+rtrim(DPACCounts_AccountType)+'~'+isnull(DPACCounts_CMBPID,'')+'~'+isnull(cast(DPACCounts_ExchangeSegmentID as varchar),'') as ID,rtrim(DPACCounts_ShortName) as ShortName", " DPACCounts_AccountType in('[POOL]','[HOLDBK]','[MRGIN]','[PLPAYIN]','[PLPAYOUT]','[DUMMYADJ]') and DPACCounts_CompanyID='" + Session["LastCompany"].ToString() + "' and DPACCounts_ExchangeSegmentID=" + Session["usersegid"].ToString() + "");

            if (DT.Rows.Count > 0)
            {
                ddlClientTargetIncoming.DataTextField = "ShortName";
                ddlClientTargetIncoming.DataValueField = "ID";
                ddlClientTargetIncoming.DataSource = DT;
                ddlClientTargetIncoming.DataBind();


                ddlClientSourceOutgoing.DataTextField = "ShortName";
                ddlClientSourceOutgoing.DataValueField = "ID";
                ddlClientSourceOutgoing.DataSource = DT;
                ddlClientSourceOutgoing.DataBind();


                ddlMarketIncoming.DataTextField = "ShortName";
                ddlMarketIncoming.DataValueField = "ID";
                ddlMarketIncoming.DataSource = DT;
                ddlMarketIncoming.DataBind();


                ddlMarketOutgoing.DataTextField = "ShortName";
                ddlMarketOutgoing.DataValueField = "ID";
                ddlMarketOutgoing.DataSource = DT;
                ddlMarketOutgoing.DataBind();

            }


        }


        protected void ddlClientTargetOutgoing_SelectedIndexChanged(object sender, EventArgs e)
        {
            Fn_ClientOutgoingFetch();
        }


        void InsertTransaction()
        {
            string slipno;
            string ownaccountsource = string.Empty;
            string customeraccountsource = string.Empty;
            string ownaccountarget = string.Empty;
            string customeraccounttarget = string.Empty;
            string sourcedpid = string.Empty;
            string sourceclientid = string.Empty;
            string targetdpid = string.Empty;
            string targetclientid = string.Empty;
            if (txtSlipNumber.Text.ToString().Trim() == "")
            {
                slipno = "NA";
            }
            else
            {
                slipno = txtSlipNumber.Text.ToString().Trim();
            }
            string remarks;
            if (txtRemarks.Text.ToString().Trim() == "")
            {
                remarks = "NA";
            }
            else
            {
                remarks = txtRemarks.Text.ToString().Trim();
            }

            string clientid;
            if (ViewState["variable"].ToString().Trim() == "exchange")
            {
                clientid = txtExchange.Text.ToString().Trim();
            }
            else
            {
                clientid = txtClientName_hidden.Text.ToString().Trim();
            }


            if (DDlTranType.SelectedItem.Value.ToString().Trim() == "CI")/////////Client Incoming
            {
                string[] A1 = ddlSAccount.SelectedItem.Value.Split('~');
                string[] A2 = ddlClientTargetIncoming.SelectedItem.Value.Split('~');

                ownaccountsource = "NA";
                customeraccountsource = A1[0].ToString().Trim();
                ownaccountarget = A2[0].ToString().Trim();
                customeraccounttarget = "NA";

                sourcedpid = A1[1].ToString().Trim();
                sourceclientid = A1[2].ToString().Trim();
                targetdpid = A2[1].ToString().Trim();
                targetclientid = A2[2].ToString().Trim();

            }
            if (DDlTranType.SelectedItem.Value.ToString().Trim() == "CO")/////////Client Outgoing
            {
                string[] A1 = ddlClientSourceOutgoing.SelectedItem.Value.Split('~');
                string[] A2 = ddlTAccount.SelectedItem.Value.Split('~');

                ownaccountsource = A1[0].ToString().Trim();
                customeraccountsource = "NA";
                ownaccountarget = "NA";
                customeraccounttarget = A2[0].ToString().Trim();

                sourcedpid = A1[1].ToString().Trim();
                sourceclientid = A1[2].ToString().Trim();
                targetdpid = A2[1].ToString().Trim();
                targetclientid = A2[2].ToString().Trim();

            }
            if (DDlTranType.SelectedItem.Value.ToString().Trim() == "MI")/////////Market Incoming[Exchange Outgoing]
            {
                string[] A1 = ddlMarketIncoming.SelectedItem.Value.Split('~');

                ownaccountsource = A1[0].ToString().Trim();
                customeraccountsource = "NA";
                ownaccountarget = "NA";
                customeraccounttarget = "NA";

                sourcedpid = A1[1].ToString().Trim();
                sourceclientid = A1[2].ToString().Trim();
                targetdpid = "NA";
                targetclientid = "NA";

            }
            if (DDlTranType.SelectedItem.Value.ToString().Trim() == "MO")/////////Market Outgoing[Exchange Incoming]
            {
                string[] A1 = ddlMarketOutgoing.SelectedItem.Value.Split('~');

                ownaccountsource = "NA";
                customeraccountsource = "NA";
                ownaccountarget = A1[0].ToString().Trim();
                customeraccounttarget = "NA";

                sourcedpid = "NA";
                sourceclientid = "NA";
                targetdpid = A1[1].ToString().Trim();
                targetclientid = A1[2].ToString().Trim();

            }

            ds = oReports.Insert_TransactionCommCurrency(
                Convert.ToString(Session["LastFinYear"]),
                  Convert.ToString(Session["LastCompany"]),
                  Convert.ToString(Session["usersegid"]),
                  clientid,
                  Convert.ToString(txtProduct_hidden.Text),
                  Convert.ToString(ddldeliverymode.SelectedItem.Value),
                  Convert.ToString(DDlTranType.SelectedItem.Value),
                 "NR",
                 "P",
                  Convert.ToString(txtSettNumber.Text),
                  slipno,
                  Convert.ToDecimal(txtQuantity.Text),
                  remarks,
                  "M",
                  ownaccountsource,
                  customeraccountsource,
                  ownaccountarget,
                  customeraccounttarget,
                  sourcedpid,
                  sourceclientid,
                  targetdpid,
                  targetclientid,
                  Convert.ToString(dtTransaction.Value),
                  Convert.ToString(dtExecution.Value),
                  Convert.ToString(HttpContext.Current.Session["ExchangeSegmentID"]),
                   Convert.ToString(txtICIN.Text),
                  Convert.ToString(Session["userid"])
                );

            if (ds.Tables[0].Rows.Count > 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "AlertResult", "AlertResult('" + ds.Tables[0].Rows[0]["rowstatus"].ToString().Trim() + "');", true);
            }

        }
        protected void btnInsert_Click(object sender, EventArgs e)
        {
            spverify();
        }
        void spverify()
        {
            if (txtProduct.Text.ToString().Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "AlertStatus1", "AlertStatus('" + ViewState["variable"].ToString().Trim() + "','Please Select Product!');", true);
            }
            else
            {
                if (txtICIN.Text.ToString().Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "AlertStatus1", "AlertStatus('" + ViewState["variable"].ToString().Trim() + "','Please Select ICIN!');", true);
                }
                else
                {
                    InsertTransaction();
                }
            }
        }
    }
}