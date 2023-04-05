//==========================================================Revision History ============================================================================================
//    1.0   Priti   V2.0.36     10-01-2023     0025324: Views to be converted to Procedures in the Listing Page of Transaction / Transit Sales/Purchase / Sales Invoice
//    2.0   Priti   V2.0.36     17-02-2023     After Listing view upgradation delete data show in listing issue solved.

//========================================== End Revision History =======================================================================================================--%>

var isFirstTime = true;

// Purchase Invoice Section Start
updateTSIGridByDate = function () {
    var sdate = cFormDate.GetValue();
    var edate = ctoDate.GetValue();
    var startDate = new Date(sdate);
    var endDate = new Date(edate);
    if (cFormDate.GetDate() == null) {
        jAlert('Please select from date.', 'Alert', function () { cFormDate.Focus(); });
    }
    else if (ctoDate.GetDate() == null) {
        jAlert('Please select to date.', 'Alert', function () { ctoDate.Focus(); });
    }
    else if (ccmbBranchfilter.GetValue() == null) {
        jAlert('Please select Branch.', 'Alert', function () { ccmbBranchfilter.Focus(); });
    }
    else if (startDate > edate) {
        jAlert('From date can not be greater than To Date.', 'Alert', function () { cFormDate.Focus(); });
    }
    else {

        localStorage.setItem("TSIFromDate", cFormDate.GetDate().format('yyyy-MM-dd'));
        localStorage.setItem("TSIToDate", ctoDate.GetDate().format('yyyy-MM-dd'));
        localStorage.setItem("TSIBranch", ccmbBranchfilter.GetValue());

        //cdownpaygrid.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue());

        $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
        $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
        $("#hfBranchID").val(ccmbBranchfilter.GetValue());
        $("#hfIsFilter").val("Y");
        //rev 1.0
        //cgrid.Refresh();
        $("#hFilterType").val("All");
        cCallbackPanel.PerformCallback("");
        //end rev 1.0
        $("#drdExport").val(0);
    }
}
  //rev 1.0
function CallbackPanelEndCall(s, e) {
    cgrid.Refresh();
}
 //end rev 1.0
updateGridAfterDelete = function () {
    var sdate = cFormDate.GetValue();
    var edate = ctoDate.GetValue();
    var startDate = new Date(sdate);
    var endDate = new Date(edate);
    if (cFormDate.GetDate() == null) {
        jAlert('Please select from date.', 'Alert', function () { cFormDate.Focus(); });
    }
    else if (ctoDate.GetDate() == null) {
        jAlert('Please select to date.', 'Alert', function () { ctoDate.Focus(); });
    }
    else if (ccmbBranchfilter.GetValue() == null) {
        jAlert('Please select Branch.', 'Alert', function () { ccmbBranchfilter.Focus(); });
    }
    else if (startDate > edate) {
        jAlert('From date can not be greater than To Date.', 'Alert', function () { cFormDate.Focus(); });
    }
    else {

        localStorage.setItem("TSIFromDate", cFormDate.GetDate().format('yyyy-MM-dd'));
        localStorage.setItem("TSIToDate", ctoDate.GetDate().format('yyyy-MM-dd'));
        localStorage.setItem("TSIBranch", ccmbBranchfilter.GetValue());
        $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
        $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
        $("#hfBranchID").val(ccmbBranchfilter.GetValue());
        $("#hfIsFilter").val("Y");

        //rev 2.0
        cgrid.Refresh();
        //$("#hFilterType").val("All");
        //cCallbackPanel.PerformCallback("");
        //end rev 2.0

        $("#drdExport").val(0);

        //cdownpaygrid.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue());
        //$("#drdExport").val(0);
    }
}
AllControlInitilize = function (s, e) {
    if (isFirstTime) {
        if (localStorage.getItem('TSIFromDate')) {
            var fromdatearray = localStorage.getItem('TSIFromDate').split('-');
            var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
            cFormDate.SetDate(fromdate);
        }

        if (localStorage.getItem('TSIToDate')) {
            var todatearray = localStorage.getItem('TSIToDate').split('-');
            var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
            ctoDate.SetDate(todate);
        }

        if (localStorage.getItem('TSIBranch')) {
            if (ccmbBranchfilter.FindItemByValue(localStorage.getItem('TSIBranch'))) {
                ccmbBranchfilter.SetValue(localStorage.getItem('TSIBranch'));
            }
        }
    }
}

//Purchase Invoice Section End

//updateGridByDate = function () {
//    if (cFormDate.GetDate() == null) {
//        jAlert('Please select from date.', 'Alert', function () { cFormDate.Focus(); });
//    }
//    else if (ctoDate.GetDate() == null) {
//        jAlert('Please select to date.', 'Alert', function () { ctoDate.Focus(); });
//    }
//    else if (ccmbBranchfilter.GetValue() == null) {
//        jAlert('Please select Branch.', 'Alert', function () { ccmbBranchfilter.Focus(); });
//    }
//    else {

//        localStorage.setItem("TSIFromDate", cFormDate.GetDate().format('yyyy-MM-dd'));
//        localStorage.setItem("TSIToDate", ctoDate.GetDate().format('yyyy-MM-dd'));
//        localStorage.setItem("TSIBranch", ccmbBranchfilter.GetValue());

//        //cdownpaygrid.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue());

//        $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
//        $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
//        $("#hfBranchID").val(ccmbBranchfilter.GetValue());
//        $("#hfIsFilter").val("Y");

//        cdownpaygrid.Refresh();

//        $("#drdExport").val(0);
//    }
//}



//updateGridAfterDelete = function () {
//    if (cFormDate.GetDate() == null) {
//        jAlert('Please select from date.', 'Alert', function () { cFormDate.Focus(); });
//    }
//    else if (ctoDate.GetDate() == null) {
//        jAlert('Please select to date.', 'Alert', function () { ctoDate.Focus(); });
//    }
//    else if (ccmbBranchfilter.GetValue() == null) {
//        jAlert('Please select Branch.', 'Alert', function () { ccmbBranchfilter.Focus(); });
//    }
//    else {

//        localStorage.setItem("TSIFromDate", cFormDate.GetDate().format('yyyy-MM-dd'));
//        localStorage.setItem("TSIToDate", ctoDate.GetDate().format('yyyy-MM-dd'));
//        localStorage.setItem("TSIBranch", ccmbBranchfilter.GetValue());

//        cdownpaygrid.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue());
//        $("#drdExport").val(0);
//    }
//}

//AllControlInitilize = function (s, e) {
//    if (isFirstTime) {
//        if (localStorage.getItem('TSIFromDate')) {
//            var fromdatearray = localStorage.getItem('TSIFromDate').split('-');
//            var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
//            cFormDate.SetDate(fromdate);
//        }

//        if (localStorage.getItem('TSIToDate')) {
//            var todatearray = localStorage.getItem('TSIToDate').split('-');
//            var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
//            ctoDate.SetDate(todate);
//        }

//        if (localStorage.getItem('TSIBranch')) {
//            if (ccmbBranchfilter.FindItemByValue(localStorage.getItem('TSIBranch'))) {
//                ccmbBranchfilter.SetValue(localStorage.getItem('TSIBranch'));
//            }
//        }
//    }
//}

//SaveOppening = function () {
//    cdownpaygrid.PerformCallback('SaveDownPayment');
//}

//Exit = function () {
//    $('#hdfID').val("");
//    $('#hdfInvoiceID').val("");
//    cfinancePopup.Hide();
//}

//OnMoreInfoClick = function (VisibleIndex) {
//    LoadingPanel.Show();
//    $('#hdfID').val("");
//    $('#hdfInvoiceID').val("");
//    cCallbackPanel.PerformCallback('Edit~' + VisibleIndex);
//    $("#drdExport").val(0);
//}

//OnClickDelete = function (keyValue) {
//    cdownpaygrid.PerformCallback('Delete~' + keyValue);
//}

//CalculateAmount = function () {
//    var DPAmount = ctxtDownPay1.GetValue();
//    var AdjAmount = ctxtDownPay2.GetValue();
//    var DivestmentAmt1 = ctxtDivestmentAmt1.GetValue();
//    var DivestmentAmt2 = ctxtDivestmentAmt2.GetValue();
//    var DivestmentAmt3 = ctxtDivestmentAmt3.GetValue();
//    var DbdAmount = ctxtDbdAmount.GetValue();
//    var MbdAmount = ctxtMbdAmount.GetValue();
//    var ProcessingFee = ctxtProcessingFee.GetValue();
//    var InvoiceAmount = ctxtBillAmount.GetValue();

//    var TotalPay = parseFloat(DPAmount) + parseFloat(AdjAmount) + parseFloat(DivestmentAmt1) + parseFloat(DivestmentAmt2) + parseFloat(DivestmentAmt3) + parseFloat(DbdAmount) + parseFloat(ProcessingFee) + parseFloat(MbdAmount);
//    var Balance = parseFloat(InvoiceAmount) - parseFloat(TotalPay);

//    ctxtTotalPay.SetValue(TotalPay);
//    ctxtbalance.SetValue(Balance);

//    if (InvoiceAmount == TotalPay) {
//        ccmbStatus.SetValue("C");
//    }
//    else if (InvoiceAmount > TotalPay) {
//        if ((InvoiceAmount - TotalPay) <= 999) {
//            ccmbStatus.SetValue("S");
//        }
//        else if ((InvoiceAmount - TotalPay) > 999) {
//            ccmbStatus.SetValue("O");
//        }
//    }
//    else if (InvoiceAmount < TotalPay) {
//        ccmbStatus.SetValue("E");
//    }
//}

//DbdPercentageCalculate = function () {
//    var InvoiceAmount = ctxtBillAmount.GetValue();
//    var DbdPercentage = ctxtDbdPercentage.GetValue();
//    var DbdAmount = (parseFloat(InvoiceAmount) * parseFloat(DbdPercentage)) / 100;

//    ctxtDbdAmount.SetValue(DbdAmount);
//    CalculateAmount();
//}

//DbdAmountCalculate = function () {
//    var InvoiceAmount = ctxtBillAmount.GetValue();
//    var DbdAmount = ctxtDbdAmount.GetValue();
//    var DbdPercentage = (100 * parseFloat(DbdAmount)) / parseFloat(InvoiceAmount);

//    ctxtDbdPercentage.SetValue(DbdPercentage);
//    CalculateAmount();
//}

//MbdPercentageCalculate = function () {
//    var InvoiceAmount = ctxtBillAmount.GetValue();
//    var MbdPercentage = ctxtMbdPercentage.GetValue();
//    var MbdAmount = (parseFloat(InvoiceAmount) * parseFloat(MbdPercentage)) / 100;

//    ctxtMbdAmount.SetValue(MbdAmount);
//    CalculateAmount();
//}

//MbdAmountCalculate = function () {
//    var InvoiceAmount = ctxtBillAmount.GetValue();
//    var MbdAmount = ctxtMbdAmount.GetValue();
//    var MbdPercentage = (100 * parseFloat(MbdAmount)) / parseFloat(InvoiceAmount);

//    ctxtMbdPercentage.SetValue(MbdPercentage);
//    CalculateAmount();
//}

//rowClick = function (s, e) {
//    LoadingPanel.Show();
//    var currentRow = e.visibleIndex;
//    var keyValue = cInvoiceGrid.GetRowKey(currentRow);
//    cCallbackPanel.PerformCallback('InvoiceEdit~' + keyValue);
//}