//<% --==========================================================Revision History ============================================================================================
//    1.0   Priti   V2.0.36   19- 01 - 2023    	0025313: Views to be converted to Procedures in the Listing Page of Transaction / Return - Sales / Sale Return - Manual
//    2.0   Priti   V2.0.36   17-02-2023     Afer Listing view upgradation delete data show in list issue solved.

//========================================== End Revision History =======================================================================================================--%>
var ReturnId = 0;
function onPrintJv(id) {
   
    var ActiveEInvoice = $('#hdnActiveEInvoice').val();
    if (ActiveEInvoice == "1") {
        $.ajax({
            type: "POST",
            url: "ReturnManualList.aspx/Prc_EInvoiceChecking_details",
            data: "{'returnid':'" + id + "','mode':'Print'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {
                var status = msg.d;
                if (status == "Yes") {
                    ReturnId = id;
                    cSelectPanel.cpSuccess = "";
                    cDocumentsPopup.Show();
                    cCmbDesignName.SetSelectedIndex(0);
                    cSelectPanel.PerformCallback('Bindalldesignes');
                    $('#btnOK').focus();
                }
                else {
                    jAlert("IRN generated can not print.");
                }
            }
        });
    }
    else {
        ReturnId = id;
        cSelectPanel.cpSuccess = "";
        cDocumentsPopup.Show();
        cCmbDesignName.SetSelectedIndex(0);
        cSelectPanel.PerformCallback('Bindalldesignes');
        $('#btnOK').focus();
    }    
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
    function OnEWayBillClick(id, EWayBillNumber, EWayBillDate, EWayBillValue) {

        var ActiveEInvoice = $('#hdnActiveEInvoice').val();
        if (ActiveEInvoice == "1") {
            $.ajax({
                type: "POST",
                url: "SalesReturnList.aspx/Prc_EInvoiceChecking_details",
                data: "{'returnid':'" + id + "','mode':'EWayBill'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var status = msg.d;
                    if (status == "Yes") {
                        $("#btnEWayBillSave").removeClass('hide');
                        $("#lblEwayBillStatus").text("");
                    }
                    else {
                        $("#lblEwayBillStatus").text("IRN not generated can not update.");

                        //jAlert("IRN not generated can not print.");
                        $("#btnEWayBillSave").addClass('hide');
                    }
                }
            });
        }
        if (EWayBillNumber.trim() != "") {
            ctxtEWayBillNumber.SetText(EWayBillNumber);
        }
        else {
            ctxtEWayBillNumber.SetText("");
        }

        if (EWayBillDate.trim() != "" && EWayBillDate.trim() != "01-01-1970" && EWayBillDate.trim() != "01-01-1900") {
            cdt_EWayBill.SetText(EWayBillDate);
        }
        else {
            cdt_EWayBill.SetText("");
        }
        if (EWayBillValue.trim() != "0.00" && EWayBillValue.trim() != "") {
            ctxtEWayBillValue.SetText(EWayBillValue);
        }
        else {
            ctxtEWayBillValue.SetText("0.0");
        }
        $('#hddnSalesReturnID').val(id);
        cPopup_EWayBill.Show();
        ctxtEWayBillNumber.Focus();
    }
function GetEWayBillDateFormat(today) {
    if (today != "") {
        var dd = today.getDate();
        var mm = today.getMonth() + 1; //January is 0!

        var yyyy = today.getFullYear();
        if (dd < 10) {
            dd = '0' + dd;
        }
        if (mm < 10) {
            mm = '0' + mm;
        }
        today = yyyy + '-' + mm + '-' + dd;
    }

    return today;
}
function CallEWayBill_save() {

    var ReturnID = $("#hddnSalesReturnID").val();

    var UpdateEWayBill = ctxtEWayBillNumber.GetValue();
    if (UpdateEWayBill == "0") {
        UpdateEWayBill = "";
    }
    if (cdt_EWayBill.GetValue() == "" && cdt_EWayBill.GetValue() == null) {
        var EWayBillDate = "1990-01-01";
    }
    else {
        var EWayBillDate = GetEWayBillDateFormat(new Date(cdt_EWayBill.GetValue()));
    }

    var EWayBillValue = ctxtEWayBillValue.GetValue();

    $.ajax({
        type: "POST",
        url: "ReturnManualList.aspx/UpdateEWayBill",
        data: JSON.stringify({
            ReturnID: ReturnID, UpdateEWayBill: UpdateEWayBill, EWayBillDate: EWayBillDate, EWayBillValue: EWayBillValue
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            var status = msg.d;
            if (status == "1") {
                jAlert("Saved successfully.");
                //ctxtEWayBillNumber.SetText("");
                cPopup_EWayBill.Hide();
                cGrdSalesReturn.Refresh();
            }
            else if (status == "-10") {
                jAlert("Data not saved.");
                cPopup_EWayBill.Hide();
            }
        }
    });
}
function CancelEWayBill_save() {
    cPopup_EWayBill.Hide();
}
function OnclickViewAttachment(obj) {
    //var URL = '/OMS/Management/Activities/SalesReturn_Document.aspx?idbldng=' + obj + '&type=SalesReturn';
    var URL = '/OMS/Management/Activities/EntriesDocuments.aspx?idbldng=' + obj + '&type=ManualSaleReturn';
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
    var url = 'ReturnManual.aspx?key=' + 'ADD';
    window.location.href = url;
}


////##### coded by Samrat Roy - 17/04/2017 - ref IssueLog(P Order - 70) 
////Add an another param to define request type 
function OnViewClick(keyValue) {
    var url = 'ReturnManual.aspx?key=' + keyValue + '&req=V' + '&type=SRM'; //Date: 31-05-2017  Author:Kallol Samanta
    window.location.href = url;
}

function OnClickDelete(keyValue) {

    var ActiveEInvoice = $('#hdnActiveEInvoice').val();
    if (ActiveEInvoice == "1") {
        $.ajax({
            type: "POST",
            url: "ReturnManualList.aspx/Prc_EInvoiceChecking_details",
            data: "{'returnid':'" + keyValue + "','mode':'Delete'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {
                var status = msg.d;
                if (status == "Yes") {
                    jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                        if (r == true) {
                            cGrdSalesReturn.PerformCallback('Delete~' + keyValue);
                        }
                    });
                }
                else {
                    jAlert("IRN generated can not delete.");
                }
            }
        });
    }
    else {
        jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
            if (r == true) {
                cGrdSalesReturn.PerformCallback('Delete~' + keyValue);
            }
        });
    }   
}
function OnEndCallback(s, e) {

    if (cGrdSalesReturn.cpDelete != null) {
        jAlert(cGrdSalesReturn.cpDelete);

        cGrdSalesReturn.cpDelete = null;
        /* Rev 2.0*/
        //cGrdSalesReturn.Refresh();
        updateGridByDate();
        /* Rev 2.0 End*/
        // window.location.href = "ReturnManualList.aspx";
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


function onCancelBranchAssignment(RetId) {
    cBranchTransferCallBackPanel.PerformCallback('CancelAssignment~' + RetId);
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
    if (cBranchTransferCallBackPanel.cpBranchAssignment == "YES") {

        // alert('yyy');
        // cPopup_BranchTransfer.Hide();
        // window.location.href = 'ReturnManualList.aspx';
        //cBranchTransferCallBackPanel.cpBranchAssignment = null;

        jAlert("Branch Assignment  Successfully.");
        cBranchTransferCallBackPanel.cpBranchAssignment = null;
        //cGrdSalesReturn.Refresh();

        cGrdSalesReturn.Refresh();
        cPopup_BranchTransfer.Hide();
    }

    if (cBranchTransferCallBackPanel.cpCancelAssignMent == "YES") {
        jAlert("Branch Assignment Cancel Successfully.");
        cBranchTransferCallBackPanel.cpCancelAssignMent = null;
        cGrdSalesReturn.Refresh();
    }


    if (cBranchTransferCallBackPanel.cpBtnVisible != null && cBranchTransferCallBackPanel.cpBtnVisible != "") {
        cBranchTransferCallBackPanel.cpBtnVisible = null;
        document.getElementById('btnSaveNew').style.display = 'none'
        document.getElementById('tagged').style.display = 'block';
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
     

        //REV 1.0
        //    cGrdSalesReturn.Refresh();
        $("#hFilterType").val("All");
        cCallbackPanel.PerformCallback("");
                //END REV 1.0
        // cGrdSalesReturn.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue())
    }
}
//REV 1.0
function CallbackPanelEndCall(s, e) {
    cGrdSalesReturn.Refresh();
}
//END REV 1.0
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