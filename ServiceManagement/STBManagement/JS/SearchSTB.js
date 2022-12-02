
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

    $('#rcn, #sln, #lct,#slDate').hide();
    $('#idTypes').change(function () {
        var value = $(this).val();
        if (value == 1) {
            $('#rcn, #sln, #lct, #slnTrack,#slDate').hide();
            $('#rcn').show();
        } else if (value == 2) {
            $('#rcn, #sln, #lct, #slnTrack,#slDate').hide();
            $('#sln').show();
        } else if (value == 3) {
            $('#rcn, #sln, #lct, #slnTrack,#slDate').hide();
            $('#lct').show();
        } else if (value == 4) {
            $('#rcn, #sln, #lct, #slnTrack,#slDate').hide();
            $('#slDate').show();
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
                    url: "search.aspx/SearchList",
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
                               { 'data': 'ModuleType' },
                               { 'data': 'Type' },
                               { 'data': 'EntityCode' },
                               { 'data': 'NetworkName' },
                               { 'data': 'ContactPerson' },
                               { 'data': 'ContactNo' },
                               { 'data': 'Location' },
                               { 'data': 'Enterby' },
                               { 'data': 'EnterOn' },
                               { 'data': 'Status' },
                               { 'data': 'ReasonForCancel' },
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
                    url: "search.aspx/WalletSearchList",
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
                               { 'data': 'ModuleType' },
                               { 'data': 'Type' },
                               { 'data': 'EntityCode' },
                               { 'data': 'NetworkName' },
                               { 'data': 'ContactPerson' },
                               { 'data': 'ContactNo' },
                               { 'data': 'Location' },
                               { 'data': 'DAS' },
                               { 'data': 'Enterby' },
                               { 'data': 'EnterOn' },
                               { 'data': 'Status' },
                               { 'data': 'ReasonForCancel' },
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
    WorkingRoster();
    if (rosterstatus) {
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
    }
    else {
        $("#divPopHead").removeClass('hide');
    }
}

function DocumentNoSearch() {
    var DocumentNo = $("#DocumentsNo").val();
    $.ajax({
        type: "POST",
        url: "search.aspx/MoneyReceiptSearchList",
        data: JSON.stringify({ DocumentNo: DocumentNo, EntityCode: "", Branch: "0" }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var status = "";
            status = status + "<table id='dataTable' class='table table-striped table-bordered display nowrap' style='width:100%'>";
            status = status + " <thead><tr>";
            status = status + " <th>Document No.</th><th>Document Date</th><th>Quantity</th><th>Rate</th><th>Amount</th><th>Model</th><th>Location</th> ";
            status = status + " <th>Mode</th><th>Ref. No</th><th>Cheque No</th><th>Cheque Date</th><th>Bank Name</th>";
            status = status + " <th>Branch</th><th>Module</th><th>Type</th><th>Req. For</th><th>Entity Code </th> ";
            status = status + " <th>Network Name</th><th>Contact Person</th><th>Contact No </th><th>Inv. Updated</th>";
            status = status + " <th>Location </th><th>DAS</th><th>Dispatch </th><th>Dispt. Ack.</th><th>Ack. Remarks </th><th>Director</th>";
            status = status + " <th>Entered By</th><th>Entered On</th><th>Status</th><th>Cancel Reason</th><th>Approved By</th> ";
            status = status + " <th>Approved On</th><th>Approved Qty</th><th>Approved Price</th><th>Inv. Updated By</th><th>Inv. Updated On</th> ";
            status = status + " <th>Hold Reason</th><th>Reject Reason</th><th>Cancel Reason</th><th>Action</th>";

            status = status + " </tr></thead>";
            status = status + " </table>";

            $('#DivDeliveryDetails').html(status);

            $('#dataTable').DataTable({
                scrollX: true,
                fixedColumns: {
                    rightColumns: 1
                },
                data: msg.d,
                "ordering": false,
                columns: [
                   { 'data': 'DocumentNumber' },
                   { 'data': 'DocumentDate' },
                   { 'data': 'Quantity' },
                   { 'data': 'UnitPrice' },
                   { 'data': 'Amount' },
                   { 'data': 'Model' },
                   { 'data': 'branch_description' },
                   { 'data': 'Payment_Mode' },
                   { 'data': 'Ref_No' },
                   { 'data': 'Cheque_No' },
                   { 'data': 'Cheque_date' },
                   { 'data': 'PaymentDetails_BankName' },
                   { 'data': 'PaymentDetails_BranchName' },
                   { 'data': 'ModuleType' },
                   { 'data': 'Type' },
                   { 'data': 'ReqFor' },
                   { 'data': 'EntityCode' },
                   { 'data': 'NetworkName' },
                   { 'data': 'ContactPerson' },
                   { 'data': 'ContactNo' },
                   { 'data': 'InvUpdated' },
                   { 'data': 'Location' },
                   { 'data': 'DAS' },
                   { 'data': 'Dispatch' },
                   { 'data': 'DisptAck' },
                   { 'data': 'AckRemarks' },
                   { 'data': 'Director' },
                   { 'data': 'Enterby' },
                   { 'data': 'EnterOn' },
                   { 'data': 'Status' },
                   { 'data': 'ReasonForCancel' },
                   { 'data': 'Approve_bY' },
                   { 'data': 'Approve_On' },
                   { 'data': 'Approve_QTY' },
                   { 'data': 'Approve_Amount' },
                   { 'data': 'InvUpdatedBy' },
                   { 'data': 'InvUpdatedOn' },
                   { 'data': 'HoldRemarks' },
                   { 'data': 'RejectRemarks' },
                   { 'data': 'CancelRemarks' },
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
        url: "search.aspx/MoneyReceiptSearchList",
        data: JSON.stringify({ DocumentNo: "", EntityCode: EntityCode, Branch: "0" }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var status = "";
            status = status + "<table id='dataTable' class='table table-striped table-bordered display nowrap' style='width:100%'>";
            status = status + " <thead><tr>";
            status = status + " <th>Document No.</th><th>Document Date</th><th>Quantity</th><th>Rate</th><th>Amount</th><th>Model</th><th>Location</th> ";
            status = status + " <th>Mode</th><th>Ref. No</th><th>Cheque No</th><th>Cheque Date</th><th>Bank Name</th>";
            status = status + " <th>Branch</th><th>Module</th><th>Type</th><th>Req. For</th><th>Entity Code </th> ";
            status = status + " <th>Network Name</th><th>Contact Person</th><th>Contact No </th><th>Inv. Updated</th>";
            status = status + " <th>Location </th><th>DAS</th><th>Dispatch </th><th>Dispt. Ack.</th><th>Ack. Remarks </th><th>Director</th>";
            status = status + " <th>Entered By</th><th>Entered On</th><th>Status</th><th>Cancel Reason</th><th>Approved By</th><th>Approved On</th>";
            status = status + " <th>Approved Qty</th><th>Approved Price</th><th>Inv. Updated By</th><th>Inv. Updated On</th>";
            status = status + " <th>Hold Reason</th><th>Reject Reason</th><th>Cancel Reason</th><th>Action</th>";

            status = status + " </tr></thead>";
            status = status + " </table>";

            $('#DivDeliveryDetails').html(status);

            $('#dataTable').DataTable({
                scrollX: true,
                fixedColumns: {
                    rightColumns: 1
                },
                data: msg.d,
                "ordering": false,
                columns: [
                   { 'data': 'DocumentNumber' },
                   { 'data': 'DocumentDate' },
                   { 'data': 'Quantity' },
                   { 'data': 'UnitPrice' },
                   { 'data': 'Amount' },
                   { 'data': 'Model' },
                   { 'data': 'branch_description' },
                   { 'data': 'Payment_Mode' },
                   { 'data': 'Ref_No' },
                   { 'data': 'Cheque_No' },
                   { 'data': 'Cheque_date' },
                   { 'data': 'PaymentDetails_BankName' },
                   { 'data': 'PaymentDetails_BranchName' },
                   { 'data': 'ModuleType' },
                   { 'data': 'Type' },
                   { 'data': 'ReqFor' },
                   { 'data': 'EntityCode' },
                   { 'data': 'NetworkName' },
                   { 'data': 'ContactPerson' },
                   { 'data': 'ContactNo' },
                   { 'data': 'InvUpdated' },
                   { 'data': 'Location' },
                   { 'data': 'DAS' },
                   { 'data': 'Dispatch' },
                   { 'data': 'DisptAck' },
                   { 'data': 'AckRemarks' },
                   { 'data': 'Director' },
                   { 'data': 'Enterby' },
                   { 'data': 'EnterOn' },
                   { 'data': 'Status' },
                   { 'data': 'ReasonForCancel' },
                   { 'data': 'Approve_bY' },
                   { 'data': 'Approve_On' },
                   { 'data': 'Approve_QTY' },
                   { 'data': 'Approve_Amount' },
                   { 'data': 'InvUpdatedBy' },
                   { 'data': 'InvUpdatedOn' },
                   { 'data': 'HoldRemarks' },
                   { 'data': 'RejectRemarks' },
                   { 'data': 'CancelRemarks' },
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
            url: "search.aspx/MoneyReceiptSearchList",
            data: JSON.stringify({ DocumentNo: "", EntityCode: "", Branch: branchss }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var status = "";
                status = status + "<table id='dataTable' class='table table-striped table-bordered display nowrap' style='width:100%'>";
                status = status + " <thead><tr>";
                status = status + " <th>Document No.</th><th>Document Date</th><th>Quantity</th><th>Rate</th><th>Amount</th><th>Model</th><th>Location</th> ";
                status = status + " <th>Mode</th><th>Ref. No</th><th>Cheque No</th><th>Cheque Date</th><th>Bank Name</th>";
                status = status + " <th>Branch</th><th>Module</th><th>Type</th><th>Req. For</th><th>Entity Code </th> ";
                status = status + " <th>Network Name</th><th>Contact Person</th><th>Contact No </th><th>Inv. Updated</th>";
                status = status + " <th>Location </th><th>DAS</th><th>Dispatch </th><th>Dispt. Ack.</th><th>Ack. Remarks </th><th>Director</th>";
                status = status + " <th>Entered By</th><th>Entered On</th><th>Status</th><th>Cancel Reason</th><th>Approved By</th><th>Approved On</th>";
                status = status + " <th>Approved Qty</th><th>Approved Price</th><th>Inv. Updated By</th><th>Inv. Updated On</th>";
                status = status + " <th>Hold Reason</th><th>Reject Reason</th><th>Cancel Reason</th><th>Action</th>";

                status = status + " </tr></thead>";
                status = status + " </table>";

                $('#DivDeliveryDetails').html(status);

                $('#dataTable').DataTable({
                    scrollX: true,
                    fixedColumns: {
                        rightColumns: 1
                    },
                    data: msg.d,
                    "ordering": false,
                    columns: [
                       { 'data': 'DocumentNumber' },
                       { 'data': 'DocumentDate' },
                       { 'data': 'Quantity' },
                       { 'data': 'UnitPrice' },
                       { 'data': 'Amount' },
                       { 'data': 'Model' },
                       { 'data': 'branch_description' },
                       { 'data': 'Payment_Mode' },
                       { 'data': 'Ref_No' },
                       { 'data': 'Cheque_No' },
                       { 'data': 'Cheque_date' },
                       { 'data': 'PaymentDetails_BankName' },
                       { 'data': 'PaymentDetails_BranchName' },
                       { 'data': 'ModuleType' },
                       { 'data': 'Type' },
                       { 'data': 'ReqFor' },
                       { 'data': 'EntityCode' },
                       { 'data': 'NetworkName' },
                       { 'data': 'ContactPerson' },
                       { 'data': 'ContactNo' },
                       { 'data': 'InvUpdated' },
                       { 'data': 'Location' },
                       { 'data': 'DAS' },
                       { 'data': 'Dispatch' },
                       { 'data': 'DisptAck' },
                       { 'data': 'AckRemarks' },
                       { 'data': 'Director' },
                       { 'data': 'Enterby' },
                       { 'data': 'EnterOn' },
                       { 'data': 'Status' },
                       { 'data': 'ReasonForCancel' },
                       { 'data': 'Approve_bY' },
                       { 'data': 'Approve_On' },
                       { 'data': 'Approve_QTY' },
                       { 'data': 'Approve_Amount' },
                       { 'data': 'InvUpdatedBy' },
                       { 'data': 'InvUpdatedOn' },
                       { 'data': 'HoldRemarks' },
                       { 'data': 'RejectRemarks' },
                       { 'data': 'CancelRemarks' },
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
        url: "search.aspx/MoneyReceiptDateWiseSearchList",
        data: JSON.stringify({ hfFromDate: hfFromDate, hfToDate: hfToDate }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var status = "";
            status = status + "<table id='dataTable' class='table table-striped table-bordered display nowrap' style='width:100%'>";
            status = status + " <thead><tr>";
            status = status + " <th>Document No.</th><th>Document Date</th><th>Quantity</th><th>Rate</th><th>Amount</th><th>Model</th><th>Location</th> ";
            status = status + " <th>Mode</th><th>Ref. No</th><th>Cheque No</th><th>Cheque Date</th><th>Bank Name</th>";
            status = status + " <th>Branch</th><th>Module</th><th>Type</th><th>Req. For</th><th>Entity Code </th> ";
            status = status + " <th>Network Name</th><th>Contact Person</th><th>Contact No </th><th>Inv. Updated</th>";
            status = status + " <th>Location </th><th>DAS</th><th>Dispatch </th><th>Dispt. Ack.</th><th>Ack. Remarks </th><th>Director</th>";
            status = status + " <th>Entered By</th><th>Entered On</th><th>Status</th><th>Cancel Reason</th><th>Approved By</th><th>Approved On</th>";
            status = status + " <th>Approved Qty</th><th>Approved Price</th><th>Inv. Updated By</th><th>Inv. Updated On</th>";
            status = status + " <th>Hold Reason</th><th>Reject Reason</th><th>Cancel Reason</th><th>Action</th>";

            status = status + " </tr></thead>";
            status = status + " </table>";

            $('#DivDeliveryDetails').html(status);

            $('#dataTable').DataTable({
                scrollX: true,
                fixedColumns: {
                    rightColumns: 1
                },
                data: msg.d,
                "ordering": false,
                columns: [
                   { 'data': 'DocumentNumber' },
                   { 'data': 'DocumentDate' },
                   { 'data': 'Quantity' },
                   { 'data': 'UnitPrice' },
                   { 'data': 'Amount' },
                   { 'data': 'Model' },
                   { 'data': 'branch_description' },
                   { 'data': 'Payment_Mode' },
                   { 'data': 'Ref_No' },
                   { 'data': 'Cheque_No' },
                   { 'data': 'Cheque_date' },
                   { 'data': 'PaymentDetails_BankName' },
                   { 'data': 'PaymentDetails_BranchName' },
                   { 'data': 'ModuleType' },
                   { 'data': 'Type' },
                   { 'data': 'ReqFor' },
                   { 'data': 'EntityCode' },
                   { 'data': 'NetworkName' },
                   { 'data': 'ContactPerson' },
                   { 'data': 'ContactNo' },
                   { 'data': 'InvUpdated' },
                   { 'data': 'Location' },
                   { 'data': 'DAS' },
                   { 'data': 'Dispatch' },
                   { 'data': 'DisptAck' },
                   { 'data': 'AckRemarks' },
                   { 'data': 'Director' },
                   { 'data': 'Enterby' },
                   { 'data': 'EnterOn' },
                   { 'data': 'Status' },
                   { 'data': 'ReasonForCancel' },
                   { 'data': 'Approve_bY' },
                   { 'data': 'Approve_On' },
                   { 'data': 'Approve_QTY' },
                   { 'data': 'Approve_Amount' },
                   { 'data': 'InvUpdatedBy' },
                   { 'data': 'InvUpdatedOn' },
                   { 'data': 'HoldRemarks' },
                   { 'data': 'RejectRemarks' },
                   { 'data': 'CancelRemarks' },
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

function WalletRecharge() {
    var DocumentNo = $("#DocumentNo").val();
    $.ajax({
        type: "POST",
        url: "search.aspx/WalletRechargeSearchList",
        data: JSON.stringify({ DocumentNo: DocumentNo }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var status = "";
            status = status + "<table id='dataTable' class='table table-striped table-bordered display nowrap' style='width:100%'>";
            status = status + " <thead><tr>";
            status = status + " <th>Document No.</th><th>Document Date</th><th>Type</th><th>Entity Code </th><th>Network Name</th><th>Contact Person</th><th>Contact No </th>";
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
                   { 'data': 'Type' },
                   { 'data': 'EntityCode' },
                   { 'data': 'NetworkName' },
                   { 'data': 'ContactPerson' },
                   { 'data': 'ContactNo' },
                   { 'data': 'Location' },
                   { 'data': 'DAS' },
                   { 'data': 'Enterby' },
                   { 'data': 'EnterOn' },
                   { 'data': 'Status' },
                   { 'data': 'ReasonForCancel' },
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
                status = status + " <th>Receipt Challan</th><th>Type</th><th>Entity Code </th><th>Network Name</th><th>Contact Person</th><th>Technician </th>";
                status = status + " <th>Location </th><th>Received By</th><th>Received On</th><th>Assigned By</th><th>Assigned On</th><th>Serv. Entered By</th>";
                status = status + " <th>Serv. Entered On</th><th>Warranty</th><th>Confirm Delivered</th><th>Confirm Delivered Date</th><th>Delivered By</th>";
                status = status + " <th>Delivered On</th><th>Status</th><th>Approved By</th><th>Approved On</th><th>Approved Qty</th><th>Approved Price</th><th>Inv. Updated By</th><th>Inv. Updated On</th><th>Action</th>";
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
                       { 'data': 'Approve_bY' },
                       { 'data': 'Approve_On' },
                       { 'data': 'Approve_QTY' },
                       { 'data': 'Approve_Amount' },
                       { 'data': 'InvUpdatedBy' },
                       { 'data': 'InvUpdatedOn' },
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
            var status = "";
            status = status + "<table id='dataTable' class='table table-striped table-bordered display nowrap' style='width:100%'>";
            status = status + " <thead><tr>";
            status = status + " <th>Challan No</th><th>Searial No</th><th>Model </th><th>Entity Code </th><th>Network Name</th><th>Technician </th><th>Problem Reported</th><th>Service Action</th><th>Component </th>";
            status = status + " <th>Warranty </th><th>Stock Entry</th><th>New Serial No</th><th>New Warranty</th><th>New Model</th><th>Return Reason</th><th>Billable</th>";
            status = status + " <th>Problem Found</th><th>Remarks</th><th>Warranty Status</th><th>Confirm Delivered</th><th>Confirm Delivered Date</th><th>Action</th>";
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
                   { 'data': 'SearialNo' },
                   { 'data': 'Model' },
                   { 'data': 'EntityCode' },
                   { 'data': 'NetworkName' },
                   { 'data': 'Technician' },
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
    MoneyReceiptView(values, 'Normal');
    //else if (module == "DU") {
    //    AssignJobView(values, module);
    //}
    //else if (module == "DN") {
    //    ServiceEntryView(values, module);
    //}
    //else if (module == "DE") {
    //    DeliveryView(values, module);
    //}
    //else if (module == "Jobsheet") {
    //    JobsheetView(values, module);
    //}
}

function MoneyReceiptView(values, module) {
    setTimeout(function () {
        $.ajax({
            type: "POST",
            url: "search.aspx/ReceptDetails",
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
                $("#txtType").val(msg.d.EntryType);

                $("#hdnIsCancelReceipt").val(msg.d.IsCancel);

                if ($("#hdnIsCancelReceipt").val() == "True") {

                    $("#DivReceiptCancelDetails").removeClass('hide');
                    $("#DivReceiptCancelData").removeClass('hide');
                    $("#txtReceiptCancellationReason").val(msg.d.ReasonForCancel);
                    $("#txtReceiptCancelRequestBY").val(msg.d.Cancel_Create_by);
                    $("#txtReceiptCancelRequestON").val(msg.d.Cancel_Create_date);
                }
                else {

                    $("#DivReceiptCancelDetails").addClass('hide');
                    $("#DivReceiptCancelData").addClass('hide');

                }

                var status = " <table id='dataTable2' class='table table-striped table-bordered display nowrap' style='width: 100%'>";
                status = status + " <thead><tr>";
                status = status + " <th>Srl.No.</th><th>Payment Mode</th><th>Amount</th><th>Cheque No</th><th>Cheque Date</th><th>Ref No.</th><th>Bank Name</th><th>Branch Name</th><th>Remarks</th>";

                status = status + " </tr></thead>";

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
                           { 'data': 'Payment_Id' },
                           { 'data': 'Payment_Mode' },
                           { 'data': 'Payment_Amount' },
                           { 'data': 'Cheque_No' },
                           { 'data': 'Cheque_date' },
                           { 'data': 'Ref_No' },
                           { 'data': 'PaymentDetails_BankName' },
                           { 'data': 'PaymentDetails_BranchName' },
                           { 'data': 'Remarks' },

                        ],
                    });
                }, 400);
                $("#detailsModal").modal('show');
            }
        });
    }, 1000);
}

function WalletViewDetails(values, module) {
    WalletRechargeView(values, 'Normal');
    //else if (module == "DU") {
    //    AssignJobView(values, module);
    //}
    //else if (module == "DN") {
    //    ServiceEntryView(values, module);
    //}
    //else if (module == "DE") {
    //    DeliveryView(values, module);
    //}
    //else if (module == "Jobsheet") {
    //    JobsheetView(values, module);
    //}
}

function WalletRechargeView(values, module) {
    setTimeout(function () {
        $.ajax({
            type: "POST",
            url: "search.aspx/WalletRechargeDetails",
            data: JSON.stringify({ WalletID: values, module: module }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var list = msg.d;
                $("#txtDocumentNumberWallet").val(msg.d.DocumentNumber);
                $("#txtLocationWallet").val(msg.d.Branch);
                $("#txtDateWallet").val(msg.d.DocumentDate);
                $("#EntityCodeWallet").val(msg.d.EntityCode);
                $("#NetworkNameWallet").val(msg.d.NetworkName);
                $("#ContactPersonWallet").val(msg.d.ContactPerson);
                $("#ContactNumberWallet").val(msg.d.ContactNo);
                $("#txtTypeWallet").val(msg.d.EntryType);


                $("#hdnIsCancelWallet").val(msg.d.IsCancel);

                if ($("#hdnIsCancelWallet").val() == "True") {

                    $("#DivCancelDetails").removeClass('hide');
                    $("#DivCancelData").removeClass('hide');
                    $("#txtReasonCancel").val(msg.d.ReasonForCancel);
                    $("#txtReasonCancelMadeBy").val(msg.d.Cancel_Create_by);
                    $("#txtReasonCancelMadeOn").val(msg.d.Cancel_Create_date);
                }
                else {

                    $("#DivCancelDetails").addClass('hide');
                    $("#DivCancelData").addClass('hide');

                }

                var status = " <table id='dataTableWR' class='table table-striped table-bordered display nowrap' style='width: 100%'>";
                status = status + " <thead><tr>";
                status = status + " <th>Srl.No.</th><th>Payment Mode</th><th>Amount</th><th>Cheque No</th><th>Cheque Date</th><th>Ref No.</th><th>Bank Name</th><th>Branch Name</th><th>Remarks</th>";

                status = status + " </tr></thead>";

                status = status + " </table>";
                $('#divWalletRechargeDtls').html(status);

                setTimeout(function () {
                    $('#dataTableWR').DataTable({
                        scrollX: true,
                        fixedColumns: {
                            rightColumns: 1
                        },
                        data: msg.d.DetailsList,
                        columns: [
                           { 'data': 'Payment_Id' },
                           { 'data': 'Payment_Mode' },
                           { 'data': 'Payment_Amount' },
                           { 'data': 'Cheque_No' },
                           { 'data': 'Cheque_date' },
                           { 'data': 'Ref_No' },
                           { 'data': 'Payment_BankName' },
                           { 'data': 'Payment_BranchName' },
                           { 'data': 'Remarks' },

                        ],
                    });
                }, 400);
                $("#detailsWalletModal").modal('show');
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
    WorkingRoster();
    if (rosterstatus) {
        var URL = '../../../OMS/Management/Activities/EntriesDocuments.aspx?idbldng=' + obj + '&type=SrvSearch';
        window.location.href = URL;
    }
    else {
        $("#divPopHead").removeClass('hide');
    }
}

var rosterstatus = false;
function WorkingRoster() {
    $.ajax({
        type: "POST",
        url: 'search.aspx/CheckWorkingRoster',
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

function RequisitionViewDetails(values, module) {
    setTimeout(function () {
        $.ajax({
            type: "POST",
            url: "search.aspx/STBRequisitionDetails",
            data: JSON.stringify({ STBRequisitionID: values, module: module }),
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

function ReturnRequisitionViewDetails(values, module) {
    setTimeout(function () {
        $.ajax({
            type: "POST",
            url: "search.aspx/STBReturnRequisitionDetails",
            data: JSON.stringify({ STBReturnRequisitionID: values, module: module }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var list = msg.d;
                $("#txtDocumentNumberSTBReturn").val(msg.d.DocumentNumber);
                $("#txtLocationSTBReturn").val(msg.d.Location);
                $("#txtDateSTBReturn").val(msg.d.DocumentDate);
                $("#txtRequisitionTypeSTBReturn").val(msg.d.RequisitionType);
                $("#txtRequisitionForSTBReturn").val(msg.d.RequisitionFor);
                $("#txtEntityCodeSTBReturn").val(msg.d.EntityCode);
                $("#txtNetworkNameSTBReturn").val(msg.d.NetworkName);
                $("#txtContactPersonSTBReturn").val(msg.d.ContactPerson);
                $("#txtContactNumberSTBReturn").val(msg.d.ContactNo);
                $("#txtDASSTBReturn").val(msg.d.DAS);


                $("#spnPayUsingAcountBalanceReturn").text(msg.d.PayUsingAcountBalance);
                $("#spnNoPaymentReturn").text(msg.d.NoPayment);
                $("#txtRemarksReturn").val(msg.d.PaymentRelatedRemarksNotes);


                var status = " <table id='dataTableReturn' class='table table-striped table-bordered display nowrap' style='width: 100%'>";
                status = status + " <thead><tr>";
                status = status + " <th>Srl.No.</th><th>Model</th><th>Unit Price</th><th>Quantity</th><th>Amount</th><th>Remarks</th>";
                status = status + " </tr></thead>";
                status = status + " </table>";
                $('#divSTBReturnRechargeDtls').html(status);

                setTimeout(function () {
                    $('#dataTableReturn').DataTable({
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

                $("#detailsSTBReturnModal").modal('show');
            }
        });
    }, 1000);
}

function AddAttachmentMoneyReceipt(obj, doc) {
    var URL = '../../../OMS/Management/Activities/EntriesDocuments.aspx?idbldng=' + obj + '&type=StbMoneyReceiptSearch';
    window.location.href = URL;
}

function AddAttachmentWalletRecharge(obj, doc) {
    var URL = '../../../OMS/Management/Activities/EntriesDocuments.aspx?idbldng=' + obj + '&type=StbWalletRechargeSearch';
    window.location.href = URL;
}

function AddAttachmentSTBRequisition(obj, doc) {
    var URL = '../../../OMS/Management/Activities/EntriesDocuments.aspx?idbldng=' + obj + '&type=StbRequisitionSearch';
    window.location.href = URL;
}

function AddAttachmentReturnRequisition(obj, doc) {
    var URL = '../../../OMS/Management/Activities/EntriesDocuments.aspx?idbldng=' + obj + '&type=StbReturnRequisitionSearch';
    window.location.href = URL;
}

function ClickUpdateInventory(val, doc) {
    WorkingRoster();
    if (rosterstatus) {
        $("#hdnmodule_ID").val(val);
        $.ajax({
            type: "POST",
            url: "search.aspx/STBReqUpdateInventory",
            data: JSON.stringify({ STBRequisitionID: val }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var list = msg.d;
                $("#txtGatepassNo").val(msg.d.GatepassNo);
                $("#txtUpdateInventoryRemarks").val(msg.d.Remarks);
                $("#UpdateInventorypop").modal('toggle');
            }
        });
    }
    else {
        $("#divPopHead").removeClass('hide');
    }
}

function UpdateInventory() {
    var Remarks = $("#txtUpdateInventoryRemarks").val();
    var GatepassNo = $("#txtGatepassNo").val();
    var module_id = $("#hdnmodule_ID").val();
    if (GatepassNo == "") {
        jAlert("Please enter Gatepass No.", "Alert", function () {
            setTimeout(function () {
                $("#txtGatepassNo").focus();
                return
            }, 200);
        });
        return
    }

    var file = ""
    if (document.querySelector('#ReceiptChallanDoc').files.length > 0) {
        file = document.querySelector('#ReceiptChallanDoc').files[0];
    }

    var AttachDocument = ""
    if (file != "") {
        SaveAttachment();
        AttachDocument = "/Documents/STBReturnRequisition/" + file.name;
    }

    $.ajax({
        type: "POST",
        url: "search.aspx/UpdateInventory",
        data: JSON.stringify({ Document_ID: module_id, Remarks: Remarks, GatepassNo: GatepassNo, AttachDocument: AttachDocument }),
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response.d) {
                if (response.d == "true") {
                    jAlert("Updated Successfully.", "Alert", function () {

                        $("#txtUpdateInventoryRemarks").val("");
                        $("#txtGatepassNo").val("");
                        $("#hdnmodule_ID").val("");
                        $("#UpdateInventorypop").toggle();
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


var filenames = [];
function SaveAttachment() {
    var process = 0;

    var fileUploadCheck = $("#ReceiptChallanDoc").get(0);
    var docFileName = "STBReturnRequisition";
    var GatepassNo = $("#txtGatepassNo").val();

    for (var i = 0; i < $('#ReceiptChallanDoc').get(0).files.length; ++i) {
        filenames.push($('#ReceiptChallanDoc').get(0).files[i].name);
    }


    if (process == 0) {
        if (GatepassNo != "") {
            if (window.FormData !== undefined) {
                var fileUpload = $("#ReceiptChallanDoc").get(0);
                var files = fileUpload.files;
                var fileData = new FormData();

                for (var i = 0; i < files.length; i++) {
                    fileData.append(files[i].name, files[i]);
                }

                fileData.append('docFileName', docFileName);

                $.ajax({
                    type: "POST",
                    url: '/SRVFileuploadDelivery/AttachmentDocumentAddUpdate',
                    contentType: false,
                    processData: false,
                    data: fileData,
                    success: function (response) {

                        if (response) {
                            // jAlert("Documents Added Successfully!");
                        }
                        else {
                            jAlert("Upload failed!");
                            return
                        }
                    }
                });
            } else {
                jAlert("Upload failed Please try again later!");
                return
            }
        }
        else {
            jAlert("Upload failed Please try again later!");
            return
        }
    }

}