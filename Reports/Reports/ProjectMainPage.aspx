<%@ Page Title="Welcome to ERP Reports" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master"  AutoEventWireup="true" CodeBehind="ProjectMainPage.aspx.cs" Inherits="Reports.Reports.ProjectMainPage" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h3 class="pull-left"><span class="">
        <asp:Label ID="lblHeading" runat="server" Text="Welcome to ERP Reports."></asp:Label></span>
    </h3>
     <div class="clear"></div>
    <ul>        
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=StockTrialSumm">Stock Trial-Summary</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=StockTrialDet">Stock Trial-Details</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=StockTrialProd">Stock Trial-Product</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=StockTrialWH">Stock Trial-Warehouse</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=StockTrial1">Stock Trial New</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=Install_Coupon">Installation Coupons</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=Invoice">Sales Invoice(GST)</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=TSInvoice">Transit Sales Invoice</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=PInvoice">Purchase Invoice</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=TPInvoice">Transit Purchase Invoice</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=Proforma">Sales Proforma</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=LedgerPost">Ledger Posting</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=CashBook">Cash Book</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=BankBook">Bank Book</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=Invoice_POS">Sales invoice(POS)</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=POS_Duplicate">Sales invoice(POS Duplicate)</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=Porder">Purchase Order</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=Sorder">Sales Order</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=BranchReq">Branch Requisition</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=BranchTranIn">Branch Transfer In</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=BranchTranOut">Branch Transfer Out</a></li>    
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=Sales_Return">Sales Return</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=Purchase_Return">Purchase Return</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=PChallan">Purchase Challan</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=SChallan">Sales Challan</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=ODSDChallan">Delivery Challan(ODSD)</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=RChallan">Road Challan</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=PDChallan">Pending Delivery Challan</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxSetDefaultDesign.aspx?modulename=Design">Select Design</a></li>    
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=CUSTRECPAY">Customer Receipt Payment</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=VENDRECPAY">Vendor Receipt Payment</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=CBVUCHR">Cash/bank Voucher</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=Second_Hand">Second Hand Sales</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=PIQuotation">Proforma Invoice/Quotation</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=CUSTDRCRNOTE">Customer Debit/Credit Note</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=VENDDRCRNOTE">Vendor Debit/Credit Note</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=CONTRAVOUCHER">Contra Voucher</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=OLDUNTRECVD">Old Unit Received</a></li>
 <%--   <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=CONTRAVOUCHER">Contra Voucher</a></li>--%>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=JOURNALVOUCHER">Journal Voucher</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=ImpPorder">Import Purchase Order</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/CardBankReport.aspx">Card/Bank Book Reports</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/CashBankReport.aspx">Cash/Bank Book Reports</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/CashBankReport_NEW.aspx">New Cash/Bank Reports</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/DebtorsDetailsDR.aspx">Debtors Details -- Debit</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/DebtorsDetailsCR.aspx">Debtors Details -- Credit</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/Creditors-Details.aspx">Creditors Details</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/FinalReport.aspx">Final Report</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/ExportDetailsReport.aspx">Export Details Report</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/Gstr-1Report.aspx">Gstr-1 Report</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/CombineStockTrial.aspx">Combined Stock Trial Report</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/Consolidated-Stock.aspx">Consolidated Stock Report</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=PURCHASE_RET_REQ">Purchase Return Request</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/CombinedSalePurchaseReport.aspx">Combined Sale/Purchase Report</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/SalaryDisbursment.aspx">Salary Disbursement</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/Insurance-Register.aspx">Insurance Register</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/Gstr2Report.aspx">Gstr-2 Report</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/LedgerPostingReport.aspx">Ledger Posting Detail</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/frm_saleouttaxreg.aspx">Output Tax Register</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/frm_purintaxreg.aspx">Input Tax Register</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/JsonParse-Gstr1.aspx">Json GSTR1 Reconcilation</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/JsonParse.aspx">Json GSTR2 Reconcilation</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/GeneralTrialReport.aspx">Trial Balance</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/Customer_Ledger.aspx">Customer Ledger</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/Vendor_Ledger.aspx">Vendor Ledger</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/CustomerOutstanding.aspx">Outstanding Report-Invoice Wise</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/PartyOutstanding.aspx">Party Outstanding</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/Demo_Register.aspx">Demo Register</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/FinanceCNReport.aspx">Finance Register</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/GSTR_Customerpaymentreceipt.aspx">GSTR Customerpaymentreceipt Register</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/GSTR_HSNcodeRate.aspx">GSTR HSNcodeRate Register</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/GSTR_PurchaseRegister.aspx">GSTR Purchase Register</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/GSTR_SaleRegister.aspx">GSTR Sales Register</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/GSTR_Vendorpaymetreceipt.aspx">GSTR Vendorpaymetreceipt Register</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/PartyLedgerPostingReport.aspx">Consolidated Party Ledger</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/PurchaseRegister.aspx">Purchase Register Summary</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/SalesRegisterReport.aspx">Sales Register Summary</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/SalesAnalysis.aspx">Sales Analysis</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/SerialwiseDetails.aspx">Serialwise Details</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/Stock_Ageing.aspx">Stock Ageing</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/StockTrialSummary.aspx">Stock Trial Summary</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/StockValuation.aspx">Stock Valuation</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/GST_RCMStatement.aspx">RCM Statement</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/TDS_Report.aspx">TDS Report</a></li>    
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/BranchWisePartyOutstanding.aspx">Branch Wise Party Outstanding</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/PurchaseRegister_Details.aspx">Purchase Invoice Register Details</a></li>    
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/SalesRegister_Details.aspx">Sales Invoice Register Details</a></li>    
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/SalesReturnRegister_Details.aspx">Sales Return Register Details</a></li>    
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/PurchaseReturnRegister_Details.aspx">Purchase Return Register Details</a></li> 
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/PurchaseOrderRegister_Details.aspx">Purchase Order Register Details</a></li>   
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/SalesOrderRegister_Details.aspx">Sale Order Register Details</a></li>   
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/CashBankBookRegister_Details.aspx">Cash/Bank Register Details</a></li>   
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/GeneralLedgerRegister.aspx">General Ledger Register</a></li>    
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/CustomerRecPayRegister.aspx">Customer Receipt/Payment Register Report</a></li>    
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/VendorRecPayRegister.aspx">Vendor Receipt/Payment Register Report</a></li>    
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/PaymentCollection_Details.aspx">Payment Collection Report</a></li>       
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/CashBankBookSummary.aspx">Cash Bank Book Summary</a></li>    
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/DayBook.aspx">Day Book Report</a></li>    
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/JournalRegister_Details.aspx">Journal Register Details</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/LedgerBook.aspx">Ledger Book</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/SubLedgerDetails.aspx">Sub Ledger Details</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/ProfitandLossStatement.aspx">Profit & Loss Statement (Group wise)</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/ProfitandLossStatement_Tabular.aspx">Profit & Loss Statement (Tabular)</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=BRCODE">Barcode Printing</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=Order_POS">Sales Order(POS)</a></li>  
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=OrderChallan_POS">Order Challan(POS)</a></li>  
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=RateDiff_Entry_Cust">Rate Difference Entry Customer</a></li>  
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=RateDiff_Entry_Vend">Rate Difference Entry Vendor</a></li> 
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=PINV_CUST">Purchase Invoice For Customer</a></li> 
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=PINV_TRANSPORTR">Purchase Invoice For Transporter</a></li> 
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/Transproter_Ledger.aspx">Transporter Ledger</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/POSOrderRegister.aspx">POS Order Register</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=Self_PInvoice">Self Purchase Invoice</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/POSOrderOldUnitRegister.aspx">POS Order Old Unit Register</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/CustomerWiseMonthlySales.aspx">Customer Wise Monthly Sales</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/StockValuationWithAgeing.aspx">Stock Valuation with Ageing</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/CustomerOutstandingDetail.aspx">Customer Outstanding-Detail</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/CustomerOutstandingSummary.aspx">Customer Outstanding-Summary</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/CustomerAgeingDetail.aspx">Customer Ageing-Detail</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/CustomerAgeingSummary.aspx">Customer Ageing-Summary</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/DSR-Summary.aspx">DSR Summary</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=AdvCustCr">Credit Note With Invoice</a></li>  
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=AdvCustDr">Advance with Debit Note</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/SubLedgerWisePosting.aspx">Sub Ledger Wise Posting - Detail</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/SubLedgerWisePosting_Summary.aspx">Sub Ledger Wise Posting - Summary</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/ScheduleTaskStatus.aspx">Schedule Task Status</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/VendorOutstandingDetail.aspx">Vendor Outstanding-Detail</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/VendorOutstandingSummary.aspx">Vendor Outstanding-Summary</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/VendorAgeingDetail.aspx">Vendor Ageing-Detail</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/VendorAgeingSummary.aspx">Vendor Ageing-Summary</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/ListingOfMaster_Customer.aspx">Listing Of Master-Customer</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/ListingOfMaster_Vendor.aspx">Listing Of Master-Vendor</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/ListingOfMaster_Transporter.aspx">Listing Of Master-Transporter</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/ListingOfMaster_Product.aspx">Listing Of Master-Product</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/BTIBTO_Register.aspx">BTI/BTO Register</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/PendingSaleOrderRegister.aspx">Pending Sale Order Register</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/PendingPurchaseOrderRegister.aspx">Pending Purchase Order Register</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/InterestCalculation.aspx">Interest Calculation</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/AdjustmentDetail_Customer.aspx">Adjustment Listing - Customers</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/AdjustmentDetail_Vendor.aspx">Adjustment Listing - Vendors</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/SubLedgerSummary.aspx">Sub Ledger Summary</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/BranchWiseSalesAnalysis.aspx">Branch wise Sales Analysis</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/BSScheduleVI.aspx">Balance Sheet Schedule VI</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/BSGroupWise.aspx">Balance Sheet (Group wise)</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/StockLedger.aspx">Stock Ledger - Detail</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/StockLedgerSummary.aspx">Stock Ledger - Summary</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/AuditTrial_Report.aspx">Audit Trial Report</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/CustomerOutstandingVSAgeingAnalysis.aspx">Customer Outstanding Vs. Ageing Analysis</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/EWayBillRegister.aspx">Eway-Bill Register Report</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/CustomerAgeingWithDaysInterval.aspx">Customer Ageing With Days Interval</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/AssignedSalesmanDetails.aspx">Assigned Salesman Details</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/InventoryControlWithStockValuation.aspx">Inventory Control With Stock Valuation</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=GPAYROLL">Generate Payroll</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=PINDENT">Purchase Indent</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/PartyLedger_Customer.aspx">Party Ledger - Customer</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/PartyLedger_Vendor.aspx">Party Ledger - Vendor</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/CRMSummary_Performance_F.aspx">CRM Summary</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/ProductWiseSalesReport.aspx">Product Wise Sales</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/Activity_Conversion.aspx">Activity & Conversion</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/MultiCompanyStockSummary.aspx">Multi Company Stock Summary</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/MismatchDetection.aspx">Mismatch Detection</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/BankReconciliationStatement.aspx">Bank Reconciliation Statement</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/CustomerAnalysis.aspx">Customer Analysis</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/VendorAnalysis.aspx">Vendor Analysis</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/TrialBalanceGroupWise.aspx">Trial Balance Group Wise</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=PSTICKER">Product Sticker</a></li>   
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/LeadActivity.aspx">Lead Activity</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/TDSeFile.aspx">TDSeFile</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=WarehouseStockTrans">Stock Transfer</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/SalesmanPerformance.aspx">Salesman Performance</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/InfluencerLedger.aspx">Influencer Ledger</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=ManufacturingBOM">Bill of Materials (BOM)</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/OrderToDelivery.aspx">Order To Delivery</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=WarehouseStockJrnl">Warehouse Wise Stock Journal</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=ManufacturingPR">Production Receipt</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/PartyLedger.aspx">Party Ledger - All</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/CashBankBookDetail_Subledgerwise.aspx">Subledger wise Cash/Bank Book - Detail</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=INFLUENCER">Influencer</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/InfluencerScheme.aspx">Influencer Scheme</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/CustomerRecPayRegister_Detail.aspx">Customer Payment/Receipt Register - Detail</a></li> 
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/VendorRecPayRegister_Detail.aspx">Vendor Payment/Receipt Register - Detail</a></li> 
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/PendingProjectSORegister_Details.aspx">Project - Pending Sale Order Register</a></li> 
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/PendingProjectPORegister_Details.aspx">Project - Pending Purchase Order Register</a></li> 
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/BatchwiseBalance_Detail.aspx">Batch wise Balance - Detail</a></li> 
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/BatchwiseBalance_Summary.aspx">Batch wise Balance - Summary</a></li> 
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/ProjectSummary.aspx">Project Summary</a></li> 
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/WarehousewiseStockValuation.aspx">Warehouse wise Stock Valuation</a></li> 
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/TrialOnNetBalance.aspx">Trial On Net Balance</a></li> 
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=TRIALONNETBALSUMARY">Trial On Net Balance(Print)</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=BRSSTATEMENT">BRS Statement</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/SalesProforma_QuotationRegister.aspx">Sales Proforma/Quotation Register</a></li> 
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/ABCAnalysis.aspx">ABC Analysis</a></li> 
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/WarehousewiseStockStatus.aspx">Warehouse wise Stock Status</a></li> 
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/CustomerWiseSales.aspx">Customer Wise Sales</a></li> 
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/VendorWisePurchase.aspx">Vendor Wise Purchase</a></li> 
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/StockDayBook.aspx">Stock Day Book</a></li> 
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/StockMovement.aspx">Stock Movement</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/ModuleWiseDocumentCount.aspx">Module Wise Document Count--Susanta</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/ModuleWiseDocCount.aspx">Module Wise Document Count</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/VendorAgeingWithDaysInterval.aspx">Vendor Ageing With Days Interval</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/ProjectWisePaymentCollection.aspx">Project Wise - Collection/Payment</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/ProjectMasterRegister.aspx">Project Master Register</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=RECPTCHALLAN">Receipt/Challan Print</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=ASSIGNJOB">Assign Job Print</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=DELIVERYCHALLAN">Delivery Challan Print</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=SRVJOBSHEET">Job Sheet Print</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/SRVMaterialInOutRegisterSummary.aspx">Components In-Out Register - Summary</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/SRVMaterialInOutRegisterDetail.aspx">Components In-Out Register - Details</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/SRVWarehouseWiseStockStatusSummary.aspx">SRVWarehouse Wise Stock Status - Summary</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/SRVWarehouseWiseStockStatusDetail.aspx">SRVWarehouse Wise Stock Status - Details</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/BranchRequisitionStatus.aspx">Branch Requisition Status</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/TCSRegister.aspx">TCS Register</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=WarehouseStockIN">Warehouse Wise Stock IN</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=WarehouseStockOUT">Warehouse Wise Stock OUT</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/BranchWHWiseDetail.aspx">Branch/Warehouse Wise Stock - Detail</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/BranchWHWiseSummary.aspx">Branch/Warehouse Wise Stock - Summary</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/SRVPerformanceStatusDetails.aspx">TAT - Details</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/SRVPerformanceStatusDayWise.aspx">TAT - Day(s) Wise</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/SRVPerformanceStatusHourWise.aspx">TAT - Hour(s) Wise</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=EstimateCosting">Estimate & Costing</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/EstimateRegisterSummary.aspx">Estimate Register - Summary</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/EstimateRegisterDetails.aspx">Estimate Register - Details</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/LocationWiseSellableStock.aspx">Location Wise - Stock In Hand</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/PendingEstimateRegisterSummary.aspx">Pending Estimate Register - Summary</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/PendingEstimateRegisterDetails.aspx">Pending Estimate Register - Details</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/SalesInquiryRegister.aspx">Sales Inquiry Register</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/PurchaseIndentRegister.aspx">Purchase Indent Register</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/PurchaseQuotationRegister.aspx">Purchase Quotation Register</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/SRVTechnicianProductivity.aspx">Technician Productivity</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/ComponentRegister.aspx">Component Register</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=MoneyReceipt">Money Receipt Print</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=WalletRecharge">Wallet Recharge Print</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/PurchaseGRNRegister_Details.aspx">Purchase GRN Register - Detail</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/BOMRegisterDetails.aspx">BOM Register - Detail</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/ProfitabilitySales.aspx">Profitability Sales</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=PAYSLIP">Pay Slip</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/WorkOrderRegisterDetails.aspx">Work Order Register - Detail</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/MFIssueRegisterDetails.aspx">Manufacturing Issue Register - Detail</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/MFFGRegisterDetails.aspx">Finished Goods Register - Detail</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/PendingWORegisterDetails.aspx">Pending Work Order Register - Detail</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=JobWorkOrder">Job Work Order</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=MMaterialsIssue">Materials Issue</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=FGReceived">FG Received</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/IssueForProductionRegister.aspx">Issue for Production Register - Detail</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/StockPosition.aspx">Stock Position</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=BRSCONSSTATEMENT">BRS Statement - Consolidated</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/PartyLedgerCustomerWithSegment.aspx">Party Ledger - Customer with Segment</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=STATEMENTOFACCOUNT">Statement of Account(Design)</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/StatementOfAccount.aspx">Statement of Account</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/DSROnOpportunities.aspx">DSR On Opportunities</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/ProjectAccounting.aspx">Project Accounting</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/ProjectWiseStockValuation.aspx">Project Wise Stock Valuation</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/ViewLeaveRegister.aspx">View Leave Register</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/DeliverySchedule.aspx">Delivery Schedule</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/BranchWiseTDSeFile.aspx">Branch wise TDSeFile Generation</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/Stbregister.aspx">STB Register</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/MFStockReceiptRegister.aspx">Stock Receipt Register</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/PhysicalStockVsStockAvailability.aspx">Physical Stock Vs Stock Availability</a></li>
    <%--mantise issue:0025139--%>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/REPXReports/RepxReportMain.aspx?reportname=SalesInquiry">Sales Inquiry</a></li>
    <%--End of mantise issue:0025139--%>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/PendingSalesOrderColumnar.aspx">Pending Sales Order Register - Columnar</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/PendingSalesQuotationColumnar.aspx">Pending Sales Quotation Register - Columnar</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/SRVServiceRegisterReport.aspx">Service Register</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Reports/GridReports/WHWiseStockOutDetail.aspx">Warehouse Wise Stock-OUT - Details</a></li>
</ul>


</asp:Content>
