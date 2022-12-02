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

    public partial class Reports_monthlyperformance : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        BusinessLogicLayer.Reports oReports = new BusinessLogicLayer.Reports();
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        DataSet ds = new DataSet();
        DataTable dtgroupcontactid = new DataTable();
        DataTable Distinctclient = new DataTable();
        ExcelFile objExcel = new ExcelFile();

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
            dtexpiryeffectuntil = oDBEngine.GetDataTable("master_equity", " distinct top 2 equity_effectuntil,equity_effectuntil+1", "  equity_exchsegmentid=2 and equity_effectuntil<getdate()", " equity_effectuntil desc");
            if (dtexpiryeffectuntil.Rows.Count == 2)
            {
                dtto.Value = Convert.ToDateTime(dtexpiryeffectuntil.Rows[0][0].ToString());
                dtfrom.Value = Convert.ToDateTime(dtexpiryeffectuntil.Rows[1][1].ToString());
            }
            else
            {
                dtto.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
                dtfrom.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());

            }
            dtfor.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());

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
            procedure();
            ds = (DataSet)ViewState["dataset"];
            if (ds.Tables[0].Rows.Count > 0)
            {
                CurrentPage = 0;
                ddlbandforgroup();
                ddlbandforClient();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD();", true);
            }

        }
        void procedure()
        {
            string fromdate = string.Empty;
            string todate = string.Empty;
            string clients = string.Empty;
            string Seriesid = string.Empty;
            string Expiry = string.Empty;
            string GrpType = string.Empty;
            string GrpId = string.Empty;
            string ckoptmtm = string.Empty;

            if (ddldate.SelectedItem.Value.ToString() == "0")
            {
                fromdate = "1900-01-01";
                todate = Convert.ToString(dtfor.Value);
            }
            else
            {
                fromdate = Convert.ToString(dtfrom.Value);
                todate = Convert.ToString(dtto.Value);
            }
            if (rdbClientALL.Checked)
            {
                clients = "ALL";
            }
            else
            {
                clients = Convert.ToString(HiddenField_Client.Value);
            }
            if (rdbunderlyingall.Checked)
            {
                Seriesid = "ALL";
            }
            else
            {
                Seriesid = Convert.ToString(txtunderlying_hidden.Text);
            }
            if (rdbExpiryAll.Checked)
            {
                Expiry = "ALL";
            }
            else
            {
                Expiry = Convert.ToString(txtExpiry_hidden.Text);
            }
            if (ddlGroup.SelectedItem.Value.ToString() == "0")
            {
                GrpType = "BRANCH";
                if (rdbranchAll.Checked)
                {
                    GrpId = "ALL";
                }
                else
                {
                    GrpId = Convert.ToString(HiddenField_Branch.Value);
                }
            }
            else
            {
                GrpType = Convert.ToString(ddlgrouptype.SelectedItem.Text);
                if (rdbranchAll.Checked)
                {
                    GrpId = "ALL";
                }
                else
                {
                    GrpId = Convert.ToString(HiddenField_Group.Value);
                }
            }
            if (chkoptmtm.Checked == true)
            {
                ckoptmtm = "CHK";
            }
            else
            {
                ckoptmtm = "UNCHK";
            }
            ds = oReports.MonthlyPerformanceReportFO(
                Convert.ToString(Session["LastCompany"]),
                  Convert.ToString(Session["usersegid"]),
                  Convert.ToString(fromdate),
                  Convert.ToString(todate),
                  Convert.ToString(clients),
                  Convert.ToString(HttpContext.Current.Session["ExchangeSegmentID"]),
                  Seriesid,
                  Expiry,
                  GrpType,
                  GrpId,
                  Convert.ToString(Session["userbranchHierarchy"]),
                  Convert.ToString(Session["LastFinYear"]),
                 ckoptmtm
                );
            ViewState["dataset"] = ds;

        }
        void ddlbandforgroup()
        {
            ds = (DataSet)ViewState["dataset"];
            DataView viewgroup = new DataView(ds.Tables[0]);
            dtgroupcontactid = viewgroup.ToTable(true, new string[] { "GRPID", "GRPNAME" });
            if (dtgroupcontactid.Rows.Count > 0)
            {
                cmbsubbroker.DataSource = dtgroupcontactid;
                cmbsubbroker.DataValueField = "GRPID";
                cmbsubbroker.DataTextField = "GRPNAME";
                cmbsubbroker.DataBind();

            }

        }
        void ddlbandforClient()
        {
            ds = (DataSet)ViewState["dataset"];
            DataView viewbroker = new DataView();
            viewbroker = ds.Tables[0].DefaultView;
            viewbroker.RowFilter = "GRPID='" + cmbsubbroker.SelectedItem.Value + "'";
            DataTable dt = new DataTable();
            dt = viewbroker.ToTable();

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
        void bind_Details()
        {
            Distinctclient = (DataTable)ViewState["clients"];
            cmbclient.SelectedIndex = CurrentPage;
            if (LastPage > -1)
            {
                listRecord.Text = CurrentPage + 1 + " of " + Distinctclient.Rows.Count.ToString() + " Record.";

            }
            htmltable();
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
        void htmltable()
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
            strHtml += "<td align=\"left\" colspan=" + colcount + ">Client Name :&nbsp;<b>" + cmbclient.SelectedItem.Text.ToString().Trim() + "</b></td></tr>";

            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            strHtml += "<td align=\"center\" ><b>Instr. Type</b></td>";
            strHtml += "<td align=\"center\" ><b>Expiry</b></td>";
            strHtml += "<td align=\"center\"><b>Strike</b></td>";
            if (ddldate.SelectedItem.Value.ToString() == "1")
            {
                strHtml += "<td align=\"center\"><b>B/F Qty</b></td>";
                strHtml += "<td align=\"center\"><b>B/F Price</b></td>";
                strHtml += "<td align=\"center\"><b>B/F Value</b></td>";
            }
            strHtml += "<td align=\"center\"><b>Buy Qty</b></td>";
            strHtml += "<td align=\"center\"><b>Buy  Value</b></td>";
            strHtml += "<td align=\"center\"><b>Sell Qty</b></td>";
            strHtml += "<td align=\"center\"><b>Sell  Value</b></td>";
            strHtml += "<td align=\"center\"><b>Exer. Qty</b></td>";
            strHtml += "<td align=\"center\"><b>Exer. Value</b></td>";
            strHtml += "<td align=\"center\"><b>Asgn. Qty</b></td>";
            strHtml += "<td align=\"center\"><b>Asgn. Value</b></td>";
            strHtml += "<td align=\"center\"><b>FS  Qty</b></td>";
            strHtml += "<td align=\"center\"><b>FS  Value</b></td>";
            strHtml += "<td align=\"center\"><b>C/F  Qty</b></td>";
            strHtml += "<td align=\"center\"><b>C/F  Price</b></td>";
            strHtml += "<td align=\"center\"><b>C/F  Value</b></td>";
            strHtml += "<td align=\"center\"><b>Profit / Loss(-)</b></td>";
            strHtml += "</tr>";
            int flag = 0;

            string symbol = null;
            string SERIES = null;
            string MASTERPRODUCTID = null;
            for (int i = 0; i < dt1.Rows.Count; i++)
            {


                if (symbol != null)
                {
                    if (symbol == dt1.Rows[i]["TICKERSYMBOL"].ToString())
                    {
                        if (SERIES != null)
                        {
                            if (SERIES != dt1.Rows[i]["INSTRUTYPE"].ToString())
                            {
                                flag = flag + 1;
                                strHtml += "<tr style=\"background-color:white ;text-align:left\" id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id)>";
                                strHtml += "<td align=\"right\" colspan=" + colcount + ">Instrument Total:&nbsp;<b>" + dt1.Rows[i - 1]["ISTRUTOTAL"].ToString() + "</b></td>";
                                strHtml += "</tr>";
                            }
                        }
                    }
                    else
                    {
                        flag = flag + 1;
                        strHtml += "<tr style=\"background-color:white ;text-align:left\" id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id)>";
                        strHtml += "<td align=\"right\" colspan=" + colcount + ">Instrument Total:&nbsp;<b>" + dt1.Rows[i - 1]["ISTRUTOTAL"].ToString() + "</b></td>";
                        strHtml += "</tr>";
                    }

                }
                if (MASTERPRODUCTID != dt1.Rows[i]["MASTERPRODUCTID"].ToString())
                {
                    if (MASTERPRODUCTID != null)
                    {
                        flag = flag + 1;
                        strHtml += "<tr style=\"background-color:white ;text-align:left\" id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id)>";
                        strHtml += "<td align=\"right\" colspan=" + colcount + ">Asset Total :&nbsp;<b>" + dt1.Rows[i - 1]["AssetPRLOSS"].ToString() + "</b></td>";
                        strHtml += "</tr>";
                    }
                }
                if (symbol != dt1.Rows[i]["TICKERSYMBOL"].ToString())
                {
                    flag = flag + 1;
                    strHtml += "<tr style=\"background-color:white ;text-align:left\" id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id)>";
                    strHtml += "<td align=\"left\" colspan=" + colcount + " style=\"color:green;\">Asset :&nbsp;<b>" + dt1.Rows[i]["TICKERSYMBOL"].ToString() + "</b></td>";
                    strHtml += "</tr>";
                    symbol = dt1.Rows[i]["TICKERSYMBOL"].ToString();
                }

                MASTERPRODUCTID = dt1.Rows[i]["MASTERPRODUCTID"].ToString();
                SERIES = dt1.Rows[i]["INSTRUTYPE"].ToString();

                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                if (dt1.Rows[i]["INSTRUTYPE"] != DBNull.Value)
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["INSTRUTYPE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (dt1.Rows[i]["EXPIRY"] != DBNull.Value)
                    strHtml += "<td align=\"left\" style=\"nowrap=nowrap;width:8%;font-size:xx-small;\">" + dt1.Rows[i]["EXPIRY"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (dt1.Rows[i]["STRIKEPRICE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["STRIKEPRICE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";
                if (ddldate.SelectedItem.Value.ToString() == "1")
                {
                    if (dt1.Rows[i]["BFQTY"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["BFQTY"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";
                    if (dt1.Rows[i]["BFPRICE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["BFPRICE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";
                    if (dt1.Rows[i]["BFVALUE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["BFVALUE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";
                }
                if (dt1.Rows[i]["BUYQTY"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["BUYQTY"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";


                if (dt1.Rows[i]["BUYVAL"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["BUYVAL"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (dt1.Rows[i]["SELLQTY"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["SELLQTY"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (dt1.Rows[i]["SELLVAL"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["SELLVAL"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (dt1.Rows[i]["EXERQTY"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["EXERQTY"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (dt1.Rows[i]["EXERVALUE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["EXERVALUE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (dt1.Rows[i]["ASSGNQTY"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["ASSGNQTY"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (dt1.Rows[i]["ASSGNVALUE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["ASSGNVALUE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (dt1.Rows[i]["FSQTY"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["FSQTY"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (dt1.Rows[i]["FSVALUE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["FSVALUE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (dt1.Rows[i]["CFQTY"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["CFQTY"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (dt1.Rows[i]["CFPRICE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["CFPRICE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (dt1.Rows[i]["CFVALUE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["CFVALUE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                if (dt1.Rows[i]["PRLOSS"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + dt1.Rows[i]["PRLOSS"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";

                strHtml += "</tr>";
            }

            flag = flag + 1;
            strHtml += "<tr style=\"background-color:white ;text-align:left\" id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id)>";
            strHtml += "<td align=\"right\" colspan=" + colcount + ">Instrument Total:&nbsp;<b>" + dt1.Rows[dt1.Rows.Count - 1]["ISTRUTOTAL"].ToString() + "</b></td>";
            strHtml += "</tr>";

            flag = flag + 1;
            strHtml += "<tr style=\"background-color:white ;text-align:left\" id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id)>";
            strHtml += "<td align=\"right\" colspan=" + colcount + ">Asset Total :&nbsp;<b>" + dt1.Rows[dt1.Rows.Count - 1]["AssetPRLOSS"].ToString() + "</b></td>";
            strHtml += "</tr>";


            flag = flag + 1;
            strHtml += "<tr style=\"background-color:white ;text-align:left\" id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id)>";
            strHtml += "<td align=\"right\" colspan=" + colcount + " >Total P/L for All Assets :&nbsp;<b>" + dt1.Rows[0]["TOTALPL"].ToString() + "</b></td>";
            strHtml += "</tr>";

            flag = flag + 1;
            strHtml += "<tr><td align=\"right\" colspan=" + colcount + "><table><tr><td>";
            strHtml += "<div style=\"border: solid 1px balck;\">";
            strHtml += "<table width=\"200px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"1\">";
            strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id)><td align=\"left\"><b><u>Charges :</u></b>&nbsp;&nbsp;</td></tr>";
            flag = flag + 1;
            if (dt1.Rows[0]["STT_CHARGE"] != DBNull.Value)
            {
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id)><td align=\"left\" >Securities Transaction Tax :</td><td align=\"right\">" + dt1.Rows[0]["STT_CHARGE"].ToString() + "</td></tr>";
            }
            else
            {
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id)><td align=\"left\">Securities Transaction Tax :</td><td align=\"right\">0.0</td></tr>";
            }
            //if (ddldate.SelectedItem.Value.ToString() == "1")
            //{
            flag = flag + 1;
            if (dt1.Rows[0]["SRVTAXBRKG_CHARGE"] != DBNull.Value)
            {
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id)><td align=\"left\" >Serv Tax & Cess on Brokerage :</td><td align=\"right\">" + dt1.Rows[0]["SRVTAXBRKG_CHARGE"].ToString() + "</td></tr>";
            }
            else
            {
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id)><td align=\"left\">Serv Tax & Cess on Brokerage :</td><td align=\"right\">0.0</td></tr>";
            }
            flag = flag + 1;
            if (dt1.Rows[0]["TRAN_CHARGE"] != DBNull.Value)
            {
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id)><td align=\"left\" >TOT Charge :</td><td align=\"right\">" + dt1.Rows[0]["TRAN_CHARGE"].ToString() + "</td></tr>";
            }
            else
            {
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id)><td align=\"left\">TOT Charge :</td><td align=\"right\">0.0</td></tr>";
            }
            flag = flag + 1;
            if (dt1.Rows[0]["SRVTAXTRAN_CHARGE"] != DBNull.Value)
            {
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id)><td align=\"left\" >Service Tax & Cess On TOT :</td><td align=\"right\">" + dt1.Rows[0]["SRVTAXTRAN_CHARGE"].ToString() + "</td></tr>";
            }
            else
            {
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id)><td align=\"left\">Service Tax & Cess On TOT :</td><td align=\"right\">0.0</td></tr>";
            }
            flag = flag + 1;
            if (dt1.Rows[0]["STAMP_CHARGE"] != DBNull.Value)
            {
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id)><td align=\"left\" >Stamp Duty  :</td><td align=\"right\">" + dt1.Rows[0]["STAMP_CHARGE"].ToString() + "</td></tr>";
            }
            else
            {
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id)><td align=\"left\">Stamp Duty :</td><td align=\"right\">0.0</td></tr>";
            }

            //}
            flag = flag + 1;
            strHtml += "</table></div></td></tr></table></td></tr>";
            strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id)><td align=\"right\" colspan=" + colcount + " style=\"color:maroon;\">Net P/L for All Assets:<b>" + dt1.Rows[0]["NETPL"].ToString() + "</b></td>";
            strHtml += "<tr></table>";

            DIVdisplayPERIOD.InnerHtml = strHtml1;
            display.InnerHtml = strHtml;

        }
        protected string GetRowColor(int i)
        {
            if (i++ % 2 == 0)
                return "#fff0f5";
            else
                return "White";
        }
        protected void cmbsubbroker_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlbandforClient();
        }
        protected void NavigationLinkC_Click(Object sender, CommandEventArgs e)
        {
            hiddencount.Value = "0";
            int curentIndex = cmbsubbroker.SelectedIndex;
            int totalNo = cmbsubbroker.Items.Count;
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
                    pageindex = int.Parse(Totalbroker.Value);
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
            cmbsubbroker.SelectedIndex = curentIndex;
            ddlbandforClient();
            bind_Details();
        }
        protected void btnprint_Click(object sender, EventArgs e)
        {
            procedure();
            ds = (DataSet)ViewState["dataset"];
            if (ds.Tables[0].Rows.Count > 0)
            {
                ReportDocument report = new ReportDocument();
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
                    //ds.Tables[0].WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\performancefo.xsd");

                    report.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;

                    string tmpPdfPath = string.Empty;
                    tmpPdfPath = HttpContext.Current.Server.MapPath("..\\Reports\\performancefo.rpt");
                    report.Load(tmpPdfPath);
                    report.SetDataSource(ds.Tables[0]);
                    report.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "Monthly Performance Report");

                    report.Dispose();
                    GC.Collect();
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD();", true);
            }

        }

        void export()
        {
            ds = (DataSet)ViewState["dataset"];
            DataTable dtExport = new DataTable();
            dtExport.Clear();
            dtExport.Columns.Add("Instr. Type", Type.GetType("System.String"));
            dtExport.Columns.Add("Expiry", Type.GetType("System.String"));
            dtExport.Columns.Add("Strike", Type.GetType("System.String"));
            if (ddldate.SelectedItem.Value.ToString() == "1")
            {
                dtExport.Columns.Add("B/F Qty", Type.GetType("System.String"));
                dtExport.Columns.Add("B/F Price", Type.GetType("System.String"));
                dtExport.Columns.Add("B/F Value", Type.GetType("System.String"));
            }
            dtExport.Columns.Add("Buy Qty", Type.GetType("System.String"));
            dtExport.Columns.Add("Buy Value", Type.GetType("System.String"));
            dtExport.Columns.Add("Sell Qty", Type.GetType("System.String"));
            dtExport.Columns.Add("Sell Value", Type.GetType("System.String"));
            dtExport.Columns.Add("Exer. Qty", Type.GetType("System.String"));
            dtExport.Columns.Add("Exer. Value", Type.GetType("System.String"));
            dtExport.Columns.Add("Asgn. Qty", Type.GetType("System.String"));
            dtExport.Columns.Add("Asgn. Value", Type.GetType("System.String"));
            dtExport.Columns.Add("FS Qty", Type.GetType("System.String"));
            dtExport.Columns.Add("FS Value", Type.GetType("System.String"));
            dtExport.Columns.Add("C/F Qty", Type.GetType("System.String"));
            dtExport.Columns.Add("C/F Price", Type.GetType("System.String"));
            dtExport.Columns.Add("C/F Value", Type.GetType("System.String"));
            dtExport.Columns.Add("Profit/Loss(-)", Type.GetType("System.String"));

            for (int i = 0; i < cmbsubbroker.Items.Count; i++)
            {
                DataRow row = dtExport.NewRow();
                row["Instr. Type"] = ddlGroup.SelectedItem.Text.ToString().Trim() + " Name:" + cmbsubbroker.Items[i].Text.ToString().Trim();
                row["Expiry"] = "Test";
                dtExport.Rows.Add(row);

                ds = (DataSet)ViewState["dataset"];
                DataView viewbroker = new DataView();
                viewbroker = ds.Tables[0].DefaultView;
                viewbroker.RowFilter = "GRPID='" + cmbsubbroker.Items[i].Value + "'";
                DataTable dt = new DataTable();
                dt = viewbroker.ToTable();

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

                for (int j = 0; j < cmbclient.Items.Count; j++)
                {
                    DataRow row1 = dtExport.NewRow();
                    row1["Instr. Type"] = "Client Name:" + cmbclient.Items[j].Text.ToString().Trim();
                    row1["Expiry"] = "Test";
                    dtExport.Rows.Add(row1);

                    DataView viewclient = new DataView();
                    viewclient = ds.Tables[0].DefaultView;
                    viewclient.RowFilter = "CUSTOMERID='" + cmbclient.Items[j].Value + "'";
                    DataTable dt1 = new DataTable();
                    dt1 = viewclient.ToTable();

                    string symbol = null;
                    string SERIES = null;
                    string MASTERPRODUCTID = null;
                    for (int k = 0; k < dt1.Rows.Count; k++)
                    {
                        if (symbol != null)
                        {
                            if (symbol == dt1.Rows[k]["TICKERSYMBOL"].ToString())
                            {
                                if (SERIES != null)
                                {
                                    if (SERIES != dt1.Rows[k]["INSTRUTYPE"].ToString())
                                    {
                                        DataRow row2 = dtExport.NewRow();
                                        row2["Instr. Type"] = "Instrument Total:";
                                        row2["Profit/Loss(-)"] = dt1.Rows[k - 1]["ISTRUTOTAL"].ToString();
                                        dtExport.Rows.Add(row2);

                                    }
                                }
                            }
                            else
                            {
                                DataRow row3 = dtExport.NewRow();
                                row3["Instr. Type"] = "Instrument Total:";
                                row3["Profit/Loss(-)"] = dt1.Rows[k - 1]["ISTRUTOTAL"].ToString();
                                dtExport.Rows.Add(row3);

                            }

                        }
                        if (MASTERPRODUCTID != dt1.Rows[k]["MASTERPRODUCTID"].ToString())
                        {
                            if (MASTERPRODUCTID != null)
                            {
                                DataRow row4 = dtExport.NewRow();
                                row4["Instr. Type"] = "Asset Total:";
                                row4["Expiry"] = dt1.Rows[k - 1]["AssetPRLOSS"].ToString();
                                dtExport.Rows.Add(row4);

                            }
                        }
                        if (symbol != dt1.Rows[k]["TICKERSYMBOL"].ToString())
                        {
                            DataRow row5 = dtExport.NewRow();
                            row5["Instr. Type"] = "Asset :";
                            row5["Expiry"] = dt1.Rows[k]["TICKERSYMBOL"].ToString();
                            dtExport.Rows.Add(row5);
                            symbol = dt1.Rows[k]["TICKERSYMBOL"].ToString();
                        }

                        MASTERPRODUCTID = dt1.Rows[k]["MASTERPRODUCTID"].ToString();
                        SERIES = dt1.Rows[k]["INSTRUTYPE"].ToString();


                        DataRow row6 = dtExport.NewRow();
                        row6["Instr. Type"] = dt1.Rows[k]["INSTRUTYPE"].ToString();
                        row6["Expiry"] = dt1.Rows[k]["EXPIRY"].ToString();
                        row6["Strike"] = dt1.Rows[k]["STRIKEPRICE"].ToString();
                        if (ddldate.SelectedItem.Value.ToString() == "1")
                        {
                            row6["B/F Qty"] = dt1.Rows[k]["BFQTY"].ToString();
                            row6["B/F Price"] = dt1.Rows[k]["BFPRICE"].ToString();
                            row6["B/F Value"] = dt1.Rows[k]["BFVALUE"].ToString();
                        }
                        row6["Buy Qty"] = dt1.Rows[k]["BUYQTY"].ToString();
                        row6["Buy Value"] = dt1.Rows[k]["BUYVAL"].ToString();
                        row6["Sell Qty"] = dt1.Rows[k]["SELLQTY"].ToString();
                        row6["Sell Value"] = dt1.Rows[k]["SELLVAL"].ToString();
                        row6["Exer. Qty"] = dt1.Rows[k]["EXERQTY"].ToString();
                        row6["Exer. Value"] = dt1.Rows[k]["EXERVALUE"].ToString();
                        row6["Asgn. Qty"] = dt1.Rows[k]["ASSGNQTY"].ToString();
                        row6["Asgn. Value"] = dt1.Rows[k]["ASSGNVALUE"].ToString();
                        row6["FS Qty"] = dt1.Rows[k]["FSQTY"].ToString();
                        row6["FS Value"] = dt1.Rows[k]["FSVALUE"].ToString();
                        row6["C/F Qty"] = dt1.Rows[k]["CFQTY"].ToString();
                        row6["C/F Price"] = dt1.Rows[k]["CFPRICE"].ToString();
                        row6["C/F Value"] = dt1.Rows[k]["CFVALUE"].ToString();
                        row6["Profit/Loss(-)"] = dt1.Rows[k]["PRLOSS"].ToString();

                        dtExport.Rows.Add(row6);
                    }
                    DataRow row7 = dtExport.NewRow();
                    row7["Instr. Type"] = "Instrument Total:";
                    row7["Profit/Loss(-)"] = dt1.Rows[dt1.Rows.Count - 1]["ISTRUTOTAL"].ToString();
                    dtExport.Rows.Add(row7);

                    DataRow row8 = dtExport.NewRow();
                    row8["Instr. Type"] = "Asset Total:";
                    row8["Profit/Loss(-)"] = dt1.Rows[dt1.Rows.Count - 1]["AssetPRLOSS"].ToString();
                    dtExport.Rows.Add(row8);


                    DataRow row9 = dtExport.NewRow();
                    row9["Instr. Type"] = "Total P/L for All Assets:";
                    row9["Profit/Loss(-)"] = dt1.Rows[0]["TOTALPL"].ToString();
                    dtExport.Rows.Add(row9);

                    DataRow row10 = dtExport.NewRow();
                    row10["Instr. Type"] = "Charges";
                    dtExport.Rows.Add(row10);

                    DataRow row11 = dtExport.NewRow();
                    row11["Instr. Type"] = "Securities Transaction Tax :";
                    row11["Profit/Loss(-)"] = dt1.Rows[0]["STT_CHARGE"].ToString();
                    dtExport.Rows.Add(row11);

                    //if (ddldate.SelectedItem.Value.ToString() == "1")
                    //{
                    DataRow row12 = dtExport.NewRow();
                    row12["Instr. Type"] = "Serv Tax & Cess on Brokerage :";
                    row12["Profit/Loss(-)"] = dt1.Rows[0]["SRVTAXBRKG_CHARGE"].ToString();
                    dtExport.Rows.Add(row12);

                    DataRow row13 = dtExport.NewRow();
                    row13["Instr. Type"] = "TOT Charge :";
                    row13["Profit/Loss(-)"] = dt1.Rows[0]["TRAN_CHARGE"].ToString();
                    dtExport.Rows.Add(row13);

                    DataRow row14 = dtExport.NewRow();
                    row14["Instr. Type"] = "Service Tax & Cess On TOT :";
                    row14["Profit/Loss(-)"] = dt1.Rows[0]["SRVTAXTRAN_CHARGE"].ToString();
                    dtExport.Rows.Add(row14);

                    DataRow row15 = dtExport.NewRow();
                    row15["Instr. Type"] = "Stamp Duty  :";
                    row15["Profit/Loss(-)"] = dt1.Rows[0]["STAMP_CHARGE"].ToString();
                    dtExport.Rows.Add(row15);
                    //}

                    DataRow row16 = dtExport.NewRow();
                    row16["Instr. Type"] = "Net P/L for All Assets:";
                    row16["Profit/Loss(-)"] = dt1.Rows[0]["NETPL"].ToString();
                    dtExport.Rows.Add(row16);

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

            DrRowR1[0] = "Monthly Performance Report:" + str;

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
                objExcel.ExportToExcelforExcel(dtExport, "Monthly Performance Report", "Charges", dtReportHeader, dtReportFooter);
            }
        }


        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            export();
        }
    }
}