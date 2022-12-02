using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;
namespace ERP.OMS.Reports
{
    public partial class Reports_frmReport_TransactionNseCm : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        string data;
        static string Clients;
        static string Script;
        static string DPAccID = null;
        static DataTable dtExport = new DataTable();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        ExcelFile objExcel = new ExcelFile();
        BusinessLogicLayer.Reports rep = new BusinessLogicLayer.Reports();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "Js", "<script language='JavaScript'>Page_Load();</script>");
                DtFrom.Value = Convert.ToDateTime(oDBEngine.GetDate());
                dtTo.Value = Convert.ToDateTime(oDBEngine.GetDate());
                BindDPAccounts();
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
        public void BindDPAccounts()
        {
            DataTable DtDPAcc = oDBEngine.GetDataTable("Master_DPAccounts", "cast(DPAccounts_ID as varchar)+'~'+rtrim(DPAccounts_AccountType) as ID,DPAccounts_ShortName", " DPAccounts_CompanyID='" + Session["LastCompany"].ToString() + "' and DPAccounts_ExchangeSegmentID in(" + Session["usersegid"].ToString() + ",0)");
            ddlDPAc.DataSource = DtDPAcc;
            ddlDPAc.DataTextField = "DPAccounts_ShortName";
            ddlDPAc.DataValueField = "ID";
            ddlDPAc.DataBind();
            ddlDPAc.Items.Insert(0, new ListItem("All", "A~A"));
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            DPAccID = null;
            string SettlementNumber = null;
            string ProductName = null;
            string ForClientExchange = null;
            string OwnAccount = null;
            string[] DpAccount = ddlDPAc.SelectedItem.Value.Split('~');
            DPAccID = DpAccount[0].ToString();
            string DPAccType = DpAccount[1].ToString();
            ViewState["DPAccType"] = DPAccType.ToString();
            if (DPAccType == "[POOL]")
            {
                string SettNum = String.Empty;
                string SettType = String.Empty;
                if (txtSettlement.Text != "")
                {
                    SettNum = txtSettlement.Text.Substring(0, 7);
                    SettType = txtSettlement.Text.Substring(7, 1);
                }
                if (ddlSett.SelectedItem.Value == "A")
                    SettlementNumber = null;
                else if (ddlSett.SelectedItem.Value == "U")
                    SettlementNumber = " and (DematTransactions_SettlementNumberT<='" + SettNum + "' or DematTransactions_SettlementNumberS<='" + SettNum + "')";
                else if (ddlSett.SelectedItem.Value == "F")
                    SettlementNumber = " and ((DematTransactions_SettlementNumberT='" + SettNum + "' and DematTransactions_SettlementTypeT='" + SettType + "') or (DematTransactions_SettlementNumberS='" + SettNum + "' and DematTransactions_SettlementTypeS='" + SettType + "'))";
            }
            else
                SettlementNumber = null;
            if (radScripAll.Checked == true)
                ProductName = null;
            else
                ProductName = " and DematTransactions_ProductSeriesID in(" + Script + ")";
            if (radExchange.Checked == true)
            {
                ForClientExchange = " and DematTransactions_CustomerID not like 'CL%'";
            }
            else if (radBoth.Checked == true)
            {
                ForClientExchange = null;
            }
            else
            {
                if (radAll.Checked == true)
                    ForClientExchange = " and DematTransactions_CustomerID like 'CL%'";
                else if (radPOAClient.Checked == true)
                    ForClientExchange = " and DematTransactions_CustomerID in(select dpd_cntID from tbl_master_contactdpdetails where dpd_POA=1)";
                else
                    ForClientExchange = " and DematTransactions_CustomerID in(" + Clients + ")";
            }
            if (DPAccID == "A")
                OwnAccount = null;
            else
                OwnAccount = " and (DematTransactions_OwnAccountS=" + DPAccID + " or DematTransactions_OwnAccountT=" + DPAccID + ")";
            string WhereClause = " cast(DATEADD(dd, 0, DATEDIFF(dd, 0,DematTransactions_Date)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + DtFrom.Value + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtTo.Value + "')) as datetime) " + OwnAccount + " " + SettlementNumber + " " + ProductName + " " + ForClientExchange + "  and Equity_SeriesID=DematTransactions_ProductSeriesID";
            string OpenWhereClause = " cast(DATEADD(dd, 0, DATEDIFF(dd, 0,DematTransactions_Date)) as datetime)< cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + DtFrom.Value + "')) as datetime) " + OwnAccount + " " + SettlementNumber + " " + ProductName + " " + ForClientExchange + " and DematTransactions_FinYear='" + Session["LastFinYear"].ToString() + "'";
            DataTable DtRowCount = oDBEngine.GetDataTable("Trans_DematTransactions,Master_Equity", "count(DematTransactions_ISIN)", "" + WhereClause + "");
            if (DtRowCount.Rows.Count > 0)
            {
                if (Convert.ToInt32(DtRowCount.Rows[0][0].ToString()) > 500000000)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JS", "HideOn();", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JS", "HideOff();", true);
                }
            }
            ViewState["WhereClause"] = WhereClause;
            ViewState["OpenWhereClause"] = OpenWhereClause;
            ViewState["startRowIndex"] = 1;
            ViewState["pageSize"] = 500000000;
            ViewState["totalRecord"] = Convert.ToInt32(DtRowCount.Rows[0][0].ToString());
            btnTransnNext.Enabled = true;

            generateTable();
        }
        protected void ddlExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindForExport();

            DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            DataTable dtReportHeader = new DataTable();
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            HeaderRow[0] = CompanyName.Rows[0][0].ToString() + "  [NSE-CM]";
            dtReportHeader.Rows.Add(HeaderRow);
            DataRow DrRowR1 = dtReportHeader.NewRow();
            DrRowR1[0] = " Demat Reports Transaction ";
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
            FooterRow[0] = "* * *  End Of Report * * *   ";
            dtReportFooter.Rows.Add(FooterRow);

            if (ddlExport.SelectedItem.Value == "E")
            {
                objExcel.ExportToExcelforExcel(dtExport, "Demat Reports Transaction", "Closing Balance", dtReportHeader, dtReportFooter);

            }
            else if (ddlExport.SelectedItem.Value == "P")
            {
                objExcel.ExportToPDF(dtExport, "Demat Reports Transaction", "Closing Balance", dtReportHeader, dtReportFooter);
            }
        }
        protected void btnTransPrevious_Click(object sender, EventArgs e)
        {
            string tranProp = null;
            ViewState["startRowIndex"] = (int)ViewState["startRowIndex"] - (int)ViewState["pageSize"];
            if ((int)ViewState["startRowIndex"] < 0)
            {

                ViewState["startRowIndex"] = 0;
            }

            if ((int)ViewState["startRowIndex"] == 0)
            {
                tranProp = "a";
            }
            else
            {
                tranProp = "b";
            }
            ViewState["Next"] = null;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "JS", "TransNext('" + tranProp + "');", true);
            generateTable();
        }
        public void generateTable()
        {
            decimal RunningBalance = 0;
            decimal InQty = 0;
            decimal OutQty = 0;
            String strHtmlAllClient = String.Empty;
            string ISIN = null;
            string SettlementNumberType = null;
            string DPAccType = ViewState["DPAccType"].ToString();
            string WhereClause = ViewState["WhereClause"].ToString();
            string OpenWhereClause = ViewState["OpenWhereClause"].ToString();
            string SettNumType = null;

            if (ddlSett.SelectedItem.Value == "F")
            {
                SettNumType = txtSettlement.Text;
            }
            else
                SettNumType = "All";
            DataTable dtTransaction = new DataTable();
            dtTransaction = rep.DematTransactionReport(WhereClause, Convert.ToInt32(ViewState["startRowIndex"]) - 1, Convert.ToInt32(ViewState["startRowIndex"]) + Convert.ToInt32(ViewState["pageSize"]),
                DPAccID, OpenWhereClause, "N", SettNumType);

            if (dtTransaction.Rows.Count > 0)
            {
                strHtmlAllClient = "<table border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                strHtmlAllClient += "<tr style=\"background-color:#fff0f5;text-align:center\">";
                strHtmlAllClient += "<td colspan=\"8\" align=\"center\" style=\"font-size:11px\"><b>Transaction For :  [" + ddlDPAc.SelectedItem.Text + "]  For Date Range: [" + oconverter.ArrangeDate2(DtFrom.Value.ToString()) + " To " + oconverter.ArrangeDate2(dtTo.Value.ToString()) + "]</b></td></tr></table>";

                strHtmlAllClient += "<table border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                strHtmlAllClient += "<tr style=\"background-color: #DBEEF3;\">";

                strHtmlAllClient += "<td align=\"center\">Tr.Date</td>";
                strHtmlAllClient += "<td align=\"center\">Customer/Exchange</td>";
                strHtmlAllClient += "<td align=\"center\">DPID</td>";
                strHtmlAllClient += "<td align=\"center\">SettlementNumber</td>";
                strHtmlAllClient += "<td align=\"center\">Remarks</td>";
                strHtmlAllClient += "<td align=\"center\">QtyIN</td>";
                strHtmlAllClient += "<td align=\"center\">QtyOut</td>";
                strHtmlAllClient += "<td align=\"center\">Running Balance</td></tr>";
                for (int i = 0; i < dtTransaction.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        ISIN = dtTransaction.Rows[i]["DematTransactions_ISIN"].ToString();
                        SettlementNumberType = dtTransaction.Rows[i]["SettNumType"].ToString();
                        strHtmlAllClient += "<tr style=\" background-color:#fff0f5;text-align:center\">";
                        if (DPAccType == "[POOL]")
                            strHtmlAllClient += "<td colspan=\"8\" align=\"left\" style=\"font-size:11px\">" + dtTransaction.Rows[i]["ProductName"].ToString() + " &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[" + dtTransaction.Rows[i]["DematTransactions_ISIN"].ToString() + "]&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + "  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; [" + dtTransaction.Rows[i]["SettNumType"].ToString() + "]" + "</td></tr>";
                        else
                            strHtmlAllClient += "<td colspan=\"8\" align=\"left\" style=\"font-size:11px\">" + dtTransaction.Rows[i]["ProductName"].ToString() + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[" + dtTransaction.Rows[i]["DematTransactions_ISIN"].ToString() + "]" + "</td></tr>";

                        strHtmlAllClient += "<tr style=\" background-color:#fff0f5;text-align:center\">";
                        strHtmlAllClient += "<td align=\"left\" style=\"font-size:11px\">" + oconverter.ArrangeDate2(DtFrom.Value.ToString()) + "</td>";
                        strHtmlAllClient += "<td colspan=\"4\" align=\"center\" style=\"font-size:11px\">Opening Balance</td>";
                        if (dtTransaction.Rows[i]["OpeningBalance"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td colspan=\"4\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtTransaction.Rows[i]["OpeningBalance"].ToString())) + "</td></tr>";
                            RunningBalance = Convert.ToDecimal(dtTransaction.Rows[i]["OpeningBalance"].ToString());
                        }
                        else
                            strHtmlAllClient += "<td colspan=\"4\" align=\"right\" style=\"font-size:xx-small\">&nbsp;</td></tr>";


                    }
                    if (DPAccType == "[POOL]")
                    {
                        if (ISIN != dtTransaction.Rows[i]["DematTransactions_ISIN"].ToString() || SettlementNumberType != dtTransaction.Rows[i]["SettNumType"].ToString())
                        {
                            strHtmlAllClient += "<tr style=\" background-color:#fff0f5;text-align:center\">";
                            strHtmlAllClient += "<td align=\"left\" style=\"font-size:11px\">" + oconverter.ArrangeDate2(dtTo.Value.ToString()) + "</td>";
                            strHtmlAllClient += "<td colspan=\"4\" align=\"center\" style=\"font-size:11px\">Closing Balance</td>";
                            strHtmlAllClient += "<td colspan=\"4\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(RunningBalance) + "</td></tr>";

                            strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\"><td colspan=\"8\">&nbsp;</td></tr>";

                            RunningBalance = 0;
                            ISIN = dtTransaction.Rows[i]["DematTransactions_ISIN"].ToString();
                            SettlementNumberType = dtTransaction.Rows[i]["SettNumType"].ToString();
                            ViewState["ISIN"] = ISIN;
                            ViewState["SettlementNumberType"] = SettlementNumberType;
                            strHtmlAllClient += "<tr style=\" background-color:#fff0f5;text-align:center\">";
                            strHtmlAllClient += "<td colspan=\"8\" align=\"left\" style=\"font-size:11px\">" + dtTransaction.Rows[i]["ProductName"].ToString() + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[" + dtTransaction.Rows[i]["DematTransactions_ISIN"].ToString() + "]" + "  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; [" + dtTransaction.Rows[i]["SettNumType"].ToString() + "]" + "</td></tr>";

                            strHtmlAllClient += "<tr style=\" background-color:#fff0f5;text-align:center\">";
                            strHtmlAllClient += "<td align=\"left\" style=\"font-size:11px\">" + oconverter.ArrangeDate2(DtFrom.Value.ToString()) + "</td>";
                            strHtmlAllClient += "<td colspan=\"4\" align=\"center\" style=\"font-size:11px\">Opening Balance</td>";
                            if (dtTransaction.Rows[i]["OpeningBalance"] != DBNull.Value)
                            {
                                strHtmlAllClient += "<td colspan=\"4\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtTransaction.Rows[i]["OpeningBalance"].ToString())) + "</td></tr>";
                                RunningBalance = Convert.ToDecimal(dtTransaction.Rows[i]["OpeningBalance"].ToString());
                            }
                            else
                                strHtmlAllClient += "<td colspan=\"4\" align=\"right\" style=\"font-size:xx-small\">&nbsp;</td></tr>";


                        }
                    }
                    else
                    {
                        if (ISIN != dtTransaction.Rows[i]["DematTransactions_ISIN"].ToString())
                        {
                            strHtmlAllClient += "<tr style=\" background-color:#fff0f5;text-align:center\">";
                            strHtmlAllClient += "<td align=\"left\" style=\"font-size:11px\">" + oconverter.ArrangeDate2(dtTo.Value.ToString()) + "</td>";
                            strHtmlAllClient += "<td colspan=\"4\" align=\"center\" style=\"font-size:11px\">Closing Balance</td>";
                            strHtmlAllClient += "<td colspan=\"4\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(RunningBalance) + "</td></tr>";

                            strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\"><td colspan=\"8\">&nbsp;</td></tr>";

                            RunningBalance = 0;

                            ISIN = dtTransaction.Rows[i]["DematTransactions_ISIN"].ToString();
                            SettlementNumberType = dtTransaction.Rows[i]["SettNumType"].ToString();
                            ViewState["ISIN"] = ISIN;
                            ViewState["SettlementNumberType"] = SettlementNumberType;
                            strHtmlAllClient += "<tr style=\" background-color:#fff0f5;text-align:center\">";
                            strHtmlAllClient += "<td colspan=\"8\" align=\"left\" style=\"font-size:11px\">" + dtTransaction.Rows[i]["ProductName"].ToString() + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[" + dtTransaction.Rows[i]["DematTransactions_ISIN"].ToString() + "]" + "</td></tr>";

                            strHtmlAllClient += "<tr style=\" background-color:#fff0f5;text-align:center\">";
                            strHtmlAllClient += "<td align=\"left\" style=\"font-size:11px\">" + oconverter.ArrangeDate2(DtFrom.Value.ToString()) + "</td>";
                            strHtmlAllClient += "<td colspan=\"4\" align=\"center\" style=\"font-size:11px\">Opening Balance</td>";
                            if (dtTransaction.Rows[i]["OpeningBalance"] != DBNull.Value)
                            {
                                strHtmlAllClient += "<td colspan=\"4\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtTransaction.Rows[i]["OpeningBalance"].ToString())) + "</td></tr>";
                                RunningBalance = Convert.ToDecimal(dtTransaction.Rows[i]["OpeningBalance"].ToString());
                            }
                            else
                                strHtmlAllClient += "<td colspan=\"4\" align=\"right\" style=\"font-size:xx-small\">&nbsp;</td></tr>";


                        }
                    }

                    strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                    strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\">" + dtTransaction.Rows[i]["TrDate"].ToString() + "</td>";
                    strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\">" + dtTransaction.Rows[i]["CustName"].ToString() + "</td>";
                    strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\">" + dtTransaction.Rows[i]["DPNAME"].ToString() + "</td>";
                    strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\">" + dtTransaction.Rows[i]["SettNumTypeShow"].ToString() + "</td>";
                    strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\">" + dtTransaction.Rows[i]["Remarks"].ToString() + "</td>";
                    if (dtTransaction.Rows[i]["InQty"] != DBNull.Value)
                    {
                        strHtmlAllClient += "<td align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtTransaction.Rows[i]["InQty"].ToString())) + "</td>";
                        InQty = Convert.ToDecimal(dtTransaction.Rows[i]["InQty"].ToString());
                    }
                    else
                    {
                        strHtmlAllClient += "<td>&nbsp;</td>";
                        InQty = 0;
                    }
                    if (dtTransaction.Rows[i]["OutQty"] != DBNull.Value)
                    {
                        strHtmlAllClient += "<td align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtTransaction.Rows[i]["OutQty"].ToString())) + "</td>";
                        OutQty = Convert.ToDecimal(dtTransaction.Rows[i]["OutQty"].ToString());
                    }
                    else
                    {
                        strHtmlAllClient += "<td>&nbsp;</td>";
                        OutQty = 0;
                    }

                    RunningBalance += InQty - OutQty;
                    if (i == 0)
                    {
                        if (ViewState["Next"] != null)
                        {
                            if (ViewState["ISIN"].ToString() == ISIN && ViewState["SettlementNumberType"].ToString() == SettlementNumberType)
                            {
                                RunningBalance = Convert.ToInt32(ViewState["RunningBalance"]) + RunningBalance;
                            }
                        }
                    }
                    ViewState["RunningBalance"] = RunningBalance;
                    strHtmlAllClient += "<td align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(RunningBalance) + "</td></tr>";

                    if (i == dtTransaction.Rows.Count - 1)
                    {
                        strHtmlAllClient += "<tr style=\" background-color:#fff0f5;text-align:center\">";
                        strHtmlAllClient += "<td align=\"left\" style=\"font-size:11px\">" + oconverter.ArrangeDate2(dtTo.Value.ToString()) + "</td>";
                        strHtmlAllClient += "<td colspan=\"4\" align=\"center\" style=\"font-size:11px\">Closing Balance</td>";
                        strHtmlAllClient += "<td colspan=\"4\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(RunningBalance) + "</td></tr>";
                    }
                    ViewState["DtTable"] = dtTransaction;
                }
                strHtmlAllClient += "</table>";
            }
            if (dtTransaction.Rows.Count > 0)
            {
                divShow.InnerHtml = strHtmlAllClient;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct2", "HeightCall();", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct1", "alert('No Record Found !!')", true);
                divShow.InnerHtml = strHtmlAllClient;
            }
        }
        public void BindForExport()
        {
            decimal RunningBalance = 0;
            decimal InQty = 0;
            decimal OutQty = 0;
            string ISIN = null;
            string SettlementNumberType = null;
            string DPAccType = ViewState["DPAccType"].ToString();
            string WhereClause = ViewState["WhereClause"].ToString();
            string OpenWhereClause = ViewState["OpenWhereClause"].ToString();

            string SettNumType = null;

            if (ddlSett.SelectedItem.Value == "F")
            {
                SettNumType = txtSettlement.Text;
            }
            else
                SettNumType = "All";

            DataTable dtTransaction = new DataTable();
            dtTransaction = rep.DematTransactionReport(WhereClause, Convert.ToInt32(ViewState["startRowIndex"]) - 1, Convert.ToInt32(ViewState["startRowIndex"]) + Convert.ToInt32(ViewState["pageSize"]),
          DPAccID, OpenWhereClause, "Y", SettNumType);
            if (dtTransaction.Rows.Count > 0)
            {
                DataTable dtReport = new DataTable();
                dtReport.Columns.Add(new DataColumn("Tr.Date", typeof(String)));
                dtReport.Columns.Add(new DataColumn("Customer/Exchange", typeof(String)));
                dtReport.Columns.Add(new DataColumn("DPID", typeof(String)));
                dtReport.Columns.Add(new DataColumn("SettlementNumber", typeof(String)));
                dtReport.Columns.Add(new DataColumn("Remarks", typeof(String)));
                dtReport.Columns.Add(new DataColumn("QtyIN", typeof(String)));
                dtReport.Columns.Add(new DataColumn("QtyOut", typeof(String)));
                dtReport.Columns.Add(new DataColumn("Running Balance", typeof(String)));
                DataRow dr = dtReport.NewRow();
                dr[0] = " Transaction For :  [" + ddlDPAc.SelectedItem.Text + "]  For Date Range: [" + oconverter.ArrangeDate2(DtFrom.Value.ToString()) + " To " + oconverter.ArrangeDate2(dtTo.Value.ToString()) + "]";
                dr[1] = "Test";
                dtReport.Rows.Add(dr);
                for (int i = 0; i < dtTransaction.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        DataRow dr2 = dtReport.NewRow();
                        if (DPAccType == "[POOL]")
                            dr2[0] = dtTransaction.Rows[i]["ProductName"].ToString() + "[" + dtTransaction.Rows[i]["DematTransactions_ISIN"].ToString() + "]" + "   [" + dtTransaction.Rows[i]["SettNumType"].ToString() + "]";
                        else
                            dr2[0] = dtTransaction.Rows[i]["ProductName"].ToString() + "[" + dtTransaction.Rows[i]["DematTransactions_ISIN"].ToString() + "]";
                        dr2[1] = "Test";
                        dtReport.Rows.Add(dr2);

                        ISIN = dtTransaction.Rows[i]["DematTransactions_ISIN"].ToString();
                        SettlementNumberType = dtTransaction.Rows[i]["SettNumType"].ToString();
                        DataRow dr4 = dtReport.NewRow();
                        dr4[0] = oconverter.ArrangeDate2(DtFrom.Value.ToString());
                        dr4[1] = "";
                        dr4[2] = "Opening Balance";
                        dr4[3] = "";
                        dr4[4] = "";
                        dr4[5] = "";
                        dr4[6] = "";
                        if (dtTransaction.Rows[i]["OpeningBalance"] != DBNull.Value)
                            dr4[7] = oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtTransaction.Rows[i]["OpeningBalance"].ToString()));
                        else
                            dr4[7] = "";
                        dtReport.Rows.Add(dr4);
                    }
                    if (DPAccType == "[POOL]")
                    {
                        if (ISIN != dtTransaction.Rows[i]["DematTransactions_ISIN"].ToString() || SettlementNumberType != dtTransaction.Rows[i]["SettNumType"].ToString())
                        {
                            DataRow dr4 = dtReport.NewRow();
                            dr4[0] = oconverter.ArrangeDate2(dtTo.Value.ToString());
                            dr4[1] = "";
                            dr4[2] = "Closing Balance";
                            dr4[3] = "";
                            dr4[4] = "";
                            dr4[5] = "";
                            dr4[6] = "";
                            dr4[7] = oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(RunningBalance);
                            dtReport.Rows.Add(dr4);
                            RunningBalance = 0;
                            DataRow dr2 = dtReport.NewRow();
                            dr2[0] = dtTransaction.Rows[i]["ProductName"].ToString() + "[" + dtTransaction.Rows[i]["DematTransactions_ISIN"].ToString() + "]" + "   [" + dtTransaction.Rows[i]["SettNumType"].ToString() + "]";
                            dr2[1] = "Test";
                            dtReport.Rows.Add(dr2);

                            ISIN = dtTransaction.Rows[i]["DematTransactions_ISIN"].ToString();
                            SettlementNumberType = dtTransaction.Rows[i]["SettNumType"].ToString();

                            DataRow dr5 = dtReport.NewRow();
                            dr5[0] = oconverter.ArrangeDate2(DtFrom.Value.ToString());
                            dr5[1] = "";
                            dr5[2] = "Opening Balance";
                            dr5[3] = "";
                            dr5[4] = "";
                            dr5[5] = "";
                            dr5[6] = "";
                            if (dtTransaction.Rows[i]["OpeningBalance"] != DBNull.Value)
                                dr5[7] = oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtTransaction.Rows[i]["OpeningBalance"].ToString()));
                            else
                                dr5[7] = "";
                            dtReport.Rows.Add(dr5);
                        }
                    }
                    else
                    {
                        if (ISIN != dtTransaction.Rows[i]["DematTransactions_ISIN"].ToString())
                        {
                            DataRow dr4 = dtReport.NewRow();
                            dr4[0] = oconverter.ArrangeDate2(dtTo.Value.ToString());
                            dr4[1] = "";
                            dr4[2] = "Closing Balance";
                            dr4[3] = "";
                            dr4[4] = "";
                            dr4[5] = "";
                            dr4[6] = "";
                            dr4[7] = oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(RunningBalance);
                            dtReport.Rows.Add(dr4);
                            RunningBalance = 0;
                            DataRow dr2 = dtReport.NewRow();
                            dr2[0] = dtTransaction.Rows[i]["ProductName"].ToString() + "[" + dtTransaction.Rows[i]["DematTransactions_ISIN"].ToString() + "]";
                            dr2[1] = "Test";
                            dtReport.Rows.Add(dr2);

                            ISIN = dtTransaction.Rows[i]["DematTransactions_ISIN"].ToString();
                            SettlementNumberType = dtTransaction.Rows[i]["SettNumType"].ToString();
                            DataRow dr5 = dtReport.NewRow();
                            dr5[0] = oconverter.ArrangeDate2(DtFrom.Value.ToString());
                            dr5[1] = "";
                            dr5[2] = "Opening Balance";
                            dr5[3] = "";
                            dr5[4] = "";
                            dr5[5] = "";
                            dr5[6] = "";
                            if (dtTransaction.Rows[i]["OpeningBalance"] != DBNull.Value)
                                dr5[7] = oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtTransaction.Rows[i]["OpeningBalance"].ToString()));
                            else
                                dr5[7] = "";
                            dtReport.Rows.Add(dr5);
                        }
                    }
                    DataRow dr1 = dtReport.NewRow();
                    dr1[0] = dtTransaction.Rows[i]["TrDate"].ToString();
                    dr1[1] = dtTransaction.Rows[i]["CustName"].ToString();
                    dr1[2] = dtTransaction.Rows[i]["DPNAME"].ToString();
                    dr1[3] = dtTransaction.Rows[i]["SettNumTypeShow"].ToString();
                    dr1[4] = dtTransaction.Rows[i]["Remarks"].ToString();
                    if (dtTransaction.Rows[i]["InQty"] != DBNull.Value)
                    {
                        InQty = Convert.ToDecimal(dtTransaction.Rows[i]["InQty"].ToString());
                        dr1[5] = oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(InQty);
                    }
                    else
                    {
                        InQty = 0;
                        dr1[5] = "";
                    }
                    if (dtTransaction.Rows[i]["OutQty"] != DBNull.Value)
                    {
                        OutQty = Convert.ToDecimal(dtTransaction.Rows[i]["OutQty"].ToString());
                        dr1[6] = oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(OutQty);
                    }
                    else
                    {
                        OutQty = 0;
                        dr1[6] = "";
                    }

                    RunningBalance += InQty - OutQty;
                    if (RunningBalance != 0)
                        dr1[7] = oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(RunningBalance);
                    else
                        dr1[7] = "";
                    dtReport.Rows.Add(dr1);
                }
                dtExport.Dispose();
                dtExport.Rows.Clear();
                dtExport = dtReport.Copy();
            }
        }
        protected void btnTransnNext_Click1(object sender, EventArgs e)
        {
            string tranProp = null;
            ViewState["startRowIndex"] = ((int)ViewState["startRowIndex"] + (int)ViewState["pageSize"]);


            //if ((int)ViewState["startRowIndex"] >= (int)ViewState["totalRecord"])
            if (((int)ViewState["totalRecord"] - (int)ViewState["startRowIndex"]) <= (int)ViewState["pageSize"])
            {
                tranProp = "a";
            }
            else
            {
                tranProp = "b";
            }
            ViewState["Next"] = "Next";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "JS", "NextTrans('" + tranProp + "');", true);
            generateTable();
        }
    }
}