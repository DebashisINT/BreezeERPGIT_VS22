/**********************************************************************************************************************************
 1.0      30-05-2023        2.0.38        Sanchita      ERP - Listing Views - Purchase Invoice. refer: 26250
***********************************************************************************************************************************/

// Rev 1.0  [ block from PurchaseInvoice.js]
var isFirstTime = true;

// Purchase Invoice Section Start
updatePBGridByDate = function () {
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

        localStorage.setItem("PBFromDate", cFormDate.GetDate().format('yyyy-MM-dd'));
        localStorage.setItem("PBToDate", ctoDate.GetDate().format('yyyy-MM-dd'));
        localStorage.setItem("PBBranch", ccmbBranchfilter.GetValue());

        //cdownpaygrid.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue());

        $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
        $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
        $("#hfBranchID").val(ccmbBranchfilter.GetValue());
        $("#hfIsFilter").val("Y");

       //cgrid.Refresh();
        $("#hFilterType").val("All");
        cCallbackPanel.PerformCallback("");
       
        //$("#drdExport").val(0);
    }
}
updateGridAfterDelete = function () {
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

        localStorage.setItem("PBFromDate", cFormDate.GetDate().format('yyyy-MM-dd'));
        localStorage.setItem("PBToDate", ctoDate.GetDate().format('yyyy-MM-dd'));
        localStorage.setItem("PBBranch", ccmbBranchfilter.GetValue());
        $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
        $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
        $("#hfBranchID").val(ccmbBranchfilter.GetValue());
        $("#hfIsFilter").val("Y");

        cgrid.Refresh();

        //$("#drdExport").val(0);

        //cdownpaygrid.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue());
        //$("#drdExport").val(0);
    }
}

AllControlInitilize = function (s, e) {
    if (isFirstTime) {
        if (localStorage.getItem('PBFromDate')) {
            var fromdatearray = localStorage.getItem('PBFromDate').split('-');
            var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
            cFormDate.SetDate(fromdate);
        }

        if (localStorage.getItem('PBToDate')) {
            var todatearray = localStorage.getItem('PBToDate').split('-');
            var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
            ctoDate.SetDate(todate);
        }

        if (localStorage.getItem('PBBranch')) {
            if (ccmbBranchfilter.FindItemByValue(localStorage.getItem('PBBranch'))) {
                ccmbBranchfilter.SetValue(localStorage.getItem('PBBranch'));
            }
        }
    }
}

// End of Rev 1.0
function BeginCallback() {
    $("#drdExport").val(0);
}
function ClearField() {
    cFormDate.SetDate(null);
    ctoDate.SetDate(null);
    ccmbBranchfilter.SetSelectedIndex(0);
}
function OnCancelClick(keyValue, visibleIndex) {
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
    //debugger;
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
function OnEWayBillClick(id, EWayBillNumber, EWayBillDate, EWayBillValue) {
    //cgrid.SetFocusedRowIndex(VisibleIndex);
    //var EWayBillNumber = cgrid.GetRow(cgrid.GetFocusedRowIndex()).children[16].innerText;
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


    ctxtTransporterGSTIN.SetText('');
    ctxtTransporterName.SetText('');
    $("#ddlTransportationMode").val('0');
    ctxtTransportationDistance.SetText('0');
    ctxtTransporterDocNo.SetText('');
    ctxtVehicleNo.SetText('');
    $("#ddlVehicleType").val('0');

    $.ajax({
        type: "POST",
        url: "Purchaseinvoicelist.aspx/EditEWayBill",
        data: JSON.stringify({
            DocID: id
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            var status = msg.d[0];
            console.log(msg);
            if (status != "") {
                console.log(status);
                ctxtTransporterGSTIN.SetText(status.TransporterGSTIN);
                ctxtTransporterName.SetText(status.TransporterName);
                if (status.Transporter_Mode != '') {
                    $("#ddlTransportationMode").val(status.Transporter_Mode);
                }
                if (status.Transporter_Distance != '') {
                    ctxtTransportationDistance.SetText(status.Transporter_Distance);
                }
                if (status.Transporter_DocNo != '') {
                    ctxtTransporterDocNo.SetText(status.Transporter_DocNo);
                }
                if (status.Vehicle_No != '') {
                    ctxtVehicleNo.SetText(status.Vehicle_No);
                }
                if (status.Vehicle_type != '') {
                    $("#ddlVehicleType").val(status.Vehicle_type);
                }
            }
        }
    });

    $('#hddnInvoiceID').val(id);
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

    var InvoiceID = $("#hddnInvoiceID").val();
    //if (ctxtEWayBillNumber.GetValue() == null) {                
    //    jAlert("Please enter E-Way Bill Number.");
    //        return false;               
    //}
    //else
    //{
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

    //Rev Extra column Tanmoy
    var TransporterGSTIN = ctxtTransporterGSTIN.GetText();
    var TransporterName = ctxtTransporterName.GetText();
    var TransportationMode = $("#ddlTransportationMode").val();
    var TransportationDistance = ctxtTransportationDistance.GetText();
    var TransporterDocNo = ctxtTransporterDocNo.GetText();
    var TransporterDocDate = null;
    if (cdt_TransporterDocDate.GetValue() == "" && cdt_TransporterDocDate.GetValue() == null) {
        var TransporterDocDate = "1990-01-01";
    }
    else {
        if (cdt_TransporterDocDate.GetValue() == null) {
            var TransporterDocDate = null;
        }
        else {
            var TransporterDocDate = GetEWayBillDateFormat(new Date(cdt_TransporterDocDate.GetValue()));
        }
    }
    var VehicleNo = ctxtVehicleNo.GetText();
    var VehicleType = $("#ddlVehicleType").val();
    //End of Rev

    $.ajax({
        type: "POST",
        url: "PurchaseInvoicelist.aspx/UpdateEWayBill",
        data: JSON.stringify({
            InvoiceID: InvoiceID, UpdateEWayBill: UpdateEWayBill, EWayBillDate: EWayBillDate, EWayBillValue: EWayBillValue, TransporterGSTIN: TransporterGSTIN
                    , TransporterName: TransporterName, TransportationMode: TransportationMode, TransportationDistance: TransportationDistance, TransporterDocNo: TransporterDocNo
                    , TransporterDocDate: TransporterDocDate, VehicleNo: VehicleNo, VehicleType: VehicleType
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
                cgrid.Refresh();
            }
            else if (status == "-10") {
                jAlert("Data not saved.");
                cPopup_EWayBill.Hide();
            }
        }
    });
    //}           
}
function CancelEWayBill_save() {
    cPopup_EWayBill.Hide();
}

function PerformCallToGridBind() {
    cSelectPanel.PerformCallback();
    cDocumentsPopup.Hide();
    return false;
}

function cSelectPanelEndCall(s, e) {
    if (cSelectPanel.cpSuccess != null) {
        var TotDocument = cSelectPanel.cpSuccess.split(',');
        var reportName = cCmbDesignName.GetValue();
        var module = 'PInvoice';
        if (TotDocument.length > 0) {
            for (var i = 0; i < TotDocument.length; i++) {
                if (TotDocument[i] != "") {
                    window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + PInvoice_id + '&PrintOption=' + TotDocument[i], '_blank')
                }
            }
        }
    }
    if (cSelectPanel.cpSuccess == "") {
        if (cSelectPanel.cpChecked != "") {
            //jAlert('Please check Original For Recipient and proceed.');
            jAlert('Please check atleast one option and proceed.');
        }
        CselectDuplicate.SetEnabled(false);
        CselectTriplicate.SetEnabled(false);
        CselectNone.SetCheckState('UnChecked');
        CselectOriginal.SetCheckState('UnChecked');
        CselectDuplicate.SetCheckState('UnChecked');
        CselectTriplicate.SetCheckState('UnChecked');
        cCmbDesignName.SetSelectedIndex(0);
    }
}
function NoneCheckChange(s, e) {
    if (s.GetCheckState() == 'Checked') {
        CselectOriginal.SetEnabled(false);
    }
    else {
        CselectOriginal.SetCheckState('UnChecked');
        CselectOriginal.SetEnabled(true);
        CselectDuplicate.SetCheckState('UnChecked');
        CselectDuplicate.SetEnabled(false);
        CselectTriplicate.SetCheckState('UnChecked');
        CselectTriplicate.SetEnabled(false);
    }
}

function OrginalCheckChange(s, e) {
    if (s.GetCheckState() == 'Checked') {
        CselectDuplicate.SetEnabled(true);
        CselectNone.SetCheckState('UnChecked');
        CselectNone.SetEnabled(false);
    }
    else {
        CselectNone.SetEnabled(true);
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

    if (event.keyCode == 73 && event.altKey == true) {
        StopDefaultAction(e);
        OnAddInventoryButtonClick();
    }
    if (event.keyCode == 78 && event.altKey == true) {
        StopDefaultAction(e);
        OnAddNonInventoryButtonClick();
    }
    if (event.keyCode == 67 && event.altKey == true) {
        StopDefaultAction(e);
        OnAddCapitalButtonClick();
    }
    if (event.keyCode == 66 && event.altKey == true) {
        StopDefaultAction(e);
        OnAddBothButtonClick();
    }
    if (event.keyCode == 83 && event.altKey == true) {
        StopDefaultAction(e);
        OnAddServiceButtonClick();
    }
}
function StopDefaultAction(e) {
    if (e.preventDefault) { e.preventDefault() }
    else { e.stop() };
    e.returnValue = false;
    e.stopPropagation();
}
function OnAddInventoryButtonClick() {
    var url = 'PurchaseInvoice.aspx?key=' + 'ADD&&InvType=Y';
    window.location.href = url;
}
function OnAddNonInventoryButtonClick() {
    var url = 'PurchaseInvoice.aspx?key=' + 'ADD&&InvType=N';
    window.location.href = url;
}
function OnAddCapitalButtonClick() {
    var url = 'PurchaseInvoice.aspx?key=' + 'ADD&&InvType=C';
    window.location.href = url;
}
function OnAddBothButtonClick() {
    var url = 'purchaseinvoice.aspx?key=' + 'ADD&&InvType=B';
    window.location.href = url;
}
function OnAddServiceButtonClick() {
    var url = 'purchaseinvoice.aspx?key=' + 'ADD&&InvType=S';
    window.location.href = url;
}
function OnClickDelete(keyValue) {
    //rev for Nil Rated TDS checking for Edit Tanmoy
    IsNillTDSCheck(keyValue);
    if (NillTDS) {
        //End of rev for Nil Rated TDS checking for Edit Tanmoy
        jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
            if (r == true) {
                cgrid.PerformCallback('Delete~' + keyValue);
            }
        });
    }
    else {
        jAlert('TDS Challan is already made for this document. Delete is not allowed.', 'Alert');
        return;
    }
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
        updateGridAfterDelete();
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
    // alert(keyValue);
    var url = '/OMS/Management/Activities/View/ViewPnv.html?id=' + keyValue;
    CAspxDirectAddPnvPopup.SetContentUrl(url);
    //AspxDirectAddCustPopup.ClearVerticalAlignedElementsCache();
    CAspxDirectAddPnvPopup.RefreshContentUrl();
    CAspxDirectAddPnvPopup.Show();
}


function OnclickViewAttachment(obj) {
    var URL = '/OMS/Management/Activities/PurchaseInvoice_Document.aspx?idbldng=' + obj + '&type=PurchaseInvoice';
    window.location.href = URL;
}

function OnAddButtonClick() {
    var url = 'PurchaseInvoice.aspx?key=' + 'ADD';
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
    uri = "PurchaseInvoice.aspx?key=" + obj + "&status=2" + '&type=PB';
    popup.SetContentUrl(uri);
    popup.Show();
    //window.location.href = uri;

}

function ch_fnApproved() {
}


function GetRejectedQuoteId(s, e, itemIndex) {

    cgridPendingApproval.GetRowValues(itemIndex, 'ID', OnGetRejectedRowValues);

}
function OnGetRejectedRowValues(obj) {
    uri = "PurchaseInvoice.aspx?key=" + obj + "&status=3" + '&type=PB';
    popup.SetContentUrl(uri);
    popup.Show();
}

// User Approval Status End

function OnApprovalEndCall(s, e) {
    $.ajax({
        type: "POST",
        url: "purchaseinvoicelist.aspx/GetPendingCase",
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

var NillTDS = true;

function IsNillTDSCheck(val) {
    NillTDS = true;
    $.ajax({
        type: "POST",
        url: "Purchaseinvoicelist.aspx/IsNillTDSCheck",
        data: JSON.stringify({ ID: val }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            var status = msg.d;
            if (status == "True") {
                NillTDS = false;
            }
            else {
                NillTDS = true;
            }
        }
    });
}
/*Mantise work 24702 04.03.2022*/

function OnPartyInvClick(id) {
    debugger;
   ctxtPartyInvNo.SetText('');
        $.ajax({
            type: "POST",
            url: "Purchaseinvoicelist.aspx/EditPartyInvDate",
            data: JSON.stringify({ DocID: id }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {
                var status = msg.d[0];
                console.log(msg);
                if (status != "") {
                    console.log(status);
                    ctxtPartyInvNo.SetText(status.PartyInvoiceNo);
                    var dob = new Date(status.PartyInvoiceDate);
                    cdt_PartyInv.SetDate(dob);
                }
            }
        });
        $('#hddnInvoiceID').val(id);
        cPopup_PartyINV.Show();
        ctxtPartyInvNo.Focus();
 }
function CancelPartyInv_save() {
    cPopup_PartyINV.Hide();
}
function CallPartyInv_save() {
    if (cdt_PartyInv.GetValue() == "" && cdt_PartyInv.GetValue() == null) {
        var PartyInvDate = "1990-01-01";
    }
    else {
        var PartyInvDate = GetEWayBillDateFormat(new Date(cdt_PartyInv.GetValue()));
    }
    var InvoiceID = $("#hddnInvoiceID").val();
    var PartyInvValue = ctxtPartyInvNo.GetText();
    $.ajax({
        type: "POST",
        url: "PurchaseInvoicelist.aspx/UpdatePartyINVDt",
        data: JSON.stringify({
            InvoiceID:InvoiceID, PartyInvoiceNo: PartyInvValue, PartyInvoiceDate: PartyInvDate
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            var status = msg.d;
            if (status == "1") {
                jAlert("Party Invoice Number Updated successfully.");
                cPopup_PartyINV.Hide();
                cgrid.Refresh();
            }
            else if (status == "-10") {
                jAlert("Data not Updated.");
                cPopup_PartyINV.Hide();
            }
        }
    });           
}
/*Clsoe of mantise work 24702 04.03.2022*/
