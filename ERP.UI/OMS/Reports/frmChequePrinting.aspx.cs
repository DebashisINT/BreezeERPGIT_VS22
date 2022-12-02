using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_frmChequePrinting : System.Web.UI.Page
    {
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);

        public string qstr;

        static DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();
        DataTable dtAccount = new DataTable();
        string[] strbankname = null;
        int i = 0;
        DataView dv = new DataView();
        int BankID = 0;
        string strBankID = "";
        string strbank = "";
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
            tbl_Other.Visible = false;
            tbl_InstrumentDate.Visible = false;
            tbl_customer.Visible = false;
            tbl_InstrumentDate1.Visible = false;
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            //Bank Population
            if (!IsPostBack)
            {
                //  ddlBank.Items.Clear();

                dtpfromDate.Date = oDBEngine.GetDate();
                dtptoDate.Date = oDBEngine.GetDate();
                dtpupdateinstrumentdate.Date = oDBEngine.GetDate();
                dtpupdateinstrumentdate1.Date = oDBEngine.GetDate();

                dt1 = oDBEngine.GetDataTable("trans_cashbankdetail,Master_MainAccount", "distinct cashbankdetail_mainaccountid as ID,(cashbankdetail_mainaccountid+'~'+MainAccount_Name+'~'+MainAccount_BankAcNumber) as BankName", "trans_cashbankdetail.cashbankdetail_mainaccountid=Master_MainAccount.mainaccount_accountcode And MainAccount_BankCashType='Bank' and (MainAccount_BankCompany='" + HttpContext.Current.Session["LastCompany"].ToString() + "' OR Isnull(MainAccount_BankCompany,'')='')");
                if (dt1.Rows.Count > 0)
                {
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            ddlBank.Items.Add(new ListItem("-Select-", "0"));
                            ddlBank.Items.Add(new ListItem(dt1.Rows[i][1].ToString(), dt1.Rows[i][0].ToString()));
                        }
                        else
                        {
                            ddlBank.Items.Add(new ListItem(dt1.Rows[i][1].ToString(), dt1.Rows[i][0].ToString()));
                        }
                    }
                }
                //ddlBank.DataSource = dt1;
                //ddlBank.DataTextField = "BankName";
                //ddlBank.DataValueField = "ID";

                ddlBank.DataBind();
                ddlBank.SelectedValue = "0";

                //if (ddlGroup.SelectedValue == "1")
                //{
                //    //dt = oDBEngine.GetDataTable("(Select CashBankDetail_ID,MainAccount_Name,CashBankDetail_Subaccountid,CashBank_CashBankID,cashbankdetail_mainaccountid,cashbank_vouchernumber,isnull(CashBankDetail_InstrumentNumber,'') as CashBankDetail_InstrumentNumber,CashBankDetail_InstrumentDate,CashBank_TransactionDate,CashBankDetail_PaymentAmount from trans_cashbankdetail,Master_MainAccount,trans_cashbankvouchers WHERE CashBank_TransactionType='P' and trans_cashbankdetail.cashbankdetail_InstrumentType='C' and  trans_cashbankdetail.cashbankdetail_mainaccountid=Master_MainAccount.mainaccount_accountcode and trans_cashbankdetail.cashbankdetail_voucherID=trans_cashbankvouchers.CashBank_ID and Master_MainAccount.MainAccount_AccountType='Asset' and CashBank_TransactionDate Between  '" + Convert.ToDateTime(dtpfromDate.Value).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(dtptoDate.Value).ToString("yyyy-MM-dd") + "' and  CashBankDetail_CashBankID='" + ddlBank.SelectedValue + "' and CashBankDetail_CashBankID Is not Null and (trans_cashbankdetail.cashbankdetail_instrumentnumber ='' or len(trans_cashbankdetail.cashbankdetail_instrumentnumber)=0) and trans_cashbankdetail.cashbankdetail_instrumentnumber is not null) as a1 left outer join master_subaccount on(a1.cashbankdetail_Subaccountid=Master_SubAccount.SubAccount_Code and Master_SubAccount.SubAccount_MainAcReferenceID=a1.cashbankdetail_mainaccountid and a1.CashBank_CashBankID='" + ddlBank.SelectedValue + "') ", " TOP 50 CashBankDetail_ID,CashBankDetail_InstrumentNumber,case when master_subaccount.SubAccount_Name is null then MainAccount_Name else master_subaccount.SubAccount_Name end as MainAccount_Name,cashbank_vouchernumber,CashBankDetail_InstrumentDate,Convert(varchar(20),CashBank_TransactionDate,105) as CashBank_TransactionDate,dbo.[format_number](ABS(CashBankDetail_PaymentAmount)) as CashBankDetail_PaymentAmount,CashBankDetail_Subaccountid,cashbankdetail_mainaccountid  ", null, " CashBankDetail_ID");
                //    dt = oDBEngine.GetDataTable("(Select CashBankDetail_ID,MainAccount_Name,CashBankDetail_Subaccountid,CashBank_CashBankID,cashbankdetail_mainaccountid,cashbank_vouchernumber,isnull(CashBankDetail_InstrumentNumber,'') as CashBankDetail_InstrumentNumber,Convert(varchar(20),CashBankDetail_InstrumentDate,106) as CashBankDetail_InstrumentDate,CashBank_TransactionDate,CashBankDetail_PaymentAmount from trans_cashbankdetail,Master_MainAccount,trans_cashbankvouchers WHERE CashBank_TransactionType='P' and trans_cashbankdetail.cashbankdetail_InstrumentType='C' and  trans_cashbankdetail.cashbankdetail_mainaccountid=Master_MainAccount.mainaccount_accountcode and trans_cashbankdetail.cashbankdetail_voucherID=trans_cashbankvouchers.CashBank_ID and Master_MainAccount.MainAccount_AccountType='Asset' and CashBank_TransactionDate Between  '" + Convert.ToDateTime(dtpfromDate.Value).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(dtptoDate.Value).ToString("yyyy-MM-dd") + "' and  CashBankDetail_CashBankID='" + ddlBank.SelectedValue + "' and CashBankDetail_CashBankID Is not Null and (trans_cashbankdetail.cashbankdetail_instrumentnumber ='' or len(trans_cashbankdetail.cashbankdetail_instrumentnumber)=0) and trans_cashbankdetail.cashbankdetail_instrumentnumber is not null) as a1 left outer join master_subaccount on(a1.cashbankdetail_Subaccountid=Master_SubAccount.SubAccount_Code and Master_SubAccount.SubAccount_MainAcReferenceID=a1.cashbankdetail_mainaccountid and a1.CashBank_CashBankID='" + ddlBank.SelectedValue + "')left outer join tbl_trans_group on(grp_contactid=cashbankdetail_subaccountId)left outer join tbl_master_groupMaster ON(gpm_id=grp_groupMaster) left outer join tbl_master_contact on(cnt_internalID=cashbankdetail_subaccountId)left outer join tbl_master_branch on(branch_id=cnt_branchID)", " TOP 50 CashBankDetail_ID,CashBankDetail_InstrumentNumber,case when master_subaccount.SubAccount_Name is null then MainAccount_Name+'['+cnt_UCC+']' else master_subaccount.SubAccount_Name +'['+cnt_UCC+']' end as MainAccount_Name,cashbank_vouchernumber,CashBankDetail_InstrumentDate,Convert(varchar(20),CashBank_TransactionDate,106) as CashBank_TransactionDate,dbo.[format_number](ABS(CashBankDetail_PaymentAmount)) as CashBankDetail_PaymentAmount,CashBankDetail_Subaccountid,cashbankdetail_mainaccountid,grp_id,grp_groupMaster,gpm_description+'['+gpm_code+']' as GroupDescription,cnt_branchID,branch_description+'['+branch_code+']' as BranchDescription", null, "branch_id,MainAccount_Name");
                //    gridCheque.DataSource = dt.DefaultView;
                //    gridCheque.DataBind();
                //}
                //else if (ddlGroup.SelectedValue == "2")
                //{
                //    //dt = oDBEngine.GetDataTable("(Select CashBankDetail_ID,MainAccount_Name,CashBankDetail_Subaccountid,CashBank_CashBankID,cashbankdetail_mainaccountid,cashbank_vouchernumber,isnull(CashBankDetail_InstrumentNumber,'') as CashBankDetail_InstrumentNumber,CashBankDetail_InstrumentDate,CashBank_TransactionDate,CashBankDetail_PaymentAmount from trans_cashbankdetail,Master_MainAccount,trans_cashbankvouchers WHERE CashBank_TransactionType='P' and trans_cashbankdetail.cashbankdetail_InstrumentType='C' and  trans_cashbankdetail.cashbankdetail_mainaccountid=Master_MainAccount.mainaccount_accountcode and trans_cashbankdetail.cashbankdetail_voucherID=trans_cashbankvouchers.CashBank_ID and Master_MainAccount.MainAccount_AccountType='Asset' and CashBank_TransactionDate Between  '" + Convert.ToDateTime(dtpfromDate.Value).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(dtptoDate.Value).ToString("yyyy-MM-dd") + "' and  CashBankDetail_CashBankID='" + ddlBank.SelectedValue + "' and CashBankDetail_CashBankID Is not Null and (trans_cashbankdetail.cashbankdetail_instrumentnumber ='' or len(trans_cashbankdetail.cashbankdetail_instrumentnumber)=0) and trans_cashbankdetail.cashbankdetail_instrumentnumber is not null) as a1 left outer join master_subaccount on(a1.cashbankdetail_Subaccountid=Master_SubAccount.SubAccount_Code and Master_SubAccount.SubAccount_MainAcReferenceID=a1.cashbankdetail_mainaccountid and a1.CashBank_CashBankID='" + ddlBank.SelectedValue + "') ", " TOP 50 CashBankDetail_ID,CashBankDetail_InstrumentNumber,case when master_subaccount.SubAccount_Name is null then MainAccount_Name else master_subaccount.SubAccount_Name end as MainAccount_Name,cashbank_vouchernumber,CashBankDetail_InstrumentDate,Convert(varchar(20),CashBank_TransactionDate,105) as CashBank_TransactionDate,dbo.[format_number](ABS(CashBankDetail_PaymentAmount)) as CashBankDetail_PaymentAmount,CashBankDetail_Subaccountid,cashbankdetail_mainaccountid  ", null, " CashBankDetail_ID");
                //    dt = oDBEngine.GetDataTable("(Select CashBankDetail_ID,MainAccount_Name,CashBankDetail_Subaccountid,CashBank_CashBankID,cashbankdetail_mainaccountid,cashbank_vouchernumber,isnull(CashBankDetail_InstrumentNumber,'') as CashBankDetail_InstrumentNumber,Convert(varchar(20),CashBankDetail_InstrumentDate,106) as CashBankDetail_InstrumentDate,CashBank_TransactionDate,CashBankDetail_PaymentAmount from trans_cashbankdetail,Master_MainAccount,trans_cashbankvouchers WHERE CashBank_TransactionType='P' and trans_cashbankdetail.cashbankdetail_InstrumentType='C' and  trans_cashbankdetail.cashbankdetail_mainaccountid=Master_MainAccount.mainaccount_accountcode and trans_cashbankdetail.cashbankdetail_voucherID=trans_cashbankvouchers.CashBank_ID and Master_MainAccount.MainAccount_AccountType='Asset' and CashBank_TransactionDate Between  '" + Convert.ToDateTime(dtpfromDate.Value).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(dtptoDate.Value).ToString("yyyy-MM-dd") + "' and  CashBankDetail_CashBankID='" + ddlBank.SelectedValue + "' and CashBankDetail_CashBankID Is not Null and (trans_cashbankdetail.cashbankdetail_instrumentnumber ='' or len(trans_cashbankdetail.cashbankdetail_instrumentnumber)=0) and trans_cashbankdetail.cashbankdetail_instrumentnumber is not null) as a1 left outer join master_subaccount on(a1.cashbankdetail_Subaccountid=Master_SubAccount.SubAccount_Code and Master_SubAccount.SubAccount_MainAcReferenceID=a1.cashbankdetail_mainaccountid and a1.CashBank_CashBankID='" + ddlBank.SelectedValue + "')left outer join tbl_trans_group on(grp_contactid=cashbankdetail_subaccountId)left outer join tbl_master_groupMaster ON(gpm_id=grp_groupMaster) left outer join tbl_master_contact on(cnt_internalID=cashbankdetail_subaccountId)left outer join tbl_master_branch on(branch_id=cnt_branchID)", " TOP 50 CashBankDetail_ID,CashBankDetail_InstrumentNumber,case when master_subaccount.SubAccount_Name is null then MainAccount_Name+'['+cnt_UCC+']' else master_subaccount.SubAccount_Name +'['+cnt_UCC+']' end as MainAccount_Name,cashbank_vouchernumber,CashBankDetail_InstrumentDate,Convert(varchar(20),CashBank_TransactionDate,105) as CashBank_TransactionDate,dbo.[format_number](ABS(CashBankDetail_PaymentAmount)) as CashBankDetail_PaymentAmount,CashBankDetail_Subaccountid,cashbankdetail_mainaccountid,grp_id,grp_groupMaster,gpm_description+'['+gpm_code+']' as GroupDescription,cnt_branchID,branch_description+'['+branch_code+']' as BranchDescription", null, "grp_groupMaster,MainAccount_Name");
                //    gridCheque.DataSource = dt.DefaultView;
                //    gridCheque.DataBind();
                //}
                hdnFromdate.Value = Convert.ToDateTime(dtpfromDate.Value).ToString("yyyy-MM-dd");
                hdnTodate.Value = Convert.ToDateTime(dtptoDate.Value).ToString("yyyy-MM-dd");
                hdnBankType.Value = ddlChequeType.SelectedValue.ToString();

                ViewState["updatedate"] = dt;

                this.Page.ClientScript.RegisterStartupScript(GetType(), "heightR", "<script>PageLoad();</script>");
                this.Page.ClientScript.RegisterStartupScript(GetType(), "heightabcd", "<script>hidediv();</script>");
                hdnType.Value = "";
            }
            if (hdnType.Value == "Print")
            {
                //btnCrystalPrint.Enabled = false;
            }
        }
        protected void gridother_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                ((CheckBox)e.Row.FindControl("cbSelectAll1")).Attributes.Add("onclick1", "javascript:SelectAllInterSegment1('" + ((CheckBox)e.Row.FindControl("cbSelectAll1")).ClientID + "')");
            }

        }

        protected void gridCheque_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.Header)
            {
                ((CheckBox)e.Row.FindControl("cbSelectAll")).Attributes.Add("onclick", "javascript:SelectAllInterSegment('" + ((CheckBox)e.Row.FindControl("cbSelectAll")).ClientID + "')");
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string strCkientID = ((Label)e.Row.FindControl("lblClientID")).Text.ToString();
                dtAccount = oDBEngine.GetDataTable("(Select CashBankDetail_ID,case when master_subaccount.SubAccount_Name is null then MainAccount_Name else master_subaccount.SubAccount_Name end as MainAccount_Name,cashbank_vouchernumber,Convert(varchar(20),CashBankDetail_InstrumentDate,106) as CashBankDetail_InstrumentDate,Convert(varchar(20),CashBank_TransactionDate,106) as CashBank_TransactionDate,dbo.[format_number](ABS(CashBankDetail_PaymentAmount)) as CashBankDetail_PaymentAmount,master_subaccount.SubAccount_Name,CashBankDetail_Subaccountid from (Select CashBankDetail_ID,MainAccount_Name,CashBankDetail_Subaccountid,CashBank_CashBankID,cashbankdetail_mainaccountid,cashbank_vouchernumber,CashBankDetail_InstrumentDate,CashBank_TransactionDate,CashBankDetail_PaymentAmount from trans_cashbankdetail,Master_MainAccount,trans_cashbankvouchers", " TOP 1 (LTRIM(RTRIM(isnull(AccountNumber,'')))+'                   '+ LTRIM(RTRIM(isnull(bnk_bankname,'')))) as AccountNumber,isnull(cbd_id,'') as cbd_id from(Select CashBankDetail_ID,tbl_trans_contactbankdetails.cbd_accountCategory,tbl_trans_contactbankdetails.cbd_accountCategory+'~'+tbl_trans_contactbankdetails.cbd_accountNumber as AccountNumber,tbl_trans_contactbankdetails.cbd_id,tbl_trans_contactbankdetails.cbd_bankCode", "CashBank_TransactionType='P' and trans_cashbankdetail.cashbankdetail_InstrumentType='C' and  trans_cashbankdetail.cashbankdetail_mainaccountid=Master_MainAccount.mainaccount_accountcode and trans_cashbankdetail.cashbankdetail_voucherID=trans_cashbankvouchers.CashBank_ID and CashBank_TransactionDate Between '" + Convert.ToDateTime(dtpfromDate.Value).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(dtptoDate.Value).ToString("yyyy-MM-dd") + "' and  CashBankDetail_CashBankID='" + ddlBank.SelectedValue.ToString() + "' and CashBankDetail_CashBankID Is not Null and (trans_cashbankdetail.cashbankdetail_instrumentnumber ='' or len(trans_cashbankdetail.cashbankdetail_instrumentnumber)=0) and trans_cashbankdetail.cashbankdetail_instrumentnumber is not null) as a1 left outer join master_subaccount on(a1.cashbankdetail_Subaccountid=Master_SubAccount.SubAccount_Code and Master_SubAccount.SubAccount_MainAcReferenceID=a1.cashbankdetail_mainaccountid AND a1.cashbankdetail_Subaccountid='" + strCkientID + "' AND Master_SubAccount.SubAccount_Code='" + strCkientID + "' and a1.CashBank_CashBankID='" + ddlBank.SelectedValue.ToString() + "')) as a2 left outer join tbl_trans_contactbankdetails on(a2.cashbankdetail_Subaccountid=cbd_cntid and cbd_cntid='" + strCkientID + "') where cbd_accountNumber is not null and cbd_id is not null and cbd_cntid is not null) as a3 left outer join tbl_master_bank ON(bnk_id=a3.cbd_bankCode)Order By a3.cbd_accountCategory,a3.CashBankDetail_ID");
                ((DropDownList)e.Row.FindControl("ddlClientAccount")).DataSource = dtAccount;
                ((DropDownList)e.Row.FindControl("ddlClientAccount")).DataTextField = "AccountNumber";
                ((DropDownList)e.Row.FindControl("ddlClientAccount")).DataValueField = "cbd_id";
                ((DropDownList)e.Row.FindControl("ddlClientAccount")).DataBind();
            }


        }

        protected void ddlBank_SelectedIndexChanged1(object sender, EventArgs e)
        {
            tbl_Other.Visible = true;
            btnSave.Visible = false;
            tbl_InstrumentDate.Visible = false;
            tbl_customer.Visible = true;
            tbl_ucc.Visible = true;
            btnsave1.Visible = false;
            tbl_InstrumentDate1.Visible = false;
            hdnFromdate.Value = Convert.ToDateTime(dtpfromDate.Value).ToString("yyyy-MM-dd");
            hdnTodate.Value = Convert.ToDateTime(dtptoDate.Value).ToString("yyyy-MM-dd");
            hdnBankType.Value = ddlChequeType.SelectedValue.ToString();
            this.Page.ClientScript.RegisterStartupScript(GetType(), "heightP", "<script>height();</script>");
            this.Page.ClientScript.RegisterStartupScript(GetType(), "heightOM", "<script>DeliveryProcessButton();</script>");

        }
        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownList1.SelectedValue == "1")
            {
                tbl_ucc.Visible = true;
            }
            else
            {
                tbl_ucc.Visible = false;
            }

            tbl_Other.Visible = true;
            btnSave.Visible = false;
            tbl_InstrumentDate.Visible = false;
            tbl_customer.Visible = true;
            btnsave1.Visible = false;


        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            tbl_Other.Visible = true;
            btnSave.Visible = false;
            //tbl_InstrumentDate.Visible = true;
            tbl_customer.Visible = true;
            btnsave1.Visible = false;
            if (DropDownList1.SelectedValue.ToString() == "2")
            {
                tbl_InstrumentDate1.Visible = true;
            }
            else
            {
                tbl_InstrumentDate.Visible = true;
            }
            this.Page.ClientScript.RegisterStartupScript(GetType(), "heightabc", "<script>showdiv();</script>");

            hdnFromdate.Value = Convert.ToDateTime(dtpfromDate.Value).ToString("yyyy-MM-dd");
            hdnTodate.Value = Convert.ToDateTime(dtptoDate.Value).ToString("yyyy-MM-dd");
            hdnBankType.Value = ddlChequeType.SelectedValue.ToString();
            hdnUcc.Value = ddlUcc.SelectedItem.Value.ToString();
            hdnAccount.Value = ddlAccount.SelectedItem.Value.ToString();
            if (DropDownList1.SelectedValue == "1")
            {

                dt = oDBEngine.GetDataTable("(Select CashBankDetail_ID,MainAccount_Name,CashBankDetail_Subaccountid,CashBank_CashBankID,cashbankdetail_mainaccountid,cashbank_vouchernumber,isnull(CashBankDetail_InstrumentNumber,'') as CashBankDetail_InstrumentNumber,getdate() as CashBankDetail_InstrumentDate,CashBank_TransactionDate,CashBankDetail_PaymentAmount from trans_cashbankdetail,Master_MainAccount,trans_cashbankvouchers WHERE CashBank_TransactionType='P' and trans_cashbankdetail.cashbankdetail_InstrumentType='C' and  trans_cashbankdetail.cashbankdetail_mainaccountid=Master_MainAccount.mainaccount_accountcode and trans_cashbankdetail.cashbankdetail_voucherID=trans_cashbankvouchers.CashBank_ID and Master_MainAccount.MainAccount_SUBLEDGERTYPE='Customers' and CashBank_TransactionDate Between  '" + Convert.ToDateTime(dtpfromDate.Value).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(dtptoDate.Value).ToString("yyyy-MM-dd") + "' and  CashBankDetail_CashBankID='" + ddlBank.SelectedValue + "' and CashBankDetail_CashBankID Is not Null and (trans_cashbankdetail.cashbankdetail_instrumentnumber ='' or len(trans_cashbankdetail.cashbankdetail_instrumentnumber)=0) and trans_cashbankdetail.cashbankdetail_instrumentnumber is not null) as a1 left outer join master_subaccount on(a1.cashbankdetail_Subaccountid=Master_SubAccount.SubAccount_Code and Master_SubAccount.SubAccount_MainAcReferenceID=a1.cashbankdetail_mainaccountid and a1.CashBank_CashBankID='" + ddlBank.SelectedValue + "')left outer join tbl_master_contact on(cnt_internalID=cashbankdetail_subaccountId)left outer join tbl_master_branch on(branch_id=cnt_branchID)", " TOP 50 CashBankDetail_ID,CashBankDetail_InstrumentNumber,case when master_subaccount.SubAccount_Name is null then MainAccount_Name+'['+cnt_UCC+']' else master_subaccount.SubAccount_Name +'['+cnt_UCC+']' end as MainAccount_Name,cashbank_vouchernumber,CashBankDetail_InstrumentDate,Convert(varchar(20),CashBank_TransactionDate,105) as CashBank_TransactionDate,dbo.[format_number](ABS(CashBankDetail_PaymentAmount)) as Payment,CashBankDetail_Subaccountid,cashbankdetail_mainaccountid,cnt_branchID,branch_description+'['+branch_code+']' as BranchDescription", null, "cnt_branchID,mainaccount_name");
                gridCheque.DataSource = dt.DefaultView;
                gridCheque.DataBind();
                gridother.Visible = false;
                btnSave.Visible = true;
            }
            else if (DropDownList1.SelectedValue == "2")
            {

                dt = oDBEngine.GetDataTable("(Select CashBankDetail_ID,MainAccount_Name,CashBankDetail_Subaccountid,CashBank_CashBankID,cashbankdetail_payeeaccountid,cashbankdetail_mainaccountid,cashbank_vouchernumber,isnull(CashBankDetail_InstrumentNumber,'') as CashBankDetail_InstrumentNumber,Convert(varchar(20),CashBankDetail_InstrumentDate,106) as CashBankDetail_InstrumentDate,CashBank_TransactionDate,CashBankDetail_PaymentAmount from trans_cashbankdetail,Master_MainAccount,trans_cashbankvouchers WHERE CashBank_TransactionType='P' and trans_cashbankdetail.cashbankdetail_InstrumentType='C' and  trans_cashbankdetail.cashbankdetail_mainaccountid=Master_MainAccount.mainaccount_accountcode and trans_cashbankdetail.cashbankdetail_voucherID=trans_cashbankvouchers.CashBank_ID and Master_MainAccount.MainAccount_subledgerType<>'Customers' and CashBank_TransactionDate Between  '" + Convert.ToDateTime(dtpfromDate.Value).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(dtptoDate.Value).ToString("yyyy-MM-dd") + "' and  CashBankDetail_CashBankID='" + ddlBank.SelectedValue + "' and CashBankDetail_CashBankID Is not Null and (trans_cashbankdetail.cashbankdetail_instrumentnumber ='' or len(trans_cashbankdetail.cashbankdetail_instrumentnumber)=0) and trans_cashbankdetail.cashbankdetail_instrumentnumber is not null) as a1 left outer join master_subaccount on(a1.cashbankdetail_Subaccountid=Master_SubAccount.SubAccount_Code and Master_SubAccount.SubAccount_MainAcReferenceID=a1.cashbankdetail_mainaccountid and a1.CashBank_CashBankID='" + ddlBank.SelectedValue + "') left outer join tbl_master_contact on(cnt_internalID=cashbankdetail_subaccountId)", " TOP 50 CashBankDetail_ID,CashBankDetail_InstrumentNumber,case when master_subaccount.SubAccount_Name is null then MainAccount_Name when cashbankdetail_payeeaccountid like 'VR%' then (select (Cnt_FirstName)+' '+(Cnt_MiddleName)+' '+(Cnt_LastName) from tbl_master_contact where cnt_internalid=cashbankdetail_payeeaccountid)  else master_subaccount.SubAccount_Name end as MainAccount_Name,cashbank_vouchernumber,CashBankDetail_InstrumentDate,Convert(varchar(20),CashBank_TransactionDate,105) as CashBank_TransactionDate,dbo.[format_number](ABS(CashBankDetail_PaymentAmount)) as Payment,isnull (CashBankDetail_Subaccountid,'') as CashBankDetail_Subaccountid,cashbankdetail_mainaccountid", null, "cashbankdetail_id");
                gridother.DataSource = dt.DefaultView;
                gridother.DataBind();
                gridCheque.Visible = false;
                btnsave1.Visible = true;
            }
            ViewState["updatedate"] = dt;
            this.Page.ClientScript.RegisterStartupScript(GetType(), "heightO", "<script>height();</script>");
            this.Page.ClientScript.RegisterStartupScript(GetType(), "heightOL", "<script>HideOn();</script>");

        }
        protected void btnAllocate_Click(object sender, EventArgs e)
        {
            tbl_Other.Visible = true;
            string strdate = "";
            string strID = "";

            DataTable dtbl = new DataTable();
            dtbl = (DataTable)ViewState["updatedate"];



            foreach (DataRow dr in dtbl.Rows)
            {


                dr["CashBankDetail_InstrumentDate"] = dtpupdateinstrumentdate.Date.ToString("yyyy-MM-dd");
            }

            dtbl.AcceptChanges();

            gridCheque.DataSource = dtbl;
            gridCheque.DataBind();



            foreach (GridViewRow row in gridCheque.Rows)
            {

                Label lblID = (Label)row.FindControl("lblCashBankDetailID");
                if (lblID == null)
                {
                    lblID.Text = "";
                }
                Label lblClientID = (Label)row.FindControl("lblClientID");
                if (lblClientID == null)
                {
                    lblClientID.Text = "";
                }
                Label lblMainClientID = (Label)row.FindControl("lblMainClientID");
                if (lblMainClientID == null)
                {
                    lblMainClientID.Text = "";
                }
                Label lblVocherNumber = (Label)row.FindControl("lblVocherNumber");
                TextBox lblChequeNumber = (TextBox)row.FindControl("txtChequeNumber");

                Label lblBankDate = (Label)row.FindControl("lblBankDate");
                ASPxDateEdit dtpInstrumentDate = (ASPxDateEdit)row.FindControl("dtpInstrumentDate");

                CheckBox ChkDelivery = (CheckBox)row.FindControl("ChkDelivery");

                DropDownList ddlClient = (DropDownList)row.FindControl("ddlClientAccount");
                strID = lblID.Text.ToString();

                strdate = dtpupdateinstrumentdate.Date.ToString("yyyy-MM-dd");

                i = oDBEngine.SetFieldValue("trans_cashbankdetail", "CashBankDetail_InstrumentDate='" + strdate + "'", "CashbankDetail_ID IN(" + strID + ")");


            }
        }

        protected void btnAllocate1_Click(object sender, EventArgs e)
        {
            tbl_Other.Visible = true;
            string strdate = "";
            string strID = "";

            DataTable dtbl = new DataTable();
            dtbl = (DataTable)ViewState["updatedate"];



            foreach (DataRow dr in dtbl.Rows)
            {


                dr["CashBankDetail_InstrumentDate"] = dtpupdateinstrumentdate1.Date.ToString("yyyy-MM-dd");
            }

            dtbl.AcceptChanges();


            gridother.DataSource = dtbl;
            gridother.DataBind();


            foreach (GridViewRow row in gridother.Rows)
            {

                Label lblID = (Label)row.FindControl("lblCashBankDetailID1");
                if (lblID == null)
                {
                    lblID.Text = "";
                }

                Label lblVocherNumber = (Label)row.FindControl("lblVocherNumber");
                TextBox lblChequeNumber = (TextBox)row.FindControl("txtChequeNumber");

                Label lblBankDate = (Label)row.FindControl("lblBankDate");
                ASPxDateEdit dtpInstrumentDate = (ASPxDateEdit)row.FindControl("dtpInstrumentDate");

                CheckBox ChkDelivery = (CheckBox)row.FindControl("ChkDelivery");


                strID = lblID.Text.ToString();

                strdate = dtpupdateinstrumentdate1.Date.ToString("yyyy-MM-dd");

                i = oDBEngine.SetFieldValue("trans_cashbankdetail", "CashBankDetail_InstrumentDate='" + strdate + "'", "CashbankDetail_ID IN(" + strID + ")");


            }
        }
        protected void gridCheque_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            foreach (GridViewRow row in gridCheque.Rows)
            {
                for (int i = 0; i <= gridCheque.Rows.Count; i++)
                {
                    Label lblInstrumentDate = (Label)row.FindControl("lblInstrumentDate");
                    lblInstrumentDate.Text = dtpupdateinstrumentdate.Date.ToString("yyyy-MM-dd");

                    gridCheque.UpdateRow(i, true);
                }
            }
        }
        protected void gridCheque_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void gridother_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            foreach (GridViewRow row in gridother.Rows)
            {
                for (int i = 0; i <= gridother.Rows.Count; i++)
                {
                    Label lblInstrumentDate = (Label)row.FindControl("lblInstrumentDate");
                    lblInstrumentDate.Text = dtpupdateinstrumentdate1.Date.ToString("yyyy-MM-dd");

                    gridother.UpdateRow(i, true);
                }
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {

            string IDS = "";
            int Counter = 0;
            strbankname = ddlBank.SelectedItem.Text.ToString().Split('~');
            strbank = strbankname[1].ToString() + " " + strbankname[2].ToString();


            DataTable dtsave = new DataTable();
            dtsave.Columns.Add(new DataColumn("CashBankDetail_RecordID", typeof(int)));//0
            dtsave.Columns.Add(new DataColumn("CashBankDetail_ID", typeof(int)));//0
            dtsave.Columns.Add(new DataColumn("CashBankDetail_InstrumentNumber", typeof(string)));//2
            dtsave.Columns.Add(new DataColumn("MainAccount_Name", typeof(string)));//3
            dtsave.Columns.Add(new DataColumn("cashbank_vouchernumber", typeof(string)));//4
            dtsave.Columns.Add(new DataColumn("CashBankDetail_InstrumentDate", typeof(string)));//5
            dtsave.Columns.Add(new DataColumn("CashBank_TransactionDate", typeof(string)));//6
            dtsave.Columns.Add(new DataColumn("Payment", typeof(decimal)));//7
            dtsave.Columns.Add(new DataColumn("CashBankDetail_Subaccountid", typeof(string)));//8
            dtsave.Columns.Add(new DataColumn("cashbankdetail_mainaccountid", typeof(string)));//9
            // dtsave.Columns.Add(new DataColumn("grp_id", typeof(string)));//10
            //dtsave.Columns.Add(new DataColumn("grp_groupMaster", typeof(string)));//11
            // dtsave.Columns.Add(new DataColumn("GroupDescription", typeof(string)));//12
            dtsave.Columns.Add(new DataColumn("cnt_branchID", typeof(int)));//13
            dtsave.Columns.Add(new DataColumn("BranchDescription", typeof(string)));//14
            dtsave.Columns.Add(new DataColumn("BankID", typeof(string)));//15
            dtsave.Columns.Add(new DataColumn("ClientBankID", typeof(string)));//16
            dtsave.Columns.Add(new DataColumn("ClientBankName", typeof(string)));//17

            foreach (GridViewRow row in gridCheque.Rows)
            {

                CheckBox ChkDelivery = (CheckBox)row.FindControl("ChkDelivery");
                if (ChkDelivery.Checked == true)
                {

                    Label lblID = (Label)row.FindControl("lblCashBankDetailID");
                    if (lblID == null)
                    {
                        lblID.Text = "";
                    }
                    Label lblMainAccountID = (Label)row.FindControl("lblMainAccount");
                    if (lblMainAccountID == null)
                    {
                        lblMainAccountID.Text = "";
                    }

                    Label lblVocherNumber = (Label)row.FindControl("lblVocherNumber");
                    TextBox lblChequeNumber = (TextBox)row.FindControl("txtChequeNumber");
                    Label lblInstrumentDate = (Label)row.FindControl("lblInstrumentDate");
                    Label lblBankDate = (Label)row.FindControl("lblBankDate");

                    Label lblPaymentAmount = (Label)row.FindControl("lblPaymentAmount");
                    Label lblClientID = (Label)row.FindControl("lblClientID");
                    Label lblGRPID = (Label)row.FindControl("lblGRPID");
                    Label lblgroupMaster = (Label)row.FindControl("lblgroupMaster");
                    Label lblgroupDescription = (Label)row.FindControl("lblgroupDescription");
                    Label lblBranchID = (Label)row.FindControl("lblBranchID");
                    Label lblBranchDescription = (Label)row.FindControl("lblBranchDescription");
                    DropDownList ddlClient = (DropDownList)row.FindControl("ddlClientAccount");

                    IDS += "" + lblID.Text.ToString() + "";

                    strbankname = ddlBank.SelectedItem.Text.ToString().Split('~');
                    strbank = strbankname[1].ToString() + " " + strbankname[2].ToString();


                    if (lblClientID == null)
                    {
                        lblClientID.Text = "";
                    }
                    Label lblMainClientID = (Label)row.FindControl("lblMainClientID");
                    if (lblMainClientID == null)
                    {
                        lblMainClientID.Text = "";
                    }
                    if (ddlClient.Items.Count > 0)
                    {
                        BankID = Convert.ToInt32(ddlClient.SelectedValue.ToString());
                        strBankID += ddlClient.SelectedValue.ToString() + ",";
                    }
                    else
                    {
                        BankID = 0;
                        strBankID = "";
                    }
                    DataRow drReport = dtsave.NewRow();
                    drReport["CashBankDetail_RecordID"] = ++Counter;
                    drReport["CashBankDetail_ID"] = lblID.Text.ToString();
                    drReport["CashBankDetail_InstrumentNumber"] = lblChequeNumber.Text.ToString();
                    drReport["MainAccount_Name"] = lblMainAccountID.Text.ToString();
                    drReport["cashbank_vouchernumber"] = lblVocherNumber.Text.ToString();
                    drReport["CashBankDetail_InstrumentDate"] = dtpupdateinstrumentdate.Text.ToString();
                    drReport["CashBank_TransactionDate"] = lblBankDate.Text.ToString();
                    drReport["Payment"] = lblPaymentAmount.Text.ToString();
                    drReport["CashBankDetail_Subaccountid"] = lblClientID.Text.ToString();
                    drReport["cashbankdetail_mainaccountid"] = lblMainClientID.Text.ToString();
                    //drReport["grp_id"] = lblGRPID.Text.ToString();
                    //drReport["grp_groupMaster"] = lblgroupMaster.Text.ToString();
                    //drReport["GroupDescription"] = lblgroupDescription.Text.ToString();
                    drReport["cnt_branchID"] = lblBranchID.Text.ToString();
                    drReport["BranchDescription"] = lblBranchDescription.Text.ToString();
                    drReport["BankID"] = ddlBank.SelectedValue.ToString();
                    drReport["ClientBankID"] = Convert.ToInt32(ddlClient.SelectedValue.ToString());
                    drReport["ClientBankName"] = strbank.ToString();
                    dtsave.Rows.Add(drReport);
                    dtsave.AcceptChanges();
                    oDBEngine.InsurtFieldValue("trans_chequeprintlog", "chequeprintlog_finyear,chequeprintlog_companyid,chequeprintlog_cashbankid,chequeprintlog_referenceid,chequeprintlog_mainaccountid,chequeprintlog_subaccountid,chequeprintlog_amount,chequeprintlog_instrumentnumber,chequeprintlog_instrumentdate,chequeprintlog_createuser,chequeprintlog_createdatetime",
                        "'" + Session["LastFinYear"].ToString() + "','" + Session["LastCompany"].ToString() + "','" + ddlBank.SelectedValue.ToString() + "','" + lblID.Text.ToString() + "','" + lblMainClientID.Text.ToString() + "','" + lblClientID.Text.ToString() + "'," + lblPaymentAmount.Text.Replace(",", "").ToString() + ",'" + lblChequeNumber.Text.ToString() + "','" + Convert.ToDateTime(dtpupdateinstrumentdate.Value).ToString("yyyy-MM-dd") + "','" + Session["userid"].ToString() + "','" + oDBEngine.GetDate().ToString() + "'");

                }

                Session["Data"] = (DataTable)dtsave;

                string ID = hdnID.Value.ToString();
                string bank = hdnbank.Value.ToString();
                string FromDate = hdnFromdate.Value.ToString();
                string ToDate = hdnTodate.Value.ToString();
                string strBank = hdnstrBank.Value.ToString();
                string chequenumber = hdnChequeNumber.Value.ToString();
                string BankType = hdnBankType.Value.ToString();
                string chkAccount = hdnAccount.Value.ToString();
                string chkUcc = hdnUcc.Value.ToString();

                this.Page.ClientScript.RegisterStartupScript(GetType(), "abcd1", "<script>OnMoreInfoClick();</script>");

            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            hdnFromdate.Value = Convert.ToDateTime(dtpfromDate.Value).ToString("yyyy-MM-dd");
            hdnTodate.Value = Convert.ToDateTime(dtptoDate.Value).ToString("yyyy-MM-dd");
            hdnBankType.Value = ddlChequeType.SelectedValue.ToString();

            if (DropDownList1.SelectedValue == "1")
            {
                dt = oDBEngine.GetDataTable("(Select CashBankDetail_ID,MainAccount_Name,CashBankDetail_Subaccountid,CashBank_CashBankID,cashbankdetail_mainaccountid,cashbank_vouchernumber,isnull(CashBankDetail_InstrumentNumber,'') as CashBankDetail_InstrumentNumber,Convert(varchar(20),CashBankDetail_InstrumentDate,106) as CashBankDetail_InstrumentDate,CashBank_TransactionDate,CashBankDetail_PaymentAmount from trans_cashbankdetail,Master_MainAccount,trans_cashbankvouchers WHERE CashBank_TransactionType='P' and trans_cashbankdetail.cashbankdetail_InstrumentType='C' and  trans_cashbankdetail.cashbankdetail_mainaccountid=Master_MainAccount.mainaccount_accountcode and trans_cashbankdetail.cashbankdetail_voucherID=trans_cashbankvouchers.CashBank_ID and Master_MainAccount.MainAccount_AccountType='Asset' and CashBank_TransactionDate Between  '" + Convert.ToDateTime(dtpfromDate.Value).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(dtptoDate.Value).ToString("yyyy-MM-dd") + "' and  CashBankDetail_CashBankID='" + ddlBank.SelectedValue + "' and CashBankDetail_CashBankID Is not Null and (trans_cashbankdetail.cashbankdetail_instrumentnumber ='' or len(trans_cashbankdetail.cashbankdetail_instrumentnumber)=0) and trans_cashbankdetail.cashbankdetail_instrumentnumber is not null) as a1 left outer join master_subaccount on(a1.cashbankdetail_Subaccountid=Master_SubAccount.SubAccount_Code and Master_SubAccount.SubAccount_MainAcReferenceID=a1.cashbankdetail_mainaccountid and a1.CashBank_CashBankID='" + ddlBank.SelectedValue + "')left outer join tbl_trans_group on(grp_contactid=cashbankdetail_subaccountId)left outer join tbl_master_groupMaster ON(gpm_id=grp_groupMaster) left outer join tbl_master_contact on(cnt_internalID=cashbankdetail_subaccountId)left outer join tbl_master_branch on(branch_id=cnt_branchID)", " TOP 50 CashBankDetail_ID,CashBankDetail_InstrumentNumber,case when master_subaccount.SubAccount_Name is null then MainAccount_Name+'['+cnt_UCC+']' else master_subaccount.SubAccount_Name +'['+cnt_UCC+']' end as MainAccount_Name,cashbank_vouchernumber,CashBankDetail_InstrumentDate,Convert(varchar(20),CashBank_TransactionDate,105) as CashBank_TransactionDate,dbo.[format_number](ABS(CashBankDetail_PaymentAmount)) as Payment,CashBankDetail_Subaccountid,cashbankdetail_mainaccountid,grp_id,grp_groupMaster,gpm_description+'['+gpm_code+']' as GroupDescription,cnt_branchID,branch_description+'['+branch_code+']' as BranchDescription", null, "branch_id,MainAccount_Name");
                gridCheque.DataSource = dt.DefaultView;
                gridCheque.DataBind();
            }
            else if (DropDownList1.SelectedValue == "2")
            {
                dt = oDBEngine.GetDataTable("(Select CashBankDetail_ID,MainAccount_Name,CashBankDetail_Subaccountid,CashBank_CashBankID,cashbankdetail_mainaccountid,cashbank_vouchernumber,isnull(CashBankDetail_InstrumentNumber,'') as CashBankDetail_InstrumentNumber,Convert(varchar(20),CashBankDetail_InstrumentDate,106) as CashBankDetail_InstrumentDate,CashBank_TransactionDate,CashBankDetail_PaymentAmount from trans_cashbankdetail,Master_MainAccount,trans_cashbankvouchers WHERE CashBank_TransactionType='P' and trans_cashbankdetail.cashbankdetail_InstrumentType='C' and  trans_cashbankdetail.cashbankdetail_mainaccountid=Master_MainAccount.mainaccount_accountcode and trans_cashbankdetail.cashbankdetail_voucherID=trans_cashbankvouchers.CashBank_ID and Master_MainAccount.MainAccount_AccountType='Asset' and CashBank_TransactionDate Between  '" + Convert.ToDateTime(dtpfromDate.Value).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(dtptoDate.Value).ToString("yyyy-MM-dd") + "' and  CashBankDetail_CashBankID='" + ddlBank.SelectedValue + "' and CashBankDetail_CashBankID Is not Null and (trans_cashbankdetail.cashbankdetail_instrumentnumber ='' or len(trans_cashbankdetail.cashbankdetail_instrumentnumber)=0) and trans_cashbankdetail.cashbankdetail_instrumentnumber is not null) as a1 left outer join master_subaccount on(a1.cashbankdetail_Subaccountid=Master_SubAccount.SubAccount_Code and Master_SubAccount.SubAccount_MainAcReferenceID=a1.cashbankdetail_mainaccountid and a1.CashBank_CashBankID='" + ddlBank.SelectedValue + "')left outer join tbl_trans_group on(grp_contactid=cashbankdetail_subaccountId)left outer join tbl_master_groupMaster ON(gpm_id=grp_groupMaster) left outer join tbl_master_contact on(cnt_internalID=cashbankdetail_subaccountId)left outer join tbl_master_branch on(branch_id=cnt_branchID)", " TOP 50 CashBankDetail_ID,CashBankDetail_InstrumentNumber,case when master_subaccount.SubAccount_Name is null then MainAccount_Name+'['+cnt_UCC+']' else master_subaccount.SubAccount_Name +'['+cnt_UCC+']' end as MainAccount_Name,cashbank_vouchernumber,CashBankDetail_InstrumentDate,Convert(varchar(20),CashBank_TransactionDate,105) as CashBank_TransactionDate,dbo.[format_number](ABS(CashBankDetail_PaymentAmount)) as Payment,CashBankDetail_Subaccountid,cashbankdetail_mainaccountid,grp_id,grp_groupMaster,gpm_description+'['+gpm_code+']' as GroupDescription,cnt_branchID,branch_description+'['+branch_code+']' as BranchDescription", null, "grp_groupMaster,MainAccount_Name");
                gridother.DataSource = dt.DefaultView;
                gridother.DataBind();
            }

            this.Page.ClientScript.RegisterStartupScript(GetType(), "heightO", "<script>height();</script>");
            this.Page.ClientScript.RegisterStartupScript(GetType(), "heightOL", "<script>HideOn();</script>");
            this.Page.ClientScript.RegisterStartupScript(GetType(), "heightabcdef", "<script>showdiv();</script>");

        }


        protected void gridother_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void btnSave1_Click(object sender, EventArgs e)
        {

            string IDS1 = "";
            DataTable dtsave1 = new DataTable();
            dtsave1.Columns.Add(new DataColumn("CashBankDetail_ID", typeof(int)));//0

            dtsave1.Columns.Add(new DataColumn("CashBankDetail_InstrumentNumber", typeof(string)));//2
            dtsave1.Columns.Add(new DataColumn("MainAccount_Name", typeof(string)));//3
            dtsave1.Columns.Add(new DataColumn("cashbank_vouchernumber", typeof(string)));//4
            dtsave1.Columns.Add(new DataColumn("CashBankDetail_InstrumentDate", typeof(string)));//5
            dtsave1.Columns.Add(new DataColumn("CashBank_TransactionDate", typeof(string)));//6
            dtsave1.Columns.Add(new DataColumn("Payment", typeof(decimal)));//7
            dtsave1.Columns.Add(new DataColumn("CashBankDetail_Subaccountid", typeof(string)));//8
            dtsave1.Columns.Add(new DataColumn("cashbankdetail_mainaccountid", typeof(string)));//9
            //dtsave1.Columns.Add(new DataColumn("CashBankDetail_InstrumentDate", typeof(string)));//10

            foreach (GridViewRow row in gridother.Rows)
            {

                CheckBox ChkDelivery = (CheckBox)row.FindControl("ChkDelivery1");
                if (ChkDelivery.Checked == true)
                {

                    Label lblID1 = (Label)row.FindControl("lblCashBankDetailID1");
                    if (lblID1 == null)
                    {
                        lblID1.Text = "";
                    }
                    Label lblMainAccountID1 = (Label)row.FindControl("lblMainAccount1");
                    if (lblMainAccountID1 == null)
                    {
                        lblMainAccountID1.Text = "";
                    }

                    Label lblVocherNumber1 = (Label)row.FindControl("lblVocherNumber1");
                    TextBox lblChequeNumber1 = (TextBox)row.FindControl("txtChequeNumber1");
                    Label lblInstrumentDate1 = (Label)row.FindControl("lblInstrumentDate1");
                    Label lblBankDate1 = (Label)row.FindControl("lblBankDate1");
                    Label lblInstrumentDate = (Label)row.FindControl("lblInstrumentDate");
                    Label lblPaymentAmount1 = (Label)row.FindControl("lblPaymentAmount1");
                    Label lblClientID1 = (Label)row.FindControl("lblClientID1");


                    IDS1 += "" + lblID1.Text.ToString() + "";


                    Label lblMainClientID1 = (Label)row.FindControl("lblMainClientID1");
                    if (lblMainClientID1 == null)
                    {
                        lblMainClientID1.Text = "";
                    }


                    DataRow drReport = dtsave1.NewRow();
                    drReport["CashBankDetail_ID"] = lblID1.Text.ToString();
                    drReport["CashBankDetail_InstrumentNumber"] = lblChequeNumber1.Text.ToString();
                    drReport["MainAccount_Name"] = lblMainAccountID1.Text.ToString();
                    drReport["cashbank_vouchernumber"] = lblVocherNumber1.Text.ToString();
                    drReport["CashBankDetail_InstrumentDate"] = dtpupdateinstrumentdate1.Text.ToString();
                    drReport["CashBank_TransactionDate"] = lblBankDate1.Text.ToString();
                    drReport["Payment"] = lblPaymentAmount1.Text.ToString();
                    drReport["CashBankDetail_Subaccountid"] = lblClientID1.Text.ToString();
                    drReport["cashbankdetail_mainaccountid"] = lblMainClientID1.Text.ToString();

                    dtsave1.Rows.Add(drReport);
                    dtsave1.AcceptChanges();
                    oDBEngine.InsurtFieldValue("trans_chequeprintlog", "chequeprintlog_finyear,chequeprintlog_companyid,chequeprintlog_cashbankid,chequeprintlog_referenceid,chequeprintlog_mainaccountid,chequeprintlog_subaccountid,chequeprintlog_amount,chequeprintlog_instrumentnumber,chequeprintlog_instrumentdate,chequeprintlog_createuser,chequeprintlog_createdatetime",
                             "'" + Session["LastFinYear"].ToString() + "','" + Session["LastCompany"].ToString() + "','" + ddlBank.SelectedValue.ToString() + "','" + lblID1.Text.ToString() + "','" + lblMainClientID1.Text.ToString() + "','" + lblClientID1.Text.ToString() + "'," + lblPaymentAmount1.Text.Replace(",", "").ToString() + ",'" + lblChequeNumber1.Text.ToString() + "','" + Convert.ToDateTime(dtpupdateinstrumentdate1.Value).ToString("yyyy-MM-dd") + "','" + Session["userid"].ToString() + "','" + oDBEngine.GetDate().ToString() + "'");


                }

                Session["Data"] = (DataTable)dtsave1;


                string ID1 = hdnID1.Value.ToString();
                string bank = hdnbank.Value.ToString();
                string FromDate = hdnFromdate.Value.ToString();
                string ToDate = hdnTodate.Value.ToString();
                //string strBank1 = hdnstrBank1.Value.ToString();
                string chequenumber1 = hdnChequeNumber1.Value.ToString();
                string BankType = hdnBankType.Value.ToString();
                string chkAccount = hdnAccount.Value.ToString();
                string chkUcc = hdnUcc.Value.ToString();

                this.Page.ClientScript.RegisterStartupScript(GetType(), "abcd12", "<script>OnMoreInfoClick1();</script>");

            }
        }

    }
}