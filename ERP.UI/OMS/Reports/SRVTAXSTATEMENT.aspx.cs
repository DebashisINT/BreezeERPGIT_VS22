using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_SRVTAXSTATEMENT : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        DataSet ds = new DataSet();
        string data;
        ExcelFile objExcel = new ExcelFile();

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
                SegmentName();
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
            data = idlist[0] + "~" + str;


        }
        void SegmentName()
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


        }
        protected void BtnScreen_Click(object sender, EventArgs e)
        {
            SPCall();
        }
        protected void BtnExcel_Click(object sender, EventArgs e)
        {
            SPCall();
        }
        DataSet Procedure()
        {
            string[] InputName = new string[9];
            string[] InputType = new string[9];
            string[] InputValue = new string[9];



            /////////////////Parameter Name
            InputName[0] = "Segmentid";
            InputName[1] = "FromDate";
            InputName[2] = "ToDate";
            InputName[3] = "Companyid";
            InputName[4] = "Branch";
            InputName[5] = "RptType";
            InputName[6] = "Consider";
            InputName[7] = "AsPer";
            InputName[8] = "SegmentBreakUp";

            /////////////////Parameter Data Type
            InputType[0] = "V";
            InputType[1] = "V";
            InputType[2] = "V";
            InputType[3] = "V";
            InputType[4] = "V";
            InputType[5] = "V";
            InputType[6] = "V";
            InputType[7] = "V";
            InputType[8] = "V";


            /////////////////Parameter Value
            if (rdbSegmentAll.Checked)
                InputValue[0] = "ALL";
            else
                InputValue[0] = HiddenField_Segment.Value.ToString().Trim();
            InputValue[1] = DtFrom.Value.ToString().Trim();
            InputValue[2] = DtTo.Value.ToString().Trim();
            InputValue[3] = HttpContext.Current.Session["LastCompany"].ToString().Trim();
            if (rdBranch.Checked)
                InputValue[4] = HttpContext.Current.Session["userbranchHierarchy"].ToString().Trim();
            else
                InputValue[4] = HiddenField_Branch.Value.ToString().Trim();
            InputValue[5] = DdlReportType.SelectedItem.Value.ToString().Trim();
            InputValue[6] = DdlConsider.SelectedItem.Value.ToString().Trim();
            InputValue[7] = ddlAsPer.SelectedItem.Value.ToString().Trim();
            InputValue[8] = ChkSegmentBreakUp.Checked.ToString().Trim();
            ds = BusinessLogicLayer.SQLProcedures.SelectProcedureArrDS("[Sp_SRVTAXSTATEMENT]", InputName, InputType, InputValue);

            return ds;
        }
        void SPCall()
        {
            ds = Procedure();
            if (ds.Tables.Count > 0)
            {
                /////////////////Heading Bind/////////////////////
                string strheading = "  For The Period  : " + oconverter.ArrangeDate2(DtFrom.Value.ToString().Trim()) + " - " + oconverter.ArrangeDate2(DtTo.Value.ToString().Trim());
                strheading = strheading.ToString().Trim() + " ; Consider : " + DdlConsider.SelectedItem.Text.ToString().Trim();
                strheading = strheading.ToString().Trim() + " ; As Per : " + ddlAsPer.SelectedItem.Text.ToString().Trim();
                /////////////////////Heding End/////////////////////

                if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "Screen")
                {
                    ViewState["DataSet"] = ds.Tables[0];
                    ViewState["strheading"] = strheading.ToString().Trim();
                    FnHtml(ds.Tables[0], strheading.ToString().Trim());

                }
                if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "Export")
                    Export(ds.Tables[0], strheading.ToString().Trim());

            }
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "FnAlert", "FnAlert('No Record Found !!');", true);
        }
        void FnHtml(DataTable Dt, string strheading)
        {
            //////////For header
            String strHtmlheader = String.Empty;


            ////////////////Heading Bind//////////////////////
            strHtmlheader = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtmlheader += "<tr><td align=\"left\" colspan=" + Dt.Columns.Count + " style=\"color:Blue;\">" + strheading.ToString().Trim() + "</td></tr>";
            strHtmlheader += "</tr></table>";

            ////////Detail Display
            String strHtml = String.Empty;
            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";



            //////////////TABLE HEADER BIND
            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            for (int i = 0; i < Dt.Columns.Count; i++)
            {
                strHtml += "<td align=\"center\" style=\"font-size:smaller;\" nowrap=nowrap;><b>" + Dt.Columns[i].ColumnName + "</b></td>";
            }
            strHtml += "</tr>";

            int flag = 0;
            foreach (DataRow dr1 in Dt.Rows)
            {
                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                for (int j = 0; j < Dt.Columns.Count; j++)
                {
                    if (dr1[j] != DBNull.Value)
                    {
                        if (dr1[j].ToString().Trim().StartsWith("Type") || dr1[j].ToString().Trim().StartsWith("Total") || dr1[j].ToString().Trim().StartsWith("Grand") || dr1[j].ToString().Trim().StartsWith("ALL") || dr1[j].ToString().Trim().StartsWith("**"))
                        {
                            strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + Dt.Columns[j].ColumnName + "\"><b>" + dr1[j] + "</b></td>";
                        }
                        else if (dr1[j].ToString().Trim().StartsWith("Test"))
                        {
                            strHtml += "<td>&nbsp;</td>";
                        }
                        else if (IsNumeric(dr1[j].ToString()) == true)
                        {
                            strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + Dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                        }
                        else
                        {
                            strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + Dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

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
            ScriptManager.RegisterStartupScript(this, this.GetType(), "FnAlert", "FnAlert('DisPlay');", true);
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
        void Export(DataTable dtExport, string strheading)
        {


            DataTable dtReportHeader = new DataTable();
            DataTable dtReportFooter = new DataTable();

            DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            HeaderRow[0] = strheading.ToString().Trim();
            dtReportHeader.Rows.Add(HeaderRow);


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

            objExcel.ExportToExcelforExcel(dtExport, "Service Tax Statement ", "Total", dtReportHeader, dtReportFooter);

        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable Dt = (DataTable)ViewState["DataSet"];
            Export(Dt, ViewState["strheading"].ToString().Trim());
        }
    }
}