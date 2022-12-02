using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
//using DevExpress.Web;
using DevExpress.Web;
using BusinessLogicLayer;

namespace ERP.OMS.Management
{
    public partial class management_journalPopup : System.Web.UI.Page
    {
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        Management_BL oManagement_BL = new Management_BL();
        DataTable DtTable = new DataTable();
        DataTable dtReport = new DataTable();
        static int ReptID = 0;
        static int ID = 0;
        static string ForJournalDate = null;
        string JournalEntryID = null;
        int NoofRowsAffected = 0;
        protected void Page_Init(object sender, EventArgs e)
        {
            dsCompany.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            dsBranch.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
               //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (!IsPostBack)
            {
                ddlCompany.DataBind();
                ddlCompany_SelectedIndexChanged(sender, e);
                tDate.EditFormatString = objConverter.GetDateFormat("Date");
                string fDate = null;
                ForJournalDate = Session["FinYearEnd"].ToString();
                //int month = oDBEngine.GetDate().Month;
                //int date = oDBEngine.GetDate().Day;
                //int Year = oDBEngine.GetDate().Year;
                int month = oDBEngine.GetDate().Month;
                int date = oDBEngine.GetDate().Day;
                int Year = oDBEngine.GetDate().Year;

                if (Convert.ToDateTime(ForJournalDate).Date < oDBEngine.GetDate())
                {
                    fDate = Convert.ToDateTime(ForJournalDate).Month.ToString() + "/" + Convert.ToDateTime(ForJournalDate).Day.ToString() + "/" + Convert.ToDateTime(ForJournalDate).Year.ToString();
                }
                else
                {
                    if (month < 3)
                        fDate = month.ToString() + "/" + date.ToString() + "/" + Convert.ToDateTime(Session["FinYearStart"].ToString()).Year.ToString();
                    else
                        fDate = month.ToString() + "/" + date.ToString() + "/" + Convert.ToDateTime(Session["FinYearEnd"].ToString()).Year.ToString();
                }
                //tDate.Value = Convert.ToDateTime(DateTime.Today);
                tDate.Value = oDBEngine.GetDate();

                if (Request.QueryString["id"] != null)
                {
                    dtReport.Clear();
                    ReptID = 0;
                    ID = 0;
                    BindGridForEdit();
                    Page.ClientScript.RegisterStartupScript(GetType(), "JSc", "<script language='javascript'>Narration_Off()</script>");
                }
                else
                {
                    dtReport.Clear();
                    ReptID = 0;
                    ID = 0;
                    txtAccountCode.Text = "JV";
                    BindGridForFooter();
                    Session["KeyVal"] = "0";
                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load()</script>");

                    //string[,] id = oDBEngine.GetFieldValue("(select exch_internalId,isnull(((select exh_shortname from tbl_master_exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId),exch_membershiptype) as Name from tbl_master_companyExchange) as D", "exch_internalId", " Name in(select seg_name from tbl_master_segment where seg_id=" + Session["userlastsegment"].ToString() + ")", 1);
                    //if (id[0, 0] != "n")
                    //{
                    //    ddlSegment.SelectedValue = id[0, 0];
                    //}
                    try
                    {
                        //string[,] compId = oDBEngine.GetFieldValue("tbl_Trans_Lastsegment", "ls_lastCompany,(select seg_name from tbl_master_segment where seg_id=tbl_Trans_Lastsegment.ls_lastsegment) as Segment", "ls_cntId='" + Session["usercontactID"].ToString() + "'", 2);
                        //if (compId[0, 0] != "n")
                        //{
                        ddlCompany.SelectedValue = HttpContext.Current.Session["LastCompany"].ToString();
                        ddlCompany.Enabled = false;
                        DataTable dtsegment = oDBEngine.GetDataTable("(SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='" + HttpContext.Current.Session["LastCompany"].ToString() + "' ) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID ", "A.EXCH_INTERNALID AS SEGMENTID ,isnull((TME.EXH_ShortName + '-' + A.EXCH_SEGMENTID),exch_membershipType) AS EXCHANGENAME ", null);
                        ddlSegment.DataSource = dtsegment;
                        ddlSegment.DataTextField = "EXCHANGENAME";
                        ddlSegment.DataValueField = "SEGMENTID";
                        ddlSegment.DataBind();
                        ddlSegment.Items.Insert(0, new ListItem("None", "0"));
                        //cmbSegment.SelectedItem.Text = compId[0, 1].ToString();
                        //DataTable DtSelect = oDBEngine.GetDataTable("(select exch_internalId, isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Segment from tbl_master_companyExchange where exch_compid in (select top 1 ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment=" + Session["userlastsegment"].ToString() + ") and exch_compId='" + HttpContext.Current.Session["LastCompany"].ToString() + "') as D", "*", " Segment in(select seg_name from tbl_master_segment where seg_id=" + Session["userlastsegment"].ToString() + ")");
                        // DataTable DtSelect = oDBEngine.GetDataTable(" (select exch_internalId,isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Segment from tbl_master_companyExchange where   exch_compId='" + HttpContext.Current.Session["LastCompany"].ToString() + "') as D ", "top 1 * ", "exch_internalid='" + Session["usersegid"].ToString() + "' ");

                        DataTable DtSelect = new DataTable();
                        if (Session["userlastsegment"].ToString().Trim() == "9" || Session["userlastsegment"].ToString().Trim() == "10")
                        {
                            DtSelect = oDBEngine.GetDataTable(" (select exch_internalId,isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Segment from tbl_master_companyExchange where   exch_compId='" + HttpContext.Current.Session["LastCompany"].ToString() + "') as D ", "top 1 * ", "exch_internalid  IN (SELECT TOP 1  EXCH_INTERNALID FROM TBL_MASTER_COMPANYEXCHANGE   WHERE EXCH_TMCODE='" + Session["usersegid"].ToString() + "') ");
                        }
                        else
                        {
                            DtSelect = oDBEngine.GetDataTable(" (select exch_internalId,isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Segment from tbl_master_companyExchange where   exch_compId='" + HttpContext.Current.Session["LastCompany"].ToString() + "') as D ", "top 1 * ", "exch_internalid='" + Session["usersegid"].ToString() + "' ");
                        }

                        if (DtSelect.Rows.Count > 0)
                            ddlSegment.SelectedValue = DtSelect.Rows[0][0].ToString();
                        if (Session["userlastsegment"].ToString().Trim() == "5")
                            ddlSegment.Enabled = false;
                        //}

                        //if (Session["userlastsegment"].ToString().Trim() == "5")
                        //{
                        //    if (DtSelect.Rows.Count > 0)
                        //        ddlSegment.SelectedValue = "0";
                        //    ddlSegment.Enabled = false;
                        //}
                        //else
                        //{

                        if (DtSelect.Rows.Count > 0)
                            ddlSegment.SelectedValue = DtSelect.Rows[0][0].ToString();
                        ddlSegment.Enabled = false;
                        // }



                    }
                    catch
                    {
                    }
                    ddlIntExchange.SelectedValue = "0";
                }
                Tdebit.Visible = false;
            }
            Page.ClientScript.RegisterStartupScript(GetType(), "JS", "<script language='javascript'>Page_Load1()</script>");
            ddlIntExchange.Attributes.Add("onchange", "SelectSegment()");
            ddlTntraExchange.Attributes.Add("onchange", "SegmentName()");
            txtSettno.Attributes.Add("onkeyup", "CallList(this,'SearchBySettlement',event)");
        }
        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlSegment.Items.Clear();

            //DataTable dtExch = oDBEngine.GetDataTable("(SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='" + ddlCompany.SelectedItem.Value.ToString() + "' ) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID ", "A.EXCH_INTERNALID AS SEGMENTID ,isnull((TME.EXH_ShortName + '-' + A.EXCH_SEGMENTID),exch_membershipType) AS EXCHANGENAME ", null);
            //if (dtExch.Rows.Count != 0)
            //{
            //    ddlSegment.DataSource = dtExch;
            //    ddlSegment.DataTextField = "EXCHANGENAME";
            //    ddlSegment.DataValueField = "SEGMENTID";
            //    ddlSegment.DataBind();
            //    ddlSegment.Items.Insert(0, new ListItem("None", "0"));
            //    ddlTntraExchange.DataSource = dtExch;
            //    ddlTntraExchange.DataTextField = "EXCHANGENAME";
            //    ddlTntraExchange.DataValueField = "SEGMENTID";
            //    ddlTntraExchange.DataBind();
            //    ddlTntraExchange.Items.Insert(0, new ListItem("None", "0"));
            //}
            DataTable dtExch = oDBEngine.GetDataTable("(SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='" + HttpContext.Current.Session["LastCompany"].ToString() + "' ) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID ", "A.EXCH_INTERNALID AS SEGMENTID ,isnull((TME.EXH_ShortName + '-' + A.EXCH_SEGMENTID),exch_membershipType) AS EXCHANGENAME ", null);
            if (dtExch.Rows.Count != 0)
            {
                ddlSegment.DataSource = dtExch;
                ddlSegment.DataTextField = "EXCHANGENAME";
                ddlSegment.DataValueField = "SEGMENTID";
                ddlSegment.DataBind();
                ddlSegment.Items.Insert(0, new ListItem("None", "0"));
                ddlTntraExchange.DataSource = dtExch;
                ddlTntraExchange.DataTextField = "EXCHANGENAME";
                ddlTntraExchange.DataValueField = "SEGMENTID";
                ddlTntraExchange.DataBind();
                ddlTntraExchange.Items.Insert(0, new ListItem("None", "0"));
            }
        }
        public void BindGridForFooter()
        {
            hddnEdit.Value = "Insert";
            if (ViewState["add"] == null)
            {
                DtTable.Dispose();
                DtTable = new DataTable();
                DtTable.Columns.Add(new DataColumn("CashReportID", typeof(int))); //0
                DtTable.Columns.Add(new DataColumn("JournalVoucherDetail_ID", typeof(String)));//1
                DtTable.Columns.Add(new DataColumn("JournalVoucherDetail_ExchangeSegmentID", typeof(String)));//2
                DtTable.Columns.Add(new DataColumn("JournalVoucherDetail_BranchID", typeof(String)));//3
                DtTable.Columns.Add(new DataColumn("JournalVoucherDetail_MainAccountCode", typeof(String)));//4
                DtTable.Columns.Add(new DataColumn("JournalVoucherDetail_SubAccountCode", typeof(String)));//5
                DtTable.Columns.Add(new DataColumn("JournalVoucherDetail_AmountDr", typeof(String)));//6
                DtTable.Columns.Add(new DataColumn("JournalVoucherDetail_AmountCr", typeof(String)));//7
                DtTable.Columns.Add(new DataColumn("JournalVoucherDetail_Narration", typeof(String)));//8
                DtTable.Columns.Add(new DataColumn("SubAccount1", typeof(string)));//9
                DtTable.Columns.Add(new DataColumn("MainAccount1", typeof(string)));//10
                DtTable.Columns.Add(new DataColumn("WithDrawl", typeof(string)));//11
                DtTable.Columns.Add(new DataColumn("Receipt", typeof(string)));//12
                DtTable.Columns.Add(new DataColumn("JournalVoucherDetail_ValueDate", typeof(string)));//13
                DtTable.Columns.Add(new DataColumn("JournalVoucherDetail_ValueDate1", typeof(string)));//14
                DtTable.Columns.Add(new DataColumn("Type", typeof(string)));//15
                DataRow drReport = DtTable.NewRow();
                drReport[0] = 0;
                drReport[1] = "";
                drReport[2] = "";
                drReport[3] = "";
                drReport[4] = "";
                drReport[5] = "";
                drReport[6] = "";
                drReport[7] = "";
                drReport[8] = "";
                drReport[9] = "";
                drReport[10] = "";
                drReport[11] = "";
                drReport[12] = "";
                drReport[13] = "";
                drReport[14] = "";
                drReport[15] = "";
                DtTable.Rows.Add(drReport);
                grdAdd.DataSource = DtTable.DefaultView;
                grdAdd.DataBind();
                grdAdd.Rows[0].Visible = false;

            }
            else
            {
                grdAdd.DataSource = dtReport.DefaultView;
                grdAdd.DataBind();
                grdAdd.Rows[0].Visible = true;
            }
        }
        public void BindGridForEdit()
        {
            Session["KeyVal"] = Request.QueryString["id"].ToString();
            dtReport = oDBEngine.GetDataTable("Trans_JournalVoucherDetail", "cast(journalvoucherdetail_ID as int) as CashReportID,journalvoucherdetail_VoucherNumber as journalvoucherdetail_ID,ltrim(rtrim(JournalVoucherDetail_ExchangeSegmentID)) as JournalVoucherDetail_ExchangeSegmentID,ltrim(rtrim(JournalVoucherDetail_BranchID)) as JournalVoucherDetail_BranchID,(select MainAccount_ReferenceID from master_mainaccount where MainAccount_AccountCode=Trans_JournalVoucherDetail.JournalVoucherDetail_MainAccountCode) as JournalVoucherDetail_MainAccountCode,JournalVoucherDetail_SubAccountCode,cast(ltrim(rtrim(JournalVoucherDetail_AmountDr)) as decimal(20,5)) as JournalVoucherDetail_AmountDr,cast(ltrim(rtrim(JournalVoucherDetail_AmountCr)) as decimal(20,5)) as JournalVoucherDetail_AmountCr,ltrim(rtrim(JournalVoucherDetail_Narration)) as JournalVoucherDetail_Narration,(isnull((select isnull(ltrim(rtrim(cnt_firstname)),'')+' '+isnull(ltrim(rtrim(cnt_middlename)),'')+' '+isnull(ltrim(rtrim(cnt_lastname)),'')+' ['+isnull(ltrim(rtrim(cnt_ucc)),'')+']' from tbl_master_contact where cnt_internalid=Trans_JournalVoucherDetail.JournalVoucherDetail_SubAccountCode),isnull((select top 1 subaccount_name+' ['+isnull(ltrim(rtrim(subaccount_code)),'')+']' from master_subaccount where cast(subaccount_code as varchar)=Trans_JournalVoucherDetail.JournalVoucherDetail_SubAccountCode and SubAccount_MainAcReferenceID=Trans_JournalVoucherDetail.JournalVoucherDetail_MainAccountCode),isnull((select subaccount_name+' ['+isnull(ltrim(rtrim(subaccount_code)),'')+']' from master_subaccount where cast(subaccount_referenceid as varchar)=Trans_JournalVoucherDetail.JournalVoucherDetail_SubAccountCode),isnull((select top 1 cdslclients_firstholdername+'['+isnull(ltrim(rtrim(cdslclients_benaccountnumber)),'')+']' from master_cdslclients where cast(cdslclients_benaccountnumber as varchar)=Trans_JournalVoucherDetail.JournalVoucherDetail_SubAccountCode),(select nsdlclients_benfirstholdername+' ['+isnull(ltrim(rtrim(nsdlclients_benaccountid)),'')+']' from master_nsdlclients where cast(nsdlclients_benaccountid as varchar)=Trans_JournalVoucherDetail.JournalVoucherDetail_SubAccountCode)))))) as SubAccount1,isnull((select MainAccount_Name from Master_MainAccount where cast(MainAccount_ReferenceId as varchar)=cast(Trans_JournalVoucherDetail.JournalVoucherDetail_MainAccountCode as varchar)),isnull((select MainAccount_Name from master_mainaccount where mainaccount_accountcode=Trans_JournalVoucherDetail.JournalVoucherDetail_MainAccountCode),'')) as MainAccount1,case JournalVoucherDetail_AmountDr when '0.0000' then null else convert(varchar(50),cast(JournalVoucherDetail_AmountDr as money),1) end as WithDrawl,case JournalVoucherDetail_AmountCr when '0.0000' then null else convert(varchar(50),cast(JournalVoucherDetail_AmountCr as money),1) end as Receipt,cast(ltrim(rtrim(JournalVoucherDetail_ValueDate)) as datetime) as JournalVoucherDetail_ValueDate,convert(varchar(12),JournalVoucherDetail_ValueDate,113) as JournalVoucherDetail_ValueDate1,isnull((select MainAccount_SubLedgerType from Master_MainAccount where cast(MainAccount_AccountCode as varchar)=cast(Trans_JournalVoucherDetail.JournalVoucherDetail_MainAccountCode as varchar)),'Systm') as Type," + Session["userid"].ToString() + " as UserID", " JournalVoucherDetail_VoucherNumber='" + Request.QueryString["id"].ToString() + "' and JournalVoucherDetail_ExchangeSegmentID=" + Request.QueryString["exch"].ToString().Trim() + " and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,JournalVoucherDetail_TransactionDate)) as datetime)=cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + Request.QueryString["date"].ToString().Trim() + "')) as datetime)", " JournalVoucherDetail_SubAccountCode desc");
            if (dtReport.Rows.Count > 0)
            {
                ReptID = Convert.ToInt32(dtReport.Compute("max(CashReportID)", ""));
                string gridForVoucherID = dtReport.Rows[0][1].ToString().Substring(0, 2);
                if (gridForVoucherID == "YF")
                {
                    grdAdd.Enabled = false;
                    btnSave.Enabled = false;
                }
            }
            DataTable dtEdit = oDBEngine.GetDataTable("trans_journalvoucher", "ltrim(rtrim(journalvoucher_companyid)) as journalvoucher_companyid,ltrim(rtrim(journalvoucher_ExchangeSegmentID)) as journalvoucher_ExchangeSegmentID,ltrim(rtrim(journalvoucher_BranchID)) as journalvoucher_BranchID,ltrim(rtrim(journalvoucher_SettlementNumber)) as journalvoucher_SettlementNumber,ltrim(rtrim(journalvoucher_BillNumber)) as journalvoucher_BillNumber,cast(journalvoucher_TransactionDate as datetime) as journalvoucher_TransactionDate,journalvoucher_Prefix,ltrim(rtrim(journalvoucher_Narration)) as journalvoucher_Narration,ltrim(rtrim(journalvoucher_VoucherNumber)) as journalvoucher_VoucherNumber,(select isnull(rtrim(settlements_Number),'')+ ' [ ' + isnull(rtrim(settlements_typeSuffix),'') + ' ]' as SettlementName from Master_Settlements where settlements_ID=trans_journalvoucher.journalvoucher_SettlementNumber) as SettNumber ", " journalvoucher_vouchernumber='" + dtReport.Rows[0][1].ToString() + "' and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,JournalVoucher_TransactionDate)) as datetime)=cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + Request.QueryString["date"].ToString().Trim() + "')) as datetime) and journalvoucher_exchangesegmentid=" + dtReport.Rows[0][2].ToString() + "");
            if (dtEdit.Rows.Count > 0)
            {
                ddlCompany.SelectedValue = dtEdit.Rows[0][0].ToString();
                ddlSegment.Items.Clear();
                //string[,] Segment = oDBEngine.GetFieldValue("(SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='" + dtEdit.Rows[0][0].ToString() + "') AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID", "LTRIM(RTRIM(A.EXCH_INTERNALID)) AS CashBank_ExchangeSegmentID ,isnull(TME.EXH_ShortName + '--' + A.EXCH_SEGMENTID,exch_membershiptype) AS EXCHANGENAME ", null, 2);
                //if (Segment[0, 0] != "n")
                //{
                //    oDBEngine.AddDataToDropDownList(Segment, ddlSegment, true);
                //}
                DataTable dtExch = oDBEngine.GetDataTable("(SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='" + dtEdit.Rows[0][0].ToString() + "' ) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID ", "A.EXCH_INTERNALID AS SEGMENTID ,isnull((TME.EXH_ShortName + '-' + A.EXCH_SEGMENTID),exch_membershipType) AS EXCHANGENAME ", null);
                if (dtExch.Rows.Count != 0)
                {
                    ddlSegment.DataSource = dtExch;
                    ddlSegment.DataTextField = "EXCHANGENAME";
                    ddlSegment.DataValueField = "SEGMENTID";
                    ddlSegment.DataBind();
                    ddlSegment.Items.Insert(0, new ListItem("None", "0"));
                }
                ddlSegment.SelectedValue = dtEdit.Rows[0][1].ToString();
                ddlBranch.DataBind();
                ddlBranch.SelectedValue = dtEdit.Rows[0][2].ToString();
                txtSettno_hidden.Value = dtEdit.Rows[0][3].ToString();
                txtSettno.Text = dtEdit.Rows[0][9].ToString();
                tDate.Value = Convert.ToDateTime(dtEdit.Rows[0][5].ToString());
                txtAccountCode.Text = dtEdit.Rows[0][6].ToString();
                txtNarration.Text = dtEdit.Rows[0][7].ToString();
                txtBillNo.Text = dtEdit.Rows[0][4].ToString();
                txtVoucherNo.Text = dtEdit.Rows[0][8].ToString();
                ddlBranch.Enabled = false;
                ddlSegment.Enabled = false;
                ddlCompany.Enabled = false;
                txtAccountCode.Enabled = false;
                ddlIntExchange.Enabled = false;
            }
            ViewState["mytable"] = dtReport;
            hddnEdit.Value = "Edit";
            grdAdd.DataSource = dtReport;
            grdAdd.DataBind();
            //if (Request.QueryString["id"] != null)
            //{
            //    grdAdd.FooterRow.Visible = false;
            //}
            //else
            //{
            //    grdAdd.FooterRow.Visible = true;
            //}
            // ViewState["EditVal"] = "a";
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
            ScriptManager.RegisterStartupScript(this, GetType(), "JScript", "Narration1()", true);
            //if (Request.QueryString["id"] != null)
            //{
            //    grdAdd.FooterRow.Visible = false;
            //}
            //else
            //{
            //    grdAdd.FooterRow.Visible = true;
            //}
        }
        protected void grdAdd_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Insert")
            {
                string Segment = "";
                TextBox txtMainAccount = (TextBox)grdAdd.FooterRow.FindControl("txtMainAccount");
                HiddenField txtMainAccount_hidden = (HiddenField)grdAdd.FooterRow.FindControl("txtMainAccount_hidden");
                TextBox txtSubAccount = (TextBox)grdAdd.FooterRow.FindControl("txtSubAccount");
                HiddenField txtSubAccount_hidden = (HiddenField)grdAdd.FooterRow.FindControl("txtSubAccount_hidden");
                ASPxTextBox txtWithdraw = (ASPxTextBox)grdAdd.FooterRow.FindControl("txtWithdraw");
                ASPxTextBox txtReceipt = (ASPxTextBox)grdAdd.FooterRow.FindControl("txtReceipt");
                ASPxDateEdit dtAspxDate = (ASPxDateEdit)grdAdd.FooterRow.FindControl("dtAspxDate");
                if (ViewState["add"] == null)
                {
                    dtReport.Dispose();
                    dtReport = new DataTable();
                    dtReport.Columns.Add(new DataColumn("CashReportID", typeof(int))); //0
                    dtReport.Columns.Add(new DataColumn("JournalVoucherDetail_ID", typeof(String)));//1
                    dtReport.Columns.Add(new DataColumn("JournalVoucherDetail_ExchangeSegmentID", typeof(String)));//2
                    dtReport.Columns.Add(new DataColumn("JournalVoucherDetail_BranchID", typeof(String)));//3
                    dtReport.Columns.Add(new DataColumn("JournalVoucherDetail_MainAccountCode", typeof(String)));//4
                    dtReport.Columns.Add(new DataColumn("JournalVoucherDetail_SubAccountCode", typeof(String)));//5
                    dtReport.Columns.Add(new DataColumn("JournalVoucherDetail_AmountDr", typeof(Decimal)));//6
                    dtReport.Columns.Add(new DataColumn("JournalVoucherDetail_AmountCr", typeof(Decimal)));//7
                    dtReport.Columns.Add(new DataColumn("JournalVoucherDetail_Narration", typeof(String)));//8
                    dtReport.Columns.Add(new DataColumn("SubAccount1", typeof(string)));//9
                    dtReport.Columns.Add(new DataColumn("MainAccount1", typeof(string)));//10
                    dtReport.Columns.Add(new DataColumn("WithDrawl", typeof(string)));//11
                    dtReport.Columns.Add(new DataColumn("Receipt", typeof(string)));//12
                    dtReport.Columns.Add(new DataColumn("JournalVoucherDetail_ValueDate", typeof(DateTime)));//13
                    dtReport.Columns.Add(new DataColumn("JournalVoucherDetail_ValueDate1", typeof(string)));//14
                    dtReport.Columns.Add(new DataColumn("Type", typeof(string)));//15
                    dtReport.Columns.Add(new DataColumn("UserID", typeof(string)));//16
                    ViewState["add"] = "aa";
                }
                //DataRow[] reportRow = dtReport.Select("JournalVoucherDetail_SubAccountCode='" + txtSubAccount_hidden.Value.ToString() + "'");
                //if (reportRow.Length > 0)
                //{
                //    Segment = ddlTntraExchange.SelectedItem.Value.ToString();
                //    ViewState["Segment"] = "Segment";
                //}
                //else
                //{
                //    Segment = ddlSegment.SelectedItem.Value.ToString();
                //}
                if (ViewState["mytable"] != null)
                    dtReport = (DataTable)ViewState["mytable"];
                string MainAccID = null;
                string[] mainAccountID = txtMainAccount_hidden.Value.ToString().Split('~');
                if (mainAccountID.Length > 1)
                    MainAccID = mainAccountID[0];
                else
                    MainAccID = txtMainAccount_hidden.Value.ToString();
                if (ddlIntExchange.SelectedItem.Value.ToString() == "1")
                {
                    Segment = ddlTntraExchange.SelectedItem.Value.ToString();
                    ViewState["Segment"] = "Segment";
                }
                else
                {
                    Segment = ddlSegment.SelectedItem.Value.ToString();
                }
                //if (ViewState["EditVal"] != null)
                //{
                //    DataTable dtEditVal = oDBEngine.GetDataTable("Trans_JournalVoucherDetail", "max(JournalVoucherDetail_ID)", null);
                //    ReptID = Convert.ToInt32(dtEditVal.Rows[0][0].ToString()) + 1;
                //    ViewState["EditVal"] = null;
                //}
                //else
                //{
                //    ReptID = ReptID + 1;
                //}
                ReptID = ReptID + 1;
                DataRow drReport = dtReport.NewRow();
                drReport[0] = ReptID.ToString();
                drReport[1] = Session["KeyVal"].ToString();
                drReport[2] = Segment;
                drReport[3] = ddlBranch.SelectedItem.Value.ToString();
                drReport[4] = MainAccID;
                drReport[5] = txtSubAccount_hidden.Value.ToString();
                drReport[6] = txtWithdraw.Text.ToString();
                drReport[7] = txtReceipt.Text.ToString();
                drReport[11] = objConverter.getFormattedvalue(Convert.ToDecimal(txtWithdraw.Text));
                drReport[12] = objConverter.getFormattedvalue(Convert.ToDecimal(txtReceipt.Text));
                drReport[8] = txtNarration1.Text;
                drReport[10] = txtMainAccount.Text.ToString();
                drReport[9] = txtSubAccount.Text.ToString();
                drReport[13] = tDate.Value;
                drReport[14] = objConverter.ArrangeDate(Convert.ToDateTime(tDate.Value.ToString()).ToShortDateString());
                string[] MainAC = txtMainAccount_hidden.Value.ToString().Split('~');
                DataTable dtType = new DataTable();
                if (MainAC.Length > 1)
                    dtType = oDBEngine.GetDataTable("master_mainaccount", "ltrim(rtrim(mainaccount_SubLedgerType))", " mainaccount_ReferenceID=" + MainAC[0].ToString() + "");
                else
                    dtType = oDBEngine.GetDataTable("master_mainaccount", "ltrim(rtrim(mainaccount_SubLedgerType))", " mainaccount_ReferenceID=" + txtMainAccount_hidden.Value.ToString() + "");
                drReport[15] = dtType.Rows[0][0].ToString();
                drReport[16] = Session["userid"].ToString();
                dtReport.Rows.Add(drReport);
                dtReport.AcceptChanges();

                ViewState["mytable"] = dtReport;

                DataView dvData = new DataView(dtReport);
                dvData.RowFilter = "UserID = '" + Session["userid"].ToString() + "'";

                ViewState["Footer"] = "Footer";
                grdAdd.DataSource = dvData;
                grdAdd.DataBind();
                string withdraw = dtReport.Compute("Sum(JournalVoucherDetail_AmountCr)", string.Empty).ToString();
                string receipt = dtReport.Compute("Sum(JournalVoucherDetail_AmountDr)", string.Empty).ToString();
                if (withdraw == receipt)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "JScript", "Button_Click()", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "JScript", "Page_Load()", true);
                }
                ScriptManager.RegisterStartupScript(this, GetType(), "JScript", "Narration1()", true);
            }
        }
        protected void grdAdd_RowCreated(object sender, GridViewRowEventArgs e)
        {
            GridViewRow row = e.Row;
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                ASPxDateEdit date = (ASPxDateEdit)row.FindControl("dtAspxDate");
                //date.Value = Convert.ToDateTime(oDBEngine.GetDate());
                date.Value = oDBEngine.GetDate();
                //TextBox txtSubAccount = (TextBox)row.FindControl("txtSubAccount");
                //Button Button1 = (Button)row.FindControl("Button1");
                //Button1.Attributes.Add("onclick", "javascript:return SubAccountCheck('" + txtSubAccount.ClientID + "');");
            }
        }
        protected void grdAdd_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow row = e.Row;
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                if (ViewState["Footer"] != null)
                {
                    Tdebit.Visible = true;
                    decimal diff = 0;

                    if (ViewState["mytable"] != null)
                        dtReport = (DataTable)ViewState["mytable"];

                    DataView dvData = new DataView(dtReport);
                    dvData.RowFilter = " UserID = '" + Session["userid"].ToString() + "'";

                    DataTable DtTab = new DataTable();
                    DtTab = dvData.ToTable();

                    int pos = DtTab.Rows.Count - 1;
                    TextBox MainAccount = (TextBox)row.FindControl("txtMainAccount");
                    HiddenField MainAccount_hidden = (HiddenField)row.FindControl("txtMainAccount_hidden");
                    ASPxTextBox withdraw = (ASPxTextBox)row.FindControl("txtWithdraw");
                    ASPxTextBox receipt = (ASPxTextBox)row.FindControl("txtReceipt");
                    TextBox SubAccount = (TextBox)row.FindControl("txtSubAccount");
                    ASPxDateEdit dtAspxDate = (ASPxDateEdit)row.FindControl("dtAspxDate");
                    decimal withdraw1 = Convert.ToDecimal(DtTab.Compute("Sum(JournalVoucherDetail_AmountDr)", string.Empty).ToString());
                    decimal receipt1 = Convert.ToDecimal(DtTab.Compute("Sum(JournalVoucherDetail_AmountCr)", string.Empty).ToString());
                    if (DtTab.Rows.Count > 0)
                    {
                        MainAccount.Text = DtTab.Rows[pos][10].ToString();
                        MainAccount_hidden.Value = DtTab.Rows[pos][4].ToString();
                        //dtAspxDate.Value = Convert.ToDateTime(dtReport.Rows[pos][13].ToString());
                    }
                    if (DtTab.Rows[pos][6].ToString() != "")
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
                    if (DtTab.Rows[pos][7].ToString() != "")
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
                    string var1 = SubAccount.ClientID;
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "JScript", "keyVal1(" + MainAccount_hidden.Value + ")", true);
                    ScriptManager.RegisterStartupScript(this, GetType(), "Jscript", "SetSubAcc1('" + var1 + "')", true);
                    txtTDebit.Text = withdraw1.ToString();
                    txtTCredit.Text = receipt1.ToString();
                }
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
                foreach (DataRow dr in DtTab.Rows)
                {
                    if (dr.ItemArray[0].ToString() == KeyName.ToString())
                    {
                        dr.Delete();
                        if (ViewState["Delete"] != null)
                        {
                            ViewState["Delete"] = ViewState["Delete"] + "," + KeyName + ",";
                        }
                        else
                        {
                            ViewState["Delete"] = KeyName + ",";
                        }
                    }
                }
            }
            catch
            {
            }
            int ii = ViewState["Delete"].ToString().LastIndexOf(",");
            ViewState["Delete"] = ViewState["Delete"].ToString().Substring(0, ii);
            DtTab.AcceptChanges();
            ViewState["add"] = "a";
            ViewState["mytable"] = DtTab;
            grdAdd.DataSource = DtTab;
            grdAdd.DataBind();
            if (DtTab.Rows.Count == 0)
            {
                ViewState["add"] = null;
                ViewState["Footer"] = null;
                BindGridForFooter();
            }
            if (DtTab.Rows.Count != 0)
            {
                string withdraw = DtTab.Compute("Sum(JournalVoucherDetail_AmountCr)", string.Empty).ToString();
                string receipt = DtTab.Compute("Sum(JournalVoucherDetail_AmountDr)", string.Empty).ToString();
                if (withdraw == receipt)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "JScript", "Button_Click()", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "JScript", "Page_Load()", true);
                }
            }
            //if (Request.QueryString["id"] != null)
            //{
            //    grdAdd.FooterRow.Visible = false;
            //}
            //else
            //{
            //    grdAdd.FooterRow.Visible = true;
            //}
        }
        protected void grdAdd_RowEditing(object sender, GridViewEditEventArgs e)
        {
            if (ViewState["mytable"] != null)
                dtReport = (DataTable)ViewState["mytable"];

            DataView dvData = new DataView(dtReport);
            dvData.RowFilter = "UserID = '" + Session["userid"].ToString() + "'";

            DataTable DtTab = new DataTable();
            DtTab = dvData.ToTable();

            ID = (int)grdAdd.DataKeys[e.NewEditIndex].Value;
            DataRow[] reportRow = DtTab.Select("CashReportID=" + ID + "");
            ViewState["SubID"] = reportRow[0][5].ToString();
            string MainID = reportRow[0][4].ToString() + "~" + reportRow[0][15].ToString();
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "JScript", "keyVal('" + MainID + "')", true);
            grdAdd.EditIndex = e.NewEditIndex;
            ViewState["mytable"] = DtTab;
            grdAdd.DataSource = DtTab;
            grdAdd.DataBind();
            ScriptManager.RegisterStartupScript(this, GetType(), "JScript", "Narration('" + reportRow[0][8].ToString() + "')", true);
            string withdraw = DtTab.Compute("Sum(JournalVoucherDetail_AmountCr)", string.Empty).ToString();
            string receipt = DtTab.Compute("Sum(JournalVoucherDetail_AmountDr)", string.Empty).ToString();
            if (withdraw == receipt)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "JScript", "Button_Click()", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "JScript", "Page_Load()", true);
            }
            if (Request.QueryString["id"] != null)
            {
                TextBox txtMAccount = grdAdd.Rows[e.NewEditIndex].FindControl("txtEditMainAccount") as TextBox;
                TextBox txtSubAccount = grdAdd.Rows[e.NewEditIndex].FindControl("txtEditSubAccount") as TextBox;
                grdAdd.FooterRow.Visible = false;
                // txtMAccount.Enabled = false;
                //txtSubAccount.Enabled = false;
            }
            else
            {
                grdAdd.FooterRow.Visible = true;
            }
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "JScr", "Narration_Val('" + reportRow[0][8].ToString() + "')", true);
        }
        protected void grdAdd_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            TextBox txtMainAccount = (TextBox)grdAdd.Rows[e.RowIndex].FindControl("txtEditMainAccount");
            HiddenField txtMainAccount_hidden = (HiddenField)grdAdd.Rows[e.RowIndex].FindControl("txtEditMainAccount_hidden");
            TextBox txtSubAccount = (TextBox)grdAdd.Rows[e.RowIndex].FindControl("txtEditSubAccount");
            HiddenField txtSubAccount_hidden = (HiddenField)grdAdd.Rows[e.RowIndex].FindControl("txtEditSubAccount_hidden");
            ASPxTextBox txtWithdraw = (ASPxTextBox)grdAdd.Rows[e.RowIndex].FindControl("txtEditWithdraw");
            ASPxTextBox txtReceipt = (ASPxTextBox)grdAdd.Rows[e.RowIndex].FindControl("txtEditRecpt");
            ASPxDateEdit dtAspxDate = (ASPxDateEdit)grdAdd.Rows[e.RowIndex].FindControl("dtEditAspxDate");

            if (ViewState["mytable"] != null)
                dtReport = (DataTable)ViewState["mytable"];

            DataView dvData = new DataView(dtReport);
            dvData.RowFilter = "UserID = '" + Session["userid"].ToString() + "'";

            DataTable DtTab = new DataTable();
            DtTab = dvData.ToTable();

            string MainAccID = null;
            string[] mainAccountID = txtMainAccount_hidden.Value.ToString().Split('~');
            if (mainAccountID.Length > 1)
                MainAccID = mainAccountID[0];
            else
                MainAccID = txtMainAccount_hidden.Value.ToString();

            DataRow[] reportRow = DtTab.Select("CashReportID=" + ID + "");
            reportRow[0][1] = Session["KeyVal"].ToString();
            reportRow[0][2] = ddlSegment.SelectedItem.Value.ToString();
            reportRow[0][3] = ddlBranch.SelectedItem.Value.ToString();
            reportRow[0][4] = MainAccID;
            reportRow[0][5] = txtSubAccount_hidden.Value.ToString();
            reportRow[0][6] = txtWithdraw.Text.ToString();
            reportRow[0][7] = txtReceipt.Text.ToString();
            reportRow[0][11] = objConverter.getFormattedvalue(Convert.ToDecimal(txtWithdraw.Text));
            reportRow[0][12] = objConverter.getFormattedvalue(Convert.ToDecimal(txtReceipt.Text));
            reportRow[0][8] = txtNarration1.Text;
            reportRow[0][10] = txtMainAccount.Text.ToString();
            reportRow[0][9] = txtSubAccount.Text.ToString();
            reportRow[0][13] = tDate.Value;
            ViewState["Footer"] = "Footer";
            reportRow[0][14] = objConverter.ArrangeDate(Convert.ToDateTime(tDate.Value).ToShortDateString());
            DtTab.AcceptChanges();
            ViewState["add"] = "a";
            grdAdd.EditIndex = -1;

            ViewState["mytable"] = DtTab;

            grdAdd.DataSource = DtTab;
            grdAdd.DataBind();
            string withdraw = dtReport.Compute("Sum(JournalVoucherDetail_AmountCr)", string.Empty).ToString();
            string receipt = dtReport.Compute("Sum(JournalVoucherDetail_AmountDr)", string.Empty).ToString();
            if (withdraw == receipt)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "JScript", "Button_Click()", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "JScript", "Page_Load()", true);
            }
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "JScript", "Narration1()", true);
            //if (Request.QueryString["id"] != null)
            //{
            //    grdAdd.FooterRow.Visible = false;
            //}
            //else
            //{
            //    grdAdd.FooterRow.Visible = true;
            //}
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            DataSet DSJournalReturn = new DataSet();
            //SqlConnection lcon = new SqlConnection(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
            SqlConnection lcon = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string settlementtype = "";
            string settlementNo = "";//2009116 [ A ]
            if (txtSettno.Text.ToString() != "")
                settlementNo = txtSettno.Text.ToString().Substring(0, 8);
            string tempsettlementtype = txtSettno.Text.ToString();

            if (ViewState["mytable"] != null)
                dtReport = (DataTable)ViewState["mytable"];
            DataView dvData = new DataView(dtReport);
            dvData.RowFilter = "UserID = '" + Session["userid"].ToString() + "'";

            DataTable DtTab = new DataTable();
            DtTab = dvData.ToTable();

            string tabledata = objConverter.ConvertDataTableToXML(DtTab);
            if (Request.QueryString["id"] == null)
            {
                if (ViewState["Segment"] != null)
                {
                    if (tempsettlementtype != "")
                    {
                        settlementtype = tempsettlementtype.Substring(Convert.ToInt32(tempsettlementtype.Length - 3), 1);
                    }
                    DtTab.Rows[0]["JournalVoucherDetail_ExchangeSegmentID"] = ddlSegment.SelectedItem.Value.ToString();
                    DtTab.AcceptChanges();
                    string tabledata1 = objConverter.ConvertDataTableToXML(DtTab);
                    //String conn = ConfigurationManager.AppSettings["DBConnectionDefault"];
                    //SqlConnection con = new SqlConnection(conn);
                    //con.Open();
                    //SqlCommand com = new SqlCommand("xmlJournalVoucherInterSegmentInsert", con);
                    //com.CommandType = CommandType.StoredProcedure;
                    //com.Parameters.AddWithValue("@journalData", tabledata1);
                    //com.Parameters.AddWithValue("@createuser", Session["userid"].ToString());
                    //com.Parameters.AddWithValue("@finyear", Session["LastFinYear"].ToString());
                    //com.Parameters.AddWithValue("@compID", ddlCompany.SelectedItem.Value.ToString());
                    //com.Parameters.AddWithValue("@JournalVoucher_Narration", txtNarration.Text);
                    //com.Parameters.AddWithValue("@JournalVoucherDetail_TransactionDate", Convert.ToDateTime(tDate.Value));
                    //com.Parameters.AddWithValue("@JournalVoucher_SettlementNumber", settlementNo);
                    //com.Parameters.AddWithValue("@JournalVoucher_SettlementType", settlementtype);
                    //com.Parameters.AddWithValue("@JournalVoucher_BillNumber", txtBillNo.Text);
                    //com.Parameters.AddWithValue("@JournalVoucher_Prefix", "YF");
                    //com.Parameters.AddWithValue("@segmentid", ddlSegment.SelectedItem.Value);

                    //NoofRowsAffected = com.ExecuteNonQuery();
                    NoofRowsAffected = oManagement_BL.xmlJournalVoucherInterSegmentInsert(tabledata1, Convert.ToString(Session["userid"]), Convert.ToString(Session["LastFinYear"]),
                        Convert.ToString(ddlCompany.SelectedItem.Value.ToString()), Convert.ToString(txtNarration.Text), Convert.ToDateTime(tDate.Value).ToString("yyyy-MM-dd"),
                        settlementNo, settlementtype, txtBillNo.Text, "YF", Convert.ToString(ddlSegment.SelectedItem.Value));
                    //con.Close();
                }
                else
                {
                    try
                    {
                        if (tempsettlementtype != "")
                        {
                            settlementtype = tempsettlementtype.Substring(Convert.ToInt32(tempsettlementtype.Length - 3), 1);
                        }
                        //String conn = ConfigurationManager.AppSettings["DBConnectionDefault"];
                        //SqlConnection con = new SqlConnection(conn);
                        //SqlCommand com = new SqlCommand("xmlJournalVoucherInsert", con);
                        //com.CommandType = CommandType.StoredProcedure;
                        //com.Parameters.AddWithValue("@journalData", tabledata);
                        //com.Parameters.AddWithValue("@createuser", Session["userid"].ToString());
                        //com.Parameters.AddWithValue("@finyear", Session["LastFinYear"].ToString());
                        //com.Parameters.AddWithValue("@compID", ddlCompany.SelectedItem.Value.ToString());
                        //com.Parameters.AddWithValue("@JournalVoucher_Narration", txtNarration.Text);
                        //com.Parameters.AddWithValue("@JournalVoucherDetail_TransactionDate", Convert.ToDateTime(tDate.Value));
                        //com.Parameters.AddWithValue("@JournalVoucher_SettlementNumber", settlementNo);
                        //com.Parameters.AddWithValue("@JournalVoucher_SettlementType", settlementtype);
                        //com.Parameters.AddWithValue("@JournalVoucher_BillNumber", txtBillNo.Text);
                        //com.Parameters.AddWithValue("@JournalVoucher_Prefix", txtAccountCode.Text);
                        //com.Parameters.AddWithValue("@segmentid", ddlSegment.SelectedItem.Value);
                        //com.CommandTimeout = 0;
                        //SqlDataAdapter AdapN = new SqlDataAdapter();
                        //AdapN.SelectCommand = com;
                        //AdapN.Fill(DSJournalReturn);
                        DSJournalReturn = oManagement_BL.xmlJournalVoucherInsert(tabledata, Convert.ToString(Session["userid"]), Convert.ToString(Session["LastFinYear"]),
                            ddlCompany.SelectedItem.Value.ToString(), Convert.ToString(txtNarration.Text), Convert.ToDateTime(tDate.Value).ToString("yyyy-MM-dd"), settlementNo,
                            settlementtype, Convert.ToString(txtBillNo.Text), Convert.ToString(txtAccountCode.Text), Convert.ToString(ddlSegment.SelectedItem.Value));
                        ViewState["DSJournalReturn"] = DSJournalReturn;
                        if (DSJournalReturn.Tables[0].Rows[0][0].ToString() == "Error")
                            Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>alertMessage();</script>");
                        else
                            Page.ClientScript.RegisterStartupScript(GetType(), "JScript589", "<script language='javascript'>AlertAfterInsert()</script>");
                    }
                    catch
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>alertMessage();</script>");
                    }
                }
            }
            else
            {

                if (ViewState["Delete"] != null)
                {
                    //oDBEngine.DeleteValue("trans_journalvoucherdetail", " journalvoucherdetail_ID in(" + ViewState["Delete"].ToString() + ")");
                    string[] delte = ViewState["Delete"].ToString().Split(',');
                    for (int j = 0; j < delte.Length; j++)
                    {
                        //SqlCommand lcmd = new SqlCommand("deleteJournalVoucher", lcon);
                        //lcmd.CommandType = CommandType.StoredProcedure;
                        //lcmd.Parameters.Add("@companyId", SqlDbType.VarChar, 12).Value = ddlCompany.SelectedItem.Value;
                        //lcmd.Parameters.Add("@VoucherID", SqlDbType.Int).Value = Convert.ToInt32(delte[j].ToString());
                        //lcon.Open();
                        //NoofRowsAffected = lcmd.ExecuteNonQuery();
                        //lcmd.Dispose();
                        //lcon.Close();
                        NoofRowsAffected = oManagement_BL.deleteJournalVoucher(Convert.ToString(ddlCompany.SelectedItem.Value), Convert.ToInt32(delte[j].ToString()));
                    }
                    ViewState["Delete"] = null;
                }

                if (tempsettlementtype != "")
                {
                    settlementtype = tempsettlementtype.Substring(Convert.ToInt32(tempsettlementtype.Length - 3), 1);
                }
                //String conn = ConfigurationManager.AppSettings["DBConnectionDefault"];
                //SqlConnection con = new SqlConnection(conn);
                //con.Open();
                //SqlCommand com = new SqlCommand("xmlJournalVoucherUpdate", con);
                //com.CommandType = CommandType.StoredProcedure;
                //com.Parameters.AddWithValue("@journalData", tabledata);
                //com.Parameters.AddWithValue("@createuser", Session["userid"].ToString());
                //com.Parameters.AddWithValue("@finyear", Session["LastFinYear"].ToString());
                //com.Parameters.AddWithValue("@compID", ddlCompany.SelectedItem.Value.ToString());
                //com.Parameters.AddWithValue("@JournalVoucher_Narration", txtNarration.Text);
                //com.Parameters.AddWithValue("@JournalVoucherDetail_TransactionDate", Convert.ToDateTime(tDate.Value));
                //com.Parameters.AddWithValue("@JournalVoucher_SettlementNumber", settlementNo);
                //com.Parameters.AddWithValue("@JournalVoucher_SettlementType", settlementtype);
                //com.Parameters.AddWithValue("@JournalVoucher_BillNumber", txtBillNo.Text);
                //com.Parameters.AddWithValue("@JournalVoucher_Prefix", txtAccountCode.Text);
                //com.Parameters.AddWithValue("@segmentid", ddlSegment.SelectedItem.Value);
                //NoofRowsAffected = com.ExecuteNonQuery();
                //com.CommandTimeout = 0;
                //con.Close();
                NoofRowsAffected = oManagement_BL.xmlJournalVoucherUpdate(tabledata, Convert.ToString(Session["userid"]), Convert.ToString(Session["LastFinYear"]),
                        Convert.ToString(ddlCompany.SelectedItem.Value.ToString()), Convert.ToString(txtNarration.Text), Convert.ToDateTime(tDate.Value).ToString("yyyy-MM-dd"),
                        settlementNo, settlementtype, txtBillNo.Text, txtAccountCode.Text, Convert.ToString(ddlSegment.SelectedItem.Value));

            }
            if (NoofRowsAffected > 0)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "Script", "<script language='javascript'>PopulateData()</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>parent.editwin.close();</script>");
            }
            ReptID = 0;
            ID = 0;
            Session["cashJournal"] = "1";
        }

        [System.Web.Services.WebMethod]
        public static string GetContactName(string custid)
        {
            string closingBalance = null;
            BusinessLogicLayer.DBEngine oDbEngine1 = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            BusinessLogicLayer.Converter objConverter1 = new BusinessLogicLayer.Converter();
            string[] dateandparam = custid.Split('~');
            // DateTime date = Convert.ToDateTime(objConverter1.DateConverter1(dateandparam[2].ToString(), "mm/dd/yyyy"));

            DateTime date = Convert.ToDateTime(ForJournalDate);

            string[,] mainacc = oDbEngine1.GetFieldValue("master_mainaccount", "MainAccount_Accountcode", "mainaccount_referenceid=" + dateandparam[3] + "", 1);
            string mainaccCode = mainacc[0, 0];
            mainaccCode = "'" + mainaccCode + "'";
            string SubAccID = dateandparam[4];
            DataTable dtClose = oDbEngine1.OpeningBalanceOnlyJournal(mainaccCode, SubAccID, date, dateandparam[1], dateandparam[0], date);
            // DataTable dtClose = oDbEngine1.OpeningBalanceOnlyJournal(mainaccCode, SubAccID, date, dateandparam[1], dateandparam[0], date);

            closingBalance = dtClose.Rows[0][0].ToString();
            return closingBalance;
        }
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            DataSet dsCrystal = new DataSet();
            DataSet DSJournalReturn = new DataSet();
            DSJournalReturn = (DataSet)ViewState["DSJournalReturn"];
            DataTable DtJournalReturn = DSJournalReturn.Tables[0];
            DateTime TranDate = Convert.ToDateTime(DtJournalReturn.Rows[0][3].ToString());
            string tabledata = objConverter.ConvertDataTableToXML(DtJournalReturn);

            //String conn = ConfigurationSettings.AppSettings["DBConnectionDefault"];
            //SqlConnection con = new SqlConnection(conn);
            //SqlCommand cmd3 = new SqlCommand("JournalVoucherPrintFromInsert", con);
            //cmd3.CommandType = CommandType.StoredProcedure;
            //cmd3.Parameters.AddWithValue("@journalData", tabledata);
            //cmd3.Parameters.AddWithValue("@TransactionDate", TranDate);
            //cmd3.CommandTimeout = 0;
            //SqlDataAdapter Adap = new SqlDataAdapter();
            //Adap.SelectCommand = cmd3;
            //Adap.Fill(dsCrystal);
            //cmd3.Dispose();
            //con.Dispose();
            dsCrystal = oManagement_BL.JournalVoucherPrintFromInsert(tabledata, Convert.ToDateTime(TranDate).ToString("yyyy-MM-dd"));
            GC.Collect();

            dsCrystal.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "//Reports//Journal.xsd");
            string[] connPath = ConfigurationManager.AppSettings["DBConnectionDefault"].Split(';');
            ReportDocument reportObj = new ReportDocument();
            string ReportPath = Server.MapPath("..\\Reports\\Journal.rpt");
            reportObj.Load(ReportPath);
            reportObj.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
            reportObj.SetDataSource(dsCrystal);
            //reportObj.Subreports["logo"].SetDataSource(dsCrystal.Tables[0]);
            reportObj.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "Journal");
            reportObj.Dispose();
            GC.Collect();
        }
    }
}