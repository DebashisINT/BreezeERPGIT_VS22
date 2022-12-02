
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

    $('#rcn, #sln, #lct,#slDate,#rsrl').hide();
    $('#idTypes').change(function () {
        var value = $(this).val();
        if (value == 1) {
            $('#rcn, #sln, #lct, #slnTrack,#slDate,#rsrl').hide();
            $('#rcn').show();
        } else if (value == 2) {
            $('#rcn, #sln, #lct, #slnTrack,#slDate,#rsrl').hide();
            $('#sln').show();
        } else if (value == 3) {
            $('#rcn, #sln, #lct, #slnTrack,#slDate,#rsrl').hide();
            $('#lct').show();
        } else if (value == 4) {
            $('#rcn, #sln, #lct, #slnTrack,#slDate,#rsrl').hide();
            $('#slDate').show();
        } else if (value == 5) {
            $('#rcn, #sln, #lct, #slnTrack,#slDate,#rsrl').hide();
            $('#rsrl').show();
        }
    });
});

function SearchClick() {
    WorkingRoster();
    if (rosterstatus) {
        var srcType = $("#idType").val();
        srcType = "1";
        var suc = true;
        if (srcType == "0") {
            jAlert("Please select Type.")
            $("#idType").focus();
            suc = false;
            return
        }
        LoadingPanel.Show();
        var LocationID = "";//gridLocationLookup.gridView.GetSelectedKeysOnPage();
        var Locations = "";
        var DocumentNo = $("#txtReceiptchallanNo").val();
        var EntityCode = $("#txtEntityCode").val();


        if (srcType == "1") {

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
                    url: "STBSchemeSearch.aspx/SearchList",
                    data: JSON.stringify({ DocumentNo: DocumentNo, EntityCode: EntityCode, Branch: Locations }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var status = "";
                        status = status + "<table id='dataTable' class='table table-striped table-bordered display nowrap' style='width:100%'>";
                        status = status + " <thead><tr>";
                        status = status + " <th>Document No.</th><th>Document Date</th><th>Module</th><th>Type</th><th>Entity Code </th><th>Network Name</th><th>Contact Person</th><th>Contact No </th>";
                        status = status + " <th>Location </th><th>Entered By</th><th>Entered On</th><th>Status</th><th>Cancel Reason</th><th>Action</th>";
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
                               { 'data': 'DocumentNumber' },
                               { 'data': 'DocumentDate' },
                               { 'data': 'Location' },
                               { 'data': 'EntityCode' },
                               { 'data': 'NetworkName' },
                               { 'data': 'ContactPerson' },
                               { 'data': 'ContactNo' },
                               { 'data': 'DAS' },
                               { 'data': 'Enterby' },
                               { 'data': 'EnterOn' },
                               { 'data': 'Status' },
                               { 'data': 'ModuleType' },
                               { 'data': 'RecvModel' },
                               { 'data': 'SerialNumber' },
                               { 'data': 'Rate' },
                               { 'data': 'Remarks' },
                               { 'data': 'Remote' },
                               { 'data': 'CordAdaptor' },
                               { 'data': 'ReqFor' },
                               { 'data': 'ReqType' },
                               { 'data': 'ReqModel' },
                               { 'data': 'UnitPrice' },
                               { 'data': 'Quantity' },
                               { 'data': 'Amount' },
                               { 'data': 'Approve_By' },
                               { 'data': 'Approve_On' },
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
        else if (srcType == "2") {
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
                    url: "STBSchemeSearch.aspx/WalletSearchList",
                    data: JSON.stringify({ DocumentNo: DocumentNo, EntityCode: EntityCode, Branch: Locations }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var status = "";
                        status = status + "<table id='dataTable' class='table table-striped table-bordered display nowrap' style='width:100%'>";
                        status = status + " <thead><tr>";
                        status = status + " <th>Document No.</th><th>Document Date</th><th>Module</th><th>Type</th><th>Entity Code </th><th>Network Name</th><th>Contact Person</th><th>Contact No </th>";
                        status = status + " <th>Location </th><th>DAS</th><th>Entered By</th><th>Entered On</th><th>Status</th><th>Cancel Reason</th><th>Action</th>";
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
                               { 'data': 'DocumentNumber' },
                               { 'data': 'DocumentDate' },
                               { 'data': 'Location' },
                               { 'data': 'EntityCode' },
                               { 'data': 'NetworkName' },
                               { 'data': 'ContactPerson' },
                               { 'data': 'ContactNo' },
                               { 'data': 'DAS' },
                               { 'data': 'Enterby' },
                               { 'data': 'EnterOn' },
                               { 'data': 'Status' },
                               { 'data': 'ModuleType' },
                               { 'data': 'RecvModel' },
                               { 'data': 'SerialNumber' },
                               { 'data': 'Rate' },
                               { 'data': 'Remarks' },
                               { 'data': 'Remote' },
                               { 'data': 'CordAdaptor' },
                               { 'data': 'ReqFor' },
                               { 'data': 'ReqType' },
                               { 'data': 'ReqModel' },
                               { 'data': 'UnitPrice' },
                               { 'data': 'Quantity' },
                               { 'data': 'Amount' },
                               { 'data': 'Approve_By' },
                               { 'data': 'Approve_On' },
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
    }
    else {
        $("#divPopHead").removeClass('hide');
    }
}

function SearchSingleClick() {
    //WorkingRoster();
    //if (rosterstatus) {
        var srcType = $("#idTypes").val();
        var suc = true;
        if (srcType == "0") {
            jAlert("Please select Type.")
            $("#idType").focus();
            suc = false;
            return
        }



        LoadingPanel.Show();
        $("#spnfilter").removeClass('hide');
        $("#drdExport").removeClass('hide');


        if (srcType == "1") {
            if ($("#DocumentsNo").val() != "") {
                DocumentNoSearch();
            }
            else {
                LoadingPanel.Hide();
            }
        }
        else if (srcType == "2") {
            if ($("#txtEntityNo").val() != "") {
                EntityCodeSearch();
            }
            else {
                LoadingPanel.Hide();
            }
        }
        else if (srcType == "3") {
            BranchSearch();
        }
        else if (srcType == "4") {
            DateSearch();
        }
        else if (srcType == "5") {
            SerialSearch();
        }
    //}
    //else {
    //    $("#divPopHead").removeClass('hide');
    //}
}


function SerialSearch() {
    var SerialNo = $("#txtSerialNo").val();
    $.ajax({
        type: "POST",
        url: "STBSchemeSearch.aspx/SchemeSearchList",
        data: JSON.stringify({ DocumentNo: "", EntityCode: "", Branch: "0", Serial: SerialNo }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var status = "";
            status = status + "<table id='dataTable' class='table table-striped table-bordered display nowrap' style='width:100%'>";
            status = status + " <thead><tr>";
            status = status + " <th>Document No.</th><th>Document Date</th><th>Location</th><th>Entity Code</th><th>Network Name</th><th>Contact Person</th><th>Contact No</th> ";
            status = status + " <th>DAS</th><th>Enter by</th><th>Enter On</th><th>Status</th><th>Module Type</th>";
            status = status + " <th>Model</th><th>SerialNumber</th><th>Rate</th><th>Remarks</th><th>Remote </th> ";
            status = status + " <th>Cord/Adaptor</th><th>Req. For</th><th>Req. Type </th><th>Req. Model</th>";
            status = status + " <th>Unit Price </th><th>Quantity</th><th>Amount </th><th>Approve By</th><th>Approve On </th><th>Action</th>";

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
                        { 'data': 'DocumentNumber' },
                        { 'data': 'DocumentDate' },
                        { 'data': 'Location' },
                        { 'data': 'EntityCode' },
                        { 'data': 'NetworkName' },
                        { 'data': 'ContactPerson' },
                        { 'data': 'ContactNo' },
                        { 'data': 'DAS' },
                        { 'data': 'Enterby' },
                        { 'data': 'EnterOn' },
                        { 'data': 'Status' },
                        { 'data': 'ModuleType' },
                        { 'data': 'RecvModel' },
                        { 'data': 'SerialNumber' },
                        { 'data': 'Rate' },
                        { 'data': 'Remarks' },
                        { 'data': 'Remote' },
                        { 'data': 'CordAdaptor' },
                        { 'data': 'ReqFor' },
                        { 'data': 'ReqType' },
                        { 'data': 'ReqModel' },
                        { 'data': 'UnitPrice' },
                        { 'data': 'Quantity' },
                        { 'data': 'Amount' },
                        { 'data': 'Approve_By' },
                        { 'data': 'Approve_On' },
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

function DocumentNoSearch() {
    var DocumentNo = $("#DocumentsNo").val();
    $.ajax({
        type: "POST",
        url: "STBSchemeSearch.aspx/SchemeSearchList",
        data: JSON.stringify({ DocumentNo: DocumentNo, EntityCode: "", Branch: "0", Serial:"" }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var status = "";
            status = status + "<table id='dataTable' class='table table-striped table-bordered display nowrap' style='width:100%'>";
            status = status + " <thead><tr>";
            status = status + " <th>Document No.</th><th>Document Date</th><th>Location</th><th>Entity Code</th><th>Network Name</th><th>Contact Person</th><th>Contact No</th> ";
            status = status + " <th>DAS</th><th>Enter by</th><th>Enter On</th><th>Status</th><th>Module Type</th>";
            status = status + " <th>Model</th><th>SerialNumber</th><th>Rate</th><th>Remarks</th><th>Remote </th> ";
            status = status + " <th>Cord/Adaptor</th><th>Req. For</th><th>Req. Type </th><th>Req. Model</th>";
            status = status + " <th>Unit Price </th><th>Quantity</th><th>Amount </th><th>Approve By</th><th>Approve On </th><th>Action</th>";
           
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
                        { 'data': 'DocumentNumber' },
                        { 'data': 'DocumentDate' },
                        { 'data': 'Location' },
                        { 'data': 'EntityCode' },
                        { 'data': 'NetworkName' },
                        { 'data': 'ContactPerson' },
                        { 'data': 'ContactNo' },
                        { 'data': 'DAS' },
                        { 'data': 'Enterby' },
                        { 'data': 'EnterOn' },
                        { 'data': 'Status' },
                        { 'data': 'ModuleType' },
                        { 'data': 'RecvModel' },
                        { 'data': 'SerialNumber' },
                        { 'data': 'Rate' },
                        { 'data': 'Remarks' },
                        { 'data': 'Remote' },
                        { 'data': 'CordAdaptor' },
                        { 'data': 'ReqFor' },
                        { 'data': 'ReqType' },
                        { 'data': 'ReqModel' },
                        { 'data': 'UnitPrice' },
                        { 'data': 'Quantity' },
                        { 'data': 'Amount' },
                        { 'data': 'Approve_By' },
                        { 'data': 'Approve_On' },
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

function EntityCodeSearch() {
    var EntityCode = $("#txtEntityNo").val();
    $.ajax({
        type: "POST",
        url: "STBSchemeSearch.aspx/SchemeSearchList",
        data: JSON.stringify({ DocumentNo: "", EntityCode: EntityCode, Branch: "0", Serial: "" }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var status = "";
            status = status + "<table id='dataTable' class='table table-striped table-bordered display nowrap' style='width:100%'>";
            status = status + " <thead><tr>";
            status = status + " <th>Document No.</th><th>Document Date</th><th>Location</th><th>Entity Code</th><th>Network Name</th><th>Contact Person</th><th>Contact No</th> ";
            status = status + " <th>DAS</th><th>Enter by</th><th>Enter On</th><th>Status</th><th>Module Type</th>";
            status = status + " <th>Model</th><th>SerialNumber</th><th>Rate</th><th>Remarks</th><th>Remote </th> ";
            status = status + " <th>Cord/Adaptor</th><th>Req. For</th><th>Req. Type </th><th>Req. Model</th>";
            status = status + " <th>Unit Price </th><th>Quantity</th><th>Amount </th><th>Approve By</th><th>Approve On </th><th>Action</th>";
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
                        { 'data': 'DocumentNumber' },
                        { 'data': 'DocumentDate' },
                        { 'data': 'Location' },
                        { 'data': 'EntityCode' },
                        { 'data': 'NetworkName' },
                        { 'data': 'ContactPerson' },
                        { 'data': 'ContactNo' },
                        { 'data': 'DAS' },
                        { 'data': 'Enterby' },
                        { 'data': 'EnterOn' },
                        { 'data': 'Status' },
                        { 'data': 'ModuleType' },
                        { 'data': 'RecvModel' },
                        { 'data': 'SerialNumber' },
                        { 'data': 'Rate' },
                        { 'data': 'Remarks' },
                        { 'data': 'Remote' },
                        { 'data': 'CordAdaptor' },
                        { 'data': 'ReqFor' },
                        { 'data': 'ReqType' },
                        { 'data': 'ReqModel' },
                        { 'data': 'UnitPrice' },
                        { 'data': 'Quantity' },
                        { 'data': 'Amount' },
                        { 'data': 'Approve_By' },
                        { 'data': 'Approve_On' },
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

function BranchSearch() {

    LoadingPanel.Show();
    if (gridbranchLookup.GetValue() == null) {
        jAlert('Please select atleast one Location.');
        LoadingPanel.Hide();
        return
    }

    var brnch = "";
    var branchss = "";

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
            url: "STBSchemeSearch.aspx/SchemeSearchList",
            data: JSON.stringify({ DocumentNo: "", EntityCode: "", Branch: branchss, Serial: "" }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var status = "";
                status = status + "<table id='dataTable' class='table table-striped table-bordered display nowrap' style='width:100%'>";
                status = status + " <thead><tr>";
                status = status + " <th>Document No.</th><th>Document Date</th><th>Location</th><th>Entity Code</th><th>Network Name</th><th>Contact Person</th><th>Contact No</th> ";
                status = status + " <th>DAS</th><th>Enter by</th><th>Enter On</th><th>Status</th><th>Module Type</th>";
                status = status + " <th>Model</th><th>SerialNumber</th><th>Rate</th><th>Remarks</th><th>Remote </th> ";
                status = status + " <th>Cord/Adaptor</th><th>Req. For</th><th>Req. Type </th><th>Req. Model</th>";
                status = status + " <th>Unit Price </th><th>Quantity</th><th>Amount </th><th>Approve By</th><th>Approve On </th><th>Action</th>";
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
                            { 'data': 'DocumentNumber' },
                            { 'data': 'DocumentDate' },
                            { 'data': 'Location' },
                            { 'data': 'EntityCode' },
                            { 'data': 'NetworkName' },
                            { 'data': 'ContactPerson' },
                            { 'data': 'ContactNo' },
                            { 'data': 'DAS' },
                            { 'data': 'Enterby' },
                            { 'data': 'EnterOn' },
                            { 'data': 'Status' },
                            { 'data': 'ModuleType' },
                            { 'data': 'RecvModel' },
                            { 'data': 'SerialNumber' },
                            { 'data': 'Rate' },
                            { 'data': 'Remarks' },
                            { 'data': 'Remote' },
                            { 'data': 'CordAdaptor' },
                            { 'data': 'ReqFor' },
                            { 'data': 'ReqType' },
                            { 'data': 'ReqModel' },
                            { 'data': 'UnitPrice' },
                            { 'data': 'Quantity' },
                            { 'data': 'Amount' },
                            { 'data': 'Approve_By' },
                            { 'data': 'Approve_On' },
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

function DateSearch() {
    var hfFromDate = cFormDate.GetDate().format('yyyy-MM-dd');
    var hfToDate = ctoDate.GetDate().format('yyyy-MM-dd');
    $.ajax({
        type: "POST",
        url: "STBSchemeSearch.aspx/DateWiseSearchList",
        data: JSON.stringify({ hfFromDate: hfFromDate, hfToDate: hfToDate }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var status = "";
            status = status + "<table id='dataTable' class='table table-striped table-bordered display nowrap' style='width:100%'>";
            status = status + " <thead><tr>";
            status = status + " <th>Document No.</th><th>Document Date</th><th>Location</th><th>Entity Code</th><th>Network Name</th><th>Contact Person</th><th>Contact No</th> ";
            status = status + " <th>DAS</th><th>Enter by</th><th>Enter On</th><th>Status</th><th>Module Type</th>";
            status = status + " <th>Model</th><th>SerialNumber</th><th>Rate</th><th>Remarks</th><th>Remote </th> ";
            status = status + " <th>Cord/Adaptor</th><th>Req. For</th><th>Req. Type </th><th>Req. Model</th>";
            status = status + " <th>Unit Price </th><th>Quantity</th><th>Amount </th><th>Approve By</th><th>Approve On </th><th>Action</th>";
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
                        { 'data': 'DocumentNumber' },
                        { 'data': 'DocumentDate' },
                        { 'data': 'Location' },
                        { 'data': 'EntityCode' },
                        { 'data': 'NetworkName' },
                        { 'data': 'ContactPerson' },
                        { 'data': 'ContactNo' },
                        { 'data': 'DAS' },
                        { 'data': 'Enterby' },
                        { 'data': 'EnterOn' },
                        { 'data': 'Status' },
                        { 'data': 'ModuleType' },
                        { 'data': 'RecvModel' },
                        { 'data': 'SerialNumber' },
                        { 'data': 'Rate' },
                        { 'data': 'Remarks' },
                        { 'data': 'Remote' },
                        { 'data': 'CordAdaptor' },
                        { 'data': 'ReqFor' },
                        { 'data': 'ReqType' },
                        { 'data': 'ReqModel' },
                        { 'data': 'UnitPrice' },
                        { 'data': 'Quantity' },
                        { 'data': 'Amount' },
                        { 'data': 'Approve_By' },
                        { 'data': 'Approve_On' },
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
                status = status + " <th>Document No.</th><th>Document Date</th><th>Location</th><th>Entity Code</th><th>Network Name</th><th>Contact Person</th><th>Contact No</th> ";
                status = status + " <th>DAS</th><th>Enter by</th><th>Enter On</th><th>Status</th><th>Module Type</th>";
                status = status + " <th>Model</th><th>SerialNumber</th><th>Rate</th><th>Remarks</th><th>Remote </th> ";
                status = status + " <th>Cord/Adaptor</th><th>Req. For</th><th>Req. Type </th><th>Req. Model</th>";
                status = status + " <th>Unit Price </th><th>Quantity</th><th>Amount </th><th>Approve By</th><th>Approve On </th><th>Action</th>";
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
                        { 'data': 'DocumentNumber' },
                        { 'data': 'DocumentDate' },
                        { 'data': 'Location' },
                        { 'data': 'EntityCode' },
                        { 'data': 'NetworkName' },
                        { 'data': 'ContactPerson' },
                        { 'data': 'ContactNo' },
                        { 'data': 'DAS' },
                        { 'data': 'Enterby' },
                        { 'data': 'EnterOn' },
                        { 'data': 'Status' },
                        { 'data': 'ModuleType' },
                        { 'data': 'RecvModel' },
                        { 'data': 'SerialNumber' },
                        { 'data': 'Rate' },
                        { 'data': 'Remarks' },
                        { 'data': 'Remote' },
                        { 'data': 'CordAdaptor' },
                        { 'data': 'ReqFor' },
                        { 'data': 'ReqType' },
                        { 'data': 'ReqModel' },
                        { 'data': 'UnitPrice' },
                        { 'data': 'Quantity' },
                        { 'data': 'Amount' },
                        { 'data': 'Approve_By' },
                        { 'data': 'Approve_On' },
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

var rosterstatus = false;
function WorkingRoster() {
    $.ajax({
        type: "POST",
        url: 'STBSchemeSearch.aspx/CheckWorkingRoster',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        data: JSON.stringify({ module_ID: '8' }),
        success: function (response) {
            if (response.d.split('~')[0] == "true") {
                rosterstatus = true;
            }
            else if (response.d.split('~')[0] == "false") {
                rosterstatus = false;
                $("#spnbegin").text(response.d.split('~')[1]);
                $("#spnEnd").text(response.d.split('~')[2]);
            }
        },
    });
}

function WorkingRosterClick() {
    $("#divPopHead").addClass('hide');
}

function RequisitionViewDetails(values, Document) {
    setTimeout(function () {
        $.ajax({
            type: "POST",
            url: "STBSchemeSearch.aspx/STBRequisitionDetails",
            data: JSON.stringify({ STBRequisitionID: values, Document: Document }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var list = msg.d;
                $("#txtDocumentNumberSTBReq").val(msg.d.DocumentNumber);
                $("#txtLocationSTBReq").val(msg.d.Location);
                $("#txtDateSTBReq").val(msg.d.DocumentDate);
                $("#txtRequisitionTypeSTBReq").val(msg.d.RequisitionType);
                $("#txtRequisitionForSTBReq").val(msg.d.RequisitionFor);
                $("#txtEntityCodeSTBReq").val(msg.d.EntityCode);
                $("#txtNetworkNameSTBReq").val(msg.d.NetworkName);
                $("#txtContactPersonSTBReq").val(msg.d.ContactPerson);
                $("#txtContactNumberSTBReq").val(msg.d.ContactNo);
                $("#txtDASSTBReq").val(msg.d.DAS);


                $("#spnPayUsingAcountBalance").text(msg.d.PayUsingAcountBalance);
                $("#spnNoPayment").text(msg.d.NoPayment);
                $("#txtPaymentRelatedRemarks").val(msg.d.PaymentRelatedRemarksNotes);

                $("#DivCancelDetails").addClass('hide');
                $("#DivCancelData").addClass('hide');

                var status = " <table id='dataTableReq' class='table table-striped table-bordered display nowrap' style='width: 100%'>";
                status = status + " <thead><tr>";
                status = status + " <th>Srl.No.</th><th>Model</th><th>Unit Price</th><th>Quantity</th><th>Amount</th><th>Remarks</th>";
                status = status + " </tr></thead>";
                status = status + " </table>";
                $('#divSTBReqRechargeDtls').html(status);

                setTimeout(function () {
                    $('#dataTableReq').DataTable({
                        scrollX: true,
                        fixedColumns: {
                            rightColumns: 1
                        },
                        data: msg.d.DetailsList,
                        columns: [
                           { 'data': 'SrlNo' },
                           { 'data': 'Model' },
                           { 'data': 'UnitPrice' },
                           { 'data': 'Quantity' },
                           { 'data': 'Amount' },
                           { 'data': 'Remarks' },
                        ],
                    });
                }, 400);

                if (msg.d.RequisitionFor != "Fresh Issue") {
                    $('#divSTBReqRechargeDtls2').removeClass('hide');
                    var status = " <table id='dataTableReq2' class='table table-striped table-bordered display nowrap' style='width: 100%'>";
                    status = status + " <thead><tr>";
                    status = status + " <th>Srl.No.</th><th>Model</th><th>Unit Price</th><th>Quantity</th><th>Amount</th><th>Remarks</th>";
                    status = status + " </tr></thead>";
                    status = status + " </table>";
                    $('#divSTBReqRechargeDtls2').html(status);

                    setTimeout(function () {
                        $('#dataTableReq2').DataTable({
                            scrollX: true,
                            fixedColumns: {
                                rightColumns: 1
                            },
                            data: msg.d.DetailsList2,
                            columns: [
                               { 'data': 'SrlNo' },
                               { 'data': 'Model' },
                               { 'data': 'UnitPrice' },
                               { 'data': 'Quantity' },
                               { 'data': 'Amount' },
                               { 'data': 'Remarks' },
                            ],
                        });
                    }, 400);
                }
                else {
                    $('#divSTBReqRechargeDtls2').addClass('hide');
                }

                $("#detailsSTBReqModal").modal('show');
            }
        });
    }, 1000);
}

function SchemeReceiveViewDetails(values, Document) {
    setTimeout(function () {
        $.ajax({
            type: "POST",
            url: "STBSchemeSearch.aspx/SchemeReceivedDetails",
            data: JSON.stringify({ SchemeReceivedID: values }),
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

                var status = " <table id='dataTable2' class='table table-striped table-bordered display nowrap' style='width: 100%'>";
                status = status + " <thead><tr>";
                status = status + " <th>Device Type</th><th>Model</th><th>Serial Number</th><th>Rate</th>";
                status = status + " <th>Remarks </th><th>Remote </th><th>Card/Adaptor</th></tr></thead>";

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
                           { 'data': 'Rate' },
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

function SchemeReceiveEdit(values, Document) {
    $("#hdnmodule_ID").val(values);
    $.ajax({
        type: "POST",
        url: "STBSchemeSearch.aspx/STBRequisitionNoDetails",
        data: JSON.stringify({ STBSchemeReceivedID: values }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var list = msg.d;
            $("#txtRequisitionNo1").val(msg.d.ReqNo1);
            $("#txtRequisitionNo2").val(msg.d.ReqNo2);
            $("#txtRequisitionNo3").val(msg.d.ReqNo3);
            $("#txtRequisitionNo4").val(msg.d.ReqNo4);
            $("#txtRequisitionNo5").val(msg.d.ReqNo5);
            $("#txtRemarks").val(msg.d.Remarks);
           

            $("#CloseRequisitionpop").modal('toggle');
        }
    });
}



function CloseRequisition() {
    if ($("#txtRequisitionNo1").val() == "" && $("#txtRequisitionNo2").val() == "" && $("#txtRequisitionNo3").val() == ""
        && $("#txtRequisitionNo4").val() == "" && $("#txtRequisitionNo5").val() == "") {

        jAlert("Please enter one Requisition no.", "Alert", function () {
            $("#txtRequisitionNo1").focus();
            return;
        });
        return;
    }
    $.ajax({
        type: "POST",
        url: 'STBSchemeSearch.aspx/RequisitionNoUpdate',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        data: JSON.stringify({
            Received_id: $("#hdnmodule_ID").val(), ReqNo1: $("#txtRequisitionNo1").val(), ReqNo2: $("#txtRequisitionNo2").val(), ReqNo3: $("#txtRequisitionNo3").val()
            , ReqNo4: $("#txtRequisitionNo4").val(), ReqNo5: $("#txtRequisitionNo5").val(), Remarks: $("#txtRemarks").val()
        }),
        success: function (response) {
            if (response.d.split('~')[0] == "true") {
                jAlert("STB Scheme-Received Closed successfully.", "Alert", function () {
                    $("#txtRequisitionNo1").val("");
                    $("#txtRequisitionNo2").val("");
                    $("#txtRequisitionNo3").val("");
                    $("#txtRequisitionNo4").val("");
                    $("#txtRequisitionNo5").val("");
                    $("#txtRemarks").val("");
                    $("#hdnmodule_ID").val("");
                    $("#CloseRequisitionpop").modal('toggle');
                   
                });
            }
            else {
                jAlert("STB Scheme-Received Not Closed!!");
            }
        },
    });
}