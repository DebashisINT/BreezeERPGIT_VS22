using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_frmReport_CashBankBookUpdate : System.Web.UI.Page
    {
        BusinessLogicLayer.Reports rep = new BusinessLogicLayer.Reports();
        ImportRoutines imprep = new ImportRoutines();
        ClsDropDownlistNameSpace.clsDropDownList clsdrp = new ClsDropDownlistNameSpace.clsDropDownList();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        DataTable dtReport = new DataTable();
        static DataTable dt = new DataTable();
        static string Olddate = null;
        static int ReptID = 0;
        int NoOfRowEffectedRow;
        static int ID = 0;
        DataTable DtTable = new DataTable();
        string strVoucherNo;
        string CashBankEntryID;
        DataTable dtUpdate = new DataTable();
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            dsCashBank.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            dsCompany.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SelectSegment.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            dsBranch.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            MainAccount.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            dsgrdClientbank.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            dsCashBank.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }

            Session["MainAccountContra"] = null;
            if (!IsPostBack)
            {
                CmbClientBank.ClientSideEvents.EndCallback = "function(s, e) {var str = ['Third Party Account', '', '','','']; s.InsertItem(0, str, -1);}";
                dteDate.EditFormatString = objConverter.GetDateFormat("Date");
                dteDate.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());

                if (Request.QueryString["date"] != null)
                {
                    dtReport.Clear();
                    dt.Clear();
                    ID = 0;
                    BindLocking();
                    BindEditingValue();
                }
                else
                {
                    dtReport.Clear();
                    dt.Clear();
                    ID = 0;
                    Session["KeyVal"] = "0";
                    try
                    {
                        string[,] compId = oDBEngine.GetFieldValue("tbl_Trans_Lastsegment", "ls_lastCompany,(select seg_name from tbl_master_segment where seg_id=tbl_Trans_Lastsegment.ls_lastsegment) as Segment", "ls_cntId='" + Session["usercontactID"].ToString() + "'", 2);
                        if (compId[0, 0] != "n")
                        {
                            cmbCompany.SelectedValue = compId[0, 0].ToString();
                            DataTable dtsegment = oDBEngine.GetDataTable("(SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='" + compId[0, 0].ToString() + "' ) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID ", "A.EXCH_INTERNALID AS SEGMENTID ,isnull((TME.EXH_ShortName + '-' + A.EXCH_SEGMENTID),exch_membershipType) AS EXCHANGENAME ", null);
                            cmbSegment.DataSource = dtsegment;
                            cmbSegment.DataTextField = "EXCHANGENAME";
                            cmbSegment.DataValueField = "SEGMENTID";
                            cmbSegment.DataBind();
                            cmbSegment.Items.Insert(0, new ListItem("None", "0"));
                            //cmbSegment.SelectedItem.Text = compId[0, 1].ToString();
                            DataTable DtSelect = oDBEngine.GetDataTable("(select exch_internalId, isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Segment from tbl_master_companyExchange where exch_compid in (select top 1 ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment=" + Session["userlastsegment"].ToString() + ") and exch_compId='" + compId[0, 0].ToString() + "') as D", "*", " Segment in(select seg_name from tbl_master_segment where seg_id=" + Session["userlastsegment"].ToString() + ")");
                            cmbSegment.SelectedValue = DtSelect.Rows[0][0].ToString();
                        }
                    }
                    catch
                    {
                    }
                }
                string[,] Payee = oDBEngine.GetFieldValue("tbl_master_contact", "cnt_internalId,isnull(ltrim(rtrim(cnt_firstName)),'')+' '+isnull(cnt_middlename,'')+' '+isnull(cnt_lastname,'')+' ['+isnull(cnt_shortname,'')+']'", "cnt_internalId like 'VR%'", 2);
                if (Payee[0, 0] != "n")
                {
                    clsdrp.AddDataToDropDownList(Payee, cmbPayee, true);
                }
                ScriptManager.RegisterStartupScript(this, GetType(), "JScript", "Page_Load()", true);
            }
            txtSettlementNo.Attributes.Add("onkeyup", "CallList(this,'SearchBySettlement',event)");
            txtIssuingBank.Attributes.Add("onkeyup", "CallListBank(this,'SearchByIssuingBank',event)");
        }
        public void BindLocking()
        {

        }
        public void BindEditingValue()
        {
            DataTable dtVal = oDBEngine.GetDataTable("trans_cashbankvouchers", "CashBank_ID", " ltrim(rtrim(CashBank_VoucherNumber))='" + Request.QueryString["vNo"].ToString().Trim() + "' and ltrim(rtrim(CashBank_CompanyID))='" + Request.QueryString["Compid"].ToString().Trim() + "' and ltrim(rtrim(CashBank_ExchangeSegmentID))='" + Request.QueryString["SegID"].ToString().Trim() + "' and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,ltrim(rtrim(CashBank_TransactionDate)))) as datetime)=cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + Request.QueryString["date"].ToString().Trim() + "')) as datetime)");
            if (dtVal.Rows.Count > 0)
            {
                Session["KeyVal"] = dtVal.Rows[0][0].ToString().Trim();
            }
            //dtReport = oDBEngine.GetDataTable("Trans_CashBankDetail AS A", "A.CashBankDetail_ID as CashReportID,A.CashBankDetail_PayeeAccountID as CashBank_PayeeAccountID,A.CashBankDetail_VoucherID as CashBank_ID,convert(varchar(11),A.CashBankDetail_InstrumentDate,113) as CashBank_InstrumentDate1,(select MainAccount_ReferenceID from master_mainaccount where MainAccount_AccountCode=A.CashBankDetail_MainAccountID) as CashBank_MainAccountID, A.CashBankDetail_SubAccountID  as SubAccountID,A.CashBankDetail_InstrumentType  as CashBank_InstrumentType,A.CashBankDetail_InstrumentNumber as CashBank_InstrumentNumber,A.CashBankDetail_InstrumentDate  as CashBank_InstrumentDate,A.CashBankDetail_PaymentAmount as CashBank_AmountWithdrawl,A.CashBankDetail_ReceiptAmount as CashBank_AmountDeposit,A.CASHBANKDETAIL_NARRATION AS LineNarration,A.CashBankDetail_DraftIssueBankBranch as IssuingBank,A.CashBankDetail_ClientBankID as CustomerBank,A.CashBankDetail_ThirdPartyReference as AuthLetterRef,
            //    (isnull((select isnull(ltrim(rtrim(cnt_firstname)),'')+' '+isnull(ltrim(rtrim(cnt_middlename)),'')+' '+isnull(ltrim(rtrim(cnt_lastname)),'')+' ['+isnull(ltrim(rtrim(cnt_ucc)),'')+']' from tbl_master_contact where cnt_internalid=A.CashBankDetail_SubAccountID),isnull((select top 1 subaccount_name+' ['+isnull(ltrim(rtrim(subaccount_code)),'')+']' from master_subaccount where cast(subaccount_code as varchar)=A.CashBankDetail_SubAccountID),isnull((select subaccount_name+' ['+isnull(ltrim(rtrim(subaccount_code)),'')+']' from master_subaccount where cast(subaccount_referenceid as varchar)=A.CashBankDetail_SubAccountID),isnull((select top 1 cdslclients_firstholdername+'['+isnull(ltrim(rtrim(cdslclients_benaccountnumber)),'')+']' from master_cdslclients where cast(cdslclients_benaccountnumber as varchar)=A.CashBankDetail_SubAccountID),(select nsdlclients_benfirstholdername+' ['+isnull(ltrim(rtrim(nsdlclients_benaccountid)),'')+']' from master_nsdlclients where cast(nsdlclients_benaccountid as varchar)=A.CashBankDetail_SubAccountID)))))) as SubAccount1,isnull((select MainAccount_Name from Master_MainAccount where cast(MainAccount_ReferenceId as varchar)=cast(A.CashBankDetail_MainAccountID as varchar)),isnull((select MainAccount_Name from master_mainaccount where mainaccount_accountcode=A.CashBankDetail_MainAccountID),'')) as MainAccount1,case A.CashBankDetail_InstrumentType when 'D' then 'Draft' when 'C' then 'Cheque' when 'E' then 'E. Trnsfr' else ' ' end as InstType1,(select isnull(ltrim(rtrim(cnt_firstname)),'')+' '+isnull(cnt_middlename,'')+' '+isnull(cnt_lastname,'') from tbl_master_contact where cnt_internalId=A.CashBankDetail_PayeeAccountId) as Payee1,isnull((select isnull(bnk_bankName,'')+ ' ~ '+ isnull(bnk_micrno,'')+' ~ '+isnull(bnk_branchName,'') from tbl_master_Bank where bnk_id=A.CashBankDetail_DraftIssueBankBranch),(select isnull(bnk_bankName,'')+ ' ~ '+ isnull(bnk_micrno,'')+' ~ '+isnull(bnk_branchName,'') from tbl_master_Bank where bnk_id=A.CashBankDetail_ClientBankID and A.CashBankDetail_IsThirdParty='Y')) as Bank,case A.CashBankDetail_PaymentAmount when '0.0000' then null else convert(varchar(50),cast(A.CashBankDetail_PaymentAmount as money),1) end as WithDrawl,case A.CashBankDetail_ReceiptAmount when '0.0000' then null else convert(varchar(50),cast(A.CashBankDetail_ReceiptAmount as money),1) end as Receipt,CashBankDetail_IsThirdParty as DraftYes,(select CashBank_TransactionType from trans_cashbankvouchers where CashBank_id=cashbankdetail_VoucherID) as Type,(select cast(CashBank_TransactionDate as varchar) from trans_cashbankvouchers where CashBank_id=cashbankdetail_VoucherID) as Date,(select CashBank_ExchangeSegmentID from trans_cashbankvouchers where CashBank_id=cashbankdetail_VoucherID) as Segement,(select CashBank_BranchID from trans_cashbankvouchers where CashBank_id=cashbankdetail_VoucherID) as Branch,CashBankDetail_CashBankID as CashBankAccount,(select CashBank_SettlementNumber from trans_cashbankvouchers where CashBank_id=cashbankdetail_VoucherID) as SettlementNo,(select CashBank_SettlementType from trans_cashbankvouchers where CashBank_id=cashbankdetail_VoucherID) as SettlementType,(select CashBank_Narration from trans_cashbankvouchers where CashBank_id=cashbankdetail_VoucherID) as Narration," + Session["userid"].ToString() + " as UserID,case when CashBankDetail_BankValueDate is null then 'NA' when CashBankDetail_BankValueDate='1900-01-01 00:00:00.000' then 'NA' else 'YA' end As BankValueDate,cast(CashBankDetail_BankValueDate as varchar) as ValueDate", " A.CashBankDetail_VoucherID='" + Session["KeyVal"].ToString() + "'");

            // dtReport = oDBEngine.GetDataTable("Trans_CashBankDetail AS A", "A.CashBankDetail_ID as CashReportID,A.CashBankDetail_PayeeAccountID as CashBank_PayeeAccountID,A.CashBankDetail_VoucherID as CashBank_ID,convert(varchar(11),A.CashBankDetail_InstrumentDate,113) as CashBank_InstrumentDate1,(select MainAccount_ReferenceID from master_mainaccount where MainAccount_AccountCode=A.CashBankDetail_MainAccountID) as CashBank_MainAccountID, A.CashBankDetail_SubAccountID  as SubAccountID,A.CashBankDetail_InstrumentType  as CashBank_InstrumentType,A.CashBankDetail_InstrumentNumber as CashBank_InstrumentNumber,A.CashBankDetail_InstrumentDate  as CashBank_InstrumentDate,A.CashBankDetail_PaymentAmount as CashBank_AmountWithdrawl,A.CashBankDetail_ReceiptAmount as CashBank_AmountDeposit,A.CASHBANKDETAIL_NARRATION AS LineNarration,A.CashBankDetail_DraftIssueBankBranch as IssuingBank,A.CashBankDetail_ClientBankID as CustomerBank,A.CashBankDetail_ThirdPartyReference as AuthLetterRef,
            //    (isnull((select isnull(ltrim(rtrim(cnt_firstname)),'')+' '+isnull(ltrim(rtrim(cnt_middlename)),'')+' '+isnull(ltrim(rtrim(cnt_lastname)),'')+' ['+isnull(ltrim(rtrim(cnt_ucc)),'')+']' from tbl_master_contact where cnt_internalid=A.CashBankDetail_SubAccountID),isnull((select top 1 subaccount_name+' ['+isnull(ltrim(rtrim(subaccount_code)),'')+']' from master_subaccount where cast(subaccount_code as varchar)=A.CashBankDetail_SubAccountID),isnull((select subaccount_name+' ['+isnull(ltrim(rtrim(subaccount_code)),'')+']' from master_subaccount where cast(subaccount_referenceid as varchar)=A.CashBankDetail_SubAccountID),isnull((select top 1 cdslclients_firstholdername+'['+isnull(ltrim(rtrim(cdslclients_benaccountnumber)),'')+']' from master_cdslclients where cast(cdslclients_benaccountnumber as varchar)=A.CashBankDetail_SubAccountID),(select nsdlclients_benfirstholdername+' ['+isnull(ltrim(rtrim(nsdlclients_benaccountid)),'')+']' from master_nsdlclients where cast(nsdlclients_benaccountid as varchar)=A.CashBankDetail_SubAccountID)))))) as SubAccount1,
            //isnull((select MainAccount_Name from Master_MainAccount where cast(MainAccount_ReferenceId as varchar)=cast(A.CashBankDetail_MainAccountID as varchar)),isnull((select MainAccount_Name from master_mainaccount where mainaccount_accountcode=A.CashBankDetail_MainAccountID),'')) as MainAccount1,case A.CashBankDetail_InstrumentType when 'D' then 'Draft' when 'C' then 'Cheque' when 'E' then 'E. Trnsfr' else ' ' end as InstType1,(select isnull(ltrim(rtrim(cnt_firstname)),'')+' '+isnull(cnt_middlename,'')+' '+isnull(cnt_lastname,'') from tbl_master_contact where cnt_internalId=A.CashBankDetail_PayeeAccountId) as Payee1,isnull((select isnull(bnk_bankName,'')+ ' ~ '+ isnull(bnk_micrno,'')+' ~ '+isnull(bnk_branchName,'') from tbl_master_Bank where bnk_id=A.CashBankDetail_DraftIssueBankBranch),(select isnull(bnk_bankName,'')+ ' ~ '+ isnull(bnk_micrno,'')+' ~ '+isnull(bnk_branchName,'') from tbl_master_Bank where bnk_id=A.CashBankDetail_ClientBankID and A.CashBankDetail_IsThirdParty='Y')) as Bank,case A.CashBankDetail_PaymentAmount when '0.0000' then null else convert(varchar(50),cast(A.CashBankDetail_PaymentAmount as money),1) end as WithDrawl,case A.CashBankDetail_ReceiptAmount when '0.0000' then null else convert(varchar(50),cast(A.CashBankDetail_ReceiptAmount as money),1) end as Receipt,CashBankDetail_IsThirdParty as DraftYes,(select CashBank_TransactionType from trans_cashbankvouchers where CashBank_id=cashbankdetail_VoucherID) as Type,(select cast(CashBank_TransactionDate as varchar) from trans_cashbankvouchers where CashBank_id=cashbankdetail_VoucherID) as Date,(select CashBank_ExchangeSegmentID from trans_cashbankvouchers where CashBank_id=cashbankdetail_VoucherID) as Segement,(select CashBank_BranchID from trans_cashbankvouchers where CashBank_id=cashbankdetail_VoucherID) as Branch,CashBankDetail_CashBankID as CashBankAccount,(select CashBank_SettlementNumber from trans_cashbankvouchers where CashBank_id=cashbankdetail_VoucherID) as SettlementNo,(select CashBank_SettlementType from trans_cashbankvouchers where CashBank_id=cashbankdetail_VoucherID) as SettlementType,(select CashBank_Narration from trans_cashbankvouchers where CashBank_id=cashbankdetail_VoucherID) as Narration," + Session["userid"].ToString() + " as UserID,case when CashBankDetail_BankValueDate is null then 'NA' when CashBankDetail_BankValueDate='1900-01-01 00:00:00.000' then 'NA' else 'YA' end As BankValueDate,cast(CashBankDetail_BankValueDate as varchar) as ValueDate", " A.CashBankDetail_VoucherID='" + Session["KeyVal"].ToString() + "'");

            dtReport = oDBEngine.GetDataTable("Trans_CashBankDetail AS A", "A.CashBankDetail_ID as CashReportID,A.CashBankDetail_PayeeAccountID as CashBank_PayeeAccountID,A.CashBankDetail_VoucherID as CashBank_ID,convert(varchar(11),A.CashBankDetail_InstrumentDate,113) as CashBank_InstrumentDate1,(select MainAccount_ReferenceID from master_mainaccount where MainAccount_AccountCode=A.CashBankDetail_MainAccountID) as CashBank_MainAccountID, A.CashBankDetail_SubAccountID  as SubAccountID,A.CashBankDetail_InstrumentType  as CashBank_InstrumentType,A.CashBankDetail_InstrumentNumber as CashBank_InstrumentNumber,A.CashBankDetail_InstrumentDate  as CashBank_InstrumentDate,A.CashBankDetail_PaymentAmount as CashBank_AmountWithdrawl,A.CashBankDetail_ReceiptAmount as CashBank_AmountDeposit,A.CASHBANKDETAIL_NARRATION AS LineNarration,A.CashBankDetail_DraftIssueBankBranch as IssuingBank,A.CashBankDetail_ClientBankID as CustomerBank,A.CashBankDetail_ThirdPartyReference as AuthLetterRef,(select subaccount_name  from master_subaccount where SubAccount_MainAcReferenceID=A.cashbankdetail_mainaccountid and SubAccount_Code=A.cashbankdetail_subaccountid) as SubAccount1,  isnull((select MainAccount_Name from Master_MainAccount where cast(MainAccount_ReferenceId as varchar)=cast(A.CashBankDetail_MainAccountID as varchar)),isnull((select MainAccount_Name from master_mainaccount where mainaccount_accountcode=A.CashBankDetail_MainAccountID),'')) as MainAccount1,case A.CashBankDetail_InstrumentType when 'D' then 'Draft' when 'C' then 'Cheque' when 'E' then 'E. Trnsfr' else ' ' end as InstType1,(select isnull(ltrim(rtrim(cnt_firstname)),'')+' '+isnull(cnt_middlename,'')+' '+isnull(cnt_lastname,'') from tbl_master_contact where cnt_internalId=A.CashBankDetail_PayeeAccountId) as Payee1,isnull((select isnull(bnk_bankName,'')+ ' ~ '+ isnull(bnk_micrno,'')+' ~ '+isnull(bnk_branchName,'') from tbl_master_Bank where bnk_id=A.CashBankDetail_DraftIssueBankBranch),(select isnull(bnk_bankName,'')+ ' ~ '+ isnull(bnk_micrno,'')+' ~ '+isnull(bnk_branchName,'') from tbl_master_Bank where bnk_id=A.CashBankDetail_ClientBankID and A.CashBankDetail_IsThirdParty='Y')) as Bank,case A.CashBankDetail_PaymentAmount when '0.0000' then null else convert(varchar(50),cast(A.CashBankDetail_PaymentAmount as money),1) end as WithDrawl,case A.CashBankDetail_ReceiptAmount when '0.0000' then null else convert(varchar(50),cast(A.CashBankDetail_ReceiptAmount as money),1) end as Receipt,CashBankDetail_IsThirdParty as DraftYes,(select CashBank_TransactionType from trans_cashbankvouchers where CashBank_id=cashbankdetail_VoucherID) as Type,(select cast(CashBank_TransactionDate as varchar) from trans_cashbankvouchers where CashBank_id=cashbankdetail_VoucherID) as Date,(select CashBank_ExchangeSegmentID from trans_cashbankvouchers where CashBank_id=cashbankdetail_VoucherID) as Segement,(select CashBank_BranchID from trans_cashbankvouchers where CashBank_id=cashbankdetail_VoucherID) as Branch,CashBankDetail_CashBankID as CashBankAccount,(select CashBank_SettlementNumber from trans_cashbankvouchers where CashBank_id=cashbankdetail_VoucherID) as SettlementNo,(select CashBank_SettlementType from trans_cashbankvouchers where CashBank_id=cashbankdetail_VoucherID) as SettlementType,(select CashBank_Narration from trans_cashbankvouchers where CashBank_id=cashbankdetail_VoucherID) as Narration," + Session["userid"].ToString() + " as UserID,case when CashBankDetail_BankValueDate is null then 'NA' when CashBankDetail_BankValueDate='1900-01-01 00:00:00.000' then 'NA' else 'YA' end As BankValueDate,cast(CashBankDetail_BankValueDate as varchar) as ValueDate", " A.CashBankDetail_VoucherID='" + Session["KeyVal"].ToString() + "'");


            if (dtReport.Rows.Count > 0)
            {
                ReptID = Convert.ToInt32(dtReport.Compute("max(CashReportID)", ""));
            }
            DataTable DtEdit = oDBEngine.GetDataTable("Trans_CashBankVouchers", "CashBank_TransactionType,CashBank_VoucherNumber,CashBank_CompanyID,CashBank_TransactionDate,ltrim(rtrim(CashBank_ExchangeSegmentID)) as CashBank_ExchangeSegmentID,ltrim(rtrim(CashBank_BranchID)) as CashBank_BranchID,CashBank_SettlementNumber,CashBank_SettlementType,CashBank_Narration,(select isnull(rtrim(settlements_Number),'')+ ' [ ' + isnull(rtrim(settlements_typeSuffix),'') + ' ]' as SettlementName from Master_Settlements where settlements_ID=Trans_CashBankVouchers.CashBank_SettlementNumber) as SettNumber,CashBank_CashBankID", " CashBank_ID='" + Session["KeyVal"].ToString() + "'");
            if (DtEdit.Rows.Count != 0)
            {
                cmbType.SelectedValue = DtEdit.Rows[0][0].ToString();
                ViewState["aa"] = DtEdit.Rows[0][0].ToString();
                dsCompany.SelectCommand = "SELECT COMP.CMP_INTERNALID AS CashBank_CompanyID , COMP.CMP_NAME AS CashBank_CompanyName  FROM TBL_MASTER_COMPANY AS COMP";
                cmbCompany.DataBind();
                cmbCompany.SelectedValue = DtEdit.Rows[0][2].ToString();
                dteDate.Value = Convert.ToDateTime(DtEdit.Rows[0][3].ToString());
                Olddate = DtEdit.Rows[0][3].ToString();
                ViewState["OldSegment"] = DtEdit.Rows[0][4].ToString();
                DataTable dtExch = oDBEngine.GetDataTable("(SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='" + DtEdit.Rows[0][2].ToString() + "' ) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID ", "A.EXCH_INTERNALID AS SEGMENTID ,isnull((TME.EXH_ShortName + '-' + A.EXCH_SEGMENTID),exch_membershipType) AS EXCHANGENAME ", null); //("(SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='" + DtEdit.Rows[0][2].ToString() + "') AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID", "LTRIM(RTRIM(A.EXCH_INTERNALID)) AS CashBank_ExchangeSegmentID ,TME.EXH_ShortName + '--' + A.EXCH_SEGMENTID AS EXCHANGENAME", null);
                if (dtExch.Rows.Count != 0)
                {
                    cmbSegment.DataSource = dtExch;
                    cmbSegment.DataTextField = "EXCHANGENAME";
                    cmbSegment.DataValueField = "SEGMENTID";
                    cmbSegment.DataBind();
                    cmbSegment.Items.Insert(0, new ListItem("None", "0"));
                    cmbSegment.SelectedValue = DtEdit.Rows[0][4].ToString();
                }
                txtVoucherNo.Text = DtEdit.Rows[0][1].ToString();
                dsBranch.SelectCommand = "SELECT BRANCH_id AS BANKBRANCH_ID , BRANCH_DESCRIPTION AS BANKBRANCH_NAME  FROM TBL_MASTER_BRANCH";
                cmbBranch.DataBind();
                cmbBranch.SelectedValue = DtEdit.Rows[0][5].ToString();
                txtNarration.Text = DtEdit.Rows[0][8].ToString();
                txtSettlementNo.Text = DtEdit.Rows[0][9].ToString();
                txtSettlementNo_hidden.Value = DtEdit.Rows[0][6].ToString();
                MainAccount.SelectCommand = "Select MainAccount_AccountCode as CashBank_MainAccountID, MainAccount_AccountCode+'-'+MainAccount_Name+' [ '+MainAccount_BankAcNumber+' ]'+' ~ '+MainAccount_BankCashType as MainAccount_Name from Master_MainAccount where MainAccount_BankCashType='Bank' or MainAccount_BankCashType='Cash'";
                cmbCashBankAc.DataBind();
                cmbCashBankAc.SelectedValue = DtEdit.Rows[0][10].ToString().Trim();
                txtVoucherNo.Enabled = false;
                txtSettlementNo.Enabled = false;
                cmbType.Enabled = false;
                cmbBranch.Enabled = false;
            }
            if (cmbType.SelectedValue == "C")
            {
                string main = "";
                for (int i = 0; i < dtReport.Rows.Count; i++)
                {
                    main += dtReport.Rows[i][4].ToString() + ",";
                }
                int main1 = main.LastIndexOf(",");
                main = main.Substring(0, main1);
                Session["MainAccountContra"] = main.ToString();
            }
            ViewState["mytable"] = dtReport;
            hddnEdit.Value = "Edit";
            grdAdd.DataSource = dtReport;
            grdAdd.DataBind();
            ScriptManager.RegisterStartupScript(this, GetType(), "JScript", "Page_Load1()", true);
        }
        protected void cmbType_SelectedIndexChanged1(object sender, EventArgs e)
        {
            if (Request.QueryString["date"] != null)
            {
                ViewState["add"] = "a";
            }
            else
            {

            }
            if (cmbType.SelectedItem.Value == "C")
            {
                grdAdd.Columns[2].Visible = true;
                grdAdd.Columns[3].Visible = true;
            }
            else
            {
                string cashBank = cmbCashBankAc.SelectedItem.Text;
                string[] Cashbank1 = cashBank.Split('~');
                if (Cashbank1[1].ToString() == " Cash")
                {
                    grdAdd.Columns[2].Visible = false;
                    grdAdd.Columns[3].Visible = false;
                }
                else
                {
                    grdAdd.Columns[2].Visible = true;
                    grdAdd.Columns[3].Visible = true;
                }
            }
        }
        protected void cmbCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbSegment.Items.Clear();
            string[,] Segment = oDBEngine.GetFieldValue("(SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='" + cmbCompany.SelectedItem.Value.ToString() + "') AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID", "LTRIM(RTRIM(A.EXCH_INTERNALID)) AS CashBank_ExchangeSegmentID ,TME.EXH_ShortName + '--' + A.EXCH_SEGMENTID AS EXCHANGENAME", null, 2);
            if (Segment[0, 0] != "n")
            {
                clsdrp.AddDataToDropDownList(Segment, cmbSegment, true);
            }
        }
        protected void CmbClientBank_OnCallback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string SubAccountRefID = e.Parameter;
            dsgrdClientbank.SelectParameters[0].DefaultValue = SubAccountRefID;
            CmbClientBank.Items.Clear();
            CmbClientBank.DataBind();
        }

        protected void grdAdd_RowEditing(object sender, GridViewEditEventArgs e)
        {
            //DropDownList ddlMain = (DropDownList)grdAdd.Rows[e.NewEditIndex].Cells[4].FindControl("ddlEditMainAccount");
            if (ViewState["mytable"] != null)
                dtReport = (DataTable)ViewState["mytable"];

            DataView dvData = new DataView(dtReport);
            dvData.RowFilter = "UserID = '" + Session["userid"].ToString() + "'";

            DataTable DtTab = new DataTable();
            DtTab = dvData.ToTable();

            ID = (int)grdAdd.DataKeys[e.NewEditIndex].Value;
            DataRow[] reportRow = DtTab.Select("CashReportID=" + ID + "");
            ViewState["SubID"] = reportRow[0][5].ToString();
            string MainID = reportRow[0][4].ToString();

            if (reportRow[0][32].ToString() == "YA")
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "JScriptAss", "alert('Entry already tagged,Modification disallowed');", true);
            }
            else
            {
                grdAdd.EditIndex = e.NewEditIndex;
                ViewState["mytable"] = DtTab;
                grdAdd.DataSource = dtReport;
                grdAdd.DataBind();
                if (cmbType.SelectedItem.Value == "P")
                {
                    if (reportRow[0][6].ToString() != "0")
                    {
                        if (reportRow[0][1].ToString().Trim() != "")
                        {
                            txtLineNarration.Text = reportRow[0][11].ToString();
                            cmbPayee.SelectedValue = reportRow[0][1].ToString();
                            ScriptManager.RegisterClientScriptBlock(this, GetType(), "JScript", "ShowLineForPayment1('" + txtLineNarration.Text + "','" + cmbPayee.SelectedValue + "')", true);
                        }
                        else
                        {
                            txtLineNarration.Text = reportRow[0][11].ToString();
                            ScriptManager.RegisterClientScriptBlock(this, GetType(), "JScript", "ShowLineForPayment('" + txtLineNarration.Text + "')", true);
                        }
                    }
                }
                else if (cmbType.SelectedItem.Value == "R")
                {
                    if (reportRow[0][6].ToString() == "D")
                    {
                        txtLineNarration.Text = reportRow[0][11].ToString();
                        txtIssuingBank.Text = reportRow[0][19].ToString();
                        txtIssuingBank_hidden.Value = reportRow[0][12].ToString();
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "JScript", "BankShow('" + txtLineNarration.Text + "','" + txtIssuingBank.Text + "','" + txtIssuingBank_hidden.Value + "')", true);
                    }
                    else
                    {
                        if (reportRow[0][6].ToString() == "C" || reportRow[0][6].ToString() == "E")
                        {
                            if (reportRow[0][22].ToString() == "Y")
                            {
                                txtLineNarration.Text = reportRow[0][11].ToString();
                                txtIssuingBank_hidden.Value = reportRow[0][13].ToString();
                                txtAuthLetterRef.Text = reportRow[0][14].ToString();
                                txtIssuingBank.Text = reportRow[0][19].ToString();
                                ScriptManager.RegisterClientScriptBlock(this, GetType(), "JScript", "BankShow2('" + txtLineNarration.Text + "','" + txtIssuingBank_hidden.Value + "','" + txtAuthLetterRef.Text + "','" + reportRow[0][5].ToString() + "','" + txtIssuingBank.Text + "')", true);
                            }
                            else
                            {
                                txtLineNarration.Text = reportRow[0][11].ToString();
                                CmbClientBank.Value = reportRow[0][13].ToString();
                                txtAuthLetterRef.Text = reportRow[0][14].ToString();
                                ScriptManager.RegisterClientScriptBlock(this, GetType(), "JScript", "BankShow1('" + txtLineNarration.Text + "','" + CmbClientBank.Value + "','" + txtAuthLetterRef.Text + "','" + reportRow[0][5].ToString() + "')", true);
                            }
                        }
                    }
                }
                if (cmbType.SelectedValue == "C")
                {
                    string main = "";
                    for (int i = 0; i < dtReport.Rows.Count; i++)
                    {
                        main += dtReport.Rows[i][4].ToString() + ",";
                    }
                    int main1 = main.LastIndexOf(",");
                    main = main.Substring(0, main1);
                    Session["MainAccountContra"] = main.ToString();
                }
                ScriptManager.RegisterStartupScript(this, GetType(), "JScript", "keyVal1('" + MainID + "')", true);
                grdAdd.FooterRow.Visible = false;
            }
        }
        protected void grdAdd_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int KeyName = (int)grdAdd.DataKeys[e.RowIndex].Value;
            if (ViewState["mytable"] != null)
                dtReport = (DataTable)ViewState["mytable"];

            DataView dvData = new DataView(dtReport);
            dvData.RowFilter = "UserID = '" + Session["userid"].ToString() + "'";

            DataTable DtTab = new DataTable();
            DtTab = dvData.ToTable();
            try
            {
                DataTable DtTabClone = DtTab.Clone();
                foreach (DataRow dr in DtTab.Rows)
                {
                    if (dr.ItemArray[0].ToString().Trim() != KeyName.ToString().Trim())
                        DtTabClone.ImportRow(dr);

                }

                foreach (DataRow dr in DtTabClone.Rows)
                {
                    if (dr.ItemArray[32].ToString() == "NA")
                    {
                        if (ViewState["Delete"] != null)
                        {
                            ViewState["Delete"] = ViewState["Delete"] + "," + KeyName + ",";
                        }
                        else
                        {
                            ViewState["Delete"] = KeyName + ",";
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "JScript", "alert('Entry already tagged,Deletion disallowed');", true);
                    }

                }
                if (ViewState["Delete"] != null)
                {
                    DtTab.Clear();
                    DtTab = DtTabClone.Copy();
                    int ii = ViewState["Delete"].ToString().LastIndexOf(",");
                    ViewState["Delete"] = ViewState["Delete"].ToString().Substring(0, ii);
                    DtTab.AcceptChanges();
                    ViewState["add"] = "a";
                    ViewState["mytable"] = DtTab;
                }
            }
            catch
            {
            }

            grdAdd.DataSource = dtReport.DefaultView;
            grdAdd.DataBind();
        }
        protected void grdAdd_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Insert")
            {
                string ExchID = "";
                string year = dteDate.Date.Year.ToString();
                string catchType = cmbType.SelectedItem.Value.ToString();
                DataRow dr;
                TextBox txtMainAccount = (TextBox)grdAdd.FooterRow.FindControl("txtMainAccount");
                HiddenField txtMainAccount_hidden = (HiddenField)grdAdd.FooterRow.FindControl("txtMainAccount_hidden");
                TextBox txtSubAccount = (TextBox)grdAdd.FooterRow.FindControl("txtSubAccount");
                HiddenField txtSubAccount_hidden = (HiddenField)grdAdd.FooterRow.FindControl("txtSubAccount_hidden");
                DropDownList ddlInstType = (DropDownList)grdAdd.FooterRow.FindControl("ddlInstType");
                TextBox txtInstNo = (TextBox)grdAdd.FooterRow.FindControl("txtInstNo");
                ASPxDateEdit dtAspxDate = (ASPxDateEdit)grdAdd.FooterRow.FindControl("dtAspxDate");
                ASPxTextBox txtWithdraw = (ASPxTextBox)grdAdd.FooterRow.FindControl("txtWithdraw");
                ASPxTextBox txtReceipt = (ASPxTextBox)grdAdd.FooterRow.FindControl("txtReceipt");
                try
                {
                    ExchID = cmbSegment.SelectedItem.Value;
                }
                catch
                {
                    ExchID = "";
                }
                string InstType = "";
                string InstType1 = "";
                if (ddlInstType.SelectedItem.Text != "Select")
                {
                    InstType = ddlInstType.SelectedItem.Value.ToString();
                    InstType1 = ddlInstType.SelectedItem.Text.ToString();
                }
                else
                {
                    InstType = "0";
                    InstType1 = "";
                }
                ReptID = ReptID + 1;
                if (dtReport.Rows.Count == 0)
                {
                    dtReport.Dispose();
                    dtReport = new DataTable();
                    dtReport.Columns.Add(new DataColumn("CashReportID", typeof(int))); //0
                    dtReport.Columns.Add(new DataColumn("CashBank_PayeeAccountID", typeof(String)));//1
                    dtReport.Columns.Add(new DataColumn("CashBank_ID", typeof(String)));//2
                    dtReport.Columns.Add(new DataColumn("CashBank_InstrumentDate1", typeof(String)));//3
                    dtReport.Columns.Add(new DataColumn("CashBank_MainAccountID", typeof(String)));//4
                    dtReport.Columns.Add(new DataColumn("SubAccountID", typeof(String)));//5
                    dtReport.Columns.Add(new DataColumn("CashBank_InstrumentType", typeof(String)));//6
                    dtReport.Columns.Add(new DataColumn("CashBank_InstrumentNumber", typeof(String)));//7
                    dtReport.Columns.Add(new DataColumn("CashBank_InstrumentDate", typeof(DateTime)));//8
                    dtReport.Columns.Add(new DataColumn("CashBank_AmountWithdrawl", typeof(Decimal)));//9
                    dtReport.Columns.Add(new DataColumn("CashBank_AmountDeposit", typeof(Decimal)));//10
                    dtReport.Columns.Add(new DataColumn("LineNarration", typeof(String)));//11
                    dtReport.Columns.Add(new DataColumn("IssuingBank", typeof(String)));//12
                    dtReport.Columns.Add(new DataColumn("CustomerBank", typeof(string)));//13
                    dtReport.Columns.Add(new DataColumn("AuthLetterRef", typeof(string)));//14
                    dtReport.Columns.Add(new DataColumn("SubAccount1", typeof(string)));//15
                    dtReport.Columns.Add(new DataColumn("MainAccount1", typeof(string)));//16
                    dtReport.Columns.Add(new DataColumn("InstType1", typeof(string)));//17
                    dtReport.Columns.Add(new DataColumn("Payee1", typeof(string)));//18
                    dtReport.Columns.Add(new DataColumn("Bank", typeof(string)));//19
                    dtReport.Columns.Add(new DataColumn("WithDrawl", typeof(string)));//20
                    dtReport.Columns.Add(new DataColumn("Receipt", typeof(string)));//21
                    dtReport.Columns.Add(new DataColumn("DraftYes", typeof(string)));//22
                    dtReport.Columns.Add(new DataColumn("Type", typeof(Char))); //23
                    dtReport.Columns.Add(new DataColumn("Date", typeof(String)));//24
                    dtReport.Columns.Add(new DataColumn("Segement", typeof(String)));//25
                    dtReport.Columns.Add(new DataColumn("Branch", typeof(String)));//26
                    dtReport.Columns.Add(new DataColumn("CashBankAccount", typeof(String)));//27
                    dtReport.Columns.Add(new DataColumn("SettlementNo", typeof(String)));//28
                    dtReport.Columns.Add(new DataColumn("SettlementType", typeof(String)));//29
                    dtReport.Columns.Add(new DataColumn("Narration", typeof(String)));//30
                    dtReport.Columns.Add(new DataColumn("UserID", typeof(String)));//31       
                    dtReport.Columns.Add(new DataColumn("BankValueDate", typeof(String)));//32    
                    dtReport.Columns.Add(new DataColumn("ValueDate", typeof(String)));//33    
                }
                if (ViewState["mytable"] != null)
                    dtReport = (DataTable)ViewState["mytable"];

                DataRow drReport = dtReport.NewRow();
                drReport[0] = ReptID.ToString();
                try
                {
                    drReport[1] = cmbPayee.SelectedItem.Value.ToString();
                }
                catch
                {
                    drReport[1] = "0";
                }
                drReport[2] = Session["KeyVal"].ToString();
                drReport[3] = objConverter.ArrangeDate(Convert.ToDateTime(dtAspxDate.Value.ToString()).ToShortDateString());
                //drReport[3] = e.NewValues["CashBank_InstrumentDate"];
                string[] mainval = txtMainAccount_hidden.Value.ToString().Split('~');
                drReport[4] = mainval[0].ToString();
                drReport[5] = txtSubAccount_hidden.Value.ToString();
                drReport[6] = InstType;
                drReport[7] = txtInstNo.Text;
                drReport[8] = dtAspxDate.Value.ToString();
                try
                {
                    drReport[9] = txtWithdraw.Text;
                    if (txtWithdraw.Text == "0.00")
                    {
                        drReport[20] = "";
                    }
                    else
                    {
                        drReport[20] = objConverter.getFormattedvalue(Convert.ToDecimal(txtWithdraw.Text));
                    }
                }
                catch
                {
                    drReport[9] = "0.00";
                    drReport[20] = "";
                }
                try
                {
                    drReport[10] = txtReceipt.Text;
                    if (txtReceipt.Text == "0.00")
                    {
                        drReport[21] = "";
                    }
                    else
                    {
                        drReport[21] = objConverter.getFormattedvalue(Convert.ToDecimal(txtReceipt.Text));
                    }
                }
                catch
                {
                    drReport[10] = "0.00";
                    drReport[21] = "";
                }
                if (txtLineNarration.Text == "Line Narration")
                    drReport[11] = "";
                else
                    drReport[11] = txtLineNarration.Text;
                try
                {
                    drReport[12] = txtIssuingBank_hidden.Value;
                    drReport[19] = txtIssuingBank.Text;
                }
                catch
                {
                    drReport[12] = "0";
                }
                if (CmbClientBank.Value != null)
                {
                    drReport[13] = CmbClientBank.Value;
                }
                else
                {
                    drReport[13] = "0";
                }
                drReport[14] = txtAuthLetterRef.Text;
                drReport[15] = txtSubAccount.Text;
                drReport[16] = txtMainAccount.Text;
                drReport[17] = InstType1;
                try
                {
                    drReport[18] = cmbPayee.SelectedItem.Text;
                }
                catch
                {
                    drReport[18] = "";
                }

                drReport[23] = cmbType.SelectedItem.Value.ToString();
                drReport[24] = dteDate.Value.ToString();
                drReport[25] = ExchID;
                drReport[26] = cmbBranch.SelectedItem.Value.ToString();
                drReport[27] = cmbCashBankAc.SelectedItem.Value.ToString();
                drReport[28] = txtSettlementNo_hidden.Value;
                drReport[29] = txtSettlementNo.Text;
                drReport[30] = txtNarration.Text;
                drReport[31] = Session["userid"].ToString();
                drReport[32] = "NA";
                drReport[33] = "1900-01-01";
                dtReport.Rows.Add(drReport);
                dtReport.AcceptChanges();
                ViewState["mytable"] = dtReport;

                DataView dvData = new DataView(dtReport);
                dvData.RowFilter = "UserID = '" + Session["userid"].ToString() + "'";
                ViewState["add"] = "a";
                ID = 0;
                ViewState["Footer"] = "Footer";
                grdAdd.DataSource = dtReport.DefaultView;
                grdAdd.DataBind();
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "Jscript", "clear1()", true);
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "Jscript", "OnInstmentTypeChange('" + ddlInstType.ClientID + "')", true);
            }
        }
        protected void grdAdd_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                string value = "";
                if (ViewState["aa"] != null)
                {
                    value = ViewState["aa"].ToString();
                }
                else
                {
                    value = cmbType.SelectedItem.Value;
                }
                GridViewRow row = e.Row;
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    if (value == "C")
                    {
                        row.Cells[0].Text = "Cash/Bank A/c";
                        row.Cells[5].Text = "WithDrawl";
                        row.Cells[6].Text = "Deposit";
                        row.Cells[1].Visible = false;
                    }
                    else
                    {
                        row.Cells[0].Text = "MainAccount";
                        row.Cells[5].Text = "Payment";
                        row.Cells[6].Text = "Receipt";
                        row.Cells[1].Visible = true;
                    }
                }
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    ASPxDateEdit date = (ASPxDateEdit)row.FindControl("dtAspxDate");
                    ASPxTextBox payment = (ASPxTextBox)row.FindControl("txtWithdraw");
                    ASPxTextBox receipt = (ASPxTextBox)row.FindControl("txtReceipt");
                    date.EditFormatString = objConverter.GetDateFormat("Date");
                    date.Value = Convert.ToDateTime(dteDate.Value);
                    if (value == "C")
                    {
                        row.Cells[1].Visible = false;
                    }
                    else
                    {
                        row.Cells[1].Visible = true;
                        if (value == "R")
                        {
                            receipt.ClientEnabled = true;
                            payment.ClientEnabled = false;

                        }
                        else
                        {
                            receipt.ClientEnabled = false;
                            payment.ClientEnabled = true;
                        }
                    }
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (value == "C")
                    {
                        row.Cells[1].Visible = false;
                    }
                    else
                    {
                        row.Cells[1].Visible = true;
                        ASPxTextBox payment = (ASPxTextBox)row.FindControl("txtEditWithdraw");
                        ASPxTextBox receipt = (ASPxTextBox)row.FindControl("txtEditRecpt");
                        if (value == "R")
                        {
                            receipt.ClientEnabled = true;
                            payment.ClientEnabled = false;

                        }
                        else
                        {
                            receipt.ClientEnabled = false;
                            payment.ClientEnabled = true;
                        }
                    }
                }
            }
            catch
            {
            }
        }
        protected void grdAdd_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow row = e.Row;
            if (e.Row.RowType == DataControlRowType.Footer)
            {

                if (cmbType.SelectedItem.Value != "C")
                {
                    if (ViewState["Footer"] != null)
                    {
                        if (ViewState["mytable"] != null)
                            dtReport = (DataTable)ViewState["mytable"];

                        DataView dvData = new DataView(dtReport);
                        dvData.RowFilter = " UserID = '" + Session["userid"].ToString() + "'";

                        DataTable DtTab = new DataTable();
                        DtTab = dvData.ToTable();

                        TextBox MainAccount = (TextBox)row.FindControl("txtMainAccount");
                        HiddenField MainAccount_hidden = (HiddenField)row.FindControl("txtMainAccount_hidden");
                        DropDownList InstType = (DropDownList)row.FindControl("ddlInstType");
                        TextBox InstNo = (TextBox)row.FindControl("txtInstNo");
                        if (DtTab.Rows.Count > 0)
                        {
                            int pos = DtTab.Rows.Count - 1;
                            MainAccount.Text = DtTab.Rows[pos][16].ToString();
                            MainAccount_hidden.Value = DtTab.Rows[pos][4].ToString();
                            InstType.SelectedValue = DtTab.Rows[pos][6].ToString();
                            string InstrumentType = DtTab.Rows[pos][6].ToString();
                            if (cmbType.SelectedValue == "P")
                            {
                                if (InstrumentType == "C")
                                {
                                    if (DtTab.Rows[pos][7].ToString() != "")
                                    {
                                        string FinalVal = null;
                                        int Length = Convert.ToInt32(DtTab.Rows[pos][7].ToString().Length);
                                        int InstrumentNo = Convert.ToInt32(DtTab.Rows[pos][7].ToString());
                                        InstrumentNo = InstrumentNo + 1;
                                        int Length1 = InstrumentNo.ToString().Length;
                                        int Length2 = Length - Length1;
                                        for (int i = 0; i < Length2; i++)
                                        {
                                            FinalVal += "0";
                                        }
                                        FinalVal = FinalVal + Convert.ToString(InstrumentNo);
                                        InstNo.Text = FinalVal;
                                    }
                                }
                            }
                        }
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "JScript", "keyVal1(" + MainAccount_hidden.Value + ")", true);
                        TextBox SubAccount = (TextBox)row.FindControl("txtSubAccount");
                        string var = SubAccount.ClientID;
                        ScriptManager.RegisterStartupScript(this, GetType(), "Jscript", "SetSubAcc('" + var + "')", true);
                    }
                }
                else if (cmbType.SelectedValue == "C")
                {
                    if (ViewState["Footer"] != null)
                    {
                        DataView dvData = new DataView(dtReport);
                        dvData.RowFilter = " UserID = '" + Session["userid"].ToString() + "'";

                        DataTable DtTab = new DataTable();
                        DtTab = dvData.ToTable();

                        decimal diff = 0;
                        TextBox MainAccount1 = (TextBox)row.FindControl("txtMainAccount");
                        DropDownList InstType1 = (DropDownList)row.FindControl("ddlInstType");
                        TextBox InstNo1 = (TextBox)row.FindControl("txtInstNo");
                        ASPxTextBox withdraw = (ASPxTextBox)row.FindControl("txtWithdraw");
                        ASPxTextBox receipt = (ASPxTextBox)row.FindControl("txtReceipt");
                        ASPxDateEdit date = (ASPxDateEdit)row.FindControl("dtAspxDate");
                        Button btnAdd = (Button)row.FindControl("Button1");
                        decimal withdraw1 = Convert.ToDecimal(DtTab.Compute("Sum(CashBank_AmountWithdrawl)", string.Empty).ToString());
                        decimal receipt1 = Convert.ToDecimal(DtTab.Compute("Sum(CashBank_AmountDeposit)", string.Empty).ToString());
                        int pos1 = DtTab.Rows.Count - 1;
                        InstType1.SelectedValue = DtTab.Rows[pos1][6].ToString();
                        if (DtTab.Rows[pos1][7].ToString() != "")
                        {
                            string InstrumentNo = DtTab.Rows[pos1][7].ToString();
                            InstNo1.Text = InstrumentNo.ToString();
                        }
                        if (DtTab.Rows[pos1][9].ToString() != "")
                        {
                            if (withdraw1 > receipt1)
                            {
                                diff = withdraw1 - receipt1;
                                receipt.Value = diff;
                            }
                            else
                            {
                                diff = receipt1 - withdraw1;
                                withdraw.Value = diff;
                            }

                        }
                        if (DtTab.Rows[pos1][10].ToString() != "")
                        {
                            if (withdraw1 > receipt1)
                            {
                                diff = withdraw1 - receipt1;
                                receipt.Value = diff;
                            }
                            else
                            {
                                diff = receipt1 - withdraw1;
                                withdraw.Value = diff;
                            }
                        }
                        if (diff == 0)
                        {
                            btnAdd.Enabled = false;
                        }
                        string main = "";
                        for (int i = 0; i < DtTab.Rows.Count; i++)
                        {
                            main += DtTab.Rows[i][4].ToString() + ",";
                        }
                        int main1 = main.LastIndexOf(",");
                        main = main.Substring(0, main1);
                        Session["MainAccountContra"] = main.ToString();
                        withdraw.ClientEnabled = false;
                        receipt.ClientEnabled = false;
                        InstType1.Enabled = false;
                        InstNo1.Enabled = false;
                        date.ClientEnabled = false;
                        string var1 = MainAccount1.ClientID;
                        ScriptManager.RegisterStartupScript(this, GetType(), "Jscript", "SetSubAcc1('" + var1 + "')", true);
                    }
                }
            }
        }

        protected void grdAdd_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grdAdd.EditIndex = -1;
            if (ViewState["mytable"] != null)
                dtReport = (DataTable)ViewState["mytable"];

            DataView dvData = new DataView(dtReport);
            dvData.RowFilter = "UserID = '" + Session["userid"].ToString() + "'";

            DataTable DtTab = new DataTable();
            DtTab = dvData.ToTable();

            grdAdd.DataSource = DtTab;
            grdAdd.DataBind();
            ScriptManager.RegisterStartupScript(this, GetType(), "Jscript", "clear1()", true);
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "Jscript", "InvisibleAll()", true);
            grdAdd.FooterRow.Visible = true;
        }
        protected void grdAdd_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            TextBox txtMainAccount = (TextBox)grdAdd.Rows[e.RowIndex].FindControl("txtEditMainAccount");
            HiddenField txtMainAccount_hidden = (HiddenField)grdAdd.Rows[e.RowIndex].FindControl("txtEditMainAccount_hidden");
            TextBox txtSubAccount = (TextBox)grdAdd.Rows[e.RowIndex].FindControl("txtEditSubAccount");
            HiddenField txtSubAccount_hidden = (HiddenField)grdAdd.Rows[e.RowIndex].FindControl("txtEditSubAccount_hidden");
            DropDownList ddlInstType = (DropDownList)grdAdd.Rows[e.RowIndex].FindControl("ddlEditInstType");
            TextBox txtInstNo = (TextBox)grdAdd.Rows[e.RowIndex].FindControl("txtEditInstNumber");
            ASPxDateEdit dtAspxDate = (ASPxDateEdit)grdAdd.Rows[e.RowIndex].FindControl("dtEditAspxDate");
            ASPxTextBox txtWithdraw = (ASPxTextBox)grdAdd.Rows[e.RowIndex].FindControl("txtEditWithdraw");
            ASPxTextBox txtReceipt = (ASPxTextBox)grdAdd.Rows[e.RowIndex].FindControl("txtEditRecpt");
            string InstType = "";
            string InstType1 = "";
            if (ViewState["mytable"] != null)
                dtReport = (DataTable)ViewState["mytable"];

            DataView dvData = new DataView(dtReport);
            dvData.RowFilter = "UserID = '" + Session["userid"].ToString() + "'";

            DataTable DtTab = new DataTable();
            DtTab = dvData.ToTable();
            if (ddlInstType.SelectedItem.Text != "Select")
            {
                InstType = ddlInstType.SelectedItem.Value.ToString();
                InstType1 = ddlInstType.SelectedItem.Text.ToString();
            }
            else
            {
                InstType = "0";
                InstType1 = "";
            }
            DataRow[] reportRow = DtTab.Select("CashReportID=" + ID + "");
            try
            {
                reportRow[0][1] = cmbPayee.SelectedItem.Value.ToString();
            }
            catch
            {
                reportRow[0][1] = "0";
            }
            reportRow[0][2] = Session["KeyVal"].ToString();
            reportRow[0][3] = objConverter.ArrangeDate(Convert.ToDateTime(dtAspxDate.Value.ToString()).ToShortDateString());
            //reportRow[0][3] = e.NewValues["CashBank_InstrumentDate"];
            string[] mainval = txtMainAccount_hidden.Value.ToString().Split('~');
            reportRow[0][4] = mainval[0].ToString();
            reportRow[0][5] = txtSubAccount_hidden.Value.ToString();
            reportRow[0][6] = InstType;
            reportRow[0][7] = txtInstNo.Text;
            reportRow[0][8] = dtAspxDate.Value;
            try
            {
                reportRow[0][9] = txtWithdraw.Text;
                reportRow[0][20] = objConverter.getFormattedvalue(Convert.ToDecimal(txtWithdraw.Text));
            }
            catch
            {
                reportRow[0][9] = "";
            }
            try
            {
                reportRow[0][10] = txtReceipt.Text;
                reportRow[0][21] = objConverter.getFormattedvalue(Convert.ToDecimal(txtReceipt.Text));
            }
            catch
            {
                reportRow[0][10] = "";
            }
            reportRow[0][11] = txtLineNarration.Text;
            try
            {
                reportRow[0][12] = txtIssuingBank_hidden.Value;
                reportRow[0][19] = txtIssuingBank.Text;
            }
            catch
            {
                reportRow[0][12] = "0";
            }
            if (CmbClientBank.Value != null)
            {
                reportRow[0][13] = CmbClientBank.Value;
            }
            else
            {
                reportRow[0][13] = "0";
            }
            reportRow[0][14] = txtAuthLetterRef.Text;
            reportRow[0][15] = txtSubAccount.Text;
            reportRow[0][16] = txtMainAccount.Text;
            reportRow[0][17] = InstType1;
            try
            {
                reportRow[0][18] = cmbPayee.SelectedItem.Text;
            }
            catch
            {
                reportRow[0][18] = "";
            }
            DtTab.AcceptChanges();
            ViewState["add"] = "a";
            ViewState["mytable"] = DtTab;
            grdAdd.EditIndex = -1;
            grdAdd.DataSource = DtTab;
            grdAdd.DataBind();
            ScriptManager.RegisterStartupScript(this, GetType(), "JScript", "clear1()", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "Jscript", "InvisibleAll()", true);
            grdAdd.FooterRow.Visible = true;
        }
        protected void btnInsert_Click(object sender, EventArgs e)
        {
            int OnlyForBRS = 0;
            DateTime FinStartDate = Convert.ToDateTime(Session["FinYearStart"].ToString());
            DateTime DtLedgerDate = Convert.ToDateTime(dteDate.Value);
            if (FinStartDate > DtLedgerDate)
                OnlyForBRS = 1;

            int counter = 0;
            string FinYear = Session["LastFinYear"].ToString();

            /////////////////For XML
            if (ViewState["mytable"] != null)
                dtReport = (DataTable)ViewState["mytable"];

            DataView dvData = new DataView(dtReport);
            dvData.RowFilter = "UserID = '" + Session["userid"].ToString() + "'";

            DataTable DtTab = new DataTable();
            DtTab = dvData.ToTable();

            for (int i = 0; i < DtTab.Rows.Count; i++)
            {
                if (txtNarration.Text.ToString().Trim() != "")
                {
                    DtTab.Rows[i]["Narration"] = txtNarration.Text.ToString().Trim();

                }
            }

            string withdraw = DtTab.Compute("Sum(CashBank_AmountWithdrawl)", string.Empty).ToString();
            string receipt = DtTab.Compute("Sum(CashBank_AmountDeposit)", string.Empty).ToString();

            string tabledata = objConverter.ConvertDataTableToXML(DtTab);
            //End        
            if (Request.QueryString["date"] == null)
            {
                DataSet DSInst = new DataSet();
                String conn = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                SqlConnection con = new SqlConnection(conn);
                if (cmbType.SelectedItem.Value != "C")
                {
                    string InstrumentNumber = "";
                    if (cmbType.SelectedItem.Value != "C")
                    {
                        for (int k = 0; k < DtTab.Rows.Count; k++)
                        {
                            if (DtTab.Rows[k][7].ToString() != "")
                            {
                                if (InstrumentNumber == "")
                                    InstrumentNumber = "'" + DtTab.Rows[k][7].ToString() + "'";
                                else
                                    InstrumentNumber += "," + "'" + DtTab.Rows[k][7].ToString() + "'";
                            }
                        }
                    }
                    if (InstrumentNumber != "")
                    {
                        DSInst = rep.CheckingForInstrumentNumber(cmbCashBankAc.SelectedItem.Value, InstrumentNumber);
                        if (DSInst.Tables[0].Rows[0][0] != DBNull.Value)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "JScript123", "AlertInstNumber('" + DSInst.Tables[0].Rows[0][0].ToString() + "')", true);
                        }
                        else
                        {
                            //con.Open();
                            //SqlCommand com = new SqlCommand("xmlCashBankInsert", con);
                            //com.CommandType = CommandType.StoredProcedure;
                            //com.Parameters.AddWithValue("@cashBankData", tabledata);
                            //com.Parameters.AddWithValue("@createuser", Session["userid"].ToString());
                            //com.Parameters.AddWithValue("@finyear", FinYear);
                            //com.Parameters.AddWithValue("@compID", cmbCompany.SelectedItem.Value.ToString());
                            //com.Parameters.AddWithValue("@CashBankName", cmbCashBankAc.SelectedItem.Value.ToString());
                            //com.Parameters.AddWithValue("@TDate", Convert.ToDateTime(dteDate.Value));
                            //com.Parameters.AddWithValue("@BRS", OnlyForBRS);
                            if (cmbType.SelectedValue == "C")
                            {
                                if (withdraw == receipt)
                                {
                                    //com.ExecuteNonQuery();
                                    //com.CommandTimeout = 0;

                                    imprep.xmlCashBankInsert(tabledata, Session["userid"].ToString(), FinYear, cmbCompany.SelectedItem.Value.ToString(),
                                        cmbCashBankAc.SelectedItem.Value.ToString(), dteDate.Value.ToString(), OnlyForBRS);
                                    Session["MainAccountContra"] = null;
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this, GetType(), "JScript", "alert('WithDraw and Deposit must be Same')", true);
                                }
                            }
                            else
                            {
                                imprep.xmlCashBankInsert(tabledata, Session["userid"].ToString(), FinYear, cmbCompany.SelectedItem.Value.ToString(),
                                       cmbCashBankAc.SelectedItem.Value.ToString(), dteDate.Value.ToString(), OnlyForBRS);
                                //com.ExecuteNonQuery();
                                //com.CommandTimeout = 0;
                            }
                            //con.Close();
                            ScriptManager.RegisterStartupScript(this, GetType(), "JScript589", "parent.editwin.close();", true);
                        }
                    }
                    else
                    {
                        con.Open();
                        SqlCommand com = new SqlCommand("xmlCashBankInsert", con);
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@cashBankData", tabledata);
                        com.Parameters.AddWithValue("@createuser", Session["userid"].ToString());
                        com.Parameters.AddWithValue("@finyear", FinYear);
                        com.Parameters.AddWithValue("@compID", cmbCompany.SelectedItem.Value.ToString());
                        com.Parameters.AddWithValue("@CashBankName", cmbCashBankAc.SelectedItem.Value.ToString());
                        com.Parameters.AddWithValue("@TDate", Convert.ToDateTime(dteDate.Value));
                        com.Parameters.AddWithValue("@BRS", OnlyForBRS);
                        if (cmbType.SelectedValue == "C")
                        {
                            if (withdraw == receipt)
                            {
                                //com.ExecuteNonQuery();
                                //com.CommandTimeout = 0;
                                imprep.xmlCashBankInsert(tabledata, Session["userid"].ToString(), FinYear, cmbCompany.SelectedItem.Value.ToString(),
                                       cmbCashBankAc.SelectedItem.Value.ToString(), dteDate.Value.ToString(), OnlyForBRS);
                                Session["MainAccountContra"] = null;
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "JScript", "alert('WithDraw and Deposit must be Same')", true);
                            }
                        }
                        else
                        {
                            //com.ExecuteNonQuery();
                            //com.CommandTimeout = 0;
                            imprep.xmlCashBankInsert(tabledata, Session["userid"].ToString(), FinYear, cmbCompany.SelectedItem.Value.ToString(),
                                       cmbCashBankAc.SelectedItem.Value.ToString(), dteDate.Value.ToString(), OnlyForBRS);
                        }
                        // con.Close();
                        // Mantis Issue 24802
                        if (con.State == ConnectionState.Open)
                        {
                            con.Close();
                        }
                        // End of Mantis Issue 24802
                        ScriptManager.RegisterStartupScript(this, GetType(), "JScript589", "parent.editwin.close();", true);
                    }
                }
                else
                {
                    //con.Open();
                    //SqlCommand com = new SqlCommand("xmlCashBankInsert", con);
                    //com.CommandType = CommandType.StoredProcedure;
                    //com.Parameters.AddWithValue("@cashBankData", tabledata);
                    //com.Parameters.AddWithValue("@createuser", Session["userid"].ToString());
                    //com.Parameters.AddWithValue("@finyear", FinYear);
                    //com.Parameters.AddWithValue("@compID", cmbCompany.SelectedItem.Value.ToString());
                    //com.Parameters.AddWithValue("@CashBankName", cmbCashBankAc.SelectedItem.Value.ToString());
                    //com.Parameters.AddWithValue("@TDate", Convert.ToDateTime(dteDate.Value));
                    //com.Parameters.AddWithValue("@BRS", OnlyForBRS);
                    if (cmbType.SelectedValue == "C")
                    {
                        if (withdraw == receipt)
                        {
                            //com.ExecuteNonQuery();
                            //com.CommandTimeout = 0;
                            imprep.xmlCashBankInsert(tabledata, Session["userid"].ToString(), FinYear, cmbCompany.SelectedItem.Value.ToString(),
                                       cmbCashBankAc.SelectedItem.Value.ToString(), dteDate.Value.ToString(), OnlyForBRS);
                            Session["MainAccountContra"] = null;
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "JScript", "alert('WithDraw and Deposit must be Same')", true);
                        }
                    }
                    else
                    {
                        //com.ExecuteNonQuery();
                        //com.CommandTimeout = 0;
                        imprep.xmlCashBankInsert(tabledata, Session["userid"].ToString(), FinYear, cmbCompany.SelectedItem.Value.ToString(),
                                       cmbCashBankAc.SelectedItem.Value.ToString(), dteDate.Value.ToString(), OnlyForBRS);
                    }
                    con.Close();
                    //Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>parent.editwin.close();</script>");
                    ScriptManager.RegisterStartupScript(this, GetType(), "JScript589", "parent.editwin.close();", true);
                }

            }
            else
            {
                //String conn = ConfigurationManager.AppSettings["DBConnectionDefault"];
                //SqlConnection con = new SqlConnection(conn);
                //con.Open();
                //SqlCommand com = new SqlCommand("xmlCashBankUpdate", con);
                //com.CommandType = CommandType.StoredProcedure;
                //com.Parameters.AddWithValue("@cashBankData", tabledata);
                //com.Parameters.AddWithValue("@createuser", Session["userid"].ToString());
                //com.Parameters.AddWithValue("@finyear", FinYear);
                //com.Parameters.AddWithValue("@compID", cmbCompany.SelectedItem.Value.ToString());
                //com.Parameters.AddWithValue("@CashBankName", cmbCashBankAc.SelectedItem.Value.ToString());
                //com.Parameters.AddWithValue("@OldDate", Olddate);
                //com.Parameters.AddWithValue("@TDate", Convert.ToDateTime(dteDate.Value));
                //com.Parameters.AddWithValue("@cashBankName1", cmbCashBankAc.SelectedItem.Value);
                //com.Parameters.AddWithValue("@NewSegment", cmbSegment.SelectedItem.Value);
                //com.Parameters.AddWithValue("@OldSegment", ViewState["OldSegment"].ToString());
                //com.Parameters.AddWithValue("@BRS", OnlyForBRS);
                if (cmbType.SelectedValue == "C")
                {
                    if (withdraw == receipt)
                    {
                        //com.ExecuteNonQuery();
                        //com.CommandTimeout = 0;
                        rep.xmlCashBankUpdate(tabledata, Session["userid"].ToString(), FinYear,
     cmbCompany.SelectedItem.Value.ToString(), cmbCashBankAc.SelectedItem.Value.ToString(), Olddate,
     dteDate.Value.ToString(), cmbCashBankAc.SelectedItem.Value, cmbSegment.SelectedItem.Value, ViewState["OldSegment"].ToString(), OnlyForBRS);
                        Session["MainAccountContra"] = null;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "JScript", "alert('WithDraw and Deposit must be Same')", true);
                    }
                }
                else
                {
                    //com.ExecuteNonQuery();
                    //com.CommandTimeout = 0;
                    rep.xmlCashBankUpdate(tabledata, Session["userid"].ToString(), FinYear,
    cmbCompany.SelectedItem.Value.ToString(), cmbCashBankAc.SelectedItem.Value.ToString(), Olddate,
    dteDate.Value.ToString(), cmbCashBankAc.SelectedItem.Value, cmbSegment.SelectedItem.Value, ViewState["OldSegment"].ToString(), OnlyForBRS);

                }
                // con.Close();
                //Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>parent.editwin.close();</script>");
                ScriptManager.RegisterStartupScript(this, GetType(), "JScript589", "parent.editwin.close();", true);

            }
            if (ViewState["Delete"] != null)
            {
                SqlConnection lcon = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                //oDBEngine.DeleteValue("trans_journalvoucherdetail", " journalvoucherdetail_ID in(" + ViewState["Delete"].ToString() + ")");
                string[] delte = ViewState["Delete"].ToString().Split(',');
                for (int j = 0; j < delte.Length; j++)
                {
                    SqlCommand lcmd = new SqlCommand("deletecashbankVoucher", lcon);
                    lcmd.CommandType = CommandType.StoredProcedure;
                    lcmd.Parameters.Add("@companyId", SqlDbType.VarChar, 12).Value = cmbCompany.SelectedItem.Value;
                    lcmd.Parameters.Add("@VoucherID", SqlDbType.Int).Value = Convert.ToInt32(delte[j].ToString());
                    lcon.Open();
                    NoOfRowEffectedRow = lcmd.ExecuteNonQuery();
                    lcmd.Dispose();
                    lcon.Close();
                }
                ViewState["Delete"] = null;
            }
        }
        protected void cmbCashBankAc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbType.SelectedItem.Value != "C")
            {
                string cashBank = cmbCashBankAc.SelectedItem.Text;
                string[] Cashbank1 = cashBank.Split('~');
                if (Cashbank1[1].ToString() == " Cash")
                {
                    grdAdd.Columns[2].Visible = false;
                    grdAdd.Columns[3].Visible = false;
                }
                else
                {
                    grdAdd.Columns[2].Visible = true;
                    grdAdd.Columns[3].Visible = true;
                }
            }
            else
            {
                grdAdd.Columns[2].Visible = true;
                grdAdd.Columns[3].Visible = true;
            }
        }
        [System.Web.Services.WebMethod]
        public static string GetContactName(string custid)
        {
            string closingBalance = null;
            DBEngine oDbEngine1 = new DBEngine();
            Converter objConverter1 = new Converter();
            string[] dateandparam = custid.Split('~');
            DateTime date = Convert.ToDateTime(objConverter1.DateConverter1(dateandparam[0].ToString(), "mm/dd/yyyy"));
            if (dateandparam[1] == "1")
            {
                DataTable dtClose = oDbEngine1.OpeningBalance(dateandparam[4], null, date, dateandparam[3], dateandparam[2]);
                decimal balance = Convert.ToDecimal(dtClose.Rows[0][0].ToString()) * (-1);
                closingBalance = objConverter1.getFormattedvalue(balance);
            }
            else
            {
                string[,] mainacc = oDbEngine1.GetFieldValue("master_mainaccount", "MainAccount_Accountcode", "mainaccount_referenceid=" + dateandparam[4] + "", 1);
                string mainaccCode = mainacc[0, 0];
                DataTable dtClose = oDbEngine1.OpeningBalance(mainaccCode, dateandparam[5], date, dateandparam[3], dateandparam[2]);
                closingBalance = dtClose.Rows[0][0].ToString();
            }
            return closingBalance;
        }
        protected void btnAllVouDelete_Click(object sender, EventArgs e)
        {
            string ReturnValue = "0";
            DataSet DS = new DataSet();
            DateTime date = Convert.ToDateTime(Convert.ToDateTime(Request.QueryString["date"].ToString()).ToShortDateString());
            string VoucherNumber = Request.QueryString["vNo"].ToString();
            string CompanyID = Request.QueryString["Compid"].ToString();
            string SegmentID = Request.QueryString["SegID"].ToString();
            string FinYear = Session["LastFinYear"].ToString();

            //String conn = ConfigurationSettings.AppSettings["DBConnectionDefault"];
            //SqlConnection con = new SqlConnection(conn);

            //SqlCommand cmd3 = new SqlCommand("CashBankVoucherDelete", con);
            //cmd3.CommandType = CommandType.StoredProcedure;
            //cmd3.Parameters.AddWithValue("@Date", date);
            //cmd3.Parameters.AddWithValue("@VoucherNumber", VoucherNumber);
            //cmd3.Parameters.AddWithValue("@CompID", CompanyID);
            //cmd3.Parameters.AddWithValue("@SegmentID", SegmentID);
            //cmd3.Parameters.AddWithValue("@Finyear", FinYear);
            //cmd3.CommandTimeout = 0;
            //SqlDataAdapter Adap = new SqlDataAdapter();
            //Adap.SelectCommand = cmd3;
            //Adap.Fill(DS);
            DS = rep.CashBankVoucherDelete(date, VoucherNumber, CompanyID, Convert.ToInt32(SegmentID), FinYear);
            ReturnValue = DS.Tables[0].Rows[0][0].ToString();
            //cmd3.Dispose();
            //con.Dispose();
            //GC.Collect();
            if (ReturnValue == "2")
                ScriptManager.RegisterStartupScript(this, GetType(), "JScript58", "alert('Delete Successfully');", true);
            else if (ReturnValue == "3")
                ScriptManager.RegisterStartupScript(this, GetType(), "JScript59", "alert('Entry Already Tagged,Deletion DisAllowed');", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "JScript589", "parent.editwin.close();", true);
        }
    }
}