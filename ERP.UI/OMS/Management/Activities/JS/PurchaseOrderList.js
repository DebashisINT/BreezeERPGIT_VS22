
function OnAddEditDateClick(e, obj) {
    var data = obj.split('~');
    cInvoiceDatepopup.Show();
    popInvoiceDatePanel.PerformCallback(obj);
}
function OnAddEditClick(e, obj) {
    var data = obj.split('~');
    cInvoiceNumberpopup.Show();
    popInvoiceNumberPanel.PerformCallback(obj);
}
var isFirstTime = true;
// Purchase Invoice Section Start
updatePOGridByDate = function () {
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

        localStorage.setItem("POFromDate", cFormDate.GetDate().format('yyyy-MM-dd'));
        localStorage.setItem("POToDate", ctoDate.GetDate().format('yyyy-MM-dd'));
        localStorage.setItem("POBranch", ccmbBranchfilter.GetValue());

        //cdownpaygrid.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue());

        $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
        $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
        $("#hfBranchID").val(ccmbBranchfilter.GetValue());
        $("#hfIsFilter").val("Y");

        // Mantis Issue 25394
        //CgvPurchaseOrder.Refresh();
        cCallbackPanel.PerformCallback("");
        // End of Mantis Issue 25394

        $("#drdExport").val(0);
    }
}

AllControlInitilize = function (s, e) {
    if (isFirstTime) {
        if (localStorage.getItem('POFromDate')) {
            var fromdatearray = localStorage.getItem('POFromDate').split('-');
            var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
            cFormDate.SetDate(fromdate);
        }

        if (localStorage.getItem('POToDate')) {
            var todatearray = localStorage.getItem('POToDate').split('-');
            var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
            ctoDate.SetDate(todate);
        }

        if (localStorage.getItem('POBranch')) {
            if (ccmbBranchfilter.FindItemByValue(localStorage.getItem('POBranch'))) {
                ccmbBranchfilter.SetValue(localStorage.getItem('POBranch'));
            }
        }
    }
}


//function onPrintJv(id) {
//    window.location.href = "../../reports/XtraReports/Viewer/OrderReportViewer.aspx?id=" + id;
//}

//This function is called to show the Status of All Sales Order Created By Login User Start
function OpenPopUPUserWiseQuotaion() {
    cgridUserWiseQuotation.PerformCallback();
    cPopupUserWiseQuotation.Show();
}
// function above  End

//This function is called to show all Pending Approval of Sales Order whose Userid has been set LevelWise using Approval Configuration Module 
function OpenPopUPApprovalStatus() {
    cgridPendingApproval.PerformCallback();
    cpopupApproval.Show();
}
// function above  End


// Status 2 is passed If Approved Check box is checked by User Both Below function is called and used to show in POPUP,  the Add Page of Respective Segment(like Page for Adding Quotation ,Sale Order ,Challan)
function GetApprovedQuoteId(s, e, itemIndex) {
    var rowvalue = cgridPendingApproval.GetRowValues(itemIndex, 'ID', OnGetApprovedRowValues);
    //cgridPendingApproval.PerformCallback('Status~' + rowvalue);
    //cgridPendingApproval.GetRowValues(itemIndex, 'ID', OnGetApprovedRowValues);

}
function OnGetApprovedRowValues(obj) {
    uri = "PurchaseOrder.aspx?key=" + obj + "&status=2" + '&type=PO';
    popup.SetContentUrl(uri);
    popup.Show();
}
// function above  End For Approved

// Status 3 is passed If Approved Check box is checked by User Both Below function is called and used to show in POPUP,  the Add Page of Respective Segment(like Page for Adding Quotation ,Sale Order ,Challan)
function GetRejectedQuoteId(s, e, itemIndex) {
    debugger;
    cgridPendingApproval.GetRowValues(itemIndex, 'ID', OnGetRejectedRowValues);

}
function OnGetRejectedRowValues(obj) {
    uri = "PurchaseOrder.aspx?key=" + obj + "&status=3" + '&type=PO';
    popup.SetContentUrl(uri);
    popup.Show();
}
// function above  End For Rejected

// To Reflect the Data in Pending Waiting Grid and Pending Waiting Counting if the user approve or Rejecte the Order and Saved 

function OnApprovalEndCall(s, e) {
    $.ajax({
        type: "POST",
        url: "PurchaseOrderList.aspx/GetPendingCase",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $('#lblWaiting').text(data.d);
        }
    });
}

// function above  End 
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
        localStorage.setItem("PurchaseOrderFromDate", cFormDate.GetDate().format('yyyy-MM-dd'));
        localStorage.setItem("PurchaseOrderToDate", ctoDate.GetDate().format('yyyy-MM-dd'));
        localStorage.setItem("PurchaseOrderBranch", ccmbBranchfilter.GetValue());

        CgvPurchaseOrder.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue())
    }
}
//<%-- Code Added By Sandip For Approval Detail Section End--%>

var POrderId = 0;
function onPrintJv(id, visibleIndex) {
    //window.location.href = "../../reports/XtraReports/Viewer/PurchaseOrderReportViewer.aspx?id=" + id;
    debugger;
    var PurOrderNumber = CgvPurchaseOrder.GetDataRow(visibleIndex).children[0].innerText;

    POrderId = id;
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
    if (cSelectPanel.cpSuccess != null) {
        var TotDocument = cSelectPanel.cpSuccess.split(',');
        var reportName = cCmbDesignName.GetValue();
        var module = 'Porder';
        window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + POrderId, '_blank')
    }
    cSelectPanel.cpSuccess = null
    if (cSelectPanel.cpSuccess == null) {
        cCmbDesignName.SetSelectedIndex(0);
    }
}
document.onkeydown = function (e) {
    if ((event.keyCode == 120 || event.keyCode == 66) && event.altKey == true) { //run code for Ctrl+B -- ie, Add Both Item
        StopDefaultAction(e);
        AddBothItem();
    }

    if ((event.keyCode == 120 || event.keyCode == 73) && event.altKey == true) { //run code for Ctrl+I -- ie, Add Inventory Item
        StopDefaultAction(e);
        AddInventoryItem();
    }

    if ((event.keyCode == 120 || event.keyCode == 78) && event.altKey == true) { //run code for Ctrl+N -- ie, Add Non-Inventory Item
        StopDefaultAction(e);
        AddNonInventoryItem();
    }

    if ((event.keyCode == 120 || event.keyCode == 67) && event.altKey == true) { //run code for Ctrl+C -- ie, Add Capital Item
        StopDefaultAction(e);
        AddCapitalItem();
    }

    if ((event.keyCode == 120 || event.keyCode == 83) && event.altKey == true) { //run code for Ctrl+C -- ie, Add Capital Item
        StopDefaultAction(e);
        AddServiceItem();
    }
}
function ShowMsgLastCall() {

    if (CgvPurchaseOrder.cpDelete != null) {

        jAlert(CgvPurchaseOrder.cpDelete)
        //CgvPurchaseOrder.PerformCallback();
        CgvPurchaseOrder.Refresh();
        CgvPurchaseOrder.cpDelete = null
    }

    if (CgvPurchaseOrder.cpMessage != null) {
        if (CgvPurchaseOrder.cpMessage == "Generated") {
            CgvPurchaseOrder.cpMessage = null;
            jAlert('Barcode generation successfully.');
        }
        else if (CgvPurchaseOrder.cpMessage == "NullStock") {
            CgvPurchaseOrder.cpMessage = null;
            jAlert('No stock available.');
        }
        else if (CgvPurchaseOrder.cpMessage == "NullBarcode") {
            CgvPurchaseOrder.cpMessage = null;
            jAlert('All Barcode generated.');
        }
        else if (CgvPurchaseOrder.cpMessage == "Error") {
            CgvPurchaseOrder.cpMessage = null;
            jAlert('Please try again later');
        }
    }
}
function StopDefaultAction(e) {
    if (e.preventDefault) { e.preventDefault() }
    else { e.stop() };

    e.returnValue = false;
    e.stopPropagation();
}

function AddServiceItem() {
    var keyOpening = document.getElementById('hdfOpening').value;
    if (keyOpening != '') {
        var url = 'PurchaseOrder.aspx?key=' + 'ADD&&op=' + 'yes&&InvType=S';
    }
    else {
        var url = 'PurchaseOrder.aspx?key=' + 'ADD&&InvType=S';
    }

    window.location.href = url;
}

function AddBothItem() {
    var keyOpening = document.getElementById('hdfOpening').value;
    if (keyOpening != '') {
        var url = 'PurchaseOrder.aspx?key=' + 'ADD&&op=' + 'yes&&InvType=B';
    }
    else {
        var url = 'PurchaseOrder.aspx?key=' + 'ADD&&InvType=B';
    }

    window.location.href = url;
}

function AddInventoryItem() {
    var keyOpening = document.getElementById('hdfOpening').value;
    if (keyOpening != '') {
        var url = 'PurchaseOrder.aspx?key=' + 'ADD&&op=' + 'yes&&InvType=Y';
    }
    else {
        var url = 'PurchaseOrder.aspx?key=' + 'ADD&&InvType=Y';
    }

    window.location.href = url;
}
function AddNonInventoryItem() {
    var keyOpening = document.getElementById('hdfOpening').value;
    if (keyOpening != '') {
        var url = 'PurchaseOrder.aspx?key=' + 'ADD&&op=' + 'yes&&InvType=N';
    }
    else {
        var url = 'PurchaseOrder.aspx?key=' + 'ADD&&InvType=N';
    }

    window.location.href = url;
}
function AddCapitalItem() {
    var keyOpening = document.getElementById('hdfOpening').value;
    if (keyOpening != '') {
        var url = 'PurchaseOrder.aspx?key=' + 'ADD&&op=' + 'yes&&InvType=C';
    }
    else {
        var url = 'PurchaseOrder.aspx?key=' + 'ADD&&InvType=C';
    }

    window.location.href = url;
}

function OnMoreInfoClick(keyValue, visibleIndex) {
    var IsBarcode = $("#hfIsBarcode").val();
    CgvPurchaseOrder.SetFocusedRowIndex(visibleIndex);
    var IsCancelFlag = CgvPurchaseOrder.GetRow(CgvPurchaseOrder.GetFocusedRowIndex()).children[12].innerText;

    if (IsCancelFlag.trim() != 'Cancelled' && IsCancelFlag.trim() != 'Closed') {

        var url = 'PurchaseOrder.aspx?key=' + keyValue + '&type=PO';
        window.location.href = url;

        //if (IsBarcode == "true") {                  
        //    jAlert("Cannot edit as barcode has already activated in the system.", "Alert", function () { });
        //}
        //else {
        //    var url = 'PurchaseOrder.aspx?key=' + keyValue + '&type=PO';
        //    window.location.href = url;
        //}
    }
    else {
        jAlert("Purchase Order is " + IsCancelFlag.trim() + ". Edit is not allowed.");
    }


}
//Mantis Issue 24920
function OnCopyClick(keyValue, visibleIndex) {
    var IsBarcode = $("#hfIsBarcode").val();
    CgvPurchaseOrder.SetFocusedRowIndex(visibleIndex);
    var IsCancelFlag = CgvPurchaseOrder.GetRow(CgvPurchaseOrder.GetFocusedRowIndex()).children[12].innerText;

    //if (IsCancelFlag.trim() != 'Cancelled' && IsCancelFlag.trim() != 'Closed') {

    //    var url = 'PurchaseOrder.aspx?key=' + keyValue + '&type=PO';
    //    window.location.href = url;

    //    //if (IsBarcode == "true") {                  
    //    //    jAlert("Cannot edit as barcode has already activated in the system.", "Alert", function () { });
    //    //}
    //    //else {
    //    //    var url = 'PurchaseOrder.aspx?key=' + keyValue + '&type=PO';
    //    //    window.location.href = url;
    //    //}
    //}
    //else {
    //    jAlert("Purchase Order is " + IsCancelFlag.trim() + ". Edit is not allowed.");
    //}
    var url = 'PurchaseOrder.aspx?key=' + keyValue + '&type=PO' + '&Copy=COPY';
    window.location.href = url;

}
//End of Mantis Issue 24920

//Add for Approve Tanmoy	
function OnApproveClick(keyValue, visibleIndex) {
    if ($("#hdnIsMultiuserApprovalRequired").val() == "Yes") {
        $.ajax({
            type: "POST",
            url: "PurchaseOrderList.aspx/PurchaseOrderApproval",
            data: JSON.stringify({ keyValue: keyValue }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {
                if (msg != null && msg == true) {
                    var IsBarcode = $("#hfIsBarcode").val();
                    CgvPurchaseOrder.SetFocusedRowIndex(visibleIndex);
                    var IsCancelFlag = CgvPurchaseOrder.GetRow(CgvPurchaseOrder.GetFocusedRowIndex()).children[12].innerText;
                    if (IsCancelFlag.trim() != 'Cancelled' && IsCancelFlag.trim() != 'Closed') {
                        if (IsBarcode == "true") {
                            //jAlert("Cannot edit as barcode has already been generated", "Alert", function () { });	
                            jAlert("Cannot approve as barcode has already activated in the system.", "Alert", function () { });
                        }
                        else {
                            var url = 'PurchaseOrder.aspx?key=' + keyValue + '&type=PO' + '&AppStat=ProjApprove';
                            window.location.href = url;
                        }
                    }
                    else {
                        jAlert("Project Purchase Order is " + IsCancelFlag.trim() + ". Edit is not allowed.");
                    }
                }
                else {
                    jAlert('You dont have permission.');
                    return false;
                }
            }
        });
    }
    else {
        var IsBarcode = $("#hfIsBarcode").val();
        CgvPurchaseOrder.SetFocusedRowIndex(visibleIndex);
        var IsCancelFlag = CgvPurchaseOrder.GetRow(CgvPurchaseOrder.GetFocusedRowIndex()).children[12].innerText;
        if (IsCancelFlag.trim() != 'Cancelled' && IsCancelFlag.trim() != 'Closed') {
            if (IsBarcode == "true") {
                //jAlert("Cannot edit as barcode has already been generated", "Alert", function () { });	
                jAlert("Cannot approve as barcode has already activated in the system.", "Alert", function () { });
            }
            else {
                var url = 'PurchaseOrder.aspx?key=' + keyValue + '&type=PO' + '&AppStat=ProjApprove';
                window.location.href = url;
            }
        }
        else {
            jAlert("Project Purchase Order is " + IsCancelFlag.trim() + ". Edit is not allowed.");
        }
    }
}
//Add for Approve Tanmoy

function OnCancelClick(keyValue, visibleIndex) {
    debugger;
    $("#hddnKeyValue").val(keyValue);
    //var IsCancel = cGrdOrder.GetRow(cGrdOrder.GetFocusedRowIndex()).children[6].innerText;

    jConfirm('Do you want to cancel the Order ?', 'Confirm Dialog', function (r) {
        if (r == true) {
            $('#MandatoryRemarksFeedback').attr('style', 'display:none;position: absolute; right: -20px; top: 8px;');
            cPopup_Feedback.Show();
        }
        else {
            return false;
        }
    });

}

function OnClosedClick(keyValue, visibleIndex) {
    $("#hddnKeyValue").val(keyValue);
    CgvPurchaseOrder.SetFocusedRowIndex(visibleIndex);
    jConfirm('Do you want to close the Order ?', 'Confirm Dialog', function (r) {
        if (r == true) {
            $('#MandatoryRemarksFeedback1').attr('style', 'display:none;position: absolute; right: -20px; top: 8px;');
            cPopup_Closed.Show();
        }
        else {
            return false;
        }
    });
}

function OnProductWiseClosedClick(keyValue, visibleIndex, PurchaseOrder) {
    $("#hddnKeyValue").val(keyValue);
    CgvPurchaseOrder.SetFocusedRowIndex(visibleIndex);
    cPopupProductwiseClose.Show();
    $("#txtVoucherNo").val(PurchaseOrder);
    cbtnCloseProduct.SetVisible(true);
    cgridProductwiseClose.PerformCallback("ShowData~" + keyValue);
}

function  CloseProduct(){
    cgridProductwiseClose.PerformCallback("CloseData");
}
function OnProductCloseEndCall()
{
    if (cgridProductwiseClose.cpProductClose == "YES") {
        jAlert("Close successfully");
        CgvPurchaseOrder.Refresh();
        cgridProductwiseClose.cpProductClose = null
        cPopupProductwiseClose.Hide();
    }
    else if(cgridProductwiseClose.cpBtnCloseVIsible == "NO") {
      
        //cbtnCloseProduct.SetVisible(false);
        cgridProductwiseClose.cpBtnCloseVIsible = "YES";
        cPopupProductwiseClose.Hide();
        jAlert("No balance quantity available for this order. Cannot proceed.");
        
    }
        
}
function OnclickViewAttachment(obj) {          
    var URL = '/OMS/Management/Activities/EntriesDocuments.aspx?idbldng=' + obj + '&type=PurchaseOrder';
    window.location.href = URL;
}

function CancelSalesOrder(keyValue, Reason) {

    $.ajax({
        type: "POST",
        url: "PurchaseOrderList.aspx/CancelPurchaseOrderOnRequest",
        data: JSON.stringify({ keyValue: keyValue, Reason: Reason }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,//Added By:Subhabrata
        success: function (msg) {
            debugger;
            var status = msg.d;
            if (status == "1") {
                jAlert("Purchase Order is cancelled successfully.");
                //cGrdOrder.PerformCallback('BindGrid');

            }
            else if (status == "-1") {
                jAlert("Purchase Order is not cancelled. Try again later");
            }
            else if (status == "-2") {
                jAlert("Selected order is tagged in other module. Cannot proceed.");
            }
            else if (status == "-3") {
                jAlert("Purchase Order is  already cancelled.");
            }
            else if (status == "-4") {
                jAlert("Order is already closed. Cannot proceed.");
            }
            else if (status == "-5") {
                jAlert("No balance quantity available for this order. Cannot proceed.");
            }
        }
    });
}
function ClosedSalesOrder(keyValue, Reason) {
    debugger;
    $.ajax({
        type: "POST",
        url: "PurchaseOrderList.aspx/ClosedPurchaseOrderOnRequest",
        data: JSON.stringify({ keyValue: keyValue, Reason: Reason }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,//Added By:Subhabrata
        success: function (msg) {
            debugger;
            var status = msg.d;
            if (status == "1") {
                jAlert("Order is closed successfully.");
                //cGrdOrder.PerformCallback('BindGrid');

            }
            else if (status == "-1") {
                jAlert("Order is not closed. Try again later");
            }
            else if (status == "-2") {
                jAlert("Selected order is tagged in other module. Cannot proceed.");
            }
            else if (status == "-3") {
                jAlert("Order is  already closed.");
            }
            else if (status == "-4") {
                jAlert("Order is already cancelled. Cannot proceed.");
            }
            else if (status == "-5") {
                jAlert("No balance quantity available for this order. Cannot proceed.");
            }
        }
    });
}
function CallFeedback_save() {
    debugger;
    var KeyVal = $("#hddnKeyValue").val();
    var flag = true;

    var Remarks = txtFeedback.GetValue();
    if (Remarks == "" || Remarks == null) {
        $('#MandatoryRemarksFeedback').attr('style', 'display:block;position: absolute; right: -20px; top: 8px;');
        flag = false;
    }
    else {
        $('#MandatoryRemarksFeedback').attr('style', 'display:none;position: absolute; right: -20px; top: 8px;');
        cPopup_Feedback.Hide();
        CancelSalesOrder(KeyVal, Remarks);
        CgvPurchaseOrder.Refresh();
        //cGrdOrder.PerformCallback('Feedback~' + KeyVal + '~' + Remarks);
        //Grid.Refresh();


    }
    return flag;
}


function CancelFeedback_save() {


    txtFeedback.SetValue();
    cPopup_Feedback.Hide();
    //$('#chkmailfeedback').prop('checked', false);
}
function CallClosed_save() {
    debugger;
    var KeyVal = $("#hddnKeyValue").val();
    var flag = true;

    var Remarks = txtClosed.GetValue();
    if (Remarks == "" || Remarks == null) {
        $('#MandatoryRemarksFeedback1').attr('style', 'display:block;position: absolute; right: -20px; top: 8px;');
        flag = false;
    }
    else {
        $('#MandatoryRemarksFeedback1').attr('style', 'display:none;position: absolute; right: -20px; top: 8px;');
        cPopup_Closed.Hide();
        ClosedSalesOrder(KeyVal, Remarks);
        CgvPurchaseOrder.Refresh();
        //cGrdOrder.PerformCallback('Feedback~' + KeyVal + '~' + Remarks);
        //Grid.Refresh();


    }
    return flag;
}
function CancelClosed_save() {
    txtClosed.SetValue();
    cPopup_Closed.Hide();
    //$('#chkmailfeedback').prop('checked', false);
}
////##### coded by Samrat Roy - 17/04/2017 - ref IssueLog(P Order - 70) 
////Add an another param to define request type 
function OnViewClick(keyValue) {
    var url = 'PurchaseOrder.aspx?key=' + keyValue + '&req=V' + '&type=PO';
    window.location.href = url;
}
function OnClickDelete(keyValue, visibleIndex) {
    CgvPurchaseOrder.SetFocusedRowIndex(visibleIndex);
    var IsCancelFlag = CgvPurchaseOrder.GetRow(CgvPurchaseOrder.GetFocusedRowIndex()).children[12].innerText;
    if (IsCancelFlag.trim() != 'Cancelled' && IsCancelFlag.trim() != 'Closed') {
        jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
            if (r == true) {
                CgvPurchaseOrder.PerformCallback('Delete~' + keyValue);
            }
        });
    }
    else {
        jAlert("Purchase Order is " + IsCancelFlag.trim() + ".Delete is not allowed.");
    }
}
function GenerateBarcode() {
    var visibleIndex = CgvPurchaseOrder.GetFocusedRowIndex();
    var key = CgvPurchaseOrder.GetRowKey(visibleIndex);

    console.log(key);
    GetObjectID('hdfDocNumber').value = key;

    if (visibleIndex != -1) {

        $.ajax({
            type: "POST",
            url: "PurchaseOrderList.aspx/GetType",
            data: JSON.stringify({ keyValue: key }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {
                var returnObject = msg.d;
                if (returnObject != "N") {
                    $.ajax({
                        type: "POST",
                        url: "PurchaseOrderList.aspx/GetStockSerial",
                        data: JSON.stringify({ keyValue: key }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        success: function (msg) {
                            var returnObject = msg.d;
                            var IsSerialActive = returnObject.split('~')[0];
                            var IsBarcodeActive = returnObject.split('~')[1];

                            if (IsSerialActive > 0 && IsBarcodeActive > 0) {
                                cPopupWarehouse.Show();
                                cDocumentPanel.PerformCallback('GetDocumentDetails~' + key);
                            }
                            else {
                                jAlert('No Barcode Activated Product(s) Found');
                            }
                        }

                    });
                }
                else {
                    jAlert('No Barcode Activated Product(s) Found');
                }


            }
        });
    }
    else {
        jAlert('No data available.');
    }
}
function PrintBarcode() {
    //LoadingPanel.Show();
    //LoadingPanel.SetText("Please wait...");
    var visibleIndex = CgvPurchaseOrder.GetFocusedRowIndex();
    var key = CgvPurchaseOrder.GetRowKey(visibleIndex);


    $.ajax({
        type: "POST",
        url: "PurchaseOrderList.aspx/GetType",
        data: JSON.stringify({ keyValue: key }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            var returnObject = msg.d;
            if (returnObject != "N") {
                $.ajax({
                    type: "POST",
                    url: "PurchaseOrderList.aspx/GetStockSerial",
                    data: JSON.stringify({ keyValue: key }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (msg) {

                        var returnObject = msg.d;
                        var IsSerialActive = returnObject.split('~')[0];
                        var IsBarcodeActive = returnObject.split('~')[1];

                        if (IsSerialActive > 0 && IsBarcodeActive > 0) {
                            var visibleIndex = CgvPurchaseOrder.GetFocusedRowIndex();
                            var key = CgvPurchaseOrder.GetRowKey(visibleIndex);
                            var Branch = ccmbBranchfilter.GetValue();
                            var doctype = 'PO';
                            var module = 'BRCODE';
                            var reportName = 'Barcode~D';
                            window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + "&modulename=BRCODE&id=" + key + "&doctype=" + doctype + "&Branch=" + Branch, '_blank')

                        }
                        else {
                            jAlert('No Barcode Activated Product(s) Found');
                        }
                    }
                });
            }
            else {
                jAlert('No Barcode Activated Product(s) Found');
            }


        }
    });

}
function GenerateProductBarcode() {
    var Qty = ctxtQty.GetValue();

    cDocumentPanel.PerformCallback('GenerateBarcode~' + Qty);
}
function OnRowClick(s, e) {
    var visibleIndex = e.visibleIndex;
    var key = cDocumentGrid.GetRowKey(visibleIndex);
    GetObjectID('hdfDocDetailsNumber').value = key;

    if (visibleIndex != -1) {
        cDocumentPanel.PerformCallback('GetDocumentStock');
    }
    else {
        jAlert('No data available.');
    }
}
function OnDetailsRowClick(s, e) {
    var visibleIndex = e.visibleIndex;
    var key = cStockGrid.GetRowKey(visibleIndex);
    GetObjectID('hdfMapID').value = key;
}
function SaveSerial() {
    var MapID = GetObjectID('hdfMapID').value;
    var Serial = cSerialNo.GetValue();

    //cDocumentPanel.PerformCallback('SaveSerial');
    cDocumentPanel.PerformCallback('UpdateSerial');
}
function DocumentPanelEndCall(s, e) {
    if (cDocumentPanel.cpDocDetails) {
        var DocDetails = cDocumentPanel.cpDocDetails;
        cDocumentPanel.cpDocDetails = null;

        var DocNumber = DocDetails.split('~')[0];
        var Vendor = DocDetails.split('~')[1];
        var Branch = DocDetails.split('~')[2];
        var BranchID = DocDetails.split('~')[3];

        ctxtDocNumber.SetValue(DocNumber);
        ctxtVendor.SetValue(Vendor);
        ctxtBatch.SetValue(Branch);
        GetObjectID('hdfBranch').value = BranchID;

        ctxtProduct.SetValue("");
        ctxtQty.SetValue("0");
        ctxtQty.SetMaxValue("0");
        ctxtQty.SetMinValue("0");
    }

    if (cDocumentPanel.cpStockDetails) {
        var DocDetails = cDocumentPanel.cpStockDetails;
        cDocumentPanel.cpStockDetails = null;

        var Products_Name = DocDetails.split('~')[0];
        var Quantity = DocDetails.split('~')[1];

        ctxtProduct.SetValue(Products_Name);
        ctxtQty.SetValue(Quantity);
        ctxtQty.SetMaxValue(Quantity);
        ctxtQty.SetMinValue("0");
    }

}
function gridRowclick(s, e) {
    $('#Grid_PurchaseOrder').find('tr').removeClass('rowActive');
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
    $("#expandCgvPurchaseOrder").click(function (e) {
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


            CgvPurchaseOrder.SetHeight(browserHeight - 150);
            CgvPurchaseOrder.SetWidth(cntWidth);
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


            CgvPurchaseOrder.SetHeight(450);

            var cntWidth = $this.parent('.makeFullscreen').width();
            CgvPurchaseOrder.SetWidth(cntWidth);

        }

    });
});

//Mantis Issue 25152
function onSmsClickJv(key) {
    $("#assignEmployee").modal('show');
    BindModalEmployee();
    //PIndentId = key;
    $("#hdnOrderId").val(key);
}
function PhoneNoSend() {

    $("#assignEmployee").modal('hide');
    var EmployeeId = $("#ddl_DirEmployee").val();
    if (EmployeeId == "0") {
        jAlert("please select an Employee.");
        return false;
    }
    //var PIndentId = $("#hdnOrderId").val();
    jConfirm('Send SMS?', 'Alert', function (r) {
        if (r) {
            $.ajax({
                type: "POST",
                url: "PurchaseOrderList.aspx/SendSMSManualNo",
                data: JSON.stringify({ POrderId: $("#hdnOrderId").val(), EmployeeId: $("#ddl_DirEmployee").val() }),
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    //console.log(response);
                    if (response.d) {
                        if (response.d == "true") {
                            jAlert("SMS sent Successfully.", "Alert", function () {
                            });
                        }
                        else {
                            jAlert(response.d);
                            return
                        }
                    }
                },
                error: function (response) {
                    console.log(response);
                }
            });
        }
    });
}
function BindModalEmployee() {
    var det = {};
    det.DBName = $('#hdDbName').val();
    var $select = $('#ddl_DirEmployee');
    $select.empty();
    //$select.append("<option value='0'>--Select--</option>");
    $.ajax({
        type: "POST",
        url: 'PurchaseOrderList.aspx/AddModalEmployee',
        contentType: "application/json; charset=utf-8",
        async: false,
        dataType: "json",
        data: JSON.stringify(det),
        success: function (data) {
            var arr = data.d;
            console.log(arr)

            var htm = '';
            for (var i = 0; i < arr.length; i++) {
                htm += '<option value="' + arr[i].cnt_internalId + '">' + arr[i].DirectorName + '</option>'
            }
            $('#ddl_DirEmployee').html(htm)
            //$('<option>', {
            //    value: data.d.cnt_internalId
            //}).append(data.d.DirectorName).appendTo($select);
            //alert(data.d[0].cnt_internalId)
            //$.each(data.d, function (i, data) {
            //    alert(data.d[i].cnt_internalId)
            //    $('<option>', {
            //        value: data.d[i].cnt_internalId
            //    }).append(data.d[i].DirectorName).appendTo($select);
            //});

        },
        error: function (mydata) { alert("error"); alert(mydata); },

    });
}
//End of Mantis Issue 25152
