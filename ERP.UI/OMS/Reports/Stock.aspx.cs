using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{

    public partial class Reports_Stock : System.Web.UI.Page
    {
        DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        Converter oconverter = new Converter();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                ShowStocks();
        }
        public void ShowStocks()
        {
            decimal SumQty = 0;
            decimal SumPledge = 0;
            decimal SumFreeBalance = 0;
            string ID = Request.QueryString["id"].ToString();
            string FinYear = Session["LastFinYear"].ToString();
            DataTable dtStocks = oDBEngine.GetDataTable("dbo.Trans_DematStocks", "isnull((select DPAccounts_ShortName from master_dpaccounts where DPAccounts_ID=DematStocks_AccountID And dpaccounts_accounttype<>'[SYSTM]'),(select isnull((select rtrim(dp_dpname) from tbl_master_depositoryparticipants where substring(dp_dpID,1,8)=dpd_dpCode),'')+' '+isnull(rtrim(dpd_ClientID),'') from tbl_master_contactdpdetails where dpd_id=DematStocks_AccountID)) as ACName,DematStocks_SettlementNumber+DematStocks_SettlementType as Settlement,DematStocks_ISIN,cast(isnull(DematStocks_OpeningQty,0)+isnull(DematStocks_InQty,0)-isnull(DematStocks_OutQty,0) as varchar) as Qty,cast(isnull(DematStocks_PledgedQty,0)+isnull(DematStocks_LockInQty,0) as varchar) as Pledge,cast((isnull(DematStocks_OpeningQty,0)+isnull(DematStocks_InQty,0)-isnull(DematStocks_OutQty,0))-(isnull(DematStocks_PledgedQty,0)+isnull(DematStocks_LockInQty,0)) as varchar) as FreeBalance", " DematStocks_ProductSeriesID='" + ID + "' and DematStocks_FinYear='" + FinYear + "' and isnull(DematStocks_OpeningQty,0)+isnull(DematStocks_InQty,0)-isnull(DematStocks_OutQty,0)>0 and isnull(DematStocks_SettlementNumber,'SYSTM')<>'SYSTM'");
            if (dtStocks.Rows.Count > 0)
            {
                for (int i = 0; i < dtStocks.Rows.Count; i++)
                {
                    if (Convert.ToDecimal(dtStocks.Rows[i]["Qty"]) != 0)
                    {
                        dtStocks.Rows[i]["Qty"] = oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtStocks.Rows[i]["Qty"]));
                        SumQty += Convert.ToDecimal(dtStocks.Rows[i]["Qty"].ToString());
                    }
                    else
                        dtStocks.Rows[i]["Qty"] = DBNull.Value;
                    if (Convert.ToDecimal(dtStocks.Rows[i]["Pledge"]) != 0)
                    {
                        dtStocks.Rows[i]["Pledge"] = oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtStocks.Rows[i]["Pledge"]));
                        SumPledge += Convert.ToDecimal(dtStocks.Rows[i]["Pledge"].ToString());
                    }
                    else
                        dtStocks.Rows[i]["Pledge"] = DBNull.Value;
                    if (Convert.ToDecimal(dtStocks.Rows[i]["FreeBalance"]) != 0)
                    {
                        dtStocks.Rows[i]["FreeBalance"] = oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtStocks.Rows[i]["FreeBalance"]));
                        SumFreeBalance += Convert.ToDecimal(dtStocks.Rows[i]["FreeBalance"].ToString());
                    }
                    else
                        dtStocks.Rows[i]["FreeBalance"] = DBNull.Value;

                }
                grdStocks.DataSource = dtStocks;
                grdStocks.DataBind();

                grdStocks.FooterRow.Cells[0].Text = "Total : ";
                if (SumQty != 0)
                    grdStocks.FooterRow.Cells[3].Text = oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(SumQty);
                if (SumPledge != 0)
                    grdStocks.FooterRow.Cells[4].Text = oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(SumPledge);
                if (SumFreeBalance != 0)
                    grdStocks.FooterRow.Cells[5].Text = oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(SumFreeBalance);
                grdStocks.FooterRow.Cells[0].HorizontalAlign = HorizontalAlign.Left;
                grdStocks.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Right;
                grdStocks.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Right;
                grdStocks.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Right;
                grdStocks.FooterRow.Cells[0].ForeColor = System.Drawing.Color.White;
                grdStocks.FooterRow.Cells[3].ForeColor = System.Drawing.Color.White;
                grdStocks.FooterRow.Cells[4].ForeColor = System.Drawing.Color.White;
                grdStocks.FooterRow.Cells[5].ForeColor = System.Drawing.Color.White;
                grdStocks.FooterRow.Cells[0].Font.Bold = true;
                grdStocks.FooterRow.Cells[3].Font.Bold = true;
                grdStocks.FooterRow.Cells[4].Font.Bold = true;
                grdStocks.FooterRow.Cells[5].Font.Bold = true;
                grdStocks.FooterRow.Cells[3].Wrap = false;
                grdStocks.FooterRow.Cells[4].Wrap = false;
                grdStocks.FooterRow.Cells[5].Wrap = false;
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "JS", "<script language='JavaScript'>alert('No Transaction Found !!');</script>");
            }
        }
    }
}