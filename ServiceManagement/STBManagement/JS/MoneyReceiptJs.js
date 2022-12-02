
$(document).ready(function () {
    //$('.js-example-basic-single').select2();

    //$('#filterToggle').hide();

    //$('.togglerSlide').click(function () {
    //    $('#filterToggle').slideDown({
    //        duration: 200,
    //        easing: "easeOutQuad"
    //    });
    //    $(this).hide()
    //});
    //$('.togglerSlidecut').click(function () {
    //    $('#filterToggle').slideUp({
    //        duration: 200,
    //        easing: "easeOutQuad"
    //    });
    //    $('.togglerSlide').show();
    //});
    //$('#dataTable').DataTable({
    //    scrollX: true,
    //    fixedColumns: {
    //        rightColumns: 2
    //    }
    //});
    //$('#dataTable2').DataTable({
    //});
    //$('#dataTable3').DataTable({
    //});
    //$('[data-toggle="tooltip"]').tooltip();

    //$(".date").datepicker({
    //    autoclose: true,
    //    todayHighlight: true,
    //    format: 'dd-mm-yyyy'
    //}).datepicker('update', new Date());

   
    document.getElementById('txtDocumentNumber').disabled = true;

    if ($("#hdAddEdit").val() == "edit") {
        $("#btnSaveNew").addClass('hide');
        if ($("#hdnIsCancel").val() == "True") {
            $("#DivCancel_Status").removeClass('hide');
            $("#DivCancelDetails").removeClass('hide');
            $("#lblCancelStatus").text("Cancellation In Progress");
            $("#DivCancelData").removeClass('hide');
            $("#btnSaveExit").addClass('hide');
        }
        else {
            $("#DivCancel_Status").addClass('hide');
            $("#DivCancelDetails").addClass('hide');
            $("#lblCancelStatus").text("");
            $("#DivCancelData").addClass('hide');
            $("#btnSaveExit").removeClass('hide');
        }

        $("#divPrintCount").removeClass('hide');

    }
    else if ($("#hdAddEdit").val() == "view") {
        $("#btnSaveNew").addClass('hide');
        $("#btnSaveExit").addClass('hide');
        $("#btnAdd").addClass('hide');
        if ($("#hdnIsCancel").val() == "True") {
            $("#DivCancel_Status").removeClass('hide');
            $("#DivCancelDetails").removeClass('hide');
            $("#lblCancelStatus").text("Cancellation In Progress");
            $("#DivCancelData").removeClass('hide');
        }
        else {
            $("#DivCancel_Status").addClass('hide');
            $("#DivCancelDetails").addClass('hide');
            $("#lblCancelStatus").text("");
            $("#DivCancelData").addClass('hide');
        }
        $("#divPrintCount").removeClass('hide');
    }
    else {
        //cFormDate.SetDate(new Date());
        $("#btnSaveNew").removeClass('hide');
        $("#btnSaveExit").removeClass('hide');
        $("#btnAdd").removeClass('hide');

        $("#DivCancelDetails").addClass('hide');
        $("#DivCancelData").addClass('hide');
    }
    cddlBranch.SetEnabled(false);
    cFormDate.SetEnabled(false);
});

function ddlModeType_SelectedIndexChanged() {

    ctxtAmount.SetText(0);
    $("#txtRefNo").val("");
    $("#txtChequeNo").val("");
    cddlBank.SetValue(0);
    $("#txtBranch").val("");
    $("#txtRemarks").val("");
    cdtChequeDate.Clear();

    var ModeType = document.getElementById('ddlModeType').value;
    if (ModeType == "Cash") {
        document.getElementById("txtRefNo").disabled = true;
        document.getElementById("txtChequeNo").disabled = true;
        cdtChequeDate.SetEnabled(false);
        cddlBank.SetEnabled(false);
        document.getElementById("txtBranch").disabled = true;

    }
    else if (ModeType == "Cheque") {
        document.getElementById("txtRefNo").disabled = true;
        document.getElementById("txtChequeNo").disabled = false;
        cdtChequeDate.SetEnabled(true);
        cddlBank.SetEnabled(true);
        document.getElementById("txtBranch").disabled = false;
        cdtChequeDate.SetDate(new Date());
    }
    else if (ModeType == "NEFT") {
        document.getElementById("txtRefNo").disabled = false;
        document.getElementById("txtChequeNo").disabled = true;
        cdtChequeDate.SetEnabled(false);
        cddlBank.SetEnabled(true);
        document.getElementById("txtBranch").disabled = false;
    }
    else if (ModeType == "RTGS") {
        document.getElementById("txtRefNo").disabled = false;
        document.getElementById("txtChequeNo").disabled = true;
        cdtChequeDate.SetEnabled(false);
        cddlBank.SetEnabled(true);
        document.getElementById("txtBranch").disabled = false;
    }
    else if (ModeType == "OtherOnline") {
        document.getElementById("txtRefNo").disabled = false;
        document.getElementById("txtChequeNo").disabled = true;
        cdtChequeDate.SetEnabled(false);
        cddlBank.SetEnabled(true);
        document.getElementById("txtBranch").disabled = false;
    }


}

function Devicenumber_change() {
    //var gridRow = cGrdDevice.GetVisibleRowsOnPage();
    if ($("#txtEntityCode").val() != "") {

        $.ajax({
            type: "POST",
            url: "ReceiptAdd.aspx/FetchEntityFromMaster",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ entity_code: $("#txtEntityCode").val() }),
            async: false,
            success: function (msg) {
                var Masterresponse = msg.d;
                var IsActive = "";
                var EntityCode = Masterresponse.split('~')[0];
                var EntityName = Masterresponse.split('~')[1];
                var ContactName = Masterresponse.split('~')[2];
                var ContactNo = Masterresponse.split('~')[3];
                if (Masterresponse != "")
                    IsActive = Masterresponse.split('~')[4];


                if (Masterresponse == "") {

                    $.ajax({
                        type: "POST",
                        url: '/SRVFileuploadDelivery/entity_codeDetails',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: JSON.stringify({ entity_code: $("#txtEntityCode").val() }),
                        async: false,
                        success: function (response) {
                            if (response.EntityCode != "" && response.EntityCode != null) {
                                $("#txtEntityCode").val(response.EntityCode);
                                $("#txtNetworkName").val(response.NetworkName);
                                $("#txtContactPerson").val(response.ContactPerson);
                                $("#txtContactNo").val(response.ContactNumber);

                            }
                            else {
                                if ($("#hdnIsEntityInformationEditableInWRMR").val() == "Yes") {
                                    jConfirm('Entity information is not available. Do you wish to add manually?', 'Alert', function (r) {
                                        if (r) {
                                            $("#txtNetworkName").prop('disabled', false);
                                            $("#txtContactPerson").prop('disabled', false);
                                            $("#txtContactNo").prop('disabled', false);
                                        }
                                        else {
                                            $("#txtNetworkName").prop('disabled', true);
                                            $("#txtContactPerson").prop('disabled', true);
                                            $("#txtContactNo").prop('disabled', true);
                                        }
                                    });
                                }
                                else {
                                    jAlert("Please enter valid Entity Code.", "Alert", function () {
                                        setTimeout(function () {
                                            $("#txtNetworkName").prop('disabled', true);
                                            $("#txtContactPerson").prop('disabled', true);
                                            $("#txtContactNo").prop('disabled', true);
                                            $("#txtEntityCode").val("");
                                            $("#txtEntityCode").focus();
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
                else {
                    if (IsActive != "True" && IsActive != "") {
                        jAlert("Entity is not Active. Speak to Administrator.");
                    }
                    else if (IsActive == "True") {
                        $.ajax({
                            type: "POST",
                            url: '/SRVFileuploadDelivery/entity_codeDetails',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: JSON.stringify({ entity_code: $("#txtEntityCode").val() }),
                            success: function (response) {
                                if (response.EntityCode != "" && response.EntityCode != null) {
                                    $("#txtEntityCode").val(response.EntityCode);
                                    $("#txtNetworkName").val(response.NetworkName);
                                    $("#txtContactPerson").val(response.ContactPerson);
                                    $("#txtContactNo").val(response.ContactNumber);

                                }
                                else {
                                    if ($("#hdnIsEntityInformationEditableInWRMR").val() == "Yes") {
                                        jConfirm('Entity information is not available. Do you wish to add manually?', 'Alert', function (r) {
                                            if (r) {
                                                $("#txtNetworkName").prop('disabled', false);
                                                $("#txtContactPerson").prop('disabled', false);
                                                $("#txtContactNo").prop('disabled', false);
                                            }
                                            else {
                                                $("#txtNetworkName").prop('disabled', true);
                                                $("#txtContactPerson").prop('disabled', true);
                                                $("#txtContactNo").prop('disabled', true);
                                            }
                                        });
                                    }
                                    else {
                                        jAlert("Please enter valid Entity Code Code.", "Alert", function () {
                                            setTimeout(function () {
                                                $("#txtNetworkName").prop('disabled', true);
                                                $("#txtContactPerson").prop('disabled', true);
                                                $("#txtContactNo").prop('disabled', true);
                                                $("#txtEntityCode").val("");
                                                $("#txtEntityCode").focus();
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
                }
            }
        });

    }
}

function grid_EndCallBack(s, e) {
    //var url = 'erp_addHoliday.aspx';
    //window.location.href = url;
}

function AddDevice() {

    var MoneyReceiptID = $("#hdnMoneyReceiptID").val();
    var Mode = $("#ddlModeType").val();
    var Amount = ctxtAmount.GetValue();
    var RefNo = $("#txtRefNo").val();
    var ChequeNo = $("#txtChequeNo").val();
    var Cheque_Date = "";
    var BankID = cddlBank.GetValue();
    var BankName = cddlBank.GetText();
    var Branch = $("#txtBranch").val();
    var Remark = $("#txtRemarks").val();
    var Gu_id = $('#hdnGuid').val();

    if (cdtChequeDate.GetValue() != null) {
        Cheque_Date = GetDateFormat(cdtChequeDate.GetValue());
    }
    var ChequeDate = Cheque_Date;

    var suc = true;
    //if (Mode == "0") {
    //    $("#MandatoryModeType").show();
    //    suc = false;
    //    return
    //}
    if ($("#ddlModeType").val() == "0") {
        jAlert("Please select Mode.")
        $("#ddlModeType").focus();
        suc = false;
        return
    }
    if (ctxtAmount.GetValue() <= 0) {
        jAlert("Please enter Amount.")
        ctxtAmount.Focus();
        suc = false;
        return
    }

    if (Mode == "Cheque") {
        if ($("#txtChequeNo").val() == "") {
            jAlert("Please enter Cheque No.")
            $("#txtChequeNo").focus();
            suc = false;
            return
        }
        if (ChequeDate == "") {
            jAlert("Please enter Cheque Date")
            cdtChequeDate.Focus();
            suc = false;
            return
        }
        if (BankID == "0") {
            jAlert("Please Select Bank")
            cddlBank.Focus();
            suc = false;
            return
        }
    }

    if (BankID == "0") {
        BankID = "";
        BankName = "";
    }

    $.ajax({
        type: "POST",
        url: "ReceiptAdd.aspx/AddData",
        data: JSON.stringify({
            Mode: Mode,
            Amount: Amount,
            RefNo: RefNo,
            ChequeNo: ChequeNo,
            ChequeDate: ChequeDate,
            BankID: BankID,
            BankName: BankName,
            Branch: Branch,
            Remark: Remark,
            Guids: Gu_id
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var list = msg.d;
            if (msg.d != "Serial number already exists.") {
                $("#ddlModeType").val("0");
                ctxtAmount.SetText(0);
                $("#txtRefNo").val("");
                $("#txtChequeNo").val("");
                //cdtChequeDate.SetValue("");
                cdtChequeDate.SetText('');
                cddlBank.SetValue(0);
                $("#txtBranch").val("");
                $("#txtRemarks").val("");
                cGrdDevice.PerformCallback();
            }

            if (msg.d == "Logout") {
                location.href = "../../OMS/SignOff.aspx";
            }

            jAlert(msg.d, "Alert", function () {
                setTimeout(function () {
                    $("#ddlModeType").focus();
                }, 200);

            });
        }
    });


}

function ClickOnEdit(val) {
    $.ajax({
        type: "POST",
        url: "ReceiptAdd.aspx/EditData",
        data: JSON.stringify({ HiddenID: val }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var list = msg.d;
            $('#hdnGuid').val(val);

            var ModeType = msg.d.Payment_Mode;
            $("#ddlModeType").val(msg.d.Payment_Mode);
            ctxtAmount.SetValue(msg.d.Amount);
            // $("#txtAmount").val(msg.d.Amount);
            $("#txtRefNo").val(msg.d.RefNo);
            $("#txtChequeNo").val(msg.d.ChequeNo);
            if (msg.d.ChequeDate != "") {
                cdtChequeDate.SetDate(new Date(msg.d.ChequeDate));
            }

            cddlBank.SetValue(msg.d.BankID);
            $("#txtBranch").val(msg.d.BranchName);
            $("#txtRemarks").val(msg.d.Remark);


            if (ModeType == "Cash") {
                document.getElementById("txtRefNo").disabled = true;
                document.getElementById("txtChequeNo").disabled = true;
                cdtChequeDate.SetEnabled(false);
                cddlBank.SetEnabled(false);
                document.getElementById("txtBranch").disabled = true;

            }
            else if (ModeType == "Cheque") {
                document.getElementById("txtRefNo").disabled = false;
                document.getElementById("txtChequeNo").disabled = false;
                cdtChequeDate.SetEnabled(true);
                cddlBank.SetEnabled(true);
                document.getElementById("txtBranch").disabled = false;
            }
            else if (ModeType == "NEFT") {
                document.getElementById("txtRefNo").disabled = false;
                document.getElementById("txtChequeNo").disabled = true;
                cdtChequeDate.SetEnabled(false);
                cddlBank.SetEnabled(true);
                document.getElementById("txtBranch").disabled = false;
            }
            else if (ModeType == "RTGS") {
                document.getElementById("txtRefNo").disabled = false;
                document.getElementById("txtChequeNo").disabled = true;
                cdtChequeDate.SetEnabled(false);
                cddlBank.SetEnabled(true);
                document.getElementById("txtBranch").disabled = false;
            }
            else if (ModeType == "OtherOnline") {
                document.getElementById("txtRefNo").disabled = false;
                document.getElementById("txtChequeNo").disabled = true;
                cdtChequeDate.SetEnabled(false);
                cddlBank.SetEnabled(true);
                document.getElementById("txtBranch").disabled = false;
            }

            cGrdDevice.PerformCallback();

        }
    });
}

function OnClickDelete(val) {
    jConfirm('Confirm Delete?', 'Alert', function (r) {
        if (r) {
            $.ajax({
                type: "POST",
                url: "ReceiptAdd.aspx/DeleteData",
                data: JSON.stringify({ HiddenID: val }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;
                    jAlert(msg.d);
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

       // cFormDate.SetDate(dt);

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
    WorkingRoster();
    if (rosterstatus) {
        LoadingPanel.Show();
        if ($("#hdAddEdit").val() == "Add") {
            if (cCmbScheme.GetValue() == "0") {
                LoadingPanel.Hide();
                jAlert("Please select Numbering Scheme.");
                cCmbScheme.Focus();
                return
            }
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

        if (cDiviceTyp.GetValue() == "0") {
            jAlert("Please Select Type.", "Alert", function () {
                setTimeout(function () {
                    LoadingPanel.Hide();
                    cDiviceTyp.Focus();
                    return
                }, 200);
            });
            return
        }

        if ($('#chkTnC').is(':checked') == false) {
            jAlert("Please check T&C.", "Alert", function () {
                setTimeout(function () {
                    LoadingPanel.Hide();
                    $("#chkTnC").focus();
                    return
                }, 200);
            });
            return
        }
        var frmDate = GetDateFormat(cFormDate.GetValue());
        //var ChequeDate = GetDateFormat(cdtChequeDate.GetValue());
        var Apply = {
            Action: $("#hdAddEdit").val(),
            MoneyReceipt_ID: $("#hdnMoneyReceiptID").val(),
            CmbScheme: cCmbScheme.GetValue(),
            DocumentNumber: $("#txtDocumentNumber").val(),
            branch: cddlBranch.GetValue(),
            date: frmDate,
            EntityCode: $("#txtEntityCode").val(),
            NetworkName: $("#txtNetworkName").val(),
            ContactPerson: $("#txtContactPerson").val(),
            ContactNo: $("#txtContactNo").val(),
            Type: cDiviceTyp.GetValue()

        }

        $.ajax({
            type: "POST",
            url: "ReceiptAdd.aspx/save",
            data: "{apply:" + JSON.stringify(Apply) + "}",
            async: true,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {

                if (response.d) {
                    if (response.d == "Logout") {
                        location.href = "../../OMS/SignOff.aspx";
                    }

                    if (response.d.split('~')[0] == "true") {
                        var msgs = "";
                        if ($("#hdAddEdit").val() == "Add") {
                            msgs = "Money Receipt " + response.d.split('~')[2] + " generated successfully.";
                            if ($("#hdnOnlinePrint").val() == "Yes") {
                                if ($("#hdAddEdit").val() == "Add") {
                                    $.ajax({
                                        type: "POST",
                                        url: 'ReceiptAdd.aspx/UpdatePrintCount',
                                        contentType: "application/json; charset=utf-8",
                                        dataType: "json",
                                        async: false,
                                        data: JSON.stringify({ WalletRechargeID: response.d.split('~')[1] }),
                                        success: function (response) {

                                        },
                                        error: function (response) {
                                            //jAlert(response);
                                        }
                                    });
                                }
                                window.open("../../OMS/Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=MoneyReceipt~D&modulename=MoneyReceipt&id=" + response.d.split('~')[1] + "&PrintOption=1", '_blank');
                            }
                        }
                        else {
                            msgs = "Money Receipt " + response.d.split('~')[2] + " updated successfully.";
                        }
                        jAlert(msgs, "Alert", function () {
                            if (values == "new") {
                                LoadingPanel.Hide();
                                window.location.href = "ReceiptAdd.aspx?Key=Add";

                            }
                            else if (values == "Exit") {
                                window.location.href = "index.aspx";
                                LoadingPanel.Hide();
                            }

                        });
                    }
                    else {
                        jAlert(response.d.split('~')[0]);

                        document.getElementById("ddlModeType").focus();
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
    else {
        //jAlert("Working period is over. Try in next working period.", "Alert", function () {
        //    window.location.href = "index.aspx";
        //});
        $("#divPopHead").removeClass('hide');
    }
}

function WorkingRosterClick() {
    window.location.href = "index.aspx";
}

function SaveExitButtonClick() {

}

function cancel() {
    window.location.href = "index.aspx";
}


$(document).ready(function () {
    setTimeout(function () {
        if ($('body').hasClass('mini-navbar')) {
            var windowWidth = $(window).width();
            var cntWidth = windowWidth - 90;
            cGrdDevice.SetWidth(cntWidth);
        } else {
            var windowWidth = $(window).width();
            var cntWidth = windowWidth - 217;
            cGrdDevice.SetWidth(cntWidth);
        }
    }, 300);
    $('.navbar-minimalize').click(function () {
        if ($('body').hasClass('mini-navbar')) {
            var windowWidth = $(window).width();
            var cntWidth = windowWidth - 217;
            cGrdDevice.SetWidth(cntWidth);
        } else {
            var windowWidth = $(window).width();
            var cntWidth = windowWidth - 90;
            cGrdDevice.SetWidth(cntWidth);
        }

    });
});


function GetDeviceDetails() {
    var gridRow = cGrdDevice.GetVisibleRowsOnPage();
    $.ajax({
        type: "POST",
        url: '/SRVFileuploadDelivery/DeviceNumberDetails',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ DeviceNumber: $("#txtDeviceNumber").val() }),
        success: function (response) {
            if (response.EntityCode != "" || response.EntityCode != null) {
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
                        }
                        else {
                            document.getElementById("txtModel").disabled = false;
                        }
                        getWarrantydt();
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
                    }
                    else {
                        document.getElementById("txtModel").disabled = false;
                    }

                    getWarrantydt();
                }
            }
            else {
                jAlert("Please enter valid Serial No.", "Alert", function () {
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

var rosterstatus = false;
function WorkingRoster() {
    $.ajax({
        type: "POST",
        url: 'index.aspx/CheckWorkingRoster',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        data: JSON.stringify({ module_ID: '1' }),
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