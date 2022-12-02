using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_DeliveryCentre : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        string data;
        static string Clients;
        static string Script;
        static DataTable DtDeliveryCentre = new DataTable();
        BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        ExcelFile objExcel = new ExcelFile();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string SettNumber = Session["LastSettNo"].ToString();
                txtSettlementNumber.Text = SettNumber.Substring(0, 7);
                txtSettlementType.Text = SettNumber.Substring(7, 1);
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='JavaScript'>Page_Load();</script>");
            }
            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            //___________-end here___//
        }
        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            string id = eventArgument.ToString();
            string[] idlist = id.Split('~');
            string[] cl = idlist[1].Split(',');
            string str = "";
            string str1 = "";
            for (int i = 0; i < cl.Length; i++)
            {
                string[] val = cl[i].Split(';');
                if (str == "")
                {
                    str = "'" + val[0] + "'";
                    str1 = val[0] + ";" + val[1];
                }
                else
                {
                    str += ",'" + val[0] + "'";
                    str1 += "," + val[0] + ";" + val[1];
                }
            }
            if (idlist[0] == "Clients")
            {
                Clients = str;
                data = "Clients~" + str1;
            }
            else if (idlist[0] == "Scrips")
            {
                Script = str;
                data = "Scrips~" + str1;
            }

        }
        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            FillGrid();
            DataTable DtEx = (DataTable)ViewState["ExDataset"];

            if (DtEx.Rows.Count > 0)
            {
                string Settlement = "";
                string Type = "";
                string AcType = "";
                string Total = null;
                if (ddlType.SelectedItem.Value == "S")
                {
                    Type = "Type: [Stocks]";
                    if (ddlAccountType.SelectedItem.Value == "P")
                    {
                        AcType = " Account Type:  [Pool A/C]";
                        if (rdbSettTypeAll.Checked == true)
                            Settlement = " All Settlement";
                        else
                            Settlement = txtSettlementNumber.Text + " Settlement";
                    }
                    else if (ddlAccountType.SelectedItem.Value == "M")
                    {
                        AcType = " Account Type:  [Margin A/C]";
                        Settlement = "";
                    }
                    else
                    {
                        AcType = " Account Type:  [Own A/C]";
                        Settlement = "";
                    }
                }
                else if (ddlType.SelectedItem.Value == "M")
                {
                    Type = "Type: [Margin]";
                    Settlement = "";
                }
                else if (ddlType.SelectedItem.Value == "O")
                {
                    Type = "Type: [Obligation]";
                    if (rdbSettTypeAll.Checked == true)
                        Settlement = "All Settlement";
                    else
                        Settlement = txtSettlementNumber.Text + " Settlement";
                }
                Total = Type + AcType + " " + Settlement;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct1", "AfterShow('" + Total + "')", true); ;
            }
        }
        public void FillGrid()
        {
            string AccountID = null;
            string SettlementType = null;
            string ForClientExchange = null;
            string Scrip = null;
            string Movement = null;
            string Nature = null;
            string SettNum = txtSettlementNumber.Text;
            string SettType = txtSettlementType.Text;
            string OrderBy = null;
            string FreeBalanceShow = null;
            string Segment = null;
            if (ddlType.SelectedItem.Value == "S")
            {
                if (ddlAccountType.SelectedItem.Value == "A")
                    AccountID = null;
                else
                    AccountID = " where DematStocks_AccountID=" + TxtAccount_hidden.Value + "";
                if (rdbScripsAll.Checked == true)
                    Scrip = null;
                else
                {
                    if (AccountID == null)
                        Scrip = " where DematStocks_ProductSeriesID in(" + Script + ")";
                    else
                        Scrip = " and DematStocks_ProductSeriesID in(" + Script + ")";
                }
                if (ddlAccountType.SelectedItem.Value == "P")
                {
                    if (rdbSettTypeAll.Checked == true && rdbSettTypeAll1.Checked == true)
                        SettlementType = null;
                    else if (rdbSettTypeAll.Checked == true && rdbSettTypeAll1.Checked == false)
                        SettlementType = " and DematStocks_SettlementType='" + SettType + "'";
                    else if (rdbSettTypeAll.Checked == false && rdbSettTypeAll1.Checked == true)
                        SettlementType = " and DematStocks_SettlementNumber='" + SettNum + "'";
                    else
                        SettlementType = " and DematStocks_SettlementNumber='" + SettNum + "'  and DematStocks_SettlementType='" + SettType + "'";
                }
                else
                    SettlementType = null;
                if (radShowNonZero.Checked == true)
                    FreeBalanceShow = " where FreeBalance<>0";
                else
                    FreeBalanceShow = null;
                if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "1" || HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "4" || HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "15" || HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "19")
                {
                    DtDeliveryCentre = oDbEngine.GetDataTable("(select (isnull((select DPAccounts_ShortName from master_dpaccounts where DPAccounts_ID=DematStocks_AccountID),(select isnull((select rtrim(dp_dpname) from tbl_master_depositoryparticipants where substring(dp_dpID,1,8)=dpd_dpCode),'')+' '+isnull(rtrim(dpd_ClientID),'') from tbl_master_contactdpdetails where dpd_id=DematStocks_AccountID))) as ACName,isnull(rtrim(Equity_TickerSymbol),'')+' ['+isnull(rtrim(Equity_Series),isnull(rtrim(Equity_TickerCode),''))+']' as Scrip,DematStocks_ISIN,DematStocks_SettlementNumber+DematStocks_SettlementType as Settlement,isnull(DematStocks_OpeningQty,0) as OpeningQty,isnull(DematStocks_InQty,0) as InQty,isnull(DematStocks_OutQty,0) as OutQty,isnull(DematStocks_PledgedQty,0) as PledgeQty,isnull(DematStocks_LockInQty,0) as Blocked,(isnull(DematStocks_OpeningQty,0)+isnull(DematStocks_InQty,0)-isnull(DematStocks_OutQty,0)-isnull(DematStocks_PledgedQty,0)) as FreeBalance,Equity_TickerCode,Equity_ExchSegmentID,Equity_TickerSymbol from Trans_DematStocks,master_equity " + AccountID + "  " + Scrip + " " + SettlementType + " and equity_exchsegmentid in ('1','4') and DematStocks_FinYear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and Equity_SeriesID=DematStocks_ProductSeriesID) as DD " + FreeBalanceShow + "", " ACName,Scrip,DematStocks_ISIN,Settlement,case when OpeningQty=0 then null else cast(OpeningQty as varchar) end AS OpeningQty,case when InQty=0 then null else cast(InQty as varchar) end as InQty,case when OutQty=0 then null else cast(OutQty as varchar) end as OutQty,case when PledgeQty=0 then null else cast(PledgeQty as varchar) end as PledgeQty,case when Blocked=0 then null else cast(Blocked as varchar) end as Blocked,case when FreeBalance=0 then null else cast(FreeBalance as varchar) end as FreeBalance,Equity_TickerCode,Equity_ExchSegmentID,Equity_TickerSymbol", null, " Scrip");
                }
                else
                {
                    DtDeliveryCentre = oDbEngine.GetDataTable("(select (isnull((select DPAccounts_ShortName from master_dpaccounts where DPAccounts_ID=DematStocks_AccountID),(select isnull((select rtrim(dp_dpname) from tbl_master_depositoryparticipants where substring(dp_dpID,1,8)=dpd_dpCode),'')+' '+isnull(rtrim(dpd_ClientID),'') from tbl_master_contactdpdetails where dpd_id=DematStocks_AccountID))) as ACName,isnull(rtrim(Equity_TickerSymbol),'')+' ['+isnull(rtrim(Equity_Series),isnull(rtrim(Equity_TickerCode),''))+']' as Scrip,DematStocks_ISIN,DematStocks_SettlementNumber+DematStocks_SettlementType as Settlement,isnull(DematStocks_OpeningQty,0) as OpeningQty,isnull(DematStocks_InQty,0) as InQty,isnull(DematStocks_OutQty,0) as OutQty,isnull(DematStocks_PledgedQty,0) as PledgeQty,isnull(DematStocks_LockInQty,0) as Blocked,(isnull(DematStocks_OpeningQty,0)+isnull(DematStocks_InQty,0)-isnull(DematStocks_OutQty,0)-isnull(DematStocks_PledgedQty,0)) as FreeBalance,Equity_TickerCode,Equity_ExchSegmentID,Equity_TickerSymbol from Trans_DematStocks,master_equity " + AccountID + "  " + Scrip + " " + SettlementType + "  and DematStocks_FinYear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and Equity_SeriesID=DematStocks_ProductSeriesID) as DD " + FreeBalanceShow + "", " ACName,Scrip,DematStocks_ISIN,Settlement,case when OpeningQty=0 then null else cast(OpeningQty as varchar) end AS OpeningQty,case when InQty=0 then null else cast(InQty as varchar) end as InQty,case when OutQty=0 then null else cast(OutQty as varchar) end as OutQty,case when PledgeQty=0 then null else cast(PledgeQty as varchar) end as PledgeQty,case when Blocked=0 then null else cast(Blocked as varchar) end as Blocked,case when FreeBalance=0 then null else cast(FreeBalance as varchar) end as FreeBalance,Equity_TickerCode,Equity_ExchSegmentID,Equity_TickerSymbol", null, " Scrip");
                }
                if (DtDeliveryCentre.Rows.Count > 0)
                {
                    for (int i = 0; i < DtDeliveryCentre.Rows.Count; i++)
                    {
                        if (DtDeliveryCentre.Rows[i]["OpeningQty"] != DBNull.Value)
                            DtDeliveryCentre.Rows[i]["OpeningQty"] = oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(DtDeliveryCentre.Rows[i]["OpeningQty"]));
                        if (DtDeliveryCentre.Rows[i]["InQty"] != DBNull.Value)
                            DtDeliveryCentre.Rows[i]["InQty"] = oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(DtDeliveryCentre.Rows[i]["InQty"]));
                        if (DtDeliveryCentre.Rows[i]["OutQty"] != DBNull.Value)
                            DtDeliveryCentre.Rows[i]["OutQty"] = oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(DtDeliveryCentre.Rows[i]["OutQty"]));
                        if (DtDeliveryCentre.Rows[i]["PledgeQty"] != DBNull.Value)
                            DtDeliveryCentre.Rows[i]["PledgeQty"] = oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(DtDeliveryCentre.Rows[i]["PledgeQty"]));
                        if (DtDeliveryCentre.Rows[i]["Blocked"] != DBNull.Value)
                            DtDeliveryCentre.Rows[i]["Blocked"] = oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(DtDeliveryCentre.Rows[i]["Blocked"]));
                        if (DtDeliveryCentre.Rows[i]["FreeBalance"] != DBNull.Value)
                            DtDeliveryCentre.Rows[i]["FreeBalance"] = oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(DtDeliveryCentre.Rows[i]["FreeBalance"]));
                    }
                    grdDematStocks.DataSource = DtDeliveryCentre;
                    grdDematStocks.DataBind();
                    ViewState["ExDataset"] = DtDeliveryCentre;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct1123", "alert('No Record Found !!')", true); ;
                    grdDematStocks.DataSource = DtDeliveryCentre;
                    grdDematStocks.DataBind();
                }
                grdDematStocks.DataSource = DtDeliveryCentre;
                grdDematStocks.DataBind();
                if (ddlAccountType.SelectedItem.Value == "A")
                {
                    grdDematStocks.Columns[3].Visible = true;
                    grdDematStocks.Columns[0].Visible = true;
                }
                else if (ddlAccountType.SelectedItem.Value == "P")
                {
                    grdDematStocks.Columns[3].Visible = true;
                    grdDematStocks.Columns[0].Visible = false;
                }
                else
                {
                    grdDematStocks.Columns[3].Visible = false;
                    grdDematStocks.Columns[0].Visible = false;
                }
            }
            else
            {

                if (rdbSettTypeAll.Checked == true && rdbSettTypeAll1.Checked == true)
                    SettlementType = null;
                else if (rdbSettTypeAll.Checked == true && rdbSettTypeAll1.Checked == false)
                    SettlementType = " and DematPosition_SettlementType='" + SettType + "'";
                else if (rdbSettTypeAll.Checked == false && rdbSettTypeAll1.Checked == true)
                    SettlementType = " and DematPosition_SettlementNumber='" + SettNum + "'";
                else
                    SettlementType = " and DematPosition_SettlementNumber='" + SettNum + "'  and DematPosition_SettlementType='" + SettType + "'";

                if (radExchange.Checked == true)
                {
                    ForClientExchange = " and DematPosition_CustomerID not like 'CL%'";
                    OrderBy = " order by Scrip";
                }
                else if (radBoth.Checked == true)
                {
                    ForClientExchange = null;
                    OrderBy = " order by Scrip,UCC desc";
                }
                else
                {
                    if (radAll.Checked == true)
                        ForClientExchange = " and DematPosition_CustomerID like 'CL%'";
                    else if (radPOAClient.Checked == true)
                        ForClientExchange = " and DematPosition_CustomerID in(select dpd_cntID from tbl_master_contactdpdetails where dpd_POA=1)";
                    else
                        ForClientExchange = " and DematPosition_CustomerID in(" + Clients + ")";
                    OrderBy = " order by Client,Scrip";
                }
                if (rdbScripsAll.Checked == true)
                    Scrip = null;
                else
                {
                    Scrip = " and DematPosition_ProductSeriesID in(" + Script + ")";
                }
                if (radSegSpecific.Checked == true)
                {
                    Segment = " and DematPosition_SegmentID=" + Session["usersegid"].ToString() + " ";
                }
                else if (radSegAll.Checked == true)
                {
                    string Allseg = null;
                    DataTable DtExchSegmentID = oDbEngine.GetDataTable("tbl_master_companyExchange", "exch_internalID", " exch_compID='" + Session["LastCompany"].ToString() + "' and exch_SegmentId in('CM','FO')");
                    if (DtExchSegmentID.Rows.Count > 0)
                    {
                        for (int p = 0; p < DtExchSegmentID.Rows.Count; p++)
                        {
                            if (Allseg == null)
                                Allseg = DtExchSegmentID.Rows[p][0].ToString();
                            else
                                Allseg = Allseg + "," + DtExchSegmentID.Rows[p][0].ToString();
                        }
                    }
                    Segment = " and DematPosition_SegmentID in(" + Allseg + ") ";
                }
                if (ddlType.SelectedItem.Value == "O")
                {
                    if (radIncoming.Checked == true)
                    {
                        if (radTransfered.Checked == true)
                            Nature = " where DematPosition_QtyToReceive is not null";
                        //Nature = " where isnull(cast(IncomingPending as numeric(28,6)),0)=0 and DematPosition_QtyToReceive is not null";
                        else if (radUntransfered.Checked == true)
                            Nature = " where isnull(cast(IncomingPending as numeric(28,6)),0)<>0";
                        else
                            Nature = " where  DematPosition_QtyToReceive is not null";
                    }
                    else if (radOutgoing.Checked == true)
                    {
                        if (radTransfered.Checked == true)
                            Nature = " where DematPosition_QtyToDeliver is not null";
                        //Nature = " where isnull(cast(OutgoingPending as numeric(28,6)),0)=0 and DematPosition_QtyToDeliver is not null";
                        else if (radUntransfered.Checked == true)
                            Nature = " where isnull(cast(OutgoingPending as numeric(28,6)),0)<>0";
                        else
                            Nature = " where DematPosition_QtyToDeliver is not null";
                    }
                    else
                    {
                        if (radTransfered.Checked == true)
                            Nature = " where isnull(cast(OutgoingPending as numeric(28,6)),0)+isnull(cast(IncomingPending as numeric(28,6)),0)=0";
                        else if (radUntransfered.Checked == true)
                            Nature = " where isnull(cast(IncomingPending as numeric(28,6)),0)+isnull(cast(OutgoingPending as numeric(28,6)),0)<>0";
                        else
                            Nature = null;
                    }
                    //DtDeliveryCentre = oDbEngine.GetDataTable("(select dematPosition_ID,(select branch_code from tbl_master_branch where branch_id=DematPosition_BranchID) as Branch,(select branch_description from tbl_master_branch where branch_id=DematPosition_BranchID) as BranchName,isnull((select isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'') from tbl_master_contact where cnt_internalID=DematPosition_CustomerID),DematPosition_CustomerID) as Client,(select isnull(rtrim(cnt_ucc),'') from tbl_master_contact where cnt_internalID=DematPosition_CustomerID ) as UCC,(select isnull(rtrim(Equity_TickerSymbol),'')+' ['+isnull(rtrim(Equity_Series),'')+']' from master_equity where Equity_SeriesID=DematPosition_ProductSeriesID) as Scrip,DematPosition_ISIN,DematPosition_SettlementNumber+' ['+DematPosition_SettlementType+']' as Settlement,case when isnull(DematPosition_QtyToReceive,0)=0 then null else cast(DematPosition_QtyToReceive as varchar) end as DematPosition_QtyToReceive,	case when isnull(DematPosition_BFOpeningQty,0)>0 then cast(isnull(DematPosition_QtyReceived,0)+isnull(DematPosition_BFOpeningQty,0) as varchar) else DematPosition_QtyReceived end as DematPosition_QtyReceived,	case when isnull(DematPosition_QtyToDeliver,0)=0 then null else cast(DematPosition_QtyToDeliver as varchar) end as DematPosition_QtyToDeliver,	case when isnull(DematPosition_BFOpeningQty,0)<0 then cast(isnull(DematPosition_QtyDelivered,0)+abs(isnull(DematPosition_BFOpeningQty,0)) as varchar) else DematPosition_QtyDelivered end as DematPosition_QtyDelivered,case when isnull(DematPosition_AuctionCFQty,0)=0 then null else cast(DematPosition_AuctionCFQty as varchar) end as DematPosition_AuctionCFQty,(isnull(DematPosition_QtyToReceive,0)-(isnull(DematPosition_QtyReceived,0)+case when isnull(DematPosition_BFOpeningQty,0)>0 then isnull(DematPosition_BFOpeningQty,0) else 0 end)) as IncomingPending,(isnull(DematPosition_QtyToDeliver,0)-(isnull(DematPosition_QtyDelivered,0)+case when isnull(DematPosition_BFOpeningQty,0)<0 then abs(isnull(DematPosition_BFOpeningQty,0)) else 0 end)) as OutgoingPending,DematPosition_ProductSeriesID from  Trans_DematPosition where DematPosition_Type='O' " + SettlementType + " " + ForClientExchange + " " + Scrip + ") as DD " + Nature + " " + OrderBy + "", " dematPosition_ID,Branch,BranchName,Client,UCC,Scrip,DematPosition_ISIN,Settlement,DematPosition_QtyToReceive,DematPosition_QtyReceived,DematPosition_QtyToDeliver,DematPosition_QtyDelivered,DematPosition_AuctionCFQty,case when (case when OutgoingPending<0 then (case when IncomingPending+abs(OutgoingPending)=0 then null else IncomingPending+abs(OutgoingPending) end)  when IncomingPending=0 then null 	when IncomingPending<0 then null else IncomingPending  end)<0 then null else 	(case when OutgoingPending<0 then (case when  cast(IncomingPending+abs(OutgoingPending) as varchar)='0.0000' then null else  cast(IncomingPending+abs(OutgoingPending) as varchar) end) when IncomingPending=0 then null when IncomingPending<0 then null else cast(IncomingPending as varchar) end) end as IncomingPending,case when (case when IncomingPending<0 then (case when abs(IncomingPending)+OutgoingPending=0 then null else abs(IncomingPending)+OutgoingPending end )  when OutgoingPending=0 then null when OutgoingPending<0 then null else OutgoingPending  end)<0 then null else (case when IncomingPending<0 then (case when cast(abs(IncomingPending)+OutgoingPending as varchar)='0.0000' then null else cast(abs(IncomingPending)+OutgoingPending as varchar) end) when OutgoingPending=0 then null when OutgoingPending<0 then null else cast(OutgoingPending as varchar) end) end as OutgoingPending,DematPosition_ProductSeriesID", null);
                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "1" || HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "4" || HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "15" || HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "19")
                    {
                        DtDeliveryCentre = oDbEngine.GetDataTable("(Select  dematPosition_ID,Branch,BranchName,Client,UCC,Scrip,DematPosition_ISIN,Settlement,DematPosition_QtyToReceive,DematPosition_QtyReceived,DematPosition_QtyToDeliver,DematPosition_QtyDelivered,DematPosition_AuctionCFQty,case when (case when OutgoingPending<0 then (case when IncomingPending+abs(OutgoingPending)=0 then null else IncomingPending+abs(OutgoingPending) end)  when IncomingPending=0 then null 	when IncomingPending<0 then null else IncomingPending  end)<0 then null else 	(case when OutgoingPending<0 then (case when  cast(IncomingPending+abs(OutgoingPending) as varchar)='0.0000' then null else  cast(IncomingPending+abs(OutgoingPending) as varchar) end) when IncomingPending=0 then null when IncomingPending<0 then null else cast(IncomingPending as varchar) end) end as IncomingPending,case when (case when IncomingPending<0 then (case when abs(IncomingPending)+OutgoingPending=0 then null else abs(IncomingPending)+OutgoingPending end )  when OutgoingPending=0 then null when OutgoingPending<0 then null else OutgoingPending  end)<0 then null else (case when IncomingPending<0 then (case when cast(abs(IncomingPending)+OutgoingPending as varchar)='0.0000' then null else cast(abs(IncomingPending)+OutgoingPending as varchar) end) when OutgoingPending=0 then null when OutgoingPending<0 then null else cast(OutgoingPending as varchar) end) end as OutgoingPending,DematPosition_ProductSeriesID,DematPosition_BranchID,DematPosition_CustomerID,(select top 1 case when dpd_poa=1 and dpd_accountType='Default' then 'G' when dpd_poa=0 and substring(dpd_dpcode,1,8) in(select exch_TMCode from tbl_master_companyExchange) and dpd_accountType='Default' then 'B' else 'N' end from tbl_master_contactdpdetails where dpd_cntId=DematPosition_CustomerID order by dpd_poa desc) as ColourType,(select exh_shortname+' - '+exch_segmentid from tbl_master_companyExchange,tbl_master_exchange where exch_exchID=exh_cntId and exch_internalID=DematPosition_SegmentID) as DematPosition_SegmentID,Equity_TickerCode,Equity_ExchSegmentID,Equity_TickerSymbol,(select top 1 phf_phoneNumber from tbl_master_phonefax where phf_cntID=DematPosition_CustomerID and phf_type='Mobile') as PhoneNumber,(select top 1 eml_email from tbl_master_email where eml_cntID=DematPosition_CustomerID) as Email  from (select dematPosition_ID,(select branch_code from tbl_master_branch where branch_id=DematPosition_BranchID) as Branch,	(select branch_description from tbl_master_branch where branch_id=DematPosition_BranchID) as BranchName,isnull((select isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'') from tbl_master_contact where cnt_internalID=DematPosition_CustomerID),DematPosition_CustomerID) as Client,(select isnull(rtrim(cnt_ucc),'') from tbl_master_contact where cnt_internalID=DematPosition_CustomerID) as UCC,isnull(rtrim(Equity_TickerSymbol),'')+' ['+isnull(rtrim(Equity_Series),isnull(rtrim(Equity_TickerCode),''))+']' as Scrip,DematPosition_ISIN,DematPosition_SettlementNumber+DematPosition_SettlementType as Settlement,case when isnull(DematPosition_QtyToReceive,0)=0 then null else cast(DematPosition_QtyToReceive as varchar) end as DematPosition_QtyToReceive,case when isnull(DematPosition_BFOpeningQty,0)>0 then cast(isnull(DematPosition_QtyReceived,0)+isnull(DematPosition_BFOpeningQty,0) as varchar) else DematPosition_QtyReceived end as DematPosition_QtyReceived,case when isnull(DematPosition_QtyToDeliver,0)=0 then null else cast(DematPosition_QtyToDeliver as varchar) end as DematPosition_QtyToDeliver,case when isnull(DematPosition_BFOpeningQty,0)<0 then cast(isnull(DematPosition_QtyDelivered,0)+abs(isnull(DematPosition_BFOpeningQty,0)) as varchar) else DematPosition_QtyDelivered end as DematPosition_QtyDelivered,case when isnull(DematPosition_AuctionCFQty,0)=0 then null else cast(DematPosition_AuctionCFQty as varchar) end as DematPosition_AuctionCFQty,(isnull(DematPosition_QtyToReceive,0)-(isnull(DematPosition_QtyReceived,0)+case when isnull(DematPosition_BFOpeningQty,0)>0 then isnull(DematPosition_BFOpeningQty,0) else 0 end)) as IncomingPending,(isnull(DematPosition_QtyToDeliver,0)-(isnull(DematPosition_QtyDelivered,0)+case when isnull(DematPosition_BFOpeningQty,0)<0 then abs(isnull(DematPosition_BFOpeningQty,0)) else 0 end)) as OutgoingPending,DematPosition_ProductSeriesID,DematPosition_BranchID,DematPosition_CustomerID,DematPosition_SegmentID,Equity_TickerCode,Equity_ExchSegmentID,Equity_TickerSymbol from  Trans_DematPosition,master_equity where DematPosition_Type='O'  " + SettlementType + " " + ForClientExchange + " " + Scrip + " " + Segment + " and equity_exchsegmentid in ('1','4','15','19') and DematPosition_FinYear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and Equity_SeriesID=DematPosition_ProductSeriesID and DematPosition_BranchID in(" + Session["userbranchHierarchy"].ToString() + ")) as DD ) as KK " + Nature + " " + OrderBy + "", " *", null);
                    }
                    else
                    {
                        DtDeliveryCentre = oDbEngine.GetDataTable("(Select  dematPosition_ID,Branch,BranchName,Client,UCC,Scrip,DematPosition_ISIN,Settlement,DematPosition_QtyToReceive,DematPosition_QtyReceived,DematPosition_QtyToDeliver,DematPosition_QtyDelivered,DematPosition_AuctionCFQty,case when (case when OutgoingPending<0 then (case when IncomingPending+abs(OutgoingPending)=0 then null else IncomingPending+abs(OutgoingPending) end)  when IncomingPending=0 then null 	when IncomingPending<0 then null else IncomingPending  end)<0 then null else 	(case when OutgoingPending<0 then (case when  cast(IncomingPending+abs(OutgoingPending) as varchar)='0.0000' then null else  cast(IncomingPending+abs(OutgoingPending) as varchar) end) when IncomingPending=0 then null when IncomingPending<0 then null else cast(IncomingPending as varchar) end) end as IncomingPending,case when (case when IncomingPending<0 then (case when abs(IncomingPending)+OutgoingPending=0 then null else abs(IncomingPending)+OutgoingPending end )  when OutgoingPending=0 then null when OutgoingPending<0 then null else OutgoingPending  end)<0 then null else (case when IncomingPending<0 then (case when cast(abs(IncomingPending)+OutgoingPending as varchar)='0.0000' then null else cast(abs(IncomingPending)+OutgoingPending as varchar) end) when OutgoingPending=0 then null when OutgoingPending<0 then null else cast(OutgoingPending as varchar) end) end as OutgoingPending,DematPosition_ProductSeriesID,DematPosition_BranchID,DematPosition_CustomerID,(select top 1 case when dpd_poa=1 and dpd_accountType='Default' then 'G' when dpd_poa=0 and substring(dpd_dpcode,1,8) in(select exch_TMCode from tbl_master_companyExchange) and dpd_accountType='Default' then 'B' else 'N' end from tbl_master_contactdpdetails where dpd_cntId=DematPosition_CustomerID order by dpd_poa desc) as ColourType,(select exh_shortname+' - '+exch_segmentid from tbl_master_companyExchange,tbl_master_exchange where exch_exchID=exh_cntId and exch_internalID=DematPosition_SegmentID) as DematPosition_SegmentID,Equity_TickerCode,Equity_ExchSegmentID,Equity_TickerSymbol,(select top 1 phf_phoneNumber from tbl_master_phonefax where phf_cntID=DematPosition_CustomerID and phf_type='Mobile') as PhoneNumber,(select top 1 eml_email from tbl_master_email where eml_cntID=DematPosition_CustomerID) as Email  from (select dematPosition_ID,(select branch_code from tbl_master_branch where branch_id=DematPosition_BranchID) as Branch,	(select branch_description from tbl_master_branch where branch_id=DematPosition_BranchID) as BranchName,isnull((select isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'') from tbl_master_contact where cnt_internalID=DematPosition_CustomerID),DematPosition_CustomerID) as Client,(select isnull(rtrim(cnt_ucc),'') from tbl_master_contact where cnt_internalID=DematPosition_CustomerID) as UCC,isnull(rtrim(Equity_TickerSymbol),'')+' ['+isnull(rtrim(Equity_Series),isnull(rtrim(Equity_TickerCode),''))+']' as Scrip,DematPosition_ISIN,DematPosition_SettlementNumber+DematPosition_SettlementType as Settlement,case when isnull(DematPosition_QtyToReceive,0)=0 then null else cast(DematPosition_QtyToReceive as varchar) end as DematPosition_QtyToReceive,case when isnull(DematPosition_BFOpeningQty,0)>0 then cast(isnull(DematPosition_QtyReceived,0)+isnull(DematPosition_BFOpeningQty,0) as varchar) else DematPosition_QtyReceived end as DematPosition_QtyReceived,case when isnull(DematPosition_QtyToDeliver,0)=0 then null else cast(DematPosition_QtyToDeliver as varchar) end as DematPosition_QtyToDeliver,case when isnull(DematPosition_BFOpeningQty,0)<0 then cast(isnull(DematPosition_QtyDelivered,0)+abs(isnull(DematPosition_BFOpeningQty,0)) as varchar) else DematPosition_QtyDelivered end as DematPosition_QtyDelivered,case when isnull(DematPosition_AuctionCFQty,0)=0 then null else cast(DematPosition_AuctionCFQty as varchar) end as DematPosition_AuctionCFQty,(isnull(DematPosition_QtyToReceive,0)-(isnull(DematPosition_QtyReceived,0)+case when isnull(DematPosition_BFOpeningQty,0)>0 then isnull(DematPosition_BFOpeningQty,0) else 0 end)) as IncomingPending,(isnull(DematPosition_QtyToDeliver,0)-(isnull(DematPosition_QtyDelivered,0)+case when isnull(DematPosition_BFOpeningQty,0)<0 then abs(isnull(DematPosition_BFOpeningQty,0)) else 0 end)) as OutgoingPending,DematPosition_ProductSeriesID,DematPosition_BranchID,DematPosition_CustomerID,DematPosition_SegmentID,Equity_TickerCode,Equity_ExchSegmentID,Equity_TickerSymbol from  Trans_DematPosition,master_equity where DematPosition_Type='O'  " + SettlementType + " " + ForClientExchange + " " + Scrip + " " + Segment + " and DematPosition_FinYear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and Equity_SeriesID=DematPosition_ProductSeriesID and DematPosition_BranchID in(" + Session["userbranchHierarchy"].ToString() + ")) as DD ) as KK " + Nature + " " + OrderBy + "", " *", null);
                    }
                }
                else if (ddlType.SelectedItem.Value == "M")
                {
                    if (radAll.Checked == true)
                        ForClientExchange = " and DematPosition_CustomerID like 'CL%'";
                    else
                        ForClientExchange = " and DematPosition_CustomerID in(" + Clients + ")";
                    if (radIncoming.Checked == true)
                    {
                        if (radTransfered.Checked == true)
                            //Nature = " where isnull(cast(IncomingPending as numeric(28,6)),0)<>0 ";
                            Nature = " where isnull(cast(IncomingPending as numeric(28,6)),0)=0 and DematPosition_QtyToReceive is not null";
                        else if (radUntransfered.Checked == true)
                            Nature = " where isnull(cast(IncomingPending as numeric(28,6)),0)<>0";
                        else
                            Nature = " where  DematPosition_QtyToReceive is not null";
                    }
                    else
                        if (radOutgoing.Checked == true)
                        {
                            if (radTransfered.Checked == true)
                                //Nature = " where isnull(cast(OutgoingPending as numeric(28,6)),0)<>0";
                                Nature = " where isnull(cast(OutgoingPending as numeric(28,6)),0)=0 and DematPosition_QtyToDeliver is not null";
                            else if (radUntransfered.Checked == true)
                                Nature = " where isnull(cast(OutgoingPending as numeric(28,6)),0)<>0";
                            else
                                Nature = " where DematPosition_QtyToDeliver is not null";
                        }
                        else
                        {
                            if (radTransfered.Checked == true)
                                Nature = " where isnull(cast(OutgoingPending as numeric(28,6)),0)+isnull(cast(IncomingPending as numeric(28,6)),0)=0";
                            else if (radUntransfered.Checked == true)
                                Nature = " where isnull(cast(IncomingPending as numeric(28,6)),0)+isnull(cast(OutgoingPending as numeric(28,6)),0)<>0";
                            else
                                Nature = null;
                        }
                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "1" || HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "4" || HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "15" || HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "19")
                    {
                        DtDeliveryCentre = oDbEngine.GetDataTable("(select dematPosition_ID,Branch,BranchName,Client,UCC,Scrip,DematPosition_ISIN,Settlement,case when isnull(cast(DematPosition_QtyToReceive as numeric(28,6)),0)=0 then null else cast(DematPosition_QtyToReceive as varchar) end as DematPosition_QtyToReceive,case when isnull(cast(DematPosition_QtyReceived as numeric(28,6)),0)=0 then null else cast(DematPosition_QtyReceived as varchar) end as DematPosition_QtyReceived,case when isnull(cast(DematPosition_QtyToDeliver as numeric(28,6)),0)=0 then null else cast(DematPosition_QtyToDeliver as varchar) end as DematPosition_QtyToDeliver,case when isnull(cast(DematPosition_QtyDelivered as numeric(28,6)),0)=0 then null else cast(DematPosition_QtyDelivered as varchar) end as DematPosition_QtyDelivered,case when isnull(cast(DematPosition_AuctionCFQty as numeric(28,6)),0)=0 then null else cast(DematPosition_AuctionCFQty as varchar) end as DematPosition_AuctionCFQty,case when (case when cast(OutgoingPending as numeric(28,6))<0 then cast(IncomingPending as numeric(28,6))+abs(cast(OutgoingPending as numeric(28,6)))  when cast(IncomingPending as numeric(28,6))=0 then null when cast(IncomingPending as numeric(28,6))<0 then null else cast(IncomingPending as numeric(28,6))  end)<0 then null else (case when cast(OutgoingPending as numeric(28,6))<0 then cast(cast(IncomingPending as numeric(28,6))+abs(cast(OutgoingPending as numeric(28,6))) as varchar) when cast(IncomingPending as numeric(28,6))=0 then null when cast(IncomingPending as numeric(28,6))<0 then null else cast(IncomingPending as varchar) end) end as IncomingPending,case when (case when cast(IncomingPending as numeric(28,6))<0 then abs(cast(IncomingPending as numeric(28,6)))+cast(OutgoingPending as numeric(28,6))  when cast(OutgoingPending as numeric(28,6))=0 then null when cast(OutgoingPending as numeric(28,6))<0 then null else cast(OutgoingPending as numeric(28,6))  end)<0 then null else (case when cast(IncomingPending as numeric(28,6))<0 then cast(abs(cast(IncomingPending as numeric(28,6)))+cast(OutgoingPending as numeric(28,6)) as varchar) when cast(OutgoingPending as numeric(28,6))=0 then null when cast(OutgoingPending as numeric(28,6))<0 then null else cast(OutgoingPending as varchar) end) end as OutgoingPending,DematPosition_ProductSeriesID,DematPosition_BranchID,DematPosition_CustomerID,(select top 1 case when dpd_poa=1 and dpd_accountType='Default' then 'G' when dpd_poa=0 and substring(dpd_dpcode,1,8) in(select exch_TMCode from tbl_master_companyExchange) and dpd_accountType='Default' then 'B' else 'N' end from tbl_master_contactdpdetails where dpd_cntId=DematPosition_CustomerID order by dpd_poa desc) as ColourType,(select exh_shortname+' - '+exch_segmentid from tbl_master_companyExchange,tbl_master_exchange where exch_exchID=exh_cntId and exch_internalID=DematPosition_SegmentID) as DematPosition_SegmentID,Equity_TickerCode,Equity_ExchSegmentID,Equity_TickerSymbol,(select top 1 phf_phoneNumber from tbl_master_phonefax where phf_cntID=DematPosition_CustomerID and phf_type='Mobile') as PhoneNumber,(select top 1 eml_email from tbl_master_email where eml_cntID=DematPosition_CustomerID) as Email from (select dematPosition_ID,(select branch_code from tbl_master_branch where branch_id=DematPosition_BranchID) as Branch,(select branch_description from tbl_master_branch where branch_id=DematPosition_BranchID) as BranchName,isnull((select isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'') from tbl_master_contact where cnt_internalID=DematPosition_CustomerID),DematPosition_CustomerID) as Client,(select isnull(rtrim(cnt_ucc),'') from tbl_master_contact where cnt_internalID=DematPosition_CustomerID) as UCC,isnull(rtrim(Equity_TickerSymbol),'')+' ['+isnull(rtrim(Equity_Series),isnull(rtrim(Equity_TickerCode),''))+']' as Scrip,DematPosition_ISIN,DematPosition_SettlementNumber+DematPosition_SettlementType as Settlement,case when isnull(DematPosition_QtyToReceive,0)=0 then null else cast(DematPosition_QtyToReceive as varchar) end as DematPosition_QtyToReceive,case when isnull(DematPosition_BFOpeningQty,0)>0 then cast(isnull(DematPosition_QtyReceived,0)+isnull(DematPosition_BFOpeningQty,0) as varchar) else DematPosition_QtyReceived end as DematPosition_QtyReceived,case when isnull(DematPosition_QtyToDeliver,0)=0 then null else cast(DematPosition_QtyToDeliver as varchar) end as DematPosition_QtyToDeliver,case when isnull(DematPosition_BFOpeningQty,0)<0 then cast(isnull(DematPosition_QtyDelivered,0)+abs(isnull(DematPosition_BFOpeningQty,0)) as varchar) else DematPosition_QtyDelivered end as DematPosition_QtyDelivered,case when isnull(DematPosition_AuctionCFQty,0)=0 then null else cast(DematPosition_AuctionCFQty as varchar) end as DematPosition_AuctionCFQty,(isnull(DematPosition_QtyToReceive,0)-(isnull(DematPosition_QtyReceived,0)+case when isnull(DematPosition_BFOpeningQty,0)>0 then isnull(DematPosition_BFOpeningQty,0) else 0 end)) as IncomingPending,(isnull(DematPosition_QtyToDeliver,0)-(isnull(DematPosition_QtyDelivered,0)+case when isnull(DematPosition_BFOpeningQty,0)<0 then abs(isnull(DematPosition_BFOpeningQty,0)) else 0 end)) as OutgoingPending,DematPosition_ProductSeriesID,DematPosition_BranchID,DematPosition_CustomerID,DematPosition_SegmentID,Equity_TickerCode,Equity_ExchSegmentID,Equity_TickerSymbol  from  Trans_DematPosition,master_equity  where DematPosition_Type in('M','H')  " + ForClientExchange + " " + Scrip + " " + Segment + " and DematPosition_FinYear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and equity_exchsegmentid in ('1','4') and Equity_SeriesID=DematPosition_ProductSeriesID and DematPosition_BranchID in(" + Session["userbranchHierarchy"].ToString() + ")) as DD) as PP " + Nature + " " + OrderBy + "", " *", null);
                    }
                    else
                    {
                        DtDeliveryCentre = oDbEngine.GetDataTable("(select dematPosition_ID,Branch,BranchName,Client,UCC,Scrip,DematPosition_ISIN,Settlement,case when isnull(cast(DematPosition_QtyToReceive as numeric(28,6)),0)=0 then null else cast(DematPosition_QtyToReceive as varchar) end as DematPosition_QtyToReceive,case when isnull(cast(DematPosition_QtyReceived as numeric(28,6)),0)=0 then null else cast(DematPosition_QtyReceived as varchar) end as DematPosition_QtyReceived,case when isnull(cast(DematPosition_QtyToDeliver as numeric(28,6)),0)=0 then null else cast(DematPosition_QtyToDeliver as varchar) end as DematPosition_QtyToDeliver,case when isnull(cast(DematPosition_QtyDelivered as numeric(28,6)),0)=0 then null else cast(DematPosition_QtyDelivered as varchar) end as DematPosition_QtyDelivered,case when isnull(cast(DematPosition_AuctionCFQty as numeric(28,6)),0)=0 then null else cast(DematPosition_AuctionCFQty as varchar) end as DematPosition_AuctionCFQty,case when (case when cast(OutgoingPending as numeric(28,6))<0 then cast(IncomingPending as numeric(28,6))+abs(cast(OutgoingPending as numeric(28,6)))  when cast(IncomingPending as numeric(28,6))=0 then null when cast(IncomingPending as numeric(28,6))<0 then null else cast(IncomingPending as numeric(28,6))  end)<0 then null else (case when cast(OutgoingPending as numeric(28,6))<0 then cast(cast(IncomingPending as numeric(28,6))+abs(cast(OutgoingPending as numeric(28,6))) as varchar) when cast(IncomingPending as numeric(28,6))=0 then null when cast(IncomingPending as numeric(28,6))<0 then null else cast(IncomingPending as varchar) end) end as IncomingPending,case when (case when cast(IncomingPending as numeric(28,6))<0 then abs(cast(IncomingPending as numeric(28,6)))+cast(OutgoingPending as numeric(28,6))  when cast(OutgoingPending as numeric(28,6))=0 then null when cast(OutgoingPending as numeric(28,6))<0 then null else cast(OutgoingPending as numeric(28,6))  end)<0 then null else (case when cast(IncomingPending as numeric(28,6))<0 then cast(abs(cast(IncomingPending as numeric(28,6)))+cast(OutgoingPending as numeric(28,6)) as varchar) when cast(OutgoingPending as numeric(28,6))=0 then null when cast(OutgoingPending as numeric(28,6))<0 then null else cast(OutgoingPending as varchar) end) end as OutgoingPending,DematPosition_ProductSeriesID,DematPosition_BranchID,DematPosition_CustomerID,(select top 1 case when dpd_poa=1 and dpd_accountType='Default' then 'G' when dpd_poa=0 and substring(dpd_dpcode,1,8) in(select exch_TMCode from tbl_master_companyExchange) and dpd_accountType='Default' then 'B' else 'N' end from tbl_master_contactdpdetails where dpd_cntId=DematPosition_CustomerID order by dpd_poa desc) as ColourType,(select exh_shortname+' - '+exch_segmentid from tbl_master_companyExchange,tbl_master_exchange where exch_exchID=exh_cntId and exch_internalID=DematPosition_SegmentID) as DematPosition_SegmentID,Equity_TickerCode,Equity_ExchSegmentID,Equity_TickerSymbol,(select top 1 phf_phoneNumber from tbl_master_phonefax where phf_cntID=DematPosition_CustomerID and phf_type='Mobile') as PhoneNumber,(select top 1 eml_email from tbl_master_email where eml_cntID=DematPosition_CustomerID) as Email from (select dematPosition_ID,(select branch_code from tbl_master_branch where branch_id=DematPosition_BranchID) as Branch,(select branch_description from tbl_master_branch where branch_id=DematPosition_BranchID) as BranchName,isnull((select isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'') from tbl_master_contact where cnt_internalID=DematPosition_CustomerID),DematPosition_CustomerID) as Client,(select isnull(rtrim(cnt_ucc),'') from tbl_master_contact where cnt_internalID=DematPosition_CustomerID) as UCC,isnull(rtrim(Equity_TickerSymbol),'')+' ['+isnull(rtrim(Equity_Series),isnull(rtrim(Equity_TickerCode),''))+']' as Scrip,DematPosition_ISIN,DematPosition_SettlementNumber+DematPosition_SettlementType as Settlement,case when isnull(DematPosition_QtyToReceive,0)=0 then null else cast(DematPosition_QtyToReceive as varchar) end as DematPosition_QtyToReceive,case when isnull(DematPosition_BFOpeningQty,0)>0 then cast(isnull(DematPosition_QtyReceived,0)+isnull(DematPosition_BFOpeningQty,0) as varchar) else DematPosition_QtyReceived end as DematPosition_QtyReceived,case when isnull(DematPosition_QtyToDeliver,0)=0 then null else cast(DematPosition_QtyToDeliver as varchar) end as DematPosition_QtyToDeliver,case when isnull(DematPosition_BFOpeningQty,0)<0 then cast(isnull(DematPosition_QtyDelivered,0)+abs(isnull(DematPosition_BFOpeningQty,0)) as varchar) else DematPosition_QtyDelivered end as DematPosition_QtyDelivered,case when isnull(DematPosition_AuctionCFQty,0)=0 then null else cast(DematPosition_AuctionCFQty as varchar) end as DematPosition_AuctionCFQty,(isnull(DematPosition_QtyToReceive,0)-(isnull(DematPosition_QtyReceived,0)+case when isnull(DematPosition_BFOpeningQty,0)>0 then isnull(DematPosition_BFOpeningQty,0) else 0 end)) as IncomingPending,(isnull(DematPosition_QtyToDeliver,0)-(isnull(DematPosition_QtyDelivered,0)+case when isnull(DematPosition_BFOpeningQty,0)<0 then abs(isnull(DematPosition_BFOpeningQty,0)) else 0 end)) as OutgoingPending,DematPosition_ProductSeriesID,DematPosition_BranchID,DematPosition_CustomerID,DematPosition_SegmentID,Equity_TickerCode,Equity_ExchSegmentID,Equity_TickerSymbol  from  Trans_DematPosition,master_equity  where DematPosition_Type in('M','H')  " + ForClientExchange + " " + Scrip + " " + Segment + " and DematPosition_FinYear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and Equity_SeriesID=DematPosition_ProductSeriesID and DematPosition_BranchID in(" + Session["userbranchHierarchy"].ToString() + ")) as DD) as PP " + Nature + " " + OrderBy + "", " *", null);
                    }
                }
                if (DtDeliveryCentre.Rows.Count > 0)
                {
                    for (int i = 0; i < DtDeliveryCentre.Rows.Count; i++)
                    {
                        if (DtDeliveryCentre.Rows[i]["DematPosition_QtyToReceive"] != DBNull.Value)
                            DtDeliveryCentre.Rows[i]["DematPosition_QtyToReceive"] = oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(DtDeliveryCentre.Rows[i]["DematPosition_QtyToReceive"]));
                        if (DtDeliveryCentre.Rows[i]["DematPosition_QtyReceived"] != DBNull.Value)
                            DtDeliveryCentre.Rows[i]["DematPosition_QtyReceived"] = oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(DtDeliveryCentre.Rows[i]["DematPosition_QtyReceived"]));
                        if (DtDeliveryCentre.Rows[i]["DematPosition_QtyToDeliver"] != DBNull.Value)
                            DtDeliveryCentre.Rows[i]["DematPosition_QtyToDeliver"] = oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(DtDeliveryCentre.Rows[i]["DematPosition_QtyToDeliver"]));
                        if (DtDeliveryCentre.Rows[i]["DematPosition_QtyDelivered"] != DBNull.Value)
                            DtDeliveryCentre.Rows[i]["DematPosition_QtyDelivered"] = oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(DtDeliveryCentre.Rows[i]["DematPosition_QtyDelivered"]));
                        if (DtDeliveryCentre.Rows[i]["IncomingPending"] != DBNull.Value)
                            DtDeliveryCentre.Rows[i]["IncomingPending"] = oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(DtDeliveryCentre.Rows[i]["IncomingPending"]));
                        if (DtDeliveryCentre.Rows[i]["OutgoingPending"] != DBNull.Value)
                            DtDeliveryCentre.Rows[i]["OutgoingPending"] = oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(DtDeliveryCentre.Rows[i]["OutgoingPending"]));
                    }

                    grdDematCentre.DataSource = DtDeliveryCentre;
                    grdDematCentre.DataBind();
                    ViewState["ExDataset"] = DtDeliveryCentre;

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct1123", "alert('No Record Found !!')", true); ;
                    grdDematCentre.DataSource = DtDeliveryCentre;
                    grdDematCentre.DataBind();
                    ViewState["ExDataset"] = DtDeliveryCentre;
                }
                if (radIncoming.Checked == true)
                {
                    grdDematCentre.Columns[8].Visible = true;
                    grdDematCentre.Columns[9].Visible = true;
                    grdDematCentre.Columns[10].Visible = false;
                    grdDematCentre.Columns[11].Visible = false;
                    grdDematCentre.Columns[12].Visible = true;
                    grdDematCentre.Columns[13].Visible = false;
                    grdDematCentre.Columns[14].Visible = false;
                    grdDematCentre.Columns[16].Visible = false;
                }
                else if (radOutgoing.Checked == true)
                {
                    grdDematCentre.Columns[8].Visible = false;
                    grdDematCentre.Columns[9].Visible = false;
                    grdDematCentre.Columns[10].Visible = true;
                    grdDematCentre.Columns[11].Visible = true;
                    grdDematCentre.Columns[12].Visible = false;
                    grdDematCentre.Columns[13].Visible = true;
                    grdDematCentre.Columns[14].Visible = false;
                    grdDematCentre.Columns[16].Visible = false;
                }
                else if (radMovBoth.Checked == true)
                {
                    grdDematCentre.Columns[8].Visible = true;
                    grdDematCentre.Columns[9].Visible = true;
                    grdDematCentre.Columns[10].Visible = true;
                    grdDematCentre.Columns[11].Visible = true;
                    grdDematCentre.Columns[12].Visible = true;
                    grdDematCentre.Columns[13].Visible = true;
                    grdDematCentre.Columns[14].Visible = false;
                    grdDematCentre.Columns[16].Visible = false;
                }
            }
        }

        protected void ddlExport_SelectedIndexChanged1(object sender, EventArgs e)
        {
            export();
        }

        void export()
        {
            DataTable dtEx = new DataTable();
            DataTable dtMain = (DataTable)ViewState["ExDataset"];
            if (ddlType.SelectedItem.Value == "S")
            {
                dtEx.Columns.Add("Scrip");
                dtEx.Columns.Add("ISIN");
                dtEx.Columns.Add("Opening Qty");
                dtEx.Columns.Add("In Qty");
                dtEx.Columns.Add("Out Qty");
                dtEx.Columns.Add("Pledge Qty");
                dtEx.Columns.Add("Blocked");
                dtEx.Columns.Add("Free Balance");

                for (int i = 0; i < dtMain.Rows.Count; i++)
                {
                    DataRow newRow = dtEx.NewRow();
                    newRow["Scrip"] = dtMain.Rows[i]["Scrip"].ToString();
                    newRow["ISIN"] = dtMain.Rows[i]["DematStocks_ISIN"].ToString();
                    newRow["Opening Qty"] = dtMain.Rows[i]["OpeningQty"].ToString();
                    newRow["In Qty"] = dtMain.Rows[i]["Inqty"].ToString();
                    newRow["Out Qty"] = dtMain.Rows[i]["OutQty"].ToString();
                    newRow["Pledge Qty"] = dtMain.Rows[i]["PledgeQty"].ToString();
                    newRow["Blocked"] = dtMain.Rows[i]["Blocked"].ToString();
                    newRow["Free Balance"] = dtMain.Rows[i]["FreeBalance"].ToString();

                    dtEx.Rows.Add(newRow);
                }


            }
            else
            {
                dtEx.Columns.Add("Segment");
                dtEx.Columns.Add("Client");
                dtEx.Columns.Add("UCC");
                dtEx.Columns.Add("Branch");
                dtEx.Columns.Add("Settlement");
                dtEx.Columns.Add("Scrip");
                dtEx.Columns.Add("ISIN");
                dtEx.Columns.Add("QtyToRecieve");
                dtEx.Columns.Add("QtyRecieved");
                dtEx.Columns.Add("QtyToDeliver");
                dtEx.Columns.Add("Qty Delivered");
                dtEx.Columns.Add("Pending Incoming");
                dtEx.Columns.Add("Pending Outgoing");
                for (int i = 0; i < dtMain.Rows.Count; i++)
                {
                    DataRow newRow = dtEx.NewRow();
                    newRow["Segment"] = dtMain.Rows[i]["DematPosition_SegmentID"].ToString();
                    newRow["Client"] = dtMain.Rows[i]["Client"].ToString();
                    newRow["UCC"] = dtMain.Rows[i]["UCC"].ToString();
                    newRow["Branch"] = dtMain.Rows[i]["Branch"].ToString();
                    newRow["Settlement"] = dtMain.Rows[i]["Settlement"].ToString();
                    newRow["Scrip"] = dtMain.Rows[i]["Scrip"].ToString();
                    newRow["ISIN"] = dtMain.Rows[i]["DematPosition_ISIN"].ToString();
                    newRow["QtyToRecieve"] = dtMain.Rows[i]["DematPosition_QtyToReceive"].ToString();
                    newRow["QtyRecieved"] = dtMain.Rows[i]["DematPosition_QtyReceived"].ToString();
                    newRow["QtyToDeliver"] = dtMain.Rows[i]["DematPosition_QtyToDeliver"].ToString();
                    newRow["Qty Delivered"] = dtMain.Rows[i]["DematPosition_QtyDelivered"].ToString();
                    newRow["Pending Incoming"] = dtMain.Rows[i]["IncomingPending"].ToString();
                    newRow["Pending Outgoing"] = dtMain.Rows[i]["OutgoingPending"].ToString();
                    dtEx.Rows.Add(newRow);
                }

            }
            DataTable dtReportHeader = new DataTable();
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            HeaderRow[0] = "Delivery Center";
            dtReportHeader.Rows.Add(HeaderRow);



            DataTable dtReportFooter = new DataTable();
            dtReportFooter.Columns.Add(new DataColumn("Footer", typeof(String))); //0
            DataRow FooterRow = dtReportFooter.NewRow();
            FooterRow[0] = "* * *  End Of Report * * *   ";
            dtReportFooter.Rows.Add(FooterRow);



            if (ddlExport.SelectedItem.Value == "E")
            {
                objExcel.ExportToExcelforExcel(dtEx, "DematCenter", "Branch/Group Total", dtReportHeader, dtReportFooter);

            }

        }
        protected void grdDematCentre_RowCreated(object sender, GridViewRowEventArgs e)
        {
            string rowID = String.Empty;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Label lblUCC = (Label)e.Row.FindControl("lblUCC");
                rowID = "row" + e.Row.RowIndex;
                int ID = Convert.ToInt32(e.Row.RowIndex) + 1;
                e.Row.Attributes.Add("id", "row" + e.Row.RowIndex);
                e.Row.Attributes.Add("onclick", "ChangeRowColor(" + "'" + rowID + "','" + DtDeliveryCentre.Rows.Count + "','" + ID + "'" + ")");
            }
        }
        protected void grdDematStocks_RowCreated(object sender, GridViewRowEventArgs e)
        {
            string rowID = String.Empty;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                rowID = "row" + e.Row.RowIndex;
                int ID = Convert.ToInt32(e.Row.RowIndex) + 1;
                e.Row.Attributes.Add("id", "row" + e.Row.RowIndex);
                e.Row.Attributes.Add("onclick", "ChangeRowColor1(" + "'" + rowID + "','" + grdDematStocks.Rows.Count + "','" + ID + "'" + ")");
            }
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            FillGrid();
        }
        protected void grdDematCentre_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = e.SortExpression;
            if (GridViewSortDirection == SortDirection.Ascending)
            {
                GridViewSortDirection = SortDirection.Descending;
                SortGridView(sortExpression, " DESC");
            }
            else
            {
                GridViewSortDirection = SortDirection.Ascending;
                SortGridView(sortExpression, " ASC");
            }
        }
        public SortDirection GridViewSortDirection
        {
            get
            {
                if (ViewState["sortDirection"] == null)
                    ViewState["sortDirection"] = SortDirection.Ascending;
                return (SortDirection)ViewState["sortDirection"];
            }
            set { ViewState["sortDirection"] = value; }

        }
        private void SortGridView(string sortExpression, string direction)
        {
            DataView dv = new DataView(DtDeliveryCentre);
            dv.Sort = sortExpression + direction;
            grdDematCentre.DataSource = dv;
            grdDematCentre.DataBind();

        }
        protected void grdDematCentre_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow row = e.Row;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblColourType = (Label)row.FindControl("lblColourType");
                if (lblColourType.Text == "G")
                {
                    e.Row.Cells[2].Style.Add("color", "green");
                    e.Row.Cells[2].Font.Bold = true;
                }
                else if (lblColourType.Text == "B")
                {
                    e.Row.Cells[2].Style.Add("color", "blue");
                    e.Row.Cells[2].Font.Bold = true;
                }
                Label lblTickerCode = (Label)e.Row.FindControl("lblTickerCode");
                Label lblExchSegmentID = (Label)e.Row.FindControl("lblExchSegmentID");
                Label lblTickerSymbol = (Label)e.Row.FindControl("lblTickerSymbol");

                Label CustomerID = (Label)e.Row.FindControl("lblDematPosition_CustomerID");
                Label lblProID = (Label)e.Row.FindControl("lblProID");
                Label lblSettlement = (Label)e.Row.FindControl("lblSettlement");
                Label lblID = (Label)e.Row.FindControl("lblID");
                Label lblClient = (Label)e.Row.FindControl("lblClient");
                Label lblUCC = (Label)e.Row.FindControl("lblUCC");

                Label lblPhoneNumber = (Label)e.Row.FindControl("lblPhoneNumber");
                Label lblEmail = (Label)e.Row.FindControl("lblEmail");

                string SettNumber = lblSettlement.Text.Substring(0, 7);
                string SettType = lblSettlement.Text.Substring(7);

                ((Label)e.Row.FindControl("lblScrip")).Attributes.Add("onclick", "javascript:ShowMarketTraker('" + lblTickerCode.Text + "','" + lblExchSegmentID.Text + "','" + lblTickerSymbol.Text + "');");
                e.Row.Cells[6].Style.Add("cursor", "hand");
                e.Row.Cells[6].ToolTip = "Click to Show Market Traker!";

                ((Label)e.Row.FindControl("lblSettlement")).Attributes.Add("onclick", "javascript:TradeRegister('" + CustomerID.Text + "','" + lblProID.Text + "','" + SettNumber + "','" + SettType + "','" + lblExchSegmentID.Text + "','" + lblID.Text + "','" + lblClient.Text + "','" + lblUCC.Text + "');");
                e.Row.Cells[5].Style.Add("cursor", "hand");
                e.Row.Cells[5].ToolTip = "Click to Show Trade Register!";

                if (lblPhoneNumber.Text != "" && lblEmail.Text != "")
                    e.Row.Cells[2].ToolTip = "Phone No: " + lblPhoneNumber.Text + " Email: " + lblEmail.Text;
                else if (lblPhoneNumber.Text != "" && lblEmail.Text == "")
                    e.Row.Cells[2].ToolTip = "Phone No: " + lblPhoneNumber.Text;
                if (lblPhoneNumber.Text == "" && lblEmail.Text != "")
                    e.Row.Cells[2].ToolTip = " Email: " + lblEmail.Text;

            }
        }
        protected void grdDematStocks_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblETickerCode = (Label)e.Row.FindControl("lblETickerCode");
                Label lblEExchSegmentID = (Label)e.Row.FindControl("lblEExchSegmentID");
                Label lblETickerSymbol = (Label)e.Row.FindControl("lblETickerSymbol");

                ((Label)e.Row.FindControl("lblScrip")).Attributes.Add("onclick", "javascript:ShowMarketTraker('" + lblETickerCode.Text + "','" + lblEExchSegmentID.Text + "','" + lblETickerSymbol.Text + "');");
                e.Row.Cells[1].Style.Add("cursor", "hand");
                e.Row.Cells[1].ToolTip = "Click to Show Market Traker!";
            }
        }
    }
}