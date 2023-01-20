//==========================================================Revision History ============================================================================================
//    1.0   Priti   V2.0.36   19 - 01 - 2023    0025371: Listing view upgradation required of Branch Requisition of Inventory
//========================================== End Revision History =======================================================================================================

function OnClosedClick(keyValue, visibleIndex) {
    $("#hddnKeyValue").val(keyValue);
    CgvPurchaseIndent.SetFocusedRowIndex(visibleIndex);
    jConfirm('Do you want to close the Branch Requisition ?', 'Confirm Dialog', function (r) {
        if (r == true) {
            $('#MandatoryRemarksFeedback1').attr('style', 'display:none;position: absolute; right: -20px; top: 8px;');
            cPopup_Closed.Show();
        }
        else {
            return false;
        }
    });
}
function CallClosed_save() {
    
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
        CgvPurchaseIndent.Refresh();
    }
    return flag;
}
function ClosedSalesOrder(keyValue, Reason) {
   
    $.ajax({
        type: "POST",
        url: "BranchRequisition.aspx/ClosedBranchRequisitionOnRequest",
        data: JSON.stringify({ keyValue: keyValue, Reason: Reason }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
           
            var status = msg.d;
            if (status == "1") {
                jAlert("Branch Requisition is closed successfully.");

            }
            else if (status == "-1") {
                jAlert("Branch Requisition is not closed. Try again later");
            }
            else if (status == "-2") {
                jAlert("Selected Branch Requisition is tagged in other module. Cannot proceed.");
            }
            else if (status == "-3") {
                jAlert("Branch Requisition is  already closed.");
            }
            else if (status == "-4") {
                jAlert("Order is already cancelled. Cannot proceed.");
            }
            else if (status == "-5") {
                jAlert("No balance quantity available for this Branch Requisition. Cannot proceed.");
            }
        }
    });
}

function CancelClosed_save() {
    txtClosed.SetValue();
    cPopup_Closed.Hide();
    
}



function clookup_Project_LostFocus() {
    //grid.batchEditApi.StartEdit(-1, 2);

    var projID = clookup_Project.GetValue();
    if (projID == null || projID == "") {
        $("#ddlHierarchy").val(0);
    }
}

function ProjectValueChange(s, e) {

    var projID = clookup_Project.GetValue();

    $.ajax({
        type: "POST",
        url: 'BranchRequisition.aspx/getHierarchyID',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ ProjID: projID }),
        success: function (msg) {
            var data = msg.d;
            $("#ddlHierarchy").val(data);
        }
    });
}


var isFirstTime = true;
function AllControlInitilize() {
    ///  document.getElementById('AddButton').style.display = 'inline-block';
    if (isFirstTime) {

        if (localStorage.getItem('BrReqFromDate')) {
            var fromdatearray = localStorage.getItem('BrReqFromDate').split('-');
            var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
            cFormDate.SetDate(fromdate);
        }

        if (localStorage.getItem('BrReqToDate')) {
            var todatearray = localStorage.getItem('BrReqToDate').split('-');
            var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
            ctoDate.SetDate(todate);
        }
        if (localStorage.getItem('BrReqBranch')) {
            if (ccmbBranchfilter.FindItemByValue(localStorage.getItem('BrReqBranch'))) {
                ccmbBranchfilter.SetValue(localStorage.getItem('BrReqBranch'));
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

        localStorage.setItem("BrReqFromDate", cFormDate.GetDate().format('yyyy-MM-dd'));
        localStorage.setItem("BrReqToDate", ctoDate.GetDate().format('yyyy-MM-dd'));
        localStorage.setItem("BrReqBranch", ccmbBranchfilter.GetValue());

        //cdownpaygrid.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue());

        $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
        $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
        $("#hfBranchID").val(ccmbBranchfilter.GetValue());
        $("#hfIsFilter").val("Y");

        

        //REV 1.0
        //CgvPurchaseIndent.Refresh();
        $("#hFilterType").val("All");
        cCallbackPanel.PerformCallback("");
        //END REV 1.0

        $("#drdExport").val(0);
    }
}
//REV 1.0
function CallbackPanelEndCall(s, e) {
    CgvPurchaseIndent.Refresh();
}
//END REV 1.0
//This function is called to show the Status of All Sales Order Created By Login User Start
function OpenPopUPUserWiseQuotaion() {
    cgridUserWiseQuotation.PerformCallback();
    cPopupUserWiseQuotation.Show();
}
// function above  End

//This function is called to show all Pending Approval of Sales Order whose Userid has been set LevelWise using Approval Configuration Module 
function OpenPopUPApprovalStatus() {
    //Mantis issue 25087
    CheckAddOrEdit();
    //End of Mantis issue 25087
    cgridPendingApproval.PerformCallback();
    cpopupApproval.Show();
    
}
// function above  End
// Status 2 is passed If Approved Check box is checked by User Both Below function is called and used to show in POPUP,  the Add Page of Respective Segment(like Page for Adding Quotation ,Sale Order ,Challan)
function GetApprovedQuoteId(s, e, itemIndex) {
    var rowvalue = cgridPendingApproval.GetRowValues(itemIndex, 'ID', OnGetApprovedRowValues);
}
function OnGetApprovedRowValues(obj) {
    uri = "BranchRequisition.aspx?key=" + obj + "&status=2";
    popup.SetContentUrl(uri);
    popup.Show();
}
// function above  End For Approved
// Status 3 is passed If Approved Check box is checked by User Both Below function is called and used to show in POPUP,  the Add Page of Respective Segment(like Page for Adding Quotation ,Sale Order ,Challan)
function GetRejectedQuoteId(s, e, itemIndex) {
    cgridPendingApproval.GetRowValues(itemIndex, 'ID', OnGetRejectedRowValues);
}
function OnGetRejectedRowValues(obj) {
    uri = "BranchRequisition.aspx?key=" + obj + "&status=3";
    popup.SetContentUrl(uri);
    popup.Show();
}
// function above  End For Rejected
// To Reflect the Data in Pending Waiting Grid and Pending Waiting Counting if the user approve or Rejecte the Order and Saved 

function OnApprovalEndCall(s, e) {
    $.ajax({
        type: "POST",
        url: "BranchRequisition.aspx/GetPendingCase",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $('#lblWaiting').text(data.d);
        }
    });
}
// function above  End 
// To Hide the Popup and Refresh the Data in Pending Waiting Grid 

$(document).ready(function () {
    $('#ApprovalCross').click(function () {
        window.parent.popup.Hide();
        window.parent.cgridPendingApproval.PerformCallback();
        window.location.href = 'BranchRequisition.aspx'
    })
});

// Basic Setting for Approval in Edit Mode this page has the List and Detial Part so to call it
function StartingSetupForApproval(indentid) {
    $('#hdnEditClick').val('T'); //Edit
    $('#hdn_Mode').val('Edit'); //Edit

    $('#lblHeading').text("Modify Branch Requisition");
    document.getElementById('DivEntry').style.display = 'block';
    document.getElementById('DivEdit').style.display = 'none';
    document.getElementById('btnAddNew').style.display = 'none';

    InsgridBatch.PerformCallback("ApprovalEdit~" + indentid);
    chkAccount = 1;
    document.getElementById('divNumberingScheme').style.display = 'none';
}

// function above  End 

var IndentId = 0;
function onPrintJv(id) {

    IndentId = id;
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

    if (cSelectPanel.cpSuccess != null) {
        var reportName = cCmbDesignName.GetValue();
        var module = 'BranchReq';
        window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + IndentId, '_blank')
    }
    cSelectPanel.cpSuccess = null
    if (cSelectPanel.cpSuccess == null) {
        cCmbDesignName.SetSelectedIndex(0);
    }
}

function CallFeedback_save() {
    var KeyVal = $("#hddnKeyValue").val();
    var flag = true;
    $("#hddnIsSavedFeedback").val("1");
    var Remarks = txtFeedback.GetValue();
    if (Remarks == "" || Remarks == null) {
        $('#MandatoryRemarksFeedback').attr('style', 'display:block;position: absolute; right: -20px; top: 8px;');
        flag = false;
    }
    else {
        $('#MandatoryRemarksFeedback').attr('style', 'display:none;position: absolute; right: -20px; top: 8px;');
        cPopup_Feedback.Hide();
        CgvPurchaseIndent.PerformCallback("Cancel~" + KeyVal + '~' + Remarks);
    }
    return flag;
}
function CancelFeedback_save() {
    $("#hddnIsSavedFeedback").val("0");
    txtFeedback.SetValue();
    cPopup_Feedback.Hide();
    $('#chkmailfeedback').prop('checked', false);
}
    //Code for UDF Control 
function OpenUdf() {
    if (document.getElementById('IsUdfpresent').value == '0') {
        jAlert("UDF not define.");
    }
    else {
        var keyVal = document.getElementById('Keyval_internalId').value;
        var url = '/OMS/management/Master/frm_BranchUdfPopUp.aspx?Type=BI&&KeyVal_InternalID=' + keyVal;
        cUDFpopup.SetContentUrl(url);
        cUDFpopup.Show();
    }
    return true;
}
function acbpCrpUdfEndCall(s, e) {
    if (cacbpCrpUdf.cpUDFBI) {
        if (cacbpCrpUdf.cpUDFBI == "true") {
            InsgridBatch.batchEditApi.EndEdit();
            InsgridBatch.UpdateEdit();
            cacbpCrpUdf.cpUDFBI = null;
        }
        else {
            // jAlert('UDF is set as Mandatory. Please enter values.');
            jAlert('UDF is set as Mandatory. Please enter values.', 'Alert Dialog: [BranchRequisition]', function (r) {
                if (r == true) {
                    OpenUdf();
                    InsgridBatch.batchEditApi.StartEdit(-1, 1);
                    InsgridBatch.batchEditApi.StartEdit(0, 2);
                }
            });

            cacbpCrpUdf.cpUDFBI = null;
        }
    }
}
    // End Udf Code
    function gridFocusedRowChanged(s, e) {
        globalRowIndex = e.visibleIndex;
    }
    var preColumn = '';
    var globalRowIndex;
    var chkAccount = 0;
    var currentval = '';
    function PageLoad() {
        FinYearCheckOnPageLoad();
    }
   
    function InstrumentDateChange() {

        var ExpectedDeliveryDate = new Date(InsgridBatch.GetEditor('ExpectedDeliveryDate').GetValue());
        var requisitionDate = new Date(ctDate.GetValue());


        var datediff = ExpectedDeliveryDate - requisitionDate;
        if (ExpectedDeliveryDate.format('yyyy-MM-dd') != requisitionDate.format('yyyy-MM-dd'))
            if (ExpectedDeliveryDate < requisitionDate) {
                jAlert('Expected Delivery date must be same or later to Requisition Date. Cannot Proceed.');
                InsgridBatch.GetEditor('ExpectedDeliveryDate').SetValue(null);
            }


    }
    function ddlBranchFor_SelectedIndexChanged() {
        var BranchFor = $("#ddlBranch").val();
        cddlBranchTo.PerformCallback(BranchFor);

    }
    function GetVisibleIndex(s, e) {
        globalRowIndex = e.visibleIndex;
    }
    //...................Shortcut keys.................
    var isCtrl = false;
    document.onkeydown = function (e) {
        if (event.keyCode == 18) isCtrl = true;



        if (event.keyCode == 78 && event.altKey == true) {


        }
        else if (event.keyCode == 88 && event.altKey == true) {

            //run code for Ctrl+X -- ie, Save & Exit! 
            if (document.getElementById('btnSaveExit').style.display != 'none') {
                document.getElementById('btnSaveExit').click();
                return false;
            }
        }
        else if ((event.keyCode == 120 || event.keyCode == 65) && event.altKey == true) {
            //run code for Ctrl+A -- ie, Add New
            if (document.getElementById('DivEntry').style.display != 'block') {
                AddButtonClick();
            }
        }
        else if (event.keyCode == 85 && event.altKey == true) {
            OpenUdf();
        }
    }
    //...................end............................
    function ChangeBranchTo() {

        if (document.getElementById('ddlBranchTo').value == "0") {
            $("#MandatoryBranchTo").show();

        }
        else {
            $("#MandatoryBranchTo").hide();
        }

        //if ($("#hdnProjectSelectInEntryModule").val() == "1") {
        //    clookup_Project.gridView.Refresh();
        //}
    }
    function ShowMsgLastCall() {

        if (CgvPurchaseIndent.cpDelete != null) {

            jAlert(CgvPurchaseIndent.cpDelete)
            //CgvPurchaseIndent.PerformCallback();
            CgvPurchaseIndent.cpDelete = null;
            CgvPurchaseIndent.Refresh();
        }
        if (CgvPurchaseIndent.cpCancel != null) {

            jAlert(CgvPurchaseIndent.cpCancel)
            //CgvPurchaseIndent.PerformCallback();
            CgvPurchaseIndent.Refresh();
            CgvPurchaseIndent.cpCancel = null;
            txtFeedback.SetValue();
        }
    }
    
    function SaveExitButtonClick() {
        InsgridBatch.AddNewRow();
        cLoadingPanelCRP.Show();

        var ProjectCode = clookup_Project.GetText();
        if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val() == "1" && ProjectCode == "") {
            //LoadingPanel.Hide();
            cLoadingPanelCRP.Hide();
            jAlert("Please Select Project.");
            return false;
        }
        //Mantis Issue 25087
        if ($("#hdnSettings").val() == "1" ) {
            if ($('#chkBranchRequisition').is(':checked') == true) {
                $("#hdnIsBranchRequisition").val(1);
            }
            else {
                $("#hdnIsBranchRequisition").val(0);
            }
        }
        //End of Mantis Issue 25087
        $('#hdnSaveNew').val("Save_Exit");

        $('#hdnRefreshType').val('E');
        $('#hdfIsDelete').val('I');
        if (document.getElementById('txtVoucherNo').value == "") {
            $("#MandatoryBillNo").show();
            cLoadingPanelCRP.Hide();
            return false;
        }

        if (cddlBranchTo.GetValue() == null) {
            $("#MandatoryBranchTo").show();
            cLoadingPanelCRP.Hide();
            return false;
        }
        var IsType = "";
        var frontRow = 0;
        var backRow = -1;

        for (var i = 0; i <= InsgridBatch.GetVisibleRowsOnPage() ; i++) {
            var frontProduct = "";
            var backProduct = "";

            frontProduct = (InsgridBatch.batchEditApi.GetCellValue(backRow, 'ProductName') != null) ? (InsgridBatch.batchEditApi.GetCellValue(backRow, 'ProductName')) : "";
            backProduct = (InsgridBatch.batchEditApi.GetCellValue(frontRow, 'ProductName') != null) ? (InsgridBatch.batchEditApi.GetCellValue(frontRow, 'ProductName')) : "";
            if (frontProduct != "" || backProduct != "") {
                IsType = "Y";
                break;
            }
            backRow--;
            frontRow++;
        }

        if (InsgridBatch.GetVisibleRowsOnPage() > 0) {

            if (IsType == "Y") {

                cacbpCrpUdf.PerformCallback();
            }
            else {
                jAlert('Cannot Save. You must enter atleast one Product to save this entry.');
                cLoadingPanelCRP.Hide();
            }
        }
        else {
            jAlert('Cannot Save. You must enter atleast one Product to save this entry.');
            cLoadingPanelCRP.Hide();
        }
        chkAccount = 0;
        return false;

    }
    function deleteAllRows() {
        var frontRow = 0;
        var backRow = -1;
        for (var i = 0; i <= InsgridBatch.GetVisibleRowsOnPage() + 100 ; i++) {
            InsgridBatch.DeleteRow(frontRow);
            InsgridBatch.DeleteRow(backRow);
            backRow--;
            frontRow++;
        }

    }
    function AutoCalValue(s, e) {

        var Quantity = (InsgridBatch.GetEditor('gvColQuantity').GetValue() != null) ? parseFloat(InsgridBatch.GetEditor('gvColQuantity').GetValue()) : "0";
        var Rate = (InsgridBatch.GetEditor('gvColRate').GetValue() != null) ? parseFloat(InsgridBatch.GetEditor('gvColRate').GetValue()) : "0";
        InsgridBatch.GetEditor('gvColValue').SetValue(Quantity * Rate);



    }
    function AutoCalValueBtRate(s, e) {
        var Quantity = (InsgridBatch.GetEditor('gvColQuantity').GetValue() != null) ? parseFloat(InsgridBatch.GetEditor('gvColQuantity').GetValue()) : "0";
        var Rate = (InsgridBatch.GetEditor('gvColRate').GetValue() != null) ? parseFloat(InsgridBatch.GetEditor('gvColRate').GetValue()) : "0";
        InsgridBatch.GetEditor('gvColValue').SetValue(Quantity * Rate);
    }
    function txtBillNo_TextChanged() {
        var VoucherNo = document.getElementById("txtVoucherNo").value;

        $.ajax({
            type: "POST",
            url: "BranchRequisition.aspx/CheckUniqueName",
            data: JSON.stringify({ VoucherNo: VoucherNo }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var data = msg.d;

                if (data == true) {
                    $("#MandatoryBillNo").show();

                    document.getElementById("txtVoucherNo").value = '';
                    document.getElementById("txtVoucherNo").focus();
                }
                else {
                    $("#MandatoryBillNo").hide();
                }
            }
        });
    }
    function BtnVisible() {
        document.getElementById('btnSaveExit').style.display = 'none'
        document.getElementById('btnnew').style.display = 'none'
        document.getElementById('tagged').style.display = 'block'

    }
    function AddNewRow() {
        InsgridBatch.AddNewRow();
        var noofvisiblerows = InsgridBatch.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
        var tbQuotation = InsgridBatch.GetEditor("SrlNo");
        tbQuotation.SetValue(noofvisiblerows);
    }

    function CmbScheme_LostFocus() {
        //if ($("#hdnProjectSelectInEntryModule").val() == "1")
        //    clookup_Project.gridView.Refresh();
    }

    
    function viewOnly() {

        if ($('#hdn_Mode').val().toUpperCase() == 'VIEW') {
            $('#DivEntry').find('input, textarea, button, select').attr('disabled', 'disabled');

            InsgridBatch.SetEnabled(false);
            cddlBranchTo.SetEnabled(false);
            ctDate.SetEnabled(false);

            cbtn_SaveRecords.SetVisible(false);
            cbtn_SaveRecordsExit.SetVisible(false);
            cbtn_SaveUdf.SetVisible(false);
            document.getElementById('tagged').style.display = 'none';
            document.getElementById('taggModify').style.display = 'none';
        }

    }
    function OnCustomButtonClick(s, e) {

        if (e.buttonID == 'CustomDeleteIDS') {

            if (InsgridBatch.GetVisibleRowsOnPage() > 1) {
                var tbQuotation = InsgridBatch.GetEditor("SrlNo");
                InsgridBatch.batchEditApi.EndEdit();
                InsgridBatch.DeleteRow(e.visibleIndex);
                $('#hdfIsDelete').val('D');

                InsgridBatch.UpdateEdit();
                InsgridBatch.PerformCallback('Display');

                var noofvisiblerows = InsgridBatch.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc                  
                tbQuotation.SetValue(noofvisiblerows);
            }

        }
        if (e.buttonID == 'CustomAddNewRow') {

            InsgridBatch.batchEditApi.StartEdit(e.visibleIndex, 2);
            var Product = (InsgridBatch.GetEditor('gvColProduct').GetValue() != null) ? InsgridBatch.GetEditor('gvColProduct').GetValue() : "";
            var Quantity = (InsgridBatch.GetEditor('gvColQuantity').GetValue() != null) ? InsgridBatch.GetEditor('gvColQuantity').GetValue() : "0.0";
            if (Product != "" && Quantity != "0.0") {
                InsgridBatch.AddNewRow();
                var noofvisiblerows = InsgridBatch.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
                var tbQuotation = InsgridBatch.GetEditor("SrlNo");
                tbQuotation.SetValue(noofvisiblerows);

                setTimeout(function () {
                    InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 2);
                }, 500);
                return false;
            }


        }
    }
    //....Tab Index Change From Rate to Grid First Column......
    $(document).ready(function () {
        $('#txtMemoPurpose_I').blur(function () {
            if (InsgridBatch.GetVisibleRowsOnPage() == 1) {
                InsgridBatch.batchEditApi.StartEdit(-1, 2);
            }
        })


    });
    //.....end..........
    function Save_ButtonClick() {

        var ProjectCode = clookup_Project.GetText();
        if ($("#hdnProjectSelectInEntryModule").val() == "1" && $("#hdnProjectMandatory").val() == "1" && ProjectCode == "") {
            //LoadingPanel.Hide();
            jAlert("Please Select Project.");
            return false;
        }

        $('#hdnSaveNew').val("Save_New");
        $('#hdfIsDelete').val('I');
        $('#hdnRefreshType').val('S');
        if (document.getElementById('txtVoucherNo').value == "") {
            $("#MandatoryBillNo").show();

            return false;
        }
        if (cddlBranchTo.GetValue() == null) {
            $("#MandatoryBranchTo").show();
            return false;
        }
        var IsType = "";
        var frontRow = 0;
        var backRow = -1;

        for (var i = 0; i <= InsgridBatch.GetVisibleRowsOnPage() ; i++) {
            var frontProduct = "";
            var backProduct = "";

            frontProduct = (InsgridBatch.batchEditApi.GetCellValue(backRow, 'ProductName') != null) ? (InsgridBatch.batchEditApi.GetCellValue(backRow, 'ProductName')) : "";
            backProduct = (InsgridBatch.batchEditApi.GetCellValue(frontRow, 'ProductName') != null) ? (InsgridBatch.batchEditApi.GetCellValue(frontRow, 'ProductName')) : "";
            if (frontProduct != "" || backProduct != "") {
                IsType = "Y";
                break;
            }
            backRow--;
            frontRow++;
        }
        if (InsgridBatch.GetVisibleRowsOnPage() > 0) {


            if (IsType == "Y") {

                cacbpCrpUdf.PerformCallback();
            }
            else {
                jAlert('Cannot Save. You must enter atleast one Product to save this entry.');
            }
        }
        else {
            jAlert('Cannot Save. You must enter atleast one Product to save this entry.');

        }
        chkAccount = 0;
        return false;
    }

    function AddBatchNew() {
        InsgridBatch.batchEditApi.EndEdit();

        var noofvisiblerows = InsgridBatch.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc

        var i;
        var cnt = 1;
        if (noofvisiblerows == "0") {
            InsgridBatch.AddNewRow();
        }
        InsgridBatch.SetFocusedRowIndex();

        for (i = -1 ; cnt <= noofvisiblerows ; i--) {
            cnt++;
        }

        var tbQuotation = InsgridBatch.GetEditor("SrlNo");
        //console.log(tbQuotation);
        tbQuotation.SetValue(cnt);

    }
    function ProductsGotFocus(s, e) {
        document.getElementById("pageheaderContent").style.display = 'block';
        document.getElementById("liToBranch").style.display = 'block';
        var tbDescription = InsgridBatch.GetEditor("gvColDiscription");
        var tbUOM = InsgridBatch.GetEditor("gvColUOM");
        var tdRate = InsgridBatch.GetEditor("gvColRate");
        var AvailableStock = InsgridBatch.GetEditor("gvColAvailableStock");
        var ProductID = (InsgridBatch.GetEditor('gvColProduct').GetValue() != null) ? InsgridBatch.GetEditor('gvColProduct').GetValue() : "0";
        var SpliteDetails = ProductID.split("||@||");
        var strProductID = SpliteDetails[0];
        var strDescription = SpliteDetails[1];
        var strUOM = SpliteDetails[2];
        var strUOMstk = SpliteDetails[4];

        var strRate = SpliteDetails[6];
        chkAccount = 1;
        tbDescription.SetValue(strDescription);
        tbUOM.SetValue(strUOM);
        tdRate.SetValue(strRate);
        $('#lblStkQty').text("0.00");
        $('#lblStkUOM').text(strUOM);
        $('#lblStkUOMTo').text(strUOM);
        if (ProductID != "0") {
            cacpAvailableStock.PerformCallback(strProductID);
        }
    }
    function ProductsGotFocusFromID(s, e) {
        pageheaderContent.style.display = "block";
        document.getElementById("liToBranch").style.display = 'block';

        var ProductID = (InsgridBatch.GetEditor('gvColProduct').GetValue() != null) ? InsgridBatch.GetEditor('gvColProduct').GetValue() : "0";
        var strProductName = (InsgridBatch.GetEditor('gvColProduct').GetText() != null) ? InsgridBatch.GetEditor('gvColProduct').GetText() : "0";


        var QuantityValue = (InsgridBatch.GetEditor('gvColQuantity').GetValue() != null) ? InsgridBatch.GetEditor('gvColQuantity').GetValue() : "0";
        var tbDescription = InsgridBatch.GetEditor("gvColDiscription");
        var tbUOM = InsgridBatch.GetEditor("gvColUOM");
        var tdRate = InsgridBatch.GetEditor("gvColRate");

        var SpliteDetails = ProductID.split("||@||");
        var strProductID = SpliteDetails[0];
        var strDescription = SpliteDetails[1];
        var strUOM = SpliteDetails[2];
        var strUOMstk = SpliteDetails[4];

        var strRate = SpliteDetails[6];
        chkAccount = 1;
        tbDescription.SetValue(strDescription);
        tbUOM.SetValue(strUOM);

        $('#lblStkQty').text("0.00");
        $('#lblStkUOM').text(strUOM);
        $('#lblStkUOMTo').text(strUOM);

        if (ProductID != "0") {
            cacpAvailableStock.PerformCallback(strProductID);
        }

    }
    function acpAvailableStockEndCall(s, e) {
        if (cacpAvailableStock.cpstock != null) {
            var AvailableStock = cacpAvailableStock.cpstock + " " + document.getElementById('lblStkUOM').innerHTML;
            $('#B_AvailableStock').text(AvailableStock);
            cacpAvailableStock.cpstock = null;
        }
        if (cacpAvailableStock.cpstockBranchTo != null) {
            document.getElementById("liToBranch").style.display = 'block';
            var AvailableStock = cacpAvailableStock.cpstockBranchTo + " " + document.getElementById('lblStkUOMTo').innerHTML;
            $('#B_AvailableStockToBranch').text(AvailableStock);
            cacpAvailableStock.cpstockBranchTo = null;
        }
        if (preColumn == "Product") {
            InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 5);
            preColumn = '';
            return;
        }


    }

    function SetLostFocusonDemand(e) {
        if ((new Date($("#hdnLockFromDate").val()) <= ctDate.GetDate()) && (ctDate.GetDate() <= new Date($("#hdnLockToDate").val()))) {
            jAlert("DATA is Freezed between   " + $("#hdnLockFromDateCon").val() + " to " + $("#hdnLockToDateCon").val() + " for Add.");
        }
    }




    function AddButtonClick() {
        $('#hdn_Mode').val('Entry'); //Entry
        $('#Keyval_internalId').val('Add');
        cCmbScheme.SetValue("0");
        ctxtRate.SetEnabled(false);
        document.getElementById('DivEntry').style.display = 'block';
        document.getElementById('DivEdit').style.display = 'none';
        document.getElementById('btnAddNew').style.display = 'none';
        document.getElementById('divfromTo').style.display = 'none';
        btncross.style.display = "block";

        document.getElementById('txtVoucherNo').disabled = true;
        document.getElementById('txtRate').disabled = true;
        document.getElementById('ddlBranch').disabled = true;
        $('#lblHeading').text("");
        $('#lblHeading').text("Add Branch Requisition");
        deleteAllRows();
        InsgridBatch.AddNewRow();
        var noofvisiblerows = InsgridBatch.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
        var tbQuotation = InsgridBatch.GetEditor("SrlNo");
        tbQuotation.SetValue(noofvisiblerows);
        cCmbScheme.Focus();
        ddlBranchFor_SelectedIndexChanged();
        //Mantis Issue 25087
        CheckAddOrEdit();
        //End of Mantis Issue 25087
    }
    //Mantis Issue 25087
    function CheckAddOrEdit() {
        if ($('#hdnSettings').val() == "1" && $("#Keyval_internalId").val() == 'Add') {
            $("#divIsRequired").removeClass('hide');
            $('#chkBranchRequisition').prop('checked', true);
        }
        else {
            $("#divIsRequired").addClass('hide');
        }
    }
    //End of Mantis Issue 25087
    function CmbScheme_ValueChange() {
        var val = cCmbScheme.GetValue();
        $.ajax({
            type: "POST",
            url: 'BranchRequisition.aspx/getSchemeType',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{sel_scheme_id:\"" + val + "\"}",
            success: function (type) {
                var schemetypeValue = type.d;
                var schemetype = schemetypeValue.toString().split('~')[0];
                var schemelength = schemetypeValue.toString().split('~')[1];
                $('#txtVoucherNo').attr('maxLength', schemelength);
                var branchID = schemetypeValue.toString().split('~')[2];


                var fromdate = schemetypeValue.toString().split('~')[3];
                var todate = schemetypeValue.toString().split('~')[4];

                var dt = new Date();

                ctDate.SetDate(dt);

                if (dt < new Date(fromdate)) {
                    ctDate.SetDate(new Date(fromdate));
                }

                if (dt > new Date(todate)) {
                    ctDate.SetDate(new Date(todate));
                }




                ctDate.SetMinDate(new Date(fromdate));
                ctDate.SetMaxDate(new Date(todate));


                if (schemetypeValue != "") {
                    document.getElementById('ddlBranch').value = branchID;
                    document.getElementById('ddlBranch').disabled = true;
                    cddlBranchTo.PerformCallback(branchID);

                }
                if (schemetype == '0') {
                    $('#hdnSchemaType').val('0');
                    document.getElementById('txtVoucherNo').disabled = false;
                    document.getElementById('txtVoucherNo').value = "";
                    $('#txtVoucherNo').focus();
                }
                else if (schemetype == '1') {
                    $('#hdnSchemaType').val('1');
                    document.getElementById('txtVoucherNo').disabled = true;
                    document.getElementById('txtVoucherNo').value = "Auto";
                    $("#MandatoryBillNo").hide();
                    ctDate.Focus();
                }
                else if (schemetype == '2') {
                    $('#hdnSchemaType').val('2');
                    document.getElementById('txtVoucherNo').disabled = true;
                    document.getElementById('txtVoucherNo').value = "Datewise";
                }
                else if (schemetype == 'n') {
                    document.getElementById('txtVoucherNo').disabled = true;
                    document.getElementById('txtVoucherNo').value = "";
                }
                $("#ProjectForBranch").val(branchID);
            }
        });

        // $("#ProjectForBranch").val(ddlBranch);
        if ($("#hdnProjectSelectInEntryModule").val() == "1") {
            clookup_Project.gridView.Refresh();
        }

    }
    
   
    function ProductKeyDown(s, e) {
        // console.log(e.htmlEvent.key);
        if (e.htmlEvent.key == "Enter") {
            s.OnButtonClick(0);
        }
           
    }

    function ProductButnClick(s, e) {
        if (e.buttonIndex == 0) {
            setTimeout(function () { $("#txtProdSearch").focus(); }, 500);
            $('#txtProdSearch').val('');
            $('#ProductModel').modal('show');
        }
           
    }
    function ValueSelected(e, indexName) {
        if (e.code == "Enter") {
            var Id = e.target.parentElement.parentElement.cells[0].innerText;
            var name = e.target.parentElement.parentElement.cells[1].children[0].value;
            if (Id) {
                if (indexName == "ProdIndex") {
                    SetProduct(Id, name);
                }
            }
        }
        else if (e.code == "ArrowDown") {
            thisindex = parseFloat(e.target.getAttribute(indexName));
            thisindex++;
            if (thisindex < 10)
                $("input[" + indexName + "=" + thisindex + "]").focus();
        }
        else if (e.code == "ArrowUp") {
            thisindex = parseFloat(e.target.getAttribute(indexName));
            thisindex--;
            if (thisindex > -1)
                $("input[" + indexName + "=" + thisindex + "]").focus();
            else {
                if (indexName == "ProdIndex") {
                    $('#txtProdSearch').focus();
                }
            }
        }
    }
    function prodkeydown(e) {
        var OtherDetails = {}
        if ($.trim($("#txtProdSearch").val()) == "" || $.trim($("#txtProdSearch").val()) == null) {
            return false;
        }
        OtherDetails.SearchKey = $("#txtProdSearch").val();
        if (e.code == "Enter" || e.code == "NumpadEnter") {
            var HeaderCaption = [];
            HeaderCaption.push("Product Code");
            HeaderCaption.push("Product Name");
            HeaderCaption.push("Inventory");
            HeaderCaption.push("HSN/SAC");
            HeaderCaption.push("Class");
            HeaderCaption.push("Brand");
            if ($("#txtProdSearch").val() != '') {

                if($("#hdnNoninventoryItemBranchReqBTOBTI").val()!="0")
                {
                    callonServer("Services/Master.asmx/GetNonInventoryProductDetailsBranchRequisition", OtherDetails, "ProductTable", HeaderCaption, "ProdIndex", "SetProduct");
                }
                else
                {
                    callonServer("Services/Master.asmx/GetProductDetailsBranchRequisition", OtherDetails, "ProductTable", HeaderCaption, "ProdIndex", "SetProduct");
                }

                    
            }
        }
        else if (e.code == "ArrowDown") {
            if ($("input[ProdIndex=0]"))
                $("input[ProdIndex=0]").focus();
        }
    }
     
    function SetProduct(Id, Name) {
        $('#ProductModel').modal('hide');
        var LookUpData = Id;
        var ProductCode = Name;
        if (!ProductCode) {
            LookUpData = null;
        }
        InsgridBatch.batchEditApi.StartEdit(globalRowIndex);
        InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 5);
        InsgridBatch.GetEditor("gvColProduct").SetText(LookUpData);
        InsgridBatch.GetEditor("ProductName").SetText(ProductCode);
        pageheaderContent.style.display = "block";
        var tbDescription = InsgridBatch.GetEditor("gvColDiscription");
        var tbUOM = InsgridBatch.GetEditor("gvColUOM");
        var tbSalePrice = InsgridBatch.GetEditor("gvColRate");
        var ProductID = (InsgridBatch.GetEditor('gvColProduct').GetText() != null) ? InsgridBatch.GetEditor('gvColProduct').GetText() : "0";
        var SpliteDetails = ProductID.split("||@||");
        var strProductID = SpliteDetails[0];
        var strDescription = SpliteDetails[1];
        var strUOM = SpliteDetails[2];
        var strStkUOM = SpliteDetails[4];
        var strSalePrice = SpliteDetails[6];
        var QuantityValue = (InsgridBatch.GetEditor('gvColQuantity').GetValue() != null) ? InsgridBatch.GetEditor('gvColQuantity').GetValue() : "0";
        tbDescription.SetValue(strDescription);
        tbUOM.SetValue(strUOM);
        tbSalePrice.SetValue(strSalePrice);
        InsgridBatch.GetEditor("gvColQuantity").SetValue("0.00");
        $('#lblStkQty').text("0.00");
        $('#lblStkUOM').text(strUOM);
        $('#lblStkUOMTo').text(strUOM);
        preColumn = "Product";
        cacpAvailableStock.PerformCallback(strProductID);
        InsgridBatch.batchEditApi.StartEdit(globalRowIndex, 5);
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
                    //console.log(value);
                    $(value).css({ 'opacity': '1' });
                }, 100);
            });
        }, 200);
    }

//Mantis Issue 25087
    //function onSmsClickJv(key) {
    //    $("#assignEmployee").modal('show');
    //    BindModalEmployee();
    //    //PIndentId = key;
    //    $("#hdnIndentId").val(key);
    //}
    function onSmsClickJv(key) {
        jConfirm('Send SMS?', 'Alert', function (r) {
            if (r) {
                $.ajax({
                    type: "POST",
                    url: "BranchRequisition.aspx/SendSMSManualNo",
                    data: JSON.stringify({ PIndentId: key }),
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
//End of Mantis Issue 25087
