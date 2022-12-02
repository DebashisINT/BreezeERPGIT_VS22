
$(document).ready(function () {
    $('.js-example-basic-single').select2();

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
    $('#dataTable').DataTable({
        scrollX: true,
        fixedColumns: {
            rightColumns: 2
        }
    });
    $('#dataTable2').DataTable({
    });
    $('#dataTable3').DataTable({
    });
    $('[data-toggle="tooltip"]').tooltip();

    $(".date").datepicker({
        autoclose: true,
        todayHighlight: true,
        format: 'dd-mm-yyyy'
    }).datepicker('update', new Date());

    cFormDate.SetDate(new Date());

    if ($("#Hidden_add_edit").val() == "edit") {
        $("#btnSaveNew").addClass('hide');

    }
    else if ($("#Hidden_add_edit").val() == "view") {
        $("#btnSaveNew").addClass('hide');
        $("#btnSaveExit").addClass('hide');
        $("#btnAdd").addClass('hide');
    }
    else {
        $("#btnSaveNew").removeClass('hide');
        $("#btnSaveExit").removeClass('hide');
        $("#btnAdd").removeClass('hide');
    }
    cddlBranch.SetEnabled(false);
    // Mantis Issue 24413
    cselModel.SetEnabled(false);
    // End of Mantis Issue 24413
});


function grid_EndCallBack(s, e) {
    //var url = 'erp_addHoliday.aspx';
    //window.location.href = url;
}

function AddDevice() {
    var DeviceType = cDiviceTyp.GetText();
    var Model = $("#txtModel").val();
    var DeviceNumber = $("#txtDeviceNumber").val();
    var Warranty = $("#txtWarranty").val();
    var Problem = cddlProblem.GetText();//$("#ddlProblem :selected").text();
    var Remarks = $("#txtRemarks").val();
    var Remote = "No";
    var CardAdaptor = "No";
    var DeviceTypeId = cDiviceTyp.GetValue();
    var ProblemID = cddlProblem.GetValue();//$("#ddlProblem :selected").val();

    var EntityCode = $("#txtEntityCode").val();

    if ($('#chkRemote').is(':checked') == true) {
        Remote = "Yes";
    }
    if ($('#chkCardAdaptor').is(':checked') == true) {
        CardAdaptor = "Yes";
    }

    var Gu_id = $('#hdnGuid').val();

    var suc = true;

    if (cCmbScheme.GetValue() == "0") {
        jAlert("Please select Numbering Scheme.");
        cCmbScheme.Focus();
        suc = false;
        return
    }


    if ($("#txtEntityCode").val() == "") {
        jAlert("Please enter Entity Code.")
        $("#txtEntityCode").focus();
        suc = false;
        return
    }

    if ($("#txtNetworkName").val() == "") {
        jAlert("Please enter Network Name.")
        $("#txtNetworkName").focus();
        suc = false;
        return
    }

    if ($("#txtContactPerson").val() == "") {
        jAlert("Please Enter Contact Person.", "Alert", function () {
            setTimeout(function () {
                $("#txtContactPerson").focus();
                suc = false;
                return
            }, 200);
        });
        suc = false;
        return
    }

    if ($("#txtContactNo").val() == "") {
        jAlert("Please Enter Contact Number.", "Alert", function () {
            setTimeout(function () {
                $("#txtContactNo").focus();
                suc = false;
                return
            }, 200);
        });
        suc = false;
        return
    }

    if ($("#hdnEntityCode").val() != "") {
        if ($("#hdnEntityCode").val() != EntityCode) {
            jAlert("This serial number is belongs to another Entity Code.", "Alert", function () {
                setTimeout(function () {
                    $("#txtEntityCode").focus();
                }, 200);
            });
            suc = false;
            return
        }
    }


    if ($("#txtDeviceNumber").val() == "") {
        jAlert("Please enter Serial Number.")
        $("#txtDeviceNumber").focus();
        suc = false;
        return
    }

    if (cDiviceTyp.GetValue() == "0") {
        jAlert("Please select Device Type.")
        cDiviceTyp.Focus();
        suc = false;
        return
    }

    if ($("#txtModel").val() == "") {
        jAlert("Please enter Model.")
        $("#txtModel").focus();
        suc = false;
        return
    }


    if (cddlProblem.GetValue() == "0") {
        jAlert("Please select Problem.")
        cddlProblem.Focus();
        suc = false;
        return
    }

    if (Problem == "OTHER") {
        if ($("#txtRemarks").val() == "") {
            jAlert("Please enter Remarks.")
            $("#txtRemarks").focus();
            suc = false;
            return
        }
    }

    if ($("#hdnServiceHistoryValidation").val() == '0') {
        jAlert("Please check Service History.")
        suc = false;
        return
    }

    $.ajax({
        type: "POST",
        url: "ReceiptChallan.aspx/AddData",
        data: JSON.stringify({
            DeviceType: DeviceType, Model: Model, DeviceNumber: DeviceNumber, Warranty: Warranty,
            Problem: Problem, Remarks: Remarks, Remote: Remote, CardAdaptor: CardAdaptor, Guids: Gu_id,
            DeviceTypeId: DeviceTypeId, ProblemID: ProblemID
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var list = msg.d;
            //cGrdDevice.Refresh();
            //  jAlert(msg.d);
            // $("#DiviceTyp").val("AL").change();
            if (msg.d != "Serial number already exists.") {
                $("#hdnEntityCode").val(EntityCode);
                cDiviceTyp.SetValue(1);
                $("#txtModel").val("");
                $("#txtDeviceNumber").val("");
                $("#txtWarranty").val("");
                $("#ddlProblem").val("");
                $("#txtRemarks").val("");
                // $("#ddlProblem").val("0").change();
                cddlProblem.SetValue(0);
                $("#hdnServiceHistoryValidation").val('0');
                $('#chkRemote').prop('checked', false);
                $('#chkCardAdaptor').prop('checked', false);
                document.getElementById("txtModel").disabled = false;
                // Mantis Issue 24413
                cselModel.SetEnabled(true);
                cselModel.SetValue(0);
                // End of Mantis Issue 24413
                cGrdDevice.PerformCallback();
            }

            if (msg.d == "Logout") {
                location.href = "../../OMS/SignOff.aspx";
            }

            jAlert(msg.d, "Alert", function () {
                setTimeout(function () {
                    $("#txtDeviceNumber").focus();
                }, 200);

            });
        }
    });


}

function ClickOnEdit(val) {
    $.ajax({
        type: "POST",
        url: "ReceiptChallan.aspx/EditData",
        data: JSON.stringify({ HiddenID: val }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var list = msg.d;
            $('#hdnGuid').val(val);

            //$("#DiviceTyp").val(msg.d.DeviceTypeId).change();
            cDiviceTyp.SetValue(msg.d.DeviceTypeId);
            $("#txtModel").val(msg.d.Model);
            $("#txtDeviceNumber").val(msg.d.DeviceNumber);
            $("#txtWarranty").val(msg.d.Warranty);
            //$("#ddlProblem").val(msg.d.ProblemID).change();
            cddlProblem.SetValue(msg.d.ProblemID);
            $("#txtRemarks").val(msg.d.Remarks);

            if (msg.d.Remote == "Yes") {
                $('#chkRemote').prop('checked', true);
            }
            else {
                $('#chkRemote').prop('checked', false);
            }

            if (msg.d.CardAdaptor == "Yes") {
                $('#chkCardAdaptor').prop('checked', true);
            }
            else {
                $('#chkCardAdaptor').prop('checked', false);
            }
            //$("#DiviceTyp").val(msg.d);
            //$("#ddlProblem").val(msg.d);



            cGrdDevice.PerformCallback();

        }
    });
}

function OnClickDelete(val) {
    jConfirm('Confirm Delete?', 'Alert', function (r) {
        if (r) {
            $.ajax({
                type: "POST",
                url: "ReceiptChallan.aspx/DeleteData",
                data: JSON.stringify({ HiddenID: val }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;
                    //cGrdDevice.Refresh();
                    jAlert(msg.d);
                    //  $("#HoliDayDetail").modal('toggle');
                    cGrdDevice.PerformCallback();

                }
            });
        }
        else {

        }
    });
}

var lastCRP = null;
function ddlEntryType_SelectedIndexChanged() {

    var ReceiptType = document.getElementById('ddlEntryType').value;

    $.ajax({
        type: "POST",
        url: "ReceiptChallan.aspx/bindScheme",
        data: JSON.stringify({ ReceiptType: ReceiptType }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            SetDataSourceOnComboBox(cCmbScheme, msg.d);
        }
    });
    // $("#ddlStatus").val(values);
}

function SetDataSourceOnComboBox(ControlObject, Source) {
    ControlObject.ClearItems();
    for (var count = 0; count < Source.length; count++) {
        ControlObject.AddItem(Source[count].Text, Source[count].Value);
    }
    ControlObject.SetSelectedIndex(0);
}


function CmbScheme_ValueChange() {

    var val = cCmbScheme.GetValue();
    // var val = GetObjectID('hdnSchemaID').value;

    $("#MandatoryBillNo").hide();

    if (val != " ") {

        //var NoSchemeTypedtl = GetObjectID('hdnSchemaID').value;
        var NoSchemeTypedtl = cCmbScheme.GetValue();
        var schemeID = NoSchemeTypedtl.toString().split('~')[0];
        $('#hdnSchemaID').val(schemeID);
        var schemetype = NoSchemeTypedtl.toString().split('~')[1];
        var schemelength = NoSchemeTypedtl.toString().split('~')[2];
        var branchID = (NoSchemeTypedtl.toString().split('~')[3] != null) ? NoSchemeTypedtl.toString().split('~')[3] : "";

        var fromdate = NoSchemeTypedtl.toString().split('~')[4];
        var todate = NoSchemeTypedtl.toString().split('~')[5];

        var dt = new Date();

        cFormDate.SetDate(dt);

        if (dt < new Date(fromdate)) {
            cFormDate.SetDate(new Date(fromdate));
        }

        if (dt > new Date(todate)) {
            cFormDate.SetDate(new Date(todate));
        }




        cFormDate.SetMinDate(new Date(fromdate));
        cFormDate.SetMaxDate(new Date(todate));

        if (branchID != "") {
            // document.getElementById('ddlBranch').value = branchID;
            cddlBranch.SetValue(branchID);
        }
        // GetInvoiceDetails();

        if (schemetype == '0') {
            $('#hdnSchemaType').val('0');
            document.getElementById('txtDocumentNumber').disabled = false;
            document.getElementById('txtDocumentNumber').value = "";
            document.getElementById("txtDocumentNumber").focus();
        }
        else if (schemetype == '1') {
            $('#hdnSchemaType').val('1');
            document.getElementById('txtDocumentNumber').disabled = true;
            document.getElementById('txtDocumentNumber').value = "Auto";
            cFormDate.Focus();
        }
        else if (schemetype == '2') {
            $('#hdnSchemaType').val('2');
            document.getElementById('txtDocumentNumber').disabled = true;
            document.getElementById('txtDocumentNumber').value = "Datewise";
        }
        else {
            document.getElementById('txtDocumentNumber').disabled = true;
            document.getElementById('txtDocumentNumber').value = "";
        }

    }
    else {
        document.getElementById('txtDocumentNumber').disabled = true;
        document.getElementById('txtDocumentNumber').value = "";
    }


}
function GetDateFormat(today) {
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
        //  today = dd + '-' + mm + '-' + yyyy;
        today = yyyy + '-' + mm + '-' + dd;
    }

    return today;
}


function SaveButtonClick(values) {

    LoadingPanel.Show();


    //if ($("#ddlEntryType").val() == "0") {
    //    jAlert("Please select Entry Type.");
    //    $("#ddlEntryType").focus();
    //    return
    //}
    var sendSMS = "No";
    if ($('#chkSendSMS').is(':checked') == true) {
        sendSMS = "Yes";
    }

    if (cCmbScheme.GetValue() == "0") {
        LoadingPanel.Hide();
        jAlert("Please select Numbering Scheme.");
        cCmbScheme.Focus();
        return
    }



    if ($("#txtDocumentNumber").val() == "") {
        LoadingPanel.Hide();
        $("#MandatoryBillNo").show();
        return
    }
    else {
        $("#MandatoryBillNo").hide();
    }

    if ($("#txtEntityCode").val() == "") {
        jAlert("Please enter Entity Code.", "Alert", function () {
            setTimeout(function () {
                LoadingPanel.Hide();
                $("#txtEntityCode").focus();
                return
            }, 200);
        });
        return
    }

    if ($("#txtNetworkName").val() == "") {
        jAlert("Please Enter Network Name.", "Alert", function () {
            setTimeout(function () {
                LoadingPanel.Hide();
                $("#txtNetworkName").focus();
                return
            }, 200);
        });
        return
    }

    if ($("#txtContactPerson").val() == "") {
        jAlert("Please Enter Contact Person.", "Alert", function () {
            setTimeout(function () {
                LoadingPanel.Hide();
                $("#txtContactPerson").focus();
                return
            }, 200);
        });
        return
    }

    if ($("#txtContactNo").val() == "") {
        jAlert("Please Enter Contact Number.", "Alert", function () {
            setTimeout(function () {
                LoadingPanel.Hide();
                $("#txtContactNo").focus();
                return
            }, 200);
        });
        return
    }



    var frmDate = GetDateFormat(cFormDate.GetValue());
    var Apply = {
        EntryType: $("#hdnEntryTypeID").val(),
        CmbScheme: cCmbScheme.GetValue(),
        DocumentNumber: $("#txtDocumentNumber").val(),
        branch: cddlBranch.GetValue(),
        date: frmDate,
        EntityCode: $("#txtEntityCode").val(),
        NetworkName: $("#txtNetworkName").val(),
        ContactPerson: $("#txtContactPerson").val(),
        Action: $("#Hidden_add_edit").val(),
        ReceiptChallan_ID: $("#hdnReceiptChalanID").val(),
        ContactNo: $("#txtContactNo").val(),
        sendSMS: sendSMS
    }

    $.ajax({
        type: "POST",
        url: "ReceiptChallan.aspx/save",
        data: "{apply:" + JSON.stringify(Apply) + "}",
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            console.log(response);
            if (response.d) {
                if (response.d == "Logout") {
                    location.href = "../../OMS/SignOff.aspx";
                }

                if (response.d.split('~')[0] == "true") {
                    var msgs = "Receipt Challan no. " + response.d.split('~')[2] + " generated successfully.";
                    if (response.d.split('~')[3] == "add") {
                        if ($("#hdnOnlinePrint").val() == "Yes") {
                            window.open("../../OMS/Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=ReceiptChallan~D&modulename=RECPTCHALLAN&id=" + response.d.split('~')[1], '_blank');
                        }
                    }

                    jAlert(msgs, "Alert", function () {
                        if (values == "new") {
                            LoadingPanel.Hide();
                            if ($("#hdnEntryTypeID").val() == "1") {
                                window.location.href = "ReceiptChallan.aspx?Key=ADD&mdl=T";
                                LoadingPanel.Hide();
                            }
                            else if ($("#hdnEntryTypeID").val() == "2") {
                                window.location.href = "ReceiptChallan.aspx?Key=ADD&mdl=C";
                                LoadingPanel.Hide();
                            }
                            else if ($("#hdnEntryTypeID").val() == "3") {
                                window.location.href = "ReceiptChallan.aspx?Key=ADD&mdl=W";
                                LoadingPanel.Hide();
                            }
                        }
                        else if (values == "Exit") {
                            window.location.href = "ReceiptChallanList.aspx";
                            LoadingPanel.Hide();
                        }

                    });
                }
                else {
                    jAlert(response.d.split('~')[0]);
                    $("#txtDeviceNumber").focus();
                    LoadingPanel.Hide();
                    return
                }
            }

        },
        error: function (response) {

            console.log(response);
        }

    });
}

function SaveExitButtonClick() {

}

function cancel() {
    if ($("#hdnEntryTypeID").val() == "1") {
        window.location.href = "ReceiptChallan.aspx?Key=ADD&mdl=T";
    }
    else if ($("#hdnEntryTypeID").val() == "2") {
        window.location.href = "ReceiptChallan.aspx?Key=ADD&mdl=C";
    }
    else if ($("#hdnEntryTypeID").val() == "3") {
        window.location.href = "ReceiptChallan.aspx?Key=ADD&mdl=W";
    }
}

function ShowRepeatHistory() {

    $.ajax({
        type: "POST",
        url: "ReceiptChallan.aspx/ServiceEntryCountShow",
        data: JSON.stringify({ model: $("#txtModel").val(), DeviceNumber: $("#txtDeviceNumber").val() }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var list = msg.d;
            $("#idRepaired").text(msg.d.Repaired);
            $("#idExchange").text(msg.d.Exchanged);
        }
    });

    $("#RepeatHistory").modal('toggle');
}

$(document).ready(function () {
    if ($('body').hasClass('mini-navbar')) {
        var windowWidth = $(window).width();
        var cntWidth = windowWidth - 90;
        cGrdDevice.SetWidth(cntWidth);
    } else {
        var windowWidth = $(window).width();
        var cntWidth = windowWidth - 220;
        cGrdDevice.SetWidth(cntWidth);
    }

    $('.navbar-minimalize').click(function () {
        if ($('body').hasClass('mini-navbar')) {
            var windowWidth = $(window).width();
            var cntWidth = windowWidth - 220;
            cGrdDevice.SetWidth(cntWidth);
        } else {
            var windowWidth = $(window).width();
            var cntWidth = windowWidth - 90;
            cGrdDevice.SetWidth(cntWidth);
        }

    });
});

function BindServiceEntryHistory() {
    $("#hdnServiceHistoryValidation").val('1');
    var status = "<table class='table table-striped table-bordered tableStyle' id='dataTable'>";
    status = status + " <thead><tr>";
    status = status + " <th>Entity Code</th><th>Ref. Receipt No.</th>";
    status = status + " <th>Service Action</th><th>Remarks</th><th>Billable</th></tr></thead>";
    status = status + " </table>";
    $('#DivHistoryTable').html(status);
    setTimeout(function () {
        $.ajax({
            type: "POST",
            url: "ReceiptChallan.aspx/ServiceEntryHistoryList",
            data: JSON.stringify({ model: $("#txtModel").val(), DeviceNumber: $("#txtDeviceNumber").val() }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {

                $('#dataTable').DataTable({
                    //scrollX: true,
                    //fixedColumns: {
                    //    rightColumns: 2
                    //},
                    data: msg.d,
                    columns: [
                       { 'data': 'EntityCode' },
                       { 'data': 'ReceiptNo' },
                       { 'data': 'ServiceAction' },
                       { 'data': 'Remarks' },
                       { 'data': 'Billable' }
                    ],

                    error: function (error) {
                        alert(error);
                    }
                });
            }
        });
    }, 1000);
}

function Devicenumber_change() {
    var isValid = true;
    if ($("#txtDeviceNumber").val() != "") {
        $("#hdnServiceHistoryValidation").val('0');
        $.ajax({
            type: "POST",
            url: "ReceiptChallan.aspx/GeSerialValidation",
            data: JSON.stringify({ DeviceNumber: $("#txtDeviceNumber").val() }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var list = msg.d;
                if (msg.d != "DE" && msg.d != "") {
                    isValid = false;
                    jAlert("Serial No is already in service.", "Alert", function () {
                        setTimeout(function () {
                            $("#txtDeviceNumber").val("");
                            $("#txtDeviceNumber").focus();
                            return;
                        }, 200);
                    });
                    return;
                }
                else {
                    GetDeviceDetails();
                }
            }
        });

    }
}

function GetDeviceDetails() {
    var gridRow = cGrdDevice.GetVisibleRowsOnPage();
    $.ajax({
        type: "POST",
        url: '/SRVFileuploadDelivery/DeviceNumberDetails',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ DeviceNumber: $("#txtDeviceNumber").val() }),
        success: function (response) {
            // Rev 24410
            //if (response.EntityCode != "" || response.EntityCode != null) {
            if (response.EntityCode != null && response.EntityCode != "") {
                // End of Rev 24410
                if ($("#txtEntityCode").val() != "" && gridRow > 0) {
                    if ($("#hdnEntityCode").val() != response.EntityCode) {
                        jAlert("This serial number is belongs to another Entity Code.", "Alert", function () {
                            setTimeout(function () {
                                $("#txtDeviceNumber").val("");
                                $("#txtDeviceNumber").focus();
                            }, 200);
                        });
                    }
                    else {
                        //$("#txtEntityCode").val(response.EntityCode);
                        //$("#txtNetworkName").val(response.NetworkName);
                        //$("#txtContactPerson").val(response.ContactPerson);
                        //$("#txtContactNo").val(response.ContactNumber);
                        $("#txtModel").val(response.Model);

                        if (response.EntityCode != "") {
                            document.getElementById("txtModel").disabled = true;
                            // Mantis Issue 24413
                            cselModel.SetEnabled(false);
                            cselModel.SetValue(0);
                            // End of Mantis Issue 24413
                        }
                        else {
                            document.getElementById("txtModel").disabled = false;
                            // Mantis Issue 24413
                            cselModel.SetEnabled(true);
                            cselModel.SetValue(0);
                            // End of Mantis Issue 24413
                        }
                        getWarrantydt();
                    }
                }
                else {
                    if (response.EntityCode == null) {
                        if ($("#hdnIsEntityInformationEdiableinReceiptChallan").val() == "Yes") {
                            jConfirm('Entity information is not available. Do you wish to add manually?', 'Alert', function (r) {
                                if (r) {
                                    $("#txtEntityCode").prop('disabled', false);
                                    $("#txtNetworkName").prop('disabled', false);
                                    $("#txtContactPerson").prop('disabled', false);
                                    $("#txtContactNo").prop('disabled', false);
                                    $("#txtModel").prop('disabled', false);
                                    // Mantis Issue 24413
                                    cselModel.SetEnabled(true);
                                    cselModel.SetValue(0);
                                    // End of Mantis Issue 24413
                                }
                                else {
                                    $("#txtEntityCode").prop('disabled', true);
                                    $("#txtNetworkName").prop('disabled', true);
                                    $("#txtContactPerson").prop('disabled', true);
                                    $("#txtContactNo").prop('disabled', true);
                                    $("#txtModel").prop('disabled', true);
                                    // Mantis Issue 24413
                                    cselModel.SetEnabled(false);
                                    cselModel.SetValue(0);
                                    // End of Mantis Issue 24413
                                }
                            });
                        }
                    }
                    else {
                        $("#txtEntityCode").val(response.EntityCode);
                        if (response.EntityCode != "") {
                            document.getElementById("txtEntityCode").disabled = true;
                        }

                        $("#txtNetworkName").val(response.NetworkName);
                        if (response.EntityCode != "") {
                            document.getElementById("txtNetworkName").disabled = true;
                        }

                        $("#txtContactPerson").val(response.ContactPerson);
                        if (response.EntityCode != "") {
                            document.getElementById("txtContactPerson").disabled = true;
                        }

                        $("#txtContactNo").val(response.ContactNumber);
                        if (response.EntityCode != "") {
                            document.getElementById("txtContactNo").disabled = true;
                        }

                        $("#txtModel").val(response.Model);
                        if (response.EntityCode != "") {
                            document.getElementById("txtModel").disabled = true;
                            // Mantis Issue 24413
                            cselModel.SetEnabled(false);
                            cselModel.SetValue(0);
                            // End of Mantis Issue 24413
                        }
                        else {
                            document.getElementById("txtModel").disabled = false;
                            // Mantis Issue 24413
                            cselModel.SetEnabled(true);
                            cselModel.SetValue(0);
                            // End of Mantis Issue 24413
                        }

                        getWarrantydt();
                    }
                }
            }
            else {
                if ($("#hdnIsEntityInformationEdiableinReceiptChallan").val() == "Yes") {
                    jConfirm('Entity information is not available. Do you wish to add manually?', 'Alert', function (r) {
                        if (r) {
                            $("#txtEntityCode").prop('disabled', false);
                            $("#txtNetworkName").prop('disabled', false);
                            $("#txtContactPerson").prop('disabled', false);
                            $("#txtContactNo").prop('disabled', false);
                            $("#txtModel").prop('disabled', false);
                            // Mantis Issue 24413
                            cselModel.SetEnabled(true);
                            cselModel.SetValue(0);
                            // End of Mantis Issue 24413
                        }
                        else {
                            $("#txtEntityCode").prop('disabled', true);
                            $("#txtNetworkName").prop('disabled', true);
                            $("#txtContactPerson").prop('disabled', true);
                            $("#txtContactNo").prop('disabled', true);
                            $("#txtModel").prop('disabled', true);
                            // Mantis Issue 24413
                            cselModel.SetEnabled(false);
                            cselModel.SetValue(0);
                            // End of Mantis Issue 24413
                        }
                    });
                }
                else {
                    jAlert("Please enter valid Serial No.", "Alert", function () {
                        setTimeout(function () {
                            $("#txtDeviceNumber").val("");
                            $("#txtDeviceNumber").focus();
                        }, 200);
                    });
                }
            }
        },
        error: function (response) {
            jAlert(response);
        }
    });
}

function getWarrantydt() {

    $.ajax({
        type: "POST",
        url: "ReceiptChallan.aspx/GetWarrantyDate",
        data: JSON.stringify({ DeviceNumber: $("#txtDeviceNumber").val() }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var list = msg.d;
            // $('#dtwrnty').datepicker("setDate", new Date(msg.d));
            $("#txtWarranty").val(msg.d);
        }
    });
}

// Mantis Issue 24412
function get_NetworkName()
{
    $.ajax({
        type: "POST",
        url: "ReceiptChallan.aspx/GetNetworkName",
        data: JSON.stringify({ EntityCode: $("#txtEntityCode").val() }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var list = msg.d;
            $("#txtNetworkName").val(msg.d);
        }
    });
}
// End of Mantis Issue 24412

// Mantis Issue 24413
function selModel_change()
{
    var selModel = cselModel.GetText();
    if (selModel != "") {
        $("#txtModel").val(selModel);
        cselModel.SetValue(0);
    }
    
}
// End of Mantis Issue 24413