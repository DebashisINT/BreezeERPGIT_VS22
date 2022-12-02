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
    public partial class frmReport_IframeLedgerViewSingleClient : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        int pageindex = 0;
        int pagecount = 0;
        int pageSize;
        int rowcount = 0;
        string SubLedgerType = "";
        string Branch;
        string Segment;
        string SegmentT;
        string MainAcID;
        string SubAcID;
        string SegMentName;
        string data;
        decimal openingBal;
        decimal debitTotal = 0;
        decimal creditTotal = 0;
        string SegmentID = null;
        string MainAcIDforOp;
        static DataTable dtCashBankBook = new DataTable();
        static DataTable dtLedgerView = new DataTable();
        BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        ExcelFile objExcel = new ExcelFile();
        static string Check;
        FAReportsOther farep = new FAReportsOther();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDbEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");

            //// this.Page.ClientScript.RegisterStartupScript(GetType(), "acctypes", "<script>ChangeAccountType();</script>");
            if (!IsPostBack)
            {
                Branch = null;
                SegmentT = null;
                MainAcID = null;
                SubAcID = null;
                MainAcIDforOp = null;
                SegMentName = null;
                SubLedgerType = "";
                Check = null;
                dtFrom.EditFormatString = oconverter.GetDateFormat("Date");
                dtTo.EditFormatString = oconverter.GetDateFormat("Date");
                //dtFrom.Value = Convert.ToDateTime(DateTime.Today);
                string[] FinalCialYear = Session["LastFinYear"].ToString().Split('-');
                //dtFrom.Value = Convert.ToDateTime(oDBEngine.GetDate().Month + "/01/" + oDBEngine.GetDate().Year);
                //dtTo.Value = Convert.ToDateTime("03/31/" + FinalCialYear[1].ToString());
                dtFrom.Value = DateTime.Today.AddDays(-30);
                dtTo.Value = DateTime.Today;

                DataTable DtSegComp = null;
                if (Request.QueryString["seg"].ToString() == "n")
                {
                    //DtSegComp = oDbEngine.GetDataTable("(select exch_compId,exch_internalId,isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Comp from tbl_master_companyExchange where exch_compid in (select top 1 ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment=" + Session["userlastsegment"].ToString() + ")) as D", "*", "Comp in(select seg_name from tbl_master_segment where seg_id=" + Session["userlastsegment"].ToString() + ")");
                    DtSegComp = oDbEngine.GetDataTable("(select exch_compId,exch_internalId,isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Comp from tbl_master_companyExchange where exch_compid in (select top 1 ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment=9)) as D", "*", "Comp in(select seg_name from tbl_master_segment where seg_id=9)");
                }
                else if (Request.QueryString["seg"].ToString() == "c")
                {
                    DtSegComp = oDbEngine.GetDataTable("(select exch_compId,exch_internalId,isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Comp from tbl_master_companyExchange where exch_compid in (select top 1 ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment=10)) as D", "*", "Comp in(select seg_name from tbl_master_segment where seg_id=10)");
                }
                if (DtSegComp.Rows.Count > 0)
                {
                    string CompanyID = DtSegComp.Rows[0][0].ToString();
                    Session["CompanyID"] = CompanyID;
                    SegmentID = DtSegComp.Rows[0][1].ToString();
                    //* litSegment.InnerText = DtSegComp.Rows[0][2].ToString();
                    Session["SegmentID"] = SegmentID;
                    SegMentName = DtSegComp.Rows[0][2].ToString();
                    ViewState["SegMentName"] = SegMentName;
                    //FillGrid();
                }

                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");
                txtsubscriptionID.Attributes.Add("onkeyup", "showOptions(this,'SearchMainAccountBranchSegment',event)");
                txtSubsubcriptionID.Attributes.Add("onkeyup", "showOptionsforSunAc(this,'selectSubAccountForMainAccount',event)");
                //FillDropDown();

                //added new
                if (Request.QueryString["seg"].ToString() == "n")
                {
                    txtMainAccount_hidden.Value = "SYSTM00043~NSDL Clients";
                }
                else if (Request.QueryString["seg"].ToString() == "c")
                {
                    txtMainAccount_hidden.Value = "SYSTM00042~CDSL Clients";

                }
                HdnSubAc.Value = Request.QueryString["accno"].ToString();
                try
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct22", "HideOnOffLoading()", true);
                    FillGrid();
                    if (ViewState["Check"] != null)
                    {
                        if (ViewState["Check"].ToString() == "b")
                        {
                            DataTable dtGrid = new DataTable();
                            grdCashBankBook.DataSource = dtGrid;
                            grdCashBankBook.DataBind();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct11", "alertMessage()", true);
                        }
                    }
                }
                catch (Exception ex)
                {
                    DataTable dtGrid = new DataTable();
                    grdCashBankBook.DataSource = dtGrid;
                    grdCashBankBook.DataBind();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct11", "alertMessage()", true);
                }


                //added new

            }
            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //___________-end here___//
        }

        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            string id = eventArgument.ToString();
            string[] idlist = id.Split('~');
            string str = "";
            string str1 = "";
            if (idlist[0] == "ComboChange")
            {
                MainAcID = idlist[1];
            }
            else
            {
                string[] cl = idlist[1].Split(',');
                for (int i = 0; i < cl.Length; i++)
                {
                    if (idlist[0] != "Ac Name")
                    {
                        SubLedgerType = "";
                        string[] val = cl[i].Split(';');
                        if (str == "")
                        {
                            str = "'" + val[0] + "'";
                            str1 = val[0] + ";" + val[1];
                        }
                        else
                        {
                            str += ",'" + val[0] + "'";
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
                        SubLedgerType = AcVal[1];
                    }
                }
                if (idlist[0] == "Branch")
                {
                    Branch = str;
                    data = "Branch~" + str;
                }
                else if (idlist[0] == "Segment")
                {
                    SegmentT = str;
                    data = "Segment~" + str;
                }
                else if (idlist[0] == "Ac Name")
                {
                    MainAcID = str;
                    data = "Ac Name~" + str + "~" + SubLedgerType;
                    // FillDropDown();
                }
                else if (idlist[0] == "Sub Ac")
                {
                    SubAcID = str;
                    data = "Sub Ac~" + str;
                }
            }
        }
        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct2", "HideOnOffLoading()", true);
                FillGrid();
                if (ViewState["Check"] != null)
                {
                    if (ViewState["Check"].ToString() == "b")
                    {
                        DataTable dtGrid = new DataTable();
                        grdCashBankBook.DataSource = dtGrid;
                        grdCashBankBook.DataBind();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct1", "alertMessage()", true);
                    }
                }
            }
            catch (Exception ex)
            {
                DataTable dtGrid = new DataTable();
                grdCashBankBook.DataSource = dtGrid;
                grdCashBankBook.DataBind();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct1", "alertMessage()", true);
            }

        }
        public void FillDropDown()
        {
            string SubID = null;
            SubAcID = HdnSubAc.Value;
            string MainSubID = null;
            ////if (rdSubAcAll.Checked == true)
            ////{
            ////    SubAcID = null;
            ////}
            SubAcID = null;
            if (SubAcID == null)
            {
                SubID = null;
                MainSubID = null;
            }
            else
            {
                SubID = " and subaccount_code in(" + SubAcID + ")";
                MainSubID = " and AccountsLedger_SubAccountID in(" + SubAcID + ")";
            }
            string MainID = null;
            string MainLedgerID = null;
            if (txtMainAccount_hidden.Value != null && txtMainAccount_hidden.Value != "" && txtMainAccount_hidden.Value != "No Record Found")
            {
                string[] MainAccount = txtMainAccount_hidden.Value.Split('~');
                MainAcID = "'" + MainAccount[0] + "'";
                SubLedgerType = MainAccount[1];
            }
            else
            {
                MainAcID = null;
            }
            ViewState["MainAcID"] = MainAcID;
            ViewState["SubLedgerType"] = SubLedgerType;
            HdnMainAc.Value = MainAcID;
            HdnSubLedgerType.Value = SubLedgerType;
            if (MainAcID == null)
            {

                MainID = " subaccount_mainacreferenceid not in('SYSTM00001','SYSTM00002')";
                MainLedgerID = " and accountsledger_mainaccountid not in('SYSTM00001','SYSTM00002')";
            }
            else
            {
                MainID = " subaccount_mainacreferenceid in(" + MainAcID + ")";
                MainLedgerID = " and accountsledger_mainaccountid in(" + MainAcID + ")";
                //MainID = MainAcID;
            }

            if (Segment == null)
            {
                Segment = Session["SegmentID"].ToString();
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct111", "HideoffOnButton()", true);
        }
        public void FillGrid()
        {
            try
            {
                if (HdnSelectLedger.Value != "S")
                    FillDropDown();
                SegmentT = HdnSegment.Value;
                SubAcID = HdnSubAc.Value;
                ViewState["SubAcID"] = SubAcID;
                MainAcID = HdnMainAc.Value;
                SubLedgerType = HdnSubLedgerType.Value;

                SegMentName = ViewState["SegMentName"].ToString();
                ViewState["Check"] = "a";
                decimal receipt = 0;
                decimal Payment = 0;
                pageSize = 25;
                DateTime TranDate = DateTime.Today;
                DataTable OpenBalance = new DataTable();
                grdCashBankBook.PageSize = pageSize;
                System.Globalization.NumberFormatInfo currencyFormat = new System.Globalization.CultureInfo("hi-IN").NumberFormat;
                currencyFormat.CurrencySymbol = "";
                currencyFormat.CurrencyNegativePattern = 2;
                ////if (rdbSegAll.Checked == true)
                ////{
                ////    DataTable dtSegment = oDbEngine.GetDataTable("tbl_master_companyExchange", "exch_internalId", "exch_compId='" + Session["CompanyID"].ToString() + "'");
                ////    if (dtSegment.Rows.Count > 0)
                ////    {
                ////        for (int i = 0; i < dtSegment.Rows.Count; i++)
                ////        {
                ////            if (Segment == null)
                ////                Segment = dtSegment.Rows[i][0].ToString();
                ////            else
                ////                Segment += "," + dtSegment.Rows[i][0].ToString();
                ////        }
                ////    }
                ////}
                ////else
                ////{
                ////    if (SegmentT == null || SegmentT == "")
                ////    {
                ////        Segment = Session["SegmentID"].ToString();
                ////    }
                ////    else
                ////        Segment = SegmentT;
                ////}

                if (SegmentT == null || SegmentT == "")
                {
                    Segment = Session["SegmentID"].ToString();
                }
                else
                    Segment = SegmentT;

                ////if (rdbbAll.Checked == true)
                ////{
                ////    Branch = Session["userbranchHierarchy"].ToString();
                ////}
                Branch = Session["userbranchHierarchy"].ToString();

                string mainAccountSearch = null;
                string SubAccountSearch = null;
                string SubACountForAll = null;
                ////if (rdSubAcAll.Checked == true)
                ////{
                ////    SubACountForAll = null;
                ////}
                ////else
                ////{
                ////    if (SubAcID == null || SubAcID == "")
                ////        SubACountForAll = null;
                ////    else
                ////        SubACountForAll = " and AccountsLedger_SubAccountID in(" + SubAcID + ") ";
                ////}

                SubACountForAll = null;


                ////if (ddlAccountType.SelectedValue == "0")
                ////{
                ////    mainAccountSearch = "and accountsledger_MainAccountID in('SYSTM00001')";
                ////    MainAcIDforOp = "'SYSTM00001'";
                ////}
                ////else if (ddlAccountType.SelectedValue == "1")
                ////{
                ////    mainAccountSearch = "and accountsledger_MainAccountID in('SYSTM00002')";
                ////    MainAcIDforOp = "'SYSTM00002'";
                ////}
                ////else if (ddlAccountType.SelectedValue == "2")
                ////{
                ////    mainAccountSearch = "and accountsledger_MainAccountID in('SYSTM00001','SYSTM00002')";
                ////    MainAcIDforOp = "'SYSTM00001','SYSTM00002'";
                ////}
                ////else
                ////{
                ////    MainAcIDforOp = MainAcID;
                ////    mainAccountSearch = "and accountsledger_MainAccountID in(" + MainAcID + ")";
                ////}
                MainAcIDforOp = MainAcID;
                mainAccountSearch = "and accountsledger_MainAccountID in(" + MainAcID + ")";

                ViewState["MainAcIDforOp"] = MainAcIDforOp;
                ViewState["Segment"] = Segment;
                dtCashBankBook = new DataTable();
                dtLedgerView = new DataTable();


                if (SubLedgerType.Trim() == "None")
                {
                    SubAccountSearch = null;
                    ViewState["SubAccountSearch"] = SubAccountSearch;
                    dtCashBankBook = oDbEngine.GetDataTable("Trans_accountsledger a ", "convert(varchar(11),a.accountsledger_transactiondate,113) as TrDate,case when a.accountsledger_valuedate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),a.accountsledger_valuedate,113) end as ValueDate,a.accountsledger_TransactionReferenceID, isnull(a.accountsledger_Narration,'') +' - '+isnull(isnull((select MainAccount_Name from Master_MainAccount where MainAccount_AccountCode=AccountsLedger_CashBankName),AccountsLedger_CashBankName),'') as accountsledger_Narration ,'1' as AccountName,case when a.Accountsledger_AmountCr='0.00000000' then null else a.Accountsledger_AmountCr end as Accountsledger_AmountDr,case when a.Accountsledger_AmountDr='0.00000000' then null else a.Accountsledger_AmountDr end as Accountsledger_AmountCr,'0.0' as Closing,a.accountsledger_transactiondate as accountsledger_transactiondate,a.accountsledger_InstrumentNumber,(a.accountsledger_SettlementNumber+' '+a.accountsledger_SettlementType) as SettlementNumber,a.accountsledger_MainAccountID as MainID,a.accountsledger_SubAccountID as SubID,a.accountsledger_companyID as CompanyID,a.accountsledger_exchangeSegmentID as SegID,a.AccountsLedger_TransactionType  as CashType,(select top 1 convert(varchar(12),Settlements_StartDateTime,113) from master_settlements where Settlements_Number=a.accountsledger_SettlementNumber and Settlements_TypeSuffix=a.accountsledger_SettlementType) as PayoutDate,(select branch_code from tbl_master_branch where branch_id=(select cnt_branchid from tbl_master_contact where cnt_internalID=accountsledger_SubAccountID)) as BranchCode," + Session["userid"].ToString() + " as UserID ", " accountsledger_companyID='" + Session["CompanyID"].ToString() + "' and accountsledger_ExchangeSegmentID in(" + Segment + ") and  cast(DATEADD(dd, 0, DATEDIFF(dd, 0,accountsledger_transactiondate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtFrom.Value + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtTo.Value + "')) as datetime)  " + mainAccountSearch + " and AccountsLedger_BranchID in(" + Branch + ") " + SubAccountSearch + "  and AccountsLedger_TransactionType<>'OpeningBalance'", " accountsledger_transactiondate,accountsledger_TransactionReferenceID");
                    dtLedgerView = oDbEngine.GetDataTable("Trans_accountsledger a ", "convert(varchar(11),a.accountsledger_transactiondate,113) as TrDate,case when a.accountsledger_valuedate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),a.accountsledger_valuedate,113) end as ValueDate,a.accountsledger_TransactionReferenceID, isnull(a.accountsledger_Narration,'') +' - '+isnull(isnull((select MainAccount_Name from Master_MainAccount where MainAccount_AccountCode=AccountsLedger_CashBankName),AccountsLedger_CashBankName),'') as accountsledger_Narration,'1' as AccountName,case when a.Accountsledger_AmountCr='0.00000000' then null else a.Accountsledger_AmountCr end as Accountsledger_AmountDr,case when a.Accountsledger_AmountDr='0.00000000' then null else a.Accountsledger_AmountDr end as Accountsledger_AmountCr,'0.0' as Closing,a.accountsledger_transactiondate as accountsledger_transactiondate,a.accountsledger_InstrumentNumber,(a.accountsledger_SettlementNumber+' '+a.accountsledger_SettlementType) as SettlementNumber,a.accountsledger_MainAccountID as MainID,a.accountsledger_SubAccountID as SubID,a.accountsledger_companyID as CompanyID,a.accountsledger_exchangeSegmentID as SegID,a.AccountsLedger_TransactionType  as CashType,(select top 1 convert(varchar(12),Settlements_StartDateTime,113) from master_settlements where Settlements_Number=a.accountsledger_SettlementNumber and Settlements_TypeSuffix=a.accountsledger_SettlementType) as PayoutDate,(select branch_code from tbl_master_branch where branch_id=(select cnt_branchid from tbl_master_contact where cnt_internalID=accountsledger_SubAccountID)) as BranchCode," + Session["userid"].ToString() + " as UserID  ", " accountsledger_companyID='" + Session["CompanyID"].ToString() + "' and accountsledger_ExchangeSegmentID in(" + Segment + ") and  accountsledger_transactiondate between ('" + dtFrom.Value + "') and ('" + dtTo.Value + "')  " + mainAccountSearch + " " + SubACountForAll + " and AccountsLedger_BranchID in(" + Branch + ")  and AccountsLedger_TransactionType<>'OpeningBalance'", " accountsledger_transactiondate,accountsledger_TransactionReferenceID");
                    OpenBalance = oDbEngine.OpeningBalanceOnlyJournal(MainAcIDforOp, null, Convert.ToDateTime(dtFrom.Value), Segment, Session["CompanyID"].ToString(), Convert.ToDateTime(dtTo.Value));
                }
                else
                {
                    ////if (cmbclientsPager.SelectedItem != null)
                    ////    SubAccountSearch = " and AccountsLedger_SubAccountID in('" + cmbclientsPager.SelectedItem.Value + "') ";
                    ////else
                    ////    SubAccountSearch = " and AccountsLedger_SubAccountID in(" + SubAcID + ") ";

                    //SubAcID = "10000334";
                    SubAccountSearch = " and AccountsLedger_SubAccountID in('" + SubAcID + "') ";

                    ViewState["SubAccountSearch"] = " and AccountsLedger_SubAccountID in('" + SubAcID + "') ";
                    dtCashBankBook = oDbEngine.GetDataTable("Trans_accountsledger a ", "convert(varchar(11),a.accountsledger_transactiondate,113) as TrDate,case when a.accountsledger_valuedate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),a.accountsledger_valuedate,113) end as ValueDate,a.accountsledger_TransactionReferenceID, isnull(a.accountsledger_Narration,'') +' - '+isnull(isnull((select MainAccount_Name from Master_MainAccount where MainAccount_AccountCode=AccountsLedger_CashBankName),AccountsLedger_CashBankName),'') as accountsledger_Narration,'1' as AccountName,case when a.Accountsledger_AmountCr='0.00000000' then null else a.Accountsledger_AmountCr end as Accountsledger_AmountDr,case when a.Accountsledger_AmountDr='0.00000000' then null else a.Accountsledger_AmountDr end as Accountsledger_AmountCr,'0.0' as Closing,a.accountsledger_transactiondate as accountsledger_transactiondate,a.accountsledger_InstrumentNumber,(a.accountsledger_SettlementNumber+' '+a.accountsledger_SettlementType) as SettlementNumber,a.accountsledger_MainAccountID as MainID,a.accountsledger_SubAccountID as SubID,a.accountsledger_companyID as CompanyID,a.accountsledger_exchangeSegmentID as SegID,a.AccountsLedger_TransactionType  as CashType,(select top 1 convert(varchar(12),Settlements_StartDateTime,113) from master_settlements where Settlements_Number=a.accountsledger_SettlementNumber and Settlements_TypeSuffix=a.accountsledger_SettlementType) as PayoutDate,(select branch_code from tbl_master_branch where branch_id=(select cnt_branchid from tbl_master_contact where cnt_internalID=accountsledger_SubAccountID)) as BranchCode," + Session["userid"].ToString() + " as UserID  ", " accountsledger_companyID='" + Session["CompanyID"].ToString() + "' and accountsledger_ExchangeSegmentID in(" + Segment + ") and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,accountsledger_transactiondate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtFrom.Value + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtTo.Value + "')) as datetime)  " + mainAccountSearch + " and AccountsLedger_BranchID in(" + Branch + ") " + SubAccountSearch + " and AccountsLedger_SubAccountID is not null and AccountsLedger_TransactionType<>'OpeningBalance'", " accountsledger_transactiondate,accountsledger_TransactionReferenceID");
                    dtLedgerView = oDbEngine.GetDataTable("Trans_accountsledger a ", "convert(varchar(11),a.accountsledger_transactiondate,113) as TrDate,case when a.accountsledger_valuedate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),a.accountsledger_valuedate,113) end as ValueDate,a.accountsledger_TransactionReferenceID, isnull(a.accountsledger_Narration,'') +' - '+isnull(isnull((select MainAccount_Name from Master_MainAccount where MainAccount_AccountCode=AccountsLedger_CashBankName),AccountsLedger_CashBankName),'') as accountsledger_Narration,'1' as AccountName,case when a.Accountsledger_AmountCr='0.00000000' then null else a.Accountsledger_AmountCr end as Accountsledger_AmountDr,case when a.Accountsledger_AmountDr='0.00000000' then null else a.Accountsledger_AmountDr end as Accountsledger_AmountCr,'0.0' as Closing,a.accountsledger_transactiondate as accountsledger_transactiondate,a.accountsledger_InstrumentNumber,(a.accountsledger_SettlementNumber+' '+a.accountsledger_SettlementType) as SettlementNumber,a.accountsledger_MainAccountID as MainID,a.accountsledger_SubAccountID as SubID,a.accountsledger_companyID as CompanyID,a.accountsledger_exchangeSegmentID as SegID,a.AccountsLedger_TransactionType  as CashType,(select top 1 convert(varchar(12),Settlements_StartDateTime,113) from master_settlements where Settlements_Number=a.accountsledger_SettlementNumber and Settlements_TypeSuffix=a.accountsledger_SettlementType) as PayoutDate,(select branch_code from tbl_master_branch where branch_id=(select cnt_branchid from tbl_master_contact where cnt_internalID=accountsledger_SubAccountID)) as BranchCode," + Session["userid"].ToString() + " as UserID  ", " accountsledger_companyID='" + Session["CompanyID"].ToString() + "' and accountsledger_ExchangeSegmentID in(" + Segment + ") and  accountsledger_transactiondate between ('" + dtFrom.Value + "') and ('" + dtTo.Value + "')  " + mainAccountSearch + " and AccountsLedger_BranchID in(" + Branch + ") " + SubACountForAll + " and AccountsLedger_SubAccountID is not null and AccountsLedger_TransactionType<>'OpeningBalance'", " accountsledger_transactiondate,accountsledger_TransactionReferenceID");
                    ////if (cmbclientsPager.SelectedItem != null)
                    ////    OpenBalance = oDbEngine.OpeningBalanceOnlyJournal(MainAcIDforOp, cmbclientsPager.SelectedItem.Value, Convert.ToDateTime(dtFrom.Value), Segment, Session["CompanyID"].ToString(), Convert.ToDateTime(dtTo.Value));
                    ////else
                    ////{
                    ////    string SubAccountID = SubAcID.Replace("'", "");
                    ////    OpenBalance = oDbEngine.OpeningBalanceOnlyJournal(MainAcIDforOp, SubAccountID, Convert.ToDateTime(dtFrom.Value), Segment, Session["CompanyID"].ToString(), Convert.ToDateTime(dtTo.Value));
                    ////}
                    string SubAccountID = SubAcID.Replace("'", "");
                    OpenBalance = oDbEngine.OpeningBalanceOnlyJournal(MainAcIDforOp, SubAccountID, Convert.ToDateTime(dtFrom.Value), Segment, Session["CompanyID"].ToString(), Convert.ToDateTime(dtTo.Value));
                }

                DataTable dtCashBankBook_New = dtCashBankBook.Copy();
                dtCashBankBook_New.Rows.Clear();
                DataRow newRow = dtCashBankBook_New.NewRow();
                newRow[1] = oconverter.ArrangeDate2(Convert.ToDateTime(dtFrom.Value).ToShortDateString());
                newRow[3] = "Opening Balance";
                //newRow[7] = Convert.ToDecimal(OpenBalance.Rows[0][1].ToString()) * (-1);
                if (Convert.ToDecimal(OpenBalance.Rows[0][1].ToString()) < 0)
                {
                    newRow[5] = Convert.ToDecimal(OpenBalance.Rows[0][1].ToString()) * (-1);
                    Payment += Convert.ToDecimal(OpenBalance.Rows[0][1].ToString()) * (-1);
                    openingBal = Convert.ToDecimal(OpenBalance.Rows[0][1].ToString());
                    //newRow[7] = Convert.ToDecimal(OpenBalance.Rows[0][1].ToString());
                }
                else
                {
                    decimal newpay = Convert.ToDecimal(OpenBalance.Rows[0][1].ToString());
                    if (newpay != 0)
                        newRow[6] = newpay;
                    receipt += Convert.ToDecimal(OpenBalance.Rows[0][1].ToString());
                    openingBal = Convert.ToDecimal(OpenBalance.Rows[0][1].ToString());
                    //newRow[7] = Convert.ToDecimal(OpenBalance.Rows[0][1].ToString());
                }
                dtCashBankBook_New.Rows.Add(newRow);
                for (int i = 0; i < dtCashBankBook.Rows.Count; i++)
                {
                    newRow = dtCashBankBook_New.NewRow();
                    newRow[0] = dtCashBankBook.Rows[i]["TrDate"];
                    newRow[1] = dtCashBankBook.Rows[i]["ValueDate"];
                    newRow[2] = dtCashBankBook.Rows[i]["accountsledger_TransactionReferenceID"];
                    newRow[3] = dtCashBankBook.Rows[i]["accountsledger_Narration"];
                    newRow[4] = dtCashBankBook.Rows[i]["AccountName"];
                    newRow[5] = dtCashBankBook.Rows[i]["Accountsledger_AmountCr"];
                    newRow[6] = dtCashBankBook.Rows[i]["Accountsledger_AmountDr"];
                    newRow[7] = dtCashBankBook.Rows[i]["Closing"];
                    newRow[8] = dtCashBankBook.Rows[i]["accountsledger_transactiondate"];
                    newRow[9] = dtCashBankBook.Rows[i]["accountsledger_InstrumentNumber"];
                    newRow[10] = dtCashBankBook.Rows[i]["SettlementNumber"];
                    newRow[11] = dtCashBankBook.Rows[i]["MainID"];
                    newRow[12] = dtCashBankBook.Rows[i]["SubID"];
                    newRow[13] = dtCashBankBook.Rows[i]["CompanyID"];
                    newRow[14] = dtCashBankBook.Rows[i]["SegID"];
                    newRow[15] = dtCashBankBook.Rows[i]["CashType"];
                    newRow[16] = dtCashBankBook.Rows[i]["PayoutDate"];
                    newRow[17] = dtCashBankBook.Rows[i]["BranchCode"];
                    newRow[18] = dtCashBankBook.Rows[i]["UserID"];
                    dtCashBankBook_New.Rows.Add(newRow);
                    TranDate = Convert.ToDateTime(dtCashBankBook.Rows[i]["accountsledger_transactiondate"].ToString());
                    if (dtCashBankBook.Rows[i]["Accountsledger_AmountDr"].ToString() != "")
                        receipt += Convert.ToDecimal(dtCashBankBook.Rows[i]["Accountsledger_AmountDr"].ToString());
                    if (dtCashBankBook.Rows[i]["Accountsledger_AmountCr"].ToString() != "")
                        Payment += Convert.ToDecimal(dtCashBankBook.Rows[i]["Accountsledger_AmountCr"].ToString());

                }
                dtCashBankBook.Rows.Clear();
                dtCashBankBook = dtCashBankBook_New.Copy();
                string DivPageCount = Convert.ToString(dtCashBankBook.Rows.Count % pageSize);
                if (DivPageCount == "0")
                    pagecount = dtCashBankBook.Rows.Count / pageSize;
                else
                    pagecount = dtCashBankBook.Rows.Count / pageSize + 1;
                TotalPages.Value = pagecount.ToString();
                if (pageindex <= 0)
                {
                    pageindex = 0;
                    openingBal = Convert.ToDecimal(OpenBalance.Rows[0][1].ToString());
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "hide", "Disable('P');", true);
                }
                if (pageindex >= int.Parse(TotalPages.Value.ToString()))
                {
                    pageindex = int.Parse(TotalPages.Value.ToString()) - 1;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "hide", "Disable('N');", true);
                }
                if (pageindex >= (int.Parse(TotalPages.Value.ToString()) - 1))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "hide", "Disable('N');", true);
                }
                if (pageindex > 0)
                {
                    int totalRecord = (pageindex) * pageSize;
                    decimal DR = 0;
                    decimal CR = 0;
                    openingBal = 0;
                    for (int i = 0; i < totalRecord; i++)
                    {
                        if (dtCashBankBook.Rows[i]["Accountsledger_AmountDr"].ToString() != "")
                            DR = decimal.Parse(dtCashBankBook.Rows[i]["Accountsledger_AmountDr"].ToString());
                        else
                            DR = 0;
                        if (dtCashBankBook.Rows[i]["Accountsledger_AmountCr"].ToString() != "")
                            CR = decimal.Parse(dtCashBankBook.Rows[i]["Accountsledger_AmountCr"].ToString());
                        else
                            CR = 0;
                        openingBal = CR - DR + openingBal;
                    }
                }
                grdCashBankBook.PageIndex = pageindex;
                CurrentPage.Value = pageindex.ToString();
                rowcount = 0;
                grdCashBankBook.DataSource = dtCashBankBook;
                grdCashBankBook.DataBind();
                if (Session["userlastsegment"].ToString() == "7")
                    grdCashBankBook.Columns[5].Visible = true;
                else
                    grdCashBankBook.Columns[5].Visible = false;
                if (Session["userlastsegment"].ToString() == "7" || Session["userlastsegment"].ToString() == "8")
                    grdCashBankBook.Columns[6].Visible = true;
                else
                    grdCashBankBook.Columns[6].Visible = false;
                ////if (radConsolidated.Checked == true)
                ////{
                ////    grdCashBankBook.Columns[2].Visible = false;
                ////}
                ////else
                ////{
                ////    grdCashBankBook.Columns[2].Visible = true;
                ////}

                grdCashBankBook.Columns[2].Visible = true;

                grdCashBankBook.FooterRow.Cells[3].Text = "Closing Balance";
                grdCashBankBook.FooterRow.Cells[11].Text = openingBal.ToString("c", currencyFormat);
                if (Payment != 0)
                    grdCashBankBook.FooterRow.Cells[9].Text = Payment.ToString("c", currencyFormat);
                else
                    grdCashBankBook.FooterRow.Cells[9].Text = "";
                if (receipt != 0)
                    grdCashBankBook.FooterRow.Cells[10].Text = receipt.ToString("c", currencyFormat);
                else
                    grdCashBankBook.FooterRow.Cells[10].Text = "";
                grdCashBankBook.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Left;
                grdCashBankBook.FooterRow.Cells[11].HorizontalAlign = HorizontalAlign.Right;
                grdCashBankBook.FooterRow.Cells[10].HorizontalAlign = HorizontalAlign.Right;
                grdCashBankBook.FooterRow.Cells[9].HorizontalAlign = HorizontalAlign.Right;
                grdCashBankBook.FooterRow.Cells[3].ForeColor = System.Drawing.Color.White;
                grdCashBankBook.FooterRow.Cells[10].ForeColor = System.Drawing.Color.White;
                grdCashBankBook.FooterRow.Cells[11].ForeColor = System.Drawing.Color.White;
                grdCashBankBook.FooterRow.Cells[9].ForeColor = System.Drawing.Color.White;
                grdCashBankBook.FooterRow.Cells[3].Font.Bold = true;
                grdCashBankBook.FooterRow.Cells[10].Font.Bold = true;
                grdCashBankBook.FooterRow.Cells[11].Font.Bold = true;
                grdCashBankBook.FooterRow.Cells[9].Font.Bold = true;
                grdCashBankBook.FooterRow.Cells[9].Wrap = false;
                grdCashBankBook.FooterRow.Cells[10].Wrap = false;
                grdCashBankBook.FooterRow.Cells[11].Wrap = false;
                string SpanText1 = oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "  - " + oconverter.ArrangeDate2(dtTo.Value.ToString());

                // ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct", "ShowHide('" + SpanText1 + "')", true);

                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "jsperiod", "<script>ShowHide('" + SpanText1 + "');</script>");

                ScriptManager.RegisterStartupScript(this, this.GetType(), "JShide", "DisabledDrp('a');", true);

                //// ScriptManager.RegisterStartupScript(this, this.GetType(), "speriod", "ShowPeriod();", true);
                ////this.Page.ClientScript.RegisterStartupScript(this.GetType(), "jsperiod", "<script>ShowPeriod();</script>");
                ////if (cmbclientsPager.SelectedItem == null)
                ////    ScriptManager.RegisterStartupScript(this, this.GetType(), "JShide", "DisabledDrp('a');", true);
                ////else
                ////    ScriptManager.RegisterStartupScript(this, this.GetType(), "JShide", "DisabledDrp('b');", true);


            }
            catch (Exception ex)
            {
                ViewState["Check"] = "b";
            }
        }
        protected void grdCashBankSummary_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string lcVar1 = ((DataRowView)e.Row.DataItem)["Accountsledger_AmountDr"].ToString();
                string lcVar2 = ((DataRowView)e.Row.DataItem)["Accountsledger_AmountCr"].ToString();
                if (lcVar1 == "")
                {
                    lcVar1 = "0";
                    e.Row.Cells[9].Text = "";
                }
                else
                    e.Row.Cells[9].Text = oconverter.getFormattedvalue(decimal.Parse(lcVar1));

                if (lcVar2 == "")
                {
                    lcVar2 = "0";
                    e.Row.Cells[10].Text = "";
                }
                else
                    e.Row.Cells[10].Text = oconverter.getFormattedvalue(decimal.Parse(lcVar2));
                debitTotal += decimal.Parse(lcVar1);
                creditTotal += decimal.Parse(lcVar2);
                System.Globalization.NumberFormatInfo currencyFormat = new System.Globalization.CultureInfo("hi-IN").NumberFormat;
                currencyFormat.CurrencySymbol = "";
                currencyFormat.CurrencyNegativePattern = 2;
                if (((DataRowView)e.Row.DataItem)["accountsledger_transactiondate"].ToString().Trim() == "")
                {
                    //openingBal = decimal.Parse(lcVar2) - decimal.Parse(lcVar1) + openingBal;
                    if (openingBal < 0)
                    {
                        e.Row.Cells[11].Text = openingBal.ToString("c", currencyFormat);//.Substring(1, openingBal.ToString("c", currencyFormat).Length - 1);
                        e.Row.Cells[11].ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        e.Row.Cells[11].Text = openingBal.ToString("c", currencyFormat);
                        //e.Row.Cells[7].Text = "Dr";
                    }
                }
                else
                {
                    openingBal = decimal.Parse(lcVar2) - decimal.Parse(lcVar1) + openingBal;
                    if (openingBal < 0)
                    {
                        e.Row.Cells[11].Text = openingBal.ToString("c", currencyFormat);//.Substring(1, openingBal.ToString("c", currencyFormat).Length - 1);
                        e.Row.Cells[11].ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        e.Row.Cells[11].Text = openingBal.ToString("c", currencyFormat);
                        //e.Row.Cells[7].Text = "Dr";
                    }
                }
                if (((DataRowView)e.Row.DataItem)["ValueDate"].ToString().Trim() == "" && ((DataRowView)e.Row.DataItem)["CashType"].ToString().Trim() == "Cash_Bank")
                {
                    e.Row.Cells[0].ForeColor = System.Drawing.Color.Red;
                    e.Row.Cells[1].ForeColor = System.Drawing.Color.Red;
                    e.Row.Cells[2].ForeColor = System.Drawing.Color.Red;
                    e.Row.Cells[3].ForeColor = System.Drawing.Color.Red;
                    e.Row.Cells[5].ForeColor = System.Drawing.Color.Red;
                    e.Row.Cells[6].ForeColor = System.Drawing.Color.Red;
                    e.Row.Cells[7].ForeColor = System.Drawing.Color.Red;
                    e.Row.Cells[8].ForeColor = System.Drawing.Color.Red;
                    e.Row.Cells[9].ForeColor = System.Drawing.Color.Red;
                    e.Row.Cells[10].ForeColor = System.Drawing.Color.Red;
                    e.Row.Cells[11].ForeColor = System.Drawing.Color.Red;
                }
                Label TradeDate = (Label)e.Row.FindControl("lblTradeDate");
                Label ReferenceID = (Label)e.Row.FindControl("lblVoucherNo");
                Label MainID = (Label)e.Row.FindControl("lblMainID");
                Label SubID = (Label)e.Row.FindControl("lblSubID");
                Label CompID = (Label)e.Row.FindControl("lblCompID");
                Label SegID = (Label)e.Row.FindControl("lblSegID");
                Label CashType = (Label)e.Row.FindControl("lblCashType");
                if (CashType.Text == "Cash_Bank")
                {
                    ((Label)e.Row.FindControl("lblVoucherNo")).Attributes.Add("onclick", "javascript:updateCashBankDetail('" + TradeDate.Text + "','" + ReferenceID.Text + "','" + MainID.Text + "','" + SubID.Text + "','" + CompID.Text + "','" + SegID.Text + "');");
                    e.Row.Cells[2].Style.Add("cursor", "hand");
                    e.Row.Cells[2].ToolTip = "Click to View Detail!";
                }
                else if (CashType.Text.Trim() == "Journal")
                {
                    if (ReferenceID.Text.Length > 2)
                    {
                        string gridForVoucherID = ReferenceID.Text.Substring(0, 2);
                        string gridForVoucher = ReferenceID.Text.Substring(0, 1);
                        Label lblDescrip = (Label)e.Row.FindControl("lblDescrip");
                        if (gridForVoucherID == "XO" || gridForVoucherID == "XP" || gridForVoucherID == "XZ" || gridForVoucherID == "XX" || gridForVoucherID == "XC")
                        {
                            Label lblTradeDate = (Label)e.Row.FindControl("lblTradeDate");
                            string[] Narration1;
                            string[] Bill1;
                            string Bill = "0";
                            string[] Narration = lblDescrip.Text.Split('[');
                            if (Narration.Length > 1)
                            {
                                Narration1 = Narration[1].Split(':');
                                if (Narration1.Length > 1)
                                {
                                    Bill1 = Narration1[1].Split(']');
                                    Bill = Bill1[0];
                                }
                            }
                            SegMentName = ViewState["SegMentName"].ToString();
                            ((Label)e.Row.FindControl("lblVoucherNo")).Attributes.Add("onclick", "javascript:ShowObligationBreakUp('" + Bill + "','" + gridForVoucherID + "','" + SegMentName + "','" + lblTradeDate.Text + "');");
                            e.Row.Cells[2].Style.Add("cursor", "hand");
                            e.Row.Cells[2].ToolTip = "Click to View Detail!";
                        }
                        else if (gridForVoucherID == "YF" || gridForVoucherID == "YG")
                        {
                            ((Label)e.Row.FindControl("lblVoucherNo")).Attributes.Add("onclick", "javascript:updateJournalDetail('" + TradeDate.Text + "','" + ReferenceID.Text + "','" + MainID.Text + "','" + SubID.Text + "','" + CompID.Text + "','" + SegID.Text + "');");
                            e.Row.Cells[2].Style.Add("cursor", "hand");
                            e.Row.Cells[2].ToolTip = "Click to View Detail!";
                        }
                        else if (gridForVoucher != "X" && gridForVoucher != "Y" && gridForVoucher != "Z")
                        {
                            ((Label)e.Row.FindControl("lblVoucherNo")).Attributes.Add("onclick", "javascript:updateJournalDetail('" + TradeDate.Text + "','" + ReferenceID.Text + "','" + MainID.Text + "','" + SubID.Text + "','" + CompID.Text + "','" + SegID.Text + "');");
                            e.Row.Cells[2].Style.Add("cursor", "hand");
                            e.Row.Cells[2].ToolTip = "Click to View Detail!";
                        }
                    }
                }
                Label TrDate = (Label)e.Row.FindControl("lblTrDate");
                string row1Text = TrDate.Text;
                if (row1Text == "a")
                {
                    if (SegMentName == "NSE-CM")
                    {
                        Label lblDescrip = (Label)e.Row.FindControl("lblDescrip");
                        Label lblValueDate = (Label)e.Row.FindControl("lblValueDate");
                        for (int j = 2 - 1; j >= 1; j += -1)
                        {
                            e.Row.Cells.RemoveAt(j);
                        }
                        e.Row.Cells[0].ColumnSpan = 2;
                        e.Row.Cells[0].Text = "Obligation Breakup for " + lblValueDate.Text;
                        for (int i = 5 - 1; i >= 1; i += -1)
                        {
                            e.Row.Cells.RemoveAt(i);
                        }
                        e.Row.Cells[1].ColumnSpan = 6;
                        e.Row.Cells[1].Text = lblDescrip.Text;
                        e.Row.Cells[2].Visible = false;
                        e.Row.Cells[3].Visible = false;
                        e.Row.Cells[4].Text = "";
                        e.Row.Cells[5].Text = "";
                    }
                    else
                    {
                        int m = e.Row.Cells.Count;
                        Label lblDescrip = (Label)e.Row.FindControl("lblDescrip");
                        for (int P = 6 - 1; P >= 1; P += -1)
                        {
                            e.Row.Cells.RemoveAt(P);
                        }
                        e.Row.Cells[0].ColumnSpan = 7;
                        e.Row.Cells[0].Text = lblDescrip.Text;
                        //e.Row.Cells[1].ColumnSpan = 3;
                        //e.Row.Cells[1].Text = "";
                        e.Row.Cells[1].Visible = false;
                        e.Row.Cells[2].Visible = false;
                        e.Row.Cells[3].Visible = false;
                        e.Row.Cells[4].Text = "";
                        e.Row.Cells[5].Text = "";
                    }
                }
            }
        }
        protected void grdCashBankBook_RowCreated(object sender, GridViewRowEventArgs e)
        {
            string rowID = String.Empty;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                rowID = "row" + e.Row.RowIndex;
                e.Row.Attributes.Add("id", "row" + e.Row.RowIndex);
                e.Row.Attributes.Add("onclick", "ChangeRowColor(" + "'" + rowID + "','" + dtCashBankBook.Rows.Count + "'" + ")");
            }

        }
        ////protected void NavigationLink_Click(Object sender, CommandEventArgs e)
        ////{
        ////    switch (e.CommandName)
        ////    {
        ////        case "First":
        ////            pageindex = 0;
        ////            break;
        ////        case "Next":
        ////            pageindex = int.Parse(CurrentPage.Value) + 1;
        ////            break;
        ////        case "Prev":
        ////            pageindex = int.Parse(CurrentPage.Value) - 1;
        ////            break;
        ////        case "Last":
        ////            pageindex = int.Parse(TotalPages.Value);
        ////            break;
        ////        default:
        ////            pageindex = int.Parse(e.CommandName.ToString());
        ////            break;
        ////    }
        ////    FillGrid();
        ////    string SpanText1 = oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "  - " + oconverter.ArrangeDate2(dtTo.Value.ToString());
        ////    ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct", "ShowHide('" + SpanText1 + "')", true);
        ////}
        ////protected void NavigationLinkC_Click(Object sender, CommandEventArgs e)
        ////{
        ////    int curentIndex = cmbclientsPager.SelectedIndex;
        ////    int totalNo = cmbclientsPager.Items.Count;
        ////    switch (e.CommandName)
        ////    {
        ////        case "First":
        ////            pageindex = 0;
        ////            break;
        ////        case "Next":
        ////            curentIndex = curentIndex + 1;
        ////            break;
        ////        case "Prev":
        ////            curentIndex = curentIndex - 1;
        ////            break;
        ////        case "Last":
        ////            pageindex = int.Parse(TotalClient.Value);
        ////            break;
        ////        default:
        ////            pageindex = int.Parse(e.CommandName.ToString());
        ////            break;
        ////    }
        ////    if (curentIndex >= totalNo)
        ////    {
        ////        curentIndex = totalNo - 1;
        ////        //Page.ClientScript.RegisterStartupScript(GetType(), "hide", "<script language='javascript'>DisableC('N');</script>");
        ////        ScriptManager.RegisterStartupScript(this, GetType(), "hide", "DisableC('N');", true);
        ////    }
        ////    else if (curentIndex <= 0)
        ////    {
        ////        curentIndex = 0;
        ////        //Page.ClientScript.RegisterStartupScript(GetType(), "hide", "<script language='javascript'>DisableC('P');</script>");
        ////        ScriptManager.RegisterStartupScript(this, GetType(), "hide", "DisableC('P');", true);
        ////    }
        ////    cmbclientsPager.SelectedIndex = curentIndex;
        ////    //MainAcID = "'" + cmbclientsPager.SelectedItem.Value.ToString() + "'";
        ////    //FillGrid();
        ////    ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct6", "HideOnOffLoading()", true);
        ////    FillGridForChanges();
        ////    string SpanText1 = oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "  - " + oconverter.ArrangeDate2(dtTo.Value.ToString());
        ////    ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct", "ShowHide('" + SpanText1 + "')", true);
        ////}
        protected void ButtonUpdate_Click(object sender, EventArgs e)
        {
            // FillDropDown();
        }


        protected void grdCashBankBook_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = e.SortExpression;
            if (GridViewSortDirection == SortDirection.Ascending)
            {
                GridViewSortDirection = SortDirection.Descending;
                SortGridView(sortExpression, " DESC");
            }
            else
            {
                GridViewSortDirection = SortDirection.Ascending;
                SortGridView(sortExpression, " ASC");
            }
        }
        public SortDirection GridViewSortDirection
        {

            get
            {

                if (ViewState["sortDirection"] == null)

                    ViewState["sortDirection"] = SortDirection.Ascending;

                return (SortDirection)ViewState["sortDirection"];

            }

            set { ViewState["sortDirection"] = value; }

        }
        private void SortGridView(string sortExpression, string direction)
        {

            // You can cache the DataTable for improving performance
            MainAcIDforOp = ViewState["MainAcIDforOp"].ToString();
            Segment = ViewState["Segment"].ToString();
            //dtLedgerView = (DataTable)ViewState["dtLedgerView"];
            //dtCashBankBook = (DataTable)ViewState["dtCashBankBook"];
            DataView foundRow = new DataView(dtCashBankBook);
            foundRow.RowFilter = " UserID=" + Session["userid"].ToString() + "";
            dtCashBankBook = foundRow.Table.Clone();
            foreach (DataRowView dvr in foundRow)
            {
                dtCashBankBook.ImportRow(dvr.Row);
            }
            dtCashBankBook.AcceptChanges();
            decimal receipt = 0;
            decimal Payment = 0;
            DataTable OpenBalance = oDbEngine.OpeningBalanceOnlyJournal(MainAcIDforOp, Request.QueryString["accno"].ToString(), Convert.ToDateTime(dtFrom.Value), Segment, Session["CompanyID"].ToString(), Convert.ToDateTime(dtTo.Value));
            System.Globalization.NumberFormatInfo currencyFormat = new System.Globalization.CultureInfo("hi-IN").NumberFormat;
            currencyFormat.CurrencySymbol = "";
            currencyFormat.CurrencyNegativePattern = 2;
            //DataTable dt = dtSubsidiary;
            DataView dv = new DataView(dtCashBankBook);
            dv.Sort = sortExpression + direction;
            grdCashBankBook.DataSource = dv;
            grdCashBankBook.DataBind();
            try
            {
                receipt = Convert.ToDecimal(dtCashBankBook.Compute("sum(Accountsledger_AmountCr)", ""));
            }
            catch
            {
                receipt = 0;
            }
            try
            {
                Payment = Convert.ToDecimal(dtCashBankBook.Compute("sum(Accountsledger_AmountDr)", ""));
            }
            catch
            {
                Payment = 0;
            }
            openingBal = openingBal + Convert.ToDecimal(OpenBalance.Rows[0][1].ToString());
            grdCashBankBook.FooterRow.Cells[3].Text = "Closing Balance";
            grdCashBankBook.FooterRow.Cells[11].Text = openingBal.ToString("c", currencyFormat);
            if (Payment != 0)
                grdCashBankBook.FooterRow.Cells[9].Text = Payment.ToString("c", currencyFormat);
            else
                grdCashBankBook.FooterRow.Cells[9].Text = "";
            if (receipt != 0)
                grdCashBankBook.FooterRow.Cells[10].Text = receipt.ToString("c", currencyFormat);
            else
                grdCashBankBook.FooterRow.Cells[10].Text = "";
            grdCashBankBook.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Left;
            grdCashBankBook.FooterRow.Cells[11].HorizontalAlign = HorizontalAlign.Right;
            grdCashBankBook.FooterRow.Cells[9].HorizontalAlign = HorizontalAlign.Right;
            grdCashBankBook.FooterRow.Cells[10].HorizontalAlign = HorizontalAlign.Right;
            grdCashBankBook.FooterRow.Cells[3].ForeColor = System.Drawing.Color.White;
            grdCashBankBook.FooterRow.Cells[9].ForeColor = System.Drawing.Color.White;
            grdCashBankBook.FooterRow.Cells[10].ForeColor = System.Drawing.Color.White;
            grdCashBankBook.FooterRow.Cells[11].ForeColor = System.Drawing.Color.White;
            grdCashBankBook.FooterRow.Cells[3].Font.Bold = true;
            grdCashBankBook.FooterRow.Cells[9].Font.Bold = true;
            grdCashBankBook.FooterRow.Cells[10].Font.Bold = true;
            grdCashBankBook.FooterRow.Cells[11].Font.Bold = true;
            grdCashBankBook.FooterRow.Cells[9].Wrap = false;
            grdCashBankBook.FooterRow.Cells[10].Wrap = false;
            grdCashBankBook.FooterRow.Cells[11].Wrap = false;


        }

        protected void ddlExport_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void ddlExport_SelectedIndexChanged1(object sender, EventArgs e)
        {
            if (ddlExport.SelectedItem.Value.ToString() == "Ex")
            {
                //Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");
            }
            else
            {
                Segment = ViewState["Segment"].ToString();
                MainAcIDforOp = ViewState["MainAcIDforOp"].ToString();
                SubAcID = HdnSubAc.Value;
                MainAcID = HdnMainAc.Value;
                SubLedgerType = HdnSubLedgerType.Value;
                if (ViewState["SubAcID"] != null)
                    SubAcID = ViewState["SubAcID"].ToString();
                if (ViewState["MainAcID"] != null)
                    MainAcID = ViewState["MainAcID"].ToString();
                if (ViewState["SubLedgerType"] != null)
                    SubLedgerType = ViewState["SubLedgerType"].ToString();

                //dtLedgerView = (DataTable)ViewState["dtLedgerView"];
                //dtCashBankBook = (DataTable)ViewState["dtCashBankBook"];
                DataTable CompanyName = oDbEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
                System.Globalization.NumberFormatInfo currencyFormat = new System.Globalization.CultureInfo("en-us").NumberFormat;
                currencyFormat.CurrencySymbol = "";
                currencyFormat.CurrencyNegativePattern = 2;
                decimal receipt = 0;
                decimal Payment = 0;
                decimal closingRate = 0;
                string CheckingValueParam = null;
                DataTable OpenBalance = new DataTable();
                DataTable dtCashBankBook1 = new DataTable();
                DataTable dtLedger = new DataTable();
                ////if (rdbSegAll.Checked == true)
                ////{
                ////    DataTable dtSegment = oDbEngine.GetDataTable("tbl_master_companyExchange", "exch_internalId", "exch_compId='" + Session["CompanyID"].ToString() + "'");
                ////    if (dtSegment.Rows.Count > 0)
                ////    {
                ////        for (int i = 0; i < dtSegment.Rows.Count; i++)
                ////        {
                ////            if (Segment == null)
                ////                Segment = dtSegment.Rows[i][0].ToString();
                ////            else
                ////                Segment += "," + dtSegment.Rows[i][0].ToString();
                ////        }
                ////    }
                ////}
                ////if (rdbbAll.Checked == true)
                ////{
                ////    Branch = Session["userbranchHierarchy"].ToString();
                ////}
                Branch = Session["userbranchHierarchy"].ToString();
                if (Segment == null)
                {
                    Segment = Session["SegmentID"].ToString();
                }
                string mainAccountSearch = null;
                string SubAccountSearch = null;

                mainAccountSearch = "and accountsledger_MainAccountID in(" + MainAcID + ")";
                ////for (int l = 0; l < cmbclientsPager.Items.Count; l++)
                ////{
                receipt = 0;
                Payment = 0;
                string valItem = Request.QueryString["accno"].ToString();
                if (SubLedgerType.Trim() == "None")
                {
                    SubAccountSearch = null;

                    dtCashBankBook1 = oDbEngine.GetDataTable("Trans_accountsledger a ", "convert(varchar(11),a.accountsledger_transactiondate,113) as TrDate,case when a.accountsledger_valuedate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),a.accountsledger_valuedate,113) end as ValueDate,a.accountsledger_TransactionReferenceID, isnull(a.accountsledger_Narration,'') +' - '+isnull(isnull((select MainAccount_Name from Master_MainAccount where MainAccount_AccountCode=AccountsLedger_CashBankName),AccountsLedger_CashBankName),'') as accountsledger_Narration,a.accountsledger_InstrumentNumber,(a.accountsledger_SettlementNumber+' '+a.accountsledger_SettlementType) as SettlementNumber,(select convert(varchar(12),Settlements_StartDateTime,113) from master_settlements where Settlements_Number=a.accountsledger_SettlementNumber and Settlements_TypeSuffix=a.accountsledger_SettlementType) as PayoutDate,(select branch_code from tbl_master_branch where branch_id=(select cnt_branchid from tbl_master_contact where cnt_internalID=accountsledger_SubAccountID)) as BranchCode,case when a.Accountsledger_AmountDr='0.00000000' then null else cast(a.Accountsledger_AmountDr as varchar(max)) end as Accountsledger_AmountCr,case when a.Accountsledger_AmountCr='0.00000000' then null else cast(a.Accountsledger_AmountCr as varchar(max)) end as Accountsledger_AmountDr,'0.0' as Closing", " accountsledger_companyID='" + Session["CompanyID"].ToString() + "' and accountsledger_ExchangeSegmentID in(" + Segment + ") and  cast(DATEADD(dd, 0, DATEDIFF(dd, 0,accountsledger_transactiondate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtFrom.Value + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtTo.Value + "')) as datetime)  " + mainAccountSearch + " and AccountsLedger_BranchID in(" + Branch + ") " + SubAccountSearch + "   and AccountsLedger_TransactionType<>'OpeningBalance'", " accountsledger_transactiondate,accountsledger_TransactionReferenceID");
                    OpenBalance = oDbEngine.OpeningBalanceOnlyJournal(MainAcIDforOp, null, Convert.ToDateTime(dtFrom.Value), Segment, Session["CompanyID"].ToString(), Convert.ToDateTime(dtTo.Value));



                }
                else
                {
                    SubAccountSearch = " and AccountsLedger_SubAccountID in('" + valItem + "') ";

                    dtCashBankBook1 = oDbEngine.GetDataTable("Trans_accountsledger a ", "convert(varchar(11),a.accountsledger_transactiondate,113) as TrDate,convert(varchar(11),a.accountsledger_valuedate,113) as ValueDate,a.accountsledger_TransactionReferenceID, isnull(a.accountsledger_Narration,'') +' - '+isnull(isnull((select MainAccount_Name from Master_MainAccount where MainAccount_AccountCode=AccountsLedger_CashBankName),AccountsLedger_CashBankName),'') as accountsledger_Narration,a.accountsledger_InstrumentNumber,(a.accountsledger_SettlementNumber+' '+a.accountsledger_SettlementType) as SettlementNumber,(select convert(varchar(12),Settlements_StartDateTime,113) from master_settlements where Settlements_Number=a.accountsledger_SettlementNumber and Settlements_TypeSuffix=a.accountsledger_SettlementType) as PayoutDate,(select branch_code from tbl_master_branch where branch_id=(select cnt_branchid from tbl_master_contact where cnt_internalID=accountsledger_SubAccountID)) as BranchCode,case when a.Accountsledger_AmountDr='0.00000000' then null else cast(a.Accountsledger_AmountDr as varchar(max)) end as Accountsledger_AmountCr,case when a.Accountsledger_AmountCr='0.00000000' then null else cast(a.Accountsledger_AmountCr as varchar(max)) end as Accountsledger_AmountDr,'0.0' as Closing", " accountsledger_companyID='" + Session["CompanyID"].ToString() + "' and accountsledger_ExchangeSegmentID in(" + Segment + ") and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,accountsledger_transactiondate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtFrom.Value + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtTo.Value + "')) as datetime)  " + mainAccountSearch + " and AccountsLedger_BranchID in(" + Branch + ") " + SubAccountSearch + "  and AccountsLedger_SubAccountID is not null and AccountsLedger_TransactionType<>'OpeningBalance'", " accountsledger_transactiondate,accountsledger_TransactionReferenceID");
                    OpenBalance = oDbEngine.OpeningBalanceOnlyJournal(MainAcIDforOp, valItem, Convert.ToDateTime(dtFrom.Value), Segment, Session["CompanyID"].ToString(), Convert.ToDateTime(dtTo.Value));



                }
                DataTable dtCashBankBook_New = dtCashBankBook1.Copy();
                dtCashBankBook_New.Rows.Clear();
                DataRow newRow = dtCashBankBook_New.NewRow();
                for (int j = 0; j < dtCashBankBook1.Rows.Count; j++)
                {
                    newRow = dtCashBankBook_New.NewRow();
                    newRow[0] = dtCashBankBook1.Rows[j]["TrDate"];
                    newRow[1] = dtCashBankBook1.Rows[j]["ValueDate"];
                    newRow[2] = dtCashBankBook1.Rows[j]["accountsledger_TransactionReferenceID"];
                    newRow[3] = dtCashBankBook1.Rows[j]["accountsledger_Narration"];
                    newRow[4] = dtCashBankBook1.Rows[j]["accountsledger_InstrumentNumber"];
                    newRow[5] = dtCashBankBook1.Rows[j]["SettlementNumber"];
                    newRow[6] = dtCashBankBook1.Rows[j]["PayoutDate"];
                    newRow[7] = dtCashBankBook1.Rows[j]["BranchCode"];
                    newRow[8] = dtCashBankBook1.Rows[j]["Accountsledger_AmountCr"];
                    newRow[9] = dtCashBankBook1.Rows[j]["Accountsledger_AmountDr"];
                    string Dr = "0";
                    string Cr = "0";
                    if (dtCashBankBook1.Rows[j]["Accountsledger_AmountDr"].ToString() != "")
                    {
                        Cr = dtCashBankBook1.Rows[j]["Accountsledger_AmountDr"].ToString();
                    }
                    if (dtCashBankBook1.Rows[j]["Accountsledger_AmountCr"].ToString() != "")
                        Dr = dtCashBankBook1.Rows[j]["Accountsledger_AmountCr"].ToString();
                    if (j == 0)
                    {
                        newRow[10] = decimal.Parse(Cr) - decimal.Parse(Dr) + (Convert.ToDecimal(OpenBalance.Rows[0][1].ToString()));
                        closingRate = decimal.Parse(Cr) - decimal.Parse(Dr) + (Convert.ToDecimal(OpenBalance.Rows[0][1].ToString()));

                    }
                    else
                    {
                        newRow[10] = decimal.Parse(Cr) - decimal.Parse(Dr) + closingRate;
                        closingRate = decimal.Parse(Cr) - decimal.Parse(Dr) + closingRate;
                    }
                    dtCashBankBook_New.Rows.Add(newRow);
                    if (dtCashBankBook1.Rows[j]["Accountsledger_AmountDr"].ToString() != "")
                        receipt += Convert.ToDecimal(dtCashBankBook1.Rows[j]["Accountsledger_AmountDr"].ToString());
                    if (dtCashBankBook1.Rows[j]["Accountsledger_AmountCr"].ToString() != "")
                        Payment += Convert.ToDecimal(dtCashBankBook1.Rows[j]["Accountsledger_AmountCr"].ToString());

                }
                dtCashBankBook1.Rows.Clear();
                dtCashBankBook1 = dtCashBankBook_New.Copy();
                DataTable dtCashBankBook_New1 = dtCashBankBook1.Copy();
                dtCashBankBook_New1.Rows.Clear();
                string Type = "";
                DataRow newRow5 = dtCashBankBook_New1.NewRow();

                if (Request.QueryString["seg"].ToString() == "n")
                {
                    Type = "NSDL Clients A/c";
                }
                else if (Request.QueryString["seg"].ToString() == "c")
                {
                    Type = "CDSL Clients A/c";
                }

                ////**CheckingValueParam = Type + ": " + cmbclientsPager.Items[0].Text + " " + "Period :" + " " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "  - " + oconverter.ArrangeDate2(dtTo.Value.ToString());
                ////**newRow5[0] = Type + ": " + cmbclientsPager.Items[0].Text + " " + "Period :" + " " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "  - " + oconverter.ArrangeDate2(dtTo.Value.ToString());

                CheckingValueParam = Type + " " + "Period :" + " " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "  - " + oconverter.ArrangeDate2(dtTo.Value.ToString());
                newRow5[0] = Type + " " + "Period :" + " " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "  - " + oconverter.ArrangeDate2(dtTo.Value.ToString());
                newRow5[1] = "Test";
                dtCashBankBook_New1.Rows.Add(newRow5);
                DataRow newRow1 = dtCashBankBook_New1.NewRow();
                newRow1[1] = oconverter.ArrangeDate2(Convert.ToDateTime(dtFrom.Value).ToShortDateString());
                newRow1[3] = "Opening Balance";
                if (Convert.ToDecimal(OpenBalance.Rows[0][1].ToString()) < 0)
                {
                    newRow1[8] = Convert.ToDecimal(OpenBalance.Rows[0][1].ToString()) * (-1);
                    Payment += Convert.ToDecimal(OpenBalance.Rows[0][1].ToString()) * (-1);
                    newRow1[10] = Convert.ToDecimal(OpenBalance.Rows[0][1].ToString());
                }
                else
                {
                    newRow1[9] = Convert.ToDecimal(OpenBalance.Rows[0][1].ToString());
                    receipt += Convert.ToDecimal(OpenBalance.Rows[0][1].ToString());
                    newRow1[10] = Convert.ToDecimal(OpenBalance.Rows[0][1].ToString());
                }
                dtCashBankBook_New1.Rows.Add(newRow1);
                for (int i = 0; i < dtCashBankBook1.Rows.Count; i++)
                {
                    newRow1 = dtCashBankBook_New1.NewRow();
                    newRow1[0] = dtCashBankBook1.Rows[i]["TrDate"];
                    newRow1[1] = dtCashBankBook1.Rows[i]["ValueDate"];
                    newRow1[2] = dtCashBankBook1.Rows[i]["accountsledger_TransactionReferenceID"];
                    newRow1[3] = dtCashBankBook1.Rows[i]["accountsledger_Narration"];
                    newRow1[4] = dtCashBankBook1.Rows[i]["accountsledger_InstrumentNumber"];
                    newRow1[5] = dtCashBankBook1.Rows[i]["SettlementNumber"];
                    newRow1[6] = dtCashBankBook1.Rows[i]["PayoutDate"];
                    newRow1[7] = dtCashBankBook1.Rows[i]["BranchCode"];
                    newRow1[8] = dtCashBankBook1.Rows[i]["Accountsledger_AmountCr"];
                    newRow1[9] = dtCashBankBook1.Rows[i]["Accountsledger_AmountDr"];
                    newRow1[10] = dtCashBankBook1.Rows[i]["Closing"];
                    dtCashBankBook_New1.Rows.Add(newRow1);
                    if (dtCashBankBook1.Rows[i]["Closing"].ToString() != "")
                        openingBal = decimal.Parse(dtCashBankBook1.Rows[i]["Closing"].ToString());
                }
                dtCashBankBook1.Rows.Clear();
                dtCashBankBook1 = dtCashBankBook_New1.Copy();
                DataRow DrRow1 = dtCashBankBook1.NewRow();
                dtCashBankBook1.Rows.Add(DrRow1);
                DataRow DrRow = dtCashBankBook1.NewRow();
                DrRow[3] = "Closing Balance";
                if (dtCashBankBook1.Rows.Count == 0)
                {
                    openingBal = Convert.ToDecimal(OpenBalance.Rows[0][1].ToString());
                }
                DrRow[10] = openingBal.ToString("c", currencyFormat);
                if (receipt != 0)
                    DrRow[9] = receipt.ToString("c", currencyFormat);

                if (Payment != 0)
                    DrRow[8] = Payment.ToString("c", currencyFormat);

                dtCashBankBook1.Rows.Add(DrRow);
                dtCashBankBook1.AcceptChanges();
                if (dtCashBankBook1.Rows.Count > 0)
                {
                    for (int k = 0; k < dtCashBankBook1.Rows.Count; k++)
                    {
                        if (dtCashBankBook1.Rows[k]["Accountsledger_AmountDr"].ToString() != "")
                            dtCashBankBook1.Rows[k]["Accountsledger_AmountDr"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dtCashBankBook1.Rows[k]["Accountsledger_AmountDr"].ToString()));
                        if (dtCashBankBook1.Rows[k]["Accountsledger_AmountCr"].ToString() != "")
                            dtCashBankBook1.Rows[k]["Accountsledger_AmountCr"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dtCashBankBook1.Rows[k]["Accountsledger_AmountCr"].ToString()));
                        if (dtCashBankBook1.Rows[k]["Closing"].ToString() != "")
                        {
                            dtCashBankBook1.Rows[k]["Closing"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dtCashBankBook1.Rows[k]["Closing"].ToString()));

                        }
                    }
                }
                ////if (cmbclientsPager.Items.Count == 1)
                ////{
                ////    dtLedger = dtCashBankBook1.Copy();
                ////}
                ////else
                ////{
                ////    if (l == 0)
                ////    {
                ////        dtLedger = dtCashBankBook1.Copy();
                ////    }
                ////    else
                ////    {
                ////        if (l == cmbclientsPager.Items.Count - 1)
                ////        {
                ////            dtLedger.Merge(dtCashBankBook1);
                ////        }
                ////        else
                ////        {
                ////            dtLedger.Merge(dtCashBankBook1);
                ////        }
                ////    }
                ////}
                dtLedger = dtCashBankBook1.Copy();

                ////}

                ////if (cmbclientsPager.Items.Count == 0)
                ////{
                ////    receipt = 0;
                ////    Payment = 0;
                ////    SubAccountSearch = null;
                ////    string Sub = null;
                ////    if (SubAcID != null && SubAcID != "")
                ////    {
                ////        SubAccountSearch = " and AccountsLedger_SubAccountID in(" + SubAcID + ") ";
                ////        Sub = SubAcID.Replace("'", "");
                ////    }
                ////    //dtCashBankBook1 = oDbEngine.GetDataTable("Trans_accountsledger a ", "convert(varchar(11),a.accountsledger_transactiondate,113) as TrDate,convert(varchar(11),a.accountsledger_valuedate,113) as ValueDate,a.accountsledger_TransactionReferenceID, a.accountsledger_Narration,a.accountsledger_InstrumentNumber,(a.accountsledger_SettlementNumber+' '+a.accountsledger_SettlementType) as SettlementNumber,case when a.Accountsledger_AmountDr='0.00000000' then null else cast(a.Accountsledger_AmountDr as varchar(max)) end as Accountsledger_AmountCr,case when a.Accountsledger_AmountCr='0.00000000' then null else cast(a.Accountsledger_AmountCr as varchar(max)) end as Accountsledger_AmountDr,'0.0' as Closing", " accountsledger_companyID='" + Session["CompanyID"].ToString() + "' and accountsledger_ExchangeSegmentID in(" + Segment + ") and  cast(DATEADD(dd, 0, DATEDIFF(dd, 0,accountsledger_transactiondate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtFrom.Value + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtTo.Value + "')) as datetime)  " + mainAccountSearch + " and AccountsLedger_BranchID in(" + Branch + ") " + SubAccountSearch + "   and AccountsLedger_TransactionType<>'OpeningBalance'", " accountsledger_transactiondate,accountsledger_TransactionReferenceID");
                ////    ////if (radConsolidated.Checked == true)
                ////    ////{
                ////    ////    if (radDateWise.Checked == true)
                ////    ////    {
                ////    ////        dtCashBankBook1 = oDbEngine.GetDataTable("(Select convert(varchar(11),a.accountsledger_transactiondate,113) as TrDate,case when a.accountsledger_valuedate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),a.accountsledger_valuedate,113) end as ValueDate,case when a.AccountsLedger_TransactionType='Journal' and Left(AccountsLedger_TransactionReferenceID,1) in('X','Y','Z') and accountsledger_SettlementNumber is not null and accountsledger_SettlementNumber<>''	then 'Consolidated Entries For '+convert(varchar(11),a.accountsledger_transactiondate,113) 	else isnull(a.accountsledger_Narration,'') end +' - '+isnull(isnull((select MainAccount_Name from Master_MainAccount where MainAccount_AccountCode=AccountsLedger_CashBankName),AccountsLedger_CashBankName),'') as accountsledger_Narration,a.accountsledger_InstrumentNumber,(a.accountsledger_SettlementNumber+' '+a.accountsledger_SettlementType) as SettlementNumber,(select convert(varchar(12),Settlements_StartDateTime,113) from master_settlements where Settlements_Number=a.accountsledger_SettlementNumber and Settlements_TypeSuffix=a.accountsledger_SettlementType) as PayoutDate,(select branch_code from tbl_master_branch where branch_id=(select cnt_branchid from tbl_master_contact where cnt_internalID=accountsledger_SubAccountID)) as BranchCode,case when a.Accountsledger_AmountDr='0.00000000' then null else a.Accountsledger_AmountDr end as Accountsledger_AmountCr,case when a.Accountsledger_AmountCr='0.00000000' then null else a.Accountsledger_AmountCr end as Accountsledger_AmountDr,'0.0' as Closing,accountsledger_transactiondate from Trans_accountsledger a  WHERE  accountsledger_companyID='" + Session["CompanyID"].ToString() + "' and accountsledger_ExchangeSegmentID in(" + Segment + ") and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,accountsledger_transactiondate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtFrom.Value + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtTo.Value + "')) as datetime)   " + mainAccountSearch + " and AccountsLedger_BranchID in(" + Branch + ")  " + SubAccountSearch + "  and AccountsLedger_TransactionType<>'OpeningBalance' ) as D ", " TrDate,ValueDate, 0 as accountsledger_TransactionReferenceID,accountsledger_Narration,accountsledger_InstrumentNumber,SettlementNumber,PayoutDate,BranchCode,case when (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0)))<0 then cast((sum(isnull(Accountsledger_AmountCr,0))-sum(isnull(Accountsledger_AmountDr,0))) as varchar(max)) else null end Accountsledger_AmountCr,case when (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0)))>0 then cast((sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0))) as varchar(max)) else null end Accountsledger_AmountDr,Closing", null, " TrDate,ValueDate,accountsledger_InstrumentNumber,SettlementNumber,PayoutDate,BranchCode,Closing,accountsledger_transactiondate,accountsledger_Narration", " accountsledger_transactiondate");
                ////    ////    }
                ////    ////    else
                ////    ////        dtCashBankBook1 = oDbEngine.GetDataTable("(Select convert(varchar(11),a.accountsledger_transactiondate,113) as TrDate,case when a.accountsledger_valuedate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),a.accountsledger_valuedate,113) end as ValueDate,isnull((select top 1 journalvoucher_Narration from trans_journalvoucher where journalvoucher_voucherNumber=a.accountsledger_TransactionReferenceID and journalvoucher_companyID=a.accountsledger_companyID and journalvoucher_ExchangeSegmentID=a.accountsledger_ExchangeSegmentID and journalvoucher_TransactionDate=a.accountsledger_transactiondate),isnull(accountsledger_Narration,'')) +' - '+isnull(isnull((select MainAccount_Name from Master_MainAccount where MainAccount_AccountCode=AccountsLedger_CashBankName),AccountsLedger_CashBankName),'') as accountsledger_Narration,a.accountsledger_InstrumentNumber,(a.accountsledger_SettlementNumber+' '+a.accountsledger_SettlementType) as SettlementNumber,(select convert(varchar(12),Settlements_StartDateTime,113) from master_settlements where Settlements_Number=a.accountsledger_SettlementNumber and Settlements_TypeSuffix=a.accountsledger_SettlementType) as PayoutDate,(select branch_code from tbl_master_branch where branch_id=(select cnt_branchid from tbl_master_contact where cnt_internalID=accountsledger_SubAccountID)) as BranchCode,case when a.Accountsledger_AmountDr='0.00000000' then null else a.Accountsledger_AmountDr end as Accountsledger_AmountCr,case when a.Accountsledger_AmountCr='0.00000000' then null else a.Accountsledger_AmountCr end as Accountsledger_AmountDr,'0.0' as Closing,accountsledger_transactiondate from Trans_accountsledger a  WHERE  accountsledger_companyID='" + Session["CompanyID"].ToString() + "' and accountsledger_ExchangeSegmentID in(" + Segment + ") and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,accountsledger_transactiondate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtFrom.Value + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtTo.Value + "')) as datetime)   " + mainAccountSearch + " and AccountsLedger_BranchID in(" + Branch + ")  " + SubAccountSearch + "  and AccountsLedger_TransactionType<>'OpeningBalance' ) as D ", " TrDate,ValueDate, 0 as accountsledger_TransactionReferenceID,accountsledger_Narration,accountsledger_InstrumentNumber,SettlementNumber,PayoutDate,BranchCode,case when (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0)))<0 then cast((sum(isnull(Accountsledger_AmountCr,0))-sum(isnull(Accountsledger_AmountDr,0))) as varchar(max)) else null end Accountsledger_AmountCr,case when (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0)))>0 then cast((sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0))) as varchar(max)) else null end Accountsledger_AmountDr,Closing", null, " TrDate,ValueDate,accountsledger_InstrumentNumber,SettlementNumber,PayoutDate,BranchCode,Closing,accountsledger_transactiondate,accountsledger_Narration", " accountsledger_transactiondate");
                ////    ////}
                ////    ////else
                ////    ////    dtCashBankBook1 = oDbEngine.GetDataTable("Trans_accountsledger a ", "convert(varchar(11),a.accountsledger_transactiondate,113) as TrDate,case when a.accountsledger_valuedate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),a.accountsledger_valuedate,113) end as ValueDate,a.accountsledger_TransactionReferenceID, isnull(a.accountsledger_Narration,'') +' - '+isnull(isnull((select MainAccount_Name from Master_MainAccount where MainAccount_AccountCode=AccountsLedger_CashBankName),AccountsLedger_CashBankName),'') as accountsledger_Narration,a.accountsledger_InstrumentNumber,(a.accountsledger_SettlementNumber+' '+a.accountsledger_SettlementType) as SettlementNumber,(select convert(varchar(12),Settlements_StartDateTime,113) from master_settlements where Settlements_Number=a.accountsledger_SettlementNumber and Settlements_TypeSuffix=a.accountsledger_SettlementType) as PayoutDate,(select branch_code from tbl_master_branch where branch_id=(select cnt_branchid from tbl_master_contact where cnt_internalID=accountsledger_SubAccountID)) as BranchCode,case when a.Accountsledger_AmountDr='0.00000000' then null else cast(a.Accountsledger_AmountDr as varchar(max)) end as Accountsledger_AmountCr,case when a.Accountsledger_AmountCr='0.00000000' then null else cast(a.Accountsledger_AmountCr as varchar(max)) end as Accountsledger_AmountDr,'0.0' as Closing", " accountsledger_companyID='" + Session["CompanyID"].ToString() + "' and accountsledger_ExchangeSegmentID in(" + Segment + ") and  cast(DATEADD(dd, 0, DATEDIFF(dd, 0,accountsledger_transactiondate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtFrom.Value + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtTo.Value + "')) as datetime)  " + mainAccountSearch + " and AccountsLedger_BranchID in(" + Branch + ") " + SubAccountSearch + "   and AccountsLedger_TransactionType<>'OpeningBalance'", " accountsledger_transactiondate,accountsledger_TransactionReferenceID");

                ////    dtCashBankBook1 = oDbEngine.GetDataTable("Trans_accountsledger a ", "convert(varchar(11),a.accountsledger_transactiondate,113) as TrDate,case when a.accountsledger_valuedate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),a.accountsledger_valuedate,113) end as ValueDate,a.accountsledger_TransactionReferenceID, isnull(a.accountsledger_Narration,'') +' - '+isnull(isnull((select MainAccount_Name from Master_MainAccount where MainAccount_AccountCode=AccountsLedger_CashBankName),AccountsLedger_CashBankName),'') as accountsledger_Narration,a.accountsledger_InstrumentNumber,(a.accountsledger_SettlementNumber+' '+a.accountsledger_SettlementType) as SettlementNumber,(select convert(varchar(12),Settlements_StartDateTime,113) from master_settlements where Settlements_Number=a.accountsledger_SettlementNumber and Settlements_TypeSuffix=a.accountsledger_SettlementType) as PayoutDate,(select branch_code from tbl_master_branch where branch_id=(select cnt_branchid from tbl_master_contact where cnt_internalID=accountsledger_SubAccountID)) as BranchCode,case when a.Accountsledger_AmountDr='0.00000000' then null else cast(a.Accountsledger_AmountDr as varchar(max)) end as Accountsledger_AmountCr,case when a.Accountsledger_AmountCr='0.00000000' then null else cast(a.Accountsledger_AmountCr as varchar(max)) end as Accountsledger_AmountDr,'0.0' as Closing", " accountsledger_companyID='" + Session["CompanyID"].ToString() + "' and accountsledger_ExchangeSegmentID in(" + Segment + ") and  cast(DATEADD(dd, 0, DATEDIFF(dd, 0,accountsledger_transactiondate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtFrom.Value + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtTo.Value + "')) as datetime)  " + mainAccountSearch + " and AccountsLedger_BranchID in(" + Branch + ") " + SubAccountSearch + "   and AccountsLedger_TransactionType<>'OpeningBalance'", " accountsledger_transactiondate,accountsledger_TransactionReferenceID");


                ////    OpenBalance = oDbEngine.OpeningBalanceOnlyJournal(MainAcIDforOp, Sub, Convert.ToDateTime(dtFrom.Value), Segment, Session["CompanyID"].ToString(), Convert.ToDateTime(dtTo.Value));
                ////    DataTable dtCashBankBook_New = dtCashBankBook1.Copy();
                ////    dtCashBankBook_New.Rows.Clear();
                ////    DataRow newRow = dtCashBankBook_New.NewRow();
                ////    for (int j = 0; j < dtCashBankBook1.Rows.Count; j++)
                ////    {
                ////        newRow = dtCashBankBook_New.NewRow();
                ////        newRow[0] = dtCashBankBook1.Rows[j]["TrDate"];
                ////        newRow[1] = dtCashBankBook1.Rows[j]["ValueDate"];
                ////        newRow[2] = dtCashBankBook1.Rows[j]["accountsledger_TransactionReferenceID"];
                ////        newRow[3] = dtCashBankBook1.Rows[j]["accountsledger_Narration"];
                ////        newRow[4] = dtCashBankBook1.Rows[j]["accountsledger_InstrumentNumber"];
                ////        newRow[5] = dtCashBankBook1.Rows[j]["SettlementNumber"];
                ////        newRow[6] = dtCashBankBook1.Rows[j]["PayoutDate"];
                ////        newRow[7] = dtCashBankBook1.Rows[j]["BranchCode"];
                ////        newRow[8] = dtCashBankBook1.Rows[j]["Accountsledger_AmountCr"];
                ////        newRow[9] = dtCashBankBook1.Rows[j]["Accountsledger_AmountDr"];
                ////        string Dr = "0";
                ////        string Cr = "0";
                ////        if (dtCashBankBook1.Rows[j]["Accountsledger_AmountDr"].ToString() != "")
                ////        {
                ////            Cr = dtCashBankBook1.Rows[j]["Accountsledger_AmountDr"].ToString();
                ////        }
                ////        if (dtCashBankBook1.Rows[j]["Accountsledger_AmountCr"].ToString() != "")
                ////            Dr = dtCashBankBook1.Rows[j]["Accountsledger_AmountCr"].ToString();
                ////        if (j == 0)
                ////        {
                ////            newRow[10] = decimal.Parse(Cr) - decimal.Parse(Dr) + (Convert.ToDecimal(OpenBalance.Rows[0][1].ToString()));
                ////            closingRate = decimal.Parse(Cr) - decimal.Parse(Dr) + (Convert.ToDecimal(OpenBalance.Rows[0][1].ToString()));

                ////        }
                ////        else
                ////        {
                ////            newRow[10] = decimal.Parse(Cr) - decimal.Parse(Dr) + closingRate;
                ////            closingRate = decimal.Parse(Cr) - decimal.Parse(Dr) + closingRate;
                ////        }
                ////        dtCashBankBook_New.Rows.Add(newRow);
                ////        if (dtCashBankBook1.Rows[j]["Accountsledger_AmountDr"].ToString() != "")
                ////            receipt += Convert.ToDecimal(dtCashBankBook1.Rows[j]["Accountsledger_AmountDr"].ToString());
                ////        if (dtCashBankBook1.Rows[j]["Accountsledger_AmountCr"].ToString() != "")
                ////            Payment += Convert.ToDecimal(dtCashBankBook1.Rows[j]["Accountsledger_AmountCr"].ToString());

                ////    }
                ////    dtCashBankBook1.Rows.Clear();
                ////    dtCashBankBook1 = dtCashBankBook_New.Copy();
                ////    DataTable dtCashBankBook_New1 = dtCashBankBook1.Copy();
                ////    dtCashBankBook_New1.Rows.Clear();
                ////    string Type = "";
                ////    DataRow newRow5 = dtCashBankBook_New1.NewRow();
                ////    //if (ddlAccountType.SelectedItem.Value == "0")
                ////    //{
                ////    //    Type = "Clients - Trading A/c  ";
                ////    //}
                ////    //else if (ddlAccountType.SelectedItem.Value == "1")
                ////    //{
                ////    //    Type = "Clients - Margin Deposit A/c  ";
                ////    //}
                ////    //else if (ddlAccountType.SelectedItem.Value == "2")
                ////    //{
                ////    //    Type = "Clients - Trading A/c and Clients - Margin Deposit A/c  ";
                ////    //}
                ////    //else if (ddlAccountType.SelectedItem.Value == "3")
                ////    //{
                ////    //    Type = txtMainAccount.Text;
                ////    //}

                ////    Type = "NSDL Clients A/c";

                ////    CheckingValueParam = Type + ": Period :" + " " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "  - " + oconverter.ArrangeDate2(dtTo.Value.ToString());
                ////    newRow5[0] = Type + ": Period :" + " " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "  - " + oconverter.ArrangeDate2(dtTo.Value.ToString());
                ////    newRow5[1] = "Test";
                ////    dtCashBankBook_New1.Rows.Add(newRow5);
                ////    DataRow newRow1 = dtCashBankBook_New1.NewRow();
                ////    newRow1[1] = oconverter.ArrangeDate2(Convert.ToDateTime(dtFrom.Value).ToShortDateString());
                ////    newRow1[3] = "Opening Balance";
                ////    if (Convert.ToDecimal(OpenBalance.Rows[0][1].ToString()) < 0)
                ////    {
                ////        newRow1[8] = Convert.ToDecimal(OpenBalance.Rows[0][1].ToString()) * (-1);
                ////        Payment += Convert.ToDecimal(OpenBalance.Rows[0][1].ToString()) * (-1);
                ////        newRow1[10] = Convert.ToDecimal(OpenBalance.Rows[0][1].ToString());
                ////    }
                ////    else
                ////    {
                ////        newRow1[9] = Convert.ToDecimal(OpenBalance.Rows[0][1].ToString());
                ////        receipt += Convert.ToDecimal(OpenBalance.Rows[0][1].ToString());
                ////        newRow1[10] = Convert.ToDecimal(OpenBalance.Rows[0][1].ToString());
                ////    }
                ////    dtCashBankBook_New1.Rows.Add(newRow1);
                ////    for (int i = 0; i < dtCashBankBook1.Rows.Count; i++)
                ////    {
                ////        newRow1 = dtCashBankBook_New1.NewRow();
                ////        newRow1[0] = dtCashBankBook1.Rows[i]["TrDate"];
                ////        newRow1[1] = dtCashBankBook1.Rows[i]["ValueDate"];
                ////        newRow1[2] = dtCashBankBook1.Rows[i]["accountsledger_TransactionReferenceID"];
                ////        newRow1[3] = dtCashBankBook1.Rows[i]["accountsledger_Narration"];
                ////        newRow1[4] = dtCashBankBook1.Rows[i]["accountsledger_InstrumentNumber"];
                ////        newRow1[5] = dtCashBankBook1.Rows[i]["SettlementNumber"];
                ////        newRow1[6] = dtCashBankBook1.Rows[i]["PayoutDate"];
                ////        newRow1[7] = dtCashBankBook1.Rows[i]["BranchCode"];
                ////        newRow1[8] = dtCashBankBook1.Rows[i]["Accountsledger_AmountCr"];
                ////        newRow1[9] = dtCashBankBook1.Rows[i]["Accountsledger_AmountDr"];
                ////        newRow1[10] = dtCashBankBook1.Rows[i]["Closing"];
                ////        dtCashBankBook_New1.Rows.Add(newRow1);
                ////        if (dtCashBankBook1.Rows[i]["Closing"].ToString() != "")
                ////            openingBal = decimal.Parse(dtCashBankBook1.Rows[i]["Closing"].ToString());

                ////    }
                ////    dtCashBankBook1.Rows.Clear();
                ////    dtCashBankBook1 = dtCashBankBook_New1.Copy();
                ////    DataRow DrRow1 = dtCashBankBook1.NewRow();
                ////    dtCashBankBook1.Rows.Add(DrRow1);
                ////    DataRow DrRow = dtCashBankBook1.NewRow();
                ////    DrRow[3] = "Closing Balance";
                ////    if (dtCashBankBook1.Rows.Count == 0)
                ////    {
                ////        openingBal = Convert.ToDecimal(OpenBalance.Rows[0][1].ToString());
                ////    }
                ////    DrRow[10] = openingBal.ToString("c", currencyFormat);
                ////    if (receipt != 0)
                ////        DrRow[9] = receipt.ToString("c", currencyFormat);

                ////    if (Payment != 0)
                ////        DrRow[8] = Payment.ToString("c", currencyFormat);

                ////    dtCashBankBook1.Rows.Add(DrRow);
                ////    dtCashBankBook1.AcceptChanges();
                ////    if (dtCashBankBook1.Rows.Count > 0)
                ////    {
                ////        for (int k = 0; k < dtCashBankBook1.Rows.Count; k++)
                ////        {
                ////            if (dtCashBankBook1.Rows[k]["Accountsledger_AmountDr"].ToString() != "")
                ////                dtCashBankBook1.Rows[k]["Accountsledger_AmountDr"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dtCashBankBook1.Rows[k]["Accountsledger_AmountDr"].ToString()));
                ////            if (dtCashBankBook1.Rows[k]["Accountsledger_AmountCr"].ToString() != "")
                ////                dtCashBankBook1.Rows[k]["Accountsledger_AmountCr"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dtCashBankBook1.Rows[k]["Accountsledger_AmountCr"].ToString()));
                ////            if (dtCashBankBook1.Rows[k]["Closing"].ToString() != "")
                ////            {
                ////                dtCashBankBook1.Rows[k]["Closing"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dtCashBankBook1.Rows[k]["Closing"].ToString()));

                ////            }
                ////        }
                ////    }
                ////    dtLedger = dtCashBankBook1.Copy();
                ////}

                DataTable dtReportHeader = new DataTable();
                dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
                DataRow HeaderRow = dtReportHeader.NewRow();
                HeaderRow[0] = CompanyName.Rows[0][0].ToString();
                dtReportHeader.Rows.Add(HeaderRow);
                DataRow DrRowR1 = dtReportHeader.NewRow();
                DrRowR1[0] = "Ledger For the  Period [" + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + " To " + oconverter.ArrangeDate2(dtTo.Value.ToString()) + "]";
                dtReportHeader.Rows.Add(DrRowR1);
                DataRow HeaderRow1 = dtReportHeader.NewRow();
                dtReportHeader.Rows.Add(HeaderRow1);
                DataRow HeaderRow2 = dtReportHeader.NewRow();
                dtReportHeader.Rows.Add(HeaderRow2);

                DataTable dtReportFooter = new DataTable();
                dtReportFooter.Columns.Add(new DataColumn("Footer", typeof(String))); //0
                DataRow FooterRow1 = dtReportFooter.NewRow();
                dtReportFooter.Rows.Add(FooterRow1);
                DataRow FooterRow2 = dtReportFooter.NewRow();
                dtReportFooter.Rows.Add(FooterRow2);
                DataRow FooterRow = dtReportFooter.NewRow();
                //FooterRow[0] = "* * *  End Of Report * * *         [" + oconverter.ArrangeDate2(oDBEngine.GetDate().ToString(), "Test") + "]";
                FooterRow[0] = "* * *  End Of Report * * *   ";
                dtReportFooter.Rows.Add(FooterRow);

                DataTable dtExport = new DataTable();
                dtExport = dtLedger.Copy();
                dtExport.Columns[2].ColumnName = "Voucher No.";
                dtExport.Columns[3].ColumnName = "Description";
                dtExport.Columns[4].ColumnName = "Instrument No.";
                dtExport.Columns[5].ColumnName = "Settlement No.";
                dtExport.Columns[6].ColumnName = "Trade Date";
                dtExport.Columns[7].ColumnName = "Branch Code";
                dtExport.Columns[8].ColumnName = "Debit";
                dtExport.Columns[9].ColumnName = "Credit";
                ////if (radConsolidated.Checked == true)
                ////    dtExport.Columns.Remove("Voucher No.");
                if (Session["userlastsegment"].ToString() != "7")
                    dtExport.Columns.Remove("Trade Date");
                if (Session["userlastsegment"].ToString() != "7" && Session["userlastsegment"].ToString() != "8")
                    dtExport.Columns.Remove("Branch Code");
                dtExport.AcceptChanges();
                if (ddlExport.SelectedItem.Value == "E")
                {
                    objExcel.ExportToExcelforExcel(dtExport, "Ledger", "Closing Balance", dtReportHeader, dtReportFooter);
                    //objExcel.ExportToExcelforExcel1(dtExport, "Ledger", "Closing Balance", dtReportHeader, dtReportFooter);

                }
                else if (ddlExport.SelectedItem.Value == "P")
                {
                    objExcel.ExportToPDF(dtExport, "Ledger", "Closing Balance", dtReportHeader, dtReportFooter);
                }
            }
        }
        protected void btnEmail_Click(object sender, EventArgs e)
        {
            MainAcIDforOp = ViewState["MainAcIDforOp"].ToString();
            //dtLedgerView = (DataTable)ViewState["dtLedgerView"];
            //dtCashBankBook = (DataTable)ViewState["dtCashBankBook"];
            Segment = ViewState["Segment"].ToString();
            decimal closing = 0;
            this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript3", "<script>ForFilterOff();</script>");
            DataTable dtCL = (DataTable)Session["Client"];
            ////for (int n = 0; n < dtCL.Rows.Count; n++)
            ////{
            ////cmbclientsPager.SelectedItem.Value = dtCL.Rows[n]["subaccount_code"].ToString();
            ////cmbclientsPager.SelectedItem.Text = dtCL.Rows[n]["subaccount_name"].ToString();
            FillGrid();
            DataTable opBal = oDbEngine.OpeningBalanceOnlyJournal(MainAcIDforOp, null, Convert.ToDateTime(dtFrom.Value), Segment, Session["CompanyID"].ToString(), Convert.ToDateTime(dtTo.Value));
            DataTable dtTbl = dtCashBankBook;

            decimal totaldebit = 0;
            decimal totalcredit = 0;
            decimal totalclosing = 0;
            string disptbl = "";
            decimal debit = 0;
            decimal credit = 0;

            disptbl = "<table width=\"1050px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"1\"><tr style=\"background-color:#BB694D;color:White;\"><td>Tr. Date</td><td>ValueDate</td><td>Voucher No.</td><td>Description</td><td>Instrument No</td><td>Settlement No</td><td>Debit</td><td>Credit</td><td>Closing</td></tr>";
            for (int j = 0; j < dtTbl.Rows.Count; j++)
            {
                if (dtTbl.Rows[j]["Accountsledger_AmountDr"].ToString() == "")
                {
                    debit = 0;
                }
                else
                {
                    debit = Convert.ToDecimal(dtTbl.Rows[j]["Accountsledger_AmountDr"].ToString());
                }
                if (dtTbl.Rows[j]["Accountsledger_AmountCr"].ToString() == "")
                {
                    credit = 0;

                }
                else
                {
                    credit = Convert.ToDecimal(dtTbl.Rows[j]["Accountsledger_AmountCr"].ToString());
                }
                decimal opbl = Convert.ToDecimal(opBal.Rows[0]["op"].ToString());

                if (j == 0)
                {
                    closing = (opbl - debit) + credit;
                }
                else
                {
                    closing = (closing - debit) + credit;
                }
                string dbt = oconverter.formatmoneyinUs(debit);
                string crdt = oconverter.formatmoneyinUs(credit);
                if (dbt == "0.00")
                {
                    dbt = "";
                }
                if (crdt == "0.00")
                {
                    crdt = "";
                }
                totaldebit = totaldebit + debit;
                totalcredit = totalcredit + credit;
                totalclosing = totalcredit - totaldebit;
                disptbl += "<tr><td>&nbsp;" + dtTbl.Rows[j]["TrDate"] + "</td><td>&nbsp;" + dtTbl.Rows[j]["ValueDate"] + "</td><td>&nbsp;" + dtTbl.Rows[j]["accountsledger_TransactionReferenceID"] + "</td><td>&nbsp;" + dtTbl.Rows[j]["accountsledger_Narration"] + "</td><td>&nbsp;" + dtTbl.Rows[j]["accountsledger_InstrumentNumber"] + "</td><td>&nbsp;" + dtTbl.Rows[j]["SettlementNumber"] + "</td><td align=\"right\">&nbsp;" + dbt + "</td><td align=\"right\">&nbsp;" + crdt + "</td><td align=\"right\">&nbsp;" + oconverter.formatmoneyinUs(totalclosing) + "</td></tr>";
                //totaldebit = totaldebit + debit;
                //totalcredit = totalcredit + credit;
                //totalclosing = totalcredit - totaldebit;
            }
            disptbl += "<tr style=\"background-color: #FFD4AA; color: Black;\"><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;Closing Balance</td><td>&nbsp;</td><td>&nbsp;</td><td align=\"right\">&nbsp;" + oconverter.formatmoneyinUs(totaldebit) + "</td><td align=\"right\">&nbsp;" + oconverter.formatmoneyinUs(totalcredit) + "</td><td align=\"right\">&nbsp;" + oconverter.formatmoneyinUs(totalclosing) + "</td></tr>";
            disptbl += "</table>";
            string Type = "";
            // DataRow newRow5 = dtCashBankBook_New1.NewRow();
            if (Request.QueryString["seg"].ToString() == "n")
            {
                Type = "NSDL Clients A/c";
            }
            else if (Request.QueryString["seg"].ToString() == "c")
            {
                Type = "CDSL Clients A/c";

            }
            string billdate = Type + ": Period :" + " " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "  - " + oconverter.ArrangeDate2(dtTo.Value.ToString());
            string emailbdy = disptbl;
            ////**  string contactid = dtCL.Rows[n]["subaccount_code"].ToString();
            string contactid = Request.QueryString["accno"].ToString();
            //string billdate = oconverter.ArrangeDate2(dtFor.Value.ToString());
            string Subject = "Ledger View For " + billdate;
            if (oDbEngine.SendReport(emailbdy, contactid, billdate, Subject) == true)
            {

                this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript6", "<script>MailsendT();</script>");
                ////cmbclientsPager.SelectedItem.Value = dtCL.Rows[0]["subaccount_code"].ToString();
                ////cmbclientsPager.SelectedItem.Text = dtCL.Rows[0]["subaccount_name"].ToString();
                FillGrid();
                //    string SpanText = oconverter.ArrangeDate2(dtFor.Value.ToString());
                //    this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript9", "<script>displaydate('" + SpanText + "');</script>");
            }
            else
            {
                ////if (dtCL.Rows.Count <= 1)
                ////{
                ////    this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript9", "<script>MailsendF();</script>");
                ////}
                this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript9", "<script>MailsendF();</script>");
                ////cmbclientsPager.SelectedItem.Value = dtCL.Rows[0]["subaccount_code"].ToString();
                ////cmbclientsPager.SelectedItem.Text = dtCL.Rows[0]["subaccount_name"].ToString();
                ////FillGrid();
                //string SpanText = oconverter.ArrangeDate2(dtFor.Value.ToString());
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript9", "<script>displaydate('" + SpanText + "');</script>");

            }


            ////}
        }
        protected void lnkExport_Click(object sender, EventArgs e)
        {
            string SingleDouble = null;
            DataSet dsCrystal = new DataSet();
            string Type = null;
            string SubAccountSearch = null;
            string mainAccountSearch = null;
            MainAcID = HdnMainAc.Value;
            if (ViewState["MainAcID"] != null)
                MainAcID = ViewState["MainAcID"].ToString();
            Segment = ViewState["Segment"].ToString();
            SubAcID = ViewState["SubAcID"].ToString();
            if (ViewState["SubAccountSearch"] != null)
            {

                SubAcID = Request.QueryString["accno"].ToString();
                SubAccountSearch = " and AccountsLedger_SubAccountID in(" + SubAcID + ") ";
            }
            else
                SubAccountSearch = "NA";
            string ComID = Session["LastCompany"].ToString();
            Branch = Session["userbranchHierarchy"].ToString();


            mainAccountSearch = "and accountsledger_MainAccountID in(" + MainAcID + ")";
            MainAcIDforOp = MainAcID;



            Type = "Detail";

            if (chkDouble.Checked == true)
                SingleDouble = "D";
            else
                SingleDouble = "S";

            dsCrystal = farep.AccountsLedgerReport_Cryatal(ComID, Segment, dtFrom.Value.ToString(), dtTo.Value.ToString(), mainAccountSearch, Branch,
                SubAccountSearch, Type, MainAcIDforOp, SubAcID, Session["LastFinYear"].ToString(), SingleDouble, "");

            byte[] logoinByte;
            dsCrystal.Tables[0].Columns.Add("Image", System.Type.GetType("System.Byte[]"));

            if (oconverter.getLogoImage(HttpContext.Current.Server.MapPath(@"..\images\logo.jpg"), out logoinByte) != 1)
            {
                ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "logo", "alert('Logo not Found.');", true);

            }
            else
            {
                for (int i = 0; i < dsCrystal.Tables[0].Rows.Count; i++)
                {
                    dsCrystal.Tables[0].Rows[i]["Image"] = logoinByte;
                }
                // dsCrystal.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "//Reports//AccountsLedger.xsd");
                dsCrystal.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "//Reports//AccountsLedger.xsd");
                string[] connPath = ConfigurationManager.AppSettings["DBConnectionDefault"].Split(';');
                ReportDocument reportObj = new ReportDocument();
                string ReportPath = Server.MapPath("..\\Reports\\AccountsLedger.rpt");
                reportObj.Load(ReportPath);
                reportObj.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
                reportObj.SetDataSource(dsCrystal);
                //reportObj.Subreports["logo"].SetDataSource(dsCrystal.Tables[0]);
                reportObj.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "AccountsLedger");
                reportObj.Dispose();
                GC.Collect();
            }


            //reportObj.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, "AccountsLedger.pdf");
        }
    }
}