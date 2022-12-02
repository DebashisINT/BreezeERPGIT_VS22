
$(document).ready(function () {
    

    //chinmoy added start 02-04-2020	
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
            //SetDocMaxLength(schemeLength);	
            $('#txt_CustDocNo input').attr('maxlength', schemeLength);
            $("#hddnDocNo").val('Auto');
        }
        else if (NoSchemeType == '0') {

            schemeLength = 50;

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


            $(".water").each(function () {
                if ($(this).val() == this.title) {
                    $(this).addClass("opaque");
                }
            });

            $(".water").focus(function () {
                if ($(this).val() == this.title) {
                    $(this).val("");
                    $(this).removeClass("opaque");
                }
            });

            $(".water").blur(function () {
                if ($.trim($(this).val()) == "") {
                    $(this).val(this.title);
                    $(this).addClass("opaque");
                }
                else {
                    $(this).removeClass("opaque");
                }
            });

            $("#chkcopy").change(function () {
                if (document.getElementById("chkcopy").checked == true)
                {
                    $("#hdncopytoproduct").val("1");
                }

                else
                { $("#hdncopytoproduct").val("0"); }
            });

        });

    //chinmoy added  start  02-04-2020	
    function UniqueCodeCheck() {
        //debugger;
        var SchemeVal = $("#ddl_numberingScheme").val();
        var NoSchemeId = SchemeVal.toString().split('~')[0];
        if (SchemeVal == "0") {
            alert('Please Select Numbering Scheme');
            //ctxt_SlOrderNo.SetValue('');	
            //ctxt_SlOrderNo.Focus();	
        }
            //if (NoSchemeId == "0")	
        else {
            var CheckUniqueCode = false;
            var uccName = "";
            var Type = "";
            if (($("#hdnAutoNumStg").val() == "1") && ($("#hdnTransactionType").val() == "CL")) {
                uccName = ctxt_CustDocNo.GetText();
                Type = "Mastercustomerclient";
            }

            $.ajax({
                type: "POST",
                url: "Contact_general.aspx/CheckUniqueNumberingCode",
                data: JSON.stringify({ uccName: uccName, Type: Type }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    CheckUniqueCode = msg.d;
                    if (CheckUniqueCode == true) {
                        alert('Please enter unique No.');
                        //jAlert('Please enter unique Sales Order No');	
                        ctxt_CustDocNo.SetValue('');
                        ctxt_CustDocNo.Focus();
                    }
                }
            });
        }
    }
//End



function ClickVIewInfo(keyValue) {
    CAspxDirectCustomerViewPopup.SetWidth(window.screen.width - 50);
    CAspxDirectCustomerViewPopup.SetHeight(window.innerHeight - 70);
    var url = '/OMS/management/master/View/ViewCustomer.html?id=' + keyValue;
    CAspxDirectCustomerViewPopup.SetContentUrl(url);
    //AspxDirectAddCustPopup.ClearVerticalAlignedElementsCache();

    CAspxDirectCustomerViewPopup.RefreshContentUrl();
    CAspxDirectCustomerViewPopup.Show();
}

function ShowMissingData(obj, obj2) {
    var url = 'frmContactMissingData.aspx?id=' + obj;
    window.location.href = url;

}
function ShowError(obj) {

    if (grid.cpDelete != null) {
        if (grid.cpDelete == 'Success') {
            grid.Refresh();
            jAlert('Deleted Successfully');

            grid.cpDelete = null;
        }
        else {
            jAlert('Used in other module.Can not delete');
            grid.cpDelete = null;
        }

    }

    // height()
}
function NewPgae(cnt_id) {
    //alert('cnt_id');
}
function ClickOnMoreInfo(keyValue) {
    //var url = 'Contact_general.aspx?contact_type=' + '<%=Session["Contactrequesttype"]%>' + '&id=' + keyValue;
    var url = 'Contact_general.aspx?id=' + keyValue;
    window.location.href = url;
}
function ClickOnSegments(keyValue) {

    $.ajax({
        url: "CustomerMasterList.aspx/CheckSegmentPresentOrNot",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ CustomerID: keyValue }),
        datatype: "JSON",
        type: "POST",
        success: function (data) {
            var grpdetl = data.d;
            if (grpdetl > "0")
            {
                var url = 'DocumentSegmentList.aspx?id=' + keyValue;
                window.location.href = url;
            }
            else
            {
                jAlert("Segment is not defined for this Customer");
            }
            
        },
        error: function (data) {

            jAlert("Please try again later");
        }
    });
   
   
}

function OnAddButtonClick() {


    var isLighterPage = $("#hidIsLigherContactPage").val();
    // alert(isLighterPage);
    if (isLighterPage == 1) {


        var url = '/OMS/management/Master/customerPopup.html?v=70.5';

        
        // alert(url);
        AspxDirectAddCustPopup.SetContentUrl(url);
        AspxDirectAddCustPopup.RefreshContentUrl();
        AspxDirectAddCustPopup.Show();
    }
    else {
        var url = 'Contact_general.aspx?id=' + 'ADD';
        window.location.href = url;
    }
}
//Rev rajdip
function CopyToCustomer(keyValue) {
    //var url = 'Contact_general.aspx?id=' + keyValue;
    //window.location.href = url;
    //Group Bind
    $("#hdncustomerucc").val(keyValue);
    $.ajax({
        url: "View/Services/ViewCustomerService.asmx/getGroupmasterDetails",
        contentType: "application/json; charset=utf-8",
        datatype: "JSON",
        type: "POST",
        success: function (data) {

            $("#copytocustomerGroupCust").empty();
            var grpdetl = data.d;

            debugger;

            var opts = "";
            opts = "<option value='0'>Select</option>";
            for (i in grpdetl)
                //if (opts == "") {
                //    opts = "<option value='0'>Select</option>"
                //}
                opts += "<option value='" + grpdetl[i].Id + "'>" + grpdetl[i].Name + "</option>";

            $("#copytocustomerGroupCust").empty().append(opts);
            //$.each(grpdetl, function (Id, Name) {
            //    $('#GroupCust')
            //        .append($("<option></option>")
            //                   .attr("value", Id)
            //                   .text(Name));
            //});
            // for (var i = 0; i < grpdetl.length; i++) {
            //     var opt = new Option(grpdetl[i].Name);

            //    $("#GroupCust").append(opt);

            //     debugger;
            //}
        },
        error: function (data) {
            debugger;
            jAlert("Please try again later");
        }
    });
    //End Group Bind
    var BillingAddress = [];
    var objectToPass = {}
    objectToPass.keyValue = keyValue;
    $.ajax({
        type: "POST",
        url: "Contact_general.aspx/GetDataFromCustomer",
        data: JSON.stringify(objectToPass),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var data = msg.d;
            BillingAddress = data
            var BillingObj = $.grep(BillingAddress, function (e) { return e.add_addressType == "Billing"; })
            var ShippingObj = $.grep(BillingAddress, function (e) { return e.add_addressType == "Shipping";})
            var cnt_ucc = data[0].cnt_ucc;
            //rev srijeeta mantis issue 0024515
            var Alternative_Code = data[0].Alternative_Code;
            //end of rev srijeeta mantis issue 0024515
            var cnt_salutation = data[0].cnt_salutation;
            var cnt_firstName = data[0].cnt_firstName;
            var cnt_middleName = data[0].cnt_middleName;
            var cnt_lastName = data[0].cnt_lastName;
            var cnt_shortName = data[0].cnt_shortName;
            var cnt_branchId = data[0].cnt_branchId;
            var cnt_sex = data[0].cnt_sex;
            var cnt_maritalStatus = data[0].cnt_maritalStatus;
            var cnt_DOB = data[0].cnt_DOB;
            var cnt_anniversaryDate = data[0].cnt_anniversaryDate;
            var cnt_legalStatus = data[0].cnt_legalStatus;
            var cnt_education = data[0].cnt_education;
            var cnt_profession = data[0].cnt_profession;
            var cnt_organization = data[0].cnt_organization;
            var cnt_designation = data[0].cnt_designation;
            var cnt_industry = data[0].cnt_industry;
            var cnt_contactSource = data[0].cnt_contactSource;
            var cnt_referedBy = data[0].cnt_referedBy;
            var cnt_contactType = data[0].cnt_contactType;
            var cnt_contactStatus = data[0].cnt_contactStatus;
            var cnt_RegistrationDate = data[0].cnt_RegistrationDate;
            var cnt_rating = data[0].cnt_rating;
            var cnt_reason = data[0].cnt_reason;
            var cnt_bloodgroup = data[0].cnt_bloodgroup;
            var WebLogin = data[0].WebLogin;
            var cnt_placeofincorporation = data[0].cnt_placeofincorporation;
            var cnt_BusinessComncDate = data[0].cnt_BusinessComncDate;
            var cnt_OtherOccupation = data[0].cnt_OtherOccupation;
            var cnt_nationality = data[0].cnt_nationality;
            var cnt_IsCreditHold = data[0].cnt_IsCreditHold;
            var cnt_CreditDays = data[0].cnt_CreditDays;
            var cnt_CreditLimit = data[0].cnt_CreditLimit;
            var cnt_PrintNameToCheque = data[0].cnt_PrintNameToCheque;
            var EnteredDate = data[0].EnteredDate;
            var CNT_GSTIN = data[0].CNT_GSTIN;
            var cnt_IdType = data[0].cnt_IdType;
            var grp_groupMaster = data[0].grp_groupMaster;
            var TCSApplicable = data[0].TCSApplicable;
            //-----------------------------------------------------------------
            //Billing Address-------------------------------------------------
            if (BillingObj.length > 0) {
                var add_addressTypeBilling = BillingObj[0].add_addressType;
                var Billingadd_address1 = data[0].add_address1;
                var Billingadd_address2 = data[0].add_address2;
                var Billingadd_address3 = data[0].add_address3;
                var Billingadd_landMark = data[0].add_landMark;
                var Billingadd_country = data[0].add_country;
                var Billingadd_state = data[0].add_state;
                var Billingadd_pin = data[0].add_pin;
                var Billingadd_phone = data[0].add_phone;
                var Billingadd_Email = data[0].add_Email;
                var Billingadd_Website = data[0].add_Website;
                var Billingadd_designation = data[0].add_designation;
                var Billingadd_address4 = data[0].add_address4;
                var BillingDistance = data[0].add_address4;
                var BillingAdd_EcomId = data[0].Add_EcomId;
                var billingadd_city = data[0].add_city;
            }
            //Shipping address---------------------------------------------------------

                 
            if (ShippingObj.length > 0) {

                var add_addressTypeBilling = ShippingObj[0].add_addressType;
                var shippingadd_address1 = ShippingObj[0].add_address1;
                var shippingadd_address2 = ShippingObj[0].add_address2;
                var shippingadd_address3 = ShippingObj[0].add_address3;
                var shippingadd_landMark = ShippingObj[0].add_landMark;
                var shippingadd_country = ShippingObj[0].add_country;
                var shippingadd_state = ShippingObj[0].add_state;
                var shippingadd_pin = ShippingObj[0].add_pin;
                var shippingadd_phone = ShippingObj[0].add_phone;
                var shippingadd_Email = ShippingObj[0].add_Email;
                var shippingadd_Website = ShippingObj[0].add_Website;
                var shippingadd_designation = ShippingObj[0].add_designation;
                var shippingadd_address4 = ShippingObj[0].add_address4;
                var shippingDistance = ShippingObj[0].add_address4;
                var shippingAdd_EcomId = ShippingObj[0].Add_EcomId;
                var shippingadd_city = ShippingObj[0].add_city;
                var contactpersonforshipping = ShippingObj[0].contactpersonforshipping;
            }
            $("#copytocustomerIdentityTypeIdType").val(cnt_IdType)
            //rev srijeeta mantis issue 0024515
            //$("#copytocustomeruniqueId").val(cnt_ucc);
            $("#copytocustomeruniqueId").val("");
            
            $("#copytocustomeruniqueIdALTCODE").val(Alternative_Code);
            //end of rev srijeeta mantis issue 0024515
            $("#copytocustomerName").val(cnt_firstName);
            $("#copytocustomerGSTIN1").val(CNT_GSTIN.substring(0, 2));
            $("#copytocustomerGSTIN2").val(CNT_GSTIN.substring(2, 12));
            $("#copytocustomerGSTIN3").val(CNT_GSTIN.substring(12, 16));
            //----------------Billing-------------------------
            $("#copytocustomerbillingpinCode").val(Billingadd_pin);
            $("#copytobillingphnocustomer").val(Billingadd_phone);
            //$("#copytobillingphnocustomer").val(cnt_firstName);
            $("#copytocustomerBillingAddress1").val(Billingadd_address1);
            $("#copytocustomerBillingAddress2").val(Billingadd_address2);
            $("#copytocustomerbillingCountry").val(Billingadd_country);
            $("#copytocustomerbillingState").val(Billingadd_state);
            $("#copytocustomerCity").val(billingadd_city);
            //---------------Shipping---------------------------
            $("#copytocustomershippingcontactperson").val(contactpersonforshipping);
            $("#copytocustomershippingpinCode").val(shippingadd_pin);
            $("#copytocustomerShippingPhone").val(shippingadd_phone);
            $("#copytocustomershippingAddress1").val(shippingadd_address1);
            $("#copytocustomershippingAddress2").val(shippingadd_address2);
            $("#copytocustomershippingCountry").val(shippingadd_country);
            $("#copytocustomershippingState").val(shippingadd_state);
            $("#copytocustomershippingCity").val(shippingadd_city);
            $("#copytocustomerGroupCust").val(grp_groupMaster);
           
            if(TCSApplicable==true)
            {
                document.getElementById("chkTCSAppli").checked = true;
            }
            
            $("#hdnsex").val(cnt_sex);
            $("#hdndob").val(cnt_DOB);
            $("#hdnanniversarydate").val(cnt_anniversaryDate);
            CASPxDirectCustomerCopyToProductPopup.Show();
        }
    });
}
function BillingCheckChangeCopytoproduct() {
    debugger;
    if (document.getElementById('copytocustomerfancy-checkbox-successforcopy').checked) {
        document.getElementById('copytocustomerfancy-checkbox-successforcopy').checked = false;
        //ClearBillingforcopy();
        shippingCopyTobillingforcopy();
    }
    else {
        document.getElementById('copytocustomerfancy-checkbox-successforcopy').checked = true;
        //shippingCopyTobilling();
    }

}
function ShippingCheckChangeforcopy() {
    if (document.getElementById('fancy-checkbox-successShippingcopytoproductforcopy').checked) {
        document.getElementById('fancy-checkbox-successShippingcopytoproductforcopy').checked = false;
        //ClearBillingforcopy();
        shippingchange();
    }
    else {
        document.getElementById('fancy-checkbox-successShippingcopytoproductforcopy').checked = true;
        //shippingCopyTobillingforcopy();
    }

}
function shippingchange() {
    $("#copytocustomerBillingAddress1").val($("#copytocustomershippingAddress1").val());
    $("#copytocustomerBillingAddress2").val($("#copytocustomershippingAddress2").val());
    $("#copytocustomerbillingpinCode").val($("#copytocustomershippingpinCode").val());
    $("#copytocustomerCity").val($("#copytocustomershippingCity").val());
    $("#copytocustomerbillingState").val($("#copytocustomershippingState").val());
    $("#copytocustomerCity").val($("#copytocustomershippingCity").val());
    $("#copytobillingphnocustomer").val($("#copytocustomerShippingPhone").val());
}
function ClearBillingforcopy() {
    $("#copytocustomerBillingAddress1").val('');
    $("#copytocustomerBillingAddress2").val('');
    $("#copytocustomerbillingpinCode").val('');
    $("#copytocustomerbillingCountry").val('');
    $("#copytocustomerbillingState").val('');
    $("#copytocustomerCity").val('');
    $("#copytobillingphnocustomer").val('');
}

function shippingCopyTobillingforcopy() {

    $("#copytocustomershippingAddress1").val($("#copytocustomerBillingAddress1").val());
    $("#copytocustomershippingAddress2").val($("#copytocustomerBillingAddress2").val());
    $("#copytocustomershippingpinCode").val($("#copytocustomerbillingpinCode").val());
    $("#copytocustomershippingCountry").val($("#copytocustomerCity").val());
    $("#copytocustomershippingState").val($("#copytocustomerbillingState").val());
    $("#copytocustomershippingCity").val($("#copytocustomerCity").val());
    $("#copytocustomerShippingPhone").val($("#copytobillingphnocustomer").val());

}
function validGstin() {
    var GSTIN1 = $("#copytocustomerGSTIN1").val().trim();
    var GSTIN2 = $("#copytocustomerGSTIN2").val().trim();
    var GSTIN3 = $("#copytocustomerGSTIN3").val().trim();

    var returnval = true;

    if (GSTIN1.length == 0 && GSTIN2.length == 0 && GSTIN3.length == 0) {
    }
    else {
        if (GSTIN1.length != 2 || GSTIN2.length != 10 || GSTIN3.length != 3) {
            $('#invalidGst').css({ 'display': 'block' });
            returnval = false;
        }


        var panPat = /^([a-zA-Z]{5})(\d{4})([a-zA-Z]{1})$/;
        var code = /([C,P,H,F,A,T,B,L,J,G])/;
        var code_chk = GSTIN2.substring(3, 4);
        if (GSTIN2.search(panPat) == -1) {
            returnval = false;
        }
        if (code.test(code_chk) == false) {
            returnval = false;
        }
    }

    return returnval;

}
function BillingPinChange() {
    var pincode = $("#copytocustomerbillingpinCode").val();

    $.ajax({
        type: "POST",
        url: "Customer_general.aspx/GetAddressdetails",
        data: "{ pinCode: '" + pincode + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: "true",
        cache: "false",
        success: function (msg) {
            var obj = msg.d;
            if (obj.status == "Ok") {
                $("#copytocustomerbillingCountry").val(obj.Country);
                $("#copytocustomerbillingState").val(obj.state);
                $("#copytocustomerCity").val(obj.city);
                $("#copytocustomerbillingpinCode").removeClass('alert-danger');
            } else {
                $("#copytocustomerbillingCountry").val('');
                $("#copytocustomerbillingState").val('');
                $("#copytocustomerCity").val('');
                $("#copytocustomerbillingpinCode").addClass('alert-danger');
            }
        },
        Error: function (x, e) {
            // On Error
        }
    });


}
function shippingPinChange() {
    var pincode = $("#copytocustomershippingpinCode").val();

    $.ajax({
        type: "POST",
        url: "Customer_general.aspx/GetAddressdetails",
        data: "{ pinCode: '" + pincode + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: "true",
        cache: "false",
        success: function (msg) {
            var obj = msg.d;
            if (obj.status == "Ok") {
                $("#copytocustomershippingCountry").val(obj.Country);
                $("#copytocustomershippingState").val(obj.state);
                $("#copytocustomershippingCity").val(obj.city);
                $("#copytocustomershippingpinCode").removeClass('alert-danger');
            } else {
                $("#copytocustomershippingCountry").val('');
                $("#copytocustomershippingCity").val('');
                $("#copytocustomershippingCity").val('');
                $("#copytocustomershippingpinCode").addClass('alert-danger');
            }
        },
        Error: function (x, e) {
            // On Error
        }
    });


}
function SaveCustomercopy() {
    debugger;
    $("#copytocustomeruniqueId").removeClass('alert-danger');
    
    $("#copytocustomerName").removeClass('alert-danger');
    $("#copytocustomerBillingAddress1").removeClass('alert-danger');
    $("#copytocustomerbillingpinCode").removeClass('alert-danger');
    $("#copytocustomerBillingAddress1").removeClass('alert-danger');
    $("#copytocustomerbillingpinCode").removeClass('alert-danger');

    $("#copytocustomerGSTIN1").removeClass('alert-danger');
    $("#copytocustomerGSTIN2").removeClass('alert-danger');
    $("#copytocustomerGSTIN3").removeClass('alert-danger');

    var uniqueID = $("#copytocustomeruniqueId").val();
    //rev srijeeta mantis issue 0024515
    var Alternative_Code = $("#copytocustomeruniqueIdALTCODE").val();
    //end of rev srijeeta mantis issue 0024515
    var Name = $("#copytocustomerName").val();
    var BillingAddress1 = $("#copytocustomerBillingAddress1").val();
    var BillingAddress2 = $("#copytocustomerBillingAddress2").val();
    var pinCode = $("#copytocustomerbillingpinCode").val();
    var Country = $("#copytocustomerbillingCountry").val();

    var shippingAddress1 = $("#copytocustomershippingAddress1").val();
    var shippingAddress2 = $("#copytocustomershippingAddress2").val();
    var shippingpinCode = $("#copytocustomershippingpinCode").val();
    var shippingCountry = $("#copytocustomershippingCountry").val();



    var BillingPhone = $("#copytobillingphnocustomer").val().trim();
    var ShippingPhone = $("#copytocustomerShippingPhone").val().trim();
    var contactperson = $("#copytocustomershippingcontactperson").val().trim();

    var GrpCust = $("#copytocustomerGroupCust option:selected").val();
    var TCSApplicable = false;
    if (document.getElementById("chkTCSAppli").checked == true)
    {
        TCSApplicable = true;
    }
    //chinmoy ediited below code start	
    var uniqueCopyID = "";
    var NumberingId = "0";

    if (($("#hdnAutoNumStg").val() == "1") && ($("#hdnTransactionType").val() == "CL")) {

        $("#hddnDocNo").val(ctxt_CustDocNo.GetText());
        if (($("#ddl_numberingScheme").val() == "0")) {
            jAlert("Please Select Numbering Scheme.");

            return;
        }
        else if (ctxt_CustDocNo.GetText() == "") {
            jAlert("Please Enter Unique ID.");

            return;
        }
    }
    if (($("#hdnAutoNumStg").val() == "1") && ($("#hdnTransactionType").val() == "CL")) {
        uniqueCopyID = ctxt_CustDocNo.GetText();
        NumberingId = $("#hdnNumberingId").val();
    }
    else {
        uniqueCopyID = uniqueID;

    }
    if (($("#hdnAutoNumStg").val() == "0") && ($("#hdnTransactionType").val() == "")) {
        if (uniqueID.trim() == "") {
            $("#copytocustomeruniqueId").addClass('alert-danger');
            $("#copytocustomeruniqueId").focus();
            return;
        }
    }
    if (Name.trim() == "") {
        $("#copytocustomerName").addClass('alert-danger');
        $("#copytocustomerName").focus();
        return;
    }
    //end
    var IdType = document.getElementById("copytocustomerIdentityTypeIdType");
    var IdTypeval = IdType.options[copytocustomerIdentityTypeIdType.selectedIndex].value;

    //alert(IdTypeval);

    //chinmoy edited below code 02-04-2020 start	
    if (($("#hdnAutoNumStg").val() == "0") && ($("#hdnTransactionType").val() == "")) {
        if (IdTypeval == 1) {
            if (isNaN(uniqueID)) {
                jAlert("Please type valid Phone No.");
                return;
            }
        } else if (IdTypeval == 2) {
            var panPat = /^([a-zA-Z]{5})(\d{4})([a-zA-Z]{1})$/;
            var code = /([C,P,H,F,A,T,B,L,J,G])/;
            var code_chk = uniqueID.substring(3, 4);
            if (uniqueID.search(panPat) == -1) {
                jAlert("Please type valid PAN No.");
                return;
            }
            if (code.test(code_chk) == false) {
                jAlert("Please type valid PAN No.");
                return;
            }
        } else if (IdTypeval == 3) {
            if (isNaN(uniqueID) || uniqueID.length != 12) {
                jAlert("Please type valid Aadhar No.");
                return;
            }
        }
    }
    else if (($("#hdnAutoNumStg").val() == "1") && ($("#hdnTransactionType").val() == "CL")) {
        IdTypeval = "0";
    }
    //end

    var GSTIN1 = $("#copytocustomerGSTIN1").val().trim();
    var GSTIN2 = $("#copytocustomerGSTIN2").val().trim();
    var GSTIN3 = $("#copytocustomerGSTIN3").val().trim();

    var GSTIN = GSTIN1 + GSTIN2 + GSTIN3;
    //-----------------GSTIN Validation-------------

    if (GSTIN.length > 0) {
        if (GSTIN1.length != 2 || GSTIN2.length != 10 || GSTIN3.length != 3) {
            jAlert('Please enter valid GSTIN.');
            return;
        }

        var panPat = /^([a-zA-Z]{5})(\d{4})([a-zA-Z]{1})$/;
        var code = /([C,P,H,F,A,T,B,L,J,G])/;
        var code_chk = GSTIN2.substring(3, 4);
        if (GSTIN2.search(panPat) == -1) {
            jAlert('Please enter valid GSTIN.');
            return;
        }
        else if (code.test(code_chk) == false) {
            jAlert('Please enter valid GSTIN.');
            return;
        }

    }

    //-----------------GSTIN Validation END---------
    if (BillingAddress1.trim() == "") {
        $("#copytocustomerBillingAddress1").addClass('alert-danger');
        $("#copytocustomerBillingAddress1").focus();
        return;
    } else if (pinCode.trim() == "") {
        $("#copytocustomerbillingpinCode").addClass('alert-danger');
        $("#copytocustomerbillingpinCode").focus();
        return;
    } else if (Country.trim() == "") {
        $("#copytocustomerbillingpinCode").addClass('alert-danger');
        $("#copytocustomerbillingpinCode").focus();
        return;
    }

    else if (shippingAddress1.trim() == "") {
        $("#copytocustomerBillingAddress1").addClass('alert-danger');
        $("#copytocustomerBillingAddress1").focus();
        return;
    } else if (shippingpinCode.trim() == "") {
        $("#copytocustomershippingpinCode").addClass('alert-danger');
        $("#copytocustomershippingpinCode").focus();
        return;
    }
    else if (shippingCountry.trim() == "") {
        $("#copytocustomershippingpinCode").addClass('alert-danger');
        $("#copytocustomershippingpinCode").focus();
        return;
    } else if (!validGstin()) {
        $("#copytocustomerGSTIN1").addClass('alert-danger');
        $("#copytocustomerGSTIN2").addClass('alert-danger');
        $("#copytocustomerGSTIN3").addClass('alert-danger');

        return;
    }
            
    var IsAllCopy = $("#hdncopytoproduct").val();
    var CustomerDetails = {}
    var oldcustomer_id = $("#hdncustomerucc").val();
    var sex = $("#hdnsex").val();
    var dob = $("#hdndob").val();
    var hdnanniversarydate = $("#hdnanniversarydate").val();
    //var 1.2
    CustomerDetails.sex = sex;
    CustomerDetails.dob = dob;
    CustomerDetails.hdnanniversarydate = hdnanniversarydate;
    CustomerDetails.IsAllCopy = IsAllCopy;
    CustomerDetails.oldcustomerid = oldcustomer_id;
    //chinmoy edited below code 02-04-2020 start	
    CustomerDetails.UniqueID = uniqueCopyID;
    //rev srijeeta mantis issue 0024515
    CustomerDetails.Alternative_Code = Alternative_Code;
    //end of rev srijeeta mantis issue 0024515
    CustomerDetails.NumberingId = NumberingId
    //End
    CustomerDetails.Name = Name;
    CustomerDetails.BillingAddress1 = BillingAddress1;
    CustomerDetails.BillingAddress2 = BillingAddress2;
    CustomerDetails.BillingPin = pinCode;
    CustomerDetails.shippingAddress1 = shippingAddress1;
    CustomerDetails.shippingAddress2 = shippingAddress2;
    CustomerDetails.shippingPin = shippingpinCode;
    CustomerDetails.GSTIN = GSTIN;
    CustomerDetails.BillingPhone = BillingPhone;
    CustomerDetails.ShippingPhone = ShippingPhone;
    CustomerDetails.contactperson = contactperson;
    CustomerDetails.IdTypeval = IdTypeval;
    //CustomerDetails.GrpCust = GrpCust;
    CustomerDetails.GrpCust = 0;
    CustomerDetails.TCSApplicable = TCSApplicable;
    //data: "{ UniqueID: '" + uniqueID + "',Name: '" + Name + "',BillingAddress1: '" + BillingAddress1 + "',BillingAddress2: '" + BillingAddress2 + "',BillingPin: '" + pinCode + "',shippingAddress1: '" + shippingAddress1 + "',shippingAddress2: '" + shippingAddress2 + "',shippingPin: '" + shippingpinCode + "', GSTIN:'" + GSTIN + "',BillingPhone:'" + BillingPhone + "',ShippingPhone:'" + ShippingPhone + "',contactperson:'" + contactperson + "' }",

    $.ajax({
        type: "POST",
        url: "CustomerMasterList.aspx/SaveCustomerMaster",
        data: JSON.stringify(CustomerDetails),
        //contentType: "application/json; charset=utf-8",
        //dataType: "json",
        //async: "true",
        //cache: "false",
        contentType: "application/json; charset=utf-8",
        datatype: "JSON",
        type: "POST",
        success: function (msg) {
            var obj = msg.d;
            //if (obj["status"] != "Ok")
            //{
            //    jAlert(obj["Msg"]);
            //}
            if (obj.status == "Ok") {

                parent.ParentCustomerOnClose(obj.InternalId, $("#Name").val(), $("#uniqueId").val(), obj.BillingStateText, obj.BillingStateCode, obj.ShippingStateText, obj.ShippingStateCode);

            }
            else if (obj.status == "DuplicateGSTIN") {                       
                jAlert('Duplicate GSTIN. ');                           
            }
            else if (obj.status == "AUTOError") {
                jAlert('Either Unique ID Exists OR Unique ID Exhausted.');
                return;
            }
            else if (obj.status == "Error") {
                jAlert('Unique ID Already Exists.');
                return;
            }
            else {
                console.log(obj.Msg);
            }
        },
        Error: function (x, e) {
            // On Error
        }
    });
}
//End Rev Rajdip
function OnDelete(keyValue) {
    jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
        if (r == true) {
            grid.PerformCallback('Delete~' + keyValue);
        }
    });
}
//-------------Subhra 16-05-2019--------------------
var keyvalueforassign = '';
function OnAssignTo(keyValue, Name, EnterBy) {
    keyvalueforassign = keyValue;
    cCallbackPanelAssign.PerformCallback('Load~' + keyvalueforassign + '~' + Name);
    $('#AssignToModel').modal('show');
    $("#cmbAssignTo").focus();
}
function Assign() {
    if ($("#txtRemarks").val() != "") {
        cCallbackPanelAssign.PerformCallback('Assign~' + keyvalueforassign);
        $('#AssignToModel').modal('hide');
    }
    else {
        jAlert('Remarks is mandatory.');
    }

}

function UnAssign(keyValue) {
    if ($("#txtRemarks").val() != "") {
        jConfirm('Confirm?', 'Confirmation Dialog', function (r) {
            if (r == true) {

                cCallbackPanelAssign.PerformCallback('Unassign~' + keyvalueforassign);
                $('#AssignToModel').modal('hide');
            }
        });
    }
    else {
        jAlert('Remarks is mandatory.', 'title', function () {
            $("#txtRemarks").focus();
        });
    }
}
function CallbackPanelEndCall(s, e) {
    if (cCallbackPanelAssign.cpAssignSave == "Save") {
        grid.Refresh();
    }

    if (cCallbackPanelAssign.cpEnteredBy != 0) {
        $("#lblenteredby").text(cCallbackPanelAssign.cpName);
        $("#txtRemarks").val(cCallbackPanelAssign.cpRemarks);
        $("#cmbAssignTo").val(cCallbackPanelAssign.cpEnteredBy);
    }
    else {
        $("#lblenteredby").text(cCallbackPanelAssign.cpName);
        $("#cmbAssignTo").val(cCallbackPanelAssign.cpAssignTo);
        $("#txtRemarks").val(cCallbackPanelAssign.cpRemarks);
    }
    if ($("#txtRemarks").val() == '') {
        $('#btnUnAssign').hide();
    }
    else {
        $('#btnUnAssign').show();
    }


}


//--------------------------------------------------
function OnBudgetopen(Cusid) {

    var url = '/OMS/Management/Master/BudgetAdd.aspx?Cusid=' + Cusid+'&&Type=Customer';
    popupbudget.SetContentUrl(url);
    popupbudget.Show();

    //}
    return true;
}

function BudgetAfterHide(s, e) {
    popupbudget.Hide();
}


function ParentCustomerOnClose(newCustId, CustomerName, CustUniqueName, BillingStateText, BillingStateCode, ShippingStateText, ShippingStateCode) {
    AspxDirectAddCustPopup.Hide();
    var url = 'CustomerMasterList.aspx';
    window.location.href = url;


}


function OnCreateActivityClick(KeyVal, cnt_id, status) {
    //kaushik
    // var url = "Lead_Activity.aspx?id=" + KeyVal + "&cnt_id=" + cnt_id;
    if (status == "Converted") {
        jAlert("Your Current Status is: Converted .Cannot Proceed");
        return false;
    }
    else if (status == "Lost") {
        jAlert("Your Current Status is: Lost .Cannot Proceed");
        return false;
    }
    else {
        var url = "../ActivityManagement/Sales_Activity.aspx?id=" + KeyVal + "&cnt_id=" + cnt_id;
        window.location.href = url;
    }
}
function ShowHideFilter(obj) {
    if (document.getElementById('TxtSeg').value == 'N') {
        document.getElementById('TxtTCODE').style.display = "none";
    }
    else {
        document.getElementById('TxtTCODE').style.display = "inline";
    }
    InitialTextVal();
    if (obj == "s")
        //document.getElementById('TrFilter').style.display="inline";
        grid.PerformCallback('ssss');
    else {
        document.getElementById('TrFilter').style.display = "none";
        grid.PerformCallback(obj);
    }
}
function callback() {
    grid.PerformCallback();
}
function OnContactInfoClick(keyValue, CompName) {

    var url = 'insurance_contactPerson.aspx?id=' + keyValue;
    window.location.href = url;
}
function OnHistoryInfoClick(keyValue, CompName) {
    var url = 'ShowHistory_Phonecall.aspx?id1=' + keyValue;
    //OnMoreInfoClick(url, "Lead  History", '940px', '450px', "Y");
    window.location.href = url;
}
function OnAddBusinessClick(keyValue, CompName) {
    var url = 'AssignIndustry.aspx?id1=' + keyValue;
    window.location.href = url;
}
function btnSearch_click() {
    document.getElementById('TrFilter').style.display = "none";
    grid.PerformCallback('s');
}
function InitialTextVal() {


    document.getElementById('txtName').value = "Name";
    document.getElementById('txtBranchName').value = "Branch Name";
    document.getElementById('txtCode').value = "Code";
    document.getElementById('txtRelationManager').value = "R. Manager";
    document.getElementById('txtReferedBy').value = "Email";
    document.getElementById('txtPhNumber').value = "Ph. Number";
    document.getElementById('txtContactStatus').value = "Contact Status";


    document.getElementById('TxtTCODE').value = "Trade. Code";
    document.getElementById('txtPAN').value = "PAN No.";
}
function gridcrmCampaignclick(s, e) {
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

    $(document).ready(function () {
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
    });
