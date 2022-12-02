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
    public partial class Reports_SUBBRKGCOMMISSIONREPORT : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        PeriodicalReports periodicalReports = new PeriodicalReports();
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

                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load('" + HttpContext.Current.Session["ExchangeSegmentID"].ToString() + "');</script>");
                date();
                SegmentnameFetch();

            }
            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //___________-end here___//

        }
        void date()
        {

            DtFromDate.EditFormatString = oconverter.GetDateFormat("Date");
            DtFromDate.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());

            DtToDate.EditFormatString = oconverter.GetDateFormat("Date");
            DtToDate.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
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

        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            string id = eventArgument.ToString();
            string[] idlist = id.Split('~');
            string[] cl = idlist[1].Split(',');
            string str = "";
            string str1 = "";
            for (int i = 0; i < cl.Length; i++)
            {
                if (idlist[0] != "Clients" && idlist[0] != "Commission")
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
                        str = "'" + AcVal[0] + "'";
                        str1 = "'" + AcVal[0] + "'" + ";" + val[1];
                    }
                    else
                    {
                        str += ",'" + AcVal[0] + "'";
                        str1 += "," + "'" + AcVal[0] + "'" + ";" + val[1];
                    }
                }
            }

            if (idlist[0] == "Commission")
            {
                data = "Commission~" + str;
            }
            else if (idlist[0] == "Clients")
            {
                data = "Clients~" + str;
            }

            else if (idlist[0] == "Group")
            {
                data = "Group~" + str;
            }
            else if (idlist[0] == "Branch")
            {
                data = "Branch~" + str;
            }
            else if (idlist[0] == "Segment")
            {
                data = "Segment~" + str;
            }
        }

        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }
        void SegmentnameFetch()
        {
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "1")
                litSegmentMain.InnerText = "NSE-CM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "4")
                litSegmentMain.InnerText = "BSE-CM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "15")
                litSegmentMain.InnerText = "CSE-CM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "2")
                litSegmentMain.InnerText = "NSE-FO";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "5")
                litSegmentMain.InnerText = "BSE-FO";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "3")
                litSegmentMain.InnerText = "NSE-CDX";
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

            HiddenField_Segment.Value = Session["usersegid"].ToString();
        }

        protected void btnhide_Click(object sender, EventArgs e)
        {
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                if (HiddenField_Group.Value.ToString().Trim() == "")
                {
                    BindGroup();
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
        protected void btnScreen_Click(object sender, EventArgs e)
        {
            if (ddlReportView.SelectedItem.Value.ToString().Trim() == "3")
            {
                if (rdbClientALL.Checked || rdbCommissionAll.Checked || rdbranchAll.Checked || rdddlgrouptypeAll.Checked)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "fnNoRecord", "fnRecord('1');", true);

                }
                else
                {
                    Procedure();
                }
            }
            else
            {
                Procedure();
            }
        }
        void Procedure()
        {
            string GrpType = "";
            string GrpId = "";
            if (ddlGroup.SelectedItem.Value.ToString() == "0")
            {
                GrpType = "BRANCH";
                if (rdbranchAll.Checked)
                {
                    GrpId = "ALL";
                }
                else
                {
                    GrpId = HiddenField_Branch.Value;
                }
            }

            else
            {
                GrpType = ddlgrouptype.SelectedItem.Text.ToString().Trim();
                if (rdddlgrouptypeAll.Checked)
                {
                    GrpId = "ALL";
                }
                else
                {
                    GrpId = HiddenField_Group.Value;
                }
            }
            ds = periodicalReports.Report_SubBrokerageCalculationReport(Session["LastCompany"].ToString(), RdbSegmentAll.Checked ? "ALL" : HiddenField_Segment.Value,
                HttpContext.Current.Session["LastFinYear"].ToString().Trim(), "20", DtFromDate.Value.ToString(), DtToDate.Value.ToString(),
                ddlCommissionFor.SelectedItem.Value.ToString().Trim(), rdbCommissionAll.Checked ? "ALL" : HiddenField_Commission.Value,
                ddlReportView.SelectedItem.Value.ToString().Trim(), GrpType, GrpId, Session["userbranchHierarchy"].ToString(),
                rdbClientALL.Checked ? "ALL" : HiddenField_Client.Value);

            string str = null;
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                str = "[" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "]";
            }
            str = str + ddlGroup.SelectedItem.Text.ToString().Trim();
            str = str + "Wise Report; Period : " + oconverter.ArrangeDate2(DtFromDate.Value.ToString()) + " - " + oconverter.ArrangeDate2(DtToDate.Value.ToString());
            if (ddlReportView.SelectedItem.Value.ToString().Trim() != "4")
            {
                str = str + " ; Commission For : " + ddlCommissionFor.SelectedItem.Text.ToString().Trim();
            }
            ViewState["Header"] = str.ToString().Trim();

            ViewState["dataset"] = ds;
            FnGeneRationCall(ds);

        }
        void FnGeneRationCall(DataSet ds)
        {

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ddlGeneration.SelectedItem.Value.ToString() == "1")///Screen
                {
                    if (ddlReportView.SelectedItem.Value.ToString().Trim() == "1" || ddlReportView.SelectedItem.Value.ToString().Trim() == "4")///ReportView:Summary Wise or Clientwise Net Earning Report
                    {
                        FnHtml_SummaryNetEarning(ds);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "fnRecord", "fnRecord('4');", true);
                    }
                    if (ddlReportView.SelectedItem.Value.ToString().Trim() == "2")///ReportView:ClientWise Summary
                    {
                        BindCommission(ds);
                    }
                    if (ddlReportView.SelectedItem.Value.ToString().Trim() == "3")///ReportView:ClientWise Detail Only Export
                    {
                        Export(ds);
                    }
                }
                if (ddlGeneration.SelectedItem.Value.ToString() == "2")///Export
                {

                    Export(ds);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "fnNoRecord", "fnRecord('3');", true);
            }
        }
        void FnHtml_SummaryNetEarning(DataSet ds)
        {
            //////////For header
            String strHtmlheader = String.Empty;

            strHtmlheader = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtmlheader += "<tr><td align=\"left\" colspan=" + ds.Tables[0].Columns.Count + " style=\"color:Blue;\">" + ViewState["Header"].ToString().Trim() + "</td></tr></table>";


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
                        if (dr1[j].ToString().Trim().StartsWith("Total") || dr1[j].ToString().Trim().StartsWith("Exchange") || dr1[j].ToString().Trim().StartsWith("Type") || dr1[j].ToString().Trim().StartsWith("Grand") || dr1[j].ToString().Trim().StartsWith("Commission"))
                        {
                            strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\"><b>" + dr1[j] + "</b></td>";
                        }
                        else if (dr1[j].ToString().Trim().StartsWith("Test"))
                        {
                            strHtml += "<td>&nbsp;</td>";
                        }
                        else if (ds.Tables[0].Columns[j].ColumnName == "RelationShip Partner" || ds.Tables[0].Columns[j].ColumnName == "Name" || ds.Tables[0].Columns[j].ColumnName == "Sub Broker")
                        {
                            strHtml += "<td align=\"left\" style=\"font-size:smaller;\" title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";
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
            DivRecord.InnerHtml = strHtml;

        }
        void FnHtml_ClientWiseSummary(DataSet ds, string CommissionName)
        {
            //////////For header
            String strHtmlheader = String.Empty;

            DataView viewclient = new DataView();
            viewclient = ds.Tables[0].DefaultView;
            viewclient.RowFilter = "(CommissionName='" + CommissionName.ToString().Trim() + "')";
            DataTable dt = new DataTable();
            dt = viewclient.ToTable();

            dt.Columns.Remove("CommissionName");

            strHtmlheader = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtmlheader += "<tr><td align=\"left\" colspan=" + dt.Columns.Count + " style=\"color:Blue;\">" + ViewState["Header"].ToString().Trim() + "</td></tr></table>";


            ////////Detail Display
            String strHtml = String.Empty;
            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";



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
                        if (dr1[j].ToString().Trim().StartsWith("Total") || dr1[j].ToString().Trim().StartsWith("Exchange") || dr1[j].ToString().Trim().StartsWith("Name") || dr1[j].ToString().Trim().StartsWith("Grand") || dr1[j].ToString().Trim().StartsWith("Commission"))
                        {
                            strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\"><b>" + dr1[j] + "</b></td>";
                        }
                        else if (dr1[j].ToString().Trim().StartsWith("Test"))
                        {
                            strHtml += "<td>&nbsp;</td>";
                        }
                        else
                        {
                            if (IsNumeric(dr1[j].ToString()) == true)
                            {
                                strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                            }
                            else
                            {
                                strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

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
            DivRecord.InnerHtml = strHtml;

        }
        void BindCommission(DataSet ds)
        {
            DataView viewData = new DataView();
            viewData = ds.Tables[0].DefaultView;
            viewData.RowFilter = " CommissionName<>'ZZZZ'";
            DataTable dt = new DataTable();
            dt = viewData.ToTable();

            DataTable DistinctCommissionName = new DataTable();
            DataView viewCommissionName = new DataView(dt);
            DistinctCommissionName = viewCommissionName.ToTable(true, new string[] { "CommissionName" });

            if (DistinctCommissionName.Rows.Count > 0)
            {
                cmbrecord.DataSource = DistinctCommissionName;
                cmbrecord.DataValueField = "CommissionName";
                cmbrecord.DataTextField = "CommissionName";
                cmbrecord.DataBind();

            }
            LastPage = DistinctCommissionName.Rows.Count - 1;
            CurrentPage = 0;
            bind_Details();
        }
        void bind_Details()
        {
            cmbrecord.SelectedIndex = CurrentPage;
            ds = (DataSet)ViewState["dataset"];
            FnHtml_ClientWiseSummary(ds, cmbrecord.SelectedItem.Value.ToString().Trim());
            ScriptManager.RegisterStartupScript(this, this.GetType(), "fnRecord", "fnRecord('5');", true);
            ShowHidePreviousNext_of_Clients();
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
        void Export(DataSet ds)
        {
            if (ddlReportView.SelectedItem.Value.ToString().Trim() == "2")///ReportView:ClientWise Summary
            {
                ds.Tables[0].Columns.Remove("CommissionName");
            }
            DataTable dtExport = ds.Tables[0].Copy();


            DataTable dtReportHeader = new DataTable();
            DataTable dtReportFooter = new DataTable();

            DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            HeaderRow[0] = CompanyName.Rows[0][0].ToString();
            dtReportHeader.Rows.Add(HeaderRow);
            DataRow DrRowR1 = dtReportHeader.NewRow();
            DrRowR1[0] = ViewState["Header"].ToString().Trim();
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

            objExcel.ExportToExcelforExcel(dtExport, "Sub Brokerage & Commission Reports", "Total:", dtReportHeader, dtReportFooter);

        }


        protected void ddlExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            ds = (DataSet)ViewState["dataset"];
            Export(ds);
        }
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            Procedure();
        }
    }
}