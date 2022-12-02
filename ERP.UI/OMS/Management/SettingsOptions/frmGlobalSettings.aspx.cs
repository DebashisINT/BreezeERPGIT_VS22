using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;

namespace ERP.OMS.Management.SettingsOptions
{
    public partial class management_SettingsOptions_frmGlobalSettings : System.Web.UI.Page
    {
        BusinessLogicLayer.Converter OConvert = new BusinessLogicLayer.Converter();

        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);

        string DataEx = "N";

        string SegmentID = string.Empty;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            Session["KeyVal_InternalID"] = "";
            Session["KeyVal"] = "n";

            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);

                //dtptoDate.EditFormatString = "";
                //dtptoDate.EditFormatString = OConvert.GetDateFormat("Date");
                //dtptoDate.Date = oDBEngine.GetDate();

            }
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            gridStatusDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userlastsegment"].ToString() != "4")
            {
                if (Session["userlastsegment"].ToString() == "1")
                {
                    SegmentID = "998";
                }
                else
                {
                    SegmentID = HttpContext.Current.Session["usersegid"].ToString();
                }

            }
            else
            {
                SegmentID = "999";
            }
            if ((SegmentID != "999") && (SegmentID != "998"))
            {
                if (Session["userlastsegment"].ToString() == "9" || Session["userlastsegment"].ToString() == "10")
                {

                    DataTable dtEx = oDBEngine.GetDataTable("tbl_master_companyexchange", "exch_internalid", "exch_TMCODE='" + Session["usersegid"].ToString() + "'");
                    SegmentID = dtEx.Rows[0][0].ToString();
                }

            }

            if (!IsPostBack)
            {
                //dtptoDate.Date = oDBEngine.GetDate();
                dtptoDate.EditFormatString = OConvert.GetDateFormat("Date");
                this.Page.ClientScript.RegisterStartupScript(GetType(), "height1", "<script>Pageload();</script>");

            }

            ASPxCallbackPanel1.JSProperties["cpaddoredit"] = null;
            fillGrid();
        }

        protected void gridStatus_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {


            if (e.Parameters != null)
            {
                string[] data = e.Parameters.Split('~');
                if (data[0] == "Delete")
                {
                    oDBEngine.DeleteValue(" config_globalsettings", " GlobalSettings_ID='" + data[1] + "'");

                }
                else if (data[0].ToString() == "s")
                {
                    gridContract.Settings.ShowFilterRow = true;

                }
                else if (data[0].ToString() == "All")
                {
                    // if data != demo then bind combo;
                    gridContract.FilterExpression = string.Empty;

                }

            }
            fillGrid();
            this.Page.ClientScript.RegisterStartupScript(GetType(), "height4454", "<script>height();</script>");

        }

        public void fillGrid()
        {
            gridStatusDataSource.SelectCommand = "select GlobalSettings_ID, case " +
            @"when  GlobalSettings_Name='GS_CDSLOPERATORID' then 'Batch Operator ID'
        when  GlobalSettings_Name='GS_MAKER' then 'Maker ID'  
        when  GlobalSettings_Name='GS_CHECKER' then 'Checker ID'
        when  GlobalSettings_Name='GS_VERIFIER' then 'Verifier ID' 
        when  GlobalSettings_Name='GS_CMBILLDATE' then 'Cm Bill Posting Day' 
         when  GlobalSettings_Name='GS_FOBILLDATE' then 'FO Bill Posting Day'
        when  GlobalSettings_Name='GS_CDXBILLDATE' then 'CDX Bill Posting Day'
        when  GlobalSettings_Name='GS_COMMBILLDATE' then 'COMM Bill Posting Day'
        when  GlobalSettings_Name='GS_CLMARGINAC' then 'Post Daily Client Margin in Accounts'      
        when GlobalSettings_Name='GS_HGVLAUTHORIZATIONS' then 'Number Of Authorizations For High Value Slips' 
        when GlobalSettings_Name='GS_CDSLCLNTOPAUTH' then 'Number of CDSL client opening Authorization'  
        when  GlobalSettings_Name='GS_BRKDECIMAL' then 'Default Brokerage Decimal Places' 
        when  GlobalSettings_Name='GS_BRKGROUND' then 'Default Brokerage Round-Off Pattern' 
        when  GlobalSettings_Name='GS_AVGPATTERN' then 'Default Trade Average Pattern' 
        when  GlobalSettings_Name='GS_MKTDECIMAL' then 'Default Market Rate Decimal' 
        when  GlobalSettings_Name='GS_MKTROUND' then 'Default Market Rate Round-Off Pattern' 
        when  GlobalSettings_Name='GS_EXCHOBLROUND' then 'Exchange Obligation Round-Off Pattern' 
        when  GlobalSettings_Name='GS_STTACCOUNTING' then 'STT Accounting Pattern' 
        when  GlobalSettings_Name='GS_NOOFREQAUTH' then 'Number Of Authorizations For Requisitions' 
        when  GlobalSettings_Name='GS_UCCPATTERN' then 'UCC Pattern For KYC Client' 
        when  GlobalSettings_Name='GS_KYCAUTH' then 'Number Of Authorizations For KYC' 
        when  GlobalSettings_Name='GS_DEBUGSTATE' then 'Server Debuger'  
        when  GlobalSettings_Name='GS_FOACCOUNTINGJV' then 'FO Segment Daily Billing Style' 
        when  GlobalSettings_Name='GS_EXCHSEBIFEE' then 'Generate SEBI Fee for Exchange' 
        when  GlobalSettings_Name='GS_EXCHTRANCHARGE' then 'Generate Transaction Charges for Exchange' 
        when  GlobalSettings_Name='GS_EXCHCLCHARGE' then 'Generate Clearing Charges for Exchange' 
        when  GlobalSettings_Name='GS_EXCHCLCHARGEST' then 'Generate Service Tax on Clearing Charges for Exchange' 
        when  GlobalSettings_Name='GS_EXCHTRANCHARGEST' then 'Generate Service Tax on Tran. Charges for Exchange' 
        when  GlobalSettings_Name='GS_EXCHOBLACCOUNT' then 'Exchange Obligation Account' 
        when  GlobalSettings_Name='GS_DORMANCY' then 'Set Client Account Dormancy Period ' 
        when  GlobalSettings_Name='GS_HIGHVALUETRNDP' then 'Set High Value DP Transaction Amount' 
        when  GlobalSettings_Name='GS_EXCHTRANRND' then 'Round Off Exchange Transaction Charges' 
        when  GlobalSettings_Name='GS_COMPANYNETWORTH' then 'Company Net Worth'  
        when  GlobalSettings_Name='GS_SEBIBRKG' then 'Brokerage not to exceed SEBI limit of' 
        when  GlobalSettings_Name='GS_LCKBNK' then 'Lock Cash Bank Entries' 
        when  GlobalSettings_Name='GS_LCKJV' then 'Lock Journal Voucher Entries' 
        when  GlobalSettings_Name='GS_LCKTRADE' then 'Lock Trade Entries'  
        when  GlobalSettings_Name='GS_LCKDEMAT' then 'Lock Demat Entries' 
        when  GlobalSettings_Name='GS_LCKALL' then 'Lock ALL Data entry & edit task'  
        when GlobalSettings_Name='GS_NSEPAYOUT' then 'Exchange Payout Method' 
        when GlobalSettings_Name='GS_EXCHCLCHARGETRADE' then 'Exchange Clearing Charge Calculation Basis' end as GlobalSettingsName,
        case when  GlobalSettings_Name='GS_CLMARGINAC'  and GlobalSettings_Value='1' then 'Yes' 
        when  GlobalSettings_Name='GS_CLMARGINAC'  and GlobalSettings_Value='2' then 'No' 
        when  GlobalSettings_Name='GS_BRKDECIMAL' and GlobalSettings_Value='2' then '2-Decimal' 
        when  GlobalSettings_Name='GS_BRKDECIMAL' and GlobalSettings_Value='3' then '3-Decimal' 
        when  GlobalSettings_Name='GS_BRKDECIMAL' and GlobalSettings_Value='4' then '4-Decimal' 
        when  GlobalSettings_Name='GS_BRKDECIMAL' and GlobalSettings_Value='5' then '5-Decimal' 
        when  GlobalSettings_Name='GS_BRKDECIMAL' and GlobalSettings_Value='6' then '6-Decimal' 
        when  GlobalSettings_Name='GS_BRKGROUND' and GlobalSettings_Value='1'  then 'Nearest Paisa' 
        when  GlobalSettings_Name='GS_BRKGROUND' and GlobalSettings_Value='2'  then 'Higher Paisa' 
        when  GlobalSettings_Name='GS_BRKGROUND' and GlobalSettings_Value='3'  then 'Lower Paisa' 
        when  GlobalSettings_Name='GS_BRKGROUND' and GlobalSettings_Value='4'  then 'Nearest 5-Paisa' 
        when  GlobalSettings_Name='GS_BRKGROUND' and GlobalSettings_Value='5'  then 'Higher 5-Paisa' 
        when  GlobalSettings_Name='GS_BRKGROUND' and GlobalSettings_Value='6'  then 'Lower 5-Paisa' 
        when  GlobalSettings_Name='GS_BRKGROUND' and GlobalSettings_Value='7'  then 'Truncate'  
        when  GlobalSettings_Name='GS_AVGPATTERN' and GlobalSettings_Value='1'  then 'None' 
        when  GlobalSettings_Name='GS_AVGPATTERN' and GlobalSettings_Value='2'  then 'Order Wise' 
        when  GlobalSettings_Name='GS_AVGPATTERN' and GlobalSettings_Value='3'  then 'Instrument Wise' 
        when  GlobalSettings_Name='GS_AVGPATTERN' and GlobalSettings_Value='4' then 'Similar Price' 
        when  GlobalSettings_Name='GS_CMBILLDATE' and GlobalSettings_Value='2' then 'Payout Date' 
        when  GlobalSettings_Name='GS_CMBILLDATE' and GlobalSettings_Value='1' then 'Trade Date'
         when  GlobalSettings_Name='GS_FOBILLDATE' and GlobalSettings_Value='2' then 'Payout Date' 
        when  GlobalSettings_Name='GS_FOBILLDATE' and GlobalSettings_Value='1' then 'Trade Date'
 when  GlobalSettings_Name='GS_CDXBILLDATE' and GlobalSettings_Value='2' then 'Payout Date' 
        when  GlobalSettings_Name='GS_CDXBILLDATE' and GlobalSettings_Value='1' then 'Trade Date'
 when  GlobalSettings_Name='GS_COMMBILLDATE' and GlobalSettings_Value='2' then 'Payout Date' 
        when  GlobalSettings_Name='GS_COMMBILLDATE' and GlobalSettings_Value='1' then 'Trade Date' 
        when  GlobalSettings_Name='GS_MKTDECIMAL' and GlobalSettings_Value='2' then '2-Decimal' 
        when  GlobalSettings_Name='GS_MKTDECIMAL' and GlobalSettings_Value='3' then '3-Decimal' 
        when  GlobalSettings_Name='GS_MKTDECIMAL' and GlobalSettings_Value='4' then '4-Decimal' 
        when  GlobalSettings_Name='GS_MKTDECIMAL' and GlobalSettings_Value='5' then '5-Decimal' 
        when  GlobalSettings_Name='GS_MKTDECIMAL' and GlobalSettings_Value='6' then '6-Decimal' 
        when  GlobalSettings_Name='GS_MKTROUND' and GlobalSettings_Value='1'  then 'Nearest Paisa' 
        when  GlobalSettings_Name='GS_MKTROUND' and GlobalSettings_Value='2'  then 'Higher Paisa' 
        when  GlobalSettings_Name='GS_MKTROUND' and GlobalSettings_Value='3'  then 'Lower Paisa' 
        when  GlobalSettings_Name='GS_MKTROUND' and GlobalSettings_Value='4'  then 'Nearest 5-Paisa' 
        when  GlobalSettings_Name='GS_MKTROUND' and GlobalSettings_Value='5'  then 'Higher 5-Paisa' 
        when  GlobalSettings_Name='GS_MKTROUND' and GlobalSettings_Value='6'  then 'Lower 5-Paisa' 
        when  GlobalSettings_Name='GS_MKTROUND' and GlobalSettings_Value='7'  then 'Truncate'  
        when  GlobalSettings_Name='GS_EXCHOBLROUND' and GlobalSettings_Value='1' then 'None' 
        when  GlobalSettings_Name='GS_EXCHOBLROUND' and GlobalSettings_Value='3' then 'Nearest-Rupee' 
        when  GlobalSettings_Name='GS_DEBUGSTATE'  and GlobalSettings_Value='0' then 'OFF' 
        when  GlobalSettings_Name='GS_DEBUGSTATE'  and GlobalSettings_Value='1' then 'ON' 
        when  GlobalSettings_Name='GS_NOOFREQAUTH' and GlobalSettings_Value='1' then 'Single' 
        when  GlobalSettings_Name='GS_NOOFREQAUTH' and GlobalSettings_Value='2' then 'Two' 
        when  GlobalSettings_Name='GS_NOOFREQAUTH' and GlobalSettings_Value='3' then 'Three' 
        when  GlobalSettings_Name='GS_UCCPATTERN' and GlobalSettings_DefaultNarration='Auto' then cast(GlobalSettings_Value AS varchar(4)) 
        when  GlobalSettings_Name='GS_UCCPATTERN' and GlobalSettings_DefaultNarration='Manual' then 'NA' 
        when  GlobalSettings_Name='GS_KYCAUTH' and GlobalSettings_Value='1' then 'Single' 
        when  GlobalSettings_Name='GS_KYCAUTH' and GlobalSettings_Value='2' then 'Two' 
        when  GlobalSettings_Name='GS_KYCAUTH' and GlobalSettings_Value='3' then 'Three'  
        when  GlobalSettings_Name='GS_STTACCOUNTING'  and GlobalSettings_Value='1' then 'Only Provisional' 
        when  GlobalSettings_Name='GS_STTACCOUNTING'  and GlobalSettings_Value='2' then 'Only Imported' 
        when  GlobalSettings_Name='GS_STTACCOUNTING'  and GlobalSettings_Value='3' then 'Provisional & Imported' 
        when  GlobalSettings_Name='GS_FOACCOUNTINGJV' and GlobalSettings_Value='1'  then 'Separate Entries' 
        when  GlobalSettings_Name='GS_FOACCOUNTINGJV' and GlobalSettings_Value='2'  then 'Consolidated Entries For The Day' 
        when  GlobalSettings_Name='GS_EXCHSEBIFEE'  and GlobalSettings_Value='1'  then  'Generate'  
        when  GlobalSettings_Name='GS_EXCHSEBIFEE'  and GlobalSettings_Value='2'  then  'Do Not Generate' 
        when  GlobalSettings_Name='GS_EXCHTRANCHARGE'  and GlobalSettings_Value='1'  then 'Generate'   
        when  GlobalSettings_Name='GS_EXCHTRANCHARGE'  and GlobalSettings_Value='2'  then 'Do Not Generate' 
        when  GlobalSettings_Name='GS_EXCHCLCHARGE'  and GlobalSettings_Value='1'  then 'Generate' 
        when  GlobalSettings_Name='GS_EXCHCLCHARGE'  and GlobalSettings_Value='2'  then 'Do Not Generate' 
        when  GlobalSettings_Name='GS_EXCHTRANCHARGEST'   and GlobalSettings_Value='1'  then 'Generate'    
        when  GlobalSettings_Name='GS_EXCHTRANCHARGEST'   and GlobalSettings_Value='2'  then 'Do Not Generate' 
        when  GlobalSettings_Name='GS_EXCHCLCHARGEST'  and GlobalSettings_Value='1'  then 'Generate' 
        when  GlobalSettings_Name='GS_EXCHCLCHARGEST'  and GlobalSettings_Value='2'  then 'Do Not Generate' 
        when  GlobalSettings_Name='GS_EXCHOBLACCOUNT' and GlobalSettings_Value='1'  then 'Default Account'  
        when  GlobalSettings_Name='GS_EXCHOBLACCOUNT' and GlobalSettings_Value='2'  then 'Selected Account' 
        when  GlobalSettings_Name='GS_EXCHTRANRND' and GlobalSettings_Value='1' then 'None'   
        when  GlobalSettings_Name='GS_EXCHTRANRND' and GlobalSettings_Value='3' then 'Nearest Rupee'  
        when  GlobalSettings_Name='GS_DORMANCY'  then cast(GlobalSettings_Value as varchar(50)) 
        when  GlobalSettings_Name='GS_HIGHVALUETRNDP'  then cast(GlobalSettings_Value as varchar(50))   
        when  GlobalSettings_Name='GS_HGVLAUTHORIZATIONS' and GlobalSettings_Value='1' then 'Single'  
        when  GlobalSettings_Name='GS_HGVLAUTHORIZATIONS' and GlobalSettings_Value='2' then 'Two'  
        when  GlobalSettings_Name='GS_HGVLAUTHORIZATIONS' and GlobalSettings_Value='3' then 'Three'   
        when  GlobalSettings_Name='GS_HGVLAUTHORIZATIONS' and GlobalSettings_Value='4' then 'four'  
        when  GlobalSettings_Name='GS_CDSLCLNTOPAUTH' and GlobalSettings_Value='1' then 'Single' 
        when  GlobalSettings_Name='GS_CDSLCLNTOPAUTH' and GlobalSettings_Value='2' then 'Two' 
        when  GlobalSettings_Name='GS_CDSLCLNTOPAUTH' and GlobalSettings_Value='3' then 'Three'   
        when  GlobalSettings_Name='GS_COMPANYNETWORTH'  then cast(GlobalSettings_Value as varchar(50))  
        when  GlobalSettings_Name='GS_SEBIBRKG'  then cast(GlobalSettings_Rate as varchar(50))  
        when  GlobalSettings_Name='GS_LCKBNK'   and GlobalSettings_Value='1' then cast(GlobalSettings_LockDays as varchar(50)) 
        When  GlobalSettings_Name='GS_LCKBNK'   and GlobalSettings_Value='2' then Convert(varchar,GlobalSettings_LockDate,106)  
        When  GlobalSettings_Name='GS_LCKJV' and GlobalSettings_Value='1' then cast(GlobalSettings_LockDays as varchar(50)) 
        When  GlobalSettings_Name='GS_LCKJV' and GlobalSettings_Value='2' then Convert(varchar,GlobalSettings_LockDate,106) 
        When  GlobalSettings_Name='GS_LCKTRADE' and GlobalSettings_Value='1' then cast(GlobalSettings_LockDays as varchar(50)) 
        When  GlobalSettings_Name='GS_LCKTRADE' and GlobalSettings_Value='2' then Convert(varchar,GlobalSettings_LockDate,106) 
        When  GlobalSettings_Name='GS_LCKDEMAT' and GlobalSettings_Value='1' then cast(GlobalSettings_LockDays as varchar(50)) 
        When  GlobalSettings_Name='GS_LCKDEMAT' and GlobalSettings_Value='2' then Convert(varchar,GlobalSettings_LockDate,106) 
        When  GlobalSettings_Name='GS_LCKALL' and GlobalSettings_Value='1' then cast(GlobalSettings_LockDays as varchar(50)) 
        When  GlobalSettings_Name='GS_LCKALL' and GlobalSettings_Value='2' then Convert(varchar,GlobalSettings_LockDate,106)  
        when GlobalSettings_Name='GS_NSEPAYOUT' and GlobalSettings_Value='1' then 'DFRS' 
        when GlobalSettings_Name='GS_NSEPAYOUT' and GlobalSettings_Value='2' then 'CADT(Direct Payout)' 
        when GlobalSettings_Name='GS_EXCHCLCHARGETRADE' and GlobalSettings_Value='1' then 'Calculate Per Trade'  
        when GlobalSettings_Name='GS_EXCHCLCHARGETRADE' and GlobalSettings_Value='2' then 'Calculate Per Day'
        when  GlobalSettings_Name='GS_MAKER' then (select LTRIM(Rtrim(user_loginId))+' ['+Cast(USER_ID as Varchar)+']' from tbl_master_user where USER_ID=GlobalSettings_Value) 
        when  GlobalSettings_Name='GS_CHECKER' then (select LTRIM(Rtrim(user_loginId))+' ['+Cast(USER_ID as Varchar)+']' from tbl_master_user where USER_ID=GlobalSettings_Value) 
        when  GlobalSettings_Name='GS_VERIFIER' then (select LTRIM(Rtrim(user_loginId))+' ['+Cast(USER_ID as Varchar)+']' from tbl_master_user where USER_ID=GlobalSettings_Value) 
        else '' end as GlobSetValue,
        GlobalSettings_Rate,GlobalSettings_LockDays,GlobalSettings_LockDate,GlobalSettings_Value,GlobalSettings_DefaultNarration  
        from config_globalsettings where GlobalSettings_SegmentID='" + SegmentID + "' ";

            gridContract.DataBind();

            this.Page.ClientScript.RegisterStartupScript(GetType(), "height477", "<script>height();</script>");

        }

        protected void ASPxCallbackPanel1_CustomJSProperties(object sender, DevExpress.Web.CustomJSPropertiesEventArgs e)
        {
            e.Properties["cpLast"] = Session["KeyVal"].ToString();
            e.Properties["cpfast"] = DataEx;
        }

        protected void ASPxCallbackPanel1_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {

            DataTable dtCheck = new DataTable();
            DataTable DT = new DataTable();
            // dtptoDate.Date = oDBEngine.GetDate();
            string[] data = e.Parameter.Split('~');
            if (data[0] == "Edit")
            {
                ASPxCallbackPanel1.JSProperties["cpaddoredit"] = "old";
                cmbType.Enabled = false;
                clearField();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "jscript10", "Validate();", true);

                DT = oDBEngine.GetDataTable("config_globalsettings ", "GlobalSettings_Name,GlobalSettings_Value,GlobalSettings_Rate,GlobalSettings_Lockdays,GlobalSettings_LockDate,case when GlobalSettings_ExchangeCRMainAccount is not  null then (select MainAccount_Name +'['+ isnull(MainAccount_AccountCode,'') +']' from master_mainaccount where  MainAccount_AccountCode=GlobalSettings_ExchangeCRMainAccount) else null end as CreditName,case when GlobalSettings_ExchangeCRSubAccount is not  null then (select  SubAccount_Name + '['+ isnull(SubAccount_Code,'') +']' from master_subaccount where  SubAccount_Code=GlobalSettings_ExchangeCRSubAccount and SubAccount_MainAcReferenceID=GlobalSettings_ExchangeCRMainAccount) else null end as CreditSName,case when GlobalSettings_ExchangeDRMainAccount is not  null then (select MainAccount_Name +'['+ isnull(MainAccount_AccountCode,'') +']' from master_mainaccount where  MainAccount_AccountCode=GlobalSettings_ExchangeDRMainAccount  ) else null end as DebitName,case when GlobalSettings_ExchangeDRSubAccount is not  null then (select  SubAccount_Name + '['+ isnull(SubAccount_Code,'') +']' from master_subaccount where  SubAccount_Code=GlobalSettings_ExchangeDRSubAccount and SubAccount_MainAcReferenceID=GlobalSettings_ExchangeDRMainAccount) else null end as DebitSName,GlobalSettings_ExchangeCRMainAccount,GlobalSettings_ExchangeCRSubAccount,GlobalSettings_ExchangeDRMainAccount,GlobalSettings_ExchangeDRSubAccount,GlobalSettings_DefaultNarration     ", "GlobalSettings_ID='" + data[1] + "'");
                cmbType.SelectedValue = DT.Rows[0]["GlobalSettings_Name"].ToString();
                //cmbValu.SelectedValue = DT.Rows[0]["GlobalSettings_Name"].ToString();
                cmbValu.Items.Clear();
                BindCombo();
                if (DT.Rows.Count > 0)
                {

                    if (cmbType.SelectedItem.Value == "GS_DORMANCY" || cmbType.SelectedItem.Value == "GS_HIGHVALUETRNDP" || cmbType.SelectedItem.Value == "GS_COMPANYNETWORTH")// || cmbType.SelectedItem.Value == "GS_SEBIBRKG" )//|| cmbType.SelectedItem.Value == "GS_LCKBNK")
                    {
                        cmbValu.SelectedIndex = 1;
                        txtTValue.Text = DT.Rows[0]["GlobalSettings_Value"].ToString();
                        txtNarr.Text = DT.Rows[0]["GlobalSettings_DefaultNarration"].ToString();
                    }
                    else if (cmbType.SelectedItem.Value == "GS_SEBIBRKG")
                    {
                        cmbValu.SelectedIndex = 1;
                        txtTValue.Text = DT.Rows[0]["GlobalSettings_Rate"].ToString();
                    }

                    else if (cmbType.SelectedItem.Value == "GS_LCKBNK" || cmbType.SelectedItem.Value == "GS_LCKJV" || cmbType.SelectedItem.Value == "GS_LCKTRADE" || cmbType.SelectedItem.Value == "GS_LCKDEMAT" || cmbType.SelectedItem.Value == "GS_LCKALL")
                    {
                        if (DT.Rows[0]["GlobalSettings_Value"].ToString() == "2")
                        //cmbValu.SelectedValue = DT.Rows[0]["GlobalSettings_Value"].ToString();
                        //txtTValue.Text = DT.Rows[0]["GlobalSettings_LockDays"].ToString();
                        {
                            dtptoDate.Date = Convert.ToDateTime(DT.Rows[0]["GlobalSettings_LockDate"].ToString());
                            ASPxCallbackPanel1.JSProperties["cphideshow"] = "2";

                        }

                        else
                        {
                            txtTValue.Text = DT.Rows[0]["GlobalSettings_LockDays"].ToString();
                            ASPxCallbackPanel1.JSProperties["cphideshow"] = "1";
                        }

                    }


                    else if (cmbType.SelectedItem.Value == "GS_EXCHSEBIFEE" || cmbType.SelectedItem.Value == "GS_EXCHTRANCHARGE" || cmbType.SelectedItem.Value == "GS_EXCHTRANCHARGEST" || cmbType.SelectedItem.Value == "GS_EXCHCLCHARGE" || cmbType.SelectedItem.Value == "GS_EXCHCLCHARGEST" || cmbType.SelectedItem.Value == "GS_EXCHOBLACCOUNT")
                    {
                        cmbValu.SelectedValue = DT.Rows[0]["GlobalSettings_Value"].ToString();


                        txtCredit.Text = DT.Rows[0]["CreditName"].ToString();
                        txtSubCredit.Text = DT.Rows[0]["CreditSName"].ToString();
                        txtDebit.Text = DT.Rows[0]["DebitName"].ToString();
                        txtSubDebit.Text = DT.Rows[0]["DebitSName"].ToString();

                        txtCredit_hidden.Value = DT.Rows[0]["GlobalSettings_ExchangeCRMainAccount"].ToString();
                        txtSubCredit_hidden.Value = DT.Rows[0]["GlobalSettings_ExchangeCRSubAccount"].ToString();
                        txtDebit_hidden.Value = DT.Rows[0]["GlobalSettings_ExchangeDRMainAccount"].ToString();
                        txtSubDebit_hidden.Value = DT.Rows[0]["GlobalSettings_ExchangeDRSubAccount"].ToString();
                        txtNarr.Text = DT.Rows[0]["GlobalSettings_DefaultNarration"].ToString();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "jscript10", "Validate();", true);
                    }
                    else if (cmbType.SelectedItem.Value == "GS_CDSLOPERATORID")
                    {
                        txtNarr.Text = DT.Rows[0]["GlobalSettings_DefaultNarration"].ToString();
                    }
                    else if (cmbType.SelectedItem.Value == "GS_CLMARGINAC")
                    {

                        cmbValu.SelectedValue = DT.Rows[0]["GlobalSettings_Value"].ToString();
                        txtCredit.Text = DT.Rows[0]["CreditName"].ToString();
                        txtDebit.Text = DT.Rows[0]["DebitName"].ToString();
                        txtCredit_hidden.Value = DT.Rows[0]["GlobalSettings_ExchangeCRMainAccount"].ToString();
                        txtDebit_hidden.Value = DT.Rows[0]["GlobalSettings_ExchangeDRMainAccount"].ToString();

                        ScriptManager.RegisterStartupScript(this, this.GetType(), "jscript10", "Validate();", true);

                    }
                    else
                    {
                        cmbValu.SelectedValue = DT.Rows[0]["GlobalSettings_Value"].ToString();
                        txtNarr.Text = DT.Rows[0]["GlobalSettings_DefaultNarration"].ToString();

                    }

                    //string tt = cmbValu.SelectedItem.Value;

                }
            }
            else if (data[0] == "AddNew")
            {
                clearField();
                //dtptoDate.Date = oDBEngine.GetDate();
                dtptoDate.Date = oDBEngine.GetDate();
                cmbType.SelectedIndex = 0;
                cmbValu.Items.Clear();
                Session["KeyVal"] = "n";
                ASPxCallbackPanel1.JSProperties["cpaddoredit"] = "Add";
            }
            else if (data[0] == "SaveNew")
            {
                ASPxCallbackPanel1.JSProperties["cpaddoredit"] = "savenw";
                dtCheck = oDBEngine.GetDataTable("Config_GlobalSettings", "*", "GlobalSettings_SegmentID='" + SegmentID + "' AND  GlobalSettings_Name='" + cmbType.SelectedItem.Value + "'");
                if (dtCheck.Rows.Count > 0)
                {
                    DataEx = "Y";
                }
                else
                {
                    if (cmbType.SelectedItem.Value == "GS_DORMANCY" || cmbType.SelectedItem.Value == "GS_HIGHVALUETRNDP" || cmbType.SelectedItem.Value == "GS_COMPANYNETWORTH")
                    {
                        dtCheck = oDBEngine.GetDataTable("Config_GlobalSettings", "*", "GlobalSettings_SegmentID='" + SegmentID + "' AND  GlobalSettings_Name='" + cmbType.SelectedItem.Value + "'");


                        //int NoofRowsAffect = oDBEngine.InsurtFieldValue("Config_GlobalSettings", "GlobalSettings_SegmentID, GlobalSettings_Name,GlobalSettings_Value,GlobalSettings_DefaultNarration,GlobalSettings_CreateUser,GlobalSettings_CreateDateTime ", " '" +    SegmentID    + "','" + cmbType.SelectedItem.Value + "','" + txtTValue.Text.ToString().Trim() + "','" + txtNarr.Text.ToString() + "','" + Session["userid"].ToString() + "','" + oDBEngine.GetDate().ToString() + "'");
                        int NoofRowsAffect = oDBEngine.InsurtFieldValue("Config_GlobalSettings", "GlobalSettings_SegmentID, GlobalSettings_Name,GlobalSettings_Value,GlobalSettings_DefaultNarration,GlobalSettings_CreateUser,GlobalSettings_CreateDateTime ", " '" + SegmentID + "','" + cmbType.SelectedItem.Value + "','" + txtTValue.Text.ToString().Trim() + "','" + txtNarr.Text.ToString() + "','" + Session["userid"].ToString() + "','" + oDBEngine.GetDate().ToString() + "'");

                    }


                    else if (cmbType.SelectedItem.Value == "GS_EXCHSEBIFEE" || cmbType.SelectedItem.Value == "GS_EXCHTRANCHARGE" || cmbType.SelectedItem.Value == "GS_EXCHTRANCHARGEST" || cmbType.SelectedItem.Value == "GS_EXCHCLCHARGE" || cmbType.SelectedItem.Value == "GS_EXCHCLCHARGEST" || cmbType.SelectedItem.Value == "GS_EXCHOBLACCOUNT")
                    {

                        string ACCR = "";
                        string ACSCR = "";
                        string ACDT = "";
                        string ACSDT = "";

                        if (txtCredit_hidden.Value != "" && txtCredit.Text.Trim() != "")
                        {
                            string[] CR = txtCredit_hidden.Value.ToString().Split('~');
                            ACCR = CR[0].ToString();
                        }
                        else
                        {
                            ACCR = null;
                        }
                        if (txtSubCredit_hidden.Value != "" && txtSubCredit.Text.Trim() != "")
                            ACSCR = txtSubCredit_hidden.Value;
                        else
                            ACSCR = null;

                        if (txtDebit_hidden.Value != "" && txtDebit.Text.Trim() != "")
                        {
                            string[] DR = txtDebit_hidden.Value.ToString().Split('~');
                            ACDT = DR[0].ToString();
                        }
                        else
                        {
                            ACDT = null;
                        }
                        if (txtSubDebit_hidden.Value != "" && txtSubDebit.Text.Trim() != "")
                            ACSDT = txtSubDebit_hidden.Value;
                        else
                            ACSDT = null;

                        //int NoofRowsAffect = oDBEngine.InsurtFieldValue("Config_GlobalSettings", "GlobalSettings_SegmentID, GlobalSettings_Name,GlobalSettings_Value,GlobalSettings_DefaultNarration,GlobalSettings_CreateUser,GlobalSettings_CreateDateTime,GlobalSettings_ExchangeCRMainAccount,GlobalSettings_ExchangeCRSubAccount,GlobalSettings_ExchangeDRMainAccount,GlobalSettings_ExchangeDRSubAccount ", " '" + SegmentID + "','" + cmbType.SelectedItem.Value + "','" + cmbValu.SelectedItem.Value + "','" + txtNarr.Text.ToString() + "','" + Session["userid"].ToString() + "','" + oDBEngine.GetDate().ToString() + "','" + ACCR + "','" + ACSCR + "','" + ACDT + "','" + ACSDT + "'");
                        int NoofRowsAffect = oDBEngine.InsurtFieldValue("Config_GlobalSettings", "GlobalSettings_SegmentID, GlobalSettings_Name,GlobalSettings_Value,GlobalSettings_DefaultNarration,GlobalSettings_CreateUser,GlobalSettings_CreateDateTime,GlobalSettings_ExchangeCRMainAccount,GlobalSettings_ExchangeCRSubAccount,GlobalSettings_ExchangeDRMainAccount,GlobalSettings_ExchangeDRSubAccount ", " '" + SegmentID + "','" + cmbType.SelectedItem.Value + "','" + cmbValu.SelectedItem.Value + "','" + txtNarr.Text.ToString() + "','" + Session["userid"].ToString() + "','" + oDBEngine.GetDate().ToString() + "','" + ACCR + "','" + ACSCR + "','" + ACDT + "','" + ACSDT + "'");


                    }
                    else if (cmbType.SelectedItem.Value == "GS_CDSLOPERATORID")
                    {

                        int NoofRowsAffect = oDBEngine.InsurtFieldValue("Config_GlobalSettings", "GlobalSettings_SegmentID, GlobalSettings_Name,GlobalSettings_DefaultNarration,GlobalSettings_CreateUser,GlobalSettings_CreateDateTime ", " '" + SegmentID + "','" + cmbType.SelectedItem.Value + "','" + txtNarr.Text.ToString() + "','" + Session["userid"].ToString() + "','" + oDBEngine.GetDate().ToString() + "'");
                    }
                    else if (cmbType.SelectedItem.Value == "GS_SEBIBRKG")
                    {
                        //dtCheck = oDBEngine.GetDataTable("Config_GlobalSettings", "*", "GlobalSettings_SegmentID='" + SegmentID + "' AND  GlobalSettings_Name='" + cmbType.SelectedItem.Value + "'");


                        int NoofRowsAffect = oDBEngine.InsurtFieldValue("Config_GlobalSettings", "GlobalSettings_SegmentID, GlobalSettings_Name,GlobalSettings_value,GlobalSettings_rate,GlobalSettings_DefaultNarration,GlobalSettings_CreateUser,GlobalSettings_CreateDateTime ", " '" + SegmentID + "','" + cmbType.SelectedItem.Value + "','" + cmbValu.SelectedItem.Value + "','" + txtTValue.Text.ToString().Trim() + "','" + txtNarr.Text.ToString() + "','" + Session["userid"].ToString() + "','" + oDBEngine.GetDate() + "'");

                    }

                    else if (cmbType.SelectedItem.Value == "GS_LCKBNK" || cmbType.SelectedItem.Value == "GS_LCKJV" || cmbType.SelectedItem.Value == "GS_LCKTRADE" || cmbType.SelectedItem.Value == "GS_LCKDEMAT" || cmbType.SelectedItem.Value == "GS_LCKALL")
                    {
                        if (cmbValu.SelectedValue == "1")
                        {
                            //dtCheck = oDBEngine.GetDataTable("Config_GlobalSettings", "*", "GlobalSettings_SegmentID='" + SegmentID + "' AND  GlobalSettings_Name='" + cmbType.SelectedItem.Value + "'");


                            int NoofRowsAffect = oDBEngine.InsurtFieldValue("Config_GlobalSettings", "GlobalSettings_SegmentID, GlobalSettings_Name,GlobalSettings_Value,GlobalSettings_Lockdays,GlobalSettings_DefaultNarration,GlobalSettings_CreateUser,GlobalSettings_CreateDateTime ", " '" + SegmentID + "','" + cmbType.SelectedItem.Value + "','" + cmbValu.SelectedItem.Value + "','" + txtTValue.Text.ToString().Trim() + "','" + txtNarr.Text.ToString() + "','" + Session["userid"].ToString() + "','" + oDBEngine.GetDate() + "'");

                        }

                        if (cmbValu.SelectedValue == "2")
                        {
                            int NoofRowsAffect = oDBEngine.InsurtFieldValue("Config_GlobalSettings", "GlobalSettings_SegmentID, GlobalSettings_Name,GlobalSettings_Value,GlobalSettings_Lockdate,GlobalSettings_DefaultNarration,GlobalSettings_CreateUser,GlobalSettings_CreateDateTime ", " '" + SegmentID + "','" + cmbType.SelectedItem.Value + "','" + cmbValu.SelectedItem.Value + "','" + Convert.ToDateTime(dtptoDate.Value).ToString("yyyy-MM-dd") + "','" + txtNarr.Text.ToString() + "','" + Session["userid"].ToString() + "','" + oDBEngine.GetDate().ToString() + "'");
                        }
                    }
                    else if (cmbType.SelectedItem.Value == "GS_CMBILLDATE")
                    {
                        int NoofRowsAffect = oDBEngine.InsurtFieldValue("Config_GlobalSettings", "GlobalSettings_SegmentID, GlobalSettings_Name,GlobalSettings_Value,GlobalSettings_CreateUser,GlobalSettings_CreateDateTime ", " '" + SegmentID + "','" + cmbType.SelectedItem.Value + "','" + cmbValu.SelectedItem.Value + "','" + Session["userid"].ToString() + "','" + oDBEngine.GetDate().ToString() + "'");
                    }
                    else if (cmbType.SelectedItem.Value == "GS_CLMARGINAC")
                    {
                        string ACCR = "";

                        string ACDT = "";

                        if (txtCredit_hidden.Value != "" && txtCredit.Text.Trim() != "")
                        {
                            string[] CR = txtCredit_hidden.Value.ToString().Split('~');
                            ACCR = CR[0].ToString();
                        }
                        else
                        {
                            ACCR = null;
                        }

                        if (txtDebit_hidden.Value != "" && txtDebit.Text.Trim() != "")
                        {
                            string[] DR = txtDebit_hidden.Value.ToString().Split('~');
                            ACDT = DR[0].ToString();
                        }
                        else
                        {
                            ACDT = null;
                        }


                        //int NoofRowsAffect = oDBEngine.InsurtFieldValue("Config_GlobalSettings", "GlobalSettings_SegmentID, GlobalSettings_Name,GlobalSettings_Value,GlobalSettings_DefaultNarration,GlobalSettings_CreateUser,GlobalSettings_CreateDateTime,GlobalSettings_ExchangeCRMainAccount,GlobalSettings_ExchangeCRSubAccount,GlobalSettings_ExchangeDRMainAccount,GlobalSettings_ExchangeDRSubAccount ", " '" + SegmentID + "','" + cmbType.SelectedItem.Value + "','" + cmbValu.SelectedItem.Value + "','" + txtNarr.Text.ToString() + "','" + Session["userid"].ToString() + "','" + oDBEngine.GetDate().ToString() + "','" + ACCR + "','" + ACSCR + "','" + ACDT + "','" + ACSDT + "'");
                        int NoofRowsAffect = oDBEngine.InsurtFieldValue("Config_GlobalSettings", "GlobalSettings_SegmentID, GlobalSettings_Name,GlobalSettings_Value,GlobalSettings_CreateUser,GlobalSettings_CreateDateTime,GlobalSettings_ExchangeCRMainAccount,GlobalSettings_ExchangeDRMainAccount ", " '" + SegmentID + "','" + cmbType.SelectedItem.Value + "','" + cmbValu.SelectedItem.Value + "'," + Session["userid"].ToString() + ",'" + oDBEngine.GetDate().ToString() + "','" + ACCR + "','" + ACDT + "'");


                    }

                    else
                    {

                        int NoofRowsAffect = oDBEngine.InsurtFieldValue("Config_GlobalSettings", "GlobalSettings_SegmentID, GlobalSettings_Name,GlobalSettings_Value,GlobalSettings_DefaultNarration,GlobalSettings_CreateUser,GlobalSettings_CreateDateTime ", " '" + SegmentID + "','" + cmbType.SelectedItem.Value + "','" + cmbValu.SelectedItem.Value + "','" + txtNarr.Text.ToString() + "','" + Session["userid"].ToString() + "','" + oDBEngine.GetDate().ToString() + "'");
                    }
                }


            }
            else if (data[0] == "SaveOld")
            {
                ASPxCallbackPanel1.JSProperties["cpaddoredit"] = "savenod";
                cmbType.Enabled = false;
                dtCheck = oDBEngine.GetDataTable("Config_GlobalSettings", "*", "GlobalSettings_SegmentID='" + SegmentID + "' AND  GlobalSettings_Name='" + cmbType.SelectedItem.Value + "' and GlobalSettings_ID !='" + data[1] + "' ");
                if (dtCheck.Rows.Count > 0)
                {
                    DataEx = "Y";
                }

                else
                {
                    if (cmbType.SelectedItem.Value == "GS_DORMANCY" || cmbType.SelectedItem.Value == "GS_HIGHVALUETRNDP" || cmbType.SelectedItem.Value == "GS_COMPANYNETWORTH")
                    {

                        oDBEngine.SetFieldValue("Config_GlobalSettings", " GlobalSettings_Name='" + cmbType.SelectedItem.Value + "',GlobalSettings_Value='" + txtTValue.Text.ToString().Trim() + "',GlobalSettings_DefaultNarration='" + txtNarr.Text.ToString() + "',GlobalSettings_ModifyUser='" + Session["userid"].ToString() + "',GlobalSettings_ModifyDateTime='" + oDBEngine.GetDate().ToString() + "'", "GlobalSettings_ID='" + data[1] + "' ");
                    }
                    else if (cmbType.SelectedItem.Value == "GS_EXCHSEBIFEE" || cmbType.SelectedItem.Value == "GS_EXCHTRANCHARGE" || cmbType.SelectedItem.Value == "GS_EXCHTRANCHARGEST" || cmbType.SelectedItem.Value == "GS_EXCHCLCHARGE" || cmbType.SelectedItem.Value == "GS_EXCHCLCHARGEST" || cmbType.SelectedItem.Value == "GS_EXCHOBLACCOUNT")
                    {
                        string ACCR = "";
                        string ACSCR = "";
                        string ACDT = "";
                        string ACSDT = "";

                        if (txtCredit_hidden.Value != "" && txtCredit.Text.Trim() != "")
                        {
                            string[] CR = txtCredit_hidden.Value.ToString().Split('~');
                            ACCR = CR[0].ToString();

                        }
                        else
                        {
                            ACCR = null;
                        }
                        if (txtSubCredit_hidden.Value != "" && txtSubCredit.Text.Trim() != "")
                            ACSCR = txtSubCredit_hidden.Value;
                        else
                            ACSCR = null;

                        if (txtDebit_hidden.Value != "" && txtDebit.Text.Trim() != "")
                        {
                            string[] DR = txtDebit_hidden.Value.ToString().Split('~');
                            ACDT = DR[0].ToString();
                        }
                        else
                        {
                            ACDT = null;
                        }
                        if (txtSubDebit_hidden.Value != "" && txtSubDebit.Text.Trim() != "")
                            ACSDT = txtSubDebit_hidden.Value;
                        else
                            ACSDT = null;
                        if (Convert.ToString(data[2]) == "1")
                        {
                            oDBEngine.SetFieldValue("Config_GlobalSettings", " GlobalSettings_Name='" + cmbType.SelectedItem.Value + "',GlobalSettings_Value='" + data[2] + "',GlobalSettings_DefaultNarration='" + txtNarr.Text.ToString() + "',GlobalSettings_ModifyUser='" + Session["userid"].ToString() + "',GlobalSettings_ModifyDateTime='" + oDBEngine.GetDate().ToString() + "',GlobalSettings_ExchangeCRMainAccount='" + ACCR + "',GlobalSettings_ExchangeCRSubAccount='" + ACSCR + "',GlobalSettings_ExchangeDRMainAccount='" + ACDT + "',GlobalSettings_ExchangeDRSubAccount='" + ACSDT + "'", "GlobalSettings_ID='" + data[1] + "' ");
                        }
                        else
                        {
                            oDBEngine.SetFieldValue("Config_GlobalSettings", " GlobalSettings_Name='" + cmbType.SelectedItem.Value + "',GlobalSettings_Value='" + data[2] + "',GlobalSettings_DefaultNarration='" + "" + "',GlobalSettings_ModifyUser='" + Session["userid"].ToString() + "',GlobalSettings_ModifyDateTime='" + oDBEngine.GetDate().ToString() + "',GlobalSettings_ExchangeCRMainAccount='" + "" + "',GlobalSettings_ExchangeCRSubAccount='" + "" + "',GlobalSettings_ExchangeDRMainAccount='" + "" + "',GlobalSettings_ExchangeDRSubAccount='" + "" + "'", "GlobalSettings_ID='" + data[1] + "' ");

                        }
                    }
                    else if (cmbType.SelectedItem.Value == "GS_CDSLOPERATORID")
                    {

                        oDBEngine.SetFieldValue("Config_GlobalSettings", " GlobalSettings_Name='" + cmbType.SelectedItem.Value + "',GlobalSettings_DefaultNarration='" + txtNarr.Text.ToString() + "',GlobalSettings_ModifyUser='" + Session["userid"].ToString() + "'", "GlobalSettings_ID='" + data[1] + "' ");
                    }
                    else if (cmbType.SelectedItem.Value == "GS_CDSLOPERATORID")
                    {

                        oDBEngine.SetFieldValue("Config_GlobalSettings", " GlobalSettings_Name='" + cmbType.SelectedItem.Value + "',GlobalSettings_DefaultNarration='" + txtNarr.Text.ToString() + "',GlobalSettings_ModifyUser='" + Session["userid"].ToString() + "'", "GlobalSettings_ID='" + data[1] + "' ");
                    }

                    else if (cmbType.SelectedItem.Value == "GS_CMBILLDATE")
                    {

                        oDBEngine.SetFieldValue("Config_GlobalSettings", " GlobalSettings_Name='" + cmbType.SelectedItem.Value + "',GlobalSettings_Value='" + data[2] + "',GlobalSettings_ModifyUser='" + Session["userid"].ToString() + "',GlobalSettings_ModifyDateTime='" + oDBEngine.GetDate().ToString() + "'", "GlobalSettings_ID='" + data[1] + "' ");
                    }

                    else if (cmbType.SelectedItem.Value == "GS_LCKBNK" || cmbType.SelectedItem.Value == "GS_LCKJV" || cmbType.SelectedItem.Value == "GS_LCKTRADE" || cmbType.SelectedItem.Value == "GS_LCKDEMAT" || cmbType.SelectedItem.Value == "GS_LCKALL")
                    {
                        DataTable dtcount = oDBEngine.GetDataTable("Config_GlobalSettings", "*", "GlobalSettings_SegmentID='" + SegmentID + "' AND  GlobalSettings_Name='" + cmbType.SelectedItem.Value + "' and GlobalSettings_ID  ='" + data[1] + "' ");
                        {
                            //if (dtcount.Rows.Count>0)
                            //if (cmbValu.SelectedValue == "1")
                            if (data[2] == "1")
                                //oDBEngine.SetFieldValue("Config_GlobalSettings", " GlobalSettings_Name='" + cmbType.SelectedItem.Value + "',GlobalSettings_Value='" + dtcount.Rows[0]["GlobalSettings_Value"] + "',GlobalSettings_LockDays='" + txtTValue.Text.ToString().Trim() + "',GlobalSettings_LockDate='" + Convert.ToDateTime(dtptoDate.Value).ToString("yyyy-MM-dd") + "',GlobalSettings_DefaultNarration='" + txtNarr.Text.ToString() + "',GlobalSettings_ModifyUser='" + Session["userid"].ToString() + "',GlobalSettings_ModifyDateTime='" + oDBEngine.GetDate().ToString() + "'", "GlobalSettings_ID='" + data[1] + "' ");
                                oDBEngine.SetFieldValue("Config_GlobalSettings", " GlobalSettings_Name='" + cmbType.SelectedItem.Value + "',GlobalSettings_Value='" + data[2].ToString().Trim() + "',GlobalSettings_LockDate= NULL,GlobalSettings_LockDays='" + txtTValue.Text.ToString().Trim() + "',GlobalSettings_DefaultNarration='" + txtNarr.Text.ToString() + "',GlobalSettings_ModifyUser='" + Session["userid"].ToString() + "',GlobalSettings_ModifyDateTime='" + oDBEngine.GetDate().ToString() + "'", "GlobalSettings_ID='" + data[1] + "' ");

                        }
                        if (data[2] == "2")
                        {
                            oDBEngine.SetFieldValue("Config_GlobalSettings", " GlobalSettings_Name='" + cmbType.SelectedItem.Value + "',GlobalSettings_Value='" + data[2].ToString().Trim() + "',GlobalSettings_LockDays= NULL,GlobalSettings_LockDate='" + Convert.ToDateTime(dtptoDate.Value).ToString("yyyy-MM-dd") + "',GlobalSettings_DefaultNarration='" + txtNarr.Text.ToString() + "',GlobalSettings_ModifyUser='" + Session["userid"].ToString() + "',GlobalSettings_ModifyDateTime='" + oDBEngine.GetDate().ToString() + "'", "GlobalSettings_ID='" + data[1] + "' ");

                        }
                    }
                    else if (cmbType.SelectedItem.Value == "GS_CLMARGINAC")
                    {
                        string ACCR = "";
                        string ACDT = "";

                        if (txtCredit_hidden.Value != "" && txtCredit.Text.Trim() != "")
                        {
                            string[] CR = txtCredit_hidden.Value.ToString().Split('~');
                            ACCR = CR[0].ToString();

                        }
                        else
                        {
                            ACCR = null;
                        }

                        if (txtDebit_hidden.Value != "" && txtDebit.Text.Trim() != "")
                        {
                            string[] DR = txtDebit_hidden.Value.ToString().Split('~');
                            ACDT = DR[0].ToString();
                        }
                        else
                        {
                            ACDT = null;
                        }

                        if (Convert.ToString(data[2]) == "1")
                        {
                            //oDBEngine.SetFieldValue("Config_GlobalSettings", " GlobalSettings_Name='" + cmbType.SelectedItem.Value + "',GlobalSettings_Value='" + data[2] + "',GlobalSettings_DefaultNarration='" + txtNarr.Text.ToString() + "',GlobalSettings_ModifyUser='" + Session["userid"].ToString() + "',GlobalSettings_ModifyDateTime='" + oDBEngine.GetDate().ToString() + "',GlobalSettings_ExchangeCRMainAccount='" + ACCR + "',GlobalSettings_ExchangeCRSubAccount='" + ACSCR + "',GlobalSettings_ExchangeDRMainAccount='" + ACDT + "',GlobalSettings_ExchangeDRSubAccount='" + ACSDT + "'", "GlobalSettings_ID='" + data[1] + "' ");
                            oDBEngine.SetFieldValue("Config_GlobalSettings", " GlobalSettings_Name='" + cmbType.SelectedItem.Value + "',GlobalSettings_Value='" + data[2] + "',GlobalSettings_ModifyUser='" + Session["userid"].ToString() + "',GlobalSettings_ModifyDateTime='" + oDBEngine.GetDate().ToString() + "',GlobalSettings_ExchangeCRMainAccount='" + ACCR + "',GlobalSettings_ExchangeDRMainAccount='" + ACDT + "'", "GlobalSettings_ID='" + data[1] + "' ");
                        }
                        else
                        {
                            oDBEngine.SetFieldValue("Config_GlobalSettings", " GlobalSettings_Name='" + cmbType.SelectedItem.Value + "',GlobalSettings_Value='" + data[2] + "',GlobalSettings_ModifyUser='" + Session["userid"].ToString() + "',GlobalSettings_ModifyDateTime='" + oDBEngine.GetDate().ToString() + "',GlobalSettings_ExchangeCRMainAccount='" + "" + "',GlobalSettings_ExchangeDRMainAccount='" + "" + "'", "GlobalSettings_ID='" + data[1] + "' ");

                        }

                    }
                    else
                    {

                        oDBEngine.SetFieldValue("Config_GlobalSettings", " GlobalSettings_Name='" + cmbType.SelectedItem.Value + "',GlobalSettings_Value='" + data[2].ToString().Trim() + "',GlobalSettings_DefaultNarration='" + txtNarr.Text.ToString() + "',GlobalSettings_ModifyUser='" + Session["userid"].ToString() + "',GlobalSettings_Modifydatetime='" + oDBEngine.GetDate().ToString() + "'", "GlobalSettings_ID='" + data[1] + "' ");

                    }
                }

            }
        }

        private void clearField()
        {

            txtCredit.Text = "";
            txtSubCredit.Text = "";
            txtDebit.Text = "";
            txtSubDebit.Text = "";
            txtCredit_hidden.Value = "";
            txtSubCredit_hidden.Value = "";
            txtDebit_hidden.Value = "";
            txtSubDebit_hidden.Value = "";
            txtTValue.Text = "";
            dtptoDate.Text = "";
            txtNarr.Text = "";

        }

        protected void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //  ViewState["CMBVAL"] = cmbValu.SelectedItem.Value;

            cmbValu.Items.Clear();
            BindCombo();

        }

        protected void cmbValu_SelectedIndexChanged(object sender, EventArgs e)
        {
            // ViewState["CMBVAL"] = cmbValu.SelectedItem.Value;

            // BindCombo();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "jscript57", "Validate();", true);
        }

        public void BindCombo()
        {
            switch (cmbType.SelectedValue.ToString().Trim())
            {
                case "GS_BRKDECIMAL":
                    // cmbValu.Items.Clear();
                    cmbValu.Items.Add(new ListItem("-Select-", "0"));
                    cmbValu.Items.Add(new ListItem("2-Decimal", "2"));
                    cmbValu.Items.Add(new ListItem("3-Decimal", "3"));
                    cmbValu.Items.Add(new ListItem("4-Decimal", "4"));
                    cmbValu.Items.Add(new ListItem("5-Decimal", "5"));
                    cmbValu.Items.Add(new ListItem("6-Decimal", "6"));
                    break;
                case "GS_BRKGROUND":
                    // cmbValu.Items.Clear();
                    cmbValu.Items.Add(new ListItem("-Select-", "0"));
                    cmbValu.Items.Add(new ListItem("Nearest Paisa", "1"));
                    cmbValu.Items.Add(new ListItem("Higher Paisa", "2"));
                    cmbValu.Items.Add(new ListItem("Lower Paisa", "3"));
                    cmbValu.Items.Add(new ListItem("Nearest 5-Paisa", "4"));
                    cmbValu.Items.Add(new ListItem("Higher 5-Paisa", "5"));
                    cmbValu.Items.Add(new ListItem("Lower 5-Paisa", "6"));
                    cmbValu.Items.Add(new ListItem("Truncate", "7"));
                    break;
                case "GS_AVGPATTERN":
                    // cmbValu.Items.Clear();
                    cmbValu.Items.Add(new ListItem("-Select-", "0"));
                    cmbValu.Items.Add(new ListItem("None", "1"));
                    cmbValu.Items.Add(new ListItem("Order Wise", "2"));
                    cmbValu.Items.Add(new ListItem("Instrument Wise", "3"));
                    cmbValu.Items.Add(new ListItem("Similar Price", "3"));
                    break;
                case "GS_MKTDECIMAL":
                    // cmbValu.Items.Clear();
                    cmbValu.Items.Add(new ListItem("-Select-", "0"));
                    cmbValu.Items.Add(new ListItem("2-Decimal", "2"));
                    cmbValu.Items.Add(new ListItem("3-Decimal", "3"));
                    cmbValu.Items.Add(new ListItem("4-Decimal", "4"));
                    cmbValu.Items.Add(new ListItem("5-Decimal", "5"));
                    cmbValu.Items.Add(new ListItem("6-Decimal", "6"));
                    break;
                case "GS_MKTROUND":
                    // cmbValu.Items.Clear();
                    cmbValu.Items.Add(new ListItem("-Select-", "0"));
                    cmbValu.Items.Add(new ListItem("Nearest Paisa", "1"));
                    cmbValu.Items.Add(new ListItem("Higher Paisa", "2"));
                    cmbValu.Items.Add(new ListItem("Lower Paisa", "3"));
                    cmbValu.Items.Add(new ListItem("Nearest 5-Paisa", "4"));
                    cmbValu.Items.Add(new ListItem("Higher 5-Paisa", "5"));
                    cmbValu.Items.Add(new ListItem("Lower 5-Paisa", "6"));
                    cmbValu.Items.Add(new ListItem("Truncate", "7"));
                    break;
                case "GS_EXCHOBLROUND":
                    // cmbValu.Items.Clear();
                    cmbValu.Items.Add(new ListItem("-Select-", "0"));
                    cmbValu.Items.Add(new ListItem("None", "1"));
                    cmbValu.Items.Add(new ListItem("Nearest-Rupee", "3"));
                    break;
                case "GS_STTACCOUNTING":
                    // cmbValu.Items.Clear();
                    cmbValu.Items.Add(new ListItem("-Select-", "0"));
                    cmbValu.Items.Add(new ListItem("Only Provisional", "1"));
                    cmbValu.Items.Add(new ListItem("Only Imported", "2"));
                    cmbValu.Items.Add(new ListItem("Provisional & Imported", "3"));
                    break;
                case "GS_FOACCOUNTINGJV":
                    // cmbValu.Items.Clear();
                    cmbValu.Items.Add(new ListItem("-Select-", "0"));
                    cmbValu.Items.Add(new ListItem("Separate Entries", "1"));
                    cmbValu.Items.Add(new ListItem("Consolidated Entries For The Day", "2"));
                    break;
                case "GS_EXCHSEBIFEE":
                    //cmbValu.Items.Clear();
                    cmbValu.Items.Add(new ListItem("-Select-", "0"));
                    cmbValu.Items.Add(new ListItem("Generate", "1"));
                    cmbValu.Items.Add(new ListItem("Do Not Generate", "2"));
                    break;
                case "GS_EXCHTRANCHARGE":
                    //cmbValu.Items.Clear();
                    cmbValu.Items.Add(new ListItem("-Select-", "0"));
                    cmbValu.Items.Add(new ListItem("Generate", "1"));
                    cmbValu.Items.Add(new ListItem("Do Not Generate", "2"));
                    break;
                case "GS_EXCHTRANCHARGEST":
                    // cmbValu.Items.Clear();
                    cmbValu.Items.Add(new ListItem("-Select-", "0"));
                    cmbValu.Items.Add(new ListItem("Generate", "1"));
                    cmbValu.Items.Add(new ListItem("Do Not Generate", "2"));
                    break;

                case "GS_EXCHCLCHARGE":
                    //cmbValu.Items.Clear();
                    cmbValu.Items.Add(new ListItem("-Select-", "0"));
                    cmbValu.Items.Add(new ListItem("Generate", "1"));
                    cmbValu.Items.Add(new ListItem("Do Not Generate", "2"));
                    break;
                case "GS_EXCHCLCHARGEST":
                    // cmbValu.Items.Clear();
                    cmbValu.Items.Add(new ListItem("-Select-", "0"));
                    cmbValu.Items.Add(new ListItem("Generate", "1"));
                    cmbValu.Items.Add(new ListItem("Do Not Generate", "2"));
                    break;
                case "GS_EXCHOBLACCOUNT":
                    // cmbValu.Items.Clear();
                    cmbValu.Items.Add(new ListItem("-Select-", "0"));
                    cmbValu.Items.Add(new ListItem("Default Account", "1"));
                    cmbValu.Items.Add(new ListItem("Selected Account", "2"));
                    break;
                case "GS_DORMANCY":
                    // cmbValu.Items.Clear();
                    cmbValu.Items.Add(new ListItem("-Select-", "0"));
                    cmbValu.Items.Add(new ListItem("Type Value in Days", "1"));
                    cmbValu.SelectedIndex = 1;
                    break;
                case "GS_HIGHVALUETRNDP":
                    //cmbValu.Items.Clear();
                    cmbValu.Items.Add(new ListItem("-Select-", "0"));
                    cmbValu.Items.Add(new ListItem("Type Amount In Rs.", "1"));
                    cmbValu.SelectedIndex = 1;

                    break;
                case "GS_EXCHTRANRND":
                    // cmbValu.Items.Clear();
                    cmbValu.Items.Add(new ListItem("-Select-", "0"));
                    cmbValu.Items.Add(new ListItem("None", "1"));
                    cmbValu.Items.Add(new ListItem("Nearest Rupee", "3"));
                    break;
                case "GS_HGVLAUTHORIZATIONS":
                    // cmbValu.Items.Clear();
                    cmbValu.Items.Add(new ListItem("-Select-", "0"));
                    cmbValu.Items.Add(new ListItem("Single", "1"));
                    cmbValu.Items.Add(new ListItem("Two", "2"));
                    cmbValu.Items.Add(new ListItem("Three", "3"));
                    cmbValu.Items.Add(new ListItem("Four", "4"));

                    break;
                case "GS_COMPANYNETWORTH":
                    // cmbValu.Items.Clear();
                    cmbValu.Items.Add(new ListItem("-Select-", "0"));
                    cmbValu.Items.Add(new ListItem("Type Amount In Rs.", "1"));
                    //cmbValu.SelectedIndex = 1;
                    break;

                case "GS_SEBIBRKG":
                    // cmbValu.Items.Clear();
                    cmbValu.Items.Add(new ListItem("-Select-", "0"));
                    cmbValu.Items.Add(new ListItem("Type Rate In (%).", "1"));
                    //cmbValu.SelectedIndex = 1;
                    break;

                case "GS_LCKBNK":
                    // cmbValu.Items.Clear();
                    //cmbValu.Items.Add(new ListItem("-Select-", "0"));
                    cmbValu.Items.Add(new ListItem("Lock data older by days", "1"));
                    cmbValu.Items.Add(new ListItem("Lockdata upto a date", "2"));
                    //cmbValu.SelectedIndex = 2;
                    break;

                case "GS_LCKJV":
                    // cmbValu.Items.Clear();
                    //cmbValu.Items.Add(new ListItem("-Select-", "0"));
                    cmbValu.Items.Add(new ListItem("Lock data older by days", "1"));
                    cmbValu.Items.Add(new ListItem("Lockdata upto a date", "2"));
                    //cmbValu.SelectedIndex = 1;
                    break;

                case "GS_LCKTRADE":
                    // cmbValu.Items.Clear();
                    //cmbValu.Items.Add(new ListItem("-Select-", "0"));
                    cmbValu.Items.Add(new ListItem("Lock data older by days", "1"));
                    cmbValu.Items.Add(new ListItem("Lockdata upto a date", "2"));

                    //cmbValu.SelectedIndex = 1;
                    break;

                case "GS_LCKDEMAT":
                    // cmbValu.Items.Clear();
                    //cmbValu.Items.Add(new ListItem("-Select-", "0"));
                    cmbValu.Items.Add(new ListItem("Lock data older by days", "1"));
                    cmbValu.Items.Add(new ListItem("Lockdata upto a date", "2"));
                    //cmbValu.SelectedIndex = 1;

                    break;

                case "GS_LCKALL":
                    // cmbValu.Items.Clear();
                    //cmbValu.Items.Add(new ListItem("-Select-", "0"));
                    cmbValu.Items.Add(new ListItem("Lock data older by days", "1"));
                    cmbValu.Items.Add(new ListItem("Lockdata upto a date", "2"));
                    //cmbValu.SelectedIndex = 1;
                    break;
                case "GS_CMBILLDATE":
                    // cmbValu.Items.Clear();
                    //cmbValu.Items.Add(new ListItem("-Select-", "0"));
                    cmbValu.Items.Add(new ListItem("Payout Date", "2"));
                    cmbValu.Items.Add(new ListItem("Trade Date", "1"));
                    //cmbValu.SelectedIndex = 1;
                    break;

                case "GS_CLMARGINAC":
                    // cmbValu.Items.Clear();
                    cmbValu.Items.Add(new ListItem("-Select-", "0"));
                    cmbValu.Items.Add(new ListItem("Yes", "1"));
                    cmbValu.Items.Add(new ListItem("No", "2"));
                    //cmbValu.SelectedIndex = 1;
                    break;
                case "GS_NSEPAYOUT":
                    // cmbValu.Items.Clear();
                    cmbValu.Items.Add(new ListItem("DFRS", "1"));
                    cmbValu.Items.Add(new ListItem("CADT", "2"));
                    //cmbValu.SelectedIndex = 1;
                    break;
                case "GS_EXCHCLCHARGETRADE":
                    cmbValu.Items.Add(new ListItem("Calcualte Per Trade", "1"));
                    cmbValu.Items.Add(new ListItem("Calcualte Per Day", "2"));
                    break;

            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "jscript47", "Validate();", true);

        }


    }
}