
var SOrderId = 0;
function onPrintJv(id) {
 $("#hdnOrderID").val(id);
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

     $.ajax({
                type: "POST",
                url: "ApproveSaleasOrder.aspx/InsertUpdateReadyToInvoice",
                data: JSON.stringify({ Orderid: $("#hdnOrderID").val(), Stage: 1, Remarks: '' }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    var status = msg.d;
                    if (status == 'OK') {
                       

                        $.ajax({
                            type: "POST",
                            url: "ApproveSaleasOrder.aspx/ButtonCountShow",
                            data: JSON.stringify({ FormDate: $("#hfFromDate").val(), toDate: $("#hfToDate").val(), Branch: $("#hfBranchID").val() }),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: false,
                            success: function (msg) {
                                //alert(msg.d);
                                var status = msg.d;
                                // if (msg.d.length>0) {
                                    $("#spnPrintedlvl").html(msg.d.Printed);
                                    $("#spnReadyforInvoicelvl").html(msg.d.ReadyForInvoice);
                                    $("#spnPendinglvl").html(msg.d.Pending);
                                    $("#spnAlllvl").html(msg.d.All);
                                //}
                                //else {
                                //    $("#spnPrintedlvl").html(0);
                                //    $("#spnReadyforInvoicelvl").html(0);
                                //    $("#spnPendinglvl").html(0);
                                //    $("#spnAlllvl").html(0);
                                //}
                            }
                        });

                        cCallbackPanel.PerformCallback("");
                       // cGrdOrder.Refresh();
                    }
                }
            });

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
        $("#hFilterType").val("");
        
        $.ajax({
            type: "POST",
            url: "ApproveSaleasOrder.aspx/ButtonCountShow",
            data: JSON.stringify({ FormDate: $("#hfFromDate").val(),toDate:$("#hfToDate").val(),Branch:$("#hfBranchID").val() }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {
                //alert(msg.d);
                var status = msg.d;
                //if (msg.d.length>0) {
                    $("#spnPrintedlvl").html(msg.d.Printed);
                    $("#spnReadyforInvoicelvl").html(msg.d.ReadyForInvoice);
                    $("#spnPendinglvl").html(msg.d.Pending);
                    $("#spnAlllvl").html(msg.d.All);
                //}
                //else {
                //    $("#spnPrintedlvl").html(0);
                //    $("#spnReadyforInvoicelvl").html(0);
                //    $("#spnPendinglvl").html(0);
                //    $("#spnAlllvl").html(0);
                //}
            }
        });

        $("#hFilterType").val("Pending");
        //cGrdOrder.Refresh();
        cCallbackPanel.PerformCallback("");

    }
}
//End

function btn_PrintedClick(e) {
    $("#hFilterType").val("Printed");
    cCallbackPanel.PerformCallback("");
}

function btn_ReadyforInvoiceClick(e) {
    $("#hFilterType").val("ReadyforInvoice");
    cCallbackPanel.PerformCallback("");
}

function btn_PendingClick(e) {
    $("#hFilterType").val("Pending");
    cCallbackPanel.PerformCallback("");
}

function btn_AllClick(e) {
    $("#hFilterType").val("ALL");
    cCallbackPanel.PerformCallback("");
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

  

}

function StopDefaultAction(e) {
    if (e.preventDefault) { e.preventDefault() }
    else { e.stop() };

    e.returnValue = false;
    e.stopPropagation();
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
        // cGrdOrder.Refresh();
        cCallbackPanel.PerformCallback("");
    }
}




////##### coded by Samrat Roy - 04/05/2017  
////Add an another param to define request type 
function OnViewClick(keyValue) {
    var url = 'SalesOrderAdd.aspx?key=' + keyValue + '&req=V&isApprovsalesOrder=Y';
    window.location.href = url;
}














function OnAddButtonClick() {
    var url = 'SalesOrderAdd.aspx?key=' + 'ADD';
    window.location.href = url;
}
//});
$(document).ready(function () {

});





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


function UpdateTransporter(values) {
    $("#hdnOrderID").val(values);
    callTransporterControl(values, "SO");
    $("#exampleModal").modal('show');
}

function InsertTransporterControlDetails(data) {
    var orderid = $("#hdnOrderID").val();
    $.ajax({
        type: "POST",
        url: "ApproveSaleasOrder.aspx/InsertTransporterControlDetails",
        data: "{'id':'" + orderid + "','hfControlData':'" + data + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,//Added By:Subhabrata
        success: function (msg) {

        }
    });
}