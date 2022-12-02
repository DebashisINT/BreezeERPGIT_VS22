using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_Transaction : System.Web.UI.Page
    {
        DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        Converter oconverter = new Converter();
        protected void Page_Load(object sender, EventArgs e)
        {
            ShowTransaction();
        }
        public void ShowTransaction()
        {
            string ID = Request.QueryString["id"].ToString();
            if (Request.QueryString["ISIN"] == null)
            {
                DataTable dtTransaction = oDBEngine.GetDataTable("(select dematTransactions_ID as TranID,convert(varchar(12),cast(DematTransactions_Date as datetime),113) as Date,DematTransactions_Quantity as Quantity,case when DematTransactions_OwnAccounts is null then null else(select DPAccounts_ShortName from master_dpaccounts where DPAccounts_ID=DematTransactions_OwnAccounts) end as DematTransactions_OwnAccounts,case when DematTransactions_CustomerAccounts is null then null else(select isnull((select rtrim(replace(dp_dpname,char(160),'')) from tbl_master_depositoryparticipants where substring(dp_dpID,1,8)=dpd_dpCode),'')+' '+isnull(rtrim(dpd_ClientID),'') from tbl_master_contactdpdetails where dpd_id=DematTransactions_CustomerAccounts) end as DematTransactions_CustomerAccounts,case when DematTransactions_OwnAccountT is null then null else(select DPAccounts_ShortName from master_dpaccounts where DPAccounts_ID=DematTransactions_OwnAccountT) end as DematTransactions_OwnAccountT,case when DematTransactions_CustomerAccountT is null then null else(select isnull((select rtrim(replace(dp_dpname,char(160),'')) from tbl_master_depositoryparticipants where substring(dp_dpID,1,8)=dpd_dpCode),'')+' '+isnull(rtrim(dpd_ClientID),'') from tbl_master_contactdpdetails where dpd_id=DematTransactions_CustomerAccountT) end as DematTransactions_CustomerAccountT,DematTransactions_DPTransactionReference as DematTransactions_SlipNumber,isnull(DematTransactions_SettlementNumberS,'')+isnull(DematTransactions_SettlementTypeS,'') as DematTransactions_SettlementNumberS,isnull(DematTransactions_SettlementNumberT,'')+isnull(DematTransactions_SettlementTypeT,'') as DematTransactions_SettlementNumberT,DematTransactions_Remarks,(case dematTransactions_Type when 'MI' then 'Market-PayIN' when 'MO' then 'Market-PayOut' when 'CI' then 'Client-PayIn' when 'CO' then 'Client-PayOut' when 'IS' then 'Inter-Settlement' when 'LN' then 'Loan' when 'MG' then 'Margin' when 'HB' then 'HoldBack' when 'PL' then 'Pledge' when 'PU' then 'Un-Pledge' when 'OM' then 'Off-Market' when 'TP' then 'Third-Patty' when 'IA' then 'Inter-Account-Transfers' when 'CB' then 'CA-Bonus' when 'CS' then 'CA-Split' when 'CM' then 'CA-Merger' when 'CD' then 'CA-DeMerger' when 'CA' then 'CA-Amalgametion' when 'CX' then 'CA-Other' when 'XX' then 'Alien Transfer' when 'XP' then 'Position Adjustment' when 'AU' then 'Auction Adjustment' when 'OP' then 'Opening' else 'NA' end) as Type from Trans_DematTransactions,Trans_dematPosition where dematPosition_CompanyID=DematTransactions_CompanyID and (dematPosition_SegmentID=DematTransactions_SegmentID or dematPosition_SegmentID=DematTransactions_SourceSegmentID) and DematTransactions_ProductSeriesID=DematPosition_ProductSeriesID and DematTransactions_CustomerID=DematPosition_CustomerID and DematTransactions_ISIN=DematPosition_ISIN and (DematTransactions_SettlementNumberT=DematPosition_SettlementNumber or DematTransactions_SettlementNumberS=DematPosition_SettlementNumber) and (DematTransactions_SettlementTypeT=DematPosition_SettlementType or DematTransactions_SettlementTypeS=DematPosition_SettlementType) and DematPosition_ID=" + ID + " and DematTransactions_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "') as DD", " TranID,Date,cast(Quantity as varchar) as Quantity,case when DematTransactions_OwnAccounts is null then DematTransactions_CustomerAccounts else DematTransactions_OwnAccounts end as DeliveredFrom,case when DematTransactions_OwnAccountT is null then DematTransactions_CustomerAccountT else DematTransactions_OwnAccountT end as DeliveredTo,DematTransactions_SlipNumber,DematTransactions_SettlementNumberS,DematTransactions_SettlementNumberT,DematTransactions_Remarks,Type", null);
                if (dtTransaction.Rows.Count > 0)
                {
                    for (int i = 0; i < dtTransaction.Rows.Count; i++)
                    {
                        if (dtTransaction.Rows[i]["Quantity"] != DBNull.Value)
                            dtTransaction.Rows[i]["Quantity"] = oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtTransaction.Rows[i]["Quantity"]));
                    }
                    grdTransaction.DataSource = dtTransaction;
                    grdTransaction.DataBind();
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "JS", "<script language='JavaScript'>alert('No Transaction Found !!');</script>");
                }
            }
            else
            {
                string Type = Request.QueryString["Type"].ToString().Trim();
                string SettNoType = Request.QueryString["SettType"].ToString();
                string SettNum = null;
                string SettType = null;
                string SettlementNumber = null;
                if (Type == "P")
                {
                    SettNum = SettNoType.Substring(0, 7);
                    SettType = SettNoType.Substring(7, 1);
                    SettlementNumber = " and (DematTransactions_SettlementNumberT='" + SettNum + "' and DematTransactions_SettlementTypeT='" + SettType + "' or DematTransactions_SettlementNumberS='" + SettNum + "' and DematTransactions_SettlementTypeS='" + SettType + "')";
                }
                else
                    SettlementNumber = null;
                DataTable dtTransaction = oDBEngine.GetDataTable("(select dematTransactions_ID as TranID,convert(varchar(12),cast(DematTransactions_Date as datetime),113) as Date,DematTransactions_Quantity as Quantity,case when DematTransactions_OwnAccounts is null then null else(select DPAccounts_ShortName from master_dpaccounts where DPAccounts_ID=DematTransactions_OwnAccounts) end as DematTransactions_OwnAccounts,case when DematTransactions_CustomerAccounts is null then null else(select isnull((select rtrim(replace(dp_dpname,char(160),'')) from tbl_master_depositoryparticipants where substring(dp_dpID,1,8)=dpd_dpCode),'')+' '+isnull(rtrim(dpd_ClientID),'') from tbl_master_contactdpdetails where dpd_id=DematTransactions_CustomerAccounts) end as DematTransactions_CustomerAccounts,case when DematTransactions_OwnAccountT is null then null else(select DPAccounts_ShortName from master_dpaccounts where DPAccounts_ID=DematTransactions_OwnAccountT) end as DematTransactions_OwnAccountT,case when DematTransactions_CustomerAccountT is null then null else(select isnull((select rtrim(replace(dp_dpname,char(160),'')) from tbl_master_depositoryparticipants where substring(dp_dpID,1,8)=dpd_dpCode),'')+' '+isnull(rtrim(dpd_ClientID),'') from tbl_master_contactdpdetails where dpd_id=DematTransactions_CustomerAccountT) end as DematTransactions_CustomerAccountT,DematTransactions_DPTransactionReference as DematTransactions_SlipNumber,isnull(DematTransactions_SettlementNumberS,'')+isnull(DematTransactions_SettlementTypeS,'') as DematTransactions_SettlementNumberS,isnull(DematTransactions_SettlementNumberT,'')+isnull(DematTransactions_SettlementTypeT,'') as DematTransactions_SettlementNumberT,DematTransactions_Remarks,(case dematTransactions_Type when 'MI' then 'Market-PayIN' when 'MO' then 'Market-PayOut' when 'CI' then 'Client-PayIn' when 'CO' then 'Client-PayOut' when 'IS' then 'Inter-Settlement' when 'LN' then 'Loan' when 'MG' then 'Margin' when 'HB' then 'HoldBack' when 'PL' then 'Pledge' when 'PU' then 'Un-Pledge' when 'OM' then 'Off-Market' when 'TP' then 'Third-Patty' when 'IA' then 'Inter-Account-Transfers' when 'CB' then 'CA-Bonus' when 'CS' then 'CA-Split' when 'CM' then 'CA-Merger' when 'CD' then 'CA-DeMerger' when 'CA' then 'CA-Amalgametion' when 'CX' then 'CA-Other' when 'XX' then 'Alien Transfer' when 'XP' then 'Position Adjustment' when 'AU' then 'Auction Adjustment' when 'OP' then 'Opening' else 'NA' end) as Type from Trans_DematTransactions where DematTransactions_CompanyID='" + Session["LastCompany"].ToString() + "' and DematTransactions_SegmentID=" + Session["usersegid"].ToString() + " and DematTransactions_ISIN='" + Request.QueryString["ISIN"].ToString() + "' and DematTransactions_finyear='" + HttpContext.Current.Session["LastFinYear"].ToString().Trim() + "' and (DematTransactions_OwnAccountS=" + Request.QueryString["id"].ToString() + " or DematTransactions_OwnAccountT=" + Request.QueryString["id"].ToString() + ") " + SettlementNumber + ") as DD", " TranID,Date,cast(Quantity as varchar) as Quantity,case when DematTransactions_OwnAccounts is null then DematTransactions_CustomerAccounts else DematTransactions_OwnAccounts end as DeliveredFrom,case when DematTransactions_OwnAccountT is null then DematTransactions_CustomerAccountT else DematTransactions_OwnAccountT end as DeliveredTo,DematTransactions_SlipNumber,DematTransactions_SettlementNumberS,DematTransactions_SettlementNumberT,DematTransactions_Remarks,Type", null);
                if (dtTransaction.Rows.Count > 0)
                {
                    for (int i = 0; i < dtTransaction.Rows.Count; i++)
                    {
                        if (dtTransaction.Rows[i]["Quantity"] != DBNull.Value)
                            dtTransaction.Rows[i]["Quantity"] = oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtTransaction.Rows[i]["Quantity"]));
                    }
                    grdTransaction.DataSource = dtTransaction;
                    grdTransaction.DataBind();
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "JS", "<script language='JavaScript'>alert('No Transaction Found !!');</script>");
                }
            }
        }
        //protected void grdTransaction_RowDeleting(object sender, GridViewDeleteEventArgs e)
        //{
        //    string ID = (string) grdTransaction.DataKeys[e.RowIndex].Value.ToString();
        //    int NoOfRowsAffect = 0;
        //    //NoOfRowsAffect = oDBEngine.DeleteValue("trans_dematTransactions", " DematTransactions_ID='" + ID + "'");
        //}
    }
}