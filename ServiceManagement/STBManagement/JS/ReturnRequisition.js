
$(document).ready(function () {

    $('#dataTable').DataTable({
        scrollX: true,
        fixedColumns: {
            rightColumns: 2
        }
    });
    $('#dataTable2').DataTable({
    });

    
    document.getElementById('txtDocumentNumber').disabled = true;

    ctxtUnitPrice.SetEnabled(true);
    ctxtAmount.SetEnabled(true);


    if ($("#hdAddEdit").val() == "edit") {
        $("#btnSaveNew").addClass('hide');
        //if ($("#hdnIsCancel").val() == "True") {
        //    $("#DivCancelData").removeClass('hide');
        //    $("#DivCancelDetails, #lblCancelMsg").removeClass('hide');
        //    $("#btnSaveExit").addClass('hide');
        //    $("#btnAdd").addClass('hide');
        //}
        //else {
        //    $("#DivCancelData").addClass('hide');
        //    $("#DivCancelDetails, #lblCancelMsg").addClass('hide');

        //}
        //$("#divPrintCount").removeClass('hide');


    }
    else if ($("#hdAddEdit").val() == "view") {
        $("#btnSaveNew").addClass('hide');
        $("#btnSaveExit").addClass('hide');
        $("#btnAdd").addClass('hide');
        //if ($("#hdnIsCancel").val() == "True") {
        //    $("#DivCancelData").removeClass('hide');
        //    $("#DivCancelDetails, #lblCancelMsg").removeClass('hide');
        //}
        //else {
        //    $("#DivCancelData, #lblCancelMsg").addClass('hide');
        //    $("#DivCancelDetails").addClass('hide');
        //}
        //$("#divPrintCount").removeClass('hide');


    }
    else {
        $("#btnSaveNew").removeClass('hide');
        $("#btnSaveExit").removeClass('hide');
        $("#btnAdd").removeClass('hide');
        //$("#DivCancelData, #lblCancelMsg").addClass('hide');
        //$("#DivCancelDetails").addClass('hide');
        //$("#divPrintCount").addClass('hide');

       // cFormDate.SetDate(new Date());

        ctxtUnitPrice.SetEnabled(false);
        ctxtAmount.SetEnabled(false);

        cddlOSTBVendor.SetEnabled(false);
    }
    cddlBranch.SetEnabled(false);
    cFormDate.SetEnabled(false);
});



function Devicenumber_change() {
    //var gridRow = cGrdDevice.GetVisibleRowsOnPage();
    if ($("#txtEntityCode").val() != "") {

        $.ajax({
            type: "POST",
            url: "ReturnRequisition.aspx/FetchEntityFromMaster",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ entity_code: $("#txtEntityCode").val() }),
            async: false,
            success: function (msg) {
                var Masterresponse = msg.d;
                var IsActive = Masterresponse;
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
                                $("#txtDAS").val(response.Das);
                            }
                            else {
                                if ($("#hdnIsEntityInformationEditableInRequisition").val() == "Yes") {
                                    jConfirm('Entity information is not available. Do you wish to add manually?', 'Alert', function (r) {
                                        if (r) {
                                            $("#txtNetworkName").prop('disabled', false);
                                            $("#txtContactPerson").prop('disabled', false);
                                            $("#txtContactNo").prop('disabled', false);
                                            $("#txtDAS").prop('disabled', false);
                                        }
                                        else {
                                            $("#txtNetworkName").prop('disabled', true);
                                            $("#txtContactPerson").prop('disabled', true);
                                            $("#txtContactNo").prop('disabled', true);
                                            $("#txtDAS").prop('disabled', true);
                                        }
                                    });
                                }
                                else {
                                    jAlert("Please enter valid Entity Code.", "Alert", function () {
                                        setTimeout(function () {
                                            $("#txtNetworkName").prop('disabled', true);
                                            $("#txtContactPerson").prop('disabled', true);
                                            $("#txtContactNo").prop('disabled', true);
                                            $("#txtDAS").prop('disabled', true);
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
                        $("#txtEntityCode").val('');
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
                                    $("#txtDAS").val(response.Das);
                                }
                                else {
                                    if ($("#hdnIsEntityInformationEditableInRequisition").val() == "Yes") {
                                        jConfirm('Entity information is not available. Do you wish to add manually?', 'Alert', function (r) {
                                            if (r) {
                                                $("#txtNetworkName").prop('disabled', false);
                                                $("#txtContactPerson").prop('disabled', false);
                                                $("#txtContactNo").prop('disabled', false);
                                                $("#txtDAS").prop('disabled', false);
                                            }
                                            else {
                                                $("#txtNetworkName").prop('disabled', true);
                                                $("#txtContactPerson").prop('disabled', true);
                                                $("#txtContactNo").prop('disabled', true);
                                                $("#txtDAS").prop('disabled', true);
                                            }
                                        });
                                    }
                                    else {
                                        jAlert("Please enter valid Entity Code Code.", "Alert", function () {
                                            setTimeout(function () {
                                                $("#txtNetworkName").prop('disabled', true);
                                                $("#txtContactPerson").prop('disabled', true);
                                                $("#txtContactNo").prop('disabled', true);
                                                $("#txtDAS").prop('disabled', true);
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

    var STBRequisitionID = $("#hdnSTBRequisitionID").val();
    var Model = cddlModel.GetText();
    var Model_ID = cddlModel.GetValue();
    var Amount = ctxtAmount.GetText();
    var UnitPrice = ctxtUnitPrice.GetValue();
    var Quantity = ctxtQuantity.GetValue();
    var Remarks = $("#txtRemarks").val();
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

    if (cddlRequisitionFor.GetText() == "STB-02") {
        if (cddlOSTBVendor.GetValue() == "0") {
            jAlert("Please select OSTB Vendor.")
            cddlModel.Focus();
            suc = false;
            return
        }
    }
    else {
        if (cddlModel.GetValue()== null || cddlModel.GetValue() == "0") {
            jAlert("Please select Model.")
            cddlModel.Focus();
            suc = false;
            return
        }
    }


    //if (ctxtUnitPrice.GetValue() == 0) {
    //    jAlert("Please enter Unit Price.")
    //    ctxtUnitPrice.Focus();
    //    suc = false;
    //    return
    //}

    if (ctxtQuantity.GetValue() <= 0) {
        jAlert("Please enter Quantity.")
        ctxtQuantity.Focus();
        suc = false;
        return
    }
    var OSTBVendor = cddlOSTBVendor.GetText();
    var OSTBVendorID = "";

    if (cddlOSTBVendor.GetText() == 'Select') {
        OSTBVendor = "";
    }
    OSTBVendorID = cddlOSTBVendor.GetValue();

    $.ajax({
        type: "POST",
        url: "ReturnRequisition.aspx/AddData",
        data: JSON.stringify({
            Model: Model,
            UnitPrice: UnitPrice,
            Quantity: Quantity,
            Amount: Amount,
            Remarks: Remarks,
            Model_ID: Model_ID,
            Guids: Gu_id,
            OSTBVendor: OSTBVendor,
            OSTBVendorID: OSTBVendorID
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var list = msg.d;
            if (msg.d != "") {
                cddlModel.SetValue(0);
                ctxtUnitPrice.SetText(0);
                ctxtQuantity.SetText(0);
                ctxtAmount.SetText(0);
                $("#txtRemarks").val("");
                $('#hdnGuid').val('');
                cddlOSTBVendor.SetValue(0);
                cGrdDevice.PerformCallback();
            }

            if (msg.d == "Logout") {
                location.href = "../../OMS/SignOff.aspx";
            }

            jAlert(msg.d, "Alert", function () {
                setTimeout(function () {
                    cddlModel.Focus();
                }, 200);

            });
        }
    });
}

function ClickOnEdit(val) {
    $.ajax({
        type: "POST",
        url: "ReturnRequisition.aspx/EditData",
        data: JSON.stringify({ HiddenID: val }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var list = msg.d;
            $('#hdnGuid').val(val);
            cddlModel.SetValue(msg.d.Model_ID);
            ctxtUnitPrice.SetText(msg.d.UnitPrice);
            ctxtQuantity.SetText(msg.d.Quantity);
            ctxtAmount.SetText(msg.d.Amount);
            $("#txtRemarks").val(msg.d.Remarks);
            cddlOSTBVendor.SetValue(msg.d.OSTBVendorID);
            cGrdDevice.PerformCallback();

        }
    });
}

function OnClickDelete(val) {
    jConfirm('Confirm Delete?', 'Alert', function (r) {
        if (r) {
            $.ajax({
                type: "POST",
                url: "ReturnRequisition.aspx/DeleteData",
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

      //  cFormDate.SetDate(dt);

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
    //WorkingRoster();
    //if (rosterstatus) {
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

    if (cddlRequisitionType.GetValue() == "0") {
        jAlert("Please Select Requisition Type.", "Alert", function () {
            setTimeout(function () {
                LoadingPanel.Hide();
                cddlRequisitionType.Focus();
                return
            }, 200);
        });
        return
    }

    if (cddlRequisitionFor.GetValue() == "0") {
        jAlert("Please Select Requisition For.", "Alert", function () {
            setTimeout(function () {
                LoadingPanel.Hide();
                cddlRequisitionFor.Focus();
                return
            }, 200);
        });
        return
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

    var IsNoPayment = "0";

    if ($('#chkNoPayment').is(':checked') == true) {
        IsNoPayment = "1";
    }

    var IsPayUsingOnAcountBalance = "0";

    if ($('#chkPayUsingOnAcountBalance').is(':checked') == true) {
        IsPayUsingOnAcountBalance = "1";
    }

    var ApprovalAction = "";
    var DirectorApprovalRequired = "";
    var ApprovalEmployee = "";
    var txtApprovalRemarks = "";
    var IsApproval = "";
    if ($("#hdnIsAproval").val() == "Yes") {

        if ($('#chkDirectorApprovalRequired').is(':checked') == true) {
            DirectorApprovalRequired = "1";

            if (cdddlApprovalEmployee.GetValue() == "0") {
                jAlert("Please Select Approval Employee.", "Alert", function () {
                    setTimeout(function () {
                        LoadingPanel.Hide();
                        cdddlApprovalEmployee.Focus();
                        return
                    }, 200);
                });
                return
            }
            ApprovalEmployee = cdddlApprovalEmployee.GetValue();
        }
        else {

            DirectorApprovalRequired = "0";

            if ($("#ddlApprovalAction").val() == "0") {
                jAlert("Please Select Approval Action.", "Alert", function () {
                    setTimeout(function () {
                        LoadingPanel.Hide();
                        $("#ddlApprovalAction").focus();
                        return
                    }, 200);
                });
                return
            }
        }

        if ($("#txtApprovalRemarks").val() == "") {
            jAlert("Please Enter Approval Remarks.", "Alert", function () {
                setTimeout(function () {
                    LoadingPanel.Hide();
                    $("#txtApprovalRemarks").focus();
                    return
                }, 200);
            });
            return
        }

        ApprovalAction = $("#ddlApprovalAction").val();
        txtApprovalRemarks = $("#txtApprovalRemarks").val();
        IsApproval = "Yes";

        if ($("#hdnLastFiveRequisitionValidation").val() == "0") {
            jAlert("Please check Last Five Requisition.")
            LoadingPanel.Hide();
            return
        }
    }

    var frmDate = GetDateFormat(cFormDate.GetValue());

    var Apply = {
        Action: $("#hdAddEdit").val(),
        STBRequisitionID: $("#hdnSTBRequisitionID").val(),
        CmbScheme: cCmbScheme.GetValue(),
        DocumentNumber: $("#txtDocumentNumber").val(),
        branch: cddlBranch.GetValue(),
        date: frmDate,
        RequisitionType: cddlRequisitionType.GetValue(),
        RequisitionFor: cddlRequisitionFor.GetValue(),
        EntityCode: $("#txtEntityCode").val(),
        NetworkName: $("#txtNetworkName").val(),
        ContactPerson: $("#txtContactPerson").val(),
        ContactNo: $("#txtContactNo").val(),
        DAS: $("#txtDAS").val(),
        IsNoPayment: IsNoPayment,
        PaymentRelatedRemarks: $("#txtPaymentRelatedRemarks").val(),
        IsPayUsingOnAcountBalance: IsPayUsingOnAcountBalance,
        ApprovalAction: ApprovalAction,
        DirectorApprovalRequired: DirectorApprovalRequired,
        ApprovalEmployee: ApprovalEmployee,
        ApprovalRemarks: txtApprovalRemarks,
        IsApproval: IsApproval
    }

    $.ajax({
        type: "POST",
        url: "ReturnRequisition.aspx/save",
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
                        msgs = "Return Requisition " + response.d.split('~')[2] + " generated successfully.";

                        //if ($("#hdnOnlinePrint").val() == "Yes") {
                        //    if ($("#hdAddEdit").val() == "Add") {
                        //        $.ajax({
                        //            type: "POST",
                        //            url: 'ReturnRequisition.aspx/UpdatePrintCount',
                        //            contentType: "application/json; charset=utf-8",
                        //            dataType: "json",
                        //            async: false,
                        //            data: JSON.stringify({ WalletRechargeID: response.d.split('~')[1] }),
                        //            success: function (response) {

                        //            },
                        //            error: function (response) {
                        //                //jAlert(response);
                        //            }
                        //        });
                        //    }

                        //    window.open("../../OMS/Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=WalletRecharge~D&modulename=WalletRecharge&id=" + response.d.split('~')[1] + "&PrintOption=1", '_blank');
                        //}
                    }
                    else {
                        msgs = "Return Requisition " + response.d.split('~')[2] + " update successfully.";
                    }

                    jAlert(msgs, "Alert", function () {
                        if (values == "new") {
                            LoadingPanel.Hide();
                            window.location.href = "ReturnRequisition.aspx?Key=Add";
                        }
                        else if (values == "Exit") {
                            if ($("#hdnIsAproval").val() == "Yes") {
                                window.parent.popup.Hide();
                                window.parent.ButtonCount();
                                window.parent.cgridStatus.Refresh();
                            }
                            else {
                                window.location.href = "ReturnRequisitionList.aspx";
                            }
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
    //}
    //else {
    //    //jAlert("Working period is over. Try in next working period.", "Alert", function () {
    //    //    window.location.href = "ReturnRequisitionList.aspx";
    //    //});
    //    $("#divPopHead").removeClass('hide');
    //}
}

function WorkingRosterClick() {
    window.location.href = "ReturnRequisitionList.aspx";
}

function cancel() {
    window.location.href = "ReturnRequisitionList.aspx";
}

$(document).ready(function () {
    setTimeout(function () {
        if ($('body').hasClass('mini-navbar')) {
            var windowWidth = $(window).width();
            var cntWidth = windowWidth - 90;
            cGrdDevice.SetWidth(cntWidth);
        } else {
            var windowWidth = $(window).width();
            var cntWidth = windowWidth - 220;
            cGrdDevice.SetWidth(cntWidth);
        }
    }, 1000);

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

var rosterstatus = false;
function WorkingRoster() {
    $.ajax({
        type: "POST",
        url: 'ReturnRequisitionList.aspx/CheckWorkingRoster',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        data: JSON.stringify({ module_ID: '4' }),
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

function OnUnitPriceLostFocus(s, e) {
    var a = parseFloat(ctxtUnitPrice.GetValue());
    var b = parseFloat(ctxtQuantity.GetValue());
    var c = a * b;
    ctxtAmount.SetValue(c);
}

function OnQuantityLostFocus(s, e) {
    var a = parseFloat(ctxtUnitPrice.GetValue());
    var b = parseFloat(ctxtQuantity.GetValue());
    var c = a * b;
    ctxtAmount.SetValue(c);
}

function ddlRequisitionFor_ValueChange() {
    //if (cddlRequisitionFor.GetText() == "Fresh Issue") {
    //    ctxtUnitPrice.SetEnabled(false);
    //    $("#divDetails2").addClass('hide');
    //}
    //else if (cddlRequisitionFor.GetValue() == "0") {
    //    $("#divDetails2").addClass('hide');
    //    ctxtUnitPrice.SetEnabled(true);
    //}
    //else {
    //    ctxtUnitPrice.SetEnabled(true);
    //    $("#spnDetails2HeaderName").text(cddlRequisitionFor.GetText() + " Details");
    //    $("#divDetails2").removeClass('hide');
    //}
    cddlOSTBVendor.SetValue(0);
    if (cddlRequisitionFor.GetText() == "STB-01") {
        cddlModel.PerformCallback("STB-01");
        cddlOSTBVendor.SetEnabled(false);
    }
    else if (cddlRequisitionFor.GetText() == "STB-02") {
        cddlModel.PerformCallback("STB-02");
        cddlOSTBVendor.SetEnabled(true);
    }
}

function ddlModel_ValueChange() {
    $.ajax({
        type: "POST",
        url: "ReturnRequisition.aspx/GetUnitPrice",
        data: JSON.stringify({
            ModelId: cddlModel.GetValue(),
            DAS: $("#txtDAS").val(),
            RequisitionFor: cddlRequisitionFor.GetValue()
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var list = msg.d;
            if (msg.d != "") {
                ctxtUnitPrice.SetText(msg.d);
            }
        }
    });
}

function chkDirectorApprovalRequired_change() {
    var dropdownli = ""
    if ($('#chkDirectorApprovalRequired').is(':checked') == true) {

        dropdownli = ''
        dropdownli += '<select id="ddlApprovalAction" class="form-control" disabled> '
        dropdownli += '<option value="4">Director Approval</option> '
        dropdownli += '</select> '

        $("#divEmployee").removeClass('hide');
    }
    else {
        dropdownli = ''
        dropdownli += '<select id="ddlApprovalAction" class="form-control"> '
        dropdownli += '<option value="0">Select</option> '
        dropdownli += '<option value="1">Approve</option> '
        dropdownli += '<option value="2">Reject</option> '
        dropdownli += '<option value="3">Hold</option> '
        dropdownli += '</select> '

        $("#divEmployee").addClass('hide');
    }
    $("#tdddlApprovalAction").html(dropdownli);
}

function TodaysRequisition_click() {
    $("#spnModelHeader").text("Todays Requisition");
    var status = "<table class='table table-striped table-bordered tableStyle' id='dataTable'>";
    status = status + " <thead><tr>";
    status = status + " <th>Document No</th><th>Date</th>";
    status = status + " <th>Req. Type</th><th>Req. For</th><th>Amount</th><th>Status</th></tr></thead>";
    status = status + " </table>";
    $('#DivHistoryTable').html(status);
    setTimeout(function () {
        $.ajax({
            type: "POST",
            url: "ReturnRequisition.aspx/MoneyReceiptRequisitionHistoryList",
            data: JSON.stringify({ report: "TodaysRequisition" }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {

                $('#dataTable').DataTable({
                    data: msg.d,
                    columns: [
                       { 'data': 'DocumentNo' },
                       { 'data': 'Date' },
                       { 'data': 'ReqType' },
                       { 'data': 'ReqFor' },
                       { 'data': 'Amount' },
                       { 'data': 'Status' }
                    ],

                    error: function (error) {
                        alert(error);
                    }
                });

                $("#srrHist").modal('show');
            }
        });
    }, 1000);
}

function TodaysMoneyReceipt_click() {
    $("#spnModelHeader").text("Todays Money Receipt");
    var status = "<table class='table table-striped table-bordered tableStyle' id='dataTable'>";
    status = status + " <thead><tr>";
    status = status + " <th>Document No</th><th>Date</th>";
    status = status + " <th>Payment Type</th><th>Amount</th><th>Status</th></tr></thead>";
    status = status + " </table>";
    $('#DivHistoryTable').html(status);
    setTimeout(function () {
        $.ajax({
            type: "POST",
            url: "ReturnRequisition.aspx/MoneyReceiptRequisitionHistoryList",
            data: JSON.stringify({ report: "TodaysMoneyReceipt" }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {

                $('#dataTable').DataTable({
                    data: msg.d,
                    columns: [
                       { 'data': 'DocumentNo' },
                       { 'data': 'Date' },
                       { 'data': 'PaymentType' },
                       { 'data': 'Amount' },
                       { 'data': 'Status' }
                    ],

                    error: function (error) {
                        alert(error);
                    }
                });

                $("#srrHist").modal('show');
            }
        });
    }, 1000);
}

function LastFiveRequisition_click() {
    $("#hdnLastFiveRequisitionValidation").val('1');
    $("#spnModelHeader").text("Last Five Requisition");
    var status = "<table class='table table-striped table-bordered tableStyle' id='dataTable'>";
    status = status + " <thead><tr>";
    status = status + " <th>Document No</th><th>Date</th>";
    status = status + " <th>Req. Type</th><th>Req. For</th><th>Amount</th><th>Status</th></tr></thead>";
    status = status + " </table>";
    $('#DivHistoryTable').html(status);
    setTimeout(function () {
        $.ajax({
            type: "POST",
            url: "ReturnRequisition.aspx/MoneyReceiptRequisitionHistoryList",
            data: JSON.stringify({ report: "LastFiveRequisition" }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {

                $('#dataTable').DataTable({
                    data: msg.d,
                    columns: [
                       { 'data': 'DocumentNo' },
                       { 'data': 'Date' },
                       { 'data': 'ReqType' },
                       { 'data': 'ReqFor' },
                       { 'data': 'Amount' },
                       { 'data': 'Status' }
                    ],

                    error: function (error) {
                        alert(error);
                    }
                });

                $("#srrHist").modal('show');
            }
        });
    }, 1000);
}

function LastFiveMoneyReceipt_click() {
    $("#spnModelHeader").text("Last Five Money Receipt");
    var status = "<table class='table table-striped table-bordered tableStyle' id='dataTable'>";
    status = status + " <thead><tr>";
    status = status + " <th>Document No</th><th>Date</th>";
    status = status + " <th>Payment Type</th><th>Amount</th><th>Status</th></tr></thead>";
    status = status + " </table>";
    $('#DivHistoryTable').html(status);
    setTimeout(function () {
        $.ajax({
            type: "POST",
            url: "ReturnRequisition.aspx/MoneyReceiptRequisitionHistoryList",
            data: JSON.stringify({ report: "LastFiveMoneyReceipt" }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {

                $('#dataTable').DataTable({
                    data: msg.d,
                    columns: [
                       { 'data': 'DocumentNo' },
                       { 'data': 'Date' },
                       { 'data': 'PaymentType' },
                       { 'data': 'Amount' },
                       { 'data': 'Status' }
                    ],

                    error: function (error) {
                        alert(error);
                    }
                });

                $("#srrHist").modal('show');
            }
        });
    }, 1000);
}