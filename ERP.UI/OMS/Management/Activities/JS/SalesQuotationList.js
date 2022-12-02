function timerTick() {
    $.ajax({
        type: "POST",
        url: "SalesQuotationList.aspx/GetTotalWatingQuotationCount",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
                  
            var status = msg.d;
            console.log(status);
            ClblQuoteweatingCount.SetText(status);
            var fetcheddata = parseFloat(document.getElementById('waitingQuotationCount').value);
            if (status != fetcheddata) {
                CwatingQuotegrid.Refresh();
                document.getElementById('waitingQuotationCount').value = status;
            }
        }
    });
}
function OpenPopUPUserWiseQuotaion() {
    cgridUserWiseQuotation.PerformCallback();
    cPopupUserWiseQuotation.Show();
}
document.onkeydown = function (e) {
    if (event.keyCode == 18) isCtrl = true;
    if (event.keyCode == 65 && isCtrl == true) { //run code for Ctrl+S -- ie, Save & New  
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
function OnClickDelete(keyValue) {
    var IsClosedFlag = cGrdQuotation.GetRow(cGrdQuotation.GetFocusedRowIndex()).children[5].innerText;
    if (IsClosedFlag != "Closed") {
    $.ajax({
        type: "POST",
        url: "SalesQuotationList.aspx/GetSalesQuoteIsExistInSalesOrder",
        data: "{'keyValue':'" + keyValue + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {                   
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
                        cGrdQuotation.PerformCallback('Delete~' + keyValue);
                    }
                });
            }
        }
    }); 
    }
    else {
        jAlert("Sales Quotation is " + IsClosedFlag.trim() + ".Delete is not allowed.");
    }
}
function OnClickStatus(keyValue) {
    GetObjectID('hiddenedit').value = keyValue;
    cGrdQuotation.PerformCallback('Edit~' + keyValue);
}

function fn_QuotationOpen(keyValue) {
    GetObjectID('hiddenedit').value = keyValue;
    cGrdQuotation.PerformCallback('Open~' + keyValue);
}
function fn_QuotationClose(keyValue) {
    GetObjectID('hiddenedit').value = keyValue;
    cGrdQuotation.PerformCallback('Close~' + keyValue);
}

function selectValue()
{
    var checked_StatusType = $("[id*=StatusType] input:checked");
    var status = checked_StatusType.val();
    if (status == "Open")
    {
        $('#divOpen').show();
        $('#DivClose').hide();
        
    }
    else {
        $('#DivClose').show();
        $('#divOpen').hide();
    }
   
}


function OpenPopUPApprovalStatus() {
    cgridPendingApproval.PerformCallback();
    cpopupApproval.Show();
}
var PIQAId = 0;
function onPrintJv(id) {
    debugger;
    PIQAId = id;
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
        var module = 'PIQuotation';
        window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + PIQAId, '_blank')
    }
    //cSelectPanel.cpSuccess = null
    if (cSelectPanel.cpSuccess == "") {
        cCmbDesignName.SetSelectedIndex(0);
    }
}
function  OpenWaitingQuote()
{
    Cpopup_QuoteWait.Show();
    CwatingQuotegrid.Focus();
}
function QuotationWattingOkClick() {
    debugger;
    var index = CwatingQuotegrid.GetFocusedRowIndex();
    var listKey = CwatingQuotegrid.GetRowKey(index);
    if (listKey) {
        if (CwatingQuotegrid.GetRow(index).children[6].innerText != "Advance") {
            var url = 'SalesQuotation.aspx?key=' + 'ADD&&BasketId=' + listKey;
            LoadingPanel.Show();
            window.location.href = url;
        } else {
            ShowbasketReceiptPayment(listKey);
        }
    }
}
function RemoveQuote(obj) {
    if (obj) {
        jConfirm("Clicking on Delete will not allow to use this Billing request again. Are you sure?", "Alert", function (ret) {
            if (ret) {
                CwatingQuotegrid.PerformCallback('Remove~' + obj);
            }
        });

    }
}
function watingQuotegridEndCallback() {
    if (CwatingQuotegrid.cpReturnMsg) {
        if (CwatingQuotegrid.cpReturnMsg != "") {
            jAlert(CwatingQuotegrid.cpReturnMsg);
            document.getElementById('waitingQuotationCount').value = parseFloat(document.getElementById('waitingQuotationCount').value) - 1;
            CwatingQuotegrid.cpReturnMsg = null;
        }
    }
}
function ListRowClicked(s, e) {

    var index = e.visibleIndex;
    var listKey = CwatingQuotegrid.GetRowKey(index);
    if (e.htmlEvent.target.id != "CloseRemoveWattingBtn") {
        if (CwatingQuotegrid.GetRow(index).children[6].innerText != "Advance") {
            var url = 'SalesQuotation.aspx?key=' + 'ADD&BasketId=' + listKey;
            //LoadingPanel.Show();
            window.location.href = url;
        } else {
            // ShowbasketReceiptPayment(listKey);
        }
    }
}
function OnWaitingGridKeyPress(e) {

    if (e.code == "Enter") {
        var index = CwatingQuotegrid.GetFocusedRowIndex();
        var listKey = CwatingQuotegrid.GetRowKey(index);
        if (listKey) {
            if (CwatingQuotegrid.GetRow(index).children[6].innerText != "Advance") {
                var url = 'SalesQuotation.aspx?key=' + 'ADD&&BasketId=' + listKey;
                // LoadingPanel.Show();
                window.location.href = url;
            } else {
                //ShowbasketReceiptPayment(listKey);
            }
        }
    }

}

function grid_EndCallBack() {
    if (cGrdQuotation.cpEdit != null) {
        GetObjectID('hiddenedit').value = cGrdQuotation.cpEdit.split('~')[0];
        cProforma.SetText(cGrdQuotation.cpEdit.split('~')[1]);
        cCustomer.SetText(cGrdQuotation.cpEdit.split('~')[4]);
        var pro_status = cGrdQuotation.cpEdit.split('~')[2]
        //cGrdQuotation.cpEdit = null;
        if (pro_status != null) {
            var radio = $("[id*=rbl_QuoteStatus] label:contains('" + pro_status + "')").closest("td").find("input");
            radio.attr("checked", "checked");
            //return false;
            //$('#rbl_QuoteStatus[type=radio][value=' + pro_status + ']').prop('checked', true); 
            cQuotationRemarks.SetText(cGrdQuotation.cpEdit.split('~')[3]);

            cQuotationStatus.Show();
        }
    }
    if (cGrdQuotation.cpClose != null) {
        GetObjectID('hiddenedit').value = cGrdQuotation.cpClose.split('~')[0];
        cProformaClose.SetText(cGrdQuotation.cpClose.split('~')[1]);
        cCloseCustomer.SetText(cGrdQuotation.cpClose.split('~')[4]);
        var pro_status = cGrdQuotation.cpClose.split('~')[2]
        //cGrdQuotation.cpEdit = null;
        if (pro_status != null) {
            if (pro_status == "Close")
            {
                $("#ddlCloseReason").val(cGrdQuotation.cpClose.split('~')[3]);
                cQuotationClose.Show();
            }
            else {
                jAlert("Status already Open");
                return false;
            }
           
        }
    }
    if (cGrdQuotation.cpOpen != null) {
        GetObjectID('hiddenedit').value = cGrdQuotation.cpOpen.split('~')[0];
        cOpenProforma.SetText(cGrdQuotation.cpOpen.split('~')[1]);
        cCustomerOpen.SetText(cGrdQuotation.cpOpen.split('~')[4]);
        var pro_status = cGrdQuotation.cpOpen.split('~')[2];
        cOpenCloseRemarks.SetText(cGrdQuotation.cpOpen.split('~')[5]);
        //cGrdQuotation.cpEdit = null;
        if (pro_status != null) {
            if (pro_status == "Open") {
                $("#StatusType").find("input[value=Open]").prop('checked', true);
              //  radio.attr("checked", "checked");
                $("#ddlOpen").val(cGrdQuotation.cpOpen.split('~')[3]);
                $("#divOpen").show();
                $("#DivClose").hide();
            }
            else if (pro_status == "Close") {
                $("#StatusType").find("input[value=Close]").prop('checked', true);
               // radio.attr("checked", "checked");
                $("#ddlClose").val(cGrdQuotation.cpOpen.split('~')[3]);
                $("#DivClose").show();
                $("#divOpen").hide();
            }
            cQuotationOpen.Show();
        }
    }
    if (cGrdQuotation.cpUpdate != null) {
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
        jAlert(cGrdQuotation.cpUpdate);
        cGrdQuotation.Refresh();
    }
    if (cGrdQuotation.cpDelete != null) {
        jAlert(cGrdQuotation.cpDelete);
        cGrdQuotation.cpDelete = null;
        cGrdQuotation.Refresh();
    }

    if (cGrdQuotation.cpUpdateOpen != null) {
        GetObjectID('hiddenedit').value = '';
        cProforma.SetText('');
        cCustomer.SetText('');
        $("#ddlOpen").val("");
        jAlert(cGrdQuotation.cpUpdateOpen);
        cQuotationOpen.Hide();
        cGrdQuotation.cpUpdateOpen = null;
        cGrdQuotation.Refresh();
    }

    if (cGrdQuotation.cpUpdateClose != null) {
        GetObjectID('hiddenedit').value = '';
        cProforma.SetText('');
        cCustomer.SetText('');
        $("#ddlCloseReason").val("");
        jAlert(cGrdQuotation.cpUpdateClose);
        cQuotationClose.Hide();
        cGrdQuotation.cpUpdateClose = null;
        cGrdQuotation.Refresh();
    }
}
function SavePrpformaStatus() {
    if (document.getElementById('hiddenedit').value == '') {
        cGrdQuotation.PerformCallback('save~');
    }
    else {
        var checked_radio = $("[id*=rbl_QuoteStatus] input:checked");
        var status = checked_radio.val();
        var remarks = cQuotationRemarks.GetText();
        cGrdQuotation.PerformCallback('update~' + GetObjectID('hiddenedit').value + '~' + status + '~' + remarks);
    }
}

function SaveQuotationOpenStatus() {   
    var OpenCloseReason = "";
    var checked_radio = $("[id*=StatusType] input:checked");
    var status = checked_radio.val();
    if (status == "Open")
    {
        OpenCloseReason = $("#ddlOpen").val();
    }
    else
    {
        OpenCloseReason = $("#ddlClose").val();
    }
    var OpenCloseRemarks = cOpenCloseRemarks.GetValue();
    cGrdQuotation.PerformCallback('OpenCloseSaveUpdate~' + GetObjectID('hiddenedit').value + '~' + OpenCloseReason + '~' + status + '~' + OpenCloseRemarks);
}


function SaveQuotationCloseStatus() {
    var CloseReason = $("#ddlCloseReason").val();
    var OpenCloseRemarks = cReOpenCloseRemarks.GetValue();
    cGrdQuotation.PerformCallback('OpenSaveUpdate~' + GetObjectID('hiddenedit').value + '~' + CloseReason + '~' + OpenCloseRemarks);
}



       
function OnViewClick(keyValue) {
    var url = 'SalesQuotation.aspx?key=' + keyValue + '&req=V' + '&type=QO';
    window.location.href = url;
}    
function OnclickViewAttachment(obj) {
    //var URL = '/OMS/Management/Activities/Quotation_Document.aspx?idbldng=' + obj + '&type=Quotation';
    var URL = '/OMS/Management/Activities/EntriesDocuments.aspx?idbldng=' + obj + '&type=Quotation';
    window.location.href = URL;
}
function OnAddButtonClick() {
    var url = 'SalesQuotation.aspx?key=' + 'ADD';
    window.location.href = url;
}
var keyval;
function GetApprovedQuoteId(s, e, itemIndex) {
    var rowvalue = cgridPendingApproval.GetRowValues(itemIndex, 'Quote_Id', OnGetApprovedRowValues);
    cgridPendingApproval.PerformCallback('Status~' + rowvalue);
    cgridPendingApproval.GetRowValues(itemIndex, 'Quote_Id', OnGetApprovedRowValues);

}
function OnGetApprovedRowValues(obj) {
    uri = "SalesQuotation.aspx?key=" + obj + "&status=2" + '&type=QO';
    popup.SetContentUrl(uri);
    popup.Show();
    //window.location.href = uri;

}
function ch_fnApproved() {
}


function GetRejectedQuoteId(s, e, itemIndex) {
    debugger;
    cgridPendingApproval.GetRowValues(itemIndex, 'Quote_Id', OnGetRejectedRowValues);

}
function OnGetRejectedRowValues(obj) {
    uri = "SalesQuotation.aspx?key=" + obj + "&status=3" + '&type=QO';
    popup.SetContentUrl(uri);
    popup.Show();
}
// User Approval Status End
function OnApprovalEndCall(s, e) {
    $.ajax({
        type: "POST",
        url: "SalesQuotationList.aspx/GetPendingCase",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $('#lblWaiting').text(data.d);
        }
    });
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
        $.each(lists, function (index, value) {
            setTimeout(function () {
                console.log(value);
                $(value).css({ 'opacity': '1' });
            }, 100);
        });
    }, 200);
}


//End For Copy
//Start For Close Sales Quotation
function fn_CloseSalesOrder(keyValue) {
    jConfirm('Do you want to close the Order ?', 'Confirmation Dialog', function (r) {
        debugger;
        if (r == true) {
            // $("hddnKeyValue").val(keyValue);
            hddnKeyValue.value = keyValue;
            $('#MandatoryRemarksFeedback1').attr('style', 'display:none;position: absolute; right: -20px; top: 8px;');
            cPopup_Closed.Show();
            // CloseSalesQuotationQuantity(keyValue);
        }
    });
}
function CloseSalesQuotationQuantity(Quote_Id,Remarks) {
    debugger;
    $.ajax({
        type: "POST",
        url: "SalesQuotation.aspx/CloseQuotationquantity",
        data: JSON.stringify({ Quote_Id: Quote_Id, Remarks: Remarks }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var status = msg.d;
            if (status == "1")
            {
                            
                //jAlert("Sales Quotation is closed successfully.");
                jAlert("Sales Quotation is closed successfully.", "Approval", function () {
                    location.reload();
                });
                //
            }
            //var url = 'SalesQuotation.aspx?key=' + keyValue + '&Permission=' + status + '&type=QO';
            //window.location.href = url;
        }
    });
}
     
function CancelClosed_save() {
    
    txtClosed.SetValue();
    cPopup_Closed.Hide();
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
        CloseSalesQuotationQuantity(KeyVal,Remarks);
        cPopup_Closed.Hide();
                 
        cGrdOrder.Refresh();
        //cGrdOrder.PerformCallback('Feedback~' + KeyVal + '~' + Remarks);
        //Grid.Refresh();


    }
    return flag;

}


    var isFirstTime = true;

function AllControlInitilize() {
    if (isFirstTime) {
        if (localStorage.getItem('QuotationList_FromDate')) {
            var fromdatearray = localStorage.getItem('QuotationList_FromDate').split('-');
            var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
            cFormDate.SetDate(fromdate);
        }

        if (localStorage.getItem('QuotationList_ToDate')) {
            var todatearray = localStorage.getItem('QuotationList_ToDate').split('-');
            var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
            ctoDate.SetDate(todate);
        }

        if (localStorage.getItem('QuotationList_Branch')) {
            if (ccmbBranchfilter.FindItemByValue(localStorage.getItem('QuotationList_Branch'))) {
                ccmbBranchfilter.SetValue(localStorage.getItem('QuotationList_Branch'));
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
        localStorage.setItem("QuotationList_FromDate", cFormDate.GetDate().format('yyyy-MM-dd'));
        localStorage.setItem("QuotationList_ToDate", ctoDate.GetDate().format('yyyy-MM-dd'));
        localStorage.setItem("QuotationList_Branch", ccmbBranchfilter.GetValue());

        $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
        $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
        $("#hfBranchID").val(ccmbBranchfilter.GetValue());
        $("#hfIsFilter").val("Y");

        //cGrdQuotation.Refresh();
        $("#hFilterType").val("All");
        cCallbackPanel.PerformCallback("");
        //cGrdQuotation.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue())
    }
}
function CallbackPanelEndCall(s, e) {
    cGrdQuotation.Refresh();
}
$(document).ready(function () {
    //Toggle fullscreen expandEntryGrid
    $("#expandcGrdQuotation").click(function (e) {
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
            cGrdQuotation.SetHeight(browserHeight - 150);
            cGrdQuotation.SetWidth(cntWidth);
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
            cGrdQuotation.SetHeight(300);
            var cntWidth = $this.parent('.makeFullscreen').width();
            cGrdQuotation.SetWidth(cntWidth);
        }
    });
});

    $(document).ready(function () {
        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                cGrdQuotation.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                cGrdQuotation.SetWidth(cntWidth);
            }

            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    cGrdQuotation.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    cGrdQuotation.SetWidth(cntWidth);
                }

            });
        });
            
    });
