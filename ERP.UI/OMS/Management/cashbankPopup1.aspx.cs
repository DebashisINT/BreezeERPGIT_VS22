using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using BusinessLogicLayer;
using System.Web;
using ClsDropDownlistNameSpace;


namespace ERP.OMS.Management
{
    public partial class management_cashbankPopup1 : System.Web.UI.Page
    {
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();

        clsDropDownList clsDropDownList = new clsDropDownList();

        static DataTable dtReport = new DataTable();
        static DataTable dt = new DataTable();
        static int ReptID = 0;
        int NoOfRowEffectedRow;
        static string ID = "";
        protected void Page_Init(object sender, EventArgs e)
        {
            dsCompany.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SelectSegment.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            dsBranch.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            MainAccount.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
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

            if (!IsPostBack)
            {
                CmbClientBank.ClientSideEvents.EndCallback = "function(s, e) {var str = ['Third Party Account', '', '','','']; s.InsertItem(0, str, -1);}";
                dteDate.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
                dtDateWith.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
            }
            txtSettlementNo.Attributes.Add("onkeyup", "CallList(this,'SearchBySettlement',event)");
            txtIssuingBank.Attributes.Add("onkeyup", "CallListBank(this,'SearchByIssuingBank',event)");
            cmbInstType.Attributes.Add("onchange", "OnInstmentTypeChange()");
            cmbSubAc.Attributes.Add("onchange", "OnCashBankReportSubAcChange()");
        }
        protected void cmbType_SelectedIndexChanged1(object sender, EventArgs e)
        {
            string[,] MainAcc = null;
            if (cmbType.SelectedItem.Value.ToString() == "C")
            {
                MainAcc = oDBEngine.GetFieldValue("Master_MainAccount", "MainAccount_ReferenceID as CashBank_MainAccountID1, MainAccount_Name as MainAccount_Name1", " (MainAccount_BankCashType='Bank')or (MainAccount_BankCashType='Cash')", 2);

            }
            else
            {
                MainAcc = oDBEngine.GetFieldValue("Master_MainAccount", "MainAccount_ReferenceID as CashBank_MainAccountID1, MainAccount_Name as MainAccount_Name1", " (MainAccount_BankCashType<>'Bank')and (MainAccount_BankCashType<>'Cash')", 2);

            }
            if (MainAcc[0, 0] != "n")
            {
                clsDropDownList.AddDataToDropDownList(MainAcc, cmbMainAc, true);
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "JScript1", "TypeSelect1();", true);
        }
        protected void cmbCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbSegment.Items.Clear();
            string[,] Segment = oDBEngine.GetFieldValue("(SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='" + cmbCompany.SelectedItem.Value.ToString() + "') AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID", "LTRIM(RTRIM(A.EXCH_INTERNALID)) AS CashBank_ExchangeSegmentID ,TME.EXH_ShortName + '--' + A.EXCH_SEGMENTID AS EXCHANGENAME", null, 2);
            if (Segment[0, 0] != "n")
            {
                clsDropDownList.AddDataToDropDownList(Segment, cmbSegment, true);
            }
        }
        protected void cmbMainAc_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[,] Data1 = { { "@CashBank_MainAccountID", SqlDbType.VarChar.ToString(), cmbMainAc.SelectedItem.Value.ToString() } };
            DataTable dtview = oDBEngine.GetDatatable_StoredProcedure("SubAccountSelect", Data1);
            cmbSubAc.DataSource = dtview;
            cmbSubAc.DataTextField = "Contact_Name";
            cmbSubAc.DataValueField = "SubAccount_ReferenceID";
            cmbSubAc.DataBind();
            cmbSubAc.Items.Insert(0, "Select");
        }
        protected void CmbClientBank_OnCallback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string SubAccountRefID = e.Parameter;
            string[] subAccountCode = oDBEngine.GetFieldValue1("Master_SubAccount", "SubAccount_Code", "SubAccount_ReferenceID='" + SubAccountRefID + "'", 1);
            if (subAccountCode[0] != null)
            {
                string subAccountCodee = subAccountCode[0].ToString();
                dsgrdClientbank.SelectParameters[0].DefaultValue = subAccountCodee;
                CmbClientBank.Items.Clear();
                CmbClientBank.DataBind();

            }
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string year = dteDate.Date.Year.ToString();
            string catchType = cmbType.SelectedItem.Value.ToString();
            DataRow dr;
            if (ViewState["add"] == null)
            {
                dt.Columns.Clear();
                dt.Columns.Add(new DataColumn("Type", typeof(Char))); //0
                dt.Columns.Add(new DataColumn("Date", typeof(DateTime)));//1
                dt.Columns.Add(new DataColumn("Segement", typeof(String)));//2
                dt.Columns.Add(new DataColumn("Branch", typeof(String)));//3
                dt.Columns.Add(new DataColumn("CashBankAccount", typeof(String)));//4
                dt.Columns.Add(new DataColumn("SettlementNo", typeof(String)));//5
                dt.Columns.Add(new DataColumn("SettlementType", typeof(String)));//6
                dt.Columns.Add(new DataColumn("Narration", typeof(String)));//7
                dt.Columns.Add(new DataColumn("CashBank_MainAccountID", typeof(String)));//8
                dt.Columns.Add(new DataColumn("SubAccountID", typeof(String)));//9
                dt.Columns.Add(new DataColumn("CashBank_InstrumentType", typeof(String)));//10
                dt.Columns.Add(new DataColumn("CashBank_InstrumentDate", typeof(String)));//11
                dt.Columns.Add(new DataColumn("CashBank_InstrumentNumber", typeof(String)));//12
                dt.Columns.Add(new DataColumn("CashBank_PayeeAccountID", typeof(String)));//13
                dt.Columns.Add(new DataColumn("CashBank_AmountWithdrawl", typeof(Decimal)));//14
                dt.Columns.Add(new DataColumn("CashBank_AmountDeposit", typeof(Decimal)));//15
                dt.Columns.Add(new DataColumn("Year", typeof(string)));//16
                dt.Columns.Add(new DataColumn("Company", typeof(string)));//17
                dt.Columns.Add(new DataColumn("LineNarration", typeof(string)));//18
                dt.Columns.Add(new DataColumn("IssuingBank", typeof(String)));//19
                dt.Columns.Add(new DataColumn("CustomerBank", typeof(string)));//20
                dt.Columns.Add(new DataColumn("AuthLetterRef", typeof(string)));//21
                dt.Columns.Add(new DataColumn("SubAccount1", typeof(string)));//22
                dt.Columns.Add(new DataColumn("MainAccount1", typeof(string)));//23
                dt.Columns.Add(new DataColumn("InstType1", typeof(string)));//24
                dt.Columns.Add(new DataColumn("Payee1", typeof(string)));//25
            }
            else
            {
            }
            if (ID != "")
            {
                DataRow[] reportRow = dtReport.Select("CashReportID=" + ID + "");
                reportRow[0][1] = cmbPayee.SelectedItem.Value.ToString();
                reportRow[0][2] = Session["KeyVal"].ToString();
                reportRow[0][3] = objConverter.ArrangeDate(Convert.ToDateTime(dtDateWith.Value.ToString()).ToShortDateString());
                //reportRow[0][3] = e.NewValues["CashBank_InstrumentDate"];
                reportRow[0][4] = cmbMainAc.SelectedItem.Value.ToString();
                reportRow[0][5] = cmbSubAc.SelectedItem.Value.ToString();
                reportRow[0][6] = cmbInstType.SelectedItem.Value.ToString();
                reportRow[0][7] = txtInstNo.Text;
                reportRow[0][8] = dtDateWith.Value;
                try
                {
                    reportRow[0][9] = txtPayment.Text;
                }
                catch
                {
                    reportRow[0][9] = "0.00";
                }
                try
                {
                    reportRow[0][10] = txtReceipt.Text;
                }
                catch
                {
                    reportRow[0][10] = "0.00";
                }
                reportRow[0][11] = txtLineNarration.Text;
                try
                {
                    reportRow[0][12] = txtIssuingBank_hidden.Value;
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
                reportRow[0][15] = cmbSubAc.SelectedItem.Text;
                reportRow[0][16] = cmbMainAc.SelectedItem.Text;
                reportRow[0][17] = cmbInstType.SelectedItem.Text;
                reportRow[0][18] = cmbPayee.SelectedItem.Text;
                dtReport.AcceptChanges();
                ViewState["add"] = "a";
            }
            else
            {
                dr = dt.NewRow();
                dr[0] = cmbType.SelectedItem.Value.ToString();
                dr[1] = dteDate.Value;
                dr[2] = cmbSegment.SelectedItem.Value.ToString();
                dr[3] = cmbBranch.SelectedItem.Value.ToString();
                dr[4] = cmbCashBankAc.SelectedItem.Value.ToString();
                dr[5] = txtSettlementNo_hidden.Value;
                dr[6] = txtSettlementNo.Text;
                dr[7] = txtNarration.Text;
                dr[8] = cmbMainAc.SelectedItem.Value.ToString();
                dr[9] = cmbSubAc.SelectedItem.Value.ToString();
                dr[10] = cmbInstType.SelectedItem.Value.ToString();
                dr[11] = dtDateWith.Value;
                dr[12] = txtInstNo.Text;
                dr[13] = cmbPayee.SelectedItem.Value.ToString();
                try
                {
                    dr[14] = txtPayment.Text;
                }
                catch
                {
                    dr[14] = "0.00";
                }
                try
                {
                    dr[15] = txtReceipt.Text;
                }
                catch
                {
                    dr[15] = "0.00";
                }
                dr[16] = year;
                dr[17] = cmbCompany.SelectedItem.Value.ToString();
                dr[18] = txtLineNarration.Text;
                try
                {
                    dr[19] = txtIssuingBank_hidden.Value;
                }
                catch
                {
                    dr[19] = "0.00";
                }
                dr[20] = CmbClientBank.Value;
                dr[21] = txtAuthLetterRef.Text;
                dr[22] = cmbSubAc.SelectedItem.Text;
                dr[23] = cmbMainAc.SelectedItem.Text;
                dr[24] = cmbInstType.SelectedItem.Text;
                dr[25] = cmbPayee.SelectedItem.Text;
                dt.Rows.Add(dr);
                ReptID = ReptID + 1;
                if (dtReport.Rows.Count == 0)
                {
                    dtReport.Dispose();
                    dtReport = new DataTable();
                    dtReport.Columns.Add(new DataColumn("CashReportID", typeof(String))); //0
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
                }
                DataRow drReport = dtReport.NewRow();
                drReport[0] = ReptID.ToString();
                drReport[1] = cmbPayee.SelectedItem.Value.ToString();
                drReport[2] = "0";//Session["KeyVal"].ToString();
                drReport[3] = objConverter.ArrangeDate(Convert.ToDateTime(dtDateWith.Value.ToString()).ToShortDateString());
                //drReport[3] = e.NewValues["CashBank_InstrumentDate"];
                drReport[4] = cmbMainAc.SelectedItem.Value.ToString();
                drReport[5] = cmbSubAc.SelectedItem.Value.ToString();
                drReport[6] = cmbInstType.SelectedItem.Value.ToString();
                drReport[7] = txtInstNo.Text;
                drReport[8] = dtDateWith.Value;
                try
                {
                    drReport[9] = txtPayment.Text;
                }
                catch
                {
                    drReport[9] = "0.00";
                }
                try
                {
                    drReport[10] = txtReceipt.Text;
                }
                catch
                {
                    drReport[10] = "0.00";
                }
                drReport[11] = txtLineNarration.Text;
                try
                {
                    drReport[12] = txtIssuingBank_hidden.Value;
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
                drReport[15] = cmbSubAc.SelectedItem.Text;
                drReport[16] = cmbMainAc.SelectedItem.Text;
                drReport[17] = cmbInstType.SelectedItem.Text;
                drReport[18] = cmbPayee.SelectedItem.Text;
                dtReport.Rows.Add(drReport);
                dtReport.AcceptChanges();
                ViewState["add"] = "a";
            }
            ID = "";
            grdAdd.DataSource = dtReport.DefaultView;
            grdAdd.DataBind();
            ScriptManager.RegisterStartupScript(this, GetType(), "JScript", "Clear()", true);

        }
        protected void grdAdd_RowEditing(object sender, GridViewEditEventArgs e)
        {
            ID = (string)grdAdd.DataKeys[e.NewEditIndex].Value;
            EditGrid();
        }
        public void EditGrid()
        {
            DataRow[] reportRow = dtReport.Select("CashReportID=" + ID + "");
            cmbPayee.SelectedItem.Value = reportRow[0][1].ToString();
            string[,] MainAcc = oDBEngine.GetFieldValue("Master_MainAccount", "MainAccount_ReferenceID as CashBank_MainAccountID1, MainAccount_Name as MainAccount_Name1", " MainAccount_ReferenceID='" + reportRow[0][4].ToString() + "'", 2);
            if (MainAcc[0, 0] != "n")
            {
                cmbMainAc.SelectedItem.Value = MainAcc[0, 0].ToString();
            }
            string[,] Data1 = { { "@CashBank_MainAccountID", SqlDbType.VarChar.ToString(), MainAcc[0, 0].ToString() } };
            DataTable dtview = oDBEngine.GetDatatable_StoredProcedure("SubAccountSelect", Data1);
            cmbSubAc.DataSource = dtview;
            cmbSubAc.DataTextField = "Contact_Name";
            cmbSubAc.DataValueField = "SubAccount_ReferenceID";
            cmbSubAc.DataBind();
            cmbSubAc.SelectedItem.Value = reportRow[0][5].ToString();
            cmbInstType.SelectedItem.Value = reportRow[0][6].ToString();
            txtInstNo.Text = reportRow[0][7].ToString();
            dtDateWith.Value = Convert.ToDateTime(reportRow[0][8].ToString());
            txtPayment.Text = reportRow[0][9].ToString();
            txtReceipt.Text = reportRow[0][10].ToString();
            txtNarration.Text = reportRow[0][11].ToString();
        }
        protected void grdAdd_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string KeyName = (string)grdAdd.DataKeys[e.RowIndex].Value;
            foreach (DataRow dr in dtReport.Rows)
            {
                if (dr.ItemArray[0].ToString() == KeyName)
                {
                    dr.Delete();
                }
            }
            dtReport.AcceptChanges();
            ViewState["add"] = "a";
            grdAdd.DataSource = dtReport.DefaultView;
            grdAdd.DataBind();
        }
    }
}
