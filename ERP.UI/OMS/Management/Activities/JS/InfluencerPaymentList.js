
document.onkeydown = function (e) {
    if ((event.keyCode == 120 || event.keyCode == 65) && event.altKey == true) { //run code for Ctrl+S -- ie, Save & New  
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
    var url = 'InfluencerPayment.aspx?key=' + 'Add';
    window.location.href = url;
}
function OnViewClick(keyValue) {
    var url = 'InfluencerPayment.aspx?key=Edit&Id=' + keyValue + '&req=View';
    window.location.href = url;
}
function OnMoreInfoClick(keyValue) {
    var url = 'InfluencerPayment.aspx?key=Edit&Id=' + keyValue;
    window.location.href = url;
}
function OnClickDelete(keyValue) {
    jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
        if (r == true) {
            CgvInfluencerPayment.PerformCallback('Delete~' + keyValue);
        }
    });
}
function ShowMsgLastCall() {

    if (CgvInfluencerPayment.cpDelete != null) {

        jAlert(CgvInfluencerPayment.cpDelete)
        CgvInfluencerPayment.PerformCallback();
        CgvInfluencerPayment.cpDelete = null
        CgvInfluencerPayment.Refresh();
    }
}

var RecPayId = 0;
function onPrintJv(id, RowIndex) {

    jAlert('Design not available.', 'Alert');

    //RecPayId = id;
    //cSelectPanel.cpSuccess = "";
    //cDocumentsPopup.Show();
    //$('#HdRecPayType').val(CgvInfluencerPayment.GetRow(RowIndex).children[1].innerText);
    //cCmbDesignName.SetSelectedIndex(0);
    //cSelectPanel.PerformCallback('Bindalldesignes');
    //$('#btnOK').focus();
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
    if (localStorage.getItem('InfluencerPayFromDate')) {
        var fromdatearray = localStorage.getItem('InfluencerPayFromDate').split('-');
        var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
        cFormDate.SetDate(fromdate);
    }

    if (localStorage.getItem('InfluencerPayToDate')) {
        var todatearray = localStorage.getItem('InfluencerPayToDate').split('-');
        var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
        ctoDate.SetDate(todate);
    }
    if (localStorage.getItem('InfluencerPayBranch')) {
        if (ccmbBranchfilter.FindItemByValue(localStorage.getItem('InfluencerPayBranch'))) {
            ccmbBranchfilter.SetValue(localStorage.getItem('InfluencerPayBranch'));
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
        localStorage.setItem("InfluencerPayFromDate", cFormDate.GetDate().format('yyyy-MM-dd'));
        localStorage.setItem("InfluencerPayToDate", ctoDate.GetDate().format('yyyy-MM-dd'));
        localStorage.setItem("InfluencerPayBranch", ccmbBranchfilter.GetValue());
        $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
        $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
        $("#hfBranchID").val(ccmbBranchfilter.GetValue());
        $("#hfIsFilter").val("Y");
        CgvInfluencerPayment.Refresh();
        $("#drdExport").val(0);
        //CgvInfluencerPayment.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue())
    }
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
                CgvInfluencerPayment.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                CgvInfluencerPayment.SetWidth(cntWidth);
            }

            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    CgvInfluencerPayment.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    CgvInfluencerPayment.SetWidth(cntWidth);
                }

            });
        });
