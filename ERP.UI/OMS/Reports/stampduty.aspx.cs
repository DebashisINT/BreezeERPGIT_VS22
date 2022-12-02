using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_stampduty : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

        string data;
        PeriodicalReports pr = new PeriodicalReports();
        DataSet ds = new DataSet();
        DataTable Distinctbrokercontactid = new DataTable();
        DataTable Distinctclient = new DataTable();
        DataTable dtExport = new DataTable();
        //DBEngine oDBEngine = new DBEngine(string.Empty);
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        DataTable dtReportHeader = new DataTable();
        DataTable dtReportFooter = new DataTable();
        ExcelFile objExcel = new ExcelFile();
        DataTable dtgroupcontactid = new DataTable();
        ReportDocument ReportDocument = new ReportDocument();
        string[] connPath = ConfigurationManager.AppSettings["DBConnectionDefault"].Split(';');
        decimal totalto1 = decimal.Zero;
        decimal totalto = decimal.Zero;
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
                FilterColumnBind();
                date();

            }
            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);

            //___________-end here___//

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
        void date()
        {


            ////dtFrom.EditFormatString = oconverter.GetDateFormat("Date");
            ////dtTo.EditFormatString = oconverter.GetDateFormat("Date");
            ////DateTime first = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-1);
            ////DateTime last = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddDays(-1);
            ////dtFrom.Value = Convert.ToDateTime(first);
            ////dtTo.Value = Convert.ToDateTime(last);


            ///////////////
            string[] FinYear = Session["LastFinYear"].ToString().Split('-');
            // dtDate.EditFormatString = oconverter.GetDateFormat("Date");
            dtFrom.EditFormatString = oconverter.GetDateFormat("Date");
            dtTo.EditFormatString = oconverter.GetDateFormat("Date");

            // dtDate.Value = Convert.ToDateTime(Session["FinYearEnd"].ToString());
            dtFrom.Value = Convert.ToDateTime(Session["FinYearStart"].ToString());
            dtTo.Value = Convert.ToDateTime(Session["FinYearEnd"].ToString());


            //////////////////
            Segment();
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
                if (idlist[0].ToString().Trim() == "Clients" || idlist[0].ToString().Trim() == "Company")
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
            else if (idlist[0] == "Company")
            {
                data = "Company~" + str;
            }
            else if (idlist[0] == "state")
            {
                data = "state~" + str;
            }
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

        protected void btnshow_Click(object sender, EventArgs e)
        {
            Procedure();
            ds = (DataSet)ViewState["dataset"];
            SpCall(ds);
        }
        void Procedure()
        {

            string GRPTYPE = "";
            string Groupby = "";
            string RPTVIEW = "";
            string Segment_id = "";
            string Companyid = "";


            if (ddlGroup.SelectedItem.Value.ToString() == "0")
            {
                GRPTYPE = "BRANCH";
                if (rdbranchAll.Checked)
                {
                    Groupby = "ALL";
                }
                else
                {
                    Groupby = HiddenField_Branch.Value;
                }


            }
            if (ddlGroup.SelectedItem.Value.ToString() == "2")
            {
                GRPTYPE = "StateWise";
                if (rdbstateall.Checked)
                {
                    Groupby = "ALL";
                }
                else
                {
                    Groupby = HiddenField_state.Value;
                }
            }
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                GRPTYPE = ddlgrouptype.SelectedItem.Value.ToString().Trim();
                if (rdddlgrouptypeAll.Checked)
                {
                    Groupby = "ALL";
                }
                else
                {
                    Groupby = HiddenField_Group.Value;
                }
            }
            if (ddlReporttype.SelectedItem.Value.ToString().Trim() == "0")
            {
                if (ddlFormat.SelectedItem.Value.ToString().Trim() == "2")
                {
                    RPTVIEW = ddlRptViewPayableToAutority1.SelectedItem.Value.ToString();
                    Segment_id = Session["usersegid"].ToString();
                    Companyid = Session["LastCompany"].ToString();
                }
                else
                {
                    RPTVIEW = ddlRptViewPayableToAutority.SelectedItem.Value.ToString();
                    Segment_id = Session["usersegid"].ToString();
                    Companyid = Session["LastCompany"].ToString();
                }
            }
            else
            {
                RPTVIEW = ddlRptViewChargeToClient.SelectedItem.Value.ToString();
                if (rdbSegmentAll.Checked)
                {
                    Segment_id = "ALL";
                }
                else
                {
                    Segment_id = HiddenField_Segment.Value;
                }
                if (RdbAllCompany.Checked)
                {
                    Companyid = "ALL";
                }
                else if (RdbCurrentCompany.Checked)
                {
                    Companyid = "'" + Session["LastCompany"].ToString().Trim() + "'";
                }
                else
                {
                    Companyid = HiddenField_Company.Value;
                }
            }

            ds = pr.Report_StampDuty(dtFrom.Value.ToString(), dtTo.Value.ToString(), rdbClientALL.Checked ? "ALL" : HiddenField_Client.Value,
                ddlReporttype.SelectedItem.Value.ToString(), GRPTYPE, Groupby, HttpContext.Current.Session["ExchangeSegmentID"].ToString(),
                Session["userbranchHierarchy"].ToString(), RPTVIEW, Segment_id, Companyid, ChkCOnsolidatedAcrossSegment.Checked ? "Y" : "N",
                cmbConsiderDate.SelectedItem.Value.ToString(), ddlFormat.SelectedItem.Value.ToString().Trim());

            ViewState["dataset"] = ds;
        }
        void SpCall(DataSet ds)
        {
            FilterColumnsFetch();

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "1")
                {
                    if (ddlRptViewChargeToClient.SelectedItem.Value.ToString().Trim() == "1")
                    {
                        if (ddlGroup.SelectedItem.Value.ToString().Trim() == "2")
                        {
                            ddlbandforGRP1(ds);
                            CurrentPage = 0;
                            bind_Details();
                        }
                        else
                        {
                            ddlbandforGRP(ds);
                            CurrentPage = 0;
                            bind_Details();
                        }
                    }
                    else
                    {
                        if (ddlRptViewChargeToClient.SelectedItem.Value.ToString().Trim() != "3")
                        {
                            if (ChkCOnsolidatedAcrossSegment.Checked)
                            {
                                ds.Tables[0].Columns.Remove("StatusOrder");
                                ds.Tables[0].Columns.Remove("Grp");
                            }
                        }
                        FnHtmlAcrossSegment(ds);
                    }
                }
                else
                {
                    if (ddlReporttype.SelectedItem.Value.ToString().Trim() == "1")
                    {
                        if (ddlRptViewChargeToClient.SelectedItem.Value.ToString().Trim() == "1")
                        {
                            if (ddlGroup.SelectedItem.Value.ToString().Trim() == "2")
                            {
                                export_ChargeToClient_GiveBranchTotal1();
                            }
                            else
                            {
                                export_ChargeToClient_GiveBranchTotal();
                            }

                        }
                        else
                        {
                            if (ddlRptViewChargeToClient.SelectedItem.Value.ToString().Trim() != "3")
                            {
                                if (ChkCOnsolidatedAcrossSegment.Checked)
                                {
                                    ds.Tables[0].Columns.Remove("StatusOrder");
                                    ds.Tables[0].Columns.Remove("Grp");
                                }
                            }
                            ExportAcrossSegment(ds);

                        }
                    }
                    if (ddlReporttype.SelectedItem.Value.ToString().Trim() == "0")
                    {
                        if (ddlFormat.SelectedItem.Value.ToString().Trim() == "1")
                        {
                            if (ddlRptViewPayableToAutority.SelectedItem.Value.ToString().Trim() == "1")
                            {
                                export_PayabelToAuthority_Detail();
                            }
                            if (ddlRptViewPayableToAutority.SelectedItem.Value.ToString().Trim() == "2")
                            {
                                export_PayabelToAuthority_Summary();
                            }
                        }
                        else
                        {
                            if (ddlRptViewPayableToAutority1.SelectedItem.Value.ToString().Trim() == "1")
                            {
                                ExportAcrossSegment1();
                            }
                            if (ddlRptViewPayableToAutority1.SelectedItem.Value.ToString().Trim() == "2")
                            {
                                ExportAcrossSegment2();
                            }
                            if (ddlRptViewPayableToAutority1.SelectedItem.Value.ToString().Trim() == "3")
                            {
                                ExportAcrossSegment3();
                            }
                        }

                    }
                }


            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD", "NORECORD();", true);
            }
        }
        void ddlbandforGRP(DataSet ds)
        {

            DataView viewGRP = new DataView(ds.Tables[0]);
            DataTable DistinctGRP = viewGRP.ToTable(true, new string[] { "GRPID", "GRPNAME" });
            if (DistinctGRP.Rows.Count > 0)
            {
                cmbGROUP.DataSource = DistinctGRP;
                cmbGROUP.DataValueField = "GRPID";
                cmbGROUP.DataTextField = "GRPNAME";
                cmbGROUP.DataBind();
            }
            LastPage = DistinctGRP.Rows.Count - 1;
        }

        void ddlbandforGRP1(DataSet ds)
        {

            DataView viewGRP = new DataView(ds.Tables[0]);
            DataTable DistinctGRP = viewGRP.ToTable(true, new string[] { "state" });
            if (DistinctGRP.Rows.Count > 0)
            {
                cmbGROUP.DataSource = DistinctGRP;
                cmbGROUP.DataValueField = "state";
                cmbGROUP.DataTextField = "state";
                cmbGROUP.DataBind();
            }
            LastPage = DistinctGRP.Rows.Count - 1;
        }
        void bind_Details()
        {
            cmbGROUP.SelectedIndex = CurrentPage;
            if (LastPage > -1)
            {
                listRecord.Text = CurrentPage + 1 + " of " + cmbGROUP.Items.Count.ToString() + " Record.";

            }

            if (ddlReporttype.SelectedItem.Value.ToString() == "1")
            {
                if (ddlGroup.SelectedItem.Value.ToString().Trim() == "2")
                {
                    HTMLSELECTION_ChargeToClient();
                }
                else
                {
                    HTMLSELECTION_ChargeToClient1();
                }
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "DISPLAY", "DISPLAY('1');", true);
            ShowHidePreviousNext_of_Clients();


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
            CurrentPage = 0;
            bind_Details();
        }
        protected void ASPxPrevious_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 0)
            {
                CurrentPage = CurrentPage - 1;
                bind_Details();
            }
        }
        protected void ASPxNext_Click(object sender, EventArgs e)
        {
            if (CurrentPage < LastPage)
            {
                CurrentPage = CurrentPage + 1;
                bind_Details();
            }
        }
        protected void ASPxLast_Click(object sender, EventArgs e)
        {
            CurrentPage = LastPage;
            bind_Details();
        }
        protected string GetRowColor(int i)
        {
            if (i++ % 2 == 0)
                return "#fff0f5";
            else
                return "#FFFFFF";
        }
        void FilterColumnBind()
        {
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "1" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "4")
            {
                chktfilter.Items.Insert(0, new ListItem("Dlv TO", "Dlv TO"));
                chktfilter.Items.Insert(1, new ListItem("Sqr TO", "Sqr TO"));
                chktfilter.Items.Insert(2, new ListItem("Total TO", "Total TO"));
                chktfilter.Items.Insert(3, new ListItem("Stamp Duty", "Stamp Duty"));

                chktfilter.Items[0].Selected = true;
                chktfilter.Items[1].Selected = true;
                chktfilter.Items[2].Selected = true;
                chktfilter.Items[3].Selected = true;
            }
            else
            {
                chktfilter.Items.Insert(0, new ListItem("FUT TO", "FUT TO"));
                chktfilter.Items.Insert(1, new ListItem("PRM TO", "PRM TO"));
                chktfilter.Items.Insert(2, new ListItem("Finn.Sett TO", "Finn.Sett TO"));
                chktfilter.Items.Insert(3, new ListItem("Total TO", "Total TO"));
                chktfilter.Items.Insert(4, new ListItem("Stamp Duty", "Stamp Duty"));

                chktfilter.Items[0].Selected = true;
                chktfilter.Items[1].Selected = true;
                chktfilter.Items[2].Selected = true;
                chktfilter.Items[3].Selected = true;
                chktfilter.Items[4].Selected = true;
            }


        }
        void FilterColumnsFetch()
        {
            ViewState["DlvTO"] = "N";
            ViewState["SqrTO"] = "N";
            ViewState["FUTTO"] = "N";
            ViewState["PRMTO"] = "N";
            ViewState["FinnSettTO"] = "N";
            ViewState["TotalTO"] = "N";
            ViewState["StampDuty"] = "N";

            foreach (ListItem listitem in chktfilter.Items)
            {
                if (listitem.Selected)
                {
                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "1" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "4")
                    {
                        if (listitem.Value == "Dlv TO")
                        {
                            ViewState["DlvTO"] = "Y";
                        }
                        if (listitem.Value == "Sqr TO")
                        {
                            ViewState["SqrTO"] = "Y";
                        }
                    }
                    else
                    {
                        if (listitem.Value == "FUT TO")
                        {
                            ViewState["FUTTO"] = "Y";
                        }
                        if (listitem.Value == "PRM TO")
                        {
                            ViewState["PRMTO"] = "Y";
                        }
                        if (listitem.Value == "Finn.Sett TO")
                        {
                            ViewState["FinnSettTO"] = "Y";
                        }

                    }
                    if (listitem.Value == "Total TO")
                    {
                        ViewState["TotalTO"] = "Y";
                    }
                    if (listitem.Value == "Stamp Duty")
                    {
                        ViewState["StampDuty"] = "Y";
                    }

                }
            }
        }
        void HTMLSELECTION_ChargeToClient()
        {
            ds = (DataSet)ViewState["dataset"];
            String strHtml = String.Empty;
            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtml += "<tr><td align=\"left\" colspan=8 style=\"color:Blue;\">[" + ddlGroup.SelectedItem.Text.ToString() + " Wise] Report Period :<b>" + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "</b> - <b>" + oconverter.ArrangeDate2(dtTo.Value.ToString()) + "</b></td></tr>";
            displayPERIOD.InnerHtml = strHtml;

            strHtml = String.Empty;
            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtml += "<tr style=\"background-color:lavender ;text-align:left\"><td align=\"left\" colspan=8 style=\"color:Blue;\">" + ddlGroup.SelectedItem.Text.ToString() + " : " + cmbGROUP.SelectedItem.Text.ToString().Trim() + "</td></tr>";

            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            strHtml += "<td align=\"center\" colspan=2><b>Client Name</b></td>";
            strHtml += "<td align=\"center\"><b>UCC</b></td>";

            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "1")
            {
                if (ViewState["DlvTO"].ToString().Trim() == "Y")
                {
                    strHtml += "<td align=\"center\"><b>Dlv TO</b></td>";
                }
                if (ViewState["SqrTO"].ToString().Trim() == "Y")
                {
                    strHtml += "<td align=\"center\"><b>Sqr TO</b></td>";
                }
            }
            else
            {
                if (ViewState["FUTTO"].ToString().Trim() == "Y")
                {
                    strHtml += "<td align=\"center\"><b>FUT TO</b></td>";
                }
                if (ViewState["PRMTO"].ToString().Trim() == "Y")
                {
                    strHtml += "<td align=\"center\"><b>PRM TO</b></td>";
                }
                if (ViewState["FinnSettTO"].ToString().Trim() == "Y")
                {
                    strHtml += "<td align=\"center\"><b>Finn.Sett TO</b></td>";
                }
            }
            if (ViewState["TotalTO"].ToString().Trim() == "Y")
            {
                strHtml += "<td align=\"center\"><b>Total TO</b></td>";
            }
            if (ViewState["StampDuty"].ToString().Trim() == "Y")
            {
                strHtml += "<td align=\"center\"><b>Stamp Duty</b></td></tr>";
            }


            int flag = 0;

            decimal dlvto_sum = decimal.Zero;
            decimal sqrto_sum = decimal.Zero;
            decimal fsto_sum = decimal.Zero;
            decimal total_sum = decimal.Zero;
            decimal stamp_sum = decimal.Zero;

            DataView viewgrp = new DataView();
            viewgrp = ds.Tables[0].DefaultView;
            viewgrp.RowFilter = "state='" + cmbGROUP.SelectedItem.Value.ToString().Trim() + "' ";
            DataTable dt = new DataTable();
            dt = viewgrp.ToTable();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                flag = flag + 1;

                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                strHtml += "<td align=\"left\" colspan=2>" + dt.Rows[i]["CLIENTNAME"].ToString() + "</td>";
                strHtml += "<td align=\"left\">" + dt.Rows[i]["UCC"].ToString() + "</td>";

                if (ViewState["DlvTO"].ToString().Trim() == "Y" || ViewState["FUTTO"].ToString().Trim() == "Y")
                {
                    if (dt.Rows[i]["DelFutTO"] != DBNull.Value)
                    {
                        strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[i]["DelFutTO"].ToString())) + "</td>";
                        dlvto_sum = dlvto_sum + Convert.ToDecimal(dt.Rows[i]["DelFutTO"].ToString());
                    }
                    else
                    {
                        strHtml += "<td>&nbsp;</td>";
                    }
                }
                if (ViewState["SqrTO"].ToString().Trim() == "Y" || ViewState["PRMTO"].ToString().Trim() == "Y")
                {
                    if (dt.Rows[i]["SqrOptPrmTO"] != DBNull.Value)
                    {
                        strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[i]["SqrOptPrmTO"].ToString())) + "</td>";
                        sqrto_sum = sqrto_sum + Convert.ToDecimal(dt.Rows[i]["SqrOptPrmTO"].ToString());
                    }
                    else
                    {
                        strHtml += "<td>&nbsp;</td>";
                    }
                }
                if (ViewState["FinnSettTO"].ToString().Trim() == "Y")
                {
                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2")
                    {
                        if (dt.Rows[i]["FSTO"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[i]["FSTO"].ToString())) + "</td>";
                            fsto_sum = fsto_sum + Convert.ToDecimal(dt.Rows[i]["FSTO"].ToString());
                        }
                        else
                        {
                            strHtml += "<td>&nbsp;</td>";
                        }
                    }
                }
                if (ViewState["TotalTO"].ToString().Trim() == "Y")
                {
                    if (dt.Rows[i]["TotalTO"] != DBNull.Value)
                    {
                        strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[i]["TotalTO"].ToString())) + "</td>";
                        total_sum = total_sum + Convert.ToDecimal(dt.Rows[i]["TotalTO"].ToString());
                    }
                    else
                    {
                        strHtml += "<td>&nbsp;</td>";
                    }
                }
                if (ViewState["StampDuty"].ToString().Trim() == "Y")
                {
                    if (dt.Rows[i]["StampDuty"] != DBNull.Value)
                    {
                        strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[i]["StampDuty"].ToString())) + "</td>";
                        stamp_sum = stamp_sum + Convert.ToDecimal(dt.Rows[i]["StampDuty"].ToString());
                    }
                    else
                    {
                        strHtml += "<td>&nbsp;</td>";
                    }
                }
                strHtml += "</tr>";

            }

            strHtml += "<tr>";
            strHtml += "<td style=\"color:Black;\" colspan=3><b>Total</b></td>";
            if (ViewState["DlvTO"].ToString().Trim() == "Y" || ViewState["FUTTO"].ToString().Trim() == "Y")
            {
                strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dlvto_sum)) + "</td>";
            }
            if (ViewState["SqrTO"].ToString().Trim() == "Y" || ViewState["PRMTO"].ToString().Trim() == "Y")
            {
                strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(sqrto_sum)) + "</td>";
            }
            if (ViewState["FinnSettTO"].ToString().Trim() == "Y")
            {
                if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2")
                {
                    strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(fsto_sum)) + "</td>";

                }
            }
            if (ViewState["TotalTO"].ToString().Trim() == "Y")
            {
                strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(total_sum)) + "</td>";
            }
            if (ViewState["StampDuty"].ToString().Trim() == "Y")
            {
                strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(stamp_sum)) + "</td></tr>";
            }

            strHtml += "</table>";
            display.InnerHtml = strHtml;
        }

        void HTMLSELECTION_ChargeToClient1()
        {
            ds = (DataSet)ViewState["dataset"];
            String strHtml = String.Empty;
            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtml += "<tr><td align=\"left\" colspan=8 style=\"color:Blue;\">[" + ddlGroup.SelectedItem.Text.ToString() + " Wise] Report Period :<b>" + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "</b> - <b>" + oconverter.ArrangeDate2(dtTo.Value.ToString()) + "</b></td></tr>";
            displayPERIOD.InnerHtml = strHtml;

            strHtml = String.Empty;
            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtml += "<tr style=\"background-color:lavender ;text-align:left\"><td align=\"left\" colspan=8 style=\"color:Blue;\">" + ddlGroup.SelectedItem.Text.ToString() + " : " + cmbGROUP.SelectedItem.Text.ToString().Trim() + "</td></tr>";

            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            strHtml += "<td align=\"center\" colspan=2><b>Client Name</b></td>";
            strHtml += "<td align=\"center\"><b>UCC</b></td>";

            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "1")
            {
                if (ViewState["DlvTO"].ToString().Trim() == "Y")
                {
                    strHtml += "<td align=\"center\"><b>Dlv TO</b></td>";
                }
                if (ViewState["SqrTO"].ToString().Trim() == "Y")
                {
                    strHtml += "<td align=\"center\"><b>Sqr TO</b></td>";
                }
            }
            else
            {
                if (ViewState["FUTTO"].ToString().Trim() == "Y")
                {
                    strHtml += "<td align=\"center\"><b>FUT TO</b></td>";
                }
                if (ViewState["PRMTO"].ToString().Trim() == "Y")
                {
                    strHtml += "<td align=\"center\"><b>PRM TO</b></td>";
                }
                if (ViewState["FinnSettTO"].ToString().Trim() == "Y")
                {
                    strHtml += "<td align=\"center\"><b>Finn.Sett TO</b></td>";
                }
            }
            if (ViewState["TotalTO"].ToString().Trim() == "Y")
            {
                strHtml += "<td align=\"center\"><b>Total TO</b></td>";
            }
            if (ViewState["StampDuty"].ToString().Trim() == "Y")
            {
                strHtml += "<td align=\"center\"><b>Stamp Duty</b></td></tr>";
            }


            int flag = 0;

            decimal dlvto_sum = decimal.Zero;
            decimal sqrto_sum = decimal.Zero;
            decimal fsto_sum = decimal.Zero;
            decimal total_sum = decimal.Zero;
            decimal stamp_sum = decimal.Zero;

            DataView viewgrp = new DataView();
            viewgrp = ds.Tables[0].DefaultView;
            viewgrp.RowFilter = "GRPID=" + cmbGROUP.SelectedItem.Value;
            DataTable dt = new DataTable();
            dt = viewgrp.ToTable();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                flag = flag + 1;

                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                strHtml += "<td align=\"left\" colspan=2>" + dt.Rows[i]["CLIENTNAME"].ToString() + "</td>";
                strHtml += "<td align=\"left\">" + dt.Rows[i]["UCC"].ToString() + "</td>";

                if (ViewState["DlvTO"].ToString().Trim() == "Y" || ViewState["FUTTO"].ToString().Trim() == "Y")
                {
                    if (dt.Rows[i]["DelFutTO"] != DBNull.Value)
                    {
                        strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[i]["DelFutTO"].ToString())) + "</td>";
                        dlvto_sum = dlvto_sum + Convert.ToDecimal(dt.Rows[i]["DelFutTO"].ToString());
                    }
                    else
                    {
                        strHtml += "<td>&nbsp;</td>";
                    }
                }
                if (ViewState["SqrTO"].ToString().Trim() == "Y" || ViewState["PRMTO"].ToString().Trim() == "Y")
                {
                    if (dt.Rows[i]["SqrOptPrmTO"] != DBNull.Value)
                    {
                        strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[i]["SqrOptPrmTO"].ToString())) + "</td>";
                        sqrto_sum = sqrto_sum + Convert.ToDecimal(dt.Rows[i]["SqrOptPrmTO"].ToString());
                    }
                    else
                    {
                        strHtml += "<td>&nbsp;</td>";
                    }
                }
                if (ViewState["FinnSettTO"].ToString().Trim() == "Y")
                {
                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2")
                    {
                        if (dt.Rows[i]["FSTO"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[i]["FSTO"].ToString())) + "</td>";
                            fsto_sum = fsto_sum + Convert.ToDecimal(dt.Rows[i]["FSTO"].ToString());
                        }
                        else
                        {
                            strHtml += "<td>&nbsp;</td>";
                        }
                    }
                }
                if (ViewState["TotalTO"].ToString().Trim() == "Y")
                {
                    if (dt.Rows[i]["TotalTO"] != DBNull.Value)
                    {
                        strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[i]["TotalTO"].ToString())) + "</td>";
                        total_sum = total_sum + Convert.ToDecimal(dt.Rows[i]["TotalTO"].ToString());
                    }
                    else
                    {
                        strHtml += "<td>&nbsp;</td>";
                    }
                }
                if (ViewState["StampDuty"].ToString().Trim() == "Y")
                {
                    if (dt.Rows[i]["StampDuty"] != DBNull.Value)
                    {
                        strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[i]["StampDuty"].ToString())) + "</td>";
                        stamp_sum = stamp_sum + Convert.ToDecimal(dt.Rows[i]["StampDuty"].ToString());
                    }
                    else
                    {
                        strHtml += "<td>&nbsp;</td>";
                    }
                }
                strHtml += "</tr>";

            }

            strHtml += "<tr>";
            strHtml += "<td style=\"color:Black;\" colspan=3><b>Total</b></td>";
            if (ViewState["DlvTO"].ToString().Trim() == "Y" || ViewState["FUTTO"].ToString().Trim() == "Y")
            {
                strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dlvto_sum)) + "</td>";
            }
            if (ViewState["SqrTO"].ToString().Trim() == "Y" || ViewState["PRMTO"].ToString().Trim() == "Y")
            {
                strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(sqrto_sum)) + "</td>";
            }
            if (ViewState["FinnSettTO"].ToString().Trim() == "Y")
            {
                if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2")
                {
                    strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(fsto_sum)) + "</td>";

                }
            }
            if (ViewState["TotalTO"].ToString().Trim() == "Y")
            {
                strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(total_sum)) + "</td>";
            }
            if (ViewState["StampDuty"].ToString().Trim() == "Y")
            {
                strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(stamp_sum)) + "</td></tr>";
            }

            strHtml += "</table>";
            display.InnerHtml = strHtml;
        }

        void export_ChargeToClient_GiveBranchTotal()
        {

            ds = (DataSet)ViewState["dataset"];
            dtExport = new DataTable();
            dtExport.Clear();
            dtExport.Columns.Add("Client Name", Type.GetType("System.String"));
            dtExport.Columns.Add("UCC", Type.GetType("System.String"));
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "1")
            {
                if (ViewState["DlvTO"].ToString().Trim() == "Y")
                {
                    dtExport.Columns.Add("Dlv TO", Type.GetType("System.String"));
                }
                if (ViewState["SqrTO"].ToString().Trim() == "Y")
                {
                    dtExport.Columns.Add("Sqr TO", Type.GetType("System.String"));
                }
            }
            else
            {
                if (ViewState["FUTTO"].ToString().Trim() == "Y")
                {
                    dtExport.Columns.Add("FUT TO", Type.GetType("System.String"));
                }
                if (ViewState["PRMTO"].ToString().Trim() == "Y")
                {
                    dtExport.Columns.Add("PRM TO", Type.GetType("System.String"));
                }
                if (ViewState["FinnSettTO"].ToString().Trim() == "Y")
                {
                    dtExport.Columns.Add("Finn.Sett TO", Type.GetType("System.String"));
                }
            }
            if (ViewState["TotalTO"].ToString().Trim() == "Y")
            {
                dtExport.Columns.Add("Total TO", Type.GetType("System.String"));
            }
            if (ViewState["StampDuty"].ToString().Trim() == "Y")
            {
                dtExport.Columns.Add("Stamp Duty", Type.GetType("System.String"));
            }


            DataView viewGRP = new DataView(ds.Tables[0]);
            DataTable DistinctGRP = viewGRP.ToTable(true, new string[] { "GRPID", "GRPNAME" });
            if (DistinctGRP.Rows.Count > 0)
            {
                cmbGROUP.DataSource = DistinctGRP;
                cmbGROUP.DataValueField = "GRPID";
                cmbGROUP.DataTextField = "GRPNAME";
                cmbGROUP.DataBind();
            }

            decimal dlvto_sum = decimal.Zero;
            decimal sqrto_sum = decimal.Zero;
            decimal total_sum = decimal.Zero;
            decimal stamp_sum = decimal.Zero;
            decimal fsto_sum = decimal.Zero;

            decimal dlvto_grpsum = decimal.Zero;
            decimal sqrto_grpsum = decimal.Zero;
            decimal total_grpsum = decimal.Zero;
            decimal stamp_grpsum = decimal.Zero;
            decimal fsto_grpsum = decimal.Zero;

            int cmbgrp_count = cmbGROUP.Items.Count;
            for (int i = 0; i < cmbgrp_count; i++)
            {
                dlvto_sum = decimal.Zero;
                sqrto_sum = decimal.Zero;
                total_sum = decimal.Zero;
                stamp_sum = decimal.Zero;
                fsto_sum = decimal.Zero;

                string valItem = cmbGROUP.Items[i].Value;
                DataView viewgrp = new DataView();
                viewgrp = ds.Tables[0].DefaultView;
                viewgrp.RowFilter = "GRPID='" + valItem + "'";
                DataTable dt = new DataTable();
                dt = viewgrp.ToTable();

                DataRow row = dtExport.NewRow();
                row["Client Name"] = ddlGroup.SelectedItem.Text.ToString() + " : " + cmbGROUP.Items[i].Text.ToString().Trim();
                row["UCC"] = "Test";
                dtExport.Rows.Add(row);

                foreach (DataRow dr1 in dt.Rows)
                {
                    DataRow row2 = dtExport.NewRow();
                    row2["Client Name"] = dr1["CLIENTNAME"].ToString();
                    row2["UCC"] = dr1["UCC"].ToString();

                    if (ViewState["DlvTO"].ToString().Trim() == "Y" || ViewState["FUTTO"].ToString().Trim() == "Y")
                    {
                        if (dr1["DelFutTO"] != DBNull.Value)
                        {
                            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2")
                            {
                                row2["FUT TO"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dr1["DelFutTO"]));
                            }
                            else
                            {
                                row2["Dlv TO"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dr1["DelFutTO"]));
                            }
                            dlvto_sum = dlvto_sum + Convert.ToDecimal(dr1["DelFutTO"]);
                            dlvto_grpsum = dlvto_grpsum + Convert.ToDecimal(dr1["DelFutTO"]);
                        }

                    }
                    if (ViewState["SqrTO"].ToString().Trim() == "Y" || ViewState["PRMTO"].ToString().Trim() == "Y")
                    {
                        if (dr1["SqrOptPrmTO"] != DBNull.Value)
                        {
                            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2")
                            {
                                row2["PRM TO"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dr1["SqrOptPrmTO"]));
                            }
                            else
                            {
                                row2["Sqr TO"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dr1["SqrOptPrmTO"]));
                            }
                            sqrto_sum = sqrto_sum + Convert.ToDecimal(dr1["SqrOptPrmTO"].ToString());
                            sqrto_grpsum = sqrto_grpsum + Convert.ToDecimal(dr1["SqrOptPrmTO"].ToString());
                        }
                    }
                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2")
                    {
                        if (ViewState["FinnSettTO"].ToString().Trim() == "Y")
                        {
                            if (dr1["FSTO"] != DBNull.Value)
                            {
                                row2["Finn.Sett TO"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dr1["FSTO"]));
                                fsto_sum = fsto_sum + Convert.ToDecimal(dr1["FSTO"].ToString());
                                fsto_grpsum = fsto_grpsum + Convert.ToDecimal(dr1["FSTO"].ToString());
                            }

                        }
                    }
                    if (ViewState["TotalTO"].ToString().Trim() == "Y")
                    {
                        if (dr1["TotalTO"] != DBNull.Value)
                        {
                            row2["Total TO"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dr1["TotalTO"]));
                            total_sum = total_sum + Convert.ToDecimal(dr1["TotalTO"].ToString());
                            total_grpsum = total_grpsum + Convert.ToDecimal(dr1["TotalTO"].ToString());
                        }
                    }
                    if (ViewState["StampDuty"].ToString().Trim() == "Y")
                    {
                        if (dr1["StampDuty"] != DBNull.Value)
                        {
                            row2["Stamp Duty"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dr1["StampDuty"]));
                            stamp_sum = stamp_sum + Convert.ToDecimal(dr1["StampDuty"].ToString());
                            stamp_grpsum = stamp_grpsum + Convert.ToDecimal(dr1["StampDuty"].ToString());
                        }
                    }
                    dtExport.Rows.Add(row2);
                }

                DataRow row11 = dtExport.NewRow();
                row11["Client Name"] = "Total";
                if (ViewState["DlvTO"].ToString().Trim() == "Y" || ViewState["FUTTO"].ToString().Trim() == "Y")
                {
                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2")
                    {
                        row11["FUT TO"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dlvto_sum));
                    }
                    else
                    {
                        row11["Dlv TO"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dlvto_sum));
                    }

                }
                if (ViewState["SqrTO"].ToString().Trim() == "Y" || ViewState["PRMTO"].ToString().Trim() == "Y")
                {
                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2")
                    {
                        row11["PRM TO"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(fsto_sum));
                    }
                    else
                    {
                        row11["Sqr TO"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(fsto_sum));
                    }
                }
                if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2")
                {
                    if (ViewState["FinnSettTO"].ToString().Trim() == "Y")
                    {
                        row11["Finn.Sett TO"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(fsto_sum));
                    }
                }
                if (ViewState["TotalTO"].ToString().Trim() == "Y")
                {
                    row11["Total TO"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(total_sum));
                }
                if (ViewState["StampDuty"].ToString().Trim() == "Y")
                {
                    row11["Stamp Duty"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(stamp_sum));
                }
                dtExport.Rows.Add(row11);
            }
            /////Grand Total
            DataRow row13 = dtExport.NewRow();
            dtExport.Rows.Add(row13);


            DataRow row12 = dtExport.NewRow();
            row12["Client Name"] = "Grand Total :";
            if (ViewState["DlvTO"].ToString().Trim() == "Y" || ViewState["FUTTO"].ToString().Trim() == "Y")
            {
                if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2")
                {
                    row12["FUT TO"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dlvto_grpsum));
                }
                else
                {
                    row12["Dlv TO"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dlvto_grpsum));
                }

            }
            if (ViewState["SqrTO"].ToString().Trim() == "Y" || ViewState["PRMTO"].ToString().Trim() == "Y")
            {
                if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2")
                {
                    row12["PRM TO"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(fsto_grpsum));
                }
                else
                {
                    row12["Sqr TO"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(fsto_grpsum));
                }
            }
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2")
            {
                if (ViewState["FinnSettTO"].ToString().Trim() == "Y")
                {
                    row12["Finn.Sett TO"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(fsto_grpsum));
                }
            }
            if (ViewState["TotalTO"].ToString().Trim() == "Y")
            {
                row12["Total TO"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(total_grpsum));
            }
            if (ViewState["StampDuty"].ToString().Trim() == "Y")
            {
                row12["Stamp Duty"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(stamp_grpsum));
            }
            dtExport.Rows.Add(row12);

            if (dtExport.Columns.Count < 4)
            {
                dtExport.Columns.Add("Remarks", Type.GetType("System.String"));
            }
            ViewState["EXPORTSUMMARY"] = dtExport;
            headerfotter();
        }

        void headerfotter()
        {
            DataTable dtExport = (DataTable)ViewState["EXPORTSUMMARY"];
            DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            HeaderRow[0] = CompanyName.Rows[0][0].ToString();
            dtReportHeader.Rows.Add(HeaderRow);
            DataRow DrRowR1 = dtReportHeader.NewRow();

            if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "1")
            {
                DrRowR1[0] = "Stamp Duty Statement :[ " + ddlGroup.SelectedItem.Text.ToString() + " Wise] Report Period :" + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtTo.Value.ToString());
            }
            else
            {
                DrRowR1[0] = "Stamp Duty Statement For :" + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtTo.Value.ToString());
            }

            dtReportHeader.Rows.Add(DrRowR1);
            DataRow DrRowR2 = dtReportHeader.NewRow();
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

            if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "1")
            {
                if (ddlExport.SelectedItem.Value == "E")
                {
                    objExcel.ExportToExcelforExcel(dtExport, "Stamp Duty Statement", "Total", dtReportHeader, dtReportFooter);
                }
                else if (ddlExport.SelectedItem.Value == "P")
                {
                    objExcel.ExportToPDF(dtExport, "Stamp Duty Statement", "Total", dtReportHeader, dtReportFooter);
                }
            }
            else
            {
                objExcel.ExportToExcelforExcel(dtExport, "Stamp Duty Statement", "Total", dtReportHeader, dtReportFooter);
                //objExcel.ExportToPDF(dtExport, "Stamp Duty Statement", "Total", dtReportHeader, dtReportFooter);

            }
        }

        protected void ddlExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            ds = (DataSet)ViewState["dataset"];
            if (ddlReporttype.SelectedItem.Value.ToString().Trim() == "1")
            {
                if (ddlRptViewChargeToClient.SelectedItem.Value.ToString().Trim() == "1")
                {
                    export_ChargeToClient_GiveBranchTotal();
                }
                else
                {


                    ExportAcrossSegment(ds);

                }
            }
        }
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            Procedure();
            ds = (DataSet)ViewState["dataset"];
            SpCall(ds);
        }
        void export_PayabelToAuthority_Summary()
        {
            ds = (DataSet)ViewState["dataset"];
            DataTable dtnew = ds.Tables[0];
            dtExport = new DataTable();
            dtExport.Clear();
            dtExport.Columns.Add("Particulars Of Trade", Type.GetType("System.String"));
            dtExport.Columns.Add("Cash Market Turnover", Type.GetType("System.String"));
            dtExport.Columns.Add("Derivatives Market Turnover", Type.GetType("System.String"));
            dtExport.Columns.Add("Stamp-Duty Rate (%)", Type.GetType("System.String"));
            dtExport.Columns.Add("Stamp-Duty Payable", Type.GetType("System.String"));
            //dtnew.DefaultView.ToTable(true, "TYPE");
            DataView viewgroup = new DataView(ds.Tables[0]);
            dtgroupcontactid = viewgroup.ToTable(true, new string[] { "TYPE" });
            //DataRow[] results = ds.Tables[0].Select("[TYPE]");
            /////////////For MahaRashtra
            DataView viewMAHA = new DataView();
            viewMAHA = ds.Tables[0].DefaultView;
            viewMAHA.RowFilter = "TYPE='MAHA'";
            DataTable dt = new DataTable();
            dt = viewMAHA.ToTable();

            DataRow row = dtExport.NewRow();
            row[0] = "MAHA";
            row[1] = "Test";
            dtExport.Rows.Add(row);

            decimal cmtot = decimal.Zero;
            decimal fotot = decimal.Zero;
            decimal pay = decimal.Zero;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row1 = dtExport.NewRow();
                row1[0] = dt.Rows[i]["RECORDTYPE"].ToString();
                row1[1] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt.Rows[i]["CMTOT"].ToString()));
                row1[2] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt.Rows[i]["FOTOT"].ToString()));
                row1[3] = dt.Rows[i]["MAHACLEINTSQRSTAMPCM"].ToString();
                row1[4] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt.Rows[i]["PABLE"].ToString()));
                dtExport.Rows.Add(row1);

                cmtot = cmtot + Convert.ToDecimal(dt.Rows[i]["CMTOT"].ToString());
                fotot = fotot + Convert.ToDecimal(dt.Rows[i]["FOTOT"].ToString());
                pay = pay + Convert.ToDecimal(dt.Rows[i]["PABLE"].ToString());
            }

            ////////////sum
            DataRow rowmahasum = dtExport.NewRow();
            rowmahasum[0] = "Total";
            rowmahasum[1] = oconverter.formatmoneyinUs(Convert.ToDecimal(cmtot));
            rowmahasum[2] = oconverter.formatmoneyinUs(Convert.ToDecimal(fotot));
            rowmahasum[4] = oconverter.formatmoneyinUs(Convert.ToDecimal(pay));
            dtExport.Rows.Add(rowmahasum);

            /////////////For Non-MahaRashtra
            DataView viewNonMAHA = new DataView();
            viewNonMAHA = ds.Tables[0].DefaultView;
            viewNonMAHA.RowFilter = "TYPE='Non-MAHA'";
            DataTable dt1 = new DataTable();
            dt1 = viewNonMAHA.ToTable();

            DataRow row2 = dtExport.NewRow();
            row2[0] = "Non-MAHA";
            row2[1] = "Test";
            dtExport.Rows.Add(row2);

            cmtot = decimal.Zero;
            fotot = decimal.Zero;
            pay = decimal.Zero;

            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                DataRow row3 = dtExport.NewRow();
                row3[0] = dt1.Rows[i]["RECORDTYPE"].ToString();
                row3[1] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["CMTOT"].ToString()));
                row3[2] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["FOTOT"].ToString()));
                row3[3] = dt1.Rows[i]["MAHACLEINTSQRSTAMPCM"].ToString();
                row3[4] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["PABLE"].ToString()));
                dtExport.Rows.Add(row3);

                cmtot = cmtot + Convert.ToDecimal(dt1.Rows[i]["CMTOT"].ToString());
                fotot = fotot + Convert.ToDecimal(dt1.Rows[i]["FOTOT"].ToString());
                pay = pay + Convert.ToDecimal(dt1.Rows[i]["PABLE"].ToString());
            }
            ////////////sum
            DataRow rownomahasum = dtExport.NewRow();
            rownomahasum[0] = "Total";
            rownomahasum[1] = oconverter.formatmoneyinUs(Convert.ToDecimal(cmtot));
            rownomahasum[2] = oconverter.formatmoneyinUs(Convert.ToDecimal(fotot));
            rownomahasum[4] = oconverter.formatmoneyinUs(Convert.ToDecimal(pay));
            dtExport.Rows.Add(rownomahasum);
            ViewState["EXPORTSUMMARY"] = dtExport;
            headerfotter();
        }
        void export_PayabelToAuthority_Detail()
        {
            ds = (DataSet)ViewState["dataset"];

            dtExport = new DataTable();
            dtExport.Clear();
            dtExport.Columns.Add("Date", Type.GetType("System.String"));
            dtExport.Columns.Add("Particulars Of Trade", Type.GetType("System.String"));
            dtExport.Columns.Add("Cash Market Turnover", Type.GetType("System.String"));
            dtExport.Columns.Add("Derivatives Market Turnover", Type.GetType("System.String"));
            dtExport.Columns.Add("Stamp-Duty Rate (%)", Type.GetType("System.String"));
            dtExport.Columns.Add("Stamp-Duty Payable", Type.GetType("System.String"));


            /////////////For MahaRashtra
            DataView viewMAHA = new DataView();
            viewMAHA = ds.Tables[0].DefaultView;
            viewMAHA.RowFilter = "TYPE='MAHA'";
            DataTable dt = new DataTable();
            dt = viewMAHA.ToTable();

            DataRow row = dtExport.NewRow();
            row[0] = "MAHA";
            row[1] = "Test";
            dtExport.Rows.Add(row);

            decimal cmtot = decimal.Zero;
            decimal fotot = decimal.Zero;
            decimal pay = decimal.Zero;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row1 = dtExport.NewRow();
                row1[0] = dt.Rows[i]["DATE1"].ToString();
                row1[1] = dt.Rows[i]["RECORDTYPE"].ToString();
                row1[2] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt.Rows[i]["CMTOT"].ToString()));
                row1[3] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt.Rows[i]["FOTOT"].ToString()));
                row1[4] = dt.Rows[i]["MAHACLEINTSQRSTAMPCM"].ToString();
                row1[5] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt.Rows[i]["PABLE"].ToString()));
                dtExport.Rows.Add(row1);

                cmtot = cmtot + Convert.ToDecimal(dt.Rows[i]["CMTOT"].ToString());
                fotot = fotot + Convert.ToDecimal(dt.Rows[i]["FOTOT"].ToString());
                pay = pay + Convert.ToDecimal(dt.Rows[i]["PABLE"].ToString());
            }

            ////////////sum
            DataRow rowmahasum = dtExport.NewRow();
            rowmahasum[0] = "Total";
            rowmahasum[2] = oconverter.formatmoneyinUs(Convert.ToDecimal(cmtot));
            rowmahasum[3] = oconverter.formatmoneyinUs(Convert.ToDecimal(fotot));
            rowmahasum[5] = oconverter.formatmoneyinUs(Convert.ToDecimal(pay));
            dtExport.Rows.Add(rowmahasum);

            /////////////For Non-MahaRashtra
            DataView viewNonMAHA = new DataView();
            viewNonMAHA = ds.Tables[0].DefaultView;
            viewNonMAHA.RowFilter = "TYPE='Non-MAHA'";
            DataTable dt1 = new DataTable();
            dt1 = viewNonMAHA.ToTable();

            DataRow row2 = dtExport.NewRow();
            row2[0] = "Non-MAHA";
            row2[1] = "Test";
            dtExport.Rows.Add(row2);

            cmtot = decimal.Zero;
            fotot = decimal.Zero;
            pay = decimal.Zero;

            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                DataRow row3 = dtExport.NewRow();
                row3[0] = dt1.Rows[i]["DATE1"].ToString();
                row3[1] = dt1.Rows[i]["RECORDTYPE"].ToString();
                row3[2] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["CMTOT"].ToString()));
                row3[3] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["FOTOT"].ToString()));
                row3[4] = dt1.Rows[i]["MAHACLEINTSQRSTAMPCM"].ToString();
                row3[5] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["PABLE"].ToString()));
                dtExport.Rows.Add(row3);

                cmtot = cmtot + Convert.ToDecimal(dt1.Rows[i]["CMTOT"].ToString());
                fotot = fotot + Convert.ToDecimal(dt1.Rows[i]["FOTOT"].ToString());
                pay = pay + Convert.ToDecimal(dt1.Rows[i]["PABLE"].ToString());
            }
            ////////////sum
            DataRow rownomahasum = dtExport.NewRow();
            rownomahasum[0] = "Total";
            rownomahasum[2] = oconverter.formatmoneyinUs(Convert.ToDecimal(cmtot));
            rownomahasum[3] = oconverter.formatmoneyinUs(Convert.ToDecimal(fotot));
            rownomahasum[5] = oconverter.formatmoneyinUs(Convert.ToDecimal(pay));
            dtExport.Rows.Add(rownomahasum);

            ViewState["EXPORTSUMMARY"] = dtExport;
            headerfotter();
        }

        protected void btnPDF_Click(object sender, EventArgs e)
        {
            Procedure();
            DataSet ds = (DataSet)ViewState["dataset"];
            if (ds.Tables[0].Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(1);", true);
            }
            else
            {
                if (ddlRptViewPayableToAutority.SelectedValue == "1" || ddlRptViewPayableToAutority1.SelectedValue == "1")
                {
                    ViewState["ShowDate"] = "True";
                }
                else
                {
                    ViewState["ShowDate"] = "False";
                }

                string Segment = null;
                if (Session["ExchangeSegmentID"].ToString() == "1" || Session["ExchangeSegmentID"].ToString() == "2")
                {
                    Segment = "NSE";
                }

                else if (Session["ExchangeSegmentID"].ToString() == "4" || Session["ExchangeSegmentID"].ToString() == "5")
                {
                    Segment = "BSE";
                }


                ViewState["billprintdate"] = oconverter.ArrangeDate2(dtFrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtTo.Value.ToString());
                string DateParameter = ViewState["billprintdate"].ToString();

                //ds.Tables[0].WriteXmlSchema("E:\\RPTXSD\\StampDuty.xsd");
                //ds.Tables[1].WriteXmlSchema("E:\\RPTXSD\\StampDuty1.xsd");
                string path = HttpContext.Current.Server.MapPath("..\\Reports\\StampDuty.rpt");
                ReportDocument.Load(path);
                ReportDocument.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
                ReportDocument.SetDataSource(ds.Tables[0]);
                ReportDocument.Subreports["StampDutyHeader"].SetDataSource(ds.Tables[1]);
                ReportDocument.SetParameterValue("@DateFormat", (object)DateParameter);
                ReportDocument.SetParameterValue("@SegmentName", (object)Segment);
                ReportDocument.SetParameterValue("@ShowDate", (object)ViewState["ShowDate"]);
                ReportDocument.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
                ReportDocument.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "StampDuty Register");
            }

        }

        void FnHtmlAcrossSegment(DataSet ds)
        {
            //////////For header
            String strHtmlheader = String.Empty;
            string str = null;
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                str = "[" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "]";
            }
            str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report Period " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtTo.Value.ToString());
            str = str + " ; Report View [Charge To Client] :" + ddlRptViewChargeToClient.SelectedItem.Text.ToString().Trim();

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
                        if (dr1[j].ToString().Trim().StartsWith("Grand") || dr1[j].ToString().Trim().StartsWith("Total") || dr1[j].ToString().Trim().StartsWith("User-Id") || dr1[j].ToString().Trim().StartsWith("Branch/Group Name") || dr1[j].ToString().Trim().StartsWith("Net Total") || dr1[j].ToString().Trim().StartsWith("Client Name") || dr1[j].ToString().Trim().StartsWith("Month Name") || dr1[j].ToString().Trim().StartsWith("Instrument"))
                        {
                            strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\"><b>" + dr1[j] + "</b></td>";
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
            display.InnerHtml = strHtmlheader + strHtml;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "DISPLAY", "DISPLAY('2');", true);

        }
        public static bool IsNumeric(object value)
        {
            double dbl;
            return double.TryParse(value.ToString(), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out dbl);
        }
        void ExportAcrossSegment(DataSet ds)
        {
            DataTable dtExport = ds.Tables[0].Copy();
            string str = null;
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                str = "[" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "]";
            }
            str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report Period " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtTo.Value.ToString());
            str = str + " ; Report View [Charge To Client] :" + ddlRptViewChargeToClient.SelectedItem.Text.ToString().Trim();


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

            objExcel.ExportToExcelforExcel(dtExport, "Stamp Duty Statement", "Total:", dtReportHeader, dtReportFooter);

        }
        void ExportAcrossSegment1()
        {
            decimal cmtot = decimal.Zero;
            decimal fotot = decimal.Zero;
            decimal pay = decimal.Zero;
            DataTable dt = new DataTable();
            ds = (DataSet)ViewState["dataset"];
            DataTable dtnew = ds.Tables[0];
            dtExport = new DataTable();
            dtExport.Clear();
            dtExport.Columns.Add("Date", Type.GetType("System.String"));
            dtExport.Columns.Add("State Name", Type.GetType("System.String"));
            dtExport.Columns.Add("Particulars Of Trade", Type.GetType("System.String"));
            dtExport.Columns.Add("Cash Market Turnover", Type.GetType("System.String"));
            dtExport.Columns.Add("Derivatives Market Turnover", Type.GetType("System.String"));
            dtExport.Columns.Add("Stamp-Duty Rate (%)", Type.GetType("System.String"));
            dtExport.Columns.Add("Stamp-Duty Payable", Type.GetType("System.String"));
            dtnew.DefaultView.ToTable(true, "TYPE");
            DataView viewgroup = new DataView(ds.Tables[0]);
            dtgroupcontactid = viewgroup.ToTable(true, new string[] { "TYPE" });
            dt = ds.Tables[0].Clone();
            for (int j = 0; j < dtgroupcontactid.Rows.Count; j++)
            {

                DataView viewMAHA = new DataView();
                viewMAHA = ds.Tables[0].DefaultView;
                viewMAHA.RowFilter = "TYPE= '" + dtgroupcontactid.Rows[j][0] + "'";
                dt = viewMAHA.ToTable();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow row1 = dtExport.NewRow();
                    row1[0] = dt.Rows[i]["DATE1"].ToString();
                    row1[1] = dt.Rows[i]["TYPE"].ToString();
                    row1[2] = dt.Rows[i]["RECORDTYPE"].ToString();
                    row1[3] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt.Rows[i]["CMTOT"].ToString()));
                    row1[4] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt.Rows[i]["FOTOT"].ToString()));
                    row1[5] = dt.Rows[i]["MAHACLEINTSQRSTAMPCM"].ToString();
                    row1[6] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt.Rows[i]["PABLE"].ToString()));
                    dtExport.Rows.Add(row1);

                    cmtot = cmtot + Convert.ToDecimal(dt.Rows[i]["CMTOT"].ToString());
                    fotot = fotot + Convert.ToDecimal(dt.Rows[i]["FOTOT"].ToString());
                    pay = pay + Convert.ToDecimal(dt.Rows[i]["PABLE"].ToString());



                }
                DataRow rowmahasum = dtExport.NewRow();
                rowmahasum[0] = "Total";
                rowmahasum[3] = oconverter.formatmoneyinUs(Convert.ToDecimal(cmtot));
                rowmahasum[4] = oconverter.formatmoneyinUs(Convert.ToDecimal(fotot));
                rowmahasum[6] = oconverter.formatmoneyinUs(Convert.ToDecimal(pay));
                dtExport.Rows.Add(rowmahasum);
                cmtot = decimal.Zero;
                fotot = decimal.Zero;
                pay = decimal.Zero;
            }
            ViewState["EXPORTSUMMARY"] = dtExport;
            headerfotter();

        }


        void ExportAcrossSegment2()
        {
            decimal cmtot = decimal.Zero;
            decimal fotot = decimal.Zero;
            decimal pay = decimal.Zero;
            DataTable dt = new DataTable();
            ds = (DataSet)ViewState["dataset"];
            DataTable dtnew = ds.Tables[0];
            dtExport = new DataTable();
            dtExport.Clear();
            //dtExport.Columns.Add("Date", Type.GetType("System.String"));
            dtExport.Columns.Add("State Name", Type.GetType("System.String"));
            dtExport.Columns.Add("Particulars Of Trade", Type.GetType("System.String"));
            dtExport.Columns.Add("Cash Market Turnover", Type.GetType("System.String"));
            dtExport.Columns.Add("Derivatives Market Turnover", Type.GetType("System.String"));
            dtExport.Columns.Add("Stamp-Duty Rate (%)", Type.GetType("System.String"));
            dtExport.Columns.Add("Stamp-Duty Payable", Type.GetType("System.String"));
            dtnew.DefaultView.ToTable(true, "TYPE");
            DataView viewgroup = new DataView(ds.Tables[0]);
            dtgroupcontactid = viewgroup.ToTable(true, new string[] { "TYPE" });
            dt = ds.Tables[0].Clone();
            for (int j = 0; j < dtgroupcontactid.Rows.Count; j++)
            {

                DataView viewMAHA = new DataView();
                viewMAHA = ds.Tables[0].DefaultView;
                viewMAHA.RowFilter = "TYPE= '" + dtgroupcontactid.Rows[j][0] + "'";
                dt = viewMAHA.ToTable();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow row1 = dtExport.NewRow();
                    //row1[0] = dt.Rows[i]["DATE1"].ToString();
                    row1[0] = dt.Rows[i]["TYPE"].ToString();
                    row1[1] = dt.Rows[i]["RECORDTYPE"].ToString();
                    row1[2] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt.Rows[i]["CMTOT"].ToString()));
                    row1[3] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt.Rows[i]["FOTOT"].ToString()));
                    row1[4] = dt.Rows[i]["MAHACLEINTSQRSTAMPCM"].ToString();
                    row1[5] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt.Rows[i]["PABLE"].ToString()));
                    dtExport.Rows.Add(row1);

                    cmtot = cmtot + Convert.ToDecimal(dt.Rows[i]["CMTOT"].ToString());
                    fotot = fotot + Convert.ToDecimal(dt.Rows[i]["FOTOT"].ToString());
                    pay = pay + Convert.ToDecimal(dt.Rows[i]["PABLE"].ToString());



                }
                DataRow rowmahasum = dtExport.NewRow();
                rowmahasum[0] = "Total";
                rowmahasum[2] = oconverter.formatmoneyinUs(Convert.ToDecimal(cmtot));
                rowmahasum[3] = oconverter.formatmoneyinUs(Convert.ToDecimal(fotot));
                rowmahasum[5] = oconverter.formatmoneyinUs(Convert.ToDecimal(pay));
                dtExport.Rows.Add(rowmahasum);
                cmtot = decimal.Zero;
                fotot = decimal.Zero;
                pay = decimal.Zero;
            }
            ViewState["EXPORTSUMMARY"] = dtExport;
            headerfotter();

        }

        void ExportAcrossSegment3()
        {
            decimal cmtot = decimal.Zero;
            decimal fotot = decimal.Zero;
            decimal pay = decimal.Zero;

            decimal cmtot1 = decimal.Zero;
            decimal fotot1 = decimal.Zero;
            decimal pay1 = decimal.Zero;

            decimal cmtot2 = decimal.Zero;
            decimal fotot2 = decimal.Zero;
            decimal pay2 = decimal.Zero;
            decimal grandtotal = decimal.Zero;
            decimal grandtotal1 = decimal.Zero;
            decimal grandtotal2 = decimal.Zero;
            decimal grandtotal3 = decimal.Zero;
            decimal grandtotal4 = decimal.Zero;
            decimal grandtotal5 = decimal.Zero;
            decimal grandtotal6 = decimal.Zero;
            decimal grandtotal7 = decimal.Zero;
            decimal grandtotal8 = decimal.Zero;

            DataTable dt = new DataTable();
            ds = (DataSet)ViewState["dataset"];
            DataTable dtnew = ds.Tables[0];
            dtExport = new DataTable();
            dtExport.Clear();

            dtExport.Columns.Add("State/Client", Type.GetType("System.String"));

            dtExport.Columns.Add("Dlivery  Turnover", Type.GetType("System.String"));

            dtExport.Columns.Add("Stamp- Duty Rate (%)", Type.GetType("System.String"));
            dtExport.Columns.Add("Stamp- Duty Payable", Type.GetType("System.String"));
            dtExport.Columns.Add("Non-Delivery Turnover", Type.GetType("System.String"));
            dtExport.Columns.Add("Stamp -Duty Rate (%)", Type.GetType("System.String"));
            dtExport.Columns.Add("Stamp -Duty Payable", Type.GetType("System.String"));
            dtExport.Columns.Add("Derivatives  Market  Turnover", Type.GetType("System.String"));
            dtExport.Columns.Add("Stamp-Duty Rate (%)", Type.GetType("System.String"));
            dtExport.Columns.Add("Stamp-Duty Payable", Type.GetType("System.String"));
            dtExport.Columns.Add("Total Stamp Duty", Type.GetType("System.String"));
            dtnew.DefaultView.ToTable(true, "TYPE");
            DataView viewgroup = new DataView(ds.Tables[0]);
            dtgroupcontactid = viewgroup.ToTable(true, new string[] { "TYPE" });
            dt = ds.Tables[0].Clone();
            for (int j = 0; j < dtgroupcontactid.Rows.Count; j++)
            {

                DataView viewMAHA = new DataView();
                viewMAHA = ds.Tables[0].DefaultView;
                viewMAHA.RowFilter = "TYPE= '" + dtgroupcontactid.Rows[j][0] + "'";
                dt = viewMAHA.ToTable();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        DataRow row2 = dtExport.NewRow();
                        row2[0] = dt.Rows[i]["TYPE"].ToString();
                        dtExport.Rows.Add(row2);
                    }
                    //dtExport.Rows.Clear();

                    DataRow row1 = dtExport.NewRow();
                    row1[0] = dt.Rows[i]["name"].ToString();
                    row1[1] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt.Rows[i]["CMTOT2"].ToString()));
                    row1[2] = dt.Rows[i]["MAHACLEINTSQRSTAMPCM2"].ToString();
                    row1[3] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt.Rows[i]["PABLE2"].ToString()));
                    row1[4] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt.Rows[i]["CMTOT1"].ToString()));
                    row1[5] = dt.Rows[i]["MAHACLEINTSQRSTAMPCM1"].ToString();
                    row1[6] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt.Rows[i]["PABLE"].ToString()));
                    row1[7] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt.Rows[i]["FOTOT1"].ToString()));
                    row1[8] = dt.Rows[i]["MAHACLEINTSQRSTAMPFO1"].ToString();
                    row1[9] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt.Rows[i]["PABLE1"].ToString()));




                    dtExport.Rows.Add(row1);

                    cmtot = cmtot + Convert.ToDecimal(dt.Rows[i]["CMTOT2"].ToString());
                    //fotot = fotot + Convert.ToDecimal(dt.Rows[i]["FOTOT2"].ToString());
                    pay = pay + Convert.ToDecimal(dt.Rows[i]["PABLE2"].ToString());

                    cmtot1 = cmtot1 + Convert.ToDecimal(dt.Rows[i]["CMTOT1"].ToString());
                    //fotot1 = fotot1 + Convert.ToDecimal(dt.Rows[i]["FOTOT1"].ToString());
                    pay1 = pay1 + Convert.ToDecimal(dt.Rows[i]["PABLE"].ToString());

                    cmtot2 = cmtot2 + Convert.ToDecimal(dt.Rows[i]["FOTOT1"].ToString());
                    //fotot2 = fotot2 + Convert.ToDecimal(dt.Rows[i]["FOTOT"].ToString());
                    pay2 = pay2 + Convert.ToDecimal(dt.Rows[i]["PABLE1"].ToString());


                }
                DataRow rowmahasum = dtExport.NewRow();
                rowmahasum[0] = "Total";
                rowmahasum[1] = oconverter.formatmoneyinUs(Convert.ToDecimal(cmtot));
                //rowmahasum[3] = oconverter.formatmoneyinUs(Convert.ToDecimal(fotot));
                rowmahasum[3] = oconverter.formatmoneyinUs(Convert.ToDecimal(pay));

                rowmahasum[4] = oconverter.formatmoneyinUs(Convert.ToDecimal(cmtot1));
                // rowmahasum[8] = oconverter.formatmoneyinUs(Convert.ToDecimal(fotot1));
                rowmahasum[6] = oconverter.formatmoneyinUs(Convert.ToDecimal(pay1));

                rowmahasum[7] = oconverter.formatmoneyinUs(Convert.ToDecimal(cmtot2));
                //rowmahasum[13] = oconverter.formatmoneyinUs(Convert.ToDecimal(fotot2));
                rowmahasum[9] = oconverter.formatmoneyinUs(Convert.ToDecimal(pay2));
                DataRow rowmahasum1 = dtExport.NewRow();
                rowmahasum[10] = oconverter.formatmoneyinUs(Convert.ToDecimal(pay + pay1 + pay2));

                grandtotal = grandtotal + cmtot;
                grandtotal1 = grandtotal1 + pay;
                grandtotal2 = grandtotal2 + cmtot1;
                grandtotal3 = grandtotal3 + pay1;
                grandtotal4 = grandtotal4 + cmtot2;
                grandtotal5 = grandtotal5 + pay2;
                grandtotal6 = grandtotal6 + (pay + pay1 + pay2);
                //totalto = oconverter.formatmoneyinUs(Convert.ToDecimal(rowmahasum[1]));

                ////grandtotal7 = grandtotal7 + cmtot;
                ////grandtotal8 = grandtotal8 + cmtot;




                dtExport.Rows.Add(rowmahasum);
                cmtot = decimal.Zero;
                fotot = decimal.Zero;
                pay = decimal.Zero;

                cmtot1 = decimal.Zero;
                fotot1 = decimal.Zero;
                pay1 = decimal.Zero;

                cmtot2 = decimal.Zero;
                fotot2 = decimal.Zero;
                pay2 = decimal.Zero;
            }
            DataRow rowmahasum2 = dtExport.NewRow();
            rowmahasum2[0] = "Grand Total";
            rowmahasum2[1] = oconverter.formatmoneyinUs(Convert.ToDecimal(grandtotal));
            //rowmahasum[3] = oconverter.formatmoneyinUs(Convert.ToDecimal(fotot));
            rowmahasum2[3] = oconverter.formatmoneyinUs(Convert.ToDecimal(grandtotal1));

            rowmahasum2[4] = oconverter.formatmoneyinUs(Convert.ToDecimal(grandtotal2));
            // rowmahasum[8] = oconverter.formatmoneyinUs(Convert.ToDecimal(fotot1));
            rowmahasum2[6] = oconverter.formatmoneyinUs(Convert.ToDecimal(grandtotal3));

            rowmahasum2[7] = oconverter.formatmoneyinUs(Convert.ToDecimal(grandtotal4));
            //rowmahasum[13] = oconverter.formatmoneyinUs(Convert.ToDecimal(fotot2));
            rowmahasum2[9] = oconverter.formatmoneyinUs(Convert.ToDecimal(grandtotal5));
            rowmahasum2[10] = oconverter.formatmoneyinUs(Convert.ToDecimal(grandtotal6));
            dtExport.Rows.Add(rowmahasum2);
            ViewState["EXPORTSUMMARY"] = dtExport;
            headerfotter();

        }

        void export_ChargeToClient_GiveBranchTotal1()
        {

            ds = (DataSet)ViewState["dataset"];
            dtExport = new DataTable();
            dtExport.Clear();
            dtExport.Columns.Add("Client Name", Type.GetType("System.String"));
            dtExport.Columns.Add("UCC", Type.GetType("System.String"));
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "1")
            {
                if (ViewState["DlvTO"].ToString().Trim() == "Y")
                {
                    dtExport.Columns.Add("Dlv TO", Type.GetType("System.String"));
                }
                if (ViewState["SqrTO"].ToString().Trim() == "Y")
                {
                    dtExport.Columns.Add("Sqr TO", Type.GetType("System.String"));
                }
            }
            else
            {
                if (ViewState["FUTTO"].ToString().Trim() == "Y")
                {
                    dtExport.Columns.Add("FUT TO", Type.GetType("System.String"));
                }
                if (ViewState["PRMTO"].ToString().Trim() == "Y")
                {
                    dtExport.Columns.Add("PRM TO", Type.GetType("System.String"));
                }
                if (ViewState["FinnSettTO"].ToString().Trim() == "Y")
                {
                    dtExport.Columns.Add("Finn.Sett TO", Type.GetType("System.String"));
                }
            }
            if (ViewState["TotalTO"].ToString().Trim() == "Y")
            {
                dtExport.Columns.Add("Total TO", Type.GetType("System.String"));
            }
            if (ViewState["StampDuty"].ToString().Trim() == "Y")
            {
                dtExport.Columns.Add("Stamp Duty", Type.GetType("System.String"));
            }


            DataView viewGRP = new DataView(ds.Tables[0]);
            DataTable DistinctGRP = viewGRP.ToTable(true, new string[] { "state" });
            DataTable dtnew = new DataTable();
            //dtnew.DefaultView.ToTable(true, "state");
            //DataView viewgroup = new DataView(ds.Tables[0]);
            // dtgroupcontactid = viewgroup.ToTable(true, new string[] { "state" });
            //DataTable dt9 = new DataTable();
            //dt9 = ds.Tables[0].Clone();
            if (DistinctGRP.Rows.Count > 0)
            {
                cmbGROUP.DataSource = DistinctGRP;
                cmbGROUP.DataValueField = "state";
                cmbGROUP.DataTextField = "state";
                cmbGROUP.DataBind();
            }

            decimal dlvto_sum = decimal.Zero;
            decimal sqrto_sum = decimal.Zero;
            decimal total_sum = decimal.Zero;
            decimal stamp_sum = decimal.Zero;
            decimal fsto_sum = decimal.Zero;

            decimal dlvto_grpsum = decimal.Zero;
            decimal sqrto_grpsum = decimal.Zero;
            decimal total_grpsum = decimal.Zero;
            decimal stamp_grpsum = decimal.Zero;
            decimal fsto_grpsum = decimal.Zero;

            int cmbgrp_count = cmbGROUP.Items.Count;
            for (int i = 0; i < cmbgrp_count; i++)
            {
                dlvto_sum = decimal.Zero;
                sqrto_sum = decimal.Zero;
                total_sum = decimal.Zero;
                stamp_sum = decimal.Zero;
                fsto_sum = decimal.Zero;

                string valItem = cmbGROUP.Items[i].Value;
                DataView viewgrp = new DataView();
                viewgrp = ds.Tables[0].DefaultView;
                viewgrp.RowFilter = "state='" + valItem + "'";
                DataTable dt = new DataTable();
                dt = viewgrp.ToTable();

                DataRow row = dtExport.NewRow();
                row["Client Name"] = ddlGroup.SelectedItem.Text.ToString() + " : " + cmbGROUP.Items[i].Text.ToString().Trim();
                row["UCC"] = "Test";
                dtExport.Rows.Add(row);

                foreach (DataRow dr1 in dt.Rows)
                {
                    DataRow row2 = dtExport.NewRow();
                    row2["Client Name"] = dr1["CLIENTNAME"].ToString();
                    row2["UCC"] = dr1["UCC"].ToString();

                    if (ViewState["DlvTO"].ToString().Trim() == "Y" || ViewState["FUTTO"].ToString().Trim() == "Y")
                    {
                        if (dr1["DelFutTO"] != DBNull.Value)
                        {
                            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2")
                            {
                                row2["FUT TO"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dr1["DelFutTO"]));
                            }
                            else
                            {
                                row2["Dlv TO"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dr1["DelFutTO"]));
                            }
                            dlvto_sum = dlvto_sum + Convert.ToDecimal(dr1["DelFutTO"]);
                            dlvto_grpsum = dlvto_grpsum + Convert.ToDecimal(dr1["DelFutTO"]);
                        }

                    }
                    if (ViewState["SqrTO"].ToString().Trim() == "Y" || ViewState["PRMTO"].ToString().Trim() == "Y")
                    {
                        if (dr1["SqrOptPrmTO"] != DBNull.Value)
                        {
                            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2")
                            {
                                row2["PRM TO"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dr1["SqrOptPrmTO"]));
                            }
                            else
                            {
                                row2["Sqr TO"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dr1["SqrOptPrmTO"]));
                            }
                            sqrto_sum = sqrto_sum + Convert.ToDecimal(dr1["SqrOptPrmTO"].ToString());
                            sqrto_grpsum = sqrto_grpsum + Convert.ToDecimal(dr1["SqrOptPrmTO"].ToString());
                        }
                    }
                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2")
                    {
                        if (ViewState["FinnSettTO"].ToString().Trim() == "Y")
                        {
                            if (dr1["FSTO"] != DBNull.Value)
                            {
                                row2["Finn.Sett TO"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dr1["FSTO"]));
                                fsto_sum = fsto_sum + Convert.ToDecimal(dr1["FSTO"].ToString());
                                fsto_grpsum = fsto_grpsum + Convert.ToDecimal(dr1["FSTO"].ToString());
                            }

                        }
                    }
                    if (ViewState["TotalTO"].ToString().Trim() == "Y")
                    {
                        if (dr1["TotalTO"] != DBNull.Value)
                        {
                            row2["Total TO"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dr1["TotalTO"]));
                            total_sum = total_sum + Convert.ToDecimal(dr1["TotalTO"].ToString());
                            total_grpsum = total_grpsum + Convert.ToDecimal(dr1["TotalTO"].ToString());
                        }
                    }
                    if (ViewState["StampDuty"].ToString().Trim() == "Y")
                    {
                        if (dr1["StampDuty"] != DBNull.Value)
                        {
                            row2["Stamp Duty"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dr1["StampDuty"]));
                            stamp_sum = stamp_sum + Convert.ToDecimal(dr1["StampDuty"].ToString());
                            stamp_grpsum = stamp_grpsum + Convert.ToDecimal(dr1["StampDuty"].ToString());
                        }
                    }
                    dtExport.Rows.Add(row2);
                }

                DataRow row11 = dtExport.NewRow();
                row11["Client Name"] = "Total";
                if (ViewState["DlvTO"].ToString().Trim() == "Y" || ViewState["FUTTO"].ToString().Trim() == "Y")
                {
                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2")
                    {
                        row11["FUT TO"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dlvto_sum));
                    }
                    else
                    {
                        row11["Dlv TO"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dlvto_sum));
                    }

                }
                if (ViewState["SqrTO"].ToString().Trim() == "Y" || ViewState["PRMTO"].ToString().Trim() == "Y")
                {
                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2")
                    {
                        row11["PRM TO"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(fsto_sum));
                    }
                    else
                    {
                        row11["Sqr TO"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(fsto_sum));
                    }
                }
                if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2")
                {
                    if (ViewState["FinnSettTO"].ToString().Trim() == "Y")
                    {
                        row11["Finn.Sett TO"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(fsto_sum));
                    }
                }
                if (ViewState["TotalTO"].ToString().Trim() == "Y")
                {
                    row11["Total TO"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(total_sum));
                }
                if (ViewState["StampDuty"].ToString().Trim() == "Y")
                {
                    row11["Stamp Duty"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(stamp_sum));
                }
                dtExport.Rows.Add(row11);
            }
            /////Grand Total
            DataRow row13 = dtExport.NewRow();
            dtExport.Rows.Add(row13);


            DataRow row12 = dtExport.NewRow();
            row12["Client Name"] = "Grand Total :";
            if (ViewState["DlvTO"].ToString().Trim() == "Y" || ViewState["FUTTO"].ToString().Trim() == "Y")
            {
                if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2")
                {
                    row12["FUT TO"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dlvto_grpsum));
                }
                else
                {
                    row12["Dlv TO"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dlvto_grpsum));
                }

            }
            if (ViewState["SqrTO"].ToString().Trim() == "Y" || ViewState["PRMTO"].ToString().Trim() == "Y")
            {
                if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2")
                {
                    row12["PRM TO"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(fsto_grpsum));
                }
                else
                {
                    row12["Sqr TO"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(fsto_grpsum));
                }
            }
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2")
            {
                if (ViewState["FinnSettTO"].ToString().Trim() == "Y")
                {
                    row12["Finn.Sett TO"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(fsto_grpsum));
                }
            }
            if (ViewState["TotalTO"].ToString().Trim() == "Y")
            {
                row12["Total TO"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(total_grpsum));
            }
            if (ViewState["StampDuty"].ToString().Trim() == "Y")
            {
                row12["Stamp Duty"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(stamp_grpsum));
            }
            dtExport.Rows.Add(row12);

            if (dtExport.Columns.Count < 4)
            {
                dtExport.Columns.Add("Remarks", Type.GetType("System.String"));
            }
            ViewState["EXPORTSUMMARY"] = dtExport;
            headerfotter();
        }
    }
}