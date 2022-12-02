using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_ProfitLossStatement : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        DataSet ds = new DataSet();

        ExcelFile objExcel = new ExcelFile();
        DataTable dtExport = new DataTable();

        ProfitLossStatementBL OProfitLossStatementBL = new ProfitLossStatementBL();
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

                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");
                Date();


            }
            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //___________-end here___//

        }
        void Date()
        {
            DtFrom.EditFormatString = oconverter.GetDateFormat("Date");
            DtTo.EditFormatString = oconverter.GetDateFormat("Date");

            DtFrom.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
            DtTo.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());

            SegmentnameFetch();

        }
        void SegmentnameFetch()
        {
            if (Convert.ToString(HttpContext.Current.Session["ExchangeSegmentID"]).Trim() == "1")
                litSegmentMain.InnerText = "NSE-CM";
            if (Convert.ToString(HttpContext.Current.Session["ExchangeSegmentID"]).Trim() == "4")
                litSegmentMain.InnerText = "BSE-CM";
            if (Convert.ToString(HttpContext.Current.Session["ExchangeSegmentID"]) == "15")
                litSegmentMain.InnerText = "CSE-CM";
            if (Convert.ToString(HttpContext.Current.Session["ExchangeSegmentID"]) == "2")
                litSegmentMain.InnerText = "NSE-FO";
            if (Convert.ToString(HttpContext.Current.Session["ExchangeSegmentID"]) == "4")
                litSegmentMain.InnerText = "BSE-FO";
            if (Convert.ToString(HttpContext.Current.Session["ExchangeSegmentID"]) == "3")
                litSegmentMain.InnerText = "NSE-CDX";
            if (Convert.ToString(HttpContext.Current.Session["ExchangeSegmentID"]) == "6")
                litSegmentMain.InnerText = "BSE-CDX";
            if (Convert.ToString(HttpContext.Current.Session["ExchangeSegmentID"]) == "7")
                litSegmentMain.InnerText = "MCX-COMM";
            if (Convert.ToString(HttpContext.Current.Session["ExchangeSegmentID"]) == "8")
                litSegmentMain.InnerText = "MCXSX-CDX";
            if (Convert.ToString(HttpContext.Current.Session["ExchangeSegmentID"]) == "9")
                litSegmentMain.InnerText = "NCDEX-COMM";
            if (Convert.ToString(HttpContext.Current.Session["ExchangeSegmentID"]) == "10")
                litSegmentMain.InnerText = "DGCX-COMM";
            if (Convert.ToString(HttpContext.Current.Session["ExchangeSegmentID"]) == "11")
                litSegmentMain.InnerText = "NMCE-COMM";
            if (Convert.ToString(HttpContext.Current.Session["ExchangeSegmentID"]) == "12")
                litSegmentMain.InnerText = "ICEX-COMM";
            if (Convert.ToString(HttpContext.Current.Session["ExchangeSegmentID"]) == "13")
                litSegmentMain.InnerText = "USE-CDX";
            if (Convert.ToString(HttpContext.Current.Session["ExchangeSegmentID"]) == "14")
                litSegmentMain.InnerText = "NSEL-SPOT";
        }

        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }
        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            string id = eventArgument.ToString();
            string[] idlist = id.Split('~');
            string str = "";
            string str1 = "";
            if (idlist[0] == "ComboChange")
            {
                //MainAcID = idlist[1];
            }
            else
            {
                string[] cl = idlist[1].Split(',');
                for (int i = 0; i < cl.Length; i++)
                {
                    if (idlist[0].ToString().Trim() != "Company")
                    {
                        string[] val = cl[i].Split(';');
                        if (str == "")
                        {
                            str = val[0];
                            str1 = val[1];
                        }
                        else
                        {
                            str += "," + val[0];
                            str1 += "," + val[1];
                        }
                    }
                    else
                    {
                        string[] val = cl[i].Split(';');
                        string[] AcVal = val[0].Split('-');
                        if (str == "")
                        {
                            str = "'" + AcVal[0] + "'";
                            str1 = val[1];
                        }
                        else
                        {
                            str += ",'" + AcVal[0] + "'";
                            str1 += "," + val[1];
                        }
                    }
                }


                if (idlist[0] == "Company")
                {
                    data = "Company~" + str + '~' + str1;
                }
                else if (idlist[0] == "Branch")
                {
                    data = "Branch~" + str + '~' + str1;
                }
                else if (idlist[0] == "Segment")
                {
                    data = "Segment~" + str + '~' + str1;
                }

            }
        }

        protected void btnScreen_Click(object sender, EventArgs e)
        {
            Procedure();
        }
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            Procedure();
        }

        void Procedure()
        {

            /* For Tier Structrure
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            {

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "[Report_ProfitLossStatement]";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@FromDate", DtFrom.Value);
                cmd.Parameters.AddWithValue("@ToDate", DtTo.Value);
                if (RdbAllCompany.Checked)
                {
                    cmd.Parameters.AddWithValue("@CompanyId", "ALL");
                }
                else if (RdbCurrentCompany.Checked)
                {
                    cmd.Parameters.AddWithValue("@CompanyId", "'" + Session["LastCompany"].ToString().Trim() + "'");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@CompanyId", HiddenField_Company.Value);
                }
                if (rdbSegmentAll.Checked)
                {
                    cmd.Parameters.AddWithValue("@Segmentid", "ALL");
                }
                else if (rdSegmentSelected.Checked)
                {
                    cmd.Parameters.AddWithValue("@Segmentid", HiddenField_Segment.Value.ToString().Trim());
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Segmentid", Convert.ToInt32(Session["usersegid"].ToString()));
                }
                if (RdBranchAll.Checked)
                {
                    cmd.Parameters.AddWithValue("@Branch", "ALL");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Branch", HiddenField_Branch.Value);
                }
                cmd.Parameters.AddWithValue("@ReportView", DdlrptView.SelectedItem.Value.ToString().Trim());
                cmd.Parameters.AddWithValue("@ReportStyle", DdlrptStyle.SelectedItem.Value.ToString().Trim());
                cmd.Parameters.AddWithValue("@MonthlyBreakUp", ChkMonthlyBreakUp.Checked.ToString().Trim());
                cmd.Parameters.AddWithValue("@ZeroAmntAc", ChkZeroAmntAc.Checked.ToString().Trim());
                cmd.Parameters.AddWithValue("@ActiveCurrency",Session["ActiveCurrency"].ToString().Split('~')[0]);
                cmd.Parameters.AddWithValue("@TradeCurrency", Session["TradeCurrency"].ToString().Split('~')[0]);

              

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.CommandTimeout = 0;
                ds.Reset();
                ds.Clear();
                da.Fill(ds);
                da.Dispose();

                */


            string CompanyId, Segmentid, Branch;

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

            if (rdbSegmentAll.Checked)
            {
                Segmentid = "ALL";
            }
            else if (rdSegmentSelected.Checked)
            {
                Segmentid = HiddenField_Segment.Value.ToString().Trim();
            }
            else
            {
                Segmentid = Session["usersegid"].ToString();
            }
            if (RdBranchAll.Checked)
            {
                Branch = "ALL";
            }
            else
            {
                Branch = HiddenField_Branch.Value;
            }



            ds = OProfitLossStatementBL.Report_ProfitLossStatement(DtFrom.Value.ToString(), DtTo.Value.ToString(), CompanyId, Segmentid, Branch, DdlrptView.SelectedItem.Value.ToString().Trim(),
              DdlrptStyle.SelectedItem.Value.ToString().Trim(), ChkMonthlyBreakUp.Checked.ToString().Trim(), ChkZeroAmntAc.Checked.ToString().Trim(),
              Session["ActiveCurrency"].ToString().Split('~')[0], Session["TradeCurrency"].ToString().Split('~')[0]);



            if (ChkMonthlyBreakUp.Checked)
            {
                ds.Tables[0].Columns.Remove("CmpName");
                ds.Tables[0].Columns.Remove("ProfitLossType");
                ds.Tables[0].Columns.Remove("StOrder");
                ds.Tables[0].Columns.Remove("MainAcountName");
            }
            ViewState["dataset"] = ds;
            DisPlay(ds);

            //}

        }

        void DisPlay(DataSet ds)
        {
            string strDate = null;
            string strCompany = null;
            string strSegment = null;
            string strBranch = null;
            strDate = " Report View:" + DdlrptView.SelectedItem.Text.ToString().Trim() + " ; Report Style:" + DdlrptStyle.SelectedItem.Text.ToString().Trim();
            strDate = strDate + " ; For The Period : " + oconverter.ArrangeDate2(DtFrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(DtTo.Value.ToString());
            if (RdbAllCompany.Checked)
            {
                strCompany = "Company :ALL";
            }
            else if (RdbCurrentCompany.Checked)
            {
                strCompany = "Company :Current";
            }
            else
            {
                strCompany = "Company :" + HiddenField_CompanyName.Value.ToString().Trim();
            }
            if (rdbSegmentAll.Checked)
            {
                strSegment = "Segment :ALL";
            }
            else if (rdSegmentSelected.Checked)
            {
                strSegment = "Segment :" + HiddenField_SegmentName.Value.ToString().Trim();
            }
            else
            {
                strSegment = "Segment :Current";
            }
            if (RdBranchAll.Checked)
            {
                strBranch = "Branch :All";
            }
            else
            {
                strBranch = "Branch :" + HiddenField_BranchName.Value.ToString().Trim();
            }


            //////////Function Call
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ddlGeneration.SelectedItem.Value.ToString() == "1")///Screen
                {
                    FnHtml(ds, strDate.ToString().Trim(), strCompany.ToString().Trim(), strSegment.ToString().Trim(), strBranch.ToString().Trim());
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "fnRecord", "fnRecord('2');", true);
                }
                if (ddlGeneration.SelectedItem.Value.ToString() == "2")///Export
                {
                    Export(ds, strDate.ToString().Trim(), strCompany.ToString().Trim(), strSegment.ToString().Trim(), strBranch.ToString().Trim());

                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "fnNoRecord", "fnRecord('1');", true);
            }
        }
        void FnHtml(DataSet ds, string strDate, string strCompany, string strSegment, string strBranch)
        {
            //////////For header
            String strHtmlheader = String.Empty;

            strHtmlheader = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtmlheader += "<tr><td align=\"left\" colspan=" + ds.Tables[0].Columns.Count + " style=\"color:Blue;\">" + strDate + "</td></tr>";
            strHtmlheader += "<tr><td align=\"left\" colspan=" + ds.Tables[0].Columns.Count + " style=\"color:Blue;\">" + strCompany + "</td></tr>";
            strHtmlheader += "<tr><td align=\"left\" colspan=" + ds.Tables[0].Columns.Count + " style=\"color:Blue;\">" + strSegment + "</td></tr>";
            strHtmlheader += "<tr><td align=\"left\" colspan=" + ds.Tables[0].Columns.Count + " style=\"color:Blue;\">" + strBranch + "</td></tr></table>";

            ////////Detail Display
            String strHtml = String.Empty;
            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";



            //////////////TABLE HEADER BIND
            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
            {
                strHtml += "<td align=\"center\" style=\"font-size:smaller;\" nowrap=nowrap;><b>" + ds.Tables[0].Columns[i].ColumnName + "</b></td>";
            }
            strHtml += "</tr>";

            int flag = 0;
            foreach (DataRow dr1 in ds.Tables[0].Rows)
            {
                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                {
                    if (dr1[j] != DBNull.Value)
                    {
                        if (dr1[j].ToString().Trim().StartsWith("Income") || dr1[j].ToString().Trim().StartsWith("Expenditure"))
                        {
                            strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\"><u><b>" + dr1[j] + "</b></u></td>";
                        }
                        else if (dr1[j].ToString().Trim().StartsWith("**") || dr1[j].ToString().Trim().StartsWith("Net") || dr1[j].ToString().Trim().StartsWith("Grand") || dr1[j].ToString().Trim().StartsWith("A/c Group") || dr1[j].ToString().Trim().StartsWith("Gross"))
                        {
                            strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\"><b>" + dr1[j] + "</b></td>";
                        }
                        else if (dr1[j].ToString().Trim().StartsWith("Test"))
                        {
                            strHtml += "<td>&nbsp;</td>";
                        }
                        else
                        {
                            if (IsNumeric(dr1[j].ToString()) == true)
                            {
                                strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                            }
                            else
                            {
                                strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                            }
                        }
                    }
                    else
                    {
                        strHtml += "<td>&nbsp;</td>";
                    }
                }

                strHtml += "</tr>";
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
        public static bool IsNumeric(object value)
        {
            double dbl;
            return double.TryParse(value.ToString(), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out dbl);
        }
        void Export(DataSet ds, string strDate, string strCompany, string strSegment, string strBranch)
        {
            DataTable dtExport = ds.Tables[0].Copy();


            DataTable dtReportHeader = new DataTable();
            DataTable dtReportFooter = new DataTable();

            DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            HeaderRow[0] = strDate.ToString().Trim() + " " + strCompany.ToString().Trim();
            dtReportHeader.Rows.Add(HeaderRow);
            DataRow DrRowR1 = dtReportHeader.NewRow();
            DrRowR1[0] = strSegment.ToString().Trim() + " " + strBranch.ToString().Trim();
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

            if (dtExport.Columns.Count < 4)
            {
                dtExport.Columns.Add("Remarks", Type.GetType("System.String"));
            }
            objExcel.ExportToExcelforExcel(dtExport, "Profit & Loss Statement", "Grand Income Over Expenditure", dtReportHeader, dtReportFooter);

        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            ds = (DataSet)ViewState["dataset"];
            string strDate = null;
            string strCompany = null;
            string strSegment = null;
            string strBranch = null;
            strDate = " Report View:" + DdlrptView.SelectedItem.Text.ToString().Trim() + " ; Report Style:" + DdlrptStyle.SelectedItem.Text.ToString().Trim();
            strDate = strDate + " ; For The Period : " + oconverter.ArrangeDate2(DtFrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(DtTo.Value.ToString());
            if (RdbAllCompany.Checked)
            {
                strCompany = "Company :ALL";
            }
            else if (RdbCurrentCompany.Checked)
            {
                strCompany = "Company :Current";
            }
            else
            {
                strCompany = "Company :" + HiddenField_CompanyName.Value.ToString().Trim();
            }
            if (rdbSegmentAll.Checked)
            {
                strSegment = "Segment :ALL";
            }
            else if (rdSegmentSelected.Checked)
            {
                strSegment = "Segment :" + HiddenField_SegmentName.Value.ToString().Trim();
            }
            else
            {
                strSegment = "Segment :Current";
            }
            if (RdBranchAll.Checked)
            {
                strBranch = "Branch :All";
            }
            else
            {
                strBranch = "Branch :" + HiddenField_BranchName.Value.ToString().Trim();
            }
            Export(ds, strDate.ToString().Trim(), strCompany.ToString().Trim(), strSegment.ToString().Trim(), strBranch.ToString().Trim());

        }
    }
}