using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_SRVTAXSTATEMENTDP : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        //DBEngine oDBEngine = new DBEngine(string.Empty);
        DataSet ds = new DataSet();
        ExcelFile objExcel = new ExcelFile();
        string data;
        DataTable dtExport = new DataTable();
        DailyReports dailyreport = new DailyReports();

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
                date();

            }
            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
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
                    str = val[0];
                    str1 = val[0] + ";" + val[1];
                }
                else
                {
                    str += "," + val[0];
                    str1 += "," + val[0] + ";" + val[1];
                }

            }
            if (idlist[0] == "Segment")
            {
                data = "Segment~" + str1 + "~" + str;

                string[] NoItems = str1.Split(',');
                string val = "";
                string[] items;
                for (int i = 0; i < NoItems.GetLength(0); i++)
                {
                    items = NoItems[i].Split(';');
                    if (val == "")
                    {
                        val = items[1];
                    }
                    else
                    {
                        val += "," + items[1];
                    }
                }
                ViewState["SegmentName"] = val;

            }

            else if (idlist[0] == "Branch")
            {

                data = "Branch~" + str1 + "~" + str;
            }

        }
        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }
        void date()
        {
            dtfrom.EditFormatString = oconverter.GetDateFormat("Date");
            dtto.EditFormatString = oconverter.GetDateFormat("Date");
            string first = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-1).ToString();
            string last = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddDays(-1).ToString();
            dtfrom.Value = Convert.ToDateTime(first);
            dtto.Value = Convert.ToDateTime(last);

            DataTable DtSegComp = oDBEngine.GetDataTable("TBL_MASTER_COMPANYEXCHANGE", "EXCH_MEMBERSHIPTYPE,EXCH_INTERNALID", "EXCH_TMCODE='" + Session["usersegid"].ToString().Trim() + "' AND EXCH_COMPID='" + Session["LastCompany"].ToString() + "'");
            if (DtSegComp.Rows.Count > 0)
            {
                litSegmentMain.InnerText = "[" + DtSegComp.Rows[0][0].ToString() + "]"; ///Segment disply within braket
                ViewState["SegmentName"] = DtSegComp.Rows[0][0].ToString();
                HiddenField_Segment.Value = DtSegComp.Rows[0][1].ToString();
            }

        }
        void fn_Segment()
        {

            if (rdbSegmentAll.Checked == true)
            {
                DataTable DtSegComp = oDBEngine.GetDataTable("TBL_MASTER_COMPANYEXCHANGE", "EXCH_MEMBERSHIPTYPE,EXCH_TMCODE", "EXCH_EXCHID IS NULL AND EXCH_COMPID='" + Session["LastCompany"].ToString() + "'");
                if (DtSegComp.Rows.Count > 0)
                {
                    ViewState["SegmentName"] = null;
                    HiddenField_Segment.Value = "";
                    for (int i = 0; i < DtSegComp.Rows.Count; i++)
                    {
                        if (HiddenField_Segment.Value.ToString().Trim() == "")
                        {
                            HiddenField_Segment.Value = "'" + DtSegComp.Rows[i][1].ToString() + "'";
                            ViewState["SegmentName"] = DtSegComp.Rows[i][0].ToString();
                        }
                        else
                        {
                            HiddenField_Segment.Value += "," + "'" + DtSegComp.Rows[i][1].ToString() + "'";
                            ViewState["SegmentName"] = ViewState["SegmentName"].ToString().Trim() + ',' + DtSegComp.Rows[i][0].ToString();
                        }
                    }

                }

            }
            else
            {
                DataTable DtSegComp = oDBEngine.GetDataTable("TBL_MASTER_COMPANYEXCHANGE", "EXCH_MEMBERSHIPTYPE,EXCH_TMCODE", "EXCH_INTERNALID in(" + HiddenField_Segment.Value.ToString().Trim() + ") AND EXCH_EXCHID IS NULL AND EXCH_COMPID='" + Session["LastCompany"].ToString() + "'");
                if (DtSegComp.Rows.Count > 0)
                {
                    ViewState["SegmentName"] = null;
                    HiddenField_Segment.Value = "";
                    for (int i = 0; i < DtSegComp.Rows.Count; i++)
                    {
                        if (HiddenField_Segment.Value.ToString().Trim() == "")
                        {
                            HiddenField_Segment.Value = "'" + DtSegComp.Rows[i][1].ToString() + "'";
                            ViewState["SegmentName"] = DtSegComp.Rows[i][0].ToString();
                        }
                        else
                        {
                            HiddenField_Segment.Value += "," + "'" + DtSegComp.Rows[i][1].ToString() + "'";
                            ViewState["SegmentName"] = ViewState["SegmentName"].ToString().Trim() + ',' + DtSegComp.Rows[i][0].ToString();
                        }
                    }

                }
            }

        }

        protected void btn_show_Click(object sender, EventArgs e)
        {
            fn_Segment();
            procedure();
        }
        void procedure()
        {
            string userbranchHierarchy;



            if (rdbBrtanchAll.Checked)
            {
                userbranchHierarchy = Session["userbranchHierarchy"].ToString();
            }
            else
            {
                userbranchHierarchy = HiddenField_Branch.Value.ToString().Trim();
            }

            ds = dailyreport.Sp_SRVTAXSTATEMENT_DP(dtfrom.Value.ToString(), dtto.Value.ToString(), HiddenField_Segment.Value.ToString().Trim(),
                Session["LastCompany"].ToString(), cmbreporttype.SelectedItem.Value, userbranchHierarchy);

            ViewState["dataset"] = ds;
            if (ds.Tables[0].Rows.Count != 0 || ds.Tables[2].Rows.Count != 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "display", "Display();", true);
                fnhtml();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD", "NORECORD(1);", true);
            }


        }
        protected string GetRowColor(int i)
        {
            if (i++ % 2 == 0)
                return "#fff0f5";
            else
                return "White";
        }
        void fnhtml()
        {
            ds = (DataSet)ViewState["dataset"];
            String strHtml = String.Empty;
            int flag;
            decimal NetEarning = decimal.Zero;
            decimal ServiceTax = decimal.Zero;
            decimal EduCess = decimal.Zero;
            decimal HgrEduCess = decimal.Zero;
            decimal TotalServTax = decimal.Zero;


            int IDCHK = 0;
            if (ds.Tables[0].Rows.Count != 0)
            {
                dtExport = new DataTable();
                dtExport.Clear();
                if (cmbreporttype.SelectedItem.Text.ToString().Trim() == "Date Wise")
                {
                    dtExport.Columns.Add("Date", Type.GetType("System.String"));
                }
                else
                {
                    dtExport.Columns.Add("Month", Type.GetType("System.String"));
                }
                dtExport.Columns.Add("Net Earning", Type.GetType("System.String"));
                dtExport.Columns.Add("Service Tax", Type.GetType("System.String"));
                dtExport.Columns.Add("Edu.Cess", Type.GetType("System.String"));
                dtExport.Columns.Add("Hgr.Edu.Cess", Type.GetType("System.String"));
                dtExport.Columns.Add("Total Serv.Tax", Type.GetType("System.String"));

                strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                strHtml += "<tr><td align=\"left\" colspan=6 style=\"color:Blue;\"><b>Period : " + oconverter.ArrangeDate2(dtfrom.Value.ToString()) + "-" + oconverter.ArrangeDate2(dtto.Value.ToString()) + "</b></td></tr>";

                strHtml += "<tr style=\"background-color: #DBEEF3;\">";
                if (cmbreporttype.SelectedItem.Text.ToString().Trim() == "Date Wise")
                {
                    strHtml += "<td align=\"center\">Date</td>";
                }
                else
                {
                    strHtml += "<td align=\"center\">Month</td>";
                }
                strHtml += "<td align=\"center\">Net Earning</td>";
                strHtml += "<td align=\"center\">Service Tax</td>";
                strHtml += "<td align=\"center\">Edu.Cess</td>";
                strHtml += "<td align=\"center\">Hgr.Edu.Cess</td>";
                strHtml += "<td align=\"center\">Total Serv.Tax</td></tr>";


                flag = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow row1 = dtExport.NewRow();
                    flag = flag + 1;
                    IDCHK = IDCHK + 1;
                    strHtml += "<tr id=\"tr_id" + IDCHK + "&" + GetRowColor(IDCHK) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    if (ds.Tables[0].Rows[i]["TradeDate"] != DBNull.Value)
                    {
                        strHtml += "<td align=\"left\">" + ds.Tables[0].Rows[i]["TradeDate"].ToString() + "</td>";
                        row1[0] = ds.Tables[0].Rows[i]["TradeDate"].ToString();
                    }
                    else
                    {
                        strHtml += "<td>&nbsp;</td>";
                    }

                    if (ds.Tables[0].Rows[i]["NETEARNING"] != DBNull.Value)
                    {
                        strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["NETEARNING"].ToString())) + "</td>";
                        NetEarning = NetEarning + Convert.ToDecimal(ds.Tables[0].Rows[i]["NETEARNING"].ToString());
                        row1["Net Earning"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[i]["NETEARNING"].ToString()));
                    }
                    else
                    {
                        strHtml += "<td>&nbsp;</td>";
                    }
                    if (ds.Tables[0].Rows[i]["ServiceTaxOnBrkg"] != DBNull.Value)
                    {
                        strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["ServiceTaxOnBrkg"].ToString())) + "</td>";
                        ServiceTax = ServiceTax + Convert.ToDecimal(ds.Tables[0].Rows[i]["ServiceTaxOnBrkg"].ToString());
                        row1["Service Tax"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[i]["ServiceTaxOnBrkg"].ToString()));

                    }
                    else
                    {
                        strHtml += "<td>&nbsp;</td>";
                    }
                    if (ds.Tables[0].Rows[i]["EduCessOnBrkg"] != DBNull.Value)
                    {
                        strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["EduCessOnBrkg"].ToString())) + "</td>";
                        EduCess = EduCess + Convert.ToDecimal(ds.Tables[0].Rows[i]["EduCessOnBrkg"].ToString());
                        row1["Edu.Cess"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[i]["EduCessOnBrkg"].ToString()));

                    }
                    else
                    {
                        strHtml += "<td>&nbsp;</td>";
                    }
                    if (ds.Tables[0].Rows[i]["HgrEduCessOnBrkg"] != DBNull.Value)
                    {
                        strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["HgrEduCessOnBrkg"].ToString())) + "</td>";
                        HgrEduCess = HgrEduCess + Convert.ToDecimal(ds.Tables[0].Rows[i]["HgrEduCessOnBrkg"].ToString());
                        row1["Hgr.Edu.Cess"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[i]["HgrEduCessOnBrkg"].ToString()));

                    }
                    else
                    {
                        strHtml += "<td>&nbsp;</td>";
                    }
                    if (ds.Tables[0].Rows[i]["TotalSrvTaxBrkg"] != DBNull.Value)
                    {
                        strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["TotalSrvTaxBrkg"].ToString())) + "</td>";
                        TotalServTax = TotalServTax + Convert.ToDecimal(ds.Tables[0].Rows[i]["TotalSrvTaxBrkg"].ToString());
                        row1["Total Serv.Tax"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[i]["TotalSrvTaxBrkg"].ToString()));

                    }
                    else
                    {
                        strHtml += "<td>&nbsp;</td>";
                    }
                    strHtml += "</tr>";
                    dtExport.Rows.Add(row1);
                }

                strHtml += "<tr><td align=\"left\"  style=\"color:Maroon;\"><b>Total:</b></td>";
                strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(NetEarning)) + "</td>";
                strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ServiceTax)) + "</td>";
                strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(EduCess)) + "</td>";
                strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(HgrEduCess)) + "</td>";
                strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(TotalServTax)) + "</td>";
                strHtml += "</tr>";


                strHtml += "</table>";
                display.InnerHtml = strHtml;
                ViewState["dtExport"] = dtExport;
            }
        }
        void EXPORT()
        {
            dtExport = (DataTable)ViewState["dtExport"];
            DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            DataTable dtReportHeader = new DataTable();
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            HeaderRow[0] = CompanyName.Rows[0][0].ToString();
            dtReportHeader.Rows.Add(HeaderRow);
            DataRow DrRowR1 = dtReportHeader.NewRow();

            DrRowR1[0] = "Service Tax Statement:Segment [" + ViewState["SegmentName"].ToString().Trim() + "] Period " + ' ' + oconverter.ArrangeDate2(dtfrom.Value.ToString()) + '-' + oconverter.ArrangeDate2(dtto.Value.ToString());

            dtReportHeader.Rows.Add(DrRowR1);
            DataRow DrRowR2 = dtReportHeader.NewRow();
            //DrRowR2[0] = txtBankName.Text;
            dtReportHeader.Rows.Add(DrRowR2);
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
                objExcel.ExportToExcelforExcel(dtExport, "Service Tax Statement", "Total", dtReportHeader, dtReportFooter);
            }
            else if (ddlExport.SelectedItem.Value == "P")
            {
                objExcel.ExportToPDF(dtExport, "Service Tax Statement", "Total", dtReportHeader, dtReportFooter);
            }

        }
        protected void ddlExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            EXPORT();
        }
    }
}