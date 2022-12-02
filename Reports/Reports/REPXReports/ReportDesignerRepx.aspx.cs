using BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.DataAccess.Sql;
using DevExpress.XtraReports.UI;
using DataAccessLayer;
using System.Data.SqlClient;
using DevExpress.DataAccess.ConnectionParameters;

namespace Reports.Reports.REPXReports
{
    public partial class ReportDesignerRepx : System.Web.UI.Page
    {
        BusinessLogicLayer.ReportLayout rpLayout = new BusinessLogicLayer.ReportLayout();
        BusinessLogicLayer.ReportData rptData = new BusinessLogicLayer.ReportData();

        protected void Page_Load(object sender, EventArgs e)
        {
            // The name for a file to save a report.
            if (!IsPostBack && !IsCallback)
            {
                StartDate.Value = HttpContext.Current.Request.QueryString["StartDate"];
                EndDate.Value = HttpContext.Current.Request.QueryString["EndDate"];
                if (string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["LoadrptName"]))
                {
                    // Run the Wizard to create a new report.
                    RptName.Value = HttpContext.Current.Request.QueryString["NewReport"];
                    string tempFile = RptName.Value;
                    CreateReport(tempFile);
                }
                else if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["LoadrptName"]))
                {
                    // Load report.
                    RptName.Value = HttpContext.Current.Request.QueryString["LoadrptName"];
                    string tempFile = RptName.Value;
                    LoadReport(tempFile);
                }
            }

            if (!IsPostBack)
            {
                HDERepornName.Value = Convert.ToString(Request.QueryString["reportname"]);
            }
        }

        private void CreateReport(string fileName)
        {
            DevExpress.DataAccess.Sql.SqlDataSource sql = GenerateSqlDataSource();
            string RptModuleName = Convert.ToString(Session["NewRptModuleName"]);
            var rpt = new DevExpress.XtraReports.UI.XtraReport();
            string rptName = fileName;
            if (string.IsNullOrEmpty(Page.Title))
            {
                Page.Title = rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
            }
            XtraReport newXtraReport = new XtraReport();
            newXtraReport.DataSource = sql;
            ASPxReportDesigner1.OpenReport(newXtraReport);
        }

        private void LoadReport(string fileName)
        {
            string rptName = fileName;
            string filePath = "";
            string RptModuleName = Convert.ToString(Session["NewRptModuleName"]);
            if (string.IsNullOrEmpty(Page.Title))
            {
                Page.Title = rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
            }
            if (RptModuleName == "StockTrialSumm")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/Inventory/StockTrial/Summary/" + rptName + ".repx");
            }
            else if (RptModuleName == "StockTrialDet")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/Inventory/StockTrial/Details/" + rptName + ".repx");
            }
                //Rev Debashis
            else if (RptModuleName == "StockTrialProd")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/Inventory/StockTrial/Product/" + rptName + ".repx");
            }
                //End of Rev Debashis
            else if (RptModuleName == "StockTrialWH")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/Inventory/StockTrial/Warehouse/" + rptName + ".repx");
            }
            else if (RptModuleName == "StockTrial1")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/Inventory/StockTrial1/" + rptName + ".repx");
            }
            else if (RptModuleName == "Invoice")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/SalesInvoice/DocDesign/Normal/" + rptName + ".repx");
            }
            else if (RptModuleName == "TSInvoice")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/SalesInvoice/DocDesign/Transit/" + rptName + ".repx");
            }
            else if (RptModuleName == "TPInvoice")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/TransitPurchaseInvoice/DocDesign/Normal/" + rptName + ".repx");
            }
            else if (RptModuleName == "Invoice_POS")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/SalesInvoice/DocDesign/SPOS/" + rptName + ".repx");
            }
            // Rev Sayantani
            else if (RptModuleName == "INFLUENCER")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/Influencer/DocDesign/Designes/" + rptName + ".repx");
            }
            // ENd of Rev Sayantani
            else if (RptModuleName == "GPAYROLL")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/PayRoll/DocDesign/Normal/" + rptName + ".repx");
            }
            else if (RptModuleName == "PSTICKER")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/PRODUCTSTICKER/DocDesign/Normal/" + rptName + ".repx");
            }
            else if (RptModuleName == "PINDENT")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/PurchaseIndent/DocDesign/Designes/" + rptName + ".repx");
            }
            else if (RptModuleName == "OrderChallan_POS")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/OrderChallanPos/DocDesign/Designes/" + rptName + ".repx");
            }
            else if (RptModuleName == "Order_POS")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/POSSALEORDER/DocDesign/SPOSORD/" + rptName + ".repx");
            }
            else if (RptModuleName == "Second_Hand")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/SecondHandSales/DocDesign/Normal/" + rptName + ".repx");
            }
            else if (RptModuleName == "POS_Duplicate")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/SalesInvoice/DocDesign/POSDUPLICATE/" + rptName + ".repx");
            }
            else if (RptModuleName == "CUSTRECPAY")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/CustomerRecPay/DocDesign/Designes/" + rptName + ".repx");
            }
            else if (RptModuleName == "VENDRECPAY")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/VendorRecPay/DocDesign/Designes/" + rptName + ".repx");
            }
            else if (RptModuleName == "CBVUCHR")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/CashBankVoucher/DocDesign/Designes/" + rptName + ".repx");
            }
            else if (RptModuleName == "PInvoice")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/PurchaseInvoice/DocDesign/Normal/" + rptName + ".repx");
            }
            else if (RptModuleName == "Self_PInvoice")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/SelfPurchaseInvoice/DocDesign/Normal/" + rptName + ".repx");
            }
            else if (RptModuleName == "PINV_CUST")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/PurchaseInvForCustomer/DocDesign/Normal/" + rptName + ".repx");
            }
            else if (RptModuleName == "PINV_TRANSPORTR")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/PurchaseInvoiceForTransporter/DocDesign/Normal/" + rptName + ".repx");
            }
            else if (RptModuleName == "TPInvoice")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/TransitPurchaseInvoice/DocDesign/Normal/" + rptName + ".repx");
            }
          
            else if (RptModuleName == "Sales_Return")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/SalesReturn/DocDesign/Normal/" + rptName + ".repx");
            }
            else if (RptModuleName == "RateDiff_Entry_Cust")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/RateDiffCustomer/DocDesign/Normal/" + rptName + ".repx");
            }
            else if (RptModuleName == "PURCHASE_RET_REQ")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/PurchaseRetRec/DocDesign/Normal/" + rptName + ".repx");
            }
            else if (RptModuleName == "OLDUNTRECVD")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/OldUnitReceived/DocDesign/Normal/" + rptName + ".repx");
            }
            else if (RptModuleName == "Purchase_Return")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/PurchaseReturn/DocDesign/Normal/" + rptName + ".repx");
            }
            else if (RptModuleName == "RateDiff_Entry_Vend")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/RateDiffVendor/DocDesign/Normal/" + rptName + ".repx");
            }
            else if (RptModuleName == "Install_Coupon")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/SalesInvoice/DocDesign/InstCoupon/" + rptName + ".repx");
            }
            else if (RptModuleName == "Proforma")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/Proforma/DocDesign/Designes/" + rptName + ".repx");
            }
            else if (RptModuleName == "LedgerPost")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/Ledger/" + rptName + ".repx");
            }
            else if (RptModuleName == "BankBook")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/CashBankBook/BankBook/" + rptName + ".repx");
            }
            else if (RptModuleName == "CashBook")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/CashBankBook/CashBooK/" + rptName + ".repx");
            }
            else if (RptModuleName == "Porder")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/PurchaseOrder/DocDesign/Designes/" + rptName + ".repx");
            }
            else if (RptModuleName == "Sorder")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/SalesOrder/DocDesign/Designes/" + rptName + ".repx");
            }
            else if (RptModuleName == "BranchReq")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/BranchRequisition/DocDesign/Designes/" + rptName + ".repx");
            }
            else if (RptModuleName == "BranchTranOut")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/BranchTransferOut/DocDesign/Designes/" + rptName + ".repx");
            }
            else if (RptModuleName == "BranchTranIn")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/BranchTransferIn/DocDesign/Designes/" + rptName + ".repx");
            }
            else if (RptModuleName == "SChallan" || RptModuleName == "PDChallan")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/SalesChallan/DocDesign/Normal/" + rptName + ".repx");
            }
            else if (RptModuleName == "PChallan")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/PurchaseChallan/DocDesign/Normal/" + rptName + ".repx");
            }
            else if (RptModuleName == "ODSDChallan")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/DeliveryChallan/DocDesign/Designes/" + rptName + ".repx");
            }
            else if (RptModuleName == "RChallan")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/SalesChallan/DocDesign/CDelivery/" + rptName + ".repx");
            }
            else if (RptModuleName == "CONTRAVOUCHER")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/ContraVoucher/DocDesign/Designes/" + rptName + ".repx");
            }
            else if (RptModuleName == "CUSTDRCRNOTE")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/CustDrCrNote/DocDesign/Designes/" + rptName + ".repx");
            }
            else if (RptModuleName == "BRCODE")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/BarCode/DocDesign/Designes/" + rptName + ".repx");
            }
            else if (RptModuleName == "VENDDRCRNOTE")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/VendDrCrNote/DocDesign/Designes/" + rptName + ".repx");
            }
            else if (RptModuleName == "PIQuotation")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/Proforma/DocDesign/Designes/" + rptName + ".repx");
            }
            //mantise issue:0025139
            else if (RptModuleName == "SalesInquiry")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/SalesInquiry/DocDesign/Designes/" + rptName + ".repx");
            }
            //End of mantise issue:0025139
            else if (RptModuleName == "JOURNALVOUCHER")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/JournalVoucher/DocDesign/Designes/" + rptName + ".repx");
            }
            else if (RptModuleName == "WarehouseStockTrans")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/WarehoseStockTranfer/DocDesign/" + rptName + ".repx");
            }
            else if (RptModuleName == "ManufacturingBOM")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/ManufacturingBOM/DocDesign/" + rptName + ".repx");
            }
            else if (RptModuleName == "JobWorkOrder")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/JobWorkOrder/DocDesign/" + rptName + ".repx");
            }
            else if (RptModuleName == "FGReceived")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/FGReceived/DocDesign/" + rptName + ".repx");
            }
            else if (RptModuleName == "MMaterialsIssue")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/MMaterialsIssue/DocDesign/" + rptName + ".repx");
            }
            else if (RptModuleName == "ManufacturingPR")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/ManufacturingPR/DocDesign/" + rptName + ".repx");
            }
            else if (RptModuleName == "WarehouseStockJrnl")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/WarehoseStockJournal/DocDesign/" + rptName + ".repx");
            }
            else if (RptModuleName == "AdvCustCr")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/AdjCustCrNote/DocDesign/Designes/" + rptName + ".repx");
            }
            else if (RptModuleName == "ImpPorder")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/ImportPurchaseOrder/DocDesign/Designes/" + rptName + ".repx");
            }
            //Rev Debashis
            else if (RptModuleName == "TRIALONNETBALSUMARY")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/TrialBalance/TrialOnNetBalance/Summary/" + rptName + ".repx");
            }
            else if (RptModuleName == "BRSSTATEMENT")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/BRSStatement/Designes/" + rptName + ".repx");
            }
            //Rev Debashis
            else if (RptModuleName == "BRSCONSSTATEMENT")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/BRSConsolidatedStatement/Designes/" + rptName + ".repx");
            }
            //End of Rev Debashis
            else if (RptModuleName == "RECPTCHALLAN")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/ReceiptChallan/DocDesign/Designes/" + rptName + ".repx");
            }
            else if (RptModuleName == "ASSIGNJOB")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/AssignJob/DocDesign/Designes/" + rptName + ".repx");
            }
            else if (RptModuleName == "DELIVERYCHALLAN")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/SRVDeliveryChallan/DocDesign/Designes/" + rptName + ".repx");
            }
            else if (RptModuleName == "SRVJOBSHEET")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/SRVJobSheet/DocDesign/Designes/" + rptName + ".repx");
            }
            //End of  Rev Debashis

            else if (RptModuleName == "WarehouseStockIN")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/WarehouseStockIN/DocDesign/" + rptName + ".repx");
            }
            //Rev Tanmoy
            else if (RptModuleName == "WarehouseStockOUT")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/WarehouseStockOUT/DocDesign/" + rptName + ".repx");
            }
            else if (RptModuleName == "EstimateCosting")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/EstimateCosting/DocDesign/" + rptName + ".repx");
            }
            //End of Rev Tanmoy
            //Rev for STB Management Tanmoy
            else if (RptModuleName == "MoneyReceipt")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/MoneyReceipt/DocDesign/Designes/" + rptName + ".repx");
            }
          
            else if (RptModuleName == "WalletRecharge")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/WalletRecharge/DocDesign/Designes/" + rptName + ".repx");
            }
            else if (RptModuleName == "PAYSLIP")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/PaySlip/DocDesign/Designes/" + rptName + ".repx");
            }
            //Rev Debashis
            else if (RptModuleName == "STATEMENTOFACCOUNT")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/SOA/Designes/" + rptName + ".repx");
            }
            //End of Rev Debashis
            //End of Rev for STB Management Tanmoy
            //else if (RptModuleName == "PDChallan")
            //{
            //    filePath = Server.MapPath("/Reports/RepxReportDesign/SalesChallan/DocDesign/Pending/" + rptName + ".repx");
            //}
            DevExpress.DataAccess.Sql.SqlDataSource sql = GenerateSqlDataSource();
            XtraReport newXtraReport = XtraReport.FromFile(filePath, true);
            newXtraReport.LoadLayout(filePath);
            newXtraReport.DataSource = sql;
            ASPxReportDesigner1.OpenReport(newXtraReport);
            //if (RptModuleName == "BRCODE")
            //{
            //    newXtraReport.Landscape = true;
            //}
        }

        private DevExpress.DataAccess.Sql.SqlDataSource GenerateSqlDataSource()
        {
            //DevExpress.DataAccess.Sql.SqlDataSource result = new DevExpress.DataAccess.Sql.SqlDataSource("crmConnectionString");

            CustomStringConnectionParameters connectionParameters = new CustomStringConnectionParameters(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            DevExpress.DataAccess.Sql.SqlDataSource result = new DevExpress.DataAccess.Sql.SqlDataSource(connectionParameters);

            //BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine();

            string RptModuleName = Convert.ToString(Session["NewRptModuleName"]);
            string Module_Name = Convert.ToString(Session["Module_Name"]);
            DataTable dtRptTables = new DataTable();
            string query = "";

            query = @"Select Query_Table_name from tbl_trans_ReportSql where Module_name = '" + Module_Name + "' order by Query_ID ";
            dtRptTables = oDbEngine.GetDataTable(query);
            string CustVendType = "";
            string SalesPurchaseType = "";
            //dtRptTables.TableName = "aaa";
            #region  for logo image
            string[] filePaths = new string[] { };
            string path = System.Web.HttpContext.Current.Server.MapPath("~");
            string path1 = path.Replace("Reports\\", "ERP.UI");
            string fullpath = path1.Replace("\\", "/");
            #endregion

            if (RptModuleName == "CUSTRECPAY" || RptModuleName == "CUSTDRCRNOTE")
            {
                CustVendType = "C";
            }
            if (RptModuleName == "VENDRECPAY" || RptModuleName == "VENDDRCRNOTE")
            {
                CustVendType = "V";
            }
            if (RptModuleName == "AdvCustCr")
            {
                CustVendType = "C";
            }
            if (RptModuleName == "AdvCustDr")
            {
                CustVendType = "D";
            }
            if (RptModuleName == "SChallan" || RptModuleName == "PDChallan" || RptModuleName == "Invoice" || RptModuleName == "Invoice_POS" || RptModuleName == "OrderChallan_POS" || RptModuleName == "Order_POS" || RptModuleName == "Second_Hand"
                || RptModuleName == "POS_Duplicate" || RptModuleName == "TSInvoice" || RptModuleName == "Sales_Return" || RptModuleName == "RateDiff_Entry_Cust" || RptModuleName == "Sorder")
            {
                SalesPurchaseType = "S";
            }
            if (RptModuleName == "PChallan" || RptModuleName == "PInvoice" || RptModuleName == "Self_PInvoice" || RptModuleName == "PINV_CUST" || RptModuleName == "PINV_TRANSPORTR" || RptModuleName == "TPInvoice" || RptModuleName == "Purchase_Return" || RptModuleName == "RateDiff_Entry_Vend" || RptModuleName == "PURCHASE_RET_REQ" || RptModuleName == "Porder")
            {
                SalesPurchaseType = "P";
            }

            if (RptModuleName == "StockTrialSumm")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_MATERIALINOUTREG '" + Convert.ToString(Session["LastCompany"]) + "','" + StartDate.Value + "','" + EndDate.Value + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + Convert.ToString(row[0]) + "','" + "" + "','" + "" + "','" + "L" + "'"));
                }
            }
            else if (RptModuleName == "StockTrialDet")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_MATERIALINOUTREGDET_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + StartDate.Value + "','" + EndDate.Value + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + Convert.ToString(row[0]) + "','" + "" + "','" + "" + "','" + "L" + "'"));
                }
            }
            //Rev Debashis
            //else if (RptModuleName == "StockTrialWH")
            else if (RptModuleName == "StockTrialProd" || RptModuleName == "StockTrialWH")
            //End of Rev Debashis
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    //Rev Debashis
                    //result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_MATERIALINOUTREGWH_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + StartDate.Value + "','" + EndDate.Value + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + Convert.ToString(row[0]) + "','" + "" + "','" + "" + "','" + "L" + "'"));
                    //result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_MATERIALINOUTREGWH_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + StartDate.Value + "','" + EndDate.Value + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + Convert.ToString(row[0]) + "','" + "" + "','" + "" + "','" + "" + "','" + "L" + "'"));
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_MATERIALINOUTREGWH_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + StartDate.Value + "','" + EndDate.Value + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + Convert.ToString(row[0]) + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "L" + "'"));
                    //End of Rev Debashis
                }
            }
            else if (RptModuleName == "StockTrial1")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_MATERIALINOUTREG1 '" + Convert.ToString(Session["LastCompany"]) + "','" + StartDate.Value + "','" + EndDate.Value + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + Convert.ToString(row[0]) + "','" + "" + "','" + "" + "','" + "L" + "'"));
                }
            }
            else if (RptModuleName == "Install_Coupon")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_INSTALLATIONCOUPON_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + fullpath + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + Convert.ToString(row[0]) + "','" + "" + "','" + "L" + "'"));
                }
            }
            else if (RptModuleName == "Invoice" || RptModuleName == "Invoice_POS" || RptModuleName == "POS_Duplicate" || RptModuleName == "Second_Hand" || RptModuleName == "PInvoice" || RptModuleName == "Self_PInvoice" || RptModuleName == "PINV_CUST" )
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_TAXINVOICE '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + "" + "','" + SalesPurchaseType + "','" + "35" + "','" + "L" + "'"));
                }
            }
                // Rev Sayantani
            else if (RptModuleName == "INFLUENCER")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_INFLUENCER_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + "" + "','" + "35" + "','" + "L" + "'"));
                }
            }
                // End of Rev Sayantani
            else if (RptModuleName == "GPAYROLL")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PRC_GENERATEPAYSLIP '" + Convert.ToString(Session["LastCompany"]) + "','" + "STRUCT0000000001" + "','" + "EMA0000243" + "','" + "1801" + "','" + "" + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + "L" + "'"));
                }
            }
            else if (RptModuleName == "PSTICKER")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PRC_PRINTPRODUCTS_REPORT '" + "1056" + "','" + Convert.ToString(row[0]) + "','" + "L" + "'"));
                }
            }
            else if (RptModuleName == "PINDENT")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_PURCHASEINDENT_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + "35" + "','" + "" + "','" + "L" + "'"));
                }
            }
            else if ( RptModuleName == "PINV_TRANSPORTR")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PRC_TRANSPORTERINVOICE_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + "" + "','" + SalesPurchaseType + "','" + "35" + "','" + "L" + "'"));
                }
            }
            else if (RptModuleName == "OrderChallan_POS")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_DELIVERYCUMORDERCHALLAN_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + "" + "','" + "S" + "','" + "2" + "','" + "L" + "'"));
                }
            }
            else if (RptModuleName == "Order_POS")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_POSORDER_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + "" + "','" + SalesPurchaseType + "','" + "35" + "','" + "L" + "'"));
                }
            }
            else if (RptModuleName == "TSInvoice" || RptModuleName == "TPInvoice")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_TRANSITSALEPURCHASE_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + "" + "','" + SalesPurchaseType + "','" + "" + "','" + "L" + "'"));
                }
            }
            else if (RptModuleName == "ODSDChallan")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_DELIVERYPENDINGCHALLAN_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + "" + "','" + "S" + "','" + "2" + "','" + "L" + "'"));
                }
            }
            else if (RptModuleName == "CUSTRECPAY" || RptModuleName == "VENDRECPAY")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_CUSTVENDRECPAY '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + "" + "','" + CustVendType + "','" + "" + "','" + "L" + "'"));
                }
            }
            else if (RptModuleName == "CBVUCHR")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_CASHBANKVOUCHER_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + "" + "','" + "" + "','" + "L" + "'"));
                }
            }
            else if (RptModuleName == "RChallan")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_DELIVERYCHALLAN '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + "" + "','" + "" + "','" + "L" + "'"));
                }
            }
            else if (RptModuleName == "Sales_Return" || RptModuleName == "Purchase_Return" || RptModuleName == "RateDiff_Entry_Cust" || RptModuleName == "RateDiff_Entry_Vend")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_SALERETURN_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + "" + "','" + SalesPurchaseType + "','" + "" + "','" + "L" + "'"));
                }
            }
            else if (RptModuleName == "PURCHASE_RET_REQ")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_PURCHASERETURN_REQUEST_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + "" + "','" + SalesPurchaseType + "','" + "" + "','" + "L" + "'"));
                }
            }
            else if (RptModuleName == "OLDUNTRECVD")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_OLDUNITRECEIVED_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + "" + "','" + "" + "','" + "L" + "'"));
                }
            }
            else if (RptModuleName == "LedgerPost" || RptModuleName == "BankBook" || RptModuleName == "CashBook")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_LEDGERPOSTING '" + Convert.ToString(Session["LastCompany"]) + "','" + StartDate.Value + "','" + EndDate.Value + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + Convert.ToString(row[0]) + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "L" + "'"));
                }
            }
            else if (RptModuleName == "Sorder" || RptModuleName == "Porder")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_SALEPURCHASEORDER_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + "" + "','" + SalesPurchaseType + "','" + "L" + "'"));
                }
            }
            else if (RptModuleName == "BranchReq")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_BRANCHREQ_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + "" + "','" + "L" + "'"));
                }
            }
            else if (RptModuleName == "BranchTranOut")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    //Rev Debashis
                    //result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_BRANCHOUT_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + "" + "','" + "L" + "'"));
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_BRANCHOUT_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + "" + "','" + "" + "','" + "L" + "'"));
                    //End of Rev Debashis
                }
            }
            else if (RptModuleName == "BranchTranIn")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_BRANCHTRANSFERIN_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + "" + "','" + "L" + "'"));
                }
            }
            else if (RptModuleName == "SChallan" || RptModuleName == "PDChallan" || RptModuleName == "PChallan")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_SALEPURCHASECHALLAN_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + "" + "','" + SalesPurchaseType + "','" + "" + "','" + "L" + "'"));
                }
            }
            else if (RptModuleName == "CONTRAVOUCHER")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_CONTRAVOUCHER_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + "" + "','" + "L" + "'"));
                }
            }
            else if (RptModuleName == "JOURNALVOUCHER")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_JOURNALVOUCHER_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + "" + "','" + "L" + "'"));
                }
            }
            else if (RptModuleName == "WarehouseStockTrans")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_STOCKTRANSFER_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + "" + "','" + "" + "','" + "L" + "'"));
                }
            }
            else if (RptModuleName == "ManufacturingBOM")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_MANUFACTURINGBOM '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + "" + "','" + "" + "','" + "L" + "'"));
                }
            }
            else if (RptModuleName == "JobWorkOrder")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_MANUFACTURINGJobWorkOrder '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + "" + "','" + "" + "','" + "L" + "'"));
                }
            }
            else if (RptModuleName == "FGReceived")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_MANUFACTURINGFGReceived '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + "" + "','" + "" + "','" + "L" + "'"));
                }
            }
            else if (RptModuleName == "MMaterialsIssue")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_MANUFACTURINGMMatialsIssue '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + "" + "','" + "" + "','" + "L" + "'"));
                }
            }
            else if (RptModuleName == "WarehouseStockJrnl")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_WAREHOUSE_WISE_STOCK_JOURNAL '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + "" + "','" + "" + "','" + "L" + "'"));
                }
            }
            else if (RptModuleName == "CUSTDRCRNOTE" || RptModuleName == "VENDDRCRNOTE")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    // Rev Sayantani
                   // result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_DEBITCREDITNOTE_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + "" + "','" + "" + "','" + CustVendType + "','" + "L" + "'"));
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_DEBITCREDITNOTE_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + "" + "','" + CustVendType + "','" + "L" + "'"));
                   // End of Rev Sayantani
                }
            }
            else if (RptModuleName == "AdvCustCr")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_ADVANCEDWITH_CRDR_NOTE '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + "" + "','" + CustVendType + "','" + "L" + "'"));
                }
            }
            else if (RptModuleName == "BRCODE")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                   // result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_BARCODE_PRINT '" + "" + "','" + "" + "','" + Convert.ToString(row[0]) + "','"+""+"','" + "P" + "'"));
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_BARCODE_PRINT '" + "" + "','" + Convert.ToString(row[0]) + "','" + "P" + "','" + "" + "'"));
                }
            }
            else if (RptModuleName == "PIQuotation")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_PROFORMAINVQUOTATION '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + "" + "','" + "L" + "'"));
                }
            }
            //mantise issue:0025139
            else if (RptModuleName == "SalesInquiry")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_SALESINQUIRY_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + "" + "','" + "L" + "'"));
                }
            }
            //End of mantise issue:0025139
            else if (RptModuleName == "ImpPorder")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PRC_IMPORTPURCHASEORDER_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + "" + "','" + "L" + "'"));
                }
            }
            //Rev Debashis
            else if (RptModuleName == "TRIALONNETBALSUMARY")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    //result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PRC_TRIALONNETBALANCE_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + StartDate.Value + "','" + EndDate.Value + "','" + "" + "','" + "Y" + "','" + Convert.ToString(0) + "','" + "" + "','" + "" + "','" + Convert.ToString(0) + "','" + "A" + "','" + Convert.ToString(0) + "','" + Convert.ToString(0) + "','" + Convert.ToString(0) + "','" + Convert.ToString(0) + "','" + Convert.ToString(0) + "','" + Convert.ToString(0) + "','" + Convert.ToString(row[0]) + "','" + Convert.ToString(0) + "','" + "L"+"','"+Convert.ToInt32(Session["userid"]) + "'"));
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PRC_TRIALONNETBALANCE_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + StartDate.Value + "','" + EndDate.Value + "','" + "" + "','" + "Y" + "','" + Convert.ToString(0) + "','" + "" + "','" + "" + "','" + Convert.ToString(0) + "','" + "A" + "','" + Convert.ToString(0) + "','" + Convert.ToString(0) + "','" + Convert.ToString(0) + "','" + Convert.ToString(0) + "','" + Convert.ToString(0) + "','" + Convert.ToString(0) + "','" + Convert.ToString(0) + "','" + Convert.ToString(row[0]) + "','" + Convert.ToString(0) + "','" + "L" + "','" + Convert.ToInt32(Session["userid"]) + "'"));
                }
            }
            else if (RptModuleName == "BRSSTATEMENT")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PRC_BRSDETAILSUMMARYFORMAT_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + StartDate.Value + "','" + EndDate.Value + "','" + "" + "','" + "" + "','" + Convert.ToString(row[0]) + "','" + "" + "','" + "L" + "'"));
                }
            }
            //Rev Debashis
            else if (RptModuleName == "BRSCONSSTATEMENT")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PRC_BRSCONSOLIDATEDFORMAT_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + StartDate.Value + "','" + EndDate.Value + "','" + "" + "','" + Convert.ToString(row[0]) + "','" + "" + "','" + "L" + "'"));
                }
            }
            //End of Rev Debashis
            else if (RptModuleName == "RECPTCHALLAN")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PRC_RECEIPTCHALLANPRINT_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + "" + "','" + "L" + "'"));
                }
            }
            else if (RptModuleName == "ASSIGNJOB")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PRC_ASSIGNJOBPRINT_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + "" + "','" + "L" + "'"));
                }
            }
            else if (RptModuleName == "DELIVERYCHALLAN")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PRC_DELIVERYCHALLANPRINT_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + "" + "','" + "L" + "'"));
                }
            }
            else if (RptModuleName == "SRVJOBSHEET")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PRC_JOBSHEETPRINT_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + "" + "','" + "L" + "'"));
                }
            }
            //Rev of Rev Debashis
            else if (RptModuleName == "WarehouseStockIN")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_WAREHOUSE_WISE_STOCK_IN '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + "" + "','" + "" + "','" + "L" + "'"));
                }
            }
            //Rev Tanmoy
            else if (RptModuleName == "WarehouseStockOUT")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_WAREHOUSE_WISE_STOCK_OUT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + "" + "','" + "" + "','" + "L" + "'"));
                }
            }
            else if (RptModuleName == "EstimateCosting")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_ESTIMATECOSTING_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + "" + "','" + "" + "','" + "L" + "'"));
                }
            }
            //End of Rev Tanmoy
            //Rev for STB Management Tanmoy
            else if (RptModuleName == "MoneyReceipt")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PRC_MoneyReceipt_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + "" + "','" + "35" + "','" + Convert.ToString(Session["UserID"]).Trim() + "','" + "L" + "'"));
                }
            }
        
            else if (RptModuleName == "WalletRecharge")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PRC_WalletRecharge_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + "" + "','" + "35" + "','" + Convert.ToString(Session["UserID"]).Trim() + "','" + "L" + "'"));
                }
            }
            else if (RptModuleName == "PAYSLIP" )
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    //Rev Debashis 0024385
                    //result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_PAYSLIP '" + Convert.ToString(row[0]) + "','" + Convert.ToString("2012").Trim() + "','" + Convert.ToString(Session["StructureID_Report"]).Trim() + "','" + Convert.ToString("").Trim() + "'"));
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_PAYSLIP '" + Convert.ToString(Session["LastCompany"]) + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + Convert.ToString("2012").Trim() + "','" + Convert.ToString("").Trim() + "'"));
                    //End of Rev Debashis 0024385
                }
            }
            //End of Rev for STB Management Tanmoy
            //Rev Debashis
            else if (RptModuleName == "STATEMENTOFACCOUNT")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PRC_SOAPARTYWISE_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + StartDate.Value + "','" + EndDate.Value + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + Convert.ToString(row[0]) + "','" + "L" + "'"));
                }
            }
            //End of Rev Debashis
            DataTable dtRptRelation = new DataTable();
            string RelationQuery = "";

            RelationQuery = @"Select Parent_Query_name,Child_Query_name, Parent_Column_name,Child_Column_name from tbl_trans_ReportTableRelation where Module_name = '" + Module_Name + "' order by Query_ID ";
            dtRptRelation = oDbEngine.GetDataTable(RelationQuery);
            foreach (DataRow row in dtRptRelation.Rows)
            {
                result.Relations.Add(Convert.ToString(row[0]), Convert.ToString(row[1]), Convert.ToString(row[2]), Convert.ToString(row[3]));
            }

            result.RebuildResultSchema();
            return result;
        }

        // Save a report to a file.
        protected void ASPxReportDesigner1_SaveReportLayout(object sender, DevExpress.XtraReports.Web.SaveReportLayoutEventArgs e)
        {
            string FileName = "";
            string filePath = "";
            //String ReportModule = Convert.ToString(Session["NewRptModuleName"]);
            string ReportModule = HttpContext.Current.Request.QueryString["reportname"];
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["NewReport"]))
            {
                FileName = HttpContext.Current.Request.QueryString["NewReport"] + "~N";
            }
            else if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["LoadrptName"]))
            {
                FileName = HttpContext.Current.Request.QueryString["LoadrptName"];
            }
            XtraReport newXtraReport = new XtraReport();
            if (ReportModule == "StockTrialSumm")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/Inventory/StockTrial/Summary/" + FileName + ".repx");
            }
            else if (ReportModule == "StockTrialDet")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/Inventory/StockTrial/Details/" + FileName + ".repx");
            }
                //Rev Debashis
            else if (ReportModule == "StockTrialProd")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/Inventory/StockTrial/Product/" + FileName + ".repx");
            }
                //End of Rev Debashis
            else if (ReportModule == "StockTrialWH")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/Inventory/StockTrial/Warehouse/" + FileName + ".repx");
            }
            else if (ReportModule == "StockTrial1")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/Inventory/StockTrial1/" + FileName + ".repx");
            }
            else if (ReportModule == "Invoice")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/SalesInvoice/DocDesign/Normal/" + FileName + ".repx");
            }
            else if (ReportModule == "TSInvoice")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/SalesInvoice/DocDesign/Transit/" + FileName + ".repx");
            }
            else if (ReportModule == "TPInvoice")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/TransitPurchaseInvoice/DocDesign/Normal/" + FileName + ".repx");
            }
            else if (ReportModule == "Invoice_POS")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/SalesInvoice/DocDesign/SPOS/" + FileName + ".repx");
            }
            // Rev Sayantani
            else if (ReportModule == "INFLUENCER")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/Influencer/DocDesign/Designes/" + FileName + ".repx");
            }
            // End of Rev Sayantani
            else if (ReportModule == "GPAYROLL")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/PayRoll/DocDesign/Normal/" + FileName + ".repx");
            }
            else if (ReportModule == "PSTICKER")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/PRODUCTSTICKER/DocDesign/Normal/" + FileName + ".repx");
            }
            else if (ReportModule == "PINDENT")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/PurchaseIndent/DocDesign/Designes/" + FileName + ".repx");
            }
            else if (ReportModule == "OrderChallan_POS")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/OrderChallanPos/DocDesign/Designes/" + FileName + ".repx");
            }
            else if (ReportModule == "Order_POS")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/POSSALEORDER/DocDesign/SPOSORD/" + FileName + ".repx");
            }
            else if (ReportModule == "Second_Hand")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/SecondHandSales/DocDesign/Normal/" + FileName + ".repx");
            }
            else if (ReportModule == "POS_Duplicate")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/SalesInvoice/DocDesign/POSDUPLICATE/" + FileName + ".repx");
            }
            else if (ReportModule == "CUSTRECPAY")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/CustomerRecPay/DocDesign/Designes/" + FileName + ".repx");
            }
            else if (ReportModule == "VENDRECPAY")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/VendorRecPay/DocDesign/Designes/" + FileName + ".repx");
            }
            else if (ReportModule == "CBVUCHR")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/CashBankVoucher/DocDesign/Designes/" + FileName + ".repx");
            }
            else if (ReportModule == "PInvoice")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/PurchaseInvoice/DocDesign/Normal/" + FileName + ".repx");
            }
            else if (ReportModule == "Self_PInvoice")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/SelfPurchaseInvoice/DocDesign/Normal/" + FileName + ".repx");
            }
            else if (ReportModule == "PINV_CUST")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/PurchaseInvForCustomer/DocDesign/Normal/" + FileName + ".repx");
            }
            else if (ReportModule == "PINV_TRANSPORTR")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/PurchaseInvoiceForTransporter/DocDesign/Normal/" + FileName + ".repx");
            }
            else if (ReportModule == "TPInvoice")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/TransitPurchaseInvoice/DocDesign/Normal/" + FileName + ".repx");
            }
          
            else if (ReportModule == "Install_Coupon")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/SalesInvoice/DocDesign/InstCoupon/" + FileName + ".repx");
            }
            else if (ReportModule == "Proforma")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/Proforma/DocDesign/Designes/" + FileName + ".repx");
            }
            else if (ReportModule == "LedgerPost")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/Ledger/" + FileName + ".repx");
            }
            else if (ReportModule == "BankBook")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/CashBankBook/BankBook/" + FileName + ".repx");
            }
            else if (ReportModule == "CashBook")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/CashBankBook/CashBook/" + FileName + ".repx");
            }
            else if (ReportModule == "Porder")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/PurchaseOrder/DocDesign/Designes/" + FileName + ".repx");
            }
            else if (ReportModule == "Sorder")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/SalesOrder/DocDesign/Designes/" + FileName + ".repx");
            }
            else if (ReportModule == "BranchReq")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/BranchRequisition/DocDesign/Designes/" + FileName + ".repx");
            }
            else if (ReportModule == "BranchTranOut")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/BranchTransferOut/DocDesign/Designes/" + FileName + ".repx");
            }
            else if (ReportModule == "BranchTranIn")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/BranchTransferIn/DocDesign/Designes/" + FileName + ".repx");
            }
            else if (ReportModule == "Sales_Return")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/SalesReturn/DocDesign/Normal/" + FileName + ".repx");
            }
            else if (ReportModule == "RateDiff_Entry_Cust")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/RateDiffCustomer/DocDesign/Normal/" + FileName + ".repx");
            }
            else if (ReportModule == "PURCHASE_RET_REQ")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/PurchaseRetRec/DocDesign/Normal/" + FileName + ".repx");
            }
            else if (ReportModule == "OLDUNTRECVD")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/OldUnitReceived/DocDesign/Normal/" + FileName + ".repx");
            }
            else if (ReportModule == "Purchase_Return")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/PurchaseReturn/DocDesign/Normal/" + FileName + ".repx");
            }
            else if (ReportModule == "RateDiff_Entry_Vend")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/RateDiffVendor/DocDesign/Normal/" + FileName + ".repx");
            }
            else if (ReportModule == "SChallan" || ReportModule == "PDChallan")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/SalesChallan/DocDesign/Normal/" + FileName + ".repx");
            }
            else if (ReportModule == "PChallan")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/PurchaseChallan/DocDesign/Normal/" + FileName + ".repx");
            }
            else if (ReportModule == "ODSDChallan")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/DeliveryChallan/DocDesign/Designes/" + FileName + ".repx");
            }
            else if (ReportModule == "RChallan")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/SalesChallan/DocDesign/CDelivery/" + FileName + ".repx");
            }
            else if (ReportModule == "CONTRAVOUCHER")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/ContraVoucher/DocDesign/Designes/" + FileName + ".repx");
            }
            else if (ReportModule == "CUSTDRCRNOTE")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/CustDrCrNote/DocDesign/Designes/" + FileName + ".repx");
            }
            else if (ReportModule == "BRCODE")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/BarCode/DocDesign/Designes/" + FileName + ".repx");
            }
            else if (ReportModule == "VENDDRCRNOTE")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/VendDrCrNote/DocDesign/Designes/" + FileName + ".repx");
            }
            else if (ReportModule == "PIQuotation")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/Proforma/DocDesign/Designes/" + FileName + ".repx");
            }
            //mantise issue:0025139
            else if (ReportModule == "SalesInquiry")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/SalesInquiry/DocDesign/Designes/" + FileName + ".repx");
            }
            //End of mantise issue:0025139
            else if (ReportModule == "JOURNALVOUCHER")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/JournalVoucher/DocDesign/Designes/" + FileName + ".repx");
            }
            else if (ReportModule == "WarehouseStockTrans")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/WarehoseStockTranfer/DocDesign/" + FileName + ".repx");
            }
            else if (ReportModule == "ManufacturingBOM")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/ManufacturingBOM/DocDesign/" + FileName + ".repx");
            }
            else if (ReportModule == "JobWorkOrder")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/JobWorkOrder/DocDesign/" + FileName + ".repx");
            }
            else if (ReportModule == "FGReceived")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/FGReceived/DocDesign/" + FileName + ".repx");
            }
            else if (ReportModule == "MMaterialsIssue")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/MMaterialsIssue/DocDesign/" + FileName + ".repx");
            }
            else if (ReportModule == "ManufacturingPR")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/ManufacturingPR/DocDesign/" + FileName + ".repx");
            }
            else if (ReportModule == "WarehouseStockJrnl")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/WarehoseStockJournal/DocDesign/" + FileName + ".repx");
            }
            else if (ReportModule == "AdvCustCr")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/AdjCustCrNote/DocDesign/Designes/" + FileName + ".repx");
            }
            else if (ReportModule == "ImpPorder")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/ImportPurchaseOrder/DocDesign/Designes/" + FileName + ".repx");
            }
            //Rev Debashis
            else if (ReportModule == "TRIALONNETBALSUMARY")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/TrialBalance/TrialOnNetBalance/Summary/" + FileName + ".repx");
            }
            else if (ReportModule == "BRSSTATEMENT")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/BRSStatement/Designes/" + FileName + ".repx");
            }
            //Rev Debashis
            else if (ReportModule == "BRSCONSSTATEMENT")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/BRSConsolidatedStatement/Designes/" + FileName + ".repx");
            }
            //End of Rev Debashis
            else if (ReportModule == "RECPTCHALLAN")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/ReceiptChallan/DocDesign/Designes/" + FileName + ".repx");
            }
            else if (ReportModule == "ASSIGNJOB")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/AssignJob/DocDesign/Designes/" + FileName + ".repx");
            }
            else if (ReportModule == "DELIVERYCHALLAN")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/SRVDeliveryChallan/DocDesign/Designes/" + FileName + ".repx");
            }
            else if (ReportModule == "SRVJOBSHEET")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/SRVJobSheet/DocDesign/Designes/" + FileName + ".repx");
            }
            //End of Rev Debashis
            else if (ReportModule == "WarehouseStockIN")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/WarehouseStockIN/DocDesign/" + FileName + ".repx");
            }
            //Rev Tanmoy
            else if (ReportModule == "WarehouseStockOUT")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/WarehouseStockOUT/DocDesign/" + FileName + ".repx");
            }
            else if (ReportModule == "EstimateCosting")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/EstimateCosting/DocDesign/" + FileName + ".repx");
            }
            //End of Rev Tanmoy
            //Rev for STB Management Tanmoy
            else if (ReportModule == "MoneyReceipt")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/MoneyReceipt/DocDesign/Designes/" + FileName + ".repx");
            }
            else if (ReportModule == "WalletRecharge")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/WalletRecharge/DocDesign/Designes/" + FileName + ".repx");
            }
            else if (ReportModule == "PAYSLIP")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/PaySlip/DocDesign/Designes/" + FileName + ".repx");
            }
            //End of Rev for STB Management Tanmoy
            //Rev Debashis
            else if (ReportModule == "STATEMENTOFACCOUNT")
            {
                filePath = Server.MapPath("/Reports/RepxReportDesign/SOA/Designes/" + FileName + ".repx");
            }
            //End of Rev Debashis
            var bytarr = e.ReportLayout;
            Stream stream = new MemoryStream(bytarr);
            newXtraReport.LoadLayout(stream);
            newXtraReport.SaveLayout(filePath);
            ASPxReportDesigner1.JSProperties["cpSaveResult"] = "Design saved successfully.";
        }
    }
}