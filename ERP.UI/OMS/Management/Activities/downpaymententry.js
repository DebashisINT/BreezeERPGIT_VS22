
var isFirstTime = true;

updateGridByDate = function () {

    if (cFormDate.GetDate() == null) {
        jAlert('Please select from date.', 'Alert', function () { cFormDate.Focus(); });
    }
    else if (ctoDate.GetDate() == null) {
        jAlert('Please select to date.', 'Alert', function () { ctoDate.Focus(); });
    }
    else if (ccmbBranchfilter.GetValue() == null) {
        jAlert('Please select Branch.', 'Alert', function () { ccmbBranchfilter.Focus(); });
    }
    else {

        localStorage.setItem("DownPaymentFromDate", cFormDate.GetDate().format('yyyy-MM-dd'));
        localStorage.setItem("DownPaymentToDate", ctoDate.GetDate().format('yyyy-MM-dd'));
        localStorage.setItem("DownPaymentBranch", ccmbBranchfilter.GetValue());

        cdownpaygrid.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue());
    }
}



AllControlInitilize = function (s, e) {
    if (isFirstTime) { 

        if (localStorage.getItem('DownPaymentFromDate')) {
            var fromdatearray = localStorage.getItem('DownPaymentFromDate').split('-');
            var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
            cFormDate.SetDate(fromdate);
        }

        if (localStorage.getItem('DownPaymentToDate')) {
            var todatearray = localStorage.getItem('DownPaymentToDate').split('-');
            var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
            ctoDate.SetDate(todate);
        }
        if (localStorage.getItem('DownPaymentBranch')) {
            if (ccmbBranchfilter.FindItemByValue(localStorage.getItem('DownPaymentBranch'))) {
                ccmbBranchfilter.SetValue(localStorage.getItem('DownPaymentBranch'));
            }

        }
        if ($("#LoadGridData").val() == "ok")
            updateGridByDate();

        isFirstTime = false;
    }
}


AddnewFinance = function () {
    cfinancePopup.Show();
    $("#txtdownPayNo").focus();
}