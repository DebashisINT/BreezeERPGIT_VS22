/**********************************************************************************************************************************
 1.0      30-05-2023        2.0.38        Sanchita      ERP - Listing Views - Purchase Invoice. refer: 26250
 2.0      04-12-2023        V2.0.41       Priti         0027044: Feature to enable to edit Party Invoice Number and Party Invoice date in Transit Purchase Invoice
***********************************************************************************************************************************/
var isFirstTime = true;

// Purchase Invoice Section Start
updateTPBGridByDate = function () {
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

        localStorage.setItem("TPBFromDate", cFormDate.GetDate().format('yyyy-MM-dd'));
        localStorage.setItem("TPBToDate", ctoDate.GetDate().format('yyyy-MM-dd'));
        localStorage.setItem("TPBBranch", ccmbBranchfilter.GetValue());

        //cdownpaygrid.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue());

        $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
        $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
        $("#hfBranchID").val(ccmbBranchfilter.GetValue());
        $("#hfIsFilter").val("Y");

        cgrid.Refresh();

        $("#drdExport").val(0);
    }
}
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

        localStorage.setItem("TPBFromDate", cFormDate.GetDate().format('yyyy-MM-dd'));
        localStorage.setItem("TPBToDate", ctoDate.GetDate().format('yyyy-MM-dd'));
        localStorage.setItem("TPBBranch", ccmbBranchfilter.GetValue());
        $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
        $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
        $("#hfBranchID").val(ccmbBranchfilter.GetValue());
        $("#hfIsFilter").val("Y");

        cgrid.Refresh();

        $("#drdExport").val(0);

        //cdownpaygrid.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue());
        //$("#drdExport").val(0);
    }
}

AllControlInitilize = function (s, e) {
    if (isFirstTime) {
        if (localStorage.getItem('TPBFromDate')) {
            var fromdatearray = localStorage.getItem('TPBFromDate').split('-');
            var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
            cFormDate.SetDate(fromdate);
        }

        if (localStorage.getItem('TPBToDate')) {
            var todatearray = localStorage.getItem('TPBToDate').split('-');
            var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
            ctoDate.SetDate(todate);
        }

        if (localStorage.getItem('TPBBranch')) {
            if (ccmbBranchfilter.FindItemByValue(localStorage.getItem('TPBBranch'))) {
                ccmbBranchfilter.SetValue(localStorage.getItem('TPBBranch'));
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

//        localStorage.setItem("TPBFromDate", cFormDate.GetDate().format('yyyy-MM-dd'));
//        localStorage.setItem("TPBToDate", ctoDate.GetDate().format('yyyy-MM-dd'));
//        localStorage.setItem("TPBBranch", ccmbBranchfilter.GetValue());

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

//        localStorage.setItem("TPBFromDate", cFormDate.GetDate().format('yyyy-MM-dd'));
//        localStorage.setItem("TPBToDate", ctoDate.GetDate().format('yyyy-MM-dd'));
//        localStorage.setItem("TPBBranch", ccmbBranchfilter.GetValue());

//        cdownpaygrid.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue());
//        $("#drdExport").val(0);
//    }
//}

//AllControlInitilize = function (s, e) {
//    if (isFirstTime) {
//        if (localStorage.getItem('TPBFromDate')) {
//            var fromdatearray = localStorage.getItem('TPBFromDate').split('-');
//            var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
//            cFormDate.SetDate(fromdate);
//        }

//        if (localStorage.getItem('TPBToDate')) {
//            var todatearray = localStorage.getItem('TPBToDate').split('-');
//            var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
//            ctoDate.SetDate(todate);
//        }

//        if (localStorage.getItem('TPBBranch')) {
//            if (ccmbBranchfilter.FindItemByValue(localStorage.getItem('TPBBranch'))) {
//                ccmbBranchfilter.SetValue(localStorage.getItem('TPBBranch'));
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

/*Rev 2.0*/
function OnPartyInvClick(id) {    
    ctxtPartyInvNo.SetText('');
    $.ajax({
        type: "POST",
        url: "TPurchaseInvoicelist.aspx/EditPartyInvDate",
        data: JSON.stringify({ DocID: id }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            var status = msg.d[0];
            console.log(msg);
            if (status != "") {
                console.log(status);
                ctxtPartyInvNo.SetText(status.PartyInvoiceNo);
                var dob = new Date(status.PartyInvoiceDate);
                cdt_PartyInv.SetDate(dob);
            }
        }
    });
    $('#hddnInvoiceID').val(id);
    cPopup_PartyINV.Show();
    ctxtPartyInvNo.Focus();
}
function CancelPartyInv_save() {
    cPopup_PartyINV.Hide();
}
function CallPartyInv_save() {
    if (cdt_PartyInv.GetValue() == "" && cdt_PartyInv.GetValue() == null) {
        var PartyInvDate = "1990-01-01";
    }
    else {
        var PartyInvDate = GetEWayBillDateFormat(new Date(cdt_PartyInv.GetValue()));
    }
    var InvoiceID = $("#hddnInvoiceID").val();
    var PartyInvValue = ctxtPartyInvNo.GetText();
    $.ajax({
        type: "POST",
        url: "TPurchaseInvoicelist.aspx/UpdatePartyINVDt",
        data: JSON.stringify({
            InvoiceID: InvoiceID, PartyInvoiceNo: PartyInvValue, PartyInvoiceDate: PartyInvDate
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            var status = msg.d;
            if (status == "1") {
                jAlert("Party Invoice Number Updated successfully.");
                cPopup_PartyINV.Hide();
                cgrid.Refresh();
            }
            else if (status == "-10") {
                jAlert("Data not Updated.");
                cPopup_PartyINV.Hide();
            }
        }
    });
}
/*Rev 2.0 End*/ 