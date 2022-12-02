using BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.DataAccess.Sql;
using DevExpress.XtraReports.UI;
using DataAccessLayer;
using DevExpress.Web;
using System.Net.Mail;
using System.Drawing;
using DevExpress.XtraPrinting.Drawing;
using System.Web.Services;
using DevExpress.DataAccess.ConnectionParameters;

namespace Reports.Reports.REPXReports
{
    public partial class RepxReportViewer : System.Web.UI.Page
    {
        BusinessLogicLayer.ReportLayout rpLayout = new BusinessLogicLayer.ReportLayout();
        BusinessLogicLayer.ReportData rptData = new BusinessLogicLayer.ReportData();
        DBEngine odbeng = new DBEngine();
        public string redirectReportKey = "";
        string BranId = "";
        string Asondt = "";
        string Zerobal = "";
        string Ho = "";
        string GrpLst = "";
        string ClStk = "";
        string Valtech = "";
        string Owmvt = "";
        string PG = "";
        string Hrchy = "";
        string SwGrp = "";
        string NDrcrOpCl = "";
        string OpAsonDt = "";
        string ConsLandCost = "";
        string ConsOverheadCost = "";
        //Rev Debashis
        string PartyIds = "";
        string Criteria = "";
        string ShowAllParty = "";
        string ProjList = "";
        string ShowHeader = "";
        string ShowFooter = "";
        string DocidforWaterMark = "";
        //End of Rev Debashis
        DataTable dtWatermark = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            if (IsPostBack)
            {
                string tempFile = HttpContext.Current.Request.QueryString["Previewrpt"];
                StartDate.Value = HttpContext.Current.Request.QueryString["StartDate"];
                EndDate.Value = HttpContext.Current.Request.QueryString["EndDate"];
                string PrintType = HttpContext.Current.Request.QueryString["PrintOption"];
                String RptModuleName = HttpContext.Current.Request.QueryString["reportname"];
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                string rptName = tempFile;
                string filePath = "";
                string ExportFileName = "";
                string filePathtoPDF = "";
                string ReportType = "";
                //Rev Debashis
                Session["ReportGenerationType"] = null;
                if (RptModuleName == "TRIALONNETBALSUMARY")
                {
                    BranId = HttpContext.Current.Request.QueryString["BranId"];
                    Asondt = HttpContext.Current.Request.QueryString["Ason"];
                    Zerobal = HttpContext.Current.Request.QueryString["Zerobal"];
                    Ho = HttpContext.Current.Request.QueryString["Ho"];
                    GrpLst = HttpContext.Current.Request.QueryString["GrpLst"];
                    ClStk = HttpContext.Current.Request.QueryString["ClStk"];
                    Valtech = HttpContext.Current.Request.QueryString["Valtech"];
                    Owmvt = HttpContext.Current.Request.QueryString["Owmvt"];
                    PG = HttpContext.Current.Request.QueryString["PG"];
                    Hrchy = HttpContext.Current.Request.QueryString["Hrchy"];
                    SwGrp = HttpContext.Current.Request.QueryString["SwGrp"];
                    NDrcrOpCl = HttpContext.Current.Request.QueryString["NDrcrOpCl"];
                    OpAsonDt = HttpContext.Current.Request.QueryString["OpAsonDt"];
                    ConsLandCost = HttpContext.Current.Request.QueryString["ConsLandCost"];
                    ConsOverheadCost = HttpContext.Current.Request.QueryString["ConsOverheadCost"];
                }
                if (RptModuleName == "BRSSTATEMENT")
                {
                    if (rptName == "Details~D")
                    {
                        Session["ReportGenerationType"] = "Details";
                    }
                    else
                    {
                        Session["ReportGenerationType"] = "Summary";
                    }
                }
                if (RptModuleName == "BRSCONSSTATEMENT")
                {
                    if (rptName == "Details~D")
                    {
                        Session["ReportGenerationType"] = "Details";
                    }
                    else
                    {
                        Session["ReportGenerationType"] = "Summary";
                    }
                }
                if (RptModuleName == "STATEMENTOFACCOUNT")
                {
                    BranId = HttpContext.Current.Request.QueryString["BranId"];
                    PartyIds = HttpContext.Current.Request.QueryString["PartyIds"];
                    Criteria = HttpContext.Current.Request.QueryString["Criteria"];
                    ShowAllParty = HttpContext.Current.Request.QueryString["ShowAllParty"];
                    ProjList = HttpContext.Current.Request.QueryString["ProjList"];
                    ShowHeader = HttpContext.Current.Request.QueryString["ShowHeader"];
                    ShowFooter = HttpContext.Current.Request.QueryString["ShowFooter"];
                }
                //End of Rev Debashis
                //String RptModuleName = Convert.ToString(Session["NewRptModuleName"]);
                if (RptModuleName == "StockTrialSumm")
                {
                    if (string.IsNullOrEmpty(Page.Title))
                    {
                        Page.Title = "Stock Trial Summary" + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/Inventory/StockTrial/Summary/" + rptName + ".repx");
                }
                else if (RptModuleName == "StockTrialDet")
                {
                    if (string.IsNullOrEmpty(Page.Title))
                    {
                        Page.Title = "Stock Trial Details" + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/Inventory/StockTrial/Details/" + rptName + ".repx");
                }
                //Rev Debashis
                else if (RptModuleName == "StockTrialProd")
                {
                    if (string.IsNullOrEmpty(Page.Title))
                    {
                        Page.Title = "Stock Trial Warehouse" + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/Inventory/StockTrial/Product/" + rptName + ".repx");
                }
                //End of Rev Debashis
                else if (RptModuleName == "StockTrialWH")
                {
                    if (string.IsNullOrEmpty(Page.Title))
                    {
                        Page.Title = "Stock Trial Warehouse" + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/Inventory/StockTrial/Warehouse/" + rptName + ".repx");
                }
                else if (RptModuleName == "Invoice")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Sale Invoice-" + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/SalesInvoice/DocDesign/Normal/" + rptName + ".repx");
                }
                else if (RptModuleName == "TSInvoice")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Transit Sale Invoice-" + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/SalesInvoice/DocDesign/Transit/" + rptName + ".repx");
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
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Purchase Invoice For Transporter-" + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/PurchaseInvoiceForTransporter/DocDesign/Normal/" + rptName + ".repx");
                }
                else if (RptModuleName == "TPInvoice")
                {
                    filePath = Server.MapPath("/Reports/RepxReportDesign/TransitPurchaseInvoice/DocDesign/Normal/" + rptName + ".repx");
                }

                else if (RptModuleName == "Invoice_POS")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Sale Invoice(POS)-" + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/SalesInvoice/DocDesign/SPOS/" + rptName + ".repx");
                }
                // Rev Sayantani
                else if (RptModuleName == "INFLUENCER")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Influencer Payment-" + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/Influencer/DocDesign/Designes/" + rptName + ".repx");
                }
                // End of Rev Sayantani
                else if (RptModuleName == "GPAYROLL")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Generate PayRoll-" + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/PayRoll/DocDesign/Normal/" + rptName + ".repx");
                }
                else if (RptModuleName == "PSTICKER")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Product Sticker-" + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/PRODUCTSTICKER/DocDesign/Normal/" + rptName + ".repx");
                }
                else if (RptModuleName == "PINDENT")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Purchase Indent-" + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/PurchaseIndent/DocDesign/Designes/" + rptName + ".repx");
                }
                else if (RptModuleName == "OrderChallan_POS")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Order Challan(POS)-" + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/OrderChallanPos/DocDesign/Designes/" + rptName + ".repx");
                }
                else if (RptModuleName == "Order_POS")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Sale Order(POS)-" + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/POSSALEORDER/DocDesign/SPOSORD/" + rptName + ".repx");
                }
                else if (RptModuleName == "Second_Hand")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Second hand sales-" + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/SecondHandSales/DocDesign/Normal/" + rptName + ".repx");
                }
                else if (RptModuleName == "POS_Duplicate")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "POS Bill Print-" + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/SalesInvoice/DocDesign/POSDUPLICATE/" + rptName + ".repx");
                }
                else if (RptModuleName == "CUSTRECPAY")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Customer Rec/Pay-" + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/CustomerRecPay/DocDesign/Designes/" + rptName + ".repx");
                }
                else if (RptModuleName == "VENDRECPAY")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Vendor Rec/Pay-" + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/VendorRecPay/DocDesign/Designes/" + rptName + ".repx");
                }
                else if (RptModuleName == "AdvCustCr")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Credit Note With Invoice" + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/AdjCustCrNote/DocDesign/Designes/" + rptName + ".repx");
                }
                else if (RptModuleName == "CBVUCHR")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Cash Bank Voucher-" + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/CashBankVoucher/DocDesign/Designes/" + rptName + ".repx");
                }
                else if (RptModuleName == "ODSDChallan")
                {
                    if (string.IsNullOrEmpty(Page.Title))
                    {
                        Page.Title = "Delivery Challan(ODSD) " + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/DeliveryChallan/DocDesign/Designes/" + rptName + ".repx");
                }
                else if (RptModuleName == "RChallan")
                {
                    if (string.IsNullOrEmpty(Page.Title))
                    {
                        Page.Title = "Road Challan " + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/SalesChallan/DocDesign/CDelivery/" + rptName + ".repx");
                }
                else if (RptModuleName == "Sales_Return")
                {
                    if (string.IsNullOrEmpty(Page.Title))
                    {
                        Page.Title = "Sales Return " + rptName.Split('~')[0];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/SalesReturn/DocDesign/Normal/" + rptName + ".repx");
                }
                else if (RptModuleName == "RateDiff_Entry_Cust")
                {
                    if (string.IsNullOrEmpty(Page.Title))
                    {
                        Page.Title = "Rate Difference Entry Customer " + rptName.Split('~')[0];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/RateDiffCustomer/DocDesign/Normal/" + rptName + ".repx");
                }
                else if (RptModuleName == "PURCHASE_RET_REQ")
                {
                    if (string.IsNullOrEmpty(Page.Title))
                    {
                        Page.Title = "Purchase Return Request-" + rptName.Split('~')[0];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/PurchaseRetRec/DocDesign/Normal/" + rptName + ".repx");
                }
                else if (RptModuleName == "OLDUNTRECVD")
                {
                    if (string.IsNullOrEmpty(Page.Title))
                    {
                        Page.Title = "Old Unit Received " + rptName.Split('~')[0];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/OldUnitReceived/DocDesign/Normal/" + rptName + ".repx");
                }
                else if (RptModuleName == "Purchase_Return")
                {
                    if (string.IsNullOrEmpty(Page.Title))
                    {
                        Page.Title = "Purchase Return " + rptName.Split('~')[0];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/PurchaseReturn/DocDesign/Normal/" + rptName + ".repx");
                }
                else if (RptModuleName == "RateDiff_Entry_Vend")
                {
                    if (string.IsNullOrEmpty(Page.Title))
                    {
                        Page.Title = "Rate Difference Entry Vendor " + rptName.Split('~')[0];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/RateDiffVendor/DocDesign/Normal/" + rptName + ".repx");
                }
                else if (RptModuleName == "Install_Coupon")
                {
                    filePath = Server.MapPath("/Reports/RepxReportDesign/SalesInvoice/DocDesign/InstCoupon/" + rptName + ".repx");
                }
                else if (RptModuleName == "LedgerPost")
                {
                    if (string.IsNullOrEmpty(Page.Title))
                    {
                        Page.Title = "Ledger Posting Details " + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/Ledger/" + rptName + ".repx");
                }
                else if (RptModuleName == "BankBook")
                {
                    if (string.IsNullOrEmpty(Page.Title))
                    {
                        Page.Title = "Bank Book Details " + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/CashBankBook/BankBook/" + rptName + ".repx");
                }
                else if (RptModuleName == "CashBook")
                {
                    if (string.IsNullOrEmpty(Page.Title))
                    {
                        Page.Title = "Cash Book Details " + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/CashBankBook/CashBook/" + rptName + ".repx");
                }
                else if (RptModuleName == "Porder")
                {
                    if (string.IsNullOrEmpty(Page.Title))
                    {
                        Page.Title = "Purchase Order " + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/PurchaseOrder/DocDesign/Designes/" + rptName + ".repx");
                }
                else if (RptModuleName == "Sorder")
                {
                    if (string.IsNullOrEmpty(Page.Title))
                    {
                        Page.Title = "Sales Order " + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/SalesOrder/DocDesign/Designes/" + rptName + ".repx");
                }
                else if (RptModuleName == "BranchReq")
                {
                    if (string.IsNullOrEmpty(Page.Title))
                    {
                        Page.Title = "Branch Requisition " + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/BranchRequisition/DocDesign/Designes/" + rptName + ".repx");
                }
                else if (RptModuleName == "BranchTranOut")
                {
                    dtWatermark = odbeng.GetDataTable("select Variable_Value from Config_SystemSettings where Variable_Name='ShowWatermarkinBTODocumentPrint'");
                    if (string.IsNullOrEmpty(Page.Title))
                    {
                        Page.Title = "Branch Transfer Out " + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/BranchTransferOut/DocDesign/Designes/" + rptName + ".repx");
                }
                else if (RptModuleName == "BranchTranIn")
                {
                    if (string.IsNullOrEmpty(Page.Title))
                    {
                        Page.Title = "Branch Transfer In " + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/BranchTransferIn/DocDesign/Designes/" + rptName + ".repx");
                }
                else if (RptModuleName == "SChallan" || RptModuleName == "PDChallan")
                {
                    if (string.IsNullOrEmpty(Page.Title))
                    {
                        if (RptModuleName == "SChallan")
                        {
                            Page.Title = "Sales Challan " + rptName.Split('~')[0];
                        }
                        else
                        {
                            Page.Title = "Pending Delivery List " + rptName.Split('~')[0];
                        }
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/SalesChallan/DocDesign/Normal/" + rptName + ".repx");
                }
                else if (RptModuleName == "PChallan")
                {
                    if (string.IsNullOrEmpty(Page.Title))
                    {
                        Page.Title = "Purchase Challan " + rptName.Split('~')[0];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/PurchaseChallan/DocDesign/Normal/" + rptName + ".repx");
                }
                else if (RptModuleName == "CONTRAVOUCHER")
                {
                    if (string.IsNullOrEmpty(Page.Title))
                    {
                        Page.Title = "Contra Voucher" + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/ContraVoucher/DocDesign/Designes/" + rptName + ".repx");
                }
                else if (RptModuleName == "JOURNALVOUCHER")
                {
                    if (string.IsNullOrEmpty(Page.Title))
                    {
                        Page.Title = "Journal Voucher" + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/JournalVoucher/DocDesign/Designes/" + rptName + ".repx");
                }
                else if (RptModuleName == "WarehouseStockTrans")
                {
                    if (string.IsNullOrEmpty(Page.Title))
                    {
                        Page.Title = "Warehouse Stock Transfer" + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/WarehoseStockTranfer/DocDesign/" + rptName + ".repx");
                }

                else if (RptModuleName == "ManufacturingBOM")
                {
                    if (string.IsNullOrEmpty(Page.Title))
                    {
                        Page.Title = "Bill of Materials" + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/ManufacturingBOM/DocDesign/" + rptName + ".repx");
                }

                else if (RptModuleName == "JobWorkOrder")
                {
                    if (string.IsNullOrEmpty(Page.Title))
                    {
                        Page.Title = "Job Work Order" + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/JobWorkOrder/DocDesign/" + rptName + ".repx");
                }
                else if (RptModuleName == "FGReceived")
                {
                    if (string.IsNullOrEmpty(Page.Title))
                    {
                        Page.Title = "FG Received" + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/FGReceived/DocDesign/" + rptName + ".repx");
                }
                else if (RptModuleName == "MMaterialsIssue")
                {
                    if (string.IsNullOrEmpty(Page.Title))
                    {
                        Page.Title = "Materials Issue No." + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/MMaterialsIssue/DocDesign/" + rptName + ".repx");
                }
                else if (RptModuleName == "ManufacturingPR")
                {
                    if (string.IsNullOrEmpty(Page.Title))
                    {
                        Page.Title = "Production Receipt" + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/ManufacturingPR/DocDesign/" + rptName + ".repx");
                }
                else if (RptModuleName == "WarehouseStockJrnl")
                {
                    if (string.IsNullOrEmpty(Page.Title))
                    {
                        Page.Title = "Warehouse Wise Stock Journal" + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/WarehoseStockJournal/DocDesign/" + rptName + ".repx");
                }
                else if (RptModuleName == "WarehouseStockIN")
                {
                    if (string.IsNullOrEmpty(Page.Title))
                    {
                        Page.Title = "Warehouse Wise Stock- IN" + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/WarehouseStockIN/DocDesign/" + rptName + ".repx");
                }
                //Rev Tanmoy
                else if (RptModuleName == "WarehouseStockOUT")
                {
                    if (string.IsNullOrEmpty(Page.Title))
                    {
                        Page.Title = "Warehouse Wise Stock- OUT" + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/WarehouseStockOUT/DocDesign/" + rptName + ".repx");
                }
                else if (RptModuleName == "EstimateCosting")
                {
                    if (string.IsNullOrEmpty(Page.Title))
                    {
                        Page.Title = "Estimate & Costing" + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/EstimateCosting/DocDesign/" + rptName + ".repx");
                }
                //End of Rev Tanmoy
                else if (RptModuleName == "CUSTDRCRNOTE")
                {
                    if (string.IsNullOrEmpty(Page.Title))
                    {
                        Page.Title = "Customer Debit/Credit Note" + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/CustDrCrNote/DocDesign/Designes/" + rptName + ".repx");
                }
                else if (RptModuleName == "BRCODE")
                {
                    if (string.IsNullOrEmpty(Page.Title))
                    {
                        Page.Title = "Barcode" + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/BarCode/DocDesign/Designes/" + rptName + ".repx");
                }
                else if (RptModuleName == "VENDDRCRNOTE")
                {
                    if (string.IsNullOrEmpty(Page.Title))
                    {
                        Page.Title = "Vendor Debit/Credit Note" + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/VendDrCrNote/DocDesign/Designes/" + rptName + ".repx");
                }
                else if (RptModuleName == "AdvCustCr")
                {
                    if (string.IsNullOrEmpty(Page.Title))
                    {
                        Page.Title = "Credit Note With Invoice" + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/AdjCustCrNote/DocDesign/Designes/" + rptName + ".repx");
                }
                else if (RptModuleName == "PIQuotation")
                {
                    if (string.IsNullOrEmpty(Page.Title))
                    {
                        Page.Title = "Proforma Invoice/Quotation" + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/Proforma/DocDesign/Designes/" + rptName + ".repx");
                }
                //mantise issue:0025139
                else if (RptModuleName == "SalesInquiry")
                {
                    if (string.IsNullOrEmpty(Page.Title))
                    {
                        Page.Title = "Sales Inquiry" + rptName.Split('~')[0]; 
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/SalesInquiry/DocDesign/Designes/" + rptName + ".repx");
                }
                //End of mantise issue:0025139
                else if (RptModuleName == "ImpPorder")
                {
                    filePath = Server.MapPath("/Reports/RepxReportDesign/ImportPurchaseOrder/DocDesign/Designes/" + rptName + ".repx");
                }
                //Rev Debashis
                else if (RptModuleName == "TRIALONNETBALSUMARY")
                {
                    if (string.IsNullOrEmpty(Page.Title))
                    {
                        Page.Title = "Trial On Net Balance_" + rptName.Split('~')[0];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/TrialBalance/TrialOnNetBalance/Summary/" + rptName + ".repx");
                }
                else if (RptModuleName == "BRSSTATEMENT")
                {
                    if (string.IsNullOrEmpty(Page.Title))
                    {
                        Page.Title = "BRS Statement_" + rptName.Split('~')[0];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/BRSStatement/Designes/" + rptName + ".repx");
                }
                //Rev Debashis
                else if (RptModuleName == "BRSCONSSTATEMENT")
                {
                    if (string.IsNullOrEmpty(Page.Title))
                    {
                        Page.Title = "BRS Consolidated_Statement_" + rptName.Split('~')[0];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/BRSConsolidatedStatement/Designes/" + rptName + ".repx");
                }
                //End of Rev Debashis
                else if (RptModuleName == "RECPTCHALLAN")
                {
                    if (string.IsNullOrEmpty(Page.Title))
                    {
                        Page.Title = "Receipt Challan_" + rptName.Split('~')[0];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/ReceiptChallan/DocDesign/Designes/" + rptName + ".repx");
                }
                else if (RptModuleName == "ASSIGNJOB")
                {
                    if (string.IsNullOrEmpty(Page.Title))
                    {
                        Page.Title = "Assign Job_" + rptName.Split('~')[0];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/AssignJob/DocDesign/Designes/" + rptName + ".repx");
                }
                else if (RptModuleName == "DELIVERYCHALLAN")
                {
                    if (string.IsNullOrEmpty(Page.Title))
                    {
                        Page.Title = "Delivery Challan_" + rptName.Split('~')[0];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/SRVDeliveryChallan/DocDesign/Designes/" + rptName + ".repx");
                }
                else if (RptModuleName == "SRVJOBSHEET")
                {
                    if (string.IsNullOrEmpty(Page.Title))
                    {
                        Page.Title = "JobSheet_" + rptName.Split('~')[0];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/SRVJobSheet/DocDesign/Designes/" + rptName + ".repx");
                }
                //End of Rev Debashis
                //Rev for STB Management Tanmoy
                else if (RptModuleName == "MoneyReceipt")
                {
                    if (string.IsNullOrEmpty(Page.Title))
                    {
                        Page.Title = "Money Receipt_" + rptName.Split('~')[0];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/MoneyReceipt/DocDesign/Designes/" + rptName + ".repx");
                }
                else if (RptModuleName == "WalletRecharge")
                {
                    if (string.IsNullOrEmpty(Page.Title))
                    {
                        Page.Title = "Wallet Recharge_" + rptName.Split('~')[0];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/WalletRecharge/DocDesign/Designes/" + rptName + ".repx");
                }
                else if (RptModuleName == "PAYSLIP")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Pay Slip-" + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/PaySlip/DocDesign/Designes/" + rptName + ".repx");
                }
                //End of Rev for STB Management Tanmoy
                //Rev Debashis
                else if (RptModuleName == "STATEMENTOFACCOUNT")
                {
                    if (string.IsNullOrEmpty(Page.Title))
                    {
                        Page.Title = "StatementOfAccount_" + rptName.Split('~')[0];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/SOA/Designes/" + rptName + ".repx");
                }
                //End of Rev Debashis
                //END OF ADDITION
                ExportFileName = Page.Title;
                DevExpress.DataAccess.Sql.SqlDataSource sql = GenerateSqlDataSource(RptModuleName);
                XtraReport newXtraReport = XtraReport.FromFile(filePath, true);
                newXtraReport.LoadLayout(filePath);
                newXtraReport.DataSource = sql;
                filePathtoPDF = filePath;
                filePathtoPDF = filePathtoPDF.Split('~')[0];

                //if (RptModuleName == "DChallan")
                //{
                //    newXtraReport.Landscape = true;
                //}

                //if (RptModuleName == "BRCODE")
                //{
                //    newXtraReport.Landscape = true;
                //}
                if (RptModuleName == "Invoice" || RptModuleName == "Invoice_POS" || RptModuleName == "Order_POS" || RptModuleName == "Second_Hand" || RptModuleName == "PInvoice" || RptModuleName == "Self_PInvoice" || RptModuleName == "PINV_CUST" || RptModuleName == "PINV_TRANSPORTR" || RptModuleName == "RChallan" || RptModuleName == "PChallan" || RptModuleName == "PDChallan" || RptModuleName == "TSInvoice")
                {
                    if (PrintType == "1")
                    {
                        ReportType = "Original";
                    }
                    else if (PrintType == "2")
                    {
                        ReportType = "Duplicate";
                    }
                    else if (PrintType == "3")
                    {
                        ReportType = "DuplicateFinance";
                    }
                    else
                    {
                        ReportType = "Triplicate";
                    }
                }
                if (RptModuleName == "Invoice_POS" || RptModuleName == "Second_Hand" || RptModuleName == "POS_Duplicate" || RptModuleName == "Order_POS")
                {
                    if (PrintType == "1")
                    {
                        newXtraReport.Watermark.Text = "ORIGINAL";
                        newXtraReport.Watermark.Font = new Font(newXtraReport.Watermark.Font.FontFamily, 109); //145
                    }
                    else if (PrintType == "2")
                    {
                        newXtraReport.Watermark.Text = "TRANSPORTER COPY";
                        newXtraReport.Watermark.Font = new Font(newXtraReport.Watermark.Font.FontFamily, 45); //60
                    }
                    else if (PrintType == "3")
                    {
                        newXtraReport.Watermark.Text = "FINANCER COPY";
                        newXtraReport.Watermark.Font = new Font(newXtraReport.Watermark.Font.FontFamily, 55); //73
                    }
                    else if (PrintType == "4")
                    {
                        newXtraReport.Watermark.Text = "SUPPLIER COPY";
                        newXtraReport.Watermark.Font = new Font(newXtraReport.Watermark.Font.FontFamily, 57); //75
                    }
                    else
                    {
                        newXtraReport.Watermark.Text = "DUPLICATE";
                        newXtraReport.Watermark.Font = new Font(newXtraReport.Watermark.Font.FontFamily, 109); //145
                    }
                    newXtraReport.Watermark.TextDirection = DirectionMode.ForwardDiagonal;
                    newXtraReport.Watermark.ForeColor = Color.LightSlateGray;
                    newXtraReport.Watermark.TextTransparency = 180;
                    newXtraReport.Watermark.ShowBehind = false;
                    newXtraReport.Watermark.PageRange = "1-2";
                }
                //Rev Tanmoy
                //Rev Debashis #0023696
                //if (RptModuleName == "MoneyReceipt" || RptModuleName == "WalletRecharge")
                //{
                //    if (PrintType == "1")
                //    {
                //        ReportType = "Original";
                //    }
                //    else if (PrintType == "2")
                //    {
                //        ReportType = "Duplicate";
                //    }
                //}
                //if (RptModuleName == "MoneyReceipt" || RptModuleName == "WalletRecharge")
                //{
                //    if (PrintType == "1")
                //    {
                //        newXtraReport.Watermark.Text = "ORIGINAL";
                //        newXtraReport.Watermark.Font = new Font(newXtraReport.Watermark.Font.FontFamily, 45); //109
                //    }
                //    else if (PrintType == "1")
                //    {
                //        newXtraReport.Watermark.Text = "DUPLICATE";
                //        newXtraReport.Watermark.Font = new Font(newXtraReport.Watermark.Font.FontFamily, 45); //109
                //    }
                //    newXtraReport.Watermark.TextDirection = DirectionMode.ForwardDiagonal;
                //    newXtraReport.Watermark.ForeColor = Color.LightSlateGray;
                //    newXtraReport.Watermark.TextTransparency = 180;
                //    newXtraReport.Watermark.Text = DevExpress.XtraPrinting.TextAlignment.TopCenter.ToString();
                //    newXtraReport.Watermark.ShowBehind = false;
                //    newXtraReport.Watermark.PageRange = "1-2";
                //}
                //End of Rev Debashis
                //End of Rev Tanmoy

                //Rev Debashis
                if (RptModuleName == "Porder" && rptName == "PO-EVAC~D")
                {
                    DataTable dtPOApproveStat = null;
                    string POApproveStat = "";
                    DocidforWaterMark = "109";
                    dtPOApproveStat = oDBEngine.GetDataTable("Select ProjectPurchase_ApproveStatus from tbl_trans_PurchaseOrder Where IsProjectOrder=1 and CONVERT(NVARCHAR(MAX),PurchaseOrder_Id)='" + DocidforWaterMark + "'");
                    POApproveStat = dtPOApproveStat.Rows[0][0].ToString();
                    if (POApproveStat == "1")
                    {
                        newXtraReport.Watermark.Text = "APPROVED";
                        newXtraReport.Watermark.Font = new Font(newXtraReport.Watermark.Font.FontFamily, 55);
                        newXtraReport.Watermark.TextDirection = DirectionMode.ForwardDiagonal;
                        newXtraReport.Watermark.ForeColor = Color.LightSlateGray;
                        newXtraReport.Watermark.TextTransparency = 180;
                        newXtraReport.Watermark.ShowBehind = false;
                        newXtraReport.Watermark.PageRange = "1-2";
                    }
                }
                if (RptModuleName == "BranchReq")
                {
                    DataTable dtBRApproveStat = null;
                    string BRApproveStat = "";
                    DocidforWaterMark = "79";
                    dtBRApproveStat = oDBEngine.GetDataTable("Select ERPApproval_Status from tbl_trans_ERPDocApprovalStatus Where ERPApproval_DOCType='BR' and CONVERT(NVARCHAR(MAX),ERPApproval_DocId)='" + DocidforWaterMark + "'");
                    BRApproveStat = dtBRApproveStat.Rows[0][0].ToString();
                    if (BRApproveStat == "D")
                    {
                        newXtraReport.Watermark.Text = "APPROVED";
                        newXtraReport.Watermark.Font = new Font(newXtraReport.Watermark.Font.FontFamily, 55);
                        newXtraReport.Watermark.TextDirection = DirectionMode.ForwardDiagonal;
                        newXtraReport.Watermark.ForeColor = Color.LightSlateGray;
                        newXtraReport.Watermark.TextTransparency = 180;
                        newXtraReport.Watermark.ShowBehind = false;
                        newXtraReport.Watermark.PageRange = "1-2";
                    }
                }
                if (RptModuleName == "BranchTranOut")
                {
                    if (dtWatermark.Rows[0][0].ToString() == "Yes")
                    {
                        if (PrintType == "1")
                        {
                            newXtraReport.Watermark.Text = "BRANCH COPY";
                            newXtraReport.Watermark.Font = new Font(newXtraReport.Watermark.Font.FontFamily, 55);
                        }
                        else if (PrintType == "2")
                        {
                            newXtraReport.Watermark.Text = "GODOWN COPY";
                            newXtraReport.Watermark.Font = new Font(newXtraReport.Watermark.Font.FontFamily, 55);
                        }
                        newXtraReport.Watermark.TextDirection = DirectionMode.ForwardDiagonal;
                        newXtraReport.Watermark.ForeColor = Color.LightSlateGray;
                        newXtraReport.Watermark.TextTransparency = 180;
                        newXtraReport.Watermark.ShowBehind = false;
                        newXtraReport.Watermark.PageRange = "1-2";
                    }
                }
                //End of Rev Debashis
                newXtraReport.DisplayName = ExportFileName;
                ASPxDocumentViewer1.Report = newXtraReport;
                //newXtraReport.DisplayName = ExportFileName;

                //// Create a new memory stream and export the report into it as PDF.
                //MemoryStream mem = new MemoryStream();
                //newXtraReport.ExportToPdf(mem);

                //// Create a new attachment and put the PDF report into it.
                //mem.Seek(0, System.IO.SeekOrigin.Begin);
                //Attachment att = new Attachment(mem, "aa.pdf", "application/pdf");

                //// Create a new message and attach the PDF report to it.
                //MailMessage mail = new MailMessage();
                //mail.Attachments.Add(att);

                //// Specify sender and recipient options for the e-mail message.
                //mail.From = new MailAddress("debashis.talukder@indusnet.co.in", "Debashis");
                ////mail.To.Add(new MailAddress(newXtraReport.ExportOptions.Email.RecipientAddress,newXtraReport.ExportOptions.Email.RecipientName));
                ////mail.To.Add(new MailAddress(newXtraReport.ExportOptions.Email.AddRecipient, newXtraReport.ExportOptions.Email.AddRecipient));
                //mail.To.Add(new MailAddress("debashis.talukder@indusnet.co.in", "Debashis"));

                //// Specify other e-mail options.
                //mail.Subject = newXtraReport.ExportOptions.Email.Subject;
                //mail.Body = "This is a test e-mail message sent by an application.";

                //// Send the e-mail message via the specified SMTP server.
                //SmtpClient smtp = new SmtpClient("smtp.gmail.com",25);
                //smtp.Send(mail);

                //// Close the memory stream.
                //mem.Close();
                //}



            }
            if (!IsPostBack)
            {
                HDRepornName.Value = Convert.ToString(Request.QueryString["reportname"]);
            }
        }

        private DevExpress.DataAccess.Sql.SqlDataSource GenerateSqlDataSource(String RptModuleName)
        {
            //Mantis issue number #0018881 started
            //DevExpress.DataAccess.Sql.SqlDataSource result = new DevExpress.DataAccess.Sql.SqlDataSource("crmConnectionString");

            CustomStringConnectionParameters connectionParameters = new CustomStringConnectionParameters(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            DevExpress.DataAccess.Sql.SqlDataSource result = new DevExpress.DataAccess.Sql.SqlDataSource(connectionParameters);
            //Mantis issue number #0018881 end

            //BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine();
            //Rev Debashis
            //string Module_Name = Convert.ToString(Session["Module_Name"]);
            string Module_Name = "";
            if (RptModuleName == "TRIALONNETBALSUMARY" || RptModuleName == "STATEMENTOFACCOUNT")
            {
                Module_Name = RptModuleName;
            }
            else
            {
                Module_Name = Convert.ToString(Session["Module_Name"]);
            }
            //End of Rev Debashis
            DataTable dtRptTables = new DataTable();
            string query = "";

            query = @"Select Query_Table_name from tbl_trans_ReportSql where Module_name = '" + Module_Name + "' order by Query_ID ";
            dtRptTables = oDbEngine.GetDataTable(query);
            //dtRptTables.TableName = "aaa";
            string CashBankType = "";
            string CustVendType = "";
            string SalesPurchaseType = "";
            string DocumentID = "3";//"101";
            string Doctype = "PO";
            string BranchId = "100";
           
            #region  for logo image
            string[] filePaths = new string[] { };
            string path = System.Web.HttpContext.Current.Server.MapPath("~");
            string path1 = path.Replace("Reports\\", "ERP.UI");
            string fullpath = path1.Replace("\\", "/");
            #endregion

            if (RptModuleName == "BankBook")
            {
                CashBankType = "Bank";
            }
            else if (RptModuleName == "CashBook")
            {
                CashBankType = "Cash";
            }
            else if (RptModuleName == "LedgerPost")
            {
                CashBankType = "";
            }

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

            if (RptModuleName == "SChallan" || RptModuleName == "PDChallan" || RptModuleName == "Invoice" || RptModuleName == "Invoice_POS" || RptModuleName == "Order_POS" || RptModuleName == "Second_Hand"
                || RptModuleName == "POS_Duplicate" || RptModuleName == "TSInvoice" || RptModuleName == "Sales_Return" || RptModuleName == "Sorder" || RptModuleName == "RateDiff_Entry_Cust" )
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
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_MATERIALINOUTREG '" + Convert.ToString(Session["LastCompany"]) + "','" + StartDate.Value + "','" + EndDate.Value + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + Convert.ToString(row[0]) + "','" + Convert.ToString(Session["SelectedBranchList"]) + "','" + Convert.ToString(Session["SelectedTagProductList"]) + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "StockTrialDet")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_MATERIALINOUTREGDET_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + StartDate.Value + "','" + EndDate.Value + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + Convert.ToString(row[0]) + "','" + Convert.ToString(Session["SelectedBranchList"]) + "','" + Convert.ToString(Session["SelectedTagProductList"]) + "','" + "P" + "'"));
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
                    //result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_MATERIALINOUTREGWH_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + StartDate.Value + "','" + EndDate.Value + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + Convert.ToString(row[0]) + "','" + Convert.ToString(Session["SelectedBranchList"]) + "','" + Convert.ToString(Session["SelectedTagProductList"]) + "','" + "P" + "'"));
                    //result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_MATERIALINOUTREGWH_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + StartDate.Value + "','" + EndDate.Value + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + Convert.ToString(row[0]) + "','" + Convert.ToString(Session["SelectedBranchList"]) + "','" + Convert.ToString(Session["SelectedTagProductList"]) + "','" + Convert.ToString(Session["SelectedTagWarehouseList"]) + "','" + "P" + "'"));
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_MATERIALINOUTREGWH_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + StartDate.Value + "','" + EndDate.Value + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + Convert.ToString(row[0]) + "','" + Convert.ToString(Session["SelectedBranchList"]) + "','" + Convert.ToString(Session["SelectedTagProductList"]) + "','" + Convert.ToString(Session["SelectedTagWarehouseList"]) + "','" + RptModuleName + "','" + "P" + "'"));
                    //End of Rev Debashis
                }
            }
            else if (RptModuleName == "Invoice" || RptModuleName == "Invoice_POS" || RptModuleName == "Second_Hand" || RptModuleName == "POS_Duplicate" || RptModuleName == "PInvoice" || RptModuleName == "Self_PInvoice" || RptModuleName == "PINV_CUST")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_TAXINVOICE '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + SalesPurchaseType + "','" + "" + "','" + "P" + "'"));
                }
            }
            // Rev Sayantani
            else if (RptModuleName == "INFLUENCER")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_INFLUENCER_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + "" + "','" + "P" + "'"));
                }
            }
            // End Of Rev Sayantani
            else if (RptModuleName == "GPAYROLL")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PRC_GENERATEPAYSLIP '" + Convert.ToString(Session["LastCompany"]) + "','" + "STRUCT0000000006" + "','" + "EMB0000026" + "','" + "1804" + "','" + "" + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "PSTICKER")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PRC_PRINTPRODUCTS_REPORT '" + DocumentID + "','" + Convert.ToString(row[0]) + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "PINDENT")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_PURCHASEINDENT_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + "" + "','" + "P" + "'"));

                }
            }
            else if (RptModuleName == "PINV_TRANSPORTR")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PRC_TRANSPORTERINVOICE_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + SalesPurchaseType + "','" + "" + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "OrderChallan_POS")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_DELIVERYCUMORDERCHALLAN_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + "S" + "','" + "2" + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "Order_POS")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_POSORDER_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + SalesPurchaseType + "','" + "" + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "TSInvoice" || RptModuleName == "TPInvoice")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_TRANSITSALEPURCHASE_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + SalesPurchaseType + "','" + "" + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "ODSDChallan")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_DELIVERYPENDINGCHALLAN_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + "S" + "','" + "2" + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "CUSTRECPAY" || RptModuleName == "VENDRECPAY")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_CUSTVENDRECPAY '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + CustVendType + "','" + "" + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "AdvCustCr")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_ADVANCEDWITH_CRDR_NOTE '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + CustVendType + "','" + "" + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "CBVUCHR")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_CASHBANKVOUCHER_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + "" + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "RChallan")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_DELIVERYCHALLAN '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + "" + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "Sales_Return" || RptModuleName == "Purchase_Return" || RptModuleName == "RateDiff_Entry_Cust" || RptModuleName == "RateDiff_Entry_Vend")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_SALERETURN_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + SalesPurchaseType + "','" + "" + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "PURCHASE_RET_REQ")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_PURCHASERETURN_REQUEST_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + SalesPurchaseType + "','" + "" + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "OLDUNTRECVD")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_OLDUNITRECEIVED_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + "" + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "Install_Coupon")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_INSTALLATIONCOUPON_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + fullpath + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + Convert.ToString(row[0]) + "','" + "86" + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "LedgerPost" || RptModuleName == "BankBook" || RptModuleName == "CashBook")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_LEDGERPOSTING '" + Convert.ToString(Session["LastCompany"]) + "','" + StartDate.Value + "','" + EndDate.Value + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + Convert.ToString(row[0]) + "','" + Convert.ToString(Session["SelectedBranchList"]) + "','" + CashBankType + "','" + Convert.ToString(Session["SelectedCashBankList"]) + "','" + Convert.ToString(Session["SelectedUserList"]) + "','" + Convert.ToString(Session["SelectedTagPartyList"]) + "','" + Convert.ToString(Session["SelectedTagLedgerList"]) + "','" + Convert.ToString(Session["SelectedTagEmployeeList"]) + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "Sorder" || RptModuleName == "Porder")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_SALEPURCHASEORDER_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + SalesPurchaseType + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "BranchReq")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_BRANCHREQ_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + "21" + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "BranchTranOut")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    //Rev Debashis
                    //result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_BRANCHOUT_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + "P" + "'"));
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_BRANCHOUT_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + "" + "','" + "P" + "'"));
                    //End of Rev Debashis
                }
            }
            else if (RptModuleName == "BranchTranIn")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_BRANCHTRANSFERIN_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "SChallan" || RptModuleName == "PDChallan" || RptModuleName == "PChallan")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_SALEPURCHASECHALLAN_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + SalesPurchaseType + "','" + "" + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "CONTRAVOUCHER")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_CONTRAVOUCHER_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "JOURNALVOUCHER")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_JOURNALVOUCHER_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "WarehouseStockTrans")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_STOCKTRANSFER_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + "" + "','" + "P" + "'"));
                }
            }

            else if (RptModuleName == "ManufacturingBOM")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_MANUFACTURINGBOM '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + "" + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "JobWorkOrder")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_MANUFACTURINGJobWorkOrder '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + "" + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "FGReceived")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_MANUFACTURINGFGReceived '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + "" + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "MMaterialsIssue")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_MANUFACTURINGMMatialsIssue '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + "" + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "ManufacturingPR")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_MANUFACTURINGBOM '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + "" + "','" + "P" + "'"));
                }
            }

            else if (RptModuleName == "WarehouseStockJrnl")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_WAREHOUSE_WISE_STOCK_JOURNAL '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + "" + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "WarehouseStockIN")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_WAREHOUSE_WISE_STOCK_IN '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + "" + "','" + "P" + "'"));
                }
            }
            //Rev Tanmoy
            else if (RptModuleName == "WarehouseStockOUT")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_WAREHOUSE_WISE_STOCK_OUT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + "" + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "EstimateCosting")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_ESTIMATECOSTING_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + "" + "','" + "P" + "'"));
                }
            }
            //End of Rev Tanmoy
            else if (RptModuleName == "CUSTDRCRNOTE" || RptModuleName == "VENDDRCRNOTE")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_DEBITCREDITNOTE_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + CustVendType + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "BRCODE")
            {
                //foreach (DataRow row in dtRptTables.Rows)
                //{
                //    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_BARCODE_PRINT '" + DocumentID + "','" + Doctype + "','" + Convert.ToString(row[0]) + "','" + BranchId + "','" + "P" + "'"));
                //}

                if (!IsPostBack)
                {
                    DataTable dtListBarcode = new DataTable();
                    ProcedureExecute proclistbarcode = new ProcedureExecute("PROC_LISTBARCODE");
                    proclistbarcode.AddVarcharPara("@DOCID_N_PRODID", 500, Convert.ToString(DocumentID));
                    proclistbarcode.AddVarcharPara("@DOC_TYPE", 3000, Convert.ToString(Doctype));
                    proclistbarcode.AddVarcharPara("@BRANCH_ID", 10, Convert.ToString(BranchId));
                    dtListBarcode = proclistbarcode.GetTable();
                    Session["ListOfBarcode"] = dtListBarcode.Rows[0][0].ToString();

                    //============================Update==================================
                    DataTable dt = new DataTable();
                    ProcedureExecute proc = new ProcedureExecute("PROC_UPDATE_BARCODEPRINT");
                    proc.AddVarcharPara("@DOCID_N_PRODID", 500, Convert.ToString(DocumentID));
                    proc.AddVarcharPara("@DOC_TYPE", 3000, Convert.ToString(Doctype));
                    //proc.AddIntegerPara("@IS_UPD_BARCODE", Convert.ToInt32(Session["S_UPDATEFORBARCODE"]));
                    proc.AddVarcharPara("@BRANCH_ID", 10, Convert.ToString(BranchId));
                    dt = proc.GetTable();
                    //====================================================================
                }


                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_BARCODE_PRINT '" + Doctype + "','" + Convert.ToString(row[0]) + "','" + "P" + "','" + Session["ListOfBarcode"].ToString() + "'"));
                }

            }
            else if (RptModuleName == "PIQuotation")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_PROFORMAINVQUOTATION '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + "P" + "'"));
                }
            }
            //mantise issue:0025139
            else if (RptModuleName == "SalesInquiry")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_SALESINQUIRY_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + "P" + "'"));
                }
            }
            //End of mantise issue:0025139
            else if (RptModuleName == "ImpPorder")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PRC_IMPORTPURCHASEORDER_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + "P" + "'"));
                }
            }
            //Rev Debashis
            else if (RptModuleName == "TRIALONNETBALSUMARY")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    //Rev Debashis
                    //result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PRC_TRIALONNETBALANCE_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + StartDate.Value + "','" + EndDate.Value + "','" + BranId + "','" + Asondt + "','" + Convert.ToString(Zerobal) + "','" + Ho + "','" + GrpLst + "','" + Convert.ToString(ClStk) + "','" + Valtech + "','" + Convert.ToString(Owmvt) + "','" + Convert.ToString(PG) + "','" + Convert.ToString(Hrchy) + "','" + Convert.ToString(SwGrp) + "','" + Convert.ToString(NDrcrOpCl) + "','" + Convert.ToString(OpAsonDt) + "','" + Convert.ToString(row[0]) + "','" + Convert.ToString(1) + "','" + "P" + "','" + Convert.ToInt32(Session["userid"]) + "'"));
                    //result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PRC_TRIALONNETBALANCE_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + StartDate.Value + "','" + EndDate.Value + "','" + BranId + "','" + Asondt + "','" + Convert.ToString(Zerobal) + "','" + Ho + "','" + GrpLst + "','" + Convert.ToString(ClStk) + "','" + Valtech + "','" + Convert.ToString(Owmvt) + "','" + Convert.ToString(PG) + "','" + Convert.ToString(Hrchy) + "','" + Convert.ToString(SwGrp) + "','" + Convert.ToString(NDrcrOpCl) + "','" + Convert.ToString(OpAsonDt) + "','" + Convert.ToString(ConsLandCost) + "','" + Convert.ToString(row[0]) + "','" + Convert.ToString(1) + "','" + "P" + "','" + Convert.ToInt32(Session["userid"]) + "'"));
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PRC_TRIALONNETBALANCE_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + StartDate.Value + "','" + EndDate.Value + "','" + BranId + "','" + Asondt + "','" + Convert.ToString(Zerobal) + "','" + Ho + "','" + GrpLst + "','" + Convert.ToString(ClStk) + "','" + Valtech + "','" + Convert.ToString(Owmvt) + "','" + Convert.ToString(PG) + "','" + Convert.ToString(Hrchy) + "','" + Convert.ToString(SwGrp) + "','" + Convert.ToString(NDrcrOpCl) + "','" + Convert.ToString(OpAsonDt) + "','" + Convert.ToString(ConsLandCost) + "','" + Convert.ToString(ConsOverheadCost) + "','" + Convert.ToString(row[0]) + "','" + Convert.ToString(1) + "','" + "P" + "','" + Convert.ToInt32(Session["userid"]) + "'"));
                    //End of Rev Debashis
                }
            }
            else if (RptModuleName == "BRSSTATEMENT")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PRC_BRSDETAILSUMMARYFORMAT_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + StartDate.Value + "','" + EndDate.Value + "','" + Convert.ToString(Session["SelectedBranchList"]) + "','" + Convert.ToString(Session["SelectedCashBankList"]) + "','" + Convert.ToString(row[0]) + "','" + Convert.ToString(Session["ReportGenerationType"]) + "','" + "P" + "'"));
                }
            }
            //Rev Debashis
            else if (RptModuleName == "BRSCONSSTATEMENT")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PRC_BRSCONSOLIDATEDFORMAT_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + StartDate.Value + "','" + EndDate.Value + "','" + Convert.ToString(Session["SelectedCashBankList"]) + "','" + Convert.ToString(row[0]) + "','" + Convert.ToString(Session["ReportGenerationType"]) + "','" + "P" + "'"));
                }
            }
            //End of Rev Debashis
            else if (RptModuleName == "RECPTCHALLAN")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PRC_RECEIPTCHALLANPRINT_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "ASSIGNJOB")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PRC_ASSIGNJOBPRINT_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "DELIVERYCHALLAN")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PRC_DELIVERYCHALLANPRINT_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "SRVJOBSHEET")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PRC_JOBSHEETPRINT_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + "P" + "'"));
                }
            }
            //End of Rev Debashis
            //Rev for STB Management Tanmoy
            else if (RptModuleName == "MoneyReceipt")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PRC_MoneyReceipt_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + "" + "','" + Convert.ToString(Session["UserID"]).Trim() + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "WalletRecharge")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PRC_WalletRecharge_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + "" + "','" + Convert.ToString(Session["UserID"]).Trim() + "','" + "P" + "'"));
                }
            }

            else if (RptModuleName == "PAYSLIP")
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
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PRC_SOAPARTYWISE_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + StartDate.Value + "','" + EndDate.Value + "','" + BranId + "','" + PartyIds + "','" + Criteria + "','" + Convert.ToString(ShowAllParty) + "','" + ProjList + "','" + Convert.ToString(ShowHeader) + "','" + Convert.ToString(ShowFooter) + "','"+ Convert.ToString(row[0]) + "','" + "P" + "'"));
                }
            }
            //End of Rev Debashis
            DataTable dtRptRelation = new DataTable();
            string RelationQuery = "";

            RelationQuery = @"Select Parent_Query_name,Child_Query_name, Parent_Column_name,Child_Column_name from tbl_trans_ReportTableRelation where Module_name = '" + Module_Name + "' order by Query_ID ";
            dtRptRelation = oDbEngine.GetDataTable(RelationQuery);
            if (dtRptRelation.Rows.Count > 0)
            {
                foreach (DataRow row in dtRptRelation.Rows)
                {
                    result.Relations.Add(Convert.ToString(row[0]), Convert.ToString(row[1]), Convert.ToString(row[2]), Convert.ToString(row[3]));
                }
            }

            result.RebuildResultSchema();
            return result;
        }
        [WebMethod]
        public static List<string> GetRecipients()
        {
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

            //DataTable DT = oDBEngine.GetDataTable("tbl_master_user", "user_name,user_id ", null);

            DataTable dtEmailTable = new DataTable();
            string EmailQuery = "";
            EmailQuery = @"select add_id as ID,add_Email as Email from tbl_master_address where add_Email<>'' or add_Email is not null union all select eml_id as ID,eml_ccEmail as Email from tbl_master_email where eml_ccEmail<>'' ";
            dtEmailTable = oDBEngine.GetDataTable(EmailQuery);

            List<string> obj = new List<string>();
            foreach (DataRow dr in dtEmailTable.Rows)
            {
                //obj.Add(Convert.ToString(dr["user_name"]) + "|" + Convert.ToString(dr["user_id"]));
                obj.Add(Convert.ToString(dr["Email"]) + "|" + Convert.ToString(dr["ID"]));
            }

            return obj;
        }

        [WebMethod]
        public static string GetFromEmail()
        {
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            DataTable dtFromEmail = new DataTable();
            string FromEmailDesc="";
            dtFromEmail = oDBEngine.GetDataTable("select top(1) EmailAccounts_EmailID from Config_EmailAccounts where EmailAccounts_InUse='Y'");
            FromEmailDesc = dtFromEmail.Rows[0][0].ToString();
            return FromEmailDesc;
        }

        protected void CallbackPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            try
            {
                //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable dtFromEmailDet = new DataTable();
                dtFromEmailDet = oDBEngine.GetDataTable("select top(1) EmailAccounts_Password,EmailAccounts_FromName,LTRIM(RTRIM(EmailAccounts_SMTP)) AS EmailAccounts_SMTP,LTRIM(RTRIM(EmailAccounts_SMTPPort)) AS EmailAccounts_SMTPPort from Config_EmailAccounts where EmailAccounts_InUse='Y'");
                var Password = dtFromEmailDet.Rows[0][0].ToString();
                var FromWhere = dtFromEmailDet.Rows[0][1].ToString();
                var OutgoingSMTPHost = dtFromEmailDet.Rows[0][2].ToString();
                var OutgoingPort = dtFromEmailDet.Rows[0][3].ToString();
                var Rpt = ASPxDocumentViewer1.Report;
                // Create a new memory stream and export the report into it as PDF.
                MemoryStream mem = new MemoryStream();
                Rpt.ExportToPdf(mem);

                // Create a new attachment and put the PDF report into it.
                mem.Seek(0, System.IO.SeekOrigin.Begin);
                Attachment att = new Attachment(mem, Rpt.DisplayName + ".pdf", "application/pdf");

                // Create a new message and attach the PDF report to it.
                MailMessage mail = new MailMessage();
                //SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                SmtpClient smtp = new SmtpClient(OutgoingSMTPHost);
                mail.Attachments.Add(att);
               
                var FromAdd = txtFrom.Text;
                //var ToAdd = txtTo.Text;
                var ToAdd = hndSelectRecipients.Value;
                var CcAdd = txtCc.Text;
                var Body = txtMailBody.Text;
                var Subject = txtSubject.Text;

                // Specify sender and recipient options for the e-mail message.
                //mail.From = new MailAddress("bcool4u@gmail.com","Debashis");
                //mail.To.Add("debashis.talukder@indusnet.co.in");
                //mail.CC.Add("subhra.mukherjee@indusnet.co.in");
                //mail.Subject = "This is a Test Mail";
                mail.From = new MailAddress(FromAdd, FromWhere);
                mail.To.Add(ToAdd);
                if (CcAdd != "")
                {
                    mail.CC.Add(CcAdd);
                }
                mail.Subject = Subject;
                mail.IsBodyHtml = true;
                //mail.Body = "This is a test e-mail message sent by an application.";
                mail.Body = Body;
                //smtp.Host = "smtp.gmail.com";
                //smtp.Port = 587;
                smtp.Host = OutgoingSMTPHost.Trim();
                smtp.Port = Convert.ToInt32(OutgoingPort);
                //smtp.Credentials = new System.Net.NetworkCredential("bcool4u@gmail.com", "*********");
                smtp.Credentials = new System.Net.NetworkCredential(FromAdd, Password);
                smtp.EnableSsl = true;
                smtp.Send(mail);
                att.Dispose();
                smtp.Dispose();
                mail.Dispose();
                //Response.Write("Email Send successfully");
                // Close the memory stream.
                mem.Close();
                //Response.End();
                //Rpt.Dispose();

                //mail.To.Add(new MailAddress(newXtraReport.ExportOptions.Email.RecipientAddress,newXtraReport.ExportOptions.Email.RecipientName));
                //mail.To.Add(new MailAddress(newXtraReport.ExportOptions.Email.AddRecipient, newXtraReport.ExportOptions.Email.AddRecipient));
                //mail.To.Add(new MailAddress("bcool4u@gmail.com", "Debashis"));
                // Specify other e-mail options.
                //mail.Subject = newXtraReport.ExportOptions.Email.Subject;
                // Send the e-mail message via the specified SMTP server.
                //SmtpClient smtp = new SmtpClient("smtp.gmail.com", 25);
            }
            catch (Exception ex)
            {
                ASPxDocumentViewer1.JSProperties["cpErrorResult"] = ex.Message;
            }
        }
    }
}