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
    public partial class Reports_finalsettlementreport : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        DataSet ds = new DataSet();
        DataTable dtgroupcontactid = new DataTable();
        DataTable Distinctclient = new DataTable();
        ExcelFile objExcel = new ExcelFile();
        DataTable dtExport = new DataTable();
        PeriodicalReports periodicalReports = new PeriodicalReports();
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
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2" | HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "5")
            {
                dtexpiryeffectuntil = oDBEngine.GetDataTable("master_equity", " DISTINCT top 2 equity_effectuntil", "  month(equity_effectuntil)<=month(getdate()) and year(equity_effectuntil)=year(getdate())", " equity_effectuntil desc");
            }
            else
            {
                dtexpiryeffectuntil = oDBEngine.GetDataTable("master_commodity", " DISTINCT top 2 commodity_expirydate", "  month(commodity_expirydate)<=month(getdate()) and year(commodity_expirydate)=year(getdate())", " commodity_expirydate desc");

            }
            if (dtexpiryeffectuntil.Rows.Count == 2)
            {
                dtfrom.Value = Convert.ToDateTime(new DateTime(Convert.ToDateTime(dtexpiryeffectuntil.Rows[1][0].ToString()).Year, Convert.ToDateTime(dtexpiryeffectuntil.Rows[1][0].ToString()).Month, Convert.ToDateTime(dtexpiryeffectuntil.Rows[1][0].ToString()).Day).AddDays(1).ToString());
                dtto.Value = Convert.ToDateTime(dtexpiryeffectuntil.Rows[0][0].ToString());

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
        void fn_Client()
        {
            string Clients;
            if (rdbClientALL.Checked)//////////////////ALL CLIENT CHECK
            {
                Clients = null;
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
                        dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%' and cnt_internalid in(select grp_contactid from tbl_trans_group where grp_groupmaster in(" + HiddenField_Group.Value.ToString().Trim() + "))");
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
                        dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%' and cnt_branchid in(" + HiddenField_Branch.Value.ToString().Trim() + ")");
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
                HiddenField_Client.Value = Clients;
            }
            else if (radPOAClient.Checked)//////////////////////ALL POA CLIENT CHECK
            {
                Clients = null;
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
                        dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%' and cnt_internalid in (select dpd_cntID from tbl_master_contactdpdetails where dpd_POA=1) and cnt_internalid in(select grp_contactid from tbl_trans_group where grp_groupmaster in(" + HiddenField_Group.Value.ToString().Trim() + "))");
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
                        dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%' and cnt_internalid in (select dpd_cntID from tbl_master_contactdpdetails where dpd_POA=1) and cnt_branchid in(" + HiddenField_Branch.Value.ToString().Trim() + ")");
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
                HiddenField_Client.Value = Clients;
            }
        }
        protected void btnshow_Click(object sender, EventArgs e)
        {
            procedure();

        }
        void procedure()
        {

            ////////////////Fetch Instrument
            string chktype = null;
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2" | HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "5")
            {
                foreach (ListItem listitem in chktradetype.Items)
                {

                    if (listitem.Selected)
                    {
                        if (chktype == null)
                            chktype = "(customertrades_tradecategory IN ( " + " '" + listitem.Value + "'";
                        else
                            chktype += "," + "'" + listitem.Value + "'";
                    }
                }
            }
            else
            {
                foreach (ListItem listitem in chktradetype.Items)
                {

                    if (listitem.Selected)
                    {
                        if (chktype == null)
                            chktype = "(comcustomertrades_tradecategory IN ( " + " '" + listitem.Value + "'";
                        else
                            chktype += "," + "'" + listitem.Value + "'";
                    }
                }
            }
            if (chktype != null)
            {
                chktype += " ))";
                fn_Client();

                ds = periodicalReports.finalsettlementreport(Session["LastCompany"].ToString(), Session["usersegid"].ToString(),
                    ddldate.SelectedItem.Value.ToString() == "0" ? "NA" : dtfrom.Value.ToString(), ddldate.SelectedItem.Value.ToString() == "0" ? dtfor.Value.ToString() : dtto.Value.ToString(),
                    HiddenField_Client.Value.ToString().Trim(), HttpContext.Current.Session["ExchangeSegmentID"].ToString(),
                    rdbunderlyingall.Checked ? "ALL" : HiddenField_Product.Value.ToString().Trim(), rdbExpiryAll.Checked ? "ALL" : HiddenField_Expiry.Value.ToString().Trim(),
                    ddlGroup.SelectedItem.Value.ToString() == "0" ? "BRANCH" : ddlgrouptype.SelectedItem.Text.ToString().Trim(),
                    chktype, rbScreen.Checked ? "SHOW" : "PRINT");

                ViewState["dataset"] = ds;
                if (ds.Tables[0].Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(1);", true);
                }
                else
                {
                    htmltable();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Display", "Display();", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(2);", true);
            }

        }
        void htmltable()
        {
            ds = (DataSet)ViewState["dataset"];
            String strHtmldetail = String.Empty;
            String strHtmldetailreportheader = String.Empty;
            String strHtmldetailcolumnname = String.Empty;

            int colcount = 11;
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
            strHtmldetailreportheader = "<table width=\"95%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtmldetailreportheader += "<tr><td align=\"left\" colspan=" + colcount + " style=\"color:Blue;\">" + str + "</td></tr></table>";

            strHtmldetail = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";



            strHtmldetailcolumnname = "<tr style=\"background-color: #DBEEF3;\">";
            strHtmldetailcolumnname += "<td align=\"center\" ><b>Instrument</b></td>";
            strHtmldetailcolumnname += "<td align=\"center\" ><b>Type</b></td>";
            strHtmldetailcolumnname += "<td align=\"center\" ><b>Trade Date</b></td>";
            strHtmldetailcolumnname += "<td align=\"center\"><b>Sett Price</b></td>";
            strHtmldetailcolumnname += "<td align=\"center\"><b>Buy Qty</b></td>";
            strHtmldetailcolumnname += "<td align=\"center\"><b>Sell Qty</b></td>";
            strHtmldetailcolumnname += "<td align=\"center\"><b>Unit Price</b></td>";
            strHtmldetailcolumnname += "<td align=\"center\"><b>Sett.Chrg</b></td>";
            strHtmldetailcolumnname += "<td align=\"center\"><b>ServTax</b></td>";
            strHtmldetailcolumnname += "<td align=\"center\"><b>Amount Dr.</b></td>";
            strHtmldetailcolumnname += "<td align=\"center\"><b>Amount Cr.</b></td>";
            strHtmldetailcolumnname += "</tr>";

            int flag = 0;

            string group = null;
            string client = null;
            int i;
            for (i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (client != null)
                {
                    if (client != ds.Tables[0].Rows[i]["CUSTOMERID"].ToString())
                    {

                        strHtmldetail += "<tr >";
                        strHtmldetail += "<td style=\"color:#488AC7;\" align=\"left\" colspan=" + 9 + "><b>Client Total</b></td>";
                        if (ds.Tables[0].Rows[i - 1]["CLIENTAMTDR"] != DBNull.Value)
                            strHtmldetail += "<td align=\"right\" style=\"font-size:xx-small;color:#488AC7;\" title=\"Amount Dr.\">" + ds.Tables[0].Rows[i - 1]["CLIENTAMTDR"].ToString() + "</td>";
                        else
                            strHtmldetail += "<td align=\"left\" >&nbsp;</td>";

                        if (ds.Tables[0].Rows[i - 1]["CLIENTAMTCR"] != DBNull.Value)
                            strHtmldetail += "<td align=\"right\" style=\"font-size:xx-small;color:#488AC7;\" title=\"Amount Cr.\">" + ds.Tables[0].Rows[i - 1]["CLIENTAMTCR"].ToString() + "</td>";
                        else
                            strHtmldetail += "<td align=\"left\" >&nbsp;</td>";

                        strHtmldetail += "</tr>";
                    }
                }
                if (group != null)
                {
                    if (group != ds.Tables[0].Rows[i]["GRPID"].ToString())
                    {

                        strHtmldetail += "<tr>";
                        strHtmldetail += "<td style=\"color:#4C7D7E;\" align=\"left\" colspan=" + 9 + "><b>Group Total</b></td>";
                        if (ds.Tables[0].Rows[i - 1]["GRPAMTDR"] != DBNull.Value)
                            strHtmldetail += "<td align=\"right\" style=\"font-size:xx-small;color:#4C7D7E;\" title=\"Amount Dr.\">" + ds.Tables[0].Rows[i - 1]["GRPAMTDR"].ToString() + "</td>";
                        else
                            strHtmldetail += "<td align=\"left\" >&nbsp;</td>";

                        if (ds.Tables[0].Rows[i - 1]["GRPAMTCR"] != DBNull.Value)
                            strHtmldetail += "<td align=\"right\" style=\"font-size:xx-small;color:#4C7D7E;\" title=\"Amount Cr.\">" + ds.Tables[0].Rows[i - 1]["GRPAMTCR"].ToString() + "</td>";
                        else
                            strHtmldetail += "<td align=\"left\" >&nbsp;</td>";

                        strHtmldetail += "</tr>";
                    }
                }
                if (group != ds.Tables[0].Rows[i]["GRPID"].ToString())
                {

                    strHtmldetail += "<tr style=\"background-color:#D2B9D3 ;text-align:left\">";
                    strHtmldetail += "<td align=\"left\" colspan=" + colcount + ">Group Name :&nbsp;<b>" + ds.Tables[0].Rows[i]["GRPNAME"].ToString().Trim() + "</b></td></tr>";

                }
                if (client != ds.Tables[0].Rows[i]["CUSTOMERID"].ToString())
                {
                    strHtmldetail += "<tr style=\"background-color:#E3E4FA ;text-align:left\">";
                    strHtmldetail += "<td align=\"left\" colspan=" + colcount + ">Client Name :&nbsp;<b>" + ds.Tables[0].Rows[i]["CLIENTNAME"].ToString().Trim() + "</b>[ <b>" + ds.Tables[0].Rows[i]["UCC"].ToString() + " </b> ]</td></tr>";

                }

                group = ds.Tables[0].Rows[i]["GRPID"].ToString();
                client = ds.Tables[0].Rows[i]["CUSTOMERID"].ToString();

                if (i == 0)
                {
                    strHtmldetail += strHtmldetailcolumnname;
                }

                flag = flag + 1;
                strHtmldetail += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                strHtmldetail += "<td align=\"left\" style=\"font-size:xx-small;\" title=\"Instrument\">" + ds.Tables[0].Rows[i]["SYMBOL"].ToString() + "</td>";
                strHtmldetail += "<td align=\"left\" style=\"font-size:xx-small;\" title=\"Type\">" + ds.Tables[0].Rows[i]["TRADETYPE"].ToString() + "</td>";
                strHtmldetail += "<td align=\"left\" style=\"font-size:xx-small;\" title=\"Trade Date\">" + ds.Tables[0].Rows[i]["TRADEDATE"].ToString() + "</td>";

                if (ds.Tables[0].Rows[i]["SETTPRICE"] != DBNull.Value)
                    strHtmldetail += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Sett Price\">" + ds.Tables[0].Rows[i]["SETTPRICE"].ToString() + "</td>";
                else
                    strHtmldetail += "<td align=\"left\" >&nbsp;</td>";

                if (ds.Tables[0].Rows[i]["BUYQTY"] != DBNull.Value)
                    strHtmldetail += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Buy Qty\">" + ds.Tables[0].Rows[i]["BUYQTY"].ToString() + "</td>";
                else
                    strHtmldetail += "<td align=\"left\" >&nbsp;</td>";

                if (ds.Tables[0].Rows[i]["SELLQTY"] != DBNull.Value)
                    strHtmldetail += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Sell Qty\">" + ds.Tables[0].Rows[i]["SELLQTY"].ToString() + "</td>";
                else
                    strHtmldetail += "<td align=\"left\" >&nbsp;</td>";

                if (ds.Tables[0].Rows[i]["MKTPRICE"] != DBNull.Value)
                    strHtmldetail += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Mkt Price\">" + ds.Tables[0].Rows[i]["MKTPRICE"].ToString() + "</td>";
                else
                    strHtmldetail += "<td align=\"left\" >&nbsp;</td>";

                if (ds.Tables[0].Rows[i]["SETTCHARG"] != DBNull.Value)
                    strHtmldetail += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Sett Chrg\">" + ds.Tables[0].Rows[i]["SETTCHARG"].ToString() + "</td>";
                else
                    strHtmldetail += "<td align=\"left\" >&nbsp;</td>";

                if (ds.Tables[0].Rows[i]["SRVTAX"] != DBNull.Value)
                    strHtmldetail += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Serv Tax\">" + ds.Tables[0].Rows[i]["SRVTAX"].ToString() + "</td>";
                else
                    strHtmldetail += "<td align=\"left\" >&nbsp;</td>";

                if (ds.Tables[0].Rows[i]["AMTDR"] != DBNull.Value)
                    strHtmldetail += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Amount Dr.\">" + ds.Tables[0].Rows[i]["AMTDR"].ToString() + "</td>";
                else
                    strHtmldetail += "<td align=\"left\" >&nbsp;</td>";

                if (ds.Tables[0].Rows[i]["AMTCR"] != DBNull.Value)
                    strHtmldetail += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Amount Cr.\">" + ds.Tables[0].Rows[i]["AMTCR"].ToString() + "</td>";
                else
                    strHtmldetail += "<td align=\"left\" >&nbsp;</td>";
                strHtmldetail += "</tr>";
            }
            ///////////////////client total

            strHtmldetail += "<tr>";
            strHtmldetail += "<td style=\"color:#488AC7;\" align=\"left\" colspan=" + 9 + "><b>Client Total</b></td>";
            if (ds.Tables[0].Rows[i - 1]["CLIENTAMTDR"] != DBNull.Value)
                strHtmldetail += "<td align=\"right\" style=\"font-size:xx-small;color:#488AC7;\" title=\"Amount Dr.\">" + ds.Tables[0].Rows[i - 1]["CLIENTAMTDR"].ToString() + "</td>";
            else
                strHtmldetail += "<td align=\"left\" >&nbsp;</td>";

            if (ds.Tables[0].Rows[i - 1]["CLIENTAMTCR"] != DBNull.Value)
                strHtmldetail += "<td align=\"right\" style=\"font-size:xx-small;color:#488AC7;\" title=\"Amount Cr.\">" + ds.Tables[0].Rows[i - 1]["CLIENTAMTCR"].ToString() + "</td>";
            else
                strHtmldetail += "<td align=\"left\" >&nbsp;</td>";

            strHtmldetail += "</tr>";

            ///////////////////group total

            strHtmldetail += "<tr>";
            strHtmldetail += "<td style=\"color:#4C7D7E;\" align=\"left\" colspan=" + 9 + "><b>Group Total</b></td>";
            if (ds.Tables[0].Rows[i - 1]["GRPAMTDR"] != DBNull.Value)
                strHtmldetail += "<td align=\"right\" style=\"font-size:xx-small;color:#4C7D7E;\" title=\"Amount Dr.\">" + ds.Tables[0].Rows[i - 1]["GRPAMTDR"].ToString() + "</td>";
            else
                strHtmldetail += "<td align=\"left\" >&nbsp;</td>";

            if (ds.Tables[0].Rows[i - 1]["GRPAMTCR"] != DBNull.Value)
                strHtmldetail += "<td align=\"right\" style=\"font-size:xx-small;color:#4C7D7E;\" title=\"Amount Cr.\">" + ds.Tables[0].Rows[i - 1]["GRPAMTCR"].ToString() + "</td>";
            else
                strHtmldetail += "<td align=\"left\" >&nbsp;</td>";

            strHtmldetail += "</tr>";
            strHtmldetail += "</table>";

            DIVdisplayPERIOD.InnerHtml = strHtmldetailreportheader;
            display.InnerHtml = strHtmldetail;

        }
        protected string GetRowColor(int i)
        {
            if (i++ % 2 == 0)
                return "#fff0f5";
            else
                return "White";
        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            procedure();
            ds = (DataSet)ViewState["dataset"];
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlbandforgroup();
                export_clientwise();
            }
            export();

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
        void export_clientwise()
        {
            ds = (DataSet)ViewState["dataset"];
            //DataTable dtExport = new DataTable();
            dtExport.Clear();
            dtExport.Columns.Add("Instrument", Type.GetType("System.String"));
            dtExport.Columns.Add("Type", Type.GetType("System.String"));

            dtExport.Columns.Add("Trade Date", Type.GetType("System.String"));

            dtExport.Columns.Add("Sett Price", Type.GetType("System.String"));

            dtExport.Columns.Add("Buy Qty", Type.GetType("System.String"));

            dtExport.Columns.Add("Sell Qty", Type.GetType("System.String"));

            dtExport.Columns.Add("Unit Price", Type.GetType("System.String"));

            dtExport.Columns.Add("Sett Chrg", Type.GetType("System.String"));

            dtExport.Columns.Add("Serv Tax", Type.GetType("System.String"));

            dtExport.Columns.Add("Amount Dr", Type.GetType("System.String"));

            dtExport.Columns.Add("Amount Cr", Type.GetType("System.String"));


            for (int j = 0; j < cmbgroup.Items.Count; j++)
            {
                DataRow row = dtExport.NewRow();
                row[0] = ddlGroup.SelectedItem.Text.ToString().Trim() + " Name:" + cmbgroup.Items[j].Text.ToString().Trim();
                row[1] = "Test";
                dtExport.Rows.Add(row);

                DataView viewgrp = new DataView();
                viewgrp = ds.Tables[0].DefaultView;
                viewgrp.RowFilter = "GRPID='" + cmbgroup.Items[j].Value + "'";
                DataTable dt = new DataTable();
                dt = viewgrp.ToTable();

                DataView viewClient = new DataView(dt);
                Distinctclient = new DataTable();
                Distinctclient = viewClient.ToTable(true, new string[] { "CUSTOMERID", "CLIENTNAME", "UCC" });

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
                    row1[0] = "Client Name:" + cmbclient.Items[k].Text.ToString().Trim() + " [ " + Distinctclient.Rows[k]["UCC"].ToString().Trim() + " ]";
                    row1[1] = "Test";
                    dtExport.Rows.Add(row1);

                    DataView viewclient = new DataView();
                    viewclient = ds.Tables[0].DefaultView;
                    viewclient.RowFilter = "CUSTOMERID='" + cmbclient.Items[k].Value + "'";
                    DataTable dt1 = new DataTable();
                    dt1 = viewclient.ToTable();

                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        DataRow row2 = dtExport.NewRow();
                        row2[0] = dt1.Rows[i]["SYMBOL"].ToString();
                        row2[1] = dt1.Rows[i]["TRADETYPE"].ToString();
                        row2[2] = dt1.Rows[i]["TRADEDATE"].ToString();
                        row2[3] = dt1.Rows[i]["SETTPRICE"].ToString();
                        row2[4] = dt1.Rows[i]["BUYQTY"].ToString();
                        row2[5] = dt1.Rows[i]["SELLQTY"].ToString();
                        row2[6] = dt1.Rows[i]["MKTPRICE"].ToString();
                        row2[7] = dt1.Rows[i]["SETTCHARG"].ToString();
                        row2[8] = dt1.Rows[i]["SRVTAX"].ToString();

                        ////if (dt1.Rows[i]["TRADEDATE"] != DBNull.Value)
                        ////    row2["Trade Date"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["TRADEDATE"].ToString()));

                        ////    if (dt1.Rows[i]["SETTPRICE"] != DBNull.Value)
                        ////        row2["Sett Price"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["SETTPRICE"].ToString()));

                        ////if (dt1.Rows[i]["BUYQTY"] != DBNull.Value)
                        ////    row2["Buy Qty"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["BUYQTY"].ToString()));

                        ////    if (dt1.Rows[i]["SELLQTY"] != DBNull.Value)
                        ////        row2["Sell Qty"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["SELLQTY"].ToString()));

                        ////        if (dt1.Rows[i]["MKTPRICE"] != DBNull.Value)
                        ////            row2["Unit Price"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["MKTPRICE"].ToString()));

                        ////if (dt1.Rows[i]["SETTCHARG"] != DBNull.Value)
                        ////    row2["Sett Chrg"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["SETTCHARG"].ToString()));

                        ////if (dt1.Rows[i]["SRVTAX"] != DBNull.Value)
                        ////    row2["Serv Tax"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["SRVTAX"].ToString()));

                        if (dt1.Rows[i]["AMTDR"] != DBNull.Value)
                            row2["Amount Dr"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["AMTDR"].ToString()));

                        if (dt1.Rows[i]["AMTCR"] != DBNull.Value)
                            row2["Amount Cr"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["AMTCR"].ToString()));



                        dtExport.Rows.Add(row2);
                    }
                    //////////client total
                    DataRow row3 = dtExport.NewRow();
                    row3[0] = " Client Total :";

                    if (dt1.Rows[0]["CLIENTAMTDR"] != DBNull.Value)
                        row3["Amount Dr"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["CLIENTAMTDR"].ToString()));


                    if (dt1.Rows[0]["CLIENTAMTCR"] != DBNull.Value)
                        row3["Amount Cr"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["CLIENTAMTCR"].ToString()));

                    dtExport.Rows.Add(row3);

                    DataRow row4 = dtExport.NewRow();
                    row4[0] = " Group Total :";

                    if (dt1.Rows[0]["GRPAMTDR"] != DBNull.Value)
                        row4["Amount Dr"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["GRPAMTDR"].ToString()));


                    if (dt1.Rows[0]["GRPAMTCR"] != DBNull.Value)
                        row4["Amount Cr"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["GRPAMTCR"].ToString()));

                    dtExport.Rows.Add(row4);



                }

            }
            ViewState["dtExport"] = dtExport;
        }
        protected void cmbgroup_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        void export()
        {
            dtExport = (DataTable)ViewState["dtExport"];
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
            DrRowR1[0] = "Final Settlement Report:" + str;

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


            objExcel.ExportToExcelforExcel(dtExport, "Final Settlement", "Total :", dtReportHeader, dtReportFooter);

        }
    }
}