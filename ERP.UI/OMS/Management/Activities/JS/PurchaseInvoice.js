var isFirstTime = true;

// Purchase Invoice Section Start
updatePBGridByDate = function () {
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

        localStorage.setItem("PBFromDate", cFormDate.GetDate().format('yyyy-MM-dd'));
        localStorage.setItem("PBToDate", ctoDate.GetDate().format('yyyy-MM-dd'));
        localStorage.setItem("PBBranch", ccmbBranchfilter.GetValue());

        //cdownpaygrid.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue());

        $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
        $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
        $("#hfBranchID").val(ccmbBranchfilter.GetValue());
        $("#hfIsFilter").val("Y");

        cgrid.Refresh();

        //$("#drdExport").val(0);
    }
}
updateGridAfterDelete = function () {
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

        localStorage.setItem("PBFromDate", cFormDate.GetDate().format('yyyy-MM-dd'));
        localStorage.setItem("PBToDate", ctoDate.GetDate().format('yyyy-MM-dd'));
        localStorage.setItem("PBBranch", ccmbBranchfilter.GetValue());
        $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
        $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
        $("#hfBranchID").val(ccmbBranchfilter.GetValue());
        $("#hfIsFilter").val("Y");

        cgrid.Refresh();

        //$("#drdExport").val(0);

        //cdownpaygrid.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue());
        //$("#drdExport").val(0);
    }
}

AllControlInitilize = function (s, e) {
    if (isFirstTime) {
        if (localStorage.getItem('PBFromDate')) {
            var fromdatearray = localStorage.getItem('PBFromDate').split('-');
            var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
            cFormDate.SetDate(fromdate);
        }

        if (localStorage.getItem('PBToDate')) {
            var todatearray = localStorage.getItem('PBToDate').split('-');
            var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
            ctoDate.SetDate(todate);
        }

        if (localStorage.getItem('PBBranch')) {
            if (ccmbBranchfilter.FindItemByValue(localStorage.getItem('PBBranch'))) {
                ccmbBranchfilter.SetValue(localStorage.getItem('PBBranch'));
            }
        }
    }
}



