using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using DevExpress.Web;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_TradedClients : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        MasterReports mr = new MasterReports();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        DataSet ds = new DataSet();
        ExcelFile objExcel = new ExcelFile();
        DataTable dtExport = new DataTable();
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        AspxHelper oAspxHelper;
        string data;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
               // Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (!IsPostBack)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");
                if (!IsPostBack)
                {
                    dtfrom.EditFormatString = oconverter.GetDateFormat("Date");
                    dtto.EditFormatString = oconverter.GetDateFormat("Date");
                    dtfrom.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
                    dtto.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
                    Segment();
                    ClientType();
                }
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
            else if (idlist[0] == "Segment")
            {
                data = "Segment~" + str;
            }

        }

        void ClientType()
        {
            oAspxHelper = new AspxHelper();
            DataTable DtClntType = new DataTable();
            DtClntType = oDBEngine.GetDataTable("select distinct isnull(cnt_clienttype,'Retail')ClientType from tbl_master_contact");
            oAspxHelper.Bind_Combo(ComboClientType, DtClntType, "ClientType", "ClientType", 0);
            ComboClientType.Items.Insert(0, new ListEditItem("All", "0"));
            ComboClientType.SelectedIndex = 0;

        }

        void Segment()
        {
            HiddenField_Segment.Value = Session["usersegid"].ToString();

            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "1")
                litSegmentMain.InnerText = "NSE-CM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "2")
                litSegmentMain.InnerText = "NSE-FO";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "3")
                litSegmentMain.InnerText = "NSE-CDX";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "4")
                litSegmentMain.InnerText = "BSE-CM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "5")
                litSegmentMain.InnerText = "BSE-FO";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "6")
                litSegmentMain.InnerText = "BSE-CDX";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "7")
                litSegmentMain.InnerText = "MCX-COMM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "8")
                litSegmentMain.InnerText = "MCXSX-CDX";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "9")
                litSegmentMain.InnerText = "NCDEX-COMM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "10")
                litSegmentMain.InnerText = "DGCX-COMM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "11")
                litSegmentMain.InnerText = "NMCE-COMM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "12")
                litSegmentMain.InnerText = "ICEX-COMM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "13")
                litSegmentMain.InnerText = "USE-CDX";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "14")
                litSegmentMain.InnerText = "NSEL-SPOT";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "15")
                litSegmentMain.InnerText = "CSE-CM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "19")
                litSegmentMain.InnerText = "MCXSX-CM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "20")
                litSegmentMain.InnerText = "MCXSX-FO";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "18")
                litSegmentMain.InnerText = "INMX-COMM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "21")
                litSegmentMain.InnerText = "BFX-COMM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "22")
                litSegmentMain.InnerText = "INSX-COMM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "23")
                litSegmentMain.InnerText = "INFX-COMM";


        }
        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }
        void Procedure()
        {
            ds = mr.ClientsHavingTradesInAPeriod(Session["LastCompany"].ToString(), rdbSegmentAll.Checked ? "ALL" : HiddenField_Segment.Value,
                dtfrom.Value.ToString(), dtto.Value.ToString(), ddllist.SelectedItem.Value.ToString() == "0" ? "SHOW" : "PRINT",
                rdbClientALL.Checked ? "ALL" : HiddenField_Client.Value, Session["userbranchHierarchy"].ToString().Trim());
            FnCall(ds);
        }
        void htmltable(DataSet ds)
        {

            String strHtml = String.Empty;

            strHtml = "<table width=\"98%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            strHtml += "<td align=\"center\" ><b>Sr</b></td>";
            strHtml += "<td align=\"center\" ><b>Client Name</b></td>";
            strHtml += "<td align=\"center\" ><b>Branch Name</b></td>";
            strHtml += "<td align=\"center\" ><b>UCC</b></td>";
            strHtml += "<td align=\"center\" ><b>Trade <br /> Code</b></td>";
            strHtml += "<td align=\"center\" ><b>Segment</b></td>";
            strHtml += "<td align=\"center\" ><b>PAN</b></td>";
            strHtml += "<td align=\"center\" ><b>Category</b></td>";
            strHtml += "<td align=\"center\" ><b>Reg.Date</b></td>";
            strHtml += "<td align=\"center\" ><b>Address</b></td>";
            strHtml += "<td align=\"center\" ><b>Email</b></td>";
            strHtml += "<td align=\"center\"><b>Mobile</b></td>";
            strHtml += "<td align=\"center\" ><b>Bank Details</b></td>";
            strHtml += "<td align=\"center\"><b>DP Details</b></td>";
            strHtml += "</tr>";


            int flag = 0;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                flag = flag + 1;
                strHtml += "<tr valign=top id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                strHtml += "<td align=\"left\" >" + ds.Tables[0].Rows[i]["SrNo"].ToString() + "</td>";
                strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >" + ds.Tables[0].Rows[i]["ClientName"].ToString() + "</td>";
                strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >" + ds.Tables[0].Rows[i]["BranchName"].ToString() + "</td>";
                strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >" + ds.Tables[0].Rows[i]["TCode"].ToString() + "</td>";
                strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >" + ds.Tables[0].Rows[i]["TradeCodeCombine"].ToString() + "</td>";
                strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >" + ds.Tables[0].Rows[i]["Segment"].ToString() + "</td>";
                if (ds.Tables[0].Rows[i]["PanCard"] != DBNull.Value)
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >" + ds.Tables[0].Rows[i]["PanCard"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";
                if (ds.Tables[0].Rows[i]["ClientCategory"] != DBNull.Value)
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >" + ds.Tables[0].Rows[i]["ClientCategory"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";
                if (ds.Tables[0].Rows[i]["RegistrationDate"] != DBNull.Value)
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >" + ds.Tables[0].Rows[i]["RegistrationDate"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";
                strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >" + ds.Tables[0].Rows[i]["ADDRESS"].ToString() + "</td>";
                strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >" + ds.Tables[0].Rows[i]["Email"].ToString() + "</td>";
                strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >" + ds.Tables[0].Rows[i]["Phonenumber"].ToString() + "</td>";


                /////////BANK DETAILS
                DataView view1 = new DataView();
                view1 = ds.Tables[1].DefaultView;
                view1.RowFilter = "cbd_cntid='" + ds.Tables[0].Rows[i]["CLIENTID"].ToString().Trim() + "'";
                DataTable dt2 = new DataTable();
                dt2 = view1.ToTable();
                strHtml += "<td>";
                strHtml += "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

                for (int k = 0; k < dt2.Rows.Count; k++)
                {
                    strHtml += "<tr>";
                    if (dt2.Rows[k]["BANCKDETAILS"] != DBNull.Value)
                        strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >" + dt2.Rows[k]["BANCKDETAILS"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";
                    strHtml += "</tr>";
                }
                strHtml += "</table></td>";



                /////////DP DETAILS
                DataView view2 = new DataView();
                view2 = ds.Tables[2].DefaultView;
                view2.RowFilter = "CLIENTDP='" + ds.Tables[0].Rows[i]["CLIENTID"].ToString().Trim() + "'";
                DataTable dt1 = new DataTable();
                dt1 = view2.ToTable();
                strHtml += "<td>";
                strHtml += "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

                for (int k = 0; k < dt1.Rows.Count; k++)
                {
                    strHtml += "<tr>";
                    if (dt1.Rows[k]["DPDETAIL"] != DBNull.Value)
                        strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >" + dt1.Rows[k]["DPDETAIL"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";
                    strHtml += "</tr>";
                }
                strHtml += "</table></td>";


                strHtml += "</tr>";

            }
            strHtml += "</table>";


            display.InnerHtml = strHtml;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Display", "FnRecord('1');", true);

        }
        protected string GetRowColor(int i)
        {
            if (i++ % 2 == 0)
                return "#fff0f5";
            else
                return "White";
        }


        protected void btnshow_Click(object sender, EventArgs e)
        {
            Procedure();
        }
        void FnCall(DataSet ds)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ddllist.SelectedItem.Value.ToString().Trim() == "0")//Show
                {
                    htmltable(ds);
                }
                if (ddllist.SelectedItem.Value.ToString().Trim() == "1")//PDF
                {
                    Print(ds);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NoRecord", "FnRecord('3');", true);
            }
        }
        void Print(DataSet ds)
        {
            DataTable dtexcel;
            dtexcel = ds.Tables[0].Copy();
            if (dtexcel.Rows.Count > 0)
            {
                GenericExcelExport oGenericExcelExport = new GenericExcelExport();
                string strDownloadFileName = "";
                string exlDateTime = oDBEngine.GetDate(113).ToString();
                string exlTime = exlDateTime.Replace(":", "");
                exlTime = exlTime.Replace(" ", "");
                string FileName = "Client Having Trade_" + exlTime;
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
                string compBranch = null;
                string ExcelVersion = "2007";
                companyname = ds.Tables[0].Rows[0]["cmpname"].ToString();
                compadd = ds.Tables[0].Rows[0]["cmpaddress"].ToString();
                compemail = ds.Tables[0].Rows[0]["cmpemail"].ToString();
                compfax = ds.Tables[0].Rows[0]["cmpfax"].ToString();
                comppan = ds.Tables[0].Rows[0]["cmppanno"].ToString();
                compsrvtax = ds.Tables[0].Rows[0]["cmpservicetax"].ToString();
                compperiod = ds.Tables[0].Rows[0]["ReportType"].ToString();

                header[0] = compperiod;
                header[1] = "Pan    " + comppan + "       " + "SrvTax Reg.No    " + compsrvtax;
                header[2] = "Email  " + compemail + "       " + "Fax  " + compfax;
                header[3] = compadd;
                header[4] = companyname;
                dtexcel.Columns["BranchName"].ColumnName = "BranchName";
                dtexcel.Columns["TCode"].ColumnName = "UCC";
                dtexcel.Columns["TradeCodeCombine"].ColumnName = "Trade Code";
                dtexcel.Columns["PanCard"].ColumnName = "PAN";
                dtexcel.Columns["ClientCategory"].ColumnName = "Category";
                dtexcel.Columns["RegistrationDate"].ColumnName = "Reg.Date";
                dtexcel.Columns["bankdetails"].ColumnName = "Bank Details";
                dtexcel.Columns["dpdetails"].ColumnName = "DP Details";

                dtexcel.Columns.Remove("cmpname");
                dtexcel.Columns.Remove("cmpaddress");
                dtexcel.Columns.Remove("cmpemail");
                dtexcel.Columns.Remove("cmpfax");
                dtexcel.Columns.Remove("cmppanno");
                dtexcel.Columns.Remove("cmpservicetax");
                dtexcel.Columns.Remove("cmpphno");
                dtexcel.Columns.Remove("CLIENTID");
                dtexcel.Columns.Remove("ReportType");

                dtexcel.AcceptChanges();


                string[] ColumnType = { "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V" };
                string[] ColumnSize = { "5", "27", "27", "8", "10", "15", "15", "22", "13", "250", "250", "50", "120", "20" };
                string[] ColumnWidthSize = { "5", "27", "27", "8", "10", "15", "15", "22", "13", "80", "30", "50", "120", "22" };
                oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, dtexcel, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, header, null);
            }
        }
        protected void btnprint_Click(object sender, EventArgs e)
        {
            Procedure();

        }
    }
}