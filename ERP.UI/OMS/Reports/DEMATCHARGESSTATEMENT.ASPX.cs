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

    public partial class Reports_DEMATCHARGESSTATEMENT : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        BusinessLogicLayer.Reports rep = new BusinessLogicLayer.Reports();
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        static string Group;
        static string Branch;
        static string Clients;
        static DataSet ds = new DataSet();
        string data;
        static DataTable Distinctbranch = new DataTable();
        DataTable dtReportHeader = new DataTable();
        DataTable dtReportFooter = new DataTable();
        ExcelFile objExcel = new ExcelFile();
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
                Clients = null;
                Group = null;
                Branch = null;

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
        void date()
        {
            dtFrom.EditFormatString = oconverter.GetDateFormat("Date");
            dtTo.EditFormatString = oconverter.GetDateFormat("Date");
            string[] idlist = oDBEngine.GetDate().GetDateTimeFormats();
            dtFrom.Value = Convert.ToDateTime(idlist[2]);
            dtTo.Value = Convert.ToDateTime(idlist[2]);
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
                Clients = str;
                data = "Clients~" + str1;
            }
            else if (idlist[0] == "Group")
            {
                Group = str;
                data = "Group~" + str;
            }
            else if (idlist[0] == "Branch")
            {
                Branch = str;
                data = "Branch~" + str;
            }
        }
        protected void btnhide_Click(object sender, EventArgs e)
        {
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                if (Group == null)
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
                        dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%' and cnt_internalid in(select grp_contactid from tbl_trans_group where grp_groupmaster in(" + Group + "))");
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
                        dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%' and cnt_branchid in(" + Branch + ")");
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
                        dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%' and cnt_internalid in (select dpd_cntID from tbl_master_contactdpdetails where dpd_POA=1) and cnt_internalid in(select grp_contactid from tbl_trans_group where grp_groupmaster in(" + Group + "))");
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
                        dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalId like 'CL%' and cnt_internalid in (select dpd_cntID from tbl_master_contactdpdetails where dpd_POA=1) and cnt_branchid in(" + Branch + ")");
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
        }
        protected void btn_show_Click(object sender, EventArgs e)
        {
            fn_Client();
            if (Clients == null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD", "NORECORD();", true);
            }
            else
            {

                procedure();
            }
        }
        void procedure()
        {
            string GEOUPBRANCH = "";
            string grouptype = "";
            if (ddlGroup.SelectedItem.Value.ToString() == "0")
            {
                GEOUPBRANCH = "BRANCH";

            }
            else
            {
                GEOUPBRANCH = "GROUP";

            }
            if (ddlType.SelectedItem.Value.ToString() == "0")
            {
                grouptype = "NA";
            }
            else
            {
                if (ddlGroup.SelectedItem.Value.ToString() == "0")
                {
                    grouptype = "NA";
                }
                else
                {
                    grouptype = ddlgrouptype.SelectedItem.Value.ToString();
                }
            }

            ds = rep.Sp_DematChargesStatement(Session["usersegid"].ToString(), Session["LastCompany"].ToString(), dtFrom.Value.ToString(),
                dtTo.Value.ToString(), HttpContext.Current.Session["ExchangeSegmentID"].ToString(), Clients, HttpContext.Current.Session["LastFinYear"].ToString(),
                ddlType.SelectedItem.Value.ToString(), GEOUPBRANCH, grouptype);
            ViewState["dataset"] = ds;
            //FnGenerateType(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ddlgeneration.SelectedItem.Value != "email")
                {

                    CurrentPage = 0;
                    DDLBIND();
                    bind_Details();
                }
                else
                {
                    string Date = oconverter.ArrangeDate2(dtFrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtTo.Value.ToString());
                    if (ddlType.SelectedItem.Value == "0")
                    {
                        EmailClientWise(ds, Date);
                    }
                    else
                    {
                        EmailGroupBranchWise(ds, Date);
                    }
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD", "NORECORD();", true);
            }

        }
        //void FnGenerateType(DataSet ds)
        //{

        //}
        void DDLBIND()
        {
            DataView viewgroup = new DataView(ds.Tables[0]);


            if (ddlType.SelectedItem.Value.ToString() == "0")
            {
                Distinctbranch = viewgroup.ToTable(true, new string[] { "CustomerID", "Name" });
                foreach (DataRow dr in Distinctbranch.Rows)
                {
                    if (Distinctbranch.Rows.Count > 0)
                    {
                        cmbdistinct.Items.Clear();
                        cmbdistinct.DataSource = Distinctbranch;
                        cmbdistinct.DataValueField = "CustomerID";
                        cmbdistinct.DataTextField = "Name";
                        cmbdistinct.DataBind();

                    }
                }
            }
            else
            {
                Distinctbranch = viewgroup.ToTable(true, new string[] { "GRPID", "GRPCODE" });
                foreach (DataRow dr in Distinctbranch.Rows)
                {
                    if (Distinctbranch.Rows.Count > 0)
                    {
                        cmbdistinct.Items.Clear();
                        cmbdistinct.DataSource = Distinctbranch;
                        cmbdistinct.DataValueField = "GRPID";
                        cmbdistinct.DataTextField = "GRPCODE";
                        cmbdistinct.DataBind();

                    }
                }
            }
            LastPage = Distinctbranch.Rows.Count - 1;
        }
        void bind_Details()
        {
            cmbdistinct.SelectedIndex = CurrentPage;
            string SpanText = null;
            if (LastPage > -1)
            {
                listRecord.Text = CurrentPage + 1 + " of " + Distinctbranch.Rows.Count.ToString() + " Record.";

            }
            if (ddlType.SelectedItem.Value.ToString() == "0")
            {
                fn_clientwiseresult();
                SpanText = "[Client Wise] " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + " To " + oconverter.ArrangeDate2(dtTo.Value.ToString());
            }
            else
            {
                fn_groupwiseresult();
                SpanText = "[Branch/Group + Client Wise] " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + " To " + oconverter.ArrangeDate2(dtTo.Value.ToString());
            }


            ScriptManager.RegisterStartupScript(this, this.GetType(), "Display", "Display('" + SpanText + "');", true);
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
        void FnHtml(DataSet ds, string parameter)
        {
            String strHtml = String.Empty;
            strHtml = "<table width=\"100%\"  border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            int flag = 0;

            decimal amount = decimal.Zero;
            decimal netdemat = decimal.Zero;



            ////////////////////CLIENT HEADER//////////////////////////////////////
            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap\">Trf Date</td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap\">Ref.ID</td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap\">Type</td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap\">Scrip</td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap\">ISIN</td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap\">Quantity</td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap\">From A/c</td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap\">Sett.No From</td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap\">To A/c</td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap\">Sett.No To</td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap\">Rate</td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap\">Amount</td></tr>";

            DataView viewclient = new DataView();
            viewclient = ds.Tables[0].DefaultView;
            ////if (ddlType.SelectedItem.Value == "0")
            ////{
            viewclient.RowFilter = "(CustomerId='" + parameter.ToString().Trim() + "' and CustomerId is not null and Customerid<>'ZZZZZZZZZZZ')";
            //}
            //else
            //{
            //    viewclient.RowFilter = "(GRPID='" + parameter.ToString().Trim() + "' and GRPID is not null )";
            //}
            DataTable dt = new DataTable();
            dt = viewclient.ToTable();

            //////////////////////////////DETAILS 
            strHtml += "<tr>";
            strHtml += "<td colspan=12>" + dt.Rows[0]["Name"] + "</td></tr>";

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                flag = flag + 1;
                strHtml += "<tr style=\"background-color: " + GetRowColor(flag) + " ;text-align:left\">";

                if (dt.Rows[i]["TransactionDate"] != DBNull.Value)
                {
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\">" + dt.Rows[i]["TransactionDate"].ToString() + "</td>";
                }
                else
                {
                    strHtml += "<td style=\"font-size:xx-small;\" >&nbsp;</td>";
                }
                if (dt.Rows[i]["TransactionId"] != DBNull.Value)
                {
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\">" + dt.Rows[i]["TransactionId"].ToString() + "</td>";
                }
                else
                {
                    strHtml += "<td style=\"font-size:xx-small;\" >&nbsp;</td>";
                }
                if (dt.Rows[i]["TransactionType"] != DBNull.Value)
                {
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\">" + dt.Rows[i]["TransactionType"].ToString() + "</td>";
                }
                else
                {
                    strHtml += "<td style=\"font-size:xx-small;\" >&nbsp;</td>";
                }
                if (dt.Rows[i]["Scrip"] != DBNull.Value)
                {
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\">" + dt.Rows[i]["Scrip"].ToString() + "</td>";
                }
                else
                {
                    strHtml += "<td style=\"font-size:xx-small;\" >&nbsp;</td>";
                }
                if (dt.Rows[i]["ISIN"] != DBNull.Value)
                {
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\">" + dt.Rows[i]["ISIN"].ToString() + "</td>";
                }
                else
                {
                    strHtml += "<td style=\"font-size:xx-small;\" >&nbsp;</td>";
                }
                if (dt.Rows[i]["Quantity"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dt.Rows[i]["Quantity"].ToString())) + "</td>";
                }
                else
                {
                    strHtml += "<td style=\"font-size:xx-small;\" >&nbsp;</td>";
                }
                if (dt.Rows[i]["FROMACNAME"] != DBNull.Value)
                {
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\">" + dt.Rows[i]["FROMACNAME"].ToString() + "</td>";
                }
                else
                {
                    strHtml += "<td style=\"font-size:xx-small;\" >&nbsp;</td>";

                }
                if (dt.Rows[i]["SettlementNumberS"] != DBNull.Value)
                {
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\">" + dt.Rows[i]["SettlementNumberS"].ToString() + "</td>";
                }
                else
                {
                    strHtml += "<td style=\"font-size:xx-small;\" >&nbsp;</td>";
                }
                if (dt.Rows[i]["TOACNAME"] != DBNull.Value)
                {
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\">" + dt.Rows[i]["TOACNAME"].ToString() + "</td>";
                }
                else
                {
                    strHtml += "<td style=\"font-size:xx-small;\" >&nbsp;</td>";
                }
                if (dt.Rows[i]["SettlementNumberT"] != DBNull.Value)
                {
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\">" + dt.Rows[i]["SettlementNumberT"].ToString() + "</td>";
                }
                else
                {
                    strHtml += "<td style=\"font-size:xx-small;\" >&nbsp;</td>";
                }
                if (dt.Rows[i]["RATE"].ToString().Trim() != "")
                {
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[i]["RATE"].ToString())) + "</td>";
                }
                else
                {
                    strHtml += "<td style=\"font-size:xx-small;\" >&nbsp;</td>";
                }
                if (dt.Rows[i]["AMOUNT"].ToString().Trim() != "")
                {
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dt.Rows[i]["AMOUNT"].ToString())) + "</td>";
                    amount = amount + Convert.ToDecimal(dt.Rows[i]["AMOUNT"].ToString());
                }
                else
                {
                    strHtml += "<td style=\"font-size:xx-small;\" >&nbsp;</td>";
                }
                strHtml += "</tr>";

            }

            strHtml += "<tr style=\"background-color:White;\">";
            strHtml += "<td align=right colspan=12><div style=\"background-color:lavender;width:30%;\">";
            strHtml += "<table width=\"100%\" border=" + Convert.ToChar(34) + 0 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtml += "<tr><td align=left style=\"width:10%;\">Total :</td>";
            strHtml += "<td align=right style=\"width:20%;\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(amount)) + "</td></tr>";
            if (dt.Rows[0]["ServiceTax"] != DBNull.Value)
            {
                strHtml += "<tr><td align=left style=\"width:10%;\">Service Tax :</td>";
                strHtml += "<td align=right style=\"width:20%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[0]["ServiceTax"].ToString())) + "</td></tr>";
                netdemat = Convert.ToDecimal(dt.Rows[0]["ServiceTax"].ToString());
            }
            if (dt.Rows[0]["EduCess"] != DBNull.Value)
            {
                strHtml += "<tr><td align=left style=\"width:10%;\">Edu. Cess:</td>";
                strHtml += "<td align=right style=\"width:20%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[0]["EduCess"].ToString())) + "</td></tr>";
                netdemat = netdemat + Convert.ToDecimal(dt.Rows[0]["EduCess"].ToString());
            }
            if (dt.Rows[0]["HgrEduCess"] != DBNull.Value)
            {
                strHtml += "<tr><td align=left style=\"width:10%;\">Hgr Edu Cess:</td>";
                strHtml += "<td align=right style=\"width:20%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[0]["HgrEduCess"].ToString())) + "</td></tr>";
                netdemat = netdemat + Convert.ToDecimal(dt.Rows[0]["HgrEduCess"].ToString());

            }
            if (netdemat != 0)
            {
                strHtml += "<tr><td align=left style=\"width:10%;\">Net Demat Charges:</td>";
                strHtml += "<td align=right style=\"width:20%;\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(netdemat + amount)) + "</td></tr>";
            }
            strHtml += "</table></div></td></tr>";

            display.InnerHtml = strHtml;
            strHtml = strHtml + "</table>";
            ViewState["mail"] = strHtml;

        }

        void fn_clientwiseresult()
        {
            String strHtml = String.Empty;
            strHtml = "<table width=\"100%\"  border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            int flag = 0;

            decimal amount = decimal.Zero;
            decimal netdemat = decimal.Zero;



            ////////////////////CLIENT HEADER//////////////////////////////////////
            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap\">Trf Date</td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap\">Ref.ID</td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap\">Type</td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap\">Scrip</td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap\">ISIN</td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap\">Quantity</td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap\">From A/c</td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap\">Sett.No From</td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap\">To A/c</td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap\">Sett.No To</td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap\">Rate</td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap\">Amount</td></tr>";

            DataView viewclient = new DataView();
            viewclient = ds.Tables[0].DefaultView;
            viewclient.RowFilter = "CustomerID='" + cmbdistinct.SelectedItem.Value + "'";
            DataTable dt = new DataTable();
            dt = viewclient.ToTable();

            //////////////////////////////DETAILS 
            strHtml += "<tr>";
            strHtml += "<td colspan=12>" + dt.Rows[0]["Name"] + "</td></tr>";

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                flag = flag + 1;
                strHtml += "<tr style=\"background-color: " + GetRowColor(flag) + " ;text-align:left\">";

                if (dt.Rows[i]["TransactionDate"] != DBNull.Value)
                {
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\">" + dt.Rows[i]["TransactionDate"].ToString() + "</td>";
                }
                else
                {
                    strHtml += "<td style=\"font-size:xx-small;\" >&nbsp;</td>";
                }
                if (dt.Rows[i]["TransactionId"] != DBNull.Value)
                {
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\">" + dt.Rows[i]["TransactionId"].ToString() + "</td>";
                }
                else
                {
                    strHtml += "<td style=\"font-size:xx-small;\" >&nbsp;</td>";
                }
                if (dt.Rows[i]["TransactionType"] != DBNull.Value)
                {
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\">" + dt.Rows[i]["TransactionType"].ToString() + "</td>";
                }
                else
                {
                    strHtml += "<td style=\"font-size:xx-small;\" >&nbsp;</td>";
                }
                if (dt.Rows[i]["Scrip"] != DBNull.Value)
                {
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\">" + dt.Rows[i]["Scrip"].ToString() + "</td>";
                }
                else
                {
                    strHtml += "<td style=\"font-size:xx-small;\" >&nbsp;</td>";
                }
                if (dt.Rows[i]["ISIN"] != DBNull.Value)
                {
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\">" + dt.Rows[i]["ISIN"].ToString() + "</td>";
                }
                else
                {
                    strHtml += "<td style=\"font-size:xx-small;\" >&nbsp;</td>";
                }
                if (dt.Rows[i]["Quantity"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dt.Rows[i]["Quantity"].ToString())) + "</td>";
                }
                else
                {
                    strHtml += "<td style=\"font-size:xx-small;\" >&nbsp;</td>";
                }
                if (dt.Rows[i]["FROMACNAME"] != DBNull.Value)
                {
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\">" + dt.Rows[i]["FROMACNAME"].ToString() + "</td>";
                }
                else
                {
                    strHtml += "<td style=\"font-size:xx-small;\" >&nbsp;</td>";

                }
                if (dt.Rows[i]["SettlementNumberS"] != DBNull.Value)
                {
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\">" + dt.Rows[i]["SettlementNumberS"].ToString() + "</td>";
                }
                else
                {
                    strHtml += "<td style=\"font-size:xx-small;\" >&nbsp;</td>";
                }
                if (dt.Rows[i]["TOACNAME"] != DBNull.Value)
                {
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\">" + dt.Rows[i]["TOACNAME"].ToString() + "</td>";
                }
                else
                {
                    strHtml += "<td style=\"font-size:xx-small;\" >&nbsp;</td>";
                }
                if (dt.Rows[i]["SettlementNumberT"] != DBNull.Value)
                {
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\">" + dt.Rows[i]["SettlementNumberT"].ToString() + "</td>";
                }
                else
                {
                    strHtml += "<td style=\"font-size:xx-small;\" >&nbsp;</td>";
                }
                if (dt.Rows[i]["RATE"].ToString().Trim() != "")
                {
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[i]["RATE"].ToString())) + "</td>";
                }
                else
                {
                    strHtml += "<td style=\"font-size:xx-small;\" >&nbsp;</td>";
                }
                if (dt.Rows[i]["AMOUNT"].ToString().Trim() != "")
                {
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dt.Rows[i]["AMOUNT"].ToString())) + "</td>";
                    amount = amount + Convert.ToDecimal(dt.Rows[i]["AMOUNT"].ToString());
                }
                else
                {
                    strHtml += "<td style=\"font-size:xx-small;\" >&nbsp;</td>";
                }
                strHtml += "</tr>";

            }

            strHtml += "<tr style=\"background-color:White;\">";
            strHtml += "<td align=right colspan=12><div style=\"background-color:lavender;width:30%;\">";
            strHtml += "<table width=\"100%\" border=" + Convert.ToChar(34) + 0 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtml += "<tr><td align=left style=\"width:10%;\">Total :</td>";
            strHtml += "<td align=right style=\"width:20%;\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(amount)) + "</td></tr>";
            if (dt.Rows[0]["ServiceTax"] != DBNull.Value)
            {
                strHtml += "<tr><td align=left style=\"width:10%;\">Service Tax :</td>";
                strHtml += "<td align=right style=\"width:20%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[0]["ServiceTax"].ToString())) + "</td></tr>";
                netdemat = Convert.ToDecimal(dt.Rows[0]["ServiceTax"].ToString());
            }
            if (dt.Rows[0]["EduCess"] != DBNull.Value)
            {
                strHtml += "<tr><td align=left style=\"width:10%;\">Edu. Cess:</td>";
                strHtml += "<td align=right style=\"width:20%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[0]["EduCess"].ToString())) + "</td></tr>";
                netdemat = netdemat + Convert.ToDecimal(dt.Rows[0]["EduCess"].ToString());
            }
            if (dt.Rows[0]["HgrEduCess"] != DBNull.Value)
            {
                strHtml += "<tr><td align=left style=\"width:10%;\">Hgr Edu Cess:</td>";
                strHtml += "<td align=right style=\"width:20%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[0]["HgrEduCess"].ToString())) + "</td></tr>";
                netdemat = netdemat + Convert.ToDecimal(dt.Rows[0]["HgrEduCess"].ToString());

            }
            if (netdemat != 0)
            {
                strHtml += "<tr><td align=left style=\"width:10%;\">Net Demat Charges:</td>";
                strHtml += "<td align=right style=\"width:20%;\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(netdemat + amount)) + "</td></tr>";
            }
            strHtml += "</table></div></td></tr>";

            display.InnerHtml = strHtml;


        }
        protected string GetRowColor(int i)
        {
            if (i++ % 2 == 0)
                return "#fff0f5";
            else
                return "White";
        }
        void fn_groupwiseresult()
        {
            String strHtml = String.Empty;
            strHtml = "<table width=\"100%\"  border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            int flag = 0;

            string clientid = null;
            decimal amount = decimal.Zero;
            decimal netdemat = decimal.Zero;

            decimal servicetax = decimal.Zero;
            decimal educess = decimal.Zero;
            decimal hgreducess = decimal.Zero;
            ////////////////////CLIENT HEADER//////////////////////////////////////
            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap\">Trf Date</td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap\">Ref.ID</td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap\">Type</td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap\">Scrip</td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap\">ISIN</td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap\">Quantity</td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap\">From A/c</td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap\">Sett. No</td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap\">To A/c</td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap\">Sett. No</td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap\">Rate</td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap\">Amount</td></tr>";

            DataView viewclient = new DataView();
            viewclient = ds.Tables[0].DefaultView;
            viewclient.RowFilter = "GRPID='" + cmbdistinct.SelectedItem.Value + "'";
            DataTable dt = new DataTable();
            dt = viewclient.ToTable();

            //////////////////////////////DETAILS 

            strHtml += "<tr>";
            strHtml += "<td colspan=12><b>" + dt.Rows[0]["GRPCODE"] + "</b></td></tr>";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                flag = flag + 1;
                if (clientid != dt.Rows[i]["CustomerID"].ToString())
                {
                    if (amount != 0)
                    {
                        if (clientid != dt.Rows[i]["CustomerID"].ToString())
                        {
                            strHtml += "<tr style=\"background-color:White;\">";
                            strHtml += "<td align=right colspan=12><div style=\"background-color:lavender;width:30%;\">";
                            strHtml += "<table width=\"100%\" border=" + Convert.ToChar(34) + 0 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                            strHtml += "<tr><td align=left style=\"width:10%;\">Total :</td>";
                            strHtml += "<td align=right style=\"width:20%;\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(amount)) + "</td></tr>";
                            if (dt.Rows[i - 1]["ServiceTax"] != DBNull.Value)
                            {
                                strHtml += "<tr><td align=left style=\"width:10%;\">Service Tax :</td>";
                                strHtml += "<td align=right style=\"width:20%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[i - 1]["ServiceTax"].ToString())) + "</td></tr>";
                                netdemat = Convert.ToDecimal(dt.Rows[i]["ServiceTax"].ToString());
                            }
                            if (dt.Rows[i - 1]["EduCess"] != DBNull.Value)
                            {
                                strHtml += "<tr><td align=left style=\"width:10%;\">Edu. Cess:</td>";
                                strHtml += "<td align=right style=\"width:20%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[i - 1]["EduCess"].ToString())) + "</td></tr>";
                                netdemat = netdemat + Convert.ToDecimal(dt.Rows[0]["EduCess"].ToString());
                            }
                            if (dt.Rows[i - 1]["HgrEduCess"] != DBNull.Value)
                            {
                                strHtml += "<tr><td align=left style=\"width:10%;\">Hgr Edu Cess:</td>";
                                strHtml += "<td align=right style=\"width:20%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[i - 1]["HgrEduCess"].ToString())) + "</td></tr>";
                                netdemat = netdemat + Convert.ToDecimal(dt.Rows[i]["HgrEduCess"].ToString());

                            }
                            if (netdemat != 0)
                            {
                                strHtml += "<tr><td align=left style=\"width:10%;\">Net Demat Charges:</td>";
                                strHtml += "<td align=right style=\"width:20%;\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(netdemat + amount)) + "</td></tr>";
                            }
                            strHtml += "</table></div></td></tr>";


                        }
                    }
                    clientid = dt.Rows[i]["CustomerID"].ToString();
                    strHtml += "<tr>";
                    strHtml += "<td colspan=12>" + dt.Rows[i]["Name"] + "</td></tr>";

                    amount = decimal.Zero;
                    netdemat = decimal.Zero;
                    servicetax = decimal.Zero;
                    educess = decimal.Zero;
                    hgreducess = decimal.Zero;
                }
                strHtml += "<tr style=\"background-color: " + GetRowColor(flag) + " ;text-align:left\">";

                if (dt.Rows[i]["TransactionDate"] != DBNull.Value)
                {
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\">" + dt.Rows[i]["TransactionDate"].ToString() + "</td>";
                }
                else
                {
                    strHtml += "<td style=\"font-size:xx-small;\" >&nbsp;</td>";
                }
                if (dt.Rows[i]["TransactionId"] != DBNull.Value)
                {
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\">" + dt.Rows[i]["TransactionId"].ToString() + "</td>";
                }
                else
                {
                    strHtml += "<td style=\"font-size:xx-small;\" >&nbsp;</td>";
                }
                if (dt.Rows[i]["TransactionType"] != DBNull.Value)
                {
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\">" + dt.Rows[i]["TransactionType"].ToString() + "</td>";
                }
                else
                {
                    strHtml += "<td style=\"font-size:xx-small;\" >&nbsp;</td>";
                }
                if (dt.Rows[i]["Scrip"] != DBNull.Value)
                {
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\">" + dt.Rows[i]["Scrip"].ToString() + "</td>";
                }
                else
                {
                    strHtml += "<td style=\"font-size:xx-small;\" >&nbsp;</td>";
                }
                if (dt.Rows[i]["ISIN"] != DBNull.Value)
                {
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\">" + dt.Rows[i]["ISIN"].ToString() + "</td>";
                }
                else
                {
                    strHtml += "<td style=\"font-size:xx-small;\" >&nbsp;</td>";
                }
                if (dt.Rows[i]["Quantity"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dt.Rows[i]["Quantity"].ToString())) + "</td>";
                }
                else
                {
                    strHtml += "<td style=\"font-size:xx-small;\" >&nbsp;</td>";
                }
                if (dt.Rows[i]["FROMACNAME"] != DBNull.Value)
                {
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\">" + dt.Rows[i]["FROMACNAME"].ToString() + "</td>";
                }
                else
                {
                    strHtml += "<td style=\"font-size:xx-small;\" >&nbsp;</td>";

                }
                if (dt.Rows[i]["SettlementNumberS"] != DBNull.Value)
                {
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\">" + dt.Rows[i]["SettlementNumberS"].ToString() + "</td>";
                }
                else
                {
                    strHtml += "<td style=\"font-size:xx-small;\" >&nbsp;</td>";
                }
                if (dt.Rows[i]["TOACNAME"] != DBNull.Value)
                {
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\">" + dt.Rows[i]["TOACNAME"].ToString() + "</td>";
                }
                else
                {
                    strHtml += "<td style=\"font-size:xx-small;\" >&nbsp;</td>";
                }
                if (dt.Rows[i]["SettlementNumberT"] != DBNull.Value)
                {
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\">" + dt.Rows[i]["SettlementNumberT"].ToString() + "</td>";
                }
                else
                {
                    strHtml += "<td style=\"font-size:xx-small;\" >&nbsp;</td>";
                }
                if (dt.Rows[i]["RATE"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dt.Rows[i]["RATE"].ToString())) + "</td>";
                }
                else
                {
                    strHtml += "<td style=\"font-size:xx-small;\" >&nbsp;</td>";
                }
                if (dt.Rows[i]["AMOUNT"] != DBNull.Value)
                {
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dt.Rows[i]["AMOUNT"].ToString())) + "</td>";
                    amount = amount + Convert.ToDecimal(dt.Rows[i]["AMOUNT"].ToString());
                }
                else
                {
                    strHtml += "<td style=\"font-size:xx-small;\" >&nbsp;</td>";
                }
                strHtml += "</tr>";
                if (dt.Rows[i]["ServiceTax"] != DBNull.Value)
                {
                    servicetax = Convert.ToDecimal(dt.Rows[i]["ServiceTax"].ToString());
                }
                if (dt.Rows[i]["EduCess"] != DBNull.Value)
                {
                    educess = Convert.ToDecimal(dt.Rows[i]["EduCess"].ToString());
                }
                if (dt.Rows[i]["HgrEduCess"] != DBNull.Value)
                {
                    hgreducess = Convert.ToDecimal(dt.Rows[i]["HgrEduCess"].ToString());
                }

            }

            strHtml += "<tr style=\"background-color:White;\">";
            strHtml += "<td align=right colspan=12><div style=\"background-color:lavender;width:30%;\">";
            strHtml += "<table width=\"100%\" border=" + Convert.ToChar(34) + 0 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtml += "<tr><td align=left style=\"width:10%;\">Total :</td>";
            strHtml += "<td align=right style=\"width:20%;\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(amount)) + "</td></tr>";
            if (servicetax != 0)
            {
                strHtml += "<tr><td align=left style=\"width:10%;\">Service Tax :</td>";
                strHtml += "<td align=right style=\"width:20%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(servicetax)) + "</td></tr>";
                netdemat = Convert.ToDecimal(servicetax);
            }
            if (educess != 0)
            {
                strHtml += "<tr><td align=left style=\"width:10%;\">Edu. Cess:</td>";
                strHtml += "<td align=right style=\"width:20%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(educess)) + "</td></tr>";
                netdemat = netdemat + Convert.ToDecimal(educess);
            }
            if (hgreducess != 0)
            {
                strHtml += "<tr><td align=left style=\"width:10%;\">Hgr Edu Cess:</td>";
                strHtml += "<td align=right style=\"width:20%;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(hgreducess)) + "</td></tr>";
                netdemat = netdemat + Convert.ToDecimal(hgreducess);

            }
            if (netdemat != 0)
            {
                strHtml += "<tr><td align=left style=\"width:10%;\">Net Demat Charges:</td>";
                strHtml += "<td align=right style=\"width:20%;\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(netdemat + amount)) + "</td></tr>";
            }

            strHtml += "</table></div></td></tr>";
            strHtml += "</table>";
            display.InnerHtml = strHtml;
        }

        protected void ddlExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlType.SelectedItem.Value.ToString() == "0")
            {
                ExportClient();
            }
            else
            {
                ExportGroup();
            }

        }
        void ExportClient()
        {
            DataTable dtExport = new DataTable();
            dtExport.Columns.Add("Trf Date", Type.GetType("System.String"));
            dtExport.Columns.Add("Ref.ID", Type.GetType("System.String"));
            dtExport.Columns.Add("Type", Type.GetType("System.String"));
            dtExport.Columns.Add("Scrip", Type.GetType("System.String"));
            dtExport.Columns.Add("ISIN", Type.GetType("System.String"));
            dtExport.Columns.Add("Quantity", Type.GetType("System.String"));
            dtExport.Columns.Add("From A/c", Type.GetType("System.String"));
            dtExport.Columns.Add("Sett.No From", Type.GetType("System.String"));
            dtExport.Columns.Add("To A/c", Type.GetType("System.String"));
            dtExport.Columns.Add("Sett.No To", Type.GetType("System.String"));
            dtExport.Columns.Add("Rate", Type.GetType("System.String"));
            dtExport.Columns.Add("Amount", Type.GetType("System.String"));

            decimal amount = decimal.Zero;
            decimal srvtax = decimal.Zero;
            decimal educess = decimal.Zero;
            decimal hgrcess = decimal.Zero;
            decimal net = decimal.Zero;

            int cmbcount = cmbdistinct.Items.Count;
            for (int i = 0; i < cmbcount; i++)
            {
                DataView viewclient = new DataView();
                viewclient = ds.Tables[0].DefaultView;
                string valItem = cmbdistinct.Items[i].Value;
                viewclient.RowFilter = "CustomerID='" + valItem + "'";
                DataTable dt = new DataTable();
                dt = viewclient.ToTable();



                DataRow row1 = dtExport.NewRow();
                row1["Trf Date"] = dt.Rows[0]["Name"];
                row1["Ref.ID"] = "Test";
                dtExport.Rows.Add(row1);

                //DataRow rowblanck = dtExport.NewRow();
                //dtExport.Rows.Add(rowblanck);

                amount = decimal.Zero;
                srvtax = decimal.Zero;
                educess = decimal.Zero;
                hgrcess = decimal.Zero;
                net = decimal.Zero;
                foreach (DataRow dr1 in dt.Rows)
                {
                    DataRow row2 = dtExport.NewRow();
                    if (dr1["TransactionDate"] != DBNull.Value)
                    {
                        row2["Trf Date"] = dr1["TransactionDate"];
                    }
                    if (dr1["TransactionId"] != DBNull.Value)
                    {
                        row2["Ref.ID"] = dr1["TransactionId"];
                    }
                    if (dr1["TransactionType"] != DBNull.Value)
                    {
                        row2["Type"] = dr1["TransactionType"];
                    }
                    if (dr1["Scrip"] != DBNull.Value)
                    {
                        row2["Scrip"] = dr1["Scrip"];
                    }
                    if (dr1["ISIN"] != DBNull.Value)
                    {
                        row2["ISIN"] = dr1["ISIN"];
                    }
                    if (dr1["Quantity"] != DBNull.Value)
                    {
                        row2["Quantity"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dr1["Quantity"]));
                    }
                    if (dr1["FROMACNAME"] != DBNull.Value)
                    {
                        row2["From A/c"] = dr1["FROMACNAME"];
                    }
                    if (dr1["SettlementNumberS"] != DBNull.Value)
                    {
                        row2["Sett.No From"] = dr1["SettlementNumberS"];
                    }
                    if (dr1["TOACNAME"] != DBNull.Value)
                    {
                        row2["To A/c"] = dr1["TOACNAME"];
                    }
                    if (dr1["SettlementNumberT"] != DBNull.Value)
                    {
                        row2["Sett.No To"] = dr1["SettlementNumberT"];
                    }
                    if (dr1["RATE"] != DBNull.Value)
                    {
                        row2["Rate"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dr1["RATE"]));
                    }
                    if (dr1["AMOUNT"] != DBNull.Value)
                    {
                        row2["Amount"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dr1["AMOUNT"]));
                        amount = amount + Convert.ToDecimal(dr1["AMOUNT"]);
                    }
                    if (dr1["ServiceTax"] != DBNull.Value)
                    {
                        srvtax = Convert.ToDecimal(dr1["ServiceTax"]);
                    }
                    if (dr1["EduCess"] != DBNull.Value)
                    {
                        educess = Convert.ToDecimal(dr1["EduCess"]);
                    }
                    if (dr1["HgrEduCess"] != DBNull.Value)
                    {
                        hgrcess = Convert.ToDecimal(dr1["HgrEduCess"]);
                    }
                    dtExport.Rows.Add(row2);
                }

                DataRow row3 = dtExport.NewRow();
                row3["Trf Date"] = "Total";
                row3["Amount"] = oconverter.formatmoneyinUs(Convert.ToDecimal(amount));
                dtExport.Rows.Add(row3);


                if (srvtax != 0)
                {
                    DataRow row4 = dtExport.NewRow();
                    row4["Trf Date"] = "Service Tax:";
                    row4["Amount"] = oconverter.formatmoneyinUs(Convert.ToDecimal(srvtax));
                    net = net + srvtax;
                    dtExport.Rows.Add(row4);
                }
                if (educess != 0)
                {
                    DataRow row5 = dtExport.NewRow();
                    row5["Trf Date"] = "Edu. Cess:";
                    row5["Amount"] = oconverter.formatmoneyinUs(Convert.ToDecimal(educess));
                    net = net + educess;
                    dtExport.Rows.Add(row5);
                }
                if (hgrcess != 0)
                {
                    DataRow row6 = dtExport.NewRow();
                    row6["Trf Date"] = "Hgr Edu Cess:";
                    row6["Amount"] = oconverter.formatmoneyinUs(Convert.ToDecimal(hgrcess));
                    net = net + hgrcess;
                    dtExport.Rows.Add(row6);
                }
                if (net != 0)
                {
                    DataRow row7 = dtExport.NewRow();
                    row7["Trf Date"] = "Net Demat Charges:";
                    row7["Amount"] = oconverter.formatmoneyinUs(Convert.ToDecimal(net + amount));
                    dtExport.Rows.Add(row7);
                }


            }
            exportheader();
            if (ddlExport.SelectedItem.Value == "E")
            {
                objExcel.ExportToExcelforExcel(dtExport, "Demat Charges Statement", "Total", dtReportHeader, dtReportFooter);
            }

        }
        void ExportGroup()
        {
            DataTable dtExport = new DataTable();
            dtExport.Columns.Add("Trf Date", Type.GetType("System.String"));
            dtExport.Columns.Add("Ref.ID", Type.GetType("System.String"));
            dtExport.Columns.Add("Type", Type.GetType("System.String"));
            dtExport.Columns.Add("Scrip", Type.GetType("System.String"));
            dtExport.Columns.Add("ISIN", Type.GetType("System.String"));
            dtExport.Columns.Add("Quantity", Type.GetType("System.String"));
            dtExport.Columns.Add("From A/c", Type.GetType("System.String"));
            dtExport.Columns.Add("Sett.No From", Type.GetType("System.String"));
            dtExport.Columns.Add("To A/c", Type.GetType("System.String"));
            dtExport.Columns.Add("Sett.No To", Type.GetType("System.String"));
            dtExport.Columns.Add("Rate", Type.GetType("System.String"));
            dtExport.Columns.Add("Amount", Type.GetType("System.String"));

            decimal amount = decimal.Zero;
            decimal srvtax = decimal.Zero;
            decimal educess = decimal.Zero;
            decimal hgrcess = decimal.Zero;
            decimal net = decimal.Zero;
            string client = null;
            int cmbcount = cmbdistinct.Items.Count;
            for (int i = 0; i < cmbcount; i++)
            {
                DataView viewclient = new DataView();
                viewclient = ds.Tables[0].DefaultView;
                string valItem = cmbdistinct.Items[i].Value;
                viewclient.RowFilter = "GRPID='" + valItem + "'";
                DataTable dt = new DataTable();
                dt = viewclient.ToTable();



                DataRow row = dtExport.NewRow();
                if (ddlGroup.SelectedItem.Value.ToString() == "0")
                {
                    row["Trf Date"] = "Branch: " + dt.Rows[0]["GRPCODE"];
                }
                else
                {
                    row["Trf Date"] = "Group: " + dt.Rows[0]["GRPCODE"];
                }

                row["Ref.ID"] = "Test";
                dtExport.Rows.Add(row);



                amount = decimal.Zero;
                srvtax = decimal.Zero;
                educess = decimal.Zero;
                hgrcess = decimal.Zero;
                net = decimal.Zero;
                foreach (DataRow dr1 in dt.Rows)
                {
                    if (client != dr1["Name"])
                    {
                        DataRow row11 = dtExport.NewRow();
                        row11["Trf Date"] = dr1["Name"];
                        row11["Ref.ID"] = "Test";
                        dtExport.Rows.Add(row11);
                        if (amount != 0)
                        {
                            DataRow row12 = dtExport.NewRow();
                            row12["Trf Date"] = "Total";
                            row12["Amount"] = oconverter.formatmoneyinUs(Convert.ToDecimal(amount));
                            dtExport.Rows.Add(row12);

                            if (srvtax != 0)
                            {
                                DataRow row13 = dtExport.NewRow();
                                row13["Trf Date"] = "Service Tax:";
                                row13["Amount"] = oconverter.formatmoneyinUs(Convert.ToDecimal(srvtax));
                                net = net + srvtax;
                                dtExport.Rows.Add(row13);
                            }
                            if (educess != 0)
                            {
                                DataRow row14 = dtExport.NewRow();
                                row14["Trf Date"] = "Edu. Cess:";
                                row14["Amount"] = oconverter.formatmoneyinUs(Convert.ToDecimal(educess));
                                net = net + educess;
                                dtExport.Rows.Add(row14);
                            }
                            if (hgrcess != 0)
                            {
                                DataRow row6 = dtExport.NewRow();
                                row6["Sett.No To"] = "Hgr Edu Cess:";
                                row6["Amount"] = oconverter.formatmoneyinUs(Convert.ToDecimal(hgrcess));
                                net = net + hgrcess;
                                dtExport.Rows.Add(row6);
                            }
                            if (net != 0)
                            {
                                DataRow row15 = dtExport.NewRow();
                                row15["Trf Date"] = "Net Demat Charges:";
                                row15["Amount"] = oconverter.formatmoneyinUs(Convert.ToDecimal(net + amount));
                                dtExport.Rows.Add(row15);
                            }
                        }
                        client = dr1["CustomerID"].ToString();
                        amount = decimal.Zero;
                        srvtax = decimal.Zero;
                        educess = decimal.Zero;
                        hgrcess = decimal.Zero;
                        net = decimal.Zero;
                    }
                    DataRow row2 = dtExport.NewRow();
                    if (dr1["TransactionDate"] != DBNull.Value)
                    {
                        row2["Trf Date"] = dr1["TransactionDate"];
                    }
                    if (dr1["TransactionId"] != DBNull.Value)
                    {
                        row2["Ref.ID"] = dr1["TransactionId"];
                    }
                    if (dr1["TransactionType"] != DBNull.Value)
                    {
                        row2["Type"] = dr1["TransactionType"];
                    }
                    if (dr1["Scrip"] != DBNull.Value)
                    {
                        row2["Scrip"] = dr1["Scrip"];
                    }
                    if (dr1["ISIN"] != DBNull.Value)
                    {
                        row2["ISIN"] = dr1["ISIN"];
                    }
                    if (dr1["Quantity"] != DBNull.Value)
                    {
                        row2["Quantity"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dr1["Quantity"]));
                    }
                    if (dr1["FROMACNAME"] != DBNull.Value)
                    {
                        row2["From A/c"] = dr1["FROMACNAME"];
                    }
                    if (dr1["SettlementNumberS"] != DBNull.Value)
                    {
                        row2["Sett.No From"] = dr1["SettlementNumberS"];
                    }
                    if (dr1["TOACNAME"] != DBNull.Value)
                    {
                        row2["To A/c"] = dr1["TOACNAME"];
                    }
                    if (dr1["SettlementNumberT"] != DBNull.Value)
                    {
                        row2["Sett.No To"] = dr1["SettlementNumberT"];
                    }
                    if (dr1["RATE"] != DBNull.Value)
                    {
                        row2["Rate"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dr1["RATE"]));
                    }
                    if (dr1["AMOUNT"] != DBNull.Value)
                    {
                        row2["Amount"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dr1["AMOUNT"]));
                        amount = amount + Convert.ToDecimal(dr1["AMOUNT"]);
                    }
                    if (dr1["ServiceTax"] != DBNull.Value)
                    {
                        srvtax = Convert.ToDecimal(dr1["ServiceTax"]);
                    }
                    if (dr1["EduCess"] != DBNull.Value)
                    {
                        educess = Convert.ToDecimal(dr1["EduCess"]);
                    }
                    if (dr1["HgrEduCess"] != DBNull.Value)
                    {
                        hgrcess = Convert.ToDecimal(dr1["HgrEduCess"]);
                    }
                    dtExport.Rows.Add(row2);
                }

                DataRow row3 = dtExport.NewRow();
                row3["Trf Date"] = "Total";
                row3["Amount"] = oconverter.formatmoneyinUs(Convert.ToDecimal(amount));
                dtExport.Rows.Add(row3);


                if (srvtax != 0)
                {
                    DataRow row4 = dtExport.NewRow();
                    row4["Trf Date"] = "Service Tax:";
                    row4["Amount"] = oconverter.formatmoneyinUs(Convert.ToDecimal(srvtax));
                    net = net + srvtax;
                    dtExport.Rows.Add(row4);
                }
                if (educess != 0)
                {
                    DataRow row5 = dtExport.NewRow();
                    row5["Trf Date"] = "Edu. Cess:";
                    row5["Amount"] = oconverter.formatmoneyinUs(Convert.ToDecimal(educess));
                    net = net + educess;
                    dtExport.Rows.Add(row5);
                }
                if (hgrcess != 0)
                {
                    DataRow row6 = dtExport.NewRow();
                    row6["Trf Date"] = "Hgr Edu Cess:";
                    row6["Amount"] = oconverter.formatmoneyinUs(Convert.ToDecimal(hgrcess));
                    net = net + hgrcess;
                    dtExport.Rows.Add(row6);
                }
                if (net != 0)
                {
                    DataRow row7 = dtExport.NewRow();
                    row7["Trf Date"] = "Net Demat Charges:";
                    row7["Amount"] = oconverter.formatmoneyinUs(Convert.ToDecimal(net + amount));
                    dtExport.Rows.Add(row7);
                }


            }
            exportheader();
            if (ddlExport.SelectedItem.Value == "E")
            {
                objExcel.ExportToExcelforExcel(dtExport, "Demat Charges Statement", "Total", dtReportHeader, dtReportFooter);
            }

        }
        void exportheader()
        {
            DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            HeaderRow[0] = CompanyName.Rows[0][0].ToString();
            dtReportHeader.Rows.Add(HeaderRow);
            DataRow DrRowR1 = dtReportHeader.NewRow();

            DrRowR1[0] = "Demat Charges Statement " + " [" + ddlType.SelectedItem.Text.ToString() + "] " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + " To " + oconverter.ArrangeDate2(dtTo.Value.ToString());

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
        }
        protected void BtnEmail_Click(object sender, EventArgs e)
        {
            fn_Client();
            if (Clients == null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD", "NORECORD();", true);
            }
            else
            {

                procedure();
            }

        }
        void EmailClientWise(DataSet ds, string Date)
        {
            DataView viewData = new DataView();
            viewData = ds.Tables[0].DefaultView;
            viewData.RowFilter = " Customerid<>'ZZZZZZZZZZZ' and Customerid is not null";
            DataTable dt = new DataTable();
            dt = viewData.ToTable();

            DataTable Distinctclient = new DataTable();
            DataView viewClient = new DataView(dt);
            Distinctclient = viewClient.ToTable(true, new string[] { "CustomerId", "Name" });

            if (Distinctclient.Rows.Count > 0)
            {
                cmbdistinct.DataSource = Distinctclient;
                cmbdistinct.DataValueField = "CustomerId";
                cmbdistinct.DataTextField = "Name";
                cmbdistinct.DataBind();

            }
            ViewState["mailsendresult"] = "mail";
            /////////For Client Email
            for (int k = 0; k < cmbdistinct.Items.Count; k++)
            {
                FnHtml(ds, cmbdistinct.Items[k].Value.ToString().Trim());

                if (oDBEngine.SendReport(ViewState["mail"].ToString().Trim(), cmbdistinct.Items[k].Value.ToString().Trim(), Date.ToString().Trim(), "Demat Charges Statement [" + Date.ToString().Trim() + "]") == true)
                {
                    if (ViewState["mailsendresult"].ToString().Trim() == "Error")
                    {
                        ViewState["mailsendresult"] = "SomeClientError";
                    }
                    else
                    {
                        ViewState["mailsendresult"] = "Success";
                    }
                }
                else
                {

                    ViewState["mailsendresult"] = "Error";
                }
            }

            if (ViewState["mailsendresult"].ToString().Trim() == "Error")
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript312", "<script language='javascript'>Page_Load();</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript314", "<script language='javascript'>alert('Error on sending!Try again.. !!');</script>");
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordStatus", "RecordDisplay('4');", true);
            }
            if (ViewState["mailsendresult"].ToString().Trim() == "SomeClientError")
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript212", "<script language='javascript'>Page_Load();</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript214", "<script language='javascript'>alert('Mail Sent Successfully !!'+'\n'+'Emails not Sent For Some Clients...');</script>");
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordStatus", "RecordDisplay('5');", true);
            }
            if (ViewState["mailsendresult"].ToString().Trim() == "Success")
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript112", "<script language='javascript'>Page_Load();</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript114", "<script language='javascript'>alert('Mail Sent Successfully');</script>");
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordDisplay", "RecordDisplay('6');", true);
            }
        }
        void EmailGroupBranchWise(DataSet ds, string Date)
        {
            ViewState["mailsendresult"] = "mail";
            ViewState["GrpEmail"] = "mail";
            DataView viewData = new DataView();
            viewData = ds.Tables[0].DefaultView;
            viewData.RowFilter = "GRPID is not null and GRPCODE is not null";
            DataTable dt = new DataTable();
            dt = viewData.ToTable();

            DataTable DistinctGrp = new DataTable();
            DataView viewGrp = new DataView(dt);
            DistinctGrp = viewGrp.ToTable(true, new string[] { "GRPID", "GRPCODE" });
            DataTable dtemail = new DataTable();
            string email = "";

            if (DistinctGrp.Rows.Count > 0)
            {
                for (int j = 0; j < DistinctGrp.Rows.Count; j++)
                {
                    DataView viewgrp = new DataView();
                    viewgrp = ds.Tables[0].DefaultView;
                    viewgrp.RowFilter = "GRPID='" + DistinctGrp.Rows[j][0].ToString().Trim() + "'";
                    DataTable dt1 = new DataTable();
                    dt1 = viewgrp.ToTable();

                    DataView viewData1 = new DataView();
                    viewData1 = dt1.DefaultView;
                    viewData1.RowFilter = " CustomerId<>'ZZZZZZZZZZZ' and Customerid is not null";
                    DataTable dt2 = new DataTable();
                    dt2 = viewData1.ToTable();

                    DataTable Distinctclient = new DataTable();
                    DataView viewClient = new DataView(dt2);
                    Distinctclient = viewClient.ToTable(true, new string[] { "CustomerId", "Name" });
                    if (Distinctclient.Rows.Count > 0)
                    {
                        cmbdistinct.DataSource = Distinctclient;
                        cmbdistinct.DataValueField = "CustomerId";
                        cmbdistinct.DataTextField = "Name";
                        cmbdistinct.DataBind();

                    }
                    for (int k = 0; k < cmbdistinct.Items.Count; k++)
                    {
                        FnHtml(ds, cmbdistinct.Items[k].Value.ToString().Trim());

                        if (ViewState["GrpEmail"].ToString().Trim() == "mail")
                        {
                            ViewState["GrpEmail"] = ViewState["mail"].ToString().Trim();
                        }
                        else
                        {
                            ViewState["GrpEmail"] = ViewState["GrpEmail"].ToString().Trim() + ViewState["mail"].ToString().Trim();
                        }


                    }
                    if (ddlGroup.SelectedItem.Value == "0")
                    {
                        dtemail = oDBEngine.GetDataTable("tbl_master_branch", "branch_cpemail", "branch_id=" + DistinctGrp.Rows[j][0] + "");
                    }
                    else
                    {
                        dtemail = oDBEngine.GetDataTable("tbl_master_groupmaster", "gpm_emailid", "gpm_id=" + DistinctGrp.Rows[j][0] + "");
                    }

                    if (dtemail.Rows.Count > 0)
                    {
                        email = dtemail.Rows[0][0].ToString();
                    }
                    else
                    {
                        email = "";
                    }

                    /////////For Client Email End
                    /////////Group/Branch Email Send Begin
                    if (oDBEngine.SendReportdemat(ViewState["GrpEmail"].ToString().Trim(), email.ToString().Trim(), Date.ToString().Trim(), "Demat Charges Statement [" + Date.ToString().Trim() + "]", DistinctGrp.Rows[j]["Grpid"].ToString().Trim()) == true)
                    {
                        if (ViewState["mailsendresult"].ToString().Trim() == "Error")
                        {
                            ViewState["mailsendresult"] = "SomeClientError";
                        }
                        else
                        {
                            ViewState["mailsendresult"] = "Success";
                        }
                    }
                    else
                    {

                        ViewState["mailsendresult"] = "Error";
                    }
                    ViewState["GrpEmail"] = "mail";
                    ////////Group/Branch Emil Send End

                }
            }
            else
            {
                ViewState["mailsendresult"] = "EmailNotFound";
            }


            if (ViewState["mailsendresult"].ToString().Trim() == "Error")
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript1312", "<script language='javascript'>Page_Load();</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript1314", "<script language='javascript'>alert('Error on sending!Try again.. !!');</script>");
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordStatus", "RecordDisplay('4');", true);
            }
            if (ViewState["mailsendresult"].ToString().Trim() == "SomeClientError")
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript1212", "<script language='javascript'>Page_Load();</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript1214", "<script language='javascript'>alert('Mail Sent Successfully !!'+'\n'+'Emails not Sent For Some Clients...');</script>");
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordStatus", "RecordDisplay('5');", true);
            }
            if (ViewState["mailsendresult"].ToString().Trim() == "Success")
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript1112", "<script language='javascript'>Page_Load();</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript1114", "<script language='javascript'>alert('Mail Sent Successfully');</script>");
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordDisplay", "RecordDisplay('6');", true);
            }
            if (ViewState["mailsendresult"].ToString().Trim() == "EmailNotFound")
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript11112", "<script language='javascript'>Page_Load();</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript11114", "<script language='javascript'>alert('Email id Not Found');</script>");
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordStatus", "RecordDisplay('7');", true);
            }
        }
    }
}


//}
