using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;

namespace ERP.OMS.Reports
{
    public partial class Reports_ObligationStatementFO : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        BusinessLogicLayer.FAReportsOther objFAReportsOther = new BusinessLogicLayer.FAReportsOther();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        DataTable dtgroup = new DataTable();
        DataTable dtClients = new DataTable();
        DataSet ds = new DataSet();
        static string SEmailTbl = string.Empty;
        static DataTable ClientTableEmail = new DataTable();

        static decimal openingbalance = decimal.Zero;
        static decimal netfund = decimal.Zero;
        static decimal netamount = decimal.Zero;
        DataTable dtgroupcontactid = new DataTable();
        int pageindex = 0;

        string data;
        static string Clients;
        //---------For Email
        static DataTable dtEmail = new DataTable();
        static DataTable dtMail = new DataTable();
        ExcelFile objExcel = new ExcelFile();
        static decimal VALUE_Sum;
        static string User;
        String CLValue = string.Empty;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (!IsPostBack)
            {
                if (Request.QueryString["date"] != null)
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "Led", "<script>Ledger();</script>");
                    dtfor.Value = Convert.ToDateTime(Request.QueryString["date"].ToString());
                    procedureForLedger();
                }
                else
                {
                    date();

                    ////////////////////////For Javascript//////////////////////////////
                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");
                }

            }

            rbClientUser.Attributes.Add("OnClick", "User('User')");
            rbOnlyClient.Attributes.Add("OnClick", "User('NoUser')");
            rbRspctvClient.Attributes.Add("OnClick", "User('NoUser')");
            txtName.Attributes.Add("onkeyup", "callAjaxEmail(this,'GetMailId',event)");

            String cbReference1 = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveSvrData", "context");
            String callbackScript1 = "function CallServer1(arg, context){ " + cbReference1 + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer1", callbackScript1, true);

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
                        }
                        else
                        {
                            str += ",'" + AcVal[0] + "'";
                            str1 += "," + "'" + AcVal[0] + "'" + ";" + val[1];
                        }
                    }
                }

            }

            if (idlist[0] == "EM")
            {
                User = str;
                data = "User~" + str;
            }



        }

        void date()
        {
            dtfor.EditFormatString = oconverter.GetDateFormat("Date");
            dtfrom.EditFormatString = oconverter.GetDateFormat("Date");
            dtto.EditFormatString = oconverter.GetDateFormat("Date");

            string[] idlist = oDBEngine.GetDate().GetDateTimeFormats();
            dtfor.Value = Convert.ToDateTime(idlist[2]);
            dtfrom.Value = Convert.ToDateTime(idlist[2]);
            dtto.Value = Convert.ToDateTime(idlist[2]);

            ////////////////////////////////MIN DATE AND MAX DATE(RESPECT TO FIN-YEAR//////////////////////////////
            string[] listfinyear = (HttpContext.Current.Session["LastFinYear"].ToString()).Split('-');
            string mindate = "04-01-" + listfinyear[0];
            string maxdate = "03-31-" + listfinyear[1];
            dtfor.MinDate = Convert.ToDateTime(mindate);
            dtfor.MaxDate = Convert.ToDateTime(maxdate);
            dtfrom.MinDate = Convert.ToDateTime(mindate);
            dtfrom.MaxDate = Convert.ToDateTime(maxdate);
            dtto.MinDate = Convert.ToDateTime(mindate);
            dtto.MaxDate = Convert.ToDateTime(maxdate);

            ////////////////////////////////////////////END////////////////////////////////////////////
        }
        protected void btnhide_Click(object sender, EventArgs e)
        {
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                if (group.Value == "")
                {
                    BindGroup();
                }
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "norecord22", "norecord(3);", true);

        }

        void fn_Client()
        {

            if (rdbClientALL.Checked)//////////////////ALL CLIENT CHECK
            {
                if (ddlGroup.SelectedItem.Value.ToString() == "1")
                {
                    if (rdddlgrouptypeAll.Checked)
                    {
                        if (ddlgrouptype.SelectedItem.Value.ToString() != "0")
                        {
                            DataTable dtclient = new DataTable();
                            dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%' and cnt_internalid in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster where gpm_type='" + ddlgrouptype.SelectedItem.Text.ToString() + "'))");
                            if (dtclient.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtclient.Rows.Count; i++)
                                {
                                    if (clientid.Value == "")
                                        clientid.Value = "'" + dtclient.Rows[i][0].ToString() + "'";
                                    else
                                        clientid.Value += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
                                }

                            }

                        }
                    }
                    else
                    {
                        DataTable dtclient = new DataTable();
                        dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%' and cnt_internalid in(select grp_contactid from tbl_trans_group where grp_groupmaster in(" + group.Value + "))");
                        if (dtclient.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtclient.Rows.Count; i++)
                            {
                                if (clientid.Value == "")
                                    clientid.Value = "'" + dtclient.Rows[i][0].ToString() + "'";
                                else
                                    clientid.Value += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
                            }

                        }
                    }
                }
                else
                {
                    if (rdbranchAll.Checked)
                    {
                        DataTable dtclient = new DataTable();
                        dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%'  and cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ")");
                        if (dtclient.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtclient.Rows.Count; i++)
                            {
                                if (clientid.Value == "")
                                    clientid.Value = "'" + dtclient.Rows[i][0].ToString() + "'";
                                else
                                    clientid.Value += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
                            }

                        }
                    }
                    else
                    {
                        DataTable dtclient = new DataTable();
                        dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%' and cnt_branchid in(" + branch.Value + ")");
                        if (dtclient.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtclient.Rows.Count; i++)
                            {
                                if (clientid.Value == "")
                                    clientid.Value = "'" + dtclient.Rows[i][0].ToString() + "'";
                                else
                                    clientid.Value += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
                            }

                        }
                    }
                }
            }
            else if (radPOAClient.Checked)//////////////////////ALL POA CLIENT CHECK
            {
                if (ddlGroup.SelectedItem.Value.ToString() == "1")
                {
                    if (rdddlgrouptypeAll.Checked)
                    {
                        if (ddlgrouptype.SelectedItem.Value.ToString() != "0")
                        {
                            DataTable dtclient = new DataTable();
                            dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%' and cnt_internalid in (select dpd_cntID from tbl_master_contactdpdetails where dpd_POA=1) and cnt_internalid in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster where gpm_type='" + ddlgrouptype.SelectedItem.Text.ToString() + "'))");
                            if (dtclient.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtclient.Rows.Count; i++)
                                {
                                    if (clientid.Value == "")
                                        clientid.Value = "'" + dtclient.Rows[i][0].ToString() + "'";
                                    else
                                        clientid.Value += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
                                }

                            }

                        }
                    }
                    else
                    {
                        DataTable dtclient = new DataTable();
                        dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%' and cnt_internalid in (select dpd_cntID from tbl_master_contactdpdetails where dpd_POA=1) and cnt_internalid in(select grp_contactid from tbl_trans_group where grp_groupmaster in(" + group.Value + "))");
                        if (dtclient.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtclient.Rows.Count; i++)
                            {
                                if (clientid.Value == "")
                                    clientid.Value = "'" + dtclient.Rows[i][0].ToString() + "'";
                                else
                                    clientid.Value += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
                            }

                        }
                    }
                }
                else
                {
                    if (rdbranchAll.Checked)
                    {
                        DataTable dtclient = new DataTable();
                        dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%'  and cnt_internalid in (select dpd_cntID from tbl_master_contactdpdetails where dpd_POA=1) and cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ")");
                        if (dtclient.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtclient.Rows.Count; i++)
                            {
                                if (clientid.Value == "")
                                    clientid.Value = "'" + dtclient.Rows[i][0].ToString() + "'";
                                else
                                    clientid.Value += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
                            }

                        }
                    }
                    else
                    {
                        DataTable dtclient = new DataTable();
                        dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%' and cnt_internalid in (select dpd_cntID from tbl_master_contactdpdetails where dpd_POA=1) and cnt_branchid in(" + branch.Value + ")");
                        if (dtclient.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtclient.Rows.Count; i++)
                            {
                                if (clientid.Value == "")
                                    clientid.Value = "'" + dtclient.Rows[i][0].ToString() + "'";
                                else
                                    clientid.Value += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
                            }

                        }
                    }
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
            if (rbScreen.Checked)
            {
                if (ddldate.SelectedItem.Value == "0")
                {
                    DataTable DtCount = oDBEngine.GetDataTable("Trans_DailyStatistics", "count(DailyStat_DateTime)", " cast(DATEADD(dd, 0, DATEDIFF(dd, 0,DailyStat_DateTime)) as datetime)=cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtfor.Value + "')) as datetime) and DailyStat_ExchangeSegmentID=" + HttpContext.Current.Session["ExchangeSegmentID"].ToString() + "");

                    if (DtCount.Rows.Count > 0)
                    {
                        if (DtCount.Rows[0][0].ToString() == "0")
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "norecord2", "norecord(2);", true);
                        }
                        else
                        {
                            displayFOClient();
                        }
                    }
                }
                else
                {
                    DataTable DtCount = oDBEngine.GetDataTable("(Select count(DailyStat_DateTime) as ctb1 from Trans_DailyStatistics" +
                    " WHERE  cast(DATEADD(dd, 0, DATEDIFF(dd, 0,DailyStat_DateTime)) as datetime)=cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtfrom.Value + "')) as datetime) " +
                    "and DailyStat_ExchangeSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"].ToString() + "'" +

                    " union all " +

                    "Select count(DailyStat_DateTime) as ctb2 from Trans_DailyStatistics WHERE " +
                    "cast(DATEADD(dd, 0, DATEDIFF(dd, 0,DailyStat_DateTime)) as datetime)=cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtto.Value + "')) as datetime)" +
                    "and DailyStat_ExchangeSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"].ToString() + "') as tb", "ctb1", null);


                    if (DtCount.Rows.Count > 0)
                    {
                        if (DtCount.Rows[0][0].ToString() == "0" || DtCount.Rows[1][0].ToString() == "0")
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "norecord2", "norecord(2);", true);
                        }
                        else
                        {
                            displayFOClient();
                        }
                    }
                }
            }

        }

        void displayFOClient()
        {
            fn_Client();
            string fromdate = string.Empty;
            string todate = string.Empty;
            string Clients = string.Empty;
            string grptype = string.Empty;
            string groupby = string.Empty;

            if (ddldate.SelectedItem.Value == "0")
            {
                fromdate = Convert.ToString(dtfor.Value);
                todate = "NA";
            }
            else
            {
                fromdate = Convert.ToString(dtfrom.Value);
                todate = Convert.ToString(dtto.Value);
            }


            if (clientid.Value == "")
            {
                Clients = "NA";
            }
            else
            {
                Clients = clientid.Value;
            }
            if (ddlGroup.SelectedItem.Value.ToString() == "0")
            {
                grptype = "NA";
                groupby = "BRANCH";
            }
            else
            {
                grptype = ddlgrouptype.SelectedItem.Text.ToString().Trim();
                groupby = "GROUP";
            }
            dtgroup = objFAReportsOther.Sp_ObligationStatementFOCLIENT1(
          Convert.ToString(fromdate),
          Convert.ToString(todate),
          Convert.ToString(Session["usersegid"]),
           Convert.ToString(Session["LastCompany"]),
           Convert.ToString(Session["userbranchHierarchy"]),
            Convert.ToString(HttpContext.Current.Session["ExchangeSegmentID"]),
          Convert.ToString(Clients),
              Convert.ToString(grptype),
               Convert.ToString(groupby)
           );
            ClientTableEmail = dtgroup;
            ViewState["groupclients"] = dtgroup;


            if (dtgroup.Rows.Count > 0)
            {
                if (rbScreen.Checked)
                {
                    ddlbandforgroup();
                }
                else
                {
                    bind_Print();
                }

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "norecord", "norecord(1);", true);

            }

        }
        void ddlbandforgroup()
        {
            dtgroup = (DataTable)ViewState["groupclients"];
            DataView viewgroup = new DataView(dtgroup);
            dtgroupcontactid = viewgroup.ToTable(true, new string[] { "groupid", "groupname" });
            if (dtgroupcontactid.Rows.Count > 0)
            {
                cmbgroup.DataSource = dtgroupcontactid;
                cmbgroup.DataValueField = "groupid";
                cmbgroup.DataTextField = "groupname";
                cmbgroup.DataBind();

                ViewState["group"] = dtgroupcontactid;
                clientdisplay();
            }

        }
        void clientdisplay()
        {
            dtgroup = (DataTable)ViewState["groupclients"];
            DataView viewgroup = new DataView();
            viewgroup = new DataView(dtgroup);
            viewgroup.RowFilter = "groupid='" + cmbgroup.SelectedItem.Value + "'";
            dtClients = new DataTable();
            dtClients = viewgroup.ToTable();
            ViewState["clients"] = dtClients;
            LastPage = dtClients.Rows.Count - 1;
            CurrentPage = 0;
            bind_ClientDetails();
        }
        void bind_ClientDetails()
        {
            if (LastPage > -1)
            {
                dtClients = (DataTable)ViewState["clients"];
                CName.Text = dtClients.Rows[CurrentPage]["Name"].ToString();
                CID.Value = dtClients.Rows[CurrentPage]["cnt_internalid"].ToString();

                listRecord.Text = CurrentPage + 1 + " of " + dtClients.Rows.Count.ToString() + " Record.";
                procedure();

            }

            ShowHidePreviousNext_of_Clients();


        }
        void procedure()
        {
            string fromdate = string.Empty;
            string todate = string.Empty;

            if (ddldate.SelectedItem.Value == "0")
            {
                fromdate = Convert.ToString(dtfor.Value);
                todate = "NA";
            }
            else
            {
                fromdate = Convert.ToString(dtfrom.Value);
                todate = Convert.ToString(dtto.Value);
            }

            ds = objFAReportsOther.Sp_ObligationStatementFO1(
          Convert.ToString(fromdate),
          Convert.ToString(todate),
          Convert.ToString(Session["usersegid"].ToString()),
           Convert.ToString(Session["LastCompany"]),
           Convert.ToString(HttpContext.Current.Session["ExchangeSegmentID"]),
            Convert.ToString(CID.Value),
              Convert.ToString(HttpContext.Current.Session["LastFinYear"])
           );
            if (ddldate.SelectedItem.Value == "0")
            {
                bindTableFORADATE();


            }
            else
            {
                bindTableFORAPeriod();

            }


        }
        void bindTableFORADATE()
        {

            String strHtml = String.Empty;
            if (ds.Tables[0].Rows.Count != 0 || ds.Tables[3].Rows.Count != 0 || ds.Tables[4].Rows.Count != 0)
            {
                decimal totalmtm = decimal.Zero;
                decimal totalexp = decimal.Zero;
                decimal totalmtmexp = decimal.Zero;
                decimal mtm = decimal.Zero;
                decimal exp = decimal.Zero;

                decimal prm = decimal.Zero;
                decimal asnexc = decimal.Zero; ;
                string expirydate = null;

                decimal totaldeposit = decimal.Zero;
                decimal stortageexcess = decimal.Zero;
                strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 0 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

                if (ds.Tables[0].Rows.Count != 0)
                {

                    DataTable dtstore1st = new DataTable();
                    dtstore1st = ds.Tables[1];
                    DataView dv = new DataView(dtstore1st);

                    DataTable dtstore2st = new DataTable();
                    dtstore2st = ds.Tables[2];
                    DataView dv1 = new DataView(dtstore2st);




                    //////////////////////MAIN TABLE/////////////////////////

                    /////////////////////////////FOR MTM////////////////////
                    strHtml += "<tr style=\"background-color: #DBEEF3;\">";
                    strHtml += "<td align=\"center\" colspan=2><b>MTM Obligation</b></td>";
                    strHtml += "<td align=\"center\"><b>Trade Date</b></td>";
                    strHtml += "<td align=\"center\"><b>Trade No.</b></td>";
                    strHtml += "<td align=\"center\"><b>Buy Qty</b></td>";
                    strHtml += "<td align=\"center\"><b>Sell Qty</b></td>";
                    strHtml += "<td align=\"center\"><b>Mkt Rate</b></td>";
                    strHtml += "<td align=\"center\"><b>Brkg</b></td>";
                    strHtml += "<td align=\"center\"><b>Net Rate</b></td>";
                    strHtml += "<td align=\"center\"><b>Amount Dr.</b></td>";
                    strHtml += "<td align=\"center\"><b>Amount Cr.</b></td></tr>";

                    /////////////////////////////////////Display Opening Ledger Balance//////////////////////////////

                    DataTable dtOpen = new DataTable();
                    dtOpen = oDBEngine.OpeningBalanceOnlyJournal("'SYSTM00001'", CID.Value, Convert.ToDateTime(dtfor.Value), Session["usersegid"].ToString(), Session["LastCompany"].ToString(), Convert.ToDateTime(dtfor.Value));
                    if (dtOpen.Rows.Count > 0)
                    {
                        openingbalance = Convert.ToDecimal(dtOpen.Rows[0][1].ToString());
                        if (openingbalance < 0)
                        {
                            strHtml += "<tr style=\"background-color:White;\"><td style=\"color:Blue;border-top:2px;border-bottom:2px;border-style: groove;border-right-style: none; \" align=\"right\" colspan=9><b>Opening Ledger Balance as on " + oconverter.ArrangeDate2(dtfor.Value.ToString()) + "<b/></td>";
                            strHtml += "<td align=\"center\" style=\"color:Black;border-top:2px;border-bottom:2px;border-style: groove;border-right-style: none;border-left-style: none; \"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(Math.Abs(openingbalance))) + "</b></td>";
                            strHtml += "<td align=\"center\" style=\"color:Black;border-top:2px;border-bottom:2px;border-style: groove;border-left-style: none; \">&nbsp;</td></tr>";
                        }
                        else
                        {
                            strHtml += "<tr style=\"background-color:White;\"><td style=\"color:Blue;border-top:2px;border-bottom:2px;border-style: groove;border-right-style: none; \" align=\"right\" colspan=9><b>Opening Ledger Balance as on " + oconverter.ArrangeDate2(dtfor.Value.ToString()) + "<b/></td>";
                            strHtml += "<td align=\"center\" style=\"color:Black;border-top:2px;border-bottom:2px;border-style: groove;border-right-style: none;border-left-style: none;  \">&nbsp;</td>";
                            strHtml += "<td align=\"center\" style=\"color:Black;border-top:2px;border-bottom:2px;border-style: groove;border-left-style: none; \"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(openingbalance)) + "</b></td></tr>";
                        }
                    }


                    ///////////////////////////////////////////////ALL MTM DATA BIND///////////////////////////////////////////////
                    int flag = 0;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        totalmtm = decimal.Zero;
                        totalexp = decimal.Zero;

                        flag = flag + 1;
                        //////////////////////////////******** BF POSITION DATA BIND***********///////////////////////////
                        strHtml += "<tr style=\"background-color: " + GetRowColor(flag) + " ;text-align:left\">";
                        strHtml += "<td colspan=2 align=left style=\"color:maroon;\">" + dr["tabSymbol"] + "</td>";
                        expirydate = dr["expirydate"].ToString();
                        if (dr["BFQuantity"] != DBNull.Value)
                        {
                            strHtml += "<td colspan=2 align=left  style=\"color:blue;\">Brought Forward</td>";

                            if (Convert.ToDecimal(dr["BFQuantity"]) < 0)
                            {
                                strHtml += "<td align=right>" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Math.Abs(Convert.ToDecimal(dr["BFQuantity"].ToString()))) + "</td>";
                                strHtml += "<td> &nbsp;</td>";
                            }
                            else
                            {
                                strHtml += "<td> &nbsp;</td>";
                                strHtml += "<td align=right>" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dr["BFQuantity"].ToString())) + "</td>";
                            }
                            strHtml += "<td align=right>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dr["OpenPrice"].ToString())) + "</td>";
                            strHtml += "<td> &nbsp;</td>";
                            strHtml += "<td> &nbsp;</td>";
                            if (Convert.ToDecimal(dr["BFValue"]) < 0)
                            {
                                strHtml += "<td align=right>" + oconverter.getFormattedvaluewithoriginalsign(Math.Abs(Convert.ToDecimal(dr["BFValue"].ToString()))) + "</td>";
                                strHtml += "<td> &nbsp;</td>";
                            }
                            else
                            {
                                strHtml += "<td> &nbsp;</td>";
                                strHtml += "<td align=right>" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dr["BFValue"].ToString())) + "</td>";
                            }
                            if (Convert.ToDateTime(expirydate) == Convert.ToDateTime(dtfor.Value))
                            {
                                totalexp = Convert.ToDecimal(dr["BFValue"]);
                            }
                            else
                            {
                                totalmtm = Convert.ToDecimal(dr["BFValue"]);
                            }

                        }
                        else
                        {
                            strHtml += "<td colspan=9> &nbsp;</td>";
                        }


                        strHtml += "</tr>";
                        ////////////////////////////////********BF POSITION DATA END*********/////////////////////////

                        dv.RowFilter = "tabProductSeriesID = '" + dr["tabProductSeriesID"] + "'";
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        dt = dv.ToTable();
                        foreach (DataRow row in dt.Rows)
                        {
                            ///////////////////////////////*********ALL TRADE FETCH*************//////////////////////////

                            flag = flag + 1;
                            strHtml += "<tr style=\"background-color: " + GetRowColor(flag) + " ;text-align:left\">";
                            strHtml += "<td colspan=2> &nbsp;</td>";
                            strHtml += "<td align=\"left\">" + row["MTMTradeDate"] + "</td>";
                            if (row["MTMRefID"].ToString() != "Expiry")
                            {
                                strHtml += "<td align=\"right\" >" + row["MTMRefID"] + "</td>";
                            }
                            else
                            {
                                strHtml += "<td align=\"right\" ><b>" + row["MTMRefID"] + "</b></td>";

                            }

                            if (row["MTMBUYQUANTITY"] != DBNull.Value)
                            {
                                strHtml += "<td align=\"right\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(row["MTMBUYQUANTITY"].ToString())) + "</td>";
                            }
                            else
                            {
                                strHtml += "<td>&nbsp;</td>";
                            }

                            if (row["MTMSELLQUANTITY"] != DBNull.Value)
                            {
                                strHtml += "<td align=\"right\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(row["MTMSELLQUANTITY"].ToString())) + "</td>";
                            }
                            else
                            {
                                strHtml += "<td> &nbsp;</td>";
                            }

                            if (row["MktRate"] != DBNull.Value)
                            {
                                strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(row["MktRate"].ToString())) + "</td>";
                            }
                            else
                            {
                                strHtml += "<td> &nbsp;</td>";
                            }

                            if (row["MTMBrkg"] != DBNull.Value)
                            {
                                strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(row["MTMBrkg"].ToString())) + "</td>";
                            }
                            else
                            {
                                strHtml += "<td> &nbsp;</td>";
                            }

                            if (row["MTMNetRatePerUnit"] != DBNull.Value)
                            {
                                strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(row["MTMNetRatePerUnit"].ToString())) + "</td>";

                            }
                            else
                            {
                                strHtml += "<td> &nbsp;</td>";
                            }


                            if (row["MTMAMOUNTDR"] != DBNull.Value)
                            {
                                strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluewithoriginalsign(Math.Abs(Convert.ToDecimal(row["MTMAMOUNTDR"].ToString()))) + "</td>";

                                if (Convert.ToDateTime(expirydate) == Convert.ToDateTime(dtfor.Value))
                                {
                                    totalexp = totalexp - Convert.ToDecimal(row["MTMAMOUNTDR"]);
                                }
                                else
                                {
                                    totalmtm = totalmtm - Convert.ToDecimal(row["MTMAMOUNTDR"]);
                                }
                            }
                            else
                            {
                                strHtml += "<td> &nbsp;</td>";
                            }

                            if (row["MTMAMOUNTCR"] != DBNull.Value)
                            {
                                strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(row["MTMAMOUNTCR"].ToString())) + "</td>";
                                if (Convert.ToDateTime(expirydate) == Convert.ToDateTime(dtfor.Value))
                                {
                                    totalexp = totalexp + Convert.ToDecimal(row["MTMAMOUNTCR"]);
                                }
                                else
                                {
                                    totalmtm = totalmtm + Convert.ToDecimal(row["MTMAMOUNTCR"]);
                                }
                            }
                            else
                            {
                                strHtml += "<td align=\"center\"> &nbsp;</td>";
                            }

                            strHtml += "</tr>";

                        }

                        //////////////////////////////**********ALL TRADE FETCH END********///////////////////////////
                        dv1.RowFilter = "tabProductSeriesID = '" + dr["tabProductSeriesID"] + "'";
                        DataTable dt1 = new DataTable();
                        dt1 = ds.Tables[0];
                        dt1 = dv1.ToTable();
                        foreach (DataRow row1 in dt1.Rows)
                        {
                            if (row1["CFQuantity"] != DBNull.Value)
                            {
                                flag = flag + 1;
                                //////////////////////////////******** CF POSITION DATA BIND***********///////////////////////////
                                strHtml += "<tr style=\"background-color: " + GetRowColor(flag) + " ;text-align:left\">";
                                strHtml += "<td colspan=2> &nbsp;</td>";


                                if (Convert.ToDateTime(expirydate) == Convert.ToDateTime(dtfor.Value))
                                {
                                    strHtml += "<td colspan=2 align=left style=\"color:Green;\">Expiry</td>";
                                }
                                else
                                {
                                    strHtml += "<td colspan=2 align=left style=\"color:Green;\">Carried Forward</td>";
                                }

                                if (Convert.ToDecimal(row1["CFQuantity"]) > 0)/////////OPPOSITE DIRECTION
                                {
                                    strHtml += "<td align=right>" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Math.Abs(Convert.ToDecimal(row1["CFQuantity"].ToString()))) + "</td>";
                                    strHtml += "<td> &nbsp;</td>";
                                }
                                else
                                {
                                    strHtml += "<td> &nbsp;</td>";
                                    strHtml += "<td align=right>" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Math.Abs(Convert.ToDecimal(row1["CFQuantity"].ToString()))) + "</td>";
                                }
                                strHtml += "<td align=right>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(row1["SettPrice"].ToString())) + "</td>";
                                strHtml += "<td> &nbsp;</td>";
                                strHtml += "<td> &nbsp;</td>";
                                if (Convert.ToDecimal(row1["CFValue"]) > 0)/////////OPPOSITE DIRECTION
                                {
                                    strHtml += "<td align=right>" + oconverter.getFormattedvaluewithoriginalsign(Math.Abs(Convert.ToDecimal(row1["CFValue"].ToString()))) + "</td>";
                                    strHtml += "<td> &nbsp;</td>";
                                }
                                else
                                {
                                    strHtml += "<td> &nbsp;</td>";
                                    strHtml += "<td align=right>" + oconverter.getFormattedvaluewithoriginalsign(Math.Abs(Convert.ToDecimal(row1["CFValue"].ToString()))) + "</td>";
                                }

                                strHtml += "</tr>";

                                if (Convert.ToDateTime(expirydate) == Convert.ToDateTime(dtfor.Value))
                                {
                                    totalexp = totalexp - Convert.ToDecimal(row1["CFValue"]);
                                }
                                else
                                {
                                    totalmtm = totalmtm - Convert.ToDecimal(row1["CFValue"]);
                                }
                            }

                        }
                        ////////////////////////////////********CF POSITION DATA END*********/////////////////////////
                        //////////////////////////////////SUMMARY DISPLAY/////////////////////////////////////////////
                        strHtml += "<tr style=\"background-color:White;\">";
                        strHtml += "<td align=right colspan=11><div style=\"background-color:lavender;width:30%;\">";
                        strHtml += "<table width=\"100%\" border=" + Convert.ToChar(34) + 0 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

                        if (Convert.ToDateTime(expirydate) == Convert.ToDateTime(dtfor.Value))
                        {
                            strHtml += "<tr><td align=left style=\"width:10%;\">Final Settlement</td>";
                            if (totalexp < 0)
                            {
                                strHtml += "<td align=\"right\" style=\"width:13%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(totalexp))) + "</b></td>";
                                strHtml += "<td style=\"width:7%;\"> &nbsp;</td>";
                                strHtml += "</tr>";
                            }
                            else
                            {
                                strHtml += "<td style=\"width:10%;\"> &nbsp;</td>";
                                strHtml += "<td align=\"right\" style=\"width:10%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(totalexp))) + "</b></td>";
                                strHtml += "</tr>";
                            }
                        }
                        else
                        {
                            strHtml += "<tr><td align=left style=\"width:10%;\">MTM Settlement</td>";

                            if (totalmtm < 0)
                            {
                                strHtml += "<td align=\"right\" style=\"width:13%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(totalmtm))) + "</b></td>";
                                strHtml += "<td style=\"width:7%;\"> &nbsp;</td>";
                                strHtml += "</tr>";
                            }
                            else
                            {
                                strHtml += "<td style=\"width:10%;\"> &nbsp;</td>";
                                strHtml += "<td align=\"right\" style=\"width:10%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(totalmtm))) + "</b></td>";
                                strHtml += "</tr>";
                            }
                        }
                        strHtml += "</div></table></td></tr>";
                        mtm = mtm + totalmtm;
                        exp = exp + totalexp;
                        //////////////////////////////////////END//////////////////////////////////////////////////////

                    }
                    //////////////////////////////////Final Settlement of All Contracts DISPLAY/////////////////////////////////////////////
                    strHtml += "<tr style=\"height:1px;background-color:White;\"><td colspan=11>&nbsp;</td></tr><tr style=\"background-color:White;\">";
                    strHtml += "<td align=right colspan=11><div style=\"border: solid 1px balck;width:50%;\">";
                    strHtml += "<table width=\"100%\" border=" + Convert.ToChar(34) + 0 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                    totalmtmexp = mtm + exp;

                    if (exp != 0)
                    {
                        strHtml += "<tr><td align=left style=\"width:35%;\">Final Settlement of All Contracts:</td>";
                        if (exp < 0)
                        {
                            strHtml += "<td align=\"right\" style=\"width:19%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(exp))) + "</b></td>";
                            strHtml += "<td style=\"width:1%;\"></td>";
                            strHtml += "</tr>";
                        }
                        else
                        {
                            strHtml += "<td style=\"width:5%;\"> &nbsp;</td>";
                            strHtml += "<td align=\"right\" style=\"width:15%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(exp))) + "</b></td>";
                            strHtml += "</tr>";
                        }
                    }
                    if (mtm != 0)
                    {
                        strHtml += "<tr><td align=left style=\"width:35%;\">MTM Of all contracts:</td>";

                        if (mtm < 0)
                        {
                            strHtml += "<td align=\"right\" style=\"width:26%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(mtm))) + "</b></td>";
                            strHtml += "<td style=\"width:10%;\"></td>";
                            strHtml += "</tr>";
                        }
                        else
                        {
                            strHtml += "<td style=\"width:10%;\"> &nbsp;</td>";
                            strHtml += "<td align=\"right\" style=\"width:15%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(mtm))) + "</b></td>";
                            strHtml += "</tr>";
                        }
                    }
                    strHtml += "<tr><td align=left style=\"width:35%;\">Net MTM and Final Settlement of All Contracts:</td>";

                    if (totalmtmexp < 0)
                    {
                        strHtml += "<td align=\"right\" style=\"width:19%;color:maroon;\"> <b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(totalmtmexp))) + "</b></td>";
                        strHtml += "<td style=\"width:1%;\"></td>";
                        strHtml += "</tr>";
                    }
                    else
                    {
                        strHtml += "<td style=\"width:5%;\"> &nbsp;</td>";
                        strHtml += "<td align=\"right\" style=\"width:15%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(totalmtmexp))) + "</b></td>";
                        strHtml += "</tr>";
                    }

                    strHtml += "</div></table></td></tr><tr style=\"height:1px;background-color:White;\"><td colspan=11>&nbsp;</td></tr>";
                    //////////////////////////////////////END//////////////////////////////////////////////////////

                }



                /////////////////////////////FOR Premium////////////////////
                if (ds.Tables[3].Rows.Count != 0)
                {
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        /////////////////////////////////////Display Opening Ledger Balance//////////////////////////////

                        DataTable dtOpen = new DataTable();
                        dtOpen = oDBEngine.OpeningBalanceOnlyJournal("'SYSTM00001'", CID.Value, Convert.ToDateTime(dtfor.Value), Session["usersegid"].ToString(), Session["LastCompany"].ToString(), Convert.ToDateTime(dtfor.Value));
                        if (dtOpen.Rows.Count > 0)
                        {
                            openingbalance = Convert.ToDecimal(dtOpen.Rows[0][1].ToString());
                            if (openingbalance < 0)
                            {
                                strHtml += "<tr style=\"background-color:White;\"><td style=\"color:Blue;border-top:2px;border-bottom:2px;border-style: groove;border-right-style: none; \" align=\"right\" colspan=9><b>Opening Ledger Balance as on " + oconverter.ArrangeDate2(dtfor.Value.ToString()) + "<b/></td>";
                                strHtml += "<td align=\"center\" style=\"color:Black;border-top:2px;border-bottom:2px;border-style: groove;border-right-style: none;border-left-style: none; \"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(Math.Abs(openingbalance))) + "</b></td>";
                                strHtml += "<td align=\"center\" style=\"color:Black;border-top:2px;border-bottom:2px;border-style: groove;border-left-style: none; \">&nbsp;</td></tr>";
                            }
                            else
                            {
                                strHtml += "<tr style=\"background-color:White;\"><td style=\"color:Blue;border-top:2px;border-bottom:2px;border-style: groove;border-right-style: none; \" align=\"right\" colspan=9><b>Opening Ledger Balance as on " + oconverter.ArrangeDate2(dtfor.Value.ToString()) + "<b/></td>";
                                strHtml += "<td align=\"center\" style=\"color:Black;border-top:2px;border-bottom:2px;border-style: groove;border-right-style: none;border-left-style: none;  \">&nbsp;</td>";
                                strHtml += "<td align=\"center\" style=\"color:Black;border-top:2px;border-bottom:2px;border-style: groove;border-left-style: none; \"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(openingbalance)) + "</b></td></tr>";
                            }
                        }
                    }
                    strHtml += "<tr style=\"background-color: #DBEEF3;\">";
                    strHtml += "<td align=\"center\" ><b>Premium Obligation</b></td>";
                    strHtml += "<td align=\"center\" ><b>Strike Price</b></td>";
                    strHtml += "<td align=\"center\"><b>Trade Date</b></td>";
                    strHtml += "<td align=\"center\"><b>Trade No.</b></td>";
                    strHtml += "<td align=\"center\"><b>Buy Qty</b></td>";
                    strHtml += "<td align=\"center\"><b>Sell Qty</b></td>";
                    strHtml += "<td align=\"center\"><b>Premium</b></td>";
                    strHtml += "<td align=\"center\"><b>Brkg</b></td>";
                    strHtml += "<td align=\"center\"><b>Net Prm</b></td>";
                    strHtml += "<td align=\"center\"><b>Amount Dr.</b></td>";
                    strHtml += "<td align=\"center\"><b>Amount Cr.</b></td></tr>";
                    int flag1 = 0;
                    string productid = null;
                    decimal netprm = decimal.Zero;
                    foreach (DataRow row in ds.Tables[3].Rows)
                    {
                        ////////////////////////////////////PRIMIUM SUMMARY DISPLAY///////////////////////////
                        if (productid != null)
                        {
                            if (productid != row["tabProductSeriesID"].ToString())
                            {

                                strHtml += "<tr style=\"background-color:White;\">";
                                strHtml += "<td align=right colspan=11><div style=\"background-color:lavender;width:30%;\">";
                                strHtml += "<table width=\"100%\" border=" + Convert.ToChar(34) + 0 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                                strHtml += "<tr><td align=left style=\"width:10%;\">Net Premium </td>";
                                if (netprm < 0)
                                {
                                    strHtml += "<td align=\"right\" style=\"width:13%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(netprm))) + "</b></td>";
                                    strHtml += "<td style=\"width:7%;\"> &nbsp;</td>";
                                    strHtml += "</tr>";
                                }
                                else
                                {
                                    strHtml += "<td style=\"width:10%;\"> &nbsp;</td>";
                                    strHtml += "<td align=\"right\" style=\"width:10%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(netprm))) + "</b></td>";
                                    strHtml += "</tr>";
                                }
                                prm = prm + netprm;
                                netprm = decimal.Zero;
                                strHtml += "</table></div></td></tr>";
                            }
                        }
                        //////////////////////////////////////////END////////////////////////////////////////////////
                        productid = row["tabProductSeriesID"].ToString();

                        flag1 = flag1 + 1;

                        strHtml += "<tr style=\"background-color: " + GetRowColor(flag1) + " ;text-align:left\">";
                        if (netprm == 0)
                        {
                            strHtml += "<td align=\"left\"  style=\"color:maroon;width:20%;\">" + row["tabSymbol"] + "</td>";
                            if (row["StrikePrice"] != DBNull.Value)
                            {
                                strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(row["StrikePrice"].ToString())) + "</td>";

                            }
                            else
                            {
                                strHtml += "<td>&nbsp;</td>";
                            }
                        }
                        else
                        {
                            strHtml += "<td>&nbsp;</td>";
                            strHtml += "<td>&nbsp;</td>";
                        }
                        strHtml += "<td align=\"left\">" + row["PRMTradeDate"] + "</td>";
                        strHtml += "<td align=\"right\">" + row["PRMRefNo"] + "</td>";

                        if (row["PRMBUYQUANTITY"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(row["PRMBUYQUANTITY"].ToString())) + "</td>";
                        }
                        else
                        {
                            strHtml += "<td>&nbsp;</td>";
                        }

                        if (row["PRMSELLQUANTITY"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(row["PRMSELLQUANTITY"].ToString())) + "</td>";
                        }
                        else
                        {
                            strHtml += "<td> &nbsp;</td>";
                        }
                        if (row["PRMUnitPrice"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(row["PRMUnitPrice"].ToString())) + "</td>";
                        }
                        else
                        {
                            strHtml += "<td> &nbsp;</td>";
                        }


                        if (row["PRMBrkg"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(row["PRMBrkg"].ToString())) + "</td>";
                        }
                        else
                        {
                            strHtml += "<td> &nbsp;</td>";
                        }

                        if (row["PRMNetRatePerUnit"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(row["PRMNetRatePerUnit"].ToString())) + "</td>";

                        }
                        else
                        {
                            strHtml += "<td> &nbsp;</td>";
                        }


                        if (row["PRMAMOUNTDR"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluewithoriginalsign(Math.Abs(Convert.ToDecimal(row["PRMAMOUNTDR"].ToString()))) + "</td>";
                            netprm = netprm - Convert.ToDecimal(row["PRMAMOUNTDR"]);
                        }
                        else
                        {
                            strHtml += "<td> &nbsp;</td>";
                        }

                        if (row["PRMAMOUNTCR"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(row["PRMAMOUNTCR"].ToString())) + "</td>";
                            netprm = netprm + Convert.ToDecimal(row["PRMAMOUNTCR"]);

                        }
                        else
                        {
                            strHtml += "<td> &nbsp;</td>";
                        }
                        strHtml += "</tr>";



                    }
                    ////////////////////////////////////PRIMIUM SUMMARY DISPLAY///////////////////////////

                    strHtml += "<tr style=\"background-color:White;\">";
                    strHtml += "<td align=right colspan=11><div style=\"background-color:lavender;width:30%;\">";
                    strHtml += "<table width=\"100%\" border=" + Convert.ToChar(34) + 0 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                    strHtml += "<tr><td align=left style=\"width:10%;\">Net Premium </td>";
                    if (netprm < 0)
                    {
                        strHtml += "<td align=\"right\" style=\"width:13%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(netprm))) + "</b></td>";
                        strHtml += "<td style=\"width:7%;\"> &nbsp;</td>";
                        strHtml += "</tr>";
                    }
                    else
                    {
                        strHtml += "<td style=\"width:10%;\"> &nbsp;</td>";
                        strHtml += "<td align=\"right\" style=\"width:10%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(netprm))) + "</b></td>";
                        strHtml += "</tr>";
                    }
                    prm = prm + netprm;
                    netprm = decimal.Zero;
                    strHtml += "</table></div></td></tr>";

                    //////////////////////////////////////////END////////////////////////////////////////////////
                    //////////////////////////////////Net Premium of All Contracts DISPLAY/////////////////////////////////////////////
                    strHtml += "<tr style=\"height:1px;background-color:White;\"><td colspan=11>&nbsp;</td></tr><tr style=\"background-color:White;\">";
                    strHtml += "<td align=right colspan=11><div style=\"border: solid 1px balck;width:35%;\">";
                    strHtml += "<table width=\"100%\" border=" + Convert.ToChar(34) + 0 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                    strHtml += "<tr><td align=left style=\"width:15%;\">Net Premium of all contracts:</td>";
                    if (prm < 0)
                    {
                        strHtml += "<td align=\"right\" style=\"width:11%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(prm))) + "</b></td>";
                        strHtml += "<td style=\"width:4%;\"> &nbsp;</td>";
                        strHtml += "</tr>";
                    }
                    else
                    {
                        strHtml += "<td style=\"width:5%;\"> &nbsp;</td>";
                        strHtml += "<td align=\"right\" style=\"width:15%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(prm))) + "</b></td>";
                        strHtml += "</tr>";
                    }

                    strHtml += "</div></table></td></tr><tr style=\"height:1px;background-color:White;\"><td colspan=11>&nbsp;</td></tr>";
                    //////////////////////////////////////END//////////////////////////////////////////////////////
                }

                /////////////////////////////FOR Premium END//////////////////// 

                /////////////////////////////FOR ASN,EXC////////////////////
                if (ds.Tables[4].Rows.Count != 0)
                {
                    if (ds.Tables[0].Rows.Count == 0 && ds.Tables[3].Rows.Count == 0)
                    {
                        /////////////////////////////////////Display Opening Ledger Balance//////////////////////////////

                        DataTable dtOpen = new DataTable();
                        dtOpen = oDBEngine.OpeningBalanceOnlyJournal("'SYSTM00001'", CID.Value, Convert.ToDateTime(dtfor.Value), Session["usersegid"].ToString(), Session["LastCompany"].ToString(), Convert.ToDateTime(dtfor.Value));
                        if (dtOpen.Rows.Count > 0)
                        {
                            openingbalance = Convert.ToDecimal(dtOpen.Rows[0][1].ToString());
                            if (openingbalance < 0)
                            {
                                strHtml += "<tr style=\"background-color:White;\"><td style=\"color:Blue;border-top:2px;border-bottom:2px;border-style: groove;border-right-style: none; \" align=\"right\" colspan=9><b>Opening Ledger Balance as on " + oconverter.ArrangeDate2(dtfor.Value.ToString()) + "<b/></td>";
                                strHtml += "<td align=\"center\" style=\"color:Black;border-top:2px;border-bottom:2px;border-style: groove;border-right-style: none;border-left-style: none; \"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(Math.Abs(openingbalance))) + "</b></td>";
                                strHtml += "<td align=\"center\" style=\"color:Black;border-top:2px;border-bottom:2px;border-style: groove;border-left-style: none; \">&nbsp;</td></tr>";
                            }
                            else
                            {
                                strHtml += "<tr style=\"background-color:White;\"><td style=\"color:Blue;border-top:2px;border-bottom:2px;border-style: groove;border-right-style: none; \" align=\"right\" colspan=9><b>Opening Ledger Balance as on " + oconverter.ArrangeDate2(dtfor.Value.ToString()) + "<b/></td>";
                                strHtml += "<td align=\"center\" style=\"color:Black;border-top:2px;border-bottom:2px;border-style: groove;border-right-style: none;border-left-style: none;  \">&nbsp;</td>";
                                strHtml += "<td align=\"center\" style=\"color:Black;border-top:2px;border-bottom:2px;border-style: groove;border-left-style: none; \"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(openingbalance)) + "</b></td></tr>";
                            }
                        }
                    }
                    strHtml += "<tr style=\"background-color: #DBEEF3;\">";
                    strHtml += "<td align=\"center\" ><b>Options Final Settlement</b></td>";
                    strHtml += "<td align=\"center\" ><b>Strike Price</b></td>";
                    strHtml += "<td align=\"center\"><b>Trade Date</b></td>";
                    strHtml += "<td align=\"center\"><b>Sett.Type</b></td>";
                    strHtml += "<td align=\"center\"><b>Qty</b></td>";
                    strHtml += "<td align=\"center\"><b>Sett Price</b></td>";
                    strHtml += "<td align=\"center\"><b>Rate</b></td>";
                    strHtml += "<td align=\"center\"><b>Sett Charg</b></td>";
                    strHtml += "<td align=\"center\"><b>Net Rate</b></td>";
                    strHtml += "<td align=\"center\"><b>Amount Dr.</b></td>";
                    strHtml += "<td align=\"center\"><b>Amount Cr.</b></td></tr>";
                    int flag2 = 0;
                    foreach (DataRow row in ds.Tables[4].Rows)
                    {
                        flag2 = flag2 + 1;

                        strHtml += "<tr style=\"background-color: " + GetRowColor(flag2) + " ;text-align:left\">";
                        strHtml += "<td align=\"left\"  style=\"color:maroon;\">" + row["tabSymbol"] + "</td>";
                        if (row["StrikePrice"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(row["StrikePrice"].ToString())) + "</td>";

                        }
                        else
                        {
                            strHtml += "<td>&nbsp;</td>";
                        }
                        strHtml += "<td align=\"left\">" + row["ASNEXCTradeDate"] + "</td>";
                        strHtml += "<td align=\"left\">" + row["setttype"] + "</td>";

                        if (row["ASNEXCQUANTITY"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(row["ASNEXCQUANTITY"].ToString())) + "</td>";
                        }
                        else
                        {
                            strHtml += "<td>&nbsp;</td>";
                        }

                        if (row["ASNEXCSettPrice"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(row["ASNEXCSettPrice"].ToString())) + "</td>";
                        }
                        else
                        {
                            strHtml += "<td> &nbsp;</td>";
                        }
                        if (row["ASNEXCUnitPrice"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(row["ASNEXCUnitPrice"].ToString())) + "</td>";
                        }
                        else
                        {
                            strHtml += "<td> &nbsp;</td>";
                        }


                        if (row["ASNEXCBrkg"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(row["ASNEXCBrkg"].ToString())) + "</td>";
                        }
                        else
                        {
                            strHtml += "<td> &nbsp;</td>";
                        }

                        if (row["ASNEXCNetRatePerUnit"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(row["ASNEXCNetRatePerUnit"].ToString())) + "</td>";

                        }
                        else
                        {
                            strHtml += "<td> &nbsp;</td>";
                        }


                        if (row["ASNEXCAMOUNTDR"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluewithoriginalsign(Math.Abs(Convert.ToDecimal(row["ASNEXCAMOUNTDR"].ToString()))) + "</td>";
                            asnexc = asnexc - Convert.ToDecimal(row["ASNEXCAMOUNTDR"]);
                        }
                        else
                        {
                            strHtml += "<td> &nbsp;</td>";
                        }

                        if (row["ASNEXCAMOUNTCR"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(row["ASNEXCAMOUNTCR"].ToString())) + "</td>";
                            asnexc = asnexc + Convert.ToDecimal(row["ASNEXCAMOUNTCR"]);

                        }
                        else
                        {
                            strHtml += "<td> &nbsp;</td>";
                        }
                        strHtml += "</tr>";

                    }
                    //////////////////////////////////Net Options Final Settlement DISPLAY/////////////////////////////////////////////
                    strHtml += "<tr style=\"height:1px;background-color:White;\"><td colspan=11>&nbsp;</td></tr><tr style=\"background-color:White;\">";
                    strHtml += "<td align=right colspan=11><div style=\"border: solid 1px balck;width:50%;\">";
                    strHtml += "<table width=\"100%\" border=" + Convert.ToChar(34) + 0 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                    strHtml += "<tr><td align=left style=\"width:35%;\">Net Options Final Settlement :</td>";
                    if (asnexc < 0)
                    {
                        strHtml += "<td align=\"right\" style=\"width:11%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(asnexc))) + "</b></td>";
                        strHtml += "<td style=\"width:4%;\"> &nbsp;</td>";
                        strHtml += "</tr>";
                    }
                    else
                    {
                        strHtml += "<td style=\"width:5%;\"> &nbsp;</td>";
                        strHtml += "<td align=\"right\" style=\"width:15%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(asnexc))) + "</b></td>";
                        strHtml += "</tr>";
                    }

                    strHtml += "</div></table></td></tr><tr style=\"height:1px;background-color:White;\"><td colspan=11>&nbsp;</td></tr>";
                    //////////////////////////////////////END//////////////////////////////////////////////////////

                }

                /////////////////////////////FOR ASN,EXC END//////////////////// 
                ////////////////////////////ALL DETAILS SUMMARY//////////////////////////
                decimal totalobligation = decimal.Zero;
                decimal totalcharges = decimal.Zero;
                totalobligation = totalmtmexp + prm + asnexc;

                strHtml += "<tr style=\"height:1px;background-color:White;\"><td colspan=11>&nbsp;</td></tr><tr style=\"background-color:White;\">";
                strHtml += "<td align=right colspan=11><div style=\"border: solid 1px balck;width:35%;\">";
                strHtml += "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                strHtml += "<tr><td align=left style=\"width:15%;\">Total Obligation For the Day :</td>";
                if (totalobligation < 0)
                {
                    strHtml += "<td align=\"right\" style=\"width:13%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(totalobligation))) + "</b></td>";
                    strHtml += "<td style=\"width:7%;\"> &nbsp;</td>";
                    strHtml += "</tr>";
                }
                else
                {
                    strHtml += "<td style=\"width:10%;\"> &nbsp;</td>";
                    strHtml += "<td align=\"right\" style=\"width:10%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(totalobligation))) + "</b></td>";
                    strHtml += "</tr>";
                }
                if (ds.Tables[5].Rows.Count > 0)
                {
                    if (ds.Tables[5].Rows[0]["Servicetaxonbrkg"] != DBNull.Value)
                    {
                        strHtml += "<tr><td align=left style=\"width:30%;\">Serv Tax & Cess on Brokerage :</td><td align=\"right\" style=\"width:13%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(ds.Tables[5].Rows[0]["Servicetaxonbrkg"].ToString()))) + "</td><td style=\"width:7%;\"> &nbsp;</td></tr>";
                        totalcharges = totalcharges + Convert.ToDecimal(ds.Tables[5].Rows[0]["Servicetaxonbrkg"]);
                    }
                }
                if (ds.Tables[6].Rows.Count > 0)
                {
                    if (ds.Tables[6].Rows[0]["Temp_TranCharge"] != DBNull.Value)
                    {
                        strHtml += "<tr><td align=left style=\"width:30%;\">Transaction Charges :</td><td align=\"right\" style=\"width:13%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(ds.Tables[6].Rows[0]["Temp_TranCharge"].ToString()))) + "</td><td style=\"width:7%;\"> &nbsp;</td></tr>";
                        totalcharges = totalcharges + Convert.ToDecimal(ds.Tables[6].Rows[0]["Temp_TranCharge"]);

                    }
                    if (ds.Tables[6].Rows[0]["charges"] != DBNull.Value)
                    {
                        strHtml += "<tr><td align=left style=\"width:30%;\">Serv Tax & Cess on Tran Charge:</td><td align=\"right\" style=\"width:13%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(ds.Tables[6].Rows[0]["charges"].ToString()))) + "</td><td style=\"width:7%;\"> &nbsp;</td></tr>";
                        totalcharges = totalcharges + Convert.ToDecimal(ds.Tables[6].Rows[0]["charges"]);

                    }
                }
                if (ds.Tables[7].Rows.Count > 0)
                {
                    if (ds.Tables[7].Rows[0]["Temp_TotalStamduty"] != DBNull.Value)
                    {
                        strHtml += "<tr><td align=left style=\"width:30%;\">Stamp Duty:</td><td align=\"right\" style=\"width:13%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(ds.Tables[7].Rows[0]["Temp_TotalStamduty"].ToString()))) + "</td><td style=\"width:7%;\"> &nbsp;</td></tr>";
                        totalcharges = totalcharges + Convert.ToDecimal(ds.Tables[7].Rows[0]["Temp_TotalStamduty"]);

                    }
                }
                if (ds.Tables[8].Rows.Count > 0)
                {
                    if (ds.Tables[8].Rows[0]["exchstttax"] != DBNull.Value)
                    {
                        strHtml += "<tr><td align=left style=\"width:30%;\">STT Tax :</td><td align=\"right\" style=\"width:13%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(ds.Tables[8].Rows[0]["exchstttax"].ToString()))) + "</td><td style=\"width:7%;\"> &nbsp;</td></tr>";
                        totalcharges = totalcharges + Convert.ToDecimal(ds.Tables[8].Rows[0]["exchstttax"]);
                    }
                }
                strHtml += "<tr><td align=left style=\"width:30%;\">Total Charges :</td><td align=\"right\" style=\"width:13%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(totalcharges))) + "</td><td style=\"width:7%;\"> &nbsp;</td></tr>";
                if ((totalobligation - totalcharges) < 0)
                {
                    strHtml += "<tr><td align=left style=\"width:30%;\"><u>Net Bill Amount :</u></td><td align=\"right\" style=\"width:13%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(totalobligation - totalcharges))) + "</td><td style=\"width:7%;\"> &nbsp;</td></tr>";

                }
                else
                {
                    strHtml += "<tr><td align=left style=\"width:30%;\"><u>Net Bill Amount :</u></td><td style=\"width:10%;\"> &nbsp;</td><td align=\"right\" style=\"width:10%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(totalobligation - totalcharges))) + "</td></tr>";

                }

                DataTable dtfund = new DataTable();
                dtfund = oDBEngine.NetFundAdjustment("'SYSTM00001'", CID.Value, Session["usersegid"].ToString(), Session["LastCompany"].ToString(), Convert.ToDateTime(dtfor.Value));

                if (dtfund.Rows.Count > 0)
                {
                    netfund = Convert.ToDecimal(dtfund.Rows[0][0].ToString());
                    if (netfund < 0)
                    {
                        strHtml += "<tr><td align=left style=\"width:30%;\">Net Fund Adjustment For the Period :</td><td align=\"right\" style=\"width:13%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(netfund))) + "</td><td style=\"width:7%;\"> &nbsp;</td></tr>";

                    }
                    else
                    {
                        strHtml += "<tr><td align=left style=\"width:30%;\">Net Fund Adjustment For the Period :</td><td style=\"width:10%;\"> &nbsp;</td><td align=\"right\" style=\"width:10%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(netfund))) + "</td></tr>";
                    }
                }
                netamount = totalobligation - totalcharges + (openingbalance + netfund);

                if (netamount < 0)
                {
                    strHtml += "<tr><td align=left style=\"width:30%;\">Net Amount Receivable By Us: </td><td align=\"right\" style=\"width:13%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(netamount))) + "</td><td style=\"width:7%;\"> &nbsp;</td></tr>";

                }
                else
                {
                    strHtml += "<tr><td align=left style=\"width:30%;\">Net Amount Payable To You :</td><td style=\"width:10%;\"> &nbsp;</td><td align=\"right\" style=\"width:10%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(netamount))) + "</td></tr>";
                }
                strHtml += "</div></table></td></tr><tr style=\"height:1px;background-color:White;\"><td colspan=11>&nbsp;</td></tr>";
                ///////////////////////////////END///////////////////////////////////////
                ////////////////////////////////Margin obligation///////////////////////
                if (ds.Tables[9].Rows.Count != 0)
                {

                    strHtml += "<tr style=\"background-color: #DBEEF3;\">";
                    strHtml += "<td align=\"center\" ><b>Margin Summary</b></td>";
                    strHtml += "<td align=\"center\" ><b>Span Margin</b></td>";
                    strHtml += "<td align=\"center\"><b>Premium Margin</b></td>";
                    strHtml += "<td align=\"center\"><b>Total Margin</b></td>";
                    strHtml += "<td align=\"center\"><b>Exposure Margin</b></td>";
                    strHtml += "<td align=\"center\"><b>Applicable Margin</b></td>";
                    strHtml += "<td align=\"center\"><b>Cash Dep</b></td>";
                    strHtml += "<td align=\"center\"><b><b>Collaterals</b></td>";
                    strHtml += "<td align=\"center\"><b>Total Deposit</b></td>";
                    strHtml += "<td align=\"center\"><b>Shortage</b></td>";
                    strHtml += "<td align=\"center\"><b>Excess</b></td></tr>";

                    strHtml += "<tr style=\"background-color: white;\"><td> &nbsp;</td>";
                    strHtml += "<td  align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[9].Rows[0]["SpanMargin"])) + "</td>";
                    strHtml += "<td  align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[9].Rows[0]["BuyPremium"])) + "</td>";
                    strHtml += "<td  align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[9].Rows[0]["TotalMargin"])) + "</td>";
                    strHtml += "<td  align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[9].Rows[0]["ExposureMargin"])) + "</td>";
                    strHtml += "<td  align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[9].Rows[0]["ApplicableMargin"])) + "</td>";

                    DataTable dtmargin = new DataTable();
                    dtmargin = oDBEngine.OpeningBalanceOnlyJournal("'SYSTM00002','SYSTM00003'", CID.Value, Convert.ToDateTime(dtfor.Value), Session["usersegid"].ToString(), Session["LastCompany"].ToString(), Convert.ToDateTime(dtfor.Value));
                    if (dtmargin.Rows.Count > 0)
                    {
                        strHtml += "<td  align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtmargin.Rows[0][0].ToString())) + "</td>";
                        totaldeposit = Convert.ToDecimal(dtmargin.Rows[0][0].ToString()) + Convert.ToDecimal(ds.Tables[9].Rows[0]["coleteral"]);

                    }
                    else
                    {
                        strHtml += "<td> &nbsp;</td>";
                    }
                    strHtml += "<td  align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[9].Rows[0]["coleteral"])) + "</td>";
                    strHtml += "<td  align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(totaldeposit)) + "</td>";

                    stortageexcess = totaldeposit - Convert.ToDecimal(ds.Tables[9].Rows[0]["ApplicableMargin"]);
                    if (stortageexcess < 0)
                    {
                        strHtml += "<td  align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(Math.Abs(stortageexcess))) + "</td>";
                        strHtml += "<td align=\"right\"></td></tr>";
                    }
                    else
                    {
                        strHtml += "<td  align=\"right\"></td>";
                        strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(stortageexcess)) + "</td></tr>";
                    }

                }

                ////////////////////////////////END////////////////////////////////////
                ////////////////////////////////////Net Amount After Adjusting Margin Shortage :///////////////////////////

                strHtml += "<tr><td style=\"height:10px;background-color:White;\" colspan=11></td></tr><tr style=\"background-color:White;\">";
                strHtml += "<td align=right colspan=11><div style=\"border: solid 1px balck;width:50%;\">";
                strHtml += "<table width=\"100%\" border=" + Convert.ToChar(34) + 0 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr>";

                if (stortageexcess < 0)
                {
                    if (netamount + stortageexcess > 0)
                    {
                        strHtml += "<td align=\"left\" style=\"width:35%;\">Net Amount Payable  After Adjusting Margin Shortage :</td>";
                        strHtml += "<td align=\"right\" style=\"width:15%;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs((netamount + stortageexcess))) + "</b></td>";

                    }
                    else
                    {
                        strHtml += "<td align=\"left\" style=\"width:35%;\">Net Amount Receivable By Us After Adjusting Margin Shortage :</td>";
                        strHtml += "<td align=\"right\" style=\"width:15%;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs((netamount + stortageexcess))) + "</b></td>";

                    }
                }
                else
                {
                    if (netamount > 0)
                    {
                        strHtml += "<td align=\"left\" style=\"width:35%;\">Net Amount Payable To You After Adjusting Margin Shortage :</td>";
                        strHtml += "<td style=\"width:10%;\"></td><td align=\"right\" style=\"width:10%;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs((netamount))) + "</b></td>";

                    }
                    else
                    {
                        strHtml += "<td align=\"left\" style=\"width:35%;\">Net Amount Receivable BY Us After Adjusting Margin Shortage :</td>";
                        strHtml += "<td align=\"right\" style=\"width:15%;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs((netamount))) + "</b></td>";

                    }
                }
                strHtml += "</tr></table></div></td></tr>";

                //////////////////////////////////////////END////////////////////////////////////////////////

                strHtml += "</table>";
                display.InnerHtml = strHtml;
                ds.Dispose();
                /////////////////////////////DISPLAY DATE/////////////////////////////////////

                string group = null;
                if (ddlGroup.SelectedItem.Value.ToString() == "0")
                {
                    group = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise Report";
                }
                else
                {
                    group = ddlGroup.SelectedItem.Text + " Wise [ " + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "] Report";
                }
                if (ddldate.SelectedItem.Value == "0")
                {

                    string SpanText = group + " For" + oconverter.ArrangeDate2(dtfor.Value.ToString());
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct", "displaydate('" + SpanText + "')", true);

                }


                ///////////////////////////////END//////////////////////////////////////////
                ScriptManager.RegisterStartupScript(this, this.GetType(), "displaygrid", "displayresult();", true);



            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "norecord", "norecord(1);", true);

            }

        }
        void bindTableFORAPeriod()
        {

            String strHtml = String.Empty;
            if (ds.Tables[0].Rows.Count != 0 || ds.Tables[3].Rows.Count != 0 || ds.Tables[4].Rows.Count != 0)
            {
                decimal totalmtm = decimal.Zero;
                decimal totalexp = decimal.Zero;
                decimal totalmtmexp = decimal.Zero;
                decimal mtm = decimal.Zero;
                decimal exp = decimal.Zero;

                decimal prm = decimal.Zero;
                decimal asnexc = decimal.Zero; ;
                string expirydate = null;

                decimal totaldeposit = decimal.Zero;
                decimal stortageexcess = decimal.Zero;
                strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 0 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

                if (ds.Tables[0].Rows.Count != 0)
                {

                    DataTable dtstore1st = new DataTable();
                    dtstore1st = ds.Tables[1];
                    DataView dv = new DataView(dtstore1st);

                    DataTable dtstore2st = new DataTable();
                    dtstore2st = ds.Tables[2];
                    DataView dv1 = new DataView(dtstore2st);




                    //////////////////////MAIN TABLE/////////////////////////

                    /////////////////////////////FOR MTM////////////////////
                    strHtml += "<tr style=\"background-color: #DBEEF3;\">";
                    strHtml += "<td align=\"center\" colspan=2><b>MTM Obligation</b></td>";
                    strHtml += "<td align=\"center\"><b>Trade Date</b></td>";
                    strHtml += "<td align=\"center\"><b>Trade No.</b></td>";
                    strHtml += "<td align=\"center\"><b>Buy Qty</b></td>";
                    strHtml += "<td align=\"center\"><b>Sell Qty</b></td>";
                    strHtml += "<td align=\"center\"><b>Mkt Rate</b></td>";
                    strHtml += "<td align=\"center\"><b>Brkg</b></td>";
                    strHtml += "<td align=\"center\"><b>Net Rate</b></td>";
                    strHtml += "<td align=\"center\"><b>Amount Dr.</b></td>";
                    strHtml += "<td align=\"center\"><b>Amount Cr.</b></td></tr>";

                    /////////////////////////////////////Display Opening Ledger Balance//////////////////////////////

                    DataTable dtOpen = new DataTable();
                    dtOpen = oDBEngine.OpeningBalanceOnlyJournal("'SYSTM00001'", CID.Value, Convert.ToDateTime(dtfrom.Value), Session["usersegid"].ToString(), Session["LastCompany"].ToString(), Convert.ToDateTime(dtto.Value));
                    if (dtOpen.Rows.Count > 0)
                    {
                        openingbalance = Convert.ToDecimal(dtOpen.Rows[0][1].ToString());
                        if (openingbalance < 0)
                        {
                            strHtml += "<tr style=\"background-color:White;\"><td style=\"color:Blue;border-top:2px;border-bottom:2px;border-style: groove;border-right-style: none; \" align=\"right\" colspan=9><b>Opening Ledger Balance as on " + oconverter.ArrangeDate2(dtfrom.Value.ToString()) + "<b/></td>";
                            strHtml += "<td align=\"center\" style=\"color:Black;border-top:2px;border-bottom:2px;border-style: groove;border-right-style: none;border-left-style: none; \"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(Math.Abs(openingbalance))) + "</b></td>";
                            strHtml += "<td align=\"center\" style=\"color:Black;border-top:2px;border-bottom:2px;border-style: groove;border-left-style: none; \">&nbsp;</td></tr>";
                        }
                        else
                        {
                            strHtml += "<tr style=\"background-color:White;\"><td style=\"color:Blue;border-top:2px;border-bottom:2px;border-style: groove;border-right-style: none; \" align=\"right\" colspan=9><b>Opening Ledger Balance as on " + oconverter.ArrangeDate2(dtfrom.Value.ToString()) + "<b/></td>";
                            strHtml += "<td align=\"center\" style=\"color:Black;border-top:2px;border-bottom:2px;border-style: groove;border-right-style: none;border-left-style: none;  \">&nbsp;</td>";
                            strHtml += "<td align=\"center\" style=\"color:Black;border-top:2px;border-bottom:2px;border-style: groove;border-left-style: none; \"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(openingbalance)) + "</b></td></tr>";
                        }
                    }


                    ///////////////////////////////////////////////ALL MTM DATA BIND///////////////////////////////////////////////
                    int flag = 0;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        totalmtm = decimal.Zero;
                        totalexp = decimal.Zero;

                        flag = flag + 1;
                        //////////////////////////////******** BF POSITION DATA BIND***********///////////////////////////
                        strHtml += "<tr style=\"background-color: " + GetRowColor(flag) + " ;text-align:left\">";
                        strHtml += "<td colspan=2 align=left style=\"color:maroon;\">" + dr["tabSymbol"] + "</td>";
                        expirydate = dr["expirydate"].ToString();
                        if (dr["BFQuantity"] != DBNull.Value)
                        {
                            strHtml += "<td colspan=2 align=left  style=\"color:blue;\">Brought Forward</td>";

                            if (Convert.ToDecimal(dr["BFQuantity"]) < 0)
                            {
                                strHtml += "<td align=right>" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Math.Abs(Convert.ToDecimal(dr["BFQuantity"].ToString()))) + "</td>";
                                strHtml += "<td> &nbsp;</td>";
                            }
                            else
                            {
                                strHtml += "<td> &nbsp;</td>";
                                strHtml += "<td align=right>" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dr["BFQuantity"].ToString())) + "</td>";
                            }
                            strHtml += "<td align=right>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dr["OpenPrice"].ToString())) + "</td>";
                            strHtml += "<td> &nbsp;</td>";
                            strHtml += "<td> &nbsp;</td>";
                            if (Convert.ToDecimal(dr["BFValue"]) < 0)
                            {
                                strHtml += "<td align=right>" + oconverter.getFormattedvaluewithoriginalsign(Math.Abs(Convert.ToDecimal(dr["BFValue"].ToString()))) + "</td>";
                                strHtml += "<td> &nbsp;</td>";
                            }
                            else
                            {
                                strHtml += "<td> &nbsp;</td>";
                                strHtml += "<td align=right>" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dr["BFValue"].ToString())) + "</td>";
                            }
                            if (Convert.ToDateTime(expirydate) == Convert.ToDateTime(dtto.Value))
                            {
                                totalexp = Convert.ToDecimal(dr["BFValue"]);
                            }
                            else
                            {
                                totalmtm = Convert.ToDecimal(dr["BFValue"]);
                            }

                        }
                        else
                        {
                            strHtml += "<td colspan=9> &nbsp;</td>";
                        }


                        strHtml += "</tr>";
                        ////////////////////////////////********BF POSITION DATA END*********/////////////////////////

                        dv.RowFilter = "tabProductSeriesID = '" + dr["tabProductSeriesID"] + "'";
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        dt = dv.ToTable();
                        foreach (DataRow row in dt.Rows)
                        {
                            ///////////////////////////////*********ALL TRADE FETCH*************//////////////////////////

                            flag = flag + 1;
                            strHtml += "<tr style=\"background-color: " + GetRowColor(flag) + " ;text-align:left\">";
                            strHtml += "<td colspan=2> &nbsp;</td>";
                            strHtml += "<td align=\"left\">" + row["MTMTradeDate"] + "</td>";
                            if (row["MTMRefID"].ToString() != "Expiry")
                            {
                                strHtml += "<td align=\"right\" >" + row["MTMRefID"] + "</td>";
                            }
                            else
                            {
                                strHtml += "<td align=\"right\" ><b>" + row["MTMRefID"] + "</b></td>";

                            }

                            if (row["MTMBUYQUANTITY"] != DBNull.Value)
                            {
                                strHtml += "<td align=\"right\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(row["MTMBUYQUANTITY"].ToString())) + "</td>";
                            }
                            else
                            {
                                strHtml += "<td>&nbsp;</td>";
                            }

                            if (row["MTMSELLQUANTITY"] != DBNull.Value)
                            {
                                strHtml += "<td align=\"right\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(row["MTMSELLQUANTITY"].ToString())) + "</td>";
                            }
                            else
                            {
                                strHtml += "<td> &nbsp;</td>";
                            }

                            if (row["MktRate"] != DBNull.Value)
                            {
                                strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(row["MktRate"].ToString())) + "</td>";
                            }
                            else
                            {
                                strHtml += "<td> &nbsp;</td>";
                            }

                            if (row["MTMBrkg"] != DBNull.Value)
                            {
                                strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(row["MTMBrkg"].ToString())) + "</td>";
                            }
                            else
                            {
                                strHtml += "<td> &nbsp;</td>";
                            }

                            if (row["MTMNetRatePerUnit"] != DBNull.Value)
                            {
                                strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(row["MTMNetRatePerUnit"].ToString())) + "</td>";

                            }
                            else
                            {
                                strHtml += "<td> &nbsp;</td>";
                            }


                            if (row["MTMAMOUNTDR"] != DBNull.Value)
                            {
                                strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluewithoriginalsign(Math.Abs(Convert.ToDecimal(row["MTMAMOUNTDR"].ToString()))) + "</td>";

                                if (Convert.ToDateTime(expirydate) == Convert.ToDateTime(dtto.Value))
                                {
                                    totalexp = totalexp - Convert.ToDecimal(row["MTMAMOUNTDR"]);
                                }
                                else
                                {
                                    totalmtm = totalmtm - Convert.ToDecimal(row["MTMAMOUNTDR"]);
                                }
                            }
                            else
                            {
                                strHtml += "<td> &nbsp;</td>";
                            }

                            if (row["MTMAMOUNTCR"] != DBNull.Value)
                            {
                                strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(row["MTMAMOUNTCR"].ToString())) + "</td>";
                                if (Convert.ToDateTime(expirydate) == Convert.ToDateTime(dtto.Value))
                                {
                                    totalexp = totalexp + Convert.ToDecimal(row["MTMAMOUNTCR"]);
                                }
                                else
                                {
                                    totalmtm = totalmtm + Convert.ToDecimal(row["MTMAMOUNTCR"]);
                                }
                            }
                            else
                            {
                                strHtml += "<td align=\"center\"> &nbsp;</td>";
                            }

                            strHtml += "</tr>";

                        }

                        //////////////////////////////**********ALL TRADE FETCH END********///////////////////////////
                        dv1.RowFilter = "tabProductSeriesID = '" + dr["tabProductSeriesID"] + "'";
                        DataTable dt1 = new DataTable();
                        dt1 = ds.Tables[0];
                        dt1 = dv1.ToTable();
                        foreach (DataRow row1 in dt1.Rows)
                        {
                            if (row1["CFQuantity"] != DBNull.Value)
                            {
                                flag = flag + 1;
                                //////////////////////////////******** CF POSITION DATA BIND***********///////////////////////////
                                strHtml += "<tr style=\"background-color: " + GetRowColor(flag) + " ;text-align:left\">";
                                strHtml += "<td colspan=2> &nbsp;</td>";


                                if (Convert.ToDateTime(expirydate) == Convert.ToDateTime(dtto.Value))
                                {
                                    strHtml += "<td colspan=2 align=left style=\"color:Green;\">Expiry</td>";
                                }
                                else
                                {
                                    strHtml += "<td colspan=2 align=left style=\"color:Green;\">Carried Forward</td>";
                                }

                                if (Convert.ToDecimal(row1["CFQuantity"]) > 0)/////////OPPOSITE DIRECTION
                                {
                                    strHtml += "<td align=right>" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Math.Abs(Convert.ToDecimal(row1["CFQuantity"].ToString()))) + "</td>";
                                    strHtml += "<td> &nbsp;</td>";
                                }
                                else
                                {
                                    strHtml += "<td> &nbsp;</td>";
                                    strHtml += "<td align=right>" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Math.Abs(Convert.ToDecimal(row1["CFQuantity"].ToString()))) + "</td>";
                                }
                                strHtml += "<td align=right>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(row1["SettPrice"].ToString())) + "</td>";
                                strHtml += "<td> &nbsp;</td>";
                                strHtml += "<td> &nbsp;</td>";
                                if (Convert.ToDecimal(row1["CFValue"]) > 0)/////////OPPOSITE DIRECTION
                                {
                                    strHtml += "<td align=right>" + oconverter.getFormattedvaluewithoriginalsign(Math.Abs(Convert.ToDecimal(row1["CFValue"].ToString()))) + "</td>";
                                    strHtml += "<td> &nbsp;</td>";
                                }
                                else
                                {
                                    strHtml += "<td> &nbsp;</td>";
                                    strHtml += "<td align=right>" + oconverter.getFormattedvaluewithoriginalsign(Math.Abs(Convert.ToDecimal(row1["CFValue"].ToString()))) + "</td>";
                                }

                                strHtml += "</tr>";

                                if (Convert.ToDateTime(expirydate) == Convert.ToDateTime(dtto.Value))
                                {
                                    totalexp = totalexp - Convert.ToDecimal(row1["CFValue"]);
                                }
                                else
                                {
                                    totalmtm = totalmtm - Convert.ToDecimal(row1["CFValue"]);
                                }
                            }

                        }
                        ////////////////////////////////********CF POSITION DATA END*********/////////////////////////
                        //////////////////////////////////SUMMARY DISPLAY/////////////////////////////////////////////
                        strHtml += "<tr style=\"background-color:White;\">";
                        strHtml += "<td align=right colspan=11><div style=\"background-color:lavender;width:30%;\">";
                        strHtml += "<table width=\"100%\" border=" + Convert.ToChar(34) + 0 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

                        if (Convert.ToDateTime(expirydate) == Convert.ToDateTime(dtto.Value))
                        {
                            strHtml += "<tr><td align=left style=\"width:10%;\">Final Settlement</td>";
                            if (totalexp < 0)
                            {
                                strHtml += "<td align=\"right\" style=\"width:13%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(totalexp))) + "</b></td>";
                                strHtml += "<td style=\"width:7%;\"> &nbsp;</td>";
                                strHtml += "</tr>";
                            }
                            else
                            {
                                strHtml += "<td style=\"width:10%;\"> &nbsp;</td>";
                                strHtml += "<td align=\"right\" style=\"width:10%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(totalexp))) + "</b></td>";
                                strHtml += "</tr>";
                            }
                        }
                        else
                        {
                            strHtml += "<tr><td align=left style=\"width:10%;\">MTM Settlement</td>";

                            if (totalmtm < 0)
                            {
                                strHtml += "<td align=\"right\" style=\"width:13%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(totalmtm))) + "</b></td>";
                                strHtml += "<td style=\"width:7%;\"> &nbsp;</td>";
                                strHtml += "</tr>";
                            }
                            else
                            {
                                strHtml += "<td style=\"width:10%;\"> &nbsp;</td>";
                                strHtml += "<td align=\"right\" style=\"width:10%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(totalmtm))) + "</b></td>";
                                strHtml += "</tr>";
                            }
                        }
                        strHtml += "</div></table></td></tr>";
                        mtm = mtm + totalmtm;
                        exp = exp + totalexp;
                        //////////////////////////////////////END//////////////////////////////////////////////////////

                    }
                    //////////////////////////////////Final Settlement of All Contracts DISPLAY/////////////////////////////////////////////
                    strHtml += "<tr style=\"height:1px;background-color:White;\"><td colspan=11>&nbsp;</td></tr><tr style=\"background-color:White;\">";
                    strHtml += "<td align=right colspan=11><div style=\"border: solid 1px balck;width:50%;\">";
                    strHtml += "<table width=\"100%\" border=" + Convert.ToChar(34) + 0 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                    totalmtmexp = mtm + exp;

                    if (exp != 0)
                    {
                        strHtml += "<tr><td align=left style=\"width:35%;\">Final Settlement of All Contracts:</td>";
                        if (exp < 0)
                        {
                            strHtml += "<td align=\"right\" style=\"width:19%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(exp))) + "</b></td>";
                            strHtml += "<td style=\"width:7%;\"></td>";
                            strHtml += "</tr>";
                        }
                        else
                        {
                            strHtml += "<td style=\"width:10%;\"> &nbsp;</td>";
                            strHtml += "<td align=\"right\" style=\"width:15%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(exp))) + "</b></td>";
                            strHtml += "</tr>";
                        }
                    }
                    if (mtm != 0)
                    {
                        strHtml += "<tr><td align=left style=\"width:35%;\">MTM Of all contracts:</td>";

                        if (mtm < 0)
                        {
                            strHtml += "<td align=\"right\" style=\"width:26%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(mtm))) + "</b></td>";
                            strHtml += "<td style=\"width:10%;\"></td>";
                            strHtml += "</tr>";
                        }
                        else
                        {
                            strHtml += "<td style=\"width:10%;\"> &nbsp;</td>";
                            strHtml += "<td align=\"right\" style=\"width:15%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(mtm))) + "</b></td>";
                            strHtml += "</tr>";
                        }
                    }
                    strHtml += "<tr><td align=left style=\"width:35%;\">Net MTM and Final Settlement of All Contracts:</td>";

                    if (totalmtmexp < 0)
                    {
                        strHtml += "<td align=\"right\" style=\"width:19%;color:maroon;\"> <b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(totalmtmexp))) + "</b></td>";
                        strHtml += "<td style=\"width:7%;\"></td>";
                        strHtml += "</tr>";
                    }
                    else
                    {
                        strHtml += "<td style=\"width:10%;\"> &nbsp;</td>";
                        strHtml += "<td align=\"right\" style=\"width:15%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(totalmtmexp))) + "</b></td>";
                        strHtml += "</tr>";
                    }

                    strHtml += "</div></table></td></tr><tr style=\"height:1px;background-color:White;\"><td colspan=11>&nbsp;</td></tr>";
                    //////////////////////////////////////END//////////////////////////////////////////////////////

                }



                /////////////////////////////FOR Premium////////////////////
                if (ds.Tables[3].Rows.Count != 0)
                {
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        /////////////////////////////////////Display Opening Ledger Balance//////////////////////////////

                        DataTable dtOpen = new DataTable();
                        dtOpen = oDBEngine.OpeningBalanceOnlyJournal("'SYSTM00001'", CID.Value, Convert.ToDateTime(dtfrom.Value), Session["usersegid"].ToString(), Session["LastCompany"].ToString(), Convert.ToDateTime(dtto.Value));
                        if (dtOpen.Rows.Count > 0)
                        {
                            openingbalance = Convert.ToDecimal(dtOpen.Rows[0][1].ToString());
                            if (openingbalance < 0)
                            {
                                strHtml += "<tr style=\"background-color:White;\"><td style=\"color:Blue;border-top:2px;border-bottom:2px;border-style: groove;border-right-style: none; \" align=\"right\" colspan=9><b>Opening Ledger Balance as on " + oconverter.ArrangeDate2(dtfrom.Value.ToString()) + "<b/></td>";
                                strHtml += "<td align=\"center\" style=\"color:Black;border-top:2px;border-bottom:2px;border-style: groove;border-right-style: none;border-left-style: none; \"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(Math.Abs(openingbalance))) + "</b></td>";
                                strHtml += "<td align=\"center\" style=\"color:Black;border-top:2px;border-bottom:2px;border-style: groove;border-left-style: none; \">&nbsp;</td></tr>";
                            }
                            else
                            {
                                strHtml += "<tr style=\"background-color:White;\"><td style=\"color:Blue;border-top:2px;border-bottom:2px;border-style: groove;border-right-style: none; \" align=\"right\" colspan=9><b>Opening Ledger Balance as on " + oconverter.ArrangeDate2(dtfrom.Value.ToString()) + "<b/></td>";
                                strHtml += "<td align=\"center\" style=\"color:Black;border-top:2px;border-bottom:2px;border-style: groove;border-right-style: none;border-left-style: none;  \">&nbsp;</td>";
                                strHtml += "<td align=\"center\" style=\"color:Black;border-top:2px;border-bottom:2px;border-style: groove;border-left-style: none; \"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(openingbalance)) + "</b></td></tr>";
                            }
                        }
                    }
                    strHtml += "<tr style=\"background-color: #DBEEF3;\">";
                    strHtml += "<td align=\"center\" ><b>Premium Obligation</b></td>";
                    strHtml += "<td align=\"center\" ><b>Strike Price</b></td>";
                    strHtml += "<td align=\"center\"><b>Trade Date</b></td>";
                    strHtml += "<td align=\"center\"><b>Trade No.</b></td>";
                    strHtml += "<td align=\"center\"><b>Buy Qty</b></td>";
                    strHtml += "<td align=\"center\"><b>Sell Qty</b></td>";
                    strHtml += "<td align=\"center\"><b>Premium</b></td>";
                    strHtml += "<td align=\"center\"><b>Brkg</b></td>";
                    strHtml += "<td align=\"center\"><b>Net Prm</b></td>";
                    strHtml += "<td align=\"center\"><b>Amount Dr.</b></td>";
                    strHtml += "<td align=\"center\"><b>Amount Cr.</b></td></tr>";
                    int flag1 = 0;
                    string productid = null;
                    decimal netprm = decimal.Zero;
                    foreach (DataRow row in ds.Tables[3].Rows)
                    {
                        ////////////////////////////////////PRIMIUM SUMMARY DISPLAY///////////////////////////
                        if (productid != null)
                        {
                            if (productid != row["tabProductSeriesID"].ToString())
                            {

                                strHtml += "<tr style=\"background-color:White;\">";
                                strHtml += "<td align=right colspan=11><div style=\"background-color:lavender;width:30%;\">";
                                strHtml += "<table width=\"100%\" border=" + Convert.ToChar(34) + 0 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                                strHtml += "<tr><td align=left style=\"width:10%;\">Net Premium </td>";
                                if (netprm < 0)
                                {
                                    strHtml += "<td align=\"right\" style=\"width:13%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(netprm))) + "</b></td>";
                                    strHtml += "<td style=\"width:7%;\"> &nbsp;</td>";
                                    strHtml += "</tr>";
                                }
                                else
                                {
                                    strHtml += "<td style=\"width:10%;\"> &nbsp;</td>";
                                    strHtml += "<td align=\"right\" style=\"width:10%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(netprm))) + "</b></td>";
                                    strHtml += "</tr>";
                                }
                                prm = prm + netprm;
                                netprm = decimal.Zero;
                                strHtml += "</table></div></td></tr>";
                            }
                        }
                        //////////////////////////////////////////END////////////////////////////////////////////////
                        productid = row["tabProductSeriesID"].ToString();

                        flag1 = flag1 + 1;

                        strHtml += "<tr style=\"background-color: " + GetRowColor(flag1) + " ;text-align:left\">";
                        if (netprm == 0)
                        {
                            strHtml += "<td align=\"left\"  style=\"color:maroon;width:20%;\">" + row["tabSymbol"] + "</td>";
                            if (row["StrikePrice"] != DBNull.Value)
                            {
                                strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(row["StrikePrice"].ToString())) + "</td>";

                            }
                            else
                            {
                                strHtml += "<td>&nbsp;</td>";
                            }
                        }
                        else
                        {
                            strHtml += "<td>&nbsp;</td>";
                            strHtml += "<td>&nbsp;</td>";
                        }
                        strHtml += "<td align=\"left\">" + row["PRMTradeDate"] + "</td>";
                        strHtml += "<td align=\"right\">" + row["PRMRefNo"] + "</td>";

                        if (row["PRMBUYQUANTITY"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(row["PRMBUYQUANTITY"].ToString())) + "</td>";
                        }
                        else
                        {
                            strHtml += "<td>&nbsp;</td>";
                        }

                        if (row["PRMSELLQUANTITY"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(row["PRMSELLQUANTITY"].ToString())) + "</td>";
                        }
                        else
                        {
                            strHtml += "<td> &nbsp;</td>";
                        }
                        if (row["PRMUnitPrice"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(row["PRMUnitPrice"].ToString())) + "</td>";
                        }
                        else
                        {
                            strHtml += "<td> &nbsp;</td>";
                        }


                        if (row["PRMBrkg"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(row["PRMBrkg"].ToString())) + "</td>";
                        }
                        else
                        {
                            strHtml += "<td> &nbsp;</td>";
                        }

                        if (row["PRMNetRatePerUnit"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(row["PRMNetRatePerUnit"].ToString())) + "</td>";

                        }
                        else
                        {
                            strHtml += "<td> &nbsp;</td>";
                        }


                        if (row["PRMAMOUNTDR"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluewithoriginalsign(Math.Abs(Convert.ToDecimal(row["PRMAMOUNTDR"].ToString()))) + "</td>";
                            netprm = netprm - Convert.ToDecimal(row["PRMAMOUNTDR"]);
                        }
                        else
                        {
                            strHtml += "<td> &nbsp;</td>";
                        }

                        if (row["PRMAMOUNTCR"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(row["PRMAMOUNTCR"].ToString())) + "</td>";
                            netprm = netprm + Convert.ToDecimal(row["PRMAMOUNTCR"]);

                        }
                        else
                        {
                            strHtml += "<td> &nbsp;</td>";
                        }
                        strHtml += "</tr>";



                    }
                    ////////////////////////////////////PRIMIUM SUMMARY DISPLAY///////////////////////////

                    strHtml += "<tr style=\"background-color:White;\">";
                    strHtml += "<td align=right colspan=11><div style=\"background-color:lavender;width:30%;\">";
                    strHtml += "<table width=\"100%\" border=" + Convert.ToChar(34) + 0 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                    strHtml += "<tr><td align=left style=\"width:10%;\">Net Premium </td>";
                    if (netprm < 0)
                    {
                        strHtml += "<td align=\"right\" style=\"width:13%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(netprm))) + "</b></td>";
                        strHtml += "<td style=\"width:7%;\"> &nbsp;</td>";
                        strHtml += "</tr>";
                    }
                    else
                    {
                        strHtml += "<td style=\"width:10%;\"> &nbsp;</td>";
                        strHtml += "<td align=\"right\" style=\"width:10%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(netprm))) + "</b></td>";
                        strHtml += "</tr>";
                    }
                    prm = prm + netprm;
                    netprm = decimal.Zero;
                    strHtml += "</table></div></td></tr>";

                    //////////////////////////////////////////END////////////////////////////////////////////////
                    //////////////////////////////////Net Premium of All Contracts DISPLAY/////////////////////////////////////////////
                    strHtml += "<tr style=\"height:1px;background-color:White;\"><td colspan=11>&nbsp;</td></tr><tr style=\"background-color:White;\">";
                    strHtml += "<td align=right colspan=11><div style=\"border: solid 1px balck;width:35%;\">";
                    strHtml += "<table width=\"100%\" border=" + Convert.ToChar(34) + 0 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                    strHtml += "<tr><td align=left style=\"width:15%;\">Net Premium of all contracts:</td>";
                    if (prm < 0)
                    {
                        strHtml += "<td align=\"right\" style=\"width:11%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(prm))) + "</b></td>";
                        strHtml += "<td style=\"width:4%;\"> &nbsp;</td>";
                        strHtml += "</tr>";
                    }
                    else
                    {
                        strHtml += "<td style=\"width:5%;\"> &nbsp;</td>";
                        strHtml += "<td align=\"right\" style=\"width:15%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(prm))) + "</b></td>";
                        strHtml += "</tr>";
                    }

                    strHtml += "</div></table></td></tr><tr style=\"height:1px;background-color:White;\"><td colspan=11>&nbsp;</td></tr>";
                    //////////////////////////////////////END//////////////////////////////////////////////////////
                }

                /////////////////////////////FOR Premium END//////////////////// 

                /////////////////////////////FOR ASN,EXC////////////////////
                if (ds.Tables[4].Rows.Count != 0)
                {
                    if (ds.Tables[0].Rows.Count == 0 && ds.Tables[3].Rows.Count == 0)
                    {
                        /////////////////////////////////////Display Opening Ledger Balance//////////////////////////////

                        DataTable dtOpen = new DataTable();
                        dtOpen = oDBEngine.OpeningBalanceOnlyJournal("'SYSTM00001'", CID.Value, Convert.ToDateTime(dtfrom.Value), Session["usersegid"].ToString(), Session["LastCompany"].ToString(), Convert.ToDateTime(dtto.Value));
                        if (dtOpen.Rows.Count > 0)
                        {
                            openingbalance = Convert.ToDecimal(dtOpen.Rows[0][1].ToString());
                            if (openingbalance < 0)
                            {
                                strHtml += "<tr style=\"background-color:White;\"><td style=\"color:Blue;border-top:2px;border-bottom:2px;border-style: groove;border-right-style: none; \" align=\"right\" colspan=9><b>Opening Ledger Balance as on " + oconverter.ArrangeDate2(dtfrom.Value.ToString()) + "<b/></td>";
                                strHtml += "<td align=\"center\" style=\"color:Black;border-top:2px;border-bottom:2px;border-style: groove;border-right-style: none;border-left-style: none; \"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(Math.Abs(openingbalance))) + "</b></td>";
                                strHtml += "<td align=\"center\" style=\"color:Black;border-top:2px;border-bottom:2px;border-style: groove;border-left-style: none; \">&nbsp;</td></tr>";
                            }
                            else
                            {
                                strHtml += "<tr style=\"background-color:White;\"><td style=\"color:Blue;border-top:2px;border-bottom:2px;border-style: groove;border-right-style: none; \" align=\"right\" colspan=9><b>Opening Ledger Balance as on " + oconverter.ArrangeDate2(dtfrom.Value.ToString()) + "<b/></td>";
                                strHtml += "<td align=\"center\" style=\"color:Black;border-top:2px;border-bottom:2px;border-style: groove;border-right-style: none;border-left-style: none;  \">&nbsp;</td>";
                                strHtml += "<td align=\"center\" style=\"color:Black;border-top:2px;border-bottom:2px;border-style: groove;border-left-style: none; \"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(openingbalance)) + "</b></td></tr>";
                            }
                        }
                    }
                    strHtml += "<tr style=\"background-color: #DBEEF3;\">";
                    strHtml += "<td align=\"center\" ><b>Options Final Settlement</b></td>";
                    strHtml += "<td align=\"center\" ><b>Strike Price</b></td>";
                    strHtml += "<td align=\"center\"><b>Trade Date</b></td>";
                    strHtml += "<td align=\"center\"><b>Sett.Type</b></td>";
                    strHtml += "<td align=\"center\"><b>Qty</b></td>";
                    strHtml += "<td align=\"center\"><b>Sett Price</b></td>";
                    strHtml += "<td align=\"center\"><b>Rate</b></td>";
                    strHtml += "<td align=\"center\"><b>Sett Charg</b></td>";
                    strHtml += "<td align=\"center\"><b>Net Rate</b></td>";
                    strHtml += "<td align=\"center\"><b>Amount Dr.</b></td>";
                    strHtml += "<td align=\"center\"><b>Amount Cr.</b></td></tr>";
                    int flag2 = 0;
                    foreach (DataRow row in ds.Tables[4].Rows)
                    {
                        flag2 = flag2 + 1;

                        strHtml += "<tr style=\"background-color: " + GetRowColor(flag2) + " ;text-align:left\">";
                        strHtml += "<td align=\"left\"  style=\"color:maroon;\">" + row["tabSymbol"] + "</td>";
                        if (row["StrikePrice"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(row["StrikePrice"].ToString())) + "</td>";

                        }
                        else
                        {
                            strHtml += "<td>&nbsp;</td>";
                        }
                        strHtml += "<td align=\"left\">" + row["ASNEXCTradeDate"] + "</td>";
                        strHtml += "<td align=\"left\">" + row["setttype"] + "</td>";

                        if (row["ASNEXCQUANTITY"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(row["ASNEXCQUANTITY"].ToString())) + "</td>";
                        }
                        else
                        {
                            strHtml += "<td>&nbsp;</td>";
                        }

                        if (row["ASNEXCSettPrice"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(row["ASNEXCSettPrice"].ToString())) + "</td>";
                        }
                        else
                        {
                            strHtml += "<td> &nbsp;</td>";
                        }
                        if (row["ASNEXCUnitPrice"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(row["ASNEXCUnitPrice"].ToString())) + "</td>";
                        }
                        else
                        {
                            strHtml += "<td> &nbsp;</td>";
                        }


                        if (row["ASNEXCBrkg"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(row["ASNEXCBrkg"].ToString())) + "</td>";
                        }
                        else
                        {
                            strHtml += "<td> &nbsp;</td>";
                        }

                        if (row["ASNEXCNetRatePerUnit"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(row["ASNEXCNetRatePerUnit"].ToString())) + "</td>";

                        }
                        else
                        {
                            strHtml += "<td> &nbsp;</td>";
                        }


                        if (row["ASNEXCAMOUNTDR"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluewithoriginalsign(Math.Abs(Convert.ToDecimal(row["ASNEXCAMOUNTDR"].ToString()))) + "</td>";
                            asnexc = asnexc - Convert.ToDecimal(row["ASNEXCAMOUNTDR"]);
                        }
                        else
                        {
                            strHtml += "<td> &nbsp;</td>";
                        }

                        if (row["ASNEXCAMOUNTCR"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(row["ASNEXCAMOUNTCR"].ToString())) + "</td>";
                            asnexc = asnexc + Convert.ToDecimal(row["ASNEXCAMOUNTCR"]);

                        }
                        else
                        {
                            strHtml += "<td> &nbsp;</td>";
                        }
                        strHtml += "</tr>";

                    }
                    //////////////////////////////////Net Options Final Settlement DISPLAY/////////////////////////////////////////////
                    strHtml += "<tr style=\"height:1px;background-color:White;\"><td colspan=11>&nbsp;</td></tr><tr style=\"background-color:White;\">";
                    strHtml += "<td align=right colspan=11><div style=\"border: solid 1px balck;width:50%;\">";
                    strHtml += "<table width=\"100%\" border=" + Convert.ToChar(34) + 0 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                    strHtml += "<tr><td align=left style=\"width:35%;\">Net Options Final Settlement :</td>";
                    if (asnexc < 0)
                    {
                        strHtml += "<td align=\"right\" style=\"width:11%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(asnexc))) + "</b></td>";
                        strHtml += "<td style=\"width:4%;\"> &nbsp;</td>";
                        strHtml += "</tr>";
                    }
                    else
                    {
                        strHtml += "<td style=\"width:5%;\"> &nbsp;</td>";
                        strHtml += "<td align=\"right\" style=\"width:15%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(asnexc))) + "</b></td>";
                        strHtml += "</tr>";
                    }

                    strHtml += "</div></table></td></tr><tr style=\"height:1px;background-color:White;\"><td colspan=11>&nbsp;</td></tr>";
                    //////////////////////////////////////END//////////////////////////////////////////////////////

                }

                /////////////////////////////FOR ASN,EXC END//////////////////// 
                ////////////////////////////ALL DETAILS SUMMARY//////////////////////////
                decimal totalobligation = decimal.Zero;
                decimal totalcharges = decimal.Zero;
                totalobligation = totalmtmexp + prm + asnexc;

                strHtml += "<tr style=\"height:1px;background-color:White;\"><td colspan=11>&nbsp;</td></tr><tr style=\"background-color:White;\">";
                strHtml += "<td align=right colspan=11><div style=\"border: solid 1px balck;width:35%;\">";
                strHtml += "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                strHtml += "<tr><td align=left style=\"width:15%;\">Total Obligation For the Day :</td>";
                if (totalobligation < 0)
                {
                    strHtml += "<td align=\"right\" style=\"width:13%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(totalobligation))) + "</b></td>";
                    strHtml += "<td style=\"width:7%;\"> &nbsp;</td>";
                    strHtml += "</tr>";
                }
                else
                {
                    strHtml += "<td style=\"width:10%;\"> &nbsp;</td>";
                    strHtml += "<td align=\"right\" style=\"width:10%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(totalobligation))) + "</b></td>";
                    strHtml += "</tr>";
                }
                if (ds.Tables[5].Rows.Count > 0)
                {
                    if (ds.Tables[5].Rows[0]["Servicetaxonbrkg"] != DBNull.Value)
                    {
                        strHtml += "<tr><td align=left style=\"width:30%;\">Serv Tax & Cess on Brokerage :</td><td align=\"right\" style=\"width:13%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(ds.Tables[5].Rows[0]["Servicetaxonbrkg"].ToString()))) + "</td><td style=\"width:7%;\"> &nbsp;</td></tr>";
                        totalcharges = totalcharges + Convert.ToDecimal(ds.Tables[5].Rows[0]["Servicetaxonbrkg"]);
                    }
                }
                if (ds.Tables[6].Rows.Count > 0)
                {
                    if (ds.Tables[6].Rows[0]["Temp_TranCharge"] != DBNull.Value)
                    {
                        strHtml += "<tr><td align=left style=\"width:30%;\">Transaction Charges :</td><td align=\"right\" style=\"width:13%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(ds.Tables[6].Rows[0]["Temp_TranCharge"].ToString()))) + "</td><td style=\"width:7%;\"> &nbsp;</td></tr>";
                        totalcharges = totalcharges + Convert.ToDecimal(ds.Tables[6].Rows[0]["Temp_TranCharge"]);

                    }
                    if (ds.Tables[6].Rows[0]["charges"] != DBNull.Value)
                    {
                        strHtml += "<tr><td align=left style=\"width:30%;\">Serv Tax & Cess on Tran Charge:</td><td align=\"right\" style=\"width:13%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(ds.Tables[6].Rows[0]["charges"].ToString()))) + "</td><td style=\"width:7%;\"> &nbsp;</td></tr>";
                        totalcharges = totalcharges + Convert.ToDecimal(ds.Tables[6].Rows[0]["charges"]);

                    }


                    if (ds.Tables[6].Rows[0]["Temp_TotalStamduty"] != DBNull.Value)
                    {
                        strHtml += "<tr><td align=left style=\"width:30%;\">Stamp Duty:</td><td align=\"right\" style=\"width:13%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(ds.Tables[6].Rows[0]["Temp_TotalStamduty"].ToString()))) + "</td><td style=\"width:7%;\"> &nbsp;</td></tr>";
                        totalcharges = totalcharges + Convert.ToDecimal(ds.Tables[6].Rows[0]["Temp_TotalStamduty"]);

                    }


                    if (ds.Tables[6].Rows[0]["exchstttax"] != DBNull.Value)
                    {
                        strHtml += "<tr><td align=left style=\"width:30%;\">STT Tax :</td><td align=\"right\" style=\"width:13%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(ds.Tables[6].Rows[0]["exchstttax"].ToString()))) + "</td><td style=\"width:7%;\"> &nbsp;</td></tr>";
                        totalcharges = totalcharges + Convert.ToDecimal(ds.Tables[6].Rows[0]["exchstttax"]);
                    }

                }
                strHtml += "<tr><td align=left style=\"width:30%;\">Total Charges :</td><td align=\"right\" style=\"width:13%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(totalcharges))) + "</td><td style=\"width:7%;\"> &nbsp;</td></tr>";
                if ((totalobligation - totalcharges) < 0)
                {
                    strHtml += "<tr><td align=left style=\"width:30%;\"><u>Net Bill Amount :</u></td><td align=\"right\" style=\"width:13%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(totalobligation - totalcharges))) + "</td><td style=\"width:7%;\"> &nbsp;</td></tr>";

                }
                else
                {
                    strHtml += "<tr><td align=left style=\"width:30%;\"><u>Net Bill Amount :</u></td><td style=\"width:10%;\"> &nbsp;</td><td align=\"right\" style=\"width:10%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(totalobligation - totalcharges))) + "</td></tr>";

                }

                DataTable dtfund = new DataTable();
                dtfund = oDBEngine.NetFundAdjustment("'SYSTM00001'", CID.Value, Session["usersegid"].ToString(), Session["LastCompany"].ToString(), Convert.ToDateTime(dtfrom.Value), Convert.ToDateTime(dtto.Value));

                if (dtfund.Rows.Count > 0)
                {
                    netfund = Convert.ToDecimal(dtfund.Rows[0][0].ToString());
                    if (netfund < 0)
                    {
                        strHtml += "<tr><td align=left style=\"width:30%;\">Net Fund Adjustment For the Period :</td><td align=\"right\" style=\"width:13%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(netfund))) + "</td><td style=\"width:7%;\"> &nbsp;</td></tr>";

                    }
                    else
                    {
                        strHtml += "<tr><td align=left style=\"width:30%;\">Net Fund Adjustment For the Period :</td><td style=\"width:10%;\"> &nbsp;</td><td align=\"right\" style=\"width:10%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(netfund))) + "</td></tr>";
                    }
                }
                netamount = totalobligation - totalcharges + (openingbalance + netfund);

                if (netamount < 0)
                {
                    strHtml += "<tr><td align=left style=\"width:30%;\">Net Amount Receivable By Us: </td><td align=\"right\" style=\"width:13%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(netamount))) + "</td><td style=\"width:7%;\"> &nbsp;</td></tr>";

                }
                else
                {
                    strHtml += "<tr><td align=left style=\"width:30%;\">Net Amount Payable To You :</td><td style=\"width:10%;\"> &nbsp;</td><td align=\"right\" style=\"width:10%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(netamount))) + "</td></tr>";
                }
                strHtml += "</div></table></td></tr><tr style=\"height:1px;background-color:White;\"><td colspan=11>&nbsp;</td></tr>";
                ///////////////////////////////END///////////////////////////////////////
                ////////////////////////////////Margin obligation///////////////////////
                if (ds.Tables[7].Rows.Count != 0)
                {

                    strHtml += "<tr style=\"background-color: #DBEEF3;\">";
                    strHtml += "<td align=\"center\" ><b>Margin Summary</b></td>";
                    strHtml += "<td align=\"center\" ><b>Span Margin</b></td>";
                    strHtml += "<td align=\"center\"><b>Premium Margin</b></td>";
                    strHtml += "<td align=\"center\"><b>Total Margin</b></td>";
                    strHtml += "<td align=\"center\"><b>Exposure Margin</b></td>";
                    strHtml += "<td align=\"center\"><b>Applicable Margin</b></td>";
                    strHtml += "<td align=\"center\"><b>Cash Dep</b></td>";
                    strHtml += "<td align=\"center\"><b><b>Collaterals</b></td>";
                    strHtml += "<td align=\"center\"><b>Total Deposit</b></td>";
                    strHtml += "<td align=\"center\"><b>Shortage</b></td>";
                    strHtml += "<td align=\"center\"><b>Excess</b></td></tr>";

                    strHtml += "<tr style=\"background-color: white;\"><td> &nbsp;</td>";
                    strHtml += "<td  align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[7].Rows[0]["SpanMargin"])) + "</td>";
                    strHtml += "<td  align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[7].Rows[0]["BuyPremium"])) + "</td>";
                    strHtml += "<td  align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[7].Rows[0]["TotalMargin"])) + "</td>";
                    strHtml += "<td  align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[7].Rows[0]["ExposureMargin"])) + "</td>";
                    strHtml += "<td  align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[7].Rows[0]["ApplicableMargin"])) + "</td>";

                    DataTable dtmargin = new DataTable();
                    dtmargin = oDBEngine.OpeningBalanceOnlyJournal("'SYSTM00002','SYSTM00003'", CID.Value, Convert.ToDateTime(dtfrom.Value), Session["usersegid"].ToString(), Session["LastCompany"].ToString(), Convert.ToDateTime(dtto.Value));
                    if (dtmargin.Rows.Count > 0)
                    {
                        strHtml += "<td  align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtmargin.Rows[0][0].ToString())) + "</td>";
                        totaldeposit = Convert.ToDecimal(dtmargin.Rows[0][0].ToString()) + Convert.ToDecimal(ds.Tables[7].Rows[0]["coleteral"]);

                    }
                    else
                    {
                        strHtml += "<td> &nbsp;</td>";
                    }
                    strHtml += "<td  align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[7].Rows[0]["coleteral"])) + "</td>";
                    strHtml += "<td  align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(totaldeposit)) + "</td>";

                    stortageexcess = totaldeposit - Convert.ToDecimal(ds.Tables[7].Rows[0]["ApplicableMargin"]);
                    if (stortageexcess < 0)
                    {
                        strHtml += "<td  align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(Math.Abs(stortageexcess))) + "</td>";
                        strHtml += "<td align=\"right\"></td></tr>";
                    }
                    else
                    {
                        strHtml += "<td  align=\"right\"></td>";
                        strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(stortageexcess)) + "</td></tr>";
                    }

                }

                ////////////////////////////////END////////////////////////////////////
                ////////////////////////////////////Net Amount After Adjusting Margin Shortage :///////////////////////////

                strHtml += "<tr><td style=\"height:10px;background-color:White;\" colspan=11></td></tr><tr style=\"background-color:White;\">";
                strHtml += "<td align=right colspan=11><div style=\"border: solid 1px balck;width:50%;\">";
                strHtml += "<table width=\"100%\" border=" + Convert.ToChar(34) + 0 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr>";

                if (stortageexcess < 0)
                {
                    if (netamount + stortageexcess > 0)
                    {
                        strHtml += "<td align=\"left\" style=\"width:35%;\">Net Amount Payable  After Adjusting Margin Shortage :</td>";
                        strHtml += "<td align=\"right\" style=\"width:15%;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs((netamount + stortageexcess))) + "</b></td>";

                    }
                    else
                    {
                        strHtml += "<td align=\"left\" style=\"width:35%;\">Net Amount Receivable By Us After Adjusting Margin Shortage :</td>";
                        strHtml += "<td align=\"right\" style=\"width:15%;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs((netamount + stortageexcess))) + "</b></td>";

                    }
                }
                else
                {
                    if (netamount > 0)
                    {
                        strHtml += "<td align=\"left\" style=\"width:35%;\">Net Amount Payable To You After Adjusting Margin Shortage :</td>";
                        strHtml += "<td style=\"width:10%;\"></td><td align=\"right\" style=\"width:10%;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs((netamount))) + "</b></td>";

                    }
                    else
                    {
                        strHtml += "<td align=\"left\" style=\"width:35%;\">Net Amount Receivable BY Us After Adjusting Margin Shortage :</td>";
                        strHtml += "<td align=\"right\" style=\"width:15%;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs((netamount))) + "</b></td>";

                    }
                }
                strHtml += "</tr></table></div></td></tr>";


                strHtml += "</table>";

                display.InnerHtml = strHtml;

                ds.Dispose();
                /////////////////////////////DISPLAY DATE/////////////////////////////////////
                string group = null;
                if (ddlGroup.SelectedItem.Value.ToString() == "0")
                {
                    group = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise Report";
                }
                else
                {
                    group = ddlGroup.SelectedItem.Text + " Wise [ " + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "] Report";
                }
                if (ddldate.SelectedItem.Value == "1")
                {
                    string SpanText = group + " For The Period-" + oconverter.ArrangeDate2(dtfrom.Value.ToString()) + " " + "to" + " " + oconverter.ArrangeDate2(dtto.Value.ToString());
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct", "displaydate('" + SpanText + "')", true);
                }

                ///////////////////////////////END//////////////////////////////////////////
                ScriptManager.RegisterStartupScript(this, this.GetType(), "displaygrid", "displayresult();", true);


            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "norecord", "norecord(1);", true);

            }

        }
        void bind_Print()
        {
            // using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            // {
            string client = null;
            string fromdate = string.Empty;
            string todate = string.Empty;
            dtgroup = (DataTable)ViewState["groupclients"];
            for (int i = 0; i < dtgroup.Rows.Count; i++)
            {
                if (client == null)
                    client = "'" + dtgroup.Rows[i]["cnt_internalid"].ToString() + "'";
                else
                    client = client + "," + "'" + dtgroup.Rows[i]["cnt_internalid"].ToString() + "'";
            }

            if (ddldate.SelectedItem.Value == "0")
            {
                fromdate = Convert.ToString(dtfor.Value);
                todate = "NA";
            }
            else
            {
                fromdate = Convert.ToString(dtfrom.Value);
                todate = Convert.ToString(dtto.Value);
            }

            ds = objFAReportsOther.Sp_ObligationStatementFO_CRYSTAL(
          Convert.ToString(fromdate),
          Convert.ToString(todate),
          Convert.ToString(Session["usersegid"].ToString()),
           Convert.ToString(Session["LastCompany"]),
           Convert.ToString(HttpContext.Current.Session["ExchangeSegmentID"]),
            Convert.ToString(client),
              Convert.ToString(HttpContext.Current.Session["LastFinYear"])
           );


            ///////////////////////Column Add
            ds.Tables[0].Columns.Add("openingbalanceDr", Type.GetType("System.Decimal"));
            ds.Tables[0].Columns.Add("openingbalanceCr", Type.GetType("System.Decimal"));
            ds.Tables[0].Columns.Add("fundDr", Type.GetType("System.Decimal"));
            ds.Tables[0].Columns.Add("fundCr", Type.GetType("System.Decimal"));
            ds.Tables[0].Columns.Add("resultDr", Type.GetType("System.Decimal"));
            ds.Tables[0].Columns.Add("resultCr", Type.GetType("System.Decimal"));
            ds.Tables[0].Columns.Add("resulttxtDr", Type.GetType("System.String"));
            ds.Tables[0].Columns.Add("resulttxtCr", Type.GetType("System.String"));
            ds.Tables[0].Columns.Add("cashdep", Type.GetType("System.String"));
            ds.Tables[0].Columns.Add("totaldeposit", Type.GetType("System.String"));
            ds.Tables[0].Columns.Add("shortage", Type.GetType("System.String"));
            ds.Tables[0].Columns.Add("excess", Type.GetType("System.String"));
            ds.Tables[0].Columns.Add("adjustshortageDr", Type.GetType("System.String"));
            ds.Tables[0].Columns.Add("adjustshortageCr", Type.GetType("System.String"));
            ds.Tables[0].Columns.Add("adjustshortagetxtDr", Type.GetType("System.String"));
            ds.Tables[0].Columns.Add("adjustshortagetxtCr", Type.GetType("System.String"));

            decimal openingbalance = decimal.Zero;
            decimal fund = decimal.Zero;
            decimal margin = decimal.Zero;
            decimal result = decimal.Zero;
            decimal totaldeposit = decimal.Zero;
            decimal shortageexcess = decimal.Zero;
            /////////////////////////////////FUNCTION AND CALCULATION ///////////////////
            //string[] clientsidnew = client.Split(',');
            int varj = 0;
            //for (int k = 0; k < clientsidnew.Length; k++)
            //{
            //    client = clientsidnew[k].ToString().Replace("'", "");

            for (int j = varj; j < ds.Tables[0].Rows.Count; j++)
            {
                //if (ds.Tables[0].Rows[j]["clientsid"].ToString() == client)
                //{
                client = ds.Tables[0].Rows[j]["clientsid"].ToString();
                DataTable dtOpen = new DataTable();
                if (ddldate.SelectedItem.Value == "0")
                {
                    dtOpen = oDBEngine.OpeningBalanceOnlyJournal("'SYSTM00001'", client, Convert.ToDateTime(dtfor.Value), Session["usersegid"].ToString(), Session["LastCompany"].ToString(), Convert.ToDateTime(dtfor.Value));
                }
                else
                {
                    dtOpen = oDBEngine.OpeningBalanceOnlyJournal("'SYSTM00001'", client, Convert.ToDateTime(dtfrom.Value), Session["usersegid"].ToString(), Session["LastCompany"].ToString(), Convert.ToDateTime(dtto.Value));
                }
                openingbalance = Convert.ToDecimal(dtOpen.Rows[0][1].ToString());
                if (openingbalance < 0)
                {
                    ds.Tables[0].Rows[j]["openingbalanceDr"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(openingbalance)));
                }
                else
                {
                    ds.Tables[0].Rows[j]["openingbalanceCr"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(openingbalance));
                }
                dtOpen.Clear();
                dtOpen.Dispose();

                DataTable dtfund = new DataTable();
                if (ddldate.SelectedItem.Value == "0")
                {
                    dtfund = oDBEngine.NetFundAdjustment("'SYSTM00001'", client, Session["usersegid"].ToString(), Session["LastCompany"].ToString(), Convert.ToDateTime(dtfor.Value));
                }
                else
                {
                    dtfund = oDBEngine.NetFundAdjustment("'SYSTM00001'", client, Session["usersegid"].ToString(), Session["LastCompany"].ToString(), Convert.ToDateTime(dtfrom.Value), Convert.ToDateTime(dtto.Value));
                }
                if (dtfund.Rows.Count > 0)
                {
                    fund = Convert.ToDecimal(dtfund.Rows[0][0].ToString());
                    if (fund < 0)
                    {
                        ds.Tables[0].Rows[j]["fundDr"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(fund)));

                    }
                    else
                    {
                        ds.Tables[0].Rows[j]["fundCr"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(fund));
                    }
                }
                dtfund.Clear();
                dtfund.Dispose();

                result = Convert.ToDecimal(ds.Tables[0].Rows[j]["netbill"]) + Convert.ToDecimal(openingbalance) + Convert.ToDecimal(fund);
                if (result < 0)
                {
                    ds.Tables[0].Rows[j]["resultDr"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(result)));
                    ds.Tables[0].Rows[j]["resulttxtDr"] = "Net Amount Receivable By Us:";

                }
                else
                {
                    ds.Tables[0].Rows[j]["resultCr"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(result));
                    ds.Tables[0].Rows[j]["resulttxtCr"] = "Net Amount Payable To You :";

                }
                DataTable dtmargin = new DataTable();
                if (ddldate.SelectedItem.Value == "0")
                {
                    dtmargin = oDBEngine.OpeningBalanceOnlyJournal("'SYSTM00002','SYSTM00003'", client, Convert.ToDateTime(dtfor.Value), Session["usersegid"].ToString(), Session["LastCompany"].ToString(), Convert.ToDateTime(dtfor.Value));
                }
                else
                {
                    dtmargin = oDBEngine.OpeningBalanceOnlyJournal("'SYSTM00002','SYSTM00003'", client, Convert.ToDateTime(dtfrom.Value), Session["usersegid"].ToString(), Session["LastCompany"].ToString(), Convert.ToDateTime(dtto.Value));
                }
                if (dtmargin.Rows.Count > 0)
                {
                    ds.Tables[0].Rows[j]["cashdep"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtmargin.Rows[0][0].ToString()));
                    if (ds.Tables[0].Rows[j]["coleteral"] != DBNull.Value)
                    {
                        totaldeposit = Convert.ToDecimal(dtmargin.Rows[0][0].ToString()) + Convert.ToDecimal(ds.Tables[0].Rows[j]["coleteral"]);

                    }
                    else
                    {
                        totaldeposit = Convert.ToDecimal(dtmargin.Rows[0][0].ToString()) + Convert.ToDecimal(0.0);
                    }

                }
                dtmargin.Clear();
                dtmargin.Dispose();

                ds.Tables[0].Rows[j]["totaldeposit"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(totaldeposit));
                if (ds.Tables[0].Rows[j]["ApplicableMargin"] != DBNull.Value)
                {
                    shortageexcess = totaldeposit - Convert.ToDecimal(ds.Tables[0].Rows[j]["ApplicableMargin"]);
                }
                else
                {
                    shortageexcess = Convert.ToDecimal(totaldeposit) - Convert.ToDecimal(0.0);

                }
                if (shortageexcess < 0)
                {
                    ds.Tables[0].Rows[j]["shortage"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(shortageexcess)));
                }
                else
                {
                    ds.Tables[0].Rows[j]["excess"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(shortageexcess));
                }
                if (shortageexcess < 0)
                {
                    if (result + shortageexcess > 0)
                    {
                        ds.Tables[0].Rows[j]["adjustshortageCr"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(result + shortageexcess)));
                        ds.Tables[0].Rows[j]["adjustshortagetxtCr"] = "Net Amount Payable  After Adjusting Margin Shortage :";
                    }
                    else
                    {
                        ds.Tables[0].Rows[j]["adjustshortageDr"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(result + shortageexcess)));
                        ds.Tables[0].Rows[j]["adjustshortagetxtDr"] = "Net Amount Receivable By Us After Adjusting Margin Shortage :";

                    }


                }
                else
                {
                    if (result > 0)
                    {
                        ds.Tables[0].Rows[j]["adjustshortageCr"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(result)));
                        ds.Tables[0].Rows[j]["adjustshortagetxtCr"] = "Net Amount Payable  After Adjusting Margin Shortage :";

                    }
                    else
                    {
                        ds.Tables[0].Rows[j]["adjustshortageDr"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(result)));
                        ds.Tables[0].Rows[j]["adjustshortagetxtDr"] = "Net Amount Receivable By Us After Adjusting Margin Shortage :";

                    }

                }

                varj = j + 1;

                //}
                //else
                //{
                //    break;
                //}


            }

            //}
            ////////////////////////////////////////END/////////////////////////////////
            byte[] logoinByte;
            ds.Tables[0].Columns.Add("Image", System.Type.GetType("System.Byte[]"));
            if (oconverter.getLogoImage(HttpContext.Current.Server.MapPath(@"..\images\logo.jpg"), out logoinByte) != 1)
            {
                ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "logo", "alert('Logo not Found.');", true);

            }
            else
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ds.Tables[0].Rows[i]["Image"] = logoinByte;
                    if (ds.Tables[0].Rows[i]["BFQuantityDr"] != DBNull.Value)
                        ds.Tables[0].Rows[i]["BFQuantityDr"] = oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(Convert.ToString(ds.Tables[0].Rows[i]["BFQuantityDr"])));

                    if (ds.Tables[0].Rows[i]["BFQuantityCr"] != DBNull.Value)
                        ds.Tables[0].Rows[i]["BFQuantityCr"] = oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(Convert.ToString(ds.Tables[0].Rows[i]["BFQuantityCr"])));

                    if (ds.Tables[0].Rows[i]["CFQuantityDr"] != DBNull.Value)
                        ds.Tables[0].Rows[i]["CFQuantityDr"] = oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(Convert.ToString(ds.Tables[0].Rows[i]["CFQuantityDr"])));

                    if (ds.Tables[0].Rows[i]["CFQuantityCr"] != DBNull.Value)
                        ds.Tables[0].Rows[i]["CFQuantityCr"] = oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(Convert.ToString(ds.Tables[0].Rows[i]["CFQuantityCr"])));

                    if (ds.Tables[0].Rows[i]["BFValueDr"] != DBNull.Value)
                        ds.Tables[0].Rows[i]["BFValueDr"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(Convert.ToString(ds.Tables[0].Rows[i]["BFValueDr"])));

                    if (ds.Tables[0].Rows[i]["BFValueCr"] != DBNull.Value)
                        ds.Tables[0].Rows[i]["BFValueCr"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(Convert.ToString(ds.Tables[0].Rows[i]["BFValueCr"])));

                    if (ds.Tables[0].Rows[i]["CFValueDr"] != DBNull.Value)
                        ds.Tables[0].Rows[i]["CFValueDr"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(Convert.ToString(ds.Tables[0].Rows[i]["CFValueDr"])));

                    if (ds.Tables[0].Rows[i]["CFValueCr"] != DBNull.Value)
                        ds.Tables[0].Rows[i]["CFValueCr"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(Convert.ToString(ds.Tables[0].Rows[i]["CFValueCr"])));

                    if (ds.Tables[0].Rows[i]["OpenPrice"] != DBNull.Value)
                        ds.Tables[0].Rows[i]["OpenPrice"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(Convert.ToString(ds.Tables[0].Rows[i]["OpenPrice"])));

                    if (ds.Tables[0].Rows[i]["SettPrice"] != DBNull.Value)
                        ds.Tables[0].Rows[i]["SettPrice"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(Convert.ToString(ds.Tables[0].Rows[i]["SettPrice"])));

                    if (ds.Tables[0].Rows[i]["MTMBUYQUANTITY"] != DBNull.Value)
                        ds.Tables[0].Rows[i]["MTMBUYQUANTITY"] = oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(Convert.ToString(ds.Tables[0].Rows[i]["MTMBUYQUANTITY"])));

                    if (ds.Tables[0].Rows[i]["MTMSELLQUANTITY"] != DBNull.Value)
                        ds.Tables[0].Rows[i]["MTMSELLQUANTITY"] = oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(Convert.ToString(ds.Tables[0].Rows[i]["MTMSELLQUANTITY"])));

                    if (ds.Tables[0].Rows[i]["MktRate"] != DBNull.Value)
                        ds.Tables[0].Rows[i]["MktRate"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(Convert.ToString(ds.Tables[0].Rows[i]["MktRate"])));

                    if (ds.Tables[0].Rows[i]["MTMBrkg"] != DBNull.Value)
                        ds.Tables[0].Rows[i]["MTMBrkg"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(Convert.ToString(ds.Tables[0].Rows[i]["MTMBrkg"])));

                    if (ds.Tables[0].Rows[i]["MTMNetRatePerUnit"] != DBNull.Value)
                        ds.Tables[0].Rows[i]["MTMNetRatePerUnit"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(Convert.ToString(ds.Tables[0].Rows[i]["MTMNetRatePerUnit"])));

                    if (ds.Tables[0].Rows[i]["MTMAMOUNTDR"] != DBNull.Value)
                        ds.Tables[0].Rows[i]["MTMAMOUNTDR"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(Convert.ToString(ds.Tables[0].Rows[i]["MTMAMOUNTDR"])));

                    if (ds.Tables[0].Rows[i]["MTMAMOUNTCR"] != DBNull.Value)
                        ds.Tables[0].Rows[i]["MTMAMOUNTCR"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(Convert.ToString(ds.Tables[0].Rows[i]["MTMAMOUNTCR"])));

                    if (ds.Tables[0].Rows[i]["StrikePrice"] != DBNull.Value)
                        ds.Tables[0].Rows[i]["StrikePrice"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(Convert.ToString(ds.Tables[0].Rows[i]["StrikePrice"])));

                    if (ds.Tables[0].Rows[i]["summtmDr"] != DBNull.Value)
                        ds.Tables[0].Rows[i]["summtmDr"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(Convert.ToString(ds.Tables[0].Rows[i]["summtmDr"])));

                    if (ds.Tables[0].Rows[i]["summtmCr"] != DBNull.Value)
                        ds.Tables[0].Rows[i]["summtmCr"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(Convert.ToString(ds.Tables[0].Rows[i]["summtmCr"])));

                    if (ds.Tables[0].Rows[i]["summtmfinsetDr"] != DBNull.Value)
                        ds.Tables[0].Rows[i]["summtmfinsetDr"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(Convert.ToString(ds.Tables[0].Rows[i]["summtmfinsetDr"])));

                    if (ds.Tables[0].Rows[i]["summtmfinsetCr"] != DBNull.Value)
                        ds.Tables[0].Rows[i]["summtmfinsetCr"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(Convert.ToString(ds.Tables[0].Rows[i]["summtmfinsetCr"])));

                    if (ds.Tables[0].Rows[i]["summtmmtmsetDr"] != DBNull.Value)
                        ds.Tables[0].Rows[i]["summtmmtmsetDr"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(Convert.ToString(ds.Tables[0].Rows[i]["summtmmtmsetDr"])));

                    if (ds.Tables[0].Rows[i]["summtmmtmsetCr"] != DBNull.Value)
                        ds.Tables[0].Rows[i]["summtmmtmsetCr"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(Convert.ToString(ds.Tables[0].Rows[i]["summtmmtmsetCr"])));

                    if (ds.Tables[0].Rows[i]["summtmallDr"] != DBNull.Value)
                        ds.Tables[0].Rows[i]["summtmallDr"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(Convert.ToString(ds.Tables[0].Rows[i]["summtmallDr"])));

                    if (ds.Tables[0].Rows[i]["summtmallCr"] != DBNull.Value)
                        ds.Tables[0].Rows[i]["summtmallCr"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(Convert.ToString(ds.Tables[0].Rows[i]["summtmallCr"])));

                    /////////////////////////////////Margin/////////////////////////////////////
                    if (ds.Tables[0].Rows[i]["SpanMargin"] != DBNull.Value)
                        ds.Tables[0].Rows[i]["SpanMargin"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["SpanMargin"].ToString()));

                    if (ds.Tables[0].Rows[i]["BuyPremium"] != DBNull.Value)
                        ds.Tables[0].Rows[i]["BuyPremium"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["BuyPremium"].ToString()));

                    if (ds.Tables[0].Rows[i]["ExposureMargin"] != DBNull.Value)
                        ds.Tables[0].Rows[i]["ExposureMargin"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["ExposureMargin"].ToString()));

                    if (ds.Tables[0].Rows[i]["TotalMargin"] != DBNull.Value)
                        ds.Tables[0].Rows[i]["TotalMargin"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["TotalMargin"].ToString()));

                    if (ds.Tables[0].Rows[i]["ApplicableMargin"] != DBNull.Value)
                        ds.Tables[0].Rows[i]["ApplicableMargin"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["ApplicableMargin"].ToString()));

                    if (ds.Tables[0].Rows[i]["coleteral"] != DBNull.Value)
                        ds.Tables[0].Rows[i]["coleteral"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["coleteral"].ToString()));

                    /////////////////////////////////FOR Charges//////////////////////////////////////
                    if (ds.Tables[0].Rows[i]["Servicetaxonbrkg"] != DBNull.Value)
                        ds.Tables[0].Rows[i]["Servicetaxonbrkg"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["Servicetaxonbrkg"].ToString()));

                    if (ds.Tables[0].Rows[i]["trancharge"] != DBNull.Value)
                        ds.Tables[0].Rows[i]["trancharge"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["trancharge"].ToString()));

                    if (ds.Tables[0].Rows[i]["STaxOnTrnCharges"] != DBNull.Value)
                        ds.Tables[0].Rows[i]["STaxOnTrnCharges"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["STaxOnTrnCharges"].ToString()));

                    if (ds.Tables[0].Rows[i]["Stamp"] != DBNull.Value)
                        ds.Tables[0].Rows[i]["Stamp"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["Stamp"].ToString()));

                    if (ds.Tables[0].Rows[i]["Sttax"] != DBNull.Value)
                        ds.Tables[0].Rows[i]["Sttax"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["Sttax"].ToString()));

                    if (ds.Tables[0].Rows[i]["TotalObligationDr"] != DBNull.Value)
                        ds.Tables[0].Rows[i]["TotalObligationDr"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["TotalObligationDr"].ToString()));

                    if (ds.Tables[0].Rows[i]["TotalObligationCr"] != DBNull.Value)
                        ds.Tables[0].Rows[i]["TotalObligationCr"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["TotalObligationCr"].ToString()));

                    if (ds.Tables[0].Rows[i]["totalcharges"] != DBNull.Value)
                        ds.Tables[0].Rows[i]["totalcharges"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["totalcharges"].ToString()));

                    if (ds.Tables[0].Rows[i]["netbillDr"] != DBNull.Value)
                        ds.Tables[0].Rows[i]["netbillDr"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["netbillDr"].ToString()));

                    if (ds.Tables[0].Rows[i]["netbillCr"] != DBNull.Value)
                        ds.Tables[0].Rows[i]["netbillCr"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["netbillCr"].ToString()));
                    /////////////////////////////////////END////////////////////////////////////////////////////
                }

                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)///////////////PRM
                {
                    if (ds.Tables[1].Rows[i]["PRMBUYQUANTITY"] != DBNull.Value)
                        ds.Tables[1].Rows[i]["PRMBUYQUANTITY"] = oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(Convert.ToString(ds.Tables[1].Rows[i]["PRMBUYQUANTITY"])));

                    if (ds.Tables[1].Rows[i]["PRMSELLQUANTITY"] != DBNull.Value)
                        ds.Tables[1].Rows[i]["PRMSELLQUANTITY"] = oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(Convert.ToString(ds.Tables[1].Rows[i]["PRMSELLQUANTITY"])));

                    if (ds.Tables[1].Rows[i]["StrikePrice"] != DBNull.Value)
                        ds.Tables[1].Rows[i]["StrikePrice"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(Convert.ToString(ds.Tables[1].Rows[i]["StrikePrice"])));

                    if (ds.Tables[1].Rows[i]["PRMBrkg"] != DBNull.Value)
                        ds.Tables[1].Rows[i]["PRMBrkg"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(Convert.ToString(ds.Tables[1].Rows[i]["PRMBrkg"])));

                    if (ds.Tables[1].Rows[i]["PRMNetRatePerUnit"] != DBNull.Value)
                        ds.Tables[1].Rows[i]["PRMNetRatePerUnit"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(Convert.ToString(ds.Tables[1].Rows[i]["PRMNetRatePerUnit"])));

                    if (ds.Tables[1].Rows[i]["PRMAMOUNTDR"] != DBNull.Value)
                        ds.Tables[1].Rows[i]["PRMAMOUNTDR"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(Convert.ToString(ds.Tables[1].Rows[i]["PRMAMOUNTDR"])));

                    if (ds.Tables[1].Rows[i]["PRMAMOUNTCR"] != DBNull.Value)
                        ds.Tables[1].Rows[i]["PRMAMOUNTCR"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(Convert.ToString(ds.Tables[1].Rows[i]["PRMAMOUNTCR"])));

                    if (ds.Tables[1].Rows[i]["productprmDr"] != DBNull.Value)
                        ds.Tables[1].Rows[i]["productprmDr"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(Convert.ToString(ds.Tables[1].Rows[i]["productprmDr"])));

                    if (ds.Tables[1].Rows[i]["productprmCr"] != DBNull.Value)
                        ds.Tables[1].Rows[i]["productprmCr"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(Convert.ToString(ds.Tables[1].Rows[i]["productprmCr"])));

                    if (ds.Tables[1].Rows[i]["netprmDr"] != DBNull.Value)
                        ds.Tables[1].Rows[i]["netprmDr"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(Convert.ToString(ds.Tables[1].Rows[i]["netprmDr"])));

                    if (ds.Tables[1].Rows[i]["netprmCr"] != DBNull.Value)
                        ds.Tables[1].Rows[i]["netprmCr"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(Convert.ToString(ds.Tables[1].Rows[i]["netprmCr"])));

                }

                for (int i = 0; i < ds.Tables[2].Rows.Count; i++)///////////////ASN EXC
                {
                    if (ds.Tables[2].Rows[i]["ASNEXCQUANTITY"] != DBNull.Value)
                        ds.Tables[2].Rows[i]["ASNEXCQUANTITY"] = oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(Convert.ToString(ds.Tables[2].Rows[i]["ASNEXCQUANTITY"])));

                    if (ds.Tables[2].Rows[i]["ASNEXCSettPrice"] != DBNull.Value)
                        ds.Tables[2].Rows[i]["ASNEXCSettPrice"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(Convert.ToString(ds.Tables[2].Rows[i]["ASNEXCSettPrice"])));

                    if (ds.Tables[2].Rows[i]["ASNEXCUnitPrice"] != DBNull.Value)
                        ds.Tables[2].Rows[i]["ASNEXCUnitPrice"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(Convert.ToString(ds.Tables[2].Rows[i]["ASNEXCUnitPrice"])));

                    if (ds.Tables[2].Rows[i]["ASNEXCStrikePrice"] != DBNull.Value)
                        ds.Tables[2].Rows[i]["ASNEXCStrikePrice"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(Convert.ToString(ds.Tables[2].Rows[i]["ASNEXCStrikePrice"])));

                    if (ds.Tables[2].Rows[i]["ASNEXCBrkg"] != DBNull.Value)
                        ds.Tables[2].Rows[i]["ASNEXCBrkg"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(Convert.ToString(ds.Tables[2].Rows[i]["ASNEXCBrkg"])));

                    if (ds.Tables[2].Rows[i]["ASNEXCNetRatePerUnit"] != DBNull.Value)
                        ds.Tables[2].Rows[i]["ASNEXCNetRatePerUnit"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(Convert.ToString(ds.Tables[2].Rows[i]["ASNEXCNetRatePerUnit"])));

                    if (ds.Tables[2].Rows[i]["ASNEXCAMOUNTDR"] != DBNull.Value)
                        ds.Tables[2].Rows[i]["ASNEXCAMOUNTDR"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(Convert.ToString(ds.Tables[2].Rows[i]["ASNEXCAMOUNTDR"])));

                    if (ds.Tables[2].Rows[i]["ASNEXCAMOUNTCR"] != DBNull.Value)
                        ds.Tables[2].Rows[i]["ASNEXCAMOUNTCR"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(Convert.ToString(ds.Tables[2].Rows[i]["ASNEXCAMOUNTCR"])));

                    if (ds.Tables[2].Rows[i]["netasnexcDr"] != DBNull.Value)
                        ds.Tables[2].Rows[i]["netasnexcDr"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(Convert.ToString(ds.Tables[2].Rows[i]["netasnexcDr"])));

                    if (ds.Tables[2].Rows[i]["netasnexcCr"] != DBNull.Value)
                        ds.Tables[2].Rows[i]["netasnexcCr"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(Convert.ToString(ds.Tables[2].Rows[i]["netasnexcCr"])));

                }
                ReportDocument report = new ReportDocument();

                string tmpPdfPath = string.Empty;
                tmpPdfPath = HttpContext.Current.Server.MapPath("..\\management\\ObligationFoReport.rpt");
                report.Load(tmpPdfPath);
                report.SetDataSource(ds.Tables[0]);
                report.Subreports["prm"].SetDataSource(ds.Tables[1]);
                report.Subreports["asnexc"].SetDataSource(ds.Tables[2]);
                //report.Subreports["margin"].SetDataSource(ds.Tables[3]);

                report.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "Bill Printing");

                report.Dispose();
                GC.Collect();
            }



        }
        protected string GetRowColor(int i)
        {
            if (i++ % 2 == 0)
                return "#fff0f5";
            else
                return "White";
        }
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            if (rbPrint.Checked)
            {
                if (ddldate.SelectedItem.Value == "0")
                {
                    DataTable DtCount = oDBEngine.GetDataTable("Trans_DailyStatistics", "count(DailyStat_DateTime)", " cast(DATEADD(dd, 0, DATEDIFF(dd, 0,DailyStat_DateTime)) as datetime)=cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtfor.Value + "')) as datetime) and DailyStat_ExchangeSegmentID=" + HttpContext.Current.Session["ExchangeSegmentID"].ToString() + "");

                    if (DtCount.Rows.Count > 0)
                    {
                        if (DtCount.Rows[0][0].ToString() == "0")
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "norecord2", "norecord(2);", true);
                        }
                        else
                        {
                            displayFOClient();
                        }
                    }
                }
                else
                {
                    DataTable DtCount = oDBEngine.GetDataTable("(Select count(DailyStat_DateTime) as ctb1 from Trans_DailyStatistics" +
                    " WHERE  cast(DATEADD(dd, 0, DATEDIFF(dd, 0,DailyStat_DateTime)) as datetime)=cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtfrom.Value + "')) as datetime) " +
                    "and DailyStat_ExchangeSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"].ToString() + "'" +

                    " union all " +

                    "Select count(DailyStat_DateTime) as ctb2 from Trans_DailyStatistics WHERE " +
                    "cast(DATEADD(dd, 0, DATEDIFF(dd, 0,DailyStat_DateTime)) as datetime)=cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtto.Value + "')) as datetime)" +
                    "and DailyStat_ExchangeSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"].ToString() + "') as tb", "ctb1", null);


                    if (DtCount.Rows.Count > 0)
                    {
                        if (DtCount.Rows[0][0].ToString() == "0" || DtCount.Rows[1][0].ToString() == "0")
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "norecord2", "norecord(2);", true);
                        }
                        else
                        {
                            displayFOClient();
                        }
                    }
                }
            }
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
            bind_ClientDetails();
        }
        protected void ASPxPrevious_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 0)
            {
                CurrentPage = CurrentPage - 1;
                bind_ClientDetails();
            }
        }
        protected void ASPxNext_Click(object sender, EventArgs e)
        {
            if (CurrentPage < LastPage)
            {
                CurrentPage = CurrentPage + 1;
                bind_ClientDetails();
            }
        }
        protected void ASPxLast_Click(object sender, EventArgs e)
        {
            CurrentPage = LastPage;
            bind_ClientDetails();
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            DataTable dtCL = new DataTable();
            String ClName = string.Empty;
            //  String CLValue = string.Empty;
            string billdate = string.Empty;
            string emailbdy = string.Empty;
            string contactid = string.Empty;
            string AddEmlHtml = string.Empty;
            string mailid = "";
            string EmailTbl = string.Empty;
            string SpanText = string.Empty;

            string group = null;
            if (ddlGroup.SelectedItem.Value.ToString() == "0")
            {
                group = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise Report";
            }
            else
            {
                group = ddlGroup.SelectedItem.Text + "Wise [ " + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "] Report";
            }
            if (ddldate.SelectedItem.Value == "0")
            {

                SpanText = group + " For " + oconverter.ArrangeDate2(dtfor.Value.ToString());

            }
            else
            {
                SpanText = group + " Period " + oconverter.ArrangeDate2(dtfrom.Value.ToString()) + " " + "to" + " " + oconverter.ArrangeDate2(dtto.Value.ToString());


            }

            billdate = SpanText;

            string Subject = "Obligation Statement[" + billdate + "]";
            DataTable dtEml = (DataTable)ViewState["groupclients"];
            for (int i = 0; i < dtEml.Rows.Count; i++)
            {
                ClName = dtEml.Rows[i]["Name"].ToString();
                CLValue = dtEml.Rows[i]["cnt_internalid"].ToString();
                procedureEmail(CLValue);
                EmailTbl = "<table width=\"950px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"1\"><tr style=\"background-color:#A8D3FD;font-weight:bold;color:#ffffff;\"><td align=\"left\">Client Name: " + ClName + "</td></tr><tr><td> " + SEmailTbl.ToString() + "</td></tr></table>";
                emailbdy = EmailTbl.ToString();
                contactid = CLValue;
                if (rbRspctvClient.Checked)
                {
                    if (oDBEngine.SendReport(emailbdy, contactid, billdate, Subject) == true)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct10", "displaydate('" + SpanText + "')", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "jscript11", "MailsendT();", true);

                    }
                    else
                    {
                        if (dtCL.Rows.Count <= 1)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct12", "displaydate('" + SpanText + "')", true);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "jscript13", "MailsendF();", true);
                        }
                    }

                }
                else if (rbOnlyClient.Checked)
                {
                    if (i < dtEml.Rows.Count - 1)
                    {
                        if (dtEml.Rows[i]["groupid"].ToString() == dtEml.Rows[i + 1]["groupid"].ToString())
                        {
                            AddEmlHtml += EmailTbl.ToString();
                            EmailTbl = "";


                        }
                        else
                        {
                            if (ddlGroup.SelectedItem.Value.ToString() == "0")
                            {
                                DataTable dtCnt = oDBEngine.GetDataTable(" TBL_MASTER_BRANCH  ", "  top 1 *  ", "BRANCH_id='" + dtEml.Rows[i]["groupid"].ToString() + "'");
                                AddEmlHtml += EmailTbl.ToString();
                                emailbdy = "<table width=\"950px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"1\"><tr style=\"background-color:#FDD2FD;font-weight:bold;text-align:center;\"><td>Branch Name: " + dtEml.Rows[i]["groupname"].ToString() + "</td></tr><tr><td> " + AddEmlHtml + "</td></tr></table>";
                                AddEmlHtml = "";
                                EmailTbl = "";
                                if (dtCnt.Rows.Count > 0)
                                {
                                    contactid = dtCnt.Rows[0]["branch_head"].ToString().Trim();
                                    mailid = dtCnt.Rows[0]["branch_cpEmail"].ToString().Trim();
                                }
                                if (mailid.Length > 0)
                                {
                                    if (oDBEngine.SendReportBr(emailbdy, mailid, billdate, Subject, contactid) == true)
                                    {
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct14", "displaydate('" + SpanText + "')", true);
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "jscript15", "MailsendT();", true);

                                    }
                                    else
                                    {
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct16", "displaydate('" + SpanText + "')", true);
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "jscript17", "MailsendF();", true);
                                    }

                                }

                            }
                            else if (ddlGroup.SelectedItem.Value.ToString() == "1")
                            {

                                DataTable dtCnt = oDBEngine.GetDataTable(" tbl_master_groupmaster  ", "    *  ", "gpm_id='" + dtEml.Rows[i]["groupid"].ToString() + "'");
                                AddEmlHtml += EmailTbl.ToString();
                                emailbdy = "<table width=\"950px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"1\"><tr style=\"background-color:#FDD2FD;font-weight:bold;text-align:center;\"><td >Group Name: " + dtEml.Rows[i]["groupname"].ToString() + "</td></tr><tr><td> " + AddEmlHtml + "</td></tr></table>";
                                AddEmlHtml = "";
                                EmailTbl = "";
                                if (dtCnt.Rows.Count > 0)
                                {
                                    contactid = dtEml.Rows[i]["groupid"].ToString();
                                    mailid = dtCnt.Rows[0]["gpm_emailID"].ToString().Trim();
                                }
                                if (mailid.Length > 0)
                                {
                                    if (oDBEngine.SendReportBr(emailbdy, mailid, billdate, Subject, contactid) == true)
                                    {
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct18", "displaydate('" + SpanText + "')", true);
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "jscript19", "MailsendT();", true);

                                    }
                                    else
                                    {
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct20", "displaydate('" + SpanText + "')", true);
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "jscript21", "MailsendF();", true);
                                    }

                                }
                            }
                        }
                    }
                    else if (i == dtEml.Rows.Count - 1)
                    {

                        if (ddlGroup.SelectedItem.Value.ToString() == "0")
                        {
                            DataTable dtCnt = oDBEngine.GetDataTable(" TBL_MASTER_BRANCH  ", "  top 1 *  ", "BRANCH_id='" + dtEml.Rows[i]["groupid"].ToString() + "'");
                            AddEmlHtml += EmailTbl.ToString();
                            emailbdy = "<table width=\"950px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"1\"><tr style=\"background-color:#FDD2FD;font-weight:bold;text-align:center;\"><td align=\"left\">Branch Name: " + dtEml.Rows[i]["groupname"].ToString() + "</td></tr><tr><td> " + AddEmlHtml + "</td></tr></table>";
                            AddEmlHtml = "";
                            EmailTbl = "";
                            if (dtCnt.Rows.Count > 0)
                            {
                                contactid = dtCnt.Rows[0]["branch_head"].ToString().Trim();
                                mailid = dtCnt.Rows[0]["branch_cpEmail"].ToString().Trim();
                            }
                            if (mailid.Length > 0)
                            {
                                if (oDBEngine.SendReportBr(emailbdy, mailid, billdate, Subject, contactid) == true)
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct22", "displaydate('" + SpanText + "')", true);
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "jscript23", "MailsendT();", true);

                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct24", "displaydate('" + SpanText + "')", true);
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "jscript25", "MailsendF();", true);
                                }

                            }

                        }
                        else if (ddlGroup.SelectedItem.Value.ToString() == "1")
                        {

                            DataTable dtCnt = oDBEngine.GetDataTable(" tbl_master_groupmaster  ", "    *  ", "gpm_id='" + dtEml.Rows[i]["groupid"].ToString() + "'");
                            AddEmlHtml += EmailTbl.ToString();
                            emailbdy = "<table width=\"950px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"1\"><tr style=\"background-color:#FDD2FD;font-weight:bold;text-align:center;\"><td>Group Name: " + dtEml.Rows[i]["groupname"].ToString() + "</td></tr><tr><td> " + AddEmlHtml + "</td></tr></table>";
                            AddEmlHtml = "";
                            EmailTbl = "";
                            if (dtCnt.Rows.Count > 0)
                            {
                                contactid = dtEml.Rows[i]["groupid"].ToString();
                                mailid = dtCnt.Rows[0]["gpm_emailID"].ToString().Trim();
                            }
                            if (mailid.Length > 0)
                            {
                                if (oDBEngine.SendReportBr(emailbdy, mailid, billdate, Subject, contactid) == true)
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct26", "displaydate('" + SpanText + "')", true);
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "jscript27", "MailsendT();", true);

                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct28", "displaydate('" + SpanText + "')", true);
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "jscript29", "MailsendF();", true);
                                }

                            }
                        }


                        //}
                    }


                }
                else if (rbClientUser.Checked)
                {
                    if (i < dtEml.Rows.Count - 1)
                    {
                        AddEmlHtml += EmailTbl.ToString();
                        EmailTbl = "";

                    }
                    else if (i == dtEml.Rows.Count - 1)
                    {
                        AddEmlHtml += EmailTbl.ToString();
                        emailbdy = AddEmlHtml;
                        AddEmlHtml = "";
                        EmailTbl = "";
                        if (User != "")
                        {
                            string[] clnt = User.ToString().Split(',');
                            for (int m = 0; m < clnt.Length; m++)
                            {
                                contactid = clnt[m].ToString();
                                if (oDBEngine.SendReportSt(emailbdy, contactid, billdate, Subject) == true)
                                {

                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct30", "displaydate('" + SpanText + "')", true);
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "jscript33", "MailsendT();", true);
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct38", "displaydate('" + SpanText + "')", true);
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "jscript31", "MailsendF();", true);
                                }
                            }
                        }

                    }


                }
            }

        }



        void bindTableFORADATEEmail()
        {

            String strHtml = String.Empty;
            if (ds.Tables[0].Rows.Count != 0 || ds.Tables[3].Rows.Count != 0 || ds.Tables[4].Rows.Count != 0)
            {
                decimal totalmtm = decimal.Zero;
                decimal totalexp = decimal.Zero;
                decimal totalmtmexp = decimal.Zero;
                decimal mtm = decimal.Zero;
                decimal exp = decimal.Zero;

                decimal prm = decimal.Zero;
                decimal asnexc = decimal.Zero; ;
                string expirydate = null;

                decimal totaldeposit = decimal.Zero;
                decimal stortageexcess = decimal.Zero;
                strHtml = "<table width=\"950px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\">";

                if (ds.Tables[0].Rows.Count != 0)
                {

                    DataTable dtstore1st = new DataTable();
                    dtstore1st = ds.Tables[1];
                    DataView dv = new DataView(dtstore1st);

                    DataTable dtstore2st = new DataTable();
                    dtstore2st = ds.Tables[2];
                    DataView dv1 = new DataView(dtstore2st);




                    //////////////////////MAIN TABLE/////////////////////////

                    /////////////////////////////FOR MTM////////////////////
                    strHtml += "<tr style=\"background-color:#BB694D;color:White;\">";
                    strHtml += "<td align=\"center\" colspan=2><b>MTM Obligation</b></td>";
                    strHtml += "<td align=\"center\"><b>Trade Date</b></td>";
                    strHtml += "<td align=\"center\"><b>Trade No.</b></td>";
                    strHtml += "<td align=\"center\"><b>Buy Qty</b></td>";
                    strHtml += "<td align=\"center\"><b>Sell Qty</b></td>";
                    strHtml += "<td align=\"center\"><b>Mkt Rate</b></td>";
                    strHtml += "<td align=\"center\"><b>Brkg</b></td>";
                    strHtml += "<td align=\"center\"><b>Net Rate</b></td>";
                    strHtml += "<td align=\"center\"><b>Amount Dr.</b></td>";
                    strHtml += "<td align=\"center\"><b>Amount Cr.</b></td></tr>";

                    /////////////////////////////////////Display Opening Ledger Balance//////////////////////////////

                    DataTable dtOpen = new DataTable();
                    dtOpen = oDBEngine.OpeningBalanceOnlyJournal("'SYSTM00001'", CLValue, Convert.ToDateTime(dtfor.Value), Session["usersegid"].ToString(), Session["LastCompany"].ToString(), Convert.ToDateTime(dtfor.Value));
                    if (dtOpen.Rows.Count > 0)
                    {
                        openingbalance = Convert.ToDecimal(dtOpen.Rows[0][1].ToString());
                        if (openingbalance < 0)
                        {
                            strHtml += "<tr style=\"background-color:White;\"><td style=\"color:Blue;border-top:2px;border-bottom:2px;border-style: groove;border-right-style: none; \" align=\"right\" colspan=9><b>Opening Ledger Balance as on " + oconverter.ArrangeDate2(dtfor.Value.ToString()) + "<b/></td>";
                            strHtml += "<td align=\"center\" style=\"color:Black;border-top:2px;border-bottom:2px;border-style: groove;border-right-style: none;border-left-style: none; \"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(Math.Abs(openingbalance))) + "</b></td>";
                            strHtml += "<td align=\"center\" style=\"color:Black;border-top:2px;border-bottom:2px;border-style: groove;border-left-style: none; \">&nbsp;</td></tr>";
                        }
                        else
                        {
                            strHtml += "<tr style=\"background-color:White;\"><td style=\"color:Blue;border-top:2px;border-bottom:2px;border-style: groove;border-right-style: none; \" align=\"right\" colspan=9><b>Opening Ledger Balance as on " + oconverter.ArrangeDate2(dtfor.Value.ToString()) + "<b/></td>";
                            strHtml += "<td align=\"center\" style=\"color:Black;border-top:2px;border-bottom:2px;border-style: groove;border-right-style: none;border-left-style: none;  \">&nbsp;</td>";
                            strHtml += "<td align=\"center\" style=\"color:Black;border-top:2px;border-bottom:2px;border-style: groove;border-left-style: none; \"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(openingbalance)) + "</b></td></tr>";
                        }
                    }


                    ///////////////////////////////////////////////ALL MTM DATA BIND///////////////////////////////////////////////
                    int flag = 0;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        totalmtm = decimal.Zero;
                        totalexp = decimal.Zero;

                        flag = flag + 1;
                        //////////////////////////////******** BF POSITION DATA BIND***********///////////////////////////
                        strHtml += "<tr style=\"background-color: " + GetRowColor(flag) + " ;text-align:left\">";
                        strHtml += "<td colspan=2 align=left style=\"color:maroon;\">" + dr["tabSymbol"] + "</td>";
                        expirydate = dr["expirydate"].ToString();
                        if (dr["BFQuantity"] != DBNull.Value)
                        {
                            strHtml += "<td colspan=2 align=left  style=\"color:blue;\">Brought Forward</td>";

                            if (Convert.ToDecimal(dr["BFQuantity"]) < 0)
                            {
                                strHtml += "<td align=right>" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Math.Abs(Convert.ToDecimal(dr["BFQuantity"].ToString()))) + "</td>";
                                strHtml += "<td> &nbsp;</td>";
                            }
                            else
                            {
                                strHtml += "<td> &nbsp;</td>";
                                strHtml += "<td align=right>" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dr["BFQuantity"].ToString())) + "</td>";
                            }
                            strHtml += "<td align=right>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dr["OpenPrice"].ToString())) + "</td>";
                            strHtml += "<td> &nbsp;</td>";
                            strHtml += "<td> &nbsp;</td>";
                            if (Convert.ToDecimal(dr["BFValue"]) < 0)
                            {
                                strHtml += "<td align=right>" + oconverter.getFormattedvaluewithoriginalsign(Math.Abs(Convert.ToDecimal(dr["BFValue"].ToString()))) + "</td>";
                                strHtml += "<td> &nbsp;</td>";
                            }
                            else
                            {
                                strHtml += "<td> &nbsp;</td>";
                                strHtml += "<td align=right>" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dr["BFValue"].ToString())) + "</td>";
                            }
                            if (Convert.ToDateTime(expirydate) == Convert.ToDateTime(dtfor.Value))
                            {
                                totalexp = Convert.ToDecimal(dr["BFValue"]);
                            }
                            else
                            {
                                totalmtm = Convert.ToDecimal(dr["BFValue"]);
                            }

                        }
                        else
                        {
                            strHtml += "<td colspan=9> &nbsp;</td>";
                        }


                        strHtml += "</tr>";
                        ////////////////////////////////********BF POSITION DATA END*********/////////////////////////

                        dv.RowFilter = "tabProductSeriesID = '" + dr["tabProductSeriesID"] + "'";
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        dt = dv.ToTable();
                        foreach (DataRow row in dt.Rows)
                        {
                            ///////////////////////////////*********ALL TRADE FETCH*************//////////////////////////

                            flag = flag + 1;
                            strHtml += "<tr style=\"background-color: " + GetRowColor(flag) + " ;text-align:left\">";
                            strHtml += "<td colspan=2> &nbsp;</td>";
                            strHtml += "<td align=\"left\">" + row["MTMTradeDate"] + "</td>";
                            if (row["MTMRefID"].ToString() != "Expiry")
                            {
                                strHtml += "<td align=\"right\" >" + row["MTMRefID"] + "</td>";
                            }
                            else
                            {
                                strHtml += "<td align=\"right\" ><b>" + row["MTMRefID"] + "</b></td>";

                            }

                            if (row["MTMBUYQUANTITY"] != DBNull.Value)
                            {
                                strHtml += "<td align=\"right\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(row["MTMBUYQUANTITY"].ToString())) + "</td>";
                            }
                            else
                            {
                                strHtml += "<td>&nbsp;</td>";
                            }

                            if (row["MTMSELLQUANTITY"] != DBNull.Value)
                            {
                                strHtml += "<td align=\"right\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(row["MTMSELLQUANTITY"].ToString())) + "</td>";
                            }
                            else
                            {
                                strHtml += "<td> &nbsp;</td>";
                            }

                            if (row["MktRate"] != DBNull.Value)
                            {
                                strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(row["MktRate"].ToString())) + "</td>";
                            }
                            else
                            {
                                strHtml += "<td> &nbsp;</td>";
                            }

                            if (row["MTMBrkg"] != DBNull.Value)
                            {
                                strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(row["MTMBrkg"].ToString())) + "</td>";
                            }
                            else
                            {
                                strHtml += "<td> &nbsp;</td>";
                            }

                            if (row["MTMNetRatePerUnit"] != DBNull.Value)
                            {
                                strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(row["MTMNetRatePerUnit"].ToString())) + "</td>";

                            }
                            else
                            {
                                strHtml += "<td> &nbsp;</td>";
                            }


                            if (row["MTMAMOUNTDR"] != DBNull.Value)
                            {
                                strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluewithoriginalsign(Math.Abs(Convert.ToDecimal(row["MTMAMOUNTDR"].ToString()))) + "</td>";

                                if (Convert.ToDateTime(expirydate) == Convert.ToDateTime(dtfor.Value))
                                {
                                    totalexp = totalexp - Convert.ToDecimal(row["MTMAMOUNTDR"]);
                                }
                                else
                                {
                                    totalmtm = totalmtm - Convert.ToDecimal(row["MTMAMOUNTDR"]);
                                }
                            }
                            else
                            {
                                strHtml += "<td> &nbsp;</td>";
                            }

                            if (row["MTMAMOUNTCR"] != DBNull.Value)
                            {
                                strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(row["MTMAMOUNTCR"].ToString())) + "</td>";
                                if (Convert.ToDateTime(expirydate) == Convert.ToDateTime(dtfor.Value))
                                {
                                    totalexp = totalexp + Convert.ToDecimal(row["MTMAMOUNTCR"]);
                                }
                                else
                                {
                                    totalmtm = totalmtm + Convert.ToDecimal(row["MTMAMOUNTCR"]);
                                }
                            }
                            else
                            {
                                strHtml += "<td align=\"center\"> &nbsp;</td>";
                            }

                            strHtml += "</tr>";

                        }

                        //////////////////////////////**********ALL TRADE FETCH END********///////////////////////////
                        dv1.RowFilter = "tabProductSeriesID = '" + dr["tabProductSeriesID"] + "'";
                        DataTable dt1 = new DataTable();
                        dt1 = ds.Tables[0];
                        dt1 = dv1.ToTable();
                        foreach (DataRow row1 in dt1.Rows)
                        {
                            if (row1["CFQuantity"] != DBNull.Value)
                            {
                                flag = flag + 1;
                                //////////////////////////////******** CF POSITION DATA BIND***********///////////////////////////
                                strHtml += "<tr style=\"background-color: " + GetRowColor(flag) + " ;text-align:left\">";
                                strHtml += "<td colspan=2> &nbsp;</td>";


                                if (Convert.ToDateTime(expirydate) == Convert.ToDateTime(dtfor.Value))
                                {
                                    strHtml += "<td colspan=2 align=left style=\"color:Green;\">Expiry</td>";
                                }
                                else
                                {
                                    strHtml += "<td colspan=2 align=left style=\"color:Green;\">Carried Forward</td>";
                                }

                                if (Convert.ToDecimal(row1["CFQuantity"]) > 0)/////////OPPOSITE DIRECTION
                                {
                                    strHtml += "<td align=right>" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Math.Abs(Convert.ToDecimal(row1["CFQuantity"].ToString()))) + "</td>";
                                    strHtml += "<td> &nbsp;</td>";
                                }
                                else
                                {
                                    strHtml += "<td> &nbsp;</td>";
                                    strHtml += "<td align=right>" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Math.Abs(Convert.ToDecimal(row1["CFQuantity"].ToString()))) + "</td>";
                                }
                                strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(row1["SettPrice"].ToString())) + "</td>";
                                strHtml += "<td> &nbsp;</td>";
                                strHtml += "<td> &nbsp;</td>";
                                if (Convert.ToDecimal(row1["CFValue"]) > 0)/////////OPPOSITE DIRECTION
                                {
                                    strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluewithoriginalsign(Math.Abs(Convert.ToDecimal(row1["CFValue"].ToString()))) + "</td>";
                                    strHtml += "<td> &nbsp;</td>";
                                }
                                else
                                {
                                    strHtml += "<td> &nbsp;</td>";
                                    strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluewithoriginalsign(Math.Abs(Convert.ToDecimal(row1["CFValue"].ToString()))) + "</td>";
                                }

                                strHtml += "</tr>";

                                if (Convert.ToDateTime(expirydate) == Convert.ToDateTime(dtfor.Value))
                                {
                                    totalexp = totalexp - Convert.ToDecimal(row1["CFValue"]);
                                }
                                else
                                {
                                    totalmtm = totalmtm - Convert.ToDecimal(row1["CFValue"]);
                                }
                            }

                        }
                        ////////////////////////////////********CF POSITION DATA END*********/////////////////////////
                        //////////////////////////////////SUMMARY DISPLAY/////////////////////////////////////////////
                        strHtml += "<tr style=\"background-color:White;\">";
                        strHtml += "<td align=right colspan=11><div style=\"background-color:lavender;\">";
                        strHtml += "<table width=\"350px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\">";

                        if (Convert.ToDateTime(expirydate) == Convert.ToDateTime(dtfor.Value))
                        {
                            strHtml += "<tr><td align=left style=\"width:10%;\">Final Settlement</td>";
                            if (totalexp < 0)
                            {
                                strHtml += "<td align=\"right\" style=\"width:13%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(totalexp))) + "</b></td>";
                                strHtml += "<td style=\"width:7%;\"> &nbsp;</td>";
                                strHtml += "</tr>";
                            }
                            else
                            {
                                strHtml += "<td style=\"width:10%;\"> &nbsp;</td>";
                                strHtml += "<td align=\"right\" style=\"width:10%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(totalexp))) + "</b></td>";
                                strHtml += "</tr>";
                            }
                        }
                        else
                        {
                            strHtml += "<tr><td align=left style=\"width:10%;\">MTM Settlement</td>";

                            if (totalmtm < 0)
                            {
                                strHtml += "<td align=\"right\" style=\"width:13%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(totalmtm))) + "</b></td>";
                                strHtml += "<td style=\"width:7%;\"> &nbsp;</td>";
                                strHtml += "</tr>";
                            }
                            else
                            {
                                strHtml += "<td style=\"width:10%;\"> &nbsp;</td>";
                                strHtml += "<td align=\"right\" style=\"width:10%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(totalmtm))) + "</b></td>";
                                strHtml += "</tr>";
                            }
                        }
                        strHtml += "</table></div></td></tr>";
                        mtm = mtm + totalmtm;
                        exp = exp + totalexp;
                        //////////////////////////////////////END//////////////////////////////////////////////////////

                    }
                    //////////////////////////////////Final Settlement of All Contracts DISPLAY/////////////////////////////////////////////
                    strHtml += "<tr style=\"height:1px;background-color:White;\"><td colspan=11>&nbsp;</td></tr><tr style=\"background-color:White;\">";
                    strHtml += "<td align=right colspan=11><div style=\"border: solid 1px balck;\">";
                    strHtml += "<table width=\"950px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"1\">";
                    totalmtmexp = mtm + exp;

                    if (exp != 0)
                    {
                        strHtml += "<tr><td align=left >Final Settlement of All Contracts:</td>";
                        if (exp < 0)
                        {
                            strHtml += "<td align=\"right\" style=\"color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(exp))) + "</b></td>";
                            strHtml += "<td></td>";
                            strHtml += "</tr>";
                        }
                        else
                        {
                            strHtml += "<td> &nbsp;</td>";
                            strHtml += "<td align=\"right\" style=\"color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(exp))) + "</b></td>";
                            strHtml += "</tr>";
                        }
                    }
                    if (mtm != 0)
                    {
                        strHtml += "<tr><td align=left >MTM Of all contracts:</td>";

                        if (mtm < 0)
                        {
                            strHtml += "<td align=\"right\" style=\"color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(mtm))) + "</b></td>";
                            strHtml += "<td></td>";
                            strHtml += "</tr>";
                        }
                        else
                        {
                            strHtml += "<td> &nbsp;</td>";
                            strHtml += "<td align=\"right\" style=\"width:15%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(mtm))) + "</b></td>";
                            strHtml += "</tr>";
                        }
                    }
                    strHtml += "<tr><td align=left >Net MTM and Final Settlement of All Contracts:</td>";

                    if (totalmtmexp < 0)
                    {
                        strHtml += "<td align=\"right\" style=\"color:maroon;\"> <b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(totalmtmexp))) + "</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "</tr>";
                    }
                    else
                    {
                        strHtml += "<td> &nbsp;</td>";
                        strHtml += "<td align=\"right\" style=\"color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(totalmtmexp))) + "</b></td>";
                        strHtml += "</tr>";
                    }

                    strHtml += "</table></div></td></tr><tr style=\"height:1px;background-color:White;\"><td colspan=11>&nbsp;</td></tr>";
                    //////////////////////////////////////END//////////////////////////////////////////////////////

                }



                /////////////////////////////FOR Premium////////////////////
                if (ds.Tables[3].Rows.Count != 0)
                {
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        /////////////////////////////////////Display Opening Ledger Balance//////////////////////////////

                        DataTable dtOpen = new DataTable();
                        dtOpen = oDBEngine.OpeningBalanceOnlyJournal("'SYSTM00001'", CLValue, Convert.ToDateTime(dtfor.Value), Session["usersegid"].ToString(), Session["LastCompany"].ToString(), Convert.ToDateTime(dtfor.Value));
                        if (dtOpen.Rows.Count > 0)
                        {
                            openingbalance = Convert.ToDecimal(dtOpen.Rows[0][1].ToString());
                            if (openingbalance < 0)
                            {
                                strHtml += "<tr style=\"background-color:White;\"><td style=\"color:Blue;border-top:2px;border-bottom:2px;border-style: groove;border-right-style: none; \" align=\"right\" colspan=9><b>Opening Ledger Balance as on " + oconverter.ArrangeDate2(dtfor.Value.ToString()) + "<b/></td>";
                                strHtml += "<td align=\"center\" style=\"color:Black;border-top:2px;border-bottom:2px;border-style: groove;border-right-style: none;border-left-style: none; \"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(Math.Abs(openingbalance))) + "</b></td>";
                                strHtml += "<td align=\"center\" style=\"color:Black;border-top:2px;border-bottom:2px;border-style: groove;border-left-style: none; \">&nbsp;</td></tr>";
                            }
                            else
                            {
                                strHtml += "<tr style=\"background-color:White;\"><td style=\"color:Blue;border-top:2px;border-bottom:2px;border-style: groove;border-right-style: none; \" align=\"right\" colspan=9><b>Opening Ledger Balance as on " + oconverter.ArrangeDate2(dtfor.Value.ToString()) + "<b/></td>";
                                strHtml += "<td align=\"center\" style=\"color:Black;border-top:2px;border-bottom:2px;border-style: groove;border-right-style: none;border-left-style: none;  \">&nbsp;</td>";
                                strHtml += "<td align=\"center\" style=\"color:Black;border-top:2px;border-bottom:2px;border-style: groove;border-left-style: none; \"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(openingbalance)) + "</b></td></tr>";
                            }
                        }
                    }
                    strHtml += "<tr style=\"background-color:#BB694D;color:White;\">";
                    strHtml += "<td align=\"center\" ><b>Premium Obligation</b></td>";
                    strHtml += "<td align=\"center\" ><b>Strike Price</b></td>";
                    strHtml += "<td align=\"center\"><b>Trade Date</b></td>";
                    strHtml += "<td align=\"center\"><b>Trade No.</b></td>";
                    strHtml += "<td align=\"center\"><b>Buy Qty</b></td>";
                    strHtml += "<td align=\"center\"><b>Sell Qty</b></td>";
                    strHtml += "<td align=\"center\"><b>Premium</b></td>";
                    strHtml += "<td align=\"center\"><b>Brkg</b></td>";
                    strHtml += "<td align=\"center\"><b>Net Prm</b></td>";
                    strHtml += "<td align=\"center\"><b>Amount Dr.</b></td>";
                    strHtml += "<td align=\"center\"><b>Amount Cr.</b></td></tr>";
                    int flag1 = 0;
                    string productid = null;
                    decimal netprm = decimal.Zero;
                    foreach (DataRow row in ds.Tables[3].Rows)
                    {
                        ////////////////////////////////////PRIMIUM SUMMARY DISPLAY///////////////////////////
                        if (productid != null)
                        {
                            if (productid != row["tabProductSeriesID"].ToString())
                            {

                                strHtml += "<tr style=\"background-color:White;\">";
                                strHtml += "<td align=right colspan=11><div style=\"background-color:lavender;\">";
                                strHtml += "<table width=\"100%\" border=" + Convert.ToChar(34) + 0 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                                strHtml += "<tr><td align=left style=\"width:10%;\">Net Premium </td>";
                                if (netprm < 0)
                                {
                                    strHtml += "<td align=\"right\" style=\"width:13%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(netprm))) + "</b></td>";
                                    strHtml += "<td style=\"width:7%;\"> &nbsp;</td>";
                                    strHtml += "</tr>";
                                }
                                else
                                {
                                    strHtml += "<td style=\"width:10%;\"> &nbsp;</td>";
                                    strHtml += "<td align=\"right\" style=\"width:10%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(netprm))) + "</b></td>";
                                    strHtml += "</tr>";
                                }
                                prm = prm + netprm;
                                netprm = decimal.Zero;
                                strHtml += "</table></div></td></tr>";
                            }
                        }
                        //////////////////////////////////////////END////////////////////////////////////////////////
                        productid = row["tabProductSeriesID"].ToString();

                        flag1 = flag1 + 1;

                        strHtml += "<tr style=\"background-color: " + GetRowColor(flag1) + " ;text-align:left\">";
                        if (netprm == 0)
                        {
                            strHtml += "<td align=\"left\"  style=\"color:maroon;width:20%;\">" + row["tabSymbol"] + "</td>";
                            if (row["StrikePrice"] != DBNull.Value)
                            {
                                strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(row["StrikePrice"].ToString())) + "</td>";

                            }
                            else
                            {
                                strHtml += "<td>&nbsp;</td>";
                            }
                        }
                        else
                        {
                            strHtml += "<td>&nbsp;</td>";
                            strHtml += "<td>&nbsp;</td>";
                        }
                        strHtml += "<td align=\"left\">" + row["PRMTradeDate"] + "</td>";
                        strHtml += "<td align=\"right\">" + row["PRMRefNo"] + "</td>";

                        if (row["PRMBUYQUANTITY"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(row["PRMBUYQUANTITY"].ToString())) + "</td>";
                        }
                        else
                        {
                            strHtml += "<td>&nbsp;</td>";
                        }

                        if (row["PRMSELLQUANTITY"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(row["PRMSELLQUANTITY"].ToString())) + "</td>";
                        }
                        else
                        {
                            strHtml += "<td> &nbsp;</td>";
                        }
                        if (row["PRMUnitPrice"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(row["PRMUnitPrice"].ToString())) + "</td>";
                        }
                        else
                        {
                            strHtml += "<td> &nbsp;</td>";
                        }


                        if (row["PRMBrkg"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(row["PRMBrkg"].ToString())) + "</td>";
                        }
                        else
                        {
                            strHtml += "<td> &nbsp;</td>";
                        }

                        if (row["PRMNetRatePerUnit"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(row["PRMNetRatePerUnit"].ToString())) + "</td>";

                        }
                        else
                        {
                            strHtml += "<td> &nbsp;</td>";
                        }


                        if (row["PRMAMOUNTDR"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluewithoriginalsign(Math.Abs(Convert.ToDecimal(row["PRMAMOUNTDR"].ToString()))) + "</td>";
                            netprm = netprm - Convert.ToDecimal(row["PRMAMOUNTDR"]);
                        }
                        else
                        {
                            strHtml += "<td> &nbsp;</td>";
                        }

                        if (row["PRMAMOUNTCR"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(row["PRMAMOUNTCR"].ToString())) + "</td>";
                            netprm = netprm + Convert.ToDecimal(row["PRMAMOUNTCR"]);

                        }
                        else
                        {
                            strHtml += "<td> &nbsp;</td>";
                        }
                        strHtml += "</tr>";



                    }
                    ////////////////////////////////////PRIMIUM SUMMARY DISPLAY///////////////////////////

                    strHtml += "<tr style=\"background-color:White;\">";
                    strHtml += "<td align=right colspan=11><div style=\"background-color:lavender;\">";
                    strHtml += "<table width=\"950px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"1\">";
                    strHtml += "<tr><td align=left style=\"width:10%;\">Net Premium </td>";
                    if (netprm < 0)
                    {
                        strHtml += "<td align=\"right\" style=\"width:13%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(netprm))) + "</b></td>";
                        strHtml += "<td style=\"width:7%;\"> &nbsp;</td>";
                        strHtml += "</tr>";
                    }
                    else
                    {
                        strHtml += "<td style=\"width:10%;\"> &nbsp;</td>";
                        strHtml += "<td align=\"right\" style=\"width:10%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(netprm))) + "</b></td>";
                        strHtml += "</tr>";
                    }
                    prm = prm + netprm;
                    netprm = decimal.Zero;
                    strHtml += "</table></div></td></tr>";

                    //////////////////////////////////////////END////////////////////////////////////////////////
                    //////////////////////////////////Net Premium of All Contracts DISPLAY/////////////////////////////////////////////
                    strHtml += "<tr style=\"height:1px;background-color:White;\"><td colspan=11>&nbsp;</td></tr><tr style=\"background-color:White;\">";
                    strHtml += "<td align=\"right\" colspan=11><div style=\"border: solid 1px balck;\">";
                    strHtml += "<table width=\"400px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"1\">";
                    strHtml += "<tr><td align=left style=\"width:15%;\">Net Premium of all contracts:</td>";
                    if (prm < 0)
                    {
                        strHtml += "<td align=\"right\" style=\"width:11%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(prm))) + "</b></td>";
                        strHtml += "<td style=\"width:4%;\"> &nbsp;</td>";
                        strHtml += "</tr>";
                    }
                    else
                    {
                        strHtml += "<td style=\"width:5%;\"> &nbsp;</td>";
                        strHtml += "<td align=\"right\" style=\"width:15%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(prm))) + "</b></td>";
                        strHtml += "</tr>";
                    }

                    strHtml += "</table></div></td></tr><tr style=\"height:1px;background-color:White;\"><td colspan=11>&nbsp;</td></tr>";
                    //////////////////////////////////////END//////////////////////////////////////////////////////
                }

                /////////////////////////////FOR Premium END//////////////////// 

                /////////////////////////////FOR ASN,EXC////////////////////
                if (ds.Tables[4].Rows.Count != 0)
                {
                    if (ds.Tables[0].Rows.Count == 0 && ds.Tables[3].Rows.Count == 0)
                    {
                        /////////////////////////////////////Display Opening Ledger Balance//////////////////////////////

                        DataTable dtOpen = new DataTable();
                        dtOpen = oDBEngine.OpeningBalanceOnlyJournal("'SYSTM00001'", CLValue, Convert.ToDateTime(dtfor.Value), Session["usersegid"].ToString(), Session["LastCompany"].ToString(), Convert.ToDateTime(dtfor.Value));
                        if (dtOpen.Rows.Count > 0)
                        {
                            openingbalance = Convert.ToDecimal(dtOpen.Rows[0][1].ToString());
                            if (openingbalance < 0)
                            {
                                strHtml += "<tr style=\"background-color:White;\"><td style=\"color:Blue;border-top:2px;border-bottom:2px;border-style: groove;border-right-style: none; \" align=\"right\" colspan=9><b>Opening Ledger Balance as on " + oconverter.ArrangeDate2(dtfor.Value.ToString()) + "<b/></td>";
                                strHtml += "<td align=\"center\" style=\"color:Black;border-top:2px;border-bottom:2px;border-style: groove;border-right-style: none;border-left-style: none; \"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(Math.Abs(openingbalance))) + "</b></td>";
                                strHtml += "<td align=\"center\" style=\"color:Black;border-top:2px;border-bottom:2px;border-style: groove;border-left-style: none; \">&nbsp;</td></tr>";
                            }
                            else
                            {
                                strHtml += "<tr style=\"background-color:White;\"><td style=\"color:Blue;border-top:2px;border-bottom:2px;border-style: groove;border-right-style: none; \" align=\"right\" colspan=9><b>Opening Ledger Balance as on " + oconverter.ArrangeDate2(dtfor.Value.ToString()) + "<b/></td>";
                                strHtml += "<td align=\"center\" style=\"color:Black;border-top:2px;border-bottom:2px;border-style: groove;border-right-style: none;border-left-style: none;  \">&nbsp;</td>";
                                strHtml += "<td align=\"center\" style=\"color:Black;border-top:2px;border-bottom:2px;border-style: groove;border-left-style: none; \"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(openingbalance)) + "</b></td></tr>";
                            }
                        }
                    }
                    strHtml += "<tr style=\"background-color: #DBEEF3;\">";
                    strHtml += "<td align=\"center\" ><b>Options Final Settlement</b></td>";
                    strHtml += "<td align=\"center\" ><b>Strike Price</b></td>";
                    strHtml += "<td align=\"center\"><b>Trade Date</b></td>";
                    strHtml += "<td align=\"center\"><b>Sett.Type</b></td>";
                    strHtml += "<td align=\"center\"><b>Qty</b></td>";
                    strHtml += "<td align=\"center\"><b>Sett Price</b></td>";
                    strHtml += "<td align=\"center\"><b>Rate</b></td>";
                    strHtml += "<td align=\"center\"><b>Sett Charg</b></td>";
                    strHtml += "<td align=\"center\"><b>Net Rate</b></td>";
                    strHtml += "<td align=\"center\"><b>Amount Dr.</b></td>";
                    strHtml += "<td align=\"center\"><b>Amount Cr.</b></td></tr>";
                    int flag2 = 0;
                    foreach (DataRow row in ds.Tables[4].Rows)
                    {
                        flag2 = flag2 + 1;

                        strHtml += "<tr style=\"background-color: " + GetRowColor(flag2) + " ;text-align:left\">";
                        strHtml += "<td align=\"left\"  style=\"color:maroon;\">" + row["tabSymbol"] + "</td>";
                        if (row["StrikePrice"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(row["StrikePrice"].ToString())) + "</td>";

                        }
                        else
                        {
                            strHtml += "<td>&nbsp;</td>";
                        }
                        strHtml += "<td align=\"left\">" + row["ASNEXCTradeDate"] + "</td>";
                        strHtml += "<td align=\"left\">" + row["setttype"] + "</td>";

                        if (row["ASNEXCQUANTITY"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(row["ASNEXCQUANTITY"].ToString())) + "</td>";
                        }
                        else
                        {
                            strHtml += "<td>&nbsp;</td>";
                        }

                        if (row["ASNEXCSettPrice"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(row["ASNEXCSettPrice"].ToString())) + "</td>";
                        }
                        else
                        {
                            strHtml += "<td> &nbsp;</td>";
                        }
                        if (row["ASNEXCUnitPrice"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(row["ASNEXCUnitPrice"].ToString())) + "</td>";
                        }
                        else
                        {
                            strHtml += "<td> &nbsp;</td>";
                        }


                        if (row["ASNEXCBrkg"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(row["ASNEXCBrkg"].ToString())) + "</td>";
                        }
                        else
                        {
                            strHtml += "<td> &nbsp;</td>";
                        }

                        if (row["ASNEXCNetRatePerUnit"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(row["ASNEXCNetRatePerUnit"].ToString())) + "</td>";

                        }
                        else
                        {
                            strHtml += "<td> &nbsp;</td>";
                        }


                        if (row["ASNEXCAMOUNTDR"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluewithoriginalsign(Math.Abs(Convert.ToDecimal(row["ASNEXCAMOUNTDR"].ToString()))) + "</td>";
                            asnexc = asnexc - Convert.ToDecimal(row["ASNEXCAMOUNTDR"]);
                        }
                        else
                        {
                            strHtml += "<td> &nbsp;</td>";
                        }

                        if (row["ASNEXCAMOUNTCR"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(row["ASNEXCAMOUNTCR"].ToString())) + "</td>";
                            asnexc = asnexc + Convert.ToDecimal(row["ASNEXCAMOUNTCR"]);

                        }
                        else
                        {
                            strHtml += "<td> &nbsp;</td>";
                        }
                        strHtml += "</tr>";

                    }
                    //////////////////////////////////Net Options Final Settlement DISPLAY/////////////////////////////////////////////
                    strHtml += "<tr style=\"height:1px;background-color:White;\"><td colspan=11>&nbsp;</td></tr><tr style=\"background-color:White;\">";
                    strHtml += "<td align=right colspan=11><div style=\"border: solid 1px balck;\">";
                    strHtml += "<table width=\"950px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"1\">";
                    strHtml += "<tr><td align=left style=\"width:35%;\">Net Options Final Settlement :</td>";
                    if (asnexc < 0)
                    {
                        strHtml += "<td align=\"right\" style=\"width:11%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(asnexc))) + "</b></td>";
                        strHtml += "<td style=\"width:4%;\"> &nbsp;</td>";
                        strHtml += "</tr>";
                    }
                    else
                    {
                        strHtml += "<td style=\"width:5%;\"> &nbsp;</td>";
                        strHtml += "<td align=\"right\" style=\"width:15%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(asnexc))) + "</b></td>";
                        strHtml += "</tr>";
                    }

                    strHtml += "</div></table></td></tr><tr style=\"height:1px;background-color:White;\"><td colspan=11>&nbsp;</td></tr>";
                    //////////////////////////////////////END//////////////////////////////////////////////////////

                }

                /////////////////////////////FOR ASN,EXC END//////////////////// 
                ////////////////////////////ALL DETAILS SUMMARY//////////////////////////
                decimal totalobligation = decimal.Zero;
                decimal totalcharges = decimal.Zero;
                totalobligation = totalmtmexp + prm + asnexc;

                strHtml += "<tr style=\"height:1px;background-color:White;\"><td colspan=11>&nbsp;</td></tr><tr style=\"background-color:White;\">";
                strHtml += "<td align=right colspan=11><div style=\"border: solid 1px balck;\">";
                strHtml += "<table width=\"420px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"1\">";
                strHtml += "<tr><td align=left style=\"width:15%;\">Total Obligation For the Day :</td>";
                if (totalobligation < 0)
                {
                    strHtml += "<td align=\"right\" style=\"width:13%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(totalobligation))) + "</b></td>";
                    strHtml += "<td style=\"width:7%;\"> &nbsp;</td>";
                    strHtml += "</tr>";
                }
                else
                {
                    strHtml += "<td style=\"width:10%;\"> &nbsp;</td>";
                    strHtml += "<td align=\"right\" style=\"width:10%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(totalobligation))) + "</b></td>";
                    strHtml += "</tr>";
                }
                if (ds.Tables[5].Rows.Count > 0)
                {
                    if (ds.Tables[5].Rows[0]["Servicetaxonbrkg"] != DBNull.Value)
                    {
                        strHtml += "<tr><td align=left style=\"width:30%;\">Serv Tax & Cess on Brokerage :</td><td align=\"right\" style=\"width:13%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(ds.Tables[5].Rows[0]["Servicetaxonbrkg"].ToString()))) + "</td><td style=\"width:7%;\"> &nbsp;</td></tr>";
                        totalcharges = totalcharges + Convert.ToDecimal(ds.Tables[5].Rows[0]["Servicetaxonbrkg"]);
                    }
                }
                if (ds.Tables[6].Rows.Count > 0)
                {
                    if (ds.Tables[6].Rows[0]["Temp_TranCharge"] != DBNull.Value)
                    {
                        strHtml += "<tr><td align=left style=\"width:30%;\">Transaction Charges :</td><td align=\"right\" style=\"width:13%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(ds.Tables[6].Rows[0]["Temp_TranCharge"].ToString()))) + "</td><td style=\"width:7%;\"> &nbsp;</td></tr>";
                        totalcharges = totalcharges + Convert.ToDecimal(ds.Tables[6].Rows[0]["Temp_TranCharge"]);

                    }
                    if (ds.Tables[6].Rows[0]["charges"] != DBNull.Value)
                    {
                        strHtml += "<tr><td align=left style=\"width:30%;\">Serv Tax & Cess on Tran Charge:</td><td align=\"right\" style=\"width:13%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(ds.Tables[6].Rows[0]["charges"].ToString()))) + "</td><td style=\"width:7%;\"> &nbsp;</td></tr>";
                        totalcharges = totalcharges + Convert.ToDecimal(ds.Tables[6].Rows[0]["charges"]);

                    }
                }
                if (ds.Tables[7].Rows.Count > 0)
                {
                    if (ds.Tables[7].Rows[0]["Temp_TotalStamduty"] != DBNull.Value)
                    {
                        strHtml += "<tr><td align=left style=\"width:30%;\">Stamp Duty:</td><td align=\"right\" style=\"width:13%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(ds.Tables[7].Rows[0]["Temp_TotalStamduty"].ToString()))) + "</td><td style=\"width:7%;\"> &nbsp;</td></tr>";
                        totalcharges = totalcharges + Convert.ToDecimal(ds.Tables[7].Rows[0]["Temp_TotalStamduty"]);

                    }
                }
                if (ds.Tables[8].Rows.Count > 0)
                {
                    if (ds.Tables[8].Rows[0]["exchstttax"] != DBNull.Value)
                    {
                        strHtml += "<tr><td align=left style=\"width:30%;\">STT Tax :</td><td align=\"right\" style=\"width:13%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(ds.Tables[8].Rows[0]["exchstttax"].ToString()))) + "</td><td style=\"width:7%;\"> &nbsp;</td></tr>";
                        totalcharges = totalcharges + Convert.ToDecimal(ds.Tables[8].Rows[0]["exchstttax"]);
                    }
                }
                strHtml += "<tr><td align=left style=\"width:30%;\">Total Charges :</td><td align=\"right\" style=\"width:13%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(totalcharges))) + "</td><td style=\"width:7%;\"> &nbsp;</td></tr>";
                if ((totalobligation - totalcharges) < 0)
                {
                    strHtml += "<tr><td align=left style=\"width:30%;\"><u>Net Bill Amount :</u></td><td align=\"right\" style=\"width:13%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(totalobligation - totalcharges))) + "</td><td style=\"width:7%;\"> &nbsp;</td></tr>";

                }
                else
                {
                    strHtml += "<tr><td align=left style=\"width:30%;\"><u>Net Bill Amount :</u></td><td style=\"width:10%;\"> &nbsp;</td><td align=\"right\" style=\"width:10%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(totalobligation - totalcharges))) + "</td></tr>";

                }

                DataTable dtfund = new DataTable();
                dtfund = oDBEngine.NetFundAdjustment("'SYSTM00001'", CLValue, Session["usersegid"].ToString(), Session["LastCompany"].ToString(), Convert.ToDateTime(dtfor.Value));

                if (dtfund.Rows.Count > 0)
                {
                    netfund = Convert.ToDecimal(dtfund.Rows[0][0].ToString());
                    if (netfund < 0)
                    {
                        strHtml += "<tr><td align=left style=\"width:30%;\">Net Fund Adjustment For the Period :</td><td align=\"right\" style=\"width:13%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(netfund))) + "</td><td style=\"width:7%;\"> &nbsp;</td></tr>";

                    }
                    else
                    {
                        strHtml += "<tr><td align=left style=\"width:30%;\">Net Fund Adjustment For the Period :</td><td style=\"width:10%;\"> &nbsp;</td><td align=\"right\" style=\"width:10%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(netfund))) + "</td></tr>";
                    }
                }
                netamount = totalobligation - totalcharges + (openingbalance + netfund);

                if (netamount < 0)
                {
                    strHtml += "<tr><td align=left style=\"width:30%;\">Net Amount Receivable By Us: </td><td align=\"right\" style=\"width:13%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(netamount))) + "</td><td style=\"width:7%;\"> &nbsp;</td></tr>";

                }
                else
                {
                    strHtml += "<tr><td align=left style=\"width:30%;\">Net Amount Payable To You :</td><td style=\"width:10%;\"> &nbsp;</td><td align=\"right\" style=\"width:10%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(netamount))) + "</td></tr>";
                }
                strHtml += "</table></div></td></tr><tr style=\"height:1px;background-color:White;\"><td colspan=11>&nbsp;</td></tr>";
                ///////////////////////////////END///////////////////////////////////////
                ////////////////////////////////Margin obligation///////////////////////
                if (ds.Tables[9].Rows.Count != 0)
                {

                    strHtml += "<tr style=\"background-color: #FFD4AA; color: Black;\">";
                    strHtml += "<td align=\"center\" ><b>Margin Summary</b></td>";
                    strHtml += "<td align=\"center\" ><b>Span Margin</b></td>";
                    strHtml += "<td align=\"center\"><b>Premium Margin</b></td>";
                    strHtml += "<td align=\"center\"><b>Total Margin</b></td>";
                    strHtml += "<td align=\"center\"><b>Exposure Margin</b></td>";
                    strHtml += "<td align=\"center\"><b>Applicable Margin</b></td>";
                    strHtml += "<td align=\"center\"><b>Cash Dep</b></td>";
                    strHtml += "<td align=\"center\"><b><b>Collaterals</b></td>";
                    strHtml += "<td align=\"center\"><b>Total Deposit</b></td>";
                    strHtml += "<td align=\"center\"><b>Shortage</b></td>";
                    strHtml += "<td align=\"center\"><b>Excess</b></td></tr>";

                    strHtml += "<tr style=\"background-color: white;\"><td> &nbsp;</td>";
                    strHtml += "<td  align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[9].Rows[0]["SpanMargin"])) + "</td>";
                    strHtml += "<td  align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[9].Rows[0]["BuyPremium"])) + "</td>";
                    strHtml += "<td  align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[9].Rows[0]["TotalMargin"])) + "</td>";
                    strHtml += "<td  align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[9].Rows[0]["ExposureMargin"])) + "</td>";
                    strHtml += "<td  align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[9].Rows[0]["ApplicableMargin"])) + "</td>";

                    DataTable dtmargin = new DataTable();
                    dtmargin = oDBEngine.OpeningBalanceOnlyJournal("'SYSTM00002','SYSTM00003'", CLValue, Convert.ToDateTime(dtfor.Value), Session["usersegid"].ToString(), Session["LastCompany"].ToString(), Convert.ToDateTime(dtfor.Value));
                    if (dtmargin.Rows.Count > 0)
                    {
                        strHtml += "<td  align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtmargin.Rows[0][0].ToString())) + "</td>";
                        totaldeposit = Convert.ToDecimal(dtmargin.Rows[0][0].ToString()) + Convert.ToDecimal(ds.Tables[9].Rows[0]["coleteral"]);

                    }
                    else
                    {
                        strHtml += "<td> &nbsp;</td>";
                    }
                    strHtml += "<td  align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[9].Rows[0]["coleteral"])) + "</td>";
                    strHtml += "<td  align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(totaldeposit)) + "</td>";

                    stortageexcess = totaldeposit - Convert.ToDecimal(ds.Tables[9].Rows[0]["ApplicableMargin"]);
                    if (stortageexcess < 0)
                    {
                        strHtml += "<td  align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(Math.Abs(stortageexcess))) + "</td>";
                        strHtml += "<td align=\"right\"></td></tr>";
                    }
                    else
                    {
                        strHtml += "<td  align=\"right\"></td>";
                        strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(stortageexcess)) + "</td></tr>";
                    }

                }

                ////////////////////////////////END////////////////////////////////////
                ////////////////////////////////////Net Amount After Adjusting Margin Shortage :///////////////////////////

                strHtml += "<tr><td style=\"height:10px;background-color:White;\" colspan=11></td></tr><tr style=\"background-color:White;\">";
                strHtml += "<td align=right colspan=11><div style=\"border: solid 1px balck;\">";
                strHtml += "<table width=\"950px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\"><tr>";

                if (stortageexcess < 0)
                {
                    if (netamount + stortageexcess > 0)
                    {
                        strHtml += "<td align=\"left\" style=\"width:35%;\">Net Amount Payable  After Adjusting Margin Shortage :</td>";
                        strHtml += "<td align=\"right\" style=\"width:15%;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs((netamount + stortageexcess))) + "</b></td>";

                    }
                    else
                    {
                        strHtml += "<td align=\"left\" style=\"width:35%;\">Net Amount Receivable By Us After Adjusting Margin Shortage :</td>";
                        strHtml += "<td align=\"right\" style=\"width:15%;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs((netamount + stortageexcess))) + "</b></td>";

                    }
                }
                else
                {
                    if (netamount > 0)
                    {
                        strHtml += "<td align=\"left\" style=\"width:35%;\">Net Amount Payable To You After Adjusting Margin Shortage :</td>";
                        strHtml += "<td style=\"width:10%;\"></td><td align=\"right\" style=\"width:10%;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs((netamount))) + "</b></td>";

                    }
                    else
                    {
                        strHtml += "<td align=\"left\" style=\"width:35%;\">Net Amount Receivable BY Us After Adjusting Margin Shortage :</td>";
                        strHtml += "<td align=\"right\" style=\"width:15%;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs((netamount))) + "</b></td>";

                    }
                }
                strHtml += "</tr></table></div></td></tr>";

                //////////////////////////////////////////END////////////////////////////////////////////////

                strHtml += "</table>";
                SEmailTbl = strHtml;
                //display.InnerHtml = strHtml;
                ds.Dispose();
                /////////////////////////////DISPLAY DATE/////////////////////////////////////

                if (ddldate.SelectedItem.Value == "0")
                {
                    string SpanText = oconverter.ArrangeDate2(dtfor.Value.ToString());
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct", "displaydate('" + SpanText + "')", true);

                }
                else
                {
                    string SpanText = oconverter.ArrangeDate2(dtfrom.Value.ToString()) + " " + "to" + " " + oconverter.ArrangeDate2(dtto.Value.ToString());
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct", "displaydate('" + SpanText + "')", true);

                }

                ///////////////////////////////END//////////////////////////////////////////
                ScriptManager.RegisterStartupScript(this, this.GetType(), "displaygrid", "displayresult();", true);



            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "norecord", "norecord(1);", true);

            }

        }
        void bindTableFORAPeriodEmail()
        {

            String strHtml = String.Empty;
            if (ds.Tables[0].Rows.Count != 0 || ds.Tables[3].Rows.Count != 0 || ds.Tables[4].Rows.Count != 0)
            {
                decimal totalmtm = decimal.Zero;
                decimal totalexp = decimal.Zero;
                decimal totalmtmexp = decimal.Zero;
                decimal mtm = decimal.Zero;
                decimal exp = decimal.Zero;

                decimal prm = decimal.Zero;
                decimal asnexc = decimal.Zero; ;
                string expirydate = null;

                decimal totaldeposit = decimal.Zero;
                decimal stortageexcess = decimal.Zero;
                strHtml = "<table width=\"950px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\">";

                if (ds.Tables[0].Rows.Count != 0)
                {

                    DataTable dtstore1st = new DataTable();
                    dtstore1st = ds.Tables[1];
                    DataView dv = new DataView(dtstore1st);

                    DataTable dtstore2st = new DataTable();
                    dtstore2st = ds.Tables[2];
                    DataView dv1 = new DataView(dtstore2st);




                    //////////////////////MAIN TABLE/////////////////////////

                    /////////////////////////////FOR MTM////////////////////
                    strHtml += "<tr style=\"background-color:#BB694D;color:White;\">";
                    strHtml += "<td align=\"center\" colspan=2><b>MTM Obligation</b></td>";
                    strHtml += "<td align=\"center\"><b>Trade Date</b></td>";
                    strHtml += "<td align=\"center\"><b>Trade No.</b></td>";
                    strHtml += "<td align=\"center\"><b>Buy Qty</b></td>";
                    strHtml += "<td align=\"center\"><b>Sell Qty</b></td>";
                    strHtml += "<td align=\"center\"><b>Mkt Rate</b></td>";
                    strHtml += "<td align=\"center\"><b>Brkg</b></td>";
                    strHtml += "<td align=\"center\"><b>Net Rate</b></td>";
                    strHtml += "<td align=\"center\"><b>Amount Dr.</b></td>";
                    strHtml += "<td align=\"center\"><b>Amount Cr.</b></td></tr>";

                    /////////////////////////////////////Display Opening Ledger Balance//////////////////////////////

                    DataTable dtOpen = new DataTable();
                    dtOpen = oDBEngine.OpeningBalanceOnlyJournal("'SYSTM00001'", CLValue, Convert.ToDateTime(dtfrom.Value), Session["usersegid"].ToString(), Session["LastCompany"].ToString(), Convert.ToDateTime(dtto.Value));
                    if (dtOpen.Rows.Count > 0)
                    {
                        openingbalance = Convert.ToDecimal(dtOpen.Rows[0][1].ToString());
                        if (openingbalance < 0)
                        {
                            strHtml += "<tr style=\"background-color:White;\"><td style=\"color:Blue;border-top:2px;border-bottom:2px;border-style: groove;border-right-style: none; \" align=\"right\" colspan=9><b>Opening Ledger Balance as on " + oconverter.ArrangeDate2(dtfrom.Value.ToString()) + "<b/></td>";
                            strHtml += "<td align=\"center\" style=\"color:Black;border-top:2px;border-bottom:2px;border-style: groove;border-right-style: none;border-left-style: none; \"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(Math.Abs(openingbalance))) + "</b></td>";
                            strHtml += "<td align=\"center\" style=\"color:Black;border-top:2px;border-bottom:2px;border-style: groove;border-left-style: none; \">&nbsp;</td></tr>";
                        }
                        else
                        {
                            strHtml += "<tr style=\"background-color:White;\"><td style=\"color:Blue;border-top:2px;border-bottom:2px;border-style: groove;border-right-style: none; \" align=\"right\" colspan=9><b>Opening Ledger Balance as on " + oconverter.ArrangeDate2(dtfrom.Value.ToString()) + "<b/></td>";
                            strHtml += "<td align=\"center\" style=\"color:Black;border-top:2px;border-bottom:2px;border-style: groove;border-right-style: none;border-left-style: none;  \">&nbsp;</td>";
                            strHtml += "<td align=\"center\" style=\"color:Black;border-top:2px;border-bottom:2px;border-style: groove;border-left-style: none; \"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(openingbalance)) + "</b></td></tr>";
                        }
                    }


                    ///////////////////////////////////////////////ALL MTM DATA BIND///////////////////////////////////////////////
                    int flag = 0;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        totalmtm = decimal.Zero;
                        totalexp = decimal.Zero;

                        flag = flag + 1;
                        //////////////////////////////******** BF POSITION DATA BIND***********///////////////////////////
                        strHtml += "<tr style=\"background-color: " + GetRowColor(flag) + " ;text-align:left\">";
                        strHtml += "<td colspan=2 align=left style=\"color:maroon;\">" + dr["tabSymbol"] + "</td>";
                        expirydate = dr["expirydate"].ToString();
                        if (dr["BFQuantity"] != DBNull.Value)
                        {
                            strHtml += "<td colspan=2 align=left  style=\"color:blue;\">Brought Forward</td>";

                            if (Convert.ToDecimal(dr["BFQuantity"]) < 0)
                            {
                                strHtml += "<td align=right>" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Math.Abs(Convert.ToDecimal(dr["BFQuantity"].ToString()))) + "</td>";
                                strHtml += "<td> &nbsp;</td>";
                            }
                            else
                            {
                                strHtml += "<td> &nbsp;</td>";
                                strHtml += "<td align=right>" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dr["BFQuantity"].ToString())) + "</td>";
                            }
                            strHtml += "<td align=right>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dr["OpenPrice"].ToString())) + "</td>";
                            strHtml += "<td> &nbsp;</td>";
                            strHtml += "<td> &nbsp;</td>";
                            if (Convert.ToDecimal(dr["BFValue"]) < 0)
                            {
                                strHtml += "<td align=right>" + oconverter.getFormattedvaluewithoriginalsign(Math.Abs(Convert.ToDecimal(dr["BFValue"].ToString()))) + "</td>";
                                strHtml += "<td> &nbsp;</td>";
                            }
                            else
                            {
                                strHtml += "<td> &nbsp;</td>";
                                strHtml += "<td align=right>" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dr["BFValue"].ToString())) + "</td>";
                            }
                            if (Convert.ToDateTime(expirydate) == Convert.ToDateTime(dtto.Value))
                            {
                                totalexp = Convert.ToDecimal(dr["BFValue"]);
                            }
                            else
                            {
                                totalmtm = Convert.ToDecimal(dr["BFValue"]);
                            }

                        }
                        else
                        {
                            strHtml += "<td colspan=9> &nbsp;</td>";
                        }


                        strHtml += "</tr>";
                        ////////////////////////////////********BF POSITION DATA END*********/////////////////////////

                        dv.RowFilter = "tabProductSeriesID = '" + dr["tabProductSeriesID"] + "'";
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        dt = dv.ToTable();
                        foreach (DataRow row in dt.Rows)
                        {
                            ///////////////////////////////*********ALL TRADE FETCH*************//////////////////////////

                            flag = flag + 1;
                            strHtml += "<tr style=\"background-color: " + GetRowColor(flag) + " ;text-align:left\">";
                            strHtml += "<td colspan=2> &nbsp;</td>";
                            strHtml += "<td align=\"left\">" + row["MTMTradeDate"] + "</td>";
                            if (row["MTMRefID"].ToString() != "Expiry")
                            {
                                strHtml += "<td align=\"right\" >" + row["MTMRefID"] + "</td>";
                            }
                            else
                            {
                                strHtml += "<td align=\"right\" ><b>" + row["MTMRefID"] + "</b></td>";

                            }

                            if (row["MTMBUYQUANTITY"] != DBNull.Value)
                            {
                                strHtml += "<td align=\"right\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(row["MTMBUYQUANTITY"].ToString())) + "</td>";
                            }
                            else
                            {
                                strHtml += "<td>&nbsp;</td>";
                            }

                            if (row["MTMSELLQUANTITY"] != DBNull.Value)
                            {
                                strHtml += "<td align=\"right\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(row["MTMSELLQUANTITY"].ToString())) + "</td>";
                            }
                            else
                            {
                                strHtml += "<td> &nbsp;</td>";
                            }

                            if (row["MktRate"] != DBNull.Value)
                            {
                                strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(row["MktRate"].ToString())) + "</td>";
                            }
                            else
                            {
                                strHtml += "<td> &nbsp;</td>";
                            }

                            if (row["MTMBrkg"] != DBNull.Value)
                            {
                                strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(row["MTMBrkg"].ToString())) + "</td>";
                            }
                            else
                            {
                                strHtml += "<td> &nbsp;</td>";
                            }

                            if (row["MTMNetRatePerUnit"] != DBNull.Value)
                            {
                                strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(row["MTMNetRatePerUnit"].ToString())) + "</td>";

                            }
                            else
                            {
                                strHtml += "<td> &nbsp;</td>";
                            }


                            if (row["MTMAMOUNTDR"] != DBNull.Value)
                            {
                                strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluewithoriginalsign(Math.Abs(Convert.ToDecimal(row["MTMAMOUNTDR"].ToString()))) + "</td>";

                                if (Convert.ToDateTime(expirydate) == Convert.ToDateTime(dtto.Value))
                                {
                                    totalexp = totalexp - Convert.ToDecimal(row["MTMAMOUNTDR"]);
                                }
                                else
                                {
                                    totalmtm = totalmtm - Convert.ToDecimal(row["MTMAMOUNTDR"]);
                                }
                            }
                            else
                            {
                                strHtml += "<td> &nbsp;</td>";
                            }

                            if (row["MTMAMOUNTCR"] != DBNull.Value)
                            {
                                strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(row["MTMAMOUNTCR"].ToString())) + "</td>";
                                if (Convert.ToDateTime(expirydate) == Convert.ToDateTime(dtto.Value))
                                {
                                    totalexp = totalexp + Convert.ToDecimal(row["MTMAMOUNTCR"]);
                                }
                                else
                                {
                                    totalmtm = totalmtm + Convert.ToDecimal(row["MTMAMOUNTCR"]);
                                }
                            }
                            else
                            {
                                strHtml += "<td align=\"center\"> &nbsp;</td>";
                            }

                            strHtml += "</tr>";

                        }

                        //////////////////////////////**********ALL TRADE FETCH END********///////////////////////////
                        dv1.RowFilter = "tabProductSeriesID = '" + dr["tabProductSeriesID"] + "'";
                        DataTable dt1 = new DataTable();
                        dt1 = ds.Tables[0];
                        dt1 = dv1.ToTable();
                        foreach (DataRow row1 in dt1.Rows)
                        {
                            if (row1["CFQuantity"] != DBNull.Value)
                            {
                                flag = flag + 1;
                                //////////////////////////////******** CF POSITION DATA BIND***********///////////////////////////
                                strHtml += "<tr style=\"background-color: " + GetRowColor(flag) + " ;text-align:left\">";
                                strHtml += "<td colspan=2> &nbsp;</td>";


                                if (Convert.ToDateTime(expirydate) == Convert.ToDateTime(dtto.Value))
                                {
                                    strHtml += "<td colspan=2 align=left style=\"color:Green;\">Expiry</td>";
                                }
                                else
                                {
                                    strHtml += "<td colspan=2 align=left style=\"color:Green;\">Carried Forward</td>";
                                }

                                if (Convert.ToDecimal(row1["CFQuantity"]) > 0)/////////OPPOSITE DIRECTION
                                {
                                    strHtml += "<td align=right>" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Math.Abs(Convert.ToDecimal(row1["CFQuantity"].ToString()))) + "</td>";
                                    strHtml += "<td> &nbsp;</td>";
                                }
                                else
                                {
                                    strHtml += "<td> &nbsp;</td>";
                                    strHtml += "<td align=right>" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Math.Abs(Convert.ToDecimal(row1["CFQuantity"].ToString()))) + "</td>";
                                }
                                strHtml += "<td align=right>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(row1["SettPrice"].ToString())) + "</td>";
                                strHtml += "<td> &nbsp;</td>";
                                strHtml += "<td> &nbsp;</td>";
                                if (Convert.ToDecimal(row1["CFValue"]) > 0)/////////OPPOSITE DIRECTION
                                {
                                    strHtml += "<td align=right>" + oconverter.getFormattedvaluewithoriginalsign(Math.Abs(Convert.ToDecimal(row1["CFValue"].ToString()))) + "</td>";
                                    strHtml += "<td> &nbsp;</td>";
                                }
                                else
                                {
                                    strHtml += "<td> &nbsp;</td>";
                                    strHtml += "<td align=right>" + oconverter.getFormattedvaluewithoriginalsign(Math.Abs(Convert.ToDecimal(row1["CFValue"].ToString()))) + "</td>";
                                }

                                strHtml += "</tr>";

                                if (Convert.ToDateTime(expirydate) == Convert.ToDateTime(dtto.Value))
                                {
                                    totalexp = totalexp - Convert.ToDecimal(row1["CFValue"]);
                                }
                                else
                                {
                                    totalmtm = totalmtm - Convert.ToDecimal(row1["CFValue"]);
                                }
                            }

                        }
                        ////////////////////////////////********CF POSITION DATA END*********/////////////////////////
                        //////////////////////////////////SUMMARY DISPLAY/////////////////////////////////////////////
                        strHtml += "<tr style=\"background-color:White;\">";
                        strHtml += "<td align=right colspan=11><div style=\"background-color:lavender;\">";
                        strHtml += "<table width=\"450px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\">";

                        if (Convert.ToDateTime(expirydate) == Convert.ToDateTime(dtto.Value))
                        {
                            strHtml += "<tr><td align=left style=\"width:10%;\">Final Settlement</td>";
                            if (totalexp < 0)
                            {
                                strHtml += "<td align=\"right\" style=\"width:13%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(totalexp))) + "</b></td>";
                                strHtml += "<td style=\"width:7%;\"> &nbsp;</td>";
                                strHtml += "</tr>";
                            }
                            else
                            {
                                strHtml += "<td style=\"width:10%;\"> &nbsp;</td>";
                                strHtml += "<td align=\"right\" style=\"width:10%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(totalexp))) + "</b></td>";
                                strHtml += "</tr>";
                            }
                        }
                        else
                        {
                            strHtml += "<tr><td align=left style=\"width:10%;\">MTM Settlement</td>";

                            if (totalmtm < 0)
                            {
                                strHtml += "<td align=\"right\" style=\"width:13%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(totalmtm))) + "</b></td>";
                                strHtml += "<td style=\"width:7%;\"> &nbsp;</td>";
                                strHtml += "</tr>";
                            }
                            else
                            {
                                strHtml += "<td style=\"width:10%;\"> &nbsp;</td>";
                                strHtml += "<td align=\"right\" style=\"width:10%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(totalmtm))) + "</b></td>";
                                strHtml += "</tr>";
                            }
                        }
                        strHtml += "</table></div></td></tr>";
                        mtm = mtm + totalmtm;
                        exp = exp + totalexp;
                        //////////////////////////////////////END//////////////////////////////////////////////////////

                    }
                    //////////////////////////////////Final Settlement of All Contracts DISPLAY/////////////////////////////////////////////
                    strHtml += "<tr style=\"height:1px;background-color:White;\"><td colspan=11>&nbsp;</td></tr><tr style=\"background-color:White;\">";
                    strHtml += "<td align=right colspan=11><div style=\"border: solid 1px balck;width:50%;\">";
                    strHtml += "<table width=\"420px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"1\">";
                    totalmtmexp = mtm + exp;

                    if (exp != 0)
                    {
                        strHtml += "<tr><td align=left style=\"width:35%;\">Final Settlement of All Contracts:</td>";
                        if (exp < 0)
                        {
                            strHtml += "<td align=\"right\" style=\"width:19%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(exp))) + "</b></td>";
                            strHtml += "<td style=\"width:7%;\"></td>";
                            strHtml += "</tr>";
                        }
                        else
                        {
                            strHtml += "<td style=\"width:10%;\"> &nbsp;</td>";
                            strHtml += "<td align=\"right\" style=\"width:15%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(exp))) + "</b></td>";
                            strHtml += "</tr>";
                        }
                    }
                    if (mtm != 0)
                    {
                        strHtml += "<tr><td align=left style=\"width:35%;\">MTM Of all contracts:</td>";

                        if (mtm < 0)
                        {
                            strHtml += "<td align=\"right\" style=\"width:26%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(mtm))) + "</b></td>";
                            strHtml += "<td style=\"width:10%;\"></td>";
                            strHtml += "</tr>";
                        }
                        else
                        {
                            strHtml += "<td style=\"width:10%;\"> &nbsp;</td>";
                            strHtml += "<td align=\"right\" style=\"width:15%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(mtm))) + "</b></td>";
                            strHtml += "</tr>";
                        }
                    }
                    strHtml += "<tr><td align=left style=\"width:35%;\">Net MTM and Final Settlement of All Contracts:</td>";

                    if (totalmtmexp < 0)
                    {
                        strHtml += "<td align=\"right\" style=\"width:19%;color:maroon;\"> <b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(totalmtmexp))) + "</b></td>";
                        strHtml += "<td style=\"width:7%;\"></td>";
                        strHtml += "</tr>";
                    }
                    else
                    {
                        strHtml += "<td style=\"width:10%;\"> &nbsp;</td>";
                        strHtml += "<td align=\"right\" style=\"width:15%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(totalmtmexp))) + "</b></td>";
                        strHtml += "</tr>";
                    }

                    strHtml += "</table></div></td></tr><tr style=\"height:1px;background-color:White;\"><td colspan=11>&nbsp;</td></tr>";
                    //////////////////////////////////////END//////////////////////////////////////////////////////

                }



                /////////////////////////////FOR Premium////////////////////
                if (ds.Tables[3].Rows.Count != 0)
                {
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        /////////////////////////////////////Display Opening Ledger Balance//////////////////////////////

                        DataTable dtOpen = new DataTable();
                        dtOpen = oDBEngine.OpeningBalanceOnlyJournal("'SYSTM00001'", CLValue, Convert.ToDateTime(dtfrom.Value), Session["usersegid"].ToString(), Session["LastCompany"].ToString(), Convert.ToDateTime(dtto.Value));
                        if (dtOpen.Rows.Count > 0)
                        {
                            openingbalance = Convert.ToDecimal(dtOpen.Rows[0][1].ToString());
                            if (openingbalance < 0)
                            {
                                strHtml += "<tr style=\"background-color:White;\"><td style=\"color:Blue;border-top:2px;border-bottom:2px;border-style: groove;border-right-style: none; \" align=\"right\" colspan=9><b>Opening Ledger Balance as on " + oconverter.ArrangeDate2(dtfrom.Value.ToString()) + "<b/></td>";
                                strHtml += "<td align=\"center\" style=\"color:Black;border-top:2px;border-bottom:2px;border-style: groove;border-right-style: none;border-left-style: none; \"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(Math.Abs(openingbalance))) + "</b></td>";
                                strHtml += "<td align=\"center\" style=\"color:Black;border-top:2px;border-bottom:2px;border-style: groove;border-left-style: none; \">&nbsp;</td></tr>";
                            }
                            else
                            {
                                strHtml += "<tr style=\"background-color:White;\"><td style=\"color:Blue;border-top:2px;border-bottom:2px;border-style: groove;border-right-style: none; \" align=\"right\" colspan=9><b>Opening Ledger Balance as on " + oconverter.ArrangeDate2(dtfrom.Value.ToString()) + "<b/></td>";
                                strHtml += "<td align=\"center\" style=\"color:Black;border-top:2px;border-bottom:2px;border-style: groove;border-right-style: none;border-left-style: none;  \">&nbsp;</td>";
                                strHtml += "<td align=\"center\" style=\"color:Black;border-top:2px;border-bottom:2px;border-style: groove;border-left-style: none; \"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(openingbalance)) + "</b></td></tr>";
                            }
                        }

                    }
                    strHtml += "<tr style=\"background-color: #DBEEF3;\">";
                    strHtml += "<td align=\"center\" ><b>Premium Obligation</b></td>";
                    strHtml += "<td align=\"center\" ><b>Strike Price</b></td>";
                    strHtml += "<td align=\"center\"><b>Trade Date</b></td>";
                    strHtml += "<td align=\"center\"><b>Trade No.</b></td>";
                    strHtml += "<td align=\"center\"><b>Buy Qty</b></td>";
                    strHtml += "<td align=\"center\"><b>Sell Qty</b></td>";
                    strHtml += "<td align=\"center\"><b>Premium</b></td>";
                    strHtml += "<td align=\"center\"><b>Brkg</b></td>";
                    strHtml += "<td align=\"center\"><b>Net Prm</b></td>";
                    strHtml += "<td align=\"center\"><b>Amount Dr.</b></td>";
                    strHtml += "<td align=\"center\"><b>Amount Cr.</b></td></tr>";
                    int flag1 = 0;
                    string productid = null;
                    decimal netprm = decimal.Zero;
                    foreach (DataRow row in ds.Tables[3].Rows)
                    {
                        ////////////////////////////////////PRIMIUM SUMMARY DISPLAY///////////////////////////
                        if (productid != null)
                        {
                            if (productid != row["tabProductSeriesID"].ToString())
                            {

                                strHtml += "<tr style=\"background-color:White;\">";
                                strHtml += "<td align=right colspan=11><div style=\"background-color:lavender;\">";
                                strHtml += "<table width=\"450px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"1\">";
                                strHtml += "<tr><td align=left style=\"width:10%;\">Net Premium </td>";
                                if (netprm < 0)
                                {
                                    strHtml += "<td align=\"right\" style=\"width:13%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(netprm))) + "</b></td>";
                                    strHtml += "<td style=\"width:7%;\"> &nbsp;</td>";
                                    strHtml += "</tr>";
                                }
                                else
                                {
                                    strHtml += "<td style=\"width:10%;\"> &nbsp;</td>";
                                    strHtml += "<td align=\"right\" style=\"width:10%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(netprm))) + "</b></td>";
                                    strHtml += "</tr>";
                                }
                                prm = prm + netprm;
                                netprm = decimal.Zero;
                                strHtml += "</table></div></td></tr>";
                            }
                        }
                        //////////////////////////////////////////END////////////////////////////////////////////////
                        productid = row["tabProductSeriesID"].ToString();

                        flag1 = flag1 + 1;

                        strHtml += "<tr style=\"background-color: " + GetRowColor(flag1) + " ;text-align:left\">";
                        if (netprm == 0)
                        {
                            strHtml += "<td align=\"left\"  style=\"color:maroon;width:20%;\">" + row["tabSymbol"] + "</td>";
                            if (row["StrikePrice"] != DBNull.Value)
                            {
                                strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(row["StrikePrice"].ToString())) + "</td>";

                            }
                            else
                            {
                                strHtml += "<td>&nbsp;</td>";
                            }
                        }
                        else
                        {
                            strHtml += "<td>&nbsp;</td>";
                            strHtml += "<td>&nbsp;</td>";
                        }
                        strHtml += "<td align=\"left\">" + row["PRMTradeDate"] + "</td>";
                        strHtml += "<td align=\"right\">" + row["PRMRefNo"] + "</td>";

                        if (row["PRMBUYQUANTITY"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(row["PRMBUYQUANTITY"].ToString())) + "</td>";
                        }
                        else
                        {
                            strHtml += "<td>&nbsp;</td>";
                        }

                        if (row["PRMSELLQUANTITY"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(row["PRMSELLQUANTITY"].ToString())) + "</td>";
                        }
                        else
                        {
                            strHtml += "<td> &nbsp;</td>";
                        }
                        if (row["PRMUnitPrice"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(row["PRMUnitPrice"].ToString())) + "</td>";
                        }
                        else
                        {
                            strHtml += "<td> &nbsp;</td>";
                        }


                        if (row["PRMBrkg"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(row["PRMBrkg"].ToString())) + "</td>";
                        }
                        else
                        {
                            strHtml += "<td> &nbsp;</td>";
                        }

                        if (row["PRMNetRatePerUnit"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(row["PRMNetRatePerUnit"].ToString())) + "</td>";

                        }
                        else
                        {
                            strHtml += "<td> &nbsp;</td>";
                        }


                        if (row["PRMAMOUNTDR"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluewithoriginalsign(Math.Abs(Convert.ToDecimal(row["PRMAMOUNTDR"].ToString()))) + "</td>";
                            netprm = netprm - Convert.ToDecimal(row["PRMAMOUNTDR"]);
                        }
                        else
                        {
                            strHtml += "<td> &nbsp;</td>";
                        }

                        if (row["PRMAMOUNTCR"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(row["PRMAMOUNTCR"].ToString())) + "</td>";
                            netprm = netprm + Convert.ToDecimal(row["PRMAMOUNTCR"]);

                        }
                        else
                        {
                            strHtml += "<td> &nbsp;</td>";
                        }
                        strHtml += "</tr>";



                    }
                    ////////////////////////////////////PRIMIUM SUMMARY DISPLAY///////////////////////////

                    strHtml += "<tr style=\"background-color:White;\">";
                    strHtml += "<td align=right colspan=11><div style=\"background-color:lavender;\">";
                    strHtml += "<table width=\"450px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"1\">";
                    strHtml += "<tr><td align=left style=\"width:10%;\">Net Premium </td>";
                    if (netprm < 0)
                    {
                        strHtml += "<td align=\"right\" style=\"width:13%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(netprm))) + "</b></td>";
                        strHtml += "<td style=\"width:7%;\"> &nbsp;</td>";
                        strHtml += "</tr>";
                    }
                    else
                    {
                        strHtml += "<td style=\"width:10%;\"> &nbsp;</td>";
                        strHtml += "<td align=\"right\" style=\"width:10%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(netprm))) + "</b></td>";
                        strHtml += "</tr>";
                    }
                    prm = prm + netprm;
                    netprm = decimal.Zero;
                    strHtml += "</table></div></td></tr>";

                    //////////////////////////////////////////END////////////////////////////////////////////////
                    //////////////////////////////////Net Premium of All Contracts DISPLAY/////////////////////////////////////////////
                    strHtml += "<tr style=\"height:1px;background-color:White;\"><td colspan=11>&nbsp;</td></tr><tr style=\"background-color:White;\">";
                    strHtml += "<td align=right colspan=11><div style=\"border: solid 1px balck;\">";
                    strHtml += "<table width=\"450px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"1\">";
                    strHtml += "<tr><td align=left style=\"width:15%;\">Net Premium of all contracts:</td>";
                    if (prm < 0)
                    {
                        strHtml += "<td align=\"right\" style=\"width:11%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(prm))) + "</b></td>";
                        strHtml += "<td style=\"width:4%;\"> &nbsp;</td>";
                        strHtml += "</tr>";
                    }
                    else
                    {
                        strHtml += "<td style=\"width:5%;\"> &nbsp;</td>";
                        strHtml += "<td align=\"right\" style=\"width:15%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(prm))) + "</b></td>";
                        strHtml += "</tr>";
                    }

                    strHtml += "</table></div></td></tr><tr style=\"height:1px;background-color:White;\"><td colspan=11>&nbsp;</td></tr>";
                    //////////////////////////////////////END//////////////////////////////////////////////////////
                }

                /////////////////////////////FOR Premium END//////////////////// 

                /////////////////////////////FOR ASN,EXC////////////////////
                if (ds.Tables[4].Rows.Count != 0)
                {
                    if (ds.Tables[0].Rows.Count == 0 && ds.Tables[3].Rows.Count == 0)
                    {
                        /////////////////////////////////////Display Opening Ledger Balance//////////////////////////////

                        DataTable dtOpen = new DataTable();
                        dtOpen = oDBEngine.OpeningBalanceOnlyJournal("'SYSTM00001'", CLValue, Convert.ToDateTime(dtfrom.Value), Session["usersegid"].ToString(), Session["LastCompany"].ToString(), Convert.ToDateTime(dtto.Value));
                        if (dtOpen.Rows.Count > 0)
                        {
                            openingbalance = Convert.ToDecimal(dtOpen.Rows[0][1].ToString());
                            if (openingbalance < 0)
                            {
                                strHtml += "<tr style=\"background-color:White;\"><td style=\"color:Blue;border-top:2px;border-bottom:2px;border-style: groove;border-right-style: none; \" align=\"right\" colspan=9><b>Opening Ledger Balance as on " + oconverter.ArrangeDate2(dtfrom.Value.ToString()) + "<b/></td>";
                                strHtml += "<td align=\"center\" style=\"color:Black;border-top:2px;border-bottom:2px;border-style: groove;border-right-style: none;border-left-style: none; \"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(Math.Abs(openingbalance))) + "</b></td>";
                                strHtml += "<td align=\"center\" style=\"color:Black;border-top:2px;border-bottom:2px;border-style: groove;border-left-style: none; \">&nbsp;</td></tr>";
                            }
                            else
                            {
                                strHtml += "<tr style=\"background-color:White;\"><td style=\"color:Blue;border-top:2px;border-bottom:2px;border-style: groove;border-right-style: none; \" align=\"right\" colspan=9><b>Opening Ledger Balance as on " + oconverter.ArrangeDate2(dtfrom.Value.ToString()) + "<b/></td>";
                                strHtml += "<td align=\"center\" style=\"color:Black;border-top:2px;border-bottom:2px;border-style: groove;border-right-style: none;border-left-style: none;  \">&nbsp;</td>";
                                strHtml += "<td align=\"center\" style=\"color:Black;border-top:2px;border-bottom:2px;border-style: groove;border-left-style: none; \"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(openingbalance)) + "</b></td></tr>";
                            }
                        }

                    }
                    strHtml += "<tr style=\"background-color: #FFD4AA; color: Black;\">";
                    strHtml += "<td align=\"center\" ><b>Options Final Settlement</b></td>";
                    strHtml += "<td align=\"center\" ><b>Strike Price</b></td>";
                    strHtml += "<td align=\"center\"><b>Trade Date</b></td>";
                    strHtml += "<td align=\"center\"><b>Sett.Type</b></td>";
                    strHtml += "<td align=\"center\"><b>Qty</b></td>";
                    strHtml += "<td align=\"center\"><b>Sett Price</b></td>";
                    strHtml += "<td align=\"center\"><b>Rate</b></td>";
                    strHtml += "<td align=\"center\"><b>Sett Charg</b></td>";
                    strHtml += "<td align=\"center\"><b>Net Rate</b></td>";
                    strHtml += "<td align=\"center\"><b>Amount Dr.</b></td>";
                    strHtml += "<td align=\"center\"><b>Amount Cr.</b></td></tr>";
                    int flag2 = 0;
                    foreach (DataRow row in ds.Tables[4].Rows)
                    {
                        flag2 = flag2 + 1;

                        strHtml += "<tr style=\"background-color: " + GetRowColor(flag2) + " ;text-align:left\">";
                        strHtml += "<td align=\"left\"  style=\"color:maroon;\">" + row["tabSymbol"] + "</td>";
                        if (row["StrikePrice"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(row["StrikePrice"].ToString())) + "</td>";

                        }
                        else
                        {
                            strHtml += "<td>&nbsp;</td>";
                        }
                        strHtml += "<td align=\"left\">" + row["ASNEXCTradeDate"] + "</td>";
                        strHtml += "<td align=\"left\">" + row["setttype"] + "</td>";

                        if (row["ASNEXCQUANTITY"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(row["ASNEXCQUANTITY"].ToString())) + "</td>";
                        }
                        else
                        {
                            strHtml += "<td>&nbsp;</td>";
                        }

                        if (row["ASNEXCSettPrice"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(row["ASNEXCSettPrice"].ToString())) + "</td>";
                        }
                        else
                        {
                            strHtml += "<td> &nbsp;</td>";
                        }
                        if (row["ASNEXCUnitPrice"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(row["ASNEXCUnitPrice"].ToString())) + "</td>";
                        }
                        else
                        {
                            strHtml += "<td> &nbsp;</td>";
                        }


                        if (row["ASNEXCBrkg"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(row["ASNEXCBrkg"].ToString())) + "</td>";
                        }
                        else
                        {
                            strHtml += "<td> &nbsp;</td>";
                        }

                        if (row["ASNEXCNetRatePerUnit"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(row["ASNEXCNetRatePerUnit"].ToString())) + "</td>";

                        }
                        else
                        {
                            strHtml += "<td> &nbsp;</td>";
                        }


                        if (row["ASNEXCAMOUNTDR"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluewithoriginalsign(Math.Abs(Convert.ToDecimal(row["ASNEXCAMOUNTDR"].ToString()))) + "</td>";
                            asnexc = asnexc - Convert.ToDecimal(row["ASNEXCAMOUNTDR"]);
                        }
                        else
                        {
                            strHtml += "<td> &nbsp;</td>";
                        }

                        if (row["ASNEXCAMOUNTCR"] != DBNull.Value)
                        {
                            strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(row["ASNEXCAMOUNTCR"].ToString())) + "</td>";
                            asnexc = asnexc + Convert.ToDecimal(row["ASNEXCAMOUNTCR"]);

                        }
                        else
                        {
                            strHtml += "<td> &nbsp;</td>";
                        }
                        strHtml += "</tr>";

                    }
                    //////////////////////////////////Net Options Final Settlement DISPLAY/////////////////////////////////////////////
                    strHtml += "<tr style=\"height:1px;background-color:White;\"><td colspan=11>&nbsp;</td></tr><tr style=\"background-color:White;\">";
                    strHtml += "<td align=right colspan=11><div style=\"border: solid 1px balck;width:50%;\">";
                    strHtml += "<table  width=\"950px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"1\">";
                    strHtml += "<tr><td align=left style=\"width:35%;\">Net Options Final Settlement :</td>";
                    if (asnexc < 0)
                    {
                        strHtml += "<td align=\"right\" style=\"width:11%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(asnexc))) + "</b></td>";
                        strHtml += "<td style=\"width:4%;\"> &nbsp;</td>";
                        strHtml += "</tr>";
                    }
                    else
                    {
                        strHtml += "<td style=\"width:5%;\"> &nbsp;</td>";
                        strHtml += "<td align=\"right\" style=\"width:15%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(asnexc))) + "</b></td>";
                        strHtml += "</tr>";
                    }

                    strHtml += "</table></div></td></tr><tr style=\"height:1px;background-color:White;\"><td colspan=11>&nbsp;</td></tr>";
                    //////////////////////////////////////END//////////////////////////////////////////////////////

                }

                /////////////////////////////FOR ASN,EXC END//////////////////// 
                ////////////////////////////ALL DETAILS SUMMARY//////////////////////////
                decimal totalobligation = decimal.Zero;
                decimal totalcharges = decimal.Zero;
                totalobligation = totalmtmexp + prm + asnexc;

                strHtml += "<tr style=\"height:1px;background-color:White;\"><td colspan=11>&nbsp;</td></tr><tr style=\"background-color:White;\">";
                strHtml += "<td align=right colspan=11><div style=\"border: solid 1px balck;\">";
                strHtml += "<table width=\"450px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"1\">";
                strHtml += "<tr><td align=left style=\"width:15%;\">Total Obligation For the Day :</td>";
                if (totalobligation < 0)
                {
                    strHtml += "<td align=\"right\" style=\"width:13%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(totalobligation))) + "</b></td>";
                    strHtml += "<td style=\"width:7%;\"> &nbsp;</td>";
                    strHtml += "</tr>";
                }
                else
                {
                    strHtml += "<td style=\"width:10%;\"> &nbsp;</td>";
                    strHtml += "<td align=\"right\" style=\"width:10%;color:maroon;\"><b>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(totalobligation))) + "</b></td>";
                    strHtml += "</tr>";
                }
                if (ds.Tables[5].Rows.Count > 0)
                {
                    if (ds.Tables[5].Rows[0]["Servicetaxonbrkg"] != DBNull.Value)
                    {
                        strHtml += "<tr><td align=left style=\"width:30%;\">Serv Tax & Cess on Brokerage :</td><td align=\"right\" style=\"width:13%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(ds.Tables[5].Rows[0]["Servicetaxonbrkg"].ToString()))) + "</td><td style=\"width:7%;\"> &nbsp;</td></tr>";
                        totalcharges = totalcharges + Convert.ToDecimal(ds.Tables[5].Rows[0]["Servicetaxonbrkg"]);
                    }
                }
                if (ds.Tables[6].Rows.Count > 0)
                {
                    if (ds.Tables[6].Rows[0]["Temp_TranCharge"] != DBNull.Value)
                    {
                        strHtml += "<tr><td align=left style=\"width:30%;\">Transaction Charges :</td><td align=\"right\" style=\"width:13%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(ds.Tables[6].Rows[0]["Temp_TranCharge"].ToString()))) + "</td><td style=\"width:7%;\"> &nbsp;</td></tr>";
                        totalcharges = totalcharges + Convert.ToDecimal(ds.Tables[6].Rows[0]["Temp_TranCharge"]);

                    }
                    if (ds.Tables[6].Rows[0]["charges"] != DBNull.Value)
                    {
                        strHtml += "<tr><td align=left style=\"width:30%;\">Serv Tax & Cess on Tran Charge:</td><td align=\"right\" style=\"width:13%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(ds.Tables[6].Rows[0]["charges"].ToString()))) + "</td><td style=\"width:7%;\"> &nbsp;</td></tr>";
                        totalcharges = totalcharges + Convert.ToDecimal(ds.Tables[6].Rows[0]["charges"]);

                    }


                    if (ds.Tables[6].Rows[0]["Temp_TotalStamduty"] != DBNull.Value)
                    {
                        strHtml += "<tr><td align=left style=\"width:30%;\">Stamp Duty:</td><td align=\"right\" style=\"width:13%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(ds.Tables[6].Rows[0]["Temp_TotalStamduty"].ToString()))) + "</td><td style=\"width:7%;\"> &nbsp;</td></tr>";
                        totalcharges = totalcharges + Convert.ToDecimal(ds.Tables[6].Rows[0]["Temp_TotalStamduty"]);

                    }


                    if (ds.Tables[6].Rows[0]["exchstttax"] != DBNull.Value)
                    {
                        strHtml += "<tr><td align=left style=\"width:30%;\">STT Tax :</td><td align=\"right\" style=\"width:13%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(ds.Tables[6].Rows[0]["exchstttax"].ToString()))) + "</td><td style=\"width:7%;\"> &nbsp;</td></tr>";
                        totalcharges = totalcharges + Convert.ToDecimal(ds.Tables[6].Rows[0]["exchstttax"]);
                    }

                }
                strHtml += "<tr><td align=left style=\"width:30%;\">Total Charges :</td><td align=\"right\" style=\"width:13%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(totalcharges))) + "</td><td style=\"width:7%;\"> &nbsp;</td></tr>";
                if ((totalobligation - totalcharges) < 0)
                {
                    strHtml += "<tr><td align=left style=\"width:30%;\"><u>Net Bill Amount :</u></td><td align=\"right\" style=\"width:13%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(totalobligation - totalcharges))) + "</td><td style=\"width:7%;\"> &nbsp;</td></tr>";

                }
                else
                {
                    strHtml += "<tr><td align=left style=\"width:30%;\"><u>Net Bill Amount :</u></td><td style=\"width:10%;\"> &nbsp;</td><td align=\"right\" style=\"width:10%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(totalobligation - totalcharges))) + "</td></tr>";

                }

                DataTable dtfund = new DataTable();
                dtfund = oDBEngine.NetFundAdjustment("'SYSTM00001'", CLValue, Session["usersegid"].ToString(), Session["LastCompany"].ToString(), Convert.ToDateTime(dtfrom.Value), Convert.ToDateTime(dtto.Value));

                if (dtfund.Rows.Count > 0)
                {
                    netfund = Convert.ToDecimal(dtfund.Rows[0][0].ToString());
                    if (netfund < 0)
                    {
                        strHtml += "<tr><td align=left style=\"width:30%;\">Net Fund Adjustment For the Period :</td><td align=\"right\" style=\"width:13%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(netfund))) + "</td><td style=\"width:7%;\"> &nbsp;</td></tr>";

                    }
                    else
                    {
                        strHtml += "<tr><td align=left style=\"width:30%;\">Net Fund Adjustment For the Period :</td><td style=\"width:10%;\"> &nbsp;</td><td align=\"right\" style=\"width:10%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(netfund))) + "</td></tr>";
                    }
                }
                netamount = totalobligation - totalcharges + (openingbalance + netfund);

                if (netamount < 0)
                {
                    strHtml += "<tr><td align=left style=\"width:30%;\">Net Amount Receivable By Us: </td><td align=\"right\" style=\"width:13%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(netamount))) + "</td><td style=\"width:7%;\"> &nbsp;</td></tr>";

                }
                else
                {
                    strHtml += "<tr><td align=left style=\"width:30%;\">Net Amount Payable To You :</td><td style=\"width:10%;\"> &nbsp;</td><td align=\"right\" style=\"width:10%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs(Convert.ToDecimal(netamount))) + "</td></tr>";
                }
                strHtml += "</table></div></td></tr><tr style=\"height:1px;background-color:White;\"><td colspan=11>&nbsp;</td></tr>";
                ///////////////////////////////END///////////////////////////////////////
                ////////////////////////////////Margin obligation///////////////////////
                if (ds.Tables[7].Rows.Count != 0)
                {

                    strHtml += "<tr style=\"background-color: #FFD4AA; color: Black;\">";
                    strHtml += "<td align=\"center\" ><b>Margin Summary</b></td>";
                    strHtml += "<td align=\"center\" ><b>Span Margin</b></td>";
                    strHtml += "<td align=\"center\"><b>Premium Margin</b></td>";
                    strHtml += "<td align=\"center\"><b>Total Margin</b></td>";
                    strHtml += "<td align=\"center\"><b>Exposure Margin</b></td>";
                    strHtml += "<td align=\"center\"><b>Applicable Margin</b></td>";
                    strHtml += "<td align=\"center\"><b>Cash Dep</b></td>";
                    strHtml += "<td align=\"center\"><b><b>Collaterals</b></td>";
                    strHtml += "<td align=\"center\"><b>Total Deposit</b></td>";
                    strHtml += "<td align=\"center\"><b>Shortage</b></td>";
                    strHtml += "<td align=\"center\"><b>Excess</b></td></tr>";

                    strHtml += "<tr style=\"background-color: white;\"><td> &nbsp;</td>";
                    strHtml += "<td  align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[7].Rows[0]["SpanMargin"])) + "</td>";
                    strHtml += "<td  align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[7].Rows[0]["BuyPremium"])) + "</td>";
                    strHtml += "<td  align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[7].Rows[0]["TotalMargin"])) + "</td>";
                    strHtml += "<td  align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[7].Rows[0]["ExposureMargin"])) + "</td>";
                    strHtml += "<td  align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[7].Rows[0]["ApplicableMargin"])) + "</td>";

                    DataTable dtmargin = new DataTable();
                    dtmargin = oDBEngine.OpeningBalanceOnlyJournal("'SYSTM00002','SYSTM00003'", CLValue, Convert.ToDateTime(dtfrom.Value), Session["usersegid"].ToString(), Session["LastCompany"].ToString(), Convert.ToDateTime(dtto.Value));
                    if (dtmargin.Rows.Count > 0)
                    {
                        strHtml += "<td  align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtmargin.Rows[0][0].ToString())) + "</td>";
                        totaldeposit = Convert.ToDecimal(dtmargin.Rows[0][0].ToString()) + Convert.ToDecimal(ds.Tables[7].Rows[0]["coleteral"]);

                    }
                    else
                    {
                        strHtml += "<td> &nbsp;</td>";
                    }
                    strHtml += "<td  align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[7].Rows[0]["coleteral"])) + "</td>";
                    strHtml += "<td  align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(totaldeposit)) + "</td>";

                    stortageexcess = totaldeposit - Convert.ToDecimal(ds.Tables[7].Rows[0]["ApplicableMargin"]);
                    if (stortageexcess < 0)
                    {
                        strHtml += "<td  align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(Math.Abs(stortageexcess))) + "</td>";
                        strHtml += "<td align=\"right\"></td></tr>";
                    }
                    else
                    {
                        strHtml += "<td  align=\"right\"></td>";
                        strHtml += "<td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(stortageexcess)) + "</td></tr>";
                    }

                }

                ////////////////////////////////END////////////////////////////////////
                ////////////////////////////////////Net Amount After Adjusting Margin Shortage :///////////////////////////

                strHtml += "<tr><td style=\"height:10px;background-color:White;\" colspan=11></td></tr><tr style=\"background-color:White;\">";
                strHtml += "<td align=right colspan=11><div style=\"border: solid 1px balck;\">";
                strHtml += "<table width=\"950px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"1\"><tr>";

                if (stortageexcess < 0)
                {
                    if (netamount + stortageexcess > 0)
                    {
                        strHtml += "<td align=\"left\" style=\"width:35%;\">Net Amount Payable  After Adjusting Margin Shortage :</td>";
                        strHtml += "<td align=\"right\" style=\"width:15%;\"><b>&nbsp;" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs((netamount + stortageexcess))) + "</b></td>";

                    }
                    else
                    {
                        strHtml += "<td align=\"left\" style=\"width:35%;\">Net Amount Receivable By Us After Adjusting Margin Shortage :</td>";
                        strHtml += "<td align=\"right\" style=\"width:15%;\"><b>&nbsp;" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs((netamount + stortageexcess))) + "</b></td>";

                    }
                }
                else
                {
                    if (netamount > 0)
                    {
                        strHtml += "<td align=\"left\" style=\"width:35%;\">Net Amount Payable To You After Adjusting Margin Shortage :</td>";
                        strHtml += "<td style=\"width:10%;\">&nbsp;</td><td align=\"right\" style=\"width:10%;\"><b>&nbsp;" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs((netamount))) + "</b></td>";

                    }
                    else
                    {
                        strHtml += "<td align=\"left\" style=\"width:35%;\">Net Amount Receivable BY Us After Adjusting Margin Shortage :</td>";
                        strHtml += "<td align=\"right\" style=\"width:15%;\"><b>&nbsp;" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Math.Abs((netamount))) + "</b></td>";

                    }
                }
                strHtml += "</tr></table></div></td></tr>";


                strHtml += "</table>";

                // display.InnerHtml = strHtml;

                SEmailTbl = strHtml;
                ds.Dispose();
                /////////////////////////////DISPLAY DATE/////////////////////////////////////
                string group = null;
                if (ddlGroup.SelectedItem.Value.ToString() == "0")
                {
                    group = ddlGroup.SelectedItem.Text.ToString().Trim() + "Wise Report";
                }
                else
                {
                    group = ddlGroup.SelectedItem.Text + "Wise [ " + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "] Report";
                }
                if (ddldate.SelectedItem.Value == "0")
                {

                    string SpanText = group + " For" + oconverter.ArrangeDate2(dtfor.Value.ToString());
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct", "displaydate('" + SpanText + "')", true);

                }
                else
                {
                    string SpanText = group + " Period" + oconverter.ArrangeDate2(dtfrom.Value.ToString()) + " " + "to" + " " + oconverter.ArrangeDate2(dtto.Value.ToString());
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct", "displaydate('" + SpanText + "')", true);

                }

                ///////////////////////////////END//////////////////////////////////////////
                ScriptManager.RegisterStartupScript(this, this.GetType(), "displaygrid", "displayresult();", true);



            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "norecord", "norecord(1);", true);

            }

        }

        void procedureEmail(string CLValue)
        {

            string fromdate = string.Empty;
            string todate = string.Empty;

            if (ddldate.SelectedItem.Value == "0")
            {
                fromdate = Convert.ToString(dtfor.Value);
                todate = "NA";
            }
            else
            {
                fromdate = Convert.ToString(dtfrom.Value);
                todate = Convert.ToString(dtto.Value);
            }

            ds = objFAReportsOther.Sp_ObligationStatementFO1(
          Convert.ToString(fromdate),
          Convert.ToString(todate),
          Convert.ToString(Session["usersegid"].ToString()),
           Convert.ToString(Session["LastCompany"]),
           Convert.ToString(HttpContext.Current.Session["ExchangeSegmentID"]),
            Convert.ToString(CLValue),
              Convert.ToString(HttpContext.Current.Session["LastFinYear"])
           );
            if (ddldate.SelectedItem.Value == "0")
            {
                // bindTableFORADATE();

                bindTableFORADATEEmail();
            }
            else
            {
                //bindTableFORAPeriod();
                bindTableFORAPeriodEmail();
            }

        }

        protected void NavigationLinkC_Click(Object sender, CommandEventArgs e)
        {
            int curentIndex = cmbgroup.SelectedIndex;
            int totalNo = cmbgroup.Items.Count;
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
                    pageindex = int.Parse(Totalgroup.Value);
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
            cmbgroup.SelectedIndex = curentIndex;
            clientdisplay();

        }


        protected void BTNLODINGDDLGROUP_Click(object sender, EventArgs e)
        {
            clientdisplay();
        }

        void procedureForLedger()
        {

            ds = objFAReportsOther.Sp_ObligationStatementFO1(
          Convert.ToString(Request.QueryString["date"]),
         "NA",
          Convert.ToString(Request.QueryString["SegID"]),
           Convert.ToString(Request.QueryString["Compid"]),
          "2",
            Convert.ToString(Request.QueryString["ClientID"]),
              Convert.ToString(HttpContext.Current.Session["LastFinYear"])
           );
            bindTableFORADATE();

        }
    }
}