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
    public partial class management_ObligationStatementFO_New : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        Converter oconverter = new Converter();
        DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        DataSet ds = new DataSet();
        DataTable dtgroupcontactid = new DataTable();
        DataTable Distinctclient = new DataTable();
        ExcelFile objExcel = new ExcelFile();
        DataTable Distinctproduct = new DataTable();
        DataTable dtExport = new DataTable();
        string data;
        int pageindex = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //___________-end here___//
            if (!IsPostBack)
            {
                //DropDownList1.DataSource = System.Drawing.Printing.PrinterSettings.InstalledPrinters;
                //DropDownList1.DataBind();
                if (Request.QueryString["date"] != null)
                {
                    procedureForLedger();
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");
                    date();
                    RateDateFetch();
                }
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
        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            string id = eventArgument.ToString();
            string[] idlist = id.Split('~');
            string[] cl = idlist[1].Split(',');
            string str = "";
            string str1 = "";
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

            if (idlist[0] == "Clients")
            {
                data = "Clients~" + str;
            }
            else if (idlist[0] == "Broker")
            {
                data = "Broker~" + str;
            }
            else if (idlist[0] == "BranchGroup")
            {
                data = "BranchGroup~" + str;
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
            dtfor.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
            dtfrom.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
            dtto.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());

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
        private void fn_grpbranch()
        {
            if (ddlGroup.SelectedItem.Text == "Branch Group")
            {
                DataTable dtbranch = new DataTable();
                string Branch = null;
                if (rdbranchAll.Checked)
                {
                    dtbranch = oDBEngine.GetDataTable("tbl_master_branch", "branch_id", "branch_id in(" + Session["userbranchHierarchy"].ToString() + ") and branch_id in (select distinct branchgroupmembers_branchid from trans_branchgroupmembers)");
                }
                else
                {
                    dtbranch = oDBEngine.GetDataTable("tbl_master_branch", "branch_id", "branch_id in(" + Session["userbranchHierarchy"].ToString() + ") and branch_id in (select distinct branchgroupmembers_branchid from trans_branchgroupmembers where branchgroupmembers_branchgroupid in(" + HiddenField_BranchGroup.Value + "))");
                }
                if (dtbranch.Rows.Count > 0)
                {
                    for (int i = 0; i < dtbranch.Rows.Count; i++)
                    {
                        if (Branch == null)
                            Branch = dtbranch.Rows[i][0].ToString();
                        else
                            Branch += "," + dtbranch.Rows[i][0].ToString();
                    }
                }
                if (Branch != null)
                {
                    HiddenField_Branch.Value = Branch;
                }
            }

        }
        protected void btnshow_Click(object sender, EventArgs e)
        {
            fn_grpbranch();
            procedure();
        }
        void procedure()
        {

            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            {

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                string ExchSegId = Convert.ToString(HttpContext.Current.Session["ExchangeSegmentID"]);
                //if (rbdos.Checked == false)
                //{
                if (ExchSegId == "2" || ExchSegId == "5")
                    cmd.CommandText = "[ObligationStatementFO_NEW]";
                else if (ExchSegId == "3" || ExchSegId == "6" || ExchSegId == "7" || ExchSegId == "8" || ExchSegId == "9" || ExchSegId == "10" || ExchSegId == "11" || ExchSegId == "12" || ExchSegId == "13" || ExchSegId == "17" || ExchSegId == "18")
                    cmd.CommandText = "[ObligationStatementCOMM_NEW]";


                cmd.CommandType = CommandType.StoredProcedure;
                if (ddldate.SelectedItem.Value.ToString() == "0")
                {
                    cmd.Parameters.AddWithValue("@fromdate", dtfor.Value);
                    cmd.Parameters.AddWithValue("@todate", "NA");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@fromdate", dtfrom.Value);
                    cmd.Parameters.AddWithValue("@todate", dtto.Value);
                }
                //if (rdbClientALL.Checked)
                //{
                //    cmd.Parameters.AddWithValue("@ClientsID", "ALL");
                //}
                //else
                //{
                //    cmd.Parameters.AddWithValue("@ClientsID", HiddenField_Client.Value);
                //}
                if (ddlviewby.SelectedItem.Value == "2")
                {
                    cmd.Parameters.AddWithValue("@Broker", "BO");
                    if (rdbbrokerall.Checked)
                    {
                        cmd.Parameters.AddWithValue("@ClientsID", "ALL");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@ClientsID", HiddenField_Broker.Value);
                    }

                }

                if (ddlviewby.SelectedItem.Value == "1")
                {
                    cmd.Parameters.AddWithValue("@Broker", "NA");
                    if (rdbClientALL.Checked)
                    {
                        cmd.Parameters.AddWithValue("@ClientsID", "ALL");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@ClientsID", HiddenField_Client.Value);
                    }
                }
                cmd.Parameters.AddWithValue("@segment", Convert.ToInt32(Session["usersegid"].ToString()));
                cmd.Parameters.AddWithValue("@MasterSegment", HttpContext.Current.Session["ExchangeSegmentID"].ToString());
                cmd.Parameters.AddWithValue("@Companyid", Session["LastCompany"].ToString());
                cmd.Parameters.AddWithValue("@Finyear", HttpContext.Current.Session["LastFinYear"]);

                if (ddlGroup.SelectedItem.Value.ToString() == "0")
                {
                    cmd.Parameters.AddWithValue("@grptype", "BRANCH");
                    if (rdbranchAll.Checked)
                    {
                        cmd.Parameters.AddWithValue("@grpid", "ALL");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@grpid", HiddenField_Branch.Value);
                    }
                }
                else if (ddlGroup.SelectedItem.Value.ToString() == "1")
                {
                    cmd.Parameters.AddWithValue("@grptype", ddlgrouptype.SelectedItem.Text.ToString().Trim());
                    if (rdddlgrouptypeAll.Checked)
                    {
                        cmd.Parameters.AddWithValue("@grpid", "ALL");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@grpid", HiddenField_Group.Value);
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@grptype", "BRANCH");
                    if (rdddlgrouptypeAll.Checked)
                    {
                        cmd.Parameters.AddWithValue("@grpid", "ALL");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@grpid", HiddenField_Branch.Value);
                    }
                }

                if (rbScreen.Checked || rbMail.Checked)
                {
                    cmd.Parameters.AddWithValue("@RESULTMODE", "SHOW");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@RESULTMODE", "PRINT");
                }
                cmd.Parameters.AddWithValue("@userbranchHierarchy", Session["userbranchHierarchy"].ToString());
                cmd.Parameters.AddWithValue("@ChkCollateralDeposit", ChkCollateralDeposit.Checked.ToString().Trim());
                cmd.Parameters.AddWithValue("@Header", txtHeader_hidden.Value);
                cmd.Parameters.AddWithValue("@Footer", txtFooter_hidden.Value);
                ////}
                ////else
                ////{
                //    if (ExchSegId == "2" || ExchSegId == "5")
                //        cmd.CommandText = "[for_dos_ObligationStatement]";
                //    else if (ExchSegId == "3" || ExchSegId == "6" || ExchSegId == "7" || ExchSegId == "8" || ExchSegId == "9" || ExchSegId == "10" || ExchSegId == "11" || ExchSegId == "12" || ExchSegId == "13" || ExchSegId == "17" || ExchSegId == "18")
                //        cmd.CommandText = "[for_dos_ObligationStatementcomm]";
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    if (ddldate.SelectedItem.Value.ToString() == "0")
                //    {
                //        cmd.Parameters.AddWithValue("@fromdate", dtfor.Value);
                //        cmd.Parameters.AddWithValue("@todate", "NA");
                //    }
                //    else
                //    {
                //        cmd.Parameters.AddWithValue("@fromdate", dtfrom.Value);
                //        cmd.Parameters.AddWithValue("@todate", dtto.Value);
                //    }
                //    if (rdbClientALL.Checked)
                //    {
                //        cmd.Parameters.AddWithValue("@ClientsID", "ALL");
                //    }
                //    else
                //    {
                //        cmd.Parameters.AddWithValue("@ClientsID", HiddenField_Client.Value);
                //    }
                //    cmd.Parameters.AddWithValue("@segment", Convert.ToInt32(Session["usersegid"].ToString()));
                //    cmd.Parameters.AddWithValue("@MasterSegment", HttpContext.Current.Session["ExchangeSegmentID"].ToString());
                //    cmd.Parameters.AddWithValue("@Companyid", Session["LastCompany"].ToString());
                //    cmd.Parameters.AddWithValue("@Finyear", HttpContext.Current.Session["LastFinYear"]);

                //    if (ddlGroup.SelectedItem.Value.ToString() == "0")
                //    {
                //        cmd.Parameters.AddWithValue("@grptype", "BRANCH");
                //        if (rdbranchAll.Checked)
                //        {
                //            cmd.Parameters.AddWithValue("@grpid", "ALL");
                //        }
                //        else
                //        {
                //            cmd.Parameters.AddWithValue("@grpid", HiddenField_Branch.Value);
                //        }
                //    }
                //    else if (ddlGroup.SelectedItem.Value.ToString() == "1")
                //    {
                //        cmd.Parameters.AddWithValue("@grptype", ddlgrouptype.SelectedItem.Text.ToString().Trim());
                //        if (rdbranchAll.Checked)
                //        {
                //            cmd.Parameters.AddWithValue("@grpid", "ALL");
                //        }
                //        else
                //        {
                //            cmd.Parameters.AddWithValue("@grpid", HiddenField_Group.Value);
                //        }
                //    }
                //    else
                //    {
                //        cmd.Parameters.AddWithValue("@grptype", "BRANCH");
                //        if (rdddlgrouptypeAll.Checked)
                //        {
                //            cmd.Parameters.AddWithValue("@grpid", "ALL");
                //        }
                //        else
                //        {
                //            cmd.Parameters.AddWithValue("@grpid", HiddenField_Branch.Value);
                //        }
                //    }

                //    if (rbScreen.Checked || rbMail.Checked)
                //    {
                //        cmd.Parameters.AddWithValue("@RESULTMODE", "SHOW");
                //    }
                //    else
                //    {
                //        cmd.Parameters.AddWithValue("@RESULTMODE", "PRINT");
                //    }
                //    cmd.Parameters.AddWithValue("@userbranchHierarchy", Session["userbranchHierarchy"].ToString());
                //    cmd.Parameters.AddWithValue("@ChkCollateralDeposit", ChkCollateralDeposit.Checked.ToString().Trim());
                //    cmd.Parameters.AddWithValue("@Header", txtHeader_hidden.Value);
                //    cmd.Parameters.AddWithValue("@Footer", txtFooter_hidden.Value);

                //}
                SqlDataAdapter da = new SqlDataAdapter();

                da.SelectCommand = cmd;
                cmd.CommandTimeout = 0;
                ds.Reset();
                da.Fill(ds);
                da.Dispose();
                ViewState["dataset"] = ds;
                if (ds.Tables[0].Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(1);", true);

                    //Page.ClientScript.RegisterStartupScript(GetType(), "JScript32", "<script language='javascript'>Page_Load();</script>");
                    //Page.ClientScript.RegisterStartupScript(GetType(), "JScript34", "<script language='javascript'>alert('No Record Found');</script>");

                }
                else
                {
                    if (rbScreen.Checked)
                    {
                        ddlbandforgroup();
                    }
                    if (rbPrint.Checked)
                    {
                        Print();
                    }
                    if (rbMail.Checked)
                    {
                        mail();
                    }
                    if (rbdos.Checked)
                    {
                        dos();
                    }
                    //if (rbexcel.Checked)
                    //{
                    //    excel();
                    //}
                }
            }

        }
        void procedureForLedger()
        {

            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            {

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "[ObligationStatementFO_NEW]";

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@fromdate", Request.QueryString["date"].ToString());
                cmd.Parameters.AddWithValue("@todate", "NA");
                cmd.Parameters.AddWithValue("@ClientsID", "'" + Request.QueryString["ClientID"].ToString() + "'");
                cmd.Parameters.AddWithValue("@segment", Convert.ToInt32(Session["usersegid"].ToString()));
                cmd.Parameters.AddWithValue("@MasterSegment", HttpContext.Current.Session["ExchangeSegmentID"].ToString());
                cmd.Parameters.AddWithValue("@Companyid", Session["LastCompany"].ToString());
                cmd.Parameters.AddWithValue("@Finyear", HttpContext.Current.Session["LastFinYear"]);
                cmd.Parameters.AddWithValue("@grptype", "BRANCH");
                cmd.Parameters.AddWithValue("@grpid", "ALL");
                cmd.Parameters.AddWithValue("@RESULTMODE", "SHOW");
                cmd.Parameters.AddWithValue("@userbranchHierarchy", Session["userbranchHierarchy"].ToString());
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.CommandTimeout = 0;
                ds.Reset();
                da.Fill(ds);
                da.Dispose();
                ViewState["dataset"] = ds;
                if (ds.Tables[0].Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(1);", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "DisplayLedger", "DisplayLedger();", true);
                    htmltable(ds.Tables[0].Rows[0]["CUSTOMERID"].ToString(), ds.Tables[0].Rows[0]["CLIENTNAME"].ToString(), ds.Tables[0].Rows[0]["GRPNAME"].ToString());
                }
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
            CurrentPage = 0;
            ddlbandforClient();
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
            htmltable(cmbclient.SelectedItem.Value, cmbclient.SelectedItem.Text.ToString().Trim(), cmbgroup.SelectedItem.Text.ToString().Trim());
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
        void htmltable(string clientid, string clientname, string grpname)
        {
            DataView viewproduct = new DataView();
            ds = (DataSet)ViewState["dataset"];
            String strHtml = String.Empty;
            String strHtml1 = String.Empty;
            int flag = 0;
            string str = null;
            /////////********FOR HEADER BEGIN
            if (Request.QueryString["date"] != null)
            {
                str = " Report For " + oconverter.ArrangeDate2(Request.QueryString["date"].ToString());
            }
            else
            {

                if (ddlGroup.SelectedItem.Value.ToString() == "1")
                {
                    str = "[" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "]";
                }

                if (ddldate.SelectedItem.Value.ToString() == "0")
                {
                    str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report For " + oconverter.ArrangeDate2(dtfor.Value.ToString());
                    ViewState["billdate"] = oconverter.ArrangeDate2(dtfor.Value.ToString());
                }
                else
                {
                    str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report Period " + oconverter.ArrangeDate2(dtfrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtto.Value.ToString());
                    ViewState["billdate"] = oconverter.ArrangeDate2(dtfrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtto.Value.ToString());
                }
                if (rbMail.Checked)
                {
                    str = str + " [ <b>" + grpname.ToString().Trim() + "</b> ]";
                }


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
            strHtml += "<td align=\"left\" colspan=12>Client Name :&nbsp;<b>" + clientname.ToString().Trim() + "</b>[ <b>" + dt1.Rows[0]["UCC"].ToString() + " </b> ]</td></tr>";


            ///////DIPLAY OPENING BALN. BEGIN
            strHtml += "<tr style=\"background-color:White;\"><td style=\"color:Blue;border-top:2px;border-bottom:2px;border-style: groove;border-right-style: none;font-size:xx-small; \" align=\"right\" colspan=10><b>" + dt1.Rows[0]["NETOPENINGNAME"].ToString().Trim() + "<b/></td>";
            if (dt1.Rows[0]["NETOPENINGDR"] != DBNull.Value)
                strHtml += "<td align=\"center\" style=\"color:Black;border-top:2px;border-bottom:2px;border-style: groove;border-right-style: none;border-left-style: none;font-size:xx-small; \">" + dt1.Rows[0]["NETOPENINGDR"].ToString().Trim() + " (Dr.)</td>";
            else
                strHtml += "<td align=\"left\" >&nbsp;</td>";
            if (dt1.Rows[0]["NETOPENINGCR"] != DBNull.Value)
                strHtml += "<td align=\"center\" style=\"color:Black;border-top:2px;border-bottom:2px;border-style: groove;border-right-style: none;border-left-style: none;font-size:xx-small; \">" + dt1.Rows[0]["NETOPENINGCR"].ToString().Trim() + " (Cr.)</td></tr>";
            else
                strHtml += "<td align=\"left\" >&nbsp;</td>";

            ///////DIPLAY OPENING BALN. END

            ////////////MTM Obligation BEGIN
            DataTable dtdisticntproduct = new DataTable();
            DataView viewdistinctproduct = dt1.DefaultView;
            viewdistinctproduct.RowFilter = "Identifier='FUT' AND (TRADECATEGORY IS NULL OR TRADECATEGORY='EXP')";
            dtdisticntproduct = viewdistinctproduct.ToTable(true, new string[] { "PRODUCTID", "SYMBOL" });

            if (dtdisticntproduct.Rows.Count > 0)
            {
                /////////HTML TABLE HEADER
                strHtml += "<tr style=\"background-color: #DBEEF3;\">";
                strHtml += "<td align=\"center\" colspan=3><b>MTM Obligation</b></td>";
                strHtml += "<td align=\"center\"><b>Trade Date</b></td>";
                strHtml += "<td align=\"center\"><b>Trade No.</b></td>";
                strHtml += "<td align=\"center\"><b>Buy Qty</b></td>";
                strHtml += "<td align=\"center\"><b>Sell Qty</b></td>";
                strHtml += "<td align=\"center\"><b>Mkt Rate</b></td>";
                strHtml += "<td align=\"center\"><b>Brkg</b></td>";
                strHtml += "<td align=\"center\"><b>Net Rate</b></td>";
                strHtml += "<td align=\"center\"><b>Amount Dr.</b></td>";
                strHtml += "<td align=\"center\"><b>Amount Cr.</b></td></tr>";
            }

            for (int p = 0; p < dtdisticntproduct.Rows.Count; p++)
            {
                viewproduct = new DataView();
                viewproduct = dt1.DefaultView;
                viewproduct.RowFilter = "PRODUCTID='" + dtdisticntproduct.Rows[p]["PRODUCTID"].ToString().Trim() + "'";
                DataTable dt = new DataTable();
                dt = viewproduct.ToTable();



                flag = flag + 1;
                strHtml += "<tr style=\"background-color: " + GetRowColor(flag) + " ;text-align:left\">";
                strHtml += "<td colspan=3 align=left style=\"color:maroon;\">" + dt.Rows[0]["SYMBOL"].ToString().Trim() + "</td>";
                /////////bf position begin

                if (dt.Rows[0]["BFQTYDR"] != DBNull.Value || dt.Rows[0]["BFQTYCR"] != DBNull.Value)
                {
                    strHtml += "<td colspan=2 align=left  style=\"font-size:xx-small;color:blue;\">Brought Forward</td>";
                    if (dt.Rows[0]["BFQTYDR"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt.Rows[0]["BFQTYDR"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";
                    if (dt.Rows[0]["BFQTYCR"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt.Rows[0]["BFQTYCR"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";

                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt.Rows[0]["BFSETTPRICE"].ToString() + "</td>";
                    strHtml += "<td align=\"left\" colspan=2>&nbsp;</td>";
                    if (dt.Rows[0]["BFVALUEDR"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt.Rows[0]["BFVALUEDR"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";
                    if (dt.Rows[0]["BFVALUECR"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt.Rows[0]["BFVALUECR"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";

                }
                else
                {
                    strHtml += "<td align=\"left\" colspan=9>&nbsp;</td>";
                }
                strHtml += "</tr>";
                /////////bf position end
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ////detail record begin

                    if (dt.Rows[i]["BUYQTY"] != DBNull.Value || dt.Rows[i]["SELLQTY"] != DBNull.Value)
                    {
                        flag = flag + 1;
                        strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                        strHtml += "<td align=\"left\" colspan=3>&nbsp;</td>";
                        strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" >" + dt.Rows[i]["TRADEDATE"].ToString() + "</td>";
                        strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" >" + dt.Rows[i]["TradeNumber"].ToString() + "</td>";
                        if (dt.Rows[i]["BUYQTY"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt.Rows[i]["BUYQTY"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" >&nbsp;</td>";

                        if (dt.Rows[i]["SELLQTY"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt.Rows[i]["SELLQTY"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" >&nbsp;</td>";

                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt.Rows[i]["MKTRATE"].ToString() + "</td>";
                        if (dt.Rows[i]["BRKG"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt.Rows[i]["BRKG"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" >&nbsp;</td>";
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt.Rows[i]["NETRATE"].ToString() + "</td>";
                        if (dt.Rows[i]["NETAMNTDR"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt.Rows[i]["NETAMNTDR"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" >&nbsp;</td>";
                        if (dt.Rows[i]["NETAMNTCR"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt.Rows[i]["NETAMNTCR"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" >&nbsp;</td>";
                        strHtml += "</tr>";
                    }
                    ////detail record end
                }
                /////////cf position begin

                if (dt.Rows[0]["CFQTYDR"] != DBNull.Value || dt.Rows[0]["CFQTYCR"] != DBNull.Value)
                {
                    flag = flag + 1;
                    strHtml += "<tr style=\"background-color: " + GetRowColor(flag) + " ;text-align:left\">";
                    strHtml += "<td align=\"left\" colspan=3>&nbsp;</td>";
                    strHtml += "<td colspan=2 align=left  style=\"font-size:xx-small;color:blue;\">Carried Forward</td>";
                    if (dt.Rows[0]["CFQTYDR"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt.Rows[0]["CFQTYDR"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";
                    if (dt.Rows[0]["CFQTYCR"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt.Rows[0]["CFQTYCR"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt.Rows[0]["CFSETTPRICE"].ToString() + "</td>";
                    strHtml += "<td align=\"left\" colspan=2>&nbsp;</td>";
                    if (dt.Rows[0]["CFVALUEDR"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt.Rows[0]["CFVALUEDR"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";
                    if (dt.Rows[0]["CFVALUECR"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt.Rows[0]["CFVALUECR"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";
                    strHtml += "</tr>";
                }
                /////////cf position end

                ///////net mtm begin(per product)
                strHtml += "<tr style=\"background-color:White;\">";
                strHtml += "<td align=right colspan=12><div style=\"background-color:lavender;width:50%;\">";
                strHtml += "<table width=\"100%\" border=" + Convert.ToChar(34) + 0 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                strHtml += "<tr><td align=left  style=\"font-size:xx-small;width:40%;\">" + dt.Rows[0]["SETTLEMENTNAME"].ToString().Trim() + "</td>";
                if (dt.Rows[0]["MTMDR"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;width:10%;\" >" + dt.Rows[0]["MTMDR"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;width:10%;\" >&nbsp;</td>";
                if (dt.Rows[0]["MTMCR"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;width:10%;\" >" + dt.Rows[0]["MTMCR"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;width:10%;\" >&nbsp;</td>";
                strHtml += "</tr></table></div></td></tr>";
                ///////net mtm end(per product)

            }

            ////total mtm begin
            if (dtdisticntproduct.Rows.Count > 0)
            {
                if (dt1.Rows[0]["FINALSETTLEMENTDR"] != DBNull.Value || dt1.Rows[0]["FINALSETTLEMENTCR"] != DBNull.Value || dt1.Rows[0]["MTMSETTLEMENTDR"] != DBNull.Value || dt1.Rows[0]["MTMSETTLEMENTCR"] != DBNull.Value || dt1.Rows[0]["NETMTMSETTLEMENTDR"] != DBNull.Value || dt1.Rows[0]["NETMTMSETTLEMENTCR"] != DBNull.Value)
                {
                    strHtml += "<tr style=\"background-color:White;\">";
                    strHtml += "<td align=right colspan=12><div style=\"background-color:lavender;width:50%;\">";
                    strHtml += "<table width=\"100%\" border=" + Convert.ToChar(34) + 0 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

                    if (dt1.Rows[0]["FINALSETTLEMENTNAME"] != DBNull.Value)
                    {
                        strHtml += "<tr><td align=left  style=\"font-size:xx-small;width:40%;\">" + dt1.Rows[0]["FINALSETTLEMENTNAME"].ToString().Trim() + "</td>";
                        if (dt1.Rows[0]["FINALSETTLEMENTDR"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;width:10%;\" >" + dt1.Rows[0]["FINALSETTLEMENTDR"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" style=\"font-size:xx-small;width:10%;\" >&nbsp;</td>";
                        if (dt1.Rows[0]["FINALSETTLEMENTCR"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;width:10%;\" >" + dt1.Rows[0]["FINALSETTLEMENTCR"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" style=\"font-size:xx-small;width:10%;\" >&nbsp;</td>";
                        strHtml += "</tr>";
                    }
                    if (dt1.Rows[0]["MTMSETTLEMENTNAME"] != DBNull.Value)
                    {
                        strHtml += "<tr><td align=left  style=\"font-size:xx-small;width:40%;\">" + dt1.Rows[0]["MTMSETTLEMENTNAME"].ToString().Trim() + "</td>";
                        if (dt1.Rows[0]["MTMSETTLEMENTDR"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;width:10%;\" >" + dt1.Rows[0]["MTMSETTLEMENTDR"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" style=\"font-size:xx-small;width:10%;\" >&nbsp;</td>";
                        if (dt1.Rows[0]["MTMSETTLEMENTCR"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;width:10%;\" >" + dt1.Rows[0]["MTMSETTLEMENTCR"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" style=\"font-size:xx-small;width:10%;\" >&nbsp;</td>";
                        strHtml += "</tr>";
                    }
                    if (dt1.Rows[0]["NETMTMSETTLEMENTNAME"] != DBNull.Value)
                    {
                        strHtml += "<tr><td align=left  style=\"font-size:xx-small;width:40%;\">" + dt1.Rows[0]["NETMTMSETTLEMENTNAME"].ToString().Trim() + "</td>";
                        if (dt1.Rows[0]["NETMTMSETTLEMENTDR"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;width:10%;\" >" + dt1.Rows[0]["NETMTMSETTLEMENTDR"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" style=\"font-size:xx-small;width:10%;\" >&nbsp;</td>";
                        if (dt1.Rows[0]["NETMTMSETTLEMENTCR"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;width:10%;\" >" + dt1.Rows[0]["NETMTMSETTLEMENTCR"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" style=\"font-size:xx-small;width:10%;\" >&nbsp;</td>";
                        strHtml += "</tr>";
                    }
                    strHtml += "</table></div></td></tr>";
                }

                /////total mtm end
            }
            ///////////MTM Obligation END

            //////////for Premium Obligation begin

            viewproduct = new DataView();
            viewproduct = dt1.DefaultView;
            viewproduct.RowFilter = "Identifier='OPT' AND (TRADECATEGORY IS NULL OR TRADECATEGORY='CA')";
            DataTable dt2 = new DataTable();
            dt2 = viewproduct.ToTable();

            if (dt2.Rows.Count > 0)
            {
                int k = 0;
                strHtml += "<tr style=\"background-color: #DBEEF3;\">";
                strHtml += "<td align=\"center\" colspan=2 ><b>Premium Obligation</b></td>";
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
                string productid = null;
                for (k = 0; k < dt2.Rows.Count; k++)
                {

                    if (dt2.Rows[k]["BUYQTY"] != DBNull.Value || dt2.Rows[k]["SELLQTY"] != DBNull.Value)
                    {
                        flag = flag + 1;
                        if (productid != dt2.Rows[k]["PRODUCTID"].ToString().Trim())
                        {
                            if (k != 0)
                            {
                                /////////product total

                                strHtml += "<tr style=\"background-color:White;\">";
                                strHtml += "<td align=right colspan=12><div style=\"background-color:lavender;width:50%;\">";
                                strHtml += "<table width=\"100%\" border=" + Convert.ToChar(34) + 0 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                                strHtml += "<tr><td align=left  style=\"font-size:xx-small;width:40%;\">" + dt2.Rows[k - 1]["SETTLEMENTNAME"].ToString().Trim() + "</td>";
                                if (dt2.Rows[k - 1]["MTMDR"] != DBNull.Value)
                                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;width:10%;\" >" + dt2.Rows[k - 1]["MTMDR"].ToString() + "</td>";
                                else
                                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;width:10%;\" >&nbsp;</td>";
                                if (dt2.Rows[k - 1]["MTMCR"] != DBNull.Value)
                                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;width:10%;\" >" + dt2.Rows[k - 1]["MTMCR"].ToString() + "</td>";
                                else
                                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;width:10%;\" >&nbsp;</td>";
                                strHtml += "</tr></table></div></td></tr>";
                            }
                            strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";

                            strHtml += "<td colspan=2 align=left style=\"color:maroon;\">" + dt2.Rows[k]["SYMBOL"].ToString().Trim() + "</td>";
                            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" >" + dt2.Rows[k]["STRIKEPRICE"].ToString() + "</td>";
                        }
                        else
                        {
                            strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";

                            strHtml += "<td align=\"left\" colspan=3>&nbsp;</td>";

                        }
                        productid = dt2.Rows[k]["PRODUCTID"].ToString().Trim();
                        strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" >" + dt2.Rows[k]["TRADEDATE"].ToString() + "</td>";
                        strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" >" + dt2.Rows[k]["TradeNumber"].ToString() + "</td>";
                        if (dt2.Rows[k]["BUYQTY"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt2.Rows[k]["BUYQTY"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" >&nbsp;</td>";

                        if (dt2.Rows[k]["SELLQTY"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt2.Rows[k]["SELLQTY"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" >&nbsp;</td>";

                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt2.Rows[k]["MKTRATE"].ToString() + "</td>";
                        if (dt2.Rows[k]["BRKG"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt2.Rows[k]["BRKG"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" >&nbsp;</td>";
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt2.Rows[k]["NETRATE"].ToString() + "</td>";
                        if (dt2.Rows[k]["NETAMNTDR"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt2.Rows[k]["NETAMNTDR"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" >&nbsp;</td>";
                        if (dt2.Rows[k]["NETAMNTCR"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt2.Rows[k]["NETAMNTCR"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" >&nbsp;</td>";
                        strHtml += "</tr>";
                    }
                }

                /////////product total
                strHtml += "<tr style=\"background-color:White;\">";
                strHtml += "<td align=right colspan=12><div style=\"background-color:lavender;width:50%;\">";
                strHtml += "<table width=\"100%\" border=" + Convert.ToChar(34) + 0 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                strHtml += "<tr><td align=left  style=\"font-size:xx-small;width:40%;\">" + dt2.Rows[k - 1]["SETTLEMENTNAME"].ToString().Trim() + "</td>";
                if (dt2.Rows[k - 1]["MTMDR"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;width:10%;\" >" + dt2.Rows[k - 1]["MTMDR"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;width:10%;\" >&nbsp;</td>";
                if (dt2.Rows[k - 1]["MTMCR"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;width:10%;\" >" + dt2.Rows[k - 1]["MTMCR"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;width:10%;\" >&nbsp;</td>";
                strHtml += "</tr></table></div></td></tr>";

                ///////net all product total
                strHtml += "<tr style=\"background-color:White;\">";
                strHtml += "<td align=right colspan=12><div style=\"background-color:lavender;width:50%;\">";
                strHtml += "<table width=\"100%\" border=" + Convert.ToChar(34) + 0 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

                if (dt2.Rows[0]["NETPRMSETTLEMENTNAME"] != DBNull.Value)
                {
                    strHtml += "<tr><td align=left  style=\"font-size:xx-small;width:40%;\">" + dt2.Rows[0]["NETPRMSETTLEMENTNAME"].ToString().Trim() + "</td>";
                    if (dt2.Rows[0]["NETPRMSETTLEMENTDR"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;width:10%;\" >" + dt2.Rows[0]["NETPRMSETTLEMENTDR"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" style=\"font-size:xx-small;width:10%;\" >&nbsp;</td>";
                    if (dt2.Rows[0]["NETPRMSETTLEMENTCR"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;width:10%;\" >" + dt2.Rows[0]["NETPRMSETTLEMENTCR"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" style=\"font-size:xx-small;width:10%;\" >&nbsp;</td>";
                    strHtml += "</tr>";
                }

                strHtml += "</table></div></td></tr>";

            }
            /////////for FOR Premium Obligation end
            ////////FOR Options Final Settlement
            viewproduct = new DataView();
            viewproduct = dt1.DefaultView;
            viewproduct.RowFilter = "Identifier='OPT' AND TRADECATEGORY IN ('Exercised','Assigned')";
            DataTable dt3 = new DataTable();
            dt3 = viewproduct.ToTable();



            if (dt3.Rows.Count > 0)
            {
                ////////html header begin
                strHtml += "<tr style=\"background-color: #DBEEF3;\">";
                strHtml += "<td align=\"center\" colspan=2><b>Options Final Settlement</b></td>";
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

                for (int p = 0; p < dt3.Rows.Count; p++)
                {

                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";

                    strHtml += "<td colspan=2 align=left style=\"color:maroon;\">" + dt3.Rows[p]["SYMBOL"].ToString() + "</td>";
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt3.Rows[p]["STRIKEPRICE"].ToString() + "</td>";
                    strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" >" + dt3.Rows[p]["TRADEDATE"].ToString() + "</td>";
                    strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" >" + dt3.Rows[p]["TRADECATEGORY"].ToString() + "</td>";
                    if (dt3.Rows[p]["BUYQTY"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt3.Rows[p]["BUYQTY"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";

                    if (dt3.Rows[p]["SELLQTY"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt3.Rows[p]["SELLQTY"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";

                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt3.Rows[p]["MKTRATE"].ToString() + "</td>";
                    if (dt3.Rows[p]["BRKG"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt3.Rows[p]["BRKG"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt3.Rows[p]["NETRATE"].ToString() + "</td>";
                    if (dt3.Rows[p]["NETAMNTDR"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt3.Rows[p]["NETAMNTDR"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";
                    if (dt3.Rows[p]["NETAMNTCR"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt3.Rows[p]["NETAMNTCR"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";
                    strHtml += "</tr>";

                }
                /////////net option detail

                strHtml += "<tr style=\"background-color:White;\">";
                strHtml += "<td align=right colspan=12><div style=\"background-color:lavender;width:50%;\">";
                strHtml += "<table width=\"100%\" border=" + Convert.ToChar(34) + 0 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

                if (dt3.Rows[0]["NETOPTIONSETTLEMENTNAME"] != DBNull.Value)
                {
                    strHtml += "<tr><td align=left  style=\"font-size:xx-small;width:40%;\">" + dt3.Rows[0]["NETOPTIONSETTLEMENTNAME"].ToString().Trim() + "</td>";
                    if (dt3.Rows[0]["NETOPTIONSETTLEMENTDR"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;width:10%;\" >" + dt3.Rows[0]["NETOPTIONSETTLEMENTDR"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" style=\"font-size:xx-small;width:10%;\" >&nbsp;</td>";
                    if (dt3.Rows[0]["NETOPTIONSETTLEMENTCR"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;width:10%;\" >" + dt3.Rows[0]["NETOPTIONSETTLEMENTCR"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" style=\"font-size:xx-small;width:10%;\" >&nbsp;</td>";
                    strHtml += "</tr>";

                }
                strHtml += "</table></div></td></tr>";
            }

            //////charges result
            if (dt1.Rows[0]["CLIENTNETAMNTDR"] != DBNull.Value || dt1.Rows[0]["CLIENTNETAMNTCR"] != DBNull.Value)
            {

                strHtml += "<tr style=\"height:1px;background-color:White;\"><td colspan=11>&nbsp;</td></tr><tr style=\"background-color:White;\">";
                strHtml += "<td align=right colspan=11><div style=\"border: solid 1px black;width:50%;\">";
                strHtml += "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

                strHtml += "<tr><td align=left style=\"width:40%;\">&nbsp;</td>";
                strHtml += "<td align=\"right\" style=\"width:10%;color:black;\"><b>Amount Dr.</b></td>";
                strHtml += "<td align=\"right\" style=\"width:10%;color:black;\"><b>Amount Cr.</b></td>";
                strHtml += "</tr>";

                strHtml += "<tr><td align=left style=\"width:40%;\">" + dt1.Rows[0]["TOTALOBLIGATIONNAME"].ToString().Trim() + "</td>";
                if (dt1.Rows[0]["TOTALOBLIGATIONDR"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"width:10%;color:maroon;\">" + dt1.Rows[0]["TOTALOBLIGATIONDR"].ToString() + "</b></td>";
                else
                    strHtml += "<td style=\"width:5%;\"> &nbsp;</td>";
                if (dt1.Rows[0]["TOTALOBLIGATIONCR"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"width:10%;color:maroon;\">" + dt1.Rows[0]["TOTALOBLIGATIONCR"].ToString() + "</td>";
                else
                    strHtml += "<td style=\"width:10%;\"> &nbsp;</td>";
                strHtml += "</tr>";
                ////////srvtax on brkg
                if (dt1.Rows[0]["SRVTAXONBRKG"] != DBNull.Value)
                {
                    strHtml += "<tr><td align=left style=\"width:40%;\">Serv Tax & Cess on Brokerage :</td>";
                    strHtml += "<td align=\"right\" style=\"width:10%;color:maroon;\">" + dt1.Rows[0]["SRVTAXONBRKG"].ToString() + "</b></td>";
                    strHtml += "<td style=\"width:10%;\"> &nbsp;</td>";
                    strHtml += "</tr>";
                }
                //////////tran charge
                if (dt1.Rows[0]["TRANCHARGE"] != DBNull.Value)
                {
                    strHtml += "<tr><td align=left style=\"width:40%;\">Transaction Charges :</td>";
                    strHtml += "<td align=\"right\" style=\"width:10%;color:maroon;\">" + dt1.Rows[0]["TRANCHARGE"].ToString() + "</b></td>";
                    strHtml += "<td style=\"width:10%;\"> &nbsp;</td>";
                    strHtml += "</tr>";
                }

                //////////srvtax on tran charge
                if (dt1.Rows[0]["STTAXTRANCHARGE"] != DBNull.Value)
                {
                    strHtml += "<tr><td align=left style=\"width:40%;\">Serv Tax & Cess on Tran Charge:</td>";
                    strHtml += "<td align=\"right\" style=\"width:10%;color:maroon;\">" + dt1.Rows[0]["STTAXTRANCHARGE"].ToString() + "</b></td>";
                    strHtml += "<td style=\"width:10%;\"> &nbsp;</td>";
                    strHtml += "</tr>";
                }

                //////////stamp duty
                if (dt1.Rows[0]["STAMP"] != DBNull.Value)
                {
                    strHtml += "<tr><td align=left style=\"width:40%;\">Stamp Duty :</td>";
                    strHtml += "<td align=\"right\" style=\"width:10%;color:maroon;\">" + dt1.Rows[0]["STAMP"].ToString() + "</b></td>";
                    strHtml += "<td style=\"width:10%;\"> &nbsp;</td>";
                    strHtml += "</tr>";
                }

                //////////STT
                if (dt1.Rows[0]["STTAX"] != DBNull.Value)
                {
                    strHtml += "<tr><td align=left style=\"width:40%;\">STT Tax :</td>";
                    strHtml += "<td align=\"right\" style=\"width:10%;color:maroon;\">" + dt1.Rows[0]["STTAX"].ToString() + "</b></td>";
                    strHtml += "<td style=\"width:10%;\"> &nbsp;</td>";
                    strHtml += "</tr>";
                }
                //////////Sebi Fee
                if (dt1.Rows[0]["SEBICHARGE"] != DBNull.Value)
                {
                    strHtml += "<tr><td align=left style=\"width:40%;\">Sebi Charge :</td>";
                    strHtml += "<td align=\"right\" style=\"width:10%;color:maroon;\">" + dt1.Rows[0]["SEBICHARGE"].ToString() + "</b></td>";
                    strHtml += "<td style=\"width:10%;\"> &nbsp;</td>";
                    strHtml += "</tr>";
                }

                // ////////total charges
                if (dt1.Rows[0]["TOTALCHARGE"] != DBNull.Value)
                {
                    strHtml += "<tr><td align=left style=\"width:40%;\">Total Charges :</td>";
                    strHtml += "<td align=\"right\" style=\"width:10%;color:maroon;\">" + dt1.Rows[0]["TOTALCHARGE"].ToString() + "</b></td>";
                    strHtml += "<td style=\"width:10%;\"> &nbsp;</td>";
                    strHtml += "</tr>";
                }

                // ////////net bill 
                if (dt1.Rows[0]["NETBILLAMNTDR"] != DBNull.Value || dt1.Rows[0]["NETBILLAMNTCR"] != DBNull.Value)
                {
                    strHtml += "<tr><td align=left style=\"width:40%;\"><u>Net Bill Amount :</u></td>";
                    if (dt1.Rows[0]["NETBILLAMNTDR"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"width:10%;color:maroon;\">" + dt1.Rows[0]["NETBILLAMNTDR"].ToString() + "</b></td>";
                    else
                        strHtml += "<td style=\"width:10%;\"> &nbsp;</td>";
                    if (dt1.Rows[0]["NETBILLAMNTCR"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"width:10%;color:maroon;\">" + dt1.Rows[0]["NETBILLAMNTCR"].ToString() + "</td>";
                    else
                        strHtml += "<td style=\"width:10%;\"> &nbsp;</td>";
                    strHtml += "</tr>";
                }

                // ////////net fund 
                if (dt1.Rows[0]["NETFUNDDR"] != DBNull.Value || dt1.Rows[0]["NETFUNDCR"] != DBNull.Value)
                {
                    strHtml += "<tr><td align=left style=\"width:40%;\">" + dt1.Rows[0]["NETFUNDNAME"].ToString().Trim() + "</td>";
                    if (dt1.Rows[0]["NETFUNDDR"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"width:10%;color:maroon;\">" + dt1.Rows[0]["NETFUNDDR"].ToString() + "</b></td>";
                    else
                        strHtml += "<td style=\"width:10%;\"> &nbsp;</td>";
                    if (dt1.Rows[0]["NETFUNDCR"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"width:10%;color:maroon;\">" + dt1.Rows[0]["NETFUNDCR"].ToString() + "</td>";
                    else
                        strHtml += "<td style=\"width:10%;\"> &nbsp;</td>";
                    strHtml += "</tr>";
                }

                // ////////net amnt receivabel 
                if (dt1.Rows[0]["CLIENTNETAMNTDR"] != DBNull.Value || dt1.Rows[0]["CLIENTNETAMNTCR"] != DBNull.Value)
                {
                    strHtml += "<tr><td align=left style=\"width:40%;\">" + dt1.Rows[0]["CLIENTNETAMNTNAME"].ToString().Trim() + "</td>";
                    if (dt1.Rows[0]["CLIENTNETAMNTDR"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"width:10%;color:maroon;\">" + dt1.Rows[0]["CLIENTNETAMNTDR"].ToString() + "</b></td>";
                    else
                        strHtml += "<td style=\"width:10%;\"> &nbsp;</td>";
                    if (dt1.Rows[0]["CLIENTNETAMNTCR"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"width:10%;color:maroon;\">" + dt1.Rows[0]["CLIENTNETAMNTCR"].ToString() + "</td>";
                    else
                        strHtml += "<td style=\"width:10%;\"> &nbsp;</td>";
                    strHtml += "</tr>";
                }


                strHtml += "</table></div></td></tr>";
            }
            ////////margin
            if (dt1.Rows[0]["SHORTAGE"] != DBNull.Value || dt1.Rows[0]["EXCESS"] != DBNull.Value)
            {
                strHtml += "<tr style=\"background-color: #DBEEF3;\">";
                strHtml += "<td align=\"center\" ><b>Margin Summary</b></td>";
                strHtml += "<td align=\"center\" ><b>Span Margin</b></td>";
                strHtml += "<td align=\"center\"><b>Premium Margin</b></td>";
                strHtml += "<td align=\"center\"><b>Total Margin</b></td>";
                strHtml += "<td align=\"center\"><b>Exposure Margin</b></td>";
                strHtml += "<td align=\"center\"><b>Applicable Margin</b></td>";
                strHtml += "<td align=\"center\"><b>Cash Dep</b></td>";
                strHtml += "<td align=\"center\"><b>FDR</b></td>";
                strHtml += "<td align=\"center\"><b>Collaterals</b></td>";
                strHtml += "<td align=\"center\"><b>Total Deposit</b></td>";
                strHtml += "<td align=\"center\"><b>Shortage</b></td>";
                strHtml += "<td align=\"center\"><b>Excess</b></td></tr>";

                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                strHtml += "<td align=\"left\" >&nbsp;</td>";
                if (dt1.Rows[0]["SPANMRGN"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt1.Rows[0]["SPANMRGN"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";
                if (dt1.Rows[0]["PRMMRGN"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt1.Rows[0]["PRMMRGN"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";
                if (dt1.Rows[0]["TOTMRGN"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt1.Rows[0]["TOTMRGN"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";
                if (dt1.Rows[0]["EXPOSURMRGN"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt1.Rows[0]["EXPOSURMRGN"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";
                if (dt1.Rows[0]["APPMRGN"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt1.Rows[0]["APPMRGN"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";
                if (dt1.Rows[0]["CASHDEPnew"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt1.Rows[0]["CASHDEPnew"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";
                if (dt1.Rows[0]["CASHDEPnew1"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt1.Rows[0]["CASHDEPnew1"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";
                if (dt1.Rows[0]["COLLATERAL"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt1.Rows[0]["COLLATERAL"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";
                if (dt1.Rows[0]["TOTDEP"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt1.Rows[0]["TOTDEP"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";
                if (dt1.Rows[0]["SHORTAGE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt1.Rows[0]["SHORTAGE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";
                if (dt1.Rows[0]["EXCESS"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt1.Rows[0]["EXCESS"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";
                strHtml += "</tr>";
            }
            ////////last result
            strHtml += "<tr style=\"background-color:White;\">";
            strHtml += "<td align=right colspan=12><div style=\"background-color:lavender;width:50%;\">";
            strHtml += "<table width=\"100%\" border=" + Convert.ToChar(34) + 0 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

            if (dt1.Rows[0]["CLIENTLASTNAME"] != DBNull.Value)
            {
                strHtml += "<tr><td align=left  style=\"font-size:xx-small;width:40%;\">" + dt1.Rows[0]["CLIENTLASTNAME"].ToString() + "</td>";
                if (dt1.Rows[0]["CLIENTLASTDR"] != DBNull.Value)
                    strHtml += "<td  align=\"right\" style=\"font-size:xx-small;width:10%;\" >" + dt1.Rows[0]["CLIENTLASTDR"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;width:10%;\" >&nbsp;</td>";
                if (dt1.Rows[0]["CLIENTLASTCR"] != DBNull.Value)
                    strHtml += "<td  align=\"right\" style=\"font-size:xx-small;width:10%;\" >" + dt1.Rows[0]["CLIENTLASTCR"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;width:10%;\" >&nbsp;</td>";
                strHtml += "</tr>";
            }
            strHtml += "</table></div></td></tr>";



            strHtml += "</table>";
            DIVdisplayPERIOD.InnerHtml = strHtml1;
            display.InnerHtml = strHtml;
            ViewState["mail"] = strHtml1 + strHtml;

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
            ddlbandforClient();

        }
        protected void btnprint_Click(object sender, EventArgs e)
        {
            procedure();
        }



        void dos()
        {
            string[] connPath = ConfigurationManager.AppSettings["DBConnectionDefault"].Split(';');
            ds = (DataSet)ViewState["dataset"];
            if (ds.Tables[0].Columns.Count > 0)
            {
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
                // ds.Tables[0].WriteXmlSchema("D:\\Abhinanda_BackUp\\RPTXSD\\foobligation_new.xsd");

                ReportDocument report = new ReportDocument();
                //report.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;

                string tmpPdfPath = string.Empty;
                tmpPdfPath = HttpContext.Current.Server.MapPath("..\\management\\Obligationdos.rpt");
                report.Load(tmpPdfPath);
                report.SetDataSource(ds.Tables[0]);
                report.Subreports["prm"].SetDataSource(ds.Tables[0]);
                report.Subreports["finsett"].SetDataSource(ds.Tables[0]);
                report.VerifyDatabase();


                report.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
                //rpt.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "ledger");
                /////report.SetDataSource(ds);
                //report.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.DefaultPaperOrientation; 
                ////if (ChkDISTRIBUTION.Checked)
                ////{
                ////    report.SetParameterValue("@distribution", (object)"CHK");
                ////}
                ////else
                ////{
                report.SetParameterValue("@distribution", (object)"UNCHK");
                ///}
                //if (chkBothPrint.Checked)
                //{
                //    report.SetParameterValue("@bothsideprint", (object)"CHK");
                //}
                //else
                //{
                report.SetParameterValue("@bothsideprint", (object)"UNCHK");
                // }
                if (ddldate.SelectedItem.Value.ToString() == "0")
                {
                    ViewState["billprintdate"] = "F&O Obligation Statment " + oconverter.ArrangeDate2(dtfor.Value.ToString());
                }
                else
                {
                    ViewState["billprintdate"] = "F&O Obligation Statment " + oconverter.ArrangeDate2(dtfrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtto.Value.ToString());
                }

                string tmpPdfPath1;
                tmpPdfPath1 = string.Empty;
                tmpPdfPath1 = HttpContext.Current.Server.MapPath(@"..\Documents\TempPdfLocation\");
                string abcd = tmpPdfPath1 + "Billprint.pdf";
                report.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, abcd);
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript4567", "<script language='javascript'>window.open('billprintpopup.aspx');</script>");
                ////////////////report.PrintOptions.PrinterName = DropDownList1.SelectedItem.Value.ToString().Trim();
                ////////////////report.PrintToPrinter(1, false, 0, 0);
                //report.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, ViewState["billprintdate"].ToString().Trim());
                //////////////////report.Dispose();
                GC.Collect();
                //////////////////Page.ClientScript.RegisterStartupScript(GetType(), "JScript12", "<script language='javascript'>Page_Load();</script>");
                //////////////////Page.ClientScript.RegisterStartupScript(GetType(), "JScript14", "<script language='javascript'>alert('Print Send to Select Printer');</script>");
                ////rbScreen.Checked=true;
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript16", "<script language='javascript'>Page_Load();</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript18", "<script language='javascript'>alert('No Record Found');</script>");

            }
        }
        void Print()
        {

            ds = (DataSet)ViewState["dataset"];

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
            //ds.Tables[0].WriteXmlSchema("D:\\Abhinanda_BackUp\\RPTXSD\\foobligation_new.xsd");

            ReportDocument report = new ReportDocument();
            report.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;

            string tmpPdfPath = string.Empty;
            tmpPdfPath = HttpContext.Current.Server.MapPath("..\\management\\ObligationFoReport_new.rpt");
            report.Load(tmpPdfPath);
            report.SetDataSource(ds.Tables[0]);
            report.Subreports["prm"].SetDataSource(ds.Tables[0]);
            report.VerifyDatabase();


            if (ChkDISTRIBUTION.Checked)
            {
                report.SetParameterValue("@distribution", (object)"CHK");
            }
            else
            {
                report.SetParameterValue("@distribution", (object)"UNCHK");
            }
            if (chkBothPrint.Checked)
            {
                report.SetParameterValue("@bothsideprint", (object)"CHK");
            }
            else
            {
                report.SetParameterValue("@bothsideprint", (object)"UNCHK");
            }
            if (ddldate.SelectedItem.Value.ToString() == "0")
            {
                ViewState["billprintdate"] = "F&O Obligation Statment " + oconverter.ArrangeDate2(dtfor.Value.ToString());
            }
            else
            {
                ViewState["billprintdate"] = "F&O Obligation Statment " + oconverter.ArrangeDate2(dtfrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtto.Value.ToString());
            }
            report.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, ViewState["billprintdate"].ToString().Trim());
            report.Dispose();
            GC.Collect();
        }
        protected void cmbgroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlbandforClient();
        }
        protected void btnmailsend_Click(object sender, EventArgs e)
        {
            procedure();
        }
        void mail()
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
                optionforemail();
            }
        }
        void clientwiseemail()
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
                    htmltable(cmbclient.Items[k].Value, cmbclient.Items[k].Text.ToString().Trim(), cmbgroup.Items[j].Text.ToString().Trim());
                    if (oDBEngine.SendReport(ViewState["mail"].ToString().Trim(), cmbclient.Items[k].Value, ViewState["billdate"].ToString().Trim(), "Obligation Statement[" + ViewState["billdate"].ToString().Trim() + "]") == true)
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
                    htmltable(cmbclient.Items[k].Value, cmbclient.Items[k].Text.ToString().Trim(), dtgroupcontactid.Rows[j]["GRPNAME"].ToString().Trim());
                    if (ViewState["GRPmail"].ToString().Trim() == "mail")
                    {
                        ViewState["GRPmail"] = ViewState["mail"].ToString().Trim();
                    }
                    else
                    {
                        ViewState["GRPmail"] = ViewState["GRPmail"].ToString().Trim() + ViewState["mail"].ToString().Trim();
                    }

                }
                if (oDBEngine.SendReportBr(ViewState["GRPmail"].ToString().Trim(), cmbgroup.Items[j].Text.ToString().Trim(), ViewState["billdate"].ToString().Trim(), "Obligation Statement[" + ViewState["billdate"].ToString().Trim() + "]", cmbgroup.Items[j].Value) == true)
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
        void optionforemail()
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
                        htmltable(cmbclient.Items[k].Value, cmbclient.Items[k].Text.ToString().Trim(), dtgroupcontactid.Rows[j]["GRPNAME"].ToString().Trim());
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
                    if (oDBEngine.SendReportSt(ViewState["Usermail"].ToString().Trim(), HiddenField_emmail.Value, ViewState["billdate"].ToString().Trim(), "Obligation Statement[" + ViewState["billdate"].ToString().Trim() + "]") == true)
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

        void RateDateFetch()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            {
                DataSet ds1 = new DataSet();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "[Report_RateDateFetch]";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Segment", Session["usersegid"].ToString().Trim());
                cmd.Parameters.AddWithValue("@Companyid", Session["LastCompany"].ToString());
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.CommandTimeout = 0;
                ds1.Reset();
                ds1.Clear();
                da.Fill(ds1);
                da.Dispose();

                if (ds1.Tables[0].Rows.Count > 0)
                {
                    String strHtml = String.Empty;
                    strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

                    //////////////TABLE HEADER BIND
                    strHtml += "<tr style=\"background-color: #DBEEF3;\">";
                    for (int i = 0; i < ds1.Tables[0].Columns.Count; i++)
                    {
                        strHtml += "<td align=\"center\" style=\"font-size:smaller;\" nowrap=nowrap;><b>" + ds1.Tables[0].Columns[i].ColumnName + "</b></td>";
                    }
                    strHtml += "</tr>";

                    int flag = 0;
                    foreach (DataRow dr1 in ds1.Tables[0].Rows)
                    {
                        flag = flag + 1;
                        strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                        for (int j = 0; j < ds1.Tables[0].Columns.Count; j++)
                        {
                            if (dr1[j] != DBNull.Value)
                            {
                                strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds1.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";
                            }
                            else
                            {
                                strHtml += "<td>&nbsp;</td>";
                            }
                        }

                        strHtml += "</tr>";
                    }
                    strHtml += "</table>";

                    DivDateDisply.InnerHtml = strHtml;

                }

            }

        }
        protected void btndos_Click(object sender, EventArgs e)
        {
            procedure();
            Page.ClientScript.RegisterStartupScript(GetType(), "JScript22", "<script language='javascript'>Page_Load();</script>");
            // Page.ClientScript.RegisterStartupScript(GetType(), "JScript24", "<script language='javascript'>alert('No Record Found');</script>");
        }
        protected void btnexport_Click(object sender, EventArgs e)
        {
            procedure();
            ds = (DataSet)ViewState["dataset"];
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlbandforgroup1();
                export_clientwise();
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript115", "<script language='javascript'>alert('No Record Found');</script>");
            }

            Page.ClientScript.RegisterStartupScript(GetType(), "JScript122", "<script language='javascript'>Page_Load();</script>");
        }
        void export_clientwise()
        {
            string str123 = null;
            if (Request.QueryString["date"] != null)
            {
                str123 = " Report For " + oconverter.ArrangeDate2(Request.QueryString["date"].ToString());
            }
            else
            {

                if (ddlGroup.SelectedItem.Value.ToString() == "1")
                {
                    str123 = "[" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "]";
                }

                if (ddldate.SelectedItem.Value.ToString() == "0")
                {
                    str123 = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str123 + "Report For " + oconverter.ArrangeDate2(dtfor.Value.ToString());
                    ViewState["billdate"] = oconverter.ArrangeDate2(dtfor.Value.ToString());
                }
                else
                {
                    str123 = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str123 + "Report Period " + oconverter.ArrangeDate2(dtfrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtto.Value.ToString());
                    ViewState["billdate"] = oconverter.ArrangeDate2(dtfrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtto.Value.ToString());
                }



            }
            Response.AppendHeader("content-disposition", "attachment;filename='" + str123 + "'.xls");

            Response.Charset = "";

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            Response.ContentType = "application/vnd.ms-excel";

            this.EnableViewState = false;

            Response.Write(ViewState["Usermail"]);


            Response.End();

            // ViewState["dtExport"] = dtExport;
        }
        void ddlbandforgroup1()
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
            CurrentPage = 0;
            ddlbandforClient1();
        }
        void ddlbandforClient1()
        {
            ds = (DataSet)ViewState["dataset"];
            DataView viewgrp = new DataView();
            viewgrp = ds.Tables[0].DefaultView;
            //viewgrp.RowFilter = "GRPID='" + cmbgroup.SelectedItem.Value + "'";
            viewgrp.RowFilter = "CUSTOMERID is not null or CUSTOMERID <> 'ZZZZZZZZZZZ'";
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
            ViewState["Usermail"] = "UserMail";
            for (int k = 0; k < cmbclient.Items.Count; k++)
            {
                htmltable1(cmbclient.Items[k].Value, cmbclient.Items[k].Text.ToString().Trim(), cmbgroup.SelectedItem.Text.ToString().Trim());
                if (ViewState["Usermail"].ToString().Trim() == "UserMail")
                {
                    ViewState["Usermail"] = ViewState["mail"].ToString().Trim();
                }
                else
                {
                    ViewState["Usermail"] = ViewState["Usermail"].ToString().Trim() + ViewState["mail"].ToString().Trim();
                }
            }
            //LastPage = Distinctclient.Rows.Count - 1;
            //CurrentPage = 0;
            //bind_Details();
        }
        void htmltable1(string clientid, string clientname, string grpname)
        {
            DataView viewproduct = new DataView();
            ds = (DataSet)ViewState["dataset"];
            String strHtml = String.Empty;
            String strHtml1 = String.Empty;
            int flag = 0;
            string str = null;
            /////////********FOR HEADER BEGIN
            if (Request.QueryString["date"] != null)
            {
                str = " Report For " + oconverter.ArrangeDate2(Request.QueryString["date"].ToString());
            }
            else
            {

                if (ddlGroup.SelectedItem.Value.ToString() == "1")
                {
                    str = "[" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "]";
                }

                if (ddldate.SelectedItem.Value.ToString() == "0")
                {
                    str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report For " + oconverter.ArrangeDate2(dtfor.Value.ToString());
                    ViewState["billdate"] = oconverter.ArrangeDate2(dtfor.Value.ToString());
                }
                else
                {
                    str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report Period " + oconverter.ArrangeDate2(dtfrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtto.Value.ToString());
                    ViewState["billdate"] = oconverter.ArrangeDate2(dtfrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtto.Value.ToString());
                }
                if (rbMail.Checked)
                {
                    str = str + " [ <b>" + grpname.ToString().Trim() + "</b> ]";
                }


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
            strHtml += "<td align=\"left\" colspan=12>Client Name :&nbsp;<b>" + clientname.ToString().Trim() + "</b>[ <b>" + dt1.Rows[0]["UCC"].ToString() + " </b> ]</td></tr>";


            ///////DIPLAY OPENING BALN. BEGIN
            strHtml += "<tr style=\"background-color:White;\"><td style=\"color:Blue;border-top:2px;border-bottom:2px;border-style: groove;border-right-style: none;font-size:xx-small; \" align=\"right\" colspan=10><b>" + dt1.Rows[0]["NETOPENINGNAME"].ToString().Trim() + "<b/></td>";
            if (dt1.Rows[0]["NETOPENINGDR"] != DBNull.Value)
                strHtml += "<td align=\"center\" style=\"color:Black;border-top:2px;border-bottom:2px;border-style: groove;border-right-style: none;border-left-style: none;font-size:xx-small; \">" + dt1.Rows[0]["NETOPENINGDR"].ToString().Trim() + " (Dr.)</td>";
            else
                strHtml += "<td align=\"left\" >&nbsp;</td>";
            if (dt1.Rows[0]["NETOPENINGCR"] != DBNull.Value)
                strHtml += "<td align=\"center\" style=\"color:Black;border-top:2px;border-bottom:2px;border-style: groove;border-right-style: none;border-left-style: none;font-size:xx-small; \">" + dt1.Rows[0]["NETOPENINGCR"].ToString().Trim() + " (Cr.)</td></tr>";
            else
                strHtml += "<td align=\"left\" >&nbsp;</td>";

            ///////DIPLAY OPENING BALN. END

            ////////////MTM Obligation BEGIN
            DataTable dtdisticntproduct = new DataTable();
            DataView viewdistinctproduct = dt1.DefaultView;
            viewdistinctproduct.RowFilter = "Identifier='FUT' AND (TRADECATEGORY IS NULL OR TRADECATEGORY='EXP')";
            dtdisticntproduct = viewdistinctproduct.ToTable(true, new string[] { "PRODUCTID", "SYMBOL" });

            if (dtdisticntproduct.Rows.Count > 0)
            {
                /////////HTML TABLE HEADER
                strHtml += "<tr style=\"background-color: #DBEEF3;\">";
                strHtml += "<td align=\"center\" colspan=3><b>MTM Obligation</b></td>";
                strHtml += "<td align=\"center\"><b>Trade Date</b></td>";
                strHtml += "<td align=\"center\"><b>Trade No.</b></td>";
                strHtml += "<td align=\"center\"><b>Buy Qty</b></td>";
                strHtml += "<td align=\"center\"><b>Sell Qty</b></td>";
                strHtml += "<td align=\"center\"><b>Mkt Rate</b></td>";
                strHtml += "<td align=\"center\"><b>Brkg</b></td>";
                strHtml += "<td align=\"center\"><b>Net Rate</b></td>";
                strHtml += "<td align=\"center\"><b>Amount Dr.</b></td>";
                strHtml += "<td align=\"center\"><b>Amount Cr.</b></td></tr>";
                //strHtml += "<td align=\"center\"><b>A</b></td></tr>";
            }

            for (int p = 0; p < dtdisticntproduct.Rows.Count; p++)
            {
                viewproduct = new DataView();
                viewproduct = dt1.DefaultView;
                viewproduct.RowFilter = "PRODUCTID='" + dtdisticntproduct.Rows[p]["PRODUCTID"].ToString().Trim() + "'";
                DataTable dt = new DataTable();
                dt = viewproduct.ToTable();



                flag = flag + 1;
                strHtml += "<tr style=\"background-color: " + GetRowColor(flag) + " ;text-align:left\">";
                strHtml += "<td colspan=3 align=left style=\"color:maroon;\">" + dt.Rows[0]["SYMBOL"].ToString().Trim() + "</td>";
                /////////bf position begin

                if (dt.Rows[0]["BFQTYDR"] != DBNull.Value || dt.Rows[0]["BFQTYCR"] != DBNull.Value)
                {
                    strHtml += "<td colspan=2 align=left  style=\"font-size:xx-small;color:blue;\">Brought Forward</td>";
                    if (dt.Rows[0]["BFQTYDR"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt.Rows[0]["BFQTYDR"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";
                    if (dt.Rows[0]["BFQTYCR"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt.Rows[0]["BFQTYCR"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";

                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt.Rows[0]["BFSETTPRICE"].ToString() + "</td>";
                    strHtml += "<td align=\"left\" colspan=2>&nbsp;</td>";
                    if (dt.Rows[0]["BFVALUEDR"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt.Rows[0]["BFVALUEDR"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";
                    if (dt.Rows[0]["BFVALUECR"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt.Rows[0]["BFVALUECR"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";

                }
                else
                {
                    strHtml += "<td align=\"left\" colspan=9>&nbsp;</td>";
                }
                strHtml += "</tr>";
                /////////bf position end
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ////detail record begin

                    if (dt.Rows[i]["BUYQTY"] != DBNull.Value || dt.Rows[i]["SELLQTY"] != DBNull.Value)
                    {
                        flag = flag + 1;
                        strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                        strHtml += "<td align=\"left\" colspan=3>&nbsp;</td>";
                        strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" >" + dt.Rows[i]["TRADEDATE"].ToString() + "</td>";
                        strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" >" + dt.Rows[i]["TradeNumber"].ToString() + "</td>";
                        if (dt.Rows[i]["BUYQTY"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt.Rows[i]["BUYQTY"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" >&nbsp;</td>";

                        if (dt.Rows[i]["SELLQTY"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt.Rows[i]["SELLQTY"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" >&nbsp;</td>";

                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt.Rows[i]["MKTRATE"].ToString() + "</td>";
                        if (dt.Rows[i]["BRKG"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt.Rows[i]["BRKG"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" >&nbsp;</td>";
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt.Rows[i]["NETRATE"].ToString() + "</td>";
                        if (dt.Rows[i]["NETAMNTDR"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt.Rows[i]["NETAMNTDR"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" >&nbsp;</td>";
                        if (dt.Rows[i]["NETAMNTCR"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt.Rows[i]["NETAMNTCR"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" >&nbsp;</td>";
                        strHtml += "</tr>";
                    }
                    ////detail record end
                }
                /////////cf position begin

                if (dt.Rows[0]["CFQTYDR"] != DBNull.Value || dt.Rows[0]["CFQTYCR"] != DBNull.Value)
                {
                    flag = flag + 1;
                    strHtml += "<tr style=\"background-color: " + GetRowColor(flag) + " ;text-align:left\">";
                    strHtml += "<td align=\"left\" colspan=3>&nbsp;</td>";
                    strHtml += "<td colspan=2 align=left  style=\"font-size:xx-small;color:blue;\">Carried Forward</td>";
                    if (dt.Rows[0]["CFQTYDR"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt.Rows[0]["CFQTYDR"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";
                    if (dt.Rows[0]["CFQTYCR"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt.Rows[0]["CFQTYCR"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt.Rows[0]["CFSETTPRICE"].ToString() + "</td>";
                    strHtml += "<td align=\"left\" colspan=2>&nbsp;</td>";
                    if (dt.Rows[0]["CFVALUEDR"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt.Rows[0]["CFVALUEDR"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";
                    if (dt.Rows[0]["CFVALUECR"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt.Rows[0]["CFVALUECR"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";
                    strHtml += "</tr>";
                }
                /////////cf position end

                ///////net mtm begin(per product)
                strHtml += "<tr style=\"background-color:White;\">";
                strHtml += "<td align=right colspan=12><div style=\"background-color:lavender;width:50%;\">";
                strHtml += "<table width=\"100%\" border=" + Convert.ToChar(34) + 0 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                strHtml += "<tr><td align=left  style=\"font-size:xx-small;width:40%;\">" + dt.Rows[0]["SETTLEMENTNAME"].ToString().Trim() + "</td>";
                if (dt.Rows[0]["MTMDR"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" ><td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" ><td align=\"right\" style=\"font-size:xx-small;width:10%;\" >" + dt.Rows[0]["MTMDR"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;width:10%;\" >&nbsp;</td>";
                if (dt.Rows[0]["MTMCR"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;width:10%;\" ><td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" ><td align=\"right\" style=\"font-size:xx-small;width:10%;\" ><td align=\"right\" style=\"font-size:xx-small;width:10%;\" >" + dt.Rows[0]["MTMCR"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;width:10%;\" >&nbsp;</td>";
                strHtml += "</tr></table></div></td></tr>";
                ///////net mtm end(per product)

            }

            ////total mtm begin
            if (dtdisticntproduct.Rows.Count > 0)
            {
                if (dt1.Rows[0]["FINALSETTLEMENTDR"] != DBNull.Value || dt1.Rows[0]["FINALSETTLEMENTCR"] != DBNull.Value || dt1.Rows[0]["MTMSETTLEMENTDR"] != DBNull.Value || dt1.Rows[0]["MTMSETTLEMENTCR"] != DBNull.Value || dt1.Rows[0]["NETMTMSETTLEMENTDR"] != DBNull.Value || dt1.Rows[0]["NETMTMSETTLEMENTCR"] != DBNull.Value)
                {
                    strHtml += "<tr style=\"background-color:White;\">";
                    strHtml += "<td align=right colspan=12><div style=\"background-color:lavender;width:50%;\">";
                    strHtml += "<table width=\"100%\" border=" + Convert.ToChar(34) + 0 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

                    if (dt1.Rows[0]["FINALSETTLEMENTNAME"] != DBNull.Value)
                    {
                        strHtml += "<tr><td align=left  style=\"font-size:xx-small;width:40%;\">" + dt1.Rows[0]["FINALSETTLEMENTNAME"].ToString().Trim() + "</td>";
                        if (dt1.Rows[0]["FINALSETTLEMENTDR"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" ><td align=\"right\" style=\"font-size:xx-small;width:10%;\" ><td align=\"right\" style=\"font-size:xx-small;width:10%;\" >" + dt1.Rows[0]["FINALSETTLEMENTDR"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" style=\"font-size:xx-small;width:10%;\" >&nbsp;</td>";
                        if (dt1.Rows[0]["FINALSETTLEMENTCR"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;width:10%;\" ><td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" ><td align=\"right\" style=\"font-size:xx-small;width:10%;\" >" + dt1.Rows[0]["FINALSETTLEMENTCR"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" style=\"font-size:xx-small;width:10%;\" >&nbsp;</td>";
                        strHtml += "</tr>";
                    }
                    if (dt1.Rows[0]["MTMSETTLEMENTNAME"] != DBNull.Value)
                    {
                        strHtml += "<tr><td align=left  style=\"font-size:xx-small;width:40%;\">" + dt1.Rows[0]["MTMSETTLEMENTNAME"].ToString().Trim() + "</td>";
                        if (dt1.Rows[0]["MTMSETTLEMENTDR"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" ><td align=\"right\" style=\"font-size:xx-small;width:10%;\" ><td align=\"right\" style=\"font-size:xx-small;width:10%;\" >" + dt1.Rows[0]["MTMSETTLEMENTDR"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" style=\"font-size:xx-small;width:10%;\" >&nbsp;</td>";
                        if (dt1.Rows[0]["MTMSETTLEMENTCR"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" ><td align=\"right\" style=\"font-size:xx-small;width:10%;\" >" + dt1.Rows[0]["MTMSETTLEMENTCR"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" style=\"font-size:xx-small;width:10%;\" >&nbsp;</td>";
                        strHtml += "</tr>";
                    }
                    if (dt1.Rows[0]["NETMTMSETTLEMENTNAME"] != DBNull.Value)
                    {
                        strHtml += "<tr><td align=left  style=\"font-size:xx-small;width:40%;\">" + dt1.Rows[0]["NETMTMSETTLEMENTNAME"].ToString().Trim() + "</td>";
                        if (dt1.Rows[0]["NETMTMSETTLEMENTDR"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" ><td align=\"right\" style=\"font-size:xx-small;width:10%;\" ><td align=\"right\" style=\"font-size:xx-small;width:10%;\" >" + dt1.Rows[0]["NETMTMSETTLEMENTDR"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" style=\"font-size:xx-small;width:10%;\" >&nbsp;</td>";
                        if (dt1.Rows[0]["NETMTMSETTLEMENTCR"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;width:10%;\" ><td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" ><td align=\"right\" style=\"font-size:xx-small;width:10%;\" >" + dt1.Rows[0]["NETMTMSETTLEMENTCR"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" style=\"font-size:xx-small;width:10%;\" >&nbsp;</td>";
                        strHtml += "</tr>";
                    }
                    strHtml += "</table></div></td></tr>";
                }

                /////total mtm end
            }
            ///////////MTM Obligation END

            //////////for Premium Obligation begin

            viewproduct = new DataView();
            viewproduct = dt1.DefaultView;
            viewproduct.RowFilter = "Identifier='OPT' AND (TRADECATEGORY IS NULL OR TRADECATEGORY='CA')";
            DataTable dt2 = new DataTable();
            dt2 = viewproduct.ToTable();

            if (dt2.Rows.Count > 0)
            {
                int k = 0;
                strHtml += "<tr style=\"background-color: #DBEEF3;\">";
                strHtml += "<td align=\"center\" colspan=2><b>Premium Obligation</b></td>";
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
                string productid = null;
                for (k = 0; k < dt2.Rows.Count; k++)
                {

                    if (dt2.Rows[k]["BUYQTY"] != DBNull.Value || dt2.Rows[k]["SELLQTY"] != DBNull.Value)
                    {
                        flag = flag + 1;
                        if (productid != dt2.Rows[k]["PRODUCTID"].ToString().Trim())
                        {
                            if (k != 0)
                            {
                                /////////product total
                                strHtml += "<tr style=\"background-color:White;\">";
                                strHtml += "<td align=right colspan=12><div style=\"background-color:lavender;width:50%;\">";
                                strHtml += "<table width=\"100%\" border=" + Convert.ToChar(34) + 0 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                                strHtml += "<tr><td align=left  style=\"font-size:xx-small;width:40%;\">" + dt2.Rows[k - 1]["SETTLEMENTNAME"].ToString().Trim() + "</td>";
                                if (dt2.Rows[k - 1]["MTMDR"] != DBNull.Value)
                                    strHtml += " <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" ><td align=\"right\" style=\"font-size:xx-small;width:10%;\" ><td align=\"right\" style=\"font-size:xx-small;width:10%;\" >" + dt2.Rows[k - 1]["MTMDR"].ToString() + "</td>";
                                else
                                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;width:10%;\" >&nbsp;</td>";
                                if (dt2.Rows[k - 1]["MTMCR"] != DBNull.Value)
                                    strHtml += " <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" ><td align=\"right\" style=\"font-size:xx-small;width:10%;\" ><td align=\"right\" style=\"font-size:xx-small;width:10%;\" >" + dt2.Rows[k - 1]["MTMCR"].ToString() + "</td>";
                                else
                                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;width:10%;\" >&nbsp;</td>";
                                strHtml += "</tr></table></div></td></tr>";
                            }
                            strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                            strHtml += "<td colspan=2 align=left style=\"color:maroon;\">" + dt2.Rows[k]["SYMBOL"].ToString().Trim() + "</td>";
                            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" >" + dt2.Rows[k]["STRIKEPRICE"].ToString() + "</td>";
                        }
                        else
                        {
                            strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                            strHtml += "<td align=\"left\" colspan=3>&nbsp;</td>";

                        }
                        productid = dt2.Rows[k]["PRODUCTID"].ToString().Trim();
                        strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" >" + dt2.Rows[k]["TRADEDATE"].ToString() + "</td>";
                        strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" >" + dt2.Rows[k]["TradeNumber"].ToString() + "</td>";
                        if (dt2.Rows[k]["BUYQTY"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt2.Rows[k]["BUYQTY"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" >&nbsp;</td>";

                        if (dt2.Rows[k]["SELLQTY"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt2.Rows[k]["SELLQTY"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" >&nbsp;</td>";

                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt2.Rows[k]["MKTRATE"].ToString() + "</td>";
                        if (dt2.Rows[k]["BRKG"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt2.Rows[k]["BRKG"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" >&nbsp;</td>";
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt2.Rows[k]["NETRATE"].ToString() + "</td>";
                        if (dt2.Rows[k]["NETAMNTDR"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt2.Rows[k]["NETAMNTDR"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" >&nbsp;</td>";
                        if (dt2.Rows[k]["NETAMNTCR"] != DBNull.Value)
                            strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt2.Rows[k]["NETAMNTCR"].ToString() + "</td>";
                        else
                            strHtml += "<td align=\"left\" >&nbsp;</td>";
                        strHtml += "</tr>";
                    }
                }

                /////////product total
                strHtml += "<tr style=\"background-color:White;\">";
                strHtml += "<td align=right colspan=12><div style=\"background-color:lavender;width:50%;\">";
                strHtml += "<table width=\"100%\" border=" + Convert.ToChar(34) + 0 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                strHtml += "<tr><td align=left  style=\"font-size:xx-small;width:40%;\">" + dt2.Rows[k - 1]["SETTLEMENTNAME"].ToString().Trim() + "</td>";
                if (dt2.Rows[k - 1]["MTMDR"] != DBNull.Value)
                    strHtml += " <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" ><td align=\"right\" style=\"font-size:xx-small;width:10%;\" ><td align=\"right\" style=\"font-size:xx-small;width:10%;\" >" + dt2.Rows[k - 1]["MTMDR"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;width:10%;\" >&nbsp;</td>";
                if (dt2.Rows[k - 1]["MTMCR"] != DBNull.Value)
                    strHtml += " <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" ><td align=\"right\" style=\"font-size:xx-small;width:10%;\" ><td align=\"right\" style=\"font-size:xx-small;width:10%;\" >" + dt2.Rows[k - 1]["MTMCR"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;width:10%;\" >&nbsp;</td>";
                strHtml += "</tr></table></div></td></tr>";

                ///////net all product total
                strHtml += "<tr style=\"background-color:White;\">";
                strHtml += "<td align=right colspan=12><div style=\"background-color:lavender;width:50%;\">";
                strHtml += "<table width=\"100%\" border=" + Convert.ToChar(34) + 0 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

                if (dt2.Rows[0]["NETPRMSETTLEMENTNAME"] != DBNull.Value)
                {
                    strHtml += "<tr><td align=left  style=\"font-size:xx-small;width:40%;\">" + dt2.Rows[0]["NETPRMSETTLEMENTNAME"].ToString().Trim() + "</td>";
                    if (dt2.Rows[0]["NETPRMSETTLEMENTDR"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" ><td align=\"right\" style=\"font-size:xx-small;width:10%;\" ><td align=\"right\" style=\"font-size:xx-small;width:10%;\" >" + dt2.Rows[0]["NETPRMSETTLEMENTDR"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" style=\"font-size:xx-small;width:10%;\" >&nbsp;</td>";
                    if (dt2.Rows[0]["NETPRMSETTLEMENTCR"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;width:10%;\" ><td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" ><td align=\"right\" style=\"font-size:xx-small;width:10%;\" >" + dt2.Rows[0]["NETPRMSETTLEMENTCR"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" style=\"font-size:xx-small;width:10%;\" >&nbsp;</td>";
                    strHtml += "</tr>";
                }

                strHtml += "</table></div></td></tr>";

            }
            /////////for FOR Premium Obligation end
            ////////FOR Options Final Settlement
            viewproduct = new DataView();
            viewproduct = dt1.DefaultView;
            viewproduct.RowFilter = "Identifier='OPT' AND TRADECATEGORY IN ('Exercised','Assigned')";
            DataTable dt3 = new DataTable();
            dt3 = viewproduct.ToTable();



            if (dt3.Rows.Count > 0)
            {
                ////////html header begin
                strHtml += "<tr style=\"background-color: #DBEEF3;\">";
                strHtml += "<td align=\"center\" colspan=2><b>Options Final Settlement</b></td>";
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

                for (int p = 0; p < dt3.Rows.Count; p++)
                {

                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    strHtml += "<td colspan=2 align=left style=\"color:maroon;\">" + dt3.Rows[p]["SYMBOL"].ToString() + "</td>";
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt3.Rows[p]["STRIKEPRICE"].ToString() + "</td>";
                    strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" >" + dt3.Rows[p]["TRADEDATE"].ToString() + "</td>";
                    strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" >" + dt3.Rows[p]["TRADECATEGORY"].ToString() + "</td>";
                    if (dt3.Rows[p]["BUYQTY"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt3.Rows[p]["BUYQTY"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";

                    if (dt3.Rows[p]["SELLQTY"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt3.Rows[p]["SELLQTY"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";

                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt3.Rows[p]["MKTRATE"].ToString() + "</td>";
                    if (dt3.Rows[p]["BRKG"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt3.Rows[p]["BRKG"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt3.Rows[p]["NETRATE"].ToString() + "</td>";
                    if (dt3.Rows[p]["NETAMNTDR"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt3.Rows[p]["NETAMNTDR"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";
                    if (dt3.Rows[p]["NETAMNTCR"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt3.Rows[p]["NETAMNTCR"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" >&nbsp;</td>";
                    strHtml += "</tr>";

                }
                /////////net option detail

                strHtml += "<tr style=\"background-color:White;\">";
                strHtml += "<td align=right colspan=12><div style=\"background-color:lavender;width:50%;\">";
                strHtml += "<table width=\"100%\" border=" + Convert.ToChar(34) + 0 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

                if (dt3.Rows[0]["NETOPTIONSETTLEMENTNAME"] != DBNull.Value)
                {
                    strHtml += "<tr><td align=left  style=\"font-size:xx-small;width:40%;\">" + dt3.Rows[0]["NETOPTIONSETTLEMENTNAME"].ToString().Trim() + "</td>";
                    if (dt3.Rows[0]["NETOPTIONSETTLEMENTDR"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" ><td align=\"right\" style=\"font-size:xx-small;width:10%;\" ><td align=\"right\" style=\"font-size:xx-small;width:10%;\" >" + dt3.Rows[0]["NETOPTIONSETTLEMENTDR"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" style=\"font-size:xx-small;width:10%;\" >&nbsp;</td>";
                    if (dt3.Rows[0]["NETOPTIONSETTLEMENTCR"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;width:10%;\" ><td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" ><td align=\"right\" style=\"font-size:xx-small;width:10%;\" >" + dt3.Rows[0]["NETOPTIONSETTLEMENTCR"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"left\" style=\"font-size:xx-small;width:10%;\" >&nbsp;</td>";
                    strHtml += "</tr>";

                }
                strHtml += "</table></div></td></tr>";
            }

            //////charges result
            if (dt1.Rows[0]["CLIENTNETAMNTDR"] != DBNull.Value || dt1.Rows[0]["CLIENTNETAMNTCR"] != DBNull.Value)
            {

                strHtml += "<tr style=\"height:1px;background-color:White;\"><td colspan=11>&nbsp;</td></tr><tr style=\"background-color:White;\">";
                strHtml += "<td align=right colspan=11><div style=\"border: solid 1px black;width:50%;\">";
                strHtml += "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

                strHtml += "<tr><td align=left style=\"width:40%;\">&nbsp;</td>";
                strHtml += "<td align=\"right\" style=\"width:10%;color:black;\"><b>Amount Dr.</b></td>";
                strHtml += "<td align=\"right\" style=\"width:10%;color:black;\"><b>Amount Cr.</b></td>";
                strHtml += "</tr>";

                strHtml += "<tr><td align=left style=\"width:40%;\">" + dt1.Rows[0]["TOTALOBLIGATIONNAME"].ToString().Trim() + "</td>";
                if (dt1.Rows[0]["TOTALOBLIGATIONDR"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"width:10%;color:maroon;\">" + dt1.Rows[0]["TOTALOBLIGATIONDR"].ToString() + "</b></td>";
                else
                    strHtml += "<td style=\"width:5%;\"> &nbsp;</td>";
                if (dt1.Rows[0]["TOTALOBLIGATIONCR"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"width:10%;color:maroon;\">" + dt1.Rows[0]["TOTALOBLIGATIONCR"].ToString() + "</td>";
                else
                    strHtml += "<td style=\"width:10%;\"> &nbsp;</td>";
                strHtml += "</tr>";
                ////////srvtax on brkg
                if (dt1.Rows[0]["SRVTAXONBRKG"] != DBNull.Value)
                {
                    strHtml += "<tr><td align=left style=\"width:40%;\">Serv Tax & Cess on Brokerage :</td>";
                    strHtml += "<td align=\"right\" style=\"width:10%;color:maroon;\">" + dt1.Rows[0]["SRVTAXONBRKG"].ToString() + "</b></td>";
                    strHtml += "<td style=\"width:10%;\"> &nbsp;</td>";
                    strHtml += "</tr>";
                }
                //////////tran charge
                if (dt1.Rows[0]["TRANCHARGE"] != DBNull.Value)
                {
                    strHtml += "<tr><td align=left style=\"width:40%;\">Transaction Charges :</td>";
                    strHtml += "<td align=\"right\" style=\"width:10%;color:maroon;\">" + dt1.Rows[0]["TRANCHARGE"].ToString() + "</b></td>";
                    strHtml += "<td style=\"width:10%;\"> &nbsp;</td>";
                    strHtml += "</tr>";
                }

                //////////srvtax on tran charge
                if (dt1.Rows[0]["STTAXTRANCHARGE"] != DBNull.Value)
                {
                    strHtml += "<tr><td align=left style=\"width:40%;\">Serv Tax & Cess on Tran Charge:</td>";
                    strHtml += "<td align=\"right\" style=\"width:10%;color:maroon;\">" + dt1.Rows[0]["STTAXTRANCHARGE"].ToString() + "</b></td>";
                    strHtml += "<td style=\"width:10%;\"> &nbsp;</td>";
                    strHtml += "</tr>";
                }

                //////////stamp duty
                if (dt1.Rows[0]["STAMP"] != DBNull.Value)
                {
                    strHtml += "<tr><td align=left style=\"width:40%;\">Stamp Duty :</td>";
                    strHtml += "<td align=\"right\" style=\"width:10%;color:maroon;\">" + dt1.Rows[0]["STAMP"].ToString() + "</b></td>";
                    strHtml += "<td style=\"width:10%;\"> &nbsp;</td>";
                    strHtml += "</tr>";
                }

                //////////STT
                if (dt1.Rows[0]["STTAX"] != DBNull.Value)
                {
                    strHtml += "<tr><td align=left style=\"width:40%;\">STT Tax :</td>";
                    strHtml += "<td align=\"right\" style=\"width:10%;color:maroon;\">" + dt1.Rows[0]["STTAX"].ToString() + "</b></td>";
                    strHtml += "<td style=\"width:10%;\"> &nbsp;</td>";
                    strHtml += "</tr>";
                }
                //////////Sebi Fee
                if (dt1.Rows[0]["SEBICHARGE"] != DBNull.Value)
                {
                    strHtml += "<tr><td align=left style=\"width:40%;\">Sebi Charge :</td>";
                    strHtml += "<td align=\"right\" style=\"width:10%;color:maroon;\">" + dt1.Rows[0]["SEBICHARGE"].ToString() + "</b></td>";
                    strHtml += "<td style=\"width:10%;\"> &nbsp;</td>";
                    strHtml += "</tr>";
                }

                // ////////total charges
                if (dt1.Rows[0]["TOTALCHARGE"] != DBNull.Value)
                {
                    strHtml += "<tr><td align=left style=\"width:40%;\">Total Charges :</td>";
                    strHtml += "<td align=\"right\" style=\"width:10%;color:maroon;\">" + dt1.Rows[0]["TOTALCHARGE"].ToString() + "</b></td>";
                    strHtml += "<td style=\"width:10%;\"> &nbsp;</td>";
                    strHtml += "</tr>";
                }

                // ////////net bill 
                if (dt1.Rows[0]["NETBILLAMNTDR"] != DBNull.Value || dt1.Rows[0]["NETBILLAMNTCR"] != DBNull.Value)
                {
                    strHtml += "<tr><td align=left style=\"width:40%;\"><u>Net Bill Amount :</u></td>";
                    if (dt1.Rows[0]["NETBILLAMNTDR"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"width:10%;color:maroon;\">" + dt1.Rows[0]["NETBILLAMNTDR"].ToString() + "</b></td>";
                    else
                        strHtml += "<td style=\"width:10%;\"> &nbsp;</td>";
                    if (dt1.Rows[0]["NETBILLAMNTCR"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"width:10%;color:maroon;\">" + dt1.Rows[0]["NETBILLAMNTCR"].ToString() + "</td>";
                    else
                        strHtml += "<td style=\"width:10%;\"> &nbsp;</td>";
                    strHtml += "</tr>";
                }

                // ////////net fund 
                if (dt1.Rows[0]["NETFUNDDR"] != DBNull.Value || dt1.Rows[0]["NETFUNDCR"] != DBNull.Value)
                {
                    strHtml += "<tr><td align=left style=\"width:40%;\">" + dt1.Rows[0]["NETFUNDNAME"].ToString().Trim() + "</td>";
                    if (dt1.Rows[0]["NETFUNDDR"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"width:10%;color:maroon;\">" + dt1.Rows[0]["NETFUNDDR"].ToString() + "</b></td>";
                    else
                        strHtml += "<td style=\"width:10%;\"> &nbsp;</td>";
                    if (dt1.Rows[0]["NETFUNDCR"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"width:10%;color:maroon;\">" + dt1.Rows[0]["NETFUNDCR"].ToString() + "</td>";
                    else
                        strHtml += "<td style=\"width:10%;\"> &nbsp;</td>";
                    strHtml += "</tr>";
                }

                // ////////net amnt receivabel 
                if (dt1.Rows[0]["CLIENTNETAMNTDR"] != DBNull.Value || dt1.Rows[0]["CLIENTNETAMNTCR"] != DBNull.Value)
                {
                    strHtml += "<tr><td align=left style=\"width:40%;\">" + dt1.Rows[0]["CLIENTNETAMNTNAME"].ToString().Trim() + "</td>";
                    if (dt1.Rows[0]["CLIENTNETAMNTDR"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"width:10%;color:maroon;\">" + dt1.Rows[0]["CLIENTNETAMNTDR"].ToString() + "</b></td>";
                    else
                        strHtml += "<td style=\"width:10%;\"> &nbsp;</td>";
                    if (dt1.Rows[0]["CLIENTNETAMNTCR"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"width:10%;color:maroon;\">" + dt1.Rows[0]["CLIENTNETAMNTCR"].ToString() + "</td>";
                    else
                        strHtml += "<td style=\"width:10%;\"> &nbsp;</td>";
                    strHtml += "</tr>";
                }


                strHtml += "</table></div></td></tr>";
            }
            ////////margin
            if (dt1.Rows[0]["SHORTAGE"] != DBNull.Value || dt1.Rows[0]["EXCESS"] != DBNull.Value)
            {
                strHtml += "<tr style=\"background-color: #DBEEF3;\">";
                strHtml += "<td align=\"center\" ><b>Margin Summary</b></td>";
                strHtml += "<td align=\"center\" ><b>Span Margin</b></td>";
                strHtml += "<td align=\"center\"><b>Premium Margin</b></td>";
                strHtml += "<td align=\"center\"><b>Total Margin</b></td>";
                strHtml += "<td align=\"center\"><b>Exposure Margin</b></td>";
                strHtml += "<td align=\"center\"><b>Applicable Margin</b></td>";
                strHtml += "<td align=\"center\"><b>Cash Dep</b></td>";
                strHtml += "<td align=\"center\"><b>FDR</b></td>";
                strHtml += "<td align=\"center\"><b>Collaterals</b></td>";
                strHtml += "<td align=\"center\"><b>Total Deposit</b></td>";
                strHtml += "<td align=\"center\"><b>Shortage</b></td>";
                strHtml += "<td align=\"center\"><b>Excess</b></td></tr>";

                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                strHtml += "<td align=\"left\" >&nbsp;</td>";
                if (dt1.Rows[0]["SPANMRGN"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt1.Rows[0]["SPANMRGN"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";
                if (dt1.Rows[0]["PRMMRGN"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt1.Rows[0]["PRMMRGN"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";
                if (dt1.Rows[0]["TOTMRGN"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt1.Rows[0]["TOTMRGN"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";
                if (dt1.Rows[0]["EXPOSURMRGN"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt1.Rows[0]["EXPOSURMRGN"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";
                if (dt1.Rows[0]["APPMRGN"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt1.Rows[0]["APPMRGN"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";
                if (dt1.Rows[0]["CASHDEPnew"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt1.Rows[0]["CASHDEPnew"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";
                if (dt1.Rows[0]["CASHDEPnew1"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt1.Rows[0]["CASHDEPnew1"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";
                if (dt1.Rows[0]["COLLATERAL"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt1.Rows[0]["COLLATERAL"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";
                if (dt1.Rows[0]["TOTDEP"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt1.Rows[0]["TOTDEP"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";
                if (dt1.Rows[0]["SHORTAGE"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt1.Rows[0]["SHORTAGE"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";
                if (dt1.Rows[0]["EXCESS"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt1.Rows[0]["EXCESS"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" >&nbsp;</td>";
                strHtml += "</tr>";
            }
            ////////last result
            strHtml += "<tr style=\"background-color:White;\">";
            strHtml += "<td align=right colspan=12><div style=\"background-color:lavender;width:50%;\">";
            strHtml += "<table width=\"100%\" border=" + Convert.ToChar(34) + 0 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

            if (dt1.Rows[0]["CLIENTLASTNAME"] != DBNull.Value)
            {
                strHtml += "<tr><td align=left  style=\"font-size:xx-small;width:40%;\">" + dt1.Rows[0]["CLIENTLASTNAME"].ToString() + "</td>";
                if (dt1.Rows[0]["CLIENTLASTDR"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" ><td align=\"right\" style=\"font-size:xx-small;width:10%;\" >" + dt1.Rows[0]["CLIENTLASTDR"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;width:10%;\" >&nbsp;</td>";
                if (dt1.Rows[0]["CLIENTLASTCR"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;width:10%;\" ><td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" > <td align=\"right\" style=\"font-size:xx-small;width:10%;\" >" + dt1.Rows[0]["CLIENTLASTCR"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;width:10%;\" >&nbsp;</td>";
                strHtml += "</tr>";
            }
            strHtml += "</table></div></td></tr>";



            strHtml += "</table>";
            DIVdisplayPERIOD.InnerHtml = strHtml1;
            display.InnerHtml = strHtml;
            ViewState["mail"] = strHtml1 + strHtml;

        }
    }
}