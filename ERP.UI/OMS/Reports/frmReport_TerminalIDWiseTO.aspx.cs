using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;
namespace ERP.OMS.Reports
{
    public partial class Reports_frmReport_TerminalIDWiseTO : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        MISReports mis = new MISReports();
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        DataSet ds = new DataSet();
        ExcelFile objExcel = new ExcelFile();
        DataTable dtExport = new DataTable();
        string data;
        int pageindex = 0;
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
            DtFrom.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
            DtTo.EditFormatString = oconverter.GetDateFormat("Date");
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
                    if (idlist[0].ToString().Trim() == "ClientsSelction")
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
                    else if (idlist[0].ToString().Trim() == "UserID")
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

                if (idlist[0] == "ScripsExchange")
                {
                    data = "ScripsExchange~" + str;
                }
                else if (idlist[0] == "ClientsSelction")
                {
                    data = "ClientsSelction~" + str;
                }
                else if (idlist[0] == "Group")
                {
                    data = "Group~" + str;
                }
                else if (idlist[0] == "Branch")
                {
                    data = "Branch~" + str;
                }
                else if (idlist[0] == "UserID")
                {
                    data = "UserID~" + str;
                }

            }
        }

        public void BindGroup()
        {
            ddlgrouptype.Items.Clear();
            DataTable DtGroup = oDBEngine.GetDataTable("tbl_master_groupmaster", "distinct gpm_Type", null);
            if (DtGroup.Rows.Count > 0)
            {
                ddlgrouptype.DataSource = DtGroup;
                ddlgrouptype.DataTextField = "gpm_Type";
                ddlgrouptype.DataValueField = "gpm_Type";
                ddlgrouptype.DataBind();
                ddlgrouptype.Items.Insert(0, new ListItem("Select GroupType", "0"));
                DtGroup.Dispose();

            }
            else
            {
                ddlgrouptype.Items.Insert(0, new ListItem("Select GroupType", "0"));
            }

        }
        protected void btnhide_Click(object sender, EventArgs e)
        {

            if (ddlGroup.SelectedItem.Value.ToString() == "2")
            {
                if (HiddenField_Group.Value.ToString().Trim() == "")
                {
                    BindGroup();
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
        public static bool IsNumeric(object value)
        {
            double dbl;
            return double.TryParse(value.ToString(), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out dbl);
        }
        void CheckBoxlistSelection()
        {
            string parameter = "";
            int colcount = 0;
            foreach (ListItem listitem in chktfilter.Items)
            {
                if (listitem.Selected)
                {
                    if (listitem.Value == "[Purchase Qty]")
                    {
                        parameter = "[Purchase Qty]";
                        colcount = colcount + 1;
                    }
                    if (listitem.Value == "[Sale Qty]")
                    {
                        if (parameter.ToString().Trim() == "")
                        {
                            parameter = "[Sale Qty]";
                        }
                        else
                        {
                            parameter = parameter.ToString().Trim() + ',' + "[Sale Qty]";

                        }
                        colcount = colcount + 1;
                    }
                    if (listitem.Value == "[Total Qty]")
                    {
                        if (parameter.ToString().Trim() == "")
                        {
                            parameter = "[Total Qty]";
                        }
                        else
                        {
                            parameter = parameter.ToString().Trim() + ',' + "[Total Qty]";

                        }
                        colcount = colcount + 1;
                    }
                    if (listitem.Value == "[Purchase Tot]")
                    {
                        if (parameter.ToString().Trim() == "")
                        {
                            parameter = "[Purchase Tot]";
                        }
                        else
                        {
                            parameter = parameter.ToString().Trim() + ',' + "[Purchase Tot]";

                        }
                        colcount = colcount + 1;
                    }
                    if (listitem.Value == "[Sale Tot]")
                    {
                        if (parameter.ToString().Trim() == "")
                        {
                            parameter = "[Sale Tot]";
                        }
                        else
                        {
                            parameter = parameter.ToString().Trim() + ',' + "[Sale Tot]";

                        }

                        colcount = colcount + 1;
                    }
                    if (listitem.Value == "[Total Tot]")
                    {
                        if (parameter.ToString().Trim() == "")
                        {
                            parameter = "[Total Tot]";
                        }
                        else
                        {
                            parameter = parameter.ToString().Trim() + ',' + "[Total Tot]";
                        }
                        colcount = colcount + 1;
                    }
                    if (listitem.Value == "[Net Obligation]")
                    {
                        if (parameter.ToString().Trim() == "")
                        {
                            parameter = "[Net Obligation]";
                        }
                        else
                        {
                            parameter = parameter.ToString().Trim() + ',' + "[Net Obligation]";
                        }
                        colcount = colcount + 1;
                    }


                }
            }
            if (colcount > 2)
            {
                Procedure(parameter.ToString().Trim());
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordStatus", "RecordStatus('4');", true);
            }

        }
        void Procedure(String Parameter)
        {
            string CommandText = "";
            if (Session["ExchangeSegmentID"].ToString().Trim() == "1" || Session["ExchangeSegmentID"].ToString().Trim() == "2" || Session["ExchangeSegmentID"].ToString().Trim() == "4" || Session["ExchangeSegmentID"].ToString().Trim() == "5" || Session["ExchangeSegmentID"].ToString().Trim() == "15" || Session["ExchangeSegmentID"].ToString().Trim() == "19" || Session["ExchangeSegmentID"].ToString().Trim() == "20")
            {
                CommandText = "Report_TerminalIdWiseTot";
            }
            else
            {
                CommandText = "Report_TerminalIdWiseTotCommCurrency";
            }

            string ClientId = "";
            string GrpType = "";
            string Grpid = "";
            if (ddlGroup.SelectedItem.Value.ToString().Trim() == "3")
            {
                if (rdBranchClientAll.Checked)
                {
                    ClientId = "ALL";
                }
                else
                {
                    ClientId = HiddenField_Client.Value;
                }
                GrpType = "BRANCH";
                Grpid = "ALL";
            }
            else
            {
                ClientId = "ALL";
                if (ddlGroup.SelectedItem.Value.ToString().Trim() == "1")
                {
                    GrpType = "BRANCH";
                    if (rdBranchClientAll.Checked)
                    {
                        Grpid = "ALL";
                    }
                    else
                    {
                        Grpid = HiddenField_Branch.Value;
                    }
                }

                if (ddlGroup.SelectedItem.Value.ToString().Trim() == "2")
                {
                    GrpType = ddlgrouptype.SelectedItem.Text.ToString().Trim();
                    if (rdddlgrouptypeAll.Checked)
                    {
                        Grpid = "ALL";
                    }
                    else
                    {
                        Grpid = HiddenField_Group.Value;
                    }
                }

            }

            ds = mis.Report_TerminalIdWiseTot_CommCurrency(CommandText, DtFrom.Value.ToString(), DtTo.Value.ToString(), Session["userbranchHierarchy"].ToString(),
                ClientId, GrpType, Grpid, HttpContext.Current.Session["usersegid"].ToString().Trim(), Session["LastCompany"].ToString(),
                rdbInstrumentAll.Checked ? "ALL" : HiddenField_Scrips.Value, rdbUserIdAll.Checked ? "ALL" : HiddenField_UserId.Value,
                Parameter.ToString().Trim(), ddlReportType.SelectedItem.Value.ToString().Trim());
            ViewState["dataset"] = ds;
            FnGeneRationCall(ds);
        }
        void FnGeneRationCall(DataSet ds)
        {

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ddlGeneration.SelectedItem.Value.ToString() == "1")///Screen
                {
                    FnHtml(ds);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordStatus", "RecordStatus('3');", true);
                }
                if (ddlGeneration.SelectedItem.Value.ToString() == "2")///Export
                {
                    Export(ds);
                }

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordStatus", "RecordStatus('1');", true);
            }
        }
        void FnHtml(DataSet ds)
        {
            //////////For header
            String strHtmlheader = String.Empty;
            string str = null;
            if (ddlGroup.SelectedItem.Value.ToString() == "2")
            {
                str = "[" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "]";
            }
            str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise Report ;" + str + " Period : " + oconverter.ArrangeDate2(DtFrom.Value.ToString()) + '-' + oconverter.ArrangeDate2(DtTo.Value.ToString());
            str = str + " ; Report View : " + ddlReportType.SelectedItem.Text.ToString().Trim();

            strHtmlheader = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtmlheader += "<tr><td align=\"left\" colspan=" + ds.Tables[0].Columns.Count + " style=\"color:Blue;\">" + str + "</td></tr></table>";


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
                        if (dr1[j].ToString().Trim().StartsWith("User") || dr1[j].ToString().Trim().StartsWith("Scrip") || dr1[j].ToString().Trim().StartsWith("Client") || dr1[j].ToString().Trim().StartsWith("Total") || dr1[j].ToString().Trim().StartsWith("Branch") || dr1[j].ToString().Trim().StartsWith("Group"))
                        {
                            strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\"><b>" + dr1[j] + "</b></td>";
                        }
                        else if (IsNumeric(dr1[j].ToString()) == true)
                        {
                            strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                        }
                        else
                        {
                            strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

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
        protected void btnScreen_Click(object sender, EventArgs e)
        {
            CheckBoxlistSelection();
        }
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            CheckBoxlistSelection();
        }
        void Export(DataSet ds)
        {
            DataTable dtExport = ds.Tables[0].Copy();


            string str = null;
            if (ddlGroup.SelectedItem.Value.ToString() == "2")
            {
                str = "[" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "]";
            }
            str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise Report ;" + str + " Period : " + oconverter.ArrangeDate2(DtFrom.Value.ToString()) + '-' + oconverter.ArrangeDate2(DtTo.Value.ToString());
            str = str + " ; Report View : " + ddlReportType.SelectedItem.Text.ToString().Trim();


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

            if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "2")
            {
                objExcel.ExportToExcelforExcel(dtExport, "TerminalID Wise Total Turn Over Report", "Total :", dtReportHeader, dtReportFooter);

            }
            else
            {
                if (ddlExport.SelectedItem.Value.ToString().Trim() == "E")
                {
                    objExcel.ExportToExcelforExcel(dtExport, "TerminalID Wise Total Turn Over Report", "Total :", dtReportHeader, dtReportFooter);
                }
                if (ddlExport.SelectedItem.Value.ToString().Trim() == "P")
                {
                    objExcel.ExportToPDF(dtExport, "TerminalID Wise Total Turn Over Report", "Total :", dtReportHeader, dtReportFooter);
                }
            }

        }
        protected void ddlExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            ds = (DataSet)ViewState["dataset"];
            Export(ds);
        }
    }
}