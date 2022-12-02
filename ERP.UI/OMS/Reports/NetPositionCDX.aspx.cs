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
    public partial class Reports_NetPositionCDX : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        DataSet ds = new DataSet();
        DataSet dsclient = new DataSet();
        DataTable Distinctclient = new DataTable();
        ExcelFile objExcel = new ExcelFile();
        DataTable dtExport = new DataTable();

        BusinessLogicLayer.Reports Reports = new BusinessLogicLayer.Reports();

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
            dtfrom.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
            dtto.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
            dtfor.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());

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
            ViewState["show"] = "SHOW";
            clientfetch();

        }
        void clientfetch()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            {

                //SqlCommand cmd = new SqlCommand();
                //cmd.Connection = con;
                //cmd.CommandText = "[CLIENTFETCHCDX]";

                //cmd.CommandType = CommandType.StoredProcedure;
                //if (ddldate.SelectedItem.Value.ToString() == "0")
                //{
                //    cmd.Parameters.AddWithValue("@fromdate", "NA");
                //    cmd.Parameters.AddWithValue("@todate", dtfor.Value);
                //}
                //else
                //{
                //    cmd.Parameters.AddWithValue("@fromdate", dtfrom.Value);
                //    cmd.Parameters.AddWithValue("@todate", dtto.Value);
                //}
                //cmd.Parameters.AddWithValue("@segment", Convert.ToInt32(Session["usersegid"].ToString()));
                //cmd.Parameters.AddWithValue("@MasterSegment", HttpContext.Current.Session["ExchangeSegmentID"].ToString());
                //cmd.Parameters.AddWithValue("@Companyid", Session["LastCompany"].ToString());
                //if (rdbClientALL.Checked)
                //{
                //    cmd.Parameters.AddWithValue("@ClientsID", "ALL");
                //}
                //else
                //{
                //    cmd.Parameters.AddWithValue("@ClientsID", HiddenField_Client.Value);
                //}
                //if (rdbScripall.Checked)
                //{
                //    cmd.Parameters.AddWithValue("@INSTRUMENT", "ALL");
                //}
                //else
                //{
                //    cmd.Parameters.AddWithValue("@INSTRUMENT", HiddenField_Scrip.Value);
                //}

                //if (ddlGroup.SelectedItem.Value.ToString() == "0")
                //{
                //    cmd.Parameters.AddWithValue("@GRPTYPE", "BRANCH");

                //}
                //else
                //{
                //    cmd.Parameters.AddWithValue("@GRPTYPE", ddlgrouptype.SelectedItem.Text.ToString().Trim());

                //}
                //cmd.Parameters.AddWithValue("@Branch", Session["userbranchHierarchy"].ToString());
                //if (rdddlgrouptypeAll.Checked)
                //{
                //    cmd.Parameters.AddWithValue("@GROUP", "ALL");
                //}
                //else
                //{
                //    cmd.Parameters.AddWithValue("@GROUP", HiddenField_Group.Value);
                //}
                //if (chkopen.Checked)
                //{
                //    cmd.Parameters.AddWithValue("@Open", "CHK");
                //}
                //else
                //{
                //    cmd.Parameters.AddWithValue("@Open", "UNCHK");
                //}
                //SqlDataAdapter da = new SqlDataAdapter();
                //da.SelectCommand = cmd;
                //cmd.CommandTimeout = 0;
                //dsclient.Reset();
                //da.Fill(dsclient);
                //da.Dispose();
                //ViewState["dsclient"] = dsclient;


                dsclient.Reset();
                dsclient = Reports.NetPositionCDX_CLIENTFETCHCDX(ddldate.SelectedItem.Value.ToString(), Convert.ToString(dtfor.Value), Convert.ToString(dtfrom.Value),
                    Session["usersegid"].ToString(), HttpContext.Current.Session["ExchangeSegmentID"].ToString(), Session["LastCompany"].ToString(), rdbClientALL.Checked,
                    HiddenField_Client.Value, rdbScripall.Checked, HiddenField_Scrip.Value, ddlGroup.SelectedItem.Value.ToString(), ddlgrouptype.SelectedItem.Text.ToString().Trim(),
                    Session["userbranchHierarchy"].ToString(), rdddlgrouptypeAll.Checked, HiddenField_Group.Value, chkopen.Checked);
                ViewState["dsclient"] = dsclient;

                if (dsclient.Tables[0].Rows[0]["status"].ToString() == "1")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(1);", true);

                }
                else
                {
                    if (dsclient.Tables[1].Rows.Count == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(2);", true);
                    }
                    else
                    {
                        clientbind();
                    }
                }

            }

        }

        void clientbind()
        {
            dsclient = (DataSet)ViewState["dsclient"];
            string Clients = null;
            if (dsclient.Tables[1].Rows.Count > 0)
            {
                for (int i = 0; i < dsclient.Tables[1].Rows.Count; i++)
                {
                    if (Clients == null)
                        Clients = "'" + dsclient.Tables[1].Rows[i][0].ToString() + "'";
                    else
                        Clients += "," + "'" + dsclient.Tables[1].Rows[i][0].ToString() + "'";
                }

                cmbclient.DataSource = dsclient.Tables[1];
                cmbclient.DataValueField = "client";
                cmbclient.DataTextField = "Name";
                cmbclient.DataBind();

                ViewState["CLIENTS"] = Clients;
            }
            procedure();
            ViewState["show"] = "SHOW";
        }
        void scripbind()
        {
            ds = (DataSet)ViewState["dataset"];
            string Clients = null;
            if (ds.Tables[0].Rows.Count > 0)
            {
                cmbclient.DataSource = ds.Tables[0];
                cmbclient.DataValueField = "tabProductSeriesID";
                cmbclient.DataTextField = "SCRIP";
                cmbclient.DataBind();

            }
            html_sharewise();
        }
        void procedure()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            {

                //SqlCommand cmd = new SqlCommand();
                //cmd.Connection = con;
                //cmd.CommandText = "[NetPositionCDX]";

                //cmd.CommandType = CommandType.StoredProcedure;
                //if (ddldate.SelectedItem.Value.ToString() == "0")
                //{
                //    cmd.Parameters.AddWithValue("@fromdate", "NA");
                //    cmd.Parameters.AddWithValue("@todate", dtfor.Value);
                //}
                //else
                //{
                //    cmd.Parameters.AddWithValue("@fromdate", dtfrom.Value);
                //    cmd.Parameters.AddWithValue("@todate", dtto.Value);
                //}
                //cmd.Parameters.AddWithValue("@segment", Convert.ToInt32(Session["usersegid"].ToString()));
                //cmd.Parameters.AddWithValue("@MasterSegment", HttpContext.Current.Session["ExchangeSegmentID"].ToString());
                //cmd.Parameters.AddWithValue("@companyid", Session["LastCompany"].ToString());

                //if (ViewState["show"] == "SHOW")
                //{
                //    cmd.Parameters.AddWithValue("@ClientsID", "'" + cmbclient.SelectedItem.Value + "'");
                //}
                //else
                //{
                //    cmd.Parameters.AddWithValue("@ClientsID", ViewState["CLIENTS"].ToString().Trim());
                //}
                //if (rdbScripall.Checked)
                //{
                //    cmd.Parameters.AddWithValue("@INSTRUMENT", "ALL");
                //}
                //else
                //{
                //    cmd.Parameters.AddWithValue("@INSTRUMENT", HiddenField_Scrip.Value);
                //}
                //if (chkopen.Checked)
                //{
                //    cmd.Parameters.AddWithValue("@Open", "CHK");
                //}
                //else
                //{
                //    cmd.Parameters.AddWithValue("@Open", "UNCHK");
                //}
                //if (chkopenbfpositive.Checked)
                //{
                //    cmd.Parameters.AddWithValue("@Sign", "CHK");
                //}
                //else
                //{
                //    cmd.Parameters.AddWithValue("@Sign", "UNCHK");
                //}
                //cmd.Parameters.AddWithValue("@RPTTYPE", ddlrptview.SelectedItem.Value.ToString().Trim());
                //SqlDataAdapter da = new SqlDataAdapter();
                //da.SelectCommand = cmd;
                //cmd.CommandTimeout = 0;
                //ds.Reset();
                //da.Fill(ds);
                //da.Dispose();
                //ViewState["dataset"] = ds;

                ds.Reset();
                ds = Reports.NetPositionCDX_NetPositionCDX(ddldate.SelectedItem.Value.ToString(), Convert.ToString(dtfor.Value), Convert.ToString(dtfrom.Value), Session["usersegid"].ToString(),
                    HttpContext.Current.Session["ExchangeSegmentID"].ToString(), Session["LastCompany"].ToString(), Convert.ToString(ViewState["show"]),
                    "'" + cmbclient.SelectedItem.Value + "'", ViewState["CLIENTS"].ToString().Trim(), rdbScripall.Checked, HiddenField_Scrip.Value,
                    chkopen.Checked, chkopenbfpositive.Checked, ddlrptview.SelectedItem.Value.ToString().Trim());
                ViewState["dataset"] = ds;

                if (ds.Tables[0].Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(2);", true);
                }
                else
                {
                    if (ddlrptview.SelectedItem.Value.ToString().Trim() == "0")
                    {
                        bind_Details();
                    }
                    else
                    {
                        scripbind();
                    }
                }

            }

        }
        void bind_Details()
        {
            ds = (DataSet)ViewState["dataset"];
            cmbclient.SelectedIndex = CurrentPage;
            if (LastPage > -1)
            {
                listRecord.Text = CurrentPage + 1 + " of " + ds.Tables[0].Rows.Count.ToString() + " Record.";

            }
            html_clientwise();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Display", "Display(1);", true);
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
            if (ddlrptview.SelectedItem.Value.ToString().Trim() == "0")
            {
                ViewState["show"] = "SHOW";
                procedure();
                html_clientwise();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Display", "Display(1);", true);
            }
            else
            {
                html_sharewise();
            }
        }

        void html_clientwise()
        {
            ds = (DataSet)ViewState["dataset"];
            String strHtml = String.Empty;
            String strHtml1 = String.Empty;
            int flag = 0;
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
            strHtml1 += "<tr><td align=\"left\" colspan=10 style=\"color:Blue;\">" + str + "</td></tr></table>";

            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\"><b>Instrument</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\"><b>Expiry </br> Date</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\"><b>B/F </br> Qty</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\"><b>Open </br> Price</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\"><b>B/F </br> Value</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\"><b>Day</br> Buy</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\"><b>Buy </br>Value</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\"><b>Buy </br>Avg.</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\"><b>Day </br>Sell</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\"><b>Sell </br> Value</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\"><b>Sell </br> Avg.</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\"><b>ASN/EXC </br> Qty</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\"><b>C/F </br> Qty</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\"><b>Sett </br> Price</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\"><b>C/F </br> Value</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\"><b>Premium</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\"><b>Mtm</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\"><b>Future </br> FinSett</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\"><b>ASN/EXC </br> Amount</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\"><b>Net </br> Obligation</b></td>";
            strHtml += "</tr>";

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";

                if (ds.Tables[0].Rows[i]["Symbol"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + ds.Tables[0].Rows[i]["Symbol"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (ds.Tables[0].Rows[i]["ExpiryDate"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + ds.Tables[0].Rows[i]["ExpiryDate"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (ds.Tables[0].Rows[i]["LOTSRESULT_B"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvalueWithounDecimalPlace(Convert.ToDecimal(ds.Tables[0].Rows[i]["LOTSRESULT_B"].ToString())) + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (ds.Tables[0].Rows[i]["SettlementPrice"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["SettlementPrice"].ToString())) + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (ds.Tables[0].Rows[i]["BFVALUE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["BFVALUE"].ToString())) + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (ds.Tables[0].Rows[i]["DAYBUY"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvalueWithounDecimalPlace(Convert.ToDecimal(ds.Tables[0].Rows[i]["DAYBUY"].ToString())) + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (ds.Tables[0].Rows[i]["BUYVALUE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["BUYVALUE"].ToString())) + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (ds.Tables[0].Rows[i]["BuyAvg"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["BuyAvg"].ToString())) + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (ds.Tables[0].Rows[i]["DAYSELL"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvalueWithounDecimalPlace(Convert.ToDecimal(ds.Tables[0].Rows[i]["DAYSELL"].ToString())) + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (ds.Tables[0].Rows[i]["SELLVALUE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["SELLVALUE"].ToString())) + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (ds.Tables[0].Rows[i]["SellAvg"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["SellAvg"].ToString())) + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (ds.Tables[0].Rows[i]["DAYQTYEXCASN"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["DAYQTYEXCASN"].ToString())) + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (ds.Tables[0].Rows[i]["CFQTY_I"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["CFQTY_I"].ToString())) + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (ds.Tables[0].Rows[i]["SETTPRICE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvalueWithounDecimalPlace(Convert.ToDecimal(ds.Tables[0].Rows[i]["SETTPRICE"].ToString())) + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (ds.Tables[0].Rows[i]["CFVALUE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["CFVALUE"].ToString())) + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (ds.Tables[0].Rows[i]["PRM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["PRM"].ToString())) + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (ds.Tables[0].Rows[i]["MTM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["MTM"].ToString())) + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (ds.Tables[0].Rows[i]["FINSETT"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["FINSETT"].ToString())) + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (ds.Tables[0].Rows[i]["ExcAsn"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["ExcAsn"].ToString())) + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (ds.Tables[0].Rows[i]["NETOBLIGATION"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["NETOBLIGATION"].ToString())) + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                strHtml += "</tr>";
            }

            flag = flag + 1;
            strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
            strHtml += "<td align=\"left\" style=\"color:#maroon\" colspan=4><b>Total :</b></td>";


            if (ds.Tables[1].Rows[0]["BFVALUE_Sum"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[1].Rows[0]["BFVALUE_Sum"].ToString())) + "</td>";
            else
                strHtml += "<td align=\"left\" >&nbsp;</td>";

            strHtml += "<td align=\"left\" >&nbsp;</td>";

            if (ds.Tables[1].Rows[0]["BUYVALUE_Sum"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[1].Rows[0]["BUYVALUE_Sum"].ToString())) + "</td>";
            else
                strHtml += "<td align=\"left\" >&nbsp;</td>";

            if (ds.Tables[1].Rows[0]["BuyAvg_Sum"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[1].Rows[0]["BuyAvg_Sum"].ToString())) + "</td>";
            else
                strHtml += "<td align=\"left\" >&nbsp;</td>";

            strHtml += "<td align=\"left\" >&nbsp;</td>";

            if (ds.Tables[1].Rows[0]["SELLVALUE_Sum"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[1].Rows[0]["SELLVALUE_Sum"].ToString())) + "</td>";
            else
                strHtml += "<td align=\"left\" >&nbsp;</td>";

            if (ds.Tables[1].Rows[0]["SellAvg_Sum"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[1].Rows[0]["SellAvg_Sum"].ToString())) + "</td>";
            else
                strHtml += "<td align=\"left\" >&nbsp;</td>";

            strHtml += "<td align=\"left\" >&nbsp;</td>";
            strHtml += "<td align=\"left\" >&nbsp;</td>";
            strHtml += "<td align=\"left\" >&nbsp;</td>";

            if (ds.Tables[1].Rows[0]["CFVALUE_Sum"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[1].Rows[0]["CFVALUE_Sum"].ToString())) + "</td>";
            else
                strHtml += "<td align=\"left\" >&nbsp;</td>";

            if (ds.Tables[1].Rows[0]["PRM_Sum"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[1].Rows[0]["PRM_Sum"].ToString())) + "</td>";
            else
                strHtml += "<td align=\"left\" >&nbsp;</td>";

            if (ds.Tables[1].Rows[0]["MTM_Sum"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[1].Rows[0]["MTM_Sum"].ToString())) + "</td>";
            else
                strHtml += "<td align=\"left\" >&nbsp;</td>";

            if (ds.Tables[1].Rows[0]["FINSETT_Sum"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[1].Rows[0]["FINSETT_Sum"].ToString())) + "</td>";
            else
                strHtml += "<td align=\"left\" >&nbsp;</td>";

            if (ds.Tables[1].Rows[0]["ExcAsn_Sum"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[1].Rows[0]["ExcAsn_Sum"].ToString())) + "</td>";
            else
                strHtml += "<td align=\"left\" >&nbsp;</td>";

            if (ds.Tables[1].Rows[0]["NETOBLIGATION_Sum"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[1].Rows[0]["NETOBLIGATION_Sum"].ToString())) + "</td>";
            else
                strHtml += "<td align=\"left\" >&nbsp;</td>";

            strHtml += "</tr>";

            /////////////CHARGES DISPLAY
            if (ds.Tables[2].Rows[0]["BRKG"] != DBNull.Value)
            {
                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                strHtml += "<td align=\"left\" style=\"color:black\" colspan=19><b>STax On Brkg :</b></td>";
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + ds.Tables[2].Rows[0]["BRKG"].ToString() + "</td>";
                strHtml += "</tr>";
            }
            if (ddldate.SelectedItem.Value.ToString().Trim() == "0")
            {
                if (ds.Tables[2].Rows[0]["TRANCHARGE"] != DBNull.Value)
                {
                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    strHtml += "<td align=\"left\" style=\"color:black\" colspan=19><b>Transaction Charges :</b></td>";
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + ds.Tables[2].Rows[0]["TRANCHARGE"].ToString() + "</td>";
                    strHtml += "</tr>";
                }
                if (ds.Tables[2].Rows[0]["SRVTRANCHARGE"] != DBNull.Value)
                {
                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    strHtml += "<td align=\"left\" style=\"color:black\" colspan=19><b>STax On Trn.Charges :</b></td>";
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + ds.Tables[2].Rows[0]["SRVTRANCHARGE"].ToString() + "</td>";
                    strHtml += "</tr>";
                }
                if (ds.Tables[2].Rows[0]["STAMPDUTY"] != DBNull.Value)
                {
                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    strHtml += "<td align=\"left\" style=\"color:black\" colspan=19><b>Stamp Duty :</b></td>";
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + ds.Tables[2].Rows[0]["STAMPDUTY"].ToString() + "</td>";
                    strHtml += "</tr>";
                }
                if (ds.Tables[2].Rows[0]["SEBI"] != DBNull.Value)
                {
                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    strHtml += "<td align=\"left\" style=\"color:black\" colspan=19><b>SEBI Fee :</b></td>";
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + ds.Tables[2].Rows[0]["SEBI"].ToString() + "</td>";
                    strHtml += "</tr>";
                }
                if (ds.Tables[2].Rows[0]["OTHERCHARGE"] != DBNull.Value)
                {
                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    strHtml += "<td align=\"left\" style=\"color:black\" colspan=19><b>" + ds.Tables[2].Rows[0]["OTHERCHARGENAME"].ToString() + "</b></td>";
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + ds.Tables[2].Rows[0]["OTHERCHARGE"].ToString() + "</td>";
                    strHtml += "</tr>";
                }
                if (ds.Tables[2].Rows[0]["OTHERCHARGESRV"] != DBNull.Value)
                {
                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    strHtml += "<td align=\"left\" style=\"color:black\" colspan=19><b>STax On :" + ds.Tables[2].Rows[0]["OTHERCHARGENAME"].ToString() + "</b></td>";
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + ds.Tables[2].Rows[0]["OTHERCHARGESRV"].ToString() + "</td>";
                    strHtml += "</tr>";
                }

            }
            if (ds.Tables[2].Rows[0]["NETTOTAL"] != DBNull.Value)
            {
                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                strHtml += "<td align=\"left\" style=\"color:black\" colspan=19><b>Net Total :</b></td>";
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + ds.Tables[2].Rows[0]["NETTOTAL"].ToString() + "</td>";
                strHtml += "</tr>";
            }
            ///////////DIV BIND
            DIVdisplayPERIOD.InnerHtml = strHtml1;
            display.InnerHtml = strHtml;
        }
        void html_sharewise()
        {
            ds = (DataSet)ViewState["dataset"];
            String strHtml = String.Empty;
            String strHtml1 = String.Empty;
            int flag = 0;
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
            strHtml1 += "<tr><td align=\"left\" colspan=10 style=\"color:Blue;\">" + str + "</td></tr></table>";

            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\"><b>B/F </br> Qty</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\"><b>Open </br> Price</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\"><b>B/F </br> Value</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\"><b>Day</br> Buy</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\"><b>Buy </br>Value</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\"><b>Buy </br>Avg.</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\"><b>Day </br>Sell</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\"><b>Sell </br> Value</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\"><b>Sell </br> Avg.</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\"><b>ASN/EXC </br> Qty</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\"><b>C/F </br> Qty</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\"><b>Sett </br> Price</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\"><b>C/F </br> Value</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\"><b>Premium</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\"><b>Mtm</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\"><b>Future </br> FinSett</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\"><b>ASN/EXC </br> Amount</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\"><b>Net </br> Obligation</b></td>";
            strHtml += "</tr>";



            DataView viewscrip = new DataView();
            viewscrip = ds.Tables[0].DefaultView;
            viewscrip.RowFilter = "tabProductSeriesID='" + cmbclient.SelectedItem.Value + "'";
            DataTable dt1 = new DataTable();
            dt1 = viewscrip.ToTable();

            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                flag = flag + 1;
                DataRow row = dtExport.NewRow();
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";

                if (dt1.Rows[i]["LOTSRESULT_B"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvalueWithounDecimalPlace(Convert.ToDecimal(dt1.Rows[i]["LOTSRESULT_B"].ToString())) + "</td>";
                }
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (dt1.Rows[i]["SettlementPrice"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt1.Rows[i]["SettlementPrice"].ToString())) + "</td>";
                }
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (dt1.Rows[i]["BFVALUE"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt1.Rows[i]["BFVALUE"].ToString())) + "</td>";
                }
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (dt1.Rows[i]["DAYBUY"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvalueWithounDecimalPlace(Convert.ToDecimal(dt1.Rows[i]["DAYBUY"].ToString())) + "</td>";
                }
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (dt1.Rows[i]["BUYVALUE"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt1.Rows[i]["BUYVALUE"].ToString())) + "</td>";
                }
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (dt1.Rows[i]["BuyAvg"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt1.Rows[i]["BuyAvg"].ToString())) + "</td>";
                }
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (dt1.Rows[i]["DAYSELL"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvalueWithounDecimalPlace(Convert.ToDecimal(dt1.Rows[i]["DAYSELL"].ToString())) + "</td>";
                }
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (dt1.Rows[i]["SELLVALUE"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt1.Rows[i]["SELLVALUE"].ToString())) + "</td>";
                }
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (dt1.Rows[i]["SellAvg"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt1.Rows[i]["SellAvg"].ToString())) + "</td>";
                }
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (dt1.Rows[i]["DAYQTYEXCASN"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt1.Rows[i]["DAYQTYEXCASN"].ToString())) + "</td>";
                }
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (dt1.Rows[i]["CFQTY_I"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt1.Rows[i]["CFQTY_I"].ToString())) + "</td>";
                }
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (dt1.Rows[i]["SETTPRICE"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvalueWithounDecimalPlace(Convert.ToDecimal(dt1.Rows[i]["SETTPRICE"].ToString())) + "</td>";
                }
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (dt1.Rows[i]["CFVALUE"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt1.Rows[i]["CFVALUE"].ToString())) + "</td>";
                }
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (dt1.Rows[i]["PRM"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt1.Rows[i]["PRM"].ToString())) + "</td>";
                }
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (dt1.Rows[i]["MTM"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt1.Rows[i]["MTM"].ToString())) + "</td>";
                }
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (dt1.Rows[i]["FINSETT"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt1.Rows[i]["FINSETT"].ToString())) + "</td>";
                }
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (dt1.Rows[i]["ExcAsn"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt1.Rows[i]["ExcAsn"].ToString())) + "</td>";
                }
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (dt1.Rows[i]["NETOBLIGATION"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt1.Rows[i]["NETOBLIGATION"].ToString())) + "</td>";
                }
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                strHtml += "</tr>";

            }

            DataView viewscrip1 = new DataView();
            viewscrip1 = ds.Tables[1].DefaultView;
            viewscrip1.RowFilter = "tabProductSeriesID='" + cmbclient.SelectedItem.Value + "'";
            DataTable dt2 = new DataTable();
            dt2 = viewscrip1.ToTable();

            flag = flag + 1;
            strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
            strHtml += "<td align=\"left\" style=\"color:#maroon\" colspan=2><b>Total :</b></td>";

            if (dt2.Rows[0]["BFVALUE_Sum"] != DBNull.Value)
            {
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt2.Rows[0]["BFVALUE_Sum"].ToString())) + "</td>";
            }
            else
                strHtml += "<td align=\"left\" >&nbsp;</td>";

            strHtml += "<td align=\"left\" >&nbsp;</td>";

            if (dt2.Rows[0]["BUYVALUE_Sum"] != DBNull.Value)
            {
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt2.Rows[0]["BUYVALUE_Sum"].ToString())) + "</td>";
            }
            else
                strHtml += "<td align=\"left\" >&nbsp;</td>";

            if (dt2.Rows[0]["BuyAvg_Sum"] != DBNull.Value)
            {
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt2.Rows[0]["BuyAvg_Sum"].ToString())) + "</td>";
            }
            else
                strHtml += "<td align=\"left\" >&nbsp;</td>";

            strHtml += "<td align=\"left\" >&nbsp;</td>";

            if (dt2.Rows[0]["SELLVALUE_Sum"] != DBNull.Value)
            {
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt2.Rows[0]["SELLVALUE_Sum"].ToString())) + "</td>";
            }
            else
                strHtml += "<td align=\"left\" >&nbsp;</td>";

            if (dt2.Rows[0]["SellAvg_Sum"] != DBNull.Value)
            {
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt2.Rows[0]["SellAvg_Sum"].ToString())) + "</td>";
            }
            else
                strHtml += "<td align=\"left\" >&nbsp;</td>";

            strHtml += "<td align=\"left\" >&nbsp;</td>";
            strHtml += "<td align=\"left\" >&nbsp;</td>";
            strHtml += "<td align=\"left\" >&nbsp;</td>";

            if (dt2.Rows[0]["CFVALUE_Sum"] != DBNull.Value)
            {
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt2.Rows[0]["CFVALUE_Sum"].ToString())) + "</td>";
            }
            else
                strHtml += "<td align=\"left\" >&nbsp;</td>";

            if (dt2.Rows[0]["PRM_Sum"] != DBNull.Value)
            {
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt2.Rows[0]["PRM_Sum"].ToString())) + "</td>";
            }
            else
                strHtml += "<td align=\"left\" >&nbsp;</td>";

            if (dt2.Rows[0]["MTM_Sum"] != DBNull.Value)
            {
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt2.Rows[0]["MTM_Sum"].ToString())) + "</td>";
            }
            else
                strHtml += "<td align=\"left\" >&nbsp;</td>";

            if (dt2.Rows[0]["FINSETT_Sum"] != DBNull.Value)
            {
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt2.Rows[0]["FINSETT_Sum"].ToString())) + "</td>";
            }
            else
                strHtml += "<td align=\"left\" >&nbsp;</td>";

            if (dt2.Rows[0]["ExcAsn_Sum"] != DBNull.Value)
            {
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt2.Rows[0]["ExcAsn_Sum"].ToString())) + "</td>";
            }
            else
                strHtml += "<td align=\"left\" >&nbsp;</td>";

            if (dt2.Rows[0]["NETOBLIGATION_Sum"] != DBNull.Value)
            {
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt2.Rows[0]["NETOBLIGATION_Sum"].ToString())) + "</td>";
            }
            else
                strHtml += "<td align=\"left\" >&nbsp;</td>";

            strHtml += "</tr>";


            ///////////DIV BIND
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Display", "Display(2);", true);

            DIVdisplayPERIOD.InnerHtml = strHtml1;
            display.InnerHtml = strHtml;
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
        void exportsharewise()
        {
            ds = (DataSet)ViewState["dataset"];
            ///////////FOR EXPORT BEGIN
            dtExport = new DataTable();
            dtExport.Clear();
            dtExport.Columns.Add("B/F Qty", Type.GetType("System.String"));
            dtExport.Columns.Add("Open Price", Type.GetType("System.String"));
            dtExport.Columns.Add("B/F Value", Type.GetType("System.String"));
            dtExport.Columns.Add("Day Buy", Type.GetType("System.String"));
            dtExport.Columns.Add("Buy Value", Type.GetType("System.String"));
            dtExport.Columns.Add("Buy Avg", Type.GetType("System.String"));
            dtExport.Columns.Add("Day Sell", Type.GetType("System.String"));
            dtExport.Columns.Add("Sell Value", Type.GetType("System.String"));
            dtExport.Columns.Add("Sell Avg", Type.GetType("System.String"));
            dtExport.Columns.Add("ASN/EXC Qty", Type.GetType("System.String"));
            dtExport.Columns.Add("C/F Qty", Type.GetType("System.String"));
            dtExport.Columns.Add("Sett Price", Type.GetType("System.String"));
            dtExport.Columns.Add("C/F Value", Type.GetType("System.String"));
            dtExport.Columns.Add("Premium", Type.GetType("System.String"));
            dtExport.Columns.Add("MTM", Type.GetType("System.String"));
            dtExport.Columns.Add("Future FinSett", Type.GetType("System.String"));
            dtExport.Columns.Add("ASN/EXC Amount", Type.GetType("System.String"));
            dtExport.Columns.Add("Net Obligation", Type.GetType("System.String"));
            ////FOR EXPORT END

            for (int j = 0; j < cmbclient.Items.Count; j++)
            {
                DataRow rowname = dtExport.NewRow();
                rowname["B/F Qty"] = "Scrip Name:" + cmbclient.Items[j].Text.ToString().Trim();
                rowname["Open Price"] = "Test";
                dtExport.Rows.Add(rowname);

                DataView viewshare = new DataView();
                viewshare = ds.Tables[0].DefaultView;
                viewshare.RowFilter = "tabProductSeriesID='" + cmbclient.Items[j].Value + "'";
                DataTable dt1 = new DataTable();
                dt1 = viewshare.ToTable();


                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    DataRow row = dtExport.NewRow();
                    if (dt1.Rows[i]["LOTSRESULT_B"] != DBNull.Value)
                        row[0] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["LOTSRESULT_B"].ToString()));
                    if (dt1.Rows[i]["SettlementPrice"] != DBNull.Value)
                        row[1] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["SettlementPrice"].ToString()));
                    if (dt1.Rows[i]["BFVALUE"] != DBNull.Value)
                        row[2] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["BFVALUE"].ToString()));
                    if (dt1.Rows[i]["DAYBUY"] != DBNull.Value)
                        row[3] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["DAYBUY"].ToString()));
                    if (dt1.Rows[i]["BUYVALUE"] != DBNull.Value)
                        row[4] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["BUYVALUE"].ToString()));
                    if (dt1.Rows[i]["BuyAvg"] != DBNull.Value)
                        row[5] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["BuyAvg"].ToString()));
                    if (dt1.Rows[i]["DAYSELL"] != DBNull.Value)
                        row[6] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["DAYSELL"].ToString()));
                    if (dt1.Rows[i]["SELLVALUE"] != DBNull.Value)
                        row[7] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["SELLVALUE"].ToString()));
                    if (dt1.Rows[i]["SellAvg"] != DBNull.Value)
                        row[8] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["SellAvg"].ToString()));
                    if (dt1.Rows[i]["DAYQTYEXCASN"] != DBNull.Value)
                        row[9] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["DAYQTYEXCASN"].ToString()));
                    if (dt1.Rows[i]["CFQTY_I"] != DBNull.Value)
                        row[10] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["CFQTY_I"].ToString()));
                    if (dt1.Rows[i]["SETTPRICE"] != DBNull.Value)
                        row[11] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["SETTPRICE"].ToString()));
                    if (dt1.Rows[i]["CFVALUE"] != DBNull.Value)
                        row[12] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["CFVALUE"].ToString()));
                    if (dt1.Rows[i]["PRM"] != DBNull.Value)
                        row[13] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["PRM"].ToString()));
                    if (dt1.Rows[i]["MTM"] != DBNull.Value)
                        row[14] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["MTM"].ToString()));
                    if (dt1.Rows[i]["FINSETT"] != DBNull.Value)
                        row[15] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["FINSETT"].ToString()));
                    if (dt1.Rows[i]["ExcAsn"] != DBNull.Value)
                        row[16] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["ExcAsn"].ToString()));
                    if (dt1.Rows[i]["NETOBLIGATION"] != DBNull.Value)
                        row[17] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["NETOBLIGATION"].ToString()));
                    dtExport.Rows.Add(row);
                }
                DataView viewshare1 = new DataView();
                viewshare1 = ds.Tables[1].DefaultView;
                viewshare1.RowFilter = "tabProductSeriesID='" + cmbclient.Items[j].Value + "'";
                DataTable dt2 = new DataTable();
                dt2 = viewshare1.ToTable();



                DataRow row1 = dtExport.NewRow();
                row1["B/F Qty"] = "Total";
                if (dt2.Rows[0]["BFVALUE_Sum"] != DBNull.Value)
                    row1[2] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt2.Rows[0]["BFVALUE_Sum"].ToString()));
                if (dt2.Rows[0]["BUYVALUE_Sum"] != DBNull.Value)
                    row1[4] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt2.Rows[0]["BUYVALUE_Sum"].ToString()));
                if (dt2.Rows[0]["BuyAvg_Sum"] != DBNull.Value)
                    row1[5] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt2.Rows[0]["BuyAvg_Sum"].ToString()));
                if (dt2.Rows[0]["SELLVALUE_Sum"] != DBNull.Value)
                    row1[7] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt2.Rows[0]["SELLVALUE_Sum"].ToString()));
                if (dt2.Rows[0]["SellAvg_Sum"] != DBNull.Value)
                    row1[8] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt2.Rows[0]["SellAvg_Sum"].ToString()));
                if (dt2.Rows[0]["CFVALUE_Sum"] != DBNull.Value)
                    row1[12] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt2.Rows[0]["CFVALUE_Sum"].ToString()));
                if (dt2.Rows[0]["PRM_Sum"] != DBNull.Value)
                    row1[13] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt2.Rows[0]["PRM_Sum"].ToString()));
                if (dt2.Rows[0]["MTM_Sum"] != DBNull.Value)
                    row1[14] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt2.Rows[0]["MTM_Sum"].ToString()));
                if (dt2.Rows[0]["FINSETT_Sum"] != DBNull.Value)
                    row1[15] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt2.Rows[0]["FINSETT_Sum"].ToString()));
                if (dt2.Rows[0]["ExcAsn_Sum"] != DBNull.Value)
                    row1[16] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt2.Rows[0]["ExcAsn_Sum"].ToString()));
                if (dt2.Rows[0]["NETOBLIGATION_Sum"] != DBNull.Value)
                    row1[17] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt2.Rows[0]["NETOBLIGATION_Sum"].ToString()));

                dtExport.Rows.Add(row1);
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

            DrRowR1[0] = "Net Position Report:" + str;

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

            if (cmbExport.SelectedItem.Value == "E")
            {
                objExcel.ExportToExcelforExcel(dtExport, "Net Position Report", "Total", dtReportHeader, dtReportFooter);
            }
        }
        void exportclientwise()
        {
            ds = (DataSet)ViewState["dataset"];
            ///////////FOR EXPORT BEGIN
            dtExport = new DataTable();
            dtExport.Clear();
            dtExport.Columns.Add("Instrument", Type.GetType("System.String"));
            dtExport.Columns.Add("Expiry Date", Type.GetType("System.String"));
            dtExport.Columns.Add("B/F Qty", Type.GetType("System.String"));
            dtExport.Columns.Add("Open Price", Type.GetType("System.String"));
            dtExport.Columns.Add("B/F Value", Type.GetType("System.String"));
            dtExport.Columns.Add("Day Buy", Type.GetType("System.String"));
            dtExport.Columns.Add("Buy Value", Type.GetType("System.String"));
            dtExport.Columns.Add("Buy Avg", Type.GetType("System.String"));
            dtExport.Columns.Add("Day Sell", Type.GetType("System.String"));
            dtExport.Columns.Add("Sell Value", Type.GetType("System.String"));
            dtExport.Columns.Add("Sell Avg", Type.GetType("System.String"));
            dtExport.Columns.Add("ASN/EXC Qty", Type.GetType("System.String"));
            dtExport.Columns.Add("C/F Qty", Type.GetType("System.String"));
            dtExport.Columns.Add("Sett Price", Type.GetType("System.String"));
            dtExport.Columns.Add("C/F Value", Type.GetType("System.String"));
            dtExport.Columns.Add("Premium", Type.GetType("System.String"));
            dtExport.Columns.Add("MTM", Type.GetType("System.String"));
            dtExport.Columns.Add("Future FinSett", Type.GetType("System.String"));
            dtExport.Columns.Add("ASN/EXC Amount", Type.GetType("System.String"));
            dtExport.Columns.Add("Net Obligation", Type.GetType("System.String"));
            ////FOR EXPORT END

            for (int j = 0; j < cmbclient.Items.Count; j++)
            {
                DataRow rowname = dtExport.NewRow();
                rowname["Instrument"] = "Client Name :" + cmbclient.Items[j].Text.ToString().Trim();
                rowname["Expiry Date"] = "Test";
                dtExport.Rows.Add(rowname);

                DataView viewshare = new DataView();
                viewshare = ds.Tables[0].DefaultView;
                viewshare.RowFilter = "CLIENTID='" + cmbclient.Items[j].Value + "'";
                DataTable dt1 = new DataTable();
                dt1 = viewshare.ToTable();


                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    DataRow row = dtExport.NewRow();
                    row[0] = dt1.Rows[i]["Symbol"].ToString().Trim();
                    row[1] = dt1.Rows[i]["ExpiryDate"].ToString().Trim();
                    if (dt1.Rows[i]["LOTSRESULT_B"] != DBNull.Value)
                        row[2] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["LOTSRESULT_B"].ToString()));
                    if (dt1.Rows[i]["SettlementPrice"] != DBNull.Value)
                        row[3] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["SettlementPrice"].ToString()));
                    if (dt1.Rows[i]["BFVALUE"] != DBNull.Value)
                        row[4] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["BFVALUE"].ToString()));
                    if (dt1.Rows[i]["DAYBUY"] != DBNull.Value)
                        row[5] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["DAYBUY"].ToString()));
                    if (dt1.Rows[i]["BUYVALUE"] != DBNull.Value)
                        row[6] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["BUYVALUE"].ToString()));
                    if (dt1.Rows[i]["BuyAvg"] != DBNull.Value)
                        row[7] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["BuyAvg"].ToString()));
                    if (dt1.Rows[i]["DAYSELL"] != DBNull.Value)
                        row[8] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["DAYSELL"].ToString()));
                    if (dt1.Rows[i]["SELLVALUE"] != DBNull.Value)
                        row[9] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["SELLVALUE"].ToString()));
                    if (dt1.Rows[i]["SellAvg"] != DBNull.Value)
                        row[10] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["SellAvg"].ToString()));
                    if (dt1.Rows[i]["DAYQTYEXCASN"] != DBNull.Value)
                        row[11] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["DAYQTYEXCASN"].ToString()));
                    if (dt1.Rows[i]["CFQTY_I"] != DBNull.Value)
                        row[12] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["CFQTY_I"].ToString()));
                    if (dt1.Rows[i]["SETTPRICE"] != DBNull.Value)
                        row[13] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["SETTPRICE"].ToString()));
                    if (dt1.Rows[i]["CFVALUE"] != DBNull.Value)
                        row[14] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["CFVALUE"].ToString()));
                    if (dt1.Rows[i]["PRM"] != DBNull.Value)
                        row[15] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["PRM"].ToString()));
                    if (dt1.Rows[i]["MTM"] != DBNull.Value)
                        row[16] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["MTM"].ToString()));
                    if (dt1.Rows[i]["FINSETT"] != DBNull.Value)
                        row[17] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["FINSETT"].ToString()));
                    if (dt1.Rows[i]["ExcAsn"] != DBNull.Value)
                        row[18] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["ExcAsn"].ToString()));
                    if (dt1.Rows[i]["NETOBLIGATION"] != DBNull.Value)
                        row[19] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["NETOBLIGATION"].ToString()));
                    dtExport.Rows.Add(row);
                }
                DataView viewshare1 = new DataView();
                viewshare1 = ds.Tables[1].DefaultView;
                viewshare1.RowFilter = "CLIENTID='" + cmbclient.Items[j].Value + "'";
                DataTable dt2 = new DataTable();
                dt2 = viewshare1.ToTable();



                DataRow row1 = dtExport.NewRow();
                row1["Instrument"] = "Total";
                if (dt2.Rows[0]["BFVALUE_Sum"] != DBNull.Value)
                    row1[2] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt2.Rows[0]["BFVALUE_Sum"].ToString()));
                if (dt2.Rows[0]["BUYVALUE_Sum"] != DBNull.Value)
                    row1[6] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt2.Rows[0]["BUYVALUE_Sum"].ToString()));
                if (dt2.Rows[0]["BuyAvg_Sum"] != DBNull.Value)
                    row1[7] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt2.Rows[0]["BuyAvg_Sum"].ToString()));
                if (dt2.Rows[0]["SELLVALUE_Sum"] != DBNull.Value)
                    row1[9] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt2.Rows[0]["SELLVALUE_Sum"].ToString()));
                if (dt2.Rows[0]["SellAvg_Sum"] != DBNull.Value)
                    row1[10] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt2.Rows[0]["SellAvg_Sum"].ToString()));
                if (dt2.Rows[0]["CFVALUE_Sum"] != DBNull.Value)
                    row1[14] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt2.Rows[0]["CFVALUE_Sum"].ToString()));
                if (dt2.Rows[0]["PRM_Sum"] != DBNull.Value)
                    row1[15] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt2.Rows[0]["PRM_Sum"].ToString()));
                if (dt2.Rows[0]["MTM_Sum"] != DBNull.Value)
                    row1[16] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt2.Rows[0]["MTM_Sum"].ToString()));
                if (dt2.Rows[0]["FINSETT_Sum"] != DBNull.Value)
                    row1[17] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt2.Rows[0]["FINSETT_Sum"].ToString()));
                if (dt2.Rows[0]["ExcAsn_Sum"] != DBNull.Value)
                    row1[18] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt2.Rows[0]["ExcAsn_Sum"].ToString()));
                if (dt2.Rows[0]["NETOBLIGATION_Sum"] != DBNull.Value)
                    row1[19] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt2.Rows[0]["NETOBLIGATION_Sum"].ToString()));

                dtExport.Rows.Add(row1);

                /////charegs 
                DataView viewshare2 = new DataView();
                viewshare2 = ds.Tables[2].DefaultView;
                viewshare2.RowFilter = "CLIENT='" + cmbclient.Items[j].Value + "'";
                DataTable dt3 = new DataTable();
                dt3 = viewshare2.ToTable();

                if (dt3.Rows[0]["BRKG"] != DBNull.Value)
                {
                    DataRow rowbrkg = dtExport.NewRow();
                    rowbrkg["Instrument"] = "STax On Brkg :";
                    rowbrkg[19] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt3.Rows[0]["BRKG"].ToString()));
                    dtExport.Rows.Add(rowbrkg);

                }
                if (ddldate.SelectedItem.Value.ToString().Trim() == "0")
                {
                    if (dt3.Rows[0]["TRANCHARGE"] != DBNull.Value)
                    {
                        DataRow rowtrancharge = dtExport.NewRow();
                        rowtrancharge["Instrument"] = "Transaction Charges :";
                        rowtrancharge[19] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt3.Rows[0]["TRANCHARGE"].ToString()));
                        dtExport.Rows.Add(rowtrancharge);

                    }
                    if (dt3.Rows[0]["SRVTRANCHARGE"] != DBNull.Value)
                    {
                        DataRow rowsttaxtrancharge = dtExport.NewRow();
                        rowsttaxtrancharge["Instrument"] = "STax On Trn.Charges :";
                        rowsttaxtrancharge[19] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt3.Rows[0]["SRVTRANCHARGE"].ToString()));
                        dtExport.Rows.Add(rowsttaxtrancharge);

                    }
                    if (dt3.Rows[0]["STAMPDUTY"] != DBNull.Value)
                    {
                        DataRow rowstamp = dtExport.NewRow();
                        rowstamp["Instrument"] = "Stamp Duty :";
                        rowstamp[19] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt3.Rows[0]["STAMPDUTY"].ToString()));
                        dtExport.Rows.Add(rowstamp);

                    }
                    if (dt3.Rows[0]["SEBI"] != DBNull.Value)
                    {
                        DataRow rowsebi = dtExport.NewRow();
                        rowsebi["Instrument"] = "SEBI Fee :";
                        rowsebi[19] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt3.Rows[0]["SEBI"].ToString()));
                        dtExport.Rows.Add(rowsebi);

                    }
                    if (dt3.Rows[0]["OTHERCHARGENAME"] != DBNull.Value)
                    {
                        DataRow rowothercharge = dtExport.NewRow();
                        rowothercharge["Instrument"] = dt3.Rows[0]["OTHERCHARGENAME"].ToString();
                        rowothercharge[19] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt3.Rows[0]["OTHERCHARGE"].ToString()));
                        dtExport.Rows.Add(rowothercharge);

                    }
                    if (dt3.Rows[0]["OTHERCHARGENAME"] != DBNull.Value)
                    {
                        DataRow rowsrvtaxothercharge = dtExport.NewRow();
                        rowsrvtaxothercharge["Instrument"] = "STax On :" + dt3.Rows[0]["OTHERCHARGENAME"].ToString();
                        rowsrvtaxothercharge[19] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt3.Rows[0]["OTHERCHARGESRV"].ToString()));
                        dtExport.Rows.Add(rowsrvtaxothercharge);

                    }
                }
                if (dt3.Rows[0]["NETTOTAL"] != DBNull.Value)
                {
                    DataRow rownettotal = dtExport.NewRow();
                    rownettotal["Instrument"] = "Net Total :";
                    rownettotal[19] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt3.Rows[0]["NETTOTAL"].ToString()));
                    dtExport.Rows.Add(rownettotal);
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

            DrRowR1[0] = "Net Position Report:" + str;

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

            if (cmbExport.SelectedItem.Value == "E")
            {
                objExcel.ExportToExcelforExcel(dtExport, "Net Position Report", "Total", dtReportHeader, dtReportFooter);
            }
        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlrptview.SelectedItem.Value.ToString().Trim() == "0")
            {
                ViewState["show"] = "EXPORT";
                procedure();
                exportclientwise();
            }
            else
            {
                exportsharewise();
            }
        }
        protected void cmbclient_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlrptview.SelectedItem.Value.ToString().Trim() == "0")
            {
                ViewState["show"] = "SHOW";
                procedure();
                html_clientwise();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Display", "Display(1);", true);
            }
            else
            {
                html_sharewise();
            }
        }
    }
}