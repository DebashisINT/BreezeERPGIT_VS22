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
    public partial class Reports_NetPosition_MTMFO : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        DailyReports dailyrep = new DailyReports();
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        DataSet ds = new DataSet();
        DataTable dtgroupcontactid = new DataTable();
        DataTable Distinctclient = new DataTable();
        ExcelFile objExcel = new ExcelFile();
        DataTable dtExport = new DataTable();
        GenericMethod oGenericMethod = null;
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
                date();
                chkboxliststyle();
                fn_cmbunderlying();
                fn_cmbexpiry();

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
            dtfor.EditFormatString = oconverter.GetDateFormat("Date");
            dtfrom.EditFormatString = oconverter.GetDateFormat("Date");
            dtto.EditFormatString = oconverter.GetDateFormat("Date");

            dtfor.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
            dtfrom.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
            dtto.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());

        }
        void chkboxliststyle()
        {
            foreach (ListItem item in chktfilter.Items)
            {
                item.Attributes.Add("style", "font-family:Times New Roman;color:#461B7E;font-size:9px");
            }
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
                    if (idlist[0] != "Clients" && idlist[0] != "Broker")
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


                if (idlist[0] == "Product")
                {
                    data = "Product~" + str;
                    seleectedunderlying();
                }
                else if (idlist[0] == "Clients")
                {
                    data = "Clients~" + str;
                }
                else if (idlist[0] == "Broker")
                {
                    data = "Broker~" + str;
                }
                else if (idlist[0] == "Group")
                {
                    data = "Group~" + str;
                }
                else if (idlist[0] == "Branch")
                {
                    data = "Branch~" + str;
                }
                else if (idlist[0] == "BranchGroup")
                {
                    data = "BranchGroup~" + str;
                }
                else if (idlist[0] == "MAILEMPLOYEE")
                {
                    data = "MAILEMPLOYEE~" + str;
                }
            }
        }
        void seleectedunderlying()
        {
            DataTable dtunderlying = oDBEngine.GetDataTable("master_equity", " distinct Equity_ProductID", "equity_exchsegmentid='" + HttpContext.Current.Session["ExchangeSegmentID"].ToString() + "' and Equity_ProductID in (select products_id from master_products where products_derivedfromid in (" + HiddenField_Product.Value + ")) ");
            if (dtunderlying.Rows.Count > 0)
            {
                HiddenField_Product.Value = "";
                for (int i = 0; i < dtunderlying.Rows.Count; i++)
                {
                    if (HiddenField_Product.Value == "")
                        HiddenField_Product.Value = dtunderlying.Rows[i][0].ToString();
                    else
                        HiddenField_Product.Value += "," + dtunderlying.Rows[i][0].ToString();
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
        void fn_cmbexpiry()
        {
            if (rdbunderlyingAll.Checked)
            {
                fn_cmbunderlying();
            }
            cmbexpiry.Items.Clear();
            string effectdate = null;
            if (ddldate.SelectedItem.Value.ToString() == "0")
            {
                effectdate = dtfor.Value.ToString().Trim();
            }
            else
            {
                effectdate = dtto.Value.ToString().Trim();
            }

            DataTable dtExpiry = new DataTable();

            if (HiddenField_Product.Value != "")
            {
                dtExpiry = oDBEngine.GetDataTable("master_equity", "distinct convert(varchar(9),Equity_EffectUntil,6) as expirydate,Equity_EffectUntil", "equity_exchsegmentid='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and Equity_ProductID in(" + HiddenField_Product.Value + ") and Equity_EffectUntil>='" + effectdate.ToString().Trim() + "'", " Equity_EffectUntil");
            }

            else
            {
                dtExpiry = oDBEngine.GetDataTable("master_equity", "distinct convert(varchar(9),Equity_EffectUntil,6) as expirydate,Equity_EffectUntil", "equity_exchsegmentid='" + HttpContext.Current.Session["ExchangeSegmentID"] + "'  and Equity_EffectUntil>='" + effectdate.ToString().Trim() + "'", " Equity_EffectUntil");
            }


            if (dtExpiry.Rows.Count > 0)
            {
                cmbexpiry.DataSource = dtExpiry;
                cmbexpiry.DataValueField = "Equity_EffectUntil";
                cmbexpiry.DataTextField = "expirydate";
                cmbexpiry.DataBind();
                cmbexpiry.Items.Insert(0, "ALL");
            }
            else
            {
                cmbexpiry.Items.Insert(0, "ALL");
            }

        }

        void fn_cmbunderlying()
        {
            string UNDERLYING = null;
            DataTable dtunderlying = new DataTable();
            if (rdbunderlyingAll.Checked)
            {
                if (cmbinstrutype.SelectedItem.Value == "0")
                {
                    dtunderlying = oDBEngine.GetDataTable("master_equity", "distinct Equity_ProductID", "equity_exchsegmentid='" + HttpContext.Current.Session["ExchangeSegmentID"] + "'", " Equity_ProductID");
                }
                else if (cmbinstrutype.SelectedItem.Value == "1")
                {
                    dtunderlying = oDBEngine.GetDataTable("master_equity", "distinct Equity_ProductID", "equity_exchsegmentid='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and (Equity_FOIdentifier='FUTSTK' or Equity_FOIdentifier='OPTSTK')", " Equity_ProductID");
                }
                else if (cmbinstrutype.SelectedItem.Value == "2")
                {
                    dtunderlying = oDBEngine.GetDataTable("master_equity", "distinct Equity_ProductID", "equity_exchsegmentid='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and Equity_FOIdentifier='FUTSTK'", " Equity_ProductID");
                }
                else if (cmbinstrutype.SelectedItem.Value == "3")
                {
                    dtunderlying = oDBEngine.GetDataTable("master_equity", "distinct Equity_ProductID", "equity_exchsegmentid='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and equity_FOIdentifier='OPTSTK'", " Equity_ProductID");
                }
                else if (cmbinstrutype.SelectedItem.Value == "4")
                {
                    dtunderlying = oDBEngine.GetDataTable("master_equity", "distinct Equity_ProductID", "equity_exchsegmentid='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and (Equity_FOIdentifier='FUTIDX' or Equity_FOIdentifier='OPTIDX')", " Equity_ProductID");
                }
                else if (cmbinstrutype.SelectedItem.Value == "5")
                {
                    dtunderlying = oDBEngine.GetDataTable("master_equity", "distinct Equity_ProductID", "equity_exchsegmentid='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and Equity_FOIdentifier='FUTIDX'", " Equity_ProductID");
                }
                else if (cmbinstrutype.SelectedItem.Value == "6")
                {
                    dtunderlying = oDBEngine.GetDataTable("master_equity", "distinct Equity_ProductID", "equity_exchsegmentid='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and Equity_FOIdentifier='OPTIDX'", " Equity_ProductID");
                }
            }
            if (dtunderlying.Rows.Count > 0)
            {
                for (int i = 0; i < dtunderlying.Rows.Count; i++)
                {
                    if (UNDERLYING == null)
                        UNDERLYING = dtunderlying.Rows[i][0].ToString();
                    else
                        UNDERLYING += "," + dtunderlying.Rows[i][0].ToString();
                }
            }
            if (UNDERLYING != null)
            {
                HiddenField_Product.Value = UNDERLYING;
            }

        }

        protected void cmbinstrutype_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "check", "rdbcheckchange();", true);
            fn_cmbunderlying();
            fn_cmbexpiry();

        }
        protected void dtfor_DateChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "check", "rdbcheckchange();", true);
            fn_cmbunderlying();
            fn_cmbexpiry();
        }
        protected void dtto_DateChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "check", "rdbcheckchange();", true);
            fn_cmbunderlying();
            fn_cmbexpiry();
        }

        protected void btnhide_Click(object sender, EventArgs e)
        {
            fn_cmbexpiry();
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                if (HiddenField_Group.Value.ToString().Trim() == "")
                {
                    BindGroup();
                }
            }
        }

        protected void btnshow_Click(object sender, EventArgs e)
        {
            fn_cmbunderlying();
            procedure();
            ds = (DataSet)ViewState["dataset"];
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ddlrptview.SelectedItem.Value.ToString().Trim() == "3")
                {
                    export_OpenExposure(ds);
                }
                else
                {
                    selectionchkboxlist();
                    if (ddlrptview.SelectedItem.Value.ToString().Trim() == "0")
                    {
                        ddlbandforgroup();
                        CurrentPage = 0;
                        ddlbandforClient();
                    }
                    else
                    {
                        ddlbandforshare();
                        htmltable_ShareWise(cmbgroup.SelectedItem.Value.ToString().Trim(), cmbgroup.SelectedItem.Text.ToString().Trim());

                    }
                }


            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(1);", true);
            }
        }
        void procedure()
        {
            string Broker = "";
            string ClientsID = "";
            string GRPTYPE = "";
            string GRPID = "";
            if (ddlviewby.SelectedItem.Value == "2")
            {
                Broker = "BO";
                if (rdbbrokerall.Checked)
                {
                    ClientsID = "ALL";
                }
                else
                {
                    ClientsID = HiddenField_Broker.Value;
                }

            }

            if (ddlviewby.SelectedItem.Value == "1")
            {
                Broker = "NA";
                if (rdbClientALL.Checked)
                {
                    ClientsID = "ALL";
                }
                else
                {
                    ClientsID = HiddenField_Client.Value;
                }
            }

            if (ddlGroup.SelectedItem.Value.ToString() == "0")
            {
                GRPTYPE = "BRANCH";
                if (rdbranchAll.Checked)
                {
                    GRPID = "ALL";
                }
                else
                {
                    GRPID = HiddenField_Branch.Value;
                }
            }
            else if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                GRPTYPE = ddlgrouptype.SelectedItem.Text.ToString().Trim();
                if (rdddlgrouptypeAll.Checked)
                {
                    GRPID = "ALL";
                }
                else
                {
                    GRPID = HiddenField_Group.Value;
                }
            }
            else
            {
                GRPTYPE = "BRANCHGROUP";
                if (rdbranchAll.Checked)
                {
                    GRPID = "ALL";
                }
                else
                {
                    GRPID = HiddenField_BranchGroup.Value;
                }
            }

            ds = dailyrep.NetPositionFO(ddldate.SelectedItem.Value.ToString() == "0" ? "NA" : dtfrom.Value.ToString(), ddldate.SelectedItem.Value.ToString() == "0" ? dtfor.Value.ToString() : dtto.Value.ToString(),
                Session["usersegid"].ToString(), HttpContext.Current.Session["LastFinYear"].ToString(), HttpContext.Current.Session["ExchangeSegmentID"].ToString(), Session["LastCompany"].ToString(),
                ddlNetMktValue.SelectedItem.Value.ToString() == "0" ? "NetVal" : "MrktVal", Broker, ClientsID, HiddenField_Product.Value, cmbexpiry.SelectedItem.Text.ToString(),
                Session["userbranchHierarchy"].ToString(), GRPTYPE, GRPID, chkOpenClientFUT.Checked ? "CHK" : "UNCHK", ChkOpenclinetOPT.Checked ? "CHK" : "UNCHK", ChkCalculateCharge.Checked ? "CHK" : "UNCHK",
                Chksign.Checked ? "CHK" : "UNCHK", ddlrptview.SelectedItem.Value.ToString().Trim(), cmbinstrutype.SelectedItem.Value.ToString().Trim(),
                 ChkCall.Checked ? "CHK" : "UNCHK", ChkPut.Checked ? "CHK" : "UNCHK", ddlExposure.SelectedItem.Value.ToString().Trim());

            ViewState["dataset"] = ds;
            if (ds.Tables[0].Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(1);", true);
            }
        }
        void selectionchkboxlist()
        {
            ViewState["B/F Qty"] = "unchk";
            ViewState["Open Price"] = "unchk";
            ViewState["B/F Value"] = "unchk";
            ViewState["Day Buy"] = "unchk";
            ViewState["Buy Value"] = "unchk";

            ViewState["Buy Avg."] = "unchk";
            ViewState["Day Sell"] = "unchk";
            ViewState["Sell Value"] = "unchk";
            ViewState["Sell Avg."] = "unchk";
            ViewState["ASN/EXC Qty"] = "unchk";

            ViewState["C/F Qty"] = "unchk";
            ViewState["Sett Price"] = "unchk";
            ViewState["C/F Value"] = "unchk";
            ViewState["Premium"] = "unchk";
            ViewState["Mtm"] = "unchk";


            ViewState["Future FinSett"] = "unchk";
            ViewState["ASN/EXC Amount"] = "unchk";
            ViewState["Net Obligation"] = "unchk";

            int colspan = 0;
            if (ddlrptview.SelectedItem.Value.ToString() == "0")
            {
                colspan = 1;
            }
            else
            {
                colspan = 2;
            }
            foreach (ListItem listitem in chktfilter.Items)
            {
                if (listitem.Selected)
                {
                    if (listitem.Value == "B/F Qty")
                    {
                        ViewState["B/F Qty"] = "chk";
                        colspan = colspan + 1;
                    }
                    if (listitem.Value == "Open Price")
                    {
                        ViewState["Open Price"] = "chk";
                        colspan = colspan + 1;
                    }
                    if (listitem.Value == "B/F Value")
                    {
                        ViewState["B/F Value"] = "chk";
                        colspan = colspan + 1;
                    }
                    if (listitem.Value == "Day Buy")
                    {
                        ViewState["Day Buy"] = "chk";
                        colspan = colspan + 1;
                    }
                    if (listitem.Value == "Buy Value")
                    {
                        ViewState["Buy Value"] = "chk";
                        colspan = colspan + 1;
                    }
                    if (listitem.Value == "Buy Avg.")
                    {
                        ViewState["Buy Avg."] = "chk";
                        colspan = colspan + 1;
                    }
                    if (listitem.Value == "Day Sell")
                    {
                        ViewState["Day Sell"] = "chk";
                        colspan = colspan + 1;
                    }
                    if (listitem.Value == "Sell Value")
                    {
                        ViewState["Sell Value"] = "chk";
                        colspan = colspan + 1;
                    }
                    if (listitem.Value == "Sell Avg.")
                    {
                        ViewState["Sell Avg."] = "chk";
                        colspan = colspan + 1;
                    }
                    if (listitem.Value == "ASN/EXC Qty")
                    {
                        ViewState["ASN/EXC Qty"] = "chk";
                        colspan = colspan + 1;
                    }
                    if (listitem.Value == "C/F Qty")
                    {
                        ViewState["C/F Qty"] = "chk";
                        colspan = colspan + 1;
                    }
                    if (listitem.Value == "Sett Price")
                    {
                        ViewState["Sett Price"] = "chk";
                        colspan = colspan + 1;
                    }
                    if (listitem.Value == "C/F Value")
                    {
                        ViewState["C/F Value"] = "chk";
                        colspan = colspan + 1;
                    }
                    if (listitem.Value == "Premium")
                    {
                        ViewState["Premium"] = "chk";
                        colspan = colspan + 1;
                    }
                    if (listitem.Value == "Mtm")
                    {
                        ViewState["Mtm"] = "chk";
                        colspan = colspan + 1;
                    }
                    if (listitem.Value == "Future FinSett")
                    {
                        ViewState["Future FinSett"] = "chk";
                        colspan = colspan + 1;
                    }
                    if (listitem.Value == "ASN/EXC Amount")
                    {
                        ViewState["ASN/EXC Amount"] = "chk";
                        colspan = colspan + 1;
                    }
                    if (listitem.Value == "Net Obligation")
                    {
                        ViewState["Net Obligation"] = "chk";
                        colspan = colspan + 1;
                    }
                }
            }
            ViewState["colcount"] = colspan.ToString().Trim();
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
        void ddlbandforshare()
        {
            ds = (DataSet)ViewState["dataset"];
            DataView viewgroup = new DataView(ds.Tables[0]);
            dtgroupcontactid = viewgroup.ToTable(true, new string[] { "PRODUCTID", "TICKERSYMBOL" });
            if (dtgroupcontactid.Rows.Count > 0)
            {
                cmbgroup.DataSource = dtgroupcontactid;
                cmbgroup.DataValueField = "PRODUCTID";
                cmbgroup.DataTextField = "TICKERSYMBOL";
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
        void bind_Details()
        {
            Distinctclient = (DataTable)ViewState["clients"];
            cmbclient.SelectedIndex = CurrentPage;
            if (LastPage > -1)
            {
                listRecord.Text = CurrentPage + 1 + " of " + Distinctclient.Rows.Count.ToString() + " Record.";

            }
            htmltable_ClientWise(cmbclient.SelectedItem.Value.ToString(), cmbclient.SelectedItem.Text.ToString().Trim(), cmbgroup.SelectedItem.Value.ToString(), cmbgroup.SelectedItem.Text.ToString().Trim());
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
        void htmltable_ClientWise(string clientid, string clientname, string grpid, string grpname)
        {
            ds = (DataSet)ViewState["dataset"];
            String strHtml = String.Empty;
            String strHtml1 = String.Empty;
            int flag = 0;
            string str = null;
            /////////********FOR HEADER BEGIN
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
            if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "1")/////////for mail
            {
                str = str + " [ <b>" + grpname.ToString().Trim() + "</b> ]";
            }
            strHtml1 = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtml1 += "<tr><td align=\"left\" colspan=11 style=\"color:Blue;\">" + str + "</td></tr></table>";
            /////////********FOR HEADER END


            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

            DataView viewclient = new DataView();
            viewclient = ds.Tables[0].DefaultView;
            viewclient.RowFilter = "CUSTOMERID='" + clientid.ToString().Trim() + "'";
            DataTable dt1 = new DataTable();
            dt1 = viewclient.ToTable();

            strHtml += "<tr style=\"background-color:lavender ;text-align:left\">";
            strHtml += "<td align=\"left\" colspan=20>Client Name :&nbsp;<b>" + clientname.ToString().Trim() + "</b>[ <b>" + dt1.Rows[0]["UCC"].ToString() + " </b> ]</td></tr>";

            /////////HTML TABLE HEADER
            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Scrip</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Expiry </br>Date</b></td>";
            if (ViewState["B/F Qty"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>B/F </br>Qty</b></td>";
            }
            if (ViewState["Open Price"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" ><b>Open </br>Price</b></td>";
            }
            if (ViewState["B/F Value"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>B/F </br>Value</b></td>";
            }
            if (ViewState["Day Buy"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\"><b>Day </br>Buy</b></td>";
            }
            if (ViewState["Buy Value"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Buy </br>Value</b></td>";
            }
            if (ViewState["Buy Avg."].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Buy </br>Avg.</b></td>";
            }
            if (ViewState["Day Sell"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\"><b>Day </br>Sell</b></td>";
            }
            if (ViewState["Sell Value"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Sell </br>Value</b></td>";
            }
            if (ViewState["Sell Avg."].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Sell </br>Avg.</b></td>";
            }
            if (ViewState["ASN/EXC Qty"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>ASN/EXC </br>Qty</b></td>";
            }
            if (ViewState["C/F Qty"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>C/F </br>Qty</b></td>";
            }
            if (ViewState["Sett Price"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\"><b>Sett </br>Price</b></td>";
            }
            if (ViewState["C/F Value"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>C/F </br>Value</b></td>";
            }
            if (ViewState["Premium"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Premium</b></td>";
            }
            if (ViewState["Mtm"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Mtm</b></td>";
            }
            if (ViewState["Future FinSett"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Future </br>FinSett</b></td>";
            }
            if (ViewState["ASN/EXC Amount"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>ASN/EXC </br>Amount</b></td>";
            }
            if (ViewState["Net Obligation"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Net </br>Obligation</b></td>";
            }
            strHtml += "</tr>";



            for (int k = 0; k < dt1.Rows.Count; k++)
            {
                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                strHtml += "<td  align=left style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[k]["SYMBOL"].ToString().Trim() + "</td>";
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[k]["EXPIRY"].ToString() + "</td>";
                if (ViewState["B/F Qty"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["BFQTY"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"B/F  Qty\">" + dt1.Rows[k]["BFQTY"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["Open Price"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["BFSETTPRICE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\"  title=\"Open Price\">" + dt1.Rows[k]["BFSETTPRICE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["B/F Value"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["BFVALUE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"B/F Value\">" + dt1.Rows[k]["BFVALUE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["Day Buy"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["BUYQTY"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Day Buy\">" + dt1.Rows[k]["BUYQTY"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["Buy Value"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["BUYVALUE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"Buy Value\">" + dt1.Rows[k]["BUYVALUE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["Buy Avg."].ToString() == "chk")
                {
                    if (dt1.Rows[k]["BUYAVG"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"Buy Avg.\">" + dt1.Rows[k]["BUYAVG"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["Day Sell"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["SELLQTY"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Day Sell\">" + dt1.Rows[k]["SELLQTY"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["Sell Value"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["SELLVALUE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"Sell Value\">" + dt1.Rows[k]["SELLVALUE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["Sell Avg."].ToString() == "chk")
                {
                    if (dt1.Rows[k]["SELLAVG"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"Sell Avg.\">" + dt1.Rows[k]["SELLAVG"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["ASN/EXC Qty"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["EXCASNQTY"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"ASN/EXC Qty\">" + dt1.Rows[k]["EXCASNQTY"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["C/F Qty"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["CFQTY"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"C/F Qty\">" + dt1.Rows[k]["CFQTY"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["Sett Price"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["CFSETTPRICE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Sett Price\">" + dt1.Rows[k]["CFSETTPRICE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["C/F Value"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["CFVALUE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"C/F Value\">" + dt1.Rows[k]["CFVALUE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["Premium"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["PRM"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"Premium\">" + dt1.Rows[k]["PRM"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["Mtm"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["MTM"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"Mtm\">" + dt1.Rows[k]["MTM"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["Future FinSett"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["FINSETT"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"Future FinSett\">" + dt1.Rows[k]["FINSETT"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["ASN/EXC Amount"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["EXCASNVALUE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"ASN/EXC Amount\">" + dt1.Rows[k]["EXCASNVALUE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["Net Obligation"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["NETOBLIGATION"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"Net Obligation\">" + dt1.Rows[k]["NETOBLIGATION"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }

                strHtml += "</tr>";
            }
            //////////TOTAL DISPLAY
            flag = flag + 1;
            strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
            strHtml += "<td  align=left style=\"color:maroon;font-size:xx-small;\" colspan=2><b>Total :</b></td>";
            if (ViewState["B/F Qty"].ToString() == "chk")
            {
                strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["Open Price"].ToString() == "chk")
            {
                strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["B/F Value"].ToString() == "chk")
            {
                if (dt1.Rows[0]["BFVALUESUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"B/F Value\">" + dt1.Rows[0]["BFVALUESUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["Day Buy"].ToString() == "chk")
            {
                if (dt1.Rows[0]["BUYQTYSUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\"  nowrap=\"nowrap;\" title=\"Buy  Qty\">" + dt1.Rows[0]["BUYQTYSUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["Buy Value"].ToString() == "chk")
            {
                if (dt1.Rows[0]["BUYVALUESUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"Buy Value\">" + dt1.Rows[0]["BUYVALUESUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["Buy Avg."].ToString() == "chk")
            {
                if (dt1.Rows[0]["BUYAVGSUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"Buy Avg.\">" + dt1.Rows[0]["BUYAVGSUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["Day Sell"].ToString() == "chk")
            {
                if (dt1.Rows[0]["SELLQTYSUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"Sell  Qty\">" + dt1.Rows[0]["SELLQTYSUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["Sell Value"].ToString() == "chk")
            {
                if (dt1.Rows[0]["SELLVALUESUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"Sell Value\">" + dt1.Rows[0]["SELLVALUESUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["Sell Avg."].ToString() == "chk")
            {
                if (dt1.Rows[0]["SELLAVGSUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"Sell Avg.\">" + dt1.Rows[0]["SELLAVGSUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["ASN/EXC Qty"].ToString() == "chk")
            {
                strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["C/F Qty"].ToString() == "chk")
            {
                strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["Sett Price"].ToString() == "chk")
            {
                strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["C/F Value"].ToString() == "chk")
            {
                if (dt1.Rows[0]["CFVALUESUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"C/F Value\">" + dt1.Rows[0]["CFVALUESUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["Premium"].ToString() == "chk")
            {
                if (dt1.Rows[0]["PRMSUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"Premium\">" + dt1.Rows[0]["PRMSUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["Mtm"].ToString() == "chk")
            {
                if (dt1.Rows[0]["MTMSUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" nowrap=\"nowrap;\" title=\"Mtm\">" + dt1.Rows[0]["MTMSUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["Future FinSett"].ToString() == "chk")
            {
                if (dt1.Rows[0]["FINSETTSUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"Future FinSett\">" + dt1.Rows[0]["FINSETTSUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["ASN/EXC Amount"].ToString() == "chk")
            {
                if (dt1.Rows[0]["EXCASNVALUESUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"ASN/EXC Amount\">" + dt1.Rows[0]["EXCASNVALUESUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["Net Obligation"].ToString() == "chk")
            {
                if (dt1.Rows[0]["NETOBLIGATIONSUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"Net Obligation\">" + dt1.Rows[0]["NETOBLIGATIONSUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            strHtml += "</tr>";
            strHtml += "</table>";
            if (ChkCalculateCharge.Checked)
            {
                strHtml += "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                strHtml += "<tr style=\"background-color:lavender ;text-align:left\">";
                strHtml += "<td align=\"left\" colspan=" + ViewState["colcount"].ToString().Trim() + "><b>Charges</b></td></tr>";

                ///////////STax On Brkg
                if (dt1.Rows[0]["BRKGCHARGE_NAME"] != DBNull.Value)
                {
                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\">" + dt1.Rows[0]["BRKGCHARGE_NAME"].ToString() + "</td>";
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[0]["BRKGCHARGE"].ToString() + "</td>";
                    strHtml += "</tr>";
                }
                ///////////Transaction Charges
                if (dt1.Rows[0]["TRANCHARGE_NAME"] != DBNull.Value)
                {
                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\">" + dt1.Rows[0]["TRANCHARGE_NAME"].ToString() + "</td>";
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[0]["TRANCHARGE"].ToString() + "</td>";
                    strHtml += "</tr>";
                }
                ///////////STax On Trn.Charges
                if (dt1.Rows[0]["SRVTAXTRANCHARGE_NAME"] != DBNull.Value)
                {
                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\">" + dt1.Rows[0]["SRVTAXTRANCHARGE_NAME"].ToString() + "</td>";
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[0]["SRVTAXTRANCHARGE"].ToString() + "</td>";
                    strHtml += "</tr>";
                }
                ///////////Stamp Duty
                if (dt1.Rows[0]["STAMPCHARGE_NAME"] != DBNull.Value)
                {
                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\">" + dt1.Rows[0]["STAMPCHARGE_NAME"].ToString() + "</td>";
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[0]["STAMPCHARGE"].ToString() + "</td>";
                    strHtml += "</tr>";
                }
                ///////////STT Tax
                if (dt1.Rows[0]["STTAX_NAME"] != DBNull.Value)
                {
                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\">" + dt1.Rows[0]["STTAX_NAME"].ToString() + "</td>";
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[0]["STTAX"].ToString() + "</td>";
                    strHtml += "</tr>";
                }
                ///////////SEBI Fee
                if (dt1.Rows[0]["SEBICHARGE_NAME"] != DBNull.Value)
                {
                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\">" + dt1.Rows[0]["SEBICHARGE_NAME"].ToString() + "</td>";
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[0]["SEBICHARGE"].ToString() + "</td>";
                    strHtml += "</tr>";
                }
                ///////////Clearing Charges
                if (dt1.Rows[0]["CLEARINGCHARGE_NAME"] != DBNull.Value)
                {
                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\">" + dt1.Rows[0]["CLEARINGCHARGE_NAME"].ToString() + "</td>";
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[0]["CLEARINGCHARGE"].ToString() + "</td>";
                    strHtml += "</tr>";
                }
                ///////////STax On CLEARING Charges
                if (dt1.Rows[0]["SRVTAXCLEARINGCHARGE_NAME"] != DBNull.Value)
                {
                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\">" + dt1.Rows[0]["SRVTAXCLEARINGCHARGE_NAME"].ToString() + "</td>";
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[0]["SRVTAXCLEARINGCHARGE"].ToString() + "</td>";
                    strHtml += "</tr>";
                }
                ///////////Min.Processing Charge
                if (dt1.Rows[0]["DIFFBRKG_NAME"] != DBNull.Value)
                {
                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\">" + dt1.Rows[0]["DIFFBRKG_NAME"].ToString() + "</td>";
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[0]["DIFFBRKG"].ToString() + "</td>";
                    strHtml += "</tr>";
                }
                ///////////STax On Min.Processing Charge
                if (dt1.Rows[0]["SRVDIFFBRKG_NAME"] != DBNull.Value)
                {
                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\">" + dt1.Rows[0]["SRVDIFFBRKG_NAME"].ToString() + "</td>";
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[0]["SRVDIFFBRKG"].ToString() + "</td>";
                    strHtml += "</tr>";
                }
                ///////////Net Total
                if (dt1.Rows[0]["NETOBLIGATIONCHARGE_NAME"] != DBNull.Value)
                {
                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\"><b>" + dt1.Rows[0]["NETOBLIGATIONCHARGE_NAME"].ToString() + "</b></td>";
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>" + dt1.Rows[0]["NETOBLIGATIONCHARGE"].ToString() + "</b></td>";
                    strHtml += "</tr>";
                }
            }
            strHtml += "</table>";
            if (ddlGeneration.SelectedItem.Value.ToString().Trim() != "1")/////////for mail
            {
                int width = 990;
                DIVdisplayPERIOD.Attributes.Add("style", "width: " + width + "px; ");
                display.Attributes.Add("style", "width: " + width + "px; overflow:scroll");
                DIVdisplayPERIOD.InnerHtml = strHtml1;
                display.InnerHtml = strHtml;
            }
            else
            {
                ViewState["mail"] = strHtml1 + strHtml;
            }
        }
        void htmltable_ShareWise(string productid, string productname)
        {
            ds = (DataSet)ViewState["dataset"];
            String strHtml = String.Empty;
            String strHtml1 = String.Empty;
            int flag = 0;
            string str = null;
            /////////********FOR HEADER BEGIN
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                str = "[Share Wise]";
            }

            if (ddldate.SelectedItem.Value.ToString() == "0")
            {
                str = str + "Report For " + oconverter.ArrangeDate2(dtfor.Value.ToString());
            }
            else
            {
                str = str + "Report Period " + oconverter.ArrangeDate2(dtfrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtto.Value.ToString());
            }
            if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "1")/////////for mail
            {
                str = str + " [ <b>" + productname.ToString().Trim() + "</b> ]";
            }
            strHtml1 = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtml1 += "<tr><td align=\"left\" colspan=11 style=\"color:Blue;\">" + str + "</td></tr></table>";
            /////////********FOR HEADER END


            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

            DataView viewclient = new DataView();
            viewclient = ds.Tables[0].DefaultView;
            viewclient.RowFilter = "PRODUCTID='" + productid.ToString().Trim() + "'";
            DataTable dt1 = new DataTable();
            dt1 = viewclient.ToTable();


            /////////HTML TABLE HEADER
            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Client </br> Name</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>UCC</b></td>";
            if (ViewState["B/F Qty"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>B/F </br>Qty</b></td>";
            }
            if (ViewState["Open Price"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" ><b>Open </br>Price</b></td>";
            }
            if (ViewState["B/F Value"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>B/F </br>Value</b></td>";
            }
            if (ViewState["Day Buy"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\"><b>Day </br>Buy</b></td>";
            }
            if (ViewState["Buy Value"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Buy </br>Value</b></td>";
            }
            if (ViewState["Buy Avg."].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Buy </br>Avg.</b></td>";
            }
            if (ViewState["Day Sell"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\"><b>Day </br>Sell</b></td>";
            }
            if (ViewState["Sell Value"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Sell </br>Value</b></td>";
            }
            if (ViewState["Sell Avg."].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Sell </br>Avg.</b></td>";
            }
            if (ViewState["ASN/EXC Qty"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>ASN/EXC </br>Qty</b></td>";
            }
            if (ViewState["C/F Qty"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>C/F </br>Qty</b></td>";
            }
            if (ViewState["Sett Price"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\"><b>Sett </br>Price</b></td>";
            }
            if (ViewState["C/F Value"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>C/F </br>Value</b></td>";
            }
            if (ViewState["Premium"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Premium</b></td>";
            }
            if (ViewState["Mtm"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Mtm</b></td>";
            }
            if (ViewState["Future FinSett"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Future </br>FinSett</b></td>";
            }
            if (ViewState["ASN/EXC Amount"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>ASN/EXC </br>Amount</b></td>";
            }
            if (ViewState["Net Obligation"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Net </br>Obligation</b></td>";
            }
            strHtml += "</tr>";



            for (int k = 0; k < dt1.Rows.Count; k++)
            {
                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                strHtml += "<td  align=left style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[k]["CLIENTNAME"].ToString().Trim() + "</td>";
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[k]["UCC"].ToString() + "</td>";
                if (ViewState["B/F Qty"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["BFQTY"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"B/F  Qty\">" + dt1.Rows[k]["BFQTY"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["Open Price"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["BFSETTPRICE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\"  title=\"Open Price\">" + dt1.Rows[k]["BFSETTPRICE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["B/F Value"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["BFVALUE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"B/F Value\">" + dt1.Rows[k]["BFVALUE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["Day Buy"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["BUYQTY"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Day Buy\">" + dt1.Rows[k]["BUYQTY"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["Buy Value"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["BUYVALUE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"Buy Value\">" + dt1.Rows[k]["BUYVALUE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["Buy Avg."].ToString() == "chk")
                {
                    if (dt1.Rows[k]["BUYAVG"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"Buy Avg.\">" + dt1.Rows[k]["BUYAVG"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["Day Sell"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["SELLQTY"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Day Sell\">" + dt1.Rows[k]["SELLQTY"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["Sell Value"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["SELLVALUE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"Sell Value\">" + dt1.Rows[k]["SELLVALUE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["Sell Avg."].ToString() == "chk")
                {
                    if (dt1.Rows[k]["SELLAVG"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"Sell Avg.\">" + dt1.Rows[k]["SELLAVG"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["ASN/EXC Qty"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["EXCASNQTY"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"ASN/EXC Qty\">" + dt1.Rows[k]["EXCASNQTY"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["C/F Qty"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["CFQTY"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"C/F Qty\">" + dt1.Rows[k]["CFQTY"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["Sett Price"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["CFSETTPRICE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" title=\"Sett Price\">" + dt1.Rows[k]["CFSETTPRICE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["C/F Value"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["CFVALUE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"C/F Value\">" + dt1.Rows[k]["CFVALUE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["Premium"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["PRM"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"Premium\">" + dt1.Rows[k]["PRM"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["Mtm"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["MTM"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"Mtm\">" + dt1.Rows[k]["MTM"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["Future FinSett"].ToString() == "chk")
                {

                    if (dt1.Rows[k]["FINSETT"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"Future FinSett\">" + dt1.Rows[k]["FINSETT"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["ASN/EXC Amount"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["EXCASNVALUE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"ASN/EXC Amount\">" + dt1.Rows[k]["EXCASNVALUE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["Net Obligation"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["NETOBLIGATION"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"Net Obligation\">" + dt1.Rows[k]["NETOBLIGATION"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }


                strHtml += "</tr>";
            }
            //////////TOTAL DISPLAY
            flag = flag + 1;
            strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
            strHtml += "<td  align=left style=\"color:maroon;font-size:xx-small;\" colspan=2><b>Total :</b></td>";
            if (ViewState["B/F Qty"].ToString() == "chk")
            {
                strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["Open Price"].ToString() == "chk")
            {
                strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["B/F Value"].ToString() == "chk")
            {
                if (dt1.Rows[0]["BFVALUESUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"B/F Value\">" + dt1.Rows[0]["BFVALUESUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["Day Buy"].ToString() == "chk")
            {
                if (dt1.Rows[0]["BUYQTYSUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\"  nowrap=\"nowrap;\" title=\"Buy  Qty\">" + dt1.Rows[0]["BUYQTYSUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["Buy Value"].ToString() == "chk")
            {
                if (dt1.Rows[0]["BUYVALUESUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"Buy Value\">" + dt1.Rows[0]["BUYVALUESUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["Buy Avg."].ToString() == "chk")
            {
                if (dt1.Rows[0]["BUYAVGSUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"Buy Avg.\">" + dt1.Rows[0]["BUYAVGSUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["Day Sell"].ToString() == "chk")
            {
                if (dt1.Rows[0]["SELLQTYSUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"Sell  Qty\">" + dt1.Rows[0]["SELLQTYSUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["Sell Value"].ToString() == "chk")
            {
                if (dt1.Rows[0]["SELLVALUESUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"Sell Avg.\">" + dt1.Rows[0]["SELLVALUESUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["Sell Avg."].ToString() == "chk")
            {
                if (dt1.Rows[0]["SELLAVGSUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"Sell Avg.\">" + dt1.Rows[0]["SELLAVGSUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["ASN/EXC Qty"].ToString() == "chk")
            {
                strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["C/F Qty"].ToString() == "chk")
            {
                strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["Sett Price"].ToString() == "chk")
            {
                strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["C/F Value"].ToString() == "chk")
            {
                if (dt1.Rows[0]["CFVALUESUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"C/F Value\">" + dt1.Rows[0]["CFVALUESUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["Premium"].ToString() == "chk")
            {
                if (dt1.Rows[0]["PRMSUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"Premium\">" + dt1.Rows[0]["PRMSUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["Mtm"].ToString() == "chk")
            {
                if (dt1.Rows[0]["MTMSUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" nowrap=\"nowrap;\" title=\"Mtm\">" + dt1.Rows[0]["MTMSUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["Future FinSett"].ToString() == "chk")
            {
                if (dt1.Rows[0]["FINSETTSUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"Future FinSett\">" + dt1.Rows[0]["FINSETTSUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["ASN/EXC Amount"].ToString() == "chk")
            {
                if (dt1.Rows[0]["EXCASNVALUESUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"ASN/EXC Amount\">" + dt1.Rows[0]["EXCASNVALUESUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["Net Obligation"].ToString() == "chk")
            {
                if (dt1.Rows[0]["NETOBLIGATIONSUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" title=\"Net Obligation\">" + dt1.Rows[0]["NETOBLIGATIONSUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            strHtml += "</tr>";


            strHtml += "</table>";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Display", "Display(2);", true);
            if (ddlGeneration.SelectedItem.Value.ToString().Trim() != "1")/////////for mail
            {
                int width = 990;
                DIVdisplayPERIOD.Attributes.Add("style", "width: " + width + "px; ");
                display.Attributes.Add("style", "width: " + width + "px; overflow:scroll");
                DIVdisplayPERIOD.InnerHtml = strHtml1;
                display.InnerHtml = strHtml;
            }
            else
            {
                ViewState["mail"] = strHtml1 + strHtml;
            }
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

            if (ddlrptview.SelectedItem.Value.ToString().Trim() == "0")
            {
                CurrentPage = 0;
                ddlbandforClient();
            }
            else
            {
                htmltable_ShareWise(cmbgroup.SelectedItem.Value.ToString().Trim(), cmbgroup.SelectedItem.Text.ToString().Trim());
            }


        }
        protected void cmbgroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlrptview.SelectedItem.Value.ToString().Trim() == "0")
            {
                CurrentPage = 0;
                ddlbandforClient();
            }
            else
            {
                htmltable_ShareWise(cmbgroup.SelectedItem.Value.ToString().Trim(), cmbgroup.SelectedItem.Text.ToString().Trim());
            }

        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            procedure();
            ds = (DataSet)ViewState["dataset"];
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ddlrptview.SelectedItem.Value.ToString().Trim() == "3")
                {
                    export_OpenExposure(ds);
                }
                else
                {
                    selectionchkboxlist();
                    if (ddlrptview.SelectedItem.Value.ToString().Trim() == "0")
                    {
                        ddlbandforgroup();
                        export_clientwise();
                    }
                    else
                    {
                        ddlbandforshare();
                        export_sharewise();
                    }
                    export();
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(1);", true);
            }


        }
        void export_clientwise()
        {
            ds = (DataSet)ViewState["dataset"];
            DataTable dtExport = new DataTable();
            dtExport.Clear();
            dtExport.Columns.Add("Scrip", Type.GetType("System.String"));
            dtExport.Columns.Add("Expiry Date", Type.GetType("System.String"));
            if (ViewState["B/F Qty"].ToString() == "chk")
            {
                dtExport.Columns.Add("B/F Qty", Type.GetType("System.String"));
            }
            if (ViewState["Open Price"].ToString() == "chk")
            {
                dtExport.Columns.Add("Open Price", Type.GetType("System.String"));
            }
            if (ViewState["B/F Value"].ToString() == "chk")
            {
                dtExport.Columns.Add("B/F Value", Type.GetType("System.String"));
            }
            if (ViewState["Day Buy"].ToString() == "chk")
            {
                dtExport.Columns.Add("Day Buy", Type.GetType("System.String"));
            }
            if (ViewState["Buy Value"].ToString() == "chk")
            {
                dtExport.Columns.Add("Buy Value", Type.GetType("System.String"));
            }
            if (ViewState["Buy Avg."].ToString() == "chk")
            {
                dtExport.Columns.Add("Buy Avg.", Type.GetType("System.String"));
            }
            if (ViewState["Day Sell"].ToString() == "chk")
            {
                dtExport.Columns.Add("Day Sell", Type.GetType("System.String"));
            }
            if (ViewState["Sell Value"].ToString() == "chk")
            {
                dtExport.Columns.Add("Sell Value", Type.GetType("System.String"));
            }
            if (ViewState["Sell Avg."].ToString() == "chk")
            {
                dtExport.Columns.Add("Sell Avg.", Type.GetType("System.String"));
            }
            if (ViewState["ASN/EXC Qty"].ToString() == "chk")
            {
                dtExport.Columns.Add("ASN/EXC Qty", Type.GetType("System.String"));
            }
            if (ViewState["C/F Qty"].ToString() == "chk")
            {
                dtExport.Columns.Add("C/F Qty", Type.GetType("System.String"));
            }
            if (ViewState["Sett Price"].ToString() == "chk")
            {
                dtExport.Columns.Add("Sett Price", Type.GetType("System.String"));
            }
            if (ViewState["C/F Value"].ToString() == "chk")
            {
                dtExport.Columns.Add("C/F Value", Type.GetType("System.String"));
            }
            if (ViewState["Premium"].ToString() == "chk")
            {
                dtExport.Columns.Add("Premium", Type.GetType("System.String"));
            }
            if (ViewState["Mtm"].ToString() == "chk")
            {
                dtExport.Columns.Add("Mtm", Type.GetType("System.String"));
            }
            if (ViewState["Future FinSett"].ToString() == "chk")
            {
                dtExport.Columns.Add("Future FinSett", Type.GetType("System.String"));
            }
            if (ViewState["ASN/EXC Amount"].ToString() == "chk")
            {
                dtExport.Columns.Add("ASN/EXC Amount", Type.GetType("System.String"));
            }
            if (ViewState["Net Obligation"].ToString() == "chk")
            {
                dtExport.Columns.Add("Net Obligation", Type.GetType("System.String"));
            }
            //===For Only PDF and Client Wise
            if ((cmbExport.SelectedItem.Value == "P") && (ddlrptview.SelectedItem.Value == "0"))
            {
                dtExport.Columns.Add("CUSTOMERID", Type.GetType("System.String"));
            }
            for (int j = 0; j < cmbgroup.Items.Count; j++)
            {
                if (cmbExport.SelectedItem.Value != "P")
                {
                    DataRow row = dtExport.NewRow();
                    row["Scrip"] = ddlGroup.SelectedItem.Text.ToString().Trim() + " Name:" + cmbgroup.Items[j].Text.ToString().Trim();
                    row["Expiry Date"] = "Test";

                    dtExport.Rows.Add(row);
                }

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
                    row1["Scrip"] = "Client Name:" + cmbclient.Items[k].Text.ToString().Trim();
                    row1["Expiry Date"] = "Test";
                    if ((cmbExport.SelectedItem.Value == "P") && (ddlrptview.SelectedItem.Value == "0"))
                    {
                        row1["CustomerID"] = cmbclient.Items[k].Value.ToString();
                    }
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
                        row2[1] = dt1.Rows[i]["EXPIRY"].ToString();
                        if (ViewState["B/F Qty"].ToString() == "chk")
                        {
                            if (dt1.Rows[i]["BFQTY"] != DBNull.Value)
                                row2["B/F Qty"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["BFQTY"].ToString()));
                        }
                        if (ViewState["Open Price"].ToString() == "chk")
                        {
                            if (dt1.Rows[i]["BFSETTPRICE"] != DBNull.Value)
                                row2["Open Price"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["BFSETTPRICE"].ToString()));
                        }
                        if (ViewState["B/F Value"].ToString() == "chk")
                        {
                            if (dt1.Rows[i]["BFVALUE"] != DBNull.Value)
                                row2["B/F Value"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["BFVALUE"].ToString()));
                        }
                        if (ViewState["Day Buy"].ToString() == "chk")
                        {
                            if (dt1.Rows[i]["BUYQTY"] != DBNull.Value)
                                row2["Day Buy"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["BUYQTY"].ToString()));
                        }
                        if (ViewState["Buy Value"].ToString() == "chk")
                        {
                            if (dt1.Rows[i]["BUYVALUE"] != DBNull.Value)
                                row2["Buy Value"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["BUYVALUE"].ToString()));
                        }
                        if (ViewState["Buy Avg."].ToString() == "chk")
                        {
                            if (dt1.Rows[i]["BUYAVG"] != DBNull.Value)
                                row2["Buy Avg."] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["BUYAVG"].ToString()));
                        }
                        if (ViewState["Day Sell"].ToString() == "chk")
                        {
                            if (dt1.Rows[i]["SELLQTY"] != DBNull.Value)
                                row2["Day Sell"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["SELLQTY"].ToString()));
                        }
                        if (ViewState["Sell Value"].ToString() == "chk")
                        {
                            if (dt1.Rows[i]["SELLVALUE"] != DBNull.Value)
                                row2["Sell Value"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["SELLVALUE"].ToString()));
                        }
                        if (ViewState["Sell Avg."].ToString() == "chk")
                        {
                            if (dt1.Rows[i]["SELLAVG"] != DBNull.Value)
                                row2["Sell Avg."] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["SELLAVG"].ToString()));
                        }
                        if (ViewState["ASN/EXC Qty"].ToString() == "chk")
                        {
                            if (dt1.Rows[i]["EXCASNQTY"] != DBNull.Value)
                                row2["ASN/EXC Qty"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["EXCASNQTY"].ToString()));
                        }
                        if (ViewState["C/F Qty"].ToString() == "chk")
                        {
                            if (dt1.Rows[i]["CFQTY"] != DBNull.Value)
                                row2["C/F Qty"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["CFQTY"].ToString()));
                        }
                        if (ViewState["Sett Price"].ToString() == "chk")
                        {
                            if (dt1.Rows[i]["CFSETTPRICE"] != DBNull.Value)
                                row2["Sett Price"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["CFSETTPRICE"].ToString()));
                        }
                        if (ViewState["C/F Value"].ToString() == "chk")
                        {
                            if (dt1.Rows[i]["CFVALUE"] != DBNull.Value)
                                row2["C/F Value"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["CFVALUE"].ToString()));
                        }
                        if (ViewState["Premium"].ToString() == "chk")
                        {
                            if (dt1.Rows[i]["PRM"] != DBNull.Value)
                                row2["Premium"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["PRM"].ToString()));
                        }
                        if (ViewState["Mtm"].ToString() == "chk")
                        {
                            if (dt1.Rows[i]["MTM"] != DBNull.Value)
                                row2["Mtm"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["MTM"].ToString()));
                        }
                        if (ViewState["Future FinSett"].ToString() == "chk")
                        {
                            if (dt1.Rows[i]["FINSETT"] != DBNull.Value)
                                row2["Future FinSett"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["FINSETT"].ToString()));
                        }
                        if (ViewState["ASN/EXC Amount"].ToString() == "chk")
                        {
                            if (dt1.Rows[i]["EXCASNVALUE"] != DBNull.Value)
                                row2["ASN/EXC Amount"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["EXCASNVALUE"].ToString()));
                        }
                        if (ViewState["Net Obligation"].ToString() == "chk")
                        {
                            if (dt1.Rows[i]["NETOBLIGATION"] != DBNull.Value)
                                row2["Net Obligation"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["NETOBLIGATION"].ToString()));
                        }
                        if ((cmbExport.SelectedItem.Value == "P") && (ddlrptview.SelectedItem.Value == "0"))
                        {
                            row2["CustomerID"] = dt1.Rows[i]["CUSTOMERID"].ToString();
                        }
                        dtExport.Rows.Add(row2);
                    }
                    //////////client total
                    DataRow row3 = dtExport.NewRow();
                    row3[0] = "Total :";
                    if (ViewState["B/F Value"].ToString() == "chk")
                    {
                        if (dt1.Rows[0]["BFVALUESUM"] != DBNull.Value)
                            row3["B/F Value"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["BFVALUESUM"].ToString()));
                    }
                    if (ViewState["Day Buy"].ToString() == "chk")
                    {
                        if (dt1.Rows[0]["BUYQTYSUM"] != DBNull.Value)
                            row3["Day Buy"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["BUYQTYSUM"].ToString()));
                    }
                    if (ViewState["Buy Value"].ToString() == "chk")
                    {
                        if (dt1.Rows[0]["BUYVALUESUM"] != DBNull.Value)
                            row3["Buy Value"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["BUYVALUESUM"].ToString()));
                    }
                    if (ViewState["Buy Avg."].ToString() == "chk")
                    {
                        if (dt1.Rows[0]["BUYAVGSUM"] != DBNull.Value)
                            row3["Buy Avg."] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["BUYAVGSUM"].ToString()));
                    }
                    if (ViewState["Day Sell"].ToString() == "chk")
                    {
                        if (dt1.Rows[0]["SELLQTYSUM"] != DBNull.Value)
                            row3["Day Sell"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["SELLQTYSUM"].ToString()));
                    }
                    if (ViewState["Sell Value"].ToString() == "chk")
                    {
                        if (dt1.Rows[0]["SELLVALUESUM"] != DBNull.Value)
                            row3["Sell Value"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["SELLVALUESUM"].ToString()));
                    }
                    if (ViewState["Sell Avg."].ToString() == "chk")
                    {
                        if (dt1.Rows[0]["SELLAVGSUM"] != DBNull.Value)
                            row3["Sell Avg."] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["SELLAVGSUM"].ToString()));
                    }
                    if (ViewState["C/F Value"].ToString() == "chk")
                    {
                        if (dt1.Rows[0]["CFVALUESUM"] != DBNull.Value)
                            row3["C/F Value"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["CFVALUESUM"].ToString()));
                    }
                    if (ViewState["Premium"].ToString() == "chk")
                    {
                        if (dt1.Rows[0]["PRMSUM"] != DBNull.Value)
                            row3["Premium"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["PRMSUM"].ToString()));
                    }
                    if (ViewState["Mtm"].ToString() == "chk")
                    {
                        if (dt1.Rows[0]["MTMSUM"] != DBNull.Value)
                            row3["Mtm"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["MTMSUM"].ToString()));
                    }
                    if (ViewState["Future FinSett"].ToString() == "chk")
                    {
                        if (dt1.Rows[0]["FINSETTSUM"] != DBNull.Value)
                            row3["Future FinSett"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["FINSETTSUM"].ToString()));
                    }
                    if (ViewState["ASN/EXC Amount"].ToString() == "chk")
                    {
                        if (dt1.Rows[0]["EXCASNVALUESUM"] != DBNull.Value)
                            row3["ASN/EXC Amount"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["EXCASNVALUESUM"].ToString()));
                    }
                    if (ViewState["Net Obligation"].ToString() == "chk")
                    {
                        if (dt1.Rows[0]["NETOBLIGATIONSUM"] != DBNull.Value)
                            row3["Net Obligation"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["NETOBLIGATIONSUM"].ToString()));
                    }
                    if ((cmbExport.SelectedItem.Value == "P") && (ddlrptview.SelectedItem.Value == "0"))
                    {
                        row3["CustomerID"] = dt1.Rows[0]["CUSTOMERID"].ToString();
                    }
                    dtExport.Rows.Add(row3);

                    ///////////charges total
                    ///////////charges total
                    DataRow row111 = dtExport.NewRow();
                    row111[0] = "Charges :";
                    row111[1] = "Test";
                    if ((cmbExport.SelectedItem.Value == "P") && (ddlrptview.SelectedItem.Value == "0"))
                    {
                        row111["CustomerID"] = dt1.Rows[0]["CUSTOMERID"].ToString();
                    }
                    dtExport.Rows.Add(row111);
                    ///////////STax On Brkg
                    if (dt1.Rows[0]["BRKGCHARGE_NAME"] != DBNull.Value)
                    {
                        DataRow row4 = dtExport.NewRow();
                        row4[0] = dt1.Rows[0]["BRKGCHARGE_NAME"].ToString();
                        row4[1] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["BRKGCHARGE"].ToString()));
                        if ((cmbExport.SelectedItem.Value == "P") && (ddlrptview.SelectedItem.Value == "0"))
                        {
                            row4["CustomerID"] = dt1.Rows[0]["CUSTOMERID"].ToString();
                        }
                        dtExport.Rows.Add(row4);
                    }
                    ///////////Transaction Charges
                    if (dt1.Rows[0]["TRANCHARGE_NAME"] != DBNull.Value)
                    {
                        DataRow row5 = dtExport.NewRow();
                        row5[0] = dt1.Rows[0]["TRANCHARGE_NAME"].ToString();
                        row5[1] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["TRANCHARGE"].ToString()));
                        if ((cmbExport.SelectedItem.Value == "P") && (ddlrptview.SelectedItem.Value == "0"))
                        {
                            row5["CustomerID"] = dt1.Rows[0]["CUSTOMERID"].ToString();
                        }
                        dtExport.Rows.Add(row5);
                    }
                    ///////////STax On Trn.Charges
                    if (dt1.Rows[0]["SRVTAXTRANCHARGE_NAME"] != DBNull.Value)
                    {
                        DataRow row6 = dtExport.NewRow();
                        row6[0] = dt1.Rows[0]["SRVTAXTRANCHARGE_NAME"].ToString();
                        row6[1] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["SRVTAXTRANCHARGE"].ToString()));
                        if ((cmbExport.SelectedItem.Value == "P") && (ddlrptview.SelectedItem.Value == "0"))
                        {
                            row6["CustomerID"] = dt1.Rows[0]["CUSTOMERID"].ToString();
                        }
                        dtExport.Rows.Add(row6);
                    }
                    ///////////Stamp Duty
                    if (dt1.Rows[0]["STAMPCHARGE_NAME"] != DBNull.Value)
                    {
                        DataRow row5 = dtExport.NewRow();
                        row5[0] = dt1.Rows[0]["STAMPCHARGE_NAME"].ToString();
                        row5[1] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["STAMPCHARGE"].ToString()));
                        if ((cmbExport.SelectedItem.Value == "P") && (ddlrptview.SelectedItem.Value == "0"))
                        {
                            row5["CustomerID"] = dt1.Rows[0]["CUSTOMERID"].ToString();
                        }
                        dtExport.Rows.Add(row5);
                    }
                    ///////////STT Tax
                    if (dt1.Rows[0]["STTAX_NAME"] != DBNull.Value)
                    {
                        DataRow row6 = dtExport.NewRow();
                        row6[0] = dt1.Rows[0]["STTAX_NAME"].ToString();
                        row6[1] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["STTAX"].ToString()));
                        if ((cmbExport.SelectedItem.Value == "P") && (ddlrptview.SelectedItem.Value == "0"))
                        {
                            row6["CustomerID"] = dt1.Rows[0]["CUSTOMERID"].ToString();
                        }
                        dtExport.Rows.Add(row6);
                    }
                    ///////////SEBI Fee
                    if (dt1.Rows[0]["SEBICHARGE_NAME"] != DBNull.Value)
                    {
                        DataRow row7 = dtExport.NewRow();
                        row7[0] = dt1.Rows[0]["SEBICHARGE_NAME"].ToString();
                        row7[1] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["SEBICHARGE"].ToString()));
                        if ((cmbExport.SelectedItem.Value == "P") && (ddlrptview.SelectedItem.Value == "0"))
                        {
                            row7["CustomerID"] = dt1.Rows[0]["CUSTOMERID"].ToString();
                        } dtExport.Rows.Add(row7);
                    }
                    ///////////CLEARING Charges
                    if (dt1.Rows[0]["CLEARINGCHARGE_NAME"] != DBNull.Value)
                    {
                        DataRow row8 = dtExport.NewRow();
                        row8[0] = dt1.Rows[0]["CLEARINGCHARGE_NAME"].ToString();
                        row8[1] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["CLEARINGCHARGE"].ToString()));
                        if ((cmbExport.SelectedItem.Value == "P") && (ddlrptview.SelectedItem.Value == "0"))
                        {
                            row8["CustomerID"] = dt1.Rows[0]["CUSTOMERID"].ToString();
                        }
                        dtExport.Rows.Add(row8);
                    }
                    ///////////STax On CLEARING Charges
                    if (dt1.Rows[0]["SRVTAXCLEARINGCHARGE_NAME"] != DBNull.Value)
                    {
                        DataRow row9 = dtExport.NewRow();
                        row9[0] = dt1.Rows[0]["SRVTAXCLEARINGCHARGE_NAME"].ToString();
                        row9[1] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["SRVTAXCLEARINGCHARGE"].ToString()));
                        if ((cmbExport.SelectedItem.Value == "P") && (ddlrptview.SelectedItem.Value == "0"))
                        {
                            row9["CustomerID"] = dt1.Rows[0]["CUSTOMERID"].ToString();
                        }
                        dtExport.Rows.Add(row9);
                    }
                    ///////////Min.Processing Charge
                    if (dt1.Rows[0]["DIFFBRKG_NAME"] != DBNull.Value)
                    {
                        DataRow row10 = dtExport.NewRow();
                        row10[0] = dt1.Rows[0]["DIFFBRKG_NAME"].ToString();
                        row10[1] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["DIFFBRKG"].ToString()));
                        if ((cmbExport.SelectedItem.Value == "P") && (ddlrptview.SelectedItem.Value == "0"))
                        {
                            row10["CustomerID"] = dt1.Rows[0]["CUSTOMERID"].ToString();
                        }
                        dtExport.Rows.Add(row10);
                    }
                    ///////////STax On Min.Processing Charge
                    if (dt1.Rows[0]["SRVDIFFBRKG_NAME"] != DBNull.Value)
                    {
                        DataRow row11 = dtExport.NewRow();
                        row11[0] = dt1.Rows[0]["SRVDIFFBRKG_NAME"].ToString();
                        row11[1] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["SRVDIFFBRKG"].ToString()));
                        if ((cmbExport.SelectedItem.Value == "P") && (ddlrptview.SelectedItem.Value == "0"))
                        {
                            row11["CustomerID"] = dt1.Rows[0]["CUSTOMERID"].ToString();
                        }
                        dtExport.Rows.Add(row11);
                    }
                    ///////////Net Total
                    if (dt1.Rows[0]["NETOBLIGATIONCHARGE_NAME"] != DBNull.Value)
                    {
                        DataRow row12 = dtExport.NewRow();
                        row12[0] = dt1.Rows[0]["NETOBLIGATIONCHARGE_NAME"].ToString();
                        row12[1] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["NETOBLIGATIONCHARGE"].ToString()));
                        if ((cmbExport.SelectedItem.Value == "P") && (ddlrptview.SelectedItem.Value == "0"))
                        {
                            row12["CustomerID"] = dt1.Rows[0]["CUSTOMERID"].ToString();
                        } dtExport.Rows.Add(row12);
                    }
                }

            }
            ViewState["dtExport"] = dtExport;
        }
        void export_sharewise()
        {
            ds = (DataSet)ViewState["dataset"];
            DataTable dtExport = new DataTable();
            dtExport.Clear();
            dtExport.Columns.Add("Client Name", Type.GetType("System.String"));
            dtExport.Columns.Add("UCC", Type.GetType("System.String"));
            if (ViewState["B/F Qty"].ToString() == "chk")
            {
                dtExport.Columns.Add("B/F Qty", Type.GetType("System.String"));
            }
            if (ViewState["Open Price"].ToString() == "chk")
            {
                dtExport.Columns.Add("Open Price", Type.GetType("System.String"));
            }
            if (ViewState["B/F Value"].ToString() == "chk")
            {
                dtExport.Columns.Add("B/F Value", Type.GetType("System.String"));
            }
            if (ViewState["Day Buy"].ToString() == "chk")
            {
                dtExport.Columns.Add("Day Buy", Type.GetType("System.String"));
            }
            if (ViewState["Buy Value"].ToString() == "chk")
            {
                dtExport.Columns.Add("Buy Value", Type.GetType("System.String"));
            }
            if (ViewState["Buy Avg."].ToString() == "chk")
            {
                dtExport.Columns.Add("Buy Avg.", Type.GetType("System.String"));
            }
            if (ViewState["Day Sell"].ToString() == "chk")
            {
                dtExport.Columns.Add("Day Sell", Type.GetType("System.String"));
            }
            if (ViewState["Sell Value"].ToString() == "chk")
            {
                dtExport.Columns.Add("Sell Value", Type.GetType("System.String"));
            }
            if (ViewState["Sell Avg."].ToString() == "chk")
            {
                dtExport.Columns.Add("Sell Avg.", Type.GetType("System.String"));
            }
            if (ViewState["ASN/EXC Qty"].ToString() == "chk")
            {
                dtExport.Columns.Add("ASN/EXC Qty", Type.GetType("System.String"));
            }
            if (ViewState["C/F Qty"].ToString() == "chk")
            {
                dtExport.Columns.Add("C/F Qty", Type.GetType("System.String"));
            }
            if (ViewState["Sett Price"].ToString() == "chk")
            {
                dtExport.Columns.Add("Sett Price", Type.GetType("System.String"));
            }
            if (ViewState["C/F Value"].ToString() == "chk")
            {
                dtExport.Columns.Add("C/F Value", Type.GetType("System.String"));
            }
            if (ViewState["Premium"].ToString() == "chk")
            {
                dtExport.Columns.Add("Premium", Type.GetType("System.String"));
            }
            if (ViewState["Mtm"].ToString() == "chk")
            {
                dtExport.Columns.Add("Mtm", Type.GetType("System.String"));
            }
            if (ViewState["Future FinSett"].ToString() == "chk")
            {
                dtExport.Columns.Add("Future FinSett", Type.GetType("System.String"));
            }
            if (ViewState["ASN/EXC Amount"].ToString() == "chk")
            {
                dtExport.Columns.Add("ASN/EXC Amount", Type.GetType("System.String"));
            }
            if (ViewState["Net Obligation"].ToString() == "chk")
            {
                dtExport.Columns.Add("Net Obligation", Type.GetType("System.String"));
            }

            for (int j = 0; j < cmbgroup.Items.Count; j++)
            {
                DataRow row = dtExport.NewRow();
                row["Client Name"] = "Share Name:" + cmbgroup.Items[j].Text.ToString().Trim();
                row["UCC"] = "Test";
                dtExport.Rows.Add(row);

                DataView viewshare = new DataView();
                viewshare = ds.Tables[0].DefaultView;
                viewshare.RowFilter = "PRODUCTID='" + cmbgroup.Items[j].Value + "'";
                DataTable dt1 = new DataTable();
                dt1 = viewshare.ToTable();


                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    DataRow row2 = dtExport.NewRow();
                    row2[0] = dt1.Rows[i]["CLIENTNAME"].ToString();
                    row2[1] = dt1.Rows[i]["UCC"].ToString();
                    if (ViewState["B/F Qty"].ToString() == "chk")
                    {
                        if (dt1.Rows[i]["BFQTY"] != DBNull.Value)
                            row2["B/F Qty"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["BFQTY"].ToString()));
                    }
                    if (ViewState["Open Price"].ToString() == "chk")
                    {
                        if (dt1.Rows[i]["BFSETTPRICE"] != DBNull.Value)
                            row2["Open Price"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["BFSETTPRICE"].ToString()));
                    }
                    if (ViewState["B/F Value"].ToString() == "chk")
                    {
                        if (dt1.Rows[i]["BFVALUE"] != DBNull.Value)
                            row2["B/F Value"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["BFVALUE"].ToString()));
                    }
                    if (ViewState["Day Buy"].ToString() == "chk")
                    {
                        if (dt1.Rows[i]["BUYQTY"] != DBNull.Value)
                            row2["Day Buy"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["BUYQTY"].ToString()));
                    }
                    if (ViewState["Buy Value"].ToString() == "chk")
                    {
                        if (dt1.Rows[i]["BUYVALUE"] != DBNull.Value)
                            row2["Buy Value"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["BUYVALUE"].ToString()));
                    }
                    if (ViewState["Buy Avg."].ToString() == "chk")
                    {
                        if (dt1.Rows[i]["BUYAVG"] != DBNull.Value)
                            row2["Buy Avg."] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["BUYAVG"].ToString()));
                    }
                    if (ViewState["Day Sell"].ToString() == "chk")
                    {
                        if (dt1.Rows[i]["SELLQTY"] != DBNull.Value)
                            row2["Day Sell"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["SELLQTY"].ToString()));
                    }
                    if (ViewState["Sell Value"].ToString() == "chk")
                    {
                        if (dt1.Rows[i]["SELLVALUE"] != DBNull.Value)
                            row2["Sell Value"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["SELLVALUE"].ToString()));
                    }
                    if (ViewState["Sell Avg."].ToString() == "chk")
                    {
                        if (dt1.Rows[i]["SELLAVG"] != DBNull.Value)
                            row2["Sell Avg."] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["SELLAVG"].ToString()));
                    }
                    if (ViewState["ASN/EXC Qty"].ToString() == "chk")
                    {
                        if (dt1.Rows[i]["EXCASNQTY"] != DBNull.Value)
                            row2["ASN/EXC Qty"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["EXCASNQTY"].ToString()));
                    }
                    if (ViewState["C/F Qty"].ToString() == "chk")
                    {
                        if (dt1.Rows[i]["CFQTY"] != DBNull.Value)
                            row2["C/F Qty"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["CFQTY"].ToString()));
                    }
                    if (ViewState["Sett Price"].ToString() == "chk")
                    {
                        if (dt1.Rows[i]["CFSETTPRICE"] != DBNull.Value)
                            row2["Sett Price"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["CFSETTPRICE"].ToString()));
                    }
                    if (ViewState["C/F Value"].ToString() == "chk")
                    {
                        if (dt1.Rows[i]["CFVALUE"] != DBNull.Value)
                            row2["C/F Value"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["CFVALUE"].ToString()));
                    }
                    if (ViewState["Premium"].ToString() == "chk")
                    {
                        if (dt1.Rows[i]["PRM"] != DBNull.Value)
                            row2["Premium"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["PRM"].ToString()));
                    }
                    if (ViewState["Mtm"].ToString() == "chk")
                    {
                        if (dt1.Rows[i]["MTM"] != DBNull.Value)
                            row2["Mtm"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["MTM"].ToString()));
                    }
                    if (ViewState["Future FinSett"].ToString() == "chk")
                    {
                        if (dt1.Rows[i]["FINSETT"] != DBNull.Value)
                            row2["Future FinSett"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["FINSETT"].ToString()));
                    }
                    if (ViewState["ASN/EXC Amount"].ToString() == "chk")
                    {
                        if (dt1.Rows[i]["EXCASNVALUE"] != DBNull.Value)
                            row2["ASN/EXC Amount"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["EXCASNVALUE"].ToString()));
                    }
                    if (ViewState["Net Obligation"].ToString() == "chk")
                    {
                        if (dt1.Rows[i]["NETOBLIGATION"] != DBNull.Value)
                            row2["Net Obligation"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["NETOBLIGATION"].ToString()));
                    }
                    dtExport.Rows.Add(row2);
                }
                //////////share total
                //////////client total
                DataRow row3 = dtExport.NewRow();
                row3[0] = "Total :";
                if (ViewState["B/F Value"].ToString() == "chk")
                {
                    if (dt1.Rows[0]["BFVALUESUM"] != DBNull.Value)
                        row3["B/F Value"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["BFVALUESUM"].ToString()));
                }
                if (ViewState["Day Buy"].ToString() == "chk")
                {
                    if (dt1.Rows[0]["BUYQTYSUM"] != DBNull.Value)
                        row3["Day Buy"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["BUYQTYSUM"].ToString()));
                }
                if (ViewState["Buy Value"].ToString() == "chk")
                {
                    if (dt1.Rows[0]["BUYVALUESUM"] != DBNull.Value)
                        row3["Buy Value"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["BUYVALUESUM"].ToString()));
                }
                if (ViewState["Buy Avg."].ToString() == "chk")
                {
                    if (dt1.Rows[0]["BUYAVGSUM"] != DBNull.Value)
                        row3["Buy Avg."] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["BUYAVGSUM"].ToString()));
                }
                if (ViewState["Day Sell"].ToString() == "chk")
                {
                    if (dt1.Rows[0]["SELLQTYSUM"] != DBNull.Value)
                        row3["Day Sell"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["SELLQTYSUM"].ToString()));
                }
                if (ViewState["Sell Value"].ToString() == "chk")
                {
                    if (dt1.Rows[0]["SELLVALUESUM"] != DBNull.Value)
                        row3["Sell Value"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["SELLVALUESUM"].ToString()));
                }
                if (ViewState["Sell Avg."].ToString() == "chk")
                {
                    if (dt1.Rows[0]["SELLAVGSUM"] != DBNull.Value)
                        row3["Sell Avg."] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["SELLAVGSUM"].ToString()));
                }
                if (ViewState["C/F Value"].ToString() == "chk")
                {
                    if (dt1.Rows[0]["CFVALUESUM"] != DBNull.Value)
                        row3["C/F Value"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["CFVALUESUM"].ToString()));
                }
                if (ViewState["Premium"].ToString() == "chk")
                {
                    if (dt1.Rows[0]["PRMSUM"] != DBNull.Value)
                        row3["Premium"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["PRMSUM"].ToString()));
                }
                if (ViewState["Mtm"].ToString() == "chk")
                {
                    if (dt1.Rows[0]["MTMSUM"] != DBNull.Value)
                        row3["Mtm"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["MTMSUM"].ToString()));
                }
                if (ViewState["Future FinSett"].ToString() == "chk")
                {
                    if (dt1.Rows[0]["FINSETTSUM"] != DBNull.Value)
                        row3["Future FinSett"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["FINSETTSUM"].ToString()));
                }
                if (ViewState["ASN/EXC Amount"].ToString() == "chk")
                {
                    if (dt1.Rows[0]["EXCASNVALUESUM"] != DBNull.Value)
                        row3["ASN/EXC Amount"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["EXCASNVALUESUM"].ToString()));
                }
                if (ViewState["Net Obligation"].ToString() == "chk")
                {
                    if (dt1.Rows[0]["NETOBLIGATIONSUM"] != DBNull.Value)
                        row3["Net Obligation"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["NETOBLIGATIONSUM"].ToString()));
                }
                dtExport.Rows.Add(row3);


            }
            ViewState["dtExport"] = dtExport;
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
            if (ddlrptview.SelectedItem.Value.ToString().Trim() == "0")
            {
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

            }
            else
            {
                if (ddlGroup.SelectedItem.Value.ToString() == "1")
                {
                    str = "[Share Wise]";
                }

                if (ddldate.SelectedItem.Value.ToString() == "0")
                {
                    str = str + "Report For " + oconverter.ArrangeDate2(dtfor.Value.ToString());
                }
                else
                {
                    str = str + "Report Period " + oconverter.ArrangeDate2(dtfrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtto.Value.ToString());
                }
            }
            DrRowR1[0] = "Net Position Report:" + str;

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
                objExcel.ExportToExcelforExcel(dtExport, "Net Position", "Total :", dtReportHeader, dtReportFooter);
            }
            if (cmbExport.SelectedItem.Value == "P")
            {
                DataSet dsExportPdf = new DataSet();
                dsExportPdf.Tables.Add(dtExport);
                ExportToPDF(dsExportPdf);
                //objExcel.ExportToExcelforExcel(dtExport, "Net Position", "Total :", dtReportHeader, dtReportFooter);
            }
        }
        void export_OpenExposure(DataSet ds)
        {
            dtExport = ds.Tables[0].Copy();
            DataTable dtReportHeader = new DataTable();
            DataTable dtReportFooter = new DataTable();

            DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            HeaderRow[0] = CompanyName.Rows[0][0].ToString();
            dtReportHeader.Rows.Add(HeaderRow);
            DataRow DrRowR1 = dtReportHeader.NewRow();

            string str = null;
            if (ddldate.SelectedItem.Value.ToString() == "0")
            {
                str = " Report For " + oconverter.ArrangeDate2(dtfor.Value.ToString());
            }
            else
            {
                str = " Report Period " + oconverter.ArrangeDate2(dtfrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtto.Value.ToString());
            }
            DrRowR1[0] = "Net Position Report:Only Open Position With Exposure ;" + str;

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
                objExcel.ExportToExcelforExcel(dtExport, "Net Position", "Total :", dtReportHeader, dtReportFooter);
            }
            if (cmbExport.SelectedItem.Value == "P")
            {
                objExcel.ExportToExcelforExcel(dtExport, "Net Position", "Total :", dtReportHeader, dtReportFooter);
            }
        }
        protected void btnmailsend_Click(object sender, EventArgs e)
        {
            procedure();
            ds = (DataSet)ViewState["dataset"];
            if (ds.Tables[0].Rows.Count > 0)
            {
                selectionchkboxlist();
                mail();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(1);", true);
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
            if (ddlrptview.SelectedItem.Value.ToString().Trim() == "0")
            {
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
            }
            else
            {
                optionforemailshare();
            }
        }
        void clientwiseemail()
        {
            string Email_Subject = null;
            oGenericMethod = new GenericMethod();
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
                    Email_Subject = "[ " + Session["Segmentname"].ToString() + " ][ " + oGenericMethod.GetClientInfo(cmbclient.Items[k].Value, 1).Rows[0][2].ToString().Trim() + " ] Net Position [" + ViewState["billdate"].ToString().Trim() + "]";
                    htmltable_ClientWise(cmbclient.Items[k].Value, cmbclient.Items[k].Text.ToString().Trim(), cmbgroup.Items[j].Value.ToString().Trim(), dt.Rows[0]["GRPNAME"].ToString().Trim());
                    if (oDBEngine.SendReport(ViewState["mail"].ToString().Trim(), cmbclient.Items[k].Value, ViewState["billdate"].ToString().Trim(), Email_Subject) == true)
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(4);", true);
            }
            if (ViewState["mailsendresult"].ToString().Trim() == "success")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(2);", true);
            }
            if (ViewState["mailsendresult"].ToString().Trim() == "errorsuccess")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(3);", true);
            }
        }
        void branhgroupemail()
        {
            ViewState["GRPmail"] = "mail";
            ViewState["mailsendresult"] = "no";
            ds = (DataSet)ViewState["dataset"];
            DataView viewgroup = new DataView(ds.Tables[0]);
            dtgroupcontactid = viewgroup.ToTable(true, new string[] { "GRPID", "EMAIL", "GRPNAME" });
            if (dtgroupcontactid.Rows.Count > 0)
            {
                cmbgroup.DataSource = dtgroupcontactid;
                cmbgroup.DataValueField = "GRPID";
                cmbgroup.DataTextField = "EMAIL";
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
                    htmltable_ClientWise(cmbclient.Items[k].Value, cmbclient.Items[k].Text.ToString().Trim(), cmbgroup.Items[j].Value.ToString().Trim(), dt.Rows[0]["GRPNAME"].ToString().Trim());
                    if (ViewState["GRPmail"].ToString().Trim() == "mail")
                    {
                        ViewState["GRPmail"] = ViewState["mail"].ToString().Trim();
                    }
                    else
                    {
                        ViewState["GRPmail"] = ViewState["GRPmail"].ToString().Trim() + ViewState["mail"].ToString().Trim();
                    }

                }
                if (oDBEngine.SendReportBr(ViewState["GRPmail"].ToString().Trim(), cmbgroup.Items[j].Text.ToString().Trim(), ViewState["billdate"].ToString().Trim(), "Net Postion [" + ViewState["billdate"].ToString().Trim() + "]", cmbgroup.Items[j].Value) == true)
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(4);", true);
            }
            if (ViewState["mailsendresult"].ToString().Trim() == "success")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(2);", true);
            }
            if (ViewState["mailsendresult"].ToString().Trim() == "errorsuccess")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(3);", true);
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
                dtgroupcontactid = viewgroup.ToTable(true, new string[] { "GRPID", "EMAIL", "GRPNAME" });
                if (dtgroupcontactid.Rows.Count > 0)
                {
                    cmbgroup.DataSource = dtgroupcontactid;
                    cmbgroup.DataValueField = "GRPID";
                    cmbgroup.DataTextField = "EMAIL";
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
                        htmltable_ClientWise(cmbclient.Items[k].Value, cmbclient.Items[k].Text.ToString().Trim(), cmbgroup.Items[j].Value.ToString().Trim(), dt.Rows[0]["GRPNAME"].ToString().Trim());
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
                    if (oDBEngine.SendReportSt(ViewState["Usermail"].ToString().Trim(), clnt[i].ToString().Trim(), ViewState["billdate"].ToString().Trim(), "[ " + Session["Segmentname"].ToString() + " ] Net Position [" + ViewState["billdate"].ToString().Trim() + "]") == true)
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
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(4);", true);
                }
                if (ViewState["mailsendresult"].ToString().Trim() == "success")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(2);", true);
                }
                if (ViewState["mailsendresult"].ToString().Trim() == "errorsuccess")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(3);", true);
                }
            }
        }
        void optionforemailshare()
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
                dtgroupcontactid = viewgroup.ToTable(true, new string[] { "PRODUCTID", "SYMBOL" });
                if (dtgroupcontactid.Rows.Count > 0)
                {
                    cmbgroup.DataSource = dtgroupcontactid;
                    cmbgroup.DataValueField = "PRODUCTID";
                    cmbgroup.DataTextField = "SYMBOL";
                    cmbgroup.DataBind();

                }
                for (int j = 0; j < cmbgroup.Items.Count; j++)
                {
                    htmltable_ShareWise(cmbgroup.Items[j].Value.ToString().Trim(), cmbgroup.Items[j].Text.ToString().Trim());
                    if (ViewState["GRPmail"].ToString().Trim() == "mail")
                    {
                        ViewState["GRPmail"] = ViewState["mail"].ToString().Trim();
                    }
                    else
                    {
                        ViewState["GRPmail"] = ViewState["GRPmail"].ToString().Trim() + ViewState["mail"].ToString().Trim();
                    }
                }
                string[] clnt = HiddenField_emmail.Value.ToString().Split(',');
                int kk = clnt.Length;
                for (int i = 0; i < clnt.Length; i++)
                {
                    if (oDBEngine.SendReportSt(ViewState["GRPmail"].ToString().Trim(), clnt[i].ToString().Trim(), ViewState["billdate"].ToString().Trim(), "[ " + Session["Segmentname"].ToString() + " ] Net Position [" + ViewState["billdate"].ToString().Trim() + "]") == true)
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
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(4);", true);
                }
                if (ViewState["mailsendresult"].ToString().Trim() == "success")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(2);", true);
                }
                if (ViewState["mailsendresult"].ToString().Trim() == "errorsuccess")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(3);", true);
                }
            }
        }

        protected void ExportToPDF(DataSet DsNetPosition)
        {
            //====Start For Logo Add==============
            byte[] logoinByte;
            DataTable DtLogo = new DataTable();
            DtLogo.Columns.Add("Image", System.Type.GetType("System.Byte[]"));

            if (oconverter.getLogoImage(HttpContext.Current.Server.MapPath(@"..\images\logo.jpg"), out logoinByte) != 1)
            {
                ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "logo", "alert('Logo not Found.');", true);
            }
            else
            {
                DtLogo.Rows.Add(logoinByte);
            }
            DtLogo.AcceptChanges();
            DataSet DsLogo = new DataSet();
            DsLogo.Tables.Add(DtLogo);

            //======Start Company Detail=================
            DataTable dtCompany;
            if (ddldate.SelectedItem.Value == "0")
            {
                dtCompany = oDBEngine.GetDataTable(@"Declare @cmpname varchar(500),@cmpaddress varchar(max),@cmpPhone varchar(500),@cmpStaxRegNo varchar(50),@cmpPan varchar(20),@cmpHoldingStatementAsAt varchar(200) 
                                                            Select @cmpname=isnull(cmp_Name,'') +' [ ' +isnull(exch_TMCode,'')+' ]',
	                                                               @cmpaddress=isnull(add_address1,'')+' '+isnull(add_address2,'')+' '+isnull(add_address3,'')+', '+isnull(city_name,'')+'-'+ isnull(add_pin,''),
	                                                               @cmpStaxRegNo=isnull(cmp_serviceTaxNo,''),
                                                                   @cmpPan=isnull(cmp_panNo,''),
                                                                   @cmpHoldingStatementAsAt='Net Position MTM Report For " + oconverter.ArrangeDate2(dtfor.Value.ToString()) + ' ' + '[' + Session["segmentname"].ToString() + ']' + "',@cmpPhone=isnull((Select phf_countryCode+'-'+phf_areaCode+'-'+phf_phoneNumber from tbl_master_phonefax where phf_cntId='" + Session["LastCompany"].ToString() + "' and phf_type='Office'),'') From  tbl_master_address,tbl_master_company,tbl_master_companyExchange,tbl_master_city Where exch_compId=cmp_internalid and cmp_internalid=add_cntId and add_city=city_id and exch_compId='" + Session["LastCompany"].ToString() + "' Select @cmpname cmp_Name,@cmpaddress cmp_Address,@cmpPan cmp_Pan,@cmpPhone cmp_Phone,@cmpStaxRegNo cmp_ServTaxRegNo,@cmpHoldingStatementAsAt as AsAtHoldingDate");

            }
            else
            {
                dtCompany = oDBEngine.GetDataTable(@"Declare @cmpname varchar(500),@cmpaddress varchar(max),@cmpPhone varchar(500),@cmpStaxRegNo varchar(50),@cmpPan varchar(20),@cmpHoldingStatementAsAt varchar(200) 
                                                            Select @cmpname=isnull(cmp_Name,'') +' [ ' +isnull(exch_TMCode,'')+' ]',
	                                                               @cmpaddress=isnull(add_address1,'')+' '+isnull(add_address2,'')+' '+isnull(add_address3,'')+', '+isnull(city_name,'')+'-'+ isnull(add_pin,''),
	                                                               @cmpStaxRegNo=isnull(cmp_serviceTaxNo,''),
                                                                   @cmpPan=isnull(cmp_panNo,''),
                                                                   @cmpHoldingStatementAsAt='Net Position MTM Report From " + oconverter.ArrangeDate2(dtfrom.Value.ToString()) + " To " + oconverter.ArrangeDate2(dtto.Value.ToString()) + ' ' + '[' + Session["segmentname"].ToString() + ']' + "',@cmpPhone=isnull((Select phf_countryCode+'-'+phf_areaCode+'-'+phf_phoneNumber from tbl_master_phonefax where phf_cntId='" + Session["LastCompany"].ToString() + "' and phf_type='Office'),'') From  tbl_master_address,tbl_master_company,tbl_master_companyExchange,tbl_master_city Where exch_compId=cmp_internalid and cmp_internalid=add_cntId and add_city=city_id and exch_compId='" + Session["LastCompany"].ToString() + "' Select @cmpname cmp_Name,@cmpaddress cmp_Address,@cmpPan cmp_Pan,@cmpPhone cmp_Phone,@cmpStaxRegNo cmp_ServTaxRegNo,@cmpHoldingStatementAsAt as AsAtHoldingDate");

            }
            DataSet DsCompany = new DataSet();
            DsCompany.Tables.Add(dtCompany);



            //======Generate XSD===========================            
            //DsNetPosition.Tables[0].WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NetPosMTM_FODetail.xsd");
            //DsCompany.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NetPosMTM_Company.xsd");
            //DsLogo.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NetPosMTM_CompanyLogo.xsd");

            ReportDocument NetPositionMTMReportDoc = new ReportDocument();
            string[] connPath = ConfigurationSettings.AppSettings["DBConnectionDefault"].Split(';');
            string path = Server.MapPath("..\\Reports\\NetPositionMTM_FO.rpt");
            NetPositionMTMReportDoc.Load(path);
            NetPositionMTMReportDoc.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
            NetPositionMTMReportDoc.SetDataSource(DsNetPosition.Tables[0]);
            NetPositionMTMReportDoc.Subreports["CompanyDetail"].SetDataSource(DsCompany.Tables[0]);
            NetPositionMTMReportDoc.Subreports["CompanyLogo"].SetDataSource(DsLogo.Tables[0]);
            //NetPositionMTMReportDoc.SetParameterValue("@SegmentName", (object)Session["Segmentname"].ToString().Split('-')[1]);
            NetPositionMTMReportDoc.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, true, "Net Position MTM");
            DsNetPosition.Dispose();
            DsCompany.Clear();
            DsLogo.Dispose();
        }

    }
}