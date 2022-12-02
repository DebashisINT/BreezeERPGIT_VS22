using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;

namespace ERP.OMS.Reports
{
    public partial class Reports_frm_ReportManualBRS : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        ExcelFile objExcel = new ExcelFile();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        DataSet ds = new DataSet();
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.Reports oReports = new BusinessLogicLayer.Reports();
        string data;

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
                SetDatteFinYear();


                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");
                DtAsOn.EditFormatString = oconverter.GetDateFormat("Date");
                //  DtAsOn.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
            }

            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //___________-end here___//
        }
        protected void SetDatteFinYear()
        {
            DataTable dtFinYear = oDBEngine.GetDataTable("MASTER_FINYEAR ", " FINYEAR_ENDDATE ", " FINYEAR_CODE ='" + Session["LastFinYear"].ToString() + "'");
            DateTime EndDate = Convert.ToDateTime(dtFinYear.Rows[0][0].ToString());
            DateTime TodayDate = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
            if (EndDate < TodayDate)
                DtAsOn.Value = EndDate;
            else
                DtAsOn.Value = TodayDate;

        }
        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }
        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            string id = eventArgument.ToString();
            string[] cl = id.Split(',');
            string str = "";
            string str1 = "";
            for (int i = 0; i < cl.Length; i++)
            {
                string[] val = cl[i].Split(';');

                if (str == "")
                {
                    str = "'" + val[0] + "'";
                    str1 = "'" + val[0] + "'" + ";" + val[1];
                }
                else
                {
                    str += ",'" + val[0] + "'";
                    str1 += "," + "'" + val[0] + "'" + ";" + val[1];
                }

            }

            data = str;


        }
        protected void btnScreen_Click(object sender, EventArgs e)
        {
            if (DdlReportView.SelectedItem.Value.ToString().Trim() == "1")
            {
                if (txtBankName_hidden.Text.ToString().Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "fnNoRecord", "fnNoRecord('3');", true);
                }
                else
                {
                    procedure();
                    ds = (DataSet)ViewState["dataset"];
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        FnHtml(ds);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordDisplay", "RecordDisplay();", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "fnNoRecord", "fnNoRecord('1');", true);
                    }
                }

            }
            else
            {
                procedure();
                ds = (DataSet)ViewState["dataset"];
                if (ds.Tables[0].Rows.Count > 0)
                {
                    FnHtml(ds);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordDisplay", "RecordDisplay();", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "fnNoRecord", "fnNoRecord('1');", true);
                }
            }

        }
        void procedure()
        {
            string BanckAc = string.Empty;

            if (DdlReportView.SelectedItem.Value.ToString().Trim() == "2")
            {
                if (rdbBankAll.Checked)
                {
                    BanckAc = "ALL";
                }
                else
                {
                    BanckAc = Convert.ToString(HiddenField_BRSAC.Value);
                }

            }
            else
            {
                BanckAc = Convert.ToString(txtBankName_hidden);
            }
            ds = oReports.Report_BRS(
                Convert.ToString(DdlReportView.SelectedItem.Value),
                  Convert.ToString(Session["LastCompany"]),
                  BanckAc,
                  Convert.ToString(DdlConsider.SelectedItem.Value),
                  Convert.ToString(DtAsOn.Value),
                  Convert.ToString(Session["LastFinYear"])
                );
            ViewState["dataset"] = ds;


        }
        void FnHtml(DataSet ds)
        {
            if (DdlReportView.SelectedItem.Value.ToString().Trim() == "1")///Detail
            {
                FnHtml_Detail(ds);
            }
            if (DdlReportView.SelectedItem.Value.ToString().Trim() == "2")///Summary
            {
                FnHtml_Summary(ds);
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordDisplay", "RecordDisplay();", true);
        }
        void FnHtml_Detail(DataSet ds)
        {
            String strHtmlheader = String.Empty;
            string str = null;
            str = "[Detail View] For :" + txtBankName.Text.ToString().Trim() + " Consider:" + DdlConsider.SelectedItem.Text.ToString();
            str = str + " ; As On " + oconverter.ArrangeDate2(DtAsOn.Value.ToString());

            strHtmlheader = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtmlheader += "<tr><td align=\"left\" colspan=" + ds.Tables[0].Columns.Count + " style=\"color:Blue;\">" + str + "</td></tr></table>";


            ////////Detail Display
            String strHtml = String.Empty;
            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtml += "<tr style=\"background-color: White\"><td style=\"color:#153E7E;\" align=\"right\"><b>Bank Balance As Per Bank Book :</b></td>";
            strHtml += "<td>";
            strHtml += "<table width=\"100%\" border=" + Convert.ToChar(34) + 0 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

            //////////Bank Baln.
            strHtml += "<tr style=\"background-color: #E3E4FA;\"  text-align:left\">";
            strHtml += "<td align=\"center\" nowrap=nowrap; ><b>Amount(Dr.)</b></td>";
            strHtml += "<td align=\"center\" nowrap=nowrap; ><b>Amount(Cr.)</b></td>";
            strHtml += "</tr>";

            strHtml += "<tr style=\"background-color: White\">";
            decimal amnt = decimal.Zero;
            amnt = Convert.ToDecimal(ds.Tables[0].Rows[0]["Amount"].ToString().Trim());
            if (amnt > 0)
            {
                strHtml += "<td align=\"left\" nowrap=nowrap; style=\"font-size:xx-small;color:maroon;\"><b>" + ds.Tables[0].Rows[0]["Result"].ToString().Trim() + "</b></td>";
                strHtml += "<td>&nbsp;</td></tr>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
                strHtml += "<td align=\"right\" nowrap=nowrap; style=\"font-size:xx-small;color:maroon;\"><b>" + ds.Tables[0].Rows[0]["Result"].ToString().Trim() + "<b></td></tr>";
            }
            strHtml += "</tr></table></td></tr>";

            //////LESS: Cheques Deposited But Not Cleared:

            DataView viewrecord = new DataView();
            viewrecord = ds.Tables[0].DefaultView;
            viewrecord.RowFilter = "StatusOrder=1";
            DataTable dt = new DataTable();
            dt = viewrecord.ToTable();
            int flag = 0;
            if (dt.Rows.Count > 0)
            {
                strHtml += "<tr><td>";
                strHtml += "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                strHtml += "<tr style=\"background-color: White\"><td colspan=6 style=\"color:#153E7E;\"><b>LESS: Cheques Deposited But Not Cleared</b></td></tr>";
                strHtml += "<tr style=\"background-color: #E3E4FA;\">";
                strHtml += "<td align=\"center\" nowrap=nowrap; ><b>Transaction Date</b></td>";
                strHtml += "<td align=\"center\" nowrap=nowrap; ><b>Voucher No</b></td>";
                strHtml += "<td align=\"center\" nowrap=nowrap; ><b>Account Name</b></td>";
                strHtml += "<td align=\"center\" nowrap=nowrap;><b>Inst No.</b></td>";
                strHtml += "<td align=\"center\" nowrap=nowrap;><b>Inst Date</b></td>";
                strHtml += "<td align=\"center\" nowrap=nowrap;><b>Amount</b></td>";
                strHtml += "</tr>";




                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";

                    if (dt.Rows[i]["TranDate"] != DBNull.Value)
                        strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt.Rows[i]["TranDate"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">&nbsp;</td>";
                    if (dt.Rows[i]["VoucherNo"] != DBNull.Value)
                        strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt.Rows[i]["VoucherNo"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">&nbsp;</td>";
                    if (dt.Rows[i]["AccountName"] != DBNull.Value)
                        strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt.Rows[i]["AccountName"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">&nbsp;</td>";
                    if (dt.Rows[i]["InstNo"] != DBNull.Value)
                        strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt.Rows[i]["InstNo"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">&nbsp;</td>";
                    if (dt.Rows[i]["InstDate"] != DBNull.Value)
                        strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt.Rows[i]["InstDate"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">&nbsp;</td>";
                    if (dt.Rows[i]["Result"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt.Rows[i]["Result"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">&nbsp;</td>";
                    strHtml += "<tr>";
                }
                strHtml += "</table></td>";
                strHtml += "<td>&nbsp;</td></tr>";
            }

            /////Total Display
            DataView viewrecord1 = new DataView();
            viewrecord1 = ds.Tables[0].DefaultView;
            viewrecord1.RowFilter = "StatusOrder=2";
            DataTable dt1 = new DataTable();
            dt1 = viewrecord1.ToTable();

            if (dt1.Rows.Count > 0)
            {
                strHtml += "<tr><td>";
                strHtml += "<table width=\"100%\" border=" + Convert.ToChar(34) + 0 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                strHtml += "<tr style=\"background-color: White\">";
                strHtml += "<td align=\"left\" nowrap=nowrap; colspan=5 style=\"font-size:xx-small;\"><b>Total :</b>";
                if (dt1.Rows[0]["Result"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>" + dt1.Rows[0]["Result"].ToString() + "</b></td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                strHtml += "</td></tr></table></td>";
                strHtml += "<td>";
                strHtml += "<table width=\"100%\" border=" + Convert.ToChar(34) + 0 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                strHtml += "<tr style=\"background-color: White\">";
                strHtml += "<td align=\"center\" nowrap=nowrap;><b>&nbsp;</b>";
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;color:maroon;\" nowrap=\"nowrap;\" ><b>" + dt1.Rows[0]["Result"].ToString() + "</b></td>";
                strHtml += "</td></tr></table></td>";
                strHtml += "<tr>";


            }
            //////ADD: Cheques Issued But Not Cleared
            DataView viewrecord2 = new DataView();
            viewrecord2 = ds.Tables[0].DefaultView;
            viewrecord2.RowFilter = "StatusOrder=3";
            DataTable dt2 = new DataTable();
            dt2 = viewrecord2.ToTable();

            if (dt2.Rows.Count > 0)
            {
                strHtml += "<tr><td>";
                strHtml += "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                strHtml += "<tr style=\"background-color: White\"><td colspan=6 style=\"color:#153E7E;\"><b>ADD: Cheques Issued But Not Cleared</b></td></tr>";
                strHtml += "<tr style=\"background-color: #E3E4FA;\">";
                strHtml += "<td align=\"center\" nowrap=nowrap; ><b>Transaction Date</b></td>";
                strHtml += "<td align=\"center\" nowrap=nowrap; ><b>Voucher No</b></td>";
                strHtml += "<td align=\"center\" nowrap=nowrap; ><b>Account Name</b></td>";
                strHtml += "<td align=\"center\" nowrap=nowrap;><b>Inst No.</b></td>";
                strHtml += "<td align=\"center\" nowrap=nowrap;><b>Inst Date</b></td>";
                strHtml += "<td align=\"center\" nowrap=nowrap;><b>Amount</b></td>";
                strHtml += "</tr>";


                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";

                    if (dt2.Rows[i]["TranDate"] != DBNull.Value)
                        strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt2.Rows[i]["TranDate"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">&nbsp;</td>";
                    if (dt2.Rows[i]["VoucherNo"] != DBNull.Value)
                        strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt2.Rows[i]["VoucherNo"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">&nbsp;</td>";
                    if (dt2.Rows[i]["AccountName"] != DBNull.Value)
                        strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt2.Rows[i]["AccountName"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">&nbsp;</td>";
                    if (dt2.Rows[i]["InstNo"] != DBNull.Value)
                        strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt2.Rows[i]["InstNo"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">&nbsp;</td>";
                    if (dt2.Rows[i]["InstDate"] != DBNull.Value)
                        strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt2.Rows[i]["InstDate"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">&nbsp;</td>";
                    if (dt2.Rows[i]["Result"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt2.Rows[i]["Result"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">&nbsp;</td>";
                    strHtml += "<tr>";
                }
                strHtml += "</table></td>";
                strHtml += "<td>&nbsp;</td></tr>";

            }
            /////Total Display
            DataView viewrecord3 = new DataView();
            viewrecord3 = ds.Tables[0].DefaultView;
            viewrecord3.RowFilter = "StatusOrder=4";
            DataTable dt3 = new DataTable();
            dt3 = viewrecord1.ToTable();

            if (dt3.Rows.Count > 0)
            {
                strHtml += "<tr><td>";
                strHtml += "<table width=\"100%\" border=" + Convert.ToChar(34) + 0 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                strHtml += "<tr style=\"background-color: White\">";
                strHtml += "<td align=\"left\" nowrap=nowrap; colspan=5 style=\"font-size:xx-small;\"><b>Total :</b>";
                if (dt3.Rows[0]["Result"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>" + dt3.Rows[0]["Result"].ToString() + "</b></td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                strHtml += "</td></tr></table></td>";
                strHtml += "<td>";
                strHtml += "<table width=\"100%\" border=" + Convert.ToChar(34) + 0 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                strHtml += "<tr style=\"background-color: White\">";
                strHtml += "<td align=\"left\" style=\"font-size:xx-small;color:maroon;\" nowrap=\"nowrap;\" ><b>" + dt3.Rows[0]["Result"].ToString() + "</b></td>";
                strHtml += "<td align=\"left\" style=\"font-size:xx-small;color:maroon;\" nowrap=\"nowrap;\">&nbsp;</td>";
                strHtml += "</td></tr></table></td>";
                strHtml += "<tr>";

            }
            //////Last Cal
            DataView viewrecord4 = new DataView();
            viewrecord4 = ds.Tables[0].DefaultView;
            viewrecord4.RowFilter = "StatusOrder=5";
            DataTable dt4 = new DataTable();
            dt4 = viewrecord1.ToTable();

            if (dt4.Rows.Count > 0)
            {
                strHtml += "<tr><td>";
                strHtml += "<table width=\"100%\" border=" + Convert.ToChar(34) + 0 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                strHtml += "<tr style=\"background-color: White\">";
                strHtml += "<td align=\"right\" nowrap=nowrap; colspan=6 style=\"font-size:xx-small;color:#153E7E;\"><b>Balance As Per Bank Statement  :</b>";
                strHtml += "</td></tr></table></td>";

                amnt = decimal.Zero;
                amnt = Convert.ToDecimal(dt4.Rows[0]["Amount"].ToString().Trim());

                strHtml += "<td>";
                strHtml += "<table width=\"100%\" border=" + Convert.ToChar(34) + 0 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                strHtml += "<tr style=\"background-color: White\">";
                if (amnt > 0)
                {
                    strHtml += "<td align=\"left\" nowrap=nowrap; style=\"font-size:xx-small;color:maroon;\"><b>" + dt4.Rows[0]["Result"].ToString().Trim() + "</b></td>";
                    strHtml += "<td>&nbsp;</td></tr>";
                }
                else
                {
                    strHtml += "<td>&nbsp;</td>";
                    strHtml += "<td align=\"right\" nowrap=nowrap; style=\"font-size:xx-small;color:maroon;\"><b>" + dt4.Rows[0]["Result"].ToString().Trim() + "<b></td></tr>";
                }
                strHtml += "</tr></table></td>";
                strHtml += "</tr>";

            }
            strHtml += "</table>";
            DivHeader.InnerHtml = strHtmlheader;
            Divdisplay.InnerHtml = strHtml;
        }
        void FnHtml_Summary(DataSet ds)
        {
            String strHtmlheader = String.Empty;
            string str = null;
            str = " [Summary View] ; As On " + oconverter.ArrangeDate2(DtAsOn.Value.ToString());

            strHtmlheader = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtmlheader += "<tr><td align=\"left\" colspan=" + ds.Tables[0].Columns.Count + " style=\"color:Blue;\">" + str + "</td></tr></table>";


            ////////Detail Display
            String strHtml = String.Empty;
            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtml += "<tr style=\"background-color: #E3E4FA;\">";
            strHtml += "<td align=\"center\" nowrap=nowrap; ><b>Account Name</b></td>";
            strHtml += "<td align=\"center\" nowrap=nowrap; ><b>Code</b></td>";
            strHtml += "<td align=\"center\" nowrap=nowrap; ><b>Account Number</b></td>";
            strHtml += "<td align=\"center\" nowrap=nowrap;><b>Book Balance</b></td>";
            strHtml += "<td align=\"center\" nowrap=nowrap;><b>UnClreared Payments</b></td>";
            strHtml += "<td align=\"center\" nowrap=nowrap;><b>UnClreared Receipts</b></td>";
            strHtml += "<td align=\"center\" nowrap=nowrap;><b>Statement Balance</b></td>";
            strHtml += "</tr>";

            int flag = 0;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";

                if (ds.Tables[0].Rows[i]["AccountName"] != DBNull.Value)
                {
                    if (ds.Tables[0].Rows[i]["AccountName"].ToString().Trim() == "Grand Total")
                    {
                        strHtml += "<td align=\"left\" style=\"font-size:xx-small;color:#153E7E;\" nowrap=\"nowrap;\"><b>Grand Total :</b></td>";
                    }
                    else
                    {
                        strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + ds.Tables[0].Rows[i]["AccountName"].ToString() + "</td>";
                    }
                }
                else
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">&nbsp;</td>";

                if (ds.Tables[0].Rows[i]["Code"] != DBNull.Value)
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + ds.Tables[0].Rows[i]["Code"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">&nbsp;</td>";
                if (ds.Tables[0].Rows[i]["AccountNumber"] != DBNull.Value)
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + ds.Tables[0].Rows[i]["AccountNumber"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">&nbsp;</td>";
                if (ds.Tables[0].Rows[i]["BookBalance"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + ds.Tables[0].Rows[i]["BookBalance"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">&nbsp;</td>";
                if (ds.Tables[0].Rows[i]["UnclearedPayments"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + ds.Tables[0].Rows[i]["UnclearedPayments"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">&nbsp;</td>";
                if (ds.Tables[0].Rows[i]["UnclearedReceipts"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + ds.Tables[0].Rows[i]["UnclearedReceipts"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">&nbsp;</td>";
                if (ds.Tables[0].Rows[i]["StatementBalance"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + ds.Tables[0].Rows[i]["StatementBalance"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">&nbsp;</td>";
                strHtml += "<tr>";
            }

            strHtml += "</table>";
            DivHeader.InnerHtml = strHtmlheader;
            Divdisplay.InnerHtml = strHtml;
        }
        protected string GetRowColor(int i)
        {
            if (i++ % 2 == 0)
                return "#fff0f5";
            else
                return "White";
        }
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            if (DdlReportView.SelectedItem.Value.ToString().Trim() == "1")
            {
                if (txtBankName_hidden.Text.ToString().Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "fnNoRecord", "fnNoRecord('3');", true);
                }
                else
                {
                    procedure();
                    ds = (DataSet)ViewState["dataset"];
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Export(ds);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "fnNoRecord", "fnNoRecord('1');", true);
                    }
                }

            }
            else
            {
                procedure();
                ds = (DataSet)ViewState["dataset"];
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Export(ds);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "fnNoRecord", "fnNoRecord('1');", true);
                }
            }
        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            ds = (DataSet)ViewState["dataset"];
            Export(ds);
        }
        void Export(DataSet ds)
        {
            if (DdlReportView.SelectedItem.Value.ToString().Trim() == "1")///Detail
            {
                FnExport_Detail(ds);
            }
            if (DdlReportView.SelectedItem.Value.ToString().Trim() == "2")///Summary
            {
                FnExport_Summary(ds);
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordDisplay", "RecordDisplay();", true);
        }
        void FnExport_Summary(DataSet ds)
        {

            DataTable dtExport = new DataTable();
            dtExport.Clear();
            dtExport.Columns.Add("Account Name", Type.GetType("System.String"));
            dtExport.Columns.Add("Code", Type.GetType("System.String"));
            dtExport.Columns.Add("Account Number", Type.GetType("System.String"));
            dtExport.Columns.Add("Book Balance", Type.GetType("System.String"));
            dtExport.Columns.Add("UnClreared Payments", Type.GetType("System.String"));
            dtExport.Columns.Add("UnClreared Receipts", Type.GetType("System.String"));
            dtExport.Columns.Add("Statement Balance", Type.GetType("System.String"));

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                DataRow row = dtExport.NewRow();
                if (ds.Tables[0].Rows[i]["AccountName"] != DBNull.Value)
                    row[0] = ds.Tables[0].Rows[i]["AccountName"].ToString();
                if (ds.Tables[0].Rows[i]["Code"] != DBNull.Value)
                    row[1] = ds.Tables[0].Rows[i]["Code"].ToString();
                if (ds.Tables[0].Rows[i]["AccountNumber"] != DBNull.Value)
                    row[2] = ds.Tables[0].Rows[i]["AccountNumber"].ToString();
                if (ds.Tables[0].Rows[i]["BookBalance"] != DBNull.Value)
                    row[3] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[i]["BookBalance"].ToString()));
                if (ds.Tables[0].Rows[i]["UnclearedPayments"] != DBNull.Value)
                    row[4] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[i]["UnclearedPayments"].ToString()));
                if (ds.Tables[0].Rows[i]["UnclearedReceipts"] != DBNull.Value)
                    row[5] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[i]["UnclearedReceipts"].ToString()));
                if (ds.Tables[0].Rows[i]["StatementBalance"] != DBNull.Value)
                    row[6] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[i]["StatementBalance"].ToString()));
                dtExport.Rows.Add(row);
            }

            string str = null;
            str = " [Summary View] ; As On " + oconverter.ArrangeDate2(DtAsOn.Value.ToString());

            DataTable dtReportHeader = new DataTable();
            DataTable dtReportFooter = new DataTable();

            DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            HeaderRow[0] = CompanyName.Rows[0][0].ToString();
            dtReportHeader.Rows.Add(HeaderRow);
            DataRow DrRowR1 = dtReportHeader.NewRow();
            DrRowR1[0] = str.ToString().Trim();
            dtReportHeader.Rows.Add(DrRowR1);

            DataRow HeaderRow1 = dtReportHeader.NewRow();
            dtReportHeader.Rows.Add(HeaderRow1);
            DataRow HeaderRow2 = dtReportHeader.NewRow();
            dtReportHeader.Rows.Add(HeaderRow2);

            dtReportFooter.Columns.Add(new DataColumn("Footer", typeof(String))); //0
            DataRow FooterRow1 = dtReportFooter.NewRow();
            dtReportFooter.Rows.Add(FooterRow1);
            DataRow FooterRow2 = dtReportFooter.NewRow();
            dtReportFooter.Rows.Add(FooterRow2);
            DataRow FooterRow = dtReportFooter.NewRow();
            FooterRow[0] = "* * *  End Of Report * * *   ";
            dtReportFooter.Rows.Add(FooterRow);

            objExcel.ExportToExcelforExcel(dtExport, "Bank Reconciliation Statement", "Grand Total", dtReportHeader, dtReportFooter);
        }
        void FnExport_Detail(DataSet ds)
        {

            DataTable dtExport = new DataTable();
            dtExport.Clear();
            dtExport.Columns.Add("Transaction Date", Type.GetType("System.String"));
            dtExport.Columns.Add("Voucher No", Type.GetType("System.String"));
            dtExport.Columns.Add("Account Name", Type.GetType("System.String"));
            dtExport.Columns.Add("Inst No.", Type.GetType("System.String"));
            dtExport.Columns.Add("Inst Date", Type.GetType("System.String"));
            dtExport.Columns.Add("Amount", Type.GetType("System.String"));
            dtExport.Columns.Add("Amount(Dr.)", Type.GetType("System.String"));
            dtExport.Columns.Add("Amount(Cr.)", Type.GetType("System.String"));

            /////////Bank Balance
            DataRow row = dtExport.NewRow();
            row["Transaction Date"] = "Bank Balance As Per Bank Book :";
            decimal amnt = decimal.Zero;
            amnt = Convert.ToDecimal(ds.Tables[0].Rows[0]["Amount"].ToString().Trim());
            if (amnt > 0)
            {
                row["Amount(Dr.)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[0]["Result"].ToString()));
            }
            else
            {
                row["Amount(Cr.)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[0]["Result"].ToString()));
            }
            dtExport.Rows.Add(row);

            /////////For LESS: Cheques Deposited But Not Cleared:
            DataView viewrecord = new DataView();
            viewrecord = ds.Tables[0].DefaultView;
            viewrecord.RowFilter = "StatusOrder=1";
            DataTable dt = new DataTable();
            dt = viewrecord.ToTable();

            if (dt.Rows.Count > 0)
            {
                DataRow row1 = dtExport.NewRow();
                row1["Transaction Date"] = "LESS: Cheques Deposited But Not Cleared";
                row1["Voucher No"] = "Test";
                dtExport.Rows.Add(row1);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow row2 = dtExport.NewRow();
                    if (dt.Rows[i]["TranDate"] != DBNull.Value)
                        row2[0] = dt.Rows[i]["TranDate"].ToString();
                    if (dt.Rows[i]["VoucherNo"] != DBNull.Value)
                        row2[1] = dt.Rows[i]["VoucherNo"].ToString();
                    if (dt.Rows[i]["AccountName"] != DBNull.Value)
                        row2[2] = dt.Rows[i]["AccountName"].ToString();
                    if (dt.Rows[i]["InstNo"] != DBNull.Value)
                        row2[3] = dt.Rows[i]["InstNo"].ToString();
                    if (dt.Rows[i]["InstDate"] != DBNull.Value)
                        row2[4] = dt.Rows[i]["InstDate"].ToString();
                    if (dt.Rows[i]["Result"] != DBNull.Value)
                        row2[5] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt.Rows[i]["Result"].ToString()));
                    dtExport.Rows.Add(row2);
                }

            }
            /////Total Display
            DataView viewrecord1 = new DataView();
            viewrecord1 = ds.Tables[0].DefaultView;
            viewrecord1.RowFilter = "StatusOrder=2";
            DataTable dt1 = new DataTable();
            dt1 = viewrecord1.ToTable();

            if (dt1.Rows.Count > 0)
            {
                DataRow row3 = dtExport.NewRow();
                row3["Transaction Date"] = "Total";
                row3["Amount"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["Result"].ToString()));
                row3["Amount(Cr.)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["Result"].ToString()));
                dtExport.Rows.Add(row3);

            }
            /////////For ADD: Cheques Issued But Not Cleared
            DataView viewrecord2 = new DataView();
            viewrecord2 = ds.Tables[0].DefaultView;
            viewrecord2.RowFilter = "StatusOrder=3";
            DataTable dt2 = new DataTable();
            dt2 = viewrecord2.ToTable();

            if (dt2.Rows.Count > 0)
            {
                DataRow row4 = dtExport.NewRow();
                row4["Transaction Date"] = "ADD: Cheques Issued But Not Cleared";
                row4["Voucher No"] = "Test";
                dtExport.Rows.Add(row4);

                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    DataRow row5 = dtExport.NewRow();
                    if (dt2.Rows[i]["TranDate"] != DBNull.Value)
                        row5[0] = dt2.Rows[i]["TranDate"].ToString();
                    if (dt2.Rows[i]["VoucherNo"] != DBNull.Value)
                        row5[1] = dt2.Rows[i]["VoucherNo"].ToString();
                    if (dt2.Rows[i]["AccountName"] != DBNull.Value)
                        row5[2] = dt2.Rows[i]["AccountName"].ToString();
                    if (dt2.Rows[i]["InstNo"] != DBNull.Value)
                        row5[3] = dt2.Rows[i]["InstNo"].ToString();
                    if (dt2.Rows[i]["InstDate"] != DBNull.Value)
                        row5[4] = dt2.Rows[i]["InstDate"].ToString();
                    if (dt2.Rows[i]["Result"] != DBNull.Value)
                        row5[5] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt2.Rows[i]["Result"].ToString()));
                    dtExport.Rows.Add(row5);
                }

            }
            /////Total Display
            DataView viewrecord3 = new DataView();
            viewrecord3 = ds.Tables[0].DefaultView;
            viewrecord3.RowFilter = "StatusOrder=4";
            DataTable dt3 = new DataTable();
            dt3 = viewrecord3.ToTable();

            if (dt3.Rows.Count > 0)
            {
                DataRow row6 = dtExport.NewRow();
                row6["Transaction Date"] = "Total";
                row6["Amount"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt3.Rows[0]["Result"].ToString()));
                row6["Amount(Dr.)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt3.Rows[0]["Result"].ToString()));
                dtExport.Rows.Add(row6);

            }
            ////Blank Row Insert
            DataRow rowBlankRow = dtExport.NewRow();
            dtExport.Rows.Add(rowBlankRow);

            /////Last Cal
            DataView viewrecord4 = new DataView();
            viewrecord4 = ds.Tables[0].DefaultView;
            viewrecord4.RowFilter = "StatusOrder=5";
            DataTable dt4 = new DataTable();
            dt4 = viewrecord4.ToTable();

            DataRow row7 = dtExport.NewRow();
            row7["Transaction Date"] = "Balance As Per Bank Statement";
            amnt = decimal.Zero;
            amnt = Convert.ToDecimal(dt4.Rows[0]["Amount"].ToString().Trim());
            if (amnt > 0)
            {
                row7["Amount(Dr.)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt4.Rows[0]["Result"].ToString()));
            }
            else
            {
                row7["Amount(Cr.)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt4.Rows[0]["Result"].ToString()));
            }
            dtExport.Rows.Add(row7);


            string str = null;
            str = "[Detail View] For :" + txtBankName.Text.ToString().Trim() + " Consider:" + DdlConsider.SelectedItem.Text.ToString();
            str = str + " ; As On " + oconverter.ArrangeDate2(DtAsOn.Value.ToString());

            DataTable dtReportHeader = new DataTable();
            DataTable dtReportFooter = new DataTable();

            DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            HeaderRow[0] = CompanyName.Rows[0][0].ToString();
            dtReportHeader.Rows.Add(HeaderRow);
            DataRow DrRowR1 = dtReportHeader.NewRow();
            DrRowR1[0] = str.ToString().Trim();
            dtReportHeader.Rows.Add(DrRowR1);

            DataRow HeaderRow1 = dtReportHeader.NewRow();
            dtReportHeader.Rows.Add(HeaderRow1);
            DataRow HeaderRow2 = dtReportHeader.NewRow();
            dtReportHeader.Rows.Add(HeaderRow2);

            dtReportFooter.Columns.Add(new DataColumn("Footer", typeof(String))); //0
            DataRow FooterRow1 = dtReportFooter.NewRow();
            dtReportFooter.Rows.Add(FooterRow1);
            DataRow FooterRow2 = dtReportFooter.NewRow();
            dtReportFooter.Rows.Add(FooterRow2);
            DataRow FooterRow = dtReportFooter.NewRow();
            FooterRow[0] = "* * *  End Of Report * * *   ";
            dtReportFooter.Rows.Add(FooterRow);

            objExcel.ExportToExcelforExcel(dtExport, "Bank Reconciliation Statement", "Total", dtReportHeader, dtReportFooter);
        }
    }
}