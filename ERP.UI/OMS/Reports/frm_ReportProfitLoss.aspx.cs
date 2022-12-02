using System;
using System.Data;
using System.Web.UI;
using System.Configuration;
using BusinessLogicLayer;


namespace ERP.OMS.Reports
{
    public partial class Reports_frm_ReportProfitLoss : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        ExcelFile objExcel = new ExcelFile();
        string data;
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
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
                SetDatteFinYear();

                //string fin = HttpContext.Current.Session["LastFinYear"].ToString();
                //string[] finyear = fin.Split('-');
                //string FirstDay = "04/01" + "/" + finyear[0].ToString();//set the date from the 1st day of current financial year
                //AspxFromDate.Value = FirstDay.ToString();

                //AspxTodate.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
                Page.ClientScript.RegisterStartupScript(GetType(), "MaintainHeight", "<script language='JavaScript'>height();</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "PageLoad", "<script language='JavaScript'>HideExport();</script>");
                DataTable DtSegComp = oDBEngine.GetDataTable("(select top 1 exch_compId,exch_internalId,isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Comp from tbl_master_companyExchange where exch_compid in (select top 1 ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment=" + Session["userlastsegment"].ToString() + ")) as D", "*", "Comp in(select seg_name from tbl_master_segment where seg_id=" + Session["userlastsegment"].ToString() + ")");
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
        protected void SetDatteFinYear()
        {
            DataTable dtFinYear = oDBEngine.GetDataTable("MASTER_FINYEAR ", " FINYEAR_STARTDATE,FINYEAR_ENDDATE ", " FINYEAR_CODE ='" + Session["LastFinYear"].ToString() + "'");
            AspxFromDate.EditFormatString = objConverter.GetDateFormat("Date");
            AspxTodate.EditFormatString = objConverter.GetDateFormat("Date");
            //AspxFromDate.Value = Convert.ToDateTime(dtFinYear.Rows[0]["FINYEAR_STARTDATE"].ToString());
            //AspxTodate.Value = Convert.ToDateTime(dtFinYear.Rows[0]["FINYEAR_ENDDATE"].ToString());
            //  DataTable dtFinYear = objEngine.GetDataTable("MASTER_FINYEAR ", "FINYEAR_STARTDATE, FINYEAR_ENDDATE ", " FINYEAR_CODE ='" + Session["LastFinYear"].ToString() + "'");
            DateTime StartDate = Convert.ToDateTime(dtFinYear.Rows[0]["FINYEAR_STARTDATE"].ToString());
            DateTime EndDate = Convert.ToDateTime(dtFinYear.Rows[0]["FINYEAR_ENDDATE"].ToString());
            DateTime TodayDate = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
            if (EndDate < TodayDate)
            {
                AspxFromDate.Value = StartDate;
                AspxTodate.Value = EndDate;
            }
            else
            {
                AspxFromDate.Value = StartDate;
                AspxTodate.Value = TodayDate;
            }

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
                DtForIncome = oDBEngine.GetDataTable("trans_accountsledger as a inner join master_mainaccount as b on a.accountsledger_mainaccountid=b.mainaccount_accountcode", " b.mainaccount_accountcode,cast(sum(a.accountsledger_amountcr-a.accountsledger_amountdr)as money)as INCOME,ltrim(rtrim(b.mainaccount_name ))as account_name", "mainaccount_accounttype='Income' and a.accountsledger_branchid in(" + Session["KeyVal"].ToString() + ") and accountsledger_exchangesegmentid in(" + Session["KeyValSegment"].ToString() + ") and a.accountsledger_transactiondate>= '" + AspxFromDate.Value.ToString() + "' and a.accountsledger_transactiondate<='" + AspxTodate.Value.ToString() + "'  and AccountsLedger_FinYear='" + Session["LastFinYear"] + "'  and accountsledger_companyid in('" + Session["LastCompany"].ToString() + @"'
            and isnull(accountsledger_currency," + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0] + ")  group by b.mainaccount_accountcode,b.mainaccount_name ", "b.mainaccount_name");
                DtForExpence = oDBEngine.GetDataTable("trans_accountsledger as a inner join master_mainaccount as b on a.accountsledger_mainaccountid=b.mainaccount_accountcode", "b.mainaccount_accountcode,cast(sum(a.accountsledger_amountdr-a.accountsledger_amountcr)as money)as expenditure,ltrim(rtrim(b.mainaccount_name ))as account_name", "mainaccount_accounttype='Expences' and a.accountsledger_branchid in(" + Session["KeyVal"].ToString() + ") and accountsledger_exchangesegmentid in(" + Session["KeyValSegment"].ToString() + ") and a.accountsledger_transactiondate>= '" + AspxFromDate.Value.ToString() + "' and a.accountsledger_transactiondate<='" + AspxTodate.Value.ToString() + "' and accountsledger_companyid in('" + Session["LastCompany"].ToString() + @"'
            and isnull(accountsledger_currency," + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0] + ") group by b.mainaccount_accountcode,b.mainaccount_name", "b.mainaccount_name");
                DtForTotalIncome = oDBEngine.GetDataTable("trans_accountsledger as a inner join master_mainaccount as b on a.accountsledger_mainaccountid=b.mainaccount_accountcode", " isnull(cast(sum(a.accountsledger_amountcr-a.accountsledger_amountdr)as money),0)as INCOME", "mainaccount_accounttype='Income' and a.accountsledger_branchid in(" + Session["KeyVal"].ToString() + ") and accountsledger_exchangesegmentid in(" + Session["KeyValSegment"].ToString() + ") and a.accountsledger_transactiondate>= '" + AspxFromDate.Value.ToString() + "' and a.accountsledger_transactiondate<='" + AspxTodate.Value.ToString() + "' and accountsledger_companyid='" + Session["LastCompany"].ToString() + @"'
            and isnull(accountsledger_currency," + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0]);
                DtForTotalExpence = oDBEngine.GetDataTable("trans_accountsledger as a inner join master_mainaccount as b on a.accountsledger_mainaccountid=b.mainaccount_accountcode", " isnull(cast(sum(a.accountsledger_amountdr-a.accountsledger_amountcr)as money),0)as Expences", "mainaccount_accounttype='Expences' and a.accountsledger_branchid in(" + Session["KeyVal"].ToString() + ") and accountsledger_exchangesegmentid in(" + Session["KeyValSegment"].ToString() + ") and a.accountsledger_transactiondate>= '" + AspxFromDate.Value.ToString() + "' and a.accountsledger_transactiondate<='" + AspxTodate.Value.ToString() + "' and accountsledger_companyid='" + Session["LastCompany"].ToString() + @"'
            and isnull(accountsledger_currency," + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0]);
            }
            else if (rdAll.Checked == true && rdAllBranch.Checked == true)
            {
                DtForIncome = oDBEngine.GetDataTable("trans_accountsledger as a inner join master_mainaccount as b on a.accountsledger_mainaccountid=b.mainaccount_accountcode", " b.mainaccount_accountcode,cast(sum(a.accountsledger_amountcr-a.accountsledger_amountdr)as money)as INCOME,ltrim(rtrim(b.mainaccount_name ))as account_name", "mainaccount_accounttype='Income' and  a.accountsledger_transactiondate>= '" + AspxFromDate.Value.ToString() + "' and a.accountsledger_transactiondate<='" + AspxTodate.Value.ToString() + "'  and AccountsLedger_FinYear='" + Session["LastFinYear"] + "'  and accountsledger_companyid='" + Session["LastCompany"].ToString() + @"'  
            and isnull(accountsledger_currency," + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0] + " group by b.mainaccount_accountcode,b.mainaccount_name ", "b.mainaccount_name");
                DtForExpence = oDBEngine.GetDataTable("trans_accountsledger as a inner join master_mainaccount as b on a.accountsledger_mainaccountid=b.mainaccount_accountcode", "b.mainaccount_accountcode,cast(sum(a.accountsledger_amountdr-a.accountsledger_amountcr)as money)as expenditure,ltrim(rtrim(b.mainaccount_name ))as account_name", "mainaccount_accounttype='Expences' and a.accountsledger_transactiondate>= '" + AspxFromDate.Value.ToString() + "' and a.accountsledger_transactiondate<='" + AspxTodate.Value.ToString() + "' and accountsledger_companyid='" + Session["LastCompany"].ToString() + @"' 
            and isnull(accountsledger_currency," + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0] + " group by b.mainaccount_accountcode,b.mainaccount_name", "b.mainaccount_name");
                DtForTotalIncome = oDBEngine.GetDataTable("trans_accountsledger as a inner join master_mainaccount as b on a.accountsledger_mainaccountid=b.mainaccount_accountcode", " isnull(cast(sum(a.accountsledger_amountcr-a.accountsledger_amountdr)as money),0)as INCOME", "mainaccount_accounttype='Income' and a.accountsledger_transactiondate>= '" + AspxFromDate.Value.ToString() + "' and a.accountsledger_transactiondate<='" + AspxTodate.Value.ToString() + "' and accountsledger_companyid='" + Session["LastCompany"].ToString() + @"'
            and isnull(accountsledger_currency," + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0]);
                DtForTotalExpence = oDBEngine.GetDataTable("trans_accountsledger as a inner join master_mainaccount as b on a.accountsledger_mainaccountid=b.mainaccount_accountcode", " isnull(cast(sum(a.accountsledger_amountdr-a.accountsledger_amountcr)as money),0)as Expences", "mainaccount_accounttype='Expences' and a.accountsledger_transactiondate>= '" + AspxFromDate.Value.ToString() + "' and a.accountsledger_transactiondate<='" + AspxTodate.Value.ToString() + "' and accountsledger_companyid='" + Session["LastCompany"].ToString() + @"'
            and isnull(accountsledger_currency," + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0]);
            }
            else if (rdselected.Checked == true && rdAllBranch.Checked == true)
            {
                DtForIncome = oDBEngine.GetDataTable("trans_accountsledger as a inner join master_mainaccount as b on a.accountsledger_mainaccountid=b.mainaccount_accountcode", " b.mainaccount_accountcode,cast(sum(a.accountsledger_amountcr-a.accountsledger_amountdr)as money)as INCOME,ltrim(rtrim(b.mainaccount_name ))as account_name", "mainaccount_accounttype='Income'  and accountsledger_exchangesegmentid in(" + Session["KeyValSegment"].ToString() + ") and a.accountsledger_transactiondate>= '" + AspxFromDate.Value.ToString() + "' and a.accountsledger_transactiondate<='" + AspxTodate.Value.ToString() + "'  and AccountsLedger_FinYear='" + Session["LastFinYear"] + "'  and accountsledger_companyid='" + Session["LastCompany"].ToString() + @"'
            and isnull(accountsledger_currency," + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0] + " group by b.mainaccount_accountcode,b.mainaccount_name ", "b.mainaccount_name");
                DtForExpence = oDBEngine.GetDataTable("trans_accountsledger as a inner join master_mainaccount as b on a.accountsledger_mainaccountid=b.mainaccount_accountcode", "b.mainaccount_accountcode,cast(sum(a.accountsledger_amountdr-a.accountsledger_amountcr)as money)as expenditure,ltrim(rtrim(b.mainaccount_name ))as account_name", "mainaccount_accounttype='Expences'  and accountsledger_exchangesegmentid in(" + Session["KeyValSegment"].ToString() + ") and a.accountsledger_transactiondate>= '" + AspxFromDate.Value.ToString() + "' and a.accountsledger_transactiondate<='" + AspxTodate.Value.ToString() + "' and accountsledger_companyid='" + Session["LastCompany"].ToString() + @"' 
            and isnull(accountsledger_currency," + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0] + " group by b.mainaccount_accountcode,b.mainaccount_name", "b.mainaccount_name");
                DtForTotalIncome = oDBEngine.GetDataTable("trans_accountsledger as a inner join master_mainaccount as b on a.accountsledger_mainaccountid=b.mainaccount_accountcode", " isnull(cast(sum(a.accountsledger_amountcr-a.accountsledger_amountdr)as money),0)as INCOME", "mainaccount_accounttype='Income'  and accountsledger_exchangesegmentid in(" + Session["KeyValSegment"].ToString() + ") and a.accountsledger_transactiondate>= '" + AspxFromDate.Value.ToString() + "' and a.accountsledger_transactiondate<='" + AspxTodate.Value.ToString() + "' and accountsledger_companyid='" + Session["LastCompany"].ToString() + @"' 
            and isnull(accountsledger_currency," + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0]);
                DtForTotalExpence = oDBEngine.GetDataTable("trans_accountsledger as a inner join master_mainaccount as b on a.accountsledger_mainaccountid=b.mainaccount_accountcode", " isnull(cast(sum(a.accountsledger_amountdr-a.accountsledger_amountcr)as money),0)as Expences", "mainaccount_accounttype='Expences' and accountsledger_exchangesegmentid in(" + Session["KeyValSegment"].ToString() + ") and a.accountsledger_transactiondate>= '" + AspxFromDate.Value.ToString() + "' and a.accountsledger_transactiondate<='" + AspxTodate.Value.ToString() + "' and accountsledger_companyid='" + Session["LastCompany"].ToString() + @"' 
            and isnull(accountsledger_currency," + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0]);
            }
            else if (rdAll.Checked == true && rdSelectedBranch.Checked == true)
            {
                DtForIncome = oDBEngine.GetDataTable("trans_accountsledger as a inner join master_mainaccount as b on a.accountsledger_mainaccountid=b.mainaccount_accountcode", " b.mainaccount_accountcode,cast(sum(a.accountsledger_amountcr-a.accountsledger_amountdr)as money)as INCOME,ltrim(rtrim(b.mainaccount_name ))as account_name", "mainaccount_accounttype='Income' and a.accountsledger_branchid in(" + Session["KeyVal"].ToString() + ") and a.accountsledger_transactiondate>= '" + AspxFromDate.Value.ToString() + "' and a.accountsledger_transactiondate<='" + AspxTodate.Value.ToString() + "'  and AccountsLedger_FinYear='" + Session["LastFinYear"] + "'  and accountsledger_companyid='" + Session["LastCompany"].ToString() + @"' 
            and isnull(accountsledger_currency," + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0] + "  group by b.mainaccount_accountcode,b.mainaccount_name ", "b.mainaccount_name");
                DtForExpence = oDBEngine.GetDataTable("trans_accountsledger as a inner join master_mainaccount as b on a.accountsledger_mainaccountid=b.mainaccount_accountcode", "b.mainaccount_accountcode,cast(sum(a.accountsledger_amountdr-a.accountsledger_amountcr)as money)as expenditure,ltrim(rtrim(b.mainaccount_name ))as account_name", "mainaccount_accounttype='Expences' and a.accountsledger_branchid in(" + Session["KeyVal"].ToString() + ") and a.accountsledger_transactiondate>= '" + AspxFromDate.Value.ToString() + "' and a.accountsledger_transactiondate<='" + AspxTodate.Value.ToString() + "' and accountsledger_companyid='" + Session["LastCompany"].ToString() + @"' 
            and isnull(accountsledger_currency," + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0] + " group by b.mainaccount_accountcode,b.mainaccount_name", "b.mainaccount_name");
                DtForTotalIncome = oDBEngine.GetDataTable("trans_accountsledger as a inner join master_mainaccount as b on a.accountsledger_mainaccountid=b.mainaccount_accountcode", " isnull(cast(sum(a.accountsledger_amountcr-a.accountsledger_amountdr)as money),0)as INCOME", "mainaccount_accounttype='Income' and a.accountsledger_branchid in(" + Session["KeyVal"].ToString() + ")  and a.accountsledger_transactiondate>= '" + AspxFromDate.Value.ToString() + "' and a.accountsledger_transactiondate<='" + AspxTodate.Value.ToString() + "' and accountsledger_companyid='" + Session["LastCompany"].ToString() + @"' 
            and isnull(accountsledger_currency," + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0]);
                DtForTotalExpence = oDBEngine.GetDataTable("trans_accountsledger as a inner join master_mainaccount as b on a.accountsledger_mainaccountid=b.mainaccount_accountcode", " isnull(cast(sum(a.accountsledger_amountdr-a.accountsledger_amountcr)as money),0)as Expences", "mainaccount_accounttype='Expences' and a.accountsledger_branchid in(" + Session["KeyVal"].ToString() + ") and a.accountsledger_transactiondate>= '" + AspxFromDate.Value.ToString() + "' and a.accountsledger_transactiondate<='" + AspxTodate.Value.ToString() + "' and accountsledger_companyid='" + Session["LastCompany"].ToString() + @"' 
            and isnull(accountsledger_currency," + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0]);
            }
            string Format = "<table cellspacing=\"0\" cellpadding=\"0\"  style=\"border:solid 1px red;background:white;width:100%\">";
            Format += "<tr><td colspan=\"2\"  style=\"border-bottom:solid 1px red;border-right:solid red 1px;font-weight:bold; color: Maroon\" align=\"centre\" width=\"50%\"> EXPENDITURE </td><td style=\"padding-left: 10px\" colspan=\"2\"  style=\"border-bottom:solid 1px red;font-weight:bold; color: Maroon\" align=\"centre\" width=\"50%\">INCOME</td></tr>";
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
                Format += "<tr style=\"background-color: " + GetRowColor(flag) + "\"><td width=\"25%\" align=\"left\">" + DtForExpence.Rows[i][2].ToString() + "</td><td style=\"padding-right:10px;border-right:solid red 1px;\" width=\"25%\" align=\"right\">" + objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(DtForExpence.Rows[i][1].ToString())) + " </td><td style=\"padding-left:10px; \" width=\"25%\" align=\"left\">" + DtForIncome.Rows[i][2].ToString() + "</td><td width=\"25%\" align=\"right\">" + objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(DtForIncome.Rows[i][1].ToString())) + "</td></tr>";
            }
            if (upperlimitfornextloopforincome > 0)
            {

                for (int i = upperlimit; i < upperlimit + upperlimitfornextloopforincome; i++)
                {
                    flag = flag + 1;
                    Format += "<tr style=\"background-color: " + GetRowColor(flag) + "\"><td width=\"25%\" align=\"left\"></td><td style=\"padding-right:10px;border-right:solid red 1px;\" width=\"25%\" align=\"right\"> &nbsp; </td><td  style=\"padding-left:10px;\" width=\"25%\" align=\"left\">" + DtForIncome.Rows[i][2].ToString() + "</td><td width=\"25%\" align=\"right\">" + objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(DtForIncome.Rows[i][1].ToString())) + "</td></tr>";

                }
            }
            else if (upperlimitfornextloopforexpence > 0)
            {

                for (int i = upperlimit; i < upperlimit + upperlimitfornextloopforexpence; i++)
                {
                    flag = flag + 1;
                    Format += "<tr style=\"background-color: " + GetRowColor(flag) + "\"><td width=\"25%\" align=\"left\">" + DtForExpence.Rows[i][2].ToString() + "</td><td style=\"padding-right:10px;border-right:solid red 1px;\" width=\"25%\" align=\"right\">" + objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(DtForExpence.Rows[i][1].ToString())) + " </td><td style=\"padding-left:10px;border-left:solid red 1px\" width=\"25%\" align=\"left\"></td><td width=\"25%\" align=\"right\"> </td></tr>";

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
            Format += "<tr style=\"background-color: " + GetRowColor(flag) + "\"><td style=\"font-weight: bold; border-bottom:solid 1px red;color:blue;border-top:solid 1px red\" width=\"25%\" align=\"left\"> Total: </td><td style=\"padding-right:10px;font-weight: bold;border-right:solid red 1px; border-bottom:solid 1px red;color:blue;border-top:solid 1px red\" width=\"25%\" align=\"right\">" + TotalExpence.ToString() + "</td><td style=\"padding-left:10px;font-weight: bold; border-bottom:solid 1px red;color:blue;border-top:solid 1px red\" width=\"25%\" align=\"left\">Total:</td><td style=\"font-weight: bold; border-bottom:solid 1px red;color:blue;border-top:solid 1px red\" width=\"25%\" align=\"right\"> " + TotalIncome.ToString() + " </td></tr>";
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
                Format += "<tr><td style=\"font-weight: bold;color: Green;\" align=\"left\">GROSS PROFIT FOR THE PERIOD </td><td style=\"font-weight: bold;padding-right:10px;color: Green;\" align=\"right\">" + NetProfit.ToString() + " </td><td style=\"font-weight: bold;color: Green;\" align=\"right\"></td> <td></td></tr>";
            }
            else if (NetLoss != 0)
            {
                Format += "<tr><td style=\"font-weight: bold;color: Green;\" align=\"right\"> </td><td style=\"font-weight: bold;color: Green;\" align=\"right\"></td> <td style=\"font-weight: bold;color: Green;\" align=\"left\"> GROSS LOSS FOR THE PERIOD</td><td style=\"font-weight: bold;color: Green;\" align=\"right\">" + NetLoss.ToString() + "</td></tr>";
            }
            decimal FinalTotalP = 0;
            decimal FinalTotalL = 0;
            FinalTotalL = NetLoss + Convert.ToDecimal(TotalIncome);
            FinalTotalP = NetProfit + Convert.ToDecimal(TotalExpence);
            Format += "<tr><td style=\"font-weight: bold;border-top:solid red 1px\" align=\"left\">Total</td><td style=\"font-weight: bold;border-top:solid red 1px;border-right:solid red 1px;padding-right:10px\" align=\"right\" >" + FinalTotalL + "</td><td style=\"font-weight: bold;border-top:solid red 1px\" align=\"left\">Total</td><td style=\"font-weight: bold;border-top:solid red 1px\" align=\"right\">" + FinalTotalP + "</td></tr>";
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
        protected void ddlExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable DtForIncome = new DataTable();
            DataTable DtForExpence = new DataTable();
            DataTable DtForTotalIncome = new DataTable();
            DataTable DtForTotalExpence = new DataTable();
            if (rdselected.Checked == true && rdSelectedBranch.Checked == true)
            {
                DtForIncome = oDBEngine.GetDataTable("trans_accountsledger as a inner join master_mainaccount as b on a.accountsledger_mainaccountid=b.mainaccount_accountcode", " b.mainaccount_accountcode,cast(sum(a.accountsledger_amountcr-a.accountsledger_amountdr)as money)as INCOME,ltrim(rtrim(b.mainaccount_name ))as account_name", "mainaccount_accounttype='Income' and a.accountsledger_branchid in(" + Session["KeyVal"].ToString() + ") and accountsledger_exchangesegmentid in(" + Session["KeyValSegment"].ToString() + ") and a.accountsledger_transactiondate>= '" + AspxFromDate.Value.ToString() + "' and a.accountsledger_transactiondate<='" + AspxTodate.Value.ToString() + "' and accountsledger_companyid in('" + Session["LastCompany"].ToString() + @"' 
            and isnull(accountsledger_currency," + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0] + "  group by b.mainaccount_accountcode,b.mainaccount_name ", "b.mainaccount_name");
                DtForExpence = oDBEngine.GetDataTable("trans_accountsledger as a inner join master_mainaccount as b on a.accountsledger_mainaccountid=b.mainaccount_accountcode", "b.mainaccount_accountcode,cast(sum(a.accountsledger_amountdr-a.accountsledger_amountcr)as money)as expenditure,ltrim(rtrim(b.mainaccount_name ))as account_name", "mainaccount_accounttype='Expences' and a.accountsledger_branchid in(" + Session["KeyVal"].ToString() + ") and accountsledger_exchangesegmentid in(" + Session["KeyValSegment"].ToString() + ") and a.accountsledger_transactiondate>= '" + AspxFromDate.Value.ToString() + "' and a.accountsledger_transactiondate<='" + AspxTodate.Value.ToString() + "' and accountsledger_companyid in('" + Session["LastCompany"].ToString() + @"' 
            and isnull(accountsledger_currency," + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0] + " group by b.mainaccount_accountcode,b.mainaccount_name", "b.mainaccount_name");
                DtForTotalIncome = oDBEngine.GetDataTable("trans_accountsledger as a inner join master_mainaccount as b on a.accountsledger_mainaccountid=b.mainaccount_accountcode", " isnull(cast(sum(a.accountsledger_amountcr-a.accountsledger_amountdr)as money),0)as INCOME", "mainaccount_accounttype='Income' and a.accountsledger_branchid in(" + Session["KeyVal"].ToString() + ") and accountsledger_exchangesegmentid in(" + Session["KeyValSegment"].ToString() + ") and a.accountsledger_transactiondate>= '" + AspxFromDate.Value.ToString() + "' and a.accountsledger_transactiondate<='" + AspxTodate.Value.ToString() + "' and accountsledger_companyid='" + Session["LastCompany"].ToString() + @"' 
            and isnull(accountsledger_currency," + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0]);
                DtForTotalExpence = oDBEngine.GetDataTable("trans_accountsledger as a inner join master_mainaccount as b on a.accountsledger_mainaccountid=b.mainaccount_accountcode", " isnull(cast(sum(a.accountsledger_amountdr-a.accountsledger_amountcr)as money),0)as Expences", "mainaccount_accounttype='Expences' and a.accountsledger_branchid in(" + Session["KeyVal"].ToString() + ") and accountsledger_exchangesegmentid in(" + Session["KeyValSegment"].ToString() + ") and a.accountsledger_transactiondate>= '" + AspxFromDate.Value.ToString() + "' and a.accountsledger_transactiondate<='" + AspxTodate.Value.ToString() + "' and accountsledger_companyid='" + Session["LastCompany"].ToString() + @"' 
            and isnull(accountsledger_currency," + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0]);
            }
            else if (rdAll.Checked == true && rdAllBranch.Checked == true)
            {
                DtForIncome = oDBEngine.GetDataTable("trans_accountsledger as a inner join master_mainaccount as b on a.accountsledger_mainaccountid=b.mainaccount_accountcode", " b.mainaccount_accountcode,cast(sum(a.accountsledger_amountcr-a.accountsledger_amountdr)as money)as INCOME,ltrim(rtrim(b.mainaccount_name ))as account_name", "mainaccount_accounttype='Income' and  a.accountsledger_transactiondate>= '" + AspxFromDate.Value.ToString() + "' and a.accountsledger_transactiondate<='" + AspxTodate.Value.ToString() + "' and accountsledger_companyid='" + Session["LastCompany"].ToString() + @"' 
            and isnull(accountsledger_currency," + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0] + " group by b.mainaccount_accountcode,b.mainaccount_name ", "b.mainaccount_name");
                DtForExpence = oDBEngine.GetDataTable("trans_accountsledger as a inner join master_mainaccount as b on a.accountsledger_mainaccountid=b.mainaccount_accountcode", "b.mainaccount_accountcode,cast(sum(a.accountsledger_amountdr-a.accountsledger_amountcr)as money)as expenditure,ltrim(rtrim(b.mainaccount_name ))as account_name", "mainaccount_accounttype='Expences' and a.accountsledger_transactiondate>= '" + AspxFromDate.Value.ToString() + "' and a.accountsledger_transactiondate<='" + AspxTodate.Value.ToString() + "' and accountsledger_companyid='" + Session["LastCompany"].ToString() + @"' 
            and isnull(accountsledger_currency," + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0] + " group by b.mainaccount_accountcode,b.mainaccount_name", "b.mainaccount_name");
                DtForTotalIncome = oDBEngine.GetDataTable("trans_accountsledger as a inner join master_mainaccount as b on a.accountsledger_mainaccountid=b.mainaccount_accountcode", " isnull(cast(sum(a.accountsledger_amountcr-a.accountsledger_amountdr)as money),0)as INCOME", "mainaccount_accounttype='Income' and a.accountsledger_transactiondate>= '" + AspxFromDate.Value.ToString() + "' and a.accountsledger_transactiondate<='" + AspxTodate.Value.ToString() + "' and accountsledger_companyid='" + Session["LastCompany"].ToString() + @"' 
            and isnull(accountsledger_currency," + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0]);
                DtForTotalExpence = oDBEngine.GetDataTable("trans_accountsledger as a inner join master_mainaccount as b on a.accountsledger_mainaccountid=b.mainaccount_accountcode", " isnull(cast(sum(a.accountsledger_amountdr-a.accountsledger_amountcr)as money),0)as Expences", "mainaccount_accounttype='Expences' and a.accountsledger_transactiondate>= '" + AspxFromDate.Value.ToString() + "' and a.accountsledger_transactiondate<='" + AspxTodate.Value.ToString() + "' and accountsledger_companyid='" + Session["LastCompany"].ToString() + @"' 
            and isnull(accountsledger_currency," + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0]);
            }
            else if (rdselected.Checked == true && rdAllBranch.Checked == true)
            {
                DtForIncome = oDBEngine.GetDataTable("trans_accountsledger as a inner join master_mainaccount as b on a.accountsledger_mainaccountid=b.mainaccount_accountcode", " b.mainaccount_accountcode,cast(sum(a.accountsledger_amountcr-a.accountsledger_amountdr)as money)as INCOME,ltrim(rtrim(b.mainaccount_name ))as account_name", "mainaccount_accounttype='Income'  and accountsledger_exchangesegmentid in(" + Session["KeyValSegment"].ToString() + ") and a.accountsledger_transactiondate>= '" + AspxFromDate.Value.ToString() + "' and a.accountsledger_transactiondate<='" + AspxTodate.Value.ToString() + "' and accountsledger_companyid='" + Session["LastCompany"].ToString() + @"' 
            and isnull(accountsledger_currency," + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0] + "  group by b.mainaccount_accountcode,b.mainaccount_name ", "b.mainaccount_name");
                DtForExpence = oDBEngine.GetDataTable("trans_accountsledger as a inner join master_mainaccount as b on a.accountsledger_mainaccountid=b.mainaccount_accountcode", "b.mainaccount_accountcode,cast(sum(a.accountsledger_amountdr-a.accountsledger_amountcr)as money)as expenditure,ltrim(rtrim(b.mainaccount_name ))as account_name", "mainaccount_accounttype='Expences'  and accountsledger_exchangesegmentid in(" + Session["KeyValSegment"].ToString() + ") and a.accountsledger_transactiondate>= '" + AspxFromDate.Value.ToString() + "' and a.accountsledger_transactiondate<='" + AspxTodate.Value.ToString() + "' and accountsledger_companyid='" + Session["LastCompany"].ToString() + @"' 
            and isnull(accountsledger_currency," + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0] + " group by b.mainaccount_accountcode,b.mainaccount_name", "b.mainaccount_name");
                DtForTotalIncome = oDBEngine.GetDataTable("trans_accountsledger as a inner join master_mainaccount as b on a.accountsledger_mainaccountid=b.mainaccount_accountcode", " isnull(cast(sum(a.accountsledger_amountcr-a.accountsledger_amountdr)as money),0)as INCOME", "mainaccount_accounttype='Income'  and accountsledger_exchangesegmentid in(" + Session["KeyValSegment"].ToString() + ") and a.accountsledger_transactiondate>= '" + AspxFromDate.Value.ToString() + "' and a.accountsledger_transactiondate<='" + AspxTodate.Value.ToString() + "' and accountsledger_companyid='" + Session["LastCompany"].ToString() + @"' 
            and isnull(accountsledger_currency," + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0]);
                DtForTotalExpence = oDBEngine.GetDataTable("trans_accountsledger as a inner join master_mainaccount as b on a.accountsledger_mainaccountid=b.mainaccount_accountcode", " isnull(cast(sum(a.accountsledger_amountdr-a.accountsledger_amountcr)as money),0)as Expences", "mainaccount_accounttype='Expences' and accountsledger_exchangesegmentid in(" + Session["KeyValSegment"].ToString() + ") and a.accountsledger_transactiondate>= '" + AspxFromDate.Value.ToString() + "' and a.accountsledger_transactiondate<='" + AspxTodate.Value.ToString() + "' and accountsledger_companyid='" + Session["LastCompany"].ToString() + @"' 
            and isnull(accountsledger_currency," + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0]);
            }
            else if (rdAll.Checked == true && rdSelectedBranch.Checked == true)
            {
                DtForIncome = oDBEngine.GetDataTable("trans_accountsledger as a inner join master_mainaccount as b on a.accountsledger_mainaccountid=b.mainaccount_accountcode", " b.mainaccount_accountcode,cast(sum(a.accountsledger_amountcr-a.accountsledger_amountdr)as money)as INCOME,ltrim(rtrim(b.mainaccount_name ))as account_name", "mainaccount_accounttype='Income' and a.accountsledger_branchid in(" + Session["KeyVal"].ToString() + ") and a.accountsledger_transactiondate>= '" + AspxFromDate.Value.ToString() + "' and a.accountsledger_transactiondate<='" + AspxTodate.Value.ToString() + "' and accountsledger_companyid='" + Session["LastCompany"].ToString() + @"' 
            and isnull(accountsledger_currency," + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0] + "  group by b.mainaccount_accountcode,b.mainaccount_name ", "b.mainaccount_name");
                DtForExpence = oDBEngine.GetDataTable("trans_accountsledger as a inner join master_mainaccount as b on a.accountsledger_mainaccountid=b.mainaccount_accountcode", "b.mainaccount_accountcode,cast(sum(a.accountsledger_amountdr-a.accountsledger_amountcr)as money)as expenditure,ltrim(rtrim(b.mainaccount_name ))as account_name", "mainaccount_accounttype='Expences' and a.accountsledger_branchid in(" + Session["KeyVal"].ToString() + ") and a.accountsledger_transactiondate>= '" + AspxFromDate.Value.ToString() + "' and a.accountsledger_transactiondate<='" + AspxTodate.Value.ToString() + "' and accountsledger_companyid='" + Session["LastCompany"].ToString() + @"' 
            and isnull(accountsledger_currency," + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0] + " group by b.mainaccount_accountcode,b.mainaccount_name", "b.mainaccount_name");
                DtForTotalIncome = oDBEngine.GetDataTable("trans_accountsledger as a inner join master_mainaccount as b on a.accountsledger_mainaccountid=b.mainaccount_accountcode", " isnull(cast(sum(a.accountsledger_amountcr-a.accountsledger_amountdr)as money),0)as INCOME", "mainaccount_accounttype='Income' and a.accountsledger_branchid in(" + Session["KeyVal"].ToString() + ")  and a.accountsledger_transactiondate>= '" + AspxFromDate.Value.ToString() + "' and a.accountsledger_transactiondate<='" + AspxTodate.Value.ToString() + "' and accountsledger_companyid='" + Session["LastCompany"].ToString() + @"' 
            and isnull(accountsledger_currency," + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0]);
                DtForTotalExpence = oDBEngine.GetDataTable("trans_accountsledger as a inner join master_mainaccount as b on a.accountsledger_mainaccountid=b.mainaccount_accountcode", " isnull(cast(sum(a.accountsledger_amountdr-a.accountsledger_amountcr)as money),0)as Expences", "mainaccount_accounttype='Expences' and a.accountsledger_branchid in(" + Session["KeyVal"].ToString() + ") and a.accountsledger_transactiondate>= '" + AspxFromDate.Value.ToString() + "' and a.accountsledger_transactiondate<='" + AspxTodate.Value.ToString() + "' and accountsledger_companyid='" + Session["LastCompany"].ToString() + @"' 
            and isnull(accountsledger_currency," + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0]);
            }
            DataTable DtForExportData = new DataTable();
            DataColumn dc = new DataColumn();
            dc.DataType = System.Type.GetType("System.String");
            dc.ColumnName = "Particulars.";
            DtForExportData.Columns.Add(dc);
            DataColumn dc1 = new DataColumn();
            dc1.DataType = System.Type.GetType("System.String");
            dc1.ColumnName = "Amount";
            DtForExportData.Columns.Add(dc1);
            DataColumn dc2 = new DataColumn();
            dc2.DataType = System.Type.GetType("System.String");
            dc2.ColumnName = "Particulars";
            DtForExportData.Columns.Add(dc2);
            DataColumn dc3 = new DataColumn();
            dc3.DataType = System.Type.GetType("System.String");
            dc3.ColumnName = "Amount.";
            DtForExportData.Columns.Add(dc3);
            int TotInRecords = DtForIncome.Rows.Count;
            int TotExRecords = DtForExpence.Rows.Count;
            int upperlimit = 0;
            int nextupperlimitincome = 0;
            int nextupperlimitexpence = 0;
            if (TotInRecords >= TotExRecords)
            {
                upperlimit = TotExRecords;
                nextupperlimitincome = TotInRecords - TotExRecords;
            }
            else
            {
                upperlimit = TotInRecords;
                nextupperlimitexpence = TotExRecords - TotInRecords;
            }
            for (int i = 0; i < upperlimit; i++)
            {
                DataRow dr = DtForExportData.NewRow();
                dr["Particulars."] = DtForExpence.Rows[i]["account_name"].ToString();
                dr["Amount"] = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(DtForExpence.Rows[i]["expenditure"].ToString()));
                dr["Particulars"] = DtForIncome.Rows[i]["account_name"].ToString();
                dr["Amount."] = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(DtForIncome.Rows[i]["INCOME"].ToString()));
                DtForExportData.Rows.Add(dr);
            }
            if (nextupperlimitexpence > nextupperlimitincome)
            {
                for (int i = upperlimit; i < (upperlimit + nextupperlimitexpence); i++)
                {
                    DataRow dr = DtForExportData.NewRow();
                    dr["Particulars."] = DtForExpence.Rows[i]["account_name"].ToString();
                    dr["Amount"] = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(DtForExpence.Rows[i]["expenditure"].ToString()));
                    //dr["IncomeAccountsName"] = "-";
                    //dr["IncomeAmount"] = "-";
                    dr["Particulars"] = "-";
                    dr["Amount."] = "-";
                    DtForExportData.Rows.Add(dr);
                }
            }
            else
            {
                for (int i = upperlimit; i < (upperlimit + nextupperlimitincome); i++)
                {
                    DataRow dr = DtForExportData.NewRow();
                    dr["Particulars."] = "-";
                    dr["Amount"] = "-";
                    dr["Particulars"] = DtForIncome.Rows[i]["account_name"].ToString();
                    dr["Amount."] = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(DtForIncome.Rows[i]["INCOME"].ToString()));
                    DtForExportData.Rows.Add(dr);
                }
            }

            DataRow drForTotal = DtForExportData.NewRow();
            drForTotal["Particulars."] = "Total";
            drForTotal["Amount"] = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(DtForTotalExpence.Rows[0][0].ToString()));
            drForTotal["Particulars"] = "Total";
            drForTotal["Amount."] = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(DtForTotalIncome.Rows[0][0].ToString()));
            DtForExportData.Rows.Add(drForTotal);

            decimal totProfit = 0;
            totProfit = Convert.ToDecimal(DtForTotalIncome.Rows[0][0].ToString());
            decimal totLoss = 0;
            totLoss = Convert.ToDecimal(DtForTotalExpence.Rows[0][0].ToString());
            decimal NetProfit = 0;
            decimal NetLoss = 0;
            if (totProfit > totLoss)
            {
                NetProfit = totProfit - totLoss;
            }
            else
            {
                NetLoss = totLoss - totProfit;
            }
            if (NetProfit > 0)
            {
                DataRow drNet = DtForExportData.NewRow();
                drNet["Particulars."] = "GROSS PROFIT FOR THE PERIOD " + objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(NetProfit.ToString()));
                drNet["Amount"] = "Test";
                //drNet["Particulars"] = "GROSS LOSS FOR THE PERIOD";
                //drNet["Amount."] = NetLoss.ToString();
                DtForExportData.Rows.Add(drNet);
            }
            else
            {
                DataRow drNet = DtForExportData.NewRow();
                //drNet["Particulars."] = "GROSS PROFIT FOR THE PERIOD";
                //drNet["Amount"] = NetProfit.ToString();
                drNet["Particulars"] = "GROSS LOSS FOR THE PERIOD";
                drNet["Amount."] = NetLoss.ToString();
                DtForExportData.Rows.Add(drNet);
            }
            DataRow drLast = DtForExportData.NewRow();
            drLast["Particulars."] = "Total";
            drLast["Amount"] = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(NetProfit + totLoss));
            drLast["Particulars"] = "Total";
            drLast["Amount."] = objConverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(NetLoss + totProfit));
            DtForExportData.Rows.Add(drLast);
            DataTable SegmentList = new DataTable();
            if (rdselected.Checked == true)
            {
                SegmentList = oDBEngine.GetDataTable("(select isnull((TME.EXH_ShortName + '-' + A.EXCH_SEGMENTID),exch_membershipType) AS EXCHANGENAME from (SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='" + Session["CompanyID"].ToString() + "' and EXCH_INTERNALID in(" + Session["KeyValSegment"].ToString() + ") ) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID ) as D ", "top 10 *", null);
            }
            DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            DataTable dtReportHeader = new DataTable();
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            HeaderRow[0] = CompanyName.Rows[0][0].ToString();
            dtReportHeader.Rows.Add(HeaderRow);
            DataRow DrRowR1 = dtReportHeader.NewRow();
            string Header = "";
            if (SegmentList.Rows.Count > 0)
            {
                string Sl = "";
                for (int i = 0; i < SegmentList.Rows.Count; i++)
                {

                    Sl += SegmentList.Rows[i][0].ToString();
                }
                Header = "Profit & Loss Statement [For The Period From " + objConverter.ArrangeDate2(AspxFromDate.Value.ToString()) + " To " + objConverter.ArrangeDate2(AspxTodate.Value.ToString()) + "]" + "Segment:" + Sl;
            }
            else
            {
                Header = "Profit & Loss Statement [For The Period From " + objConverter.ArrangeDate2(AspxFromDate.Value.ToString()) + " To " + objConverter.ArrangeDate2(AspxTodate.Value.ToString()) + "]";

            }
            DrRowR1[0] = Header;
            dtReportHeader.Rows.Add(DrRowR1);
            DataRow HeaderRow1 = dtReportHeader.NewRow();
            dtReportHeader.Rows.Add(HeaderRow1);
            DataRow HeaderRow2 = dtReportHeader.NewRow();
            dtReportHeader.Rows.Add(HeaderRow2);
            DataTable dtReportFooter = new DataTable();
            dtReportFooter.Columns.Add(new DataColumn("Footer", typeof(String))); //0
            DataRow FooterRow1 = dtReportFooter.NewRow();
            dtReportFooter.Rows.Add(FooterRow1);
            DataRow FooterRow2 = dtReportFooter.NewRow();
            dtReportFooter.Rows.Add(FooterRow2);
            DataRow FooterRow = dtReportFooter.NewRow();
            //FooterRow[0] = "* * *  End Of Report * * *         [" + oconverter.ArrangeDate2(oDBEngine.GetDate().ToString(), "Test") + "]";
            FooterRow[0] = "* * *  End Of Report * * *   ";
            dtReportFooter.Rows.Add(FooterRow);
            if (ddlExport.SelectedItem.Value == "E")
            {
                //oconverter.ExcelImport(dtBilling, "Daily Billing");
                //ddlExport.SelectedIndex = -1;
                //ddlExport.SelectedValue = "Ex";
                //ddlExport.SelectedItem.Text = "Export";
                objExcel.ExportToExcelforExcel(DtForExportData, "Profit & Loss Statement", "Total", dtReportHeader, dtReportFooter);
            }
            else if (ddlExport.SelectedItem.Value == "P")
            {
                objExcel.ExportToPDF(DtForExportData, "Profit & Loss Statement", "Total", dtReportHeader, dtReportFooter);
            }

        }
    }
}