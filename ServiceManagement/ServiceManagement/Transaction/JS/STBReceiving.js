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

    cChallanDate.SetDate(new Date());

    if ($("#Hidden_add_edit").val() == "Edit") {
        $("#btnSaveNew").addClass('hide');

    }
    else if ($("#Hidden_add_edit").val() == "View") {
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
});

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

function AddDevice() {

    var ChallanDate = cChallanDate.GetValue();
    var DeviceType = cDiviceTyp.GetText();
    var ChallanNumber = $("#txtChallanNumber").val();
    var LCOCode = $("#txtLCOCode").val();
    var LCOName = $("#txtLCOName").val();
    var Remarks = $("#txtRemarks").val();
    var MSO = cddlMSO.GetText();
    var MSOID = cddlMSO.GetValue();

    var STBModel = cSTBModel.GetText();
    var STBModelID = cSTBModel.GetValue();

    var DeviceTypeId = cDiviceTyp.GetValue();
    var Quantity = ctxtQuantity.GetText();

    var LocationId = cddlLocation.GetValue();
    var Location = cddlLocation.GetText();

    var Gu_id = $('#hdnGuid').val();

    var suc = true;

    if (cCmbScheme.GetValue() == "0") {
        jAlert("Please select Numbering Scheme.");
        cCmbScheme.Focus();
        suc = false;
        return
    }

    if (ChallanDate == '01-01-0100' || ChallanDate == "") {
        jAlert("Please select proper challan date.");
        cChallanDate.Focus();
        suc = false;
        return
    }

    if (ChallanNumber == "") {
        jAlert("Please enter challan number.");
        $("#txtChallanNumber").focus();
        suc = false;
        return
    }

    if (LCOCode == "") {
        jAlert("Please enter LCO Code.");
        $("#txtLCOCode").focus();
        suc = false;
        return
    }

    if (LCOName == "") {
        jAlert("Please enter LCO Name.");
        $("#txtLCOName").focus();
        suc = false;
        return
    }

    if (MSOID == "0") {
        jAlert("Please select MSO.");
        cddlMSO.Focus();
        suc = false;
        return
    }

    if (STBModelID == "0") {
        jAlert("Please enter STB Model.");
        cSTBModel.Focus();
        suc = false;
        return
    }

    if (DeviceTypeId == "0") {
        jAlert("Please select Type.");
        cDiviceTyp.Focus();
        suc = false;
        return
    }

    if (Quantity <= 0) {
        jAlert("Please enter Quantity.");
        ctxtQuantity.Focus();
        suc = false;
        return
    }


    $.ajax({
        type: "POST",
        url: "stbAdd.aspx/AddData",
        data: JSON.stringify({
            ChallanDate: ChallanDate, DeviceType: DeviceType, ChallanNumber: ChallanNumber, LCOCode: LCOCode,
            LCOName: LCOName, Remarks: Remarks, MSO: MSO, Quantity: Quantity, Guids: Gu_id,
            Location: Location, DeviceTypeId: DeviceTypeId, LocationId: LocationId, MSOId: MSOID, STBModelID: STBModelID, STBModel: STBModel
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var list = msg.d;

            if (msg.d != "Serial number already exists.") {
                cChallanDate.SetDate(new Date());
                cDiviceTyp.SetValue(0);
                $("#txtChallanNumber").val('');
                $("#txtLCOCode").val('');
                $("#txtLCOName").val('');
                $("#txtRemarks").val('');
                cddlMSO.SetValue(0);
                ctxtQuantity.SetValue(0);
                cSTBModel.SetValue(0);
                cChallanDate.Focus();
                $('#hdnGuid').val('');
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

function OnClickDelete(val) {
    jConfirm('Confirm Delete?', 'Alert', function (r) {
        if (r) {
            $.ajax({
                type: "POST",
                url: "stbAdd.aspx/DeleteData",
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

function ClickOnEdit(val) {
    $.ajax({
        type: "POST",
        url: "stbAdd.aspx/EditData",
        data: JSON.stringify({ HiddenID: val }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var list = msg.d;
            $('#hdnGuid').val(val);

            cChallanDate.SetDate(new Date(parseInt(msg.d.ChallanDate.substr(6))));
            //cChallanDate.SetDate(new Date(parseInt(msg.d.ChallanDate)));
            cDiviceTyp.SetValue(msg.d.DeviceTypeId);
            $("#txtChallanNumber").val(msg.d.ChallanNumber);
            $("#txtLCOCode").val(msg.d.LCOCode);
            $("#txtLCOName").val(msg.d.LCOName);
            $("#txtRemarks").val(msg.d.Remarks);
            cddlMSO.SetValue(msg.d.MSOId);
            cSTBModel.SetValue(msg.d.STBModelID);
            ctxtQuantity.SetValue(msg.d.Quantity);
            cGrdDevice.PerformCallback();

            cGrdDevice.PerformCallback();
        }
    });
}

function Devicenumber_change() {
    var gridRow = cGrdDevice.GetVisibleRowsOnPage();
    $.ajax({
        type: "POST",
        url: '/SRVFileuploadDelivery/entity_codeDetails',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ entity_code: $("#txtLCOCode").val() }),
        success: function (response) {
            if (response.EntityCode != "" || response.EntityCode != null) {
                $("#txtLCOName").val(response.NetworkName);
                if (response.EntityCode != "") {
                    document.getElementById("txtLCOName").disabled = true;
                }
            }
            else {
                jAlert("Please enter valid LOC Code.", "Alert", function () {
                    setTimeout(function () {
                        $("#txtDeviceNumber").val("");
                        $("#txtDeviceNumber").focus();
                    }, 200);
                });
            }
        },
        error: function (response) {
            jAlert(response);
        }
    });
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


    var frmDate = GetDateFormat(cFormDate.GetValue());
    var Apply = {
        CmbScheme: cCmbScheme.GetValue(),
        DocumentNumber: $("#txtDocumentNumber").val(),
        branch: cddlBranch.GetValue(),
        date: frmDate,
        Action: $("#Hidden_add_edit").val(),
        STBReceived_ID: $("#hdnSTBReceivedID").val(),
        Remarks: $("#txtHeaderRemarks").val()
    }

    $.ajax({
        type: "POST",
        url: "stbAdd.aspx/save",
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
                    var msgs = "";
                    if ($("#Hidden_add_edit").val() != "Edit") {
                        msgs = "STB Receiving no. " + response.d.split('~')[2] + " generated successfully.";
                    }
                    else {
                        msgs = "STB Receiving no. " + response.d.split('~')[2] + " updated successfully.";
                    }
                    //if (response.d.split('~')[3] == "add") {
                    //    if ($("#hdnOnlinePrint").val() == "Yes") {
                    //        window.open("../../OMS/Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=ReceiptChallan~D&modulename=RECPTCHALLAN&id=" + response.d.split('~')[1], '_blank');
                    //    }
                    //}

                    jAlert(msgs, "Alert", function () {
                        if (values == "new") {
                            LoadingPanel.Hide();
                            window.location.href = "stbAdd.aspx?Key=ADD";
                            LoadingPanel.Hide();
                        }
                        else if (values == "Exit") {
                            window.location.href = "stbList.aspx";
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

function cancel() {
    window.location.href = "stbAdd.aspx?Key=ADD";
}