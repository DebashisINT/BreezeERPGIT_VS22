
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

    if ($("#Hidden_add_edit").val() == "Insert") {
        cFormDate.SetDate(new Date());
        cPostingDate.SetDate(new Date());
    }

    cddlBranch.SetEnabled(false);
    cPostingDate.SetEnabled(false);
    //ctxtProdName.SetEnabled(false);
    cddlReturnReason.SetEnabled(false);
    gridComponentLookup.SetEnabled(false);
    // Mantis Issue 24413/24417
    cselModel.SetEnabled(false);
    // End of Mantis Issue 24413/24417

    if ($("#Hidden_add_edit").val() == "Update") {
        $("#btnSaveNew").addClass('hide');
        document.getElementById("txtEntityCode").disabled = true;
        document.getElementById("txtNetworkName").disabled = true;
        document.getElementById("txtContactPerson").disabled = true;
        document.getElementById("txtContactNumber").disabled = true;
    }
    else if ($("#Hidden_add_edit").val() == "View") {
        $("#btnSaveNew").addClass('hide');
        $("#btnSaveExit").addClass('hide');
        document.getElementById("txtEntityCode").disabled = true;
        document.getElementById("txtNetworkName").disabled = true;
        document.getElementById("txtContactPerson").disabled = true;
        document.getElementById("txtContactNumber").disabled = true;
    }
    else {
        $("#btnSaveNew").removeClass('hide');
        $("#btnSaveExit").removeClass('hide');
        document.getElementById("txtEntityCode").disabled = true;
        document.getElementById("txtNetworkName").disabled = true;
        document.getElementById("txtContactPerson").disabled = true;
        document.getElementById("txtContactNumber").disabled = true;
    }
});
$(function () {

});

function SetSelectedValues(Id, Name, ArrName) {
    if (ArrName == 'ClassSource') {
        var key = Id;
        if (key != null && key != '') {
            $('#ClassModel').modal('hide');
            ctxtClass.SetText(Name);
            GetObjectID('hdnClassId').value = key;
            // ctxtProdName.SetText('');
            $('#txtProdSearch').val('')
            var OtherDetailsProd = {}
            OtherDetailsProd.SearchKey = 'undefined text';
            OtherDetailsProd.ClassID = '';
            var HeaderCaption = [];
            HeaderCaption.push("Code");
            HeaderCaption.push("Name");
            HeaderCaption.push("Hsn");
            callonServerM("Services/Master.asmx/GetClassWiseProductJobsheet", OtherDetailsProd, "ProductTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "ProductSource");
        }
        else {
            ctxtClass.SetText('');
            GetObjectID('hdnClassId').value = '';
        }
    }
    else if (ArrName == 'ProductSource') {
        var key = Id;
        if (key != null && key != '') {
            $('#ProdModel').modal('hide');
            // ctxtProdName.SetText(Name);
            GetObjectID('hdncWiseProductId').value = key;
        }
        else {
            // ctxtProdName.SetText('');
            GetObjectID('hdncWiseProductId').value = '';
        }
    }
    else if (ArrName == 'BrandSource') {
        var key = Id;
        if (key != null && key != '') {
            $('#BrandModel').modal('hide');
            ctxtBrandName.SetText(Name);
            GetObjectID('hdnBranndId').value = key;
        }
        else {
            ctxtBrandName.SetText('');
            GetObjectID('hdnBranndId').value = '';
        }
    }
}

$(document).ready(function () {
    $('#ClassModel').on('shown.bs.modal', function () {
        $('#txtClassSearch').focus();
    })
});
var ClassArr = new Array();
$(document).ready(function () {
    var ClassObj = new Object();
    ClassObj.Name = "ClassSource";
    ClassObj.ArraySource = ClassArr;
    arrMultiPopup.push(ClassObj);
})
function ClassButnClick(s, e) {
    $('#ClassModel').modal('show');
}
function Class_KeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        $('#ClassModel').modal('show');
    }
}
function Classkeydown(e) {
    var OtherDetails = {}
    if ($.trim($("#txtClassSearch").val()) == "" || $.trim($("#txtClassSearch").val()) == null) {
        return false;
    }
    OtherDetails.SearchKey = $("#txtClassSearch").val();
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Class Name");

        if ($("#txtClassSearch").val() != "") {
            callonServerM("Services/Master.asmx/GetClass", OtherDetails, "ClassTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "ClassSource");
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[dPropertyIndex=0]"))
            $("input[dPropertyIndex=0]").focus();
    }
}
function SetfocusOnseach(indexName) {
    if (indexName == "dPropertyIndex")
        $('#txtClassSearch').focus();
    else
        $('#txtClassSearch').focus();
}

$(document).ready(function () {
    $('#ProdModel').on('shown.bs.modal', function () {
        $('#txtProdSearch').focus();
    })
})
var ProdArr = new Array();
$(document).ready(function () {
    var ProdObj = new Object();
    ProdObj.Name = "ProductSource";
    ProdObj.ArraySource = ProdArr;
    arrMultiPopup.push(ProdObj);
})
function ProductButnClick(s, e) {
    $('#ProdModel').modal('show');
}
function Product_KeyDown(s, e) {
    if (e.htmlEvent.key == "Enter") {
        $('#ProdModel').modal('show');
    }
}
function Productkeydown(e) {
    var OtherDetails = {}
    if ($.trim($("#txtProdSearch").val()) == "" || $.trim($("#txtProdSearch").val()) == null) {
        return false;
    }
    OtherDetails.SearchKey = $("#txtProdSearch").val();
    OtherDetails.ClassID = hdnClassId.value;
    if (e.code == "Enter" || e.code == "NumpadEnter") {
        var HeaderCaption = [];
        HeaderCaption.push("Code");
        HeaderCaption.push("Name");
        HeaderCaption.push("Hsn");
        if ($("#txtProdSearch").val() != "") {
            callonServerM("../Services/Master.asmx/GetClassWiseProductJobsheet", OtherDetails, "ProductTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "ProductSource");
        }
    }
    else if (e.code == "ArrowDown") {
        if ($("input[dPropertyIndex=0]"))
            $("input[dPropertyIndex=0]").focus();
    }
}
function SetfocusOnseach(indexName) {
    if (indexName == "dPropertyIndex")
        $('#txtProdSearch').focus();
    else
        $('#txtProdSearch').focus();
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

function SaveJob(values) {

    var suc = true;
    LoadingPanel.Show();
    if (cCmbScheme.GetValue() == "0") {
        jAlert("Please select Numbering Scheme.", "Alert", function () {
            LoadingPanel.Hide();
            cCmbScheme.Focus();
            return
        });
        suc = false;
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

    if ($("#txtRefJobsheet").val() == "") {
        jAlert("Please enter ref jobsheet.", "Alert", function () {
            LoadingPanel.Hide();
            $("#txtRefJobsheet").focus();
            return
        });
        suc = false;
        return
    }

    if (cddlTechnician.GetValue() == "0") {
        jAlert("Please select assign to.", "Alert", function () {
            cddlTechnician.Focus();
            suc = false;
            return
        });
        return
    }

    if ($("#txtEntityCode").val() == "") {
        jAlert("Please enter Entity Code.", "Alert", function () {
            LoadingPanel.Hide();
            $("#txtEntityCode").focus();
            return
        });
        suc = false;
        return
    }

    if ($("#txtNetworkName").val() == "") {
        jAlert("Please enter Network Name.", "Alert", function () {
            LoadingPanel.Hide();
            $("#txtNetworkName").focus();
            return
        });
        suc = false;
        return
    }

    if ($("#txtContactPerson").val() == "") {
        jAlert("Please Enter Contact Person.", "Alert", function () {
            setTimeout(function () {
                LoadingPanel.Hide();
                $("#txtContactPerson").focus();
                suc = false;
                return
            }, 200);
        });
        suc = false;
        return
    }

    if ($("#txtContactNumber").val() == "") {
        jAlert("Please Enter Contact Number.", "Alert", function () {
            setTimeout(function () {
                LoadingPanel.Hide();
                $("#txtContactNumber").focus();
                suc = false;
                return
            }, 200);
        });
        suc = false;
        return
    }


    //if ($("#txtDeviceNumber").val() == "") {
    //    jAlert("Please enter serial number.", "Alert", function () {
    //        $("#txtDeviceNumber").focus();
    //        suc = false;
    //        return
    //    });
    //    suc = false;
    //    return
    //}

    //if (cDiviceTyp.GetValue() == "0") {
    //    jAlert("Please select device type.", "Alert", function () {
    //        cDiviceTyp.Focus();
    //        suc = false;
    //        return
    //    });
    //    return
    //}

    //if ($("#txtDeviceModel").val() == "") {
    //    jAlert("Please enter model.", "Alert", function () {
    //        $("#txtDeviceModel").focus();
    //        return
    //    });
    //    suc = false;
    //    return
    //}

    //if (cddlProblem.GetText() == "OTHER") {
    //    if ($("#txtOtherProblem").val() == "") {
    //        jAlert("Please enter other problem.", "Alert", function () {
    //            $("#txtOtherProblem").focus();
    //            return
    //        });
    //        suc = false;
    //        return
    //    }
    //}

    //if (cddlServiceAction.GetValue() == "0") {
    //    jAlert("Please select service action.", "Alert", function () {
    //        cddlServiceAction.Focus();
    //        suc = false;
    //        return
    //    });
    //    return
    //}

    //var Component = gridComponentLookup.gridView.GetSelectedKeysOnPage();
    var Components = "";
    //for (var i = 0; i < Component.length; i++) {
    //    Components += ',' + Component[i];
    //}

    //if (cddlServiceAction.GetText() == "Repaired") {
    //    if (Component == "") {
    //        jAlert("Please Component.", "Alert", function () {
    //            cddlServiceAction.Focus();
    //            suc = false;
    //            return
    //        });
    //        return
    //    }
    //}

    var IsBilable = 0;
    //if ($('#chkBillable').is(':checked') == true) {
    //    IsBilable = 1;
    //}

    var Warrenty = null;
    //if ($("#txtWarranty").val() != "") {
    //    var warsplit = $("#txtWarranty").val().split('-');
    //    Warrenty = warsplit[2] + '-' + warsplit[1] + '-' + warsplit[0];
    //}



    var frmDate = GetDateFormat(cFormDate.GetValue());
    var PostingDate = GetDateFormat(cPostingDate.GetValue());
    var data = {
        Action: $("#Hidden_add_edit").val(),
        NumberingScheme: cCmbScheme.GetValue(),
        ChallanNumber: $("#txtDocumentNumber").val(),
        RefJobsheet: $("#txtRefJobsheet").val(),
        AssignTo: cddlTechnician.GetValue(),
        WorkDoneOn: frmDate,
        Location: cddlBranch.GetValue(),
        HeaderRemarks: $("#txtHeaderRemarks").val(),
        EntityCode: $("#txtEntityCode").val(),
        NetworkName: $("#txtNetworkName").val(),
        ContactPerson: $("#txtContactPerson").val(),
        ContactNumber: $("#txtContactNumber").val(),
        SerialNumber: $("#txtDeviceNumber").val(),
        DeviceType: cDiviceTyp.GetValue(),
        Model: $("#txtDeviceModel").val(),
        Problem: cddlProblem.GetValue(),
        OtherProblem: $("#txtOtherProblem").val(),
        ServiceAction: cddlServiceAction.GetValue(),
        //Components: $("#hdncWiseProductId").val(),
        Components: Components,
        Warranty: Warrenty,
        ReturnReason: cddlReturnReason.GetValue(),
        NewModel: $("#ddlModel").val(),
        DetailsRemarks: $("#txtDetailsRemarks").val(),
        Billable: IsBilable,
        JobsheetID: $("#hdnJobSheetID").val(),
        PostingDate: PostingDate
    }

    $.ajax({
        type: "POST",
        url: "jobsheetEntry.aspx/save",
        data: "{apply:" + JSON.stringify(data) + "}",
        contentType: "application/json; charset=utf-8",
        async: true,
        dataType: "json",
        success: function (msg) {
            var list = msg.d;

            if (msg.d) {
                LoadingPanel.Hide();
                if (msg.d.split('~')[0] == "Success") {

                    var msgs = "";
                    if ($("#Hidden_add_edit").val() == "Insert") {
                        msgs = "Jobsheet no. " + msg.d.split('~')[1] + " generated successfully.";
                        var module = 'SRVJOBSHEET';
                        if (msg.d.split('~')[3] > 0) {
                            if ($("#hdnOnlinePrint").val() == "Yes") {
                                window.open("../../../OMS/Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=JobSheet~D&modulename=" + module + '&id=' + msg.d.split('~')[3], '_blank')
                            }
                        }
                    }
                    else {
                        msgs = "Jobsheet no. " + msg.d.split('~')[1] + " updated successfully.";
                    }

                    jAlert(msgs, "Alert", function () {
                        if (values == "New") {
                            window.location.href = "jobsheetEntry.aspx?Key=ADD";
                        }
                        else if (values == "Exit") {
                            window.location.href = "jobsheetList.aspx";
                        }
                    });
                }
                else {
                    jAlert(msg.d.split('~')[2]);
                    // $("#txtDeviceNumber").focus();
                    return
                }
            }
        }
    });

}

function cancel() {
    if ($("#Hidden_add_edit").val() != "Insert") {
        window.location.href = "jobsheetList.aspx";
    }
    else {
        window.location.href = "jobsheetEntry.aspx?Key=ADD";
    }
}

function ShowRepeatHistory() {

    $.ajax({
        type: "POST",
        url: "jobsheetEntry.aspx/ServiceEntryCountShow",
        data: JSON.stringify({ model: $("#txtDeviceModel").val(), DeviceNumber: $("#txtDeviceNumber").val() }),
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

function BindServiceEntryHistory() {
    var status = "<table class='table table-striped table-bordered tableStyle' id='dataTable'>";
    status = status + " <thead><tr>";
    status = status + " <th>Entity Code</th><th>Ref. Receipt No.</th>";
    status = status + " <th>Service Action</th><th>Remarks</th><th>Billable</th></tr></thead>";
    status = status + " </table>";
    $('#DivHistoryTable').html(status);
    setTimeout(function () {
        $.ajax({
            type: "POST",
            url: "jobsheetEntry.aspx/ServiceEntryHistoryList",
            data: JSON.stringify({ model: $("#txtDeviceModel").val(), DeviceNumber: $("#txtDeviceNumber").val() }),
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
        $.ajax({
            type: "POST",
            url: "jobsheetEntry.aspx/GeSerialValidation",
            data: JSON.stringify({ DeviceNumber: $("#txtDeviceNumber").val() }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var list = msg.d;
                if (msg.d != "DE" && msg.d != "") {
                    isValid = false;
                    jAlert("Serial No is already in service.", "Alert", function () {
                        setTimeout(function () {
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
    //else {
    //    $("#txtEntityCode").val("");
    //    $("#txtNetworkName").val("");
    //    $("#txtContactPerson").val("");
    //    $("#txtContactNumber").val("");
    //    $("#txtModel").val("");
    //}
}

//function GetDeviceDetails() {
//    $.ajax({
//        type: "POST",
//        url: '/SRVFileuploadDelivery/DeviceNumberDetails',
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        data: JSON.stringify({ DeviceNumber: $("#txtDeviceNumber").val() }),
//        success: function (response) {
//            if (response.EntityCode != "" && response.EntityCode != null) {
//                $("#txtEntityCode").val(response.EntityCode);
//                $("#txtNetworkName").val(response.NetworkName);
//                $("#txtContactPerson").val(response.ContactPerson);
//                $("#txtContactNumber").val(response.ContactNumber);
//                $("#txtDeviceModel").val(response.Model);
//                getWarrantydt();
//                cComponentPanel.PerformCallback('BindComponentGrid' + '~' + response.Model);
//            }
//            else {
//                jAlert("Please enter valid Serial No.", "Alert", function () {
//                    setTimeout(function () {
//                        // $("#txtDeviceNumber").val("");
//                        $("#txtDeviceNumber").focus();
//                    }, 200);
//                });
//            }
//        },
//        error: function (response) {

//        }
//    });
//}

function GetDeviceDetails() {
    var gridRow = cGrdDevice.GetVisibleRowsOnPage();
    $.ajax({
        type: "POST",
        url: '/SRVFileuploadDelivery/DeviceNumberDetails',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ DeviceNumber: $("#txtDeviceNumber").val() }),
        success: function (response) {
            // Mantis Issue 24413/24417
            //if (response.EntityCode != "" || response.EntityCode != null) {
            if (response.EntityCode != null && response.EntityCode != "") {
                // End of Mantis Issue 24413/24417
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
                        $("#txtDeviceModel").val(response.Model);

                        if (response.EntityCode != "") {
                            document.getElementById("txtDeviceModel").disabled = true;
                            // Mantis Issue 24413/24417
                            cselModel.SetEnabled(false);
                            cselModel.SetValue(0);
                            // End of Mantis Issue 24413/24417
                        }
                        else {
                            document.getElementById("txtDeviceModel").disabled = false;
                            // Mantis Issue 24413/24417
                            cselModel.SetEnabled(true);
                            cselModel.SetValue(0);
                            // End of Mantis Issue 24413/24417
                        }
                        getWarrantydt();
                        // cComponentPanel.PerformCallback('BindComponentGrid' + '~' + response.Model);
                    }
                }
                else {
                    if (response.EntityCode == null) {
                        if ($("#hdnIsEntityInformationEditableInJobsheet").val() == "Yes") {
                            jConfirm('Entity information is not available. Do you wish to add manually?', 'Alert', function (r) {
                                if (r) {
                                    $("#txtEntityCode").prop('disabled', false);
                                    $("#txtNetworkName").prop('disabled', false);
                                    $("#txtContactPerson").prop('disabled', false);
                                    // Mantis Issue 24413/24417
                                    //$("#txtContactNo").prop('disabled', false);
                                    //$("#txtModel").prop('disabled', false);
                                    $("#txtContactNumber").prop('disabled', false);
                                    $("#txtDeviceModel").prop('disabled', false);
                                    cselModel.SetEnabled(true);
                                    cselModel.SetValue(0);
                                    // End of Mantis Issue 24413/24417
                                }
                                else {
                                    $("#txtEntityCode").prop('disabled', true);
                                    $("#txtNetworkName").prop('disabled', true);
                                    $("#txtContactPerson").prop('disabled', true);
                                    // Mantis Issue 24413/24417
                                    //$("#txtContactNo").prop('disabled', true);
                                    //$("#txtModel").prop('disabled', true);
                                    $("#txtContactNumber").prop('disabled', true);
                                    $("#txtDeviceModel").prop('disabled', true);
                                    cselModel.SetEnabled(false);
                                    cselModel.SetValue(0);
                                    // End of Mantis Issue 24413/24417
                                }
                            });
                        }
                    }
                    else {
                        $("#txtEntityCode").val(response.EntityCode);
                        if (response.EntityCode != "") {
                            document.getElementById("txtEntityCode").disabled = true;
                        }
                        $("#hdnEntityCode").val(response.EntityCode);

                        $("#txtNetworkName").val(response.NetworkName);
                        if (response.EntityCode != "") {
                            document.getElementById("txtNetworkName").disabled = true;
                        }

                        $("#txtContactPerson").val(response.ContactPerson);
                        if (response.EntityCode != "") {
                            document.getElementById("txtContactPerson").disabled = true;
                        }

                        $("#txtContactNumber").val(response.ContactNumber);
                        if (response.EntityCode != "") {
                            document.getElementById("txtContactNumber").disabled = true;
                        }

                        $("#txtDeviceModel").val(response.Model);
                        if (response.EntityCode != "") {
                            document.getElementById("txtDeviceModel").disabled = true;
                            // Mantis Issue 24413/24417
                            cselModel.SetEnabled(false);
                            cselModel.SetValue(0);
                            // End of Mantis Issue 24413/24417
                        }
                        else {
                            document.getElementById("txtDeviceModel").disabled = false;
                            // Mantis Issue 24413/24417
                            cselModel.SetEnabled(true);
                            cselModel.SetValue(0);
                            // End of Mantis Issue 24413/24417
                        }

                        getWarrantydt();
                        // cComponentPanel.PerformCallback('BindComponentGrid' + '~' + response.Model);
                    }
                }
            }
            else {
                if ($("#hdnIsEntityInformationEditableInJobsheet").val() == "Yes") {
                    jConfirm('Entity information is not available. Do you wish to add manually?', 'Alert', function (r) {
                        if (r) {
                            $("#txtEntityCode").prop('disabled', false);
                            $("#txtNetworkName").prop('disabled', false);
                            $("#txtContactPerson").prop('disabled', false);
                            // Mantis Issue 24413/24417
                            //$("#txtContactNo").prop('disabled', false);
                            //$("#txtModel").prop('disabled', false);
                            $("#txtContactNumber").prop('disabled', false);
                            $("#txtDeviceModel").prop('disabled', false);
                            cselModel.SetEnabled(true);
                            cselModel.SetValue(0);
                            // End of Mantis Issue 24413/24417
                        }
                        else {
                            $("#txtEntityCode").prop('disabled', true);
                            $("#txtNetworkName").prop('disabled', true);
                            $("#txtContactPerson").prop('disabled', true);
                            // Mantis Issue 24413/24417
                            //$("#txtContactNo").prop('disabled', true);
                            //$("#txtModel").prop('disabled', true);
                            $("#txtContactNumber").prop('disabled', true);
                            $("#txtDeviceModel").prop('disabled', true);
                            cselModel.SetEnabled(false);
                            cselModel.SetValue(0);
                            // End of Mantis Issue 24413/24417
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
        url: "jobsheetEntry.aspx/GetWarrantyDate",
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


function ddlServiceAction_change() {
    gridComponentLookup.SetEnabled(false);
    cddlReturnReason.SetEnabled(false);
    //ctxtProdName.SetEnabled(false);
    var ServiceAction = cddlServiceAction.GetText();
    if (ServiceAction == "Repaired") {
        //ctxtProdName.SetEnabled(true);
        gridComponentLookup.SetEnabled(true);
        cComponentPanel.PerformCallback('BindComponentGrid' + '~' + $("#txtDeviceModel").val());
        $("#hdncWiseProductId").val("");
        //ctxtProdName.SetText("");
        unselectAllProduct();
        cddlReturnReason.SetValue(0);
    }
    else if (ServiceAction == "Returned") {
        cddlReturnReason.SetEnabled(true);
    }
    else {
        $("#hdncWiseProductId").val("");
        //ctxtProdName.SetText("");
        unselectAllProduct();
        gridComponentLookup.SetEnabled(false);
        cddlReturnReason.SetValue(0);
    }

}

$(function () {
    // cComponentPanel.PerformCallback('BindComponentGrid' + '~' + $("#txtDeviceModel").val());
});

function selectAllProduct() {
    gridComponentLookup.gridView.SelectRows();
}
function unselectAllProduct() {
    gridComponentLookup.gridView.UnselectRows();
}
function CloseProductLookup() {
    gridComponentLookup.ConfirmCurrentSelection();
    gridComponentLookup.HideDropDown();
    gridComponentLookup.Focus();
}

function AddDevice() {

    var suc = true;
    var Gu_id = $('#hdnGuid').val();

    if (cCmbScheme.GetValue() == "0") {
        jAlert("Please select Numbering Scheme.", "Alert", function () {
            cCmbScheme.Focus();
            return
        });
        suc = false;
        return
    }

    if ($("#txtDocumentNumber").val() == "") {
        $("#MandatoryBillNo").show();
        return
    }
    else {
        $("#MandatoryBillNo").hide();
    }

    if ($("#txtRefJobsheet").val() == "") {
        jAlert("Please enter ref jobsheet.", "Alert", function () {
            $("#txtRefJobsheet").focus();
            return
        });
        suc = false;
        return
    }

    if (cddlTechnician.GetValue() == "0") {
        jAlert("Please select assign to.", "Alert", function () {
            cddlTechnician.Focus();
            suc = false;
            return
        });
        return
    }

    if ($("#txtEntityCode").val() == "") {
        jAlert("Please enter Entity Code.", "Alert", function () {
            $("#txtEntityCode").focus();
            return
        });
        suc = false;
        return
    }

    if ($("#txtNetworkName").val() == "") {
        jAlert("Please enter Network Name.", "Alert", function () {
            $("#txtNetworkName").focus();
            return
        });
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

    if ($("#txtContactNumber").val() == "") {
        jAlert("Please Enter Contact Number.", "Alert", function () {
            setTimeout(function () {
                $("#txtContactNumber").focus();
                suc = false;
                return
            }, 200);
        });
        suc = false;
        return
    }


    if ($("#txtDeviceNumber").val() == "") {
        jAlert("Please enter serial number.", "Alert", function () {
            $("#txtDeviceNumber").focus();
            suc = false;
            return
        });
        suc = false;
        return
    }

    if (cDiviceTyp.GetValue() == "0") {
        jAlert("Please select device type.", "Alert", function () {
            cDiviceTyp.Focus();
            suc = false;
            return
        });
        return
    }

    if ($("#txtDeviceModel").val() == "") {
        jAlert("Please enter model.", "Alert", function () {
            $("#txtDeviceModel").focus();
            return
        });
        suc = false;
        return
    }

    if (cddlProblem.GetText() == "OTHER") {
        if ($("#txtOtherProblem").val() == "") {
            jAlert("Please enter other problem.", "Alert", function () {
                $("#txtOtherProblem").focus();
                return
            });
            suc = false;
            return
        }
    }

    if (cddlServiceAction.GetValue() == "0") {
        jAlert("Please select service action.", "Alert", function () {
            cddlServiceAction.Focus();
            suc = false;
            return
        });
        return
    }

    var Component = "";//gridComponentLookup.gridView.GetSelectedKeysOnPage();
    var Components = "";
    //for (var i = 0; i < Component.length; i++) {
    //    Components += ',' + Component[i];
    //}

    if (cddlServiceAction.GetText() == "Repaired") {
        if (gridComponentLookup.GetText() == "") {
            jAlert("Please Component.", "Alert", function () {
                cddlServiceAction.Focus();
                suc = false;
                return
            });
            return
        }
    }

    var BilableValue = "NO";
    var IsBilable = 0;
    if ($('#chkBillable').is(':checked') == true) {
        IsBilable = 1;
        BilableValue = "YES";
    }

    var Warrenty = null;
    if ($("#txtWarranty").val() != "") {
        var warsplit = $("#txtWarranty").val().split('-');
        Warrenty = warsplit[2] + '-' + warsplit[1] + '-' + warsplit[0];
    }

    var com_qty = [];
    gridComponentLookup.gridView.GetSelectedFieldValues("ID", function (val) {
        Component = val
        for (var i = 0; i < Component.length; i++) {
            Components += ',' + Component[i];
        }

        if (ComponentidwithQty != "") {
            for (var i = 0; i < ComponentidwithQty.length; i++) {
                if ($('#' + ComponentidwithQty[i].Productid).val() != "" && $('#' + ComponentidwithQty[i].Productid).val() > 0) {
                    com_qty.push({ id: ComponentidwithQty[i].Productid, Value: $('#' + ComponentidwithQty[i].Productid).val() });
                }
                else {
                    jAlert("Quantity must be entered.", "Alart", function () {
                        LoadingPanel.Hide();
                        $("#detailsModalComponent").modal('show');
                    });

                    suc = false;
                    return
                }
            }
        }
        else {
            for (var i = 0; i < Component.length; i++) {
                com_qty.push({ id: Component[i], Value: '0' });
            }
        }

        var model = {
            SerialNumber: $("#txtDeviceNumber").val(),
            DeviceType: cDiviceTyp.GetText(),
            DeviceTypeID: cDiviceTyp.GetValue(),
            Model: $("#txtDeviceModel").val(),
            Problem: cddlProblem.GetText(),
            ProblemID: cddlProblem.GetValue(),
            Other: $("#txtOtherProblem").val(),
            ServiceAction: cddlServiceAction.GetText(),
            ServiceActionID: cddlServiceAction.GetValue(),
            Components: gridComponentLookup.GetText(),
            ComponentsID: Components,
            Warranty: $("#txtWarranty").val(),
            ReturnReason: cddlReturnReason.GetText(),
            ReturnReasonID: cddlReturnReason.GetValue(),
            Remarks: $("#txtDetailsRemarks").val(),
            Billable: BilableValue,
            Guid: Gu_id,
            JobsheetID: $("#hdnJobSheetID").val(),
            DetailsID: $("#hdnDetailsID").val(),
            TechnicianID: cddlTechnician.GetValue(),
            com_qty: com_qty
        }

        $.ajax({
            type: "POST",
            url: "jobsheetEntry.aspx/AddData",
            data: "{model:" + JSON.stringify(model) + "}",
            contentType: "application/json; charset=utf-8",
            async: false,
            dataType: "json",
            success: function (msg) {
                var list = msg.d;
                if (msg.d.split('~')[1] == "Success") {
                    $("#txtDeviceNumber").val('');
                    cDiviceTyp.SetValue(1);
                    $("#txtDeviceModel").val('');
                    cddlProblem.SetValue(0);
                    $("#txtOtherProblem").val('');
                    cddlServiceAction.SetValue(0);
                    $("#txtWarranty").val('');
                    cddlReturnReason.SetValue(0);
                    $("#txtDetailsRemarks").val('');
                    $('#chkBillable').prop('checked', false);
                    $(".date").datepicker('update', new Date());
                    gridComponentLookup.gridView.UnselectRows();
                    $('#hdnGuid').val('');
                    cGrdDevice.PerformCallback();
                    ComponentidwithQty = "";
                }

                jAlert(msg.d.split('~')[0], "Alert", function () {
                    setTimeout(function () {
                        $("#txtDeviceNumber").focus();
                    }, 200);

                });
            }
        });
    });
}


function ClickOnEdit(val) {
    $.ajax({
        type: "POST",
        url: "jobsheetEntry.aspx/EditData",
        data: JSON.stringify({ HiddenID: val }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var list = msg.d;
            $("#hdnDetailsID").val(msg.d.DetailsID);;
            $('#hdnGuid').val(val);
            $("#txtDeviceNumber").val(msg.d.SerialNumber);
            cDiviceTyp.SetValue(msg.d.DeviceTypeID);
            $("#txtDeviceModel").val(msg.d.Model);
            cddlProblem.SetValue(msg.d.ProblemID);
            $("#txtOtherProblem").val(msg.d.Other);
            cddlServiceAction.SetValue(msg.d.ServiceActionID);
            $("#txtWarranty").val(msg.d.Warranty);
            cddlReturnReason.SetValue(msg.d.ReturnReasonID);
            $("#txtDetailsRemarks").val(msg.d.Remarks);
            if (msg.d.Billable == "YES") {
                $('#chkBillable').prop('checked', true);
            }
            else {
                $('#chkBillable').prop('checked', false);
            }
            if (msg.d.ServiceAction == "Repaired") {
                cComponentPanel.PerformCallback('SetComponentGrid' + '~' + msg.d.ComponentsID + '~' + msg.d.Model);
            }
            else {

            }
            $(".date").datepicker('update', msg.d.Warranty);



            cGrdDevice.PerformCallback();

        }
    });
}

function OnClickDelete(val) {
    jConfirm('Confirm Delete?', 'Alert', function (r) {
        if (r) {
            $.ajax({
                type: "POST",
                url: "jobsheetEntry.aspx/DeleteData",
                data: JSON.stringify({ HiddenID: val, JobSheetID: $("#hdnJobSheetID").val(), TechnicianID: cddlTechnician.GetValue() }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;
                    //cGrdDevice.Refresh();
                    jAlert(msg.d.split('~')[0]);
                    //  $("#HoliDayDetail").modal('toggle');
                    cGrdDevice.PerformCallback();

                }
            });
        }
        else {

        }
    });
}

function Component_GotFocus() {
    gridComponentLookup.gridView.Refresh();
}


var ComponentidwithQty = "";
function JobSheetEntryAdd() {
    var suc = true;
    var Gu_id = $('#hdnGuid').val();

    if (cCmbScheme.GetValue() == "0") {
        jAlert("Please select Numbering Scheme.", "Alert", function () {
            cCmbScheme.Focus();
            return
        });
        suc = false;
        return
    }

    if ($("#txtDocumentNumber").val() == "") {
        $("#MandatoryBillNo").show();
        return
    }
    else {
        $("#MandatoryBillNo").hide();
    }

    if ($("#txtRefJobsheet").val() == "") {
        jAlert("Please enter ref jobsheet.", "Alert", function () {
            $("#txtRefJobsheet").focus();
            return
        });
        suc = false;
        return
    }

    if (cddlTechnician.GetValue() == "0") {
        jAlert("Please select assign to.", "Alert", function () {
            cddlTechnician.Focus();
            suc = false;
            return
        });
        return
    }

    if ($("#txtEntityCode").val() == "") {
        jAlert("Please enter Entity Code.", "Alert", function () {
            $("#txtEntityCode").focus();
            return
        });
        suc = false;
        return
    }

    if ($("#txtNetworkName").val() == "") {
        jAlert("Please enter Network Name.", "Alert", function () {
            $("#txtNetworkName").focus();
            return
        });
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

    if ($("#txtContactNumber").val() == "") {
        jAlert("Please Enter Contact Number.", "Alert", function () {
            setTimeout(function () {
                $("#txtContactNumber").focus();
                suc = false;
                return
            }, 200);
        });
        suc = false;
        return
    }


    if ($("#txtDeviceNumber").val() == "") {
        jAlert("Please enter serial number.", "Alert", function () {
            $("#txtDeviceNumber").focus();
            suc = false;
            return
        });
        suc = false;
        return
    }

    if (cDiviceTyp.GetValue() == "0") {
        jAlert("Please select device type.", "Alert", function () {
            cDiviceTyp.Focus();
            suc = false;
            return
        });
        return
    }

    if ($("#txtDeviceModel").val() == "") {
        jAlert("Please enter model.", "Alert", function () {
            $("#txtDeviceModel").focus();
            return
        });
        suc = false;
        return
    }

    if (cddlProblem.GetText() == "OTHER") {
        if ($("#txtOtherProblem").val() == "") {
            jAlert("Please enter other problem.", "Alert", function () {
                $("#txtOtherProblem").focus();
                return
            });
            suc = false;
            return
        }
    }

    if (cddlServiceAction.GetValue() == "0") {
        jAlert("Please select service action.", "Alert", function () {
            cddlServiceAction.Focus();
            suc = false;
            return
        });
        return
    }

    if (cddlServiceAction.GetText() == "Repaired") {
        if (gridComponentLookup.GetText() == "") {
            jAlert("Please Component.", "Alert", function () {
                cddlServiceAction.Focus();
                suc = false;
                return
            });
            return
        }
    }

    var BilableValue = "NO";
    var IsBilable = 0;
    if ($('#chkBillable').is(':checked') == true) {
        IsBilable = 1;
        BilableValue = "YES";
    }

    var Warrenty = null;
    if ($("#txtWarranty").val() != "") {
        var warsplit = $("#txtWarranty").val().split('-');
        Warrenty = warsplit[2] + '-' + warsplit[1] + '-' + warsplit[0];
    }

    var ComponentIDs = "";
    var Components = "";
    if ($("#hdnComponentQty").val() == "Yes") {
        gridComponentLookup.gridView.GetSelectedFieldValues("ID", function (val) {
            ComponentIDs = val
            if (ComponentIDs != "") {
                for (var i = 0; i < ComponentIDs.length; i++) {
                    if (Components == "") {
                        Components = ComponentIDs[i];
                    }
                    else {
                        Components += ',' + ComponentIDs[i];
                    }
                }
                $('#dataTableComponent').DataTable().destroy();
                $('#dataTableComponent').DataTable().destroy();

                $.ajax({
                    type: "POST",
                    url: "jobsheetEntry.aspx/ShowComponentQty",
                    data: JSON.stringify({ ComponentID: Components, hiddenid: Gu_id, serialno: $("#txtDeviceNumber").val(), EntryMode: $("#hdnEntryMode").val() }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        if (msg.d != "") {
                            ComponentidwithQty = msg.d;
                            $("#detailsModalComponent").modal('show');
                            $('#dataTableComponent').DataTable().destroy();
                            //$('#dataTableComponent').DataTable().fnDestroy();
                            var list = msg.d;
                            var status = " <table id='dataTableComponent' class='table tableStyle table-striped table-bordered display nowrap' style='width: 100%'>";
                            status = status + " <thead><tr>";
                            status = status + " <th class='hide'>Product id</th><th>Product Code</th><th>Product Name</th><th>Replaceable</th><th>Quantity</th></tr></thead>";
                            status = status + " </table>";
                            $('#ComponentTable').html(status);

                            $('#dataTableComponent').DataTable({
                                retrieve: true,
                                "ordering": false,
                                fixedColumns: {
                                    rightColumns: 1
                                },
                                data: msg.d,
                                columns: [
                                    { 'data': 'Productid' },
                                    { 'data': 'ProductCode' },
                                    { 'data': 'ProductName' },
                                    { 'data': 'Replaceable' },
                                    { 'data': 'TEXTBOX' },
                                ],
                                "columnDefs": [
                                {
                                    "visible": false,
                                    "targets": 0
                                }
                                ]
                            });
                        }
                        else {
                            AddDevice();
                        }
                    }
                });
            }
            else {
                AddDevice();
            }
        });
    }
    else {
        AddDevice();
    }
}


function ComponentQty_Submit() {
    AddDevice();
}

function validateFloatKeyPress(el, evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    var number = el.value.split('.');
    if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    if (number.length > 1 && charCode == 46) {
        return false;
    }
    return true;
}

// Mantis Issue 24412
function get_NetworkName() {
    $.ajax({
        type: "POST",
        url: "jobsheetEntry.aspx/GetNetworkName",
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
// Mantis Issue 24413/24417
function selModel_change() {
    var selModel = cselModel.GetText();
    if (selModel != "") {
        $("#txtDeviceModel").val(selModel);
        cselModel.SetValue(0);
    }

}
// End of Mantis Issue 24413/24417