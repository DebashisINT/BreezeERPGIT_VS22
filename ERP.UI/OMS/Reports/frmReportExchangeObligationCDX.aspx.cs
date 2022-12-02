using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;
namespace ERP.OMS.Reports
{
    public partial class Reports_frmReportExchangeObligationCDX : System.Web.UI.Page
    {
        ExcelFile objExcel = new ExcelFile();
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        //DBEngine oDBEngine = new DBEngine(string.Empty);
        DataSet ds = new DataSet();
        DataTable dtExport = new DataTable();
        DailyReports dailyrep = new DailyReports();
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
                date();
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");
            }
        }

        void date()
        {
            dtFor.EditFormatString = oconverter.GetDateFormat("Date");
            dtFor.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
        }

        protected void btn_show_Click1(object sender, EventArgs e)
        {
            procedure();
        }
        void procedure()
        {

            ds = dailyrep.ExchangeObligationCDX(dtFor.Value.ToString(), Session["usersegid"].ToString(), HttpContext.Current.Session["ExchangeSegmentID"].ToString(), Session["LastCompany"].ToString());
            ViewState["dataset"] = ds;
            if (ds.Tables[0].Rows[0]["status"].ToString().Trim() == "1")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD2", "NORECORD(2);", true);

            }
            else
            {
                if (ds.Tables[1].Rows.Count > 0)
                {
                    htmltable();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD1", "NORECORD(1);", true);

                }
            }
        }



        void htmltable()
        {
            ds = (DataSet)ViewState["dataset"];
            String strHtml = String.Empty;
            String strHtml1 = String.Empty;

            strHtml1 = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtml1 += "<tr><td align=\"left\" colspan=16 style=\"color:Blue;\">Report Period : " + oconverter.ArrangeDate2(dtFor.Value.ToString()) + "</td></tr></table>";

            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtml += "<tr style=\"background-color:lavender ;text-align:left\">";
            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            strHtml += "<td align=\"center\" ><b>Instrument</b></td>";
            strHtml += "<td align=\"center\" ><b>Expiry </br> Date</b></td>";
            strHtml += "<td align=\"center\" ><b>B/F  </br> Qty</b></td>";
            strHtml += "<td align=\"center\"><b>Open  </br> Price</b></td>";
            strHtml += "<td align=\"center\" ><b>B/F  </br> Value</b></td>";
            strHtml += "<td align=\"center\"><b>Day   </br> Buy</b></td>";
            strHtml += "<td align=\"center\" ><b>Buy  </br> Value</b></td>";
            strHtml += "<td align=\"center\"><b>Day  </br> Sell</b></td>";
            strHtml += "<td align=\"center\" ><b>Sell  </br> Value</b></td>";
            strHtml += "<td align=\"center\"><b>C/F  </br> Qty</b></td>";
            strHtml += "<td align=\"center\" ><b>Sett  </br> Price</b></td>";
            strHtml += "<td align=\"center\"><b>C/F  </br> Value</b></td>";
            strHtml += "<td align=\"center\" ><b>Premium</b></td>";
            strHtml += "<td align=\"center\"><b>MTM</b></td>";
            strHtml += "<td align=\"center\" ><b>Future </br>FinSett</b></td>";
            strHtml += "<td align=\"center\"><b>ASN/EXC </br>Amount</b></td>";
            strHtml += "<td align=\"center\" ><b>Net </br>Obligation</b></td>";
            strHtml += "</tr>";

            ///////////FOR EXPORT BEGIN
            DataTable dtExport = new DataTable();
            dtExport.Clear();
            dtExport.Columns.Add("Instrument", Type.GetType("System.String"));
            dtExport.Columns.Add("Expiry Date", Type.GetType("System.String"));
            dtExport.Columns.Add("B/F Qty", Type.GetType("System.String"));
            dtExport.Columns.Add("Open Price", Type.GetType("System.String"));
            dtExport.Columns.Add("B/F Value", Type.GetType("System.String"));
            dtExport.Columns.Add("Day Buy", Type.GetType("System.String"));
            dtExport.Columns.Add("Buy Value", Type.GetType("System.String"));
            dtExport.Columns.Add("Day Sell", Type.GetType("System.String"));
            dtExport.Columns.Add("Sell Value", Type.GetType("System.String"));
            dtExport.Columns.Add("C/F Qty", Type.GetType("System.String"));
            dtExport.Columns.Add("Sett Price", Type.GetType("System.String"));
            dtExport.Columns.Add("C/F Value", Type.GetType("System.String"));
            dtExport.Columns.Add("Premium", Type.GetType("System.String"));
            dtExport.Columns.Add("MTM", Type.GetType("System.String"));
            dtExport.Columns.Add("Future FinSett", Type.GetType("System.String"));
            dtExport.Columns.Add("ASN/EXC Amount", Type.GetType("System.String"));
            dtExport.Columns.Add("Net Obligation", Type.GetType("System.String"));
            ////FOR EXPORT END

            int flag = 0;
            for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
            {
                flag = flag + 1;
                DataRow row = dtExport.NewRow();
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                if (ds.Tables[1].Rows[i]["Symbol"] != DBNull.Value)
                {
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;color:#348017\" >" + ds.Tables[1].Rows[i]["Symbol"].ToString() + "</td>";
                    row[0] = ds.Tables[1].Rows[i]["Symbol"].ToString();
                }
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (ds.Tables[1].Rows[i]["expirydate"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + ds.Tables[1].Rows[i]["expirydate"].ToString() + "</td>";
                    row[1] = ds.Tables[1].Rows[i]["expirydate"].ToString();
                }
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (ds.Tables[1].Rows[i]["LOTSRESULT_B"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + oconverter.getFormattedvalueWithounDecimalPlace(Convert.ToDecimal(ds.Tables[1].Rows[i]["LOTSRESULT_B"].ToString())) + "</td>";
                    row[2] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[1].Rows[i]["LOTSRESULT_B"].ToString()));
                }
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (ds.Tables[1].Rows[i]["SettlementPrice"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[1].Rows[i]["SettlementPrice"].ToString())) + "</td>";
                    row[3] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[1].Rows[i]["SettlementPrice"].ToString()));
                }
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (ds.Tables[1].Rows[i]["BFVALUE"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[1].Rows[i]["BFVALUE"].ToString())) + "</td>";
                    row[4] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[1].Rows[i]["BFVALUE"].ToString()));
                }
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (ds.Tables[1].Rows[i]["DAYBUY"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + oconverter.getFormattedvalueWithounDecimalPlace(Convert.ToDecimal(ds.Tables[1].Rows[i]["DAYBUY"].ToString())) + "</td>";
                    row[5] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[1].Rows[i]["DAYBUY"].ToString()));

                }
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (ds.Tables[1].Rows[i]["BUYVALUE"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[1].Rows[i]["BUYVALUE"].ToString())) + "</td>";
                    row[6] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[1].Rows[i]["BUYVALUE"].ToString()));

                }
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (ds.Tables[1].Rows[i]["DAYSELL"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + oconverter.getFormattedvalueWithounDecimalPlace(Convert.ToDecimal(ds.Tables[1].Rows[i]["DAYSELL"].ToString())) + "</td>";
                    row[7] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[1].Rows[i]["DAYSELL"].ToString()));

                }
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (ds.Tables[1].Rows[i]["SELLVALUE"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[1].Rows[i]["SELLVALUE"].ToString())) + "</td>";
                    row[8] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[1].Rows[i]["SELLVALUE"].ToString()));

                }
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (ds.Tables[1].Rows[i]["CFQTY_I"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + oconverter.getFormattedvalueWithounDecimalPlace(Convert.ToDecimal(ds.Tables[1].Rows[i]["CFQTY_I"].ToString())) + "</td>";
                    row[9] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[1].Rows[i]["CFQTY_I"].ToString()));

                }
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (ds.Tables[1].Rows[i]["SETTPRICE"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[1].Rows[i]["SETTPRICE"].ToString())) + "</td>";
                    row[10] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[1].Rows[i]["SETTPRICE"].ToString()));

                }
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (ds.Tables[1].Rows[i]["CFVALUE"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[1].Rows[i]["CFVALUE"].ToString())) + "</td>";
                    row[11] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[1].Rows[i]["CFVALUE"].ToString()));

                }
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (ds.Tables[1].Rows[i]["PRM"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[1].Rows[i]["PRM"].ToString())) + "</td>";
                    row[12] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[1].Rows[i]["PRM"].ToString()));

                }
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (ds.Tables[1].Rows[i]["MTM"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[1].Rows[i]["MTM"].ToString())) + "</td>";
                    row[13] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[1].Rows[i]["MTM"].ToString()));

                }
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (ds.Tables[1].Rows[i]["FutFin"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[1].Rows[i]["FutFin"].ToString())) + "</td>";
                    row[14] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[1].Rows[i]["FutFin"].ToString()));

                }
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (ds.Tables[1].Rows[i]["ExcAsn"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[1].Rows[i]["ExcAsn"].ToString())) + "</td>";
                    row[15] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[1].Rows[i]["ExcAsn"].ToString()));

                }
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (ds.Tables[1].Rows[i]["NETOBLIGATION"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[1].Rows[i]["NETOBLIGATION"].ToString())) + "</td>";
                    row[16] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[1].Rows[i]["NETOBLIGATION"].ToString()));

                }
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                strHtml += "</tr>";
                dtExport.Rows.Add(row);
            }
            /////////Total Display
            flag = flag + 1;
            strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
            DataRow row1 = dtExport.NewRow();
            strHtml += "<td align=\"left\" style=\"color:#maroon\" colspan=4><b>Total :</b></td>";
            row1[0] = "Total";

            if (ds.Tables[2].Rows[0]["BFVALUE_Sum"] != DBNull.Value)
            {
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[2].Rows[0]["BFVALUE_Sum"].ToString())) + "</td>";
                row1[4] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[2].Rows[0]["BFVALUE_Sum"].ToString()));

            }
            else
                strHtml += "<td align=\"left\" >&nbsp;</td>";

            strHtml += "<td align=\"left\" >&nbsp;</td>";

            if (ds.Tables[2].Rows[0]["BUYVALUE_Sum"] != DBNull.Value)
            {
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[2].Rows[0]["BUYVALUE_Sum"].ToString())) + "</td>";
                row1[6] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[2].Rows[0]["BUYVALUE_Sum"].ToString()));

            }
            else
                strHtml += "<td align=\"left\" >&nbsp;</td>";


            strHtml += "<td align=\"left\" >&nbsp;</td>";

            if (ds.Tables[2].Rows[0]["SELLVALUE_Sum"] != DBNull.Value)
            {
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[2].Rows[0]["SELLVALUE_Sum"].ToString())) + "</td>";
                row1[8] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[2].Rows[0]["SELLVALUE_Sum"].ToString()));

            }
            else
                strHtml += "<td align=\"left\" >&nbsp;</td>";

            strHtml += "<td align=\"left\" >&nbsp;</td>";
            strHtml += "<td align=\"left\" >&nbsp;</td>";

            if (ds.Tables[2].Rows[0]["CFVALUE_Sum"] != DBNull.Value)
            {
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[2].Rows[0]["CFVALUE_Sum"].ToString())) + "</td>";
                row1[11] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[2].Rows[0]["CFVALUE_Sum"].ToString()));

            }
            else
                strHtml += "<td align=\"left\" >&nbsp;</td>";

            if (ds.Tables[2].Rows[0]["PRM_Sum"] != DBNull.Value)
            {
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[2].Rows[0]["PRM_Sum"].ToString())) + "</td>";
                row1[12] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[2].Rows[0]["PRM_Sum"].ToString()));

            }
            else
                strHtml += "<td align=\"left\" >&nbsp;</td>";

            if (ds.Tables[2].Rows[0]["MTM_Sum"] != DBNull.Value)
            {
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[2].Rows[0]["MTM_Sum"].ToString())) + "</td>";
                row1[13] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[2].Rows[0]["MTM_Sum"].ToString()));

            }
            else
                strHtml += "<td align=\"left\" >&nbsp;</td>";

            if (ds.Tables[2].Rows[0]["FINSETT_Sum"] != DBNull.Value)
            {
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[2].Rows[0]["FINSETT_Sum"].ToString())) + "</td>";
                row1[14] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[2].Rows[0]["FINSETT_Sum"].ToString()));

            }
            else
                strHtml += "<td align=\"left\" >&nbsp;</td>";

            if (ds.Tables[2].Rows[0]["ExcAsn_Sum"] != DBNull.Value)
            {
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[2].Rows[0]["ExcAsn_Sum"].ToString())) + "</td>";
                row1[15] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[2].Rows[0]["ExcAsn_Sum"].ToString()));

            }
            else
                strHtml += "<td align=\"left\" >&nbsp;</td>";

            if (ds.Tables[2].Rows[0]["NETOBLIGATION_Sum"] != DBNull.Value)
            {
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[2].Rows[0]["NETOBLIGATION_Sum"].ToString())) + "</td>";
                row1[16] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[2].Rows[0]["NETOBLIGATION_Sum"].ToString()));

            }
            else
                strHtml += "<td align=\"left\" >&nbsp;</td>";

            strHtml += "</tr>";
            dtExport.Rows.Add(row1);

            ViewState["dtExport"] = dtExport;
            DIVdisplayPERIOD.InnerHtml = strHtml1;
            display.InnerHtml = strHtml;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Display", "Display();", true);
        }
        protected string GetRowColor(int i)
        {
            if (i++ % 2 == 0)
                return "#fff0f5";
            else
                return "White";
        }
        protected void ddlExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            dtExport = (DataTable)ViewState["dtExport"];

            DataTable dtReportHeader = new DataTable();
            DataTable dtReportFooter = new DataTable();

            DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            HeaderRow[0] = CompanyName.Rows[0][0].ToString();
            dtReportHeader.Rows.Add(HeaderRow);
            DataRow DrRowR1 = dtReportHeader.NewRow();



            DrRowR1[0] = "Exchange Obligation :" + oconverter.ArrangeDate2(dtFor.Value.ToString());

            dtReportHeader.Rows.Add(DrRowR1);
            DataRow DrRowR2 = dtReportHeader.NewRow();
            //DrRowR2[0] = txtBankName.Text;
            dtReportHeader.Rows.Add(DrRowR2);
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

            if (ddlExport.SelectedItem.Value == "E")
            {
                objExcel.ExportToExcelforExcel(dtExport, "Exchange Obligation", "Total", dtReportHeader, dtReportFooter);
            }
        }
    }
}