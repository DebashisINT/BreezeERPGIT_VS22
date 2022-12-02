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
}
function OnGetApprovedRowValues(obj) {
    uri = "PurchaseChallan.aspx?key=" + obj + "&status=2" + '&type=PC';
    popup.SetContentUrl(uri);
    popup.Show();
}
// function above  End For Approved
// Status 3 is passed If Approved Check box is checked by User Both Below function is called and used to show in POPUP,  the Add Page of Respective Segment(like Page for Adding Quotation ,Sale Order ,Challan)
function GetRejectedQuoteId(s, e, itemIndex) {
    //debugger;
    cgridPendingApproval.GetRowValues(itemIndex, 'ID', OnGetRejectedRowValues);

}
function OnGetRejectedRowValues(obj) {
    uri = "PurchaseChallan.aspx?key=" + obj + "&status=3" + '&type=PC';
    popup.SetContentUrl(uri);
    popup.Show();
}
// function above  End For Rejected
// To Reflect the Data in Pending Waiting Grid and Pending Waiting Counting if the user approve or Rejecte the Order and Saved 
function OnApprovalEndCall(s, e) {
    $.ajax({
        type: "POST",
        url: "PurchaseChallanList.aspx/GetPendingCase",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $('#lblWaiting').text(data.d);
        }
    });
}
// function above  End 
//<%-- Code Added By Sandip For Approval Detail Section End--%>
var PChallan_id = 0;
function onPrintJv(id) {
    //debugger;
    PChallan_id = id;
    cSelectPanel.cpSuccess = "";
    cDocumentsPopup.Show();
    //CselectDuplicate.SetEnabled(false);
    //CselectTriplicate.SetEnabled(false);
    CselectOriginal.SetCheckState('UnChecked');
    CselectDuplicate.SetCheckState('UnChecked');
    CselectTriplicate.SetCheckState('UnChecked');
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
    //debugger;
    if (cSelectPanel.cpSuccess != null) {
        var TotDocument = cSelectPanel.cpSuccess.split(',');
        var reportName = cCmbDesignName.GetValue();
        var module = 'PChallan';
        if (TotDocument.length > 0) {
            for (var i = 0; i < TotDocument.length; i++) {
                if (TotDocument[i] != "") {
                    window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + PChallan_id + '&PrintOption=' + TotDocument[i], '_blank')
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
$(document).ready(function () {
})
document.onkeydown = function (e) {
    //if (event.keyCode == 18) isCtrl = true;

    if (event.keyCode == 73 && event.altKey == true) { //run code for alt+a -- ie, Add
        StopDefaultAction(e);
        OnAddInventoryButtonClick();
    }
    else if (event.keyCode == 66 && event.altKey == true) { //run code for alt+a -- ie, Add
        StopDefaultAction(e);
        OnAddBothButtonClick();
    }

}
function StopDefaultAction(e) {
    if (e.preventDefault) { e.preventDefault() }
    else { e.stop() };

    e.returnValue = false;
    e.stopPropagation();
}
function AddButtonClick() {
    var url = 'PurchaseChallan.aspx?key=' + 'ADD';
    window.location.href = url;
}
function OnAddInventoryButtonClick() {
    var url = 'PurchaseChallan.aspx?key=' + 'ADD&InvType=Y';
    window.location.href = url;
}
function OnAddBothButtonClick() {
    var url = 'PurchaseChallan.aspx?key=' + 'ADD&InvType=B';
    window.location.href = url;
}
var CheckInvoiceTaggedval = 0;
function CheckInvoiceTagged(ChallanId)
{
    $.ajax({
        type: "POST",
        url: "PurchaseChallanList.aspx/CheckInvoiceTagged",
        data: JSON.stringify({
            ChallanId: ChallanId
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            var status = msg.d;
            if(status==1)
            {
                CheckInvoiceTaggedval = 1;
            }
           
        }
    });
}

function OnMoreInfoClick(keyValue) {
    var url = "";
    CheckInvoiceTagged(keyValue);
    if (CheckInvoiceTaggedval == "1")
    {
        url = 'PurchasechallanEdit.aspx?key=' + keyValue + '&type=PC';
    }
    else
    {
        url = 'PurchaseChallan.aspx?key=' + keyValue + '&type=PC';
    }
    
    window.location.href = url;
}

////##### coded by Samrat Roy - 04/05/2017  
////Add an another param to define request type 
function OnViewClick(keyValue) {
    var url = 'PurchaseChallan.aspx?key=' + keyValue + '&req=V' + '&type=PC';
    window.location.href = url;
}

function OnClickDelete(keyValue) {
    jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
        if (r == true) {
            CgvPurchaseOrder.PerformCallback('Delete~' + keyValue);
        }
    });
}
function OnEndCallback(s, e) {

    if (CgvPurchaseOrder.cpDelete != null) {
        var _messege = CgvPurchaseOrder.cpDelete;
        CgvPurchaseOrder.cpDelete = null;
        jAlert(_messege);

        updateGridByDate();
        //window.location.href = "PurchaseChallanList.aspx";
    }
}
//function OnclickViewAttachment(obj) {
//    var URL = '/OMS/Management/Activities/PurchaseChallanDocument.aspx?idbldng=' + obj + '&type=PC';
//    window.location.href = URL;
//}
function OnclickViewAttachment(obj) {
    //var URL = '/OMS/Management/Activities/SalesInvoice_Document.aspx?idbldng=' + obj + '&type=SalesInvoice';
    var URL = '/OMS/Management/Activities/EntriesDocuments.aspx?idbldng=' + obj + '&type=PurchaseChallan';
    window.location.href = URL;
}
function OnEWayBillClick(id, EWayBillNumber, EWayBillDate, EWayBillValue) {
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
        url: "PurchaseChallanList.aspx/EditEWayBill",
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
        url: "PurchaseChallanList.aspx/UpdateEWayBill",
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
                cPopup_EWayBill.Hide();
                CgvPurchaseOrder.Refresh();
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
var isFirstTime = true;
function AllControlInitilize() {
    if (isFirstTime) {
        if (localStorage.getItem('GRNList_FromDate')) {
            var fromdatearray = localStorage.getItem('GRNList_FromDate').split('-');
            var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
            cFormDate.SetDate(fromdate);
        }

        if (localStorage.getItem('GRNList_ToDate')) {
            var todatearray = localStorage.getItem('GRNList_ToDate').split('-');
            var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
            ctoDate.SetDate(todate);
        }

        if (localStorage.getItem('GRNList_Branch')) {
            if (ccmbBranchfilter.FindItemByValue(localStorage.getItem('GRNList_Branch'))) {
                ccmbBranchfilter.SetValue(localStorage.getItem('GRNList_Branch'));
            }

        }

        // updateGridByDate();
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
        localStorage.setItem("GRNList_FromDate", cFormDate.GetDate().format('yyyy-MM-dd'));
        localStorage.setItem("GRNList_ToDate", ctoDate.GetDate().format('yyyy-MM-dd'));
        localStorage.setItem("GRNList_Branch", ccmbBranchfilter.GetValue());

        //CgvPurchaseOrder.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue())

        $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
        $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
        $("#hfBranchID").val(ccmbBranchfilter.GetValue());
        $("#hfIsFilter").val("Y");

        // Mantis Issue 25402
        //CgvPurchaseOrder.Refresh();
        cCallbackPanel.PerformCallback("");
        // End of Mantis Issue 25402
    }
}
function gridRowclick(s, e) {
    $('#Grid_PurchaseChallan').find('tr').removeClass('rowActive');
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
$(document).ready(function () {
        if ($('body').hasClass('mini-navbar')) {
            var windowWidth = $(window).width();
            var cntWidth = windowWidth - 90;
            CgvPurchaseOrder.SetWidth(cntWidth);
        } else {
            var windowWidth = $(window).width();
            var cntWidth = windowWidth - 220;
            CgvPurchaseOrder.SetWidth(cntWidth);
        }
        $('.navbar-minimalize').click(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                CgvPurchaseOrder.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                CgvPurchaseOrder.SetWidth(cntWidth);
            }

        });
    });

function OnClosedClick(ChallanId)
{
    $("#hddnChallanID").val(ChallanId);
    $.ajax({
        type: "POST",
        url: "PurchaseChallanList.aspx/CheckGRNFullTagOrNot",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: "{ChallanId:\"" + ChallanId + "\"}",
        async: false,
        success: function (msg) {
            var status = msg.d;
            if(status=="FullTagg")
            {
                jAlert("This GRN has no open Items to Close further.");
            }
            else if (status == "PartialTagg")
            {
                cgridproducts.PerformCallback('BindProductsDetails~' + ChallanId);
                cProductsPopup.Show();
            }
        }    
    });
}

function OnClosedOK()
{
    var ChallanDetailsIDs = cgridproducts.GetSelectedKeysOnPage();
    var ChallanDetails_ID = "",Product_IDS="";    
    for (var i = 0; i < ChallanDetailsIDs.length; i++) {
        if (ChallanDetails_ID == "") {
            ChallanDetails_ID = ChallanDetailsIDs[i];
            Product_IDS=cgridproducts.GetRow(i).children[3].innerText;
        }
        else {
            ChallanDetails_ID += ',' + ChallanDetailsIDs[i];
            Product_IDS += ',' + cgridproducts.GetRow(i).children[3].innerText;
        }
    }
    var ChallanID = $("#hddnChallanID").val();

    var ProductType=""

    if (ChallanDetailsIDs.length > 0) {
        for (var loopcount = 0 ; loopcount < cgridproducts.GetVisibleRowsOnPage() ; loopcount++)
        {
            var ProductID = cgridproducts.GetRow(loopcount).children[3].innerText;
            if (ProductID != "")
            {
                $.ajax({
                    type: "POST",
                    url: 'PurchaseChallanList.aspx/getProductType',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: "{Products_ID:\"" + ProductID + "\"}",
                    success: function (msg) {
                        var Type = msg.d;
                        ProductType = Type;
                    }
                });
            }
            if(ProductType!="W")
            {
                break;
            }           
        }
    }
    if (ProductType="W")
    {
        $.ajax({
            type: "POST",
            url: "PurchaseChallanList.aspx/ClosePartialData",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ ChallanId: ChallanID, ChallanDetails_ID: ChallanDetails_ID, Product_IDS: Product_IDS }),
            async: false,
            success: function (msg) {
                var status = msg.d;
                if(status=="Delete")
                {
                    cProductsPopup.Hide();
                    jAlert("Deleted successfully.");
                }
                else if (status == "negative")
                {
                    jAlert("Available stock will become negative. Cannot Delete.");
                }

            }
        });
    }
    
}
