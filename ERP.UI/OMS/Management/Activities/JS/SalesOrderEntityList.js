
function OnProductWiseClosedClick(keyValue, visibleIndex, PurchaseOrder) {
    $("#hddnKeyValue").val(keyValue);
    cGrdOrder.SetFocusedRowIndex(visibleIndex);
    cPopupProductwiseClose.Show();
    $("#txtVoucherNo").val(PurchaseOrder);
    cbtnCloseProduct.SetVisible(true);
    cgridProductwiseClose.PerformCallback("ShowData~" + keyValue);
}

function CloseProduct() {
    cgridProductwiseClose.PerformCallback("CloseData");
}
function OnProductCloseEndCall() {
    if (cgridProductwiseClose.cpProductClose == "YES") {
        jAlert("Close successfully");
        cGrdOrder.Refresh();
        cgridProductwiseClose.cpProductClose = null
        cPopupProductwiseClose.Hide();
    }
    else if (cgridProductwiseClose.cpBtnCloseVIsible == "NO") {

        //cbtnCloseProduct.SetVisible(false);
        cgridProductwiseClose.cpBtnCloseVIsible = "YES";
        cPopupProductwiseClose.Hide();
        jAlert("No balance quantity available for this order. Cannot proceed.");
    }

}
        function timerTick() {
            $.ajax({
                type: "POST",
                url: "SalesOrderEntityList.aspx/GetTotalWatingOrderCount",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    //debugger;
                    var status = msg.d;
                    //console.log(status);
                    ClblQuoteweatingCount.SetText(status);
                    var fetcheddata = parseFloat(document.getElementById('waitingOrderCount').value);
                    if (status != fetcheddata) {
                        CwatingQuotegrid.Refresh();
                        document.getElementById('waitingOrderCount').value = status;
                    }
                }
            });
        }
function OrderWattingOkClick() {
    var index = CwatingOrdergrid.GetFocusedRowIndex();
    var listKey = CwatingOrdergrid.GetRowKey(index);
    if (listKey) {
        if (CwatingOrdergrid.GetRow(index).children[6].innerText != "Advance") {
            var url = 'SalesOrderAdd.aspx?key=' + 'ADD&&BasketId=' + listKey;
            window.location.href = url;
        } else {
            // ShowbasketReceiptPayment(listKey);
        }
    }
}
function OnWaitingGridKeyPress(e) {

    if (e.code == "Enter") {
        var index = CwatingOrdergrid.GetFocusedRowIndex();
        var listKey = CwatingOrdergrid.GetRowKey(index);
        if (listKey) {
            if (CwatingOrdergrid.GetRow(index).children[6].innerText != "Advance") {
                var url = 'SalesOrderAdd.aspx?key=' + 'ADD&&BasketId=' + listKey;
                // LoadingPanel.Show();
                window.location.href = url;
            } else {
                //ShowbasketReceiptPayment(listKey);
            }
        }
    }

}

function OpenWaitingOrder() {
    CwatingOrdergrid.PerformCallback();
    Cpopup_OrderWait.Show();
    CwatingOrdergrid.Focus();
}

function ListRowClicked(s, e) {

    var index = e.visibleIndex;
    var listKey = CwatingOrdergrid.GetRowKey(index);
    if (e.htmlEvent.target.id != "CloseRemoveWattingBtn") {
        if (CwatingOrdergrid.GetRow(index).children[6].innerText != "Advance") {
            var url = 'SalesOrderAdd.aspx?key=' + 'ADD&&BasketId=' + listKey;
            //LoadingPanel.Show();
            window.location.href = url;
        } else {
            // ShowbasketReceiptPayment(listKey);
        }
    }
}


function watingOrdergridEndCallback() {
    if (CwatingOrdergrid.cpReturnMsg) {
        if (CwatingOrdergrid.cpReturnMsg != "") {
            jAlert(CwatingOrdergrid.cpReturnMsg);
            document.getElementById('waitingOrderCount').value = parseFloat(document.getElementById('waitingOrderCount').value) - 1;
            CwatingOrdergrid.cpReturnMsg = null;
        }
    }
}

function OpenPopUPUserWiseQuotaion() {
    // cgridUserWiseQuotation.PerformCallback();
    $("#hdnIsFilter").val("Y");
    
    cgridUserWiseQuotation.Refresh();
    cPopupUserWiseQuotation.Show();
}
// function above  End

//This function is called to show all Pending Approval of Sales Order whose Userid has been set LevelWise using Approval Configuration Module 
function OpenPopUPApprovalStatus() {
   // cgridPendingApproval.PerformCallback();
   // clookup_PendingApproval.gridView.Refresh();
   // $("#hdnIsFilter").val("Y");
   // cgridPendingApproval.Refresh();
    //cpopupApproval.Show();
    //clookup_PendingApproval.ShowDropDown();

    $('#popupApprovalModel').modal('show');
    PendingApproval();
}

function PendingApproval() {
    LoadingPanel.Show();
    //$("#hdnSearchType").val("PendingApproval");
    //$('.circulerUl>li').removeClass('activeCrcl').addClass('ds');
    //$('#d1').addClass('activeCrcl');

    $.ajax({
        type: "POST",
        url: "SalesOrderEntityList.aspx/PendingApproval_List",
     /*   data: JSON.stringify({ model: $("#hdnUserType").val(), SearchType: "PendingApproval" }),*/
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {           

            var status = "";
            status = status + "<table id='dataTable' class='table table-striped table-bordered display nowrap' style='width: 100%'>";
            status = status + " <thead><tr>";
            status = status + " <th>Document No.</th><th>Party Name</th><th>Posting Date</th><th>Unit</th>";
            status = status + " <th>Entered By</th><th>Approved </th><th>Rejected </th>";           
            status = status + " </tr></thead>";

            status = status + " </table>";

            $('#divListData').html(status);

            $('#dataTable').DataTable({
                scrollX: true,
                fixedColumns: {
                    rightColumns: 2
                },
                data: msg.d.DetailsList,
                columns: [
                    { 'data': 'DocumentNo' },
                    { 'data': 'PartyName' },
                    { 'data': 'PostingDate' },
                    { 'data': 'Unit' },
                    { 'data': 'EnteredBy' },
                    { 'data': 'Approved' },
                    { 'data': 'Rejected' },
                   
                ],
                dom: 'Bfrtip',
                //buttons: [
                //    {
                //        extend: 'excel',
                //        title: null,
                //        filename: 'Service Entry',
                //        text: 'Save as Excel',
                //        customize: function (xlsx) {
                //            var sheet = xlsx.xl.worksheets['sheet1.xml'];
                //            $('row:first c', sheet).attr('s', '42');
                //        },

                //        exportOptions: {
                //        }
                //    }
                //],
                error: function (error) {
                    alert(error);
                    LoadingPanel.Hide();
                }
            });
            LoadingPanel.Hide();
        }
    });
}
function OnGetApprovedRowValues(obj) {
    uri = "SalesOrderAdd.aspx?key=" + obj + "&status=2" + '&type=SO' + '&isformApprove=YES' + '&UpperApprove=UpApprove';
    popup.SetContentUrl(uri);
    popup.Show();
}




// function above  End


// Status 2 is passed If Approved Check box is checked by User Both Below function is called and used to show in POPUP,  the Add Page of Respective Segment(like Page for Adding Quotation ,Sale Order ,Challan)
function GetApprovedQuoteId(s, e, itemIndex) {
   var rowvalue = cgridPendingApproval.GetRowValues(itemIndex, 'ID', OnGetApprovedRowValues);
    //var rowvalue = clookup_PendingApproval.gridView.GetRowValues(itemIndex, 'ID', OnGetApprovedRowValues);

}
function OnGetApprovedRowValues(obj) {
    uri = "SalesOrderAdd.aspx?key=" + obj + "&status=2" + '&type=SO' + '&isformApprove=YES' + '&UpperApprove=UpApprove';
    popup.SetContentUrl(uri);
    popup.Show();
}
function closeUserApproval() {
    popup.Hide();
  
    //clookup_PendingApproval.ShowDropDown();
}
// function above  End For Approved

// Status 3 is passed If Approved Check box is checked by User Both Below function is called and used to show in POPUP,  the Add Page of Respective Segment(like Page for Adding Quotation ,Sale Order ,Challan)
function GetRejectedQuoteId(s, e, itemIndex) {
    
    cgridPendingApproval.GetRowValues(itemIndex, 'ID', OnGetRejectedRowValues);
    //clookup_PendingApproval.gridView.GetRowValues(itemIndex, 'ID', OnGetRejectedRowValues);

}
function OnGetRejectedRowValues(obj) {
    uri = "SalesOrderAdd.aspx?key=" + obj + "&status=3" + '&type=SO' + '&UpperApprove=UpApprove';
    popup.SetContentUrl(uri);
    popup.Show();
}
// function above  End For Rejected

// To Reflect the Data in Pending Waiting Grid and Pending Waiting Counting if the user approve or Rejecte the Order and Saved 

function OnApprovalEndCall(s, e) {
    $.ajax({
        type: "POST",
        url: "SalesOrderList.aspx/GetPendingCase",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $('#lblWaiting').text(data.d);
        }
    });
    PendingApproval();
}
// function above  End 
//<%-- Code Added By Sandip For Approval Detail Section End--%>
//<%-- Code Added By Debashis Talukder For Document Printing End--%>
    var SOrderId = 0;
function onPrintJv(id) {
    //window.location.href = "../../reports/XtraReports/Viewer/PurchaseOrderReportViewer.aspx?id=" + id;
    //debugger;
    SOrderId = id;
    cDocumentsPopup.Show();
    cCmbDesignName.SetSelectedIndex(0);
    cSelectPanel.PerformCallback('Bindalldesignes');
    $('#btnOK').focus();
}



var InvoiceId = 0;
var Type = '';
function onPrintSi(id, type) {
    //debugger;
    Type = type;
    InvoiceId = id;
    cInvoiceSelectPanel.cpSuccess = "";
    cInvoiceDocumentsPopup.Show();
    CselectOriginal.SetCheckState('UnChecked');
    CselectDuplicate.SetCheckState('UnChecked');
    CselectTriplicate.SetCheckState('UnChecked');
    CselectOfficecopy.SetCheckState('UnChecked');
    cInvoiceCmbDesignName.SetSelectedIndex(0);
    cInvoiceSelectPanel.PerformCallback('Bindalldesignes' + '~' + Type);
    $('#btnInvoiceOK').focus();
}



function PerformCallToGridBind() {
    cSelectPanel.PerformCallback('Bindsingledesign');
    cDocumentsPopup.Hide();
    return false;
}


function PerformInvoiceCallToGridBind() {
    //cgriddocuments.PerformCallback('BindDocumentsGridOnSelection' + '~' + InvoiceId);
    //cDocumentsPopup.Hide();
    //return false;
    cInvoiceSelectPanel.PerformCallback('Bindsingledesign' + '~' + Type);
    cInvoiceDocumentsPopup.Hide();
    return false;
}
var isFirstTime = true;
function AllControlInitilize() {
    //debugger;
    if (isFirstTime) {

        if (localStorage.getItem('FromDateSalesOrder')) {
            var fromdatearray = localStorage.getItem('FromDateSalesOrder').split('-');
            var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
            cFormDate.SetDate(fromdate);
        }
        if (localStorage.getItem('ToDateSalesOrder')) {
            var todatearray = localStorage.getItem('ToDateSalesOrder').split('-');
            var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
            ctoDate.SetDate(todate);
        }
        if (localStorage.getItem('OrderBranch')) {
            if (ccmbBranchfilter.FindItemByValue(localStorage.getItem('OrderBranch'))) {
                ccmbBranchfilter.SetValue(localStorage.getItem('OrderBranch'));
            }

        }
        //updateGridByDate();

        isFirstTime = false;
    }
}

//Function for Date wise filteration
function updateGridByDate() {
    //debugger;
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
        localStorage.setItem("FromDateSalesOrder", cFormDate.GetDate().format('yyyy-MM-dd'));
        localStorage.setItem("ToDateSalesOrder", ctoDate.GetDate().format('yyyy-MM-dd'));
        localStorage.setItem("OrderBranch", ccmbBranchfilter.GetValue());
        //cGrdOrder.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue());
        $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
        $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
        $("#hfBranchID").val(ccmbBranchfilter.GetValue());
        $("#hfIsFilter").val("Y");
      //  cGrdOrder.Refresh();
        $("#hFilterType").val("All");       
        cCallbackPanel.PerformCallback("");

    }
}
//End
function CallbackPanelEndCall(s, e) {
    cGrdOrder.Refresh();
}
function cSelectPanelEndCall(s, e) {
    //debugger;
    if (cSelectPanel.cpSuccess != null) {
        var TotDocument = cSelectPanel.cpSuccess.split(',');
        var reportName = cCmbDesignName.GetValue();
        var module = 'Sorder';
        window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + SOrderId, '_blank')
    }
    cSelectPanel.cpSuccess = null
    if (cSelectPanel.cpSuccess == null) {
        cCmbDesignName.SetSelectedIndex(0);
    }
}



function cInvoiceSelectPanelEndCall(s, e) {
    //debugger;
    if (cInvoiceSelectPanel.cpSuccess != "") {
        var TotDocument = cInvoiceSelectPanel.cpSuccess.split(',');
        var reportName = cInvoiceCmbDesignName.GetValue();
        if (cInvoiceSelectPanel.cpType == "SI") {
            var module = 'Invoice';
        }
        else {
            var module = 'TSInvoice';
        }

        if (TotDocument.length > 0) {
            for (var i = 0; i < TotDocument.length; i++) {
                if (TotDocument[i] != "") {
                    window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + InvoiceId + '&PrintOption=' + TotDocument[i], '_blank')
                }
            }
        }
    }
    //cSelectPanel.cpSuccess = null
    if (cInvoiceSelectPanel.cpSuccess == "") {
        if (cInvoiceSelectPanel.cpChecked != "") {
            jAlert('Please check Original For Recipient and proceed.');
        }
        //CselectDuplicate.SetEnabled(false);
        //CselectTriplicate.SetEnabled(false);
        CselectOriginal.SetCheckState('UnChecked');
        CselectDuplicate.SetCheckState('UnChecked');
        CselectTriplicate.SetCheckState('UnChecked');
        CselectOfficecopy.SetCheckState('UnChecked');
        cInvoiceCmbDesignName.SetSelectedIndex(0);
    }
}
//<%-- Code Added By Debashis Talukder For Document Printing End--%>
document.onkeydown = function (e) {
    var isCtrl = false;
    if (event.keyCode == 18) isCtrl = true;


    if (event.keyCode == 65 && isCtrl == true) { //run code for alt+a -- ie, Add
        StopDefaultAction(e);
        OnAddButtonClick();
    }
    //Subhabrata cancel close hotkey

    var CancelCloseFlag = $("#hddnCancelCloseFlag").val();

    if (event.keyCode == 83 && isCtrl == true && CancelCloseFlag.trim() == 'CA') {
        CallFeedback_save();
    }
    if (event.keyCode == 67 && isCtrl == true && CancelCloseFlag.trim() == 'CA') {
        CancelFeedback_save();
    }
    if (event.keyCode == 83 && isCtrl == true && CancelCloseFlag.trim() == 'CL') {
        CallClosed_save();
    }
    if (event.keyCode == 67 && isCtrl == true && CancelCloseFlag.trim() == 'CL') {
        CancelClosed_save();
    }
    //End

}

function StopDefaultAction(e) {
    if (e.preventDefault) { e.preventDefault() }
    else { e.stop() };

    e.returnValue = false;
    e.stopPropagation();
}
function OnClickDelete(keyValue, visibleIndex) {
    cGrdOrder.SetFocusedRowIndex(visibleIndex);
    //var IsBalMapQtyExists = cGrdOrder.GetRow(cGrdOrder.GetFocusedRowIndex()).children[18].innerText;
    //if (IsBalMapQtyExists.trim() != "0") {
    var IsCancelFlag = cGrdOrder.GetRow(cGrdOrder.GetFocusedRowIndex()).children[12].innerText;
    if (IsCancelFlag.trim() != 'Cancelled' && IsCancelFlag.trim() != 'Closed') {

        $.ajax({
            type: "POST",
            url: "SalesOrderList.aspx/GetSalesInvoiceIsExistInSalesInvoice",
            data: "{'keyValue':'" + keyValue + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,//Added By:Subhabrata
            success: function (msg) {
                //debugger;
                var status = msg.d;
                if (status == "1") {
                    jAlert('Used in other module(s). Cannot Delete.', 'Confirmation Dialog', function (r) {
                        if (r == true) {
                            return false;
                        }
                    });
                }
                else {
                    jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                        if (r == true) {
                            cGrdOrder.PerformCallback('Delete~' + keyValue);
                        }
                    });
                }
            }
        });
    }
    else {
        jAlert("Sales Order is " + IsCancelFlag.trim() + ".Delete is not allowed.");
    }
    //}
    //else
    //{
    //    jAlert("Sales Order is tagged with other module.Delete is not allowed.");
    //}
}

function OnclickViewAttachment(obj) {
    //var URL = '/OMS/Management/Activities/SalesOrder_Document.aspx?idbldng=' + obj + '&type=SalesOrder';
    var URL = '/OMS/Management/Activities/EntriesDocuments.aspx?idbldng=' + obj + '&type=SalesOrder';
    window.location.href = URL;
}
function OnClickStatus(keyValue) {
    GetObjectID('hiddenedit').value = keyValue;
    cGrdOrder.PerformCallback('Edit~' + keyValue);
}
function grid_EndCallBack() {
    if (cGrdOrder.cpEdit != null) {
        GetObjectID('hiddenedit').value = cGrdOrder.cpEdit.split('~')[0];
        cProforma.SetText(cGrdOrder.cpEdit.split('~')[1]);
        cCustomer.SetText(cGrdOrder.cpEdit.split('~')[4]);
        var pro_status = cGrdOrder.cpEdit.split('~')[2]
        if (pro_status != null) {
            var radio = $("[id*=rbl_OrderStatus] label:contains('" + pro_status + "')").closest("td").find("input");
            radio.attr("checked", "checked");
            //return false;
            //$('#rbl_QuoteStatus[type=radio][value=' + pro_status + ']').prop('checked', true); 
            cOrderRemarks.SetText(cGrdOrder.cpEdit.split('~')[3]);
            cOrderStatus.Show();
        }
    }
    if (cGrdOrder.cpDelete != null) {
        jAlert(cGrdOrder.cpDelete);
        cGrdOrder.cpDelete = null;
        //cGrdOrder.Refresh();
        updateGridByDate();
    }
}
function SavePrpformaStatus() {
    if (document.getElementById('hiddenedit').value == '') {
        cGrdOrder.PerformCallback('save~');
    }
    else {
        cGrdOrder.PerformCallback('update~' + GetObjectID('hiddenedit').value);
    }

}



////##### coded by Samrat Roy - 04/05/2017  
////Add an another param to define request type 
function OnViewClick(keyValue) {
    var url = 'SalesOrderAdd.aspx?key=' + keyValue + '&req=V' + '&type=SO';
    window.location.href = url;
}

function OnCancelClick(keyValue, visibleIndex) {
    //debugger;
    $("#hddnKeyValue").val(keyValue);
    $("#hddnCancelCloseFlag").val('CA');
    cGrdOrder.SetFocusedRowIndex(visibleIndex);
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
    //debugger;
    $("#hddnKeyValue").val(keyValue);
    $("#hddnCancelCloseFlag").val('CL');
    cGrdOrder.SetFocusedRowIndex(visibleIndex);
    var IsCancel = cGrdOrder.GetRow(cGrdOrder.GetFocusedRowIndex()).children[6].innerText;

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

function CallFeedback_save() {
    // debugger;
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
        cGrdOrder.Refresh();
        //cGrdOrder.PerformCallback('Feedback~' + KeyVal + '~' + Remarks);
        //Grid.Refresh();


    }
    return flag;

}

function CallClosed_save() {
    //debugger;
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
        cGrdOrder.Refresh();
        //cGrdOrder.PerformCallback('Feedback~' + KeyVal + '~' + Remarks);
        //Grid.Refresh();


    }
    return flag;

}

function CancelSalesOrder(keyValue, Reason) {
    //debugger;
    $.ajax({
        type: "POST",
        url: "SalesOrderEntityList.aspx/CancelSalesOrderOnRequest",
        data: JSON.stringify({ keyValue: keyValue, Reason: Reason }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,//Added By:Subhabrata
        success: function (msg) {
            //debugger;
            var status = msg.d;
            if (status == "1") {
                jAlert("Order is cancelled successfully.");
                //cGrdOrder.PerformCallback('BindGrid');


            }
            else if (status == "-1") {
                jAlert("Order is not cancelled.Try again later");
            }
            else if (status == "-2") {
                jAlert("Selected order is tagged in other module. Cannot proceed.");
            }
            else if (status == "-3") {
                jAlert("Order is  already cancelled.");
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
    //debugger;
    $.ajax({
        type: "POST",
        url: "SalesOrderEntityList.aspx/ClosedSalesOrderOnRequest",
        data: JSON.stringify({ keyValue: keyValue, Reason: Reason }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,//Added By:Subhabrata
        success: function (msg) {
            //debugger;
            var status = msg.d;
            if (status == "1") {
                jAlert("Order is closed successfully.");
                //cGrdOrder.PerformCallback('BindGrid');

            }
            else if (status == "-1") {
                jAlert("Order is not closed.Try again later");
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




function CancelFeedback_save() {

   
     txtFeedback.SetValue();
    cPopup_Feedback.Hide();
    //$('#chkmailfeedback').prop('checked', false);
}

function CancelClosed_save() {

    txtClosed.SetValue();
    cPopup_Closed.Hide();
    //$('#chkmailfeedback').prop('checked', false);
}




function OnAddButtonClick() {
    var url = 'SalesOrderAdd.aspx?key=' + 'ADD';
    window.location.href = url;
}
//});
$(document).ready(function () {

});




function OnAddEditClick(e, obj) {

    var data = obj.split('~');
    cInvoiceNumberpopup.Show();
    popInvoiceNumberPanel.PerformCallback(obj);

}


function OnAddEditDateClick(e, obj) {
    var data = obj.split('~');

    //if (data[2] == 'Multiple') {
    cInvoiceDatepopup.Show();
    popInvoiceDatePanel.PerformCallback(obj);
    // }

}
function gridRowclick(s, e) {
    //alert('hi');
    $('#GrdOrder').find('tr').removeClass('rowActive');
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
                //console.log(value);
                $(value).css({ 'opacity': '1' });
            }, 100);
        });
    }, 200);
}

    $(document).ready(function () {
        $('.navbar-minimalize').click(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                cGrdOrder.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                cGrdOrder.SetWidth(cntWidth);
            }
        });
    });

