using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;
using System.Web.Services;
using System.Collections.Generic;
using DevExpress.Web;
using DevExpress.Data;
using System.Web.UI.HtmlControls;
using System.Linq;
using System.Globalization;

namespace ERP.OMS.Management.DailyTask
{
    public partial class management_DailyTask_frm_ManualBRSwithGrid : ERP.OMS.ViewState_class.VSPage
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        DataTable DT = new DataTable();
        DataTable DT1 = new DataTable();
        FinancialAccounting oFinancialAccounting = new FinancialAccounting();

        BusinessLogicLayer.Converter ObjConvert = new BusinessLogicLayer.Converter();
        string CheckStatus = "";


        protected void Page_Init(object sender, EventArgs e)
        {
            //string fDate = null;
            //string FinYear = Convert.ToString(Session["FinYearEnd"]);
            //int month = oDBEngine.GetDate().Month;
            //int date = oDBEngine.GetDate().Day;
            //int Year = oDBEngine.GetDate().Year;

            //if (Convert.ToDateTime(FinYear).Date < oDBEngine.GetDate().Date)
            //{
            //    fDate = Convert.ToDateTime(FinYear).Month.ToString() + "/" + Convert.ToDateTime(FinYear).Day.ToString() + "/" + Convert.ToDateTime(FinYear).Year.ToString();
            //    DateTo.Date = Convert.ToDateTime(FinYear).Date;
            //    //DateTo.Date = Convert.ToDateTime(fDate);
            //    FromDate.Date = Convert.ToDateTime(FinYear).Date.AddDays(-15);
            //    //FromDate.Date = Convert.ToDateTime(fDate).AddDays(-15);
            //}
            //else
            //{
            //    DateTo.Date = oDBEngine.GetDate().Date;
            //    FromDate.Date = oDBEngine.GetDate().AddDays(-15);
            //}

            //BindTable();


        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string fDate = null;
                string FinYear = Convert.ToString(Session["FinYearEnd"]);
                int month = oDBEngine.GetDate().Month;
                int date = oDBEngine.GetDate().Day;
                int Year = oDBEngine.GetDate().Year;
                btnUpdate.Visible = false;
                btnCancel.Visible = false;
                txtBankName_hidden.Value = null;
                if (FinYear == "")
                {

                }
                else if (Convert.ToDateTime(FinYear).Date < oDBEngine.GetDate().Date)
                {
                    fDate = Convert.ToDateTime(FinYear).Month.ToString() + "/" + Convert.ToDateTime(FinYear).Day.ToString() + "/" + Convert.ToDateTime(FinYear).Year.ToString();
                    DateTo.Date = Convert.ToDateTime(FinYear).Date;
                    //DateTo.Date = Convert.ToDateTime(fDate);
                    FromDate.Date = Convert.ToDateTime(FinYear).Date.AddDays(-15);
                    //FromDate.Date = Convert.ToDateTime(fDate).AddDays(-15);
                }
                else
                {
                    DateTo.Date = oDBEngine.GetDate().Date;
                    FromDate.Date = oDBEngine.GetDate().AddDays(-15);
                }


                //BindTable();
                Page.ClientScript.RegisterStartupScript(GetType(), "ForSearch", "<script language='Javascript'>SearchVisible('N');</script>");
                //added 06-09-2017
                dtdstatedateall.Date = oDBEngine.GetDate().Date;


            }
            Page.ClientScript.RegisterStartupScript(GetType(), "HeightCalling", "<script language='Javascript'>height();</script>");
            Page.ClientScript.RegisterStartupScript(GetType(), "PageLoadCalling", "<script language='Javascript'>Page_Laod();</script>");

            //Find Segment For NSDL/CDSL
            DataTable DtCurrentSegment = new DataTable();
            DtCurrentSegment = oDBEngine.GetDataTable("(select exch_internalId, isnull((select top 1 exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId),exch_membershiptype) as Segment from tbl_master_companyExchange where exch_compid in (select top 1 ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment=" + Convert.ToString(Session["userlastsegment"]) + " and ls_userid=" + Convert.ToString(Session["UserID"]) + ") and exch_compId='" + Convert.ToString(HttpContext.Current.Session["LastCompany"]) + "') as D", "*", " Segment in(select seg_name from tbl_master_segment where seg_id=" + Convert.ToString(Session["userlastsegment"]) + ")");

            hdn_CurrentSegment.Value = "1";// Convert.ToString(DtCurrentSegment.Rows[0][0]);


        }
        public void BindTable()
        {
            DataSet DsBind = new DataSet();
            if (RdAll.Checked == true)
            {
                CheckStatus = "A";
            }
            else if (RdUnCleared.Checked == true)
            {
                if (ChkConsiderAllDate.Checked)
                    CheckStatus = "UA";
                else
                    CheckStatus = "U";

            }
            else if (RdCleared.Checked == true)
            {
                CheckStatus = "C";
            }
            //if (CheckStatus == "U")
            //{
            //    //DT = oDBEngine.GetDataTable("Trans_CashBankVouchers  as a inner join Trans_CashBankDetail as b on a.cashbank_id=b.cashbankdetail_voucherid inner join master_mainaccount as c on b.cashbankdetail_mainaccountid=c.MAinaccount_accountcode inner join master_subaccount as d on b.cashbankdetail_subAccountid=d.subaccount_code", "distinct a.cashbank_transactionDate as cashbank_transactionDate_test,(convert(varchar(11),a.cashbank_transactionDate,113))as cashbank_transactionDate,a.cashbank_vouchernumber,(convert(varchar(11),b.cashbankdetail_instrumentdate,113))as cashbankdetail_instrumentdate,b.cashbankdetail_instrumentdate as cashbankdetail_instrumentdate_test,b.cashbankdetail_instrumentnumber,c.mainaccount_name +' ['+isnull(d.subaccount_name,'')+'['+ (isnull((select isnull(ltrim(rtrim(cnt_ucc)),'') from tbl_master_contact where cnt_internalid=d.subaccount_code),(select top 1 subaccount_code from master_subaccount where cast(subaccount_code as varchar)=d.subaccount_code)))+']' +']' as AccountCode,case b.cashbankdetail_paymentamount when 0 then null else cast(b.cashbankdetail_paymentamount as money) end as cashbankdetail_paymentamount,case b.cashbankdetail_receiptamount when 0 then null else cast(b.cashbankdetail_receiptamount as money) end as cashbankdetail_receiptamount,case b.cashbankdetail_bankvaluedate when '1900-01-01 00:00:00.000' then null else convert(varchar(6),cashbankdetail_bankvaluedate,105)+substring(convert(varchar(10),cashbankdetail_bankvaluedate,101),7,4)end cashbankdetail_bankvaluedate,b.cashbankdetail_id,d.subaccount_code,b.cashbankdetail_mainaccountid,d.subaccount_name ", " a.cashbank_transactionDate>='" + FromDate.Value + "' and a.cashbank_transactionDate<='" + DateTo.Value + "' and (b.cashbankdetail_bankvaluedate is null or b.cashbankdetail_bankvaluedate ='1900-01-01 00:00:00.000') and (b.cashbankdetail_cashbankid='" + txtBankName_hidden.Value.ToString() + "' or b.cashbankdetail_mainaccountid='" + txtBankName_hidden.Value.ToString() + "')", "cashbank_transactionDate_test");

            //    DT = oDBEngine.GetDataTable("(Select distinct a.cashbank_transactionDate as cashbank_transactionDate_test,(convert(varchar(11),a.cashbank_transactionDate,113))as cashbank_transactionDate,a.cashbank_vouchernumber,(convert(varchar(11),b.cashbankdetail_instrumentdate,113))as cashbankdetail_instrumentdate,b.cashbankdetail_instrumentdate as cashbankdetail_instrumentdate_test,b.cashbankdetail_instrumentnumber,c.mainaccount_name +' ['+isnull((isnull(rtrim(ltrim(d.subaccount_name)),'')+'['+ (isnull((select isnull(ltrim(rtrim(cnt_ucc)),'') from tbl_master_contact where cnt_internalid=d.subaccount_code),(select top 1 subaccount_code from master_subaccount where cast(subaccount_code as varchar)=d.subaccount_code and subaccount_mainacreferenceid=b.cashbankdetail_mainaccountid)))+']'),'') +']' as AccountCode,case b.cashbankdetail_paymentamount when 0 then null else cast(b.cashbankdetail_paymentamount as money) end as cashbankdetail_paymentamount,case b.cashbankdetail_receiptamount when 0 then null else cast(b.cashbankdetail_receiptamount as money) end as cashbankdetail_receiptamount,case b.cashbankdetail_bankvaluedate when '1900-01-01 00:00:00.000' then null else convert(varchar(6),cashbankdetail_bankvaluedate,105)+substring(convert(varchar(10),cashbankdetail_bankvaluedate,101),7,4)end cashbankdetail_bankvaluedate,case b.cashbankdetail_bankstatementdate when '1900-01-01 00:00:00.000' then null else convert(varchar(6),cashbankdetail_bankstatementdate,105)+substring(convert(varchar(10),cashbankdetail_bankstatementdate,101),7,4)end cashbankdetail_bankstatementdate,b.cashbankdetail_id,cashbankdetail_voucherid ,d.subaccount_code,b.cashbankdetail_mainaccountid,case when cashbankdetail_cashbankid is null then (select top 1 e.cashbankdetail_mainaccountid from trans_cashbankdetail as e where e.cashbankdetail_mainaccountid <>'" + txtBankName_hidden.Value.ToString() + "' and e.cashbankdetail_voucherid=b.cashbankdetail_voucherid and cashbankdetail_cashbankid is null) else b.cashbankdetail_mainaccountid end as test,d.subaccount_name  from Trans_CashBankVouchers  as a inner join Trans_CashBankDetail as b on a.cashbank_id=b.cashbankdetail_voucherid inner join master_mainaccount  as c on b.cashbankdetail_mainaccountid=c.MAinaccount_accountcode inner join master_subaccount as d  on b.cashbankdetail_subAccountid=d.subaccount_code WHERE  a.cashbank_transactionDate>='" + FromDate.Value.ToString() + "'  and a.cashbank_transactionDate<='" + DateTo.Value.ToString() + "' and (b.cashbankdetail_cashbankid='" + txtBankName_hidden.Value.ToString() + "' or b.cashbankdetail_mainaccountid='" + txtBankName_hidden.Value.ToString() + "')and(b.cashbankdetail_bankvaluedate is  null  or b.cashbankdetail_bankvaluedate ='1900-01-01 00:00:00.000')AND (subaccount_mainacreferenceid=b.cashbankdetail_mainaccountid or cashbankdetail_Subaccountid='' or cashbankdetail_Subaccountid is null))as d", "d.cashbank_transactionDate_test,d.cashbank_transactionDate,d.cashbank_vouchernumber,d.cashbankdetail_instrumentdate,d.cashbankdetail_instrumentdate_test,d.cashbankdetail_instrumentnumber,case substring(d.cashbank_vouchernumber,1,1) when 'C' then (select top 1 mainaccount_name from master_mainaccount where mainaccount_accountcode= d.test)  else d.AccountCode end as AccountCode,d.cashbankdetail_paymentamount,d.cashbankdetail_receiptamount,d.cashbankdetail_bankvaluedate,d.cashbankdetail_id,d.cashbankdetail_voucherid ,d.subaccount_code,d.subaccount_name,cashbankdetail_mainaccountid,d.cashbankdetail_bankstatementdate", null, "cashbank_transactionDate_test,cashbank_vouchernumber,cashbankdetail_instrumentnumber");
            //}
            //else if (CheckStatus == "C")
            //{
            //    //DT = oDBEngine.GetDataTable("Trans_CashBankVouchers  as a inner join Trans_CashBankDetail as b on a.cashbank_id=b.cashbankdetail_voucherid inner join master_mainaccount as c on b.cashbankdetail_mainaccountid=c.MAinaccount_accountcode inner join master_subaccount as d on b.cashbankdetail_subAccountid=d.subaccount_code", "distinct a.cashbank_transactionDate as cashbank_transactionDate_test,(convert(varchar(11),a.cashbank_transactionDate,113))as cashbank_transactionDate,a.cashbank_vouchernumber,(convert(varchar(11),b.cashbankdetail_instrumentdate,113))as cashbankdetail_instrumentdate,b.cashbankdetail_instrumentdate as cashbankdetail_instrumentdate_test,b.cashbankdetail_instrumentnumber,c.mainaccount_name +' ['+isnull(d.subaccount_name,'')+'['+ (isnull((select isnull(ltrim(rtrim(cnt_ucc)),'') from tbl_master_contact where cnt_internalid=d.subaccount_code),(select top 1 subaccount_code from master_subaccount where cast(subaccount_code as varchar)=d.subaccount_code)))+']' +']' as AccountCode,case b.cashbankdetail_paymentamount when 0 then null else cast(b.cashbankdetail_paymentamount as money) end as cashbankdetail_paymentamount,case b.cashbankdetail_receiptamount when 0 then null else cast(b.cashbankdetail_receiptamount as money) end as cashbankdetail_receiptamount,case b.cashbankdetail_bankvaluedate when '1900-01-01 00:00:00.000' then null else convert(varchar(6),cashbankdetail_bankvaluedate,105)+substring(convert(varchar(10),cashbankdetail_bankvaluedate,101),7,4)end cashbankdetail_bankvaluedate,b.cashbankdetail_id,d.subaccount_code,b.cashbankdetail_mainaccountid,d.subaccount_name ", " a.cashbank_transactionDate>='" + FromDate.Value + "' and a.cashbank_transactionDate<='" + DateTo.Value + "' and (b.cashbankdetail_bankvaluedate is not null  and b.cashbankdetail_bankvaluedate <>'1900-01-01 00:00:00.000') and (b.cashbankdetail_cashbankid='" + txtBankName_hidden.Value.ToString() + "' or b.cashbankdetail_mainaccountid='" + txtBankName_hidden.Value.ToString() + "')", "cashbank_transactionDate_test");
            //    DT = oDBEngine.GetDataTable("(Select distinct a.cashbank_transactionDate as cashbank_transactionDate_test,(convert(varchar(11),a.cashbank_transactionDate,113))as cashbank_transactionDate,a.cashbank_vouchernumber,(convert(varchar(11),b.cashbankdetail_instrumentdate,113))as cashbankdetail_instrumentdate,b.cashbankdetail_instrumentdate as cashbankdetail_instrumentdate_test,b.cashbankdetail_instrumentnumber,c.mainaccount_name +' ['+isnull((isnull(rtrim(ltrim(d.subaccount_name)),'')+'['+ (isnull((select isnull(ltrim(rtrim(cnt_ucc)),'') from tbl_master_contact where cnt_internalid=d.subaccount_code),(select top 1 subaccount_code from master_subaccount where cast(subaccount_code as varchar)=d.subaccount_code and subaccount_mainacreferenceid=b.cashbankdetail_mainaccountid)))+']'),'') +']' as AccountCode,case b.cashbankdetail_paymentamount when 0 then null else cast(b.cashbankdetail_paymentamount as money) end as cashbankdetail_paymentamount,case b.cashbankdetail_receiptamount when 0 then null else cast(b.cashbankdetail_receiptamount as money) end as cashbankdetail_receiptamount,case b.cashbankdetail_bankvaluedate when '1900-01-01 00:00:00.000' then null else convert(varchar(6),cashbankdetail_bankvaluedate,105)+substring(convert(varchar(10),cashbankdetail_bankvaluedate,101),7,4)end cashbankdetail_bankvaluedate,case b.cashbankdetail_bankstatementdate when '1900-01-01 00:00:00.000' then null else convert(varchar(6),cashbankdetail_bankstatementdate,105)+substring(convert(varchar(10),cashbankdetail_bankstatementdate,101),7,4)end cashbankdetail_bankstatementdate,b.cashbankdetail_id,cashbankdetail_voucherid ,d.subaccount_code,b.cashbankdetail_mainaccountid,case when cashbankdetail_cashbankid is null then (select top 1 e.cashbankdetail_mainaccountid from trans_cashbankdetail as e where e.cashbankdetail_mainaccountid <>'" + txtBankName_hidden.Value.ToString() + "' and e.cashbankdetail_voucherid=b.cashbankdetail_voucherid and cashbankdetail_cashbankid is null) else b.cashbankdetail_mainaccountid end as test,d.subaccount_name  from Trans_CashBankVouchers  as a inner join Trans_CashBankDetail as b on a.cashbank_id=b.cashbankdetail_voucherid inner join master_mainaccount  as c on b.cashbankdetail_mainaccountid=c.MAinaccount_accountcode inner join master_subaccount as d  on b.cashbankdetail_subAccountid=d.subaccount_code WHERE  a.cashbank_transactionDate>='" + FromDate.Value.ToString() + "'  and a.cashbank_transactionDate<='" + DateTo.Value.ToString() + "' and (b.cashbankdetail_cashbankid='" + txtBankName_hidden.Value.ToString() + "' or b.cashbankdetail_mainaccountid='" + txtBankName_hidden.Value.ToString() + "')and(b.cashbankdetail_bankvaluedate is  not null  or b.cashbankdetail_bankvaluedate<>'1900-01-01 00:00:00.000')AND (subaccount_mainacreferenceid=b.cashbankdetail_mainaccountid or cashbankdetail_Subaccountid='' or cashbankdetail_Subaccountid is null))as d", "d.cashbank_transactionDate_test,d.cashbank_transactionDate,d.cashbank_vouchernumber,d.cashbankdetail_instrumentdate,d.cashbankdetail_instrumentdate_test,d.cashbankdetail_instrumentnumber,case substring(d.cashbank_vouchernumber,1,1) when 'C' then (select top 1 mainaccount_name from master_mainaccount where mainaccount_accountcode= d.test)  else d.AccountCode end as AccountCode,d.cashbankdetail_paymentamount,d.cashbankdetail_receiptamount,d.cashbankdetail_bankvaluedate,d.cashbankdetail_id,d.cashbankdetail_voucherid ,d.subaccount_code,d.subaccount_name,cashbankdetail_mainaccountid,d.cashbankdetail_bankstatementdate", "(d.cashbankdetail_bankvaluedate is  not null  or d.cashbankdetail_bankvaluedate<>'1900-01-01 00:00:00.000')", "cashbank_transactionDate_test,cashbank_vouchernumber,cashbankdetail_instrumentnumber");
            //}
            //else if (CheckStatus == "A")
            //{
            //    DT = oDBEngine.GetDataTable("(Select distinct a.cashbank_transactionDate as cashbank_transactionDate_test,(convert(varchar(11),a.cashbank_transactionDate,113))as cashbank_transactionDate,a.cashbank_vouchernumber,(convert(varchar(11),b.cashbankdetail_instrumentdate,113))as cashbankdetail_instrumentdate,b.cashbankdetail_instrumentdate as cashbankdetail_instrumentdate_test,b.cashbankdetail_instrumentnumber,c.mainaccount_name +' ['+isnull((isnull(rtrim(ltrim(d.subaccount_name)),'')+'['+ (isnull((select isnull(ltrim(rtrim(cnt_ucc)),'') from tbl_master_contact where cnt_internalid=d.subaccount_code),(select top 1 subaccount_code from master_subaccount where cast(subaccount_code as varchar)=d.subaccount_code and subaccount_mainacreferenceid=b.cashbankdetail_mainaccountid)))+']'),'') +']' as AccountCode,case b.cashbankdetail_paymentamount when 0 then null else cast(b.cashbankdetail_paymentamount as money) end as cashbankdetail_paymentamount,case b.cashbankdetail_receiptamount when 0 then null else cast(b.cashbankdetail_receiptamount as money) end as cashbankdetail_receiptamount,case b.cashbankdetail_bankvaluedate when '1900-01-01 00:00:00.000' then null else convert(varchar(6),cashbankdetail_bankvaluedate,105)+substring(convert(varchar(10),cashbankdetail_bankvaluedate,101),7,4)end cashbankdetail_bankvaluedate,case b.cashbankdetail_bankstatementdate when '1900-01-01 00:00:00.000' then null else convert(varchar(6),cashbankdetail_bankstatementdate,105)+substring(convert(varchar(10),cashbankdetail_bankstatementdate,101),7,4)end cashbankdetail_bankstatementdate,b.cashbankdetail_id,cashbankdetail_voucherid ,d.subaccount_code,b.cashbankdetail_mainaccountid,case when cashbankdetail_cashbankid is null then (select top 1 e.cashbankdetail_mainaccountid from trans_cashbankdetail as e where e.cashbankdetail_mainaccountid <>'" + txtBankName_hidden.Value.ToString() + "' and e.cashbankdetail_voucherid=b.cashbankdetail_voucherid and cashbankdetail_cashbankid is null) else b.cashbankdetail_mainaccountid end as test,d.subaccount_name  from Trans_CashBankVouchers  as a inner join Trans_CashBankDetail as b on a.cashbank_id=b.cashbankdetail_voucherid inner join master_mainaccount  as c on b.cashbankdetail_mainaccountid=c.MAinaccount_accountcode inner join master_subaccount as d  on b.cashbankdetail_subAccountid=d.subaccount_code WHERE  a.cashbank_transactionDate>='" + FromDate.Value.ToString() + "'  and a.cashbank_transactionDate<='" + DateTo.Value.ToString() + "' and (b.cashbankdetail_cashbankid='" + txtBankName_hidden.Value.ToString() + "' or b.cashbankdetail_mainaccountid='" + txtBankName_hidden.Value.ToString() + "')AND (subaccount_mainacreferenceid=b.cashbankdetail_mainaccountid or cashbankdetail_Subaccountid='' or cashbankdetail_Subaccountid is null))as d", "d.cashbank_transactionDate_test,d.cashbank_transactionDate,d.cashbank_vouchernumber,d.cashbankdetail_instrumentdate,d.cashbankdetail_instrumentdate_test,d.cashbankdetail_instrumentnumber,case substring(d.cashbank_vouchernumber,1,1) when 'C' then (select top 1 mainaccount_name from master_mainaccount where mainaccount_accountcode= d.test)  else d.AccountCode end as AccountCode,d.cashbankdetail_paymentamount,d.cashbankdetail_receiptamount,d.cashbankdetail_bankvaluedate,d.cashbankdetail_id,d.cashbankdetail_voucherid ,d.subaccount_code,d.subaccount_name,cashbankdetail_mainaccountid,d.cashbankdetail_bankstatementdate", null, "cashbank_transactionDate_test,cashbank_vouchernumber,cashbankdetail_instrumentnumber");
            //}  
            //DT = oDBEngine.GetDataTable("Trans_CashBankVouchers  as a inner join Trans_CashBankDetail as b on a.cashbank_id=b.cashbankdetail_voucherid inner join master_mainaccount as c on b.cashbankdetail_mainaccountid=c.MAinaccount_accountcode inner join master_subaccount as d on b.cashbankdetail_subAccountid=d.subaccount_code", "distinct a.cashbank_transactionDate as cashbank_transactionDate_test,(convert(varchar(11),a.cashbank_transactionDate,113))as cashbank_transactionDate,a.cashbank_vouchernumber,(convert(varchar(11),b.cashbankdetail_instrumentdate,113))as cashbankdetail_instrumentdate,b.cashbankdetail_instrumentdate as cashbankdetail_instrumentdate_test,b.cashbankdetail_instrumentnumber,c.mainaccount_name +'['+d.subaccount_name+'['+ (select  ltrim(rtrim(cnt_ucc)) from tbl_master_contact where cnt_internalid=d.subaccount_code )+']' +']' as AccountCode,case b.cashbankdetail_paymentamount when 0 then null else b.cashbankdetail_paymentamount end as cashbankdetail_paymentamount,case b.cashbankdetail_receiptamount when 0 then null else b.cashbankdetail_receiptamount end as cashbankdetail_receiptamount,b.cashbankdetail_bankvaluedate,b.cashbankdetail_id,d.subaccount_code,b.cashbankdetail_mainaccountid ", " a.cashbank_transactionDate>='" + FromDate.Value + "' and a.cashbank_transactionDate<='" + DateTo.Value + "' and (b.cashbankdetail_bankvaluedate is null or b.cashbankdetail_bankvaluedate ='1900-01-01 00:00:00.000') and b.cashbankdetail_cashbankid='" + txtBankName_hidden.Value.ToString() + "'", "cashbank_transactionDate");



            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            //{
            //    if (con.State == ConnectionState.Open) con.Close();

            //    using (SqlCommand com = new SqlCommand("Fetch_ManualBRS_Data", con))
            //    {
            //        com.CommandType = CommandType.StoredProcedure;
            //        com.CommandTimeout = 0;
            //        com.Parameters.AddWithValue("@TdateTo", Convert.ToDateTime(DateTo.Value).ToString("yyyy-MM-dd"));
            //        com.Parameters.AddWithValue("@TdateFrom", Convert.ToDateTime(FromDate.Value).ToString("yyyy-MM-dd"));
            //        com.Parameters.AddWithValue("@WhichRecord", CheckStatus);
            //        com.Parameters.AddWithValue("@AccountID", txtBankName_hidden.Value.ToString());
            //        con.Open();
            //        using (SqlDataAdapter Da = new SqlDataAdapter(com))
            //        {
            //            Da.Fill(DsBind);
            //        }
            //    }
            //}
            string nn = txtBankName_hidden.Value;
            string dd = Convert.ToString(DateTo.Value);

            string def = Convert.ToString(FromDate.Value);


            DsBind = oFinancialAccounting.FetchManualBRSData(Convert.ToDateTime(DateTo.Value).ToString("yyyy-MM-dd"), Convert.ToDateTime(FromDate.Value).ToString("yyyy-MM-dd"),
                CheckStatus, txtBankName_hidden.Value.ToString());

            //comment by sanjib due to grid changed
            //grdDetails.DataSource = DsBind;
            //grdDetails.DataBind();

            grdmanualBRS.DataSource = DsBind;
            grdmanualBRS.DataBind();

            if (DsBind.Tables[0].Rows.Count > 0)
            {
                trhypertext.Visible = true;
                MainContent.Visible = true;
                ViewState["TableForThePage"] = DsBind.Tables[0];
                btnUpdate.Visible = true;
                btnCancel.Visible = true;
            }

        }

        public void BindTablesbydate(string dates)
        {
            DataSet DsBind = new DataSet();
            if (RdAll.Checked == true)
            {
                CheckStatus = "A";
            }
            else if (RdUnCleared.Checked == true)
            {
                if (ChkConsiderAllDate.Checked)
                    CheckStatus = "UA";
                else
                    CheckStatus = "U";

            }
            else if (RdCleared.Checked == true)
            {
                CheckStatus = "C";
            }
            //if (CheckStatus == "U")
            //{
            //    //DT = oDBEngine.GetDataTable("Trans_CashBankVouchers  as a inner join Trans_CashBankDetail as b on a.cashbank_id=b.cashbankdetail_voucherid inner join master_mainaccount as c on b.cashbankdetail_mainaccountid=c.MAinaccount_accountcode inner join master_subaccount as d on b.cashbankdetail_subAccountid=d.subaccount_code", "distinct a.cashbank_transactionDate as cashbank_transactionDate_test,(convert(varchar(11),a.cashbank_transactionDate,113))as cashbank_transactionDate,a.cashbank_vouchernumber,(convert(varchar(11),b.cashbankdetail_instrumentdate,113))as cashbankdetail_instrumentdate,b.cashbankdetail_instrumentdate as cashbankdetail_instrumentdate_test,b.cashbankdetail_instrumentnumber,c.mainaccount_name +' ['+isnull(d.subaccount_name,'')+'['+ (isnull((select isnull(ltrim(rtrim(cnt_ucc)),'') from tbl_master_contact where cnt_internalid=d.subaccount_code),(select top 1 subaccount_code from master_subaccount where cast(subaccount_code as varchar)=d.subaccount_code)))+']' +']' as AccountCode,case b.cashbankdetail_paymentamount when 0 then null else cast(b.cashbankdetail_paymentamount as money) end as cashbankdetail_paymentamount,case b.cashbankdetail_receiptamount when 0 then null else cast(b.cashbankdetail_receiptamount as money) end as cashbankdetail_receiptamount,case b.cashbankdetail_bankvaluedate when '1900-01-01 00:00:00.000' then null else convert(varchar(6),cashbankdetail_bankvaluedate,105)+substring(convert(varchar(10),cashbankdetail_bankvaluedate,101),7,4)end cashbankdetail_bankvaluedate,b.cashbankdetail_id,d.subaccount_code,b.cashbankdetail_mainaccountid,d.subaccount_name ", " a.cashbank_transactionDate>='" + FromDate.Value + "' and a.cashbank_transactionDate<='" + DateTo.Value + "' and (b.cashbankdetail_bankvaluedate is null or b.cashbankdetail_bankvaluedate ='1900-01-01 00:00:00.000') and (b.cashbankdetail_cashbankid='" + txtBankName_hidden.Value.ToString() + "' or b.cashbankdetail_mainaccountid='" + txtBankName_hidden.Value.ToString() + "')", "cashbank_transactionDate_test");

            //    DT = oDBEngine.GetDataTable("(Select distinct a.cashbank_transactionDate as cashbank_transactionDate_test,(convert(varchar(11),a.cashbank_transactionDate,113))as cashbank_transactionDate,a.cashbank_vouchernumber,(convert(varchar(11),b.cashbankdetail_instrumentdate,113))as cashbankdetail_instrumentdate,b.cashbankdetail_instrumentdate as cashbankdetail_instrumentdate_test,b.cashbankdetail_instrumentnumber,c.mainaccount_name +' ['+isnull((isnull(rtrim(ltrim(d.subaccount_name)),'')+'['+ (isnull((select isnull(ltrim(rtrim(cnt_ucc)),'') from tbl_master_contact where cnt_internalid=d.subaccount_code),(select top 1 subaccount_code from master_subaccount where cast(subaccount_code as varchar)=d.subaccount_code and subaccount_mainacreferenceid=b.cashbankdetail_mainaccountid)))+']'),'') +']' as AccountCode,case b.cashbankdetail_paymentamount when 0 then null else cast(b.cashbankdetail_paymentamount as money) end as cashbankdetail_paymentamount,case b.cashbankdetail_receiptamount when 0 then null else cast(b.cashbankdetail_receiptamount as money) end as cashbankdetail_receiptamount,case b.cashbankdetail_bankvaluedate when '1900-01-01 00:00:00.000' then null else convert(varchar(6),cashbankdetail_bankvaluedate,105)+substring(convert(varchar(10),cashbankdetail_bankvaluedate,101),7,4)end cashbankdetail_bankvaluedate,case b.cashbankdetail_bankstatementdate when '1900-01-01 00:00:00.000' then null else convert(varchar(6),cashbankdetail_bankstatementdate,105)+substring(convert(varchar(10),cashbankdetail_bankstatementdate,101),7,4)end cashbankdetail_bankstatementdate,b.cashbankdetail_id,cashbankdetail_voucherid ,d.subaccount_code,b.cashbankdetail_mainaccountid,case when cashbankdetail_cashbankid is null then (select top 1 e.cashbankdetail_mainaccountid from trans_cashbankdetail as e where e.cashbankdetail_mainaccountid <>'" + txtBankName_hidden.Value.ToString() + "' and e.cashbankdetail_voucherid=b.cashbankdetail_voucherid and cashbankdetail_cashbankid is null) else b.cashbankdetail_mainaccountid end as test,d.subaccount_name  from Trans_CashBankVouchers  as a inner join Trans_CashBankDetail as b on a.cashbank_id=b.cashbankdetail_voucherid inner join master_mainaccount  as c on b.cashbankdetail_mainaccountid=c.MAinaccount_accountcode inner join master_subaccount as d  on b.cashbankdetail_subAccountid=d.subaccount_code WHERE  a.cashbank_transactionDate>='" + FromDate.Value.ToString() + "'  and a.cashbank_transactionDate<='" + DateTo.Value.ToString() + "' and (b.cashbankdetail_cashbankid='" + txtBankName_hidden.Value.ToString() + "' or b.cashbankdetail_mainaccountid='" + txtBankName_hidden.Value.ToString() + "')and(b.cashbankdetail_bankvaluedate is  null  or b.cashbankdetail_bankvaluedate ='1900-01-01 00:00:00.000')AND (subaccount_mainacreferenceid=b.cashbankdetail_mainaccountid or cashbankdetail_Subaccountid='' or cashbankdetail_Subaccountid is null))as d", "d.cashbank_transactionDate_test,d.cashbank_transactionDate,d.cashbank_vouchernumber,d.cashbankdetail_instrumentdate,d.cashbankdetail_instrumentdate_test,d.cashbankdetail_instrumentnumber,case substring(d.cashbank_vouchernumber,1,1) when 'C' then (select top 1 mainaccount_name from master_mainaccount where mainaccount_accountcode= d.test)  else d.AccountCode end as AccountCode,d.cashbankdetail_paymentamount,d.cashbankdetail_receiptamount,d.cashbankdetail_bankvaluedate,d.cashbankdetail_id,d.cashbankdetail_voucherid ,d.subaccount_code,d.subaccount_name,cashbankdetail_mainaccountid,d.cashbankdetail_bankstatementdate", null, "cashbank_transactionDate_test,cashbank_vouchernumber,cashbankdetail_instrumentnumber");
            //}
            //else if (CheckStatus == "C")
            //{
            //    //DT = oDBEngine.GetDataTable("Trans_CashBankVouchers  as a inner join Trans_CashBankDetail as b on a.cashbank_id=b.cashbankdetail_voucherid inner join master_mainaccount as c on b.cashbankdetail_mainaccountid=c.MAinaccount_accountcode inner join master_subaccount as d on b.cashbankdetail_subAccountid=d.subaccount_code", "distinct a.cashbank_transactionDate as cashbank_transactionDate_test,(convert(varchar(11),a.cashbank_transactionDate,113))as cashbank_transactionDate,a.cashbank_vouchernumber,(convert(varchar(11),b.cashbankdetail_instrumentdate,113))as cashbankdetail_instrumentdate,b.cashbankdetail_instrumentdate as cashbankdetail_instrumentdate_test,b.cashbankdetail_instrumentnumber,c.mainaccount_name +' ['+isnull(d.subaccount_name,'')+'['+ (isnull((select isnull(ltrim(rtrim(cnt_ucc)),'') from tbl_master_contact where cnt_internalid=d.subaccount_code),(select top 1 subaccount_code from master_subaccount where cast(subaccount_code as varchar)=d.subaccount_code)))+']' +']' as AccountCode,case b.cashbankdetail_paymentamount when 0 then null else cast(b.cashbankdetail_paymentamount as money) end as cashbankdetail_paymentamount,case b.cashbankdetail_receiptamount when 0 then null else cast(b.cashbankdetail_receiptamount as money) end as cashbankdetail_receiptamount,case b.cashbankdetail_bankvaluedate when '1900-01-01 00:00:00.000' then null else convert(varchar(6),cashbankdetail_bankvaluedate,105)+substring(convert(varchar(10),cashbankdetail_bankvaluedate,101),7,4)end cashbankdetail_bankvaluedate,b.cashbankdetail_id,d.subaccount_code,b.cashbankdetail_mainaccountid,d.subaccount_name ", " a.cashbank_transactionDate>='" + FromDate.Value + "' and a.cashbank_transactionDate<='" + DateTo.Value + "' and (b.cashbankdetail_bankvaluedate is not null  and b.cashbankdetail_bankvaluedate <>'1900-01-01 00:00:00.000') and (b.cashbankdetail_cashbankid='" + txtBankName_hidden.Value.ToString() + "' or b.cashbankdetail_mainaccountid='" + txtBankName_hidden.Value.ToString() + "')", "cashbank_transactionDate_test");
            //    DT = oDBEngine.GetDataTable("(Select distinct a.cashbank_transactionDate as cashbank_transactionDate_test,(convert(varchar(11),a.cashbank_transactionDate,113))as cashbank_transactionDate,a.cashbank_vouchernumber,(convert(varchar(11),b.cashbankdetail_instrumentdate,113))as cashbankdetail_instrumentdate,b.cashbankdetail_instrumentdate as cashbankdetail_instrumentdate_test,b.cashbankdetail_instrumentnumber,c.mainaccount_name +' ['+isnull((isnull(rtrim(ltrim(d.subaccount_name)),'')+'['+ (isnull((select isnull(ltrim(rtrim(cnt_ucc)),'') from tbl_master_contact where cnt_internalid=d.subaccount_code),(select top 1 subaccount_code from master_subaccount where cast(subaccount_code as varchar)=d.subaccount_code and subaccount_mainacreferenceid=b.cashbankdetail_mainaccountid)))+']'),'') +']' as AccountCode,case b.cashbankdetail_paymentamount when 0 then null else cast(b.cashbankdetail_paymentamount as money) end as cashbankdetail_paymentamount,case b.cashbankdetail_receiptamount when 0 then null else cast(b.cashbankdetail_receiptamount as money) end as cashbankdetail_receiptamount,case b.cashbankdetail_bankvaluedate when '1900-01-01 00:00:00.000' then null else convert(varchar(6),cashbankdetail_bankvaluedate,105)+substring(convert(varchar(10),cashbankdetail_bankvaluedate,101),7,4)end cashbankdetail_bankvaluedate,case b.cashbankdetail_bankstatementdate when '1900-01-01 00:00:00.000' then null else convert(varchar(6),cashbankdetail_bankstatementdate,105)+substring(convert(varchar(10),cashbankdetail_bankstatementdate,101),7,4)end cashbankdetail_bankstatementdate,b.cashbankdetail_id,cashbankdetail_voucherid ,d.subaccount_code,b.cashbankdetail_mainaccountid,case when cashbankdetail_cashbankid is null then (select top 1 e.cashbankdetail_mainaccountid from trans_cashbankdetail as e where e.cashbankdetail_mainaccountid <>'" + txtBankName_hidden.Value.ToString() + "' and e.cashbankdetail_voucherid=b.cashbankdetail_voucherid and cashbankdetail_cashbankid is null) else b.cashbankdetail_mainaccountid end as test,d.subaccount_name  from Trans_CashBankVouchers  as a inner join Trans_CashBankDetail as b on a.cashbank_id=b.cashbankdetail_voucherid inner join master_mainaccount  as c on b.cashbankdetail_mainaccountid=c.MAinaccount_accountcode inner join master_subaccount as d  on b.cashbankdetail_subAccountid=d.subaccount_code WHERE  a.cashbank_transactionDate>='" + FromDate.Value.ToString() + "'  and a.cashbank_transactionDate<='" + DateTo.Value.ToString() + "' and (b.cashbankdetail_cashbankid='" + txtBankName_hidden.Value.ToString() + "' or b.cashbankdetail_mainaccountid='" + txtBankName_hidden.Value.ToString() + "')and(b.cashbankdetail_bankvaluedate is  not null  or b.cashbankdetail_bankvaluedate<>'1900-01-01 00:00:00.000')AND (subaccount_mainacreferenceid=b.cashbankdetail_mainaccountid or cashbankdetail_Subaccountid='' or cashbankdetail_Subaccountid is null))as d", "d.cashbank_transactionDate_test,d.cashbank_transactionDate,d.cashbank_vouchernumber,d.cashbankdetail_instrumentdate,d.cashbankdetail_instrumentdate_test,d.cashbankdetail_instrumentnumber,case substring(d.cashbank_vouchernumber,1,1) when 'C' then (select top 1 mainaccount_name from master_mainaccount where mainaccount_accountcode= d.test)  else d.AccountCode end as AccountCode,d.cashbankdetail_paymentamount,d.cashbankdetail_receiptamount,d.cashbankdetail_bankvaluedate,d.cashbankdetail_id,d.cashbankdetail_voucherid ,d.subaccount_code,d.subaccount_name,cashbankdetail_mainaccountid,d.cashbankdetail_bankstatementdate", "(d.cashbankdetail_bankvaluedate is  not null  or d.cashbankdetail_bankvaluedate<>'1900-01-01 00:00:00.000')", "cashbank_transactionDate_test,cashbank_vouchernumber,cashbankdetail_instrumentnumber");
            //}
            //else if (CheckStatus == "A")
            //{
            //    DT = oDBEngine.GetDataTable("(Select distinct a.cashbank_transactionDate as cashbank_transactionDate_test,(convert(varchar(11),a.cashbank_transactionDate,113))as cashbank_transactionDate,a.cashbank_vouchernumber,(convert(varchar(11),b.cashbankdetail_instrumentdate,113))as cashbankdetail_instrumentdate,b.cashbankdetail_instrumentdate as cashbankdetail_instrumentdate_test,b.cashbankdetail_instrumentnumber,c.mainaccount_name +' ['+isnull((isnull(rtrim(ltrim(d.subaccount_name)),'')+'['+ (isnull((select isnull(ltrim(rtrim(cnt_ucc)),'') from tbl_master_contact where cnt_internalid=d.subaccount_code),(select top 1 subaccount_code from master_subaccount where cast(subaccount_code as varchar)=d.subaccount_code and subaccount_mainacreferenceid=b.cashbankdetail_mainaccountid)))+']'),'') +']' as AccountCode,case b.cashbankdetail_paymentamount when 0 then null else cast(b.cashbankdetail_paymentamount as money) end as cashbankdetail_paymentamount,case b.cashbankdetail_receiptamount when 0 then null else cast(b.cashbankdetail_receiptamount as money) end as cashbankdetail_receiptamount,case b.cashbankdetail_bankvaluedate when '1900-01-01 00:00:00.000' then null else convert(varchar(6),cashbankdetail_bankvaluedate,105)+substring(convert(varchar(10),cashbankdetail_bankvaluedate,101),7,4)end cashbankdetail_bankvaluedate,case b.cashbankdetail_bankstatementdate when '1900-01-01 00:00:00.000' then null else convert(varchar(6),cashbankdetail_bankstatementdate,105)+substring(convert(varchar(10),cashbankdetail_bankstatementdate,101),7,4)end cashbankdetail_bankstatementdate,b.cashbankdetail_id,cashbankdetail_voucherid ,d.subaccount_code,b.cashbankdetail_mainaccountid,case when cashbankdetail_cashbankid is null then (select top 1 e.cashbankdetail_mainaccountid from trans_cashbankdetail as e where e.cashbankdetail_mainaccountid <>'" + txtBankName_hidden.Value.ToString() + "' and e.cashbankdetail_voucherid=b.cashbankdetail_voucherid and cashbankdetail_cashbankid is null) else b.cashbankdetail_mainaccountid end as test,d.subaccount_name  from Trans_CashBankVouchers  as a inner join Trans_CashBankDetail as b on a.cashbank_id=b.cashbankdetail_voucherid inner join master_mainaccount  as c on b.cashbankdetail_mainaccountid=c.MAinaccount_accountcode inner join master_subaccount as d  on b.cashbankdetail_subAccountid=d.subaccount_code WHERE  a.cashbank_transactionDate>='" + FromDate.Value.ToString() + "'  and a.cashbank_transactionDate<='" + DateTo.Value.ToString() + "' and (b.cashbankdetail_cashbankid='" + txtBankName_hidden.Value.ToString() + "' or b.cashbankdetail_mainaccountid='" + txtBankName_hidden.Value.ToString() + "')AND (subaccount_mainacreferenceid=b.cashbankdetail_mainaccountid or cashbankdetail_Subaccountid='' or cashbankdetail_Subaccountid is null))as d", "d.cashbank_transactionDate_test,d.cashbank_transactionDate,d.cashbank_vouchernumber,d.cashbankdetail_instrumentdate,d.cashbankdetail_instrumentdate_test,d.cashbankdetail_instrumentnumber,case substring(d.cashbank_vouchernumber,1,1) when 'C' then (select top 1 mainaccount_name from master_mainaccount where mainaccount_accountcode= d.test)  else d.AccountCode end as AccountCode,d.cashbankdetail_paymentamount,d.cashbankdetail_receiptamount,d.cashbankdetail_bankvaluedate,d.cashbankdetail_id,d.cashbankdetail_voucherid ,d.subaccount_code,d.subaccount_name,cashbankdetail_mainaccountid,d.cashbankdetail_bankstatementdate", null, "cashbank_transactionDate_test,cashbank_vouchernumber,cashbankdetail_instrumentnumber");
            //}  
            //DT = oDBEngine.GetDataTable("Trans_CashBankVouchers  as a inner join Trans_CashBankDetail as b on a.cashbank_id=b.cashbankdetail_voucherid inner join master_mainaccount as c on b.cashbankdetail_mainaccountid=c.MAinaccount_accountcode inner join master_subaccount as d on b.cashbankdetail_subAccountid=d.subaccount_code", "distinct a.cashbank_transactionDate as cashbank_transactionDate_test,(convert(varchar(11),a.cashbank_transactionDate,113))as cashbank_transactionDate,a.cashbank_vouchernumber,(convert(varchar(11),b.cashbankdetail_instrumentdate,113))as cashbankdetail_instrumentdate,b.cashbankdetail_instrumentdate as cashbankdetail_instrumentdate_test,b.cashbankdetail_instrumentnumber,c.mainaccount_name +'['+d.subaccount_name+'['+ (select  ltrim(rtrim(cnt_ucc)) from tbl_master_contact where cnt_internalid=d.subaccount_code )+']' +']' as AccountCode,case b.cashbankdetail_paymentamount when 0 then null else b.cashbankdetail_paymentamount end as cashbankdetail_paymentamount,case b.cashbankdetail_receiptamount when 0 then null else b.cashbankdetail_receiptamount end as cashbankdetail_receiptamount,b.cashbankdetail_bankvaluedate,b.cashbankdetail_id,d.subaccount_code,b.cashbankdetail_mainaccountid ", " a.cashbank_transactionDate>='" + FromDate.Value + "' and a.cashbank_transactionDate<='" + DateTo.Value + "' and (b.cashbankdetail_bankvaluedate is null or b.cashbankdetail_bankvaluedate ='1900-01-01 00:00:00.000') and b.cashbankdetail_cashbankid='" + txtBankName_hidden.Value.ToString() + "'", "cashbank_transactionDate");



            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            //{
            //    if (con.State == ConnectionState.Open) con.Close();

            //    using (SqlCommand com = new SqlCommand("Fetch_ManualBRS_Data", con))
            //    {
            //        com.CommandType = CommandType.StoredProcedure;
            //        com.CommandTimeout = 0;
            //        com.Parameters.AddWithValue("@TdateTo", Convert.ToDateTime(DateTo.Value).ToString("yyyy-MM-dd"));
            //        com.Parameters.AddWithValue("@TdateFrom", Convert.ToDateTime(FromDate.Value).ToString("yyyy-MM-dd"));
            //        com.Parameters.AddWithValue("@WhichRecord", CheckStatus);
            //        com.Parameters.AddWithValue("@AccountID", txtBankName_hidden.Value.ToString());
            //        con.Open();
            //        using (SqlDataAdapter Da = new SqlDataAdapter(com))
            //        {
            //            Da.Fill(DsBind);
            //        }
            //    }
            //}
            string nn = txtBankName_hidden.Value;
            string dd = Convert.ToString(DateTo.Value);

            string def = Convert.ToString(FromDate.Value);


            DsBind = oFinancialAccounting.FetchManualBRSData(Convert.ToDateTime(DateTo.Value).ToString("yyyy-MM-dd"), Convert.ToDateTime(FromDate.Value).ToString("yyyy-MM-dd"),
                CheckStatus, txtBankName_hidden.Value.ToString());

            //comment by sanjib due to grid changed
            //grdDetails.DataSource = DsBind;
            //grdDetails.DataBind();
            if (dtdstatedateall != null && !string.IsNullOrEmpty(Convert.ToString(dtdstatedateall.Value)) )
            {
                if (DsBind.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < DsBind.Tables[0].Rows.Count; i++)
                    {
                        DsBind.Tables[0].Rows[i][16] = dtdstatedateall.Value;
                    }

                }
            }
            


            grdmanualBRS.DataSource = DsBind;
            grdmanualBRS.DataBind();

            if (DsBind.Tables[0].Rows.Count > 0)
            {
                trhypertext.Visible = true;
                MainContent.Visible = true;
                ViewState["TableForThePage"] = DsBind.Tables[0];
                btnUpdate.Visible = true;
                btnCancel.Visible = true;
            }

        }
        protected void btnShow_Click1(object sender, EventArgs e)
        {
            BindTable();
            if (grdmanualBRS.VisibleRowCount == 0)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "MessageForBlankRecord", "<script language='JavaScript'>jAlert('No Record Found');</script>");

            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "ShowUpdateCancel", "<script language='Javascript'>ShowUpdateCancelButton();</script>");

                //added by sanjib for search if data exist.
                Page.ClientScript.RegisterStartupScript(GetType(), "ForSearchHide1", "<script language='Javascript'>SearchVisible('');</script>");
            }
            //Page.ClientScript.RegisterStartupScript(GetType(), "CallingHeight", "<script language='Javascript'>height();</script>");
            Page.ClientScript.RegisterStartupScript(GetType(), "ForSearchHide1", "<script language='Javascript'>SearchVisible('N');</script>");

        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {

            DataSet TempDs = new DataSet();
            DataTable TempTable = TempDs.Tables.Add();
            DataColumn[] keys = new DataColumn[1];
            DataColumn column;
            Boolean isvaluedate = false;

       
            

            //for (int i = 0; i < grdmanualBRS.VisibleRowCount; i++)
            //{
            for (int i = 0; i < grdmanualBRS.VisibleRowCount; i++)
            {
               

                string cashbank_transactionDate = Convert.ToString(grdmanualBRS.GetRowValues(i, "cashbank_transactionDate"));
                string cashbank_vouchernumber = Convert.ToString(grdmanualBRS.GetRowValues(i, "cashbank_vouchernumber"));
                string cashbankdetail_instrumentdate = Convert.ToString(grdmanualBRS.GetRowValues(i, "cashbankdetail_instrumentdate"));
                string cashbankdetail_instrumentnumber = Convert.ToString(grdmanualBRS.GetRowValues(i, "cashbankdetail_instrumentnumber"));
                string AccountCode = Convert.ToString(grdmanualBRS.GetRowValues(i, "AccountCode"));
                string cashbankdetail_paymentamount = Convert.ToString(grdmanualBRS.GetRowValues(i, "cashbankdetail_paymentamount"));
                string cashbankdetail_receiptamount = Convert.ToString(grdmanualBRS.GetRowValues(i, "cashbankdetail_receiptamount"));
                //Debashis
                string Module_Type = Convert.ToString(grdmanualBRS.GetRowValues(i, "Type"));
                //Debashis
                //Comment Out on 06/03/2017
                //string cashbankdetail_id = Convert.ToString(grdmanualBRS.GetRowValues(i, "cashbankdetail_id"));
                

                //string cashbank_transactionDate_test = Convert.ToString(grdmanualBRS.GetRowValues(i, "cashbank_transactionDate_test"));
                //string cashbankdetail_mainaccountid = Convert.ToString(grdmanualBRS.GetRowValues(i, "cashbankdetail_mainaccountid"));
                //string Cashbank_ExchangeSegmentID = Convert.ToString(grdmanualBRS.GetRowValues(i, "Cashbank_ExchangeSegmentID"));
                //string subaccount_code = Convert.ToString(grdmanualBRS.GetRowValues(i, "subaccount_code"));

                //Comment Out on 06/03/2017

                //var cashbank_transactionDate = DateTime.Now.ToString("MM/dd/yyyy")cashbank_transactionDates.ToString("yyyy-mm-dd");

                string cashbankdetail_bankstatementdate = string.Empty;
                string cashbankdetail_bankvaluedate = string.Empty;

                //Debashis
                //GridViewDataColumn col1 = grdmanualBRS.Columns[8] as GridViewDataColumn;
                //GridViewDataColumn col2 = grdmanualBRS.Columns[9] as GridViewDataColumn;
                GridViewDataColumn col1 = grdmanualBRS.Columns[9] as GridViewDataColumn;
                GridViewDataColumn col2 = grdmanualBRS.Columns[10] as GridViewDataColumn;
                //Debashis

                //ASPxTextBox chkIsVal = grdmanualBRS.FindRowCellTemplateControl(i, col1, "txt_cashbankdate") as ASPxTextBox;

                //ASPxTextBox textBox = grdmanualBRS.FindRowCellTemplateControl(i, col2, "txtvaluedate") as ASPxTextBox;

                ASPxDateEdit chkIsVal = grdmanualBRS.FindRowCellTemplateControl(i, col1, "txt_cashbankdate") as ASPxDateEdit;
                ASPxDateEdit textBox = grdmanualBRS.FindRowCellTemplateControl(i, col2, "bankvaluedate") as ASPxDateEdit;

                if (chkIsVal != null && !string.IsNullOrEmpty(chkIsVal.Text))
                {

                    cashbankdetail_bankstatementdate = chkIsVal.Text.Replace("/", "-").Trim();
                }
                else
                {
                    cashbankdetail_bankstatementdate = "1900-01-01";
                }
                if (textBox != null && !string.IsNullOrEmpty(textBox.Text) && textBox.Text.Trim()!="")
                { cashbankdetail_bankvaluedate = textBox.Text.Replace("/", "-").Trim(); }
                else
                {
                    cashbankdetail_bankvaluedate = "1900-01-01";
                }

                //}

                //foreach (GridViewRow row in grdDetails.Rows)
                //{
                //Label lblGetID = (Label)row.FindControl("lblid");
                //Label lblSegID = (Label)row.FindControl("lblSegID");

                //Comment Out on 06/03/2017
                //string IdForUpdateData = cashbankdetail_id;
                //string strSegID = Cashbank_ExchangeSegmentID;

                //Comment Out on 06/03/2017



                //added---06-09-2017-------------------------------
                DateTime docdate = Convert.ToDateTime(grdmanualBRS.GetRowValues(i, "cashbank_transactionDate"));
                if (textBox.Text.Trim() != "")
                {
                   // DateTime valuedate = Convert.ToDateTime(textBox.Text.Replace("/", "-").Trim());
                    DateTime valuedate = textBox.Date;
                    if (valuedate >= docdate)
                    {
                        isvaluedate = false;
                    }
                    else
                    {
                        isvaluedate = true;
                        break;
                    }
                }
                //-------------------------------------------------




                TextBox txtValueDate = new TextBox();
                txtValueDate.Text = cashbankdetail_bankvaluedate;
                TextBox txtStatementDate = new TextBox();
                txtStatementDate.Text = cashbankdetail_bankstatementdate;
                string[] DateFormat;
                string[] DateFormatStatement;
                string FormatedValueDate;
                string FormatedStatementDate = "01-01-1900";
                if (txtValueDate.Text != "" && txtValueDate.Text != "01-01-0100")
                {
                    DateFormat = txtValueDate.Text.Split('-');

                  
                    //Comment Out on 06/03/2017

                    string day = DateFormat[1].ToString();
                    string MM = DateFormat[0].ToString();
                    string Y = DateFormat[2].ToString();
                    //if (DateFormat[1].ToString().Length != 2)
                    //{
                    //    day = "0" + DateFormat[1].ToString();
                    //}
                    //if (DateFormat[0].ToString().Length != 2)
                    //{
                    //    MM = "0" + DateFormat[0].ToString();
                    //}
                    //Comment Out on 06/03/2017

                    //FormatedValueDate = Y.Trim() + "-" + MM.Trim() + "-" + day.Trim();

                    //Comment Out on 06/03/2017
                    FormatedValueDate = MM.Trim() + "-" + day.Trim() + "-" + Y.Trim();
                }
                else
                {
                   
                    FormatedValueDate = "1900-01-01";
                 
                   // FormatedValueDate = string.Empty;
                }
                if (txtStatementDate.Text != "" && txtStatementDate.Text != "01-01-0100")
                {
                    DateFormatStatement = txtStatementDate.Text.Split('-');

                   
                    //Comment Out on 06/03/2017

                    string day = DateFormatStatement[1].ToString();
                    string MM = DateFormatStatement[0].ToString();
                    string Y = DateFormatStatement[2].ToString();
                    //if (DateFormatStatement[1].ToString().Length != 2)
                    //{
                    //    day = "0" + DateFormatStatement[1].ToString();
                    //}
                    //if (DateFormatStatement[0].ToString().Length != 2)
                    //{
                    //    MM = "0" + DateFormatStatement[0].ToString();
                    //}
                    //Comment Out on 06/03/2017

                  
                  //  FormatedStatementDate = Y.Trim() + "-" + MM.Trim() + "-" + day.Trim();

                    //Comment Out on 06/03/2017

                    FormatedStatementDate = MM.Trim() + "-" + day.Trim() + "-" + Y.Trim();
                    //FormatedStatementDate = DateFormatStatement[1].ToString() + "-" + DateFormatStatement[0].ToString() + "-" + DateFormatStatement[2].ToString();
                }
                else
                {
                    FormatedStatementDate = "1900-01-01";
                }
                //Comment Out on 06/03/2017

                //Label lblSAcc = new Label();
                //lblSAcc.Text = subaccount_code;
                //Label lblMAcc = new Label();
                //lblMAcc.Text = cashbankdetail_mainaccountid;

                //Comment Out on 06/03/2017

                Label lblRefID = new Label();
                lblRefID.Text = cashbankdetail_instrumentnumber;
                Label lblVouNo = new Label();
                lblVouNo.Text = cashbank_vouchernumber;
                //Debashis
                Label lblModuleType = new Label();
                lblModuleType.Text = Module_Type;
                //Debashis
                Label lblINo = new Label();
                lblINo.Text = cashbankdetail_instrumentnumber;
                Label lblTDate = new Label();
                if (!string.IsNullOrEmpty(cashbank_transactionDate))
                {
                    DateTime dts = Convert.ToDateTime(cashbank_transactionDate);
                 
                    lblTDate.Text = dts.Date.ToString("yyyy-MM-dd");
                }
                else { lblTDate.Text = string.Empty; }




                //if (lblVouNo.Text.Substring(0, 1) == "C")
                //{
                //    string ca = oDBEngine.GetFieldValue("Trans_CashBankDetail as a inner join Trans_CashBankVouchers as  b on a.cashbankdetail_voucherid=b.cashbank_id ", "b.cashbank_id", " a.cashbankdetail_id='" + lblGetID.Text.ToString().Trim() + "'", 1)[0, 0];
                //    if (FormatedValueDate.ToString().Trim() != "")
                //        oDBEngine.SetFieldValue("Trans_CashBankDetail", "cashbankdetail_bankvaluedate='" + FormatedValueDate.ToString().Trim() + "',cashbankdetail_bankstatementdate='" + FormatedStatementDate.ToString().Trim() + "',cashbankdetail_ModifyUser='" + Session["userid"].ToString() + "',cashbankdetail_ModifyDateTime='" + oDBEngine.GetDate() + "',CashBankDetail_IsLocked='Y'", "cashbankdetail_voucherid='" + ca.ToString().Trim() + "'");
                //    else
                //        oDBEngine.SetFieldValue("Trans_CashBankDetail", "cashbankdetail_bankvaluedate='" + FormatedValueDate.ToString().Trim() + "',cashbankdetail_bankstatementdate='" + FormatedStatementDate.ToString().Trim() + "',cashbankdetail_ModifyUser='" + Session["userid"].ToString() + "',cashbankdetail_ModifyDateTime='" + oDBEngine.GetDate() + "',CashBankDetail_IsLocked=NULL", "cashbankdetail_voucherid='" + ca.ToString().Trim() + "'");
                //}
                //else
                //{
                //    if (FormatedValueDate.ToString().Trim() != "")
                //        oDBEngine.SetFieldValue("Trans_CashBankDetail", "cashbankdetail_bankvaluedate='" + FormatedValueDate.ToString().Trim() + "',cashbankdetail_bankstatementdate='" + FormatedStatementDate.ToString().Trim() + "',cashbankdetail_ModifyUser='" + Session["userid"].ToString() + "',cashbankdetail_ModifyDateTime='" + oDBEngine.GetDate() + "',CashBankDetail_IsLocked='Y'", "cashbankdetail_id='" + lblGetID.Text.ToString().Trim() + "'");
                //    else
                //        oDBEngine.SetFieldValue("Trans_CashBankDetail", "cashbankdetail_bankvaluedate='" + FormatedValueDate.ToString().Trim() + "',cashbankdetail_bankstatementdate='" + FormatedStatementDate.ToString().Trim() + "',cashbankdetail_ModifyUser='" + Session["userid"].ToString() + "',cashbankdetail_ModifyDateTime='" + oDBEngine.GetDate() + "',CashBankDetail_IsLocked=NULL", "cashbankdetail_id='" + lblGetID.Text.ToString().Trim() + "'");
                //}
                //oDBEngine.SetFieldValue("trans_accountsledger", "accountsledger_valuedate='" + FormatedValueDate.ToString().Trim() + "'", "accountsledger_subaccountid='" + lblSAcc.Text.ToString().Trim() + "' and accountsledger_Mainaccountid='" + lblMAcc.Text.ToString().Trim() + "' and accountsledger_transactionreferenceid='" + lblVouNo.Text.ToString().Trim() + "' and accountsledger_instrumentnumber='" + lblINo.Text.ToString().Trim() + "' and accountsledger_transactiondate='" + lblTDate.Text.ToString().Trim() + "'");
                //oDBEngine.SetFieldValue("trans_accountsledger", "accountsledger_valuedate='" + FormatedValueDate.ToString().Trim() + "'", "accountsledger_Mainaccountid='" + txtBankName_hidden.Value.ToString().Trim() + "' and accountsledger_transactionreferenceid='" + lblVouNo.Text.ToString().Trim() + "' and accountsledger_transactiondate='" + lblTDate.Text.ToString().Trim() + "' AND ACCOUNTSLEDGER_TRANSACTIONTYPE='Cash_Bank'");
                if (!string.IsNullOrEmpty(lblVouNo.Text) && !string.IsNullOrEmpty(lblINo.Text))
                {
                    if (TempDs.Tables[0].Rows.Count > 0)
                    {
                        //Comment Out on 06/03/2017
                        //TempDs.Tables[0].Rows.Add(IdForUpdateData, lblMAcc.Text, lblSAcc.Text, lblINo.Text, lblVouNo.Text, lblTDate.Text, FormatedValueDate, FormatedStatementDate, strSegID);
                        //Comment Out on 06/03/2017
                        //Debashis
                        //TempDs.Tables[0].Rows.Add(lblINo.Text, lblVouNo.Text, lblTDate.Text, FormatedValueDate, FormatedStatementDate);
                        TempDs.Tables[0].Rows.Add(lblINo.Text, lblVouNo.Text, lblTDate.Text, FormatedValueDate, FormatedStatementDate,lblModuleType.Text);
                        //Debashis

                    }
                    else
                    {
                        //// comment OutChanges
                        //column = new DataColumn();
                        //column.DataType = System.Type.GetType("System.String");
                        //column.ColumnName = "CashBankID";
                        //TempTable.Columns.Add(column);
                        //keys[0] = column;

                      
                        //TempTable.Columns.Add("MainAc", typeof(string));
                        //TempTable.Columns.Add("SubAc", typeof(string));

                        //// comment OutChanges


                        TempTable.Columns.Add("InstNo", typeof(string));
                        TempTable.Columns.Add("VoucherNo", typeof(string));
                        TempTable.Columns.Add("TranDate", typeof(string));
                        TempTable.Columns.Add("ValueDate", typeof(string));
                        TempTable.Columns.Add("StatementDate", typeof(string));
                        //Debashis
                        TempTable.Columns.Add("Module_Type", typeof(string));
                        //Debashis
                        //Comment Out on 06/03/2017
                        //TempTable.Columns.Add("UserSegID", typeof(string));
                        //Comment Out on 06/03/2017
                        TempTable.PrimaryKey = keys;
                        TempTable.TableName = "ManualBRS";
                        //Comment Out on 06/03/2017
                        //TempTable.Rows.Add(IdForUpdateData, lblMAcc.Text, lblSAcc.Text, lblINo.Text, lblVouNo.Text, lblTDate.Text, FormatedValueDate, FormatedStatementDate, strSegID);
                        //Comment Out on 06/03/2017

                        //Debashis
                        //TempTable.Rows.Add(lblINo.Text, lblVouNo.Text, lblTDate.Text, FormatedValueDate, FormatedStatementDate);
                        TempTable.Rows.Add(lblINo.Text, lblVouNo.Text, lblTDate.Text, FormatedValueDate, FormatedStatementDate,lblModuleType.Text);
                        //Debashis

                    }
                }
            }
            DataView TempDV = TempTable.DefaultView;

            if (isvaluedate==false)
            {
            if (TempDV.Count > 0)
            {
                TempDV.RowFilter = "ValueDate<>''";
                //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
                //{
                //    if (con.State == ConnectionState.Open) con.Close();
                //    using (SqlCommand com = new SqlCommand("Update_ManualBRS", con))
                //    {
                //        com.CommandType = CommandType.StoredProcedure;
                //        com.Parameters.AddWithValue("@Doc", TempDs.GetXml());
                //        com.Parameters.AddWithValue("@UserID", Session["userid"].ToString());
                //        con.Open();
                //        com.ExecuteNonQuery();
                //        con.Close();
                //    }
                //}
                oFinancialAccounting.UpdateManualBRS(TempDs.GetXml(), Session["userid"].ToString());
                BindTable();
                Page.ClientScript.RegisterStartupScript(GetType(), "ForSearchHideUpdate", "<script language='Javascript'>jAlert('Saved Successfully');</script>");
            }

            }
            else
            {
                //Page.ClientScript.RegisterStartupScript(GetType(), "ForSearchHideCheck", "<script language='Javascript'>jAlert('Document Date Can't be greater than Value Date');</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "ForSearchHideCheck", "<script language='Javascript'>ValueDocAlert('Y');</script>");
            }

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
                Dr = DT.Select("cashbankdetail_instrumentnumber like '%" + txtInsNo.Text.ToString() + "%'");
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
            if (AspValueDate.Value != null)
            {

                Dr = DT.Select("cashbankdetail_bankvaluedate = '" + AspValueDate.Value.ToString() + "'");
                CreateNewDataTableForSearch(Dr.Length, Dr);

            }
            if (txtSubName.Text.ToString() != "" && txtSubName.Text.ToString() != "SubAccount")
            {
                Dr = DT.Select("subaccount_name like '" + txtSubName.Text.ToString() + "%'");
                CreateNewDataTableForSearch(Dr.Length, Dr);
            }
            if (DT1.Rows.Count == 0)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "Message", "<script language='JavaScript'>jAlert('No record found');</script>");
            }

            //BindTable();

            trhypertext.Visible = true;
            Page.ClientScript.RegisterStartupScript(GetType(), "ShowUpdateCancel", "<script language='Javascript'>ShowUpdateCancelButton();</script>");


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
                drnew["cashbank_transactionDate_test"] = Convert.ToString(DataRow[count]["cashbank_transactionDate_test"]);
                drnew["cashbank_transactionDate"] = Convert.ToString(DataRow[count]["cashbank_transactionDate"]);
                drnew["cashbank_vouchernumber"] = Convert.ToString(DataRow[count]["cashbank_vouchernumber"]);
                drnew["cashbankdetail_instrumentdate"] = Convert.ToString(DataRow[count]["cashbankdetail_instrumentdate"]);
                drnew["cashbankdetail_instrumentdate_test"] = Convert.ToString(DataRow[count]["cashbankdetail_instrumentdate_test"]);
                drnew["cashbankdetail_instrumentnumber"] = Convert.ToString(DataRow[count]["cashbankdetail_instrumentnumber"]);
                drnew["AccountCode"] = Convert.ToString(DataRow[count]["AccountCode"]);
                //if (DataRow[count]["cashbankdetail_paymentamount"] != null)
                //{
                drnew["cashbankdetail_paymentamount"] = DataRow[count]["cashbankdetail_paymentamount"];
                //}
                //if (DataRow[count]["cashbankdetail_receiptamount"] != null)
                //{
                drnew["cashbankdetail_receiptamount"] = DataRow[count]["cashbankdetail_receiptamount"];
                //}
                drnew["cashbankdetail_bankvaluedate"] = DataRow[count]["cashbankdetail_bankvaluedate"];
                drnew["cashbankdetail_id"] = Convert.ToString(DataRow[count]["cashbankdetail_id"]);
                drnew["subaccount_code"] = Convert.ToString(DataRow[count]["subaccount_code"]);
                drnew["subaccount_name"] = Convert.ToString(DataRow[count]["subaccount_name"]);
                drnew["cashbankdetail_mainaccountid"] = Convert.ToString(DataRow[count]["cashbankdetail_mainaccountid"]);
                drnew["cashbankdetail_bankstatementdate"] = Convert.ToString(DataRow[count]["cashbankdetail_bankstatementdate"]);
                drnew["Cashbank_ExchangeSegmentID"] = Convert.ToString(DataRow[count]["Cashbank_ExchangeSegmentID"]);
                DT1.Rows.Add(drnew);

                //DT1[count]["cashbank_transactionDate"] = Dr[count]["cashbank_transactionDate"].ToString();
            }
            trhypertext.Visible = true;
            grdmanualBRS.DataSource = DT1;
            grdmanualBRS.DataBind();
            //comment by sanjib due to grid changed
            //grdDetails.DataSource = DT1;
            //grdDetails.DataBind();
            //BindTable();

        }
        public void ClearControls()
        {
            //AspTranDate.Value = null;
            //txtVoucherNo.Text = "Voucher No";
            //txtVoucherNo.ToolTip = "Voucher No";
            //txtAccName.Text = "Main Account";
            //txtAccName.ToolTip = "Main Account";
            //AspInsDate.Value = null;
            //txtInsNo.Text = "Ins No";
            //txtInsNo.ToolTip = "Ins No";
            //txtPayAmt.Text = "Pmt Amount";
            //txtPayAmt.ToolTip = "Pmt Amount";
            //txtReptAmt.Text = "Rcpt Amount";
            //txtReptAmt.ToolTip = "Rcpt Amount";
            //txtSubName.Text = "SubAccount";
            //txtSubName.ToolTip = "SubAccount";
            //AspValueDate.Value = null;
            AspTranDate.Value = null;
            txtVoucherNo.Text = "Document No";
            txtVoucherNo.ToolTip = "Document No";
            txtAccName.Text = "Main Account";
            txtAccName.ToolTip = "Main Account";
            AspInsDate.Value = null;
            txtInsNo.Text = "Instrument No";
            txtInsNo.ToolTip = "Instrument No";
            txtPayAmt.Text = "Payment Amount";
            txtPayAmt.ToolTip = "Payment Amount";
            txtReptAmt.Text = "Receipt Amount";
            txtReptAmt.ToolTip = "Receipt Amount";
            txtSubName.Text = "SubAccount";
            txtSubName.ToolTip = "SubAccount";
            AspValueDate.Value = null;
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("../ProjectMainPage.aspx");
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


            }

        }

        //protected void grdmanualBRS_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        //{

        //    BindTable();
        //}

        [WebMethod]
        public static List<string> Getbanks(string query)
        {

            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
            DataTable DT = new DataTable();
            DT.Rows.Clear();
            DT = oDBEngine.GetDataTable(query);
            List<string> obj = new List<string>();
            foreach (DataRow dr in DT.Rows)
            {

                obj.Add(Convert.ToString(dr["IntegrateMainAccount"]) + "|" + Convert.ToString(dr["MainAccount_AccountCode"]));
            }
            return obj;

        }

        //protected void grdmanualBRS_BeforeHeaderFilterFillItems(object sender, ASPxGridViewBeforeHeaderFilterFillItemsEventArgs e)
        //{
        //    BindTable();
        //}

        //protected void grdmanualBRS_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        //{
        //    string strSplitCommand = e.CallbackName.Split('~')[0];
        //    if (strSplitCommand == "Display")
        //    {
        //        string dates = e.CallbackName.Split('~')[1];
        //        BindTablesbydate(dates);
        //    }
        //    else {
        //        BindTable();
        //    }

        //}

        protected DateTime Setstatementdate(object container)
        {
            string FormatedStatementDate = string.Empty;
            GridViewDataItemTemplateContainer c = container as GridViewDataItemTemplateContainer;
            object value = c.Grid.GetRowValues(c.VisibleIndex, "cashbankdetail_bankstatementdate");
            string[] DateFormatStatement = null;
            DateTime dts = new DateTime(); ;
            if (value != null && !string.IsNullOrEmpty(Convert.ToString(value)))
            {
                if (Convert.ToString(value).Contains("-"))
                {
                    DateFormatStatement = Convert.ToString(value).Split('-');
                    string MM = DateFormatStatement[1].ToString();
                    string day = DateFormatStatement[0].ToString();
                    string Y = DateFormatStatement[2].ToString();
                    if (DateFormatStatement[0].ToString().Length != 2)
                    {
                        day = "0" + DateFormatStatement[0].ToString();
                    }
                    if (DateFormatStatement[1].ToString().Length != 2)
                    {
                        MM = "0" + DateFormatStatement[1].ToString();
                    }
                    FormatedStatementDate = day.Trim() + "-" + MM.Trim() + "-" + Y.Trim();
                    //string dd = Convert.ToString(value);

                    // DateTime.TryParseExact(dd, "dd/MM/yyyy", enUS, DateTimeStyles.None, out dt);
                    DateTime dt = DateTime.ParseExact(FormatedStatementDate.Trim(), "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    return dt;
                }
                else if (Convert.ToString(value).Contains("/"))
                {
                    DateFormatStatement = Convert.ToString(value).Split('/');
                    string MM = DateFormatStatement[0].ToString();
                    string day = DateFormatStatement[1].ToString();
                    string Y = DateFormatStatement[2].ToString();
                    if (DateFormatStatement[1].ToString().Length != 2)
                    {
                        day = "0" + DateFormatStatement[1].ToString();
                    }
                    if (DateFormatStatement[0].ToString().Length != 2)
                    {
                        MM = "0" + DateFormatStatement[0].ToString();
                    }
                    FormatedStatementDate = day.Trim() + "-" + MM.Trim() + "-" + Y.Trim().Split(' ')[0];
                    //string dd = Convert.ToString(value);

                    // DateTime.TryParseExact(dd, "dd/MM/yyyy", enUS, DateTimeStyles.None, out dt);
                    DateTime dt = DateTime.ParseExact(FormatedStatementDate.Trim(), "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    return dt;
                }
                


            }
            return dts;
        }
        protected DateTime Setbankvaluedate(object container)
        {
            string[] DateFormatStatement;
            string FormatedStatementDate = string.Empty;
            GridViewDataItemTemplateContainer c = container as GridViewDataItemTemplateContainer;
            object value = c.Grid.GetRowValues(c.VisibleIndex, "cashbankdetail_bankvaluedate");
            string[] DateFormatvalue;
            DateTime dts = new DateTime(); ;
            if (value != null && !string.IsNullOrEmpty(Convert.ToString(value)))
            {

                DateFormatStatement = Convert.ToString(value).Split('-');
                string MM = DateFormatStatement[1].ToString();
                string day = DateFormatStatement[0].ToString();
                string Y = DateFormatStatement[2].ToString();
                if (DateFormatStatement[0].ToString().Length != 2)
                {
                    day = "0" + DateFormatStatement[0].ToString();
                }
                if (DateFormatStatement[1].ToString().Length != 2)
                {
                    MM = "0" + DateFormatStatement[1].ToString();
                }
                FormatedStatementDate = day.Trim() + "-" + MM.Trim() + "-" + Y.Trim();
                //string dd = Convert.ToString(value);

                // DateTime.TryParseExact(dd, "dd/MM/yyyy", enUS, DateTimeStyles.None, out dt);
                // DateTime dt = Convert.ToDateTime(FormatedStatementDate.Trim());
                DateTime myDate = DateTime.ParseExact(FormatedStatementDate.Trim(), "dd-MM-yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);
                return myDate;
            }
            return dts;
        }

        protected void grdmanualBRS_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string strSplitCommand = e.Parameters.Split('~')[0];
            if (strSplitCommand == "Setdate")
            {
                string dates = e.Parameters.Split('~')[1];
                BindTablesbydate(dates);
            }
            else
            {
                BindTable();
            }
        }

    }

}