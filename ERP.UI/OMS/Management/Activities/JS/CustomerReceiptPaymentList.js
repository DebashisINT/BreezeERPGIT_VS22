//==========================================================Revision History ============================================================================================
//    1.0   Priti   V2.0.36     17-02-2023     Afer Listing view upgradation delete data show in list issue solved.
//========================================== End Revision History =======================================================================================================



        document.onkeydown = function (e) {
            if ((event.keyCode == 120 || event.keyCode == 65) && event.altKey == true) { //run code for alt+A -- ie, Save & New  
                StopDefaultAction(e);
                AddButtonClick();
            }
            if ((event.keyCode == 82) && event.altKey == true) { //run code for alt+A -- ie, Save & New  
                StopDefaultAction(e);
                AddReceiptButtonClick();
            }
            if ((event.keyCode == 80) && event.altKey == true) { //run code for alt+A -- ie, Save & New  
                StopDefaultAction(e);
                AddPaymentButtonClick();
            }


        }

function AllControlInitilize() {
    if (localStorage.getItem('CustomerRecPayFromDate')) {
        var fromdatearray = localStorage.getItem('CustomerRecPayFromDate').split('-');
        var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
        cFormDate.SetDate(fromdate);
    }

    if (localStorage.getItem('CustomerRecPayToDate')) {
        var todatearray = localStorage.getItem('CustomerRecPayToDate').split('-');
        var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
        ctoDate.SetDate(todate);
    }
    if (localStorage.getItem('CustomerRecPayBranch')) {
        if (ccmbBranchfilter.FindItemByValue(localStorage.getItem('CustomerRecPayBranch'))) {
            ccmbBranchfilter.SetValue(localStorage.getItem('CustomerRecPayBranch'));
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
        localStorage.setItem("CustomerRecPayFromDate", cFormDate.GetDate().format('yyyy-MM-dd'));
        localStorage.setItem("CustomerRecPayToDate", ctoDate.GetDate().format('yyyy-MM-dd'));
        localStorage.setItem("CustomerRecPayBranch", ccmbBranchfilter.GetValue());
        $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
        $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
        $("#hfBranchID").val(ccmbBranchfilter.GetValue());
        $("#hfIsFilter").val("Y");
        //CgvCustomerReceiptPayment.Refresh();
        $("#hFilterType").val("All");
        cCallbackPanel.PerformCallback("");
        //CgvCustomerReceiptPayment.Refresh();
        //$("#drdExport").val(0);

        //CgvCustomerReceiptPayment.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue())
    }
}

function CallbackPanelEndCall(s, e) {
    CgvCustomerReceiptPayment.Refresh();
}

function CustomerClick(s, e) {


    $("#hdnCustomerId").val(e);


    $("#drdExport").val('0');
    cOutstandingPopup.Show();
    var CustomerId = $("#hdnCustomerId").val();
    var BranchId = ccmbBranchfilter.GetValue();
    $("#hddnBranchId").val(BranchId);
    var AsOnDate = new Date().format('yyyy-MM-dd');
    $("#hddnAsOnDate").val(AsOnDate);
    $("#hddnOutStandingBlock").val('1');
    //Clear Row
    var rw = $("[id$='CustomerOutstanding_DXMainTable']").find("tr")
    for (var RowClount = 0; RowClount < rw.length; RowClount++) {
        rw[RowClount].remove();
    }



    //cCustomerOutstanding.Refresh();

    //cCustomerOutstanding.PerformCallback('BindOutStanding~' + CustomerId + '~' + BranchId + '~' + AsOnDate);
    var CheckUniqueCode = false;
    $.ajax({
        type: "POST",
        url: "SalesOrderAdd.aspx/GetCustomerOutStanding",
        data: JSON.stringify({ strAsOnDate: AsOnDate, strCustomerId: CustomerId, BranchId: BranchId }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        //async:false,
        success: function (msg) {

            CheckUniqueCode = msg.d;
            if (CheckUniqueCode == true) {
                cCustomerOutstanding.Refresh();

            }
        }
    });


    //cCustomerOutstanding.Refresh();
    //cOutstandingPopup.Show();


}
function StopDefaultAction(e) {
    if (e.preventDefault) { e.preventDefault() }
    else { e.stop() };

    e.returnValue = false;
    e.stopPropagation();
}
function AddButtonClick() {
    var url = 'CustomerReceiptPayment.aspx?key=' + 'ADD';
    window.location.href = url;
}

function AddReceiptButtonClick() {
    var url = 'CustomerReceipt.aspx?key=' + 'Add';
    window.location.href = url;
}
function AddPaymentButtonClick() {
    var url = 'CustomerPayment.aspx?key=' + 'Add';
    window.location.href = url;
}


function OnViewClick(keyValue) {
    var url = 'CustomerReceiptPayment.aspx?key=' + keyValue + '&type=CRP' + '&req=V';
    window.location.href = url;
}
function OnMoreInfoClick(keyValue, RowIndex) {

    var Type = CgvCustomerReceiptPayment.GetRow(RowIndex).children[1].innerText;

    if (Type.trim() == "Receipt") {
        var url = 'CustomerReceipt.aspx?key=Edit&id=' + keyValue;
    }
    else {
        var url = 'CustomerPayment.aspx?key=Edit&id=' + keyValue;
    }
    //var url = 'CustomerReceiptPayment.aspx?key=' + keyValue + '&type=CRP';
    window.location.href = url;
}
/*Rev Work Date:-21.03.2022 -Copy Function add*/
function OnCopyInfoClick(keyValue, RowIndex) {
    debugger;
    var Type = CgvCustomerReceiptPayment.GetRow(RowIndex).children[1].innerText;

    if (Type.trim() == "Receipt") {
        var url = 'CustomerReceipt.aspx?key=Copy&id=' + keyValue;
    }
    else {
        var url = 'CustomerPayment.aspx?key=Copy&id=' + keyValue;
    }
    window.location.href = url;
    /* var url = 'CustomerReceiptPayment.aspx?key=' + keyValue + '&type=Copy';
        window.location.href = url;*/
}
/* <%--Close of Rev Work Date:-21.03.2022 -Copy Function add--%>*/
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
        CgvCustomerReceiptPayment.cpDelete = null;
        /* Rev 1.0*/
        //CgvCustomerReceiptPayment.Refresh();
        updateGridByDate();
        /* Rev 1.0 End*/
    }
}

var RecPayId = 0;
function onPrintJv(id, RowIndex) {
    debugger;
    RecPayId = id;
    cSelectPanel.cpSuccess = "";
    cDocumentsPopup.Show();
    $('#HdRecPayType').val(CgvCustomerReceiptPayment.GetRow(RowIndex).children[1].innerText);
    cCmbDesignName.SetSelectedIndex(0);
    cSelectPanel.PerformCallback('Bindalldesignes');
    $('#btnOK').focus();
}

function onShowAdjustment(id, RowIndex) {
    cdoc_selectPanel.PerformCallback("GetCountDoc~" + id + "~" + RowIndex);

}
function cdoc_selectPanelEndCall() {
    var result = cdoc_selectPanel.cpresult;
    if (result != null) {
        var from = result.split('~')[0];
        if (from == "GetCount") {
            var count = result.split('~')[1];

            if (parseInt(count) == 0) {
                jAlert("No adjustment found.", 'Alert', function () { cpopDoc.Hide(); });

            }
            else {
                cpopDoc.Show();
                cdoc_selectPanel.PerformCallback("BindGrid~" + result.split('~')[2]);
            }

        }
        else {

        }
    }
}


function PerformCallToGridBind() {
    cSelectPanel.PerformCallback('Bindsingledesign');
    cDocumentsPopup.Hide();
    return false;
}

//function cSelectPanelEndCall(s, e) {
//    debugger;
//    if (cSelectPanel.cpSuccess != null) {
//        var TotDocument = cSelectPanel.cpSuccess.split(',');
//        var reportName = cCmbDesignName.GetValue();
//        var module = 'CUSTRECPAY';
//        window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + RecPayId, '_blank')
//    }
//    cSelectPanel.cpSuccess = null
//    if (cSelectPanel.cpSuccess == null) {
//        cCmbDesignName.SetSelectedIndex(0);
//    }
//}


function cSelectPanelEndCall(s, e) {
    debugger;
    if (cSelectPanel.cpSuccess != "") {
        var TotDocument = cSelectPanel.cpSuccess.split(',');
        var reportName = cCmbDesignName.GetValue();
        var module = 'CUSTRECPAY';
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
function gridRowclick(s, e) {
    //alert('hi');
    $('#gridcrmCampaign').find('tr').removeClass('rowActive');
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
            CgvCustomerReceiptPayment.SetWidth(cntWidth);
        } else {
            var windowWidth = $(window).width();
            var cntWidth = windowWidth - 220;
            CgvCustomerReceiptPayment.SetWidth(cntWidth);
        }

        $('.navbar-minimalize').click(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                CgvCustomerReceiptPayment.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                CgvCustomerReceiptPayment.SetWidth(cntWidth);
            }

        });
    });
