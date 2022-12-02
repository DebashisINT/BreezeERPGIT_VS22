using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management
{
    public partial class Reports_frmReport_IframeSubsidiaryTrial : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        int pageindex = 0;
        int pagecount = 0;
        int pageSize;
        int rowcount = 0;
        string data;
        string BranchId = null;
        string Clients;
        string Group = null;
        string MainAcc = null;
        static string CompanyID = null;
        string SegmentID = null;
        String LinkFirst;
        String LinkPrev;
        String LinkNext;
        String LinkLast;
        DataTable dtSubsidiary = new DataTable();
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.Management_BL oManagement_BL = new BusinessLogicLayer.Management_BL();
        ExcelFile objExcel = new ExcelFile();
        string SegMentName;
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
               //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (!IsPostBack)
            {
                MainAcc = null;
                SegmentID = null;
                BranchId = null;
                CompanyID = null;
                DataTable DtSegComp = new DataTable();
                DataTable dtSeg = oDBEngine.GetDataTable("tbl_master_segment", "seg_name", " seg_id=" + Session["userlastsegment"].ToString() + "");
                if (dtSeg.Rows[0][0].ToString().EndsWith("CM"))
                    DtSegComp = oDBEngine.GetDataTable("(select exch_compId,exch_internalId,isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Comp from tbl_master_companyExchange where exch_compid in (select top 1 ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment in(" + Session["userallsegmentnotonlyLast"].ToString() + "))) as D", "*", "Comp in(select seg_name from tbl_master_segment where seg_id in(" + Session["userallsegmentnotonlyLast"].ToString() + "))  and Comp like '%CM' and exch_compID='" + Session["LastCompany"].ToString() + "'");
                else
                    DtSegComp = oDBEngine.GetDataTable("(select exch_compId,exch_internalId,isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Comp from tbl_master_companyExchange where exch_compid in (select top 1 ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment=" + Session["userlastsegment"].ToString() + ")) as D", "*", "Comp in(select seg_name from tbl_master_segment where seg_id=" + Session["userlastsegment"].ToString() + ")");
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
                    ViewState["SegmentID"] = SegmentID;
                    Span2.InnerText = SegMentName;
                }
                if (Request.QueryString["mainacc"] != null)
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "JS", "<script language='javascript'>FromGeneralLedger();</script>");
                    FillGrid();
                }
                else
                {
                    string[] FinYear = Session["LastFinYear"].ToString().Split('-');
                    dtDate.EditFormatString = oconverter.GetDateFormat("Date");
                    dtFrom.EditFormatString = oconverter.GetDateFormat("Date");
                    dtTo.EditFormatString = oconverter.GetDateFormat("Date");
                    dtDate.Value = Convert.ToDateTime("03/31/" + FinYear[1].ToString());
                    dtFrom.Value = Convert.ToDateTime("04/01/" + FinYear[0].ToString());
                    dtTo.Value = Convert.ToDateTime("03/31/" + FinYear[1].ToString());
                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");
                    //txtsubscriptionID.Attributes.Add("onkeyup", "ShowMainAccountName(this,'SelectMainAccountName',event)");
                    //txtsubscriptionID.Attributes.Add("onkeyup", "ShowMainAccountName(this,'SearchMainAccountBranchSegment',event)");
                    //rdbMainAll.Attributes.Add("OnClick", "MainAll('all','MainAcc')");
                    //rdbMainSelected.Attributes.Add("OnClick", "MainAll('Selc','MainAcc')");
                    //rdAllBranch.Attributes.Add("onclick", "MainAll('all','Branch')");
                    //rdSelBranch.Attributes.Add("onclick", "MainAll('Selc','Branch')");
                    rdAllSegment.Attributes.Add("onclick", "MainAll('all','Segment')");
                    rdSelSegment.Attributes.Add("onclick", "MainAll('Selc','Segment')");
                }

            }
            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            //___________-end here___//
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
            if (idlist[0] == "MainAcc")
            {
                MainAcc = str;
                data = "MainAcc~" + str;
            }
            if (idlist[0] == "Clients")
            {
                Clients = str;
                data = "Clients~" + str;
            }

            else if (idlist[0] == "Group")
            {
                Group = str;
                data = "Group~" + str;
            }
            else if (idlist[0] == "Branch")
            {
                BranchId = str;
                data = "Branch~" + str;
            }


        }
        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
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
        protected void btnShow_Click(object sender, EventArgs e)
        {
            if (RadAsOnDate.Checked == true)
            {
                FillGrid();
                //ScriptManager.RegisterStartupScript(this, GetType(), "JS", "ShowGrid();", true);
            }
            else
            {
                FillGridForPeriod();
                //ScriptManager.RegisterStartupScript(this, GetType(), "JS", "ShowGrid1();", true);
            }
        }
        public void FillGrid()
        {
            try
            {
                if (Session["userlastsegment"].ToString() == "9" || Session["userlastsegment"].ToString() == "10")
                    fn_ClientCDSL();
                else
                    fn_Client();
                string BranchName = null;
                string ForGroup = null;
                BranchId = HdnBranchId.Value;
                Group = HdnGroup.Value;
                MainAcc = HdnMainAcc.Value;
                if (ViewState["Clients"] != null)
                    Clients = ViewState["Clients"].ToString();
                DataTable dtSubsidiary_New = new DataTable();
                System.Globalization.NumberFormatInfo currencyFormat = new System.Globalization.CultureInfo("hi-IN").NumberFormat;
                currencyFormat.CurrencySymbol = "";
                currencyFormat.CurrencyNegativePattern = 2;
                pageSize = 150000;
                string WehereMainAccount = null;
                string WhereMainBranch = null;
                DateTime date;
                decimal SumForBranchDR = 0;
                decimal SumForBranchCR = 0;
                decimal DifOfDRCR = 0;
                string[] ClientValue = null;
                if (rdbMainSelected.Checked == true)
                {
                    ClientValue = cmbclientsPager.SelectedItem.Value.ToString().Split('~');
                    ViewState["SubType"] = ClientValue[1].ToString().Trim();
                }
                if (Request.QueryString["mainacc"] != null)
                {
                    WehereMainAccount = "and accountsledger_mainaccountid in('" + Request.QueryString["mainacc"].ToString() + "')";
                    date = Convert.ToDateTime(Request.QueryString["date"].ToString());
                    SegmentID = Request.QueryString["Segment"].ToString();
                }
                else
                {
                    if (ddlGroup.SelectedItem.Value == "0")
                    {
                        if (rdbranchAll.Checked == true)
                            WhereMainBranch = "and accountsledger_branchid in(" + Session["userbranchHierarchy"].ToString() + ")";
                        else
                            WhereMainBranch = "and accountsledger_branchid in(" + BranchId + ")";
                    }
                    if (rdbMainAll.Checked == true)
                        WehereMainAccount = null;
                    else
                        WehereMainAccount = " and accountsledger_mainaccountid in('" + ClientValue[0].ToString() + "')";
                    WehereMainAccount += " and accountsledger_subaccountid in(" + Clients + ")";

                    date = Convert.ToDateTime(dtDate.Value);
                }
                if (Request.QueryString["mainacc"] == null)
                {
                    if (rdAllSegment.Checked == true)
                    {
                        DataTable DT = oDBEngine.GetDataTable("(select A.EXCH_INTERNALID AS SEGMENTID ,isnull((TME.EXH_ShortName + '-' + A.EXCH_SEGMENTID),exch_membershipType) AS EXCHANGENAME from (SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='" + Session["LastCompany"].ToString() + "' ) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID ) as D ", "top 10 *", null);
                        SegmentID = DT.Rows[0][0].ToString();
                        for (int i = 1; i < DT.Rows.Count; i++)
                        {
                            SegmentID = SegmentID + "," + DT.Rows[i][0].ToString();
                        }
                    }
                    else
                    {
                        SegmentID = ViewState["SegmentID"].ToString();
                    }
                }
                if (Group != null && Group != "")
                    ForGroup = " ID in(" + Group + ")";
                if (rdBoth.Checked == true)
                {
                    if (rdbMainSelected.Checked == true)
                    {
                        if (ClientValue[1].ToString().Trim() == "Customers" || ClientValue[1].ToString().Trim() == "NSDL Clients" || ClientValue[1].ToString().Trim() == "CDSL Clients")
                        {
                            if (ddlGroup.SelectedItem.Value == "0")
                            {
                                dtSubsidiary = oDBEngine.GetDataTable("(Select BranchName,accountsledger_mainaccountid,accountsledger_subaccountid,case when UCC='0' then null else UCC end as Ucc ,cast(AmountDr as varchar(max)) as AmountDr,cast(AmountCr as varchar(max)) as AmountCr,isnull(sum(D.AmountDr),0) as DR,isnull(sum(D.AmountCr),0) as CR,0 as ID,MainID,SubID  from (select (select mainaccount_name from master_mainaccount where mainaccount_accountcode=trans_accountsledger.accountsledger_mainaccountid) as accountsledger_mainaccountid,(isnull((select isnull(ltrim(rtrim(cnt_firstname)),'')+' '+isnull(ltrim(rtrim(cnt_middlename)),'')+' '+isnull(ltrim(rtrim(cnt_lastname)),'') from tbl_master_contact where cnt_internalid=trans_accountsledger.accountsledger_subaccountid),isnull((select top 1 subaccount_name from master_subaccount where cast(subaccount_code as varchar)=trans_accountsledger.accountsledger_subaccountid and SubAccount_MainAcReferenceID=trans_accountsledger.accountsledger_mainaccountid),isnull((select subaccount_name from master_subaccount where cast(subaccount_referenceid as varchar)=trans_accountsledger.accountsledger_subaccountid),isnull((select top 1 cdslclients_firstholdername from master_cdslclients where cast(cdslclients_benaccountnumber as varchar)=trans_accountsledger.accountsledger_subaccountid),(select nsdlclients_benfirstholdername from master_nsdlclients where cast(nsdlclients_benaccountid as varchar)=trans_accountsledger.accountsledger_subaccountid)))))) as accountsledger_subaccountid,(isnull((select isnull(ltrim(rtrim(cnt_ucc)),'') from tbl_master_contact where cnt_internalid=trans_accountsledger.accountsledger_subaccountid),isnull((select SubAccount_Code from master_subaccount where cast(SubAccount_ReferenceID as varchar)=cast(trans_accountsledger.accountsledger_subaccountid as varchar) and SubAccount_MainAcReferenceID=trans_accountsledger.accountsledger_mainaccountid),isnull((select top 1 nsdlclients_benaccountid from master_nsdlclients where cast(nsdlclients_benaccountid as varchar)=trans_accountsledger.accountsledger_subaccountid),(select top 1 cdslclients_benaccountnumber from master_cdslclients where cast(cdslclients_benaccountnumber as varchar)=trans_accountsledger.accountsledger_subaccountid))))) as UCC,case when (sum(accountsledger_amountdr)-sum(accountsledger_amountcr))>0 then sum(accountsledger_amountdr)-sum(accountsledger_amountcr) else null end as AmountDR,case when (sum(accountsledger_amountdr)-sum(accountsledger_amountcr))<0 then ((-1)*(sum(accountsledger_amountdr)-sum(accountsledger_amountcr))) else null end as AmountCR,(select rtrim(branch_description)+' ['+rtrim(branch_code)+']' from tbl_master_branch where branch_id=accountsledger_branchID) as BranchName,accountsledger_mainaccountid as MainID,accountsledger_subaccountid as SubID  from trans_accountsledger WHERE  accountsledger_subaccountid is not null and accountsledger_subaccountid<>'' and accountsledger_transactiondate<='" + date + "' " + WehereMainAccount + " and accountsledger_exchangesegmentid in(" + SegmentID + ") and accountsledger_companyid='" + CompanyID + "'" + WhereMainBranch + " and accountsledger_FinYear='" + Session["LastFinYear"].ToString() + "' group By  accountsledger_mainaccountid,accountsledger_subaccountid,accountsledger_branchID ) as D group By  accountsledger_mainaccountid,accountsledger_subaccountid,AmountDr,AmountCr,Ucc,BranchName,MainID,SubID ) as K ", "* ", " DR+CR<>0", "BranchName,accountsledger_mainaccountid,accountsledger_subaccountid ");
                            }
                            else
                            {
                                if (Session["userlastsegment"].ToString() == "10" || Session["userlastsegment"].ToString() == "9")
                                    dtSubsidiary = oDBEngine.GetDataTable("(Select *  from (Select BranchName,accountsledger_mainaccountid,accountsledger_subaccountid,case when UCC='0' then null else UCC end as Ucc ,cast(AmountDr as varchar(max)) as AmountDr,cast(AmountCr as varchar(max)) as AmountCr,isnull(sum(D.AmountDr),0) as DR,isnull(sum(D.AmountCr),0) as CR,ID,MainID,SubID  from (select (select mainaccount_name from master_mainaccount where mainaccount_accountcode=trans_accountsledger.accountsledger_mainaccountid) as accountsledger_mainaccountid,(isnull((select isnull(ltrim(rtrim(cnt_firstname)),'')+' '+isnull(ltrim(rtrim(cnt_middlename)),'')+' '+isnull(ltrim(rtrim(cnt_lastname)),'') from tbl_master_contact where cnt_internalid=trans_accountsledger.accountsledger_subaccountid),isnull((select top 1 subaccount_name from master_subaccount where cast(subaccount_code as varchar)=trans_accountsledger.accountsledger_subaccountid and SubAccount_MainAcReferenceID=trans_accountsledger.accountsledger_mainaccountid),isnull((select subaccount_name from master_subaccount where cast(subaccount_referenceid as varchar)=trans_accountsledger.accountsledger_subaccountid),isnull((select top 1 cdslclients_firstholdername from master_cdslclients where cast(cdslclients_benaccountnumber as varchar)=trans_accountsledger.accountsledger_subaccountid),(select nsdlclients_benfirstholdername from master_nsdlclients where cast(nsdlclients_benaccountid as varchar)=trans_accountsledger.accountsledger_subaccountid)))))) as accountsledger_subaccountid,(isnull((select isnull(ltrim(rtrim(cnt_ucc)),'') from tbl_master_contact where cnt_internalid=trans_accountsledger.accountsledger_subaccountid),isnull((select SubAccount_Code from master_subaccount where cast(SubAccount_ReferenceID as varchar)=cast(trans_accountsledger.accountsledger_subaccountid as varchar) and SubAccount_MainAcReferenceID=trans_accountsledger.accountsledger_mainaccountid),isnull((select top 1 nsdlclients_benaccountid from master_nsdlclients where cast(nsdlclients_benaccountid as varchar)=trans_accountsledger.accountsledger_subaccountid),(select top 1 cdslclients_benaccountnumber from master_cdslclients where cast(cdslclients_benaccountnumber as varchar)=trans_accountsledger.accountsledger_subaccountid))))) as UCC,case when (sum(accountsledger_amountdr)-sum(accountsledger_amountcr))>0 then sum(accountsledger_amountdr)-sum(accountsledger_amountcr) else null end as AmountDR,	case when (sum(accountsledger_amountdr)-sum(accountsledger_amountcr))<0 then ((-1)*(sum(accountsledger_amountdr)-sum(accountsledger_amountcr))) else null end as AmountCR,	(select top 1 gpm_Description from tbl_master_groupmaster where gpm_id in(select grp_groupMaster from tbl_trans_group where substring(grp_contactid,9,8)=accountsledger_SubAccountID) and gpm_type=ltrim('" + ddlgrouptype.SelectedItem.Value + "')) as BranchName,	(select top 1 gpm_ID from tbl_master_groupmaster where gpm_id in(select grp_groupMaster from tbl_trans_group where substring(grp_contactid,9,8)=accountsledger_SubAccountID) and gpm_type=ltrim('" + ddlgrouptype.SelectedItem.Value + "')) as ID,accountsledger_mainaccountid as MainID,accountsledger_subaccountid as SubID  from trans_accountsledger WHERE  accountsledger_subaccountid is not null and accountsledger_subaccountid<>'' and accountsledger_transactiondate<='" + date + "' " + WehereMainAccount + " and accountsledger_exchangesegmentid in(" + SegmentID + ") and accountsledger_companyid='" + CompanyID + "' " + WhereMainBranch + " and accountsledger_FinYear='" + Session["LastFinYear"].ToString() + "' group By  accountsledger_mainaccountid,accountsledger_subaccountid,accountsledger_branchID ) as D group By  accountsledger_mainaccountid,accountsledger_subaccountid,AmountDr,AmountCr,Ucc,BranchName,ID,MainID,SubID) as K  WHERE  DR+CR<>0 ) as DD ", "*", ForGroup, " BranchName,accountsledger_mainaccountid,accountsledger_subaccountid");
                                else
                                    dtSubsidiary = oDBEngine.GetDataTable("(Select *  from (Select BranchName,accountsledger_mainaccountid,accountsledger_subaccountid,case when UCC='0' then null else UCC end as Ucc ,cast(AmountDr as varchar(max)) as AmountDr,cast(AmountCr as varchar(max)) as AmountCr,isnull(sum(D.AmountDr),0) as DR,isnull(sum(D.AmountCr),0) as CR,ID,MainID,SubID  from (select (select mainaccount_name from master_mainaccount where mainaccount_accountcode=trans_accountsledger.accountsledger_mainaccountid) as accountsledger_mainaccountid,(isnull((select isnull(ltrim(rtrim(cnt_firstname)),'')+' '+isnull(ltrim(rtrim(cnt_middlename)),'')+' '+isnull(ltrim(rtrim(cnt_lastname)),'') from tbl_master_contact where cnt_internalid=trans_accountsledger.accountsledger_subaccountid),isnull((select top 1 subaccount_name from master_subaccount where cast(subaccount_code as varchar)=trans_accountsledger.accountsledger_subaccountid and SubAccount_MainAcReferenceID=trans_accountsledger.accountsledger_mainaccountid),isnull((select subaccount_name from master_subaccount where cast(subaccount_referenceid as varchar)=trans_accountsledger.accountsledger_subaccountid),isnull((select top 1 cdslclients_firstholdername from master_cdslclients where cast(cdslclients_benaccountnumber as varchar)=trans_accountsledger.accountsledger_subaccountid),(select nsdlclients_benfirstholdername from master_nsdlclients where cast(nsdlclients_benaccountid as varchar)=trans_accountsledger.accountsledger_subaccountid)))))) as accountsledger_subaccountid,(isnull((select isnull(ltrim(rtrim(cnt_ucc)),'') from tbl_master_contact where cnt_internalid=trans_accountsledger.accountsledger_subaccountid),isnull((select SubAccount_Code from master_subaccount where cast(SubAccount_ReferenceID as varchar)=cast(trans_accountsledger.accountsledger_subaccountid as varchar) and SubAccount_MainAcReferenceID=trans_accountsledger.accountsledger_mainaccountid),isnull((select top 1 nsdlclients_benaccountid from master_nsdlclients where cast(nsdlclients_benaccountid as varchar)=trans_accountsledger.accountsledger_subaccountid),(select top 1 cdslclients_benaccountnumber from master_cdslclients where cast(cdslclients_benaccountnumber as varchar)=trans_accountsledger.accountsledger_subaccountid))))) as UCC,case when (sum(accountsledger_amountdr)-sum(accountsledger_amountcr))>0 then sum(accountsledger_amountdr)-sum(accountsledger_amountcr) else null end as AmountDR,	case when (sum(accountsledger_amountdr)-sum(accountsledger_amountcr))<0 then ((-1)*(sum(accountsledger_amountdr)-sum(accountsledger_amountcr))) else null end as AmountCR,	(select top 1 gpm_Description from tbl_master_groupmaster where gpm_id in(select grp_groupMaster from tbl_trans_group where grp_contactid=accountsledger_SubAccountID) and gpm_type=ltrim('" + ddlgrouptype.SelectedItem.Value + "')) as BranchName,	(select top 1 gpm_ID from tbl_master_groupmaster where gpm_id in(select grp_groupMaster from tbl_trans_group where grp_contactid=accountsledger_SubAccountID) and gpm_type=ltrim('" + ddlgrouptype.SelectedItem.Value + "')) as ID,accountsledger_mainaccountid as MainID,accountsledger_subaccountid as SubID  from trans_accountsledger WHERE  accountsledger_subaccountid is not null and accountsledger_subaccountid<>'' and accountsledger_transactiondate<='" + date + "' " + WehereMainAccount + " and accountsledger_exchangesegmentid in(" + SegmentID + ") and accountsledger_companyid='" + CompanyID + "' " + WhereMainBranch + " and accountsledger_FinYear='" + Session["LastFinYear"].ToString() + "' group By  accountsledger_mainaccountid,accountsledger_subaccountid,accountsledger_branchID ) as D group By  accountsledger_mainaccountid,accountsledger_subaccountid,AmountDr,AmountCr,Ucc,BranchName,ID,MainID,SubID) as K  WHERE  DR+CR<>0 ) as DD ", "*", ForGroup, " BranchName,accountsledger_mainaccountid,accountsledger_subaccountid");
                            }
                        }
                        else
                            dtSubsidiary = oDBEngine.GetDataTable("(Select BranchName,accountsledger_mainaccountid,accountsledger_subaccountid,case when UCC='0' then null else UCC end as Ucc ,case when (isnull(sum(D.AmountDr),0)-isnull(sum(D.AmountCr),0))>0 then (isnull(sum(D.AmountDr),0)-isnull(sum(D.AmountCr),0)) else 0 end as DR,case when (isnull(sum(D.AmountCr),0)-isnull(sum(D.AmountDr),0))>0 then (isnull(sum(D.AmountCr),0)-isnull(sum(D.AmountDr),0)) else 0 end as CR,0 as ID,MainID,SubID  from (select (select mainaccount_name from master_mainaccount where mainaccount_accountcode=trans_accountsledger.accountsledger_mainaccountid) as accountsledger_mainaccountid,(isnull((select isnull(ltrim(rtrim(cnt_firstname)),'')+' '+isnull(ltrim(rtrim(cnt_middlename)),'')+' '+isnull(ltrim(rtrim(cnt_lastname)),'') from tbl_master_contact where cnt_internalid=trans_accountsledger.accountsledger_subaccountid),isnull((select top 1 subaccount_name from master_subaccount where cast(subaccount_code as varchar)=trans_accountsledger.accountsledger_subaccountid and SubAccount_MainAcReferenceID=trans_accountsledger.accountsledger_mainaccountid),isnull((select subaccount_name from master_subaccount where cast(subaccount_referenceid as varchar)=trans_accountsledger.accountsledger_subaccountid),isnull((select top 1 cdslclients_firstholdername from master_cdslclients where cast(cdslclients_benaccountnumber as varchar)=trans_accountsledger.accountsledger_subaccountid),(select nsdlclients_benfirstholdername from master_nsdlclients where cast(nsdlclients_benaccountid as varchar)=trans_accountsledger.accountsledger_subaccountid)))))) as accountsledger_subaccountid,(isnull((select isnull(ltrim(rtrim(cnt_ucc)),'') from tbl_master_contact where cnt_internalid=trans_accountsledger.accountsledger_subaccountid),isnull((select SubAccount_Code from master_subaccount where cast(SubAccount_ReferenceID as varchar)=cast(trans_accountsledger.accountsledger_subaccountid as varchar) and SubAccount_MainAcReferenceID=trans_accountsledger.accountsledger_mainaccountid),isnull((select top 1 nsdlclients_benaccountid from master_nsdlclients where cast(nsdlclients_benaccountid as varchar)=trans_accountsledger.accountsledger_subaccountid),(select top 1 cdslclients_benaccountnumber from master_cdslclients where cast(cdslclients_benaccountnumber as varchar)=trans_accountsledger.accountsledger_subaccountid))))) as UCC,case when (sum(accountsledger_amountdr)-sum(accountsledger_amountcr))>0 then sum(accountsledger_amountdr)-sum(accountsledger_amountcr) else null end as AmountDR,case when (sum(accountsledger_amountdr)-sum(accountsledger_amountcr))<0 then ((-1)*(sum(accountsledger_amountdr)-sum(accountsledger_amountcr))) else null end as AmountCR,0 as BranchName,accountsledger_mainaccountid as MainID,accountsledger_subaccountid as SubID  from trans_accountsledger WHERE  accountsledger_subaccountid is not null and accountsledger_subaccountid<>'' and accountsledger_transactiondate<='" + date + "' " + WehereMainAccount + " and accountsledger_exchangesegmentid in(" + SegmentID + ") and accountsledger_companyid='" + CompanyID + "'" + WhereMainBranch + " and accountsledger_FinYear='" + Session["LastFinYear"].ToString() + "' group By  accountsledger_mainaccountid,accountsledger_subaccountid,accountsledger_branchID ) as D group By  accountsledger_mainaccountid,accountsledger_subaccountid,Ucc,BranchName,MainID,SubID  ) as K ", "BranchName,accountsledger_mainaccountid,accountsledger_subaccountid,Ucc,case when DR=0 then null else cast(DR as varchar(max)) end as AmountDr,case when CR=0 then null else cast(CR as varchar(max)) end as AmountCr,DR,CR,MainID,SubID ", " DR+CR<>0", "BranchName,accountsledger_mainaccountid,accountsledger_subaccountid ");
                    }
                    else
                        dtSubsidiary = oDBEngine.GetDataTable("(Select BranchName,accountsledger_mainaccountid,accountsledger_subaccountid,case when UCC='0' then null else UCC end as Ucc ,cast(AmountDr as varchar(max)) as AmountDr,cast(AmountCr as varchar(max)) as AmountCr,isnull(sum(D.AmountDr),0) as DR,isnull(sum(D.AmountCr),0) as CR,0 as ID,MainID,SubID  from (select (select mainaccount_name from master_mainaccount where mainaccount_accountcode=trans_accountsledger.accountsledger_mainaccountid) as accountsledger_mainaccountid,(isnull((select isnull(ltrim(rtrim(cnt_firstname)),'')+' '+isnull(ltrim(rtrim(cnt_middlename)),'')+' '+isnull(ltrim(rtrim(cnt_lastname)),'') from tbl_master_contact where cnt_internalid=trans_accountsledger.accountsledger_subaccountid),isnull((select top 1 subaccount_name from master_subaccount where cast(subaccount_code as varchar)=trans_accountsledger.accountsledger_subaccountid and SubAccount_MainAcReferenceID=trans_accountsledger.accountsledger_mainaccountid),isnull((select subaccount_name from master_subaccount where cast(subaccount_referenceid as varchar)=trans_accountsledger.accountsledger_subaccountid),isnull((select top 1 cdslclients_firstholdername from master_cdslclients where cast(cdslclients_benaccountnumber as varchar)=trans_accountsledger.accountsledger_subaccountid),(select nsdlclients_benfirstholdername from master_nsdlclients where cast(nsdlclients_benaccountid as varchar)=trans_accountsledger.accountsledger_subaccountid)))))) as accountsledger_subaccountid,(isnull((select isnull(ltrim(rtrim(cnt_ucc)),'') from tbl_master_contact where cnt_internalid=trans_accountsledger.accountsledger_subaccountid),isnull((select SubAccount_Code from master_subaccount where cast(SubAccount_ReferenceID as varchar)=cast(trans_accountsledger.accountsledger_subaccountid as varchar) and SubAccount_MainAcReferenceID=trans_accountsledger.accountsledger_mainaccountid),isnull((select top 1 nsdlclients_benaccountid from master_nsdlclients where cast(nsdlclients_benaccountid as varchar)=trans_accountsledger.accountsledger_subaccountid),(select top 1 cdslclients_benaccountnumber from master_cdslclients where cast(cdslclients_benaccountnumber as varchar)=trans_accountsledger.accountsledger_subaccountid))))) as UCC,case when (sum(accountsledger_amountdr)-sum(accountsledger_amountcr))>0 then sum(accountsledger_amountdr)-sum(accountsledger_amountcr) else null end as AmountDR,case when (sum(accountsledger_amountdr)-sum(accountsledger_amountcr))<0 then ((-1)*(sum(accountsledger_amountdr)-sum(accountsledger_amountcr))) else null end as AmountCR,0 as BranchName,accountsledger_mainaccountid as MainID,accountsledger_subaccountid as SubID  from trans_accountsledger WHERE  accountsledger_subaccountid is not null and accountsledger_subaccountid<>'' and accountsledger_transactiondate<='" + date + "' " + WehereMainAccount + " and accountsledger_exchangesegmentid in(" + SegmentID + ") and accountsledger_companyid='" + CompanyID + "'" + WhereMainBranch + " and accountsledger_FinYear='" + Session["LastFinYear"].ToString() + "' group By  accountsledger_mainaccountid,accountsledger_subaccountid,accountsledger_branchID ) as D group By  accountsledger_mainaccountid,accountsledger_subaccountid,AmountDr,AmountCr,Ucc,BranchName,MainID,SubID  ) as K ", "* ", " DR+CR<>0", "BranchName,accountsledger_mainaccountid,accountsledger_subaccountid ");
                    if (dtSubsidiary.Rows.Count == 0)
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "Message", "<script language='JavaScript'>alert('No record found');</script>");

                    }
                }
                else if (rdDebit.Checked == true)
                {
                    if (rdbMainSelected.Checked == true)
                    {
                        if (ClientValue[1].ToString().Trim() == "Customers" || ClientValue[1].ToString().Trim() == "NSDL Clients" || ClientValue[1].ToString().Trim() == "CDSL Clients")
                        {
                            if (ddlGroup.SelectedItem.Value == "0")
                                dtSubsidiary = oDBEngine.GetDataTable("(Select BranchName,accountsledger_mainaccountid,accountsledger_subaccountid,case when UCC='0' then null else UCC end as Ucc ,cast(AmountDr as varchar(max)) as AmountDr,cast(AmountCr as varchar(max)) as AmountCr,isnull(sum(D.AmountDr),0) as DR,isnull(sum(D.AmountCr),0) as CR,0 as ID,MainID,SubID  from (select (select mainaccount_name from master_mainaccount where mainaccount_accountcode=trans_accountsledger.accountsledger_mainaccountid) as accountsledger_mainaccountid,(isnull((select isnull(ltrim(rtrim(cnt_firstname)),'')+' '+isnull(ltrim(rtrim(cnt_middlename)),'')+' '+isnull(ltrim(rtrim(cnt_lastname)),'') from tbl_master_contact where cnt_internalid=trans_accountsledger.accountsledger_subaccountid),isnull((select top 1 subaccount_name from master_subaccount where cast(subaccount_code as varchar)=trans_accountsledger.accountsledger_subaccountid and SubAccount_MainAcReferenceID=trans_accountsledger.accountsledger_mainaccountid),isnull((select subaccount_name from master_subaccount where cast(subaccount_referenceid as varchar)=trans_accountsledger.accountsledger_subaccountid),isnull((select top 1 cdslclients_firstholdername from master_cdslclients where cast(cdslclients_benaccountnumber as varchar)=trans_accountsledger.accountsledger_subaccountid),(select nsdlclients_benfirstholdername from master_nsdlclients where cast(nsdlclients_benaccountid as varchar)=trans_accountsledger.accountsledger_subaccountid)))))) as accountsledger_subaccountid,(isnull((select isnull(ltrim(rtrim(cnt_ucc)),'') from tbl_master_contact where cnt_internalid=trans_accountsledger.accountsledger_subaccountid),isnull((select SubAccount_Code from master_subaccount where cast(SubAccount_ReferenceID as varchar)=cast(trans_accountsledger.accountsledger_subaccountid as varchar) and SubAccount_MainAcReferenceID=trans_accountsledger.accountsledger_mainaccountid),isnull((select top 1 nsdlclients_benaccountid from master_nsdlclients where cast(nsdlclients_benaccountid as varchar)=trans_accountsledger.accountsledger_subaccountid),(select top 1 cdslclients_benaccountnumber from master_cdslclients where cast(cdslclients_benaccountnumber as varchar)=trans_accountsledger.accountsledger_subaccountid))))) as UCC,case when (sum(accountsledger_amountdr)-sum(accountsledger_amountcr))>0 then sum(accountsledger_amountdr)-sum(accountsledger_amountcr) else null end as AmountDR,case when (sum(accountsledger_amountdr)-sum(accountsledger_amountcr))<0 then ((-1)*(sum(accountsledger_amountdr)-sum(accountsledger_amountcr))) else null end as AmountCR,(select rtrim(branch_description)+' ['+rtrim(branch_code)+']' from tbl_master_branch where branch_id=accountsledger_branchID) as BranchName,accountsledger_mainaccountid as MainID,accountsledger_subaccountid as SubID  from trans_accountsledger WHERE  accountsledger_subaccountid is not null and accountsledger_subaccountid<>'' and accountsledger_transactiondate<='" + date + "' " + WehereMainAccount + " and accountsledger_exchangesegmentid in(" + SegmentID + ") and accountsledger_companyid='" + CompanyID + "' " + WhereMainBranch + " accountsledger_FinYear='" + Session["LastFinYear"].ToString() + "' group By  accountsledger_mainaccountid,accountsledger_subaccountid,accountsledger_branchID ) as D group By  accountsledger_mainaccountid,accountsledger_subaccountid,AmountDr,AmountCr,Ucc,BranchName,MainID,SubID  ) as K ", "* ", " DR+CR<>0 and dr<>0", "BranchName,accountsledger_mainaccountid,accountsledger_subaccountid ");
                            else
                            {
                                if (Session["userlastsegment"].ToString() == "10" || Session["userlastsegment"].ToString() == "9")
                                    dtSubsidiary = oDBEngine.GetDataTable("(Select *  from (Select BranchName,accountsledger_mainaccountid,accountsledger_subaccountid,case when UCC='0' then null else UCC end as Ucc ,cast(AmountDr as varchar(max)) as AmountDr,cast(AmountCr as varchar(max)) as AmountCr,isnull(sum(D.AmountDr),0) as DR,isnull(sum(D.AmountCr),0) as CR,ID,MainID,SubID  from (	select (select mainaccount_name from master_mainaccount where mainaccount_accountcode=trans_accountsledger.accountsledger_mainaccountid) as accountsledger_mainaccountid,(isnull((select isnull(ltrim(rtrim(cnt_firstname)),'')+' '+isnull(ltrim(rtrim(cnt_middlename)),'')+' '+isnull(ltrim(rtrim(cnt_lastname)),'') from tbl_master_contact where cnt_internalid=trans_accountsledger.accountsledger_subaccountid),isnull((select top 1 subaccount_name from master_subaccount where cast(subaccount_code as varchar)=trans_accountsledger.accountsledger_subaccountid and SubAccount_MainAcReferenceID=trans_accountsledger.accountsledger_mainaccountid),isnull((select subaccount_name from master_subaccount where cast(subaccount_referenceid as varchar)=trans_accountsledger.accountsledger_subaccountid),isnull((select top 1 cdslclients_firstholdername from master_cdslclients where cast(cdslclients_benaccountnumber as varchar)=trans_accountsledger.accountsledger_subaccountid),(select nsdlclients_benfirstholdername from master_nsdlclients where cast(nsdlclients_benaccountid as varchar)=trans_accountsledger.accountsledger_subaccountid)))))) as accountsledger_subaccountid,(isnull((select isnull(ltrim(rtrim(cnt_ucc)),'') from tbl_master_contact where cnt_internalid=trans_accountsledger.accountsledger_subaccountid),isnull((select SubAccount_Code from master_subaccount where cast(SubAccount_ReferenceID as varchar)=cast(trans_accountsledger.accountsledger_subaccountid as varchar) and SubAccount_MainAcReferenceID=trans_accountsledger.accountsledger_mainaccountid),isnull((select top 1 nsdlclients_benaccountid from master_nsdlclients where cast(nsdlclients_benaccountid as varchar)=trans_accountsledger.accountsledger_subaccountid),(select top 1 cdslclients_benaccountnumber from master_cdslclients where cast(cdslclients_benaccountnumber as varchar)=trans_accountsledger.accountsledger_subaccountid))))) as UCC,case when (sum(accountsledger_amountdr)-sum(accountsledger_amountcr))>0 then sum(accountsledger_amountdr)-sum(accountsledger_amountcr) else null end as AmountDR,	case when (sum(accountsledger_amountdr)-sum(accountsledger_amountcr))<0 then ((-1)*(sum(accountsledger_amountdr)-sum(accountsledger_amountcr))) else null end as AmountCR,	(select top 1 gpm_Description from tbl_master_groupmaster where gpm_id in(select grp_groupMaster from tbl_trans_group where substring(grp_contactid,9,8)=accountsledger_SubAccountID) and gpm_type=ltrim('" + ddlgrouptype.SelectedItem.Value + "')) as BranchName,	(select top 1 gpm_ID from tbl_master_groupmaster where gpm_id in(select grp_groupMaster from tbl_trans_group where substring(grp_contactid,9,8)=accountsledger_SubAccountID) and gpm_type=ltrim('" + ddlgrouptype.SelectedItem.Value + "')) as ID,accountsledger_mainaccountid as MainID,accountsledger_subaccountid as SubID  from trans_accountsledger WHERE  accountsledger_subaccountid is not null and accountsledger_subaccountid<>'' and accountsledger_transactiondate<='" + date + "' " + WehereMainAccount + " and accountsledger_exchangesegmentid in(" + SegmentID + ") and accountsledger_companyid='" + CompanyID + "' " + WhereMainBranch + " and accountsledger_FinYear='" + Session["LastFinYear"].ToString() + "' group By  accountsledger_mainaccountid,accountsledger_subaccountid,accountsledger_branchID ) as D group By  accountsledger_mainaccountid,accountsledger_subaccountid,AmountDr,AmountCr,Ucc,BranchName,ID,MainID,SubID  ) as K  WHERE  DR+CR<>0 and dr<>0 ) as DD ", "*", ForGroup, " BranchName,accountsledger_mainaccountid,accountsledger_subaccountid");
                                else
                                    dtSubsidiary = oDBEngine.GetDataTable("(Select *  from (Select BranchName,accountsledger_mainaccountid,accountsledger_subaccountid,case when UCC='0' then null else UCC end as Ucc ,cast(AmountDr as varchar(max)) as AmountDr,cast(AmountCr as varchar(max)) as AmountCr,isnull(sum(D.AmountDr),0) as DR,isnull(sum(D.AmountCr),0) as CR,ID,MainID,SubID  from (	select (select mainaccount_name from master_mainaccount where mainaccount_accountcode=trans_accountsledger.accountsledger_mainaccountid) as accountsledger_mainaccountid,(isnull((select isnull(ltrim(rtrim(cnt_firstname)),'')+' '+isnull(ltrim(rtrim(cnt_middlename)),'')+' '+isnull(ltrim(rtrim(cnt_lastname)),'') from tbl_master_contact where cnt_internalid=trans_accountsledger.accountsledger_subaccountid),isnull((select top 1 subaccount_name from master_subaccount where cast(subaccount_code as varchar)=trans_accountsledger.accountsledger_subaccountid and SubAccount_MainAcReferenceID=trans_accountsledger.accountsledger_mainaccountid),isnull((select subaccount_name from master_subaccount where cast(subaccount_referenceid as varchar)=trans_accountsledger.accountsledger_subaccountid),isnull((select top 1 cdslclients_firstholdername from master_cdslclients where cast(cdslclients_benaccountnumber as varchar)=trans_accountsledger.accountsledger_subaccountid),(select nsdlclients_benfirstholdername from master_nsdlclients where cast(nsdlclients_benaccountid as varchar)=trans_accountsledger.accountsledger_subaccountid)))))) as accountsledger_subaccountid,(isnull((select isnull(ltrim(rtrim(cnt_ucc)),'') from tbl_master_contact where cnt_internalid=trans_accountsledger.accountsledger_subaccountid),isnull((select SubAccount_Code from master_subaccount where cast(SubAccount_ReferenceID as varchar)=cast(trans_accountsledger.accountsledger_subaccountid as varchar) and SubAccount_MainAcReferenceID=trans_accountsledger.accountsledger_mainaccountid),isnull((select top 1 nsdlclients_benaccountid from master_nsdlclients where cast(nsdlclients_benaccountid as varchar)=trans_accountsledger.accountsledger_subaccountid),(select top 1 cdslclients_benaccountnumber from master_cdslclients where cast(cdslclients_benaccountnumber as varchar)=trans_accountsledger.accountsledger_subaccountid))))) as UCC,case when (sum(accountsledger_amountdr)-sum(accountsledger_amountcr))>0 then sum(accountsledger_amountdr)-sum(accountsledger_amountcr) else null end as AmountDR,	case when (sum(accountsledger_amountdr)-sum(accountsledger_amountcr))<0 then ((-1)*(sum(accountsledger_amountdr)-sum(accountsledger_amountcr))) else null end as AmountCR,	(select top 1 gpm_Description from tbl_master_groupmaster where gpm_id in(select grp_groupMaster from tbl_trans_group where grp_contactid=accountsledger_SubAccountID) and gpm_type=ltrim('" + ddlgrouptype.SelectedItem.Value + "')) as BranchName,	(select top 1 gpm_ID from tbl_master_groupmaster where gpm_id in(select grp_groupMaster from tbl_trans_group where grp_contactid=accountsledger_SubAccountID) and gpm_type=ltrim('" + ddlgrouptype.SelectedItem.Value + "')) as ID,accountsledger_mainaccountid as MainID,accountsledger_subaccountid as SubID  from trans_accountsledger WHERE  accountsledger_subaccountid is not null and accountsledger_subaccountid<>'' and accountsledger_transactiondate<='" + date + "' " + WehereMainAccount + " and accountsledger_exchangesegmentid in(" + SegmentID + ") and accountsledger_companyid='" + CompanyID + "' " + WhereMainBranch + " and accountsledger_FinYear='" + Session["LastFinYear"].ToString() + "' group By  accountsledger_mainaccountid,accountsledger_subaccountid,accountsledger_branchID ) as D group By  accountsledger_mainaccountid,accountsledger_subaccountid,AmountDr,AmountCr,Ucc,BranchName,ID,MainID,SubID  ) as K  WHERE  DR+CR<>0 and dr<>0 ) as DD ", "*", ForGroup, " BranchName,accountsledger_mainaccountid,accountsledger_subaccountid");
                            }

                        }
                        else
                            dtSubsidiary = oDBEngine.GetDataTable("(Select BranchName,accountsledger_mainaccountid,accountsledger_subaccountid,case when UCC='0' then null else UCC end as Ucc ,case when (isnull(sum(D.AmountDr),0)-isnull(sum(D.AmountCr),0))>0 then (isnull(sum(D.AmountDr),0)-isnull(sum(D.AmountCr),0)) else 0 end as DR,case when (isnull(sum(D.AmountCr),0)-isnull(sum(D.AmountDr),0))>0 then (isnull(sum(D.AmountCr),0)-isnull(sum(D.AmountDr),0)) else 0 end as CR,0 as ID,MainID,SubID  from (select (select mainaccount_name from master_mainaccount where mainaccount_accountcode=trans_accountsledger.accountsledger_mainaccountid) as accountsledger_mainaccountid,(isnull((select isnull(ltrim(rtrim(cnt_firstname)),'')+' '+isnull(ltrim(rtrim(cnt_middlename)),'')+' '+isnull(ltrim(rtrim(cnt_lastname)),'') from tbl_master_contact where cnt_internalid=trans_accountsledger.accountsledger_subaccountid),isnull((select top 1 subaccount_name from master_subaccount where cast(subaccount_code as varchar)=trans_accountsledger.accountsledger_subaccountid and SubAccount_MainAcReferenceID=trans_accountsledger.accountsledger_mainaccountid),isnull((select subaccount_name from master_subaccount where cast(subaccount_referenceid as varchar)=trans_accountsledger.accountsledger_subaccountid),isnull((select top 1 cdslclients_firstholdername from master_cdslclients where cast(cdslclients_benaccountnumber as varchar)=trans_accountsledger.accountsledger_subaccountid),(select nsdlclients_benfirstholdername from master_nsdlclients where cast(nsdlclients_benaccountid as varchar)=trans_accountsledger.accountsledger_subaccountid)))))) as accountsledger_subaccountid,(isnull((select isnull(ltrim(rtrim(cnt_ucc)),'') from tbl_master_contact where cnt_internalid=trans_accountsledger.accountsledger_subaccountid),isnull((select SubAccount_Code from master_subaccount where cast(SubAccount_ReferenceID as varchar)=cast(trans_accountsledger.accountsledger_subaccountid as varchar) and SubAccount_MainAcReferenceID=trans_accountsledger.accountsledger_mainaccountid),isnull((select top 1 nsdlclients_benaccountid from master_nsdlclients where cast(nsdlclients_benaccountid as varchar)=trans_accountsledger.accountsledger_subaccountid),(select top 1 cdslclients_benaccountnumber from master_cdslclients where cast(cdslclients_benaccountnumber as varchar)=trans_accountsledger.accountsledger_subaccountid))))) as UCC,case when (sum(accountsledger_amountdr)-sum(accountsledger_amountcr))>0 then sum(accountsledger_amountdr)-sum(accountsledger_amountcr) else null end as AmountDR,case when (sum(accountsledger_amountdr)-sum(accountsledger_amountcr))<0 then ((-1)*(sum(accountsledger_amountdr)-sum(accountsledger_amountcr))) else null end as AmountCR,0 as BranchName,accountsledger_mainaccountid as MainID,accountsledger_subaccountid as SubID  from trans_accountsledger WHERE  accountsledger_subaccountid is not null and accountsledger_subaccountid<>'' and accountsledger_transactiondate<='" + date + "' " + WehereMainAccount + " and accountsledger_exchangesegmentid in(" + SegmentID + ") and accountsledger_companyid='" + CompanyID + "' " + WhereMainBranch + " accountsledger_FinYear='" + Session["LastFinYear"].ToString() + "' group By  accountsledger_mainaccountid,accountsledger_subaccountid,accountsledger_branchID ) as D group By  accountsledger_mainaccountid,accountsledger_subaccountid,Ucc,BranchName,MainID,SubID) as K ", "BranchName,accountsledger_mainaccountid,accountsledger_subaccountid,Ucc,case when DR=0 then null else cast(DR as varchar(max)) end as AmountDr,case when CR=0 then null else cast(CR as varchar(max)) end as AmountCr,DR,CR,MainID,SubID", " DR+CR<>0 and dr<>0", "BranchName,accountsledger_mainaccountid,accountsledger_subaccountid ");
                    }
                    else
                        dtSubsidiary = oDBEngine.GetDataTable("(Select BranchName,accountsledger_mainaccountid,accountsledger_subaccountid,case when UCC='0' then null else UCC end as Ucc ,cast(AmountDr as varchar(max)) as AmountDr,cast(AmountCr as varchar(max)) as AmountCr,isnull(sum(D.AmountDr),0) as DR,isnull(sum(D.AmountCr),0) as CR,0 as ID,MainID,SubID  from (select (select mainaccount_name from master_mainaccount where mainaccount_accountcode=trans_accountsledger.accountsledger_mainaccountid) as accountsledger_mainaccountid,(isnull((select isnull(ltrim(rtrim(cnt_firstname)),'')+' '+isnull(ltrim(rtrim(cnt_middlename)),'')+' '+isnull(ltrim(rtrim(cnt_lastname)),'') from tbl_master_contact where cnt_internalid=trans_accountsledger.accountsledger_subaccountid),isnull((select top 1 subaccount_name from master_subaccount where cast(subaccount_code as varchar)=trans_accountsledger.accountsledger_subaccountid and SubAccount_MainAcReferenceID=trans_accountsledger.accountsledger_mainaccountid),isnull((select subaccount_name from master_subaccount where cast(subaccount_referenceid as varchar)=trans_accountsledger.accountsledger_subaccountid),isnull((select top 1 cdslclients_firstholdername from master_cdslclients where cast(cdslclients_benaccountnumber as varchar)=trans_accountsledger.accountsledger_subaccountid),(select nsdlclients_benfirstholdername from master_nsdlclients where cast(nsdlclients_benaccountid as varchar)=trans_accountsledger.accountsledger_subaccountid)))))) as accountsledger_subaccountid,(isnull((select isnull(ltrim(rtrim(cnt_ucc)),'') from tbl_master_contact where cnt_internalid=trans_accountsledger.accountsledger_subaccountid),isnull((select SubAccount_Code from master_subaccount where cast(SubAccount_ReferenceID as varchar)=cast(trans_accountsledger.accountsledger_subaccountid as varchar) and SubAccount_MainAcReferenceID=trans_accountsledger.accountsledger_mainaccountid),isnull((select top 1 nsdlclients_benaccountid from master_nsdlclients where cast(nsdlclients_benaccountid as varchar)=trans_accountsledger.accountsledger_subaccountid),(select top 1 cdslclients_benaccountnumber from master_cdslclients where cast(cdslclients_benaccountnumber as varchar)=trans_accountsledger.accountsledger_subaccountid))))) as UCC,case when (sum(accountsledger_amountdr)-sum(accountsledger_amountcr))>0 then sum(accountsledger_amountdr)-sum(accountsledger_amountcr) else null end as AmountDR,case when (sum(accountsledger_amountdr)-sum(accountsledger_amountcr))<0 then ((-1)*(sum(accountsledger_amountdr)-sum(accountsledger_amountcr))) else null end as AmountCR,0 as BranchName,accountsledger_mainaccountid as MainID,accountsledger_subaccountid as SubID  from trans_accountsledger WHERE  accountsledger_subaccountid is not null and accountsledger_subaccountid<>'' and accountsledger_transactiondate<='" + date + "' " + WehereMainAccount + " and accountsledger_exchangesegmentid in(" + SegmentID + ") and accountsledger_companyid='" + CompanyID + "' " + WhereMainBranch + " accountsledger_FinYear='" + Session["LastFinYear"].ToString() + "' group By  accountsledger_mainaccountid,accountsledger_subaccountid,accountsledger_branchID ) as D group By  accountsledger_mainaccountid,accountsledger_subaccountid,AmountDr,AmountCr,Ucc,BranchName,MainID,SubID) as K ", "* ", " DR+CR<>0 and dr<>0", "BranchName,accountsledger_mainaccountid,accountsledger_subaccountid ");


                }
                else if (rdCredit.Checked == true)
                {
                    if (rdbMainSelected.Checked == true)
                    {
                        if (ClientValue[1].ToString().Trim() == "Customers" || ClientValue[1].ToString().Trim() == "NSDL Clients" || ClientValue[1].ToString().Trim() == "CDSL Clients")
                        {
                            if (ddlGroup.SelectedItem.Value == "0")
                                dtSubsidiary = oDBEngine.GetDataTable("(Select BranchName,accountsledger_mainaccountid,accountsledger_subaccountid,case when UCC='0' then null else UCC end as Ucc ,cast(AmountDr as varchar(max)) as AmountDr,cast(AmountCr as varchar(max)) as AmountCr,isnull(sum(D.AmountDr),0) as DR,isnull(sum(D.AmountCr),0) as CR,0 as ID,MainID,SubID  from (select (select mainaccount_name from master_mainaccount where mainaccount_accountcode=trans_accountsledger.accountsledger_mainaccountid) as accountsledger_mainaccountid,(isnull((select isnull(ltrim(rtrim(cnt_firstname)),'')+' '+isnull(ltrim(rtrim(cnt_middlename)),'')+' '+isnull(ltrim(rtrim(cnt_lastname)),'') from tbl_master_contact where cnt_internalid=trans_accountsledger.accountsledger_subaccountid),isnull((select top 1 subaccount_name from master_subaccount where cast(subaccount_code as varchar)=trans_accountsledger.accountsledger_subaccountid and SubAccount_MainAcReferenceID=trans_accountsledger.accountsledger_mainaccountid),isnull((select subaccount_name from master_subaccount where cast(subaccount_referenceid as varchar)=trans_accountsledger.accountsledger_subaccountid),isnull((select top 1 cdslclients_firstholdername from master_cdslclients where cast(cdslclients_benaccountnumber as varchar)=trans_accountsledger.accountsledger_subaccountid),(select nsdlclients_benfirstholdername from master_nsdlclients where cast(nsdlclients_benaccountid as varchar)=trans_accountsledger.accountsledger_subaccountid)))))) as accountsledger_subaccountid,(isnull((select isnull(ltrim(rtrim(cnt_ucc)),'') from tbl_master_contact where cnt_internalid=trans_accountsledger.accountsledger_subaccountid),isnull((select SubAccount_Code from master_subaccount where cast(SubAccount_ReferenceID as varchar)=cast(trans_accountsledger.accountsledger_subaccountid as varchar) and SubAccount_MainAcReferenceID=trans_accountsledger.accountsledger_mainaccountid),isnull((select top 1 nsdlclients_benaccountid from master_nsdlclients where cast(nsdlclients_benaccountid as varchar)=trans_accountsledger.accountsledger_subaccountid),(select top 1 cdslclients_benaccountnumber from master_cdslclients where cast(cdslclients_benaccountnumber as varchar)=trans_accountsledger.accountsledger_subaccountid))))) as UCC,case when (sum(accountsledger_amountdr)-sum(accountsledger_amountcr))>0 then sum(accountsledger_amountdr)-sum(accountsledger_amountcr) else null end as AmountDR,case when (sum(accountsledger_amountdr)-sum(accountsledger_amountcr))<0 then ((-1)*(sum(accountsledger_amountdr)-sum(accountsledger_amountcr))) else null end as AmountCR,(select rtrim(branch_description)+' ['+rtrim(branch_code)+']' from tbl_master_branch where branch_id=accountsledger_branchID) as BranchName,accountsledger_mainaccountid as MainID,accountsledger_subaccountid as SubID  from trans_accountsledger WHERE  accountsledger_subaccountid is not null and accountsledger_subaccountid<>'' and accountsledger_transactiondate<='" + date + "' " + WehereMainAccount + " and accountsledger_exchangesegmentid in(" + SegmentID + ") and accountsledger_companyid='" + CompanyID + "' " + WhereMainBranch + " accountsledger_FinYear='" + Session["LastFinYear"].ToString() + "' group By  accountsledger_mainaccountid,accountsledger_subaccountid,accountsledger_branchID ) as D group By  accountsledger_mainaccountid,accountsledger_subaccountid,AmountDr,AmountCr,Ucc,BranchName,MainID,SubID  ) as K ", "* ", " DR+CR<>0 and cr<>0", "BranchName,accountsledger_mainaccountid,accountsledger_subaccountid ");
                            else
                            {
                                if (Session["userlastsegment"].ToString() == "10" || Session["userlastsegment"].ToString() == "9")
                                    dtSubsidiary = oDBEngine.GetDataTable("(Select *  from (Select BranchName,accountsledger_mainaccountid,accountsledger_subaccountid,case when UCC='0' then null else UCC end as Ucc ,cast(AmountDr as varchar(max)) as AmountDr,cast(AmountCr as varchar(max)) as AmountCr,isnull(sum(D.AmountDr),0) as DR,isnull(sum(D.AmountCr),0) as CR,ID,MainID,SubID  from (	select (select mainaccount_name from master_mainaccount where mainaccount_accountcode=trans_accountsledger.accountsledger_mainaccountid) as accountsledger_mainaccountid,(isnull((select isnull(ltrim(rtrim(cnt_firstname)),'')+' '+isnull(ltrim(rtrim(cnt_middlename)),'')+' '+isnull(ltrim(rtrim(cnt_lastname)),'') from tbl_master_contact where cnt_internalid=trans_accountsledger.accountsledger_subaccountid),isnull((select top 1 subaccount_name from master_subaccount where cast(subaccount_code as varchar)=trans_accountsledger.accountsledger_subaccountid and SubAccount_MainAcReferenceID=trans_accountsledger.accountsledger_mainaccountid),isnull((select subaccount_name from master_subaccount where cast(subaccount_referenceid as varchar)=trans_accountsledger.accountsledger_subaccountid),isnull((select top 1 cdslclients_firstholdername from master_cdslclients where cast(cdslclients_benaccountnumber as varchar)=trans_accountsledger.accountsledger_subaccountid),(select nsdlclients_benfirstholdername from master_nsdlclients where cast(nsdlclients_benaccountid as varchar)=trans_accountsledger.accountsledger_subaccountid)))))) as accountsledger_subaccountid,(isnull((select isnull(ltrim(rtrim(cnt_ucc)),'') from tbl_master_contact where cnt_internalid=trans_accountsledger.accountsledger_subaccountid),isnull((select SubAccount_Code from master_subaccount where cast(SubAccount_ReferenceID as varchar)=cast(trans_accountsledger.accountsledger_subaccountid as varchar) and SubAccount_MainAcReferenceID=trans_accountsledger.accountsledger_mainaccountid),isnull((select top 1 nsdlclients_benaccountid from master_nsdlclients where cast(nsdlclients_benaccountid as varchar)=trans_accountsledger.accountsledger_subaccountid),(select top 1 cdslclients_benaccountnumber from master_cdslclients where cast(cdslclients_benaccountnumber as varchar)=trans_accountsledger.accountsledger_subaccountid))))) as UCC,case when (sum(accountsledger_amountdr)-sum(accountsledger_amountcr))>0 then sum(accountsledger_amountdr)-sum(accountsledger_amountcr) else null end as AmountDR,	case when (sum(accountsledger_amountdr)-sum(accountsledger_amountcr))<0 then ((-1)*(sum(accountsledger_amountdr)-sum(accountsledger_amountcr))) else null end as AmountCR,	(select top 1 gpm_Description from tbl_master_groupmaster where gpm_id in(select grp_groupMaster from tbl_trans_group where substring(grp_contactid,9,8)=accountsledger_SubAccountID) and gpm_type=ltrim('" + ddlgrouptype.SelectedItem.Value + "')) as BranchName,	(select top 1 gpm_ID from tbl_master_groupmaster where gpm_id in(select grp_groupMaster from tbl_trans_group where substring(grp_contactid,9,8)=accountsledger_SubAccountID) and gpm_type=ltrim('" + ddlgrouptype.SelectedItem.Value + "')) as ID,accountsledger_mainaccountid as MainID,accountsledger_subaccountid as SubID  from trans_accountsledger WHERE  accountsledger_subaccountid is not null and accountsledger_subaccountid<>'' and accountsledger_transactiondate<='" + date + "' " + WehereMainAccount + " and accountsledger_exchangesegmentid in(" + SegmentID + ") and accountsledger_companyid='" + CompanyID + "' " + WhereMainBranch + " and accountsledger_FinYear='" + Session["LastFinYear"].ToString() + "' group By  accountsledger_mainaccountid,accountsledger_subaccountid,accountsledger_branchID ) as D group By  accountsledger_mainaccountid,accountsledger_subaccountid,AmountDr,AmountCr,Ucc,BranchName,ID,MainID,SubID  ) as K  WHERE  DR+CR<>0 and cr<>0 ) as DD ", "*", ForGroup, " BranchName,accountsledger_mainaccountid,accountsledger_subaccountid");
                                else
                                    dtSubsidiary = oDBEngine.GetDataTable("(Select *  from (Select BranchName,accountsledger_mainaccountid,accountsledger_subaccountid,case when UCC='0' then null else UCC end as Ucc ,cast(AmountDr as varchar(max)) as AmountDr,cast(AmountCr as varchar(max)) as AmountCr,isnull(sum(D.AmountDr),0) as DR,isnull(sum(D.AmountCr),0) as CR,ID,MainID,SubID  from (	select (select mainaccount_name from master_mainaccount where mainaccount_accountcode=trans_accountsledger.accountsledger_mainaccountid) as accountsledger_mainaccountid,(isnull((select isnull(ltrim(rtrim(cnt_firstname)),'')+' '+isnull(ltrim(rtrim(cnt_middlename)),'')+' '+isnull(ltrim(rtrim(cnt_lastname)),'') from tbl_master_contact where cnt_internalid=trans_accountsledger.accountsledger_subaccountid),isnull((select top 1 subaccount_name from master_subaccount where cast(subaccount_code as varchar)=trans_accountsledger.accountsledger_subaccountid and SubAccount_MainAcReferenceID=trans_accountsledger.accountsledger_mainaccountid),isnull((select subaccount_name from master_subaccount where cast(subaccount_referenceid as varchar)=trans_accountsledger.accountsledger_subaccountid),isnull((select top 1 cdslclients_firstholdername from master_cdslclients where cast(cdslclients_benaccountnumber as varchar)=trans_accountsledger.accountsledger_subaccountid),(select nsdlclients_benfirstholdername from master_nsdlclients where cast(nsdlclients_benaccountid as varchar)=trans_accountsledger.accountsledger_subaccountid)))))) as accountsledger_subaccountid,(isnull((select isnull(ltrim(rtrim(cnt_ucc)),'') from tbl_master_contact where cnt_internalid=trans_accountsledger.accountsledger_subaccountid),isnull((select SubAccount_Code from master_subaccount where cast(SubAccount_ReferenceID as varchar)=cast(trans_accountsledger.accountsledger_subaccountid as varchar) and SubAccount_MainAcReferenceID=trans_accountsledger.accountsledger_mainaccountid),isnull((select top 1 nsdlclients_benaccountid from master_nsdlclients where cast(nsdlclients_benaccountid as varchar)=trans_accountsledger.accountsledger_subaccountid),(select top 1 cdslclients_benaccountnumber from master_cdslclients where cast(cdslclients_benaccountnumber as varchar)=trans_accountsledger.accountsledger_subaccountid))))) as UCC,case when (sum(accountsledger_amountdr)-sum(accountsledger_amountcr))>0 then sum(accountsledger_amountdr)-sum(accountsledger_amountcr) else null end as AmountDR,	case when (sum(accountsledger_amountdr)-sum(accountsledger_amountcr))<0 then ((-1)*(sum(accountsledger_amountdr)-sum(accountsledger_amountcr))) else null end as AmountCR,	(select top 1 gpm_Description from tbl_master_groupmaster where gpm_id in(select grp_groupMaster from tbl_trans_group where grp_contactid=accountsledger_SubAccountID) and gpm_type=ltrim('" + ddlgrouptype.SelectedItem.Value + "')) as BranchName,	(select top 1 gpm_ID from tbl_master_groupmaster where gpm_id in(select grp_groupMaster from tbl_trans_group where grp_contactid=accountsledger_SubAccountID) and gpm_type=ltrim('" + ddlgrouptype.SelectedItem.Value + "')) as ID,accountsledger_mainaccountid as MainID,accountsledger_subaccountid as SubID  from trans_accountsledger WHERE  accountsledger_subaccountid is not null and accountsledger_subaccountid<>'' and accountsledger_transactiondate<='" + date + "' " + WehereMainAccount + " and accountsledger_exchangesegmentid in(" + SegmentID + ") and accountsledger_companyid='" + CompanyID + "' " + WhereMainBranch + " and accountsledger_FinYear='" + Session["LastFinYear"].ToString() + "' group By  accountsledger_mainaccountid,accountsledger_subaccountid,accountsledger_branchID ) as D group By  accountsledger_mainaccountid,accountsledger_subaccountid,AmountDr,AmountCr,Ucc,BranchName,ID,MainID,SubID  ) as K  WHERE  DR+CR<>0 and cr<>0 ) as DD ", "*", ForGroup, " BranchName,accountsledger_mainaccountid,accountsledger_subaccountid");
                            }
                        }
                        else
                            dtSubsidiary = oDBEngine.GetDataTable("(Select BranchName,accountsledger_mainaccountid,accountsledger_subaccountid,case when UCC='0' then null else UCC end as Ucc ,case when (isnull(sum(D.AmountDr),0)-isnull(sum(D.AmountCr),0))>0 then (isnull(sum(D.AmountDr),0)-isnull(sum(D.AmountCr),0)) else 0 end as DR,case when (isnull(sum(D.AmountCr),0)-isnull(sum(D.AmountDr),0))>0 then (isnull(sum(D.AmountCr),0)-isnull(sum(D.AmountDr),0)) else 0 end as CR,0 as ID,MainID,SubID  from (select (select mainaccount_name from master_mainaccount where mainaccount_accountcode=trans_accountsledger.accountsledger_mainaccountid) as accountsledger_mainaccountid,(isnull((select isnull(ltrim(rtrim(cnt_firstname)),'')+' '+isnull(ltrim(rtrim(cnt_middlename)),'')+' '+isnull(ltrim(rtrim(cnt_lastname)),'') from tbl_master_contact where cnt_internalid=trans_accountsledger.accountsledger_subaccountid),isnull((select top 1 subaccount_name from master_subaccount where cast(subaccount_code as varchar)=trans_accountsledger.accountsledger_subaccountid and SubAccount_MainAcReferenceID=trans_accountsledger.accountsledger_mainaccountid),isnull((select subaccount_name from master_subaccount where cast(subaccount_referenceid as varchar)=trans_accountsledger.accountsledger_subaccountid),isnull((select top 1 cdslclients_firstholdername from master_cdslclients where cast(cdslclients_benaccountnumber as varchar)=trans_accountsledger.accountsledger_subaccountid),(select nsdlclients_benfirstholdername from master_nsdlclients where cast(nsdlclients_benaccountid as varchar)=trans_accountsledger.accountsledger_subaccountid)))))) as accountsledger_subaccountid,(isnull((select isnull(ltrim(rtrim(cnt_ucc)),'') from tbl_master_contact where cnt_internalid=trans_accountsledger.accountsledger_subaccountid),isnull((select SubAccount_Code from master_subaccount where cast(SubAccount_ReferenceID as varchar)=cast(trans_accountsledger.accountsledger_subaccountid as varchar) and SubAccount_MainAcReferenceID=trans_accountsledger.accountsledger_mainaccountid),isnull((select top 1 nsdlclients_benaccountid from master_nsdlclients where cast(nsdlclients_benaccountid as varchar)=trans_accountsledger.accountsledger_subaccountid),(select top 1 cdslclients_benaccountnumber from master_cdslclients where cast(cdslclients_benaccountnumber as varchar)=trans_accountsledger.accountsledger_subaccountid))))) as UCC,case when (sum(accountsledger_amountdr)-sum(accountsledger_amountcr))>0 then sum(accountsledger_amountdr)-sum(accountsledger_amountcr) else null end as AmountDR,case when (sum(accountsledger_amountdr)-sum(accountsledger_amountcr))<0 then ((-1)*(sum(accountsledger_amountdr)-sum(accountsledger_amountcr))) else null end as AmountCR,0 as BranchName,accountsledger_mainaccountid as MainID,accountsledger_subaccountid as SubID  from trans_accountsledger WHERE  accountsledger_subaccountid is not null and accountsledger_subaccountid<>'' and accountsledger_transactiondate<='" + date + "' " + WehereMainAccount + " and accountsledger_exchangesegmentid in(" + SegmentID + ") and accountsledger_companyid='" + CompanyID + "' " + WhereMainBranch + " accountsledger_FinYear='" + Session["LastFinYear"].ToString() + "' group By  accountsledger_mainaccountid,accountsledger_subaccountid,accountsledger_branchID ) as D group By  accountsledger_mainaccountid,accountsledger_subaccountid,Ucc,BranchName  ) as K ", "BranchName,accountsledger_mainaccountid,accountsledger_subaccountid,Ucc,case when DR=0 then null else cast(DR as varchar(max)) end as AmountDr,case when CR=0 then null else cast(CR as varchar(max)) end as AmountCr,DR,CR,MainID,SubID ", " DR+CR<>0 and cr<>0", "BranchName,accountsledger_mainaccountid,accountsledger_subaccountid ");
                    }
                    else
                        dtSubsidiary = oDBEngine.GetDataTable("(Select BranchName,accountsledger_mainaccountid,accountsledger_subaccountid,case when UCC='0' then null else UCC end as Ucc ,cast(AmountDr as varchar(max)) as AmountDr,cast(AmountCr as varchar(max)) as AmountCr,isnull(sum(D.AmountDr),0) as DR,isnull(sum(D.AmountCr),0) as CR,0 as ID,MainID,SubID  from (select (select mainaccount_name from master_mainaccount where mainaccount_accountcode=trans_accountsledger.accountsledger_mainaccountid) as accountsledger_mainaccountid,(isnull((select isnull(ltrim(rtrim(cnt_firstname)),'')+' '+isnull(ltrim(rtrim(cnt_middlename)),'')+' '+isnull(ltrim(rtrim(cnt_lastname)),'') from tbl_master_contact where cnt_internalid=trans_accountsledger.accountsledger_subaccountid),isnull((select top 1 subaccount_name from master_subaccount where cast(subaccount_code as varchar)=trans_accountsledger.accountsledger_subaccountid and SubAccount_MainAcReferenceID=trans_accountsledger.accountsledger_mainaccountid),isnull((select subaccount_name from master_subaccount where cast(subaccount_referenceid as varchar)=trans_accountsledger.accountsledger_subaccountid),isnull((select top 1 cdslclients_firstholdername from master_cdslclients where cast(cdslclients_benaccountnumber as varchar)=trans_accountsledger.accountsledger_subaccountid),(select nsdlclients_benfirstholdername from master_nsdlclients where cast(nsdlclients_benaccountid as varchar)=trans_accountsledger.accountsledger_subaccountid)))))) as accountsledger_subaccountid,(isnull((select isnull(ltrim(rtrim(cnt_ucc)),'') from tbl_master_contact where cnt_internalid=trans_accountsledger.accountsledger_subaccountid),isnull((select SubAccount_Code from master_subaccount where cast(SubAccount_ReferenceID as varchar)=cast(trans_accountsledger.accountsledger_subaccountid as varchar) and SubAccount_MainAcReferenceID=trans_accountsledger.accountsledger_mainaccountid),isnull((select top 1 nsdlclients_benaccountid from master_nsdlclients where cast(nsdlclients_benaccountid as varchar)=trans_accountsledger.accountsledger_subaccountid),(select top 1 cdslclients_benaccountnumber from master_cdslclients where cast(cdslclients_benaccountnumber as varchar)=trans_accountsledger.accountsledger_subaccountid))))) as UCC,case when (sum(accountsledger_amountdr)-sum(accountsledger_amountcr))>0 then sum(accountsledger_amountdr)-sum(accountsledger_amountcr) else null end as AmountDR,case when (sum(accountsledger_amountdr)-sum(accountsledger_amountcr))<0 then ((-1)*(sum(accountsledger_amountdr)-sum(accountsledger_amountcr))) else null end as AmountCR,0 as BranchName,accountsledger_mainaccountid as MainID,accountsledger_subaccountid as SubID  from trans_accountsledger WHERE  accountsledger_subaccountid is not null and accountsledger_subaccountid<>'' and accountsledger_transactiondate<='" + date + "' " + WehereMainAccount + " and accountsledger_exchangesegmentid in(" + SegmentID + ") and accountsledger_companyid='" + CompanyID + "' " + WhereMainBranch + " accountsledger_FinYear='" + Session["LastFinYear"].ToString() + "' group By  accountsledger_mainaccountid,accountsledger_subaccountid,accountsledger_branchID ) as D group By  accountsledger_mainaccountid,accountsledger_subaccountid,AmountDr,AmountCr,Ucc,BranchName,MainID,SubID) as K ", "* ", " DR+CR<>0 and cr<>0", "BranchName,accountsledger_mainaccountid,accountsledger_subaccountid ");

                }
                decimal SumDr = 0;
                decimal SumCr = 0;
                pagecount = dtSubsidiary.Rows.Count / pageSize + 1;
                TotalPages.Value = pagecount.ToString();
                if (pageindex <= 0)
                {
                    pageindex = 0;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "hide", "Disable('P');", true);
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "hide", "Disable('P','" + LinkFirst + "','" + LinkPrev + "','" + LinkNext + "','" + LinkLast + "');", true);
                }
                if (pageindex >= int.Parse(TotalPages.Value.ToString()))
                {
                    pageindex = int.Parse(TotalPages.Value.ToString()) - 1;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "hide", "Disable('N');", true);
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "hide", "Disable('N','" + LinkFirst + "','" + LinkPrev + "','" + LinkNext + "','" + LinkLast + "');", true);
                }
                if (pageindex >= (int.Parse(TotalPages.Value.ToString()) - 1))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "hide", "Disable('N');", true);
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "hide", "Disable('N','" + LinkFirst + "','" + LinkPrev + "','" + LinkNext + "','" + LinkLast + "');", true);
                }
                grdSubsidiaryTrial.PageIndex = pageindex;
                CurrentPage.Value = pageindex.ToString();
                rowcount = 0;
                dtSubsidiary_New = dtSubsidiary.Clone();
                if (dtSubsidiary.Rows.Count > 0)
                {
                    for (int k = 0; k < dtSubsidiary.Rows.Count; k++)
                    {
                        if (k == 0)
                            BranchName = dtSubsidiary.Rows[k]["BranchName"].ToString();
                        if (dtSubsidiary.Rows[k]["BranchName"].ToString() != BranchName)
                        {
                            DataRow DrNew1 = dtSubsidiary_New.NewRow();
                            DrNew1[0] = "Total";
                            DrNew1[4] = oconverter.getFormattedvaluewithoriginalsign(SumForBranchDR);
                            DrNew1[5] = oconverter.getFormattedvaluewithoriginalsign(SumForBranchCR);
                            dtSubsidiary_New.Rows.Add(DrNew1);
                            DifOfDRCR = SumForBranchDR - SumForBranchCR;
                            DataRow DrNew2 = dtSubsidiary_New.NewRow();
                            if (ddlGroup.SelectedItem.Value == "0")
                                DrNew2[0] = "Branch Net";
                            else
                                DrNew2[0] = "Group Net";
                            if (DifOfDRCR >= 0)
                                DrNew2[4] = oconverter.getFormattedvaluewithoriginalsign(DifOfDRCR);
                            else
                                DrNew2[5] = oconverter.getFormattedvaluewithoriginalsign(Math.Abs(DifOfDRCR));
                            dtSubsidiary_New.Rows.Add(DrNew2);
                            DifOfDRCR = 0;
                            SumForBranchDR = 0;
                            SumForBranchCR = 0;
                            DataRow DrNewBlank = dtSubsidiary_New.NewRow();
                            DrNewBlank[0] = "Blank";
                            dtSubsidiary_New.Rows.Add(DrNewBlank);
                        }
                        DataRow DrNew = dtSubsidiary_New.NewRow();
                        DrNew[0] = dtSubsidiary.Rows[k]["BranchName"].ToString();
                        DrNew[1] = dtSubsidiary.Rows[k]["accountsledger_mainaccountid"].ToString();
                        DrNew[2] = dtSubsidiary.Rows[k]["accountsledger_subaccountid"].ToString();
                        DrNew[3] = dtSubsidiary.Rows[k]["Ucc"].ToString();
                        if (dtSubsidiary.Rows[k]["AmountDr"].ToString() != "")
                        {
                            dtSubsidiary.Rows[k]["AmountDr"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtSubsidiary.Rows[k]["AmountDr"].ToString()));
                            DrNew[4] = dtSubsidiary.Rows[k]["AmountDr"].ToString();
                        }
                        else
                            DrNew[4] = "";
                        if (dtSubsidiary.Rows[k]["AmountCr"].ToString() != "")
                        {
                            dtSubsidiary.Rows[k]["AmountCr"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtSubsidiary.Rows[k]["AmountCr"].ToString()));
                            DrNew[5] = dtSubsidiary.Rows[k]["AmountCr"].ToString();
                        }
                        else
                            DrNew[5] = "";
                        DrNew[9] = dtSubsidiary.Rows[k]["MainID"].ToString();
                        DrNew[10] = dtSubsidiary.Rows[k]["SubID"].ToString();
                        if (dtSubsidiary.Rows[k]["AmountDr"] != DBNull.Value)
                        {
                            SumDr += Convert.ToDecimal(dtSubsidiary.Rows[k]["AmountDr"].ToString());
                            SumForBranchDR += Convert.ToDecimal(dtSubsidiary.Rows[k]["AmountDr"].ToString());
                        }
                        if (dtSubsidiary.Rows[k]["AmountCr"] != DBNull.Value)
                        {
                            SumCr += Convert.ToDecimal(dtSubsidiary.Rows[k]["AmountCr"].ToString());
                            SumForBranchCR += Convert.ToDecimal(dtSubsidiary.Rows[k]["AmountCr"].ToString());
                        }
                        BranchName = dtSubsidiary.Rows[k]["BranchName"].ToString();
                        dtSubsidiary_New.Rows.Add(DrNew);
                        if (rdbMainSelected.Checked == true)
                        {
                            if (ClientValue[1].ToString().Trim() == "Customers" || ClientValue[1].ToString().Trim() == "NSDL Clients" || ClientValue[1].ToString().Trim() == "CDSL Clients")
                            {
                                if (k == dtSubsidiary.Rows.Count - 1)
                                {
                                    DataRow DrNew3 = dtSubsidiary_New.NewRow();
                                    DrNew3[0] = "Total";
                                    DrNew3[4] = oconverter.getFormattedvaluewithoriginalsign(SumForBranchDR);
                                    DrNew3[5] = oconverter.getFormattedvaluewithoriginalsign(SumForBranchCR);
                                    dtSubsidiary_New.Rows.Add(DrNew3);
                                    DifOfDRCR = SumForBranchDR - SumForBranchCR;
                                    DataRow DrNew4 = dtSubsidiary_New.NewRow();
                                    if (ddlGroup.SelectedItem.Value == "0")
                                        DrNew4[0] = "Branch Net";
                                    else
                                        DrNew4[0] = "Group Net";
                                    if (DifOfDRCR >= 0)
                                        DrNew4[4] = oconverter.getFormattedvaluewithoriginalsign(DifOfDRCR);
                                    else
                                        DrNew4[5] = oconverter.getFormattedvaluewithoriginalsign(Math.Abs(DifOfDRCR));
                                    dtSubsidiary_New.Rows.Add(DrNew4);
                                    DifOfDRCR = 0;
                                    SumForBranchDR = 0;
                                    SumForBranchCR = 0;
                                }
                            }
                        }
                    }
                }
                ViewState["SumDr"] = SumDr.ToString("c", currencyFormat);
                ViewState["SumCr"] = SumCr.ToString("c", currencyFormat);
                ViewState["ExportTab"] = dtSubsidiary_New;
                grdSubsidiaryTrial.DataSource = dtSubsidiary_New;
                grdSubsidiaryTrial.DataBind();
                if (Request.QueryString["mainacc"] != null)
                {
                    grdSubsidiaryTrial.Columns[1].Visible = false;
                    grdSubsidiaryTrial.Columns[0].Visible = false;
                }
                if (rdbMainSelected.Checked == true)
                {
                    if (ClientValue[1].ToString().Trim() != "Customers" && ClientValue[1].ToString().Trim() != "NSDL Clients" && ClientValue[1].ToString().Trim() != "CDSL Clients")
                        grdSubsidiaryTrial.Columns[0].Visible = false;
                    else
                        grdSubsidiaryTrial.Columns[0].Visible = true;
                }
                if (rdBoth.Checked == true)
                {
                    grdSubsidiaryTrial.Columns[4].Visible = true;
                    grdSubsidiaryTrial.Columns[5].Visible = true;
                }
                else if (rdDebit.Checked == true)
                {
                    grdSubsidiaryTrial.Columns[4].Visible = false;
                    grdSubsidiaryTrial.Columns[5].Visible = true;
                }
                else if (rdCredit.Checked == true)
                {
                    grdSubsidiaryTrial.Columns[5].Visible = false;
                    grdSubsidiaryTrial.Columns[4].Visible = true;
                }
                if (rdbMainSelected.Checked == true)
                {
                    if (ClientValue[1].ToString().Trim() != "Customers" && ClientValue[1].ToString().Trim() != "NSDL Clients" && ClientValue[1].ToString().Trim() != "CDSL Clients")
                        grdSubsidiaryTrial.FooterRow.Cells[1].Text = "Total";
                    else
                        grdSubsidiaryTrial.FooterRow.Cells[0].Text = "Total";
                }

                if (SumDr == 0)
                    grdSubsidiaryTrial.FooterRow.Cells[4].Text = "";
                else
                    grdSubsidiaryTrial.FooterRow.Cells[4].Text = SumDr.ToString("c", currencyFormat);
                if (SumCr == 0)
                    grdSubsidiaryTrial.FooterRow.Cells[5].Text = "";
                else
                    grdSubsidiaryTrial.FooterRow.Cells[5].Text = SumCr.ToString("c", currencyFormat);
                grdSubsidiaryTrial.FooterRow.Cells[0].HorizontalAlign = HorizontalAlign.Left;
                grdSubsidiaryTrial.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Left;
                grdSubsidiaryTrial.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Right;
                grdSubsidiaryTrial.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Right;
                grdSubsidiaryTrial.FooterRow.Cells[0].ForeColor = System.Drawing.Color.White;
                grdSubsidiaryTrial.FooterRow.Cells[1].ForeColor = System.Drawing.Color.White;
                grdSubsidiaryTrial.FooterRow.Cells[4].ForeColor = System.Drawing.Color.White;
                grdSubsidiaryTrial.FooterRow.Cells[5].ForeColor = System.Drawing.Color.White;
                grdSubsidiaryTrial.FooterRow.Cells[0].Font.Bold = true;
                grdSubsidiaryTrial.FooterRow.Cells[1].Font.Bold = true;
                grdSubsidiaryTrial.FooterRow.Cells[4].Font.Bold = true;
                grdSubsidiaryTrial.FooterRow.Cells[5].Font.Bold = true;
                grdSubsidiaryTrial.FooterRow.Cells[4].Wrap = false;
                grdSubsidiaryTrial.FooterRow.Cells[5].Wrap = false;
                ScriptManager.RegisterStartupScript(this, GetType(), "JS", "ShowGrid();", true);
            }
            catch
            {
            }
        }
        protected void grdSubsidiaryTrial_RowCreated(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#FFE9BA';");
            //    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#FFFFFF';");
            //}
            string rowID = String.Empty;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                rowID = "row" + e.Row.RowIndex;

                e.Row.Attributes.Add("id", "row" + e.Row.RowIndex);

                e.Row.Attributes.Add("onclick", "ChangeRowColor(" + "'" + rowID + "','" + dtSubsidiary.Rows.Count + "'" + ")");

            }
            if (e.Row.RowType == DataControlRowType.Pager)
            {
                //LinkFirst = string.Empty;
                //LinkPrev = string.Empty;
                //LinkNext = string.Empty;
                //LinkLast = string.Empty;
                //GridViewRow row = e.Row;
                //LinkButton btnFirst = (LinkButton)row.FindControl("FirstPage");
                //LinkButton btnPrevious = (LinkButton)row.FindControl("PreviousPage");
                //LinkButton btnNext = (LinkButton)row.FindControl("NextPage");
                //LinkButton btnLast = (LinkButton)row.FindControl("LastPage");
                //LinkFirst = btnFirst.ClientID;
                //LinkPrev = btnPrevious.ClientID;
                //LinkNext = btnNext.ClientID;
                //LinkLast = btnLast.ClientID;
            }
        }

        protected void BtnDropdown_Click(object sender, EventArgs e)
        {
            FillDropDown();
            string[] ClientValue = cmbclientsPager.SelectedItem.Value.ToString().Split('~');
            string Type = ClientValue[1].ToString().Trim();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "JSC", "LedType('" + Type + "')", true);
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
                ScriptManager.RegisterStartupScript(this, GetType(), "hide", "DisableC('N');", true);
            }
            else if (curentIndex <= 0)
            {
                curentIndex = 0;
                ScriptManager.RegisterStartupScript(this, GetType(), "hide", "DisableC('P');", true);
            }
            cmbclientsPager.SelectedIndex = curentIndex;
            if (RadAsOnDate.Checked == true)
                FillGrid();
            else
                FillGridForPeriod();
        }
        public void FillDropDown()
        {
            MainAcc = HdnMainAcc.Value;
            DataTable Clients = oDBEngine.GetDataTable("master_mainaccount", " distinct MainAccount_AccountCode+'~'+MainAccount_SubLedgerType as mainaccount_accountcode,ltrim(rtrim(mainaccount_name))+' ['+mainaccount_accountcode+']' as mainaccount_name", " mainaccount_accountcode in(" + MainAcc + ")");
            cmbclientsPager.DataSource = Clients;
            cmbclientsPager.DataValueField = "mainaccount_accountcode";
            cmbclientsPager.DataTextField = "mainaccount_name";
            cmbclientsPager.DataBind();
            //FillGrid();
        }
        protected void cmbclientsPager_SelectedIndexChanged(object sender, EventArgs e)
        {
            //MainAcc = cmbclientsPager.SelectedItem.Value;
            FillGrid();
        }

        protected void grdSubsidiaryTrial_Sorting(object sender, GridViewSortEventArgs e)
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
            dtSubsidiary = (DataTable)ViewState["ExportTab"];
            System.Globalization.NumberFormatInfo currencyFormat = new System.Globalization.CultureInfo("hi-IN").NumberFormat;
            currencyFormat.CurrencySymbol = "";
            currencyFormat.CurrencyNegativePattern = 2;
            DataView dv = new DataView(dtSubsidiary);

            dv.Sort = sortExpression + direction;
            decimal SumDr = Convert.ToDecimal(dtSubsidiary.Compute("sum(DR)", ""));
            decimal SumCr = Convert.ToDecimal(dtSubsidiary.Compute("sum(CR)", ""));
            grdSubsidiaryTrial.DataSource = dv;
            grdSubsidiaryTrial.DataBind();
            grdSubsidiaryTrial.FooterRow.Cells[0].Text = "Total";
            if (SumDr == 0)
                grdSubsidiaryTrial.FooterRow.Cells[3].Text = "";
            else
                grdSubsidiaryTrial.FooterRow.Cells[3].Text = SumDr.ToString("c", currencyFormat);
            if (SumCr == 0)
                grdSubsidiaryTrial.FooterRow.Cells[4].Text = "";
            else
                grdSubsidiaryTrial.FooterRow.Cells[4].Text = SumCr.ToString("c", currencyFormat);
            grdSubsidiaryTrial.FooterRow.Cells[0].HorizontalAlign = HorizontalAlign.Left;
            grdSubsidiaryTrial.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Right;
            grdSubsidiaryTrial.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Right;
            grdSubsidiaryTrial.FooterRow.Cells[0].ForeColor = System.Drawing.Color.White;
            grdSubsidiaryTrial.FooterRow.Cells[3].ForeColor = System.Drawing.Color.White;
            grdSubsidiaryTrial.FooterRow.Cells[4].ForeColor = System.Drawing.Color.White;
            grdSubsidiaryTrial.FooterRow.Cells[0].Font.Bold = true;
            grdSubsidiaryTrial.FooterRow.Cells[3].Font.Bold = true;
            grdSubsidiaryTrial.FooterRow.Cells[4].Font.Bold = true;
            grdSubsidiaryTrial.FooterRow.Cells[3].Wrap = false;
            grdSubsidiaryTrial.FooterRow.Cells[4].Wrap = false;
        }

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
        void fn_Client()
        {
            BranchId = HdnBranchId.Value;
            Group = HdnGroup.Value;
            MainAcc = HdnMainAcc.Value;
            if (Request.QueryString["mainacc"] != null)
            {
                MainAcc = Request.QueryString["mainacc"].ToString();
                DataTable dtclient1 = new DataTable();
                dtclient1 = oDBEngine.GetDataTable("Trans_AccountsLedger", "AccountsLedger_SubAccountID", " AccountsLedger_MainAccountID in('" + MainAcc + "')");
                if (dtclient1.Rows.Count > 0)
                {
                    for (int i = 0; i < dtclient1.Rows.Count; i++)
                    {
                        if (Clients == null)
                            Clients = "'" + dtclient1.Rows[i][0].ToString() + "'";
                        else
                            Clients += "," + "'" + dtclient1.Rows[i][0].ToString() + "'";
                    }

                }
            }
            else
            {
                string[] ClientValue = null;
                ClientValue = cmbclientsPager.SelectedItem.Value.ToString().Split('~');
                if (ClientValue[1].ToString().Trim() == "Custom")
                {
                    DataTable dtclient = new DataTable();
                    dtclient = oDBEngine.GetDataTable("Trans_AccountsLedger", "AccountsLedger_SubAccountID", " AccountsLedger_MainAccountID in(" + MainAcc + ")");
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
                else if (ClientValue[1].ToString().Trim() == "Products-Equity")
                {
                    DataTable dtclient = new DataTable();
                    dtclient = oDBEngine.GetDataTable("Master_SubAccount", "SubAccount_Code", " SubAccount_MainAcreferenceID in(" + MainAcc + ")");
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
                    if (rdbClientALL.Checked)//////////////////ALL CLIENT CHECK
                    {
                        if (ddlGroup.SelectedItem.Value.ToString() == "1")
                        {
                            if (rdddlgrouptypeAll.Checked)
                            {
                                if (ddlgrouptype.SelectedItem.Value.ToString() != "0")
                                {
                                    DataTable dtclient = new DataTable();
                                    dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalid in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster where gpm_type='" + ddlgrouptype.SelectedItem.Text.ToString() + "'))");
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
                                dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalid in(select grp_contactid from tbl_trans_group where grp_groupmaster in(" + Group + "))");
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
                                dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ")");
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
                                dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_branchid in(" + BranchId + ")");
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
                                    dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalid in (select dpd_cntID from tbl_master_contactdpdetails where dpd_POA=1) and cnt_internalid in(select grp_contactid from tbl_trans_group where  grp_groupmaster in(select gpm_id from tbl_master_groupmaster where gpm_type='" + ddlgrouptype.SelectedItem.Text.ToString() + "'))");
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
                                dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalid in (select dpd_cntID from tbl_master_contactdpdetails where dpd_POA=1) and cnt_internalid in(select grp_contactid from tbl_trans_group where grp_groupmaster in(" + Group + "))");
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
                                dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalid in (select dpd_cntID from tbl_master_contactdpdetails where dpd_POA=1) and cnt_branchid in(" + Session["userbranchHierarchy"].ToString() + ")");
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
                                dtclient = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalID", " cnt_internalid in (select dpd_cntID from tbl_master_contactdpdetails where dpd_POA=1) and cnt_branchid in(" + BranchId + ")");
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

                if (rdbClientSelected.Checked == true)
                    Clients = HdnClients.Value;
            }
            ViewState["Clients"] = Clients;
        }
        void fn_ClientCDSL()
        {
            ViewState["Clients"] = null;
            BranchId = HdnBranchId.Value;
            Group = HdnGroup.Value;
            MainAcc = HdnMainAcc.Value;
            string NSDlCdsl = null;
            if (Session["userlastsegment"].ToString() == "10")
                NSDlCdsl = "select CDSLClients_ContactID from Master_CDSLClients where CDSLClients_ContactID is not null";
            if (Session["userlastsegment"].ToString() == "9")
                NSDlCdsl = "select NSDLClients_ContactID from Master_NSDLClients where NSDLClients_ContactID is not null";
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
                        if (Session["userlastsegment"].ToString() == "10")
                            dtclient = oDBEngine.GetDataTable("master_CdslClients", "CdslClients_BenAccountNumber", null);
                        else
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
                        if (Session["userlastsegment"].ToString() == "10")
                            dtclient = oDBEngine.GetDataTable("master_CdslClients", "CdslClients_BenAccountNumber", null);
                        else
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
                Clients = HdnClients.Value;
            ViewState["Clients"] = Clients;
        }

        public void FillGridForPeriod()
        {
            BranchId = HdnBranchId.Value;
            Group = HdnGroup.Value;
            MainAcc = HdnMainAcc.Value;
            if (Session["userlastsegment"].ToString() == "9" || Session["userlastsegment"].ToString() == "10")
                fn_ClientCDSL();
            else
                fn_Client();
            DataSet DS = new DataSet();
            DataTable dtSubsidiary_New = new DataTable();
            decimal Amountdr = 0;
            decimal AmountCr = 0;
            decimal OpenDr = 0;
            decimal OpenCr = 0;
            decimal CloseDr = 0;
            decimal CloseCr = 0;
            string SelectMainForSub = null;
            System.Globalization.NumberFormatInfo currencyFormat = new System.Globalization.CultureInfo("hi-IN").NumberFormat;
            currencyFormat.CurrencySymbol = "";
            currencyFormat.CurrencyNegativePattern = 2;
            pageSize = 15;
            string WehereMainAccount = null;
            string WhereMainBranch = null;
            decimal SumForBranchDR = 0;
            decimal SumForBranchCR = 0;
            decimal DifOfDRCR = 0;
            string BranchName = null;
            DateTime date;
            string[] ClientValue = null;
            if (rdbMainSelected.Checked == true)
                ClientValue = cmbclientsPager.SelectedItem.Value.ToString().Split('~');
            if (rdbMainSelected.Checked == true)
            {
                if (ClientValue[1].ToString().Trim() == "Customers" || ClientValue[1].ToString().Trim() == "NSDL Clients" || ClientValue[1].ToString().Trim() == "CDSL Clients")
                    SelectMainForSub = "C";
                else
                    SelectMainForSub = "N";
            }
            else
                SelectMainForSub = "N";
            if (ddlGroup.SelectedItem.Value == "0")
            {
                if (rdbranchAll.Checked == true)
                    WhereMainBranch = "and accountsledger_branchid in(" + Session["userbranchHierarchy"].ToString() + ")";
                else
                    WhereMainBranch = "and accountsledger_branchid in(" + BranchId + ")";
            }
            if (rdbMainAll.Checked == true)
                WehereMainAccount = null;
            else
                WehereMainAccount = " and accountsledger_mainaccountid in('" + ClientValue[0].ToString() + "')";
            if (ViewState["Clients"] != null)
                WehereMainAccount += " and accountsledger_subaccountid in(" + Clients + ")";
            date = Convert.ToDateTime(dtDate.Value);

            if (rdAllSegment.Checked == true)
            {
                DataTable DT = oDBEngine.GetDataTable("(select A.EXCH_INTERNALID AS SEGMENTID ,isnull((TME.EXH_ShortName + '-' + A.EXCH_SEGMENTID),exch_membershipType) AS EXCHANGENAME from (SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='" + Session["LastCompany"].ToString() + "' ) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID ) as D ", "top 10 *", null);
                SegmentID = DT.Rows[0][0].ToString();
                for (int i = 1; i < DT.Rows.Count; i++)
                {
                    SegmentID = SegmentID + "," + DT.Rows[i][0].ToString();
                }
            }
            else
            {
                if (BranchId == null || BranchId == "")
                {
                    SegmentID = ViewState["SegmentID"].ToString();
                }
                //SegmentID = Session["usersegid"].ToString();
            }
            if (WehereMainAccount == null)
                WehereMainAccount = "";
            if (WhereMainBranch == null)
                WhereMainBranch = "";
            DS = oManagement_BL.SubsidiaryTrialPeriodic(
                Convert.ToString(dtFrom.Value),
                 Convert.ToString(dtTo.Value),
                  Convert.ToString(SegmentID),
                 Convert.ToString(Session["LastFinYear"]),
                 Convert.ToString(Session["LastCompany"]),
                 Convert.ToString(WehereMainAccount),
                 Convert.ToString(WhereMainBranch),
                  Convert.ToString(SelectMainForSub)
                );

            dtSubsidiary_New = DS.Tables[0].Clone();
            if (ViewState["FirstTime"] == null)
            {
                info.AddMergedColumns(new int[] { 4, 5 }, "Opening");
                info.AddMergedColumns(new int[] { 6, 7 }, "Amount");
                info.AddMergedColumns(new int[] { 8, 9 }, "Closing");
            }
            for (int i = 0; i < DS.Tables[0].Rows.Count; i++)
            {

                if (i == 0)
                    BranchName = DS.Tables[0].Rows[i]["BranchName"].ToString();
                if (DS.Tables[0].Rows[i]["BranchName"].ToString() != BranchName)
                {
                    DataRow DrNew1 = dtSubsidiary_New.NewRow();
                    DrNew1[0] = "Total";
                    DrNew1[10] = oconverter.getFormattedvaluewithoriginalsign(SumForBranchDR);
                    DrNew1[11] = oconverter.getFormattedvaluewithoriginalsign(SumForBranchCR);
                    dtSubsidiary_New.Rows.Add(DrNew1);
                    DifOfDRCR = SumForBranchDR - SumForBranchCR;
                    DataRow DrNew2 = dtSubsidiary_New.NewRow();
                    DrNew2[0] = "Branch Net";
                    if (DifOfDRCR >= 0)
                        DrNew2[10] = oconverter.getFormattedvaluewithoriginalsign(DifOfDRCR);
                    else
                        DrNew2[11] = oconverter.getFormattedvaluewithoriginalsign(Math.Abs(DifOfDRCR));
                    dtSubsidiary_New.Rows.Add(DrNew2);
                    DifOfDRCR = 0;
                    SumForBranchDR = 0;
                    SumForBranchCR = 0;
                    DataRow DrNewBlank = dtSubsidiary_New.NewRow();
                    DrNewBlank[0] = "Blank";
                    dtSubsidiary_New.Rows.Add(DrNewBlank);
                }
                DataRow DrNew = dtSubsidiary_New.NewRow();
                DrNew[0] = DS.Tables[0].Rows[i]["BranchName"].ToString();
                DrNew[1] = DS.Tables[0].Rows[i]["MainID"].ToString();
                DrNew[2] = DS.Tables[0].Rows[i]["SubID"].ToString();
                DrNew[3] = DS.Tables[0].Rows[i]["accountsledger_mainaccountid"].ToString();
                DrNew[4] = DS.Tables[0].Rows[i]["accountsledger_subaccountid"].ToString();
                DrNew[5] = DS.Tables[0].Rows[i]["Ucc"].ToString();

                if (DS.Tables[0].Rows[i]["AmountDr"] != DBNull.Value)
                {
                    Amountdr += Convert.ToDecimal(DS.Tables[0].Rows[i]["AmountDr"].ToString());
                    DS.Tables[0].Rows[i]["AmountDr"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(DS.Tables[0].Rows[i]["AmountDr"].ToString()));
                    DrNew[8] = DS.Tables[0].Rows[i]["AmountDr"];
                }
                else
                    DrNew[8] = "";
                if (DS.Tables[0].Rows[i]["AmountCr"] != DBNull.Value)
                {
                    AmountCr += Convert.ToDecimal(DS.Tables[0].Rows[i]["AmountCr"].ToString());
                    DS.Tables[0].Rows[i]["AmountCr"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(DS.Tables[0].Rows[i]["AmountCr"].ToString()));
                    DrNew[9] = DS.Tables[0].Rows[i]["AmountCr"];
                }
                else
                    DrNew[9] = "";
                if (DS.Tables[0].Rows[i]["OpeningDR"] != DBNull.Value)
                {
                    OpenDr += Convert.ToDecimal(DS.Tables[0].Rows[i]["OpeningDR"].ToString());
                    DS.Tables[0].Rows[i]["OpeningDR"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(DS.Tables[0].Rows[i]["OpeningDR"].ToString()));
                    DrNew[6] = DS.Tables[0].Rows[i]["OpeningDR"];
                }
                else
                    DrNew[6] = "";
                if (DS.Tables[0].Rows[i]["OpeningCR"] != DBNull.Value)
                {
                    OpenCr += Convert.ToDecimal(DS.Tables[0].Rows[i]["OpeningCR"].ToString());
                    DS.Tables[0].Rows[i]["OpeningCR"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(DS.Tables[0].Rows[i]["OpeningCR"].ToString()));
                    DrNew[7] = DS.Tables[0].Rows[i]["OpeningCR"];
                }
                else
                    DrNew[7] = "";
                if (DS.Tables[0].Rows[i]["ClosingDR"] != DBNull.Value)
                {
                    CloseDr += Convert.ToDecimal(DS.Tables[0].Rows[i]["ClosingDR"].ToString());
                    SumForBranchDR += Convert.ToDecimal(DS.Tables[0].Rows[i]["ClosingDR"].ToString());
                    DS.Tables[0].Rows[i]["ClosingDR"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(DS.Tables[0].Rows[i]["ClosingDR"].ToString()));
                    DrNew[10] = DS.Tables[0].Rows[i]["ClosingDR"];
                }
                else
                    DrNew[10] = "";
                if (DS.Tables[0].Rows[i]["ClosingCR"] != DBNull.Value)
                {
                    CloseCr += Convert.ToDecimal(DS.Tables[0].Rows[i]["ClosingCR"].ToString());
                    SumForBranchCR += Convert.ToDecimal(DS.Tables[0].Rows[i]["ClosingCR"].ToString());
                    DS.Tables[0].Rows[i]["ClosingCR"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(DS.Tables[0].Rows[i]["ClosingCR"].ToString()));
                    DrNew[11] = DS.Tables[0].Rows[i]["ClosingCR"];
                }
                else
                    DrNew[11] = "";
                BranchName = DS.Tables[0].Rows[i]["BranchName"].ToString();
                dtSubsidiary_New.Rows.Add(DrNew);
                if (rdbMainSelected.Checked == true)
                {
                    if (ClientValue[1].ToString().Trim() == "Customers")
                    {
                        if (i == dtSubsidiary.Rows.Count - 1)
                        {
                            DataRow DrNew3 = dtSubsidiary_New.NewRow();
                            DrNew3[0] = "Total";
                            DrNew3[10] = oconverter.getFormattedvaluewithoriginalsign(SumForBranchDR);
                            DrNew3[11] = oconverter.getFormattedvaluewithoriginalsign(SumForBranchCR);
                            dtSubsidiary_New.Rows.Add(DrNew3);
                            DifOfDRCR = SumForBranchDR - SumForBranchCR;
                            DataRow DrNew4 = dtSubsidiary_New.NewRow();
                            DrNew4[0] = "Branch Net";
                            if (DifOfDRCR >= 0)
                                DrNew4[10] = oconverter.getFormattedvaluewithoriginalsign(DifOfDRCR);
                            else
                                DrNew4[11] = oconverter.getFormattedvaluewithoriginalsign(Math.Abs(DifOfDRCR));
                            dtSubsidiary_New.Rows.Add(DrNew4);
                            DifOfDRCR = 0;
                            SumForBranchDR = 0;
                            SumForBranchCR = 0;
                        }
                    }
                }
            }
            ViewState["ExportTab"] = dtSubsidiary_New;
            ViewState["Amountdr"] = Amountdr.ToString("c", currencyFormat);
            ViewState["AmountCr"] = AmountCr.ToString("c", currencyFormat);
            ViewState["OpenDr"] = OpenDr.ToString("c", currencyFormat);
            ViewState["OpenCr"] = OpenCr.ToString("c", currencyFormat);
            ViewState["CloseDr"] = CloseDr.ToString("c", currencyFormat);
            ViewState["CloseCr"] = CloseCr.ToString("c", currencyFormat);
            grdSubsidiaryTrialPeriod.DataSource = dtSubsidiary_New;
            grdSubsidiaryTrialPeriod.DataBind();
            if (dtSubsidiary_New.Rows.Count > 0)
            {
                if (rdbMainSelected.Checked == true)
                {
                    if (ClientValue[1].ToString().Trim() != "Customers")
                    {
                        grdSubsidiaryTrialPeriod.Columns[0].Visible = false;
                        grdSubsidiaryTrialPeriod.FooterRow.Cells[1].Text = "Total";
                    }
                    else
                    {
                        grdSubsidiaryTrialPeriod.Columns[0].Visible = true;
                        grdSubsidiaryTrialPeriod.FooterRow.Cells[0].Text = "Total";
                    }
                }
                if (OpenDr == 0)
                    grdSubsidiaryTrialPeriod.FooterRow.Cells[4].Text = "";
                else
                    grdSubsidiaryTrialPeriod.FooterRow.Cells[4].Text = OpenDr.ToString("c", currencyFormat);
                if (OpenCr == 0)
                    grdSubsidiaryTrialPeriod.FooterRow.Cells[5].Text = "";
                else
                    grdSubsidiaryTrialPeriod.FooterRow.Cells[5].Text = OpenCr.ToString("c", currencyFormat);
                if (Amountdr == 0)
                    grdSubsidiaryTrialPeriod.FooterRow.Cells[6].Text = "";
                else
                    grdSubsidiaryTrialPeriod.FooterRow.Cells[6].Text = Amountdr.ToString("c", currencyFormat);
                if (AmountCr == 0)
                    grdSubsidiaryTrialPeriod.FooterRow.Cells[7].Text = "";
                else
                    grdSubsidiaryTrialPeriod.FooterRow.Cells[7].Text = AmountCr.ToString("c", currencyFormat);
                if (CloseDr == 0)
                    grdSubsidiaryTrialPeriod.FooterRow.Cells[8].Text = "";
                else
                    grdSubsidiaryTrialPeriod.FooterRow.Cells[8].Text = CloseDr.ToString("c", currencyFormat);
                if (CloseCr == 0)
                    grdSubsidiaryTrialPeriod.FooterRow.Cells[9].Text = "";
                else
                    grdSubsidiaryTrialPeriod.FooterRow.Cells[9].Text = CloseCr.ToString("c", currencyFormat);
                grdSubsidiaryTrialPeriod.FooterRow.Cells[0].HorizontalAlign = HorizontalAlign.Left;
                grdSubsidiaryTrialPeriod.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Left;
                grdSubsidiaryTrialPeriod.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Right;
                grdSubsidiaryTrialPeriod.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Right;
                grdSubsidiaryTrialPeriod.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Right;
                grdSubsidiaryTrialPeriod.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Right;
                grdSubsidiaryTrialPeriod.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Right;
                grdSubsidiaryTrialPeriod.FooterRow.Cells[9].HorizontalAlign = HorizontalAlign.Right;
                grdSubsidiaryTrialPeriod.FooterRow.Cells[0].ForeColor = System.Drawing.Color.White;
                grdSubsidiaryTrialPeriod.FooterRow.Cells[1].ForeColor = System.Drawing.Color.White;
                grdSubsidiaryTrialPeriod.FooterRow.Cells[4].ForeColor = System.Drawing.Color.White;
                grdSubsidiaryTrialPeriod.FooterRow.Cells[5].ForeColor = System.Drawing.Color.White;
                grdSubsidiaryTrialPeriod.FooterRow.Cells[6].ForeColor = System.Drawing.Color.White;
                grdSubsidiaryTrialPeriod.FooterRow.Cells[7].ForeColor = System.Drawing.Color.White;
                grdSubsidiaryTrialPeriod.FooterRow.Cells[8].ForeColor = System.Drawing.Color.White;
                grdSubsidiaryTrialPeriod.FooterRow.Cells[9].ForeColor = System.Drawing.Color.White;
                grdSubsidiaryTrialPeriod.FooterRow.Cells[0].Font.Bold = true;
                grdSubsidiaryTrialPeriod.FooterRow.Cells[1].Font.Bold = true;
                grdSubsidiaryTrialPeriod.FooterRow.Cells[4].Font.Bold = true;
                grdSubsidiaryTrialPeriod.FooterRow.Cells[5].Font.Bold = true;
                grdSubsidiaryTrialPeriod.FooterRow.Cells[6].Font.Bold = true;
                grdSubsidiaryTrialPeriod.FooterRow.Cells[7].Font.Bold = true;
                grdSubsidiaryTrialPeriod.FooterRow.Cells[8].Font.Bold = true;
                grdSubsidiaryTrialPeriod.FooterRow.Cells[9].Font.Bold = true;
                grdSubsidiaryTrialPeriod.FooterRow.Cells[4].Wrap = false;
                grdSubsidiaryTrialPeriod.FooterRow.Cells[5].Wrap = false;
                grdSubsidiaryTrialPeriod.FooterRow.Cells[6].Wrap = false;
                grdSubsidiaryTrialPeriod.FooterRow.Cells[7].Wrap = false;
                grdSubsidiaryTrialPeriod.FooterRow.Cells[8].Wrap = false;
                grdSubsidiaryTrialPeriod.FooterRow.Cells[9].Wrap = false;

            }
            ScriptManager.RegisterStartupScript(this, GetType(), "JS", "ShowGrid1();", true);
            ViewState["FirstTime"] = "FirstTime";
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
            FillGridForPeriod();
        }
        protected void ddlExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RadAsOnDate.Checked == true)
            {
                dtSubsidiary = (DataTable)ViewState["ExportTab"];
                dtSubsidiary.Columns[1].ColumnName = "Main Account";
                dtSubsidiary.Columns[2].ColumnName = "Sub Account";
                dtSubsidiary.Columns[3].ColumnName = "UCC";
                dtSubsidiary.Columns[4].ColumnName = "Debit";
                dtSubsidiary.Columns[5].ColumnName = "Credit";
                dtSubsidiary.Columns.Remove("DR");
                dtSubsidiary.Columns.Remove("CR");
                DataRow NewRow = dtSubsidiary.NewRow();
                NewRow[1] = "Total";
                NewRow[4] = ViewState["SumDr"].ToString();
                NewRow[5] = ViewState["SumCr"].ToString();
                dtSubsidiary.Rows.Add(NewRow);
            }
            else
            {
                dtSubsidiary = (DataTable)ViewState["ExportTab"];
                dtSubsidiary.Columns[3].ColumnName = "Main Account";
                dtSubsidiary.Columns[4].ColumnName = "Sub Account";
                dtSubsidiary.Columns[5].ColumnName = "UCC";
                dtSubsidiary.Columns[6].ColumnName = "Opening Debit";
                dtSubsidiary.Columns[7].ColumnName = "Opening Credit";
                dtSubsidiary.Columns[8].ColumnName = "Amount Debit";
                dtSubsidiary.Columns[9].ColumnName = "Amount Credit";
                dtSubsidiary.Columns[10].ColumnName = "Closing Debit";
                dtSubsidiary.Columns[11].ColumnName = "Closing Credit";
                DataRow NewRow = dtSubsidiary.NewRow();
                NewRow[0] = "Total";
                NewRow[6] = ViewState["OpenDr"].ToString();
                NewRow[7] = ViewState["OpenCr"].ToString();
                NewRow[8] = ViewState["Amountdr"].ToString();
                NewRow[9] = ViewState["AmountCr"].ToString();
                NewRow[10] = ViewState["CloseDr"].ToString();
                NewRow[11] = ViewState["CloseCr"].ToString();
                dtSubsidiary.Rows.Add(NewRow);
                dtSubsidiary.Columns.Remove("MainID");
                dtSubsidiary.Columns.Remove("SubID");
            }

            DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            DataTable dtReportHeader = new DataTable();
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            HeaderRow[0] = CompanyName.Rows[0][0].ToString();
            dtReportHeader.Rows.Add(HeaderRow);
            DataRow DrRowR1 = dtReportHeader.NewRow();
            DrRowR1[0] = "Subsidiary Trial As On Date [" + oconverter.ArrangeDate2(dtDate.Value.ToString()) + "]";
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
                objExcel.ExportToExcelforExcel(dtSubsidiary, "Subsidiary Trial", "Total", dtReportHeader, dtReportFooter);
            }
            else if (ddlExport.SelectedItem.Value == "P")
            {
                objExcel.ExportToPDF(dtSubsidiary, "Subsidiary Trial", "Total", dtReportHeader, dtReportFooter);
            }
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
            grdSubsidiaryTrialPeriod.HeaderStyle.AddAttributesToRender(output);
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
        protected void grdSubsidiaryTrialPeriod_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.SetRenderMethodDelegate(RenderHeader);
            }
        }
        protected void grdSubsidiaryTrial_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow gvr = e.Row;
            if (gvr.RowType == DataControlRowType.Header)
            {
                if (ddlGroup.SelectedItem.Value == "0")
                    gvr.Cells[0].Text = "Branch Net";
                else
                    gvr.Cells[0].Text = "Group Net";
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (ViewState["SubType"] != null)
                {
                    if (ViewState["SubType"].ToString() == "Customers")
                    {
                        string lblMainID = ((DataRowView)e.Row.DataItem)["MainID"].ToString();
                        string lblSubID = ((DataRowView)e.Row.DataItem)["SubID"].ToString();

                        string MainAccName = ((DataRowView)e.Row.DataItem)["accountsledger_mainaccountid"].ToString();
                        string SubAccName = ((DataRowView)e.Row.DataItem)["accountsledger_subaccountid"].ToString();
                        string UCC = ((DataRowView)e.Row.DataItem)["Ucc"].ToString();
                        string dt = Convert.ToDateTime(dtDate.Value.ToString()).ToShortDateString();

                        //Label lblMainID = (Label)e.Row.FindControl("lblMainID");
                        //Label lblSubID = (Label)e.Row.FindControl("lblSubID");
                        ((Label)e.Row.FindControl("lblTrDate")).Attributes.Add("onclick", "javascript:ShowLedger('" + lblMainID + "','" + lblSubID + "','" + ViewState["SegmentID"].ToString() + "','" + MainAccName + "','" + SubAccName + "','" + UCC + "','" + dt + "');");
                        e.Row.Cells[1].Style.Add("cursor", "hand");
                        e.Row.Cells[1].ToolTip = "Click to View Detail!";
                    }
                }

                string BrName = ((DataRowView)e.Row.DataItem)["BranchName"].ToString();
                string AmtDr = ((DataRowView)e.Row.DataItem)["AmountDr"].ToString();
                string AmtCr = ((DataRowView)e.Row.DataItem)["AmountCr"].ToString();
                if (BrName == "Total")
                {
                    e.Row.Cells[0].ForeColor = System.Drawing.Color.Blue;
                    e.Row.Cells[4].ForeColor = System.Drawing.Color.Blue;
                    e.Row.Cells[5].ForeColor = System.Drawing.Color.Blue;
                }
                else if (BrName == "Branch Net")
                {
                    e.Row.Cells[0].ForeColor = System.Drawing.Color.Blue;
                    e.Row.Cells[4].ForeColor = System.Drawing.Color.Blue;
                    e.Row.Cells[5].ForeColor = System.Drawing.Color.Blue;
                }
                else if (BrName == "Group Net")
                {
                    e.Row.Cells[0].ForeColor = System.Drawing.Color.Blue;
                    e.Row.Cells[4].ForeColor = System.Drawing.Color.Blue;
                    e.Row.Cells[5].ForeColor = System.Drawing.Color.Blue;
                }
                else if (BrName == "Blank")
                {
                    e.Row.Cells[0].ForeColor = System.Drawing.Color.White;
                }

            }
        }
        protected void grdSubsidiaryTrialPeriod_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string BrName = ((DataRowView)e.Row.DataItem)["BranchName"].ToString();
                if (BrName == "Total")
                {
                    e.Row.Cells[0].ForeColor = System.Drawing.Color.Blue;
                    e.Row.Cells[8].ForeColor = System.Drawing.Color.Blue;
                    e.Row.Cells[9].ForeColor = System.Drawing.Color.Blue;
                }
                else if (BrName == "Branch Net")
                {
                    e.Row.Cells[0].ForeColor = System.Drawing.Color.Blue;
                    e.Row.Cells[8].ForeColor = System.Drawing.Color.Blue;
                    e.Row.Cells[9].ForeColor = System.Drawing.Color.Blue;
                }
                else if (BrName == "Blank")
                {
                    e.Row.Cells[0].ForeColor = System.Drawing.Color.White;
                }
            }
        }
    }
}