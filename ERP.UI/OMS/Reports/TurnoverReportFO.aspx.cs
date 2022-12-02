using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Reports
{
    public partial class Reports_TurnoverReportFO : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        BusinessLogicLayer.Reports oReports = new BusinessLogicLayer.Reports();
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        DataSet ds = new DataSet();
        DataTable dtgroupcontactid = new DataTable();
        DataTable Distinctclient = new DataTable();
        ExcelFile objExcel = new ExcelFile();
        DataTable dtExport = new DataTable();
        string data;
        int pageindex = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
               // Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
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
                if (idlist[0] != "Clients" && idlist[0] != "Expiry" && idlist[0] != "Broker")
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
            else if (idlist[0] == "Broker")
            {
                data = "Broker~" + str;
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
            dtfrom.EditFormatString = oconverter.GetDateFormat("Date");
            dtto.EditFormatString = oconverter.GetDateFormat("Date");
            dtfrom.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
            dtto.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
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
                if (ddlrptfor.SelectedItem.Value.ToString().Trim() == "0")
                {
                    ddlbandforgroup();
                    if (ddlrpttype.SelectedItem.Value.ToString().Trim() == "1")
                    {
                        CurrentPage = 0;
                        ddlbandforClient();
                    }
                    else
                    {
                        htmltable_rpttype_Consolidated();
                    }
                }
                else
                {
                    htmltable_rpttype_EXCHANGE();
                }
            }
        }
        void procedure()
        {
            string Broker = string.Empty;
            string clients = string.Empty;
            string grptype = string.Empty;
            string grp = string.Empty;
            string branch = string.Empty;
            string rpttype = string.Empty;
            string chkFigures = string.Empty;
            if (ddlviewby.SelectedItem.Value == "2")
            {
                Broker = "BO";
                if (rdbbrokerall.Checked)
                {
                    clients = "ALL";
                }
                else
                {
                    clients = Convert.ToString(HiddenField_Broker.Value);
                }

            }

            if (ddlviewby.SelectedItem.Value == "1")
            {
                Broker = "NA";
                if (rdbClientALL.Checked)
                {
                    clients = "ALL";
                }
                else
                {
                    clients = Convert.ToString(HiddenField_Client.Value);
                }
            }

            if (ddlrptfor.SelectedItem.Value.ToString() == "0")
            {
                if (ddlGroup.SelectedItem.Value.ToString() == "0")
                {
                    grptype = "BRANCH";
                    if (rdbranchAll.Checked)
                    {
                        grp = "ALL";
                        branch = Convert.ToString(Session["userbranchHierarchy"]);
                    }
                    else
                    {
                        grp = Convert.ToString(HiddenField_Branch.Value);
                        branch = "NOT";
                    }
                }
                else
                {
                    grptype = Convert.ToString(ddlgrouptype.SelectedItem.Text);
                    if (rdddlgrouptypeAll.Checked)
                    {
                        grp = "ALL";
                        branch = Convert.ToString(Session["userbranchHierarchy"]);
                    }
                    else
                    {
                        grp = Convert.ToString(HiddenField_Group.Value);
                        branch = Convert.ToString(Session["userbranchHierarchy"]);
                    }
                }
                rpttype = Convert.ToString(ddlrpttype.SelectedItem.Value);
            }
            else
            {
                grptype = "All";
                grp = "ALL";
                branch = Convert.ToString(Session["userbranchHierarchy"]);
                rpttype = "1";
            }

            if (chFigures.Checked)
            {
                chkFigures = "CHK";
            }
            else
            {
                chkFigures = "UNCHK";
            }



            ds = oReports.TOTReportFO(
                Convert.ToString(Session["LastCompany"]),
                Convert.ToString(Session["usersegid"]),
                Convert.ToString(dtfrom.Value),
                Convert.ToString(dtto.Value),
                 Broker,
                clients,
               Convert.ToString(HttpContext.Current.Session["ExchangeSegmentID"]),
               grptype,
               grp,
               branch,
               rpttype,
                Convert.ToString(ddlrptfor.SelectedItem.Value),
                Convert.ToString(ddloption.SelectedItem.Value),
               chkFigures
               );

            ViewState["dataset"] = ds;
            if (ds.Tables[0].Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(1);", true);
            }
            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            //{

            //    SqlCommand cmd = new SqlCommand();
            //    cmd.Connection = con;
            //    cmd.CommandText = "[TOTReportFO]";

            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.Parameters.AddWithValue("@companyid", Session["LastCompany"].ToString());
            //    cmd.Parameters.AddWithValue("@segment", Session["usersegid"].ToString().Trim());
            //    cmd.Parameters.AddWithValue("@fromdate", dtfrom.Value);
            //    cmd.Parameters.AddWithValue("@todate", dtto.Value);

            //    if (ddlviewby.SelectedItem.Value == "2")
            //    {
            //        cmd.Parameters.AddWithValue("@Broker", "BO");
            //        if (rdbbrokerall.Checked)
            //        {
            //            cmd.Parameters.AddWithValue("@clients", "ALL");
            //        }
            //        else
            //        {
            //            cmd.Parameters.AddWithValue("@clients", HiddenField_Broker.Value);
            //        }

            //    }

            //    if (ddlviewby.SelectedItem.Value == "1")
            //    {
            //        cmd.Parameters.AddWithValue("@Broker", "NA");
            //        if (rdbClientALL.Checked)
            //        {
            //            cmd.Parameters.AddWithValue("@clients", "ALL");
            //        }
            //        else
            //        {
            //            cmd.Parameters.AddWithValue("@clients", HiddenField_Client.Value);
            //        }
            //    }
            //    cmd.Parameters.AddWithValue("@MasterSegment", HttpContext.Current.Session["ExchangeSegmentID"].ToString());

            //    if (ddlrptfor.SelectedItem.Value.ToString() == "0")
            //    {
            //        if (ddlGroup.SelectedItem.Value.ToString() == "0")
            //        {
            //            cmd.Parameters.AddWithValue("@grptype", "BRANCH");
            //            if (rdbranchAll.Checked)
            //            {
            //                cmd.Parameters.AddWithValue("@grp", "ALL");
            //                cmd.Parameters.AddWithValue("@branch", Session["userbranchHierarchy"].ToString().Trim());
            //            }
            //            else
            //            {
            //                cmd.Parameters.AddWithValue("@grp", HiddenField_Branch.Value.ToString().Trim());
            //                cmd.Parameters.AddWithValue("@branch", "NOT");
            //            }
            //        }
            //        else
            //        {
            //            cmd.Parameters.AddWithValue("@grptype", ddlgrouptype.SelectedItem.Text.ToString().Trim());
            //            if (rdddlgrouptypeAll.Checked)
            //            {
            //                cmd.Parameters.AddWithValue("@grp", "ALL");
            //                cmd.Parameters.AddWithValue("@branch", Session["userbranchHierarchy"].ToString().Trim());
            //            }
            //            else
            //            {
            //                cmd.Parameters.AddWithValue("@grp", HiddenField_Group.Value.ToString().Trim());
            //                cmd.Parameters.AddWithValue("@branch", Session["userbranchHierarchy"].ToString().Trim());
            //            }
            //        }
            //        cmd.Parameters.AddWithValue("@rpttype", ddlrpttype.SelectedItem.Value.ToString().Trim());
            //    }
            //    else
            //    {
            //        cmd.Parameters.AddWithValue("@grptype", "All");
            //        cmd.Parameters.AddWithValue("@grp", "ALL");
            //        cmd.Parameters.AddWithValue("@branch", Session["userbranchHierarchy"].ToString().Trim());
            //        cmd.Parameters.AddWithValue("@rpttype", "1");
            //    }
            //    cmd.Parameters.AddWithValue("@for",ddlrptfor.SelectedItem.Value.ToString().Trim());

            //    cmd.Parameters.AddWithValue("@option", ddloption.SelectedItem.Value.ToString().Trim());
            //    if (chFigures.Checked)
            //    {
            //        cmd.Parameters.AddWithValue("@chFigures", "CHK");
            //    }
            //    else
            //    {
            //        cmd.Parameters.AddWithValue("@chFigures", "UNCHK");
            //    }
            //    SqlDataAdapter da = new SqlDataAdapter();
            //    da.SelectCommand = cmd;
            //    cmd.CommandTimeout = 0;
            //    ds.Reset();
            //    da.Fill(ds);
            //    da.Dispose();
            //    ViewState["dataset"] = ds;
            //    if (ds.Tables[0].Rows.Count == 0)
            //    {
            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(1);", true);
            //    }
            //}


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
            htmltable_rpttype_datewise();
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
        void htmltable_rpttype_datewise()
        {
            ds = (DataSet)ViewState["dataset"];
            String strHtml = String.Empty;
            String strHtml1 = String.Empty;
            int colcount = ds.Tables[0].Columns.Count;
            string str = null;
            int flag = 0;
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                str = "[" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "]";
            }


            str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "[ <B>" + ddlrpttype.SelectedItem.Text.ToString().Trim() + "</B> ] Report Period " + oconverter.ArrangeDate2(dtfrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtto.Value.ToString());
            strHtml1 = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtml1 += "<tr><td align=\"left\" colspan=" + colcount + " style=\"color:Blue;\">" + str + "</td></tr></table>";

            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

            DataView viewclient = new DataView();
            viewclient = ds.Tables[0].DefaultView;
            viewclient.RowFilter = "CUSTOMERID='" + cmbclient.SelectedItem.Value + "'";
            DataTable dt1 = new DataTable();
            dt1 = viewclient.ToTable();


            strHtml += "<tr style=\"background-color:lavender ;text-align:left\">";
            strHtml += "<td align=\"left\" colspan=" + colcount + ">Client Name :&nbsp;<b>" + cmbclient.SelectedItem.Text.ToString().Trim() + "</b>[ <b>" + dt1.Rows[0]["UCC"].ToString() + " </b> ]</td></tr>";

            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            //if (dtfrom.Value.ToString().Trim() != dtto.Value.ToString().Trim())
            //{
            strHtml += "<td align=\"center\"><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr valign=\"top\"><td colspan=2 width=\"100%\" align=\"center\"><b>Date</b></td></tr></table></td>";
            //}
            strHtml += "<td align=\"center\"><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr><td colspan=2 width=\"100%\" align=\"center\"><b>Future</b></td></tr><tr><td width=\"50%\" align=\"center\"><b>Index </b></td><td width=\"50%\" align=\"center\"><b>  Stocks </b></td></tr></table></td>";
            strHtml += "<td align=\"center\"><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr><td colspan=2 width=\"100%\" align=\"center\"><b>Option [Prm TO]</b></td></tr><tr><td width=\"50%\" align=\"center\"><b>Index </b></td><td width=\"50%\" align=\"center\"><b>  Stocks </b></td></tr></table></td>";
            strHtml += "<td align=\"center\"><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr><td colspan=2 width=\"100%\" align=\"center\"><b>Option [Strike TO]</b></td></tr><tr><td width=\"50%\" align=\"center\"><b>Index </b></td><td width=\"50%\" align=\"center\"><b>  Stocks </b></td></tr></table></td>";
            strHtml += "<td align=\"center\"><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr><td colspan=2 width=\"100%\" align=\"center\"><b>Option [Prm+Strk TO]</b></td></tr><tr><td width=\"50%\" align=\"center\"><b>Index </b></td><td width=\"50%\" align=\"center\"><b>  Stocks </b></td></tr></table></td>";
            strHtml += "<td align=\"center\"><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr><td colspan=2 width=\"100%\" align=\"center\"><b>Future Final Sett.</b></td></tr><tr><td width=\"50%\" align=\"center\"><b>Index </b></td><td width=\"50%\" align=\"center\"><b>  Stocks </b></td></tr></table></td>";
            strHtml += "<td align=\"center\"><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr><td colspan=2 width=\"100%\" align=\"center\"><b>Options Fin Settlmnt</b></td></tr><tr><td width=\"50%\" align=\"center\"><b>Index </b></td><td width=\"50%\" align=\"center\"><b>  Stocks </b></td></tr></table></td>";
            strHtml += "<td align=\"center\"><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr valign=\"top\"><td colspan=2 width=\"100%\" align=\"center\"><b>Total  TO</b></td></tr><tr><td colspan=2 width=\"100%\" align=\"center\"></td></tr></table></td>";
            strHtml += "</tr>";

            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";


                //////////Date
                strHtml += "<td align=\"center\"><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr>";
                strHtml += "<td align=\"center\"  colspan=2 width=\"100%\" style=\"nowrap=nowrap;\">" + dt1.Rows[i]["TRADEDATE"].ToString() + "</td>";
                strHtml += "</tr></table></td>";


                //////////Future
                strHtml += "<td align=\"center\" ><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr>";
                if (dt1.Rows[i]["FUTINDEX"] != DBNull.Value)
                    strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;overflow:x;\">" + dt1.Rows[i]["FUTINDEX"].ToString() + "</td>";
                else
                    strHtml += "<td width=\"50%\">&nbsp;</td>";

                if (dt1.Rows[i]["FUTSTOCKS"] != DBNull.Value)
                    strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;overflow:hidden;\">" + dt1.Rows[i]["FUTSTOCKS"].ToString() + "</td>";
                else
                    strHtml += "<td width=\"50%\">&nbsp;</td>";
                strHtml += "</tr></table></td>";

                //////////Option [Prm TO]
                strHtml += "<td align=\"center\" ><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr>";
                if (dt1.Rows[i]["OPTINDEX_PRM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + dt1.Rows[i]["OPTINDEX_PRM"].ToString() + "</td>";
                else
                    strHtml += "<td width=\"50%\">&nbsp;</td>";

                if (dt1.Rows[i]["OPSTOCKS_PRM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + dt1.Rows[i]["OPSTOCKS_PRM"].ToString() + "</td>";
                else
                    strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";
                strHtml += "</tr></table></td>";

                //////////Option [Strike TO]
                strHtml += "<td align=\"center\" ><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr>";
                if (dt1.Rows[i]["OPTINDEX_STRIKE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + dt1.Rows[i]["OPTINDEX_STRIKE"].ToString() + "</td>";
                else
                    strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";

                if (dt1.Rows[i]["OPSTOCKS_STRIKE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + dt1.Rows[i]["OPSTOCKS_STRIKE"].ToString() + "</td>";
                else
                    strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";
                strHtml += "</tr></table></td>";

                //////////Option [Prm+Strk TO]
                strHtml += "<td align=\"center\" ><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr>";
                if (dt1.Rows[i]["OPTINDEX_PRMSTRIKE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + dt1.Rows[i]["OPTINDEX_PRMSTRIKE"].ToString() + "</td>";
                else
                    strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";

                if (dt1.Rows[i]["OPSTOCKS_PRMSTRIKE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + dt1.Rows[i]["OPSTOCKS_PRMSTRIKE"].ToString() + "</td>";
                else
                    strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";
                strHtml += "</tr></table></td>";

                //////////Future Final Sett.
                strHtml += "<td align=\"center\" ><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr>";
                if (dt1.Rows[i]["FUTINDEX_EXP"] != DBNull.Value)
                    strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + dt1.Rows[i]["FUTINDEX_EXP"].ToString() + "</td>";
                else
                    strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";

                if (dt1.Rows[i]["FUTSTOCKS_EXP"] != DBNull.Value)
                    strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + dt1.Rows[i]["FUTSTOCKS_EXP"].ToString() + "</td>";
                else
                    strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";
                strHtml += "</tr></table></td>";


                //////////Options Fin Settlmnt
                strHtml += "<td align=\"center\" ><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr>";
                if (dt1.Rows[i]["OPTINDEX_EXCASN"] != DBNull.Value)
                    strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + dt1.Rows[i]["OPTINDEX_EXCASN"].ToString() + "</td>";
                else
                    strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";

                if (dt1.Rows[i]["OPSTOCKS_EXCASN"] != DBNull.Value)
                    strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + dt1.Rows[i]["OPSTOCKS_EXCASN"].ToString() + "</td>";
                else
                    strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";
                strHtml += "</tr></table></td>";

                //////////Total  TO
                strHtml += "<td align=\"center\" ><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr>";
                if (dt1.Rows[i]["TOT"] != DBNull.Value)
                    strHtml += "<td align=\"right\"  colspan=2 width=\"100%\" style=\"nowrap=nowrap;\">" + dt1.Rows[i]["TOT"].ToString() + "</td>";
                else
                    strHtml += "<td colspan=2 width=\"100%\" style=\"nowrap=nowrap;\">&nbsp;</td>";

                strHtml += "</tr></table></td>";

                strHtml += "</tr>";
            }

            //////////CLIENT TOTAL
            flag = flag + 1;
            strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";


            //////////Date
            strHtml += "<td align=\"center\"><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr>";
            strHtml += "<td align=\"center\"  colspan=2 width=\"100%\" style=\"nowrap=nowrap;color:maroon;\"><b>Client Total :</b></td>";
            strHtml += "</tr></table></td>";


            //////////Future
            strHtml += "<td align=\"center\" ><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr>";
            if (dt1.Rows[0]["FUTINDEX_SUM"] != DBNull.Value)
                strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;overflow:x;\">" + dt1.Rows[0]["FUTINDEX_SUM"].ToString() + "</td>";
            else
                strHtml += "<td width=\"50%\">&nbsp;</td>";

            if (dt1.Rows[0]["FUTSTOCKS_SUM"] != DBNull.Value)
                strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;overflow:hidden;\">" + dt1.Rows[0]["FUTSTOCKS_SUM"].ToString() + "</td>";
            else
                strHtml += "<td width=\"50%\">&nbsp;</td>";
            strHtml += "</tr></table></td>";

            //////////Option [Prm TO]
            strHtml += "<td align=\"center\" ><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr>";
            if (dt1.Rows[0]["OPTINDEX_PRM_SUM"] != DBNull.Value)
                strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + dt1.Rows[0]["OPTINDEX_PRM_SUM"].ToString() + "</td>";
            else
                strHtml += "<td width=\"50%\">&nbsp;</td>";

            if (dt1.Rows[0]["OPSTOCKS_PRM_SUM"] != DBNull.Value)
                strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + dt1.Rows[0]["OPSTOCKS_PRM_SUM"].ToString() + "</td>";
            else
                strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";
            strHtml += "</tr></table></td>";

            //////////Option [Strike TO]
            strHtml += "<td align=\"center\" ><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr>";
            if (dt1.Rows[0]["OPTINDEX_STRIKE_SUM"] != DBNull.Value)
                strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + dt1.Rows[0]["OPTINDEX_STRIKE_SUM"].ToString() + "</td>";
            else
                strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";

            if (dt1.Rows[0]["OPSTOCKS_STRIKE_SUM"] != DBNull.Value)
                strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + dt1.Rows[0]["OPSTOCKS_STRIKE_SUM"].ToString() + "</td>";
            else
                strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";
            strHtml += "</tr></table></td>";

            //////////Option [Prm+Strk TO]
            strHtml += "<td align=\"center\" ><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr>";
            if (dt1.Rows[0]["OPTINDEX_PRMSTRIKE_SUM"] != DBNull.Value)
                strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + dt1.Rows[0]["OPTINDEX_PRMSTRIKE_SUM"].ToString() + "</td>";
            else
                strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";

            if (dt1.Rows[0]["OPSTOCKS_PRMSTRIKE_SUM"] != DBNull.Value)
                strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + dt1.Rows[0]["OPSTOCKS_PRMSTRIKE_SUM"].ToString() + "</td>";
            else
                strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";
            strHtml += "</tr></table></td>";

            //////////Future Final Sett.
            strHtml += "<td align=\"center\" ><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr>";
            if (dt1.Rows[0]["FUTINDEX_EXP_SUM"] != DBNull.Value)
                strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + dt1.Rows[0]["FUTINDEX_EXP_SUM"].ToString() + "</td>";
            else
                strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";

            if (dt1.Rows[0]["FUTSTOCKS_EXP_SUM"] != DBNull.Value)
                strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + dt1.Rows[0]["FUTSTOCKS_EXP_SUM"].ToString() + "</td>";
            else
                strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";
            strHtml += "</tr></table></td>";


            //////////Options Fin Settlmnt
            strHtml += "<td align=\"center\" ><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr>";
            if (dt1.Rows[0]["OPTINDEX_EXCASN_SUM"] != DBNull.Value)
                strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + dt1.Rows[0]["OPTINDEX_EXCASN_SUM"].ToString() + "</td>";
            else
                strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";

            if (dt1.Rows[0]["OPSTOCKS_EXCASN_SUM"] != DBNull.Value)
                strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + dt1.Rows[0]["OPSTOCKS_EXCASN_SUM"].ToString() + "</td>";
            else
                strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";
            strHtml += "</tr></table></td>";

            //////////Total  TO
            strHtml += "<td align=\"center\" ><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr>";
            if (dt1.Rows[0]["TOT_SUM"] != DBNull.Value)
                strHtml += "<td align=\"right\"  colspan=2 width=\"100%\" style=\"nowrap=nowrap;\">" + dt1.Rows[0]["TOT_SUM"].ToString() + "</td>";
            else
                strHtml += "<td colspan=2 width=\"100%\" style=\"nowrap=nowrap;\">&nbsp;</td>";

            strHtml += "</tr></table></td>";

            strHtml += "</tr>";


            //////////GROUP TOTAL
            flag = flag + 1;
            strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";


            //////////Date
            strHtml += "<td align=\"center\"><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr>";
            strHtml += "<td align=\"center\"  colspan=2 width=\"100%\" style=\"nowrap=nowrap;color:black;\"><b>Group Total :</b></td>";
            strHtml += "</tr></table></td>";


            //////////Future
            strHtml += "<td align=\"center\" ><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr>";
            if (dt1.Rows[0]["FUTINDEX_GRPSUM"] != DBNull.Value)
                strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;overflow:x;\">" + dt1.Rows[0]["FUTINDEX_GRPSUM"].ToString() + "</td>";
            else
                strHtml += "<td width=\"50%\">&nbsp;</td>";

            if (dt1.Rows[0]["FUTSTOCKS_GRPSUM"] != DBNull.Value)
                strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;overflow:hidden;\">" + dt1.Rows[0]["FUTSTOCKS_GRPSUM"].ToString() + "</td>";
            else
                strHtml += "<td width=\"50%\">&nbsp;</td>";
            strHtml += "</tr></table></td>";

            //////////Option [Prm TO]
            strHtml += "<td align=\"center\" ><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr>";
            if (dt1.Rows[0]["OPTINDEX_PRM_GRPSUM"] != DBNull.Value)
                strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + dt1.Rows[0]["OPTINDEX_PRM_GRPSUM"].ToString() + "</td>";
            else
                strHtml += "<td width=\"50%\">&nbsp;</td>";

            if (dt1.Rows[0]["OPSTOCKS_PRM_GRPSUM"] != DBNull.Value)
                strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + dt1.Rows[0]["OPSTOCKS_PRM_GRPSUM"].ToString() + "</td>";
            else
                strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";
            strHtml += "</tr></table></td>";

            //////////Option [Strike TO]
            strHtml += "<td align=\"center\" ><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr>";
            if (dt1.Rows[0]["OPTINDEX_STRIKE_GRPSUM"] != DBNull.Value)
                strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + dt1.Rows[0]["OPTINDEX_STRIKE_GRPSUM"].ToString() + "</td>";
            else
                strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";

            if (dt1.Rows[0]["OPSTOCKS_STRIKE_GRPSUM"] != DBNull.Value)
                strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + dt1.Rows[0]["OPSTOCKS_STRIKE_GRPSUM"].ToString() + "</td>";
            else
                strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";
            strHtml += "</tr></table></td>";

            //////////Option [Prm+Strk TO]
            strHtml += "<td align=\"center\" ><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr>";
            if (dt1.Rows[0]["OPTINDEX_PRMSTRIKE_GRPSUM"] != DBNull.Value)
                strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + dt1.Rows[0]["OPTINDEX_PRMSTRIKE_GRPSUM"].ToString() + "</td>";
            else
                strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";

            if (dt1.Rows[0]["OPSTOCKS_PRMSTRIKE_GRPSUM"] != DBNull.Value)
                strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + dt1.Rows[0]["OPSTOCKS_PRMSTRIKE_GRPSUM"].ToString() + "</td>";
            else
                strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";
            strHtml += "</tr></table></td>";

            //////////Future Final Sett.
            strHtml += "<td align=\"center\" ><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr>";
            if (dt1.Rows[0]["FUTINDEX_EXP_GRPSUM"] != DBNull.Value)
                strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + dt1.Rows[0]["FUTINDEX_EXP_GRPSUM"].ToString() + "</td>";
            else
                strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";

            if (dt1.Rows[0]["FUTSTOCKS_EXP_GRPSUM"] != DBNull.Value)
                strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + dt1.Rows[0]["FUTSTOCKS_EXP_GRPSUM"].ToString() + "</td>";
            else
                strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";
            strHtml += "</tr></table></td>";


            //////////Options Fin Settlmnt
            strHtml += "<td align=\"center\" ><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr>";
            if (dt1.Rows[0]["OPTINDEX_EXCASN_GRPSUM"] != DBNull.Value)
                strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + dt1.Rows[0]["OPTINDEX_EXCASN_GRPSUM"].ToString() + "</td>";
            else
                strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";

            if (dt1.Rows[0]["OPSTOCKS_EXCASN_GRPSUM"] != DBNull.Value)
                strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + dt1.Rows[0]["OPSTOCKS_EXCASN_GRPSUM"].ToString() + "</td>";
            else
                strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";
            strHtml += "</tr></table></td>";

            //////////Total  TO
            strHtml += "<td align=\"center\" ><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr>";
            if (dt1.Rows[0]["TOT_GRPSUM"] != DBNull.Value)
                strHtml += "<td align=\"right\"  colspan=2 width=\"100%\" style=\"nowrap=nowrap;\">" + dt1.Rows[0]["TOT_GRPSUM"].ToString() + "</td>";
            else
                strHtml += "<td colspan=2 width=\"100%\" style=\"nowrap=nowrap;\">&nbsp;</td>";

            strHtml += "</tr></table></td>";

            strHtml += "</tr>";


            strHtml += "</table>";
            DIVdisplayPERIOD.InnerHtml = strHtml1;
            display.InnerHtml = strHtml;

        }
        void htmltable_rpttype_Consolidated()
        {
            ds = (DataSet)ViewState["dataset"];
            String strHtml = String.Empty;
            String strHtml1 = String.Empty;
            int colcount = ds.Tables[0].Columns.Count;
            string str = null;
            int flag = 0;
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                str = "[" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "]";
            }


            str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "[ <B>" + ddlrpttype.SelectedItem.Text.ToString().Trim() + "</B> ] Report Period " + oconverter.ArrangeDate2(dtfrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtto.Value.ToString());
            strHtml1 = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtml1 += "<tr><td align=\"left\" colspan=" + colcount + " style=\"color:Blue;\">" + str + "</td></tr></table>";

            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

            DataView viewgrp = new DataView();
            viewgrp = ds.Tables[0].DefaultView;
            viewgrp.RowFilter = "GRPID='" + cmbgroup.SelectedItem.Value + "'";
            DataTable dt1 = new DataTable();
            dt1 = viewgrp.ToTable();



            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            strHtml += "<td align=\"center\"><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr valign=\"top\"><td colspan=2 width=\"100%\" align=\"center\"><b>Client Name</b></td></tr></table></td>";
            strHtml += "<td align=\"center\"><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr><td colspan=2 width=\"100%\" align=\"center\"><b>Future</b></td></tr><tr><td width=\"50%\" align=\"center\"><b>Index </b></td><td width=\"50%\" align=\"center\"><b>  Stocks </b></td></tr></table></td>";
            strHtml += "<td align=\"center\"><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr><td colspan=2 width=\"100%\" align=\"center\"><b>Option [Prm TO]</b></td></tr><tr><td width=\"50%\" align=\"center\"><b>Index </b></td><td width=\"50%\" align=\"center\"><b>  Stocks </b></td></tr></table></td>";
            strHtml += "<td align=\"center\"><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr><td colspan=2 width=\"100%\" align=\"center\"><b>Option [Strike TO]</b></td></tr><tr><td width=\"50%\" align=\"center\"><b>Index </b></td><td width=\"50%\" align=\"center\"><b>  Stocks </b></td></tr></table></td>";
            strHtml += "<td align=\"center\"><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr><td colspan=2 width=\"100%\" align=\"center\"><b>Option [Prm+Strk TO]</b></td></tr><tr><td width=\"50%\" align=\"center\"><b>Index </b></td><td width=\"50%\" align=\"center\"><b>  Stocks </b></td></tr></table></td>";
            strHtml += "<td align=\"center\"><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr><td colspan=2 width=\"100%\" align=\"center\"><b>Future Final Sett.</b></td></tr><tr><td width=\"50%\" align=\"center\"><b>Index </b></td><td width=\"50%\" align=\"center\"><b>  Stocks </b></td></tr></table></td>";
            strHtml += "<td align=\"center\"><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr><td colspan=2 width=\"100%\" align=\"center\"><b>Options Fin Settlmnt</b></td></tr><tr><td width=\"50%\" align=\"center\"><b>Index </b></td><td width=\"50%\" align=\"center\"><b>  Stocks </b></td></tr></table></td>";
            strHtml += "<td align=\"center\"><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr valign=\"top\"><td colspan=2 width=\"100%\" align=\"center\"><b>Total  TO</b></td></tr><tr><td colspan=2 width=\"100%\" align=\"center\"></td></tr></table></td>";
            strHtml += "</tr>";

            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";

                //////////Client Name
                strHtml += "<td align=\"center\"><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr>";
                strHtml += "<td align=\"left\"  colspan=2 width=\"100%\" style=\"nowrap=nowrap;\">" + dt1.Rows[i]["CLIENTNAME"].ToString() + "[" + dt1.Rows[i]["UCC"].ToString() + "]</td>";
                strHtml += "</tr></table></td>";

                //////////Future
                strHtml += "<td align=\"center\" ><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr>";
                if (dt1.Rows[i]["FUTINDEX_SUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;overflow:x;\">" + dt1.Rows[i]["FUTINDEX_SUM"].ToString() + "</td>";
                else
                    strHtml += "<td width=\"50%\">&nbsp;</td>";

                if (dt1.Rows[i]["FUTSTOCKS_SUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;overflow:hidden;\">" + dt1.Rows[i]["FUTSTOCKS_SUM"].ToString() + "</td>";
                else
                    strHtml += "<td width=\"50%\">&nbsp;</td>";
                strHtml += "</tr></table></td>";

                //////////Option [Prm TO]
                strHtml += "<td align=\"center\" ><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr>";
                if (dt1.Rows[i]["OPTINDEX_PRM_SUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + dt1.Rows[i]["OPTINDEX_PRM_SUM"].ToString() + "</td>";
                else
                    strHtml += "<td width=\"50%\">&nbsp;</td>";

                if (dt1.Rows[i]["OPSTOCKS_PRM_SUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + dt1.Rows[i]["OPSTOCKS_PRM_SUM"].ToString() + "</td>";
                else
                    strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";
                strHtml += "</tr></table></td>";

                //////////Option [Strike TO]
                strHtml += "<td align=\"center\" ><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr>";
                if (dt1.Rows[i]["OPTINDEX_STRIKE_SUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + dt1.Rows[i]["OPTINDEX_STRIKE_SUM"].ToString() + "</td>";
                else
                    strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";

                if (dt1.Rows[i]["OPSTOCKS_STRIKE_SUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + dt1.Rows[i]["OPSTOCKS_STRIKE_SUM"].ToString() + "</td>";
                else
                    strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";
                strHtml += "</tr></table></td>";

                //////////Option [Prm+Strk TO]
                strHtml += "<td align=\"center\" ><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr>";
                if (dt1.Rows[i]["OPTINDEX_PRMSTRIKE_SUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + dt1.Rows[i]["OPTINDEX_PRMSTRIKE_SUM"].ToString() + "</td>";
                else
                    strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";

                if (dt1.Rows[i]["OPSTOCKS_PRMSTRIKE_SUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + dt1.Rows[i]["OPSTOCKS_PRMSTRIKE_SUM"].ToString() + "</td>";
                else
                    strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";
                strHtml += "</tr></table></td>";

                //////////Future Final Sett.
                strHtml += "<td align=\"center\" ><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr>";
                if (dt1.Rows[i]["FUTINDEX_EXP_SUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + dt1.Rows[i]["FUTINDEX_EXP_SUM"].ToString() + "</td>";
                else
                    strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";

                if (dt1.Rows[i]["FUTSTOCKS_EXP_SUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + dt1.Rows[i]["FUTSTOCKS_EXP_SUM"].ToString() + "</td>";
                else
                    strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";
                strHtml += "</tr></table></td>";


                //////////Options Fin Settlmnt
                strHtml += "<td align=\"center\" ><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr>";
                if (dt1.Rows[i]["OPTINDEX_EXCASN_SUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + dt1.Rows[i]["OPTINDEX_EXCASN_SUM"].ToString() + "</td>";
                else
                    strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";

                if (dt1.Rows[i]["OPSTOCKS_EXCASN_SUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + dt1.Rows[i]["OPSTOCKS_EXCASN_SUM"].ToString() + "</td>";
                else
                    strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";
                strHtml += "</tr></table></td>";

                //////////Total  TO
                strHtml += "<td align=\"center\" ><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr>";
                if (dt1.Rows[i]["TOT_SUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\"  colspan=2 width=\"100%\" style=\"nowrap=nowrap;\">" + dt1.Rows[i]["TOT_SUM"].ToString() + "</td>";
                else
                    strHtml += "<td colspan=2 width=\"100%\" style=\"nowrap=nowrap;\">&nbsp;</td>";

                strHtml += "</tr></table></td>";

                strHtml += "</tr>";
            }





            //////////GROUP TOTAL
            flag = flag + 1;
            strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";

            //////////Client Name
            strHtml += "<td align=\"center\"><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr>";
            strHtml += "<td align=\"left\"  colspan=2 width=\"100%\" style=\"nowrap=nowrap;\"><b>Group Total :</b></td>";
            strHtml += "</tr></table></td>";

            //////////Future
            strHtml += "<td align=\"center\" ><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr>";
            if (dt1.Rows[0]["FUTINDEX_GRPSUM"] != DBNull.Value)
                strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;overflow:x;\">" + dt1.Rows[0]["FUTINDEX_GRPSUM"].ToString() + "</td>";
            else
                strHtml += "<td width=\"50%\">&nbsp;</td>";

            if (dt1.Rows[0]["FUTSTOCKS_GRPSUM"] != DBNull.Value)
                strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;overflow:hidden;\">" + dt1.Rows[0]["FUTSTOCKS_GRPSUM"].ToString() + "</td>";
            else
                strHtml += "<td width=\"50%\">&nbsp;</td>";
            strHtml += "</tr></table></td>";

            //////////Option [Prm TO]
            strHtml += "<td align=\"center\" ><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr>";
            if (dt1.Rows[0]["OPTINDEX_PRM_GRPSUM"] != DBNull.Value)
                strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + dt1.Rows[0]["OPTINDEX_PRM_GRPSUM"].ToString() + "</td>";
            else
                strHtml += "<td width=\"50%\">&nbsp;</td>";

            if (dt1.Rows[0]["OPSTOCKS_PRM_GRPSUM"] != DBNull.Value)
                strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + dt1.Rows[0]["OPSTOCKS_PRM_GRPSUM"].ToString() + "</td>";
            else
                strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";
            strHtml += "</tr></table></td>";

            //////////Option [Strike TO]
            strHtml += "<td align=\"center\" ><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr>";
            if (dt1.Rows[0]["OPTINDEX_STRIKE_GRPSUM"] != DBNull.Value)
                strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + dt1.Rows[0]["OPTINDEX_STRIKE_GRPSUM"].ToString() + "</td>";
            else
                strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";

            if (dt1.Rows[0]["OPSTOCKS_STRIKE_GRPSUM"] != DBNull.Value)
                strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + dt1.Rows[0]["OPSTOCKS_STRIKE_GRPSUM"].ToString() + "</td>";
            else
                strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";
            strHtml += "</tr></table></td>";

            //////////Option [Prm+Strk TO]
            strHtml += "<td align=\"center\" ><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr>";
            if (dt1.Rows[0]["OPTINDEX_PRMSTRIKE_GRPSUM"] != DBNull.Value)
                strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + dt1.Rows[0]["OPTINDEX_PRMSTRIKE_GRPSUM"].ToString() + "</td>";
            else
                strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";

            if (dt1.Rows[0]["OPSTOCKS_PRMSTRIKE_GRPSUM"] != DBNull.Value)
                strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + dt1.Rows[0]["OPSTOCKS_PRMSTRIKE_GRPSUM"].ToString() + "</td>";
            else
                strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";
            strHtml += "</tr></table></td>";

            //////////Future Final Sett.
            strHtml += "<td align=\"center\" ><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr>";
            if (dt1.Rows[0]["FUTINDEX_EXP_GRPSUM"] != DBNull.Value)
                strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + dt1.Rows[0]["FUTINDEX_EXP_GRPSUM"].ToString() + "</td>";
            else
                strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";

            if (dt1.Rows[0]["FUTSTOCKS_EXP_GRPSUM"] != DBNull.Value)
                strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + dt1.Rows[0]["FUTSTOCKS_EXP_GRPSUM"].ToString() + "</td>";
            else
                strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";
            strHtml += "</tr></table></td>";


            //////////Options Fin Settlmnt
            strHtml += "<td align=\"center\" ><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr>";
            if (dt1.Rows[0]["OPTINDEX_EXCASN_GRPSUM"] != DBNull.Value)
                strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + dt1.Rows[0]["OPTINDEX_EXCASN_GRPSUM"].ToString() + "</td>";
            else
                strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";

            if (dt1.Rows[0]["OPSTOCKS_EXCASN_GRPSUM"] != DBNull.Value)
                strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + dt1.Rows[0]["OPSTOCKS_EXCASN_GRPSUM"].ToString() + "</td>";
            else
                strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";
            strHtml += "</tr></table></td>";

            //////////Total  TO
            strHtml += "<td align=\"center\" ><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr>";
            if (dt1.Rows[0]["TOT_GRPSUM"] != DBNull.Value)
                strHtml += "<td align=\"right\"  colspan=2 width=\"100%\" style=\"nowrap=nowrap;\">" + dt1.Rows[0]["TOT_GRPSUM"].ToString() + "</td>";
            else
                strHtml += "<td colspan=2 width=\"100%\" style=\"nowrap=nowrap;\">&nbsp;</td>";

            strHtml += "</tr></table></td>";

            strHtml += "</tr>";


            strHtml += "</table>";
            DIVdisplayPERIOD.InnerHtml = strHtml1;
            display.InnerHtml = strHtml;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Display", "Display(2);", true);


        }
        void htmltable_rpttype_EXCHANGE()
        {
            ds = (DataSet)ViewState["dataset"];
            String strHtml = String.Empty;
            String strHtml1 = String.Empty;
            int colcount = ds.Tables[0].Columns.Count;
            string str = null;
            int flag = 0;
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                str = "[" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "]";
            }


            str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "[ <B>" + ddlrpttype.SelectedItem.Text.ToString().Trim() + "</B> ] Report Period " + oconverter.ArrangeDate2(dtfrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtto.Value.ToString());
            strHtml1 = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtml1 += "<tr><td align=\"left\" colspan=" + colcount + " style=\"color:Blue;\">" + str + "</td></tr></table>";

            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";



            strHtml += "<tr style=\"background-color:lavender ;text-align:left\">";

            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            strHtml += "<td align=\"center\"><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr valign=\"top\"><td colspan=2 width=\"100%\" align=\"center\"><b>Date</b></td></tr></table></td>";
            strHtml += "<td align=\"center\"><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr><td colspan=2 width=\"100%\" align=\"center\"><b>Future</b></td></tr><tr><td width=\"50%\" align=\"center\"><b>Index </b></td><td width=\"50%\" align=\"center\"><b>  Stocks </b></td></tr></table></td>";
            strHtml += "<td align=\"center\"><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr><td colspan=2 width=\"100%\" align=\"center\"><b>Option [Prm TO]</b></td></tr><tr><td width=\"50%\" align=\"center\"><b>Index </b></td><td width=\"50%\" align=\"center\"><b>  Stocks </b></td></tr></table></td>";
            strHtml += "<td align=\"center\"><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr><td colspan=2 width=\"100%\" align=\"center\"><b>Option [Strike TO]</b></td></tr><tr><td width=\"50%\" align=\"center\"><b>Index </b></td><td width=\"50%\" align=\"center\"><b>  Stocks </b></td></tr></table></td>";
            strHtml += "<td align=\"center\"><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr><td colspan=2 width=\"100%\" align=\"center\"><b>Option [Prm+Strk TO]</b></td></tr><tr><td width=\"50%\" align=\"center\"><b>Index </b></td><td width=\"50%\" align=\"center\"><b>  Stocks </b></td></tr></table></td>";
            strHtml += "<td align=\"center\"><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr><td colspan=2 width=\"100%\" align=\"center\"><b>Future Final Sett.</b></td></tr><tr><td width=\"50%\" align=\"center\"><b>Index </b></td><td width=\"50%\" align=\"center\"><b>  Stocks </b></td></tr></table></td>";
            strHtml += "<td align=\"center\"><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr><td colspan=2 width=\"100%\" align=\"center\"><b>Options Fin Settlmnt</b></td></tr><tr><td width=\"50%\" align=\"center\"><b>Index </b></td><td width=\"50%\" align=\"center\"><b>  Stocks </b></td></tr></table></td>";
            strHtml += "<td align=\"center\"><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr valign=\"top\"><td colspan=2 width=\"100%\" align=\"center\"><b>Total  TO</b></td></tr><tr><td colspan=2 width=\"100%\" align=\"center\"></td></tr></table></td>";
            strHtml += "</tr>";

            DataTable dtExport = new DataTable();
            dtExport.Clear();
            dtExport.Columns.Add("Date", Type.GetType("System.String"));
            dtExport.Columns.Add("Future Index", Type.GetType("System.String"));
            dtExport.Columns.Add("Future Stocks", Type.GetType("System.String"));
            dtExport.Columns.Add("Option [Prm TO] Index", Type.GetType("System.String"));
            dtExport.Columns.Add("Option [Prm TO] Stocks", Type.GetType("System.String"));
            dtExport.Columns.Add("Option [Strike TO] Index", Type.GetType("System.String"));
            dtExport.Columns.Add("Option [Strike TO] Stocks", Type.GetType("System.String"));
            dtExport.Columns.Add("Option [Prm+Strk TO] Index", Type.GetType("System.String"));
            dtExport.Columns.Add("Option [Prm+Strk TO] Stocks", Type.GetType("System.String"));
            dtExport.Columns.Add("Future Final Sett. Index", Type.GetType("System.String"));
            dtExport.Columns.Add("Future Final Sett. Stocks", Type.GetType("System.String"));
            dtExport.Columns.Add("Options Fin Settlmnt Index", Type.GetType("System.String"));
            dtExport.Columns.Add("Options Fin Settlmnt Stocks", Type.GetType("System.String"));
            dtExport.Columns.Add("Total  TO", Type.GetType("System.String"));


            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";


                //////////Date
                strHtml += "<td align=\"center\"><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr>";
                strHtml += "<td align=\"center\"  colspan=2 width=\"100%\" style=\"nowrap=nowrap;\">" + ds.Tables[0].Rows[i]["TRADEDATE"].ToString() + "</td>";
                strHtml += "</tr></table></td>";


                //////////Future
                strHtml += "<td align=\"center\" ><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr>";
                if (ds.Tables[0].Rows[i]["FUTINDEX"] != DBNull.Value)
                    strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;overflow:x;\">" + ds.Tables[0].Rows[i]["FUTINDEX"].ToString() + "</td>";
                else
                    strHtml += "<td width=\"50%\">&nbsp;</td>";

                if (ds.Tables[0].Rows[i]["FUTSTOCKS"] != DBNull.Value)
                    strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;overflow:hidden;\">" + ds.Tables[0].Rows[i]["FUTSTOCKS"].ToString() + "</td>";
                else
                    strHtml += "<td width=\"50%\">&nbsp;</td>";
                strHtml += "</tr></table></td>";

                //////////Option [Prm TO]
                strHtml += "<td align=\"center\" ><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr>";
                if (ds.Tables[0].Rows[i]["OPTINDEX_PRM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + ds.Tables[0].Rows[i]["OPTINDEX_PRM"].ToString() + "</td>";
                else
                    strHtml += "<td width=\"50%\">&nbsp;</td>";

                if (ds.Tables[0].Rows[i]["OPSTOCKS_PRM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + ds.Tables[0].Rows[i]["OPSTOCKS_PRM"].ToString() + "</td>";
                else
                    strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";
                strHtml += "</tr></table></td>";

                //////////Option [Strike TO]
                strHtml += "<td align=\"center\" ><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr>";
                if (ds.Tables[0].Rows[i]["OPTINDEX_STRIKE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + ds.Tables[0].Rows[i]["OPTINDEX_STRIKE"].ToString() + "</td>";
                else
                    strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";

                if (ds.Tables[0].Rows[i]["OPSTOCKS_STRIKE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + ds.Tables[0].Rows[i]["OPSTOCKS_STRIKE"].ToString() + "</td>";
                else
                    strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";
                strHtml += "</tr></table></td>";

                //////////Option [Prm+Strk TO]
                strHtml += "<td align=\"center\" ><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr>";
                if (ds.Tables[0].Rows[i]["OPTINDEX_PRMSTRIKE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + ds.Tables[0].Rows[i]["OPTINDEX_PRMSTRIKE"].ToString() + "</td>";
                else
                    strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";

                if (ds.Tables[0].Rows[i]["OPSTOCKS_PRMSTRIKE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + ds.Tables[0].Rows[i]["OPSTOCKS_PRMSTRIKE"].ToString() + "</td>";
                else
                    strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";
                strHtml += "</tr></table></td>";

                //////////Future Final Sett.
                strHtml += "<td align=\"center\" ><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr>";
                if (ds.Tables[0].Rows[i]["FUTINDEX_EXP"] != DBNull.Value)
                    strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + ds.Tables[0].Rows[i]["FUTINDEX_EXP"].ToString() + "</td>";
                else
                    strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";

                if (ds.Tables[0].Rows[i]["FUTSTOCKS_EXP"] != DBNull.Value)
                    strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + ds.Tables[0].Rows[i]["FUTSTOCKS_EXP"].ToString() + "</td>";
                else
                    strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";
                strHtml += "</tr></table></td>";


                //////////Options Fin Settlmnt
                strHtml += "<td align=\"center\" ><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr>";
                if (ds.Tables[0].Rows[i]["OPTINDEX_EXCASN"] != DBNull.Value)
                    strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + ds.Tables[0].Rows[i]["OPTINDEX_EXCASN"].ToString() + "</td>";
                else
                    strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";

                if (ds.Tables[0].Rows[i]["OPSTOCKS_EXCASN"] != DBNull.Value)
                    strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + ds.Tables[0].Rows[i]["OPSTOCKS_EXCASN"].ToString() + "</td>";
                else
                    strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";
                strHtml += "</tr></table></td>";

                //////////Total  TO
                strHtml += "<td align=\"center\" ><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr>";
                if (ds.Tables[0].Rows[i]["TOT"] != DBNull.Value)
                    strHtml += "<td align=\"right\"  colspan=2 width=\"100%\" style=\"nowrap=nowrap;\">" + ds.Tables[0].Rows[i]["TOT"].ToString() + "</td>";
                else
                    strHtml += "<td colspan=2 width=\"100%\" style=\"nowrap=nowrap;\">&nbsp;</td>";

                strHtml += "</tr></table></td>";

                strHtml += "</tr>";

                DataRow rowDETAIL = dtExport.NewRow();
                rowDETAIL["Date"] = ds.Tables[0].Rows[i]["TRADEDATE"].ToString();

                if (ds.Tables[0].Rows[i]["FUTINDEX"] != DBNull.Value)
                    rowDETAIL["Future Index"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[i]["FUTINDEX"].ToString()));
                if (ds.Tables[0].Rows[i]["FUTSTOCKS"] != DBNull.Value)
                    rowDETAIL["Future Stocks"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[i]["FUTSTOCKS"].ToString()));
                if (ds.Tables[0].Rows[i]["OPTINDEX_PRM"] != DBNull.Value)
                    rowDETAIL["Option [Prm TO] Index"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[i]["OPTINDEX_PRM"].ToString()));
                if (ds.Tables[0].Rows[i]["OPSTOCKS_PRM"] != DBNull.Value)
                    rowDETAIL["Option [Prm TO] Stocks"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[i]["OPSTOCKS_PRM"].ToString()));
                if (ds.Tables[0].Rows[i]["OPTINDEX_STRIKE"] != DBNull.Value)
                    rowDETAIL["Option [Strike TO] Index"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[i]["OPTINDEX_STRIKE"].ToString()));
                if (ds.Tables[0].Rows[i]["OPSTOCKS_STRIKE"] != DBNull.Value)
                    rowDETAIL["Option [Strike TO] Stocks"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[i]["OPSTOCKS_STRIKE"].ToString()));
                if (ds.Tables[0].Rows[i]["OPTINDEX_PRMSTRIKE"] != DBNull.Value)
                    rowDETAIL["Option [Prm+Strk TO] Index"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[i]["OPTINDEX_PRMSTRIKE"].ToString()));
                if (ds.Tables[0].Rows[i]["OPSTOCKS_PRMSTRIKE"] != DBNull.Value)
                    rowDETAIL["Option [Prm+Strk TO] Stocks"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[i]["OPSTOCKS_PRMSTRIKE"].ToString()));
                if (ds.Tables[0].Rows[i]["FUTINDEX_EXP"] != DBNull.Value)
                    rowDETAIL["Future Final Sett. Index"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[i]["FUTINDEX_EXP"].ToString()));
                if (ds.Tables[0].Rows[i]["FUTSTOCKS_EXP"] != DBNull.Value)
                    rowDETAIL["Future Final Sett. Stocks"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[i]["FUTSTOCKS_EXP"].ToString()));
                if (ds.Tables[0].Rows[i]["OPTINDEX_EXCASN"] != DBNull.Value)
                    rowDETAIL["Options Fin Settlmnt Index"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[i]["OPTINDEX_EXCASN"].ToString()));
                if (ds.Tables[0].Rows[i]["OPSTOCKS_EXCASN"] != DBNull.Value)
                    rowDETAIL["Options Fin Settlmnt Stocks"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[i]["OPSTOCKS_EXCASN"].ToString()));
                if (ds.Tables[0].Rows[i]["TOT"] != DBNull.Value)
                    rowDETAIL["Total  TO"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[i]["TOT"].ToString()));

                dtExport.Rows.Add(rowDETAIL);

            }

            ////////// TOTAL
            flag = flag + 1;
            strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";


            //////////Date
            strHtml += "<td align=\"center\"><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr>";
            strHtml += "<td align=\"center\"  colspan=2 width=\"100%\" style=\"nowrap=nowrap;color:maroon;\"><b>Total :</b></td>";
            strHtml += "</tr></table></td>";


            //////////Future
            strHtml += "<td align=\"center\" ><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr>";
            if (ds.Tables[0].Rows[0]["FUTINDEX_SUM"] != DBNull.Value)
                strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;overflow:x;\">" + ds.Tables[0].Rows[0]["FUTINDEX_SUM"].ToString() + "</td>";
            else
                strHtml += "<td width=\"50%\">&nbsp;</td>";

            if (ds.Tables[0].Rows[0]["FUTSTOCKS_SUM"] != DBNull.Value)
                strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;overflow:hidden;\">" + ds.Tables[0].Rows[0]["FUTSTOCKS_SUM"].ToString() + "</td>";
            else
                strHtml += "<td width=\"50%\">&nbsp;</td>";
            strHtml += "</tr></table></td>";

            //////////Option [Prm TO]
            strHtml += "<td align=\"center\" ><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr>";
            if (ds.Tables[0].Rows[0]["OPTINDEX_PRM_SUM"] != DBNull.Value)
                strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + ds.Tables[0].Rows[0]["OPTINDEX_PRM_SUM"].ToString() + "</td>";
            else
                strHtml += "<td width=\"50%\">&nbsp;</td>";

            if (ds.Tables[0].Rows[0]["OPSTOCKS_PRM_SUM"] != DBNull.Value)
                strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + ds.Tables[0].Rows[0]["OPSTOCKS_PRM_SUM"].ToString() + "</td>";
            else
                strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";
            strHtml += "</tr></table></td>";

            //////////Option [Strike TO]
            strHtml += "<td align=\"center\" ><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr>";
            if (ds.Tables[0].Rows[0]["OPTINDEX_STRIKE_SUM"] != DBNull.Value)
                strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + ds.Tables[0].Rows[0]["OPTINDEX_STRIKE_SUM"].ToString() + "</td>";
            else
                strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";

            if (ds.Tables[0].Rows[0]["OPSTOCKS_STRIKE_SUM"] != DBNull.Value)
                strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + ds.Tables[0].Rows[0]["OPSTOCKS_STRIKE_SUM"].ToString() + "</td>";
            else
                strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";
            strHtml += "</tr></table></td>";

            //////////Option [Prm+Strk TO]
            strHtml += "<td align=\"center\" ><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr>";
            if (ds.Tables[0].Rows[0]["OPTINDEX_PRMSTRIKE_SUM"] != DBNull.Value)
                strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + ds.Tables[0].Rows[0]["OPTINDEX_PRMSTRIKE_SUM"].ToString() + "</td>";
            else
                strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";

            if (ds.Tables[0].Rows[0]["OPSTOCKS_PRMSTRIKE_SUM"] != DBNull.Value)
                strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + ds.Tables[0].Rows[0]["OPSTOCKS_PRMSTRIKE_SUM"].ToString() + "</td>";
            else
                strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";
            strHtml += "</tr></table></td>";

            //////////Future Final Sett.
            strHtml += "<td align=\"center\" ><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr>";
            if (ds.Tables[0].Rows[0]["FUTINDEX_EXP_SUM"] != DBNull.Value)
                strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + ds.Tables[0].Rows[0]["FUTINDEX_EXP_SUM"].ToString() + "</td>";
            else
                strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";

            if (ds.Tables[0].Rows[0]["FUTSTOCKS_EXP_SUM"] != DBNull.Value)
                strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + ds.Tables[0].Rows[0]["FUTSTOCKS_EXP_SUM"].ToString() + "</td>";
            else
                strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";
            strHtml += "</tr></table></td>";


            //////////Options Fin Settlmnt
            strHtml += "<td align=\"center\" ><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr>";
            if (ds.Tables[0].Rows[0]["OPTINDEX_EXCASN_SUM"] != DBNull.Value)
                strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + ds.Tables[0].Rows[0]["OPTINDEX_EXCASN_SUM"].ToString() + "</td>";
            else
                strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";

            if (ds.Tables[0].Rows[0]["OPSTOCKS_EXCASN_SUM"] != DBNull.Value)
                strHtml += "<td align=\"right\" width=\"50%\" style=\"nowrap=nowrap;\">" + ds.Tables[0].Rows[0]["OPSTOCKS_EXCASN_SUM"].ToString() + "</td>";
            else
                strHtml += "<td width=\"50%\" style=\"nowrap=nowrap;\">&nbsp;</td>";
            strHtml += "</tr></table></td>";

            //////////Total  TO
            strHtml += "<td align=\"center\" ><table  width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr>";
            if (ds.Tables[0].Rows[0]["TOT_SUM"] != DBNull.Value)
                strHtml += "<td align=\"right\"  colspan=2 width=\"100%\" style=\"nowrap=nowrap;\">" + ds.Tables[0].Rows[0]["TOT_SUM"].ToString() + "</td>";
            else
                strHtml += "<td colspan=2 width=\"100%\" style=\"nowrap=nowrap;\">&nbsp;</td>";

            strHtml += "</tr></table></td>";

            strHtml += "</tr>";




            strHtml += "</table>";
            DataRow row2 = dtExport.NewRow();
            row2["Date"] = "Total :";

            if (ds.Tables[0].Rows[0]["FUTINDEX_SUM"] != DBNull.Value)
                row2["Future Index"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[0]["FUTINDEX_SUM"].ToString()));
            if (ds.Tables[0].Rows[0]["FUTSTOCKS_SUM"] != DBNull.Value)
                row2["Future Stocks"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[0]["FUTSTOCKS_SUM"].ToString()));
            if (ds.Tables[0].Rows[0]["OPTINDEX_PRM_SUM"] != DBNull.Value)
                row2["Option [Prm TO] Index"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[0]["OPTINDEX_PRM_SUM"].ToString()));
            if (ds.Tables[0].Rows[0]["OPSTOCKS_PRM_SUM"] != DBNull.Value)
                row2["Option [Prm TO] Stocks"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[0]["OPSTOCKS_PRM_SUM"].ToString()));
            if (ds.Tables[0].Rows[0]["OPTINDEX_STRIKE_SUM"] != DBNull.Value)
                row2["Option [Strike TO] Index"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[0]["OPTINDEX_STRIKE_SUM"].ToString()));
            if (ds.Tables[0].Rows[0]["OPSTOCKS_STRIKE_SUM"] != DBNull.Value)
                row2["Option [Strike TO] Stocks"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[0]["OPSTOCKS_STRIKE_SUM"].ToString()));
            if (ds.Tables[0].Rows[0]["OPTINDEX_PRMSTRIKE_SUM"] != DBNull.Value)
                row2["Option [Prm+Strk TO] Index"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[0]["OPTINDEX_PRMSTRIKE_SUM"].ToString()));
            if (ds.Tables[0].Rows[0]["OPSTOCKS_PRMSTRIKE_SUM"] != DBNull.Value)
                row2["Option [Prm+Strk TO] Stocks"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[0]["OPSTOCKS_PRMSTRIKE_SUM"].ToString()));
            if (ds.Tables[0].Rows[0]["FUTINDEX_EXP_SUM"] != DBNull.Value)
                row2["Future Final Sett. Index"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[0]["FUTINDEX_EXP_SUM"].ToString()));
            if (ds.Tables[0].Rows[0]["FUTSTOCKS_EXP_SUM"] != DBNull.Value)
                row2["Future Final Sett. Stocks"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[0]["FUTSTOCKS_EXP_SUM"].ToString()));
            if (ds.Tables[0].Rows[0]["OPTINDEX_EXCASN_SUM"] != DBNull.Value)
                row2["Options Fin Settlmnt Index"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[0]["OPTINDEX_EXCASN_SUM"].ToString()));
            if (ds.Tables[0].Rows[0]["OPSTOCKS_EXCASN_SUM"] != DBNull.Value)
                row2["Options Fin Settlmnt Stocks"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[0]["OPSTOCKS_EXCASN_SUM"].ToString()));
            if (ds.Tables[0].Rows[0]["TOT_SUM"] != DBNull.Value)
                row2["Total  TO"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[0]["TOT_SUM"].ToString()));
            dtExport.Rows.Add(row2);



            ViewState["dtExport"] = dtExport;

            DIVdisplayPERIOD.InnerHtml = strHtml1;
            display.InnerHtml = strHtml;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Display", "Display(3);", true);
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
            if (ddlrpttype.SelectedItem.Value.ToString().Trim() == "1")
            {
                CurrentPage = 0;
                ddlbandforClient();
            }
            else
            {
                htmltable_rpttype_Consolidated();
            }
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
            if (ddlrpttype.SelectedItem.Value.ToString().Trim() == "1")
            {
                CurrentPage = 0;
                ddlbandforClient();
            }
            else
            {
                htmltable_rpttype_Consolidated();
            }
        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            export();
        }
        void export()
        {
            if (ddlrptfor.SelectedItem.Value.ToString().Trim() == "0")
            {
                if (ddlrpttype.SelectedItem.Value.ToString().Trim() == "0")
                {
                    EXPORT_clientwise_consolidated();
                }
                else
                {
                    EXPORT_clientwise_datewise();
                }
            }
            headerfotter();
        }
        void headerfotter()
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
            string str1 = null;

            str = " Report Period " + oconverter.ArrangeDate2(dtfrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtto.Value.ToString());
            if (ddlrptfor.SelectedItem.Value.ToString().Trim() == "0")
            {
                if (ddlGroup.SelectedItem.Value.ToString().Trim() == "0")
                {
                    str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str;

                }
                else
                {
                    str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise [" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "]" + str;

                }
                str1 = " [ Clients Wise " + ddlrpttype.SelectedItem.Text.ToString().Trim() + "]";
            }
            else
            {
                str1 = " [ Exchange Wise ]";
            }
            DrRowR1[0] = str1 + "Turnover Report:" + str;

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
                objExcel.ExportToExcelforExcel(dtExport, "Turnover Report", "Group Total :", dtReportHeader, dtReportFooter);
            }
        }
        void EXPORT_clientwise_consolidated()
        {
            ds = (DataSet)ViewState["dataset"];
            DataTable dtExport = new DataTable();
            dtExport.Clear();
            dtExport.Columns.Add("Client Name", Type.GetType("System.String"));
            dtExport.Columns.Add("Future Index", Type.GetType("System.String"));
            dtExport.Columns.Add("Future Stocks", Type.GetType("System.String"));
            dtExport.Columns.Add("Option [Prm TO] Index", Type.GetType("System.String"));
            dtExport.Columns.Add("Option [Prm TO] Stocks", Type.GetType("System.String"));
            dtExport.Columns.Add("Option [Strike TO] Index", Type.GetType("System.String"));
            dtExport.Columns.Add("Option [Strike TO] Stocks", Type.GetType("System.String"));
            dtExport.Columns.Add("Option [Prm+Strk TO] Index", Type.GetType("System.String"));
            dtExport.Columns.Add("Option [Prm+Strk TO] Stocks", Type.GetType("System.String"));
            dtExport.Columns.Add("Future Final Sett. Index", Type.GetType("System.String"));
            dtExport.Columns.Add("Future Final Sett. Stocks", Type.GetType("System.String"));
            dtExport.Columns.Add("Options Fin Settlmnt Index", Type.GetType("System.String"));
            dtExport.Columns.Add("Options Fin Settlmnt Stocks", Type.GetType("System.String"));
            dtExport.Columns.Add("Total  TO", Type.GetType("System.String"));


            for (int j = 0; j < cmbgroup.Items.Count; j++)
            {
                DataRow row = dtExport.NewRow();
                row["Client Name"] = ddlGroup.SelectedItem.Text.ToString().Trim() + " Name:" + cmbgroup.Items[j].Text.ToString().Trim();
                row["Future Index"] = "Test";
                dtExport.Rows.Add(row);

                DataView viewgrp = new DataView();
                viewgrp = ds.Tables[0].DefaultView;
                viewgrp.RowFilter = "GRPID='" + cmbgroup.Items[j].Value + "'";
                DataTable dt1 = new DataTable();
                dt1 = viewgrp.ToTable();

                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    DataRow row1 = dtExport.NewRow();
                    row1["Client Name"] = dt1.Rows[i]["CLIENTNAME"].ToString() + "[" + dt1.Rows[i]["UCC"].ToString() + "]";

                    if (dt1.Rows[i]["FUTINDEX_SUM"] != DBNull.Value)
                        row1["Future Index"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["FUTINDEX_SUM"].ToString()));
                    if (dt1.Rows[i]["FUTSTOCKS_SUM"] != DBNull.Value)
                        row1["Future Stocks"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["FUTSTOCKS_SUM"].ToString()));
                    if (dt1.Rows[i]["OPTINDEX_PRM_SUM"] != DBNull.Value)
                        row1["Option [Prm TO] Index"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["OPTINDEX_PRM_SUM"].ToString()));
                    if (dt1.Rows[i]["OPSTOCKS_PRM_SUM"] != DBNull.Value)
                        row1["Option [Prm TO] Stocks"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["OPSTOCKS_PRM_SUM"].ToString()));
                    if (dt1.Rows[i]["OPTINDEX_STRIKE_SUM"] != DBNull.Value)
                        row1["Option [Strike TO] Index"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["OPTINDEX_STRIKE_SUM"].ToString()));
                    if (dt1.Rows[i]["OPSTOCKS_STRIKE_SUM"] != DBNull.Value)
                        row1["Option [Strike TO] Stocks"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["OPSTOCKS_STRIKE_SUM"].ToString()));
                    if (dt1.Rows[i]["OPTINDEX_PRMSTRIKE_SUM"] != DBNull.Value)
                        row1["Option [Prm+Strk TO] Index"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["OPTINDEX_PRMSTRIKE_SUM"].ToString()));
                    if (dt1.Rows[i]["OPSTOCKS_PRMSTRIKE_SUM"] != DBNull.Value)
                        row1["Option [Prm+Strk TO] Stocks"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["OPSTOCKS_PRMSTRIKE_SUM"].ToString()));
                    if (dt1.Rows[i]["FUTINDEX_EXP_SUM"] != DBNull.Value)
                        row1["Future Final Sett. Index"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["FUTINDEX_EXP_SUM"].ToString()));
                    if (dt1.Rows[i]["FUTSTOCKS_EXP_SUM"] != DBNull.Value)
                        row1["Future Final Sett. Stocks"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["FUTSTOCKS_EXP_SUM"].ToString()));
                    if (dt1.Rows[i]["OPTINDEX_EXCASN_SUM"] != DBNull.Value)
                        row1["Options Fin Settlmnt Index"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["OPTINDEX_EXCASN_SUM"].ToString()));
                    if (dt1.Rows[i]["OPSTOCKS_EXCASN_SUM"] != DBNull.Value)
                        row1["Options Fin Settlmnt Stocks"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["OPSTOCKS_EXCASN_SUM"].ToString()));
                    if (dt1.Rows[i]["TOT_SUM"] != DBNull.Value)
                        row1["Total  TO"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["TOT_SUM"].ToString()));

                    dtExport.Rows.Add(row1);
                }

                DataRow row2 = dtExport.NewRow();
                row2["Client Name"] = "Group Total :";

                if (dt1.Rows[0]["FUTINDEX_GRPSUM"] != DBNull.Value)
                    row2["Future Index"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["FUTINDEX_GRPSUM"].ToString()));
                if (dt1.Rows[0]["FUTSTOCKS_GRPSUM"] != DBNull.Value)
                    row2["Future Stocks"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["FUTSTOCKS_GRPSUM"].ToString()));
                if (dt1.Rows[0]["OPTINDEX_PRM_GRPSUM"] != DBNull.Value)
                    row2["Option [Prm TO] Index"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["OPTINDEX_PRM_GRPSUM"].ToString()));
                if (dt1.Rows[0]["OPSTOCKS_PRM_GRPSUM"] != DBNull.Value)
                    row2["Option [Prm TO] Stocks"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["OPSTOCKS_PRM_GRPSUM"].ToString()));
                if (dt1.Rows[0]["OPTINDEX_STRIKE_GRPSUM"] != DBNull.Value)
                    row2["Option [Strike TO] Index"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["OPTINDEX_STRIKE_GRPSUM"].ToString()));
                if (dt1.Rows[0]["OPSTOCKS_STRIKE_GRPSUM"] != DBNull.Value)
                    row2["Option [Strike TO] Stocks"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["OPSTOCKS_STRIKE_GRPSUM"].ToString()));
                if (dt1.Rows[0]["OPTINDEX_PRMSTRIKE_GRPSUM"] != DBNull.Value)
                    row2["Option [Prm+Strk TO] Index"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["OPTINDEX_PRMSTRIKE_GRPSUM"].ToString()));
                if (dt1.Rows[0]["OPSTOCKS_PRMSTRIKE_GRPSUM"] != DBNull.Value)
                    row2["Option [Prm+Strk TO] Stocks"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["OPSTOCKS_PRMSTRIKE_GRPSUM"].ToString()));
                if (dt1.Rows[0]["FUTINDEX_EXP_GRPSUM"] != DBNull.Value)
                    row2["Future Final Sett. Index"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["FUTINDEX_EXP_GRPSUM"].ToString()));
                if (dt1.Rows[0]["FUTSTOCKS_EXP_GRPSUM"] != DBNull.Value)
                    row2["Future Final Sett. Stocks"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["FUTSTOCKS_EXP_GRPSUM"].ToString()));
                if (dt1.Rows[0]["OPTINDEX_EXCASN_GRPSUM"] != DBNull.Value)
                    row2["Options Fin Settlmnt Index"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["OPTINDEX_EXCASN_GRPSUM"].ToString()));
                if (dt1.Rows[0]["OPSTOCKS_EXCASN_GRPSUM"] != DBNull.Value)
                    row2["Options Fin Settlmnt Stocks"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["OPSTOCKS_EXCASN_GRPSUM"].ToString()));
                if (dt1.Rows[0]["TOT_GRPSUM"] != DBNull.Value)
                    row2["Total  TO"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["TOT_GRPSUM"].ToString()));
                dtExport.Rows.Add(row2);
            }
            DataRow RowGrandTotal = dtExport.NewRow();
            RowGrandTotal["Client Name"] = "Grand Total :";
            if (ds.Tables[0].Rows[0]["FUTINDEX_GrandSUM"] != DBNull.Value)
                RowGrandTotal["Future Index"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[0]["FUTINDEX_GrandSUM"].ToString()));
            if (ds.Tables[0].Rows[0]["FUTSTOCKS_GrandSUM"] != DBNull.Value)
                RowGrandTotal["Future Stocks"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[0]["FUTSTOCKS_GrandSUM"].ToString()));
            if (ds.Tables[0].Rows[0]["OPTINDEX_PRM_GrandSUM"] != DBNull.Value)
                RowGrandTotal["Option [Prm TO] Index"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[0]["OPTINDEX_PRM_GrandSUM"].ToString()));
            if (ds.Tables[0].Rows[0]["OPSTOCKS_PRM_GrandSUM"] != DBNull.Value)
                RowGrandTotal["Option [Prm TO] Stocks"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[0]["OPSTOCKS_PRM_GrandSUM"].ToString()));
            if (ds.Tables[0].Rows[0]["OPTINDEX_STRIKE_GrandSUM"] != DBNull.Value)
                RowGrandTotal["Option [Strike TO] Index"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[0]["OPTINDEX_STRIKE_GrandSUM"].ToString()));
            if (ds.Tables[0].Rows[0]["OPSTOCKS_STRIKE_GrandSUM"] != DBNull.Value)
                RowGrandTotal["Option [Strike TO] Stocks"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[0]["OPSTOCKS_STRIKE_GrandSUM"].ToString()));
            if (ds.Tables[0].Rows[0]["OPTINDEX_PRMSTRIKE_GrandSUM"] != DBNull.Value)
                RowGrandTotal["Option [Prm+Strk TO] Index"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[0]["OPTINDEX_PRMSTRIKE_GrandSUM"].ToString()));
            if (ds.Tables[0].Rows[0]["OPSTOCKS_PRMSTRIKE_GrandSUM"] != DBNull.Value)
                RowGrandTotal["Option [Prm+Strk TO] Stocks"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[0]["OPSTOCKS_PRMSTRIKE_GrandSUM"].ToString()));
            if (ds.Tables[0].Rows[0]["FUTINDEX_EXP_GrandSUM"] != DBNull.Value)
                RowGrandTotal["Future Final Sett. Index"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[0]["FUTINDEX_EXP_GrandSUM"].ToString()));
            if (ds.Tables[0].Rows[0]["FUTSTOCKS_EXP_GrandSUM"] != DBNull.Value)
                RowGrandTotal["Future Final Sett. Stocks"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[0]["FUTSTOCKS_EXP_GrandSUM"].ToString()));
            if (ds.Tables[0].Rows[0]["OPTINDEX_EXCASN_GrandSUM"] != DBNull.Value)
                RowGrandTotal["Options Fin Settlmnt Index"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[0]["OPTINDEX_EXCASN_GrandSUM"].ToString()));
            if (ds.Tables[0].Rows[0]["OPSTOCKS_EXCASN_GrandSUM"] != DBNull.Value)
                RowGrandTotal["Options Fin Settlmnt Stocks"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[0]["OPSTOCKS_EXCASN_GrandSUM"].ToString()));
            if (ds.Tables[0].Rows[0]["TOT_GrandSUM"] != DBNull.Value)
                RowGrandTotal["Total  TO"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[0]["TOT_GrandSUM"].ToString()));
            dtExport.Rows.Add(RowGrandTotal);
            ViewState["dtExport"] = dtExport;
        }

        void EXPORT_clientwise_datewise()
        {
            ds = (DataSet)ViewState["dataset"];
            DataTable dtExport = new DataTable();
            dtExport.Clear();
            dtExport.Columns.Add("Date", Type.GetType("System.String"));
            dtExport.Columns.Add("Future Index", Type.GetType("System.String"));
            dtExport.Columns.Add("Future Stocks", Type.GetType("System.String"));
            dtExport.Columns.Add("Option [Prm TO] Index", Type.GetType("System.String"));
            dtExport.Columns.Add("Option [Prm TO] Stocks", Type.GetType("System.String"));
            dtExport.Columns.Add("Option [Strike TO] Index", Type.GetType("System.String"));
            dtExport.Columns.Add("Option [Strike TO] Stocks", Type.GetType("System.String"));
            dtExport.Columns.Add("Option [Prm+Strk TO] Index", Type.GetType("System.String"));
            dtExport.Columns.Add("Option [Prm+Strk TO] Stocks", Type.GetType("System.String"));
            dtExport.Columns.Add("Future Final Sett. Index", Type.GetType("System.String"));
            dtExport.Columns.Add("Future Final Sett. Stocks", Type.GetType("System.String"));
            dtExport.Columns.Add("Options Fin Settlmnt Index", Type.GetType("System.String"));
            dtExport.Columns.Add("Options Fin Settlmnt Stocks", Type.GetType("System.String"));
            dtExport.Columns.Add("Total  TO", Type.GetType("System.String"));


            for (int j = 0; j < cmbgroup.Items.Count; j++)
            {
                DataRow row = dtExport.NewRow();
                row["Date"] = ddlGroup.SelectedItem.Text.ToString().Trim() + " Name:" + cmbgroup.Items[j].Text.ToString().Trim();
                row["Future Index"] = "Test";
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
                    DataRow rowCLIENT = dtExport.NewRow();
                    rowCLIENT["Date"] = "Client Name:" + cmbclient.Items[k].Text.ToString().Trim();
                    rowCLIENT["Future Index"] = "Test";
                    dtExport.Rows.Add(rowCLIENT);


                    DataView viewclient = new DataView();
                    viewclient = ds.Tables[0].DefaultView;
                    viewclient.RowFilter = "CUSTOMERID='" + cmbclient.Items[k].Value + "'";
                    DataTable dt1 = new DataTable();
                    dt1 = viewclient.ToTable();


                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        DataRow rowDETAIL = dtExport.NewRow();
                        rowDETAIL["Date"] = dt1.Rows[i]["TRADEDATE"].ToString();

                        if (dt1.Rows[i]["FUTINDEX"] != DBNull.Value)
                            rowDETAIL["Future Index"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["FUTINDEX"].ToString()));
                        if (dt1.Rows[i]["FUTSTOCKS"] != DBNull.Value)
                            rowDETAIL["Future Stocks"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["FUTSTOCKS"].ToString()));
                        if (dt1.Rows[i]["OPTINDEX_PRM"] != DBNull.Value)
                            rowDETAIL["Option [Prm TO] Index"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["OPTINDEX_PRM"].ToString()));
                        if (dt1.Rows[i]["OPSTOCKS_PRM"] != DBNull.Value)
                            rowDETAIL["Option [Prm TO] Stocks"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["OPSTOCKS_PRM"].ToString()));
                        if (dt1.Rows[i]["OPTINDEX_STRIKE"] != DBNull.Value)
                            rowDETAIL["Option [Strike TO] Index"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["OPTINDEX_STRIKE"].ToString()));
                        if (dt1.Rows[i]["OPSTOCKS_STRIKE"] != DBNull.Value)
                            rowDETAIL["Option [Strike TO] Stocks"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["OPSTOCKS_STRIKE"].ToString()));
                        if (dt1.Rows[i]["OPTINDEX_PRMSTRIKE"] != DBNull.Value)
                            rowDETAIL["Option [Prm+Strk TO] Index"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["OPTINDEX_PRMSTRIKE"].ToString()));
                        if (dt1.Rows[i]["OPSTOCKS_PRMSTRIKE"] != DBNull.Value)
                            rowDETAIL["Option [Prm+Strk TO] Stocks"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["OPSTOCKS_PRMSTRIKE"].ToString()));
                        if (dt1.Rows[i]["FUTINDEX_EXP"] != DBNull.Value)
                            rowDETAIL["Future Final Sett. Index"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["FUTINDEX_EXP"].ToString()));
                        if (dt1.Rows[i]["FUTSTOCKS_EXP"] != DBNull.Value)
                            rowDETAIL["Future Final Sett. Stocks"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["FUTSTOCKS_EXP"].ToString()));
                        if (dt1.Rows[i]["OPTINDEX_EXCASN"] != DBNull.Value)
                            rowDETAIL["Options Fin Settlmnt Index"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["OPTINDEX_EXCASN"].ToString()));
                        if (dt1.Rows[i]["OPSTOCKS_EXCASN"] != DBNull.Value)
                            rowDETAIL["Options Fin Settlmnt Stocks"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["OPSTOCKS_EXCASN"].ToString()));
                        if (dt1.Rows[i]["TOT"] != DBNull.Value)
                            rowDETAIL["Total  TO"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["TOT"].ToString()));

                        dtExport.Rows.Add(rowDETAIL);
                    }
                    DataRow row1 = dtExport.NewRow();
                    row1["Date"] = "Client Total :";

                    if (dt1.Rows[0]["FUTINDEX_SUM"] != DBNull.Value)
                        row1["Future Index"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["FUTINDEX_SUM"].ToString()));
                    if (dt1.Rows[0]["FUTSTOCKS_SUM"] != DBNull.Value)
                        row1["Future Stocks"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["FUTSTOCKS_SUM"].ToString()));
                    if (dt1.Rows[0]["OPTINDEX_PRM_SUM"] != DBNull.Value)
                        row1["Option [Prm TO] Index"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["OPTINDEX_PRM_SUM"].ToString()));
                    if (dt1.Rows[0]["OPSTOCKS_PRM_SUM"] != DBNull.Value)
                        row1["Option [Prm TO] Stocks"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["OPSTOCKS_PRM_SUM"].ToString()));
                    if (dt1.Rows[0]["OPTINDEX_STRIKE_SUM"] != DBNull.Value)
                        row1["Option [Strike TO] Index"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["OPTINDEX_STRIKE_SUM"].ToString()));
                    if (dt1.Rows[0]["OPSTOCKS_STRIKE_SUM"] != DBNull.Value)
                        row1["Option [Strike TO] Stocks"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["OPSTOCKS_STRIKE_SUM"].ToString()));
                    if (dt1.Rows[0]["OPTINDEX_PRMSTRIKE_SUM"] != DBNull.Value)
                        row1["Option [Prm+Strk TO] Index"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["OPTINDEX_PRMSTRIKE_SUM"].ToString()));
                    if (dt1.Rows[0]["OPSTOCKS_PRMSTRIKE_SUM"] != DBNull.Value)
                        row1["Option [Prm+Strk TO] Stocks"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["OPSTOCKS_PRMSTRIKE_SUM"].ToString()));
                    if (dt1.Rows[0]["FUTINDEX_EXP_SUM"] != DBNull.Value)
                        row1["Future Final Sett. Index"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["FUTINDEX_EXP_SUM"].ToString()));
                    if (dt1.Rows[0]["FUTSTOCKS_EXP_SUM"] != DBNull.Value)
                        row1["Future Final Sett. Stocks"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["FUTSTOCKS_EXP_SUM"].ToString()));
                    if (dt1.Rows[0]["OPTINDEX_EXCASN_SUM"] != DBNull.Value)
                        row1["Options Fin Settlmnt Index"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["OPTINDEX_EXCASN_SUM"].ToString()));
                    if (dt1.Rows[0]["OPSTOCKS_EXCASN_SUM"] != DBNull.Value)
                        row1["Options Fin Settlmnt Stocks"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["OPSTOCKS_EXCASN_SUM"].ToString()));
                    if (dt1.Rows[0]["TOT_SUM"] != DBNull.Value)
                        row1["Total  TO"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["TOT_SUM"].ToString()));

                    dtExport.Rows.Add(row1);
                }

                DataRow row2 = dtExport.NewRow();
                row2["Date"] = "Group Total :";

                if (dt.Rows[0]["FUTINDEX_GRPSUM"] != DBNull.Value)
                    row2["Future Index"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt.Rows[0]["FUTINDEX_GRPSUM"].ToString()));
                if (dt.Rows[0]["FUTSTOCKS_GRPSUM"] != DBNull.Value)
                    row2["Future Stocks"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt.Rows[0]["FUTSTOCKS_GRPSUM"].ToString()));
                if (dt.Rows[0]["OPTINDEX_PRM_GRPSUM"] != DBNull.Value)
                    row2["Option [Prm TO] Index"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt.Rows[0]["OPTINDEX_PRM_GRPSUM"].ToString()));
                if (dt.Rows[0]["OPSTOCKS_PRM_GRPSUM"] != DBNull.Value)
                    row2["Option [Prm TO] Stocks"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt.Rows[0]["OPSTOCKS_PRM_GRPSUM"].ToString()));
                if (dt.Rows[0]["OPTINDEX_STRIKE_GRPSUM"] != DBNull.Value)
                    row2["Option [Strike TO] Index"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt.Rows[0]["OPTINDEX_STRIKE_GRPSUM"].ToString()));
                if (dt.Rows[0]["OPSTOCKS_STRIKE_GRPSUM"] != DBNull.Value)
                    row2["Option [Strike TO] Stocks"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt.Rows[0]["OPSTOCKS_STRIKE_GRPSUM"].ToString()));
                if (dt.Rows[0]["OPTINDEX_PRMSTRIKE_GRPSUM"] != DBNull.Value)
                    row2["Option [Prm+Strk TO] Index"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt.Rows[0]["OPTINDEX_PRMSTRIKE_GRPSUM"].ToString()));
                if (dt.Rows[0]["OPSTOCKS_PRMSTRIKE_GRPSUM"] != DBNull.Value)
                    row2["Option [Prm+Strk TO] Stocks"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt.Rows[0]["OPSTOCKS_PRMSTRIKE_GRPSUM"].ToString()));
                if (dt.Rows[0]["FUTINDEX_EXP_GRPSUM"] != DBNull.Value)
                    row2["Future Final Sett. Index"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt.Rows[0]["FUTINDEX_EXP_GRPSUM"].ToString()));
                if (dt.Rows[0]["FUTSTOCKS_EXP_GRPSUM"] != DBNull.Value)
                    row2["Future Final Sett. Stocks"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt.Rows[0]["FUTSTOCKS_EXP_GRPSUM"].ToString()));
                if (dt.Rows[0]["OPTINDEX_EXCASN_GRPSUM"] != DBNull.Value)
                    row2["Options Fin Settlmnt Index"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt.Rows[0]["OPTINDEX_EXCASN_GRPSUM"].ToString()));
                if (dt.Rows[0]["OPSTOCKS_EXCASN_GRPSUM"] != DBNull.Value)
                    row2["Options Fin Settlmnt Stocks"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt.Rows[0]["OPSTOCKS_EXCASN_GRPSUM"].ToString()));
                if (dt.Rows[0]["TOT_GRPSUM"] != DBNull.Value)
                    row2["Total  TO"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt.Rows[0]["TOT_GRPSUM"].ToString()));
                dtExport.Rows.Add(row2);
            }
            DataRow RowGrandTotal = dtExport.NewRow();
            RowGrandTotal["Date"] = "Grand Total :";
            if (ds.Tables[0].Rows[0]["FUTINDEX_GrandSUM"] != DBNull.Value)
                RowGrandTotal["Future Index"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[0]["FUTINDEX_GrandSUM"].ToString()));
            if (ds.Tables[0].Rows[0]["FUTSTOCKS_GrandSUM"] != DBNull.Value)
                RowGrandTotal["Future Stocks"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[0]["FUTSTOCKS_GrandSUM"].ToString()));
            if (ds.Tables[0].Rows[0]["OPTINDEX_PRM_GrandSUM"] != DBNull.Value)
                RowGrandTotal["Option [Prm TO] Index"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[0]["OPTINDEX_PRM_GrandSUM"].ToString()));
            if (ds.Tables[0].Rows[0]["OPSTOCKS_PRM_GrandSUM"] != DBNull.Value)
                RowGrandTotal["Option [Prm TO] Stocks"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[0]["OPSTOCKS_PRM_GrandSUM"].ToString()));
            if (ds.Tables[0].Rows[0]["OPTINDEX_STRIKE_GrandSUM"] != DBNull.Value)
                RowGrandTotal["Option [Strike TO] Index"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[0]["OPTINDEX_STRIKE_GrandSUM"].ToString()));
            if (ds.Tables[0].Rows[0]["OPSTOCKS_STRIKE_GrandSUM"] != DBNull.Value)
                RowGrandTotal["Option [Strike TO] Stocks"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[0]["OPSTOCKS_STRIKE_GrandSUM"].ToString()));
            if (ds.Tables[0].Rows[0]["OPTINDEX_PRMSTRIKE_GrandSUM"] != DBNull.Value)
                RowGrandTotal["Option [Prm+Strk TO] Index"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[0]["OPTINDEX_PRMSTRIKE_GrandSUM"].ToString()));
            if (ds.Tables[0].Rows[0]["OPSTOCKS_PRMSTRIKE_GrandSUM"] != DBNull.Value)
                RowGrandTotal["Option [Prm+Strk TO] Stocks"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[0]["OPSTOCKS_PRMSTRIKE_GrandSUM"].ToString()));
            if (ds.Tables[0].Rows[0]["FUTINDEX_EXP_GrandSUM"] != DBNull.Value)
                RowGrandTotal["Future Final Sett. Index"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[0]["FUTINDEX_EXP_GrandSUM"].ToString()));
            if (ds.Tables[0].Rows[0]["FUTSTOCKS_EXP_GrandSUM"] != DBNull.Value)
                RowGrandTotal["Future Final Sett. Stocks"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[0]["FUTSTOCKS_EXP_GrandSUM"].ToString()));
            if (ds.Tables[0].Rows[0]["OPTINDEX_EXCASN_GrandSUM"] != DBNull.Value)
                RowGrandTotal["Options Fin Settlmnt Index"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[0]["OPTINDEX_EXCASN_GrandSUM"].ToString()));
            if (ds.Tables[0].Rows[0]["OPSTOCKS_EXCASN_GrandSUM"] != DBNull.Value)
                RowGrandTotal["Options Fin Settlmnt Stocks"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[0]["OPSTOCKS_EXCASN_GrandSUM"].ToString()));
            if (ds.Tables[0].Rows[0]["TOT_GrandSUM"] != DBNull.Value)
                RowGrandTotal["Total  TO"] = oconverter.formatmoneyinUs(Convert.ToDecimal(ds.Tables[0].Rows[0]["TOT_GrandSUM"].ToString()));
            dtExport.Rows.Add(RowGrandTotal);
            ViewState["dtExport"] = dtExport;
        }
    }
}