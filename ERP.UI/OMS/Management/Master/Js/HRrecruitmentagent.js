
function ClickVIewInfo(keyValue) {
    debugger;
            var url = '/OMS/management/master/View/ViewVendor.html?id=' + keyValue;
            CAspxDirectCustomerViewPopup.SetWidth(window.screen.width - 50);
            CAspxDirectCustomerViewPopup.SetHeight(window.innerHeight - 70);
            CAspxDirectCustomerViewPopup.SetContentUrl(url);
            //AspxDirectAddCustPopup.ClearVerticalAlignedElementsCache();

            CAspxDirectCustomerViewPopup.RefreshContentUrl();
            CAspxDirectCustomerViewPopup.Show();
        }

function OnContactInfoClick(keyValue) {
    var url = 'insurance_contactPerson.aspx?id=' + keyValue;
    //OnMoreInfoClick(url, "Name : " + name + "", '940px', '450px', 'Y');
    window.location.href = url;
}

function ClickOnMoreInfo(keyValue) {
    var url = 'HRrecruitmentagent_general.aspx?id=' + keyValue;
    //OnMoreInfoClick(url, 'Modify Agent Details', '940px', '450px', 'Y');
    window.location.href = url;
}
function callback() {
    grid.PerformCallback();
}
function OnAddButtonClick() {
    debugger;
    //var url = 'HRrecruitmentagent_general.aspx?id=' + 'ADD';            
    //window.location.href = url;

    var isLighterPage = $("#hidIsLigherContactPage").val();
    // alert(isLighterPage);
    if (isLighterPage == 1) {
        var url = '/OMS/management/Master/vendorPopup.html?var=2.0';
        // var url = '/OMS/management/Master/VendorMasterPagePopup.html?var=1.6';
        // alert(url);
        AspxDirectAddCustPopup.SetContentUrl(url);
        AspxDirectAddCustPopup.RefreshContentUrl();
        AspxDirectAddCustPopup.Show();
    }
    else {
        var url = 'HRrecruitmentagent_general.aspx?id=' + 'ADD';
        window.location.href = url;

    }



}
function getbacktolisting() {
    //setTimeout(function () { window.location.href = "/OMS/management/Master/HRrecruitmentagent.aspx?requesttype=VendorService" }, 400);
    grid.Refresh();
}

function ValidatePanno() {
    debugger;
    $("#hdnflag").val("0");
    var PAN = ctxtNumber.GetText().toUpperCase();
    var panPat = /^([a-zA-Z]{5})(\d{4})([a-zA-Z]{1})$/;
    var code = /([C,P,H,F,A,T,B,L,J,G])/;
    var code_chk = PAN.substring(3, 4);
    if (PAN != "") {
        if (PAN.search(panPat) == -1) {
            $("#hdnflag").val("1");

            return false;
        }
        if (code.test(code_chk) == false) {
            $("#hdnflag").val("1");
            return false;
        }

    }
}
function ParentCustomerOnClose(newCustId, CustomerName, CustUniqueName, BillingStateText, BillingStateCode, ShippingStateText, ShippingStateCode) {
    AspxDirectAddCustPopup.Hide();
    var url = 'HRrecruitmentagent.aspx?requesttype=VendorService';
    window.location.href = url;

}

function ShowHideFilter(obj) {
    grid.PerformCallback(obj);
}

function OnDelete(keyValue) {
    jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
        if (r == true) {
            grid.PerformCallback('Delete~' + keyValue);
        }
    });
}
function ShowError(obj) {
    if (grid.cpDelete != null) {
        if (grid.cpDelete == 'Success') {
            jAlert('Deleted Successfully');
            grid.cpDelete = null;
        }
        else {
            jAlert('Used in other module.Can not delete');
            grid.cpDelete = null;
        }
        grid.Refresh();
    }
}
function gridRowclick(s, e) {
    $('#EmployeeGrid').find('tr').removeClass('rowActive');
    $('.floatedBtnArea').removeClass('insideGrid');
    //$('.floatedBtnArea a .ico').css({ 'opacity': '0' });
    $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea').addClass('insideGrid');
    $(s.GetRow(e.visibleIndex)).addClass('rowActive');
    setTimeout(function () {
        //alert('delay');
        var lists = $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a');
        //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a .ico').css({'opacity': '1'});
        //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a').each(function (e) {
        //    setTimeout(function () {
        //        $(this).fadeIn();
        //    }, 100);
        //});    
        $.each(lists, function (index, value) {
            //console.log(index);
            //console.log(value);
            setTimeout(function () {
                console.log(value);
                $(value).css({ 'opacity': '1' });
            }, 100);
        });
    }, 200);
}
        
//Chinmoy added start 02-04-2020 	
function UniqueCodeCheck() {
    debugger;
    var SchemeVal = $("#ddl_numberingScheme").val();
    var NoSchemeId = SchemeVal.toString().split('~')[0];
    if (SchemeVal == "0") {
        jAlert('Please Select Numbering Scheme');
        //ctxt_SlOrderNo.SetValue('');	
        //ctxt_SlOrderNo.Focus();	
    }
        //if (NoSchemeId == "0")	
    else {
        var CheckUniqueCode = false;
        var uccName = "";
        var Type = "";
        if (($("#hdnAutoNumStg").val() == "DVNumb1") && ($("#hdnTransactionType").val() == "DV")) {
            uccName = ctxt_CustDocNo.GetText();
            Type = "MasterVendor";
        }
        $.ajax({
            type: "POST",
            url: "HRrecruitmentagent_general.aspx/CheckUniqueNumberingCode",
            data: JSON.stringify({ uccName: uccName, Type: Type }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                CheckUniqueCode = msg.d;
                if (CheckUniqueCode == true) {
                    jAlert('Please enter Unique ID.');
                    //jAlert('Please enter unique Sales Order No');	
                    ctxt_CustDocNo.SetValue('');
                    ctxt_CustDocNo.Focus();
                }
            }
        });
    }
}
//End



//Rev Rajdip

function AboutPanClick() {
    cpopuppan.Show();
}
//End Rev Rajdip



        
    $(document).ready(function () {
        //debugger;
        if (sessionStorage.getItem("popupcopyvendor") == "") {
            setTimeout(function () { CASPxDirectCustomerCopyToVendorPopup.Hide() },200);
            sessionStorage.setItem("popupcopyvendor", "");
        }
        //Rev 24507
        /// setTimeout(function () { CASPxDirectCustomerCopyToVendorPopup.Show() },500); to    setTimeout(function () { CASPxDirectCustomerCopyToVendorPopup.Show() },3000);
        if (sessionStorage.getItem("popupcopyvendor") == "vendorcopy") {
            setTimeout(function () { CASPxDirectCustomerCopyToVendorPopup.Show() },3000);
            sessionStorage.setItem("popupcopyvendor", "");
        }
        //End Rev Bapi

        if ($('body').hasClass('mini-navbar')) {
            var windowWidth = $(window).width();
            var cntWidth = windowWidth - 90;
            grid.SetWidth(cntWidth);
        } else {
            var windowWidth = $(window).width();
            var cntWidth = windowWidth - 220;
            grid.SetWidth(cntWidth);
        }
        $('.navbar-minimalize').click(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                grid.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                grid.SetWidth(cntWidth);
            }

        });
        setTimeout(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                grid.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                grid.SetWidth(cntWidth);
            }
        }, 1000);

        //chinmoy add for numbering scheme start	
        $('#ddl_numberingScheme').change(function () {
            //	
            var NoSchemeTypedtl = $(this).val();
            var NoSchemeId = NoSchemeTypedtl.toString().split('~')[0];
            var NoSchemeType = NoSchemeTypedtl.toString().split('~')[1];
            $("#hdnNumberingId").val(NoSchemeId);
            var schemeLength = NoSchemeTypedtl.toString().split('~')[2];
            if (NoSchemeType == '1') {
                ctxt_CustDocNo.SetText('Auto');
                ctxt_CustDocNo.SetEnabled(false);
                $('#txt_CustDocNo input').attr('maxlength', schemeLength);
                $("#hddnDocNo").val('Auto');
            }
            else if (NoSchemeType == '0') {
                schemeLength = 80;
                ctxt_CustDocNo.SetText("");
                ctxt_CustDocNo.SetEnabled(true);
                $('#txt_CustDocNo input').attr('maxlength', schemeLength);
            }
            else if ($('#ddl_numberingScheme').val() == "0") {
                ctxt_CustDocNo.SetText("");
                ctxt_CustDocNo.SetEnabled(true);
            }
        });
        //End	

    });

//REV RAJDIP
function InvalidUDF() {
    jAlert("Udf is mandatory.", "Alert", function () { OpenUdf(); });

}
function validateGSTIN() {
    $("#myInput").removeClass("hide");
    $("#myInput").val($("#txtGSTIN1_I").val().trim() + $("#txtGSTIN2_I").val().trim() + $("#txtGSTIN3_I").val().trim());
    CopyFunction();


    window.open('https://services.gst.gov.in/services/searchtp');
}

function CopyFunction() {
    var copyText = document.getElementById("myInput");
    copyText.select();
    document.execCommand("copy");
    $("#myInput").addClass("hide");
}

function branchGridEndCallBack() {

    if (cbranchGrid.cpReceviedString) {
        if (cbranchGrid.cpReceviedString == 'SetAllRecordToDataTable') {
            cBranchSelectPopup.Hide();
        }
    }

    if (cbranchGrid.cpBrselected) {

        if (cbranchGrid.cpBrselected == '1') {
            jAlert("Individual branch selection not allowed when all branch option is checked.");
            cbranchGrid.cpBrselected = null;
            cBranchSelectPopup.Show();
        }
        else { cbranchGrid.cpBrselected = null; }
    }

    if (cbranchGrid.cpBrChecked) {
        if (cbranchGrid.cpBrChecked == '1') {
            $('#lblBranch>').attr('style', 'display:inline');
            $('#chkAllBranch>').prop('checked', true)
            cbranchGrid.cpBrChecked = null;
            $('#hdnBranchAllSelected>').val('0');
        }
        else {
            $('#lblBranch>').attr('style', 'display:none');
            $('#chkAllBranch>').prop('checked', false)
            cbranchGrid.cpBrChecked = null;
            $('#hdnBranchAllSelected>').val('1');
        }
    }
}

function ClearSelectedBranch() {
    cbranchGrid.PerformCallback('ClearSelectedBranch');
}

function SelectAllBranches(e) {

    if (e.checked == true) {

        ClearSelectedBranch();
        $('#hdnBranchAllSelected>').val('0');
        $('#lblBranch>').attr('style', 'display:inline');

    }
    else {
        $('#hdnBranchAllSelected>').val('1');
        $('#lblBranch>').attr('style', 'display:none');

    }
}




function CmbBranchChanged() {
    var branchCode = CmbBranch.GetValue();
    if (branchCode == 0) {
        $('#MultiBranchButton').show();
    }
    else {
        $('#MultiBranchButton').hide();
    }
}

function MultiBranchClick() {
    cbranchGrid.PerformCallback('SetAllSelectedRecord');
    cBranchSelectPopup.Show();
}

function SaveSelectedBranch() {
    cbranchGrid.PerformCallback('SetAllRecordToDataTable');
}

function selectAll() {

    cbranchGrid.PerformCallback('SelectAllBranchesFromList');
    cBranchSelectPopup.Show();
}
function unselectAll() {
    cbranchGrid.PerformCallback('ClearSelectedBranch');
    cBranchSelectPopup.Show();
}




//Gstin settings


//Chinmoy edited start 07//5/2018

function changeFunc() {
    debugger;
    var MainAccount_val2 = document.getElementById("lstTaxRates_MainAccount").value;
    if (MainAccount_val2 === "0") {
        if (document.getElementById('hdIsMainAccountInUse').value == "IsInUse") {
            jAlert("Transaction exists for the selected Ledger. Cannot proceed.");
            Changeselectedvalue();
        }
        else {
            document.getElementById("hndTaxRates_MainAccount_hidden").value = document.getElementById("lstTaxRates_MainAccount").value;
            ChangeSubSource();
        }
    }
    else {
        if (document.getElementById('hdIsMainAccountInUse').value == "IsInUse") {
            jAlert("Transaction exists for the selected Ledger. Cannot proceed.");
            Changeselectedvalue();
        } else {

            document.getElementById("hndTaxRates_MainAccount_hidden").value = document.getElementById("lstTaxRates_MainAccount").value;
            ChangeSubSource();
        }
    }

}


//Chinmoy edited end 07//5/2018

function changeSubFunc() {
    document.getElementById("hndTaxRates_SubAccount_hidden").value = document.getElementById("lstTaxRates_SubAccount").value;
}
//function ListBind() {
//    debugger;
//    var config = {
//        '.chsn': {},
//        '.chsn-deselect': { allow_single_deselect: true },
//        '.chsn-no-single': { disable_search_threshold: 10 },
//        '.chsn-no-results': { no_results_text: 'Oops, nothing found!' },
//        '.chsn-width': { width: "100%" }
//    }
//    for (var selector in config) {
//        $(selector).chosen(config[selector]);
//    }

//}

function ChangeSubSource() {
    var fname = "%";
    var mainAccount = document.getElementById("hndTaxRates_MainAccount_hidden").value;;
    var lReportTo = $('select[id$=lstTaxRates_SubAccount]');
    lReportTo.empty();

    $.ajax({
        type: "POST",
        url: "HRrecruitmentagent_general.aspx/GetSubAccountList",
        data: JSON.stringify({ reqStr: fname, mainreqStr: mainAccount }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var list = msg.d;
            var listItems = [];
            if (list.length > 0) {

                for (var i = 0; i < list.length; i++) {
                    var id = '';
                    var name = '';
                    id = list[i].split('|')[1];
                    name = list[i].split('|')[0];
                    $('#lstTaxRates_SubAccount').append($('<option>').text(name).val(id));
                }

                $(lReportTo).append(listItems.join(''));
                ListBind();
                $('#lstTaxRates_SubAccount').trigger("chosen:updated");
                $('#lstTaxRates_SubAccount').trigger("chosen:updated");
            }
            else {
                $('#lstTaxRates_SubAccount').trigger("chosen:updated");
            }
        }
    });
}

function Changeselectedvalue() {
    var lstTaxRates_MainAccount = document.getElementById("lstTaxRates_MainAccount");
    if (document.getElementById("hndTaxRates_MainAccount_hidden").value != '') {
        for (var i = 0; i < lstTaxRates_MainAccount.options.length; i++) {
            if (lstTaxRates_MainAccount.options[i].value == document.getElementById("hndTaxRates_MainAccount_hidden").value) {
                lstTaxRates_MainAccount.options[i].selected = true;
            }
        }
        $('#lstTaxRates_MainAccount').trigger("chosen:updated");
    }
}
function ChangeSourceMainAccount() {
    debugger;
    var fname = "%";
    var lReportTo = $('select[id$=lstTaxRates_MainAccount]');
    lReportTo.empty();

    $.ajax({
        type: "POST",
        url: "HRrecruitmentagent_general.aspx/GetMainAccountList",
        data: JSON.stringify({ reqStr: fname }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var list = msg.d;
            var listItems = [];
            if (list.length > 0) {
                $('#lstTaxRates_MainAccount').empty().append('<option selected="selected" value="0">None</option>');//Subhabrata
                for (var i = 0; i < list.length; i++) {
                    var id = '';
                    var name = '';
                    id = list[i].split('|')[1];
                    name = list[i].split('|')[0];

                    $('#lstTaxRates_MainAccount').append($('<option>').text(name).val(id));
                }

                $(lReportTo).append(listItems.join(''));

                $('#lstTaxRates_MainAccount').trigger("chosen:updated");
                Changeselectedvalue();
            }
            else {
                $('#lstTaxRates_MainAccount').trigger("chosen:updated");
            }
        }
    });
}




//Debjyoti Code for UDF
function OpenUdf() {
    if (document.getElementById('IsUdfpresent').value == '0') {
        jAlert("UDF not define.");
    }
    else {
        var keyVal = document.getElementById('Keyval_internalId').value;
        var url = '/OMS/management/Master/frm_BranchUdfPopUp.aspx?Type=DV&&KeyVal_InternalID=' + keyVal;
        popup.SetContentUrl(url);
        popup.Show();
    }
    return true;
}


//Debjyoti Code for GSTIN 060217
function fn_AllowonlyNumeric(s, e) {
    var theEvent = e.htmlEvent || window.event;
    var key = theEvent.keyCode || theEvent.which;
    var keychar = String.fromCharCode(key);
    if (key == 9 || key == 37 || key == 38 || key == 39 || key == 40 || key == 8) { //tab/ Left / Up / Right / Down Arrow, Backspace, Delete keys
        return;
    }
    var regex = /[0-9]/;

    if (!regex.test(keychar)) {
        theEvent.returnValue = false;
        if (theEvent.preventDefault)
            theEvent.preventDefault();
    }
}
function Gstin2TextChanged(s, e) {

    if (!e.htmlEvent.ctrlKey) {
        if (e.htmlEvent.key != 'Control') {
            s.SetText(s.GetText().toUpperCase());
        }
    }

}

$(document).ready(function () {
    //debugger;
    grid.Refresh();
    grid.Refresh();

    $('#lblBranch>').attr('style', 'display:none');
    if ($("#radioregistercheck").find(":checked").val() == '1') {

        // alert(1);
        ctxtGSTIN111.SetEnabled(true);
        ctxtGSTIN222.SetEnabled(true);
        ctxtGSTIN333.SetEnabled(true);
        cApplicableFrom.SetEnabled(true);
    }
    else {
        // alert(2);
        ctxtGSTIN111.SetText('');
        ctxtGSTIN222.SetText('');
        ctxtGSTIN333.SetText('');

        ctxtGSTIN111.SetEnabled(false);
        ctxtGSTIN222.SetEnabled(false);
        ctxtGSTIN333.SetEnabled(false);
        cApplicableFrom.SetEnabled(false);

    }

    ListBind();
    ChangeSource();
    ChangeSourceMainAccount();

    $('body').on('click', '#radioregistercheck', function () {
        var optionText = $('#radioregistercheck> input:checked').val();
        if (optionText == '1') {
            // $("#spanmandategstn").attr('style', 'display:inline;color:red');
            //alert(1);
            ctxtGSTIN111.SetEnabled(true);
            ctxtGSTIN222.SetEnabled(true);
            ctxtGSTIN333.SetEnabled(true);
            cApplicableFrom.SetEnabled(true);
        }
        else {
            //alert(2);
            ctxtGSTIN111.SetText('');
            ctxtGSTIN222.SetText('');
            ctxtGSTIN333.SetText('');

            ctxtGSTIN111.SetEnabled(false);
            ctxtGSTIN222.SetEnabled(false);
            ctxtGSTIN333.SetEnabled(false);
            cApplicableFrom.SetEnabled(false);
            // $("#spanmandategstn").attr('style', 'display:none;');
        }
    });




});
function hidepopup()
{
    debugger;
    sessionStorage.setItem('popupcopyvendor', "");
}
function validate() {
    debugger;
    var fName = ctxtFirstName.GetText();
    var sName = ctxtCode.GetText();
    var retValue = true;
    var validateFlag = true;


    if (($("#hdnAutoNumStg").val() == "DVNumb1") && ($("#hdnTransactionType").val() == "DV")) {
        ctxtCode.SetText("ABC");
        $("#hddnDocNo").val(ctxt_CustDocNo.GetText());
        if (($("#ddl_numberingScheme").val() == "0")) {
            jAlert("Please Select Numbering Scheme.");
            retValue = false;
            return false;
        }
        else if (ctxt_CustDocNo.GetText() == "") {
            jAlert("Please Enter Unique ID.");
            retValue = false;
            return false;
        }
    }
    if (($("#hdnAutoNumStg").val() == "DVNumb0") && ($("#hdnTransactionType").val() == "")) {
        if (sName.trim() == "") {
            $('#MandatoryShortName').css({ 'display': 'block' });
            retValue = false;
        } else {
            $('#MandatoryShortName').css({ 'display': 'none' });
        }
    }


    if (fName.trim() == "") {
        $('#MandatoryName').css({ 'display': 'block' });
        retValue = false;
    }
    else {
        $('#MandatoryName').css({ 'display': 'none' });
    }
    //if (sName.trim() == "") {
    //    $('#MandatoryShortName').css({ 'display': 'block' });
    //    retValue = false;
    //} else {
    //    $('#MandatoryShortName').css({ 'display': 'none' });
    //}

    //Debjyoti 060217
    $('#invalidGst').css({ 'display': 'none' });
    var gst1 = ctxtGSTIN111.GetText().trim();
    var gst2 = ctxtGSTIN222.GetText().trim();
    var gst3 = ctxtGSTIN333.GetText().trim();

    if (gst1.length == 0 && gst2.length == 0 && gst3.length == 0) {
        var isregistered = $('#radioregistercheck> input:checked').val();
        if (isregistered == 1) {
            jAlert('GSTIN is mandatory.');
            retValue = false;
        }

    }
    else {


        if (gst1.length != 2 || gst2.length != 10 || gst3.length != 3) {
            $('#invalidGst').css({ 'display': 'block' });
            retValue = false;
        }


        var panPat = /^([a-zA-Z]{5})(\d{4})([a-zA-Z]{1})$/;
        var panPat1 = /^([a-zA-Z]{4})(\d{5})([a-zA-Z]{1})$/;
        var code = /([C,P,H,F,A,T,B,L,J,G])/;
        var code_chk = gst2.substring(3, 4);
        if (gst2.search(panPat) == -1 && gst2.search(panPat1)) {
            $('#invalidGst').css({ 'display': 'block' });
            retValue = false;
        }
        if (code.test(code_chk) == false) {
            $('#invalidGst').css({ 'display': 'block' });
            retValue = false;
        }


        //Subhabrata
        var isregisteredCheck = $('#radioregistercheck> input:checked').val();
        var finalGST = (gst1 + gst2 + gst3);
        var GSTINOldval = $("#hddnGSTIN2Val").val();


                  
        if (cApplicableFrom.GetDate() == null && $("#hddnApplicationMode").val() == "Edit" && GSTINOldval.trim() != finalGST.trim()) {
            if (isregisteredCheck == "1") {
                jAlert("Please enter Applicable from.");
                retValue = false;
                validateFlag = false;
            }
        }
      


            if (GSTINOldval.trim() !== finalGST.trim() && validateFlag == true && $("#hddnApplicationMode").val() === "Edit") {
                if (isregisteredCheck == "1") {
                    var r = confirm("You have entered GSTIN Applicable date.Based on the applicable date all the transaction will be updated with entered GSTIN.\nAre you sure?");
                    if (r == true) {
                        retValue = true;
                    }
                    else {
                        retValue = false;
                    }
                    $("#hddnGSTINFlag").val("UPDATE");
                }
            }
    else {
        $("#hddnGSTINFlag").val("NotUPDATE");
    }




    //End



}






//End Here Debjyoti 060217
return retValue;
}

function disp_prompt(name) {
    //console.log(name);
    if (name == "tab0") {
        //alert(name);
        document.location.href = "HRrecruitmentagent_general.aspx";
    }
    if (name == "tab1") {
        //alert(name);
        document.location.href = "HRrecruitmentagent_ContactPerson.aspx";
    }
    else if (name == "tab2") {
        //alert(name);
        document.location.href = "HRrecruitmentagent_Correspondence.aspx";
    }
    else if (name == "tab3") {
        //alert(name);
        document.location.href = "HRrecruitmentagent_BankDetails.aspx";
    }

    else if (name == "tab4") {
        document.location.href = "HRrecruitmentagent_Document.aspx";
    }
    else if (name == "tab6") {
        document.location.href = "HRrecruitmentagent_GroupMember.aspx";

    }
    else if (name == "tab5") {
        //alert(name);
        document.location.href = "HRrecruitmentagent_Registration.aspx";
    }
    else if (name == "tab7") {
        document.location.href = "vendors_tds.aspx";
    }

}

function UniqueCheck() {
    var code = 0;
    if (GetObjectID('HdId').value != '') {
        code = GetObjectID('HdId').value;
    }
    var reminderShortName = ctxtCode.GetText();
    if (reminderShortName.trim() == '')
        return;
    var CheckUniqueCode = false;
    $.ajax({
        type: "POST",
        url: "HRrecruitmentagent_general.aspx/CheckUniqueCode",
        data: JSON.stringify({ CategoriesShortCode: reminderShortName, Code: code }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            CheckUniqueCode = msg.d;
            if (CheckUniqueCode != true) {
                jAlert('Unique ID already exists.');
                ctxtCode.SetText('');
                ctxCode.Focus();
            }
        }
    });
}

function CallList(obj1, obj2, obj3) {
    if (obj1.value == "") {
        obj1.value = "%";
    }

    var obj4 = document.getElementById('cmbSource>');
    var obj5 = obj4.value;


    if (obj5 != '18') {
        ajax_showOptionsTEST(obj1, obj2, obj3, obj5);
        if (obj1.value == "%") {
            obj1.value = "";
        }
    }
}
function ListBind() {

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
function lstReferedBy() {

    // $('#lstReferedBy').chosen();
    $('#lstReferedBy').fadeIn();
}

function switchRefferedControl(obj) {
    debugger;
    switch (obj) {
        case "1":
            document.getElementById("refferByDD").value = false;
            $('#txtReferedBy').css({ 'display': 'block' });
            $('#lstReferedBy_chosen').css({ 'display': 'none' });
            return false;
            break;
        case "2":
            document.getElementById("refferByDD").value = false;
            $('#txtReferedBy').css({ 'display': 'block' });
            $('#lstReferedBy_chosen').css({ 'display': 'none' });
            return false;
            break;
        case "5":
            document.getElementById("refferByDD").value = false;
            $('#txtReferedBy').css({ 'display': 'block' });
            $('#lstReferedBy_chosen').css({ 'display': 'none' });
            return false;
            break;
        case "6":
            document.getElementById("refferByDD").value = false;
            $('#txtReferedBy').css({ 'display': 'block' });
            $('#lstReferedBy_chosen').css({ 'display': 'none' });
            return false;
            break;
        case "7":
            document.getElementById("refferByDD").value = false;
            $('#txtReferedBy').css({ 'display': 'block' });
            $('#lstReferedBy_chosen').css({ 'display': 'none' });
            return false;
            break;
        case "9":
            document.getElementById("refferByDD").value = false;
            $('#txtReferedBy').css({ 'display': 'block' });
            $('#lstReferedBy_chosen').css({ 'display': 'none' });
            return false;
            break;
        case "11":
            document.getElementById("refferByDD").value = false;
            $('#txtReferedBy').css({ 'display': 'block' });
            $('#lstReferedBy_chosen').css({ 'display': 'none' });
            return false;
            break;
        case "12":
            document.getElementById("refferByDD").value = false;
            $('#txtReferedBy').css({ 'display': 'block' });
            $('#lstReferedBy_chosen').css({ 'display': 'none' });
            return false;
            break;
        case "13":
            document.getElementById("refferByDD").value = false;
            $('#txtReferedBy').css({ 'display': 'block' });
            $('#lstReferedBy_chosen').css({ 'display': 'none' });
            return false;
            break;
        case "15":
            document.getElementById("refferByDD").value = false;
            $('#txtReferedBy').css({ 'display': 'block' });
            $('#lstReferedBy_chosen').css({ 'display': 'none' });
            return false;
            break;
        case "16":
            document.getElementById("refferByDD").value = false;
            $('#txtReferedBy').css({ 'display': 'block' });
            $('#lstReferedBy_chosen').css({ 'display': 'none' });
            return false;
            break;

        case "17":
            document.getElementById("refferByDD").value = false;
            $('#txtReferedBy').css({ 'display': 'block' });
            $('#lstReferedBy_chosen').css({ 'display': 'none' });
            return false;
            break;
        case "18":
            document.getElementById("refferByDD").value = false;
            $('#txtReferedBy').css({ 'display': 'block' });
            $('#lstReferedBy_chosen').css({ 'display': 'none' });
            return false;
            break;
        case "0":
            //dd
            document.getElementById("refferByDD").value = true;
            $('#txtReferedBy').css({ 'display': 'none' });
            $('#lstReferedBy_chosen').css({ 'display': 'block' });
            return true;
            break;
        case "3":
            //dd
            document.getElementById("refferByDD").value = true;
            $('#txtReferedBy').css({ 'display': 'none' });
            $('#lstReferedBy_chosen').css({ 'display': 'block' });
            return true;
            break;
        case "4":
            //dd
            document.getElementById("refferByDD").value = true;
            $('#txtReferedBy').css({ 'display': 'none' });
            $('#lstReferedBy_chosen').css({ 'display': 'block' });
            return true;
            break;
        case "8":
            //dd
            document.getElementById("refferByDD").value = true;
            $('#txtReferedBy').css({ 'display': 'none' });
            $('#lstReferedBy_chosen').css({ 'display': 'block' });
            return true;
            break;
        case "10":
            //dd
            document.getElementById("refferByDD").value = true;
            $('#txtReferedBy').css({ 'display': 'none' });
            $('#lstReferedBy_chosen').css({ 'display': 'block' });
            return true;
            break;
        case "14":
            document.getElementById("refferByDD").value = true;
            $('#txtReferedBy').css({ 'display': 'none' });
            $('#lstReferedBy_chosen').css({ 'display': 'block' });
            return true;
            break;
        case "20":
            //dd
            document.getElementById("refferByDD").value = true;
            $('#txtReferedBy').css({ 'display': 'none' });
            $('#lstReferedBy_chosen').css({ 'display': 'block' });
            return true;
            break;
        case "24":
            //dd
            document.getElementById("refferByDD").value = true;
            $('#txtReferedBy').css({ 'display': 'none' });
            $('#lstReferedBy_chosen').css({ 'display': 'block' });
            return true;
            break;
        case "25":
            //dd
            document.getElementById("refferByDD").value = true;
            $('#txtReferedBy').css({ 'display': 'none' });
            $('#lstReferedBy_chosen').css({ 'display': 'block' });
            return true;
            break;
        default:
            //dd
            document.getElementById("refferByDD").value = true;
            $('#txtReferedBy').css({ 'display': 'none' });
            $('#lstReferedBy_chosen').css({ 'display': 'block' });
            return true;
            break;
    }
}

function ChangeSource() {
    debugger;
    var InterId = "";
    if (document.getElementById("cmbSource").value) {
        InterId = document.getElementById("cmbSource").value;
    }
    var id = document.getElementById("cmbSource").value;
    if (switchRefferedControl(id)) {
        var lReferBy = $('select[id$=lstReferedBy]');
        lReferBy.empty();
        $.ajax({
            type: "POST",
            url: "HRrecruitmentagent_general.aspx/GetReffer",
            data: JSON.stringify({ sourceId: id, hdKeyValIntId: InterId }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var list = msg.d;
                var listItems = [];
                if (list.length > 0) {

                    for (var i = 0; i < list.length; i++) {
                        var id = '';
                        var name = '';
                        id = list[i].split('|')[1];
                        name = list[i].split('|')[0];

                        listItems.push('<option value="' +
                        name + '">' + name
                        + '</option>');
                    }

                    $(lReferBy).append(listItems.join(''));

                    lstReferedBy();
                    $('#lstReferedBy').trigger("chosen:updated");

                    if (switchRefferedControl(document.getElementById("cmbSource").value)) {
                        var valRefferBy = document.getElementById('RefferedByValue').value;

                        if (valRefferBy) {
                            var refferByChosen = document.getElementById("lstReferedBy");

                            for (var i = 0; i < refferByChosen.options.length; i++) {
                                if (refferByChosen.options[i].value == valRefferBy) {
                                    refferByChosen.options[i].selected = true;
                                }
                            }
                            $('#lstReferedBy').trigger("chosen:updated");
                            document.getElementById('RefferedByValue').value = null;
                        }
                    }

                }
                else {
                    debugger;
                    jAlert("No records found");
                    lstReferedBy();
                    $('#lstReferedBy').trigger("chosen:updated");
                }
            }
        });
    }
}

FieldName = 'cmbLegalStatus';
//END REV RAJDIP
