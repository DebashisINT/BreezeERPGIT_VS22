
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

    if ($("#hdAddEdit").val() == "edit") {
        $("#btnSaveNew").addClass('hide');
    }
    else if ($("#hdAddEdit").val() == "view") {
        $("#btnSaveNew").addClass('hide');
        $("#btnSaveExit").addClass('hide');
        $("#btnAdd").addClass('hide');
    }
    else {
        cFormDate.SetDate(new Date());

        $("#btnSaveNew").removeClass('hide');
        $("#btnSaveExit").removeClass('hide');
        $("#btnAdd").removeClass('hide');
    }
    cddlBranch.SetEnabled(false);
    cFormDate.SetEnabled(false);
});

function Details2Hide(val) {  
    ctxtUnitPrice.SetEnabled(true);
   
}

function Devicenumber_change() {
    //var gridRow = cGrdDevice.GetVisibleRowsOnPage();
    if ($("#txtEntityCode").val() != "") {

        $.ajax({
            type: "POST",
            url: "STBSchemeRequisition.aspx/FetchEntityFromMaster",
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
                            if (response.EntityCode != "" || response.EntityCode != null) {
                                $("#txtEntityCode").val(response.EntityCode);
                                $("#txtNetworkName").val(response.NetworkName);
                                $("#txtContactPerson").val(response.ContactPerson);
                                $("#txtContactNo").val(response.ContactNumber);
                                $("#txtDAS").val(response.Das);
                            }
                            else {
                                jAlert("Please enter valid Entity Code.", "Alert", function () {
                                    setTimeout(function () {
                                        $("#txtEntityCode").val("");
                                        $("#txtEntityCode").focus();
                                    }, 200);
                                });
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
                                if (response.EntityCode != "" || response.EntityCode != null) {
                                    $("#txtEntityCode").val(response.EntityCode);
                                    $("#txtNetworkName").val(response.NetworkName);
                                    $("#txtContactPerson").val(response.ContactPerson);
                                    $("#txtContactNo").val(response.ContactNumber);
                                    $("#txtDAS").val(response.Das);
                                }
                                else {
                                    jAlert("Please enter valid Entity Code Code.", "Alert", function () {
                                        setTimeout(function () {
                                            $("#txtEntityCode").val("");
                                            $("#txtEntityCode").focus();
                                        }, 200);
                                    });
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
    var OSTBVendor = cddlOSTBVendor.GetText();
    var OSTBVendor_ID = cddlOSTBVendor.GetValue();

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

    if (cddlModel.GetValue() == "0") {
        jAlert("Please select Model.")
        cddlModel.Focus();
        suc = false;
        return
    }

    if (cddlRequisitionFor.GetValue() == "1") {
        if (ctxtUnitPrice.GetValue() == 0) {
            jAlert("Please enter Unit Price.")
            ctxtUnitPrice.Focus();
            suc = false;
            return
        }
    }

    if (ctxtQuantity.GetValue() == 0) {
        jAlert("Please enter Quantity.")
        ctxtQuantity.Focus();
        suc = false;
        return
    }


    $.ajax({
        type: "POST",
        url: "STBSchemeRequisition.aspx/AddData",
        data: JSON.stringify({
            Model: Model,
            UnitPrice: UnitPrice,
            Quantity: Quantity,
            Amount: Amount,
            Remarks: Remarks,
            Model_ID: Model_ID,
            Guids: Gu_id,
            OSTBVendor: OSTBVendor,
            OSTBVendor_ID: OSTBVendor_ID
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

    $("#divDetails1EntryLeven").removeClass('hide');

    $.ajax({
        type: "POST",
        url: "STBSchemeRequisition.aspx/EditData",
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
            cddlOSTBVendor.SetValue(msg.d.OSTBVendor_ID);
            $("#txtRemarks").val(msg.d.Remarks);

            cGrdDevice.PerformCallback();

        }
    });
}

function OnClickDelete(val) {
    jConfirm('Confirm Delete?', 'Alert', function (r) {
        if (r) {
            $.ajax({
                type: "POST",
                url: "STBSchemeRequisition.aspx/DeleteData",
                data: JSON.stringify({ HiddenID: val }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;
                    $("#divDetails1EntryLeven").removeClass('hide');
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
            DAS: $("#txtDAS").val()
        }

        $.ajax({
            type: "POST",
            url: "STBSchemeRequisition.aspx/save",
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
                            msgs = "STB Scheme Requisition " + response.d.split('~')[2] + " generated successfully.";

                            //if ($("#hdnOnlinePrint").val() == "Yes") {
                            //    if ($("#hdAddEdit").val() == "Add") {
                            //        $.ajax({
                            //            type: "POST",
                            //            url: 'STBRequisitionAdd.aspx/UpdatePrintCount',
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
                            msgs = "STB Scheme Requisition " + response.d.split('~')[2] + " update successfully.";
                        }

                        jAlert(msgs, "Alert", function () {
                            if (values == "new") {
                                LoadingPanel.Hide();
                                window.location.href = "STBSchemeRequisition.aspx?Key=Add";
                            }
                            else if (values == "Exit") {
                                if ($("#hdnIsAproval").val() == "Yes") {
                                    window.parent.popup.Hide();
                                    window.parent.ButtonCount();
                                    window.parent.cgridStatus.Refresh();
                                }
                                else {
                                    window.location.href = "STBSchemeRequisitionList.aspx";
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
    }
    else {
        //jAlert("Working period is over. Try in next working period.", "Alert", function () {
        //    window.location.href = "STBRequisition.aspx";
        //});
        $("#divPopHead").removeClass('hide');
    }
}

function WorkingRosterClick() {
    window.location.href = "STBSchemeRequisitionList.aspx";
}

function cancel() {
    window.location.href = "STBSchemeRequisitionList.aspx";
}

$(document).ready(function () {
    setTimeout(function () {
        if ($('body').hasClass('mini-navbar')) {
            var windowWidth = $(window).width();
            var cntWidth = windowWidth - 90;
            cGrdDevice.SetWidth(cntWidth);
            cgridDeviceDetails2.SetWidth(cntWidth);
        } else {
            var windowWidth = $(window).width();
            var cntWidth = windowWidth - 220;
            cGrdDevice.SetWidth(cntWidth);
            cgridDeviceDetails2.SetWidth(cntWidth);
        }
    }, 1000);

    $('.navbar-minimalize').click(function () {
        if ($('body').hasClass('mini-navbar')) {
            var windowWidth = $(window).width();
            var cntWidth = windowWidth - 220;
            cGrdDevice.SetWidth(cntWidth);
            cgridDeviceDetails2.SetWidth(cntWidth);
        } else {
            var windowWidth = $(window).width();
            var cntWidth = windowWidth - 90;
            cGrdDevice.SetWidth(cntWidth);
            cgridDeviceDetails2.SetWidth(cntWidth);
        }

    });
});

var rosterstatus = false;
function WorkingRoster() {
    $.ajax({
        type: "POST",
        url: 'STBSchemeRequisitionList.aspx/CheckWorkingRoster',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        data: JSON.stringify({ module_ID: '3' }),
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
    var a = parseInt(ctxtUnitPrice.GetText());
    var b = parseInt(ctxtQuantity.GetText());
    var c = a * b;
    ctxtAmount.SetValue(c);
}

function OnQuantityLostFocus(s, e) {
    var a = parseInt(ctxtUnitPrice.GetText());
    var b = parseInt(ctxtQuantity.GetText());
    var c = a * b;
    ctxtAmount.SetValue(c);
}

function ddlRequisitionFor_ValueChange() {
    if (cddlRequisitionFor.GetText() == "Fresh Issue") {
        ctxtUnitPrice.SetEnabled(false);
    }
    else if (cddlRequisitionFor.GetValue() == "0") {
        ctxtUnitPrice.SetEnabled(true);
    }
    else {
        ctxtUnitPrice.SetEnabled(true);
    }
}

function ddlModel_ValueChange() {
    $.ajax({
        type: "POST",
        url: "STBSchemeRequisition.aspx/GetUnitPrice",
        data: JSON.stringify({
            ModelId: cddlModel.GetValue(),
            DAS: $("#txtDAS").val()
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
