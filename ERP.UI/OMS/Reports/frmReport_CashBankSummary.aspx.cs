using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_frmReport_CashBankSummary : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        static string Branch;
        static string Segment;
        string data;
        decimal openingBal = 0;
        decimal debitTotal = 0;
        decimal creditTotal = 0;
        string SegmentID = null;
        static DataTable DtOpenBalance = new DataTable();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        ExcelFile objExcel = new ExcelFile();
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
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            if (!IsPostBack)
            {
                SetDatteFinYear();

                dtDate.EditFormatString = oconverter.GetDateFormat("Date");
                // dtDate.Value = Convert.ToDateTime(DateTime.Today);
               // DataTable DtSegComp = oDBEngine.GetDataTable("(select exch_compId,exch_internalId,(select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId as Comp from tbl_master_companyExchange where exch_compid in (select top 1 ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment=" + Session["userlastsegment"].ToString() + ")) as D", "*", "Comp in(select seg_name from tbl_master_segment where seg_id=" + Session["userlastsegment"].ToString() + ")");
                DataTable DtSegComp = oDBEngine.GetDataTable("(select top 1 exch_compId,exch_internalId,case when exch_segmentId is null then (select top 1  exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId) else (select top 1  exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId end as Comp from tbl_master_companyExchange where exch_compid in (select top 1 ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment=" + Session["userlastsegment"].ToString() + ")) as D", "*", "Comp in(select seg_name from tbl_master_segment where seg_id=" + Session["userlastsegment"].ToString() + ")");
                if (DtSegComp.Rows.Count > 0)
                {
                    string CompanyID = DtSegComp.Rows[0][0].ToString();
                    Session["CompanyID"] = CompanyID;
                    SegmentID = DtSegComp.Rows[0][1].ToString();
                    litSegment.InnerText = DtSegComp.Rows[0][2].ToString();
                    Session["SegmentID"] = SegmentID;
                    BindGrid();
                }
                else if (Session["userlastsegment"].ToString() == "5")
                {
                    Session["CompanyID"] = Session["LastCompany"].ToString();
                    SegmentID = "0";
                    Session["SegmentID"] = "0";
                    BindGrid();
                }
                rdbSegAll.Attributes.Add("OnClick", "SegAll('seg')");
                rdbSegSelected.Attributes.Add("OnClick", "SegSelected('seg')");
                rdbbAll.Attributes.Add("OnClick", "SegAll('branch')");
                rdbbSelected.Attributes.Add("OnClick", "SegSelected('branch')");
                txtsubscriptionID.Attributes.Add("onkeyup", "showOptions(this,'SearchBankSegmentBranch',event)");
            }
            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //___________-end here___//
            Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load()</script>");
        }

        protected void SetDatteFinYear()
        {
            DataTable dtFinYear = oDBEngine.GetDataTable("MASTER_FINYEAR ", " FINYEAR_ENDDATE ", " FINYEAR_CODE ='" + Session["LastFinYear"].ToString() + "'");
            DateTime EndDate = Convert.ToDateTime(dtFinYear.Rows[0][0].ToString());
            DateTime TodayDate = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
            if (EndDate < TodayDate)
                dtDate.Value = EndDate;
            else
                dtDate.Value = TodayDate;

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
            if (idlist[0] == "Branch")
            {
                Branch = str;
                data = "Branch~" + str1;
            }
            else if (idlist[0] == "Segment")
            {
                Segment = str;
                data = "Segment~" + str1;
            }

        }
        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }
        public void BindGrid()
        {
            System.Globalization.NumberFormatInfo currencyFormat = new System.Globalization.CultureInfo("hi-IN").NumberFormat;
            currencyFormat.CurrencySymbol = "";
            currencyFormat.CurrencyNegativePattern = 2;
            DtOpenBalance = oDBEngine.GetDataTable("trans_accountsledger", @"row_number() over(order by AccountsLedger_MainAccountID) as SRNO, 
        AccountsLedger_MainAccountID,(select MainAccount_Name from Master_MainAccount 
        where MainAccount_AccountCode=trans_accountsledger.AccountsLedger_MainAccountID) as AccountsLedger_CashBankName,
        (select MainAccount_BankAcNumber from master_mainaccount where 
        ltrim(rtrim(MainAccount_AccountCode))=ltrim(rtrim(trans_accountsledger.AccountsLedger_MainAccountID))) as AcNumber,
        case when sum(AccountsLedger_AmountDr-AccountsLedger_AmountCr)<0 then null else sum(AccountsLedger_AmountDr-AccountsLedger_AmountCr) 
        end as Dr,case when sum(AccountsLedger_AmountCr-AccountsLedger_AmountDr)<0 then null 
        else sum(AccountsLedger_AmountCr-AccountsLedger_AmountDr) end as Cr ", " AccountsLedger_BranchId in("
            + Session["userbranchHierarchy"].ToString() + @") and 
        cast(DATEADD(dd, 0, DATEDIFF(dd, 0,AccountsLedger_TransactionDate)) as datetime)<=cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + Convert.ToDateTime(dtDate.Value).ToString("MM-dd-yyyy") + @"')) as datetime) 
        and AccountsLedger_MainAccountID in(select MainAccount_AccountCode from Master_MainAccount 
        where MainAccount_BankCashType='Bank' or MainAccount_BankCashType='Cash') and AccountsLedger_TransactionType<>'Journal' 
        and AccountsLedger_CompanyID='" + Session["CompanyID"].ToString() + "' and AccountsLedger_ExchangeSegmentID in(" + SegmentID + @") 
        and AccountsLedger_FinYear='" + Session["LastFinYear"].ToString() + @"'
        and isnull(AccountsLedger_Currency," + Session["TradeCurrency"].ToString().Split('~')[0] + ")='" + Session["ActiveCurrency"].ToString().Split('~')[0] +
            "'group by AccountsLedger_MainAccountID");
            if (DtOpenBalance.Rows.Count > 0)
            {
                grdCashBankSummary.DataSource = DtOpenBalance;
                grdCashBankSummary.DataBind();
                grdCashBankSummary.FooterRow.Cells[0].Text = "Total";
                grdCashBankSummary.FooterRow.Cells[0].ForeColor = System.Drawing.Color.White;
                grdCashBankSummary.FooterRow.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                grdCashBankSummary.FooterRow.Cells[0].Font.Bold = true;
                if (debitTotal != 0)
                {
                    grdCashBankSummary.FooterRow.Cells[4].Text = debitTotal.ToString("c", currencyFormat);
                    grdCashBankSummary.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Right;
                    grdCashBankSummary.FooterRow.Cells[4].ForeColor = System.Drawing.Color.White;
                    grdCashBankSummary.FooterRow.Cells[4].Font.Bold = true;
                }
                else
                {
                    grdCashBankSummary.FooterRow.Cells[4].Text = "";
                }
                if (creditTotal != 0)
                {
                    grdCashBankSummary.FooterRow.Cells[5].Text = creditTotal.ToString("c", currencyFormat);
                    grdCashBankSummary.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Right;
                    grdCashBankSummary.FooterRow.Cells[5].ForeColor = System.Drawing.Color.White;
                    grdCashBankSummary.FooterRow.Cells[5].Font.Bold = true;
                }
                else
                {
                    grdCashBankSummary.FooterRow.Cells[5].Text = "";
                }
            }
        }
        protected void grdCashBankSummary_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string lcVar1 = ((DataRowView)e.Row.DataItem)["Dr"].ToString();
                string lcVar2 = ((DataRowView)e.Row.DataItem)["Cr"].ToString();
                if (lcVar1 == "")
                {
                    lcVar1 = "0";
                    e.Row.Cells[4].Text = "";
                }
                else
                    e.Row.Cells[4].Text = oconverter.getFormattedvalue(decimal.Parse(lcVar1));

                if (lcVar2 == "")
                {
                    lcVar2 = "0";
                    e.Row.Cells[5].Text = "";
                }
                else
                    e.Row.Cells[5].Text = oconverter.getFormattedvalue(decimal.Parse(lcVar2));
                debitTotal += decimal.Parse(lcVar1);
                creditTotal += decimal.Parse(lcVar2);
                System.Globalization.NumberFormatInfo currencyFormat = new System.Globalization.CultureInfo("hi-IN").NumberFormat;
                currencyFormat.CurrencySymbol = "";
                currencyFormat.CurrencyNegativePattern = 2;
                //openingBal = decimal.Parse(lcVar1) - decimal.Parse(lcVar2) + openingBal;
                //if (openingBal < 0)
                //{
                //    e.Row.Cells[6].Text = openingBal.ToString("c", currencyFormat).Substring(1, openingBal.ToString("c", currencyFormat).Length - 1);
                //    e.Row.Cells[7].Text = "Cr";
                //}
                //else
                //{
                //    e.Row.Cells[6].Text = openingBal.ToString("c", currencyFormat);
                //    e.Row.Cells[7].Text = "Dr";
                //}
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            System.Globalization.NumberFormatInfo currencyFormat = new System.Globalization.CultureInfo("hi-IN").NumberFormat;
            currencyFormat.CurrencySymbol = "";
            currencyFormat.CurrencyNegativePattern = 2;
            if (rdbSegAll.Checked == true)
            {
                DataTable dtSegment = oDBEngine.GetDataTable("tbl_master_companyExchange", "exch_internalId", "exch_compId='" + Session["CompanyID"].ToString() + "'");
                if (dtSegment.Rows.Count > 0)
                {
                    for (int i = 0; i < dtSegment.Rows.Count; i++)
                    {
                        if (Segment == null)
                            Segment = dtSegment.Rows[i][0].ToString();
                        else
                            Segment += "," + dtSegment.Rows[i][0].ToString();
                    }
                }
            }
            if (rdbbAll.Checked == true)
            {
                Branch = Session["userbranchHierarchy"].ToString();
            }
            if (Segment == null)
            {
                Segment = Session["SegmentID"].ToString();
            }
            DtOpenBalance = oDBEngine.GetDataTable("trans_accountsledger", "row_number() over(order by AccountsLedger_MainAccountID) as SRNO, AccountsLedger_MainAccountID,(select MainAccount_Name from Master_MainAccount where MainAccount_AccountCode=trans_accountsledger.AccountsLedger_MainAccountID) as AccountsLedger_CashBankName,(select MainAccount_BankAcNumber from master_mainaccount where ltrim(rtrim(MainAccount_AccountCode))=ltrim(rtrim(trans_accountsledger.AccountsLedger_MainAccountID))) as AcNumber,case when sum(AccountsLedger_AmountDr-AccountsLedger_AmountCr)<0 then null else sum(AccountsLedger_AmountDr-AccountsLedger_AmountCr) end as Dr,case when sum(AccountsLedger_AmountCr-AccountsLedger_AmountDr)<0 then null else sum(AccountsLedger_AmountCr-AccountsLedger_AmountDr) end as Cr ", " AccountsLedger_BranchId in(" + Branch + ") and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,AccountsLedger_TransactionDate)) as datetime)<=cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtDate.Value + "')) as datetime) and AccountsLedger_MainAccountID in(select MainAccount_AccountCode from Master_MainAccount where MainAccount_BankCashType='Bank' or MainAccount_BankCashType='Cash') and AccountsLedger_TransactionType<>'Journal' and AccountsLedger_CompanyID='" + Session["CompanyID"].ToString() + "' and AccountsLedger_ExchangeSegmentID in(" + Segment + ") and AccountsLedger_FinYear='" + Session["LastFinYear"].ToString() + "' group by AccountsLedger_MainAccountID");
            if (DtOpenBalance.Rows.Count > 0)
            {
                grdCashBankSummary.DataSource = DtOpenBalance;
                grdCashBankSummary.DataBind();
                grdCashBankSummary.FooterRow.Cells[0].Text = "Total";
                grdCashBankSummary.FooterRow.Cells[0].ForeColor = System.Drawing.Color.White;
                grdCashBankSummary.FooterRow.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                grdCashBankSummary.FooterRow.Cells[0].Font.Bold = true;
                if (debitTotal != 0)
                {
                    grdCashBankSummary.FooterRow.Cells[4].Text = debitTotal.ToString("c", currencyFormat);
                    grdCashBankSummary.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Right;
                    grdCashBankSummary.FooterRow.Cells[4].ForeColor = System.Drawing.Color.White;
                    grdCashBankSummary.FooterRow.Cells[4].Font.Bold = true;
                }
                else
                {
                    grdCashBankSummary.FooterRow.Cells[4].Text = "";
                }
                if (creditTotal != 0)
                {
                    grdCashBankSummary.FooterRow.Cells[5].Text = creditTotal.ToString("c", currencyFormat);
                    grdCashBankSummary.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Right;
                    grdCashBankSummary.FooterRow.Cells[5].ForeColor = System.Drawing.Color.White;
                    grdCashBankSummary.FooterRow.Cells[5].Font.Bold = true;
                }
                else
                {
                    grdCashBankSummary.FooterRow.Cells[5].Text = "";
                }
            }
            else
            {
                grdCashBankSummary.DataSource = DtOpenBalance;
                grdCashBankSummary.DataBind();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Scpt", "CheckDataExists()", true);
            }
            //Branch = null;
            //Segment = null;
        }
        protected void grdCashBankSummary_RowCreated(object sender, GridViewRowEventArgs e)
        {
            string rowID = String.Empty;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                rowID = "row" + e.Row.RowIndex;

                e.Row.Attributes.Add("id", "row" + e.Row.RowIndex);

                e.Row.Attributes.Add("onclick", "ChangeRowColor(" + "'" + rowID + "','" + DtOpenBalance.Rows.Count + "','" + grdCashBankSummary.ClientID + "'" + ")");

            }
        }
        protected void ddlExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            decimal debit = 0;
            decimal credit = 0;
            if (rdbSegAll.Checked == true)
            {
                DataTable dtSegment = oDBEngine.GetDataTable("tbl_master_companyExchange", "exch_internalId", "exch_compId='" + Session["CompanyID"].ToString() + "'");
                if (dtSegment.Rows.Count > 0)
                {
                    for (int i = 0; i < dtSegment.Rows.Count; i++)
                    {
                        if (Segment == null)
                            Segment = dtSegment.Rows[i][0].ToString();
                        else
                            Segment += "," + dtSegment.Rows[i][0].ToString();
                    }
                }
            }
            if (rdbbAll.Checked == true)
            {
                Branch = Session["userbranchHierarchy"].ToString();
            }
            if (Segment == null)
            {
                Segment = Session["SegmentID"].ToString();
            }
            DataTable dtforExport = oDBEngine.GetDataTable("trans_accountsledger", "cast(row_number() over(order by AccountsLedger_MainAccountID) as varchar(20)) as SrNo, AccountsLedger_MainAccountID as BankCode,(select MainAccount_Name from Master_MainAccount where MainAccount_AccountCode=trans_accountsledger.AccountsLedger_MainAccountID) as BankName,(select MainAccount_BankAcNumber from master_mainaccount where ltrim(rtrim(MainAccount_AccountCode))=ltrim(rtrim(trans_accountsledger.AccountsLedger_MainAccountID))) as AccountNumber,case when sum(AccountsLedger_AmountDr-AccountsLedger_AmountCr)<0 then null else cast(sum(AccountsLedger_AmountDr-AccountsLedger_AmountCr) as varchar(max)) end as AmountDr,case when sum(AccountsLedger_AmountCr-AccountsLedger_AmountDr)<0 then null else cast(sum(AccountsLedger_AmountCr-AccountsLedger_AmountDr) as varchar(50)) end as AmountCr ", " AccountsLedger_BranchId in(" + Branch + ") and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,AccountsLedger_TransactionDate)) as datetime)<=cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtDate.Value + "')) as datetime) and AccountsLedger_MainAccountID in(select MainAccount_AccountCode from Master_MainAccount where MainAccount_BankCashType='Bank' or MainAccount_BankCashType='Cash') and AccountsLedger_TransactionType<>'Journal' and AccountsLedger_CompanyID='" + Session["CompanyID"].ToString() + "' and AccountsLedger_ExchangeSegmentID in(" + Segment + ") and AccountsLedger_FinYear='" + Session["LastFinYear"].ToString() + "' group by AccountsLedger_MainAccountID");
            if (dtforExport.Rows.Count > 0)
            {
                for (int k = 0; k < dtforExport.Rows.Count; k++)
                {
                    if (dtforExport.Rows[k]["AmountDr"].ToString() != "")
                        dtforExport.Rows[k]["AmountDr"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dtforExport.Rows[k]["AmountDr"].ToString()));
                    if (dtforExport.Rows[k]["AmountCr"].ToString() != "")
                        dtforExport.Rows[k]["AmountCr"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dtforExport.Rows[k]["AmountCr"].ToString()));
                    if (dtforExport.Rows[k]["AmountDr"] != DBNull.Value)
                        debit += Convert.ToDecimal(dtforExport.Rows[k]["AmountDr"].ToString());
                    if (dtforExport.Rows[k]["AmountCr"] != DBNull.Value)
                        credit += Convert.ToDecimal(dtforExport.Rows[k]["AmountCr"].ToString());

                }
            }
            DataRow DrRow = dtforExport.NewRow();
            dtforExport.Rows.Add(DrRow);
            DataRow newRow = dtforExport.NewRow();
            newRow[0] = "Total";
            if (debit != 0)
                newRow[4] = oconverter.formatmoneyinUs(debit);
            if (credit != 0)
                newRow[5] = oconverter.formatmoneyinUs(credit);
            dtforExport.Rows.Add(newRow);

            DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            DataTable dtReportHeader = new DataTable();
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            HeaderRow[0] = CompanyName.Rows[0][0].ToString();
            dtReportHeader.Rows.Add(HeaderRow);
            DataRow DrRowR1 = dtReportHeader.NewRow();
            DrRowR1[0] = "Cash/Bank Summary As On Date [" + oconverter.ArrangeDate2(dtDate.Value.ToString()) + "]";
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
                objExcel.ExportToExcelforExcel(dtforExport, "Cash Bank Summary", "Total", dtReportHeader, dtReportFooter);
            }
            else if (ddlExport.SelectedItem.Value == "P")
            {
                objExcel.ExportToPDF(dtforExport, "Cash Bank Summary", "Total", dtReportHeader, dtReportFooter);
            }
        }
    }
}