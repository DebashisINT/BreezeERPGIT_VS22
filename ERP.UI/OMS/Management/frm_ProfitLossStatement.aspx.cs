using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Management
{
    public partial class management_frm_ProfitLossStatement : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        string data;
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        string SegmentID;
        string CompanyID;
        string BranchId;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SegmentID = null;
                CompanyID = null;
                BranchId = null;
                txtsegselected.Attributes.Add("onkeyup", "ShowMainAccountName(this,'SearchMainAccountBranchSegment',event)");
                //txtSelectBranch.Attributes.Add("onkeyup", "ShowBranchName(this,'SearchMainAccountBranchSegment',event)");
                rdAll.Attributes.Add("OnClick", "MainAll('Segment','all')");
                rdselected.Attributes.Add("OnClick", "MainAll('Segment','Selc')");
                rdAllBranch.Attributes.Add("OnClick", "MainAll('Branch','all')");
                rdSelectedBranch.Attributes.Add("OnClick", "MainAll('Branch','Selc')");
                string fin = HttpContext.Current.Session["LastFinYear"].ToString();
                string[] finyear = fin.Split('-');
                string FirstDay = "04/01" + "/" + finyear[0].ToString();//set the date from the 1st day of current financial year
                AspxFromDate.Value = FirstDay.ToString();
                AspxFromDate.EditFormatString = objConverter.GetDateFormat("Date");
                AspxTodate.EditFormatString = objConverter.GetDateFormat("Date");
                AspxTodate.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
                Page.ClientScript.RegisterStartupScript(GetType(), "MaintainHeight", "<script language='JavaScript'>height();</script>");
                DataTable DtSegComp = oDBEngine.GetDataTable("(select exch_compId,exch_internalId,isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Comp from tbl_master_companyExchange where exch_compid in (select top 1 ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment=" + Session["userlastsegment"].ToString() + ")) as D", "*", "Comp in(select seg_name from tbl_master_segment where seg_id=" + Session["userlastsegment"].ToString() + ")");
                if (DtSegComp.Rows.Count > 0)
                {
                    CompanyID = DtSegComp.Rows[0][0].ToString();
                    Session["CompanyID"] = CompanyID;
                    SegmentID = DtSegComp.Rows[0][1].ToString();
                    //litSegment.InnerText = "( " + DtSegComp.Rows[0][2].ToString() + " )";
                }

            }
            //else
            //{
            //    string a = Session["KeyVal"].ToString();
            //}
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
            if (idlist[0] == "Segment")
            {
                SegmentID = str;
                Session["KeyValSegment"] = str;
                data = "Segment~" + str1;
            }
            if (idlist[0] == "Branch")
            {
                BranchId = str;
                Session["KeyVal"] = str;
                data = "Branch~" + str1;
            }
        }
        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            int flag = 1;
            DataTable DtForIncome = new DataTable();
            DataTable DtForExpence = new DataTable();
            DataTable DtForTotalIncome = new DataTable();
            DataTable DtForTotalExpence = new DataTable();
            if (rdselected.Checked == true && rdSelectedBranch.Checked == true)
            {
                DtForIncome = oDBEngine.GetDataTable("trans_accountsledger as a inner join master_mainaccount as b on a.accountsledger_mainaccountid=b.mainaccount_accountcode", " b.mainaccount_accountcode,cast(sum(a.accountsledger_amountcr-a.accountsledger_amountdr)as money)as INCOME,ltrim(rtrim(b.mainaccount_name ))as account_name", "mainaccount_accounttype='Income' and a.accountsledger_branchid=" + Session["KeyVal"].ToString() + " and accountsledger_exchangesegmentid=" + Session["KeyValSegment"].ToString() + " and a.accountsledger_transactiondate>= '" + AspxFromDate.Value.ToString() + "' and a.accountsledger_transactiondate<='" + AspxTodate.Value.ToString() + "' and accountsledger_companyid='" + Session["LastCompany"].ToString() + "'  group by b.mainaccount_accountcode,b.mainaccount_name ", "b.mainaccount_name");
                DtForExpence = oDBEngine.GetDataTable("trans_accountsledger as a inner join master_mainaccount as b on a.accountsledger_mainaccountid=b.mainaccount_accountcode", "b.mainaccount_accountcode,cast(sum(a.accountsledger_amountdr-a.accountsledger_amountcr)as money)as expenditure,ltrim(rtrim(b.mainaccount_name ))as account_name", "mainaccount_accounttype='Expences' and a.accountsledger_branchid=" + Session["KeyVal"].ToString() + " and accountsledger_exchangesegmentid=" + Session["KeyValSegment"].ToString() + " and a.accountsledger_transactiondate>= '" + AspxFromDate.Value.ToString() + "' and a.accountsledger_transactiondate<='" + AspxTodate.Value.ToString() + "' and accountsledger_companyid='" + Session["LastCompany"].ToString() + "' group by b.mainaccount_accountcode,b.mainaccount_name", "b.mainaccount_name");
                DtForTotalIncome = oDBEngine.GetDataTable("trans_accountsledger as a inner join master_mainaccount as b on a.accountsledger_mainaccountid=b.mainaccount_accountcode", " isnull(cast(sum(a.accountsledger_amountcr-a.accountsledger_amountdr)as money),0)as INCOME", "mainaccount_accounttype='Income' and a.accountsledger_branchid=" + Session["KeyVal"].ToString() + " and accountsledger_exchangesegmentid=" + Session["KeyValSegment"].ToString() + " and a.accountsledger_transactiondate>= '" + AspxFromDate.Value.ToString() + "' and a.accountsledger_transactiondate<='" + AspxTodate.Value.ToString() + "' and accountsledger_companyid='" + Session["LastCompany"].ToString() + "'");
                DtForTotalExpence = oDBEngine.GetDataTable("trans_accountsledger as a inner join master_mainaccount as b on a.accountsledger_mainaccountid=b.mainaccount_accountcode", " isnull(cast(sum(a.accountsledger_amountdr-a.accountsledger_amountcr)as money),0)as Expences", "mainaccount_accounttype='Expences' and a.accountsledger_branchid=" + Session["KeyVal"].ToString() + " and accountsledger_exchangesegmentid=" + Session["KeyValSegment"].ToString() + " and a.accountsledger_transactiondate>= '" + AspxFromDate.Value.ToString() + "' and a.accountsledger_transactiondate<='" + AspxTodate.Value.ToString() + "' and accountsledger_companyid='" + Session["LastCompany"].ToString() + "'");
            }
            else if (rdAll.Checked == true && rdAllBranch.Checked == true)
            {
                DtForIncome = oDBEngine.GetDataTable("trans_accountsledger as a inner join master_mainaccount as b on a.accountsledger_mainaccountid=b.mainaccount_accountcode", " b.mainaccount_accountcode,cast(sum(a.accountsledger_amountcr-a.accountsledger_amountdr)as money)as INCOME,ltrim(rtrim(b.mainaccount_name ))as account_name", "mainaccount_accounttype='Income' and  a.accountsledger_transactiondate>= '" + AspxFromDate.Value.ToString() + "' and a.accountsledger_transactiondate<='" + AspxTodate.Value.ToString() + "' and accountsledger_companyid='" + Session["LastCompany"].ToString() + "'  group by b.mainaccount_accountcode,b.mainaccount_name ", "b.mainaccount_name");
                DtForExpence = oDBEngine.GetDataTable("trans_accountsledger as a inner join master_mainaccount as b on a.accountsledger_mainaccountid=b.mainaccount_accountcode", "b.mainaccount_accountcode,cast(sum(a.accountsledger_amountdr-a.accountsledger_amountcr)as money)as expenditure,ltrim(rtrim(b.mainaccount_name ))as account_name", "mainaccount_accounttype='Expences' and a.accountsledger_transactiondate>= '" + AspxFromDate.Value.ToString() + "' and a.accountsledger_transactiondate<='" + AspxTodate.Value.ToString() + "' and accountsledger_companyid='" + Session["LastCompany"].ToString() + "' group by b.mainaccount_accountcode,b.mainaccount_name", "b.mainaccount_name");
                DtForTotalIncome = oDBEngine.GetDataTable("trans_accountsledger as a inner join master_mainaccount as b on a.accountsledger_mainaccountid=b.mainaccount_accountcode", " isnull(cast(sum(a.accountsledger_amountcr-a.accountsledger_amountdr)as money),0)as INCOME", "mainaccount_accounttype='Income' and a.accountsledger_transactiondate>= '" + AspxFromDate.Value.ToString() + "' and a.accountsledger_transactiondate<='" + AspxTodate.Value.ToString() + "' and accountsledger_companyid='" + Session["LastCompany"].ToString() + "'");
                DtForTotalExpence = oDBEngine.GetDataTable("trans_accountsledger as a inner join master_mainaccount as b on a.accountsledger_mainaccountid=b.mainaccount_accountcode", " isnull(cast(sum(a.accountsledger_amountdr-a.accountsledger_amountcr)as money),0)as Expences", "mainaccount_accounttype='Expences' and a.accountsledger_transactiondate>= '" + AspxFromDate.Value.ToString() + "' and a.accountsledger_transactiondate<='" + AspxTodate.Value.ToString() + "' and accountsledger_companyid='" + Session["LastCompany"].ToString() + "'");
            }
            else if (rdselected.Checked == true && rdAllBranch.Checked == true)
            {
                DtForIncome = oDBEngine.GetDataTable("trans_accountsledger as a inner join master_mainaccount as b on a.accountsledger_mainaccountid=b.mainaccount_accountcode", " b.mainaccount_accountcode,cast(sum(a.accountsledger_amountcr-a.accountsledger_amountdr)as money)as INCOME,ltrim(rtrim(b.mainaccount_name ))as account_name", "mainaccount_accounttype='Income'  and accountsledger_exchangesegmentid=" + Session["KeyValSegment"].ToString() + " and a.accountsledger_transactiondate>= '" + AspxFromDate.Value.ToString() + "' and a.accountsledger_transactiondate<='" + AspxTodate.Value.ToString() + "' and accountsledger_companyid='" + Session["LastCompany"].ToString() + "'  group by b.mainaccount_accountcode,b.mainaccount_name ", "b.mainaccount_name");
                DtForExpence = oDBEngine.GetDataTable("trans_accountsledger as a inner join master_mainaccount as b on a.accountsledger_mainaccountid=b.mainaccount_accountcode", "b.mainaccount_accountcode,cast(sum(a.accountsledger_amountdr-a.accountsledger_amountcr)as money)as expenditure,ltrim(rtrim(b.mainaccount_name ))as account_name", "mainaccount_accounttype='Expences'  and accountsledger_exchangesegmentid=" + Session["KeyValSegment"].ToString() + " and a.accountsledger_transactiondate>= '" + AspxFromDate.Value.ToString() + "' and a.accountsledger_transactiondate<='" + AspxTodate.Value.ToString() + "' and accountsledger_companyid='" + Session["LastCompany"].ToString() + "' group by b.mainaccount_accountcode,b.mainaccount_name", "b.mainaccount_name");
                DtForTotalIncome = oDBEngine.GetDataTable("trans_accountsledger as a inner join master_mainaccount as b on a.accountsledger_mainaccountid=b.mainaccount_accountcode", " isnull(cast(sum(a.accountsledger_amountcr-a.accountsledger_amountdr)as money),0)as INCOME", "mainaccount_accounttype='Income'  and accountsledger_exchangesegmentid=" + Session["KeyValSegment"].ToString() + " and a.accountsledger_transactiondate>= '" + AspxFromDate.Value.ToString() + "' and a.accountsledger_transactiondate<='" + AspxTodate.Value.ToString() + "' and accountsledger_companyid='" + Session["LastCompany"].ToString() + "'");
                DtForTotalExpence = oDBEngine.GetDataTable("trans_accountsledger as a inner join master_mainaccount as b on a.accountsledger_mainaccountid=b.mainaccount_accountcode", " isnull(cast(sum(a.accountsledger_amountdr-a.accountsledger_amountcr)as money),0)as Expences", "mainaccount_accounttype='Expences' and accountsledger_exchangesegmentid=" + Session["KeyValSegment"].ToString() + " and a.accountsledger_transactiondate>= '" + AspxFromDate.Value.ToString() + "' and a.accountsledger_transactiondate<='" + AspxTodate.Value.ToString() + "' and accountsledger_companyid='" + Session["LastCompany"].ToString() + "'");
            }
            else if (rdAll.Checked == true && rdSelectedBranch.Checked == true)
            {
                DtForIncome = oDBEngine.GetDataTable("trans_accountsledger as a inner join master_mainaccount as b on a.accountsledger_mainaccountid=b.mainaccount_accountcode", " b.mainaccount_accountcode,cast(sum(a.accountsledger_amountcr-a.accountsledger_amountdr)as money)as INCOME,ltrim(rtrim(b.mainaccount_name ))as account_name", "mainaccount_accounttype='Income' and a.accountsledger_branchid=" + Session["KeyVal"].ToString() + " and a.accountsledger_transactiondate>= '" + AspxFromDate.Value.ToString() + "' and a.accountsledger_transactiondate<='" + AspxTodate.Value.ToString() + "' and accountsledger_companyid='" + Session["LastCompany"].ToString() + "'  group by b.mainaccount_accountcode,b.mainaccount_name ", "b.mainaccount_name");
                DtForExpence = oDBEngine.GetDataTable("trans_accountsledger as a inner join master_mainaccount as b on a.accountsledger_mainaccountid=b.mainaccount_accountcode", "b.mainaccount_accountcode,cast(sum(a.accountsledger_amountdr-a.accountsledger_amountcr)as money)as expenditure,ltrim(rtrim(b.mainaccount_name ))as account_name", "mainaccount_accounttype='Expences' and a.accountsledger_branchid=" + Session["KeyVal"].ToString() + "  and a.accountsledger_transactiondate>= '" + AspxFromDate.Value.ToString() + "' and a.accountsledger_transactiondate<='" + AspxTodate.Value.ToString() + "' and accountsledger_companyid='" + Session["LastCompany"].ToString() + "' group by b.mainaccount_accountcode,b.mainaccount_name", "b.mainaccount_name");
                DtForTotalIncome = oDBEngine.GetDataTable("trans_accountsledger as a inner join master_mainaccount as b on a.accountsledger_mainaccountid=b.mainaccount_accountcode", " isnull(cast(sum(a.accountsledger_amountcr-a.accountsledger_amountdr)as money),0)as INCOME", "mainaccount_accounttype='Income' and a.accountsledger_branchid=" + Session["KeyVal"].ToString() + "  and a.accountsledger_transactiondate>= '" + AspxFromDate.Value.ToString() + "' and a.accountsledger_transactiondate<='" + AspxTodate.Value.ToString() + "' and accountsledger_companyid='" + Session["LastCompany"].ToString() + "'");
                DtForTotalExpence = oDBEngine.GetDataTable("trans_accountsledger as a inner join master_mainaccount as b on a.accountsledger_mainaccountid=b.mainaccount_accountcode", " isnull(cast(sum(a.accountsledger_amountdr-a.accountsledger_amountcr)as money),0)as Expences", "mainaccount_accounttype='Expences' and a.accountsledger_branchid=" + Session["KeyVal"].ToString() + " and a.accountsledger_transactiondate>= '" + AspxFromDate.Value.ToString() + "' and a.accountsledger_transactiondate<='" + AspxTodate.Value.ToString() + "' and accountsledger_companyid='" + Session["LastCompany"].ToString() + "'");
            }
            string Format = "<table cellspacing=\"1\" cellpadding=\"1\"  style=\"border:solid 1px red;background:white;width:100%\">";
            Format += "<tr><td colspan=\"2\"  style=\"border-bottom:solid 1px red;border-right:solid red 1px;font-weight:bold; color: Maroon\" align=\"centre\" width=\"50%\"> INCOME </td><td style=\"padding-left: 10px\" colspan=\"2\"  style=\"border-bottom:solid 1px red;font-weight:bold; color: Maroon\" align=\"centre\" width=\"50%\">EXPENDITURE</td></tr>";
            int upperlimitforincome = 0;
            int upperlimitforexpence = 0;
            int upperlimit = 0;
            int upperlimitfornextloopforincome = 0;
            int upperlimitfornextloopforexpence = 0;
            if (DtForIncome.Rows.Count > 0)
            {
                upperlimitforincome = Convert.ToInt16(DtForIncome.Rows.Count.ToString());
            }
            if (DtForExpence.Rows.Count > 0)
            {
                upperlimitforexpence = Convert.ToInt16(DtForExpence.Rows.Count.ToString());
            }
            if (upperlimitforexpence >= upperlimitforincome)
            {
                upperlimit = upperlimitforincome;
                upperlimitfornextloopforexpence = upperlimitforexpence - upperlimitforincome;
            }
            else
            {
                upperlimit = upperlimitforexpence;
                upperlimitfornextloopforincome = upperlimitforincome - upperlimitforexpence;
            }
            for (int i = 0; i < upperlimit; i++)
            {
                flag = flag + 1;
                Format += "<tr style=\"background-color: " + GetRowColor(flag) + "\"><td width=\"25%\" align=\"left\">" + DtForIncome.Rows[i][2].ToString() + "</td><td style=\"padding-right:10px;border-right:solid red 1px;\" width=\"25%\" align=\"right\">" + objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(DtForIncome.Rows[i][1].ToString())) + " </td><td style=\"padding-left:10px;\" width=\"25%\" align=\"left\">" + DtForExpence.Rows[i][2].ToString() + "</td><td width=\"25%\" align=\"right\">" + objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(DtForExpence.Rows[i][1].ToString())) + "</td></tr>";
            }
            if (upperlimitfornextloopforincome > 0)
            {

                for (int i = upperlimit; i < upperlimit + upperlimitfornextloopforincome; i++)
                {
                    flag = flag + 1;
                    Format += "<tr style=\"background-color: " + GetRowColor(flag) + "\"><td width=\"25%\" align=\"left\">" + DtForIncome.Rows[i][2].ToString() + "</td><td style=\"padding-right:10px;border-right:solid red 1px;\" width=\"25%\" align=\"right\">" + objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(DtForIncome.Rows[i][1].ToString())) + " </td><td width=\"25%\" align=\"left\"></td><td width=\"25%\" align=\"left\"></td></tr>";

                }
            }
            else if (upperlimitfornextloopforexpence > 0)
            {

                for (int i = upperlimit; i < upperlimit + upperlimitfornextloopforexpence; i++)
                {
                    flag = flag + 1;
                    Format += "<tr style=\"background-color: " + GetRowColor(flag) + "\"><td width=\"25%\" align=\"left\"></td><td width=\"25%\" align=\"left\"></td><td style=\"padding-left:10px;border-left:solid red 1px\" width=\"25%\" align=\"left\">" + DtForExpence.Rows[i][2].ToString() + "</td><td width=\"25%\" align=\"right\"> " + objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(DtForExpence.Rows[i][1].ToString())) + " </td></tr>";

                }
            }
            string TotalIncome = "0";
            string TotalExpence = "0";
            decimal NetProfit = 0;
            decimal NetLoss = 0;
            if (DtForTotalIncome.Rows[0][0].ToString() != null)
            {
                TotalIncome = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(DtForTotalIncome.Rows[0][0].ToString()));
            }
            if (DtForTotalExpence.Rows[0][0] != "0")
            {
                TotalExpence = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(DtForTotalExpence.Rows[0][0].ToString()));
                //if (TotalExpence == " 0.00")
                //{
                //    TotalExpence = "";
                //}
            }
            flag = flag + 1;
            Format += "<tr style=\"background-color: " + GetRowColor(flag) + "\"><td style=\"font-weight: bold; border-bottom:solid 1px red;color:blue;border-top:solid 1px red\" width=\"25%\" align=\"left\"> Total: </td><td style=\"padding-right:10px;font-weight: bold; border-bottom:solid 1px red;color:blue;border-top:solid 1px red\" width=\"25%\" align=\"right\">" + TotalIncome.ToString() + "</td><td style=\"padding-left:10px;border-left:solid red 1px;font-weight: bold; border-bottom:solid 1px red;color:blue;border-top:solid 1px red\" width=\"25%\" align=\"left\">Total:</td><td style=\"font-weight: bold; border-bottom:solid 1px red;color:blue;border-top:solid 1px red\" width=\"25%\" align=\"right\"> " + TotalExpence.ToString() + " </td></tr>";
            decimal tIncome = 0;
            decimal tExpence = 0;
            tIncome = Convert.ToDecimal(TotalIncome.ToString());
            if (TotalExpence == "")
            {
                TotalExpence = "0";
            }
            tExpence = Convert.ToDecimal(TotalExpence.ToString());
            if (tIncome > tExpence)
            {
                NetProfit = tIncome - tExpence;
            }
            else if (tExpence > tIncome)
            {
                NetLoss = tExpence - tIncome;
            }
            if (NetProfit != 0)
            {
                Format += "<tr><td colspan=\"2\"> </td><td style=\"font-weight: bold;padding-left:10px;color: Green;\" align=\"left\"> NET PROFIT FOR THE PERIOD=</td><td style=\"font-weight: bold;color: Green;\" align=\"right\">" + NetProfit.ToString() + "</td> </tr>";
            }
            else if (NetLoss != 0)
            {
                Format += "<tr><td style=\"font-weight: bold;color: Green;\" align=\"right\"> NET LOSS FOR THE PERIOD=</td><td style=\"font-weight: bold;color: Green;\" align=\"right\">" + NetLoss.ToString() + "</td> <td colspan=\"2\"></td></tr>";
            }
            Format += "</table>";
            MainContainer.InnerHtml = Format;
        }
        protected string GetRowColor(int i)
        {
            //if (i++ % 2 == 0)
            //    return "#fff0f5";
            //else
            //    return "lavender";
            if (i++ % 2 == 0)
                return "#fff0f4";
            else
                return "White";
        }
    }
}
