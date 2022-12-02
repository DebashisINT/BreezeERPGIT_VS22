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
    public partial class Reports_ExcessMarginRefundStatement : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        PeriodicalReports pr = new PeriodicalReports();
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        DataSet ds = new DataSet();
        DataTable dtgroupcontactid = new DataTable();
        DataTable Distinctclient = new DataTable();
        ExcelFile objExcel = new ExcelFile();

        decimal RELEASE = decimal.Zero;
        decimal Stocksresult = decimal.Zero;

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
                dtSettlementDate.Date = oDBEngine.GetDate();
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

        protected void btnhide_Click(object sender, EventArgs e)
        {
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                if (HiddenField_Group.Value.ToString() == "")
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
        void FnSegmentFetch()
        {
            DataTable DtSeg = new DataTable();
            if (rdSegmentALLCM.Checked)
            {
                DtSeg = oDBEngine.GetDataTable("tbl_master_companyExchange", "exch_internalid", "exch_compid='" + Session["LastCompany"].ToString() + "' and exch_segmentid='CM'");
            }
            if (rdSegmentALLCMFO.Checked)
            {
                DtSeg = oDBEngine.GetDataTable("tbl_master_companyExchange", "exch_internalid", "exch_compid='" + Session["LastCompany"].ToString() + "' and exch_segmentid in ('CM','FO','CDX')");
            }
            if (rdSpecificSegment.Checked)
            {
                if (txtSegment_hidden.Text.ToString().Trim() == "")
                {
                    DtSeg = oDBEngine.GetDataTable("tbl_master_companyExchange", "exch_internalid", "exch_compid='" + Session["LastCompany"].ToString() + "' and exch_segmentid='CM'");
                }
            }
            string Segment = null;
            if (DtSeg.Rows.Count > 0)
            {
                for (int i = 0; i < DtSeg.Rows.Count; i++)
                {
                    if (Segment == null)
                        Segment = DtSeg.Rows[i][0].ToString();
                    else
                        Segment += "," + DtSeg.Rows[i][0].ToString();
                }

            }
            if (Segment != null)
            {
                txtSegment_hidden.Text = Segment.ToString().Trim();
            }
        }
        void procedure()
        {
            FnSegmentFetch();

            string grptype = "";
            string branch = "";
            string grpval = "";

            if (ddlGroup.SelectedItem.Value.ToString() == "0")
            {
                grptype = "BRANCH";
                if (rdbranchAll.Checked)
                {
                    branch = "ALL";
                    grpval = HttpContext.Current.Session["userbranchHierarchy"].ToString();
                }
                else
                {
                    branch = HiddenField_Branch.Value.ToString().Trim();
                    grpval = "NA";
                }
            }
            else
            {
                grptype = ddlgrouptype.SelectedItem.Text.ToString().Trim();
                branch = HttpContext.Current.Session["userbranchHierarchy"].ToString();
                if (rdddlgrouptypeAll.Checked)
                {
                    grpval = "ALL";
                }
                else
                {
                    grpval = HiddenField_Group.Value.ToString().Trim();
                }
            }

            ds = pr.ExcessMarginRefundStatement(dtSettlementDate.Value.ToString(), dtSettlementDate.Value.ToString(), dtSettlementDate.Value.ToString(),
                rdbClientALL.Checked ? "ALL" : HiddenField_Client.Value.ToString().Trim(), txtSegment_hidden.Text.ToString().Trim(), Session["LastCompany"].ToString(),
                HttpContext.Current.Session["LastFinYear"].ToString(), txtWorkingdays.Text.ToString().Trim() == "" ? "1" : txtWorkingdays.Text.ToString().Trim(),
                txtmarkupappmrgn.Value.ToString(), txtmaintaincashcomp.Value.ToString(), chkpartialrelease.Checked ? "CHK" : "UNCHK",
                grptype, branch, grpval, ddlStock.SelectedItem.Value.ToString().Trim(), rbScreen.Checked ? "SHOW" : "PRINT");

            ViewState["dataset"] = ds;

            if (ds.Tables[0].Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(1);", true);
            }
        }
        void ddlbandforgroup()
        {
            ds = (DataSet)ViewState["dataset"];
            DataView viewgroup = new DataView(ds.Tables[0]);
            dtgroupcontactid = viewgroup.ToTable(true, new string[] { "GROUPID", "GRPNAME" });
            if (dtgroupcontactid.Rows.Count > 0)
            {
                cmbgroup.DataSource = dtgroupcontactid;
                cmbgroup.DataValueField = "GROUPID";
                cmbgroup.DataTextField = "GRPNAME";
                cmbgroup.DataBind();

            }
        }
        void ddlbandforClient()
        {
            ds = (DataSet)ViewState["dataset"];
            DataView viewgrp = new DataView();
            viewgrp = ds.Tables[0].DefaultView;
            viewgrp.RowFilter = "GROUPID='" + cmbgroup.SelectedItem.Value + "'";
            DataTable dt = new DataTable();
            dt = viewgrp.ToTable();

            DataView viewClient = new DataView(dt);
            Distinctclient = viewClient.ToTable(true, new string[] { "clientid", "CLIENTNAME" });

            if (Distinctclient.Rows.Count > 0)
            {
                cmbclient.DataSource = Distinctclient;
                cmbclient.DataValueField = "clientid";
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
            ddlbandforClient();
            bind_Details();
        }

        void htmltable()
        {
            ds = (DataSet)ViewState["dataset"];
            String strHtml = String.Empty;
            String strHtml1 = String.Empty;
            string str = null;
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                str = "[ " + ddlgrouptype.SelectedItem.Text.ToString().Trim() + " ]";
            }

            str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + " Report For " + oconverter.ArrangeDate2(dtSettlementDate.Value.ToString());

            strHtml1 = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtml1 += "<tr><td align=\"left\" colspan=14 style=\"color:Blue;\">" + str + "</td></tr></table>";

            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

            DataView viewclient = new DataView();
            viewclient = ds.Tables[0].DefaultView;
            viewclient.RowFilter = "clientid='" + cmbclient.SelectedItem.Value + "'";
            DataTable dt1 = new DataTable();
            dt1 = viewclient.ToTable();

            strHtml += "<tr style=\"background-color:lavender ;text-align:left\">";
            strHtml += "<td align=\"left\" colspan=14>Client Name :&nbsp;<b>" + cmbclient.SelectedItem.Text.ToString().Trim() + "</b>[ <b>" + dt1.Rows[0]["UCC"].ToString() + " </b> ]</td></tr>";

            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            strHtml += "<td align=\"center\" ><b>Ledgr </br> Baln</b></td>";
            strHtml += "<td align=\"center\" ><b>Cash</br> Deposit</b></td>";
            strHtml += "<td align=\"center\" ><b>FDRs/</br> BGs</b></td>";
            strHtml += "<td align=\"center\"><b>Total Cash </br> Deposit</b></td>";
            strHtml += "<td align=\"center\"><b>Pndng  </br> Pur/Sale</b></td>";
            strHtml += "<td align=\"center\"><b>Hldbk </br> Stocks</b></td>";
            strHtml += "<td align=\"center\"><b>Margin  </br> Stocks</b></td>";
            strHtml += "<td align=\"center\"><b>Non-Cash </br> Deposit</b></td>";
            strHtml += "<td align=\"center\"><b>Net </br> Deposit</b></td>";
            strHtml += "<td align=\"center\" ><b>Appl.Mrgn </br>(with Markup)</b></td>";
            strHtml += "<td align=\"center\"><b>Excess</b></td>";
            strHtml += "<td align=\"center\"><b>Refund  </br> (Cash)</b></td>";
            strHtml += "<td align=\"center\" ><b>Refund </br>(Stocks)</b></td>";
            strHtml += "</tr>";

            int flag = 0;
            flag = flag + 1;
            strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";

            if (dt1.Rows[0]["ledgerbaln"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt1.Rows[0]["ledgerbaln"].ToString() + "</td>";
            else
                strHtml += "<td align=\"left\" >&nbsp;</td>";

            if (dt1.Rows[0]["cashmrfndept"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt1.Rows[0]["cashmrfndept"].ToString() + "</td>";
            else
                strHtml += "<td align=\"left\" >&nbsp;</td>";

            if (dt1.Rows[0]["fdrsbgs"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt1.Rows[0]["fdrsbgs"].ToString() + "</td>";
            else
                strHtml += "<td align=\"left\" >&nbsp;</td>";

            if (dt1.Rows[0]["totaldept"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt1.Rows[0]["totaldept"].ToString() + "</td>";
            else
                strHtml += "<td align=\"left\" >&nbsp;</td>";

            if (dt1.Rows[0]["pendpursale"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt1.Rows[0]["pendpursale"].ToString() + "</td>";
            else
                strHtml += "<td align=\"left\" >&nbsp;</td>";

            if (dt1.Rows[0]["hldbkstocks"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt1.Rows[0]["hldbkstocks"].ToString() + "</td>";
            else
                strHtml += "<td align=\"left\" >&nbsp;</td>";

            if (dt1.Rows[0]["mrgnstocks"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt1.Rows[0]["mrgnstocks"].ToString() + "</td>";
            else
                strHtml += "<td align=\"left\" >&nbsp;</td>";

            if (dt1.Rows[0]["noncashdep"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt1.Rows[0]["noncashdep"].ToString() + "</td>";
            else
                strHtml += "<td align=\"left\" >&nbsp;</td>";

            if (dt1.Rows[0]["netdeposit"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt1.Rows[0]["netdeposit"].ToString() + "</td>";
            else
                strHtml += "<td align=\"left\" >&nbsp;</td>";

            if (dt1.Rows[0]["appmrgnwithmarkup"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt1.Rows[0]["appmrgnwithmarkup"].ToString() + "</td>";
            else
                strHtml += "<td align=\"left\" >&nbsp;</td>";

            if (dt1.Rows[0]["excessshortage"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt1.Rows[0]["excessshortage"].ToString() + "</td>";
            else
                strHtml += "<td align=\"left\" >&nbsp;</td>";

            if (dt1.Rows[0]["refundcash"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt1.Rows[0]["refundcash"].ToString() + "</td>";
            else
                strHtml += "<td align=\"left\" >&nbsp;</td>";

            if (dt1.Rows[0]["refundstocks"] != DBNull.Value)
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt1.Rows[0]["refundstocks"].ToString() + "</td>";
            else
                strHtml += "<td align=\"left\" >&nbsp;</td>";

            strHtml += "</tr>";

            dt1 = new DataTable();
            dt1.Clear();
            DataView viewstockclient = new DataView();
            viewstockclient = ds.Tables[0].DefaultView;
            viewstockclient.RowFilter = "CUSTOMER='" + cmbclient.SelectedItem.Value + "'";
            dt1 = viewstockclient.ToTable();

            if (dt1.Rows.Count > 0)
            {
                strHtml += "<tr style=\"background-color:lavender ;text-align:left\">";
                strHtml += "<td align=\"left\" colspan=14><b><u>Stock Details</u></b></td></tr>";

                strHtml += "<tr style=\"background-color: #DBEEF3;\">";
                strHtml += "<td align=\"center\" ><b>Our Dp </br> A/c</b></td>";
                strHtml += "<td align=\"center\" ><b>Pledge</br> A/c</b></td>";
                strHtml += "<td align=\"center\" ><b>Type</b></td>";
                strHtml += "<td align=\"center\" ><b>Scrip</b></td>";
                strHtml += "<td align=\"center\"><b>Qty</b></td>";
                strHtml += "<td align=\"center\"><b>Rate</b></td>";
                strHtml += "<td align=\"center\"><b>Haircut</b></td>";
                strHtml += "<td align=\"center\"><b>Value</b></td>";
                strHtml += "<td align=\"center\"><b>Release</b></td>";
                strHtml += "</tr>";

                RELEASE = decimal.Zero;
                Stocksresult = decimal.Zero;

                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >" + dt1.Rows[i]["DPAC"].ToString() + "</td>";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >" + dt1.Rows[i]["PLEDGEAC"].ToString() + "</td>";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >" + dt1.Rows[i]["STKTYPE"].ToString() + "</td>";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >" + dt1.Rows[i]["SCRIPNAME"].ToString() + "</td>";

                    if (dt1.Rows[i]["Quantity"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt1.Rows[i]["Quantity"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";

                    if (dt1.Rows[i]["closeprice"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt1.Rows[i]["closeprice"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";

                    if (dt1.Rows[i]["varmargin"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt1.Rows[i]["varmargin"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";

                    if (dt1.Rows[i]["Stocksresult"] != DBNull.Value)
                    {
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt1.Rows[i]["Stocksresult"].ToString() + "</td>";
                        Stocksresult = Stocksresult + Convert.ToDecimal(dt1.Rows[i]["Stocksresult1"].ToString());

                    }
                    else
                    {
                        strHtml += "<td align=\"left\" >&nbsp;</td>";
                    }

                    if (dt1.Rows[i]["RELEASE"] != DBNull.Value)
                    {
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt1.Rows[i]["RELEASE"].ToString() + "</td>";
                        RELEASE = RELEASE + Convert.ToDecimal(dt1.Rows[i]["RELEASE1"].ToString());

                    }
                    else
                    {
                        strHtml += "<td align=\"left\" >&nbsp;</td>";
                    }
                    strHtml += "</tr>";
                }
                ///////////////////total display

                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                strHtml += "<td align=\"left\" colspan=7><b>Total :</b></td>";
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(Convert.ToString(Stocksresult))) + "</td>";
                strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(Convert.ToString(RELEASE))) + "</td>";
                strHtml += "</tr>";
            }
            strHtml += "</table>";
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
        protected void cmbgroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlbandforClient();
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
        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            EXPORT();
        }
        void EXPORT()
        {
            ds = (DataSet)ViewState["dataset"];
            DataTable dtExport = new DataTable();
            dtExport.Clear();
            dtExport.Columns.Add("Ledgr Baln", Type.GetType("System.String"));
            dtExport.Columns.Add("Cash Deposit", Type.GetType("System.String"));
            dtExport.Columns.Add("FDRs/BGs", Type.GetType("System.String"));
            dtExport.Columns.Add("Total Cash Deposit", Type.GetType("System.String"));
            dtExport.Columns.Add("Pndng Pur/Sale", Type.GetType("System.String"));
            dtExport.Columns.Add("Hldbk Stocks", Type.GetType("System.String"));
            dtExport.Columns.Add("Mrgn Stocks", Type.GetType("System.String"));
            dtExport.Columns.Add("Non-Cash Deposit", Type.GetType("System.String"));
            dtExport.Columns.Add("Net Deposit", Type.GetType("System.String"));
            dtExport.Columns.Add("Appl.Mrgn(with Markup)", Type.GetType("System.String"));
            dtExport.Columns.Add("Excess", Type.GetType("System.String"));
            dtExport.Columns.Add("Refund (Cash)", Type.GetType("System.String"));
            dtExport.Columns.Add("Refund (Stocks)", Type.GetType("System.String"));




            for (int j = 0; j < cmbgroup.Items.Count; j++)
            {
                DataRow row = dtExport.NewRow();
                row["Ledgr Baln"] = ddlGroup.SelectedItem.Text.ToString().Trim() + " Name:" + cmbgroup.Items[j].Text.ToString().Trim();
                row["Cash Deposit"] = "Test";
                dtExport.Rows.Add(row);


                ds = (DataSet)ViewState["dataset"];
                DataView viewgrp = new DataView();
                viewgrp = ds.Tables[0].DefaultView;
                viewgrp.RowFilter = "GROUPID='" + cmbgroup.Items[j].Value + "'";
                DataTable dt = new DataTable();
                dt = viewgrp.ToTable();

                DataView viewClient = new DataView(dt);
                Distinctclient = new DataTable();
                Distinctclient = viewClient.ToTable(true, new string[] { "clientid", "CLIENTNAME" });

                if (Distinctclient.Rows.Count > 0)
                {
                    cmbclient.Items.Clear();
                    cmbclient.DataSource = Distinctclient;
                    cmbclient.DataValueField = "clientid";
                    cmbclient.DataTextField = "CLIENTNAME";
                    cmbclient.DataBind();

                }
                for (int k = 0; k < cmbclient.Items.Count; k++)
                {
                    DataRow row1 = dtExport.NewRow();
                    row1["Ledgr Baln"] = "Client Name:" + cmbclient.Items[k].Text.ToString().Trim();
                    row1["Cash Deposit"] = "Test";
                    dtExport.Rows.Add(row1);

                    DataView viewclient = new DataView();
                    viewclient = ds.Tables[0].DefaultView;
                    viewclient.RowFilter = "clientid='" + cmbclient.Items[k].Value + "'";
                    DataTable dt1 = new DataTable();
                    dt1 = viewclient.ToTable();


                    DataRow row2 = dtExport.NewRow();

                    if (dt1.Rows[0]["ledgerbaln"] != DBNull.Value)
                        row2["Ledgr Baln"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["ledgerbaln"].ToString()));

                    if (dt1.Rows[0]["cashmrfndept"] != DBNull.Value)
                        row2["Cash Deposit"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["cashmrfndept"].ToString()));

                    if (dt1.Rows[0]["fdrsbgs"] != DBNull.Value)
                        row2["FDRs/BGs"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["fdrsbgs"].ToString()));

                    if (dt1.Rows[0]["totaldept"] != DBNull.Value)
                        row2["Total Cash Deposit"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["totaldept"].ToString()));

                    if (dt1.Rows[0]["pendpursale"] != DBNull.Value)
                        row2["Pndng Pur/Sale"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["pendpursale"].ToString()));

                    if (dt1.Rows[0]["hldbkstocks"] != DBNull.Value)
                        row2["Hldbk Stocks"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["hldbkstocks"].ToString()));

                    if (dt1.Rows[0]["mrgnstocks"] != DBNull.Value)
                        row2["Mrgn Stocks"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["mrgnstocks"].ToString()));

                    if (dt1.Rows[0]["noncashdep"] != DBNull.Value)
                        row2["Non-Cash Deposit"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["noncashdep"].ToString()));

                    if (dt1.Rows[0]["netdeposit"] != DBNull.Value)
                        row2["Net Deposit"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["netdeposit"].ToString()));

                    if (dt1.Rows[0]["appmrgnwithmarkup"] != DBNull.Value)
                        row2["Appl.Mrgn(with Markup)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["appmrgnwithmarkup"].ToString()));

                    if (dt1.Rows[0]["excessshortage"] != DBNull.Value)
                        row2["Excess"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["excessshortage"].ToString()));

                    if (dt1.Rows[0]["refundcash"] != DBNull.Value)
                        row2["Refund (Cash)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["refundcash"].ToString()));

                    if (dt1.Rows[0]["refundstocks"] != DBNull.Value)
                        row2["Refund (Stocks)"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["refundstocks"].ToString()));


                    dtExport.Rows.Add(row2);


                    DataRow row3 = dtExport.NewRow();
                    row3["Ledgr Baln"] = "Stock Details";
                    row3["Cash Deposit"] = "Test";
                    dtExport.Rows.Add(row3);


                    dt1 = new DataTable();
                    dt1.Clear();
                    DataView viewstockclient = new DataView();
                    viewstockclient = ds.Tables[0].DefaultView;
                    viewstockclient.RowFilter = "clientid='" + cmbclient.Items[k].Value + "'";
                    dt1 = viewstockclient.ToTable();

                    DataRow rowblank = dtExport.NewRow();
                    dtExport.Rows.Add(rowblank);

                    DataRow row4 = dtExport.NewRow();
                    row4["Ledgr Baln"] = "Our Dp A/c";
                    row4["Cash Deposit"] = "Pledge A/c";
                    row4["FDRs/BGs"] = "Type";
                    row4["Total Cash Deposit"] = "Scrip";
                    row4["Pndng Pur/Sale"] = "Qty";
                    row4["Hldbk Stocks"] = "Rate";
                    row4["Mrgn Stocks"] = "Haircut";
                    row4["Non-Cash Deposit"] = "Value";
                    row4["Net Deposit"] = "Release";
                    dtExport.Rows.Add(row4);

                    RELEASE = decimal.Zero;
                    Stocksresult = decimal.Zero;

                    for (int p = 0; p < dt1.Rows.Count; p++)
                    {
                        DataRow row5 = dtExport.NewRow();
                        row5["Ledgr Baln"] = dt1.Rows[p]["DPAC"].ToString();
                        row5["Cash Deposit"] = dt1.Rows[p]["PLEDGEAC"].ToString();
                        row5["FDRs/BGs"] = dt1.Rows[p]["STKTYPE"].ToString();
                        row5["Total Cash Deposit"] = dt1.Rows[p]["SCRIPNAME"].ToString();

                        if (dt1.Rows[p]["Quantity"] != DBNull.Value)
                            row5["Pndng Pur/Sale"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[p]["Quantity"].ToString()));

                        if (dt1.Rows[p]["closeprice"] != DBNull.Value)
                            row5["Hldbk Stocks"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[p]["closeprice"].ToString()));

                        if (dt1.Rows[p]["varmargin"] != DBNull.Value)
                            row5["Mrgn Stocks"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[p]["varmargin"].ToString()));

                        if (dt1.Rows[p]["Stocksresult"] != DBNull.Value)
                        {
                            row5["Non-Cash Deposit"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[p]["Stocksresult"].ToString()));
                            Stocksresult = Stocksresult + Convert.ToDecimal(dt1.Rows[p]["Stocksresult1"].ToString());
                        }
                        if (dt1.Rows[p]["RELEASE"] != DBNull.Value)
                        {
                            row5["Net Deposit"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[p]["RELEASE"].ToString()));
                            RELEASE = RELEASE + Convert.ToDecimal(dt1.Rows[p]["RELEASE1"].ToString());
                        }

                        dtExport.Rows.Add(row5);

                    }

                    DataRow row6 = dtExport.NewRow();
                    row6["Ledgr Baln"] = "Total :";
                    row6["Non-Cash Deposit"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(Convert.ToString(Stocksresult)));
                    row6["Net Deposit"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(Convert.ToString(RELEASE)));
                    dtExport.Rows.Add(row6);
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

            str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report For " + oconverter.ArrangeDate2(dtSettlementDate.Value.ToString());
            DrRowR1[0] = "Excess Margin Refund Statement:" + str;

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

            if (cmbExport.SelectedItem.Value == "E")
            {
                objExcel.ExportToExcelforExcel(dtExport, "Excess Margin Refund Statement", "Total :", dtReportHeader, dtReportFooter);
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
            //ds.Tables[0].WriteXmlSchema("E:\\RPTXSD\\excessmrgnrefundstatement.xsd");

            //report.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;

            string tmpPdfPath = string.Empty;
            tmpPdfPath = HttpContext.Current.Server.MapPath("..\\management\\ExcessMarginRefundStatement.rpt");
            report.Load(tmpPdfPath);
            report.SetDataSource(ds.Tables[0]);

            if (ChkDISTRIBUTION.Checked)
            {
                report.SetParameterValue("@PAR1", (object)"CHK");
            }
            else
            {
                report.SetParameterValue("@PAR1", (object)"UNCHK");
            }
            report.SetParameterValue("@fordate", (object)oconverter.ArrangeDate2(dtSettlementDate.Value.ToString()));
            report.SetParameterValue("@ledgerdate", (object)oconverter.ArrangeDate2(dtSettlementDate.Value.ToString()));
            report.SetParameterValue("@cashdepdate", (object)oconverter.ArrangeDate2(dtSettlementDate.Value.ToString()));
            report.SetParameterValue("@markupapplmrgn", (object)txtmarkupappmrgn.Value);
            report.SetParameterValue("@maintaincash", (object)txtmaintaincashcomp.Value);
            report.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "Excess Margin Refund Statement");
            report.Dispose();
            GC.Collect();
        }
    }
}