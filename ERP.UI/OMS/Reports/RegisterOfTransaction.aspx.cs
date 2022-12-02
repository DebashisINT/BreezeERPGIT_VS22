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


    public partial class Reports_RegisterOfTransaction : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        StatutoryReports sr = new StatutoryReports();
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        DataSet ds = new DataSet();
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
                DateBind();

            }
            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //___________-end here___//

        }
        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
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
                if (idlist[0].ToString().Trim() == "Clients")
                {
                    string[] val = cl[i].Split(';');
                    string[] AcVal = val[0].Split('-');
                    if (str == "")
                    {

                        str = "'" + AcVal[0] + "'";
                        str1 = "'" + AcVal[0] + "'" + ";" + val[1];
                    }
                    else
                    {

                        str += ",'" + AcVal[0] + "'";
                        str1 += "," + "'" + AcVal[0] + "'" + ";" + val[1];
                    }
                }
                else
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

            }

            if (idlist[0] == "Clients")
            {
                data = "Clients~" + str;
            }
            else if (idlist[0] == "ScripsExchange")
            {
                data = "ScripsExchange~" + str;
            }

        }
        void DateBind()
        {
            DtFrom.EditFormatString = oconverter.GetDateFormat("Date");
            DtTo.EditFormatString = oconverter.GetDateFormat("Date");

            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "1" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "4" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "15" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "19")
            {
                string[,] DtStartEnddate = oDBEngine.GetFieldValue("Master_Settlements", "distinct cast(Settlements_StartDateTime as varchar)+','+cast(Settlements_FundsPayin as varchar)+','+cast(Settlements_EndDateTime as varchar)", "settlements_ExchangeSegmentId='" + HttpContext.Current.Session["ExchangeSegmentID"].ToString() + "' and settlements_number='" + Session["LastSettNo"].ToString().Substring(0, 7) + "' and settlements_TYPESUFFIX='" + Session["LastSettNo"].ToString().Substring(7, 1) + "' and settlements_finyear='" + Session["LastFinYear"].ToString().Trim() + "'", 1);
                if (DtStartEnddate[0, 0] != "n")
                {
                    string[] idlist = DtStartEnddate[0, 0].ToString().Split(','); // fetch startdate and FundsPayin and End 
                    DtFrom.Value = Convert.ToDateTime(idlist[0]);
                    DtTo.Value = Convert.ToDateTime(idlist[0]);
                }
            }
            else
            {
                DtFrom.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
                DtTo.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
            }
        }
        void Procedure()
        {

            string Time = null;
            if (Session["ExchangeSegmentID"].ToString().Trim() == "1" || Session["ExchangeSegmentID"].ToString().Trim() == "4" || Session["ExchangeSegmentID"].ToString().Trim() == "5" || Session["ExchangeSegmentID"].ToString().Trim() == "15" || Session["ExchangeSegmentID"].ToString().Trim() == "2" || Session["ExchangeSegmentID"].ToString().Trim() == "19" || Session["ExchangeSegmentID"].ToString().Trim() == "20")
            {
                Time = "(CONVERT(VARCHAR(8), EXCHANGETRADES_TRADEENTRYTIME, 108) BETWEEN " + "'" + txtfromtime.Value.ToString().Trim() + "'" + " AND " + "'" + txttotime.Value.ToString().Trim() + "' OR EXCHANGETRADES_TRADEENTRYTIME IS NULL)";
            }
            else
            {
                Time = "(CONVERT(VARCHAR(8), ComEXCHANGETRADES_TRADEENTRYTIME, 108) BETWEEN " + "'" + txtfromtime.Value.ToString().Trim() + "'" + " AND " + "'" + txttotime.Value.ToString().Trim() + "' OR ComEXCHANGETRADES_TRADEENTRYTIME IS NULL)";
            }

            ds = sr.Report_RegisterOfTransaction(DdlRptType.SelectedItem.Value.ToString().Trim(), Session["LastCompany"].ToString().Trim(),
                HttpContext.Current.Session["usersegid"].ToString().Trim(), DtFrom.Value.ToString(), DtTo.Value.ToString(), Session["LastFinYear"].ToString().Trim(),
                rdbScripsAll.Checked ? "ALL" : HiddenField_ScripsExchange.Value, rdbClientALL.Checked ? "ALL" : HiddenField_Client.Value, Session["ExchangeSegmentID"].ToString().Trim(),
                Time.ToString().Trim(), DdlPanDetails.SelectedItem.Value.ToString().Trim(), Session["userbranchHierarchy"].ToString().Trim(),
                ChkBannedEntity.Checked.ToString().Trim(), DdlSecurityType.SelectedItem.Value.ToString().Trim());

            FnCall(ds);
        }
        void FnCall(DataSet ds)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ChkBannedEntity.Checked)
                {
                    FnHtml(ds);
                }
                else
                {
                    Print(ds);
                }


            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "fnNoRecord", "fnAlert('1');", true);
            }
        }
        void Print(DataSet ds)
        {

            byte[] logoinByte;
            ds.Tables[1].Columns.Add("Image", System.Type.GetType("System.Byte[]"));
            if (ChkLogoPrint.Checked == false)
            {
                if (oconverter.getLogoImage(HttpContext.Current.Server.MapPath(@"..\images\logo.bmp"), out logoinByte) != 1)
                {
                    ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "logo", "alert('Logo not Found.');", true);
                }
                else
                {
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        ds.Tables[1].Rows[i]["Image"] = logoinByte;

                    }
                }
            }
            //ds.Tables[0].WriteXmlSchema("D:\\RegisterOfTransaction.xsd");
            //ds.Tables[1].WriteXmlSchema("D:\\RegisterOfTransactionCompDetails.xsd");
            ReportDocument report = new ReportDocument();
            //report.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;

            string tmpPdfPath = string.Empty;
            tmpPdfPath = HttpContext.Current.Server.MapPath("..\\Reports\\RegisterOfTransaction.rpt");
            report.Load(tmpPdfPath);
            report.SetDataSource(ds.Tables[0]);
            report.Subreports["compdetail"].SetDataSource(ds.Tables[1]);
            report.VerifyDatabase();


            string ExchSegmnet = null;
            if (Session["ExchangeSegmentID"].ToString().Trim() == "1" || Session["ExchangeSegmentID"].ToString().Trim() == "15" || Session["ExchangeSegmentID"].ToString().Trim() == "4" || Session["ExchangeSegmentID"].ToString().Trim() == "19")
            {
                ExchSegmnet = "Sett No";
            }
            else
            {
                ExchSegmnet = "Category";
            }
            report.SetParameterValue("@ExchangeSegmnet", (object)ExchSegmnet.ToString().Trim());

            if (ddlExport.SelectedItem.Value.ToString().Trim() == "PDF")
            {
                report.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "Register Of Transaction");
            }
            if (ddlExport.SelectedItem.Value.ToString().Trim() == "Excel")
            {
                DataTable dtexcel;
                dtexcel = ds.Tables[0].Copy();

                dtexcel.Columns["Display_1"].ColumnName = "Trade Date";
                dtexcel.Columns["Display_2"].ColumnName = "Trade Time";
                dtexcel.Columns["Display_3"].ColumnName = "Client Name";
                dtexcel.Columns["Display_4"].ColumnName = "Pancard";
                dtexcel.Columns["Display_5"].ColumnName = "Trade code";
                dtexcel.Columns["Display_6"].ColumnName = "Share Name";
                if (Session["ExchangeSegmentID"].ToString().Trim() == "1" || Session["ExchangeSegmentID"].ToString().Trim() == "15" || Session["ExchangeSegmentID"].ToString().Trim() == "4" || Session["ExchangeSegmentID"].ToString().Trim() == "19")
                {
                    dtexcel.Columns["Display_7"].ColumnName = "Sett No";
                }
                else
                {
                    dtexcel.Columns["Display_7"].ColumnName = "Category";
                }
                dtexcel.Columns["OrderNo"].ColumnName = "Order No";
                dtexcel.Columns["OrderEntryTime"].ColumnName = "Order Entry Time";
                dtexcel.Columns["Terminalctclid"].ColumnName = "Terminal/Ctcl";

                dtexcel.AcceptChanges();
                if (dtexcel.Rows.Count > 0)
                {
                    GenericExcelExport oGenericExcelExport = new GenericExcelExport();
                    string strDownloadFileName = "";
                    string exlDateTime = oDBEngine.GetDate(113).ToString();
                    string exlTime = exlDateTime.Replace(":", "");
                    exlTime = exlTime.Replace(" ", "");
                    string FileName = "Register Of Transaction_" + exlTime;
                    strDownloadFileName = "~/Documents/";
                    string[] header = new string[5];
                    string companyname = null;
                    string compadd = null;
                    string compemail = null;
                    string compfax = null;
                    string comppan = null;
                    string compsrvtax = null;
                    string compsegment = null;
                    string compperiod = null;
                    string ExcelVersion = "2007";
                    companyname = ds.Tables[1].Rows[0]["CompanyName"].ToString();
                    compadd = ds.Tables[1].Rows[0]["CmpAddress"].ToString();
                    compemail = ds.Tables[1].Rows[0]["CmpEmail"].ToString();
                    compfax = ds.Tables[1].Rows[0]["Cmpfax"].ToString();
                    comppan = ds.Tables[1].Rows[0]["CmpPanNo"].ToString();
                    compsrvtax = ds.Tables[1].Rows[0]["CmpServiceTax"].ToString();
                    compsegment = ds.Tables[1].Rows[0]["SegmentName"].ToString();
                    compperiod = ds.Tables[1].Rows[0]["Period"].ToString();
                    header[0] = compsegment + "       " + compperiod;
                    header[1] = "Pan    " + comppan + "       " + "SrvTax Reg.No    " + compsrvtax;
                    header[2] = "Email  " + compemail + "       " + "Fax  " + compfax;
                    header[3] = compadd;
                    header[4] = companyname;
                    string[] ColumnType = { "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "N", "N", "N" };
                    string[] ColumnSize = { "12", "12", "50", "12", "15", "50", "16", "16", "20", "16", "16", "16", "20", "18,2", "18,2", "18,2" };
                    string[] ColumnWidthSize = { "12", "10", "30", "12", "10", "34", "14", "16", "14", "16", "14", "6", "8", "12", "12", "12" };
                    oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, dtexcel, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, header, null);
                }
                //report.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.Excel, HttpContext.Current.Response, true, "Register Of Transaction");
            }
            //report.Dispose();
            //GC.Collect();


        }
        protected void BtnExcel_Click(object sender, EventArgs e)
        {
            Procedure();
        }
        protected void BtnScreen_Click(object sender, EventArgs e)
        {
            Procedure();
        }
        void FnHtml(DataSet ds)
        {
            //////////For header
            String strHtmlheader = String.Empty;
            string str = null;

            str = "[Banned Entity] Period : " + oconverter.ArrangeDate2(DtFrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(DtTo.Value.ToString());
            str = str + " ; Report Type : " + DdlRptType.SelectedItem.Text.ToString().Trim();

            strHtmlheader = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtmlheader += "<tr><td align=\"left\" colspan=" + ds.Tables[0].Columns.Count + " style=\"color:Blue;\">" + str + "</td></tr></table>";


            ////////Detail Display
            String strHtml = String.Empty;
            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

            DataView viewrecord = new DataView();
            viewrecord = ds.Tables[0].DefaultView;
            DataTable dt = viewrecord.ToTable();

            dt.Columns[0].ColumnName = "Trade Date";
            dt.Columns[1].ColumnName = "Trade Time";
            dt.Columns[2].ColumnName = "Client Name";
            dt.Columns[3].ColumnName = "Pancard";
            dt.Columns[4].ColumnName = "Trade Code";
            dt.Columns[5].ColumnName = "Share Name";

            if (Session["ExchangeSegmentID"].ToString().Trim() == "1" || Session["ExchangeSegmentID"].ToString().Trim() == "15" || Session["ExchangeSegmentID"].ToString().Trim() == "4" || Session["ExchangeSegmentID"].ToString().Trim() == "19")
            {
                dt.Columns[6].ColumnName = "Sett No";
            }
            else
            {
                dt.Columns[6].ColumnName = "Category";
            }


            //////////////TABLE HEADER BIND
            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                strHtml += "<td align=\"center\" style=\"font-size:smaller;\" nowrap=nowrap;><b>" + dt.Columns[i].ColumnName + "</b></td>";
            }
            strHtml += "</tr>";

            int flag = 0;
            foreach (DataRow dr1 in dt.Rows)
            {
                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (dr1[j] != DBNull.Value)
                    {
                        if (IsNumeric(dr1[j].ToString()) == true)
                        {
                            strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                        }
                        else if (dt.Columns[j].ColumnName.ToString().Trim() == "Pancard")
                        {
                            strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\"><a href=javascript:void(0); onClick=javascript:FnPopUp('" + dr1[j].ToString().Trim() + "','BtnPanCard')>" + dr1[j] + "</a></td>";

                        }
                        else
                        {
                            strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

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

            ScriptManager.RegisterStartupScript(this, this.GetType(), "fnNoRecord", "fnAlert('2');", true);

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
        protected void BtnPanCard_Click(object sender, EventArgs e)
        {
            DataTable DtPan = oDBEngine.GetDataTable("Master_BannedEntity", "top 1 BannedEntity_Id", "BannedEntity_Pan='" + HiddenField_PanCard.Value.ToString().Trim() + "'");
            if (DtPan.Rows.Count > 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "fnNoRecord", "FnPopUp(" + DtPan.Rows[0][0].ToString().Trim() + ",'PopUp');", true);
            }
        }
    }
}