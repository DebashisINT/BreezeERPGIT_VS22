using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using CrystalDecisions.CrystalReports.Engine;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Report_CorporateActions : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        BusinessLogicLayer.Reports rep = new BusinessLogicLayer.Reports();
        ExcelFile objExcel = new ExcelFile();
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        ReportDocument ReportDocument = new ReportDocument();
        string[] connPath = ConfigurationManager.AppSettings["DBConnectionDefault"].Split(';');
        static string Client;
        static string Branch;
        static string Group;
        string data;
        DataSet ds = new DataSet();
        static string SubClients = "";
        DataTable dtEmail = new DataTable();
        DataTable Distinctgroup = new DataTable();
        DataTable dt = new DataTable();
        string TerminalID = null;
        string ScripName = null;
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
            Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");
            if (!IsPostBack)
            {
                date();
                Client = null;
                Branch = null;
                Group = null;

                //_____For performing operation without refreshing page___//

                String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
                String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
                //___________-end here___//
            }

        }

        void date()
        {
            dtFrom.EditFormatString = oconverter.GetDateFormat("Date");
            string[] idlist = oDBEngine.GetDate().GetDateTimeFormats();
            dtFrom.Value = Convert.ToDateTime(idlist[2]);

            dtTo.EditFormatString = oconverter.GetDateFormat("Date");
            string[] idlist1 = oDBEngine.GetDate().GetDateTimeFormats();
            dtTo.Value = Convert.ToDateTime(idlist1[2]);

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
            string[] cl = idlist[1].Split(',');
            for (int i = 0; i < cl.Length; i++)
            {
                if (idlist[0] != "Clients")
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
                else
                {
                    string[] val = cl[i].Split(';');
                    string[] AcVal = val[0].Split('-');
                    if (str == "")
                    {
                        if (idlist[0] == "EM")
                        {
                            str = AcVal[0];
                            str1 = "'" + AcVal[0] + "'" + ";" + val[1];

                        }
                        else
                        {
                            str = "'" + AcVal[0] + "'";
                            str1 = "'" + AcVal[0] + "'" + ";" + val[1];
                        }
                    }
                    else
                    {
                        if (idlist[0] == "EM")
                        {
                            str += "," + AcVal[0];
                            str1 += "," + "'" + AcVal[0] + "'" + ";" + val[1];
                        }
                        else
                        {
                            str += ",'" + AcVal[0] + "'";
                            str1 += "," + "'" + AcVal[0] + "'" + ";" + val[1];
                        }
                    }
                }
            }

            if (idlist[0] == "Clients")
            {
                Client = str;
                data = "Clients~" + str1;
            }

            else if (idlist[0] == "Group")
            {
                Group = str;
                data = "Group~" + str;
            }
            else if (idlist[0] == "Branch")
            {
                Branch = str;
                data = "Branch~" + str;
            }
            else if (idlist[0] == "EM")
            {
                SubClients = str;
            }
            else if (idlist[0] == "ScripsExchange")
            {
                data = "ScripsExchange~" + str;
            }

        }


        void procedure()
        {
            ds = rep.Report_CorporateAction(Convert.ToDateTime(dtFrom.Value).ToString("yyyy-MM-dd"), Convert.ToDateTime(dtTo.Value).ToString("yyyy-MM-dd"),
                ddlType.SelectedItem.Text, rdInstrumentAll.Checked ? "All" : HiddenField_Instrument.Value.ToString().Trim());
            ViewState["dataset"] = ds;
            ViewState["header"] = Convert.ToDateTime(dtFrom.Value).ToString("dd-MMM-yyyy") + " To " + Convert.ToDateTime(dtTo.Value).ToString("dd-MMM-yyyy");
        }

        protected void btn_show_Click(object sender, EventArgs e)
        {
            procedure();
            ds = (DataSet)ViewState["dataset"];
            if (ds.Tables[0].Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD", "NORECORD();", true);
            }
            else
            {
                html();
            }
        }
        void html()
        {
            ds = (DataSet)ViewState["dataset"];
            String strHtml = String.Empty;
            String strHtml1 = String.Empty;
            int colcount = ds.Tables[0].Columns.Count;
            DataView viewclient = new DataView();
            viewclient = ds.Tables[0].DefaultView;
            DataTable dt1 = new DataTable();
            dt1 = viewclient.ToTable();

            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

            strHtml += "<tr style=\"background-color: #FFFFFF;\">";
            strHtml += "<td align=\"center\" colspan=13><B>" + " Corporate Actions Report For : " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtTo.Value.ToString()) + "</B></td>";
            strHtml += "</tr>";
            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            strHtml += "<td align=\"center\" ><b>Security Name</b></td>";
            strHtml += "<td align=\"center\" ><b>Symbol-Series</b></td>";
            strHtml += "<td align=\"center\" ><b>ISIN</b></td>";
            strHtml += "<td align=\"center\" ><b>Face Value</b></td>";
            strHtml += "<td align=\"center\" ><b>Type</b></td>";
            strHtml += "<td align=\"center\" ><b>Div[Rate]</b></td>";
            strHtml += "<td align=\"center\" ><b>Div[Amount]</b></td>";
            strHtml += "<td align=\"center\" ><b>Ratio</b></td>";
            strHtml += "<td align=\"center\" ><b>Record Date</b></td>";
            strHtml += "<td align=\"center\"><b>Ex-Date</b></td>";
            strHtml += "<td align=\"center\" ><b>BCStart Date</b></td>";
            strHtml += "<td align=\"center\" ><b>BCEnd Date</b></td>";
            strHtml += "<td align=\"center\"><b>Remarks</b></td>";
            strHtml += "</tr>";
            int flag = 0;


            int i;
            if (dt1.Rows.Count > 0)
            {
                for (i = 0; i < dt1.Rows.Count; i++)
                {
                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    strHtml += "<td align=\"left\">" + dt1.Rows[i]["SecurityName"].ToString() + "</td>";
                    strHtml += "<td align=\"left\">" + dt1.Rows[i]["Symbol"].ToString() + " [" + dt1.Rows[i]["Series"].ToString() + "]" + "</td>";
                    strHtml += "<td align=\"left\">" + dt1.Rows[i]["Isin"].ToString() + " </td>";
                    strHtml += "<td align=\"left\">" + dt1.Rows[i]["FaceValue"].ToString() + " </td>";
                    strHtml += "<td align=\"left\">" + dt1.Rows[i]["Type"].ToString() + " </td>";
                    strHtml += "<td align=\"right\">" + dt1.Rows[i]["DivRate"].ToString() + " </td>";
                    strHtml += "<td align=\"right\">" + dt1.Rows[i]["DivAmout"].ToString() + " </td>";
                    strHtml += "<td align=\"right\">" + dt1.Rows[i]["Ratio"].ToString() + " </td>";
                    strHtml += "<td align=\"right\">" + dt1.Rows[i]["RecordDate"].ToString() + " </td>";
                    strHtml += "<td align=\"right\">" + dt1.Rows[i]["Exdate"].ToString() + " </td>";
                    strHtml += "<td align=\"right\">" + dt1.Rows[i]["BCStartDate"].ToString() + " </td>";
                    strHtml += "<td align=\"right\">" + dt1.Rows[i]["BCEndDate"].ToString() + " </td>";
                    strHtml += "<td align=\"right\">" + dt1.Rows[i]["Remarks"].ToString() + " </td>";
                }

                displayALL.InnerHtml = strHtml;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "linehtml", "line();", true);
            }
        }
        protected string GetRowColor(int i)
        {
            if (i++ % 2 == 0)
                return "#fff0f5";
            else
                return "White";
        }


        void export()
        {

            ds = (DataSet)ViewState["dataset"];
            DataTable dtExport1 = new DataTable();


            dtExport1.Columns.Add("Security Name", Type.GetType("System.String"));
            dtExport1.Columns.Add("Symbol[Series]", Type.GetType("System.String"));
            dtExport1.Columns.Add("ISIN", Type.GetType("System.String"));
            dtExport1.Columns.Add("Face Value", Type.GetType("System.String"));
            dtExport1.Columns.Add("Type", Type.GetType("System.String"));
            dtExport1.Columns.Add("Div[Rate]", Type.GetType("System.String"));
            dtExport1.Columns.Add("Div[Amount]", Type.GetType("System.String"));
            dtExport1.Columns.Add("Ratio", Type.GetType("System.String"));
            dtExport1.Columns.Add("Record Date", Type.GetType("System.String"));
            dtExport1.Columns.Add("Ex-Date", Type.GetType("System.String"));
            dtExport1.Columns.Add("BCStart date", Type.GetType("System.String"));
            dtExport1.Columns.Add("BCEnd Date", Type.GetType("System.String"));
            dtExport1.Columns.Add("Remarks", Type.GetType("System.String"));


            if (ds.Tables[0].Rows.Count > 0)
            {

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    DataRow row = dtExport1.NewRow();
                    if (ds.Tables[0].Rows[i]["SecurityName"] != DBNull.Value)
                    {
                        row["Security Name"] = ds.Tables[0].Rows[i]["SecurityName"].ToString();
                    }


                    if (ds.Tables[0].Rows[i]["Symbol"] != DBNull.Value)
                    {
                        row["Symbol[Series]"] = ds.Tables[0].Rows[i]["Symbol"].ToString() + " [" + ds.Tables[0].Rows[i]["Series"].ToString() + "]";
                    }

                    if (ds.Tables[0].Rows[i]["Isin"] != DBNull.Value)
                    {
                        row["ISIN"] = ds.Tables[0].Rows[i]["Isin"].ToString();
                    }

                    if (ds.Tables[0].Rows[i]["FaceValue"] != DBNull.Value)
                    {
                        row["Face Value"] = ds.Tables[0].Rows[i]["FaceValue"].ToString();
                    }

                    if (ds.Tables[0].Rows[i]["Type"] != DBNull.Value)
                    {
                        row["Type"] = ds.Tables[0].Rows[i]["Type"].ToString();
                    }

                    if (ds.Tables[0].Rows[i]["DivRate"] != DBNull.Value)
                    {
                        row["Div[Rate]"] = ds.Tables[0].Rows[i]["DivRate"].ToString();
                    }

                    if (ds.Tables[0].Rows[i]["DivAmout"] != DBNull.Value)
                    {
                        row["Div[Amount]"] = ds.Tables[0].Rows[i]["DivAmout"].ToString();
                    }

                    if (ds.Tables[0].Rows[i]["Ratio"] != DBNull.Value)
                    {
                        row["Ratio"] = ds.Tables[0].Rows[i]["Ratio"].ToString();
                    }

                    if (ds.Tables[0].Rows[i]["RecordDate"] != DBNull.Value)
                    {
                        row["Record Date"] = ds.Tables[0].Rows[i]["RecordDate"].ToString();
                    }

                    if (ds.Tables[0].Rows[i]["Exdate"] != DBNull.Value)
                    {
                        row["Ex-Date"] = ds.Tables[0].Rows[i]["Exdate"].ToString();
                    }
                    if (ds.Tables[0].Rows[i]["BCStartDate"] != DBNull.Value)
                    {
                        row["BCStart Date"] = ds.Tables[0].Rows[i]["BCStartDate"].ToString();
                    }

                    if (ds.Tables[0].Rows[i]["BCEndDate"] != DBNull.Value)
                    {
                        row["BCEnd Date"] = ds.Tables[0].Rows[i]["BCEndDate"].ToString();
                    }

                    if (ds.Tables[0].Rows[i]["Remarks"] != DBNull.Value)
                    {
                        row["Remarks"] = ds.Tables[0].Rows[i]["Remarks"].ToString();
                    }

                    dtExport1.Rows.Add(row);

                }

                DataTable dtReportHeader1 = new DataTable();
                DataTable dtReportFooter1 = new DataTable();

                DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
                dtReportHeader1.Columns.Add(new DataColumn("Header", typeof(String))); //0

                DataRow HeaderRow = dtReportHeader1.NewRow();
                HeaderRow[0] = CompanyName.Rows[0][0].ToString();
                dtReportHeader1.Rows.Add(HeaderRow);
                DataRow DrRowR1 = dtReportHeader1.NewRow();
                string str = null;
                str = " Corporate Actions Report For the Period Of : " + ViewState["header"];
                DrRowR1[0] = str;
                dtReportHeader1.Rows.Add(DrRowR1);

                dtReportFooter1.Columns.Add(new DataColumn("Footer", typeof(String)));
                DataRow FooterRow1 = dtReportFooter1.NewRow();
                dtReportFooter1.Rows.Add(FooterRow1);
                DataRow FooterRow2 = dtReportFooter1.NewRow();
                dtReportFooter1.Rows.Add(FooterRow2);
                DataRow FooterRow = dtReportFooter1.NewRow();
                FooterRow[0] = "* * *  End Of Report * * *   ";
                dtReportFooter1.Rows.Add(FooterRow);
                objExcel.ExportToExcelforExcel(dtExport1, "Corporate Actions Report", "CAR", dtReportHeader1, dtReportFooter1);

            }

        }
        protected void ddlExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlExport.SelectedItem.Value == "E")
            {
                export();
            }

        }
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            procedure();
            ds = (DataSet)ViewState["dataset"];
            if (ds.Tables[0].Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD1", "NORECORD();", true);
            }
            else
            {
                export();
            }
        }
    }

}