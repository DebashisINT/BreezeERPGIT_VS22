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
    public partial class Reports_MarginStocksIORegister : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        BusinessLogicLayer.Reports rep = new BusinessLogicLayer.Reports();
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        DataSet ds = new DataSet();
        ExcelFile objExcel = new ExcelFile();
        DataTable dtExport = new DataTable();
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
                Date();
                BindDPAccounts();
                SegmentName();
                CorpAction();
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
        void Date()
        {
            DtFor.EditFormatString = oconverter.GetDateFormat("Date");
            DtFor.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
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
                    if (idlist[0] != "ClientsSelction")
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

                if (idlist[0] == "Scrips")
                {
                    data = "Scrips~" + str;
                }
                else if (idlist[0] == "ClientsSelction")
                {
                    data = "ClientsSelction~" + str;
                }
                else if (idlist[0] == "Group")
                {
                    data = "Group~" + str;
                }
                else if (idlist[0] == "Branch")
                {
                    data = "Branch~" + str;
                }
                else if (idlist[0] == "Segment")
                {
                    data = "Segment~" + str;
                }
                else if (idlist[0] == "MAILEMPLOYEE")
                {
                    data = "MAILEMPLOYEE~" + str;
                }
            }
        }
        void SegmentName()
        {
            if (Session["ExchangeSegmentID"].ToString() == "1")
                litSegmentMain.InnerText = "NSE - CM";
            else if (Session["ExchangeSegmentID"].ToString() == "2")
                litSegmentMain.InnerText = "NSE - FO";
            else if (Session["ExchangeSegmentID"].ToString() == "3")
                litSegmentMain.InnerText = "NSE - CDX";
            else if (Session["ExchangeSegmentID"].ToString() == "4")
                litSegmentMain.InnerText = "BSE - CM";
            else if (Session["ExchangeSegmentID"].ToString() == "7")
                litSegmentMain.InnerText = "MCX - COMM";
            else if (Session["ExchangeSegmentID"].ToString() == "9")
                litSegmentMain.InnerText = "NCDEX - COMM";
            else if (Session["ExchangeSegmentID"].ToString() == "5")
                litSegmentMain.InnerText = "BSE - FO";
            else if (Session["ExchangeSegmentID"].ToString() == "6")
                litSegmentMain.InnerText = "BSE - CDX";
            else if (Session["ExchangeSegmentID"].ToString() == "8")
                litSegmentMain.InnerText = "MCXSX - CDX";
            else if (Session["ExchangeSegmentID"].ToString() == "10")
                litSegmentMain.InnerText = "DGCX - COMM";
            else if (Session["ExchangeSegmentID"].ToString() == "11")
                litSegmentMain.InnerText = "NMCE - COMM";
            else if (Session["ExchangeSegmentID"].ToString() == "12")
                litSegmentMain.InnerText = "ICEX - COMM";
            else if (Session["ExchangeSegmentID"].ToString() == "15")
                litSegmentMain.InnerText = "CSE - CM";
            else if (Session["ExchangeSegmentID"].ToString() == "14")
                litSegmentMain.InnerText = "NSEL - SPOT";
            else if (Session["ExchangeSegmentID"].ToString() == "13")
                litSegmentMain.InnerText = "USE - CDX";
            else if (Session["ExchangeSegmentID"].ToString() == "19")
                litSegmentMain.InnerText = "MCXSX - CM";
            else if (Session["ExchangeSegmentID"].ToString() == "20")
                litSegmentMain.InnerText = "MCXSX - FO";
            else if (Session["ExchangeSegmentID"].ToString() == "17")
                litSegmentMain.InnerText = "ACE - COMM";
            else if (Session["ExchangeSegmentID"].ToString() == "18")
                litSegmentMain.InnerText = "INMX - COMM";
            else if (Session["ExchangeSegmentID"].ToString() == "21")
                litSegmentMain.InnerText = "BFX - COMM";
            else if (Session["ExchangeSegmentID"].ToString() == "22")
                litSegmentMain.InnerText = "INSX - COMM";
            else if (Session["ExchangeSegmentID"].ToString() == "23")
                litSegmentMain.InnerText = "INFX - COMM";

            HiddenField_Segment.Value = HttpContext.Current.Session["usersegid"].ToString().Trim();
        }
        public void BindDPAccounts()
        {
            string[] InputName = new string[1];
            string[] InputType = new string[1];
            string[] InputValue = new string[1];

            InputName[0] = "Criteria";
            InputType[0] = "V";

            InputValue[0] = " where rtrim(DPACCounts_AccountType) in ('[HOLDBK]','[MRGIN]') and DPACCounts_CompanyID='" + Session["LastCompany"].ToString() + "'";

            DataTable DtDPAcc = SQLProcedures.SelectProcedureArr("[Fetch_AccountName]", InputName, InputType, InputValue);

            ddlDPAc.Items.Clear();

            if (DtDPAcc.Rows.Count > 0)
            {
                ddlDPAc.DataTextField = "ShortName";
                ddlDPAc.DataValueField = "ID";
                ddlDPAc.DataSource = DtDPAcc;
                ddlDPAc.DataBind();
                ddlDPAc.Items.Insert(0, new ListItem("ALL", "0"));
            }
            else
            {
                ddlDPAc.Items.Insert(0, new ListItem("ALL", "0"));
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
        void CorpAction()
        {
            ddlCorpActionType.Items.Clear();
            DataTable DtCorp = oDBEngine.GetDataTable("trans_corporateactions", "distinct ltrim(rtrim(corpaction_type)) as CorpType", null);
            if (DtCorp.Rows.Count > 0)
            {
                ddlCorpActionType.DataSource = DtCorp;
                ddlCorpActionType.DataTextField = "CorpType";
                ddlCorpActionType.DataValueField = "CorpType";
                ddlCorpActionType.DataBind();
                ddlCorpActionType.Items.Insert(0, new ListItem("ALL", "ALL"));
                DtCorp.Dispose();

            }
            else
            {
                ddlCorpActionType.Items.Insert(0, new ListItem("ALL", "ALL"));
            }
        }
        protected void btnhide_Click(object sender, EventArgs e)
        {

            if (ddlGroup.SelectedItem.Value.ToString() == "2")
            {
                if (HiddenField_Group.Value.ToString().Trim() == "")
                {
                    BindGroup();
                }
            }
        }

        protected void btnScreen_Click(object sender, EventArgs e)
        {
            BindFunctionCall();
        }






        void ProcedureCall()
        {
            string ClientId = "";
            string GrpType = "";
            string Grpid = "";
            string Accountid = "";
            if (ddlGroup.SelectedItem.Value.ToString().Trim() == "3")
            {
                if (rdBranchClientAll.Checked)
                {
                    ClientId = "ALL";
                }
                else
                {
                    ClientId = HiddenField_Client.Value;
                }
                GrpType = "BRANCH";
                Grpid = "ALL";
            }
            else
            {
                ClientId = "ALL";
                if (ddlGroup.SelectedItem.Value.ToString().Trim() == "1")
                {
                    GrpType = "BRANCH";
                    if (rdBranchClientAll.Checked)
                    {
                        Grpid = "ALL";
                    }
                    else
                    {
                        Grpid = HiddenField_Branch.Value;
                    }
                }

                if (ddlGroup.SelectedItem.Value.ToString().Trim() == "2")
                {
                    GrpType = ddlgrouptype.SelectedItem.Text.ToString().Trim();
                    if (rdddlgrouptypeAll.Checked)
                    {
                        Grpid = "ALL";
                    }
                    else
                    {
                        Grpid = HiddenField_Group.Value;
                    }
                }

            }
            if (ddlDPAc.SelectedItem.Value.ToString().Trim() == "0")
            {
                Accountid = "ALL";
            }
            else
            {
                string DPAc = ddlDPAc.SelectedItem.Value;
                string[] DPAcId = DPAc.Split('~');

                Accountid = DPAcId[0].ToString().Trim();
            }
            ds = rep.Report_MarginStocksInwardOutwardRegister(DtFor.Value.ToString(), ClientId, GrpType, Grpid,
                rdbSegmentAll.Checked ? "ALL" : HiddenField_Segment.Value, Session["LastCompany"].ToString(),
                Session["LastFinYear"].ToString().Trim(), rdbInstrumentAll.Checked ? "ALL" : HiddenField_Scrips.Value,
                Session["userbranchHierarchy"].ToString(), Accountid, ChkPendingPurchase.Checked ? "Chk" : "UnChk",
                ChkPendingSales.Checked ? "Chk" : "UnChk", ChkDpDetails.Checked ? "Chk" : "UnChk",
                 ChkLedgerBaln.Checked ? "Chk" : "UnChk", ChkCashMarginDep.Checked ? "Chk" : "UnChk", ddlreporttype.SelectedItem.Value.ToString().Trim(),
                 rdbCloseprice.Checked ? "Close" : "Trade", ChkNegative.Checked ? "Chk" : "UnChk", DdlSecurityType.SelectedItem.Value.ToString().Trim(),
                 rdbVarmarginElm.Checked ? "AppMrgn" : "VarMrgn", ChkCorpAcType.Checked.ToString().Trim(), ddlCorpActionType.SelectedItem.Value.ToString().Trim());


            ViewState["dataset"] = ds;
        }



        void BindFunctionCall()
        {
            ProcedureCall();
            ds = (DataSet)ViewState["dataset"];
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ddlGeneration.SelectedItem.Value.ToString() == "1")///Screen
                {
                    if (ddlreporttype.SelectedItem.Value.ToString() == "1")///Client Wise [Default]
                    {
                        ClientBind(ds);
                    }
                    if ((ddlreporttype.SelectedItem.Value.ToString() == "2"))////DP Account+Scrip+Client
                    {
                        AccountBind(ds);
                    }


                    if ((ddlreporttype.SelectedItem.Value.ToString() == "3"))////DP Account+Scrip+Client
                    {
                        AccountBind_DP(ds);
                    }
                }
                if (ddlGeneration.SelectedItem.Value.ToString() == "2")///Export to PDF
                {
                    if (ddlreporttype.SelectedItem.Value.ToString() == "1")///Client Wise [Default]
                    {
                        ExportToClientWisePDF(ds);
                    }
                    if (ddlreporttype.SelectedItem.Value.ToString() == "2")////DP Account+Scrip+Client
                    {
                        ExportToAccountWisePDF(ds);
                    }

                    if (ddlreporttype.SelectedItem.Value.ToString() == "3")////DP Account+Scrip+DP
                    {
                        ExportToAccountWisePDF_DP(ds);
                    }



                }
                if (ddlGeneration.SelectedItem.Value.ToString() == "3")///Email
                {
                    if (ddlreporttype.SelectedItem.Value.ToString() == "1")///Client Wise [Default]
                    {
                        if (ddloptionformail.SelectedItem.Value.ToString().Trim() == "1")////////Branch Group
                        {
                            branhgroupemail(ds);
                        }
                        if (ddloptionformail.SelectedItem.Value.ToString().Trim() == "2")//////////User Wise
                        {
                            optionforemail(ds);
                        }
                        if (ddloptionformail.SelectedItem.Value.ToString().Trim() == "3")/////////Client Wise
                        {
                            clientwiseemail(ds);
                        }
                    }
                    if (ddlreporttype.SelectedItem.Value.ToString() == "2")////DP Account+Scrip+Client
                    {
                        userwiseemail(ds);
                    }

                    if (ddlreporttype.SelectedItem.Value.ToString() == "3")////DP Account+Scrip+DP
                    {
                        userwiseemail_DP(ds);
                    }

                }

                if (ddlGeneration.SelectedItem.Value.ToString() == "4")///Export to Excel
                {
                    if (ddlreporttype.SelectedItem.Value.ToString() == "1")///Client Wise [Default]
                    {
                        ExportToExcelClientWise(ds);
                    }
                    if (ddlreporttype.SelectedItem.Value.ToString() == "2")////DP Account+Scrip+Client
                    {
                        ExportToExcelAccountWise(ds);
                    }

                    if (ddlreporttype.SelectedItem.Value.ToString() == "3")////DP Account+Scrip+DP
                    {

                        //ExportToExcelAccountWiseDP(ds);

                        ExportDP(ds);

                    }
                }


            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordStatus", "RecordStatus('1');", true);

            }
        }


        protected string GetRowColor(int i)
        {
            if (i++ % 2 == 0)
                return "#fff0f5";
            else
                return "White";
        }
        public static bool IsNumeric(object value)
        {
            double dbl;
            return double.TryParse(value.ToString(), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out dbl);
        }
        void ClientBind(DataSet ds)
        {
            DataView viewData = new DataView();
            viewData = ds.Tables[0].DefaultView;
            viewData.RowFilter = " ClientId not in ('G','ZZZ')";
            DataTable dt = new DataTable();
            dt = viewData.ToTable();

            DataTable Distinctclient = new DataTable();
            DataView viewClient = new DataView(dt);
            Distinctclient = viewClient.ToTable(true, new string[] { "ClientId" });

            if (Distinctclient.Rows.Count > 0)
            {
                cmb.DataSource = Distinctclient;
                cmb.DataValueField = "ClientId";
                cmb.DataTextField = "ClientId";
                cmb.DataBind();

            }

            LastPage = Distinctclient.Rows.Count - 1;
            CurrentPage = 0;
            bind_Details();
        }
        void AccountBind(DataSet ds)
        {
            DataView viewData = new DataView();
            viewData = ds.Tables[0].DefaultView;
            viewData.RowFilter = " MainOrder='3'";
            DataTable dt = new DataTable();
            dt = viewData.ToTable();

            DataTable DistinctAccountid = new DataTable();
            DataView viewAc = new DataView(dt);
            DistinctAccountid = viewAc.ToTable(true, new string[] { "OwnAcname" });

            if (DistinctAccountid.Rows.Count > 0)
            {
                cmb.DataSource = DistinctAccountid;
                cmb.DataValueField = "OwnAcname";
                cmb.DataTextField = "OwnAcname";
                cmb.DataBind();

            }

            LastPage = DistinctAccountid.Rows.Count - 1;
            CurrentPage = 0;
            bind_Details();
        }

        void bind_Details()
        {
            cmb.SelectedIndex = CurrentPage;
            ds = (DataSet)ViewState["dataset"];
            if (ddlreporttype.SelectedItem.Value.ToString() == "1")///Client Wise [Default]
            {
                FnHtmlClientWise(ds, cmb.SelectedItem.Value.ToString());
            }
            if (ddlreporttype.SelectedItem.Value.ToString() == "2")////DP Account+Scrip+Client
            {
                FnHtmlAccountWise(ds, cmb.SelectedItem.Value.ToString());
            }


            if (ddlreporttype.SelectedItem.Value.ToString() == "3")////DP Account+Scrip+DP
            {
                // FnHtmlAccountWiseDP(ds, cmb.SelectedItem.Value.ToString());



                FnHtmlClientWiseDP(ds, cmb.SelectedItem.Value.ToString());
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordStatus", "RecordStatus('3');", true);
            ShowHidePreviousNext_of_Clients();
        }

        void FnHtmlClientWise(DataSet ds, string Clientid)
        {
            //////////For header
            String strHtmlheader = String.Empty;
            string str = null;
            if (ddlGroup.SelectedItem.Value.ToString() == "2")
            {
                str = "[" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "]";
            }
            str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report For " + oconverter.ArrangeDate2(DtFor.Value.ToString());
            str = str + " ; Report View : " + ddlreporttype.SelectedItem.Text.ToString().Trim();

            strHtmlheader = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtmlheader += "<tr><td align=\"left\" colspan=" + ds.Tables[0].Columns.Count + " style=\"color:Blue;\">" + str + "</td></tr></table>";


            ////////Detail Display
            String strHtml = String.Empty;
            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";




            DataView viewclient = new DataView();
            viewclient = ds.Tables[0].DefaultView;
            viewclient.RowFilter = "ClientId='" + Clientid.ToString().Trim() + "'";
            DataTable dt = new DataTable();
            dt = viewclient.ToTable();

            dt.Columns.Remove("ClientId");
            dt.Columns.Remove("Grpname");
            dt.Columns.Remove("GrpId");
            dt.Columns.Remove("GrpEmail");


            strHtml += "<tr style=\"background-color:lavender ;text-align:left\">";
            strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; ><b>" + dt.Rows[0][0].ToString().Trim() + "</b></td>";
            strHtml += "</tr>";

            dt.Rows[0].Delete();

            //////////////TABLE HEADER BIND
            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                strHtml += "<td align=\"center\" style=\"font-size:smaller;\" nowrap=nowrap;><b>" + dt.Columns[i].ColumnName + "</b></td>";
            }
            strHtml += "</tr>";

            int flag = 0;
            foreach (DataRow dr1 in dt.Rows)
            {
                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (dr1[j] != DBNull.Value)
                    {
                        if (dr1[j].ToString().Trim().StartsWith("**") || dr1[j].ToString().Trim().StartsWith("Net:") || dr1[j].ToString().Trim().StartsWith("Total:"))
                        {
                            strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\"><b>" + dr1[j] + "</b></td>";
                        }
                        else if (dr1[j].ToString().Trim().StartsWith("Test"))
                        {
                            strHtml += "<td>&nbsp;</td>";
                        }
                        else
                        {
                            if (IsNumeric(dr1[j].ToString()) == true)
                            {
                                strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                            }
                            else
                            {
                                strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                            }
                        }
                    }
                    else
                    {
                        strHtml += "<td>&nbsp;</td>";
                    }
                }

                strHtml += "</tr>";
            }
            strHtml += "</table>";
            if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "1")
            {
                DivHeader.InnerHtml = strHtmlheader;
                Divdisplay.InnerHtml = strHtml;
            }
            if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "3")
            {
                ViewState["mail"] = strHtml;
            }



        }
        void FnHtmlAccountWise(DataSet ds, string Account)
        {
            //////////For header
            String strHtmlheader = String.Empty;
            string str = null;
            if (ddlGroup.SelectedItem.Value.ToString() == "2")
            {
                str = "[" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "]";
            }
            str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report For " + oconverter.ArrangeDate2(DtFor.Value.ToString());
            str = str + " ; Report View : " + ddlreporttype.SelectedItem.Text.ToString().Trim();

            strHtmlheader = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtmlheader += "<tr><td align=\"left\" colspan=" + ds.Tables[0].Columns.Count + " style=\"color:Blue;\">" + str + "</td></tr></table>";


            ////////Detail Display
            String strHtml = String.Empty;
            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";




            DataView viewclient = new DataView();
            viewclient = ds.Tables[0].DefaultView;
            viewclient.RowFilter = "OwnAcname='" + Account.ToString().Trim() + "'";
            DataTable dt = new DataTable();
            dt = viewclient.ToTable();

            dt.Columns.Remove("OwnAcname");

            strHtml += "<tr style=\"background-color:lavender ;text-align:left\">";
            strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; ><b>" + dt.Rows[0][0].ToString().Trim() + "</b></td>";
            strHtml += "</tr>";

            dt.Rows[0].Delete();

            //////////////TABLE HEADER BIND
            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (dt.Columns[i].ColumnName.ToString().Trim() != "MainOrder")
                {
                    strHtml += "<td align=\"center\" style=\"font-size:smaller;\" nowrap=nowrap;><b>" + dt.Columns[i].ColumnName + "</b></td>";
                }
            }
            strHtml += "</tr>";

            int flag = 0;
            string stcachk = "A";
            foreach (DataRow dr1 in dt.Rows)
            {
                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";

                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (dt.Columns[j].ColumnName.ToString().Trim() != "MainOrder")
                    {
                        if (dt.Rows[flag - 1]["MainOrder"].ToString().Trim() == "2")
                        {
                            if (dr1[j].ToString().Trim().StartsWith("**"))
                            {
                                strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\" colspan=8><b>" + dr1[j] + "</b></td>";
                            }

                        }
                        else
                        {
                            if (dr1[j] != DBNull.Value)
                            {
                                if (dr1[j].ToString().Trim().StartsWith("Account") || dr1[j].ToString().Trim().StartsWith("Total:"))
                                {
                                    strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\"><b>" + dr1[j] + "</b></td>";
                                }
                                else if (dr1[j].ToString().Trim().StartsWith("Test"))
                                {
                                    strHtml += "<td>&nbsp;</td>";
                                }
                                else
                                {
                                    if (IsNumeric(dr1[j].ToString()) == true)
                                    {
                                        strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                                    }
                                    else
                                    {
                                        strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                                    }
                                }
                            }
                            else
                            {
                                strHtml += "<td>&nbsp;</td>";

                            }

                        }



                    }

                }
                strHtml += "</tr>";
            }
            strHtml += "</table>";
            if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "1")
            {
                DivHeader.InnerHtml = strHtmlheader;
                Divdisplay.InnerHtml = strHtml;
            }
            if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "3")
            {
                ViewState["mail"] = strHtml;
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
        void ExportToExcelClientWise(DataSet ds)
        {
            DataTable dtExport = ds.Tables[0].Copy();
            dtExport.Columns.Remove("ClientId");
            dtExport.Columns.Remove("Grpname");
            dtExport.Columns.Remove("GrpId");
            dtExport.Columns.Remove("GrpEmail");

            string str = null;
            if (ddlGroup.SelectedItem.Value.ToString() == "2")
            {
                str = "[" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "]";
            }
            str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report For " + oconverter.ArrangeDate2(DtFor.Value.ToString());
            str = str + " ; Report View : " + ddlreporttype.SelectedItem.Text.ToString().Trim();

            DataTable dtReportHeader = new DataTable();
            DataTable dtReportFooter = new DataTable();

            DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            HeaderRow[0] = CompanyName.Rows[0][0].ToString();
            dtReportHeader.Rows.Add(HeaderRow);
            DataRow DrRowR1 = dtReportHeader.NewRow();
            DrRowR1[0] = str.ToString().Trim();
            dtReportHeader.Rows.Add(DrRowR1);


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

            objExcel.ExportToExcelforExcel(dtExport, "Margin Stocks Inward/Outward Register", "Total:", dtReportHeader, dtReportFooter);

        }
        void ExportToExcelAccountWise(DataSet ds)
        {
            DataTable dtExport = ds.Tables[0].Copy();
            dtExport.Columns.Remove("OwnAcname");
            dtExport.Columns.Remove("MainOrder");

            string str = null;
            if (ddlGroup.SelectedItem.Value.ToString() == "2")
            {
                str = "[" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "]";
            }
            str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report For " + oconverter.ArrangeDate2(DtFor.Value.ToString());
            str = str + " ; Report View : " + ddlreporttype.SelectedItem.Text.ToString().Trim();

            DataTable dtReportHeader = new DataTable();
            DataTable dtReportFooter = new DataTable();

            DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            HeaderRow[0] = CompanyName.Rows[0][0].ToString();
            dtReportHeader.Rows.Add(HeaderRow);
            DataRow DrRowR1 = dtReportHeader.NewRow();
            DrRowR1[0] = str.ToString().Trim();
            dtReportHeader.Rows.Add(DrRowR1);


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

            objExcel.ExportToExcelforExcel(dtExport, "Margin Stocks Inward/Outward Register", "Total:", dtReportHeader, dtReportFooter);

        }
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            BindFunctionCall();
        }

        protected void ddlExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            ds = (DataSet)ViewState["dataset"];
            if (ddlExport.SelectedItem.Value.ToString().Trim() == "E")
            {
                if (ddlreporttype.SelectedItem.Value.ToString() == "1")///Client Wise [Default]
                {
                    ExportToExcelClientWise(ds);
                }
                if (ddlreporttype.SelectedItem.Value.ToString() == "2")////DP Account+Scrip+Client
                {
                    ExportToExcelAccountWise(ds);
                }

                if (ddlreporttype.SelectedItem.Value.ToString() == "3")////DP Account+Scrip+DP
                {
                    //ExportToExcelAccountWiseDP(ds);
                    ExportDP(ds);
                }


            }
            if (ddlExport.SelectedItem.Value.ToString().Trim() == "P")
            {
                if (ddlreporttype.SelectedItem.Value.ToString() == "1")///Client Wise [Default]
                {
                    ExportToClientWisePDF(ds);
                }
                if (ddlreporttype.SelectedItem.Value.ToString() == "2")////DP Account+Scrip+Client
                {
                    ExportToAccountWisePDF(ds);
                }

                if (ddlreporttype.SelectedItem.Value.ToString() == "3")////DP Account+Scrip+DP
                {
                    ExportToAccountWisePDF_DP(ds);
                }





            }
        }

        void ExportToClientWisePDF(DataSet ds)
        {
            byte[] logoinByte;
            ds.Tables[1].Columns.Add("Image", System.Type.GetType("System.Byte[]"));
            if (ChkLogoPrint.Checked == false)
            {
                if (oconverter.getLogoImage(HttpContext.Current.Server.MapPath(@"..\images\logo.bmp"), out logoinByte) != 1)
                {
                    ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "logo", "alert('Logo not Found.');", true);
                }
                else
                {
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        ds.Tables[1].Rows[i]["Image"] = logoinByte;

                    }
                }
            }
            // ds.Tables[0].WriteXmlSchema("E:\\RPTXSD\\collateral.xsd");
            //ds.Tables[1].WriteXmlSchema("E:\\RPTXSD\\CollateralCompany.xsd");
            ReportDocument report = new ReportDocument();
            report.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;

            string tmpPdfPath = string.Empty;
            tmpPdfPath = HttpContext.Current.Server.MapPath("..\\Reports\\collateral.rpt");
            report.Load(tmpPdfPath);
            report.SetDataSource(ds.Tables[0]);
            report.Subreports["CollateralCompany"].SetDataSource(ds.Tables[1]);


            report.VerifyDatabase();
            report.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "Margin Stocks Inward/Outward Register");
            report.Dispose();
            GC.Collect();
        }
        void ExportToAccountWisePDF(DataSet ds)
        {
            byte[] logoinByte;
            ds.Tables[1].Columns.Add("Image", System.Type.GetType("System.Byte[]"));
            if (ChkLogoPrint.Checked == false)
            {
                if (oconverter.getLogoImage(HttpContext.Current.Server.MapPath(@"..\images\logo.bmp"), out logoinByte) != 1)
                {
                    ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "logo", "alert('Logo not Found.');", true);
                }
                else
                {
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        ds.Tables[1].Rows[i]["Image"] = logoinByte;

                    }
                }
            }
            ////ds.Tables[0].WriteXmlSchema("E:\\RPTXSD\\collateral1.xsd");
            ////ds.Tables[1].WriteXmlSchema("E:\\RPTXSD\\CollateralCompany.xsd");
            ReportDocument report = new ReportDocument();
            report.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;

            string tmpPdfPath = string.Empty;
            tmpPdfPath = HttpContext.Current.Server.MapPath("..\\Reports\\collateralaccount.rpt");
            report.Load(tmpPdfPath);
            report.SetDataSource(ds.Tables[0]);
            report.Subreports["CollateralCompany"].SetDataSource(ds.Tables[1]);


            report.VerifyDatabase();
            report.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "Margin Stocks Inward/Outward Register");
            report.Dispose();
            GC.Collect();
        }
        protected void btnEmail_Click(object sender, EventArgs e)
        {
            BindFunctionCall();


        }
        void branhgroupemail(DataSet ds)
        {
            ViewState["GRPmail"] = "mail";
            ViewState["mailsendresult"] = "no";


            DataView viewgroup = new DataView(ds.Tables[0]);
            DataTable dtgroupcontactid = viewgroup.ToTable(true, new string[] { "GrpId", "GrpEmail", "Grpname" });

            for (int j = 0; j < dtgroupcontactid.Rows.Count; j++)
            {
                DataView viewgrp = new DataView();
                viewgrp = ds.Tables[0].DefaultView;
                viewgrp.RowFilter = "GrpId='" + dtgroupcontactid.Rows[j][0].ToString().Trim() + "'";
                DataTable dt = new DataTable();
                dt = viewgrp.ToTable();

                DataView viewData = new DataView();
                viewData = dt.DefaultView;
                viewData.RowFilter = " ClientId not in ('G','ZZZ')";
                DataTable dt1 = new DataTable();
                dt1 = viewData.ToTable();

                DataTable Distinctclient = new DataTable();
                DataView viewClient = new DataView(dt1);
                Distinctclient = viewClient.ToTable(true, new string[] { "ClientId" });

                if (Distinctclient.Rows.Count > 0)
                {
                    cmb.DataSource = Distinctclient;
                    cmb.DataValueField = "ClientId";
                    cmb.DataTextField = "ClientId";
                    cmb.DataBind();

                }

                for (int k = 0; k < cmb.Items.Count; k++)
                {
                    FnHtmlClientWise(ds, cmb.Items[k].Value.ToString().Trim());
                    if (ViewState["GRPmail"].ToString().Trim() == "mail")
                    {
                        ViewState["GRPmail"] = ViewState["mail"].ToString().Trim();
                    }
                    else
                    {
                        ViewState["GRPmail"] = ViewState["GRPmail"].ToString().Trim() + ViewState["mail"].ToString().Trim();
                    }

                }
                if (oDBEngine.SendReportBr(ViewState["GRPmail"].ToString().Trim(), dtgroupcontactid.Rows[j]["GrpEmail"].ToString().Trim(), oconverter.ArrangeDate2(DtFor.Value.ToString()), "Margin Stocks Inward/Outward Register [" + oconverter.ArrangeDate2(DtFor.Value.ToString().Trim()) + "]", dtgroupcontactid.Rows[j]["GrpId"].ToString().Trim()) == true)
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordStatus", "RecordStatus(6);", true);
            }
            if (ViewState["mailsendresult"].ToString().Trim() == "success")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordStatus", "RecordStatus(4);", true);
            }
            if (ViewState["mailsendresult"].ToString().Trim() == "errorsuccess")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordStatus", "RecordStatus(5);", true);
            }
        }
        void optionforemail(DataSet ds)
        {
            if (HiddenField_emmail.Value.ToString().Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordStatus5", "RecordStatus(7);", true);
            }
            else
            {
                ViewState["GRPmail"] = "mail";
                ViewState["Usermail"] = "UserMail";
                ViewState["mailsendresult"] = "no";

                DataView viewgroup = new DataView(ds.Tables[0]);
                DataTable dtgroupcontactid = viewgroup.ToTable(true, new string[] { "GrpId", "GrpEmail", "Grpname" });

                for (int j = 0; j < dtgroupcontactid.Rows.Count; j++)
                {
                    DataView viewgrp = new DataView();
                    viewgrp = ds.Tables[0].DefaultView;
                    viewgrp.RowFilter = "GrpId='" + dtgroupcontactid.Rows[j][0].ToString().Trim() + "'";
                    DataTable dt = new DataTable();
                    dt = viewgrp.ToTable();

                    DataView viewData = new DataView();
                    viewData = dt.DefaultView;
                    viewData.RowFilter = " ClientId not in ('G','ZZZ')";
                    DataTable dt1 = new DataTable();
                    dt1 = viewData.ToTable();

                    DataTable Distinctclient = new DataTable();
                    DataView viewClient = new DataView(dt1);
                    Distinctclient = viewClient.ToTable(true, new string[] { "ClientId" });

                    if (Distinctclient.Rows.Count > 0)
                    {
                        cmb.DataSource = Distinctclient;
                        cmb.DataValueField = "ClientId";
                        cmb.DataTextField = "ClientId";
                        cmb.DataBind();

                    }

                    for (int k = 0; k < cmb.Items.Count; k++)
                    {
                        FnHtmlClientWise(ds, cmb.Items[k].Value.ToString().Trim());
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
                    if (oDBEngine.SendReportSt(ViewState["Usermail"].ToString().Trim(), clnt[i].ToString().Trim(), oconverter.ArrangeDate2(DtFor.Value.ToString().Trim()), "Margin Stocks Inward/Outward Register [" + oconverter.ArrangeDate2(DtFor.Value.ToString().Trim()) + "]") == true)
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
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordStatus", "RecordStatus(6);", true);
                }
                if (ViewState["mailsendresult"].ToString().Trim() == "success")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordStatus", "RecordStatus(4);", true);
                }
                if (ViewState["mailsendresult"].ToString().Trim() == "errorsuccess")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordStatus", "RecordStatus(5);", true);
                }
            }
        }
        void userwiseemail(DataSet ds)
        {
            if (HiddenField_emmail.Value.ToString().Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordStatus5", "RecordStatus(7);", true);
            }
            else
            {

                ViewState["Usermail"] = "UserMail";
                ViewState["mailsendresult"] = "no";

                DataView viewData = new DataView();
                viewData = ds.Tables[0].DefaultView;
                viewData.RowFilter = " MainOrder='3'";
                DataTable dt = new DataTable();
                dt = viewData.ToTable();

                DataTable DistinctAccountid = new DataTable();
                DataView viewAc = new DataView(dt);
                DistinctAccountid = viewAc.ToTable(true, new string[] { "OwnAcname" });

                if (DistinctAccountid.Rows.Count > 0)
                {
                    cmb.DataSource = DistinctAccountid;
                    cmb.DataValueField = "OwnAcname";
                    cmb.DataTextField = "OwnAcname";
                    cmb.DataBind();

                }

                for (int k = 0; k < cmb.Items.Count; k++)
                {
                    FnHtmlAccountWise(ds, cmb.Items[k].Value.ToString().Trim());
                    if (ViewState["Usermail"].ToString().Trim() == "UserMail")
                    {
                        ViewState["Usermail"] = ViewState["mail"].ToString().Trim();
                    }
                    else
                    {
                        ViewState["Usermail"] = ViewState["Usermail"].ToString().Trim() + ViewState["mail"].ToString().Trim();
                    }

                }
                string[] clnt = HiddenField_emmail.Value.ToString().Split(',');
                int kk = clnt.Length;
                for (int i = 0; i < clnt.Length; i++)
                {
                    if (oDBEngine.SendReportSt(ViewState["Usermail"].ToString().Trim(), clnt[i].ToString().Trim(), oconverter.ArrangeDate2(DtFor.Value.ToString().Trim()), "Margin Stocks Inward/Outward Register [" + oconverter.ArrangeDate2(DtFor.Value.ToString().Trim()) + "]") == true)
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
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordStatus", "RecordStatus(6);", true);
                }
                if (ViewState["mailsendresult"].ToString().Trim() == "success")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordStatus", "RecordStatus(4);", true);
                }
                if (ViewState["mailsendresult"].ToString().Trim() == "errorsuccess")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordStatus", "RecordStatus(5);", true);
                }
            }
        }
        void clientwiseemail(DataSet ds)
        {

            ViewState["mailsendresult"] = "no";


            DataView viewgroup = new DataView(ds.Tables[0]);
            DataTable dtgroupcontactid = viewgroup.ToTable(true, new string[] { "GrpId", "GrpEmail", "Grpname" });

            for (int j = 0; j < dtgroupcontactid.Rows.Count; j++)
            {
                DataView viewgrp = new DataView();
                viewgrp = ds.Tables[0].DefaultView;
                viewgrp.RowFilter = "GrpId='" + dtgroupcontactid.Rows[j][0].ToString().Trim() + "'";
                DataTable dt = new DataTable();
                dt = viewgrp.ToTable();

                DataView viewData = new DataView();
                viewData = dt.DefaultView;
                viewData.RowFilter = " ClientId not in ('G','ZZZ')";
                DataTable dt1 = new DataTable();
                dt1 = viewData.ToTable();

                DataTable Distinctclient = new DataTable();
                DataView viewClient = new DataView(dt1);
                Distinctclient = viewClient.ToTable(true, new string[] { "ClientId" });

                if (Distinctclient.Rows.Count > 0)
                {
                    cmb.DataSource = Distinctclient;
                    cmb.DataValueField = "ClientId";
                    cmb.DataTextField = "ClientId";
                    cmb.DataBind();

                }

                for (int k = 0; k < cmb.Items.Count; k++)
                {
                    FnHtmlClientWise(ds, cmb.Items[k].Value.ToString().Trim());
                    if (oDBEngine.SendReport(ViewState["mail"].ToString().Trim(), cmb.Items[k].Value.ToString().Trim(), oconverter.ArrangeDate2(DtFor.Value.ToString()), "Margin Stocks Inward/Outward Register [" + oconverter.ArrangeDate2(DtFor.Value.ToString().Trim()) + "]") == true)
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordStatus", "RecordStatus(6);", true);
            }
            if (ViewState["mailsendresult"].ToString().Trim() == "success")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordStatus", "RecordStatus(4);", true);
            }
            if (ViewState["mailsendresult"].ToString().Trim() == "errorsuccess")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordStatus", "RecordStatus(5);", true);
            }
        }

        //------------------------------ For DP ---------------------------------------------

        void FnHtmlAccountWiseDP(DataSet ds, string Account)
        {


            DateTime vDatetime = Convert.ToDateTime(DtFor.Value);

            DateTime dtFor = new DateTime(vDatetime.Year, vDatetime.Month, vDatetime.Day, 16, 5, 7, 123);

            DateTime dtNow = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 7, 123);


            //////////For header
            String strHtmlheader = String.Empty;
            string str = null;
            if (ddlGroup.SelectedItem.Value.ToString() == "2")
            {
                str = "[" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "]";
            }
            str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report For " + oconverter.ArrangeDate2(DtFor.Value.ToString());
            str = str + " ; Report View : " + ddlreporttype.SelectedItem.Text.ToString().Trim();


            string vSegment, vBranchGroup, vInstrument = "";
            vBranchGroup = "";

            if (rdbSegmentAll.Checked == true)
            {
                vSegment = "All";

            }

            else
            {
                // vSegment = vSegment = HiddenField_Segment.Value;

                vSegment = litSegmentMain.InnerText;
            }


            if (Convert.ToInt32(ddlGroup.SelectedValue) == 1)
            {
                if (rdBranchClientAll.Checked == true)
                {
                    vBranchGroup = "All";

                }

                else
                {
                    vBranchGroup = "Selective";
                }
            }

            else if (Convert.ToInt32(ddlGroup.SelectedValue) == 2)
            {
                if (rdddlgrouptypeAll.Checked == true)
                {
                    vBranchGroup = "All";

                }

                else
                {
                    vBranchGroup = "Selective";
                }
            }

            if (rdbInstrumentAll.Checked == true)
            {
                vInstrument = "All";
            }

            else
            {
                vInstrument = "Selective";
            }

            str = "Collateral Report - Scrip+Client+DP" + "Account [Segments:" + vSegment + " | DP Accounts :" + ddlDPAc.SelectedItem.Text +
                "| Branch/Group: " + vBranchGroup + " | Instruments: " + vInstrument + "]";


            strHtmlheader = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            // strHtmlheader += "<tr><td align=\"left\" colspan=" + ds.Tables[0].Columns.Count + " style=\"color:Blue;\">" + str + "</td></tr> </table>";
            strHtmlheader += "<tr><td align=\"center\" colspan=" + ds.Tables[0].Columns.Count + " style=\"color:Blue;\">" + str + "</td></tr>";
            strHtmlheader += "<tr><td align=\"left\" colspan=" + ds.Tables[0].Columns.Count / 2 + " style=\"color:Blue;\">" + "Report For :" + String.Format("{0:d-MMM -yyyy}", dtFor) + "</td>" +
              "<td align=\"right\" colspan=" + ds.Tables[0].Columns.Count / 2 + " style=\"color:Blue;\">" + "Report Generation Time :" + String.Format("{0:d-MMM -yyyy HH:mm:ss}", dtNow) + "</td>" +
               "</tr></table>";




            ////////Detail Display
            String strHtml = String.Empty;
            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";






            DataView viewclient = new DataView();
            viewclient = ds.Tables[0].DefaultView;
            viewclient.RowFilter = "OwnAcname='" + Account.ToString().Trim() + "'";
            DataTable dt = new DataTable();
            dt = viewclient.ToTable();

            dt.Columns.Remove("OwnAcname");
            dt.Columns.Remove("Group/Branch");



            strHtml += "<tr style=\"background-color:lavender ;text-align:left\">";
            strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; ><b>" + dt.Rows[0][0].ToString().Trim() + "</b></td>";
            strHtml += "</tr>";

            dt.Rows[0].Delete();

            //////////////TABLE HEADER BIND
            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (dt.Columns[i].ColumnName.ToString().Trim() != "MainOrder")
                {
                    strHtml += "<td align=\"center\" style=\"font-size:smaller;\" nowrap=nowrap;><b>" + dt.Columns[i].ColumnName + "</b></td>";
                }
            }
            strHtml += "</tr>";

            int flag = 0;
            string stcachk = "A";
            foreach (DataRow dr1 in dt.Rows)
            {
                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";

                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (dt.Columns[j].ColumnName.ToString().Trim() != "MainOrder")
                    {
                        if (dt.Rows[flag - 1]["MainOrder"].ToString().Trim() == "2")
                        {
                            if (dr1[j].ToString().Trim().StartsWith("**"))
                            {
                                strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\" colspan=8><b>" + dr1[j] + "</b></td>";
                            }

                        }
                        else
                        {
                            if (dr1[j] != DBNull.Value)
                            {
                                if (dr1[j].ToString().Trim().StartsWith("Account") || dr1[j].ToString().Trim().StartsWith("Total:"))
                                {
                                    if (j == 4)
                                    {
                                        strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\"><b>" + dr1[j] + "</b></td>";

                                    }
                                    else
                                    {
                                        strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\"><b>" + dr1[j] + "</b></td>";
                                    }



                                }
                                else if (dr1[j].ToString().Trim().StartsWith("Test"))
                                {
                                    strHtml += "<td>&nbsp;</td>";
                                }
                                else
                                {
                                    if (IsNumeric(dr1[j].ToString()) == true)
                                    {
                                        strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                                    }
                                    else
                                    {
                                        strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                                    }
                                }
                            }
                            else
                            {
                                strHtml += "<td>&nbsp;</td>";

                            }

                        }



                    }

                }
                strHtml += "</tr>";
            }
            strHtml += "</table>";
            if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "1")
            {
                DivHeader.InnerHtml = strHtmlheader;
                Divdisplay.InnerHtml = strHtml;
            }
            if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "3")
            {
                ViewState["mail"] = strHtml;
            }



        }



        void ExportToExcelAccountWiseDP(DataSet ds)
        {
            DataTable dtExport = ds.Tables[0].Copy();
            dtExport.Columns.Remove("OwnAcname");
            dtExport.Columns.Remove("MainOrder");
            dtExport.Columns.Remove("Group/Branch");

            string str = null;
            if (ddlGroup.SelectedItem.Value.ToString() == "2")
            {
                str = "[" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "]";
            }
            str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report For " + oconverter.ArrangeDate2(DtFor.Value.ToString());
            str = str + " ; Report View : " + ddlreporttype.SelectedItem.Text.ToString().Trim();


            //----------------------------------------------------------------------


            string vSegment, vBranchGroup, vInstrument = "";
            vBranchGroup = "";

            if (rdbSegmentAll.Checked == true)
            {
                vSegment = "All";

            }

            else
            {
                // vSegment = vSegment = HiddenField_Segment.Value;

                vSegment = litSegmentMain.InnerText;
            }


            if (Convert.ToInt32(ddlGroup.SelectedValue) == 1)
            {
                if (rdBranchClientAll.Checked == true)
                {
                    vBranchGroup = "All";

                }

                else
                {
                    vBranchGroup = "Selective";
                }
            }

            else if (Convert.ToInt32(ddlGroup.SelectedValue) == 2)
            {
                if (rdddlgrouptypeAll.Checked == true)
                {
                    vBranchGroup = "All";

                }

                else
                {
                    vBranchGroup = "Selective";
                }
            }

            if (rdbInstrumentAll.Checked == true)
            {
                vInstrument = "All";
            }

            else
            {
                vInstrument = "Selective";
            }

            //str = "Collateral Report - Scrip+Client+DP" + "Account [Segments:" + vSegment + " | DP Accounts :" + ddlDPAc.SelectedItem.Text +
            //    "| Branch/Group: " + vBranchGroup + " | Instruments: " + vInstrument + "]";


            //strHtmlheader = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            //// strHtmlheader += "<tr><td align=\"left\" colspan=" + ds.Tables[0].Columns.Count + " style=\"color:Blue;\">" + str + "</td></tr> </table>";
            //strHtmlheader += "<tr><td align=\"center\" colspan=" + ds.Tables[0].Columns.Count + " style=\"color:Blue;\">" + str + "</td></tr>";
            //strHtmlheader += "<tr><td align=\"left\" colspan=" + ds.Tables[0].Columns.Count / 2 + " style=\"color:Blue;\">" + "Report For :" + DtFor.Value + "</td>" +
            //  "<td align=\"right\" colspan=" + ds.Tables[0].Columns.Count / 2 + " style=\"color:Blue;\">" + "Report Generation Time :" + DateTime.Today + "</td>" +
            //   "</tr></table>";



            //-----------------------------------------------------------------------------------




            DataTable dtReportHeader = new DataTable();
            DataTable dtReportFooter = new DataTable();

            DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            HeaderRow[0] = CompanyName.Rows[0][0].ToString();
            dtReportHeader.Rows.Add(HeaderRow);
            DataRow DrRowR1 = dtReportHeader.NewRow();
            DrRowR1[0] = str.ToString().Trim();
            dtReportHeader.Rows.Add(DrRowR1);


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

            objExcel.ExportToExcelforExcel(dtExport, "Margin Stocks Inward/Outward Register DP", "Total:", dtReportHeader, dtReportFooter);

        }



        void ExportDP(DataSet ds)
        {


            DateTime vDatetime = Convert.ToDateTime(DtFor.Value);

            DateTime dtFor = new DateTime(vDatetime.Year, vDatetime.Month, vDatetime.Day, 16, 5, 7, 123);

            DateTime dtNow = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 7, 123);


            DataTable dtExport = ds.Tables[0].Copy();




            if (dtExport.Columns.Contains("OwnAcname"))
            {
                dtExport.Columns.Remove("OwnAcname");
            }

            if (dtExport.Columns.Contains("Group/Branch"))
            {
                dtExport.Columns.Remove("Group/Branch");
            }


            if (dtExport.Columns.Contains("MainOrder"))
            {
                dtExport.Columns.Remove("MainOrder");
            }

            if (dtExport.Columns.Contains("ScripName"))
            {
                dtExport.Columns.Remove("ScripName");
            }



            string str = null;

            //------------------------------------------------------------------------------


            string vSegment, vBranchGroup, vInstrument = "";
            vBranchGroup = "";

            if (rdbSegmentAll.Checked == true)
            {
                vSegment = "All";

            }

            else
            {
                //  vSegment = HiddenField_Segment.Value;

                vSegment = litSegmentMain.InnerText;
            }


            if (Convert.ToInt32(ddlGroup.SelectedValue) == 1)
            {
                if (rdBranchClientAll.Checked == true)
                {
                    vBranchGroup = "All";

                }

                else
                {
                    vBranchGroup = "Selective";
                }
            }

            else if (Convert.ToInt32(ddlGroup.SelectedValue) == 2)
            {
                if (rdddlgrouptypeAll.Checked == true)
                {
                    vBranchGroup = "All";

                }

                else
                {
                    vBranchGroup = "Selective";
                }
            }

            if (rdbInstrumentAll.Checked == true)
            {
                vInstrument = "All";
            }

            else
            {
                vInstrument = "Selective";
            }

            str = "Collateral Report - Scrip+Client+DP" + "Account [Segments:" + vSegment + " | DP Accounts :" + ddlDPAc.SelectedItem.Text +
                "| Branch/Group: " + vBranchGroup + " | Instruments: " + vInstrument + "]";

            //str += "Report For :" + DtFor.Value;
            //str += "Report Generation Time :" + DateTime.Now.ToString();





            //-----------------------------------------------------------------------------------


            //  str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report Period " + oconverter.ArrangeDate2(DtFor.Value.ToString());//+ " - " + oconverter.ArrangeDate2(dtTo.Value.ToString());



            //--------------------------------------------------------------------------------


            DataTable dtReportHeader = new DataTable();
            DataTable dtReportFooter = new DataTable();

            DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            HeaderRow[0] = CompanyName.Rows[0][0].ToString();
            dtReportHeader.Rows.Add(HeaderRow);
            DataRow DrRowR1 = dtReportHeader.NewRow();

            DrRowR1[0] = str.ToString().Trim();
            dtReportHeader.Rows.Add(DrRowR1);

            //--------------

            DataRow DrRowR3 = dtReportHeader.NewRow();

            DrRowR3[0] = " Report For : " + String.Format("{0:d-MMM -yyyy}", dtFor) + " , " + "  Report Generation Time : " + String.Format("{0:d-MMM -yyyy HH:mm:ss}", dtNow);




            dtReportHeader.Rows.Add(DrRowR3);

            //--------------------

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

            objExcel.ExportToExcelforExcel(dtExport, "Margin Stocks Inward/Outward Register", "Total:", dtReportHeader, dtReportFooter);

        }



        void ExportToAccountWisePDF_DP(DataSet ds)
        {


            //------------- For Search Condition Start--------------------------


            string str = null;


            string vSegment, vBranchGroup, vInstrument = "";
            vBranchGroup = "";

            if (rdbSegmentAll.Checked == true)
            {
                vSegment = "All";

            }

            else
            {
                // vSegment = HiddenField_Segment.Value;

                vSegment = litSegmentMain.InnerText;
            }


            if (Convert.ToInt32(ddlGroup.SelectedValue) == 1)
            {
                if (rdBranchClientAll.Checked == true)
                {
                    vBranchGroup = "All";

                }

                else
                {
                    vBranchGroup = "Selective";
                }
            }

            else if (Convert.ToInt32(ddlGroup.SelectedValue) == 2)
            {
                if (rdddlgrouptypeAll.Checked == true)
                {
                    vBranchGroup = "All";

                }

                else
                {
                    vBranchGroup = "Selective";
                }
            }

            if (rdbInstrumentAll.Checked == true)
            {
                vInstrument = "All";
            }

            else
            {
                vInstrument = "Selective";
            }

            str = "Collateral Report - Scrip+Client+DP" + "Account [Segments:" + vSegment + " | DP Accounts :" + ddlDPAc.SelectedItem.Text +
                "| Branch/Group: " + vBranchGroup + " | Instruments: " + vInstrument + "]";



            //-------------- For Search Condition End---------------------------------


            byte[] logoinByte;
            ds.Tables[1].Columns.Add("Image", System.Type.GetType("System.Byte[]"));
            if (ChkLogoPrint.Checked == false)
            {
                if (oconverter.getLogoImage(HttpContext.Current.Server.MapPath(@"..\images\logo.bmp"), out logoinByte) != 1)
                {
                    ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "logo", "alert('Logo not Found.');", true);
                }
                else
                {
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        ds.Tables[1].Rows[i]["Image"] = logoinByte;

                    }
                }
            }


            //ds.Tables[0].WriteXmlSchema("E:\\RPTXSD\\collateral.xsd");
            //ds.Tables[1].WriteXmlSchema("E:\\RPTXSD\\CollateralCompany.xsd");





            ReportDocument report = new ReportDocument();
            report.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;

            string tmpPdfPath = string.Empty;
            tmpPdfPath = HttpContext.Current.Server.MapPath("..\\Reports\\collateralaccount_DP.rpt");
            report.Load(tmpPdfPath);


            DataTable dtSub = new DataTable();

            dtSub = ds.Tables[1];

            dtSub.Columns.Add("SearchCondition", typeof(System.String));

            foreach (DataRow dr in dtSub.Rows)
            {
                //need to set value to MyRow column
                dr["SearchCondition"] = str;   // or set it to some other value
            }
            dtSub.AcceptChanges();


            report.SetDataSource(ds.Tables[2]);
            report.Subreports["CollateralCompany"].SetDataSource(dtSub);

            // report.SetParameterValue("VCondition", "Hello1");


            report.VerifyDatabase();
            report.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "Margin Stocks Inward/Outward Register");
            report.Dispose();
            GC.Collect();
        }


        void userwiseemail_DP(DataSet ds)
        {
            if (HiddenField_emmail.Value.ToString().Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordStatus5", "RecordStatus(7);", true);
            }
            else
            {

                ViewState["Usermail"] = "UserMail";
                ViewState["mailsendresult"] = "no";

                DataView viewData = new DataView();
                viewData = ds.Tables[0].DefaultView;
                viewData.RowFilter = " MainOrder='3'";
                DataTable dt = new DataTable();
                dt = viewData.ToTable();

                DataTable DistinctAccountid = new DataTable();
                DataView viewAc = new DataView(dt);
                DistinctAccountid = viewAc.ToTable(true, new string[] { "OwnAcname" });

                if (DistinctAccountid.Rows.Count > 0)
                {
                    cmb.DataSource = DistinctAccountid;
                    cmb.DataValueField = "OwnAcname";
                    cmb.DataTextField = "OwnAcname";
                    cmb.DataBind();

                }

                for (int k = 0; k < cmb.Items.Count; k++)
                {
                    // FnHtmlAccountWiseDP(ds, cmb.Items[k].Value.ToString().Trim());

                    FnHtmlClientWiseDP(ds, cmb.Items[k].Value.ToString().Trim());



                    if (ViewState["Usermail"].ToString().Trim() == "UserMail")
                    {
                        ViewState["Usermail"] = ViewState["mail"].ToString().Trim();
                    }
                    else
                    {
                        ViewState["Usermail"] = ViewState["Usermail"].ToString().Trim() + ViewState["mail"].ToString().Trim();
                    }

                }
                string[] clnt = HiddenField_emmail.Value.ToString().Split(',');
                int kk = clnt.Length;
                for (int i = 0; i < clnt.Length; i++)
                {
                    if (oDBEngine.SendReportSt(ViewState["Usermail"].ToString().Trim(), clnt[i].ToString().Trim(), oconverter.ArrangeDate2(DtFor.Value.ToString().Trim()), "Margin Stocks Inward/Outward Register [" + oconverter.ArrangeDate2(DtFor.Value.ToString().Trim()) + "]") == true)
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
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordStatus", "RecordStatus(6);", true);
                }
                if (ViewState["mailsendresult"].ToString().Trim() == "success")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordStatus", "RecordStatus(4);", true);
                }
                if (ViewState["mailsendresult"].ToString().Trim() == "errorsuccess")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordStatus", "RecordStatus(5);", true);
                }
            }
        }



        ///-----------------------------------
        ///

        void FnHtmlClientWiseDP(DataSet ds, string Clientid)
        {

            DateTime vDatetime = Convert.ToDateTime(DtFor.Value);

            DateTime dtFor = new DateTime(vDatetime.Year, vDatetime.Month, vDatetime.Day, 16, 5, 7, 123);

            DateTime dtNow = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 7, 123);


            //////////For header
            String strHtmlheader = String.Empty;
            string str = null;
            if (ddlGroup.SelectedItem.Value.ToString() == "2")
            {
                str = "[" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "]";
            }
            str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report For " + oconverter.ArrangeDate2(DtFor.Value.ToString());
            str = str + " ; Report View : " + ddlreporttype.SelectedItem.Text.ToString().Trim();



            //-----------------------------------------

            string vSegment, vBranchGroup, vInstrument = "";
            vBranchGroup = "";

            if (rdbSegmentAll.Checked == true)
            {
                vSegment = "All";

            }

            else
            {
                //vSegment = vSegment = HiddenField_Segment.Value;

                vSegment = litSegmentMain.InnerText;
            }


            if (Convert.ToInt32(ddlGroup.SelectedValue) == 1)
            {
                if (rdBranchClientAll.Checked == true)
                {
                    vBranchGroup = "All";

                }

                else
                {
                    vBranchGroup = "Selective";
                }
            }

            else if (Convert.ToInt32(ddlGroup.SelectedValue) == 2)
            {
                if (rdddlgrouptypeAll.Checked == true)
                {
                    vBranchGroup = "All";

                }

                else
                {
                    vBranchGroup = "Selective";
                }
            }

            if (rdbInstrumentAll.Checked == true)
            {
                vInstrument = "All";
            }

            else
            {
                vInstrument = "Selective";
            }

            str = "Collateral Report - Scrip+Client+DP" + "Account [Segments:" + vSegment + " | DP Accounts :" + ddlDPAc.SelectedItem.Text +
                "| Branch/Group: " + vBranchGroup + " | Instruments: " + vInstrument + "]";


            strHtmlheader = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            // strHtmlheader += "<tr><td align=\"left\" colspan=" + ds.Tables[0].Columns.Count + " style=\"color:Blue;\">" + str + "</td></tr> </table>";
            strHtmlheader += "<tr><td align=\"center\" colspan=" + ds.Tables[0].Columns.Count + " style=\"color:Blue;\">" + str + "</td></tr>";
            strHtmlheader += "<tr><td align=\"left\" colspan=" + ds.Tables[0].Columns.Count / 2 + " style=\"color:Blue;\">" + "Report For :" + String.Format("{0:d-MMM -yyyy}", dtFor) + "</td>" +
              "<td align=\"right\" colspan=" + ds.Tables[0].Columns.Count / 2 + " style=\"color:Blue;\">" + "Report Generation Time :" + String.Format("{0:d-MMM -yyyy HH:mm:ss}", dtNow) + "</td>" +
               "</tr></table>";



            //-----------------------------------------------







            ////////Detail Display
            String strHtml = String.Empty;
            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";




            DataView viewclient = new DataView();
            // viewclient = ds.Tables[0].de;
            // viewclient.RowFilter = "[Script/Client Name]='" + Clientid.ToString().Trim() + "'";
            DataTable dt = new DataTable();
            // dt = viewclient.ToTable();



            dt = ds.Tables[0];



            if (dt.Columns.Contains("OwnAcname"))
            {
                dt.Columns.Remove("OwnAcname");
            }

            if (dt.Columns.Contains("Group/Branch"))
            {
                dt.Columns.Remove("Group/Branch");
            }


            if (dt.Columns.Contains("MainOrder"))
            {
                dt.Columns.Remove("MainOrder");
            }

            if (dt.Columns.Contains("ScripName"))
            {
                dt.Columns.Remove("ScripName");
            }


            //dt.Columns.Remove("GrpEmail");


            strHtml += "<tr style=\"background-color:lavender ;text-align:left\">";
            // strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; ><b>" + dt.Rows[0][0].ToString().Trim() + "</b></td>";
            strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; ><b>" + "" + "</b></td>";
            strHtml += "</tr>";

            //dt.Rows[0].Delete();

            //////////////TABLE HEADER BIND
            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                strHtml += "<td align=\"center\" style=\"font-size:smaller;\" nowrap=nowrap;><b>" + dt.Columns[i].ColumnName + "</b></td>";
            }
            strHtml += "</tr>";

            int flag = 0;
            foreach (DataRow dr1 in dt.Rows)
            {
                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (dr1[j] != DBNull.Value)
                    {
                        if (dr1[j].ToString().Trim().StartsWith("**") || dr1[j].ToString().Trim().StartsWith("Net:") || dr1[j].ToString().Trim().StartsWith("Total:"))
                        {
                            strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\"><b>" + dr1[j] + "</b></td>";
                        }
                        else if (dr1[j].ToString().Trim().StartsWith("Test"))
                        {
                            strHtml += "<td>&nbsp;</td>";
                        }
                        else
                        {
                            if (IsNumeric(dr1[j].ToString()) == true)
                            {
                                strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                            }
                            else
                            {
                                strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                            }
                        }



                    }
                    else
                    {
                        strHtml += "<td>&nbsp;</td>";
                    }
                }

                strHtml += "</tr>";
            }
            strHtml += "</table>";
            if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "1")
            {
                DivHeader.InnerHtml = strHtmlheader;
                Divdisplay.InnerHtml = strHtml;
            }
            if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "3")
            {
                ViewState["mail"] = strHtml;
            }



        }




        void AccountBind_DP(DataSet ds)
        {
            DataView viewData = new DataView();
            viewData = ds.Tables[0].DefaultView;
            viewData.RowFilter = " MainOrder='3'";
            DataTable dt = new DataTable();
            dt = viewData.ToTable();

            DataTable DistinctAccountid = new DataTable();
            DataView viewAc = new DataView(dt);
            DistinctAccountid = viewAc.ToTable(true, new string[] { "ScripName" });

            if (DistinctAccountid.Rows.Count > 0)
            {
                cmb.DataSource = DistinctAccountid;
                cmb.DataValueField = "ScripName";
                cmb.DataTextField = "ScripName";
                cmb.DataBind();

            }

            LastPage = DistinctAccountid.Rows.Count - 1;
            CurrentPage = 0;
            bind_Details();
        }



    }
}