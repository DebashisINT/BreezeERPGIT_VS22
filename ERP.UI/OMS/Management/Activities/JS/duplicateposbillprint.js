var isFirstTime = true;
function AllControlInitilize() {
    if (isFirstTime) {
        PopulateCurrentBankBalance(ccmbBranchfilter.GetValue());

        if (localStorage.getItem('DuplicatePosListFromDate')) {
            var fromdatearray = localStorage.getItem('DuplicatePosListFromDate').split('-');
            var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
            cFormDate.SetDate(fromdate);
        }

        if (localStorage.getItem('DuplicatePosListToDate')) {
            var todatearray = localStorage.getItem('DuplicatePosListToDate').split('-');
            var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
            ctoDate.SetDate(todate);
        }
        if (localStorage.getItem('DuplicatePosListBranch')) {
            if (ccmbBranchfilter.FindItemByValue(localStorage.getItem('DuplicatePosListBranch'))) {
                ccmbBranchfilter.SetValue(localStorage.getItem('DuplicatePosListBranch'));
            }

        }
        //updateGridByDate();
        isFirstTime = false;
    }
}
function updateGridByDate() {
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

        localStorage.setItem("DuplicatePosListFromDate", cFormDate.GetDate().format('yyyy-MM-dd'));
        localStorage.setItem("DuplicatePosListToDate", ctoDate.GetDate().format('yyyy-MM-dd'));
        localStorage.setItem("DuplicatePosListBranch", ccmbBranchfilter.GetValue());

        $('#branchName').text(ccmbBranchfilter.GetText());
        PopulateCurrentBankBalance(ccmbBranchfilter.GetValue());
        //cGrdQuotation.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue())
        $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
        $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
        $("#hfBranchID").val(ccmbBranchfilter.GetValue());
        $("#hfIsFilter").val("Y");
        cGrdQuotation.Refresh();

    }
}
function ListingGridEndCallback(s, e) {
    if (cGrdQuotation.cpCancelAssignMent) {
        if (cGrdQuotation.cpCancelAssignMent == "yes") {
            jAlert("Branch Assignment Cancel Successfully.");
            cGrdQuotation.cpCancelAssignMent = null;
            cGrdQuotation.Refresh();
        }
    }
    if (cGrdQuotation.cpDelete) {
        jAlert(cGrdQuotation.cpDelete);
        cGrdQuotation.cpDelete = null;
        cGrdQuotation.Refresh();
    }
}
function PopulateCurrentBankBalance(BranchId) {
    var frDate = cFormDate.GetDate().format('yyyy-MM-dd');
    var toDate = ctoDate.GetDate().format('yyyy-MM-dd');

    $.ajax({
        type: "POST",
        url: 'PosSalesInvoicelist.aspx/GetCurrentBankBalance',
        data: JSON.stringify({ BranchId: BranchId, fromDate: frDate, todate: toDate }),

        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var list = msg.d;

            if (msg.d.length > 0) {
                document.getElementById("pageheaderContent").style.display = 'block';
                if (msg.d.split('~')[0] != '') {

                    document.getElementById('<%=B_BankBalance.ClientID %>').innerHTML = msg.d.split('~')[0];
                    document.getElementById('<%=B_BankBalance.ClientID %>').style.color = "Black";
                }
                else {
                    document.getElementById('<%=B_BankBalance.ClientID %>').innerHTML = '0.0';
                    document.getElementById('<%=B_BankBalance.ClientID %>').style.color = "Black";

                }
            }

        },

    });

}
function onPrintJv(id, RowIndex) {
    debugger;
    InvoiceId = id;
    var module = "POS_Duplicate"
    var Type = cGrdQuotation.GetRow(RowIndex).children[4].innerText;
    var reportName = "";

    if (Type == "Cash") {
        reportName = "Cash-Duplicate~D";
    }
    else if (Type == "Finance") {
        reportName = "Finance-Duplicate~D";
    }
    else if (Type == "Credit") {
        reportName = "Credit-Duplicate~D";
    }

    window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + InvoiceId + '&PrintOption=' + 5, '_blank')
            
}
function gridRowclick(s, e) {
    $('#GrdQuotation').find('tr').removeClass('rowActive');
    $('.floatedBtnArea').removeClass('insideGrid');
    //$('.floatedBtnArea a .ico').css({ 'opacity': '0' });
    $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea').addClass('insideGrid');
    $(s.GetRow(e.visibleIndex)).addClass('rowActive');
    setTimeout(function () {
        //alert('delay');
        var lists = $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a');
        //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a .ico').css({'opacity': '1'});
        //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a').each(function (e) {
        //    setTimeout(function () {
        //        $(this).fadeIn();
        //    }, 100);
        //});    
        $.each(lists, function (index, value) {
            //console.log(index);
            //console.log(value);
            setTimeout(function () {
                console.log(value);
                $(value).css({ 'opacity': '1' });
            }, 100);
        });
    }, 200);
}
    $(document).ready(function () {
        if ($('body').hasClass('mini-navbar')) {
            var windowWidth = $(window).width();
            var cntWidth = windowWidth - 90;
            cGrdQuotation.SetWidth(cntWidth);
        } else {
            var windowWidth = $(window).width();
            var cntWidth = windowWidth - 220;
            cGrdQuotation.SetWidth(cntWidth);
        }
        $('.navbar-minimalize').click(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                cGrdQuotation.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                cGrdQuotation.SetWidth(cntWidth);
            }

        });
    });
