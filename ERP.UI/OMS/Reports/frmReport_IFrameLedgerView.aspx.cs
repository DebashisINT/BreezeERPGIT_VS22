using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
//using LibDosPrint;
//using BusinessLogicLayer;
namespace ERP.OMS.Reports
{
    public partial class Reports_frmReport_IFrameLedgerView : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        BusinessLogicLayer.FAReportsOther objFAReportsOther = new BusinessLogicLayer.FAReportsOther();
        #region Global Variable
        int pageindex = 0;
        int pagecount = 0;
        int pageSize;
        int rowcount = 0;
        string BtnclickShow = "";
        string SubLedgerType = "";
        string Branch;
        string Segment;
        string SegmentT;
        string MainAcID;
        string SubAcID;
        string SegMentName;
        string Group;
        string data;
        decimal openingBal;
        decimal debitTotal = 0;
        decimal creditTotal = 0;
        string SegmentID = null;
        string BranchId;
        string MainAcc;
        string MainAcIDforOp;
        string Clients;
        string DrpChange = "N";
        protected string fDate;
        protected string tDate;
        PrintDocument pd = new PrintDocument();
        DataTable dtCashBankBook = new DataTable();
        DataTable dtLedgerView = new DataTable();
        DataTable OpenBalance = new DataTable();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        ExcelFile objExcel = new ExcelFile();
        DataSet DS = new DataSet();
        BusinessLogicLayer.GenericMethod oGenericMethod = null;
        string ReportType = "";
        //CrystalDecisions.Web.CrystalReportViewer = new CrystalDecisions.Web.CrystalReportViewer();
        //CrystalDecisions.Web.CrystalReportSource = new CrystalDecisions.Web.CrystalReportSource();
        public string ClosingBlncPstv
        {
            get { return (string)Session["ClosingBlncPstv_P"]; }
            set { Session["ClosingBlncPstv_P"] = value; }
        }

        static string Check;
        string SegN = "";
        String EmailInd = "N";
        #endregion

        #region Page Method
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
            DataTable dtBr = oDBEngine.GetDataTable("tbl_master_branch ", " branch_parentid ", " branch_id in (select cnt_branchid from tbl_master_contact where cnt_internalid='" + Session["usercontactID"].ToString() + "')");
            if (dtBr.Rows.Count > 0)
            {
                if (dtBr.Rows[0][0].ToString() == "0")
                {
                    ddlAccountType.Items[3].Enabled = true;
                }
                else
                {
                    if (Session["userlastsegment"].ToString() == "9" || Session["userlastsegment"].ToString() == "10")
                        ddlAccountType.Items[3].Enabled = true;
                    else
                        ddlAccountType.Items[3].Enabled = false;
                }
            }
            else
            {
                ddlAccountType.Items[3].Enabled = false;
            }
            //DataTable dtClsngBlnc = oDBEngine.GetDataTable("tbl_master_company ", " isnull(cmp_LedgerView,'C') ", " cmp_internalid = '" + Session["LastCompany"].ToString()+"'");
            //if (dtClsngBlnc.Rows.Count > 0)
            //{
            //ClosingBlncPstv = Session["LedgerView"].ToString();
            ClosingBlncPstv = Convert.ToString(Session["LedgerView"]);
            //dtClsngBlnc.Rows[0][0].ToString();
            //}
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (Session["userlastsegment"].ToString() == "5")
            {
                HDNAccInd.Value = "N";
            }
            else
            {
                HDNAccInd.Value = "Y";
            }
            //if (Session["userlastsegment"].ToString() == "5")
            //{
            //    trSeg.Visible = false;
            //    Segment = "0";
            //    ViewState["SegMentName"] = "Accounts";
            //    Session["SegmentID"] = "0";
            //    Session["CompanyID"] = Session["LastCompany"].ToString();

            //}
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            if (!IsPostBack)
            {
                //DropDownList1.DataSource = System.Drawing.Printing.PrinterSettings.InstalledPrinters;
                //DropDownList1.DataBind();
                //Tr_Location.Visible = false;
                //FnDosPrint();
                // txtVoucherPrefix.Attributes.Add("onfocus", "ManageMJV()");
                txtVoucherPrefix.Attributes.Add("onclick", "ManageMJV()");
                DataTable DtSegComp = new DataTable();
                DataTable dtSeg = oDBEngine.GetDataTable("tbl_master_segment", "seg_name", " seg_id=" + Session["userlastsegment"].ToString() + "");
                ViewState["dtSeg"] = dtSeg;
                if (dtSeg.Rows[0][0].ToString().EndsWith("CM"))
                    DtSegComp = oDBEngine.GetDataTable("(select top 1 exch_compId,exch_internalId ,isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Seg , isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+' - '+exch_segmentId,exch_membershiptype) as Comp from tbl_master_companyExchange where exch_compid in (select  ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment in(" + Session["userallsegmentnotonlyLast"].ToString() + "))) as D", " exch_compId,exch_internalId ,Comp ", "Seg in(select seg_name from tbl_master_segment where seg_id in(" + Session["userallsegmentnotonlyLast"].ToString() + "))  and Comp like '%CM' and exch_compID='" + Session["LastCompany"].ToString() + "'");
                else
                    DtSegComp = oDBEngine.GetDataTable("(select top 1 exch_compId,exch_internalId,isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Seg,isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+' - '+exch_segmentId,exch_membershiptype) as Comp from tbl_master_companyExchange where exch_compid in (select  ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment=" + Session["userlastsegment"].ToString() + ")) as D", " exch_compId,exch_internalId ,Comp ", "Seg in(select seg_name from tbl_master_segment where seg_id=" + Session["userlastsegment"].ToString() + ") and  exch_compID='" + Session["LastCompany"].ToString() + "'");
                if (DtSegComp.Rows.Count > 0)
                {
                    string CompanyID = DtSegComp.Rows[0][0].ToString();
                    for (int i = 0; i < DtSegComp.Rows.Count; i++)
                    {
                        if (SegmentID == null)
                        {
                            SegmentID = DtSegComp.Rows[i][1].ToString();
                            SegMentName = DtSegComp.Rows[i][2].ToString();
                            SegN = "'" + DtSegComp.Rows[i][2].ToString() + "'";
                        }
                        else
                        {
                            SegmentID = SegmentID + "," + DtSegComp.Rows[i][1].ToString();
                            SegMentName = SegMentName + "," + DtSegComp.Rows[i][2].ToString();
                            SegN = SegN + ",'" + DtSegComp.Rows[i][2].ToString() + "'";
                        }
                    }

                    ViewState["SegMentName"] = SegMentName;
                    Session["CompanyID"] = CompanyID;
                    Session["SegmentID"] = SegmentID;
                    //  ViewState["Selectedjvs"] = "";
                    litSegment.InnerText = SegN;
                    HDNSeg.Value = SegN;
                }
                ViewState["Clients"] = null;
                if (Request.QueryString["MainID"] != null)
                {
                    HDNMAIN.Value = Request.QueryString["MainID"].ToString();
                    FillGrid();
                    Page.ClientScript.RegisterStartupScript(GetType(), "Led", "<script>OnlySubsidiaryTrial();</script>");
                    //ScriptManager.RegisterStartupScript(this,this.GetType(), "HideAll", "OnlySubsidiaryTrial()", true);
                    //   ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct414", "OnlySubsidiaryTrial()", true);

                    dtFromG.EditFormatString = oconverter.GetDateFormat("Date");
                    dtToG.EditFormatString = oconverter.GetDateFormat("Date");
                    //dtFrom.Value = Convert.ToDateTime(DateTime.Today);  
                    //dtFromG.Value = Convert.ToDateTime(oDBEngine.GetDate().Month + "/1/" + oDBEngine.GetDate().Year);
                    //dtToG.Value = Convert.ToDateTime(Session["FinYearEnd"].ToString());    
                }
                else
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
                    dtFrom.Value = Convert.ToDateTime(oDBEngine.GetDate().Month + "/1/" + oDBEngine.GetDate().Year);
                    dtTo.Value = Convert.ToDateTime(Session["FinYearEnd"].ToString());
                    ddlAccountType.Attributes.Add("onchange", "ChangeAccountType();");
                    rdbSegAll.Attributes.Add("OnClick", "SegAll('seg')");
                    rdbSegSelected.Attributes.Add("OnClick", "SegSelected('seg')");
                    rdSubAcAll.Attributes.Add("OnClick", "SegAll('Sub')");
                    rdSubAcSelected.Attributes.Add("OnClick", "SegSelected('Sub')");

                    RadSettA.Attributes.Add("OnClick", "SegAll('Set')");
                    RadSettS.Attributes.Add("OnClick", "SegSelected('Set')");

                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");
                    txtsubscriptionID.Attributes.Add("onkeyup", "showOptions(this,'SearchMainAccountBranchSegment',event)");
                    txtSubsubcriptionID.Attributes.Add("onkeyup", "showOptionsforSunAc(this,'selectSubAccountForMainAccount',event)");
                }
                //___________For CDSL/NSDL_____________________
                if (Session["userlastsegment"].ToString() == "9" || Session["userlastsegment"].ToString() == "10")
                {
                    string NsdlCdslType = null;
                    if (Session["userlastsegment"].ToString() == "9")
                        NsdlCdslType = "NSDL";
                    else if (Session["userlastsegment"].ToString() == "10")
                        NsdlCdslType = "CDSL";
                    Page.ClientScript.RegisterStartupScript(GetType(), "JScriptNCd", "<script language='javascript'>Page_Load_NsdlCdsl('" + NsdlCdslType + "');</script>");
                }
                fDate = oDBEngine.GetDate().Month + "/1/" + oDBEngine.GetDate().Year;
                tDate = Session["FinYearEnd"].ToString();
                if (Convert.ToDateTime(fDate.ToString()) > Convert.ToDateTime(tDate.ToString()))
                    fDate = tDate;
                dtFrom.Value = Convert.ToDateTime(fDate);
                dtTo.Value = Convert.ToDateTime(tDate);

                //________________End________________
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
            string str2 = "";
            if (idlist[0] == "ComboChange")
            {
                MainAcID = idlist[1];
            }
            else if (idlist[0] == "GeneratePrint")
            {
                //string date = idlist[1].ToString();
                //string date1 = idlist[2].ToString();
                //BtnGenerate_Click();
                data = "GeneratePrint";
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
                            str2 = val[0];
                        }
                        else
                        {
                            str += ",'" + val[0] + "'";
                            str1 += "," + val[0] + ";" + val[1];
                            str2 += "," + val[0];
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
                    //  data = "Segment~" + str;
                    data = "Segment~" + str1;
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
                else if (idlist[0] == "Clients")
                {
                    SubAcID = str;
                    data = "Clients~" + str;
                    //ViewState["Clients"] = str;
                }
                else if (idlist[0] == "Group")
                {
                    SubAcID = str;
                    data = "Group~" + str;
                }
                else if (idlist[0] == "Settlement")
                {
                    data = "Settlement~" + str;
                }
                else if (idlist[0] == "Employee")
                {

                    data = "Employee~" + str2;
                }
                else if (idlist[0] == "System JVs")
                {
                    //ViewState["Selectedjvs"] = str;
                    data = "System JVs~" + str;
                }
            }
            // string abc = Convert.ToString(ViewState["Clients"]);
        }
        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }
        #endregion

        public void FillDropDown()
        {
            string SubID = null;
            SubAcID = HdnSubAc.Value;
            string MainSubID = null;
            if (rdSubAcAll.Checked == true)
            {
                SubAcID = null;
            }
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
                if (ddlAccountType.SelectedValue == "0")
                {
                    MainID = " subaccount_mainacreferenceid in('SYSTM00001')";
                    MainLedgerID = " and accountsledger_mainaccountid in('SYSTM00001')";
                }
                else if (ddlAccountType.SelectedValue == "1")
                {
                    MainID = " subaccount_mainacreferenceid in('SYSTM00002')";
                    MainLedgerID = " and accountsledger_mainaccountid in('SYSTM00002')";
                }
                else if (ddlAccountType.SelectedValue == "2")
                {
                    MainID = " subaccount_mainacreferenceid in('SYSTM00001','SYSTM00002')";
                    MainLedgerID = " and accountsledger_mainaccountid in('SYSTM00001','SYSTM00002')";
                }
                else
                {
                    MainID = " subaccount_mainacreferenceid not in('SYSTM00001','SYSTM00002')";
                    MainLedgerID = " and accountsledger_mainaccountid not in('SYSTM00001','SYSTM00002')";
                }
            }
            else
            {
                MainID = " subaccount_mainacreferenceid in(" + MainAcID + ")";
                MainLedgerID = " and accountsledger_mainaccountid in(" + MainAcID + ")";
                //MainID = MainAcID;
            }
            if (rdbSegAll.Checked == true)
            {
                DataTable dtSegment = oDBEngine.GetDataTable("tbl_master_companyExchange", "exch_internalId", "exch_compId='" + Session["CompanyID"].ToString() + "'");
                if (dtSegment.Rows.Count > 0)
                {
                    for (int i = 0; i < dtSegment.Rows.Count; i++)
                    {
                        if (Segment == null)
                            Segment = dtSegment.Rows[i][0].ToString();
                        else
                            Segment += "," + dtSegment.Rows[i][0].ToString();
                    }
                }
            }
            if (Segment == null)
            {
                Segment = Session["SegmentID"].ToString();
            }
            if (SubLedgerType.Trim() == "None")
            {
                DataTable Clients = new DataTable();
                cmbclientsPager.DataSource = Clients;
                cmbclientsPager.DataBind();
            }
            else
            {
                DataTable Clients = new DataTable();
                cmbclientsPager.Items.Clear();
                if (Session["userlastsegment"].ToString() == "5")
                {
                    if (ViewState["Clients"] == null)
                        Clients = oDBEngine.GetDataTable("(	select subaccount_code,ltrim(rtrim(subaccount_name))+isnull(' ['+isnull((select case when (ltrim(rtrim(cnt_ucc))='' or ltrim(rtrim(cnt_ucc))=null) then cnt_shortname else rtrim(ltrim(cnt_ucc)) end from tbl_master_contact where ltrim(rtrim(cnt_internalid))=ltrim(rtrim(master_subaccount.subAccount_Code))),isnull((select ltrim(rtrim(cdslClients_BenAccountNumber)) from master_cdslClients where ltrim(rtrim(cdslClients_BenAccountNumber))=ltrim(rtrim(master_subaccount.subAccount_Code))),(select ltrim(rtrim(nsdlClients_BenAccountID)) from master_nsdlClients where ltrim(rtrim(nsdlClients_BenAccountID))=ltrim(rtrim(master_subaccount.subAccount_Code)))))+']','') as subaccount_name	from master_subaccount	where " + MainID + " " + SubID + "	and subaccount_code in(select accountsledger_subaccountid from trans_accountsledger where cast(DATEADD(dd, 0, DATEDIFF(dd, 0,accountsledger_transactiondate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtFrom.Value + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtTo.Value + "')) as datetime) " + MainLedgerID + "  and accountsledger_BranchID in(" + Session["userbranchHierarchy"].ToString() + ")) and subaccount_name is not null	union all	select subaccount_code,ltrim(rtrim(subaccount_name))+isnull(' ['+isnull((select ltrim(rtrim(cnt_ucc)) from tbl_master_contact where ltrim(rtrim(cnt_internalid))=ltrim(rtrim(master_subaccount.subAccount_Code))),isnull((select ltrim(rtrim(cdslClients_BenAccountNumber)) from master_cdslClients where ltrim(rtrim(cdslClients_BenAccountNumber))=ltrim(rtrim(master_subaccount.subAccount_Code))),(select ltrim(rtrim(nsdlClients_BenAccountID)) from master_nsdlClients where ltrim(rtrim(nsdlClients_BenAccountID))=ltrim(rtrim(master_subaccount.subAccount_Code)))))+']','') as subaccount_name	from (select sum(AccountsLedger_AmountDr)-sum(AccountsLedger_AmountCr) as Amount,AccountsLedger_SubAccountID ,AccountsLedger_MainAccountID from  Trans_AccountsLedger where   AccountsLedger_FinYear='" + Session["LastFinYear"].ToString() + "' " + MainLedgerID + " and accountsledger_transactiondate<'" + dtFrom.Value + "'	" + MainSubID + " and accountsledger_exchangesegmentid in(" + Segment + ")	group by AccountsLedger_MainAccountID,AccountsLedger_SubAccountID) as DD,master_subaccount 	where Amount<>0 and AccountsLedger_SubAccountID=SubAccount_Code and AccountsLedger_MainAccountID=SubAccount_MainAcReferenceID) as KK", " distinct subaccount_code,subaccount_name", null, " subaccount_name");
                    else
                        Clients = oDBEngine.GetDataTable("(	select subaccount_code,ltrim(rtrim(subaccount_name))+isnull(' ['+isnull((select case when (ltrim(rtrim(cnt_ucc))='' or ltrim(rtrim(cnt_ucc))=null) then cnt_shortname else rtrim(ltrim(cnt_ucc)) end from tbl_master_contact where ltrim(rtrim(cnt_internalid))=ltrim(rtrim(master_subaccount.subAccount_Code))),isnull((select ltrim(rtrim(cdslClients_BenAccountNumber)) from master_cdslClients where ltrim(rtrim(cdslClients_BenAccountNumber))=ltrim(rtrim(master_subaccount.subAccount_Code))),(select ltrim(rtrim(nsdlClients_BenAccountID)) from master_nsdlClients where ltrim(rtrim(nsdlClients_BenAccountID))=ltrim(rtrim(master_subaccount.subAccount_Code)))))+']','') as subaccount_name	from master_subaccount	where " + MainID + " " + SubID + "	and subaccount_code in(select accountsledger_subaccountid from trans_accountsledger where cast(DATEADD(dd, 0, DATEDIFF(dd, 0,accountsledger_transactiondate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtFrom.Value + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtTo.Value + "')) as datetime) " + MainLedgerID + "  and accountsledger_BranchID in(" + ViewState["branchID"].ToString() + ")) and subaccount_code in(" + ViewState["Clients"].ToString() + ")	union all	select subaccount_code,ltrim(rtrim(subaccount_name))+isnull(' ['+isnull((select ltrim(rtrim(cnt_ucc)) from tbl_master_contact where ltrim(rtrim(cnt_internalid))=ltrim(rtrim(master_subaccount.subAccount_Code))),isnull((select ltrim(rtrim(cdslClients_BenAccountNumber)) from master_cdslClients where ltrim(rtrim(cdslClients_BenAccountNumber))=ltrim(rtrim(master_subaccount.subAccount_Code))),(select ltrim(rtrim(nsdlClients_BenAccountID)) from master_nsdlClients where ltrim(rtrim(nsdlClients_BenAccountID))=ltrim(rtrim(master_subaccount.subAccount_Code)))))+']','') as subaccount_name	from (select sum(AccountsLedger_AmountDr)-sum(AccountsLedger_AmountCr) as Amount,AccountsLedger_SubAccountID ,AccountsLedger_MainAccountID from  Trans_AccountsLedger where   AccountsLedger_FinYear='" + Session["LastFinYear"].ToString() + "' " + MainLedgerID + " and accountsledger_transactiondate<'" + dtFrom.Value + "'	" + MainSubID + " and accountsledger_exchangesegmentid in(" + Segment + ")	group by AccountsLedger_MainAccountID,AccountsLedger_SubAccountID) as DD,master_subaccount 	where Amount<>0 and AccountsLedger_SubAccountID=SubAccount_Code and AccountsLedger_MainAccountID=SubAccount_MainAcReferenceID and subaccount_code in(" + ViewState["Clients"].ToString() + ")) as KK", " distinct subaccount_code,subaccount_name", null, " subaccount_name");

                }
                else
                {
                    if (Convert.ToString(ViewState["Clients"]) == "")
                        Clients = oDBEngine.GetDataTable("(	select subaccount_code,ltrim(rtrim(subaccount_name))+isnull(' ['+isnull((select case when (ltrim(rtrim(cnt_ucc))='' or ltrim(rtrim(cnt_ucc))=null) then cnt_shortname else rtrim(ltrim(cnt_ucc)) end from tbl_master_contact where ltrim(rtrim(cnt_internalid))=ltrim(rtrim(master_subaccount.subAccount_Code))),isnull((select ltrim(rtrim(cdslClients_BenAccountNumber)) from master_cdslClients where ltrim(rtrim(cdslClients_BenAccountNumber))=ltrim(rtrim(master_subaccount.subAccount_Code))),(select ltrim(rtrim(nsdlClients_BenAccountID)) from master_nsdlClients where ltrim(rtrim(nsdlClients_BenAccountID))=ltrim(rtrim(master_subaccount.subAccount_Code)))))+']','') as subaccount_name	from master_subaccount	where " + MainID + " " + SubID + "	and subaccount_code in(select accountsledger_subaccountid from trans_accountsledger where cast(DATEADD(dd, 0, DATEDIFF(dd, 0,accountsledger_transactiondate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtFrom.Value + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtTo.Value + "')) as datetime) " + MainLedgerID + " and accountsledger_exchangesegmentid in(" + Segment + ") and accountsledger_BranchID in(" + Session["userbranchHierarchy"].ToString() + ")) and subaccount_name is not null	union all	select subaccount_code,ltrim(rtrim(subaccount_name))+isnull(' ['+isnull((select ltrim(rtrim(cnt_ucc)) from tbl_master_contact where ltrim(rtrim(cnt_internalid))=ltrim(rtrim(master_subaccount.subAccount_Code))),isnull((select ltrim(rtrim(cdslClients_BenAccountNumber)) from master_cdslClients where ltrim(rtrim(cdslClients_BenAccountNumber))=ltrim(rtrim(master_subaccount.subAccount_Code))),(select ltrim(rtrim(nsdlClients_BenAccountID)) from master_nsdlClients where ltrim(rtrim(nsdlClients_BenAccountID))=ltrim(rtrim(master_subaccount.subAccount_Code)))))+']','') as subaccount_name	from (select sum(AccountsLedger_AmountDr)-sum(AccountsLedger_AmountCr) as Amount,AccountsLedger_SubAccountID ,AccountsLedger_MainAccountID from  Trans_AccountsLedger where   AccountsLedger_FinYear='" + Session["LastFinYear"].ToString() + "' " + MainLedgerID + " and accountsledger_transactiondate<'" + dtFrom.Value + "'	" + MainSubID + " and accountsledger_exchangesegmentid in(" + Segment + ")	group by AccountsLedger_MainAccountID,AccountsLedger_SubAccountID) as DD,master_subaccount 	where Amount<>0 and AccountsLedger_SubAccountID=SubAccount_Code and AccountsLedger_MainAccountID=SubAccount_MainAcReferenceID) as KK", " distinct subaccount_code,subaccount_name", null, " subaccount_name");
                    else
                        Clients = oDBEngine.GetDataTable("(	select subaccount_code,ltrim(rtrim(subaccount_name))+isnull(' ['+isnull((select case when (ltrim(rtrim(cnt_ucc))='' or ltrim(rtrim(cnt_ucc))=null) then cnt_shortname else rtrim(ltrim(cnt_ucc)) end from tbl_master_contact where ltrim(rtrim(cnt_internalid))=ltrim(rtrim(master_subaccount.subAccount_Code))),isnull((select ltrim(rtrim(cdslClients_BenAccountNumber)) from master_cdslClients where ltrim(rtrim(cdslClients_BenAccountNumber))=ltrim(rtrim(master_subaccount.subAccount_Code))),(select ltrim(rtrim(nsdlClients_BenAccountID)) from master_nsdlClients where ltrim(rtrim(nsdlClients_BenAccountID))=ltrim(rtrim(master_subaccount.subAccount_Code)))))+']','') as subaccount_name	from master_subaccount	where " + MainID + " " + SubID + "	and subaccount_code in(select accountsledger_subaccountid from trans_accountsledger where cast(DATEADD(dd, 0, DATEDIFF(dd, 0,accountsledger_transactiondate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtFrom.Value + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtTo.Value + "')) as datetime) " + MainLedgerID + " and accountsledger_exchangesegmentid in(" + Segment + ") and accountsledger_BranchID in(" + ViewState["branchID"].ToString() + ")) and subaccount_code in(" + ViewState["Clients"].ToString() + ")	union all	select subaccount_code,ltrim(rtrim(subaccount_name))+isnull(' ['+isnull((select ltrim(rtrim(cnt_ucc)) from tbl_master_contact where ltrim(rtrim(cnt_internalid))=ltrim(rtrim(master_subaccount.subAccount_Code))),isnull((select ltrim(rtrim(cdslClients_BenAccountNumber)) from master_cdslClients where ltrim(rtrim(cdslClients_BenAccountNumber))=ltrim(rtrim(master_subaccount.subAccount_Code))),(select ltrim(rtrim(nsdlClients_BenAccountID)) from master_nsdlClients where ltrim(rtrim(nsdlClients_BenAccountID))=ltrim(rtrim(master_subaccount.subAccount_Code)))))+']','') as subaccount_name	from (select sum(AccountsLedger_AmountDr)-sum(AccountsLedger_AmountCr) as Amount,AccountsLedger_SubAccountID ,AccountsLedger_MainAccountID from  Trans_AccountsLedger where   AccountsLedger_FinYear='" + Session["LastFinYear"].ToString() + "' " + MainLedgerID + " and accountsledger_transactiondate<'" + dtFrom.Value + "'	" + MainSubID + " and accountsledger_exchangesegmentid in(" + Segment + ")	group by AccountsLedger_MainAccountID,AccountsLedger_SubAccountID) as DD,master_subaccount 	where Amount<>0 and AccountsLedger_SubAccountID=SubAccount_Code and AccountsLedger_MainAccountID=SubAccount_MainAcReferenceID and subaccount_code in(" + ViewState["Clients"].ToString() + ")) as KK", " distinct subaccount_code,subaccount_name", null, " subaccount_name");
                }
                Session["Client"] = Clients;
                cmbclientsPager.DataSource = Clients;
                cmbclientsPager.DataValueField = "subaccount_code";
                cmbclientsPager.DataTextField = "subaccount_name";
                cmbclientsPager.DataBind();
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct111", "HideoffOnButton()", true);
        }

        #region Grid Related

        public void FillGrid()
        {
            //try
            //{
            DataTable OpenBalance = new DataTable();
            dtCashBankBook = new DataTable();
            dtLedgerView = new DataTable();
            decimal receipt = 0;
            decimal Payment = 0;
            DateTime TranDate = DateTime.Today;
            System.Globalization.NumberFormatInfo currencyFormat = new System.Globalization.CultureInfo("hi-IN").NumberFormat;
            currencyFormat.CurrencySymbol = "";
            currencyFormat.CurrencyNegativePattern = 2;
            pageSize = 25;
            grdCashBankBook.PageSize = pageSize;

            if (Request.QueryString["MainID"] != null)
            {
                if (BtnclickShow == "Click")
                {
                    dtFrom.Value = dtFromG.Value;
                    dtTo.Value = dtToG.Value;
                    SpanShowHeader.InnerText = "   Period  From    " + oconverter.ArrangeDate2(dtFromG.Value.ToString()) + "      To    " + oconverter.ArrangeDate2(dtToG.Value.ToString());
                }
                else
                {
                    DateTime dtTranDate = new DateTime();
                    DateTime date = Convert.ToDateTime(Request.QueryString["date"].ToString());
                    DateTime ToDay = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());

                    DataTable dtFinYear = oDBEngine.GetDataTable("MASTER_FINYEAR ", " FINYEAR_STARTDATE ", " FINYEAR_CODE ='" + Session["LastFinYear"].ToString() + "'");
                    DateTime StartDate = Convert.ToDateTime(dtFinYear.Rows[0][0].ToString());
                    if (date < ToDay)
                        dtTranDate = Convert.ToDateTime(date.AddDays(-45).ToShortDateString());
                    else
                        dtTranDate = Convert.ToDateTime(oDBEngine.GetDate().AddDays(-45).ToShortDateString());


                    if (dtTranDate < StartDate)
                    {
                        dtTranDate = StartDate;
                        dtFrom.Value = StartDate;
                        dtTo.Value = date;
                        dtFromG.Value = StartDate;
                        dtToG.Value = date;
                    }
                    else
                    {
                        dtFrom.Value = dtTranDate;
                        dtTo.Value = date;
                        dtFromG.Value = dtTranDate;
                        dtToG.Value = date;
                    }
                    SpanShowHeader.InnerText = "   Period  From    " + oconverter.ArrangeDate2(dtTranDate.ToString()) + "      To    " + oconverter.ArrangeDate2(date.ToString());
                }

                if (Request.QueryString["SubID"].ToString() == "GeneralTrial ")
                {
                    // dtCashBankBook = oDBEngine.GetDataTable("(Select convert(varchar(11),a.accountsledger_transactiondate,113) as TrDate,case when a.accountsledger_valuedate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),a.accountsledger_valuedate,113) end as ValueDate,AccountsLedger_TransactionReferenceID,case when a.AccountsLedger_TransactionType='Journal' and Left(AccountsLedger_TransactionReferenceID,1) in('X','Y','Z') and Left(AccountsLedger_TransactionReferenceID,2) not in('YD') and accountsledger_SettlementNumber is not null and accountsledger_SettlementNumber<>''	then 'Consolidated Entries For '+convert(varchar(11),a.accountsledger_transactiondate,113) 	when a.AccountsLedger_TransactionType='Journal'  and Left(AccountsLedger_TransactionReferenceID,2) in('YD') and accountsledger_SettlementNumber is not null and accountsledger_SettlementNumber<>''	then 'Short Back Add Delivery '+convert(varchar(11),a.accountsledger_transactiondate,113) else isnull(a.accountsledger_Narration,'') end +' - '+isnull(isnull((select MainAccount_Name from Master_MainAccount where MainAccount_AccountCode=AccountsLedger_CashBankName),AccountsLedger_CashBankName),'') as accountsledger_Narration,'1' as AccountName,case when a.Accountsledger_AmountCr='0.00000000' then null else a.Accountsledger_AmountCr end as Accountsledger_AmountDr,case when a.Accountsledger_AmountDr='0.00000000' then null else a.Accountsledger_AmountDr end as Accountsledger_AmountCr,'0.0' as Closing,a.accountsledger_transactiondate as accountsledger_transactiondate,a.accountsledger_InstrumentNumber,(a.accountsledger_SettlementNumber+' '+a.accountsledger_SettlementType) as SettlementNumber,a.accountsledger_MainAccountID as MainID,a.accountsledger_SubAccountID as SubID,a.accountsledger_companyID as CompanyID,a.accountsledger_exchangeSegmentID as SegID,a.AccountsLedger_TransactionType  as CashType,(select top 1 convert(varchar(12),Settlements_StartDateTime,113) from master_settlements where Settlements_Number=a.accountsledger_SettlementNumber and Settlements_TypeSuffix=a.accountsledger_SettlementType) as PayoutDate,(select branch_code from tbl_master_branch where branch_id=(select cnt_branchid from tbl_master_contact where cnt_internalID=accountsledger_SubAccountID)) as BranchCode from Trans_accountsledger a  WHERE  accountsledger_companyID='" + Session["LastCompany"].ToString() + "' and accountsledger_ExchangeSegmentID in(" + Request.QueryString["SegmentID"].ToString() + ") and  accountsledger_transactiondate>='" + dtFrom.Value + "' and accountsledger_MainAccountID in('" + Request.QueryString["MainID"].ToString() + "')   and AccountsLedger_SubAccountID is not null and AccountsLedger_TransactionType<>'OpeningBalance' ) as D", "TrDate,ValueDate,AccountsLedger_TransactionReferenceID as accountsledger_TransactionReferenceID,accountsledger_Narration,AccountName,case when (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0)))>0 then (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0))) else null end Accountsledger_AmountDr,case when (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0)))<0 then (sum(isnull(Accountsledger_AmountCr,0))-sum(isnull(Accountsledger_AmountDr,0))) else null end Accountsledger_AmountCr,Closing,accountsledger_transactiondate,accountsledger_InstrumentNumber,SettlementNumber,MainID,SubID,CompanyID,SegID,CashType,PayoutDate,BranchCode,396 as UserID ", null, " TrDate,ValueDate,AccountsLedger_TransactionReferenceID,AccountName,Closing,accountsledger_transactiondate,accountsledger_InstrumentNumber,SettlementNumber,MainID,SubID,CompanyID,SegID,CashType,accountsledger_Narration,PayoutDate,BranchCode", " accountsledger_transactiondate");


                    //dtCashBankBook = oDBEngine.GetDataTable("(Select convert(varchar(11),a.accountsledger_transactiondate,113) as TrDate,case when a.accountsledger_valuedate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),a.accountsledger_valuedate,113) end as ValueDate,case when AccountsLedger_TransactionType='Cash_Bank' then AccountsLedger_TransactionReferenceID else  null end   as  AccountsLedger_TransactionReferenceID,case when a.AccountsLedger_TransactionType='Journal' and Left(AccountsLedger_TransactionReferenceID,1) in('X','Y','Z') and Left(AccountsLedger_TransactionReferenceID,2) not in('YD') and accountsledger_SettlementNumber is not null and accountsledger_SettlementNumber<>''	then 'Consolidated Entries For '+convert(varchar(11),a.accountsledger_transactiondate,113) 	when a.AccountsLedger_TransactionType='Journal'  and Left(AccountsLedger_TransactionReferenceID,2) in('YD') and accountsledger_SettlementNumber is not null and accountsledger_SettlementNumber<>''	then 'Short Back Add Delivery '+convert(varchar(11),a.accountsledger_transactiondate,113) else isnull(a.accountsledger_Narration,'') end +' - '++' - '+isnull(isnull((select MainAccount_Name from Master_MainAccount where MainAccount_AccountCode=AccountsLedger_CashBankName),AccountsLedger_CashBankName),'') as accountsledger_Narration,'1' as AccountName,case when a.Accountsledger_AmountCr='0.00000000' then null else a.Accountsledger_AmountCr end as Accountsledger_AmountDr,case when a.Accountsledger_AmountDr='0.00000000' then null else a.Accountsledger_AmountDr end as Accountsledger_AmountCr,'0.0' as Closing,a.accountsledger_transactiondate as accountsledger_transactiondate,a.accountsledger_InstrumentNumber,(a.accountsledger_SettlementNumber+' '+a.accountsledger_SettlementType) as SettlementNumber,a.accountsledger_MainAccountID as MainID,a.accountsledger_SubAccountID as SubID,a.accountsledger_companyID as CompanyID,a.accountsledger_exchangeSegmentID as SegID,a.AccountsLedger_TransactionType  as CashType,(select top 1 convert(varchar(12),Settlements_StartDateTime,113) from master_settlements where Settlements_Number=a.accountsledger_SettlementNumber and Settlements_TypeSuffix=a.accountsledger_SettlementType) as PayoutDate,(select branch_code from tbl_master_branch where branch_id=(select cnt_branchid from tbl_master_contact where cnt_internalID=accountsledger_SubAccountID)) as BranchCode from Trans_accountsledger a  WHERE  accountsledger_companyID='" + Session["LastCompany"].ToString() + "' and accountsledger_ExchangeSegmentID in(" + Request.QueryString["SegmentID"].ToString() + ") and  accountsledger_transactiondate between  '" + dtFrom.Value + "'  and  '" + dtTo.Value + "' and accountsledger_MainAccountID in('" + Request.QueryString["MainID"].ToString() + "') and accountsledger_finyear='" + Session["LastFinYear"].ToString() + "'   and AccountsLedger_TransactionType<>'OpeningBalance' ) as D", "TrDate,ValueDate,AccountsLedger_TransactionReferenceID as accountsledger_TransactionReferenceID,accountsledger_Narration,AccountName,case when (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0)))>0 then (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0))) else null end Accountsledger_AmountDr,case when (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0)))<0 then (sum(isnull(Accountsledger_AmountCr,0))-sum(isnull(Accountsledger_AmountDr,0))) else null end Accountsledger_AmountCr,Closing,accountsledger_transactiondate,accountsledger_InstrumentNumber,SettlementNumber,MainID,SubID,CompanyID,SegID,CashType,PayoutDate,BranchCode,396 as UserID ", null, " TrDate,ValueDate,AccountsLedger_TransactionReferenceID,AccountName,Closing,accountsledger_transactiondate,accountsledger_InstrumentNumber,SettlementNumber,MainID,SubID,CompanyID,SegID,CashType,accountsledger_Narration,PayoutDate,BranchCode", " accountsledger_transactiondate");
                    dtCashBankBook = oDBEngine.GetDataTable(@"(Select convert(varchar(11),a.accountsledger_transactiondate,113) as TrDate,
                case when a.accountsledger_valuedate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),a.accountsledger_valuedate,113) 
                end as ValueDate,  AccountsLedger_TransactionReferenceID,case when a.AccountsLedger_TransactionType='Journal' 
                and Left(AccountsLedger_TransactionReferenceID,1) in('X','Y','Z') and Left(AccountsLedger_TransactionReferenceID,2) 
                not in('YD') and accountsledger_SettlementNumber is not null and accountsledger_SettlementNumber<>''	
                then 'Consolidated Entries For '+convert(varchar(11),a.accountsledger_transactiondate,113) 	
                when a.AccountsLedger_TransactionType='Journal'  and Left(AccountsLedger_TransactionReferenceID,2) in('YD') 
                and accountsledger_SettlementNumber is not null and accountsledger_SettlementNumber<>''	then 'Short Back Add Delivery '+
                convert(varchar(11),a.accountsledger_transactiondate,113) else isnull(a.accountsledger_Narration,'') end +' - '++' - '+
                isnull(isnull((select MainAccount_Name from Master_MainAccount where MainAccount_AccountCode=AccountsLedger_CashBankName),
                AccountsLedger_CashBankName),'') as accountsledger_Narration,'1' as AccountName,case when a.Accountsledger_AmountCr='0.00000000' 
                then null else a.Accountsledger_AmountCr end as Accountsledger_AmountDr,case when a.Accountsledger_AmountDr='0.00000000' 
                then null else a.Accountsledger_AmountDr end as Accountsledger_AmountCr,'0.0' as Closing,a.accountsledger_transactiondate 
                as accountsledger_transactiondate,a.accountsledger_InstrumentNumber,(a.accountsledger_SettlementNumber+' '+
                a.accountsledger_SettlementType) as SettlementNumber,a.accountsledger_MainAccountID as MainID,
                a.accountsledger_SubAccountID as SubID,a.accountsledger_companyID as CompanyID,a.accountsledger_exchangeSegmentID as SegID,
                a.AccountsLedger_TransactionType  as CashType,(select top 1 convert(varchar(12),Settlements_StartDateTime,113) 
                from master_settlements where Settlements_Number=a.accountsledger_SettlementNumber and Settlements_TypeSuffix=a.accountsledger_SettlementType) as PayoutDate,
                (select branch_code from tbl_master_branch where branch_id=(select cnt_branchid from tbl_master_contact 
                where cnt_internalID=accountsledger_SubAccountID)) as BranchCode,
(select EXCHANGENAME  from (select A.EXCH_INTERNALID AS SEGMENTID ,isnull((TME.EXH_ShortName + '-' + A.EXCH_SEGMENTID),exch_membershipType) AS EXCHANGENAME from (SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID=a.AccountsLedger_CompanyID ) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID) as DD where cast(SEGMENTID as varchar)=a.AccountsLedger_ExchangeSegmentID)  AS SegmentName
                    from Trans_accountsledger a  
                WHERE  accountsledger_companyID='" + Session["LastCompany"].ToString() + @"' and accountsledger_ExchangeSegmentID 
                in(" + Request.QueryString["SegmentID"].ToString() + ") and  accountsledger_transactiondate between  '" + dtFrom.Value + @"'  
                and  '" + dtTo.Value + "' and accountsledger_MainAccountID in('" + Request.QueryString["MainID"].ToString() + @"') 
                and accountsledger_finyear='" + Session["LastFinYear"].ToString() + @"'   
                and AccountsLedger_TransactionType<>'OpeningBalance' and 
                isnull(AccountsLedger_Currency," + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0] + ") as D", @"TrDate,ValueDate,AccountsLedger_TransactionReferenceID 
                as accountsledger_TransactionReferenceID,accountsledger_Narration,AccountName,
                case when (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0)))>0 
                then (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0))) else null end Accountsledger_AmountDr,
                case when (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0)))<0 
                then (sum(isnull(Accountsledger_AmountCr,0))-sum(isnull(Accountsledger_AmountDr,0))) else null end Accountsledger_AmountCr,
                Closing,accountsledger_transactiondate,accountsledger_InstrumentNumber,SettlementNumber,MainID,SubID,CompanyID,SegID,
                CashType,PayoutDate,BranchCode,396 as UserID,SegmentName ", null, @" TrDate,ValueDate,AccountsLedger_TransactionReferenceID,
                AccountName,Closing,accountsledger_transactiondate,accountsledger_InstrumentNumber,SettlementNumber,MainID,SubID,
                CompanyID,SegID,CashType,accountsledger_Narration,PayoutDate,BranchCode,SegmentName", " accountsledger_transactiondate");
                    OpenBalance = oDBEngine.OpeningBalanceJournal1("'" + Request.QueryString["MainID"].ToString() + "'", "", Convert.ToDateTime(dtFrom.Value), Request.QueryString["SegmentID"].ToString(), Session["LastCompany"].ToString(), Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString()), Convert.ToInt32(Session["ActiveCurrency"].ToString().Split('~')[0]));

                }
                else
                {
                    //This code Will Be Run Through GeneralTrail --- Subsidiary Trail---Ledger
                    DataSet dsCnt = new DataSet();
                    dsCnt = objFAReportsOther.Fetch_LedgerView(
                        Convert.ToString(Session["LastCompany"]),
                        Convert.ToString(Session["LastFinYear"]),
                         Convert.ToString(dtFrom.Value),
                         Convert.ToString(dtTo.Value),
                        Convert.ToString(Request.QueryString["MainID"]),
                        Convert.ToString(Request.QueryString["SubID"]),
                        Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]),
                        "ConsolidateDt",
                        Convert.ToString(Request.QueryString["SegmentID"]),
                        Convert.ToString(Session["userid"]),
                        "",
                        "all",
                        "n",
                        "n",
                        "n",
                        "",
                        "",
                        Session["ActiveCurrency"].ToString().Split('~')[0],
                        Session["TradeCurrency"].ToString().Split('~')[0]);
                    dtCashBankBook = dsCnt.Tables[0];
                    dtLedgerView = dsCnt.Tables[0];
                    OpenBalance = dsCnt.Tables[1];


                }
            }
            else
            {

                //string ReportType = "";
                string SubAc = "";
                string mainAccountSearch = null;
                string SubAccountSearch = null;
                string SubACountForAll = null;
                string Settlement = "";
                string strTranType = "";
                string strCbPayment = "n";
                string strCbReceipt = "n";
                string strCbContract = "n";
                string strJvType = "";
                string strSelectedJv = "";

                if (RadSettA.Checked == true)
                {
                    Settlement = "";
                }
                else
                {
                    Settlement = HdnSettlement.Value;
                }

                ViewState["Clients"] = null;
                ViewState["branchID"] = null;
                SubLedgerType = HdnSubLedgerType.Value;
                SegmentT = HdnBranch.Value;
                SubAcID = HdnSubAc.Value;
                ViewState["SubAcID"] = SubAcID;
                MainAcID = HdnMainAc.Value;
                if (SubLedgerType == "CDSL Clients" || SubLedgerType == "NSDL Clients")
                    fn_ClientCDSL();
                else if (HdnForBranchGroup.Value != "a")
                    fn_Client();
                if (HdnSelectLedger.Value != "S")
                {
                    if (DrpChange != "Y")
                    {
                        FillDropDown();
                    }
                }

                SegMentName = ViewState["SegMentName"].ToString();
                ViewState["Check"] = "a";
                if (rdbSegAll.Checked == true)
                {
                    DataTable dtSegment = oDBEngine.GetDataTable("tbl_master_companyExchange", "exch_internalId", "exch_compId='" + Session["CompanyID"].ToString() + "'");
                    if (dtSegment.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtSegment.Rows.Count; i++)
                        {
                            if (Segment == null)
                                Segment = dtSegment.Rows[i][0].ToString();
                            else
                                Segment += "," + dtSegment.Rows[i][0].ToString();
                        }
                    }
                }
                else
                {
                    if (Session["userlastsegment"].ToString() == "5")
                    {
                        DataTable DtSeg = new DataTable();
                        DtSeg = oDBEngine.GetDataTable("(select exch_compId,exch_internalId,isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Comp from tbl_master_companyExchange where exch_compid in (select ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment=" + Session["userlastsegment"].ToString() + ")) as D", "*", "Comp in(select seg_name from tbl_master_segment where seg_id=" + Session["userlastsegment"].ToString() + ") and  exch_compID='" + Session["LastCompany"].ToString() + "'");
                        Segment = DtSeg.Rows[0][1].ToString();

                    }
                    else
                    {
                        if (SegmentT == null || SegmentT == "")
                        {
                            Segment = Session["SegmentID"].ToString();
                        }
                        else
                            Segment = SegmentT;
                    }
                }
                if (ViewState["branchID"] == null)
                {
                    if (rdbranchAll.Checked == true)
                    {
                        Branch = Session["userbranchHierarchy"].ToString();
                    }
                }
                else
                    Branch = ViewState["branchID"].ToString();


                if (rdSubAcAll.Checked == true)
                {
                    SubACountForAll = null;
                }
                else
                {
                    if (SubAcID == null || SubAcID == "")
                        SubACountForAll = null;
                    else
                        SubACountForAll = SubAcID;
                }
                if (ViewState["Clients"] != null)
                    SubACountForAll = ViewState["Clients"].ToString();
                if (ddlAccountType.SelectedValue == "0")
                {
                    mainAccountSearch = "'SYSTM00001'";
                    MainAcIDforOp = "'SYSTM00001'";
                    SubLedgerType = "Customers";
                }
                else if (ddlAccountType.SelectedValue == "1")
                {
                    mainAccountSearch = "'SYSTM00002'";
                    MainAcIDforOp = "'SYSTM00002'";
                    SubLedgerType = "Customers";
                }
                else if (ddlAccountType.SelectedValue == "2")
                {
                    mainAccountSearch = "'SYSTM00001','SYSTM00002'";
                    MainAcIDforOp = "'SYSTM00001','SYSTM00002'";
                    SubLedgerType = "Customers";
                }
                else
                {
                    MainAcIDforOp = MainAcID;
                    mainAccountSearch = MainAcID;
                }
                ViewState["MainAcIDforOp"] = MainAcIDforOp;
                ViewState["Segment"] = Segment;


                if (SubLedgerType.Trim() == "None")
                {
                    SubAccountSearch = null;
                    ViewState["SubAccountSearch"] = SubAccountSearch;
                }
                else
                {
                    if (cmbclientsPager.SelectedItem != null)
                        SubAccountSearch = "'" + cmbclientsPager.SelectedItem.Value + "'";
                    else
                        SubAccountSearch = SubAcID;

                }
                if (radConsolidated.Checked == true)
                {
                    if (radDateWise.Checked == true)
                    {
                        ReportType = "ConsolidateDt";
                    }
                    else if (radExpDateWise.Checked == true)
                    {

                        ReportType = "ConsolidateExp~" + litSegment.InnerText.Split('-')[1].Replace("'", "").Trim();
                    }
                    else
                    {

                        ReportType = "ConsolidateV";
                    }

                }
                else if (radBreakDetail.Checked == true)
                {
                    ReportType = "ObligationBrkUp";

                }
                else
                {
                    ReportType = "Detail";

                }

                if (SubAccountSearch == null)
                    SubAccountSearch = "";

                if (ddlAccountType.SelectedItem.Value.ToString() == "0" || ddlAccountType.SelectedItem.Value.ToString() == "1" || ddlAccountType.SelectedItem.Value.ToString() == "2")
                {
                    if (SubLedgerType.Trim() == "None")
                    {
                        SubLedgerType = "";
                    }
                }

                if (rbTanAll.Checked == true)
                    strTranType = "all";
                else if (rbTranCashBank.Checked == true)
                {
                    strTranType = "cb";
                    if (chkPayment.Checked == true)
                        strCbPayment = "y";
                    if (chkReceipts.Checked == true)
                        strCbReceipt = "y";
                    if (chkContracts.Checked == true)
                        strCbContract = "y";

                }
                else if (rbTranJv.Checked == true)
                {
                    strTranType = "jv";
                    if (rbAllJV.Checked == true)
                        strJvType = "all";
                    else if (rbManualJV.Checked == true)
                    {
                        strJvType = "man";
                        strSelectedJv = txtVoucherPrefix.Text;
                    }
                    else if (rbSystemJV.Checked == true)
                    {
                        strJvType = "sys";
                        //strSelectedJv = ViewState["Selectedjvs"].ToString();
                        if (rbSystemJVAll.Checked == true)
                            strSelectedJv = "all";
                        else if (rbSystemJVSelected.Checked == true)
                            strSelectedJv = hdnSystemJvs.Value;
                    }

                }

                if ((SubLedgerType.Trim() != "None" && cmbclientsPager.Items.Count != 0) || (SubLedgerType.Trim() == "None" && cmbclientsPager.Items.Count == 0))
                {
                    DataSet DS = new DataSet();

                    if (radBreakDetail.Checked == true)
                    {
                        DS = objFAReportsOther.Fetch_LedgerForCrystalReport(
                       Convert.ToString(Session["LastCompany"]),
                       Convert.ToString(Session["LastFinYear"]),
                        Convert.ToString(dtFrom.Value),
                        Convert.ToString(dtTo.Value),
                       Convert.ToString(mainAccountSearch),
                       Convert.ToString(SubAccountSearch),
                       Convert.ToString(Branch),
                       Convert.ToString(ReportType),
                       Convert.ToString(Segment),
                       Convert.ToString(Session["userid"]),
                       Convert.ToString(SubLedgerType.Trim()),
                       Convert.ToString(Settlement),
                       Convert.ToString(strTranType),
                       Convert.ToString(strCbPayment),
                       Convert.ToString(strCbReceipt),
                        Convert.ToString(strCbContract),
                            Convert.ToString(strJvType),
                       Convert.ToString(strSelectedJv));
                        ViewState["dsHTML"] = DS;
                        GenerateHTML();

                    }
                    else
                    {
                        //For Sever Debugger Variable
                        string[,] strParam = new string[19, 2];
                        string SpName = String.Empty;



                        //SD Code (Server Debugging Code)
                        strParam[0, 0] = "CompanyID"; strParam[0, 1] = "'" + Session["LastCompany"].ToString() + "'";
                        strParam[1, 0] = "FinYear"; strParam[1, 1] = "'" + Session["LastFinYear"].ToString() + "'";
                        strParam[2, 0] = "FromDate"; strParam[2, 1] = "'" + dtFrom.Value + "'";
                        strParam[3, 0] = "ToDate"; strParam[3, 1] = "'" + dtTo.Value + "'";
                        strParam[4, 0] = "MainAccount"; strParam[4, 1] = "'" + mainAccountSearch + "'";
                        strParam[5, 0] = "SubAccount"; strParam[5, 1] = "'" + SubAccountSearch + "'";
                        strParam[6, 0] = "Branch"; strParam[6, 1] = "'" + Branch + "'";
                        strParam[7, 0] = "ReportType"; strParam[7, 1] = "'" + ReportType + "'";
                        strParam[8, 0] = "Segment"; strParam[8, 1] = "'" + Segment + "'";
                        strParam[9, 0] = "UserID"; strParam[9, 1] = "'" + Session["userid"].ToString() + "'";
                        strParam[10, 0] = "Settlement"; strParam[10, 1] = "'" + Settlement + "'";
                        strParam[11, 0] = "TranType"; strParam[11, 1] = "'" + strTranType + "'";
                        strParam[12, 0] = "CbPayment"; strParam[12, 1] = "'" + Convert.ToChar(strCbPayment) + "'";
                        strParam[13, 0] = "CbReceipt"; strParam[13, 1] = "'" + Convert.ToChar(strCbReceipt) + "'";
                        strParam[14, 0] = "CbContract"; strParam[14, 1] = "'" + Convert.ToChar(strCbContract) + "'";
                        strParam[15, 0] = "JvType"; strParam[15, 1] = "'" + strJvType + "'";
                        strParam[16, 0] = "SelectedJv"; strParam[16, 1] = "'" + strSelectedJv + "'";
                        strParam[17, 0] = "ActiveCurrency"; strParam[17, 1] = "'" + Session["ActiveCurrency"].ToString().Split('~')[0] + "'";
                        strParam[18, 0] = "TradeCurrency"; strParam[18, 1] = "'" + Session["TradeCurrency"].ToString().Split('~')[0] + "'";

                        //For Server Debugging Purpose
                        oGenericMethod = new BusinessLogicLayer.GenericMethod();
                        if (oGenericMethod.CallGeneric_ScalerFunction_Int("GetGlobalSettingsValue", Session["UserSegID"].ToString() + "~GS_DEBUGSTATE") == 1)
                        {
                            string strDateTime = oGenericMethod.GetDate().ToString("yyyyMMddHHmmss");
                            string FilePath = "../ExportFiles/ServerDebugging/Fetch_LedgerView" + strDateTime + ".txt";
                            oGenericMethod.WriteFile(oGenericMethod.OldSpExecuteWriter(strParam, "Fetch_LedgerView"), FilePath, false);
                        }

                        DataSet dsCnt = new DataSet();
                        dsCnt = objFAReportsOther.Fetch_LedgerView(
                            Convert.ToString(Session["LastCompany"]),
                            Convert.ToString(Session["LastFinYear"]),
                            Convert.ToString(dtFrom.Value),
                            Convert.ToString(dtTo.Value),
                            Convert.ToString(mainAccountSearch),
                            Convert.ToString(SubAccountSearch),
                            Convert.ToString(Branch),
                            Convert.ToString(ReportType),
                             Convert.ToString(Segment),
                             Convert.ToString(Session["userid"]),
                            Convert.ToString(Settlement),
                            Convert.ToString(strTranType),
                           Convert.ToString(strCbPayment),
                           Convert.ToString(strCbReceipt),
                           Convert.ToString(strCbContract),
                           Convert.ToString(strJvType),
                           Convert.ToString(strSelectedJv),
                            Session["ActiveCurrency"].ToString().Split('~')[0],
                            Session["TradeCurrency"].ToString().Split('~')[0]);
                        dtCashBankBook = dsCnt.Tables[0];
                        dtLedgerView = dsCnt.Tables[0];
                        OpenBalance = dsCnt.Tables[1];



                    }
                }
                else
                {

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "AlertScript", "alert('No  Record Found!')", true);
                }

            }
            if (radBreakDetail.Checked == false)
            {

                ViewState["dtCashBankBook"] = dtCashBankBook;
                ViewState["dtLedgerView"] = dtLedgerView;
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
                    //newRow[3] = dtCashBankBook.Rows[i]["SegmentName"];
                    newRow[4] = dtCashBankBook.Rows[i]["AccountName"];
                    newRow[5] = dtCashBankBook.Rows[i]["Accountsledger_AmountCr"];
                    newRow[6] = dtCashBankBook.Rows[i]["Accountsledger_AmountDr"];
                    newRow[7] = dtCashBankBook.Rows[i]["Closing"];
                    newRow[8] = dtCashBankBook.Rows[i]["accountsledger_transactiondate"];
                    newRow[9] = dtCashBankBook.Rows[i]["accountsledger_InstrumentNumber"];
                    if (dtCashBankBook.Rows[i]["SettlementNumber"].ToString().Contains("F"))
                        newRow[10] = "";
                    else
                        newRow[10] = dtCashBankBook.Rows[i]["SettlementNumber"];
                    newRow[11] = dtCashBankBook.Rows[i]["MainID"];
                    newRow[12] = dtCashBankBook.Rows[i]["SubID"];
                    newRow[13] = dtCashBankBook.Rows[i]["CompanyID"];
                    newRow[14] = dtCashBankBook.Rows[i]["SegID"];
                    newRow[15] = dtCashBankBook.Rows[i]["CashType"];
                    if (dtCashBankBook.Rows[i]["SegmentName"].ToString().Contains("CM"))
                        newRow[16] = dtCashBankBook.Rows[i]["PayoutDate"];
                    else
                        newRow[16] = dtCashBankBook.Rows[i]["TrDate"];
                    newRow[17] = dtCashBankBook.Rows[i]["BranchCode"];
                    newRow[18] = dtCashBankBook.Rows[i]["UserID"];
                    newRow[19] = dtCashBankBook.Rows[i]["SegmentName"];
                    dtCashBankBook_New.Rows.Add(newRow);
                    if ((dtCashBankBook.Rows[i]["accountsledger_transactiondate"].ToString() != "") || (dtCashBankBook.Rows[i]["accountsledger_transactiondate"].ToString() != "NA"))
                        TranDate = Convert.ToDateTime(dtCashBankBook.Rows[i]["accountsledger_transactiondate"].ToString());
                    else
                        TranDate = Convert.ToDateTime("1900-01-01");
                    if (dtCashBankBook.Rows[i]["Accountsledger_AmountDr"].ToString() != "")
                        receipt += Convert.ToDecimal(dtCashBankBook.Rows[i]["Accountsledger_AmountDr"].ToString());
                    if (dtCashBankBook.Rows[i]["Accountsledger_AmountCr"].ToString() != "")
                        Payment += Convert.ToDecimal(dtCashBankBook.Rows[i]["Accountsledger_AmountCr"].ToString());
                    #region
                    if (radBreakDetail.Checked == true)
                    {
                        string[] Narration1;
                        string[] Bill1;
                        string Bill = "0";
                        string TranID = dtCashBankBook.Rows[i]["accountsledger_TransactionReferenceID"].ToString().Substring(0, 2);
                        string[] Narration = dtCashBankBook.Rows[i]["accountsledger_Narration"].ToString().Split('[');
                        if (Narration.Length > 1)
                        {
                            Narration1 = Narration[1].Split(':');
                            if (Narration1.Length > 1)
                            {
                                Bill1 = Narration1[1].Split(']');
                                Bill = Bill1[0];
                            }
                        }
                        if (SegMentName == "NSE - CM" || SegMentName == "BSE - CM" || SegMentName == "NSE - CM,BSE - CM")
                        {
                            if (TranID == "XO")
                            {
                                DataTable dtProduct = oDBEngine.GetDataTable("Trans_CMPosition", "(select ltrim(rtrim(isnull(Equity_TickerSymbol,'')))+(case when isnull(convert(varchar(12),Equity_EffectUntil,113),'')='' then '' else ' ['+convert(varchar(12),Equity_EffectUntil,113)+']' end)+' ['+ltrim(rtrim(isnull(Equity_Series,rtrim(isnull(Equity_TickerCode,'')))))+']'+ case when isnull(cast(Equity_StrikePrice as varchar),'')='' then '' else ' ['+cast(Equity_StrikePrice as varchar)+']' end from master_Equity where Equity_SeriesID=Trans_CMPosition.CMPosition_ProductSeriesID) as CMPosition_ProductSeriesID,case when CMPosition_SqrOffQty=0 then null else CMPosition_SqrOffQty end as CMPosition_SqrOffQty,case when CMPosition_SqrOffPL=0 then null else CMPosition_SqrOffPL end as CMPosition_SqrOffPL,case when CMPosition_DeliveryBuyQty=0 then null else CMPosition_DeliveryBuyQty end as CMPosition_DeliveryBuyQty,case when CMPosition_DeliveryBuyValue=0 then null else CMPosition_DeliveryBuyValue end as CMPosition_DeliveryBuyValue,case when CMPosition_DeliverySellQty=0 then null else CMPosition_DeliverySellQty end as CMPosition_DeliverySellQty,case when CMPosition_DeliverySellValue=0 then null else CMPosition_DeliverySellValue end as CMPosition_DeliverySellValue,case when CMPosition_NetObligation=0 then null else CMPosition_NetObligation end as CMPosition_NetObligation,CMPosition_SettlementNumber+CMPosition_SettlementType as SettnumType,case when (isnull(CMPosition_DeliveryBuyValue,0)/isnull((case when CMPosition_DeliveryBuyQty=0 then null else CMPosition_DeliveryBuyQty end),1))=0 then null else (isnull(CMPosition_DeliveryBuyValue,0)/isnull((case when CMPosition_DeliveryBuyQty=0 then null else CMPosition_DeliveryBuyQty end),1)) end as AvgBuyVal,case when (isnull(CMPosition_DeliverySellValue,0)/isnull((case when CMPosition_DeliverySellQty=0 then null else CMPosition_DeliverySellQty end),1))=0 then null else (isnull(CMPosition_DeliverySellValue,0)/isnull((case when CMPosition_DeliverySellQty=0 then null else CMPosition_DeliverySellQty end),1)) end as AvgSellVal", " CmPosition_BillNumber='" + Bill + "'");
                                DataRow newRow1 = dtCashBankBook_New.NewRow();
                                String strHtmlAllClient = String.Empty;
                                decimal TotalVal = 0;
                                strHtmlAllClient = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + ">";
                                strHtmlAllClient += "<tr style=\"background-color: #DBEEF3;\">";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Scrip</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Delv.Buy Qty</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Delv.Buy Value</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Avg.Buy Value</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Delv.Sell Qty</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Delv.Sell Value</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Avg.Sell Value</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Sqr Qty</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Sqr P/L</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Total</td>";
                                strHtmlAllClient += "</tr>";
                                for (int j = 0; j < dtProduct.Rows.Count; j++)
                                {
                                    strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                                    strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\">" + dtProduct.Rows[j]["CMPosition_ProductSeriesID"] + "</td>";
                                    if (dtProduct.Rows[j]["CMPosition_DeliveryBuyQty"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["CMPosition_DeliveryBuyQty"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["CMPosition_DeliveryBuyValue"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["CMPosition_DeliveryBuyValue"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["AvgBuyVal"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["AvgBuyVal"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["CMPosition_DeliverySellQty"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["CMPosition_DeliverySellQty"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["CMPosition_DeliverySellValue"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["CMPosition_DeliverySellValue"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["AvgSellVal"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["AvgSellVal"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["CMPosition_SqrOffQty"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["CMPosition_SqrOffQty"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["CMPosition_SqrOffPL"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["CMPosition_SqrOffPL"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["CMPosition_NetObligation"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["CMPosition_NetObligation"])) + "</td></tr>";
                                        TotalVal += Convert.ToDecimal(dtProduct.Rows[j]["CMPosition_NetObligation"]);
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td></tr>";
                                    }
                                }
                                strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                                strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\"><b>Total :</b></td>";
                                strHtmlAllClient += "<td align=\"right\" colspan=\"9\" style=\"font-size:xx-small\"><b>" + oconverter.getFormattedvaluewithoriginalsign(TotalVal) + "</b></td></tr>";
                                strHtmlAllClient += "</table>";
                                newRow1[0] = "a";
                                newRow1[1] = dtProduct.Rows[0]["SettnumType"].ToString();
                                newRow1[2] = "";
                                newRow1[3] = strHtmlAllClient;
                                newRow1[4] = "";
                                newRow1[5] = 0;
                                newRow1[6] = 0;
                                newRow1[7] = "";
                                newRow1[8] = dtCashBankBook.Rows[i]["accountsledger_transactiondate"];
                                newRow1[9] = "";
                                newRow1[10] = "";
                                newRow1[11] = "";
                                newRow1[12] = "";
                                newRow1[13] = "";
                                newRow1[14] = "";
                                newRow1[15] = "";
                                newRow1[16] = "";
                                dtCashBankBook_New.Rows.Add(newRow1);
                                //displayALLCLIENT.InnerHtml = strHtmlAllClient;
                            }
                        }
                        else if (SegMentName == "NSE-FO")
                        {
                            if (TranID == "XO")
                            {
                                DataTable dtProduct = new DataTable();
                                if (TranDate != Convert.ToDateTime("1900-01-01"))
                                    dtProduct = oDBEngine.GetDataTable("trans_foposition", "(select ltrim(rtrim(isnull(Equity_TickerSymbol,'')))+(case when isnull(convert(varchar(12),Equity_EffectUntil,113),'')='' then '' else ' ['+convert(varchar(12),Equity_EffectUntil,113)+']' end)+' ['+ltrim(rtrim(isnull(Equity_Series,'')))+']'+ case when isnull(CONVERT(VARCHAR,CONVERT(MONEY,cast(Equity_StrikePrice as varchar)),1),'')='' then '' when Equity_StrikePrice=0 then '' else ' ['+CONVERT(VARCHAR,CONVERT(MONEY,cast(Equity_StrikePrice as varchar)),1)+']' end from master_Equity where Equity_SeriesID=trans_foposition.FOPosition_ProductSeriesID) as FOPosition_ProductSeriesID,case when isnull(foposition_BFPriceUnits,0)=0 then null else foposition_BFPriceUnits end as foposition_BFPriceUnits,case when isnull(foposition_openprice,0)=0 then null else foposition_openprice end as foposition_openprice,case when isnull(foposition_buyLots,0)=0 then null else foposition_buyLots end as foposition_buyPriceUnits,case when isnull(foposition_BuyValue,0)=0 then foposition_BuyValue else foposition_BuyValue end as foposition_BuyValue,case when isnull(foposition_BuyAverage,0)=0 then null else foposition_BuyAverage end as foposition_BuyAverage,case when isnull(foposition_sellLots,0)=0 then null else foposition_sellLots end as foposition_sellPriceUnits,case when isnull(foposition_SellValue,0)=0 then null else foposition_SellValue end as foposition_SellValue,case when isnull(foposition_SellAverage,0)=0 then null else foposition_SellAverage end as foposition_SellAverage,case when isnull(foposition_PostExcAsnDlvLongPriceUnits,0)=0 then foposition_PostExcAsnDlvShortPriceUnits else (-1)*foposition_PostExcAsnDlvLongPriceUnits end as CFQty,case when foposition_PostExcAsnDlvLongValue=0 then foposition_PostExcAsnDlvShortValue else foposition_PostExcAsnDlvLongValue end as CFPrice,foposition_MTMPL,(select DailyStat_SettlementPrice from Trans_dailyStatistics where DailyStat_ProductSeriesID=trans_foposition.FOPosition_ProductSeriesID and DailyStat_ExchangeSegmentID=" + Session["ExchangeSegmentID"].ToString() + " and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,DailyStat_DateTime)) as datetime) =cast(DATEADD(dd, 0, DATEDIFF(dd, 0, '" + TranDate + "')) as datetime)) as SettPrice", " foposition_BillNumber='" + Bill + "' and foposition_productseriesid in(select equity_seriesid from master_equity where equity_foidentifier like 'FUT%' and equity_effectuntil<>'" + dtCashBankBook.Rows[i]["accountsledger_transactiondate"] + "')");
                                DataRow newRow1 = dtCashBankBook_New.NewRow();
                                String strHtmlAllClient = String.Empty;
                                decimal TotalVal = 0;
                                strHtmlAllClient = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + ">";
                                strHtmlAllClient += "<tr style=\"background-color: #DBEEF3;\">";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Scrip</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">B/F Qty</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Price</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Buy Qty</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Value</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Avg</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Sell Qty</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Value</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Avg</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">C/F Qty</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Price</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">MTM</td>";
                                strHtmlAllClient += "</tr>";
                                for (int j = 0; j < dtProduct.Rows.Count; j++)
                                {
                                    strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                                    strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\">" + dtProduct.Rows[j]["FOPosition_ProductSeriesID"] + "</td>";
                                    if (dtProduct.Rows[j]["foposition_BFPriceUnits"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_BFPriceUnits"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["foposition_openprice"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_openprice"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["foposition_buyPriceUnits"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_buyPriceUnits"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["foposition_BuyValue"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_BuyValue"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["foposition_BuyAverage"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_BuyValue"]) / Convert.ToDecimal(dtProduct.Rows[j]["foposition_buyPriceUnits"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["foposition_sellPriceUnits"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_sellPriceUnits"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["foposition_SellValue"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_SellValue"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["foposition_SellAverage"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_SellValue"]) / Convert.ToDecimal(dtProduct.Rows[j]["foposition_sellPriceUnits"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["CFQty"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["CFQty"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["SettPrice"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["SettPrice"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["foposition_MTMPL"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_MTMPL"])) + "</td></tr>";
                                        TotalVal += Convert.ToDecimal(dtProduct.Rows[j]["foposition_MTMPL"]);
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td></tr>";
                                    }
                                }
                                strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                                strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\"><b>Total :</b></td>";
                                strHtmlAllClient += "<td align=\"right\" colspan=\"11\" style=\"font-size:xx-small\"><b>" + oconverter.getFormattedvaluewithoriginalsign(TotalVal) + "</b></td></tr>";
                                strHtmlAllClient += "</table>";
                                newRow1[0] = "a";
                                newRow1[1] = "";
                                newRow1[2] = "";
                                newRow1[3] = strHtmlAllClient;
                                newRow1[4] = "";
                                newRow1[5] = 0;
                                newRow1[6] = 0;
                                newRow1[7] = "";
                                newRow1[8] = dtCashBankBook.Rows[i]["accountsledger_transactiondate"];
                                newRow1[9] = "";
                                newRow1[10] = "";
                                newRow1[11] = "";
                                newRow1[12] = "";
                                newRow1[13] = "";
                                newRow1[14] = "";
                                newRow1[15] = "";
                                newRow1[16] = "";
                                dtCashBankBook_New.Rows.Add(newRow1);
                                //displayALLCLIENT.InnerHtml = strHtmlAllClient;
                            }
                            if (TranID == "XP")
                            {
                                DataTable dtProduct = oDBEngine.GetDataTable("trans_foposition", "(select ltrim(rtrim(isnull(Equity_TickerSymbol,'')))+(case when isnull(convert(varchar(12),Equity_EffectUntil,113),'')='' then '' else ' ['+convert(varchar(12),Equity_EffectUntil,113)+']' end)+' ['+ltrim(rtrim(isnull(Equity_Series,'')))+']'+ case when isnull(CONVERT(VARCHAR,CONVERT(MONEY,cast(Equity_StrikePrice as varchar)),1),'')='' then '' when Equity_StrikePrice=0 then '' else ' ['+CONVERT(VARCHAR,CONVERT(MONEY,cast(Equity_StrikePrice as varchar)),1)+']' end from master_Equity where Equity_SeriesID=trans_foposition.FOPosition_ProductSeriesID) as FOPosition_ProductSeriesID,case when isnull(foposition_BuyLots,0)=0 then null else foposition_BuyLots end as foposition_BuyLots,case when isnull(foposition_BuyValue,0)=0 then null else foposition_BuyValue end as foposition_BuyValue,case when isnull(foposition_BuyAverage,0)=0 then null else foposition_BuyAverage end as foposition_BuyAverage,case when isnull(foposition_SellLots,0)=0 then null else foposition_SellLots end as foposition_SellLots,case when isnull(foposition_SellValue,0)=0 then null else foposition_SellValue end as foposition_SellValue,case when isnull(foposition_SellAverage,0)=0 then null else foposition_SellAverage end as foposition_SellAverage,case when isnull(foposition_NetPremium,0)=0 then null else foposition_NetPremium end as foposition_NetPremium ", " foposition_BillNumber='" + Bill + "' and  foposition_NetPremium is not null");
                                DataRow newRow1 = dtCashBankBook_New.NewRow();
                                String strHtmlAllClient = String.Empty;
                                decimal TotalVal = 0;
                                strHtmlAllClient = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + ">";
                                strHtmlAllClient += "<tr style=\"background-color: #DBEEF3;\">";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Scrip</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Buy Qty</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Value</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Avg</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Sell Qty</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Value</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Avg</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Net Premium</td>";
                                strHtmlAllClient += "</tr>";
                                for (int j = 0; j < dtProduct.Rows.Count; j++)
                                {
                                    strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                                    strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\">" + dtProduct.Rows[j]["FOPosition_ProductSeriesID"] + "</td>";
                                    if (dtProduct.Rows[j]["foposition_BuyLots"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_BuyLots"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["foposition_BuyValue"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_BuyValue"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["foposition_BuyAverage"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_BuyValue"]) / Convert.ToDecimal(dtProduct.Rows[j]["foposition_BuyLots"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["foposition_SellLots"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_SellLots"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["foposition_SellValue"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_SellValue"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["foposition_SellAverage"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_SellValue"]) / Convert.ToDecimal(dtProduct.Rows[j]["foposition_SellLots"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["foposition_NetPremium"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_NetPremium"])) + "</td></tr>";
                                        TotalVal += Convert.ToDecimal(dtProduct.Rows[j]["foposition_NetPremium"]);
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td></tr>";
                                    }
                                }
                                strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                                strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\"><b>Total :</b></td>";
                                strHtmlAllClient += "<td align=\"right\" colspan=\"11\" style=\"font-size:xx-small\"><b>" + oconverter.getFormattedvaluewithoriginalsign(TotalVal) + "</b></td></tr>";
                                strHtmlAllClient += "</table>";
                                newRow1[0] = "a";
                                newRow1[1] = "";
                                newRow1[2] = "";
                                newRow1[3] = strHtmlAllClient;
                                newRow1[4] = "";
                                newRow1[5] = 0;
                                newRow1[6] = 0;
                                newRow1[7] = "";
                                newRow1[8] = dtCashBankBook.Rows[i]["accountsledger_transactiondate"];
                                newRow1[9] = "";
                                newRow1[10] = "";
                                newRow1[11] = "";
                                newRow1[12] = "";
                                newRow1[13] = "";
                                newRow1[14] = "";
                                newRow1[15] = "";
                                newRow1[16] = "";
                                dtCashBankBook_New.Rows.Add(newRow1);
                                //displayALLCLIENT.InnerHtml = strHtmlAllClient;
                            }
                            if (TranID == "XZ")
                            {
                                DataTable dtProduct = oDBEngine.GetDataTable("trans_foposition", "(select ltrim(rtrim(isnull(Equity_TickerSymbol,'')))+(case when isnull(convert(varchar(12),Equity_EffectUntil,113),'')='' then '' else ' ['+convert(varchar(12),Equity_EffectUntil,113)+']' end)+' ['+ltrim(rtrim(isnull(Equity_Series,'')))+']'+ case when isnull(CONVERT(VARCHAR,CONVERT(MONEY,cast(Equity_StrikePrice as varchar)),1),'')='' then '' when Equity_StrikePrice=0 then '' else ' ['+CONVERT(VARCHAR,CONVERT(MONEY,cast(Equity_StrikePrice as varchar)),1)+']' end from master_Equity where Equity_SeriesID=trans_foposition.FOPosition_ProductSeriesID) as FOPosition_ProductSeriesID,case when foposition_ExcAsnDlvLots<0 then 'Exercise' else 'Assigned' end as SettType,case when isnull(foposition_ExcAsnDlvLots,0)=0 then null else abs(foposition_ExcAsnDlvLots) end as foposition_ExcAsnDlvLots,case when isnull(foposition_SettlementPrice,0)=0 then null else abs(foposition_SettlementPrice) end as foposition_SettlementPrice,case when isnull(foposition_ExcAsnDlvMarkedValue,0)=0 then null else foposition_ExcAsnDlvMarkedValue end as foposition_ExcAsnDlvMarkedValue ", " foposition_BillNumber='" + Bill + "' and  foposition_ExcAsnDlvMarkedValue is not null");
                                DataRow newRow1 = dtCashBankBook_New.NewRow();
                                String strHtmlAllClient = String.Empty;
                                decimal TotalVal = 0;
                                strHtmlAllClient = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + ">";
                                strHtmlAllClient += "<tr style=\"background-color: #DBEEF3;\">";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Scrip</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Sett.Type</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Qty</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Set.Price</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Net Amount</td>";
                                strHtmlAllClient += "</tr>";
                                for (int j = 0; j < dtProduct.Rows.Count; j++)
                                {
                                    strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                                    strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\">" + dtProduct.Rows[j]["FOPosition_ProductSeriesID"] + "</td>";
                                    strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\">" + dtProduct.Rows[j]["SettType"] + "</td>";
                                    if (dtProduct.Rows[j]["foposition_ExcAsnDlvLots"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_ExcAsnDlvLots"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["foposition_SettlementPrice"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_SettlementPrice"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["foposition_ExcAsnDlvMarkedValue"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_ExcAsnDlvMarkedValue"])) + "</td></tr>";
                                        TotalVal += Convert.ToDecimal(dtProduct.Rows[j]["foposition_ExcAsnDlvMarkedValue"]);
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td></tr>";
                                    }
                                }
                                strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                                strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\"><b>Total :</b></td>";
                                strHtmlAllClient += "<td align=\"right\" colspan=\"11\" style=\"font-size:xx-small\"><b>" + oconverter.getFormattedvaluewithoriginalsign(TotalVal) + "</b></td></tr>";
                                strHtmlAllClient += "</table>";
                                newRow1[0] = "a";
                                newRow1[1] = "";
                                newRow1[2] = "";
                                newRow1[3] = strHtmlAllClient;
                                newRow1[4] = "";
                                newRow1[5] = 0;
                                newRow1[6] = 0;
                                newRow1[7] = "";
                                newRow1[8] = dtCashBankBook.Rows[i]["accountsledger_transactiondate"];
                                newRow1[9] = "";
                                newRow1[10] = "";
                                newRow1[11] = "";
                                newRow1[12] = "";
                                newRow1[13] = "";
                                newRow1[14] = "";
                                newRow1[15] = "";
                                newRow1[16] = "";
                                dtCashBankBook_New.Rows.Add(newRow1);
                                //displayALLCLIENT.InnerHtml = strHtmlAllClient;
                            }
                            if (TranID == "XX")
                            {
                                DataTable dtProduct = new DataTable();
                                if (TranDate != Convert.ToDateTime("1900-01-01"))
                                    dtProduct = oDBEngine.GetDataTable("trans_foposition", "(select ltrim(rtrim(isnull(Equity_TickerSymbol,'')))+(case when isnull(convert(varchar(12),Equity_EffectUntil,113),'')='' then '' else ' ['+convert(varchar(12),Equity_EffectUntil,113)+']' end)+' ['+ltrim(rtrim(isnull(Equity_Series,'')))+']'+ case when isnull(CONVERT(VARCHAR,CONVERT(MONEY,cast(Equity_StrikePrice as varchar)),1),'')='' then '' when Equity_StrikePrice=0 then '' else ' ['+CONVERT(VARCHAR,CONVERT(MONEY,cast(Equity_StrikePrice as varchar)),1)+']' end from master_Equity where Equity_SeriesID=trans_foposition.FOPosition_ProductSeriesID) as FOPosition_ProductSeriesID,case when isnull(foposition_BFPriceUnits,0)=0 then null else foposition_BFPriceUnits end as foposition_BFPriceUnits,case when isnull(foposition_openprice,0)=0 then null else foposition_openprice end as foposition_openprice,case when isnull(foposition_buyLots,0)=0 then null else foposition_buyLots end as foposition_buyPriceUnits,case when isnull(foposition_BuyValue,0)=0 then foposition_BuyValue else foposition_BuyValue end as foposition_BuyValue,case when isnull(foposition_BuyAverage,0)=0 then null else foposition_BuyAverage end as foposition_BuyAverage,case when isnull(foposition_sellLots,0)=0 then null else foposition_sellLots end as foposition_sellPriceUnits,case when isnull(foposition_SellValue,0)=0 then null else foposition_SellValue end as foposition_SellValue,case when isnull(foposition_SellAverage,0)=0 then null else foposition_SellAverage end as foposition_SellAverage,case when isnull(foposition_PostExcAsnDlvLongPriceUnits,0)=0 then foposition_PostExcAsnDlvShortPriceUnits else (-1)*foposition_PostExcAsnDlvLongPriceUnits end as CFQty,case when foposition_PostExcAsnDlvLongValue=0 then foposition_PostExcAsnDlvShortValue else foposition_PostExcAsnDlvLongValue end as CFPrice,foposition_FutureFinalSettlement,(select DailyStat_SettlementPrice from Trans_dailyStatistics where DailyStat_ProductSeriesID=trans_foposition.FOPosition_ProductSeriesID and DailyStat_ExchangeSegmentID=" + Session["ExchangeSegmentID"].ToString() + " and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,DailyStat_DateTime)) as datetime) =cast(DATEADD(dd, 0, DATEDIFF(dd, 0, '" + TranDate + "')) as datetime)) as SettPrice", " foposition_BillNumber='" + Bill + "' and foposition_productseriesid in(select equity_seriesid from master_equity where equity_foidentifier like 'FUT%' and equity_effectuntil='" + dtCashBankBook.Rows[i]["accountsledger_transactiondate"] + "')");
                                DataRow newRow1 = dtCashBankBook_New.NewRow();
                                String strHtmlAllClient = String.Empty;
                                decimal TotalVal = 0;
                                strHtmlAllClient = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + ">";
                                strHtmlAllClient += "<tr style=\"background-color: #DBEEF3;\">";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Scrip</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">B/F Qty</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Price</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Buy Qty</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Value</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Avg</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Sell Qty</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Value</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Avg</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Exp Qty</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Price</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Future Final Sett</td>";
                                strHtmlAllClient += "</tr>";
                                for (int j = 0; j < dtProduct.Rows.Count; j++)
                                {
                                    strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                                    strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\">" + dtProduct.Rows[j]["FOPosition_ProductSeriesID"] + "</td>";
                                    if (dtProduct.Rows[j]["foposition_BFPriceUnits"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_BFPriceUnits"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["foposition_openprice"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_openprice"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["foposition_buyPriceUnits"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_buyPriceUnits"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["foposition_BuyValue"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_BuyValue"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["foposition_BuyAverage"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_BuyValue"]) / Convert.ToDecimal(dtProduct.Rows[j]["foposition_buyPriceUnits"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["foposition_sellPriceUnits"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_sellPriceUnits"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["foposition_SellValue"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_SellValue"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["foposition_SellAverage"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_SellValue"]) / Convert.ToDecimal(dtProduct.Rows[j]["foposition_sellPriceUnits"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["CFQty"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["CFQty"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["SettPrice"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["SettPrice"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["foposition_FutureFinalSettlement"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_FutureFinalSettlement"])) + "</td></tr>";
                                        TotalVal += Convert.ToDecimal(dtProduct.Rows[j]["foposition_FutureFinalSettlement"]);
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td></tr>";
                                    }
                                }
                                strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                                strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\"><b>Total :</b></td>";
                                strHtmlAllClient += "<td align=\"right\" colspan=\"11\" style=\"font-size:xx-small\"><b>" + oconverter.getFormattedvaluewithoriginalsign(TotalVal) + "</b></td></tr>";
                                strHtmlAllClient += "</table>";
                                newRow1[0] = "a";
                                newRow1[1] = "";
                                newRow1[2] = "";
                                newRow1[3] = strHtmlAllClient;
                                newRow1[4] = "";
                                newRow1[5] = 0;
                                newRow1[6] = 0;
                                newRow1[7] = "";
                                newRow1[8] = dtCashBankBook.Rows[i]["accountsledger_transactiondate"];
                                newRow1[9] = "";
                                newRow1[10] = "";
                                newRow1[11] = "";
                                newRow1[12] = "";
                                newRow1[13] = "";
                                newRow1[14] = "";
                                newRow1[15] = "";
                                newRow1[16] = "";
                                dtCashBankBook_New.Rows.Add(newRow1);
                                //displayALLCLIENT.InnerHtml = strHtmlAllClient;
                            }
                        }
                        else if (SegMentName == "ICEX-COMM" || SegMentName == "MCX-COMM")
                        {
                            if (TranID == "XC")
                            {
                                DataTable dtProduct = oDBEngine.GetDataTable("Trans_comPosition", "(select ltrim(rtrim(isnull(Commodity_TickerSymbol,'')))+' ['+convert(varchar(12),Commodity_ExpiryDate,113)+']' from master_commodity where commodity_ProductSeriesID=trans_composition.comPosition_ProductSeriesID) as comPosition_ProductSeriesID,case when isnull(comPosition_BFPriceUnits,0)=0 then null else comPosition_BFPriceUnits end as comPosition_BFPriceUnits,case when isnull(comPosition_openprice,0)=0 then null else comPosition_openprice end as comPosition_openprice,case when isnull(comPosition_buyPriceUnits,0)=0 then null else comPosition_buyPriceUnits end as comPosition_buyPriceUnits,case when isnull(comPosition_BuyValue,0)=0 then comPosition_BuyValue else comPosition_BuyValue end as comPosition_BuyValue,case when isnull(comPosition_BuyAverage,0)=0 then null else comPosition_BuyAverage end as comPosition_BuyAverage,case when isnull(comPosition_sellPriceUnits,0)=0 then null else comPosition_sellPriceUnits end as comPosition_sellPriceUnits,case when isnull(comPosition_SellValue,0)=0 then null else comPosition_SellValue end as comPosition_SellValue,case when isnull(comPosition_SellAverage,0)=0 then null else comPosition_SellAverage end as comPosition_SellAverage,case when isnull(comPosition_PostExcAsnDlvLongPriceUnits,0)=0 then comPosition_PostExcAsnDlvShortPriceUnits else comPosition_PostExcAsnDlvLongPriceUnits end as CFQty,case when isnull(comPosition_PostExcAsnDlvLongValue,0)=0 then case when isnull(comPosition_PostExcAsnDlvShortValue,0)/abs(isnull(comPosition_PostExcAsnDlvShortPriceUnits,1))=0 then null else isnull(comPosition_PostExcAsnDlvShortValue,0)/abs(isnull(comPosition_PostExcAsnDlvShortPriceUnits,1)) end else case when isnull(comPosition_PostExcAsnDlvLongValue,0)/abs(isnull(comPosition_PostExcAsnDlvLongPriceUnits,1))=0 then null else isnull(comPosition_PostExcAsnDlvLongValue,0)/abs(isnull(comPosition_PostExcAsnDlvLongPriceUnits,1)) end end as CFPrice,comPosition_MTMPL", " comPosition_BillNumber='" + Bill + "'");
                                DataRow newRow1 = dtCashBankBook_New.NewRow();
                                String strHtmlAllClient = String.Empty;
                                decimal TotalVal = 0;
                                strHtmlAllClient = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + ">";
                                strHtmlAllClient += "<tr style=\"background-color: #DBEEF3;\">";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Scrip</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">B/F Qty</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Price</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Buy Qty</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Value</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Avg</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Sell Qty</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Value</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Avg</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">C/F Qty</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Price</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">MTM</td>";
                                strHtmlAllClient += "</tr>";
                                for (int j = 0; j < dtProduct.Rows.Count; j++)
                                {
                                    strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                                    strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\">" + dtProduct.Rows[j]["comPosition_ProductSeriesID"] + "</td>";
                                    if (dtProduct.Rows[j]["comPosition_BFPriceUnits"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["comPosition_BFPriceUnits"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["comPosition_openprice"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["comPosition_openprice"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["comPosition_buyPriceUnits"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["comPosition_buyPriceUnits"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["comPosition_BuyValue"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["comPosition_BuyValue"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["comPosition_BuyAverage"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["comPosition_BuyAverage"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["comPosition_sellPriceUnits"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["comPosition_sellPriceUnits"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["comPosition_SellValue"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["comPosition_SellValue"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["comPosition_SellAverage"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["comPosition_SellAverage"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["CFQty"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["CFQty"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["CFPrice"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["CFPrice"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["comPosition_MTMPL"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["comPosition_MTMPL"])) + "</td></tr>";
                                        TotalVal += Convert.ToDecimal(dtProduct.Rows[j]["comPosition_MTMPL"]);
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td></tr>";
                                    }
                                }
                                strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                                strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\"><b>Total :</b></td>";
                                strHtmlAllClient += "<td align=\"right\" colspan=\"11\" style=\"font-size:xx-small\"><b>" + oconverter.getFormattedvaluewithoriginalsign(TotalVal) + "</b></td></tr>";
                                strHtmlAllClient += "</table>";
                                newRow1[0] = "a";
                                newRow1[1] = "";
                                newRow1[2] = "";
                                newRow1[3] = strHtmlAllClient;
                                newRow1[4] = "";
                                newRow1[5] = 0;
                                newRow1[6] = 0;
                                newRow1[7] = "";
                                newRow1[8] = dtCashBankBook.Rows[i]["accountsledger_transactiondate"];
                                newRow1[9] = "";
                                newRow1[10] = "";
                                newRow1[11] = "";
                                newRow1[12] = "";
                                newRow1[13] = "";
                                newRow1[14] = "";
                                newRow1[15] = "";
                                newRow1[16] = "";
                                dtCashBankBook_New.Rows.Add(newRow1);
                                //displayALLCLIENT.InnerHtml = strHtmlAllClient;
                            }
                        }
                    }
                    #endregion
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
                ViewState["dtCashBankBook"] = dtCashBankBook;
                grdCashBankBook.DataSource = dtCashBankBook;
                grdCashBankBook.DataBind();

                DisplayOblg.Visible = false;
                grdCashBankBook.Visible = true;



                if (Session["userlastsegment"].ToString() == "7" || Session["userlastsegment"].ToString() == "18")
                    grdCashBankBook.Columns[5].Visible = true;
                else
                    grdCashBankBook.Columns[5].Visible = false;
                if (Session["userlastsegment"].ToString() == "7" || Session["userlastsegment"].ToString() == "8" || Session["userlastsegment"].ToString() == "18")
                    grdCashBankBook.Columns[6].Visible = true;
                else
                    grdCashBankBook.Columns[6].Visible = false;
                if (radConsolidated.Checked == true)
                {
                    if (radDateWise.Checked == true)
                    {
                        grdCashBankBook.Columns[2].Visible = true;
                    }
                    else
                    {
                        grdCashBankBook.Columns[2].Visible = false;
                    }

                }
                else
                {
                    grdCashBankBook.Columns[2].Visible = true;
                }
                if (Session["userlastsegment"].ToString() == "8")
                    grdCashBankBook.Columns[7].Visible = false;
                else
                    grdCashBankBook.Columns[7].Visible = true;
                grdCashBankBook.FooterRow.Cells[3].Text = "Closing Balance";

                if (ClosingBlncPstv == "B")
                    grdCashBankBook.FooterRow.Cells[11].Text = (-1 * openingBal).ToString("c", currencyFormat);
                else
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
                //StringBuilder sb = new StringBuilder();
                //StringWriter sw = new StringWriter(sb);
                //HtmlTextWriter hw = new HtmlTextWriter(sw);
                //grdCashBankBook.RenderControl(hw);
                //return sb.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct", "ShowHide('" + SpanText1 + "')", true);
                if (cmbclientsPager.SelectedItem == null)
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JShide", "DisabledDrp('a');", true);
                else
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JShide", "DisabledDrp('b');", true);


                //}
                //catch(Exception ex)
                //{
                //    ViewState["Check"] = "b";
                //}
                DisplayOblg.Visible = false;
                grdCashBankBook.Visible = true;

            }
            else
            {
                grdCashBankBook.Visible = false;

            }
            //New Addition for Value Date Show Or Not
            if (!ChkShowValueDate.Checked)
                grdCashBankBook.Columns[1].Visible = false;
            else
                grdCashBankBook.Columns[1].Visible = true;
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

                        if (ClosingBlncPstv == "B")
                        {
                            e.Row.Cells[11].Text = (-1 * openingBal).ToString("c", currencyFormat);//.Substring(1, openingBal.ToString("c", currencyFormat).Length - 1);
                            e.Row.Cells[11].ForeColor = System.Drawing.Color.Black;
                        }
                        else
                        {
                            e.Row.Cells[11].Text = openingBal.ToString("c", currencyFormat);//.Substring(1, openingBal.ToString("c", currencyFormat).Length - 1);
                            e.Row.Cells[11].ForeColor = System.Drawing.Color.Red;
                        }

                    }
                    else
                    {
                        if (ClosingBlncPstv == "B")
                        {
                            e.Row.Cells[11].Text = (-1 * openingBal).ToString("c", currencyFormat);//.Substring(1, openingBal.ToString("c", currencyFormat).Length - 1);
                            e.Row.Cells[11].ForeColor = System.Drawing.Color.Red;
                        }
                        else
                        {
                            e.Row.Cells[11].Text = openingBal.ToString("c", currencyFormat);
                        }
                        //e.Row.Cells[7].Text = "Dr";
                    }
                }
                else
                {
                    openingBal = decimal.Parse(lcVar2) - decimal.Parse(lcVar1) + openingBal;
                    if (openingBal < 0)
                    {

                        if (ClosingBlncPstv == "B")
                        {
                            e.Row.Cells[11].Text = (-1 * openingBal).ToString("c", currencyFormat);//.Substring(1, openingBal.ToString("c", currencyFormat).Length - 1);
                            e.Row.Cells[11].ForeColor = System.Drawing.Color.Black;
                        }
                        else
                        {
                            e.Row.Cells[11].Text = openingBal.ToString("c", currencyFormat);//.Substring(1, openingBal.ToString("c", currencyFormat).Length - 1);
                            e.Row.Cells[11].ForeColor = System.Drawing.Color.Red;
                        }
                    }
                    else
                    {
                        if (ClosingBlncPstv == "B")
                        {
                            e.Row.Cells[11].Text = (-1 * openingBal).ToString("c", currencyFormat);
                            e.Row.Cells[11].ForeColor = System.Drawing.Color.Red;
                        }
                        else
                        {
                            e.Row.Cells[11].Text = openingBal.ToString("c", currencyFormat);
                        }
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
                    //e.Row.Cells[9].ForeColor = System.Drawing.Color.Red;
                    //e.Row.Cells[10].ForeColor = System.Drawing.Color.Red;
                    //e.Row.Cells[11].ForeColor = System.Drawing.Color.Red;
                }
                Label TradeDate = (Label)e.Row.FindControl("lblTradeDate");
                Label ReferenceID = (Label)e.Row.FindControl("lblVoucherNo");
                Label MainID = (Label)e.Row.FindControl("lblMainID");
                Label SubID = (Label)e.Row.FindControl("lblSubID");
                Label CompID = (Label)e.Row.FindControl("lblCompID");
                Label SegID = (Label)e.Row.FindControl("lblSegID");
                Label CashType = (Label)e.Row.FindControl("lblCashType");
                if (Session["EntryProfileType"].ToString() == "F")
                {
                    if (CashType.Text == "Cash_Bank")
                    {
                        if (Session["LCKBNK"] != null)
                        {
                            if (Convert.ToDateTime(TradeDate.Text) >= Convert.ToDateTime(Session["LCKBNK"].ToString()))
                            {
                                ((Label)e.Row.FindControl("lblVoucherNo")).Attributes.Add("onclick", "javascript:updateCashBankDetail('" + TradeDate.Text + "','" + ReferenceID.Text + "','" + MainID.Text + "','" + SubID.Text + "','" + CompID.Text + "','" + SegID.Text + "');");
                                e.Row.Cells[2].ToolTip = "Click to View Detail!";
                                e.Row.Cells[2].Style.Add("cursor", "hand");
                            }
                            else
                            {
                                e.Row.Cells[2].ToolTip = "Voucher Locked !";
                            }
                        }
                        else
                        {
                            ((Label)e.Row.FindControl("lblVoucherNo")).Attributes.Add("onclick", "javascript:updateCashBankDetail('" + TradeDate.Text + "','" + ReferenceID.Text + "','" + MainID.Text + "','" + SubID.Text + "','" + CompID.Text + "','" + SegID.Text + "');");
                            e.Row.Cells[2].ToolTip = "Click to View Detail!";
                            e.Row.Cells[2].Style.Add("cursor", "hand");
                        }
                    }
                    else if (CashType.Text.Trim() == "Journal")
                    {
                        if (ReferenceID.Text.Length > 2)
                        {
                            string gridForVoucherID = ReferenceID.Text.Substring(0, 2);
                            string gridForVoucher = ReferenceID.Text.Substring(0, 1);
                            Label lblDescrip = (Label)e.Row.FindControl("lblDescrip");
                            Label lblSett = (Label)e.Row.FindControl("lblSettNo");
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


                                string SetN = "";
                                string SetType = "";
                                string[] SettlementNo = lblSett.Text.Split(' ');
                                if (SettlementNo.Length > 1)
                                {
                                    SetN = SettlementNo[0].ToString();
                                    SetType = SettlementNo[1].ToString();
                                }

                                SegMentName = ViewState["SegMentName"].ToString();
                                //((Label)e.Row.FindControl("lblVoucherNo")).Attributes.Add("onclick", "javascript:ShowObligationBreakUp('" + Bill + "','" + gridForVoucherID + "','" + SegMentName + "','" + lblTradeDate.Text + "');");
                                ((Label)e.Row.FindControl("lblVoucherNo")).Attributes.Add("onclick", "javascript:ShowObligationBreakUp('" + TradeDate.Text + "','" + SetN.ToString() + "','" + SetType.ToString() + "','" + MainID.Text + "','" + SubID.Text + "','" + CompID.Text + "','" + SegID.Text + "','" + SegMentName + "','" + gridForVoucherID + "');");
                                e.Row.Cells[2].Style.Add("cursor", "hand");
                                e.Row.Cells[2].ToolTip = "Click to View Detail!";
                            }
                            else if (gridForVoucherID == "YF" || gridForVoucherID == "YG" || gridForVoucherID == "YR")
                            {
                                ((Label)e.Row.FindControl("lblVoucherNo")).Attributes.Add("onclick", "javascript:updateJournalDetail('" + TradeDate.Text + "','" + ReferenceID.Text + "','" + MainID.Text + "','" + SubID.Text + "','" + CompID.Text + "','" + SegID.Text + "');");
                                e.Row.Cells[2].Style.Add("cursor", "hand");
                                e.Row.Cells[2].ToolTip = "Click to View Detail!";

                            }
                            else if (gridForVoucher != "U" && gridForVoucher != "V" && gridForVoucher != "X" && gridForVoucher != "Y" && gridForVoucher != "Z")
                            {
                                ((Label)e.Row.FindControl("lblVoucherNo")).Attributes.Add("onclick", "javascript:updateJournalDetail('" + TradeDate.Text + "','" + ReferenceID.Text + "','" + MainID.Text + "','" + SubID.Text + "','" + CompID.Text + "','" + SegID.Text + "');");
                                e.Row.Cells[2].Style.Add("cursor", "hand");
                                e.Row.Cells[2].ToolTip = "Click to View Detail!";
                            }
                        }
                        else
                        {
                            Label lblDescrip = (Label)e.Row.FindControl("lblDescrip");
                            Label lblSettNo = (Label)e.Row.FindControl("lblSettNo");
                            string Descip = lblDescrip.Text;
                            if (Descip.Contains("Consolidated Entries For"))
                            {
                                DataTable dtSeg = (DataTable)ViewState["dtSeg"];
                                if (dtSeg.Rows[0][0].ToString().EndsWith("CM"))
                                {
                                    if (Request.QueryString["MainID"] != null)
                                        ((Label)e.Row.FindControl("lblDescrip")).Attributes.Add("onclick", "javascript:ShowBillPrintingCMSubTrial('" + TradeDate.Text + "','" + lblSettNo.Text + "','" + SubID.Text + "','" + SegID.Text + "','" + CompID.Text + "');");
                                    else
                                        ((Label)e.Row.FindControl("lblDescrip")).Attributes.Add("onclick", "javascript:ShowBillPrintingCM('" + TradeDate.Text + "','" + lblSettNo.Text + "','" + SubID.Text + "','" + SegID.Text + "','" + CompID.Text + "');");
                                    e.Row.Cells[2].Style.Add("cursor", "hand");
                                    e.Row.Cells[2].ToolTip = "Click to View Detail!";
                                }
                                else if (dtSeg.Rows[0][0].ToString().EndsWith("FO"))
                                {
                                    if (!ReportType.Contains("ConsolidateExp"))
                                    {
                                        if (Request.QueryString["MainID"] != null)
                                            ((Label)e.Row.FindControl("lblDescrip")).Attributes.Add("onclick", "javascript:ShowBillPrintingFOSubTrial('" + TradeDate.Text + "','" + SubID.Text + "','" + SegID.Text + "','" + CompID.Text + "');");
                                        else
                                            ((Label)e.Row.FindControl("lblDescrip")).Attributes.Add("onclick", "javascript:ShowBillPrintingFO('" + TradeDate.Text + "','" + SubID.Text + "','" + SegID.Text + "','" + CompID.Text + "');");
                                        e.Row.Cells[2].Style.Add("cursor", "hand");
                                        e.Row.Cells[2].ToolTip = "Click to View Detail!";
                                    }
                                }
                            }
                        }
                    }
                }
                Label TrDate = (Label)e.Row.FindControl("lblTrDate");
                string row1Text = TrDate.Text;
                if (row1Text == "a")
                {
                    if (SegMentName == "NSE - CM" || SegMentName == "BSE - CM" || SegMentName == "NSE - CM,BSE - CM")
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
            dtCashBankBook = (DataTable)ViewState["dtCashBankBook"];
            string rowID = String.Empty;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                rowID = "row" + e.Row.RowIndex;
                e.Row.Attributes.Add("id", "row" + e.Row.RowIndex);
                e.Row.Attributes.Add("onclick", "ChangeRowColor(" + "'" + rowID + "','" + dtCashBankBook.Rows.Count + "'" + ")");
            }

        }

        protected void NavigationLink_Click(Object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "First":
                    pageindex = 0;
                    break;
                case "Next":
                    pageindex = int.Parse(CurrentPage.Value) + 1;
                    break;
                case "Prev":
                    pageindex = int.Parse(CurrentPage.Value) - 1;
                    break;
                case "Last":
                    pageindex = int.Parse(TotalPages.Value);
                    break;
                default:
                    pageindex = int.Parse(e.CommandName.ToString());
                    break;
            }
            FillGrid();
            string SpanText1 = oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "  - " + oconverter.ArrangeDate2(dtTo.Value.ToString());
            ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct", "ShowHide('" + SpanText1 + "')", true);
        }

        protected void NavigationLinkC_Click(Object sender, CommandEventArgs e)
        {
            int curentIndex = cmbclientsPager.SelectedIndex;
            int totalNo = cmbclientsPager.Items.Count;
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
                    pageindex = int.Parse(TotalClient.Value);
                    break;
                default:
                    pageindex = int.Parse(e.CommandName.ToString());
                    break;
            }
            if (curentIndex >= totalNo)
            {
                curentIndex = totalNo - 1;
                //Page.ClientScript.RegisterStartupScript(GetType(), "hide", "<script language='javascript'>DisableC('N');</script>");
                ScriptManager.RegisterStartupScript(this, GetType(), "hide", "DisableC('N');", true);
            }
            else if (curentIndex <= 0)
            {
                curentIndex = 0;
                //Page.ClientScript.RegisterStartupScript(GetType(), "hide", "<script language='javascript'>DisableC('P');</script>");
                ScriptManager.RegisterStartupScript(this, GetType(), "hide", "DisableC('P');", true);
            }
            cmbclientsPager.SelectedIndex = curentIndex;
            //MainAcID = "'" + cmbclientsPager.SelectedItem.Value.ToString() + "'";
            //FillGrid();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct6", "HideOnOffLoading()", true);
            //FillGridForChanges();
            FillGrid();
            string SpanText1 = oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "  - " + oconverter.ArrangeDate2(dtTo.Value.ToString());
            ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct", "ShowHide('" + SpanText1 + "')", true);
        }

        public void FillGridForChanges()
        {
            Segment = ViewState["Segment"].ToString();
            MainAcIDforOp = ViewState["MainAcIDforOp"].ToString();
            SegMentName = ViewState["SegMentName"].ToString();
            pageSize = 25;
            decimal receipt = 0;
            decimal Payment = 0;
            DateTime TranDate = DateTime.Today;
            System.Globalization.NumberFormatInfo currencyFormat = new System.Globalization.CultureInfo("hi-IN").NumberFormat;
            currencyFormat.CurrencySymbol = "";
            currencyFormat.CurrencyNegativePattern = 2;
            dtCashBankBook = (DataTable)ViewState["dtCashBankBook"];
            dtLedgerView = (DataTable)ViewState["dtLedgerView"];
            DataView foundRow = new DataView(dtLedgerView);
            foundRow.RowFilter = "SubID='" + cmbclientsPager.SelectedItem.Value + "'  and UserID=" + Session["userid"].ToString() + "";
            dtCashBankBook = foundRow.Table.Clone();
            foreach (DataRowView dvr in foundRow)
            {
                dtCashBankBook.ImportRow(dvr.Row);
            }
            dtCashBankBook.AcceptChanges();
            DataTable OpenBalance = oDBEngine.OpeningBalanceJournal1(MainAcIDforOp, cmbclientsPager.SelectedItem.Value, Convert.ToDateTime(dtFrom.Value), Segment, Session["CompanyID"].ToString(), Convert.ToDateTime(dtTo.Value), Convert.ToInt32(Session["ActiveCurrency"].ToString().Split('~')[0]));
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
                dtCashBankBook_New.Rows.Add(newRow);
                TranDate = Convert.ToDateTime(dtCashBankBook.Rows[i]["accountsledger_transactiondate"].ToString());
                if (dtCashBankBook.Rows[i]["Accountsledger_AmountDr"].ToString() != "")
                    receipt += Convert.ToDecimal(dtCashBankBook.Rows[i]["Accountsledger_AmountDr"].ToString());
                if (dtCashBankBook.Rows[i]["Accountsledger_AmountCr"].ToString() != "")
                    Payment += Convert.ToDecimal(dtCashBankBook.Rows[i]["Accountsledger_AmountCr"].ToString());
                #region
                if (radBreakDetail.Checked == true)
                {
                    string[] Narration1;
                    string[] Bill1;
                    string Bill = "0";
                    string TranID = dtCashBankBook.Rows[i]["accountsledger_TransactionReferenceID"].ToString().Substring(0, 2);
                    string[] Narration = dtCashBankBook.Rows[i]["accountsledger_Narration"].ToString().Split('[');
                    if (Narration.Length > 1)
                    {
                        Narration1 = Narration[1].Split(':');
                        if (Narration1.Length > 1)
                        {
                            Bill1 = Narration1[1].Split(']');
                            Bill = Bill1[0];
                        }
                    }
                    if (SegMentName == "NSE - CM" || SegMentName == "BSE - CM" || SegMentName == "NSE - CM,BSE - CM")
                    {
                        if (TranID == "XO")
                        {
                            DataTable dtProduct = oDBEngine.GetDataTable("Trans_CMPosition", "(select ltrim(rtrim(isnull(Equity_TickerSymbol,'')))+(case when isnull(convert(varchar(12),Equity_EffectUntil,113),'')='' then '' else ' ['+convert(varchar(12),Equity_EffectUntil,113)+']' end)+' ['+ltrim(rtrim(isnull(Equity_Series,'')))+']'+ case when isnull(cast(Equity_StrikePrice as varchar),'')='' then '' else ' ['+cast(Equity_StrikePrice as varchar)+']' end from master_Equity where Equity_SeriesID=Trans_CMPosition.CMPosition_ProductSeriesID) as CMPosition_ProductSeriesID,case when CMPosition_SqrOffQty=0 then null else CMPosition_SqrOffQty end as CMPosition_SqrOffQty,case when CMPosition_SqrOffPL=0 then null else CMPosition_SqrOffPL end as CMPosition_SqrOffPL,case when CMPosition_DeliveryBuyQty=0 then null else CMPosition_DeliveryBuyQty end as CMPosition_DeliveryBuyQty,case when CMPosition_DeliveryBuyValue=0 then null else CMPosition_DeliveryBuyValue end as CMPosition_DeliveryBuyValue,case when CMPosition_DeliverySellQty=0 then null else CMPosition_DeliverySellQty end as CMPosition_DeliverySellQty,case when CMPosition_DeliverySellValue=0 then null else CMPosition_DeliverySellValue end as CMPosition_DeliverySellValue,case when CMPosition_NetObligation=0 then null else CMPosition_NetObligation end as CMPosition_NetObligation,CMPosition_SettlementNumber+CMPosition_SettlementType as SettnumType,case when (isnull(CMPosition_DeliveryBuyValue,0)/isnull((case when CMPosition_DeliveryBuyQty=0 then null else CMPosition_DeliveryBuyQty end),1))=0 then null else (isnull(CMPosition_DeliveryBuyValue,0)/isnull((case when CMPosition_DeliveryBuyQty=0 then null else CMPosition_DeliveryBuyQty end),1)) end as AvgBuyVal,case when (isnull(CMPosition_DeliverySellValue,0)/isnull((case when CMPosition_DeliverySellQty=0 then null else CMPosition_DeliverySellQty end),1))=0 then null else (isnull(CMPosition_DeliverySellValue,0)/isnull((case when CMPosition_DeliverySellQty=0 then null else CMPosition_DeliverySellQty end),1)) end as AvgSellVal", " CmPosition_BillNumber='" + Bill + "'");
                            DataRow newRow1 = dtCashBankBook_New.NewRow();
                            String strHtmlAllClient = String.Empty;
                            decimal TotalVal = 0;
                            strHtmlAllClient = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + ">";
                            strHtmlAllClient += "<tr style=\"background-color: #DBEEF3;\">";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Scrip</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Delv.Buy Qty</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Delv.Buy Value</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Avg.Buy Value</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Delv.Sell Qty</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Delv.Sell Value</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Avg.Sell Value</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Sqr Qty</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Sqr P/L</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Total</td>";
                            strHtmlAllClient += "</tr>";
                            for (int j = 0; j < dtProduct.Rows.Count; j++)
                            {
                                strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                                strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\">" + dtProduct.Rows[j]["CMPosition_ProductSeriesID"] + "</td>";
                                if (dtProduct.Rows[j]["CMPosition_DeliveryBuyQty"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["CMPosition_DeliveryBuyQty"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["CMPosition_DeliveryBuyValue"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["CMPosition_DeliveryBuyValue"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["AvgBuyVal"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["AvgBuyVal"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["CMPosition_DeliverySellQty"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["CMPosition_DeliverySellQty"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["CMPosition_DeliverySellValue"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["CMPosition_DeliverySellValue"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["AvgSellVal"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["AvgSellVal"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["CMPosition_SqrOffQty"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["CMPosition_SqrOffQty"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["CMPosition_SqrOffPL"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["CMPosition_SqrOffPL"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["CMPosition_NetObligation"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["CMPosition_NetObligation"])) + "</td></tr>";
                                    TotalVal += Convert.ToDecimal(dtProduct.Rows[j]["CMPosition_NetObligation"]);
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td></tr>";
                                }
                            }
                            strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                            strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\"><b>Total :</b></td>";
                            strHtmlAllClient += "<td align=\"right\" colspan=\"9\" style=\"font-size:xx-small\"><b>" + oconverter.getFormattedvaluewithoriginalsign(TotalVal) + "</b></td></tr>";
                            strHtmlAllClient += "</table>";
                            newRow1[0] = "a";
                            newRow1[1] = dtProduct.Rows[0]["SettnumType"].ToString();
                            newRow1[2] = "";
                            newRow1[3] = strHtmlAllClient;
                            newRow1[4] = "";
                            newRow1[5] = 0;
                            newRow1[6] = 0;
                            newRow1[7] = "";
                            newRow1[8] = dtCashBankBook.Rows[i]["accountsledger_transactiondate"];
                            newRow1[9] = "";
                            newRow1[10] = "";
                            newRow1[11] = "";
                            newRow1[12] = "";
                            newRow1[13] = "";
                            newRow1[14] = "";
                            newRow1[15] = "";
                            newRow1[16] = "";
                            dtCashBankBook_New.Rows.Add(newRow1);
                            //displayALLCLIENT.InnerHtml = strHtmlAllClient;
                        }
                    }
                    else if (SegMentName == "NSE-FO")
                    {
                        if (TranID == "XO")
                        {
                            DataTable dtProduct = oDBEngine.GetDataTable("trans_foposition", "(select ltrim(rtrim(isnull(Equity_TickerSymbol,'')))+(case when isnull(convert(varchar(12),Equity_EffectUntil,113),'')='' then '' else ' ['+convert(varchar(12),Equity_EffectUntil,113)+']' end)+' ['+ltrim(rtrim(isnull(Equity_Series,'')))+']'+ case when isnull(CONVERT(VARCHAR,CONVERT(MONEY,cast(Equity_StrikePrice as varchar)),1),'')='' then '' else ' ['+CONVERT(VARCHAR,CONVERT(MONEY,cast(Equity_StrikePrice as varchar)),1)+']' end from master_Equity where Equity_SeriesID=trans_foposition.FOPosition_ProductSeriesID) as FOPosition_ProductSeriesID,case when isnull(foposition_BFPriceUnits,0)=0 then null else foposition_BFPriceUnits end as foposition_BFPriceUnits,case when isnull(foposition_openprice,0)=0 then null else foposition_openprice end as foposition_openprice,case when isnull(foposition_buyLots,0)=0 then null else foposition_buyLots end as foposition_buyPriceUnits,case when isnull(foposition_BuyValue,0)=0 then foposition_BuyValue else foposition_BuyValue end as foposition_BuyValue,case when isnull(foposition_BuyAverage,0)=0 then null else foposition_BuyAverage end as foposition_BuyAverage,case when isnull(foposition_sellLots,0)=0 then null else foposition_sellLots end as foposition_sellPriceUnits,case when isnull(foposition_SellValue,0)=0 then null else foposition_SellValue end as foposition_SellValue,case when isnull(foposition_SellAverage,0)=0 then null else foposition_SellAverage end as foposition_SellAverage,case when isnull(foposition_PostExcAsnDlvLongPriceUnits,0)=0 then foposition_PostExcAsnDlvShortPriceUnits else (-1)*foposition_PostExcAsnDlvLongPriceUnits end as CFQty,case when foposition_PostExcAsnDlvLongValue=0 then foposition_PostExcAsnDlvShortValue else foposition_PostExcAsnDlvLongValue end as CFPrice,foposition_MTMPL,(select DailyStat_SettlementPrice from Trans_dailyStatistics where DailyStat_ProductSeriesID=trans_foposition.FOPosition_ProductSeriesID and DailyStat_ExchangeSegmentID=" + Session["ExchangeSegmentID"].ToString() + " and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,DailyStat_DateTime)) as datetime) =cast(DATEADD(dd, 0, DATEDIFF(dd, 0, '" + TranDate + "')) as datetime)) as SettPrice", " foposition_BillNumber='" + Bill + "' and foposition_productseriesid in(select equity_seriesid from master_equity where equity_foidentifier like 'FUT%' and equity_effectuntil<>'" + dtCashBankBook.Rows[i]["accountsledger_transactiondate"] + "')");
                            DataRow newRow1 = dtCashBankBook_New.NewRow();
                            String strHtmlAllClient = String.Empty;
                            decimal TotalVal = 0;
                            strHtmlAllClient = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + ">";
                            strHtmlAllClient += "<tr style=\"background-color: #DBEEF3;\">";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Scrip</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">B/F Qty</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Price</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Buy Qty</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Value</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Avg</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Sell Qty</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Value</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Avg</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">C/F Qty</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Price</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">MTM</td>";
                            strHtmlAllClient += "</tr>";
                            for (int j = 0; j < dtProduct.Rows.Count; j++)
                            {
                                strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                                strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\">" + dtProduct.Rows[j]["FOPosition_ProductSeriesID"] + "</td>";
                                if (dtProduct.Rows[j]["foposition_BFPriceUnits"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_BFPriceUnits"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["foposition_openprice"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_openprice"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["foposition_buyPriceUnits"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_buyPriceUnits"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["foposition_BuyValue"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_BuyValue"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["foposition_BuyAverage"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_BuyValue"]) / Convert.ToDecimal(dtProduct.Rows[j]["foposition_buyPriceUnits"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["foposition_sellPriceUnits"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_sellPriceUnits"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["foposition_SellValue"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_SellValue"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["foposition_SellAverage"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_SellValue"]) / Convert.ToDecimal(dtProduct.Rows[j]["foposition_sellPriceUnits"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["CFQty"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["CFQty"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["SettPrice"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["SettPrice"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["foposition_MTMPL"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_MTMPL"])) + "</td></tr>";
                                    TotalVal += Convert.ToDecimal(dtProduct.Rows[j]["foposition_MTMPL"]);
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td></tr>";
                                }
                            }
                            strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                            strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\"><b>Total :</b></td>";
                            strHtmlAllClient += "<td align=\"right\" colspan=\"11\" style=\"font-size:xx-small\"><b>" + oconverter.getFormattedvaluewithoriginalsign(TotalVal) + "</b></td></tr>";
                            strHtmlAllClient += "</table>";
                            newRow1[0] = "a";
                            newRow1[1] = "";
                            newRow1[2] = "";
                            newRow1[3] = strHtmlAllClient;
                            newRow1[4] = "";
                            newRow1[5] = 0;
                            newRow1[6] = 0;
                            newRow1[7] = "";
                            newRow1[8] = dtCashBankBook.Rows[i]["accountsledger_transactiondate"];
                            newRow1[9] = "";
                            newRow1[10] = "";
                            newRow1[11] = "";
                            newRow1[12] = "";
                            newRow1[13] = "";
                            newRow1[14] = "";
                            newRow1[15] = "";
                            newRow1[16] = "";
                            dtCashBankBook_New.Rows.Add(newRow1);
                            //displayALLCLIENT.InnerHtml = strHtmlAllClient;
                        }
                        if (TranID == "XP")
                        {
                            DataTable dtProduct = oDBEngine.GetDataTable("trans_foposition", "(select ltrim(rtrim(isnull(Equity_TickerSymbol,'')))+(case when isnull(convert(varchar(12),Equity_EffectUntil,113),'')='' then '' else ' ['+convert(varchar(12),Equity_EffectUntil,113)+']' end)+' ['+ltrim(rtrim(isnull(Equity_Series,'')))+']'+ case when isnull(CONVERT(VARCHAR,CONVERT(MONEY,cast(Equity_StrikePrice as varchar)),1),'')='' then '' else ' ['+CONVERT(VARCHAR,CONVERT(MONEY,cast(Equity_StrikePrice as varchar)),1)+']' end from master_Equity where Equity_SeriesID=trans_foposition.FOPosition_ProductSeriesID) as FOPosition_ProductSeriesID,case when isnull(foposition_BuyLots,0)=0 then null else foposition_BuyLots end as foposition_BuyLots,case when isnull(foposition_BuyValue,0)=0 then null else foposition_BuyValue end as foposition_BuyValue,case when isnull(foposition_BuyAverage,0)=0 then null else foposition_BuyAverage end as foposition_BuyAverage,case when isnull(foposition_SellLots,0)=0 then null else foposition_SellLots end as foposition_SellLots,case when isnull(foposition_SellValue,0)=0 then null else foposition_SellValue end as foposition_SellValue,case when isnull(foposition_SellAverage,0)=0 then null else foposition_SellAverage end as foposition_SellAverage,case when isnull(foposition_NetPremium,0)=0 then null else foposition_NetPremium end as foposition_NetPremium ", " foposition_BillNumber='" + Bill + "' and  foposition_NetPremium is not null");
                            DataRow newRow1 = dtCashBankBook_New.NewRow();
                            String strHtmlAllClient = String.Empty;
                            decimal TotalVal = 0;
                            strHtmlAllClient = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + ">";
                            strHtmlAllClient += "<tr style=\"background-color: #DBEEF3;\">";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Scrip</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Buy Qty</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Value</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Avg</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Sell Qty</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Value</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Avg</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Net Premium</td>";
                            strHtmlAllClient += "</tr>";
                            for (int j = 0; j < dtProduct.Rows.Count; j++)
                            {
                                strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                                strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\">" + dtProduct.Rows[j]["FOPosition_ProductSeriesID"] + "</td>";
                                if (dtProduct.Rows[j]["foposition_BuyLots"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_BuyLots"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["foposition_BuyValue"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_BuyValue"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["foposition_BuyAverage"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_BuyValue"]) / Convert.ToDecimal(dtProduct.Rows[j]["foposition_BuyLots"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["foposition_SellLots"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_SellLots"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["foposition_SellValue"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_SellValue"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["foposition_SellAverage"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_SellValue"]) / Convert.ToDecimal(dtProduct.Rows[j]["foposition_SellLots"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["foposition_NetPremium"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_NetPremium"])) + "</td></tr>";
                                    TotalVal += Convert.ToDecimal(dtProduct.Rows[j]["foposition_NetPremium"]);
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td></tr>";
                                }
                            }
                            strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                            strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\"><b>Total :</b></td>";
                            strHtmlAllClient += "<td align=\"right\" colspan=\"11\" style=\"font-size:xx-small\"><b>" + oconverter.getFormattedvaluewithoriginalsign(TotalVal) + "</b></td></tr>";
                            strHtmlAllClient += "</table>";
                            newRow1[0] = "a";
                            newRow1[1] = "";
                            newRow1[2] = "";
                            newRow1[3] = strHtmlAllClient;
                            newRow1[4] = "";
                            newRow1[5] = 0;
                            newRow1[6] = 0;
                            newRow1[7] = "";
                            newRow1[8] = dtCashBankBook.Rows[i]["accountsledger_transactiondate"];
                            newRow1[9] = "";
                            newRow1[10] = "";
                            newRow1[11] = "";
                            newRow1[12] = "";
                            newRow1[13] = "";
                            newRow1[14] = "";
                            newRow1[15] = "";
                            newRow1[16] = "";
                            dtCashBankBook_New.Rows.Add(newRow1);
                            //displayALLCLIENT.InnerHtml = strHtmlAllClient;
                        }
                        if (TranID == "XZ")
                        {
                            DataTable dtProduct = oDBEngine.GetDataTable("trans_foposition", "(select ltrim(rtrim(isnull(Equity_TickerSymbol,'')))+(case when isnull(convert(varchar(12),Equity_EffectUntil,113),'')='' then '' else ' ['+convert(varchar(12),Equity_EffectUntil,113)+']' end)+' ['+ltrim(rtrim(isnull(Equity_Series,'')))+']'+ case when isnull(CONVERT(VARCHAR,CONVERT(MONEY,cast(Equity_StrikePrice as varchar)),1),'')='' then '' else ' ['+CONVERT(VARCHAR,CONVERT(MONEY,cast(Equity_StrikePrice as varchar)),1)+']' end from master_Equity where Equity_SeriesID=trans_foposition.FOPosition_ProductSeriesID) as FOPosition_ProductSeriesID,case when foposition_ExcAsnDlvLots<0 then 'Exercise' else 'Assigned' end as SettType,case when isnull(foposition_ExcAsnDlvLots,0)=0 then null else abs(foposition_ExcAsnDlvLots) end as foposition_ExcAsnDlvLots,case when isnull(foposition_SettlementPrice,0)=0 then null else abs(foposition_SettlementPrice) end as foposition_SettlementPrice,case when isnull(foposition_ExcAsnDlvMarkedValue,0)=0 then null else foposition_ExcAsnDlvMarkedValue end as foposition_ExcAsnDlvMarkedValue ", " foposition_BillNumber='" + Bill + "' and  foposition_ExcAsnDlvMarkedValue is not null");
                            DataRow newRow1 = dtCashBankBook_New.NewRow();
                            String strHtmlAllClient = String.Empty;
                            decimal TotalVal = 0;
                            strHtmlAllClient = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + ">";
                            strHtmlAllClient += "<tr style=\"background-color: #DBEEF3;\">";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Scrip</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Sett.Type</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Qty</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Set.Price</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Net Amount</td>";
                            strHtmlAllClient += "</tr>";
                            for (int j = 0; j < dtProduct.Rows.Count; j++)
                            {
                                strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                                strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\">" + dtProduct.Rows[j]["FOPosition_ProductSeriesID"] + "</td>";
                                strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\">" + dtProduct.Rows[j]["SettType"] + "</td>";
                                if (dtProduct.Rows[j]["foposition_ExcAsnDlvLots"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_ExcAsnDlvLots"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["foposition_SettlementPrice"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_SettlementPrice"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["foposition_ExcAsnDlvMarkedValue"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_ExcAsnDlvMarkedValue"])) + "</td></tr>";
                                    TotalVal += Convert.ToDecimal(dtProduct.Rows[j]["foposition_ExcAsnDlvMarkedValue"]);
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td></tr>";
                                }
                            }
                            strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                            strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\"><b>Total :</b></td>";
                            strHtmlAllClient += "<td align=\"right\" colspan=\"11\" style=\"font-size:xx-small\"><b>" + oconverter.getFormattedvaluewithoriginalsign(TotalVal) + "</b></td></tr>";
                            strHtmlAllClient += "</table>";
                            newRow1[0] = "a";
                            newRow1[1] = "";
                            newRow1[2] = "";
                            newRow1[3] = strHtmlAllClient;
                            newRow1[4] = "";
                            newRow1[5] = 0;
                            newRow1[6] = 0;
                            newRow1[7] = "";
                            newRow1[8] = dtCashBankBook.Rows[i]["accountsledger_transactiondate"];
                            newRow1[9] = "";
                            newRow1[10] = "";
                            newRow1[11] = "";
                            newRow1[12] = "";
                            newRow1[13] = "";
                            newRow1[14] = "";
                            newRow1[15] = "";
                            newRow1[16] = "";
                            dtCashBankBook_New.Rows.Add(newRow1);
                            //displayALLCLIENT.InnerHtml = strHtmlAllClient;
                        }
                        if (TranID == "XX")
                        {
                            DataTable dtProduct = oDBEngine.GetDataTable("trans_foposition", "(select ltrim(rtrim(isnull(Equity_TickerSymbol,'')))+(case when isnull(convert(varchar(12),Equity_EffectUntil,113),'')='' then '' else ' ['+convert(varchar(12),Equity_EffectUntil,113)+']' end)+' ['+ltrim(rtrim(isnull(Equity_Series,'')))+']'+ case when isnull(CONVERT(VARCHAR,CONVERT(MONEY,cast(Equity_StrikePrice as varchar)),1),'')='' then '' else ' ['+CONVERT(VARCHAR,CONVERT(MONEY,cast(Equity_StrikePrice as varchar)),1)+']' end from master_Equity where Equity_SeriesID=trans_foposition.FOPosition_ProductSeriesID) as FOPosition_ProductSeriesID,case when isnull(foposition_BFPriceUnits,0)=0 then null else foposition_BFPriceUnits end as foposition_BFPriceUnits,case when isnull(foposition_openprice,0)=0 then null else foposition_openprice end as foposition_openprice,case when isnull(foposition_buyLots,0)=0 then null else foposition_buyLots end as foposition_buyPriceUnits,case when isnull(foposition_BuyValue,0)=0 then foposition_BuyValue else foposition_BuyValue end as foposition_BuyValue,case when isnull(foposition_BuyAverage,0)=0 then null else foposition_BuyAverage end as foposition_BuyAverage,case when isnull(foposition_sellLots,0)=0 then null else foposition_sellLots end as foposition_sellPriceUnits,case when isnull(foposition_SellValue,0)=0 then null else foposition_SellValue end as foposition_SellValue,case when isnull(foposition_SellAverage,0)=0 then null else foposition_SellAverage end as foposition_SellAverage,case when isnull(foposition_PostExcAsnDlvLongPriceUnits,0)=0 then foposition_PostExcAsnDlvShortPriceUnits else (-1)*foposition_PostExcAsnDlvLongPriceUnits end as CFQty,case when foposition_PostExcAsnDlvLongValue=0 then foposition_PostExcAsnDlvShortValue else foposition_PostExcAsnDlvLongValue end as CFPrice,foposition_FutureFinalSettlement,(select DailyStat_SettlementPrice from Trans_dailyStatistics where DailyStat_ProductSeriesID=trans_foposition.FOPosition_ProductSeriesID and DailyStat_ExchangeSegmentID=" + Session["ExchangeSegmentID"].ToString() + " and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,DailyStat_DateTime)) as datetime) =cast(DATEADD(dd, 0, DATEDIFF(dd, 0, '" + TranDate + "')) as datetime)) as SettPrice", " foposition_BillNumber='" + Bill + "' and foposition_productseriesid in(select equity_seriesid from master_equity where equity_foidentifier like 'FUT%' and equity_effectuntil='" + dtCashBankBook.Rows[i]["accountsledger_transactiondate"] + "')");
                            DataRow newRow1 = dtCashBankBook_New.NewRow();
                            String strHtmlAllClient = String.Empty;
                            decimal TotalVal = 0;
                            strHtmlAllClient = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + ">";
                            strHtmlAllClient += "<tr style=\"background-color: #DBEEF3;\">";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Scrip</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">B/F Qty</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Price</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Buy Qty</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Value</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Avg</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Sell Qty</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Value</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Avg</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Exp Qty</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Price</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">MTM</td>";
                            strHtmlAllClient += "</tr>";
                            for (int j = 0; j < dtProduct.Rows.Count; j++)
                            {
                                strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                                strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\">" + dtProduct.Rows[j]["FOPosition_ProductSeriesID"] + "</td>";
                                if (dtProduct.Rows[j]["foposition_BFPriceUnits"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_BFPriceUnits"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["foposition_openprice"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_openprice"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["foposition_buyPriceUnits"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_buyPriceUnits"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["foposition_BuyValue"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_BuyValue"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["foposition_BuyAverage"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_BuyValue"]) / Convert.ToDecimal(dtProduct.Rows[j]["foposition_buyPriceUnits"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["foposition_sellPriceUnits"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_sellPriceUnits"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["foposition_SellValue"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_SellValue"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["foposition_SellAverage"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_SellValue"]) / Convert.ToDecimal(dtProduct.Rows[j]["foposition_sellPriceUnits"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["CFQty"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["CFQty"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["SettPrice"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["SettPrice"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["foposition_FutureFinalSettlement"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_FutureFinalSettlement"])) + "</td></tr>";
                                    TotalVal += Convert.ToDecimal(dtProduct.Rows[j]["foposition_FutureFinalSettlement"]);
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td></tr>";
                                }
                            }
                            strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                            strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\"><b>Total :</b></td>";
                            strHtmlAllClient += "<td align=\"right\" colspan=\"11\" style=\"font-size:xx-small\"><b>" + oconverter.getFormattedvaluewithoriginalsign(TotalVal) + "</b></td></tr>";
                            strHtmlAllClient += "</table>";
                            newRow1[0] = "a";
                            newRow1[1] = "";
                            newRow1[2] = "";
                            newRow1[3] = strHtmlAllClient;
                            newRow1[4] = "";
                            newRow1[5] = 0;
                            newRow1[6] = 0;
                            newRow1[7] = "";
                            newRow1[8] = dtCashBankBook.Rows[i]["accountsledger_transactiondate"];
                            newRow1[9] = "";
                            newRow1[10] = "";
                            newRow1[11] = "";
                            newRow1[12] = "";
                            newRow1[13] = "";
                            newRow1[14] = "";
                            newRow1[15] = "";
                            newRow1[16] = "";
                            dtCashBankBook_New.Rows.Add(newRow1);
                            //displayALLCLIENT.InnerHtml = strHtmlAllClient;
                        }
                    }
                    else if (SegMentName == "ICEX-COMM" || SegMentName == "MCX-COMM")
                    {
                        if (TranID == "XC")
                        {
                            DataTable dtProduct = oDBEngine.GetDataTable("Trans_comPosition", "(select ltrim(rtrim(isnull(Commodity_TickerSymbol,'')))+' ['+convert(varchar(12),Commodity_ExpiryDate,113)+']' from master_commodity where commodity_ProductSeriesID=trans_composition.comPosition_ProductSeriesID) as comPosition_ProductSeriesID,case when isnull(comPosition_BFPriceUnits,0)=0 then null else comPosition_BFPriceUnits end as comPosition_BFPriceUnits,case when isnull(comPosition_openprice,0)=0 then null else comPosition_openprice end as comPosition_openprice,case when isnull(comPosition_buyPriceUnits,0)=0 then null else comPosition_buyPriceUnits end as comPosition_buyPriceUnits,case when isnull(comPosition_BuyValue,0)=0 then comPosition_BuyValue else comPosition_BuyValue end as comPosition_BuyValue,case when isnull(comPosition_BuyAverage,0)=0 then null else comPosition_BuyAverage end as comPosition_BuyAverage,case when isnull(comPosition_sellPriceUnits,0)=0 then null else comPosition_sellPriceUnits end as comPosition_sellPriceUnits,case when isnull(comPosition_SellValue,0)=0 then null else comPosition_SellValue end as comPosition_SellValue,case when isnull(comPosition_SellAverage,0)=0 then null else comPosition_SellAverage end as comPosition_SellAverage,case when comPosition_PostExcAsnDlvLongPriceUnits=0 then comPosition_PostExcAsnDlvShortPriceUnits else comPosition_PostExcAsnDlvLongPriceUnits end as CFQty,case when comPosition_PostExcAsnDlvLongValue=0 then case when isnull(comPosition_PostExcAsnDlvShortValue,0)/abs(isnull(comPosition_PostExcAsnDlvShortPriceUnits,1))=0 then null else isnull(comPosition_PostExcAsnDlvShortValue,0)/abs(isnull(comPosition_PostExcAsnDlvShortPriceUnits,1)) end else case when isnull(comPosition_PostExcAsnDlvLongValue,0)/abs(isnull(comPosition_PostExcAsnDlvLongPriceUnits,1))=0 then null else isnull(comPosition_PostExcAsnDlvLongValue,0)/abs(isnull(comPosition_PostExcAsnDlvLongPriceUnits,1)) end end as CFPrice,comPosition_MTMPL", " comPosition_BillNumber='" + Bill + "'");
                            DataRow newRow1 = dtCashBankBook_New.NewRow();
                            String strHtmlAllClient = String.Empty;
                            decimal TotalVal = 0;
                            strHtmlAllClient = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + ">";
                            strHtmlAllClient += "<tr style=\"background-color: #DBEEF3;\">";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Scrip</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">B/F Qty</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Price</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Buy Qty</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Value</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Avg</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Sell Qty</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Value</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Avg</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">C/F Qty</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Price</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">MTM</td>";
                            strHtmlAllClient += "</tr>";
                            for (int j = 0; j < dtProduct.Rows.Count; j++)
                            {
                                strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                                strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\">" + dtProduct.Rows[j]["comPosition_ProductSeriesID"] + "</td>";
                                if (dtProduct.Rows[j]["comPosition_BFPriceUnits"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["comPosition_BFPriceUnits"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["comPosition_openprice"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["comPosition_openprice"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["comPosition_buyPriceUnits"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["comPosition_buyPriceUnits"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["comPosition_BuyValue"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["comPosition_BuyValue"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["comPosition_BuyAverage"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["comPosition_BuyAverage"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["comPosition_sellPriceUnits"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["comPosition_sellPriceUnits"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["comPosition_SellValue"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["comPosition_SellValue"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["comPosition_SellAverage"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["comPosition_SellAverage"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["CFQty"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["CFQty"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["CFPrice"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["CFPrice"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["comPosition_MTMPL"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["comPosition_MTMPL"])) + "</td></tr>";
                                    TotalVal += Convert.ToDecimal(dtProduct.Rows[j]["comPosition_MTMPL"]);
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td></tr>";
                                }
                            }
                            strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                            strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\"><b>Total :</b></td>";
                            strHtmlAllClient += "<td align=\"right\" colspan=\"11\" style=\"font-size:xx-small\"><b>" + oconverter.getFormattedvaluewithoriginalsign(TotalVal) + "</b></td></tr>";
                            strHtmlAllClient += "</table>";
                            newRow1[0] = "a";
                            newRow1[1] = "";
                            newRow1[2] = "";
                            newRow1[3] = strHtmlAllClient;
                            newRow1[4] = "";
                            newRow1[5] = 0;
                            newRow1[6] = 0;
                            newRow1[7] = "";
                            newRow1[8] = dtCashBankBook.Rows[i]["accountsledger_transactiondate"];
                            newRow1[9] = "";
                            newRow1[10] = "";
                            newRow1[11] = "";
                            newRow1[12] = "";
                            newRow1[13] = "";
                            newRow1[14] = "";
                            newRow1[15] = "";
                            newRow1[16] = "";
                            dtCashBankBook_New.Rows.Add(newRow1);
                            //displayALLCLIENT.InnerHtml = strHtmlAllClient;
                        }
                    }
                }
                #endregion
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
            ViewState["CashBankEmail"] = dtCashBankBook;
            grdCashBankBook.DataSource = dtCashBankBook;
            grdCashBankBook.DataBind();
            if (Session["userlastsegment"].ToString() == "7" || Session["userlastsegment"].ToString() == "18")
                grdCashBankBook.Columns[5].Visible = true;
            else
                grdCashBankBook.Columns[5].Visible = false;
            if (Session["userlastsegment"].ToString() == "7" || Session["userlastsegment"].ToString() == "8" || Session["userlastsegment"].ToString() == "18")
                grdCashBankBook.Columns[6].Visible = true;
            else
                grdCashBankBook.Columns[6].Visible = false;
            if (radConsolidated.Checked == true)
            {
                grdCashBankBook.Columns[2].Visible = false;
            }
            else
            {
                grdCashBankBook.Columns[2].Visible = true;
            }
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
            grdCashBankBook.FooterRow.Cells[9].HorizontalAlign = HorizontalAlign.Right;
            grdCashBankBook.FooterRow.Cells[10].HorizontalAlign = HorizontalAlign.Right;
            grdCashBankBook.FooterRow.Cells[11].HorizontalAlign = HorizontalAlign.Right;
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
            dtCashBankBook = (DataTable)ViewState["dtCashBankBook"];
            dtLedgerView = (DataTable)ViewState["dtLedgerView"];
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
            DataTable OpenBalance = oDBEngine.OpeningBalanceJournal1(MainAcIDforOp, cmbclientsPager.SelectedItem.Value, Convert.ToDateTime(dtFrom.Value), Segment, Session["CompanyID"].ToString(), Convert.ToDateTime(dtTo.Value), Convert.ToInt32(Session["ActiveCurrency"].ToString().Split('~')[0]));
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

        #endregion

        protected void ButtonUpdate_Click(object sender, EventArgs e)
        {
            // FillDropDown();
        }

        protected void cmbclientsPager_SelectedIndexChanged(object sender, EventArgs e)
        {
            // MainAcID = "'" + cmbclientsPager.SelectedItem.Value.ToString() + "'";
            DrpChange = "Y";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct5", "HideOnOffLoading()", true);
            //FillGridForChanges();
            FillGrid();
            string SpanText1 = oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "  - " + oconverter.ArrangeDate2(dtTo.Value.ToString());
            ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct", "ShowHide('" + SpanText1 + "')", true);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            dtCashBankBook = (DataTable)ViewState["dtCashBankBook"];
            dtLedgerView = (DataTable)ViewState["dtLedgerView"];
            decimal receipt = 0;
            decimal Payment = 0;
            string whereclause = null;
            string trDate = null;
            MainAcIDforOp = ViewState["MainAcIDforOp"].ToString();
            Segment = ViewState["Segment"].ToString();
            DateTime TranDate = DateTime.Today;
            if (dtSearchDate.Value == null)
            {
                trDate = "1/1/0001 12:00:00 AM";
            }
            if (txtVouno.Text != "Voucher Number" && trDate != "1/1/0001 12:00:00 AM")
            {
                whereclause = " accountsledger_TransactionReferenceID like '" + txtVouno.Text + "%' and accountsledger_transactiondate='" + dtSearchDate.Value + "' and UserID=" + Session["userid"].ToString() + "";
            }
            else if (trDate != "1/1/0001 12:00:00 AM" && txtVouno.Text == "Voucher Number")
            {
                whereclause = " accountsledger_transactiondate='" + dtSearchDate.Value + "'  and UserID=" + Session["userid"].ToString() + "";
            }
            else if (trDate == "1/1/0001 12:00:00 AM" && txtVouno.Text != "Voucher Number")
            {
                whereclause = " accountsledger_TransactionReferenceID like '" + txtVouno.Text + "%'  and UserID=" + Session["userid"].ToString() + "";
            }
            pageSize = 25;
            grdCashBankBook.PageSize = pageSize;
            System.Globalization.NumberFormatInfo currencyFormat = new System.Globalization.CultureInfo("hi-IN").NumberFormat;
            currencyFormat.CurrencySymbol = "";
            currencyFormat.CurrencyNegativePattern = 2;

            DataView foundRow = new DataView(dtLedgerView);
            foundRow.RowFilter = whereclause;
            dtCashBankBook = foundRow.Table.Clone();
            foreach (DataRowView dvr in foundRow)
            {
                dtCashBankBook.ImportRow(dvr.Row);
            }
            dtCashBankBook.AcceptChanges();
            DataTable OpenBalance = oDBEngine.OpeningBalanceJournal1(MainAcIDforOp, cmbclientsPager.SelectedItem.Value, Convert.ToDateTime(dtFrom.Value), Segment, Session["CompanyID"].ToString(), Convert.ToDateTime(dtTo.Value), Convert.ToInt32(Session["ActiveCurrency"].ToString().Split('~')[0]));
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
                //newRow[7] = Convert.ToDecimal(OpenBalance.Rows[0][1].ToString());
            }
            else
            {
                decimal newpay = Convert.ToDecimal(OpenBalance.Rows[0][1].ToString());
                if (newpay != 0)
                    newRow[6] = newpay;
                receipt += Convert.ToDecimal(OpenBalance.Rows[0][1].ToString());
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
                #region
                if (radBreakDetail.Checked == true)
                {
                    string[] Narration1;
                    string[] Bill1;
                    string Bill = "0";
                    string TranID = dtCashBankBook.Rows[i]["accountsledger_TransactionReferenceID"].ToString().Substring(0, 2);
                    string[] Narration = dtCashBankBook.Rows[i]["accountsledger_Narration"].ToString().Split('[');
                    if (Narration.Length > 1)
                    {
                        Narration1 = Narration[1].Split(':');
                        if (Narration1.Length > 1)
                        {
                            Bill1 = Narration1[1].Split(']');
                            Bill = Bill1[0];
                        }
                    }
                    if (SegMentName == "NSE - CM" || SegMentName == "BSE - CM" || SegMentName == "NSE - CM,BSE - CM")
                    {
                        if (TranID == "XO")
                        {
                            DataTable dtProduct = oDBEngine.GetDataTable("Trans_CMPosition", "(select ltrim(rtrim(isnull(Equity_TickerSymbol,'')))+(case when isnull(convert(varchar(12),Equity_EffectUntil,113),'')='' then '' else ' ['+convert(varchar(12),Equity_EffectUntil,113)+']' end)+' ['+ltrim(rtrim(isnull(Equity_Series,'')))+']'+ case when isnull(cast(Equity_StrikePrice as varchar),'')='' then '' else ' ['+cast(Equity_StrikePrice as varchar)+']' end from master_Equity where Equity_SeriesID=Trans_CMPosition.CMPosition_ProductSeriesID) as CMPosition_ProductSeriesID,case when CMPosition_SqrOffQty=0 then null else CMPosition_SqrOffQty end as CMPosition_SqrOffQty,case when CMPosition_SqrOffPL=0 then null else CMPosition_SqrOffPL end as CMPosition_SqrOffPL,case when CMPosition_DeliveryBuyQty=0 then null else CMPosition_DeliveryBuyQty end as CMPosition_DeliveryBuyQty,case when CMPosition_DeliveryBuyValue=0 then null else CMPosition_DeliveryBuyValue end as CMPosition_DeliveryBuyValue,case when CMPosition_DeliverySellQty=0 then null else CMPosition_DeliverySellQty end as CMPosition_DeliverySellQty,case when CMPosition_DeliverySellValue=0 then null else CMPosition_DeliverySellValue end as CMPosition_DeliverySellValue,case when CMPosition_NetObligation=0 then null else CMPosition_NetObligation end as CMPosition_NetObligation,CMPosition_SettlementNumber+CMPosition_SettlementType as SettnumType,case when (isnull(CMPosition_DeliveryBuyValue,0)/isnull((case when CMPosition_DeliveryBuyQty=0 then null else CMPosition_DeliveryBuyQty end),1))=0 then null else (isnull(CMPosition_DeliveryBuyValue,0)/isnull((case when CMPosition_DeliveryBuyQty=0 then null else CMPosition_DeliveryBuyQty end),1)) end as AvgBuyVal,case when (isnull(CMPosition_DeliverySellValue,0)/isnull((case when CMPosition_DeliverySellQty=0 then null else CMPosition_DeliverySellQty end),1))=0 then null else (isnull(CMPosition_DeliverySellValue,0)/isnull((case when CMPosition_DeliverySellQty=0 then null else CMPosition_DeliverySellQty end),1)) end as AvgSellVal", " CmPosition_BillNumber='" + Bill + "'");
                            DataRow newRow1 = dtCashBankBook_New.NewRow();
                            String strHtmlAllClient = String.Empty;
                            decimal TotalVal = 0;
                            strHtmlAllClient = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + ">";
                            strHtmlAllClient += "<tr style=\"background-color: #DBEEF3;\">";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Scrip</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Delv.Buy Qty</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Delv.Buy Value</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Avg.Buy Value</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Delv.Sell Qty</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Delv.Sell Value</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Avg.Sell Value</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Sqr Qty</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Sqr P/L</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Total</td>";
                            strHtmlAllClient += "</tr>";
                            for (int j = 0; j < dtProduct.Rows.Count; j++)
                            {
                                strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                                strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\">" + dtProduct.Rows[j]["CMPosition_ProductSeriesID"] + "</td>";
                                if (dtProduct.Rows[j]["CMPosition_DeliveryBuyQty"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["CMPosition_DeliveryBuyQty"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["CMPosition_DeliveryBuyValue"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["CMPosition_DeliveryBuyValue"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["AvgBuyVal"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["AvgBuyVal"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["CMPosition_DeliverySellQty"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["CMPosition_DeliverySellQty"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["CMPosition_DeliverySellValue"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["CMPosition_DeliverySellValue"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["AvgSellVal"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["AvgSellVal"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["CMPosition_SqrOffQty"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["CMPosition_SqrOffQty"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["CMPosition_SqrOffPL"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["CMPosition_SqrOffPL"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["CMPosition_NetObligation"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["CMPosition_NetObligation"])) + "</td></tr>";
                                    TotalVal += Convert.ToDecimal(dtProduct.Rows[j]["CMPosition_NetObligation"]);
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td></tr>";
                                }
                            }
                            strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                            strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\"><b>Total :</b></td>";
                            strHtmlAllClient += "<td align=\"right\" colspan=\"9\" style=\"font-size:xx-small\"><b>" + oconverter.getFormattedvaluewithoriginalsign(TotalVal) + "</b></td></tr>";
                            strHtmlAllClient += "</table>";
                            newRow1[0] = "a";
                            newRow1[1] = dtProduct.Rows[0]["SettnumType"].ToString();
                            newRow1[2] = "";
                            newRow1[3] = strHtmlAllClient;
                            newRow1[4] = "";
                            newRow1[5] = 0;
                            newRow1[6] = 0;
                            newRow1[7] = "";
                            newRow1[8] = dtCashBankBook.Rows[i]["accountsledger_transactiondate"];
                            newRow1[9] = "";
                            newRow1[10] = "";
                            newRow1[11] = "";
                            newRow1[12] = "";
                            newRow1[13] = "";
                            newRow1[14] = "";
                            newRow1[15] = "";
                            newRow1[16] = "";
                            dtCashBankBook_New.Rows.Add(newRow1);
                            //displayALLCLIENT.InnerHtml = strHtmlAllClient;
                        }
                    }
                    else if (SegMentName == "NSE-FO")
                    {
                        if (TranID == "XO")
                        {
                            DataTable dtProduct = oDBEngine.GetDataTable("trans_foposition", "(select ltrim(rtrim(isnull(Equity_TickerSymbol,'')))+(case when isnull(convert(varchar(12),Equity_EffectUntil,113),'')='' then '' else ' ['+convert(varchar(12),Equity_EffectUntil,113)+']' end)+' ['+ltrim(rtrim(isnull(Equity_Series,'')))+']'+ case when isnull(CONVERT(VARCHAR,CONVERT(MONEY,cast(Equity_StrikePrice as varchar)),1),'')='' then '' else ' ['+CONVERT(VARCHAR,CONVERT(MONEY,cast(Equity_StrikePrice as varchar)),1)+']' end from master_Equity where Equity_SeriesID=trans_foposition.FOPosition_ProductSeriesID) as FOPosition_ProductSeriesID,case when isnull(foposition_BFPriceUnits,0)=0 then null else foposition_BFPriceUnits end as foposition_BFPriceUnits,case when isnull(foposition_openprice,0)=0 then null else foposition_openprice end as foposition_openprice,case when isnull(foposition_buyLots,0)=0 then null else foposition_buyLots end as foposition_buyPriceUnits,case when isnull(foposition_BuyValue,0)=0 then foposition_BuyValue else foposition_BuyValue end as foposition_BuyValue,case when isnull(foposition_BuyAverage,0)=0 then null else foposition_BuyAverage end as foposition_BuyAverage,case when isnull(foposition_sellLots,0)=0 then null else foposition_sellLots end as foposition_sellPriceUnits,case when isnull(foposition_SellValue,0)=0 then null else foposition_SellValue end as foposition_SellValue,case when isnull(foposition_SellAverage,0)=0 then null else foposition_SellAverage end as foposition_SellAverage,case when isnull(foposition_PostExcAsnDlvLongPriceUnits,0)=0 then foposition_PostExcAsnDlvShortPriceUnits else (-1)*foposition_PostExcAsnDlvLongPriceUnits end as CFQty,case when foposition_PostExcAsnDlvLongValue=0 then foposition_PostExcAsnDlvShortValue else foposition_PostExcAsnDlvLongValue end as CFPrice,foposition_MTMPL,(select DailyStat_SettlementPrice from Trans_dailyStatistics where DailyStat_ProductSeriesID=trans_foposition.FOPosition_ProductSeriesID and DailyStat_ExchangeSegmentID=" + Session["ExchangeSegmentID"].ToString() + " and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,DailyStat_DateTime)) as datetime) =cast(DATEADD(dd, 0, DATEDIFF(dd, 0, '" + TranDate + "')) as datetime)) as SettPrice", " foposition_BillNumber='" + Bill + "' and foposition_productseriesid in(select equity_seriesid from master_equity where equity_foidentifier like 'FUT%' and equity_effectuntil<>'" + dtCashBankBook.Rows[i]["accountsledger_transactiondate"] + "')");
                            DataRow newRow1 = dtCashBankBook_New.NewRow();
                            String strHtmlAllClient = String.Empty;
                            decimal TotalVal = 0;
                            strHtmlAllClient = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + ">";
                            strHtmlAllClient += "<tr style=\"background-color: #DBEEF3;\">";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Scrip</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">B/F Qty</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Price</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Buy Qty</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Value</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Avg</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Sell Qty</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Value</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Avg</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">C/F Qty</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Price</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">MTM</td>";
                            strHtmlAllClient += "</tr>";
                            for (int j = 0; j < dtProduct.Rows.Count; j++)
                            {
                                strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                                strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\">" + dtProduct.Rows[j]["FOPosition_ProductSeriesID"] + "</td>";
                                if (dtProduct.Rows[j]["foposition_BFPriceUnits"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_BFPriceUnits"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["foposition_openprice"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_openprice"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["foposition_buyPriceUnits"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_buyPriceUnits"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["foposition_BuyValue"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_BuyValue"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["foposition_BuyAverage"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_BuyValue"]) / Convert.ToDecimal(dtProduct.Rows[j]["foposition_buyPriceUnits"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["foposition_sellPriceUnits"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_sellPriceUnits"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["foposition_SellValue"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_SellValue"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["foposition_SellAverage"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_SellValue"]) / Convert.ToDecimal(dtProduct.Rows[j]["foposition_sellPriceUnits"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["CFQty"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["CFQty"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["SettPrice"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["SettPrice"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["foposition_MTMPL"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_MTMPL"])) + "</td></tr>";
                                    TotalVal += Convert.ToDecimal(dtProduct.Rows[j]["foposition_MTMPL"]);
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td></tr>";
                                }
                            }
                            strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                            strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\"><b>Total :</b></td>";
                            strHtmlAllClient += "<td align=\"right\" colspan=\"11\" style=\"font-size:xx-small\"><b>" + oconverter.getFormattedvaluewithoriginalsign(TotalVal) + "</b></td></tr>";
                            strHtmlAllClient += "</table>";
                            newRow1[0] = "a";
                            newRow1[1] = "";
                            newRow1[2] = "";
                            newRow1[3] = strHtmlAllClient;
                            newRow1[4] = "";
                            newRow1[5] = 0;
                            newRow1[6] = 0;
                            newRow1[7] = "";
                            newRow1[8] = dtCashBankBook.Rows[i]["accountsledger_transactiondate"];
                            newRow1[9] = "";
                            newRow1[10] = "";
                            newRow1[11] = "";
                            newRow1[12] = "";
                            newRow1[13] = "";
                            newRow1[14] = "";
                            newRow1[15] = "";
                            newRow1[16] = "";
                            dtCashBankBook_New.Rows.Add(newRow1);
                            //displayALLCLIENT.InnerHtml = strHtmlAllClient;
                        }
                        if (TranID == "XP")
                        {
                            DataTable dtProduct = oDBEngine.GetDataTable("trans_foposition", "(select ltrim(rtrim(isnull(Equity_TickerSymbol,'')))+(case when isnull(convert(varchar(12),Equity_EffectUntil,113),'')='' then '' else ' ['+convert(varchar(12),Equity_EffectUntil,113)+']' end)+' ['+ltrim(rtrim(isnull(Equity_Series,'')))+']'+ case when isnull(CONVERT(VARCHAR,CONVERT(MONEY,cast(Equity_StrikePrice as varchar)),1),'')='' then '' else ' ['+CONVERT(VARCHAR,CONVERT(MONEY,cast(Equity_StrikePrice as varchar)),1)+']' end from master_Equity where Equity_SeriesID=trans_foposition.FOPosition_ProductSeriesID) as FOPosition_ProductSeriesID,case when isnull(foposition_BuyLots,0)=0 then null else foposition_BuyLots end as foposition_BuyLots,case when isnull(foposition_BuyValue,0)=0 then null else foposition_BuyValue end as foposition_BuyValue,case when isnull(foposition_BuyAverage,0)=0 then null else foposition_BuyAverage end as foposition_BuyAverage,case when isnull(foposition_SellLots,0)=0 then null else foposition_SellLots end as foposition_SellLots,case when isnull(foposition_SellValue,0)=0 then null else foposition_SellValue end as foposition_SellValue,case when isnull(foposition_SellAverage,0)=0 then null else foposition_SellAverage end as foposition_SellAverage,case when isnull(foposition_NetPremium,0)=0 then null else foposition_NetPremium end as foposition_NetPremium ", " foposition_BillNumber='" + Bill + "' and  foposition_NetPremium is not null");
                            DataRow newRow1 = dtCashBankBook_New.NewRow();
                            String strHtmlAllClient = String.Empty;
                            decimal TotalVal = 0;
                            strHtmlAllClient = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + ">";
                            strHtmlAllClient += "<tr style=\"background-color: #DBEEF3;\">";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Scrip</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Buy Qty</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Value</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Avg</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Sell Qty</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Value</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Avg</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Net Premium</td>";
                            strHtmlAllClient += "</tr>";
                            for (int j = 0; j < dtProduct.Rows.Count; j++)
                            {
                                strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                                strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\">" + dtProduct.Rows[j]["FOPosition_ProductSeriesID"] + "</td>";
                                if (dtProduct.Rows[j]["foposition_BuyLots"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_BuyLots"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["foposition_BuyValue"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_BuyValue"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["foposition_BuyAverage"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_BuyValue"]) / Convert.ToDecimal(dtProduct.Rows[j]["foposition_BuyLots"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["foposition_SellLots"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_SellLots"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["foposition_SellValue"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_SellValue"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["foposition_SellAverage"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_SellValue"]) / Convert.ToDecimal(dtProduct.Rows[j]["foposition_SellLots"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["foposition_NetPremium"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_NetPremium"])) + "</td></tr>";
                                    TotalVal += Convert.ToDecimal(dtProduct.Rows[j]["foposition_NetPremium"]);
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td></tr>";
                                }
                            }
                            strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                            strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\"><b>Total :</b></td>";
                            strHtmlAllClient += "<td align=\"right\" colspan=\"11\" style=\"font-size:xx-small\"><b>" + oconverter.getFormattedvaluewithoriginalsign(TotalVal) + "</b></td></tr>";
                            strHtmlAllClient += "</table>";
                            newRow1[0] = "a";
                            newRow1[1] = "";
                            newRow1[2] = "";
                            newRow1[3] = strHtmlAllClient;
                            newRow1[4] = "";
                            newRow1[5] = 0;
                            newRow1[6] = 0;
                            newRow1[7] = "";
                            newRow1[8] = dtCashBankBook.Rows[i]["accountsledger_transactiondate"];
                            newRow1[9] = "";
                            newRow1[10] = "";
                            newRow1[11] = "";
                            newRow1[12] = "";
                            newRow1[13] = "";
                            newRow1[14] = "";
                            newRow1[15] = "";
                            newRow1[16] = "";
                            dtCashBankBook_New.Rows.Add(newRow1);
                            //displayALLCLIENT.InnerHtml = strHtmlAllClient;
                        }
                        if (TranID == "XZ")
                        {
                            DataTable dtProduct = oDBEngine.GetDataTable("trans_foposition", "(select ltrim(rtrim(isnull(Equity_TickerSymbol,'')))+(case when isnull(convert(varchar(12),Equity_EffectUntil,113),'')='' then '' else ' ['+convert(varchar(12),Equity_EffectUntil,113)+']' end)+' ['+ltrim(rtrim(isnull(Equity_Series,'')))+']'+ case when isnull(CONVERT(VARCHAR,CONVERT(MONEY,cast(Equity_StrikePrice as varchar)),1),'')='' then '' else ' ['+CONVERT(VARCHAR,CONVERT(MONEY,cast(Equity_StrikePrice as varchar)),1)+']' end from master_Equity where Equity_SeriesID=trans_foposition.FOPosition_ProductSeriesID) as FOPosition_ProductSeriesID,case when foposition_ExcAsnDlvLots<0 then 'Exercise' else 'Assigned' end as SettType,case when isnull(foposition_ExcAsnDlvLots,0)=0 then null else abs(foposition_ExcAsnDlvLots) end as foposition_ExcAsnDlvLots,case when isnull(foposition_SettlementPrice,0)=0 then null else abs(foposition_SettlementPrice) end as foposition_SettlementPrice,case when isnull(foposition_ExcAsnDlvMarkedValue,0)=0 then null else foposition_ExcAsnDlvMarkedValue end as foposition_ExcAsnDlvMarkedValue ", " foposition_BillNumber='" + Bill + "' and  foposition_ExcAsnDlvMarkedValue is not null");
                            DataRow newRow1 = dtCashBankBook_New.NewRow();
                            String strHtmlAllClient = String.Empty;
                            decimal TotalVal = 0;
                            strHtmlAllClient = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + ">";
                            strHtmlAllClient += "<tr style=\"background-color: #DBEEF3;\">";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Scrip</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Sett.Type</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Qty</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Set.Price</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Net Amount</td>";
                            strHtmlAllClient += "</tr>";
                            for (int j = 0; j < dtProduct.Rows.Count; j++)
                            {
                                strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                                strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\">" + dtProduct.Rows[j]["FOPosition_ProductSeriesID"] + "</td>";
                                strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\">" + dtProduct.Rows[j]["SettType"] + "</td>";
                                if (dtProduct.Rows[j]["foposition_ExcAsnDlvLots"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_ExcAsnDlvLots"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["foposition_SettlementPrice"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_SettlementPrice"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["foposition_ExcAsnDlvMarkedValue"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_ExcAsnDlvMarkedValue"])) + "</td></tr>";
                                    TotalVal += Convert.ToDecimal(dtProduct.Rows[j]["foposition_ExcAsnDlvMarkedValue"]);
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td></tr>";
                                }
                            }
                            strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                            strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\"><b>Total :</b></td>";
                            strHtmlAllClient += "<td align=\"right\" colspan=\"11\" style=\"font-size:xx-small\"><b>" + oconverter.getFormattedvaluewithoriginalsign(TotalVal) + "</b></td></tr>";
                            strHtmlAllClient += "</table>";
                            newRow1[0] = "a";
                            newRow1[1] = "";
                            newRow1[2] = "";
                            newRow1[3] = strHtmlAllClient;
                            newRow1[4] = "";
                            newRow1[5] = 0;
                            newRow1[6] = 0;
                            newRow1[7] = "";
                            newRow1[8] = dtCashBankBook.Rows[i]["accountsledger_transactiondate"];
                            newRow1[9] = "";
                            newRow1[10] = "";
                            newRow1[11] = "";
                            newRow1[12] = "";
                            newRow1[13] = "";
                            newRow1[14] = "";
                            newRow1[15] = "";
                            newRow1[16] = "";
                            dtCashBankBook_New.Rows.Add(newRow1);
                            //displayALLCLIENT.InnerHtml = strHtmlAllClient;
                        }
                        if (TranID == "XX")
                        {
                            DataTable dtProduct = oDBEngine.GetDataTable("trans_foposition", "(select ltrim(rtrim(isnull(Equity_TickerSymbol,'')))+(case when isnull(convert(varchar(12),Equity_EffectUntil,113),'')='' then '' else ' ['+convert(varchar(12),Equity_EffectUntil,113)+']' end)+' ['+ltrim(rtrim(isnull(Equity_Series,'')))+']'+ case when isnull(CONVERT(VARCHAR,CONVERT(MONEY,cast(Equity_StrikePrice as varchar)),1),'')='' then '' else ' ['+CONVERT(VARCHAR,CONVERT(MONEY,cast(Equity_StrikePrice as varchar)),1)+']' end from master_Equity where Equity_SeriesID=trans_foposition.FOPosition_ProductSeriesID) as FOPosition_ProductSeriesID,case when isnull(foposition_BFPriceUnits,0)=0 then null else foposition_BFPriceUnits end as foposition_BFPriceUnits,case when isnull(foposition_openprice,0)=0 then null else foposition_openprice end as foposition_openprice,case when isnull(foposition_buyLots,0)=0 then null else foposition_buyLots end as foposition_buyPriceUnits,case when isnull(foposition_BuyValue,0)=0 then foposition_BuyValue else foposition_BuyValue end as foposition_BuyValue,case when isnull(foposition_BuyAverage,0)=0 then null else foposition_BuyAverage end as foposition_BuyAverage,case when isnull(foposition_sellLots,0)=0 then null else foposition_sellLots end as foposition_sellPriceUnits,case when isnull(foposition_SellValue,0)=0 then null else foposition_SellValue end as foposition_SellValue,case when isnull(foposition_SellAverage,0)=0 then null else foposition_SellAverage end as foposition_SellAverage,case when isnull(foposition_PostExcAsnDlvLongPriceUnits,0)=0 then foposition_PostExcAsnDlvShortPriceUnits else (-1)*foposition_PostExcAsnDlvLongPriceUnits end as CFQty,case when foposition_PostExcAsnDlvLongValue=0 then foposition_PostExcAsnDlvShortValue else foposition_PostExcAsnDlvLongValue end as CFPrice,foposition_FutureFinalSettlement,(select DailyStat_SettlementPrice from Trans_dailyStatistics where DailyStat_ProductSeriesID=trans_foposition.FOPosition_ProductSeriesID and DailyStat_ExchangeSegmentID=" + Session["ExchangeSegmentID"].ToString() + " and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,DailyStat_DateTime)) as datetime) =cast(DATEADD(dd, 0, DATEDIFF(dd, 0, '" + TranDate + "')) as datetime)) as SettPrice", " foposition_BillNumber='" + Bill + "' and foposition_productseriesid in(select equity_seriesid from master_equity where equity_foidentifier like 'FUT%' and equity_effectuntil='" + dtCashBankBook.Rows[i]["accountsledger_transactiondate"] + "')");
                            DataRow newRow1 = dtCashBankBook_New.NewRow();
                            String strHtmlAllClient = String.Empty;
                            decimal TotalVal = 0;
                            strHtmlAllClient = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + ">";
                            strHtmlAllClient += "<tr style=\"background-color: #DBEEF3;\">";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Scrip</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">B/F Qty</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Price</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Buy Qty</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Value</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Avg</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Sell Qty</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Value</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Avg</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Exp Qty</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Price</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">MTM</td>";
                            strHtmlAllClient += "</tr>";
                            for (int j = 0; j < dtProduct.Rows.Count; j++)
                            {
                                strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                                strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\">" + dtProduct.Rows[j]["FOPosition_ProductSeriesID"] + "</td>";
                                if (dtProduct.Rows[j]["foposition_BFPriceUnits"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_BFPriceUnits"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["foposition_openprice"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_openprice"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["foposition_buyPriceUnits"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_buyPriceUnits"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["foposition_BuyValue"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_BuyValue"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["foposition_BuyAverage"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_BuyValue"]) / Convert.ToDecimal(dtProduct.Rows[j]["foposition_buyPriceUnits"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["foposition_sellPriceUnits"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_sellPriceUnits"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["foposition_SellValue"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_SellValue"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["foposition_SellAverage"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_SellValue"]) / Convert.ToDecimal(dtProduct.Rows[j]["foposition_sellPriceUnits"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["CFQty"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["CFQty"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["SettPrice"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["SettPrice"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["foposition_FutureFinalSettlement"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_FutureFinalSettlement"])) + "</td></tr>";
                                    TotalVal += Convert.ToDecimal(dtProduct.Rows[j]["foposition_FutureFinalSettlement"]);
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td></tr>";
                                }
                            }
                            strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                            strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\"><b>Total :</b></td>";
                            strHtmlAllClient += "<td align=\"right\" colspan=\"11\" style=\"font-size:xx-small\"><b>" + oconverter.getFormattedvaluewithoriginalsign(TotalVal) + "</b></td></tr>";
                            strHtmlAllClient += "</table>";
                            newRow1[0] = "a";
                            newRow1[1] = "";
                            newRow1[2] = "";
                            newRow1[3] = strHtmlAllClient;
                            newRow1[4] = "";
                            newRow1[5] = 0;
                            newRow1[6] = 0;
                            newRow1[7] = "";
                            newRow1[8] = dtCashBankBook.Rows[i]["accountsledger_transactiondate"];
                            newRow1[9] = "";
                            newRow1[10] = "";
                            newRow1[11] = "";
                            newRow1[12] = "";
                            newRow1[13] = "";
                            newRow1[14] = "";
                            newRow1[15] = "";
                            newRow1[16] = "";
                            dtCashBankBook_New.Rows.Add(newRow1);
                            //displayALLCLIENT.InnerHtml = strHtmlAllClient;
                        }
                    }
                    else if (SegMentName == "ICEX-COMM" || SegMentName == "MCX-COMM")
                    {
                        if (TranID == "XC")
                        {
                            DataTable dtProduct = oDBEngine.GetDataTable("Trans_comPosition", "(select ltrim(rtrim(isnull(Commodity_TickerSymbol,'')))+' ['+convert(varchar(12),Commodity_ExpiryDate,113)+']' from master_commodity where commodity_ProductSeriesID=trans_composition.comPosition_ProductSeriesID) as comPosition_ProductSeriesID,case when isnull(comPosition_BFPriceUnits,0)=0 then null else comPosition_BFPriceUnits end as comPosition_BFPriceUnits,case when isnull(comPosition_openprice,0)=0 then null else comPosition_openprice end as comPosition_openprice,case when isnull(comPosition_buyPriceUnits,0)=0 then null else comPosition_buyPriceUnits end as comPosition_buyPriceUnits,case when isnull(comPosition_BuyValue,0)=0 then comPosition_BuyValue else comPosition_BuyValue end as comPosition_BuyValue,case when isnull(comPosition_BuyAverage,0)=0 then null else comPosition_BuyAverage end as comPosition_BuyAverage,case when isnull(comPosition_sellPriceUnits,0)=0 then null else comPosition_sellPriceUnits end as comPosition_sellPriceUnits,case when isnull(comPosition_SellValue,0)=0 then null else comPosition_SellValue end as comPosition_SellValue,case when isnull(comPosition_SellAverage,0)=0 then null else comPosition_SellAverage end as comPosition_SellAverage,case when isnull(comPosition_PostExcAsnDlvLongPriceUnits,0)=0 then comPosition_PostExcAsnDlvShortPriceUnits else comPosition_PostExcAsnDlvLongPriceUnits end as CFQty,case when isnull(comPosition_PostExcAsnDlvLongValue,0)=0 then case when isnull(comPosition_PostExcAsnDlvShortValue,0)/abs(isnull(comPosition_PostExcAsnDlvShortPriceUnits,1))=0 then null else isnull(comPosition_PostExcAsnDlvShortValue,0)/abs(isnull(comPosition_PostExcAsnDlvShortPriceUnits,1)) end else case when isnull(comPosition_PostExcAsnDlvLongValue,0)/abs(isnull(comPosition_PostExcAsnDlvLongPriceUnits,1))=0 then null else isnull(comPosition_PostExcAsnDlvLongValue,0)/abs(isnull(comPosition_PostExcAsnDlvLongPriceUnits,1)) end end as CFPrice,comPosition_MTMPL", " comPosition_BillNumber='" + Bill + "'");
                            DataRow newRow1 = dtCashBankBook_New.NewRow();
                            String strHtmlAllClient = String.Empty;
                            decimal TotalVal = 0;
                            strHtmlAllClient = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + ">";
                            strHtmlAllClient += "<tr style=\"background-color: #DBEEF3;\">";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Scrip</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">B/F Qty</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Price</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Buy Qty</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Value</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Avg</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Sell Qty</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Value</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Avg</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">C/F Qty</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Price</td>";
                            strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">MTM</td>";
                            strHtmlAllClient += "</tr>";
                            for (int j = 0; j < dtProduct.Rows.Count; j++)
                            {
                                strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                                strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\">" + dtProduct.Rows[j]["comPosition_ProductSeriesID"] + "</td>";
                                if (dtProduct.Rows[j]["comPosition_BFPriceUnits"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["comPosition_BFPriceUnits"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["comPosition_openprice"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["comPosition_openprice"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["comPosition_buyPriceUnits"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["comPosition_buyPriceUnits"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["comPosition_BuyValue"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["comPosition_BuyValue"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["comPosition_BuyAverage"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["comPosition_BuyAverage"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["comPosition_sellPriceUnits"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["comPosition_sellPriceUnits"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["comPosition_SellValue"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["comPosition_SellValue"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["comPosition_SellAverage"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["comPosition_SellAverage"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["CFQty"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["CFQty"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["CFPrice"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["CFPrice"])) + "</td>";
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                                }
                                if (dtProduct.Rows[j]["comPosition_MTMPL"] != DBNull.Value)
                                {
                                    strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["comPosition_MTMPL"])) + "</td></tr>";
                                    TotalVal += Convert.ToDecimal(dtProduct.Rows[j]["comPosition_MTMPL"]);
                                }
                                else
                                {
                                    strHtmlAllClient += "<td>&nbsp;</td></tr>";
                                }
                            }
                            strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                            strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\"><b>Total :</b></td>";
                            strHtmlAllClient += "<td align=\"right\" colspan=\"11\" style=\"font-size:xx-small\"><b>" + oconverter.getFormattedvaluewithoriginalsign(TotalVal) + "</b></td></tr>";
                            strHtmlAllClient += "</table>";
                            newRow1[0] = "a";
                            newRow1[1] = "";
                            newRow1[2] = "";
                            newRow1[3] = strHtmlAllClient;
                            newRow1[4] = "";
                            newRow1[5] = 0;
                            newRow1[6] = 0;
                            newRow1[7] = "";
                            newRow1[8] = dtCashBankBook.Rows[i]["accountsledger_transactiondate"];
                            newRow1[9] = "";
                            newRow1[10] = "";
                            newRow1[11] = "";
                            newRow1[12] = "";
                            newRow1[13] = "";
                            newRow1[14] = "";
                            newRow1[15] = "";
                            newRow1[16] = "";
                            dtCashBankBook_New.Rows.Add(newRow1);
                            //displayALLCLIENT.InnerHtml = strHtmlAllClient;
                        }
                    }
                }
                #endregion
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
                //openingBal = Convert.ToDecimal(OpenBalance.Rows[0][1].ToString()) * (-1);
                openingBal = Convert.ToDecimal(OpenBalance.Rows[0][1].ToString());
                ScriptManager.RegisterStartupScript(this, this.GetType(), "hide", "Disable('P');", true);
                // Page.ClientScript.RegisterStartupScript(GetType(), "hide", "<script language='javascript'>Disable('P');</script>");
            }
            if (pageindex >= int.Parse(TotalPages.Value.ToString()))
            {
                pageindex = int.Parse(TotalPages.Value.ToString()) - 1;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "hide", "Disable('N');", true);
                // Page.ClientScript.RegisterStartupScript(GetType(), "hide", "<script language='javascript'>Disable('N');</script>");
            }
            if (pageindex >= (int.Parse(TotalPages.Value.ToString()) - 1))
            {
                //pageindex = int.Parse(TotalPages.Value.ToString()) - 1;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "hide", "Disable('N');", true);
                // Page.ClientScript.RegisterStartupScript(GetType(), "hide", "<script language='javascript'>Disable('N');</script>");
            }
            if (pageindex > 0)
            {
                int totalRecord = (pageindex) * pageSize;
                openingBal = Convert.ToDecimal(OpenBalance.Rows[0][1].ToString()) * (-1);
                decimal DR = 0;
                decimal CR = 0;
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
            if (Session["userlastsegment"].ToString() == "7" || Session["userlastsegment"].ToString() == "18")
                grdCashBankBook.Columns[5].Visible = true;
            else
                grdCashBankBook.Columns[5].Visible = false;
            if (Session["userlastsegment"].ToString() == "7" || Session["userlastsegment"].ToString() == "8" || Session["userlastsegment"].ToString() == "18")
                grdCashBankBook.Columns[6].Visible = true;
            else
                grdCashBankBook.Columns[6].Visible = false;
            if (radConsolidated.Checked == true)
            {
                grdCashBankBook.Columns[2].Visible = false;
            }
            else
            {
                grdCashBankBook.Columns[2].Visible = true;
            }
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
            grdCashBankBook.FooterRow.Cells[9].HorizontalAlign = HorizontalAlign.Right;
            grdCashBankBook.FooterRow.Cells[10].HorizontalAlign = HorizontalAlign.Right;
            grdCashBankBook.FooterRow.Cells[11].HorizontalAlign = HorizontalAlign.Right;
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
            dtSearchDate.Value = "01-01-0100";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct", "ShowHideSearch()", true);
        }

        #region Populate Group
        protected void btnhide_Click(object sender, EventArgs e)
        {
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                BindGroup();
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
        #endregion

        void fn_Client()
        {
            BranchId = HdnSegment.Value;
            Group = HdnGroup.Value;
            if (ddlAccountType.SelectedItem.Value == "0")
                MainAcc = "'SYSTM00001'";
            else if (ddlAccountType.SelectedItem.Value == "1")
                MainAcc = "'SYSTM00002'";
            else if (ddlAccountType.SelectedItem.Value == "2")
                MainAcc = "'SYSTM00001','SYSTM00002'";
            else
                MainAcc = HdnMainAc.Value;
            if (BranchId != "" && BranchId != null)
                ViewState["branchID"] = BranchId;
            else
                ViewState["branchID"] = Session["userbranchHierarchy"].ToString();

            if (rdbClientALL.Checked)//////////////////ALL CLIENT CHECK
            {
                if (ddlGroup.SelectedItem.Value.ToString() == "1")
                {
                    if (rdddlgrouptypeAll.Checked)
                    {
                        if (ddlgrouptype.SelectedItem.Value.ToString() != "0")
                        {
                            DataTable dtclient = new DataTable();
                            dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalid in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster where gpm_type='" + ddlgrouptype.SelectedItem.Text.ToString() + "')) and cnt_branchid in (" + Session["userbranchHierarchy"].ToString() + ")", "cnt_branchid");
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
                        dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalid in(select grp_contactid from tbl_trans_group where grp_groupmaster in(" + Group + ") and cnt_branchid in (" + Session["userbranchHierarchy"].ToString() + "))", "cnt_branchid");
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
                        dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ")", "cnt_branchid");
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
                        dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_branchid in(" + BranchId + ")", "cnt_branchid");
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
                if (ddlGroup.SelectedItem.Value.ToString() == "1")
                {
                    if (rdddlgrouptypeAll.Checked)
                    {
                        if (ddlgrouptype.SelectedItem.Value.ToString() != "0")
                        {
                            DataTable dtclient = new DataTable();
                            dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalid in (select dpd_cntID from tbl_master_contactdpdetails where dpd_POA=1) and cnt_internalid in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster where gpm_type='" + ddlgrouptype.SelectedItem.Text.ToString() + "'))", "cnt_branchid");
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
                        dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalid in (select dpd_cntID from tbl_master_contactdpdetails where dpd_POA=1) and cnt_internalid in(select grp_contactid from tbl_trans_group where grp_groupmaster in(" + Group + "))", "cnt_branchid");
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
                        dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalid in (select dpd_cntID from tbl_master_contactdpdetails where dpd_POA=1) and cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ")", "cnt_branchid");
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
                        dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalid in (select dpd_cntID from tbl_master_contactdpdetails where dpd_POA=1) and cnt_branchid in(" + BranchId + ")", "cnt_branchid");
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


            if (rdbClientSelected.Checked == true)
                Clients = HdnSubAc.Value;

            ViewState["Clients"] = Clients;


            //HdnSubAc.Value =Convert.ToString(ViewState["Clients"]);

        }

        void fn_ClientCDSL()
        {
            ViewState["Clients"] = null;
            BranchId = HdnSegment.Value;
            Group = HdnGroup.Value;
            MainAcc = HdnMainAc.Value;
            SubLedgerType = HdnSubLedgerType.Value;
            if (BranchId != "" && BranchId != null)
                ViewState["branchID"] = BranchId;
            else
                ViewState["branchID"] = Session["userbranchHierarchy"].ToString();
            string NSDlCdsl = null;
            if (rdbClientALL.Checked)//////////////////ALL CLIENT CHECK
            {
                if (ddlGroup.SelectedItem.Value.ToString() == "1")
                {
                    if (rdddlgrouptypeAll.Checked)
                    {
                        if (ddlgrouptype.SelectedItem.Value.ToString() != "0")
                        {
                            DataTable dtclient = new DataTable();
                            dtclient = oDBEngine.GetDataTable("tbl_trans_group", "substring(ltrim(rtrim(grp_contactid)),9,8)", "  grp_contactid in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster where gpm_type='" + ddlgrouptype.SelectedItem.Text.ToString() + "'))");
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
                        dtclient = oDBEngine.GetDataTable("tbl_trans_group", "substring(ltrim(rtrim(grp_contactid)),9,8)", " grp_contactid in(select grp_contactid from tbl_trans_group where grp_groupmaster in(" + Group + "))");
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
                        if (SubLedgerType == "CDSL Clients")
                            dtclient = oDBEngine.GetDataTable("master_CdslClients", "CdslClients_BenAccountNumber", null);
                        else if (SubLedgerType == "NSDL Clients")
                            dtclient = oDBEngine.GetDataTable("master_NsdlClients", "NsdlClients_BenAccountID", null);
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
                        if (SubLedgerType == "CDSL Clients")
                            dtclient = oDBEngine.GetDataTable("master_CdslClients", "CdslClients_BenAccountNumber", null);
                        else if (SubLedgerType == "NSDL Clients")
                            dtclient = oDBEngine.GetDataTable("master_NsdlClients", "NsdlClients_BenAccountID", null);
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
            if (rdbClientSelected.Checked == true)
                Clients = HdnSubAc.Value;

            ViewState["Clients"] = Clients;


        }

        public void GenerateHTML()
        {
            DataSet dsHTML = (DataSet)ViewState["dsHTML"];
            DataTable dtMain = dsHTML.Tables[2];
            DataTable dtScripCM = dsHTML.Tables[0];
            DataTable dtTax = dsHTML.Tables[1];
            DataTable dtScripFO = dsHTML.Tables[5];
            ViewState["EmailHTML"] = "";
            string SegmentName = "";
            string SegID = "";
            String CustomerID = "";
            string SettlementNo = "";
            string SettlementType = "";
            string SettlementNOType = "";
            string TransactionDate = "";
            string IsConsolidate = "";
            string strHTML = "";
            decimal TotOblg = 0;
            decimal TotChrg = 0;
            decimal TotNet = 0;

            strHTML = "<table border=\"1\" width=\"100%\" cellspacing=\"0\" cellpadding=\"0\">";
            strHTML = strHTML + "<tr style=\"background-color:#B7CEEC;font-weight:bold;height:30px\">";
            strHTML = strHTML + "<td style=\"font-weight:bold;\">Tr. Date</td>";
            strHTML = strHTML + "<td style=\"font-weight:bold;\">Value Date</td>";
            strHTML = strHTML + "<td style=\"font-weight:bold;\">Inst. No.</td>";
            strHTML = strHTML + "<td style=\"font-weight:bold;\">Description</td>";
            strHTML = strHTML + "<td style=\"font-weight:bold;\">Trade Date</td>";
            strHTML = strHTML + "<td style=\"font-weight:bold;\">Settlement No.</td>";
            strHTML = strHTML + "<td style=\"font-weight:bold;\"  align=\"right\" >Debit</td>";
            strHTML = strHTML + "<td style=\"font-weight:bold;\"  align=\"right\" >Credit</td>";
            strHTML = strHTML + "<td style=\"font-weight:bold;\"  align=\"right\" >Closing</td>";
            strHTML = strHTML + "</tr>";
            if (dtMain.Rows.Count > 0)
            {
                if (dtMain.Rows[0]["OpeningBalanceDr"] == DBNull.Value || dtMain.Rows[0]["OpeningBalanceDr"] == "")
                    dtMain.Rows[0]["OpeningBalanceDr"] = "0";
                if (dtMain.Rows[0]["OpeningBalanceCr"] == DBNull.Value || dtMain.Rows[0]["OpeningBalanceCr"] == "")
                    dtMain.Rows[0]["OpeningBalanceCr"] = "0";
                if (dtMain.Rows[0]["OpeningTotal"] == DBNull.Value || dtMain.Rows[0]["OpeningTotal"] == "")
                    dtMain.Rows[0]["OpeningTotal"] = "0";

                if (dtMain.Rows[0]["OpeningBalanceDr"] == DBNull.Value || dtMain.Rows[0]["OpeningBalanceDr"].ToString() == "" || dtMain.Rows[0]["OpeningBalanceDr"].ToString() == "0")
                    strHTML = strHTML + "<tr  style=\"background-color:#EEE0E5;font-weight:bold;\"><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td style=\"font-weight:bold;\">Opening Balance</td><td>&nbsp;</td><td>&nbsp;</td><td  align=\"right\" style=\"font-weight:bold;\">&nbsp;</td>";
                else
                    strHTML = strHTML + "<tr  style=\"background-color:#EEE0E5;font-weight:bold;\"><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td style=\"font-weight:bold;\">Opening Balance</td><td>&nbsp;</td><td>&nbsp;</td><td  align=\"right\" style=\"font-weight:bold;\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[0]["OpeningBalanceDr"].ToString())) + "</td>";
                if (dtMain.Rows[0]["OpeningBalanceCr"] == DBNull.Value || dtMain.Rows[0]["OpeningBalanceCr"].ToString() == "" || dtMain.Rows[0]["OpeningBalanceCr"].ToString() == "0")
                    strHTML = strHTML + "<td  align=\"right\" style=\"font-weight:bold;\">&nbsp</td>";
                else
                    strHTML = strHTML + "<td  align=\"right\" style=\"font-weight:bold;\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[0]["OpeningBalanceCr"].ToString())) + "</td>";

                if (dtMain.Rows[0]["OpeningTotal"] == DBNull.Value || dtMain.Rows[0]["OpeningTotal"].ToString() == "" || dtMain.Rows[0]["OpeningTotal"].ToString() == "0")
                    strHTML = strHTML + "<td  align=\"right\" style=\"font-weight:bold;\">&nbsp</td></tr>";
                else
                    strHTML = strHTML + "<td  align=\"right\" style=\"font-weight:bold;\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[0]["OpeningTotal"].ToString())) + "</td></tr>";
            }


            for (int i = 0; i < dtMain.Rows.Count; i++)
            {
                //--------------------Get Variable-------------------
                SegmentName = dtMain.Rows[i]["SegmentName"].ToString();
                SegID = dtMain.Rows[i]["SegID"].ToString();
                CustomerID = dtMain.Rows[i]["SubID"].ToString();
                SettlementNo = dtMain.Rows[i]["SettlementN"].ToString();
                SettlementType = dtMain.Rows[i]["SettlementT"].ToString();
                SettlementNOType = dtMain.Rows[i]["SettlementNumber"].ToString();
                TransactionDate = dtMain.Rows[i]["accountsledger_transactiondate"].ToString();

                if (dtMain.Rows[i]["accountsledger_Narration"].ToString().Length > 20)
                {
                    IsConsolidate = dtMain.Rows[i]["accountsledger_Narration"].ToString().Substring(0, 21);
                }
                else
                {
                    IsConsolidate = dtMain.Rows[i]["accountsledger_Narration"].ToString();
                }
                //--------------------End -------------------

                //if (dtMain.Rows[i][7] == DBNull.Value || dtMain.Rows[i][7] == "")
                //    dtMain.Rows[i][7] = "0";
                //if (dtMain.Rows[i][6] == DBNull.Value || dtMain.Rows[i][6] == "")
                //    dtMain.Rows[i][6] = "0";
                //if (dtMain.Rows[i][8] == DBNull.Value || dtMain.Rows[i][8] == "")
                //    dtMain.Rows[i][8] = "0";

                strHTML = strHTML + "<tr>";

                strHTML = strHTML + "<td style=\"Width:7%;\">&nbsp;" + dtMain.Rows[i][1].ToString() + "</td>";
                strHTML = strHTML + "<td style=\"Width:7%;\">&nbsp;" + dtMain.Rows[i][2].ToString() + "</td>";
                strHTML = strHTML + "<td>&nbsp;" + dtMain.Rows[i][10].ToString() + "</td>";
                strHTML = strHTML + "<td>&nbsp;" + dtMain.Rows[i][4].ToString() + "| [" + SegmentName + "]</td>";
                if (SegmentName == "NSE-CM" || SegmentName == "BSE-CM" || SegmentName == "CSE-CM")
                {
                    strHTML = strHTML + "<td>&nbsp;" + dtMain.Rows[i]["PayoutDate"].ToString() + "</td>";
                }
                else
                {
                    strHTML = strHTML + "<td>&nbsp;" + dtMain.Rows[i]["TrDate"].ToString() + "</td>";
                }
                strHTML = strHTML + "<td>&nbsp;" + dtMain.Rows[i][11].ToString() + "</td>";


                if (dtMain.Rows[i]["Accountsledger_AmountCr"] == DBNull.Value || dtMain.Rows[i]["Accountsledger_AmountCr"].ToString() == "" || dtMain.Rows[i]["Accountsledger_AmountCr"].ToString() == "0")
                    strHTML = strHTML + "<td align=\"right\">&nbsp;</td>";
                else
                    strHTML = strHTML + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["Accountsledger_AmountCr"].ToString())) + "</td>";

                if (dtMain.Rows[i]["Accountsledger_AmountDr"] == DBNull.Value || dtMain.Rows[i]["Accountsledger_AmountDr"].ToString() == "" || dtMain.Rows[i]["Accountsledger_AmountDr"].ToString() == "0")
                    strHTML = strHTML + "<td align=\"right\">&nbsp;</td>";
                else
                    strHTML = strHTML + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["Accountsledger_AmountDr"].ToString())) + "</td>";

                if (dtMain.Rows[i]["Closing"] == DBNull.Value || dtMain.Rows[i]["Closing"].ToString() == "" || dtMain.Rows[i]["Closing"].ToString() == "0")
                {
                    strHTML = strHTML + "<td align=\"right\">&nbsp;</td>";
                }
                else
                {
                    if (dtMain.Rows[i]["Closing"].ToString().Substring(0, 1) == "-")
                        strHTML = strHTML + "<td align=\"right\" style=\"color:red\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["Closing"].ToString())) + "</td>";
                    else
                        strHTML = strHTML + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["Closing"].ToString())) + "</td>";
                }

                strHTML = strHTML + "</tr>";
                string SubStr = "";
                if (IsConsolidate == "Settlement Obligation")
                {

                    if (SegmentName == "NSE-CM" || SegmentName == "BSE-CM" || SegmentName == "CSE-CM")
                    {
                        if (dtScripCM.Rows.Count > 0)
                        {
                            TotOblg = 0;
                            SubStr = "<table border=\"1\" width=\"100%\">";
                            SubStr = SubStr + "<tr style=\"background-color:#EEE0E5;font-weight:bold;\"><td style=\"font-weight:bold;\" colspan=\"5\">Obligation Breakup For Settlement No. " + SettlementNOType + "</td></tr>";
                            SubStr = SubStr + "<tr style=\"background-color:#EEE0E5;font-weight:bold;\"><td style=\"font-weight:bold;\">Scrip</td><td style=\"font-weight:bold;\">Type</td><td style=\"font-weight:bold;\" align=\"right\">Quantity</td><td align='right' style=\"font-weight:bold;\">Average</td><td align=\"right\" style=\"font-weight:bold;\">Amount</td></tr>";
                            for (int j = 0; j < dtScripCM.Rows.Count; j++)
                            {

                                if (dtScripCM.Rows[j]["CMPOSITION_CUSTOMERID"].ToString().Trim() == CustomerID && (dtScripCM.Rows[j]["SETTLEMENT"].ToString().Trim() + " " == SettlementNOType))
                                {

                                    SubStr = SubStr + "<tr><td>&nbsp;" + dtScripCM.Rows[j]["PRODUCTSERIESID"].ToString() + "</td><td>&nbsp;" + dtScripCM.Rows[j]["DELIVERYTYPE"].ToString() + "</td>";
                                    if (oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripCM.Rows[j]["QUANTITY"].ToString())) == "0.00")
                                        SubStr = SubStr + "<td align=\"right\">&nbsp;</td><td align=\"right\">&nbsp;</td>";
                                    else
                                    {
                                        if (Convert.ToString(dtScripCM.Rows[j]["DELIVERYTYPE"]) != "DIFF")
                                            SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripCM.Rows[j]["QUANTITY"].ToString())) + "</td><td align=\"right\">&nbsp;" + getFormattedDecvalue(Convert.ToDecimal(getFormattedDecvalue(Convert.ToDecimal(dtScripCM.Rows[j]["AMOUNT"].ToString()))) / Convert.ToDecimal(getFormattedDecvalue(Convert.ToDecimal(dtScripCM.Rows[j]["QUANTITY"].ToString())))) + "</td>";
                                        else
                                            SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripCM.Rows[j]["QUANTITY"].ToString())) + "</td><td align=\"right\">&nbsp;</td>";
                                    }

                                    if (oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripCM.Rows[j]["AMOUNT"].ToString())) == "0.00")
                                        SubStr = SubStr + "<td align=\"right\">&nbsp;</td></tr>";
                                    else
                                        SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripCM.Rows[j]["AMOUNT"].ToString())) + "</td></tr>";


                                    TotOblg = TotOblg + Convert.ToDecimal(dtScripCM.Rows[j]["AMOUNT"].ToString());
                                }

                            }
                            SubStr = SubStr + "<tr style=\"background-color:#EEEED1;font-weight:bold;\"><td colspan=\"3\" align=\"right\" style=\"font-weight:bold;\">&nbsp;Net Obligation</td><td colspan='2' align=\"right\" >&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(TotOblg) + "</td></tr>";
                            TotChrg = 0;
                            for (int k = 0; k < dtTax.Rows.Count; k++)
                            {
                                if (dtTax.Rows[k]["ACCOUNTSLEDGER_SUBACCOUNTID"].ToString().Trim() == CustomerID && (dtTax.Rows[k]["SETTLEMENT"].ToString().Trim() + " ") == SettlementNOType)
                                {
                                    SubStr = SubStr + "<tr><td colspan=\"3\" align=\"right\">&nbsp;" + dtTax.Rows[k]["CHARGE_TYPE1"].ToString() + "</td><td colspan='2' align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtTax.Rows[k]["ACCOUNTSLEDGER_AMOUNTDR"].ToString())) + "</td></tr>";
                                    TotChrg = TotChrg + Convert.ToDecimal(dtTax.Rows[k]["ACCOUNTSLEDGER_AMOUNTDR"].ToString());

                                }

                            }
                            TotNet = 0;
                            TotNet = TotOblg + TotChrg;
                            SubStr = SubStr + "<tr style=\"background-color:#EEEED1;font-weight:bold;\"><td colspan=\"3\" align=\"right\" style=\"font-weight:bold;\">&nbsp;Total Charges</td><td colspan='2' align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(TotChrg) + "</td></tr>";
                            SubStr = SubStr + "<tr style=\"background-color:#EEEED1;font-weight:bold;\"><td  colspan=\"3\" align=\"right\" style=\"font-weight:bold;\">&nbsp;Net Total</td><td colspan='2' align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(TotNet) + "</td></tr>";


                            SubStr = SubStr + "</table>";

                        }

                    }
                    if (SegmentName == "NSE-FO" || SegmentName == "BSE-FO")
                    {

                        if (dtScripFO.Rows.Count > 0)
                        {
                            SubStr = "<table border=\"1\" width=\"100%\">";
                            SubStr = SubStr + "<tr style=\"background-color:#EEE0E5;font-weight:bold;\"><td style=\"font-weight:bold;\" colspan=\"10\">Obligation Break Up For " + TransactionDate + "</td></tr>";
                            SubStr = SubStr + "<tr style=\"background-color:#EEE0E5;font-weight:bold;\"><td style=\"font-weight:bold;\">SCRIP</td><td align=\"right\" style=\"font-weight:bold;\">BF</td><td align=\"right\" style=\"font-weight:bold;\">BF PRICE</td><td align=\"right\" style=\"font-weight:bold;\">BUY</td><td align=\"right\" style=\"font-weight:bold;\">BUY PRICE</td><td align=\"right\" style=\"font-weight:bold;\">SELL</td><td align=\"right\" style=\"font-weight:bold;\">SELL PRICE</td><td align=\"right\" style=\"font-weight:bold;\">CF</td><td align=\"right\" style=\"font-weight:bold;\">CF PRICE</td><td align=\"right\" style=\"font-weight:bold;\">NET</td></tr>";
                            for (int n = 0; n < dtScripFO.Rows.Count; n++)
                            {
                                if (dtScripFO.Rows[n]["FOPosition_CustomerExchangeID"].ToString() == CustomerID && dtScripFO.Rows[n]["DATE_FOR"].ToString() == TransactionDate)
                                {

                                    SubStr = SubStr + "<tr><td>&nbsp;" + dtScripFO.Rows[n]["SCRIP"].ToString() + "</td>";


                                    if (dtScripFO.Rows[n]["BF"] == DBNull.Value || dtScripFO.Rows[n]["BF"].ToString() == "" || dtScripFO.Rows[n]["BF"].ToString() == "0.000000")
                                        SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                    else
                                        SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["BF"].ToString())) + "</td>";

                                    if (dtScripFO.Rows[n]["BF_PRICE"] == DBNull.Value || dtScripFO.Rows[n]["BF_PRICE"].ToString() == "" || dtScripFO.Rows[n]["BF_PRICE"].ToString() == "0.000000")
                                        SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                    else
                                        //SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["BF_PRICE"].ToString())) + "</td>";
                                        SubStr = SubStr + "<td align=\"right\">&nbsp;" + getFormattedDecvalue(Convert.ToDecimal(dtScripFO.Rows[n]["BF_PRICE"].ToString())) + "</td>";



                                    if (dtScripFO.Rows[n]["BUY"] == DBNull.Value || dtScripFO.Rows[n]["BUY"].ToString() == "" || dtScripFO.Rows[n]["BUY"].ToString() == "0.000000")
                                        SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                    else
                                        SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["BUY"].ToString())) + "</td>";
                                    if (dtScripFO.Rows[n]["BUY_PRICE"] == DBNull.Value || dtScripFO.Rows[n]["BUY_PRICE"].ToString() == "" || dtScripFO.Rows[n]["BUY_PRICE"].ToString() == "0.000000")
                                        SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                    else
                                        //SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["BUY_PRICE"].ToString())) + "</td>";
                                        SubStr = SubStr + "<td align=\"right\">&nbsp;" + getFormattedDecvalue(Convert.ToDecimal(dtScripFO.Rows[n]["BUY_PRICE"].ToString())) + "</td>";



                                    if (dtScripFO.Rows[n]["SELL"] == DBNull.Value || dtScripFO.Rows[n]["SELL"].ToString() == "" || dtScripFO.Rows[n]["SELL"].ToString() == "0.000000")
                                        SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                    else
                                        SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["SELL"].ToString())) + "</td>";
                                    if (dtScripFO.Rows[n]["SELL_PRICE"] == DBNull.Value || dtScripFO.Rows[n]["SELL_PRICE"].ToString() == "" || dtScripFO.Rows[n]["SELL_PRICE"].ToString() == "0.000000")
                                        SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                    else
                                        //SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["SELL_PRICE"].ToString())) + "</td>";
                                        SubStr = SubStr + "<td align=\"right\">&nbsp;" + getFormattedDecvalue(Convert.ToDecimal(dtScripFO.Rows[n]["SELL_PRICE"].ToString())) + "</td>";




                                    if (dtScripFO.Rows[n]["CF"] == DBNull.Value || dtScripFO.Rows[n]["CF"].ToString() == "" || dtScripFO.Rows[n]["CF"].ToString() == "0.000000")
                                        SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                    else
                                        SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["CF"].ToString())) + "</td>";
                                    if (dtScripFO.Rows[n]["CF_PRICE"] == DBNull.Value || dtScripFO.Rows[n]["CF_PRICE"].ToString() == "" || dtScripFO.Rows[n]["CF_PRICE"].ToString() == "0.000000")
                                        SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                    else
                                        //SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["CF_PRICE"].ToString())) + "</td>";
                                        SubStr = SubStr + "<td align=\"right\">&nbsp;" + getFormattedDecvalue(Convert.ToDecimal(dtScripFO.Rows[n]["CF_PRICE"].ToString())) + "</td>";


                                    SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["NET"].ToString())) + "</td></tr>";
                                    TotOblg = TotOblg + Convert.ToDecimal(dtScripFO.Rows[n]["NET"].ToString());
                                }

                            }

                            SubStr = SubStr + "<tr style=\"background-color:#EEEED1;font-weight:bold;\"><td colspan=\"9\" align=\"right\" style=\"font-weight:bold;\">&nbsp;Net Obligation </td><td align=\"right\">" + oconverter.getFormattedvaluewithoriginalsign(TotOblg) + "</td></tr>";


                            TotChrg = 0;
                            for (int k = 0; k < dtTax.Rows.Count; k++)
                            {
                                if (dtTax.Rows[k]["ACCOUNTSLEDGER_SUBACCOUNTID"].ToString().Trim() == CustomerID && (dtTax.Rows[k]["SETTLEMENT"].ToString().Trim() + " ") == SettlementNOType && dtTax.Rows[k]["CHARGE_DATE"].ToString().Trim() == TransactionDate)
                                {
                                    SubStr = SubStr + "<tr><td colspan=\"9\" align=\"right\">&nbsp;" + dtTax.Rows[k]["CHARGE_TYPE1"].ToString() + "</td><td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtTax.Rows[k]["ACCOUNTSLEDGER_AMOUNTDR"].ToString())) + "</td></tr>";
                                    TotChrg = TotChrg + Convert.ToDecimal(dtTax.Rows[k]["ACCOUNTSLEDGER_AMOUNTDR"].ToString());

                                }

                            }
                            TotNet = 0;
                            TotNet = TotOblg + TotChrg;
                            SubStr = SubStr + "<tr style=\"background-color:#EEEED1;font-weight:bold;\"><td colspan=\"9\" align=\"right\" style=\"font-weight:bold;\">&nbsp;Total Charges</td><td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(TotChrg) + "</td></tr>";
                            SubStr = SubStr + "<tr style=\"background-color:#EEEED1;font-weight:bold;\"><td colspan=\"9\" align=\"right\" style=\"font-weight:bold;\">&nbsp;Net Total</td><td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(TotNet) + "</td></tr>";


                            SubStr = SubStr + "</table>";

                        }




                    }




                    else
                    {

                        if (dtScripFO.Rows.Count > 0)
                        {
                            string test = dtMain.Rows[i]["SegID"].ToString();
                            SubStr = "<table border=\"1\" width=\"100%\">";
                            SubStr = SubStr + "<tr style=\"background-color:#EEE0E5;font-weight:bold;\"><td style=\"font-weight:bold;\" colspan=\"10\">Obligation Break Up For " + TransactionDate + "</td></tr>";
                            SubStr = SubStr + "<tr style=\"background-color:#EEE0E5;font-weight:bold;\"><td style=\"font-weight:bold;\">SCRIP</td><td align=\"right\" style=\"font-weight:bold;\">BF</td><td align=\"right\" style=\"font-weight:bold;\">BF PRICE</td><td align=\"right\" style=\"font-weight:bold;\">BUY</td><td align=\"right\" style=\"font-weight:bold;\">BUY PRICE</td><td align=\"right\" style=\"font-weight:bold;\">SELL</td><td align=\"right\" style=\"font-weight:bold;\">SELL PRICE</td><td align=\"right\" style=\"font-weight:bold;\">CF</td><td align=\"right\" style=\"font-weight:bold;\">CF PRICE</td><td align=\"right\" style=\"font-weight:bold;\">NET</td></tr>";
                            for (int n = 0; n < dtScripFO.Rows.Count; n++)
                            {
                                if (dtScripFO.Rows[n]["FOPosition_CustomerExchangeID"].ToString() == CustomerID && dtScripFO.Rows[n]["DATE_FOR"].ToString() == TransactionDate && dtScripFO.Rows[n]["segid"].ToString() == test.Trim())
                                {

                                    SubStr = SubStr + "<tr><td>&nbsp;" + dtScripFO.Rows[n]["SCRIP"].ToString() + "</td>";


                                    if (dtScripFO.Rows[n]["BF"] == DBNull.Value || dtScripFO.Rows[n]["BF"].ToString() == "" || dtScripFO.Rows[n]["BF"].ToString() == "0.000000")
                                        SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                    else
                                        SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["BF"].ToString())) + "</td>";

                                    if (dtScripFO.Rows[n]["BF_PRICE"] == DBNull.Value || dtScripFO.Rows[n]["BF_PRICE"].ToString() == "" || dtScripFO.Rows[n]["BF_PRICE"].ToString() == "0.000000")
                                        SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                    else
                                        //SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["BF_PRICE"].ToString())) + "</td>";
                                        SubStr = SubStr + "<td align=\"right\">&nbsp;" + getFormattedDecvalue(Convert.ToDecimal(dtScripFO.Rows[n]["BF_PRICE"].ToString())) + "</td>";



                                    if (dtScripFO.Rows[n]["BUY"] == DBNull.Value || dtScripFO.Rows[n]["BUY"].ToString() == "" || dtScripFO.Rows[n]["BUY"].ToString() == "0.000000")
                                        SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                    else
                                        SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["BUY"].ToString())) + "</td>";
                                    if (dtScripFO.Rows[n]["BUY_PRICE"] == DBNull.Value || dtScripFO.Rows[n]["BUY_PRICE"].ToString() == "" || dtScripFO.Rows[n]["BUY_PRICE"].ToString() == "0.000000")
                                        SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                    else
                                        //SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["BUY_PRICE"].ToString())) + "</td>";
                                        SubStr = SubStr + "<td align=\"right\">&nbsp;" + getFormattedDecvalue(Convert.ToDecimal(dtScripFO.Rows[n]["BUY_PRICE"].ToString())) + "</td>";



                                    if (dtScripFO.Rows[n]["SELL"] == DBNull.Value || dtScripFO.Rows[n]["SELL"].ToString() == "" || dtScripFO.Rows[n]["SELL"].ToString() == "0.000000")
                                        SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                    else
                                        SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["SELL"].ToString())) + "</td>";
                                    if (dtScripFO.Rows[n]["SELL_PRICE"] == DBNull.Value || dtScripFO.Rows[n]["SELL_PRICE"].ToString() == "" || dtScripFO.Rows[n]["SELL_PRICE"].ToString() == "0.000000")
                                        SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                    else
                                        //SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["SELL_PRICE"].ToString())) + "</td>";
                                        SubStr = SubStr + "<td align=\"right\">&nbsp;" + getFormattedDecvalue(Convert.ToDecimal(dtScripFO.Rows[n]["SELL_PRICE"].ToString())) + "</td>";




                                    if (dtScripFO.Rows[n]["CF"] == DBNull.Value || dtScripFO.Rows[n]["CF"].ToString() == "" || dtScripFO.Rows[n]["CF"].ToString() == "0.000000")
                                        SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                    else
                                        SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["CF"].ToString())) + "</td>";
                                    if (dtScripFO.Rows[n]["CF_PRICE"] == DBNull.Value || dtScripFO.Rows[n]["CF_PRICE"].ToString() == "" || dtScripFO.Rows[n]["CF_PRICE"].ToString() == "0.000000")
                                        SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                    else
                                        //SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["CF_PRICE"].ToString())) + "</td>";
                                        SubStr = SubStr + "<td align=\"right\">&nbsp;" + getFormattedDecvalue(Convert.ToDecimal(dtScripFO.Rows[n]["CF_PRICE"].ToString())) + "</td>";


                                    SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["NET"].ToString())) + "</td></tr>";
                                    TotOblg = TotOblg + Convert.ToDecimal(dtScripFO.Rows[n]["NET"].ToString());
                                }

                            }

                            SubStr = SubStr + "<tr style=\"background-color:#EEEED1;font-weight:bold;\"><td colspan=\"9\" align=\"right\" style=\"font-weight:bold;\">&nbsp;Net Obligation </td><td align=\"right\">" + oconverter.getFormattedvaluewithoriginalsign(TotOblg) + "</td></tr>";


                            TotChrg = 0;
                            for (int k = 0; k < dtTax.Rows.Count; k++)
                            {
                                if (dtTax.Rows[k]["ACCOUNTSLEDGER_SUBACCOUNTID"].ToString().Trim() == CustomerID && (dtTax.Rows[k]["SETTLEMENT"].ToString().Trim() + " ") == SettlementNOType && dtTax.Rows[k]["CHARGE_DATE"].ToString().Trim() == TransactionDate && dtTax.Rows[k]["SegmentName"].ToString().Trim() == SegmentName.Trim())
                                {
                                    SubStr = SubStr + "<tr><td colspan=\"9\" align=\"right\">&nbsp;" + dtTax.Rows[k]["CHARGE_TYPE1"].ToString() + "</td><td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtTax.Rows[k]["ACCOUNTSLEDGER_AMOUNTDR"].ToString())) + "</td></tr>";
                                    TotChrg = TotChrg + Convert.ToDecimal(dtTax.Rows[k]["ACCOUNTSLEDGER_AMOUNTDR"].ToString());

                                }

                            }
                            TotNet = 0;
                            TotNet = TotOblg + TotChrg;
                            SubStr = SubStr + "<tr style=\"background-color:#EEEED1;font-weight:bold;\"><td colspan=\"9\" align=\"right\" style=\"font-weight:bold;\">&nbsp;Total Charges</td><td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(TotChrg) + "</td></tr>";
                            SubStr = SubStr + "<tr style=\"background-color:#EEEED1;font-weight:bold;\"><td colspan=\"9\" align=\"right\" style=\"font-weight:bold;\">&nbsp;Net Total</td><td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(TotNet) + "</td></tr>";


                            SubStr = SubStr + "</table>";

                        }




                    }

                    strHTML = strHTML + "<tr><td>&nbsp;</td><td>&nbsp;</td><td colspan=\"4\" valign=\"top\">&nbsp;" + SubStr + "</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr>";
                }


                SegmentName = "";
                SegID = "";
                CustomerID = "";
                SettlementNo = "";
                SettlementType = "";
                SettlementNOType = "";
                TransactionDate = "";
                IsConsolidate = "";
                TotOblg = 0;
                TotChrg = 0;
                TotNet = 0;


            }
            if (dtMain.Rows.Count > 0)
            {
                if (dtMain.Rows[0]["ClosingBalanceDr"] == DBNull.Value || dtMain.Rows[0]["ClosingBalanceDr"] == "")
                    dtMain.Rows[0]["ClosingBalanceDr"] = "0";
                if (dtMain.Rows[0]["ClosingBalancecr"] == DBNull.Value || dtMain.Rows[0]["ClosingBalancecr"] == "")
                    dtMain.Rows[0]["ClosingBalancecr"] = "0";
                if (dtMain.Rows[0]["ClosingTotal"] == DBNull.Value || dtMain.Rows[0]["ClosingTotal"] == "")
                    dtMain.Rows[0]["ClosingTotal"] = "0";

                strHTML = strHTML + "<tr style=\"background-color:#EEE0E5;font-weight:bold;\"><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td style=\"font-weight:bold;\">Closing Balance</td><td>&nbsp;</td><td>&nbsp;</td><td align=\"right\"  style=\"font-weight:bold;\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[0]["ClosingBalanceDr"].ToString())) + "</td><td  align=\"right\" style=\"font-weight:bold;\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[0]["ClosingBalanceCr"].ToString())) + "</td><td  align=\"right\" style=\"font-weight:bold;\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[0]["ClosingTotal"].ToString())) + "</td></tr>";
            }
            strHTML = strHTML + "</table>";
            ViewState["EmailHTML"] = strHTML;
            if (EmailInd == "N")
            {
                DisplayOblg.InnerHtml = strHTML;
                DisplayOblg.Visible = true;

            }


            string SpanText1 = oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "  - " + oconverter.ArrangeDate2(dtTo.Value.ToString());
            ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct", "ShowHide('" + SpanText1 + "')", true);
            if (cmbclientsPager.SelectedItem == null)
                ScriptManager.RegisterStartupScript(this, this.GetType(), "JShide", "DisabledDrp('a');", true);
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "JShide", "DisabledDrp('b');", true);


        }

        public void GenerateHTMLConsolidate()
        {

            DataSet dsHTML = (DataSet)ViewState["dsEmail"];
            DataTable dtMain = dsHTML.Tables[2];
            DataTable dtScripCM = dsHTML.Tables[0];
            DataTable dtTax = dsHTML.Tables[1];
            DataTable dtScripFO = dsHTML.Tables[5];
            ViewState["EmailHTML"] = "";
            DataTable dtMail = new DataTable();
            dtMail.Columns.Add("CustomerID");
            dtMail.Columns.Add("BranchID");
            dtMail.Columns.Add("EmailBody");
            dtMail.Columns.Add("Type");

            string SegmentName = "";
            string SegID = "";
            String CustomerID = "";
            string SettlementNo = "";
            string SettlementType = "";
            string SettlementNOType = "";
            string TransactionDate = "";
            string IsConsolidate = "";
            string strHTML = "";
            string SubHTML = "";
            string SubStr = "";
            decimal TotOblg = 0;
            decimal TotChrg = 0;
            decimal TotNet = 0;
            int opInd = 0;
            int clInd = 0;
            strHTML = "<table border=\"1\" width=\"100%\" cellspacing=\"0\" cellpadding=\"0\">";
            strHTML = strHTML + "<tr style=\"background-color:#B7CEEC;font-weight:bold;height:30px\">";
            strHTML = strHTML + "<td style=\"font-weight:bold;\">Tr. Date</td>";
            strHTML = strHTML + "<td style=\"font-weight:bold;\">Value Date</td>";
            strHTML = strHTML + "<td style=\"font-weight:bold;\">Voucher No.</td>";
            strHTML = strHTML + "<td style=\"font-weight:bold;\">Inst. No.</td>";
            strHTML = strHTML + "<td style=\"font-weight:bold;\">Description</td>";
            strHTML = strHTML + "<td style=\"font-weight:bold;\">Trade Date</td>";
            strHTML = strHTML + "<td style=\"font-weight:bold;\">Settlement No.</td>";
            strHTML = strHTML + "<td style=\"font-weight:bold;\"  align=\"right\" >Debit</td>";
            strHTML = strHTML + "<td style=\"font-weight:bold;\"  align=\"right\" >Credit</td>";
            strHTML = strHTML + "<td style=\"font-weight:bold;\"  align=\"right\" >Closing</td>";
            strHTML = strHTML + "</tr>";

            for (int i = 0; i < dtMain.Rows.Count; i++)
            {
                //------------------Variable Declaration--------------
                SegID = dtMain.Rows[i]["SegID"].ToString();
                SegmentName = dtMain.Rows[i]["SegmentName"].ToString();
                SegID = dtMain.Rows[i]["SegID"].ToString();
                CustomerID = dtMain.Rows[i]["SubID"].ToString();
                SettlementNo = dtMain.Rows[i]["SettlementN"].ToString();
                SettlementType = dtMain.Rows[i]["SettlementT"].ToString();
                SettlementNOType = dtMain.Rows[i]["SettlementNumber"].ToString();
                TransactionDate = dtMain.Rows[i]["accountsledger_transactiondate"].ToString();
                if (dtMain.Rows[i]["accountsledger_Narration"].ToString().Length > 20)
                {
                    IsConsolidate = dtMain.Rows[i]["accountsledger_Narration"].ToString().Substring(0, 21);
                }
                else
                {
                    IsConsolidate = dtMain.Rows[i]["accountsledger_Narration"].ToString();
                }
                //------------------End of Variable Declaration--------------


                if (i != dtMain.Rows.Count - 1)
                {
                    if (dtMain.Rows[i]["SubID"].ToString() == dtMain.Rows[i + 1]["SubID"].ToString())
                    {
                        //----Opening Balance Row------ 
                        if (opInd == 0)
                        {
                            if (dtMain.Rows[i]["OpeningBalanceDr"] == DBNull.Value || dtMain.Rows[i]["OpeningBalanceDr"] == "")
                                dtMain.Rows[i]["OpeningBalanceDr"] = "0";
                            if (dtMain.Rows[i]["OpeningBalanceCr"] == DBNull.Value || dtMain.Rows[i]["OpeningBalanceCr"] == "")
                                dtMain.Rows[i]["OpeningBalanceCr"] = "0";
                            if (dtMain.Rows[i]["OpeningTotal"] == DBNull.Value || dtMain.Rows[i]["OpeningTotal"] == "")
                                dtMain.Rows[i]["OpeningTotal"] = "0";
                            if (dtMain.Rows[i]["OpeningBalanceDr"] == DBNull.Value || dtMain.Rows[i]["OpeningBalanceDr"].ToString() == "" || dtMain.Rows[i]["OpeningBalanceDr"].ToString() == "0")
                                SubHTML = SubHTML + "<tr  style=\"background-color:#EEE0E5;font-weight:bold;\"><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td style=\"font-weight:bold;\">Opening Balance</td><td>&nbsp;</td><td>&nbsp;</td><td  align=\"right\" style=\"font-weight:bold;\">&nbsp;</td>";
                            else
                                SubHTML = SubHTML + "<tr  style=\"background-color:#EEE0E5;font-weight:bold;\"><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td style=\"font-weight:bold;\">Opening Balance</td><td>&nbsp;</td><td>&nbsp;</td><td  align=\"right\" style=\"font-weight:bold;\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["OpeningBalanceDr"].ToString())) + "</td>";
                            if (dtMain.Rows[i]["OpeningBalanceCr"] == DBNull.Value || dtMain.Rows[i]["OpeningBalanceCr"].ToString() == "" || dtMain.Rows[i]["OpeningBalanceCr"].ToString() == "0")
                                SubHTML = SubHTML + "<td  align=\"right\" style=\"font-weight:bold;\">&nbsp</td>";
                            else
                                SubHTML = SubHTML + "<td  align=\"right\" style=\"font-weight:bold;\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["OpeningBalanceCr"].ToString())) + "</td>";
                            if (dtMain.Rows[i]["OpeningTotal"] == DBNull.Value || dtMain.Rows[i]["OpeningTotal"].ToString() == "" || dtMain.Rows[i]["OpeningTotal"].ToString() == "0")
                                SubHTML = SubHTML + "<td  align=\"right\" style=\"font-weight:bold;\">&nbsp</td></tr>";
                            else
                                SubHTML = SubHTML + "<td  align=\"right\" style=\"font-weight:bold;\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["OpeningTotal"].ToString())) + "</td></tr>";

                            opInd = 1;
                        }
                        //---- End of Opening Balance Row------ 
                        //-----Transaction Details ---------
                        SubHTML = SubHTML + "<tr>";
                        SubHTML = SubHTML + "<td style=\"width:7%;\" >&nbsp;" + dtMain.Rows[i][1].ToString() + "</td>";
                        SubHTML = SubHTML + "<td style=\"width:7%;\">&nbsp;" + dtMain.Rows[i][2].ToString() + "</td>";
                        SubHTML = SubHTML + "<td>&nbsp;" + dtMain.Rows[i]["accountsledger_TransactionReferenceID"].ToString() + "</td>";
                        SubHTML = SubHTML + "<td>&nbsp;" + dtMain.Rows[i][10].ToString() + "</td>";
                        SubHTML = SubHTML + "<td>&nbsp;" + dtMain.Rows[i][4].ToString() + "| [" + SegmentName + "]</td>";
                        if (SegmentName == "NSE-CM" || SegmentName == "BSE-CM" || SegmentName == "CSE-CM")
                        {
                            SubHTML = SubHTML + "<td>&nbsp;" + dtMain.Rows[i]["PayoutDate"].ToString() + "</td>";
                        }
                        else
                        {
                            SubHTML = SubHTML + "<td>&nbsp;" + dtMain.Rows[i]["TrDate"].ToString() + "</td>";
                        }
                        SubHTML = SubHTML + "<td>&nbsp;" + dtMain.Rows[i][11].ToString() + "</td>";


                        if (dtMain.Rows[i]["Accountsledger_AmountCr"] == DBNull.Value || dtMain.Rows[i]["Accountsledger_AmountCr"].ToString() == "" || dtMain.Rows[i]["Accountsledger_AmountCr"].ToString() == "0")
                            SubHTML = SubHTML + "<td align=\"right\">&nbsp;</td>";
                        else
                            SubHTML = SubHTML + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["Accountsledger_AmountCr"].ToString())) + "</td>";

                        if (dtMain.Rows[i]["Accountsledger_AmountDr"] == DBNull.Value || dtMain.Rows[i]["Accountsledger_AmountDr"].ToString() == "" || dtMain.Rows[i]["Accountsledger_AmountDr"].ToString() == "0")
                            SubHTML = SubHTML + "<td align=\"right\">&nbsp;</td>";
                        else
                            SubHTML = SubHTML + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["Accountsledger_AmountDr"].ToString())) + "</td>";

                        if (dtMain.Rows[i]["Closing"] == DBNull.Value || dtMain.Rows[i]["Closing"].ToString() == "" || dtMain.Rows[i]["Closing"].ToString() == "0")
                        {
                            SubHTML = SubHTML + "<td align=\"right\">&nbsp;</td>";
                        }
                        else
                        {
                            if (dtMain.Rows[i]["Closing"].ToString().Substring(0, 1) == "-")
                                SubHTML = SubHTML + "<td align=\"right\" style=\"color:red\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["Closing"].ToString())) + "</td>";
                            else
                                SubHTML = SubHTML + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["Closing"].ToString())) + "</td>";
                        }

                        SubHTML = SubHTML + "</tr>";
                        //-----End Of Transaction Details ---------

                        //-----Obligation Breakup-----------------
                        SubStr = "";
                        if (IsConsolidate == "Settlement Obligation")
                        {

                            if (SegmentName == "NSE-CM" || SegmentName == "BSE-CM" || SegmentName == "CSE-CM")
                            {
                                if (dtScripCM.Rows.Count > 0)
                                {
                                    TotOblg = 0;
                                    SubStr = "<table border=\"1\" width=\"100%\">";
                                    SubStr = SubStr + "<tr style=\"background-color:#EEE0E5;font-weight:bold;\"><td style=\"font-weight:bold;\" colspan=\"5\">Obligation Breakup For Settlement No. " + SettlementNOType + "</td></tr>";
                                    SubStr = SubStr + "<tr style=\"background-color:#EEE0E5;font-weight:bold;\"><td style=\"font-weight:bold;\">Scrip</td><td style=\"font-weight:bold;\">Type</td><td style=\"font-weight:bold;\" align=\"right\">Quantity</td><td align=\"right\" style=\"font-weight:bold;\">Average</td><td align=\"right\" style=\"font-weight:bold;\">Amount</td></tr>";
                                    for (int j = 0; j < dtScripCM.Rows.Count; j++)
                                    {

                                        if (dtScripCM.Rows[j]["CMPOSITION_CUSTOMERID"].ToString().Trim() == CustomerID && (dtScripCM.Rows[j]["SETTLEMENT"].ToString().Trim() + " " == SettlementNOType))
                                        {

                                            SubStr = SubStr + "<tr><td>&nbsp;" + dtScripCM.Rows[j]["PRODUCTSERIESID"].ToString() + "</td><td>&nbsp;" + dtScripCM.Rows[j]["DELIVERYTYPE"].ToString() + "</td>";
                                            if (oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripCM.Rows[j]["QUANTITY"].ToString())) == "0.00")
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;</td><td align=\"right\">&nbsp;</td>";
                                            else
                                            {
                                                if (Convert.ToString(dtScripCM.Rows[j]["DELIVERYTYPE"]) != "DIFF")
                                                    SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripCM.Rows[j]["QUANTITY"].ToString())) + "</td><td align=\"right\">&nbsp;" + getFormattedDecvalue(Convert.ToDecimal(getFormattedDecvalue(Convert.ToDecimal(dtScripCM.Rows[j]["AMOUNT"].ToString()))) / Convert.ToDecimal(getFormattedDecvalue(Convert.ToDecimal(dtScripCM.Rows[j]["QUANTITY"].ToString())))) + "</td>";
                                                else
                                                    SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripCM.Rows[j]["QUANTITY"].ToString())) + "</td><td align=\"right\">&nbsp;</td>";
                                            }
                                            if (oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripCM.Rows[j]["AMOUNT"].ToString())) == "0.00")
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;</td></tr>";
                                            else
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripCM.Rows[j]["AMOUNT"].ToString())) + "</td></tr>";


                                            TotOblg = TotOblg + Convert.ToDecimal(dtScripCM.Rows[j]["AMOUNT"].ToString());
                                        }

                                    }
                                    SubStr = SubStr + "<tr style=\"background-color:#EEEED1;font-weight:bold;\"><td colspan=\"3\" align=\"right\" style=\"font-weight:bold;\">&nbsp;Net Obligation</td><td colspan='2'  align=\"right\" >&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(TotOblg) + "</td></tr>";
                                    TotChrg = 0;
                                    for (int k = 0; k < dtTax.Rows.Count; k++)
                                    {
                                        if (dtTax.Rows[k]["ACCOUNTSLEDGER_SUBACCOUNTID"].ToString().Trim() == CustomerID && (dtTax.Rows[k]["SETTLEMENT"].ToString().Trim() + " ") == SettlementNOType)
                                        {
                                            SubStr = SubStr + "<tr><td colspan=\"3\" align=\"right\">&nbsp;" + dtTax.Rows[k]["CHARGE_TYPE1"].ToString() + "</td><td colspan='2' align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtTax.Rows[k]["ACCOUNTSLEDGER_AMOUNTDR"].ToString())) + "</td></tr>";
                                            TotChrg = TotChrg + Convert.ToDecimal(dtTax.Rows[k]["ACCOUNTSLEDGER_AMOUNTDR"].ToString());

                                        }

                                    }
                                    TotNet = 0;
                                    TotNet = TotOblg + TotChrg;
                                    SubStr = SubStr + "<tr style=\"background-color:#EEEED1;font-weight:bold;\"><td colspan=\"3\" align=\"right\" style=\"font-weight:bold;\">&nbsp;Total Charges</td><td colspan='2' align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(TotChrg) + "</td></tr>";
                                    SubStr = SubStr + "<tr style=\"background-color:#EEEED1;font-weight:bold;\"><td  colspan=\"3\" align=\"right\" style=\"font-weight:bold;\">&nbsp;Net Total</td><td colspan='2' align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(TotNet) + "</td></tr>";


                                    SubStr = SubStr + "</table>";

                                }

                            }
                            if (SegmentName == "NSE-FO" || SegmentName == "BSE-FO")
                            {

                                if (dtScripFO.Rows.Count > 0)
                                {
                                    SubStr = "<table border=\"1\" width=\"100%\">";
                                    SubStr = SubStr + "<tr style=\"background-color:#EEE0E5;font-weight:bold;\"><td style=\"font-weight:bold;\" colspan=\"10\">Obligation Break Up For " + TransactionDate + "</td></tr>";
                                    SubStr = SubStr + "<tr style=\"background-color:#EEE0E5;font-weight:bold;\"><td style=\"font-weight:bold;\">SCRIP</td><td align=\"right\" style=\"font-weight:bold;\">BF</td><td align=\"right\" style=\"font-weight:bold;\">BF PRICE</td><td align=\"right\" style=\"font-weight:bold;\">BUY</td><td align=\"right\" style=\"font-weight:bold;\">BUY PRICE</td><td align=\"right\" style=\"font-weight:bold;\">SELL</td><td align=\"right\" style=\"font-weight:bold;\">SELL PRICE</td><td align=\"right\" style=\"font-weight:bold;\">CF</td><td align=\"right\" style=\"font-weight:bold;\">CF PRICE</td><td align=\"right\" style=\"font-weight:bold;\">NET</td></tr>";
                                    for (int n = 0; n < dtScripFO.Rows.Count; n++)
                                    {
                                        if (dtScripFO.Rows[n]["FOPosition_CustomerExchangeID"].ToString() == CustomerID && dtScripFO.Rows[n]["SETTLEMENT"].ToString() == SettlementNOType && dtScripFO.Rows[n]["DATE_FOR"].ToString() == TransactionDate)
                                        {
                                            SubStr = SubStr + "<tr><td>&nbsp;" + dtScripFO.Rows[n]["SCRIP"].ToString() + "</td>";
                                            if (dtScripFO.Rows[n]["BF"] == DBNull.Value || dtScripFO.Rows[n]["BF"].ToString() == "" || dtScripFO.Rows[n]["BF"].ToString() == "0.000000")
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                            else
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["BF"].ToString())) + "</td>";

                                            if (dtScripFO.Rows[n]["BF_PRICE"] == DBNull.Value || dtScripFO.Rows[n]["BF_PRICE"].ToString() == "" || dtScripFO.Rows[n]["BF_PRICE"].ToString() == "0.000000")
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                            else
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["BF_PRICE"].ToString())) + "</td>";



                                            if (dtScripFO.Rows[n]["BUY"] == DBNull.Value || dtScripFO.Rows[n]["BUY"].ToString() == "" || dtScripFO.Rows[n]["BUY"].ToString() == "0.000000")
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                            else
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["BUY"].ToString())) + "</td>";
                                            if (dtScripFO.Rows[n]["BUY_PRICE"] == DBNull.Value || dtScripFO.Rows[n]["BUY_PRICE"].ToString() == "" || dtScripFO.Rows[n]["BUY_PRICE"].ToString() == "0.000000")
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                            else
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["BUY_PRICE"].ToString())) + "</td>";



                                            if (dtScripFO.Rows[n]["SELL"] == DBNull.Value || dtScripFO.Rows[n]["SELL"].ToString() == "" || dtScripFO.Rows[n]["SELL"].ToString() == "0.000000")
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                            else
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["SELL"].ToString())) + "</td>";
                                            if (dtScripFO.Rows[n]["SELL_PRICE"] == DBNull.Value || dtScripFO.Rows[n]["SELL_PRICE"].ToString() == "" || dtScripFO.Rows[n]["SELL_PRICE"].ToString() == "0.000000")
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                            else
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["SELL_PRICE"].ToString())) + "</td>";




                                            if (dtScripFO.Rows[n]["CF"] == DBNull.Value || dtScripFO.Rows[n]["CF"].ToString() == "" || dtScripFO.Rows[n]["CF"].ToString() == "0.000000")
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                            else
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["CF"].ToString())) + "</td>";
                                            if (dtScripFO.Rows[n]["CF_PRICE"] == DBNull.Value || dtScripFO.Rows[n]["CF_PRICE"].ToString() == "" || dtScripFO.Rows[n]["CF_PRICE"].ToString() == "0.000000")
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                            else
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["CF_PRICE"].ToString())) + "</td>";


                                            SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["NET"].ToString())) + "</td></tr>";
                                            TotOblg = TotOblg + Convert.ToDecimal(dtScripFO.Rows[n]["NET"].ToString());
                                        }

                                    }

                                    SubStr = SubStr + "<tr style=\"background-color:#EEEED1;font-weight:bold;\"><td colspan=\"9\" align=\"right\" style=\"font-weight:bold;\">&nbsp;Net Obligation </td><td align=\"right\">" + oconverter.getFormattedvaluewithoriginalsign(TotOblg) + "</td></tr>";


                                    TotChrg = 0;
                                    for (int k = 0; k < dtTax.Rows.Count; k++)
                                    {
                                        if (dtTax.Rows[k]["ACCOUNTSLEDGER_SUBACCOUNTID"].ToString().Trim() == CustomerID && (dtTax.Rows[k]["SETTLEMENT"].ToString().Trim() + " ") == SettlementNOType && dtTax.Rows[k]["CHARGE_DATE"].ToString().Trim() == TransactionDate)
                                        {
                                            SubStr = SubStr + "<tr><td colspan=\"9\" align=\"right\">&nbsp;" + dtTax.Rows[k]["CHARGE_TYPE1"].ToString() + "</td><td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtTax.Rows[k]["ACCOUNTSLEDGER_AMOUNTDR"].ToString())) + "</td></tr>";
                                            TotChrg = TotChrg + Convert.ToDecimal(dtTax.Rows[k]["ACCOUNTSLEDGER_AMOUNTDR"].ToString());

                                        }

                                    }
                                    TotNet = 0;
                                    TotNet = TotOblg + TotChrg;
                                    SubStr = SubStr + "<tr style=\"background-color:#EEEED1;font-weight:bold;\"><td colspan=\"9\" align=\"right\" style=\"font-weight:bold;\">&nbsp;Total Charges</td><td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(TotChrg) + "</td></tr>";
                                    SubStr = SubStr + "<tr style=\"background-color:#EEEED1;font-weight:bold;\"><td colspan=\"9\" align=\"right\" style=\"font-weight:bold;\">&nbsp;Net Total</td><td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(TotNet) + "</td></tr>";


                                    SubStr = SubStr + "</table>";

                                }
                            }
                            else
                            {


                                if (dtScripFO.Rows.Count > 0)
                                {
                                    string test = dtMain.Rows[i]["SegID"].ToString();
                                    SubStr = "<table border=\"1\" width=\"100%\">";
                                    SubStr = SubStr + "<tr style=\"background-color:#EEE0E5;font-weight:bold;\"><td style=\"font-weight:bold;\" colspan=\"10\">Obligation Break Up For " + TransactionDate + "</td></tr>";
                                    SubStr = SubStr + "<tr style=\"background-color:#EEE0E5;font-weight:bold;\"><td style=\"font-weight:bold;\">SCRIP</td><td align=\"right\" style=\"font-weight:bold;\">BF</td><td align=\"right\" style=\"font-weight:bold;\">BF PRICE</td><td align=\"right\" style=\"font-weight:bold;\">BUY</td><td align=\"right\" style=\"font-weight:bold;\">BUY PRICE</td><td align=\"right\" style=\"font-weight:bold;\">SELL</td><td align=\"right\" style=\"font-weight:bold;\">SELL PRICE</td><td align=\"right\" style=\"font-weight:bold;\">CF</td><td align=\"right\" style=\"font-weight:bold;\">CF PRICE</td><td align=\"right\" style=\"font-weight:bold;\">NET</td></tr>";
                                    for (int n = 0; n < dtScripFO.Rows.Count; n++)
                                    {
                                        if (dtScripFO.Rows[n]["FOPosition_CustomerExchangeID"].ToString() == CustomerID && dtScripFO.Rows[n]["DATE_FOR"].ToString() == TransactionDate && dtScripFO.Rows[n]["segid"].ToString() == test.Trim())
                                        {

                                            SubStr = SubStr + "<tr><td>&nbsp;" + dtScripFO.Rows[n]["SCRIP"].ToString() + "</td>";


                                            if (dtScripFO.Rows[n]["BF"] == DBNull.Value || dtScripFO.Rows[n]["BF"].ToString() == "" || dtScripFO.Rows[n]["BF"].ToString() == "0.000000")
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                            else
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["BF"].ToString())) + "</td>";

                                            if (dtScripFO.Rows[n]["BF_PRICE"] == DBNull.Value || dtScripFO.Rows[n]["BF_PRICE"].ToString() == "" || dtScripFO.Rows[n]["BF_PRICE"].ToString() == "0.000000")
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                            else
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["BF_PRICE"].ToString())) + "</td>";



                                            if (dtScripFO.Rows[n]["BUY"] == DBNull.Value || dtScripFO.Rows[n]["BUY"].ToString() == "" || dtScripFO.Rows[n]["BUY"].ToString() == "0.000000")
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                            else
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["BUY"].ToString())) + "</td>";
                                            if (dtScripFO.Rows[n]["BUY_PRICE"] == DBNull.Value || dtScripFO.Rows[n]["BUY_PRICE"].ToString() == "" || dtScripFO.Rows[n]["BUY_PRICE"].ToString() == "0.000000")
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                            else
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["BUY_PRICE"].ToString())) + "</td>";



                                            if (dtScripFO.Rows[n]["SELL"] == DBNull.Value || dtScripFO.Rows[n]["SELL"].ToString() == "" || dtScripFO.Rows[n]["SELL"].ToString() == "0.000000")
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                            else
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["SELL"].ToString())) + "</td>";
                                            if (dtScripFO.Rows[n]["SELL_PRICE"] == DBNull.Value || dtScripFO.Rows[n]["SELL_PRICE"].ToString() == "" || dtScripFO.Rows[n]["SELL_PRICE"].ToString() == "0.000000")
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                            else
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["SELL_PRICE"].ToString())) + "</td>";




                                            if (dtScripFO.Rows[n]["CF"] == DBNull.Value || dtScripFO.Rows[n]["CF"].ToString() == "" || dtScripFO.Rows[n]["CF"].ToString() == "0.000000")
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                            else
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["CF"].ToString())) + "</td>";
                                            if (dtScripFO.Rows[n]["CF_PRICE"] == DBNull.Value || dtScripFO.Rows[n]["CF_PRICE"].ToString() == "" || dtScripFO.Rows[n]["CF_PRICE"].ToString() == "0.000000")
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                            else
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["CF_PRICE"].ToString())) + "</td>";


                                            SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["NET"].ToString())) + "</td></tr>";
                                            TotOblg = TotOblg + Convert.ToDecimal(dtScripFO.Rows[n]["NET"].ToString());
                                        }

                                    }

                                    SubStr = SubStr + "<tr style=\"background-color:#EEEED1;font-weight:bold;\"><td colspan=\"9\" align=\"right\" style=\"font-weight:bold;\">&nbsp;Net Obligation </td><td align=\"right\">" + oconverter.getFormattedvaluewithoriginalsign(TotOblg) + "</td></tr>";


                                    TotChrg = 0;
                                    for (int k = 0; k < dtTax.Rows.Count; k++)
                                    {
                                        if (dtTax.Rows[k]["ACCOUNTSLEDGER_SUBACCOUNTID"].ToString().Trim() == CustomerID && (dtTax.Rows[k]["SETTLEMENT"].ToString().Trim() + " ") == SettlementNOType && dtTax.Rows[k]["CHARGE_DATE"].ToString().Trim() == TransactionDate && dtTax.Rows[k]["SegmentName"].ToString().Trim() == SegmentName.Trim())
                                        {
                                            SubStr = SubStr + "<tr><td colspan=\"9\" align=\"right\">&nbsp;" + dtTax.Rows[k]["CHARGE_TYPE1"].ToString() + "</td><td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtTax.Rows[k]["ACCOUNTSLEDGER_AMOUNTDR"].ToString())) + "</td></tr>";
                                            TotChrg = TotChrg + Convert.ToDecimal(dtTax.Rows[k]["ACCOUNTSLEDGER_AMOUNTDR"].ToString());

                                        }

                                    }
                                    TotNet = 0;
                                    TotNet = TotOblg + TotChrg;
                                    SubStr = SubStr + "<tr style=\"background-color:#EEEED1;font-weight:bold;\"><td colspan=\"9\" align=\"right\" style=\"font-weight:bold;\">&nbsp;Total Charges</td><td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(TotChrg) + "</td></tr>";
                                    SubStr = SubStr + "<tr style=\"background-color:#EEEED1;font-weight:bold;\"><td colspan=\"9\" align=\"right\" style=\"font-weight:bold;\">&nbsp;Net Total</td><td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(TotNet) + "</td></tr>";


                                    SubStr = SubStr + "</table>";

                                }




                            }

                            SubHTML = SubHTML + "<tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td colspan=\"4\" valign=\"top\">&nbsp;" + SubStr + "</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr>";
                        }



                        //-----End  of Obligation Breakup-----------------


                    }
                    else
                    {

                        //-----Transaction Details ---------
                        SubHTML = SubHTML + "<tr>";
                        SubHTML = SubHTML + "<td >&nbsp;" + dtMain.Rows[i][1].ToString() + "</td>";
                        SubHTML = SubHTML + "<td>&nbsp;" + dtMain.Rows[i][2].ToString() + "</td>";
                        SubHTML = SubHTML + "<td>&nbsp;" + dtMain.Rows[i]["accountsledger_TransactionReferenceID"].ToString() + "</td>";
                        SubHTML = SubHTML + "<td>&nbsp;" + dtMain.Rows[i][10].ToString() + "</td>";
                        SubHTML = SubHTML + "<td>&nbsp;" + dtMain.Rows[i][4].ToString() + "| [" + SegmentName + "]</td>";
                        if (SegmentName == "NSE-CM" || SegmentName == "BSE-CM" || SegmentName == "CSE-CM")
                        {
                            SubHTML = SubHTML + "<td>&nbsp;" + dtMain.Rows[i]["PayoutDate"].ToString() + "</td>";
                        }
                        else
                        {
                            SubHTML = SubHTML + "<td>&nbsp;" + dtMain.Rows[i]["TrDate"].ToString() + "</td>";
                        }
                        SubHTML = SubHTML + "<td>&nbsp;" + dtMain.Rows[i][11].ToString() + "</td>";


                        if (dtMain.Rows[i]["Accountsledger_AmountCr"] == DBNull.Value || dtMain.Rows[i]["Accountsledger_AmountCr"].ToString() == "" || dtMain.Rows[i]["Accountsledger_AmountCr"].ToString() == "0")
                            SubHTML = SubHTML + "<td align=\"right\">&nbsp;</td>";
                        else
                            SubHTML = SubHTML + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["Accountsledger_AmountCr"].ToString())) + "</td>";

                        if (dtMain.Rows[i]["Accountsledger_AmountDr"] == DBNull.Value || dtMain.Rows[i]["Accountsledger_AmountDr"].ToString() == "" || dtMain.Rows[i]["Accountsledger_AmountDr"].ToString() == "0")
                            SubHTML = SubHTML + "<td align=\"right\">&nbsp;</td>";
                        else
                            SubHTML = SubHTML + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["Accountsledger_AmountDr"].ToString())) + "</td>";

                        if (dtMain.Rows[i]["Closing"] == DBNull.Value || dtMain.Rows[i]["Closing"].ToString() == "" || dtMain.Rows[i]["Closing"].ToString() == "0")
                        {
                            SubHTML = SubHTML + "<td align=\"right\">&nbsp;</td>";
                        }
                        else
                        {
                            if (dtMain.Rows[i]["Closing"].ToString().Substring(0, 1) == "-")
                                SubHTML = SubHTML + "<td align=\"right\" style=\"color:red\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["Closing"].ToString())) + "</td>";
                            else
                                SubHTML = SubHTML + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["Closing"].ToString())) + "</td>";
                        }

                        SubHTML = SubHTML + "</tr>";
                        //-----End Of Transaction Details ---------

                        //-----Obligation Breakup-----------------
                        SubStr = "";
                        if (IsConsolidate == "Settlement Obligation")
                        {

                            if (SegmentName == "NSE-CM" || SegmentName == "BSE-CM" || SegmentName == "CSE-CM")
                            {
                                if (dtScripCM.Rows.Count > 0)
                                {
                                    TotOblg = 0;
                                    SubStr = "<table border=\"1\" width=\"100%\">";
                                    SubStr = SubStr + "<tr style=\"background-color:#EEE0E5;font-weight:bold;\"><td style=\"font-weight:bold;\" colspan=\"4\">Obligation Breakup For Settlement No. " + SettlementNOType + "</td></tr>";
                                    SubStr = SubStr + "<tr style=\"background-color:#EEE0E5;font-weight:bold;\"><td style=\"font-weight:bold;\">Scrip</td><td style=\"font-weight:bold;\">Type</td><td style=\"font-weight:bold;\" align=\"right\">Quantity</td><td align=\"right\" style=\"font-weight:bold;\">Amount</td></tr>";
                                    for (int j = 0; j < dtScripCM.Rows.Count; j++)
                                    {

                                        if (dtScripCM.Rows[j]["CMPOSITION_CUSTOMERID"].ToString().Trim() == CustomerID && (dtScripCM.Rows[j]["SETTLEMENT"].ToString().Trim() + " " == SettlementNOType))
                                        {

                                            SubStr = SubStr + "<tr><td>&nbsp;" + dtScripCM.Rows[j]["PRODUCTSERIESID"].ToString() + "</td><td>&nbsp;" + dtScripCM.Rows[j]["DELIVERYTYPE"].ToString() + "</td>";
                                            if (oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripCM.Rows[j]["QUANTITY"].ToString())) == "0.00")
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                            else
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripCM.Rows[j]["QUANTITY"].ToString())) + "</td>";

                                            if (oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripCM.Rows[j]["AMOUNT"].ToString())) == "0.00")
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;</td></tr>";
                                            else
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripCM.Rows[j]["AMOUNT"].ToString())) + "</td></tr>";


                                            TotOblg = TotOblg + Convert.ToDecimal(dtScripCM.Rows[j]["AMOUNT"].ToString());
                                        }

                                    }
                                    SubStr = SubStr + "<tr style=\"background-color:#EEEED1;font-weight:bold;\"><td colspan=\"3\" align=\"right\" style=\"font-weight:bold;\">&nbsp;Net Obligation</td><td  align=\"right\" >&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(TotOblg) + "</td></tr>";
                                    TotChrg = 0;
                                    for (int k = 0; k < dtTax.Rows.Count; k++)
                                    {
                                        if (dtTax.Rows[k]["ACCOUNTSLEDGER_SUBACCOUNTID"].ToString().Trim() == CustomerID && (dtTax.Rows[k]["SETTLEMENT"].ToString().Trim() + " ") == SettlementNOType)
                                        {
                                            SubStr = SubStr + "<tr><td colspan=\"3\" align=\"right\">&nbsp;" + dtTax.Rows[k]["CHARGE_TYPE1"].ToString() + "</td><td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtTax.Rows[k]["ACCOUNTSLEDGER_AMOUNTDR"].ToString())) + "</td></tr>";
                                            TotChrg = TotChrg + Convert.ToDecimal(dtTax.Rows[k]["ACCOUNTSLEDGER_AMOUNTDR"].ToString());

                                        }

                                    }
                                    TotNet = 0;
                                    TotNet = TotOblg + TotChrg;
                                    SubStr = SubStr + "<tr style=\"background-color:#EEEED1;font-weight:bold;\"><td colspan=\"3\" align=\"right\" style=\"font-weight:bold;\">&nbsp;Total Charges</td><td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(TotChrg) + "</td></tr>";
                                    SubStr = SubStr + "<tr style=\"background-color:#EEEED1;font-weight:bold;\"><td  colspan=\"3\" align=\"right\" style=\"font-weight:bold;\">&nbsp;Net Total</td><td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(TotNet) + "</td></tr>";


                                    SubStr = SubStr + "</table>";

                                }

                            }
                            else
                            {

                                if (dtScripFO.Rows.Count > 0)
                                {
                                    SubStr = "<table border=\"1\" width=\"100%\">";
                                    SubStr = SubStr + "<tr style=\"background-color:#EEE0E5;font-weight:bold;\"><td style=\"font-weight:bold;\" colspan=\"10\">Obligation Break Up For " + TransactionDate + "</td></tr>";
                                    SubStr = SubStr + "<tr style=\"background-color:#EEE0E5;font-weight:bold;\"><td style=\"font-weight:bold;\">SCRIP</td><td align=\"right\" style=\"font-weight:bold;\">BF</td><td align=\"right\" style=\"font-weight:bold;\">BF PRICE</td><td align=\"right\" style=\"font-weight:bold;\">BUY</td><td align=\"right\" style=\"font-weight:bold;\">BUY PRICE</td><td align=\"right\" style=\"font-weight:bold;\">SELL</td><td align=\"right\" style=\"font-weight:bold;\">SELL PRICE</td><td align=\"right\" style=\"font-weight:bold;\">CF</td><td align=\"right\" style=\"font-weight:bold;\">CF PRICE</td><td align=\"right\" style=\"font-weight:bold;\">NET</td></tr>";
                                    for (int n = 0; n < dtScripFO.Rows.Count; n++)
                                    {
                                        if (dtScripFO.Rows[n]["FOPosition_CustomerExchangeID"].ToString() == CustomerID && dtScripFO.Rows[n]["SETTLEMENT"].ToString() == SettlementNOType && dtScripFO.Rows[n]["DATE_FOR"].ToString() == TransactionDate)
                                        {
                                            SubStr = SubStr + "<tr><td>&nbsp;" + dtScripFO.Rows[n]["SCRIP"].ToString() + "</td>";
                                            if (dtScripFO.Rows[n]["BF"] == DBNull.Value || dtScripFO.Rows[n]["BF"].ToString() == "" || dtScripFO.Rows[n]["BF"].ToString() == "0.000000")
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                            else
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["BF"].ToString())) + "</td>";

                                            if (dtScripFO.Rows[n]["BF_PRICE"] == DBNull.Value || dtScripFO.Rows[n]["BF_PRICE"].ToString() == "" || dtScripFO.Rows[n]["BF_PRICE"].ToString() == "0.000000")
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                            else
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["BF_PRICE"].ToString())) + "</td>";



                                            if (dtScripFO.Rows[n]["BUY"] == DBNull.Value || dtScripFO.Rows[n]["BUY"].ToString() == "" || dtScripFO.Rows[n]["BUY"].ToString() == "0.000000")
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                            else
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["BUY"].ToString())) + "</td>";
                                            if (dtScripFO.Rows[n]["BUY_PRICE"] == DBNull.Value || dtScripFO.Rows[n]["BUY_PRICE"].ToString() == "" || dtScripFO.Rows[n]["BUY_PRICE"].ToString() == "0.000000")
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                            else
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["BUY_PRICE"].ToString())) + "</td>";



                                            if (dtScripFO.Rows[n]["SELL"] == DBNull.Value || dtScripFO.Rows[n]["SELL"].ToString() == "" || dtScripFO.Rows[n]["SELL"].ToString() == "0.000000")
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                            else
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["SELL"].ToString())) + "</td>";
                                            if (dtScripFO.Rows[n]["SELL_PRICE"] == DBNull.Value || dtScripFO.Rows[n]["SELL_PRICE"].ToString() == "" || dtScripFO.Rows[n]["SELL_PRICE"].ToString() == "0.000000")
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                            else
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["SELL_PRICE"].ToString())) + "</td>";




                                            if (dtScripFO.Rows[n]["CF"] == DBNull.Value || dtScripFO.Rows[n]["CF"].ToString() == "" || dtScripFO.Rows[n]["CF"].ToString() == "0.000000")
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                            else
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["CF"].ToString())) + "</td>";
                                            if (dtScripFO.Rows[n]["CF_PRICE"] == DBNull.Value || dtScripFO.Rows[n]["CF_PRICE"].ToString() == "" || dtScripFO.Rows[n]["CF_PRICE"].ToString() == "0.000000")
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                            else
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["CF_PRICE"].ToString())) + "</td>";


                                            SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["NET"].ToString())) + "</td></tr>";
                                            TotOblg = TotOblg + Convert.ToDecimal(dtScripFO.Rows[n]["NET"].ToString());
                                        }

                                    }

                                    SubStr = SubStr + "<tr style=\"background-color:#EEEED1;font-weight:bold;\"><td colspan=\"9\" align=\"right\" style=\"font-weight:bold;\">&nbsp;Net Obligation </td><td align=\"right\">" + oconverter.getFormattedvaluewithoriginalsign(TotOblg) + "</td></tr>";


                                    TotChrg = 0;
                                    for (int k = 0; k < dtTax.Rows.Count; k++)
                                    {
                                        if (dtTax.Rows[k]["ACCOUNTSLEDGER_SUBACCOUNTID"].ToString().Trim() == CustomerID && (dtTax.Rows[k]["SETTLEMENT"].ToString().Trim() + " ") == SettlementNOType && dtTax.Rows[k]["CHARGE_DATE"].ToString().Trim() == TransactionDate)
                                        {
                                            SubStr = SubStr + "<tr><td colspan=\"9\" align=\"right\">&nbsp;" + dtTax.Rows[k]["CHARGE_TYPE1"].ToString() + "</td><td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtTax.Rows[k]["ACCOUNTSLEDGER_AMOUNTDR"].ToString())) + "</td></tr>";
                                            TotChrg = TotChrg + Convert.ToDecimal(dtTax.Rows[k]["ACCOUNTSLEDGER_AMOUNTDR"].ToString());

                                        }

                                    }
                                    TotNet = 0;
                                    TotNet = TotOblg + TotChrg;
                                    SubStr = SubStr + "<tr style=\"background-color:#EEEED1;font-weight:bold;\"><td colspan=\"9\" align=\"right\" style=\"font-weight:bold;\">&nbsp;Total Charges</td><td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(TotChrg) + "</td></tr>";
                                    SubStr = SubStr + "<tr style=\"background-color:#EEEED1;font-weight:bold;\"><td colspan=\"9\" align=\"right\" style=\"font-weight:bold;\">&nbsp;Net Total</td><td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(TotNet) + "</td></tr>";


                                    SubStr = SubStr + "</table>";

                                }
                            }
                            SubHTML = SubHTML + "<tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td colspan=\"4\" valign=\"top\">&nbsp;" + SubStr + "</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr>";
                        }



                        //-----End  of Obligation Breakup-----------------

                        //-----Closing Balance Row------ 
                        if (clInd == 0)
                        {
                            if (dtMain.Rows[i]["ClosingBalanceDr"] == DBNull.Value || dtMain.Rows[i]["ClosingBalanceDr"] == "")
                                dtMain.Rows[i]["ClosingBalanceDr"] = "0";
                            if (dtMain.Rows[i]["ClosingBalancecr"] == DBNull.Value || dtMain.Rows[i]["ClosingBalancecr"] == "")
                                dtMain.Rows[i]["ClosingBalancecr"] = "0";
                            if (dtMain.Rows[i]["ClosingTotal"] == DBNull.Value || dtMain.Rows[i]["ClosingTotal"] == "")
                                dtMain.Rows[i]["ClosingTotal"] = "0";
                            SubHTML = SubHTML + "<tr style=\"background-color:#EEE0E5;font-weight:bold;\"><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td style=\"font-weight:bold;\">Closing Balance</td><td>&nbsp;</td><td>&nbsp;</td><td align=\"right\"  style=\"font-weight:bold;\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["ClosingBalanceDr"].ToString())) + "</td><td  align=\"right\" style=\"font-weight:bold;\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["ClosingBalanceCr"].ToString())) + "</td><td  align=\"right\" style=\"font-weight:bold;\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["ClosingTotal"].ToString())) + "</td></tr>";
                            clInd = 0;

                        }
                        //-----End of Closing Balance Row------    

                        DataRow Row1 = dtMail.NewRow();
                        Row1[0] = dtMain.Rows[i]["SubID"].ToString();
                        Row1[1] = dtMain.Rows[i]["BranchCode"].ToString();
                        Row1[2] = strHTML + " " + SubHTML + "</table>";
                        Row1[3] = dtMain.Rows[i]["SubLedgerType"].ToString();
                        dtMail.Rows.Add(Row1);
                        SubHTML = "";
                        clInd = 0;
                        opInd = 0;
                    }
                }
                else
                {
                    if (dtMain.Rows.Count > 1)
                    {
                        if (dtMain.Rows[i]["SubID"].ToString() != dtMain.Rows[i - 1]["SubID"].ToString())
                        {

                            //-----Transaction Details ---------
                            SubHTML = SubHTML + "<tr>";
                            SubHTML = SubHTML + "<td >&nbsp;" + dtMain.Rows[i][1].ToString() + "</td>";
                            SubHTML = SubHTML + "<td>&nbsp;" + dtMain.Rows[i][2].ToString() + "</td>";
                            SubHTML = SubHTML + "<td>&nbsp;" + dtMain.Rows[i]["accountsledger_TransactionReferenceID"].ToString() + "</td>";
                            SubHTML = SubHTML + "<td>&nbsp;" + dtMain.Rows[i][10].ToString() + "</td>";
                            SubHTML = SubHTML + "<td>&nbsp;" + dtMain.Rows[i][4].ToString() + "| [" + SegmentName + "]</td>";
                            if (SegmentName == "NSE-CM" || SegmentName == "BSE-CM" || SegmentName == "CSE-CM")
                            {
                                SubHTML = SubHTML + "<td>&nbsp;" + dtMain.Rows[i]["PayoutDate"].ToString() + "</td>";
                            }
                            else
                            {
                                SubHTML = SubHTML + "<td>&nbsp;" + dtMain.Rows[i]["TrDate"].ToString() + "</td>";
                            }
                            SubHTML = SubHTML + "<td>&nbsp;" + dtMain.Rows[i][11].ToString() + "</td>";


                            if (dtMain.Rows[i]["Accountsledger_AmountCr"] == DBNull.Value || dtMain.Rows[i]["Accountsledger_AmountCr"].ToString() == "" || dtMain.Rows[i]["Accountsledger_AmountCr"].ToString() == "0")
                                SubHTML = SubHTML + "<td align=\"right\">&nbsp;</td>";
                            else
                                SubHTML = SubHTML + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["Accountsledger_AmountCr"].ToString())) + "</td>";

                            if (dtMain.Rows[i]["Accountsledger_AmountDr"] == DBNull.Value || dtMain.Rows[i]["Accountsledger_AmountDr"].ToString() == "" || dtMain.Rows[i]["Accountsledger_AmountDr"].ToString() == "0")
                                SubHTML = SubHTML + "<td align=\"right\">&nbsp;</td>";
                            else
                                SubHTML = SubHTML + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["Accountsledger_AmountDr"].ToString())) + "</td>";

                            if (dtMain.Rows[i]["Closing"] == DBNull.Value || dtMain.Rows[i]["Closing"].ToString() == "" || dtMain.Rows[i]["Closing"].ToString() == "0")
                            {
                                SubHTML = SubHTML + "<td align=\"right\">&nbsp;</td>";
                            }
                            else
                            {
                                if (dtMain.Rows[i]["Closing"].ToString().Substring(0, 1) == "-")
                                    SubHTML = SubHTML + "<td align=\"right\" style=\"color:red\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["Closing"].ToString())) + "</td>";
                                else
                                    SubHTML = SubHTML + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["Closing"].ToString())) + "</td>";
                            }

                            SubHTML = SubHTML + "</tr>";
                            //-----End Of Transaction Details ---------

                            //-----Obligation Breakup-----------------
                            SubStr = "";
                            if (IsConsolidate == "Settlement Obligation")
                            {

                                if (SegmentName == "NSE-CM" || SegmentName == "BSE-CM" || SegmentName == "CSE-CM")
                                {
                                    if (dtScripCM.Rows.Count > 0)
                                    {
                                        TotOblg = 0;
                                        SubStr = "<table border=\"1\" width=\"100%\">";
                                        SubStr = SubStr + "<tr style=\"background-color:#EEE0E5;font-weight:bold;\"><td style=\"font-weight:bold;\" colspan=\"4\">Obligation Breakup For Settlement No. " + SettlementNOType + "</td></tr>";
                                        SubStr = SubStr + "<tr style=\"background-color:#EEE0E5;font-weight:bold;\"><td style=\"font-weight:bold;\">Scrip</td><td style=\"font-weight:bold;\">Type</td><td style=\"font-weight:bold;\" align=\"right\">Quantity</td><td align=\"right\" style=\"font-weight:bold;\">Amount</td></tr>";
                                        for (int j = 0; j < dtScripCM.Rows.Count; j++)
                                        {

                                            if (dtScripCM.Rows[j]["CMPOSITION_CUSTOMERID"].ToString().Trim() == CustomerID && (dtScripCM.Rows[j]["SETTLEMENT"].ToString().Trim() + " " == SettlementNOType))
                                            {

                                                SubStr = SubStr + "<tr><td>&nbsp;" + dtScripCM.Rows[j]["PRODUCTSERIESID"].ToString() + "</td><td>&nbsp;" + dtScripCM.Rows[j]["DELIVERYTYPE"].ToString() + "</td>";
                                                if (oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripCM.Rows[j]["QUANTITY"].ToString())) == "0.00")
                                                    SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                                else
                                                    SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripCM.Rows[j]["QUANTITY"].ToString())) + "</td>";

                                                if (oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripCM.Rows[j]["AMOUNT"].ToString())) == "0.00")
                                                    SubStr = SubStr + "<td align=\"right\">&nbsp;</td></tr>";
                                                else
                                                    SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripCM.Rows[j]["AMOUNT"].ToString())) + "</td></tr>";


                                                TotOblg = TotOblg + Convert.ToDecimal(dtScripCM.Rows[j]["AMOUNT"].ToString());
                                            }

                                        }
                                        SubStr = SubStr + "<tr style=\"background-color:#EEEED1;font-weight:bold;\"><td colspan=\"3\" align=\"right\" style=\"font-weight:bold;\">&nbsp;Net Obligation</td><td  align=\"right\" >&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(TotOblg) + "</td></tr>";
                                        TotChrg = 0;
                                        for (int k = 0; k < dtTax.Rows.Count; k++)
                                        {
                                            if (dtTax.Rows[k]["ACCOUNTSLEDGER_SUBACCOUNTID"].ToString().Trim() == CustomerID && (dtTax.Rows[k]["SETTLEMENT"].ToString().Trim() + " ") == SettlementNOType)
                                            {
                                                SubStr = SubStr + "<tr><td colspan=\"3\" align=\"right\">&nbsp;" + dtTax.Rows[k]["CHARGE_TYPE1"].ToString() + "</td><td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtTax.Rows[k]["ACCOUNTSLEDGER_AMOUNTDR"].ToString())) + "</td></tr>";
                                                TotChrg = TotChrg + Convert.ToDecimal(dtTax.Rows[k]["ACCOUNTSLEDGER_AMOUNTDR"].ToString());

                                            }

                                        }
                                        TotNet = 0;
                                        TotNet = TotOblg + TotChrg;
                                        SubStr = SubStr + "<tr style=\"background-color:#EEEED1;font-weight:bold;\"><td colspan=\"3\" align=\"right\" style=\"font-weight:bold;\">&nbsp;Total Charges</td><td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(TotChrg) + "</td></tr>";
                                        SubStr = SubStr + "<tr style=\"background-color:#EEEED1;font-weight:bold;\"><td  colspan=\"3\" align=\"right\" style=\"font-weight:bold;\">&nbsp;Net Total</td><td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(TotNet) + "</td></tr>";


                                        SubStr = SubStr + "</table>";

                                    }

                                }
                                else
                                {

                                    if (dtScripFO.Rows.Count > 0)
                                    {
                                        SubStr = "<table border=\"1\" width=\"100%\">";
                                        SubStr = SubStr + "<tr style=\"background-color:#EEE0E5;font-weight:bold;\"><td style=\"font-weight:bold;\" colspan=\"10\">Obligation Break Up For " + TransactionDate + "</td></tr>";
                                        SubStr = SubStr + "<tr style=\"background-color:#EEE0E5;font-weight:bold;\"><td style=\"font-weight:bold;\">SCRIP</td><td align=\"right\" style=\"font-weight:bold;\">BF</td><td align=\"right\" style=\"font-weight:bold;\">BF PRICE</td><td align=\"right\" style=\"font-weight:bold;\">BUY</td><td align=\"right\" style=\"font-weight:bold;\">BUY PRICE</td><td align=\"right\" style=\"font-weight:bold;\">SELL</td><td align=\"right\" style=\"font-weight:bold;\">SELL PRICE</td><td align=\"right\" style=\"font-weight:bold;\">CF</td><td align=\"right\" style=\"font-weight:bold;\">CF PRICE</td><td align=\"right\" style=\"font-weight:bold;\">NET</td></tr>";
                                        for (int n = 0; n < dtScripFO.Rows.Count; n++)
                                        {
                                            if (dtScripFO.Rows[n]["FOPosition_CustomerExchangeID"].ToString() == CustomerID && dtScripFO.Rows[n]["SETTLEMENT"].ToString() == SettlementNOType && dtScripFO.Rows[n]["DATE_FOR"].ToString() == TransactionDate)
                                            {
                                                SubStr = SubStr + "<tr><td>&nbsp;" + dtScripFO.Rows[n]["SCRIP"].ToString() + "</td>";
                                                if (dtScripFO.Rows[n]["BF"] == DBNull.Value || dtScripFO.Rows[n]["BF"].ToString() == "" || dtScripFO.Rows[n]["BF"].ToString() == "0.000000")
                                                    SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                                else
                                                    SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["BF"].ToString())) + "</td>";

                                                if (dtScripFO.Rows[n]["BF_PRICE"] == DBNull.Value || dtScripFO.Rows[n]["BF_PRICE"].ToString() == "" || dtScripFO.Rows[n]["BF_PRICE"].ToString() == "0.000000")
                                                    SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                                else
                                                    SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["BF_PRICE"].ToString())) + "</td>";



                                                if (dtScripFO.Rows[n]["BUY"] == DBNull.Value || dtScripFO.Rows[n]["BUY"].ToString() == "" || dtScripFO.Rows[n]["BUY"].ToString() == "0.000000")
                                                    SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                                else
                                                    SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["BUY"].ToString())) + "</td>";
                                                if (dtScripFO.Rows[n]["BUY_PRICE"] == DBNull.Value || dtScripFO.Rows[n]["BUY_PRICE"].ToString() == "" || dtScripFO.Rows[n]["BUY_PRICE"].ToString() == "0.000000")
                                                    SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                                else
                                                    SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["BUY_PRICE"].ToString())) + "</td>";



                                                if (dtScripFO.Rows[n]["SELL"] == DBNull.Value || dtScripFO.Rows[n]["SELL"].ToString() == "" || dtScripFO.Rows[n]["SELL"].ToString() == "0.000000")
                                                    SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                                else
                                                    SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["SELL"].ToString())) + "</td>";
                                                if (dtScripFO.Rows[n]["SELL_PRICE"] == DBNull.Value || dtScripFO.Rows[n]["SELL_PRICE"].ToString() == "" || dtScripFO.Rows[n]["SELL_PRICE"].ToString() == "0.000000")
                                                    SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                                else
                                                    SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["SELL_PRICE"].ToString())) + "</td>";




                                                if (dtScripFO.Rows[n]["CF"] == DBNull.Value || dtScripFO.Rows[n]["CF"].ToString() == "" || dtScripFO.Rows[n]["CF"].ToString() == "0.000000")
                                                    SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                                else
                                                    SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["CF"].ToString())) + "</td>";
                                                if (dtScripFO.Rows[n]["CF_PRICE"] == DBNull.Value || dtScripFO.Rows[n]["CF_PRICE"].ToString() == "" || dtScripFO.Rows[n]["CF_PRICE"].ToString() == "0.000000")
                                                    SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                                else
                                                    SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["CF_PRICE"].ToString())) + "</td>";


                                                SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["NET"].ToString())) + "</td></tr>";
                                                TotOblg = TotOblg + Convert.ToDecimal(dtScripFO.Rows[n]["NET"].ToString());
                                            }

                                        }

                                        SubStr = SubStr + "<tr style=\"background-color:#EEEED1;font-weight:bold;\"><td colspan=\"9\" align=\"right\" style=\"font-weight:bold;\">&nbsp;Net Obligation </td><td align=\"right\">" + oconverter.getFormattedvaluewithoriginalsign(TotOblg) + "</td></tr>";


                                        TotChrg = 0;
                                        for (int k = 0; k < dtTax.Rows.Count; k++)
                                        {
                                            if (dtTax.Rows[k]["ACCOUNTSLEDGER_SUBACCOUNTID"].ToString().Trim() == CustomerID && (dtTax.Rows[k]["SETTLEMENT"].ToString().Trim() + " ") == SettlementNOType && dtTax.Rows[k]["CHARGE_DATE"].ToString().Trim() == TransactionDate)
                                            {
                                                SubStr = SubStr + "<tr><td colspan=\"9\" align=\"right\">&nbsp;" + dtTax.Rows[k]["CHARGE_TYPE1"].ToString() + "</td><td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtTax.Rows[k]["ACCOUNTSLEDGER_AMOUNTDR"].ToString())) + "</td></tr>";
                                                TotChrg = TotChrg + Convert.ToDecimal(dtTax.Rows[k]["ACCOUNTSLEDGER_AMOUNTDR"].ToString());

                                            }

                                        }
                                        TotNet = 0;
                                        TotNet = TotOblg + TotChrg;
                                        SubStr = SubStr + "<tr style=\"background-color:#EEEED1;font-weight:bold;\"><td colspan=\"9\" align=\"right\" style=\"font-weight:bold;\">&nbsp;Total Charges</td><td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(TotChrg) + "</td></tr>";
                                        SubStr = SubStr + "<tr style=\"background-color:#EEEED1;font-weight:bold;\"><td colspan=\"9\" align=\"right\" style=\"font-weight:bold;\">&nbsp;Net Total</td><td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(TotNet) + "</td></tr>";


                                        SubStr = SubStr + "</table>";

                                    }
                                }
                                SubHTML = SubHTML + "<tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td colspan=\"4\" valign=\"top\">&nbsp;" + SubStr + "</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr>";
                            }



                            //-----End  of Obligation Breakup-----------------
                            //-----Closing Balance Row------ 
                            if (clInd == 0)
                            {
                                if (dtMain.Rows[i]["ClosingBalanceDr"] == DBNull.Value || dtMain.Rows[i]["ClosingBalanceDr"] == "")
                                    dtMain.Rows[i]["ClosingBalanceDr"] = "0";
                                if (dtMain.Rows[i]["ClosingBalancecr"] == DBNull.Value || dtMain.Rows[i]["ClosingBalancecr"] == "")
                                    dtMain.Rows[i]["ClosingBalancecr"] = "0";
                                if (dtMain.Rows[i]["ClosingTotal"] == DBNull.Value || dtMain.Rows[i]["ClosingTotal"] == "")
                                    dtMain.Rows[i]["ClosingTotal"] = "0";
                                SubHTML = SubHTML + "<tr style=\"background-color:#EEE0E5;font-weight:bold;\"><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td style=\"font-weight:bold;\">Closing Balance</td><td>&nbsp;</td><td>&nbsp;</td><td align=\"right\"  style=\"font-weight:bold;\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["ClosingBalanceDr"].ToString())) + "</td><td  align=\"right\" style=\"font-weight:bold;\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["ClosingBalanceCr"].ToString())) + "</td><td  align=\"right\" style=\"font-weight:bold;\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["ClosingTotal"].ToString())) + "</td></tr>";
                                clInd = 1;

                            }
                            //-----End of Closing Balance Row------    
                            DataRow Row2 = dtMail.NewRow();
                            Row2[0] = dtMain.Rows[i]["SubID"].ToString();
                            Row2[1] = dtMain.Rows[i]["BranchCode"].ToString();
                            Row2[2] = strHTML + " " + SubHTML + "</table>";
                            Row2[3] = dtMain.Rows[i]["SubLedgerType"].ToString();
                            dtMail.Rows.Add(Row2);
                            SubHTML = "";
                            clInd = 0;
                            opInd = 0;
                        }
                        else
                        {
                            //----Opening Balance Row------ 
                            if (opInd == 0)
                            {
                                if (dtMain.Rows[i]["OpeningBalanceDr"] == DBNull.Value || dtMain.Rows[i]["OpeningBalanceDr"] == "")
                                    dtMain.Rows[i]["OpeningBalanceDr"] = "0";
                                if (dtMain.Rows[i]["OpeningBalanceCr"] == DBNull.Value || dtMain.Rows[i]["OpeningBalanceCr"] == "")
                                    dtMain.Rows[i]["OpeningBalanceCr"] = "0";
                                if (dtMain.Rows[i]["OpeningTotal"] == DBNull.Value || dtMain.Rows[i]["OpeningTotal"] == "")
                                    dtMain.Rows[i]["OpeningTotal"] = "0";
                                if (dtMain.Rows[i]["OpeningBalanceDr"] == DBNull.Value || dtMain.Rows[i]["OpeningBalanceDr"].ToString() == "" || dtMain.Rows[i]["OpeningBalanceDr"].ToString() == "0")
                                    SubHTML = SubHTML + "<tr  style=\"background-color:#EEE0E5;font-weight:bold;\"><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td style=\"font-weight:bold;\">Opening Balance</td><td>&nbsp;</td><td>&nbsp;</td><td  align=\"right\" style=\"font-weight:bold;\">&nbsp;</td>";
                                else
                                    SubHTML = SubHTML + "<tr  style=\"background-color:#EEE0E5;font-weight:bold;\"><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td style=\"font-weight:bold;\">Opening Balance</td><td>&nbsp;</td><td>&nbsp;</td><td  align=\"right\" style=\"font-weight:bold;\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["OpeningBalanceDr"].ToString())) + "</td>";
                                if (dtMain.Rows[i]["OpeningBalanceCr"] == DBNull.Value || dtMain.Rows[i]["OpeningBalanceCr"].ToString() == "" || dtMain.Rows[i]["OpeningBalanceCr"].ToString() == "0")
                                    SubHTML = SubHTML + "<td  align=\"right\" style=\"font-weight:bold;\">&nbsp</td>";
                                else
                                    SubHTML = SubHTML + "<td  align=\"right\" style=\"font-weight:bold;\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["OpeningBalanceCr"].ToString())) + "</td>";
                                if (dtMain.Rows[i]["OpeningTotal"] == DBNull.Value || dtMain.Rows[i]["OpeningTotal"].ToString() == "" || dtMain.Rows[i]["OpeningTotal"].ToString() == "0")
                                    SubHTML = SubHTML + "<td  align=\"right\" style=\"font-weight:bold;\">&nbsp</td></tr>";
                                else
                                    SubHTML = SubHTML + "<td  align=\"right\" style=\"font-weight:bold;\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["OpeningTotal"].ToString())) + "</td></tr>";

                                opInd = 1;
                            }
                            //---- End of Opening Balance Row------ 
                            //-----Transaction Details ---------
                            SubHTML = SubHTML + "<tr>";
                            SubHTML = SubHTML + "<td >&nbsp;" + dtMain.Rows[i][1].ToString() + "</td>";
                            SubHTML = SubHTML + "<td>&nbsp;" + dtMain.Rows[i][2].ToString() + "</td>";
                            SubHTML = SubHTML + "<td>&nbsp;" + dtMain.Rows[i]["accountsledger_TransactionReferenceID"].ToString() + "</td>";
                            SubHTML = SubHTML + "<td>&nbsp;" + dtMain.Rows[i][10].ToString() + "</td>";
                            SubHTML = SubHTML + "<td>&nbsp;" + dtMain.Rows[i][4].ToString() + "| [" + SegmentName + "]</td>";
                            if (SegmentName == "NSE-CM" || SegmentName == "BSE-CM" || SegmentName == "CSE-CM")
                            {
                                SubHTML = SubHTML + "<td>&nbsp;" + dtMain.Rows[i]["PayoutDate"].ToString() + "</td>";
                            }
                            else
                            {
                                SubHTML = SubHTML + "<td>&nbsp;" + dtMain.Rows[i]["TrDate"].ToString() + "</td>";
                            }
                            SubHTML = SubHTML + "<td>&nbsp;" + dtMain.Rows[i][11].ToString() + "</td>";


                            if (dtMain.Rows[i]["Accountsledger_AmountCr"] == DBNull.Value || dtMain.Rows[i]["Accountsledger_AmountCr"].ToString() == "" || dtMain.Rows[i]["Accountsledger_AmountCr"].ToString() == "0")
                                SubHTML = SubHTML + "<td align=\"right\">&nbsp;</td>";
                            else
                                SubHTML = SubHTML + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["Accountsledger_AmountCr"].ToString())) + "</td>";

                            if (dtMain.Rows[i]["Accountsledger_AmountDr"] == DBNull.Value || dtMain.Rows[i]["Accountsledger_AmountDr"].ToString() == "" || dtMain.Rows[i]["Accountsledger_AmountDr"].ToString() == "0")
                                SubHTML = SubHTML + "<td align=\"right\">&nbsp;</td>";
                            else
                                SubHTML = SubHTML + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["Accountsledger_AmountDr"].ToString())) + "</td>";

                            if (dtMain.Rows[i]["Closing"] == DBNull.Value || dtMain.Rows[i]["Closing"].ToString() == "" || dtMain.Rows[i]["Closing"].ToString() == "0")
                            {
                                SubHTML = SubHTML + "<td align=\"right\">&nbsp;</td>";
                            }
                            else
                            {
                                if (dtMain.Rows[i]["Closing"].ToString().Substring(0, 1) == "-")
                                    SubHTML = SubHTML + "<td align=\"right\" style=\"color:red\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["Closing"].ToString())) + "</td>";
                                else
                                    SubHTML = SubHTML + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["Closing"].ToString())) + "</td>";
                            }

                            SubHTML = SubHTML + "</tr>";
                            //-----End Of Transaction Details ---------
                            //-----Obligation Breakup-----------------
                            SubStr = "";
                            if (IsConsolidate == "Settlement Obligation")
                            {

                                if (SegmentName == "NSE-CM" || SegmentName == "BSE-CM" || SegmentName == "CSE-CM")
                                {
                                    if (dtScripCM.Rows.Count > 0)
                                    {
                                        TotOblg = 0;
                                        SubStr = "<table border=\"1\" width=\"100%\">";
                                        SubStr = SubStr + "<tr style=\"background-color:#EEE0E5;font-weight:bold;\"><td style=\"font-weight:bold;\" colspan=\"4\">Obligation Breakup For Settlement No. " + SettlementNOType + "</td></tr>";
                                        SubStr = SubStr + "<tr style=\"background-color:#EEE0E5;font-weight:bold;\"><td style=\"font-weight:bold;\">Scrip</td><td style=\"font-weight:bold;\">Type</td><td style=\"font-weight:bold;\" align=\"right\">Quantity</td><td align=\"right\" style=\"font-weight:bold;\">Amount</td></tr>";
                                        for (int j = 0; j < dtScripCM.Rows.Count; j++)
                                        {

                                            if (dtScripCM.Rows[j]["CMPOSITION_CUSTOMERID"].ToString().Trim() == CustomerID && (dtScripCM.Rows[j]["SETTLEMENT"].ToString().Trim() + " " == SettlementNOType))
                                            {

                                                SubStr = SubStr + "<tr><td>&nbsp;" + dtScripCM.Rows[j]["PRODUCTSERIESID"].ToString() + "</td><td>&nbsp;" + dtScripCM.Rows[j]["DELIVERYTYPE"].ToString() + "</td>";
                                                if (oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripCM.Rows[j]["QUANTITY"].ToString())) == "0.00")
                                                    SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                                else
                                                    SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripCM.Rows[j]["QUANTITY"].ToString())) + "</td>";

                                                if (oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripCM.Rows[j]["AMOUNT"].ToString())) == "0.00")
                                                    SubStr = SubStr + "<td align=\"right\">&nbsp;</td></tr>";
                                                else
                                                    SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripCM.Rows[j]["AMOUNT"].ToString())) + "</td></tr>";


                                                TotOblg = TotOblg + Convert.ToDecimal(dtScripCM.Rows[j]["AMOUNT"].ToString());
                                            }

                                        }
                                        SubStr = SubStr + "<tr style=\"background-color:#EEEED1;font-weight:bold;\"><td colspan=\"3\" align=\"right\" style=\"font-weight:bold;\">&nbsp;Net Obligation</td><td  align=\"right\" >&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(TotOblg) + "</td></tr>";
                                        TotChrg = 0;
                                        for (int k = 0; k < dtTax.Rows.Count; k++)
                                        {
                                            if (dtTax.Rows[k]["ACCOUNTSLEDGER_SUBACCOUNTID"].ToString().Trim() == CustomerID && (dtTax.Rows[k]["SETTLEMENT"].ToString().Trim() + " ") == SettlementNOType)
                                            {
                                                SubStr = SubStr + "<tr><td colspan=\"3\" align=\"right\">&nbsp;" + dtTax.Rows[k]["CHARGE_TYPE1"].ToString() + "</td><td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtTax.Rows[k]["ACCOUNTSLEDGER_AMOUNTDR"].ToString())) + "</td></tr>";
                                                TotChrg = TotChrg + Convert.ToDecimal(dtTax.Rows[k]["ACCOUNTSLEDGER_AMOUNTDR"].ToString());

                                            }

                                        }
                                        TotNet = 0;
                                        TotNet = TotOblg + TotChrg;
                                        SubStr = SubStr + "<tr style=\"background-color:#EEEED1;font-weight:bold;\"><td colspan=\"3\" align=\"right\" style=\"font-weight:bold;\">&nbsp;Total Charges</td><td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(TotChrg) + "</td></tr>";
                                        SubStr = SubStr + "<tr style=\"background-color:#EEEED1;font-weight:bold;\"><td  colspan=\"3\" align=\"right\" style=\"font-weight:bold;\">&nbsp;Net Total</td><td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(TotNet) + "</td></tr>";


                                        SubStr = SubStr + "</table>";

                                    }

                                }
                                else
                                {

                                    if (dtScripFO.Rows.Count > 0)
                                    {
                                        SubStr = "<table border=\"1\" width=\"100%\">";
                                        SubStr = SubStr + "<tr style=\"background-color:#EEE0E5;font-weight:bold;\"><td style=\"font-weight:bold;\" colspan=\"10\">Obligation Break Up For " + TransactionDate + "</td></tr>";
                                        SubStr = SubStr + "<tr style=\"background-color:#EEE0E5;font-weight:bold;\"><td style=\"font-weight:bold;\">SCRIP</td><td align=\"right\" style=\"font-weight:bold;\">BF</td><td align=\"right\" style=\"font-weight:bold;\">BF PRICE</td><td align=\"right\" style=\"font-weight:bold;\">BUY</td><td align=\"right\" style=\"font-weight:bold;\">BUY PRICE</td><td align=\"right\" style=\"font-weight:bold;\">SELL</td><td align=\"right\" style=\"font-weight:bold;\">SELL PRICE</td><td align=\"right\" style=\"font-weight:bold;\">CF</td><td align=\"right\" style=\"font-weight:bold;\">CF PRICE</td><td align=\"right\" style=\"font-weight:bold;\">NET</td></tr>";
                                        for (int n = 0; n < dtScripFO.Rows.Count; n++)
                                        {
                                            if (dtScripFO.Rows[n]["FOPosition_CustomerExchangeID"].ToString() == CustomerID && dtScripFO.Rows[n]["SETTLEMENT"].ToString() == SettlementNOType && dtScripFO.Rows[n]["DATE_FOR"].ToString() == TransactionDate)
                                            {
                                                SubStr = SubStr + "<tr><td>&nbsp;" + dtScripFO.Rows[n]["SCRIP"].ToString() + "</td>";
                                                if (dtScripFO.Rows[n]["BF"] == DBNull.Value || dtScripFO.Rows[n]["BF"].ToString() == "" || dtScripFO.Rows[n]["BF"].ToString() == "0.000000")
                                                    SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                                else
                                                    SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["BF"].ToString())) + "</td>";

                                                if (dtScripFO.Rows[n]["BF_PRICE"] == DBNull.Value || dtScripFO.Rows[n]["BF_PRICE"].ToString() == "" || dtScripFO.Rows[n]["BF_PRICE"].ToString() == "0.000000")
                                                    SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                                else
                                                    SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["BF_PRICE"].ToString())) + "</td>";



                                                if (dtScripFO.Rows[n]["BUY"] == DBNull.Value || dtScripFO.Rows[n]["BUY"].ToString() == "" || dtScripFO.Rows[n]["BUY"].ToString() == "0.000000")
                                                    SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                                else
                                                    SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["BUY"].ToString())) + "</td>";
                                                if (dtScripFO.Rows[n]["BUY_PRICE"] == DBNull.Value || dtScripFO.Rows[n]["BUY_PRICE"].ToString() == "" || dtScripFO.Rows[n]["BUY_PRICE"].ToString() == "0.000000")
                                                    SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                                else
                                                    SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["BUY_PRICE"].ToString())) + "</td>";



                                                if (dtScripFO.Rows[n]["SELL"] == DBNull.Value || dtScripFO.Rows[n]["SELL"].ToString() == "" || dtScripFO.Rows[n]["SELL"].ToString() == "0.000000")
                                                    SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                                else
                                                    SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["SELL"].ToString())) + "</td>";
                                                if (dtScripFO.Rows[n]["SELL_PRICE"] == DBNull.Value || dtScripFO.Rows[n]["SELL_PRICE"].ToString() == "" || dtScripFO.Rows[n]["SELL_PRICE"].ToString() == "0.000000")
                                                    SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                                else
                                                    SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["SELL_PRICE"].ToString())) + "</td>";




                                                if (dtScripFO.Rows[n]["CF"] == DBNull.Value || dtScripFO.Rows[n]["CF"].ToString() == "" || dtScripFO.Rows[n]["CF"].ToString() == "0.000000")
                                                    SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                                else
                                                    SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["CF"].ToString())) + "</td>";
                                                if (dtScripFO.Rows[n]["CF_PRICE"] == DBNull.Value || dtScripFO.Rows[n]["CF_PRICE"].ToString() == "" || dtScripFO.Rows[n]["CF_PRICE"].ToString() == "0.000000")
                                                    SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                                else
                                                    SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["CF_PRICE"].ToString())) + "</td>";


                                                SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["NET"].ToString())) + "</td></tr>";
                                                TotOblg = TotOblg + Convert.ToDecimal(dtScripFO.Rows[n]["NET"].ToString());
                                            }

                                        }

                                        SubStr = SubStr + "<tr style=\"background-color:#EEEED1;font-weight:bold;\"><td colspan=\"9\" align=\"right\" style=\"font-weight:bold;\">&nbsp;Net Obligation </td><td align=\"right\">" + oconverter.getFormattedvaluewithoriginalsign(TotOblg) + "</td></tr>";


                                        TotChrg = 0;
                                        for (int k = 0; k < dtTax.Rows.Count; k++)
                                        {
                                            if (dtTax.Rows[k]["ACCOUNTSLEDGER_SUBACCOUNTID"].ToString().Trim() == CustomerID && (dtTax.Rows[k]["SETTLEMENT"].ToString().Trim() + " ") == SettlementNOType && dtTax.Rows[k]["CHARGE_DATE"].ToString().Trim() == TransactionDate)
                                            {
                                                SubStr = SubStr + "<tr><td colspan=\"9\" align=\"right\">&nbsp;" + dtTax.Rows[k]["CHARGE_TYPE1"].ToString() + "</td><td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtTax.Rows[k]["ACCOUNTSLEDGER_AMOUNTDR"].ToString())) + "</td></tr>";
                                                TotChrg = TotChrg + Convert.ToDecimal(dtTax.Rows[k]["ACCOUNTSLEDGER_AMOUNTDR"].ToString());

                                            }

                                        }
                                        TotNet = 0;
                                        TotNet = TotOblg + TotChrg;
                                        SubStr = SubStr + "<tr style=\"background-color:#EEEED1;font-weight:bold;\"><td colspan=\"9\" align=\"right\" style=\"font-weight:bold;\">&nbsp;Total Charges</td><td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(TotChrg) + "</td></tr>";
                                        SubStr = SubStr + "<tr style=\"background-color:#EEEED1;font-weight:bold;\"><td colspan=\"9\" align=\"right\" style=\"font-weight:bold;\">&nbsp;Net Total</td><td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(TotNet) + "</td></tr>";


                                        SubStr = SubStr + "</table>";

                                    }
                                }
                                SubHTML = SubHTML + "<tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td colspan=\"4\" valign=\"top\">&nbsp;" + SubStr + "</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr>";
                            }



                            //-----End  of Obligation Breakup-----------------

                            //-----Closing Balance Row------ 
                            if (clInd == 0)
                            {
                                if (dtMain.Rows[i]["ClosingBalanceDr"] == DBNull.Value || dtMain.Rows[i]["ClosingBalanceDr"] == "")
                                    dtMain.Rows[i]["ClosingBalanceDr"] = "0";
                                if (dtMain.Rows[i]["ClosingBalancecr"] == DBNull.Value || dtMain.Rows[i]["ClosingBalancecr"] == "")
                                    dtMain.Rows[i]["ClosingBalancecr"] = "0";
                                if (dtMain.Rows[i]["ClosingTotal"] == DBNull.Value || dtMain.Rows[i]["ClosingTotal"] == "")
                                    dtMain.Rows[i]["ClosingTotal"] = "0";
                                SubHTML = SubHTML + "<tr style=\"background-color:#EEE0E5;font-weight:bold;\"><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td style=\"font-weight:bold;\">Closing Balance</td><td>&nbsp;</td><td>&nbsp;</td><td align=\"right\"  style=\"font-weight:bold;\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["ClosingBalanceDr"].ToString())) + "</td><td  align=\"right\" style=\"font-weight:bold;\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["ClosingBalanceCr"].ToString())) + "</td><td  align=\"right\" style=\"font-weight:bold;\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["ClosingTotal"].ToString())) + "</td></tr>";
                                clInd = 1;

                            }
                            //-----End of Closing Balance Row------

                            DataRow Row3 = dtMail.NewRow();
                            Row3[0] = dtMain.Rows[i]["SubID"].ToString();
                            Row3[1] = dtMain.Rows[i]["BranchCode"].ToString();
                            Row3[2] = strHTML + " " + SubHTML + "</table>";
                            Row3[3] = dtMain.Rows[i]["SubLedgerType"].ToString();
                            dtMail.Rows.Add(Row3);
                            SubHTML = "";
                            clInd = 0;
                            opInd = 0;

                        }

                    }
                    else
                    {
                        //----Opening Balance Row------ 
                        if (opInd == 0)
                        {
                            if (dtMain.Rows[i]["OpeningBalanceDr"] == DBNull.Value || dtMain.Rows[i]["OpeningBalanceDr"] == "")
                                dtMain.Rows[i]["OpeningBalanceDr"] = "0";
                            if (dtMain.Rows[i]["OpeningBalanceCr"] == DBNull.Value || dtMain.Rows[i]["OpeningBalanceCr"] == "")
                                dtMain.Rows[i]["OpeningBalanceCr"] = "0";
                            if (dtMain.Rows[i]["OpeningTotal"] == DBNull.Value || dtMain.Rows[i]["OpeningTotal"] == "")
                                dtMain.Rows[i]["OpeningTotal"] = "0";
                            if (dtMain.Rows[i]["OpeningBalanceDr"] == DBNull.Value || dtMain.Rows[i]["OpeningBalanceDr"].ToString() == "" || dtMain.Rows[i]["OpeningBalanceDr"].ToString() == "0")
                                SubHTML = SubHTML + "<tr  style=\"background-color:#EEE0E5;font-weight:bold;\"><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td style=\"font-weight:bold;\">Opening Balance</td><td>&nbsp;</td><td>&nbsp;</td><td  align=\"right\" style=\"font-weight:bold;\">&nbsp;</td>";
                            else
                                SubHTML = SubHTML + "<tr  style=\"background-color:#EEE0E5;font-weight:bold;\"><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td style=\"font-weight:bold;\">Opening Balance</td><td>&nbsp;</td><td>&nbsp;</td><td  align=\"right\" style=\"font-weight:bold;\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["OpeningBalanceDr"].ToString())) + "</td>";
                            if (dtMain.Rows[i]["OpeningBalanceCr"] == DBNull.Value || dtMain.Rows[i]["OpeningBalanceCr"].ToString() == "" || dtMain.Rows[i]["OpeningBalanceCr"].ToString() == "0")
                                SubHTML = SubHTML + "<td  align=\"right\" style=\"font-weight:bold;\">&nbsp</td>";
                            else
                                SubHTML = SubHTML + "<td  align=\"right\" style=\"font-weight:bold;\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["OpeningBalanceCr"].ToString())) + "</td>";
                            if (dtMain.Rows[i]["OpeningTotal"] == DBNull.Value || dtMain.Rows[i]["OpeningTotal"].ToString() == "" || dtMain.Rows[i]["OpeningTotal"].ToString() == "0")
                                SubHTML = SubHTML + "<td  align=\"right\" style=\"font-weight:bold;\">&nbsp</td></tr>";
                            else
                                SubHTML = SubHTML + "<td  align=\"right\" style=\"font-weight:bold;\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["OpeningTotal"].ToString())) + "</td></tr>";

                            opInd = 1;
                        }
                        //---- End of Opening Balance Row------ 
                        //-----Transaction Details ---------
                        SubHTML = SubHTML + "<tr>";
                        SubHTML = SubHTML + "<td >&nbsp;" + dtMain.Rows[i][1].ToString() + "</td>";
                        SubHTML = SubHTML + "<td>&nbsp;" + dtMain.Rows[i][2].ToString() + "</td>";
                        SubHTML = SubHTML + "<td>&nbsp;" + dtMain.Rows[i]["accountsledger_TransactionReferenceID"].ToString() + "</td>";
                        SubHTML = SubHTML + "<td>&nbsp;" + dtMain.Rows[i][10].ToString() + "</td>";
                        SubHTML = SubHTML + "<td>&nbsp;" + dtMain.Rows[i][4].ToString() + "| [" + SegmentName + "]</td>";
                        if (SegmentName == "NSE-CM" || SegmentName == "BSE-CM" || SegmentName == "CSE-CM")
                        {
                            SubHTML = SubHTML + "<td>&nbsp;" + dtMain.Rows[i]["PayoutDate"].ToString() + "</td>";
                        }
                        else
                        {
                            SubHTML = SubHTML + "<td>&nbsp;" + dtMain.Rows[i]["TrDate"].ToString() + "</td>";
                        }
                        SubHTML = SubHTML + "<td>&nbsp;" + dtMain.Rows[i][11].ToString() + "</td>";


                        if (dtMain.Rows[i]["Accountsledger_AmountCr"] == DBNull.Value || dtMain.Rows[i]["Accountsledger_AmountCr"].ToString() == "" || dtMain.Rows[i]["Accountsledger_AmountCr"].ToString() == "0")
                            SubHTML = SubHTML + "<td align=\"right\">&nbsp;</td>";
                        else
                            SubHTML = SubHTML + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["Accountsledger_AmountCr"].ToString())) + "</td>";

                        if (dtMain.Rows[i]["Accountsledger_AmountDr"] == DBNull.Value || dtMain.Rows[i]["Accountsledger_AmountDr"].ToString() == "" || dtMain.Rows[i]["Accountsledger_AmountDr"].ToString() == "0")
                            SubHTML = SubHTML + "<td align=\"right\">&nbsp;</td>";
                        else
                            SubHTML = SubHTML + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["Accountsledger_AmountDr"].ToString())) + "</td>";

                        if (dtMain.Rows[i]["Closing"] == DBNull.Value || dtMain.Rows[i]["Closing"].ToString() == "" || dtMain.Rows[i]["Closing"].ToString() == "0")
                        {
                            SubHTML = SubHTML + "<td align=\"right\">&nbsp;</td>";
                        }
                        else
                        {
                            if (dtMain.Rows[i]["Closing"].ToString().Substring(0, 1) == "-")
                                SubHTML = SubHTML + "<td align=\"right\" style=\"color:red\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["Closing"].ToString())) + "</td>";
                            else
                                SubHTML = SubHTML + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["Closing"].ToString())) + "</td>";
                        }

                        SubHTML = SubHTML + "</tr>";
                        //-----End Of Transaction Details ---------
                        //-----Obligation Breakup-----------------
                        SubStr = "";
                        if (IsConsolidate == "Settlement Obligation")
                        {

                            if (SegmentName == "NSE-CM" || SegmentName == "BSE-CM" || SegmentName == "CSE-CM")
                            {
                                if (dtScripCM.Rows.Count > 0)
                                {
                                    TotOblg = 0;
                                    SubStr = "<table border=\"1\" width=\"100%\">";
                                    SubStr = SubStr + "<tr style=\"background-color:#EEE0E5;font-weight:bold;\"><td style=\"font-weight:bold;\" colspan=\"4\">Obligation Breakup For Settlement No. " + SettlementNOType + "</td></tr>";
                                    SubStr = SubStr + "<tr style=\"background-color:#EEE0E5;font-weight:bold;\"><td style=\"font-weight:bold;\">Scrip</td><td style=\"font-weight:bold;\">Type</td><td style=\"font-weight:bold;\" align=\"right\">Quantity</td><td align=\"right\" style=\"font-weight:bold;\">Amount</td></tr>";
                                    for (int j = 0; j < dtScripCM.Rows.Count; j++)
                                    {

                                        if (dtScripCM.Rows[j]["CMPOSITION_CUSTOMERID"].ToString().Trim() == CustomerID && (dtScripCM.Rows[j]["SETTLEMENT"].ToString().Trim() + " " == SettlementNOType))
                                        {

                                            SubStr = SubStr + "<tr><td>&nbsp;" + dtScripCM.Rows[j]["PRODUCTSERIESID"].ToString() + "</td><td>&nbsp;" + dtScripCM.Rows[j]["DELIVERYTYPE"].ToString() + "</td>";
                                            if (oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripCM.Rows[j]["QUANTITY"].ToString())) == "0.00")
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                            else
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripCM.Rows[j]["QUANTITY"].ToString())) + "</td>";

                                            if (oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripCM.Rows[j]["AMOUNT"].ToString())) == "0.00")
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;</td></tr>";
                                            else
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripCM.Rows[j]["AMOUNT"].ToString())) + "</td></tr>";


                                            TotOblg = TotOblg + Convert.ToDecimal(dtScripCM.Rows[j]["AMOUNT"].ToString());
                                        }

                                    }
                                    SubStr = SubStr + "<tr style=\"background-color:#EEEED1;font-weight:bold;\"><td colspan=\"3\" align=\"right\" style=\"font-weight:bold;\">&nbsp;Net Obligation</td><td  align=\"right\" >&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(TotOblg) + "</td></tr>";
                                    TotChrg = 0;
                                    for (int k = 0; k < dtTax.Rows.Count; k++)
                                    {
                                        if (dtTax.Rows[k]["ACCOUNTSLEDGER_SUBACCOUNTID"].ToString().Trim() == CustomerID && (dtTax.Rows[k]["SETTLEMENT"].ToString().Trim() + " ") == SettlementNOType)
                                        {
                                            SubStr = SubStr + "<tr><td colspan=\"3\" align=\"right\">&nbsp;" + dtTax.Rows[k]["CHARGE_TYPE1"].ToString() + "</td><td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtTax.Rows[k]["ACCOUNTSLEDGER_AMOUNTDR"].ToString())) + "</td></tr>";
                                            TotChrg = TotChrg + Convert.ToDecimal(dtTax.Rows[k]["ACCOUNTSLEDGER_AMOUNTDR"].ToString());

                                        }

                                    }
                                    TotNet = 0;
                                    TotNet = TotOblg + TotChrg;
                                    SubStr = SubStr + "<tr style=\"background-color:#EEEED1;font-weight:bold;\"><td colspan=\"3\" align=\"right\" style=\"font-weight:bold;\">&nbsp;Total Charges</td><td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(TotChrg) + "</td></tr>";
                                    SubStr = SubStr + "<tr style=\"background-color:#EEEED1;font-weight:bold;\"><td  colspan=\"3\" align=\"right\" style=\"font-weight:bold;\">&nbsp;Net Total</td><td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(TotNet) + "</td></tr>";


                                    SubStr = SubStr + "</table>";

                                }

                            }
                            else
                            {

                                if (dtScripFO.Rows.Count > 0)
                                {
                                    SubStr = "<table border=\"1\" width=\"100%\">";
                                    SubStr = SubStr + "<tr style=\"background-color:#EEE0E5;font-weight:bold;\"><td style=\"font-weight:bold;\" colspan=\"10\">Obligation Break Up For " + TransactionDate + "</td></tr>";
                                    SubStr = SubStr + "<tr style=\"background-color:#EEE0E5;font-weight:bold;\"><td style=\"font-weight:bold;\">SCRIP</td><td align=\"right\" style=\"font-weight:bold;\">BF</td><td align=\"right\" style=\"font-weight:bold;\">BF PRICE</td><td align=\"right\" style=\"font-weight:bold;\">BUY</td><td align=\"right\" style=\"font-weight:bold;\">BUY PRICE</td><td align=\"right\" style=\"font-weight:bold;\">SELL</td><td align=\"right\" style=\"font-weight:bold;\">SELL PRICE</td><td align=\"right\" style=\"font-weight:bold;\">CF</td><td align=\"right\" style=\"font-weight:bold;\">CF PRICE</td><td align=\"right\" style=\"font-weight:bold;\">NET</td></tr>";
                                    for (int n = 0; n < dtScripFO.Rows.Count; n++)
                                    {
                                        if (dtScripFO.Rows[n]["FOPosition_CustomerExchangeID"].ToString() == CustomerID && dtScripFO.Rows[n]["SETTLEMENT"].ToString() == SettlementNOType && dtScripFO.Rows[n]["DATE_FOR"].ToString() == TransactionDate)
                                        {
                                            SubStr = SubStr + "<tr><td>&nbsp;" + dtScripFO.Rows[n]["SCRIP"].ToString() + "</td>";
                                            if (dtScripFO.Rows[n]["BF"] == DBNull.Value || dtScripFO.Rows[n]["BF"].ToString() == "" || dtScripFO.Rows[n]["BF"].ToString() == "0.000000")
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                            else
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["BF"].ToString())) + "</td>";

                                            if (dtScripFO.Rows[n]["BF_PRICE"] == DBNull.Value || dtScripFO.Rows[n]["BF_PRICE"].ToString() == "" || dtScripFO.Rows[n]["BF_PRICE"].ToString() == "0.000000")
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                            else
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["BF_PRICE"].ToString())) + "</td>";



                                            if (dtScripFO.Rows[n]["BUY"] == DBNull.Value || dtScripFO.Rows[n]["BUY"].ToString() == "" || dtScripFO.Rows[n]["BUY"].ToString() == "0.000000")
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                            else
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["BUY"].ToString())) + "</td>";
                                            if (dtScripFO.Rows[n]["BUY_PRICE"] == DBNull.Value || dtScripFO.Rows[n]["BUY_PRICE"].ToString() == "" || dtScripFO.Rows[n]["BUY_PRICE"].ToString() == "0.000000")
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                            else
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["BUY_PRICE"].ToString())) + "</td>";



                                            if (dtScripFO.Rows[n]["SELL"] == DBNull.Value || dtScripFO.Rows[n]["SELL"].ToString() == "" || dtScripFO.Rows[n]["SELL"].ToString() == "0.000000")
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                            else
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["SELL"].ToString())) + "</td>";
                                            if (dtScripFO.Rows[n]["SELL_PRICE"] == DBNull.Value || dtScripFO.Rows[n]["SELL_PRICE"].ToString() == "" || dtScripFO.Rows[n]["SELL_PRICE"].ToString() == "0.000000")
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                            else
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["SELL_PRICE"].ToString())) + "</td>";




                                            if (dtScripFO.Rows[n]["CF"] == DBNull.Value || dtScripFO.Rows[n]["CF"].ToString() == "" || dtScripFO.Rows[n]["CF"].ToString() == "0.000000")
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                            else
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["CF"].ToString())) + "</td>";
                                            if (dtScripFO.Rows[n]["CF_PRICE"] == DBNull.Value || dtScripFO.Rows[n]["CF_PRICE"].ToString() == "" || dtScripFO.Rows[n]["CF_PRICE"].ToString() == "0.000000")
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;</td>";
                                            else
                                                SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["CF_PRICE"].ToString())) + "</td>";


                                            SubStr = SubStr + "<td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtScripFO.Rows[n]["NET"].ToString())) + "</td></tr>";
                                            TotOblg = TotOblg + Convert.ToDecimal(dtScripFO.Rows[n]["NET"].ToString());
                                        }
                                        SubStr = SubStr + "<tr style=\"background-color:#EEEED1;font-weight:bold;\"><td colspan=\"9\" align=\"right\" style=\"font-weight:bold;\">&nbsp;Net Obligation </td><td align=\"right\">" + oconverter.getFormattedvaluewithoriginalsign(TotOblg) + "</td></tr>";
                                    }




                                    TotChrg = 0;
                                    for (int k = 0; k < dtTax.Rows.Count; k++)
                                    {
                                        if (dtTax.Rows[k]["ACCOUNTSLEDGER_SUBACCOUNTID"].ToString().Trim() == CustomerID && (dtTax.Rows[k]["SETTLEMENT"].ToString().Trim() + " ") == SettlementNOType && dtTax.Rows[k]["CHARGE_DATE"].ToString().Trim() == TransactionDate)
                                        {
                                            SubStr = SubStr + "<tr><td colspan=\"9\" align=\"right\">&nbsp;" + dtTax.Rows[k]["CHARGE_TYPE1"].ToString() + "</td><td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtTax.Rows[k]["ACCOUNTSLEDGER_AMOUNTDR"].ToString())) + "</td></tr>";
                                            TotChrg = TotChrg + Convert.ToDecimal(dtTax.Rows[k]["ACCOUNTSLEDGER_AMOUNTDR"].ToString());

                                        }

                                    }
                                    TotNet = 0;
                                    TotNet = TotOblg + TotChrg;
                                    SubStr = SubStr + "<tr style=\"background-color:#EEEED1;font-weight:bold;\"><td colspan=\"9\" align=\"right\" style=\"font-weight:bold;\">&nbsp;Total Charges</td><td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(TotChrg) + "</td></tr>";
                                    SubStr = SubStr + "<tr style=\"background-color:#EEEED1;font-weight:bold;\"><td colspan=\"9\" align=\"right\" style=\"font-weight:bold;\">&nbsp;Net Total</td><td align=\"right\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(TotNet) + "</td></tr>";


                                    SubStr = SubStr + "</table>";

                                }
                            }
                        }

                        SubHTML = SubHTML + "<tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td colspan=\"4\" valign=\"top\">&nbsp;" + SubStr + "</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr>";

                        //-----End  of Obligation Breakup-----------------
                        //-----Closing Balance Row------ 
                        if (clInd == 0)
                        {
                            if (dtMain.Rows[i]["ClosingBalanceDr"] == DBNull.Value || dtMain.Rows[i]["ClosingBalanceDr"] == "")
                                dtMain.Rows[i]["ClosingBalanceDr"] = "0";
                            if (dtMain.Rows[i]["ClosingBalancecr"] == DBNull.Value || dtMain.Rows[i]["ClosingBalancecr"] == "")
                                dtMain.Rows[i]["ClosingBalancecr"] = "0";
                            if (dtMain.Rows[i]["ClosingTotal"] == DBNull.Value || dtMain.Rows[i]["ClosingTotal"] == "")
                                dtMain.Rows[i]["ClosingTotal"] = "0";
                            SubHTML = SubHTML + "<tr style=\"background-color:#EEE0E5;font-weight:bold;\"><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td style=\"font-weight:bold;\">Closing Balance</td><td>&nbsp;</td><td>&nbsp;</td><td align=\"right\"  style=\"font-weight:bold;\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["ClosingBalanceDr"].ToString())) + "</td><td  align=\"right\" style=\"font-weight:bold;\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["ClosingBalanceCr"].ToString())) + "</td><td  align=\"right\" style=\"font-weight:bold;\">&nbsp;" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["ClosingTotal"].ToString())) + "</td></tr>";
                            clInd = 1;

                        }
                        //-----End of Closing Balance Row------
                        DataRow Row4 = dtMail.NewRow();
                        Row4[0] = dtMain.Rows[i]["SubID"].ToString();
                        Row4[1] = dtMain.Rows[i]["BranchCode"].ToString();
                        Row4[2] = strHTML + " " + SubHTML + "</table>";
                        Row4[3] = dtMain.Rows[i]["SubLedgerType"].ToString();
                        dtMail.Rows.Add(Row4);
                        SubHTML = "";
                        clInd = 0;
                        opInd = 0;

                    }

                }

                SegmentName = "";
                SegID = "";
                CustomerID = "";
                SettlementNo = "";
                SettlementType = "";
                SettlementNOType = "";
                TransactionDate = "";
                IsConsolidate = "";
                TotOblg = 0;
                TotChrg = 0;
                TotNet = 0;
            }

            ViewState["EmailTable"] = dtMail;
        }

        protected string getFormattedDecvalue(decimal Value)
        {
            System.Globalization.NumberFormatInfo currencyFormat = new System.Globalization.CultureInfo("hi-IN").NumberFormat;
            currencyFormat.CurrencySymbol = "";
            currencyFormat.CurrencyNegativePattern = 2;
            //currencyFormat.NumberDecimalDigits = 4;
            currencyFormat.CurrencyDecimalDigits = 4;
            if (Value < 0)
                Value = Value * -1;

            return Value.ToString("c", currencyFormat);

        }

        #region Email
        string mailbodycreatebygrid()
        {
            //DataTable OpenBalance = new DataTable();
            decimal receipt = 0;
            decimal Payment = 0;
            DateTime TranDate = DateTime.Today;
            System.Globalization.NumberFormatInfo currencyFormat = new System.Globalization.CultureInfo("hi-IN").NumberFormat;
            currencyFormat.CurrencySymbol = "";
            currencyFormat.CurrencyNegativePattern = 2;
            pageSize = 25;
            grdCashBankBook.PageSize = pageSize;
            ViewState["dtCashBankBook"] = dtCashBankBook;
            ViewState["dtLedgerView"] = dtLedgerView;
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
                //newRow[3] = dtCashBankBook.Rows[i]["SegmentName"];
                newRow[4] = dtCashBankBook.Rows[i]["AccountName"];
                newRow[5] = dtCashBankBook.Rows[i]["Accountsledger_AmountCr"];
                newRow[6] = dtCashBankBook.Rows[i]["Accountsledger_AmountDr"];
                newRow[7] = dtCashBankBook.Rows[i]["Closing"];
                newRow[8] = dtCashBankBook.Rows[i]["accountsledger_transactiondate"];
                newRow[9] = dtCashBankBook.Rows[i]["accountsledger_InstrumentNumber"];
                if (dtCashBankBook.Rows[i]["SettlementNumber"].ToString().Contains("F"))
                    newRow[10] = "";
                else
                    newRow[10] = dtCashBankBook.Rows[i]["SettlementNumber"];
                newRow[11] = dtCashBankBook.Rows[i]["MainID"];
                newRow[12] = dtCashBankBook.Rows[i]["SubID"];
                newRow[13] = dtCashBankBook.Rows[i]["CompanyID"];
                newRow[14] = dtCashBankBook.Rows[i]["SegID"];
                newRow[15] = dtCashBankBook.Rows[i]["CashType"];
                if (dtCashBankBook.Rows[i]["SegmentName"].ToString().Contains("CM"))
                    newRow[16] = dtCashBankBook.Rows[i]["PayoutDate"];
                else
                    newRow[16] = dtCashBankBook.Rows[i]["TrDate"];
                newRow[17] = dtCashBankBook.Rows[i]["BranchCode"];
                newRow[18] = dtCashBankBook.Rows[i]["UserID"];
                newRow[19] = dtCashBankBook.Rows[i]["SegmentName"];
                dtCashBankBook_New.Rows.Add(newRow);
                if ((dtCashBankBook.Rows[i]["accountsledger_transactiondate"].ToString() != "") || (dtCashBankBook.Rows[i]["accountsledger_transactiondate"].ToString() != "NA"))
                    TranDate = Convert.ToDateTime(dtCashBankBook.Rows[i]["accountsledger_transactiondate"].ToString());
                else
                    TranDate = Convert.ToDateTime("1900-01-01");
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
            ViewState["dtCashBankBook"] = dtCashBankBook;
            grdCashBankBook.DataSource = dtCashBankBook;
            grdCashBankBook.DataBind();

            DisplayOblg.Visible = false;
            grdCashBankBook.Visible = true;



            if (Session["userlastsegment"].ToString() == "7" || Session["userlastsegment"].ToString() == "18")
                grdCashBankBook.Columns[5].Visible = true;
            else
                grdCashBankBook.Columns[5].Visible = false;
            if (Session["userlastsegment"].ToString() == "7" || Session["userlastsegment"].ToString() == "8" || Session["userlastsegment"].ToString() == "18")
                grdCashBankBook.Columns[6].Visible = true;
            else
                grdCashBankBook.Columns[6].Visible = false;
            if (radConsolidated.Checked == true)
            {
                if (radDateWise.Checked == true)
                {
                    grdCashBankBook.Columns[2].Visible = true;
                }
                else
                {
                    grdCashBankBook.Columns[2].Visible = false;
                }

            }
            else
            {
                grdCashBankBook.Columns[2].Visible = true;
            }
            if (Session["userlastsegment"].ToString() == "8")
                grdCashBankBook.Columns[7].Visible = false;
            else
                grdCashBankBook.Columns[7].Visible = true;
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
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            grdCashBankBook.RenderControl(hw);
            return sb.ToString();
        }
        protected void btnSend_Click(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct3", "HideOnOffLoading()", true);
            string SelectedSegmentName = null;
            DataTable CompanySegmentDeatil = null;
            oGenericMethod = new BusinessLogicLayer.GenericMethod();
            string ReportType = "";
            string SubAc = "";
            string mainAccountSearch = null;
            string SubAccountSearch = null;
            string SubACountForAll = null;
            string BranchGroupInd = "";
            ViewState["Clients"] = null;
            ViewState["branchID"] = null;
            SubLedgerType = HdnSubLedgerType.Value;
            SegmentT = HdnBranch.Value;
            SubAcID = HdnSubAc.Value;
            ViewState["SubAcID"] = SubAcID;
            MainAcID = HdnMainAc.Value;
            if (SubLedgerType == "CDSL Clients" || SubLedgerType == "NSDL Clients")
                fn_ClientCDSL();
            else if (HdnForBranchGroup.Value != "a")
                fn_Client();

            string Settlement = "";
            if (RadSettA.Checked == true)
            {
                Settlement = "";
            }
            else
            {
                Settlement = HdnSettlement.Value;
            }
            SegMentName = ViewState["SegMentName"].ToString();
            ViewState["Check"] = "a";
            if (rdbSegAll.Checked == true)
            {
                DataTable dtSegment = oDBEngine.GetDataTable("tbl_master_companyExchange", "exch_internalId", "exch_compId='" + Session["CompanyID"].ToString() + "'");
                if (dtSegment.Rows.Count > 0)
                {
                    for (int i = 0; i < dtSegment.Rows.Count; i++)
                    {
                        if (Segment == null)
                            Segment = dtSegment.Rows[i][0].ToString();
                        else
                            Segment += "," + dtSegment.Rows[i][0].ToString();
                    }
                }
            }
            else
            {
                if (SegmentT == null || SegmentT == "")
                {
                    Segment = Session["SegmentID"].ToString();
                }
                else
                    Segment = SegmentT;
            }

            //Get Segment Name
            CompanySegmentDeatil = oGenericMethod.GetExchangeSegmentName(Segment);
            foreach (DataRow dr in CompanySegmentDeatil.Rows)
            {
                if (SelectedSegmentName == null)
                    SelectedSegmentName = dr[0].ToString().Trim();
                else
                    SelectedSegmentName = SelectedSegmentName + "," + dr[0].ToString().Trim();
            }


            if (ViewState["branchID"] == null)
            {
                if (rdbranchAll.Checked == true)
                {
                    Branch = Session["userbranchHierarchy"].ToString();
                }
            }
            else
                Branch = ViewState["branchID"].ToString();


            if (rdSubAcAll.Checked == true)
            {
                SubACountForAll = null;
            }
            else
            {
                if (SubAcID == null || SubAcID == "")
                    SubACountForAll = null;
                else
                    SubACountForAll = SubAcID;
            }
            if (ViewState["Clients"] != null)
                SubACountForAll = ViewState["Clients"].ToString();
            //if (ddlAccountType.SelectedValue == "0")
            //{
            //    mainAccountSearch = "'SYSTM00001'";
            //    SubLedgerType = "Customers";
            //}
            //else if (ddlAccountType.SelectedValue == "1")
            //{
            //    mainAccountSearch = "'SYSTM00002'";
            //    SubLedgerType = "Customers";

            //}
            //else if (ddlAccountType.SelectedValue == "2")
            //{
            //    mainAccountSearch = "'SYSTM00001','SYSTM00002'";
            //    SubLedgerType = "Customers";
            //}
            //else
            //{
            //    mainAccountSearch = MainAcID;
            //}
            if (ddlAccountType.SelectedValue == "0")
            {
                if (MainAcID == "" || MainAcID == null)
                {
                    mainAccountSearch = "'SYSTM00001'";
                }
                else
                {
                    mainAccountSearch = MainAcID;
                }
                if (SubLedgerType == "")
                    SubLedgerType = "Customers";

            }
            else if (ddlAccountType.SelectedValue == "1")
            {
                if (MainAcID == "" || MainAcID == null)
                {
                    mainAccountSearch = "'SYSTM00002'";
                }
                else
                {
                    mainAccountSearch = MainAcID;
                }
                if (SubLedgerType == "")
                    SubLedgerType = "Customers";
            }
            else if (ddlAccountType.SelectedValue == "2")
            {

                if (MainAcID == "" || MainAcID == null)
                {
                    mainAccountSearch = "'SYSTM00001','SYSTM00002'";
                }
                else
                {
                    mainAccountSearch = MainAcID;
                }

                if (SubLedgerType == "")
                    SubLedgerType = "Customers";
            }
            else
            {
                mainAccountSearch = MainAcID;
            }
            ViewState["MainAcIDforOp"] = MainAcIDforOp;
            ViewState["Segment"] = Segment;


            if (SubLedgerType.Trim() == "None")
            {
                SubAccountSearch = null;
                ViewState["SubAccountSearch"] = SubAccountSearch;
            }
            else
            {
                if (ViewState["Clients"] == null)
                {
                    SubAccountSearch = "";
                }
                else
                {
                    SubAccountSearch = ViewState["Clients"].ToString();
                }


            }

            if (radBreakDetail.Checked == true)
            {
                ReportType = "ObligationBrkUp";

            }
            else if (radConsolidated.Checked == true)
            {
                if (radDateWise.Checked == true)
                    ReportType = "DateWise";
                else if (radVoucherWise.Checked == true)
                    ReportType = "VoucherWise";
                else if (radExpDateWise.Checked == true)
                    //{

                    ReportType = "ConsolidateExp~" + litSegment.InnerText.Split('-')[1].Replace("'", "").Trim();
                //}
                //else
                //    ReportType = "DateWise";

            }
            else
            {
                ReportType = "Detail";

            }

            if (SubAccountSearch == "")
            {
                SubAccountSearch = " ";
            }

            string SingleDouble = null;
            if (chkBothPrint.Checked == true)
                SingleDouble = "D";
            else
                SingleDouble = "S";

            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                BranchGroupInd = "G";
            }
            else
            {
                BranchGroupInd = "B";
            }

            string strTranType = "";
            string strCbPayment = "n";
            string strCbReceipt = "n";
            string strCbContract = "n";
            string strJvType = "";
            string strSelectedJv = "";



            if (rbTanAll.Checked == true)
                strTranType = "all";
            else if (rbTranCashBank.Checked == true)
            {
                strTranType = "cb";
                if (chkPayment.Checked == true)
                    strCbPayment = "y";
                if (chkReceipts.Checked == true)
                    strCbReceipt = "y";
                if (chkContracts.Checked == true)
                    strCbContract = "y";

            }
            else if (rbTranJv.Checked == true)
            {
                strTranType = "jv";
                if (rbAllJV.Checked == true)
                    strJvType = "all";
                else if (rbManualJV.Checked == true)
                {
                    strJvType = "man";
                    strSelectedJv = txtVoucherPrefix.Text;
                }
                else if (rbSystemJV.Checked == true)
                {
                    strJvType = "sys";
                    //strSelectedJv = ViewState["Selectedjvs"].ToString();
                    if (rbSystemJVAll.Checked == true)
                        strSelectedJv = "all";
                    else if (rbSystemJVSelected.Checked == true)
                        strSelectedJv = hdnSystemJvs.Value;
                }

            }

            string strSqlCb = "";
            string strSqlJv = "";

            if (strTranType == "cb")
            {
                strSqlCb = " and  AccountsLedger_TransactionType='Cash_Bank' ";
                if (strCbPayment == "y" && strCbReceipt != "y" && strCbContract != "y")
                    strSqlCb = strSqlCb + " and patindex('%/%',AccountsLedger_TransactionReferenceID)>0 and  left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1)='P' ";

                if (strCbPayment != "y" && strCbReceipt == "y" && strCbContract != "y")
                    strSqlCb = strSqlCb + " and patindex('%/%',AccountsLedger_TransactionReferenceID)>0 and left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1)='R' ";

                if (strCbPayment != "y" && strCbReceipt != "y" && strCbContract == "y")
                    strSqlCb = strSqlCb + " and patindex('%/%',AccountsLedger_TransactionReferenceID)>0 and left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1)='C' ";

                if (strCbPayment == "y" && strCbReceipt == "y" && strCbContract != "y")
                    strSqlCb = strSqlCb + "  and patindex('%/%',AccountsLedger_TransactionReferenceID)>0 and  (left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1)='P' or left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1)='R') ";
                if (strCbPayment != "y" && strCbReceipt == "y" && strCbContract == "y")
                    strSqlCb = strSqlCb + " and patindex('%/%',AccountsLedger_TransactionReferenceID)>0 and (left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1)='R' or left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1)='C') ";
                if (strCbPayment == "y" && strCbReceipt != "y" && strCbContract == "y")
                    strSqlCb = strSqlCb + " and patindex('%/%',AccountsLedger_TransactionReferenceID)>0 and  (left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1)='P' or left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1)='C') ";
                if (strCbPayment == "y" && strCbReceipt == "y" && strCbContract == "y")
                    strSqlCb = strSqlCb + " and patindex('%/%',AccountsLedger_TransactionReferenceID)>0 and  (left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1)='P' or left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1)='R' or left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1)='C') ";
                if (strCbPayment != "y" && strCbReceipt != "y" && strCbContract != "y")
                    strSqlCb = strSqlCb + "  and patindex('%/%',AccountsLedger_TransactionReferenceID)>0 and  (left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1)!='P' and left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1)!='R' and left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1)!='C') ";


            }
            if (strTranType == "jv")
            {
                if (strJvType == "all")
                    strSqlJv = " and  AccountsLedger_TransactionType='Journal' ";
                else if (strJvType == "man")
                    if (strSelectedJv == "??")
                    {
                        strSqlJv = " and  patindex('%/%',AccountsLedger_TransactionReferenceID)>0 and AccountsLedger_TransactionType='Journal' and left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1) not like 'u%' and left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1) not like 'v%' and left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1) not like 'w%' and left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1) not like 'x%' and left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1) not like 'y%' and left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1) not like 'z%' ";

                    }
                    else
                        strSqlJv = " and  patindex('%/%',AccountsLedger_TransactionReferenceID)>0 and AccountsLedger_TransactionType='Journal' and left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1)='" + strSelectedJv + "' ";
                else if (strJvType == "sys")
                    if (strSelectedJv == "all")
                    {
                        strSqlJv = " and  patindex('%/%',AccountsLedger_TransactionReferenceID)>0 and AccountsLedger_TransactionType='Journal' and (left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1) like 'u%' or left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1) like 'v%' or left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1) like 'w%' or left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1) like 'w%' or left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1) like 'x%' or left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1) like 'y%' or left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1) like 'z%' )";

                    }
                    else
                        strSqlJv = " and  patindex('%/%',AccountsLedger_TransactionReferenceID)>0 and AccountsLedger_TransactionType='Journal' and left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1) in(" + strSelectedJv + ") ";


            }

            DataSet ds = new DataSet();
            DataTable dtorderbyclient = new DataTable();
            dtorderbyclient = oDBEngine.GetDataTable("declare @CustomerID varchar(max) select @CustomerID=coalesce(@CustomerID+''',''', '') + Convert(varchar,cnt_internalId) from (Select Distinct cnt_internalId,cnt_branchid from tbl_master_contact WHERE  cnt_internalId in (" + SubAccountSearch.ToString() + ")) rr order by rr.cnt_branchid set @CustomerID=''''+@CustomerID+'''' select @CustomerID");
            //string[] totalsubaccount = SubAccountSearch.Split(',');//.ToString();
            string[] totalsubaccount = dtorderbyclient.Rows[0][0].ToString().Split(',');
            string filterclient = "";

            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            {

                if (ReportType.Contains("ConsolidateExp"))
                {
                    string finalmailbody = "";
                    string billdate = "";
                    string emailbdy = "";
                    string contactid = "";
                    string Subject = "";
                    string Type = "";
                    if (ResBranch.Checked == false)
                    {
                        for (int m = 0; m < Convert.ToInt32(totalsubaccount.Length.ToString()); m++)
                        {
                            filterclient = totalsubaccount[m].ToString();

                            ds = objFAReportsOther.Fetch_LedgerView(
                                Convert.ToString(Session["LastCompany"]),
                                Convert.ToString(Session["LastFinYear"]),
                                Convert.ToString(dtFrom.Value),
                                Convert.ToString(dtTo.Value),
                                Convert.ToString(mainAccountSearch),
                                Convert.ToString(filterclient),
                                Convert.ToString(Branch),
                                Convert.ToString(ReportType),
                                 Convert.ToString(Segment),
                                 Convert.ToString(Session["userid"]),
                                Convert.ToString(Settlement),
                                Convert.ToString(strTranType),
                               Convert.ToString(strCbPayment),
                               Convert.ToString(strCbReceipt),
                               Convert.ToString(strCbContract),
                               Convert.ToString(strJvType),
                               Convert.ToString(strSelectedJv),
                                Session["ActiveCurrency"].ToString().Split('~')[0],
                                Session["TradeCurrency"].ToString().Split('~')[0]);
                            dtCashBankBook = ds.Tables[0];
                            dtLedgerView = ds.Tables[0];
                            OpenBalance = ds.Tables[1];
                            ViewState["dsEmail"] = ds;

                            if (ds.Tables.Count > 0)
                            {
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    finalmailbody = mailbodycreatebygrid();

                                    if (ddlAccountType.SelectedItem.Value == "0")
                                    {
                                        Type = "Clients - Trading A/c  ";
                                    }
                                    else if (ddlAccountType.SelectedItem.Value == "1")
                                    {
                                        Type = "Clients - Margin Deposit A/c  ";
                                    }
                                    else if (ddlAccountType.SelectedItem.Value == "2")
                                    {
                                        Type = "Clients - Trading A/c and Clients - Margin Deposit A/c  ";
                                    }
                                    else if (ddlAccountType.SelectedItem.Value == "3")
                                    {
                                        Type = txtMainAccount.Text;
                                    }



                                    if (ResClient.Checked == true)
                                    {
                                        billdate = " From " + " " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "  To  " + oconverter.ArrangeDate2(dtTo.Value.ToString());
                                        Subject = "[" + SelectedSegmentName + "] : Ledger For : " + Type + "  " + billdate;
                                        emailbdy = finalmailbody.ToString();
                                        contactid = filterclient.Replace("'", "");
                                        if (oDBEngine.SendReport(emailbdy, contactid, billdate, Subject) == true)
                                        {
                                            ScriptManager.RegisterStartupScript(this, this.GetType(), "MailSendT1", "MailsendT();", true);

                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this, this.GetType(), "MailSendF1", "MailsendF();", true);
                                        }
                                    }
                                    DataTable dtsg = oDBEngine.GetDataTable(" tbl_master_segment  ", "*", "seg_id='" + HttpContext.Current.Session["userlastsegment"].ToString() + "'");
                                    string segmentname = dtsg.Rows[0]["seg_name"].ToString();

                                    DataTable dtname = oDBEngine.GetDataTable(" tbl_master_Contact  ", "(isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+' ['+isnull(cnt_UCC,'')+']') as ClientName", "cnt_internalId='" + filterclient.Replace("'", "") + "' ");
                                    string ClientName = dtname.Rows[0]["ClientName"].ToString();
                                    //emailbdy = emailbdy + finalmailbody;
                                    //emailbdy = finalmailbody + "<table width=\"100%\"><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Name:" + ClientName + "</td></tr><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">" + Subject + "</td></tr> <tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Segment: " + segmentname + "</td></tr><tr><td>" + emailbdy + "</td></tr></table>";
                                    emailbdy = emailbdy + "<table width=\"100%\"><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Name:" + ClientName + "</td></tr><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">" + Subject + "</td></tr> <tr><td>" + finalmailbody + "</td></tr></table>";
                                }
                            }
                        }
                        if (ResUser.Checked == true)
                        {

                            billdate = "Period :" + " " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "  - " + oconverter.ArrangeDate2(dtTo.Value.ToString());
                            contactid = HDNEmp.Value;
                            Subject = "[" + SelectedSegmentName + "] : Ledger For " + billdate;
                            if (contactid.ToString() != "")
                            {
                                string[] Cnt = contactid.Split(',');
                                for (int k = 0; k < Cnt.Length; k++)
                                {
                                    if (oDBEngine.SendReportSt(emailbdy, Cnt[k].ToString(), billdate, Subject) == true)
                                    {
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "MailSendT1", "MailsendT();", true);

                                    }
                                    else
                                    {
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "MailSendF1", "MailsendF();", true);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        /////////branch wise mail send for expiry
                        DataTable dtdistinctbranch = oDBEngine.GetDataTable("declare @CustomerID varchar(max) select @CustomerID=coalesce(@CustomerID+',', '') + Convert(varchar,cnt_branchid) from (Select Distinct cnt_branchid from tbl_master_contact WHERE  cnt_internalid in (" + dtorderbyclient.Rows[0][0].ToString() + ")) rr select @CustomerID");
                        string strdistinctbranch = dtdistinctbranch.Rows[0][0].ToString();
                        string[] totalsubaccountbranch = strdistinctbranch.Split(',');//.ToString();
                        string filterclientbranch = "";
                        if (ddlGroup.SelectedItem.Value == "0")
                        {
                            for (int p = 0; p < Convert.ToInt32(totalsubaccountbranch.Length.ToString()); p++)
                            {
                                filterclientbranch = totalsubaccountbranch[p].ToString();

                                for (int m = 0; m < Convert.ToInt32(totalsubaccount.Length.ToString()); m++)
                                {
                                    filterclient = totalsubaccount[m].ToString();
                                    DataTable dtfilterclientbranch = oDBEngine.GetDataTable("select cnt_branchid from tbl_master_contact where cnt_internalid=" + filterclient.ToString().Trim() + "");
                                    if (dtfilterclientbranch.Rows[0][0].ToString().Trim() == filterclientbranch.ToString().Trim())
                                    {

                                        ds = objFAReportsOther.Fetch_LedgerView(
                                            Convert.ToString(Session["LastCompany"]),
                                            Convert.ToString(Session["LastFinYear"]),
                                            Convert.ToString(dtFrom.Value),
                                            Convert.ToString(dtTo.Value),
                                            Convert.ToString(mainAccountSearch),
                                            Convert.ToString(filterclient),
                                            Convert.ToString(Branch),
                                            Convert.ToString(ReportType),
                                             Convert.ToString(Segment),
                                             Convert.ToString(Session["userid"]),
                                            Convert.ToString(Settlement),
                                            Convert.ToString(strTranType),
                                           Convert.ToString(strCbPayment),
                                           Convert.ToString(strCbReceipt),
                                           Convert.ToString(strCbContract),
                                           Convert.ToString(strJvType),
                                           Convert.ToString(strSelectedJv),
                                            Session["ActiveCurrency"].ToString().Split('~')[0],
                                            Session["TradeCurrency"].ToString().Split('~')[0]);
                                        dtCashBankBook = ds.Tables[0];
                                        dtLedgerView = ds.Tables[0];
                                        OpenBalance = ds.Tables[1];
                                        ViewState["dsEmail"] = ds;

                                        if (ds.Tables.Count > 0)
                                        {
                                            if (ds.Tables[0].Rows.Count > 0)
                                            {
                                                DataTable dtfilterbranch = oDBEngine.GetDataTable("select ltrim(rtrim(branch_code)),branch_cpemail from tbl_master_branch where branch_id=" + filterclientbranch + "");
                                                if (dtfilterbranch.Rows[0][0].ToString().Trim().ToUpper() == ds.Tables[0].Rows[0]["BranchCode"].ToString().Trim().ToUpper())
                                                {




                                                    finalmailbody = mailbodycreatebygrid();


                                                    if (ddlAccountType.SelectedItem.Value == "0")
                                                    {
                                                        Type = "Clients - Trading A/c  ";
                                                    }
                                                    else if (ddlAccountType.SelectedItem.Value == "1")
                                                    {
                                                        Type = "Clients - Margin Deposit A/c  ";
                                                    }
                                                    else if (ddlAccountType.SelectedItem.Value == "2")
                                                    {
                                                        Type = "Clients - Trading A/c and Clients - Margin Deposit A/c  ";
                                                    }
                                                    else if (ddlAccountType.SelectedItem.Value == "3")
                                                    {
                                                        Type = txtMainAccount.Text;
                                                    }
                                                    DataTable dtsg = oDBEngine.GetDataTable(" tbl_master_segment  ", "*", "seg_id='" + HttpContext.Current.Session["userlastsegment"].ToString() + "'");
                                                    string segmentname = dtsg.Rows[0]["seg_name"].ToString();

                                                    DataTable dtname = oDBEngine.GetDataTable(" tbl_master_Contact  ", "(isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+' ['+isnull(cnt_UCC,'')+']') as ClientName", "cnt_internalId='" + filterclient.Replace("'", "") + "' ");
                                                    string ClientName = dtname.Rows[0]["ClientName"].ToString();
                                                    //emailbdy = emailbdy + finalmailbody;
                                                    //emailbdy = finalmailbody + "<table width=\"100%\"><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Name:" + ClientName + "</td></tr><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">" + Subject + "</td></tr> <tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Segment: " + segmentname + "</td></tr><tr><td>" + emailbdy + "</td></tr></table>";
                                                    emailbdy = emailbdy + "<table width=\"100%\"><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Name:" + ClientName + "</td></tr><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">" + Subject + "</td></tr> <tr><td>" + finalmailbody + "</td></tr></table>";
                                                }
                                                else
                                                {
                                                    billdate = "Period :" + " " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "  - " + oconverter.ArrangeDate2(dtTo.Value.ToString());
                                                    contactid = HDNEmp.Value;
                                                    Subject = "[" + SelectedSegmentName + "] : Ledger For " + billdate;
                                                    if (oDBEngine.SendReportBr(emailbdy, dtfilterbranch.Rows[0][1].ToString(), billdate, Subject, filterclientbranch.ToString()) == true)
                                                    {
                                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "MailSendT", "MailsendT();", true);

                                                    }
                                                    else
                                                    {
                                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "MailSendF", "MailsendF();", true);

                                                    }
                                                    break;
                                                }

                                            }

                                        }
                                    }
                                    else
                                    {
                                        if (emailbdy != "")
                                        {
                                            billdate = "Period :" + " " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "  - " + oconverter.ArrangeDate2(dtTo.Value.ToString());
                                            contactid = HDNEmp.Value;
                                            Subject = "[" + SelectedSegmentName + "] : Ledger For " + billdate;
                                            DataTable dtemail = oDBEngine.GetDataTable("select branch_cpemail from tbl_master_branch where branch_id=" + filterclientbranch + "");
                                            if (oDBEngine.SendReportBr(emailbdy, dtemail.Rows[0][0].ToString(), billdate, Subject, filterclientbranch.ToString()) == true)
                                            {
                                                emailbdy = "";
                                                ScriptManager.RegisterStartupScript(this, this.GetType(), "MailSendT", "MailsendT();", true);

                                            }
                                            else
                                            {
                                                ScriptManager.RegisterStartupScript(this, this.GetType(), "MailSendF", "MailsendF();", true);

                                            }
                                            break;
                                        }
                                    }

                                }
                            }////branch loop end
                            if (emailbdy != "")
                            {
                                billdate = "Period :" + " " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "  - " + oconverter.ArrangeDate2(dtTo.Value.ToString());
                                contactid = HDNEmp.Value;
                                Subject = "[" + SelectedSegmentName + "] : Ledger For " + billdate;
                                DataTable dtemail = oDBEngine.GetDataTable("select branch_cpemail from tbl_master_branch where branch_id=" + filterclientbranch + "");
                                if (oDBEngine.SendReportBr(emailbdy, dtemail.Rows[0][0].ToString(), billdate, Subject, filterclientbranch.ToString()) == true)
                                {
                                    emailbdy = "";
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "MailSendT", "MailsendT();", true);

                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "MailSendF", "MailsendF();", true);

                                }

                            }
                        }
                        else//////////////Groupwise mail sending start
                        {
                            //dtdistinctbranch = oDBEngine.GetDataTable("select distinct gpm_id,ISNULL(gpm_emailID,'') from tbl_trans_group,tbl_master_groupMaster where grp_groupMaster=gpm_id and grp_groupType=gpm_Type and gpm_Type='" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "' and grp_groupType='" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "' and grp_contactId in (" + dtorderbyclient.Rows[0][0].ToString() + ") and gpm_emailID is not null ");
                            dtdistinctbranch = oDBEngine.GetDataTable("declare @CustomerID varchar(max) select @CustomerID=coalesce(@CustomerID+',', '') + Convert(varchar,gpm_id) from (Select Distinct gpm_id from tbl_trans_group,tbl_master_groupMaster where grp_groupMaster=gpm_id and grp_groupType=gpm_Type and gpm_Type='" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "' and grp_groupType='" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "' and grp_contactId in (" + dtorderbyclient.Rows[0][0].ToString() + ") and gpm_emailID is not null) rr select @CustomerID");
                            strdistinctbranch = dtdistinctbranch.Rows[0][0].ToString();
                            totalsubaccountbranch = strdistinctbranch.Split(',');//.ToString();
                            filterclientbranch = "";
                            for (int p = 0; p < Convert.ToInt32(totalsubaccountbranch.Length.ToString()); p++)
                            {
                                filterclientbranch = totalsubaccountbranch[p].ToString();

                                for (int m = 0; m < Convert.ToInt32(totalsubaccount.Length.ToString()); m++)
                                {
                                    filterclient = totalsubaccount[m].ToString();
                                    DataTable dtfilterclientbranch = oDBEngine.GetDataTable("select grp_groupMaster from tbl_trans_group where grp_contactId=" + filterclient.ToString().Trim() + " and grp_groupType='" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "'");
                                    if (dtfilterclientbranch.Rows[0][0].ToString().Trim() == filterclientbranch.ToString().Trim())
                                    {
                                        //For Sever Debugger Variable
                                        string[,] strParam = new string[19, 2];
                                        string SpName = String.Empty;

                                        //SD Code (Server Debugging Code)
                                        strParam[0, 0] = "CompanyID"; strParam[0, 1] = "'" + Session["LastCompany"].ToString() + "'";
                                        strParam[1, 0] = "FinYear"; strParam[1, 1] = "'" + Session["LastFinYear"].ToString() + "'";
                                        strParam[2, 0] = "FromDate"; strParam[2, 1] = "'" + dtFrom.Value + "'";
                                        strParam[3, 0] = "ToDate"; strParam[3, 1] = "'" + dtTo.Value + "'";
                                        strParam[4, 0] = "MainAccount"; strParam[4, 1] = "'" + mainAccountSearch + "'";
                                        strParam[5, 0] = "SubAccount"; strParam[5, 1] = "'" + filterclient + "'";
                                        strParam[6, 0] = "Branch"; strParam[6, 1] = "'" + Branch + "'";
                                        strParam[7, 0] = "ReportType"; strParam[7, 1] = "'" + ReportType + "'";
                                        strParam[8, 0] = "Segment"; strParam[8, 1] = "'" + Segment + "'";
                                        strParam[9, 0] = "UserID"; strParam[9, 1] = "'" + Session["userid"].ToString() + "'";
                                        strParam[10, 0] = "Settlement"; strParam[10, 1] = "'" + Settlement + "'";
                                        strParam[11, 0] = "TranType"; strParam[11, 1] = "'" + strTranType + "'";
                                        strParam[12, 0] = "CbPayment"; strParam[12, 1] = "'" + Convert.ToChar(strCbPayment) + "'";
                                        strParam[13, 0] = "CbReceipt"; strParam[13, 1] = "'" + Convert.ToChar(strCbReceipt) + "'";
                                        strParam[14, 0] = "CbContract"; strParam[14, 1] = "'" + Convert.ToChar(strCbContract) + "'";
                                        strParam[15, 0] = "JvType"; strParam[15, 1] = "'" + strJvType + "'";
                                        strParam[16, 0] = "SelectedJv"; strParam[16, 1] = "'" + strSelectedJv + "'";
                                        strParam[17, 0] = "ActiveCurrency"; strParam[17, 1] = "'" + Session["ActiveCurrency"].ToString().Split('~')[0] + "'";
                                        strParam[18, 0] = "TradeCurrency"; strParam[18, 1] = "'" + Session["TradeCurrency"].ToString().Split('~')[0] + "'";

                                        //For Server Debugging Purpose
                                        oGenericMethod = new BusinessLogicLayer.GenericMethod();
                                        if (oGenericMethod.CallGeneric_ScalerFunction_Int("GetGlobalSettingsValue", Session["UserSegID"].ToString() + "~GS_DEBUGSTATE") == 1)
                                        {
                                            string strDateTime = oGenericMethod.GetDate().ToString("yyyyMMddHHmmss");
                                            string FilePath = "../ExportFiles/ServerDebugging/Fetch_LedgerView" + strDateTime + ".txt";
                                            oGenericMethod.WriteFile(oGenericMethod.OldSpExecuteWriter(strParam, "Fetch_LedgerView"), FilePath, false);
                                        }



                                        ds = objFAReportsOther.Fetch_LedgerView(
                                            Convert.ToString(Session["LastCompany"]),
                                            Convert.ToString(Session["LastFinYear"]),
                                            Convert.ToString(dtFrom.Value),
                                            Convert.ToString(dtTo.Value),
                                            Convert.ToString(mainAccountSearch),
                                            Convert.ToString(filterclient),
                                            Convert.ToString(Branch),
                                            Convert.ToString(ReportType),
                                             Convert.ToString(Segment),
                                             Convert.ToString(Session["userid"]),
                                            Convert.ToString(Settlement),
                                            Convert.ToString(strTranType),
                                           Convert.ToString(strCbPayment),
                                           Convert.ToString(strCbReceipt),
                                           Convert.ToString(strCbContract),
                                           Convert.ToString(strJvType),
                                           Convert.ToString(strSelectedJv),
                                            Session["ActiveCurrency"].ToString().Split('~')[0],
                                            Session["TradeCurrency"].ToString().Split('~')[0]);
                                        dtCashBankBook = ds.Tables[0];
                                        dtLedgerView = ds.Tables[0];
                                        OpenBalance = ds.Tables[1];
                                        ViewState["dsEmail"] = ds;

                                        if (ds.Tables.Count > 0)
                                        {
                                            if (ds.Tables[0].Rows.Count > 0)
                                            {
                                                DataTable dtfilterbranch = oDBEngine.GetDataTable("select ltrim(rtrim(branch_code)),branch_cpemail from tbl_master_branch where branch_id=" + filterclientbranch + "");
                                                //if (dtfilterbranch.Rows[0][0].ToString().Trim().ToUpper() == ds.Tables[0].Rows[0]["BranchCode"].ToString().Trim().ToUpper())
                                                //{




                                                finalmailbody = mailbodycreatebygrid();


                                                if (ddlAccountType.SelectedItem.Value == "0")
                                                {
                                                    Type = "Clients - Trading A/c  ";
                                                }
                                                else if (ddlAccountType.SelectedItem.Value == "1")
                                                {
                                                    Type = "Clients - Margin Deposit A/c  ";
                                                }
                                                else if (ddlAccountType.SelectedItem.Value == "2")
                                                {
                                                    Type = "Clients - Trading A/c and Clients - Margin Deposit A/c  ";
                                                }
                                                else if (ddlAccountType.SelectedItem.Value == "3")
                                                {
                                                    Type = txtMainAccount.Text;
                                                }
                                                DataTable dtsg = oDBEngine.GetDataTable(" tbl_master_segment  ", "*", "seg_id='" + HttpContext.Current.Session["userlastsegment"].ToString() + "'");
                                                string segmentname = dtsg.Rows[0]["seg_name"].ToString();

                                                DataTable dtname = oDBEngine.GetDataTable(" tbl_master_Contact  ", "(isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+' ['+isnull(cnt_UCC,'')+']') as ClientName", "cnt_internalId='" + filterclient.Replace("'", "") + "' ");
                                                string ClientName = dtname.Rows[0]["ClientName"].ToString();
                                                //emailbdy = emailbdy + finalmailbody;
                                                //emailbdy = finalmailbody + "<table width=\"100%\"><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Name:" + ClientName + "</td></tr><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">" + Subject + "</td></tr> <tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Segment: " + segmentname + "</td></tr><tr><td>" + emailbdy + "</td></tr></table>";
                                                emailbdy = emailbdy + "<table width=\"100%\"><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Name:" + ClientName + "</td></tr><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">" + Subject + "</td></tr> <tr><td>" + finalmailbody + "</td></tr></table>";
                                                //}
                                                //else
                                                //{
                                                //    billdate = "Period :" + " " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "  - " + oconverter.ArrangeDate2(dtTo.Value.ToString());
                                                //    contactid = HDNEmp.Value;
                                                //    Subject = "Ledger For " + billdate;
                                                //    if (oDBEngine.SendReportBr(emailbdy, dtfilterbranch.Rows[0][1].ToString(), billdate, Subject, filterclientbranch.ToString()) == true)
                                                //    {
                                                //        ScriptManager.RegisterStartupScript(this, this.GetType(), "MailSendT", "MailsendT();", true);

                                                //    }
                                                //    else
                                                //    {
                                                //        ScriptManager.RegisterStartupScript(this, this.GetType(), "MailSendF", "MailsendF();", true);

                                                //    }
                                                //    break;
                                                //}

                                            }

                                        }
                                    }
                                    else
                                    {
                                        if (emailbdy != "")
                                        {
                                            billdate = "Period :" + " " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "  - " + oconverter.ArrangeDate2(dtTo.Value.ToString());
                                            contactid = HDNEmp.Value;
                                            Subject = "[" + SelectedSegmentName + "] : Ledger For " + billdate;
                                            DataTable dtemail = oDBEngine.GetDataTable("select gpm_emailID from tbl_master_groupMaster where gpm_id=" + filterclientbranch + "");
                                            if (oDBEngine.SendReportBr(emailbdy, dtemail.Rows[0][0].ToString(), billdate, Subject, filterclientbranch.ToString()) == true)
                                            {
                                                emailbdy = "";
                                                ScriptManager.RegisterStartupScript(this, this.GetType(), "MailSendT", "MailsendT();", true);

                                            }
                                            else
                                            {
                                                ScriptManager.RegisterStartupScript(this, this.GetType(), "MailSendF", "MailsendF();", true);

                                            }
                                            break;
                                        }
                                    }

                                }
                                if (emailbdy != "")
                                {
                                    billdate = "Period :" + " " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "  - " + oconverter.ArrangeDate2(dtTo.Value.ToString());
                                    contactid = HDNEmp.Value;
                                    Subject = "[" + SelectedSegmentName + "] : Ledger For " + billdate;
                                    DataTable dtemail = oDBEngine.GetDataTable("select gpm_emailID from tbl_master_groupMaster where gpm_id=" + filterclientbranch + "");
                                    if (oDBEngine.SendReportBr(emailbdy, dtemail.Rows[0][0].ToString(), billdate, Subject, filterclientbranch.ToString()) == true)
                                    {
                                        emailbdy = "";
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "MailSendT", "MailsendT();", true);

                                    }
                                    else
                                    {
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "MailSendF", "MailsendF();", true);

                                    }

                                }
                            }////Group loop end
                            if (emailbdy != "")
                            {
                                billdate = "Period :" + " " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "  - " + oconverter.ArrangeDate2(dtTo.Value.ToString());
                                contactid = HDNEmp.Value;
                                Subject = "[" + SelectedSegmentName + "] : Ledger For " + billdate;
                                DataTable dtemail = oDBEngine.GetDataTable("select gpm_emailID from tbl_master_groupMaster where gpm_id=" + filterclientbranch + "");
                                if (oDBEngine.SendReportBr(emailbdy, dtemail.Rows[0][0].ToString(), billdate, Subject, filterclientbranch.ToString()) == true)
                                {
                                    emailbdy = "";
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "MailSendT", "MailsendT();", true);

                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "MailSendF", "MailsendF();", true);

                                }

                            }
                        }
                    }
                }


                else
                {
                    DataSet dsCnt = new DataSet();
                    dsCnt = objFAReportsOther.Fetch_LedgerForSendEmail(
                        Convert.ToString(Session["LastCompany"]),
                        Convert.ToString(Session["LastFinYear"]),
                         Convert.ToString(dtFrom.Value),
                         Convert.ToString(dtTo.Value),
                        Convert.ToString(mainAccountSearch),
                        Convert.ToString(SubAccountSearch),
                        Convert.ToString(Branch),
                        Convert.ToString(ReportType),
                        Convert.ToString(Segment),
                        Convert.ToString(Session["userid"]),
                        Convert.ToString(SubLedgerType.Trim()),
                        Convert.ToString(Settlement),
                       Convert.ToString(BranchGroupInd),
                       Convert.ToString(strTranType),
                       Convert.ToString(strCbPayment),
                      Convert.ToString(strCbReceipt),
                      Convert.ToString(strCbContract),
                    Convert.ToString(strJvType),
                       Convert.ToString(strSelectedJv),
                        Session["ActiveCurrency"].ToString().Split('~')[0],
                        Session["TradeCurrency"].ToString().Split('~')[0]);
                    ViewState["dsEmail"] = dsCnt;

                    GenerateHTMLConsolidate();
                    DataTable dtEmail = (DataTable)ViewState["EmailTable"];
                    string billdate = "";
                    string emailbdy = "";
                    string contactid = "";
                    string Subject = "";
                    string Type = "";
                    DataTable dtCnt = new DataTable();
                    DataTable dtBr = new DataTable();
                    string strClientInfo = null;
                    if (ddlAccountType.SelectedItem.Value == "0")
                    {
                        Type = "Clients - Trading A/c  ";
                    }
                    else if (ddlAccountType.SelectedItem.Value == "1")
                    {
                        Type = "Clients - Margin Deposit A/c  ";
                    }
                    else if (ddlAccountType.SelectedItem.Value == "2")
                    {
                        Type = "Clients - Trading A/c and Clients - Margin Deposit A/c  ";
                    }
                    else if (ddlAccountType.SelectedItem.Value == "3")
                    {
                        Type = txtMainAccount.Text;
                    }


                    billdate = " From " + " " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "  To  " + oconverter.ArrangeDate2(dtTo.Value.ToString());
                    Subject = "[" + SelectedSegmentName + "] : Ledger For : " + Type + "  " + billdate;

                    if (ResClient.Checked == true)
                    {

                        this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript3", "<script>ForFilterOff();</script>");
                        for (int n = 0; n < dtEmail.Rows.Count; n++)
                        {

                            dtCnt.Clear();
                            emailbdy = "";
                            if (dtEmail.Rows[n][3].ToString() == "CDSL Clients")
                            {
                                dtCnt = oDBEngine.GetDataTable("MASTER_CDSLCLIENTS", "LTRIM(RTRIM(ISNULL(CDSLCLIENTS_FIRSTHOLDERNAME,'')))+'['+ LTRIM(RTRIM(ISNULL(RIGHT(CDSLCLIENTS_BOID,8),''))) ,CDSLCLIENTS_CONTACTID ", "CDSLCLIENTS_CONTACTID='" + dtEmail.Rows[n][0].ToString() + "'");
                                contactid = dtCnt.Rows[0][1].ToString();
                            }
                            else if (dtEmail.Rows[n][3].ToString() == "NSDL Clients")
                            {
                                dtCnt = oDBEngine.GetDataTable("MASTER_NSDLCLIENTS ", " LTRIM(RTRIM(ISNULL(NSDLCLIENTS_BENFIRSTHOLDERNAME,''))) +'['+LTRIM(RTRIM(ISNULL(NSDLCLIENTS_BENACCOUNTID,'')))+']' ", "NSDLCLIENTS_BENACCOUNTID ='" + dtEmail.Rows[n][0].ToString() + "'");
                                contactid = dtCnt.Rows[0][1].ToString();
                            }
                            else
                            {
                                dtCnt = oDBEngine.GetDataTable("TBL_MASTER_CONTACT ", "  LTRIM(RTRIM(ISNULL(CNT_FIRSTNAME,'')))+''+LTRIM(RTRIM(ISNULL(CNT_MIDDLENAME,'')))+LTRIM(RTRIM(ISNULL(CNT_LASTNAME,''))) +'['+LTRIM(RTRIM(ISNULL(CNT_UCC,'')))+']',CNT_INTERNALID ", "CNT_INTERNALID='" + dtEmail.Rows[n][0].ToString() + "'");
                                contactid = dtCnt.Rows[0][1].ToString();
                                //Find Ucc To Add in Subject
                                strClientInfo = dtCnt.Rows[0][0].ToString();
                                strClientInfo = strClientInfo.Substring(strClientInfo.IndexOf('['), strClientInfo.IndexOf(']') - strClientInfo.IndexOf('[') + 1);
                            }

                            Subject = "[" + SelectedSegmentName + "] " + strClientInfo + " : Ledger For : " + Type + "  " + billdate;


                            emailbdy = emailbdy + "<table width=\"1000px\"  style=\"font-family:Verdana;font-size:8pt;border:solid 1pt black\">";
                            if (dtCnt.Rows.Count > 0)
                                emailbdy = emailbdy + "<tr><td align=\"left\">Name: " + dtCnt.Rows[0][0].ToString() + "</td></tr>";
                            emailbdy = emailbdy + "<tr><td align=\"left\">Main Account:" + Type.ToString() + "</td></tr>";
                            if (rdbSegAll.Checked == true)
                                emailbdy = emailbdy + "<tr><td align=\"left\">Segment:ALL</td></tr>";
                            else
                                emailbdy = emailbdy + "<tr><td>Segment:" + HDNSeg.Value.ToString() + "</td></tr>";
                            emailbdy = emailbdy + "<tr><td>Period : From " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "  - " + oconverter.ArrangeDate2(dtTo.Value.ToString()) + "</td></tr>";
                            emailbdy = emailbdy + "<tr><td>" + dtEmail.Rows[n][2].ToString() + "</td></tr>";
                            emailbdy = emailbdy + "</table>";

                            if (oDBEngine.SendReport(emailbdy, contactid, billdate, Subject) == true)
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "MailSendT", "MailsendT();", true);

                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "MailSendF", "MailsendF();", true);

                            }
                        }



                    }
                    else if (ResBranch.Checked == true)
                    {
                        DataTable dtBranch = new DataTable();
                        dtBranch.Columns.Add("Branch");
                        dtBranch.Columns.Add("BranchEmail");
                        dtBranch.Columns.Add("BranchContact");
                        dtBranch.Columns.Add("EmailContent");
                        emailbdy = "";

                        string EmailBody = "";
                        for (int n = 0; n < dtEmail.Rows.Count; n++)
                        {
                            dtCnt.Clear();
                            emailbdy = "";
                            if (dtEmail.Rows[n][3].ToString() == "Customers")
                            {
                                dtCnt = oDBEngine.GetDataTable("TBL_MASTER_CONTACT ", "  LTRIM(RTRIM(ISNULL(CNT_FIRSTNAME,'')))+''+LTRIM(RTRIM(ISNULL(CNT_MIDDLENAME,'')))+LTRIM(RTRIM(ISNULL(CNT_LASTNAME,''))) +'['+LTRIM(RTRIM(ISNULL(CNT_UCC,'')))+']',CNT_INTERNALID ", "CNT_INTERNALID='" + dtEmail.Rows[n][0].ToString() + "'");
                                contactid = dtCnt.Rows[0][1].ToString();
                            }
                            else if (dtEmail.Rows[n][3].ToString() == "CDSL Clients")
                            {
                                dtCnt = oDBEngine.GetDataTable("MASTER_CDSLCLIENTS", "LTRIM(RTRIM(ISNULL(CDSLCLIENTS_FIRSTHOLDERNAME,'')))+'['+ LTRIM(RTRIM(ISNULL(RIGHT(CDSLCLIENTS_BOID,8),''))) ,CDSLCLIENTS_CONTACTID ", "CDSLCLIENTS_CONTACTID='" + dtEmail.Rows[n][0].ToString() + "'");
                                contactid = dtCnt.Rows[0][1].ToString();
                            }
                            else if (dtEmail.Rows[n][3].ToString() == "NSDL Clients")
                            {
                                dtCnt = oDBEngine.GetDataTable("MASTER_NSDLCLIENTS ", " LTRIM(RTRIM(ISNULL(NSDLCLIENTS_BENFIRSTHOLDERNAME,''))) +'['+LTRIM(RTRIM(ISNULL(NSDLCLIENTS_BENACCOUNTID,'')))+']' ", "NSDLCLIENTS_BENACCOUNTID ='" + dtEmail.Rows[n][0].ToString() + "'");
                                contactid = dtCnt.Rows[0][1].ToString();
                            }
                            else if (dtEmail.Rows[n][3].ToString() == "None")
                            {
                                contactid = dtEmail.Rows[n][0].ToString();
                            }
                            else
                            {
                                dtCnt = oDBEngine.GetDataTable("MASTER_SUBACCOUNT ", "LTRIM(RTRIM(ISNULL(SUBACCOUNT_NAME,'')))+'['+ LTRIM(RTRIM(ISNULL(SUBACCOUNT_CODE,'')))+']' ,SUBACCOUNT_CODE", "SUBACCOUNT_CODE='" + dtEmail.Rows[n][0].ToString() + "'");

                            }
                            emailbdy = emailbdy + "<table width=\"100%\">";
                            if (dtEmail.Rows[n][3].ToString() != "None")
                            {
                                emailbdy = emailbdy + "<tr><td align=\"left\">Name: " + dtCnt.Rows[0][0].ToString() + "</td></tr>";
                                emailbdy = emailbdy + "<tr><td align=\"left\">Main Account:" + Type.ToString() + "</td></tr>";
                            }
                            else
                            {
                                emailbdy = emailbdy + "<tr><td align=\"left\">Main Account:" + Type.ToString() + "</td></tr>";
                            }
                            if (rdbSegAll.Checked == true)
                                emailbdy = emailbdy + "<tr><td align=\"left\">Segment:ALL</td></tr>";
                            else
                                emailbdy = emailbdy + "<tr><td>Segment:" + HDNSeg.Value.ToString() + "</td></tr>";
                            emailbdy = emailbdy + "<tr><td>Period : From " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "  - " + oconverter.ArrangeDate2(dtTo.Value.ToString()) + "</td></tr>";
                            emailbdy = emailbdy + "<tr><td>" + dtEmail.Rows[n][2].ToString() + "</td></tr>";
                            emailbdy = emailbdy + "</table>";



                            if (n != dtEmail.Rows.Count - 1)
                            {
                                if (dtEmail.Rows[n][1].ToString() == dtEmail.Rows[n + 1][1].ToString())
                                {
                                    EmailBody = EmailBody + "<tr><td>" + emailbdy + "</td></tr>";
                                }
                                else
                                {
                                    EmailBody = EmailBody + "<tr><td>" + emailbdy + "</td></tr>";

                                    if (ddlGroup.SelectedItem.Value.ToString() == "1")
                                    {
                                        dtBr = oDBEngine.GetDataTable("TBL_MASTER_GROUPMASTER", " GPM_EMAILID AS branch_cpemail,GPM_OWNER AS branch_head", "GPM_CODE='" + dtEmail.Rows[n][1].ToString() + "'");
                                    }
                                    else
                                    {
                                        dtBr = oDBEngine.GetDataTable("tbl_master_branch", " branch_cpemail,branch_head", "branch_code='" + dtEmail.Rows[n][1].ToString() + "'");
                                    }
                                    DataRow drRow1 = dtBranch.NewRow();
                                    drRow1[0] = dtEmail.Rows[n][1].ToString();
                                    drRow1[1] = dtBr.Rows[0][0].ToString();
                                    drRow1[2] = dtBr.Rows[0][1].ToString();
                                    drRow1[3] = "<table width=\"1000px\"   style=\"font-family:Verdana;font-size:8pt;border:solid 1pt black\">" + EmailBody + "</table>";
                                    dtBranch.Rows.Add(drRow1);
                                    dtBr.Clear();
                                    EmailBody = "";

                                }

                            }
                            else
                            {
                                if (dtEmail.Rows.Count > 1)
                                {
                                    if (dtEmail.Rows[n][1].ToString() == dtEmail.Rows[n - 1][1].ToString())
                                    {
                                        EmailBody = EmailBody + "<tr><td>" + emailbdy + "</td></tr></table>";

                                        if (ddlGroup.SelectedItem.Value.ToString() == "1")
                                        {
                                            dtBr = oDBEngine.GetDataTable("TBL_MASTER_GROUPMASTER", " GPM_EMAILID AS branch_cpemail,GPM_OWNER AS branch_head", "GPM_CODE='" + dtEmail.Rows[n][1].ToString() + "'");
                                        }
                                        else
                                        {
                                            dtBr = oDBEngine.GetDataTable("tbl_master_branch", " branch_cpemail,branch_head", "branch_code='" + dtEmail.Rows[n][1].ToString() + "'");
                                        }
                                        DataRow drRow1 = dtBranch.NewRow();
                                        drRow1[0] = dtEmail.Rows[n][1].ToString();
                                        drRow1[1] = dtBr.Rows[0][0].ToString();
                                        drRow1[2] = dtBr.Rows[0][1].ToString();
                                        drRow1[3] = "<table width=\"1000px\"   style=\"font-family:Verdana;font-size:8pt;border:solid 1pt black\">" + EmailBody + "</table>";
                                        dtBranch.Rows.Add(drRow1);
                                        dtBr.Clear();
                                        EmailBody = "";

                                    }
                                    else
                                    {
                                        EmailBody = EmailBody + "<tr><td>" + emailbdy + "</td></tr></table>";

                                        if (ddlGroup.SelectedItem.Value.ToString() == "1")
                                        {
                                            dtBr = oDBEngine.GetDataTable("TBL_MASTER_GROUPMASTER", " GPM_EMAILID AS branch_cpemail,GPM_OWNER AS branch_head", "GPM_CODE='" + dtEmail.Rows[n][1].ToString() + "'");
                                        }
                                        else
                                        {
                                            dtBr = oDBEngine.GetDataTable("tbl_master_branch", " branch_cpemail,branch_head", "branch_code='" + dtEmail.Rows[n][1].ToString() + "'");
                                        }
                                        DataRow drRow1 = dtBranch.NewRow();
                                        drRow1[0] = dtEmail.Rows[n][1].ToString();
                                        drRow1[1] = dtBr.Rows[0][0].ToString();
                                        drRow1[2] = dtBr.Rows[0][1].ToString();
                                        drRow1[3] = "<table width=\"1000px\"  s>" + EmailBody + "</table>";
                                        dtBranch.Rows.Add(drRow1);
                                        dtBr.Clear();
                                        EmailBody = "";
                                    }
                                }
                                else
                                {
                                    EmailBody = EmailBody + "<tr><td>" + emailbdy + "</td></tr></table>";

                                    if (ddlGroup.SelectedItem.Value.ToString() == "1")
                                    {
                                        dtBr = oDBEngine.GetDataTable("TBL_MASTER_GROUPMASTER", " GPM_EMAILID AS branch_cpemail,GPM_OWNER AS branch_head", "GPM_CODE='" + dtEmail.Rows[n][1].ToString() + "'");
                                    }
                                    else
                                    {
                                        dtBr = oDBEngine.GetDataTable("tbl_master_branch", " branch_cpemail,branch_head", "branch_code='" + dtEmail.Rows[n][1].ToString() + "'");
                                    }
                                    DataRow drRow1 = dtBranch.NewRow();
                                    drRow1[0] = dtEmail.Rows[n][1].ToString();
                                    drRow1[1] = dtBr.Rows[0][0].ToString();
                                    drRow1[2] = dtBr.Rows[0][1].ToString();
                                    drRow1[3] = "<table width=\"1000px\"   style=\"font-family:Verdana;font-size:8pt;border:solid 1pt black\">" + EmailBody + "</table>";
                                    dtBranch.Rows.Add(drRow1);
                                    dtBr.Clear();
                                    EmailBody = "";

                                }
                            }
                        }

                        if (dtBranch.Rows.Count > 0)
                        {
                            for (int m = 0; m < dtBranch.Rows.Count; m++)
                            {
                                string EmlBdy = dtBranch.Rows[m][3].ToString();
                                string BranchEmail = dtBranch.Rows[m][1].ToString();
                                string BranchContact = dtBranch.Rows[m][2].ToString();

                                if (oDBEngine.SendReportBr(EmlBdy, BranchEmail, billdate, Subject, BranchContact) == true)
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "MailSendT", "MailsendT();", true);

                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "MailSendF", "MailsendF();", true);

                                }
                            }
                        }

                    }
                    else if (ResUser.Checked == true)
                    {
                        this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript3", "<script>ForFilterOff();</script>");
                        emailbdy = "";
                        string HTBody = "<table width=\"100%\">";
                        for (int n = 0; n < dtEmail.Rows.Count; n++)
                        {
                            dtCnt.Clear();
                            emailbdy = "";
                            if (dtEmail.Rows[n][3].ToString() == "Customers")
                            {
                                dtCnt = oDBEngine.GetDataTable("TBL_MASTER_CONTACT ", "  LTRIM(RTRIM(ISNULL(CNT_FIRSTNAME,'')))+''+LTRIM(RTRIM(ISNULL(CNT_MIDDLENAME,'')))+LTRIM(RTRIM(ISNULL(CNT_LASTNAME,''))) +'['+LTRIM(RTRIM(ISNULL(CNT_UCC,'')))+']',CNT_INTERNALID ", "CNT_INTERNALID='" + dtEmail.Rows[n][0].ToString() + "'");
                                contactid = dtCnt.Rows[0][1].ToString();
                            }
                            else if (dtEmail.Rows[n][3].ToString() == "CDSL Clients")
                            {
                                dtCnt = oDBEngine.GetDataTable("MASTER_CDSLCLIENTS", "LTRIM(RTRIM(ISNULL(CDSLCLIENTS_FIRSTHOLDERNAME,'')))+'['+ LTRIM(RTRIM(ISNULL(RIGHT(CDSLCLIENTS_BOID,8),''))) ,CDSLCLIENTS_CONTACTID ", "CDSLCLIENTS_CONTACTID='" + dtEmail.Rows[n][0].ToString() + "'");
                                contactid = dtCnt.Rows[0][1].ToString();
                            }
                            else if (dtEmail.Rows[n][3].ToString() == "NSDL Clients")
                            {
                                dtCnt = oDBEngine.GetDataTable("MASTER_NSDLCLIENTS ", " LTRIM(RTRIM(ISNULL(NSDLCLIENTS_BENFIRSTHOLDERNAME,''))) +'['+LTRIM(RTRIM(ISNULL(NSDLCLIENTS_BENACCOUNTID,'')))+']' ", "NSDLCLIENTS_BENACCOUNTID ='" + dtEmail.Rows[n][0].ToString() + "'");
                                contactid = dtCnt.Rows[0][1].ToString();
                            }
                            else if (dtEmail.Rows[n][3].ToString() == "None")
                            {
                                contactid = dtEmail.Rows[n][0].ToString();
                            }
                            else
                            {
                                dtCnt = oDBEngine.GetDataTable("MASTER_SUBACCOUNT ", "LTRIM(RTRIM(ISNULL(SUBACCOUNT_NAME,'')))+'['+ LTRIM(RTRIM(ISNULL(SUBACCOUNT_CODE,'')))+']' ,SUBACCOUNT_CODE", "SUBACCOUNT_CODE='" + dtEmail.Rows[n][0].ToString() + "'");

                            }
                            emailbdy = emailbdy + "<table width=\"1000px\"   style=\"font-family:Verdana;font-size:8pt;border:solid 1pt black\">";
                            if (dtEmail.Rows[n][3].ToString() != "None")
                            {
                                emailbdy = emailbdy + "<tr><td align=\"left\">Name: " + dtCnt.Rows[0][0].ToString() + "</td></tr>";
                                emailbdy = emailbdy + "<tr><td align=\"left\">Main Account:" + Type.ToString() + "</td></tr>";
                            }
                            else
                            {
                                emailbdy = emailbdy + "<tr><td align=\"left\">Main Account:" + Type.ToString() + "</td></tr>";
                            }
                            if (rdbSegAll.Checked == true)
                                emailbdy = emailbdy + "<tr><td align=\"left\">Segment:ALL</td></tr>";
                            else
                                emailbdy = emailbdy + "<tr><td>Segment:" + HDNSeg.Value.ToString() + "</td></tr>";
                            emailbdy = emailbdy + "<tr><td>Period : From " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "  - " + oconverter.ArrangeDate2(dtTo.Value.ToString()) + "</td></tr>";
                            emailbdy = emailbdy + "<tr><td>" + dtEmail.Rows[n][2].ToString() + "</td></tr>";
                            emailbdy = emailbdy + "</table>";
                            if (n == 0)
                            {
                                HTBody = HTBody + "<tr><td>" + emailbdy + "</td></tr>";
                            }
                            else
                            {
                                HTBody = HTBody + "<tr><td>" + emailbdy + "</td></tr>";
                            }

                        }
                        HTBody = HTBody + "</table>";
                        billdate = "Period :" + " " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "  - " + oconverter.ArrangeDate2(dtTo.Value.ToString());
                        contactid = HDNEmp.Value;
                        Subject = "[" + SelectedSegmentName + "] : Ledger For " + billdate;
                        if (contactid.ToString() != "")
                        {
                            string[] Cnt = contactid.Split(',');
                            for (int k = 0; k < Cnt.Length; k++)
                            {
                                if (oDBEngine.SendReport(HTBody, Cnt[k].ToString(), billdate, Subject) == true)
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "MailSendT", "MailsendT();", true);

                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "MailSendF", "MailsendF();", true);
                                }
                            }
                        }
                    }
                }
            }


        }


        protected void btnEmail_Click(object sender, EventArgs e)
        {
            string SelectedSegmentName = null;
            DataTable CompanySegmentDeatil = null;
            oGenericMethod = new BusinessLogicLayer.GenericMethod();
            CompanySegmentDeatil = oGenericMethod.GetCompanyDetail(Session["LastCompany"].ToString(), ViewState["Segment"].ToString());
            foreach (DataRow dr in CompanySegmentDeatil.Rows)
            {
                if (SelectedSegmentName == null)
                    SelectedSegmentName = dr[0].ToString().Trim();
                else
                    SelectedSegmentName = SelectedSegmentName + "," + dr[0].ToString().Trim();
            }
            if (radBreakDetail.Checked == true)
            {
                EmailInd = "Y";
                this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript3", "<script>ForFilterOff();</script>");
                DataTable dtCL = (DataTable)Session["Client"];
                for (int n = 0; n < dtCL.Rows.Count; n++)
                {
                    cmbclientsPager.SelectedItem.Value = dtCL.Rows[n]["subaccount_code"].ToString();
                    cmbclientsPager.SelectedItem.Text = dtCL.Rows[n]["subaccount_name"].ToString();
                    DrpChange = "Y";
                    FillGrid();
                    string Type = "";
                    if (ddlAccountType.SelectedItem.Value == "0")
                    {
                        Type = "Clients - Trading A/c  ";
                    }
                    else if (ddlAccountType.SelectedItem.Value == "1")
                    {
                        Type = "Clients - Margin Deposit A/c  ";
                    }
                    else if (ddlAccountType.SelectedItem.Value == "2")
                    {
                        Type = "Clients - Trading A/c and Clients - Margin Deposit A/c  ";
                    }
                    else if (ddlAccountType.SelectedItem.Value == "3")
                    {
                        Type = txtMainAccount.Text;
                    }
                    string billdate = Type + ": Period :" + " " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "  - " + oconverter.ArrangeDate2(dtTo.Value.ToString());
                    string emailbdy = ViewState["EmailHTML"].ToString();
                    string contactid = dtCL.Rows[n]["subaccount_code"].ToString();
                    string Subject = "[" + SelectedSegmentName + "] : Ledger View For " + billdate;
                    if (oDBEngine.SendReport(emailbdy, contactid, billdate, Subject) == true)
                    {

                        this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript6", "<script>MailsendT();</script>");
                        cmbclientsPager.SelectedItem.Value = dtCL.Rows[0]["subaccount_code"].ToString();
                        cmbclientsPager.SelectedItem.Text = dtCL.Rows[0]["subaccount_name"].ToString();
                        // FillGrid();

                    }
                    else
                    {
                        if (dtCL.Rows.Count <= 1)
                        {
                            this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript9", "<script>MailsendF();</script>");
                        }
                        cmbclientsPager.SelectedItem.Value = dtCL.Rows[0]["subaccount_code"].ToString();
                        cmbclientsPager.SelectedItem.Text = dtCL.Rows[0]["subaccount_name"].ToString();
                        // FillGrid();


                    }
                }


            }
            else
            {
                MainAcIDforOp = ViewState["MainAcIDforOp"].ToString();
                dtCashBankBook = (DataTable)ViewState["dtCashBankBook"];
                dtLedgerView = (DataTable)ViewState["dtLedgerView"];
                Segment = ViewState["Segment"].ToString();
                decimal closing = 0;
                this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript3", "<script>ForFilterOff();</script>");
                DataTable dtCL = (DataTable)Session["Client"];
                for (int n = 0; n < dtCL.Rows.Count; n++)
                {
                    cmbclientsPager.SelectedItem.Value = dtCL.Rows[n]["subaccount_code"].ToString();
                    cmbclientsPager.SelectedItem.Text = dtCL.Rows[n]["subaccount_name"].ToString();
                    //  FillGrid();
                    FillGridForChanges();
                    DataTable opBal = oDBEngine.OpeningBalanceJournal1(MainAcIDforOp, null, Convert.ToDateTime(dtFrom.Value), Segment, Session["CompanyID"].ToString(), Convert.ToDateTime(dtTo.Value), Convert.ToInt32(Session["ActiveCurrency"].ToString().Split('~')[0]));
                    //DataTable dtTbl = dtCashBankBook;
                    //DataTable dtTbl = (DataTable)ViewState["dtCashBankBook"];
                    DataTable dtTbl = (DataTable)ViewState["CashBankEmail"];
                    decimal totaldebit = 0;
                    decimal totalcredit = 0;
                    decimal totalclosing = 0;
                    string disptbl = "";
                    decimal debit = 0;
                    decimal credit = 0;
                    if (radBreakDetail.Checked == true)
                    {
                        disptbl = "<table width=\"1200px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"1\"><tr style=\"background-color:#BB694D;color:White;\"><td>Tr. Date</td><td>ValueDate</td><td>Voucher No.</td><td>Description</td><td>Instrument No</td><td>Settlement No</td><td>Debit</td><td>Credit</td><td>Closing</td></tr>";
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
                            if (dtTbl.Rows[j]["TrDate"] == "a")
                            {
                                disptbl += "<tr><td colspan=\"6\">&nbsp;" + dtTbl.Rows[j]["accountsledger_Narration"] + "</td><td align=\"right\">&nbsp;</td><td align=\"right\">&nbsp;</td><td align=\"right\">&nbsp;</td></tr>";
                            }
                            else
                            {
                                disptbl += "<tr><td>&nbsp;" + dtTbl.Rows[j]["TrDate"] + "</td><td>&nbsp;" + dtTbl.Rows[j]["ValueDate"] + "</td><td>&nbsp;" + dtTbl.Rows[j]["accountsledger_TransactionReferenceID"] + "</td><td>&nbsp;" + dtTbl.Rows[j]["accountsledger_Narration"] + "</td><td>&nbsp;" + dtTbl.Rows[j]["accountsledger_InstrumentNumber"] + "</td><td>&nbsp;" + dtTbl.Rows[j]["SettlementNumber"] + "</td><td align=\"right\">&nbsp;" + dbt + "</td><td align=\"right\">&nbsp;" + crdt + "</td><td align=\"right\">&nbsp;" + oconverter.formatmoneyinUs(totalclosing) + "</td></tr>";
                            }

                            //totaldebit = totaldebit + debit;
                            //totalcredit = totalcredit + credit;
                            //totalclosing = totalcredit - totaldebit;
                        }
                        disptbl += "<tr style=\"background-color: #FFD4AA; color: Black;\"><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;Closing Balance</td><td>&nbsp;</td><td>&nbsp;</td><td align=\"right\">&nbsp;" + oconverter.formatmoneyinUs(totaldebit) + "</td><td align=\"right\">&nbsp;" + oconverter.formatmoneyinUs(totalcredit) + "</td><td align=\"right\">&nbsp;" + oconverter.formatmoneyinUs(totalclosing) + "</td></tr>";
                        disptbl += "</table>";
                    }
                    else
                    {

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

                    }

                    string Type = "";
                    // DataRow newRow5 = dtCashBankBook_New1.NewRow();
                    if (ddlAccountType.SelectedItem.Value == "0")
                    {
                        Type = "Clients - Trading A/c  ";
                    }
                    else if (ddlAccountType.SelectedItem.Value == "1")
                    {
                        Type = "Clients - Margin Deposit A/c  ";
                    }
                    else if (ddlAccountType.SelectedItem.Value == "2")
                    {
                        Type = "Clients - Trading A/c and Clients - Margin Deposit A/c  ";
                    }
                    else if (ddlAccountType.SelectedItem.Value == "3")
                    {
                        Type = txtMainAccount.Text;
                    }
                    string billdate = Type + ": Period :" + " " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "  - " + oconverter.ArrangeDate2(dtTo.Value.ToString());
                    string emailbdy = disptbl;
                    string contactid = dtCL.Rows[n]["subaccount_code"].ToString();
                    //string billdate = oconverter.ArrangeDate2(dtFor.Value.ToString());
                    string Subject = "[" + SelectedSegmentName + "] : Ledger View For " + billdate;
                    if (oDBEngine.SendReport(emailbdy, contactid, billdate, Subject) == true)
                    {

                        this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript6", "<script>MailsendT();</script>");
                        cmbclientsPager.SelectedItem.Value = dtCL.Rows[0]["subaccount_code"].ToString();
                        cmbclientsPager.SelectedItem.Text = dtCL.Rows[0]["subaccount_name"].ToString();
                        FillGrid();
                        //    string SpanText = oconverter.ArrangeDate2(dtFor.Value.ToString());
                        //    this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript9", "<script>displaydate('" + SpanText + "');</script>");
                    }
                    else
                    {
                        if (dtCL.Rows.Count <= 1)
                        {
                            this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript9", "<script>MailsendF();</script>");
                        }
                        cmbclientsPager.SelectedItem.Value = dtCL.Rows[0]["subaccount_code"].ToString();
                        cmbclientsPager.SelectedItem.Text = dtCL.Rows[0]["subaccount_name"].ToString();
                        FillGrid();
                        //string SpanText = oconverter.ArrangeDate2(dtFor.Value.ToString());
                        //this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript9", "<script>displaydate('" + SpanText + "');</script>");

                    }

                    // dtCashBankBook.Clear();
                }
            }
        }

        #endregion

        #region Excel
        protected void ddlExport_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void ddlExport_SelectedIndexChanged1(object sender, EventArgs e)
        {
            string SelectedSegmentName = null;
            DataTable CompanySegmentDeatil = null;
            oGenericMethod = new BusinessLogicLayer.GenericMethod();
            CompanySegmentDeatil = oGenericMethod.GetCompanyDetail(Session["LastCompany"].ToString(), ViewState["Segment"].ToString());
            foreach (DataRow dr in CompanySegmentDeatil.Rows)
            {
                if (SelectedSegmentName == null)
                    SelectedSegmentName = dr[0].ToString().Trim();
                else
                    SelectedSegmentName = SelectedSegmentName + "," + dr[0].ToString().Trim();
            }

            if (radBreakDetail.Checked == true)
            {

                EmailInd = "Y";
                this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript3", "<script>ForFilterOff();</script>");
                DataTable dtComp = oDBEngine.GetDataTable("tbl_master_company", "cmp_name", "cmp_internalid='" + Session["LastCompany"].ToString() + "'");
                string CompName = dtComp.Rows[0][0].ToString();
                string ExHTML = "";
                ExHTML = "<table  border=\"1\" width=\"100%\" cellspacing=\"0\" cellpadding=\"0\">";
                DataTable dtCL = (DataTable)Session["Client"];
                for (int n = 0; n < dtCL.Rows.Count; n++)
                {
                    cmbclientsPager.SelectedItem.Value = dtCL.Rows[n]["subaccount_code"].ToString();
                    cmbclientsPager.SelectedItem.Text = dtCL.Rows[n]["subaccount_name"].ToString();
                    DrpChange = "Y";
                    FillGrid();
                    string Type = "";
                    if (ddlAccountType.SelectedItem.Value == "0")
                    {
                        Type = "Clients - Trading A/c  ";
                    }
                    else if (ddlAccountType.SelectedItem.Value == "1")
                    {
                        Type = "Clients - Margin Deposit A/c  ";
                    }
                    else if (ddlAccountType.SelectedItem.Value == "2")
                    {
                        Type = "Clients - Trading A/c and Clients - Margin Deposit A/c  ";
                    }
                    else if (ddlAccountType.SelectedItem.Value == "3")
                    {
                        Type = txtMainAccount.Text;
                    }
                    string billdate = Type + ": Period :" + " " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "  - " + oconverter.ArrangeDate2(dtTo.Value.ToString());
                    string emailbdy = ViewState["EmailHTML"].ToString();
                    string contactid = dtCL.Rows[n]["subaccount_code"].ToString();
                    string Subject = "[" + SelectedSegmentName + "] : Ledger View For " + billdate;

                    ExHTML = ExHTML + "<tr><td align=\"center\" style=\"background-color:#EEE0E5;font-weight:bold;\">" + CompName + "</td></tr>";
                    ExHTML = ExHTML + "<tr><td align=\"center\" style=\"background-color:#EEE0E5;font-weight:bold;\">" + cmbclientsPager.SelectedItem.Text + "</td></tr>";
                    ExHTML = ExHTML + "<tr><td align=\"center\" style=\"background-color:#EEE0E5;font-weight:bold;\">" + billdate + "</td></tr>";
                    if (rdbSegAll.Checked == true)
                    {
                        ExHTML = ExHTML + "<tr><td align=\"center\" style=\"background-color:#EEE0E5;font-weight:bold;\">Segment:ALL</td></tr>";
                    }
                    else
                    {
                        ExHTML = ExHTML + "<tr><td align=\"center\" style=\"background-color:#EEE0E5;font-weight:bold;\">Segment:" + litSegment.InnerText.ToString() + "</td></tr>";

                    }
                    ExHTML = ExHTML + "<tr><td align=\"center\" style=\"font-weight:bold;\">" + ViewState["EmailHTML"].ToString() + "</td></tr>";
                    ExHTML = ExHTML + "<tr><td></td></tr>";
                    ViewState["EmailHTML"] = "";

                }
                ExHTML = ExHTML + "</table>";

                Response.AppendHeader("Content-Disposition", "attachment; filename=Ledger.xls");
                Response.ContentType = "application/ms-excel";
                Response.Write(ExHTML.ToString());
                Response.End();



            }
            else
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

                    dtCashBankBook = (DataTable)ViewState["dtCashBankBook"];
                    dtLedgerView = (DataTable)ViewState["dtLedgerView"];

                    //DataTable DtOriginal = new DataTable();
                    //DtOriginal = (DataTable)ViewState["dtCashBankBook"];
                    //dtCashBankBook = DtOriginal.Clone();
                    //if (ChkShowValueDate.Checked)
                    //    dtCashBankBook = DtOriginal.Clone();
                    //else
                    //{
                    //    //dtCashBankBook = DtOriginal.Clone();
                    //    foreach (DataRow Row in DtOriginal.Rows)
                    //        dtCashBankBook.ImportRow(Row);
                    //    dtCashBankBook.Columns.RemoveAt(1);
                    //}

                    DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
                    System.Globalization.NumberFormatInfo currencyFormat = new System.Globalization.CultureInfo("en-us").NumberFormat;
                    currencyFormat.CurrencySymbol = "";
                    currencyFormat.CurrencyNegativePattern = 2;
                    decimal receipt = 0;
                    decimal Payment = 0;
                    decimal closingRate = 0;
                    string CheckingValueParam = null;
                    string strTranType = "";
                    string strCbPayment = "n";
                    string strCbReceipt = "n";
                    string strCbContract = "n";
                    string strJvType = "";
                    string strSelectedJv = "";

                    DataTable OpenBalance = new DataTable();
                    DataTable dtCashBankBook1 = new DataTable();
                    DataTable dtLedger = new DataTable();
                    if (rdbSegAll.Checked == true)
                    {
                        DataTable dtSegment = oDBEngine.GetDataTable("tbl_master_companyExchange", "exch_internalId", "exch_compId='" + Session["CompanyID"].ToString() + "'");
                        if (dtSegment.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtSegment.Rows.Count; i++)
                            {
                                if (Segment == null)
                                    Segment = dtSegment.Rows[i][0].ToString();
                                else
                                    Segment += "," + dtSegment.Rows[i][0].ToString();
                            }
                        }
                    }
                    if (rdbranchAll.Checked == true)
                    {
                        Branch = Session["userbranchHierarchy"].ToString();
                    }
                    else
                        Branch = ViewState["branchID"].ToString();
                    if (Segment == null)
                    {
                        Segment = Session["SegmentID"].ToString();
                    }
                    string mainAccountSearch = null;
                    string SubAccountSearch = null;
                    //if (ddlAccountType.SelectedValue == "0")
                    //{
                    //    mainAccountSearch = "and accountsledger_MainAccountID in('SYSTM00001')";
                    //    MainAcIDforOp = "'SYSTM00001'";
                    //}
                    //else if (ddlAccountType.SelectedValue == "1")
                    //{
                    //    mainAccountSearch = "and accountsledger_MainAccountID in('SYSTM00002')";
                    //    MainAcIDforOp = "'SYSTM00002'";
                    //}
                    //else if (ddlAccountType.SelectedValue == "2")
                    //{
                    //    mainAccountSearch = "and accountsledger_MainAccountID in('SYSTM00001','SYSTM00002')";
                    //    MainAcIDforOp = "'SYSTM00001','SYSTM00002'";
                    //}
                    //else
                    //{
                    //    mainAccountSearch = "and accountsledger_MainAccountID in(" + MainAcID + ")";
                    //}

                    if (ddlAccountType.SelectedValue == "0")
                    {

                        if (MainAcID == "" || MainAcID == null)
                        {
                            mainAccountSearch = "and accountsledger_MainAccountID in('SYSTM00001')";
                            MainAcIDforOp = "'SYSTM00001'";
                        }
                        else
                        {
                            mainAccountSearch = "and accountsledger_MainAccountID in(" + MainAcID + ")";
                            MainAcIDforOp = MainAcID;
                        }
                        if (SubLedgerType == "")
                            SubLedgerType = "Customers";

                    }
                    else if (ddlAccountType.SelectedValue == "1")
                    {

                        if (MainAcID == "" || MainAcID == null)
                        {
                            mainAccountSearch = "and accountsledger_MainAccountID in('SYSTM00002')";
                            MainAcIDforOp = "'SYSTM00002'";
                        }
                        else
                        {
                            mainAccountSearch = "and accountsledger_MainAccountID in(" + MainAcID + ")";
                            MainAcIDforOp = MainAcID;
                        }
                        if (SubLedgerType == "")
                            SubLedgerType = "Customers";
                    }
                    else if (ddlAccountType.SelectedValue == "2")
                    {

                        if (MainAcID == "" || MainAcID == null)
                        {

                            mainAccountSearch = "and accountsledger_MainAccountID in('SYSTM00001','SYSTM00002')";
                            MainAcIDforOp = "'SYSTM00001','SYSTM00002'";

                        }
                        else
                        {
                            mainAccountSearch = "and accountsledger_MainAccountID in('" + MainAcID + "')";
                            MainAcIDforOp = MainAcID;
                        }

                        if (SubLedgerType == "")
                            SubLedgerType = "Customers";
                    }
                    else
                    {
                        mainAccountSearch = "and accountsledger_MainAccountID in(" + MainAcID + ")";
                    }

                    if (rbTanAll.Checked == true)
                        strTranType = "all";
                    else if (rbTranCashBank.Checked == true)
                    {
                        strTranType = "cb";
                        if (chkPayment.Checked == true)
                            strCbPayment = "y";
                        if (chkReceipts.Checked == true)
                            strCbReceipt = "y";
                        if (chkContracts.Checked == true)
                            strCbContract = "y";

                    }
                    else if (rbTranJv.Checked == true)
                    {
                        strTranType = "jv";
                        if (rbAllJV.Checked == true)
                            strJvType = "all";
                        else if (rbManualJV.Checked == true)
                        {
                            strJvType = "man";
                            strSelectedJv = txtVoucherPrefix.Text;
                        }
                        else if (rbSystemJV.Checked == true)
                        {
                            strJvType = "sys";
                            //strSelectedJv = ViewState["Selectedjvs"].ToString();
                            if (rbSystemJVAll.Checked == true)
                                strSelectedJv = "all";
                            else if (rbSystemJVSelected.Checked == true)
                                strSelectedJv = hdnSystemJvs.Value;
                        }

                    }

                    string strSqlCb = "";
                    string strSqlJv = "";

                    if (strTranType == "cb")
                    {
                        strSqlCb = " and  AccountsLedger_TransactionType='Cash_Bank' ";
                        if (strCbPayment == "y" && strCbReceipt != "y" && strCbContract != "y")
                            strSqlCb = strSqlCb + " and patindex('%/%',AccountsLedger_TransactionReferenceID)>0 and  left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1)='P' ";

                        if (strCbPayment != "y" && strCbReceipt == "y" && strCbContract != "y")
                            strSqlCb = strSqlCb + " and patindex('%/%',AccountsLedger_TransactionReferenceID)>0 and left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1)='R' ";

                        if (strCbPayment != "y" && strCbReceipt != "y" && strCbContract == "y")
                            strSqlCb = strSqlCb + " and patindex('%/%',AccountsLedger_TransactionReferenceID)>0 and left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1)='C' ";

                        if (strCbPayment == "y" && strCbReceipt == "y" && strCbContract != "y")
                            strSqlCb = strSqlCb + "  and patindex('%/%',AccountsLedger_TransactionReferenceID)>0 and  (left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1)='P' or left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1)='R') ";
                        if (strCbPayment != "y" && strCbReceipt == "y" && strCbContract == "y")
                            strSqlCb = strSqlCb + " and patindex('%/%',AccountsLedger_TransactionReferenceID)>0 and (left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1)='R' or left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1)='C') ";
                        if (strCbPayment == "y" && strCbReceipt != "y" && strCbContract == "y")
                            strSqlCb = strSqlCb + " and patindex('%/%',AccountsLedger_TransactionReferenceID)>0 and  (left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1)='P' or left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1)='C') ";
                        if (strCbPayment == "y" && strCbReceipt == "y" && strCbContract == "y")
                            strSqlCb = strSqlCb + " and patindex('%/%',AccountsLedger_TransactionReferenceID)>0 and  (left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1)='P' or left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1)='R' or left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1)='C') ";
                        if (strCbPayment != "y" && strCbReceipt != "y" && strCbContract != "y")
                            strSqlCb = strSqlCb + "  and patindex('%/%',AccountsLedger_TransactionReferenceID)>0 and  (left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1)!='P' and left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1)!='R' and left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1)!='C') ";


                    }
                    if (strTranType == "jv")
                    {
                        if (strJvType == "all")
                            strSqlJv = " and  AccountsLedger_TransactionType='Journal' ";
                        else if (strJvType == "man")
                            if (strSelectedJv == "??")
                            {
                                strSqlJv = " and  patindex('%/%',AccountsLedger_TransactionReferenceID)>0 and AccountsLedger_TransactionType='Journal' and left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1) not like 'u%' and left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1) not like 'v%' and left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1) not like 'w%' and left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1) not like 'x%' and left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1) not like 'y%' and left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1) not like 'z%' ";

                            }
                            else
                                strSqlJv = " and  patindex('%/%',AccountsLedger_TransactionReferenceID)>0 and AccountsLedger_TransactionType='Journal' and left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1)='" + strSelectedJv + "' ";
                        else if (strJvType == "sys")
                            if (strSelectedJv == "all")
                            {
                                strSqlJv = " and  patindex('%/%',AccountsLedger_TransactionReferenceID)>0 and AccountsLedger_TransactionType='Journal' and (left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1) like 'u%' or left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1) like 'v%' or left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1) like 'w%' or left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1) like 'w%' or left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1) like 'x%' or left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1) like 'y%' or left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1) like 'z%' )";

                            }
                            else
                                strSqlJv = " and  patindex('%/%',AccountsLedger_TransactionReferenceID)>0 and AccountsLedger_TransactionType='Journal' and left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1) in(" + strSelectedJv + ") ";


                    }


                    for (int l = 0; l < cmbclientsPager.Items.Count; l++)
                    {
                        receipt = 0;
                        Payment = 0;
                        string valItem = cmbclientsPager.Items[l].Value;
                        if (SubLedgerType.Trim() == "None")
                        {
                            SubAccountSearch = null;
                            if (radConsolidated.Checked == true)
                            {
                                if (radDateWise.Checked == true)
                                {
                                    dtCashBankBook1 = oDBEngine.GetDataTable("(Select convert(varchar(11),a.accountsledger_transactiondate,113) as TrDate,case when a.accountsledger_valuedate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),a.accountsledger_valuedate,113) end as ValueDate,case when a.AccountsLedger_TransactionType='Journal' and Left(AccountsLedger_TransactionReferenceID,1) in('X','Y','Z') and accountsledger_SettlementNumber is not null and accountsledger_SettlementNumber<>''	then 'Consolidated Entries For '+convert(varchar(11),a.accountsledger_transactiondate,113) 	else isnull(a.accountsledger_Narration,'') end +' - '+isnull(isnull((select MainAccount_Name from Master_MainAccount where MainAccount_AccountCode=AccountsLedger_CashBankName),AccountsLedger_CashBankName),'') as accountsledger_Narration,a.accountsledger_InstrumentNumber,(a.accountsledger_SettlementNumber+' '+a.accountsledger_SettlementType) as SettlementNumber,(select top 1 convert(varchar(12),Settlements_StartDateTime,113) from master_settlements where Settlements_Number=a.accountsledger_SettlementNumber and Settlements_TypeSuffix=a.accountsledger_SettlementType) as PayoutDate,(select branch_code from tbl_master_branch where branch_id=(select cnt_branchid from tbl_master_contact where cnt_internalID=accountsledger_SubAccountID)) as BranchCode,case when a.Accountsledger_AmountDr='0.00000000' then null else a.Accountsledger_AmountDr end as Accountsledger_AmountCr,case when a.Accountsledger_AmountCr='0.00000000' then null else a.Accountsledger_AmountCr end as Accountsledger_AmountDr,'0.0' as Closing,accountsledger_transactiondate from Trans_accountsledger a  WHERE  accountsledger_companyID='" + Session["CompanyID"].ToString() + "' and accountsledger_ExchangeSegmentID in(" + Segment + ") and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,accountsledger_transactiondate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtFrom.Value + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtTo.Value + "')) as datetime)   " + mainAccountSearch + " and AccountsLedger_BranchID in(" + Branch + ")  " + SubAccountSearch + @"  and AccountsLedger_TransactionType<>'OpeningBalance' 
                                and isnull(AccountsLedger_Currency," + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0] + ") as D ", " TrDate,ValueDate, 0 as accountsledger_TransactionReferenceID,accountsledger_Narration,accountsledger_InstrumentNumber,SettlementNumber,PayoutDate,BranchCode,case when (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0)))<0 then cast((sum(isnull(Accountsledger_AmountCr,0))-sum(isnull(Accountsledger_AmountDr,0))) as varchar(max)) else null end Accountsledger_AmountCr,case when (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0)))>0 then cast((sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0))) as varchar(max)) else null end Accountsledger_AmountDr,Closing", null, " TrDate,ValueDate,accountsledger_InstrumentNumber,SettlementNumber,PayoutDate,BranchCode,Closing,accountsledger_transactiondate,accountsledger_Narration", " accountsledger_transactiondate");
                                }
                                else if (radVoucherWise.Checked == true)
                                    dtCashBankBook1 = oDBEngine.GetDataTable("(Select convert(varchar(11),a.accountsledger_transactiondate,113) as TrDate,case when a.accountsledger_valuedate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),a.accountsledger_valuedate,113) end as ValueDate,isnull((select top 1 journalvoucher_Narration from trans_journalvoucher where journalvoucher_voucherNumber=a.accountsledger_TransactionReferenceID and journalvoucher_companyID=a.accountsledger_companyID and journalvoucher_ExchangeSegmentID=a.accountsledger_ExchangeSegmentID and journalvoucher_TransactionDate=a.accountsledger_transactiondate),isnull(accountsledger_Narration,'')) +' - '+isnull(isnull((select MainAccount_Name from Master_MainAccount where MainAccount_AccountCode=AccountsLedger_CashBankName),AccountsLedger_CashBankName),'') as accountsledger_Narration,a.accountsledger_InstrumentNumber,(a.accountsledger_SettlementNumber+' '+a.accountsledger_SettlementType) as SettlementNumber,(select top 1 convert(varchar(12),Settlements_StartDateTime,113) from master_settlements where Settlements_Number=a.accountsledger_SettlementNumber and Settlements_TypeSuffix=a.accountsledger_SettlementType) as PayoutDate,(select branch_code from tbl_master_branch where branch_id=(select cnt_branchid from tbl_master_contact where cnt_internalID=accountsledger_SubAccountID)) as BranchCode,case when a.Accountsledger_AmountDr='0.00000000' then null else a.Accountsledger_AmountDr end as Accountsledger_AmountCr,case when a.Accountsledger_AmountCr='0.00000000' then null else a.Accountsledger_AmountCr end as Accountsledger_AmountDr,'0.0' as Closing,accountsledger_transactiondate from Trans_accountsledger a  WHERE  accountsledger_companyID='" + Session["CompanyID"].ToString() + "' and accountsledger_ExchangeSegmentID in(" + Segment + ") and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,accountsledger_transactiondate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtFrom.Value + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtTo.Value + "')) as datetime)   " + mainAccountSearch + " and AccountsLedger_BranchID in(" + Branch + ")  " + SubAccountSearch + @"  and AccountsLedger_TransactionType<>'OpeningBalance' 
                                and isnull(AccountsLedger_Currency,
                                " + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0] + ") as D ", " TrDate,ValueDate, 0 as accountsledger_TransactionReferenceID,accountsledger_Narration,accountsledger_InstrumentNumber,SettlementNumber,PayoutDate,BranchCode,case when (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0)))<0 then cast((sum(isnull(Accountsledger_AmountCr,0))-sum(isnull(Accountsledger_AmountDr,0))) as varchar(max)) else null end Accountsledger_AmountCr,case when (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0)))>0 then cast((sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0))) as varchar(max)) else null end Accountsledger_AmountDr,Closing", null, " TrDate,ValueDate,accountsledger_InstrumentNumber,SettlementNumber,PayoutDate,BranchCode,Closing,accountsledger_transactiondate,accountsledger_Narration", " accountsledger_transactiondate");
                                else
                                    dtCashBankBook1 = oDBEngine.GetDataTable("(Select case when a.AccountsLedger_TransactionType='Journal' and Left(AccountsLedger_TransactionReferenceID,1) in('X','Z') then null else convert(varchar(11),a.accountsledger_transactiondate,113) end as TrDate,case when a.AccountsLedger_TransactionType='Journal' and Left(AccountsLedger_TransactionReferenceID,1) in('X','Z') then null else (case when a.accountsledger_valuedate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),a.accountsledger_valuedate,113) end) end as ValueDate,case when a.AccountsLedger_TransactionType='Journal' and Left(AccountsLedger_TransactionReferenceID,1) in('X','Z') then 'Consolidated Entries For the period ' else accountsledger_Narration end as accountsledger_Narration,a.accountsledger_InstrumentNumber,'0' as SettlementNumber,'1/1/1900' as PayoutDate,(select branch_code from tbl_master_branch where branch_id=(select cnt_branchid from tbl_master_contact where cnt_internalID=accountsledger_SubAccountID)) as BranchCode,case when a.Accountsledger_AmountDr='0.00000000' then null else a.Accountsledger_AmountDr end as Accountsledger_AmountCr,case when a.Accountsledger_AmountCr='0.00000000' then null else a.Accountsledger_AmountCr end as Accountsledger_AmountDr,'0.0' as Closing,'1/1/1900' as accountsledger_transactiondate,(select top 1 equity_effectuntil from master_equity where equity_exchsegmentid=2 and equity_effectuntil >= accountsledger_transactiondate order by equity_effectuntil) as ExpDate,(select case when (select top 1 equity_effectuntil from master_equity where equity_exchsegmentid=2 and equity_effectuntil < accountsledger_transactiondate order by equity_effectuntil desc)<'" + dtFrom.Value + "' then '" + dtFrom.Value + "' else (select top 1 DATEADD(dd, 1, CAST(equity_effectuntil AS datetime)) from master_equity where equity_exchsegmentid=2 and equity_effectuntil < accountsledger_transactiondate order by equity_effectuntil desc) end) as FDate,case when a.AccountsLedger_TransactionType='Journal' and Left(AccountsLedger_TransactionReferenceID,1) in('X','Z') then (select case when (select top 1 equity_effectuntil from master_equity where equity_exchsegmentid=2 and equity_effectuntil >= accountsledger_transactiondate order by equity_effectuntil)>'" + dtTo.Value + "' then '" + dtTo.Value + "' else ( select top 1 equity_effectuntil from master_equity where equity_exchsegmentid=2 and equity_effectuntil >= accountsledger_transactiondate order by equity_effectuntil) end) else accountsledger_transactiondate end as TDate from Trans_accountsledger a  WHERE  accountsledger_companyID='" + Session["CompanyID"].ToString() + "' and accountsledger_ExchangeSegmentID in(" + Segment + ") and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,accountsledger_transactiondate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtFrom.Value + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtTo.Value + "')) as datetime)   " + mainAccountSearch + " and AccountsLedger_BranchID in(" + Branch + ")  " + SubAccountSearch + @"  and AccountsLedger_TransactionType<>'OpeningBalance' 
                                and isnull(AccountsLedger_Currency,
                                " + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0] + ") as D ", " case when TrDate is null then convert(varchar(11),TDate,113) else TrDate end as TrDate,ValueDate, 0 as accountsledger_TransactionReferenceID,case when accountsledger_Narration='Consolidated Entries For the period' then accountsledger_Narration+convert(varchar(11),FDate,113)+' To '+convert(varchar(11),TDate,113) else accountsledger_Narration end as accountsledger_Narration,accountsledger_InstrumentNumber,SettlementNumber,PayoutDate,BranchCode,case when (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0)))<0 then cast((sum(isnull(Accountsledger_AmountCr,0))-sum(isnull(Accountsledger_AmountDr,0))) as varchar(max)) else null end Accountsledger_AmountCr,case when (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0)))>0 then cast((sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0))) as varchar(max)) else null end Accountsledger_AmountDr,Closing", null, " TrDate,ValueDate,accountsledger_InstrumentNumber,SettlementNumber,PayoutDate,BranchCode,Closing,accountsledger_transactiondate,accountsledger_Narration,FDate,TDate", " TDate");
                            }
                            else
                                dtCashBankBook1 = oDBEngine.GetDataTable("Trans_accountsledger a ", "convert(varchar(11),a.accountsledger_transactiondate,113) as TrDate,case when a.accountsledger_valuedate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),a.accountsledger_valuedate,113) end as ValueDate,a.accountsledger_TransactionReferenceID, isnull(a.accountsledger_Narration,'') +' - '+isnull(isnull((select MainAccount_Name from Master_MainAccount where MainAccount_AccountCode=AccountsLedger_CashBankName),AccountsLedger_CashBankName),'') as accountsledger_Narration,a.accountsledger_InstrumentNumber,(a.accountsledger_SettlementNumber+' '+a.accountsledger_SettlementType) as SettlementNumber,(select top 1 convert(varchar(12),Settlements_StartDateTime,113) from master_settlements where Settlements_Number=a.accountsledger_SettlementNumber and Settlements_TypeSuffix=a.accountsledger_SettlementType) as PayoutDate,(select branch_code from tbl_master_branch where branch_id=(select cnt_branchid from tbl_master_contact where cnt_internalID=accountsledger_SubAccountID)) as BranchCode,case when a.Accountsledger_AmountDr='0.00000000' then null else cast(a.Accountsledger_AmountDr as varchar(max)) end as Accountsledger_AmountCr,case when a.Accountsledger_AmountCr='0.00000000' then null else cast(a.Accountsledger_AmountCr as varchar(max)) end as Accountsledger_AmountDr,'0.0' as Closing", " accountsledger_companyID='" + Session["CompanyID"].ToString() + "' and accountsledger_ExchangeSegmentID in(" + Segment + ") and  cast(DATEADD(dd, 0, DATEDIFF(dd, 0,accountsledger_transactiondate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtFrom.Value + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtTo.Value + "')) as datetime)  " + mainAccountSearch + " and AccountsLedger_BranchID in(" + Branch + ") " + SubAccountSearch + @"   and AccountsLedger_TransactionType<>'OpeningBalance'
                            and isnull(AccountsLedger_Currency,
                            " + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0] + strSqlCb + strSqlJv, " accountsledger_transactiondate,accountsledger_TransactionReferenceID");
                            OpenBalance = oDBEngine.OpeningBalanceJournal1(MainAcIDforOp, null, Convert.ToDateTime(dtFrom.Value), Segment, Session["CompanyID"].ToString(), Convert.ToDateTime(dtTo.Value), Convert.ToInt32(Session["ActiveCurrency"].ToString().Split('~')[0]));
                        }
                        else
                        {
                            SubAccountSearch = " and AccountsLedger_SubAccountID in('" + valItem + "') ";
                            if (radConsolidated.Checked == true)
                            {
                                if (radDateWise.Checked == true)
                                {
                                    dtCashBankBook1 = oDBEngine.GetDataTable("(Select convert(varchar(11),a.accountsledger_transactiondate,113) as TrDate,case when a.accountsledger_valuedate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),a.accountsledger_valuedate,113) end as ValueDate,case when a.AccountsLedger_TransactionType='Journal' and Left(AccountsLedger_TransactionReferenceID,1) in('X','Y','Z') and accountsledger_SettlementNumber is not null and accountsledger_SettlementNumber<>''	then 'Consolidated Entries For '+convert(varchar(11),a.accountsledger_transactiondate,113) 	else isnull(a.accountsledger_Narration,'') end +' - '+isnull(isnull((select MainAccount_Name from Master_MainAccount where MainAccount_AccountCode=AccountsLedger_CashBankName),AccountsLedger_CashBankName),'') as accountsledger_Narration,a.accountsledger_InstrumentNumber,(a.accountsledger_SettlementNumber+' '+a.accountsledger_SettlementType) as SettlementNumber,(select top 1 convert(varchar(12),Settlements_StartDateTime,113) from master_settlements where Settlements_Number=a.accountsledger_SettlementNumber and Settlements_TypeSuffix=a.accountsledger_SettlementType) as PayoutDate,(select branch_code from tbl_master_branch where branch_id=(select cnt_branchid from tbl_master_contact where cnt_internalID=accountsledger_SubAccountID)) as BranchCode,case when a.Accountsledger_AmountDr='0.00000000' then null else a.Accountsledger_AmountDr end as Accountsledger_AmountCr,case when a.Accountsledger_AmountCr='0.00000000' then null else a.Accountsledger_AmountCr end as Accountsledger_AmountDr,'0.0' as Closing,accountsledger_transactiondate from Trans_accountsledger a  WHERE  accountsledger_companyID='" + Session["CompanyID"].ToString() + "' and accountsledger_ExchangeSegmentID in(" + Segment + ") and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,accountsledger_transactiondate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtFrom.Value + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtTo.Value + "')) as datetime)   " + mainAccountSearch + " and AccountsLedger_BranchID in(" + Branch + ")  " + SubAccountSearch + @"  and AccountsLedger_SubAccountID is not null and AccountsLedger_TransactionType<>'OpeningBalance' 
                                and isnull(AccountsLedger_Currency,
                                " + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0] + strSqlCb + strSqlJv + ") as D ", " TrDate,ValueDate, 0 as accountsledger_TransactionReferenceID,accountsledger_Narration,accountsledger_InstrumentNumber,SettlementNumber,PayoutDate,BranchCode,case when (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0)))<0 then cast((sum(isnull(Accountsledger_AmountCr,0))-sum(isnull(Accountsledger_AmountDr,0))) as varchar(max)) else null end Accountsledger_AmountCr,case when (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0)))>0 then cast((sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0))) as varchar(max)) else null end Accountsledger_AmountDr,Closing", null, " TrDate,ValueDate,accountsledger_InstrumentNumber,SettlementNumber,PayoutDate,BranchCode,Closing,accountsledger_transactiondate,accountsledger_Narration", " accountsledger_transactiondate");
                                }
                                else if (radVoucherWise.Checked == true)
                                    dtCashBankBook1 = oDBEngine.GetDataTable("(Select convert(varchar(11),a.accountsledger_transactiondate,113) as TrDate,case when a.accountsledger_valuedate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),a.accountsledger_valuedate,113) end as ValueDate,isnull((select top 1 journalvoucher_Narration from trans_journalvoucher where journalvoucher_voucherNumber=a.accountsledger_TransactionReferenceID and journalvoucher_companyID=a.accountsledger_companyID and journalvoucher_ExchangeSegmentID=a.accountsledger_ExchangeSegmentID and journalvoucher_TransactionDate=a.accountsledger_transactiondate),isnull(accountsledger_Narration,'')) +' - '+isnull(isnull((select MainAccount_Name from Master_MainAccount where MainAccount_AccountCode=AccountsLedger_CashBankName),AccountsLedger_CashBankName),'') as accountsledger_Narration,a.accountsledger_InstrumentNumber,(a.accountsledger_SettlementNumber+' '+a.accountsledger_SettlementType) as SettlementNumber,(select top 1 convert(varchar(12),Settlements_StartDateTime,113) from master_settlements where Settlements_Number=a.accountsledger_SettlementNumber and Settlements_TypeSuffix=a.accountsledger_SettlementType) as PayoutDate,(select branch_code from tbl_master_branch where branch_id=(select cnt_branchid from tbl_master_contact where cnt_internalID=accountsledger_SubAccountID)) as BranchCode,case when a.Accountsledger_AmountDr='0.00000000' then null else a.Accountsledger_AmountDr end as Accountsledger_AmountCr,case when a.Accountsledger_AmountCr='0.00000000' then null else a.Accountsledger_AmountCr end as Accountsledger_AmountDr,'0.0' as Closing,accountsledger_transactiondate from Trans_accountsledger a  WHERE  accountsledger_companyID='" + Session["CompanyID"].ToString() + "' and accountsledger_ExchangeSegmentID in(" + Segment + ") and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,accountsledger_transactiondate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtFrom.Value + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtTo.Value + "')) as datetime)   " + mainAccountSearch + " and AccountsLedger_BranchID in(" + Branch + ")  " + SubAccountSearch + @" and AccountsLedger_SubAccountID is not null and AccountsLedger_TransactionType<>'OpeningBalance' 
                                and isnull(AccountsLedger_Currency,
                                " + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0] + strSqlCb + strSqlJv + ") as D ", " TrDate,ValueDate, 0 as accountsledger_TransactionReferenceID,accountsledger_Narration,accountsledger_InstrumentNumber,SettlementNumber,PayoutDate,BranchCode,case when (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0)))<0 then cast((sum(isnull(Accountsledger_AmountCr,0))-sum(isnull(Accountsledger_AmountDr,0))) as varchar(max)) else null end Accountsledger_AmountCr,case when (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0)))>0 then cast((sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0))) as varchar(max)) else null end Accountsledger_AmountDr,Closing", null, " TrDate,ValueDate,accountsledger_InstrumentNumber,SettlementNumber,PayoutDate,BranchCode,Closing,accountsledger_transactiondate,accountsledger_Narration", " accountsledger_transactiondate");
                                else
                                    dtCashBankBook1 = oDBEngine.GetDataTable("(Select case when a.AccountsLedger_TransactionType='Journal' and Left(AccountsLedger_TransactionReferenceID,1) in('X','Z') then null else convert(varchar(11),a.accountsledger_transactiondate,113) end as TrDate,case when a.AccountsLedger_TransactionType='Journal' and Left(AccountsLedger_TransactionReferenceID,1) in('X','Z') then null else (case when a.accountsledger_valuedate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),a.accountsledger_valuedate,113) end) end as ValueDate,case when a.AccountsLedger_TransactionType='Journal' and Left(AccountsLedger_TransactionReferenceID,1) in('X','Z') then 'Consolidated Entries For the period ' else accountsledger_Narration end as accountsledger_Narration,a.accountsledger_InstrumentNumber,'0' as SettlementNumber,'1/1/1900' as PayoutDate,(select branch_code from tbl_master_branch where branch_id=(select cnt_branchid from tbl_master_contact where cnt_internalID=accountsledger_SubAccountID)) as BranchCode,case when a.Accountsledger_AmountDr='0.00000000' then null else a.Accountsledger_AmountDr end as Accountsledger_AmountCr,case when a.Accountsledger_AmountCr='0.00000000' then null else a.Accountsledger_AmountCr end as Accountsledger_AmountDr,'0.0' as Closing,'1/1/1900' as accountsledger_transactiondate,(select top 1 equity_effectuntil from master_equity where equity_exchsegmentid=2 and equity_effectuntil >= accountsledger_transactiondate order by equity_effectuntil) as ExpDate,(select case when (select top 1 equity_effectuntil from master_equity where equity_exchsegmentid=2 and equity_effectuntil < accountsledger_transactiondate order by equity_effectuntil desc)<'" + dtFrom.Value + "' then '" + dtFrom.Value + "' else (select top 1 DATEADD(dd, 1, CAST(equity_effectuntil AS datetime)) from master_equity where equity_exchsegmentid=2 and equity_effectuntil < accountsledger_transactiondate order by equity_effectuntil desc) end) as FDate,case when a.AccountsLedger_TransactionType='Journal' and Left(AccountsLedger_TransactionReferenceID,1) in('X','Z') then (select case when (select top 1 equity_effectuntil from master_equity where equity_exchsegmentid=2 and equity_effectuntil >= accountsledger_transactiondate order by equity_effectuntil)>'" + dtTo.Value + "' then '" + dtTo.Value + "' else ( select top 1 equity_effectuntil from master_equity where equity_exchsegmentid=2 and equity_effectuntil >= accountsledger_transactiondate order by equity_effectuntil) end) else accountsledger_transactiondate end as TDate from Trans_accountsledger a  WHERE  accountsledger_companyID='" + Session["CompanyID"].ToString() + "' and accountsledger_ExchangeSegmentID in(" + Segment + ") and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,accountsledger_transactiondate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtFrom.Value + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtTo.Value + "')) as datetime)   " + mainAccountSearch + " and AccountsLedger_BranchID in(" + Branch + ")  " + SubAccountSearch + @" and AccountsLedger_SubAccountID is not null and AccountsLedger_TransactionType<>'OpeningBalance' 
                                and isnull(AccountsLedger_Currency,
                                " + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0] + strSqlCb + strSqlJv + ") as D ", " case when TrDate is null then convert(varchar(11),TDate,113) else TrDate  end as TrDate,ValueDate, 0 as accountsledger_TransactionReferenceID,case when accountsledger_Narration='Consolidated Entries For the period' then accountsledger_Narration+convert(varchar(11),FDate,113)+' To '+convert(varchar(11),TDate,113) else accountsledger_Narration end as accountsledger_Narration,accountsledger_InstrumentNumber,SettlementNumber,PayoutDate,BranchCode,case when (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0)))<0 then cast((sum(isnull(Accountsledger_AmountCr,0))-sum(isnull(Accountsledger_AmountDr,0))) as varchar(max)) else null end Accountsledger_AmountCr,case when (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0)))>0 then cast((sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0))) as varchar(max)) else null end Accountsledger_AmountDr,Closing", null, " TrDate,ValueDate,accountsledger_InstrumentNumber,SettlementNumber,PayoutDate,BranchCode,Closing,accountsledger_transactiondate,accountsledger_Narration,FDate,TDate", " TDate");
                            }
                            else
                                dtCashBankBook1 = oDBEngine.GetDataTable("Trans_accountsledger a ", "convert(varchar(11),a.accountsledger_transactiondate,113) as TrDate,convert(varchar(11),a.accountsledger_valuedate,113) as ValueDate,a.accountsledger_TransactionReferenceID, isnull(a.accountsledger_Narration,'') +' - '+isnull(isnull((select MainAccount_Name from Master_MainAccount where MainAccount_AccountCode=AccountsLedger_CashBankName),AccountsLedger_CashBankName),'') as accountsledger_Narration,a.accountsledger_InstrumentNumber,(a.accountsledger_SettlementNumber+' '+a.accountsledger_SettlementType) as SettlementNumber,(select top 1 convert(varchar(12),Settlements_StartDateTime,113) from master_settlements where Settlements_Number=a.accountsledger_SettlementNumber and Settlements_TypeSuffix=a.accountsledger_SettlementType) as PayoutDate,(select branch_code from tbl_master_branch where branch_id=(select cnt_branchid from tbl_master_contact where cnt_internalID=accountsledger_SubAccountID)) as BranchCode,case when a.Accountsledger_AmountDr='0.00000000' then null else cast(a.Accountsledger_AmountDr as varchar(max)) end as Accountsledger_AmountCr,case when a.Accountsledger_AmountCr='0.00000000' then null else cast(a.Accountsledger_AmountCr as varchar(max)) end as Accountsledger_AmountDr,'0.0' as Closing", " accountsledger_companyID='" + Session["CompanyID"].ToString() + "' and accountsledger_ExchangeSegmentID in(" + Segment + ") and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,accountsledger_transactiondate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtFrom.Value + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtTo.Value + "')) as datetime)  " + mainAccountSearch + " and AccountsLedger_BranchID in(" + Branch + ") " + SubAccountSearch + @"  and AccountsLedger_SubAccountID is not null and AccountsLedger_TransactionType<>'OpeningBalance' and AccountsLedger_FinYear='" + Session["LastFinYear"].ToString() + @"'
                            and isnull(AccountsLedger_Currency,
                            " + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0] + strSqlCb + strSqlJv, " accountsledger_transactiondate,accountsledger_TransactionReferenceID");
                            OpenBalance = oDBEngine.OpeningBalanceJournal1(MainAcIDforOp, valItem, Convert.ToDateTime(dtFrom.Value), Segment, Session["CompanyID"].ToString(), Convert.ToDateTime(dtTo.Value), Convert.ToInt32(Session["ActiveCurrency"].ToString().Split('~')[0]));
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
                        if (ddlAccountType.SelectedItem.Value == "0")
                        {
                            Type = "Clients - Trading A/c  ";
                        }
                        else if (ddlAccountType.SelectedItem.Value == "1")
                        {
                            Type = "Clients - Margin Deposit A/c  ";
                        }
                        else if (ddlAccountType.SelectedItem.Value == "2")
                        {
                            Type = "Clients - Trading A/c and Clients - Margin Deposit A/c  ";
                        }
                        else if (ddlAccountType.SelectedItem.Value == "3")
                        {
                            Type = txtMainAccount.Text;
                        }
                        CheckingValueParam = Type + ": " + cmbclientsPager.Items[l].Text + " " + "Period :" + " " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "  - " + oconverter.ArrangeDate2(dtTo.Value.ToString());
                        newRow5[0] = Type + ": " + cmbclientsPager.Items[l].Text + " " + "Period :" + " " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "  - " + oconverter.ArrangeDate2(dtTo.Value.ToString());
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

                                if (dtCashBankBook1.Rows.Count == 4)
                                {
                                    if (dtCashBankBook1.Rows[k]["Accountsledger_AmountCr"].ToString() != "")
                                    {
                                        dtCashBankBook1.Rows[k]["Closing"] = dtCashBankBook1.Rows[k]["Accountsledger_AmountCr"];
                                    }
                                    else
                                    {
                                        dtCashBankBook1.Rows[k]["Closing"] = dtCashBankBook1.Rows[k]["Accountsledger_AmountDr"];
                                    }

                                }

                                if (dtCashBankBook1.Rows[k]["Closing"].ToString() != "")
                                {
                                    dtCashBankBook1.Rows[k]["Closing"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dtCashBankBook1.Rows[k]["Closing"].ToString()));

                                }


                                //else if (dtCashBankBook1.Rows[k]["Closing"].ToString() == "")
                                //{
                                //    dtCashBankBook1.Rows[k]["Closing"] = 0.00;
                                //}
                            }
                            if (dtCashBankBook1.Rows[dtCashBankBook1.Rows.Count - 1]["Closing"].ToString() == "")
                            {
                                dtCashBankBook1.Rows[dtCashBankBook1.Rows.Count - 1]["Closing"] = 0.00;
                            }

                        }
                        if (cmbclientsPager.Items.Count == 1)
                        {
                            dtLedger = dtCashBankBook1.Copy();
                        }
                        else
                        {
                            if (l == 0)
                            {
                                dtLedger = dtCashBankBook1.Copy();
                            }
                            else
                            {
                                if (l == cmbclientsPager.Items.Count - 1)
                                {
                                    dtLedger.Merge(dtCashBankBook1);
                                }
                                else
                                {
                                    dtLedger.Merge(dtCashBankBook1);
                                }
                            }
                        }
                    }
                    if (cmbclientsPager.Items.Count == 0)
                    {
                        receipt = 0;
                        Payment = 0;
                        SubAccountSearch = null;
                        string Sub = null;
                        if (SubAcID != null && SubAcID != "")
                        {
                            SubAccountSearch = " and AccountsLedger_SubAccountID in(" + SubAcID + ") ";
                            Sub = SubAcID.Replace("'", "");
                        }
                        //dtCashBankBook1 = oDBEngine.GetDataTable("Trans_accountsledger a ", "convert(varchar(11),a.accountsledger_transactiondate,113) as TrDate,convert(varchar(11),a.accountsledger_valuedate,113) as ValueDate,a.accountsledger_TransactionReferenceID, a.accountsledger_Narration,a.accountsledger_InstrumentNumber,(a.accountsledger_SettlementNumber+' '+a.accountsledger_SettlementType) as SettlementNumber,case when a.Accountsledger_AmountDr='0.00000000' then null else cast(a.Accountsledger_AmountDr as varchar(max)) end as Accountsledger_AmountCr,case when a.Accountsledger_AmountCr='0.00000000' then null else cast(a.Accountsledger_AmountCr as varchar(max)) end as Accountsledger_AmountDr,'0.0' as Closing", " accountsledger_companyID='" + Session["CompanyID"].ToString() + "' and accountsledger_ExchangeSegmentID in(" + Segment + ") and  cast(DATEADD(dd, 0, DATEDIFF(dd, 0,accountsledger_transactiondate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtFrom.Value + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtTo.Value + "')) as datetime)  " + mainAccountSearch + " and AccountsLedger_BranchID in(" + Branch + ") " + SubAccountSearch + "   and AccountsLedger_TransactionType<>'OpeningBalance'", " accountsledger_transactiondate,accountsledger_TransactionReferenceID");
                        if (radConsolidated.Checked == true)
                        {
                            if (radDateWise.Checked == true)
                            {
                                dtCashBankBook1 = oDBEngine.GetDataTable("(Select convert(varchar(11),a.accountsledger_transactiondate,113) as TrDate,case when a.accountsledger_valuedate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),a.accountsledger_valuedate,113) end as ValueDate,case when a.AccountsLedger_TransactionType='Journal' and Left(AccountsLedger_TransactionReferenceID,1) in('X','Y','Z') and accountsledger_SettlementNumber is not null and accountsledger_SettlementNumber<>''	then 'Consolidated Entries For '+convert(varchar(11),a.accountsledger_transactiondate,113) 	else isnull(a.accountsledger_Narration,'') end +' - '+isnull(isnull((select MainAccount_Name from Master_MainAccount where MainAccount_AccountCode=AccountsLedger_CashBankName),AccountsLedger_CashBankName),'') as accountsledger_Narration,a.accountsledger_InstrumentNumber,(a.accountsledger_SettlementNumber+' '+a.accountsledger_SettlementType) as SettlementNumber,(select top 1 convert(varchar(12),Settlements_StartDateTime,113) from master_settlements where Settlements_Number=a.accountsledger_SettlementNumber and Settlements_TypeSuffix=a.accountsledger_SettlementType) as PayoutDate,(select branch_code from tbl_master_branch where branch_id=(select cnt_branchid from tbl_master_contact where cnt_internalID=accountsledger_SubAccountID)) as BranchCode,case when a.Accountsledger_AmountDr='0.00000000' then null else a.Accountsledger_AmountDr end as Accountsledger_AmountCr,case when a.Accountsledger_AmountCr='0.00000000' then null else a.Accountsledger_AmountCr end as Accountsledger_AmountDr,'0.0' as Closing,accountsledger_transactiondate from Trans_accountsledger a  WHERE  accountsledger_companyID='" + Session["CompanyID"].ToString() + "' and accountsledger_ExchangeSegmentID in(" + Segment + ") and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,accountsledger_transactiondate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtFrom.Value + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtTo.Value + "')) as datetime)   " + mainAccountSearch + " and AccountsLedger_BranchID in(" + Branch + ")  " + SubAccountSearch + @"  and AccountsLedger_TransactionType<>'OpeningBalance' 
                            and isnull(AccountsLedger_Currency,
                           " + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0] + ") as D ", " TrDate,ValueDate, 0 as accountsledger_TransactionReferenceID,accountsledger_Narration,accountsledger_InstrumentNumber,SettlementNumber,PayoutDate,BranchCode,case when (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0)))<0 then cast((sum(isnull(Accountsledger_AmountCr,0))-sum(isnull(Accountsledger_AmountDr,0))) as varchar(max)) else null end Accountsledger_AmountCr,case when (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0)))>0 then cast((sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0))) as varchar(max)) else null end Accountsledger_AmountDr,Closing", null, " TrDate,ValueDate,accountsledger_InstrumentNumber,SettlementNumber,PayoutDate,BranchCode,Closing,accountsledger_transactiondate,accountsledger_Narration", " accountsledger_transactiondate");
                            }
                            else if (radVoucherWise.Checked == true)
                                dtCashBankBook1 = oDBEngine.GetDataTable("(Select convert(varchar(11),a.accountsledger_transactiondate,113) as TrDate,case when a.accountsledger_valuedate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),a.accountsledger_valuedate,113) end as ValueDate,isnull((select top 1 journalvoucher_Narration from trans_journalvoucher where journalvoucher_voucherNumber=a.accountsledger_TransactionReferenceID and journalvoucher_companyID=a.accountsledger_companyID and journalvoucher_ExchangeSegmentID=a.accountsledger_ExchangeSegmentID and journalvoucher_TransactionDate=a.accountsledger_transactiondate),isnull(accountsledger_Narration,'')) +' - '+isnull(isnull((select MainAccount_Name from Master_MainAccount where MainAccount_AccountCode=AccountsLedger_CashBankName),AccountsLedger_CashBankName),'') as accountsledger_Narration,a.accountsledger_InstrumentNumber,(a.accountsledger_SettlementNumber+' '+a.accountsledger_SettlementType) as SettlementNumber,(select top 1 convert(varchar(12),Settlements_StartDateTime,113) from master_settlements where Settlements_Number=a.accountsledger_SettlementNumber and Settlements_TypeSuffix=a.accountsledger_SettlementType) as PayoutDate,(select branch_code from tbl_master_branch where branch_id=(select cnt_branchid from tbl_master_contact where cnt_internalID=accountsledger_SubAccountID)) as BranchCode,case when a.Accountsledger_AmountDr='0.00000000' then null else a.Accountsledger_AmountDr end as Accountsledger_AmountCr,case when a.Accountsledger_AmountCr='0.00000000' then null else a.Accountsledger_AmountCr end as Accountsledger_AmountDr,'0.0' as Closing,accountsledger_transactiondate from Trans_accountsledger a  WHERE  accountsledger_companyID='" + Session["CompanyID"].ToString() + "' and accountsledger_ExchangeSegmentID in(" + Segment + ") and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,accountsledger_transactiondate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtFrom.Value + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtTo.Value + "')) as datetime)   " + mainAccountSearch + " and AccountsLedger_BranchID in(" + Branch + ")  " + SubAccountSearch + @"  and AccountsLedger_TransactionType<>'OpeningBalance' 
                            and isnull(AccountsLedger_Currency,
                            " + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0] + ") as D ", " TrDate,ValueDate, 0 as accountsledger_TransactionReferenceID,accountsledger_Narration,accountsledger_InstrumentNumber,SettlementNumber,PayoutDate,BranchCode,case when (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0)))<0 then cast((sum(isnull(Accountsledger_AmountCr,0))-sum(isnull(Accountsledger_AmountDr,0))) as varchar(max)) else null end Accountsledger_AmountCr,case when (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0)))>0 then cast((sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0))) as varchar(max)) else null end Accountsledger_AmountDr,Closing", null, " TrDate,ValueDate,accountsledger_InstrumentNumber,SettlementNumber,PayoutDate,BranchCode,Closing,accountsledger_transactiondate,accountsledger_Narration", " accountsledger_transactiondate");
                            else
                                dtCashBankBook1 = oDBEngine.GetDataTable("(Select case when a.AccountsLedger_TransactionType='Journal' and Left(AccountsLedger_TransactionReferenceID,1) in('X','Z') then null else convert(varchar(11),a.accountsledger_transactiondate,113) end as TrDate,case when a.AccountsLedger_TransactionType='Journal' and Left(AccountsLedger_TransactionReferenceID,1) in('X','Z') then null else (case when a.accountsledger_valuedate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),a.accountsledger_valuedate,113) end) end as ValueDate,case when a.AccountsLedger_TransactionType='Journal' and Left(AccountsLedger_TransactionReferenceID,1) in('X','Z') then 'Consolidated Entries For the period ' else accountsledger_Narration end as accountsledger_Narration,a.accountsledger_InstrumentNumber,'0' as SettlementNumber,'1/1/1900' as PayoutDate,(select branch_code from tbl_master_branch where branch_id=(select cnt_branchid from tbl_master_contact where cnt_internalID=accountsledger_SubAccountID)) as BranchCode,case when a.Accountsledger_AmountDr='0.00000000' then null else a.Accountsledger_AmountDr end as Accountsledger_AmountCr,case when a.Accountsledger_AmountCr='0.00000000' then null else a.Accountsledger_AmountCr end as Accountsledger_AmountDr,'0.0' as Closing,'1/1/1900' as accountsledger_transactiondate,,(select top 1 equity_effectuntil from master_equity where equity_exchsegmentid=2 and equity_effectuntil >= accountsledger_transactiondate order by equity_effectuntil) as ExpDate,(select case when (select top 1 equity_effectuntil from master_equity where equity_exchsegmentid=2 and equity_effectuntil < accountsledger_transactiondate order by equity_effectuntil desc)<'" + dtFrom.Value + "' then '" + dtFrom.Value + "' else (select top 1 DATEADD(dd, 1, CAST(equity_effectuntil AS datetime)) from master_equity where equity_exchsegmentid=2 and equity_effectuntil < accountsledger_transactiondate order by equity_effectuntil desc) end) as FDate,case when a.AccountsLedger_TransactionType='Journal' and Left(AccountsLedger_TransactionReferenceID,1) in('X','Z') then (select case when (select top 1 equity_effectuntil from master_equity where equity_exchsegmentid=2 and equity_effectuntil >= accountsledger_transactiondate order by equity_effectuntil)>'" + dtTo.Value + "' then '" + dtTo.Value + "' else ( select top 1 equity_effectuntil from master_equity where equity_exchsegmentid=2 and equity_effectuntil >= accountsledger_transactiondate order by equity_effectuntil) end) else accountsledger_transactiondate end as TDate from Trans_accountsledger a  WHERE  accountsledger_companyID='" + Session["CompanyID"].ToString() + "' and accountsledger_ExchangeSegmentID in(" + Segment + ") and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,accountsledger_transactiondate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtFrom.Value + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtTo.Value + "')) as datetime)   " + mainAccountSearch + " and AccountsLedger_BranchID in(" + Branch + ")  " + SubAccountSearch + @"  and AccountsLedger_TransactionType<>'OpeningBalance' 
                            and isnull(AccountsLedger_Currency,
                            " + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0] + ") as D ", " case when TrDate is null then convert(varchar(11),TDate,113) else TrDate end as TrDate,ValueDate, 0 as accountsledger_TransactionReferenceID,case when accountsledger_Narration='Consolidated Entries For the period' then accountsledger_Narration+convert(varchar(11),FDate,113)+' To '+convert(varchar(11),TDate,113) else accountsledger_Narration end as accountsledger_Narration,accountsledger_InstrumentNumber,SettlementNumber,PayoutDate,BranchCode,case when (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0)))<0 then cast((sum(isnull(Accountsledger_AmountCr,0))-sum(isnull(Accountsledger_AmountDr,0))) as varchar(max)) else null end Accountsledger_AmountCr,case when (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0)))>0 then cast((sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0))) as varchar(max)) else null end Accountsledger_AmountDr,Closing", null, " TrDate,ValueDate,accountsledger_InstrumentNumber,SettlementNumber,PayoutDate,BranchCode,Closing,accountsledger_transactiondate,accountsledger_Narration,FDate,TDate", " TDate");
                        }
                        else
                            dtCashBankBook1 = oDBEngine.GetDataTable("Trans_accountsledger a ", "convert(varchar(11),a.accountsledger_transactiondate,113) as TrDate,case when a.accountsledger_valuedate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),a.accountsledger_valuedate,113) end as ValueDate,a.accountsledger_TransactionReferenceID, isnull(a.accountsledger_Narration,'') +' - '+isnull(isnull((select MainAccount_Name from Master_MainAccount where MainAccount_AccountCode=AccountsLedger_CashBankName),AccountsLedger_CashBankName),'') as accountsledger_Narration,a.accountsledger_InstrumentNumber,(a.accountsledger_SettlementNumber+' '+a.accountsledger_SettlementType) as SettlementNumber,(select top 1 convert(varchar(12),Settlements_StartDateTime,113) from master_settlements where Settlements_Number=a.accountsledger_SettlementNumber and Settlements_TypeSuffix=a.accountsledger_SettlementType) as PayoutDate,(select branch_code from tbl_master_branch where branch_id=(select cnt_branchid from tbl_master_contact where cnt_internalID=accountsledger_SubAccountID)) as BranchCode,case when a.Accountsledger_AmountDr='0.00000000' then null else cast(a.Accountsledger_AmountDr as varchar(max)) end as Accountsledger_AmountCr,case when a.Accountsledger_AmountCr='0.00000000' then null else cast(a.Accountsledger_AmountCr as varchar(max)) end as Accountsledger_AmountDr,'0.0' as Closing", " accountsledger_companyID='" + Session["CompanyID"].ToString() + "' and accountsledger_ExchangeSegmentID in(" + Segment + ") and  cast(DATEADD(dd, 0, DATEDIFF(dd, 0,accountsledger_transactiondate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtFrom.Value + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtTo.Value + "')) as datetime)  " + mainAccountSearch + " and AccountsLedger_BranchID in(" + Branch + ") " + SubAccountSearch + @"   and AccountsLedger_TransactionType<>'OpeningBalance'
                        and isnull(AccountsLedger_Currency,
                        " + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0], " accountsledger_transactiondate,accountsledger_TransactionReferenceID");

                        OpenBalance = oDBEngine.OpeningBalanceJournal1(MainAcIDforOp, Sub, Convert.ToDateTime(dtFrom.Value), Segment, Session["CompanyID"].ToString(), Convert.ToDateTime(dtTo.Value), Convert.ToInt32(Session["ActiveCurrency"].ToString().Split('~')[0]));
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
                        if (ddlAccountType.SelectedItem.Value == "0")
                        {
                            Type = "Clients - Trading A/c  ";
                        }
                        else if (ddlAccountType.SelectedItem.Value == "1")
                        {
                            Type = "Clients - Margin Deposit A/c  ";
                        }
                        else if (ddlAccountType.SelectedItem.Value == "2")
                        {
                            Type = "Clients - Trading A/c and Clients - Margin Deposit A/c  ";
                        }
                        else if (ddlAccountType.SelectedItem.Value == "3")
                        {
                            Type = txtMainAccount.Text;
                        }
                        CheckingValueParam = Type + ": Period :" + " " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "  - " + oconverter.ArrangeDate2(dtTo.Value.ToString());
                        newRow5[0] = Type + ": Period :" + " " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "  - " + oconverter.ArrangeDate2(dtTo.Value.ToString());
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
                        dtLedger = dtCashBankBook1.Copy();
                    }
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
                    if (radConsolidated.Checked == true)
                        dtExport.Columns.Remove("Voucher No.");
                    //if (Session["userlastsegment"].ToString() != "7" || Session["userlastsegment"].ToString() == "18")
                    //{
                    //    dtExport.Columns.Remove("Trade Date");
                    //    dtExport.Columns.Remove("Settlement No.");
                    //}
                    if (Session["userlastsegment"].ToString() != "7" && Session["userlastsegment"].ToString() != "8" || Session["userlastsegment"].ToString() == "18")
                        dtExport.Columns.Remove("Branch Code");
                    dtExport.AcceptChanges();

                    //For the Purpose Of Showing Or Not Showing Value Date
                    DataTable DtOriginal = new DataTable();
                    DataTable DtModified = new DataTable();
                    DtOriginal = dtExport;
                    DtModified = DtOriginal.Clone();

                    foreach (DataRow Row in DtOriginal.Rows)
                        DtModified.ImportRow(Row);
                    if (!ChkShowValueDate.Checked)
                        DtModified.Columns.RemoveAt(1);

                    //====Check CM Selection To show Settlement No and Trade Date======
                    string isCMSelected = "";
                    //DataTable DtCheckForCM = oDBEngine.GetDataTable("Select * From tbl_master_companyExchange where exch_internalId in (" + Segment + ") And exch_segmentId='CM'");
                    DataTable DtCheckForCM = oDBEngine.GetDataTable("Select * From tbl_master_companyExchange where exch_internalId in (" + Segment + ") And exch_segmentId<>'CM'");
                    if (DtCheckForCM != null)
                    {
                        if (DtCheckForCM.Rows.Count > 0)
                            isCMSelected = "false";
                        else
                            isCMSelected = "true";
                    }
                    DtCheckForCM.Dispose();

                    if (isCMSelected != "true")
                    {
                        if (DtModified.Columns["SegmentName"] != null)
                            DtModified.Columns.Remove("SegmentName");
                        if (DtModified.Columns["Settlement No."] != null)
                            DtModified.Columns.Remove("Settlement No.");
                        if (DtModified.Columns["Trade Date"] != null)
                            DtModified.Columns.Remove("Trade Date");

                    }
                    DtModified.AcceptChanges();

                    if (ddlExport.SelectedItem.Value == "E")
                    {
                        objExcel.ExportToExcelforExcel(DtModified, "Ledger", "Closing Balance", dtReportHeader, dtReportFooter);
                    }
                    else if (ddlExport.SelectedItem.Value == "P")
                    {
                        objExcel.ExportToPDF(DtModified, "Ledger", "Closing Balance", dtReportHeader, dtReportFooter);
                    }
                }
            }
        }
        protected void btnExExport_Click(object sender, EventArgs e)
        {
            //FillGrid();
            FetchDataToExport();
            string SelectedSegmentName = null;
            DataTable CompanySegmentDeatil = null;
            oGenericMethod = new BusinessLogicLayer.GenericMethod();
            CompanySegmentDeatil = oGenericMethod.GetCompanyDetail(Session["LastCompany"].ToString(), ViewState["Segment"].ToString());
            foreach (DataRow dr in CompanySegmentDeatil.Rows)
            {
                if (SelectedSegmentName == null)
                    SelectedSegmentName = dr[0].ToString().Trim();
                else
                    SelectedSegmentName = SelectedSegmentName + "," + dr[0].ToString().Trim();
            }
            if (radBreakDetail.Checked == true)
            {
                EmailInd = "Y";
                this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript3", "<script>ForFilterOff();</script>");
                DataTable dtComp = oDBEngine.GetDataTable("tbl_master_company", "cmp_name", "cmp_internalid='" + Session["LastCompany"].ToString() + "'");
                string CompName = dtComp.Rows[0][0].ToString();
                string ExHTML = "";
                ExHTML = "<table  border=\"1\" width=\"100%\" cellspacing=\"0\" cellpadding=\"0\">";
                DataTable dtCL = (DataTable)Session["Client"];
                for (int n = 0; n < dtCL.Rows.Count; n++)
                {
                    cmbclientsPager.SelectedItem.Value = dtCL.Rows[n]["subaccount_code"].ToString();
                    cmbclientsPager.SelectedItem.Text = dtCL.Rows[n]["subaccount_name"].ToString();
                    DrpChange = "Y";
                    FillGrid();
                    string Type = "";
                    if (ddlAccountType.SelectedItem.Value == "0")
                    {
                        Type = "Clients - Trading A/c  ";
                    }
                    else if (ddlAccountType.SelectedItem.Value == "1")
                    {
                        Type = "Clients - Margin Deposit A/c  ";
                    }
                    else if (ddlAccountType.SelectedItem.Value == "2")
                    {
                        Type = "Clients - Trading A/c and Clients - Margin Deposit A/c  ";
                    }
                    else if (ddlAccountType.SelectedItem.Value == "3")
                    {
                        Type = txtMainAccount.Text;
                    }
                    string billdate = Type + ": Period :" + " " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "  - " + oconverter.ArrangeDate2(dtTo.Value.ToString());
                    string emailbdy = ViewState["EmailHTML"].ToString();
                    string contactid = dtCL.Rows[n]["subaccount_code"].ToString();
                    string Subject = "[" + SelectedSegmentName + "] : Ledger View For " + billdate;

                    ExHTML = ExHTML + "<tr><td align=\"center\" style=\"background-color:#EEE0E5;font-weight:bold;\">" + CompName + "</td></tr>";
                    ExHTML = ExHTML + "<tr><td align=\"center\" style=\"background-color:#EEE0E5;font-weight:bold;\">" + cmbclientsPager.SelectedItem.Text + "</td></tr>";
                    ExHTML = ExHTML + "<tr><td align=\"center\" style=\"background-color:#EEE0E5;font-weight:bold;\">" + billdate + "</td></tr>";
                    if (rdbSegAll.Checked == true)
                    {
                        ExHTML = ExHTML + "<tr><td align=\"center\" style=\"background-color:#EEE0E5;font-weight:bold;\">Segment:ALL</td></tr>";
                    }
                    else
                    {
                        ExHTML = ExHTML + "<tr><td align=\"center\" style=\"background-color:#EEE0E5;font-weight:bold;\">Segment:" + litSegment.InnerText.ToString() + "</td></tr>";

                    }
                    ExHTML = ExHTML + "<tr><td align=\"center\" style=\"font-weight:bold;\">" + ViewState["EmailHTML"].ToString() + "</td></tr>";
                    ExHTML = ExHTML + "<tr><td></td></tr>";
                    ViewState["EmailHTML"] = "";

                }
                ExHTML = ExHTML + "</table>";

                Response.AppendHeader("Content-Disposition", "attachment; filename=Ledger.xls");
                Response.ContentType = "application/ms-excel";
                Response.Write(ExHTML.ToString());
                Response.End();

            }
            else
            { //========Modify For New Excel====

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

                dtCashBankBook = (DataTable)ViewState["dtCashBankBook"];
                dtLedgerView = (DataTable)ViewState["dtLedgerView"];
                DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
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
                string strTranType = "";
                string strCbPayment = "n";
                string strCbReceipt = "n";
                string strCbContract = "n";
                string strJvType = "";
                string strSelectedJv = "";


                if (rdbSegAll.Checked == true)
                {
                    DataTable dtSegment = oDBEngine.GetDataTable("tbl_master_companyExchange", "exch_internalId", "exch_compId='" + Session["CompanyID"].ToString() + "'");
                    if (dtSegment.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtSegment.Rows.Count; i++)
                        {
                            if (Segment == null)
                                Segment = dtSegment.Rows[i][0].ToString();
                            else
                                Segment += "," + dtSegment.Rows[i][0].ToString();
                        }
                    }
                }
                if (rdbranchAll.Checked == true)
                {
                    Branch = Session["userbranchHierarchy"].ToString();
                }
                else
                    Branch = ViewState["branchID"].ToString();
                if (Segment == null)
                {
                    Segment = Session["SegmentID"].ToString();
                }
                string mainAccountSearch = null;
                string SubAccountSearch = null;
                //if (ddlAccountType.SelectedValue == "0")
                //{
                //    mainAccountSearch = "and accountsledger_MainAccountID in('SYSTM00001')";
                //    MainAcIDforOp = "'SYSTM00001'";
                //}
                //else if (ddlAccountType.SelectedValue == "1")
                //{
                //    mainAccountSearch = "and accountsledger_MainAccountID in('SYSTM00002')";
                //    MainAcIDforOp = "'SYSTM00002'";
                //}
                //else if (ddlAccountType.SelectedValue == "2")
                //{
                //    mainAccountSearch = "and accountsledger_MainAccountID in('SYSTM00001','SYSTM00002')";
                //    MainAcIDforOp = "'SYSTM00001','SYSTM00002'";
                //}
                //else
                //{
                //    mainAccountSearch = "and accountsledger_MainAccountID in(" + MainAcID + ")";
                //}

                if (ddlAccountType.SelectedValue == "0")
                {

                    if (MainAcID == "" || MainAcID == null)
                    {
                        mainAccountSearch = "and accountsledger_MainAccountID in('SYSTM00001')";
                        MainAcIDforOp = "'SYSTM00001'";
                    }
                    else
                    {
                        mainAccountSearch = "and accountsledger_MainAccountID in(" + MainAcID + ")";
                        MainAcIDforOp = MainAcID;
                    }
                    if (SubLedgerType == "")
                        SubLedgerType = "Customers";

                }
                else if (ddlAccountType.SelectedValue == "1")
                {

                    if (MainAcID == "" || MainAcID == null)
                    {
                        mainAccountSearch = "and accountsledger_MainAccountID in('SYSTM00002')";
                        MainAcIDforOp = "'SYSTM00002'";
                    }
                    else
                    {
                        mainAccountSearch = "and accountsledger_MainAccountID in(" + MainAcID + ")";
                        MainAcIDforOp = MainAcID;
                    }
                    if (SubLedgerType == "")
                        SubLedgerType = "Customers";
                }
                else if (ddlAccountType.SelectedValue == "2")
                {

                    if (MainAcID == "" || MainAcID == null)
                    {

                        mainAccountSearch = "and accountsledger_MainAccountID in('SYSTM00001','SYSTM00002')";
                        MainAcIDforOp = "'SYSTM00001','SYSTM00002'";

                    }
                    else
                    {
                        mainAccountSearch = "and accountsledger_MainAccountID in('" + MainAcID + "')";
                        MainAcIDforOp = MainAcID;
                    }

                    if (SubLedgerType == "")
                        SubLedgerType = "Customers";
                }
                else
                {
                    mainAccountSearch = "and accountsledger_MainAccountID in(" + MainAcID + ")";
                }


                if (rbTanAll.Checked == true)
                    strTranType = "all";
                else if (rbTranCashBank.Checked == true)
                {
                    strTranType = "cb";
                    if (chkPayment.Checked == true)
                        strCbPayment = "y";
                    if (chkReceipts.Checked == true)
                        strCbReceipt = "y";
                    if (chkContracts.Checked == true)
                        strCbContract = "y";

                }
                else if (rbTranJv.Checked == true)
                {
                    strTranType = "jv";
                    if (rbAllJV.Checked == true)
                        strJvType = "all";
                    else if (rbManualJV.Checked == true)
                    {
                        strJvType = "man";
                        strSelectedJv = txtVoucherPrefix.Text;
                    }
                    else if (rbSystemJV.Checked == true)
                    {
                        strJvType = "sys";
                        //strSelectedJv = ViewState["Selectedjvs"].ToString();
                        if (rbSystemJVAll.Checked == true)
                            strSelectedJv = "all";
                        else if (rbSystemJVSelected.Checked == true)
                            strSelectedJv = hdnSystemJvs.Value;
                    }

                }

                string strSqlCb = "";
                string strSqlJv = "";

                if (strTranType == "cb")
                {
                    strSqlCb = " and  AccountsLedger_TransactionType='Cash_Bank' ";
                    if (strCbPayment == "y" && strCbReceipt != "y" && strCbContract != "y")
                        strSqlCb = strSqlCb + " and patindex('%/%',AccountsLedger_TransactionReferenceID)>0 and  left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1)='P' ";

                    if (strCbPayment != "y" && strCbReceipt == "y" && strCbContract != "y")
                        strSqlCb = strSqlCb + " and patindex('%/%',AccountsLedger_TransactionReferenceID)>0 and left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1)='R' ";

                    if (strCbPayment != "y" && strCbReceipt != "y" && strCbContract == "y")
                        strSqlCb = strSqlCb + " and patindex('%/%',AccountsLedger_TransactionReferenceID)>0 and left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1)='C' ";

                    if (strCbPayment == "y" && strCbReceipt == "y" && strCbContract != "y")
                        strSqlCb = strSqlCb + "  and patindex('%/%',AccountsLedger_TransactionReferenceID)>0 and  (left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1)='P' or left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1)='R') ";
                    if (strCbPayment != "y" && strCbReceipt == "y" && strCbContract == "y")
                        strSqlCb = strSqlCb + " and patindex('%/%',AccountsLedger_TransactionReferenceID)>0 and (left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1)='R' or left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1)='C') ";
                    if (strCbPayment == "y" && strCbReceipt != "y" && strCbContract == "y")
                        strSqlCb = strSqlCb + " and patindex('%/%',AccountsLedger_TransactionReferenceID)>0 and  (left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1)='P' or left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1)='C') ";
                    if (strCbPayment == "y" && strCbReceipt == "y" && strCbContract == "y")
                        strSqlCb = strSqlCb + " and patindex('%/%',AccountsLedger_TransactionReferenceID)>0 and  (left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1)='P' or left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1)='R' or left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1)='C') ";
                    if (strCbPayment != "y" && strCbReceipt != "y" && strCbContract != "y")
                        strSqlCb = strSqlCb + "  and patindex('%/%',AccountsLedger_TransactionReferenceID)>0 and  (left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1)!='P' and left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1)!='R' and left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1)!='C') ";


                }
                if (strTranType == "jv")
                {
                    if (strJvType == "all")
                        strSqlJv = " and  AccountsLedger_TransactionType='Journal' ";
                    else if (strJvType == "man")
                        if (strSelectedJv == "??")
                        {
                            strSqlJv = " and  patindex('%/%',AccountsLedger_TransactionReferenceID)>0 and AccountsLedger_TransactionType='Journal' and left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1) not like 'u%' and left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1) not like 'v%' and left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1) not like 'w%' and left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1) not like 'x%' and left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1) not like 'y%' and left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1) not like 'z%' ";

                        }
                        else
                            strSqlJv = " and  patindex('%/%',AccountsLedger_TransactionReferenceID)>0 and AccountsLedger_TransactionType='Journal' and left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1)='" + strSelectedJv + "' ";
                    else if (strJvType == "sys")
                        if (strSelectedJv == "all")
                        {
                            strSqlJv = " and  patindex('%/%',AccountsLedger_TransactionReferenceID)>0 and AccountsLedger_TransactionType='Journal' and (left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1) like 'u%' or left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1) like 'v%' or left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1) like 'w%' or left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1) like 'w%' or left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1) like 'x%' or left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1) like 'y%' or left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1) like 'z%' )";

                        }
                        else
                            strSqlJv = " and  patindex('%/%',AccountsLedger_TransactionReferenceID)>0 and AccountsLedger_TransactionType='Journal' and left(AccountsLedger_TransactionReferenceID,patindex('%/%',AccountsLedger_TransactionReferenceID)-1) in(" + strSelectedJv + ") ";


                }






                for (int l = 0; l < cmbclientsPager.Items.Count; l++)
                {
                    receipt = 0;
                    Payment = 0;
                    string valItem = cmbclientsPager.Items[l].Value;
                    if (SubLedgerType.Trim() == "None")
                    {
                        SubAccountSearch = null;
                        if (radConsolidated.Checked == true)
                        {
                            if (radDateWise.Checked == true)
                            {
                                dtCashBankBook1 = oDBEngine.GetDataTable("(Select convert(varchar(11),a.accountsledger_transactiondate,113) as TrDate,case when a.accountsledger_valuedate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),a.accountsledger_valuedate,113) end as ValueDate,case when a.AccountsLedger_TransactionType='Journal' and Left(AccountsLedger_TransactionReferenceID,1) in('X','Y','Z') and accountsledger_SettlementNumber is not null and accountsledger_SettlementNumber<>''	then 'Consolidated Entries For '+convert(varchar(11),a.accountsledger_transactiondate,113) 	else isnull(a.accountsledger_Narration,'') end +' - '+isnull(isnull((select MainAccount_Name from Master_MainAccount where MainAccount_AccountCode=AccountsLedger_CashBankName),AccountsLedger_CashBankName),'') as accountsledger_Narration,a.accountsledger_InstrumentNumber,(a.accountsledger_SettlementNumber+' '+a.accountsledger_SettlementType) as SettlementNumber,(select top 1 convert(varchar(12),Settlements_StartDateTime,113) from master_settlements where Settlements_Number=a.accountsledger_SettlementNumber and Settlements_TypeSuffix=a.accountsledger_SettlementType) as PayoutDate,(select branch_code from tbl_master_branch where branch_id=(select cnt_branchid from tbl_master_contact where cnt_internalID=accountsledger_SubAccountID)) as BranchCode,case when a.Accountsledger_AmountDr='0.00000000' then null else a.Accountsledger_AmountDr end as Accountsledger_AmountCr,case when a.Accountsledger_AmountCr='0.00000000' then null else a.Accountsledger_AmountCr end as Accountsledger_AmountDr,'0.0' as Closing,accountsledger_transactiondate from Trans_accountsledger a  WHERE  accountsledger_companyID='" + Session["CompanyID"].ToString() + "' and accountsledger_ExchangeSegmentID in(" + Segment + ") and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,accountsledger_transactiondate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtFrom.Value + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtTo.Value + "')) as datetime)   " + mainAccountSearch + " and AccountsLedger_BranchID in(" + Branch + ")  " + SubAccountSearch + @"  and AccountsLedger_TransactionType<>'OpeningBalance' 
                            and isnull(AccountsLedger_Currency,
                           " + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0] + ") as D ", " TrDate,ValueDate, 0 as accountsledger_TransactionReferenceID,accountsledger_Narration,accountsledger_InstrumentNumber,SettlementNumber,PayoutDate,BranchCode,case when (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0)))<0 then cast((sum(isnull(Accountsledger_AmountCr,0))-sum(isnull(Accountsledger_AmountDr,0))) as varchar(max)) else null end Accountsledger_AmountCr,case when (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0)))>0 then cast((sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0))) as varchar(max)) else null end Accountsledger_AmountDr,Closing", null, " TrDate,ValueDate,accountsledger_InstrumentNumber,SettlementNumber,PayoutDate,BranchCode,Closing,accountsledger_transactiondate,accountsledger_Narration", " accountsledger_transactiondate");
                            }
                            else if (radVoucherWise.Checked == true)
                                dtCashBankBook1 = oDBEngine.GetDataTable("(Select convert(varchar(11),a.accountsledger_transactiondate,113) as TrDate,case when a.accountsledger_valuedate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),a.accountsledger_valuedate,113) end as ValueDate,isnull((select top 1 journalvoucher_Narration from trans_journalvoucher where journalvoucher_voucherNumber=a.accountsledger_TransactionReferenceID and journalvoucher_companyID=a.accountsledger_companyID and journalvoucher_ExchangeSegmentID=a.accountsledger_ExchangeSegmentID and journalvoucher_TransactionDate=a.accountsledger_transactiondate),isnull(accountsledger_Narration,'')) +' - '+isnull(isnull((select MainAccount_Name from Master_MainAccount where MainAccount_AccountCode=AccountsLedger_CashBankName),AccountsLedger_CashBankName),'') as accountsledger_Narration,a.accountsledger_InstrumentNumber,(a.accountsledger_SettlementNumber+' '+a.accountsledger_SettlementType) as SettlementNumber,(select top 1 convert(varchar(12),Settlements_StartDateTime,113) from master_settlements where Settlements_Number=a.accountsledger_SettlementNumber and Settlements_TypeSuffix=a.accountsledger_SettlementType) as PayoutDate,(select branch_code from tbl_master_branch where branch_id=(select cnt_branchid from tbl_master_contact where cnt_internalID=accountsledger_SubAccountID)) as BranchCode,case when a.Accountsledger_AmountDr='0.00000000' then null else a.Accountsledger_AmountDr end as Accountsledger_AmountCr,case when a.Accountsledger_AmountCr='0.00000000' then null else a.Accountsledger_AmountCr end as Accountsledger_AmountDr,'0.0' as Closing,accountsledger_transactiondate from Trans_accountsledger a  WHERE  accountsledger_companyID='" + Session["CompanyID"].ToString() + "' and accountsledger_ExchangeSegmentID in(" + Segment + ") and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,accountsledger_transactiondate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtFrom.Value + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtTo.Value + "')) as datetime)   " + mainAccountSearch + " and AccountsLedger_BranchID in(" + Branch + ")  " + SubAccountSearch + @"  and AccountsLedger_TransactionType<>'OpeningBalance' 
                                and isnull(AccountsLedger_Currency,
                                " + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0] + ") as D ", " TrDate,ValueDate, 0 as accountsledger_TransactionReferenceID,accountsledger_Narration,accountsledger_InstrumentNumber,SettlementNumber,PayoutDate,BranchCode,case when (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0)))<0 then cast((sum(isnull(Accountsledger_AmountCr,0))-sum(isnull(Accountsledger_AmountDr,0))) as varchar(max)) else null end Accountsledger_AmountCr,case when (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0)))>0 then cast((sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0))) as varchar(max)) else null end Accountsledger_AmountDr,Closing", null, " TrDate,ValueDate,accountsledger_InstrumentNumber,SettlementNumber,PayoutDate,BranchCode,Closing,accountsledger_transactiondate,accountsledger_Narration", " accountsledger_transactiondate");
                            else
                                dtCashBankBook1 = oDBEngine.GetDataTable("(Select case when a.AccountsLedger_TransactionType='Journal' and Left(AccountsLedger_TransactionReferenceID,1) in('X','Z') then null else convert(varchar(11),a.accountsledger_transactiondate,113) end as TrDate,case when a.AccountsLedger_TransactionType='Journal' and Left(AccountsLedger_TransactionReferenceID,1) in('X','Z') then null else (case when a.accountsledger_valuedate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),a.accountsledger_valuedate,113) end) end as ValueDate,case when a.AccountsLedger_TransactionType='Journal' and Left(AccountsLedger_TransactionReferenceID,1) in('X','Z') then 'Consolidated Entries For the period ' else accountsledger_Narration end as accountsledger_Narration,a.accountsledger_InstrumentNumber,'0' as SettlementNumber,'1/1/1900' as PayoutDate,(select branch_code from tbl_master_branch where branch_id=(select cnt_branchid from tbl_master_contact where cnt_internalID=accountsledger_SubAccountID)) as BranchCode,case when a.Accountsledger_AmountDr='0.00000000' then null else a.Accountsledger_AmountDr end as Accountsledger_AmountCr,case when a.Accountsledger_AmountCr='0.00000000' then null else a.Accountsledger_AmountCr end as Accountsledger_AmountDr,'0.0' as Closing,'1/1/1900' as accountsledger_transactiondate,(select top 1 equity_effectuntil from master_equity where equity_exchsegmentid=2 and equity_effectuntil >= accountsledger_transactiondate order by equity_effectuntil) as ExpDate,(select case when (select top 1 equity_effectuntil from master_equity where equity_exchsegmentid=2 and equity_effectuntil < accountsledger_transactiondate order by equity_effectuntil desc)<'" + dtFrom.Value + "' then '" + dtFrom.Value + "' else (select top 1 DATEADD(dd, 1, CAST(equity_effectuntil AS datetime)) from master_equity where equity_exchsegmentid=2 and equity_effectuntil < accountsledger_transactiondate order by equity_effectuntil desc) end) as FDate,case when a.AccountsLedger_TransactionType='Journal' and Left(AccountsLedger_TransactionReferenceID,1) in('X','Z') then (select case when (select top 1 equity_effectuntil from master_equity where equity_exchsegmentid=2 and equity_effectuntil >= accountsledger_transactiondate order by equity_effectuntil)>'" + dtTo.Value + "' then '" + dtTo.Value + "' else ( select top 1 equity_effectuntil from master_equity where equity_exchsegmentid=2 and equity_effectuntil >= accountsledger_transactiondate order by equity_effectuntil) end) else accountsledger_transactiondate end as TDate from Trans_accountsledger a  WHERE  accountsledger_companyID='" + Session["CompanyID"].ToString() + "' and accountsledger_ExchangeSegmentID in(" + Segment + ") and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,accountsledger_transactiondate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtFrom.Value + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtTo.Value + "')) as datetime)   " + mainAccountSearch + " and AccountsLedger_BranchID in(" + Branch + ")  " + SubAccountSearch + @"  and AccountsLedger_TransactionType<>'OpeningBalance' 
                            and isnull(AccountsLedger_Currency,
                            " + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0] + ") as D ", " case when TrDate is null then convert(varchar(11),TDate,113) else TrDate end as TrDate,ValueDate, 0 as accountsledger_TransactionReferenceID,case when accountsledger_Narration='Consolidated Entries For the period' then accountsledger_Narration+convert(varchar(11),FDate,113)+' To '+convert(varchar(11),TDate,113) else accountsledger_Narration end as accountsledger_Narration,accountsledger_InstrumentNumber,SettlementNumber,PayoutDate,BranchCode,case when (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0)))<0 then cast((sum(isnull(Accountsledger_AmountCr,0))-sum(isnull(Accountsledger_AmountDr,0))) as varchar(max)) else null end Accountsledger_AmountCr,case when (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0)))>0 then cast((sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0))) as varchar(max)) else null end Accountsledger_AmountDr,Closing", null, " TrDate,ValueDate,accountsledger_InstrumentNumber,SettlementNumber,PayoutDate,BranchCode,Closing,accountsledger_transactiondate,accountsledger_Narration,FDate,TDate", " TDate");
                        }
                        else
                            dtCashBankBook1 = oDBEngine.GetDataTable("Trans_accountsledger a ", "convert(varchar(11),a.accountsledger_transactiondate,113) as TrDate,case when a.accountsledger_valuedate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),a.accountsledger_valuedate,113) end as ValueDate,a.accountsledger_TransactionReferenceID, isnull(a.accountsledger_Narration,'') +' - '+isnull(isnull((select MainAccount_Name from Master_MainAccount where MainAccount_AccountCode=AccountsLedger_CashBankName),AccountsLedger_CashBankName),'') as accountsledger_Narration,a.accountsledger_InstrumentNumber,(a.accountsledger_SettlementNumber+' '+a.accountsledger_SettlementType) as SettlementNumber,(select top 1 convert(varchar(12),Settlements_StartDateTime,113) from master_settlements where Settlements_Number=a.accountsledger_SettlementNumber and Settlements_TypeSuffix=a.accountsledger_SettlementType) as PayoutDate,(select branch_code from tbl_master_branch where branch_id=(select cnt_branchid from tbl_master_contact where cnt_internalID=accountsledger_SubAccountID)) as BranchCode,case when a.Accountsledger_AmountDr='0.00000000' then null else cast(a.Accountsledger_AmountDr as varchar(max)) end as Accountsledger_AmountCr,case when a.Accountsledger_AmountCr='0.00000000' then null else cast(a.Accountsledger_AmountCr as varchar(max)) end as Accountsledger_AmountDr,'0.0' as Closing", " accountsledger_companyID='" + Session["CompanyID"].ToString() + "' and accountsledger_ExchangeSegmentID in(" + Segment + ") and  cast(DATEADD(dd, 0, DATEDIFF(dd, 0,accountsledger_transactiondate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtFrom.Value + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtTo.Value + "')) as datetime)  " + mainAccountSearch + " and AccountsLedger_BranchID in(" + Branch + ") " + SubAccountSearch + @"   and AccountsLedger_TransactionType<>'OpeningBalance'
                        and isnull(AccountsLedger_Currency,
                        " + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0] + " and accountsledger_finyear='" + Session["LastFinYear"].ToString() + "'" + strSqlCb + strSqlJv, " accountsledger_transactiondate,accountsledger_TransactionReferenceID");
                        OpenBalance = oDBEngine.OpeningBalanceJournal1(MainAcIDforOp, null, Convert.ToDateTime(dtFrom.Value), Segment, Session["CompanyID"].ToString(), Convert.ToDateTime(dtTo.Value), Convert.ToInt32(Session["ActiveCurrency"].ToString().Split('~')[0]));
                    }
                    else
                    {
                        SubAccountSearch = " and AccountsLedger_SubAccountID in('" + valItem + "') ";
                        if (radConsolidated.Checked == true)
                        {
                            if (radDateWise.Checked == true)
                            {
                                dtCashBankBook1 = oDBEngine.GetDataTable("(Select convert(varchar(11),a.accountsledger_transactiondate,113) as TrDate,case when a.accountsledger_valuedate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),a.accountsledger_valuedate,113) end as ValueDate,case when a.AccountsLedger_TransactionType='Journal' and Left(AccountsLedger_TransactionReferenceID,1) in('X','Y','Z') and accountsledger_SettlementNumber is not null and accountsledger_SettlementNumber<>''	then 'Consolidated Entries For '+convert(varchar(11),a.accountsledger_transactiondate,113) 	else isnull(a.accountsledger_Narration,'') end +' - '+isnull(isnull((select MainAccount_Name from Master_MainAccount where MainAccount_AccountCode=AccountsLedger_CashBankName),AccountsLedger_CashBankName),'') as accountsledger_Narration,a.accountsledger_InstrumentNumber,(a.accountsledger_SettlementNumber+' '+a.accountsledger_SettlementType) as SettlementNumber,(select top 1 convert(varchar(12),Settlements_StartDateTime,113) from master_settlements where Settlements_Number=a.accountsledger_SettlementNumber and Settlements_TypeSuffix=a.accountsledger_SettlementType) as PayoutDate,(select branch_code from tbl_master_branch where branch_id=(select cnt_branchid from tbl_master_contact where cnt_internalID=accountsledger_SubAccountID)) as BranchCode,case when a.Accountsledger_AmountDr='0.00000000' then null else a.Accountsledger_AmountDr end as Accountsledger_AmountCr,case when a.Accountsledger_AmountCr='0.00000000' then null else a.Accountsledger_AmountCr end as Accountsledger_AmountDr,'0.0' as Closing,accountsledger_transactiondate from Trans_accountsledger a  WHERE  accountsledger_companyID='" + Session["CompanyID"].ToString() + "' and accountsledger_ExchangeSegmentID in(" + Segment + ") and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,accountsledger_transactiondate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtFrom.Value + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtTo.Value + "')) as datetime)   " + mainAccountSearch + " and AccountsLedger_BranchID in(" + Branch + ")  " + SubAccountSearch + @"  and AccountsLedger_SubAccountID is not null and AccountsLedger_TransactionType<>'OpeningBalance' 
                                and isnull(AccountsLedger_Currency,
                            " + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0] + ") as D ", " TrDate,ValueDate, 0 as accountsledger_TransactionReferenceID,accountsledger_Narration,accountsledger_InstrumentNumber,SettlementNumber,PayoutDate,BranchCode,case when (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0)))<0 then cast((sum(isnull(Accountsledger_AmountCr,0))-sum(isnull(Accountsledger_AmountDr,0))) as varchar(max)) else null end Accountsledger_AmountCr,case when (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0)))>0 then cast((sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0))) as varchar(max)) else null end Accountsledger_AmountDr,Closing", null, " TrDate,ValueDate,accountsledger_InstrumentNumber,SettlementNumber,PayoutDate,BranchCode,Closing,accountsledger_transactiondate,accountsledger_Narration", " accountsledger_transactiondate");
                            }
                            else if (radVoucherWise.Checked == true)
                                dtCashBankBook1 = oDBEngine.GetDataTable("(Select convert(varchar(11),a.accountsledger_transactiondate,113) as TrDate,case when a.accountsledger_valuedate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),a.accountsledger_valuedate,113) end as ValueDate,isnull((select top 1 journalvoucher_Narration from trans_journalvoucher where journalvoucher_voucherNumber=a.accountsledger_TransactionReferenceID and journalvoucher_companyID=a.accountsledger_companyID and journalvoucher_ExchangeSegmentID=a.accountsledger_ExchangeSegmentID and journalvoucher_TransactionDate=a.accountsledger_transactiondate),isnull(accountsledger_Narration,'')) +' - '+isnull(isnull((select MainAccount_Name from Master_MainAccount where MainAccount_AccountCode=AccountsLedger_CashBankName),AccountsLedger_CashBankName),'') as accountsledger_Narration,a.accountsledger_InstrumentNumber,(a.accountsledger_SettlementNumber+' '+a.accountsledger_SettlementType) as SettlementNumber,(select top 1 convert(varchar(12),Settlements_StartDateTime,113) from master_settlements where Settlements_Number=a.accountsledger_SettlementNumber and Settlements_TypeSuffix=a.accountsledger_SettlementType) as PayoutDate,(select branch_code from tbl_master_branch where branch_id=(select cnt_branchid from tbl_master_contact where cnt_internalID=accountsledger_SubAccountID)) as BranchCode,case when a.Accountsledger_AmountDr='0.00000000' then null else a.Accountsledger_AmountDr end as Accountsledger_AmountCr,case when a.Accountsledger_AmountCr='0.00000000' then null else a.Accountsledger_AmountCr end as Accountsledger_AmountDr,'0.0' as Closing,accountsledger_transactiondate from Trans_accountsledger a  WHERE  accountsledger_companyID='" + Session["CompanyID"].ToString() + "' and accountsledger_ExchangeSegmentID in(" + Segment + ") and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,accountsledger_transactiondate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtFrom.Value + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtTo.Value + "')) as datetime)   " + mainAccountSearch + " and AccountsLedger_BranchID in(" + Branch + ")  " + SubAccountSearch + @" and AccountsLedger_SubAccountID is not null 
                                and AccountsLedger_TransactionType<>'OpeningBalance' and 
                                isnull(AccountsLedger_Currency,
                            " + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0] + ") as D ", " TrDate,ValueDate, 0 as accountsledger_TransactionReferenceID,accountsledger_Narration,accountsledger_InstrumentNumber,SettlementNumber,PayoutDate,BranchCode,case when (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0)))<0 then cast((sum(isnull(Accountsledger_AmountCr,0))-sum(isnull(Accountsledger_AmountDr,0))) as varchar(max)) else null end Accountsledger_AmountCr,case when (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0)))>0 then cast((sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0))) as varchar(max)) else null end Accountsledger_AmountDr,Closing", null, " TrDate,ValueDate,accountsledger_InstrumentNumber,SettlementNumber,PayoutDate,BranchCode,Closing,accountsledger_transactiondate,accountsledger_Narration", " accountsledger_transactiondate");
                            else
                                dtCashBankBook1 = oDBEngine.GetDataTable("(Select case when a.AccountsLedger_TransactionType='Journal' and Left(AccountsLedger_TransactionReferenceID,1) in('X','Z') then null else convert(varchar(11),a.accountsledger_transactiondate,113) end as TrDate,case when a.AccountsLedger_TransactionType='Journal' and Left(AccountsLedger_TransactionReferenceID,1) in('X','Z') then null else (case when a.accountsledger_valuedate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),a.accountsledger_valuedate,113) end) end as ValueDate,case when a.AccountsLedger_TransactionType='Journal' and Left(AccountsLedger_TransactionReferenceID,1) in('X','Z') then 'Consolidated Entries For the period ' else accountsledger_Narration end as accountsledger_Narration,a.accountsledger_InstrumentNumber,'0' as SettlementNumber,'1/1/1900' as PayoutDate,(select branch_code from tbl_master_branch where branch_id=(select cnt_branchid from tbl_master_contact where cnt_internalID=accountsledger_SubAccountID)) as BranchCode,case when a.Accountsledger_AmountDr='0.00000000' then null else a.Accountsledger_AmountDr end as Accountsledger_AmountCr,case when a.Accountsledger_AmountCr='0.00000000' then null else a.Accountsledger_AmountCr end as Accountsledger_AmountDr,'0.0' as Closing,'1/1/1900' as accountsledger_transactiondate,(select top 1 equity_effectuntil from master_equity where equity_exchsegmentid=2 and equity_effectuntil >= accountsledger_transactiondate order by equity_effectuntil) as ExpDate,(select case when (select top 1 equity_effectuntil from master_equity where equity_exchsegmentid=2 and equity_effectuntil < accountsledger_transactiondate order by equity_effectuntil desc)<'" + dtFrom.Value + "' then '" + dtFrom.Value + "' else (select top 1 DATEADD(dd, 1, CAST(equity_effectuntil AS datetime)) from master_equity where equity_exchsegmentid=2 and equity_effectuntil < accountsledger_transactiondate order by equity_effectuntil desc) end) as FDate,case when a.AccountsLedger_TransactionType='Journal' and Left(AccountsLedger_TransactionReferenceID,1) in('X','Z') then (select case when (select top 1 equity_effectuntil from master_equity where equity_exchsegmentid=2 and equity_effectuntil >= accountsledger_transactiondate order by equity_effectuntil)>'" + dtTo.Value + "' then '" + dtTo.Value + "' else ( select top 1 equity_effectuntil from master_equity where equity_exchsegmentid=2 and equity_effectuntil >= accountsledger_transactiondate order by equity_effectuntil) end) else accountsledger_transactiondate end as TDate from Trans_accountsledger a  WHERE  accountsledger_companyID='" + Session["CompanyID"].ToString() + "' and accountsledger_ExchangeSegmentID in(" + Segment + ") and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,accountsledger_transactiondate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtFrom.Value + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtTo.Value + "')) as datetime)   " + mainAccountSearch + " and AccountsLedger_BranchID in(" + Branch + ")  " + SubAccountSearch + @" and AccountsLedger_SubAccountID is not null and AccountsLedger_TransactionType<>'OpeningBalance' 
                                and isnull(AccountsLedger_Currency,
                            " + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0] + ") as D ", " case when TrDate is null then convert(varchar(11),TDate,113) else TrDate  end as TrDate,ValueDate, 0 as accountsledger_TransactionReferenceID,case when accountsledger_Narration='Consolidated Entries For the period' then accountsledger_Narration+convert(varchar(11),FDate,113)+' To '+convert(varchar(11),TDate,113) else accountsledger_Narration end as accountsledger_Narration,accountsledger_InstrumentNumber,SettlementNumber,PayoutDate,BranchCode,case when (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0)))<0 then cast((sum(isnull(Accountsledger_AmountCr,0))-sum(isnull(Accountsledger_AmountDr,0))) as varchar(max)) else null end Accountsledger_AmountCr,case when (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0)))>0 then cast((sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0))) as varchar(max)) else null end Accountsledger_AmountDr,Closing", null, " TrDate,ValueDate,accountsledger_InstrumentNumber,SettlementNumber,PayoutDate,BranchCode,Closing,accountsledger_transactiondate,accountsledger_Narration,FDate,TDate", " TDate");
                        }
                        else
                            dtCashBankBook1 = oDBEngine.GetDataTable("Trans_accountsledger a ", "convert(varchar(11),a.accountsledger_transactiondate,113) as TrDate,convert(varchar(11),a.accountsledger_valuedate,113) as ValueDate,a.accountsledger_TransactionReferenceID, isnull(a.accountsledger_Narration,'') +' - '+isnull(isnull((select MainAccount_Name from Master_MainAccount where MainAccount_AccountCode=AccountsLedger_CashBankName),AccountsLedger_CashBankName),'') as accountsledger_Narration,a.accountsledger_InstrumentNumber,(a.accountsledger_SettlementNumber+' '+a.accountsledger_SettlementType) as SettlementNumber,(select top 1 convert(varchar(12),Settlements_StartDateTime,113) from master_settlements where Settlements_Number=a.accountsledger_SettlementNumber and Settlements_TypeSuffix=a.accountsledger_SettlementType) as PayoutDate,(select branch_code from tbl_master_branch where branch_id=(select cnt_branchid from tbl_master_contact where cnt_internalID=accountsledger_SubAccountID)) as BranchCode,case when a.Accountsledger_AmountDr='0.00000000' then null else cast(a.Accountsledger_AmountDr as varchar(max)) end as Accountsledger_AmountCr,case when a.Accountsledger_AmountCr='0.00000000' then null else cast(a.Accountsledger_AmountCr as varchar(max)) end as Accountsledger_AmountDr,'0.0' as Closing,(select EXCHANGENAME  from (select A.EXCH_INTERNALID AS SEGMENTID ,isnull((TME.EXH_ShortName + '-' + A.EXCH_SEGMENTID),exch_membershipType) AS EXCHANGENAME from (SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID=a.AccountsLedger_CompanyID ) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID) as DD where cast(SEGMENTID as varchar)=a.AccountsLedger_ExchangeSegmentID)  AS SegmentName ", " accountsledger_companyID='" + Session["CompanyID"].ToString() + "' and accountsledger_ExchangeSegmentID in(" + Segment + ") and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,accountsledger_transactiondate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtFrom.Value + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtTo.Value + "')) as datetime)  " + mainAccountSearch + " and AccountsLedger_BranchID in(" + Branch + ") " + SubAccountSearch + @"  and AccountsLedger_SubAccountID is not null and AccountsLedger_TransactionType<>'OpeningBalance' 
                        and isnull(AccountsLedger_Currency,
                        " + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0] + " and accountsledger_finyear='" + Session["LastFinYear"].ToString() + "'" + strSqlCb + strSqlJv, " accountsledger_transactiondate,accountsledger_TransactionReferenceID");
                        ////dtCashBankBook1 = oDBEngine.GetDataTable("Trans_accountsledger a ", "convert(varchar(11),a.accountsledger_transactiondate,113) as TrDate,convert(varchar(11),a.accountsledger_valuedate,113) as ValueDate,a.accountsledger_TransactionReferenceID, isnull(a.accountsledger_Narration,'') +' - '+isnull(isnull((select MainAccount_Name from Master_MainAccount where MainAccount_AccountCode=AccountsLedger_CashBankName),AccountsLedger_CashBankName),'') as accountsledger_Narration,a.accountsledger_InstrumentNumber,(a.accountsledger_SettlementNumber+' '+a.accountsledger_SettlementType) as SettlementNumber,(select top 1 convert(varchar(12),Settlements_StartDateTime,113) from master_settlements where Settlements_Number=a.accountsledger_SettlementNumber and Settlements_TypeSuffix=a.accountsledger_SettlementType) as PayoutDate,(select branch_code from tbl_master_branch where branch_id=(select cnt_branchid from tbl_master_contact where cnt_internalID=accountsledger_SubAccountID)) as BranchCode,case when a.Accountsledger_AmountDr='0.00000000' then null else cast(a.Accountsledger_AmountDr as varchar(max)) end as Accountsledger_AmountCr,case when a.Accountsledger_AmountCr='0.00000000' then null else cast(a.Accountsledger_AmountCr as varchar(max)) end as Accountsledger_AmountDr,'0.0' as Closing", " accountsledger_companyID='" + Session["CompanyID"].ToString() + "' and accountsledger_ExchangeSegmentID in(" + Segment + ") and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,accountsledger_transactiondate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtFrom.Value + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtTo.Value + "')) as datetime)  " + mainAccountSearch + " and AccountsLedger_BranchID in(" + Branch + ") " + SubAccountSearch + "  and AccountsLedger_SubAccountID is not null and AccountsLedger_TransactionType<>'OpeningBalance'", " accountsledger_transactiondate,accountsledger_TransactionReferenceID");

                        OpenBalance = oDBEngine.OpeningBalanceJournal1(MainAcIDforOp, valItem, Convert.ToDateTime(dtFrom.Value), Segment, Session["CompanyID"].ToString(), Convert.ToDateTime(dtTo.Value), Convert.ToInt32(Session["ActiveCurrency"].ToString().Split('~')[0]));
                    }
                    if ((dtCashBankBook1.Rows.Count > 0) || (Convert.ToDecimal(OpenBalance.Rows[0][0].ToString()) > 0 || Convert.ToDecimal(OpenBalance.Rows[0][1].ToString()) > 0))
                    {

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
                            if (dtCashBankBook1.Rows[j].Table.Columns.Contains("SegmentName"))
                                newRow[11] = dtCashBankBook1.Rows[j]["SegmentName"];
                            //newRow[11] = dtCashBankBook1.Rows[j]["SegmentName"];
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
                        if (ddlAccountType.SelectedItem.Value == "0")
                        {
                            Type = "Clients - Trading A/c  ";
                        }
                        else if (ddlAccountType.SelectedItem.Value == "1")
                        {
                            Type = "Clients - Margin Deposit A/c  ";
                        }
                        else if (ddlAccountType.SelectedItem.Value == "2")
                        {
                            Type = "Clients - Trading A/c and Clients - Margin Deposit A/c  ";
                        }
                        else if (ddlAccountType.SelectedItem.Value == "3")
                        {
                            Type = txtMainAccount.Text;
                        }
                        CheckingValueParam = Type + ": " + cmbclientsPager.Items[l].Text + " " + "Period :" + " " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "  - " + oconverter.ArrangeDate2(dtTo.Value.ToString());
                        newRow5[0] = Type + ": " + cmbclientsPager.Items[l].Text + " " + "Period :" + " " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "  - " + oconverter.ArrangeDate2(dtTo.Value.ToString());
                        newRow5[1] = "";// "Test";
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
                            if (dtCashBankBook1.Rows[i].Table.Columns.Contains("SegmentName"))
                                newRow[11] = dtCashBankBook1.Rows[i]["SegmentName"];
                            //newRow1[11] = dtCashBankBook1.Rows[i]["SegmentName"];

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
                        if (cmbclientsPager.Items.Count == 1)
                        {
                            dtLedger = dtCashBankBook1.Copy();
                        }
                        else
                        {
                            if (l == 0)
                            {
                                dtLedger = dtCashBankBook1.Copy();
                            }
                            else
                            {
                                if (l == cmbclientsPager.Items.Count - 1)
                                {
                                    dtLedger.Merge(dtCashBankBook1);
                                }
                                else
                                {
                                    dtLedger.Merge(dtCashBankBook1);
                                }
                            }
                        }
                    }
                }//End Client For
                if (cmbclientsPager.Items.Count == 0)
                {
                    receipt = 0;
                    Payment = 0;
                    SubAccountSearch = null;
                    string Sub = null;
                    if (SubAcID != null && SubAcID != "")
                    {
                        SubAccountSearch = " and AccountsLedger_SubAccountID in(" + SubAcID + ") ";
                        Sub = SubAcID.Replace("'", "");
                    }
                    //dtCashBankBook1 = oDBEngine.GetDataTable("Trans_accountsledger a ", "convert(varchar(11),a.accountsledger_transactiondate,113) as TrDate,convert(varchar(11),a.accountsledger_valuedate,113) as ValueDate,a.accountsledger_TransactionReferenceID, a.accountsledger_Narration,a.accountsledger_InstrumentNumber,(a.accountsledger_SettlementNumber+' '+a.accountsledger_SettlementType) as SettlementNumber,case when a.Accountsledger_AmountDr='0.00000000' then null else cast(a.Accountsledger_AmountDr as varchar(max)) end as Accountsledger_AmountCr,case when a.Accountsledger_AmountCr='0.00000000' then null else cast(a.Accountsledger_AmountCr as varchar(max)) end as Accountsledger_AmountDr,'0.0' as Closing", " accountsledger_companyID='" + Session["CompanyID"].ToString() + "' and accountsledger_ExchangeSegmentID in(" + Segment + ") and  cast(DATEADD(dd, 0, DATEDIFF(dd, 0,accountsledger_transactiondate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtFrom.Value + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtTo.Value + "')) as datetime)  " + mainAccountSearch + " and AccountsLedger_BranchID in(" + Branch + ") " + SubAccountSearch + "   and AccountsLedger_TransactionType<>'OpeningBalance'", " accountsledger_transactiondate,accountsledger_TransactionReferenceID");
                    if (radConsolidated.Checked == true)
                    {
                        if (radDateWise.Checked == true)
                        {
                            dtCashBankBook1 = oDBEngine.GetDataTable("(Select convert(varchar(11),a.accountsledger_transactiondate,113) as TrDate,case when a.accountsledger_valuedate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),a.accountsledger_valuedate,113) end as ValueDate,case when a.AccountsLedger_TransactionType='Journal' and Left(AccountsLedger_TransactionReferenceID,1) in('X','Y','Z') and accountsledger_SettlementNumber is not null and accountsledger_SettlementNumber<>''	then 'Consolidated Entries For '+convert(varchar(11),a.accountsledger_transactiondate,113) 	else isnull(a.accountsledger_Narration,'') end +' - '+isnull(isnull((select MainAccount_Name from Master_MainAccount where MainAccount_AccountCode=AccountsLedger_CashBankName),AccountsLedger_CashBankName),'') as accountsledger_Narration,a.accountsledger_InstrumentNumber,(a.accountsledger_SettlementNumber+' '+a.accountsledger_SettlementType) as SettlementNumber,(select top 1 convert(varchar(12),Settlements_StartDateTime,113) from master_settlements where Settlements_Number=a.accountsledger_SettlementNumber and Settlements_TypeSuffix=a.accountsledger_SettlementType) as PayoutDate,(select branch_code from tbl_master_branch where branch_id=(select cnt_branchid from tbl_master_contact where cnt_internalID=accountsledger_SubAccountID)) as BranchCode,case when a.Accountsledger_AmountDr='0.00000000' then null else a.Accountsledger_AmountDr end as Accountsledger_AmountCr,case when a.Accountsledger_AmountCr='0.00000000' then null else a.Accountsledger_AmountCr end as Accountsledger_AmountDr,'0.0' as Closing,accountsledger_transactiondate from Trans_accountsledger a  WHERE  accountsledger_companyID='" + Session["CompanyID"].ToString() + "' and accountsledger_ExchangeSegmentID in(" + Segment + ") and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,accountsledger_transactiondate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtFrom.Value + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtTo.Value + "')) as datetime)   " + mainAccountSearch + " and AccountsLedger_BranchID in(" + Branch + ")  " + SubAccountSearch + @"  and AccountsLedger_TransactionType<>'OpeningBalance' 
                        and isnull(AccountsLedger_Currency,
                        " + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0] + ") as D ", " TrDate,ValueDate, 0 as accountsledger_TransactionReferenceID,accountsledger_Narration,accountsledger_InstrumentNumber,SettlementNumber,PayoutDate,BranchCode,case when (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0)))<0 then cast((sum(isnull(Accountsledger_AmountCr,0))-sum(isnull(Accountsledger_AmountDr,0))) as varchar(max)) else null end Accountsledger_AmountCr,case when (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0)))>0 then cast((sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0))) as varchar(max)) else null end Accountsledger_AmountDr,Closing", null, " TrDate,ValueDate,accountsledger_InstrumentNumber,SettlementNumber,PayoutDate,BranchCode,Closing,accountsledger_transactiondate,accountsledger_Narration", " accountsledger_transactiondate");
                        }
                        else if (radVoucherWise.Checked == true)
                            dtCashBankBook1 = oDBEngine.GetDataTable("(Select convert(varchar(11),a.accountsledger_transactiondate,113) as TrDate,case when a.accountsledger_valuedate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),a.accountsledger_valuedate,113) end as ValueDate,isnull((select top 1 journalvoucher_Narration from trans_journalvoucher where journalvoucher_voucherNumber=a.accountsledger_TransactionReferenceID and journalvoucher_companyID=a.accountsledger_companyID and journalvoucher_ExchangeSegmentID=a.accountsledger_ExchangeSegmentID and journalvoucher_TransactionDate=a.accountsledger_transactiondate),isnull(accountsledger_Narration,'')) +' - '+isnull(isnull((select MainAccount_Name from Master_MainAccount where MainAccount_AccountCode=AccountsLedger_CashBankName),AccountsLedger_CashBankName),'') as accountsledger_Narration,a.accountsledger_InstrumentNumber,(a.accountsledger_SettlementNumber+' '+a.accountsledger_SettlementType) as SettlementNumber,(select top 1 convert(varchar(12),Settlements_StartDateTime,113) from master_settlements where Settlements_Number=a.accountsledger_SettlementNumber and Settlements_TypeSuffix=a.accountsledger_SettlementType) as PayoutDate,(select branch_code from tbl_master_branch where branch_id=(select cnt_branchid from tbl_master_contact where cnt_internalID=accountsledger_SubAccountID)) as BranchCode,case when a.Accountsledger_AmountDr='0.00000000' then null else a.Accountsledger_AmountDr end as Accountsledger_AmountCr,case when a.Accountsledger_AmountCr='0.00000000' then null else a.Accountsledger_AmountCr end as Accountsledger_AmountDr,'0.0' as Closing,accountsledger_transactiondate from Trans_accountsledger a  WHERE  accountsledger_companyID='" + Session["CompanyID"].ToString() + "' and accountsledger_ExchangeSegmentID in(" + Segment + ") and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,accountsledger_transactiondate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtFrom.Value + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtTo.Value + "')) as datetime)   " + mainAccountSearch + " and AccountsLedger_BranchID in(" + Branch + ")  " + SubAccountSearch + @"  and AccountsLedger_TransactionType<>'OpeningBalance' 
                        and isnull(AccountsLedger_Currency,
                        " + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0] + ") as D ", " TrDate,ValueDate, 0 as accountsledger_TransactionReferenceID,accountsledger_Narration,accountsledger_InstrumentNumber,SettlementNumber,PayoutDate,BranchCode,case when (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0)))<0 then cast((sum(isnull(Accountsledger_AmountCr,0))-sum(isnull(Accountsledger_AmountDr,0))) as varchar(max)) else null end Accountsledger_AmountCr,case when (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0)))>0 then cast((sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0))) as varchar(max)) else null end Accountsledger_AmountDr,Closing", null, " TrDate,ValueDate,accountsledger_InstrumentNumber,SettlementNumber,PayoutDate,BranchCode,Closing,accountsledger_transactiondate,accountsledger_Narration", " accountsledger_transactiondate");
                        else
                            dtCashBankBook1 = oDBEngine.GetDataTable("(Select case when a.AccountsLedger_TransactionType='Journal' and Left(AccountsLedger_TransactionReferenceID,1) in('X','Z') then null else convert(varchar(11),a.accountsledger_transactiondate,113) end as TrDate,case when a.AccountsLedger_TransactionType='Journal' and Left(AccountsLedger_TransactionReferenceID,1) in('X','Z') then null else (case when a.accountsledger_valuedate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),a.accountsledger_valuedate,113) end) end as ValueDate,case when a.AccountsLedger_TransactionType='Journal' and Left(AccountsLedger_TransactionReferenceID,1) in('X','Z') then 'Consolidated Entries For the period ' else accountsledger_Narration end as accountsledger_Narration,a.accountsledger_InstrumentNumber,'0' as SettlementNumber,'1/1/1900' as PayoutDate,(select branch_code from tbl_master_branch where branch_id=(select cnt_branchid from tbl_master_contact where cnt_internalID=accountsledger_SubAccountID)) as BranchCode,case when a.Accountsledger_AmountDr='0.00000000' then null else a.Accountsledger_AmountDr end as Accountsledger_AmountCr,case when a.Accountsledger_AmountCr='0.00000000' then null else a.Accountsledger_AmountCr end as Accountsledger_AmountDr,'0.0' as Closing,'1/1/1900' as accountsledger_transactiondate,,(select top 1 equity_effectuntil from master_equity where equity_exchsegmentid=2 and equity_effectuntil >= accountsledger_transactiondate order by equity_effectuntil) as ExpDate,(select case when (select top 1 equity_effectuntil from master_equity where equity_exchsegmentid=2 and equity_effectuntil < accountsledger_transactiondate order by equity_effectuntil desc)<'" + dtFrom.Value + "' then '" + dtFrom.Value + "' else (select top 1 DATEADD(dd, 1, CAST(equity_effectuntil AS datetime)) from master_equity where equity_exchsegmentid=2 and equity_effectuntil < accountsledger_transactiondate order by equity_effectuntil desc) end) as FDate,case when a.AccountsLedger_TransactionType='Journal' and Left(AccountsLedger_TransactionReferenceID,1) in('X','Z') then (select case when (select top 1 equity_effectuntil from master_equity where equity_exchsegmentid=2 and equity_effectuntil >= accountsledger_transactiondate order by equity_effectuntil)>'" + dtTo.Value + "' then '" + dtTo.Value + "' else ( select top 1 equity_effectuntil from master_equity where equity_exchsegmentid=2 and equity_effectuntil >= accountsledger_transactiondate order by equity_effectuntil) end) else accountsledger_transactiondate end as TDate from Trans_accountsledger a  WHERE  accountsledger_companyID='" + Session["CompanyID"].ToString() + "' and accountsledger_ExchangeSegmentID in(" + Segment + ") and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,accountsledger_transactiondate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtFrom.Value + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtTo.Value + "')) as datetime)   " + mainAccountSearch + " and AccountsLedger_BranchID in(" + Branch + ")  " + SubAccountSearch + @"  and AccountsLedger_TransactionType<>'OpeningBalance' 
                        and isnull(AccountsLedger_Currency,
                        " + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0] + ") as D ", " case when TrDate is null then convert(varchar(11),TDate,113) else TrDate end as TrDate,ValueDate, 0 as accountsledger_TransactionReferenceID,case when accountsledger_Narration='Consolidated Entries For the period' then accountsledger_Narration+convert(varchar(11),FDate,113)+' To '+convert(varchar(11),TDate,113) else accountsledger_Narration end as accountsledger_Narration,accountsledger_InstrumentNumber,SettlementNumber,PayoutDate,BranchCode,case when (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0)))<0 then cast((sum(isnull(Accountsledger_AmountCr,0))-sum(isnull(Accountsledger_AmountDr,0))) as varchar(max)) else null end Accountsledger_AmountCr,case when (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0)))>0 then cast((sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0))) as varchar(max)) else null end Accountsledger_AmountDr,Closing", null, " TrDate,ValueDate,accountsledger_InstrumentNumber,SettlementNumber,PayoutDate,BranchCode,Closing,accountsledger_transactiondate,accountsledger_Narration,FDate,TDate", " TDate");
                    }
                    else
                        dtCashBankBook1 = oDBEngine.GetDataTable("Trans_accountsledger a ", "convert(varchar(11),a.accountsledger_transactiondate,113) as TrDate,case when a.accountsledger_valuedate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),a.accountsledger_valuedate,113) end as ValueDate,a.accountsledger_TransactionReferenceID, isnull(a.accountsledger_Narration,'') +' - '+isnull(isnull((select MainAccount_Name from Master_MainAccount where MainAccount_AccountCode=AccountsLedger_CashBankName),AccountsLedger_CashBankName),'') as accountsledger_Narration,a.accountsledger_InstrumentNumber,(a.accountsledger_SettlementNumber+' '+a.accountsledger_SettlementType) as SettlementNumber,(select top 1 convert(varchar(12),Settlements_StartDateTime,113) from master_settlements where Settlements_Number=a.accountsledger_SettlementNumber and Settlements_TypeSuffix=a.accountsledger_SettlementType) as PayoutDate,(select branch_code from tbl_master_branch where branch_id=(select cnt_branchid from tbl_master_contact where cnt_internalID=accountsledger_SubAccountID)) as BranchCode,case when a.Accountsledger_AmountDr='0.00000000' then null else cast(a.Accountsledger_AmountDr as varchar(max)) end as Accountsledger_AmountCr,case when a.Accountsledger_AmountCr='0.00000000' then null else cast(a.Accountsledger_AmountCr as varchar(max)) end as Accountsledger_AmountDr,'0.0' as Closing", " accountsledger_companyID='" + Session["CompanyID"].ToString() + "' and accountsledger_ExchangeSegmentID in(" + Segment + ") and  cast(DATEADD(dd, 0, DATEDIFF(dd, 0,accountsledger_transactiondate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtFrom.Value + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtTo.Value + "')) as datetime)  " + mainAccountSearch + " and AccountsLedger_BranchID in(" + Branch + ") " + SubAccountSearch + @"   and AccountsLedger_TransactionType<>'OpeningBalance'
                    and isnull(AccountsLedger_Currency,
                    " + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0], " accountsledger_transactiondate,accountsledger_TransactionReferenceID");

                    OpenBalance = oDBEngine.OpeningBalanceJournal1(MainAcIDforOp, Sub, Convert.ToDateTime(dtFrom.Value), Segment, Session["CompanyID"].ToString(), Convert.ToDateTime(dtTo.Value), Convert.ToInt32(Session["ActiveCurrency"].ToString().Split('~')[0]));
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
                        if (dtCashBankBook1.Rows[j].Table.Columns.Contains("SegmentName"))
                            newRow[11] = dtCashBankBook1.Rows[j]["SegmentName"];
                        //newRow[11] = dtCashBankBook1.Rows[j]["SegmentName"];
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
                    if (ddlAccountType.SelectedItem.Value == "0")
                    {
                        Type = "Clients - Trading A/c  ";
                    }
                    else if (ddlAccountType.SelectedItem.Value == "1")
                    {
                        Type = "Clients - Margin Deposit A/c  ";
                    }
                    else if (ddlAccountType.SelectedItem.Value == "2")
                    {
                        Type = "Clients - Trading A/c and Clients - Margin Deposit A/c  ";
                    }
                    else if (ddlAccountType.SelectedItem.Value == "3")
                    {
                        Type = txtMainAccount.Text;
                    }
                    CheckingValueParam = Type + ": Period :" + " " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "  - " + oconverter.ArrangeDate2(dtTo.Value.ToString());
                    newRow5[0] = Type + ": Period :" + " " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "  - " + oconverter.ArrangeDate2(dtTo.Value.ToString());
                    newRow5[1] = "";//"Test";
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
                        if (dtCashBankBook1.Rows[i].Table.Columns.Contains("SegmentName"))
                            newRow[11] = dtCashBankBook1.Rows[i]["SegmentName"];
                        //newRow[11] = dtCashBankBook1.Rows[i]["SegmentName"];
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
                    dtLedger = dtCashBankBook1.Copy();
                }
                //DataTable dtReportHeader = new DataTable();
                //dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
                //DataRow HeaderRow = dtReportHeader.NewRow();
                //HeaderRow[0] = CompanyName.Rows[0][0].ToString();
                //dtReportHeader.Rows.Add(HeaderRow);
                //DataRow DrRowR1 = dtReportHeader.NewRow();
                //DrRowR1[0] = "Ledger For the  Period [" + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + " To " + oconverter.ArrangeDate2(dtTo.Value.ToString()) + "]";
                //dtReportHeader.Rows.Add(DrRowR1);
                //DataRow HeaderRow1 = dtReportHeader.NewRow();
                //dtReportHeader.Rows.Add(HeaderRow1);
                //DataRow HeaderRow2 = dtReportHeader.NewRow();
                //dtReportHeader.Rows.Add(HeaderRow2);

                //DataTable dtReportFooter = new DataTable();
                //dtReportFooter.Columns.Add(new DataColumn("Footer", typeof(String))); //0
                //DataRow FooterRow1 = dtReportFooter.NewRow();
                //dtReportFooter.Rows.Add(FooterRow1);
                //DataRow FooterRow2 = dtReportFooter.NewRow();
                //dtReportFooter.Rows.Add(FooterRow2);
                //DataRow FooterRow = dtReportFooter.NewRow();
                ////FooterRow[0] = "* * *  End Of Report * * *         [" + oconverter.ArrangeDate2(oDBEngine.GetDate().ToString(), "Test") + "]";
                //FooterRow[0] = "* * *  End Of Report * * *   ";
                //dtReportFooter.Rows.Add(FooterRow);

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
                if (radConsolidated.Checked == true)
                    dtExport.Columns.Remove("Voucher No.");
                //if (Session["userlastsegment"].ToString() != "7" || Session["userlastsegment"].ToString() == "18")
                //{
                //    dtExport.Columns.Remove("Trade Date");
                //    dtExport.Columns.Remove("Settlement No.");
                //}
                if (Session["userlastsegment"].ToString() != "7" && Session["userlastsegment"].ToString() != "8" || Session["userlastsegment"].ToString() == "18")
                    dtExport.Columns.Remove("Branch Code");
                dtExport.AcceptChanges();



                //For the Purpose Of Showing Or Not Showing Value Date
                DataTable DtOriginal = new DataTable();
                DataTable DtModified = new DataTable();
                DtOriginal = dtExport;
                DtModified = DtOriginal.Clone();

                foreach (DataRow Row in DtOriginal.Rows)
                    DtModified.ImportRow(Row);
                if (!ChkShowValueDate.Checked)
                    DtModified.Columns.RemoveAt(1);

                //====Check CM Selection To show Settlement No and Trade Date======
                string isCMSelected = "";
                //DataTable DtCheckForCM = oDBEngine.GetDataTable("Select * From tbl_master_companyExchange where exch_internalId in (" + Segment + ") And exch_segmentId='CM'");
                DataTable DtCheckForCM = oDBEngine.GetDataTable("Select * From tbl_master_companyExchange where exch_internalId in (" + Segment + ") And exch_segmentId<>'CM'");
                if (DtCheckForCM != null)
                {
                    if (DtCheckForCM.Rows.Count > 0)
                        isCMSelected = "false";
                    else
                        isCMSelected = "true";
                }
                DtCheckForCM.Dispose();

                if (isCMSelected != "true")
                {
                    DtModified.Columns.Remove("Settlement No.");
                    DtModified.Columns.Remove("Trade Date");

                }
                DtModified.AcceptChanges();

                //objExcel.ExportToExcelforExcel(DtModified, "Ledger", "Closing Balance", dtReportHeader, dtReportFooter);
                //========New Excel Start====

                GenericExcelExport oGenericExcelExport = new GenericExcelExport();
                string strDownloadFileName = "";
                string grpBy = null;
                string reportType = null;

                string strReportHeader = null;
                if ((ddlGroup.SelectedItem.Text) != null) grpBy = ddlGroup.SelectedItem.Text;
                if ((radDetail.Checked) == true) reportType = "Detail";
                else if ((radConsolidated.Checked) == true) reportType = "Consolidated";
                else reportType = "Obligation Breakup";
                strReportHeader = "Showing " + reportType + " Ledger Report Search By Date  Between  " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "  to " + oconverter.ArrangeDate2(dtTo.Value.ToString());
                if (grpBy != null) strReportHeader += " and Filter By " + grpBy;

                string exlDateTime = oDBEngine.GetDate(113).ToString();
                string exlTime = exlDateTime.Replace(":", "");
                exlTime = exlTime.Replace(" ", "");
                string FileName = "LedgerView_" + exlTime;
                strDownloadFileName = "~/Documents/";

                string[] strHead = new string[3];
                strHead[0] = exlDateTime;
                strHead[1] = strReportHeader;
                strHead[2] = CompanyName.Rows[0][0].ToString(); ;

                if (ChkShowValueDate.Checked != true)
                {
                    if (isCMSelected != "true")
                    {
                        string[] ColumnType = { "V", "V", "V", "V", "V", "V", "V", "V", "V" };
                        string[] ColumnSize = { "150", "30", "150", "30", "20", "20", "20", "30", "20" };
                        string[] ColumnWidthSize = { "25", "16", "40", "20", "10", "15", "10", "20", "15" };
                        oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, DtModified, Server.MapPath(strDownloadFileName), "2007", FileName, strHead, null);
                    }
                    else
                    {
                        string[] ColumnType = { "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V" };
                        string[] ColumnSize = { "150", "30", "150", "30", "20", "20", "30", "20", "20", "30", "20" };
                        string[] ColumnWidthSize = { "25", "16", "40", "20", "10", "10", "10", "15", "10", "20", "15" };
                        oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, DtModified, Server.MapPath(strDownloadFileName), "2007", FileName, strHead, null);
                    }

                    //string[] ColumnType =      { "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V" };
                    //string[] ColumnSize =      { "150", "30", "150", "30", "20", "20", "30", "20", "20", "30", "20" };
                    //string[] ColumnWidthSize = { "25", "16", "40", "20", "10", "10", "10", "15", "10", "20", "15" };
                    //oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, DtModified, Server.MapPath(strDownloadFileName), "2007", FileName, strHead, null);
                }
                else   //Without Show value date Checked
                {
                    if (DtModified.Columns["SegmentName"] != null)
                        DtModified.Columns.Remove("SegmentName");
                    DtModified.AcceptChanges();
                    if (isCMSelected != "true")
                    {
                        string[] ColumnType = { "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V" };
                        string[] ColumnSize = { "150", "15", "30", "150", "30", "20", "20", "30", "20", "20", "30", "20" };
                        string[] ColumnWidthSize = { "25", "12", "16", "40", "20", "10", "10", "10", "15", "10", "20", "15" };
                        oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, DtModified, Server.MapPath(strDownloadFileName), "2007", FileName, strHead, null);
                    }
                    else
                    {
                        string[] ColumnType = { "V", "V", "V", "V", "V", "V", "V", "V", "V", "V" };
                        string[] ColumnSize = { "150", "15", "30", "150", "30", "30", "20", "20", "30", "20" };
                        string[] ColumnWidthSize = { "25", "12", "16", "40", "20", "10", "15", "10", "20", "15" };
                        oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, DtModified, Server.MapPath(strDownloadFileName), "2007", FileName, strHead, null);
                    }
                    //string[] ColumnType =      { "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V" };
                    //string[] ColumnSize =      { "150", "15", "30", "150", "30", "20", "20", "30", "20", "20", "30", "20" };
                    //string[] ColumnWidthSize = { "25", "12", "16", "40", "20", "10", "10", "10", "15", "10", "20", "15" };
                    //oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, DtModified, Server.MapPath(strDownloadFileName), "2007", FileName, strHead, null);
                }
                //========New Excel End====
            }
        }
        protected void FetchDataToExport()
        {
            DataTable OpenBalance = new DataTable();
            dtCashBankBook = new DataTable();
            dtLedgerView = new DataTable();
            decimal receipt = 0;
            decimal Payment = 0;
            DateTime TranDate = DateTime.Today;
            System.Globalization.NumberFormatInfo currencyFormat = new System.Globalization.CultureInfo("hi-IN").NumberFormat;
            currencyFormat.CurrencySymbol = "";
            currencyFormat.CurrencyNegativePattern = 2;
            pageSize = 25;


            if (Request.QueryString["MainID"] != null)
            {
                if (BtnclickShow == "Click")
                {
                    dtFrom.Value = dtFromG.Value;
                    dtTo.Value = dtToG.Value;
                    SpanShowHeader.InnerText = "   Period  From    " + oconverter.ArrangeDate2(dtFromG.Value.ToString()) + "      To    " + oconverter.ArrangeDate2(dtToG.Value.ToString());
                }
                else
                {
                    DateTime dtTranDate = new DateTime();
                    DateTime date = Convert.ToDateTime(Request.QueryString["date"].ToString());
                    DateTime ToDay = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());

                    DataTable dtFinYear = oDBEngine.GetDataTable("MASTER_FINYEAR ", " FINYEAR_STARTDATE ", " FINYEAR_CODE ='" + Session["LastFinYear"].ToString() + "'");
                    DateTime StartDate = Convert.ToDateTime(dtFinYear.Rows[0][0].ToString());
                    if (date < ToDay)
                        dtTranDate = Convert.ToDateTime(date.AddDays(-45).ToShortDateString());
                    else
                        dtTranDate = Convert.ToDateTime(oDBEngine.GetDate().AddDays(-45).ToShortDateString());


                    if (dtTranDate < StartDate)
                    {
                        dtTranDate = StartDate;
                        dtFrom.Value = StartDate;
                        dtTo.Value = date;
                        dtFromG.Value = StartDate;
                        dtToG.Value = date;
                    }
                    else
                    {
                        dtFrom.Value = dtTranDate;
                        dtTo.Value = date;
                        dtFromG.Value = dtTranDate;
                        dtToG.Value = date;
                    }
                    SpanShowHeader.InnerText = "   Period  From    " + oconverter.ArrangeDate2(dtTranDate.ToString()) + "      To    " + oconverter.ArrangeDate2(date.ToString());
                }

                if (Request.QueryString["SubID"].ToString() == "GeneralTrial ")
                {
                    // dtCashBankBook = oDBEngine.GetDataTable("(Select convert(varchar(11),a.accountsledger_transactiondate,113) as TrDate,case when a.accountsledger_valuedate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),a.accountsledger_valuedate,113) end as ValueDate,AccountsLedger_TransactionReferenceID,case when a.AccountsLedger_TransactionType='Journal' and Left(AccountsLedger_TransactionReferenceID,1) in('X','Y','Z') and Left(AccountsLedger_TransactionReferenceID,2) not in('YD') and accountsledger_SettlementNumber is not null and accountsledger_SettlementNumber<>''	then 'Consolidated Entries For '+convert(varchar(11),a.accountsledger_transactiondate,113) 	when a.AccountsLedger_TransactionType='Journal'  and Left(AccountsLedger_TransactionReferenceID,2) in('YD') and accountsledger_SettlementNumber is not null and accountsledger_SettlementNumber<>''	then 'Short Back Add Delivery '+convert(varchar(11),a.accountsledger_transactiondate,113) else isnull(a.accountsledger_Narration,'') end +' - '+isnull(isnull((select MainAccount_Name from Master_MainAccount where MainAccount_AccountCode=AccountsLedger_CashBankName),AccountsLedger_CashBankName),'') as accountsledger_Narration,'1' as AccountName,case when a.Accountsledger_AmountCr='0.00000000' then null else a.Accountsledger_AmountCr end as Accountsledger_AmountDr,case when a.Accountsledger_AmountDr='0.00000000' then null else a.Accountsledger_AmountDr end as Accountsledger_AmountCr,'0.0' as Closing,a.accountsledger_transactiondate as accountsledger_transactiondate,a.accountsledger_InstrumentNumber,(a.accountsledger_SettlementNumber+' '+a.accountsledger_SettlementType) as SettlementNumber,a.accountsledger_MainAccountID as MainID,a.accountsledger_SubAccountID as SubID,a.accountsledger_companyID as CompanyID,a.accountsledger_exchangeSegmentID as SegID,a.AccountsLedger_TransactionType  as CashType,(select top 1 convert(varchar(12),Settlements_StartDateTime,113) from master_settlements where Settlements_Number=a.accountsledger_SettlementNumber and Settlements_TypeSuffix=a.accountsledger_SettlementType) as PayoutDate,(select branch_code from tbl_master_branch where branch_id=(select cnt_branchid from tbl_master_contact where cnt_internalID=accountsledger_SubAccountID)) as BranchCode from Trans_accountsledger a  WHERE  accountsledger_companyID='" + Session["LastCompany"].ToString() + "' and accountsledger_ExchangeSegmentID in(" + Request.QueryString["SegmentID"].ToString() + ") and  accountsledger_transactiondate>='" + dtFrom.Value + "' and accountsledger_MainAccountID in('" + Request.QueryString["MainID"].ToString() + "')   and AccountsLedger_SubAccountID is not null and AccountsLedger_TransactionType<>'OpeningBalance' ) as D", "TrDate,ValueDate,AccountsLedger_TransactionReferenceID as accountsledger_TransactionReferenceID,accountsledger_Narration,AccountName,case when (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0)))>0 then (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0))) else null end Accountsledger_AmountDr,case when (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0)))<0 then (sum(isnull(Accountsledger_AmountCr,0))-sum(isnull(Accountsledger_AmountDr,0))) else null end Accountsledger_AmountCr,Closing,accountsledger_transactiondate,accountsledger_InstrumentNumber,SettlementNumber,MainID,SubID,CompanyID,SegID,CashType,PayoutDate,BranchCode,396 as UserID ", null, " TrDate,ValueDate,AccountsLedger_TransactionReferenceID,AccountName,Closing,accountsledger_transactiondate,accountsledger_InstrumentNumber,SettlementNumber,MainID,SubID,CompanyID,SegID,CashType,accountsledger_Narration,PayoutDate,BranchCode", " accountsledger_transactiondate");


                    //dtCashBankBook = oDBEngine.GetDataTable("(Select convert(varchar(11),a.accountsledger_transactiondate,113) as TrDate,case when a.accountsledger_valuedate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),a.accountsledger_valuedate,113) end as ValueDate,case when AccountsLedger_TransactionType='Cash_Bank' then AccountsLedger_TransactionReferenceID else  null end   as  AccountsLedger_TransactionReferenceID,case when a.AccountsLedger_TransactionType='Journal' and Left(AccountsLedger_TransactionReferenceID,1) in('X','Y','Z') and Left(AccountsLedger_TransactionReferenceID,2) not in('YD') and accountsledger_SettlementNumber is not null and accountsledger_SettlementNumber<>''	then 'Consolidated Entries For '+convert(varchar(11),a.accountsledger_transactiondate,113) 	when a.AccountsLedger_TransactionType='Journal'  and Left(AccountsLedger_TransactionReferenceID,2) in('YD') and accountsledger_SettlementNumber is not null and accountsledger_SettlementNumber<>''	then 'Short Back Add Delivery '+convert(varchar(11),a.accountsledger_transactiondate,113) else isnull(a.accountsledger_Narration,'') end +' - '++' - '+isnull(isnull((select MainAccount_Name from Master_MainAccount where MainAccount_AccountCode=AccountsLedger_CashBankName),AccountsLedger_CashBankName),'') as accountsledger_Narration,'1' as AccountName,case when a.Accountsledger_AmountCr='0.00000000' then null else a.Accountsledger_AmountCr end as Accountsledger_AmountDr,case when a.Accountsledger_AmountDr='0.00000000' then null else a.Accountsledger_AmountDr end as Accountsledger_AmountCr,'0.0' as Closing,a.accountsledger_transactiondate as accountsledger_transactiondate,a.accountsledger_InstrumentNumber,(a.accountsledger_SettlementNumber+' '+a.accountsledger_SettlementType) as SettlementNumber,a.accountsledger_MainAccountID as MainID,a.accountsledger_SubAccountID as SubID,a.accountsledger_companyID as CompanyID,a.accountsledger_exchangeSegmentID as SegID,a.AccountsLedger_TransactionType  as CashType,(select top 1 convert(varchar(12),Settlements_StartDateTime,113) from master_settlements where Settlements_Number=a.accountsledger_SettlementNumber and Settlements_TypeSuffix=a.accountsledger_SettlementType) as PayoutDate,(select branch_code from tbl_master_branch where branch_id=(select cnt_branchid from tbl_master_contact where cnt_internalID=accountsledger_SubAccountID)) as BranchCode from Trans_accountsledger a  WHERE  accountsledger_companyID='" + Session["LastCompany"].ToString() + "' and accountsledger_ExchangeSegmentID in(" + Request.QueryString["SegmentID"].ToString() + ") and  accountsledger_transactiondate between  '" + dtFrom.Value + "'  and  '" + dtTo.Value + "' and accountsledger_MainAccountID in('" + Request.QueryString["MainID"].ToString() + "') and accountsledger_finyear='" + Session["LastFinYear"].ToString() + "'   and AccountsLedger_TransactionType<>'OpeningBalance' ) as D", "TrDate,ValueDate,AccountsLedger_TransactionReferenceID as accountsledger_TransactionReferenceID,accountsledger_Narration,AccountName,case when (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0)))>0 then (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0))) else null end Accountsledger_AmountDr,case when (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0)))<0 then (sum(isnull(Accountsledger_AmountCr,0))-sum(isnull(Accountsledger_AmountDr,0))) else null end Accountsledger_AmountCr,Closing,accountsledger_transactiondate,accountsledger_InstrumentNumber,SettlementNumber,MainID,SubID,CompanyID,SegID,CashType,PayoutDate,BranchCode,396 as UserID ", null, " TrDate,ValueDate,AccountsLedger_TransactionReferenceID,AccountName,Closing,accountsledger_transactiondate,accountsledger_InstrumentNumber,SettlementNumber,MainID,SubID,CompanyID,SegID,CashType,accountsledger_Narration,PayoutDate,BranchCode", " accountsledger_transactiondate");
                    dtCashBankBook = oDBEngine.GetDataTable("(Select convert(varchar(11),a.accountsledger_transactiondate,113) as TrDate,case when a.accountsledger_valuedate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),a.accountsledger_valuedate,113) end as ValueDate,  AccountsLedger_TransactionReferenceID,case when a.AccountsLedger_TransactionType='Journal' and Left(AccountsLedger_TransactionReferenceID,1) in('X','Y','Z') and Left(AccountsLedger_TransactionReferenceID,2) not in('YD') and accountsledger_SettlementNumber is not null and accountsledger_SettlementNumber<>''	then 'Consolidated Entries For '+convert(varchar(11),a.accountsledger_transactiondate,113) 	when a.AccountsLedger_TransactionType='Journal'  and Left(AccountsLedger_TransactionReferenceID,2) in('YD') and accountsledger_SettlementNumber is not null and accountsledger_SettlementNumber<>''	then 'Short Back Add Delivery '+convert(varchar(11),a.accountsledger_transactiondate,113) else isnull(a.accountsledger_Narration,'') end +' - '++' - '+isnull(isnull((select MainAccount_Name from Master_MainAccount where MainAccount_AccountCode=AccountsLedger_CashBankName),AccountsLedger_CashBankName),'') as accountsledger_Narration,'1' as AccountName,case when a.Accountsledger_AmountCr='0.00000000' then null else a.Accountsledger_AmountCr end as Accountsledger_AmountDr,case when a.Accountsledger_AmountDr='0.00000000' then null else a.Accountsledger_AmountDr end as Accountsledger_AmountCr,'0.0' as Closing,a.accountsledger_transactiondate as accountsledger_transactiondate,a.accountsledger_InstrumentNumber,(a.accountsledger_SettlementNumber+' '+a.accountsledger_SettlementType) as SettlementNumber,a.accountsledger_MainAccountID as MainID,a.accountsledger_SubAccountID as SubID,a.accountsledger_companyID as CompanyID,a.accountsledger_exchangeSegmentID as SegID,a.AccountsLedger_TransactionType  as CashType,(select top 1 convert(varchar(12),Settlements_StartDateTime,113) from master_settlements where Settlements_Number=a.accountsledger_SettlementNumber and Settlements_TypeSuffix=a.accountsledger_SettlementType) as PayoutDate,(select branch_code from tbl_master_branch where branch_id=(select cnt_branchid from tbl_master_contact where cnt_internalID=accountsledger_SubAccountID)) as BranchCode from Trans_accountsledger a  WHERE  accountsledger_companyID='" + Session["LastCompany"].ToString() + "' and accountsledger_ExchangeSegmentID in(" + Request.QueryString["SegmentID"].ToString() + ") and  accountsledger_transactiondate between  '" + dtFrom.Value + "'  and  '" + dtTo.Value + "' and accountsledger_MainAccountID in('" + Request.QueryString["MainID"].ToString() + "') and accountsledger_finyear='" + Session["LastFinYear"].ToString() + @"'   and AccountsLedger_TransactionType<>'OpeningBalance' 
                and isnull(AccountsLedger_Currency,
                " + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0] + ") as D", "TrDate,ValueDate,AccountsLedger_TransactionReferenceID as accountsledger_TransactionReferenceID,accountsledger_Narration,AccountName,case when (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0)))>0 then (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0))) else null end Accountsledger_AmountDr,case when (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0)))<0 then (sum(isnull(Accountsledger_AmountCr,0))-sum(isnull(Accountsledger_AmountDr,0))) else null end Accountsledger_AmountCr,Closing,accountsledger_transactiondate,accountsledger_InstrumentNumber,SettlementNumber,MainID,SubID,CompanyID,SegID,CashType,PayoutDate,BranchCode,396 as UserID ", null, " TrDate,ValueDate,AccountsLedger_TransactionReferenceID,AccountName,Closing,accountsledger_transactiondate,accountsledger_InstrumentNumber,SettlementNumber,MainID,SubID,CompanyID,SegID,CashType,accountsledger_Narration,PayoutDate,BranchCode", " accountsledger_transactiondate");
                    OpenBalance = oDBEngine.OpeningBalanceJournal1("'" + Request.QueryString["MainID"].ToString() + "'", "", Convert.ToDateTime(dtFrom.Value), Request.QueryString["SegmentID"].ToString(), Session["LastCompany"].ToString(), Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString()), Convert.ToInt32(Session["ActiveCurrency"].ToString().Split('~')[0]));

                }
                else
                {

                    //  dtCashBankBook = oDBEngine.GetDataTable("(Select convert(varchar(11),a.accountsledger_transactiondate,113) as TrDate,case when a.accountsledger_valuedate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),a.accountsledger_valuedate,113) end as ValueDate,case when AccountsLedger_TransactionType='Cash_Bank' then AccountsLedger_TransactionReferenceID else  null end   as  AccountsLedger_TransactionReferenceID,case when a.AccountsLedger_TransactionType='Journal' and Left(AccountsLedger_TransactionReferenceID,1) in('X','Y','Z') and Left(AccountsLedger_TransactionReferenceID,2) not in('YD') and accountsledger_SettlementNumber is not null and accountsledger_SettlementNumber<>''	then 'Consolidated Entries For '+convert(varchar(11),a.accountsledger_transactiondate,113) 	when a.AccountsLedger_TransactionType='Journal'  and Left(AccountsLedger_TransactionReferenceID,2) in('YD') and accountsledger_SettlementNumber is not null and accountsledger_SettlementNumber<>''	then 'Short Back Add Delivery '+convert(varchar(11),a.accountsledger_transactiondate,113) else isnull(a.accountsledger_Narration,'') end +' - '++' - '+isnull(isnull((select MainAccount_Name from Master_MainAccount where MainAccount_AccountCode=AccountsLedger_CashBankName),AccountsLedger_CashBankName),'') as accountsledger_Narration,'1' as AccountName,case when a.Accountsledger_AmountCr='0.00000000' then null else a.Accountsledger_AmountCr end as Accountsledger_AmountDr,case when a.Accountsledger_AmountDr='0.00000000' then null else a.Accountsledger_AmountDr end as Accountsledger_AmountCr,'0.0' as Closing,a.accountsledger_transactiondate as accountsledger_transactiondate,a.accountsledger_InstrumentNumber,(a.accountsledger_SettlementNumber+' '+a.accountsledger_SettlementType) as SettlementNumber,a.accountsledger_MainAccountID as MainID,a.accountsledger_SubAccountID as SubID,a.accountsledger_companyID as CompanyID,a.accountsledger_exchangeSegmentID as SegID,a.AccountsLedger_TransactionType  as CashType,(select top 1 convert(varchar(12),Settlements_StartDateTime,113) from master_settlements where Settlements_Number=a.accountsledger_SettlementNumber and Settlements_TypeSuffix=a.accountsledger_SettlementType) as PayoutDate,(select branch_code from tbl_master_branch where branch_id=(select cnt_branchid from tbl_master_contact where cnt_internalID=accountsledger_SubAccountID)) as BranchCode from Trans_accountsledger a  WHERE  accountsledger_companyID='" + Session["LastCompany"].ToString() + "' and  accountsledger_ExchangeSegmentID in(" + Request.QueryString["SegmentID"].ToString() + ") and  accountsledger_transactiondate  between  '" + dtFrom.Value + "'  and  '" + dtTo.Value + "'  and accountsledger_MainAccountID in('" + Request.QueryString["MainID"].ToString() + "')  and accountsledger_finyear='" + Session["LastFinYear"].ToString() + "' and AccountsLedger_SubAccountID in('" + Request.QueryString["SubID"].ToString() + "')   and AccountsLedger_SubAccountID is not null and AccountsLedger_TransactionType<>'OpeningBalance' ) as D", "TrDate,ValueDate,AccountsLedger_TransactionReferenceID  as accountsledger_TransactionReferenceID,accountsledger_Narration,AccountName,case when (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0)))>0 then (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0))) else null end Accountsledger_AmountDr,case when (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0)))<0 then (sum(isnull(Accountsledger_AmountCr,0))-sum(isnull(Accountsledger_AmountDr,0))) else null end Accountsledger_AmountCr,Closing,accountsledger_transactiondate,accountsledger_InstrumentNumber,SettlementNumber,MainID,SubID,CompanyID,SegID,CashType,PayoutDate,BranchCode,396 as UserID ", null, " TrDate,ValueDate,AccountsLedger_TransactionReferenceID,AccountName,Closing,accountsledger_transactiondate,accountsledger_InstrumentNumber,SettlementNumber,MainID,SubID,CompanyID,SegID,CashType,accountsledger_Narration,PayoutDate,BranchCode", " accountsledger_transactiondate");
                    dtCashBankBook = oDBEngine.GetDataTable("(Select convert(varchar(11),a.accountsledger_transactiondate,113) as TrDate,case when a.accountsledger_valuedate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),a.accountsledger_valuedate,113) end as ValueDate, AccountsLedger_TransactionReferenceID  as  AccountsLedger_TransactionReferenceID,case when a.AccountsLedger_TransactionType='Journal' and Left(AccountsLedger_TransactionReferenceID,1) in('X','Y','Z') and Left(AccountsLedger_TransactionReferenceID,2) not in('YD') and accountsledger_SettlementNumber is not null and accountsledger_SettlementNumber<>''	then 'Consolidated Entries For '+convert(varchar(11),a.accountsledger_transactiondate,113) 	when a.AccountsLedger_TransactionType='Journal'  and Left(AccountsLedger_TransactionReferenceID,2) in('YD') and accountsledger_SettlementNumber is not null and accountsledger_SettlementNumber<>''	then 'Short Back Add Delivery '+convert(varchar(11),a.accountsledger_transactiondate,113) else isnull(a.accountsledger_Narration,'') end +' - '++' - '+isnull(isnull((select MainAccount_Name from Master_MainAccount where MainAccount_AccountCode=AccountsLedger_CashBankName),AccountsLedger_CashBankName),'') as accountsledger_Narration,'1' as AccountName,case when a.Accountsledger_AmountCr='0.00000000' then null else a.Accountsledger_AmountCr end as Accountsledger_AmountDr,case when a.Accountsledger_AmountDr='0.00000000' then null else a.Accountsledger_AmountDr end as Accountsledger_AmountCr,'0.0' as Closing,a.accountsledger_transactiondate as accountsledger_transactiondate,a.accountsledger_InstrumentNumber,(a.accountsledger_SettlementNumber+' '+a.accountsledger_SettlementType) as SettlementNumber,a.accountsledger_MainAccountID as MainID,a.accountsledger_SubAccountID as SubID,a.accountsledger_companyID as CompanyID,a.accountsledger_exchangeSegmentID as SegID,a.AccountsLedger_TransactionType  as CashType,(select top 1 convert(varchar(12),Settlements_StartDateTime,113) from master_settlements where Settlements_Number=a.accountsledger_SettlementNumber and Settlements_TypeSuffix=a.accountsledger_SettlementType) as PayoutDate,(select branch_code from tbl_master_branch where branch_id=(select cnt_branchid from tbl_master_contact where cnt_internalID=accountsledger_SubAccountID)) as BranchCode from Trans_accountsledger a  WHERE  accountsledger_companyID='" + Session["LastCompany"].ToString() + "' and  accountsledger_ExchangeSegmentID in(" + Request.QueryString["SegmentID"].ToString() + ") and  accountsledger_transactiondate  between  '" + dtFrom.Value + "'  and  '" + dtTo.Value + "'  and accountsledger_MainAccountID in('" + Request.QueryString["MainID"].ToString() + "')  and accountsledger_finyear='" + Session["LastFinYear"].ToString() + "' and AccountsLedger_SubAccountID in('" + Request.QueryString["SubID"].ToString() + @"')   and AccountsLedger_SubAccountID is not null and AccountsLedger_TransactionType<>'OpeningBalance' 
                and isnull(AccountsLedger_Currency,
               " + Session["TradeCurrency"].ToString().Split('~')[0] + ")=" + Session["ActiveCurrency"].ToString().Split('~')[0] + ") as D", "TrDate,ValueDate,AccountsLedger_TransactionReferenceID  as accountsledger_TransactionReferenceID,accountsledger_Narration,AccountName,case when (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0)))>0 then (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0))) else null end Accountsledger_AmountDr,case when (sum(isnull(Accountsledger_AmountDr,0))-sum(isnull(Accountsledger_AmountCr,0)))<0 then (sum(isnull(Accountsledger_AmountCr,0))-sum(isnull(Accountsledger_AmountDr,0))) else null end Accountsledger_AmountCr,Closing,accountsledger_transactiondate,accountsledger_InstrumentNumber,SettlementNumber,MainID,SubID,CompanyID,SegID,CashType,PayoutDate,BranchCode,396 as UserID ", null, " TrDate,ValueDate,AccountsLedger_TransactionReferenceID,AccountName,Closing,accountsledger_transactiondate,accountsledger_InstrumentNumber,SettlementNumber,MainID,SubID,CompanyID,SegID,CashType,accountsledger_Narration,PayoutDate,BranchCode", " accountsledger_transactiondate");
                    OpenBalance = oDBEngine.OpeningBalanceJournal1("'" + Request.QueryString["MainID"].ToString() + "'", Request.QueryString["SubID"].ToString(), Convert.ToDateTime(dtFrom.Value), Request.QueryString["SegmentID"].ToString(), Session["LastCompany"].ToString(), Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString()), Convert.ToInt32(Session["ActiveCurrency"].ToString().Split('~')[0]));

                }
            }
            else
            {

                string ReportType = "";
                string SubAc = "";
                string mainAccountSearch = null;
                string SubAccountSearch = null;
                string SubACountForAll = null;
                string Settlement = "";
                string strTranType = "";
                string strCbPayment = "n";
                string strCbReceipt = "n";
                string strCbContract = "n";
                string strJvType = "";
                string strSelectedJv = "";
                if (RadSettA.Checked == true)
                {
                    Settlement = "";
                }
                else
                {
                    Settlement = HdnSettlement.Value;
                }

                ViewState["Clients"] = null;
                ViewState["branchID"] = null;
                SubLedgerType = HdnSubLedgerType.Value;
                SegmentT = HdnBranch.Value;
                SubAcID = HdnSubAc.Value;
                ViewState["SubAcID"] = SubAcID;
                MainAcID = HdnMainAc.Value;
                if (SubLedgerType == "CDSL Clients" || SubLedgerType == "NSDL Clients")
                    fn_ClientCDSL();
                else if (HdnForBranchGroup.Value != "a")
                    fn_Client();
                if (HdnSelectLedger.Value != "S")
                {
                    if (DrpChange != "Y")
                    {
                        FillDropDown();
                    }
                }

                SegMentName = ViewState["SegMentName"].ToString();
                ViewState["Check"] = "a";
                if (rdbSegAll.Checked == true)
                {
                    DataTable dtSegment = oDBEngine.GetDataTable("tbl_master_companyExchange", "exch_internalId", "exch_compId='" + Session["CompanyID"].ToString() + "'");
                    if (dtSegment.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtSegment.Rows.Count; i++)
                        {
                            if (Segment == null)
                                Segment = dtSegment.Rows[i][0].ToString();
                            else
                                Segment += "," + dtSegment.Rows[i][0].ToString();
                        }
                    }
                }
                else
                {
                    if (Session["userlastsegment"].ToString() == "5")
                    {
                        DataTable DtSeg = new DataTable();
                        DtSeg = oDBEngine.GetDataTable("(select exch_compId,exch_internalId,isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Comp from tbl_master_companyExchange where exch_compid in (select ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment=" + Session["userlastsegment"].ToString() + ")) as D", "*", "Comp in(select seg_name from tbl_master_segment where seg_id=" + Session["userlastsegment"].ToString() + ") and  exch_compID='" + Session["LastCompany"].ToString() + "'");
                        Segment = DtSeg.Rows[0][1].ToString();
                    }
                    else
                    {
                        if (SegmentT == null || SegmentT == "")
                        {
                            Segment = Session["SegmentID"].ToString();
                        }
                        else
                            Segment = SegmentT;
                    }
                }
                if (ViewState["branchID"] == null)
                {
                    if (rdbranchAll.Checked == true)
                    {
                        Branch = Session["userbranchHierarchy"].ToString();
                    }
                }
                else
                    Branch = ViewState["branchID"].ToString();


                if (rdSubAcAll.Checked == true)
                {
                    SubACountForAll = null;
                }
                else
                {
                    if (SubAcID == null || SubAcID == "")
                        SubACountForAll = null;
                    else
                        SubACountForAll = SubAcID;
                }
                if (ViewState["Clients"] != null)
                    SubACountForAll = ViewState["Clients"].ToString();
                if (ddlAccountType.SelectedValue == "0")
                {
                    mainAccountSearch = "'SYSTM00001'";
                    MainAcIDforOp = "'SYSTM00001'";
                    SubLedgerType = "Customers";
                }
                else if (ddlAccountType.SelectedValue == "1")
                {
                    mainAccountSearch = "'SYSTM00002'";
                    MainAcIDforOp = "'SYSTM00002'";
                    SubLedgerType = "Customers";
                }
                else if (ddlAccountType.SelectedValue == "2")
                {
                    mainAccountSearch = "'SYSTM00001','SYSTM00002'";
                    MainAcIDforOp = "'SYSTM00001','SYSTM00002'";
                    SubLedgerType = "Customers";
                }
                else
                {
                    MainAcIDforOp = MainAcID;
                    mainAccountSearch = MainAcID;
                }
                ViewState["MainAcIDforOp"] = MainAcIDforOp;
                ViewState["Segment"] = Segment;


                if (SubLedgerType.Trim() == "None")
                {
                    SubAccountSearch = null;
                    ViewState["SubAccountSearch"] = SubAccountSearch;
                }
                else
                {
                    if (cmbclientsPager.SelectedItem != null)
                        SubAccountSearch = "'" + cmbclientsPager.SelectedItem.Value + "'";
                    else
                        SubAccountSearch = SubAcID;
                }
                if (radConsolidated.Checked == true)
                {
                    if (radDateWise.Checked == true)
                    {
                        ReportType = "ConsolidateDt";
                    }
                    else if (radExpDateWise.Checked == true)
                    {

                        ReportType = "ConsolidateExp~" + litSegment.InnerText.Split('-')[1].Replace("'", "").Trim();
                    }
                    else
                    {
                        ReportType = "ConsolidateV";
                    }
                }
                else if (radBreakDetail.Checked == true)
                {
                    ReportType = "ObligationBrkUp";
                }
                else
                {
                    ReportType = "Detail";
                }
                if (SubAccountSearch == null)
                    SubAccountSearch = "";

                if (ddlAccountType.SelectedItem.Value.ToString() == "0" || ddlAccountType.SelectedItem.Value.ToString() == "1" || ddlAccountType.SelectedItem.Value.ToString() == "2")
                {
                    if (SubLedgerType.Trim() == "None")
                    {
                        SubLedgerType = "";
                    }
                }


                if (rbTanAll.Checked == true)
                    strTranType = "all";
                else if (rbTranCashBank.Checked == true)
                {
                    strTranType = "cb";
                    if (chkPayment.Checked == true)
                        strCbPayment = "y";
                    if (chkReceipts.Checked == true)
                        strCbReceipt = "y";
                    if (chkContracts.Checked == true)
                        strCbContract = "y";
                }
                else if (rbTranJv.Checked == true)
                {
                    strTranType = "jv";
                    if (rbAllJV.Checked == true)
                        strJvType = "all";
                    else if (rbManualJV.Checked == true)
                    {
                        strJvType = "man";
                        strSelectedJv = txtVoucherPrefix.Text;
                    }
                    else if (rbSystemJV.Checked == true)
                    {
                        strJvType = "sys";
                        //strSelectedJv = ViewState["Selectedjvs"].ToString();
                        if (rbSystemJVAll.Checked == true)
                            strSelectedJv = "all";
                        else if (rbSystemJVSelected.Checked == true)
                            strSelectedJv = hdnSystemJvs.Value;
                    }
                }
                if ((SubLedgerType.Trim() != "None" && cmbclientsPager.Items.Count != 0) || (SubLedgerType.Trim() == "None" && cmbclientsPager.Items.Count == 0))
                {
                    DataSet DS = new DataSet();
                    if (radBreakDetail.Checked == true)
                    {
                        using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
                        {
                            using (SqlDataAdapter da = new SqlDataAdapter("Fetch_LedgerForCrystalReport", con))
                            {
                                da.SelectCommand.Parameters.AddWithValue("@CompanyID", Session["LastCompany"].ToString());
                                da.SelectCommand.Parameters.AddWithValue("@FinYear", Session["LastFinYear"].ToString());
                                da.SelectCommand.Parameters.AddWithValue("@FromDate", dtFrom.Value);
                                da.SelectCommand.Parameters.AddWithValue("@ToDate", dtTo.Value);
                                da.SelectCommand.Parameters.AddWithValue("@MainAccount", mainAccountSearch);
                                da.SelectCommand.Parameters.AddWithValue("@SubAccount", SubAccountSearch);
                                da.SelectCommand.Parameters.AddWithValue("@Branch", Branch);
                                da.SelectCommand.Parameters.AddWithValue("@ReportType", ReportType);
                                da.SelectCommand.Parameters.AddWithValue("@Segment", Segment);
                                da.SelectCommand.Parameters.AddWithValue("@UserID", Session["userid"].ToString());
                                da.SelectCommand.Parameters.AddWithValue("@SubledgerType", SubLedgerType.Trim());
                                da.SelectCommand.Parameters.AddWithValue("@Settlement", Settlement);
                                da.SelectCommand.Parameters.AddWithValue("@TranType", strTranType);
                                da.SelectCommand.Parameters.AddWithValue("@CbPayment", Convert.ToChar(strCbPayment));
                                da.SelectCommand.Parameters.AddWithValue("@CbReceipt", Convert.ToChar(strCbReceipt));
                                da.SelectCommand.Parameters.AddWithValue("@CbContract", Convert.ToChar(strCbContract));
                                da.SelectCommand.Parameters.AddWithValue("@JvType", strJvType);
                                da.SelectCommand.Parameters.AddWithValue("@SelectedJv", strSelectedJv);
                                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                                da.SelectCommand.CommandTimeout = 0;

                                if (con.State == ConnectionState.Closed)
                                    con.Open();
                                DS.Reset();
                                da.Fill(DS);
                                ViewState["dsHTML"] = DS;
                                GenerateHTML();
                                // Mantis Issue 24802
                                if (con.State == ConnectionState.Open)
                                {
                                    con.Close();
                                }
                                // End of Mantis Issue 24802
                            }
                        }
                    }
                    else  //For Excel============
                    {


                        DS = objFAReportsOther.Fetch_LedgerView(
                              Convert.ToString(Session["LastCompany"]),
                              Convert.ToString(Session["LastFinYear"]),
                              Convert.ToString(dtFrom.Value),
                              Convert.ToString(dtTo.Value),
                              Convert.ToString(mainAccountSearch),
                              Convert.ToString(SubAccountSearch),
                              Convert.ToString(Branch),
                              Convert.ToString(ReportType),
                               Convert.ToString(Segment),
                               Convert.ToString(Session["userid"]),
                              Convert.ToString(Settlement),
                              Convert.ToString(strTranType),
                             Convert.ToString(strCbPayment),
                             Convert.ToString(strCbReceipt),
                             Convert.ToString(strCbContract),
                             Convert.ToString(strJvType),
                             Convert.ToString(strSelectedJv),
                              Session["ActiveCurrency"].ToString().Split('~')[0],
                              Session["TradeCurrency"].ToString().Split('~')[0]);
                        dtCashBankBook = DS.Tables[0];
                        dtLedgerView = DS.Tables[0];
                        OpenBalance = DS.Tables[1];
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "AlertScript", "alert('No  Record Found!')", true);
                }
            }
            if (radBreakDetail.Checked == false)
            {
                ViewState["dtCashBankBook"] = dtCashBankBook;
                ViewState["dtLedgerView"] = dtLedgerView;
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
                    #region
                    if (radBreakDetail.Checked == true)
                    {
                        string[] Narration1;
                        string[] Bill1;
                        string Bill = "0";
                        string TranID = dtCashBankBook.Rows[i]["accountsledger_TransactionReferenceID"].ToString().Substring(0, 2);
                        string[] Narration = dtCashBankBook.Rows[i]["accountsledger_Narration"].ToString().Split('[');
                        if (Narration.Length > 1)
                        {
                            Narration1 = Narration[1].Split(':');
                            if (Narration1.Length > 1)
                            {
                                Bill1 = Narration1[1].Split(']');
                                Bill = Bill1[0];
                            }
                        }
                        if (SegMentName == "NSE - CM" || SegMentName == "BSE - CM" || SegMentName == "NSE - CM,BSE - CM")
                        {
                            if (TranID == "XO")
                            {
                                DataTable dtProduct = oDBEngine.GetDataTable("Trans_CMPosition", "(select ltrim(rtrim(isnull(Equity_TickerSymbol,'')))+(case when isnull(convert(varchar(12),Equity_EffectUntil,113),'')='' then '' else ' ['+convert(varchar(12),Equity_EffectUntil,113)+']' end)+' ['+ltrim(rtrim(isnull(Equity_Series,rtrim(isnull(Equity_TickerCode,'')))))+']'+ case when isnull(cast(Equity_StrikePrice as varchar),'')='' then '' else ' ['+cast(Equity_StrikePrice as varchar)+']' end from master_Equity where Equity_SeriesID=Trans_CMPosition.CMPosition_ProductSeriesID) as CMPosition_ProductSeriesID,case when CMPosition_SqrOffQty=0 then null else CMPosition_SqrOffQty end as CMPosition_SqrOffQty,case when CMPosition_SqrOffPL=0 then null else CMPosition_SqrOffPL end as CMPosition_SqrOffPL,case when CMPosition_DeliveryBuyQty=0 then null else CMPosition_DeliveryBuyQty end as CMPosition_DeliveryBuyQty,case when CMPosition_DeliveryBuyValue=0 then null else CMPosition_DeliveryBuyValue end as CMPosition_DeliveryBuyValue,case when CMPosition_DeliverySellQty=0 then null else CMPosition_DeliverySellQty end as CMPosition_DeliverySellQty,case when CMPosition_DeliverySellValue=0 then null else CMPosition_DeliverySellValue end as CMPosition_DeliverySellValue,case when CMPosition_NetObligation=0 then null else CMPosition_NetObligation end as CMPosition_NetObligation,CMPosition_SettlementNumber+CMPosition_SettlementType as SettnumType,case when (isnull(CMPosition_DeliveryBuyValue,0)/isnull((case when CMPosition_DeliveryBuyQty=0 then null else CMPosition_DeliveryBuyQty end),1))=0 then null else (isnull(CMPosition_DeliveryBuyValue,0)/isnull((case when CMPosition_DeliveryBuyQty=0 then null else CMPosition_DeliveryBuyQty end),1)) end as AvgBuyVal,case when (isnull(CMPosition_DeliverySellValue,0)/isnull((case when CMPosition_DeliverySellQty=0 then null else CMPosition_DeliverySellQty end),1))=0 then null else (isnull(CMPosition_DeliverySellValue,0)/isnull((case when CMPosition_DeliverySellQty=0 then null else CMPosition_DeliverySellQty end),1)) end as AvgSellVal", " CmPosition_BillNumber='" + Bill + "'");
                                DataRow newRow1 = dtCashBankBook_New.NewRow();
                                String strHtmlAllClient = String.Empty;
                                decimal TotalVal = 0;
                                strHtmlAllClient = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + ">";
                                strHtmlAllClient += "<tr style=\"background-color: #DBEEF3;\">";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Scrip</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Delv.Buy Qty</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Delv.Buy Value</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Avg.Buy Value</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Delv.Sell Qty</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Delv.Sell Value</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Avg.Sell Value</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Sqr Qty</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Sqr P/L</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Total</td>";
                                strHtmlAllClient += "</tr>";
                                for (int j = 0; j < dtProduct.Rows.Count; j++)
                                {
                                    strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                                    strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\">" + dtProduct.Rows[j]["CMPosition_ProductSeriesID"] + "</td>";
                                    if (dtProduct.Rows[j]["CMPosition_DeliveryBuyQty"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["CMPosition_DeliveryBuyQty"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["CMPosition_DeliveryBuyValue"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["CMPosition_DeliveryBuyValue"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["AvgBuyVal"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["AvgBuyVal"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["CMPosition_DeliverySellQty"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["CMPosition_DeliverySellQty"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["CMPosition_DeliverySellValue"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["CMPosition_DeliverySellValue"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["AvgSellVal"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["AvgSellVal"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["CMPosition_SqrOffQty"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["CMPosition_SqrOffQty"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["CMPosition_SqrOffPL"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["CMPosition_SqrOffPL"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["CMPosition_NetObligation"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["CMPosition_NetObligation"])) + "</td></tr>";
                                        TotalVal += Convert.ToDecimal(dtProduct.Rows[j]["CMPosition_NetObligation"]);
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td></tr>";
                                    }
                                }
                                strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                                strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\"><b>Total :</b></td>";
                                strHtmlAllClient += "<td align=\"right\" colspan=\"9\" style=\"font-size:xx-small\"><b>" + oconverter.getFormattedvaluewithoriginalsign(TotalVal) + "</b></td></tr>";
                                strHtmlAllClient += "</table>";
                                newRow1[0] = "a";
                                newRow1[1] = dtProduct.Rows[0]["SettnumType"].ToString();
                                newRow1[2] = "";
                                newRow1[3] = strHtmlAllClient;
                                newRow1[4] = "";
                                newRow1[5] = 0;
                                newRow1[6] = 0;
                                newRow1[7] = "";
                                newRow1[8] = dtCashBankBook.Rows[i]["accountsledger_transactiondate"];
                                newRow1[9] = "";
                                newRow1[10] = "";
                                newRow1[11] = "";
                                newRow1[12] = "";
                                newRow1[13] = "";
                                newRow1[14] = "";
                                newRow1[15] = "";
                                newRow1[16] = "";
                                dtCashBankBook_New.Rows.Add(newRow1);
                                //displayALLCLIENT.InnerHtml = strHtmlAllClient;
                            }
                        }
                        else if (SegMentName == "NSE-FO")
                        {
                            if (TranID == "XO")
                            {
                                DataTable dtProduct = oDBEngine.GetDataTable("trans_foposition", "(select ltrim(rtrim(isnull(Equity_TickerSymbol,'')))+(case when isnull(convert(varchar(12),Equity_EffectUntil,113),'')='' then '' else ' ['+convert(varchar(12),Equity_EffectUntil,113)+']' end)+' ['+ltrim(rtrim(isnull(Equity_Series,'')))+']'+ case when isnull(CONVERT(VARCHAR,CONVERT(MONEY,cast(Equity_StrikePrice as varchar)),1),'')='' then '' when Equity_StrikePrice=0 then '' else ' ['+CONVERT(VARCHAR,CONVERT(MONEY,cast(Equity_StrikePrice as varchar)),1)+']' end from master_Equity where Equity_SeriesID=trans_foposition.FOPosition_ProductSeriesID) as FOPosition_ProductSeriesID,case when isnull(foposition_BFPriceUnits,0)=0 then null else foposition_BFPriceUnits end as foposition_BFPriceUnits,case when isnull(foposition_openprice,0)=0 then null else foposition_openprice end as foposition_openprice,case when isnull(foposition_buyLots,0)=0 then null else foposition_buyLots end as foposition_buyPriceUnits,case when isnull(foposition_BuyValue,0)=0 then foposition_BuyValue else foposition_BuyValue end as foposition_BuyValue,case when isnull(foposition_BuyAverage,0)=0 then null else foposition_BuyAverage end as foposition_BuyAverage,case when isnull(foposition_sellLots,0)=0 then null else foposition_sellLots end as foposition_sellPriceUnits,case when isnull(foposition_SellValue,0)=0 then null else foposition_SellValue end as foposition_SellValue,case when isnull(foposition_SellAverage,0)=0 then null else foposition_SellAverage end as foposition_SellAverage,case when isnull(foposition_PostExcAsnDlvLongPriceUnits,0)=0 then foposition_PostExcAsnDlvShortPriceUnits else (-1)*foposition_PostExcAsnDlvLongPriceUnits end as CFQty,case when foposition_PostExcAsnDlvLongValue=0 then foposition_PostExcAsnDlvShortValue else foposition_PostExcAsnDlvLongValue end as CFPrice,foposition_MTMPL,(select DailyStat_SettlementPrice from Trans_dailyStatistics where DailyStat_ProductSeriesID=trans_foposition.FOPosition_ProductSeriesID and DailyStat_ExchangeSegmentID=" + Session["ExchangeSegmentID"].ToString() + " and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,DailyStat_DateTime)) as datetime) =cast(DATEADD(dd, 0, DATEDIFF(dd, 0, '" + TranDate + "')) as datetime)) as SettPrice", " foposition_BillNumber='" + Bill + "' and foposition_productseriesid in(select equity_seriesid from master_equity where equity_foidentifier like 'FUT%' and equity_effectuntil<>'" + dtCashBankBook.Rows[i]["accountsledger_transactiondate"] + "')");
                                DataRow newRow1 = dtCashBankBook_New.NewRow();
                                String strHtmlAllClient = String.Empty;
                                decimal TotalVal = 0;
                                strHtmlAllClient = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + ">";
                                strHtmlAllClient += "<tr style=\"background-color: #DBEEF3;\">";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Scrip</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">B/F Qty</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Price</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Buy Qty</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Value</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Avg</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Sell Qty</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Value</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Avg</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">C/F Qty</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Price</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">MTM</td>";
                                strHtmlAllClient += "</tr>";
                                for (int j = 0; j < dtProduct.Rows.Count; j++)
                                {
                                    strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                                    strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\">" + dtProduct.Rows[j]["FOPosition_ProductSeriesID"] + "</td>";
                                    if (dtProduct.Rows[j]["foposition_BFPriceUnits"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_BFPriceUnits"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["foposition_openprice"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_openprice"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["foposition_buyPriceUnits"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_buyPriceUnits"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["foposition_BuyValue"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_BuyValue"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["foposition_BuyAverage"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_BuyValue"]) / Convert.ToDecimal(dtProduct.Rows[j]["foposition_buyPriceUnits"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["foposition_sellPriceUnits"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_sellPriceUnits"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["foposition_SellValue"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_SellValue"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["foposition_SellAverage"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_SellValue"]) / Convert.ToDecimal(dtProduct.Rows[j]["foposition_sellPriceUnits"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["CFQty"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["CFQty"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["SettPrice"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["SettPrice"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["foposition_MTMPL"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_MTMPL"])) + "</td></tr>";
                                        TotalVal += Convert.ToDecimal(dtProduct.Rows[j]["foposition_MTMPL"]);
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td></tr>";
                                    }
                                }
                                strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                                strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\"><b>Total :</b></td>";
                                strHtmlAllClient += "<td align=\"right\" colspan=\"11\" style=\"font-size:xx-small\"><b>" + oconverter.getFormattedvaluewithoriginalsign(TotalVal) + "</b></td></tr>";
                                strHtmlAllClient += "</table>";
                                newRow1[0] = "a";
                                newRow1[1] = "";
                                newRow1[2] = "";
                                newRow1[3] = strHtmlAllClient;
                                newRow1[4] = "";
                                newRow1[5] = 0;
                                newRow1[6] = 0;
                                newRow1[7] = "";
                                newRow1[8] = dtCashBankBook.Rows[i]["accountsledger_transactiondate"];
                                newRow1[9] = "";
                                newRow1[10] = "";
                                newRow1[11] = "";
                                newRow1[12] = "";
                                newRow1[13] = "";
                                newRow1[14] = "";
                                newRow1[15] = "";
                                newRow1[16] = "";
                                dtCashBankBook_New.Rows.Add(newRow1);
                                //displayALLCLIENT.InnerHtml = strHtmlAllClient;
                            }
                            if (TranID == "XP")
                            {
                                DataTable dtProduct = oDBEngine.GetDataTable("trans_foposition", "(select ltrim(rtrim(isnull(Equity_TickerSymbol,'')))+(case when isnull(convert(varchar(12),Equity_EffectUntil,113),'')='' then '' else ' ['+convert(varchar(12),Equity_EffectUntil,113)+']' end)+' ['+ltrim(rtrim(isnull(Equity_Series,'')))+']'+ case when isnull(CONVERT(VARCHAR,CONVERT(MONEY,cast(Equity_StrikePrice as varchar)),1),'')='' then '' when Equity_StrikePrice=0 then '' else ' ['+CONVERT(VARCHAR,CONVERT(MONEY,cast(Equity_StrikePrice as varchar)),1)+']' end from master_Equity where Equity_SeriesID=trans_foposition.FOPosition_ProductSeriesID) as FOPosition_ProductSeriesID,case when isnull(foposition_BuyLots,0)=0 then null else foposition_BuyLots end as foposition_BuyLots,case when isnull(foposition_BuyValue,0)=0 then null else foposition_BuyValue end as foposition_BuyValue,case when isnull(foposition_BuyAverage,0)=0 then null else foposition_BuyAverage end as foposition_BuyAverage,case when isnull(foposition_SellLots,0)=0 then null else foposition_SellLots end as foposition_SellLots,case when isnull(foposition_SellValue,0)=0 then null else foposition_SellValue end as foposition_SellValue,case when isnull(foposition_SellAverage,0)=0 then null else foposition_SellAverage end as foposition_SellAverage,case when isnull(foposition_NetPremium,0)=0 then null else foposition_NetPremium end as foposition_NetPremium ", " foposition_BillNumber='" + Bill + "' and  foposition_NetPremium is not null");
                                DataRow newRow1 = dtCashBankBook_New.NewRow();
                                String strHtmlAllClient = String.Empty;
                                decimal TotalVal = 0;
                                strHtmlAllClient = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + ">";
                                strHtmlAllClient += "<tr style=\"background-color: #DBEEF3;\">";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Scrip</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Buy Qty</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Value</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Avg</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Sell Qty</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Value</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Avg</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Net Premium</td>";
                                strHtmlAllClient += "</tr>";
                                for (int j = 0; j < dtProduct.Rows.Count; j++)
                                {
                                    strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                                    strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\">" + dtProduct.Rows[j]["FOPosition_ProductSeriesID"] + "</td>";
                                    if (dtProduct.Rows[j]["foposition_BuyLots"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_BuyLots"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["foposition_BuyValue"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_BuyValue"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["foposition_BuyAverage"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_BuyValue"]) / Convert.ToDecimal(dtProduct.Rows[j]["foposition_BuyLots"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["foposition_SellLots"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_SellLots"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["foposition_SellValue"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_SellValue"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["foposition_SellAverage"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_SellValue"]) / Convert.ToDecimal(dtProduct.Rows[j]["foposition_SellLots"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["foposition_NetPremium"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_NetPremium"])) + "</td></tr>";
                                        TotalVal += Convert.ToDecimal(dtProduct.Rows[j]["foposition_NetPremium"]);
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td></tr>";
                                    }
                                }
                                strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                                strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\"><b>Total :</b></td>";
                                strHtmlAllClient += "<td align=\"right\" colspan=\"11\" style=\"font-size:xx-small\"><b>" + oconverter.getFormattedvaluewithoriginalsign(TotalVal) + "</b></td></tr>";
                                strHtmlAllClient += "</table>";
                                newRow1[0] = "a";
                                newRow1[1] = "";
                                newRow1[2] = "";
                                newRow1[3] = strHtmlAllClient;
                                newRow1[4] = "";
                                newRow1[5] = 0;
                                newRow1[6] = 0;
                                newRow1[7] = "";
                                newRow1[8] = dtCashBankBook.Rows[i]["accountsledger_transactiondate"];
                                newRow1[9] = "";
                                newRow1[10] = "";
                                newRow1[11] = "";
                                newRow1[12] = "";
                                newRow1[13] = "";
                                newRow1[14] = "";
                                newRow1[15] = "";
                                newRow1[16] = "";
                                dtCashBankBook_New.Rows.Add(newRow1);
                                //displayALLCLIENT.InnerHtml = strHtmlAllClient;
                            }
                            if (TranID == "XZ")
                            {
                                DataTable dtProduct = oDBEngine.GetDataTable("trans_foposition", "(select ltrim(rtrim(isnull(Equity_TickerSymbol,'')))+(case when isnull(convert(varchar(12),Equity_EffectUntil,113),'')='' then '' else ' ['+convert(varchar(12),Equity_EffectUntil,113)+']' end)+' ['+ltrim(rtrim(isnull(Equity_Series,'')))+']'+ case when isnull(CONVERT(VARCHAR,CONVERT(MONEY,cast(Equity_StrikePrice as varchar)),1),'')='' then '' when Equity_StrikePrice=0 then '' else ' ['+CONVERT(VARCHAR,CONVERT(MONEY,cast(Equity_StrikePrice as varchar)),1)+']' end from master_Equity where Equity_SeriesID=trans_foposition.FOPosition_ProductSeriesID) as FOPosition_ProductSeriesID,case when foposition_ExcAsnDlvLots<0 then 'Exercise' else 'Assigned' end as SettType,case when isnull(foposition_ExcAsnDlvLots,0)=0 then null else abs(foposition_ExcAsnDlvLots) end as foposition_ExcAsnDlvLots,case when isnull(foposition_SettlementPrice,0)=0 then null else abs(foposition_SettlementPrice) end as foposition_SettlementPrice,case when isnull(foposition_ExcAsnDlvMarkedValue,0)=0 then null else foposition_ExcAsnDlvMarkedValue end as foposition_ExcAsnDlvMarkedValue ", " foposition_BillNumber='" + Bill + "' and  foposition_ExcAsnDlvMarkedValue is not null");
                                DataRow newRow1 = dtCashBankBook_New.NewRow();
                                String strHtmlAllClient = String.Empty;
                                decimal TotalVal = 0;
                                strHtmlAllClient = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + ">";
                                strHtmlAllClient += "<tr style=\"background-color: #DBEEF3;\">";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Scrip</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Sett.Type</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Qty</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Set.Price</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Net Amount</td>";
                                strHtmlAllClient += "</tr>";
                                for (int j = 0; j < dtProduct.Rows.Count; j++)
                                {
                                    strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                                    strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\">" + dtProduct.Rows[j]["FOPosition_ProductSeriesID"] + "</td>";
                                    strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\">" + dtProduct.Rows[j]["SettType"] + "</td>";
                                    if (dtProduct.Rows[j]["foposition_ExcAsnDlvLots"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_ExcAsnDlvLots"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["foposition_SettlementPrice"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_SettlementPrice"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["foposition_ExcAsnDlvMarkedValue"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_ExcAsnDlvMarkedValue"])) + "</td></tr>";
                                        TotalVal += Convert.ToDecimal(dtProduct.Rows[j]["foposition_ExcAsnDlvMarkedValue"]);
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td></tr>";
                                    }
                                }
                                strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                                strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\"><b>Total :</b></td>";
                                strHtmlAllClient += "<td align=\"right\" colspan=\"11\" style=\"font-size:xx-small\"><b>" + oconverter.getFormattedvaluewithoriginalsign(TotalVal) + "</b></td></tr>";
                                strHtmlAllClient += "</table>";
                                newRow1[0] = "a";
                                newRow1[1] = "";
                                newRow1[2] = "";
                                newRow1[3] = strHtmlAllClient;
                                newRow1[4] = "";
                                newRow1[5] = 0;
                                newRow1[6] = 0;
                                newRow1[7] = "";
                                newRow1[8] = dtCashBankBook.Rows[i]["accountsledger_transactiondate"];
                                newRow1[9] = "";
                                newRow1[10] = "";
                                newRow1[11] = "";
                                newRow1[12] = "";
                                newRow1[13] = "";
                                newRow1[14] = "";
                                newRow1[15] = "";
                                newRow1[16] = "";
                                dtCashBankBook_New.Rows.Add(newRow1);
                                //displayALLCLIENT.InnerHtml = strHtmlAllClient;
                            }
                            if (TranID == "XX")
                            {
                                DataTable dtProduct = oDBEngine.GetDataTable("trans_foposition", "(select ltrim(rtrim(isnull(Equity_TickerSymbol,'')))+(case when isnull(convert(varchar(12),Equity_EffectUntil,113),'')='' then '' else ' ['+convert(varchar(12),Equity_EffectUntil,113)+']' end)+' ['+ltrim(rtrim(isnull(Equity_Series,'')))+']'+ case when isnull(CONVERT(VARCHAR,CONVERT(MONEY,cast(Equity_StrikePrice as varchar)),1),'')='' then '' when Equity_StrikePrice=0 then '' else ' ['+CONVERT(VARCHAR,CONVERT(MONEY,cast(Equity_StrikePrice as varchar)),1)+']' end from master_Equity where Equity_SeriesID=trans_foposition.FOPosition_ProductSeriesID) as FOPosition_ProductSeriesID,case when isnull(foposition_BFPriceUnits,0)=0 then null else foposition_BFPriceUnits end as foposition_BFPriceUnits,case when isnull(foposition_openprice,0)=0 then null else foposition_openprice end as foposition_openprice,case when isnull(foposition_buyLots,0)=0 then null else foposition_buyLots end as foposition_buyPriceUnits,case when isnull(foposition_BuyValue,0)=0 then foposition_BuyValue else foposition_BuyValue end as foposition_BuyValue,case when isnull(foposition_BuyAverage,0)=0 then null else foposition_BuyAverage end as foposition_BuyAverage,case when isnull(foposition_sellLots,0)=0 then null else foposition_sellLots end as foposition_sellPriceUnits,case when isnull(foposition_SellValue,0)=0 then null else foposition_SellValue end as foposition_SellValue,case when isnull(foposition_SellAverage,0)=0 then null else foposition_SellAverage end as foposition_SellAverage,case when isnull(foposition_PostExcAsnDlvLongPriceUnits,0)=0 then foposition_PostExcAsnDlvShortPriceUnits else (-1)*foposition_PostExcAsnDlvLongPriceUnits end as CFQty,case when foposition_PostExcAsnDlvLongValue=0 then foposition_PostExcAsnDlvShortValue else foposition_PostExcAsnDlvLongValue end as CFPrice,foposition_FutureFinalSettlement,(select DailyStat_SettlementPrice from Trans_dailyStatistics where DailyStat_ProductSeriesID=trans_foposition.FOPosition_ProductSeriesID and DailyStat_ExchangeSegmentID=" + Session["ExchangeSegmentID"].ToString() + " and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,DailyStat_DateTime)) as datetime) =cast(DATEADD(dd, 0, DATEDIFF(dd, 0, '" + TranDate + "')) as datetime)) as SettPrice", " foposition_BillNumber='" + Bill + "' and foposition_productseriesid in(select equity_seriesid from master_equity where equity_foidentifier like 'FUT%' and equity_effectuntil='" + dtCashBankBook.Rows[i]["accountsledger_transactiondate"] + "')");
                                DataRow newRow1 = dtCashBankBook_New.NewRow();
                                String strHtmlAllClient = String.Empty;
                                decimal TotalVal = 0;
                                strHtmlAllClient = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + ">";
                                strHtmlAllClient += "<tr style=\"background-color: #DBEEF3;\">";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Scrip</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">B/F Qty</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Price</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Buy Qty</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Value</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Avg</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Sell Qty</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Value</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Avg</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Exp Qty</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Price</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Future Final Sett</td>";
                                strHtmlAllClient += "</tr>";
                                for (int j = 0; j < dtProduct.Rows.Count; j++)
                                {
                                    strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                                    strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\">" + dtProduct.Rows[j]["FOPosition_ProductSeriesID"] + "</td>";
                                    if (dtProduct.Rows[j]["foposition_BFPriceUnits"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_BFPriceUnits"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["foposition_openprice"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_openprice"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["foposition_buyPriceUnits"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_buyPriceUnits"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["foposition_BuyValue"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_BuyValue"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["foposition_BuyAverage"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_BuyValue"]) / Convert.ToDecimal(dtProduct.Rows[j]["foposition_buyPriceUnits"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["foposition_sellPriceUnits"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_sellPriceUnits"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["foposition_SellValue"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_SellValue"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["foposition_SellAverage"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_SellValue"]) / Convert.ToDecimal(dtProduct.Rows[j]["foposition_sellPriceUnits"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["CFQty"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["CFQty"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["SettPrice"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["SettPrice"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["foposition_FutureFinalSettlement"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["foposition_FutureFinalSettlement"])) + "</td></tr>";
                                        TotalVal += Convert.ToDecimal(dtProduct.Rows[j]["foposition_FutureFinalSettlement"]);
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td></tr>";
                                    }
                                }
                                strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                                strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\"><b>Total :</b></td>";
                                strHtmlAllClient += "<td align=\"right\" colspan=\"11\" style=\"font-size:xx-small\"><b>" + oconverter.getFormattedvaluewithoriginalsign(TotalVal) + "</b></td></tr>";
                                strHtmlAllClient += "</table>";
                                newRow1[0] = "a";
                                newRow1[1] = "";
                                newRow1[2] = "";
                                newRow1[3] = strHtmlAllClient;
                                newRow1[4] = "";
                                newRow1[5] = 0;
                                newRow1[6] = 0;
                                newRow1[7] = "";
                                newRow1[8] = dtCashBankBook.Rows[i]["accountsledger_transactiondate"];
                                newRow1[9] = "";
                                newRow1[10] = "";
                                newRow1[11] = "";
                                newRow1[12] = "";
                                newRow1[13] = "";
                                newRow1[14] = "";
                                newRow1[15] = "";
                                newRow1[16] = "";
                                dtCashBankBook_New.Rows.Add(newRow1);
                                //displayALLCLIENT.InnerHtml = strHtmlAllClient;
                            }
                        }
                        else if (SegMentName == "ICEX-COMM" || SegMentName == "MCX-COMM")
                        {
                            if (TranID == "XC")
                            {
                                DataTable dtProduct = oDBEngine.GetDataTable("Trans_comPosition", "(select ltrim(rtrim(isnull(Commodity_TickerSymbol,'')))+' ['+convert(varchar(12),Commodity_ExpiryDate,113)+']' from master_commodity where commodity_ProductSeriesID=trans_composition.comPosition_ProductSeriesID) as comPosition_ProductSeriesID,case when isnull(comPosition_BFPriceUnits,0)=0 then null else comPosition_BFPriceUnits end as comPosition_BFPriceUnits,case when isnull(comPosition_openprice,0)=0 then null else comPosition_openprice end as comPosition_openprice,case when isnull(comPosition_buyPriceUnits,0)=0 then null else comPosition_buyPriceUnits end as comPosition_buyPriceUnits,case when isnull(comPosition_BuyValue,0)=0 then comPosition_BuyValue else comPosition_BuyValue end as comPosition_BuyValue,case when isnull(comPosition_BuyAverage,0)=0 then null else comPosition_BuyAverage end as comPosition_BuyAverage,case when isnull(comPosition_sellPriceUnits,0)=0 then null else comPosition_sellPriceUnits end as comPosition_sellPriceUnits,case when isnull(comPosition_SellValue,0)=0 then null else comPosition_SellValue end as comPosition_SellValue,case when isnull(comPosition_SellAverage,0)=0 then null else comPosition_SellAverage end as comPosition_SellAverage,case when isnull(comPosition_PostExcAsnDlvLongPriceUnits,0)=0 then comPosition_PostExcAsnDlvShortPriceUnits else comPosition_PostExcAsnDlvLongPriceUnits end as CFQty,case when isnull(comPosition_PostExcAsnDlvLongValue,0)=0 then case when isnull(comPosition_PostExcAsnDlvShortValue,0)/abs(isnull(comPosition_PostExcAsnDlvShortPriceUnits,1))=0 then null else isnull(comPosition_PostExcAsnDlvShortValue,0)/abs(isnull(comPosition_PostExcAsnDlvShortPriceUnits,1)) end else case when isnull(comPosition_PostExcAsnDlvLongValue,0)/abs(isnull(comPosition_PostExcAsnDlvLongPriceUnits,1))=0 then null else isnull(comPosition_PostExcAsnDlvLongValue,0)/abs(isnull(comPosition_PostExcAsnDlvLongPriceUnits,1)) end end as CFPrice,comPosition_MTMPL", " comPosition_BillNumber='" + Bill + "'");
                                DataRow newRow1 = dtCashBankBook_New.NewRow();
                                String strHtmlAllClient = String.Empty;
                                decimal TotalVal = 0;
                                strHtmlAllClient = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + ">";
                                strHtmlAllClient += "<tr style=\"background-color: #DBEEF3;\">";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Scrip</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">B/F Qty</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Price</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Buy Qty</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Value</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Avg</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Sell Qty</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Value</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Avg</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">C/F Qty</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">Price</td>";
                                strHtmlAllClient += "<td align=\"center\" style=\"font-size:xx-small\">MTM</td>";
                                strHtmlAllClient += "</tr>";
                                for (int j = 0; j < dtProduct.Rows.Count; j++)
                                {
                                    strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                                    strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\">" + dtProduct.Rows[j]["comPosition_ProductSeriesID"] + "</td>";
                                    if (dtProduct.Rows[j]["comPosition_BFPriceUnits"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["comPosition_BFPriceUnits"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["comPosition_openprice"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["comPosition_openprice"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["comPosition_buyPriceUnits"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["comPosition_buyPriceUnits"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["comPosition_BuyValue"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["comPosition_BuyValue"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["comPosition_BuyAverage"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["comPosition_BuyAverage"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["comPosition_sellPriceUnits"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["comPosition_sellPriceUnits"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["comPosition_SellValue"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["comPosition_SellValue"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["comPosition_SellAverage"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["comPosition_SellAverage"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["CFQty"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(Convert.ToDecimal(dtProduct.Rows[j]["CFQty"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["CFPrice"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["CFPrice"])) + "</td>";
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td>";
                                    }
                                    if (dtProduct.Rows[j]["comPosition_MTMPL"] != DBNull.Value)
                                    {
                                        strHtmlAllClient += "<td nowrap=\"nowrap\" align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtProduct.Rows[j]["comPosition_MTMPL"])) + "</td></tr>";
                                        TotalVal += Convert.ToDecimal(dtProduct.Rows[j]["comPosition_MTMPL"]);
                                    }
                                    else
                                    {
                                        strHtmlAllClient += "<td>&nbsp;</td></tr>";
                                    }
                                }
                                strHtmlAllClient += "<tr style=\"background-color:white;text-align:center\">";
                                strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\"><b>Total :</b></td>";
                                strHtmlAllClient += "<td align=\"right\" colspan=\"11\" style=\"font-size:xx-small\"><b>" + oconverter.getFormattedvaluewithoriginalsign(TotalVal) + "</b></td></tr>";
                                strHtmlAllClient += "</table>";
                                newRow1[0] = "a";
                                newRow1[1] = "";
                                newRow1[2] = "";
                                newRow1[3] = strHtmlAllClient;
                                newRow1[4] = "";
                                newRow1[5] = 0;
                                newRow1[6] = 0;
                                newRow1[7] = "";
                                newRow1[8] = dtCashBankBook.Rows[i]["accountsledger_transactiondate"];
                                newRow1[9] = "";
                                newRow1[10] = "";
                                newRow1[11] = "";
                                newRow1[12] = "";
                                newRow1[13] = "";
                                newRow1[14] = "";
                                newRow1[15] = "";
                                newRow1[16] = "";
                                dtCashBankBook_New.Rows.Add(newRow1);
                                //displayALLCLIENT.InnerHtml = strHtmlAllClient;
                            }
                        }
                    }
                    #endregion
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

                CurrentPage.Value = pageindex.ToString();
                rowcount = 0;
                ViewState["dtCashBankBook"] = dtCashBankBook;
                DisplayOblg.Visible = false;

                string SpanText1 = oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "  - " + oconverter.ArrangeDate2(dtTo.Value.ToString());
                ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct", "ShowHide('" + SpanText1 + "')", true);
                if (cmbclientsPager.SelectedItem == null)
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JShide", "DisabledDrp('a');", true);
                else
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JShide", "DisabledDrp('b');", true);

                //}
                //catch(Exception ex)
                //{
                //    ViewState["Check"] = "b";
                //}
                DisplayOblg.Visible = false;
            }
            else
            {
                grdCashBankBook.Visible = false;
            }
        }
        protected void lnkExport_Click(object sender, EventArgs e)
        {
            string SegmentName = null;
            if (Session["ExchangeSegmentID"] == null)
            {
                SegmentName = null;


            }
            else
            {
                if (Session["ExchangeSegmentID"].ToString() == "1")
                    SegmentName = "NSE - CM";
                else if (Session["ExchangeSegmentID"].ToString() == "2")
                    SegmentName = "NSE - FO";
                else if (Session["ExchangeSegmentID"].ToString() == "3")
                    SegmentName = "NSE - CDX";
                else if (Session["ExchangeSegmentID"].ToString() == "4")
                    SegmentName = "BSE - CM";
                else if (Session["ExchangeSegmentID"].ToString() == "7")
                    SegmentName = "MCX - COMM";
                else if (Session["ExchangeSegmentID"].ToString() == "9")
                    SegmentName = "NCDEX - COMM";
                else if (Session["ExchangeSegmentID"].ToString() == "5")
                    SegmentName = "BSE - FO";
                else if (Session["ExchangeSegmentID"].ToString() == "6")
                    SegmentName = "BSE - CDX";
                else if (Session["ExchangeSegmentID"].ToString() == "8")
                    SegmentName = "MCXSX - CDX";
                else if (Session["ExchangeSegmentID"].ToString() == "10")
                    SegmentName = "DGCX - COMM";
                else if (Session["ExchangeSegmentID"].ToString() == "11")
                    SegmentName = "NMCE - COMM";
                else if (Session["ExchangeSegmentID"].ToString() == "12")
                    SegmentName = "ICEX - COMM";
                else if (Session["ExchangeSegmentID"].ToString() == "13")
                    SegmentName = "USE - CDX";
                else if (Session["ExchangeSegmentID"].ToString() == "14")
                    SegmentName = "NSEL - SPOT";
                else if (Session["ExchangeSegmentID"].ToString() == "15")
                    SegmentName = "CSE - CM";
                else if (Session["ExchangeSegmentID"].ToString() == "17")
                    SegmentName = "ACE - COMM";
                else if (Session["ExchangeSegmentID"].ToString() == "18")
                    SegmentName = "INMX - COMM";
                else if (Session["ExchangeSegmentID"].ToString() == "19")
                    SegmentName = "MCXSX - CM";
                else if (Session["ExchangeSegmentID"].ToString() == "20")
                    SegmentName = "MCXSX - FO";
                else if (Session["ExchangeSegmentID"].ToString() == "21")
                    SegmentName = "BFX - COMM";
                else if (Session["ExchangeSegmentID"].ToString() == "22")
                    SegmentName = "INSX - COMM";
                else if (Session["ExchangeSegmentID"].ToString() == "23")
                    SegmentName = "INFX - COMM";

            }

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
                for (int l = 0; l < cmbclientsPager.Items.Count; l++)
                {
                    if (l == 0)
                        SubAcID = "'" + cmbclientsPager.Items[l].Value + "'";
                    else
                        SubAcID = SubAcID + ",'" + cmbclientsPager.Items[l].Value + "'";
                }
                SubAccountSearch = " and AccountsLedger_SubAccountID in(" + SubAcID + ") ";
            }
            else
                SubAccountSearch = "NA";
            string ComID = Session["LastCompany"].ToString();
            Branch = Session["userbranchHierarchy"].ToString();

            if (ddlAccountType.SelectedValue == "0")
            {
                mainAccountSearch = "and accountsledger_MainAccountID in('SYSTM00001')";
                MainAcIDforOp = "'SYSTM00001'";
            }
            else if (ddlAccountType.SelectedValue == "1")
            {
                mainAccountSearch = "and accountsledger_MainAccountID in('SYSTM00002')";
                MainAcIDforOp = "'SYSTM00002'";
            }
            else if (ddlAccountType.SelectedValue == "2")
            {
                mainAccountSearch = "and accountsledger_MainAccountID in('SYSTM00001','SYSTM00002')";
                MainAcIDforOp = "'SYSTM00001','SYSTM00002'";
            }
            else
            {
                mainAccountSearch = "and accountsledger_MainAccountID in(" + MainAcID + ")";
                MainAcIDforOp = MainAcID;
            }
            if (radConsolidated.Checked == true)
            {
                if (radDateWise.Checked == true)
                {
                    Type = "DateWise";
                }
                else
                {
                    Type = "VoucherWise";
                }
            }
            else if (radDetail.Checked == true)
            {
                Type = "Detail";
            }
            else if (radBreakDetail.Checked == true)
            {
                Type = "BrkDetail";
            }
            if (chkDouble.Checked == true)
                SingleDouble = "D";
            else
                SingleDouble = "S";

            dsCrystal = objFAReportsOther.AccountsLedgerReport_Cryatal(
                Convert.ToString(ComID),
                Convert.ToString(Segment),
                Convert.ToDateTime(dtFrom.Value).ToString("yyyy-MM-dd"),
                 Convert.ToDateTime(dtTo.Value).ToString("yyyy-MM-dd"),
                 Convert.ToString(mainAccountSearch),
                Convert.ToString(Branch),
                Convert.ToString(SubAccountSearch),
                 Convert.ToString(Type),
                  Convert.ToString(MainAcIDforOp),
                   Convert.ToString(SubAcID),
                Convert.ToString(Session["LastFinYear"]),
               Convert.ToString(SingleDouble),
                   Convert.ToString(SegmentName));

            DataTable dtComp = oDBEngine.GetDataTable("tbl_master_company", "cmp_id", "cmp_internalid='" + Session["LastCompany"].ToString() + "'");
            string CompID = dtComp.Rows[0][0].ToString();


            byte[] logoinByte;


            dsCrystal.Tables[0].Columns.Add("Image", System.Type.GetType("System.Byte[]"));
            string filePath = @"..\images\logo_" + CompID.ToString().Trim() + ".bmp";
            if (oconverter.getLogoImage(HttpContext.Current.Server.MapPath(filePath), out logoinByte) != 1)
            {

                if (oconverter.getLogoImage(HttpContext.Current.Server.MapPath(@"..\images\logo.bmp"), out logoinByte) == 1)
                {
                    for (int i = 0; i < dsCrystal.Tables[0].Rows.Count; i++)
                    {
                        dsCrystal.Tables[0].Rows[i]["Image"] = logoinByte;
                    }

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


            }
            else
            {
                for (int i = 0; i < dsCrystal.Tables[0].Rows.Count; i++)
                {
                    dsCrystal.Tables[0].Rows[i]["Image"] = logoinByte;
                }
                // dsCrystal.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "//Reports//AccountsLedger.xsd");
                //dsCrystal.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "//Reports//AccountsLedger.xsd");
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

        #endregion

        #region PDF
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct2", "HideOnOffLoading()", true);

            string ReportType = "";
            string SubAc = "";
            string mainAccountSearch = null;
            string SubAccountSearch = null;
            string SubACountForAll = null;
            string strTranType = "";
            string strCbPayment = "n";
            string strCbReceipt = "n";
            string strCbContract = "n";
            string strJvType = "";
            string strSelectedJv = "";

            ViewState["Clients"] = null;
            ViewState["branchID"] = null;
            SubLedgerType = HdnSubLedgerType.Value;
            SegmentT = HdnBranch.Value;
            SubAcID = HdnSubAc.Value;
            ViewState["SubAcID"] = SubAcID;
            MainAcID = HdnMainAc.Value;
            if (SubLedgerType == "CDSL Clients" || SubLedgerType == "NSDL Clients")
                fn_ClientCDSL();
            else if (HdnForBranchGroup.Value != "a")
                fn_Client();

            if (HdnSelectLedger.Value != "S")
            {
                FillDropDownForPrint();

            }

            string Settlement = "";
            if (RadSettA.Checked == true)
            {
                Settlement = "";
            }
            else
            {
                Settlement = HdnSettlement.Value;
            }

            //if (HdnSelectLedger.Value != "S")
            //    FillDropDown();

            SegMentName = ViewState["SegMentName"].ToString();
            ViewState["Check"] = "a";
            if (rdbSegAll.Checked == true)
            {
                DataTable dtSegment = oDBEngine.GetDataTable("tbl_master_companyExchange", "exch_internalId", "exch_compId='" + Session["CompanyID"].ToString() + "'");
                if (dtSegment.Rows.Count > 0)
                {
                    for (int i = 0; i < dtSegment.Rows.Count; i++)
                    {
                        if (Segment == null)
                            Segment = dtSegment.Rows[i][0].ToString();
                        else
                            Segment += "," + dtSegment.Rows[i][0].ToString();
                    }
                }
            }
            else
            {
                if (Session["userlastsegment"].ToString() == "5")
                {
                    DataTable DtSeg = new DataTable();
                    DtSeg = oDBEngine.GetDataTable("(select exch_compId,exch_internalId,isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Comp from tbl_master_companyExchange where exch_compid in (select ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment=" + Session["userlastsegment"].ToString() + ")) as D", "*", "Comp in(select seg_name from tbl_master_segment where seg_id=" + Session["userlastsegment"].ToString() + ") and  exch_compID='" + Session["LastCompany"].ToString() + "'");
                    Segment = DtSeg.Rows[0][1].ToString();

                }
                else
                {
                    if (SegmentT == null || SegmentT == "")
                    {
                        Segment = Session["SegmentID"].ToString();
                    }
                    else
                        Segment = SegmentT;
                }
            }
            if (ViewState["branchID"] == null)
            {
                if (rdbranchAll.Checked == true)
                {
                    Branch = Session["userbranchHierarchy"].ToString();
                }
            }
            else
                Branch = ViewState["branchID"].ToString();


            if (rdSubAcAll.Checked == true)
            {
                SubACountForAll = null;
            }
            else
            {
                if (SubAcID == null || SubAcID == "")
                    SubACountForAll = null;
                else
                    SubACountForAll = SubAcID;
            }
            if (ViewState["Clients"] != null)
                SubACountForAll = ViewState["Clients"].ToString();

            if (ddlAccountType.SelectedValue == "0")
            {
                if (MainAcID == "" || MainAcID == null)
                {
                    mainAccountSearch = "'SYSTM00001'";
                }
                else
                {
                    mainAccountSearch = MainAcID;
                }
                if (SubLedgerType == "")
                    SubLedgerType = "Customers";

            }
            else if (ddlAccountType.SelectedValue == "1")
            {
                if (MainAcID == "" || MainAcID == null)
                {
                    mainAccountSearch = "'SYSTM00002'";
                }
                else
                {
                    mainAccountSearch = MainAcID;
                }
                if (SubLedgerType == "")
                    SubLedgerType = "Customers";
            }
            else if (ddlAccountType.SelectedValue == "2")
            {

                if (MainAcID == "" || MainAcID == null)
                {
                    mainAccountSearch = "'SYSTM00001','SYSTM00002'";
                }
                else
                {
                    mainAccountSearch = MainAcID;
                }

                if (SubLedgerType == "")
                    SubLedgerType = "Customers";
            }
            else
            {
                mainAccountSearch = MainAcID;
            }

            //if (ddlAccountType.SelectedValue == "0")
            //{
            //    mainAccountSearch = "'SYSTM00001'";
            //    SubLedgerType = "Customers";

            //}
            //else if (ddlAccountType.SelectedValue == "1")
            //{
            //    mainAccountSearch = "'SYSTM00002'";
            //    SubLedgerType = "Customers";
            //}
            //else if (ddlAccountType.SelectedValue == "2")
            //{
            //    mainAccountSearch = "'SYSTM00001','SYSTM00002'";
            //    SubLedgerType = "Customers";
            //}
            //else
            //{
            //    mainAccountSearch = MainAcID;
            //}
            ViewState["MainAcIDforOp"] = MainAcIDforOp;
            ViewState["Segment"] = Segment;

            if (SubLedgerType.Trim() == "None")
            {
                SubAccountSearch = "";
                ViewState["SubAccountSearch"] = "";
            }
            else
            {
                if (ViewState["Clients"] == null)
                {
                    SubAccountSearch = "";
                }
                else
                {
                    SubAccountSearch = ViewState["Clients"].ToString();
                }
                //if (cmbclientsPager.SelectedItem != null)
                //    SubAccountSearch = "'" + cmbclientsPager.SelectedItem.Value + "'";
                //else
                //    SubAccountSearch = SubAcID;

            }
            if (radBreakDetail.Checked == true)
            {
                ReportType = "ObligationBrkUp";
            }
            else if (radConsolidated.Checked == true)
            {
                if (radDateWise.Checked == true)
                    ReportType = "DateWise";
                else if (radExpDateWise.Checked == true)
                {

                    ReportType = "ConsolidateExp~" + litSegment.InnerText.Split('-')[1].Replace("'", "").Trim();
                }
                //else if (radVoucherWise.Checked == true)
                //    ReportType = "VoucherWise";
                else
                    ReportType = "VoucherWise";
            }
            else
            {
                ReportType = "Detail";
            }

            if (SubAccountSearch == "")
            {
                SubAccountSearch = " ";
            }
            string SingleDouble = null;
            if (chkBothPrint.Checked == true)
                SingleDouble = "D";
            else
                SingleDouble = "S";


            if (rbTanAll.Checked == true)
                strTranType = "all";
            else if (rbTranCashBank.Checked == true)
            {
                strTranType = "cb";
                if (chkPayment.Checked == true)
                    strCbPayment = "y";
                if (chkReceipts.Checked == true)
                    strCbReceipt = "y";
                if (chkContracts.Checked == true)
                    strCbContract = "y";
            }
            else if (rbTranJv.Checked == true)
            {
                strTranType = "jv";
                if (rbAllJV.Checked == true)
                    strJvType = "all";
                else if (rbManualJV.Checked == true)
                {
                    strJvType = "man";
                    strSelectedJv = txtVoucherPrefix.Text;
                }
                else if (rbSystemJV.Checked == true)
                {
                    strJvType = "sys";
                    //strSelectedJv = ViewState["Selectedjvs"].ToString();
                    if (rbSystemJVAll.Checked == true)
                        strSelectedJv = "all";
                    else if (rbSystemJVSelected.Checked == true)
                        strSelectedJv = hdnSystemJvs.Value;
                }
            }
            DataSet ds = new DataSet();
            DataSet dsopen = new DataSet();
            if (radBreakDetail.Checked == false)
            {


                ds = objFAReportsOther.Fetch_LedgerForopeningblnc1(
                    Convert.ToString(Session["LastCompany"]),
                    Convert.ToString(Session["LastFinYear"]),
                     Convert.ToString(dtFrom.Value),
                     Convert.ToString(dtTo.Value),
                    Convert.ToString(mainAccountSearch),
                    Convert.ToString(SubAccountSearch),
                    Convert.ToString(Branch),
                    Convert.ToString(ReportType),
                    Convert.ToString(Segment),
                    Convert.ToString(Session["userid"]),
                    Convert.ToString(SubLedgerType.Trim()),
                    Convert.ToString(Settlement),
                    Convert.ToString(strTranType),
                    Convert.ToString(strCbPayment),
                    Convert.ToString(strCbReceipt),
                    Convert.ToString(strCbContract),
                    Convert.ToString(strJvType),
                    Convert.ToString(strSelectedJv),
                    Session["ActiveCurrency"].ToString().Split('~')[0],
                    Session["TradeCurrency"].ToString().Split('~')[0]);

            }
            else
            {

                ds = objFAReportsOther.Fetch_LedgerFordosprintall(
                  Convert.ToString(Session["LastCompany"]),
                  Convert.ToString(Session["LastFinYear"]),
                   Convert.ToString(dtFrom.Value),
                   Convert.ToString(dtTo.Value),
                  Convert.ToString(mainAccountSearch),
                  Convert.ToString(SubAccountSearch),
                  Convert.ToString(Branch),
                  Convert.ToString(ReportType),
                  Convert.ToString(Segment),
                  Convert.ToString(Session["userid"]),
                  Convert.ToString(SubLedgerType.Trim()),
                  Convert.ToString(Settlement),
                  Convert.ToString(strTranType),
                  Convert.ToString(strCbPayment),
                  Convert.ToString(strCbReceipt),
                  Convert.ToString(strCbContract),
                  Convert.ToString(strJvType),
                  Convert.ToString(strSelectedJv),
                   Convert.ToString(txtHeader_hidden.Value),
                  Convert.ToString(txtFooter_hidden.Value));
            }
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[2].Rows.Count > 0)
                {
                    DataTable dtComp = oDBEngine.GetDataTable("tbl_master_company", "cmp_id", "cmp_internalid='" + Session["LastCompany"].ToString() + "'");
                    string CompID = dtComp.Rows[0][0].ToString();
                    byte[] logoinByte = null;
                    ds.Tables[3].Columns.Add("Image", System.Type.GetType("System.Byte[]"));
                    string filePath = @"..\images\logo_" + CompID.ToString().Trim() + ".bmp";
                    if (ChkLogo.Checked == true)
                    {
                        if (oconverter.getLogoImage(HttpContext.Current.Server.MapPath(filePath), out logoinByte) != 1)
                        {
                            if (oconverter.getLogoImage(HttpContext.Current.Server.MapPath(@"..\images\logo_" + CompID.ToString().Trim() + ".bmp"), out logoinByte) == 1)
                            {
                                for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                                {
                                    ds.Tables[3].Rows[i]["Image"] = logoinByte;
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                            {
                                ds.Tables[3].Rows[i]["Image"] = logoinByte;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                        {
                            ds.Tables[3].Rows[i]["Image"] = logoinByte;
                        }
                    }

                    ReportDocument report = new ReportDocument();
                    //ds.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "//Reports//LedgerCM.xsd");
                    //ds.Tables[1].WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "//Reports//LedgerCH.xsd");
                    //ds.Tables[2].WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "//Reports//LedgerCN.xsd");
                    report.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;

                    string tmpPdfPath = string.Empty;
                    if (ReportType == "ObligationBrkUp")
                        tmpPdfPath = HttpContext.Current.Server.MapPath("..\\Reports\\LedgerPrint.rpt");
                    else
                        tmpPdfPath = HttpContext.Current.Server.MapPath("..\\Reports\\LedgerPrintDetail.rpt");

                    report.Load(tmpPdfPath);
                    report.SetDataSource(ds);
                    report.VerifyDatabase();

                    //====Check CM Selection To show Settlement No and Trade Date======
                    string isCMSelected = "";
                    //DataTable DtCheckForCM = oDBEngine.GetDataTable("Select * From tbl_master_companyExchange where exch_internalId in (" + Segment + ") And exch_segmentId='CM'");
                    DataTable DtCheckForCM = oDBEngine.GetDataTable("Select * From tbl_master_companyExchange where exch_internalId in (" + Segment + ") And exch_segmentId<>'CM'");
                    if (DtCheckForCM != null)
                    {
                        if (DtCheckForCM.Rows.Count > 0)
                            isCMSelected = "false";
                        else
                            isCMSelected = "true";
                    }
                    DtCheckForCM.Dispose();
                    report.SetParameterValue("@IsSelected_CM", isCMSelected);

                    string MainAcc = "";
                    if (ddlAccountType.SelectedItem.Value == "0")
                    {
                        MainAcc = "Clients - Trading A/c  ";
                    }
                    else if (ddlAccountType.SelectedItem.Value == "1")
                    {
                        MainAcc = "Clients - Margin Deposit A/c  ";
                    }
                    else if (ddlAccountType.SelectedItem.Value == "2")
                    {
                        MainAcc = "Clients - Trading A/c and Clients - Margin Deposit A/c  ";
                    }
                    else if (ddlAccountType.SelectedItem.Value == "3")
                    {
                        MainAcc = txtMainAccount.Text;
                    }

                    if (rdbSegAll.Checked == true)
                    {
                        report.SetParameterValue("@Segment", (string)"ALL");
                    }
                    else
                    {
                        report.SetParameterValue("@Segment", (string)HDNSeg.Value.ToString());
                    }

                    report.SetParameterValue("@MainAccount", (string)MainAcc);
                    report.SetParameterValue("@Period", (string)"From " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + " To " + oconverter.ArrangeDate2(dtTo.Value.ToString()));
                    report.SetParameterValue("@PrintType", (string)SingleDouble);
                    report.SetParameterValue("@fromdate", (string)oconverter.ArrangeDate2(dtFrom.Value.ToString()));
                    report.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "LDGERVIEW");
                    report.Dispose();
                    GC.Collect();
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "NoRecord", "<script>norecordfound();</script>");
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "NoRecord", "<script>norecordfound();</script>");

            }
            //else if (dsopen.Tables[2].Rows.Count > 0)
            //{
            //    DataTable dtComp = oDBEngine.GetDataTable("tbl_master_company", "cmp_id", "cmp_internalid='" + Session["LastCompany"].ToString() + "'");
            //    string CompID = dtComp.Rows[0][0].ToString();
            //    byte[] logoinByte = null;
            //    dsopen.Tables[3].Columns.Add("Image", System.Type.GetType("System.Byte[]"));
            //    string filePath = @"..\images\logo_" + CompID.ToString().Trim() + ".bmp";
            //    if (ChkLogo.Checked == true)
            //    {
            //        if (oconverter.getLogoImage(HttpContext.Current.Server.MapPath(filePath), out logoinByte) != 1)
            //        {
            //            if (oconverter.getLogoImage(HttpContext.Current.Server.MapPath(@"..\images\logo.bmp"), out logoinByte) == 1)
            //            {
            //                for (int i = 0; i < dsopen.Tables[3].Rows.Count; i++)
            //                {
            //                    dsopen.Tables[3].Rows[i]["Image"] = logoinByte;
            //                }
            //            }
            //        }
            //        else
            //        {
            //            for (int i = 0; i < dsopen.Tables[3].Rows.Count; i++)
            //            {
            //                dsopen.Tables[3].Rows[i]["Image"] = logoinByte;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        for (int i = 0; i < dsopen.Tables[3].Rows.Count; i++)
            //        {
            //            dsopen.Tables[3].Rows[i]["Image"] = logoinByte;
            //        }
            //    }
            //    ReportDocument report = new ReportDocument();
            //    //  ds.WriteXmlSchema("E:\\RPTXSD\\LedgerCM.xsd");
            //    //ds.Tables[1].WriteXmlSchema("E:\\RPTXSD\\LedgerCH.xsd");
            //    //ds.Tables[2].WriteXmlSchema("E:\\RPTXSD\\LedgerCN.xsd");
            //    report.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
            //    string tmpPdfPath = string.Empty;
            //    //if (ds.Tables[""]
            //    if (ReportType == "ObligationBrkUp")
            //    {
            //        tmpPdfPath = HttpContext.Current.Server.MapPath("..\\Reports\\LedgerPrint.rpt");
            //    }
            //    else
            //    {
            //        tmpPdfPath = HttpContext.Current.Server.MapPath("..\\Reports\\LedgerPrintopening.rpt");

            //        //tmpPdfPath = HttpContext.Current.Server.MapPath("..\\Reports\\LedgerPrintDetail.rpt");
            //    }

            //    report.Load(tmpPdfPath);
            //    report.SetDataSource(dsopen);
            //    report.VerifyDatabase();

            //    string MainAcc = "";
            //    if (ddlAccountType.SelectedItem.Value == "0")
            //    {
            //        MainAcc = "Clients - Trading A/c  ";
            //    }
            //    else if (ddlAccountType.SelectedItem.Value == "1")
            //    {
            //        MainAcc = "Clients - Margin Deposit A/c  ";
            //    }
            //    else if (ddlAccountType.SelectedItem.Value == "2")
            //    {
            //        MainAcc = "Clients - Trading A/c and Clients - Margin Deposit A/c  ";
            //    }
            //    else if (ddlAccountType.SelectedItem.Value == "3")
            //    {
            //        MainAcc = txtMainAccount.Text;
            //    }

            //    if (rdbSegAll.Checked == true)
            //    {
            //        report.SetParameterValue("@Segment", (string)"ALL");
            //    }
            //    else
            //    {
            //        report.SetParameterValue("@Segment", (string)HDNSeg.Value.ToString());
            //    }

            //    report.SetParameterValue("@MainAccount", (string)MainAcc);
            //    report.SetParameterValue("@Period", (string)"From " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + " To " + oconverter.ArrangeDate2(dtTo.Value.ToString()));
            //    report.SetParameterValue("@PrintType", (string)SingleDouble);

            //    report.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "LDGERVIEW");
            //    report.Dispose();
            //    GC.Collect();
            //}
        }
        #endregion

        #region Screen
        protected void Button2_Click(object sender, EventArgs e)
        {
            BtnclickShow = "Click";
            HDNMAIN.Value = Request.QueryString["MainID"].ToString();
            FillGrid();
            //Page.ClientScript.RegisterStartupScript(GetType(), "Led", "<script>OnlySubsidiaryTrial();</script>");
            //  ScriptManager.RegisterStartupScript(this,this.GetType(),"HideAll", "OnlySubsidiaryTrial()", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct14", "OnlySubsidiaryTrial()", true);

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
        public override void VerifyRenderingInServerForm(System.Web.UI.Control control)
        {

        }
        #endregion

        #region Dos Print
        public void FillDropDownForPrint()
        {
            string SubID = null;
            SubAcID = HdnSubAc.Value;
            string MainSubID = null;
            if (rdSubAcAll.Checked == true)
            {
                SubAcID = null;
            }
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
                if (ddlAccountType.SelectedValue == "0")
                {
                    MainID = " subaccount_mainacreferenceid in('SYSTM00001')";
                    MainLedgerID = " and accountsledger_mainaccountid in('SYSTM00001')";
                }
                else if (ddlAccountType.SelectedValue == "1")
                {
                    MainID = " subaccount_mainacreferenceid in('SYSTM00002')";
                    MainLedgerID = " and accountsledger_mainaccountid in('SYSTM00002')";
                }
                else if (ddlAccountType.SelectedValue == "2")
                {
                    MainID = " subaccount_mainacreferenceid in('SYSTM00001','SYSTM00002')";
                    MainLedgerID = " and accountsledger_mainaccountid in('SYSTM00001','SYSTM00002')";
                }
                else
                {
                    MainID = " subaccount_mainacreferenceid not in('SYSTM00001','SYSTM00002')";
                    MainLedgerID = " and accountsledger_mainaccountid not in('SYSTM00001','SYSTM00002')";
                }
            }
            else
            {
                MainID = " subaccount_mainacreferenceid in(" + MainAcID + ")";
                MainLedgerID = " and accountsledger_mainaccountid in(" + MainAcID + ")";
                //MainID = MainAcID;
            }
            if (rdbSegAll.Checked == true)
            {
                DataTable dtSegment = oDBEngine.GetDataTable("tbl_master_companyExchange", "exch_internalId", "exch_compId='" + Session["CompanyID"].ToString() + "'");
                if (dtSegment.Rows.Count > 0)
                {
                    for (int i = 0; i < dtSegment.Rows.Count; i++)
                    {
                        if (Segment == null)
                            Segment = dtSegment.Rows[i][0].ToString();
                        else
                            Segment += "," + dtSegment.Rows[i][0].ToString();
                    }
                }
            }
            if (Segment == null)
            {
                Segment = Session["SegmentID"].ToString();
            }
            if (SubLedgerType.Trim() != "None")
            {

                DataTable dtclient = new DataTable();
                string DrpClient = "";
                cmbclientsPager.Items.Clear();
                if (ViewState["Clients"] == null)
                    dtclient = oDBEngine.GetDataTable("(	select subaccount_code,ltrim(rtrim(subaccount_name))+isnull(' ['+isnull((select ltrim(rtrim(cnt_ucc)) from tbl_master_contact where ltrim(rtrim(cnt_internalid))=ltrim(rtrim(master_subaccount.subAccount_Code))),isnull((select ltrim(rtrim(cdslClients_BenAccountNumber)) from master_cdslClients where ltrim(rtrim(cdslClients_BenAccountNumber))=ltrim(rtrim(master_subaccount.subAccount_Code))),(select ltrim(rtrim(nsdlClients_BenAccountID)) from master_nsdlClients where ltrim(rtrim(nsdlClients_BenAccountID))=ltrim(rtrim(master_subaccount.subAccount_Code)))))+']','') as subaccount_name	from master_subaccount	where " + MainID + " " + SubID + "	and subaccount_code in(select accountsledger_subaccountid from trans_accountsledger where cast(DATEADD(dd, 0, DATEDIFF(dd, 0,accountsledger_transactiondate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtFrom.Value + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtTo.Value + "')) as datetime) " + MainLedgerID + " and accountsledger_exchangesegmentid in(" + Segment + ") and accountsledger_BranchID in(" + Session["userbranchHierarchy"].ToString() + ")) and subaccount_name is not null	union all	select subaccount_code,ltrim(rtrim(subaccount_name))+isnull(' ['+isnull((select ltrim(rtrim(cnt_ucc)) from tbl_master_contact where ltrim(rtrim(cnt_internalid))=ltrim(rtrim(master_subaccount.subAccount_Code))),isnull((select ltrim(rtrim(cdslClients_BenAccountNumber)) from master_cdslClients where ltrim(rtrim(cdslClients_BenAccountNumber))=ltrim(rtrim(master_subaccount.subAccount_Code))),(select ltrim(rtrim(nsdlClients_BenAccountID)) from master_nsdlClients where ltrim(rtrim(nsdlClients_BenAccountID))=ltrim(rtrim(master_subaccount.subAccount_Code)))))+']','') as subaccount_name	from (select sum(AccountsLedger_AmountDr)-sum(AccountsLedger_AmountCr) as Amount,AccountsLedger_SubAccountID ,AccountsLedger_MainAccountID from  Trans_AccountsLedger where   AccountsLedger_FinYear='" + Session["LastFinYear"].ToString() + "' " + MainLedgerID + " and accountsledger_transactiondate<'" + dtFrom.Value + "'	" + MainSubID + " and accountsledger_exchangesegmentid in(" + Segment + ")	group by AccountsLedger_MainAccountID,AccountsLedger_SubAccountID) as DD,master_subaccount 	where Amount<>0 and AccountsLedger_SubAccountID=SubAccount_Code and AccountsLedger_MainAccountID=SubAccount_MainAcReferenceID) as KK", " distinct subaccount_code", null);
                else
                    dtclient = oDBEngine.GetDataTable("(	select subaccount_code,ltrim(rtrim(subaccount_name))+isnull(' ['+isnull((select ltrim(rtrim(cnt_ucc)) from tbl_master_contact where ltrim(rtrim(cnt_internalid))=ltrim(rtrim(master_subaccount.subAccount_Code))),isnull((select ltrim(rtrim(cdslClients_BenAccountNumber)) from master_cdslClients where ltrim(rtrim(cdslClients_BenAccountNumber))=ltrim(rtrim(master_subaccount.subAccount_Code))),(select ltrim(rtrim(nsdlClients_BenAccountID)) from master_nsdlClients where ltrim(rtrim(nsdlClients_BenAccountID))=ltrim(rtrim(master_subaccount.subAccount_Code)))))+']','') as subaccount_name	from master_subaccount	where " + MainID + " " + SubID + "	and subaccount_code in(select accountsledger_subaccountid from trans_accountsledger where cast(DATEADD(dd, 0, DATEDIFF(dd, 0,accountsledger_transactiondate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtFrom.Value + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtTo.Value + "')) as datetime) " + MainLedgerID + " and accountsledger_exchangesegmentid in(" + Segment + ") and accountsledger_BranchID in(" + ViewState["branchID"].ToString() + ")) and subaccount_code in(" + ViewState["Clients"].ToString() + ")	union all	select subaccount_code,ltrim(rtrim(subaccount_name))+isnull(' ['+isnull((select ltrim(rtrim(cnt_ucc)) from tbl_master_contact where ltrim(rtrim(cnt_internalid))=ltrim(rtrim(master_subaccount.subAccount_Code))),isnull((select ltrim(rtrim(cdslClients_BenAccountNumber)) from master_cdslClients where ltrim(rtrim(cdslClients_BenAccountNumber))=ltrim(rtrim(master_subaccount.subAccount_Code))),(select ltrim(rtrim(nsdlClients_BenAccountID)) from master_nsdlClients where ltrim(rtrim(nsdlClients_BenAccountID))=ltrim(rtrim(master_subaccount.subAccount_Code)))))+']','') as subaccount_name	from (select sum(AccountsLedger_AmountDr)-sum(AccountsLedger_AmountCr) as Amount,AccountsLedger_SubAccountID ,AccountsLedger_MainAccountID from  Trans_AccountsLedger where   AccountsLedger_FinYear='" + Session["LastFinYear"].ToString() + "' " + MainLedgerID + " and accountsledger_transactiondate<'" + dtFrom.Value + "'	" + MainSubID + " and accountsledger_exchangesegmentid in(" + Segment + ")	group by AccountsLedger_MainAccountID,AccountsLedger_SubAccountID) as DD,master_subaccount 	where Amount<>0 and AccountsLedger_SubAccountID=SubAccount_Code and AccountsLedger_MainAccountID=SubAccount_MainAcReferenceID and subaccount_code in(" + ViewState["Clients"].ToString() + ")) as KK", " distinct subaccount_code", null);


                if (dtclient.Rows.Count > 0)
                {
                    for (int i = 0; i < dtclient.Rows.Count; i++)
                    {
                        if (DrpClient == "")
                            DrpClient = "'" + dtclient.Rows[i][0].ToString() + "'";
                        else
                            DrpClient += "," + "'" + dtclient.Rows[i][0].ToString() + "'";
                    }

                }

                ViewState["Clients"] = DrpClient;
                //cmbclientsPager.DataSource = Clients;
                //cmbclientsPager.DataValueField = "subaccount_code";
                //cmbclientsPager.DataTextField = "subaccount_name";
                //cmbclientsPager.DataBind();
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct111", "HideoffOnButton()", true);
        }

        public void BtnGenerate_Click(object sender, EventArgs e)
        {
            string Path = string.Empty;

            CrystalDecisions.Web.CrystalReportViewer a1 = new CrystalDecisions.Web.CrystalReportViewer();
            CrystalDecisions.Web.CrystalReportSource a2 = new CrystalDecisions.Web.CrystalReportSource();
            string[] connPath = ConfigurationManager.AppSettings["DBConnectionDefault"].Split(';');
            ReportDocument rpt = new ReportDocument();
            PrintDocument pd = new PrintDocument();
            DataSet dsCrystal = new DataSet();
            string ReportType = "";
            string SubAc = "";
            string mainAccountSearch = null;
            string SubAccountSearch = null;
            string SubACountForAll = null;
            string strTranType = "";
            string strCbPayment = "n";
            string strCbReceipt = "n";
            string strCbContract = "n";
            string strJvType = "";
            string strSelectedJv = "";
            // string sXMLFileName, sOutputFileName, sData;
            DataTable dt1 = new DataTable();
            DataSet ds1 = new DataSet();
            string printername = rpt.PrintOptions.PrinterName;

            //System.Drawing.Printing.PrinterSettings print1 = new System.Drawing.Printing.PrinterSettings();
            //int print2 = 0;

            ViewState["Clients"] = null;
            ViewState["branchID"] = null;
            SubLedgerType = HdnSubLedgerType.Value;
            SegmentT = HdnBranch.Value;
            SubAcID = HdnSubAc.Value;
            ViewState["SubAcID"] = SubAcID;
            MainAcID = HdnMainAc.Value;
            if (SubLedgerType == "CDSL Clients" || SubLedgerType == "NSDL Clients")
                fn_ClientCDSL();
            else if (HdnForBranchGroup.Value != "a")
                fn_Client();

            if (HdnSelectLedger.Value != "S")
            {
                FillDropDownForPrint();
            }
            string Settlement = "";
            if (RadSettA.Checked == true)
            {
                Settlement = "";
            }
            else
            {
                Settlement = HdnSettlement.Value;
            }

            //if (HdnSelectLedger.Value != "S")
            //    FillDropDown();

            SegMentName = ViewState["SegMentName"].ToString();
            ViewState["Check"] = "a";
            if (rdbSegAll.Checked == true)
            {
                DataTable dtSegment = oDBEngine.GetDataTable("tbl_master_companyExchange", "exch_internalId", "exch_compId='" + Session["CompanyID"].ToString() + "'");
                if (dtSegment.Rows.Count > 0)
                {
                    for (int i = 0; i < dtSegment.Rows.Count; i++)
                    {
                        if (Segment == null)
                            Segment = dtSegment.Rows[i][0].ToString();
                        else
                            Segment += "," + dtSegment.Rows[i][0].ToString();
                    }
                }
            }
            else
            {
                if (Session["userlastsegment"].ToString() == "5")
                {
                    DataTable DtSeg = new DataTable();
                    DtSeg = oDBEngine.GetDataTable("(select exch_compId,exch_internalId,isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Comp from tbl_master_companyExchange where exch_compid in (select ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment=" + Session["userlastsegment"].ToString() + ")) as D", "*", "Comp in(select seg_name from tbl_master_segment where seg_id=" + Session["userlastsegment"].ToString() + ") and  exch_compID='" + Session["LastCompany"].ToString() + "'");
                    Segment = DtSeg.Rows[0][1].ToString();

                }
                else
                {
                    if (SegmentT == null || SegmentT == "")
                    {
                        Segment = Session["SegmentID"].ToString();
                    }
                    else
                        Segment = SegmentT;
                }
            }
            if (ViewState["branchID"] == null)
            {
                if (rdbranchAll.Checked == true)
                {
                    Branch = Session["userbranchHierarchy"].ToString();
                }
            }
            else
                Branch = ViewState["branchID"].ToString();

            if (rdSubAcAll.Checked == true)
            {
                SubACountForAll = null;
            }
            else
            {
                if (SubAcID == null || SubAcID == "")
                    SubACountForAll = null;
                else
                    SubACountForAll = SubAcID;
            }
            if (ViewState["Clients"] != null)
                SubACountForAll = ViewState["Clients"].ToString();

            if (ddlAccountType.SelectedValue == "0")
            {
                if (MainAcID == "" || MainAcID == null)
                {
                    mainAccountSearch = "'SYSTM00001'";
                }
                else
                {
                    mainAccountSearch = MainAcID;
                }
                if (SubLedgerType == "")
                    SubLedgerType = "Customers";

            }
            else if (ddlAccountType.SelectedValue == "1")
            {
                if (MainAcID == "" || MainAcID == null)
                {
                    mainAccountSearch = "'SYSTM00002'";
                }
                else
                {
                    mainAccountSearch = MainAcID;
                }
                if (SubLedgerType == "")
                    SubLedgerType = "Customers";
            }
            else if (ddlAccountType.SelectedValue == "2")
            {

                if (MainAcID == "" || MainAcID == null)
                {
                    mainAccountSearch = "'SYSTM00001','SYSTM00002'";
                }
                else
                {
                    mainAccountSearch = MainAcID;
                }

                if (SubLedgerType == "")
                    SubLedgerType = "Customers";
            }
            else
            {
                mainAccountSearch = MainAcID;
            }

            //if (ddlAccountType.SelectedValue == "0")
            //{
            //    mainAccountSearch = "'SYSTM00001'";
            //    SubLedgerType = "Customers";

            //}
            //else if (ddlAccountType.SelectedValue == "1")
            //{
            //    mainAccountSearch = "'SYSTM00002'";
            //    SubLedgerType = "Customers";
            //}
            //else if (ddlAccountType.SelectedValue == "2")
            //{
            //    mainAccountSearch = "'SYSTM00001','SYSTM00002'";
            //    SubLedgerType = "Customers";
            //}
            //else
            //{
            //    mainAccountSearch = MainAcID;
            //}
            ViewState["MainAcIDforOp"] = MainAcIDforOp;
            ViewState["Segment"] = Segment;

            if (SubLedgerType.Trim() == "None")
            {
                SubAccountSearch = "";
                ViewState["SubAccountSearch"] = "";
            }
            else
            {
                if (ViewState["Clients"] == null)
                {
                    SubAccountSearch = "";
                }
                else
                {
                    SubAccountSearch = ViewState["Clients"].ToString();
                }
                //if (cmbclientsPager.SelectedItem != null)
                //    SubAccountSearch = "'" + cmbclientsPager.SelectedItem.Value + "'";
                //else
                //    SubAccountSearch = SubAcID;
            }
            if (radBreakDetail.Checked == true)
            {
                ReportType = "ObligationBrkUp";
            }
            else if (radConsolidated.Checked == true)
            {
                if (radDateWise.Checked == true)
                    ReportType = "DateWise";
                else if (radExpDateWise.Checked == true)
                {

                    ReportType = "ConsolidateExp~" + litSegment.InnerText.Split('-')[1].Replace("'", "").Trim();
                }
                //else if (radVoucherWise.Checked == true)
                //    ReportType = "VoucherWise";
                //else
                ReportType = "VoucherWise";
            }
            else
            {
                ReportType = "Detail";
            }

            if (SubAccountSearch == "")
            {
                SubAccountSearch = " ";
            }
            string SingleDouble = null;
            if (chkBothPrint.Checked == true)
                SingleDouble = "D";
            else
                SingleDouble = "S";


            if (rbTanAll.Checked == true)
                strTranType = "all";
            else if (rbTranCashBank.Checked == true)
            {
                strTranType = "cb";
                if (chkPayment.Checked == true)
                    strCbPayment = "y";
                if (chkReceipts.Checked == true)
                    strCbReceipt = "y";
                if (chkContracts.Checked == true)
                    strCbContract = "y";

            }
            else if (rbTranJv.Checked == true)
            {
                strTranType = "jv";
                if (rbAllJV.Checked == true)
                    strJvType = "all";
                else if (rbManualJV.Checked == true)
                {
                    strJvType = "man";
                    strSelectedJv = txtVoucherPrefix.Text;
                }
                else if (rbSystemJV.Checked == true)
                {
                    strJvType = "sys";
                    //strSelectedJv = ViewState["Selectedjvs"].ToString();
                    if (rbSystemJVAll.Checked == true)
                        strSelectedJv = "all";
                    else if (rbSystemJVSelected.Checked == true)
                        strSelectedJv = hdnSystemJvs.Value;
                }
            }

            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            {
                //using (SqlDataAdapter da = new SqlDataAdapter("Fetch_LedgerFordosprint", con))
                //{
                //    da.SelectCommand.Parameters.AddWithValue("@CompanyID", Session["LastCompany"].ToString());
                //    da.SelectCommand.Parameters.AddWithValue("@FinYear", Session["LastFinYear"].ToString());
                //    da.SelectCommand.Parameters.AddWithValue("@FromDate", dtFrom.Value);
                //    da.SelectCommand.Parameters.AddWithValue("@ToDate", dtTo.Value);
                //    da.SelectCommand.Parameters.AddWithValue("@MainAccount", mainAccountSearch);
                //    da.SelectCommand.Parameters.AddWithValue("@SubAccount", SubAccountSearch);
                //    da.SelectCommand.Parameters.AddWithValue("@Branch", Branch);
                //    da.SelectCommand.Parameters.AddWithValue("@ReportType", ReportType);
                //    da.SelectCommand.Parameters.AddWithValue("@Segment", Segment);
                //    da.SelectCommand.Parameters.AddWithValue("@UserID", Session["userid"].ToString());
                //    da.SelectCommand.Parameters.AddWithValue("@SubledgerType", SubLedgerType.Trim());
                //    da.SelectCommand.Parameters.AddWithValue("@Settlement", Settlement);
                //    da.SelectCommand.Parameters.AddWithValue("@TranType", strTranType);
                //    da.SelectCommand.Parameters.AddWithValue("@CbPayment", Convert.ToChar(strCbPayment));
                //    da.SelectCommand.Parameters.AddWithValue("@CbReceipt", Convert.ToChar(strCbReceipt));
                //    da.SelectCommand.Parameters.AddWithValue("@CbContract", Convert.ToChar(strCbContract));
                //    da.SelectCommand.Parameters.AddWithValue("@JvType", strJvType);
                //    da.SelectCommand.Parameters.AddWithValue("@SelectedJv", strSelectedJv);

                //    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                //    da.SelectCommand.CommandTimeout = 0;

                //    if (con.State == ConnectionState.Closed)
                //        con.Open();
                //    ds.Reset();
                //    da.Fill(ds);
                //    //dt1 = ds.Tables[2].Copy();
                //    //dt1.Columns.Add
                //    //ds1.Tables.Add(dt1);
                //    //dt1 = ds1.Tables[0];
                //    dt1 = ds.Tables[2].Copy();
                //    ds1.Tables.Add(dt1);
                //    dt1 = null;
                //    dt1 = ds.Tables[3].Copy();
                //    ds1.Tables.Add(dt1);
                //    dt1 = null;
                //    dt1 = ds.Tables[4].Copy();
                //    ds1.Tables.Add(dt1);
                //    dt1 = null;
                //}
                //using (SqlDataAdapter da = new SqlDataAdapter("Fetch_LedgerFordosprintall", con))
                //{
                //    da.SelectCommand.Parameters.AddWithValue("@CompanyID", Session["LastCompany"].ToString());
                //    da.SelectCommand.Parameters.AddWithValue("@FinYear", Session["LastFinYear"].ToString());
                //    da.SelectCommand.Parameters.AddWithValue("@FromDate", dtFrom.Value);
                //    da.SelectCommand.Parameters.AddWithValue("@ToDate", dtTo.Value);
                //    da.SelectCommand.Parameters.AddWithValue("@MainAccount", mainAccountSearch);
                //    da.SelectCommand.Parameters.AddWithValue("@SubAccount", SubAccountSearch);
                //    da.SelectCommand.Parameters.AddWithValue("@Branch", Branch);
                //    da.SelectCommand.Parameters.AddWithValue("@ReportType", ReportType);
                //    da.SelectCommand.Parameters.AddWithValue("@Segment", Segment);
                //    da.SelectCommand.Parameters.AddWithValue("@UserID", Session["userid"].ToString());
                //    da.SelectCommand.Parameters.AddWithValue("@SubledgerType", SubLedgerType.Trim());
                //    da.SelectCommand.Parameters.AddWithValue("@Settlement", Settlement);
                //    da.SelectCommand.Parameters.AddWithValue("@TranType", strTranType);
                //    da.SelectCommand.Parameters.AddWithValue("@CbPayment", Convert.ToChar(strCbPayment));
                //    da.SelectCommand.Parameters.AddWithValue("@CbReceipt", Convert.ToChar(strCbReceipt));
                //    da.SelectCommand.Parameters.AddWithValue("@CbContract", Convert.ToChar(strCbContract));
                //    da.SelectCommand.Parameters.AddWithValue("@JvType", strJvType);
                //    da.SelectCommand.Parameters.AddWithValue("@SelectedJv", strSelectedJv);
                //    da.SelectCommand.Parameters.AddWithValue("@Header", txtHeader_hidden.Value);
                //    da.SelectCommand.Parameters.AddWithValue("@Footer", txtFooter_hidden.Value);

                //    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                //    da.SelectCommand.CommandTimeout = 0;

                //    if (con.State == ConnectionState.Closed)
                //        con.Open();
                //    ds.Reset();
                //    da.Fill(dsCrystal);
                //    GC.Collect();

                //}

                dsCrystal = objFAReportsOther.Fetch_LedgerFordosprintall(
                  Convert.ToString(Session["LastCompany"]),
                  Convert.ToString(Session["LastFinYear"]),
                   Convert.ToString(dtFrom.Value),
                   Convert.ToString(dtTo.Value),
                  Convert.ToString(mainAccountSearch),
                  Convert.ToString(SubAccountSearch),
                  Convert.ToString(Branch),
                  Convert.ToString(ReportType),
                  Convert.ToString(Segment),
                  Convert.ToString(Session["userid"]),
                  Convert.ToString(SubLedgerType.Trim()),
                  Convert.ToString(Settlement),
                  Convert.ToString(strTranType),
                  Convert.ToString(strCbPayment),
                  Convert.ToString(strCbReceipt),
                  Convert.ToString(strCbContract),
                  Convert.ToString(strJvType),
                  Convert.ToString(strSelectedJv),
                   Convert.ToString(txtHeader_hidden.Value),
                  Convert.ToString(txtFooter_hidden.Value));
            }
            if (dsCrystal.Tables[2].Rows.Count > 0)
            {
                if (radBreakDetail.Checked == true)
                {
                    Path = Server.MapPath(@"..\Reports\LedgerPrintdos.rpt");
                }
                else
                {
                    Path = Server.MapPath(@"..\Reports\LedgerPrintdetailconso.rpt");
                }

                //if (ds1.Tables[0].Rows.Count > 0)
                //{
                //    //oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds1.Tables[0].Rows[i25]["Closing"]));
                //    sData = null;
                //    string[] sArQuery = new string[1];
                //    sArQuery[0] = "";
                //    string PrintLoaction = null;
                //    if (ddlLocation.SelectedItem.Value.ToString().Trim() == "0")
                //    {
                //        PrintLoaction = ConfigurationManager.AppSettings["SaveCSVsql"];
                //    }
                //    else
                //    {
                //        PrintLoaction = ddlLocation.SelectedItem.Value.ToString().Trim();
                //    }
                //    string FileName = "Outputledger_" + Session["userid"].ToString() + "_" + Session["usersegid"].ToString() + "_" + oDBEngine.GetDate().ToString("ddMMyyyy") + "_" + oDBEngine.GetDate().ToString("hhmmss") + ".txt";

                //    //if (radBreakDetail.Checked == false)
                //    //{
                //    //    sXMLFileName = Server.MapPath("ReportFormat") + "\\ledger.xml";
                //    //    sOutputFileName = Server.MapPath("ReportOutput") + "\\" + FileName;
                //    //}
                //    //else

                //        sXMLFileName = Server.MapPath("ReportFormat") + "\\ledger1.xml";
                //        sOutputFileName = Server.MapPath("ReportOutput") + "\\" + FileName;

                //    StreamReader fp;
                //    fp = File.OpenText(sXMLFileName);
                //    sData = fp.ReadToEnd();
                //    fp.Close();

                //    //// Dim objPrint As New LibDosPrint.DosPrint(sData) ''xml file
                //    DosPrint objPrint = new DosPrint(sData, sArQuery);

                //    objPrint.PrintMainReport(sOutputFileName, ds1); //'' out put file


                //    //}
                //    string path = Path.Combine(PrintLoaction, FileName);


                //    File.Copy(sOutputFileName, path, true);
                //Response.End();

                //if (radBreakDetail.Checked == true)
                //{

                //    Path = Server.MapPath (@"..\Reports\LedgerPrintdos.rpt");

                //rpt.SetParameterValue("@PrintType", (string)SingleDouble);

                rpt.Load(Path);
                rpt.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
                //rpt.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.RichText, HttpContext.Current.Response, true, "ledger");
                rpt.SetDataSource(dsCrystal);
                string MainAcc = "";
                if (ddlAccountType.SelectedItem.Value == "0")
                {
                    MainAcc = "Clients - Trading A/c  ";
                }
                else if (ddlAccountType.SelectedItem.Value == "1")
                {
                    MainAcc = "Clients - Margin Deposit A/c  ";
                }
                else if (ddlAccountType.SelectedItem.Value == "2")
                {
                    MainAcc = "Clients - Trading A/c and Clients - Margin Deposit A/c  ";
                }
                else if (ddlAccountType.SelectedItem.Value == "3")
                {
                    MainAcc = txtMainAccount.Text;
                }
                if (rdbSegAll.Checked == true)
                {
                    rpt.SetParameterValue("@Segment", (string)"ALL");
                }
                else
                {
                    string dot = HDNSeg.Value.ToString().Trim();
                    if (dot.Contains("'"))
                    {
                        dot = dot.Replace("'", string.Empty);
                        dot.IndexOf("'");
                    }
                    rpt.SetParameterValue("@Segment", (string)dot.ToString().Trim());
                }

                //                string DefaultPrinterName= null; 
                //{ 
                //string printName= null;
                //PrinterSettings printerSetting = new PrinterSettings();
                //try
                //{
                //printName= printerSetting .PrinterName;
                //}
                //catch
                //{
                //printName= "";
                //}
                //finally
                //{
                //printerSetting = null;
                //} 

                rpt.SetParameterValue("@MainAccount", (string)MainAcc);
                rpt.SetParameterValue("@Period", (string)"From " + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + " To " + oconverter.ArrangeDate2(dtTo.Value.ToString()));
                rpt.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.WordForWindows, HttpContext.Current.Response, true, "ledger");
                //ExportOptions exportOpts = new ExportOptions();
                //PdfRtfWordFormatOptions pdfOpts = ExportOptions.CreatePdfRtfWordFormatOptions();

                //exportOpts.ExportFormatType = ExportFormatType.PortableDocFormat;
                //exportOpts.ExportFormatOptions = pdfOpts;
                //reportDocument.ExportToHttpResponse(exportOpts, Response, false, "");
                //rpt.PrintOptions.PrinterName = @"\\192.168.1.90\Epson";


                //rpt.PrintOptions.PrinterName = DropDownList1.SelectedItem.Value.ToString().Trim();
                // rpt.PrintOptions.PrinterName = "";
                //a1.PrintMode = CrystalDecisions.Web.PrintMode.Pdf;
                //a1.HasPrintButton = true;
                //a1.Visible = true;
                //a1.HasPageNavigationButtons = true;

                ////////////////////////////////////////////////rpt.PrintToPrinter(1, false, 0, 0);
                /////////////////////string tmpPdfPath;
                ///////////////////// tmpPdfPath = string.Empty;
                /////////////////////tmpPdfPath = HttpContext.Current.Server.MapPath(@"..\Documents\TempPdfLocation\");
                //////////////////////string abcd = tmpPdfPath  + "ledger.doc";
                /////////////////////rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.WordForWindows, abcd);
                ////string pdfPath = "E:\\Influx_New123\\Documents\\SIGNEDDOCS\\2012\\Feb\\NSE - CM-08042011-596-CLD0000072.pdf";// Server.MapPath("E:\\Influx_New123\\Documents\\SIGNEDDOCS\\2012\\Feb\\NSE - CM-08042011-596-CLD0000072.pdf");
                ////////string pdfPath = (abcd);
                ////////WebClient client = new WebClient();
                ////////Byte[] buffer = client.DownloadData(pdfPath);
                ////////Response.ContentType = "application/pdf";
                ////////Response.AddHeader("content-length", buffer.Length.ToString());
                ////////Response.BinaryWrite(buffer);
                //string filepath = Server.MapPath("test.doc");

                // Create New instance of FileInfo class to get the properties of the file being downloaded
                ///////////////////Response.Redirect(@"..\Reports\ledgerpopup.aspx\");
                ////////////////////////Page.ClientScript.RegisterStartupScript(GetType(), "JScript4567", "<script language='javascript'>window.open('ledgerpopup.aspx');</script>");
                //FileInfo file = new FileInfo(abcd);



                //// Checking if file exists
                //if (file.Exists)
                //{
                //    // Clear the content of the response
                //    Response.ClearContent();

                //    // LINE1: Add the file name and attachment, which will force the open/cance/save dialog to show, to the header
                //    ////Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);

                //    // Add the file size into the response header
                //    Response.AddHeader("Content-Length", file.Length.ToString());

                //    // Set the ContentType
                //    Response.ContentType = "application/pdf";

                //    // Write the file into the response (TransmitFile is for ASP.NET 2.0. In ASP.NET 1.1 you have to use WriteFile instead)
                //    Response.TransmitFile(file.FullName);

                //    // End the response

                //    Response.End();
                //}

                //string rptName = "..\\Reports\\LedgerPrintdos.rpt";
                //Response.Redirect("E:\\Influx_New123\\Documents\\SIGNEDDOCS\\2012\\Feb\\NSE - CM-08042011-596-CLD0000072.pdf");
                //rpt.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, rptName);
                //DropDownList1.DataSource = System.Drawing.Printing.PrinterSettings.InstalledPrinters;
                //DropDownList1.DataBind();
                //return printName;
                // }

                //a1.PrintMode = CrystalDecisions.Web.PrintMode.ActiveX;
                //a1.HasPrintButton = true;
                //a1.HasViewList = false;
                //a1.HasGotoPageButton = false;
                //a1.HasExportButton = false;
                //a1.HasPageNavigationButtons = true;
                //a1.HasSearchButton = false;
                //a1.HasZoomFactorList = false;
                //a1.HasRefreshButton = false;
                //a1.HasDrillUpButton = false;
                //a1.HasToggleGroupTreeButton = false;
                //a1.DisplayGroupTree = false;
                //a1.SeparatePages = false;
                //a1.Visible = true;

                Page.ClientScript.RegisterStartupScript(GetType(), "JScript12", "<script language='javascript'>Page_Load();</script>");
                //Page.ClientScript.RegisterStartupScript(GetType(), "JScript14", "<script language='javascript'>alert('Print Send to Select Printer');</script>");
            }
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct7", "Page_Load()", true);
            //}
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript18", "<script language='javascript'>Page_Load();</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript15", "<script language='javascript'>alert('No Record Found');</script>");
            }
        }

        #endregion




        //public void FnDosPrint()
        //{
        //    ddlLocation.Items.Clear();
        //    DataTable DtDosPrint = oDBEngine.GetDataTable("Config_DosPrinter", "distinct DosPrinter_Name+'['+DosPrinter_Location+']' as DosPrintName,DosPrinter_Location", "DosPrinter_User='" + HttpContext.Current.Session["userid"].ToString() + "'");
        //    if (DtDosPrint.Rows.Count > 0)
        //    {
        //        ddlLocation.DataSource = DtDosPrint;
        //        ddlLocation.DataTextField = "DosPrintName";
        //        ddlLocation.DataValueField = "DosPrinter_Location";
        //        ddlLocation.DataBind();
        //        ddlLocation.Items.Insert(0, new ListItem("Select DosPrint Location", "0"));
        //        DtDosPrint.Dispose();

        //    }
        //    else
        //    {
        //        ddlLocation.Items.Insert(0, new ListItem("Select DosPrint Location", "0"));
        //    }

        //}

        //    public void exportReport(CrystalDecisions.CrystalReports.Engine.ReportClass selectedReport, CrystalDecisions.Shared.ExportFormatType eft)
        //{
        //    selectedReport.ExportOptions.ExportFormatType = eft;

        //    string contentType ="";
        //    // Make sure asp.net has create and delete permissions in the directory
        //    string tempDir = System.Configuration.ConfigurationSettings.AppSettings["TempDir"];
        //    string tempFileName = Session.SessionID.ToString() + ".";
        //    switch (eft)
        //    {
        //    case CrystalDecisions.Shared.ExportFormatType.PortableDocFormat : 
        //        tempFileName += "pdf";
        //        contentType = "application/pdf";
        //        break;
        //    case CrystalDecisions.Shared.ExportFormatType.WordForWindows : 
        //        tempFileName+= "doc";
        //        contentType = "application/msword";
        //        break;
        //    case CrystalDecisions.Shared.ExportFormatType.Excel : 
        //        tempFileName+= "xls";
        //        contentType = "application/vnd.ms-excel";
        //        break;
        //    case CrystalDecisions.Shared.ExportFormatType.HTML32 : 
        //    case CrystalDecisions.Shared.ExportFormatType.HTML40 : 
        //        tempFileName+= "htm";
        //        contentType = "text/html";
        //        CrystalDecisions.Shared.HTMLFormatOptions hop = new CrystalDecisions.Shared.HTMLFormatOptions();
        //        hop.HTMLBaseFolderName = tempDir;
        //        hop.HTMLFileName = tempFileName;
        //        selectedReport.ExportOptions.FormatOptions = hop;
        //        break;
        //    }

        //    CrystalDecisions.Shared.DiskFileDestinationOptions dfo = new CrystalDecisions.Shared.DiskFileDestinationOptions();
        //    dfo.DiskFileName = tempDir + tempFileName;
        //    selectedReport.ExportOptions.DestinationOptions = dfo;
        //    selectedReport.ExportOptions.ExportDestinationType = CrystalDecisions.Shared.ExportDestinationType.DiskFile;

        //    selectedReport.Export();
        //    selectedReport.Close();

        //    string tempFileNameUsed;
        //    if (eft == CrystalDecisions.Shared.ExportFormatType.HTML32 || eft == CrystalDecisions.Shared.ExportFormatType.HTML40)
        //    {
        //        string[] fp = selectedReport.FilePath.Split("\\".ToCharArray());
        //        string leafDir = fp[fp.Length-1];
        //        // strip .rpt extension
        //        leafDir = leafDir.Substring(0, leafDir.Length – 4);
        //        tempFileNameUsed = string.Format("{0}{1}\\{2}", tempDir, leafDir, tempFileName);
        //    }
        //    else
        //        tempFileNameUsed = tempDir + tempFileName;

        //    Response.ClearContent();
        //    Response.ClearHeaders();
        //    Response.ContentType = contentType;

        //    Response.WriteFile(tempFileNameUsed);
        //    Response.Flush();
        //    Response.Close();

        //    System.IO.File.Delete(tempFileNameUsed);
        //    }   
    }
}