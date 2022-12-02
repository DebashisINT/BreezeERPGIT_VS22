
function CheckLength() {
    // var textbox = document.getElementById('txtDescription').value;
    // alert(textbox.length);
    /* if (textbox.length >= 150) {
         alert("Here");
        return false;
    }
     else {
        return true;
     }*/

}
function TdsSectionChanged(s, e) {
    document.getElementById('txtCode').value = s.GetValue();



    if ($('#txtCode').val() != '') {
        $('#tdsshortname').attr('style', 'display:none');
        var TDSTCSId = 0;
        if (GetObjectID('hddnVal').value != '') {
            TDSTCSId = GetObjectID('hddnVal').value;
        }

        var TDSTCSCode = $('#txtCode').val();
        $.ajax({
            type: "POST",
            url: "frm_TdsTcsPopUp.aspx/CheckUniqueName",
            data: JSON.stringify({ TDSTCSId: TDSTCSId, TDSTCSCode: TDSTCSCode }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var data = msg.d;
                if (data == true) {
                    //jjAlert("Please enter unique name"); 
                    jAlert("Section already exists");
                    cTdsSection.SetValue('0');
                    $('#txtCode').val('');
                    $('#txtCode').focus();
                    //document.getElementById("Popup_Empcitys_ctxtPro_Code_I").focus();
                    //document.getElementById("txtPro_Code_I").focus();
                    return false;
                }
            }

        });

    }
}


//function keyVal(obj) {
//    var mainSubLedger = obj.split('~')
//    SubledgerType = mainSubLedger[0, 2];
//    if (mainSubLedger[0, 1].toUpperCase() != 'NONE') {
//        document.getElementById('Label1').style.display = 'inline';
//        document.getElementById('lstSubAccount').style.display = 'inline';
//    }
//    else {
//        document.getElementById('Label1').style.display = 'none';
//        document.getElementById('lstSubAccount').style.display = 'none';
//    }
//}





function myFunction() {
    //if (document.getElementById('ddlType').value == 'TCS') {
    if (document.getElementById('ddlType').value == '1') {
        //$('#divsubAC').css({ 'display': 'block' });
        $('#divTCSPayable').css({ 'display': 'block' });
        var tdstype = $('#ddlType option:selected').val();
        var mainacvalue = $("#lstMainAccount").val();
        if (mainacvalue != '' && mainacvalue != null && mainacvalue != '0') {
            BindSubAccountList(mainacvalue);
        }

        $("#lblTdsTcsSection").html("TCS Section: ");
        $("#lblPostingLedger").html("Interim TCS Posting:");
        $("#lblSubAccount").html("TCS Payable:");

        $('#divCalculationBasedOn').css({ 'display': 'block' });

        $('#divInterestLedger').css({ 'display': 'none' });
        $('#divLateFeeLedger').css({ 'display': 'none' });
        $('#divOthersLedger').css({ 'display': 'none' });
        $('#divPurchase').css({ 'display': 'block' });
        // Rev Mantis Issue 24161
        $('#divAdvanceLedger').css({ 'display': 'none' });
        // End of Rev Mantis Issue 24161

        $('#divSales').css({ 'display': 'none' });
        BindTdsDetailsList("TCS");
        BindApplicableforTypeWise("TCS");

        $("#divOldTds").addClass("hidden");
        $("#divNewTDS").removeClass("hidden");
    }
    else {
        //document.getElementById('Label1').style.display = 'none';
        //document.getElementById('lstSubAccount').style.display = 'none';
        $('#divsubAC').css({ 'display': 'none' });
        $('#divTCSPayable').css({ 'display': 'none' });
        $('#divPurchase').css({ 'display': 'none' });

        $('#divSales').css({ 'display': 'block' });

        $("#lblTdsTcsSection").html("TDS Section: ");
        $("#lblPostingLedger").html("TDS Posting Ledger: ");
        $("#lblSubAccount").html("Sub Account:");

        $('#divCalculationBasedOn').css({ 'display': 'block' });

        $('#divInterestLedger').css({ 'display': 'block' });
        $('#divLateFeeLedger').css({ 'display': 'block' });
        $('#divOthersLedger').css({ 'display': 'block' });
        // Mantis Issue 24161
        $('#divAdvanceLedger').css({ 'display': 'block' });
        // End of Rev Mantis Issue 24161

        BindTdsDetailsList("TDS");
        BindApplicableforTypeWise("TDS");

        if ($("#hdnOldTDS").val() == "No") {
            $("#divOldTds").addClass("hidden");
            $("#divNewTDS").removeClass("hidden");
        }
        else {
            $("#divOldTds").removeClass("hidden");
            $("#divNewTDS").addClass("hidden");
        }
    }
}


function BindTdsDetailsList(Type) {
    var OtherDetails = {}
    OtherDetails.Type = Type;
    $.ajax({
        type: "POST",
        url: "frm_TdsTcsPopUp.aspx/GetTdsDetailsList",
        data: JSON.stringify(OtherDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            var returnObject = msg.d;
            if (returnObject) {
                SetDataSourceOnComboBox(cTdsSection, returnObject);
            }
        }
    });
}

function BindApplicableforTypeWise(Type) {
    var OtherDetails = {}
    OtherDetails.Type = Type;
    $.ajax({
        type: "POST",
        url: "frm_TdsTcsPopUp.aspx/BindApplicableforTypeWise",
        data: JSON.stringify(OtherDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            var returnObject = msg.d;
            if (returnObject) {
                SetDataSourceOnComboBox(caspxDeducteesNew, returnObject);
            }
        }
    });
}

function SetDataSourceOnComboBox(ControlObject, Source) {
    ControlObject.ClearItems();
    for (var count = 0; count < Source.length; count++) {
        ControlObject.AddItem(Source[count].Name, Source[count].Id);
    }
    ControlObject.SetSelectedIndex(0);
}


//function Page_Load() {
//    document.getElementById('Label1').style.display = 'none';
//    $('#lstSubAccount').prop('visible', false).trigger("chosen:updated");
//    //document.getElementById('lstSubAccount').style.display = 'none';
//}


function call_ajax1(obj1, obj2, obj3) {
    ajax_showOptions(obj1, obj2, obj3, 'Employees', 'Sub');
}
//function call_ajax1(obj1, obj2, obj3) {
//     var mainSubLedger = obj.split('~')
//    SubledgerType = mainSubLedger[0, 2];
//    ajax_showOptions(obj1, obj2, obj3, 'Employees', 'Sub');
//}
function ShowInsert(obj) {
    var objInser = obj.split('~')
    if (objInser[0, 0] == 'Insert') {
        jAlert('Saved Successfully');
        document.getElementById('hddnVal').value = objInser[0, 1];
        //window.location.href = "IframeTdsTcs.aspx";
    }
    else if (objInser[0, 0] == 'Error')
        jAlert('This Code Already Exists');
    else if (objInser[0, 0] == 'Update') {
        jAlert('Saved Successfully');
        //window.location.href = "IframeTdsTcs.aspx";
    }
}
function btnSave_Click() {
    var Mainacdtl = "";
    Mainacdtl = $("#lstMainAccount").val();

    var MainAcType = "";
    if ($("#txtCode").val().trim() != "") {
        if ($("#lstMainAccount").val() != null && $("#lstMainAccount").val() != '0') {
            MainAcType = Mainacdtl.split('~')[1];
            $('#tdsshortname').attr('style', 'display:none');
            $('#mainacname').attr('style', 'display:none');
            if ($("#ddlType").val() == '0') {
                comboInsert.PerformCallback();
            }
            else {
                //if (MainAcType != 'None') {
                //if ($("#lstSubAccount").val() != '0' && $("#lstSubAccount").val() != null) {
                if ($("#lstTCSPayable").val() != '0' && $("#lstTCSPayable").val() != null) {
                    if ($("#ddlCalculationBasedOn").val() != '0' && $("#ddlCalculationBasedOn").val() != null) {
                        comboInsert.PerformCallback();
                    }
                    else {
                        alert("Please select Calculation Based On");
                        return false;
                    }
                }
                else {
                    $('#subacname').attr('style', 'display:block');
                    return false;
                }
            }
            //mainSubLedger[0, 2];


        }
        else {
            $('#mainacname').attr('style', 'display:block');
            return false;
            //jAlert("Please enter Main Account name");
        }
        //comboInsert.PerformCallback();
    }
    else {
        $('#tdsshortname').attr('style', 'display:block');
        return false;
    }


}
function ForEdit() {

    //$('#divsubAC').css({ 'display': 'block' });

    $('#divTCSPayable').css({ 'display': 'block' });
    $('#divCalculationBasedOn').css({ 'display': 'block' });

    $('#divInterestLedger').css({ 'display': 'none' });
    $('#divLateFeeLedger').css({ 'display': 'none' });
    $('#divOthersLedger').css({ 'display': 'none' });
    $('#divPurchase').css({ 'display': 'block' });
    // Rev Mantis Issue 24161
    $('#divAdvanceLedger').css({ 'display': 'none' });
    // End of Rev Mantis Issue 24161


    //document.getElementById('Label1').style.display = 'inline';
    //document.getElementById('txtSubAccount').style.display = 'inline';
    //document.getElementById('lstSubAccount').style.display = 'inline';

}
function OnAddButtonClick() {
    var Hddata = document.getElementById('hddnVal').value;
    if (Hddata != '') {
        gridTdsTcs.SetEnabled(true);
        gridTdsTcs.AddNewRow();
    }
    else
        jAlert('Cannot Proceed. Enter TDS/TCS details first to proceed with this entry.');
}
function OnAddButtonNewClick() {
    var Hddata = document.getElementById('hddnVal').value;
    if (Hddata != '') {
        $("#HoliDayDetail").modal('toggle');
        ctxtSurcharge.SetValue("");
        ctxtEduCess.SetValue("");
        ctxtHgrEduCess.SetValue("");
        ctxtApplicableAmount.SetValue("");
        //caspxDeducteesNew.SetValue(1);
        cCmbRouding.SetValue(0);
        ctxtRate.SetValue("");
        $("#TDSTCSRates_ID").val("");
    }
    else
        jAlert('Cannot Proceed. Enter TDS/TCS details first to proceed with this entry.');
}
function ShowDateEn(obj) {

    var today = new Date();
    var tomorrow = new Date(today);

    var objShow = obj.split('~');
    if (objShow[0, 1] == '1') {

        gridTdsTcs.GetEditor("TDSTCSRates_DateFrom").SetEnabled(false);
    }
    else if (objShow[0, 1] == '2') {
        // var date = new Date();
        // var newDate = new Date(date.getYear(), date.getMonth(), date.getDate() + 1, 10, 0, 0);

        gridTdsTcs.GetEditor("TDSTCSRates_DateFrom").SetValue(today);
    }
}
function Back(path) {

    window.location.href = path;
}
FieldName = 'btnCancel';


// For Chosen DropDown by sam on 20122016
$(document).ready(function () {
    $('#lstMainAccount').chosen({ allow_single_deselect: true });

    $('#lstInterestLedger').chosen({ allow_single_deselect: true });
    //Rev Mantis Issue 24161
    $('#lstAdvanceLedger').chosen({ allow_single_deselect: true });
    // End of Rev Mantis Issue 24161
    $('#lstLateFeeLedger').chosen({ allow_single_deselect: true });
    $('#lstOthersLedger').chosen({ allow_single_deselect: true });
    //$('#lstMainAccount').trigger("chosen:updated");
    $('#lstSubAccount').chosen({ allow_single_deselect: true });

    $('#lstPurchase').chosen({ allow_single_deselect: true });
    $('#lstTCSPayable').chosen({ allow_single_deselect: true });

    $('#lstSales').chosen({ allow_single_deselect: true });
    //end  For Chosen DropDown by sam on 20122016



    if (document.getElementById('ddlType').value == '1') {
        $("#lblTdsTcsSection").html("TCS Section: ");
        $("#lblPostingLedger").html("Interim TCS Posting:");
        $("#lblSubAccount").html("TCS Payable:");
        $('#divPurchase').css({ 'display': 'block' });

        $('#divSales').css({ 'display': 'none' });
    }
    else {

        $("#lblTdsTcsSection").html("TDS Section: ");
        $("#lblPostingLedger").html("TDS Posting Ledger: ");
        $("#lblSubAccount").html("Sub Account:");
        $('#divPurchase').css({ 'display': 'none' });

        $('#divSales').css({ 'display': 'block' });
    }
    $('#divCalculationBasedOn').css({ 'display': 'block' });
});


$(document).ready(function () {
    $('#txtCode').blur(function () {
        if ($('#txtCode').val() != '') {
            $('#tdsshortname').attr('style', 'display:none');
            var TDSTCSId = 0;
            if (GetObjectID('hddnVal').value != '') {
                TDSTCSId = GetObjectID('hddnVal').value;
            }

            var TDSTCSCode = $('#txtCode').val();
            $.ajax({
                type: "POST",
                url: "frm_TdsTcsPopUp.aspx/CheckUniqueName",
                data: JSON.stringify({ TDSTCSId: TDSTCSId, TDSTCSCode: TDSTCSCode }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var data = msg.d;
                    if (data == true) {
                        //jjAlert("Please enter unique name"); 
                        jAlert("Please enter unique name");
                        $('#txtCode').val('');
                        $('#txtCode').focus();
                        //document.getElementById("Popup_Empcitys_ctxtPro_Code_I").focus();
                        //document.getElementById("txtPro_Code_I").focus();
                        return false;
                    }
                }

            });

        }
    })
})
//<%--// .............................Code Above Commented and Added by Sam on 21122016...................................... --%>


$(function () {
    $('#lstMainAccount').change(function () {

        var values = "";
        var MainAcType = "";
        values = $('#lstMainAccount option:selected').val();
        MainAcType = values.split('~')[1];
        if (values != '0') {
            $('#mainacname').attr('style', 'display:none');
        }
        else {
            BindSubAccountList(0);
            $('#mainacname').attr('style', 'display:block');
            return false;
        }

        var tdstype = $('#ddlType option:selected').val();
        if (tdstype == '1') {
            var mainacvalue = $("#lstMainAccount").val();
            if (mainacvalue != '' && mainacvalue != null && MainAcType != 'None') {
                BindSubAccountList(mainacvalue);
            } else {

                var lBox = $('select[id$=lstSubAccount]');
                lBox.empty();
                var lBox1 = $('select[id$=lstPurchase]');
                lBox1.empty();

                ListSubAccountBind();
                $('#lstSubAccount').trigger("chosen:updated");
                $('#lstSubAccount').prop('disabled', true).trigger("chosen:updated");
                $('#lstPurchase').trigger("chosen:updated");
                $('#lstPurchase').prop('disabled', true).trigger("chosen:updated");

            }
        }
        else {
            var mainacvalue = $("#lstMainAccount").val();
            if (mainacvalue != '' && mainacvalue != null && MainAcType != 'None') {
                BindTCSPayableList(mainacvalue);
            } else {
                var lBox1 = $('select[id$=lstSales]');
                lBox1.empty();

                $('#lstSales').trigger("chosen:updated");
                $('#lstSales').prop('disabled', true).trigger("chosen:updated");
            }
        }
    });
});

$(function () {
    $('#lstSubAccount').change(function () {
        var ddltype = "";

        ddltype = $('#ddlType option:selected').val();
        if (ddltype != '0') {
            var values = "";
            values = $('#lstSubAccount option:selected').val();
            if (values != '0') {
                $('#subacname').attr('style', 'display:none');
            }
            else {
                $('#subacname').attr('style', 'display:block');
                return false;
            }
        }
    });

    $('#lstPurchase').change(function () {
        var ddltype = "";

        ddltype = $('#ddlType option:selected').val();
        if (ddltype != '0') {
            var values = "";
            values = $('#lstPurchase option:selected').val();
            if (values != '0') {
                $('#puracname').attr('style', 'display:none');
            }
            else {
                $('#puracname').attr('style', 'display:block');
                return false;
            }
        }
    });

    $('#lstSales').change(function () {
        var ddltype = "";

        ddltype = $('#ddlType option:selected').val();
        if (ddltype != '0') {
            var values = "";
            values = $('#lstSales option:selected').val();
            if (values != '0') {
                $('#salesacname').attr('style', 'display:none');
            }
            else {
                $('#salesacname').attr('style', 'display:block');
                return false;
            }
        }
    });

    $('#lstTCSPayable').change(function () {
        var ddltype = "";

        ddltype = $('#ddlType option:selected').val();
        if (ddltype != '0') {
            var values = "";
            values = $('#lstTCSPayable option:selected').val();
            if (values != '0') {
                $('#TCSPayablename').attr('style', 'display:none');
            }
            else {
                $('#TCSPayablename').attr('style', 'display:block');
                return false;
            }
        }
    });
});

function BindSubAccountList(mainacvalue) {
    var ProcedureName = "SubAccountSelect";
    var InputName = "MainAccountID";
    var InputType = "V";

    var InputValue = mainacvalue;
    var CombinedQuery = ProcedureName + "$" + InputName + "$" + InputType + "$" + InputValue;
    var lBox = $('select[id$=lstSubAccount]');

    var lBox1 = $('select[id$=lstPurchase]');
    var lstSubAccountItems = [];
    //Customer or Lead radio button is clicked kaushik 21-11-2016

    lBox.empty();
    lBox1.empty();

    if (mainacvalue != '0') {
        $.ajax({
            type: "POST",
            url: 'frm_TdsTcsPopUp.aspx/GetSubAccountList',
            data: "{CombinedQuery:\"" + CombinedQuery + "\"}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var list = msg.d;

                if (list.length > 0) {

                    for (var i = 0; i < list.length; i++) {

                        var id = '';
                        var name = '';
                        id = list[i].split('|')[1];
                        name = list[i].split('|')[0];

                        lstSubAccountItems.push('<option value="' +
                        id + '">' + name
                        + '</option>');
                    }

                    $(lBox).append(lstSubAccountItems.join(''));
                    $(lBox1).append(lstSubAccountItems.join(''));
                    ListSubAccountBind();
                    $('#lstSubAccount').trigger("chosen:updated");
                    $('#lstSubAccount').prop('disabled', false).trigger("chosen:updated");
                    $('#lstPurchase').trigger("chosen:updated");
                    $('#lstPurchase').prop('disabled', false).trigger("chosen:updated");
                }
                else {
                    lBox.empty();
                    lBox1.empty();
                    ListSubAccountBind();
                    $('#lstSubAccount').trigger("chosen:updated");
                    $('#lstSubAccount').prop('disabled', true).trigger("chosen:updated");
                    $('#lstPurchase').trigger("chosen:updated");
                    $('#lstPurchase').prop('disabled', true).trigger("chosen:updated");
                }

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                jAlert(textStatus);
            }
        });
    } else {

        lBox.empty();
        lBox1.empty();
        ListSubAccountBind();
        $('#lstSubAccount').trigger("chosen:updated");
        $('#lstSubAccount').prop('disabled', true).trigger("chosen:updated");
        $('#lstPurchase').trigger("chosen:updated");
        $('#lstPurchase').prop('disabled', true).trigger("chosen:updated");

    }


}


function BindTCSPayableList(mainacvalue) {
    var ProcedureName = "SubAccountSelect";
    var InputName = "MainAccountID";
    var InputType = "V";

    var InputValue = mainacvalue;
    var CombinedQuery = ProcedureName + "$" + InputName + "$" + InputType + "$" + InputValue;
    var lBox = $('select[id$=lstTCSPayable]');
    var lBox1 = $('select[id$=lstPurchase]');
    var lstTCSPayableItems = [];
    //Customer or Lead radio button is clicked kaushik 21-11-2016

    lBox.empty();
    lBox1.empty();

    if (mainacvalue != '0') {
        $.ajax({
            type: "POST",
            url: 'frm_TdsTcsPopUp.aspx/GetSubAccountList',
            data: "{CombinedQuery:\"" + CombinedQuery + "\"}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var list = msg.d;

                if (list.length > 0) {

                    for (var i = 0; i < list.length; i++) {

                        var id = '';
                        var name = '';
                        id = list[i].split('|')[1];
                        name = list[i].split('|')[0];

                        lstTCSPayableItems.push('<option value="' +
                        id + '">' + name
                        + '</option>');
                    }

                    $(lBox).append(lstTCSPayableItems.join(''));
                    $(lBox1).append(lstTCSPayableItems.join(''));
                    ListTCSPayableBind();
                    $('#lstTCSPayable').trigger("chosen:updated");
                    $('#lstTCSPayable').prop('disabled', false).trigger("chosen:updated");

                    $('#lstSales').trigger("chosen:updated");
                    $('#lstSales').prop('disabled', false).trigger("chosen:updated");
                }
                else {
                    lBox.empty();
                    lBox1.empty();
                    ListTCSPayableBind();
                    $('#lstTCSPayable').trigger("chosen:updated");
                    $('#lstTCSPayable').prop('disabled', true).trigger("chosen:updated");

                    $('#lstSales').trigger("chosen:updated");
                    $('#lstSales').prop('disabled', true).trigger("chosen:updated");

                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                jAlert(textStatus);
            }
        });
    } else {

        lBox.empty();
        ListTCSPayableBind();
        $('#lstTCSPayable').trigger("chosen:updated");
        $('#lstTCSPayable').prop('disabled', true).trigger("chosen:updated");
    }


}

function ListSubAccountBind() {

    var config = {
        '.chsn': {},
        '.chsn-deselect': { allow_single_deselect: true },
        '.chsn-no-single': { disable_search_threshold: 10 },
        '.chsn-no-results': { no_results_text: 'Oops, nothing found!' },
        '.chsn-width': { width: "100%" }
    }
    for (var selector in config) {
        $(selector).chosen(config[selector]);
    }

}


function ListTCSPayableBind() {

    var config = {
        '.chsn': {},
        '.chsn-deselect': { allow_single_deselect: true },
        '.chsn-no-single': { disable_search_threshold: 10 },
        '.chsn-no-results': { no_results_text: 'Oops, nothing found!' },
        '.chsn-width': { width: "100%" }
    }
    for (var selector in config) {
        $(selector).chosen(config[selector]);
    }

}

$(document).ready(function () {
    $("#lstSubAccount").chosen().change(function () {
        var productId = $(this).val();

        $('#hdnSubACCode').val(productId);
        //jAlert(productId);


    })

    $("#lstPurchase").chosen().change(function () {
        var productId = $(this).val();

        $('#ltsPurchase_code').val(productId);
        //jAlert(productId);


    })

    $("#lstSales").chosen().change(function () {
        var productId = $(this).val();

        $('#ltsSales_code').val(productId);
        //jAlert(productId);


    })


    $("#lstTCSPayable").chosen().change(function () {
        var productId = $(this).val();

        $('#hdnSubACCode').val(productId);
        //jAlert(productId);


    })
})

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


function AddRate() {

    var Rate = ctxtRate.GetText();
    var Surcharge = (ctxtSurcharge.GetText() != "") ? ctxtSurcharge.GetText() : null;
    var EduCess = (ctxtEduCess.GetText() != "") ? ctxtEduCess.GetText() : null;
    var HgrEduCess = (ctxtHgrEduCess.GetText() != "") ? ctxtHgrEduCess.GetText() : null;
    var ApplicableAmount = (ctxtApplicableAmount.GetText() != "") ? ctxtApplicableAmount.GetText() : null;
    var ApplicableFor = caspxDeducteesNew.GetValue();
    var Code = cTdsSection.GetValue();
    var Rouding = cCmbRouding.GetValue();
    var ID = $("#TDSTCSRates_ID").val();
    var FromMainDate = (cFrmDateDetl.GetValue() != null) ? cFrmDateDetl.GetValue() : "";
    FromMainDate = GetDateFormat(FromMainDate);
    var Gu_id = $('#hdnGuid').val();
    var suc = true;

    if (FromMainDate == "") {
        suc = false;
        jAlert("Please Select From", "Alert", function () {
            setTimeout(function () {
                cFrmDateDetl.Focus();
                return
            }, 200);
        });
    }
    else {

        if ((ctxtRate.GetText() == "0.000" || ctxtRate.GetText() == "") && caspxDeducteesNew.GetValue()!="7") {
            jAlert("Please Enter Rate.")
            ctxtRate.Focus();
            suc = false;
            return
        }
        else {
            $.ajax({
                type: "POST",
                url: "frm_TdsTcsPopUp.aspx/save",
                data: JSON.stringify({ Rates_Code: Code, Rates_DateFrom: FromMainDate, Rates_Rate: Rate, Rates_Surcharge: Surcharge, Rates_EduCess: EduCess, Rates_HgrEduCess: HgrEduCess, Rates_ApplicableAmount: ApplicableAmount, Rates_ApplicableFor: ApplicableFor, Rates_Rouding: Rouding, Rates_ID: ID }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;
                    //cGrdHoliday.Refresh();
                    jAlert(msg.d);
                    if (msg.d != "Applicable for Already Exists.") {
                        $("#HoliDayDetail").modal('toggle');
                        ctxtSurcharge.SetValue("");
                        ctxtEduCess.SetValue("");
                        ctxtHgrEduCess.SetValue("");
                        ctxtApplicableAmount.SetValue("");
                        // caspxDeducteesNew.SetValue(1);
                        cCmbRouding.SetValue(0);
                        ctxtRate.SetValue("");
                        $("#TDSTCSRates_ID").val('');
                    }
                    gridTdsTcsNew.PerformCallback();

                }
            });
        }
    }
}

function OnClickDelete(val) {
    jConfirm('Confirm Delete?', 'Alert', function (r) {
        if (r) {
            $.ajax({
                type: "POST",
                url: "frm_TdsTcsPopUp.aspx/DeleteData",
                data: JSON.stringify({ HiddenID: val }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;
                    //cGrdHoliday.Refresh();
                    jAlert(msg.d);
                    //  $("#HoliDayDetail").modal('toggle');
                    gridTdsTcsNew.PerformCallback();

                }
            });
        }
        else {
            // alert("false");
        }
    });
}

function ClickOnEdit(val) {
    $.ajax({
        type: "POST",
        url: "frm_TdsTcsPopUp.aspx/EditData",
        data: JSON.stringify({ HiddenID: val }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {

            var list = msg.d;
            var dtstrt = new Date(parseInt(msg.d.TDSTCSRates_DateFrom.substr(6)));
            cFrmDateDetl.SetDate(dtstrt);
            ctxtRate.SetValue(msg.d.TDSTCSRates_Rate);
            ctxtSurcharge.SetValue(msg.d.TDSTCSRates_Surcharge);
            caspxDeducteesNew.SetValue(msg.d.TDSTCSRates_ApplicableFor);
            ctxtEduCess.SetValue(msg.d.TDSTCSRates_EduCess);
            ctxtHgrEduCess.SetValue(msg.d.TDSTCSRates_HgrEduCess);
            ctxtApplicableAmount.SetValue(msg.d.TDSTCSRates_ApplicableAmount);
            cCmbRouding.SetValue(msg.d.TDSTCSRates_Rouding);
            $("#TDSTCSRates_ID").val(msg.d.TDSTCSRates_ID);
            $("#TDSTCSRates_Code").val(msg.d.TDSTCSRates_Code);
            $("#HoliDayDetail").modal('toggle');
        }
    });
}