using System;
using System.Configuration;
using System.Data;
using System.Drawing.Printing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_ChequePrintingintermediate : System.Web.UI.Page
    {
        DataTable DtChequePrint = new DataTable();
        DataTable DtChequePrintSession = new DataTable();
        DataTable dtReprint = new DataTable();

        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);

        int i = 0;
        int k = 0;
        int l = 0;
        int j = 0;
        string StrID = "";
        string Bank = "";
        string Fromdate = "";
        string ToDate = "";
        string strBank = "";
        string strchequenum = "";
        string[] strChequeNumber = null;
        string strchequeType = "";
        string Account = "";
        string ClientUcc = "";
        protected void Page_Load(object sender, EventArgs e)
        {

            this.Page.ClientScript.RegisterStartupScript(GetType(), "heightR", "<script>HideTr();</script>");
            if (Request.QueryString["ID"].ToString() != "" && Request.QueryString["bank"].ToString() != "" && Request.QueryString["Fromdate"].ToString() != "" && Request.QueryString["strBank"].ToString() != "" && Request.QueryString["ChequeNumber"].ToString() != "" && Request.QueryString["Todate"].ToString() != "" || Request.QueryString["BankType"].ToString() != "")
            {
                StrID = Request.QueryString["ID"].ToString();
                hdnID.Value = StrID;
                Bank = Request.QueryString["bank"].ToString();
                Fromdate = Request.QueryString["Fromdate"].ToString();
                strBank = Request.QueryString["strBank"].ToString();
                hdnbank.Value = strBank;
                strChequeNumber = Request.QueryString["ChequeNumber"].ToString().Split(',');
                strchequenum = Request.QueryString["ChequeNumber"].ToString();
                ToDate = Request.QueryString["Todate"].ToString();
                strchequeType = Request.QueryString["BankType"].ToString();
                Account = Request.QueryString["Account"].ToString();
                ClientUcc = Request.QueryString["Ucc"].ToString();
                hdnFromdate.Value = Convert.ToDateTime(Fromdate).ToString("yyyy-MM-dd");
                hdnTodate.Value = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd");

                if (!IsPostBack)
                {
                    if (Session["Data"] != null)
                    {

                        ViewState["Cheques"] = (DataTable)Session["Data"];

                        DtChequePrintSession = (DataTable)ViewState["Cheques"];
                    }


                    if (StrID != "" && Bank != "" && Fromdate != "" && ToDate != "" && Session["LastCompany"] != null)
                    {

                        if (strchequeType == "HC")
                        {
                            try
                            {

                                gridview.DataSource = DtChequePrintSession;
                                gridview.DataBind();
                                this.CrystalReportViewer1.Visible = false;
                                this.CrystalReportViewer2.Visible = false;


                            }
                            catch (Exception err)
                            {

                                //MessageBox.Show(err.ToString());

                            }


                        }
                        else if (strchequeType == "UTI")
                        {
                            try
                            {

                                gridview.DataSource = DtChequePrintSession;
                                gridview.DataBind();
                                this.CrystalReportViewer1.Visible = false;
                                this.CrystalReportViewer2.Visible = false;


                            }
                            catch (Exception err)
                            {

                                //MessageBox.Show(err.ToString());

                            }
                        }

                    }

                    else
                    {


                        ScriptManager.RegisterStartupScript(this, GetType(), "abc", "<script>alert('Please LogOff And Retry!');</script>", true);
                    }
                }
            }
        }


        protected void gridview_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataTable dtbl2 = new DataTable();
            dtbl2 = (DataTable)Session["Data"];
            gridview.DataSource = dtbl2;
            gridview.PageIndex = e.NewPageIndex;
            gridview.DataBind();


        }
        protected void btnExport_Click(object sender, EventArgs e)
        {

            if (ViewState["Cheques"] != null)
            {
                DtChequePrintSession = (DataTable)ViewState["Cheques"];
            }

            if (ddlExport.SelectedValue == "PDF")
            {
                ReportDocument PrintDocument = new ReportDocument();
                string[] connPath = ConfigurationManager.AppSettings["DBConnectionDefault"].Split(';');
                string path;
                path = string.Empty;
                path = HttpContext.Current.Server.MapPath("..\\Reports\\chequereport.rpt");
                PrintDocument.Load(path);
                PrintDocument.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
                PrintDocument.SetDataSource(DtChequePrintSession);
                DataSet ds = new DataSet();
                ds.Tables.Add(DtChequePrintSession);
                ds.Tables[0].TableName = "DtChequePrintSession";
                PrintDocument.DataSourceConnections.Clear();
                PrintDocument.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "Cheque Report");
            }

        }
        protected void ddlPrintMode_SelectedIndexChanged(object sender, EventArgs e)
        {

            btnUpdateCheque.Enabled = true;

            DtChequePrint = oDBEngine.GetDataTable("(Select CashBankDetail_BranchID,CashBankDetail_ID,cashbankdetail_Subaccountid,CashBankDetail_ClientBankID,case when master_subaccount.SubAccount_Name is null then MainAccount_Name else master_subaccount.SubAccount_Name end as MainAccount_Name,cashbank_vouchernumber,Convert(varchar(20),CashBankDetail_InstrumentDate,106) as CashBankDetail_InstrumentDate,dbo.[format_number](ABS(CashBankDetail_PaymentAmount)) as Payment,master_subaccount.SubAccount_Name,Substring(dbo.fNumToWords(ABS(CashBankDetail_PaymentAmount)),7,len(dbo.fNumToWords(ABS(CashBankDetail_PaymentAmount)))) as WordPayment from (Select CashBankDetail_BranchID,CashBankDetail_ID,MainAccount_Name,CashBankDetail_Subaccountid,CashBank_CashBankID,CashBankDetail_ClientBankID,cashbankdetail_mainaccountid,cashbank_vouchernumber,CashBankDetail_PaymentAmount,Convert(varchar(20),CashBankDetail_InstrumentDate,106) as CashBankDetail_InstrumentDate,dbo.[format_number](ABS(CashBankDetail_PaymentAmount)) as Payment from trans_cashbankdetail,Master_MainAccount,trans_cashbankvouchers", "ROW_NUMBER()  OVER (ORDER BY  CashBankDetail_BranchID,mainaccount_name) as CashBankDetail_RecordID,CashBankDetail_BranchID,CashBankDetail_ID,CashBankDetail_ClientBankID,MainAccount_Name,cashbank_vouchernumber,Convert(varchar(11),CashBankDetail_InstrumentDate,105) as CashBankDetail_InstrumentDate,Payment,WordPayment,cbd_AccountNumber,cbd_BankCode,cbd_id,cbd_cntID,bnk_bankName,cnt_Ucc from(Select CashBankDetail_BranchID,CashBankDetail_ID,CashBankDetail_ClientBankID,MainAccount_Name,cashbank_vouchernumber,Convert(varchar(20),CashBankDetail_InstrumentDate,106) as CashBankDetail_InstrumentDate,Payment,WordPayment,cbd_AccountNumber,cbd_BankCode,cbd_id,cbd_cntID", "CashBank_TransactionType='P' and trans_cashbankdetail.cashbankdetail_InstrumentType='C' and trans_cashbankdetail.cashbankdetail_mainaccountid=Master_MainAccount.mainaccount_accountcode and trans_cashbankdetail.cashbankdetail_voucherID=trans_cashbankvouchers.CashBank_ID and Master_MainAccount.MainAccount_SubledgerType='Customers' AND cashbank_VoucherNumber Like 'P%' and CashBankDetail_CashBankID Is not Null and (trans_cashbankdetail.cashbankdetail_instrumentnumber ='' or len(trans_cashbankdetail.cashbankdetail_instrumentnumber)=0 or trans_cashbankdetail.cashbankdetail_instrumentnumber is null) and trans_cashbankdetail.cashbankdetail_ID In(" + hdnID.Value + ")) as a1 left outer join master_subaccount on(a1.cashbankdetail_Subaccountid=Master_SubAccount.SubAccount_Code and Master_SubAccount.SubAccount_MainAcReferenceID=a1.cashbankdetail_mainaccountid and CashBankDetail_ID In(" + hdnID.Value + ")))as a2 left outer join tbl_trans_contactbankdetails on(a2.cashbankdetail_Subaccountid=cbd_cntid and cbd_id IN(" + hdnbank.Value + ")))as a3 left outer join tbl_master_bank ON(bnk_id=a3.cbd_bankCode) left outer join tbl_master_contact ON(cnt_internalID=a3.cbd_cntID)", "CashBankDetail_BranchID,mainaccount_name");

            if (DtChequePrint.Rows.Count > 0)
            {
                if (Session["Data"] != null)
                {
                    DtChequePrintSession = (DataTable)ViewState["Cheques"];
                }

                if (ddlPrintMode.SelectedValue == "1")
                {
                    if (strchequeType == "HC")
                    {
                        this.CrystalReportSource1.ReportDocument.SetDataSource(DtChequePrint);
                        this.CrystalReportSource1.ReportDocument.PrintOptions.PrinterName = "";
                        this.CrystalReportSource1.ReportDocument.SetParameterValue("@Field", (object)"A/C");
                        this.CrystalReportSource1.ReportDocument.SetParameterValue("@Field2", (object)"A/C No.");
                        this.CrystalReportSource1.ReportDocument.SetParameterValue("@Field3", (object)Account);
                        this.CrystalReportSource1.ReportDocument.SetParameterValue("@Field4", (object)ClientUcc);
                        this.CrystalReportViewer1.PrintMode = CrystalDecisions.Web.PrintMode.Pdf;
                        this.CrystalReportViewer1.HasPrintButton = true;
                        this.CrystalReportViewer1.HasViewList = false;
                        this.CrystalReportViewer1.HasGotoPageButton = false;
                        this.CrystalReportViewer1.HasExportButton = false;
                        this.CrystalReportViewer1.HasPageNavigationButtons = true;
                        this.CrystalReportViewer1.HasSearchButton = false;
                        this.CrystalReportViewer1.HasZoomFactorList = false;
                        this.CrystalReportViewer1.HasRefreshButton = false;
                        this.CrystalReportViewer1.HasDrillUpButton = false;
                        this.CrystalReportViewer1.HasToggleGroupTreeButton = false;
                        this.CrystalReportViewer1.DisplayGroupTree = false;
                        this.CrystalReportViewer1.SeparatePages = false;
                        this.CrystalReportViewer1.Visible = true;
                        DataSet ds = new DataSet();
                        ds.Tables.Add(DtChequePrint);
                        ds.Tables[0].TableName = "DtChequePrint";
                        //ds.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\DtChequePrint.xsd");

                        this.Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>Show();</script>");
                        this.Page.ClientScript.RegisterStartupScript(GetType(), "heightM", "<script>height();</script>");

                    }
                    else if (strchequeType == "UTI")
                    {

                        this.CrystalReportSource2.ReportDocument.SetDataSource(DtChequePrint);
                        this.CrystalReportSource2.ReportDocument.PrintOptions.PrinterName = "";
                        this.CrystalReportSource2.ReportDocument.SetParameterValue("@Field", (object)"A/C");
                        this.CrystalReportSource2.ReportDocument.SetParameterValue("@Field2", (object)"A/C No.");
                        //this.CrystalReportSource2.ReportDocument.SetParameterValue("@Field3", (object)Account);
                        //this.CrystalReportSource2.ReportDocument.SetParameterValue("@Field4", (object)ClientUcc);
                        this.CrystalReportViewer2.PrintMode = CrystalDecisions.Web.PrintMode.Pdf;
                        this.CrystalReportViewer2.HasPrintButton = true;
                        this.CrystalReportViewer2.HasViewList = false;
                        this.CrystalReportViewer2.HasGotoPageButton = false;
                        this.CrystalReportViewer2.HasExportButton = false;
                        this.CrystalReportViewer2.HasPageNavigationButtons = false;
                        this.CrystalReportViewer2.HasSearchButton = false;
                        this.CrystalReportViewer2.HasZoomFactorList = false;
                        this.CrystalReportViewer2.HasRefreshButton = false;
                        this.CrystalReportViewer2.HasDrillUpButton = false;
                        this.CrystalReportViewer2.HasToggleGroupTreeButton = false;
                        this.CrystalReportViewer2.DisplayGroupTree = false;
                        this.CrystalReportViewer2.SeparatePages = false;
                        this.CrystalReportViewer2.Visible = true;
                        this.CrystalReportViewer1.Visible = false;
                        DataSet ds = new DataSet();
                        ds.Tables.Add(DtChequePrint);
                        ds.Tables[0].TableName = "DtChequePrint";
                        this.Page.ClientScript.RegisterStartupScript(GetType(), "heightM", "<script>height();</script>");


                    }
                }
                else if (ddlPrintMode.SelectedValue == "2")
                {
                    this.CrystalReportViewer1.Visible = false;
                    this.CrystalReportViewer2.Visible = false;
                    ReportDocument PrintDoc = new ReportDocument();
                    string[] connPath = ConfigurationManager.AppSettings["DBConnectionDefault"].Split(';');
                    string path;
                    path = string.Empty;
                    path = HttpContext.Current.Server.MapPath("..\\Reports\\ChequePrintlazerfinal.rpt");
                    PrintDoc.Load(path);
                    PrintDoc.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
                    PrintDoc.SetDataSource(DtChequePrint);
                    PrintDoc.SetParameterValue("@Field", (object)"A/C");
                    PrintDoc.SetParameterValue("@Field2", (object)"A/C No.");
                    PrintDoc.SetParameterValue("@Field3", (object)Account);
                    PrintDoc.SetParameterValue("@Field4", (object)ClientUcc);
                    PrintDoc.DataSourceConnections.Clear();
                    this.Page.ClientScript.RegisterStartupScript(GetType(), "heightP", "<script>height();</script>");
                    this.Page.ClientScript.RegisterStartupScript(GetType(), "heightQ", "<script>Hide();</script>");
                    PrintDoc.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "Lazer Cheque Print");

                }
                ddlPrintMode.SelectedIndex = 0;
            }
        }
        protected void btnUpdateCheque_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "abc11", "alert('Are you sure all Cheque Printed Properly?');", true);

            this.Page.ClientScript.RegisterStartupScript(GetType(), "heightS", "<script>ShowTr();</script>");
        }
        protected void gridview_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                ((CheckBox)e.Row.FindControl("cbSelectAll")).Attributes.Add("onclick", "javascript:SelectAllInterSegment('" + ((CheckBox)e.Row.FindControl("cbSelectAll")).ClientID + "')");
            }
        }
        protected void gridview_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

        }
        protected void btnYes_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gridview.Rows)
            {
                CheckBox ChkDelivery = (CheckBox)row.FindControl("ChkDelivery");
                try
                {
                    if (ChkDelivery.Checked == true)
                    {
                        string ID = row.Cells[1].Text.ToString().Trim();
                        string instrumentNumber = row.Cells[2].Text.ToString().Trim();
                        string MainAccountName = row.Cells[3].Text.ToString().Trim();
                        string VoucherNumber = row.Cells[4].Text.ToString().Trim();
                        string[] sdate1 = row.Cells[5].Text.ToString().Trim().Split('-');
                        string sdate = sdate1[2].ToString() + "-" + sdate1[1].ToString() + "-" + sdate1[0].ToString();
                        string[] TransDate = row.Cells[6].Text.ToString().Trim().Split('-');
                        string TransactionDate = TransDate[2].ToString() + "-" + TransDate[1].ToString() + "-" + TransDate[0].ToString();
                        string Payment = row.Cells[7].Text.ToString().Trim();
                        string SubaccountID = row.Cells[8].Text.ToString().Trim();
                        string MainaccountID = row.Cells[9].Text.ToString().Trim();
                        //string GroupID = row.Cells[10].Text.ToString().Trim();
                        //string GroupMaster = row.Cells[11].Text.ToString().Trim();
                        //string GroupDescription = row.Cells[12].Text.ToString().Trim();
                        string branchID = row.Cells[10].Text.ToString().Trim();
                        string BranchDescription = row.Cells[11].Text.ToString().Trim();
                        string BankId = row.Cells[12].Text.ToString();
                        string ClientBankID = row.Cells[13].Text.ToString();
                        string ClientBankName = row.Cells[14].Text.ToString();

                        i = oDBEngine.SetFieldValue("trans_cashbankdetail", "Cashbankdetail_IsLocked='Y',cashbankdetail_modifyuser='" + Session["userid"].ToString() + "',cashbankdetail_ModifyDateTime='" + oDBEngine.GetDate().ToString() + "',CashBankDetail_InstrumentNumber='" + instrumentNumber + "',CashBankDetail_InstrumentDate='" + sdate + "',CashBankDetail_ClientBankID='" + ClientBankID + "'", "CashbankDetail_ID IN(" + ID + ") And Cashbankdetail_Mainaccountid='" + MainaccountID + "'  AND CashBankDetail_subaccountid='" + SubaccountID + "' ");
                        j = oDBEngine.SetFieldValue("trans_AccountsLedger", "AccountsLedger_InstrumentNumber='" + instrumentNumber + "',AccountsLedger_InstrumentDate='" + sdate + "',AccountsLedger_CashBankName='" + ClientBankName + "'", "AccountsLedger_TransactionType='Cash_Bank' AND AccountsLedger_CompanyID='" + Session["LastCompany"].ToString() + "'AND AccountsLedger_ExchangeSegmentID=" + Session["usersegid"].ToString() + " AND AccountsLedger_TransactionDate='" + Convert.ToDateTime(TransactionDate).ToString("yyyy-MM-dd") + "' AND AccountsLedger_TransactionReferenceID='" + VoucherNumber + "' AND AccountsLedger_MainAccountID='" + MainaccountID + "' AND AccountsLedger_SubAccountID='" + SubaccountID + "'");
                        oDBEngine.SetFieldValue("Trans_ChequePrintLog", "ChequePrintLog_Status='U',ChequePrintLog_InstrumentNumber='" + instrumentNumber + "',ChequePrintLog_ModifyUser='" + Session["userid"].ToString() + "',ChequePrintLog_ModifyDateTime='" + oDBEngine.GetDate().ToString() + "'", "ChequePrintLog_ReferenceID=" + ID.ToString() + " and ChequePrintLog_InstrumentNumber='" + instrumentNumber.ToString() + "' AND chequeprintlog_subaccountid='" + SubaccountID + "'   ");

                    }
                }
                catch (Exception ex)
                {

                }
                finally
                {

                }

            }
            ScriptManager.RegisterStartupScript(this, GetType(), "abc1115", "alert('Updated Successfully!');", true);

            this.Page.ClientScript.RegisterStartupScript(GetType(), "heightT", "<script>parent.editwin.close();</script>");

        }
        protected void btnNo_Click(object sender, EventArgs e)
        {

        }
        protected void btnUpdateprint_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gridview.Rows)
            {
                CheckBox ChkDelivery = (CheckBox)row.FindControl("ChkDelivery");
                try
                {
                    if (ChkDelivery.Checked == true)
                    {
                        string ID = row.Cells[1].Text.ToString().Trim();
                        string instrumentNumber = row.Cells[2].Text.ToString().Trim();
                        string MainAccountName = row.Cells[3].Text.ToString().Trim();
                        string VoucherNumber = row.Cells[4].Text.ToString().Trim();
                        string[] sdate1 = row.Cells[5].Text.ToString().Trim().Split('-');
                        string sdate = sdate1[2].ToString() + "-" + sdate1[1].ToString() + "-" + sdate1[0].ToString();
                        string[] TransDate = row.Cells[6].Text.ToString().Trim().Split('-');
                        string TransactionDate = TransDate[2].ToString() + "-" + TransDate[1].ToString() + "-" + TransDate[0].ToString();
                        string Payment = row.Cells[7].Text.ToString().Trim();
                        string SubaccountID = row.Cells[8].Text.ToString().Trim();
                        string MainaccountID = row.Cells[9].Text.ToString().Trim();
                        string GroupID = row.Cells[10].Text.ToString().Trim();
                        string GroupMaster = row.Cells[11].Text.ToString().Trim();
                        string GroupDescription = row.Cells[12].Text.ToString().Trim();
                        string branchID = row.Cells[13].Text.ToString().Trim();
                        string BranchDescription = row.Cells[14].Text.ToString().Trim();
                        string BankId = row.Cells[15].Text.ToString();
                        string ClientBankID = row.Cells[16].Text.ToString();
                        string ClientBankName = row.Cells[17].Text.ToString();

                        i = oDBEngine.SetFieldValue("trans_cashbankdetail", "CashBankDetail_InstrumentNumber='" + instrumentNumber + "',CashBankDetail_InstrumentDate='" + sdate + "',CashBankDetail_ClientBankID='" + ClientBankID + "'", "CashbankDetail_ID IN(" + ID + ")");
                        j = oDBEngine.SetFieldValue("trans_AccountsLedger", "AccountsLedger_InstrumentNumber='" + instrumentNumber + "',AccountsLedger_InstrumentDate='" + sdate + "',AccountsLedger_CashBankName='" + ClientBankName + "'", "AccountsLedger_TransactionType='Cash_Bank' AND AccountsLedger_CompanyID='" + Session["LastCompany"].ToString() + "'AND AccountsLedger_ExchangeSegmentID=" + Session["usersegid"].ToString() + " AND AccountsLedger_TransactionDate='" + Convert.ToDateTime(TransactionDate).ToString("yyyy-MM-dd") + "' AND AccountsLedger_TransactionReferenceID='" + VoucherNumber + "' AND AccountsLedger_MainAccountID='" + MainaccountID + "' AND AccountsLedger_SubAccountID='" + SubaccountID + "'");
                    }
                }
                catch (Exception ex)
                {
                    lblerror.Text = ex.Message.ToString();
                }
                finally
                {

                }

            }
            ScriptManager.RegisterStartupScript(this, GetType(), "abc11111", "alert('Updated Successfully!');", true);

            if (ViewState["Cheques"] != null)
            {
                DtChequePrintSession = (DataTable)ViewState["Cheques"];

                if (ddlExport.SelectedValue == "PDF")
                {
                    ReportDocument PrintDocument = new ReportDocument();
                    string[] connPath = ConfigurationManager.AppSettings["DBConnectionDefault"].Split(';');
                    string path;
                    path = string.Empty;
                    path = HttpContext.Current.Server.MapPath("..\\Reports\\chequereport.rpt");
                    PrintDocument.Load(path);
                    PrintDocument.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
                    PrintDocument.SetDataSource(DtChequePrintSession);
                    DataSet ds = new DataSet();
                    ds.Tables.Add(DtChequePrintSession);
                    ds.Tables[0].TableName = "DtChequePrintSession";
                    PrintDocument.DataSourceConnections.Clear();
                    PrintDocument.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "Cheque Report");
                }
            }
            this.Page.ClientScript.RegisterStartupScript(GetType(), "heightS", "<script>parent.editwin.close();</script>");


        }
    }
}