using System;
using System.Collections;
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
    public partial class Reports_New_frmReport_TradeRegisteriframe : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        # region Local Variables
        DailyReports dailyrep = new DailyReports();
        ExcelFile objExcel = new ExcelFile();
        string data = null;
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        string reportType = null;
        string bindParams = null;
        DataSet ds = new DataSet();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        int pageindex = 0;
        ReportDocument ReportDocument = new ReportDocument();
        string[] connPath = ConfigurationManager.AppSettings["DBConnectionDefault"].Split(';');
        # endregion Local Variables
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
            Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");
            DataTable dtname = oDBEngine.GetDataTable(" tbl_master_company  ", "cmp_Name", "cmp_internalid='" + HttpContext.Current.Session["LastCompany"] + "' ");
            ViewState["CompanyName"] = dtname.Rows[0]["cmp_Name"].ToString();
            //=========================
            //HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "1" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "4"
            //string ExchangeSegmentID=HttpContext.Current.Session["ExchangeSegmentID"].ToString();
            //=========================
            if (!IsPostBack)
            {
                FnTradeType();
                if (Request.QueryString["Custid"] == null)
                {
                    SettNo();
                    Date();
                    SegmentnameFetch();
                    chkboxliststyle();
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "JScriptDemat", "<script language='javascript'>ForDemat();</script>");
                    DataTable DTSettDate = oDBEngine.GetDataTable("master_settlements", " settlements_startdatetime,settlements_enddatetime", " settlements_number='" + Request.QueryString["SettNo"].ToString() + "' and settlements_typesuffix='" + Request.QueryString["SettType"].ToString() + "'");
                    dtFrom.Value = Convert.ToDateTime(DTSettDate.Rows[0][0].ToString());
                    dtTo.Value = Convert.ToDateTime(DTSettDate.Rows[0][1].ToString());
                    FilterColumnCheck();
                    //if (ddlGeneration.SelectedItem.Value.ToString() == "2") reportType = "Screen";
                    if (Request.QueryString["WhichCall"] != null)
                    {
                        string cntract = Request.QueryString["ContractNo"].ToString().Trim();
                        reportType = "cntrscreen" + '~' + cntract;
                    }
                    else
                        reportType = "Screen";
                    Procedure(bindParams, reportType);

                }

            }
            //_____For performing operation without refreshing page___//

            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //___________-end here___//
        }

        # region DDL/Combo Boxes & Hidden Fields
        void SettNo()
        {
            litSettlementNo.InnerText = Session["LastSettNo"].ToString().Substring(0, 7);
            litSettlementType.InnerText = Session["LastSettNo"].ToString().Substring(7, 1);
            HiddenField_SettNo.Value = Session["LastSettNo"].ToString().Substring(0, 7);
            HiddenField_Setttype.Value = "'" + Session["LastSettNo"].ToString().Substring(7, 1) + "'";
        }
        void SegmentnameFetch()
        {
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "1")
                litSegmentMain.InnerText = "NSE-CM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "4")
                litSegmentMain.InnerText = "BSE-CM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "15")
                litSegmentMain.InnerText = "CSE-CM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "2")
                litSegmentMain.InnerText = "NSE-FO";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "5")
                litSegmentMain.InnerText = "BSE-FO";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "19")
                litSegmentMain.InnerText = "MCXSX-CM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "20")
                litSegmentMain.InnerText = "MCXSX-FO";
            HiddenField_Segment.Value = Session["usersegid"].ToString();
        }
        void Date()
        {
            dtFrom.EditFormatString = oconverter.GetDateFormat("Date");
            dtTo.EditFormatString = oconverter.GetDateFormat("Date");

            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "1" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "4" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "15" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "19")
            {
                ///Start Date and End Date Fetch///////
                string[,] DtStartEnddate = oDBEngine.GetFieldValue("Master_Settlements", "(cast(Settlements_StartDateTime as varchar)+','+cast(Settlements_FundsPayin as varchar)+','+cast(Settlements_EndDateTime as varchar))", "settlements_Number='" + Session["LastSettNo"].ToString().Substring(0, 7) + "' and settlements_TypeSuffix='" + Session["LastSettNo"].ToString().Substring(7, 1) + "' and Settlements_ExchangeSegmentID='" + HttpContext.Current.Session["ExchangeSegmentID"] + "'", 1);
                if (DtStartEnddate[0, 0] != "n")
                {
                    string[] idlist = DtStartEnddate[0, 0].ToString().Split(','); // fetch startdate and FundsPayin and End Date from Master_Settlements
                    dtFrom.Value = Convert.ToDateTime(idlist[0]);
                    dtTo.Value = Convert.ToDateTime(idlist[2]);
                }
            }
            else
            {
                dtFrom.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
                dtTo.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());

            }
        }
        void FnTradeType()
        {
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "2" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "5" || HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "20")
            {
                ddlTradeType.Items.Insert(0, new ListItem("All Types", "3"));
                ddlTradeType.Items.Insert(1, new ListItem("Only Confirmed", "2"));
                ddlTradeType.Items.Insert(2, new ListItem("Show Only Zero Brkg Trades", "4"));
                ddlTradeType.SelectedValue = "3";
            }
            else
            {
                ddlTradeType.Items.Insert(0, new ListItem("Only Delivery", "0"));
                ddlTradeType.Items.Insert(1, new ListItem("Only ND", "1"));
                ddlTradeType.Items.Insert(2, new ListItem("Only Confirmed", "2"));
                ddlTradeType.Items.Insert(3, new ListItem("All Types", "3"));
                ddlTradeType.Items.Insert(4, new ListItem("Show Only Zero Brkg Trades", "4"));
                ddlTradeType.SelectedValue = "3";
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
                    if ((idlist[0] != "Clients") && (idlist[0] != "Broker"))
                    {
                        if (idlist[0] == "SettlementType")
                        {
                            string[] val = cl[i].Split(';');
                            if (str == "")
                            {
                                str = "'" + val[0] + "'";
                                str1 = "'" + val[0] + "'" + ";" + val[1];
                            }
                            else
                            {
                                str += ",'" + val[0] + "'";
                                str1 += "," + "'" + val[0] + "'" + ";" + val[1];
                            }
                        }
                        else
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

                if (idlist[0] == "Branch")
                {
                    data = "Branch~" + str;
                }
                else if (idlist[0] == "BranchGroup")
                {
                    data = "BranchGroup~" + str;
                }
                else if (idlist[0] == "Segment")
                {
                    data = "Segment~" + str;
                }
                else if (idlist[0] == "Broker")
                {
                    data = "Broker~" + str;
                }
                else if (idlist[0] == "Clients")
                {
                    data = "Clients~" + str;
                }
                else if (idlist[0] == "SettlementNo")
                {
                    data = "SettlementNo~" + str;
                }
                else if (idlist[0] == "SettlementType")
                {
                    data = "SettlementType~" + str;
                }
                else if (idlist[0] == "Group")
                {
                    data = "Group~" + str;
                }
                else if (idlist[0] == "MAILEMPLOYEE")
                {
                    data = "MAILEMPLOYEE~" + str;
                }
                else if (idlist[0] == "Scrips")
                {
                    //=====22-05-2014=======Modified For FOIdentiFier for FO Segment Add instrument type===========
                    string FOIdentity = null;
                    DataTable DTFOIdntity = oDBEngine.GetDataTable(@"select distinct equity_FOIdentifier from Master_Equity where Equity_SeriesID in(" + str + ")");
                    if (DTFOIdntity.Rows.Count > 0)
                    {
                        foreach (DataRow DrFOIdntity in DTFOIdntity.Rows)
                        {
                            if (FOIdentity == null) FOIdentity = DrFOIdntity[0].ToString().Trim();
                            else FOIdentity = FOIdentity + "," + DrFOIdntity[0].ToString().Trim();
                        }
                    }
                    data = "Scrips~" + str + "~" + FOIdentity;
                }
                else if (idlist[0] == "Product")
                {
                    //=====22-05-2014=======Modified For FOIdentiFier for FO Segment Add instrument type===========
                    string SeriesIDs = null;
                    DataTable DT = oDBEngine.GetDataTable(@"Master_Equity Where Equity_ProductId in (
                Select Products_ID from Master_Products where (Products_ID in (" + str + ") or Products_DerivedFromID in (" + str + @")))
                and Equity_ExchSegmentID in (1,2,4,5)", "Equity_SeriesID", null);
                    if (DT.Rows.Count > 0)
                    {
                        foreach (DataRow DrSeriesID in DT.Rows)
                        {
                            if (SeriesIDs == null) SeriesIDs = DrSeriesID[0].ToString();
                            else SeriesIDs = SeriesIDs + "," + DrSeriesID[0].ToString();
                        }
                    }
                    //====Query fetch for Retrieve FOIdentifier From Equity_SeriesID Of HiddenField_Product======
                    string FOIdentity = null;
                    DataTable DTFOIdntity = oDBEngine.GetDataTable(@"select distinct equity_FOIdentifier from Master_Equity where Equity_SeriesID in(" + SeriesIDs + ")");
                    if (DTFOIdntity.Rows.Count > 0)
                    {
                        foreach (DataRow DrFOIdntity in DTFOIdntity.Rows)
                        {
                            if (DrFOIdntity[0].ToString().Trim() != string.Empty)
                            {
                                if (FOIdentity == null) FOIdentity = DrFOIdntity[0].ToString().Trim();
                                else FOIdentity = FOIdentity + "," + DrFOIdntity[0].ToString().Trim();
                            }
                        }
                    }
                    data = "Product~" + SeriesIDs + "~" + FOIdentity;
                }
            }
        }
        void chkboxliststyle()
        {
            foreach (ListItem item in ChkFilterDetail.Items)
            {
                item.Attributes.Add("style", "font-family:Times New Roman;color:#461B7E;font-size:9px");
            }
        }
        public void BindGroup()
        {
            DataTable DtGroup = oDBEngine.GetDataTable("tbl_master_groupmaster", "distinct gpm_Type", null);
            if (DtGroup.Rows.Count > 0)
            {
                ddlGroup.DataSource = DtGroup;
                ddlGroup.DataTextField = "gpm_Type";
                ddlGroup.DataValueField = "gpm_Type";
                ddlGroup.DataBind();
                DtGroup.Dispose();
            }
        }
        protected void BtnGroup_Click(object sender, EventArgs e)
        {
            if (ddlGroupBy.SelectedItem.Value.ToString().Trim() == "Group")
            {
                if (HiddenField_Group.Value.ToString().Trim() == "")
                {
                    BindGroup();
                }
            }
        }
        # endregion DDL & Hidden Fields

        void FilterColumnCheck()
        {
            string Terminalid = "N";
            string TradeCode = "N";
            string OrderNo = "N";
            string OrderEntryTime = "N";
            string TradeNo = "N";
            string TradeEntryTime = "N";
            string CntrNo = "N";
            int FilterCount = 0;
            /////////Filter Column Begin
            foreach (ListItem listitem in ChkFilterDetail.Items)
            {
                if (listitem.Selected)
                {
                    if (listitem.Value == "Terminalid")
                    {
                        Terminalid = "Y";
                        FilterCount = FilterCount + 1;
                    }
                    if (listitem.Value == "TradeCode")
                    {
                        TradeCode = "Y";
                        FilterCount = FilterCount + 1;
                    }
                    if (listitem.Value == "OrderNo")
                    {
                        OrderNo = "Y";
                        FilterCount = FilterCount + 1;
                    }
                    if (listitem.Value == "OrderEntryTime")
                    {
                        OrderEntryTime = "Y";
                        FilterCount = FilterCount + 1;
                    }
                    if (listitem.Value == "TradeNo")
                    {
                        TradeNo = "Y";
                        FilterCount = FilterCount + 1;
                    }
                    if (listitem.Value == "TradeEntryTime")
                    {
                        TradeEntryTime = "Y";
                        FilterCount = FilterCount + 1;
                    }
                    if (listitem.Value == "CntrNo")
                    {
                        CntrNo = "Y";
                        FilterCount = FilterCount + 1;
                    }
                }
            }
            ////////Filter Column End       

            ///////SPCall
            bindParams = Terminalid.ToString().Trim() + '~' + TradeCode.ToString().Trim() + '~' + OrderNo.ToString().Trim() + '~' + OrderEntryTime.ToString().Trim() + '~' + TradeNo.ToString().Trim() + '~' + TradeEntryTime.ToString().Trim() + '~' + CntrNo.ToString().Trim();

        }

        void Procedure(string parameter, string reportType)
        {
            string[,] strParam = new string[24, 2];
            string SpName = String.Empty;
            if (ddlTradeType.SelectedValue == "4")
            {
            }
            string strFOIdentifier = "";
            if ((Session["ExchangeSegmentID"].ToString() == "2") || (Session["ExchangeSegmentID"].ToString() == "5") || (Session["ExchangeSegmentID"].ToString() == "20"))
            {
                strFOIdentifier = HiddenField_SelectedFOIdentifier.Value.ToString();
            }
            if (Request.QueryString["Custid"] != null && Request.QueryString["DematID"] != null)//////////PopUp Display From Delivery Center
            {
                HiddenField_Client.Value = "'" + Request.QueryString["Custid"].ToString().Trim() + "'";
                RadioBtnOtherGroupBySelected.Checked = true;
                RadioBtnOtherGroupByAll.Checked = false;
                HiddenField_Instrument.Value = Request.QueryString["ProdID"].ToString().Trim();
                rdInstrumentSelected.Checked = true;
                rdInstrumentAll.Checked = false;
                HiddenField_SettNo.Value = Request.QueryString["SettNo"].ToString().Trim();
                HiddenField_Setttype.Value = "'" + Request.QueryString["SettType"].ToString().Trim() + "'";

                DataTable dtSeg = oDBEngine.GetDataTable("Trans_DematPosition", "DematPosition_SegmentID", " DematPosition_ID=" + Request.QueryString["DematID"].ToString() + "");
                HiddenField_Segment.Value = dtSeg.Rows[0][0].ToString();
                rdbSegSelected.Checked = true;
                rdbSegAll.Checked = false;
                ddlGroupBy.SelectedItem.Value = "Clients";
                ddlGeneration.SelectedItem.Value = "Screen";
            }
            else if (Request.QueryString["Custid"] != null)//////////PopUp Display From contractregisteriframe 
            {
                HiddenField_Client.Value = "'" + Request.QueryString["Custid"].ToString().Trim() + "'";
                RadioBtnOtherGroupBySelected.Checked = true;
                RadioBtnOtherGroupByAll.Checked = false;
                HiddenField_Instrument.Value = Request.QueryString["ProdID"].ToString().Trim();
                rdInstrumentSelected.Checked = true;
                rdInstrumentAll.Checked = false;
                HiddenField_SettNo.Value = Request.QueryString["SettNo"].ToString().Trim();
                HiddenField_Setttype.Value = "'" + Request.QueryString["SettType"].ToString().Trim() + "'";
                HiddenField_Segment.Value = Request.QueryString["Segment"].ToString();
                string contractno = "'" + Request.QueryString["ContractNo"].ToString().Trim() + "'";
                //reportType = "'" + Request.QueryString["WhichCall"].ToString().Trim() + "'";

                rdbSegSelected.Checked = true;
                rdbSegAll.Checked = false;
                ddlGroupBy.SelectedItem.Value = "Clients";
                ddlGeneration.SelectedItem.Value = "Screen";
            }


            string GrpType = "";
            string GrpId = "";
            string Clients = "";
            string Broker = "";
            string TradeCategory = "";
            string Terminalid = "";
            string CtClid = "";
            string Scrip = "";
            string Segment = "";
            string Settno = "";
            string SettType = "";

            SpName = "[TradeRegister]";//SD Code

            strParam[0, 0] = "companyid"; strParam[0, 1] = "'" + Session["LastCompany"].ToString().Replace(",", "','") + "'";//SDCode
            strParam[1, 0] = "FromDate"; strParam[1, 1] = "'" + dtFrom.Value.ToString() + "'";//SDCode
            strParam[2, 0] = "ToDate"; strParam[2, 1] = "'" + dtTo.Value.ToString() + "'";//SDCode
            strParam[3, 0] = "TradeType"; strParam[3, 1] = "'" + ddlTradeType.SelectedItem.Value.ToString().Trim() + "'";//SDCode
            strParam[4, 0] = "RptType"; strParam[4, 1] = "'" + reportType + "'";//SDCode
            strParam[5, 0] = "BranchHierchy"; strParam[5, 1] = "'" + Session["userbranchHierarchy"].ToString() + "'";//SDCode

            if (ddlGroupBy.SelectedItem.Value.ToString().Trim() == "Branch")/////group type branch selection
            {
                GrpType = "BRANCH";
                strParam[6, 0] = "GrpType"; strParam[6, 1] = "'BRANCH'";//SDCode
                if (RadioBtnOtherGroupByAll.Checked)
                {
                    GrpId = "ALL";
                    strParam[7, 0] = "GrpId"; strParam[7, 1] = "'ALL'";//SDCode
                }
                else
                {
                    GrpId = HiddenField_Branch.Value.ToString().Trim();
                    strParam[7, 0] = "GrpId"; strParam[7, 1] = "'" + Session["userbranchHierarchy"].ToString() + "'";//SDCode
                }
            }
            else if (ddlGroupBy.SelectedItem.Value.ToString().Trim() == "BranchGroup")/////group type branch-group selection
            {
                GrpType = "BRANCHGROUP";
                strParam[6, 0] = "GrpType"; strParam[6, 1] = "'BRANCHGROUP'";//SDCode
                if (RadioBtnOtherGroupByAll.Checked)
                {
                    GrpId = "ALL";
                    strParam[7, 0] = "GrpId"; strParam[7, 1] = "'ALL'";//SDCode
                }
                else
                {
                    GrpId = HiddenField_BranchGroup.Value.ToString().Trim();
                    strParam[7, 0] = "GrpId"; strParam[7, 1] = "'" + HiddenField_BranchGroup.Value.ToString().Trim() + "'";//SDCode
                }
            }
            else if (ddlGroupBy.SelectedItem.Value.ToString().Trim() == "Group")/////group type group selection
            {
                GrpType = ddlGroup.SelectedItem.Text.ToString().Trim();
                strParam[6, 0] = "GrpType"; strParam[6, 1] = "'BRANCHGROUP'";//SDCode
                if (RadioBtnGroupAll.Checked)
                {
                    GrpId = "All";
                    strParam[7, 0] = "GrpId"; strParam[7, 1] = "'ALL'";//SDCode
                }
                else
                {
                    GrpId = HiddenField_Group.Value.ToString().Trim();
                    strParam[7, 0] = "GrpId"; strParam[7, 1] = "'" + HiddenField_BranchGroup.Value.ToString().Trim() + "'";//SDCode
                }
            }
            else                                /////group type client selection
            {
                GrpType = "BRANCH";
                GrpId = "All";
                strParam[6, 0] = "GrpType"; strParam[6, 1] = "'BRANCH'";//SDCode
                strParam[7, 0] = "GrpId"; strParam[7, 1] = "'ALL'";//SDCode
            }

            if (ddlGroupBy.SelectedItem.Value.ToString().Trim() == "Clients")/////group type client selection
            {
                if (RadioBtnOtherGroupByAll.Checked)
                {
                    Clients = "All";
                    Broker = "CL";
                    strParam[8, 0] = "Clients"; strParam[8, 1] = "'All'";//SDCode
                    strParam[9, 0] = "Broker"; strParam[9, 1] = "'CL'";//SDCode
                }
                else
                {
                    Clients = HiddenField_Client.Value.ToString().Trim();
                    Broker = "CL";
                    strParam[8, 0] = "Clients"; strParam[8, 1] = "''" + HiddenField_Client.Value.ToString().Trim() + "''";//SDCode
                    strParam[9, 0] = "Broker"; strParam[9, 1] = "'CL'";//SDCode
                }
            }
            else if (ddlGroupBy.SelectedItem.Value.ToString().Trim() == "Broker")/////group type branch-group selection
            {

                if (RadioBtnOtherGroupByAll.Checked)
                {
                    Clients = "All";
                    Broker = "BO";
                    strParam[8, 0] = "Clients"; strParam[8, 1] = "'All'";//SDCode
                    strParam[9, 0] = "Broker"; strParam[9, 1] = "'BO'";//SDCode

                }
                else
                {
                    Clients = HiddenField_Broker.Value.ToString().Trim();
                    Broker = "NA";
                    strParam[8, 0] = "Clients"; strParam[8, 1] = "'" + HiddenField_Broker.Value.ToString().Trim() + "'";//SDCode
                    strParam[9, 0] = "Broker"; strParam[9, 1] = "'NA'";//SDCode
                }
            }
            else
            {
                Broker = "CL";
                Clients = "All";
                strParam[8, 0] = "Clients"; strParam[8, 1] = "'All'";//SDCode
                strParam[9, 0] = "Broker"; strParam[9, 1] = "'CL'";//SDCode
            }
            strParam[10, 0] = "groupbyvalue"; strParam[10, 1] = "'" + ddlGroupBy.SelectedItem.Value.ToString().Trim() + "'";//SDCode
            if (txtTradeCategory.Text.ToString().Trim() == "")
            {
                TradeCategory = "Only NULL";
                strParam[11, 0] = "TradeCategory"; strParam[11, 1] = "'Only NULL'";//SDCode
            }
            else if (txtTradeCategory.Text.ToString().Trim() == "???")
            {
                TradeCategory = "ALL";
                strParam[11, 0] = "TradeCategory"; strParam[11, 1] = "'ALL'";//SDCode
            }
            else
            {
                TradeCategory = txtTradeCategory.Text.ToString().Trim();
                strParam[11, 0] = "TradeCategory"; strParam[11, 1] = "'" + txtTradeCategory.Text.ToString().Trim() + "'";//SDCode
            }
            if (rdbTerminalAll.Checked)
            {
                Terminalid = "ALL";
                strParam[12, 0] = "Terminalid"; strParam[12, 1] = "'ALL'";//SDCode
            }
            else
            {
                Terminalid = txtTerminal_hidden.Text.ToString().Trim();
                strParam[12, 0] = "Terminalid"; strParam[12, 1] = "'" + txtTerminal_hidden.Text.ToString().Trim() + "'";//SDCode
            }

            if (rdbCTCLAll.Checked)
            {
                CtClid = "ALL";
                strParam[13, 0] = "CtClid"; strParam[13, 1] = "'ALL'";//SDCode
            }
            else
            {
                CtClid = txtCtCLID_hidden.Text.ToString().Trim();
                strParam[13, 0] = "CtClid"; strParam[13, 1] = "'" + txtCtCLID_hidden.Text.ToString().Trim() + "'";//SDCode
            }
            if (rdbAssets.Checked)
            {
                if (rdbunderlyingall.Checked)
                {
                    //==22-05-2014==Modified For Instrument type =============
                    if ((Session["ExchangeSegmentID"].ToString() == "2") || (Session["ExchangeSegmentID"].ToString() == "5") || (Session["ExchangeSegmentID"].ToString() == "20"))
                        Scrip = "All~" + strFOIdentifier;
                    else
                        Scrip = "All~";
                    strParam[14, 0] = "Scrip"; strParam[14, 1] = "'ALL~'";//SDCode
                }
                else
                {
                    //==22-05-2014==Modified For Instrument type =============
                    if ((Session["ExchangeSegmentID"].ToString() == "2") || (Session["ExchangeSegmentID"].ToString() == "5") || (Session["ExchangeSegmentID"].ToString() == "20"))
                        Scrip = HiddenField_Product.Value.ToString().Trim() + "~" + strFOIdentifier;
                    else
                        Scrip = HiddenField_Product.Value.ToString().Trim() + "~";
                    strParam[14, 0] = "Scrip"; strParam[14, 1] = "'" + HiddenField_Product.Value.ToString().Trim() + "~'";//SDCode
                }
            }
            if (rdbscrips.Checked)
            {
                if (rdInstrumentAll.Checked)
                {
                    //==22-05-2014==Modified For Instrument type =============
                    if ((Session["ExchangeSegmentID"].ToString() == "2") || (Session["ExchangeSegmentID"].ToString() == "5") || (Session["ExchangeSegmentID"].ToString() == "20"))
                        Scrip = "All~" + strFOIdentifier;
                    else
                        Scrip = "All~";
                    strParam[14, 0] = "Scrip"; strParam[14, 1] = "'ALL~'";//SDCode
                }
                else
                {
                    //==22-05-2014==Modified For Instrument type =============
                    if ((Session["ExchangeSegmentID"].ToString() == "2") || (Session["ExchangeSegmentID"].ToString() == "5") || (Session["ExchangeSegmentID"].ToString() == "20"))
                        Scrip = HiddenField_Instrument.Value.ToString().Trim() + "~" + strFOIdentifier;
                    else
                        Scrip = HiddenField_Instrument.Value.ToString().Trim() + "~";
                    strParam[14, 0] = "Scrip"; strParam[14, 1] = "'" + HiddenField_Instrument.Value.ToString().Trim() + "~'";//SDCode
                }
            }
            if (rdbSegAll.Checked)
            {
                Segment = "All";
                strParam[15, 0] = "Segment"; strParam[15, 1] = "'ALL'";//SDCode
            }
            else
            {
                Segment = HiddenField_Segment.Value.ToString().Trim();
                strParam[15, 0] = "Segment"; strParam[15, 1] = "'" + HiddenField_Segment.Value.ToString().Trim() + "'";//SDCode
            }
            if (rdbSettNoAll.Checked)
            {
                Settno = "ALL";
                strParam[16, 0] = "Settno"; strParam[16, 1] = "'ALL'";//SDCode
            }
            else
            {
                Settno = HiddenField_SettNo.Value.ToString().Trim();
                strParam[16, 0] = "Settno"; strParam[16, 1] = "'" + HiddenField_SettNo.Value.ToString().Trim() + "'";//SDCode
            }
            if (RadbSettlementTypeAll.Checked)
            {
                SettType = "ALL";
                strParam[17, 0] = "SettType"; strParam[17, 1] = "'ALL'";//SDCode
            }
            else
            {
                SettType = HiddenField_Setttype.Value.ToString().Trim();
                strParam[17, 0] = "SettType"; strParam[17, 1] = "''" + HiddenField_Setttype.Value.ToString().Trim() + "''";//SDCode
            }
            string TIME = "(CONVERT(VARCHAR(8), EXCHANGETRADES_TRADEENTRYTIME, 108) BETWEEN " + "'" + txtfromtime.Value.ToString().Trim() + "'" + " AND " + "'" + txttotime.Value.ToString().Trim() + "' OR EXCHANGETRADES_TRADEENTRYTIME IS NULL)";
            string TIMESDCode = "(CONVERT(VARCHAR(8), EXCHANGETRADES_TRADEENTRYTIME, 108) BETWEEN " + "''" + txtfromtime.Value.ToString().Trim() + "''" + " AND " + "''" + txttotime.Value.ToString().Trim() + "'' OR EXCHANGETRADES_TRADEENTRYTIME IS NULL)";
            strParam[18, 0] = "TradeTime"; strParam[18, 1] = "'" + TIMESDCode.ToString().Trim() + "'";//SDCode
            strParam[19, 0] = "Parameter"; strParam[19, 1] = "'" + parameter.ToString().Trim() + "'";//SDCode
            strParam[20, 0] = "OrderBy"; strParam[20, 1] = "'" + ddlOrderBy.SelectedItem.Value.ToString().Trim() + "'";//SDCode
            strParam[21, 0] = "SecurityType"; strParam[21, 1] = "'" + DdlSecurityType.SelectedItem.Value.ToString().Trim() + "'";//SDCode
            strParam[22, 0] = "Header"; strParam[22, 1] = "'" + txtHeader_hidden.Value + "'";//SDCode
            strParam[23, 0] = "Footer"; strParam[23, 1] = "'" + txtFooter_hidden.Value.ToString().Replace(",", "") + "'";//SDCode

            //For Server Debugging Purpose
            oGenericMethod = new GenericMethod();
            if (oGenericMethod.CallGeneric_ScalerFunction_Int("GetGlobalSettingsValue", Session["UserSegID"].ToString() + "~GS_DEBUGSTATE") == 1)
            {
                string strDateTime = oGenericMethod.GetDate().ToString("yyyyMMddHHmmss");
                string FilePath = "../ExportFiles/ServerDebugging/" + SpName + strDateTime + ".txt";
                oGenericMethod.WriteFile(oGenericMethod.OldSpExecuteWriter(strParam, SpName), FilePath, false);
            }
            ds = dailyrep.TradeRegister(Session["LastCompany"].ToString(), dtFrom.Value.ToString(), dtTo.Value.ToString(), ddlTradeType.SelectedItem.Value.ToString().Trim(), reportType,
                Session["userbranchHierarchy"].ToString(), GrpType, GrpId, Clients, Broker, ddlGroupBy.SelectedItem.Value.ToString().Trim(),
                TradeCategory, Terminalid, CtClid, Scrip, Segment, Settno, SettType, TIME.ToString().Trim(), parameter.ToString().Trim(),
                 ddlOrderBy.SelectedItem.Value.ToString().Trim(), DdlSecurityType.SelectedItem.Value.ToString().Trim(), txtHeader_hidden.Value, txtFooter_hidden.Value);
            ViewState["dataset"] = ds;
            FnGenerateType(ds);
        }

        void FnGenerateType(DataSet ds)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (reportType == "Screen")
                {
                    BindClient(ds);
                }
                else if (reportType == "Export")//for Excel Export
                {
                    Export(ds);
                }
                else if (reportType == "PDF")
                {
                    PdfPrint(ds);
                }
                else if (reportType == "Mail")
                {
                    Email(ds);
                }
                else if (reportType == "dos")
                {
                    dosprint(ds);
                }
                else if (reportType.Substring(0, 10) == "cntrscreen")
                {
                    DataSet dsScreen = new DataSet();
                    DataTable dtDisplay = new DataTable();
                    dtDisplay = ds.Tables[3].Copy();
                    dsScreen.Tables.Add(dtDisplay);
                    BindClient(dsScreen);

                    DataSet dsGridBind = new DataSet();
                    DataTable dtDisplayGrid = new DataTable();
                    dtDisplayGrid = ds.Tables[0].Copy();
                    dsGridBind.Tables.Add(dtDisplayGrid);

                    DataSet dsGridCalc = new DataSet();
                    DataTable dtCalcGridBind = new DataTable();
                    dtCalcGridBind = ds.Tables[1].Copy();
                    dsGridCalc.Tables.Add(dtCalcGridBind);
                    BindGrid(dsGridBind, dsGridCalc);
                }
            }
            else
            {
                if (Request.QueryString["Custid"] != null)//////////PopUp Display From Delivery Center
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordDisplay", "RecordDisplay('9');", true);

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordDisplay", "RecordDisplay('1');", true);
                }
            }
        }

        # region Pagination and Grid Row color
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

        protected void cmbrecord_SelectedIndexChanged(object sender, EventArgs e)
        {
            ds = (DataSet)ViewState["dataset"];
            FnHtml(ds, cmbrecord.SelectedItem.Value.ToString().Trim());
        }

        protected void NavigationLinkC_Click(Object sender, CommandEventArgs e)
        {
            hiddencount.Value = "0";
            int curentIndex = cmbrecord.SelectedIndex;
            int totalNo = cmbrecord.Items.Count;
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
            }
            else if (curentIndex <= 0)
            {
                curentIndex = 0;
            }
            cmbrecord.SelectedIndex = curentIndex;

            ds = (DataSet)ViewState["dataset"];
            FnHtml(ds, cmbrecord.SelectedItem.Value.ToString().Trim());
        }
        # endregion Pagination and Grid Row color

        # region Screen Format
        protected void BtnScreen_Click(object sender, EventArgs e)
        {
            reportType = "Screen";
            string strFO = cmbinstrutype.SelectedValue.ToString();
            FilterColumnCheck();
            Procedure(bindParams, reportType);
        }
        void BindClient(DataSet ds)
        {
            DataView viewData = new DataView();
            viewData = ds.Tables[0].DefaultView;
            viewData.RowFilter = " Customerid<>'ZZZZZZZZZZZ' and Customerid is not null";
            DataTable dt = new DataTable();
            dt = viewData.ToTable();
            DataTable Distinctclient = new DataTable();
            DataView viewClient = new DataView(dt);
            Distinctclient = viewClient.ToTable(true, new string[] { "CustomerId", "CustomerName" });
            if (Distinctclient.Rows.Count > 0)
            {
                cmbrecord.DataSource = Distinctclient;
                cmbrecord.DataValueField = "CustomerId";
                cmbrecord.DataTextField = "CustomerName";
                cmbrecord.DataBind();
                FnHtml(ds, cmbrecord.SelectedItem.Value.ToString().Trim());
            }
            else
            {
                if (Request.QueryString["Custid"] != null)//////////PopUp Display From Delivery Center
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordDisplay", "RecordDisplay('9');", true);

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordDisplay", "RecordDisplay('1');", true);
                }
            }
        }

        void BindGrid(DataSet dsGrid, DataSet dsCalc)
        {
            if (dsGrid.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsGrid.Tables[0].Rows.Count; i++)
                {
                    if (dsGrid.Tables[0].Rows[i]["ContractNotes_DelFutTO"] != DBNull.Value)
                        dsGrid.Tables[0].Rows[i]["ContractNotes_DelFutTO"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dsGrid.Tables[0].Rows[i]["ContractNotes_DelFutTO"].ToString()));

                    if (dsGrid.Tables[0].Rows[i]["ContractNotes_SqrOptPrmTO"] != DBNull.Value)
                        dsGrid.Tables[0].Rows[i]["ContractNotes_SqrOptPrmTO"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dsGrid.Tables[0].Rows[i]["ContractNotes_SqrOptPrmTO"].ToString()));

                    if (dsGrid.Tables[0].Rows[i]["ContractNotes_TotalTO"] != DBNull.Value)
                        dsGrid.Tables[0].Rows[i]["ContractNotes_TotalTO"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dsGrid.Tables[0].Rows[i]["ContractNotes_TotalTO"].ToString()));

                    if (dsGrid.Tables[0].Rows[i]["ContractNotes_TotalBrokerage"] != DBNull.Value)
                        dsGrid.Tables[0].Rows[i]["ContractNotes_TotalBrokerage"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dsGrid.Tables[0].Rows[i]["ContractNotes_TotalBrokerage"].ToString()));

                    if (dsGrid.Tables[0].Rows[i]["ContractNotes_TransactionCharges"] != DBNull.Value)
                        dsGrid.Tables[0].Rows[i]["ContractNotes_TransactionCharges"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dsGrid.Tables[0].Rows[i]["ContractNotes_TransactionCharges"].ToString()));

                    if (dsGrid.Tables[0].Rows[i]["ContractNotes_StampDuty"] != DBNull.Value)
                        dsGrid.Tables[0].Rows[i]["ContractNotes_StampDuty"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dsGrid.Tables[0].Rows[i]["ContractNotes_StampDuty"].ToString()));

                    if (dsGrid.Tables[0].Rows[i]["TotalTax"] != DBNull.Value)
                        dsGrid.Tables[0].Rows[i]["TotalTax"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dsGrid.Tables[0].Rows[i]["TotalTax"].ToString()));

                    if (dsGrid.Tables[0].Rows[i]["ContractNotes_NetAmount"] != DBNull.Value)
                        dsGrid.Tables[0].Rows[i]["ContractNotes_NetAmount"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dsGrid.Tables[0].Rows[i]["ContractNotes_NetAmount"].ToString()));

                    if (dsGrid.Tables[0].Rows[i]["ContractNotes_STTAmount"] != DBNull.Value)
                        dsGrid.Tables[0].Rows[i]["ContractNotes_STTAmount"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dsGrid.Tables[0].Rows[i]["ContractNotes_STTAmount"].ToString()));

                }
                ViewState["del"] = Convert.ToDecimal(dsCalc.Tables[0].Rows[0]["del"]);

                ViewState["sqr"] = Convert.ToDecimal(dsCalc.Tables[0].Rows[0]["sqr"]);

                ViewState["totalto"] = Convert.ToDecimal(dsCalc.Tables[0].Rows[0]["totalto"]);

                ViewState["totalbrkg"] = Convert.ToDecimal(dsCalc.Tables[0].Rows[0]["totalbrkg"]);

                ViewState["trancharge"] = Convert.ToDecimal(dsCalc.Tables[0].Rows[0]["trancharge"]);

                ViewState["stamp"] = Convert.ToDecimal(dsCalc.Tables[0].Rows[0]["stamp"]);

                ViewState["Total"] = Convert.ToDecimal(dsCalc.Tables[0].Rows[0]["Total"]);

                ViewState["netamount"] = Convert.ToDecimal(dsCalc.Tables[0].Rows[0]["netamount"]);

                ViewState["STTAmount"] = Convert.ToDecimal(dsCalc.Tables[0].Rows[0]["STTAmount"]);

                ViewState["Sebifee"] = Convert.ToDecimal(dsCalc.Tables[0].Rows[0]["SEBIFee"]);
                ViewState["Deliverycharge"] = Convert.ToDecimal(dsCalc.Tables[0].Rows[0]["DeliveryCharges"]);
                ViewState["Othercharge"] = Convert.ToDecimal(dsCalc.Tables[0].Rows[0]["OtherCharges"]);
                ViewState["RoundAmount"] = Convert.ToDecimal(dsCalc.Tables[0].Rows[0]["RoundAmount"]);

                /////////////////////////////DISPLAY DATE/////////////////////////////////////

                //string SpanText = oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "  - " + oconverter.ArrangeDate2(dtTo.Value.ToString());
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct", "displaydate('" + SpanText + "')", true);
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "allselected", "ALLSELECTED()();", true);

                ///////////////////////////////END//////////////////////////////////////////



                DataView dv2 = new DataView(dsGrid.Tables[0]);

                //dv2.Sort = sortExpression + direction;
                grdContractRegister.DataSource = dv2;
                grdContractRegister.DataBind();


                // ScriptManager.RegisterStartupScript(this, this.GetType(), "JSgriddiv", "Setdivwd();", true);
                //divgrid.Attributes.Add("style", "width: " + hidsGridcreenWd.Value + "px; overflow:scroll");


            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "hi", "NoRecord()", true);
            }

        }

        void FnHtml(DataSet ds, string parameter)
        {
            //////////For header
            String strHtmlheader = String.Empty;
            string str = null;

            if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "Screen")
            {
                str = ddlGroupBy.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report Period " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtTo.Value.ToString());

                strHtmlheader = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                strHtmlheader += "<tr><td align=\"left\" colspan=" + ds.Tables[0].Columns.Count + " style=\"color:Blue;\">" + str + "</td></tr></table>";

            }
            ////////Detail Display
            String strHtml = String.Empty;
            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            DataView viewclient = new DataView();
            viewclient = ds.Tables[0].DefaultView;
            viewclient.RowFilter = "(CustomerId='" + parameter.ToString().Trim() + "' and CustomerId is not null and Customerid<>'ZZZZZZZZZZZ')";
            DataTable dt = new DataTable();
            dt = viewclient.ToTable();

            dt.Columns.Remove("CustomerId");
            dt.Columns.Remove("CustomerName");
            dt.Columns.Remove("Grpid");
            dt.Columns.Remove("GrpEmail");

            if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "Mail")
                dt.Columns.Remove("ExchTrade");
            //dt.AcceptChanges();

            if (ddlGeneration.SelectedItem.Value.ToString().Trim() != "Screen")
            {
                strHtml += "<tr style=\"background-color:lavender ;text-align:left\">";
                strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; ><b>" + dt.Rows[0][0].ToString().Trim() + "</b></td>";
                strHtml += "</tr>";

            }
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

                if (ChkFilterDetail.Items[2].Selected)
                {
                    if (dr1["Order"] != DBNull.Value)
                        dr1["Order"] = dr1["Order"].ToString().Substring(1, dr1["Order"].ToString().Length - 1);
                }

                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (dr1[j] != DBNull.Value)
                    {
                        if (dr1[j].ToString().Trim().StartsWith("Total:"))
                        {
                            strHtml += "</tr>";
                            strHtml += "<tr style=\"background-color:lavender ;text-align:left\">";
                            strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\"><b>" + dr1[j] + "</b></td>";
                        }
                        else if (dr1[j].ToString().Trim().StartsWith("Test"))
                        {
                            strHtml += "<td>&nbsp;</td>";
                        }
                        else if (dt.Columns[j].ToString() == "ExchTrade")
                        {
                            strHtml += "<td><a href=\"javascript:void(0);\" onclick=\"fn_AverageView('" + dr1[j] + "' )\" title=\"View Exchange Trade Detail\"><b style=\"color:#333;\">View</b></a></td>";
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
            if (strHtml.Contains("ZZZZZZZZZZZ"))
                strHtml = strHtml.Replace("ZZZZZZZZZZZ", "&nbsp;");

            if (Request.QueryString["Custid"] != null)//////////PopUp Display From Delivery Center
            {
                Divdisplay.InnerHtml = strHtml;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordDisplay1", "RecordDisplay('8');", true);

            }
            else
            {
                if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "Screen")
                {
                    DivHeader.InnerHtml = strHtmlheader;
                    Divdisplay.InnerHtml = strHtml;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordDisplay", "RecordDisplay('3');", true);

                }
                else
                {
                    ViewState["mail"] = strHtmlheader + strHtml;
                }
            }
        }
        # endregion

        # region Excel Export Format
        protected void BtnExport_Click(object sender, EventArgs e)
        {
            if (ddlExportType.SelectedItem.Text == "Excel")
                reportType = "Export";
            else
                reportType = "PDF";
            FilterColumnCheck();
            Procedure(bindParams, reportType);
        }
        void Export(DataSet ds)
        {
            DataTable dtExport = ds.Tables[0].Copy();
            if (dtExport.Rows.Count > 1)
            {
                if (!chkrawprint.Checked)
                {
                    dtExport.Columns.Remove("CustomerName");
                }
                dtExport.AcceptChanges();
                //*******Start of Modify in 24-07-2012***************//
                GenericExcelExport oGenericExcelExport = new GenericExcelExport();
                string strDownloadFileName = "";
                string exlDateTime = oDBEngine.GetDate(113).ToString();
                string exlTime = exlDateTime.Replace(":", "");
                exlTime = exlTime.Replace(" ", "");
                string FileName = "TradeRegister_" + exlTime;
                strDownloadFileName = "~/Documents/";
                string[] strHead = new string[3];
                string searchCriteria = null;
                searchCriteria = ddlGroupBy.SelectedItem.Text.ToString().Trim() + " Wise Trade Register Period " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtTo.Value.ToString());
                if (RadioBtnOtherGroupByAll.Checked == true)
                    searchCriteria += " With All " + ddlGroupBy.SelectedItem.Text.ToString().Trim();
                else
                    searchCriteria += " With Selected " + ddlGroupBy.SelectedItem.Text.ToString().Trim();
                if (rdbAssets.Checked == true)
                {
                    if (rdbunderlyingall.Checked == true)
                        searchCriteria += " , All Product";
                    else
                        searchCriteria += " , Selected Product";
                }
                else if (rdbscrips.Checked == true)
                {
                    if (rdInstrumentAll.Checked == true)

                        searchCriteria += " , All Instrument";
                    else
                        searchCriteria += " , Selected Instrument";
                }
                if (rdbSegAll.Checked == true)
                    searchCriteria += " , All Segment";
                else
                    searchCriteria += " , Selected Segment";
                if (DdlSecurityType.SelectedValue != null)
                    searchCriteria += " And " + DdlSecurityType.SelectedItem.Text.ToString().Trim() + " Security Type";
                if (ddlOrderBy.SelectedValue != null)
                    searchCriteria += " With Order By " + ddlOrderBy.SelectedItem.Text.ToString().Trim();
                strHead[0] = exlDateTime;
                strHead[1] = searchCriteria;
                strHead[2] = "Trade Register Of " + ViewState["CompanyName"].ToString();
                string ExcelVersion = "2007";

                string[] filterColumns = bindParams.Split('~');
                string SelectTerminalid = filterColumns[0];
                string SelectTradeCode = filterColumns[1];
                string SelectOrderNo = filterColumns[2];
                string SelectOrderEntryTime = filterColumns[3];
                string SelectTradeNo = filterColumns[4];
                string SelectTradeEntryTime = filterColumns[5];
                string SelectCntrNo = filterColumns[6];

                if (rdbSegAll.Checked)
                {
                    if (!(rdbSettNoAll.Checked) && !(RadbSettlementTypeAll.Checked))
                    {
                        if (chkrawprint.Checked)//1 c    1.CustomerName,2.TradeDate,3.Scrip,4.Segmnt,5.Terminal,6.TrdCode,7.Order,8.OrderTime, same in all cond.
                        {
                            string[] ColumnType = { "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "I", "I", "I", "N", "V", "N", "N", "N", "N", "N", "V", "N" };
                            string[] ColumnSize = { "40", "40", "40", "40", "20", "20", "30", "20", "20", "20", "16", "16", "16", "18,4", "15", "18,4", "18,2", "18,4", "18,2", "18,2", "6", "18,2" };
                            string[] ColumnWidthSize = { "30", "10", "30", "8", "10", "6", "16", "8", "10", "8", "6", "10", "10", "8", "4", "8", "10", "10", "12", "8", "6", "12" };

                            ArrayList ColumnTypeList = new ArrayList(ColumnType);
                            ArrayList ColumnSizeList = new ArrayList(ColumnSize);
                            ArrayList ColumnWidthSizeList = new ArrayList(ColumnWidthSize);

                            int removeCounter = 0;

                            if (SelectTerminalid == "N")
                            {
                                ColumnTypeList.RemoveAt(4);
                                ColumnSizeList.RemoveAt(4);
                                ColumnWidthSizeList.RemoveAt(4);
                                removeCounter = 1;
                            }
                            if (SelectTradeCode == "N")
                            {
                                ColumnTypeList.RemoveAt(5 - removeCounter);
                                ColumnSizeList.RemoveAt(5 - removeCounter);
                                ColumnWidthSizeList.RemoveAt(5 - removeCounter);
                                removeCounter = removeCounter + 1;
                            }
                            if (SelectOrderNo == "N")
                            {
                                ColumnTypeList.RemoveAt(6 - removeCounter);
                                ColumnSizeList.RemoveAt(6 - removeCounter);
                                ColumnWidthSizeList.RemoveAt(6 - removeCounter);
                                removeCounter = removeCounter + 1;
                            }
                            if (SelectOrderEntryTime == "N")
                            {
                                ColumnTypeList.RemoveAt(7 - removeCounter);
                                ColumnSizeList.RemoveAt(7 - removeCounter);
                                ColumnWidthSizeList.RemoveAt(7 - removeCounter);
                                removeCounter = removeCounter + 1;
                            }
                            if (SelectTradeNo == "N")
                            {
                                ColumnTypeList.RemoveAt(8 - removeCounter);
                                ColumnSizeList.RemoveAt(8 - removeCounter);
                                ColumnWidthSizeList.RemoveAt(8 - removeCounter);
                                removeCounter = removeCounter + 1;
                            }
                            if (SelectTradeEntryTime == "N")
                            {
                                ColumnTypeList.RemoveAt(9 - removeCounter);
                                ColumnSizeList.RemoveAt(9 - removeCounter);
                                ColumnWidthSizeList.RemoveAt(9 - removeCounter);
                                removeCounter = removeCounter + 1;
                            }
                            if (SelectCntrNo == "N")
                            {
                                ColumnTypeList.RemoveAt(10 - removeCounter);
                                ColumnSizeList.RemoveAt(10 - removeCounter);
                                ColumnWidthSizeList.RemoveAt(10 - removeCounter);
                                removeCounter = removeCounter + 1;
                            }

                            ColumnType = (string[])ColumnTypeList.ToArray(typeof(string));
                            ColumnSize = (string[])ColumnSizeList.ToArray(typeof(string));
                            ColumnWidthSize = (string[])ColumnWidthSizeList.ToArray(typeof(string));

                            oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, dtExport, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                        }
                        else//2  c                       1.TradeDate,2.Scrip,3.Segmnt,4.Terminal,5.TrdCode,6.Order,7.OrderTime, same in all cond.
                        {
                            string[] ColumnType = { "V", "V", "V", "V", "V", "V", "V", "V", "V", "I", "I", "I", "N", "V", "N", "N", "N", "N", "N", "V", "N" };
                            string[] ColumnSize = { "40", "40", "40", "20", "20", "30", "20", "20", "20", "16", "16", "16", "18,4", "15", "18,4", "18,2", "18,4", "18,2", "18,2", "6", "18,2" };
                            string[] ColumnWidthSize = { "10", "30", "8", "10", "6", "16", "8", "10", "8", "6", "10", "10", "8", "4", "8", "10", "10", "12", "8", "6", "12" };


                            ArrayList ColumnTypeList = new ArrayList(ColumnType);
                            ArrayList ColumnSizeList = new ArrayList(ColumnSize);
                            ArrayList ColumnWidthSizeList = new ArrayList(ColumnWidthSize);

                            int removeCounter = 0;

                            if (SelectTerminalid == "N")
                            {
                                ColumnTypeList.RemoveAt(3);
                                ColumnSizeList.RemoveAt(3);
                                ColumnWidthSizeList.RemoveAt(3);
                                removeCounter = 1;
                            }
                            if (SelectTradeCode == "N")
                            {
                                ColumnTypeList.RemoveAt(4 - removeCounter);
                                ColumnSizeList.RemoveAt(4 - removeCounter);
                                ColumnWidthSizeList.RemoveAt(4 - removeCounter);
                                removeCounter = removeCounter + 1;
                            }
                            if (SelectOrderNo == "N")
                            {
                                ColumnTypeList.RemoveAt(5 - removeCounter);
                                ColumnSizeList.RemoveAt(5 - removeCounter);
                                ColumnWidthSizeList.RemoveAt(5 - removeCounter);
                                removeCounter = removeCounter + 1;
                            }
                            if (SelectOrderEntryTime == "N")
                            {
                                ColumnTypeList.RemoveAt(6 - removeCounter);
                                ColumnSizeList.RemoveAt(6 - removeCounter);
                                ColumnWidthSizeList.RemoveAt(6 - removeCounter);
                                removeCounter = removeCounter + 1;
                            }
                            if (SelectTradeNo == "N")
                            {
                                ColumnTypeList.RemoveAt(7 - removeCounter);
                                ColumnSizeList.RemoveAt(7 - removeCounter);
                                ColumnWidthSizeList.RemoveAt(7 - removeCounter);
                                removeCounter = removeCounter + 1;
                            }
                            if (SelectTradeEntryTime == "N")
                            {
                                ColumnTypeList.RemoveAt(8 - removeCounter);
                                ColumnSizeList.RemoveAt(8 - removeCounter);
                                ColumnWidthSizeList.RemoveAt(8 - removeCounter);
                                removeCounter = removeCounter + 1;
                            }
                            if (SelectCntrNo == "N")
                            {
                                ColumnTypeList.RemoveAt(9 - removeCounter);
                                ColumnSizeList.RemoveAt(9 - removeCounter);
                                ColumnWidthSizeList.RemoveAt(9 - removeCounter);
                                removeCounter = removeCounter + 1;
                            }

                            ColumnType = (string[])ColumnTypeList.ToArray(typeof(string));
                            ColumnSize = (string[])ColumnSizeList.ToArray(typeof(string));
                            ColumnWidthSize = (string[])ColumnWidthSizeList.ToArray(typeof(string));

                            oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, dtExport, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                        }
                    }
                    else
                    {
                        if (chkrawprint.Checked)//3c   1.CustomerName,2.TradeDate,3.Scrip,4.Segmnt,5.Sett,6.Terminal,7.TrdCode,8.Order,9.OrderTime,10.TradeNo,11.TradeTime,12.CntrNo.,13.BuyQty,14.SaleQty,15.MktPrice,16.Type,17.Brkg,18.TotBrkg,19.NetRate,20.NetValue,21.SrvTax,22.Mode,23.NetAmnt
                        {                                 //1    2    3    4    5            6     7    8    9   10   11   12    13   14   15   16   17    18   19  20   21   22   23
                            string[] ColumnType = { "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "I", "I", "I", "N", "V", "N", "N", "N", "N", "N", "V", "N" };
                            string[] ColumnSize = { "40", "40", "40", "40", "20", "20", "20", "20", "20", "20", "20", "16", "16", "16", "18,4", "15", "18,4", "18,2", "18,4", "18,2", "18,2", "6", "18,2" };
                            string[] ColumnWidthSize = { "30", "10", "25", "8", "8", "10", "6", "16", "8", "10", "8", "6", "10", "10", "8", "4", "8", "10", "10", "12", "8", "6", "12" };


                            ArrayList ColumnTypeList = new ArrayList(ColumnType);
                            ArrayList ColumnSizeList = new ArrayList(ColumnSize);
                            ArrayList ColumnWidthSizeList = new ArrayList(ColumnWidthSize);

                            int removeCounter = 0;

                            if (SelectTerminalid == "N")
                            {
                                ColumnTypeList.RemoveAt(5);
                                ColumnSizeList.RemoveAt(5);
                                ColumnWidthSizeList.RemoveAt(5);
                                removeCounter = 1;
                            }
                            if (SelectTradeCode == "N")
                            {
                                ColumnTypeList.RemoveAt(6 - removeCounter);
                                ColumnSizeList.RemoveAt(6 - removeCounter);
                                ColumnWidthSizeList.RemoveAt(6 - removeCounter);
                                removeCounter = removeCounter + 1;
                            }
                            if (SelectOrderNo == "N")
                            {
                                ColumnTypeList.RemoveAt(7 - removeCounter);
                                ColumnSizeList.RemoveAt(7 - removeCounter);
                                ColumnWidthSizeList.RemoveAt(7 - removeCounter);
                                removeCounter = removeCounter + 1;
                            }
                            if (SelectOrderEntryTime == "N")
                            {
                                ColumnTypeList.RemoveAt(8 - removeCounter);
                                ColumnSizeList.RemoveAt(8 - removeCounter);
                                ColumnWidthSizeList.RemoveAt(8 - removeCounter);
                                removeCounter = removeCounter + 1;
                            }
                            if (SelectTradeNo == "N")
                            {
                                ColumnTypeList.RemoveAt(9 - removeCounter);
                                ColumnSizeList.RemoveAt(9 - removeCounter);
                                ColumnWidthSizeList.RemoveAt(9 - removeCounter);
                                removeCounter = removeCounter + 1;
                            }
                            if (SelectTradeEntryTime == "N")
                            {
                                ColumnTypeList.RemoveAt(10 - removeCounter);
                                ColumnSizeList.RemoveAt(10 - removeCounter);
                                ColumnWidthSizeList.RemoveAt(10 - removeCounter);
                                removeCounter = removeCounter + 1;
                            }
                            if (SelectCntrNo == "N")
                            {
                                ColumnTypeList.RemoveAt(11 - removeCounter);
                                ColumnSizeList.RemoveAt(11 - removeCounter);
                                ColumnWidthSizeList.RemoveAt(11 - removeCounter);
                                removeCounter = removeCounter + 1;
                            }

                            ColumnType = (string[])ColumnTypeList.ToArray(typeof(string));
                            ColumnSize = (string[])ColumnSizeList.ToArray(typeof(string));
                            ColumnWidthSize = (string[])ColumnWidthSizeList.ToArray(typeof(string));

                            oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, dtExport, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                        }
                        else//4 c                         1.TradeDate,2.Scrip,3.Segmnt,4.Sett,5.Terminal,6.TrdCode,7.Order,8.OrderTime, same in all cond.
                        {
                            string[] ColumnType = { "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "I", "I", "I", "N", "V", "N", "N", "N", "N", "N", "V", "N" };
                            string[] ColumnSize = { "40", "40", "40", "20", "20", "20", "30", "20", "20", "20", "16", "16", "16", "18,4", "15", "18,4", "18,2", "18,4", "18,2", "18,2", "6", "18,2" };
                            string[] ColumnWidthSize = { "10", "25", "8", "8", "10", "6", "16", "8", "10", "8", "6", "10", "10", "8", "4", "8", "10", "10", "12", "8", "6", "12" };

                            ArrayList ColumnTypeList = new ArrayList(ColumnType);
                            ArrayList ColumnSizeList = new ArrayList(ColumnSize);
                            ArrayList ColumnWidthSizeList = new ArrayList(ColumnWidthSize);

                            int removeCounter = 0;

                            if (SelectTerminalid == "N")
                            {
                                ColumnTypeList.RemoveAt(4);
                                ColumnSizeList.RemoveAt(4);
                                ColumnWidthSizeList.RemoveAt(4);
                                removeCounter = 1;
                            }
                            if (SelectTradeCode == "N")
                            {
                                ColumnTypeList.RemoveAt(5 - removeCounter);
                                ColumnSizeList.RemoveAt(5 - removeCounter);
                                ColumnWidthSizeList.RemoveAt(5 - removeCounter);
                                removeCounter = removeCounter + 1;
                            }
                            if (SelectOrderNo == "N")
                            {
                                ColumnTypeList.RemoveAt(6 - removeCounter);
                                ColumnSizeList.RemoveAt(6 - removeCounter);
                                ColumnWidthSizeList.RemoveAt(6 - removeCounter);
                                removeCounter = removeCounter + 1;
                            }
                            if (SelectOrderEntryTime == "N")
                            {
                                ColumnTypeList.RemoveAt(7 - removeCounter);
                                ColumnSizeList.RemoveAt(7 - removeCounter);
                                ColumnWidthSizeList.RemoveAt(7 - removeCounter);
                                removeCounter = removeCounter + 1;
                            }
                            if (SelectTradeNo == "N")
                            {
                                ColumnTypeList.RemoveAt(8 - removeCounter);
                                ColumnSizeList.RemoveAt(8 - removeCounter);
                                ColumnWidthSizeList.RemoveAt(8 - removeCounter);
                                removeCounter = removeCounter + 1;
                            }
                            if (SelectTradeEntryTime == "N")
                            {
                                ColumnTypeList.RemoveAt(9 - removeCounter);
                                ColumnSizeList.RemoveAt(9 - removeCounter);
                                ColumnWidthSizeList.RemoveAt(9 - removeCounter);
                                removeCounter = removeCounter + 1;
                            }
                            if (SelectCntrNo == "N")
                            {
                                ColumnTypeList.RemoveAt(10 - removeCounter);
                                ColumnSizeList.RemoveAt(10 - removeCounter);
                                ColumnWidthSizeList.RemoveAt(10 - removeCounter);
                                removeCounter = removeCounter + 1;
                            }

                            ColumnType = (string[])ColumnTypeList.ToArray(typeof(string));
                            ColumnSize = (string[])ColumnSizeList.ToArray(typeof(string));
                            ColumnWidthSize = (string[])ColumnWidthSizeList.ToArray(typeof(string));

                            oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, dtExport, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                        }
                    }
                }
                else //segment=selected 
                {
                    if (!(rdbSettNoAll.Checked) && !(RadbSettlementTypeAll.Checked))
                    {
                        if (chkrawprint.Checked)//5c    1.CustomerName,2.TradeDate,3.Scrip,4.Terminal,5.TrdCode,6.Order,7.OrderTime, same in all cond.
                        {
                            string[] ColumnType = { "V", "V", "V", "V", "V", "V", "V", "V", "V", "I", "I", "I", "N", "V", "N", "N", "N", "N", "N", "V", "N" };
                            string[] ColumnSize = { "40", "40", "40", "20", "20", "30", "20", "20", "20", "16", "16", "16", "18,4", "15", "18,4", "18,2", "18,4", "18,2", "18,2", "6", "18,2" };
                            string[] ColumnWidthSize = { "30", "10", "30", "10", "6", "16", "8", "10", "8", "6", "10", "10", "8", "4", "8", "10", "10", "12", "8", "6", "12" };


                            ArrayList ColumnTypeList = new ArrayList(ColumnType);
                            ArrayList ColumnSizeList = new ArrayList(ColumnSize);
                            ArrayList ColumnWidthSizeList = new ArrayList(ColumnWidthSize);

                            int removeCounter = 0;

                            if (SelectTerminalid == "N")
                            {
                                ColumnTypeList.RemoveAt(3);
                                ColumnSizeList.RemoveAt(3);
                                ColumnWidthSizeList.RemoveAt(3);
                                removeCounter = 1;
                            }
                            if (SelectTradeCode == "N")
                            {
                                ColumnTypeList.RemoveAt(4 - removeCounter);
                                ColumnSizeList.RemoveAt(4 - removeCounter);
                                ColumnWidthSizeList.RemoveAt(4 - removeCounter);
                                removeCounter = removeCounter + 1;
                            }
                            if (SelectOrderNo == "N")
                            {
                                ColumnTypeList.RemoveAt(5 - removeCounter);
                                ColumnSizeList.RemoveAt(5 - removeCounter);
                                ColumnWidthSizeList.RemoveAt(5 - removeCounter);
                                removeCounter = removeCounter + 1;
                            }
                            if (SelectOrderEntryTime == "N")
                            {
                                ColumnTypeList.RemoveAt(6 - removeCounter);
                                ColumnSizeList.RemoveAt(6 - removeCounter);
                                ColumnWidthSizeList.RemoveAt(6 - removeCounter);
                                removeCounter = removeCounter + 1;
                            }
                            if (SelectTradeNo == "N")
                            {
                                ColumnTypeList.RemoveAt(7 - removeCounter);
                                ColumnSizeList.RemoveAt(7 - removeCounter);
                                ColumnWidthSizeList.RemoveAt(7 - removeCounter);
                                removeCounter = removeCounter + 1;
                            }
                            if (SelectTradeEntryTime == "N")
                            {
                                ColumnTypeList.RemoveAt(8 - removeCounter);
                                ColumnSizeList.RemoveAt(8 - removeCounter);
                                ColumnWidthSizeList.RemoveAt(8 - removeCounter);
                                removeCounter = removeCounter + 1;
                            }
                            if (SelectCntrNo == "N")
                            {
                                ColumnTypeList.RemoveAt(9 - removeCounter);
                                ColumnSizeList.RemoveAt(9 - removeCounter);
                                ColumnWidthSizeList.RemoveAt(9 - removeCounter);
                                removeCounter = removeCounter + 1;
                            }

                            ColumnType = (string[])ColumnTypeList.ToArray(typeof(string));
                            ColumnSize = (string[])ColumnSizeList.ToArray(typeof(string));
                            ColumnWidthSize = (string[])ColumnWidthSizeList.ToArray(typeof(string));

                            oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, dtExport, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                        }
                        else//6 c                         1.TradeDate,2.Scrip,3.Terminal,4.TrdCode,5.Order,6.OrderTime, same in all cond.  
                        {
                            string[] ColumnType = { "V", "V", "V", "V", "V", "V", "V", "V", "I", "I", "I", "N", "V", "N", "N", "N", "N", "N", "V", "N" };
                            string[] ColumnSize = { "40", "40", "20", "20", "30", "20", "20", "20", "16", "16", "16", "18,4", "15", "18,4", "18,2", "18,4", "18,2", "18,2", "6", "18,2" };
                            string[] ColumnWidthSize = { "10", "30", "10", "6", "16", "8", "10", "8", "6", "10", "10", "8", "4", "8", "10", "10", "12", "8", "6", "12" };


                            ArrayList ColumnTypeList = new ArrayList(ColumnType);
                            ArrayList ColumnSizeList = new ArrayList(ColumnSize);
                            ArrayList ColumnWidthSizeList = new ArrayList(ColumnWidthSize);

                            int removeCounter = 0;

                            if (SelectTerminalid == "N")
                            {
                                ColumnTypeList.RemoveAt(2);
                                ColumnSizeList.RemoveAt(2);
                                ColumnWidthSizeList.RemoveAt(2);
                                removeCounter = 1;
                            }
                            if (SelectTradeCode == "N")
                            {
                                ColumnTypeList.RemoveAt(3 - removeCounter);
                                ColumnSizeList.RemoveAt(3 - removeCounter);
                                ColumnWidthSizeList.RemoveAt(3 - removeCounter);
                                removeCounter = removeCounter + 1;
                            }
                            if (SelectOrderNo == "N")
                            {
                                ColumnTypeList.RemoveAt(4 - removeCounter);
                                ColumnSizeList.RemoveAt(4 - removeCounter);
                                ColumnWidthSizeList.RemoveAt(4 - removeCounter);
                                removeCounter = removeCounter + 1;
                            }
                            if (SelectOrderEntryTime == "N")
                            {
                                ColumnTypeList.RemoveAt(5 - removeCounter);
                                ColumnSizeList.RemoveAt(5 - removeCounter);
                                ColumnWidthSizeList.RemoveAt(5 - removeCounter);
                                removeCounter = removeCounter + 1;
                            }
                            if (SelectTradeNo == "N")
                            {
                                ColumnTypeList.RemoveAt(6 - removeCounter);
                                ColumnSizeList.RemoveAt(6 - removeCounter);
                                ColumnWidthSizeList.RemoveAt(6 - removeCounter);
                                removeCounter = removeCounter + 1;
                            }
                            if (SelectTradeEntryTime == "N")
                            {
                                ColumnTypeList.RemoveAt(7 - removeCounter);
                                ColumnSizeList.RemoveAt(7 - removeCounter);
                                ColumnWidthSizeList.RemoveAt(7 - removeCounter);
                                removeCounter = removeCounter + 1;
                            }
                            if (SelectCntrNo == "N")
                            {
                                ColumnTypeList.RemoveAt(8 - removeCounter);
                                ColumnSizeList.RemoveAt(8 - removeCounter);
                                ColumnWidthSizeList.RemoveAt(8 - removeCounter);
                                removeCounter = removeCounter + 1;
                            }

                            ColumnType = (string[])ColumnTypeList.ToArray(typeof(string));
                            ColumnSize = (string[])ColumnSizeList.ToArray(typeof(string));
                            ColumnWidthSize = (string[])ColumnWidthSizeList.ToArray(typeof(string));

                            oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, dtExport, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                        }
                    }
                    else
                    {
                        if (chkrawprint.Checked)//7 c       1.CustomerName,2.TradeDate,3.Scrip,4.Sett,5.Terminal,6.TrdCode,7.Order,8.OrderTime, same in all cond.
                        {
                            string[] ColumnType = { "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "I", "I", "I", "N", "V", "N", "N", "N", "N", "N", "V", "N" };
                            string[] ColumnSize = { "40", "40", "40", "20", "20", "20", "30", "20", "20", "20", "16", "16", "16", "18,4", "15", "18,4", "18,2", "18,4", "18,2", "18,2", "6", "18,2" };
                            string[] ColumnWidthSize = { "30", "10", "30", "8", "10", "6", "16", "8", "10", "8", "6", "10", "10", "8", "4", "8", "10", "10", "12", "8", "6", "12" };


                            ArrayList ColumnTypeList = new ArrayList(ColumnType);
                            ArrayList ColumnSizeList = new ArrayList(ColumnSize);
                            ArrayList ColumnWidthSizeList = new ArrayList(ColumnWidthSize);

                            int removeCounter = 0;

                            if (SelectTerminalid == "N")
                            {
                                ColumnTypeList.RemoveAt(4);
                                ColumnSizeList.RemoveAt(4);
                                ColumnWidthSizeList.RemoveAt(4);
                                removeCounter = 1;
                            }
                            if (SelectTradeCode == "N")
                            {
                                ColumnTypeList.RemoveAt(5 - removeCounter);
                                ColumnSizeList.RemoveAt(5 - removeCounter);
                                ColumnWidthSizeList.RemoveAt(5 - removeCounter);
                                removeCounter = removeCounter + 1;
                            }
                            if (SelectOrderNo == "N")
                            {
                                ColumnTypeList.RemoveAt(6 - removeCounter);
                                ColumnSizeList.RemoveAt(6 - removeCounter);
                                ColumnWidthSizeList.RemoveAt(6 - removeCounter);
                                removeCounter = removeCounter + 1;
                            }
                            if (SelectOrderEntryTime == "N")
                            {
                                ColumnTypeList.RemoveAt(7 - removeCounter);
                                ColumnSizeList.RemoveAt(7 - removeCounter);
                                ColumnWidthSizeList.RemoveAt(7 - removeCounter);
                                removeCounter = removeCounter + 1;
                            }
                            if (SelectTradeNo == "N")
                            {
                                ColumnTypeList.RemoveAt(8 - removeCounter);
                                ColumnSizeList.RemoveAt(8 - removeCounter);
                                ColumnWidthSizeList.RemoveAt(8 - removeCounter);
                                removeCounter = removeCounter + 1;
                            }
                            if (SelectTradeEntryTime == "N")
                            {
                                ColumnTypeList.RemoveAt(9 - removeCounter);
                                ColumnSizeList.RemoveAt(9 - removeCounter);
                                ColumnWidthSizeList.RemoveAt(9 - removeCounter);
                                removeCounter = removeCounter + 1;
                            }
                            if (SelectCntrNo == "N")
                            {
                                ColumnTypeList.RemoveAt(10 - removeCounter);
                                ColumnSizeList.RemoveAt(10 - removeCounter);
                                ColumnWidthSizeList.RemoveAt(10 - removeCounter);
                                removeCounter = removeCounter + 1;
                            }

                            ColumnType = (string[])ColumnTypeList.ToArray(typeof(string));
                            ColumnSize = (string[])ColumnSizeList.ToArray(typeof(string));
                            ColumnWidthSize = (string[])ColumnWidthSizeList.ToArray(typeof(string));

                            oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, dtExport, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                        }
                        else//8 c                        1.TradeDate,2.Scrip,3.Sett,4.Terminal,5.TrdCode,6.Order,7.OrderTime, same in all cond.
                        {
                            string[] ColumnType = { "V", "V", "V", "V", "V", "V", "V", "V", "V", "I", "I", "I", "N", "V", "N", "N", "N", "N", "N", "V", "N" };
                            string[] ColumnSize = { "40", "40", "20", "20", "20", "30", "20", "20", "20", "16", "16", "16", "18,4", "15", "18,4", "18,2", "18,4", "18,2", "18,2", "6", "18,2" };
                            string[] ColumnWidthSize = { "10", "30", "8", "10", "6", "16", "8", "10", "8", "6", "10", "10", "8", "4", "8", "10", "10", "12", "8", "6", "12" };


                            ArrayList ColumnTypeList = new ArrayList(ColumnType);
                            ArrayList ColumnSizeList = new ArrayList(ColumnSize);
                            ArrayList ColumnWidthSizeList = new ArrayList(ColumnWidthSize);

                            int removeCounter = 0;

                            if (SelectTerminalid == "N")
                            {
                                ColumnTypeList.RemoveAt(3);
                                ColumnSizeList.RemoveAt(3);
                                ColumnWidthSizeList.RemoveAt(3);
                                removeCounter = 1;
                            }
                            if (SelectTradeCode == "N")
                            {
                                ColumnTypeList.RemoveAt(4 - removeCounter);
                                ColumnSizeList.RemoveAt(4 - removeCounter);
                                ColumnWidthSizeList.RemoveAt(4 - removeCounter);
                                removeCounter = removeCounter + 1;
                            }
                            if (SelectOrderNo == "N")
                            {
                                ColumnTypeList.RemoveAt(5 - removeCounter);
                                ColumnSizeList.RemoveAt(5 - removeCounter);
                                ColumnWidthSizeList.RemoveAt(5 - removeCounter);
                                removeCounter = removeCounter + 1;
                            }
                            if (SelectOrderEntryTime == "N")
                            {
                                ColumnTypeList.RemoveAt(6 - removeCounter);
                                ColumnSizeList.RemoveAt(6 - removeCounter);
                                ColumnWidthSizeList.RemoveAt(6 - removeCounter);
                                removeCounter = removeCounter + 1;
                            }
                            if (SelectTradeNo == "N")
                            {
                                ColumnTypeList.RemoveAt(7 - removeCounter);
                                ColumnSizeList.RemoveAt(7 - removeCounter);
                                ColumnWidthSizeList.RemoveAt(7 - removeCounter);
                                removeCounter = removeCounter + 1;
                            }
                            if (SelectTradeEntryTime == "N")
                            {
                                ColumnTypeList.RemoveAt(8 - removeCounter);
                                ColumnSizeList.RemoveAt(8 - removeCounter);
                                ColumnWidthSizeList.RemoveAt(8 - removeCounter);
                                removeCounter = removeCounter + 1;
                            }
                            if (SelectCntrNo == "N")
                            {
                                ColumnTypeList.RemoveAt(9 - removeCounter);
                                ColumnSizeList.RemoveAt(9 - removeCounter);
                                ColumnWidthSizeList.RemoveAt(9 - removeCounter);
                                removeCounter = removeCounter + 1;
                            }

                            ColumnType = (string[])ColumnTypeList.ToArray(typeof(string));
                            ColumnSize = (string[])ColumnSizeList.ToArray(typeof(string));
                            ColumnWidthSize = (string[])ColumnWidthSizeList.ToArray(typeof(string));

                            oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, dtExport, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                        }
                    }
                }
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Reload1", "Reload();", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordDisplay", "RecordDisplay('1');", true);
            }
        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            reportType = "Export";
            FilterColumnCheck();
            Procedure(bindParams, reportType);
            //ds = (DataSet)ViewState["dataset"];
            //Export(ds);
        }
        # endregion

        # region Email Format
        protected void BtnEmail_Click(object sender, EventArgs e)
        {
            reportType = "Mail";
            FilterColumnCheck();
            Procedure(bindParams, reportType);
        }
        void Email(DataSet ds)
        {
            string Date = oconverter.ArrangeDate2(dtFrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtTo.Value.ToString());

            if (ddlEmail.SelectedItem.Value.ToString().Trim() == "Client")
            {
                EmailClientWise(ds, Date);
            }
            if (ddlEmail.SelectedItem.Value.ToString().Trim() == "Group/Branch")
            {
                EmailGroupBranchWise(ds, Date);
            }
            if (ddlEmail.SelectedItem.Value.ToString().Trim() == "User")
            {
                EmailUserWise(ds, Date);
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
            Distinctclient = viewClient.ToTable(true, new string[] { "CustomerId", "CustomerName" });

            if (Distinctclient.Rows.Count > 0)
            {
                cmbrecord.DataSource = Distinctclient;
                cmbrecord.DataValueField = "CustomerId";
                cmbrecord.DataTextField = "CustomerName";
                cmbrecord.DataBind();

            }
            ViewState["mailsendresult"] = "mail";
            /////////For Client Email
            for (int k = 0; k < cmbrecord.Items.Count; k++)
            {
                FnHtml(ds, cmbrecord.Items[k].Value.ToString().Trim());

                if (oDBEngine.SendReport(ViewState["mail"].ToString().Trim(), cmbrecord.Items[k].Value.ToString().Trim(), Date.ToString().Trim(), "Trade Register [" + Date.ToString().Trim() + "]") == true)
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordStatus", "RecordDisplay('4');", true);
            }
            if (ViewState["mailsendresult"].ToString().Trim() == "SomeClientError")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordStatus", "RecordDisplay('5');", true);
            }
            if (ViewState["mailsendresult"].ToString().Trim() == "Success")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordDisplay", "RecordDisplay('6');", true);
            }
        }
        void EmailGroupBranchWise(DataSet ds, string Date)
        {
            ViewState["mailsendresult"] = "mail";
            ViewState["GrpEmail"] = "mail";

            DataView viewData = new DataView();
            viewData = ds.Tables[0].DefaultView;
            viewData.RowFilter = " Grpid<>'ZZZZZZZZZZZ' and Grpid is not null and GrpEmail is not null";
            DataTable dt = new DataTable();
            dt = viewData.ToTable();

            DataTable DistinctGrp = new DataTable();
            DataView viewGrp = new DataView(dt);
            DistinctGrp = viewGrp.ToTable(true, new string[] { "Grpid", "GrpEmail" });


            if (DistinctGrp.Rows.Count > 0)
            {
                for (int j = 0; j < DistinctGrp.Rows.Count; j++)
                {
                    DataView viewgrp = new DataView();
                    viewgrp = ds.Tables[0].DefaultView;
                    viewgrp.RowFilter = "GrpId='" + DistinctGrp.Rows[j][0].ToString().Trim() + "'";
                    DataTable dt1 = new DataTable();
                    dt1 = viewgrp.ToTable();

                    DataView viewData1 = new DataView();
                    viewData1 = dt1.DefaultView;
                    viewData1.RowFilter = " Customerid<>'ZZZZZZZZZZZ' and Customerid is not null";
                    DataTable dt2 = new DataTable();
                    dt2 = viewData1.ToTable();

                    DataTable Distinctclient = new DataTable();
                    DataView viewClient = new DataView(dt2);
                    Distinctclient = viewClient.ToTable(true, new string[] { "CustomerId", "CustomerName" });

                    if (Distinctclient.Rows.Count > 0)
                    {
                        cmbrecord.DataSource = Distinctclient;
                        cmbrecord.DataValueField = "CustomerId";
                        cmbrecord.DataTextField = "CustomerName";
                        cmbrecord.DataBind();

                    }
                    /////////For Client Email Begin
                    for (int k = 0; k < cmbrecord.Items.Count; k++)
                    {
                        FnHtml(ds, cmbrecord.Items[k].Value.ToString().Trim());

                        if (ViewState["GrpEmail"].ToString().Trim() == "mail")
                        {
                            ViewState["GrpEmail"] = ViewState["mail"].ToString().Trim();
                        }
                        else
                        {
                            ViewState["GrpEmail"] = ViewState["GrpEmail"].ToString().Trim() + ViewState["mail"].ToString().Trim();
                        }
                    }
                    /////////For Client Email End
                    /////////Group/Branch Email Send Begin
                    if (oDBEngine.SendReportBr(ViewState["GrpEmail"].ToString().Trim(), DistinctGrp.Rows[j]["GrpEmail"].ToString().Trim(), Date.ToString().Trim(), "Trade Register [" + Date.ToString().Trim() + "]", DistinctGrp.Rows[j]["Grpid"].ToString().Trim()) == true)
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordStatus", "RecordDisplay('4');", true);
            }
            if (ViewState["mailsendresult"].ToString().Trim() == "SomeClientError")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordStatus", "RecordDisplay('5');", true);
            }
            if (ViewState["mailsendresult"].ToString().Trim() == "Success")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordStatus", "RecordDisplay('6');", true);
            }
            if (ViewState["mailsendresult"].ToString().Trim() == "EmailNotFound")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordStatus", "RecordDisplay('7');", true);
            }
        }
        void EmailUserWise(DataSet ds, string Date)
        {
            if (HiddenField_emmail.Value.ToString().Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordStatus5", "RecordDisplay(7);", true);
            }
            else
            {
                DataView viewData = new DataView();
                viewData = ds.Tables[0].DefaultView;
                viewData.RowFilter = " Customerid<>'ZZZZZZZZZZZ' and Customerid is not null";
                DataTable dt = new DataTable();
                dt = viewData.ToTable();

                DataTable Distinctclient = new DataTable();
                DataView viewClient = new DataView(dt);
                Distinctclient = viewClient.ToTable(true, new string[] { "CustomerId", "CustomerName" });

                if (Distinctclient.Rows.Count > 0)
                {
                    cmbrecord.DataSource = Distinctclient;
                    cmbrecord.DataValueField = "CustomerId";
                    cmbrecord.DataTextField = "CustomerName";
                    cmbrecord.DataBind();

                }
                ViewState["mailsendresult"] = "mail";
                ViewState["Usermail"] = "UserMail";

                /////////For Client Email Begin
                for (int k = 0; k < cmbrecord.Items.Count; k++)
                {
                    FnHtml(ds, cmbrecord.Items[k].Value.ToString().Trim());

                    if (ViewState["Usermail"].ToString().Trim() == "UserMail")
                    {
                        ViewState["Usermail"] = ViewState["mail"].ToString().Trim();
                    }
                    else
                    {
                        ViewState["Usermail"] = ViewState["Usermail"].ToString().Trim() + ViewState["mail"].ToString().Trim();
                    }
                }
                /////////For Client Email End

                /////////User Email Send
                string[] clnt = HiddenField_emmail.Value.ToString().Split(',');
                int kk = clnt.Length;
                for (int i = 0; i < clnt.Length; i++)
                {
                    if (oDBEngine.SendReportSt(ViewState["Usermail"].ToString().Trim(), clnt[i].ToString().Trim(), Date.ToString().Trim(), "Trade Register [" + Date.ToString().Trim() + "]") == true)
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
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordStatus", "RecordDisplay('4');", true);
                }
                if (ViewState["mailsendresult"].ToString().Trim() == "SomeClientError")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordStatus", "RecordDisplay('5');", true);
                }
                if (ViewState["mailsendresult"].ToString().Trim() == "Success")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordStatus", "RecordDisplay('6');", true);
                }
            }
        }
        # endregion

        # region PDF Export Format
        void PdfPrint(DataSet ds)
        {
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
            }

            ViewState["billprintdate"] = oconverter.ArrangeDate2(dtFrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtTo.Value.ToString());

            string DateParameter = ViewState["billprintdate"].ToString();
            //ds.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\TradeRegister.xsd");
            string path = HttpContext.Current.Server.MapPath("..\\Reports\\TradeRegister.rpt");
            ReportDocument.Load(path);
            ReportDocument.SetDataSource(ds.Tables[0]);

            ReportDocument.SetParameterValue("@DateFormat", (object)DateParameter);
            ReportDocument.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
            ReportDocument.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
            ReportDocument.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "Trade Register");

        }
        # endregion

        # region Dos Format
        void dosprint(DataSet ds)
        {
            //string printName = new PrintDocument().PrinterSettings.PrinterName.Trim();
            //System.Management.ManagementObjectCollection printName = new System.Management.ManagementClass("Win32_Printer").GetInstances();
            //System.Management.ManagementObjectCollection printers = new System.Management.ManagementClass("Win32_Printer ").GetInstances();
            ReportDocument rpt = new ReportDocument();

            if (ds.Tables[0].Rows.Count > 0)
            {
                ds.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "//Reports//dostrade.xsd");
                ViewState["billprintdate"] = oconverter.ArrangeDate2(dtFrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(dtTo.Value.ToString());

                string DateParameter = ViewState["billprintdate"].ToString();
                string path = HttpContext.Current.Server.MapPath("..\\Reports\\dosTradeRegister.rpt");
                ReportDocument.Load(path);
                ReportDocument.SetDataSource(ds.Tables[0]);

                ReportDocument.SetParameterValue("@DateFormat", (object)DateParameter);
                ReportDocument.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
                //ReportDocument.PrintOptions.PrinterName = DropDownList1.SelectedItem.Value.ToString().Trim();
                //ReportDocument.PrintOptions.PaperSize =CrystalDecisions.Shared.PaperSize "9.5x12";
                //ReportDocument.PrintOptions.PrinterName = printName.ToString().Trim();
                //ReportDocument.PrintToPrinter(1, false, 0, 0);
                string tmpPdfPath;
                tmpPdfPath = string.Empty;
                tmpPdfPath = HttpContext.Current.Server.MapPath(@"..\Documents\TempPdfLocation\");
                string abcd = tmpPdfPath + "trade.pdf";
                ReportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, abcd);
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript4567", "<script language='javascript'>window.open('traderegisterpopup.aspx');</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript12", "<script language='javascript'>Page_Load();</script>");
                //Page.ClientScript.RegisterStartupScript(GetType(), "JScript14", "<script language='javascript'>alert('Print Send to Select Printer');</script>");
            }

        }
        protected void btndos_Click(object sender, EventArgs e)
        {
            reportType = "dos";
            FilterColumnCheck();
            Procedure(bindParams, reportType);
        }
        # endregion
    }
}