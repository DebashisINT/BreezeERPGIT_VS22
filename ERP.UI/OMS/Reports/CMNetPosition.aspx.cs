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
    public partial class Reports_CMNetPosition : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        DailyReports dailyrep = new DailyReports();
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        DataSet ds = new DataSet();
        DataTable dtgroupcontactid = new DataTable();
        DataTable Distinctclient = new DataTable();
        ExcelFile objExcel = new ExcelFile();
        DataTable dtExport = new DataTable();
        string data;
        int pageindex = 0;


        //New Varibales
        GenericMethod oGenericMethod = null;
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


            string[,] DtStartEnddate = oDBEngine.GetFieldValue("Master_Settlements", "(cast(Settlements_StartDateTime as varchar)+','+cast(Settlements_FundsPayin as varchar)+','+cast(Settlements_EndDateTime as varchar))", "settlements_Number='" + Session["LastSettNo"].ToString().Substring(0, 7) + "' and settlements_TypeSuffix='" + Session["LastSettNo"].ToString().Substring(7, 1) + "' and Settlements_ExchangeSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "'", 1);
            if (DtStartEnddate[0, 0] != "n")
            {
                string[] idlist = DtStartEnddate[0, 0].ToString().Split(','); // fetch startdate and FundsPayin and End Date from Master_Settlements
                dtfor.Value = Convert.ToDateTime(idlist[0]);

            }
            DateTime firstDayOfCurrentMonth = new DateTime(oDBEngine.GetDate().Year, oDBEngine.GetDate().Month, 1);
            dtfrom.Value = Convert.ToDateTime(firstDayOfCurrentMonth.ToShortDateString());
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
                    if (idlist[0] != "Clients" && idlist[0] != "SettlementType" && idlist[0] != "Broker")
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


                if (idlist[0] == "ScripsExchange")
                {
                    data = "ScripsExchange~" + str;
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
                else if (idlist[0] == "SettlementType")
                {
                    data = "SettlementType~" + str;
                }
                else if (idlist[0] == "MAILEMPLOYEE")
                {
                    data = "MAILEMPLOYEE~" + str;
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

        protected void btnshow_Click(object sender, EventArgs e)
        {
            if (ddlrptview.SelectedItem.Value.ToString() == "0")
            {
                if (ddlGeneration.SelectedItem.Value.ToString() == "3")//////select a type
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD7", "NORECORD(7);", true);
                }
                else if (ddlGeneration.SelectedItem.Value.ToString() == "0")/////select screen
                {
                    if (ddlGroup.SelectedItem.Value.ToString().Trim() == "0")/////if branch
                    {
                        if (rdbranchclientSelected.Checked == true && HiddenField_Branch.Value.ToString().Trim() == "")
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD8", "NORECORD(8);", true);
                        }
                        else
                        {
                            spcall();
                        }

                    }
                    if (ddlGroup.SelectedItem.Value.ToString().Trim() == "1")/////if group
                    {
                        if (rdddlgrouptypeSelected.Checked == true && HiddenField_Group.Value.ToString().Trim() == "")
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD8", "NORECORD(8);", true);
                        }
                        else
                        {
                            spcall();
                        }

                    }
                    if (ddlGroup.SelectedItem.Value.ToString().Trim() == "2")/////if branch-group
                    {
                        if (rdbranchclientSelected.Checked == true && HiddenField_BranchGroup.Value.ToString().Trim() == "")
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD8", "NORECORD(8);", true);
                        }
                        else
                        {
                            spcall();
                        }

                    }
                    if (ddlGroup.SelectedItem.Value.ToString().Trim() == "3")/////if client wise
                    {
                        if (rdbranchclientSelected.Checked == true && HiddenField_Client.Value.ToString().Trim() == "")
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD8", "NORECORD(8);", true);
                        }
                        else
                        {
                            spcall();
                        }

                    }

                    if (ddlGroup.SelectedItem.Value.ToString().Trim() == "4")/////if Broker wise
                    {
                        if (rdbranchclientSelected.Checked == true && HiddenField_Broker.Value.ToString().Trim() == "")
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD8", "NORECORD(8);", true);
                        }
                        else
                        {
                            spcall();
                        }

                    }
                }
            }
            else
            {
                if (rdbScripSelected.Checked == true && HiddenField_ScripsExchange.Value.ToString().Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD8", "NORECORD(8);", true);
                }
                else
                {
                    spcall();
                }
            }


        }
        void spcall()
        {
            procedure();
            ds = (DataSet)ViewState["dataset"];
            if (ds.Tables[0].Rows.Count > 0)
            {

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
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(1);", true);
            }
        }
        void procedure()
        {
            //Sever Debugger Variable
            string[,] strSDParam = new string[19, 2];
            string SpName = String.Empty;
            string Broker = "";
            string ClientsID = "";
            string instrument = "";
            string settype = "";
            string GRPTYPE = "";
            string GRPID = "";
            string openposition = "";
            string ChkCharge = "";
            string Chk_sign = "";

            if (ddldate.SelectedItem.Value.ToString() == "0")
            {
                //SDCode
                strSDParam[0, 0] = "fromdate"; strSDParam[0, 1] = "'" + dtfor.Value + "'";
                strSDParam[1, 0] = "todate"; strSDParam[1, 1] = "'" + dtfor.Value + "'";
                strSDParam[2, 0] = "SettNo"; strSDParam[2, 1] = "'" + Session["LastSettNo"].ToString().Substring(0, 7) + "'";
            }
            else
            {
                //SDCode
                strSDParam[0, 0] = "fromdate"; strSDParam[0, 1] = "'" + dtfor.Value + "'";
                strSDParam[1, 0] = "todate"; strSDParam[1, 1] = "'" + dtfor.Value + "'";
                strSDParam[2, 0] = "SettNo"; strSDParam[2, 1] = "'ALL'";
            }

            //SDCode
            strSDParam[3, 0] = "segment"; strSDParam[3, 1] = "'" + Convert.ToInt32(Session["usersegid"].ToString()) + "'";
            strSDParam[4, 0] = "MasterSegment"; strSDParam[4, 1] = "'" + HttpContext.Current.Session["ExchangeSegmentID"].ToString() + "'";
            strSDParam[5, 0] = "Companyid"; strSDParam[5, 1] = "'" + Session["LastCompany"].ToString() + "'";

            if (ddlGroup.SelectedItem.Value.ToString() == "3")/////option client wise
            {
                Broker = "NA";
                strSDParam[6, 0] = "@Broker"; strSDParam[6, 1] = "'NA'";
                if (rdbranchclientAll.Checked)
                {
                    ClientsID = "ALL";
                    strSDParam[7, 0] = "ClientsID"; strSDParam[7, 1] = "'ALL'";
                }
                else
                {
                    if (ddlrptview.SelectedItem.Value.ToString() == "1")
                    {
                        ClientsID = "ALL";
                        strSDParam[7, 0] = "ClientsID"; strSDParam[7, 1] = "'ALL'";
                    }
                    else
                    {
                        ClientsID = HiddenField_Client.Value;
                        strSDParam[7, 0] = "ClientsID"; strSDParam[7, 1] = HiddenField_Client.Value;
                    }

                }
            }
            else if (ddlGroup.SelectedItem.Value.ToString() == "4")/////option client wise
            {
                Broker = "BO";
                strSDParam[6, 0] = "Broker"; strSDParam[6, 1] = "'BO'";
                if (rdbranchclientAll.Checked)
                {
                    ClientsID = "ALL";
                    strSDParam[7, 0] = "ClientsID"; strSDParam[7, 1] = "'ALL'";
                }
                else
                {
                    if (ddlrptview.SelectedItem.Value.ToString() == "1")
                    {
                        ClientsID = "ALL";
                        strSDParam[7, 0] = "ClientsID"; strSDParam[7, 1] = "'ALL'";
                    }
                    else
                    {
                        ClientsID = HiddenField_Broker.Value;
                        strSDParam[7, 0] = "ClientsID"; strSDParam[7, 1] = HiddenField_Client.Value;
                    }

                }
            }
            else
            {
                Broker = "NA";
                strSDParam[6, 0] = "Broker"; strSDParam[6, 1] = "'NA'";
                ClientsID = "ALL";
                strSDParam[7, 0] = "ClientsID"; strSDParam[7, 1] = "'ALL'";
            }
            if (rdbScripAll.Checked)
            {
                instrument = "ALL";
                strSDParam[8, 0] = "instrument"; strSDParam[8, 1] = "'ALL'";
            }
            else
            {
                instrument = HiddenField_ScripsExchange.Value;
                strSDParam[8, 0] = "instrument"; strSDParam[8, 1] = HiddenField_ScripsExchange.Value;
            }
            if (rdSetttypeAll.Checked)
            {
                settype = "ALL";
                strSDParam[9, 0] = "settype"; strSDParam[9, 1] = "'ALL'";
            }
            else
            {
                settype = HiddenField_SettlementType.Value;
                strSDParam[9, 0] = "settype"; strSDParam[9, 1] = "'" + HiddenField_SettlementType.Value + "'";
            }
            strSDParam[10, 0] = "Branch"; strSDParam[10, 1] = "'" + Session["userbranchHierarchy"].ToString() + "'";
            if (ddlGroup.SelectedItem.Value.ToString() == "0")/////option branch 
            {
                GRPTYPE = "BRANCH";
                strSDParam[11, 0] = "GRPTYPE"; strSDParam[11, 1] = "'BRANCH'";
                if (rdbranchclientAll.Checked)
                {
                    GRPID = "ALL";
                    strSDParam[12, 0] = "GRPID"; strSDParam[12, 1] = "'ALL'";
                }
                else
                {
                    GRPID = HiddenField_Branch.Value;
                    strSDParam[12, 0] = "GRPID"; strSDParam[12, 1] = "'" + HiddenField_Branch.Value + "'";
                }
            }
            else if (ddlGroup.SelectedItem.Value.ToString() == "1")/////option group
            {
                GRPTYPE = ddlgrouptype.SelectedItem.Text.ToString().Trim();
                strSDParam[11, 0] = "GRPTYPE"; strSDParam[11, 1] = "'" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "'";
                if (rdddlgrouptypeAll.Checked)
                {
                    GRPID = "ALL";
                    strSDParam[12, 0] = "GRPID"; strSDParam[12, 1] = "'ALL'";
                }
                else
                {
                    GRPID = HiddenField_Group.Value;
                    strSDParam[12, 0] = "GRPID"; strSDParam[12, 1] = "'" + HiddenField_Group.Value + "'";
                }
            }
            else if (ddlGroup.SelectedItem.Value.ToString() == "2")/////option branch group
            {
                GRPTYPE = "BRANCHGROUP";
                strSDParam[11, 0] = "GRPTYPE"; strSDParam[11, 1] = "'BRANCHGROUP'";
                if (rdbranchclientAll.Checked)
                {
                    GRPID = "ALL";
                    strSDParam[12, 0] = "GRPID"; strSDParam[12, 1] = "'ALL'";
                }
                else
                {
                    GRPID = HiddenField_BranchGroup.Value;
                    strSDParam[12, 0] = "GRPID"; strSDParam[12, 1] = "'" + HiddenField_BranchGroup.Value + "'";
                }
            }
            else
            {
                GRPTYPE = "BRANCH";
                GRPID = "ALL";
                strSDParam[11, 0] = "GRPTYPE"; strSDParam[11, 1] = "'BRANCH'";
                strSDParam[12, 0] = "GRPID"; strSDParam[12, 1] = "'ALL'";

            }

            if (chkopen.Checked)
            {
                openposition = "CHK";
                strSDParam[13, 0] = "openposition"; strSDParam[13, 1] = "'CHK'";
            }
            else
            {
                openposition = "UNCHK";
                strSDParam[13, 0] = "openposition"; strSDParam[13, 1] = "'UNCHK'";
            }

            if (ChkCalculateCharge.Checked)
            {
                ChkCharge = "CHK";
                strSDParam[14, 0] = "ChkCharge"; strSDParam[14, 1] = "'CHK'";
            }
            else
            {
                ChkCharge = "UNCHK";
                strSDParam[14, 0] = "ChkCharge"; strSDParam[14, 1] = "'UNCHK'";
            }
            if (Chksign.Checked)
            {
                Chk_sign = "CHK";
                strSDParam[15, 0] = "Chksign"; strSDParam[15, 1] = "'CHK'";
            }
            else
            {
                Chk_sign = "UNCHK";
                strSDParam[15, 0] = "Chksign"; strSDParam[15, 1] = "'UNCHK'";
            }

            strSDParam[16, 0] = "rptview"; strSDParam[16, 1] = "'" + ddlrptview.SelectedItem.Value.ToString().Trim() + "'";
            strSDParam[17, 0] = "AmntGreaterThan"; strSDParam[17, 1] = "'" + txtAmntGreaterThan.Value + "'";
            strSDParam[18, 0] = "SecurityType"; strSDParam[18, 1] = "'" + DdlSecurityType.SelectedItem.Value.ToString().Trim() + "'";

            //For Server Debugging Purpose
            oGenericMethod = new GenericMethod();
            if (oGenericMethod.CallGeneric_ScalerFunction_Int("GetGlobalSettingsValue", Session["UserSegID"].ToString() + "~") == 1)
            {
                string strDateTime = oGenericMethod.GetDate().ToString("yyyyMMddHHmmss");
                string FilePath = "../ExportFiles/ServerDebugging/NetPositionCM" + strDateTime + ".txt";
                oGenericMethod.WriteFile(oGenericMethod.OldSpExecuteWriter(strSDParam, "NetPositionCM"), FilePath, false);
            }

            ds = dailyrep.NetPositionCM(dtfor.Value.ToString(), dtfor.Value.ToString(), Session["LastSettNo"].ToString().Substring(0, 7),
                Session["usersegid"].ToString(), HttpContext.Current.Session["ExchangeSegmentID"].ToString(), Session["LastCompany"].ToString(),
                Broker, ClientsID, instrument, settype, Session["userbranchHierarchy"].ToString(), GRPTYPE, GRPID, openposition, ChkCharge, Chk_sign, ddlrptview.SelectedItem.Value.ToString().Trim(),
                txtAmntGreaterThan.Value.ToString(), DdlSecurityType.SelectedItem.Value.ToString().Trim());
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
            dtgroupcontactid = viewgroup.ToTable(true, new string[] { "PRODUCTID", "TickerSymbol", "SecurityType" });
            cmbgroup.Items.Clear();
            foreach (DataRow row in dtgroupcontactid.Rows)
            {
                ListItem item = new ListItem(row["TickerSymbol"].ToString(), row["PRODUCTID"].ToString());
                if (row["SecurityType"].ToString().Trim() == "Approved")
                {
                    item.Attributes.Add("style", "color:Green;");
                }
                if (row["SecurityType"].ToString().Trim() == "UnApproved")
                {
                    item.Attributes.Add("style", "color:Red;");
                }
                else if (row["SecurityType"].ToString().Trim() == "Illiquid")
                {
                    item.Attributes.Add("style", "color:Red;");
                }
                cmbgroup.Items.Add(item);
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
        void selectionchkboxlist()
        {
            ViewState["Buy Qty"] = "unchk";
            ViewState["Buy MktValue"] = "unchk";
            ViewState["BuyAvg Mkt"] = "unchk";
            ViewState["Buy NetValue"] = "unchk";
            ViewState["BuyAvg Net"] = "unchk";

            ViewState["Sell Qty"] = "unchk";
            ViewState["Sell MktValue"] = "unchk";
            ViewState["SellAvg Mkt"] = "unchk";
            ViewState["Sell NetValue"] = "unchk";
            ViewState["SellAvg Net"] = "unchk";

            ViewState["Diff Qty"] = "unchk";
            ViewState["Diff P/L"] = "unchk";
            ViewState["Dlv Qty"] = "unchk";
            ViewState["Dlv Value"] = "unchk";
            ViewState["Avg Dlv"] = "unchk";

            ViewState["Net Amnt"] = "unchk";
            ViewState["STT"] = "unchk";
            ViewState["Close Price"] = "unchk";
            ViewState["MTM"] = "unchk";

            int colspan = 0;
            if (ddlrptview.SelectedItem.Value.ToString() == "0")
            {
                colspan = 2;
            }
            else
            {
                colspan = 2;
            }
            foreach (ListItem listitem in chktfilter.Items)
            {
                if (listitem.Selected)
                {
                    if (listitem.Value == "Buy Qty")
                    {
                        ViewState["Buy Qty"] = "chk";
                        colspan = colspan + 1;
                    }
                    if (listitem.Value == "Buy MktValue")
                    {
                        ViewState["Buy MktValue"] = "chk";
                        colspan = colspan + 1;
                    }
                    if (listitem.Value == "BuyAvg Mkt")
                    {
                        ViewState["BuyAvg Mkt"] = "chk";
                        colspan = colspan + 1;
                    }
                    if (listitem.Value == "Buy NetValue")
                    {
                        ViewState["Buy NetValue"] = "chk";
                        colspan = colspan + 1;
                    }
                    if (listitem.Value == "BuyAvg Net")
                    {
                        ViewState["BuyAvg Net"] = "chk";
                        colspan = colspan + 1;
                    }
                    if (listitem.Value == "Sell Qty")
                    {
                        ViewState["Sell Qty"] = "chk";
                        colspan = colspan + 1;
                    }
                    if (listitem.Value == "Sell MktValue")
                    {
                        ViewState["Sell MktValue"] = "chk";
                        colspan = colspan + 1;
                    }
                    if (listitem.Value == "SellAvg Mkt")
                    {
                        ViewState["SellAvg Mkt"] = "chk";
                        colspan = colspan + 1;
                    }
                    if (listitem.Value == "Sell NetValue")
                    {
                        ViewState["Sell NetValue"] = "chk";
                        colspan = colspan + 1;
                    }
                    if (listitem.Value == "SellAvg Net")
                    {
                        ViewState["SellAvg Net"] = "chk";
                        colspan = colspan + 1;
                    }
                    if (listitem.Value == "Diff Qty")
                    {
                        ViewState["Diff Qty"] = "chk";
                        colspan = colspan + 1;
                    }
                    if (listitem.Value == "Diff P/L")
                    {
                        ViewState["Diff P/L"] = "chk";
                        colspan = colspan + 1;
                    }
                    if (listitem.Value == "Dlv Qty")
                    {
                        ViewState["Dlv Qty"] = "chk";
                        colspan = colspan + 1;
                    }
                    if (listitem.Value == "Dlv Value")
                    {
                        ViewState["Dlv Value"] = "chk";
                        colspan = colspan + 1;
                    }
                    if (listitem.Value == "Avg Dlv")
                    {
                        ViewState["Avg Dlv"] = "chk";
                        colspan = colspan + 1;
                    }
                    if (listitem.Value == "Net Amnt")
                    {
                        ViewState["Net Amnt"] = "chk";
                        colspan = colspan + 1;
                    }
                    if (listitem.Value == "STT")
                    {
                        ViewState["STT"] = "chk";
                        colspan = colspan + 1;
                    }
                    if (listitem.Value == "Close Price")
                    {
                        ViewState["Close Price"] = "chk";
                        colspan = colspan + 1;
                    }
                    if (listitem.Value == "MTM")
                    {
                        ViewState["MTM"] = "chk";
                        colspan = colspan + 1;
                    }
                }
            }
            ViewState["colcount"] = colspan.ToString().Trim();
        }
        void htmltable_ClientWise(string clientid, string clientname, string grpid, string grpname)
        {
            selectionchkboxlist();
            ds = (DataSet)ViewState["dataset"];
            String strHtml = String.Empty;
            String strHtml1 = String.Empty;
            int flag = 0;
            string str = null;
            /////////********FOR HEADER BEGIN
            if (ddlGeneration.SelectedItem.Value.ToString().Trim() != "1")
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
            else/////////for mail
            {
                str = str + " [ <b>" + grpname.ToString().Trim() + "</b> ]";
            }
            strHtml1 = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtml1 += "<tr><td align=\"left\" colspan=" + ViewState["colcount"].ToString().Trim() + " style=\"color:Blue;\">" + str + "</td></tr></table>";
            /////////********FOR HEADER END


            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

            DataView viewclient = new DataView();
            viewclient = ds.Tables[0].DefaultView;
            viewclient.RowFilter = "CUSTOMERID='" + clientid.ToString().Trim() + "'";
            DataTable dt1 = new DataTable();
            dt1 = viewclient.ToTable();

            strHtml += "<tr style=\"background-color:lavender ;text-align:left\">";
            strHtml += "<td align=\"left\" colspan=" + ViewState["colcount"].ToString().Trim() + ">Client Name :&nbsp;<b>" + clientname.ToString().Trim() + "</b>[ <b>" + dt1.Rows[0]["UCC"].ToString() + " </b> ]</td></tr>";

            /////////HTML TABLE HEADER
            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Scrip</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Isin</b></td>";
            if (ViewState["Buy Qty"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Buy</br>Qty </b></td>";
            }
            if (ViewState["Buy MktValue"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Buy</br>Mkt Value</b></td>";
            }
            if (ViewState["BuyAvg Mkt"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Avg.</br>Mkt</b></td>";
            }
            if (ViewState["Buy NetValue"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Net</br>Value</b></td>";
            }
            if (ViewState["BuyAvg Net"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Avg.</br>Net</b></td>";
            }
            if (ViewState["Sell Qty"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Sell</br>Qty </b></td>";
            }
            if (ViewState["Sell MktValue"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Sell</br>Mkt Value</b></td>";
            }
            if (ViewState["SellAvg Mkt"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Avg.</br>Mkt</b></td>";
            }
            if (ViewState["Sell NetValue"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Net</br>Value</b></td>";
            }
            if (ViewState["SellAvg Net"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Avg.</br>Net</b></td>";
            }
            if (ViewState["Diff Qty"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Diff</br>Qty</b></td>";
            }
            if (ViewState["Diff P/L"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Diff</br>P/L</b></td>";
            }
            if (ViewState["Dlv Qty"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Dlv</br>Qty</b></td>";
            }
            if (ViewState["Dlv Value"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Dlv</br>Value</b></td>";
            }
            if (ViewState["Avg Dlv"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Avg</br>Dlv</b></td>";
            }
            if (ViewState["Net Amnt"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Net</br>Amount</b></td>";
            }
            if (ViewState["STT"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>ST</br>Tax</b></td>";
            }
            if (ViewState["Close Price"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Close</br>Price</b></td>";
            }
            if (ViewState["MTM"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>MTM</b></td>";
            }
            strHtml += "</tr>";



            for (int k = 0; k < dt1.Rows.Count; k++)
            {
                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                strHtml += "<td  align=left style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[k]["SYMBOL"].ToString().Trim() + "</td>";
                strHtml += "<td  align=left style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[k]["Isin"].ToString().Trim() + "</td>";

                if (ViewState["Buy Qty"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["BUYQTY"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[k]["BUYQTY"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["Buy MktValue"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["BUYMKTVALUE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt1.Rows[k]["BUYMKTVALUE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["BuyAvg Mkt"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["BUYMKTAVG"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[k]["BUYMKTAVG"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["Buy NetValue"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["BUYNETVALUE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[k]["BUYNETVALUE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["BuyAvg Net"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["BUYNETAVG"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[k]["BUYNETAVG"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["Sell Qty"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["SELLQTY"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[k]["SELLQTY"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["Sell MktValue"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["SELLMKTVALUE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[k]["SELLMKTVALUE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["SellAvg Mkt"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["SELLMKTAVG"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[k]["SELLMKTAVG"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["Sell NetValue"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["SELLNETVALUE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[k]["SELLNETVALUE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["SellAvg Net"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["SELLNETAVG"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[k]["SELLNETAVG"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["Diff Qty"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["DIFFQTY"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[k]["DIFFQTY"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["Diff P/L"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["DIFFPL"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[k]["DIFFPL"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["Dlv Qty"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["DLVQTY"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[k]["DLVQTY"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["Dlv Value"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["DLVVALUE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[k]["DLVVALUE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }

                if (ViewState["Avg Dlv"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["AVGDLV"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[k]["AVGDLV"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["Net Amnt"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["NETVALUE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[k]["NETVALUE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["STT"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["STTAX"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[k]["STTAX"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["Close Price"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["CLOSEPRICE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[k]["CLOSEPRICE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["MTM"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["MTM"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[k]["MTM"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                strHtml += "</tr>";
            }

            //////////////TOTAL DISPLAY
            flag = flag + 1;
            strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
            strHtml += "<td  align=left style=\"font-size:xx-small;\" nowrap=\"nowrap;\" colspan=2><b>Total :</b></td>";
            if (ViewState["Buy Qty"].ToString() == "chk")
            {
                strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["Buy MktValue"].ToString() == "chk")
            {
                strHtml += "<td align=\"right\" >&nbsp;</td>";
            }

            if (ViewState["BuyAvg Mkt"].ToString() == "chk")
            {
                strHtml += "<td align=\"right\" >&nbsp;</td>";
            }

            if (ViewState["Buy NetValue"].ToString() == "chk")
            {
                if (dt1.Rows[0]["BUYNETVALUE_SUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[0]["BUYNETVALUE_SUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["BuyAvg Net"].ToString() == "chk")
            {
                strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["Sell Qty"].ToString() == "chk")
            {
                strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["Sell MktValue"].ToString() == "chk")
            {
                strHtml += "<td align=\"right\" >&nbsp;</td>";
            }

            if (ViewState["SellAvg Mkt"].ToString() == "chk")
            {
                strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["Sell NetValue"].ToString() == "chk")
            {
                if (dt1.Rows[0]["SELLNETVALUE_SUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[0]["SELLNETVALUE_SUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["SellAvg Net"].ToString() == "chk")
            {
                strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["Diff Qty"].ToString() == "chk")
            {
                strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["Diff P/L"].ToString() == "chk")
            {
                if (dt1.Rows[0]["DIFFPL_SUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[0]["DIFFPL_SUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["Dlv Qty"].ToString() == "chk")
            {
                strHtml += "<td align=\"right\" colspan=1>&nbsp;</td>";
            }
            if (ViewState["Dlv Value"].ToString() == "chk")
            {
                if (dt1.Rows[0]["DLVVALUE_SUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[0]["DLVVALUE_SUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["Avg Dlv"].ToString() == "chk")
            {
                strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["Net Amnt"].ToString() == "chk")
            {
                if (dt1.Rows[0]["NETAMNT_SUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[0]["NETAMNT_SUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["STT"].ToString() == "chk")
            {
                if (dt1.Rows[0]["STT"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[0]["STT"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["Close Price"].ToString() == "chk")
            {
                strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["MTM"].ToString() == "chk")
            {
                if (dt1.Rows[0]["MTM_SUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[0]["MTM_SUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            strHtml += "</tr>";
            strHtml += "</table>";

            /////Charges
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
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >" + dt1.Rows[0]["BRKGCHARGE_NAME"].ToString() + "</td>";
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[0]["BRKGCHARGE_SUM"].ToString() + "</td>";
                    strHtml += "</tr>";
                }
                ///////////Min Processesing Charges
                if (dt1.Rows[0]["DIFFBRKG_NAME"] != DBNull.Value)
                {
                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >" + dt1.Rows[0]["DIFFBRKG_NAME"].ToString() + "</td>";
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[0]["DIFFBRKG"].ToString() + "</td>";
                    strHtml += "</tr>";
                }
                ///////////STax On Min Processesing Charges
                if (dt1.Rows[0]["SRVDIFFBRKG_NAME"] != DBNull.Value)
                {
                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\">" + dt1.Rows[0]["SRVDIFFBRKG_NAME"].ToString() + "</td>";
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[0]["SRVDIFFBRKG"].ToString() + "</td>";
                    strHtml += "</tr>";
                }
                ///////////Transaction Charges
                if (dt1.Rows[0]["TRANCHARGE_NAME"] != DBNull.Value)
                {
                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >" + dt1.Rows[0]["TRANCHARGE_NAME"].ToString() + "</td>";
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
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >" + dt1.Rows[0]["STAMPCHARGE_NAME"].ToString() + "</td>";
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[0]["STAMPCHARGE"].ToString() + "</td>";
                    strHtml += "</tr>";
                }
                ///////////STT Tax
                if (dt1.Rows[0]["STTAX_NAME"] != DBNull.Value)
                {
                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >" + dt1.Rows[0]["STTAX_NAME"].ToString() + "</td>";
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[0]["STTAX_SUM"].ToString() + "</td>";
                    strHtml += "</tr>";
                }
                ///////////SEBI Fee
                if (dt1.Rows[0]["SEBICHARGE_NAME"] != DBNull.Value)
                {
                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >" + dt1.Rows[0]["SEBICHARGE_NAME"].ToString() + "</td>";
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[0]["SEBICHARGE"].ToString() + "</td>";
                    strHtml += "</tr>";
                }
                ///////////DELIVERY CHARGES
                if (dt1.Rows[0]["DELIVERYCHARGE_NAME"] != DBNull.Value)
                {
                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >" + dt1.Rows[0]["DELIVERYCHARGE_NAME"].ToString() + "</td>";
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[0]["DELIVERYCHARGE"].ToString() + "</td>";
                    strHtml += "</tr>";
                }
                ///////////STTAX ON DELIVERY CHARGES
                if (dt1.Rows[0]["SRVTAXDELIVERYCHARGE_NAME"] != DBNull.Value)
                {
                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >" + dt1.Rows[0]["SRVTAXDELIVERYCHARGE_NAME"].ToString() + "</td>";
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[0]["SRVTAXDELIVERYCHARGE"].ToString() + "</td>";
                    strHtml += "</tr>";
                }
                ///////////Round Off Adjustment
                if (dt1.Rows[0]["NETROUNDOFF_NAME"] != DBNull.Value)
                {
                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >" + dt1.Rows[0]["NETROUNDOFF_NAME"].ToString() + "</td>";
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[0]["NETROUNDOFF"].ToString() + "</td>";
                    strHtml += "</tr>";
                }

                ///////////Net Total
                if (dt1.Rows[0]["NETOBLIGATIONCHARGE_NAME"] != DBNull.Value)
                {
                    flag = flag + 1;
                    strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" ><b>" + dt1.Rows[0]["NETOBLIGATIONCHARGE_NAME"].ToString() + "</b></td>";
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>" + dt1.Rows[0]["NETOBLIGATIONCHARGE"].ToString() + "</b></td>";
                    strHtml += "</tr>";
                }
                strHtml += "</table>";
            }

            if (ddlGeneration.SelectedItem.Value.ToString().Trim() != "1")/////////for mail
            {
                int width = 990;
                //display.Attributes.Add("style", "width: " + hidScreenwd.Value + "px; overflow:scroll");
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
            selectionchkboxlist();
            ds = (DataSet)ViewState["dataset"];
            String strHtml = String.Empty;
            String strHtml1 = String.Empty;
            int flag = 0;
            string str = null;
            /////////********FOR HEADER BEGIN
            if (ddlGeneration.SelectedItem.Value.ToString().Trim() != "1")
            {
                if (ddlrptview.SelectedItem.Value.ToString() == "1")
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
            else
            {
                if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "1")/////////for mail
                {
                    str = str + " [ <b>" + productname.ToString().Trim() + "</b> ]";
                }

            }
            strHtml1 = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtml1 += "<tr><td align=\"left\" colspan=" + ViewState["colcount"].ToString().Trim() + "  style=\"color:Blue;\">" + str + "</td></tr></table>";
            /////////********FOR HEADER END


            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

            DataView viewproduct = new DataView();
            viewproduct = ds.Tables[0].DefaultView;
            viewproduct.RowFilter = "PRODUCTID='" + productid.ToString().Trim() + "'";
            DataTable dt1 = new DataTable();
            dt1 = viewproduct.ToTable();


            /////////HTML TABLE HEADER
            /////////HTML TABLE HEADER
            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Client Name</b></td>";
            strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>UCC</b></td>";
            if (ViewState["Buy Qty"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Buy</br>Qty </b></td>";
            }
            if (ViewState["Buy MktValue"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Buy</br>Mkt Value</b></td>";
            }
            if (ViewState["BuyAvg Mkt"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Avg.</br>Mkt</b></td>";
            }
            if (ViewState["Buy NetValue"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Net</br>Value</b></td>";
            }
            if (ViewState["BuyAvg Net"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Avg.</br>Net</b></td>";
            }
            if (ViewState["Sell Qty"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Sell</br>Qty </b></td>";
            }
            if (ViewState["Sell MktValue"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Sell</br>Mkt Value</b></td>";
            }
            if (ViewState["SellAvg Mkt"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Avg.</br>Mkt</b></td>";
            }
            if (ViewState["Sell NetValue"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Net</br>Value</b></td>";
            }
            if (ViewState["SellAvg Net"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Avg.</br>Net</b></td>";
            }
            if (ViewState["Diff Qty"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Diff</br>Qty</b></td>";
            }
            if (ViewState["Diff P/L"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Diff</br>P/L</b></td>";
            }
            if (ViewState["Dlv Qty"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Dlv</br>Qty</b></td>";
            }
            if (ViewState["Dlv Value"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Dlv</br>Value</b></td>";
            }
            if (ViewState["Avg Dlv"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Avg</br>Dlv</b></td>";
            }
            if (ViewState["Net Amnt"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Net</br>Amount</b></td>";
            }
            if (ViewState["STT"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>ST</br>Tax</b></td>";
            }
            if (ViewState["Close Price"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>Close</br>Price</b></td>";
            }
            if (ViewState["MTM"].ToString() == "chk")
            {
                strHtml += "<td align=\"center\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\"><b>MTM</b></td>";
            }
            strHtml += "</tr>";




            for (int k = 0; k < dt1.Rows.Count; k++)
            {
                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                strHtml += "<td  align=left style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[k]["CLIENTNAME"].ToString().Trim() + "</td>";
                strHtml += "<td  align=left style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[k]["UCC"].ToString().Trim() + "</td>";
                if (ViewState["Buy Qty"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["BUYQTY"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[k]["BUYQTY"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["Buy MktValue"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["BUYMKTVALUE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" >" + dt1.Rows[k]["BUYMKTVALUE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["BuyAvg Mkt"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["BUYMKTAVG"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[k]["BUYMKTAVG"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["Buy NetValue"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["BUYNETVALUE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[k]["BUYNETVALUE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["BuyAvg Net"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["BUYNETAVG"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[k]["BUYNETAVG"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["Sell Qty"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["SELLQTY"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[k]["SELLQTY"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["Sell MktValue"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["SELLMKTVALUE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[k]["SELLMKTVALUE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["SellAvg Mkt"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["SELLMKTAVG"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[k]["SELLMKTAVG"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["Sell NetValue"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["SELLNETVALUE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[k]["SELLNETVALUE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["SellAvg Net"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["SELLNETAVG"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[k]["SELLNETAVG"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["Diff Qty"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["DIFFQTY"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[k]["DIFFQTY"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["Diff P/L"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["DIFFPL"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[k]["DIFFPL"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["Dlv Qty"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["DLVQTY"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[k]["DLVQTY"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["Dlv Value"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["DLVVALUE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[k]["DLVVALUE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }

                if (ViewState["Avg Dlv"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["AVGDLV"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[k]["AVGDLV"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["Net Amnt"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["NETVALUE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[k]["NETVALUE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["STT"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["STTAX"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[k]["STTAX"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["Close Price"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["CLOSEPRICE"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[k]["CLOSEPRICE"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                if (ViewState["MTM"].ToString() == "chk")
                {
                    if (dt1.Rows[k]["MTM"] != DBNull.Value)
                        strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[k]["MTM"].ToString() + "</td>";
                    else
                        strHtml += "<td align=\"right\" >&nbsp;</td>";
                }
                strHtml += "</tr>";
            }

            //////////////TOTAL DISPLAY
            flag = flag + 1;
            strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
            strHtml += "<td  align=left style=\"font-size:xx-small;\" nowrap=\"nowrap;\" colspan=2><b>Total :</b></td>";
            if (ViewState["Buy Qty"].ToString() == "chk")
            {
                strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["Buy MktValue"].ToString() == "chk")
            {
                strHtml += "<td align=\"right\" >&nbsp;</td>";
            }

            if (ViewState["BuyAvg Mkt"].ToString() == "chk")
            {
                strHtml += "<td align=\"right\" >&nbsp;</td>";
            }

            if (ViewState["Buy NetValue"].ToString() == "chk")
            {
                if (dt1.Rows[0]["BUYNETVALUE_SUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[0]["BUYNETVALUE_SUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["BuyAvg Net"].ToString() == "chk")
            {
                strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["Sell Qty"].ToString() == "chk")
            {
                strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["Sell MktValue"].ToString() == "chk")
            {
                strHtml += "<td align=\"right\" >&nbsp;</td>";
            }

            if (ViewState["SellAvg Mkt"].ToString() == "chk")
            {
                strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["Sell NetValue"].ToString() == "chk")
            {
                if (dt1.Rows[0]["SELLNETVALUE_SUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[0]["SELLNETVALUE_SUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["SellAvg Net"].ToString() == "chk")
            {
                strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["Diff Qty"].ToString() == "chk")
            {
                strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["Diff P/L"].ToString() == "chk")
            {
                if (dt1.Rows[0]["DIFFPL_SUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[0]["DIFFPL_SUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["Dlv Qty"].ToString() == "chk")
            {
                strHtml += "<td align=\"right\" colspan=1>&nbsp;</td>";
            }
            if (ViewState["Dlv Value"].ToString() == "chk")
            {
                if (dt1.Rows[0]["DLVVALUE_SUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\">" + dt1.Rows[0]["DLVVALUE_SUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["Avg Dlv"].ToString() == "chk")
            {
                strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["Net Amnt"].ToString() == "chk")
            {
                if (dt1.Rows[0]["NETAMNT_SUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[0]["NETAMNT_SUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["STT"].ToString() == "chk")
            {
                if (dt1.Rows[0]["STT"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[0]["STT"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["Close Price"].ToString() == "chk")
            {
                strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            if (ViewState["MTM"].ToString() == "chk")
            {
                if (dt1.Rows[0]["MTM_SUM"] != DBNull.Value)
                    strHtml += "<td align=\"right\" style=\"font-size:xx-small;\" nowrap=\"nowrap;\" >" + dt1.Rows[0]["MTM_SUM"].ToString() + "</td>";
                else
                    strHtml += "<td align=\"right\" >&nbsp;</td>";
            }
            strHtml += "</tr>";


            strHtml += "</table>";

            if (ddlGeneration.SelectedItem.Value.ToString().Trim() != "1")/////////for mail
            {
                display.Attributes.Add("style", "width: " + hidScreenwd.Value + "px; overflow:scroll");
                DIVdisplayPERIOD.InnerHtml = strHtml1;
                display.InnerHtml = strHtml;

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Display", "Display(2);", true);
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
                selectionchkboxlist();
                if (ddlrptview.SelectedItem.Value.ToString().Trim() == "0")
                {
                    ddlbandforgroup();
                    export_clientwise();
                }
                if (ddlrptview.SelectedItem.Value.ToString().Trim() == "1")
                {

                    export_sharewise();
                }
                export();

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
            dtExport.Columns.Add("Isin", Type.GetType("System.String"));
            if (ViewState["Buy Qty"].ToString() == "chk")
            {
                dtExport.Columns.Add("Buy Qty", Type.GetType("System.String"));
            }
            if (ViewState["Buy MktValue"].ToString() == "chk")
            {
                dtExport.Columns.Add("Buy Mkt Value", Type.GetType("System.String"));
            }
            if (ViewState["BuyAvg Mkt"].ToString() == "chk")
            {
                dtExport.Columns.Add("Buy Avg.Mkt", Type.GetType("System.String"));
            }
            if (ViewState["Buy NetValue"].ToString() == "chk")
            {
                dtExport.Columns.Add("Buy Net Value", Type.GetType("System.String"));
            }
            if (ViewState["BuyAvg Net"].ToString() == "chk")
            {
                dtExport.Columns.Add("Buy Avg.Net", Type.GetType("System.String"));
            }
            if (ViewState["Sell Qty"].ToString() == "chk")
            {
                dtExport.Columns.Add("Sell Qty", Type.GetType("System.String"));
            }
            if (ViewState["Sell MktValue"].ToString() == "chk")
            {
                dtExport.Columns.Add("Sell Mkt Value", Type.GetType("System.String"));
            }
            if (ViewState["SellAvg Mkt"].ToString() == "chk")
            {
                dtExport.Columns.Add("Sell Avg.Mkt", Type.GetType("System.String"));
            }
            if (ViewState["Sell NetValue"].ToString() == "chk")
            {
                dtExport.Columns.Add("Sell Net Value", Type.GetType("System.String"));
            }
            if (ViewState["SellAvg Net"].ToString() == "chk")
            {
                dtExport.Columns.Add("Sell Avg.Net", Type.GetType("System.String"));
            }
            if (ViewState["Diff Qty"].ToString() == "chk")
            {
                dtExport.Columns.Add("Diff Qty", Type.GetType("System.String"));
            }
            if (ViewState["Diff P/L"].ToString() == "chk")
            {
                dtExport.Columns.Add("Diff P/L", Type.GetType("System.String"));
            }
            if (ViewState["Dlv Qty"].ToString() == "chk")
            {
                dtExport.Columns.Add("Dlv Qty", Type.GetType("System.String"));
            }
            if (ViewState["Dlv Value"].ToString() == "chk")
            {
                dtExport.Columns.Add("Dlv Value", Type.GetType("System.String"));
            }
            if (ViewState["Avg Dlv"].ToString() == "chk")
            {
                dtExport.Columns.Add("Avg Dlv", Type.GetType("System.String"));
            }
            if (ViewState["Net Amnt"].ToString() == "chk")
            {
                dtExport.Columns.Add("Net Amount", Type.GetType("System.String"));
            }
            if (ViewState["STT"].ToString() == "chk")
            {
                dtExport.Columns.Add("ST Tax", Type.GetType("System.String"));
            }
            if (ViewState["Close Price"].ToString() == "chk")
            {
                dtExport.Columns.Add("Close Price", Type.GetType("System.String"));
            }
            if (ViewState["MTM"].ToString() == "chk")
            {
                dtExport.Columns.Add("MTM", Type.GetType("System.String"));
            }

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
                        row2[1] = dt1.Rows[i]["Isin"].ToString();
                        if (ViewState["Buy Qty"].ToString() == "chk")
                        {
                            if (dt1.Rows[i]["BUYQTY"] != DBNull.Value)
                                row2["Buy Qty"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["BUYQTY"].ToString()));
                        }
                        if (ViewState["Buy MktValue"].ToString() == "chk")
                        {
                            if (dt1.Rows[i]["BUYMKTVALUE"] != DBNull.Value)
                                row2["Buy Mkt Value"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["BUYMKTVALUE"].ToString()));
                        }
                        if (ViewState["BuyAvg Mkt"].ToString() == "chk")
                        {
                            if (dt1.Rows[i]["BUYMKTAVG"] != DBNull.Value)
                                row2["Buy Avg.Mkt"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["BUYMKTAVG"].ToString()));
                        }
                        if (ViewState["Buy NetValue"].ToString() == "chk")
                        {
                            if (dt1.Rows[i]["BUYNETVALUE"] != DBNull.Value)
                                row2["Buy Net Value"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["BUYNETVALUE"].ToString()));
                        }
                        if (ViewState["BuyAvg Net"].ToString() == "chk")
                        {
                            if (dt1.Rows[i]["BUYNETAVG"] != DBNull.Value)
                                row2["Buy Avg.Net"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["BUYNETAVG"].ToString()));
                        }
                        if (ViewState["Sell Qty"].ToString() == "chk")
                        {
                            if (dt1.Rows[i]["SELLQTY"] != DBNull.Value)
                                row2["Sell Qty"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["SELLQTY"].ToString()));
                        }
                        if (ViewState["Sell MktValue"].ToString() == "chk")
                        {
                            if (dt1.Rows[i]["SELLMKTVALUE"] != DBNull.Value)
                                row2["Sell Mkt Value"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["SELLMKTVALUE"].ToString()));
                        }
                        if (ViewState["SellAvg Mkt"].ToString() == "chk")
                        {
                            if (dt1.Rows[i]["SELLMKTAVG"] != DBNull.Value)
                                row2["Sell Avg.Mkt"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["SELLMKTAVG"].ToString()));
                        }
                        if (ViewState["Sell NetValue"].ToString() == "chk")
                        {
                            if (dt1.Rows[i]["SELLNETVALUE"] != DBNull.Value)
                                row2["Sell Net Value"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["SELLNETVALUE"].ToString()));
                        }
                        if (ViewState["SellAvg Net"].ToString() == "chk")
                        {
                            if (dt1.Rows[i]["SELLNETAVG"] != DBNull.Value)
                                row2["Sell Avg.Net"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["SELLNETAVG"].ToString()));
                        }
                        if (ViewState["Diff Qty"].ToString() == "chk")
                        {
                            if (dt1.Rows[i]["DIFFQTY"] != DBNull.Value)
                                row2["Diff Qty"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["DIFFQTY"].ToString()));
                        }
                        if (ViewState["Diff P/L"].ToString() == "chk")
                        {
                            if (dt1.Rows[i]["DIFFPL"] != DBNull.Value)
                                row2["Diff P/L"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["DIFFPL"].ToString()));
                        }
                        if (ViewState["Dlv Qty"].ToString() == "chk")
                        {
                            if (dt1.Rows[i]["DLVQTY"] != DBNull.Value)
                                row2["Dlv Qty"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["DLVQTY"].ToString()));
                        }
                        if (ViewState["Dlv Value"].ToString() == "chk")
                        {
                            if (dt1.Rows[i]["DLVVALUE"] != DBNull.Value)
                                row2["Dlv Value"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["DLVVALUE"].ToString()));
                        }
                        if (ViewState["Avg Dlv"].ToString() == "chk")
                        {
                            if (dt1.Rows[i]["AVGDLV"] != DBNull.Value)
                                row2["Avg Dlv"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["AVGDLV"].ToString()));
                        }
                        if (ViewState["Net Amnt"].ToString() == "chk")
                        {
                            if (dt1.Rows[i]["NETVALUE"] != DBNull.Value)
                                row2["Net Amount"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["NETVALUE"].ToString()));
                        }
                        if (ViewState["STT"].ToString() == "chk")
                        {
                            if (dt1.Rows[i]["STTAX"] != DBNull.Value)
                                row2["ST Tax"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["STTAX"].ToString()));
                        }
                        if (ViewState["Close Price"].ToString() == "chk")
                        {
                            if (dt1.Rows[i]["CLOSEPRICE"] != DBNull.Value)
                                row2["Close Price"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["CLOSEPRICE"].ToString()));
                        }
                        if (ViewState["MTM"].ToString() == "chk")
                        {
                            if (dt1.Rows[i]["MTM"] != DBNull.Value)
                                row2["MTM"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["MTM"].ToString()));
                        }

                        dtExport.Rows.Add(row2);
                    }
                    //////////client total
                    DataRow row3 = dtExport.NewRow();
                    row3[0] = "Total :";
                    if (ViewState["Buy NetValue"].ToString() == "chk")
                    {
                        if (dt1.Rows[0]["BUYNETVALUE_SUM"] != DBNull.Value)
                            row3["Buy Net Value"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["BUYNETVALUE_SUM"].ToString()));
                    }
                    if (ViewState["Sell NetValue"].ToString() == "chk")
                    {
                        if (dt1.Rows[0]["SELLNETVALUE_SUM"] != DBNull.Value)
                            row3["Sell Net Value"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["SELLNETVALUE_SUM"].ToString()));
                    }
                    if (ViewState["Diff P/L"].ToString() == "chk")
                    {
                        if (dt1.Rows[0]["DIFFPL_SUM"] != DBNull.Value)
                            row3["Diff P/L"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["DIFFPL_SUM"].ToString()));
                    }
                    if (ViewState["Dlv Value"].ToString() == "chk")
                    {
                        if (dt1.Rows[0]["DLVVALUE_SUM"] != DBNull.Value)
                            row3["Dlv Value"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["DLVVALUE_SUM"].ToString()));
                    }
                    if (ViewState["Net Amnt"].ToString() == "chk")
                    {
                        if (dt1.Rows[0]["NETAMNT_SUM"] != DBNull.Value)
                            row3["Net Amount"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["NETAMNT_SUM"].ToString()));
                    }
                    if (ViewState["STT"].ToString() == "chk")
                    {
                        if (dt1.Rows[0]["STT"] != DBNull.Value)
                            row3["ST Tax"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["STT"].ToString()));
                    }
                    if (ViewState["MTM"].ToString() == "chk")
                    {
                        if (dt1.Rows[0]["MTM_SUM"] != DBNull.Value)
                            row3["MTM"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["MTM_SUM"].ToString()));
                    }
                    dtExport.Rows.Add(row3);

                    ///////////charges total
                    DataRow row111 = dtExport.NewRow();
                    row111[0] = "Charges Calculation";
                    row111[1] = "Test";
                    dtExport.Rows.Add(row111);

                    ///////////STax On Brkg
                    if (dt1.Rows[0]["BRKGCHARGE_NAME"] != DBNull.Value)
                    {
                        DataRow row4 = dtExport.NewRow();
                        row4[0] = dt1.Rows[0]["BRKGCHARGE_NAME"].ToString();
                        row4[1] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["BRKGCHARGE_SUM"].ToString()));
                        dtExport.Rows.Add(row4);
                    }
                    ///////////Min. Processesing Charge
                    if (dt1.Rows[0]["DIFFBRKG_NAME"] != DBNull.Value)
                    {
                        DataRow row5 = dtExport.NewRow();
                        row5[0] = dt1.Rows[0]["DIFFBRKG_NAME"].ToString();
                        row5[1] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["DIFFBRKG"].ToString()));
                        dtExport.Rows.Add(row5);
                    }
                    ///////////STax On Min. Processesing Charge
                    if (dt1.Rows[0]["SRVDIFFBRKG_NAME"] != DBNull.Value)
                    {
                        DataRow row6 = dtExport.NewRow();
                        row6[0] = dt1.Rows[0]["SRVDIFFBRKG_NAME"].ToString();
                        row6[1] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["SRVDIFFBRKG"].ToString()));
                        dtExport.Rows.Add(row6);
                    }
                    ///////////Transaction Charges
                    if (dt1.Rows[0]["TRANCHARGE_NAME"] != DBNull.Value)
                    {
                        DataRow row7 = dtExport.NewRow();
                        row7[0] = dt1.Rows[0]["TRANCHARGE_NAME"].ToString();
                        row7[1] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["TRANCHARGE"].ToString()));
                        dtExport.Rows.Add(row7);
                    }
                    ///////////STax On Trn.Charges
                    if (dt1.Rows[0]["SRVTAXTRANCHARGE_NAME"] != DBNull.Value)
                    {
                        DataRow row8 = dtExport.NewRow();
                        row8[0] = dt1.Rows[0]["SRVTAXTRANCHARGE_NAME"].ToString();
                        row8[1] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["SRVTAXTRANCHARGE"].ToString()));
                        dtExport.Rows.Add(row8);
                    }
                    ///////////Stamp Duty
                    if (dt1.Rows[0]["STAMPCHARGE_NAME"] != DBNull.Value)
                    {
                        DataRow row9 = dtExport.NewRow();
                        row9[0] = dt1.Rows[0]["STAMPCHARGE_NAME"].ToString();
                        row9[1] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["STAMPCHARGE"].ToString()));
                        dtExport.Rows.Add(row9);
                    }
                    ///////////STT Tax
                    if (dt1.Rows[0]["STTAX_NAME"] != DBNull.Value)
                    {
                        DataRow row10 = dtExport.NewRow();
                        row10[0] = dt1.Rows[0]["STTAX_NAME"].ToString();
                        row10[1] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["STTAX_SUM"].ToString()));
                        dtExport.Rows.Add(row10);
                    }
                    ///////////SEBI Fee
                    if (dt1.Rows[0]["SEBICHARGE_NAME"] != DBNull.Value)
                    {
                        DataRow row11 = dtExport.NewRow();
                        row11[0] = dt1.Rows[0]["SEBICHARGE_NAME"].ToString();
                        row11[1] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["SEBICHARGE"].ToString()));
                        dtExport.Rows.Add(row11);
                    }
                    ///////////Delivery Charges
                    if (dt1.Rows[0]["DELIVERYCHARGE_NAME"] != DBNull.Value)
                    {
                        DataRow row12 = dtExport.NewRow();
                        row12[0] = dt1.Rows[0]["DELIVERYCHARGE_NAME"].ToString();
                        row12[1] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["DELIVERYCHARGE"].ToString()));
                        dtExport.Rows.Add(row12);
                    }
                    ///////////Srv Tax On Delivery Charges
                    if (dt1.Rows[0]["SRVTAXDELIVERYCHARGE_NAME"] != DBNull.Value)
                    {
                        DataRow row13 = dtExport.NewRow();
                        row13[0] = dt1.Rows[0]["SRVTAXDELIVERYCHARGE_NAME"].ToString();
                        row13[1] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["SRVTAXDELIVERYCHARGE"].ToString()));
                        dtExport.Rows.Add(row13);
                    }
                    ///////////Round Off Adjustment
                    if (dt1.Rows[0]["NETROUNDOFF_NAME"] != DBNull.Value)
                    {
                        DataRow row14 = dtExport.NewRow();
                        row14[0] = dt1.Rows[0]["NETROUNDOFF_NAME"].ToString();
                        row14[1] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["NETROUNDOFF"].ToString()));
                        dtExport.Rows.Add(row14);
                    }
                    ///////////Net Total
                    if (dt1.Rows[0]["NETOBLIGATIONCHARGE"] != DBNull.Value)
                    {
                        DataRow row15 = dtExport.NewRow();
                        row15[0] = dt1.Rows[0]["NETOBLIGATIONCHARGE_NAME"].ToString();
                        row15[1] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["NETOBLIGATIONCHARGE"].ToString()));
                        dtExport.Rows.Add(row15);
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
            if (ViewState["Buy Qty"].ToString() == "chk")
            {
                dtExport.Columns.Add("Buy Qty", Type.GetType("System.String"));
            }
            if (ViewState["Buy MktValue"].ToString() == "chk")
            {
                dtExport.Columns.Add("Buy Mkt Value", Type.GetType("System.String"));
            }
            if (ViewState["BuyAvg Mkt"].ToString() == "chk")
            {
                dtExport.Columns.Add("Buy Avg.Mkt", Type.GetType("System.String"));
            }
            if (ViewState["Buy NetValue"].ToString() == "chk")
            {
                dtExport.Columns.Add("Buy Net Value", Type.GetType("System.String"));
            }
            if (ViewState["BuyAvg Net"].ToString() == "chk")
            {
                dtExport.Columns.Add("Buy Avg.Net", Type.GetType("System.String"));
            }
            if (ViewState["Sell Qty"].ToString() == "chk")
            {
                dtExport.Columns.Add("Sell Qty", Type.GetType("System.String"));
            }
            if (ViewState["Sell MktValue"].ToString() == "chk")
            {
                dtExport.Columns.Add("Sell Mkt Value", Type.GetType("System.String"));
            }
            if (ViewState["SellAvg Mkt"].ToString() == "chk")
            {
                dtExport.Columns.Add("Sell Avg.Mkt", Type.GetType("System.String"));
            }
            if (ViewState["Sell NetValue"].ToString() == "chk")
            {
                dtExport.Columns.Add("Sell Net Value", Type.GetType("System.String"));
            }
            if (ViewState["SellAvg Net"].ToString() == "chk")
            {
                dtExport.Columns.Add("Sell Avg.Net", Type.GetType("System.String"));
            }
            if (ViewState["Diff Qty"].ToString() == "chk")
            {
                dtExport.Columns.Add("Diff Qty", Type.GetType("System.String"));
            }
            if (ViewState["Diff P/L"].ToString() == "chk")
            {
                dtExport.Columns.Add("Diff P/L", Type.GetType("System.String"));
            }
            if (ViewState["Dlv Qty"].ToString() == "chk")
            {
                dtExport.Columns.Add("Dlv Qty", Type.GetType("System.String"));
            }
            if (ViewState["Dlv Value"].ToString() == "chk")
            {
                dtExport.Columns.Add("Dlv Value", Type.GetType("System.String"));
            }
            if (ViewState["Avg Dlv"].ToString() == "chk")
            {
                dtExport.Columns.Add("Avg Dlv", Type.GetType("System.String"));
            }
            if (ViewState["Net Amnt"].ToString() == "chk")
            {
                dtExport.Columns.Add("Net Amount", Type.GetType("System.String"));
            }
            if (ViewState["STT"].ToString() == "chk")
            {
                dtExport.Columns.Add("ST Tax", Type.GetType("System.String"));
            }
            if (ViewState["Close Price"].ToString() == "chk")
            {
                dtExport.Columns.Add("Close Price", Type.GetType("System.String"));
            }
            if (ViewState["MTM"].ToString() == "chk")
            {
                dtExport.Columns.Add("MTM", Type.GetType("System.String"));
            }

            /////////Share Name Bind
            ds = (DataSet)ViewState["dataset"];
            DataView viewgroup = new DataView(ds.Tables[0]);
            dtgroupcontactid = viewgroup.ToTable(true, new string[] { "PRODUCTID", "TickerSymbolExport" });
            cmbgroup.Items.Clear();

            if (dtgroupcontactid.Rows.Count > 0)
            {
                cmbgroup.DataSource = dtgroupcontactid;
                cmbgroup.DataValueField = "PRODUCTID";
                cmbgroup.DataTextField = "TickerSymbolExport";
                cmbgroup.DataBind();

            }

            for (int j = 0; j < cmbgroup.Items.Count; j++)
            {
                DataRow row = dtExport.NewRow();
                row[0] = "Share Name:" + cmbgroup.Items[j].Text.ToString().Trim();
                row[1] = "Test";
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
                    if (ViewState["Buy Qty"].ToString() == "chk")
                    {
                        if (dt1.Rows[i]["BUYQTY"] != DBNull.Value)
                            row2["Buy Qty"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["BUYQTY"].ToString()));
                    }
                    if (ViewState["Buy MktValue"].ToString() == "chk")
                    {
                        if (dt1.Rows[i]["BUYMKTVALUE"] != DBNull.Value)
                            row2["Buy Mkt Value"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["BUYMKTVALUE"].ToString()));
                    }
                    if (ViewState["BuyAvg Mkt"].ToString() == "chk")
                    {
                        if (dt1.Rows[i]["BUYMKTAVG"] != DBNull.Value)
                            row2["Buy Avg.Mkt"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["BUYMKTAVG"].ToString()));
                    }
                    if (ViewState["Buy NetValue"].ToString() == "chk")
                    {
                        if (dt1.Rows[i]["BUYNETVALUE"] != DBNull.Value)
                            row2["Buy Net Value"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["BUYNETVALUE"].ToString()));
                    }
                    if (ViewState["BuyAvg Net"].ToString() == "chk")
                    {
                        if (dt1.Rows[i]["BUYNETAVG"] != DBNull.Value)
                            row2["Buy Avg.Net"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["BUYNETAVG"].ToString()));
                    }
                    if (ViewState["Sell Qty"].ToString() == "chk")
                    {
                        if (dt1.Rows[i]["SELLQTY"] != DBNull.Value)
                            row2["Sell Qty"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["SELLQTY"].ToString()));
                    }
                    if (ViewState["Sell MktValue"].ToString() == "chk")
                    {
                        if (dt1.Rows[i]["SELLMKTVALUE"] != DBNull.Value)
                            row2["Sell Mkt Value"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["SELLMKTVALUE"].ToString()));
                    }
                    if (ViewState["SellAvg Mkt"].ToString() == "chk")
                    {
                        if (dt1.Rows[i]["SELLMKTAVG"] != DBNull.Value)
                            row2["Sell Avg.Mkt"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["SELLMKTAVG"].ToString()));
                    }
                    if (ViewState["Sell NetValue"].ToString() == "chk")
                    {
                        if (dt1.Rows[i]["SELLNETVALUE"] != DBNull.Value)
                            row2["Sell Net Value"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["SELLNETVALUE"].ToString()));
                    }
                    if (ViewState["SellAvg Net"].ToString() == "chk")
                    {
                        if (dt1.Rows[i]["SELLNETAVG"] != DBNull.Value)
                            row2["Sell Avg.Net"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["SELLNETAVG"].ToString()));
                    }
                    if (ViewState["Diff Qty"].ToString() == "chk")
                    {
                        if (dt1.Rows[i]["DIFFQTY"] != DBNull.Value)
                            row2["Diff Qty"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["DIFFQTY"].ToString()));
                    }
                    if (ViewState["Diff P/L"].ToString() == "chk")
                    {
                        if (dt1.Rows[i]["DIFFPL"] != DBNull.Value)
                            row2["Diff P/L"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["DIFFPL"].ToString()));
                    }
                    if (ViewState["Dlv Qty"].ToString() == "chk")
                    {
                        if (dt1.Rows[i]["DLVQTY"] != DBNull.Value)
                            row2["Dlv Qty"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["DLVQTY"].ToString()));
                    }
                    if (ViewState["Dlv Value"].ToString() == "chk")
                    {
                        if (dt1.Rows[i]["DLVVALUE"] != DBNull.Value)
                            row2["Dlv Value"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["DLVVALUE"].ToString()));
                    }
                    if (ViewState["Avg Dlv"].ToString() == "chk")
                    {
                        if (dt1.Rows[i]["AVGDLV"] != DBNull.Value)
                            row2["Avg Dlv"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["AVGDLV"].ToString()));
                    }
                    if (ViewState["Net Amnt"].ToString() == "chk")
                    {
                        if (dt1.Rows[i]["NETVALUE"] != DBNull.Value)
                            row2["Net Amount"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["NETVALUE"].ToString()));
                    }
                    if (ViewState["STT"].ToString() == "chk")
                    {
                        if (dt1.Rows[i]["STTAX"] != DBNull.Value)
                            row2["ST Tax"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["STTAX"].ToString()));
                    }
                    if (ViewState["Close Price"].ToString() == "chk")
                    {
                        if (dt1.Rows[i]["CLOSEPRICE"] != DBNull.Value)
                            row2["Close Price"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["CLOSEPRICE"].ToString()));
                    }
                    if (ViewState["MTM"].ToString() == "chk")
                    {
                        if (dt1.Rows[i]["MTM"] != DBNull.Value)
                            row2["MTM"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[i]["MTM"].ToString()));
                    }
                    dtExport.Rows.Add(row2);
                }


                //////////share total
                DataRow row3 = dtExport.NewRow();
                row3[0] = "Total :";
                if (ViewState["Buy NetValue"].ToString() == "chk")
                {
                    if (dt1.Rows[0]["BUYNETVALUE_SUM"] != DBNull.Value)
                        row3["Buy Net Value"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["BUYNETVALUE_SUM"].ToString()));
                }
                if (ViewState["Sell NetValue"].ToString() == "chk")
                {
                    if (dt1.Rows[0]["SELLNETVALUE_SUM"] != DBNull.Value)
                        row3["Sell Net Value"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["SELLNETVALUE_SUM"].ToString()));
                }
                if (ViewState["Diff P/L"].ToString() == "chk")
                {
                    if (dt1.Rows[0]["DIFFPL_SUM"] != DBNull.Value)
                        row3["Diff P/L"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["DIFFPL_SUM"].ToString()));
                }
                if (ViewState["Dlv Value"].ToString() == "chk")
                {
                    if (dt1.Rows[0]["DLVVALUE_SUM"] != DBNull.Value)
                        row3["Dlv Value"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["DLVVALUE_SUM"].ToString()));
                }
                if (ViewState["Net Amnt"].ToString() == "chk")
                {
                    if (dt1.Rows[0]["NETAMNT_SUM"] != DBNull.Value)
                        row3["Net Amount"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["NETAMNT_SUM"].ToString()));
                }
                if (ViewState["STT"].ToString() == "chk")
                {
                    if (dt1.Rows[0]["STT"] != DBNull.Value)
                        row3["ST Tax"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["STT"].ToString()));
                }
                if (ViewState["MTM"].ToString() == "chk")
                {
                    if (dt1.Rows[0]["MTM_SUM"] != DBNull.Value)
                        row3["MTM"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dt1.Rows[0]["MTM_SUM"].ToString()));
                }
                dtExport.Rows.Add(row3);


            }
            ViewState["dtExport"] = dtExport;
        }
        void export()
        {
            dtExport = (DataTable)ViewState["dtExport"];
            DataRow Dr;
            int RowCount = 0;
            if (chkrawprint.Checked)
            {
                DataColumn Col1 = new DataColumn("CustomerName", typeof(string));
                DataColumn Col2 = new DataColumn("CustomerCode", typeof(string));
                dtExport.Columns.Add(Col1);
                dtExport.Columns.Add(Col2);
                string CustomerName = String.Empty;
                string CustomerCode = String.Empty;
                string strTemp = String.Empty;
                bool DelOrNot1 = false;
                bool DelOrNot2 = false;
                for (; RowCount < dtExport.Rows.Count; RowCount++)
                {
                    Dr = dtExport.Rows[RowCount];
                    if (Dr != null)
                    {
                        if (Dr[0].ToString().StartsWith("Client Name"))
                        {
                            strTemp = Dr[0].ToString();
                            CustomerName = strTemp.Substring(strTemp.IndexOf(":") + 1, (strTemp.IndexOf("[") - strTemp.IndexOf(":")) - 1);
                            CustomerCode = strTemp.Substring(strTemp.IndexOf("[") + 1, (strTemp.IndexOf("]") - strTemp.IndexOf("[")) - 1);

                        }
                        if (CustomerName != String.Empty && CustomerCode != String.Empty)
                        {

                            dtExport.Rows[RowCount]["CustomerName"] = CustomerName;
                            dtExport.Rows[RowCount]["CustomerCode"] = CustomerCode;
                        }
                        if (Dr[0].ToString().Trim() == "Charges Calculation" || Dr[0].ToString().Trim().StartsWith("Branch Name") || Dr[0].ToString().StartsWith("Client Name"))
                        {
                            DelOrNot1 = true;
                        }
                        if (!DelOrNot1)
                        {
                            if (Dr[0].ToString().Trim() == "STax On Brkg[Exclusive]:" || Dr[0].ToString().Trim() == "STT Tax :" || Dr[0].ToString().Trim() == "Round Off Adjustment :" || Dr[0].ToString().Trim() == "Net Payable To You :")
                            {
                                if (!ChkCalculateCharge.Checked)
                                {
                                    DelOrNot2 = true;
                                }
                            }
                        }
                        if (DelOrNot1 || DelOrNot2)
                        {
                            Dr.Delete();
                            RowCount = RowCount - 1;
                            DelOrNot1 = false;
                            DelOrNot2 = false;
                        }
                    }
                }
            }

            if (!ChkCalculateCharge.Checked)
            {
                for (; RowCount < dtExport.Rows.Count; RowCount++)
                {
                    Dr = dtExport.Rows[RowCount];

                    if (Dr[0].ToString().Trim() == "STax On Brkg[Exclusive]:" || Dr[0].ToString().Trim() == "STT Tax :" || Dr[0].ToString().Trim() == "Round Off Adjustment :" || Dr[0].ToString().Trim() == "Net Payable To You :" || Dr[0].ToString().Trim() == "Charges Calculation")
                    {
                        if (Dr != null)
                        {
                            Dr.Delete();
                            RowCount = RowCount - 1;
                        }
                    }
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
                if (chkrawprint.Checked) objExcel.ExportToExcelforExcel(dtExport, "Net Position", "Total :", dtReportHeader, dtReportFooter, "Raw");
                else objExcel.ExportToExcelforExcel(dtExport, "Net Position", "Total :", dtReportHeader, dtReportFooter);
            }
            if (cmbExport.SelectedItem.Value == "P")
            {
                objExcel.ExportToPDF(dtExport, "Net Position", "Total :", dtReportHeader, dtReportFooter);
            }
        }
        protected void btnmailsend_Click(object sender, EventArgs e)
        {
            procedure();
            ds = (DataSet)ViewState["dataset"];
            if (ds.Tables[0].Rows.Count > 0)
            {
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
                    Email_Subject = "[ " + Session["Segmentname"].ToString() + " ]" + "[ " + oGenericMethod.GetClientInfo(cmbclient.Items[k].Value, 1).Rows[0][2].ToString().Trim() + " ]" + " Net Position [" + ViewState["billdate"].ToString().Trim() + "]";
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
                dtgroupcontactid = viewgroup.ToTable(true, new string[] { "PRODUCTID", "TickerSymbolExport" });
                if (dtgroupcontactid.Rows.Count > 0)
                {
                    cmbgroup.DataSource = dtgroupcontactid;
                    cmbgroup.DataValueField = "PRODUCTID";
                    cmbgroup.DataTextField = "TickerSymbolExport";
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
                    if (oDBEngine.SendReportSt(ViewState["GRPmail"].ToString().Trim(), clnt[i].ToString().Trim(), ViewState["billdate"].ToString().Trim(), "[ " + Session["Segmentname"].ToString() + "Net Position [" + ViewState["billdate"].ToString().Trim() + "]") == true)
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
    }
}
