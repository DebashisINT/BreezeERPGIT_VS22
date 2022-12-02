using System;
using System.Data;
namespace ERP.OMS.Management.DailyTask
{
    public partial class management_DailyTask_frmBankStatementIndividual : ERP.OMS.ViewState_class.VSPage
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        string aa = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            //Id='+obj1+'&DP_Client='+obj2+'&SettNumType='+obj3+'&ISIN='+obj4+'&Qty='+obj5;

            string ID = Request.QueryString["Id"].ToString();
            hdnID.Value = ID.ToString();
            string TransactionDate = Request.QueryString["TransactionDate"].ToString();
            hdnbankdate.Value = TransactionDate.ToString();
            string ValueDate = Request.QueryString["ValueDate"].ToString();
            hdnValueDate.Value = ValueDate.ToString();
            string InstrumentNumber = Request.QueryString["InstrumentNumber"].ToString();
            string Transactionamount = Request.QueryString["Transactionamount"].ToString();
            string Description = Request.QueryString["Description"].ToString();
            string RunningAmount = Request.QueryString["RunningAmount"].ToString();
            string Receipt = Request.QueryString["Receipt"].ToString();
            Label1.Text = ID + '~' + TransactionDate + '~' + ValueDate + '~' + InstrumentNumber + '~' + Transactionamount + '~' + Description + '~' + RunningAmount + '~' + Receipt;
            lblAmount.Text = Transactionamount;

            DataTable dtbl = null;
            if (Receipt == "Receipt")
            {
                dtbl = oDBEngine.GetDataTable("trans_cashbankdetail", "CASHBANKDETAIL_ID,CashBankDetail_InstrumentDate,CashBankDetail_MainAccountId,CashBankDetail_SubAccountId,CashBankDetail_PaymentAmount,CashBankDetail_ReceiptAmount,CashBankDetail_InstrumentNumber", "CashBankDetail_ReceiptAmount=" + Transactionamount + "");
            }
            else if (Receipt == "Payment")
            {
                dtbl = oDBEngine.GetDataTable("trans_cashbankdetail", "CASHBANKDETAIL_ID,CashBankDetail_InstrumentDate,CashBankDetail_MainAccountId,CashBankDetail_SubAccountId,CashBankDetail_PaymentAmount,CashBankDetail_ReceiptAmount,CashBankDetail_InstrumentNumber", "CashBankDetail_PaymentAmount=" + Transactionamount + "");
            }
            gridSummary.DataSource = dtbl;
            gridSummary.DataBind();
        }

        protected void gridSummary_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            string[] param = e.Parameters.Split('~');
            if (param[0].ToString() != "")
            {
                int i = oDBEngine.SetFieldValue("trans_cashbankdetail", "CashBankDetail_BankStatementDate=" + param[1].ToString() + "CashBankDetail_BankValueDate=" + param[2].ToString() + "", "CashBankDetail_ID=" + param[0].ToString() + "");
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct", "alert('Update Successfully')", true);
                //Page.ClientScript.RegisterStartupScript(GetType(), "PageScript", "<script language='javascript'>parent.gridrefresh(" + param[3].ToString() + ");</script>");
                aa = param[3].ToString();
                Session["aa"] = aa.ToString();
            }
        }

    }

}