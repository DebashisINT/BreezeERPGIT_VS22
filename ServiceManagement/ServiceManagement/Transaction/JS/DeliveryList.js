
$(document).ready(function () {
    $('#dataTable').DataTable({
        scrollX: true,
        ordering: false,
        sortable: false,
        fixedColumns: {
            rightColumns: 1
        }
    });
    $(".date").datepicker({
        autoclose: true,
        todayHighlight: true,
        format: 'dd-mm-yyyy'
    }).datepicker('update', new Date());

    $('#filterToggle').hide();

    $('.togglerSlide').click(function () {
        $('#filterToggle').slideDown({
            duration: 200,
            easing: "easeOutQuad"
        });
        $(this).hide()
    });
    $('.togglerSlidecut').click(function () {
        $('#filterToggle').slideUp({
            duration: 200,
            easing: "easeOutQuad"
        });
        $('.togglerSlide').show();
    });

});

//<%-- Code Added By Debashis Talukder For Document Printing End--%>
var RCId = 0;
function onPrintJv(id) {
    RCId = id;
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
        var TotDocument = cSelectPanel.cpSuccess.split(',');
        var reportName = cCmbDesignName.GetValue();
        var module = 'DELIVERYCHALLAN';
        window.open("../../../OMS/Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + RCId, '_blank')
    }
    cSelectPanel.cpSuccess = null
    if (cSelectPanel.cpSuccess == null) {
        cCmbDesignName.SetSelectedIndex(0);
    }
}
//<%-- Code Added By Debashis Talukder For Document Printing End--%>

$(document).ready(function () {

    //if ($("#hdnUserType").val() == "Technician") {
    //    $("#LiTotalJob").addClass('hide');
    //    $("#home").addClass('hide');
    //    $("#spnMassAssign").addClass('hide');
    //    MyJob();
    //}
    //else {
    //    $("#LiTotalJob").removeClass('hide');
    //    $("#home").removeClass('hide');
    //    $("#spnMassAssign").removeClass('hide');
    //}

    DeliveryCount();

});

function DeliveryCount() {
    $.ajax({
        type: "POST",
        url: "DeliveryList.aspx/CountJob",
        data: JSON.stringify({ UserType: $("#hdnUserType").val() }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var list = msg.d;
            $("#DivTotalEntered").text(msg.d.TotalEntered);
            $("#DivTotalDelivered").text(msg.d.TotalDelivered);
            $("#divPendingdelivery").text(msg.d.Pendingdelivery);
        }
    });
}

function TotalEntered() {
    LoadingPanel.Show();
    $("#hdnSearchType").val("TotalEntered");
    $('.circulerUl>li').removeClass('activeCrcl').addClass('ds');
    $('#d1').addClass('activeCrcl');
    $('#DivSearchTechinician').removeClass('hide');
    //    console.log(this);
    // setTimeout(function () {
    $.ajax({
        type: "POST",
        url: "DeliveryList.aspx/BindTotalList",
        data: JSON.stringify({ Type: "TotalEntered" }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            $("#DivTotalEntered").text(msg.d.TotalEntered);
            $("#DivTotalDelivered").text(msg.d.TotalDelivered);
            $("#divPendingdelivery").text(msg.d.Pendingdelivery);

            var status = "<div>";
            status = status + "<table id='dataTable' class='table table-striped table-bordered display nowrap' style='width:100%'>";
            status = status + " <thead><tr>";
            status = status + " <th>Receipt Challan</th><th>Type</th><th>Entity Code </th><th>Network Name</th><th>Contact Person</th><th>Technician </th>";
            status = status + " <th>Location </th><th>Received By</th><th>Received On</th><th>Assigned By</th><th>Assigned On</th><th>Serv. Entered By</th>";
            status = status + " <th>Serv. Entered On</th><th>Delivery By</th><th>Delivery On</th><th>Delivery Update By</th><th>Delivery Update On</th><th>Status</th><th>Action</th>";
            status = status + " </tr></thead>";
            status = status + " </table>";
            status = status + "</div>";
            $('#DivDeliveryDetails').html(status);

            $('#dataTable').DataTable({
                scrollX: true,
                ordering: false,
                sortable: false,
                fixedColumns: {
                    rightColumns: 2
                },
                data: msg.d.DetailsList,
                columns: [
                   { 'data': 'ReceiptChallan' },
                   { 'data': 'Type' },
                   { 'data': 'EntityCode' },
                   { 'data': 'NetworkName' },
                   { 'data': 'ContactPerson' },
                   { 'data': 'Technician' },
                   { 'data': 'Location' },
                   { 'data': 'Receivedby' },
                   { 'data': 'Receivedon' },
                   { 'data': 'Assignedby' },
                   { 'data': 'Assignedon' },
                   { 'data': 'ServEnteredBy' },
                   { 'data': 'ServEnteredOn' },
                   { 'data': 'DeliveryBy' },
                   { 'data': 'DeliveryOn' },
                   { 'data': 'UpdateBy' },
                   { 'data': 'UpdateOn' },
                   { 'data': 'Status' },
                   { 'data': 'Action' },
                ],
                dom: 'Bfrtip',
                buttons: [
                    {
                        extend: 'excel',
                        title: null,
                        filename: 'Delivery',
                        text: 'Save as Excel',
                        customize: function (xlsx) {
                            var sheet = xlsx.xl.worksheets['sheet1.xml'];
                            $('row:first c', sheet).attr('s', '42');
                        },

                        exportOptions: {
                        }
                    }
                ],
                error: function (error) {
                    alert(error);
                }
            });

            LoadingPanel.Hide();
        }
    });
    // }, 1000);
}

function TotalDelivered() {
    LoadingPanel.Show();
    $("#hdnSearchType").val("TotalDelivered");
    $('.circulerUl>li').removeClass('activeCrcl').addClass('ds');
    $('#d2').addClass('activeCrcl');
    $('#DivSearchTechinician').removeClass('hide');
    $.ajax({
        type: "POST",
        url: "DeliveryList.aspx/BindTotalList",
        data: JSON.stringify({ Type: "TotalDelivered" }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            $("#DivTotalEntered").text(msg.d.TotalEntered);
            $("#DivTotalDelivered").text(msg.d.TotalDelivered);
            $("#divPendingdelivery").text(msg.d.Pendingdelivery);

            var status = "<div>";
            status = status + "<table id='dataTable' class='table table-striped table-bordered display nowrap' style='width:100%'>";
            status = status + " <thead><tr>";
            status = status + " <th>Receipt Challan</th><th>Type</th><th>Entity Code </th><th>Network Name</th><th>Contact Person</th><th>Technician </th>";
            status = status + " <th>Location </th><th>Received By</th><th>Received On</th><th>Assigned By</th><th>Assigned On</th><th>Serv. Entered By</th>";
            status = status + " <th>Serv. Entered On</th><th>Delivery By</th><th>Delivery On</th><th>Delivery Update By</th><th>Delivery Update On</th><th>Status</th><th>Action</th>";
            status = status + " </tr></thead>";
            status = status + " </table>";
            status = status + "</div>";
            $('#DivDeliveryDetails').html(status);

            $('#dataTable').DataTable({
                scrollX: true,
                fixedColumns: {
                    rightColumns: 2
                },
                data: msg.d.DetailsList,
                columns: [
                   { 'data': 'ReceiptChallan' },
                   { 'data': 'Type' },
                   { 'data': 'EntityCode' },
                   { 'data': 'NetworkName' },
                   { 'data': 'ContactPerson' },
                   { 'data': 'Technician' },
                   { 'data': 'Location' },
                   { 'data': 'Receivedby' },
                   { 'data': 'Receivedon' },
                   { 'data': 'Assignedby' },
                   { 'data': 'Assignedon' },
                   { 'data': 'ServEnteredBy' },
                   { 'data': 'ServEnteredOn' },
                   { 'data': 'DeliveryBy' },
                   { 'data': 'DeliveryOn' },
                   { 'data': 'UpdateBy' },
                   { 'data': 'UpdateOn' },
                   { 'data': 'Status' },
                   { 'data': 'Action' },
                ],
                dom: 'Bfrtip',
                buttons: [
                    {
                        extend: 'excel',
                        title: null,
                        filename: 'Delivery',
                        text: 'Save as Excel',
                        customize: function (xlsx) {
                            var sheet = xlsx.xl.worksheets['sheet1.xml'];
                            $('row:first c', sheet).attr('s', '42');
                        },

                        exportOptions: {
                        }
                    }
                ],
                error: function (error) {
                    alert(error);
                }
            });

            LoadingPanel.Hide();
        }
    });
}

function Pendingdelivery() {
    LoadingPanel.Show();
    $("#hdnSearchType").val("Pendingdelivery");
    $('.circulerUl>li').removeClass('activeCrcl').addClass('ds');
    $('#d3').addClass('activeCrcl');
    $('#DivSearchTechinician').addClass('hide');
    $.ajax({
        type: "POST",
        url: "DeliveryList.aspx/BindTotalList",
        data: JSON.stringify({ Type: "Pendingdelivery" }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            $("#DivTotalEntered").text(msg.d.TotalEntered);
            $("#DivTotalDelivered").text(msg.d.TotalDelivered);
            $("#divPendingdelivery").text(msg.d.Pendingdelivery);

            var status = "<div>";
            status = status + "<table id='dataTable' class='table table-striped table-bordered display nowrap' style='width:100%'>";
            status = status + " <thead><tr>";
            status = status + " <th>Receipt Challan</th><th>Type</th><th>Entity Code </th><th>Network Name</th><th>Contact Person</th><th>Technician </th>";
            status = status + " <th>Location </th><th>Received By</th><th>Received On</th><th>Assigned By</th><th>Assigned On</th><th>Serv. Entered By</th>";
            status = status + " <th>Serv. Entered On</th><th>Delivery By</th><th>Delivery On</th><th>Delivery Update By</th><th>Delivery Update On</th><th>Status</th><th>Action</th>";
            status = status + " </tr></thead>";
            status = status + " </table>";
            status = status + "</div>";
            $('#DivDeliveryDetails').html(status);

            $('#dataTable').DataTable({
                scrollX: true,
                fixedColumns: {
                    rightColumns: 2
                },
                data: msg.d.DetailsList,
                columns: [
                   { 'data': 'ReceiptChallan' },
                   { 'data': 'Type' },
                   { 'data': 'EntityCode' },
                   { 'data': 'NetworkName' },
                   { 'data': 'ContactPerson' },
                   { 'data': 'Technician' },
                   { 'data': 'Location' },
                   { 'data': 'Receivedby' },
                   { 'data': 'Receivedon' },
                   { 'data': 'Assignedby' },
                   { 'data': 'Assignedon' },
                   { 'data': 'ServEnteredBy' },
                   { 'data': 'ServEnteredOn' },
                   { 'data': 'DeliveryBy' },
                   { 'data': 'DeliveryOn' },
                   { 'data': 'UpdateBy' },
                   { 'data': 'UpdateOn' },
                   { 'data': 'Status' },
                   { 'data': 'Action' },
                ],
                dom: 'Bfrtip',
                buttons: [
                    {
                        extend: 'excel',
                        title: null,
                        filename: 'Delivery',
                        text: 'Save as Excel',
                        customize: function (xlsx) {
                            var sheet = xlsx.xl.worksheets['sheet1.xml'];
                            $('row:first c', sheet).attr('s', '42');
                        },

                        exportOptions: {
                        }
                    }
                ],
                error: function (error) {
                    alert(error);
                }
            });
            LoadingPanel.Hide();
        }
    });
}


function AddDelivery(values) {
    location.href = "DeliveryEntry.aspx?key=ADD&id=" + values;
}

function EditDelivery(values) {
    location.href = "DeliveryEntry.aspx?key=Edit&id=" + values;
}

function ViewDelivery(values) {
    location.href = "DeliveryEntry.aspx?key=View&id=" + values;
}

function OnclickViewAttachment(obj) {
    var URL = '../../../OMS/Management/Activities/EntriesDocuments.aspx?idbldng=' + obj + '&type=SrvDelivery';
    window.location.href = URL;
}

function Delete(values) {
    jConfirm('Confirm Delete?', 'Alert', function (r) {
        if (r) {
            $.ajax({
                type: "POST",
                url: "DeliveryList.aspx/DeleteDelivery",
                data: JSON.stringify({ ReceiptChallanID: values }),
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    //console.log(response);
                    if (response.d) {
                        if (response.d == "true") {
                            jAlert("Delete Successfully.", "Alert", function () {
                                DeliveryCount();
                                TotalEntered();
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

function SearchClick() {
    LoadingPanel.Show();
    var brnch = "";//gridbranchLookup.gridView.GetSelectedKeysOnPage();
    var branchss = "";
    //for (var i = 0; i < brnch.length; i++) {
    //    branchss += ',' + brnch[i];
    //}

    var Tech = "";//gridTechnicianLookup.gridView.GetSelectedKeysOnPage();
    var Technician = "";
    //for (var i = 0; i < Tech.length; i++) {
    //    if (Technician == "") {
    //        Technician = Tech[i];
    //    }
    //    else {
    //        Technician += ',' + Tech[i];
    //    }
    //}

    var SearchType = "";
    if ($("#hdnSearchType").val() == "") {
        SearchType = "TotalEntered";
    }
    else {
        SearchType = $("#hdnSearchType").val();
    }

    gridbranchLookup.gridView.GetSelectedFieldValues("ID", function (val) {
        brnch = val

        for (var i = 0; i < brnch.length; i++) {
            if (branchss == "") {
                branchss = brnch[i];
            }
            else {
                branchss += ',' + brnch[i];
            }
        }
        gridTechnicianLookup.gridView.GetSelectedFieldValues("cnt_internalId", function (val) {
            Tech = val

            for (var i = 0; i < Tech.length; i++) {
                if (Technician == "") {
                    Technician = Tech[i];
                }
                else {
                    Technician += ',' + Tech[i];
                }
            }
            var Apply = {
                FromDate: $("#FromDate").val(),
                ToDate: $("#ToDate").val(),
                Branch: branchss,// $("#ddlBranch").val(),
                SearchType: SearchType,
                Technician_ID: Technician//$("#ddlTechnician").val()
            }

            $.ajax({
                type: "POST",
                url: "DeliveryList.aspx/SearchData",
                data: "{model:" + JSON.stringify(Apply) + "}",
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {

                    $("#DivTotalEntered").text(msg.d.TotalEntered);
                    $("#DivTotalDelivered").text(msg.d.TotalDelivered);
                    $("#divPendingdelivery").text(msg.d.Pendingdelivery);

                    var status = "<div>";
                    status = status + "<table id='dataTable' class='table table-striped table-bordered display nowrap' style='width:100%'>";
                    status = status + " <thead><tr>";
                    status = status + " <th>Receipt Challan</th><th>Type</th><th>Entity Code </th><th>Network Name</th><th>Contact Person</th><th>Technician </th>";
                    status = status + " <th>Location </th><th>Received By</th><th>Received On</th><th>Assigned By</th><th>Assigned On</th><th>Serv. Entered By</th>";
                    status = status + " <th>Serv. Entered On</th><th>Delivery By</th><th>Delivery On</th><th>Delivery Update By</th><th>Delivery Update On</th><th>Status</th><th>Action</th>";
                    status = status + " </tr></thead>";
                    status = status + " </table>";
                    status = status + "</div>";
                    $('#DivDeliveryDetails').html(status);

                    $('#dataTable').DataTable({
                        scrollX: true,
                        fixedColumns: {
                            rightColumns: 2
                        },
                        data: msg.d.DetailsList,
                        columns: [
                           { 'data': 'ReceiptChallan' },
                           { 'data': 'Type' },
                           { 'data': 'EntityCode' },
                           { 'data': 'NetworkName' },
                           { 'data': 'ContactPerson' },
                           { 'data': 'Technician' },
                           { 'data': 'Location' },
                           { 'data': 'Receivedby' },
                           { 'data': 'Receivedon' },
                           { 'data': 'Assignedby' },
                           { 'data': 'Assignedon' },
                           { 'data': 'ServEnteredBy' },
                           { 'data': 'ServEnteredOn' },
                           { 'data': 'DeliveryBy' },
                           { 'data': 'DeliveryOn' },
                           { 'data': 'UpdateBy' },
                           { 'data': 'UpdateOn' },
                           { 'data': 'Status' },
                           { 'data': 'Action' },
                        ],
                        dom: 'Bfrtip',
                        buttons: [
                            {
                                extend: 'excel',
                                title: null,
                                filename: 'Delivery',
                                text: 'Save as Excel',
                                customize: function (xlsx) {
                                    var sheet = xlsx.xl.worksheets['sheet1.xml'];
                                    $('row:first c', sheet).attr('s', '42');
                                },

                                exportOptions: {
                                }
                            }
                        ],
                        error: function (error) {
                            alert(error);
                            LoadingPanel.Hide();
                        }
                    });

                    LoadingPanel.Hide();
                },
                error: function (response) {
                    console.log(response);
                    LoadingPanel.Hide();
                }
            });
        });
    });
}

$(function () {
    cBranchPanel.PerformCallback('BindComponentGrid' + '~' + "All");
    cTechnicianPanel.PerformCallback('BindTechnicianGrid' + '~' + "All");
});

function selectAllBranch() {
    gridbranchLookup.gridView.SelectRows();
}
function unselectAllBranch() {
    gridbranchLookup.gridView.UnselectRows();
}
function CloseBranchLookup() {
    gridbranchLookup.ConfirmCurrentSelection();
    gridbranchLookup.HideDropDown();
    gridbranchLookup.Focus();
}

function selectAllTechnician() {
    gridTechnicianLookup.gridView.SelectRows();
}
function unselectAllTechnician() {
    gridTechnicianLookup.gridView.UnselectRows();
}
function CloseTechnicianLookup() {
    gridTechnicianLookup.ConfirmCurrentSelection();
    gridTechnicianLookup.HideDropDown();
    gridTechnicianLookup.Focus();
}

function ExportChange() {
    if ($("#drdExport").val() == "1") {
        $('#dataTable').DataTable().button('.buttons-pdf').trigger();
    }
    if ($("#drdExport").val() == "2") {
        $('#dataTable').DataTable().button('.buttons-excel').trigger();
    }
    if ($("#drdExport").val() == "4") {
        $('#dataTable').DataTable().button('.buttons-csv').trigger();
    }
}

function ConfirmDelivery(values) {
    jConfirm('Confirm Delivery?', 'Alert', function (r) {
        if (r) {
            $.ajax({
                type: "POST",
                url: "DeliveryList.aspx/ConfirmDelivery",
                data: JSON.stringify({ ReceiptChallanID: values }),
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    //console.log(response);
                    if (response.d) {
                        if (response.d == "true") {
                            jAlert("Delivery Successfully.", "Alert", function () {
                                DeliveryCount();
                                TotalEntered();
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

function MassConfirm() {
    $.ajax({
        type: "POST",
        url: "DeliveryList.aspx/bindMassAssign",
        data: JSON.stringify({ ReceiptType: "" }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {

            var status = "<table id='dataTable3' class='table table-striped table-bordered' style='width: 100%'>";
            status = status + " <thead><tr>";
            status = status + " <th>#</th><th>Receipt Challan</th><th>Type</th><th>Entity Code </th><th>Network Name</th><th>Contact Person</th><th>Delivery To</th><th>Delivery On</th>";
            status = status + " </tr></thead>";
            status = status + ' <tbody>';
            for (var i = 0; i < msg.d.length; i++) {
                status = status + ' <tr>';
                status = status + "<td><div class='form-check'><input class='form-check-input' type='checkbox' onclick='CheckChangesMas(" + msg.d[i].ReceiptChallan_ID + ")' value='' id='chk" + msg.d[i].ReceiptChallan_ID + "' />";
                status = status + "<label class='form-check-label' for='defaultCheck1'></label></div></td>"
                status = status + '<td>' + msg.d[i].DocumentNumber + '</td><td>' + msg.d[i].EntryType + '</td>';
                status = status + '<td>' + msg.d[i].EntityCode + '</td><td>' + msg.d[i].NetworkName + '</td>';
                status = status + '<td>' + msg.d[i].ContactPerson + '</td><td>' + msg.d[i].DeliveredTo + '</td><td>' + msg.d[i].DELIVERY_DATE + '</td>';
                status = status + '</tr>';
            }
            status = status + '</tbody> </table>';
            $('#divMassConfirm').html(status);

        }
    });
}

var MassAssing = [];

function CheckChangesMas(values) {
    if ($('#chk' + values).is(':checked') == true) {
        MassAssing.push(values);
    }
    else {
        //  var itemtoRemove = "HTML";
        MassAssing.splice($.inArray(values, MassAssing), 1);
    }
    console.log(MassAssing);
}

function MassDeliveryConfirm() {
    jConfirm('Confirm Delivery?', 'Alert', function (r) {
        if (r) {
            var Apply = {
                RecptID: MassAssing
            }

            $.ajax({
                type: "POST",
                url: "DeliveryList.aspx/MassDeliveryConfirm",
                data: "{model:" + JSON.stringify(Apply) + "}",
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    console.log(response);
                    if (response.d) {
                        if (response.d == "true") {
                            jAlert("Delivery Successfully.", "Alert", function () {
                                $("#confirmModal").hide();
                                DeliveryCount();
                                TotalEntered();
                            });
                        }
                        else if (response.d == "Logout") {
                            location.href = "../../OMS/SignOff.aspx";
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