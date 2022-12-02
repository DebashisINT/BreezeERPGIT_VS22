
$(function () {
    cBranchPanel.PerformCallback('BindComponentGrid' + '~' + "All");
    cLocationPanel.PerformCallback('BindLocationGrid' + '~' + "All");
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

$(document).ready(function () {


    $('#dataTable').DataTable({
        scrollX: true,
        fixedColumns: {
            rightColumns: 1
        }
    });

    $('#dataTable2').DataTable({
        scrollX: true,
        fixedColumns: {
            rightColumns: 1
        }
    });

    $('.datepicker').datepicker({
        format: 'mm/dd/yyyy'
    });

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

$(document).ready(function () {
    setTimeout(function () {
        $('#example-getting-started').multiselect();
    }, 1000);

    $('#rcn, #sln, #lct').hide();
    $('#idType').change(function () {
        var value = $(this).val();
        if (value == 1) {
            $('#rcn, #sln, #lct, #slnTrack').hide();
            $('#rcn').show();
        } else if (value == 2) {
            $('#rcn, #sln, #lct, #slnTrack').hide();
            $('#sln').show();
        } else if (value == 3) {
            $('#rcn, #sln, #lct, #slnTrack').hide();
            $('#lct').show();
        } else if (value == 4) {
            $('#rcn, #sln, #lct, #slnTrack').hide();
            $('#slnTrack').show();
        }
    });
});

function SearchClick() {
    LoadingPanel.Show();
    var LocationID = "";//gridLocationLookup.gridView.GetSelectedKeysOnPage();
    var Locations = "";
    //for (var i = 0; i < LocationID.length; i++) {
    //    if (Locations == "") {
    //        Locations = LocationID[i];
    //    }
    //    else {
    //        Locations += ',' + LocationID[i];
    //    }
    //}

    var ReceiptchallanNo = $("#txtReceiptchallanNo").val();
    var EntityCode = $("#txtEntityCode").val();
    // var Branch = $("#ddlBranch").val();

    gridLocationLookup.gridView.GetSelectedFieldValues("ID", function (val) {
        LocationID = val

        for (var i = 0; i < LocationID.length; i++) {
            if (Locations == "") {
                Locations = LocationID[i];
            }
            else {
                Locations += ',' + LocationID[i];
            }
        }

        $.ajax({
            type: "POST",
            url: "searchqueries.aspx/SearchList",
            data: JSON.stringify({ ReceiptchallanNo: ReceiptchallanNo, EntityCode: EntityCode, Branch: Locations }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var status = "";
                status = status + "<table id='dataTable' class='table table-striped table-bordered display nowrap' style='width:100%'>";
                status = status + " <thead><tr>";
                status = status + " <th>Receipt Challan</th><th>Type</th><th>Entity Code </th><th>Network Name</th><th>Contact Person</th><th>Technician </th>";
                status = status + " <th>Location </th><th>Received By</th><th>Received On</th><th>Assigned By</th><th>Assigned On</th><th>Serv. Entered By</th>";
                status = status + " <th>Serv. Entered On</th><th>Warranty</th><th>Confirm Delivered</th><th>Confirm Delivered Date</th><th>Delivered By</th>";
                status = status + " <th>Delivered On</th><th>Status</th><th>Action</th>";
                status = status + " </tr></thead>";
                status = status + " </table>";

                $('#DivDeliveryDetails').html(status);

                $('#dataTable').DataTable({
                    scrollX: true,
                    fixedColumns: {
                        rightColumns: 1
                    },
                    data: msg.d,
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
                       { 'data': 'Warranty' },
                       { 'data': 'ConfirmDelivery' },
                       { 'data': 'ConfirmDeliveryDate' },
                       { 'data': 'DeliveryBy' },
                       { 'data': 'DeliveryOn' },
                       { 'data': 'Status' },
                       { 'data': 'Action' },
                    ],
                    dom: 'Bfrtip',
                    buttons: [
                        {
                            extend: 'excel',
                            title: null,
                            filename: 'Search Details',
                            text: 'Save as Excel',
                            customize: function (xlsx) {
                                var sheet = xlsx.xl.worksheets['sheet1.xml'];
                                $('row:first c', sheet).attr('s', '42');
                            },

                            exportOptions: {
                                //columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12,13,14,15,16,17,18,19,20,21,22]
                            }
                        }
                    ],
                    error: function (error) {
                        alert(error);
                        LoadingPanel.Hide();
                    }
                });
                LoadingPanel.Hide();
            }
        });
    });
}

function SearchSingleClick() {
    LoadingPanel.Show();
    $("#spnfilter").removeClass('hide');
    $("#drdExport").removeClass('hide');
    var srcType = $("#idType").val();

    if (srcType == "1") {
        if ($("#ReceiptchallanNo").val() != "") {
            ReceiptChallan();
        }
        else {
            LoadingPanel.Hide();
        }
    } else if (srcType == "2") {
        if ($("#txtSerialNo").val() != "") {
            SerialNo();
        }
        else {
            LoadingPanel.Hide();
        }
    } else if (srcType == "3") {
        Locations();
    } else if (srcType == "4") {
        $("#spnfilter").addClass('hide');
        $("#drdExport").addClass('hide');
        if ($("#txtSerialTrack").val() != "") {
            GetDeviceDetails();
        }
        else {
            LoadingPanel.Hide();
        }
    } else {
        jAlert('Please select Type.');
        LoadingPanel.Hide();
    }
}

function ReceiptChallan() {
    var ReceiptchallanNo = $("#ReceiptchallanNo").val();
    $.ajax({
        type: "POST",
        url: "searchqueries.aspx/ReceiptChallanSearchList",
        data: JSON.stringify({ ReceiptchallanNo: ReceiptchallanNo }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var status = "";
            status = status + "<table id='dataTable' class='table table-striped table-bordered display nowrap' style='width:100%'>";
            status = status + " <thead><tr>";
            status = status + " <th>Receipt Challan</th><th>Challan Date</th><th>Type</th><th>Entity Code </th><th>Network Name</th><th>Contact Person</th><th>Technician </th>";
            status = status + " <th>Location </th><th>Received By</th><th>Received On</th><th>Assigned By</th><th>Assigned On</th><th>Serv. Entered By</th>";
            status = status + " <th>Serv. Entered On</th><th>Warranty</th><th>Confirm Delivered</th><th>Confirm Delivered Date</th><th>Delivered By</th>";
            status = status + " <th>Delivered On</th><th>Status</th><th>Action</th>";
            status = status + " </tr></thead>";
            status = status + " </table>";

            $('#DivDeliveryDetails').html(status);

            $('#dataTable').DataTable({
                scrollX: true,
                fixedColumns: {
                    rightColumns: 1
                },
                data: msg.d,
                columns: [
                   { 'data': 'ReceiptChallan' },
                   { 'data': 'DocumentDate' },
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
                   { 'data': 'Warranty' },
                   { 'data': 'ConfirmDelivery' },
                   { 'data': 'ConfirmDeliveryDate' },
                   { 'data': 'DeliveryBy' },
                   { 'data': 'DeliveryOn' },
                   { 'data': 'Status' },
                   { 'data': 'Action' },
                ],
                dom: 'Bfrtip',
                buttons: [
                    {
                        extend: 'excel',
                        title: null,
                        filename: 'Search Details',
                        text: 'Save as Excel',
                        customize: function (xlsx) {
                            var sheet = xlsx.xl.worksheets['sheet1.xml'];
                            $('row:first c', sheet).attr('s', '42');
                        },

                        exportOptions: {
                            //columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12,13,14,15,16,17,18,19,20,21,22]
                        }
                    }
                ],
                error: function (error) {
                    alert(error);
                    LoadingPanel.Hide();
                }
            });
            LoadingPanel.Hide();
        }
    });
}

function Locations() {
    LoadingPanel.Show();
    if (gridbranchLookup.GetValue() == null) {
        jAlert('Please select atleast one Location.');
        LoadingPanel.Hide();
        return
    }


    var brnch = "";//gridbranchLookup.gridView.GetSelectedKeysOnPage();
    var branchss = "";
    //for (var i = 0; i < brnch.length; i++) {
    //    branchss += ',' + brnch[i];
    //}

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

        $.ajax({
            type: "POST",
            url: "searchqueries.aspx/SearchList",
            data: JSON.stringify({ ReceiptchallanNo: "", EntityCode: "", Branch: branchss }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var status = "";
                status = status + "<table id='dataTable' class='table table-striped table-bordered display nowrap' style='width:100%'>";
                status = status + " <thead><tr>";
                status = status + " <th>Receipt Challan</th><th>Challan Date</th><th>Type</th><th>Entity Code </th><th>Network Name</th><th>Contact Person</th><th>Technician </th>";
                status = status + " <th>Location </th><th>Received By</th><th>Received On</th><th>Assigned By</th><th>Assigned On</th><th>Serv. Entered By</th>";
                status = status + " <th>Serv. Entered On</th><th>Warranty</th><th>Confirm Delivered</th><th>Confirm Delivered Date</th><th>Delivered By</th>";
                status = status + " <th>Delivered On</th><th>Status</th><th>Action</th>";
                status = status + " </tr></thead>";
                status = status + " </table>";

                $('#DivDeliveryDetails').html(status);

                $('#dataTable').DataTable({
                    scrollX: true,
                    fixedColumns: {
                        rightColumns: 1
                    },
                    data: msg.d,
                    columns: [
                       { 'data': 'ReceiptChallan' },
                       { 'data': 'DocumentDate' },
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
                       { 'data': 'Warranty' },
                       { 'data': 'ConfirmDelivery' },
                       { 'data': 'ConfirmDeliveryDate' },
                       { 'data': 'DeliveryBy' },
                       { 'data': 'DeliveryOn' },
                       { 'data': 'Status' },
                       { 'data': 'Action' },
                    ],
                    dom: 'Bfrtip',
                    buttons: [
                        {
                            extend: 'excel',
                            title: null,
                            filename: 'Search Details',
                            text: 'Save as Excel',
                            customize: function (xlsx) {
                                var sheet = xlsx.xl.worksheets['sheet1.xml'];
                                $('row:first c', sheet).attr('s', '42');
                            },

                            exportOptions: {
                                //columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12,13,14,15,16,17,18,19,20,21,22]
                            }
                        }
                    ],
                    error: function (error) {
                        alert(error);
                        LoadingPanel.Hide();
                    }
                });
                LoadingPanel.Hide();
            }
        });
    });
}

function SerialNo() {
    LoadingPanel.Show();
    var SerialNo = $("#txtSerialNo").val();
    $.ajax({
        type: "POST",
        url: "searchqueries.aspx/SerialNoSearchList",
        data: JSON.stringify({ SerialNo: SerialNo }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            // Mantis Issue 24265 [ new header branch added -  <th>Location </th>  ]
            var status = "";
            status = status + "<table id='dataTable' class='table table-striped table-bordered display nowrap' style='width:100%'>";
            status = status + " <thead><tr>";
            status = status + " <th>Challan No</th><th>Challan Date</th><th>Searial No</th><th>Model </th><th>Entity Code </th><th>Network Name</th><th>Technician </th><th>Location </th><th>Problem Reported</th><th>Service Action</th><th>Component </th>";
            status = status + " <th>Warranty </th><th>Stock Entry</th><th>New Serial No</th><th>New Warranty</th><th>New Model</th><th>Return Reason</th><th>Billable</th><th>Unbillable Reason</th>";
            status = status + " <th>Problem Found</th><th>Remarks</th><th>Warranty Status</th><<th>Confirm Delivered</th><th>Confirm Delivered Date</th><th>Action</th>";
            status = status + " </tr></thead>";
            status = status + " </table>";

            $('#DivDeliveryDetails').html(status);

            $('#dataTable').DataTable({
                scrollX: true,
                fixedColumns: {
                    rightColumns: 1
                },
                data: msg.d,
                columns: [
                   { 'data': 'ChallanNo' },
                   { 'data': 'DocumentDate' },
                   { 'data': 'SearialNo' },
                   { 'data': 'Model' },
                   { 'data': 'EntityCode' },
                   { 'data': 'NetworkName' },
                   { 'data': 'Technician' },
                   // Mantis Issue 24265
                   { 'data': 'Location' },
                   // End of Mantis Issue 24265
                   { 'data': 'ProblemReported' },
                   { 'data': 'ServiceAction' },
                   { 'data': 'Component' },
                   { 'data': 'Warranty' },
                   { 'data': 'StockEntry' },
                   { 'data': 'NewSerialNo' },
                   { 'data': 'NewWarranty' },
                   { 'data': 'ItemModel' },
                   { 'data': 'ReturnReason' },
                   { 'data': 'Billable' },
                   { 'data': 'UnbillableReason' },
                   { 'data': 'ProblemFound' },
                   { 'data': 'Remarks' },
                   { 'data': 'WarrantyStatus' },                   
                   { 'data': 'ConfirmDelivery' },
                   { 'data': 'ConfirmDeliveryDate' },
                   { 'data': 'Action' }
                ],
                dom: 'Bfrtip',
                buttons: [
                    {
                        extend: 'excel',
                        title: null,
                        filename: 'Search Details',
                        text: 'Save as Excel',
                        customize: function (xlsx) {
                            var sheet = xlsx.xl.worksheets['sheet1.xml'];
                            $('row:first c', sheet).attr('s', '42');
                        },

                        exportOptions: {
                            //columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12,13,14,15,16,17,18,19,20,21,22]
                        }
                    }
                ],
                error: function (error) {
                    alert(error);
                    LoadingPanel.Hide();
                }
            });
            LoadingPanel.Hide();
        }
    });
}

function selectAllLocation() {
    gridLocationLookup.gridView.SelectRows();
}

function unselectAllLocation() {
    gridLocationLookup.gridView.UnselectRows();
}

function CloseLoactionLookup() {
    gridLocationLookup.ConfirmCurrentSelection();
    gridLocationLookup.HideDropDown();
    gridLocationLookup.Focus();
}

function ViewDetails(values, module) {
    if (module == "P") {
        ReceiptchallanView(values, module);
    }
    else if (module == "DU") {
        AssignJobView(values, module);
    }
    else if (module == "DN") {
        ServiceEntryView(values, module);
    }
    else if (module == "DE") {
        DeliveryView(values, module);
    }
    else if (module == "Jobsheet") {
        JobsheetView(values, module);
    }
}

//Mantis Issue 24360
function SmsRequest(values, module) {
    //Mantis Issue 0024781
    //jConfirm('Send SMS?', 'Alert', function (r) {
    //    if (r) {
    //        $.ajax({
    //            type: "POST",
    //            url: "searchqueries.aspx/SendSMS",
    //            data: JSON.stringify({ ReceiptID: values, module: module }),
    //            async: false,
    //            contentType: "application/json; charset=utf-8",
    //            dataType: "json",
    //            success: function (response) {
    //                //console.log(response);
    //                if (response.d) {
    //                    if (response.d == "true") {
    //                        jAlert("SMS sent Successfully.", "Alert", function () {

    //                        });
    //                    }
    //                    else {
    //                        jAlert(response.d);
    //                        return
    //                    }
    //                }
    //            },
    //            error: function (response) {
    //                console.log(response);
    //            }
    //        });
    //    }
    //});
    $("#hdnReceiptChallan_ID").val(values);
    $("#hdnModule_ID").val(module);
    $("#assignPhnNo").modal('show');
    //End of Mantis Issue 0024781
}
//End of mantis Issue 24360
//Mantis Issue 24781
function onlyNumbers(e) {
    var unicode = e.charCode ? e.charCode : e.keyCode;
    if (unicode != 8) {
        if (unicode < 9 || unicode > 9 && unicode < 46 || unicode > 57 || unicode == 47) {
            if (unicode == 37 || unicode == 38) {
                return true;
            }
            else {
                return false;
            }
        }
        else {
            return true;
        }
    }
    else {
        return true;
    }
}
function PhoneNoSend() {
    if ($("#txtPhoneNo").val().trim() != "") {

        var mob = /^\d{10}$/;
        if ($('#txtPhoneNo').val().length != '10') {
            alert('Please Enter 10 Digit Mobile Number');
            $('#txtPhoneNo').focus();
            return false;
        }
        else {
            if (!mob.test($('#txtPhoneNo').val())) {
                alert('Invalid mobile number');
                $('#txtPhoneNo').focus();
                return false;
            }
        }
    }
    $("#assignPhnNo").modal('hide');
    var PhoneNo = $("#txtPhoneNo").val();
    jConfirm('Send SMS?', 'Alert', function (r) {
        if (r) {
            $.ajax({
                type: "POST",
                url: "searchqueries.aspx/SendSMSManualNo",
                data: JSON.stringify({ ReceiptID: $("#hdnReceiptChallan_ID").val(), module: $("#hdnModule_ID").val(), MobileNo: PhoneNo }),
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
//End of Mantis Issue 24781

function ReceiptchallanView(values, module) {
    setTimeout(function () {
        $.ajax({
            type: "POST",
            url: "searchqueries.aspx/ReceptDetails",
            data: JSON.stringify({ ReceiptID: values, module: module }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var list = msg.d;
                $("#txtDocumentNumber").val(msg.d.DocumentNumber);
                $("#txtLocation").val(msg.d.Branch);
                $("#txtDate").val(msg.d.DocumentDate);
                $("#EntityCode").val(msg.d.EntityCode);
                $("#NetworkName").val(msg.d.NetworkName);
                $("#ContactPerson").val(msg.d.ContactPerson);
                $("#ContactNumber").val(msg.d.ContactNo);
                $("#txtTechnician").val("");

                var status = " <table id='dataTable2' class='table table-striped table-bordered display nowrap' style='width: 100%'>";
                status = status + " <thead><tr>";
                status = status + " <th>Device Type</th><th>Model</th><th>Serial Number</th><th>Warranty</th>";
                status = status + " <th>Problem</th><th>Remarks </th><th>Remote </th><th>Card/Adaptor</th></tr></thead>";

                status = status + " </table>";
                $('#divReceiptChallanDtls').html(status);

                setTimeout(function () {
                    $('#dataTable2').DataTable({
                        scrollX: true,
                        fixedColumns: {
                            rightColumns: 1
                        },
                        data: msg.d.DetailsList,
                        columns: [
                           { 'data': 'DeviceType' },
                           { 'data': 'Model' },
                           { 'data': 'DeviceNumber' },
                           { 'data': 'Warranty' },
                           { 'data': 'Problem' },
                           { 'data': 'Remarks' },
                           { 'data': 'Remotes' },
                           { 'data': 'CardAdaptor' },
                        ],
                    });
                }, 400);
                $("#detailsModal").modal('show');
            }
        });
    }, 1000);
}


function AssignJobView(values, module) {
    setTimeout(function () {
        $.ajax({
            type: "POST",
            url: "searchqueries.aspx/ReceptDetails",
            data: JSON.stringify({ ReceiptID: values, module: module }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var list = msg.d;
                $("#txtDocumentNumber").val(msg.d.DocumentNumber);
                $("#txtLocation").val(msg.d.Branch);
                $("#txtDate").val(msg.d.DocumentDate);
                $("#EntityCode").val(msg.d.EntityCode);
                $("#NetworkName").val(msg.d.NetworkName);
                $("#ContactPerson").val(msg.d.ContactPerson);
                $("#ContactNumber").val(msg.d.ContactNo);
                $("#txtTechnician").val(msg.d.Technician);

                var status = " <table id='dataTable2' class='table table-striped table-bordered display nowrap' style='width: 100%'>";
                status = status + " <thead><tr>";
                status = status + " <th>Device Type</th><th>Model</th><th>Serial Number</th><th>Warranty</th>";
                status = status + " <th>Problem</th><th>Remarks </th><th>Remote </th><th>Card/Adaptor</th></tr></thead>";

                status = status + " </table>";
                $('#divReceiptChallanDtls').html(status);
                $("#detailsModal").modal('show');
                setTimeout(function () {
                    $('#dataTable2').DataTable({
                        scrollX: true,
                        fixedColumns: {
                            rightColumns: 1
                        },
                        data: msg.d.DetailsList,
                        columns: [
                           { 'data': 'DeviceType' },
                           { 'data': 'Model' },
                           { 'data': 'DeviceNumber' },
                           { 'data': 'Warranty' },
                           { 'data': 'Problem' },
                           { 'data': 'Remarks' },
                           { 'data': 'Remotes' },
                           { 'data': 'CardAdaptor' },
                        ],
                    });
                }, 400)
            }
        });
    }, 1000);
}

function ServiceEntryView(values, module) {
    $('#dataTableServiceEntry').DataTable().destroy();
    $("#detailsModalServiceEntry").modal('show');
    setTimeout(function () {

        $.ajax({
            type: "POST",
            url: "searchqueries.aspx/ServiceEntryView",
            data: JSON.stringify({ ReceiptID: values, module: module }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var list = msg.d;
                $("#SrvEntryChallanNo").val(msg.d.ChallanNo);
                $("#SrvEntryEntityCode").val(msg.d.EntityCode);
                $("#SrvEntryNetworkName").val(msg.d.NetworkName);
                $("#SrvEntryContactPerson").val(msg.d.ContactPerson);
                $("#SrvEntryReceivedOn").val(msg.d.ReceivedOn);
                $("#SrvEntryReceivedBy").val(msg.d.ReceivedBy);
                $("#SrvEntryAssignedTo").val(msg.d.AssignedTo);
                $("#SrvEntryAssignedBy").val(msg.d.AssignedBy);
                $("#SrvEntryAssignedOn").val(msg.d.AssignedOn);

                var status = " <table id='dataTableServiceEntry' class='table table-striped table-bordered display nowrap' style='width: 100%'>";
                status = status + " <thead><tr>";
                status = status + " <<th>Model No</th><th>Serial No</th><th>Problem Reported</th><th>Service Action</th><th>Components</th>";
                status = status + " <th>Warranty</th><th>Stock Entry</th><th>New Model</th><th>New Serial No</th><th>New Warranty</th><th>Problem Found</th>";
                status = status + " <th>Remarks </th><th>Warranty Status</th><th>Return Reason</th><th>Billable </th></tr></thead>";

                status = status + " </table>";
                $('#SrvEntryTable').html(status);
                $('#dataTableServiceEntry').DataTable({
                    scrollX: true,
                    retrieve: true,
                    fixedColumns: {
                        rightColumns: 1
                    },
                    data: msg.d.DetailsList,
                    columns: [
                       { 'data': 'ModelNo' },
                       { 'data': 'SerialNo' },
                       { 'data': 'ProblemReported' },
                       { 'data': 'ServiceAction' },
                       { 'data': 'Components' },
                       { 'data': 'Warranty' },
                       { 'data': 'StockEntry' },
                       { 'data': 'NewModel' },
                       { 'data': 'NewSerialNo' },
                       { 'data': 'NewWarranty' },
                       { 'data': 'ProblemFound' },
                       { 'data': 'Remarks' },
                       { 'data': 'WarrantyStatus' },
                       { 'data': 'ReturnReason' },
                       { 'data': 'Billable' },
                    ],
                });

            }
        });
    }, 1000);
}

function DeliveryView(values, module) {
    $('#dataTableDelivery').DataTable().destroy();
    $("#detailsModalDelivery").modal('show');
    setTimeout(function () {

        $.ajax({
            type: "POST",
            url: "searchqueries.aspx/DeliveryView",
            data: JSON.stringify({ ReceiptID: values, module: module }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {

                var list = msg.d;
                $("#DelChallanNo").val(msg.d.ChallanNo);
                $("#DelEntityCode").val(msg.d.EntityCode);
                $("#DelNetworkName").val(msg.d.NetworkName);
                $("#DelContactPerson").val(msg.d.ContactPerson);
                $("#DelReceivedOn").val(msg.d.ReceivedOn);
                $("#DelReceivedBy").val(msg.d.ReceivedBy);
                $("#DelAssignedTo").val(msg.d.AssignedTo);
                $("#DelAssignedBy").val(msg.d.AssignedBy);
                $("#DelAssignedOn").val(msg.d.AssignedOn);
                $("#DelDeliveredTo").val(msg.d.DeliveredTo);
                $("#DelPhoneNo").val(msg.d.PhoneNo);
                $("#DelRemarks").val(msg.d.Remarks);

                var status = " <table id='dataTableDelivery' class='table table-striped table-bordered display nowrap' style='width: 100%'>";
                status = status + " <thead><tr>";
                status = status + " <th>Device Type</th><th>Model No</th><th>Serial No</th><th>Problem found</th><th>Service Action</th>";
                status = status + "  <th>Warranty</th><th>AC Cord / Adapter	</th><th>Remote </th></tr></thead>";

                status = status + " </table>";
                $('#DeliveryTable').html(status);


                $('#dataTableDelivery').DataTable({
                    // scrollX: true,

                    retrieve: true,
                    fixedColumns: {
                        rightColumns: 1
                    },
                    data: msg.d.DetailsList,
                    columns: [
                        { 'data': 'DeviceType' },
                        { 'data': 'Model' },
                        { 'data': 'DeviceNumber' },
                        { 'data': 'Problemfound' },
                        { 'data': 'ServiceAction' },
                        { 'data': 'Warranty' },
                        { 'data': 'CardAdaptor' },
                        { 'data': 'Remotes' },
                    ],
                });



            }
        });
    }, 1000);
}

function JobsheetView(values, module) {
    $('#dataTablejobsheetView').DataTable().destroy();
    $("#detailsModalJobsheet").modal('show');
    setTimeout(function () {

        $.ajax({
            type: "POST",
            url: "searchqueries.aspx/JobSheetView",
            data: JSON.stringify({ ReceiptID: values, module: module }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var list = msg.d;
                $("#jobChallanNumber").val(msg.d.ChallanNumber);
                $("#jobPostingDate").val(msg.d.PostingDate);
                $("#jobRefJobsheet").val(msg.d.RefJobsheet);
                $("#jobAssignTo").val(msg.d.AssignTo);
                $("#jobWorkDoneOn").val(msg.d.WorkDoneOn);
                $("#jobLocation").val(msg.d.Location);
                $("#jobRemarks").val(msg.d.Remarks);
                $("#jobEntityCode").val(msg.d.EntityCode);
                $("#jobNetworkName").val(msg.d.NetworkName);
                $("#jobContactPerson").val(msg.d.ContactPerson);
                $("#jobContactNumber").val(msg.d.ContactNumber);

                var status = " <table id='dataTablejobsheetView' class='table table-striped table-bordered display nowrap' style='width: 100%'>";
                status = status + " <thead><tr>";
                status = status + " <th>Serial Number</th><th>Device Type</th><th>Model</th><th>Problem found</th><th>Other</th><th>Service Action</th>";
                status = status + " <th>Components</th><th>Warranty</th><th>Return Reason</th><th>Remarks </th><th>Billable </th></tr></thead>";

                status = status + " </table>";
                $('#JobSheetTable').html(status);

                setTimeout(function () {
                    $('#dataTablejobsheetView').DataTable({
                        scrollX: true,
                        retrieve: true,
                        fixedColumns: {
                            rightColumns: 1
                        },
                        data: msg.d.DetailsList,
                        columns: [
                           { 'data': 'SerialNumber' },
                           { 'data': 'DeviceType' },
                           { 'data': 'Model' },
                           { 'data': 'Problemfound' },
                           { 'data': 'Other' },
                           { 'data': 'ServiceAction' },
                           { 'data': 'Components' },
                           { 'data': 'Warranty' },
                           { 'data': 'ReturnReason' },
                           { 'data': 'Remarks' },
                           { 'data': 'Billable' },
                        ],
                    });
                }, 400)

            }
        });
    }, 1000);
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

$(document).ready(function () {
    $('#detailsModalJobsheet').on('shown.bs.modal', function (e) {
        // do something...
        $('#dataTablejobsheetView').DataTable({
            scrollX: true
        });
    })
    $('#detailsModalServiceEntry').on('shown.bs.modal', function (e) {
        // do something...
        $('#dataTableServiceEntry').DataTable({
            scrollX: true
        });
    });
    $('#detailsModalDelivery').on('shown.bs.modal', function (e) {
        // do something...
        $('#dataTableDelivery').DataTable({
            scrollX: true
        });
    })
})


function UndoDelivery(values) {
    jConfirm('Undo Delivery?', 'Alert', function (r) {
        if (r) {
            $.ajax({
                type: "POST",
                url: "searchqueries.aspx/UndoDelivery",
                data: JSON.stringify({ ReceiptChallanID: values }),
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    //console.log(response);
                    if (response.d) {
                        if (response.d == "true") {
                            jAlert("Undo Delivery Successfully.", "Alert", function () {

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



function GetDeviceDetails() {
    //var gridRow = cGrdDevice.GetVisibleRowsOnPage();
    $.ajax({
        type: "POST",
        url: '/SRVFileuploadDelivery/DeviceNumberDetails',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ DeviceNumber: $("#txtSerialTrack").val() }),
        success: function (response) {
            var EntityCode = "";
            var NetworkName = "";
            var Model = "";
            if (response.EntityCode != null) {
                EntityCode = response.EntityCode;
            }
            if (response.NetworkName != null) {
                NetworkName = response.NetworkName;
            }

            if (response.Model != null) {
                Model = response.Model;
            }

            var status = "";
            status = status + "<table id='dataTable' class='table table-striped table-bordered display nowrap' style='width:50%'>";
            status = status + " <thead><tr>";
            status = status + " <th>Entity Code </th><th>Network Name</th><th>Model</th>";

            status = status + " </tr></thead>";
            status = status + " <tbody><tr><th>" + EntityCode + "</th><th>" + NetworkName + "</th><th>" + Model + "</th></tr></tbody>";
            status = status + " </table>";

            $('#DivDeliveryDetails').html(status);
            LoadingPanel.Hide();
        },
        error: function (response) {
            jAlert(response);
            LoadingPanel.Hide();
        }
    });
}


function OnclickViewAttachment(obj) {
    var URL = '../../../OMS/Management/Activities/EntriesDocuments.aspx?idbldng=' + obj + '&type=SrvSearch';
    window.location.href = URL;
}