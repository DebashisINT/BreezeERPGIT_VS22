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
    uri = "PurchaseChallan_Stock.aspx?key=" + obj + "&status=2" + '&type=PC';
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
    uri = "PurchaseChallan_Stock.aspx?key=" + obj + "&status=3" + '&type=PC';
    popup.SetContentUrl(uri);
    popup.Show();
}
// function above  End For Rejected
// To Reflect the Data in Pending Waiting Grid and Pending Waiting Counting if the user approve or Rejecte the Order and Saved 
function OnApprovalEndCall(s, e) {
    $.ajax({
        type: "POST",
        url: "PurchaseChallanList_Stock.aspx/GetPendingCase",
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
    debugger;
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
    debugger;
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

    //$('#ApprovalCross').click(function () {
    //    debugger;
    //    window.parent.popup.Hide();
    //    window.parent.cgridPendingApproval.Refresh()();
    //})
})
document.onkeydown = function (e) {
    //if (event.keyCode == 18) isCtrl = true;

    if (event.keyCode == 65 && event.altKey == true) { //run code for alt+a -- ie, Add
        StopDefaultAction(e);
        AddButtonClick();
    }


}
function StopDefaultAction(e) {
    if (e.preventDefault) { e.preventDefault() }
    else { e.stop() };

    e.returnValue = false;
    e.stopPropagation();
}
function AddButtonClick() {
    var url = 'PurchaseChallan_Stock.aspx?key=' + 'ADD';
    window.location.href = url;
}
function OnMoreInfoClick(keyValue) {
    var url = 'PurchaseChallan_Stock.aspx?key=' + keyValue + '&type=PC';
    window.location.href = url;
}

////##### coded by Samrat Roy - 04/05/2017  
////Add an another param to define request type 
function OnViewClick(keyValue) {
    var url = 'PurchaseChallan_Stock.aspx?key=' + keyValue + '&req=V' + '&type=PC';
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
        //window.location.href = "PurchaseChallanList_Stock_Stock.aspx";
    }
}
function OnclickViewAttachment(obj) {
    //var URL = '/OMS/Management/Activities/SalesInvoice_Document.aspx?idbldng=' + obj + '&type=SalesInvoice';
    var URL = '/OMS/Management/Activities/EntriesDocuments.aspx?idbldng=' + obj + '&type=PurchaseChallan_Stock';
    window.location.href = URL;
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
        localStorage.setItem("GRNList_FromDate", cFormDate.GetDate().format('yyyy-MM-dd'));
        localStorage.setItem("GRNList_ToDate", ctoDate.GetDate().format('yyyy-MM-dd'));
        localStorage.setItem("GRNList_Branch", ccmbBranchfilter.GetValue());

        CgvPurchaseOrder.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue())
    }
}
$(document).ready(function () {
    //Toggle fullscreen expandEntryGrid
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
            CgvPurchaseOrder.SetHeight(300);
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
