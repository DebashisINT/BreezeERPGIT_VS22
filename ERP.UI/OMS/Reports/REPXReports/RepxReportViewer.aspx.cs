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
using ERP.OMS.Reports.XtraReports;
using DevExpress.DataAccess.Sql;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrinting.Preview;
using DevExpress.Web;
using System.Net.Mail;
using System.Drawing;
using DevExpress.XtraPrinting.Drawing;
using DataAccessLayer;
using System.Web.Services;
using DevExpress.DataAccess.ConnectionParameters;

namespace ERP.OMS.Reports.REPXReports
{
    public partial class RepxReportViewer : System.Web.UI.Page
    {
        BusinessLogicLayer.ReportLayout rpLayout = new BusinessLogicLayer.ReportLayout();
        BusinessLogicLayer.ReportData rptData = new BusinessLogicLayer.ReportData();

        string Module_name = "";
        DataTable dtWatermark = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                string tempFile = HttpContext.Current.Request.QueryString["Previewrpt"];
                string RptModuleName = HttpContext.Current.Request.QueryString["modulename"]; //Convert.ToString(Session["NewRptModuleName"]);   
                string PrintType = HttpContext.Current.Request.QueryString["PrintOption"];
                string DocumentID = HttpContext.Current.Request.QueryString["id"];
                string Doctype = HttpContext.Current.Request.QueryString["doctype"];

                DBEngine odbeng = new DBEngine();
                DataTable WatermarkDt = odbeng.GetDataTable("select Variable_Value from Config_SystemSettings where Variable_Name='ShowWatermarkinDocumentPrint'");

                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                string rptName = tempFile;
                string filePath = "";
                string filePathtoPDF = "";
                string ReportType = "";

                string[] filePaths = new string[] { };
                string DesignPath = "";
                string PDFFilePath = "";
                string ExportFileName = "";

                if (RptModuleName == "Invoice")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Sale Invoice-" + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    Module_name = "SALETAX";
                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\SalesInvoice\DocDesign\Normal\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\SalesInvoice\DocDesign\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\SalesInvoice\DocDesign\Normal\";
                        PDFFilePath = @"Reports\RepxReportDesign\SalesInvoice\DocDesign\PDFFiles\";
                    }
                }
                else if (RptModuleName == "TSInvoice")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Transit Sale Invoice-" + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    Module_name = "SALETAX";
                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\SalesInvoice\DocDesign\Transit\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\SalesInvoice\DocDesign\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\SalesInvoice\DocDesign\Transit\";
                        PDFFilePath = @"Reports\RepxReportDesign\SalesInvoice\DocDesign\PDFFiles\";
                    }
                }
                else if (RptModuleName == "GPAYROLL")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Generate PayRoll-" + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    Module_name = "PAYSLIP";
                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\PayRoll\DocDesign\Normal\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\PayRoll\DocDesign\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\PayRoll\DocDesign\Normal\";
                        PDFFilePath = @"Reports\RepxReportDesign\PayRoll\DocDesign\PDFFiles\";
                    }
                }
                //Rev 25-04-2019
                else if (RptModuleName == "PSTICKER")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Product Sticker-" + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    Module_name = "STICKER";
                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\PRODUCTSTICKER\DocDesign\Normal\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\PRODUCTSTICKER\DocDesign\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\PRODUCTSTICKER\DocDesign\Normal\";
                        PDFFilePath = @"Reports\RepxReportDesign\PRODUCTSTICKER\DocDesign\PDFFiles\";
                    }
                }
                //End of Rev 25-04-2019
                else if (RptModuleName == "TPInvoice")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Transit Purchase Invoice-" + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    Module_name = "TPURCHASE";
                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\TransitPurchaseInvoice\DocDesign\Normal\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\TransitPurchaseInvoice\DocDesign\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\TransitPurchaseInvoice\DocDesign\Normal\";
                        PDFFilePath = @"Reports\RepxReportDesign\TransitPurchaseInvoice\DocDesign\PDFFiles\";
                    }
                }
                else if (RptModuleName == "Invoice_POS")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Sale Invoice(POS)-" + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    Module_name = "SALETAX";

                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\SalesInvoice\DocDesign\SPOS\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\SalesInvoice\DocDesign\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\SalesInvoice\DocDesign\SPOS\";
                        PDFFilePath = @"Reports\RepxReportDesign\SalesInvoice\DocDesign\PDFFiles\";
                    }
                }
                else if (RptModuleName == "OrderChallan_POS")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Order Challan(POS)-" + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    Module_name = "SALETAX";

                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\OrderChallanPos\DocDesign\Designes\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\OrderChallanPos\DocDesign\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\OrderChallanPos\DocDesign\Designes\";
                        PDFFilePath = @"Reports\RepxReportDesign\OrderChallanPos\DocDesign\PDFFiles\";
                    }
                }

                else if (RptModuleName == "Order_POS")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Sale Order(POS)-" + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    Module_name = "POSSORDER";

                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\POSSALEORDER\DocDesign\SPOSORD\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\POSSALEORDER\DocDesign\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\POSSALEORDER\DocDesign\SPOSORD\";
                        PDFFilePath = @"Reports\RepxReportDesign\POSSALEORDER\DocDesign\PDFFiles\";
                    }
                }

                else if (RptModuleName == "Second_Hand")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Second Hand sales-" + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    Module_name = "SALETAX";

                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\SecondHandSales\DocDesign\Normal\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\SecondHandSales\DocDesign\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\SecondHandSales\DocDesign\Normal\";
                        PDFFilePath = @"Reports\RepxReportDesign\SecondHandSales\DocDesign\PDFFiles\";
                    }
                }
                else if (RptModuleName == "POS_Duplicate")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "POS Bill Print-" + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    Module_name = "SALETAX";

                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\SalesInvoice\DocDesign\POSDUPLICATE\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\SalesInvoice\DocDesign\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\SalesInvoice\DocDesign\POSDUPLICATE\";
                        PDFFilePath = @"Reports\RepxReportDesign\SalesInvoice\DocDesign\PDFFiles\";
                    }
                }
                else if (RptModuleName == "CUSTRECPAY")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Customer Rec/Pay-" + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    Module_name = "CUSTRECPAY";

                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\CustomerRecPay\DocDesign\Designes\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\CustomerRecPay\DocDesign\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\CustomerRecPay\DocDesign\Designes\";
                        PDFFilePath = @"Reports\RepxReportDesign\CustomerRecPay\DocDesign\PDFFiles\";
                    }
                }
                else if (RptModuleName == "VENDRECPAY")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Vendor Rec/Pay-" + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    Module_name = "CUSTRECPAY";

                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\VendorRecPay\DocDesign\Designes\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\VendorRecPay\DocDesign\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\VendorRecPay\DocDesign\Designes\";
                        PDFFilePath = @"Reports\RepxReportDesign\VendorRecPay\DocDesign\PDFFiles\";
                    }
                }
                else if (RptModuleName == "CBVUCHR")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Cash Bank Voucher-" + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    Module_name = "CASHBANK";
                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\CashBankVoucher\DocDesign\Designes\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\CashBankVoucher\DocDesign\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\CashBankVoucher\DocDesign\Designes\";
                        PDFFilePath = @"Reports\RepxReportDesign\CashBankVoucher\DocDesign\PDFFiles\";
                    }
                }
                else if (RptModuleName == "CUSTDRCRNOTE")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Customer Debit/Credit Note-" + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    Module_name = "CUSTVENDDRCRNOTE";

                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\CustDrCrNote\DocDesign\Designes\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\CustDrCrNote\DocDesign\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\CustDrCrNote\DocDesign\Designes\";
                        PDFFilePath = @"Reports\RepxReportDesign\CustDrCrNote\DocDesign\PDFFiles\";
                    }
                }
                else if (RptModuleName == "PAYSLIP")
                {
                    Module_name = "PAYSLIP";
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Pay Slip-" + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    filePath = Server.MapPath("/Reports/RepxReportDesign/PaySlip/DocDesign/Designes/" + rptName + ".repx");
                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\PaySlip\DocDesign\Designes\";
                        PDFFilePath = @"Reports\Reports\PaySlip\DocDesign\Designes\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\PaySlip\DocDesign\Designes\";
                        PDFFilePath = @"Reports\PaySlip\DocDesign\Designes\";
                    }
                }
                else if (RptModuleName == "VENDDRCRNOTE")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Vendor Debit/Credit Note-" + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    Module_name = "CUSTVENDDRCRNOTE";

                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\VendDrCrNote\DocDesign\Designes\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\VendDrCrNote\DocDesign\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\VendDrCrNote\DocDesign\Designes\";
                        PDFFilePath = @"Reports\RepxReportDesign\VendDrCrNote\DocDesign\PDFFiles\";
                    }
                }
                else if (RptModuleName == "ODSDChallan")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Sales Challan-" + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    Module_name = "SALETAX";

                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\DeliveryChallan\DocDesign\Designes\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\DeliveryChallan\DocDesign\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\DeliveryChallan\DocDesign\Designes\";
                        PDFFilePath = @"Reports\RepxReportDesign\DeliveryChallan\DocDesign\PDFFiles\";
                    }
                }
                else if (RptModuleName == "RChallan")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Road Challan-" + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    Module_name = "RCHALLAN";

                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\SalesChallan\DocDesign\CDelivery\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\SalesChallan\DocDesign\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\SalesChallan\DocDesign\CDelivery\";
                        PDFFilePath = @"Reports\RepxReportDesign\SalesChallan\DocDesign\PDFFiles\";
                    }
                }
                else if (RptModuleName == "SChallan")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Sales Challan-" + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    Module_name = "SCHALLAN";

                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\SalesChallan\DocDesign\Normal\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\SalesChallan\DocDesign\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\SalesChallan\DocDesign\Normal\";
                        PDFFilePath = @"Reports\RepxReportDesign\SalesChallan\DocDesign\PDFFiles\";
                    }
                }
                else if (RptModuleName == "PDChallan")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Pending Delivery List-" + rptName.Split('~')[0];
                    }
                    Module_name = "SCHALLAN";

                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\SalesChallan\DocDesign\Normal\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\SalesChallan\DocDesign\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\SalesChallan\DocDesign\Normal\";
                        PDFFilePath = @"Reports\RepxReportDesign\SalesChallan\DocDesign\PDFFiles\";
                    }
                }
                else if (RptModuleName == "PChallan")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Delivery Challan-" + rptName.Split('~')[0];
                    }
                    Module_name = "PCHALLAN";

                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\PurchaseChallan\DocDesign\Normal\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\PurchaseChallan\DocDesign\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\PurchaseChallan\DocDesign\Normal\";
                        PDFFilePath = @"Reports\RepxReportDesign\PurchaseChallan\DocDesign\PDFFiles\";
                    }
                }
                else if (RptModuleName == "PInvoice")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Purchase Invoice-" + rptName.Split('~')[0];
                    }
                    Module_name = "PURCHASE";

                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\PurchaseInvoice\DocDesign\Normal\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\PurchaseInvoice\DocDesign\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\PurchaseInvoice\DocDesign\Normal\";
                        PDFFilePath = @"Reports\RepxReportDesign\PurchaseInvoice\DocDesign\PDFFiles\";
                    }
                }
                else if (RptModuleName == "Self_PInvoice")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Self Purchase Invoice-" + rptName.Split('~')[0];
                    }
                    Module_name = "PURCHASE";

                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\SelfPurchaseInvoice\DocDesign\Normal\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\SelfPurchaseInvoice\DocDesign\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\SelfPurchaseInvoice\DocDesign\Normal\";
                        PDFFilePath = @"Reports\RepxReportDesign\SelfPurchaseInvoice\DocDesign\PDFFiles\";
                    }
                }
                else if (RptModuleName == "PINV_CUST")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Purchase Invoice For Customer-" + rptName.Split('~')[0];
                    }
                    Module_name = "PURCHASE";

                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\PurchaseInvForCustomer\DocDesign\Normal\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\PurchaseInvForCustomer\DocDesign\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\PurchaseInvForCustomer\DocDesign\Normal\";
                        PDFFilePath = @"Reports\RepxReportDesign\PurchaseInvForCustomer\DocDesign\PDFFiles\";
                    }
                }
                else if (RptModuleName == "PINV_TRANSPORTR")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Purchase Invoice For Transporter-" + rptName.Split('~')[0];
                    }
                    Module_name = "PUR_TRANSPORTER";

                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\PurchaseInvoiceForTransporter\DocDesign\Normal\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\PurchaseInvoiceForTransporter\DocDesign\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\PurchaseInvoiceForTransporter\DocDesign\Normal\";
                        PDFFilePath = @"Reports\RepxReportDesign\PurchaseInvoiceForTransporter\DocDesign\PDFFiles\";
                    }
                }
                else if (RptModuleName == "Sales_Return")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Sale Return-" + rptName.Split('~')[0];
                    }
                    Module_name = "SALERET";
                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\SalesReturn\DocDesign\Normal\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\SalesReturn\DocDesign\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\SalesReturn\DocDesign\Normal\";
                        PDFFilePath = @"Reports\RepxReportDesign\SalesReturn\DocDesign\PDFFiles\";
                    }
                }

                else if (RptModuleName == "RateDiff_Entry_Cust")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Rate Difference Entry Customer-" + rptName.Split('~')[0];
                    }
                    Module_name = "SALERET";
                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\RateDiffCustomer\DocDesign\Normal\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\RateDiffCustomer\DocDesign\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\RateDiffCustomer\DocDesign\Normal\";
                        PDFFilePath = @"Reports\RepxReportDesign\RateDiffCustomer\DocDesign\PDFFiles\";
                    }
                }
                else if (RptModuleName == "PURCHASE_RET_REQ")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Purchase Return Request-" + rptName.Split('~')[0];
                    }
                    Module_name = "PRETREQ";
                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\PurchaseRetRec\DocDesign\Normal\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\PurchaseRetRec\DocDesign\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\PurchaseRetRec\DocDesign\Normal\";
                        PDFFilePath = @"Reports\RepxReportDesign\PurchaseRetRec\DocDesign\PDFFiles\";
                    }
                }
                else if (RptModuleName == "OLDUNTRECVD")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Old Unit Received-" + rptName.Split('~')[0];
                    }
                    Module_name = "OLDUREC";
                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\OldUnitReceived\DocDesign\Normal\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\OldUnitReceived\DocDesign\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\OldUnitReceived\DocDesign\Normal\";
                        PDFFilePath = @"Reports\RepxReportDesign\OldUnitReceived\DocDesign\PDFFiles\";
                    }
                }
                else if (RptModuleName == "Purchase_Return")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Purchase Return-" + rptName.Split('~')[0];
                    }
                    Module_name = "PURRET";
                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\PurchaseReturn\DocDesign\Normal\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\PurchaseReturn\DocDesign\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\PurchaseReturn\DocDesign\Normal\";
                        PDFFilePath = @"Reports\RepxReportDesign\PurchaseReturn\DocDesign\PDFFiles\";
                    }
                }
                else if (RptModuleName == "RateDiff_Entry_Vend")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Rate Difference Entry Vendor-" + rptName.Split('~')[0];
                    }
                    Module_name = "PURRET";
                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\RateDiffVendor\DocDesign\Normal\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\RateDiffVendor\DocDesign\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\RateDiffVendor\DocDesign\Normal\";
                        PDFFilePath = @"Reports\RepxReportDesign\RateDiffVendor\DocDesign\PDFFiles\";
                    }
                }
                else if (RptModuleName == "Install_Coupon")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Installation Coupon-" + rptName.Split('~')[0];
                    }
                    Module_name = "INSCUPN";
                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\SalesInvoice\DocDesign\InstCoupon\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\SalesInvoice\DocDesign\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\SalesInvoice\DocDesign\InstCoupon\";
                        PDFFilePath = @"Reports\RepxReportDesign\SalesInvoice\DocDesign\PDFFiles\";
                    }
                }
                else if (RptModuleName == "PIQuotation")
                {
                    if (string.IsNullOrEmpty(Page.Title))
                    {
                        Page.Title = "Proforma Invoice/Quotation " + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    Module_name = "PIQUOTATION";
                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\Proforma\DocDesign\Designes\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\Proforma\DocDesign\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\Proforma\DocDesign\Designes\";
                        PDFFilePath = @"Reports\RepxReportDesign\Proforma\DocDesign\PDFFiles\";
                    }
                }
                //mantise issue:0025139
                else if (RptModuleName == "SalesInquiry")
                {
                    if (string.IsNullOrEmpty(Page.Title))
                    {
                        Page.Title = "Sales Inquiry " + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    Module_name = "SALESINQUIRY";
                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\SalesInquiry\DocDesign\Designes\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\SalesInquiry\DocDesign\PDFFiles\";                        
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\SalesInquiry\DocDesign\Designes\";
                        PDFFilePath = @"Reports\RepxReportDesign\SalesInquiry\DocDesign\PDFFiles\";                       
                    }
                }
                //End of mantise issue:0025139
                else if (RptModuleName == "BRCODE")
                {
                    if (string.IsNullOrEmpty(Page.Title))
                    {
                        Page.Title = "Opening Barcode " + rptName.Split('~')[0]; //RptModuleName;//ConfigurationManager.AppSettings[RptModuleName];
                    }
                    Module_name = "BRCODE";
                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\BarCode\DocDesign\Designes\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\BarCode\DocDesign\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\BarCode\DocDesign\Designes\";
                        PDFFilePath = @"Reports\RepxReportDesign\BarCode\DocDesign\PDFFiles\";
                    }
                }

                else if (RptModuleName == "BranchReq")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Branch Requisition-" + rptName.Split('~')[0];
                    }
                    Module_name = "BRANCHREQ";
                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\BranchRequisition\DocDesign\Designes\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\BranchRequisition\DocDesign\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\BranchRequisition\DocDesign\Designes\";
                        PDFFilePath = @"Reports\RepxReportDesign\BranchRequisition\DocDesign\PDFFiles\";
                    }
                }
                else if (RptModuleName == "BranchTranOut")
                {
                    dtWatermark = odbeng.GetDataTable("select Variable_Value from Config_SystemSettings where Variable_Name='ShowWatermarkinBTODocumentPrint'");
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Branch Transfer Out-" + rptName.Split('~')[0];
                    }
                    Module_name = "BRANCHTRANOUT";
                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\BranchTransferOut\DocDesign\Designes\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\BranchTransferOut\DocDesign\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\BranchTransferOut\DocDesign\Designes\";
                        PDFFilePath = @"Reports\RepxReportDesign\BranchTransferOut\DocDesign\PDFFiles\";
                    }
                }
                else if (RptModuleName == "BranchTranIn")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Branch Transfer In-" + rptName.Split('~')[0];
                    }
                    Module_name = "BRANCHTRANIN";
                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\BranchTransferIn\DocDesign\Designes\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\BranchTransferIn\DocDesign\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\BranchTransferIn\DocDesign\Designes\";
                        PDFFilePath = @"Reports\RepxReportDesign\BranchTransferIn\DocDesign\PDFFiles\";
                    }
                }
                else if (RptModuleName == "Porder")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Purchase Order-" + rptName.Split('~')[0];
                    }
                    Module_name = "PORDER";
                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\PurchaseOrder\DocDesign\Designes\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\PurchaseOrder\DocDesign\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\PurchaseOrder\DocDesign\Designes\";
                        PDFFilePath = @"Reports\RepxReportDesign\PurchaseOrder\DocDesign\PDFFiles\";
                    }
                }

                //Rev Subhra 0019337  23-01-2019
                else if (RptModuleName == "PINDENT")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Purchase Indent-" + rptName.Split('~')[0];
                    }
                    Module_name = "PURCHASEIND";
                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\PurchaseIndent\DocDesign\Designes\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\PurchaseIndent\DocDesign\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\PurchaseIndent\DocDesign\Designes\";
                        PDFFilePath = @"Reports\RepxReportDesign\PurchaseIndent\DocDesign\PDFFiles\";
                    }
                }
                //End of Rev Subhra 0019337  23-01-2019

                else if (RptModuleName == "Sorder")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Sales Order-" + rptName.Split('~')[0];
                    }
                    Module_name = "SORDER";
                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\SalesOrder\DocDesign\Designes\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\SalesOrder\DocDesign\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\SalesOrder\DocDesign\Designes\";
                        PDFFilePath = @"Reports\RepxReportDesign\SalesOrder\DocDesign\PDFFiles\";
                    }
                }
                else if (RptModuleName == "JOURNALVOUCHER")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Journal Voucher-" + rptName.Split('~')[0];
                    }
                    Module_name = "JOURNALVOUCHER";
                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\JournalVoucher\DocDesign\Designes\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\JournalVoucher\DocDesign\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\JournalVoucher\DocDesign\Designes\";
                        PDFFilePath = @"Reports\RepxReportDesign\JournalVoucher\DocDesign\PDFFiles\";
                    }
                }
                else if (RptModuleName == "WHRSSTOCKTRANS")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Warehouse Stock Transfer-" + rptName.Split('~')[0];
                    }
                    Module_name = "WHRSSTOCKTRANS";
                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\WarehoseStockTranfer\DocDesign\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\WarehoseStockTranfer\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\WarehoseStockTranfer\DocDesign\";
                        PDFFilePath = @"Reports\RepxReportDesign\WarehoseStockTranfer\PDFFiles\";
                    }
                }

                else if (RptModuleName == "ManufacturingBOM")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Bill of Materials-" + rptName.Split('~')[0];
                    }
                    Module_name = "MNUBOM";
                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\ManufacturingBOM\DocDesign\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\ManufacturingBOM\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\ManufacturingBOM\DocDesign\";
                        PDFFilePath = @"Reports\RepxReportDesign\ManufacturingBOM\PDFFiles\";
                    }
                }
                else if (RptModuleName == "JobWorkOrder")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Job Work Order-" + rptName.Split('~')[0];
                    }
                    Module_name = "MNUJobWOrder";
                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\JobWorkOrder\DocDesign\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\JobWorkOrder\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\JobWorkOrder\DocDesign\";
                        PDFFilePath = @"Reports\RepxReportDesign\JobWorkOrder\PDFFiles\";
                    }
                }
                else if (RptModuleName == "FGReceived")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "FG Received-" + rptName.Split('~')[0];
                    }
                    Module_name = "FGReceived";
                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\FGReceived\DocDesign\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\FGReceived\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\FGReceived\DocDesign\";
                        PDFFilePath = @"Reports\RepxReportDesign\FGReceived\PDFFiles\";
                    }
                }
                else if (RptModuleName == "MMaterialsIssue")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Materials Issue No.-" + rptName.Split('~')[0];
                    }
                    Module_name = "MMaterialsIssue";
                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\MMaterialsIssue\DocDesign\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\MMaterialsIssue\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\MMaterialsIssue\DocDesign\";
                        PDFFilePath = @"Reports\RepxReportDesign\MMaterialsIssue\PDFFiles\";
                    }
                }
                else if (RptModuleName == "ManufacturingPR")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Production Receipt-" + rptName.Split('~')[0];
                    }
                    Module_name = "MNUBOM";
                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\ManufacturingPR\DocDesign\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\ManufacturingPR\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\ManufacturingPR\DocDesign\";
                        PDFFilePath = @"Reports\RepxReportDesign\ManufacturingPR\PDFFiles\";
                    }
                }
                else if (RptModuleName == "WarehouseStockJrnl")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Warehouse Stock Journal-" + rptName.Split('~')[0];
                    }
                    Module_name = "WHRSESTKJRNL";
                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\WarehoseStockJournal\DocDesign\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\WarehoseStockJournal\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\WarehoseStockJournal\DocDesign\";
                        PDFFilePath = @"Reports\RepxReportDesign\WarehoseStockJournal\PDFFiles\";
                    }
                }
                else if (RptModuleName == "WarehouseStockIN")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Warehouse Stock IN-" + rptName.Split('~')[0];
                    }
                    Module_name = "WarehouseStockIN";
                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\WarehouseStockIN\DocDesign\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\WarehouseStockIN\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\WarehouseStockIN\DocDesign\";
                        PDFFilePath = @"Reports\RepxReportDesign\WarehouseStockIN\PDFFiles\";
                    }
                }
                //Rev Tanmoy 
                else if (RptModuleName == "WarehouseStockOUT")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Warehouse Stock OUT-" + rptName.Split('~')[0];
                    }
                    Module_name = "WarehouseStockOUT";
                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\WarehouseStockOUT\DocDesign\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\WarehouseStockOUT\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\WarehouseStockOUT\DocDesign\";
                        PDFFilePath = @"Reports\RepxReportDesign\WarehouseStockOUT\PDFFiles\";
                    }
                }
                //End of Rev Tanmoy
                //Rev Sayantani
                else if (RptModuleName == "INFLUENCER")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Influencer Payment-" + rptName.Split('~')[0];
                    }
                    Module_name = "INFLNCR";
                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\Influencer\DocDesign\Designes\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\Influencer\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\Influencer\DocDesign\Designes\";
                        PDFFilePath = @"Reports\RepxReportDesign\Influencer\PDFFiles\";
                    }
                }
                //End of Rev Sayantani
                else if (RptModuleName == "CONTRAVOUCHER")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Contra Voucher-" + rptName.Split('~')[0];
                    }
                    Module_name = "CONTRAVOUCHER";
                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\ContraVoucher\DocDesign\Designes\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\ContraVoucher\DocDesign\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\ContraVoucher\DocDesign\Designes\";
                        PDFFilePath = @"Reports\RepxReportDesign\ContraVoucher\DocDesign\PDFFiles\";
                    }
                }
                //Rev Debashis
                else if (RptModuleName == "RECPTCHALLAN")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Receipt Challan_" + rptName.Split('~')[0];
                    }
                    Module_name = "RECPTCHALLAN";
                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\ReceiptChallan\DocDesign\Designes\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\ReceiptChallan\DocDesign\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\ReceiptChallan\DocDesign\Designes\";
                        PDFFilePath = @"Reports\RepxReportDesign\ReceiptChallan\DocDesign\PDFFiles\";
                    }
                }
                //Rev Priti
                else if (RptModuleName == "MoneyReceipt")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Money Receipt_" + rptName.Split('~')[0];
                    }
                    Module_name = "MoneyReceipt";
                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\MoneyReceipt\DocDesign\Designes\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\MoneyReceipt\DocDesign\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\MoneyReceipt\DocDesign\Designes\";
                        PDFFilePath = @"Reports\RepxReportDesign\MoneyReceipt\DocDesign\PDFFiles\";
                    }
                }
                else if (RptModuleName == "WalletRecharge")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Wallet Recharge_" + rptName.Split('~')[0];
                    }
                    Module_name = "WalletRecharge";
                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\WalletRecharge\DocDesign\Designes\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\WalletRecharge\DocDesign\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\WalletRecharge\DocDesign\Designes\";
                        PDFFilePath = @"Reports\RepxReportDesign\WalletRecharge\DocDesign\PDFFiles\";
                    }
                }
                //Rev Priti
                else if (RptModuleName == "ASSIGNJOB")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Assign Job_" + rptName.Split('~')[0];
                    }
                    Module_name = "ASSIGNJOB";
                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\AssignJob\DocDesign\Designes\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\AssignJob\DocDesign\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\AssignJob\DocDesign\Designes\";
                        PDFFilePath = @"Reports\RepxReportDesign\AssignJob\DocDesign\PDFFiles\";
                    }
                }
                else if (RptModuleName == "DELIVERYCHALLAN")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Delivery Challan_" + rptName.Split('~')[0];
                    }
                    Module_name = "DELIVERYCHALLAN";
                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\SRVDeliveryChallan\DocDesign\Designes\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\SRVDeliveryChallan\DocDesign\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\SRVDeliveryChallan\DocDesign\Designes\";
                        PDFFilePath = @"Reports\RepxReportDesign\SRVDeliveryChallan\DocDesign\PDFFiles\";
                    }
                }
                else if (RptModuleName == "SRVJOBSHEET")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "JobSheet_" + rptName.Split('~')[0];
                    }
                    Module_name = "SRVJOBSHEET";
                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\SRVJobSheet\DocDesign\Designes\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\SRVJobSheet\DocDesign\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\SRVJobSheet\DocDesign\Designes\";
                        PDFFilePath = @"Reports\RepxReportDesign\SRVJobSheet\DocDesign\PDFFiles\";
                    }
                }
                //End of Rev Debashis
                //Rev Priti
                else if (RptModuleName == "WarehouseStockIN")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Warehouse Stock IN-" + rptName.Split('~')[0];
                    }
                    Module_name = "WarehouseStockIN";
                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\WarehouseStockIN\DocDesign\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\WarehouseStockIN\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\WarehouseStockIN\DocDesign\";
                        PDFFilePath = @"Reports\RepxReportDesign\WarehouseStockIN\PDFFiles\";
                    }
                }
                //Rev TANMOY
                else if (RptModuleName == "WarehouseStockOUT")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Warehouse Stock OUT-" + rptName.Split('~')[0];
                    }
                    Module_name = "WarehouseStockOUT";
                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\WarehouseStockOUT\DocDesign\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\WarehouseStockOUT\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\WarehouseStockOUT\DocDesign\";
                        PDFFilePath = @"Reports\RepxReportDesign\WarehouseStockOUT\PDFFiles\";
                    }
                }
                else if (RptModuleName == "EstimateCosting")
                {
                    if (string.IsNullOrEmpty(Page.Title) && rptName != null)
                    {
                        Page.Title = "Estimate & Costing-" + rptName.Split('~')[0];
                    }
                    Module_name = "EstimateCosting";
                    if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                    {
                        DesignPath = @"Reports\Reports\RepxReportDesign\EstimateCosting\DocDesign\";
                        PDFFilePath = @"Reports\Reports\RepxReportDesign\EstimateCosting\PDFFiles\";
                    }
                    else
                    {
                        DesignPath = @"Reports\RepxReportDesign\EstimateCosting\DocDesign\";
                        PDFFilePath = @"Reports\RepxReportDesign\EstimateCosting\PDFFiles\";
                    }
                }
                //END OF REV TANMOY
                ExportFileName = Page.Title;
                Session["Module_Name"] = Module_name;
                string fullpath = Server.MapPath("~");
                fullpath = fullpath.Replace("ERP.UI\\", "");
                string DesignFullPath = fullpath + DesignPath;
                string PDFFullPath = fullpath + PDFFilePath;
                filePath = System.IO.Path.GetDirectoryName(DesignFullPath);
                filePath = filePath + "\\" + rptName + ".repx";
                DevExpress.DataAccess.Sql.SqlDataSource sql = GenerateSqlDataSource(RptModuleName);

                XtraReport newXtraReport = XtraReport.FromFile(filePath, true);
                //newXtraReport.ReportPrintOptions.DetailCount = 1;
                newXtraReport.LoadLayout(filePath);
                newXtraReport.DataSource = sql;
                filePathtoPDF = filePath;
                filePathtoPDF = filePathtoPDF.Split('~')[0];
                //filePathtoPDF=filePath.Replace(".repx",".pdf");


                //Update Barcode//

                //
                newXtraReport.DisplayName = ExportFileName;

                //if (RptModuleName == "Invoice" || RptModuleName == "Invoice_POS" || RptModuleName == "PInvoice" || RptModuleName == "ODSDChallan" || RptModuleName == "DChallan" || RptModuleName == "SChallan" || RptModuleName == "PChallan" || RptModuleName == "PDChallan")
                if (RptModuleName == "Invoice" || RptModuleName == "Invoice_POS" || RptModuleName == "Order_POS" || RptModuleName == "Second_Hand" || RptModuleName == "PInvoice" || RptModuleName == "Self_PInvoice" || RptModuleName == "PINV_CUST" || RptModuleName == "PINV_TRANSPORTR" || RptModuleName == "RChallan" || RptModuleName == "PChallan" || RptModuleName == "PDChallan" || RptModuleName == "TSInvoice" || RptModuleName == "CUSTRECPAY" || RptModuleName == "VENDRECPAY")
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
                    else if (PrintType == "4")
                    {
                        ReportType = "Triplicate";
                    }
                    else
                    {
                        ReportType = "Extra_Office";
                    }

                    //PDF file generation has been blocked as per requirment.
                    //if (RptModuleName == "Invoice" || RptModuleName == "PInvoice" || RptModuleName == "PChallan" || RptModuleName == "PDChallan")
                    //{
                    //    filePathtoPDF = filePathtoPDF.Replace("Normal", "PDFFiles");
                    //}
                    //else if (RptModuleName == "TSInvoice")
                    //{
                    //    filePathtoPDF = filePathtoPDF.Replace("Transit", "PDFFiles");
                    //}
                    //else if (RptModuleName == "Invoice_POS")
                    //{
                    //    filePathtoPDF = filePathtoPDF.Replace("SPOS", "PDFFiles");
                    //}
                    //else if (RptModuleName == "Second_Hand")
                    //{
                    //    filePathtoPDF = filePathtoPDF.Replace("Normal", "PDFFiles");
                    //}
                    //else if (RptModuleName == "POS_Duplicate")
                    //{
                    //    filePathtoPDF = filePathtoPDF.Replace("POSDUPLICATE", "PDFFiles");
                    //}
                    //else if (RptModuleName == "RChallan")
                    //{
                    //    filePathtoPDF = filePathtoPDF.Replace("CDelivery", "PDFFiles");
                    //}
                    //PDF file generation has been blocked as per requirment.
                }
                //PDF file generation has been blocked as per requirment.
                //else if (RptModuleName == "Sales_Return" || RptModuleName == "SChallan" || RptModuleName == "Purchase_Return")
                //{
                //    filePathtoPDF = filePathtoPDF.Replace("Normal", "PDFFiles");
                //}
                //else if (RptModuleName == "PURCHASE_RET_REQ")
                //{
                //    filePathtoPDF = filePathtoPDF.Replace("Normal", "PDFFiles");
                //}
                //else if (RptModuleName == "OLDUNTRECVD")
                //{
                //    filePathtoPDF = filePathtoPDF.Replace("Normal", "PDFFiles");
                //}

                //else if (RptModuleName == "Install_Coupon")
                //{
                //    filePathtoPDF = filePathtoPDF.Replace("InstCoupon", "PDFFiles");
                //}
                //else if (RptModuleName == "BranchReq" || RptModuleName == "BranchTranOut" || RptModuleName == "Porder" || RptModuleName == "Sorder" || RptModuleName == "ODSDChallan" ||
                //    RptModuleName == "CBVUCHR" || RptModuleName == "PIQuotation" || RptModuleName == "CUSTDRCRNOTE" || RptModuleName == "VENDDRCRNOTE" || RptModuleName == "JOURNALVOUCHER" ||
                //    RptModuleName == "CONTRAVOUCHER")
                //{
                //    filePathtoPDF = filePathtoPDF.Replace("Designes", "PDFFiles");
                //}
                //PDF file generation has been blocked as per requirment.

                ////else
                ////{
                ////    filePathtoPDF = filePathtoPDF + ".pdf";
                ////}

                //PDF file generation has been blocked as per requirment.
                //if (RptModuleName == "Invoice" || RptModuleName == "Invoice_POS" || RptModuleName == "Second_Hand" || RptModuleName == "PInvoice" || RptModuleName == "ODSDChallan" || 
                //    RptModuleName == "RChallan" || RptModuleName == "SChallan" || RptModuleName == "PChallan" || RptModuleName == "PDChallan" || RptModuleName == "TSInvoice" || 
                //    RptModuleName == "CUSTRECPAY" || RptModuleName == "VENDRECPAY")
                //{
                //    filePathtoPDF = filePathtoPDF + "-" + ReportType + "-" + DocumentID + ".pdf";
                //}            
                //else
                //{
                //    filePathtoPDF = filePathtoPDF + "-" + DocumentID + ".pdf";
                //}
                //PDF file generation has been blocked as per requirment.

                //newXtraReport.ExportToPdf(filePathtoPDF); -- FOR EXPORT PDF FILE
                //if (RptModuleName == "DChallan")
                //{
                //    newXtraReport.Landscape = true;
                //}
                if (RptModuleName == "Invoice_POS" || RptModuleName == "POS_Duplicate" || RptModuleName == "Second_Hand" || RptModuleName == "Order_POS")
                {
                    //Rev 19906: Gate Pass Design reqiured | Amith Marble(According to Bhaskar da)  13-05-2019
                    string str_IsNeedWatermarks = "";
                    str_IsNeedWatermarks = WatermarkDt.Rows[0][0].ToString();
                    if (str_IsNeedWatermarks != "No")
                    {
                        ///End of Rev 
                        if (PrintType == "1")
                        {
                            newXtraReport.Watermark.Text = "ORIGINAL";
                            newXtraReport.Watermark.Font = new Font(newXtraReport.Watermark.Font.FontFamily, 55); //145

                        }
                        if (PrintType == "2")
                        {
                            newXtraReport.Watermark.Text = "TRANSPORTER COPY";
                            newXtraReport.Watermark.Font = new Font(newXtraReport.Watermark.Font.FontFamily, 55); //60
                        }
                        if (PrintType == "3")
                        {
                            newXtraReport.Watermark.Text = "FINANCER COPY";
                            newXtraReport.Watermark.Font = new Font(newXtraReport.Watermark.Font.FontFamily, 55); //73
                        }
                        if (PrintType == "4")
                        {
                            newXtraReport.Watermark.Text = "SUPPLIER COPY";
                            newXtraReport.Watermark.Font = new Font(newXtraReport.Watermark.Font.FontFamily, 55); //75
                        }
                        if (PrintType == "5")
                        {
                            newXtraReport.Watermark.Text = "DUPLICATE";
                            newXtraReport.Watermark.Font = new Font(newXtraReport.Watermark.Font.FontFamily, 55); //75
                        }
                    }
                    newXtraReport.Watermark.TextDirection = DirectionMode.ForwardDiagonal;
                    newXtraReport.Watermark.ForeColor = Color.LightSlateGray;
                    newXtraReport.Watermark.TextTransparency = 180;
                    newXtraReport.Watermark.ShowBehind = false;
                    newXtraReport.Watermark.PageRange = "1-2";
                }
                //Rev Tanmoy
                //Rev Debashis #0023696
                //if (RptModuleName == "MoneyReceipt" || RptModuleName == "WalletRecharge" )
                //{

                //    //string str_IsNeedWatermarks = "";
                //    //str_IsNeedWatermarks = WatermarkDt.Rows[0][0].ToString();
                //    //if (str_IsNeedWatermarks != "No")
                //    //{

                //        if (PrintType == "1")
                //        {
                //            newXtraReport.Watermark.Text = "ORIGINAL";
                //            newXtraReport.Watermark.Font = new Font(newXtraReport.Watermark.Font.FontFamily, 55); //145

                //        }
                //        if (PrintType == "2")
                //        {
                //            newXtraReport.Watermark.Text = "DUPLICATE";
                //            newXtraReport.Watermark.Font = new Font(newXtraReport.Watermark.Font.FontFamily, 55); //75
                //        }

                //    //}
                //    newXtraReport.Watermark.TextDirection = DirectionMode.ForwardDiagonal;
                //    newXtraReport.Watermark.ForeColor = Color.LightSlateGray;
                //    newXtraReport.Watermark.TextTransparency = 180;
                //    newXtraReport.Watermark.ShowBehind = false;
                //    newXtraReport.Watermark.PageRange = "1-2";
                //}
                //End of Rev Debashis
                //End of Rev
                //Rev Debashis

                if (RptModuleName == "Porder" && rptName == "PO-EVAC~D")
                {
                    DataTable dtPOApproveStat = null;
                    string POApproveStat = "";
                    dtPOApproveStat = oDBEngine.GetDataTable("Select ProjectPurchase_ApproveStatus from tbl_trans_PurchaseOrder Where IsProjectOrder=1 and CONVERT(NVARCHAR(MAX),PurchaseOrder_Id)='" + DocumentID + "'");
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
                    dtBRApproveStat = oDBEngine.GetDataTable("Select ERPApproval_Status from tbl_trans_ERPDocApprovalStatus Where ERPApproval_DOCType='BR' and CONVERT(NVARCHAR(MAX),ERPApproval_DocId)='" + DocumentID + "'");
                    if (dtBRApproveStat.Rows.Count > 0)
                    {
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

                ASPxDocumentViewer1.Report = newXtraReport;

                //PDF file generation has been blocked as per requirment.
                //Reportname = Path.GetFileNameWithoutExtension(filePathtoPDF);
                //PDF file generation has been blocked as per requirment.

                //// Create a new attachment and put the PDF report into it.            
                //Attachment att = new Attachment(filePathtoPDF, "application/pdf");            

                //// Create a new message and attach the PDF report to it.
                //MailMessage mail = new MailMessage();
                //mail.Attachments.Add(att);

                //// Specify sender and recipient options for the e-mail message.
                //mail.From = new MailAddress("debashis.talukder@indusnet.co.in", "Debashis");
                //mail.To.Add(new MailAddress("debashis.talukder@indusnet.co.in", "Debashis"));

                //// Specify other e-mail options.
                //mail.Subject = newXtraReport.ExportOptions.Email.Subject;
                //mail.Body = "This is a test e-mail message sent by an application.";

                //// Send the e-mail message via the specified SMTP server.
                //SmtpClient smtp = new SmtpClient("smtp.gmail.com",25);
                //smtp.Send(mail);

                //MemoryStream stream = new MemoryStream();
                ////PdfExportOptions opts = new PdfExportOptions();
                //string fileType = "pdf";
                ////opts.ShowPrintDialogOnOpen = true;
                ////newXtraReport.ExportToPdf(stream,opts);
                //newXtraReport.ExportToPdf(stream);
                //Response.ContentType = "application/" + fileType;
                //Response.BinaryWrite(stream.ToArray());
                //Response.End();

                //using (MemoryStream ms = new MemoryStream())
                //{
                //    XtraReport1 r = new XtraReport1();
                //    r.CreateDocument();
                //    PdfExportOptions opts = new PdfExportOptions();
                //    opts.ShowPrintDialogOnOpen = true;
                //    r.ExportToPdf(ms, opts);
                //    ms.Seek(0, SeekOrigin.Begin);
                //    byte[] report = ms.ToArray();
                //    Page.Response.ContentType = "application/pdf";
                //    Page.Response.Clear();
                //    Page.Response.OutputStream.Write(report, 0, report.Length);
                //    Page.Response.End();
                //}


                //ASPxDocumentViewer1.Report.Print();
                //ASPxDocumentViewer1.Report.PrintDialog();    

            }
            if (!IsPostBack)
            {
                Session["S_UPDATEFORBARCODE"] = 0;
            }
            if (!IsPostBack)
            {
                HDRepornName.Value = Convert.ToString(Request.QueryString["reportname"]);
                //Session["S_UPDATEFORBARCODE"] = 0;
            }
        }

        private DevExpress.DataAccess.Sql.SqlDataSource GenerateSqlDataSource(String RptModuleName)
        {
            // DevExpress.DataAccess.Sql.SqlDataSource result = new DevExpress.DataAccess.Sql.SqlDataSource("crmConnectionString");
            CustomStringConnectionParameters connectionParameters = new CustomStringConnectionParameters(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            DevExpress.DataAccess.Sql.SqlDataSource result = new DevExpress.DataAccess.Sql.SqlDataSource(connectionParameters);
            //BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine();

            // string Module_Name = Convert.ToString(Session["Module_Name"]);
            DataTable dtRptTables = new DataTable();
            string query = "";

            query = @"Select Query_Table_name from tbl_trans_ReportSql where Module_name = '" + Module_name + "' order by Query_ID ";
            dtRptTables = oDbEngine.GetDataTable(query);
            //dtRptTables.TableName = "aaa";
            string CustVendType = "";
            string SalesPurchaseType = "";
            string[] filePaths = new string[] { };
            string path = System.Web.HttpContext.Current.Server.MapPath("~");
            string path1 = path.Replace("Reports\\", "ERP.UI");
            string fullpath = path1.Replace("\\", "/");
            string DocumentID = HttpContext.Current.Request.QueryString["Id"];
            string Doctype = HttpContext.Current.Request.QueryString["doctype"];
            string BranchId = HttpContext.Current.Request.QueryString["Branch"];
            //new added for generate payroll report on 03-12-2018
            string structureid = HttpContext.Current.Request.QueryString["structureid"];
            string employeeid = "";
            if (Session["employeeid_Payslip"] == null)
                employeeid = HttpContext.Current.Request.QueryString["employeeid"];
            else
            {
                // Rev Sanchita
                //employeeid = Convert.ToString(Session["employeeid_Payslip"]);
                DataTable employeeid_Payslip = (DataTable)Session["employeeid_Payslip"];
                DataRow[] dremployeeid_Payslip = employeeid_Payslip.Select("PayStructureCode='" + structureid + "'");
                employeeid = Convert.ToString(dremployeeid_Payslip[0]["EmpList"]);
                // End of Rev Sanchita
            }
            string yymm = HttpContext.Current.Request.QueryString["yymm"];

            //end


            if (RptModuleName == "CUSTRECPAY" || RptModuleName == "CUSTDRCRNOTE")
            {
                CustVendType = "C";
            }
            if (RptModuleName == "VENDRECPAY" || RptModuleName == "VENDDRCRNOTE")
            {
                CustVendType = "V";
            }

            if (RptModuleName == "SChallan" || RptModuleName == "PDChallan" || RptModuleName == "Invoice" || RptModuleName == "Invoice_POS" || RptModuleName == "Second_Hand"
                || RptModuleName == "POS_Duplicate" || RptModuleName == "TSInvoice" || RptModuleName == "Sales_Return" || RptModuleName == "RateDiff_Entry_Cust" || RptModuleName == "Sorder" || RptModuleName == "Order_POS")
            {
                SalesPurchaseType = "S";
            }
            if (RptModuleName == "PChallan" || RptModuleName == "PInvoice" || RptModuleName == "Self_PInvoice" || RptModuleName == "PINV_CUST" || RptModuleName == "PINV_TRANSPORTR" || RptModuleName == "TPInvoice" || RptModuleName == "Purchase_Return" || RptModuleName == "RateDiff_Entry_Vend" || RptModuleName == "PURCHASE_RET_REQ" || RptModuleName == "Porder")
            {
                SalesPurchaseType = "P";
            }

            if (RptModuleName == "Invoice" || RptModuleName == "Invoice_POS" || RptModuleName == "Second_Hand" || RptModuleName == "POS_Duplicate" || RptModuleName == "PInvoice" || RptModuleName == "Self_PInvoice" || RptModuleName == "PINV_CUST")
            {
                string PrintOption = HttpContext.Current.Request.QueryString["PrintOption"];

                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_TAXINVOICE '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + SalesPurchaseType + "','" + PrintOption + "','" + "P" + "'"));
                }
            }
            if (RptModuleName == "PINV_TRANSPORTR")
            {
                string PrintOption = HttpContext.Current.Request.QueryString["PrintOption"];

                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PRC_TRANSPORTERINVOICE_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + SalesPurchaseType + "','" + PrintOption + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "OrderChallan_POS")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_DELIVERYCUMORDERCHALLAN_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + "S" + "','" + "1" + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "Order_POS")
            {
                string PrintOption = HttpContext.Current.Request.QueryString["PrintOption"];

                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_POSORDER_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + SalesPurchaseType + "','" + PrintOption + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "TSInvoice" || RptModuleName == "TPInvoice")
            {
                string PrintOption = HttpContext.Current.Request.QueryString["PrintOption"];

                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_TRANSITSALEPURCHASE_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + SalesPurchaseType + "','" + PrintOption + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "GPAYROLL")
            {
                string PrintOption = HttpContext.Current.Request.QueryString["PrintOption"];

                foreach (DataRow row in dtRptTables.Rows)
                {
                    //result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PRC_GENERATEPAYSLIP '" + Convert.ToString(Session["LastCompany"]) + "','" + "STRUCT0000000001" + "','" + "EMA0000243" + "','" + "1801" + "','" + "" + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + "P" + "'"));
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PRC_GENERATEPAYSLIP '" + Convert.ToString(Session["LastCompany"]) + "','" + structureid + "','" + employeeid + "','" + yymm + "','" + "" + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + "P" + "'"));
                }
            }
            //Rev Subhra 25-04-2019
            else if (RptModuleName == "PAYSLIP")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    //Rev Debashis 0024385
                    //result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_PAYSLIP '" + Convert.ToString(row[0]) + "','" + Convert.ToString(yymm).Trim() + "','" + Convert.ToString(structureid).Trim() + "','" + Convert.ToString(employeeid).Trim() + "'"));
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_PAYSLIP '" + Convert.ToString(Session["LastCompany"]) + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + Convert.ToString(yymm).Trim() + "','" + Convert.ToString(employeeid).Trim() + "'"));
                    //End of Rev Debashis 0024385
                }
            }
            else if (RptModuleName == "PSTICKER")
            {
                string PrintOption = HttpContext.Current.Request.QueryString["PrintOption"];

                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PRC_PRINTPRODUCTS_REPORT '" + DocumentID + "','" + Convert.ToString(row[0]) + "','" + "P" + "'"));
                }
            }
            //End of Rev Subhra 25-04-2019
            else if (RptModuleName == "CUSTDRCRNOTE" || RptModuleName == "VENDDRCRNOTE")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_DEBITCREDITNOTE_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + CustVendType + "','" + "P" + "'"));
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
                string PrintOption = HttpContext.Current.Request.QueryString["PrintOption"];

                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_CUSTVENDRECPAY '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + CustVendType + "','" + PrintOption + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "CBVUCHR")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_CASHBANKVOUCHER_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + "" + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "JOURNALVOUCHER")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_JOURNALVOUCHER_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "WHRSSTOCKTRANS")
            {
                string PrintOption = HttpContext.Current.Request.QueryString["PrintOption"];
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_STOCKTRANSFER_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + PrintOption + "','" + "P" + "'"));
                }
            }
            // Rev Sayanatani
            else if (RptModuleName == "WHRSSTOCKTRANS")
            {
                string PrintOption = HttpContext.Current.Request.QueryString["PrintOption"];
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_STOCKTRANSFER_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + PrintOption + "','" + "P" + "'"));
                }
            }
            // End of Rev Sayantani
            else if (RptModuleName == "INFLUENCER")
            {
                string PrintOption = HttpContext.Current.Request.QueryString["PrintOption"];
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_INFLUENCER_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + PrintOption + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "ManufacturingBOM")
            {
                string PrintOption = HttpContext.Current.Request.QueryString["PrintOption"];
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_MANUFACTURINGBOM '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + PrintOption + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "JobWorkOrder")
            {
                string PrintOption = HttpContext.Current.Request.QueryString["PrintOption"];
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_MANUFACTURINGJobWorkOrder '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + PrintOption + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "FGReceived")
            {
                string PrintOption = HttpContext.Current.Request.QueryString["PrintOption"];
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_MANUFACTURINGFGReceived '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + PrintOption + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "MMaterialsIssue")
            {
                string PrintOption = HttpContext.Current.Request.QueryString["PrintOption"];
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_MANUFACTURINGMMatialsIssue '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + PrintOption + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "WarehouseStockJrnl")
            {
                string PrintOption = HttpContext.Current.Request.QueryString["PrintOption"];
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_WAREHOUSE_WISE_STOCK_JOURNAL '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + PrintOption + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "WarehouseStockIN")
            {
                string PrintOption = HttpContext.Current.Request.QueryString["PrintOption"];
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC  PROC_WAREHOUSE_WISE_STOCK_IN '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + PrintOption + "','" + "P" + "'"));
                }
            }
            //REV TANMOY
            else if (RptModuleName == "WarehouseStockOUT")
            {
                string PrintOption = HttpContext.Current.Request.QueryString["PrintOption"];
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC  PROC_WAREHOUSE_WISE_STOCK_OUT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + PrintOption + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "EstimateCosting")
            {
                string PrintOption = HttpContext.Current.Request.QueryString["PrintOption"];
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_ESTIMATECOSTING_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + PrintOption + "','" + "P" + "'"));
                }
            }
            //END OF REV TANMOY
            else if (RptModuleName == "CONTRAVOUCHER")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_CONTRAVOUCHER_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "RChallan")
            {
                string PrintOption = HttpContext.Current.Request.QueryString["PrintOption"];

                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_DELIVERYCHALLAN '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + PrintOption + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "SChallan" || RptModuleName == "PDChallan" || RptModuleName == "PChallan")
            {
                string PrintOption = HttpContext.Current.Request.QueryString["PrintOption"];

                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_SALEPURCHASECHALLAN_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + SalesPurchaseType + "','" + PrintOption + "','" + "P" + "'"));
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
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_INSTALLATIONCOUPON_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + fullpath + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "BranchReq")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_BRANCHREQ_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "BranchTranOut")
            {
                //Rev Debashis
                string PrintOption = HttpContext.Current.Request.QueryString["PrintOption"];
                //End of Rev Debashis
                foreach (DataRow row in dtRptTables.Rows)
                {
                    //Rev Debashis
                    //result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_BRANCHOUT_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + "P" + "'"));
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_BRANCHOUT_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + PrintOption + "','" + "P" + "'"));
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
            else if (RptModuleName == "Sorder" || RptModuleName == "Porder")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_SALEPURCHASEORDER_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + SalesPurchaseType + "','" + "P" + "'"));
                }
            }
            //Rev Subhra 0019337  23-01-2019
            else if (RptModuleName == "PINDENT")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_PURCHASEINDENT_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + "" + "','" + "P" + "'"));
                }
            }
            //End of Rev Subhra 0019337  23-01-2019
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
            else if (RptModuleName == "BRCODE")
            {
                //if (!IsPostBack)
                //{
                DataTable dtListBarcode = new DataTable();
                ProcedureExecute proclistbarcode = new ProcedureExecute("PROC_LISTBARCODE");
                proclistbarcode.AddVarcharPara("@DOCID_N_PRODID", 500, Convert.ToString(DocumentID));
                proclistbarcode.AddVarcharPara("@DOC_TYPE", 3000, Convert.ToString(Doctype));
                proclistbarcode.AddVarcharPara("@BRANCH_ID", 10, Convert.ToString(BranchId));
                dtListBarcode = proclistbarcode.GetTable();
                Session["ListOfBarcode"] = dtListBarcode.Rows[0][0].ToString();

                //============================Update isprint=1==================================
                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("PROC_UPDATE_BARCODEPRINT");
                proc.AddVarcharPara("@DOCID_N_PRODID", 500, Convert.ToString(DocumentID));
                proc.AddVarcharPara("@DOC_TYPE", 3000, Convert.ToString(Doctype));
                //proc.AddIntegerPara("@IS_UPD_BARCODE", Convert.ToInt32(Session["S_UPDATEFORBARCODE"]));
                proc.AddVarcharPara("@BRANCH_ID", 10, Convert.ToString(BranchId));
                dt = proc.GetTable();
                //====================================================================
                //}


                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_BARCODE_PRINT '" + Doctype + "','" + Convert.ToString(row[0]) + "','" + "P" + "','" + Session["ListOfBarcode"].ToString() + "'"));
                }
            }
            //Rev Debashis
            else if (RptModuleName == "RECPTCHALLAN")
            {

                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PRC_RECEIPTCHALLANPRINT_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "MoneyReceipt")
            {
                string PrintOption = HttpContext.Current.Request.QueryString["PrintOption"];
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PRC_MoneyReceipt_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + PrintOption + "','" + Convert.ToString(Session["UserID"]).Trim() + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "WalletRecharge")
            {
                string PrintOption = HttpContext.Current.Request.QueryString["PrintOption"];
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PRC_WalletRecharge_REPORT '" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + PrintOption + "','" + Convert.ToString(Session["UserID"]).Trim() + "','" + "P" + "'"));
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

            DataTable dtRptRelation = new DataTable();
            string RelationQuery = "";
            RelationQuery = @"Select Parent_Query_name,Child_Query_name, Parent_Column_name,Child_Column_name from tbl_trans_ReportTableRelation where Module_name = '" + Module_name + "' order by Query_ID ";
            dtRptRelation = oDbEngine.GetDataTable(RelationQuery);
            if (dtRptRelation.Rows.Count > 0)
            {
                foreach (DataRow row in dtRptRelation.Rows)
                {
                    result.Relations.Add(Convert.ToString(row[0]), Convert.ToString(row[1]), Convert.ToString(row[2]), Convert.ToString(row[3]));
                }
            }

            result.RebuildResultSchema();
            result.Connection.Close();
            return result;
        }

        [WebMethod]
        public static string GetFromEmail()
        {
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            DataTable dtFromEmail = new DataTable();
            string FromEmailDesc = "";
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
                mail.Body = Body;
                //smtp.Host = "smtp.gmail.com";
                //smtp.Port = 587;
                smtp.Host = OutgoingSMTPHost.Trim();
                smtp.Port = Convert.ToInt32(OutgoingPort);
                smtp.Credentials = new System.Net.NetworkCredential(FromAdd, Password);
                smtp.EnableSsl = true;
                smtp.Send(mail);
                att.Dispose();
                smtp.Dispose();
                mail.Dispose();
                // Close the memory stream.
                mem.Close();
            }
            catch (Exception ex)
            {
                ASPxDocumentViewer1.JSProperties["cpErrorResult"] = ex.Message;
            }
        }
    }
}