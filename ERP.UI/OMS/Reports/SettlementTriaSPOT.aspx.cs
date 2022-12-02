using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;

namespace ERP.OMS.Reports
{
    public partial class Reports_SettlementTriaSPOT : System.Web.UI.Page
    {
        BusinessLogicLayer.Reports oReports = new BusinessLogicLayer.Reports();
        ExcelFile objExcel = new ExcelFile();
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        DataSet ds = new DataSet();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        //DBEngine oDBEngine = new DBEngine(string.Empty);
        String strHtml = String.Empty;
        DataTable dtExport = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");

            if (!IsPostBack)
            {
                dtdate.EditFormatString = oconverter.GetDateFormat("Date");
                dtdate.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
            }
        }

        protected void btn_show_Click(object sender, EventArgs e)
        {
            procedure();
        }
        void procedure()
        {
            ds = oReports.SettlementTrialSPOT(
                 Convert.ToString(dtdate.Value),
                 Convert.ToString(Session["LastCompany"]),
                  Convert.ToString(Session["usersegid"]),
                 Convert.ToString(HttpContext.Current.Session["ExchangeSegmentID"])
                 );
            ViewState["dataset"] = ds;


            if (ds.Tables[0].Rows[0]["status"].ToString().Trim() == "1")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD1", "NORECORD(1);", true);
            }
            else
            {
                if (ds.Tables[0].Rows.Count == 0 || ds.Tables[0].Rows.Count == 3)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(2);", true);
                }
                else
                {
                    if (ddlType.SelectedItem.Value.ToString() == "0")
                    {
                        clientwise();
                    }
                    else
                    {
                        //branchwise();
                    }
                }
            }

        }
        protected string GetRowColor(int i)
        {

            if (i++ % 2 == 0)
                return "#fff0f5";
            else
                return "White";
        }
        void clientwise()
        {
            int flag = 0;
            ds = (DataSet)ViewState["dataset"];
            if (ds.Tables[1].Rows.Count != 0 || ds.Tables[3].Rows.Count != 0)
            {
                strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                strHtml += "<tr><td align=\"left\" colspan=9 style=\"color:Blue;\"><b>Settelment Trail for " + oconverter.ArrangeDate2(dtdate.Value.ToString()) + "</b></td></tr>";

                strHtml += "<tr style=\"background-color: #DBEEF3;\">";
                strHtml += "<td align=\"center\" colspan=2>Client Name</td>";
                strHtml += "<td align=\"center\">Code</td>";
                strHtml += "<td align=\"center\">MTM</td>";
                strHtml += "<td align=\"center\">Tran Charges</td>";
                strHtml += "<td align=\"center\">Other Charges</td>";
                strHtml += "<td align=\"center\">Serv Tax & Cess</td>";
                strHtml += "<td align=\"center\">Stamp Duty</td>";
                strHtml += "<td align=\"center\">Net Receivable (Dr.)</td>";
                strHtml += "<td align=\"center\">Net Payable (Cr.)</td></tr>";

                dtExport.Columns.Add("Client Name", Type.GetType("System.String"));
                dtExport.Columns.Add("Code", Type.GetType("System.String"));
                dtExport.Columns.Add("MTM", Type.GetType("System.String"));
                dtExport.Columns.Add("Tran Charges", Type.GetType("System.String"));
                dtExport.Columns.Add("Other Charges", Type.GetType("System.String"));
                dtExport.Columns.Add("Serv Tax & Cess", Type.GetType("System.String"));
                dtExport.Columns.Add("Stamp Duty", Type.GetType("System.String"));
                dtExport.Columns.Add("Net Receivable (Dr.)", Type.GetType("System.String"));
                dtExport.Columns.Add("Net Payable (Cr.)", Type.GetType("System.String"));
            }
            if (ds.Tables[1].Rows.Count != 0)
            {

                DataView view1 = new DataView(ds.Tables[5]);
                DataTable dt = new DataTable();
                dt = view1.ToTable(true, new string[] { "branchcode", "branchname" });

                if (dt.Rows.Count > 0)
                {
                    cmb.DataSource = dt;
                    cmb.DataValueField = "branchcode";
                    cmb.DataTextField = "branchname";
                    cmb.DataBind();
                }
                for (int j = 0; j < cmb.Items.Count; j++)
                {
                    DataView view2 = new DataView();
                    view2 = ds.Tables[1].DefaultView;
                    view2.RowFilter = "branchcode='" + cmb.Items[j].Value + "'";
                    DataTable dt1 = new DataTable();
                    dt1 = view2.ToTable();

                    strHtml += "<tr style=\"background-color:lavender;\">";
                    strHtml += "<td align=\"left\" colspan=10>Branch Name :" + cmb.Items[j].Text.ToString().Trim() + " [" + cmb.Items[j].Value + "]</td>";
                    strHtml += "</tr>";

                    DataRow row = dtExport.NewRow();
                    row["Client Name"] = " Branch Name:" + cmb.Items[j].Text.ToString().Trim() + " [" + cmb.Items[j].Value + "]";
                    row["Code"] = "Test";
                    dtExport.Rows.Add(row);

                    for (int k = 0; k < dt1.Rows.Count; k++)
                    {
                        flag = flag + 1;
                        strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                        DataRow row1 = dtExport.NewRow();

                        if (dt1.Rows[k]["Name"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" colspan=2>" + dt1.Rows[k]["Name"].ToString() + "</td>";
                            row1[0] = dt1.Rows[k]["Name"].ToString().ToString();
                        }
                        else
                            strHtml += "<td align=\"left\" >&nbsp;</td>";

                        if (dt1.Rows[k]["code"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >" + dt1.Rows[k]["code"].ToString() + "</td>";
                            row1[1] = dt1.Rows[k]["code"].ToString();
                        }
                        else
                            strHtml += "<td align=\"right\" >&nbsp;</td>";

                        if (dt1.Rows[k]["mtm"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt1.Rows[k]["mtm"].ToString())) + "</td>";
                            row1[2] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[k]["mtm"].ToString()));
                        }
                        else
                            strHtml += "<td align=\"right\" >&nbsp;</td>";

                        if (dt1.Rows[k]["trancharge"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt1.Rows[k]["trancharge"].ToString())) + "</td>";
                            row1[3] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[k]["trancharge"].ToString()));
                        }
                        else
                            strHtml += "<td align=\"right\" >&nbsp;</td>";

                        if (dt1.Rows[k]["othercharge"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt1.Rows[k]["othercharge"].ToString())) + "</td>";
                            row1[4] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[k]["othercharge"].ToString()));
                        }
                        else
                            strHtml += "<td align=\"right\" >&nbsp;</td>";

                        if (dt1.Rows[k]["srvtaxcess"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt1.Rows[k]["srvtaxcess"].ToString())) + "</td>";
                            row1[5] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[k]["srvtaxcess"].ToString()));
                        }
                        else
                            strHtml += "<td align=\"right\" >&nbsp;</td>";

                        if (dt1.Rows[k]["StampDuty"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt1.Rows[k]["StampDuty"].ToString())) + "</td>";
                            row1[6] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[k]["StampDuty"].ToString()));
                        }
                        else
                            strHtml += "<td align=\"right\" >&nbsp;</td>";

                        if (dt1.Rows[k]["NetDr"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt1.Rows[k]["NetDr"].ToString())) + "</td>";
                            row1[7] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[k]["NetDr"].ToString()));
                        }
                        else
                            strHtml += "<td align=\"right\" >&nbsp;</td>";

                        if (dt1.Rows[k]["NetCr"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt1.Rows[k]["NetCr"].ToString())) + "</td>";
                            row1[8] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[k]["NetCr"].ToString()));
                        }
                        else
                            strHtml += "<td align=\"right\" >&nbsp;</td>";

                        strHtml += "</tr>";
                        dtExport.Rows.Add(row1);
                    }

                    /////BRANCH TOTAL
                    view1.RowFilter = "branchcode='" + cmb.Items[j].Value + "'";
                    DataTable dt3 = new DataTable();
                    dt3 = view1.ToTable();

                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    strHtml += "<td align=\"left\" colspan=3><b>Branch Total :</b></td>";
                    DataRow row2 = dtExport.NewRow();
                    row2[0] = "Branch Total :";

                    if (dt3.Rows[0]["mtm"] != DBNull.Value)
                    {
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt3.Rows[0]["mtm"].ToString())) + "</td>";
                        row2[2] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt3.Rows[0]["mtm"].ToString()));
                    }
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";

                    if (dt3.Rows[0]["trancharge"] != DBNull.Value)
                    {
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt3.Rows[0]["trancharge"].ToString())) + "</td>";
                        row2[3] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt3.Rows[0]["trancharge"].ToString()));
                    }
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";

                    if (dt3.Rows[0]["othercharge"] != DBNull.Value)
                    {
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt3.Rows[0]["othercharge"].ToString())) + "</td>";
                        row2[4] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt3.Rows[0]["othercharge"].ToString()));
                    }
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";

                    if (dt3.Rows[0]["srvtaxcess"] != DBNull.Value)
                    {
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt3.Rows[0]["srvtaxcess"].ToString())) + "</td>";
                        row2[5] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt3.Rows[0]["srvtaxcess"].ToString()));
                    }
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";

                    if (dt3.Rows[0]["StampDuty"] != DBNull.Value)
                    {
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt3.Rows[0]["StampDuty"].ToString())) + "</td>";
                        row2[6] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt3.Rows[0]["StampDuty"].ToString()));
                    }
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";

                    if (dt3.Rows[0]["NetDr"] != DBNull.Value)
                    {
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt3.Rows[0]["NetDr"].ToString())) + "</td>";
                        row2[7] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt3.Rows[0]["NetDr"].ToString()));
                    }
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";

                    if (dt3.Rows[0]["NetCr"] != DBNull.Value)
                    {
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt3.Rows[0]["NetCr"].ToString())) + "</td>";
                        row2[8] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt3.Rows[0]["NetCr"].ToString()));
                    }
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";

                    strHtml += "</tr>";
                    dtExport.Rows.Add(row2);
                }

                //////////CLIENT TOTAL
                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                strHtml += "<td align=\"left\" colspan=3><b>Client Total :</b></td>";
                DataRow row3 = dtExport.NewRow();
                row3[0] = "Client Total :";

                if (ds.Tables[2].Rows[0]["summtm"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[2].Rows[0]["summtm"].ToString())) + "</td>";
                    row3[2] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[2].Rows[0]["summtm"].ToString()));
                }
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";

                if (ds.Tables[2].Rows[0]["sumtrancharge"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[2].Rows[0]["sumtrancharge"].ToString())) + "</td>";
                    row3[3] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[2].Rows[0]["sumtrancharge"].ToString()));
                }
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";

                if (ds.Tables[2].Rows[0]["sumothercharge"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[2].Rows[0]["sumothercharge"].ToString())) + "</td>";
                    row3[4] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[2].Rows[0]["sumothercharge"].ToString()));

                }
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";

                if (ds.Tables[2].Rows[0]["sumsrvtaxcess"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[2].Rows[0]["sumsrvtaxcess"].ToString())) + "</td>";
                    row3[5] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[2].Rows[0]["sumsrvtaxcess"].ToString()));

                }
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";

                if (ds.Tables[2].Rows[0]["StampDuty"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[2].Rows[0]["StampDuty"].ToString())) + "</td>";
                    row3[6] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[2].Rows[0]["StampDuty"].ToString()));

                }
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";

                if (ds.Tables[2].Rows[0]["NetDr"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[2].Rows[0]["NetDr"].ToString())) + "</td>";
                    row3[7] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[2].Rows[0]["NetDr"].ToString()));

                }
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";

                if (ds.Tables[2].Rows[0]["NetCr"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[2].Rows[0]["NetCr"].ToString())) + "</td>";
                    row3[8] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[2].Rows[0]["NetCr"].ToString()));

                }
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";

                strHtml += "</tr>";
                dtExport.Rows.Add(row3);
            }
            if (ds.Tables[3].Rows.Count > 0)
            {
                strHtml += "<tr style=\"background-color: white\" >";
                strHtml += "<td align=\"left\" colspan=3><b>Pro Account Obligation :</b></td>";
                DataRow row4 = dtExport.NewRow();
                row4[0] = "Pro Account Obligation ";

                if (ds.Tables[3].Rows[0]["summtmPro"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[3].Rows[0]["summtmPro"].ToString())) + "</td>";
                    row4[2] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[3].Rows[0]["summtmPro"].ToString()));

                }
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";

                strHtml += "<td align=\"right\" colspan=4>&nbsp;</td>";

                if (ds.Tables[3].Rows[0]["NetDrPro"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[3].Rows[0]["NetDrPro"].ToString())) + "</td>";
                    row4[7] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[3].Rows[0]["NetDrPro"].ToString()));

                }
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";

                if (ds.Tables[3].Rows[0]["NetCrPro"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[3].Rows[0]["NetCrPro"].ToString())) + "</td>";
                    row4[8] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[3].Rows[0]["NetCrPro"].ToString()));

                }
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";

                strHtml += "</tr>";
                dtExport.Rows.Add(row4);
            }
            /////////Exchange Obligation
            if (ds.Tables[4].Rows[0]["Exchangeobligation"] != DBNull.Value)
            {
                DataRow row5 = dtExport.NewRow();
                row5[0] = "Exchange Obligation For The Day ";

                if (Convert.ToDecimal(ds.Tables[4].Rows[0]["Exchangeobligation"].ToString()) > 0)
                {
                    strHtml += "<tr ><td colspan=8 align=\"left\"><b>Exchange Obligation :</b></td><td align=\"right\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(ds.Tables[4].Rows[0]["EXCHANGEOBLIGATION"].ToString()))) + "</b></td><td></td></tr>";
                    row5[7] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[4].Rows[0]["Exchangeobligation"].ToString()));

                }
                else
                {
                    strHtml += "<tr ><td colspan=8 align=\"left\"><b>Exchange Obligation :</b></td><td></td><td align=\"right\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(ds.Tables[4].Rows[0]["EXCHANGEOBLIGATION"].ToString()))) + "</b></td></tr>";
                    row5[8] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[4].Rows[0]["Exchangeobligation"].ToString()));

                }
                dtExport.Rows.Add(row5);
            }
            /////////Brokerage Income
            if (ds.Tables[4].Rows[0]["BRKGINCOME"].ToString().Trim() != "0.000000")
            {
                DataRow row6 = dtExport.NewRow();
                row6[0] = "Brokerage Income ";
                strHtml += "<tr ><td colspan=8 align=\"left\">Brokerage Income :</td><td></td><td align=\"right\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(ds.Tables[4].Rows[0]["BRKGINCOME"].ToString()))) + "</b></td></tr>";
                row6[8] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[4].Rows[0]["BRKGINCOME"].ToString()));
                dtExport.Rows.Add(row6);
            }
            /////////Transaction Charges Collected
            if (ds.Tables[2].Rows[0]["sumtrancharge"].ToString().Trim() != "0.000000")
            {
                DataRow row7 = dtExport.NewRow();
                row7[0] = "Transaction Charges Collected ";
                strHtml += "<tr ><td colspan=8 align=\"left\">Transaction Charges Collected :</td><td></td><td align=\"right\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(ds.Tables[2].Rows[0]["sumtrancharge"].ToString()))) + "</b></td></tr>";
                row7[8] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[2].Rows[0]["sumtrancharge"].ToString()));
                dtExport.Rows.Add(row7);
            }
            /////////Other Charges Collected
            if (ds.Tables[2].Rows[0]["sumothercharge"].ToString().Trim() != "0.000000")
            {
                strHtml += "<tr ><td colspan=8 align=\"left\">Other Charges Collected :</td><td></td><td align=\"right\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(ds.Tables[2].Rows[0]["sumothercharge"].ToString()))) + "</b></td></tr>";
                DataRow row8 = dtExport.NewRow();
                row8[0] = "Other Charges Collected ";
                row8[8] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[2].Rows[0]["sumothercharge"].ToString()));
                dtExport.Rows.Add(row8);
            }
            /////////Stamp Duty Collected
            if (ds.Tables[2].Rows[0]["StampDuty"].ToString().Trim() != "0.000000")
            {
                strHtml += "<tr ><td colspan=8 align=\"left\">Stamp Duty Collected :</td><td></td><td align=\"right\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(ds.Tables[2].Rows[0]["StampDuty"].ToString()))) + "</b></td></tr>";
                DataRow row9 = dtExport.NewRow();
                row9[0] = "Stamp Duty Collected ";
                row9[8] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[2].Rows[0]["StampDuty"].ToString()));
                dtExport.Rows.Add(row9);
            }
            /////////Service Tax Payable
            if (ds.Tables[2].Rows[0]["sumsrvtaxcess"] != DBNull.Value)
            {
                strHtml += "<tr ><td colspan=8 align=\"left\">Service Tax Payable:</td><td></td><td align=\"right\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(ds.Tables[2].Rows[0]["sumsrvtaxcess"].ToString()))) + "</b></td></tr>";
                DataRow row10 = dtExport.NewRow();
                row10[0] = "Service Tax Payable ";
                row10[8] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[2].Rows[0]["sumsrvtaxcess"].ToString()));
                dtExport.Rows.Add(row10);
            }
            /////////Service Tax Unrecoverable
            if (ds.Tables[4].Rows[0]["UNRECOVER"] != DBNull.Value)
            {
                DataRow row11 = dtExport.NewRow();
                row11[0] = "Service Tax Unrecoverable";

                if (Convert.ToDecimal(ds.Tables[4].Rows[0]["UNRECOVER"].ToString()) < 0)
                {
                    strHtml += "<tr ><td colspan=8 align=\"left\">Service Tax Unrecoverable:</td><td align=\"right\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(ds.Tables[4].Rows[0]["UNRECOVER"].ToString()))) + "</b></td><td></td></tr>";
                    row11[7] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[4].Rows[0]["UNRECOVER"].ToString()));

                }
                else
                {
                    strHtml += "<tr ><td colspan=8 align=\"left\">Service Tax Unrecoverable:</td><td></td><td align=\"right\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(ds.Tables[4].Rows[0]["UNRECOVER"].ToString()))) + "</b></td></tr>";
                    row11[8] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[4].Rows[0]["UNRECOVER"].ToString()));

                }
                dtExport.Rows.Add(row11);
            }
            /////////Round-Off (Due To Trade Averaging) 
            if (ds.Tables[4].Rows[0]["TRADEAVG"].ToString().Trim() != "0.000000")
            {
                DataRow row12 = dtExport.NewRow();
                row12[0] = "Round-Off (Due To Trade Averaging)  ";
                if (Convert.ToDecimal(ds.Tables[4].Rows[0]["TRADEAVG"].ToString()) < 0)
                {
                    strHtml += "<tr ><td colspan=8 align=\"left\"><b>Round-Off (Due To Trade Averaging) :</b></td><td align=\"right\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(ds.Tables[4].Rows[0]["TRADEAVG"].ToString()))) + "</b></td><td></td></tr>";
                    row12[7] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[4].Rows[0]["TRADEAVG"].ToString()));

                }
                else
                {
                    strHtml += "<tr ><td colspan=8 align=\"left\"><b>Round-Off (Due To Trade Averaging) :</b></td><td></td><td align=\"right\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(ds.Tables[4].Rows[0]["TRADEAVG"].ToString()))) + "</b></td></tr>";
                    row12[8] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[4].Rows[0]["TRADEAVG"].ToString()));

                }
                dtExport.Rows.Add(row12);
            }
            strHtml += "<tr style=\"background-color: white\"><td colspan=8 align=\"left\"><b>Total :</b></td><td align=\"right\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(ds.Tables[4].Rows[0]["NETDR"].ToString()))) + "</b></td><td align=\"right\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(ds.Tables[4].Rows[0]["NETCR"].ToString()))) + "</b></td></tr>";
            DataRow row13 = dtExport.NewRow();
            row13[0] = "Total :";
            row13[7] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[4].Rows[0]["NETDR"].ToString()));
            row13[8] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[4].Rows[0]["NETCR"].ToString()));
            dtExport.Rows.Add(row13);

            strHtml += "<tr style=\"background-color: white\"><td colspan=8 align=\"left\"><b>Diff(If Any):</b></td><td align=\"right\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(ds.Tables[4].Rows[0]["DIFFDR"].ToString()))) + "</b></td><td align=\"right\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(ds.Tables[4].Rows[0]["DIFFCR"].ToString()))) + "</b></td></tr>";
            DataRow row14 = dtExport.NewRow();
            row14[0] = "Diff(If Any)";
            row14[7] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[4].Rows[0]["DIFFDR"].ToString()));
            row14[8] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[4].Rows[0]["DIFFCR"].ToString()));
            dtExport.Rows.Add(row14);

            display.InnerHtml = strHtml;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "display", "display();", true);
            ViewState["dtExport"] = dtExport;
        }

        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            EXPORT();
        }
        public static bool IsNumeric(object value)
        {
            double dbl;
            return double.TryParse(value.ToString(), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out dbl);

        }
        void EXPORT()
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

            string str = null;
            str = "Report Date " + oconverter.ArrangeDate2(dtdate.Value.ToString());

            DrRowR1[0] = "Settlement Trial:" + str;

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

            if (cmbExport.SelectedItem.Value == "E")
            {
                objExcel.ExportToExcelforExcel(dtExport, "Settlement Trial", "Total :", dtReportHeader, dtReportFooter);
            }
        }
    }
}