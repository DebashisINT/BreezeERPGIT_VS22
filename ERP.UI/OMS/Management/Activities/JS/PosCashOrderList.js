
var ReturnId = 0;
function onPrintJv(id) {
    // debugger;
    ReturnId = id;
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
    // debugger;
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

function OnclickViewAttachment(obj) {
    //var URL = '/OMS/Management/Activities/SalesReturn_Document.aspx?idbldng=' + obj + '&type=SalesReturn';
    var URL = '/OMS/Management/Activities/EntriesDocuments.aspx?idbldng=' + obj + '&type=CustomerReturn';
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
    var url = 'CustomerReturn.aspx?key=' + 'ADD';
    window.location.href = url;
}
////##### coded by Samrat Roy - 17/04/2017 - ref IssueLog(P Order - 70) 
////Add an another param to define request type 
function OnViewClick(keyValue) {
    var url = 'CustomerReturn.aspx?key=' + keyValue + '&req=V' + '&type=CR';
    window.location.href = url;
}

function OnClickDelete(keyValue) {
    jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
        if (r == true) {
            cGrdCustomerReturn.PerformCallback('Delete~' + keyValue);
        }
    });
}
function OnEndCallback(s, e) {

    if (cGrdCustomerReturn.cpDelete != null) {
        jAlert(cGrdCustomerReturn.cpDelete);

        cGrdCustomerReturn.cpDelete = null;
        cGrdCustomerReturn.Refresh();
        // window.location.href = "CustomerReturnList.aspx";
    }
}
//function OnClickDelete(keyValue) {
//    jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
//        if (r == true) {
//            cGrdQuotation.PerformCallback('Delete~' + keyValue);
//        }
//    });
//}


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
        cGrdCustomerReturn.Refresh();
        // cGrdCustomerReturn.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue())
    }
}

function gridRowclick(s, e) {
            
    $('#gridcrmCampaign').find('tr').removeClass('rowActive');
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
