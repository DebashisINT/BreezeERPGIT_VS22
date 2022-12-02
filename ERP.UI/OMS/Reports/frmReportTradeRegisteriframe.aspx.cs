using System;
using System.Collections;
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
    public partial class Reports_frmReportComTradeRegisteriframe : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        # region Local variable
        string reportType = null;
        string exportType = null;
        DataTable dtClients = new DataTable();
        string data;
        string bindParams = null;
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        DataSet ds = new DataSet();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        int pageindex = 0;
        ReportDocument ReportDocument = new ReportDocument();
        string[] connPath = ConfigurationManager.AppSettings["DBConnectionDefault"].Split(';');
        GenericMethod oGenericMethod;
        DailyReports dailyrep = new DailyReports();
        # endregion

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);

                string[] PageSession = { null };
                oGenericMethod = new GenericMethod();
                oGenericMethod.PageInitializer(GenericMethod.WhichCall.DistroyUnWantedSession_AllExceptPage, PageSession);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");
            DataTable dtname = oDBEngine.GetDataTable(" tbl_master_company  ", "cmp_Name", "cmp_internalid='" + HttpContext.Current.Session["LastCompany"] + "' ");
            ViewState["CompanyName"] = dtname.Rows[0]["cmp_Name"].ToString();
            if (!IsPostBack)
            {
                ddlbind();
                if (Request.QueryString["Custid"] == null)
                {
                    settno();
                    //date();
                    dtFrom.EditFormatString = oconverter.GetDateFormat("Date");
                    dtTo.EditFormatString = oconverter.GetDateFormat("Date");

                    DateTime fromdate = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
                    string dd1 = (HttpContext.Current.Session["LastFinYear"].ToString());
                    string[] dd2 = dd1.Split('-');
                    int year = Convert.ToInt32(dd2[1].ToString().Trim());
                    DateTime startDate = new DateTime(year, 3, 31); // 1st Feb this year
                    if (fromdate >= startDate)
                    {
                        dtFrom.Value = Convert.ToDateTime(startDate.ToShortDateString());
                        dtTo.Value = Convert.ToDateTime(startDate.ToShortDateString());
                    }
                    else
                    {
                        dtFrom.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
                        dtTo.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
                    }
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "JScriptDemat", "<script language='javascript'>ForDamat();</script>");
                    DataTable DTSettDate = oDBEngine.GetDataTable("master_settlements", " settlements_startdatetime,settlements_enddatetime", " settlements_number='" + Request.QueryString["SettNo"].ToString() + "' and settlements_typesuffix='" + Request.QueryString["SettType"].ToString() + "'");
                    dtFrom.Value = Convert.ToDateTime(DTSettDate.Rows[0][0].ToString());
                    dtTo.Value = Convert.ToDateTime(DTSettDate.Rows[0][1].ToString());

                    reportType = "2";
                    exportType = "Screen";
                    procedure(reportType, exportType);
                }
            }

            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //___________-end here___//       
        }

        void FilterColumnCheck()
        {
            string Terminalid = "N";
            string TradeCode = "N";
            string OrderNo = "N";
            string OrderEntryTime = "N";
            string TradeNo = "N";
            string TradeEntryTime = "N";
            string CntrNo = "N";
            int FilterCount = 0;
            /////////Filter Column Begin
            foreach (ListItem listitem in ChkFilterDetail.Items)
            {
                if (listitem.Selected)
                {
                    if (listitem.Value == "Terminalid")
                    {
                        Terminalid = "Y";
                        FilterCount = FilterCount + 1;
                    }
                    if (listitem.Value == "TradeCode")
                    {
                        TradeCode = "Y";
                        FilterCount = FilterCount + 1;
                    }
                    if (listitem.Value == "OrderNo")
                    {
                        OrderNo = "Y";
                        FilterCount = FilterCount + 1;
                    }
                    if (listitem.Value == "OrderEntryTime")
                    {
                        OrderEntryTime = "Y";
                        FilterCount = FilterCount + 1;
                    }
                    if (listitem.Value == "TradeNo")
                    {
                        TradeNo = "Y";
                        FilterCount = FilterCount + 1;
                    }
                    if (listitem.Value == "TradeEntryTime")
                    {
                        TradeEntryTime = "Y";
                        FilterCount = FilterCount + 1;
                    }
                    if (listitem.Value == "CntrNo")
                    {
                        CntrNo = "Y";
                        FilterCount = FilterCount + 1;
                    }
                }
            }
            ////////Filter Column End       

            ///////SPCall
            bindParams = Terminalid.ToString().Trim() + '~' + TradeCode.ToString().Trim() + '~' + OrderNo.ToString().Trim() + '~' + OrderEntryTime.ToString().Trim() + '~' + TradeNo.ToString().Trim() + '~' + TradeEntryTime.ToString().Trim() + '~' + CntrNo.ToString().Trim();

        }

        void settno()
        {
            DataTable DtSegComp = oDBEngine.GetDataTable("(select exch_compId,exch_internalId,isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Comp from tbl_master_companyExchange where exch_compid='" + Session["LastCompany"].ToString() + "') as D", "*", "Comp in(select seg_name from tbl_master_segment where seg_id=" + Session["userlastsegment"].ToString() + ")");
            if (DtSegComp.Rows.Count > 0)
            {
                litSegment.InnerText = DtSegComp.Rows[0][2].ToString(); ///Segment disply within braket
                HiddenField_Segment.Value = DtSegComp.Rows[0][1].ToString();
            }
        }



        void ddlbind()
        {
            ddlTradeType.Items.Insert(0, new ListItem("All Types", "3"));
            ddlTradeType.Items.Insert(1, new ListItem("Only Confirmed", "2"));
            ddlTradeType.Items.Insert(2, new ListItem("Show Only Zero Brkg Trades", "4"));
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
                    if ((idlist[0] != "Clients") && (idlist[0] != "Broker"))
                    {
                        if (idlist[0] == "SettlementType")
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
                    else
                    {
                        string[] val = cl[i].Split(';');
                        string[] AcVal = val[0].Split('-');
                        if (str == "")
                        {
                            if (idlist[0] == "MAILEMPLOYEE")
                            {
                                str = AcVal[0];
                            }
                            else
                            {
                                str = "'" + AcVal[0] + "'";
                                str1 = "'" + AcVal[0] + "'" + ";" + val[1];
                            }
                        }
                        else
                        {
                            if (idlist[0] == "MAILEMPLOYEE")
                            {
                                str += "," + AcVal[0];
                            }
                            else
                            {
                                str += ",'" + AcVal[0] + "'";
                                str1 += "," + "'" + AcVal[0] + "'" + ";" + val[1];
                            }
                        }
                    }
                }
                if (idlist[0] == "Branch")
                {
                    data = "Branch~" + str;
                }
                else if (idlist[0] == "Group")
                {
                    data = "Group~" + str;
                }
                else if (idlist[0] == "BranchGroup")
                {
                    data = "BranchGroup~" + str;
                }
                else if (idlist[0] == "Clients")
                {
                    data = "Clients~" + str;
                }
                else if (idlist[0] == "Broker")
                {
                    data = "Broker~" + str;
                }
                else if (idlist[0] == "Segment")
                {
                    data = "Segment~" + str;
                }
                else if (idlist[0] == "ScripsExchange")
                {
                    data = "ScripsExchange~" + str;
                }
                else if (idlist[0] == "SettlementNo")
                {
                    data = "SettlementNo~" + str;
                }
                else if (idlist[0] == "SettlementType")
                {
                    data = "SettlementType~" + str;
                }
                else if (idlist[0] == "MAILEMPLOYEE")
                {
                    data = "MAILTOEMPLOYEE~" + str;
                }
            }
        }

        # region Bind Group Type
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
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                if (HiddenField_Group.Value.ToString().Trim() == "")
                {
                    BindGroup();
                }
            }
        }
        # endregion

        void SPCALL(string rptType, string expType)
        {
            if (rdbTerminalSpecific.Checked)
            {
                if (txtTerminal_hidden.Text.ToString().Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertshow", "alertshow('Please Select Terminal Id !!');", true);
                }
                else
                {
                    procedure(rptType, expType);
                }
            }
            else if (rdbCTCLSpecific.Checked)
            {
                if (txtCtCLID_hidden.Text.ToString().Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertshow", "alertshow('Please Select CTCL Id !!');", true);
                }
                else
                {
                    procedure(rptType, expType);
                }
            }
            else
            {
                procedure(rptType, expType);
            }
        }

        void procedure(string reportType, string exportType)
        {
            string filterParameter = "Y~Y~Y~Y~Y~Y~Y";
            if (reportType == "3" && exportType == "Excel")
            {
                FilterColumnCheck();
                filterParameter = bindParams.ToString().Trim();
            }
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            {
                if (Request.QueryString["Custid"] != null)
                {
                    HiddenField_Client.Value = "'" + Request.QueryString["Custid"].ToString().Trim() + "'";
                    rdbranchclientSelected.Checked = true;
                    rdbranchclientAll.Checked = false;
                    HiddenField_Instrument.Value = Request.QueryString["ProdID"].ToString().Trim();
                    rdInstrumentSelected.Checked = true;
                    rdInstrumentAll.Checked = false;
                    DataTable dtSeg = oDBEngine.GetDataTable("Trans_DematPosition", "DematPosition_SegmentID", " DematPosition_ID=" + Request.QueryString["DematID"].ToString() + "");
                    HiddenField_Segment.Value = dtSeg.Rows[0][0].ToString();
                    rdbSegSelected.Checked = true;
                    rdbSegAll.Checked = false;
                }


                string clients = "";
                string Broker = "";
                if (ddlGroup.SelectedItem.Value.ToString() == "3")/////group type client selection
                {
                    if (rdbranchclientAll.Checked)
                    {
                        clients = "All";
                        Broker = "CL";
                    }
                    else
                    {
                        clients = HiddenField_Client.Value.ToString().Trim();
                        Broker = "CL";
                    }
                }


                else if (ddlGroup.SelectedItem.Value.ToString() == "4")
                {
                    if (rdbranchclientAll.Checked)
                    {
                        clients = "All";
                        Broker = "BO";
                    }
                    else
                    {
                        clients = HiddenField_Broker.Value.ToString().Trim();
                        Broker = "BO";
                    }
                }
                else
                {
                    clients = "All";
                    Broker = "CL";
                }

                string instruments = "";
                string segment = "";
                string grptype = "";
                string grp = "";
                if (rdInstrumentAll.Checked)
                {
                    instruments = "All";
                }
                else
                {
                    instruments = HiddenField_Instrument.Value.ToString().Trim();
                }
                if (rdbSegAll.Checked)
                {
                    segment = "All";
                }
                else
                {
                    segment = HiddenField_Segment.Value.ToString().Trim();
                }
                if (ddlGroup.SelectedItem.Value.ToString() == "0")/////group type branch selection
                {
                    grptype = "BRANCH";
                    if (rdbranchclientAll.Checked)
                    {
                        grp = Session["userbranchHierarchy"].ToString();
                    }
                    else
                    {
                        grp = HiddenField_Branch.Value.ToString().Trim();
                    }
                }
                else if (ddlGroup.SelectedItem.Value.ToString() == "2")/////group type branch-group selection
                {
                    grptype = "BRANCHGROUP";
                    if (rdbranchclientAll.Checked)
                    {
                        grp = "ALL";
                    }
                    else
                    {
                        grp = HiddenField_BranchGroup.Value.ToString().Trim();
                    }
                }
                else if (ddlGroup.SelectedItem.Value.ToString() == "1")/////group type group selection
                {
                    grptype = ddlgrouptype.SelectedItem.Text.ToString().Trim();
                    if (rdddlgrouptypeAll.Checked)
                    {
                        grp = "All";
                    }
                    else
                    {
                        grp = HiddenField_Group.Value.ToString().Trim();
                    }
                }
                else   /////group type client selection
                {
                    grptype = "BRANCH";
                    grp = "All";
                }
                string TERMINALID = "";
                string CTCLID = "";
                if (rdbTerminalAll.Checked)
                {
                    TERMINALID = "ALL";
                }
                else
                {
                    TERMINALID = txtTerminal_hidden.Text.ToString().Trim();
                }
                if (rdbCTCLAll.Checked)
                {
                    CTCLID = "ALL";
                }
                else
                {
                    CTCLID = txtCtCLID_hidden.Text.ToString().Trim();
                }
                string groupByFilter = "";
                if (ddlGroup.SelectedValue == "0")
                    groupByFilter = "BRANCH";
                else if (ddlGroup.SelectedValue == "1")
                    groupByFilter = "GROUP";
                else if (ddlGroup.SelectedValue == "2")
                    groupByFilter = "BRANCHGROUP";
                else if (ddlGroup.SelectedValue == "3")
                    groupByFilter = "CLIENT";
                else if (ddlGroup.SelectedValue == "4")
                    groupByFilter = "Broker";

                ds = dailyrep.Reports_ComTrade_Register(dtFrom.Value.ToString(), dtTo.Value.ToString(), ddlTradeType.SelectedItem.Value.ToString(), Session["LastCompany"].ToString(),
                    clients, Broker, instruments, segment, grptype, grp, TERMINALID, Session["userbranchHierarchy"].ToString(), CTCLID, reportType, exportType,
                    groupByFilter, ddlOrderBy.SelectedValue.ToString(), filterParameter.ToString().Trim());

                ViewState["dataset"] = ds;
                if (ds.Tables[0].Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(1);", true);
                }
                else
                {
                    if (reportType == "2")
                    {
                        ddlbandforgroup();
                        if (Request.QueryString["Custid"] != null)
                        {
                            grdTradeRegister.Columns[1].Visible = false;
                            grdTradeRegister.Columns[2].Visible = false;
                            grdTradeRegister.Columns[3].Visible = false;
                            grdTradeRegister.Columns[4].Visible = false;
                            grdTradeRegister.Columns[5].Visible = false;
                            grdTradeRegister.Columns[6].Visible = false;
                        }
                        else
                        {
                            if (dtFrom.Value.ToString().Trim() == dtTo.Value.ToString().Trim())
                            {
                                grdTradeRegister.Columns[0].Visible = false;
                            }
                            else
                            {
                                grdTradeRegister.Columns[0].Visible = true;
                            }
                            if (rdbSegAll.Checked)
                            {
                                grdTradeRegister.Columns[1].Visible = true;
                            }
                            else
                            {
                                grdTradeRegister.Columns[1].Visible = false;
                            }
                        }
                    }
                    else if ((reportType == "3") && (exportType == "Excel"))
                        ExcelExport();
                }
            }
        }

        # region Screen Generate
        protected void btnshow_Click(object sender, EventArgs e)
        {
            reportType = "2";
            exportType = "Screen";
            GROUPCHECKING(reportType, exportType);
        }
        void GROUPCHECKING(string reportType, string exportType)
        {
            if (ddlGroup.SelectedItem.Value.ToString().Trim() == "0")/////BRANCH
            {
                if ((rdbranchclientSelected.Checked == true) && (HiddenField_Branch.Value.ToString().Trim() == ""))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertshow", "alertshow('Please Select Branch !!');", true);
                }
                else
                {
                    SPCALL(reportType, exportType);
                }
            }
            if (ddlGroup.SelectedItem.Value.ToString().Trim() == "1")/////GROUP
            {
                if (HiddenField_Group.Value.ToString().Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertshow", "alertshow('Please Select Group !!');", true);
                }
                else
                {
                    SPCALL(reportType, exportType);
                }
            }
            if (ddlGroup.SelectedItem.Value.ToString().Trim() == "2")/////Branch-Group
            {
                if (HiddenField_BranchGroup.Value.ToString().Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertshow", "alertshow('Please Select Branch-Group !!');", true);
                }
                else
                {
                    SPCALL(reportType, exportType);
                }
            }
            if (ddlGroup.SelectedItem.Value.ToString().Trim() == "3")/////Clients
            {
                if (HiddenField_Client.Value.ToString().Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertshow", "alertshow('Please Select Clients !!');", true);
                }
                else
                {
                    SPCALL(reportType, exportType);
                }
            }
            if (ddlGroup.SelectedItem.Value.ToString().Trim() == "4")/////Clients
            {
                if (HiddenField_Broker.Value.ToString().Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertshow", "alertshow('Please Select Clients !!');", true);
                }
                else
                {
                    SPCALL(reportType, exportType);
                }
            }
        }
        void ddlbandforgroup()
        {
            ds = (DataSet)ViewState["dataset"];
            DataView viewclient = new DataView(ds.Tables[0]);
            dtClients = viewclient.ToTable(true, new string[] { "CustomerID", "CLIENTNAME" });
            if (dtClients.Rows.Count > 0)
            {
                cmbclient.DataSource = dtClients;
                cmbclient.DataValueField = "CustomerID";
                cmbclient.DataTextField = "CLIENTNAME";
                cmbclient.DataBind();
            }
            gridbind();
        }
        protected void NavigationLinkC_Click(Object sender, CommandEventArgs e)
        {
            int curentIndex = cmbclient.SelectedIndex;
            int totalNo = cmbclient.Items.Count;
            switch (e.CommandName)
            {
                case "First":
                    pageindex = 0;
                    break;
                case "Next":
                    curentIndex = curentIndex + 1;
                    break;
                case "Prev":
                    curentIndex = curentIndex - 1;
                    break;
                case "Last":
                    pageindex = int.Parse(TotalGrp.Value);
                    break;
                default:
                    pageindex = int.Parse(e.CommandName.ToString());
                    break;
            }
            if (curentIndex >= totalNo)
            {
                curentIndex = totalNo - 1;
                ScriptManager.RegisterStartupScript(this, GetType(), "hide", "DisableC('N');", true);
            }
            else if (curentIndex <= 0)
            {
                curentIndex = 0;
                ScriptManager.RegisterStartupScript(this, GetType(), "hide", "DisableC('P');", true);
            }
            cmbclient.SelectedIndex = curentIndex;
            gridbind();
        }
        void gridbind()
        {
            ds = (DataSet)ViewState["dataset"];
            String strHtml1 = String.Empty;
            string str = null;
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                str = "[ <b>" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + " </b>]";
            }
            str = "[ <b> Branch/Group Name Wise Report </b>]  " + str + "<span style=\"color:Blue;\"><b> Period :</b></span> " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtTo.Value.ToString()) + ";";
            if (rdbSegSelected.Checked)
            {
                str = str + "<span style=\"color:Blue;\"><b> Segmnt :</b></span> " + litSegment.InnerText.ToString().Trim() + " ;";
            }


            strHtml1 = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtml1 += "<tr><td align=\"left\" colspan=10>" + str + "</td></tr></table>";

            DIVdisplayPERIOD.InnerHtml = strHtml1;


            DataView viewclient = new DataView();
            viewclient = ds.Tables[0].DefaultView;
            viewclient.RowFilter = "CustomerID='" + cmbclient.SelectedItem.Value + "'";
            dtClients.Clear();
            dtClients = viewclient.ToTable();

            //////////NET CALCULATION

            string prv_Quantity = null;
            string Quantity = null;
            string productseries = null;
            int j;
            for (j = 0; j < dtClients.Rows.Count; j++)
            {
                Quantity = dtClients.Rows[j]["Quantity"].ToString();
                dtClients.Rows[j]["Quantity"] = DBNull.Value;
                if (productseries != null)
                {
                    if (productseries != dtClients.Rows[j]["ProductSeriesID"].ToString().Trim())
                    {
                        dtClients.Rows[j - 1]["Quantity"] = prv_Quantity;
                    }
                }
                productseries = dtClients.Rows[j]["ProductSeriesID"].ToString();
                prv_Quantity = Quantity;
            }

            dtClients.Rows[j - 1]["Quantity"] = prv_Quantity;

            grdTradeRegister.DataSource = dtClients;
            ViewState["datatable"] = dtClients;
            ds.Dispose();
            grdTradeRegister.DataBind();
            if (Request.QueryString["Custid"] == null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Display", "Display();", true);
            }

        }
        protected void cmbclient_SelectedIndexChanged(object sender, EventArgs e)
        {
            gridbind();
        }
        protected void grdTradeRegister_RowCreated(object sender, GridViewRowEventArgs e)
        {
            ds = (DataSet)ViewState["dataset"];
            string rowID = String.Empty;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                rowID = "row" + e.Row.RowIndex;
                e.Row.Attributes.Add("id", "row" + e.Row.RowIndex);
                e.Row.Attributes.Add("onclick", "ChangeRowColor(" + "'" + rowID + "','" + ds.Tables[0].Rows.Count + "'" + ")");
            }

        }
        protected void grdTradeRegister_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                dtClients = (DataTable)ViewState["datatable"];
                Label lblBought_sum = (Label)e.Row.FindControl("lblBought_sum");
                Label lblSold_sum = (Label)e.Row.FindControl("lblSold_sum");
                Label lbltotalbrkg_sum = (Label)e.Row.FindControl("lbltotalbrkg_sum");
                Label lblNetValue_sum = (Label)e.Row.FindControl("lblNetValue_sum");
                Label lblSrvTax_sum = (Label)e.Row.FindControl("lblSrvTax_sum");
                Label lblNetAmountsum = (Label)e.Row.FindControl("lblNetAmountsum");

                lblBought_sum.Text = dtClients.Rows[0]["Boughtsum"].ToString();
                lblSold_sum.Text = dtClients.Rows[0]["Soldsum"].ToString();
                lbltotalbrkg_sum.Text = dtClients.Rows[0]["TotalBrokeragesum"].ToString();
                lblNetValue_sum.Text = dtClients.Rows[0]["NetValueesum"].ToString();
                lblSrvTax_sum.Text = dtClients.Rows[0]["servicetaxsum"].ToString();
                lblNetAmountsum.Text = dtClients.Rows[0]["netamntesum"].ToString();


            }
        }
        # endregion

        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbExport.SelectedItem.Value == "E")
            {
                if (xlExport_Validation() == true)
                {
                    reportType = "3";
                    exportType = "Excel";
                    procedure(reportType, exportType);
                    if ((ds.Tables[0].Rows.Count == 0) || (ds.Tables[0].Rows[0]["TradeDate"].ToString() == "") || (ds.Tables[0].Rows[0]["TradeDate"].ToString() == "Grand Total") || (ds.Tables[0].Rows[0]["Segment"].ToString() == ""))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(1);", true);
                    }
                    else
                    {
                        ExcelExport();
                    }
                }
            }
            else if (cmbExport.SelectedItem.Value == "P")
            {
                PdfExport();
            }
        }

        #region Excel Generate
        void ExcelExport()
        {
            ds = (DataSet)ViewState["dataset"];
            DataTable dtExport = ds.Tables[0].Copy();
            if (dtExport.Rows.Count > 1)
            {
                if (!chkrawprint.Checked)
                {
                    dtExport.Columns.Remove("CustomerName");
                    dtExport.Columns.Remove("TCode");
                    dtExport.Columns.Remove("TradeDate");
                    dtExport.Columns.Remove("Scrip");
                }
                dtExport.AcceptChanges();

                GenericExcelExport oGenericExcelExport = new GenericExcelExport();
                string strDownloadFileName = "";
                string exlDateTime = oDBEngine.GetDate(113).ToString();
                string exlTime = exlDateTime.Replace(":", "");
                exlTime = exlTime.Replace(" ", "");
                string FileName = "ComTradeRegister_" + exlTime;
                strDownloadFileName = "~/Documents/";
                string[] strHead = new string[3];
                string searchCriteria = null;
                searchCriteria = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise Trade Register Period " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtTo.Value.ToString());
                if (ddlTradeType.SelectedValue != null)
                {
                    searchCriteria += ", Trading " + ddlTradeType.SelectedItem.Text.ToString().Trim();
                }
                if (ddlGroup.SelectedValue == "1")
                {
                    if (rdddlgrouptypeAll.Checked == true)
                        searchCriteria += " With All " + ddlGroup.SelectedItem.Text.ToString().Trim();
                    else if (rdddlgrouptypeSelected.Checked == true)
                        searchCriteria += " With Selected " + ddlGroup.SelectedItem.Text.ToString().Trim();
                }
                else
                {
                    if (rdbranchclientAll.Checked == true)
                        searchCriteria += " With All " + ddlGroup.SelectedItem.Text.ToString().Trim();
                    else
                        searchCriteria += " With Selected " + ddlGroup.SelectedItem.Text.ToString().Trim();
                }
                if (rdInstrumentAll.Checked == true)
                    searchCriteria += " , All Product";
                else if (rdInstrumentSelected.Checked == true)
                    searchCriteria += " , Selected Product";
                if (rdbSegAll.Checked == true)
                    searchCriteria += " , All Segment";
                else
                    searchCriteria += " , Selected Segment";
                searchCriteria += " And Order By " + ddlOrderBy.SelectedItem.Text.ToString();
                strHead[0] = exlDateTime;
                strHead[1] = searchCriteria;
                strHead[2] = "Trade Register Of " + ViewState["CompanyName"].ToString();
                string ExcelVersion = "2007";

                string[] filterColumns = bindParams.Split('~');
                string SelectTerminalid = filterColumns[0];
                string SelectTradeCode = filterColumns[1];
                string SelectOrderNo = filterColumns[2];
                string SelectOrderEntryTime = filterColumns[3];
                string SelectTradeNo = filterColumns[4];
                string SelectTradeEntryTime = filterColumns[5];
                string SelectCntrNo = filterColumns[6];

                if (!chkrawprint.Checked)
                {

                    string[] ColumnType = { "V", "V", "V", "V", "V", "V", "V", "I", "N", "V", "N", "N", "N", "V", "V", "N", "N", "N", "N", "N", "N", "V" };
                    string[] ColumnSize = { "150", "50", "28", "50", "50", "28", "50", "10", "28,0", "50", "28,0", "28,0", "28,4", "20", "20", "28,4", "18,2", "28,4", "28,2", "28,2", "28,2", "30" };
                    string[] ColumnWidthSize = { "30", "10", "10", "10", "15", "10", "15", "10", "15", "15", "15", "15", "15", "10", "10", "15", "15", "15", "15", "15", "15", "15" };

                    ArrayList ColumnTypeList = new ArrayList(ColumnType);
                    ArrayList ColumnSizeList = new ArrayList(ColumnSize);
                    ArrayList ColumnWidthSizeList = new ArrayList(ColumnWidthSize);

                    int removeCounter = 0;
                    if (SelectTerminalid == "N")
                    {
                        ColumnTypeList.RemoveAt(1);
                        ColumnSizeList.RemoveAt(1);
                        ColumnWidthSizeList.RemoveAt(1);
                        removeCounter = 1;
                    }
                    if (SelectTradeCode == "N")
                    {
                        ColumnTypeList.RemoveAt(2 - removeCounter);
                        ColumnSizeList.RemoveAt(2 - removeCounter);
                        ColumnWidthSizeList.RemoveAt(2 - removeCounter);
                        removeCounter = removeCounter + 1;
                    }
                    if (SelectOrderNo == "N")
                    {
                        ColumnTypeList.RemoveAt(3 - removeCounter);
                        ColumnSizeList.RemoveAt(3 - removeCounter);
                        ColumnWidthSizeList.RemoveAt(3 - removeCounter);
                        removeCounter = removeCounter + 1;
                    }
                    if (SelectOrderEntryTime == "N")
                    {
                        ColumnTypeList.RemoveAt(4 - removeCounter);
                        ColumnSizeList.RemoveAt(4 - removeCounter);
                        ColumnWidthSizeList.RemoveAt(4 - removeCounter);
                        removeCounter = removeCounter + 1;
                    }
                    if (SelectTradeNo == "N")
                    {
                        ColumnTypeList.RemoveAt(5 - removeCounter);
                        ColumnSizeList.RemoveAt(5 - removeCounter);
                        ColumnWidthSizeList.RemoveAt(5 - removeCounter);
                        removeCounter = removeCounter + 1;
                    }
                    if (SelectTradeEntryTime == "N")
                    {
                        ColumnTypeList.RemoveAt(6 - removeCounter);
                        ColumnSizeList.RemoveAt(6 - removeCounter);
                        ColumnWidthSizeList.RemoveAt(6 - removeCounter);
                        removeCounter = removeCounter + 1;
                    }
                    if (SelectCntrNo == "N")
                    {
                        ColumnTypeList.RemoveAt(7 - removeCounter);
                        ColumnSizeList.RemoveAt(7 - removeCounter);
                        ColumnWidthSizeList.RemoveAt(7 - removeCounter);
                        removeCounter = removeCounter + 1;
                    }

                    ColumnType = (string[])ColumnTypeList.ToArray(typeof(string));
                    ColumnSize = (string[])ColumnSizeList.ToArray(typeof(string));
                    ColumnWidthSize = (string[])ColumnWidthSizeList.ToArray(typeof(string));

                    oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, dtExport, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                }
                else
                {
                    if (ddlOrderBy.SelectedValue == "1")
                    {
                        string[] ColumnType = { "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "I", "N", "V", "N", "N", "N", "V", "V", "N", "N", "N", "N", "N", "N", "V" };
                        string[] ColumnSize = { "150", "15", "150", "15", "150", "50", "28", "50", "50", "28", "50", "10", "28,0", "50", "28,0", "28,0", "28,4", "20", "20", "28,4", "18,2", "28,4", "28,2", "28,2", "28,2", "30" };
                        string[] ColumnWidthSize = { "30", "10", "30", "10", "30", "10", "10", "10", "15", "10", "15", "10", "15", "15", "15", "15", "15", "10", "10", "15", "15", "15", "15", "15", "15", "15" };

                        ArrayList ColumnTypeList = new ArrayList(ColumnType);
                        ArrayList ColumnSizeList = new ArrayList(ColumnSize);
                        ArrayList ColumnWidthSizeList = new ArrayList(ColumnWidthSize);

                        int removeCounter = 0;
                        if (SelectTerminalid == "N")
                        {
                            ColumnTypeList.RemoveAt(5);
                            ColumnSizeList.RemoveAt(5);
                            ColumnWidthSizeList.RemoveAt(5);
                            removeCounter = 1;
                        }
                        if (SelectTradeCode == "N")
                        {
                            ColumnTypeList.RemoveAt(6 - removeCounter);
                            ColumnSizeList.RemoveAt(6 - removeCounter);
                            ColumnWidthSizeList.RemoveAt(6 - removeCounter);
                            removeCounter = removeCounter + 1;
                        }
                        if (SelectOrderNo == "N")
                        {
                            ColumnTypeList.RemoveAt(7 - removeCounter);
                            ColumnSizeList.RemoveAt(7 - removeCounter);
                            ColumnWidthSizeList.RemoveAt(7 - removeCounter);
                            removeCounter = removeCounter + 1;
                        }
                        if (SelectOrderEntryTime == "N")
                        {
                            ColumnTypeList.RemoveAt(8 - removeCounter);
                            ColumnSizeList.RemoveAt(8 - removeCounter);
                            ColumnWidthSizeList.RemoveAt(8 - removeCounter);
                            removeCounter = removeCounter + 1;
                        }
                        if (SelectTradeNo == "N")
                        {
                            ColumnTypeList.RemoveAt(9 - removeCounter);
                            ColumnSizeList.RemoveAt(9 - removeCounter);
                            ColumnWidthSizeList.RemoveAt(9 - removeCounter);
                            removeCounter = removeCounter + 1;
                        }
                        if (SelectTradeEntryTime == "N")
                        {
                            ColumnTypeList.RemoveAt(10 - removeCounter);
                            ColumnSizeList.RemoveAt(10 - removeCounter);
                            ColumnWidthSizeList.RemoveAt(10 - removeCounter);
                            removeCounter = removeCounter + 1;
                        }
                        if (SelectCntrNo == "N")
                        {
                            ColumnTypeList.RemoveAt(11 - removeCounter);
                            ColumnSizeList.RemoveAt(11 - removeCounter);
                            ColumnWidthSizeList.RemoveAt(11 - removeCounter);
                            removeCounter = removeCounter + 1;
                        }

                        ColumnType = (string[])ColumnTypeList.ToArray(typeof(string));
                        ColumnSize = (string[])ColumnSizeList.ToArray(typeof(string));
                        ColumnWidthSize = (string[])ColumnWidthSizeList.ToArray(typeof(string));

                        oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, dtExport, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);

                    }
                    else
                    {
                        string[] ColumnType = { "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "I", "N", "V", "N", "N", "N", "V", "V", "N", "N", "N", "N", "N", "N", "V" };
                        string[] ColumnSize = { "150", "15", "15", "150", "150", "50", "28", "50", "50", "28", "50", "10", "28,0", "50", "28,0", "28,0", "28,4", "20", "20", "28,4", "18,2", "28,4", "28,2", "28,2", "28,2", "30" };
                        string[] ColumnWidthSize = { "30", "10", "10", "30", "30", "10", "10", "10", "15", "10", "15", "10", "15", "15", "15", "15", "15", "10", "10", "15", "15", "15", "15", "15", "15", "15" };


                        ArrayList ColumnTypeList = new ArrayList(ColumnType);
                        ArrayList ColumnSizeList = new ArrayList(ColumnSize);
                        ArrayList ColumnWidthSizeList = new ArrayList(ColumnWidthSize);

                        int removeCounter = 0;
                        if (SelectTerminalid == "N")
                        {
                            ColumnTypeList.RemoveAt(5);
                            ColumnSizeList.RemoveAt(5);
                            ColumnWidthSizeList.RemoveAt(5);
                            removeCounter = 1;
                        }
                        if (SelectTradeCode == "N")
                        {
                            ColumnTypeList.RemoveAt(6 - removeCounter);
                            ColumnSizeList.RemoveAt(6 - removeCounter);
                            ColumnWidthSizeList.RemoveAt(6 - removeCounter);
                            removeCounter = removeCounter + 1;
                        }
                        if (SelectOrderNo == "N")
                        {
                            ColumnTypeList.RemoveAt(7 - removeCounter);
                            ColumnSizeList.RemoveAt(7 - removeCounter);
                            ColumnWidthSizeList.RemoveAt(7 - removeCounter);
                            removeCounter = removeCounter + 1;
                        }
                        if (SelectOrderEntryTime == "N")
                        {
                            ColumnTypeList.RemoveAt(8 - removeCounter);
                            ColumnSizeList.RemoveAt(8 - removeCounter);
                            ColumnWidthSizeList.RemoveAt(8 - removeCounter);
                            removeCounter = removeCounter + 1;
                        }
                        if (SelectTradeNo == "N")
                        {
                            ColumnTypeList.RemoveAt(9 - removeCounter);
                            ColumnSizeList.RemoveAt(9 - removeCounter);
                            ColumnWidthSizeList.RemoveAt(9 - removeCounter);
                            removeCounter = removeCounter + 1;
                        }
                        if (SelectTradeEntryTime == "N")
                        {
                            ColumnTypeList.RemoveAt(10 - removeCounter);
                            ColumnSizeList.RemoveAt(10 - removeCounter);
                            ColumnWidthSizeList.RemoveAt(10 - removeCounter);
                            removeCounter = removeCounter + 1;
                        }
                        if (SelectCntrNo == "N")
                        {
                            ColumnTypeList.RemoveAt(11 - removeCounter);
                            ColumnSizeList.RemoveAt(11 - removeCounter);
                            ColumnWidthSizeList.RemoveAt(11 - removeCounter);
                            removeCounter = removeCounter + 1;
                        }

                        ColumnType = (string[])ColumnTypeList.ToArray(typeof(string));
                        ColumnSize = (string[])ColumnSizeList.ToArray(typeof(string));
                        ColumnWidthSize = (string[])ColumnWidthSizeList.ToArray(typeof(string));

                        oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, dtExport, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                    }
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordDisplay", "RecordDisplay('1');", true);
            }
        }
        protected bool xlExport_Validation()
        {
            bool result = true;
            if (ddlGeneration.SelectedValue == "3")
            {
                if (ddlGroup.SelectedValue != null)
                {
                    if ((ddlGroup.SelectedValue == "0") && (rdbranchclientSelected.Checked == true))
                    {
                        if ((HiddenField_Branch.Value == null) || (HiddenField_Branch.Value == string.Empty))
                        {
                            result = false;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "ExcelValidate", "ExcelValidate(1);", true);
                        }
                    }
                    if ((ddlGroup.SelectedValue == "1") && (rdddlgrouptypeSelected.Checked == true))
                    {
                        if ((HiddenField_Group.Value == null) || (HiddenField_Group.Value == string.Empty))
                        {
                            result = false;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "ExcelValidate", "ExcelValidate(2);", true);
                        }
                    }
                    else if ((ddlGroup.SelectedValue == "2") && (rdbranchclientSelected.Checked == true))
                    {
                        if ((HiddenField_BranchGroup.Value == null) || (HiddenField_BranchGroup.Value == string.Empty))
                        {
                            result = false;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "ExcelValidate", "ExcelValidate(3);", true);
                        }
                    }
                    else if ((ddlGroup.SelectedValue == "3") && (rdbranchclientSelected.Checked == true))
                    {
                        if ((HiddenField_Client.Value == null) || (HiddenField_Client.Value == string.Empty))
                        {
                            result = false;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "ExcelValidate", "ExcelValidate(4);", true);
                        }
                    }
                }
                if (rdInstrumentSelected.Checked == true)
                {
                    if ((HiddenField_Instrument.Value == null) || (HiddenField_Instrument.Value == string.Empty))
                    {
                        result = false;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ExcelValidate", "ExcelValidate(5);", true);
                    }
                }
                if (rdbSegSelected.Checked == true)
                {
                    if ((HiddenField_Segment.Value == null) || (HiddenField_Segment.Value == string.Empty))
                    {
                        result = false;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ExcelValidate", "ExcelValidate(6);", true);
                    }
                }
            }
            return result;
        }
        protected void btnexport_Click(object sender, EventArgs e)
        {
            if (xlExport_Validation() == true)
            {
                reportType = "3";
                exportType = "Excel";
                procedure(reportType, exportType);
                if ((ds.Tables[0].Rows.Count == 0) || (ds.Tables[0].Rows[0]["Segment"].ToString() == "") || (ds.Tables[0].Rows[0]["Segment"].ToString() == "Grand Total"))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(1);", true);
                }
                else
                {
                    ExcelExport();
                }
            }
        }
        # endregion

        #region Email Generate
        protected void btnmail_Click(object sender, EventArgs e)
        {
            reportType = "4";
            exportType = "Email";
            procedure(reportType, exportType);
            ds = (DataSet)ViewState["dataset"];
            if (ds.Tables[0].Rows.Count > 0)
            {
                mail();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(1);", true);
            }
        }
        void mail()
        {
            ViewState["billdate"] = oconverter.ArrangeDate2(dtFrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtTo.Value.ToString());
            if (ddloptionformail.SelectedItem.Value.ToString().Trim() == "0")
            {
                clientwiseemail();
            }
            if (ddloptionformail.SelectedItem.Value.ToString().Trim() == "1")
            {
                branhgroupemail();
            }
            if (ddloptionformail.SelectedItem.Value.ToString().Trim() == "2")
            {
                optionforemailclient();
            }
            if (ddloptionformail.SelectedItem.Value.ToString().Trim() == "4")
            {
                clientwiseemail();
            }
        }
        void emailbind(string clientid, string grpid)
        {
            ds = (DataSet)ViewState["dataset"];
            DataView viewclient = new DataView();
            viewclient = ds.Tables[0].DefaultView;
            viewclient.RowFilter = "CustomerID='" + clientid.ToString().Trim() + "'";
            dtClients.Clear();
            dtClients = viewclient.ToTable();

            //////////NET CALCULATION

            string prv_Quantity = null;
            string Quantity = null;
            string productseries = null;
            int j;
            for (j = 0; j < dtClients.Rows.Count; j++)
            {
                Quantity = dtClients.Rows[j]["Quantity"].ToString();
                dtClients.Rows[j]["Quantity"] = DBNull.Value;
                if (productseries != null)
                {
                    if (productseries != dtClients.Rows[j]["ProductSeriesID"].ToString().Trim())
                    {
                        dtClients.Rows[j - 1]["Quantity"] = prv_Quantity;
                    }
                }
                productseries = dtClients.Rows[j]["ProductSeriesID"].ToString();
                prv_Quantity = Quantity;
            }

            dtClients.Rows[j - 1]["Quantity"] = prv_Quantity;
            String strHtml = String.Empty;
            int flag = 0;
            /////////HTML TABLE HEADER
            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" colspan=5><b>" + dtClients.Rows[0]["CLIENTNAME"].ToString().Trim() + "</b></td>";
            strHtml += "</tr>";
            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Trade Date</b></td>";
            if (rdbSegAll.Checked)
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Segment</b></td>";
            }

            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Terminal Id</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>TrdCode</td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Order No</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Trade No</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Trade Time</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Scrip</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Bought</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Sold</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Unit Price</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Type</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Brkg</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Total Brkg</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Net Rate</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Net Value</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Srv Tax</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Net Amnt</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Net</b></td>";

            for (int k = 0; k < dtClients.Rows.Count; k++)
            {
                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                strHtml += "<td  align=left style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dtClients.Rows[k]["TradeDate"].ToString().Trim() + "</td>";
                if (rdbSegAll.Checked)
                {
                    strHtml += "<td  align=left style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dtClients.Rows[k]["segmentname"].ToString().Trim() + "</td>";
                }

                strHtml += "<td  align=left style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dtClients.Rows[k]["Terminalid"].ToString().Trim() + "</td>";
                strHtml += "<td  align=left style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dtClients.Rows[k]["EXCHGECUSTOMERUCC"].ToString().Trim() + "</td>";
                strHtml += "<td  align=left style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dtClients.Rows[k]["OrderNumber"].ToString().Trim() + "</td>";
                strHtml += "<td  align=left style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dtClients.Rows[k]["TradeNumber"].ToString().Trim() + "</td>";
                strHtml += "<td  align=left style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dtClients.Rows[k]["TradeEntryTime"].ToString().Trim() + "</td>";
                strHtml += "<td  align=left style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dtClients.Rows[k]["Symbol"].ToString().Trim() + "</td>";

                if (dtClients.Rows[k]["Bought"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dtClients.Rows[k]["Bought"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
                if (dtClients.Rows[k]["Sold"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dtClients.Rows[k]["Sold"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
                if (dtClients.Rows[k]["MKTPRICE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dtClients.Rows[k]["MKTPRICE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
                strHtml += "<td  align=left style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dtClients.Rows[k]["brkgtype"].ToString().Trim() + "</td>";
                if (dtClients.Rows[k]["UnitBrokerage"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dtClients.Rows[k]["UnitBrokerage"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
                if (dtClients.Rows[k]["TotalBrokerage"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dtClients.Rows[k]["TotalBrokerage"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
                if (dtClients.Rows[k]["NetRatePerUnit"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dtClients.Rows[k]["NetRatePerUnit"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
                if (dtClients.Rows[k]["NetValue"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dtClients.Rows[k]["NetValue"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
                if (dtClients.Rows[k]["servicetax"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dtClients.Rows[k]["servicetax"].ToString() + "[" + dtClients.Rows[k]["ServiceTaxMode"].ToString().Trim() + "]</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
                if (dtClients.Rows[k]["netamount"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dtClients.Rows[k]["netamount"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
                if (dtClients.Rows[k]["Quantity"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dtClients.Rows[k]["Quantity"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";

                strHtml += "</tr>";
            }
            //////////////TOTAL DISPLAY
            flag = flag + 1;
            strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
            strHtml += "<td  align=left style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Total :</b></td>";
            if (rdbSegAll.Checked)
            {
                strHtml += "<td align=\"right\" >&nbsp;</td>";
            }

            strHtml += "<td align=\"right\" colspan=6>&nbsp;</td>";
            if (dtClients.Rows[0]["Boughtsum"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dtClients.Rows[0]["Boughtsum"].ToString() + "</td>";
            else
                strHtml += "<td align=\"right\" >&nbsp;</td>";
            if (dtClients.Rows[0]["Soldsum"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dtClients.Rows[0]["Soldsum"].ToString() + "</td>";
            else
                strHtml += "<td align=\"right\" >&nbsp;</td>";
            strHtml += "<td align=\"right\" colspan=3>&nbsp;</td>";
            if (dtClients.Rows[0]["TotalBrokeragesum"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dtClients.Rows[0]["TotalBrokeragesum"].ToString() + "</td>";
            else
                strHtml += "<td align=\"right\" >&nbsp;</td>";
            strHtml += "<td align=\"right\" >&nbsp;</td>";
            if (dtClients.Rows[0]["NetValueesum"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dtClients.Rows[0]["NetValueesum"].ToString() + "</td>";
            else
                strHtml += "<td align=\"right\" >&nbsp;</td>";
            if (dtClients.Rows[0]["servicetaxsum"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dtClients.Rows[0]["servicetaxsum"].ToString() + "</td>";
            else
                strHtml += "<td align=\"right\" >&nbsp;</td>";
            if (dtClients.Rows[0]["netamntesum"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dtClients.Rows[0]["netamntesum"].ToString() + "</td>";
            else
                strHtml += "<td align=\"right\" >&nbsp;</td>";
            strHtml += "<td align=\"right\" >&nbsp;</td>";
            strHtml += "</tr>";
            strHtml += "</table>";

            ViewState["mail"] = strHtml;
        }
        protected string GetRowColor(int i)
        {
            if (i++ % 2 == 0)
                return "#fff0f5";
            else
                return "White";
        }
        void clientwiseemail()
        {
            ViewState["mailsendresult"] = "no";
            ds = (DataSet)ViewState["dataset"];
            DataView viewgroup = new DataView(ds.Tables[0]);
            DataTable dtgroupcontactid = viewgroup.ToTable(true, new string[] { "grpid", "grpname" });
            for (int j = 0; j < dtgroupcontactid.Rows.Count; j++)
            {
                ds = (DataSet)ViewState["dataset"];
                DataView viewgrp = new DataView();
                viewgrp = ds.Tables[0].DefaultView;
                viewgrp.RowFilter = "grpid='" + dtgroupcontactid.Rows[j]["grpid"].ToString().Trim() + "'";
                DataTable dt = new DataTable();
                dt = viewgrp.ToTable();
                DataView viewClient = new DataView(dt);
                DataTable Distinctclient = viewClient.ToTable(true, new string[] { "CustomerID", "CLIENTNAME" });
                if (Distinctclient.Rows.Count > 0)
                {
                    cmbclient.DataSource = Distinctclient;
                    cmbclient.DataValueField = "CustomerID";
                    cmbclient.DataTextField = "CLIENTNAME";
                    cmbclient.DataBind();
                }
                for (int k = 0; k < cmbclient.Items.Count; k++)
                {
                    emailbind(cmbclient.Items[k].Value, dtgroupcontactid.Rows[j]["grpid"].ToString().Trim());
                    if (oDBEngine.SendReport(ViewState["mail"].ToString().Trim(), cmbclient.Items[k].Value, ViewState["billdate"].ToString().Trim(), "Trade Register : [" + ViewState["billdate"].ToString().Trim() + "]") == true)
                    {
                        if (ViewState["mailsendresult"].ToString().Trim() == "errorsuccess")
                        {
                            ViewState["mailsendresult"] = "someclienterror";
                        }
                        else
                        {
                            ViewState["mailsendresult"] = "success";
                        }
                    }
                    else
                    {
                        ViewState["mailsendresult"] = "errorsuccess";
                    }
                }
            }
            if (ViewState["mailsendresult"].ToString().Trim() == "someclienterror")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert9", "alert('Mail Sent Successfully !!'+'\n'+'Emails not Sent For Clients Without Email-Id ...');", true);
            }
            if (ViewState["mailsendresult"].ToString().Trim() == "success")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert10", "alert('Mail Sent Successfully !!');", true);
            }
            if (ViewState["mailsendresult"].ToString().Trim() == "errorsuccess")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert11", "alert('Error on sending!Try again.. !!');", true);
            }
        }
        void branhgroupemail()
        {
            ViewState["GRPmail"] = "mail";
            ViewState["mailsendresult"] = "no";
            ds = (DataSet)ViewState["dataset"];
            DataView viewgroup = new DataView(ds.Tables[0]);
            DataTable dtgroupcontactid = viewgroup.ToTable(true, new string[] { "grpid", "grpemail", "GRPNAME" });
            for (int j = 0; j < dtgroupcontactid.Rows.Count; j++)
            {
                ds = (DataSet)ViewState["dataset"];
                DataView viewgrp = new DataView();
                viewgrp = ds.Tables[0].DefaultView;
                viewgrp.RowFilter = "GRPID='" + dtgroupcontactid.Rows[j]["grpid"].ToString().Trim() + "'";
                DataTable dt = new DataTable();
                dt = viewgrp.ToTable();
                DataView viewClient = new DataView(dt);
                DataTable Distinctclient = viewClient.ToTable(true, new string[] { "CUSTOMERID", "CLIENTNAME" });
                if (Distinctclient.Rows.Count > 0)
                {
                    cmbclient.DataSource = Distinctclient;
                    cmbclient.DataValueField = "CUSTOMERID";
                    cmbclient.DataTextField = "CLIENTNAME";
                    cmbclient.DataBind();
                }
                for (int k = 0; k < cmbclient.Items.Count; k++)
                {
                    emailbind(cmbclient.Items[k].Value, dtgroupcontactid.Rows[j]["grpid"].ToString().Trim());
                    if (ViewState["GRPmail"].ToString().Trim() == "mail")
                    {
                        ViewState["GRPmail"] = ViewState["mail"].ToString().Trim();
                    }
                    else
                    {
                        ViewState["GRPmail"] = ViewState["GRPmail"].ToString().Trim() + ViewState["mail"].ToString().Trim();
                    }
                }
                DataTable dtBranchGrp = new DataTable();
                string branchGrpEmail = "";
                if (ddlGroup.SelectedValue.ToString() == "0")
                {
                    dtBranchGrp = oDBEngine.GetDataTable("Select isnull(branch_cpEmail,'') from tbl_master_branch where branch_id=" + dtgroupcontactid.Rows[j]["grpid"].ToString().Trim());
                    branchGrpEmail = dtBranchGrp.Rows[0][0].ToString();
                }
                if (ddlGroup.SelectedValue.ToString() == "1")
                {
                    dtBranchGrp = oDBEngine.GetDataTable("Select isnull(gpm_emailID,'') from TBL_MASTER_GROUPMASTER where gpm_id=" + dtgroupcontactid.Rows[j]["grpid"].ToString().Trim());
                    branchGrpEmail = dtBranchGrp.Rows[0][0].ToString();
                }
                if (oDBEngine.SendReportBr(ViewState["GRPmail"].ToString().Trim(), branchGrpEmail, ViewState["billdate"].ToString().Trim(), "Trade Register [" + ViewState["billdate"].ToString().Trim() + "]", dtgroupcontactid.Rows[j]["grpid"].ToString().Trim()) == true)
                {
                    if (branchGrpEmail == "")
                    {
                        ViewState["mailsendresult"] = "branchMailNotFound";
                    }
                    else
                    {
                        if (ViewState["mailsendresult"].ToString().Trim() == "errorsuccess")
                        {
                            ViewState["mailsendresult"] = "someclienterror";
                        }
                        else
                        {
                            ViewState["mailsendresult"] = "success";
                        }
                    }
                }
                else
                {
                    ViewState["mailsendresult"] = "errorsuccess";
                }
                ViewState["GRPmail"] = "mail";
            }
            if (ViewState["mailsendresult"].ToString().Trim() == "someclienterror")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert4", "alert('Mail Sent Successfully !!'+'\n'+'Emails not Sent For Clients Without Email-Id ...');", true);
            }
            if (ViewState["mailsendresult"].ToString().Trim() == "success")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert5", "alert('Mail Sent Successfully !!');", true);
            }
            if (ViewState["mailsendresult"].ToString().Trim() == "errorsuccess")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert6", "alert('Error on sending!Try again.. !!');", true);
            }
            if (ViewState["mailsendresult"].ToString().Trim() == "branchMailNotFound")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert7", "alert('Mail ID Not Found!!');", true);
            }
        }
        void optionforemailclient()
        {
            if (HiddenField_emmail.Value.ToString().Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(5);", true);
            }
            else
            {
                ViewState["GRPmail"] = "mail";
                ViewState["Usermail"] = "UserMail";
                ViewState["mailsendresult"] = "no";
                ds = (DataSet)ViewState["dataset"];
                DataView viewgroup = new DataView(ds.Tables[0]);
                DataTable dtgroupcontactid = viewgroup.ToTable(true, new string[] { "grpid", "grpemail", "grpname" });
                for (int j = 0; j < dtgroupcontactid.Rows.Count; j++)
                {
                    ds = (DataSet)ViewState["dataset"];
                    DataView viewgrp = new DataView();
                    viewgrp = ds.Tables[0].DefaultView;
                    viewgrp.RowFilter = "GRPID='" + dtgroupcontactid.Rows[j]["grpid"].ToString().Trim() + "'";
                    DataTable dt = new DataTable();
                    dt = viewgrp.ToTable();
                    DataView viewClient = new DataView(dt);
                    DataTable Distinctclient = viewClient.ToTable(true, new string[] { "CUSTOMERID", "CLIENTNAME" });
                    if (Distinctclient.Rows.Count > 0)
                    {
                        cmbclient.DataSource = Distinctclient;
                        cmbclient.DataValueField = "CUSTOMERID";
                        cmbclient.DataTextField = "CLIENTNAME";
                        cmbclient.DataBind();
                    }
                    for (int k = 0; k < cmbclient.Items.Count; k++)
                    {
                        emailbind(cmbclient.Items[k].Value, dtgroupcontactid.Rows[j]["grpid"].ToString().Trim());
                        if (ViewState["GRPmail"].ToString().Trim() == "mail")
                        {
                            ViewState["GRPmail"] = ViewState["mail"].ToString().Trim();
                        }
                        else
                        {
                            ViewState["GRPmail"] = ViewState["GRPmail"].ToString().Trim() + ViewState["mail"].ToString().Trim();
                        }
                    }
                    if (ViewState["Usermail"].ToString().Trim() == "UserMail")
                    {
                        ViewState["Usermail"] = ViewState["GRPmail"].ToString().Trim();
                    }
                    else
                    {
                        ViewState["Usermail"] = ViewState["Usermail"].ToString().Trim() + ViewState["GRPmail"].ToString().Trim();
                    }
                    ViewState["GRPmail"] = "mail";
                }
                string[] clnt = HiddenField_emmail.Value.ToString().Split(',');
                int kk = clnt.Length;
                for (int i = 0; i < clnt.Length; i++)
                {
                    if (oDBEngine.SendReportSt(ViewState["Usermail"].ToString().Trim(), clnt[i].ToString().Trim(), ViewState["billdate"].ToString().Trim(), "Trade Register [" + ViewState["billdate"].ToString().Trim() + "]") == true)
                    {
                        if (ViewState["mailsendresult"].ToString().Trim() == "errorsuccess")
                        {
                            ViewState["mailsendresult"] = "someclienterror";
                        }
                        else
                        {
                            ViewState["mailsendresult"] = "success";
                        }
                    }
                    else
                    {
                        ViewState["mailsendresult"] = "errorsuccess";
                    }
                }
                if (ViewState["mailsendresult"].ToString().Trim() == "someclienterror")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert1", "alert('Mail Sent Successfully !!'+'\n'+'Emails not Sent For Clients Without Email-Id ...');", true);
                }
                if (ViewState["mailsendresult"].ToString().Trim() == "success")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert2", "alert('Mail Sent Successfully !!');", true);
                }
                if (ViewState["mailsendresult"].ToString().Trim() == "errorsuccess")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert3", "alert('Error on sending!Try again.. !!');", true);
                }
            }
        }
        # endregion

        #region Pdf Generate
        protected void btnPDF_Click(object sender, EventArgs e)
        {
            PdfExport();
        }

        void PdfExport()
        {
            string reportType = "3";
            string exportType = "Pdf";
            procedure(reportType, exportType);
            if (ds.Tables[0].Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(1);", true);
            }
            else
            {
                Print();
                string DateParameter = ViewState["billprintdate"].ToString();
                string path = HttpContext.Current.Server.MapPath("..\\Reports\\ComTradeRegister.rpt");
                ReportDocument.Load(path);
                ReportDocument.SetDataSource(ds.Tables[0]);
                ReportDocument.SetParameterValue("@DateFormat", (object)DateParameter);
                ReportDocument.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
                ReportDocument.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                ReportDocument.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "Trade Register");
            }
        }

        void Print()
        {
            ds = (DataSet)ViewState["dataset"];
            byte[] logoinByte;
            ds.Tables[0].Columns.Add("Image", System.Type.GetType("System.Byte[]"));
            if (oconverter.getLogoImage(HttpContext.Current.Server.MapPath(@"..\images\logo.bmp"), out logoinByte) != 1)
            {
                ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "logo", "alert('Logo not Found.');", true);
            }
            else
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ds.Tables[0].Rows[i]["Image"] = logoinByte;
                }
            }
            ViewState["billprintdate"] = oconverter.ArrangeDate2(dtFrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtTo.Value.ToString());
        }
        # endregion
    }
}