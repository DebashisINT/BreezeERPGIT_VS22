using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using CrystalDecisions.CrystalReports.Engine;

namespace ERP.OMS.Reports
{
    public partial class Reports_frm_ReportManualBRSNew : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        BusinessLogicLayer.FAReportsOther objfaReportOther = new BusinessLogicLayer.FAReportsOther();
        ExcelFile objExcel = new ExcelFile();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        DataSet ds = new DataSet();
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        string data;
        DataTable DtParam = new DataTable();
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
        public int CurrentPage
        {

            get
            {
                if (this.ViewState["CurrentPage"] == null)
                    return 0;
                else
                    return Convert.ToInt16(this.ViewState["CurrentPage"].ToString());
            }

            set
            {
                this.ViewState["CurrentPage"] = value;
            }

        }

        public int LastPage
        {
            get
            {
                if (this.ViewState["LastPage"] == null)
                    return 0;
                else
                    return Convert.ToInt16(this.ViewState["LastPage"].ToString());
            }
            set
            {
                this.ViewState["LastPage"] = value;
            }

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
            Procedure();

        }
        void Procedure()
        {
            string CompanyId = string.Empty;
            string BanckAc = string.Empty;
            if (RdbAllCompany.Checked)
            {
                CompanyId = "ALL";
            }
            else if (RdbCurrentCompany.Checked)
            {
                CompanyId = "'" + Session["LastCompany"].ToString().Trim() + "'";
            }
            else
            {
                CompanyId = HiddenField_Company.Value;
            }

            if (rdbBankAll.Checked)
            {
                BanckAc = "ALL";
            }
            else
            {
                BanckAc = HiddenField_BRSAC.Value;
            }

            ds = objfaReportOther.Report_BRSNew(
                     Convert.ToString(DdlReportView.SelectedItem.Value),
                    Convert.ToString(CompanyId),
                    Convert.ToString(BanckAc),
                    Convert.ToString(DdlConsider.SelectedItem.Value),
                  Convert.ToString(DtAsOn.Value),
                   Convert.ToString(Session["LastFinYear"])
                     );
            ViewState["dataset"] = ds;
            FunctionCall(ds);
            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            //{

            //    SqlCommand cmd = new SqlCommand();
            //    cmd.Connection = con;
            //    cmd.CommandText = "[Report_BRSNew]";
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.Parameters.AddWithValue("@ReportView", DdlReportView.SelectedItem.Value.ToString().Trim());
            //    if (RdbAllCompany.Checked)
            //    {
            //        cmd.Parameters.AddWithValue("@CompanyId", "ALL");
            //    }
            //    else if (RdbCurrentCompany.Checked)
            //    {
            //        cmd.Parameters.AddWithValue("@CompanyId", "'" + Session["LastCompany"].ToString().Trim() + "'");
            //    }
            //    else
            //    {
            //        cmd.Parameters.AddWithValue("@CompanyId", HiddenField_Company.Value);
            //    }
            //    if (rdbBankAll.Checked)
            //    {
            //        cmd.Parameters.AddWithValue("@BanckAc", "ALL");
            //    }
            //    else
            //    {
            //        cmd.Parameters.AddWithValue("@BanckAc", HiddenField_BRSAC.Value);
            //    }
            //    cmd.Parameters.AddWithValue("@ConsiderDate", DdlConsider.SelectedItem.Value.ToString().Trim());
            //    cmd.Parameters.AddWithValue("@Date", DtAsOn.Value);
            //    cmd.Parameters.AddWithValue("@FinYear", Session["LastFinYear"].ToString().Trim());

            //    SqlDataAdapter da = new SqlDataAdapter();
            //    da.SelectCommand = cmd;
            //    cmd.CommandTimeout = 0;
            //    ds.Reset();
            //    ds.Clear();
            //    da.Fill(ds);
            //    da.Dispose();
            //    ViewState["dataset"] = ds;
            //    FunctionCall(ds);
            //}

        }

        void FnHtml_Detail(DataSet ds)
        {
            String strHtmlheader = String.Empty;
            string str = null;
            str = " Consider:" + DdlConsider.SelectedItem.Text.ToString();
            str = str + " ; As On " + oconverter.ArrangeDate2(DtAsOn.Value.ToString());

            strHtmlheader = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtmlheader += "<tr><td align=\"left\" colspan=" + ds.Tables[0].Columns.Count + " style=\"color:Blue;\">" + str + "</td></tr></table>";

            DataView viewclient = new DataView();
            viewclient = ds.Tables[0].DefaultView;
            viewclient.RowFilter = "Param='" + cmbrecord.SelectedItem.Value.ToString().Trim() + "'";
            DataTable DtRecord = new DataTable();
            DtRecord = viewclient.ToTable();


            ////////Detail Display
            String strHtml = String.Empty;
            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtml += "<tr style=\"background-color:lavender ;text-align:left\">";
            strHtml += "<td align=\"left\" nowrap=\"nowrap;\">Bank Name :&nbsp;<b>" + cmbrecord.SelectedItem.Text.ToString().Trim() + "</b></td></tr>";


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
            amnt = Convert.ToDecimal(DtRecord.Rows[0]["Amount"].ToString().Trim());
            if (amnt > 0)
            {
                strHtml += "<td align=\"left\" nowrap=nowrap; style=\"font-size:xx-small;color:maroon;\"><b>" + DtRecord.Rows[0]["Result"].ToString().Trim() + "</b></td>";
                strHtml += "<td>&nbsp;</td></tr>";
            }
            else
            {
                strHtml += "<td>&nbsp;</td>";
                strHtml += "<td align=\"right\" nowrap=nowrap; style=\"font-size:xx-small;color:maroon;\"><b>" + DtRecord.Rows[0]["Result"].ToString().Trim() + "<b></td></tr>";
            }
            strHtml += "</tr></table></td></tr>";

            //////LESS: Cheques Deposited But Not Cleared:

            DataView viewrecord = new DataView();
            viewrecord = DtRecord.DefaultView;
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
            viewrecord1 = DtRecord.DefaultView;
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
            viewrecord2 = DtRecord.DefaultView;
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
            viewrecord3 = DtRecord.DefaultView;
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
            viewrecord4 = DtRecord.DefaultView;
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
            ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordDisplay", "RecordDisplay();", true);
        }
        void FnHtml_Summary(DataSet ds)
        {
            String strHtmlheader = String.Empty;
            string str = null;
            str = " [Summary View] ; As On " + oconverter.ArrangeDate2(DtAsOn.Value.ToString());

            strHtmlheader = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtmlheader += "<tr><td align=\"left\" colspan=" + ds.Tables[0].Columns.Count + " style=\"color:Blue;\">" + str + "</td></tr></table>";


            DataView viewclient = new DataView();
            viewclient = ds.Tables[0].DefaultView;
            viewclient.RowFilter = "Param='" + cmbrecord.SelectedItem.Value.ToString().Trim() + "'";
            DataTable DtRecord = new DataTable();
            DtRecord = viewclient.ToTable();


            ////////Detail Display
            String strHtml = String.Empty;
            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtml += "<tr style=\"background-color:lavender ;text-align:left\">";
            strHtml += "<td align=\"left\" nowrap=\"nowrap;\">Company Name :&nbsp;<b>" + cmbrecord.SelectedItem.Text.ToString().Trim() + "</b></td></tr>";


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
            for (int i = 0; i < DtRecord.Rows.Count; i++)
            {
                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";

                if (DtRecord.Rows[i]["AccountName"] != DBNull.Value)
                {
                    if (DtRecord.Rows[i]["AccountName"].ToString().Trim() == "Total")
                    {
                        strHtml += "<td align=\"left\" style=\"font-size:xx-small;color:#153E7E;\" nowrap=\"nowrap;\"><b>Total :</b></td>";
                    }
                    else
                    {
                        strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + DtRecord.Rows[i]["AccountName"].ToString() + "</td>";
                    }
                }
                else
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">&nbsp;</td>";

                if (DtRecord.Rows[i]["Code"] != DBNull.Value)
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + DtRecord.Rows[i]["Code"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">&nbsp;</td>";
                if (DtRecord.Rows[i]["AccountNumber"] != DBNull.Value)
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + DtRecord.Rows[i]["AccountNumber"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">&nbsp;</td>";
                if (DtRecord.Rows[i]["BookBalance"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + DtRecord.Rows[i]["BookBalance"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">&nbsp;</td>";
                if (DtRecord.Rows[i]["UnclearedPayments"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + DtRecord.Rows[i]["UnclearedPayments"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">&nbsp;</td>";
                if (DtRecord.Rows[i]["UnclearedReceipts"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + DtRecord.Rows[i]["UnclearedReceipts"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">&nbsp;</td>";
                if (DtRecord.Rows[i]["StatementBalance"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + DtRecord.Rows[i]["StatementBalance"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">&nbsp;</td>";
                strHtml += "<tr>";
            }

            strHtml += "</table>";
            DivHeader.InnerHtml = strHtmlheader;
            Divdisplay.InnerHtml = strHtml;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordDisplay", "RecordDisplay();", true);

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
            Procedure();
        }
        protected void btnpdf_Click(object sender, EventArgs e)
        {
            string Path = string.Empty;

            string[] connPath = ConfigurationManager.AppSettings["DBConnectionDefault"].Split(';');
            ReportDocument rpt = new ReportDocument();
            ReportDocument reportobj = new ReportDocument();
            //ds.WriteXmlSchema("E:\\RPTXSD\\brs.xsd");
            DataSet ds1 = new DataSet();
            string CompanyId = string.Empty;
            string BanckAc = string.Empty;
            if (RdbAllCompany.Checked)
            {
                CompanyId = "ALL";
            }
            else if (RdbCurrentCompany.Checked)
            {
                CompanyId = Session["LastCompany"].ToString().Trim();
            }
            else
            {
                CompanyId = HiddenField_Company.Value;
            }

            if (rdbBankAll.Checked)
            {
                BanckAc = "ALL";
            }
            else
            {
                BanckAc = HiddenField_BRSAC.Value;
            }

            ds = objfaReportOther.Report_BRSNew(
                     Convert.ToString(DdlReportView.SelectedItem.Value),
                    Convert.ToString(CompanyId),
                    Convert.ToString(BanckAc),
                    Convert.ToString(DdlConsider.SelectedItem.Value),
                  Convert.ToString(DtAsOn.Value),
                   Convert.ToString(Session["LastFinYear"])
                     );

            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            //{

            //    SqlCommand cmd = new SqlCommand();
            //    cmd.Connection = con;
            //    cmd.CommandText = "[Report_BRSNew]";
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.Parameters.AddWithValue("@ReportView", DdlReportView.SelectedItem.Value.ToString().Trim());
            //    if (RdbAllCompany.Checked)
            //    {
            //        cmd.Parameters.AddWithValue("@CompanyId", "ALL");
            //    }
            //    else if (RdbCurrentCompany.Checked)
            //    {
            //        cmd.Parameters.AddWithValue("@CompanyId", "'" + Session["LastCompany"].ToString().Trim() + "'");
            //    }
            //    else
            //    {
            //        cmd.Parameters.AddWithValue("@CompanyId", HiddenField_Company.Value);
            //    }
            //    if (rdbBankAll.Checked)
            //    {
            //        cmd.Parameters.AddWithValue("@BanckAc", "ALL");
            //    }
            //    else
            //    {
            //        cmd.Parameters.AddWithValue("@BanckAc", HiddenField_BRSAC.Value);
            //    }
            //    cmd.Parameters.AddWithValue("@ConsiderDate", DdlConsider.SelectedItem.Value.ToString().Trim());
            //    cmd.Parameters.AddWithValue("@Date", DtAsOn.Value);
            //    cmd.Parameters.AddWithValue("@FinYear", Session["LastFinYear"].ToString().Trim());

            //    SqlDataAdapter da = new SqlDataAdapter();
            //    da.SelectCommand = cmd;
            //    cmd.CommandTimeout = 0;
            //    ds.Reset();
            //    ds.Clear();
            //    da.Fill(ds);
            //    da.Dispose();
            //    GC.Collect();

            //}

            string newPath = ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\brs.xsd";
            ds.WriteXmlSchema(newPath);
            if (ds.Tables[0].Rows.Count > 0)
            {
                // ds.WriteXmlSchema("E:\\RPTXSD\\brs.xsd");
                Path = Server.MapPath(@"..\Reports\brs.rpt");
                rpt.Load(Path);
                rpt.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
                //rpt.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "ledger");
                rpt.SetDataSource(ds.Tables[0]);
                string MainAcc = "";
                if (DdlConsider.SelectedItem.Value == "1")
                {
                    MainAcc = "Value Date ";
                }
                else if (DdlConsider.SelectedItem.Value == "2")
                {
                    MainAcc = "Statement Date ";
                }
                rpt.SetParameterValue("@MainAccount", (string)MainAcc);
                rpt.SetParameterValue("@Period", oconverter.ArrangeDate2(DtAsOn.Value.ToString()));
                rpt.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "Bank Reconsilation Statement");
                rpt.Dispose();

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "fnNoRecord", "fnNoRecord('1');", true);
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

            DataView viewgrand = new DataView();
            viewgrand = ds.Tables[0].DefaultView;
            viewgrand.RowFilter = "Param<>'ZZZZZ'";
            DataTable DtGrand = new DataTable();
            DtGrand = viewgrand.ToTable();

            DataTable DistinctRecord = new DataTable();
            DataView viewClient = new DataView(DtGrand);
            DistinctRecord = viewClient.ToTable(true, new string[] { "Param" });

            for (int k = 0; k < DistinctRecord.Rows.Count; k++)
            {
                DataView viewgrp = new DataView();
                viewgrp = ds.Tables[0].DefaultView;
                viewgrp.RowFilter = "Param='" + DistinctRecord.Rows[k][0].ToString().Trim() + "'";
                DataTable DtRecord = new DataTable();
                DtRecord = viewgrp.ToTable();

                ////////Bank Name
                DataRow rowComName = dtExport.NewRow();
                rowComName[0] = "Company Name: " + DistinctRecord.Rows[k][0].ToString().Trim();
                rowComName[1] = "Test";
                dtExport.Rows.Add(rowComName);

                for (int i = 0; i < DtRecord.Rows.Count; i++)
                {
                    DataRow row = dtExport.NewRow();
                    if (DtRecord.Rows[i]["AccountName"] != DBNull.Value)
                        row[0] = DtRecord.Rows[i]["AccountName"].ToString();
                    if (DtRecord.Rows[i]["Code"] != DBNull.Value)
                        row[1] = DtRecord.Rows[i]["Code"].ToString();
                    if (DtRecord.Rows[i]["AccountNumber"] != DBNull.Value)
                        row[2] = DtRecord.Rows[i]["AccountNumber"].ToString();
                    if (DtRecord.Rows[i]["BookBalance"] != DBNull.Value)
                        row[3] = oconverter.formatmoneyinUs(Convert.ToDecimal(DtRecord.Rows[i]["BookBalance"].ToString()));
                    if (DtRecord.Rows[i]["UnclearedPayments"] != DBNull.Value)
                        row[4] = oconverter.formatmoneyinUs(Convert.ToDecimal(DtRecord.Rows[i]["UnclearedPayments"].ToString()));
                    if (DtRecord.Rows[i]["UnclearedReceipts"] != DBNull.Value)
                        row[5] = oconverter.formatmoneyinUs(Convert.ToDecimal(DtRecord.Rows[i]["UnclearedReceipts"].ToString()));
                    if (DtRecord.Rows[i]["StatementBalance"] != DBNull.Value)
                        row[6] = oconverter.formatmoneyinUs(Convert.ToDecimal(DtRecord.Rows[i]["StatementBalance"].ToString()));
                    dtExport.Rows.Add(row);
                }
            }

            DataView viewgrand1 = new DataView();
            viewgrand1 = ds.Tables[0].DefaultView;
            viewgrand1.RowFilter = "Param='ZZZZZ'";
            DataTable DtGrand1 = new DataTable();
            DtGrand1 = viewgrand1.ToTable();

            ////////Grand Total
            DataRow rowGrand = dtExport.NewRow();
            rowGrand[0] = "Grand Total:";
            if (DtGrand1.Rows[0]["BookBalance"] != DBNull.Value)
                rowGrand[3] = oconverter.formatmoneyinUs(Convert.ToDecimal(DtGrand1.Rows[0]["BookBalance"].ToString()));
            if (DtGrand1.Rows[0]["UnclearedPayments"] != DBNull.Value)
                rowGrand[4] = oconverter.formatmoneyinUs(Convert.ToDecimal(DtGrand1.Rows[0]["UnclearedPayments"].ToString()));
            if (DtGrand1.Rows[0]["UnclearedReceipts"] != DBNull.Value)
                rowGrand[5] = oconverter.formatmoneyinUs(Convert.ToDecimal(DtGrand1.Rows[0]["UnclearedReceipts"].ToString()));
            if (DtGrand1.Rows[0]["StatementBalance"] != DBNull.Value)
                rowGrand[6] = oconverter.formatmoneyinUs(Convert.ToDecimal(DtGrand1.Rows[0]["StatementBalance"].ToString()));
            dtExport.Rows.Add(rowGrand);

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

            objExcel.ExportToExcelforExcel(dtExport, "Bank Reconciliation Statement", "Total", dtReportHeader, dtReportFooter);
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


            DataTable DistinctRecord = new DataTable();
            DataView viewClient = new DataView(ds.Tables[0]);
            DistinctRecord = viewClient.ToTable(true, new string[] { "Param" });

            for (int k = 0; k < DistinctRecord.Rows.Count; k++)
            {
                DataView viewgrp = new DataView();
                viewgrp = ds.Tables[0].DefaultView;
                viewgrp.RowFilter = "Param='" + DistinctRecord.Rows[k][0].ToString().Trim() + "'";
                DataTable DtRecord = new DataTable();
                DtRecord = viewgrp.ToTable();

                ////////Bank Name
                DataRow rowBankName = dtExport.NewRow();
                rowBankName[0] = "Bank Name: " + DistinctRecord.Rows[k][0].ToString().Trim();
                rowBankName[1] = "Test";
                dtExport.Rows.Add(rowBankName);
                /////////Bank Balance

                DataRow row = dtExport.NewRow();
                row["Transaction Date"] = "Bank Balance As Per Bank Book :";
                decimal amnt = decimal.Zero;
                amnt = Convert.ToDecimal(DtRecord.Rows[0]["Amount"].ToString().Trim());
                if (amnt > 0)
                {
                    row["Amount(Dr.)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(DtRecord.Rows[0]["Result"].ToString()));
                }
                else
                {
                    row["Amount(Cr.)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(DtRecord.Rows[0]["Result"].ToString()));
                }
                dtExport.Rows.Add(row);

                /////////For LESS: Cheques Deposited But Not Cleared:
                DataView viewrecord = new DataView();
                viewrecord = DtRecord.DefaultView;
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
                viewrecord1 = DtRecord.DefaultView;
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
                viewrecord2 = DtRecord.DefaultView;
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
                viewrecord3 = DtRecord.DefaultView;
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
                viewrecord4 = DtRecord.DefaultView;
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

            }


            string str = null;
            //str = "[Detail View] For :" + txtBankName.Text.ToString().Trim() + " Consider:" + DdlConsider.SelectedItem.Text.ToString();
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

        void FunctionCall(DataSet ds)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "1")
                {
                    DropDownBindForDetailView(ds);
                }
                else
                {
                    Export(ds);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "fnNoRecord", "fnNoRecord('1');", true);
            }
        }
        void DropDownBindForDetailView(DataSet ds)
        {
            DataView viewgrand = new DataView();
            viewgrand = ds.Tables[0].DefaultView;
            viewgrand.RowFilter = "Param<>'ZZZZZ'";
            DataTable DtGrand = new DataTable();
            DtGrand = viewgrand.ToTable();


            DataView viewParam = new DataView(DtGrand);
            DtParam = viewParam.ToTable(true, new string[] { "Param" });
            if (DtParam.Rows.Count > 0)
            {
                cmbrecord.DataSource = DtParam;
                cmbrecord.DataValueField = "Param";
                cmbrecord.DataTextField = "Param";
                cmbrecord.DataBind();

            }
            ViewState["Bank"] = DtParam;
            LastPage = DtParam.Rows.Count - 1;
            CurrentPage = 0;
            bind_Details();
        }
        void bind_Details()
        {
            DtParam = (DataTable)ViewState["Bank"];
            cmbrecord.SelectedIndex = CurrentPage;
            if (LastPage > -1)
            {
                listRecord.Text = CurrentPage + 1 + " of " + DtParam.Rows.Count.ToString() + " Record.";

            }
            ds = (DataSet)ViewState["dataset"];
            if (DdlReportView.SelectedItem.Value.ToString().Trim() == "1")///Detail
            {
                FnHtml_Detail(ds);
            }
            if (DdlReportView.SelectedItem.Value.ToString().Trim() == "2")///Summary
            {
                FnHtml_Summary(ds);
            }

            ShowHidePreviousNext_of_Clients();
        }
        void ShowHidePreviousNext_of_Clients()
        {
            if (LastPage == 0 || LastPage == -1)
            {
                ASPxFirst.Style["Display"] = "none";
                ASPxPrevious.Style["Display"] = "none";
                ASPxNext.Style["Display"] = "none";
                ASPxLast.Style["Display"] = "none";

            }
            else
            {
                ASPxFirst.Style["Display"] = "Display";
                ASPxPrevious.Style["Display"] = "Display";
                ASPxNext.Style["Display"] = "Display";
                ASPxLast.Style["Display"] = "Display";

            }

            if (CurrentPage == LastPage && LastPage != 0)
            {

                ASPxFirst.Enabled = true;
                ASPxPrevious.Enabled = true;

                ASPxNext.Enabled = false;
                ASPxLast.Enabled = false;

            }
            else
                if (CurrentPage == 0 && LastPage != 0)
                {
                    ASPxFirst.Enabled = false;
                    ASPxPrevious.Enabled = false;

                    ASPxNext.Enabled = true;
                    ASPxLast.Enabled = true;


                }
                else
                {
                    ASPxFirst.Enabled = true;
                    ASPxPrevious.Enabled = true;
                    ASPxNext.Enabled = true;
                    ASPxLast.Enabled = true;
                }
        }
        protected void ASPxFirst_Click(object sender, EventArgs e)
        {
            hiddencount.Value = "0";
            CurrentPage = 0;
            bind_Details();
        }
        protected void ASPxPrevious_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 0)
            {
                hiddencount.Value = "0";
                CurrentPage = CurrentPage - 1;
                bind_Details();
            }
        }
        protected void ASPxNext_Click(object sender, EventArgs e)
        {
            if (CurrentPage < LastPage)
            {
                hiddencount.Value = "0";
                CurrentPage = CurrentPage + 1;
                bind_Details();
            }
        }
        protected void ASPxLast_Click(object sender, EventArgs e)
        {
            hiddencount.Value = "0";
            CurrentPage = LastPage;
            bind_Details();
        }
    }
}