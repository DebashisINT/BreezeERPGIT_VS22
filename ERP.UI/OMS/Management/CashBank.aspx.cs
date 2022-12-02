using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
//using DevExpress.Web;
using DevExpress.Web;
using BusinessLogicLayer;


namespace ERP.OMS.Management
{
    public partial class management_CashBank : System.Web.UI.Page
    {
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();

        Management_BL Management_BL = new Management_BL();

        //DBEngine oDBEngine = new DBEngine(string.Empty);
        static string CheckingCashBank = null;
        string ReturnValue = "1";
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
            dsCompany.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SelectSegment.ConnectionString=Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                dsBranch.ConnectionString=Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                MainAccount.ConnectionString=Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                SelectSubaccount.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            //------- For Read Only User in SQL Datasource Connection String   Start-----------------

            if (HttpContext.Current.Session["EntryProfileType"] != null)
            {
                if (Convert.ToString(HttpContext.Current.Session["EntryProfileType"]) == "R")
                {
                    //dsCashBank.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"];MULTI
                    //dsgrdClientbank.ConnectionString = ConfigurationSettings.AppSettings["DBReadOnlyConnection"];MULTI
                    dsCashBank.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                    dsgrdClientbank.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
                else
                {
                    //dsCashBank.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
                    //dsgrdClientbank.ConnectionString = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
                    dsCashBank.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                    dsgrdClientbank.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                }
            }

            //------- For Read Only User in SQL Datasource Connection String   End-----------------

            if (HttpContext.Current.Session["userid"] == null)
            {
               //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }

            if (!IsPostBack)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "JScript", "Page_Load()", true);
            }
            if (!Page.IsPostBack)
            {
                AssignQuery();
            }
            else
            {
                if (CheckingCashBank == "s")
                    FilterQuery();
                else
                    AssignQuery();
            }
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
        }
        protected void grdCashbank_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            grdCashbank.ClearSort();
            if (e.Parameters == "s")
            {
                CheckingCashBank = "s";
                FilterQuery();
            }
            if (e.Parameters == "All")
            {
                CheckingCashBank = "All";
                AssignQuery();
            }
        }
        protected void grdCashbank_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            string ID = e.Keys[0].ToString();
            string VNo = e.Values[2].ToString();
            string BranchID = e.Values[7].ToString();
            string SegmentID = e.Values[6].ToString();
            string TDate = e.Values[1].ToString();
            //oDBEngine.DeleteValue("trans_cashbankvouchers", " CashBank_ID=" + ID + "");
            //oDBEngine.DeleteValue("trans_cashbankdetail", " cashbankdetail_voucherid=" + ID + "");
            //oDBEngine.DeleteValue("trans_accountsledger", " accountsledger_TransactionReferenceID='" + VNo + "' and accountsledger_BranchID='" + BranchID + "' and accountsledger_ExchangeSegmentID='" + SegmentID + "' and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,accountsledger_TransactionDate)) as datetime)=cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + TDate + "')) as datetime)");
            DataSet DS = new DataSet();
            //String conn = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
            String conn = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlConnection con = new SqlConnection(conn);
            //SqlCommand cmd3 = new SqlCommand("CashBankDelete", con);
            //cmd3.CommandType = CommandType.StoredProcedure;
            //cmd3.Parameters.AddWithValue("@ID", ID);
            //cmd3.Parameters.AddWithValue("@VoucherNo", VNo);
            //cmd3.Parameters.AddWithValue("@BranchID", BranchID);
            //cmd3.Parameters.AddWithValue("@SegmentID", SegmentID);
            //cmd3.Parameters.AddWithValue("@TransactionDate", Convert.ToDateTime(TDate));
            //cmd3.CommandTimeout = 0; 
            //SqlDataAdapter Adap = new SqlDataAdapter();
            //Adap.SelectCommand = cmd3;
            //Adap.Fill(DS);

            DS = Management_BL.management_CashBankDelete(ID, VNo, BranchID, SegmentID, Convert.ToDateTime(TDate));

            ReturnValue = DS.Tables[0].Rows[0][0].ToString();
            // cmd3.Dispose();
            e.Cancel = true;
        }
        protected void grdCashbank_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpEND"] = ReturnValue;
        }
        public void FilterQuery()
        {
            string WhereClause = null;
            string WhereForAccNo = null;
            if (dtTrDate.Value != null && dtTrDate.Value != "1/1/0100 12:00:00 AM")
            {
                WhereClause += " and cashbank_TransactionDate ='" + dtTrDate.Value + "'";
            }
            if (txtRefNo.Text != "Ref. No.")
            {
                WhereClause += " and cashbank_vouchernumber like '" + txtRefNo.Text + "%'";
            }
            if (txtAcNo.Text != "A/C No")
            {
                WhereForAccNo += " where AccountNumber like '%" + txtAcNo.Text + "%'";
            }
            if (txtInstNo.Text != "Inst. Number")
            {
                WhereClause += " and CashBankDetail_InstrumentNumber like '" + txtInstNo.Text + "%'";
            }
            if (txtPayment.Text != "Payment")
            {
                WhereClause += " and cashbankdetail_paymentamount like '" + txtPayment.Text + "%'";
            }
            if (txtReceipt.Text != "Receipt")
            {
                WhereClause += " and cashbankdetail_receiptamount like '" + txtReceipt.Text + "%'";
            }

            if (HttpContext.Current.Session["userlastsegment"].ToString() == "5")
            {
                dsCashBank.SelectCommand = "select cast(CashBank_ID as varchar) as CashBank_ID ,cast(cashbank_vouchernumber as varchar) as VoucherNo,convert(varchar(11),cashbank_transactionDate,113) as Date,cashbank_TransactionDate,CashBank_ExchangeSegmentID,CashBank_BranchID,CashBank_CashBankID,(select MainAccount_Name+' ['+MainAccount_BankAcNumber+']' from Master_MainAccount where MainAccount_AccountCode=Trans_CashBankDetail.CashBankDetail_MainAccountID) as AccountNumber,cashbank_CreateDatetime,case when cashbankdetail_paymentamount=0 then null else (convert(varchar(50),cast(cashbankdetail_paymentamount as money),1)) end as PaymentAmount,case when cashbankdetail_receiptamount=0 then null else (convert(varchar(50),cast(cashbankdetail_receiptamount as money),1)) end as ReceiptAmount from trans_cashbankvouchers,trans_cashbankdetail where CashBank_TransactionType='C' and cashbank_id=cashbankdetail_voucherid and CashBank_ExchangeSegmentID in(0) " + WhereClause + " union all select distinct *,case when (select sum(CashBankDetail_PaymentAmount-CashBankDetail_ReceiptAmount)  from Trans_CashBankDetail where Voucher.CashBank_ID=CashBankDetail_VoucherID and Voucher.CashBank_CashBankID=CashBankDetail_CashBankID)<0 then null else (select convert(varchar(50),cast(sum(CashBankDetail_PaymentAmount-CashBankDetail_ReceiptAmount) as money),1) from Trans_CashBankDetail where Voucher.CashBank_ID=CashBankDetail_VoucherID and Voucher.CashBank_CashBankID=CashBankDetail_CashBankID) end  as PaymentAmount,case when (select sum(CashBankDetail_ReceiptAmount-CashBankDetail_PaymentAmount)  from Trans_CashBankDetail where Voucher.CashBank_ID=CashBankDetail_VoucherID and Voucher.CashBank_CashBankID=CashBankDetail_CashBankID)<0 then null else (select convert(varchar(50),cast(sum(CashBankDetail_ReceiptAmount-CashBankDetail_PaymentAmount) as money),1) from Trans_CashBankDetail,trans_cashbankdetail where Voucher.CashBank_ID=CashBankDetail_VoucherID and Voucher.CashBank_CashBankID=CashBankDetail_CashBankID) end as ReceiptAmount from (select CashBank_ID,CashBank_VoucherNumber as VoucherNo,convert(varchar(11),cashbank_TransactionDate,113) as Date,cashbank_TransactionDate,CashBank_ExchangeSegmentID,CashBank_BranchID,CashBank_CashBankID,(select MainAccount_Name+' ['+MainAccount_BankAcNumber+']' from Master_MainAccount where MainAccount_AccountCode=Trans_CashBankVouchers.CashBank_CashBankID) as AccountNumber,cashbank_CreateDatetime from Trans_CashBankVouchers where CashBank_TransactionType<>'C' and CashBank_ExchangeSegmentID in(0) and cashbank_id=cashbankdetail_voucherid " + WhereClause + ") as Voucher " + WhereForAccNo + " order by cashbank_CreateDatetime desc";
            }
            else
            {
                dsCashBank.SelectCommand = "select cast(CashBank_ID as varchar) as CashBank_ID ,cast(cashbank_vouchernumber as varchar) as VoucherNo,convert(varchar(11),cashbank_transactionDate,113) as Date,cashbank_TransactionDate,CashBank_ExchangeSegmentID,CashBank_BranchID,CashBank_CashBankID,(select MainAccount_Name+' ['+MainAccount_BankAcNumber+']' from Master_MainAccount where MainAccount_AccountCode=Trans_CashBankDetail.CashBankDetail_MainAccountID) as AccountNumber,cashbank_CreateDatetime,case when cashbankdetail_paymentamount=0 then null else (convert(varchar(50),cast(cashbankdetail_paymentamount as money),1)) end as PaymentAmount,case when cashbankdetail_receiptamount=0 then null else (convert(varchar(50),cast(cashbankdetail_receiptamount as money),1)) end as ReceiptAmount from trans_cashbankvouchers,trans_cashbankdetail where CashBank_TransactionType='C' and cashbank_id=cashbankdetail_voucherid and CashBank_ExchangeSegmentID in(select exch_internalId from (select exch_internalId,isnull(((select exh_shortname from tbl_master_exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId),exch_membershiptype) as Name from tbl_master_companyExchange) as D where Name in(select seg_name from tbl_master_segment where seg_id=" + HttpContext.Current.Session["userlastsegment"] + ")) and ltrim(rtrim(CashBank_FinYear))=ltrim(rtrim('" + HttpContext.Current.Session["LastFinYear"] + "')) and CashBank_CompanyID='" + HttpContext.Current.Session["LastCompany"].ToString() + "' " + WhereClause + "  union all select distinct *,case when (select sum(CashBankDetail_PaymentAmount-CashBankDetail_ReceiptAmount)  from Trans_CashBankDetail where Voucher.CashBank_ID=CashBankDetail_VoucherID and Voucher.CashBank_CashBankID=CashBankDetail_CashBankID)<0 then null else (select convert(varchar(50),cast(sum(CashBankDetail_PaymentAmount-CashBankDetail_ReceiptAmount) as money),1) from Trans_CashBankDetail where Voucher.CashBank_ID=CashBankDetail_VoucherID and Voucher.CashBank_CashBankID=CashBankDetail_CashBankID) end  as PaymentAmount,case when (select sum(CashBankDetail_ReceiptAmount-CashBankDetail_PaymentAmount)  from Trans_CashBankDetail where Voucher.CashBank_ID=CashBankDetail_VoucherID and Voucher.CashBank_CashBankID=CashBankDetail_CashBankID)<0 then null else (select convert(varchar(50),cast(sum(CashBankDetail_ReceiptAmount-CashBankDetail_PaymentAmount) as money),1) from Trans_CashBankDetail where Voucher.CashBank_ID=CashBankDetail_VoucherID and Voucher.CashBank_CashBankID=CashBankDetail_CashBankID) end as ReceiptAmount from (select CashBank_ID,CashBank_VoucherNumber as VoucherNo,convert(varchar(11),cashbank_TransactionDate,113) as Date,cashbank_TransactionDate,CashBank_ExchangeSegmentID,CashBank_BranchID,CashBank_CashBankID,(select MainAccount_Name+' ['+MainAccount_BankAcNumber+']' from Master_MainAccount where MainAccount_AccountCode=Trans_CashBankVouchers.CashBank_CashBankID) as AccountNumber,cashbank_CreateDatetime from Trans_CashBankVouchers,trans_cashbankdetail where CashBank_TransactionType<>'C' and CashBank_ExchangeSegmentID in(select exch_internalId from (select exch_internalId,isnull(((select exh_shortname from tbl_master_exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId),exch_membershiptype) as Name from tbl_master_companyExchange) as D where Name in(select seg_name from tbl_master_segment where seg_id=" + HttpContext.Current.Session["userlastsegment"] + ")) and ltrim(rtrim(CashBank_FinYear))=ltrim(rtrim('" + HttpContext.Current.Session["LastFinYear"] + "')) and CashBank_CompanyID='" + HttpContext.Current.Session["LastCompany"].ToString() + "' and cashbank_id=cashbankdetail_voucherid " + WhereClause + ") as Voucher " + WhereForAccNo + " order by cashbank_CreateDatetime desc";
            }
            try
            {
                grdCashbank.DataBind();
            }
            catch
            {
            }
            // dtTrDate.Value = Convert.ToDateTime("01-01-0100");
        }
        public void AssignQuery()
        {
            //if (HttpContext.Current.Session["userlastsegment"].ToString() == "5")
            //{
            //    //dsCashBank.SelectCommand = "select CashBank_ID,CashBank_VoucherNumber as VoucherNo,convert(varchar(11),cashbank_TransactionDate,113) as Date,(select (select exh_shortName from tbl_master_exchange where exh_cntId=tbl_master_companyexchange.exch_exchId)+'-'+exch_segmentId from tbl_master_companyexchange where exch_internalId=Trans_CashBankVouchers.CashBank_ExchangeSegmentID) as Segment,cashbank_TransactionDate,CashBank_ExchangeSegmentID,CashBank_BranchID from Trans_CashBankVouchers where CashBank_BranchID in(" + Session["userbranchHierarchy"].ToString() + ") and CashBank_ExchangeSegmentID in(0)  order by CashBank_CreateDateTime desc";
            //    dsCashBank.SelectCommand = "select cast(CashBank_ID as varchar) as CashBank_ID ,cast(cashbank_vouchernumber as varchar) as VoucherNo,convert(varchar(11),cashbank_transactionDate,113) as Date,cashbank_TransactionDate,CashBank_ExchangeSegmentID,CashBank_BranchID,CashBank_CashBankID,(select MainAccount_Name+' ['+MainAccount_BankAcNumber+']' from Master_MainAccount where MainAccount_AccountCode=Trans_CashBankDetail.CashBankDetail_MainAccountID) as AccountNumber,cashbank_CreateDatetime,case when cashbankdetail_paymentamount=0 then null else (convert(varchar(50),cast(cashbankdetail_paymentamount as money),1)) end as PaymentAmount,case when cashbankdetail_receiptamount=0 then null else (convert(varchar(50),cast(cashbankdetail_receiptamount as money),1)) end as ReceiptAmount from trans_cashbankvouchers,trans_cashbankdetail where CashBank_TransactionType='C' and cashbank_id=cashbankdetail_voucherid and CashBank_ExchangeSegmentID in(0) and CashBank_CreateUser=" + Session["userid"].ToString() + " union all select distinct *,case when (select sum(CashBankDetail_PaymentAmount-CashBankDetail_ReceiptAmount)  from Trans_CashBankDetail where Voucher.CashBank_ID=CashBankDetail_VoucherID and Voucher.CashBank_CashBankID=CashBankDetail_CashBankID)<0 then null else (select convert(varchar(50),cast(sum(CashBankDetail_PaymentAmount-CashBankDetail_ReceiptAmount) as money),1) from Trans_CashBankDetail where Voucher.CashBank_ID=CashBankDetail_VoucherID and Voucher.CashBank_CashBankID=CashBankDetail_CashBankID) end  as PaymentAmount,case when (select sum(CashBankDetail_ReceiptAmount-CashBankDetail_PaymentAmount)  from Trans_CashBankDetail where Voucher.CashBank_ID=CashBankDetail_VoucherID and Voucher.CashBank_CashBankID=CashBankDetail_CashBankID)<0 then null else (select convert(varchar(50),cast(sum(CashBankDetail_ReceiptAmount-CashBankDetail_PaymentAmount) as money),1) from Trans_CashBankDetail where Voucher.CashBank_ID=CashBankDetail_VoucherID and Voucher.CashBank_CashBankID=CashBankDetail_CashBankID) end as ReceiptAmount from (select CashBank_ID,CashBank_VoucherNumber as VoucherNo,convert(varchar(11),cashbank_TransactionDate,113) as Date,cashbank_TransactionDate,CashBank_ExchangeSegmentID,CashBank_BranchID,CashBank_CashBankID,(select MainAccount_Name+' ['+MainAccount_BankAcNumber+']' from Master_MainAccount where MainAccount_AccountCode=Trans_CashBankVouchers.CashBank_CashBankID) as AccountNumber,cashbank_CreateDatetime from Trans_CashBankVouchers where CashBank_TransactionType<>'C' and CashBank_ExchangeSegmentID in(0) and CashBank_CreateUser=" + Session["userid"].ToString() + ") as Voucher order by cashbank_CreateDatetime desc";
            //}
            //else
            //{
            //dsCashBank.SelectCommand = "select CashBank_ID,CashBank_VoucherNumber as VoucherNo,convert(varchar(11),cashbank_TransactionDate,113) as Date,(select (select exh_shortName from tbl_master_exchange where exh_cntId=tbl_master_companyexchange.exch_exchId)+'-'+exch_segmentId from tbl_master_companyexchange where exch_internalId=Trans_CashBankVouchers.CashBank_ExchangeSegmentID) as Segment,cashbank_TransactionDate,CashBank_ExchangeSegmentID,CashBank_BranchID from Trans_CashBankVouchers where CashBank_BranchID in(" + Session["userbranchHierarchy"].ToString() + ") and CashBank_ExchangeSegmentID in(select exch_internalId from (select exch_internalId,isnull(((select exh_shortname from tbl_master_exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId),exch_membershiptype) as Name from tbl_master_companyExchange) as D where Name in(select seg_name from tbl_master_segment where seg_id=" + HttpContext.Current.Session["userlastsegment"] + ")) and ltrim(rtrim(CashBank_FinYear))=ltrim(rtrim('" + HttpContext.Current.Session["LastFinYear"] + "')) and CashBank_CompanyID='" + HttpContext.Current.Session["LastCompany"].ToString() + "' order by CashBank_CreateDateTime desc";
            dsCashBank.SelectCommand = "select cast(CashBank_ID as varchar) as CashBank_ID ,cast(cashbank_vouchernumber as varchar) as VoucherNo,convert(varchar(11),cashbank_transactionDate,113) as Date,cashbank_TransactionDate,CashBank_ExchangeSegmentID,CashBank_BranchID,CashBank_CashBankID,(select MainAccount_Name+' ['+MainAccount_BankAcNumber+']' from Master_MainAccount where MainAccount_AccountCode=Trans_CashBankDetail.CashBankDetail_MainAccountID) as AccountNumber,cashbank_CreateDatetime,case when cashbankdetail_paymentamount=0 then null else (convert(varchar(50),cast(cashbankdetail_paymentamount as money),1)) end as PaymentAmount,case when cashbankdetail_receiptamount=0 then null else (convert(varchar(50),cast(cashbankdetail_receiptamount as money),1)) end as ReceiptAmount from trans_cashbankvouchers,trans_cashbankdetail where CashBank_TransactionType='C' and cashbank_id=cashbankdetail_voucherid and CashBank_ExchangeSegmentID in(select exch_internalId from (select exch_internalId,isnull(((select exh_shortname from tbl_master_exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId),exch_membershiptype) as Name from tbl_master_companyExchange) as D where Name in(select seg_name from tbl_master_segment where seg_id=" + HttpContext.Current.Session["userlastsegment"] + ")) and ltrim(rtrim(CashBank_FinYear))=ltrim(rtrim('" + HttpContext.Current.Session["LastFinYear"] + "')) and CashBank_CompanyID='" + HttpContext.Current.Session["LastCompany"].ToString() + "' and CashBank_CreateUser=" + Session["userid"].ToString() + "  union all select distinct *,case when (select sum(CashBankDetail_PaymentAmount-CashBankDetail_ReceiptAmount)  from Trans_CashBankDetail where Voucher.CashBank_ID=CashBankDetail_VoucherID and Voucher.CashBank_CashBankID=CashBankDetail_CashBankID)<0 then null else (select convert(varchar(50),cast(sum(CashBankDetail_PaymentAmount-CashBankDetail_ReceiptAmount) as money),1) from Trans_CashBankDetail where Voucher.CashBank_ID=CashBankDetail_VoucherID and Voucher.CashBank_CashBankID=CashBankDetail_CashBankID) end  as PaymentAmount,case when (select sum(CashBankDetail_ReceiptAmount-CashBankDetail_PaymentAmount)  from Trans_CashBankDetail where Voucher.CashBank_ID=CashBankDetail_VoucherID and Voucher.CashBank_CashBankID=CashBankDetail_CashBankID)<0 then null else (select convert(varchar(50),cast(sum(CashBankDetail_ReceiptAmount-CashBankDetail_PaymentAmount) as money),1) from Trans_CashBankDetail where Voucher.CashBank_ID=CashBankDetail_VoucherID and Voucher.CashBank_CashBankID=CashBankDetail_CashBankID) end as ReceiptAmount from (select CashBank_ID,CashBank_VoucherNumber as VoucherNo,convert(varchar(11),cashbank_TransactionDate,113) as Date,cashbank_TransactionDate,CashBank_ExchangeSegmentID,CashBank_BranchID,CashBank_CashBankID,(select MainAccount_Name+' ['+MainAccount_BankAcNumber+']' from Master_MainAccount where MainAccount_AccountCode=Trans_CashBankVouchers.CashBank_CashBankID) as AccountNumber,cashbank_CreateDatetime from Trans_CashBankVouchers where CashBank_TransactionType<>'C' and CashBank_ExchangeSegmentID in(select exch_internalId from (select exch_internalId,isnull(((select exh_shortname from tbl_master_exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId),exch_membershiptype) as Name from tbl_master_companyExchange) as D where Name in(select seg_name from tbl_master_segment where seg_id=" + HttpContext.Current.Session["userlastsegment"] + ")) and ltrim(rtrim(CashBank_FinYear))=ltrim(rtrim('" + HttpContext.Current.Session["LastFinYear"] + "')) and CashBank_CompanyID='" + HttpContext.Current.Session["LastCompany"].ToString() + "' and CashBank_CreateUser=" + Session["userid"].ToString() + " ) as Voucher order by cashbank_CreateDatetime desc";
            //}
            grdCashbank.DataBind();
        }
    }
}