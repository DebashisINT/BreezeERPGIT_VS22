using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;
using DevExpress.Web;
using EntityLayer.CommonELS;
using DevExpress.XtraPrinting;
using DevExpress.Export;
namespace ERP.OMS.Reports
{
    public partial class Reports_frmReport_IframeGeneralTrial : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        string data;
        int pageindex = 0;
        int pagecount = 0;
        int pageSize;
        int rowcount = 0;
        string CompanyID = null;
        string SegmentID = null;
        DataTable dtSubsidiary = new DataTable();
        DataTable dtPeriod = new DataTable();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        ExcelFile objExcel = new ExcelFile();
        static string Checking = null;
        DataTable dtAsonAssetLiability = new DataTable();

        frmReport_IframeGeneralTrialBL OfrmReport_IframeGeneralTrialBL = new frmReport_IframeGeneralTrialBL();


        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
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
            if (Session["userlastsegment"].ToString() == "5")
            {
                HDNAccInd.Value = "N";
            }
            else
            {
                HDNAccInd.Value = "Y";
            }
            if (!IsPostBack)
            {
                //Null both session value for grid
                //Debjyoti
                Session.Remove("RptGeneralTrialPeriod");
                Session.Remove("RptGeneralTrial");
                //End Here

                SegmentID = null;
                CompanyID = null;
                dtDate.EditFormatString = oconverter.GetDateFormat("Date");
                dtFrom.EditFormatString = oconverter.GetDateFormat("Date");
                dtTo.EditFormatString = oconverter.GetDateFormat("Date");

                SetDatteFinYear();
                // dtDate.Value = Convert.ToDateTime(DateTime.Today);
                string fDate = null;
                string tDate = null;
                fDate = Session["FinYearStart"].ToString();
                tDate = Session["FinYearEnd"].ToString();
                dtFrom.Value = Convert.ToDateTime(fDate);
                dtTo.Value = Convert.ToDateTime(tDate);
                DataTable DtSegComp = new DataTable();
                string SegMentName = null;
                //if (Session["userlastsegment"].ToString() == "5")
                //{
                //    //ViewState["SegMentName"] = SegMentName;
                //    Session["CompanyID"] = Session["LastCompany"].ToString();
                //    ViewState["SegmentID"] = "0";
                //    litSegment.InnerText = "Accounts";
                //    TrAccount.Visible = false;
                //}
                //else
                //{
                DataTable dtSeg = oDBEngine.GetDataTable("tbl_master_segment", "seg_name", " seg_id=" + Session["userlastsegment"].ToString() + "");
                if (dtSeg.Rows[0][0].ToString().EndsWith("CM"))
                    DtSegComp = oDBEngine.GetDataTable("(select exch_compId,exch_internalId,isnull((select top 1  exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Comp from tbl_master_companyExchange where exch_compid in (select ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment in(" + Session["userallsegmentnotonlyLast"].ToString() + "))) as D", "*", "Comp in(select seg_name from tbl_master_segment where seg_id in(" + Session["userallsegmentnotonlyLast"].ToString() + "))  and Comp like '%CM' and exch_compID='" + Session["LastCompany"].ToString() + "'");
                else
                    DtSegComp = oDBEngine.GetDataTable("(select exch_compId,exch_internalId,isnull((select top 1  exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Comp from tbl_master_companyExchange where exch_compid in (select ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment=" + Session["userlastsegment"].ToString() + ")) as D", "*", "Comp in(select seg_name from tbl_master_segment where seg_id=" + Session["userlastsegment"].ToString() + ") and  exch_compID='" + Session["LastCompany"].ToString() + "'");
                //if (dtSeg.Rows[0][0].ToString().EndsWith("CM"))
                //    DtSegComp = oDBEngine.GetDataTable("(select exch_compId,exch_internalId,isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Comp from tbl_master_companyExchange where exch_compid in (select ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment in(" + Session["userallsegmentnotonlyLast"].ToString() + "))) as D", "*", "Comp in(select seg_name from tbl_master_segment where seg_id in(" + Session["userallsegmentnotonlyLast"].ToString() + "))  and Comp like '%CM' and exch_compID='" + Session["LastCompany"].ToString() + "'");
                //else
                //    DtSegComp = oDBEngine.GetDataTable("(select exch_compId,exch_internalId,isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Comp from tbl_master_companyExchange where exch_compid in (select ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment=" + Session["userlastsegment"].ToString() + ")) as D", "*", "Comp in(select seg_name from tbl_master_segment where seg_id=" + Session["userlastsegment"].ToString() + ") and  exch_compID='" + Session["LastCompany"].ToString() + "'");
                if (DtSegComp.Rows.Count > 0)
                {
                    CompanyID = DtSegComp.Rows[0][0].ToString();
                    for (int i = 0; i < DtSegComp.Rows.Count; i++)
                    {
                        if (SegmentID == null)
                        {
                            SegmentID = DtSegComp.Rows[i][1].ToString();
                            SegMentName = DtSegComp.Rows[i][2].ToString();
                        }
                        else
                        {
                            SegmentID = SegmentID + "," + DtSegComp.Rows[i][1].ToString();
                            SegMentName = SegMentName + "," + DtSegComp.Rows[i][2].ToString();
                        }
                    }

                    ViewState["SegMentName"] = SegMentName;
                    Session["CompanyID"] = CompanyID;
                    ViewState["SegmentID"] = SegmentID;
                    litSegment.InnerText = SegMentName;
                }
                // }


                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");
                txtsubscriptionID.Attributes.Add("onkeyup", "ShowMainAccountName(this,'SearchMainAccountBranchSegment',event)");
                rdbMainAll.Attributes.Add("OnClick", "MainAll('all')");
                rdbMainSelected.Attributes.Add("OnClick", "MainAll('Selc')");
                radAsDate.Attributes.Add("OnClick", "DateAll('all')");
                radPeriod.Attributes.Add("OnClick", "DateAll('Selc')");
            }
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Reports/frmReport_IframeGeneralTrial.aspx");
            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            //___________-end here___//
        }


        protected void SetDatteFinYear()
        {
            DataTable dtFinYear = oDBEngine.GetDataTable("MASTER_FINYEAR ", " FINYEAR_ENDDATE ", " FINYEAR_CODE ='" + Session["LastFinYear"].ToString() + "'");
            DateTime EndDate = Convert.ToDateTime(dtFinYear.Rows[0][0].ToString());
            DateTime TodayDate = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
            if (EndDate < TodayDate)
                dtDate.Value = EndDate;
            else
                dtDate.Value = EndDate;

        }
        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            string id = eventArgument.ToString();
            string[] idlist = id.Split('~');
            string[] cl = idlist[1].Split(',');
            string str = "";
            string strShow = "";
            string str1 = "";
            for (int i = 0; i < cl.Length; i++)
            {
                string[] val = cl[i].Split(';');
                if (str == "")
                {
                    str = "'" + val[0] + "'";
                    strShow = val[0];
                    str1 = val[0] + ";" + val[1];

                }
                else
                {
                    str += ",'" + val[0] + "'";
                    strShow += "," + val[0];
                    str1 += "," + val[0] + ";" + val[1];
                }
            }
            if (idlist[0] == "Segment")
            {
                SegmentID = strShow;
                //SegmentShow=strShow;
                // data = "Segment~" + strShow;
                data = "Segment~" + str1;
            }

        }
        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {


            if (radPeriod.Checked == true)
            {

                string Spantext = " General Trial For Period :";// +"  ( From ) : " + oconverter.ArrangeDate2(dtDate.Value.ToString()) + "  ( To ) : " + oconverter.ArrangeDate2(dtToDate.Value.ToString()) + "";
                string SpanText1 = oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "  - " + oconverter.ArrangeDate2(dtTo.Value.ToString());
                ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct", "ShowHidePeriod('" + Spantext + "','" + SpanText1 + "')", true);


                Checking = "b";
                FillPeriodGrid();
                //dtSubsidiary = new DataTable();
                //grdGeneralTrial.DataSource = dtSubsidiary;
                //grdGeneralTrial.DataBind();
                //if (ViewState["dtPeriod"] != null)
                //    dtPeriod = (DataTable)ViewState["dtPeriod"];
                //if (dtPeriod.Rows.Count == 0)
                //{
                //    grdPeriod.DataSource = dtPeriod;
                //    grdPeriod.DataBind();
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct", "alert('No Record Found!!');", true);
                //}
                // ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct", "ShowHide('b')", true);
            }
            else
            {

                string Spantext = "General Trial As On Date :";// +"  ( From ) : " + oconverter.ArrangeDate2(dtDate.Value.ToString()) + "  ( To ) : " + oconverter.ArrangeDate2(dtToDate.Value.ToString()) + "";
                string SpanText1 = oconverter.ArrangeDate2(dtDate.Value.ToString());
                ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct", "ShowHideAsOn('" + Spantext + "','" + SpanText1 + "')", true);

                Checking = "a";
                FillGrid();
                dtPeriod = new DataTable();
                grdPeriod.DataSource = dtPeriod;
                grdPeriod.DataBind();
                if (ViewState["dtSubsidiary"] != null)
                    dtSubsidiary = (DataTable)ViewState["dtSubsidiary"];
                if (dtSubsidiary.Rows.Count == 0)
                {
                    grdGeneralTrial.DataSource = dtSubsidiary;
                    grdGeneralTrial.DataBind();
                    //Debjyoti Bind New Dev Express Grid
                    aspxGdGeneralTrial.DataSource = dtSubsidiary;
                    aspxGdGeneralTrial.DataBind();
                    Session["RptGeneralTrial"] = dtSubsidiary;

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct", "alert('No Record Found!!');", true);
                }
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct", "ShowHide('a')", true);
            }
        }
        public void FillGrid()
        {
            try
            {
                decimal SumAssetDr = 0;
                decimal SumAssetCr = 0;
                decimal SumLiabilityDr = 0;
                decimal SumLiabilityCr = 0;
                decimal SumIncomeDr = 0;
                decimal SumIncomeCr = 0;
                decimal SumExpenseDr = 0;
                decimal SumExpenseCr = 0;

                string ZeroBal = "";

                System.Globalization.NumberFormatInfo currencyFormat = new System.Globalization.CultureInfo("hi-IN").NumberFormat;
                currencyFormat.CurrencySymbol = "";
                currencyFormat.CurrencyNegativePattern = 2;
                SegmentID = hdnSegment.Value;
                //LastCompany CompanyID = Session["CompanyID"].ToString();
                //Debjyoti
                CompanyID = Session["LastCompany"].ToString();
                if (rdbMainAll.Checked == true)
                {
                    SegmentID = null;
                    DataTable dtSegment = oDBEngine.GetDataTable("tbl_master_companyExchange", "exch_internalId", "exch_compId='" + Session["CompanyID"].ToString() + "'");
                    if (dtSegment.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtSegment.Rows.Count; i++)
                        {
                            if (SegmentID == null)
                                SegmentID = dtSegment.Rows[i][0].ToString();
                            else
                                SegmentID += "," + dtSegment.Rows[i][0].ToString();
                        }
                        ViewState["SegmentID"] = SegmentID;
                    }
                    //if (Session["userlastsegment"].ToString() == "5")
                    //{
                    //    SegmentID += "0" + SegmentID;
                    //    ViewState["SegmentID"] = SegmentID;
                    //}
                }
                else
                {
                    if (Session["userlastsegment"].ToString() == "5")
                    {
                        DataTable DtSeg = new DataTable();
                        DtSeg = oDBEngine.GetDataTable("(select exch_compId,exch_internalId,isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Comp from tbl_master_companyExchange where exch_compid in (select ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment=" + Session["userlastsegment"].ToString() + ")) as D", "*", "Comp in(select seg_name from tbl_master_segment where seg_id=" + Session["userlastsegment"].ToString() + ") and  exch_compID='" + Session["LastCompany"].ToString() + "'");
                        SegmentID = DtSeg.Rows[0][1].ToString();

                    }
                    else
                    {
                        if (SegmentID == "")
                            // SegmentID = ViewState["SegmentID"].ToString();
                            //Debjyoti
                            SegmentID = Convert.ToString(Session["userlastsegment"]);
                        ViewState["SegmentID"] = SegmentID;
                    }
                }

                if (chkZero.Checked == true)
                {
                    ZeroBal = "Y";
                }
                else
                {
                    ZeroBal = "N";

                }


                pageSize = 20000;
                //dtSubsidiary = oDBEngine.GetDataTable("(Select (select mainaccount_name from master_mainaccount where mainaccount_accountcode=trans_accountsledger.accountsledger_mainaccountid) as accountsledger_mainaccountid,case when (sum(accountsledger_amountdr)-sum(accountsledger_amountcr))>0 then sum(accountsledger_amountdr)-sum(accountsledger_amountcr) else null end as AmountDR,case when (sum(accountsledger_amountdr)-sum(accountsledger_amountcr))<0 then ((-1)*(sum(accountsledger_amountdr)-sum(accountsledger_amountcr))) else null end as AmountCR,accountsledger_mainaccountid as ID,(select ltrim(rtrim(MainAccount_SubLedgerType)) from master_mainaccount where mainaccount_accountcode=trans_accountsledger.accountsledger_mainaccountid) as SubLedgerType,(select mainaccount_accountcode from master_mainaccount where mainaccount_accountcode=trans_accountsledger.accountsledger_mainaccountid) as MainAccID from Trans_AccountsLedger WHERE  accountsledger_mainaccountid is not null and accountsledger_exchangesegmentid in(" + SegmentID + ") and accountsledger_companyid='" + CompanyID + "' and AccountsLedger_TransactionDate<='" + dtDate.Value + "' group By  accountsledger_mainaccountid ) as D ", "accountsledger_mainaccountid,convert(varchar(50),cast(AmountDr as money),1) as AmountDr,convert(varchar(50),cast(AmountCr as money),1) as AmountCr,ID,SubLedgerType,isnull(sum(D.AmountDr),0) as DR,isnull(sum(D.AmountCr),0) as CR,MainAccID  ", null, "  D.accountsledger_mainaccountid,D.AmountDR,D.AmountCR,D.ID,D.SubLedgerType,D.MainAccID", " accountsledger_mainaccountid");
                //dtSubsidiary = oDBEngine.GetDataTable("(Select accountsledger_mainaccountid,MainAccID,cast(AmountDr as varchar(max)) as AmountDr,cast(AmountCr as varchar(max)) as AmountCr,ID,SubLedgerType,isnull(sum(D.AmountDr),0) as DR,isnull(sum(D.AmountCr),0) as CR,AccType   from (Select mainaccount_name as accountsledger_mainaccountid,case when (sum(accountsledger_amountdr)-sum(accountsledger_amountcr))>0 then sum(accountsledger_amountdr)-sum(accountsledger_amountcr) else null end as AmountDR,case when (sum(accountsledger_amountdr)-sum(accountsledger_amountcr))<0 then ((-1)*(sum(accountsledger_amountdr)-sum(accountsledger_amountcr))) else null end as AmountCR,accountsledger_mainaccountid as ID,ltrim(rtrim(MainAccount_SubLedgerType)) as SubLedgerType,mainaccount_accountcode as MainAccID,mainaccount_accounttype as AccType from Trans_AccountsLedger,master_mainaccount WHERE  accountsledger_mainaccountid is not null and accountsledger_exchangesegmentid in(" + SegmentID + ") and accountsledger_companyid='" + CompanyID + "' and AccountsLedger_TransactionDate<='" + dtDate.Value + "' and AccountsLedger_FinYear='" + Session["LastFinYear"].ToString() + "' and mainaccount_accountcode=accountsledger_mainaccountid group By  accountsledger_mainaccountid,mainaccount_name,mainaccount_accountcode,MainAccount_SubLedgerType,mainaccount_accounttype ) as D  group By   D.accountsledger_mainaccountid,D.AmountDR,D.AmountCR,D.ID,D.SubLedgerType,D.MainAccID,D.AccType ) as E ", "accountsledger_mainaccountid,MainAccID,AmountDr,AmountCr,ID,SubLedgerType,DR,CR,case when (DR=0 and AccType='Asset') then 'Liability' when  (CR=0 and AccType='Liability')  then 'Asset'	when (CR=0 and AccType='Asset') then 'Asset' when  (DR=0 and AccType='Liability')  then 'Liability'	when (CR=0 and AccType='Income') then 'Expenses' when (DR=0 and AccType='Expences') then 'Income' when (DR=0 and AccType='Income') then 'Income' else 'Expenses' end as AccType", " (E.DR<>0 or E.CR<>0)", " AccType,accountsledger_mainaccountid");
                //dtSubsidiary = oDBEngine.GetDataTable("(Select accountsledger_mainaccountid,MainAccID,cast(AmountDr as varchar(max)) as AmountDr,cast(AmountCr as varchar(max)) as AmountCr,ID,SubLedgerType,isnull(sum(D.AmountDr),0) as DR,isnull(sum(D.AmountCr),0) as CR,AccType,CashBankType    from (Select mainaccount_name as accountsledger_mainaccountid,case when (sum(accountsledger_amountdr)-sum(accountsledger_amountcr))>0 then sum(accountsledger_amountdr)-sum(accountsledger_amountcr) else null end as AmountDR,case when (sum(accountsledger_amountdr)-sum(accountsledger_amountcr))<0 then ((-1)*(sum(accountsledger_amountdr)-sum(accountsledger_amountcr))) else null end as AmountCR,accountsledger_mainaccountid as ID,ltrim(rtrim(MainAccount_SubLedgerType)) as SubLedgerType,ltrim(rtrim(mainaccount_bankcashtype)) as CashBankType,mainaccount_accountcode as MainAccID,mainaccount_accounttype as AccType from Trans_AccountsLedger,master_mainaccount WHERE  accountsledger_mainaccountid is not null and accountsledger_exchangesegmentid in(" + SegmentID + ") and accountsledger_companyid='" + CompanyID + "' and AccountsLedger_TransactionDate<='" + dtDate.Value + "' and AccountsLedger_FinYear='" + Session["LastFinYear"].ToString() + "' and mainaccount_accountcode=accountsledger_mainaccountid group By  accountsledger_mainaccountid,mainaccount_name,mainaccount_accountcode,MainAccount_SubLedgerType,mainaccount_accounttype,mainaccount_bankcashtype  ) as D  group By   D.accountsledger_mainaccountid,D.AmountDR,D.AmountCR,D.ID,D.SubLedgerType,D.MainAccID,D.AccType,D.CashBankType  ) as E ", "accountsledger_mainaccountid,MainAccID,AmountDr,AmountCr,ID,SubLedgerType,DR,CR,case when (DR=0 and AccType='Asset') then 'Liability' when  (CR=0 and AccType='Liability')  then 'Asset'	when (CR=0 and AccType='Asset') then 'Asset' when  (DR=0 and AccType='Liability')  then 'Liability'	when (CR=0 and AccType='Income') then 'Expenses' when (DR=0 and AccType='Expenses') then 'Income' when (DR=0 and AccType='Income') then 'Income' else 'Expenses' end as AccType,CashBankType", " (E.DR<>0 or E.CR<>0)", " AccType,accountsledger_mainaccountid");

                DataSet ds = new DataSet();
                /* For Tier Structure
                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter("Fetch_GeneralTrialAsOnDate", con))
                    {
                        da.SelectCommand.Parameters.AddWithValue("@ToDate", dtDate.Value);
                        da.SelectCommand.Parameters.AddWithValue("@Segment", SegmentID);
                        da.SelectCommand.Parameters.AddWithValue("@Company", CompanyID);
                        da.SelectCommand.Parameters.AddWithValue("@FinancialYr", Session["LastFinYear"].ToString());
                        da.SelectCommand.Parameters.AddWithValue("@ZeroBal", ZeroBal);
                        ///Currency Setting
                        da.SelectCommand.Parameters.AddWithValue("@ActiveCurrency", Session["ActiveCurrency"].ToString().Split('~')[0]);
                        da.SelectCommand.Parameters.AddWithValue("@TradeCurrency", Session["TradeCurrency"].ToString().Split('~')[0]);
                        da.SelectCommand.CommandType = CommandType.StoredProcedure;
                        da.SelectCommand.CommandTimeout = 0;

                 
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        ds.Reset();
                        da.Fill(ds);
                        */


                string strbranchList = Convert.ToString(branchList.Value);

                if (strbranchList == "")
                {
                    string[] branhArray = oDBEngine.GetFieldValue1("tbl_master_branch FOR XML PATH('')),1,1,'')", "STUFF((Select ','+Convert(varchar(10),branch_id)", "1=1", 1);
                  strbranchList   =branhArray[0];
                }


                ds = OfrmReport_IframeGeneralTrialBL.Fetch_GeneralTrialAsOnDate(dtDate.Value.ToString(), SegmentID, CompanyID, Session["LastFinYear"].ToString(), ZeroBal,
                   Session["ActiveCurrency"].ToString().Split('~')[0], Session["TradeCurrency"].ToString().Split('~')[0], strbranchList);

                dtSubsidiary = ds.Tables[0];
                //   ViewState["dataset"] = ds;

                //    }
                //}
                if (dtSubsidiary.Rows.Count > 0)
                {
                    decimal SumDr = Convert.ToDecimal(dtSubsidiary.Compute("sum(DR)", ""));
                    decimal SumCr = Convert.ToDecimal(dtSubsidiary.Compute("sum(CR)", ""));
                    ViewState["Diff"] = Convert.ToString(SumDr - SumCr);

                    ViewState["SumDr"] = SumDr.ToString();
                    ViewState["SumCr"] = SumCr.ToString();

                    pagecount = dtSubsidiary.Rows.Count / pageSize + 1;
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
                    grdGeneralTrial.PageIndex = pageindex;
                    CurrentPage.Value = pageindex.ToString();
                    rowcount = 0;

                    DataTable dtAsset = dtSubsidiary.Clone();
                    DataTable dtIncome = dtSubsidiary.Clone();
                    DataTable dtLiability = dtSubsidiary.Clone();
                    DataTable dtExpenditure = dtSubsidiary.Clone();



                    DataView foundRow = new DataView(dtSubsidiary);
                    foundRow.RowFilter = "AccType='Asset'";
                    foreach (DataRowView dvr in foundRow)
                    {
                        dtAsset.ImportRow(dvr.Row);
                    }
                    dtAsset.AcceptChanges();

                    DataView foundRow1 = new DataView(dtSubsidiary);
                    foundRow1.RowFilter = "AccType='Liability'";
                    foreach (DataRowView dvr in foundRow1)
                    {
                        dtLiability.ImportRow(dvr.Row);
                    }
                    dtLiability.AcceptChanges();

                    DataView foundRow2 = new DataView(dtSubsidiary);
                    foundRow2.RowFilter = "AccType='Income'";
                    foreach (DataRowView dvr in foundRow2)
                    {
                        dtIncome.ImportRow(dvr.Row);
                    }
                    dtIncome.AcceptChanges();

                    DataView foundRow3 = new DataView(dtSubsidiary);
                    foundRow3.RowFilter = "AccType='Expense'";
                    foreach (DataRowView dvr in foundRow3)
                    {
                        dtExpenditure.ImportRow(dvr.Row);
                    }
                    dtExpenditure.AcceptChanges();

                    foundRow.Dispose();
                    foundRow1.Dispose();
                    foundRow2.Dispose();
                    foundRow3.Dispose();

                    if (dtAsset.Rows.Count > 0)
                    {
                        SumAssetDr = Convert.ToDecimal(dtAsset.Compute("sum(DR)", ""));
                        SumAssetCr = Convert.ToDecimal(dtAsset.Compute("sum(CR)", ""));
                    }
                    if (dtLiability.Rows.Count > 0)
                    {
                        SumLiabilityDr = Convert.ToDecimal(dtLiability.Compute("sum(DR)", ""));
                        SumLiabilityCr = Convert.ToDecimal(dtLiability.Compute("sum(CR)", ""));
                    }
                    if (dtIncome.Rows.Count > 0)
                    {
                        SumIncomeDr = Convert.ToDecimal(dtIncome.Compute("sum(DR)", ""));
                        SumIncomeCr = Convert.ToDecimal(dtIncome.Compute("sum(CR)", ""));
                    }
                    if (dtExpenditure.Rows.Count > 0)
                    {
                        SumExpenseDr = Convert.ToDecimal(dtExpenditure.Compute("sum(DR)", ""));
                        SumExpenseCr = Convert.ToDecimal(dtExpenditure.Compute("sum(CR)", ""));
                    }

                    dtAsonAssetLiability = dtSubsidiary.Clone();

                    DataColumn dcolColumn = new DataColumn("Debit", typeof(string));
                    dtAsonAssetLiability.Columns.Add(dcolColumn);
                    DataColumn dcolColumn1 = new DataColumn("Credit", typeof(string));
                    dtAsonAssetLiability.Columns.Add(dcolColumn1);



                    #region Liability
                    if (dtLiability.Rows.Count > 0)
                    {
                        for (int k = 0; k < dtLiability.Rows.Count; k++)
                        {
                            if (k == 0)
                            {
                                DataRow DrNew1 = dtAsonAssetLiability.NewRow();
                                DrNew1[0] = "Liabilities";
                                DrNew1[5] = "None";
                                dtAsonAssetLiability.Rows.Add(DrNew1);
                            }
                            DataRow DrNew = dtAsonAssetLiability.NewRow();
                            DrNew[0] = dtLiability.Rows[k][0].ToString();
                            DrNew[1] = dtLiability.Rows[k][1].ToString();
                            if (dtLiability.Rows[k][2] != DBNull.Value)
                            {
                                DrNew[2] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtLiability.Rows[k]["AmountDr"].ToString()));
                                DrNew[9] = oconverter.formatmoneyinUs(Convert.ToDecimal(dtLiability.Rows[k]["AmountDr"].ToString()));
                            }
                            else
                            {
                                DrNew[2] = "";
                                DrNew[9] = "";
                            }
                            if (dtLiability.Rows[k][3] != DBNull.Value)
                            {
                                DrNew[3] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtLiability.Rows[k]["AmountCr"].ToString()));
                                DrNew[10] = oconverter.formatmoneyinUs(Convert.ToDecimal(dtLiability.Rows[k]["AmountCr"].ToString()));
                            }
                            else
                            {
                                DrNew[3] = "";
                                DrNew[10] = "";
                            }

                            DrNew[4] = dtLiability.Rows[k][4].ToString();
                            DrNew[5] = dtLiability.Rows[k][5].ToString();
                            DrNew[6] = dtLiability.Rows[k][6].ToString();
                            DrNew[7] = dtLiability.Rows[k][7].ToString();
                            DrNew[8] = dtLiability.Rows[k][8].ToString();
                            DrNew[9] = dtLiability.Rows[k][9].ToString();
                            dtAsonAssetLiability.Rows.Add(DrNew);
                            if (k == dtLiability.Rows.Count - 1)
                            {
                                DataRow DrNew3 = dtAsonAssetLiability.NewRow();
                                DrNew3[5] = "None";
                                dtAsonAssetLiability.Rows.Add(DrNew3);
                                DataRow DrNew2 = dtAsonAssetLiability.NewRow();
                                DrNew2[0] = "Total";
                                DrNew2[2] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(SumLiabilityDr));
                                DrNew2[3] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(SumLiabilityCr));
                                DrNew2[9] = oconverter.formatmoneyinUs(Convert.ToDecimal(SumLiabilityDr));
                                DrNew2[10] = oconverter.formatmoneyinUs(Convert.ToDecimal(SumLiabilityCr));
                                DrNew2[5] = "None";
                                dtAsonAssetLiability.Rows.Add(DrNew2);
                            }

                        }
                    }
                    #endregion
                    #region Asset
                    if (dtAsset.Rows.Count > 0)
                    {
                        for (int Asst = 0; Asst < dtAsset.Rows.Count; Asst++)
                        {
                            if (Asst == 0)
                            {
                                DataRow DrNew1 = dtAsonAssetLiability.NewRow();
                                DrNew1[0] = "Assets";
                                DrNew1[5] = "None";
                                dtAsonAssetLiability.Rows.Add(DrNew1);
                            }
                            DataRow DrNew = dtAsonAssetLiability.NewRow();
                            DrNew[0] = dtAsset.Rows[Asst][0].ToString();
                            DrNew[1] = dtAsset.Rows[Asst][1].ToString();
                            if (dtAsset.Rows[Asst][2] != DBNull.Value)
                            {
                                DrNew[2] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtAsset.Rows[Asst]["AmountDr"].ToString()));
                                DrNew[9] = oconverter.formatmoneyinUs(Convert.ToDecimal(dtAsset.Rows[Asst]["AmountDr"].ToString()));
                            }
                            else
                            {
                                DrNew[2] = "";
                                DrNew[9] = "";
                            }
                            if (dtAsset.Rows[Asst][3] != DBNull.Value)
                            {
                                DrNew[3] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtAsset.Rows[Asst]["AmountCr"].ToString()));
                                DrNew[10] = oconverter.formatmoneyinUs(Convert.ToDecimal(dtAsset.Rows[Asst]["AmountCr"].ToString()));
                            }
                            else
                            {
                                DrNew[3] = "";
                                DrNew[10] = "";
                            }
                            DrNew[4] = dtAsset.Rows[Asst][4].ToString();
                            DrNew[5] = dtAsset.Rows[Asst][5].ToString();
                            DrNew[6] = dtAsset.Rows[Asst][6].ToString();
                            DrNew[7] = dtAsset.Rows[Asst][7].ToString();
                            DrNew[8] = dtAsset.Rows[Asst][8].ToString();
                            DrNew[9] = dtAsset.Rows[Asst][9].ToString();
                            dtAsonAssetLiability.Rows.Add(DrNew);
                            if (Asst == dtAsset.Rows.Count - 1)
                            {
                                DataRow DrNew3 = dtAsonAssetLiability.NewRow();
                                DrNew3[5] = "None";
                                dtAsonAssetLiability.Rows.Add(DrNew3);
                                DataRow DrNew2 = dtAsonAssetLiability.NewRow();
                                DrNew2[0] = "Total";
                                DrNew2[2] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(SumAssetDr));
                                DrNew2[3] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(SumAssetCr));
                                DrNew2[9] = oconverter.formatmoneyinUs(Convert.ToDecimal(SumAssetDr));
                                DrNew2[10] = oconverter.formatmoneyinUs(Convert.ToDecimal(SumAssetCr));
                                DrNew2[5] = "None";
                                dtAsonAssetLiability.Rows.Add(DrNew2);
                            }

                        }
                    }
                    #endregion
                    #region Income
                    if (dtIncome.Rows.Count > 0)
                    {
                        for (int k = 0; k < dtIncome.Rows.Count; k++)
                        {
                            if (k == 0)
                            {
                                DataRow DrNew1 = dtAsonAssetLiability.NewRow();
                                DrNew1[0] = "Income";
                                DrNew1[5] = "None";
                                dtAsonAssetLiability.Rows.Add(DrNew1);
                            }
                            DataRow DrNew = dtAsonAssetLiability.NewRow();
                            DrNew[0] = dtIncome.Rows[k][0].ToString();
                            DrNew[1] = dtIncome.Rows[k][1].ToString();
                            if (dtIncome.Rows[k][2] != DBNull.Value)
                            {
                                DrNew[2] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtIncome.Rows[k]["AmountDr"].ToString()));
                                DrNew[9] = oconverter.formatmoneyinUs(Convert.ToDecimal(dtIncome.Rows[k]["AmountDr"].ToString()));
                            }
                            else
                            {
                                DrNew[2] = "";
                                DrNew[9] = "";
                            }
                            if (dtIncome.Rows[k][3] != DBNull.Value)
                            {
                                DrNew[3] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtIncome.Rows[k]["AmountCr"].ToString()));
                                DrNew[10] = oconverter.formatmoneyinUs(Convert.ToDecimal(dtIncome.Rows[k]["AmountCr"].ToString()));
                            }
                            else
                            {
                                DrNew[3] = "";
                                DrNew[10] = "";
                            }

                            DrNew[4] = dtIncome.Rows[k][4].ToString();
                            DrNew[5] = dtIncome.Rows[k][5].ToString();
                            DrNew[6] = dtIncome.Rows[k][6].ToString();
                            DrNew[7] = dtIncome.Rows[k][7].ToString();
                            DrNew[8] = dtIncome.Rows[k][8].ToString();
                            DrNew[9] = dtIncome.Rows[k][9].ToString();
                            dtAsonAssetLiability.Rows.Add(DrNew);
                            if (k == dtIncome.Rows.Count - 1)
                            {
                                DataRow DrNew3 = dtAsonAssetLiability.NewRow();
                                DrNew3[5] = "None";
                                dtAsonAssetLiability.Rows.Add(DrNew3);
                                DataRow DrNew2 = dtAsonAssetLiability.NewRow();
                                DrNew2[0] = "Total";
                                DrNew2[2] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(SumIncomeDr));
                                DrNew2[3] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(SumIncomeCr));
                                DrNew2[9] = oconverter.formatmoneyinUs(Convert.ToDecimal(SumIncomeDr));
                                DrNew2[10] = oconverter.formatmoneyinUs(Convert.ToDecimal(SumIncomeCr));
                                DrNew2[5] = "None";
                                dtAsonAssetLiability.Rows.Add(DrNew2);
                            }

                        }
                    }
                    #endregion
                    #region Expenditure
                    if (dtExpenditure.Rows.Count > 0)
                    {
                        for (int k = 0; k < dtExpenditure.Rows.Count; k++)
                        {
                            if (k == 0)
                            {
                                DataRow DrNew1 = dtAsonAssetLiability.NewRow();
                                DrNew1[0] = "Expenditure";
                                DrNew1[5] = "None";
                                dtAsonAssetLiability.Rows.Add(DrNew1);
                            }
                            DataRow DrNew = dtAsonAssetLiability.NewRow();
                            DrNew[0] = dtExpenditure.Rows[k][0].ToString();
                            DrNew[1] = dtExpenditure.Rows[k][1].ToString();
                            if (dtExpenditure.Rows[k][2] != DBNull.Value)
                            {
                                DrNew[2] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtExpenditure.Rows[k]["AmountDr"].ToString()));
                                DrNew[9] = oconverter.formatmoneyinUs(Convert.ToDecimal(dtExpenditure.Rows[k]["AmountDr"].ToString()));
                            }
                            else
                            {
                                DrNew[2] = "";
                                DrNew[9] = "";
                            }
                            if (dtExpenditure.Rows[k][3] != DBNull.Value)
                            {
                                DrNew[3] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtExpenditure.Rows[k]["AmountCr"].ToString()));
                                DrNew[10] = oconverter.formatmoneyinUs(Convert.ToDecimal(dtExpenditure.Rows[k]["AmountCr"].ToString()));
                            }
                            else
                            {
                                DrNew[3] = "";
                                DrNew[10] = "";
                            }

                            DrNew[4] = dtExpenditure.Rows[k][4].ToString();
                            DrNew[5] = dtExpenditure.Rows[k][5].ToString();
                            DrNew[6] = dtExpenditure.Rows[k][6].ToString();
                            DrNew[7] = dtExpenditure.Rows[k][7].ToString();
                            DrNew[8] = dtExpenditure.Rows[k][8].ToString();
                            DrNew[9] = dtExpenditure.Rows[k][9].ToString();
                            dtAsonAssetLiability.Rows.Add(DrNew);
                            if (k == dtExpenditure.Rows.Count - 1)
                            {
                                DataRow DrNew3 = dtAsonAssetLiability.NewRow();
                                DrNew3[5] = "None";
                                dtAsonAssetLiability.Rows.Add(DrNew3);
                                DataRow DrNew2 = dtAsonAssetLiability.NewRow();
                                DrNew2[0] = "Total";
                                DrNew2[2] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(SumExpenseDr));
                                DrNew2[3] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(SumExpenseCr));
                                DrNew2[9] = oconverter.formatmoneyinUs(Convert.ToDecimal(SumExpenseDr));
                                DrNew2[10] = oconverter.formatmoneyinUs(Convert.ToDecimal(SumExpenseCr));
                                DrNew2[5] = "None";
                                dtAsonAssetLiability.Rows.Add(DrNew2);
                            }

                        }
                    }
                    #endregion

                    ViewState["dtAsonAssetLiability"] = dtAsonAssetLiability;

                    grdGeneralTrial.DataSource = dtAsonAssetLiability;
                    grdGeneralTrial.DataBind();
                   

                    ViewState["dtSubsidiary"] = dtSubsidiary;
                    if (pagecount == 1)
                    {
                        grdGeneralTrial.BottomPagerRow.Visible = true;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "hide3", "DisableFirst();", true);
                    }
                   

                    DataRow genTrialRow = dtAsonAssetLiability.NewRow();

                     grdGeneralTrial.FooterRow.Cells[0].Text = "Total";
                     genTrialRow["accountsledger_mainaccountid"] = "Total";

                    if (SumDr == 0)
                        grdGeneralTrial.FooterRow.Cells[3].Text = "";
                    else
                    {
                        grdGeneralTrial.FooterRow.Cells[3].Text = SumDr.ToString("c", currencyFormat);
                        genTrialRow["AmountDr"] = SumDr.ToString("c", currencyFormat);
                    }
                    if (SumCr == 0)
                        grdGeneralTrial.FooterRow.Cells[4].Text = "";
                    else
                    {
                        grdGeneralTrial.FooterRow.Cells[4].Text = SumCr.ToString("c", currencyFormat);
                        genTrialRow["AmountCr"] = SumCr.ToString("c", currencyFormat);
                    }
                    grdGeneralTrial.FooterRow.Cells[0].HorizontalAlign = HorizontalAlign.Left;
                    grdGeneralTrial.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Right;
                    grdGeneralTrial.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Right;
                    grdGeneralTrial.FooterRow.Cells[0].ForeColor = System.Drawing.Color.White;
                    grdGeneralTrial.FooterRow.Cells[3].ForeColor = System.Drawing.Color.White;
                    grdGeneralTrial.FooterRow.Cells[4].ForeColor = System.Drawing.Color.White;
                    grdGeneralTrial.FooterRow.Cells[0].Font.Bold = true;
                    grdGeneralTrial.FooterRow.Cells[3].Font.Bold = true;
                    grdGeneralTrial.FooterRow.Cells[4].Font.Bold = true;
                    grdGeneralTrial.FooterRow.Cells[3].Wrap = false;
                    grdGeneralTrial.FooterRow.Cells[4].Wrap = false;


                    //Debjyoti Bind New Dev Express Grid
                    dtAsonAssetLiability.Rows.Add(genTrialRow);
                    aspxGdGeneralTrial.DataSource = dtAsonAssetLiability;
                    aspxGdGeneralTrial.DataBind();
                    Session["RptGeneralTrial"] = dtAsonAssetLiability;




                }
            }
            catch
            {
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
        }
        protected void grdSubsidiaryTrial_RowCreated(object sender, GridViewRowEventArgs e)
        {
            string rowID = String.Empty;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (ViewState["dtSubsidiary"] != null)
                    dtSubsidiary = (DataTable)ViewState["dtSubsidiary"];
                rowID = "row" + e.Row.RowIndex;
                e.Row.Attributes.Add("id", "row" + e.Row.RowIndex);
                e.Row.Attributes.Add("onclick", "ChangeRowColor(" + "'" + rowID + "','" + dtSubsidiary.Rows.Count + "'" + ")");
                //e.Row.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
                //e.Row.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
                //e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.grdGeneralTrial, "Select$" + e.Row.RowIndex);
            }

        }
        protected void grdGeneralTrial_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            string ZeroBal = "";
            if (chkZero.Checked == true)
            {
                ZeroBal = "Y";
            }
            else
            {
                ZeroBal = "N";

            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label MainAcc = (Label)e.Row.FindControl("lblMainAcc");
                Label SubLedgerType = (Label)e.Row.FindControl("lblSubLedgerType");
                Label CashBankType = (Label)e.Row.FindControl("lblCashBankType");
                Label Main = (Label)e.Row.FindControl("lblMainAccount");
                DateTime date = Convert.ToDateTime(dtDate.Value);
                DateTime Tdate = Convert.ToDateTime(dtTo.Value);
                if (SubLedgerType.Text != "None")
                {
                    //((Label)e.Row.FindControl("lblMainAccount")).Attributes.Add("onclick", "javascript:ShowGeneralTrialDetail('" + MainAcc.Text + "','" + date + "');");
                    e.Row.Cells[2].Style.Add("cursor", "hand");
                    e.Row.Cells[2].Attributes.Add("onclick", "javascript:ShowGeneralTrialDetail('" + MainAcc.Text + "','" + date + "','" + Main.Text + "','" + ViewState["SegmentID"].ToString() + "','" + date + "','A','" + SubLedgerType.Text + "','" + ZeroBal + "');");
                    e.Row.Cells[2].Text = "View";
                    e.Row.Cells[2].Style.Add("color", "blue");
                }
                if (SubLedgerType.Text == "None")
                {
                    if (CashBankType.Text == "Bank" || CashBankType.Text == "Cash")
                    {
                        e.Row.Cells[0].Style.Add("cursor", "hand");
                        e.Row.Cells[0].Attributes.Add("onclick", "javascript:ShowCashBankDetail('" + MainAcc.Text + "','" + date + "','" + Main.Text + "','" + ViewState["SegmentID"].ToString() + "','" + date + "','A','" + SubLedgerType.Text + "');");
                        //e.Row.Cells[0].Text = "View";
                        e.Row.Cells[0].ToolTip = "Click To View CashBank Details";
                    }
                    else
                    {
                        e.Row.Cells[0].Style.Add("cursor", "hand");
                        //  e.Row.Cells[2].Attributes.Add("onclick", "javascript:ShowLedger('" + MainAcc.Text + "','" + Fdate + "','" + Main.Text + "','" + ViewState["SegmentID"].ToString() + "','" + Tdate + "','P','" + SubLedgerType.Text + "');");
                        e.Row.Cells[0].Attributes.Add("onclick", "javascript:ShowLedger('" + MainAcc.Text + "','GeneralTrial','" + ViewState["SegmentID"].ToString() + "','" + Main.Text + "','SubAccName','UCC','" + date + "');");
                        //e.Row.Cells[0].Text = "View";
                        e.Row.Cells[0].ToolTip = "Click To View Ledger View Details";

                    }

                }
                string MainName = ((DataRowView)e.Row.DataItem)["accountsledger_mainaccountid"].ToString();
                if (MainName == "Total")
                {
                    e.Row.Cells[0].ForeColor = System.Drawing.Color.Brown;
                    e.Row.Cells[3].ForeColor = System.Drawing.Color.Brown;
                    e.Row.Cells[4].ForeColor = System.Drawing.Color.Brown;
                    e.Row.Cells[0].Font.Bold = true;
                    e.Row.Cells[3].Font.Bold = true;
                    e.Row.Cells[4].Font.Bold = true;
                }
                else if (MainName == "Expenditure" || MainName == "Income" || MainName == "Liabilities" || MainName == "Assets")
                {
                    e.Row.Cells[0].ForeColor = System.Drawing.Color.Blue;
                    e.Row.Cells[0].Font.Bold = true;
                }

            }
            if (e.Row.RowType == DataControlRowType.Pager)
            {
                System.Globalization.NumberFormatInfo currencyFormat = new System.Globalization.CultureInfo("hi-IN").NumberFormat;
                currencyFormat.CurrencySymbol = "";
                currencyFormat.CurrencyNegativePattern = 2;
                GridViewRow row = e.Row;
                Literal litDiff = (Literal)row.FindControl("litDiff");
                if (ViewState["Diff"] != null)
                {
                    string Diff = null;
                    decimal difference = Convert.ToDecimal(ViewState["Diff"].ToString());
                    if (difference < 0)
                    {
                        Diff = difference.ToString("c", currencyFormat) + " (CR)";
                    }
                    else
                    {
                        Diff = difference.ToString("c", currencyFormat) + " (DR)";
                    }
                    litDiff.Text = "Difference :&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp" + Diff;
                }
            }
        }
        protected void grdGeneralTrial_Sorting(object sender, GridViewSortEventArgs e)
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

            //DataTable dt = dtSubsidiary;
            if (ViewState["dtSubsidiary"] != null)
                dtSubsidiary = (DataTable)ViewState["dtSubsidiary"];
            System.Globalization.NumberFormatInfo currencyFormat = new System.Globalization.CultureInfo("hi-IN").NumberFormat;
            currencyFormat.CurrencySymbol = "";
            currencyFormat.CurrencyNegativePattern = 2;
            DataView dv = new DataView(dtSubsidiary);

            dv.Sort = sortExpression + direction;

            decimal SumDr = Convert.ToDecimal(dtSubsidiary.Compute("sum(DR)", ""));
            decimal SumCr = Convert.ToDecimal(dtSubsidiary.Compute("sum(CR)", ""));
            ViewState["Diff"] = Convert.ToString(SumDr - SumCr);

            grdGeneralTrial.DataSource = dv;
            grdGeneralTrial.DataBind();

            //Debjyoti Bind New Dev Express Grid
            aspxGdGeneralTrial.DataSource = dv ;
            aspxGdGeneralTrial.DataBind();
            Session["RptGeneralTrial"] = dv;

            grdGeneralTrial.FooterRow.Cells[0].Text = "Total";
            if (SumDr == 0)
                grdGeneralTrial.FooterRow.Cells[2].Text = "";
            else
                grdGeneralTrial.FooterRow.Cells[2].Text = SumDr.ToString("c", currencyFormat);
            if (SumCr == 0)
                grdGeneralTrial.FooterRow.Cells[3].Text = "";
            else
                grdGeneralTrial.FooterRow.Cells[3].Text = SumCr.ToString("c", currencyFormat);
            grdGeneralTrial.FooterRow.Cells[0].HorizontalAlign = HorizontalAlign.Left;
            grdGeneralTrial.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Right;
            grdGeneralTrial.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Right;
            grdGeneralTrial.FooterRow.Cells[0].ForeColor = System.Drawing.Color.White;
            grdGeneralTrial.FooterRow.Cells[2].ForeColor = System.Drawing.Color.White;
            grdGeneralTrial.FooterRow.Cells[3].ForeColor = System.Drawing.Color.White;
            grdGeneralTrial.FooterRow.Cells[0].Font.Bold = true;
            grdGeneralTrial.FooterRow.Cells[2].Font.Bold = true;
            grdGeneralTrial.FooterRow.Cells[3].Font.Bold = true;
            grdGeneralTrial.FooterRow.Cells[2].Wrap = false;
            grdGeneralTrial.FooterRow.Cells[3].Wrap = false;



        }

        public void FillPeriodGrid()
        {
            decimal SumAssetOpenDr = 0;
            decimal SumAssetOpenCr = 0;
            decimal SumAssetAmountDr = 0;
            decimal SumAssetAmountCr = 0;
            decimal SumAssetClosingDr = 0;
            decimal SumAssetClosingCr = 0;

            decimal SumLiabilityOpenDr = 0;
            decimal SumLiabilityOpenCr = 0;
            decimal SumLiabilityAmountDr = 0;
            decimal SumLiabilityAmountCr = 0;
            decimal SumLiabilityClosingDr = 0;
            decimal SumLiabilityClosingCr = 0;

            decimal SumIncomeOpenDr = 0;
            decimal SumIncomeOpenCr = 0;
            decimal SumIncomeAmountDr = 0;
            decimal SumIncomeAmountCr = 0;
            decimal SumIncomeClosingDr = 0;
            decimal SumIncomeClosingCr = 0;

            decimal SumExpenseOpenDr = 0;
            decimal SumExpenseOpenCr = 0;
            decimal SumExpenseAmountDr = 0;
            decimal SumExpenseAmountCr = 0;
            decimal SumExpenseClosingDr = 0;
            decimal SumExpenseClosingCr = 0;
            string ZeroBal = "";

            System.Globalization.NumberFormatInfo currencyFormat = new System.Globalization.CultureInfo("hi-IN").NumberFormat;
            currencyFormat.CurrencySymbol = "";
            currencyFormat.CurrencyNegativePattern = 2;
            SegmentID = hdnSegment.Value;
            
            //CompanyID = Session["CompanyID"].ToString();
            //debjyoti
            CompanyID = Session["LastCompany"].ToString();
            if (rdbMainAll.Checked == true)
            {
                SegmentID = null;
                DataTable dtSegment = oDBEngine.GetDataTable("tbl_master_companyExchange", "exch_internalId", "exch_compId='" + Session["CompanyID"].ToString() + "'");
                if (dtSegment.Rows.Count > 0)
                {
                    for (int i = 0; i < dtSegment.Rows.Count; i++)
                    {
                        if (SegmentID == null)
                            SegmentID = dtSegment.Rows[i][0].ToString();
                        else
                            SegmentID += "," + dtSegment.Rows[i][0].ToString();
                    }
                    ViewState["SegmentID"] = SegmentID;
                }
                //if (Session["userlastsegment"].ToString() == "5")
                //{
                //    SegmentID += "0" + SegmentID;
                //}
                //ViewState["SegmentID"] = SegmentID;
            }
            else
            {
                if (Session["userlastsegment"].ToString() == "5")
                {
                    DataTable DtSeg = new DataTable();
                    DtSeg = oDBEngine.GetDataTable("(select exch_compId,exch_internalId,isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Comp from tbl_master_companyExchange where exch_compid in (select ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment=" + Session["userlastsegment"].ToString() + ")) as D", "*", "Comp in(select seg_name from tbl_master_segment where seg_id=" + Session["userlastsegment"].ToString() + ") and  exch_compID='" + Session["LastCompany"].ToString() + "'");
                    SegmentID = DtSeg.Rows[0][1].ToString();

                }
                else
                {
                    if (SegmentID == "")
                        //SegmentID = ViewState["SegmentID"].ToString();
                        //debjyoti
                       SegmentID= Session["userlastsegment"].ToString();
                    ViewState["SegmentID"] = SegmentID;
                }
            }
            if (chkZero.Checked == true)
            {
                ZeroBal = "Y";
            }
            else
            {
                ZeroBal = "N";

            }

            DataSet ds = new DataSet();

            /* For Tier Structrure--------------------
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            {
                using (SqlDataAdapter da = new SqlDataAdapter("Fetch_GeneralTrial", con))
                {
                    da.SelectCommand.Parameters.AddWithValue("@FromDate", dtFrom.Value);
                    da.SelectCommand.Parameters.AddWithValue("@ToDate", dtTo.Value);
                    da.SelectCommand.Parameters.AddWithValue("@Segment", SegmentID);
                    da.SelectCommand.Parameters.AddWithValue("@Company", CompanyID);
                    da.SelectCommand.Parameters.AddWithValue("@FinancialYr", Session["LastFinYear"].ToString());
                    da.SelectCommand.Parameters.AddWithValue("@ZeroBal", ZeroBal);
                    ///Currency Setting
                    da.SelectCommand.Parameters.AddWithValue("@ActiveCurrency", Session["ActiveCurrency"].ToString().Split('~')[0]);
                    da.SelectCommand.Parameters.AddWithValue("@TradeCurrency", Session["TradeCurrency"].ToString().Split('~')[0]);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.CommandTimeout = 0;
                              
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    ds.Reset();
                    da.Fill(ds);
                    */

            string strbranchList = Convert.ToString(branchList.Value);

            if (strbranchList == "")
                {
                    string[] branhArray = oDBEngine.GetFieldValue1("tbl_master_branch FOR XML PATH('')),1,1,'')", "STUFF((Select ','+Convert(varchar(10),branch_id)", "1=1", 1);
                  strbranchList   =branhArray[0];
                }

           ds= OfrmReport_IframeGeneralTrialBL.Fetch_GeneralTrial(dtFrom.Value.ToString(), dtTo.Value.ToString(), SegmentID, CompanyID, Session["LastFinYear"].ToString(), ZeroBal,
            Session["ActiveCurrency"].ToString().Split('~')[0], Session["TradeCurrency"].ToString().Split('~')[0], strbranchList);


            ViewState["dataset"] = ds;

            //    }
            //}

            DataTable dtEx = new DataTable();
            DataTable dtMain = ds.Tables[0];
            dtEx.Columns.Add("accountsledger_mainaccountid");
            dtEx.Columns.Add("MainAccID");
            dtEx.Columns.Add("ID");
            dtEx.Columns.Add("OpeningDr");
            dtEx.Columns.Add("OpeningCr");
            dtEx.Columns.Add("AmountDR");
            dtEx.Columns.Add("AmountCR");
            dtEx.Columns.Add("ClosingDr");
            dtEx.Columns.Add("ClosingCr");
            dtEx.Columns.Add("SubLedgerType");
            dtEx.Columns.Add("AccountType");
            dtEx.Columns.Add("CashBankType");

            #region
            DataRow DrNew2 = dtEx.NewRow();
            DrNew2[0] = "Liabilities";
            DrNew2[9] = "None";
            dtEx.Rows.Add(DrNew2);

            for (int i = 0; i < dtMain.Rows.Count; i++)
            {
                if (dtMain.Rows[i]["AccType"].ToString() == "Liability")
                {

                    DataRow newRow = dtEx.NewRow();
                    newRow["accountsledger_mainaccountid"] = dtMain.Rows[i]["accountsledger_mainaccountid"].ToString();
                    newRow["MainAccID"] = dtMain.Rows[i]["ClosingID"].ToString();
                    newRow["ID"] = dtMain.Rows[i]["ClosingID"].ToString();
                    if (dtMain.Rows[i]["OpeningDr"].ToString() == "")
                        newRow["OpeningDr"] = dtMain.Rows[i]["OpeningDr"].ToString();
                    else
                        newRow["OpeningDr"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["OpeningDr"].ToString()));
                    if (dtMain.Rows[i]["OpeningCr"].ToString() == "")
                        newRow["OpeningCr"] = dtMain.Rows[i]["OpeningCr"].ToString();
                    else
                        newRow["OpeningCr"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["OpeningCr"].ToString()));
                    if (dtMain.Rows[i]["AmountDr"].ToString() == "")
                        newRow["AmountDR"] = dtMain.Rows[i]["AmountDr"].ToString();
                    else
                        newRow["AmountDR"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["AmountDr"].ToString()));
                    if (dtMain.Rows[i]["AmountCr"].ToString() == "")
                        newRow["AmountCR"] = dtMain.Rows[i]["AmountCr"].ToString();
                    else
                        newRow["AmountCR"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["AmountCr"].ToString()));
                    if (dtMain.Rows[i]["ClosingDr"].ToString() == "")
                        newRow["ClosingDr"] = dtMain.Rows[i]["ClosingDr"].ToString();
                    else
                        newRow["ClosingDr"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["ClosingDr"].ToString()));
                    if (dtMain.Rows[i]["ClosingCr"].ToString() == "")
                        newRow["ClosingCr"] = dtMain.Rows[i]["ClosingCr"].ToString();
                    else
                        newRow["ClosingCr"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["ClosingCr"].ToString()));

                    //newRow["OpeningDr"] = dtMain.Rows[i]["OpeningDr"].ToString();
                    //newRow["OpeningCr"] = dtMain.Rows[i]["OpeningCr"].ToString();
                    //newRow["AmountDR"] = dtMain.Rows[i]["AmountDr"].ToString();
                    //newRow["AmountCR"] = dtMain.Rows[i]["AmountCr"].ToString();
                    //newRow["ClosingDr"] = dtMain.Rows[i]["ClosingDr"].ToString();
                    //newRow["ClosingCr"] = dtMain.Rows[i]["ClosingCr"].ToString();
                    newRow["SubLedgerType"] = dtMain.Rows[i]["SubLedgerType"].ToString();
                    newRow["AccountType"] = dtMain.Rows[i]["AccType"].ToString();
                    newRow["CashBankType"] = dtMain.Rows[i]["CashBankType"].ToString();
                    dtEx.Rows.Add(newRow);
                }
            }
            DataRow TotLI1 = dtEx.NewRow();
            TotLI1[9] = "None";
            dtEx.Rows.Add(TotLI1);
            DataRow TotLI = dtEx.NewRow();
            TotLI[0] = "Total";
            TotLI[3] = ds.Tables[3].Rows[0]["LIOPDr"].ToString();
            TotLI[4] = ds.Tables[3].Rows[0]["LIOpCr"].ToString();
            TotLI[5] = ds.Tables[3].Rows[0]["LIAmtDr"].ToString();
            TotLI[6] = ds.Tables[3].Rows[0]["LIAmtCr"].ToString();
            TotLI[7] = ds.Tables[3].Rows[0]["LICLDr"].ToString();
            TotLI[8] = ds.Tables[3].Rows[0]["LICLCr"].ToString();
            TotLI[9] = "None";
            dtEx.Rows.Add(TotLI);
            #endregion

            #region
            DataRow DrNew8 = dtEx.NewRow();
            DrNew8[0] = "Assets";
            DrNew8[9] = "None";
            dtEx.Rows.Add(DrNew8);

            for (int i = 0; i < dtMain.Rows.Count; i++)
            {
                if (dtMain.Rows[i]["AccType"].ToString() == "Asset")
                {

                    DataRow newRow = dtEx.NewRow();
                    newRow["accountsledger_mainaccountid"] = dtMain.Rows[i]["accountsledger_mainaccountid"].ToString();
                    newRow["MainAccID"] = dtMain.Rows[i]["ClosingID"].ToString();
                    newRow["ID"] = dtMain.Rows[i]["ClosingID"].ToString();
                    if (dtMain.Rows[i]["OpeningDr"].ToString() == "")
                        newRow["OpeningDr"] = dtMain.Rows[i]["OpeningDr"].ToString();
                    else
                        newRow["OpeningDr"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["OpeningDr"].ToString()));
                    if (dtMain.Rows[i]["OpeningCr"].ToString() == "")
                        newRow["OpeningCr"] = dtMain.Rows[i]["OpeningCr"].ToString();
                    else
                        newRow["OpeningCr"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["OpeningCr"].ToString()));
                    if (dtMain.Rows[i]["AmountDr"].ToString() == "")
                        newRow["AmountDR"] = dtMain.Rows[i]["AmountDr"].ToString();
                    else
                        newRow["AmountDR"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["AmountDr"].ToString()));
                    if (dtMain.Rows[i]["AmountCr"].ToString() == "")
                        newRow["AmountCR"] = dtMain.Rows[i]["AmountCr"].ToString();
                    else
                        newRow["AmountCR"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["AmountCr"].ToString()));
                    if (dtMain.Rows[i]["ClosingDr"].ToString() == "")
                        newRow["ClosingDr"] = dtMain.Rows[i]["ClosingDr"].ToString();
                    else
                        newRow["ClosingDr"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["ClosingDr"].ToString()));
                    if (dtMain.Rows[i]["ClosingCr"].ToString() == "")
                        newRow["ClosingCr"] = dtMain.Rows[i]["ClosingCr"].ToString();
                    else
                        newRow["ClosingCr"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["ClosingCr"].ToString()));






                    newRow["SubLedgerType"] = dtMain.Rows[i]["SubLedgerType"].ToString();
                    newRow["AccountType"] = dtMain.Rows[i]["AccType"].ToString();
                    newRow["CashBankType"] = dtMain.Rows[i]["CashBankType"].ToString();
                    dtEx.Rows.Add(newRow);
                }
            }

            DataRow TotAsset = dtEx.NewRow();
            TotAsset[9] = "None";
            dtEx.Rows.Add(TotAsset);
            DataRow TotAsset1 = dtEx.NewRow();
            TotAsset1[0] = "Total";
            TotAsset1[3] = ds.Tables[2].Rows[0]["AssetOPDr"].ToString();
            TotAsset1[4] = ds.Tables[2].Rows[0]["AssetOpCr"].ToString();
            TotAsset1[5] = ds.Tables[2].Rows[0]["AssetAmtDr"].ToString();
            TotAsset1[6] = ds.Tables[2].Rows[0]["AssetAmtCr"].ToString();
            TotAsset1[7] = ds.Tables[2].Rows[0]["AssetCLDr"].ToString();
            TotAsset1[8] = ds.Tables[2].Rows[0]["AssetCLCr"].ToString();
            TotAsset1[9] = "None";
            dtEx.Rows.Add(TotAsset1);

            #endregion
            #region
            DataRow DrNew3 = dtEx.NewRow();
            DrNew3[0] = "Income";
            DrNew3[9] = "None";
            dtEx.Rows.Add(DrNew3);

            for (int i = 0; i < dtMain.Rows.Count; i++)
            {
                if (dtMain.Rows[i]["AccType"].ToString() == "Income")
                {

                    DataRow newRow = dtEx.NewRow();
                    newRow["accountsledger_mainaccountid"] = dtMain.Rows[i]["accountsledger_mainaccountid"].ToString();
                    newRow["MainAccID"] = dtMain.Rows[i]["ClosingID"].ToString();
                    newRow["ID"] = dtMain.Rows[i]["ClosingID"].ToString();



                    if (dtMain.Rows[i]["OpeningDr"].ToString() == "")
                        newRow["OpeningDr"] = dtMain.Rows[i]["OpeningDr"].ToString();
                    else
                        newRow["OpeningDr"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["OpeningDr"].ToString()));
                    if (dtMain.Rows[i]["OpeningCr"].ToString() == "")
                        newRow["OpeningCr"] = dtMain.Rows[i]["OpeningCr"].ToString();
                    else
                        newRow["OpeningCr"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["OpeningCr"].ToString()));
                    if (dtMain.Rows[i]["AmountDr"].ToString() == "")
                        newRow["AmountDR"] = dtMain.Rows[i]["AmountDr"].ToString();
                    else
                        newRow["AmountDR"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["AmountDr"].ToString()));
                    if (dtMain.Rows[i]["AmountCr"].ToString() == "")
                        newRow["AmountCR"] = dtMain.Rows[i]["AmountCr"].ToString();
                    else
                        newRow["AmountCR"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["AmountCr"].ToString()));
                    if (dtMain.Rows[i]["ClosingDr"].ToString() == "")
                        newRow["ClosingDr"] = dtMain.Rows[i]["ClosingDr"].ToString();
                    else
                        newRow["ClosingDr"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["ClosingDr"].ToString()));
                    if (dtMain.Rows[i]["ClosingCr"].ToString() == "")
                        newRow["ClosingCr"] = dtMain.Rows[i]["ClosingCr"].ToString();
                    else
                        newRow["ClosingCr"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["ClosingCr"].ToString()));

                    //newRow["OpeningDr"] = dtMain.Rows[i]["OpeningDr"].ToString();
                    //newRow["OpeningCr"] = dtMain.Rows[i]["OpeningCr"].ToString();
                    //newRow["AmountDR"] = dtMain.Rows[i]["AmountDr"].ToString();
                    //newRow["AmountCR"] = dtMain.Rows[i]["AmountCr"].ToString();
                    //newRow["ClosingDr"] = dtMain.Rows[i]["ClosingDr"].ToString();
                    //newRow["ClosingCr"] = dtMain.Rows[i]["ClosingCr"].ToString();
                    newRow["SubLedgerType"] = dtMain.Rows[i]["SubLedgerType"].ToString();
                    newRow["AccountType"] = dtMain.Rows[i]["AccType"].ToString();
                    newRow["CashBankType"] = dtMain.Rows[i]["CashBankType"].ToString();
                    dtEx.Rows.Add(newRow);
                }
            }
            DataRow TotIN1 = dtEx.NewRow();
            TotIN1[9] = "None";
            dtEx.Rows.Add(TotIN1);
            DataRow TotIN = dtEx.NewRow();
            TotIN[0] = "Total";
            TotIN[3] = ds.Tables[4].Rows[0]["InOPDr"].ToString();
            TotIN[4] = ds.Tables[4].Rows[0]["InOpCr"].ToString();
            TotIN[5] = ds.Tables[4].Rows[0]["InAmtDr"].ToString();
            TotIN[6] = ds.Tables[4].Rows[0]["InAmtCr"].ToString();
            TotIN[7] = ds.Tables[4].Rows[0]["InCLDr"].ToString();
            TotIN[8] = ds.Tables[4].Rows[0]["InCLCr"].ToString();
            TotIN[9] = "None";
            dtEx.Rows.Add(TotIN);
            #endregion
            #region
            DataRow DrNew4 = dtEx.NewRow();
            DrNew4[0] = "Expenditure";
            DrNew4[9] = "None";
            dtEx.Rows.Add(DrNew4);

            for (int i = 0; i < dtMain.Rows.Count; i++)
            {
                if (dtMain.Rows[i]["AccType"].ToString() == "Expense")
                {

                    DataRow newRow = dtEx.NewRow();
                    newRow["accountsledger_mainaccountid"] = dtMain.Rows[i]["accountsledger_mainaccountid"].ToString();
                    newRow["MainAccID"] = dtMain.Rows[i]["ClosingID"].ToString();
                    newRow["ID"] = dtMain.Rows[i]["ClosingID"].ToString();

                    if (dtMain.Rows[i]["OpeningDr"].ToString() == "")
                        newRow["OpeningDr"] = dtMain.Rows[i]["OpeningDr"].ToString();
                    else
                        newRow["OpeningDr"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["OpeningDr"].ToString()));
                    if (dtMain.Rows[i]["OpeningCr"].ToString() == "")
                        newRow["OpeningCr"] = dtMain.Rows[i]["OpeningCr"].ToString();
                    else
                        newRow["OpeningCr"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["OpeningCr"].ToString()));
                    if (dtMain.Rows[i]["AmountDr"].ToString() == "")
                        newRow["AmountDR"] = dtMain.Rows[i]["AmountDr"].ToString();
                    else
                        newRow["AmountDR"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["AmountDr"].ToString()));
                    if (dtMain.Rows[i]["AmountCr"].ToString() == "")
                        newRow["AmountCR"] = dtMain.Rows[i]["AmountCr"].ToString();
                    else
                        newRow["AmountCR"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["AmountCr"].ToString()));
                    if (dtMain.Rows[i]["ClosingDr"].ToString() == "")
                        newRow["ClosingDr"] = dtMain.Rows[i]["ClosingDr"].ToString();
                    else
                        newRow["ClosingDr"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["ClosingDr"].ToString()));
                    if (dtMain.Rows[i]["ClosingCr"].ToString() == "")
                        newRow["ClosingCr"] = dtMain.Rows[i]["ClosingCr"].ToString();
                    else
                        newRow["ClosingCr"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtMain.Rows[i]["ClosingCr"].ToString()));

                    //newRow["OpeningDr"] = dtMain.Rows[i]["OpeningDr"].ToString();
                    //newRow["OpeningCr"] = dtMain.Rows[i]["OpeningCr"].ToString();
                    //newRow["AmountDR"] = dtMain.Rows[i]["AmountDr"].ToString();
                    //newRow["AmountCR"] = dtMain.Rows[i]["AmountCr"].ToString();
                    //newRow["ClosingDr"] = dtMain.Rows[i]["ClosingDr"].ToString();
                    //newRow["ClosingCr"] = dtMain.Rows[i]["ClosingCr"].ToString();
                    newRow["SubLedgerType"] = dtMain.Rows[i]["SubLedgerType"].ToString();
                    newRow["AccountType"] = dtMain.Rows[i]["AccType"].ToString();
                    newRow["CashBankType"] = dtMain.Rows[i]["CashBankType"].ToString();
                    dtEx.Rows.Add(newRow);
                }
            }
            DataRow TotEx1 = dtEx.NewRow();
            TotEx1[9] = "None";
            dtEx.Rows.Add(TotEx1);
            DataRow TotEx = dtEx.NewRow();
            TotEx[0] = "Total";
            TotEx[3] = ds.Tables[5].Rows[0]["ExOPDr"].ToString();
            TotEx[4] = ds.Tables[5].Rows[0]["ExOpCr"].ToString();
            TotEx[5] = ds.Tables[5].Rows[0]["ExAmtDr"].ToString();
            TotEx[6] = ds.Tables[5].Rows[0]["ExAmtCr"].ToString();
            TotEx[7] = ds.Tables[5].Rows[0]["ExCLDr"].ToString();
            TotEx[8] = ds.Tables[5].Rows[0]["ExCLCr"].ToString();
            TotEx[9] = "None";
            dtEx.Rows.Add(TotEx);
            #endregion

            DataRow NetTot1 = dtEx.NewRow();
            NetTot1[9] = "None";
            dtEx.Rows.Add(NetTot1);
            DataRow NetTot = dtEx.NewRow();
            NetTot[0] = "Total";
            NetTot[3] = ds.Tables[1].Rows[0]["TotOPDr"].ToString();
            NetTot[4] = ds.Tables[1].Rows[0]["TotOpCr"].ToString();
            NetTot[5] = ds.Tables[1].Rows[0]["TotAmtDr"].ToString();
            NetTot[6] = ds.Tables[1].Rows[0]["TotAmtCr"].ToString();
            NetTot[7] = ds.Tables[1].Rows[0]["TotCLDr"].ToString();
            NetTot[8] = ds.Tables[1].Rows[0]["TotCLCr"].ToString();
            NetTot[9] = "None";
            dtEx.Rows.Add(NetTot);
            ViewState["dtPeriod"] = dtEx;
            grdPeriod.DataSource = dtEx;
            grdPeriod.DataBind();
            //debjyoti
            aspxPeriodGrid.DataSource = dtEx;
            aspxPeriodGrid.DataBind();
            Session["RptGeneralTrialPeriod"] = dtEx;

            //pageSize = 20000;
            //int Date = Convert.ToDateTime(dtFrom.Value.ToString()).Day;
            //int Month = Convert.ToDateTime(dtFrom.Value.ToString()).Month;
            //if (Date == 1 && Month == 4)
            //   dtPeriod = oDBEngine.GetDataTable("(select accountsledger_mainaccountid,case when AmountDr=0 then null else convert(varchar(50),cast(AmountDr as money),1) end as AmountDr,case when AmountCr=0 then null else convert(varchar(50),cast(AmountCr as money),1) end as AmountCr,ID,SubLedgerType,case when op>0 then convert(varchar(50),cast(op as money),1) else null end as OpeningDr,case when op<0 then convert(varchar(50),cast(((-1)*op) as money),1) else null end as OpeningCr,case when cl+op>0 then convert(varchar(50),cast(cl+op as money),1) else null end as ClosingDr,case when cl+op<0 then convert(varchar(50),cast(((-1)*(cl+op)) as money),1) else null end as ClosingCr,MainAccID,AmountDr as AmtDr,AmountCr as AmtCr,case when op>0 then op else 0 end as opnDr,case when op<0 then ((-1)*op) else 0 end as opnCr,case when cl+op>0 then cl+op  else 0 end as ClsDr,case when cl+op<0 then ((-1)*(cl+op)) else 0 end as ClsCr,AccType  from (Select (select mainaccount_name from master_mainaccount where mainaccount_accountcode=trans_accountsledger.accountsledger_mainaccountid) as accountsledger_mainaccountid,(select isnull(sum(b.accountsledger_amountDr),0) from trans_accountsledger b where  b.AccountsLedger_MainAccountID=Trans_AccountsLedger.AccountsLedger_MainAccountID and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,b.AccountsLedger_TransactionDate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtFrom.Value + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtTo.Value + "')) as datetime)  and b.AccountsLedger_ExchangeSegmentID in(" + SegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "' and b.AccountsLedger_TransactionType<>'OpeningBalance') as AmountDR,(select isnull(sum(b.accountsledger_amountCr),0) from trans_accountsledger b where  b.AccountsLedger_MainAccountID=Trans_AccountsLedger.AccountsLedger_MainAccountID and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,b.AccountsLedger_TransactionDate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtFrom.Value + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtTo.Value + "')) as datetime)  and b.AccountsLedger_ExchangeSegmentID in(" + SegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "' and b.AccountsLedger_TransactionType<>'OpeningBalance') as AmountCR,accountsledger_mainaccountid as ID,(select ltrim(rtrim(MainAccount_SubLedgerType)) from master_mainaccount where mainaccount_accountcode=trans_accountsledger.accountsledger_mainaccountid) as SubLedgerType,(select mainaccount_accountcode from master_mainaccount where mainaccount_accountcode=trans_accountsledger.accountsledger_mainaccountid) as MainAccID,(select isnull(sum(a.accountsledger_amountDr-a.accountsledger_amountCr),0) from trans_accountsledger a where a.AccountsLedger_MainAccountID=Trans_AccountsLedger.AccountsLedger_MainAccountID and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,a.AccountsLedger_TransactionDate)) as datetime)=cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtFrom.Value + "')) as datetime) and a.AccountsLedger_ExchangeSegmentID in(" + SegmentID + ") and a.AccountsLedger_CompanyID='" + CompanyID + "' and a.AccountsLedger_Finyear='" + Session["LastFinYear"].ToString() + "' and a.AccountsLedger_TransactionType='OpeningBalance') as op,(select isnull(sum(b.accountsledger_amountDr-b.accountsledger_amountCr),0) from trans_accountsledger b where  b.AccountsLedger_MainAccountID=Trans_AccountsLedger.AccountsLedger_MainAccountID and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,b.AccountsLedger_TransactionDate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtFrom.Value + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtTo.Value + "')) as datetime)  and b.AccountsLedger_ExchangeSegmentID in(" + SegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "' and AccountsLedger_Finyear='" + Session["LastFinYear"].ToString() + "' and AccountsLedger_TransactionType<>'OpeningBalance') as cl,(select mainaccount_accounttype from master_mainaccount where mainaccount_accountcode=trans_accountsledger.accountsledger_mainaccountid) as AccType from Trans_AccountsLedger WHERE  accountsledger_mainaccountid is not null and accountsledger_exchangesegmentid in(" + SegmentID + ") and accountsledger_companyid='" + CompanyID + "' and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,AccountsLedger_TransactionDate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtFrom.Value + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtTo.Value + "')) as datetime) group By  accountsledger_mainaccountid ) as D group By  D.accountsledger_mainaccountid,D.AmountDR,D.ID,D.SubLedgerType,D.MainAccID,D.op,D.cl,D.AmountCR,D.AccType ) as KK", " accountsledger_mainaccountid,MainAccID,OpeningDr,OpeningCr,AmountDr,AmountCr,ClosingDr,ClosingCr,ID,SubLedgerType,AmtDr,AmtCr,opnDr,opnCr,ClsDr,ClsCr,case when ((ClsDr-ClsCr)<0 and AccType='Asset') then 'Liability' when  ((ClsDr-ClsCr)>0 and AccType='Liability')  then 'Asset'	when ((ClsDr-ClsCr)>=0 and AccType='Asset') then 'Asset' when  ((ClsDr-ClsCr)<=0 and AccType='Liability')  then 'Liability'	when ((ClsDr-ClsCr)>0 and AccType='Income') then 'Expenses' when ((ClsDr-ClsCr)<0 and AccType='Expenses') then 'Income' when ((ClsDr-ClsCr)<=0 and AccType='Income') then 'Income' else 'Expenses' end as AccType ", null, " AccType,accountsledger_mainaccountid ");
            //    //dtPeriod = oDBEngine.GetDataTable("(select accountsledger_mainaccountid,AmountDR,AmountCR,ClosingID as ID,SubLedgerType,ClosingID as MainAccID, OpeningCRDR as op,ClosingCRDR as cl,AccType from( select * from  (     select  (select mainaccount_name from master_mainaccount  where mainaccount_accountcode=trans_accountsledger.accountsledger_mainaccountid ) as accountsledger_mainaccountid, (select ltrim(rtrim(MainAccount_SubLedgerType)) from master_mainaccount where mainaccount_accountcode=trans_accountsledger.accountsledger_mainaccountid) as SubLedgerType, (select mainaccount_accounttype from master_mainaccount where mainaccount_accountcode=trans_accountsledger.accountsledger_mainaccountid) as AccType , accountsledger_mainaccountid as ClosingID,isnull(sum(accountsledger_amountcr-accountsledger_amountdr),0) as ClosingCRDR 	from trans_accountsledger where 	AccountsLedger_Finyear='" + Session["LastFinYear"].ToString() + "' 	and accountsledger_exchangesegmentid in (" + SegmentID + ") 	and accountsledger_transactiondate<='" + dtTo.Value + "' and AccountsLedger_CompanyID='" + CompanyID + "' 	group by accountsledger_mainaccountid 	 )as A left  outer join   (select  (select mainaccount_name from master_mainaccount  where mainaccount_accountcode=trans_accountsledger.accountsledger_mainaccountid ) as accountsledger_mainaccountid1, (select ltrim(rtrim(MainAccount_SubLedgerType)) from master_mainaccount where mainaccount_accountcode=trans_accountsledger.accountsledger_mainaccountid) as SubLedgerType1, (select mainaccount_accounttype from master_mainaccount where mainaccount_accountcode=trans_accountsledger.accountsledger_mainaccountid) as AccType1 ,    accountsledger_mainaccountid as OpenID,isnull(sum(accountsledger_amountcr-accountsledger_amountdr),0) as OpeningCRDR  	from trans_accountsledger where    AccountsLedger_Finyear='" + Session["LastFinYear"].ToString() + "' 	and accountsledger_exchangesegmentid in (" + SegmentID + ") 	and accountsledger_transactiondate< '" + dtFrom.Value + "'     and AccountsLedger_CompanyID='" + CompanyID + "' 	group by accountsledger_mainaccountid 	having sum(accountsledger_amountcr-accountsledger_amountdr)<>0.00) as B on A.ClosingID=B.OpenID   )as C left outer join  (	select  (select mainaccount_name from master_mainaccount  where mainaccount_accountcode=trans_accountsledger.accountsledger_mainaccountid ) as accountsledger_mainaccountid2, (select ltrim(rtrim(MainAccount_SubLedgerType)) from master_mainaccount where mainaccount_accountcode=trans_accountsledger.accountsledger_mainaccountid) as SubLedgerType2, (select mainaccount_accounttype from master_mainaccount where mainaccount_accountcode=trans_accountsledger.accountsledger_mainaccountid) as AccType2 , accountsledger_mainaccountid as AmountID,isnull(sum(accountsledger_amountcr),0) as  AmountCR,isnull(sum(accountsledger_amountdr),0) as AmountDR 	from trans_accountsledger where 	AccountsLedger_Finyear='" + Session["LastFinYear"].ToString() + "' 	and accountsledger_exchangesegmentid in (" + SegmentID + ") 	and accountsledger_transactiondate between  '" + dtFrom.Value + "' and  '" + dtTo.Value + "' and AccountsLedger_CompanyID='" + CompanyID + "' 	group by accountsledger_mainaccountid 	 ) as E  on C.ClosingID=E.AmountID   ) as D group By  D.accountsledger_mainaccountid,D.AmountDR,D.ID,D.SubLedgerType,D.MainAccID,D.op,D.cl,D.AmountCR,D.AccType ) as KK  ", "accountsledger_mainaccountid,MainAccID,ISNULL(OpeningDr,0) AS OpeningDr ,ISNULL(OpeningCr,0) AS OpeningCr,ISNULL(AmountDr,0) AS AmountDr,ISNULL(AmountCr,0) AS AmountCr ,ISNULL(ClosingDr,0) AS ClosingDr,ISNULL(ClosingCr,0) AS ClosingCr,ID,SubLedgerType,ISNULL(AmtDr,0) AS AmtDr,ISNULL(AmtCr,0) AS AmtCr,ISNULL(opnDr,0) AS opnDr,ISNULL(opnCr,0)AS opnCr,ISNULL(ClsDr,0) AS ClsDr,ISNULL(ClsCr,0) AS ClsCr,case when ((ClsDr-ClsCr)<0 and AccType='Asset') then 'Liability' when  ((ClsDr-ClsCr)>0 and AccType='Liability')  then 'Asset'	when ((ClsDr-ClsCr)>=0 and AccType='Asset') then 'Asset' when  ((ClsDr-ClsCr)<=0 and AccType='Liability')  then 'Liability'	when ((ClsDr-ClsCr)>0 and AccType='Income') then 'Expenses' when ((ClsDr-ClsCr)<0 and AccType='Expenses') then 'Income' when ((ClsDr-ClsCr)<=0 and AccType='Income') then 'Income' else 'Expenses' end as AccType  from (select accountsledger_mainaccountid,case when AmountDr=0 then null else convert(varchar(50),cast(AmountDr as money),1) end as AmountDr,case when AmountCr=0 then null else convert(varchar(50),cast(AmountCr as money),1) end as AmountCr,ID,SubLedgerType,case when op>0 then convert(varchar(50),cast(op as money),1) else null end as OpeningDr,case when op<0 then convert(varchar(50),cast(((-1)*op) as money),1) else null end as OpeningCr,case when cl+op>0 then convert(varchar(50),cast(cl+op as money),1) else null end as ClosingDr,case when cl+op<0 then convert(varchar(50),cast(((-1)*(cl+op)) as money),1) else null end as ClosingCr,MainAccID,AmountDr as AmtDr,AmountCr as AmtCr,case when op>0 then op else 0 end as opnDr,case when op<0 then ((-1)*op) else 0 end as opnCr,case when cl+op>0 then cl+op  else 0 end as ClsDr,case when cl+op<0 then ((-1)*(cl+op)) else 0 end as ClsCr,AccType", null, "AccType,accountsledger_mainaccountid");
            //else
            //     dtPeriod = oDBEngine.GetDataTable("(Select  accountsledger_mainaccountid,MainAccID,	case when op>0 then convert(varchar(50),cast(op as money),1) else null end as OpeningDr,case when op<0 then convert(varchar(50),cast(((-1)*op) as money),1) else null end as OpeningCr,	case when AmountDr=0 then null else convert(varchar(50),cast(AmountDr as money),1) end as AmountDr,	case when AmountCr=0 then null else convert(varchar(50),cast(AmountCr as money),1) end as AmountCr,case when cl+op>0 then convert(varchar(50),cast(cl+op as money),1) else null end as ClosingDr,case when cl+op<0 then convert(varchar(50),cast(((-1)*(cl+op)) as money),1) else null end as ClosingCr,ID,SubLedgerType,AmountDr as AmtDr,AmountCr as AmtCr,case when op>0 then op else 0 end as opnDr,case when op<0 then ((-1)*op) else 0 end as opnCr,case when cl+op>0 then cl+op  else 0 end as ClsDr,case when cl+op<0 then ((-1)*(cl+op)) else 0 end as ClsCr,AccType from (Select (select mainaccount_name from master_mainaccount where mainaccount_accountcode=trans_accountsledger.accountsledger_mainaccountid) as accountsledger_mainaccountid,(select isnull(sum(b.accountsledger_amountDr),0) from trans_accountsledger b where  b.AccountsLedger_MainAccountID=Trans_AccountsLedger.AccountsLedger_MainAccountID and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,b.AccountsLedger_TransactionDate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtFrom.Value + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtTo.Value + "')) as datetime)  and b.AccountsLedger_ExchangeSegmentID in(" + SegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "' and b.AccountsLedger_TransactionType<>'OpeningBalance') as AmountDR,(select isnull(sum(b.accountsledger_amountCr),0) from trans_accountsledger b where  b.AccountsLedger_MainAccountID=Trans_AccountsLedger.AccountsLedger_MainAccountID and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,b.AccountsLedger_TransactionDate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtFrom.Value + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtTo.Value + "')) as datetime)  and b.AccountsLedger_ExchangeSegmentID in(" + SegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "' and b.AccountsLedger_TransactionType<>'OpeningBalance') as AmountCR,accountsledger_mainaccountid as ID,(select ltrim(rtrim(MainAccount_SubLedgerType)) from master_mainaccount where mainaccount_accountcode=trans_accountsledger.accountsledger_mainaccountid) as SubLedgerType,(select mainaccount_accountcode from master_mainaccount where mainaccount_accountcode=trans_accountsledger.accountsledger_mainaccountid) as MainAccID,(select isnull(sum(a.accountsledger_amountDr-a.accountsledger_amountCr),0) from trans_accountsledger a where a.AccountsLedger_MainAccountID=Trans_AccountsLedger.AccountsLedger_MainAccountID and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,a.AccountsLedger_TransactionDate)) as datetime)<cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtFrom.Value + "')) as datetime) and a.AccountsLedger_ExchangeSegmentID in(" + SegmentID + ") and a.AccountsLedger_CompanyID='" + CompanyID + "' and a.AccountsLedger_Finyear='" + Session["LastFinYear"].ToString() + "') as op,(select isnull(sum(b.accountsledger_amountDr-b.accountsledger_amountCr),0) from trans_accountsledger b where  b.AccountsLedger_MainAccountID=Trans_AccountsLedger.AccountsLedger_MainAccountID and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,b.AccountsLedger_TransactionDate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtFrom.Value + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtTo.Value + "')) as datetime)  and b.AccountsLedger_ExchangeSegmentID in(" + SegmentID + ") and AccountsLedger_CompanyID='" + CompanyID + "' and AccountsLedger_Finyear='" + Session["LastFinYear"].ToString() + "' and AccountsLedger_TransactionType<>'OpeningBalance') as cl,(select mainaccount_accounttype from master_mainaccount where mainaccount_accountcode=trans_accountsledger.accountsledger_mainaccountid) as AccType  from Trans_AccountsLedger WHERE  accountsledger_mainaccountid is not null and accountsledger_exchangesegmentid in(" + SegmentID + ") and accountsledger_companyid='" + CompanyID + "' and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,AccountsLedger_TransactionDate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtFrom.Value + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtTo.Value + "')) as datetime) group By  accountsledger_mainaccountid ) as D group By  D.accountsledger_mainaccountid,D.AmountDR,D.ID,D.SubLedgerType,D.MainAccID,D.op,D.cl,D.AmountCR,D.AccType ) as KK ", " accountsledger_mainaccountid,MainAccID,OpeningDr,OpeningCr,AmountDr,AmountCr,ClosingDr,ClosingCr,ID,SubLedgerType,AmtDr,AmtCr,opnDr,opnCr,ClsDr,ClsCr,case when ((ClsDr-ClsCr)<0 and AccType='Asset') then 'Liability' when  ((ClsDr-ClsCr)>0 and AccType='Liability')  then 'Asset'	when ((ClsDr-ClsCr)>=0 and AccType='Asset') then 'Asset' when  ((ClsDr-ClsCr)<=0 and AccType='Liability')  then 'Liability'	when ((ClsDr-ClsCr)>0 and AccType='Income') then 'Expenses' when ((ClsDr-ClsCr)<0 and AccType='Expenses') then 'Income' when ((ClsDr-ClsCr)<=0 and AccType='Income') then 'Income' else 'Expenses' end as AccType ", null, " AccType,accountsledger_mainaccountid");
            //    //dtPeriod = oDBEngine.GetDataTable("(select accountsledger_mainaccountid,AmountDR,AmountCR,ClosingID as ID,SubLedgerType,ClosingID as MainAccID, OpeningCRDR as op,ClosingCRDR as cl,AccType from( select * from  (     select  (select mainaccount_name from master_mainaccount  where mainaccount_accountcode=trans_accountsledger.accountsledger_mainaccountid ) as accountsledger_mainaccountid, (select ltrim(rtrim(MainAccount_SubLedgerType)) from master_mainaccount where mainaccount_accountcode=trans_accountsledger.accountsledger_mainaccountid) as SubLedgerType, (select mainaccount_accounttype from master_mainaccount where mainaccount_accountcode=trans_accountsledger.accountsledger_mainaccountid) as AccType , accountsledger_mainaccountid as ClosingID,isnull(sum(accountsledger_amountcr-accountsledger_amountdr),0) as ClosingCRDR 	from trans_accountsledger where 	AccountsLedger_Finyear='" + Session["LastFinYear"].ToString() + "' 	and accountsledger_exchangesegmentid in (" + SegmentID + ") 	and accountsledger_transactiondate<='" + dtTo.Value + "' and AccountsLedger_CompanyID='" + CompanyID + "' 	group by accountsledger_mainaccountid 	 )as A left  outer join   (select  (select mainaccount_name from master_mainaccount  where mainaccount_accountcode=trans_accountsledger.accountsledger_mainaccountid ) as accountsledger_mainaccountid1, (select ltrim(rtrim(MainAccount_SubLedgerType)) from master_mainaccount where mainaccount_accountcode=trans_accountsledger.accountsledger_mainaccountid) as SubLedgerType1, (select mainaccount_accounttype from master_mainaccount where mainaccount_accountcode=trans_accountsledger.accountsledger_mainaccountid) as AccType1 ,    accountsledger_mainaccountid as OpenID,isnull(sum(accountsledger_amountcr-accountsledger_amountdr),0) as OpeningCRDR  	from trans_accountsledger where    AccountsLedger_Finyear='" + Session["LastFinYear"].ToString() + "' 	and accountsledger_exchangesegmentid in (" + SegmentID + ") 	and accountsledger_transactiondate< '" + dtFrom.Value + "'     and AccountsLedger_CompanyID='" + CompanyID + "' 	group by accountsledger_mainaccountid 	having sum(accountsledger_amountcr-accountsledger_amountdr)<>0.00) as B on A.ClosingID=B.OpenID   )as C left outer join  (	select  (select mainaccount_name from master_mainaccount  where mainaccount_accountcode=trans_accountsledger.accountsledger_mainaccountid ) as accountsledger_mainaccountid2, (select ltrim(rtrim(MainAccount_SubLedgerType)) from master_mainaccount where mainaccount_accountcode=trans_accountsledger.accountsledger_mainaccountid) as SubLedgerType2, (select mainaccount_accounttype from master_mainaccount where mainaccount_accountcode=trans_accountsledger.accountsledger_mainaccountid) as AccType2 , accountsledger_mainaccountid as AmountID,isnull(sum(accountsledger_amountcr),0) as  AmountCR,isnull(sum(accountsledger_amountdr),0) as AmountDR 	from trans_accountsledger where 	AccountsLedger_Finyear='" + Session["LastFinYear"].ToString() + "' 	and accountsledger_exchangesegmentid in (" + SegmentID + ") 	and accountsledger_transactiondate between  '" + dtFrom.Value + "' and  '" + dtTo.Value + "' and AccountsLedger_CompanyID='" + CompanyID + "' 	group by accountsledger_mainaccountid 	 ) as E  on C.ClosingID=E.AmountID   ) as D group By  D.accountsledger_mainaccountid,D.AmountDR,D.ID,D.SubLedgerType,D.MainAccID,D.op,D.cl,D.AmountCR,D.AccType ) as KK  ", "accountsledger_mainaccountid,MainAccID,ISNULL(OpeningDr,0) AS OpeningDr ,ISNULL(OpeningCr,0) AS OpeningCr,ISNULL(AmountDr,0) AS AmountDr,ISNULL(AmountCr,0) AS AmountCr ,ISNULL(ClosingDr,0) AS ClosingDr,ISNULL(ClosingCr,0) AS ClosingCr,ID,SubLedgerType,ISNULL(AmtDr,0) AS AmtDr,ISNULL(AmtCr,0) AS AmtCr,ISNULL(opnDr,0) AS opnDr,ISNULL(opnCr,0)AS opnCr,ISNULL(ClsDr,0) AS ClsDr,ISNULL(ClsCr,0) AS ClsCr,case when ((ClsDr-ClsCr)<0 and AccType='Asset') then 'Liability' when  ((ClsDr-ClsCr)>0 and AccType='Liability')  then 'Asset'	 when ((ClsDr-ClsCr)>=0 and AccType='Asset') then 'Asset'  when  ((ClsDr-ClsCr)<=0 and AccType='Liability')  then 'Liability'	when ((ClsDr-ClsCr)>0 and AccType='Income') then 'Expenses'  when ((ClsDr-ClsCr)<0 and AccType='Expenses') then 'Income'  when ((ClsDr-ClsCr)<=0 and AccType='Income') then 'Income'  else 'Expenses' end as AccType   from (Select  accountsledger_mainaccountid,MainAccID,	 case when op>0 then convert(varchar(50),cast(op as money),1) else null end as OpeningDr, case when op<0 then convert(varchar(50),cast(((-1)*op) as money),1) else null end as OpeningCr,	 case when AmountDr=0 then null else convert(varchar(50),cast(AmountDr as money),1) end as AmountDr,	 case when AmountCr=0 then null else convert(varchar(50),cast(AmountCr as money),1) end as AmountCr, case when cl+op>0 then convert(varchar(50),cast(cl+op as money),1) else null end as ClosingDr,case when cl+op<0 then convert(varchar(50),cast(((-1)*(cl+op)) as money),1) else null end as ClosingCr,ID, SubLedgerType,AmountDr as AmtDr,AmountCr as AmtCr,case when op>0 then op else 0 end as opnDr, case when op<0 then ((-1)*op) else 0 end as opnCr, case when cl+op>0 then cl+op  else 0 end as ClsDr, case when cl+op<0 then ((-1)*(cl+op)) else 0 end as ClsCr, AccType", null, "AccType,accountsledger_mainaccountid");
            //decimal SumAmountDr = Convert.ToDecimal(dtPeriod.Compute("sum(amtDr)", ""));
            //decimal SumAmountCr = Convert.ToDecimal(dtPeriod.Compute("sum(amtCr)", ""));
            //decimal SumOpeningCr = Convert.ToDecimal(dtPeriod.Compute("sum(opnDr)", ""));
            //decimal SumOpeningDr = Convert.ToDecimal(dtPeriod.Compute("sum(opnCr)", ""));
            //decimal SumClosingDr = Convert.ToDecimal(dtPeriod.Compute("sum(ClsDr)", ""));
            //decimal SumClosingCr = Convert.ToDecimal(dtPeriod.Compute("sum(ClsCr)", ""));
            //ViewState["SumAmountDr"] = SumAmountDr.ToString();
            //ViewState["SumAmountCr"] = SumAmountCr.ToString();
            //ViewState["SumOpeningCr"] = SumOpeningCr.ToString();
            //ViewState["SumOpeningDr"] = SumOpeningDr.ToString();
            //ViewState["SumClosingDr"] = SumClosingDr.ToString();
            //ViewState["SumClosingCr"] = SumClosingCr.ToString();

            //pagecount = dtPeriod.Rows.Count / pageSize + 1;
            //TotalPagesPeriod.Value = pagecount.ToString();
            //if (pageindex <= 0)
            //{
            //    pageindex = 0;
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "hide", "DisablePeriod('P');", true);
            //}
            //if (pageindex >= int.Parse(TotalPagesPeriod.Value.ToString()))
            //{
            //    pageindex = int.Parse(TotalPagesPeriod.Value.ToString()) - 1;
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "hide", "DisablePeriod('N');", true);
            //}
            //if (pageindex >= (int.Parse(TotalPagesPeriod.Value.ToString()) - 1))
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "hide", "DisablePeriod('N');", true);
            //}
            //grdPeriod.PageIndex = pageindex;
            //CurrentPagePeriod.Value = pageindex.ToString();
            //rowcount = 0;
            //if (ViewState["FirstTime"] == null)
            //{
            //    info.AddMergedColumns(new int[] { 3, 4 }, "Opening");
            //    info.AddMergedColumns(new int[] { 5, 6 }, "During the Period");
            //    info.AddMergedColumns(new int[] { 7, 8 }, "Closing");
            //}

            //DataTable dtAsset = dtPeriod.Clone();
            //DataTable dtIncome = dtPeriod.Clone();
            //DataTable dtLiability = dtPeriod.Clone();
            //DataTable dtExpenditure = dtPeriod.Clone();



            //DataView foundRow = new DataView(dtPeriod);
            //foundRow.RowFilter = "AccType='Asset'";
            //foreach (DataRowView dvr in foundRow)
            //{
            //    dtAsset.ImportRow(dvr.Row);
            //}
            //dtAsset.AcceptChanges();

            //DataView foundRow1 = new DataView(dtPeriod);
            //foundRow1.RowFilter = "AccType='Liability'";
            //foreach (DataRowView dvr in foundRow1)
            //{
            //    dtLiability.ImportRow(dvr.Row);
            //}
            //dtLiability.AcceptChanges();

            //DataView foundRow2 = new DataView(dtPeriod);
            //foundRow2.RowFilter = "AccType='Income'";
            //foreach (DataRowView dvr in foundRow2)
            //{
            //    dtIncome.ImportRow(dvr.Row);
            //}
            //dtIncome.AcceptChanges();

            //DataView foundRow3 = new DataView(dtPeriod);
            //foundRow3.RowFilter = "AccType='Expenses'";
            //foreach (DataRowView dvr in foundRow3)
            //{
            //    dtExpenditure.ImportRow(dvr.Row);
            //}
            //dtExpenditure.AcceptChanges();

            //foundRow.Dispose();
            //foundRow1.Dispose();
            //foundRow2.Dispose();
            //foundRow3.Dispose();

            //if (dtAsset.Rows.Count > 0)
            //{
            //    SumAssetOpenDr = Convert.ToDecimal(dtAsset.Compute("sum(opnDr)", ""));
            //    SumAssetOpenCr = Convert.ToDecimal(dtAsset.Compute("sum(opnCr)", ""));
            //    SumAssetAmountDr = Convert.ToDecimal(dtAsset.Compute("sum(AmtDr)", ""));
            //    SumAssetAmountCr = Convert.ToDecimal(dtAsset.Compute("sum(AmtCr)", ""));
            //    SumAssetClosingDr = Convert.ToDecimal(dtAsset.Compute("sum(ClsDr)", ""));
            //    SumAssetClosingCr = Convert.ToDecimal(dtAsset.Compute("sum(ClsCr)", ""));
            //}
            //if (dtLiability.Rows.Count > 0)
            //{
            //    SumLiabilityOpenDr = Convert.ToDecimal(dtLiability.Compute("sum(opnDr)", ""));
            //    SumLiabilityOpenCr = Convert.ToDecimal(dtLiability.Compute("sum(opnCr)", ""));
            //    SumLiabilityAmountDr = Convert.ToDecimal(dtLiability.Compute("sum(AmtDr)", ""));
            //    SumLiabilityAmountCr = Convert.ToDecimal(dtLiability.Compute("sum(AmtCr)", ""));
            //    SumLiabilityClosingDr = Convert.ToDecimal(dtLiability.Compute("sum(ClsDr)", ""));
            //    SumLiabilityClosingCr = Convert.ToDecimal(dtLiability.Compute("sum(ClsCr)", ""));
            //}
            //if (dtIncome.Rows.Count > 0)
            //{
            //    SumIncomeOpenDr = Convert.ToDecimal(dtIncome.Compute("sum(opnDr)", ""));
            //    SumIncomeOpenCr = Convert.ToDecimal(dtIncome.Compute("sum(opnCr)", ""));
            //    SumIncomeAmountDr = Convert.ToDecimal(dtIncome.Compute("sum(AmtDr)", ""));
            //    SumIncomeAmountCr = Convert.ToDecimal(dtIncome.Compute("sum(AmtCr)", ""));
            //    SumIncomeClosingDr = Convert.ToDecimal(dtIncome.Compute("sum(ClsDr)", ""));
            //    SumIncomeClosingCr = Convert.ToDecimal(dtIncome.Compute("sum(ClsCr)", ""));
            //}
            //if (dtExpenditure.Rows.Count > 0)
            //{
            //    SumExpenseOpenDr = Convert.ToDecimal(dtExpenditure.Compute("sum(opnDr)", ""));
            //    SumExpenseOpenCr = Convert.ToDecimal(dtExpenditure.Compute("sum(opnCr)", ""));
            //    SumExpenseAmountDr = Convert.ToDecimal(dtExpenditure.Compute("sum(AmtDr)", ""));
            //    SumExpenseAmountCr = Convert.ToDecimal(dtExpenditure.Compute("sum(AmtCr)", ""));
            //    SumExpenseClosingDr = Convert.ToDecimal(dtExpenditure.Compute("sum(ClsDr)", ""));
            //    SumExpenseClosingCr = Convert.ToDecimal(dtExpenditure.Compute("sum(ClsCr)", ""));
            //}

            //dtAsonAssetLiability = dtPeriod.Clone();

            //DataColumn dcolColumn = new DataColumn("opnDebit", typeof(string));
            //dtAsonAssetLiability.Columns.Add(dcolColumn);
            //DataColumn dcolColumn1 = new DataColumn("opnCredit", typeof(string));
            //dtAsonAssetLiability.Columns.Add(dcolColumn1);
            //DataColumn dcolColumn2 = new DataColumn("AmtDebit", typeof(string));
            //dtAsonAssetLiability.Columns.Add(dcolColumn2);
            //DataColumn dcolColumn3 = new DataColumn("AmtCredit", typeof(string));
            //dtAsonAssetLiability.Columns.Add(dcolColumn3);
            //DataColumn dcolColumn4 = new DataColumn("CloDebit", typeof(string));
            //dtAsonAssetLiability.Columns.Add(dcolColumn4);
            //DataColumn dcolColumn5 = new DataColumn("CloCredit", typeof(string));
            //dtAsonAssetLiability.Columns.Add(dcolColumn5);

            //#region Asset
            //if (dtAsset.Rows.Count > 0)
            //{
            //    for (int Asst = 0; Asst < dtAsset.Rows.Count; Asst++)
            //    {
            //        if (Asst == 0)
            //        {
            //            DataRow DrNew1 = dtAsonAssetLiability.NewRow();
            //            DrNew1[0] = "Assets";
            //            DrNew1[9] = "None";
            //            dtAsonAssetLiability.Rows.Add(DrNew1);
            //        }
            //        DataRow DrNew = dtAsonAssetLiability.NewRow();
            //        DrNew[0] = dtAsset.Rows[Asst][0].ToString();
            //        DrNew[1] = dtAsset.Rows[Asst][1].ToString();
            //        if (dtAsset.Rows[Asst][2] != DBNull.Value)
            //        {
            //            DrNew[2] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtAsset.Rows[Asst]["OpeningDr"].ToString()));
            //            DrNew[17] = oconverter.formatmoneyinUs(Convert.ToDecimal(dtAsset.Rows[Asst]["OpeningDr"].ToString()));
            //        }
            //        else
            //        {
            //            DrNew[2] = "";
            //            DrNew[17] = "";
            //        }
            //        if (dtAsset.Rows[Asst][3] != DBNull.Value)
            //        {
            //            DrNew[3] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtAsset.Rows[Asst]["OpeningCr"].ToString()));
            //            DrNew[18] = oconverter.formatmoneyinUs(Convert.ToDecimal(dtAsset.Rows[Asst]["OpeningCr"].ToString()));
            //        }
            //        else
            //        {
            //            DrNew[3] = "";
            //            DrNew[18] = "";
            //        }
            //        if (dtAsset.Rows[Asst][4] != DBNull.Value)
            //        {
            //            DrNew[4] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtAsset.Rows[Asst]["AmountDr"].ToString()));
            //            DrNew[19] = oconverter.formatmoneyinUs(Convert.ToDecimal(dtAsset.Rows[Asst]["AmountDr"].ToString()));
            //        }
            //        else
            //        {
            //            DrNew[4] = "";
            //            DrNew[19] = "";
            //        }
            //        if (dtAsset.Rows[Asst][5] != DBNull.Value)
            //        {
            //            DrNew[5] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtAsset.Rows[Asst]["AmountCr"].ToString()));
            //            DrNew[20] = oconverter.formatmoneyinUs(Convert.ToDecimal(dtAsset.Rows[Asst]["AmountCr"].ToString()));
            //        }
            //        else
            //        {
            //            DrNew[5] = "";
            //            DrNew[20] = "";
            //        }
            //        if (dtAsset.Rows[Asst][6] != DBNull.Value)
            //        {
            //            DrNew[6] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtAsset.Rows[Asst]["ClosingDr"].ToString()));
            //            DrNew[21] = oconverter.formatmoneyinUs(Convert.ToDecimal(dtAsset.Rows[Asst]["ClosingDr"].ToString()));
            //        }
            //        else
            //        {
            //            DrNew[6] = "";
            //            DrNew[21] = "";
            //        }
            //        if (dtAsset.Rows[Asst][7] != DBNull.Value)
            //        {
            //            DrNew[7] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtAsset.Rows[Asst]["ClosingCr"].ToString()));
            //            DrNew[22] = oconverter.formatmoneyinUs(Convert.ToDecimal(dtAsset.Rows[Asst]["ClosingCr"].ToString()));
            //        }
            //        else
            //        {
            //            DrNew[7] = "";
            //            DrNew[22] = "";
            //        }
            //        DrNew[8] = dtAsset.Rows[Asst][8].ToString();
            //        DrNew[9] = dtAsset.Rows[Asst][9].ToString();
            //        //if (dtAsset.Rows[Asst][10].ToString() != "")
            //        //    DrNew[10] = dtAsset.Rows[Asst][10].ToString();
            //        //else
            //        //    DrNew[10] = 0;
            //        //if (dtAsset.Rows[Asst][11].ToString() != "")
            //        //    DrNew[11] = dtAsset.Rows[Asst][11].ToString();
            //        //else
            //        //    DrNew[11] = 0;
            //        //if (dtAsset.Rows[Asst][12].ToString() != "")
            //        //    DrNew[12] = dtAsset.Rows[Asst][12].ToString();
            //        //else
            //        //    DrNew[12] = 0;
            //        //if (dtAsset.Rows[Asst][13].ToString() != "")
            //        //    DrNew[13] = dtAsset.Rows[Asst][13].ToString();
            //        //else
            //        //    DrNew[13] = 0;

            //        //if (dtAsset.Rows[Asst][14].ToString() != "")
            //        //    DrNew[14] = dtAsset.Rows[Asst][14].ToString();
            //        //else
            //        //    DrNew[14] = 0;
            //        //if (dtAsset.Rows[Asst][15].ToString() != "")
            //        //    DrNew[15] = dtAsset.Rows[Asst][15].ToString();
            //        //else
            //        //    DrNew[15] = 0;



            //        DrNew[11] = dtAsset.Rows[Asst][10].ToString();
            //        DrNew[11] = dtAsset.Rows[Asst][11].ToString();
            //        DrNew[12] = dtAsset.Rows[Asst][12].ToString();
            //        DrNew[13] = dtAsset.Rows[Asst][13].ToString();
            //        DrNew[14] = dtAsset.Rows[Asst][14].ToString();
            //        DrNew[15] = dtAsset.Rows[Asst][15].ToString();
            //        DrNew[16] = dtAsset.Rows[Asst][16].ToString();
            //        dtAsonAssetLiability.Rows.Add(DrNew);
            //        if (Asst == dtAsset.Rows.Count - 1)
            //        {
            //            DataRow DrNew3 = dtAsonAssetLiability.NewRow();
            //            DrNew3[9] = "None";
            //            dtAsonAssetLiability.Rows.Add(DrNew3);
            //            DataRow DrNew2 = dtAsonAssetLiability.NewRow();
            //            DrNew2[0] = "Total";
            //            DrNew2[2] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(SumAssetOpenDr));
            //            DrNew2[3] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(SumAssetOpenCr));
            //            DrNew2[4] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(SumAssetAmountDr));
            //            DrNew2[5] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(SumAssetAmountCr));
            //            DrNew2[6] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(SumAssetClosingDr));
            //            DrNew2[7] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(SumAssetClosingCr));

            //            DrNew2[17] = oconverter.formatmoneyinUs(Convert.ToDecimal(SumAssetOpenDr));
            //            DrNew2[18] = oconverter.formatmoneyinUs(Convert.ToDecimal(SumAssetOpenCr));
            //            DrNew2[19] = oconverter.formatmoneyinUs(Convert.ToDecimal(SumAssetAmountDr));
            //            DrNew2[20] = oconverter.formatmoneyinUs(Convert.ToDecimal(SumAssetAmountCr));
            //            DrNew2[21] = oconverter.formatmoneyinUs(Convert.ToDecimal(SumAssetClosingDr));
            //            DrNew2[22] = oconverter.formatmoneyinUs(Convert.ToDecimal(SumAssetClosingCr));
            //            DrNew2[9] = "None";
            //            dtAsonAssetLiability.Rows.Add(DrNew2);
            //        }

            //    }
            //}
            //#endregion
            //#region Liability
            //if (dtLiability.Rows.Count > 0)
            //{
            //    for (int k = 0; k < dtLiability.Rows.Count; k++)
            //    {
            //        if (k == 0)
            //        {
            //            DataRow DrNew1 = dtAsonAssetLiability.NewRow();
            //            DrNew1[0] = "Liabilities";
            //            DrNew1[9] = "None";
            //            dtAsonAssetLiability.Rows.Add(DrNew1);
            //        }
            //        DataRow DrNew = dtAsonAssetLiability.NewRow();
            //        DrNew[0] = dtLiability.Rows[k][0].ToString();
            //        DrNew[1] = dtLiability.Rows[k][1].ToString();
            //        if (dtLiability.Rows[k][2] != DBNull.Value)
            //        {                  

            //            DrNew[2] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtLiability.Rows[k]["OpeningDr"].ToString()));
            //            DrNew[17] = oconverter.formatmoneyinUs(Convert.ToDecimal(dtLiability.Rows[k]["OpeningDr"].ToString()));
            //        }
            //        else
            //        {
            //            DrNew[2] = "";
            //            DrNew[17] = "";
            //        }
            //        if (dtLiability.Rows[k][3] != DBNull.Value)
            //        {
            //            DrNew[3] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtLiability.Rows[k]["OpeningCr"].ToString()));
            //            DrNew[18] = oconverter.formatmoneyinUs(Convert.ToDecimal(dtLiability.Rows[k]["OpeningCr"].ToString()));
            //        }
            //        else
            //        {
            //            DrNew[3] = "";
            //            DrNew[18] = "";
            //        }
            //        if (dtLiability.Rows[k][4] != DBNull.Value)
            //        {
            //            DrNew[4] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtLiability.Rows[k]["AmountDr"].ToString()));
            //            DrNew[19] = oconverter.formatmoneyinUs(Convert.ToDecimal(dtLiability.Rows[k]["AmountDr"].ToString()));
            //        }
            //        else
            //        {
            //            DrNew[4] = "";
            //            DrNew[19] = "";
            //        }
            //        if (dtLiability.Rows[k][5] != DBNull.Value)
            //        {
            //            DrNew[5] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtLiability.Rows[k]["AmountCr"].ToString()));
            //            DrNew[20] = oconverter.formatmoneyinUs(Convert.ToDecimal(dtLiability.Rows[k]["AmountCr"].ToString()));
            //        }
            //        else
            //        {
            //            DrNew[5] = "";
            //            DrNew[20] = "";
            //        }
            //        if (dtLiability.Rows[k][6] != DBNull.Value)
            //        {
            //            DrNew[6] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtLiability.Rows[k]["ClosingDr"].ToString()));
            //            DrNew[21] = oconverter.formatmoneyinUs(Convert.ToDecimal(dtLiability.Rows[k]["ClosingDr"].ToString()));
            //        }
            //        else
            //        {
            //            DrNew[6] = "";
            //            DrNew[21] = "";
            //        }
            //        if (dtLiability.Rows[k][7] != DBNull.Value)
            //        {
            //            DrNew[7] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtLiability.Rows[k]["ClosingCr"].ToString()));
            //            DrNew[22] = oconverter.formatmoneyinUs(Convert.ToDecimal(dtLiability.Rows[k]["ClosingCr"].ToString()));
            //        }
            //        else
            //        {
            //            DrNew[7] = "";
            //            DrNew[22] = "";
            //        }

            //        DrNew[8] = dtLiability.Rows[k][8].ToString();
            //        DrNew[9] = dtLiability.Rows[k][9].ToString();
            //        DrNew[10] = dtLiability.Rows[k][10].ToString();
            //        DrNew[11] = dtLiability.Rows[k][11].ToString();
            //        DrNew[12] = dtLiability.Rows[k][12].ToString();
            //        DrNew[13] = dtLiability.Rows[k][13].ToString();
            //        DrNew[14] = dtLiability.Rows[k][14].ToString();
            //        DrNew[15] = dtLiability.Rows[k][15].ToString();
            //        DrNew[16] = dtLiability.Rows[k][16].ToString();
            //        dtAsonAssetLiability.Rows.Add(DrNew);
            //        if (k == dtLiability.Rows.Count - 1)
            //        {
            //            DataRow DrNew3 = dtAsonAssetLiability.NewRow();
            //            DrNew3[9] = "None";
            //            dtAsonAssetLiability.Rows.Add(DrNew3);
            //            DataRow DrNew2 = dtAsonAssetLiability.NewRow();
            //            DrNew2[0] = "Total";
            //            DrNew2[2] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(SumLiabilityOpenDr));
            //            DrNew2[3] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(SumLiabilityOpenCr));
            //            DrNew2[4] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(SumLiabilityAmountDr));
            //            DrNew2[5] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(SumLiabilityAmountCr));
            //            DrNew2[6] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(SumLiabilityClosingDr));
            //            DrNew2[7] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(SumLiabilityClosingCr));

            //            DrNew2[17] = oconverter.formatmoneyinUs(Convert.ToDecimal(SumLiabilityOpenDr));
            //            DrNew2[18] = oconverter.formatmoneyinUs(Convert.ToDecimal(SumLiabilityOpenCr));
            //            DrNew2[19] = oconverter.formatmoneyinUs(Convert.ToDecimal(SumLiabilityAmountDr));
            //            DrNew2[20] = oconverter.formatmoneyinUs(Convert.ToDecimal(SumLiabilityAmountCr));
            //            DrNew2[21] = oconverter.formatmoneyinUs(Convert.ToDecimal(SumLiabilityClosingDr));
            //            DrNew2[22] = oconverter.formatmoneyinUs(Convert.ToDecimal(SumLiabilityClosingCr));
            //            DrNew2[9] = "None";
            //            dtAsonAssetLiability.Rows.Add(DrNew2);
            //        }

            //    }
            //}
            //#endregion
            //#region Income
            //if (dtIncome.Rows.Count > 0)
            //{
            //    for (int k = 0; k < dtIncome.Rows.Count; k++)
            //    {
            //        if (k == 0)
            //        {
            //            DataRow DrNew1 = dtAsonAssetLiability.NewRow();
            //            DrNew1[0] = "Income";
            //            DrNew1[9] = "None";
            //            dtAsonAssetLiability.Rows.Add(DrNew1);
            //        }
            //        DataRow DrNew = dtAsonAssetLiability.NewRow();
            //        DrNew[0] = dtIncome.Rows[k][0].ToString();
            //        DrNew[1] = dtIncome.Rows[k][1].ToString();
            //        if (dtIncome.Rows[k][2] != DBNull.Value)
            //        {

            //            DrNew[2] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtIncome.Rows[k]["OpeningDr"].ToString()));
            //            DrNew[17] = oconverter.formatmoneyinUs(Convert.ToDecimal(dtIncome.Rows[k]["OpeningDr"].ToString()));
            //        }
            //        else
            //        {
            //            DrNew[2] = "";
            //            DrNew[17] = "";
            //        }
            //        if (dtIncome.Rows[k][3] != DBNull.Value)
            //        {
            //            DrNew[3] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtIncome.Rows[k]["OpeningCr"].ToString()));
            //            DrNew[18] = oconverter.formatmoneyinUs(Convert.ToDecimal(dtIncome.Rows[k]["OpeningCr"].ToString()));
            //        }
            //        else
            //        {
            //            DrNew[3] = "";
            //            DrNew[18] = "";
            //        }
            //        if (dtIncome.Rows[k][4] != DBNull.Value)
            //        {
            //            DrNew[4] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtIncome.Rows[k]["AmountDr"].ToString()));
            //            DrNew[19] = oconverter.formatmoneyinUs(Convert.ToDecimal(dtIncome.Rows[k]["AmountDr"].ToString()));
            //        }
            //        else
            //        {
            //            DrNew[4] = "";
            //            DrNew[19] = "";
            //        }
            //        if (dtIncome.Rows[k][5] != DBNull.Value)
            //        {
            //            DrNew[5] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtIncome.Rows[k]["AmountCr"].ToString()));
            //            DrNew[20] = oconverter.formatmoneyinUs(Convert.ToDecimal(dtIncome.Rows[k]["AmountCr"].ToString()));
            //        }
            //        else
            //        {
            //            DrNew[5] = "";
            //            DrNew[20] = "";
            //        }
            //        if (dtIncome.Rows[k][6] != DBNull.Value)
            //        {
            //            DrNew[6] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtIncome.Rows[k]["ClosingDr"].ToString()));
            //            DrNew[21] = oconverter.formatmoneyinUs(Convert.ToDecimal(dtIncome.Rows[k]["ClosingDr"].ToString()));
            //        }
            //        else
            //        {
            //            DrNew[6] = "";
            //            DrNew[21] = "";
            //        }
            //        if (dtIncome.Rows[k][7] != DBNull.Value)
            //        {
            //            DrNew[7] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtIncome.Rows[k]["ClosingCr"].ToString()));
            //            DrNew[22] = oconverter.formatmoneyinUs(Convert.ToDecimal(dtIncome.Rows[k]["ClosingCr"].ToString()));
            //        }
            //        else
            //        {
            //            DrNew[7] = "";
            //            DrNew[22] = "";
            //        }

            //        DrNew[8] = dtIncome.Rows[k][8].ToString();
            //        DrNew[9] = dtIncome.Rows[k][9].ToString();
            //        //if (dtIncome.Rows[k][10].ToString() != "")
            //        //    DrNew[10] = dtIncome.Rows[k][10].ToString();
            //        //else
            //        //    DrNew[10] = 0;
            //        //if (dtIncome.Rows[k][11].ToString() != "")
            //        //    DrNew[11] = dtIncome.Rows[k][11].ToString();
            //        //else
            //        //    DrNew[11] = 0;
            //        //if (dtIncome.Rows[k][12].ToString() != "")
            //        //    DrNew[12] = dtIncome.Rows[k][12].ToString();
            //        //else
            //        //    DrNew[12] = 0;
            //        //if (dtIncome.Rows[k][13].ToString() != "")
            //        //    DrNew[13] = dtIncome.Rows[k][13].ToString();
            //        //else
            //        //    DrNew[13] = 0; 
            //        //if (dtIncome.Rows[k][14].ToString() != "")
            //        //    DrNew[14] = dtIncome.Rows[k][14].ToString();
            //        //else
            //        //    DrNew[14] = 0;
            //        //DrNew[15] = dtIncome.Rows[k][15].ToString();


            //        DrNew[10] = dtIncome.Rows[k][10].ToString();
            //        DrNew[11] = dtIncome.Rows[k][11].ToString();
            //        DrNew[12] = dtIncome.Rows[k][12].ToString();
            //        DrNew[13] = dtIncome.Rows[k][13].ToString();
            //        DrNew[14] = dtIncome.Rows[k][14].ToString();
            //        DrNew[15] = dtIncome.Rows[k][15].ToString();
            //        DrNew[16] = dtIncome.Rows[k][16].ToString();
            //        dtAsonAssetLiability.Rows.Add(DrNew);
            //        if (k == dtIncome.Rows.Count - 1)
            //        {
            //            DataRow DrNew3 = dtAsonAssetLiability.NewRow();
            //            DrNew3[9] = "None";
            //            dtAsonAssetLiability.Rows.Add(DrNew3);
            //            DataRow DrNew2 = dtAsonAssetLiability.NewRow();
            //            DrNew2[0] = "Total";
            //            DrNew2[2] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(SumIncomeOpenDr));
            //            DrNew2[3] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(SumIncomeOpenCr));
            //            DrNew2[4] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(SumIncomeAmountDr));
            //            DrNew2[5] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(SumIncomeAmountCr));
            //            DrNew2[6] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(SumIncomeClosingDr));
            //            DrNew2[7] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(SumIncomeClosingCr));

            //            DrNew2[17] = oconverter.formatmoneyinUs(Convert.ToDecimal(SumIncomeOpenDr));
            //            DrNew2[18] = oconverter.formatmoneyinUs(Convert.ToDecimal(SumIncomeOpenCr));
            //            DrNew2[19] = oconverter.formatmoneyinUs(Convert.ToDecimal(SumIncomeAmountDr));
            //            DrNew2[20] = oconverter.formatmoneyinUs(Convert.ToDecimal(SumIncomeAmountCr));
            //            DrNew2[21] = oconverter.formatmoneyinUs(Convert.ToDecimal(SumIncomeClosingDr));
            //            DrNew2[22] = oconverter.formatmoneyinUs(Convert.ToDecimal(SumIncomeClosingCr));
            //            DrNew2[9] = "None";
            //            dtAsonAssetLiability.Rows.Add(DrNew2);
            //        }

            //    }
            //}
            //#endregion
            //#region Expenditure
            //if (dtExpenditure.Rows.Count > 0)
            //{
            //    for (int k = 0; k < dtExpenditure.Rows.Count; k++)
            //    {
            //        if (k == 0)
            //        {
            //            DataRow DrNew1 = dtAsonAssetLiability.NewRow();
            //            DrNew1[0] = "Expenditure";
            //            DrNew1[9] = "None";
            //            dtAsonAssetLiability.Rows.Add(DrNew1);
            //        }
            //        DataRow DrNew = dtAsonAssetLiability.NewRow();
            //        DrNew[0] = dtExpenditure.Rows[k][0].ToString();
            //        DrNew[1] = dtExpenditure.Rows[k][1].ToString();
            //        if (dtExpenditure.Rows[k][2] != DBNull.Value)
            //        {

            //            DrNew[2] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtExpenditure.Rows[k]["OpeningDr"].ToString()));
            //            DrNew[17] = oconverter.formatmoneyinUs(Convert.ToDecimal(dtExpenditure.Rows[k]["OpeningDr"].ToString()));
            //        }
            //        else
            //        {
            //            DrNew[2] = "";
            //            DrNew[17] = "";
            //        }
            //        if (dtExpenditure.Rows[k][3] != DBNull.Value)
            //        {
            //            DrNew[3] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtExpenditure.Rows[k]["OpeningCr"].ToString()));
            //            DrNew[18] = oconverter.formatmoneyinUs(Convert.ToDecimal(dtExpenditure.Rows[k]["OpeningCr"].ToString()));
            //        }
            //        else
            //        {
            //            DrNew[3] = "";
            //            DrNew[18] = "";
            //        }
            //        if (dtExpenditure.Rows[k][4] != DBNull.Value)
            //        {
            //            DrNew[4] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtExpenditure.Rows[k]["AmountDr"].ToString()));
            //            DrNew[19] = oconverter.formatmoneyinUs(Convert.ToDecimal(dtExpenditure.Rows[k]["AmountDr"].ToString()));
            //        }
            //        else
            //        {
            //            DrNew[4] = "";
            //            DrNew[19] = "";
            //        }
            //        if (dtExpenditure.Rows[k][5] != DBNull.Value)
            //        {
            //            DrNew[5] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtExpenditure.Rows[k]["AmountCr"].ToString()));
            //            DrNew[20] = oconverter.formatmoneyinUs(Convert.ToDecimal(dtExpenditure.Rows[k]["AmountCr"].ToString()));
            //        }
            //        else
            //        {
            //            DrNew[5] = "";
            //            DrNew[20] = "";
            //        }
            //        if (dtExpenditure.Rows[k][6] != DBNull.Value)
            //        {
            //            DrNew[6] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtExpenditure.Rows[k]["ClosingDr"].ToString()));
            //            DrNew[21] = oconverter.formatmoneyinUs(Convert.ToDecimal(dtExpenditure.Rows[k]["ClosingDr"].ToString()));
            //        }
            //        else
            //        {
            //            DrNew[6] = "";
            //            DrNew[21] = "";
            //        }
            //        if (dtExpenditure.Rows[k][7] != DBNull.Value)
            //        {
            //            DrNew[7] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtExpenditure.Rows[k]["ClosingCr"].ToString()));
            //            DrNew[22] = oconverter.formatmoneyinUs(Convert.ToDecimal(dtExpenditure.Rows[k]["ClosingCr"].ToString()));
            //        }
            //        else
            //        {
            //            DrNew[7] = "";
            //            DrNew[22] = "";
            //        }

            //        DrNew[8] = dtExpenditure.Rows[k][8].ToString();
            //        DrNew[9] = dtExpenditure.Rows[k][9].ToString();

            //        if (dtExpenditure.Rows[k][10].ToString() != "")
            //            DrNew[10] = dtExpenditure.Rows[k][10].ToString();
            //        else
            //            DrNew[10] = 0;
            //        if (dtExpenditure.Rows[k][11].ToString() != "")
            //            DrNew[11] = dtExpenditure.Rows[k][11].ToString();
            //        else
            //            DrNew[11] = 0;
            //        if (dtExpenditure.Rows[k][12].ToString() != "")
            //            DrNew[12] = dtExpenditure.Rows[k][12].ToString();
            //        else
            //            DrNew[12] = 0;
            //        if (dtExpenditure.Rows[k][13].ToString() != "")
            //            DrNew[13] = dtExpenditure.Rows[k][13].ToString();
            //        else
            //            DrNew[13] = 0;
            //        if (dtExpenditure.Rows[k][14].ToString() != "")
            //            DrNew[14] = dtExpenditure.Rows[k][14].ToString();
            //        else
            //            DrNew[14] = 0;
            //        DrNew[15] = dtExpenditure.Rows[k][15].ToString();


            //        DrNew[10] = dtExpenditure.Rows[k][10].ToString();
            //        DrNew[11] = dtExpenditure.Rows[k][11].ToString();
            //        DrNew[12] = dtExpenditure.Rows[k][12].ToString();
            //        DrNew[13] = dtExpenditure.Rows[k][13].ToString();
            //        DrNew[14] = dtExpenditure.Rows[k][14].ToString();
            //        DrNew[15] = dtExpenditure.Rows[k][15].ToString();
            //        DrNew[16] = dtExpenditure.Rows[k][16].ToString();
            //        dtAsonAssetLiability.Rows.Add(DrNew);
            //        if (k == dtExpenditure.Rows.Count - 1)
            //        {
            //            DataRow DrNew3 = dtAsonAssetLiability.NewRow();
            //            DrNew3[9] = "None";
            //            dtAsonAssetLiability.Rows.Add(DrNew3);
            //            DataRow DrNew2 = dtAsonAssetLiability.NewRow();
            //            DrNew2[0] = "Total";
            //            DrNew2[2] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(SumExpenseOpenDr));
            //            DrNew2[3] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(SumExpenseOpenCr));
            //            DrNew2[4] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(SumExpenseAmountDr));
            //            DrNew2[5] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(SumExpenseAmountCr));
            //            DrNew2[6] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(SumExpenseClosingDr));
            //            DrNew2[7] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(SumExpenseClosingCr));

            //            DrNew2[17] = oconverter.formatmoneyinUs(Convert.ToDecimal(SumExpenseOpenDr));
            //            DrNew2[18] = oconverter.formatmoneyinUs(Convert.ToDecimal(SumExpenseOpenCr));
            //            DrNew2[19] = oconverter.formatmoneyinUs(Convert.ToDecimal(SumExpenseAmountDr));
            //            DrNew2[20] = oconverter.formatmoneyinUs(Convert.ToDecimal(SumExpenseAmountCr));
            //            DrNew2[21] = oconverter.formatmoneyinUs(Convert.ToDecimal(SumExpenseClosingDr));
            //            DrNew2[22] = oconverter.formatmoneyinUs(Convert.ToDecimal(SumExpenseClosingCr));
            //            DrNew2[9] = "None";
            //            dtAsonAssetLiability.Rows.Add(DrNew2);
            //        }

            //    }
            //}
            //#endregion

            //grdPeriod.DataSource = dtAsonAssetLiability;
            //grdPeriod.DataBind();
            //ViewState["dtPeriod"] = dtAsonAssetLiability;
            //grdPeriod.FooterRow.Cells[0].Text = "Total";
            //if (SumOpeningDr == 0)
            //    grdPeriod.FooterRow.Cells[4].Text = "";
            //else
            //    grdPeriod.FooterRow.Cells[4].Text = SumOpeningDr.ToString("c", currencyFormat);
            //if (SumOpeningCr == 0)
            //    grdPeriod.FooterRow.Cells[3].Text = "";
            //else
            //    grdPeriod.FooterRow.Cells[3].Text = SumOpeningCr.ToString("c", currencyFormat);
            //if (SumAmountDr == 0)
            //    grdPeriod.FooterRow.Cells[5].Text = "";
            //else
            //    grdPeriod.FooterRow.Cells[5].Text = SumAmountDr.ToString("c", currencyFormat);
            //if (SumAmountCr == 0)
            //    grdPeriod.FooterRow.Cells[6].Text = "";
            //else
            //    grdPeriod.FooterRow.Cells[6].Text = SumAmountCr.ToString("c", currencyFormat);
            //if (SumClosingDr == 0)
            //    grdPeriod.FooterRow.Cells[7].Text = "";
            //else
            //    grdPeriod.FooterRow.Cells[7].Text = SumClosingDr.ToString("c", currencyFormat);
            //if (SumClosingCr == 0)
            //    grdPeriod.FooterRow.Cells[8].Text = "";
            //else
            //    grdPeriod.FooterRow.Cells[8].Text = SumClosingCr.ToString("c", currencyFormat);
            //grdPeriod.FooterRow.Cells[0].HorizontalAlign = HorizontalAlign.Left;
            //grdPeriod.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Right;
            //grdPeriod.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Right;
            //grdPeriod.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Right;
            //grdPeriod.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Right;
            //grdPeriod.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Right;
            //grdPeriod.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Right;
            //grdPeriod.FooterRow.Cells[0].ForeColor = System.Drawing.Color.White;
            //grdPeriod.FooterRow.Cells[3].ForeColor = System.Drawing.Color.White;
            //grdPeriod.FooterRow.Cells[4].ForeColor = System.Drawing.Color.White;
            //grdPeriod.FooterRow.Cells[5].ForeColor = System.Drawing.Color.White;
            //grdPeriod.FooterRow.Cells[6].ForeColor = System.Drawing.Color.White;
            //grdPeriod.FooterRow.Cells[7].ForeColor = System.Drawing.Color.White;
            //grdPeriod.FooterRow.Cells[8].ForeColor = System.Drawing.Color.White;
            //grdPeriod.FooterRow.Cells[0].Font.Bold = true;
            //grdPeriod.FooterRow.Cells[3].Font.Bold = true;
            //grdPeriod.FooterRow.Cells[4].Font.Bold = true;
            //grdPeriod.FooterRow.Cells[5].Font.Bold = true;
            //grdPeriod.FooterRow.Cells[6].Font.Bold = true;
            //grdPeriod.FooterRow.Cells[7].Font.Bold = true;
            //grdPeriod.FooterRow.Cells[8].Font.Bold = true;
            //grdPeriod.FooterRow.Cells[3].Wrap = false;
            //grdPeriod.FooterRow.Cells[4].Wrap = false;
            //grdPeriod.FooterRow.Cells[5].Wrap = false;
            //grdPeriod.FooterRow.Cells[6].Wrap = false;
            //grdPeriod.FooterRow.Cells[7].Wrap = false;
            //grdPeriod.FooterRow.Cells[8].Wrap = false;
            //ViewState["FirstTime"] = "FirstTime";
        }
        protected void grdPeriod_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.SetRenderMethodDelegate(RenderHeader);
            }
            string rowID = String.Empty;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (ViewState["dtPeriod"] != null)
                    dtPeriod = (DataTable)ViewState["dtPeriod"];
                rowID = "row" + e.Row.RowIndex;
                e.Row.Attributes.Add("id", "row" + e.Row.RowIndex);
                e.Row.Attributes.Add("onclick", "ChangeRowColorPeriod(" + "'" + rowID + "','" + dtPeriod.Rows.Count + "'" + ")");
            }
        }
        protected void grdPeriod_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string ZeroBal = "";
            if (chkZero.Checked == true)
            {
                ZeroBal = "Y";
            }
            else
            {
                ZeroBal = "N";

            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label MainAcc = (Label)e.Row.FindControl("lblMainAcc");
                Label SubLedgerType = (Label)e.Row.FindControl("lblSubLedgerType");
                Label CashBankType = (Label)e.Row.FindControl("lblCashBankType");
                DateTime Fdate = Convert.ToDateTime(dtFrom.Value);
                DateTime Tdate = Convert.ToDateTime(dtTo.Value);
                Label Main = (Label)e.Row.FindControl("lblMainAccount");
                if (SubLedgerType.Text != "None")
                {
                    e.Row.Cells[2].Style.Add("cursor", "hand");
                    e.Row.Cells[2].Attributes.Add("onclick", "javascript:ShowGeneralTrialDetail('" + MainAcc.Text + "','" + Fdate + "','" + Main.Text + "','" + ViewState["SegmentID"].ToString() + "','" + Tdate + "','P','" + SubLedgerType.Text + "','" + ZeroBal + "');");
                    e.Row.Cells[2].Text = "View";
                    e.Row.Cells[2].ToolTip = "Click To View Sub Acount";
                }
                if (SubLedgerType.Text == "None")
                {
                    if (CashBankType.Text == "Bank" || CashBankType.Text == "Cash")
                    {
                        e.Row.Cells[0].Style.Add("cursor", "hand");
                        e.Row.Cells[0].Attributes.Add("onclick", "javascript:ShowCashBankDetail('" + MainAcc.Text + "','" + Fdate + "','" + Main.Text + "','" + ViewState["SegmentID"].ToString() + "','" + Tdate + "','P','" + SubLedgerType.Text + "');");
                        //  e.Row.Cells[0].Text = "View";
                        e.Row.Cells[0].ToolTip = "Click To View CashBank Details";
                    }
                    else
                    {
                        e.Row.Cells[0].Style.Add("cursor", "hand");
                        //  e.Row.Cells[2].Attributes.Add("onclick", "javascript:ShowLedger('" + MainAcc.Text + "','" + Fdate + "','" + Main.Text + "','" + ViewState["SegmentID"].ToString() + "','" + Tdate + "','P','" + SubLedgerType.Text + "');");
                        e.Row.Cells[0].Attributes.Add("onclick", "javascript:ShowLedger('" + MainAcc.Text + "','GeneralTrial','" + ViewState["SegmentID"].ToString() + "','" + Main.Text + "','SubAccName','UCC','" + Tdate + "');");
                        //e.Row.Cells[0].Text = "View";
                        e.Row.Cells[0].ToolTip = "Click To View Ledger View Details";

                    }

                }
                string MainName = ((DataRowView)e.Row.DataItem)["accountsledger_mainaccountid"].ToString();
                if (MainName == "Total")
                {
                    e.Row.Cells[0].ForeColor = System.Drawing.Color.Brown;
                    e.Row.Cells[3].ForeColor = System.Drawing.Color.Brown;
                    e.Row.Cells[4].ForeColor = System.Drawing.Color.Brown;
                    e.Row.Cells[5].ForeColor = System.Drawing.Color.Brown;
                    e.Row.Cells[6].ForeColor = System.Drawing.Color.Brown;
                    e.Row.Cells[7].ForeColor = System.Drawing.Color.Brown;
                    e.Row.Cells[8].ForeColor = System.Drawing.Color.Brown;
                    e.Row.Cells[0].Font.Bold = true;
                    e.Row.Cells[3].Font.Bold = true;
                    e.Row.Cells[4].Font.Bold = true;
                    e.Row.Cells[5].Font.Bold = true;
                    e.Row.Cells[6].Font.Bold = true;
                    e.Row.Cells[7].Font.Bold = true;
                    e.Row.Cells[8].Font.Bold = true;
                }
                else if (MainName == "Expenditure" || MainName == "Income" || MainName == "Liabilities" || MainName == "Assets")
                {
                    e.Row.Cells[0].ForeColor = System.Drawing.Color.Blue;
                    e.Row.Cells[0].Font.Bold = true;
                }
            }
        }
        protected void NavigationLinkPeriod_Click(Object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "First":
                    pageindex = 0;
                    break;
                case "Next":
                    pageindex = int.Parse(CurrentPagePeriod.Value) + 1;
                    break;
                case "Prev":
                    pageindex = int.Parse(CurrentPagePeriod.Value) - 1;
                    break;
                case "Last":
                    pageindex = int.Parse(TotalPagesPeriod.Value);
                    break;
                default:
                    pageindex = int.Parse(e.CommandName.ToString());
                    break;
            }
            FillPeriodGrid();
        }
        private void RenderHeader(HtmlTextWriter output, System.Web.UI.Control container)
        {
            for (int i = 0; i < container.Controls.Count; i++)
            {
                TableCell cell = (TableCell)container.Controls[i];
                //stretch non merged columns for two rows
                if (!info.MergedColumns.Contains(i))
                {
                    cell.Attributes["rowspan"] = "2";
                    cell.RenderControl(output);
                }
                else //render merged columns common title
                    if (info.StartColumns.Contains(i))
                    {
                        output.Write(string.Format("<th align='center' colspan='{0}'>{1}</th>",
                                 info.StartColumns[i], info.Titles[i]));
                    }
            }

            //close the first row	
            output.RenderEndTag();
            //set attributes for the second row
            grdPeriod.HeaderStyle.AddAttributesToRender(output);
            //start the second row
            output.RenderBeginTag("tr");

            //render the second row (only the merged columns)
            for (int i = 0; i < info.MergedColumns.Count; i++)
            {
                TableCell cell = (TableCell)container.Controls[info.MergedColumns[i]];
                cell.RenderControl(output);
            }
        }
        private MergedColumnsInfo info
        {
            get
            {
                if (ViewState["info"] == null)
                    ViewState["info"] = new MergedColumnsInfo();
                return (MergedColumnsInfo)ViewState["info"];
            }
        }
        [Serializable]
        private class MergedColumnsInfo
        {
            // indexes of merged columns
            public List<int> MergedColumns = new List<int>();
            // key-value pairs: key = the first column index, value = number of the merged columns
            public Hashtable StartColumns = new Hashtable();
            // key-value pairs: key = the first column index, value = common title of the merged columns 
            public Hashtable Titles = new Hashtable();

            //parameters: the merged columns indexes, common title of the merged columns 
            public void AddMergedColumns(int[] columnsIndexes, string title)
            {
                MergedColumns.AddRange(columnsIndexes);
                StartColumns.Add(columnsIndexes[0], columnsIndexes.Length);
                Titles.Add(columnsIndexes[0], title);
            }
        }

        protected void ddlExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Checking == "a")
            {
                DataTable dtGTrial = (DataTable)ViewState["dtAsonAssetLiability"];
                if (dtGTrial != null)
                {
                    DataRow DrRow = dtGTrial.NewRow();
                    dtGTrial.Rows.Add(DrRow);
                    DataRow newRow = dtGTrial.NewRow();
                    newRow[0] = "Total";
                    if (Convert.ToDecimal(ViewState["SumDr"].ToString()) != 0)
                        newRow[2] = oconverter.formatmoneyinUs(Convert.ToDecimal(ViewState["SumDr"].ToString()));
                    if (Convert.ToDecimal(ViewState["SumCr"].ToString()) != 0)
                        newRow[3] = oconverter.formatmoneyinUs(Convert.ToDecimal(ViewState["SumCr"].ToString()));
                    dtGTrial.Rows.Add(newRow);

                    DataRow[] reportRow = dtGTrial.Select("accountsledger_mainaccountid='Assets'");
                    if (reportRow.Length > 0)
                        reportRow[0][1] = "Test";
                    DataRow[] reportRow1 = dtGTrial.Select("accountsledger_mainaccountid='Liabilities'");
                    if (reportRow1.Length > 0)
                        reportRow1[0][1] = "Test";
                    DataRow[] reportRow2 = dtGTrial.Select("accountsledger_mainaccountid='Income'");
                    if (reportRow2.Length > 0)
                        reportRow2[0][1] = "Test";
                    DataRow[] reportRow3 = dtGTrial.Select("accountsledger_mainaccountid='Expenditure'");
                    if (reportRow3.Length > 0)
                        reportRow3[0][1] = "Test";

                    dtGTrial.Columns[0].ColumnName = "Main Account";
                    dtGTrial.Columns[1].ColumnName = "Account Code";

                    dtGTrial.Columns.Remove("Debit");
                    dtGTrial.Columns.Remove("Credit");
                    //dtGTrial.Columns.Remove("AmountDr");
                    //dtGTrial.Columns.Remove("AmountCr");
                    dtGTrial.Columns.Remove("ID");
                    dtGTrial.Columns.Remove("SubLedgerType");
                    dtGTrial.Columns.Remove("DR");
                    dtGTrial.Columns.Remove("CR");
                    dtGTrial.Columns.Remove("AccType");
                    dtGTrial.Columns.Remove("CashBankType");

                    DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
                    DataTable dtReportHeader = new DataTable();
                    dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
                    DataRow HeaderRow = dtReportHeader.NewRow();
                    HeaderRow[0] = CompanyName.Rows[0][0].ToString();
                    dtReportHeader.Rows.Add(HeaderRow);
                    DataRow DrRowR1 = dtReportHeader.NewRow();
                    DrRowR1[0] = "General Trial As On Date [" + oconverter.ArrangeDate2(dtDate.Value.ToString()) + "]";
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
                    if (ddlExport.SelectedItem.Value == "E")
                    {
                        //oconverter.ExcelImport(dtBilling, "Daily Billing");
                        objExcel.ExportToExcelforExcel(dtGTrial, "General Trial", "Total", dtReportHeader, dtReportFooter);
                    }
                    else if (ddlExport.SelectedItem.Value == "P")
                    {
                        objExcel.ExportToPDF(dtGTrial, "General Trial", "Total", dtReportHeader, dtReportFooter);
                    }
                }
            }
            else if (Checking == "b")
            {

                DataTable DtGAllGrid = (DataTable)ViewState["dtPeriod"];

                DtGAllGrid.Columns.Remove("SubLedgerType");
                DtGAllGrid.Columns.Remove("AccountType");
                DtGAllGrid.Columns.Remove("CashBankType");
                DtGAllGrid.Columns.Remove("ID");
                DtGAllGrid.Columns[0].ColumnName = "Main Account";
                DtGAllGrid.Columns[1].ColumnName = "Account Code";
                //DataRow DrRow = DtGAllGrid.NewRow();
                //DtGAllGrid.Rows.Add(DrRow);
                //DataRow newRow = DtGAllGrid.NewRow();
                //newRow[0] = "Total";
                //if (Convert.ToDecimal(ViewState["SumOpeningDr"].ToString()) != 0)
                //    newRow[17] = oconverter.formatmoneyinUs(Convert.ToDecimal(ViewState["SumOpeningDr"].ToString()));
                //if (Convert.ToDecimal(ViewState["SumOpeningCr"].ToString()) != 0)
                //    newRow[18] = oconverter.formatmoneyinUs(Convert.ToDecimal(ViewState["SumOpeningCr"].ToString()));
                //if (Convert.ToDecimal(ViewState["SumAmountDr"].ToString()) != 0)
                //    newRow[19] = oconverter.formatmoneyinUs(Convert.ToDecimal(ViewState["SumAmountDr"].ToString()));
                //if (Convert.ToDecimal(ViewState["SumAmountCr"].ToString()) != 0)
                //    newRow[20] = oconverter.formatmoneyinUs(Convert.ToDecimal(ViewState["SumAmountCr"].ToString()));                      
                //if (Convert.ToDecimal(ViewState["SumClosingDr"].ToString()) != 0)
                //    newRow[21] = oconverter.formatmoneyinUs(Convert.ToDecimal(ViewState["SumClosingDr"].ToString()));
                //if (Convert.ToDecimal(ViewState["SumClosingCr"].ToString()) != 0)
                //    newRow[22] = oconverter.formatmoneyinUs(Convert.ToDecimal(ViewState["SumClosingCr"].ToString()));
                //DtGAllGrid.Rows.Add(newRow);

                //DataRow[] reportRow = DtGAllGrid.Select("accountsledger_mainaccountid='Assets'");
                //if (reportRow.Length > 0)
                //    reportRow[0][1] = "Test";
                //DataRow[] reportRow1 = DtGAllGrid.Select("accountsledger_mainaccountid='Liabilities'");
                //if (reportRow1.Length > 0)
                //    reportRow1[0][1] = "Test";
                //DataRow[] reportRow2 = DtGAllGrid.Select("accountsledger_mainaccountid='Income'");
                //if (reportRow2.Length > 0)
                //    reportRow2[0][1] = "Test";
                //DataRow[] reportRow3 = DtGAllGrid.Select("accountsledger_mainaccountid='Expenditure'");
                //if (reportRow3.Length > 0)
                //    reportRow3[0][1] = "Test";

                //DtGAllGrid.Columns.Remove("OpeningDr");
                //DtGAllGrid.Columns.Remove("OpeningCr");
                //DtGAllGrid.Columns.Remove("AmountDr");
                //DtGAllGrid.Columns.Remove("AmountCr");
                //DtGAllGrid.Columns.Remove("ClosingDr");
                //DtGAllGrid.Columns.Remove("ClosingCr");
                //DtGAllGrid.Columns.Remove("ID");
                //DtGAllGrid.Columns.Remove("SubLedgerType");
                //DtGAllGrid.Columns.Remove("AmtDr");
                //DtGAllGrid.Columns.Remove("AmtCr");
                //DtGAllGrid.Columns.Remove("opnDr");
                //DtGAllGrid.Columns.Remove("opnCr");
                //DtGAllGrid.Columns.Remove("ClsDr");
                //DtGAllGrid.Columns.Remove("ClsCr");
                //DtGAllGrid.Columns.Remove("AccType");

                //DtGAllGrid.Columns[0].ColumnName = "MainAccount";
                //DtGAllGrid.Columns[1].ColumnName = "MainAccountCode";
                //DtGAllGrid.Columns[2].ColumnName = "Opening Debit";
                //DtGAllGrid.Columns[3].ColumnName = "Opening Credit";
                //DtGAllGrid.Columns[4].ColumnName = "Amount Debit";
                //DtGAllGrid.Columns[5].ColumnName = "Amount Credit";
                //DtGAllGrid.Columns[6].ColumnName = "Closing Debit";
                //DtGAllGrid.Columns[7].ColumnName = "Closing Credit";

                DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
                DataTable dtReportHeader = new DataTable();
                dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
                DataRow HeaderRow = dtReportHeader.NewRow();
                HeaderRow[0] = CompanyName.Rows[0][0].ToString();
                dtReportHeader.Rows.Add(HeaderRow);
                DataRow DrRowR1 = dtReportHeader.NewRow();
                DrRowR1[0] = "General Trial Period [" + oconverter.ArrangeDate2(dtFrom.Value.ToString()) + " To " + oconverter.ArrangeDate2(dtTo.Value.ToString()) + "]";
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

                if (ddlExport.SelectedItem.Value == "E")
                {
                    //oconverter.ExcelImport(dtBilling, "Daily Billing");
                    objExcel.ExportToExcelforExcel(DtGAllGrid, "General Trial", "Total", dtReportHeader, dtReportFooter);
                }
                else if (ddlExport.SelectedItem.Value == "P")
                {
                    objExcel.ExportToPDF(DtGAllGrid, "General Trial", "Total", dtReportHeader, dtReportFooter);
                }
            }
            Checking = null;
        }


        #region DevExpress Report Normal General Trial
        protected void aspxGdGeneralTrial_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
              string Spantext = "General Trial As On Date :";// +"  ( From ) : " + oconverter.ArrangeDate2(dtDate.Value.ToString()) + "  ( To ) : " + oconverter.ArrangeDate2(dtToDate.Value.ToString()) + "";
                string SpanText1 = oconverter.ArrangeDate2(dtDate.Value.ToString());
                ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct", "ShowHideAsOn('" + Spantext + "','" + SpanText1 + "')", true);

                Checking = "a";
                FillGrid();
                dtPeriod = new DataTable();
                grdPeriod.DataSource = dtPeriod;
                grdPeriod.DataBind();
                if (ViewState["dtSubsidiary"] != null)
                    dtSubsidiary = (DataTable)ViewState["dtSubsidiary"];
                if (dtSubsidiary.Rows.Count == 0)
                {
                    
                    aspxGdGeneralTrial.DataSource = dtSubsidiary;
                    aspxGdGeneralTrial.DataBind();
                    Session["RptGeneralTrial"] = dtSubsidiary;

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct", "alert('No Record Found!!');", true);
                } 
            
        
        
        }

        protected void grid_DataBinding(object sender, EventArgs e)
        {
            if (Session["RptGeneralTrial"] != null)
            {
                aspxGdGeneralTrial.DataSource = (DataTable)Session["RptGeneralTrial"];
            }


        }

        protected void aspxGdGeneralTrial_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data) return;
            if (e.GetValue("accountsledger_mainaccountid") == "Total")
            {
                e.Row.BackColor = System.Drawing.Color.FromArgb(0xE3, 0xBA, 0xBA);
                e.Row.Font.Bold = true;
                return;
            }
            //Row brfore Total
            if (Convert.ToString(e.GetValue("accountsledger_mainaccountid")).Trim() == "")
            {
                e.Row.CssClass = "hide";
            }

            //Set Color For heading like assets,Liability
            if ((e.GetValue("accountsledger_mainaccountid") == "Assets" || e.GetValue("accountsledger_mainaccountid") == "Liabilities" || e.GetValue("accountsledger_mainaccountid") == "Expenditure" || e.GetValue("accountsledger_mainaccountid") == "Income") && Convert.ToString(e.GetValue("MainAccID")).Trim() == "")
            {
                e.Row.BackColor = System.Drawing.Color.FromArgb(0x8C, 0xAF, 0x9F);
                e.Row.Font.Bold = true;
                return;
            }

            //For PopUp view cash bank 
            string ZeroBal = "";
            if (chkZero.Checked == true)
            {
                ZeroBal = "Y";
            }
            else
            {
                ZeroBal = "N";

            }
            string MainAcc = Convert.ToString(e.GetValue("ID"));
            string SubLedgerType = Convert.ToString(e.GetValue("SubLedgerType"));
            string CashBankType = Convert.ToString(e.GetValue("CashBankType"));
            string Main = Convert.ToString(e.GetValue("accountsledger_mainaccountid"));  
            DateTime date = Convert.ToDateTime(dtDate.Value);
            DateTime Tdate = Convert.ToDateTime(dtTo.Value);
            if (SubLedgerType  != "None")
            {
                e.Row.Cells[2].Style.Add("cursor", "pointer");
                e.Row.Cells[2].Attributes.Add("onclick", "javascript:ShowGeneralTrialDetail('" + MainAcc + "','" + date + "','" + Main + "','1','" + date + "','A','" + SubLedgerType + "','" + ZeroBal + "');");
                e.Row.Cells[2].Text = "View";
                e.Row.Cells[2].Style.Add("color", "blue");
            }
            if (SubLedgerType == "None")
            {
                if (CashBankType == "Bank" || CashBankType == "Cash")
                {
                    e.Row.Cells[0].Style.Add("cursor", "pointer");
                    e.Row.Cells[0].Attributes.Add("onclick", "javascript:ShowCashBankDetail('" + MainAcc + "','" + date + "','" + Main + "','1','" + date + "','A','" + SubLedgerType+ "');");
                    e.Row.Cells[0].ToolTip = "Click To View CashBank Details";
                }
                else
                {
                    e.Row.Cells[0].Style.Add("cursor", "pointer");
                    e.Row.Cells[0].Attributes.Add("onclick", "javascript:ShowLedger('" + MainAcc + "','GeneralTrial','1','" + Main + "','SubAccName','UCC','" + date + "');");
                    e.Row.Cells[0].ToolTip = "Click To View Ledger View Details";

                }

            }
        }



        protected void aspxPeriodGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string Spantext = " General Trial For Period :";// +"  ( From ) : " + oconverter.ArrangeDate2(dtDate.Value.ToString()) + "  ( To ) : " + oconverter.ArrangeDate2(dtToDate.Value.ToString()) + "";
            string SpanText1 = oconverter.ArrangeDate2(dtFrom.Value.ToString()) + "  - " + oconverter.ArrangeDate2(dtTo.Value.ToString());
            ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct", "ShowHidePeriod('" + Spantext + "','" + SpanText1 + "')", true);


            Checking = "b";
            FillPeriodGrid();
        
        }
        protected void aspxPeriodGrid_DataBinding(object sender, EventArgs e)
        {
            if (Session["RptGeneralTrialPeriod"] != null)
            {
                aspxPeriodGrid.DataSource = (DataTable)Session["RptGeneralTrialPeriod"];
            }
        }

        protected void aspxPeriodGrid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data) return;
            if (e.GetValue("accountsledger_mainaccountid") == "Total")
            {
                e.Row.BackColor = System.Drawing.Color.FromArgb(0xE3, 0xBA, 0xBA);
                e.Row.Font.Bold = true;
            }
            //Row brfore Total
            if (Convert.ToString(e.GetValue("accountsledger_mainaccountid")).Trim() == "")
            {
                e.Row.CssClass = "hide";
            }

            //Set Color For heading like assets,Liability
            if ((e.GetValue("accountsledger_mainaccountid") == "Assets" || e.GetValue("accountsledger_mainaccountid") == "Liabilities" || e.GetValue("accountsledger_mainaccountid") == "Expenditure" || e.GetValue("accountsledger_mainaccountid") == "Income") && Convert.ToString(e.GetValue("MainAccID")).Trim() == "")
            {
                e.Row.BackColor = System.Drawing.Color.FromArgb(0x8C, 0xAF, 0x9F);
                e.Row.Font.Bold = true;
                return;
            }

            string ZeroBal = "";
            if (chkZero.Checked == true)
            {
                ZeroBal = "Y";
            }
            else
            {
                ZeroBal = "N";

            }

            //Show cash Bank Popup for period report

                string MainAcc = Convert.ToString(e.GetValue("ID"));
                string SubLedgerType = Convert.ToString(e.GetValue("SubLedgerType"));
                string CashBankType = Convert.ToString(e.GetValue("CashBankType"));
                string Main = Convert.ToString(e.GetValue("accountsledger_mainaccountid"));
                DateTime Fdate = Convert.ToDateTime(dtFrom.Value);
                DateTime date = Convert.ToDateTime(dtDate.Value);
                DateTime Tdate = Convert.ToDateTime(dtTo.Value); 

                if (SubLedgerType != "None")
                {
                    e.Row.Cells[2].Style.Add("cursor", "pointer");
                    e.Row.Cells[2].Attributes.Add("onclick", "javascript:ShowGeneralTrialDetail('" + MainAcc + "','" + Fdate + "','" + Main + "','1','" + Tdate + "','P','" + SubLedgerType + "','" + ZeroBal + "');");
                    e.Row.Cells[2].Text = "View";
                    e.Row.Cells[2].Style.Add("color", "blue");
                    e.Row.Cells[2].ToolTip = "Click To View Sub Acount";
                }
                if (SubLedgerType == "None")
                {
                    if (CashBankType == "Bank" || CashBankType== "Cash")
                    {
                        e.Row.Cells[0].Style.Add("cursor", "pointer");
                        e.Row.Cells[0].Attributes.Add("onclick", "javascript:ShowCashBankDetail('" + MainAcc + "','" + Fdate + "','" + Main + "','1','" + Tdate + "','P','" + SubLedgerType + "');");
                        e.Row.Cells[0].ToolTip = "Click To View CashBank Details";
                    }
                    else
                    {
                        e.Row.Cells[0].Style.Add("cursor", "pointer");
                        e.Row.Cells[0].Attributes.Add("onclick", "javascript:ShowLedger('" + MainAcc + "','GeneralTrial','1','" + Main + "','SubAccName','UCC','" + Tdate + "');");
                        e.Row.Cells[0].ToolTip = "Click To View Ledger View Details";

                    }

                }



        }


        public void bindexport(int Filter)
        {
            string filename = "";
            string FileHeader = "";
            string strPeriod = (IsPeriod.Value == "") ? "false" : IsPeriod.Value;

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();


            if (Convert.ToBoolean(strPeriod))
            {
                filename = "GeneralTrial Period";
                exporter.GridViewID = "aspxPeriodGrid";

                FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true,true, true, true) + Environment.NewLine + "GeneralTrial Period" + Environment.NewLine + "For the period " + Convert.ToDateTime(dtFrom.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(dtTo.Date).ToString("dd-MM-yyyy");

            }
            else
            {
                filename = "GeneralTrial As On Date";
                exporter.GridViewID = "aspxGdGeneralTrial";

                FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true,true, true, true) + Environment.NewLine + "GeneralTrial As On Date" + Environment.NewLine + "As on  " + Convert.ToDateTime(dtTo.Date).ToString("dd-MM-yyyy");


            }
            exporter.FileName = filename;
            exporter.PageHeader.Left = FileHeader;
            exporter.PageHeader.Font.Size = 10;
            exporter.PageHeader.Font.Name = "Tahoma";


            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";

            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();
                    break;
                case 2:
                    exporter.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
                    break;
                case 3:
                    exporter.WriteRtfToResponse();
                    break;
                case 4:
                    exporter.WriteCsvToResponse();
                    break;
            }
        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                if (Session["exportval"] == null)
                {
                    Session["exportval"] = Filter;
                    bindexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportval"]) != Filter)
                {
                    Session["exportval"] = Filter;
                    bindexport(Filter);
                }
            }
        }


        #endregion

        protected void listBox_Init(object sender, EventArgs e)
        {
            // Branch List

            DataTable dt = oDBEngine.GetDataTable(" (Select '0' as branch_id,'-All-' as branch_description Union SELECT branch_id,branch_description FROM tbl_master_Branch) as tbl", " branch_id,branch_description ", " branch_description <>'' Order By branch_description asc  ");
            ASPxListBox lb = sender as ASPxListBox;
            lb.DataSource = dt;
            lb.ValueField = "branch_id";
            lb.TextField = "branch_description";
            lb.ValueType = typeof(string);
            lb.DataBind();

            // Branch List
        }
    }
}