//==========================================================Revision History ============================================================================================
//    1.0   Priti   V2.0.36   10-01-2023     0025372: Listing view upgradation required of Branch Transfer Out of Inventory
//    2.0   Priti   V2.0.36   17-02-2023     After Listing view upgradation delete data show in listing issue solved.

//========================================== End Revision History =======================================================================================================--%>


var StockId = 0;
function onPrintJv(id) { 
    
    StockId = id;
    cSelectPanel.cpSuccess = "";
    cDocumentsPopup.Show();
    CselectBranchCopy.SetCheckState('UnChecked');
    CselectGodownCopy.SetCheckState('UnChecked');
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
  
    if (cSelectPanel.cpSuccess != "") {
        var TotDocument = cSelectPanel.cpSuccess.split(',');
        var reportName = cCmbDesignName.GetValue();
        var module = 'BranchTranOut';
        if (TotDocument.length > 0) {
            for (var i = 0; i < TotDocument.length; i++) {
                if (TotDocument[i] != "") {
                    window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + StockId + '&PrintOption=' + TotDocument[i], '_blank')
                }
            }
        }
    }
    //cSelectPanel.cpSuccess = null
    if (cSelectPanel.cpSuccess == "") {
        if (cSelectPanel.cpChecked != "") {
            jAlert('Please check Branch Copy OR Godown Copy and proceed.');
        }
        CselectBranchCopy.SetCheckState('UnChecked');
        CselectGodownCopy.SetCheckState('UnChecked');
        cCmbDesignName.SetSelectedIndex(0);
    }
}


var isFirstTime = true;
function AllControlInitilize() {
    //debugger;
    if (isFirstTime) {

        if (localStorage.getItem('FromDateBTO')) {
            var fromdatearray = localStorage.getItem('FromDateBTO').split('-');
            var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
            cFormDate.SetDate(fromdate);
        }

        if (localStorage.getItem('ToDateBTO')) {
            var todatearray = localStorage.getItem('ToDateBTO').split('-');
            var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
            ctoDate.SetDate(todate);
        }
        if (localStorage.getItem('BranchBTO')) {
            if (ccmbBranchfilter.FindItemByValue(localStorage.getItem('BranchBTO'))) {
                ccmbBranchfilter.SetValue(localStorage.getItem('BranchBTO'));
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
        localStorage.setItem("FromDateBTO", cFormDate.GetDate().format('yyyy-MM-dd'));
        localStorage.setItem("ToDateBTO", ctoDate.GetDate().format('yyyy-MM-dd'));
        localStorage.setItem("BranchBTO", ccmbBranchfilter.GetValue());

        $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
        $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
        $("#hfBranchID").val(ccmbBranchfilter.GetValue());
        $("#hfIsFilter").val("Y");

        //REV 1.0
        //cGrdOrder.Refresh();
        $("#hFilterType").val("All");
        cCallbackPanel.PerformCallback("");
        //END REV 1.0
        //cGrdOrder.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue());

    }
}
//End


 //REV 1.0
function CallbackPanelEndCall(s, e) {
    cGrdOrder.Refresh();
}
 //END REV 1.0

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
function OnClickDelete(keyValue) {
    debugger;
    $.ajax({
        type: "POST",
        url: "BranchTransferOutList.aspx/GetBOIsExistInBI",
        data: "{'keyValue':'" + keyValue + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,//Added By:Subhabrata
        success: function (msg) {
            debugger;
            var status = msg.d;
            if (status == "1") {
                jAlert('Branch Stock In for the particular document has  been done. Cannot Delete', 'Alert Dialog', function (r) {
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

function OnclickViewAttachment(obj) {
    var URL = '/OMS/Management/Activities/EntriesDocuments.aspx?idbldng=' + obj + '&type=BTOUT';
    window.location.href = URL;
}
function OnClickStatus(keyValue) {
    GetObjectID('hiddenedit').value = keyValue;
    cGrdOrder.PerformCallback('Edit~' + keyValue);
}
function grid_EndCallBack() {
    //debugger;

    if (cGrdOrder.cpEdit != null) {
        GetObjectID('hiddenedit').value = cGrdOrder.cpEdit.split('~')[0];
        cProforma.SetText(cGrdOrder.cpEdit.split('~')[1]);
        cCustomer.SetText(cGrdOrder.cpEdit.split('~')[4]);
        var pro_status = cGrdOrder.cpEdit.split('~')[2];
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
       
        /* Rev 2.0*/
         cGrdOrder.Refresh();
        //updateGridByDate();
            //end rev 2.0
    }
    if (cGrdOrder.cpCancelUpdate == 'Reason saved successfully') {
        cGrdOrder.cpCancelUpdate = null;
        $("#hddnIsSavedFeedback").val("1");
        txtFeedback.SetValue();
        var keyValue = $("#hddnKeyValue").val();
        CancelBranchTransferOut(keyValue);
        cGrdOrder.Refresh();

    }
    if (cGrdOrder.cpCancelUpdate == 'Reason not saved') {
        cGrdOrder.cpCancelUpdate = null;
        var keyValue = $("#hddnKeyValue").val();
        CancelBranchTransferOut(keyValue);

    }
}

function CancelBranchTransferOut(keyValue) {
    debugger;
    $.ajax({
        type: "POST",
        url: "BranchTransferOutList.aspx/CancelBranchTransferOutOnRequest",
        data: "{'keyValue':'" + keyValue + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,//Added By:Subhabrata
        success: function (msg) {
            debugger;
            var status = msg.d;
            if (status == "1") {
                jAlert("BTO is cancelled successfully.");
                //cGrdOrder.PerformCallback('BindGrid');
                        
            }
            else if (status == "-1") {
                jAlert("BTO is not cancelled.Try again later");
            }
            else if (status == "-2") {
                jAlert("BTO is taggged in Branch Transfer In.Can not proceed.");
            }
        }
    });
}


function SavePrpformaStatus() {
    if (document.getElementById('hiddenedit').value == '') {
        cGrdOrder.PerformCallback('save~');
    }
    else {
        cGrdOrder.PerformCallback('update~' + GetObjectID('hiddenedit').value);
    }

}

function CallFeedback_save() {
    debugger;
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
        cGrdOrder.PerformCallback('Feedback~' + KeyVal + '~' + Remarks);
        //Grid.Refresh();
    }
    return flag;

}




function CancelFeedback_save() {

    $("#hddnIsSavedFeedback").val("0");
    txtFeedback.SetValue();
    cPopup_Feedback.Hide();
    $('#chkmailfeedback').prop('checked', false);
}


function OnCancelClick(keyValue, visibleIndex) {
    debugger;
    $("#hddnKeyValue").val(keyValue);
    cGrdOrder.SetFocusedRowIndex(visibleIndex);
    var IsCancel = cGrdOrder.GetRow(cGrdOrder.GetFocusedRowIndex()).children[8].innerText;
    if (IsCancel.trim() == "True") {
        jAlert("BTO is already cancelled");
    }
    else {
        jConfirm('Do you want to cancel the BTO ?', 'Confirm Dialog', function (r) {
            if (r == true) {
                $('#MandatoryRemarksFeedback').attr('style', 'display:none;position: absolute; right: -20px; top: 8px;');
                cPopup_Feedback.Show();
            }
            else {
                return false;
            }
        });

        //$.ajax({
        //    type: "POST",
        //    url: "BranchTransferOutList.aspx/CancelBranchTransferOutOnRequest",
        //    data: "{'keyValue':'" + keyValue + "'}",
        //    contentType: "application/json; charset=utf-8",
        //    dataType: "json",
        //    async: false,//Added By:Subhabrata
        //    success: function (msg) {
        //        debugger;
        //        var status = msg.d;
        //        if (status == "1") {
        //            jAlert("BTO is cancelled successfully.");
        //        }
        //        else if (status == "-1") {
        //            jAlert("BTO is not cancelled.Try again later");
        //        }
        //        else if (status == "-2") {
        //            jAlert("BTO is taggged in Branch Transfer In.Can not proceed.");
        //        }
        //    }
        //});

    }


}



////##### coded by Samrat Roy - 04/05/2017  
////Add an another param to define request type 
function OnViewClick(keyValue) {
    var url = 'BranchTransferOut.aspx?key=' + keyValue + '&req=V';
    window.location.href = url;
}

function OnAddButtonClick() {
    var url = 'BranchTransferOut.aspx?key=' + 'ADD';
    window.location.href = url;
}
//});

// Mantis Issue 25238
function onSmsClickJv(key) {
    jConfirm('Send SMS?', 'Alert', function (r) {
        if (r) {
            $.ajax({
                type: "POST",
                url: "BranchTransferOutLEntityList.aspx/SendSMSManualNo",
                data: JSON.stringify({ BranchTransferOutId: key }),
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
// End of Mantis Issue 25238

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

        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                cGrdOrder.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                cGrdOrder.SetWidth(cntWidth);
            }
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
