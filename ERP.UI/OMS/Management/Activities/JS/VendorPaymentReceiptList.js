document.onkeydown = function (e) {

    if (event.keyCode == 80 && event.altKey == true) { //run code for Ctrl+S -- ie, Save & New  event.keyCode == 120 ||
        StopDefaultAction(e);
        AddButtonClick();
    }
}
function StopDefaultAction(e) {
    if (e.preventDefault) { e.preventDefault() }
    else { e.stop() };

    e.returnValue = false;
    e.stopPropagation();
}
function AddButtonClick() {
    var url = 'VendorPaymentReceipt.aspx?key=' + 'ADD';
    window.location.href = url;
}
function OnViewClick(keyValue) {
    var url = 'VendorPaymentReceipt.aspx?key=' + keyValue + '&type=VPR' + '&req=V';
    window.location.href = url;
}
function OnMoreInfoClick(keyValue) {
    var url = 'VendorPaymentReceipt.aspx?key=' + keyValue + '&type=VPR';
    window.location.href = url;
}
function OnClickDelete(keyValue) {
    jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
        if (r == true) {
            CgvCustomerReceiptPayment.PerformCallback('Delete~' + keyValue);
        }
    });
}
function ShowMsgLastCall() {

    if (CgvCustomerReceiptPayment.cpDelete != null) {

        jAlert(CgvCustomerReceiptPayment.cpDelete)
        CgvCustomerReceiptPayment.PerformCallback();
        CgvCustomerReceiptPayment.cpDelete = null
        CgvCustomerReceiptPayment.Refresh();
    }
}

var RecPayId = 0;
function onPrintJv(id, RowIndex) {

    RecPayId = id;
    cSelectPanel.cpSuccess = "";
    cDocumentsPopup.Show();
    $('#HdRecPayType').val(CgvCustomerReceiptPayment.GetRow(RowIndex).children[1].innerText);
    cCmbDesignName.SetSelectedIndex(0);
    cSelectPanel.PerformCallback('Bindalldesignes');
    $('#btnOK').focus();
}

function PerformCallToGridBind() {
    cSelectPanel.PerformCallback('Bindsingledesign');
    cDocumentsPopup.Hide();
    return false;
}

function cSelectPanelEndCall(s, e) {

    if (cSelectPanel.cpSuccess != "") {
        var TotDocument = cSelectPanel.cpSuccess.split(',');
        var reportName = cCmbDesignName.GetValue();
        var module = 'VENDRECPAY';
        if (TotDocument.length > 0) {
            for (var i = 0; i < TotDocument.length; i++) {
                if (TotDocument[i] != "") {
                    window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + RecPayId + '&PrintOption=' + TotDocument[i], '_blank')
                }
            }
        }
    }
    if (cSelectPanel.cpSuccess == "") {
        if (cSelectPanel.cpChecked != "") {
            jAlert('Please check Original For Recipient and proceed.');
        }
        CselectOriginal.SetCheckState('UnChecked');
        CselectDuplicate.SetCheckState('UnChecked');
        CselectTriplicate.SetCheckState('UnChecked');
        cCmbDesignName.SetSelectedIndex(0);
    }
}

function AllControlInitilize() {
    if (localStorage.getItem('VendorRecPayFromDate')) {
        var fromdatearray = localStorage.getItem('VendorRecPayFromDate').split('-');
        var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
        cFormDate.SetDate(fromdate);
    }

    if (localStorage.getItem('VendorRecPayToDate')) {
        var todatearray = localStorage.getItem('VendorRecPayToDate').split('-');
        var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
        ctoDate.SetDate(todate);
    }
    if (localStorage.getItem('VendorRecPayBranch')) {
        if (ccmbBranchfilter.FindItemByValue(localStorage.getItem('VendorRecPayBranch'))) {
            ccmbBranchfilter.SetValue(localStorage.getItem('VendorRecPayBranch'));
        }

    }
    // updateGridByDate();
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
        localStorage.setItem("VendorRecPayFromDate", cFormDate.GetDate().format('yyyy-MM-dd'));
        localStorage.setItem("VendorRecPayToDate", ctoDate.GetDate().format('yyyy-MM-dd'));
        localStorage.setItem("VendorRecPayBranch", ccmbBranchfilter.GetValue());
        $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
        $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
        $("#hfBranchID").val(ccmbBranchfilter.GetValue());
        $("#hfIsFilter").val("Y");
        CgvCustomerReceiptPayment.Refresh();
        $("#drdExport").val(0);
        //CgvCustomerReceiptPayment.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue())
    }
}
function gridRowclick(s, e) {
    $('#Grid_CustomerReceiptPayment').find('tr').removeClass('rowActive');
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
    $('.navbar-minimalize').click(function () {
        CgvCustomerReceiptPayment.Refresh();
    });
    CgvCustomerReceiptPayment.Refresh();

    $("#expandCgvCustomerReceiptPayment").click(function (e) {
        e.preventDefault();
        var $this = $(this);
        if ($this.children('i').hasClass('fa-expand')) {
            $this.removeClass('hovered half').addClass('full');
            $this.attr('title', 'Minimize Grid');
            $this.children('i').removeClass('fa-expand');
            $this.children('i').addClass('fa-arrows-alt');
            var gridId = $(this).attr('data-instance');
            $(this).closest('.makeFullscreen').addClass('panel-fullscreen');
            var cntWidth = $(this).parent('.makeFullscreen').width();
            var browserHeight = document.documentElement.clientHeight;
            var browserWidth = document.documentElement.clientWidth;
            CgvCustomerReceiptPayment.SetHeight(browserHeight - 150);
            CgvCustomerReceiptPayment.SetWidth(cntWidth);
        }
        else if ($this.children('i').hasClass('fa-arrows-alt')) {
            $this.children('i').removeClass('fa-arrows-alt');
            $this.removeClass('full').addClass('hovered half');
            $this.attr('title', 'Maximize Grid');
            $this.children('i').addClass('fa-expand');
            var gridId = $(this).attr('data-instance');
            $(this).closest('.makeFullscreen').removeClass('panel-fullscreen');
            var browserHeight = document.documentElement.clientHeight;
            var browserWidth = document.documentElement.clientWidth;
            CgvCustomerReceiptPayment.SetHeight(450);
            var cntWidth = $this.parent('.makeFullscreen').width();
            CgvCustomerReceiptPayment.SetWidth(cntWidth);
        }
    });
});
