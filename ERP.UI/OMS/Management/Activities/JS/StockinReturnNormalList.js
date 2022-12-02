
var ReturnId = 0;
function onPrintJv(id) {
    // debugger;
    ReturnId = id;
    cSelectPanel.cpSuccess = "";
    cDocumentsPopup.Show();
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
    debugger;
    if (cSelectPanel.cpSuccess != "") {
        var TotDocument = cSelectPanel.cpSuccess.split(',');
        var reportName = cCmbDesignName.GetValue();
        var module = 'Sales_Return';
        if (TotDocument.length > 0) {
            for (var i = 0; i < TotDocument.length; i++) {
                if (TotDocument[i] != "") {
                    window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + ReturnId + '&PrintOption=' + TotDocument[i], '_blank')
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

var isFirstTime = true;

function AllControlInitilize() {
    if (isFirstTime) {
        if (localStorage.getItem('ReturnList_FromDate')) {
            var fromdatearray = localStorage.getItem('ReturnList_FromDate').split('-');
            var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
            cFormDate.SetDate(fromdate);
        }

        if (localStorage.getItem('ReturnList_ToDate')) {
            var todatearray = localStorage.getItem('ReturnList_ToDate').split('-');
            var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
            ctoDate.SetDate(todate);
        }

        if (localStorage.getItem('ReturnList_Branch')) {
            if (ccmbBranchfilter.FindItemByValue(localStorage.getItem('ReturnList_Branch'))) {
                ccmbBranchfilter.SetValue(localStorage.getItem('ReturnList_Branch'));
            }

        }

        //if ($("#LoadGridData").val() == "ok")
        //    updateGridByDate();
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
        localStorage.setItem("ReturnList_FromDate", cFormDate.GetDate().format('yyyy-MM-dd'));
        localStorage.setItem("ReturnList_ToDate", ctoDate.GetDate().format('yyyy-MM-dd'));
        localStorage.setItem("ReturnList_Branch", ccmbBranchfilter.GetValue());
        $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
        $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
        $("#hfBranchID").val(ccmbBranchfilter.GetValue());
        $("#hfIsFilter").val("Y");
        cGrdSalesReturn.Refresh();
        //  cGrdSalesReturn.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue())
    }
}


function OnclickViewAttachment(obj) {
    //var URL = '/OMS/Management/Activities/SalesReturn_Document.aspx?idbldng=' + obj + '&type=SalesReturn';
    var URL = '/OMS/Management/Activities/EntriesDocuments.aspx?idbldng=' + obj + '&type=NormalSaleReturn';
    window.location.href = URL;
}
document.onkeydown = function (e) {
    if (event.keyCode == 18) isCtrl = true;


    if (event.keyCode == 65 && isCtrl == true) { //run code for alt+a -- ie, Add

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
function OnAddButtonClick() {
    var url = 'StockinReturnNormal.aspx?key=' + 'ADD';
    window.location.href = url;
}



////##### coded by Samrat Roy - 17/04/2017 - ref IssueLog(P Order - 70) 
////Add an another param to define request type 
function OnViewClick(keyValue) {
    var url = 'StockinReturnNormal.aspx?key=' + keyValue + '&req=V' + '&type=SRN';  // Date: 31-05-2017    Author: Kallol Samanta
    window.location.href = url;
}

function OnClickDelete(keyValue) {
    jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
        if (r == true) {
            cGrdSalesReturn.PerformCallback('Delete~' + keyValue);
        }
    });
}
function OnEndCallback(s, e) {

    if (cGrdSalesReturn.cpDelete != null) {
        jAlert(cGrdSalesReturn.cpDelete);

        cGrdSalesReturn.cpDelete = null;

        window.location.href = "StockinReturnNormalList.aspx";
    }
}
//function OnClickDelete(keyValue) {
//    jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
//        if (r == true) {
//            cGrdQuotation.PerformCallback('Delete~' + keyValue);
//        }
//    });
//}

function OnTransferClick(keyValue) {
    cPopup_BranchTransfer.Show();

    cBranchTransferCallBackPanel.PerformCallback('Edit~' + keyValue);
}


function SaveButtonClick() {
    var branch = $("#ddlBranch").val();
    if (branch == "") {
        $("#MandatoryBranch").show();
        return false;
    }
    //var CashBank = cddlCashBank.GetValue();
    //if (CashBank == null) {
    //    $("#MandatoryCashBank").show();
    //    return false;
    //}
    var EditID = document.getElementById('hdnEditID').value;
    //var branch = $("#ddlBranch").val();
    cBranchTransferCallBackPanel.PerformCallback('Save~' + EditID + '~' + branch);
}
function BranchTransferEndCallBack() {
    if (cBranchTransferCallBackPanel.cpEdit != null) {
        document.getElementById('hdnEditID').value = cBranchTransferCallBackPanel.cpEdit;
        cBranchTransferCallBackPanel.cpEdit = null;
    }
    if (cBranchTransferCallBackPanel.cpTransfer == "YES") {
        cPopup_BranchTransfer.Hide();
        window.location.href = 'StockinReturnNormalList.aspx';
        cBranchTransferCallBackPanel.cpTransfer = null;
    }
    if (cBranchTransferCallBackPanel.cpBtnVisible != null && cBranchTransferCallBackPanel.cpBtnVisible != "") {
        cBranchTransferCallBackPanel.cpBtnVisible = null;
        document.getElementById('btnSaveNew').style.display = 'none'
        document.getElementById('tagged').style.display = 'block';
    }
}

    $(document).ready(function () {
        if ($('body').hasClass('mini-navbar')) {
            var windowWidth = $(window).width();
            var cntWidth = windowWidth - 90;
            cGrdSalesReturn.SetWidth(cntWidth);
        } else {
            var windowWidth = $(window).width();
            var cntWidth = windowWidth - 220;
            cGrdSalesReturn.SetWidth(cntWidth);
        }
        $('.navbar-minimalize').click(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                cGrdSalesReturn.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                cGrdSalesReturn.SetWidth(cntWidth);
            }

        });
    });
