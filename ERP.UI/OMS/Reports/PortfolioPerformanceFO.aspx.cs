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
    public partial class Reports_PortfolioPerformanceFO : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        DailyReports dailyrep = new DailyReports();
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        DataSet ds = new DataSet();
        DataTable dtgroupcontactid = new DataTable();
        DataTable Distinctclient = new DataTable();
        ExcelFile objExcel = new ExcelFile();
        DataTable dtExport = new DataTable();
        DataTable dtReportHeader = new DataTable();
        DataTable dtReportFooter = new DataTable();
        string data;
        int pageindex = 0;
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
                if (idlist[0] != "Clients" && idlist[0] != "Expiry")
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
                //else
                //{

                //    string[] val = cl[i].Split(';');
                //    string[] AcVal = val[0].Split('-');
                //    if (str == "")
                //    {

                //            str = "'" + AcVal[0] + "'";
                //            str1 = "'" + AcVal[0] + "'" + ";" + val[1];
                //    }
                //    else
                //    {

                //            str += ",'" + AcVal[0] + "'";
                //            str1 += "," + "'" + AcVal[0] + "'" + ";" + val[1];
                //    }
                //}
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

            if (idlist[0] == "Clients")
            {
                data = "Clients~" + str;
            }
            else if (idlist[0] == "Expiry")
            {
                data = "Expiry~" + str;
            }
            else if (idlist[0] == "Product")
            {
                data = "Product~" + str;
            }
            else if (idlist[0] == "Group")
            {
                data = "Group~" + str;
            }
            else if (idlist[0] == "Branch")
            {
                data = "Branch~" + str;
            }
            else if (idlist[0] == "MAILEMPLOYEE")
            {
                data = "MAILEMPLOYEE~" + str;
            }

        }
        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }
        void date()
        {
            dtfor.EditFormatString = oconverter.GetDateFormat("Date");
            dtfrom.EditFormatString = oconverter.GetDateFormat("Date");
            dtto.EditFormatString = oconverter.GetDateFormat("Date");
            DataTable dtexpiryeffectuntil = new DataTable();
            dtexpiryeffectuntil = oDBEngine.GetDataTable("master_equity", " DISTINCT top 2 equity_effectuntil", "  month(equity_effectuntil)<=month(getdate()) and year(equity_effectuntil)=year(getdate())", " equity_effectuntil desc");
            if (dtexpiryeffectuntil.Rows.Count == 2)
            {
                dtfrom.Value = Convert.ToDateTime(new DateTime(Convert.ToDateTime(dtexpiryeffectuntil.Rows[1][0].ToString()).Year, Convert.ToDateTime(dtexpiryeffectuntil.Rows[1][0].ToString()).Month, Convert.ToDateTime(dtexpiryeffectuntil.Rows[1][0].ToString()).Day).AddDays(1).ToString());
                dtto.Value = Convert.ToDateTime(dtexpiryeffectuntil.Rows[0][0].ToString());
            }
            else
            {
                dtfrom.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
                dtto.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
            }
            string[] idlist = oDBEngine.GetDate().GetDateTimeFormats();
            dtfor.Value = Convert.ToDateTime(idlist[2]);

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
        void fn_Client()
        {

            string Clients = null;
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
                                    if (Clients == null)
                                        Clients = "'" + dtclient.Rows[i][0].ToString() + "'";
                                    else
                                        Clients += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
                                }

                            }

                        }
                    }
                    else
                    {
                        DataTable dtclient = new DataTable();
                        dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%' and cnt_internalid in(select grp_contactid from tbl_trans_group where grp_groupmaster in(" + HiddenField_Group.Value + "))");
                        if (dtclient.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtclient.Rows.Count; i++)
                            {
                                if (Clients == null)
                                    Clients = "'" + dtclient.Rows[i][0].ToString() + "'";
                                else
                                    Clients += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
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
                                if (Clients == null)
                                    Clients = "'" + dtclient.Rows[i][0].ToString() + "'";
                                else
                                    Clients += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
                            }

                        }
                    }
                    else
                    {
                        DataTable dtclient = new DataTable();
                        dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%' and cnt_branchid in(" + HiddenField_Branch.Value + ")");
                        if (dtclient.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtclient.Rows.Count; i++)
                            {
                                if (Clients == null)
                                    Clients = "'" + dtclient.Rows[i][0].ToString() + "'";
                                else
                                    Clients += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
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
                                    if (Clients == null)
                                        Clients = "'" + dtclient.Rows[i][0].ToString() + "'";
                                    else
                                        Clients += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
                                }

                            }

                        }
                    }
                    else
                    {
                        DataTable dtclient = new DataTable();
                        dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%' and cnt_internalid in (select dpd_cntID from tbl_master_contactdpdetails where dpd_POA=1) and cnt_internalid in(select grp_contactid from tbl_trans_group where grp_groupmaster in(" + HiddenField_Group.Value + "))");
                        if (dtclient.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtclient.Rows.Count; i++)
                            {
                                if (Clients == null)
                                    Clients = "'" + dtclient.Rows[i][0].ToString() + "'";
                                else
                                    Clients += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
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
                                if (Clients == null)
                                    Clients = "'" + dtclient.Rows[i][0].ToString() + "'";
                                else
                                    Clients += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
                            }

                        }
                    }
                    else
                    {
                        DataTable dtclient = new DataTable();
                        dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%' and cnt_internalid in (select dpd_cntID from tbl_master_contactdpdetails where dpd_POA=1) and cnt_branchid in(" + HiddenField_Branch.Value + ")");
                        if (dtclient.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtclient.Rows.Count; i++)
                            {
                                if (Clients == null)
                                    Clients = "'" + dtclient.Rows[i][0].ToString() + "'";
                                else
                                    Clients += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
                            }

                        }
                    }
                }
            }
            if (Clients != null)
            {
                HiddenField_Client.Value = Clients;
            }
        }
        void fn_segment()
        {

            DataTable DtSeg = oDBEngine.GetDataTable("tbl_master_companyexchange", "EXCH_INTERNALID", "exch_segmentid like 'CM%'", null);
            if (DtSeg.Rows.Count > 0)
            {
                for (int i = 0; i < DtSeg.Rows.Count; i++)
                {
                    ViewState["segment"] += "," + DtSeg.Rows[i][0].ToString();
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
            procedure();

        }
        void htmlbind(string result)
        {
            if (result == "YES")
            {
                ds = (DataSet)ViewState["dataset"];
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ddlrptview.SelectedItem.Value.ToString() == "0")
                    {
                        ddlbandforgroup();
                        if (ddlrpttype.SelectedItem.Value.ToString().Trim() == "0")
                        {
                            CurrentPage = 0;
                            ddlbandforClient();
                        }
                        else
                        {
                            htmltable_rpttypeclientwise_summary();
                        }
                    }
                    else
                    {
                        ddlbandforasset();
                        if (ddlrpttype.SelectedItem.Value.ToString().Trim() == "0")
                        {
                            CurrentPage = 0;
                            ddlbandforScrip();
                        }
                        else
                        {
                            htmltable_rpttypescripwise_summary();
                        }
                    }

                }
            }
            if (result == "NO")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(1);", true);

            }
            if (result == "SEECTION")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(2);", true);

            }
        }
        void procedure()
        {
            fn_Client();
            ////////////////Fetch Instrument
            string chkcalbasis = null;
            ViewState["segment"] = Convert.ToInt32(Session["usersegid"].ToString());
            foreach (ListItem listitem in chkinstrutype.Items)
            {
                if (listitem.Selected)
                {
                    if (listitem.Value == "FUT")
                    {
                        if (chkcalbasis == null)
                            chkcalbasis = "(EQUITY_FOIDENTIFIER LIKE " + " '" + listitem.Value + "%" + "'";
                        else
                            chkcalbasis += " or " + "EQUITY_FOIDENTIFIER LIKE " + "'" + listitem.Value + "%" + "'";


                    }
                    else if (listitem.Value == "C" || listitem.Value == "P")
                    {
                        if (chkcalbasis == null)
                            chkcalbasis = "((EQUITY_FOIDENTIFIER LIKE 'OPT%' AND EQUITY_SERIES LIKE " + " '" + listitem.Value + "%" + "')";
                        else
                            chkcalbasis += " or " + "(EQUITY_FOIDENTIFIER LIKE 'OPT%' AND EQUITY_SERIES LIKE " + " '" + listitem.Value + "%" + "')";


                    }
                    else
                    {
                        if (chkcalbasis == null)
                            chkcalbasis = "(EQUITY_FOIDENTIFIER is null";
                        else
                            chkcalbasis += " or " + "EQUITY_FOIDENTIFIER is null ";

                        fn_segment();
                    }
                }
            }
            if (chkcalbasis != null)
            {
                chkcalbasis += " )";


                string chknetclients = "";
                if (rdbnetclientboth.Checked)
                {
                    chknetclients = "BOTH";
                }
                if (rdbnetclientrecivabel.Checked)
                {
                    chknetclients = "RECEIVE";
                }
                if (rdbnetclientpayabel.Checked)
                {
                    chknetclients = "PAYBLE";
                }

                ds = dailyrep.PerformanceReportFO(Session["LastCompany"].ToString(), ViewState["segment"].ToString(), ddldate.SelectedItem.Value.ToString() == "0" ? "NA" : dtfrom.Value.ToString(),
                    ddldate.SelectedItem.Value.ToString() == "0" ? dtfor.Value.ToString() : dtto.Value.ToString(), HiddenField_Client.Value, HttpContext.Current.Session["ExchangeSegmentID"].ToString(),
                    rdbunderlyingall.Checked ? "ALL" : HiddenField_Product.Value, rdbExpiryAll.Checked ? "ALL" : HiddenField_Expiry.Value,
                    ddlGroup.SelectedItem.Value.ToString() == "0" ? "BRANCH" : ddlgrouptype.SelectedItem.Text.ToString().Trim(), ddlmtmcalbasis.SelectedItem.Value.ToString().Trim(),
                    ddlrpttype.SelectedItem.Value.ToString().Trim(), chkcalbasis, rbPrint.Checked ? "PRINT" : "SHOW", ChkDISTRIBUTION.Checked ? "CHK" : "UNCHK",
                    ChkBFQty.Checked ? "CHK" : "UNCHK", chkopen.Checked ? "CHK" : "UNCHK", chkopenbfpositive.Checked ? "CHK" : "UNCHK", chkclosepricezero.Checked ? "true" : "false",
                    chknetclients, rdbTerminalAll.Checked ? "ALL" : txtTerminal_hidden.Text.ToString().Trim(), ChkBFPositionValue.Checked ? "CHK" : "UNCHK",
                    ChkConsolidatedSpot.Checked ? "CHK" : "UNCHK", chknetpremium.Checked ? "CHK" : "UNCHK", ddlrptview.SelectedItem.Value.ToString().Trim());

                ViewState["dataset"] = ds;
                if (ds.Tables[0].Rows.Count == 0)
                {
                    htmlbind("NO");
                }
                else
                {
                    if (rbScreen.Checked)
                    {
                        htmlbind("YES");
                    }

                }
            }
            else
            {
                htmlbind("SEECTION");
            }

        }
        void ddlbandforgroup()
        {
            ds = (DataSet)ViewState["dataset"];
            DataView viewgroup = new DataView(ds.Tables[0]);
            dtgroupcontactid = viewgroup.ToTable(true, new string[] { "GRPID", "GRPNAME" });
            if (dtgroupcontactid.Rows.Count > 0)
            {
                cmbgroup.DataSource = dtgroupcontactid;
                cmbgroup.DataValueField = "GRPID";
                cmbgroup.DataTextField = "GRPNAME";
                cmbgroup.DataBind();

            }

        }
        void ddlbandforasset()
        {
            ds = (DataSet)ViewState["dataset"];
            DataView viewgroup = new DataView(ds.Tables[0]);
            dtgroupcontactid = viewgroup.ToTable(true, new string[] { "MASTERPRODUCTID", "ASSETNAME" });
            if (dtgroupcontactid.Rows.Count > 0)
            {
                cmbgroup.DataSource = dtgroupcontactid;
                cmbgroup.DataValueField = "MASTERPRODUCTID";
                cmbgroup.DataTextField = "ASSETNAME";
                cmbgroup.DataBind();

            }

        }
        void ddlbandforClient()
        {
            ds = (DataSet)ViewState["dataset"];
            DataView viewgrp = new DataView();
            viewgrp = ds.Tables[0].DefaultView;
            viewgrp.RowFilter = "GRPID='" + cmbgroup.SelectedItem.Value + "'";
            DataTable dt = new DataTable();
            dt = viewgrp.ToTable();

            DataView viewClient = new DataView(dt);
            Distinctclient = viewClient.ToTable(true, new string[] { "CUSTOMERID", "CLIENTNAME" });

            if (Distinctclient.Rows.Count > 0)
            {
                cmbclient.DataSource = Distinctclient;
                cmbclient.DataValueField = "CUSTOMERID";
                cmbclient.DataTextField = "CLIENTNAME";
                cmbclient.DataBind();

            }
            ViewState["clients"] = Distinctclient;
            LastPage = Distinctclient.Rows.Count - 1;
            CurrentPage = 0;
            bind_Details();
        }
        void ddlbandforScrip()
        {
            ds = (DataSet)ViewState["dataset"];
            DataView viewgrp = new DataView();
            viewgrp = ds.Tables[0].DefaultView;
            viewgrp.RowFilter = "MASTERPRODUCTID='" + cmbgroup.SelectedItem.Value + "'";
            DataTable dt = new DataTable();
            dt = viewgrp.ToTable();

            DataView viewClient = new DataView(dt);
            Distinctclient = viewClient.ToTable(true, new string[] { "PRODUCTID", "SCRIPNAME" });

            if (Distinctclient.Rows.Count > 0)
            {
                cmbclient.DataSource = Distinctclient;
                cmbclient.DataValueField = "PRODUCTID";
                cmbclient.DataTextField = "SCRIPNAME";
                cmbclient.DataBind();

            }
            ViewState["clients"] = Distinctclient;
            LastPage = Distinctclient.Rows.Count - 1;
            CurrentPage = 0;
            bind_Details();
        }
        void bind_Details()
        {
            Distinctclient = (DataTable)ViewState["clients"];
            cmbclient.SelectedIndex = CurrentPage;
            if (LastPage > -1)
            {
                listRecord.Text = CurrentPage + 1 + " of " + Distinctclient.Rows.Count.ToString() + " Record.";

            }
            if (rbmail.Checked == false)
            {
                if (ddlrptview.SelectedItem.Value.ToString().Trim() == "0")
                {
                    htmltable_rpttypecleintwise_detail(cmbclient.SelectedItem.Value.ToString(), cmbclient.SelectedItem.Text.ToString().Trim(), cmbgroup.SelectedItem.Value.ToString(), cmbgroup.SelectedItem.Text.ToString().Trim());
                }
                else
                {
                    htmltable_rpttypescripwise_detail(cmbclient.SelectedItem.Value.ToString(), cmbclient.SelectedItem.Text.ToString().Trim(), cmbgroup.SelectedItem.Value.ToString(), cmbgroup.SelectedItem.Text.ToString().Trim());
                }
            }
            else
            {
                htmltable_rpttypecleintwise_detail1(cmbclient.SelectedItem.Value.ToString(), cmbclient.SelectedItem.Text.ToString().Trim(), cmbgroup.SelectedItem.Value.ToString(), cmbgroup.SelectedItem.Text.ToString().Trim());
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Display", "Display();", true);
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
        void htmltable_rpttypecleintwise_detail(string clientid, string clientname, string grpid, string grpname)
        {
            ds = (DataSet)ViewState["dataset"];
            String strHtml = String.Empty;
            String strHtml1 = String.Empty;
            int colcount = ds.Tables[0].Columns.Count;
            string str = null;
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                str = "[" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "]";
            }

            if (ddldate.SelectedItem.Value.ToString() == "0")
            {
                str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report For " + oconverter.ArrangeDate2(dtfor.Value.ToString());
            }
            else
            {
                str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report Period " + oconverter.ArrangeDate2(dtfrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtto.Value.ToString());
            }
            strHtml1 = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtml1 += "<tr><td align=\"left\" colspan=" + colcount + " style=\"color:Blue;\">" + str + "</td></tr></table>";

            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

            DataView viewclient = new DataView();
            viewclient = ds.Tables[0].DefaultView;
            viewclient.RowFilter = "CUSTOMERID='" + cmbclient.SelectedItem.Value + "'";
            DataTable dt1 = new DataTable();
            dt1 = viewclient.ToTable();


            strHtml += "<tr style=\"background-color:lavender ;text-align:left\">";
            strHtml += "<td align=\"left\" colspan=" + colcount + " nowrap=\"nowrap;\">Client Name :&nbsp;<b>" + cmbclient.SelectedItem.Text.ToString().Trim() + "</b>[ <b>" + dt1.Rows[0]["UCC"].ToString() + " </b> ]</td></tr>";

            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Asset</b></td>";
            strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Instr. </br> Type</b></td>";
            strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Expiry </br> Date</b></td>";
            strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Strike </br> Price</b></td>";
            if (ddldate.SelectedItem.Value.ToString() == "1")
            {
                if (ChkBFQty.Checked == false)
                {
                    strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>B/F Qty</b></td>";
                }
            }
            strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Buy </br> Qty</b></td>";
            strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Sell </br> Qty</b></td>";
            strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Booked </br> P/L</b></td>";
            if (chknetpremium.Checked)
            {
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Premium</b></td>";
            }
            strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>C/F  Qty</b></td>";
            strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Avg. </br> Cost </br> (Residual)</b></td>";
            if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
            {
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"> <b>Close </br> Price </br> (Instrmnt)</b></td>";
            }
            else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
            {
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Close </br> Price </br> (Asset)</b></td>";
            }
            else
            {
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Close </br> Price </br> (Instrmnt)</b></td>";
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Close </br> Price </br> (Asset)</b></td>";
            }
            if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
            {
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>MTM </br>(Instr.</br> Close)</b></td>";
            }
            else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
            {
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>MTM </br>(Asset. </br> Close)</b></td>";
            }
            else
            {
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>MTM </br>(Instr.</br> Close)</b></td>";
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>MTM </br>(Asset.</br> Close)</b></td>";
            }
            if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
            {
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Total </br> P/L </br> (Instr.</br> Close)</b></td>";
            }
            else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
            {
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Total </br> P/L </br> (Asset.</br> Close)</b></td>";
            }
            else
            {
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Total </br> P/L </br> (Instr.</br> Close)</b></td>";
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Total </br> P/L </br> (Asset.</br> Close)</b></td>";
            }
            strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Exposure</b></td>";
            strHtml += "</tr>";
            int flag = 0;

            string MASTERPRODUCT = null;
            int i;
            for (i = 0; i < dt1.Rows.Count; i++)
            {
                if (MASTERPRODUCT != null)
                {
                    if (MASTERPRODUCT != dt1.Rows[i]["MASTERPRODUCTID"].ToString())
                    {
                        ///////////////////////MASTERPRODUCT TOTAL

                        flag = flag + 1;
                        strHtml += "<tr style=\"background-color:lavender;text-align:center\">";
                        strHtml += "<td align=\"left\" colspan=4 nowrap=\"nowrap;\">&nbsp;</td>";
                        if (ddldate.SelectedItem.Value.ToString() == "1")
                        {
                            if (ChkBFQty.Checked == false)
                            {
                                if (dt1.Rows[i - 1]["MASTERPRODUCTID_BFQTY"] != DBNull.Value)
                                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"B/F Qty\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_BFQTY"].ToString() + "</td>";
                                else
                                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                            }
                        }
                        if (dt1.Rows[i - 1]["MASTERPRODUCTID_BUYQTY"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Buy Qty\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_BUYQTY"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                        if (dt1.Rows[i - 1]["MASTERPRODUCTID_SELLQTY"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Sell Qty\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_SELLQTY"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                        if (dt1.Rows[i - 1]["MASTERPRODUCTID_BOOKEDPL"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Booked P/L\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_BOOKEDPL"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                        if (chknetpremium.Checked)
                        {
                            if (dt1.Rows[i - 1]["MASTERPRODUCTID_Premium"] != DBNull.Value)
                                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Premium\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_Premium"].ToString() + "</td>";
                            else
                                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                        }

                        if (dt1.Rows[i - 1]["MASTERPRODUCTID_CFQTY"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"C/F Qty\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_CFQTY"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                        strHtml += "<td align=\"left\" colspan=1 nowrap=\"nowrap;\">&nbsp;</td>";

                        if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
                        {
                            strHtml += "<td align=\"left\" >&nbsp;</td>";
                            if (dt1.Rows[i - 1]["MASTERPRODUCTID_MTMINSTRUCLOSE"] != DBNull.Value)
                                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Instr.</br> Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_MTMINSTRUCLOSE"].ToString() + "</td>";
                            else
                                strHtml += "<td align=\"right\" nowrap=\"nowrap;\">&nbsp;</td>";

                        }
                        else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
                        {
                            strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                            if (dt1.Rows[i - 1]["MASTERPRODUCTID_MTMASSETCLOSE"] != DBNull.Value)
                                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Asset.</br> Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_MTMASSETCLOSE"].ToString() + "</td>";
                            else
                                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";


                        }
                        else
                        {
                            strHtml += "<td align=\"left\" colspan=2>&nbsp;</td>";
                            if (dt1.Rows[i - 1]["MASTERPRODUCTID_MTMINSTRUCLOSE"] != DBNull.Value)
                                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Instr.</br> Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_MTMINSTRUCLOSE"].ToString() + "</td>";
                            else
                                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                            if (dt1.Rows[i - 1]["MASTERPRODUCTID_MTMASSETCLOSE"] != DBNull.Value)
                                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Asset.</br> Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_MTMASSETCLOSE"].ToString() + "</td>";
                            else
                                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";


                        }
                        if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
                        {

                            if (dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLINSTRUCLOSE"] != DBNull.Value)
                                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L  (Instr.Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLINSTRUCLOSE"].ToString() + "</td>";
                            else
                                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                        }
                        else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
                        {

                            if (dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLASSETCLOSE"] != DBNull.Value)
                                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L  (Asset.Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLASSETCLOSE"].ToString() + "</td>";
                            else
                                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                        }
                        else
                        {

                            if (dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLINSTRUCLOSE"] != DBNull.Value)
                                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L  (Instr.Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLINSTRUCLOSE"].ToString() + "</td>";
                            else
                                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                            if (dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLASSETCLOSE"] != DBNull.Value)
                                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L  (Asset.Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLASSETCLOSE"].ToString() + "</td>";
                            else
                                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                        }
                        if (dt1.Rows[i - 1]["MASTERPRODUCTID_EXPOSURE"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\">Exposure\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_EXPOSURE"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                        strHtml += "</tr>";
                        ///////////////////////////////////////////MASTERPRODUCT TOTAL END
                    }
                }
                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";

                if (MASTERPRODUCT != dt1.Rows[i]["MASTERPRODUCTID"].ToString())
                {
                    if (dt1.Rows[i]["TICKERSYMBOL"] != DBNull.Value)
                        strHtml += "<td align=\"left\" style=\"font-size:xx-small;color:#348017\" nowrap=\"nowrap;\">" + dt1.Rows[i]["TICKERSYMBOL"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }
                else
                {
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }

                MASTERPRODUCT = dt1.Rows[i]["MASTERPRODUCTID"].ToString();

                if (dt1.Rows[i]["SERIES"] != DBNull.Value)
                {
                    if (dt1.Rows[i]["SERIES"].ToString().Trim().StartsWith("S"))
                    {
                        strHtml += "<td align=\"left\" colspan=3 nowrap=\"nowrap;\" style=\"width:8%;font-size:xx-small;\" title=\"Instr. Type For " + cmbclient.SelectedItem.Text.ToString().Trim() + " [ " + dt1.Rows[i]["UCC"].ToString() + " ]\">" + dt1.Rows[i]["SERIES"].ToString() + "</td>";
                    }
                    else
                    {
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\" style=\"width:8%;font-size:xx-small;\" title=\"Instr. Type For " + cmbclient.SelectedItem.Text.ToString().Trim() + " [ " + dt1.Rows[i]["UCC"].ToString() + " ]\">" + dt1.Rows[i]["SERIES"].ToString() + "</td>";
                        if (dt1.Rows[i]["EXPIRY"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[i]["EXPIRY"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" >&nbsp;</td>";

                        if (dt1.Rows[i]["STRIKEPRICE"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Strike Price\" nowrap=\"nowrap;\">" + dt1.Rows[i]["STRIKEPRICE"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                    }
                }
                else
                {
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                    if (dt1.Rows[i]["EXPIRY"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[i]["EXPIRY"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";

                    if (dt1.Rows[i]["STRIKEPRICE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Strike Price\" nowrap=\"nowrap;\">" + dt1.Rows[i]["STRIKEPRICE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }

                if (ddldate.SelectedItem.Value.ToString() == "1")
                {
                    if (ChkBFQty.Checked == false)
                    {
                        if (dt1.Rows[i]["BFQTY"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"B/F Qty\" nowrap=\"nowrap;\">" + dt1.Rows[i]["BFQTY"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                    }
                }
                if (dt1.Rows[i]["BUYQTY"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Buy Qty\" nowrap=\"nowrap;\">" + dt1.Rows[i]["BUYQTY"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                if (dt1.Rows[i]["SELLQTY"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Sell Qty\" nowrap=\"nowrap;\">" + dt1.Rows[i]["SELLQTY"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                if (dt1.Rows[i]["BOOKEDPL"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Booked P/L\" nowrap=\"nowrap;\">" + dt1.Rows[i]["BOOKEDPL"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                if (chknetpremium.Checked)
                {
                    if (dt1.Rows[i]["Premium"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Premium\" nowrap=\"nowrap;\">" + dt1.Rows[i]["Premium"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                }

                if (dt1.Rows[i]["CFQTY"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"C/F  Qty\">" + dt1.Rows[i]["CFQTY"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (dt1.Rows[i]["AVGCOSTRESEDUAL"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Avg. Cost (Residual)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["AVGCOSTRESEDUAL"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
                {
                    if (dt1.Rows[i]["CLOSEPRICEINSTRU"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Close Price (Instrmnt)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["CLOSEPRICEINSTRU"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }
                else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
                {
                    if (dt1.Rows[i]["CLOSEPRICEASSET"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Close Price (Asset)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["CLOSEPRICEASSET"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }
                else
                {
                    if (dt1.Rows[i]["CLOSEPRICEINSTRU"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Close Price (Instrmnt)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["CLOSEPRICEINSTRU"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                    if (dt1.Rows[i]["CLOSEPRICEASSET"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Close Price (Asset)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["CLOSEPRICEASSET"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }

                if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
                {
                    if (dt1.Rows[i]["MTMINSTRUCLOSE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Instr. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["MTMINSTRUCLOSE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }
                else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
                {
                    if (dt1.Rows[i]["MTMASSETCLOSE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Asset. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["MTMASSETCLOSE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }
                else
                {
                    if (dt1.Rows[i]["MTMINSTRUCLOSE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Instr. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["MTMINSTRUCLOSE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                    if (dt1.Rows[i]["MTMASSETCLOSE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Asset. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["MTMASSETCLOSE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }


                if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
                {
                    if (dt1.Rows[i]["TOTALPLINSTRUCLOSE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L (Instr..Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["TOTALPLINSTRUCLOSE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";
                }
                else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
                {
                    if (dt1.Rows[i]["TOTALPLASSETCLOSE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L (Asset..Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["TOTALPLASSETCLOSE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }
                else
                {
                    if (dt1.Rows[i]["TOTALPLINSTRUCLOSE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L (Instr.Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["TOTALPLINSTRUCLOSE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                    if (dt1.Rows[i]["TOTALPLASSETCLOSE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L (Asset.Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["TOTALPLASSETCLOSE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }

                if (dt1.Rows[i]["Exposure"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Exposure\">" + dt1.Rows[i]["EXPOSURE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                strHtml += "</tr>";
            }
            ///////////////////////MASTERPRODUCT TOTAL

            flag = flag + 1;
            strHtml += "<tr style=\"background-color:lavender;text-align:center\">";
            strHtml += "<td align=\"left\" colspan=4 nowrap=\"nowrap;\">&nbsp;</td>";
            if (ddldate.SelectedItem.Value.ToString() == "1")
            {
                if (ChkBFQty.Checked == false)
                {
                    if (dt1.Rows[i - 1]["MASTERPRODUCTID_BFQTY"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"B/F Qty\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_BFQTY"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                }
            }
            if (dt1.Rows[i - 1]["MASTERPRODUCTID_BUYQTY"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Buy Qty\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_BUYQTY"].ToString() + "</td>";
            else
                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

            if (dt1.Rows[i - 1]["MASTERPRODUCTID_SELLQTY"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Sell Qty\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_SELLQTY"].ToString() + "</td>";
            else
                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

            if (dt1.Rows[i - 1]["MASTERPRODUCTID_BOOKEDPL"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Booked P/L\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_BOOKEDPL"].ToString() + "</td>";
            else
                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

            if (chknetpremium.Checked)
            {

                if (dt1.Rows[i - 1]["MASTERPRODUCTID_Premium"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Premium\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_Premium"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

            }

            if (dt1.Rows[i - 1]["MASTERPRODUCTID_CFQTY"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"C/F  Qty\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_CFQTY"].ToString() + "</td>";
            else
                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

            strHtml += "<td align=\"left\" colspan=1 nowrap=\"nowrap;\">&nbsp;</td>";

            if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
            {
                strHtml += "<td align=\"left\" >&nbsp;</td>";
                if (dt1.Rows[i - 1]["MASTERPRODUCTID_MTMINSTRUCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Instr. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_MTMINSTRUCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
            {
                strHtml += "<td align=\"left\" >&nbsp;</td>";
                if (dt1.Rows[i - 1]["MASTERPRODUCTID_MTMASSETCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Asset. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_MTMASSETCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            else
            {
                strHtml += "<td align=\"left\" colspan=2>&nbsp;</td>";

                if (dt1.Rows[i - 1]["MASTERPRODUCTID_MTMINSTRUCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Instr. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_MTMINSTRUCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                if (dt1.Rows[i - 1]["MASTERPRODUCTID_MTMASSETCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Asset. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_MTMASSETCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
            {

                if (dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLINSTRUCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L (Instr.Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLINSTRUCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
            {

                if (dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLASSETCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L (Asset.Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLASSETCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            else
            {

                if (dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLINSTRUCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L (Instr.Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLINSTRUCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                if (dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLASSETCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L (Asset.Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLASSETCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            if (dt1.Rows[i - 1]["MASTERPRODUCTID_EXPOSURE"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Exposure\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_EXPOSURE"].ToString() + "</td>";
            else
                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

            strHtml += "</tr>";
            ///////////////////////////////////////////MASTERPRODUCT TOTAL END
            ///////////////////////CLIENT TOTAL

            flag = flag + 1;
            strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
            strHtml += "<td align=\"left\" nowrap=\"nowrap;\" colspan=6 title=\"For " + cmbclient.SelectedItem.Text.ToString().Trim() + " [ " + dt1.Rows[0]["UCC"].ToString() + " ]\"><B>Client Total :</B></td>";
            if (ddldate.SelectedItem.Value.ToString() == "1")
            {
                if (ChkBFQty.Checked == false)
                {
                    strHtml += "<td align=\"left\" >&nbsp;</td>";
                }
            }
            if (dt1.Rows[0]["CUSTOMERID_BOOKEDPL"] != DBNull.Value)
                strHtml += "<td align=\"right\" nowrap=\"nowrap;\" style=\"font-size:xx-small;\" title=\"Booked P/L\">" + dt1.Rows[0]["CUSTOMERID_BOOKEDPL"].ToString() + "</td>";
            else
                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

            if (chknetpremium.Checked)
            {
                if (dt1.Rows[0]["CUSTOMERID_Premium"] != DBNull.Value)
                    strHtml += "<td align=\"right\" nowrap=\"nowrap;\" style=\"font-size:xx-small;\" title=\"Premium\">" + dt1.Rows[0]["CUSTOMERID_Premium"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

            }

            strHtml += "<td align=\"left\" colspan=2 nowrap=\"nowrap;\">&nbsp;</td>";

            if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
            {
                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                if (dt1.Rows[0]["CUSTOMERID_MTMINSTRUCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Instr. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["CUSTOMERID_MTMINSTRUCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
            {
                strHtml += "<td align=\"left\" >&nbsp;</td>";
                if (dt1.Rows[0]["CUSTOMERID_MTMASSETCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Asset. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["CUSTOMERID_MTMASSETCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            else
            {

                strHtml += "<td align=\"left\" colspan=2 >&nbsp;</td>";
                if (dt1.Rows[0]["CUSTOMERID_MTMINSTRUCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Instr. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["CUSTOMERID_MTMINSTRUCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (dt1.Rows[0]["CUSTOMERID_MTMASSETCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Asset. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["CUSTOMERID_MTMASSETCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
            {

                if (dt1.Rows[0]["CUSTOMERID_TOTALPLINSTRUCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L(Instr. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["CUSTOMERID_TOTALPLINSTRUCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
            {

                if (dt1.Rows[0]["CUSTOMERID_TOTALPLASSETCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L(Asset. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["CUSTOMERID_TOTALPLASSETCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            else
            {

                if (dt1.Rows[0]["CUSTOMERID_TOTALPLINSTRUCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L(Instr. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["CUSTOMERID_TOTALPLINSTRUCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                if (dt1.Rows[0]["CUSTOMERID_TOTALPLASSETCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L(Asset. Close)\">" + dt1.Rows[0]["CUSTOMERID_TOTALPLASSETCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";
            }
            if (dt1.Rows[0]["CUSTOMERID_Exposure"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Exposure\" nowrap=\"nowrap;\">" + dt1.Rows[0]["CUSTOMERID_Exposure"].ToString() + "</td>";
            else
                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

            strHtml += "</tr>";

            ///////////////////////////////////////////CLIENT TOTAL END


            ///////////////////////GROUP TOTAL

            flag = flag + 1;

            strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
            strHtml += "<td align=\"left\" nowrap=\"nowrap;\" colspan=6 title=\"For " + cmbgroup.SelectedItem.Text.ToString().Trim() + "\"><B>Branch/Group Total :</B></td>";
            if (ddldate.SelectedItem.Value.ToString() == "1")
            {
                if (ChkBFQty.Checked == false)
                {
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }
            }
            if (dt1.Rows[0]["GROUP_BOOKEDPL"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Booked P/L\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_BOOKEDPL"].ToString() + "</td>";
            else
                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

            strHtml += "<td align=\"left\" colspan=2 nowrap=\"nowrap;\">&nbsp;</td>";

            if (chknetpremium.Checked)
            {
                if (dt1.Rows[0]["GROUP_Premium"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Premium\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_Premium"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }

            if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
            {
                strHtml += "<td align=\"left\" >&nbsp;</td>";
                if (dt1.Rows[0]["GROUP_MTMINSTRUCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Instr. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_MTMINSTRUCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
            {
                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                if (dt1.Rows[0]["GROUP_MTMASSETCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Asset. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_MTMASSETCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            else
            {
                strHtml += "<td align=\"left\" colspan=2>&nbsp;</td>";
                if (dt1.Rows[0]["GROUP_MTMINSTRUCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Instr. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_MTMINSTRUCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                if (dt1.Rows[0]["GROUP_MTMASSETCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Asset. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_MTMASSETCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
            {
                if (dt1.Rows[0]["GROUP_TOTALPLINSTRUCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L (Instr. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_TOTALPLINSTRUCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
            {
                if (dt1.Rows[0]["GROUP_TOTALPLASSETCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L (Asset. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_TOTALPLASSETCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            else
            {
                if (dt1.Rows[0]["GROUP_TOTALPLINSTRUCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L (Instr. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_TOTALPLINSTRUCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                if (dt1.Rows[0]["GROUP_TOTALPLASSETCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L (Asset. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_TOTALPLASSETCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            if (dt1.Rows[0]["GROUP_Exposure"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Exposure\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_Exposure"].ToString() + "</td>";
            else
                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

            strHtml += "</tr>";
            strHtml += "</table>";
            ///////////////////////////////////////////GROUP TOTAL END
            if (rbmail.Checked == false)
            {
                DIVdisplayPERIOD.InnerHtml = strHtml1;
                display.InnerHtml = strHtml;

            }
            else
            {
                ViewState["mail"] = strHtml1 + strHtml;
            }




        }
        void htmltable_rpttypescripwise_detail(string clientid, string clientname, string grpid, string grpname)
        {
            ds = (DataSet)ViewState["dataset"];
            String strHtml = String.Empty;
            String strHtml1 = String.Empty;
            int colcount = ds.Tables[0].Columns.Count;
            string str = null;
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                str = "[" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "]";
            }

            if (ddldate.SelectedItem.Value.ToString() == "0")
            {
                str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report For " + oconverter.ArrangeDate2(dtfor.Value.ToString());
            }
            else
            {
                str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report Period " + oconverter.ArrangeDate2(dtfrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtto.Value.ToString());
            }
            strHtml1 = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtml1 += "<tr><td align=\"left\" colspan=" + colcount + " style=\"color:Blue;\">" + str + "</td></tr></table>";

            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

            DataView viewclient = new DataView();
            viewclient = ds.Tables[0].DefaultView;
            viewclient.RowFilter = "PRODUCTID='" + cmbclient.SelectedItem.Value + "'";
            DataTable dt1 = new DataTable();
            dt1 = viewclient.ToTable();


            strHtml += "<tr style=\"background-color:lavender ;text-align:left\">";
            strHtml += "<td align=\"left\" colspan=" + colcount + " nowrap=\"nowrap;\">Scrip Name :&nbsp;<b>" + cmbclient.SelectedItem.Text.ToString().Trim() + "</b></td></tr>";

            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            strHtml += "<td align=\"center\"  colspan=2 nowrap=\"nowrap;\"><b>Client Name</b></td>";
            strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>UCC</b></td>";
            if (ddldate.SelectedItem.Value.ToString() == "1")
            {
                if (ChkBFQty.Checked == false)
                {
                    strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>B/F Qty</b></td>";
                }
            }
            strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Buy </br> Qty</b></td>";
            strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Sell </br> Qty</b></td>";
            strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Booked </br> P/L</b></td>";
            if (chknetpremium.Checked)
            {
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Premium</b></td>";
            }

            strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>C/F  Qty</b></td>";
            strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Avg. </br> Cost </br> (Residual)</b></td>";
            if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
            {
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Close </br> Price </br> (Instrmnt)</b></td>";
            }
            else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
            {
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Close </br> Price </br> (Asset)</b></td>";
            }
            else
            {
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Close </br> Price </br> (Instrmnt)</b></td>";
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Close </br> Price </br> (Asset)</b></td>";
            }
            if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
            {
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>MTM </br>(Instr.</br> Close)</b></td>";
            }
            else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
            {
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>MTM </br>(Asset. </br> Close)</b></td>";
            }
            else
            {
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>MTM </br>(Instr.</br> Close)</b></td>";
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>MTM </br>(Asset.</br> Close)</b></td>";
            }
            if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
            {
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Total </br> P/L </br> (Instr.</br> Close)</b></td>";
            }
            else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
            {
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Total </br> P/L </br> (Asset.</br> Close)</b></td>";
            }
            else
            {
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Total </br> P/L </br> (Instr.</br> Close)</b></td>";
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Total </br> P/L </br> (Asset.</br> Close)</b></td>";
            }
            strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Exposure</b></td>";
            strHtml += "</tr>";
            int flag = 0;

            int i = 0;
            for (i = 0; i < dt1.Rows.Count; i++)
            {
                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" colspan=2 nowrap=\"nowrap;\">" + dt1.Rows[i]["CLIENTNAME"].ToString() + "</td>";
                strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[i]["UCC"].ToString() + "</td>";
                if (ddldate.SelectedItem.Value.ToString() == "1")
                {
                    if (ChkBFQty.Checked == false)
                    {
                        if (dt1.Rows[i]["BFQTY"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"B/F Qty\" nowrap=\"nowrap;\">" + dt1.Rows[i]["BFQTY"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                    }
                }
                if (dt1.Rows[i]["BUYQTY"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Buy Qty\" nowrap=\"nowrap;\">" + dt1.Rows[i]["BUYQTY"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                if (dt1.Rows[i]["SELLQTY"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Sell Qty\" nowrap=\"nowrap;\">" + dt1.Rows[i]["SELLQTY"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                if (dt1.Rows[i]["BOOKEDPL"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Booked P/L\" nowrap=\"nowrap;\">" + dt1.Rows[i]["BOOKEDPL"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                if (chknetpremium.Checked)
                {
                    if (dt1.Rows[i]["Premium"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Premium\" nowrap=\"nowrap;\">" + dt1.Rows[i]["Premium"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                }

                if (dt1.Rows[i]["CFQTY"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"C/F  Qty\" nowrap=\"nowrap;\">" + dt1.Rows[i]["CFQTY"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                if (dt1.Rows[i]["AVGCOSTRESEDUAL"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Avg. Cost (Residual)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["AVGCOSTRESEDUAL"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
                {
                    if (dt1.Rows[i]["CLOSEPRICEINSTRU"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Close Price (Instrmnt)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["CLOSEPRICEINSTRU"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }
                else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
                {
                    if (dt1.Rows[i]["CLOSEPRICEASSET"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Close Price (Asset)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["CLOSEPRICEASSET"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }
                else
                {
                    if (dt1.Rows[i]["CLOSEPRICEINSTRU"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Close Price (Instrmnt)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["CLOSEPRICEINSTRU"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                    if (dt1.Rows[i]["CLOSEPRICEASSET"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Close Price (Asset)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["CLOSEPRICEASSET"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }

                if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
                {
                    if (dt1.Rows[i]["MTMINSTRUCLOSE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Instr. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["MTMINSTRUCLOSE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }
                else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
                {
                    if (dt1.Rows[i]["MTMASSETCLOSE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Asset. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["MTMASSETCLOSE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }
                else
                {
                    if (dt1.Rows[i]["MTMINSTRUCLOSE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Instr. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["MTMINSTRUCLOSE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                    if (dt1.Rows[i]["MTMASSETCLOSE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Asset. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["MTMASSETCLOSE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }


                if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
                {
                    if (dt1.Rows[i]["TOTALPLINSTRUCLOSE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L (Instr..Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["TOTALPLINSTRUCLOSE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }
                else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
                {
                    if (dt1.Rows[i]["TOTALPLASSETCLOSE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L (Asset..Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["TOTALPLASSETCLOSE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\" >&nbsp;</td>";
                }
                else
                {
                    if (dt1.Rows[i]["TOTALPLINSTRUCLOSE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L (Instr.Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["TOTALPLINSTRUCLOSE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                    if (dt1.Rows[i]["TOTALPLASSETCLOSE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L (Asset.Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["TOTALPLASSETCLOSE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }

                if (dt1.Rows[i]["Exposure"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Exposure\" nowrap=\"nowrap;\">" + dt1.Rows[i]["EXPOSURE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                strHtml += "</tr>";

            }
            ////////SCRIP TOTAL BEGIN
            flag = flag + 1;
            strHtml += "<tr style=\"background-color:lavender;text-align:center\">";
            strHtml += "<td align=\"left\" colspan=3 nowrap=\"nowrap;\"><b>Instrument Total :</b></td>";
            if (ddldate.SelectedItem.Value.ToString() == "1")
            {
                if (ChkBFQty.Checked == false)
                {
                    if (dt1.Rows[i - 1]["MASTERPRODUCTID_BFQTY"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"B/F Qty\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_BFQTY"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                }
            }
            if (dt1.Rows[i - 1]["MASTERPRODUCTID_BUYQTY"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Buy Qty\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_BUYQTY"].ToString() + "</td>";
            else
                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

            if (dt1.Rows[i - 1]["MASTERPRODUCTID_SELLQTY"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Sell Qty\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_SELLQTY"].ToString() + "</td>";
            else
                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

            if (dt1.Rows[i - 1]["MASTERPRODUCTID_BOOKEDPL"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Booked P/L\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_BOOKEDPL"].ToString() + "</td>";
            else
                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

            if (chknetpremium.Checked)
            {
                if (dt1.Rows[i - 1]["MASTERPRODUCTID_Premium"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Premium\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_Premium"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

            }

            if (dt1.Rows[i - 1]["MASTERPRODUCTID_CFQTY"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"C/F Qty\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_CFQTY"].ToString() + "</td>";
            else
                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

            strHtml += "<td align=\"left\" colspan=1 nowrap=\"nowrap;\">&nbsp;</td>";

            if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
            {
                strHtml += "<td align=\"left\" >&nbsp;</td>";
                if (dt1.Rows[i - 1]["MASTERPRODUCTID_MTMINSTRUCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Instr.</br> Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_MTMINSTRUCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" nowrap=\"nowrap;\">&nbsp;</td>";

            }
            else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
            {
                strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (dt1.Rows[i - 1]["MASTERPRODUCTID_MTMASSETCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Asset.</br> Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_MTMASSETCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";


            }
            else
            {
                strHtml += "<td align=\"left\" colspan=2 nowrap=\"nowrap;\">&nbsp;</td>";
                if (dt1.Rows[i - 1]["MASTERPRODUCTID_MTMINSTRUCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Instr.</br> Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_MTMINSTRUCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                if (dt1.Rows[i - 1]["MASTERPRODUCTID_MTMASSETCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Asset.</br> Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_MTMASSETCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";


            }
            if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
            {

                if (dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLINSTRUCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L  (Instr.Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLINSTRUCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
            {

                if (dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLASSETCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L  (Asset.Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLASSETCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            else
            {

                if (dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLINSTRUCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L  (Instr.Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLINSTRUCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                if (dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLASSETCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L  (Asset.Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLASSETCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            if (dt1.Rows[i - 1]["MASTERPRODUCTID_EXPOSURE"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\">Exposure\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_EXPOSURE"].ToString() + "</td>";
            else
                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

            strHtml += "</tr>";

            ////////SCRIP TOTAL END
            ///////ASSET TOTAL BEGIN
            flag = flag + 1;
            strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
            strHtml += "<td align=\"left\" colspan=3 title=\"For " + cmbgroup.SelectedItem.Text.ToString().Trim() + "\" nowrap=\"nowrap;\"><B>Asset Total :</B></td>";
            if (ddldate.SelectedItem.Value.ToString() == "1")
            {
                if (ChkBFQty.Checked == false)
                {
                    if (dt1.Rows[0]["GROUP_BFQTY"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"B/F Qty\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_BFQTY"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                }
            }
            if (dt1.Rows[0]["GROUP_BUYQTY"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Buy Qty\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_BUYQTY"].ToString() + "</td>";
            else
                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

            if (dt1.Rows[0]["GROUP_SELLQTY"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Sell Qty\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_SELLQTY"].ToString() + "</td>";
            else
                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

            if (dt1.Rows[0]["GROUP_BOOKEDPL"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Booked P/L\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_BOOKEDPL"].ToString() + "</td>";
            else
                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

            if (chknetpremium.Checked)
            {
                if (dt1.Rows[i - 1]["GROUP_Premium"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Premium\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["GROUP_Premium"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

            }

            if (dt1.Rows[0]["GROUP_CFQTY"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"C/F Qty\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_CFQTY"].ToString() + "</td>";
            else
                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

            strHtml += "<td align=\"left\" colspan=1 nowrap=\"nowrap;\">&nbsp;</td>";

            if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
            {
                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                if (dt1.Rows[0]["GROUP_MTMINSTRUCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Instr. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_MTMINSTRUCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
            {
                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                if (dt1.Rows[0]["GROUP_MTMASSETCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Asset. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_MTMASSETCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            else
            {
                strHtml += "<td align=\"left\" colspan=2 nowrap=\"nowrap;\">&nbsp;</td>";
                if (dt1.Rows[0]["GROUP_MTMINSTRUCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Instr. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_MTMINSTRUCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                if (dt1.Rows[0]["GROUP_MTMASSETCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Asset. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_MTMASSETCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
            {
                if (dt1.Rows[0]["GROUP_TOTALPLINSTRUCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L (Instr. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_TOTALPLINSTRUCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
            {
                if (dt1.Rows[0]["GROUP_TOTALPLASSETCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L (Asset. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_TOTALPLASSETCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            else
            {
                if (dt1.Rows[0]["GROUP_TOTALPLINSTRUCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L (Instr. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_TOTALPLINSTRUCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                if (dt1.Rows[0]["GROUP_TOTALPLASSETCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L (Asset. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_TOTALPLASSETCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            if (dt1.Rows[0]["GROUP_Exposure"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Exposure\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_Exposure"].ToString() + "</td>";
            else
                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

            strHtml += "</tr>";
            //////ASSET TOTAL END
            strHtml += "</table>";
            if (rbmail.Checked == false)
            {
                DIVdisplayPERIOD.InnerHtml = strHtml1;
                display.InnerHtml = strHtml;
            }
            else
            {
                ViewState["mail"] = strHtml1 + strHtml;
            }


        }
        void htmltable_rpttypeclientwise_summary()
        {
            ds = (DataSet)ViewState["dataset"];
            String strHtml = String.Empty;
            String strHtml1 = String.Empty;
            int colcount = ds.Tables[0].Columns.Count;
            string str = null;
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                str = "[" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "]";
            }

            if (ddldate.SelectedItem.Value.ToString() == "0")
            {
                str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report For " + oconverter.ArrangeDate2(dtfor.Value.ToString());
            }
            else
            {
                str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report Period " + oconverter.ArrangeDate2(dtfrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtto.Value.ToString());
            }
            strHtml1 = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtml1 += "<tr><td align=\"left\" colspan=" + colcount + " style=\"color:Blue;\">" + str + "</td></tr></table>";

            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

            DataView viewclient = new DataView();
            viewclient = ds.Tables[0].DefaultView;
            viewclient.RowFilter = "GRPID='" + cmbgroup.SelectedItem.Value + "'";
            DataTable dt1 = new DataTable();
            dt1 = viewclient.ToTable();



            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Client Name</b></td>";
            strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>UCC</b></td>";
            strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Booked </br> P/L</b></td>";
            if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
            {
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>MTM </br>(Instr.</br> Close)</b></td>";
            }
            else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
            {
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>MTM </br>(Asset. </br> Close)</b></td>";
            }
            else
            {
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>MTM </br>(Instr.</br> Close)</b></td>";
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>MTM </br>(Asset.</br> Close)</b></td>";
            }
            if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
            {
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Total </br> P/L </br> (Instr.</br> Close)</b></td>";
            }
            else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
            {
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Total </br> P/L </br> (Asset.</br> Close)</b></td>";
            }
            else
            {
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Total </br> P/L </br> (Instr.</br> Close)</b></td>";
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Total </br> P/L </br> (Asset.</br> Close)</b></td>";
            }
            strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Exposure</b></td>";
            strHtml += "</tr>";
            int flag = 0;

            string MASTERPRODUCT = null;
            int i;
            for (i = 0; i < dt1.Rows.Count; i++)
            {
                ///////////////////////ALL CLIENT 

                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">" + dt1.Rows[i]["CLIENTNAME"].ToString() + "</td>";
                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">" + dt1.Rows[i]["UCC"].ToString() + " </td>";
                if (dt1.Rows[i]["CUSTOMERID_BOOKEDPL"] != DBNull.Value)
                    strHtml += "<td align=\"right\" nowrap=\"nowrap;\" style=\"font-size:xx-small;\" title=\"Booked P/L\">" + dt1.Rows[i]["CUSTOMERID_BOOKEDPL"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\" >&nbsp;</td>";


                if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
                {
                    if (dt1.Rows[i]["CUSTOMERID_MTMINSTRUCLOSE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Instr. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["CUSTOMERID_MTMINSTRUCLOSE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }
                else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
                {
                    if (dt1.Rows[i]["CUSTOMERID_MTMASSETCLOSE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Asset. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["CUSTOMERID_MTMASSETCLOSE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }
                else
                {

                    if (dt1.Rows[i]["CUSTOMERID_MTMINSTRUCLOSE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Instr. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["CUSTOMERID_MTMINSTRUCLOSE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                    if (dt1.Rows[i]["CUSTOMERID_MTMASSETCLOSE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Asset. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["CUSTOMERID_MTMASSETCLOSE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }
                if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
                {

                    if (dt1.Rows[i]["CUSTOMERID_TOTALPLINSTRUCLOSE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L(Instr. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["CUSTOMERID_TOTALPLINSTRUCLOSE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }
                else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
                {

                    if (dt1.Rows[i]["CUSTOMERID_TOTALPLASSETCLOSE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L(Asset. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["CUSTOMERID_TOTALPLASSETCLOSE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }
                else
                {

                    if (dt1.Rows[i]["CUSTOMERID_TOTALPLINSTRUCLOSE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L(Instr. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["CUSTOMERID_TOTALPLINSTRUCLOSE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                    if (dt1.Rows[i]["CUSTOMERID_TOTALPLASSETCLOSE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L(Asset. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["CUSTOMERID_TOTALPLASSETCLOSE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }
                if (dt1.Rows[i]["CUSTOMERID_Exposure"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Exposure\" nowrap=\"nowrap;\">" + dt1.Rows[i]["CUSTOMERID_Exposure"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                strHtml += "</tr>";
            }
            ///////////////////////GROUP TOTAL

            flag = flag + 1;
            strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
            strHtml += "<td align=\"left\" colspan=2 title=\"For " + cmbgroup.SelectedItem.Text.ToString().Trim() + "\" nowrap=\"nowrap;\"><B>Branch/Group Total :</B></td>";

            if (dt1.Rows[0]["GROUP_BOOKEDPL"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Booked P/L\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_BOOKEDPL"].ToString() + "</td>";
            else
                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";


            if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
            {
                if (dt1.Rows[0]["GROUP_MTMINSTRUCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Instr. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_MTMINSTRUCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
            {
                if (dt1.Rows[0]["GROUP_MTMASSETCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Asset. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_MTMASSETCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            else
            {
                if (dt1.Rows[0]["GROUP_MTMINSTRUCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Instr. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_MTMINSTRUCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                if (dt1.Rows[0]["GROUP_MTMASSETCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Asset. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_MTMASSETCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
            {
                if (dt1.Rows[0]["GROUP_TOTALPLINSTRUCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L (Instr. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_TOTALPLINSTRUCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
            {
                if (dt1.Rows[0]["GROUP_TOTALPLASSETCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L (Asset. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_TOTALPLASSETCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            else
            {
                if (dt1.Rows[0]["GROUP_TOTALPLINSTRUCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L (Instr. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_TOTALPLINSTRUCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                if (dt1.Rows[0]["GROUP_TOTALPLASSETCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L (Asset. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_TOTALPLASSETCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            if (dt1.Rows[0]["GROUP_Exposure"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Exposure\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_Exposure"].ToString() + "</td>";
            else
                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

            strHtml += "</tr>";
            ///////////////////////////////////////////GROUP TOTAL END
            DIVdisplayPERIOD.InnerHtml = strHtml1;
            display.InnerHtml = strHtml;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Display", "Display();", true);

        }
        void htmltable_rpttypescripwise_summary()
        {
            ds = (DataSet)ViewState["dataset"];
            String strHtml = String.Empty;
            String strHtml1 = String.Empty;
            int colcount = ds.Tables[0].Columns.Count;
            string str = null;
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                str = "[" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "]";
            }

            if (ddldate.SelectedItem.Value.ToString() == "0")
            {
                str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report For " + oconverter.ArrangeDate2(dtfor.Value.ToString());
            }
            else
            {
                str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report Period " + oconverter.ArrangeDate2(dtfrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtto.Value.ToString());
            }
            strHtml1 = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtml1 += "<tr><td align=\"left\" colspan=" + colcount + " style=\"color:Blue;\">" + str + "</td></tr></table>";

            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

            DataView viewclient = new DataView();
            viewclient = ds.Tables[0].DefaultView;
            viewclient.RowFilter = "MASTERPRODUCTID='" + cmbgroup.SelectedItem.Value + "'";
            DataTable dt1 = new DataTable();
            dt1 = viewclient.ToTable();



            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            strHtml += "<td align=\"center\"  colspan=2 nowrap=\"nowrap;\"><b>Scrip Name</b></td>";
            if (ddldate.SelectedItem.Value.ToString() == "1")
            {
                if (ChkBFQty.Checked == false)
                {
                    strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>B/F Qty</b></td>";
                }
            }
            strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Buy </br> Qty</b></td>";
            strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Sell </br> Qty</b></td>";
            strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Booked </br> P/L</b></td>";
            strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>C/F  Qty</b></td>";
            if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
            {
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>MTM </br>(Instr.</br> Close)</b></td>";
            }
            else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
            {
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>MTM </br>(Asset. </br> Close)</b></td>";
            }
            else
            {
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>MTM </br>(Instr.</br> Close)</b></td>";
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>MTM </br>(Asset.</br> Close)</b></td>";
            }
            if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
            {
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Total </br> P/L </br> (Instr.</br> Close)</b></td>";
            }
            else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
            {
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Total </br> P/L </br> (Asset.</br> Close)</b></td>";
            }
            else
            {
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Total </br> P/L </br> (Instr.</br> Close)</b></td>";
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Total </br> P/L </br> (Asset.</br> Close)</b></td>";
            }
            strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Exposure</b></td>";
            strHtml += "</tr>";
            int flag = 0;

            int i = 0;
            for (i = 0; i < dt1.Rows.Count; i++)
            {
                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" colspan=2 nowrap=\"nowrap;\">" + dt1.Rows[i]["SCRIPNAME"].ToString() + "</td>";
                if (ddldate.SelectedItem.Value.ToString() == "1")
                {
                    if (ChkBFQty.Checked == false)
                    {
                        if (dt1.Rows[i]["MASTERPRODUCTID_BFQTY"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"B/F Qty\" nowrap=\"nowrap;\">" + dt1.Rows[i]["MASTERPRODUCTID_BFQTY"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                    }
                }
                if (dt1.Rows[i]["MASTERPRODUCTID_BUYQTY"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Buy Qty\" nowrap=\"nowrap;\">" + dt1.Rows[i]["MASTERPRODUCTID_BUYQTY"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                if (dt1.Rows[i]["MASTERPRODUCTID_SELLQTY"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Sell Qty\" nowrap=\"nowrap;\">" + dt1.Rows[i]["MASTERPRODUCTID_SELLQTY"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                if (dt1.Rows[i]["MASTERPRODUCTID_BOOKEDPL"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Booked P/L\" nowrap=\"nowrap;\">" + dt1.Rows[i]["MASTERPRODUCTID_BOOKEDPL"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                if (dt1.Rows[i]["MASTERPRODUCTID_CFQTY"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"C/F Qty\" nowrap=\"nowrap;\">" + dt1.Rows[i]["MASTERPRODUCTID_CFQTY"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";



                if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
                {

                    if (dt1.Rows[i]["MASTERPRODUCTID_MTMINSTRUCLOSE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Instr.</br> Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["MASTERPRODUCTID_MTMINSTRUCLOSE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" nowrap=\"nowrap;\">&nbsp;</td>";

                }
                else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
                {


                    if (dt1.Rows[i]["MASTERPRODUCTID_MTMASSETCLOSE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Asset.</br> Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["MASTERPRODUCTID_MTMASSETCLOSE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";


                }
                else
                {

                    if (dt1.Rows[i]["MASTERPRODUCTID_MTMINSTRUCLOSE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Instr.</br> Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["MASTERPRODUCTID_MTMINSTRUCLOSE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                    if (dt1.Rows[i]["MASTERPRODUCTID_MTMASSETCLOSE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Asset.</br> Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["MASTERPRODUCTID_MTMASSETCLOSE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";


                }
                if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
                {

                    if (dt1.Rows[i]["MASTERPRODUCTID_TOTALPLINSTRUCLOSE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L  (Instr.Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["MASTERPRODUCTID_TOTALPLINSTRUCLOSE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }
                else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
                {

                    if (dt1.Rows[i]["MASTERPRODUCTID_TOTALPLASSETCLOSE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L  (Asset.Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["MASTERPRODUCTID_TOTALPLASSETCLOSE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }
                else
                {

                    if (dt1.Rows[i]["MASTERPRODUCTID_TOTALPLINSTRUCLOSE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L  (Instr.Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["MASTERPRODUCTID_TOTALPLINSTRUCLOSE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                    if (dt1.Rows[i]["MASTERPRODUCTID_TOTALPLASSETCLOSE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L  (Asset.Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["MASTERPRODUCTID_TOTALPLASSETCLOSE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }
                if (dt1.Rows[i]["MASTERPRODUCTID_EXPOSURE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\">Exposure\" nowrap=\"nowrap;\">" + dt1.Rows[i]["MASTERPRODUCTID_EXPOSURE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                strHtml += "</tr>";

            }

            ///////ASSET TOTAL BEGIN
            flag = flag + 1;
            strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
            strHtml += "<td align=\"left\" colspan=2 title=\"For " + cmbgroup.SelectedItem.Text.ToString().Trim() + "\" nowrap=\"nowrap;\"><B>Asset Total :</B></td>";
            if (ddldate.SelectedItem.Value.ToString() == "1")
            {
                if (ChkBFQty.Checked == false)
                {
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }
            }
            strHtml += "<td align=\"left\" colspan=2 >&nbsp;</td>";
            if (dt1.Rows[0]["GROUP_BOOKEDPL"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Booked P/L\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_BOOKEDPL"].ToString() + "</td>";
            else
                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

            strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

            if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
            {

                if (dt1.Rows[0]["GROUP_MTMINSTRUCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Instr. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_MTMINSTRUCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
            {

                if (dt1.Rows[0]["GROUP_MTMASSETCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Asset. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_MTMASSETCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            else
            {

                if (dt1.Rows[0]["GROUP_MTMINSTRUCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Instr. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_MTMINSTRUCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                if (dt1.Rows[0]["GROUP_MTMASSETCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Asset. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_MTMASSETCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
            {
                if (dt1.Rows[0]["GROUP_TOTALPLINSTRUCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L (Instr. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_TOTALPLINSTRUCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
            {
                if (dt1.Rows[0]["GROUP_TOTALPLASSETCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L (Asset. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_TOTALPLASSETCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            else
            {
                if (dt1.Rows[0]["GROUP_TOTALPLINSTRUCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L (Instr. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_TOTALPLINSTRUCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                if (dt1.Rows[0]["GROUP_TOTALPLASSETCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L (Asset. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_TOTALPLASSETCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            if (dt1.Rows[0]["GROUP_Exposure"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Exposure\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_Exposure"].ToString() + "</td>";
            else
                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

            strHtml += "</tr>";
            //////ASSET TOTAL END
            strHtml += "</table>";
            if (rbmail.Checked == false)
            {
                DIVdisplayPERIOD.InnerHtml = strHtml1;
                display.InnerHtml = strHtml;
            }
            else
            {
                ViewState["mail"] = strHtml1 + strHtml;
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Display", "Display();", true);
        }
        protected string GetRowColor(int i)
        {
            if (i++ % 2 == 0)
                return "#fff0f5";
            else
                return "White";
        }

        protected void NavigationLinkC_Click(Object sender, CommandEventArgs e)
        {
            hiddencount.Value = "0";
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
            cmbgroup.SelectedIndex = curentIndex;
            if (ddlrptview.SelectedItem.Value.ToString() == "0")
            {
                if (ddlrpttype.SelectedItem.Value.ToString().Trim() == "0")
                {
                    ddlbandforClient();
                }
                else
                {
                    htmltable_rpttypeclientwise_summary();
                }
            }
            else
            {
                if (ddlrpttype.SelectedItem.Value.ToString().Trim() == "0")
                {
                    ddlbandforScrip();
                }
                else
                {
                    htmltable_rpttypescripwise_summary();
                }
            }
        }
        protected void btnprint_Click(object sender, EventArgs e)
        {
            procedure();
            ds = (DataSet)ViewState["dataset"];
            ReportDocument report = new ReportDocument();
            byte[] logoinByte;
            ds.Tables[0].Columns.Add("Image", System.Type.GetType("System.Byte[]"));
            if (CHKLOGOPRINT.Checked == false)
            {
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
            }
            //ds.Tables[0].WriteXmlSchema("E:\\RPTXSD\\portfoliorpt.xsd");

            //report.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;

            string tmpPdfPath = string.Empty;
            tmpPdfPath = HttpContext.Current.Server.MapPath("..\\Reports\\portfoliorpt.rpt");
            report.Load(tmpPdfPath);
            report.SetDataSource(ds.Tables[0]);
            report.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "Portfolio Performance Report");
            report.Dispose();
            GC.Collect();
        }

        void export_rpttypeclientwise_detail()
        {
            ds = (DataSet)ViewState["dataset"];
            DataTable dtExport = new DataTable();
            dtExport.Clear();
            dtExport.Columns.Add("Asset", Type.GetType("System.String"));
            dtExport.Columns.Add("Instr. Type", Type.GetType("System.String"));
            dtExport.Columns.Add("Expiry", Type.GetType("System.String"));
            dtExport.Columns.Add("Strike", Type.GetType("System.String"));
            if (ddldate.SelectedItem.Value.ToString() == "1")
            {
                if (ChkBFQty.Checked == false)
                {
                    dtExport.Columns.Add("B/F Qty", Type.GetType("System.String"));
                }
            }
            dtExport.Columns.Add("Buy Qty", Type.GetType("System.String"));
            dtExport.Columns.Add("Sell Qty", Type.GetType("System.String"));
            dtExport.Columns.Add("Booked P/L", Type.GetType("System.String"));
            if (chknetpremium.Checked)
            {
                dtExport.Columns.Add("Premium", Type.GetType("System.String"));
            }
            dtExport.Columns.Add("C/F Qty", Type.GetType("System.String"));
            dtExport.Columns.Add("Avg.Cost (Residual)", Type.GetType("System.String"));
            if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
            {
                dtExport.Columns.Add("Close Price (Instrmnt)", Type.GetType("System.String"));
            }
            else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
            {
                dtExport.Columns.Add("Close Price (Asset)", Type.GetType("System.String"));
            }
            else
            {
                dtExport.Columns.Add("Close Price (Instr)", Type.GetType("System.String"));
                dtExport.Columns.Add("Close Price (Asset)", Type.GetType("System.String"));
            }
            if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
            {
                dtExport.Columns.Add("MTM (Instr.Close)", Type.GetType("System.String"));
            }
            else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
            {
                dtExport.Columns.Add("MTM (Asset.Close)", Type.GetType("System.String"));
            }
            else
            {
                dtExport.Columns.Add("MTM (Instr.Close)", Type.GetType("System.String"));
                dtExport.Columns.Add("MTM (Asset.Close)", Type.GetType("System.String"));
            }
            if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
            {
                dtExport.Columns.Add("Total P/L (Instr.Close)", Type.GetType("System.String"));
            }
            else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
            {
                dtExport.Columns.Add("Total P/L (Asset.Close)", Type.GetType("System.String"));
            }
            else
            {
                dtExport.Columns.Add("Total P/L (Instr.Close)", Type.GetType("System.String"));
                dtExport.Columns.Add("Total P/L (Asset.Close)", Type.GetType("System.String"));
            }

            dtExport.Columns.Add("Exposure", Type.GetType("System.String"));

            string MASTERPRODUCT = null;
            int i;

            for (int j = 0; j < cmbgroup.Items.Count; j++)
            {
                DataRow row = dtExport.NewRow();
                row["Asset"] = ddlGroup.SelectedItem.Text.ToString().Trim() + " Name:" + cmbgroup.Items[j].Text.ToString().Trim();
                row["Instr. Type"] = "Test";
                dtExport.Rows.Add(row);

                DataView viewgrp = new DataView();
                viewgrp = ds.Tables[0].DefaultView;
                viewgrp.RowFilter = "GRPID='" + cmbgroup.Items[j].Value + "'";
                DataTable dt = new DataTable();
                dt = viewgrp.ToTable();

                DataView viewClient = new DataView(dt);
                Distinctclient = new DataTable();
                Distinctclient = viewClient.ToTable(true, new string[] { "CUSTOMERID", "CLIENTNAME" });

                if (Distinctclient.Rows.Count > 0)
                {
                    cmbclient.Items.Clear();
                    cmbclient.DataSource = Distinctclient;
                    cmbclient.DataValueField = "CUSTOMERID";
                    cmbclient.DataTextField = "CLIENTNAME";
                    cmbclient.DataBind();

                }

                for (int k = 0; k < cmbclient.Items.Count; k++)
                {
                    DataRow row1 = dtExport.NewRow();
                    row1["Asset"] = "Client Name:" + cmbclient.Items[k].Text.ToString().Trim();
                    row1["Instr. Type"] = "Test";
                    dtExport.Rows.Add(row1);

                    DataView viewclient = new DataView();
                    viewclient = ds.Tables[0].DefaultView;
                    viewclient.RowFilter = "CUSTOMERID='" + cmbclient.Items[k].Value + "'";
                    DataTable dt1 = new DataTable();
                    dt1 = viewclient.ToTable();

                    MASTERPRODUCT = null;

                    for (i = 0; i < dt1.Rows.Count; i++)
                    {
                        if (MASTERPRODUCT != null)
                        {
                            if (MASTERPRODUCT != dt1.Rows[i]["MASTERPRODUCTID"].ToString())
                            {
                                ///////////////////////MASTERPRODUCT TOTAL

                                DataRow row2 = dtExport.NewRow();
                                row2["Asset"] = "Asset Total";

                                if (ddldate.SelectedItem.Value.ToString() == "1")
                                {
                                    if (ChkBFQty.Checked == false)
                                    {
                                        if (dt1.Rows[i - 1]["MASTERPRODUCTID_BFQTY"] != DBNull.Value)
                                            row2["B/F Qty"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i - 1]["MASTERPRODUCTID_BFQTY"].ToString()));
                                    }
                                }

                                if (dt1.Rows[i - 1]["MASTERPRODUCTID_BUYQTY"] != DBNull.Value)
                                    row2["Buy Qty"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i - 1]["MASTERPRODUCTID_BUYQTY"].ToString()));

                                if (dt1.Rows[i - 1]["MASTERPRODUCTID_SELLQTY"] != DBNull.Value)
                                    row2["Sell Qty"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i - 1]["MASTERPRODUCTID_SELLQTY"].ToString()));

                                if (dt1.Rows[i - 1]["MASTERPRODUCTID_BOOKEDPL"] != DBNull.Value)
                                    row2["Booked P/L"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i - 1]["MASTERPRODUCTID_BOOKEDPL"].ToString()));

                                if (chknetpremium.Checked)
                                {
                                    if (dt1.Rows[i - 1]["MASTERPRODUCTID_Premium"] != DBNull.Value)
                                        row2["Premium"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i - 1]["MASTERPRODUCTID_Premium"].ToString()));
                                }


                                if (dt1.Rows[i - 1]["MASTERPRODUCTID_CFQTY"] != DBNull.Value)
                                    row2["C/F Qty"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i - 1]["MASTERPRODUCTID_CFQTY"].ToString()));

                                if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
                                {
                                    if (dt1.Rows[i - 1]["MASTERPRODUCTID_MTMINSTRUCLOSE"] != DBNull.Value)
                                        row2["MTM (Instr.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i - 1]["MASTERPRODUCTID_MTMINSTRUCLOSE"].ToString()));
                                }
                                else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
                                {
                                    if (dt1.Rows[i - 1]["MASTERPRODUCTID_MTMASSETCLOSE"] != DBNull.Value)
                                        row2["MTM (Asset.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i - 1]["MASTERPRODUCTID_MTMASSETCLOSE"].ToString()));
                                }
                                else
                                {
                                    if (dt1.Rows[i - 1]["MASTERPRODUCTID_MTMINSTRUCLOSE"] != DBNull.Value)
                                        row2["MTM (Instr.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i - 1]["MASTERPRODUCTID_MTMINSTRUCLOSE"].ToString()));

                                    if (dt1.Rows[i - 1]["MASTERPRODUCTID_MTMASSETCLOSE"] != DBNull.Value)
                                        row2["MTM (Asset.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i - 1]["MASTERPRODUCTID_MTMASSETCLOSE"].ToString()));

                                }
                                if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
                                {

                                    if (dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLINSTRUCLOSE"] != DBNull.Value)
                                        row2["Total P/L (Instr.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLINSTRUCLOSE"].ToString()));

                                }
                                else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
                                {
                                    if (dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLASSETCLOSE"] != DBNull.Value)
                                        row2["Total P/L (Asset.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLASSETCLOSE"].ToString()));

                                }
                                else
                                {

                                    if (dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLINSTRUCLOSE"] != DBNull.Value)
                                        row2["Total P/L (Instr.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLINSTRUCLOSE"].ToString()));

                                    if (dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLASSETCLOSE"] != DBNull.Value)
                                        row2["Total P/L (Asset.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLASSETCLOSE"].ToString()));

                                }
                                if (dt1.Rows[i - 1]["MASTERPRODUCTID_EXPOSURE"] != DBNull.Value)
                                    row2["Exposure"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i - 1]["MASTERPRODUCTID_EXPOSURE"].ToString()));

                                dtExport.Rows.Add(row2);
                                ///////////////////////////////////////////MASTERPRODUCT TOTAL END
                            }
                        }

                        ////////////////////////ALL DATA BEGIN
                        DataRow row6 = dtExport.NewRow();
                        if (MASTERPRODUCT != dt1.Rows[i]["MASTERPRODUCTID"].ToString())
                        {
                            if (dt1.Rows[i]["TICKERSYMBOL"] != DBNull.Value)
                                row6["Asset"] = dt1.Rows[i]["TICKERSYMBOL"].ToString();
                        }

                        MASTERPRODUCT = dt1.Rows[i]["MASTERPRODUCTID"].ToString();

                        if (dt1.Rows[i]["SERIES"] != DBNull.Value)
                            row6["Instr. Type"] = dt1.Rows[i]["SERIES"].ToString();

                        if (dt1.Rows[i]["EXPIRY"] != DBNull.Value)
                            row6["Expiry"] = dt1.Rows[i]["EXPIRY"].ToString();

                        if (dt1.Rows[i]["STRIKEPRICE"] != DBNull.Value)
                            row6["Strike"] = dt1.Rows[i]["STRIKEPRICE"].ToString();


                        if (ddldate.SelectedItem.Value.ToString() == "1")
                        {
                            if (ChkBFQty.Checked == false)
                            {
                                if (dt1.Rows[i]["BFQTY"] != DBNull.Value)
                                    row6["B/F Qty"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["BFQTY"].ToString()));
                            }
                        }
                        if (dt1.Rows[i]["BUYQTY"] != DBNull.Value)
                            row6["Buy Qty"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["BUYQTY"].ToString()));

                        if (dt1.Rows[i]["SELLQTY"] != DBNull.Value)
                            row6["Sell Qty"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["SELLQTY"].ToString()));

                        if (dt1.Rows[i]["BOOKEDPL"] != DBNull.Value)
                            row6["Booked P/L"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["BOOKEDPL"].ToString()));

                        if (chknetpremium.Checked)
                        {
                            if (dt1.Rows[i]["Premium"] != DBNull.Value)
                                row6["Premium"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["Premium"].ToString()));

                        }

                        if (dt1.Rows[i]["CFQTY"] != DBNull.Value)
                            row6["C/F Qty"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["CFQTY"].ToString()));

                        if (dt1.Rows[i]["AVGCOSTRESEDUAL"] != DBNull.Value)
                            row6["Avg.Cost (Residual)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["AVGCOSTRESEDUAL"].ToString()));

                        if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
                        {
                            if (dt1.Rows[i]["CLOSEPRICEINSTRU"] != DBNull.Value)
                                row6["Close Price (Instrmnt)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["CLOSEPRICEINSTRU"].ToString()));

                        }
                        else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
                        {
                            if (dt1.Rows[i]["CLOSEPRICEASSET"] != DBNull.Value)
                                row6["Close Price (Asset)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["CLOSEPRICEASSET"].ToString()));

                        }
                        else
                        {
                            if (dt1.Rows[i]["CLOSEPRICEINSTRU"] != DBNull.Value)
                                row6["Close Price (Instr)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["CLOSEPRICEINSTRU"].ToString()));

                            if (dt1.Rows[i]["CLOSEPRICEASSET"] != DBNull.Value)
                                row6["Close Price (Asset)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["CLOSEPRICEASSET"].ToString()));

                        }

                        if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
                        {
                            if (dt1.Rows[i]["MTMINSTRUCLOSE"] != DBNull.Value)
                                row6["MTM (Instr.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["MTMINSTRUCLOSE"].ToString()));

                        }
                        else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
                        {
                            if (dt1.Rows[i]["MTMASSETCLOSE"] != DBNull.Value)
                                row6["MTM (Asset.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["MTMASSETCLOSE"].ToString()));

                        }
                        else
                        {
                            if (dt1.Rows[i]["MTMINSTRUCLOSE"] != DBNull.Value)
                                row6["MTM (Instr.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["MTMINSTRUCLOSE"].ToString()));


                            if (dt1.Rows[i]["MTMASSETCLOSE"] != DBNull.Value)
                                row6["MTM (Asset.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["MTMASSETCLOSE"].ToString()));

                        }


                        if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
                        {
                            if (dt1.Rows[i]["TOTALPLINSTRUCLOSE"] != DBNull.Value)
                                row6["Total P/L (Instr.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["TOTALPLINSTRUCLOSE"].ToString()));
                        }
                        else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
                        {
                            if (dt1.Rows[i]["TOTALPLASSETCLOSE"] != DBNull.Value)
                                row6["Total P/L (Asset.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["TOTALPLASSETCLOSE"].ToString()));
                        }
                        else
                        {
                            if (dt1.Rows[i]["TOTALPLINSTRUCLOSE"] != DBNull.Value)
                                row6["Total P/L (Instr.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["TOTALPLINSTRUCLOSE"].ToString()));

                            if (dt1.Rows[i]["TOTALPLASSETCLOSE"] != DBNull.Value)
                                row6["Total P/L (Asset.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["TOTALPLASSETCLOSE"].ToString()));
                        }

                        if (dt1.Rows[i]["Exposure"] != DBNull.Value)
                            row6["Exposure"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["EXPOSURE"].ToString()));

                        dtExport.Rows.Add(row6);
                        //////////////////////ALL DATA END
                    }

                    DataRow row3 = dtExport.NewRow();
                    row3["Asset"] = "Asset Total";

                    if (ddldate.SelectedItem.Value.ToString() == "1")
                    {
                        if (ChkBFQty.Checked == false)
                        {
                            if (dt1.Rows[i - 1]["MASTERPRODUCTID_BFQTY"] != DBNull.Value)
                                row3["B/F Qty"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i - 1]["MASTERPRODUCTID_BFQTY"].ToString()));
                        }
                    }

                    if (dt1.Rows[i - 1]["MASTERPRODUCTID_BUYQTY"] != DBNull.Value)
                        row3["Buy Qty"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i - 1]["MASTERPRODUCTID_BUYQTY"].ToString()));

                    if (dt1.Rows[i - 1]["MASTERPRODUCTID_SELLQTY"] != DBNull.Value)
                        row3["Sell Qty"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i - 1]["MASTERPRODUCTID_SELLQTY"].ToString()));

                    if (dt1.Rows[i - 1]["MASTERPRODUCTID_BOOKEDPL"] != DBNull.Value)
                        row3["Booked P/L"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i - 1]["MASTERPRODUCTID_BOOKEDPL"].ToString()));

                    if (chknetpremium.Checked)
                    {
                        if (dt1.Rows[i - 1]["MASTERPRODUCTID_Premium"] != DBNull.Value)
                            row3["Premium"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i - 1]["MASTERPRODUCTID_Premium"].ToString()));
                    }

                    if (dt1.Rows[i - 1]["MASTERPRODUCTID_CFQTY"] != DBNull.Value)
                        row3["C/F Qty"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i - 1]["MASTERPRODUCTID_CFQTY"].ToString()));

                    if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
                    {
                        if (dt1.Rows[i - 1]["MASTERPRODUCTID_MTMINSTRUCLOSE"] != DBNull.Value)
                            row3["MTM (Instr.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i - 1]["MASTERPRODUCTID_MTMINSTRUCLOSE"].ToString()));
                    }
                    else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
                    {
                        if (dt1.Rows[i - 1]["MASTERPRODUCTID_MTMASSETCLOSE"] != DBNull.Value)
                            row3["MTM (Asset.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i - 1]["MASTERPRODUCTID_MTMASSETCLOSE"].ToString()));
                    }
                    else
                    {
                        if (dt1.Rows[i - 1]["MASTERPRODUCTID_MTMINSTRUCLOSE"] != DBNull.Value)
                            row3["MTM (Instr.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i - 1]["MASTERPRODUCTID_MTMINSTRUCLOSE"].ToString()));

                        if (dt1.Rows[i - 1]["MASTERPRODUCTID_MTMASSETCLOSE"] != DBNull.Value)
                            row3["MTM (Asset.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i - 1]["MASTERPRODUCTID_MTMASSETCLOSE"].ToString()));

                    }
                    if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
                    {

                        if (dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLINSTRUCLOSE"] != DBNull.Value)
                            row3["Total P/L (Instr.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLINSTRUCLOSE"].ToString()));

                    }
                    else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
                    {
                        if (dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLASSETCLOSE"] != DBNull.Value)
                            row3["Total P/L (Asset.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLASSETCLOSE"].ToString()));

                    }
                    else
                    {

                        if (dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLINSTRUCLOSE"] != DBNull.Value)
                            row3["Total P/L (Instr.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLINSTRUCLOSE"].ToString()));

                        if (dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLASSETCLOSE"] != DBNull.Value)
                            row3["Total P/L (Asset.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLASSETCLOSE"].ToString()));

                    }
                    if (dt1.Rows[i - 1]["MASTERPRODUCTID_EXPOSURE"] != DBNull.Value)
                        row3["Exposure"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i - 1]["MASTERPRODUCTID_EXPOSURE"].ToString()));

                    dtExport.Rows.Add(row3);
                    ///////////////////////////////////////////MASTERPRODUCT TOTAL END
                    //////////////////////////////////////////CLIENT TOTAL
                    DataRow blankrow = dtExport.NewRow();
                    dtExport.Rows.Add(blankrow);

                    DataRow row4 = dtExport.NewRow();
                    row4["Asset"] = "Client Total";

                    if (dt1.Rows[i - 1]["CUSTOMERID_BOOKEDPL"] != DBNull.Value)
                        row4["Booked P/L"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i - 1]["CUSTOMERID_BOOKEDPL"].ToString()));

                    if (chknetpremium.Checked)
                    {
                        if (dt1.Rows[i - 1]["CUSTOMERID_Premium"] != DBNull.Value)
                            row4["Premium"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i - 1]["CUSTOMERID_Premium"].ToString()));

                    }

                    if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
                    {
                        if (dt1.Rows[i - 1]["CUSTOMERID_MTMINSTRUCLOSE"] != DBNull.Value)
                            row4["MTM (Instr.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i - 1]["CUSTOMERID_MTMINSTRUCLOSE"].ToString()));
                    }
                    else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
                    {
                        if (dt1.Rows[i - 1]["CUSTOMERID_MTMASSETCLOSE"] != DBNull.Value)
                            row4["MTM (Asset.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i - 1]["CUSTOMERID_MTMASSETCLOSE"].ToString()));
                    }
                    else
                    {
                        if (dt1.Rows[i - 1]["CUSTOMERID_MTMINSTRUCLOSE"] != DBNull.Value)
                            row4["MTM (Instr.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i - 1]["CUSTOMERID_MTMINSTRUCLOSE"].ToString()));

                        if (dt1.Rows[i - 1]["CUSTOMERID_MTMASSETCLOSE"] != DBNull.Value)
                            row4["MTM (Asset.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i - 1]["CUSTOMERID_MTMASSETCLOSE"].ToString()));

                    }
                    if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
                    {

                        if (dt1.Rows[i - 1]["CUSTOMERID_TOTALPLINSTRUCLOSE"] != DBNull.Value)
                            row4["Total P/L (Instr.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i - 1]["CUSTOMERID_TOTALPLINSTRUCLOSE"].ToString()));

                    }
                    else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
                    {
                        if (dt1.Rows[i - 1]["CUSTOMERID_TOTALPLASSETCLOSE"] != DBNull.Value)
                            row4["Total P/L (Asset.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i - 1]["CUSTOMERID_TOTALPLASSETCLOSE"].ToString()));

                    }
                    else
                    {

                        if (dt1.Rows[i - 1]["CUSTOMERID_TOTALPLINSTRUCLOSE"] != DBNull.Value)
                            row4["Total P/L (Instr.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i - 1]["CUSTOMERID_TOTALPLINSTRUCLOSE"].ToString()));

                        if (dt1.Rows[i - 1]["CUSTOMERID_TOTALPLASSETCLOSE"] != DBNull.Value)
                            row4["Total P/L (Asset.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i - 1]["CUSTOMERID_TOTALPLASSETCLOSE"].ToString()));

                    }
                    if (dt1.Rows[i - 1]["CUSTOMERID_Exposure"] != DBNull.Value)
                        row4["Exposure"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i - 1]["CUSTOMERID_Exposure"].ToString()));

                    dtExport.Rows.Add(row4);
                    ///////////////////////////////////////////CLIENT TOTAL END
                }
            }


            DataTable dtReportHeader = new DataTable();
            DataTable dtReportFooter = new DataTable();

            DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            HeaderRow[0] = CompanyName.Rows[0][0].ToString();
            dtReportHeader.Rows.Add(HeaderRow);
            DataRow DrRowR1 = dtReportHeader.NewRow();

            string str = null;
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                str = "[" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "]";
            }

            if (ddldate.SelectedItem.Value.ToString() == "0")
            {
                str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report For " + oconverter.ArrangeDate2(dtfor.Value.ToString());
            }
            else
            {
                str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report Period " + oconverter.ArrangeDate2(dtfrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtto.Value.ToString());
            }

            DrRowR1[0] = "F&O Portfolio Performance Report:" + str;

            dtReportHeader.Rows.Add(DrRowR1);
            DataRow DrRowR2 = dtReportHeader.NewRow();
            //DrRowR2[0] = txtBankName.Text;
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

            ViewState["dtExport"] = dtExport;
            ViewState["dtReportHeader"] = dtReportHeader;
            ViewState["dtReportFooter"] = dtReportFooter;
        }
        void export_rpttypeclientwise_summary()
        {
            ds = (DataSet)ViewState["dataset"];
            DataTable dtExport = new DataTable();
            dtExport.Clear();
            dtExport.Columns.Add("Client Name", Type.GetType("System.String"));
            dtExport.Columns.Add("UCC", Type.GetType("System.String"));
            dtExport.Columns.Add("Booked P/L", Type.GetType("System.String"));
            if (chknetpremium.Checked)
            {
                dtExport.Columns.Add("Premium", Type.GetType("System.String"));

            }
            if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
            {
                dtExport.Columns.Add("MTM (Instr.Close)", Type.GetType("System.String"));
            }
            else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
            {
                dtExport.Columns.Add("MTM (Asset.Close)", Type.GetType("System.String"));
            }
            else
            {
                dtExport.Columns.Add("MTM (Instr.Close)", Type.GetType("System.String"));
                dtExport.Columns.Add("MTM (Asset.Close)", Type.GetType("System.String"));
            }
            if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
            {
                dtExport.Columns.Add("Total P/L (Instr.Close)", Type.GetType("System.String"));
            }
            else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
            {
                dtExport.Columns.Add("Total P/L (Asset.Close)", Type.GetType("System.String"));
            }
            else
            {
                dtExport.Columns.Add("Total P/L (Instr.Close)", Type.GetType("System.String"));
                dtExport.Columns.Add("Total P/L (Asset.Close)", Type.GetType("System.String"));
            }

            dtExport.Columns.Add("Exposure", Type.GetType("System.String"));

            string MASTERPRODUCT = null;
            int i;

            for (int j = 0; j < cmbgroup.Items.Count; j++)
            {
                DataRow row = dtExport.NewRow();
                row["Client Name"] = ddlGroup.SelectedItem.Text.ToString().Trim() + " Name:" + cmbgroup.Items[j].Text.ToString().Trim();
                row["UCC"] = "Test";
                dtExport.Rows.Add(row);

                DataView viewgrp = new DataView();
                viewgrp = ds.Tables[0].DefaultView;
                viewgrp.RowFilter = "GRPID='" + cmbgroup.Items[j].Value + "'";
                DataTable dt1 = new DataTable();
                dt1 = viewgrp.ToTable();



                for (i = 0; i < dt1.Rows.Count; i++)
                {

                    DataRow row4 = dtExport.NewRow();
                    row4["Client Name"] = dt1.Rows[i]["CLIENTNAME"].ToString().Trim();
                    row4["UCC"] = dt1.Rows[i]["UCC"].ToString().Trim();

                    if (dt1.Rows[i]["CUSTOMERID_BOOKEDPL"] != DBNull.Value)
                        row4["Booked P/L"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["CUSTOMERID_BOOKEDPL"].ToString()));

                    if (chknetpremium.Checked)
                    {
                        if (dt1.Rows[i]["CUSTOMERID_Premium"] != DBNull.Value)
                            row4["Premium"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["CUSTOMERID_Premium"].ToString()));
                    }
                    if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
                    {
                        if (dt1.Rows[i]["CUSTOMERID_MTMINSTRUCLOSE"] != DBNull.Value)
                            row4["MTM (Instr.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["CUSTOMERID_MTMINSTRUCLOSE"].ToString()));
                    }
                    else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
                    {
                        if (dt1.Rows[i]["CUSTOMERID_MTMASSETCLOSE"] != DBNull.Value)
                            row4["MTM (Asset.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["CUSTOMERID_MTMASSETCLOSE"].ToString()));
                    }
                    else
                    {
                        if (dt1.Rows[i]["CUSTOMERID_MTMINSTRUCLOSE"] != DBNull.Value)
                            row4["MTM (Instr.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["CUSTOMERID_MTMINSTRUCLOSE"].ToString()));

                        if (dt1.Rows[i]["CUSTOMERID_MTMASSETCLOSE"] != DBNull.Value)
                            row4["MTM (Asset.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["CUSTOMERID_MTMASSETCLOSE"].ToString()));

                    }
                    if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
                    {

                        if (dt1.Rows[i]["CUSTOMERID_TOTALPLINSTRUCLOSE"] != DBNull.Value)
                            row4["Total P/L (Instr.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["CUSTOMERID_TOTALPLINSTRUCLOSE"].ToString()));

                    }
                    else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
                    {
                        if (dt1.Rows[i]["CUSTOMERID_TOTALPLASSETCLOSE"] != DBNull.Value)
                            row4["Total P/L (Asset.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["CUSTOMERID_TOTALPLASSETCLOSE"].ToString()));

                    }
                    else
                    {

                        if (dt1.Rows[i]["CUSTOMERID_TOTALPLINSTRUCLOSE"] != DBNull.Value)
                            row4["Total P/L (Instr.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["CUSTOMERID_TOTALPLINSTRUCLOSE"].ToString()));

                        if (dt1.Rows[i]["CUSTOMERID_TOTALPLASSETCLOSE"] != DBNull.Value)
                            row4["Total P/L (Asset.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["CUSTOMERID_TOTALPLASSETCLOSE"].ToString()));

                    }
                    if (dt1.Rows[i]["CUSTOMERID_Exposure"] != DBNull.Value)
                        row4["Exposure"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["CUSTOMERID_Exposure"].ToString()));

                    dtExport.Rows.Add(row4);

                }

                ////////////group total
                DataRow row5 = dtExport.NewRow();
                row5["Client Name"] = "Group Total :";

                if (dt1.Rows[0]["GROUP_BOOKEDPL"] != DBNull.Value)
                    row5["Booked P/L"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["GROUP_BOOKEDPL"].ToString()));

                if (chknetpremium.Checked)
                {
                    if (dt1.Rows[0]["GROUP_Premium"] != DBNull.Value)
                        row5["Premium"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["GROUP_Premium"].ToString()));
                }
                if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
                {
                    if (dt1.Rows[0]["GROUP_MTMINSTRUCLOSE"] != DBNull.Value)
                        row5["MTM (Instr.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["GROUP_MTMINSTRUCLOSE"].ToString()));
                }
                else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
                {
                    if (dt1.Rows[0]["GROUP_MTMASSETCLOSE"] != DBNull.Value)
                        row5["MTM (Asset.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["GROUP_MTMASSETCLOSE"].ToString()));
                }
                else
                {
                    if (dt1.Rows[0]["CUSTOMERID_MTMINSTRUCLOSE"] != DBNull.Value)
                        row5["MTM (Instr.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["CUSTOMERID_MTMINSTRUCLOSE"].ToString()));

                    if (dt1.Rows[0]["CUSTOMERID_MTMASSETCLOSE"] != DBNull.Value)
                        row5["MTM (Asset.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["CUSTOMERID_MTMASSETCLOSE"].ToString()));

                }
                if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
                {

                    if (dt1.Rows[0]["GROUP_TOTALPLINSTRUCLOSE"] != DBNull.Value)
                        row5["Total P/L (Instr.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["GROUP_TOTALPLINSTRUCLOSE"].ToString()));

                }
                else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
                {
                    if (dt1.Rows[0]["GROUP_TOTALPLASSETCLOSE"] != DBNull.Value)
                        row5["Total P/L (Asset.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["GROUP_TOTALPLASSETCLOSE"].ToString()));

                }
                else
                {

                    if (dt1.Rows[0]["GROUP_TOTALPLINSTRUCLOSE"] != DBNull.Value)
                        row5["Total P/L (Instr.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["GROUP_TOTALPLINSTRUCLOSE"].ToString()));

                    if (dt1.Rows[0]["GROUP_TOTALPLASSETCLOSE"] != DBNull.Value)
                        row5["Total P/L (Asset.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["GROUP_TOTALPLASSETCLOSE"].ToString()));

                }
                if (dt1.Rows[0]["GROUP_Exposure"] != DBNull.Value)
                    row5["Exposure"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["GROUP_Exposure"].ToString()));

                dtExport.Rows.Add(row5);

            }


            DataTable dtReportHeader = new DataTable();
            DataTable dtReportFooter = new DataTable();

            DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            HeaderRow[0] = CompanyName.Rows[0][0].ToString();
            dtReportHeader.Rows.Add(HeaderRow);
            DataRow DrRowR1 = dtReportHeader.NewRow();

            string str = null;
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                str = "[" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "]";
            }

            if (ddldate.SelectedItem.Value.ToString() == "0")
            {
                str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report For " + oconverter.ArrangeDate2(dtfor.Value.ToString());
            }
            else
            {
                str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report Period " + oconverter.ArrangeDate2(dtfrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtto.Value.ToString());
            }

            DrRowR1[0] = "F&O Portfolio Performance Report:" + str;

            dtReportHeader.Rows.Add(DrRowR1);
            DataRow DrRowR2 = dtReportHeader.NewRow();
            //DrRowR2[0] = txtBankName.Text;
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

            ViewState["dtExport"] = dtExport;
            ViewState["dtReportHeader"] = dtReportHeader;
            ViewState["dtReportFooter"] = dtReportFooter;
        }
        void export_rpttypescripwise_detail()
        {
            ds = (DataSet)ViewState["dataset"];
            DataTable dtExport = new DataTable();
            dtExport.Clear();
            dtExport.Columns.Add("Scrip Name", Type.GetType("System.String"));
            dtExport.Columns.Add("UCC", Type.GetType("System.String"));
            if (ddldate.SelectedItem.Value.ToString() == "1")
            {
                if (ChkBFQty.Checked == false)
                {
                    dtExport.Columns.Add("B/F Qty", Type.GetType("System.String"));
                }
            }
            dtExport.Columns.Add("Buy Qty", Type.GetType("System.String"));
            dtExport.Columns.Add("Sell Qty", Type.GetType("System.String"));
            dtExport.Columns.Add("Booked P/L", Type.GetType("System.String"));
            if (chknetpremium.Checked)
            {
                dtExport.Columns.Add("Premium", Type.GetType("System.String"));
            }
            dtExport.Columns.Add("C/F Qty", Type.GetType("System.String"));
            dtExport.Columns.Add("Avg.Cost (Residual)", Type.GetType("System.String"));
            if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
            {
                dtExport.Columns.Add("Close Price (Instrmnt)", Type.GetType("System.String"));
            }
            else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
            {
                dtExport.Columns.Add("Close Price (Asset)", Type.GetType("System.String"));
            }
            else
            {
                dtExport.Columns.Add("Close Price (Instr)", Type.GetType("System.String"));
                dtExport.Columns.Add("Close Price (Asset)", Type.GetType("System.String"));
            }
            if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
            {
                dtExport.Columns.Add("MTM (Instr.Close)", Type.GetType("System.String"));
            }
            else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
            {
                dtExport.Columns.Add("MTM (Asset.Close)", Type.GetType("System.String"));
            }
            else
            {
                dtExport.Columns.Add("MTM (Instr.Close)", Type.GetType("System.String"));
                dtExport.Columns.Add("MTM (Asset.Close)", Type.GetType("System.String"));
            }
            if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
            {
                dtExport.Columns.Add("Total P/L (Instr.Close)", Type.GetType("System.String"));
            }
            else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
            {
                dtExport.Columns.Add("Total P/L (Asset.Close)", Type.GetType("System.String"));
            }
            else
            {
                dtExport.Columns.Add("Total P/L (Instr.Close)", Type.GetType("System.String"));
                dtExport.Columns.Add("Total P/L (Asset.Close)", Type.GetType("System.String"));
            }

            dtExport.Columns.Add("Exposure", Type.GetType("System.String"));


            int i = 0;

            for (int j = 0; j < cmbgroup.Items.Count; j++)
            {
                DataRow row = dtExport.NewRow();
                row[0] = "Asset Name:" + cmbgroup.Items[j].Text.ToString().Trim();
                row[1] = "Test";
                dtExport.Rows.Add(row);

                DataView viewgrp = new DataView();
                viewgrp = ds.Tables[0].DefaultView;
                viewgrp.RowFilter = "MASTERPRODUCTID='" + cmbgroup.Items[j].Value + "'";
                DataTable dt = new DataTable();
                dt = viewgrp.ToTable();

                DataView viewClient = new DataView(dt);
                Distinctclient = new DataTable();
                Distinctclient = viewClient.ToTable(true, new string[] { "PRODUCTID", "SCRIPNAME" });

                if (Distinctclient.Rows.Count > 0)
                {
                    cmbclient.Items.Clear();
                    cmbclient.DataSource = Distinctclient;
                    cmbclient.DataValueField = "PRODUCTID";
                    cmbclient.DataTextField = "SCRIPNAME";
                    cmbclient.DataBind();

                }

                for (int k = 0; k < cmbclient.Items.Count; k++)
                {
                    DataRow row1 = dtExport.NewRow();
                    row1[0] = "Scrip Name:" + cmbclient.Items[k].Text.ToString().Trim();
                    row1[1] = "Test";
                    dtExport.Rows.Add(row1);

                    DataView viewclient = new DataView();
                    viewclient = ds.Tables[0].DefaultView;
                    viewclient.RowFilter = "PRODUCTID='" + cmbclient.Items[k].Value + "'";
                    DataTable dt1 = new DataTable();
                    dt1 = viewclient.ToTable();


                    for (i = 0; i < dt1.Rows.Count; i++)
                    {
                        ////////////////////////ALL DATA BEGIN
                        DataRow row6 = dtExport.NewRow();
                        row6[0] = dt1.Rows[i]["CLIENTNAME"].ToString();
                        row6[1] = dt1.Rows[i]["UCC"].ToString();
                        if (ddldate.SelectedItem.Value.ToString() == "1")
                        {
                            if (ChkBFQty.Checked == false)
                            {
                                if (dt1.Rows[i]["BFQTY"] != DBNull.Value)
                                    row6["B/F Qty"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["BFQTY"].ToString()));
                            }
                        }
                        if (dt1.Rows[i]["BUYQTY"] != DBNull.Value)
                            row6["Buy Qty"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["BUYQTY"].ToString()));

                        if (dt1.Rows[i]["SELLQTY"] != DBNull.Value)
                            row6["Sell Qty"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["SELLQTY"].ToString()));

                        if (dt1.Rows[i]["BOOKEDPL"] != DBNull.Value)
                            row6["Booked P/L"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["BOOKEDPL"].ToString()));

                        if (chknetpremium.Checked)
                        {
                            if (dt1.Rows[i]["Premium"] != DBNull.Value)
                                row6["Premium"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["Premium"].ToString()));

                        }

                        if (dt1.Rows[i]["CFQTY"] != DBNull.Value)
                            row6["C/F Qty"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["CFQTY"].ToString()));

                        if (dt1.Rows[i]["AVGCOSTRESEDUAL"] != DBNull.Value)
                            row6["Avg.Cost (Residual)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["AVGCOSTRESEDUAL"].ToString()));

                        if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
                        {
                            if (dt1.Rows[i]["CLOSEPRICEINSTRU"] != DBNull.Value)
                                row6["Close Price (Instrmnt)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["CLOSEPRICEINSTRU"].ToString()));

                        }
                        else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
                        {
                            if (dt1.Rows[i]["CLOSEPRICEASSET"] != DBNull.Value)
                                row6["Close Price (Asset)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["CLOSEPRICEASSET"].ToString()));

                        }
                        else
                        {
                            if (dt1.Rows[i]["CLOSEPRICEINSTRU"] != DBNull.Value)
                                row6["Close Price (Instr)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["CLOSEPRICEINSTRU"].ToString()));

                            if (dt1.Rows[i]["CLOSEPRICEASSET"] != DBNull.Value)
                                row6["Close Price (Asset)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["CLOSEPRICEASSET"].ToString()));

                        }

                        if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
                        {
                            if (dt1.Rows[i]["MTMINSTRUCLOSE"] != DBNull.Value)
                                row6["MTM (Instr.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["MTMINSTRUCLOSE"].ToString()));

                        }
                        else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
                        {
                            if (dt1.Rows[i]["MTMASSETCLOSE"] != DBNull.Value)
                                row6["MTM (Asset.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["MTMASSETCLOSE"].ToString()));

                        }
                        else
                        {
                            if (dt1.Rows[i]["MTMINSTRUCLOSE"] != DBNull.Value)
                                row6["MTM (Instr.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["MTMINSTRUCLOSE"].ToString()));


                            if (dt1.Rows[i]["MTMASSETCLOSE"] != DBNull.Value)
                                row6["MTM (Asset.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["MTMASSETCLOSE"].ToString()));

                        }


                        if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
                        {
                            if (dt1.Rows[i]["TOTALPLINSTRUCLOSE"] != DBNull.Value)
                                row6["Total P/L (Instr.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["TOTALPLINSTRUCLOSE"].ToString()));
                        }
                        else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
                        {
                            if (dt1.Rows[i]["TOTALPLASSETCLOSE"] != DBNull.Value)
                                row6["Total P/L (Asset.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["TOTALPLASSETCLOSE"].ToString()));
                        }
                        else
                        {
                            if (dt1.Rows[i]["TOTALPLINSTRUCLOSE"] != DBNull.Value)
                                row6["Total P/L (Instr.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["TOTALPLINSTRUCLOSE"].ToString()));

                            if (dt1.Rows[i]["TOTALPLASSETCLOSE"] != DBNull.Value)
                                row6["Total P/L (Asset.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["TOTALPLASSETCLOSE"].ToString()));
                        }

                        if (dt1.Rows[i]["Exposure"] != DBNull.Value)
                            row6["Exposure"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["EXPOSURE"].ToString()));

                        dtExport.Rows.Add(row6);
                        //////////////////////ALL DATA END
                    }
                    //////////////////////////////////////////scrip total
                    DataRow blankrow = dtExport.NewRow();
                    dtExport.Rows.Add(blankrow);

                    DataRow row3 = dtExport.NewRow();
                    row3[0] = "Scrip Total";

                    if (ddldate.SelectedItem.Value.ToString() == "1")
                    {
                        if (ChkBFQty.Checked == false)
                        {
                            if (dt1.Rows[i - 1]["MASTERPRODUCTID_BFQTY"] != DBNull.Value)
                                row3["B/F Qty"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i - 1]["MASTERPRODUCTID_BFQTY"].ToString()));
                        }
                    }

                    if (dt1.Rows[i - 1]["MASTERPRODUCTID_BUYQTY"] != DBNull.Value)
                        row3["Buy Qty"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i - 1]["MASTERPRODUCTID_BUYQTY"].ToString()));

                    if (dt1.Rows[i - 1]["MASTERPRODUCTID_SELLQTY"] != DBNull.Value)
                        row3["Sell Qty"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i - 1]["MASTERPRODUCTID_SELLQTY"].ToString()));

                    if (dt1.Rows[i - 1]["MASTERPRODUCTID_BOOKEDPL"] != DBNull.Value)
                        row3["Booked P/L"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i - 1]["MASTERPRODUCTID_BOOKEDPL"].ToString()));

                    if (chknetpremium.Checked)
                    {
                        if (dt1.Rows[i - 1]["MASTERPRODUCTID_Premium"] != DBNull.Value)
                            row3["Premium"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i - 1]["MASTERPRODUCTID_Premium"].ToString()));

                    }

                    if (dt1.Rows[i - 1]["MASTERPRODUCTID_CFQTY"] != DBNull.Value)
                        row3["C/F Qty"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i - 1]["MASTERPRODUCTID_CFQTY"].ToString()));

                    if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
                    {
                        if (dt1.Rows[i - 1]["MASTERPRODUCTID_MTMINSTRUCLOSE"] != DBNull.Value)
                            row3["MTM (Instr.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i - 1]["MASTERPRODUCTID_MTMINSTRUCLOSE"].ToString()));
                    }
                    else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
                    {
                        if (dt1.Rows[i - 1]["MASTERPRODUCTID_MTMASSETCLOSE"] != DBNull.Value)
                            row3["MTM (Asset.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i - 1]["MASTERPRODUCTID_MTMASSETCLOSE"].ToString()));
                    }
                    else
                    {
                        if (dt1.Rows[i - 1]["MASTERPRODUCTID_MTMINSTRUCLOSE"] != DBNull.Value)
                            row3["MTM (Instr.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i - 1]["MASTERPRODUCTID_MTMINSTRUCLOSE"].ToString()));

                        if (dt1.Rows[i - 1]["MASTERPRODUCTID_MTMASSETCLOSE"] != DBNull.Value)
                            row3["MTM (Asset.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i - 1]["MASTERPRODUCTID_MTMASSETCLOSE"].ToString()));

                    }
                    if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
                    {

                        if (dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLINSTRUCLOSE"] != DBNull.Value)
                            row3["Total P/L (Instr.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLINSTRUCLOSE"].ToString()));

                    }
                    else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
                    {
                        if (dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLASSETCLOSE"] != DBNull.Value)
                            row3["Total P/L (Asset.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLASSETCLOSE"].ToString()));

                    }
                    else
                    {

                        if (dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLINSTRUCLOSE"] != DBNull.Value)
                            row3["Total P/L (Instr.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLINSTRUCLOSE"].ToString()));

                        if (dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLASSETCLOSE"] != DBNull.Value)
                            row3["Total P/L (Asset.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLASSETCLOSE"].ToString()));

                    }
                    if (dt1.Rows[i - 1]["MASTERPRODUCTID_EXPOSURE"] != DBNull.Value)
                        row3["Exposure"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i - 1]["MASTERPRODUCTID_EXPOSURE"].ToString()));

                    dtExport.Rows.Add(row3);
                }
                ////////////group total
                DataRow row5 = dtExport.NewRow();
                row5[0] = "Asset Total :";
                if (ddldate.SelectedItem.Value.ToString() == "1")
                {
                    if (ChkBFQty.Checked == false)
                    {
                        if (dt.Rows[0]["GROUP_BFQTY"] != DBNull.Value)
                            row5["B/F Qty"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt.Rows[0]["GROUP_BFQTY"].ToString()));
                    }
                }

                if (dt.Rows[0]["GROUP_BUYQTY"] != DBNull.Value)
                    row5["Buy Qty"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt.Rows[0]["GROUP_BUYQTY"].ToString()));

                if (dt.Rows[0]["GROUP_SELLQTY"] != DBNull.Value)
                    row5["Sell Qty"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt.Rows[0]["GROUP_SELLQTY"].ToString()));

                if (dt.Rows[0]["GROUP_BOOKEDPL"] != DBNull.Value)
                    row5["Booked P/L"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt.Rows[0]["GROUP_BOOKEDPL"].ToString()));

                if (chknetpremium.Checked)
                {
                    if (dt.Rows[0]["GROUP_Premium"] != DBNull.Value)
                        row5["Premium"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt.Rows[0]["GROUP_Premium"].ToString()));

                }

                if (dt.Rows[0]["GROUP_CFQTY"] != DBNull.Value)
                    row5["C/F Qty"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt.Rows[0]["GROUP_CFQTY"].ToString()));


                if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
                {
                    if (dt.Rows[0]["GROUP_MTMINSTRUCLOSE"] != DBNull.Value)
                        row5["MTM (Instr.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt.Rows[0]["GROUP_MTMINSTRUCLOSE"].ToString()));
                }
                else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
                {
                    if (dt.Rows[0]["GROUP_MTMASSETCLOSE"] != DBNull.Value)
                        row5["MTM (Asset.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt.Rows[0]["GROUP_MTMASSETCLOSE"].ToString()));
                }
                else
                {
                    if (dt.Rows[0]["CUSTOMERID_MTMINSTRUCLOSE"] != DBNull.Value)
                        row5["MTM (Instr.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt.Rows[0]["CUSTOMERID_MTMINSTRUCLOSE"].ToString()));

                    if (dt.Rows[0]["CUSTOMERID_MTMASSETCLOSE"] != DBNull.Value)
                        row5["MTM (Asset.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt.Rows[0]["CUSTOMERID_MTMASSETCLOSE"].ToString()));

                }
                if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
                {

                    if (dt.Rows[0]["GROUP_TOTALPLINSTRUCLOSE"] != DBNull.Value)
                        row5["Total P/L (Instr.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt.Rows[0]["GROUP_TOTALPLINSTRUCLOSE"].ToString()));

                }
                else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
                {
                    if (dt.Rows[0]["GROUP_TOTALPLASSETCLOSE"] != DBNull.Value)
                        row5["Total P/L (Asset.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt.Rows[0]["GROUP_TOTALPLASSETCLOSE"].ToString()));

                }
                else
                {

                    if (dt.Rows[0]["GROUP_TOTALPLINSTRUCLOSE"] != DBNull.Value)
                        row5["Total P/L (Instr.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt.Rows[0]["GROUP_TOTALPLINSTRUCLOSE"].ToString()));

                    if (dt.Rows[0]["GROUP_TOTALPLASSETCLOSE"] != DBNull.Value)
                        row5["Total P/L (Asset.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt.Rows[0]["GROUP_TOTALPLASSETCLOSE"].ToString()));

                }
                if (dt.Rows[0]["GROUP_Exposure"] != DBNull.Value)
                    row5["Exposure"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt.Rows[0]["GROUP_Exposure"].ToString()));

                dtExport.Rows.Add(row5);
            }

            DataTable dtReportHeader = new DataTable();
            DataTable dtReportFooter = new DataTable();

            DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            HeaderRow[0] = CompanyName.Rows[0][0].ToString();
            dtReportHeader.Rows.Add(HeaderRow);
            DataRow DrRowR1 = dtReportHeader.NewRow();

            string str = null;
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                str = "[" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "]";
            }

            if (ddldate.SelectedItem.Value.ToString() == "0")
            {
                str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report For " + oconverter.ArrangeDate2(dtfor.Value.ToString());
            }
            else
            {
                str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report Period " + oconverter.ArrangeDate2(dtfrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtto.Value.ToString());
            }

            DrRowR1[0] = "F&O Portfolio Performance Report:" + str;

            dtReportHeader.Rows.Add(DrRowR1);
            DataRow DrRowR2 = dtReportHeader.NewRow();
            //DrRowR2[0] = txtBankName.Text;
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

            ViewState["dtExport"] = dtExport;
            ViewState["dtReportHeader"] = dtReportHeader;
            ViewState["dtReportFooter"] = dtReportFooter;
        }
        void export_rpttypescripwise_summary()
        {
            ds = (DataSet)ViewState["dataset"];
            DataTable dtExport = new DataTable();
            dtExport.Clear();
            dtExport.Columns.Add("Scrip Name", Type.GetType("System.String"));
            if (ddldate.SelectedItem.Value.ToString() == "1")
            {
                if (ChkBFQty.Checked == false)
                {
                    dtExport.Columns.Add("B/F Qty", Type.GetType("System.String"));
                }
            }
            dtExport.Columns.Add("Buy Qty", Type.GetType("System.String"));
            dtExport.Columns.Add("Sell Qty", Type.GetType("System.String"));
            dtExport.Columns.Add("Booked P/L", Type.GetType("System.String"));
            if (chknetpremium.Checked)
            {
                dtExport.Columns.Add("Premium", Type.GetType("System.String"));

            }

            dtExport.Columns.Add("C/F Qty", Type.GetType("System.String"));
            if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
            {
                dtExport.Columns.Add("MTM (Instr.Close)", Type.GetType("System.String"));
            }
            else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
            {
                dtExport.Columns.Add("MTM (Asset.Close)", Type.GetType("System.String"));
            }
            else
            {
                dtExport.Columns.Add("MTM (Instr.Close)", Type.GetType("System.String"));
                dtExport.Columns.Add("MTM (Asset.Close)", Type.GetType("System.String"));
            }
            if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
            {
                dtExport.Columns.Add("Total P/L (Instr.Close)", Type.GetType("System.String"));
            }
            else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
            {
                dtExport.Columns.Add("Total P/L (Asset.Close)", Type.GetType("System.String"));
            }
            else
            {
                dtExport.Columns.Add("Total P/L (Instr.Close)", Type.GetType("System.String"));
                dtExport.Columns.Add("Total P/L (Asset.Close)", Type.GetType("System.String"));
            }

            dtExport.Columns.Add("Exposure", Type.GetType("System.String"));


            int i;

            for (int j = 0; j < cmbgroup.Items.Count; j++)
            {
                DataRow row = dtExport.NewRow();
                row[0] = "Asset Name:" + cmbgroup.Items[j].Text.ToString().Trim();
                row[1] = "Test";
                dtExport.Rows.Add(row);

                DataView viewgrp = new DataView();
                viewgrp = ds.Tables[0].DefaultView;
                viewgrp.RowFilter = "MASTERPRODUCTID='" + cmbgroup.Items[j].Value + "'";
                DataTable dt1 = new DataTable();
                dt1 = viewgrp.ToTable();



                for (i = 0; i < dt1.Rows.Count; i++)
                {

                    DataRow row4 = dtExport.NewRow();
                    row4[0] = dt1.Rows[i]["SCRIPNAME"].ToString().Trim();
                    if (ddldate.SelectedItem.Value.ToString() == "1")
                    {
                        if (ChkBFQty.Checked == false)
                        {
                            if (dt1.Rows[i]["MASTERPRODUCTID_BFQTY"] != DBNull.Value)
                                row4["B/F Qty"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["MASTERPRODUCTID_BFQTY"].ToString()));
                        }
                    }
                    if (dt1.Rows[i]["MASTERPRODUCTID_BUYQTY"] != DBNull.Value)
                        row4["Buy Qty"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["MASTERPRODUCTID_BUYQTY"].ToString()));
                    if (dt1.Rows[i]["MASTERPRODUCTID_SELLQTY"] != DBNull.Value)
                        row4["Sell Qty"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["MASTERPRODUCTID_SELLQTY"].ToString()));
                    if (dt1.Rows[i]["MASTERPRODUCTID_BOOKEDPL"] != DBNull.Value)
                        row4["Booked P/L"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["MASTERPRODUCTID_BOOKEDPL"].ToString()));

                    if (chknetpremium.Checked)
                    {
                        if (dt1.Rows[i]["MASTERPRODUCTID_Premium"] != DBNull.Value)
                            row4["Premium"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["MASTERPRODUCTID_Premium"].ToString()));
                    }

                    if (dt1.Rows[i]["MASTERPRODUCTID_CFQTY"] != DBNull.Value)
                        row4["C/F Qty"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["MASTERPRODUCTID_CFQTY"].ToString()));

                    if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
                    {
                        if (dt1.Rows[i]["MASTERPRODUCTID_MTMINSTRUCLOSE"] != DBNull.Value)
                            row4["MTM (Instr.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["MASTERPRODUCTID_MTMINSTRUCLOSE"].ToString()));
                    }
                    else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
                    {
                        if (dt1.Rows[i]["MASTERPRODUCTID_MTMASSETCLOSE"] != DBNull.Value)
                            row4["MTM (Asset.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["MASTERPRODUCTID_MTMASSETCLOSE"].ToString()));
                    }
                    else
                    {
                        if (dt1.Rows[i]["MASTERPRODUCTID_MTMINSTRUCLOSE"] != DBNull.Value)
                            row4["MTM (Instr.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["MASTERPRODUCTID_MTMINSTRUCLOSE"].ToString()));

                        if (dt1.Rows[i]["MASTERPRODUCTID_MTMASSETCLOSE"] != DBNull.Value)
                            row4["MTM (Asset.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["MASTERPRODUCTID_MTMASSETCLOSE"].ToString()));

                    }
                    if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
                    {

                        if (dt1.Rows[i]["MASTERPRODUCTID_TOTALPLINSTRUCLOSE"] != DBNull.Value)
                            row4["Total P/L (Instr.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["MASTERPRODUCTID_TOTALPLINSTRUCLOSE"].ToString()));

                    }
                    else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
                    {
                        if (dt1.Rows[i]["MASTERPRODUCTID_TOTALPLASSETCLOSE"] != DBNull.Value)
                            row4["Total P/L (Asset.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["MASTERPRODUCTID_TOTALPLASSETCLOSE"].ToString()));

                    }
                    else
                    {

                        if (dt1.Rows[i]["MASTERPRODUCTID_TOTALPLINSTRUCLOSE"] != DBNull.Value)
                            row4["Total P/L (Instr.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["MASTERPRODUCTID_TOTALPLINSTRUCLOSE"].ToString()));

                        if (dt1.Rows[i]["MASTERPRODUCTID_TOTALPLASSETCLOSE"] != DBNull.Value)
                            row4["Total P/L (Asset.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["MASTERPRODUCTID_TOTALPLASSETCLOSE"].ToString()));

                    }
                    if (dt1.Rows[i]["MASTERPRODUCTID_EXPOSURE"] != DBNull.Value)
                        row4["Exposure"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["MASTERPRODUCTID_EXPOSURE"].ToString()));

                    dtExport.Rows.Add(row4);

                }

                ////////////group total
                DataRow row5 = dtExport.NewRow();
                row5[0] = "Group Total :";

                if (dt1.Rows[0]["GROUP_BOOKEDPL"] != DBNull.Value)
                    row5["Booked P/L"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["GROUP_BOOKEDPL"].ToString()));

                if (chknetpremium.Checked)
                {
                    if (dt1.Rows[0]["GROUP_Premium"] != DBNull.Value)
                        row5["Premium"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["GROUP_Premium"].ToString()));
                }

                if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
                {
                    if (dt1.Rows[0]["GROUP_MTMINSTRUCLOSE"] != DBNull.Value)
                        row5["MTM (Instr.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["GROUP_MTMINSTRUCLOSE"].ToString()));
                }
                else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
                {
                    if (dt1.Rows[0]["GROUP_MTMASSETCLOSE"] != DBNull.Value)
                        row5["MTM (Asset.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["GROUP_MTMASSETCLOSE"].ToString()));
                }
                else
                {
                    if (dt1.Rows[0]["GROUP_MTMINSTRUCLOSE"] != DBNull.Value)
                        row5["MTM (Instr.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["GROUP_MTMINSTRUCLOSE"].ToString()));

                    if (dt1.Rows[0]["GROUP_MTMASSETCLOSE"] != DBNull.Value)
                        row5["MTM (Asset.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["GROUP_MTMASSETCLOSE"].ToString()));

                }
                if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
                {

                    if (dt1.Rows[0]["GROUP_TOTALPLINSTRUCLOSE"] != DBNull.Value)
                        row5["Total P/L (Instr.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["GROUP_TOTALPLINSTRUCLOSE"].ToString()));

                }
                else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
                {
                    if (dt1.Rows[0]["GROUP_TOTALPLASSETCLOSE"] != DBNull.Value)
                        row5["Total P/L (Asset.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["GROUP_TOTALPLASSETCLOSE"].ToString()));

                }
                else
                {

                    if (dt1.Rows[0]["GROUP_TOTALPLINSTRUCLOSE"] != DBNull.Value)
                        row5["Total P/L (Instr.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["GROUP_TOTALPLINSTRUCLOSE"].ToString()));

                    if (dt1.Rows[0]["GROUP_TOTALPLASSETCLOSE"] != DBNull.Value)
                        row5["Total P/L (Asset.Close)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["GROUP_TOTALPLASSETCLOSE"].ToString()));

                }
                if (dt1.Rows[0]["GROUP_Exposure"] != DBNull.Value)
                    row5["Exposure"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["GROUP_Exposure"].ToString()));

                dtExport.Rows.Add(row5);

            }


            DataTable dtReportHeader = new DataTable();
            DataTable dtReportFooter = new DataTable();

            DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            HeaderRow[0] = CompanyName.Rows[0][0].ToString();
            dtReportHeader.Rows.Add(HeaderRow);
            DataRow DrRowR1 = dtReportHeader.NewRow();

            string str = null;
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                str = "[" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "]";
            }

            if (ddldate.SelectedItem.Value.ToString() == "0")
            {
                str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report For " + oconverter.ArrangeDate2(dtfor.Value.ToString());
            }
            else
            {
                str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report Period " + oconverter.ArrangeDate2(dtfrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtto.Value.ToString());
            }

            DrRowR1[0] = "F&O Portfolio Performance Report:" + str;

            dtReportHeader.Rows.Add(DrRowR1);
            DataRow DrRowR2 = dtReportHeader.NewRow();
            //DrRowR2[0] = txtBankName.Text;
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

            ViewState["dtExport"] = dtExport;
            ViewState["dtReportHeader"] = dtReportHeader;
            ViewState["dtReportFooter"] = dtReportFooter;
        }


        protected void cmbgroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlrptview.SelectedItem.Value.ToString() == "0")
            {
                if (ddlrpttype.SelectedItem.Value.ToString().Trim() == "0")
                {
                    ddlbandforClient();
                }
                else
                {
                    htmltable_rpttypeclientwise_summary();
                }
            }
            else
            {
                if (ddlrpttype.SelectedItem.Value.ToString().Trim() == "0")
                {
                    ddlbandforScrip();
                }
                else
                {
                    htmltable_rpttypescripwise_summary();
                }
            }
        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlrptview.SelectedItem.Value.ToString().Trim() == "0")
            {
                if (ddlrpttype.SelectedItem.Value.ToString().Trim() == "0")
                {
                    export_rpttypeclientwise_detail();
                }
                else
                {
                    export_rpttypeclientwise_summary();
                }
                dtExport = (DataTable)ViewState["dtExport"];
                dtReportHeader = (DataTable)ViewState["dtReportHeader"];
                dtReportFooter = (DataTable)ViewState["dtReportFooter"];

                if (cmbExport.SelectedItem.Value == "E")
                {
                    objExcel.ExportToExcelforExcel(dtExport, "F&O Portfolio Performance Report", "Asset Total", dtReportHeader, dtReportFooter);
                }
            }
            else
            {
                if (ddlrpttype.SelectedItem.Value.ToString().Trim() == "0")
                {
                    export_rpttypescripwise_detail();
                }
                else
                {
                    export_rpttypescripwise_summary();
                }
                dtExport = (DataTable)ViewState["dtExport"];
                dtReportHeader = (DataTable)ViewState["dtReportHeader"];
                dtReportFooter = (DataTable)ViewState["dtReportFooter"];

                if (cmbExport.SelectedItem.Value == "E")
                {
                    objExcel.ExportToExcelforExcel(dtExport, "F&O Portfolio Performance Report", "Asset Total :", dtReportHeader, dtReportFooter);
                }
            }
        }
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            procedure();

            if (ddlrptview.SelectedItem.Value.ToString().Trim() == "0")
            {
                ddlbandforgroup();
                if (ddlrpttype.SelectedItem.Value.ToString().Trim() == "0")
                {
                    export_rpttypeclientwise_detail();
                }
                else
                {
                    export_rpttypeclientwise_summary();
                }
                dtExport = (DataTable)ViewState["dtExport"];
                dtReportHeader = (DataTable)ViewState["dtReportHeader"];
                dtReportFooter = (DataTable)ViewState["dtReportFooter"];

                objExcel.ExportToExcelforExcel(dtExport, "F&O Portfolio Performance Report", "Asset Total", dtReportHeader, dtReportFooter);

            }
            else
            {
                ddlbandforasset();
                if (ddlrpttype.SelectedItem.Value.ToString().Trim() == "0")
                {
                    export_rpttypescripwise_detail();
                }
                else
                {
                    export_rpttypescripwise_summary();
                }
                dtExport = (DataTable)ViewState["dtExport"];
                dtReportHeader = (DataTable)ViewState["dtReportHeader"];
                dtReportFooter = (DataTable)ViewState["dtReportFooter"];

                objExcel.ExportToExcelforExcel(dtExport, "F&O Portfolio Performance Report", "Asset Total", dtReportHeader, dtReportFooter);

            }

        }
        protected void btnmail_Click(object sender, EventArgs e)
        {
            procedure();
            ds = (DataSet)ViewState["dataset"];
            if (ds.Tables[0].Rows.Count > 0)
            {
                mail();
            }
        }

        void mail()
        {
            if (ddldate.SelectedItem.Value.ToString() == "0")
            {
                ViewState["billdate"] = oconverter.ArrangeDate2(dtfor.Value.ToString());
            }
            else
            {
                ViewState["billdate"] = oconverter.ArrangeDate2(dtfrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtto.Value.ToString());
            }
            if (ddloptionformail.SelectedItem.Value.ToString().Trim() == "0")
            {
                clientwisemail();
            }
            if (ddloptionformail.SelectedItem.Value.ToString().Trim() == "1")
            {
                branhgroupemail();
            }
            if (ddloptionformail.SelectedItem.Value.ToString().Trim() == "2")
            {
                optionforemailclient();
            }
        }
        void clientwisemail()
        {
            ViewState["mailsendresult"] = "no";
            ds = (DataSet)ViewState["dataset"];
            DataView viewgroup = new DataView(ds.Tables[0]);
            dtgroupcontactid = viewgroup.ToTable(true, new string[] { "GRPID", "GRPNAME" });
            if (dtgroupcontactid.Rows.Count > 0)
            {
                cmbgroup.DataSource = dtgroupcontactid;
                cmbgroup.DataValueField = "GRPID";
                cmbgroup.DataTextField = "GRPNAME";
                cmbgroup.DataBind();

            }
            for (int j = 0; j < cmbgroup.Items.Count; j++)
            {
                ds = (DataSet)ViewState["dataset"];
                DataView viewgrp = new DataView();
                viewgrp = ds.Tables[0].DefaultView;
                viewgrp.RowFilter = "GRPID='" + cmbgroup.Items[j].Value + "'";
                DataTable dt = new DataTable();
                dt = viewgrp.ToTable();

                DataView viewClient = new DataView(dt);
                Distinctclient = viewClient.ToTable(true, new string[] { "CUSTOMERID", "CLIENTNAME" });

                if (Distinctclient.Rows.Count > 0)
                {
                    cmbclient.DataSource = Distinctclient;
                    cmbclient.DataValueField = "CUSTOMERID";
                    cmbclient.DataTextField = "CLIENTNAME";
                    cmbclient.DataBind();

                }
                for (int k = 0; k < cmbclient.Items.Count; k++)
                {
                    if (ddlrpttype.SelectedItem.Value == "0")
                    {
                        htmltable_rpttypecleintwise_detail1(cmbclient.Items[k].Value, cmbclient.Items[k].Text.ToString().Trim(), cmbgroup.Items[j].Value.ToString().Trim(), dt.Rows[0]["GRPNAME"].ToString().Trim());
                    }
                    else
                    {
                        htmltable_rpttypescripwise_detail(cmbclient.Items[k].Value, cmbclient.Items[k].Text.ToString().Trim(), cmbgroup.Items[j].Value.ToString().Trim(), dt.Rows[0]["GRPNAME"].ToString().Trim());
                    }
                    if (oDBEngine.SendReport(ViewState["mail"].ToString().Trim(), cmbclient.Items[k].Value, ViewState["billdate"].ToString().Trim(), "Portfolio Performance [" + ViewState["billdate"].ToString().Trim() + "]") == true)
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
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript70", "<script language='javascript'>alert('Mail Sent Successfully !!'+'\n'+'Emails not Sent For Clients Without Email-Id ...');</script>");
            }
            if (ViewState["mailsendresult"].ToString().Trim() == "success")
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript80", "<script language='javascript'>alert('Mail Sent Successfully !!');</script>");
            }
            if (ViewState["mailsendresult"].ToString().Trim() == "errorsuccess")
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript90", "<script language='javascript'>alert('Error on sending!Try again..');</script>");
            }
            Page.ClientScript.RegisterStartupScript(GetType(), "JScript32", "<script language='javascript'>Page_Load();</script>");
            //Page.ClientScript.RegisterStartupScript(GetType(), "JScript34", "<script language='javascript'>alert('Mail Sent Successfully');</script>");
        }
        void branhgroupemail()
        {
            ViewState["GRPmail"] = "mail";
            ViewState["mailsendresult"] = "no";
            ds = (DataSet)ViewState["dataset"];
            DataView viewgroup = new DataView(ds.Tables[0]);
            dtgroupcontactid = viewgroup.ToTable(true, new string[] { "GRPID", "GRPNAME" });
            if (dtgroupcontactid.Rows.Count > 0)
            {
                cmbgroup.DataSource = dtgroupcontactid;
                cmbgroup.DataValueField = "GRPID";
                cmbgroup.DataTextField = "GRPNAME";
                cmbgroup.DataBind();

            }
            for (int j = 0; j < cmbgroup.Items.Count; j++)
            {
                ds = (DataSet)ViewState["dataset"];
                DataView viewgrp = new DataView();
                viewgrp = ds.Tables[0].DefaultView;
                viewgrp.RowFilter = "GRPID='" + cmbgroup.Items[j].Value + "'";
                DataTable dt = new DataTable();
                dt = viewgrp.ToTable();

                DataView viewClient = new DataView(dt);
                Distinctclient = viewClient.ToTable(true, new string[] { "CUSTOMERID", "CLIENTNAME" });

                if (Distinctclient.Rows.Count > 0)
                {
                    cmbclient.DataSource = Distinctclient;
                    cmbclient.DataValueField = "CUSTOMERID";
                    cmbclient.DataTextField = "CLIENTNAME";
                    cmbclient.DataBind();

                }
                for (int k = 0; k < cmbclient.Items.Count; k++)
                {
                    htmltable_rpttypecleintwise_detail2(cmbclient.Items[k].Value, cmbclient.Items[k].Text.ToString().Trim(), cmbgroup.Items[j].Value.ToString().Trim(), dt.Rows[0]["GRPNAME"].ToString().Trim());
                    if (ViewState["GRPmail"].ToString().Trim() == "mail")
                    {
                        ViewState["GRPmail"] = ViewState["mail"].ToString().Trim();
                    }
                    else
                    {
                        ViewState["GRPmail"] = ViewState["GRPmail"].ToString().Trim() + ViewState["mail"].ToString().Trim();
                    }

                }
                if (ddlGroup.SelectedItem.Value == "1")
                {
                    if (oDBEngine.SendReportBrportfoliogroup(ViewState["GRPmail"].ToString().Trim(), cmbgroup.Items[j].Text.ToString().Trim(), ViewState["billdate"].ToString().Trim(), "Portfolio Performance [" + ViewState["billdate"].ToString().Trim() + "]", cmbgroup.Items[j].Value) == true)
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
                    ViewState["GRPmail"] = "mail";
                }
                if (oDBEngine.SendReportBrportfolio(ViewState["GRPmail"].ToString().Trim(), cmbgroup.Items[j].Text.ToString().Trim(), ViewState["billdate"].ToString().Trim(), "Portfolio Performance [" + ViewState["billdate"].ToString().Trim() + "]", cmbgroup.Items[j].Value) == true)
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
                ViewState["GRPmail"] = "mail";
            }


            if (ViewState["mailsendresult"].ToString().Trim() == "someclienterror")
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript40", "<script language='javascript'>alert('Mail Sent Successfully !!'+'\n'+'Emails not Sent For Clients Without Email-Id ...');</script>");
            }
            if (ViewState["mailsendresult"].ToString().Trim() == "success")
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript50", "<script language='javascript'>alert('Mail Sent Successfully !!');</script>");
            }
            if (ViewState["mailsendresult"].ToString().Trim() == "errorsuccess")
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript60", "<script language='javascript'>alert('Error on sending!Try again..');</script>");
            }
            Page.ClientScript.RegisterStartupScript(GetType(), "JScript12", "<script language='javascript'>Page_Load();</script>");
        }
        void optionforemailclient()
        {
            if (HiddenField_emmail.Value.ToString().Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD", "NORECORD(5);", true);
            }
            else
            {
                ViewState["GRPmail"] = "mail";
                ViewState["Usermail"] = "UserMail";
                ViewState["mailsendresult"] = "no";
                ds = (DataSet)ViewState["dataset"];
                DataView viewgroup = new DataView(ds.Tables[0]);
                dtgroupcontactid = viewgroup.ToTable(true, new string[] { "GRPID", "GRPNAME" });
                if (dtgroupcontactid.Rows.Count > 0)
                {
                    cmbgroup.DataSource = dtgroupcontactid;
                    cmbgroup.DataValueField = "GRPID";
                    cmbgroup.DataTextField = "GRPNAME";
                    cmbgroup.DataBind();

                }
                for (int j = 0; j < cmbgroup.Items.Count; j++)
                {
                    ds = (DataSet)ViewState["dataset"];
                    DataView viewgrp = new DataView();
                    viewgrp = ds.Tables[0].DefaultView;
                    viewgrp.RowFilter = "GRPID='" + cmbgroup.Items[j].Value + "'";
                    DataTable dt = new DataTable();
                    dt = viewgrp.ToTable();

                    DataView viewClient = new DataView(dt);
                    Distinctclient = viewClient.ToTable(true, new string[] { "CUSTOMERID", "CLIENTNAME" });

                    if (Distinctclient.Rows.Count > 0)
                    {
                        cmbclient.DataSource = Distinctclient;
                        cmbclient.DataValueField = "CUSTOMERID";
                        cmbclient.DataTextField = "CLIENTNAME";
                        cmbclient.DataBind();

                    }
                    for (int k = 0; k < cmbclient.Items.Count; k++)
                    {
                        htmltable_rpttypecleintwise_detail2(cmbclient.Items[k].Value, cmbclient.Items[k].Text.ToString().Trim(), cmbgroup.Items[j].Value.ToString().Trim(), dt.Rows[0]["GRPNAME"].ToString().Trim());
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
                    if (oDBEngine.SendReportSt(ViewState["Usermail"].ToString().Trim(), clnt[i].ToString().Trim(), ViewState["billdate"].ToString().Trim(), "Portfolio Performance [" + ViewState["billdate"].ToString().Trim() + "]") == true)
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
                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript10", "<script language='javascript'>alert('Mail Sent Successfully !!'+'\n'+'Emails not Sent For Clients Without Email-Id ...');</script>");
                }
                if (ViewState["mailsendresult"].ToString().Trim() == "success")
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript20", "<script language='javascript'>alert('Mail Sent Successfully !!');</script>");
                }
                if (ViewState["mailsendresult"].ToString().Trim() == "errorsuccess")
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript30", "<script language='javascript'>alert('Error on sending!Try again..');</script>");
                }
            }
            Page.ClientScript.RegisterStartupScript(GetType(), "JScript22", "<script language='javascript'>Page_Load();</script>");
            //Page.ClientScript.RegisterStartupScript(GetType(), "JScript24", "<script language='javascript'>alert('Mail Sent Successfully');</script>");
        }
        void htmltable_rpttypecleintwise_detail1(string clientid, string clientname, string grpid, string grpname)
        {
            ds = (DataSet)ViewState["dataset"];
            String strHtml = String.Empty;
            String strHtml1 = String.Empty;
            int colcount = ds.Tables[0].Columns.Count;
            string str = null;
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                str = "[" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "]";
            }

            if (ddldate.SelectedItem.Value.ToString() == "0")
            {
                str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report For " + oconverter.ArrangeDate2(dtfor.Value.ToString());
            }
            else
            {
                str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report Period " + oconverter.ArrangeDate2(dtfrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtto.Value.ToString());
            }
            strHtml1 = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtml1 += "<tr><td align=\"left\" colspan=" + colcount + " style=\"color:Blue;\">" + str + "</td></tr></table>";

            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

            DataView viewclient = new DataView();
            viewclient = ds.Tables[0].DefaultView;
            viewclient.RowFilter = "CUSTOMERID='" + clientid.ToString().Trim() + "'";
            DataTable dt1 = new DataTable();
            dt1 = viewclient.ToTable();


            strHtml += "<tr style=\"background-color:lavender ;text-align:left\">";
            strHtml += "<td align=\"left\" colspan=" + colcount + " nowrap=\"nowrap;\">Client Name :&nbsp;<b>" + clientname.ToString().Trim() + "</b>[ <b>" + dt1.Rows[0]["UCC"].ToString() + " </b> ]</td></tr>";

            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Asset</b></td>";
            strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Instr. </br> Type</b></td>";
            strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Expiry </br> Date</b></td>";
            strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Strike </br> Price</b></td>";
            if (ddldate.SelectedItem.Value.ToString() == "1")
            {
                if (ChkBFQty.Checked == false)
                {
                    strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>B/F Qty</b></td>";
                }
            }
            strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Buy </br> Qty</b></td>";
            strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Sell </br> Qty</b></td>";
            strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Booked </br> P/L</b></td>";
            if (chknetpremium.Checked)
            {
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Premium</b></td>";
            }
            strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>C/F  Qty</b></td>";
            strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Avg. </br> Cost </br> (Residual)</b></td>";
            if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
            {
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"> <b>Close </br> Price </br> (Instrmnt)</b></td>";
            }
            else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
            {
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Close </br> Price </br> (Asset)</b></td>";
            }
            else
            {
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Close </br> Price </br> (Instrmnt)</b></td>";
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Close </br> Price </br> (Asset)</b></td>";
            }
            if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
            {
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>MTM </br>(Instr.</br> Close)</b></td>";
            }
            else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
            {
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>MTM </br>(Asset. </br> Close)</b></td>";
            }
            else
            {
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>MTM </br>(Instr.</br> Close)</b></td>";
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>MTM </br>(Asset.</br> Close)</b></td>";
            }
            if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
            {
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Total </br> P/L </br> (Instr.</br> Close)</b></td>";
            }
            else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
            {
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Total </br> P/L </br> (Asset.</br> Close)</b></td>";
            }
            else
            {
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Total </br> P/L </br> (Instr.</br> Close)</b></td>";
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Total </br> P/L </br> (Asset.</br> Close)</b></td>";
            }
            strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Exposure</b></td>";
            strHtml += "</tr>";
            int flag = 0;

            string MASTERPRODUCT = null;
            int i;
            for (i = 0; i < dt1.Rows.Count; i++)
            {
                if (MASTERPRODUCT != null)
                {
                    if (MASTERPRODUCT != dt1.Rows[i]["MASTERPRODUCTID"].ToString())
                    {
                        ///////////////////////MASTERPRODUCT TOTAL

                        flag = flag + 1;
                        strHtml += "<tr style=\"background-color:lavender;text-align:center\">";
                        strHtml += "<td align=\"left\" colspan=4 nowrap=\"nowrap;\">&nbsp;</td>";
                        if (ddldate.SelectedItem.Value.ToString() == "1")
                        {
                            if (ChkBFQty.Checked == false)
                            {
                                if (dt1.Rows[i - 1]["MASTERPRODUCTID_BFQTY"] != DBNull.Value)
                                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"B/F Qty\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_BFQTY"].ToString() + "</td>";
                                else
                                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                            }
                        }
                        if (dt1.Rows[i - 1]["MASTERPRODUCTID_BUYQTY"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Buy Qty\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_BUYQTY"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                        if (dt1.Rows[i - 1]["MASTERPRODUCTID_SELLQTY"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Sell Qty\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_SELLQTY"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                        if (dt1.Rows[i - 1]["MASTERPRODUCTID_BOOKEDPL"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Booked P/L\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_BOOKEDPL"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                        if (chknetpremium.Checked)
                        {
                            if (dt1.Rows[i - 1]["MASTERPRODUCTID_Premium"] != DBNull.Value)
                                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Premium\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_Premium"].ToString() + "</td>";
                            else
                                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                        }

                        if (dt1.Rows[i - 1]["MASTERPRODUCTID_CFQTY"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"C/F Qty\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_CFQTY"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                        strHtml += "<td align=\"left\" colspan=1 nowrap=\"nowrap;\">&nbsp;</td>";

                        if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
                        {
                            strHtml += "<td align=\"left\" >&nbsp;</td>";
                            if (dt1.Rows[i - 1]["MASTERPRODUCTID_MTMINSTRUCLOSE"] != DBNull.Value)
                                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Instr.</br> Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_MTMINSTRUCLOSE"].ToString() + "</td>";
                            else
                                strHtml += "<td align=\"right\" nowrap=\"nowrap;\">&nbsp;</td>";

                        }
                        else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
                        {
                            strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                            if (dt1.Rows[i - 1]["MASTERPRODUCTID_MTMASSETCLOSE"] != DBNull.Value)
                                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Asset.</br> Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_MTMASSETCLOSE"].ToString() + "</td>";
                            else
                                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";


                        }
                        else
                        {
                            strHtml += "<td align=\"left\" colspan=2>&nbsp;</td>";
                            if (dt1.Rows[i - 1]["MASTERPRODUCTID_MTMINSTRUCLOSE"] != DBNull.Value)
                                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Instr.</br> Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_MTMINSTRUCLOSE"].ToString() + "</td>";
                            else
                                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                            if (dt1.Rows[i - 1]["MASTERPRODUCTID_MTMASSETCLOSE"] != DBNull.Value)
                                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Asset.</br> Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_MTMASSETCLOSE"].ToString() + "</td>";
                            else
                                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";


                        }
                        if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
                        {

                            if (dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLINSTRUCLOSE"] != DBNull.Value)
                                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L  (Instr.Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLINSTRUCLOSE"].ToString() + "</td>";
                            else
                                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                        }
                        else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
                        {

                            if (dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLASSETCLOSE"] != DBNull.Value)
                                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L  (Asset.Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLASSETCLOSE"].ToString() + "</td>";
                            else
                                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                        }
                        else
                        {

                            if (dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLINSTRUCLOSE"] != DBNull.Value)
                                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L  (Instr.Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLINSTRUCLOSE"].ToString() + "</td>";
                            else
                                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                            if (dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLASSETCLOSE"] != DBNull.Value)
                                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L  (Asset.Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLASSETCLOSE"].ToString() + "</td>";
                            else
                                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                        }
                        if (dt1.Rows[i - 1]["MASTERPRODUCTID_EXPOSURE"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\">Exposure\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_EXPOSURE"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                        strHtml += "</tr>";
                        ///////////////////////////////////////////MASTERPRODUCT TOTAL END
                    }
                }
                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";

                if (MASTERPRODUCT != dt1.Rows[i]["MASTERPRODUCTID"].ToString())
                {
                    if (dt1.Rows[i]["TICKERSYMBOL"] != DBNull.Value)
                        strHtml += "<td align=\"left\" style=\"font-size:xx-small;color:#348017\" nowrap=\"nowrap;\">" + dt1.Rows[i]["TICKERSYMBOL"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }
                else
                {
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }

                MASTERPRODUCT = dt1.Rows[i]["MASTERPRODUCTID"].ToString();

                if (dt1.Rows[i]["SERIES"] != DBNull.Value)
                {
                    if (dt1.Rows[i]["SERIES"].ToString().Trim().StartsWith("S"))
                    {
                        strHtml += "<td align=\"left\" colspan=3 nowrap=\"nowrap;\" style=\"width:8%;font-size:xx-small;\" title=\"Instr. Type For " + cmbclient.SelectedItem.Text.ToString().Trim() + " [ " + dt1.Rows[i]["UCC"].ToString() + " ]\">" + dt1.Rows[i]["SERIES"].ToString() + "</td>";
                    }
                    else
                    {
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\" style=\"width:8%;font-size:xx-small;\" title=\"Instr. Type For " + cmbclient.SelectedItem.Text.ToString().Trim() + " [ " + dt1.Rows[i]["UCC"].ToString() + " ]\">" + dt1.Rows[i]["SERIES"].ToString() + "</td>";
                        if (dt1.Rows[i]["EXPIRY"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[i]["EXPIRY"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" >&nbsp;</td>";

                        if (dt1.Rows[i]["STRIKEPRICE"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Strike Price\" nowrap=\"nowrap;\">" + dt1.Rows[i]["STRIKEPRICE"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                    }
                }
                else
                {
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                    if (dt1.Rows[i]["EXPIRY"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[i]["EXPIRY"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";

                    if (dt1.Rows[i]["STRIKEPRICE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Strike Price\" nowrap=\"nowrap;\">" + dt1.Rows[i]["STRIKEPRICE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }

                if (ddldate.SelectedItem.Value.ToString() == "1")
                {
                    if (ChkBFQty.Checked == false)
                    {
                        if (dt1.Rows[i]["BFQTY"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"B/F Qty\" nowrap=\"nowrap;\">" + dt1.Rows[i]["BFQTY"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                    }
                }
                if (dt1.Rows[i]["BUYQTY"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Buy Qty\" nowrap=\"nowrap;\">" + dt1.Rows[i]["BUYQTY"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                if (dt1.Rows[i]["SELLQTY"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Sell Qty\" nowrap=\"nowrap;\">" + dt1.Rows[i]["SELLQTY"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                if (dt1.Rows[i]["BOOKEDPL"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Booked P/L\" nowrap=\"nowrap;\">" + dt1.Rows[i]["BOOKEDPL"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                if (chknetpremium.Checked)
                {
                    if (dt1.Rows[i]["Premium"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Premium\" nowrap=\"nowrap;\">" + dt1.Rows[i]["Premium"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                }

                if (dt1.Rows[i]["CFQTY"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"C/F  Qty\">" + dt1.Rows[i]["CFQTY"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (dt1.Rows[i]["AVGCOSTRESEDUAL"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Avg. Cost (Residual)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["AVGCOSTRESEDUAL"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
                {
                    if (dt1.Rows[i]["CLOSEPRICEINSTRU"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Close Price (Instrmnt)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["CLOSEPRICEINSTRU"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }
                else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
                {
                    if (dt1.Rows[i]["CLOSEPRICEASSET"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Close Price (Asset)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["CLOSEPRICEASSET"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }
                else
                {
                    if (dt1.Rows[i]["CLOSEPRICEINSTRU"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Close Price (Instrmnt)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["CLOSEPRICEINSTRU"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                    if (dt1.Rows[i]["CLOSEPRICEASSET"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Close Price (Asset)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["CLOSEPRICEASSET"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }

                if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
                {
                    if (dt1.Rows[i]["MTMINSTRUCLOSE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Instr. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["MTMINSTRUCLOSE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }
                else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
                {
                    if (dt1.Rows[i]["MTMASSETCLOSE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Asset. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["MTMASSETCLOSE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }
                else
                {
                    if (dt1.Rows[i]["MTMINSTRUCLOSE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Instr. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["MTMINSTRUCLOSE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                    if (dt1.Rows[i]["MTMASSETCLOSE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Asset. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["MTMASSETCLOSE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }


                if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
                {
                    if (dt1.Rows[i]["TOTALPLINSTRUCLOSE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L (Instr..Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["TOTALPLINSTRUCLOSE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";
                }
                else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
                {
                    if (dt1.Rows[i]["TOTALPLASSETCLOSE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L (Asset..Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["TOTALPLASSETCLOSE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }
                else
                {
                    if (dt1.Rows[i]["TOTALPLINSTRUCLOSE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L (Instr.Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["TOTALPLINSTRUCLOSE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                    if (dt1.Rows[i]["TOTALPLASSETCLOSE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L (Asset.Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["TOTALPLASSETCLOSE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }

                if (dt1.Rows[i]["Exposure"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Exposure\">" + dt1.Rows[i]["EXPOSURE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                strHtml += "</tr>";
            }
            ///////////////////////MASTERPRODUCT TOTAL

            flag = flag + 1;
            strHtml += "<tr style=\"background-color:lavender;text-align:center\">";
            strHtml += "<td align=\"left\" colspan=4 nowrap=\"nowrap;\">&nbsp;</td>";
            if (ddldate.SelectedItem.Value.ToString() == "1")
            {
                if (ChkBFQty.Checked == false)
                {
                    if (dt1.Rows[i - 1]["MASTERPRODUCTID_BFQTY"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"B/F Qty\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_BFQTY"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                }
            }
            if (dt1.Rows[i - 1]["MASTERPRODUCTID_BUYQTY"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Buy Qty\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_BUYQTY"].ToString() + "</td>";
            else
                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

            if (dt1.Rows[i - 1]["MASTERPRODUCTID_SELLQTY"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Sell Qty\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_SELLQTY"].ToString() + "</td>";
            else
                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

            if (dt1.Rows[i - 1]["MASTERPRODUCTID_BOOKEDPL"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Booked P/L\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_BOOKEDPL"].ToString() + "</td>";
            else
                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

            if (chknetpremium.Checked)
            {

                if (dt1.Rows[i - 1]["MASTERPRODUCTID_Premium"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Premium\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_Premium"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

            }

            if (dt1.Rows[i - 1]["MASTERPRODUCTID_CFQTY"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"C/F  Qty\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_CFQTY"].ToString() + "</td>";
            else
                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

            strHtml += "<td align=\"left\" colspan=1 nowrap=\"nowrap;\">&nbsp;</td>";

            if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
            {
                strHtml += "<td align=\"left\" >&nbsp;</td>";
                if (dt1.Rows[i - 1]["MASTERPRODUCTID_MTMINSTRUCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Instr. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_MTMINSTRUCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
            {
                strHtml += "<td align=\"left\" >&nbsp;</td>";
                if (dt1.Rows[i - 1]["MASTERPRODUCTID_MTMASSETCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Asset. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_MTMASSETCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            else
            {
                strHtml += "<td align=\"left\" colspan=2>&nbsp;</td>";

                if (dt1.Rows[i - 1]["MASTERPRODUCTID_MTMINSTRUCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Instr. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_MTMINSTRUCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                if (dt1.Rows[i - 1]["MASTERPRODUCTID_MTMASSETCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Asset. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_MTMASSETCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
            {

                if (dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLINSTRUCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L (Instr.Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLINSTRUCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
            {

                if (dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLASSETCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L (Asset.Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLASSETCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            else
            {

                if (dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLINSTRUCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L (Instr.Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLINSTRUCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                if (dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLASSETCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L (Asset.Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLASSETCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            if (dt1.Rows[i - 1]["MASTERPRODUCTID_EXPOSURE"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Exposure\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_EXPOSURE"].ToString() + "</td>";
            else
                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

            strHtml += "</tr>";
            ///////////////////////////////////////////MASTERPRODUCT TOTAL END
            ///////////////////////CLIENT TOTAL

            flag = flag + 1;
            strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
            strHtml += "<td align=\"left\" nowrap=\"nowrap;\" colspan=6 title=\"For " + cmbclient.SelectedItem.Text.ToString().Trim() + " [ " + dt1.Rows[0]["UCC"].ToString() + " ]\"><B>Client Total :</B></td>";
            if (ddldate.SelectedItem.Value.ToString() == "1")
            {
                if (ChkBFQty.Checked == false)
                {
                    strHtml += "<td align=\"left\" >&nbsp;</td>";
                }
            }
            if (dt1.Rows[0]["CUSTOMERID_BOOKEDPL"] != DBNull.Value)
                strHtml += "<td align=\"right\" nowrap=\"nowrap;\" style=\"font-size:xx-small;\" title=\"Booked P/L\">" + dt1.Rows[0]["CUSTOMERID_BOOKEDPL"].ToString() + "</td>";
            else
                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

            if (chknetpremium.Checked)
            {
                if (dt1.Rows[0]["CUSTOMERID_Premium"] != DBNull.Value)
                    strHtml += "<td align=\"right\" nowrap=\"nowrap;\" style=\"font-size:xx-small;\" title=\"Premium\">" + dt1.Rows[0]["CUSTOMERID_Premium"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

            }

            strHtml += "<td align=\"left\" colspan=2 nowrap=\"nowrap;\">&nbsp;</td>";

            if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
            {
                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                if (dt1.Rows[0]["CUSTOMERID_MTMINSTRUCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Instr. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["CUSTOMERID_MTMINSTRUCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
            {
                strHtml += "<td align=\"left\" >&nbsp;</td>";
                if (dt1.Rows[0]["CUSTOMERID_MTMASSETCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Asset. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["CUSTOMERID_MTMASSETCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            else
            {

                strHtml += "<td align=\"left\" colspan=2 >&nbsp;</td>";
                if (dt1.Rows[0]["CUSTOMERID_MTMINSTRUCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Instr. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["CUSTOMERID_MTMINSTRUCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (dt1.Rows[0]["CUSTOMERID_MTMASSETCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Asset. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["CUSTOMERID_MTMASSETCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
            {

                if (dt1.Rows[0]["CUSTOMERID_TOTALPLINSTRUCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L(Instr. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["CUSTOMERID_TOTALPLINSTRUCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
            {

                if (dt1.Rows[0]["CUSTOMERID_TOTALPLASSETCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L(Asset. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["CUSTOMERID_TOTALPLASSETCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            else
            {

                if (dt1.Rows[0]["CUSTOMERID_TOTALPLINSTRUCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L(Instr. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["CUSTOMERID_TOTALPLINSTRUCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                if (dt1.Rows[0]["CUSTOMERID_TOTALPLASSETCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L(Asset. Close)\">" + dt1.Rows[0]["CUSTOMERID_TOTALPLASSETCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";
            }
            if (dt1.Rows[0]["CUSTOMERID_Exposure"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Exposure\" nowrap=\"nowrap;\">" + dt1.Rows[0]["CUSTOMERID_Exposure"].ToString() + "</td>";
            else
                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

            strHtml += "</tr>";
            strHtml += "</table>";
            ///////////////////////////////////////////CLIENT TOTAL END


            ///////////////////////GROUP TOTAL

            flag = flag + 1;
            //if (rbmail.Checked == true)
            //{
            if (ddloptionformail.SelectedItem.Value != "0")
            {
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                strHtml += "<td align=\"left\" nowrap=\"nowrap;\" colspan=6 title=\"For " + cmbgroup.SelectedItem.Text.ToString().Trim() + "\"><B>Branch/Group Total :</B></td>";
                if (ddldate.SelectedItem.Value.ToString() == "1")
                {
                    if (ChkBFQty.Checked == false)
                    {
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                    }
                }
                if (dt1.Rows[0]["GROUP_BOOKEDPL"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Booked P/L\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_BOOKEDPL"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                strHtml += "<td align=\"left\" colspan=2 nowrap=\"nowrap;\">&nbsp;</td>";

                if (chknetpremium.Checked)
                {
                    if (dt1.Rows[0]["GROUP_Premium"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Premium\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_Premium"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }

                if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
                {
                    strHtml += "<td align=\"left\" >&nbsp;</td>";
                    if (dt1.Rows[0]["GROUP_MTMINSTRUCLOSE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Instr. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_MTMINSTRUCLOSE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }
                else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
                {
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                    if (dt1.Rows[0]["GROUP_MTMASSETCLOSE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Asset. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_MTMASSETCLOSE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }
                else
                {
                    strHtml += "<td align=\"left\" colspan=2>&nbsp;</td>";
                    if (dt1.Rows[0]["GROUP_MTMINSTRUCLOSE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Instr. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_MTMINSTRUCLOSE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                    if (dt1.Rows[0]["GROUP_MTMASSETCLOSE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Asset. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_MTMASSETCLOSE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }
                if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
                {
                    if (dt1.Rows[0]["GROUP_TOTALPLINSTRUCLOSE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L (Instr. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_TOTALPLINSTRUCLOSE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }
                else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
                {
                    if (dt1.Rows[0]["GROUP_TOTALPLASSETCLOSE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L (Asset. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_TOTALPLASSETCLOSE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }
                else
                {
                    if (dt1.Rows[0]["GROUP_TOTALPLINSTRUCLOSE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L (Instr. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_TOTALPLINSTRUCLOSE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                    if (dt1.Rows[0]["GROUP_TOTALPLASSETCLOSE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L (Asset. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_TOTALPLASSETCLOSE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }
                if (dt1.Rows[0]["GROUP_Exposure"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Exposure\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_Exposure"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                strHtml += "</tr>";
                strHtml += "</table>";
                ///////////////////////////////////////////GROUP TOTAL END

                DIVdisplayPERIOD.InnerHtml = strHtml1;
                display.InnerHtml = strHtml;
                ViewState["mail"] = strHtml1 + strHtml;
            }
            else
            {
                ViewState["mail"] = strHtml1 + strHtml;

            }




        }
        void htmltable_rpttypecleintwise_detail2(string clientid, string clientname, string grpid, string grpname)
        {
            ds = (DataSet)ViewState["dataset"];
            String strHtml = String.Empty;
            String strHtml1 = String.Empty;
            int colcount = ds.Tables[0].Columns.Count;
            string str = null;
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                str = "[" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "]";
            }

            if (ddldate.SelectedItem.Value.ToString() == "0")
            {
                str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report For " + oconverter.ArrangeDate2(dtfor.Value.ToString());
            }
            else
            {
                str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report Period " + oconverter.ArrangeDate2(dtfrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtto.Value.ToString());
            }
            strHtml1 = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtml1 += "<tr><td align=\"left\" colspan=" + colcount + " style=\"color:Blue;\">" + str + "</td></tr></table>";

            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

            DataView viewclient = new DataView();
            viewclient = ds.Tables[0].DefaultView;
            viewclient.RowFilter = "CUSTOMERID='" + clientid.ToString().Trim() + "'";
            DataTable dt1 = new DataTable();
            dt1 = viewclient.ToTable();


            strHtml += "<tr style=\"background-color:lavender ;text-align:left\">";
            strHtml += "<td align=\"left\" colspan=" + colcount + " nowrap=\"nowrap;\">Client Name :&nbsp;<b>" + clientname.ToString().Trim() + "</b>[ <b>" + dt1.Rows[0]["UCC"].ToString() + " </b> ]</td></tr>";

            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Asset</b></td>";
            strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Instr. </br> Type</b></td>";
            strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Expiry </br> Date</b></td>";
            strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Strike </br> Price</b></td>";
            if (ddldate.SelectedItem.Value.ToString() == "1")
            {
                if (ChkBFQty.Checked == false)
                {
                    strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>B/F Qty</b></td>";
                }
            }
            strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Buy </br> Qty</b></td>";
            strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Sell </br> Qty</b></td>";
            strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Booked </br> P/L</b></td>";
            if (chknetpremium.Checked)
            {
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Premium</b></td>";
            }
            strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>C/F  Qty</b></td>";
            strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Avg. </br> Cost </br> (Residual)</b></td>";
            if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
            {
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"> <b>Close </br> Price </br> (Instrmnt)</b></td>";
            }
            else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
            {
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Close </br> Price </br> (Asset)</b></td>";
            }
            else
            {
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Close </br> Price </br> (Instrmnt)</b></td>";
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Close </br> Price </br> (Asset)</b></td>";
            }
            if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
            {
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>MTM </br>(Instr.</br> Close)</b></td>";
            }
            else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
            {
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>MTM </br>(Asset. </br> Close)</b></td>";
            }
            else
            {
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>MTM </br>(Instr.</br> Close)</b></td>";
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>MTM </br>(Asset.</br> Close)</b></td>";
            }
            if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
            {
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Total </br> P/L </br> (Instr.</br> Close)</b></td>";
            }
            else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
            {
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Total </br> P/L </br> (Asset.</br> Close)</b></td>";
            }
            else
            {
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Total </br> P/L </br> (Instr.</br> Close)</b></td>";
                strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Total </br> P/L </br> (Asset.</br> Close)</b></td>";
            }
            strHtml += "<td align=\"center\" nowrap=\"nowrap;\"><b>Exposure</b></td>";
            strHtml += "</tr>";
            int flag = 0;

            string MASTERPRODUCT = null;
            int i;
            for (i = 0; i < dt1.Rows.Count; i++)
            {
                if (MASTERPRODUCT != null)
                {
                    if (MASTERPRODUCT != dt1.Rows[i]["MASTERPRODUCTID"].ToString())
                    {
                        ///////////////////////MASTERPRODUCT TOTAL

                        flag = flag + 1;
                        strHtml += "<tr style=\"background-color:lavender;text-align:center\">";
                        strHtml += "<td align=\"left\" colspan=4 nowrap=\"nowrap;\">&nbsp;</td>";
                        if (ddldate.SelectedItem.Value.ToString() == "1")
                        {
                            if (ChkBFQty.Checked == false)
                            {
                                if (dt1.Rows[i - 1]["MASTERPRODUCTID_BFQTY"] != DBNull.Value)
                                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"B/F Qty\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_BFQTY"].ToString() + "</td>";
                                else
                                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                            }
                        }
                        if (dt1.Rows[i - 1]["MASTERPRODUCTID_BUYQTY"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Buy Qty\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_BUYQTY"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                        if (dt1.Rows[i - 1]["MASTERPRODUCTID_SELLQTY"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Sell Qty\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_SELLQTY"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                        if (dt1.Rows[i - 1]["MASTERPRODUCTID_BOOKEDPL"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Booked P/L\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_BOOKEDPL"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                        if (chknetpremium.Checked)
                        {
                            if (dt1.Rows[i - 1]["MASTERPRODUCTID_Premium"] != DBNull.Value)
                                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Premium\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_Premium"].ToString() + "</td>";
                            else
                                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                        }

                        if (dt1.Rows[i - 1]["MASTERPRODUCTID_CFQTY"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"C/F Qty\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_CFQTY"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                        strHtml += "<td align=\"left\" colspan=1 nowrap=\"nowrap;\">&nbsp;</td>";

                        if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
                        {
                            strHtml += "<td align=\"left\" >&nbsp;</td>";
                            if (dt1.Rows[i - 1]["MASTERPRODUCTID_MTMINSTRUCLOSE"] != DBNull.Value)
                                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Instr.</br> Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_MTMINSTRUCLOSE"].ToString() + "</td>";
                            else
                                strHtml += "<td align=\"right\" nowrap=\"nowrap;\">&nbsp;</td>";

                        }
                        else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
                        {
                            strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                            if (dt1.Rows[i - 1]["MASTERPRODUCTID_MTMASSETCLOSE"] != DBNull.Value)
                                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Asset.</br> Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_MTMASSETCLOSE"].ToString() + "</td>";
                            else
                                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";


                        }
                        else
                        {
                            strHtml += "<td align=\"left\" colspan=2>&nbsp;</td>";
                            if (dt1.Rows[i - 1]["MASTERPRODUCTID_MTMINSTRUCLOSE"] != DBNull.Value)
                                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Instr.</br> Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_MTMINSTRUCLOSE"].ToString() + "</td>";
                            else
                                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                            if (dt1.Rows[i - 1]["MASTERPRODUCTID_MTMASSETCLOSE"] != DBNull.Value)
                                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Asset.</br> Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_MTMASSETCLOSE"].ToString() + "</td>";
                            else
                                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";


                        }
                        if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
                        {

                            if (dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLINSTRUCLOSE"] != DBNull.Value)
                                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L  (Instr.Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLINSTRUCLOSE"].ToString() + "</td>";
                            else
                                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                        }
                        else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
                        {

                            if (dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLASSETCLOSE"] != DBNull.Value)
                                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L  (Asset.Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLASSETCLOSE"].ToString() + "</td>";
                            else
                                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                        }
                        else
                        {

                            if (dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLINSTRUCLOSE"] != DBNull.Value)
                                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L  (Instr.Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLINSTRUCLOSE"].ToString() + "</td>";
                            else
                                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                            if (dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLASSETCLOSE"] != DBNull.Value)
                                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L  (Asset.Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLASSETCLOSE"].ToString() + "</td>";
                            else
                                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                        }
                        if (dt1.Rows[i - 1]["MASTERPRODUCTID_EXPOSURE"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\">Exposure\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_EXPOSURE"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                        strHtml += "</tr>";
                        ///////////////////////////////////////////MASTERPRODUCT TOTAL END
                    }
                }
                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";

                if (MASTERPRODUCT != dt1.Rows[i]["MASTERPRODUCTID"].ToString())
                {
                    if (dt1.Rows[i]["TICKERSYMBOL"] != DBNull.Value)
                        strHtml += "<td align=\"left\" style=\"font-size:xx-small;color:#348017\" nowrap=\"nowrap;\">" + dt1.Rows[i]["TICKERSYMBOL"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }
                else
                {
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }

                MASTERPRODUCT = dt1.Rows[i]["MASTERPRODUCTID"].ToString();

                if (dt1.Rows[i]["SERIES"] != DBNull.Value)
                {
                    if (dt1.Rows[i]["SERIES"].ToString().Trim().StartsWith("S"))
                    {
                        strHtml += "<td align=\"left\" colspan=3 nowrap=\"nowrap;\" style=\"width:8%;font-size:xx-small;\" title=\"Instr. Type For " + cmbclient.SelectedItem.Text.ToString().Trim() + " [ " + dt1.Rows[i]["UCC"].ToString() + " ]\">" + dt1.Rows[i]["SERIES"].ToString() + "</td>";
                    }
                    else
                    {
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\" style=\"width:8%;font-size:xx-small;\" title=\"Instr. Type For " + cmbclient.SelectedItem.Text.ToString().Trim() + " [ " + dt1.Rows[i]["UCC"].ToString() + " ]\">" + dt1.Rows[i]["SERIES"].ToString() + "</td>";
                        if (dt1.Rows[i]["EXPIRY"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[i]["EXPIRY"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" >&nbsp;</td>";

                        if (dt1.Rows[i]["STRIKEPRICE"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Strike Price\" nowrap=\"nowrap;\">" + dt1.Rows[i]["STRIKEPRICE"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                    }
                }
                else
                {
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                    if (dt1.Rows[i]["EXPIRY"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[i]["EXPIRY"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";

                    if (dt1.Rows[i]["STRIKEPRICE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Strike Price\" nowrap=\"nowrap;\">" + dt1.Rows[i]["STRIKEPRICE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }

                if (ddldate.SelectedItem.Value.ToString() == "1")
                {
                    if (ChkBFQty.Checked == false)
                    {
                        if (dt1.Rows[i]["BFQTY"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"B/F Qty\" nowrap=\"nowrap;\">" + dt1.Rows[i]["BFQTY"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                    }
                }
                if (dt1.Rows[i]["BUYQTY"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Buy Qty\" nowrap=\"nowrap;\">" + dt1.Rows[i]["BUYQTY"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                if (dt1.Rows[i]["SELLQTY"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Sell Qty\" nowrap=\"nowrap;\">" + dt1.Rows[i]["SELLQTY"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                if (dt1.Rows[i]["BOOKEDPL"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Booked P/L\" nowrap=\"nowrap;\">" + dt1.Rows[i]["BOOKEDPL"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                if (chknetpremium.Checked)
                {
                    if (dt1.Rows[i]["Premium"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Premium\" nowrap=\"nowrap;\">" + dt1.Rows[i]["Premium"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                }

                if (dt1.Rows[i]["CFQTY"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"C/F  Qty\">" + dt1.Rows[i]["CFQTY"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (dt1.Rows[i]["AVGCOSTRESEDUAL"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Avg. Cost (Residual)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["AVGCOSTRESEDUAL"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
                {
                    if (dt1.Rows[i]["CLOSEPRICEINSTRU"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Close Price (Instrmnt)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["CLOSEPRICEINSTRU"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }
                else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
                {
                    if (dt1.Rows[i]["CLOSEPRICEASSET"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Close Price (Asset)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["CLOSEPRICEASSET"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }
                else
                {
                    if (dt1.Rows[i]["CLOSEPRICEINSTRU"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Close Price (Instrmnt)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["CLOSEPRICEINSTRU"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                    if (dt1.Rows[i]["CLOSEPRICEASSET"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Close Price (Asset)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["CLOSEPRICEASSET"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }

                if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
                {
                    if (dt1.Rows[i]["MTMINSTRUCLOSE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Instr. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["MTMINSTRUCLOSE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }
                else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
                {
                    if (dt1.Rows[i]["MTMASSETCLOSE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Asset. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["MTMASSETCLOSE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }
                else
                {
                    if (dt1.Rows[i]["MTMINSTRUCLOSE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Instr. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["MTMINSTRUCLOSE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                    if (dt1.Rows[i]["MTMASSETCLOSE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Asset. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["MTMASSETCLOSE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }


                if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
                {
                    if (dt1.Rows[i]["TOTALPLINSTRUCLOSE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L (Instr..Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["TOTALPLINSTRUCLOSE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";
                }
                else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
                {
                    if (dt1.Rows[i]["TOTALPLASSETCLOSE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L (Asset..Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["TOTALPLASSETCLOSE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }
                else
                {
                    if (dt1.Rows[i]["TOTALPLINSTRUCLOSE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L (Instr.Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["TOTALPLINSTRUCLOSE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                    if (dt1.Rows[i]["TOTALPLASSETCLOSE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L (Asset.Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i]["TOTALPLASSETCLOSE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }

                if (dt1.Rows[i]["Exposure"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Exposure\">" + dt1.Rows[i]["EXPOSURE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                strHtml += "</tr>";
            }
            ///////////////////////MASTERPRODUCT TOTAL

            flag = flag + 1;
            strHtml += "<tr style=\"background-color:lavender;text-align:center\">";
            strHtml += "<td align=\"left\" colspan=4 nowrap=\"nowrap;\">&nbsp;</td>";
            if (ddldate.SelectedItem.Value.ToString() == "1")
            {
                if (ChkBFQty.Checked == false)
                {
                    if (dt1.Rows[i - 1]["MASTERPRODUCTID_BFQTY"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"B/F Qty\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_BFQTY"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                }
            }
            if (dt1.Rows[i - 1]["MASTERPRODUCTID_BUYQTY"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Buy Qty\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_BUYQTY"].ToString() + "</td>";
            else
                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

            if (dt1.Rows[i - 1]["MASTERPRODUCTID_SELLQTY"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Sell Qty\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_SELLQTY"].ToString() + "</td>";
            else
                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

            if (dt1.Rows[i - 1]["MASTERPRODUCTID_BOOKEDPL"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Booked P/L\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_BOOKEDPL"].ToString() + "</td>";
            else
                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

            if (chknetpremium.Checked)
            {

                if (dt1.Rows[i - 1]["MASTERPRODUCTID_Premium"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Premium\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_Premium"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

            }

            if (dt1.Rows[i - 1]["MASTERPRODUCTID_CFQTY"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"C/F  Qty\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_CFQTY"].ToString() + "</td>";
            else
                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

            strHtml += "<td align=\"left\" colspan=1 nowrap=\"nowrap;\">&nbsp;</td>";

            if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
            {
                strHtml += "<td align=\"left\" >&nbsp;</td>";
                if (dt1.Rows[i - 1]["MASTERPRODUCTID_MTMINSTRUCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Instr. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_MTMINSTRUCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
            {
                strHtml += "<td align=\"left\" >&nbsp;</td>";
                if (dt1.Rows[i - 1]["MASTERPRODUCTID_MTMASSETCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Asset. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_MTMASSETCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            else
            {
                strHtml += "<td align=\"left\" colspan=2>&nbsp;</td>";

                if (dt1.Rows[i - 1]["MASTERPRODUCTID_MTMINSTRUCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Instr. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_MTMINSTRUCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                if (dt1.Rows[i - 1]["MASTERPRODUCTID_MTMASSETCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Asset. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_MTMASSETCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
            {

                if (dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLINSTRUCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L (Instr.Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLINSTRUCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
            {

                if (dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLASSETCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L (Asset.Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLASSETCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            else
            {

                if (dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLINSTRUCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L (Instr.Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLINSTRUCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                if (dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLASSETCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L (Asset.Close)\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_TOTALPLASSETCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            if (dt1.Rows[i - 1]["MASTERPRODUCTID_EXPOSURE"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Exposure\" nowrap=\"nowrap;\">" + dt1.Rows[i - 1]["MASTERPRODUCTID_EXPOSURE"].ToString() + "</td>";
            else
                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

            strHtml += "</tr>";
            ///////////////////////////////////////////MASTERPRODUCT TOTAL END
            ///////////////////////CLIENT TOTAL

            flag = flag + 1;
            strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
            strHtml += "<td align=\"left\" nowrap=\"nowrap;\" colspan=6 title=\"For " + cmbclient.SelectedItem.Text.ToString().Trim() + " [ " + dt1.Rows[0]["UCC"].ToString() + " ]\"><B>Client Total :</B></td>";
            if (ddldate.SelectedItem.Value.ToString() == "1")
            {
                if (ChkBFQty.Checked == false)
                {
                    strHtml += "<td align=\"left\" >&nbsp;</td>";
                }
            }
            if (dt1.Rows[0]["CUSTOMERID_BOOKEDPL"] != DBNull.Value)
                strHtml += "<td align=\"right\" nowrap=\"nowrap;\" style=\"font-size:xx-small;\" title=\"Booked P/L\">" + dt1.Rows[0]["CUSTOMERID_BOOKEDPL"].ToString() + "</td>";
            else
                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

            if (chknetpremium.Checked)
            {
                if (dt1.Rows[0]["CUSTOMERID_Premium"] != DBNull.Value)
                    strHtml += "<td align=\"right\" nowrap=\"nowrap;\" style=\"font-size:xx-small;\" title=\"Premium\">" + dt1.Rows[0]["CUSTOMERID_Premium"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

            }

            strHtml += "<td align=\"left\" colspan=2 nowrap=\"nowrap;\">&nbsp;</td>";

            if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
            {
                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                if (dt1.Rows[0]["CUSTOMERID_MTMINSTRUCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Instr. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["CUSTOMERID_MTMINSTRUCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
            {
                strHtml += "<td align=\"left\" >&nbsp;</td>";
                if (dt1.Rows[0]["CUSTOMERID_MTMASSETCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Asset. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["CUSTOMERID_MTMASSETCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            else
            {

                strHtml += "<td align=\"left\" colspan=2 >&nbsp;</td>";
                if (dt1.Rows[0]["CUSTOMERID_MTMINSTRUCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Instr. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["CUSTOMERID_MTMINSTRUCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (dt1.Rows[0]["CUSTOMERID_MTMASSETCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Asset. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["CUSTOMERID_MTMASSETCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
            {

                if (dt1.Rows[0]["CUSTOMERID_TOTALPLINSTRUCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L(Instr. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["CUSTOMERID_TOTALPLINSTRUCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
            {

                if (dt1.Rows[0]["CUSTOMERID_TOTALPLASSETCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L(Asset. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["CUSTOMERID_TOTALPLASSETCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            else
            {

                if (dt1.Rows[0]["CUSTOMERID_TOTALPLINSTRUCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L(Instr. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["CUSTOMERID_TOTALPLINSTRUCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                if (dt1.Rows[0]["CUSTOMERID_TOTALPLASSETCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L(Asset. Close)\">" + dt1.Rows[0]["CUSTOMERID_TOTALPLASSETCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";
            }
            if (dt1.Rows[0]["CUSTOMERID_Exposure"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Exposure\" nowrap=\"nowrap;\">" + dt1.Rows[0]["CUSTOMERID_Exposure"].ToString() + "</td>";
            else
                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

            strHtml += "</tr>";

            ///////////////////////////////////////////CLIENT TOTAL END


            ///////////////////////GROUP TOTAL

            flag = flag + 1;

            strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
            strHtml += "<td align=\"left\" nowrap=\"nowrap;\" colspan=6 title=\"For " + cmbgroup.SelectedItem.Text.ToString().Trim() + "\"><B>Branch/Group Total :</B></td>";
            if (ddldate.SelectedItem.Value.ToString() == "1")
            {
                if (ChkBFQty.Checked == false)
                {
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                }
            }
            if (dt1.Rows[0]["GROUP_BOOKEDPL"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Booked P/L\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_BOOKEDPL"].ToString() + "</td>";
            else
                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

            strHtml += "<td align=\"left\" colspan=2 nowrap=\"nowrap;\">&nbsp;</td>";

            if (chknetpremium.Checked)
            {
                if (dt1.Rows[0]["GROUP_Premium"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Premium\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_Premium"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }

            if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
            {
                strHtml += "<td align=\"left\" >&nbsp;</td>";
                if (dt1.Rows[0]["GROUP_MTMINSTRUCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Instr. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_MTMINSTRUCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
            {
                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
                if (dt1.Rows[0]["GROUP_MTMASSETCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Asset. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_MTMASSETCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            else
            {
                strHtml += "<td align=\"left\" colspan=2>&nbsp;</td>";
                if (dt1.Rows[0]["GROUP_MTMINSTRUCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Instr. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_MTMINSTRUCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                if (dt1.Rows[0]["GROUP_MTMASSETCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"MTM (Asset. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_MTMASSETCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "1")
            {
                if (dt1.Rows[0]["GROUP_TOTALPLINSTRUCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L (Instr. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_TOTALPLINSTRUCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            else if (ddlmtmcalbasis.SelectedItem.Value.ToString() == "2")
            {
                if (dt1.Rows[0]["GROUP_TOTALPLASSETCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L (Asset. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_TOTALPLASSETCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            else
            {
                if (dt1.Rows[0]["GROUP_TOTALPLINSTRUCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L (Instr. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_TOTALPLINSTRUCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

                if (dt1.Rows[0]["GROUP_TOTALPLASSETCLOSE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Total P/L (Asset. Close)\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_TOTALPLASSETCLOSE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";
            }
            if (dt1.Rows[0]["GROUP_Exposure"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Exposure\" nowrap=\"nowrap;\">" + dt1.Rows[0]["GROUP_Exposure"].ToString() + "</td>";
            else
                strHtml += "<td align=\"left\" nowrap=\"nowrap;\">&nbsp;</td>";

            strHtml += "</tr>";
            strHtml += "</table>";
            ///////////////////////////////////////////GROUP TOTAL END
            if (rbmail.Checked == false)
            {
                DIVdisplayPERIOD.InnerHtml = strHtml1;
                display.InnerHtml = strHtml;

            }
            else
            {
                ViewState["mail"] = strHtml1 + strHtml;
                // ViewState["Usermail"] = strHtml1 + strHtml;
            }




        }
    }
}