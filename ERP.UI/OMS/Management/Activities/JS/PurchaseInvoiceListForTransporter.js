var isFirstTime = true;
// Purchase Invoice Section Start
updatePBTRGridByDate = function () {
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

        localStorage.setItem("PBTRFromDate", cFormDate.GetDate().format('yyyy-MM-dd'));
        localStorage.setItem("PBTRToDate", ctoDate.GetDate().format('yyyy-MM-dd'));
        localStorage.setItem("PBTRBranch", ccmbBranchfilter.GetValue());

        //cdownpaygrid.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue());

        $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
        $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
        $("#hfBranchID").val(ccmbBranchfilter.GetValue());
        $("#hfIsFilter").val("Y");

        cgrid.Refresh();

        $("#drdExport").val(0);
    }
}

AllControlInitilize = function (s, e) {
    if (isFirstTime) {
        if (localStorage.getItem('PBTRFromDate')) {
            var fromdatearray = localStorage.getItem('PBTRFromDate').split('-');
            var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
            cFormDate.SetDate(fromdate);
        }

        if (localStorage.getItem('PBTRToDate')) {
            var todatearray = localStorage.getItem('PBTRToDate').split('-');
            var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
            ctoDate.SetDate(todate);
        }

        if (localStorage.getItem('PBTRBranch')) {
            if (ccmbBranchfilter.FindItemByValue(localStorage.getItem('PBTRBranch'))) {
                ccmbBranchfilter.SetValue(localStorage.getItem('PBTRBranch'));
            }
        }
    }
}
function BeginCallback() {
    $("#drdExport").val(0);
}

function ClearField() {
    cFormDate.SetDate(null);
    ctoDate.SetDate(null);
    ccmbBranchfilter.SetSelectedIndex(0);
}
function updateGridByDate() {
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
        $('#branchName').text(ccmbBranchfilter.GetText());
        //PopulateCurrentBankBalance(ccmbBranchfilter.GetValue());
        //if (page.activeTabIndex == 0) {
        cgrid.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue())
    }
}
//<%-- Filteration Section Start By Sam--%>
function OnAddEditClick(e, obj) {
    var data = obj.split('~');
    if (data.length > 1)
        RowID = data[1];
    cproductpopup.Show();
    popproductPanel.PerformCallback(obj);
}
var PInvoice_id = 0;
function onPrintJv(id) {
    PInvoice_id = id;
    cSelectPanel.cpSuccess = "";
    cDocumentsPopup.Show();
    CselectDuplicate.SetEnabled(false);
    CselectTriplicate.SetEnabled(false);
    CselectOriginal.SetCheckState('UnChecked');
    CselectDuplicate.SetCheckState('UnChecked');
    CselectTriplicate.SetCheckState('UnChecked');
    cCmbDesignName.SetSelectedIndex(0);
    cSelectPanel.PerformCallback('Bindalldesignes');
    $('#btnOK').focus();
}
function PerformCallToGridBind() {
    //cgriddocuments.PerformCallback('BindDocumentsGridOnSelection' + '~' + PInvoice_id);
    cSelectPanel.PerformCallback();
    cDocumentsPopup.Hide();
    return false;
}
function cSelectPanelEndCall(s, e) {
    debugger;
    if (cSelectPanel.cpSuccess != null) {
        var TotDocument = cSelectPanel.cpSuccess.split(',');
        var reportName = cCmbDesignName.GetValue();
        var module = 'PINV_TRANSPORTR';
        if (TotDocument.length > 0) {
            for (var i = 0; i < TotDocument.length; i++) {
                if (TotDocument[i] != "") {
                    window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + PInvoice_id + '&PrintOption=' + TotDocument[i], '_blank')
                }
            }
        }
    }
    //cSelectPanel.cpSuccess = null
    if (cSelectPanel.cpSuccess == "") {
        if (cSelectPanel.cpChecked != "") {
            jAlert('Please check Original For Recipient and proceed.');
        }
        CselectDuplicate.SetEnabled(false);
        CselectTriplicate.SetEnabled(false);
        CselectOriginal.SetCheckState('UnChecked');
        CselectDuplicate.SetCheckState('UnChecked');
        CselectTriplicate.SetCheckState('UnChecked');
        cCmbDesignName.SetSelectedIndex(0);
    }
}

function OrginalCheckChange(s, e) {
    debugger;
    if (s.GetCheckState() == 'Checked') {
        CselectDuplicate.SetEnabled(true);
    }
    else {
        CselectDuplicate.SetCheckState('UnChecked');
        CselectDuplicate.SetEnabled(false);
        CselectTriplicate.SetCheckState('UnChecked');
        CselectTriplicate.SetEnabled(false);
    }

}
function DuplicateCheckChange(s, e) {
    if (s.GetCheckState() == 'Checked') {
        CselectTriplicate.SetEnabled(true);
    }
    else {
        CselectTriplicate.SetCheckState('UnChecked');
        CselectTriplicate.SetEnabled(false);
    }

}
function OpenPopUPUserWiseQuotaion() {
    cgridUserWiseQuotation.PerformCallback();
    cPopupUserWiseQuotation.Show();
}
document.onkeydown = function (e) {
    if (event.keyCode == 18) isCtrl = true;
    if (event.keyCode == 65 && isCtrl == true) { //run code for Ctrl+S -- ie, Save & New  
        StopDefaultAction(e);
        OnAddButtonClick();
    }
}
function StopDefaultAction(e) {
    if (e.preventDefault) { e.preventDefault() }
    else { e.stop() };

    e.returnValue = false;
    e.stopPropagation();
}
function OnClickDelete(keyValue) {
    jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
        if (r == true) {
            cgrid.PerformCallback('Delete~' + keyValue);
        }
    });
}
function OnClickStatus(keyValue) {
    GetObjectID('hiddenedit').value = keyValue;
    cgrid.PerformCallback('Edit~' + keyValue);
}
function OpenPopUPApprovalStatus() {
    cgridPendingApproval.PerformCallback();
    cpopupApproval.Show();
}
function grid_EndCallBack() {
    if (cgrid.cpEdit != null) {
        GetObjectID('hiddenedit').value = cgrid.cpEdit.split('~')[0];
        cProforma.SetText(cgrid.cpEdit.split('~')[1]);
        cCustomer.SetText(cgrid.cpEdit.split('~')[4]);
        var pro_status = cgrid.cpEdit.split('~')[2]
        //cgrid.cpEdit = null;
        if (pro_status != null) {
            var radio = $("[id*=rbl_QuoteStatus] label:contains('" + pro_status + "')").closest("td").find("input");
            radio.attr("checked", "checked");
            //return false;
            //$('#rbl_QuoteStatus[type=radio][value=' + pro_status + ']').prop('checked', true); 
            cQuotationRemarks.SetText(cgrid.cpEdit.split('~')[3]);

            cQuotationStatus.Show();
        }
    }
    if (cgrid.cpUpdate != null) {
        GetObjectID('hiddenedit').value = '';
        cProforma.SetText('');
        cCustomer.SetText('');
        cQuotationRemarks.SetText('');
        var pro_status = 2;
        if (pro_status != null) {
            var radio = $("[id*=rbl_QuoteStatus] label:contains('" + pro_status + "')").closest("td").find("input");
            radio.attr("checked", "checked");
            cQuotationStatus.Hide();
        }
        jAlert(cgrid.cpUpdate);
    }
    if (cgrid.cpDelete != null) {
        jAlert(cgrid.cpDelete);
        cgrid.Refresh();
        cgrid.cpDelete = null;
    }


}
function SavePrpformaStatus() {
    if (document.getElementById('hiddenedit').value == '') {
        cgrid.PerformCallback('save~');
    }
    else {
        var checked_radio = $("[id*=rbl_QuoteStatus] input:checked");
        var status = checked_radio.val();
        var remarks = cQuotationRemarks.GetText();
        cgrid.PerformCallback('update~' + GetObjectID('hiddenedit').value + '~' + status + '~' + remarks);
    }

}
////##### coded by Samrat Roy - 04/05/2017   
////Add an another param to define request type 
function OnViewClick(keyValue) {
    var url = 'PurchaseInvoiceForTransporter.aspx?key=' + keyValue + '&req=V' + '&type=PB';
    window.location.href = url;
}
function OnclickViewAttachment(obj) {
    var URL = '/OMS/Management/Activities/PurchaseInvoice_Document.aspx?idbldng=' + obj + '&type=PurchaseInvoice';
    window.location.href = URL;
}

function OnAddButtonClick() {
    var url = 'PurchaseInvoiceForTransporter.aspx?key=' + 'ADD';
    window.location.href = url;
}
var keyval;
//function FocusedRowChanged(s, e) {
//    keyval=s.GetRowKey(s.GetFocusedRowIndex());
//}

//var globalRowIndex;

//function GetVisibleIndex(s, e) {
//    globalRowIndex = e.visibleIndex;
//}
//RowClick = "GetVisibleIndex"

// User Approval Status Start

function GetApprovedQuoteId(s, e, itemIndex) {
    var rowvalue = cgridPendingApproval.GetRowValues(itemIndex, 'ID', OnGetApprovedRowValues);
    //var currentRow = cgridPendingApproval.GetRow(0);
    //var col1 = currentRow.find("td:eq(0)").html();

    cgridPendingApproval.PerformCallback('Status~' + rowvalue);
    cgridPendingApproval.GetRowValues(itemIndex, 'ID', OnGetApprovedRowValues);

}
function OnGetApprovedRowValues(obj) {
    uri = "PurchaseInvoiceForTransporter.aspx?key=" + obj + "&status=2" + '&type=PB';
    popup.SetContentUrl(uri);
    popup.Show();
    //window.location.href = uri;

}

function ch_fnApproved() {
}


function GetRejectedQuoteId(s, e, itemIndex) {
    debugger;
    cgridPendingApproval.GetRowValues(itemIndex, 'ID', OnGetRejectedRowValues);

}
function OnGetRejectedRowValues(obj) {
    uri = "PurchaseInvoiceForTransporter.aspx?key=" + obj + "&status=3" + '&type=PB';
    popup.SetContentUrl(uri);
    popup.Show();
}
// User Approval Status End
function OnApprovalEndCall(s, e) {
    $.ajax({
        type: "POST",
        url: "PurchaseInvoiceListForTransporter.aspx.aspx/GetPendingCase",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $('#lblWaiting').text(data.d);
        }
    });
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
    //Toggle fullscreen expandEntryGrid
    $("#expandcgrid").click(function (e) {
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
            cgrid.SetHeight(browserHeight - 150);
            cgrid.SetWidth(cntWidth);
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
            cgrid.SetHeight(450);
            var cntWidth = $this.parent('.makeFullscreen').width();
            cgrid.SetWidth(cntWidth);
        }
    });
});
$(document).ready(function () {
        if ($('body').hasClass('mini-navbar')) {
            var windowWidth = $(window).width();
            var cntWidth = windowWidth - 90;
            cgrid.SetWidth(cntWidth);
        } else {
            var windowWidth = $(window).width();
            var cntWidth = windowWidth - 220;
            cgrid.SetWidth(cntWidth);
        }
        $('.navbar-minimalize').click(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                cgrid.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                cgrid.SetWidth(cntWidth);
            }

        });
    });
