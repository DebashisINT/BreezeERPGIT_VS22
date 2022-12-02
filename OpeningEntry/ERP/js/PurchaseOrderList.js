

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
    uri = "PurchaseOrder.aspx?key=" + obj + "&status=2";
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
    uri = "PurchaseOrder.aspx?key=" + obj + "&status=3";
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
           
        }
    });
}

function onPrintJv(id) {
    window.location.href = "../../reports/XtraReports/Viewer/PurchaseOrderReportViewer.aspx?id=" + id;
}

document.onkeydown = function (e) {
    // if (event.keyCode == 18) isCtrl = true;
    if ((event.keyCode == 120 || event.keyCode == 65) && event.altKey == true) { //run code for Ctrl+S -- ie, Save & New  
        StopDefaultAction(e);
        AddButtonClick();
    }
}
function ShowMsgLastCall() {

    if (CgvPurchaseOrder.cpDelete != null) {

        jAlert(CgvPurchaseOrder.cpDelete)
        CgvPurchaseOrder.PerformCallback();
        CgvPurchaseOrder.cpDelete = null
    }
}
function StopDefaultAction(e) {
    if (e.preventDefault) { e.preventDefault() }
    else { e.stop() };

    e.returnValue = false;
    e.stopPropagation();
}

function AddButtonClick() {
    var keyOpening = document.getElementById('hdfOpening').value;
    if (keyOpening != '')
    {
        var url = 'PurchaseOrder.aspx?key=' + 'ADD&&op=' + 'yes';
    }
    else {
        var url = 'PurchaseOrder.aspx?key=' + 'ADD';
    }
          
    window.location.href = url;
}
function OnMoreInfoClick(keyValue) {
    var url = 'PurchaseOrder.aspx?key=' + keyValue + '&type=PO';
    window.location.href = url;
}
////##### coded by Samrat Roy - 17/04/2017 - ref IssueLog(P Order - 70) 
////Add an another param to define request type 
function OnViewClick(keyValue) {
    var url = 'PurchaseOrder.aspx?key=' + keyValue + '&req=V';
    window.location.href = url;
}
function OnClickDelete(keyValue) {
    jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
        if (r == true) {
            CgvPurchaseOrder.PerformCallback('Delete~' + keyValue);
        }
    });
}
