using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Web;

namespace ERP.OMS.Reports
{
    public partial class Reports_rpt_BankStatement : System.Web.UI.Page
    {
        ExcelFile objExcel = new ExcelFile();
        DataTable DT = new DataTable();
        DataTable DT1 = new DataTable();
        DataTable DTExport = new DataTable();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.Converter ObjConvert = new BusinessLogicLayer.Converter();
        string CheckStatus = "";
        decimal PaymentAmount = 0;
        decimal ReceiptAmount = 0;
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
            if (!IsPostBack)
            {
                SetDatteFinYear();
                DateTime FromDateForSplit = new DateTime();
                FromDate.EditFormatString = ObjConvert.GetDateFormat("Date");
                //FromDateForSplit = Convert.ToDateTime(oDBEngine.GetDate().ToString()).AddMonths(-1);
                //string mm = FromDateForSplit.Month.ToString();
                //string yy = FromDateForSplit.Year.ToString();
                // FromDate.Value = Convert.ToDateTime(mm + "-" + "01" + "-" + yy);
                DateTo.EditFormatString = ObjConvert.GetDateFormat("Date");
                // DateTo.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
                // string FinYear = Session["LastFinYear"].ToString();
                // string[] YearRange = FinYear.Split('-');
                // DateTime LastDateOfFiYear = Convert.ToDateTime("03-31-" + YearRange[1].ToString());
                //int returnValue = 0;
                //returnValue = DateTime.Compare(Convert.ToDateTime(LastDateOfFiYear),Convert.ToDateTime(DateTo.Value));
                //if (returnValue < 0)
                //{
                //    DateTo.Value = Convert.ToDateTime(LastDateOfFiYear);
                //    FromDate.Value = Convert.ToDateTime("03-01-" + YearRange[1].ToString());
                //}
                //DateTo.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
                //FromDate.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());

                BindTable();
                Page.ClientScript.RegisterStartupScript(GetType(), "ForSearch", "<script language='Javascript'>SearchVisible('N');</script>");

            }
            //Page.ClientScript.RegisterStartupScript(GetType(), "HeightCalling", "<script language='Javascript'>height();</script>");


        }
        protected void SetDatteFinYear()
        {
            DataTable dtFinYear = oDBEngine.GetDataTable("MASTER_FINYEAR ", "FINYEAR_STARTDATE, FINYEAR_ENDDATE ", " FINYEAR_CODE ='" + Session["LastFinYear"].ToString() + "'");
            DateTime StartDate = Convert.ToDateTime(dtFinYear.Rows[0]["FINYEAR_STARTDATE"].ToString());
            DateTime EndDate = Convert.ToDateTime(dtFinYear.Rows[0]["FINYEAR_ENDDATE"].ToString());
            DateTime TodayDate = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
            if (EndDate < TodayDate)
            {
                FromDate.Value = StartDate;
                DateTo.Value = EndDate;
            }
            else
            {
                FromDate.Value = StartDate;
                DateTo.Value = TodayDate;
            }

        }
        public void BindTable()
        {
            if (RdAll.Checked == true)
            {
                CheckStatus = "A";
            }
            else if (RdUnCleared.Checked == true)
            {
                CheckStatus = "U";
            }
            else if (RdCleared.Checked == true)
            {
                CheckStatus = "C";
            }
            if (CheckStatus == "U")
            {
                //DT = oDBEngine.GetDataTable("Trans_CashBankVouchers  as a inner join Trans_CashBankDetail as b on a.cashbank_id=b.cashbankdetail_voucherid inner join master_mainaccount as c on b.cashbankdetail_mainaccountid=c.MAinaccount_accountcode inner join master_subaccount as d on b.cashbankdetail_subAccountid=d.subaccount_code", "distinct a.cashbank_transactionDate as cashbank_transactionDate_test,(convert(varchar(11),a.cashbank_transactionDate,113))as cashbank_transactionDate,a.cashbank_vouchernumber,(convert(varchar(11),b.cashbankdetail_instrumentdate,113))as cashbankdetail_instrumentdate,b.cashbankdetail_instrumentdate as cashbankdetail_instrumentdate_test,b.cashbankdetail_instrumentnumber,c.mainaccount_name +' ['+isnull(d.subaccount_name,'')+'['+ (isnull((select isnull(ltrim(rtrim(cnt_ucc)),'') from tbl_master_contact where cnt_internalid=d.subaccount_code),(select top 1 subaccount_code from master_subaccount where cast(subaccount_code as varchar)=d.subaccount_code)))+']' +']' as AccountCode,case b.cashbankdetail_paymentamount when 0 then null else cast(b.cashbankdetail_paymentamount as money) end as cashbankdetail_paymentamount,case b.cashbankdetail_receiptamount when 0 then null else cast(b.cashbankdetail_receiptamount as money) end as cashbankdetail_receiptamount,case b.cashbankdetail_bankvaluedate when '1900-01-01 00:00:00.000' then null else convert(varchar(6),cashbankdetail_bankvaluedate,105)+substring(convert(varchar(10),cashbankdetail_bankvaluedate,101),7,4)end cashbankdetail_bankvaluedate,b.cashbankdetail_id,d.subaccount_code,b.cashbankdetail_mainaccountid,d.subaccount_name ", " a.cashbank_transactionDate>='" + FromDate.Value + "' and a.cashbank_transactionDate<='" + DateTo.Value + "' and (b.cashbankdetail_bankvaluedate is null or b.cashbankdetail_bankvaluedate ='1900-01-01 00:00:00.000') and (b.cashbankdetail_cashbankid='" + txtBankName_hidden.Value.ToString() + "' or b.cashbankdetail_mainaccountid='" + txtBankName_hidden.Value.ToString() + "')", "cashbank_transactionDate_test");
                DT = oDBEngine.GetDataTable("(Select distinct a.cashbank_transactionDate as cashbank_transactionDate_test,(convert(varchar(11),a.cashbank_transactionDate,113))as cashbank_transactionDate,a.cashbank_vouchernumber,(convert(varchar(11),b.cashbankdetail_instrumentdate,113))as cashbankdetail_instrumentdate,b.cashbankdetail_instrumentdate as cashbankdetail_instrumentdate_test,b.cashbankdetail_instrumentnumber,c.mainaccount_name +' ['+isnull((isnull(rtrim(ltrim(d.subaccount_name)),'')+'['+ (isnull((select isnull(ltrim(rtrim(cnt_ucc)),'') from tbl_master_contact where cnt_internalid=d.subaccount_code),(select top 1 subaccount_code from master_subaccount where cast(subaccount_code as varchar)=d.subaccount_code and subaccount_mainacreferenceid=b.cashbankdetail_mainaccountid)))+']'),'') +']' as AccountCode,case b.cashbankdetail_paymentamount when 0 then null else cast(b.cashbankdetail_paymentamount as money) end as cashbankdetail_paymentamount,case b.cashbankdetail_receiptamount when 0 then null else cast(b.cashbankdetail_receiptamount as money) end as cashbankdetail_receiptamount,case b.cashbankdetail_bankvaluedate when '1900-01-01 00:00:00.000' then null else convert(varchar(6),cashbankdetail_bankvaluedate,105)+substring(convert(varchar(10),cashbankdetail_bankvaluedate,101),7,4)end cashbankdetail_bankvaluedate,b.cashbankdetail_id,cashbankdetail_voucherid ,d.subaccount_code,b.cashbankdetail_mainaccountid,case when cashbankdetail_cashbankid is null then (select e.cashbankdetail_mainaccountid from trans_cashbankdetail as e where e.cashbankdetail_mainaccountid <>'" + txtBankName_hidden.Value.ToString() + "' and e.cashbankdetail_voucherid=b.cashbankdetail_voucherid and cashbankdetail_cashbankid is null) else b.cashbankdetail_mainaccountid end as test,d.subaccount_name  from Trans_CashBankVouchers  as a inner join Trans_CashBankDetail as b on a.cashbank_id=b.cashbankdetail_voucherid inner join master_mainaccount  as c on b.cashbankdetail_mainaccountid=c.MAinaccount_accountcode inner join master_subaccount as d  on b.cashbankdetail_subAccountid=d.subaccount_code WHERE  a.cashbank_transactionDate>='" + Convert.ToDateTime(FromDate.Value).ToShortDateString() + "'  and a.cashbank_transactionDate<='" + Convert.ToDateTime(DateTo.Value).ToShortDateString() + "' and (b.cashbankdetail_cashbankid='" + txtBankName_hidden.Value.ToString() + "' or b.cashbankdetail_mainaccountid='" + txtBankName_hidden.Value.ToString() + "')and(b.cashbankdetail_bankvaluedate is  null  or b.cashbankdetail_bankvaluedate ='1900-01-01 00:00:00.000')AND (subaccount_mainacreferenceid=b.cashbankdetail_mainaccountid or cashbankdetail_Subaccountid='' or cashbankdetail_Subaccountid is null))as d", "d.cashbank_transactionDate_test,d.cashbank_transactionDate,d.cashbank_vouchernumber,d.cashbankdetail_instrumentdate,d.cashbankdetail_instrumentdate_test,d.cashbankdetail_instrumentnumber,case substring(d.cashbank_vouchernumber,1,1) when 'C' then (select mainaccount_name from master_mainaccount where mainaccount_accountcode= d.test)  else d.AccountCode end as AccountCode,d.cashbankdetail_paymentamount,d.cashbankdetail_receiptamount,d.cashbankdetail_bankvaluedate,d.cashbankdetail_id,d.cashbankdetail_voucherid ,d.subaccount_code,d.subaccount_name,cashbankdetail_mainaccountid", null, "cashbank_transactionDate_test,cashbankdetail_instrumentnumber");
                DTExport = oDBEngine.GetDataTable("(Select distinct case a.cashbankdetail_instrumenttype when 'E' then 'E-Payment' when 'C' then 'Cheque' else 'Not applicable' end as type1,a.cashbank_transactionDate as cashbank_transactionDate_test,(convert(varchar(11),a.cashbank_transactionDate,113))as cashbank_transactionDate,a.cashbank_vouchernumber,(convert(varchar(11),a.cashbankdetail_instrumentdate,113))as cashbankdetail_instrumentdate,b.cashbankdetail_instrumentdate as cashbankdetail_instrumentdate_test,a.cashbankdetail_instrumentnumber,c.mainaccount_name +' ['+isnull((isnull(rtrim(ltrim(d.subaccount_name)),'')+'['+ (isnull((select isnull(ltrim(rtrim(cnt_ucc)),'') from tbl_master_contact where cnt_internalid=d.subaccount_code),(select top 1 subaccount_code from master_subaccount where cast(subaccount_code as varchar)=d.subaccount_code and subaccount_mainacreferenceid=b.cashbankdetail_mainaccountid)))+']'),'') +']' as AccountCode,case b.cashbankdetail_paymentamount when 0 then null else cast(b.cashbankdetail_paymentamount as money) end as cashbankdetail_paymentamount,case b.cashbankdetail_receiptamount when 0 then null else cast(b.cashbankdetail_receiptamount as money) end as cashbankdetail_receiptamount,case b.cashbankdetail_bankvaluedate when '1900-01-01 00:00:00.000' then null else convert(varchar(6),cashbankdetail_bankvaluedate,105)+substring(convert(varchar(10),cashbankdetail_bankvaluedate,101),7,4)end cashbankdetail_bankvaluedate,(convert(varchar(11),b.cashbankdetail_bankstatementdate,113))as bankstatementdate,b.cashbankdetail_id,cashbankdetail_voucherid ,d.subaccount_code,b.cashbankdetail_mainaccountid,case when cashbankdetail_cashbankid is null then (select e.cashbankdetail_mainaccountid from trans_cashbankdetail as e where e.cashbankdetail_mainaccountid <>'" + txtBankName_hidden.Value.ToString() + "' and e.cashbankdetail_voucherid=b.cashbankdetail_voucherid and cashbankdetail_cashbankid is null) else b.cashbankdetail_mainaccountid end as test,d.subaccount_name  from Trans_CashBankVouchers  as a inner join Trans_CashBankDetail as b on a.cashbank_id=b.cashbankdetail_voucherid inner join master_mainaccount  as c on b.cashbankdetail_mainaccountid=c.MAinaccount_accountcode inner join master_subaccount as d  on b.cashbankdetail_subAccountid=d.subaccount_code WHERE  a.cashbank_transactionDate>='" + Convert.ToDateTime(FromDate.Value).ToShortDateString() + "'  and a.cashbank_transactionDate<='" + Convert.ToDateTime(DateTo.Value).ToShortDateString() + "' and (b.cashbankdetail_cashbankid='" + txtBankName_hidden.Value.ToString() + "' or b.cashbankdetail_mainaccountid='" + txtBankName_hidden.Value.ToString() + "')and(b.cashbankdetail_bankvaluedate is  null  or b.cashbankdetail_bankvaluedate ='1900-01-01 00:00:00.000')AND (subaccount_mainacreferenceid=b.cashbankdetail_mainaccountid or cashbankdetail_Subaccountid='' or cashbankdetail_Subaccountid is null))as d", "d.cashbank_transactionDate as [Trans Date],d.cashbank_vouchernumber as [Voucher No],d.cashbankdetail_instrumentdate as [Ins Date],d.cashbankdetail_instrumentnumber as [Ins No],d.type1 as Type,case substring(d.cashbank_vouchernumber,1,1) when 'C' then (select mainaccount_name from master_mainaccount where mainaccount_accountcode= d.test)  else d.AccountCode end as AccountCode,d.cashbankdetail_paymentamount as [Pmt Amt],d.cashbankdetail_receiptamount as [Rcpt Amt],d.cashbankdetail_bankvaluedate as [Value Date],d.bankstatementdate as [Statement Date]", null, "cashbank_transactionDate_test,cashbankdetail_instrumentnumber");
            }
            else if (CheckStatus == "C")
            {
                //DT = oDBEngine.GetDataTable("Trans_CashBankVouchers  as a inner join Trans_CashBankDetail as b on a.cashbank_id=b.cashbankdetail_voucherid inner join master_mainaccount as c on b.cashbankdetail_mainaccountid=c.MAinaccount_accountcode inner join master_subaccount as d on b.cashbankdetail_subAccountid=d.subaccount_code", "distinct a.cashbank_transactionDate as cashbank_transactionDate_test,(convert(varchar(11),a.cashbank_transactionDate,113))as cashbank_transactionDate,a.cashbank_vouchernumber,(convert(varchar(11),b.cashbankdetail_instrumentdate,113))as cashbankdetail_instrumentdate,b.cashbankdetail_instrumentdate as cashbankdetail_instrumentdate_test,b.cashbankdetail_instrumentnumber,c.mainaccount_name +' ['+isnull(d.subaccount_name,'')+'['+ (isnull((select isnull(ltrim(rtrim(cnt_ucc)),'') from tbl_master_contact where cnt_internalid=d.subaccount_code),(select top 1 subaccount_code from master_subaccount where cast(subaccount_code as varchar)=d.subaccount_code)))+']' +']' as AccountCode,case b.cashbankdetail_paymentamount when 0 then null else cast(b.cashbankdetail_paymentamount as money) end as cashbankdetail_paymentamount,case b.cashbankdetail_receiptamount when 0 then null else cast(b.cashbankdetail_receiptamount as money) end as cashbankdetail_receiptamount,case b.cashbankdetail_bankvaluedate when '1900-01-01 00:00:00.000' then null else convert(varchar(6),cashbankdetail_bankvaluedate,105)+substring(convert(varchar(10),cashbankdetail_bankvaluedate,101),7,4)end cashbankdetail_bankvaluedate,b.cashbankdetail_id,d.subaccount_code,b.cashbankdetail_mainaccountid,d.subaccount_name ", " a.cashbank_transactionDate>='" + FromDate.Value + "' and a.cashbank_transactionDate<='" + DateTo.Value + "' and (b.cashbankdetail_bankvaluedate is not null  and b.cashbankdetail_bankvaluedate <>'1900-01-01 00:00:00.000') and (b.cashbankdetail_cashbankid='" + txtBankName_hidden.Value.ToString() + "' or b.cashbankdetail_mainaccountid='" + txtBankName_hidden.Value.ToString() + "')", "cashbank_transactionDate_test");
                if (ddlConsider.SelectedItem.Value == "VD")
                {
                    string strSelectQuery = @"cashbank_transactionDate_test,cashbank_transactionDate,cashbank_vouchernumber,cashbankdetail_instrumentdate,
                cashbankdetail_instrumentdate_test,cashbankdetail_instrumentnumber,
                Case
                When CashBank_TransactionType='C'
                Then (Select (Select Ltrim(Rtrim(MainAccount_Name)) from Master_MainAccount Where MainACcount_AccountCode=CashBankDetail_MainAccountID) from Trans_CashbankDetail Where CashBankDetail_IBRef=T1.CashBankDetail_IBRef
                and CashBankDetail_MainAccountID<>T1.CashBankDetail_MainAccountID)
                Else OwnAccountCode
                End as AccountCode,cashbankdetail_paymentamount,cashbankdetail_receiptamount,cashbankdetail_bankvaluedate,
                bankstatementdate,cashbankdetail_id,subaccount_code,subaccount_name,cashbankdetail_mainAccountid,type1";

                    ///// Old Code//////////
//                    string strFromQuery = @"(Select Convert(Varchar,CashBank_TransactionDate,106) as cashbank_transactionDate_test,
//                Convert(Varchar,cashbank_transactionDate,106) as cashbank_transactionDate,cashbank_vouchernumber,
//                Convert(Varchar,cashbankdetail_instrumentdate,106) as cashbankdetail_instrumentdate,
//                Convert(Varchar,cashbankdetail_instrumentdate,106) as cashbankdetail_instrumentdate_test,
//                cashbankdetail_instrumentnumber,
//                Case
//                When (Select Upper(MainAccount_SubLedgerType) from Master_MainAccount Where MainACcount_AccountCode=CashBankDetail_MainAccountID)<>'NONE'
//                Then (Select 
//                (Select Ltrim(Rtrim(MainAccount_Name)) from Master_MainAccount Where MainACcount_AccountCode=CashBankDetail_MainAccountID)+
//                (Select Top 1 '[ '+Ltrim(Rtrim(SubAccount_Name))+' ]' from Master_SubAccount Where SubAccount_MainAcReferenceID=CashBankDetail_MainAccountID 
//                and SubAccount_Code=CashBankDetail_SubAccountID))
//                Else (Select Ltrim(Rtrim(MainAccount_Name)) from Master_MainAccount Where MainACcount_AccountCode=CashBankDetail_MainAccountID)
//                End as OwnAccountCode,cashbankdetail_paymentamount,cashbankdetail_receiptamount,Convert(Varchar,cashbankdetail_bankvaluedate,106) as cashbankdetail_bankvaluedate,
//                cashbankdetail_id,
//                CashBankDetail_SubAccountID as subaccount_code,(Select Top 1 isnull(Ltrim(Rtrim(SubAccount_Name)),'') from Master_SubAccount Where SubAccount_MainAcReferenceID=CashBankDetail_MainAccountID 
//                and SubAccount_Code=CashBankDetail_SubAccountID) as subaccount_name,cashbankdetail_mainAccountid,Convert(Varchar,CashBankDetail_BankStatementDAte,106) as bankstatementdate,
//                Case 
//                When CashBankDetail_InstrumentType='C' Then 'Cheque'
//                When CashBankDetail_InstrumentType='D' Then 'Draft'
//                When CashBankDetail_InstrumentType='E' Then 'E-Transfer'
//                When CashBankDetail_InstrumentType='0' Then 'Cash'
//                End as type1,CashBank_TransactionType,CashBankDetail_IBRef  from Trans_CashBankVouchers,Trans_CashBankDetail WHERE CashBank_IBRef=CashBankDetail_IBRef
//                and (isnull(CashBankDetail_BankValueDate,'')<>'')
//                and (CashBankDetail_BankValueDate Between '" + Convert.ToDateTime(FromDate.Value).ToShortDateString() + "' and '" + Convert.ToDateTime(DateTo.Value).ToShortDateString() + @"')
//                and (CashBankDetail_MainAccountID='" + txtBankName_hidden.Value.ToString() + "' Or CashBankDetail_CashBankID='" + txtBankName_hidden.Value.ToString() + "')) as T1";
///////////////////////////


                    ///////////////New Code////////////
                    string strFromQuery = @"(Select Convert(Varchar,CashBank_TransactionDate,106) as cashbank_transactionDate_test,
                Convert(Varchar,cashbank_transactionDate,106) as cashbank_transactionDate,cashbank_vouchernumber,
                Convert(Varchar,cashbankdetail_instrumentdate,106) as cashbankdetail_instrumentdate,
                Convert(Varchar,cashbankdetail_instrumentdate,106) as cashbankdetail_instrumentdate_test,
                cashbankdetail_instrumentnumber,
                Case
                When (Select Upper(MainAccount_SubLedgerType) from Master_MainAccount Where MainACcount_AccountCode=CashBankDetail_MainAccountID)<>'NONE'
                Then (Select 
                (Select Ltrim(Rtrim(MainAccount_Name)) from Master_MainAccount Where MainACcount_AccountCode=CashBankDetail_MainAccountID)+
                (Select Top 1 '[ '+Ltrim(Rtrim(SubAccount_Name))+' ]' from Master_SubAccount Where SubAccount_MainAcReferenceID=CashBankDetail_MainAccountID 
                and SubAccount_Code=CashBankDetail_SubAccountID))
                Else (Select Ltrim(Rtrim(MainAccount_Name)) from Master_MainAccount Where MainACcount_AccountCode=CashBankDetail_MainAccountID)
                End as OwnAccountCode,cashbankdetail_paymentamount,cashbankdetail_receiptamount,Convert(Varchar,cashbankdetail_bankvaluedate,106) as cashbankdetail_bankvaluedate,
                cashbankdetail_id,
                CashBankDetail_SubAccountID as subaccount_code,(Select Top 1 isnull(Ltrim(Rtrim(SubAccount_Name)),'') from Master_SubAccount Where SubAccount_MainAcReferenceID=CashBankDetail_MainAccountID 
                and SubAccount_Code=CashBankDetail_SubAccountID) as subaccount_name,cashbankdetail_mainAccountid,Convert(Varchar,CashBankDetail_BankStatementDAte,106) as bankstatementdate,
                Case 
                When CashBankDetail_InstrumentType='C' Then 'Cheque'
                When CashBankDetail_InstrumentType='D' Then 'Draft'
                When CashBankDetail_InstrumentType='E' Then 'E-Transfer'
                When CashBankDetail_InstrumentType='0' Then 'Cash'
                End as type1,CashBank_TransactionType,CashBankDetail_IBRef  from Trans_CashBankVouchers,Trans_CashBankDetail WHERE CashBank_IBRef=CashBankDetail_IBRef
                and (isnull(CashBankDetail_BankValueDate,'')<>'')
                and (CashBankDetail_BankValueDate Between '" + Convert.ToDateTime(FromDate.Value).ToString("MM-dd-yyyy") + "' and '" + Convert.ToDateTime(DateTo.Value).ToString("MM-dd-yyyy") + @"')
                and (CashBankDetail_MainAccountID='" + txtBankName_hidden.Value.ToString() + "' Or CashBankDetail_CashBankID='" + txtBankName_hidden.Value.ToString() + "')) as T1";
                    ///////////////////////

                    DT = oDBEngine.GetDataTable(strFromQuery, strSelectQuery, null);
                    DTExport = DT;
                }
                else
                {
                    string strSelectQuery = @"cashbank_transactionDate_test,cashbank_transactionDate,cashbank_vouchernumber,cashbankdetail_instrumentdate,
                cashbankdetail_instrumentdate_test,cashbankdetail_instrumentnumber,
                Case
                When CashBank_TransactionType='C'
                Then (Select (Select Ltrim(Rtrim(MainAccount_Name)) from Master_MainAccount Where MainACcount_AccountCode=CashBankDetail_MainAccountID) from Trans_CashbankDetail Where CashBankDetail_IBRef=T1.CashBankDetail_IBRef
                and CashBankDetail_MainAccountID<>T1.CashBankDetail_MainAccountID)
                Else OwnAccountCode
                End as AccountCode,cashbankdetail_paymentamount,cashbankdetail_receiptamount,cashbankdetail_bankvaluedate,
                bankstatementdate,cashbankdetail_id,subaccount_code,subaccount_name,cashbankdetail_mainAccountid,type1";

                    string strFromQuery = @"(Select Convert(Varchar,CashBank_TransactionDate,106) as cashbank_transactionDate_test,
                Convert(Varchar,cashbank_transactionDate,106) as cashbank_transactionDate,cashbank_vouchernumber,
                Convert(Varchar,cashbankdetail_instrumentdate,106) as cashbankdetail_instrumentdate,
                Convert(Varchar,cashbankdetail_instrumentdate,106) as cashbankdetail_instrumentdate_test,
                cashbankdetail_instrumentnumber,
                Case
                When (Select Upper(MainAccount_SubLedgerType) from Master_MainAccount Where MainACcount_AccountCode=CashBankDetail_MainAccountID)<>'NONE'
                Then (Select 
                (Select Ltrim(Rtrim(MainAccount_Name)) from Master_MainAccount Where MainACcount_AccountCode=CashBankDetail_MainAccountID)+
                (Select Top 1 '[ '+Ltrim(Rtrim(SubAccount_Name))+' ]' from Master_SubAccount Where SubAccount_MainAcReferenceID=CashBankDetail_MainAccountID 
                and SubAccount_Code=CashBankDetail_SubAccountID))
                Else (Select Ltrim(Rtrim(MainAccount_Name)) from Master_MainAccount Where MainACcount_AccountCode=CashBankDetail_MainAccountID)
                End as OwnAccountCode,cashbankdetail_paymentamount,cashbankdetail_receiptamount,Convert(Varchar,cashbankdetail_bankvaluedate,106) as cashbankdetail_bankvaluedate,
                cashbankdetail_id,
                CashBankDetail_SubAccountID as subaccount_code,(Select Top 1 isnull(Ltrim(Rtrim(SubAccount_Name)),'') from Master_SubAccount Where SubAccount_MainAcReferenceID=CashBankDetail_MainAccountID 
                and SubAccount_Code=CashBankDetail_SubAccountID) as subaccount_name,cashbankdetail_mainAccountid,Convert(Varchar,CashBankDetail_BankStatementDAte,106) as bankstatementdate,
                Case 
                When CashBankDetail_InstrumentType='C' Then 'Cheque'
                When CashBankDetail_InstrumentType='D' Then 'Draft'
                When CashBankDetail_InstrumentType='E' Then 'E-Transfer'
                When CashBankDetail_InstrumentType='0' Then 'Cash'
                End as type1,CashBank_TransactionType,CashBankDetail_IBRef  from Trans_CashBankVouchers,Trans_CashBankDetail WHERE CashBank_IBRef=CashBankDetail_IBRef
                and (isnull(CashBankDetail_BankStatementDate,'')<>'')
                and (CashBankDetail_BankStatementDate Between '" + Convert.ToDateTime(FromDate.Value).ToShortDateString() + "' and '" + Convert.ToDateTime(DateTo.Value).ToShortDateString() + @"')
                and (CashBankDetail_MainAccountID='" + txtBankName_hidden.Value.ToString() + "' Or CashBankDetail_CashBankID='" + txtBankName_hidden.Value.ToString() + "')) as T1";
                    DT = oDBEngine.GetDataTable(strFromQuery, strSelectQuery, null);
                    DTExport = DT;
                }

            }
            else if (CheckStatus == "A")
            {
                DT = oDBEngine.GetDataTable("(Select distinct a.cashbank_transactionDate as cashbank_transactionDate_test,(convert(varchar(11),a.cashbank_transactionDate,113))as cashbank_transactionDate,a.cashbank_vouchernumber,(convert(varchar(11),b.cashbankdetail_instrumentdate,113))as cashbankdetail_instrumentdate,b.cashbankdetail_instrumentdate as cashbankdetail_instrumentdate_test,b.cashbankdetail_instrumentnumber,c.mainaccount_name +' ['+isnull((isnull(rtrim(ltrim(d.subaccount_name)),'')+'['+ (isnull((select isnull(ltrim(rtrim(cnt_ucc)),'') from tbl_master_contact where cnt_internalid=d.subaccount_code),(select top 1 subaccount_code from master_subaccount where cast(subaccount_code as varchar)=d.subaccount_code and subaccount_mainacreferenceid=b.cashbankdetail_mainaccountid)))+']'),'') +']' as AccountCode,case b.cashbankdetail_paymentamount when 0 then null else cast(b.cashbankdetail_paymentamount as money) end as cashbankdetail_paymentamount,case b.cashbankdetail_receiptamount when 0 then null else cast(b.cashbankdetail_receiptamount as money) end as cashbankdetail_receiptamount,case b.cashbankdetail_bankvaluedate when '1900-01-01 00:00:00.000' then null else convert(varchar(6),cashbankdetail_bankvaluedate,105)+substring(convert(varchar(10),cashbankdetail_bankvaluedate,101),7,4)end cashbankdetail_bankvaluedate,b.cashbankdetail_id,cashbankdetail_voucherid ,d.subaccount_code,b.cashbankdetail_mainaccountid,case when cashbankdetail_cashbankid is null then (select e.cashbankdetail_mainaccountid from trans_cashbankdetail as e where e.cashbankdetail_mainaccountid <>'" + txtBankName_hidden.Value.ToString() + "' and e.cashbankdetail_voucherid=b.cashbankdetail_voucherid and cashbankdetail_cashbankid is null) else b.cashbankdetail_mainaccountid end as test,d.subaccount_name  from Trans_CashBankVouchers  as a inner join Trans_CashBankDetail as b on a.cashbank_id=b.cashbankdetail_voucherid inner join master_mainaccount  as c on b.cashbankdetail_mainaccountid=c.MAinaccount_accountcode inner join master_subaccount as d  on b.cashbankdetail_subAccountid=d.subaccount_code WHERE  a.cashbank_transactionDate>='" + Convert.ToDateTime(FromDate.Value).ToShortDateString() + "'  and a.cashbank_transactionDate<='" + Convert.ToDateTime(DateTo.Value).ToShortDateString() + "' and (b.cashbankdetail_cashbankid='" + txtBankName_hidden.Value.ToString() + "' or b.cashbankdetail_mainaccountid='" + txtBankName_hidden.Value.ToString() + "')AND (subaccount_mainacreferenceid=b.cashbankdetail_mainaccountid or cashbankdetail_Subaccountid='' or cashbankdetail_Subaccountid is null))as d", "d.cashbank_transactionDate_test,d.cashbank_transactionDate,d.cashbank_vouchernumber,d.cashbankdetail_instrumentdate,d.cashbankdetail_instrumentdate_test,d.cashbankdetail_instrumentnumber,case substring(d.cashbank_vouchernumber,1,1) when 'C' then (select mainaccount_name from master_mainaccount where mainaccount_accountcode= d.test)  else d.AccountCode end as AccountCode,d.cashbankdetail_paymentamount,d.cashbankdetail_receiptamount,d.cashbankdetail_bankvaluedate,d.cashbankdetail_id,d.cashbankdetail_voucherid ,d.subaccount_code,d.subaccount_name,cashbankdetail_mainaccountid", null, "cashbank_transactionDate_test,cashbankdetail_instrumentnumber");
                DTExport = oDBEngine.GetDataTable("(Select distinct a.cashbank_transactionDate as cashbank_transactionDate_test,(convert(varchar(11),a.cashbank_transactionDate,113))as cashbank_transactionDate,a.cashbank_vouchernumber,(convert(varchar(11),b.cashbankdetail_instrumentdate,113))as cashbankdetail_instrumentdate,b.cashbankdetail_instrumentdate as cashbankdetail_instrumentdate_test,b.cashbankdetail_instrumentnumber,c.mainaccount_name +' ['+isnull((isnull(rtrim(ltrim(d.subaccount_name)),'')+'['+ (isnull((select isnull(ltrim(rtrim(cnt_ucc)),'') from tbl_master_contact where cnt_internalid=d.subaccount_code),(select top 1 subaccount_code from master_subaccount where cast(subaccount_code as varchar)=d.subaccount_code and subaccount_mainacreferenceid=b.cashbankdetail_mainaccountid)))+']'),'') +']' as AccountCode,case b.cashbankdetail_paymentamount when 0 then null else cast(b.cashbankdetail_paymentamount as money) end as cashbankdetail_paymentamount,case b.cashbankdetail_receiptamount when 0 then null else cast(b.cashbankdetail_receiptamount as money) end as cashbankdetail_receiptamount,case b.cashbankdetail_bankvaluedate when '1900-01-01 00:00:00.000' then null else convert(varchar(6),cashbankdetail_bankvaluedate,105)+substring(convert(varchar(10),cashbankdetail_bankvaluedate,101),7,4)end cashbankdetail_bankvaluedate,b.cashbankdetail_id,cashbankdetail_voucherid ,d.subaccount_code,b.cashbankdetail_mainaccountid,case when cashbankdetail_cashbankid is null then (select e.cashbankdetail_mainaccountid from trans_cashbankdetail as e where e.cashbankdetail_mainaccountid <>'" + txtBankName_hidden.Value.ToString() + "' and e.cashbankdetail_voucherid=b.cashbankdetail_voucherid and cashbankdetail_cashbankid is null) else b.cashbankdetail_mainaccountid end as test,d.subaccount_name  from Trans_CashBankVouchers  as a inner join Trans_CashBankDetail as b on a.cashbank_id=b.cashbankdetail_voucherid inner join master_mainaccount  as c on b.cashbankdetail_mainaccountid=c.MAinaccount_accountcode inner join master_subaccount as d  on b.cashbankdetail_subAccountid=d.subaccount_code WHERE  a.cashbank_transactionDate>='" + Convert.ToDateTime(FromDate.Value).ToShortDateString() + "'  and a.cashbank_transactionDate<='" + Convert.ToDateTime(DateTo.Value).ToShortDateString() + "' and (b.cashbankdetail_cashbankid='" + txtBankName_hidden.Value.ToString() + "' or b.cashbankdetail_mainaccountid='" + txtBankName_hidden.Value.ToString() + "')AND (subaccount_mainacreferenceid=b.cashbankdetail_mainaccountid or cashbankdetail_Subaccountid='' or cashbankdetail_Subaccountid is null))as d", "d.cashbank_transactionDate as [Trans Date],d.cashbank_vouchernumber as [Voucher No],d.cashbankdetail_instrumentdate as [Ins Date],d.cashbankdetail_instrumentnumber as [Ins No],d.type1 as Type,case substring(d.cashbank_vouchernumber,1,1) when 'C' then (select mainaccount_name from master_mainaccount where mainaccount_accountcode= d.test)  else d.AccountCode end as AccountCode,d.cashbankdetail_paymentamount as [Pmt Amt],d.cashbankdetail_receiptamount as [Rcpt Amt],d.cashbankdetail_bankvaluedate as [Value Date],d.bankstatementdate as [Statement Date]", null, "cashbank_transactionDate_test,cashbankdetail_instrumentnumber");
            }
            //DT = oDBEngine.GetDataTable("Trans_CashBankVouchers  as a inner join Trans_CashBankDetail as b on a.cashbank_id=b.cashbankdetail_voucherid inner join master_mainaccount as c on b.cashbankdetail_mainaccountid=c.MAinaccount_accountcode inner join master_subaccount as d on b.cashbankdetail_subAccountid=d.subaccount_code", "distinct a.cashbank_transactionDate as cashbank_transactionDate_test,(convert(varchar(11),a.cashbank_transactionDate,113))as cashbank_transactionDate,a.cashbank_vouchernumber,(convert(varchar(11),b.cashbankdetail_instrumentdate,113))as cashbankdetail_instrumentdate,b.cashbankdetail_instrumentdate as cashbankdetail_instrumentdate_test,b.cashbankdetail_instrumentnumber,c.mainaccount_name +'['+d.subaccount_name+'['+ (select  ltrim(rtrim(cnt_ucc)) from tbl_master_contact where cnt_internalid=d.subaccount_code )+']' +']' as AccountCode,case b.cashbankdetail_paymentamount when 0 then null else b.cashbankdetail_paymentamount end as cashbankdetail_paymentamount,case b.cashbankdetail_receiptamount when 0 then null else b.cashbankdetail_receiptamount end as cashbankdetail_receiptamount,b.cashbankdetail_bankvaluedate,b.cashbankdetail_id,d.subaccount_code,b.cashbankdetail_mainaccountid ", " a.cashbank_transactionDate>='" + FromDate.Value + "' and a.cashbank_transactionDate<='" + DateTo.Value + "' and (b.cashbankdetail_bankvaluedate is null or b.cashbankdetail_bankvaluedate ='1900-01-01 00:00:00.000') and b.cashbankdetail_cashbankid='" + txtBankName_hidden.Value.ToString() + "'", "cashbank_transactionDate");
            ViewState["DTExport"] = DTExport;
            grdDetails.DataSource = DT;
            grdDetails.DataBind();
            if (DT.Rows.Count > 0)
            {
                grdDetails.FooterRow.Cells[0].Text = "Total";
                grdDetails.FooterRow.Cells[0].Font.Bold = true;
                if (PaymentAmount != 0)
                {
                    grdDetails.FooterRow.Cells[6].Text = ObjConvert.getFormattedvalue(PaymentAmount);
                    grdDetails.FooterRow.Cells[6].Font.Bold = true;
                    grdDetails.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Right;

                }
                ViewState["PaymentAmount"] = PaymentAmount;
                if (ReceiptAmount != 0)
                {
                    grdDetails.FooterRow.Cells[7].Text = ObjConvert.getFormattedvalue(ReceiptAmount);
                    grdDetails.FooterRow.Cells[7].Font.Bold = true;
                    grdDetails.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Right;
                    grdDetails.FooterRow.Cells[7].ForeColor = System.Drawing.Color.Blue;

                }
                ViewState["ReceiptAmount"] = ReceiptAmount;
                trhypertext.Visible = true;
                MainContent.Visible = true;
                ViewState["TableForThePage"] = DT;
            }

        }
        protected void btnShow_Click1(object sender, EventArgs e)
        {
            BindTable();
            if (grdDetails.Rows.Count == 0)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "MessageForBlankRecord", "<script language='JavaScript'>alert('No Record Found');</script>");

            }
            //Page.ClientScript.RegisterStartupScript(GetType(), "CallingHeight", "<script language='Javascript'>height();</script>");
            Page.ClientScript.RegisterStartupScript(GetType(), "ForSearchHide1", "<script language='Javascript'>SearchVisible('N');</script>");

        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in grdDetails.Rows)
            {
                Label lblGetID = (Label)row.FindControl("lblid");
                string IdForUpdateData = lblGetID.Text.ToString();
                TextBox txtValueDate = (TextBox)row.FindControl("txtValueDate");
                string[] DateFormat;
                string FormatedValueDate;
                if (txtValueDate.Text != "")
                {
                    DateFormat = txtValueDate.Text.Split('-');
                    FormatedValueDate = DateFormat[1].ToString() + "-" + DateFormat[0].ToString() + "-" + DateFormat[2].ToString();
                }
                else
                {
                    FormatedValueDate = "";
                }
                Label lblSAcc = (Label)row.FindControl("lblSubCode");
                Label lblMAcc = (Label)row.FindControl("lblMainAcc");
                Label lblRefID = (Label)row.FindControl("lblInsNo");
                Label lblVouNo = (Label)row.FindControl("lblVoucherNo");
                Label lblINo = (Label)row.FindControl("lblInsNo");
                Label lblTDate = (Label)row.FindControl("lblTranDate");

                if (lblVouNo.Text.Substring(0, 1) == "C")
                {
                    string ca = oDBEngine.GetFieldValue("Trans_CashBankDetail as a inner join Trans_CashBankVouchers as  b on a.cashbankdetail_voucherid=b.cashbank_id ", "b.cashbank_id", " a.cashbankdetail_id='" + lblGetID.Text.ToString().Trim() + "'", 1)[0, 0];
                    oDBEngine.SetFieldValue("Trans_CashBankDetail", "cashbankdetail_bankvaluedate='" + FormatedValueDate.ToString().Trim() + "'", "cashbankdetail_voucherid='" + ca.ToString().Trim() + "'");

                }
                else
                {
                    oDBEngine.SetFieldValue("Trans_CashBankDetail", "cashbankdetail_bankvaluedate='" + FormatedValueDate.ToString().Trim() + "'", "cashbankdetail_id='" + lblGetID.Text.ToString().Trim() + "'");
                }
                oDBEngine.SetFieldValue("trans_accountsledger", "accountsledger_valuedate='" + FormatedValueDate.ToString().Trim() + "'", "accountsledger_subaccountid='" + lblSAcc.Text.ToString().Trim() + "' and accountsledger_Mainaccountid='" + lblMAcc.Text.ToString().Trim() + "' and accountsledger_transactionreferenceid='" + lblVouNo.Text.ToString().Trim() + "' and accountsledger_instrumentnumber='" + lblINo.Text.ToString().Trim() + "' and accountsledger_transactiondate='" + lblTDate.Text.ToString().Trim() + "'");
            }
            BindTable();
            Page.ClientScript.RegisterStartupScript(GetType(), "ForSearchHideUpdate", "<script language='Javascript'>SearchVisible('N');</script>");

        }
        protected void lnAllRecords_Click(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "ForSearchHide", "<script language='Javascript'>SearchVisible('N');</script>");
            BindTable();
            ClearControls();
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            DT = (DataTable)ViewState["TableForThePage"];
            DT1 = DT.Copy();
            DT1.Clear();
            DataRow[] Dr;
            if (AspTranDate.Value != null)
            {
                Dr = DT.Select("cashbank_transactionDate_test = '" + AspTranDate.Value.ToString() + "'");
                CreateNewDataTableForSearch(Dr.Length, Dr);

            }
            if (txtVoucherNo.Text != "" && txtVoucherNo.Text != "Voucher No")
            {
                Dr = DT.Select("cashbank_vouchernumber like '" + txtVoucherNo.Text.ToString() + "%'");
                CreateNewDataTableForSearch(Dr.Length, Dr);
            }
            if (AspInsDate.Value != null)
            {

                Dr = DT.Select("cashbankdetail_instrumentdate_test = '" + AspInsDate.Value.ToString() + "'");
                CreateNewDataTableForSearch(Dr.Length, Dr);
            }
            if (txtInsNo.Text.ToString() != "" && txtInsNo.Text.ToString() != "Ins No")
            {
                Dr = DT.Select("cashbankdetail_instrumentnumber like '" + txtInsNo.Text.ToString() + "%'");
                CreateNewDataTableForSearch(Dr.Length, Dr);

            }
            if (txtAccName.Text.ToString() != "" && txtAccName.Text.ToString() != "Main Account")
            {
                Dr = DT.Select("AccountCode like '" + txtAccName.Text.ToString() + "%'");
                CreateNewDataTableForSearch(Dr.Length, Dr);
            }
            if (txtPayAmt.Text.ToString() != "" && txtPayAmt.Text.ToString() != "Pmt Amount")
            {
                Dr = DT.Select("cashbankdetail_paymentamount = '" + txtPayAmt.Text.ToString() + "'");
                CreateNewDataTableForSearch(Dr.Length, Dr);
            }
            if (txtReptAmt.Text.ToString() != "" && txtReptAmt.Text.ToString() != "Rcpt Amount")
            {
                Dr = DT.Select("cashbankdetail_receiptamount = '" + txtReptAmt.Text.ToString() + "'");
                CreateNewDataTableForSearch(Dr.Length, Dr);
            }
            //if (AspValueDate.Value != null)
            //{

            //    Dr = DT.Select("cashbankdetail_bankvaluedate = '" + AspValueDate.Value.ToString() + "'");
            //    CreateNewDataTableForSearch(Dr.Length, Dr);

            //}
            if (txtSubName.Text.ToString() != "" && txtSubName.Text.ToString() != "SubAccount")
            {
                Dr = DT.Select("subaccount_name like '" + txtSubName.Text.ToString() + "%'");
                CreateNewDataTableForSearch(Dr.Length, Dr);
            }
            if (DT1.Rows.Count == 0)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "Message", "<script language='JavaScript'>alert('No record found');</script>");
            }

            //BindTable();

            trhypertext.Visible = true;


        }
        public void CreateNewDataTableForSearch(int length, DataRow[] DataRow)
        {

            if (length == 0)
            {
                DT1.Rows.Clear();
            }
            for (int count = 0; count < length; count++)
            {
                DataRow drnew = DT1.NewRow();
                drnew["cashbank_transactionDate_test"] = DataRow[count]["cashbank_transactionDate_test"].ToString();
                drnew["cashbank_transactionDate"] = DataRow[count]["cashbank_transactionDate"].ToString();
                drnew["cashbank_vouchernumber"] = DataRow[count]["cashbank_vouchernumber"].ToString();
                drnew["cashbankdetail_instrumentdate"] = DataRow[count]["cashbankdetail_instrumentdate"].ToString();
                drnew["cashbankdetail_instrumentdate_test"] = DataRow[count]["cashbankdetail_instrumentdate_test"].ToString();
                drnew["cashbankdetail_instrumentnumber"] = DataRow[count]["cashbankdetail_instrumentnumber"].ToString();
                drnew["AccountCode"] = DataRow[count]["AccountCode"].ToString();
                //if (DataRow[count]["cashbankdetail_paymentamount"] != null)
                //{
                drnew["cashbankdetail_paymentamount"] = DataRow[count]["cashbankdetail_paymentamount"];
                //}
                //if (DataRow[count]["cashbankdetail_receiptamount"] != null)
                //{
                drnew["cashbankdetail_receiptamount"] = DataRow[count]["cashbankdetail_receiptamount"];
                //}
                drnew["cashbankdetail_bankvaluedate"] = DataRow[count]["cashbankdetail_bankvaluedate"];
                drnew["cashbankdetail_id"] = DataRow[count]["cashbankdetail_id"].ToString();
                drnew["subaccount_code"] = DataRow[count]["subaccount_code"].ToString();
                drnew["subaccount_name"] = DataRow[count]["subaccount_name"].ToString();
                drnew["cashbankdetail_mainaccountid"] = DataRow[count]["cashbankdetail_mainaccountid"].ToString();
                drnew["bankstatementdate"] = DataRow[count]["bankstatementdate"].ToString();

                DT1.Rows.Add(drnew);

                //DT1[count]["cashbank_transactionDate"] = Dr[count]["cashbank_transactionDate"].ToString();
            }
            trhypertext.Visible = true;
            grdDetails.DataSource = DT1;
            grdDetails.DataBind();
            //BindTable();

        }
        public void ClearControls()
        {
            AspTranDate.Value = null;
            txtVoucherNo.Text = "Voucher No";
            txtVoucherNo.ToolTip = "Voucher No";
            txtAccName.Text = "Main Account";
            txtAccName.ToolTip = "Main Account";
            AspInsDate.Value = null;
            txtInsNo.Text = "Ins No";
            txtInsNo.ToolTip = "Ins No";
            txtPayAmt.Text = "Pmt Amount";
            txtPayAmt.ToolTip = "Pmt Amount";
            txtReptAmt.Text = "Rcpt Amount";
            txtReptAmt.ToolTip = "Rcpt Amount";
            txtSubName.Text = "SubAccount";
            txtSubName.ToolTip = "SubAccount";
            //AspValueDate.Value = null;
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Server.Transfer("Welcome.aspx");
        }
        protected void grdDetails_DataBound(object sender, EventArgs e)
        {
            foreach (GridViewRow row in grdDetails.Rows)
            {
                Label lblpAmount = (Label)row.FindControl("lblPmtAmt");
                string lcVar = lblpAmount.Text.ToString();
                if (lcVar.ToString() != "")
                {
                    row.Cells[6].Text = ObjConvert.getFormattedvalue(decimal.Parse(lcVar));
                }
                Label lblrAmount = (Label)row.FindControl("lblRcptAmt");
                string lcVar1 = lblrAmount.Text.ToString();
                if (lcVar1.ToString() != "")
                {
                    row.Cells[7].Text = ObjConvert.getFormattedvalue(decimal.Parse(lcVar1));
                }
                if (lcVar == "")
                    lcVar = "0";
                if (lcVar1 == "")
                    lcVar1 = "0";
                PaymentAmount += decimal.Parse(lcVar);
                ReceiptAmount += decimal.Parse(lcVar1);
            }

        }

        protected void ddlExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            //DtForExportData.Rows.Clear();
            //DataTable ClosingBal = new DataTable();//creating data table for closing bank
            //ClosingBal = oDBEngine.OpeningBalance(txtBankName_hidden.Value.ToString(), null, Convert.ToDateTime(ASPxAsOnDate.Value), Session["usersegid"].ToString(), Session["LastCompany"].ToString());
            //decimal BankBalance = Convert.ToDecimal(ClosingBal.Rows[0]["cl"].ToString());
            //DtChequeDeposited = oDBEngine.GetDataTable("Trans_CashBankVouchers  as a inner join Trans_CashBankDetail as b on a.cashbank_id=b.cashbankdetail_voucherid inner join master_mainaccount as c on b.cashbankdetail_mainaccountid=c.MAinaccount_accountcode inner join master_subaccount as d on b.cashbankdetail_subAccountid=d.subaccount_code  ", "distinct(convert(varchar(11),a.cashbank_transactionDate,113))as TransactionDate,a.cashbank_vouchernumber as VoucherNumber,(convert(varchar(11),b.cashbankdetail_instrumentdate,113)) as InstrumentDate,b.cashbankdetail_instrumentnumber as InstrumentNumber,c.mainaccount_name +' ['+isnull(rtrim(ltrim(d.subaccount_name)),'')+'['+ (isnull((select isnull(ltrim(rtrim(cnt_ucc)),'') from tbl_master_contact where cnt_internalid=d.subaccount_code),(select top 1 subaccount_code from master_subaccount where cast(subaccount_code as varchar)=d.subaccount_code and subaccount_mainacreferenceid=b.cashbankdetail_mainaccountid)))+']' +']' as AccountCode,case b.cashbankdetail_paymentamount when 0 then null else cast(b.cashbankdetail_paymentamount as money) end  as PaymentAmount,case b.cashbankdetail_receiptamount when 0 then null else cast(b.cashbankdetail_receiptamount as money) end  as ReceiptAmount", "(b.cashbankdetail_bankvaluedate is null or b.cashbankdetail_bankvaluedate ='1900-01-01 00:00:00.000' or b.cashbankdetail_bankvaluedate >'" + ASPxAsOnDate.Value + "') and (substring(a.cashbank_vouchernumber,1,1)='R' or (substring(a.cashbank_vouchernumber,1,1)='C' and  b.cashbankdetail_paymentamount= 0)) and (b.cashbankdetail_cashbankid='" + txtBankName_hidden.Value.ToString() + "' or b.cashbankdetail_mainaccountid='" + txtBankName_hidden.Value.ToString() + "') and a.cashbank_transactionDate<='" + ASPxAsOnDate.Value.ToString() + "'", null);
            ////Creating New Datatable for exporting data to excel 
            //DtForExportData = DtChequeDeposited.Copy();
            //DtForExportData.Columns.Add("AmountDebit", Type.GetType("System.String"));
            //DtForExportData.Columns.Add("AmountCredit", Type.GetType("System.String"));
            //DtForExportData.Rows.Clear();
            //DataRow drFirst = DtForExportData.NewRow();//Adding First Row
            //drFirst["TransactionDate"] = " Bank Balance As Per Bank Book : ";
            //if (BankBalance < 0)
            //{
            //    drFirst["AmountDebit"] = Math.Abs(BankBalance).ToString();
            //}
            //else
            //{
            //    drFirst["AmountCredit"] = Math.Abs(BankBalance).ToString();

            //}
            //DtForExportData.Rows.Add(drFirst);
            //DataRow drSecond = DtForExportData.NewRow();//Adding Second Row
            //drSecond["TransactionDate"] = " Cheques Deposited But Not Cleared: ";
            //DtForExportData.Rows.Add(drSecond);
            //for (int count = 0; count < DtChequeDeposited.Rows.Count; count++)
            //{
            //    DataRow dr = DtForExportData.NewRow();
            //    dr["TransactionDate"] = DtChequeDeposited.Rows[count]["TransactionDate"];
            //    dr["VoucherNumber"] = DtChequeDeposited.Rows[count]["VoucherNumber"];
            //    dr["InstrumentDate"] = DtChequeDeposited.Rows[count]["InstrumentDate"];
            //    dr["InstrumentNumber"] = DtChequeDeposited.Rows[count]["InstrumentNumber"];
            //    dr["AccountCode"] = DtChequeDeposited.Rows[count]["AccountCode"];
            //    dr["PaymentAmount"] = DtChequeDeposited.Rows[count]["PaymentAmount"];
            //    if (DtChequeDeposited.Rows[count]["ReceiptAmount"].ToString() != "")
            //    {
            //        dr["ReceiptAmount"] = ObjConvert.getFormattedvaluewithoriginalsign(Convert.ToDecimal(DtChequeDeposited.Rows[count]["ReceiptAmount"]));

            //    }
            //    DtForExportData.Rows.Add(dr);

            //}
            ////DataRow drTotDeposited = DtForExportData.NewRow();//Adding Total for Cheque Deposited
            ////drTotDeposited["TransactionDate"] = " Total: ";
            //string TotalDeposited = oDBEngine.GetFieldValue("(Select distinct(convert(varchar(11),a.cashbank_transactionDate,113))as cashbank_transactionDate,a.cashbank_transactionDate as ForOrder,a.cashbank_vouchernumber,(convert(varchar(11),b.cashbankdetail_instrumentdate,113))as cashbankdetail_instrumentdate,b.cashbankdetail_instrumentnumber,c.mainaccount_name+' ' +'['+isnull(rtrim(ltrim(d.subaccount_name)),'')+'['+ (isnull((select isnull(ltrim(rtrim(cnt_ucc)),'') from tbl_master_contact where cnt_internalid=d.subaccount_code),(select top 1 subaccount_code from master_subaccount where cast(subaccount_code as varchar)=d.subaccount_code and subaccount_mainacreferenceid=b.cashbankdetail_mainaccountid)))+']' +']' as AccountCode,case b.cashbankdetail_paymentamount when 0 then null else b.cashbankdetail_paymentamount end as cashbankdetail_paymentamount,case b.cashbankdetail_receiptamount when 0 then null else b.cashbankdetail_receiptamount end as cashbankdetail_receiptamount   from Trans_CashBankVouchers  as a inner join Trans_CashBankDetail as b on a.cashbank_id=b.cashbankdetail_voucherid inner join master_mainaccount as c on b.cashbankdetail_mainaccountid=c.MAinaccount_accountcode inner join master_subaccount as d on b.cashbankdetail_subAccountid=d.subaccount_code   WHERE (b.cashbankdetail_bankvaluedate is null or b.cashbankdetail_bankvaluedate ='1900-01-01 00:00:00.000' or b.cashbankdetail_bankvaluedate >'" + ASPxAsOnDate.Value + "') and (substring(a.cashbank_vouchernumber,1,1)='R' or (substring(a.cashbank_vouchernumber,1,1)='C' and  b.cashbankdetail_paymentamount= 0)) and (b.cashbankdetail_cashbankid='" + txtBankName_hidden.Value.ToString() + "' or b.cashbankdetail_mainaccountid='" + txtBankName_hidden.Value.ToString() + "') and  a.cashbank_transactionDate<='" + ASPxAsOnDate.Value.ToString() + "')as a", " sum(cashbankdetail_receiptamount) as total", null, 1)[0, 0];
            //string TotalIssued = oDBEngine.GetFieldValue("(Select  distinct(convert(varchar(11),a.cashbank_transactionDate,113))as cashbank_transactionDate,a.cashbank_transactionDate as ForOrder,a.cashbank_vouchernumber,(convert(varchar(11),b.cashbankdetail_instrumentdate,113))as cashbankdetail_instrumentdate,b.cashbankdetail_instrumentnumber,c.mainaccount_name+' ' +'['+isnull(rtrim(ltrim(d.subaccount_name)),'')+'['+ (isnull((select isnull(ltrim(rtrim(cnt_ucc)),'') from tbl_master_contact where cnt_internalid=d.subaccount_code),(select top 1 subaccount_code from master_subaccount where cast(subaccount_code as varchar)=d.subaccount_code and subaccount_mainacreferenceid=b.cashbankdetail_mainaccountid)))+']' +']' as AccountCode,case b.cashbankdetail_paymentamount when 0 then null else b.cashbankdetail_paymentamount end as cashbankdetail_paymentamount,case b.cashbankdetail_receiptamount when 0 then null else b.cashbankdetail_receiptamount end as cashbankdetail_receiptamount from Trans_CashBankVouchers  as a inner join Trans_CashBankDetail as b on a.cashbank_id=b.cashbankdetail_voucherid inner join  master_mainaccount as c on b.cashbankdetail_mainaccountid=c.MAinaccount_accountcode inner join master_subaccount as d on b.cashbankdetail_subAccountid=d.subaccount_code   WHERE (b.cashbankdetail_bankvaluedate is null or b.cashbankdetail_bankvaluedate ='1900-01-01 00:00:00.000' or b.cashbankdetail_bankvaluedate >'" + ASPxAsOnDate.Value.ToString() + "') and substring(a.cashbank_vouchernumber,1,1)='P' and b.cashbankdetail_cashbankid='" + txtBankName_hidden.Value.ToString() + "'and a.cashbank_transactionDate<='" + ASPxAsOnDate.Value.ToString() + "' AND (subaccount_mainacreferenceid=b.cashbankdetail_mainaccountid or cashbankdetail_Subaccountid='' or cashbankdetail_Subaccountid is null) union select FinalTable.cashbank_transactionDate,FinalTable.ForOrder,FinalTable.cashbank_vouchernumber,FinalTable.cashbankdetail_instrumentdate,FinalTable.cashbankdetail_instrumentnumber,master_mainaccount.mainaccount_name as Accountcode,FinalTable.cashbankdetail_paymentamount,FinalTable.cashbankdetail_receiptamount from (Select distinct(convert(varchar(11),a.cashbank_transactionDate,113))as cashbank_transactionDate,a.cashbank_transactionDate as ForOrder,a.cashbank_vouchernumber,(convert(varchar(11),b.cashbankdetail_instrumentdate,113))as cashbankdetail_instrumentdate,b.cashbankdetail_instrumentnumber,(select pay.cashbankdetail_mainaccountid from (Select distinct b.* from Trans_CashBankVouchers  as a inner join Trans_CashBankDetail as b on a.cashbank_id=b.cashbankdetail_voucherid inner join master_mainaccount as c on b.cashbankdetail_mainaccountid=c.MAinaccount_accountcode inner join master_subaccount as d on b.cashbankdetail_subAccountid=d.subaccount_code   WHERE (b.cashbankdetail_bankvaluedate is null or b.cashbankdetail_bankvaluedate ='1900-01-01 00:00:00.000' or b.cashbankdetail_bankvaluedate >'" + ASPxAsOnDate.Value.ToString() + "') and substring(a.cashbank_vouchernumber,1,1)='C' and  b.cashbankdetail_paymentamount= 0)as Pay,(Select distinct b.* from Trans_CashBankVouchers  as a inner join Trans_CashBankDetail as b on a.cashbank_id=b.cashbankdetail_voucherid inner join master_mainaccount as c on b.cashbankdetail_mainaccountid=c.MAinaccount_accountcode inner join master_subaccount as d on b.cashbankdetail_subAccountid=d.subaccount_code   WHERE (b.cashbankdetail_bankvaluedate is null or b.cashbankdetail_bankvaluedate ='1900-01-01 00:00:00.000' or b.cashbankdetail_bankvaluedate >'" + ASPxAsOnDate.Value.ToString() + "') and substring(a.cashbank_vouchernumber,1,1)='C' and  b.cashbankdetail_receiptamount= 0)as Receipt where pay.cashbankdetail_voucherid=receipt.cashbankdetail_voucherid and b.cashbankdetail_voucherid=receipt.cashbankdetail_voucherid) as acc,c.mainaccount_name+' ' +'['+isnull(rtrim(ltrim(d.subaccount_name)),'')+'['+ (isnull((select isnull(ltrim(rtrim(cnt_ucc)),'') from tbl_master_contact where cnt_internalid=d.subaccount_code),(select top 1 subaccount_code from master_subaccount where cast(subaccount_code as varchar)=d.subaccount_code and subaccount_mainacreferenceid=b.cashbankdetail_mainaccountid)))+']' +']' as AccountCode,case b.cashbankdetail_paymentamount when 0 then null else b.cashbankdetail_paymentamount end as cashbankdetail_paymentamount,case b.cashbankdetail_receiptamount when 0 then null else b.cashbankdetail_receiptamount end as cashbankdetail_receiptamount   from Trans_CashBankVouchers  as a inner join Trans_CashBankDetail as b on a.cashbank_id=b.cashbankdetail_voucherid inner join master_mainaccount as c on b.cashbankdetail_mainaccountid=c.MAinaccount_accountcode inner join master_subaccount as d on b.cashbankdetail_subAccountid=d.subaccount_code   WHERE (b.cashbankdetail_bankvaluedate is null or b.cashbankdetail_bankvaluedate ='1900-01-01 00:00:00.000' or b.cashbankdetail_bankvaluedate >'" + ASPxAsOnDate.Value.ToString() + "') and substring(a.cashbank_vouchernumber,1,1)='C' and  b.cashbankdetail_receiptamount= 0 and b.cashbankdetail_mainaccountid='" + txtBankName_hidden.Value.ToString() + "' and a.cashbank_transactionDate<='" + ASPxAsOnDate.Value.ToString() + "') as FinalTable left outer join master_mainaccount on FinalTable.acc=master_mainaccount.mainaccount_accountcode WHERE FinalTable.cashbank_transactionDate<='" + ASPxAsOnDate.Value.ToString() + "')as a", "sum(cashbankdetail_paymentamount) as total", null, 1)[0, 0];
            ////oDBEngine.GetFieldValue("(Select  distinct(convert(varchar(11),a.cashbank_transactionDate,113))as cashbank_transactionDate,a.cashbank_transactionDate as ForOrder,a.cashbank_vouchernumber,(convert(varchar(11),b.cashbankdetail_instrumentdate,113))as cashbankdetail_instrumentdate,b.cashbankdetail_instrumentnumber,c.mainaccount_name +' ['+isnull((isnull(rtrim(ltrim(d.subaccount_name)),'')+'['+ (isnull((select isnull(ltrim(rtrim(cnt_ucc)),'') from tbl_master_contact where cnt_internalid=d.subaccount_code),(select top 1 subaccount_code from master_subaccount where cast(subaccount_code as varchar)=d.subaccount_code and subaccount_mainacreferenceid=b.cashbankdetail_mainaccountid)))+']'),'') +']' as AccountCode,case b.cashbankdetail_paymentamount when 0 then null else b.cashbankdetail_paymentamount end as cashbankdetail_paymentamount,case b.cashbankdetail_receiptamount when 0 then null else b.cashbankdetail_receiptamount end as cashbankdetail_receiptamount from Trans_CashBankVouchers  as a inner join Trans_CashBankDetail as b on a.cashbank_id=b.cashbankdetail_voucherid inner join  master_mainaccount as c on b.cashbankdetail_mainaccountid=c.MAinaccount_accountcode inner join master_subaccount as d on b.cashbankdetail_subAccountid=d.subaccount_code   WHERE (b.cashbankdetail_bankvaluedate is null or b.cashbankdetail_bankvaluedate ='1900-01-01 00:00:00.000' or b.cashbankdetail_bankvaluedate >'" + ASPxAsOnDate.Value.ToString() + "') and substring(a.cashbank_vouchernumber,1,1)='P' and b.cashbankdetail_cashbankid='" + txtBankName_hidden.Value.ToString() + "' and  a.cashbank_transactionDate<='" + ASPxAsOnDate.Value.ToString() + "' union select FinalTable.cashbank_transactionDate,FinalTable.ForOrder,FinalTable.cashbank_vouchernumber,FinalTable.cashbankdetail_instrumentdate,FinalTable.cashbankdetail_instrumentnumber,master_mainaccount.mainaccount_name as Accountcode,FinalTable.cashbankdetail_paymentamount,FinalTable.cashbankdetail_receiptamount from (Select distinct(convert(varchar(11),a.cashbank_transactionDate,113))as cashbank_transactionDate,a.cashbank_transactionDate as ForOrder,a.cashbank_vouchernumber,(convert(varchar(11),b.cashbankdetail_instrumentdate,113))as cashbankdetail_instrumentdate,b.cashbankdetail_instrumentnumber,(select pay.cashbankdetail_mainaccountid from (Select distinct b.* from Trans_CashBankVouchers  as a inner join Trans_CashBankDetail as b on a.cashbank_id=b.cashbankdetail_voucherid inner join master_mainaccount as c on b.cashbankdetail_mainaccountid=c.MAinaccount_accountcode inner join master_subaccount as d on b.cashbankdetail_subAccountid=d.subaccount_code   WHERE (b.cashbankdetail_bankvaluedate is null or b.cashbankdetail_bankvaluedate ='1900-01-01 00:00:00.000' or b.cashbankdetail_bankvaluedate >'" + ASPxAsOnDate.Value.ToString() + "') and substring(a.cashbank_vouchernumber,1,1)='C' and  b.cashbankdetail_paymentamount= 0)as Pay,(Select distinct b.* from Trans_CashBankVouchers  as a inner join Trans_CashBankDetail as b on a.cashbank_id=b.cashbankdetail_voucherid inner join master_mainaccount as c on b.cashbankdetail_mainaccountid=c.MAinaccount_accountcode inner join master_subaccount as d on b.cashbankdetail_subAccountid=d.subaccount_code   WHERE (b.cashbankdetail_bankvaluedate is null or b.cashbankdetail_bankvaluedate ='1900-01-01 00:00:00.000' or b.cashbankdetail_bankvaluedate >'" + ASPxAsOnDate.Value.ToString() + "') and substring(a.cashbank_vouchernumber,1,1)='C' and  b.cashbankdetail_receiptamount= 0)as Receipt where pay.cashbankdetail_voucherid=receipt.cashbankdetail_voucherid and b.cashbankdetail_voucherid=receipt.cashbankdetail_voucherid) as acc,c.mainaccount_name+' ' +'['+isnull(rtrim(ltrim(d.subaccount_name)),'')+'['+ (isnull((select isnull(ltrim(rtrim(cnt_ucc)),'') from tbl_master_contact where cnt_internalid=d.subaccount_code),(select top 1 subaccount_code from master_subaccount where cast(subaccount_code as varchar)=d.subaccount_code and subaccount_mainacreferenceid=b.cashbankdetail_mainaccountid)))+']' +']' as AccountCode,case b.cashbankdetail_paymentamount when 0 then null else b.cashbankdetail_paymentamount end as cashbankdetail_paymentamount,case b.cashbankdetail_receiptamount when 0 then null else b.cashbankdetail_receiptamount end as cashbankdetail_receiptamount   from Trans_CashBankVouchers  as a inner join Trans_CashBankDetail as b on a.cashbank_id=b.cashbankdetail_voucherid inner join master_mainaccount as c on b.cashbankdetail_mainaccountid=c.MAinaccount_accountcode inner join master_subaccount as d on b.cashbankdetail_subAccountid=d.subaccount_code   WHERE (b.cashbankdetail_bankvaluedate is null or b.cashbankdetail_bankvaluedate ='1900-01-01 00:00:00.000' or b.cashbankdetail_bankvaluedate >'" + ASPxAsOnDate.Value.ToString() + "') and substring(a.cashbank_vouchernumber,1,1)='C' and  b.cashbankdetail_receiptamount= 0 and b.cashbankdetail_mainaccountid='" + txtBankName_hidden.Value.ToString() + "' and a.cashbank_transactionDate<='" + ASPxAsOnDate.Value.ToString() + "') as FinalTable left outer join master_mainaccount on FinalTable.acc=master_mainaccount.mainaccount_accountcode WHERE FinalTable.cashbank_transactionDate<='" + ASPxAsOnDate.Value.ToString() + "')as a", "sum(cashbankdetail_paymentamount)", null, 1)[0, 0];
            //DataRow drTotDeposited = DtForExportData.NewRow();
            //if (TotalDeposited == "n")
            //{
            //    TotalDeposited = "0";
            //}
            //drTotDeposited["TransactionDate"] = "Total";
            //drTotDeposited["AmountCredit"] = ObjConvert.getFormattedvaluewithoriginalsign(Convert.ToDecimal(TotalDeposited));
            //drTotDeposited["ReceiptAmount"] = ObjConvert.getFormattedvaluewithoriginalsign(Convert.ToDecimal(TotalDeposited));
            //DtForExportData.Rows.Add(drTotDeposited);
            ////Row for heading
            //DataRow drChHeading = DtForExportData.NewRow();
            //drChHeading["TransactionDate"] = "Cheques Issued But Not Cleared:";
            //DtForExportData.Rows.Add(drChHeading);
            ////DtChequeIssued = oDBEngine.GetDataTable("Trans_CashBankVouchers  as a inner join Trans_CashBankDetail as b on a.cashbank_id=b.cashbankdetail_voucherid inner join master_mainaccount as c on b.cashbankdetail_mainaccountid=c.MAinaccount_accountcode inner join master_subaccount as d on b.cashbankdetail_subAccountid=d.subaccount_code  ", "distinct(convert(varchar(11),a.cashbank_transactionDate,113))as TransactionDate,a.cashbank_vouchernumber as VoucherNumber,(convert(varchar(11),b.cashbankdetail_instrumentdate,113)) as InstrumentDate,b.cashbankdetail_instrumentnumber as InstrumentNumber,c.mainaccount_name +' ['+isnull(rtrim(ltrim(d.subaccount_name)),'')+'['+ (isnull((select isnull(ltrim(rtrim(cnt_ucc)),'') from tbl_master_contact where cnt_internalid=d.subaccount_code),(select top 1 subaccount_code from master_subaccount where cast(subaccount_code as varchar)=d.subaccount_code and subaccount_mainacreferenceid=b.cashbankdetail_mainaccountid)))+']' +']' as AccountCode,case b.cashbankdetail_paymentamount when 0 then null else b.cashbankdetail_paymentamount end  as PaymentAmount,case b.cashbankdetail_receiptamount when 0 then null else b.cashbankdetail_receiptamount end  as ReceiptAmount", "(b.cashbankdetail_bankvaluedate is null or b.cashbankdetail_bankvaluedate ='1900-01-01 00:00:00.000' or b.cashbankdetail_bankvaluedate >'" + ASPxAsOnDate.Value + "') and (substring(a.cashbank_vouchernumber,1,1)='P' or (substring(a.cashbank_vouchernumber,1,1)='C' and  b.cashbankdetail_receiptamount= 0)) and (b.cashbankdetail_cashbankid='" + txtBankName_hidden.Value.ToString() + "' or b.cashbankdetail_mainaccountid='" + txtBankName_hidden.Value.ToString() + "')", null);
            //DtChequeIssued = oDBEngine.GetDataTable("Trans_CashBankVouchers  as a inner join Trans_CashBankDetail as b on a.cashbank_id=b.cashbankdetail_voucherid inner join  master_mainaccount as c on b.cashbankdetail_mainaccountid=c.MAinaccount_accountcode inner join master_subaccount as d on b.cashbankdetail_subAccountid=d.subaccount_code   WHERE (b.cashbankdetail_bankvaluedate is null or b.cashbankdetail_bankvaluedate ='1900-01-01 00:00:00.000' or b.cashbankdetail_bankvaluedate >'" + ASPxAsOnDate.Value.ToString() + "') and substring(a.cashbank_vouchernumber,1,1)='P' and b.cashbankdetail_cashbankid='" + txtBankName_hidden.Value.ToString() + "' and a.cashbank_transactionDate<='" + ASPxAsOnDate.Value.ToString() + "'AND (subaccount_mainacreferenceid=b.cashbankdetail_mainaccountid or cashbankdetail_Subaccountid='' or cashbankdetail_Subaccountid is null) union select FinalTable.cashbank_transactionDate,FinalTable.cashbank_vouchernumber,FinalTable.cashbankdetail_instrumentdate,FinalTable.cashbankdetail_instrumentnumber,master_mainaccount.mainaccount_name as Accountcode,FinalTable.cashbankdetail_paymentamount,FinalTable.cashbankdetail_receiptamount from (Select distinct(convert(varchar(11),a.cashbank_transactionDate,113))as cashbank_transactionDate,a.cashbank_transactionDate as ForOrder,a.cashbank_vouchernumber,(convert(varchar(11),b.cashbankdetail_instrumentdate,113))as cashbankdetail_instrumentdate,b.cashbankdetail_instrumentnumber,(select pay.cashbankdetail_mainaccountid from (Select distinct b.* from Trans_CashBankVouchers  as a inner join Trans_CashBankDetail as b on a.cashbank_id=b.cashbankdetail_voucherid inner join master_mainaccount as c on b.cashbankdetail_mainaccountid=c.MAinaccount_accountcode inner join master_subaccount as d on b.cashbankdetail_subAccountid=d.subaccount_code   WHERE (b.cashbankdetail_bankvaluedate is null or b.cashbankdetail_bankvaluedate ='1900-01-01 00:00:00.000' or b.cashbankdetail_bankvaluedate >'" + ASPxAsOnDate.Value.ToString() + "') and substring(a.cashbank_vouchernumber,1,1)='C' and  b.cashbankdetail_paymentamount= 0)as Pay,(Select distinct b.* from Trans_CashBankVouchers  as a inner join Trans_CashBankDetail as b on a.cashbank_id=b.cashbankdetail_voucherid inner join master_mainaccount as c on b.cashbankdetail_mainaccountid=c.MAinaccount_accountcode inner join master_subaccount as d on b.cashbankdetail_subAccountid=d.subaccount_code   WHERE (b.cashbankdetail_bankvaluedate is null or b.cashbankdetail_bankvaluedate ='1900-01-01 00:00:00.000' or b.cashbankdetail_bankvaluedate >'" + ASPxAsOnDate.Value.ToString() + "') and substring(a.cashbank_vouchernumber,1,1)='C' and  b.cashbankdetail_receiptamount= 0)as Receipt where pay.cashbankdetail_voucherid=receipt.cashbankdetail_voucherid and b.cashbankdetail_voucherid=receipt.cashbankdetail_voucherid) as acc,c.mainaccount_name+' ' +'['+isnull(rtrim(ltrim(d.subaccount_name)),'')+'['+ (isnull((select isnull(ltrim(rtrim(cnt_ucc)),'') from tbl_master_contact where cnt_internalid=d.subaccount_code),(select top 1 subaccount_code from master_subaccount where cast(subaccount_code as varchar)=d.subaccount_code and subaccount_mainacreferenceid=b.cashbankdetail_mainaccountid)))+']' +']' as AccountCode,case b.cashbankdetail_paymentamount when 0 then null else b.cashbankdetail_paymentamount end as cashbankdetail_paymentamount,case b.cashbankdetail_receiptamount when 0 then null else b.cashbankdetail_receiptamount end as cashbankdetail_receiptamount   from Trans_CashBankVouchers  as a inner join Trans_CashBankDetail as b on a.cashbank_id=b.cashbankdetail_voucherid inner join master_mainaccount as c on b.cashbankdetail_mainaccountid=c.MAinaccount_accountcode inner join master_subaccount as d on b.cashbankdetail_subAccountid=d.subaccount_code   WHERE (b.cashbankdetail_bankvaluedate is null or b.cashbankdetail_bankvaluedate ='1900-01-01 00:00:00.000' or b.cashbankdetail_bankvaluedate >'" + ASPxAsOnDate.Value.ToString() + "') and substring(a.cashbank_vouchernumber,1,1)='C' and  b.cashbankdetail_receiptamount= 0 and b.cashbankdetail_mainaccountid='" + txtBankName_hidden.Value.ToString() + "' and a.cashbank_transactionDate<='" + ASPxAsOnDate.Value.ToString() + "') as FinalTable left outer join master_mainaccount on FinalTable.acc=master_mainaccount.mainaccount_accountcode WHERE FinalTable.cashbank_transactionDate<='" + ASPxAsOnDate.Value.ToString() + "'", "distinct(convert(varchar(11),a.cashbank_transactionDate,113))as TransactionDate,a.cashbank_vouchernumber as VoucherNumber,(convert(varchar(11),b.cashbankdetail_instrumentdate,113))as InstrumentDate,b.cashbankdetail_instrumentnumber as InstrumentNumber,c.mainaccount_name+' ' +'['+isnull(rtrim(ltrim(d.subaccount_name)),'')+'['+ (isnull((select isnull(ltrim(rtrim(cnt_ucc)),'') from tbl_master_contact where cnt_internalid=d.subaccount_code),(select top 1 subaccount_code from master_subaccount where cast(subaccount_code as varchar)=d.subaccount_code and subaccount_mainacreferenceid=b.cashbankdetail_mainaccountid)))+']' +']' as AccountCode,case b.cashbankdetail_paymentamount when 0 then null else b.cashbankdetail_paymentamount end  as  PaymentAmount,case b.cashbankdetail_receiptamount when 0 then null else b.cashbankdetail_receiptamount end as ReceiptAmount", null);
            //for (int count = 0; count < DtChequeIssued.Rows.Count; count++)
            //{
            //    DataRow dr = DtForExportData.NewRow();
            //    dr["TransactionDate"] = DtChequeIssued.Rows[count]["TransactionDate"];
            //    dr["VoucherNumber"] = DtChequeIssued.Rows[count]["VoucherNumber"];
            //    dr["InstrumentDate"] = DtChequeIssued.Rows[count]["InstrumentDate"];
            //    dr["InstrumentNumber"] = DtChequeIssued.Rows[count]["InstrumentNumber"];
            //    dr["AccountCode"] = DtChequeIssued.Rows[count]["AccountCode"];
            //    dr["PaymentAmount"] = ObjConvert.getFormattedvaluewithoriginalsign(Convert.ToDecimal(DtChequeIssued.Rows[count]["PaymentAmount"]));
            //    dr["ReceiptAmount"] = DtChequeIssued.Rows[count]["ReceiptAmount"];
            //    DtForExportData.Rows.Add(dr);
            //    //LastCount = LastCount + 1;
            //}
            ////Row for adding total
            //DataRow drTotIssued = DtForExportData.NewRow();
            //drTotIssued["TransactionDate"] = "Total";
            //if (TotalIssued == "n")
            //{
            //    TotalIssued = "0";
            //}
            //drTotIssued["AmountDebit"] = ObjConvert.getFormattedvaluewithoriginalsign(Convert.ToDecimal(TotalIssued));
            //drTotIssued["PaymentAmount"] = ObjConvert.getFormattedvaluewithoriginalsign(Convert.ToDecimal(TotalIssued));
            //DtForExportData.Rows.Add(drTotIssued);
            ////Row for Balance
            //string Balance = ObjConvert.getFormattedvaluewithoriginalsign(Convert.ToDecimal(ClosingBal.Rows[0]["cl"].ToString()) + Convert.ToDecimal(TotalDeposited.ToString()) - Convert.ToDecimal(TotalIssued.ToString()));
            //decimal BalValue = Convert.ToDecimal(ClosingBal.Rows[0]["cl"].ToString()) + Convert.ToDecimal(TotalDeposited.ToString()) - Convert.ToDecimal(TotalIssued.ToString());
            //string BalCredit = "";
            //// Check the balance wheathe it is Credit amount or debit
            //if (BalValue < 0)
            //{
            //    Balance = ObjConvert.getFormattedvaluewithoriginalsign(Math.Abs(BalValue)).ToString();
            //    BalCredit = "";
            //}
            //else
            //{
            //    BalCredit = ObjConvert.getFormattedvaluewithoriginalsign(Math.Abs(BalValue)).ToString();
            //    Balance = "";
            //}
            //DataRow drBalance = DtForExportData.NewRow();
            //drBalance["TransactionDate"] = "Balance as per Bank Statement : ";
            //drBalance["AmountCredit"] = BalCredit;
            //drBalance["AmountDebit"] = Balance;
            //DtForExportData.Rows.Add(drBalance);
            //DT = oDBEngine.GetDataTable("(Select distinct (convert(varchar(11),a.cashbank_transactionDate,113))as TransactionDate,a.cashbank_vouchernumber,(convert(varchar(11),b.cashbankdetail_instrumentdate,113))as cashbankdetail_instrumentdate,b.cashbankdetail_instrumentdate as cashbankdetail_instrumentdate_test,b.cashbankdetail_instrumentnumber,c.mainaccount_name +' ['+isnull((isnull(rtrim(ltrim(d.subaccount_name)),'')+'['+ (isnull((select isnull(ltrim(rtrim(cnt_ucc)),'') from tbl_master_contact where cnt_internalid=d.subaccount_code),(select top 1 subaccount_code from master_subaccount where cast(subaccount_code as varchar)=d.subaccount_code and subaccount_mainacreferenceid=b.cashbankdetail_mainaccountid)))+']'),'') +']' as AccountCode,case b.cashbankdetail_paymentamount when 0 then null else cast(b.cashbankdetail_paymentamount as money) end as cashbankdetail_paymentamount,case b.cashbankdetail_receiptamount when 0 then null else cast(b.cashbankdetail_receiptamount as money) end as cashbankdetail_receiptamount,case b.cashbankdetail_bankvaluedate when '1900-01-01 00:00:00.000' then null else convert(varchar(6),cashbankdetail_bankvaluedate,105)+substring(convert(varchar(10),cashbankdetail_bankvaluedate,101),7,4)end cashbankdetail_bankvaluedate,b.cashbankdetail_id,cashbankdetail_voucherid ,d.subaccount_code,b.cashbankdetail_mainaccountid,case when cashbankdetail_cashbankid is null then (select e.cashbankdetail_mainaccountid from trans_cashbankdetail as e where e.cashbankdetail_mainaccountid <>'" + txtBankName_hidden.Value.ToString() + "' and e.cashbankdetail_voucherid=b.cashbankdetail_voucherid and cashbankdetail_cashbankid is null) else b.cashbankdetail_mainaccountid end as test,d.subaccount_name  from Trans_CashBankVouchers  as a inner join Trans_CashBankDetail as b on a.cashbank_id=b.cashbankdetail_voucherid inner join master_mainaccount  as c on b.cashbankdetail_mainaccountid=c.MAinaccount_accountcode inner join master_subaccount as d  on b.cashbankdetail_subAccountid=d.subaccount_code WHERE  b.cashbankdetail_bankvaluedate>='" + FromDate.Value.ToString() + "'  and b.cashbankdetail_bankvaluedate<='" + DateTo.Value.ToString() + "' and (b.cashbankdetail_cashbankid='" + txtBankName_hidden.Value.ToString() + "' or b.cashbankdetail_mainaccountid='" + txtBankName_hidden.Value.ToString() + "')AND (subaccount_mainacreferenceid=b.cashbankdetail_mainaccountid or cashbankdetail_Subaccountid='' or cashbankdetail_Subaccountid is null))as d", "d.TransactionDate,d.cashbank_vouchernumber,d.cashbankdetail_instrumentdate,d.cashbankdetail_instrumentnumber,case substring(d.cashbank_vouchernumber,1,1) when 'C' then (select mainaccount_name from master_mainaccount where mainaccount_accountcode= d.test)  else d.AccountCode end as AccountCode,d.cashbankdetail_paymentamount,d.cashbankdetail_receiptamount,d.cashbankdetail_id,d.cashbankdetail_voucherid ,d.subaccount_code,d.subaccount_name", null, "cashbank_transactionDate_test,cashbankdetail_instrumentnumber");


            DT = (DataTable)ViewState["DTExport"];//oDBEngine.GetDataTable("(Select distinct case b.cashbankdetail_instrumenttype when 'E' then 'E-Payment' when 'C' then 'Cheque' else 'Not applicable' end as type1 ,(convert(varchar(11),b.cashbankdetail_bankstatementdate,113))as bankstatementdate,(convert(varchar(11),a.cashbank_transactionDate,113))as TransactionDate,a.cashbank_vouchernumber,a.cashbank_transactionDate,(convert(varchar(11),b.cashbankdetail_instrumentdate,113))as cashbankdetail_instrumentdate,b.cashbankdetail_instrumentdate as cashbankdetail_instrumentdate_test,b.cashbankdetail_instrumentnumber,c.mainaccount_name +' ['+isnull((isnull(rtrim(ltrim(d.subaccount_name)),'')+'['+ (isnull((select isnull(ltrim(rtrim(cnt_ucc)),'') from tbl_master_contact where cnt_internalid=d.subaccount_code),(select top 1 subaccount_code from master_subaccount where cast(subaccount_code as varchar)=d.subaccount_code and subaccount_mainacreferenceid=b.cashbankdetail_mainaccountid)))+']'),'') +']' as AccountCode,case b.cashbankdetail_paymentamount when 0 then null else cast(b.cashbankdetail_paymentamount as money) end as cashbankdetail_paymentamount,case b.cashbankdetail_receiptamount when 0 then null else cast(b.cashbankdetail_receiptamount as money) end as cashbankdetail_receiptamount,case b.cashbankdetail_bankvaluedate when '1900-01-01 00:00:00.000' then null else convert(varchar(6),cashbankdetail_bankvaluedate,105)+substring(convert(varchar(10),cashbankdetail_bankvaluedate,101),7,4)end cashbankdetail_bankvaluedate,b.cashbankdetail_id,cashbankdetail_voucherid ,d.subaccount_code,b.cashbankdetail_mainaccountid,case when cashbankdetail_cashbankid is null then (select e.cashbankdetail_mainaccountid from trans_cashbankdetail as e where e.cashbankdetail_mainaccountid <>'" + txtBankName_hidden.Value.ToString() + "' and e.cashbankdetail_voucherid=b.cashbankdetail_voucherid and cashbankdetail_cashbankid is null) else b.cashbankdetail_mainaccountid end as test,d.subaccount_name  from Trans_CashBankVouchers  as a inner join Trans_CashBankDetail as b on a.cashbank_id=b.cashbankdetail_voucherid inner join master_mainaccount  as c on b.cashbankdetail_mainaccountid=c.MAinaccount_accountcode inner join master_subaccount as d  on b.cashbankdetail_subAccountid=d.subaccount_code WHERE  b.cashbankdetail_bankvaluedate>='" + FromDate.Value.ToString() + "'  and b.cashbankdetail_bankvaluedate<='" + DateTo.Value.ToString() + "' and (b.cashbankdetail_cashbankid='" + txtBankName_hidden.Value.ToString() + "' or b.cashbankdetail_mainaccountid='" + txtBankName_hidden.Value.ToString() + "')AND (subaccount_mainacreferenceid=b.cashbankdetail_mainaccountid or cashbankdetail_Subaccountid='' or cashbankdetail_Subaccountid is null))as d", "d.TransactionDate AS [TRANSACTION DATE],d.cashbank_vouchernumber AS [VOUCHER NUMBER],d.cashbankdetail_instrumentdate AS [INSRTUMENT DATE],d.cashbankdetail_instrumentnumber AS [INSTRUMENT NUMBER],d.type1 AS TYPE,case substring(d.cashbank_vouchernumber,1,1) when 'C' then (select mainaccount_name from master_mainaccount where mainaccount_accountcode= d.test)  else d.AccountCode end as [ACCOUNT CODE],d.cashbankdetail_paymentamount AS [PAYMENT AMOUNT],d.cashbankdetail_receiptamount AS [RECEIPT AMOUNT],d.bankstatementdate as [STATEMENT DATE]", null, "D.cashbank_transactionDate,cashbankdetail_instrumentnumber");
            if (DT.Rows.Count > 0)
            {
                DataRow newRow = DT.NewRow();
                newRow[0] = "Total";
                newRow[6] = ObjConvert.getFormattedvalue(Convert.ToDecimal(ViewState["PaymentAmount"].ToString()));
                newRow[7] = ObjConvert.getFormattedvalue(Convert.ToDecimal(ViewState["ReceiptAmount"].ToString()));
                DT.Rows.Add(newRow);

                DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
                DataTable dtReportHeader = new DataTable();
                dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
                DataRow HeaderRow = dtReportHeader.NewRow();
                HeaderRow[0] = CompanyName.Rows[0][0].ToString();
                dtReportHeader.Rows.Add(HeaderRow);
                DataRow DrRowR1 = dtReportHeader.NewRow();
                DrRowR1[0] = "Bank Statement [From " + ObjConvert.ArrangeDate2(FromDate.Value.ToString()) + " To " + ObjConvert.ArrangeDate2(DateTo.Value.ToString()) + "]" + " for " + txtBankName.Text.ToString();
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
                    objExcel.ExportToExcelforExcel(DT, "Bank Statement", "Total", dtReportHeader, dtReportFooter);
                }
                else if (ddlExport.SelectedItem.Value == "P")
                {
                    objExcel.ExportToPDF(DT, "Bank Statement", "Total", dtReportHeader, dtReportFooter);
                }
            }
        }
    }
}