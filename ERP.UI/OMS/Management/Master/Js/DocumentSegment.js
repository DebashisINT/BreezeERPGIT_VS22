

function SaveCustomer() {
    document.getElementById("btnSaveCust").disabled = true;

    $("#uniqueId").removeClass('alert-danger');
    $("#Name").removeClass('alert-danger');
    $("#BillingAddress1").removeClass('alert-danger');
    $("#pinCode").removeClass('alert-danger');
    $("#shippingAddress1").removeClass('alert-danger');
    $("#shippingpinCode").removeClass('alert-danger');

    $("#GSTIN1").removeClass('alert-danger');
    $("#GSTIN2").removeClass('alert-danger');
    $("#GSTIN3").removeClass('alert-danger');
    var uniqueID ="";
    
    var code = $("#uniqueId").val();
    var Name = $("#Name").val();
    var BillingAddress1 = $("#BillingAddress1").val();
    var BillingAddress2 = $("#BillingAddress2").val();
    var pinCode = $("#pinCode").val();
    var Country = $("#Country").val();

    var shippingAddress1 = $("#shippingAddress1").val();
    var shippingAddress2 = $("#shippingAddress2").val();
    var shippingpinCode = $("#shippingpinCode").val();
    var shippingCountry = $("#shippingCountry").val();

    var GSTIN = $("#GSTIN1").val().trim() + $("#GSTIN2").val().trim() + $("#GSTIN3").val().trim();

    var BillingPhone = $("#BillingPhone").val().trim();
    var ShippingPhone = $("#ShippingPhone").val().trim();
    var contactperson = $("#contactperson").val().trim();

    //var GrpCust = $("#GroupCust option:selected").val();
    var SegmentID = $("#ddlSegment option:selected").val();
    var ParentSegmentID = $("#ddlParentSegment option:selected").val();
    //Add rev for Transcation Category Tanmoy
    //var TransactionCategory = $("#TransactionCategory option:selected").val();
    //Add rev for Transcation Category Tanmoy

    //var IdType = document.getElementById("IdType");
    //var IdTypeval = IdType.options[IdType.selectedIndex].value;
    //var TCSApplicable = false;
    //if (document.getElementById("chkTCSAppli").checked == true) {
    //    TCSApplicable = true;
    //}
    var DocumentSegmentId = $('#hdDocumentSegmentId').val();

    var PANNum = $("#PANNum").val();
    if (PANNum != "")
    {
        var panPat = /^([a-zA-Z]{5})(\d{4})([a-zA-Z]{1})$/;
        var code = /([C,P,H,F,A,T,B,L,J,G])/;
        var code_chk = PANNum;
        //if (uniqueID.search(panPat) == -1) {
        //    jAlert("Please type valid PAN No.");
        //    document.getElementById("btnSaveCust").disabled = false;
        //    return;
        //}
        if (code.test(code_chk) == false) {
            jAlert("Please type valid PAN No.");
            document.getElementById("btnSaveCust").disabled = false;
            return;
        }
    }
    

    if ($("#uniqueId").val().trim() == "") {
        document.getElementById("btnSaveCust").disabled = false;
        $("#uniqueId").addClass('alert-danger');
        $("#uniqueId").focus();
        return;
    }

    

    if (SegmentID.trim() == "Select") {
        document.getElementById("btnSaveCust").disabled = false;
        $("#ddlSegment").addClass('alert-danger');
        $("#ddlSegment").focus();
        return;
    }
    else if (Name.trim() == "") {
        document.getElementById("btnSaveCust").disabled = false;
        $("#Name").addClass('alert-danger');
        $("#Name").focus();

        return;
    }
    else if (BillingAddress1.trim() == "") {
        document.getElementById("btnSaveCust").disabled = false;
        $("#BillingAddress1").addClass('alert-danger');
        $("#BillingAddress1").focus();
        return;
    } else if (pinCode.trim() == "") {
        document.getElementById("btnSaveCust").disabled = false;
        $("#pinCode").addClass('alert-danger');
        $("#pinCode").focus();
        return;
    } else if (Country.trim() == "") {
        document.getElementById("btnSaveCust").disabled = false;
        $("#pinCode").addClass('alert-danger');
        $("#pinCode").focus();
        return;
    }

    else if (shippingAddress1.trim() == "") {
        document.getElementById("btnSaveCust").disabled = false;
        $("#shippingAddress1").addClass('alert-danger');
        $("#shippingAddress1").focus();
        return;
    } else if (shippingpinCode.trim() == "") {
        document.getElementById("btnSaveCust").disabled = false;
        $("#shippingpinCode").addClass('alert-danger');
        $("#shippingpinCode").focus();
        return;
    }
    else if (shippingCountry.trim() == "") {
        document.getElementById("btnSaveCust").disabled = false;
        $("#shippingpinCode").addClass('alert-danger');
        $("#shippingpinCode").focus();
        return;
    } else if (!validGstin()) {
        document.getElementById("btnSaveCust").disabled = false;
        $("#GSTIN1").addClass('alert-danger');
        $("#GSTIN2").addClass('alert-danger');
        $("#GSTIN3").addClass('alert-danger');

        return;
    }

    var PAN = $("#PANNum").val();

    if (PAN.trim() != "") {
        var panPat = /^([a-zA-Z]{5})(\d{4})([a-zA-Z]{1})$/;
        var code = /([C,P,H,F,A,T,B,L,J,G])/;
        var code_chk = PAN.substring(3, 4);
        if (PAN.search(panPat) == -1) {
            document.getElementById("btnSaveCust").disabled = false;
            jAlert("Please enter valid PAN.");

            return;
        }
        if (code.test(code_chk) == false) {
            document.getElementById("btnSaveCust").disabled = false;
            jAlert("Please enter valid PAN.");

            return;
        }
    }

    var CrgNumber = $("#PANNum").val();
    var procode = "0";
    if (CrgNumber.trim() != "") {
        $.ajax({
            type: "POST",
            //url: "Contact_Registration.aspx/CheckUniqueNumber",     
            url: "/OMS/management/Master/Contact_Registration.aspx/SettingsWisecheckCustomerPanNameExists",
            data: JSON.stringify({ CrgNumber: CrgNumber, procode: procode }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {
                var data = msg.d;

                if (data != "") {
                    document.getElementById("btnSaveCust").disabled = false;
                    jAlert("Entered value is already exist for Customer " + data + ". Cannot Proceed.");
                    ctxtNumber.Focus();
                    return;
                }
            }

        });
    }

    var mode=$("#hdAddEdit").val();
    var cnt_id = $("#hdnCustomerID").val();

    if (ParentSegmentID == "Select")
    {
        ParentSegmentID = "0";
    }

    var ServiceBranch = $('#ddlServiceBranch').val();
    
    var SEgmentDetails = {}
    
    SEgmentDetails.mode = mode;
    SEgmentDetails.cnt_id = cnt_id;
    SEgmentDetails.SegmentID = SegmentID;
    SEgmentDetails.UniqueID = code;
    SEgmentDetails.Name = Name;
    SEgmentDetails.ParentSegmentID = ParentSegmentID;
    SEgmentDetails.BillingAddress1 = BillingAddress1;
    SEgmentDetails.BillingAddress2 = BillingAddress2;
    SEgmentDetails.BillingPin = pinCode;
    SEgmentDetails.shippingAddress1 = shippingAddress1;
    SEgmentDetails.shippingAddress2 = shippingAddress2;
    SEgmentDetails.shippingPin = shippingpinCode;
    SEgmentDetails.GSTIN = GSTIN;
    SEgmentDetails.BillingPhone = BillingPhone;
    SEgmentDetails.ShippingPhone = ShippingPhone;
    SEgmentDetails.contactperson = contactperson;   
    SEgmentDetails.PANValue = $("#PANNum").val();

    SEgmentDetails.BillingCountryID = $("#hdnBillingCountryID").val();
    SEgmentDetails.BillingStateID = $("#hdnBillingStateID").val();
    SEgmentDetails.BillingCityID = $("#hdnBillingCityID").val();
    SEgmentDetails.BillingPinID = $("#hdnBillingPinID").val();

    
    SEgmentDetails.ServiceCountryID = $("#hdnServiceCountryID").val();
    SEgmentDetails.ServiceStateID = $("#hdnServiceStateID").val();
    SEgmentDetails.ServiceCityID = $("#hdnServiceCityID").val();
    SEgmentDetails.ServicePinID = $("#hdnServicePinID").val();  


    SEgmentDetails.BillingLatitude = $("#BillingLatitude").val();
    SEgmentDetails.BillingLongitude = $("#BillingLongitude").val();
    SEgmentDetails.ServiceLatitude = $("#ServiceLatitude").val();
    SEgmentDetails.ServiceLongitude = $("#ServiceLongitude").val();

    SEgmentDetails.DocumentSegmentId = DocumentSegmentId;
    SEgmentDetails.TreatmentArea = $("#TreatmentArea").val();

    SEgmentDetails.ServiceBranch = ServiceBranch;
    SEgmentDetails.SyncCustomertoFSMWhileCreating = $("#hdnSyncCustomertoFSMWhileCreating").val();

    $.ajax({
        type: "POST",
        url: "/OMS/management/Master/DocumentSegmentAdd.aspx/SaveCustomer",
        data: JSON.stringify(SEgmentDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: "true",
        cache: "false",
        success: function (msg) {
            var obj = msg.d;
            if (obj.status == "OK") {

                //parent.ParentCustomerOnClose(obj.InternalId, $("#Name").val(), $("#uniqueId").val(), obj.BillingStateText, obj.BillingStateCode, obj.ShippingStateText, obj.ShippingStateCode);
               // parent.ParentCustomerOnClose(obj.InternalId, $("#Name").val(), uniqueID, obj.BillingStateText, obj.BillingStateCode, obj.ShippingStateText, obj.ShippingStateCode);

                // window.location.href = 'DocumentSegmentList.aspx?id=' + $("#hdnCustomerID").val();
                window.parent.cPosView.Hide();
                window.parent.cgridSegment.Refresh();

            }
            else if (obj.status == "DuplicateGSTIN") {
                document.getElementById("btnSaveCust").disabled = false;
                jAlert('Duplicate GSTIN. ');

            }
            else if (obj.status == "AUTOError") {
                document.getElementById("btnSaveCust").disabled = false;
                jAlert('Either Unique ID Exists OR Unique ID Exhausted.');
                return;
            }
            else if (obj.status == "BannedPAN") {
                document.getElementById("btnSaveCust").disabled = false;
                jAlert('This PAN is banned by SEBI.');
                return;
            }
            else if (obj.status == "DuplicatePAN") {
                document.getElementById("btnSaveCust").disabled = false;
                jAlert('Duplicate PAN number is not allowed.');
                return;
            }
            else {
                document.getElementById("btnSaveCust").disabled = false;
                console.log(obj.Msg);
            }
        },
        Error: function (x, e) {
            // On Error
        }
    });
}

function UniqueVendorCodeCheck() {
    debugger;
    var SchemeVal = $("#NumSchemeVendor").val();

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

        uccName = $("#DocNum").val();
        Type = "MasterVendor";


        $.ajax({
            type: "POST",
            url: "/OMS/management/Master/HRrecruitmentagent_general.aspx/CheckUniqueNumberingCode",
            data: JSON.stringify({ uccName: uccName, Type: Type }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                CheckUniqueCode = msg.d;
                if (CheckUniqueCode == true) {
                    $("#DocNum").val('');
                    $("#DocNum").addClass('alert-danger');
                    $("#DocNum").focus();
                    // console.log('Please enter unique document number');
                    //jAlert('Please enter unique Sales Order No');

                }

            }

        });
    }
}

function NameForSave() {
    document.getElementById("btnSaveCust").disabled = false;
}

function SetPanNumber() {

    $("#PANNum").val($("#GSTIN2").val());
    
}

function UniquePanNumberCheck() {
    var PAN = $("#PANNum").val();
    // $("#hdnflag").val("0");
    if (PAN.trim() != "") {
        var panPat = /^([a-zA-Z]{5})(\d{4})([a-zA-Z]{1})$/;
        var code = /([C,P,H,F,A,T,B,L,J,G])/;
        var code_chk = PAN.substring(3, 4);
        if (PAN.search(panPat) == -1) {
            jAlert("Please enter valid PAN.");
            // $("#hdnflag").val("1");
            //alert('hi');
            return;
        }
        if (code.test(code_chk) == false) {
            jAlert("Please enter valid PAN.");
            //$("#hdnflag").val("1");
            return;
        }
    }
    //var CrgNumber = $("#PANNum").val();
    //var procode = "0";
    //if (CrgNumber.trim() != "") {
    //    $.ajax({
    //        type: "POST",             
    //        url: "/OMS/management/Master/Contact_Registration.aspx/SettingsWisecheckCustomerPanNameExists",
    //        data: JSON.stringify({ CrgNumber: CrgNumber, procode: procode }),
    //        contentType: "application/json; charset=utf-8",
    //        dataType: "json",
    //        async: false,
    //        success: function (msg) {
    //            var data = msg.d;

    //            if (data != "") {
    //                jAlert("Entered value is already exist for Customer " + data + ". Cannot Proceed.");
    //                ctxtNumber.Focus();
    //                return;
    //            }
    //        }

    //    });
    //}


}

function UniqueCodeCheck() {
    debugger;
    var SchemeVal = $("#NumScheme").val();

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

        uccName = $("#DocNum").val();
        Type = "Mastercustomerclient";


        $.ajax({
            type: "POST",
            url: "/OMS/management/Master/Contact_general.aspx/CheckUniqueNumberingCode",
            data: JSON.stringify({ uccName: uccName, Type: Type }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                CheckUniqueCode = msg.d;
                if (CheckUniqueCode == true) {
                    $("#DocNum").val('');
                    $("#DocNum").addClass('alert-danger');
                    //$("#DocNum").focus();
                    // console.log('Please enter unique document number');
                    jAlert('Duplicate Unique Id. Cannot Proceed.');

                }
                else {
                    $("#DocNum").removeClass('alert-danger');
                }

            }

        });
    }
}

function PinChange() {
    var pincode = $("#pinCode").val();

    $.ajax({
        type: "POST",
        url: "DocumentSegmentAdd.aspx/GetAddressdetails",
        data: "{ pinCode: '" + pincode + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: "true",
        cache: "false",
        success: function (msg) {
            var obj = msg.d;
            if (obj.status == "Ok") {
                $("#Country").val(obj.Country);
                $("#State").val(obj.state);
                $("#City").val(obj.city);

                $("#hdnBillingCountryID").val(obj.countryID);
                $("#hdnBillingStateID").val(obj.stateId);
                $("#hdnBillingCityID").val(obj.cityID);
                $("#hdnBillingPinID").val(obj.PinID);

                
                $("#pinCode").removeClass('alert-danger');
            } else {
                $("#Country").val('');
                $("#State").val('');
                $("#City").val('');
                $("#pinCode").addClass('alert-danger');
                $("#hdnBillingCountryID").val('0');
                $("#hdnBillingStateID").val('0');
                $("#hdnBillingCityID").val('0');
                $("#hdnBillingPinID").val('0');
            }
        },
        Error: function (x, e) {
            // On Error
        }
    });


}

function shippingPinChange() {
    var pincode = $("#shippingpinCode").val();

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
                $("#shippingCountry").val(obj.Country);
                $("#shippingState").val(obj.state);
                $("#shippingCity").val(obj.city);
                $("#shippingpinCode").removeClass('alert-danger');
            } else {
                $("#shippingCountry").val('');
                $("#shippingState").val('');
                $("#shippingCity").val('');
                $("#shippingpinCode").addClass('alert-danger');
            }
        },
        Error: function (x, e) {
            // On Error
        }
    });


}

function VendorshippingPinChange() {
    var pincode = $("#VenshippingpinCode").val();

    $.ajax({
        type: "POST",
        url: "/OMS/management/Master/Customer_general.aspx/GetAddressdetails",
        data: "{ pinCode: '" + pincode + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: "true",
        cache: "false",
        success: function (msg) {
            var obj = msg.d;
            if (obj.status == "Ok") {
                $("#VenshippingCountry").val(obj.Country);
                $("#VenshippingState").val(obj.state);
                $("#VenshippingCity").val(obj.city);
                $("#VenshippingpinCode").removeClass('alert-danger');
            } else {
                $("#VenshippingCountry").val('');
                $("#VenshippingState").val('');
                $("#VenshippingCity").val('');
                $("#VenshippingpinCode").addClass('alert-danger');
            }
        },
        Error: function (x, e) {
            // On Error
        }
    });


}

function CheckUnique() {
    var uniqueId = $("#uniqueId").val();
    if (uniqueId.trim() != '') {
        var CustomerID = $("#hdnCustomerID").val();
        $.ajax({
            type: "POST",
            url: "DocumentSegmentAdd.aspx/CheckuniqueId",
            //data: "{ uniqueId: '" + uniqueId + "',CustomerID: CustomerID}",
            data: JSON.stringify({ uniqueId: uniqueId, CustomerID: CustomerID }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: "true",
            cache: "false",
            success: function (msg) {
                
                var obj = msg.d;
                if (obj.IsPresent) {
                    $("#uniqueId").addClass('alert-danger');
                    $("#uniqueId").val('');
                  
                    jAlert("Duplicate Unique Id.Cannot Proceed.");
                   
                    $("#uniqueId").focus();
                } else {
                    $("#uniqueId").removeClass('alert-danger');
                }
            },
            Error: function (x, e) {
                // On Error
            }
        });
    }

}

function billingCopyToshipping() {

    $("#shippingAddress1").val($("#BillingAddress1").val());
    $("#shippingAddress2").val($("#BillingAddress2").val());
    $("#shippingpinCode").val($("#pinCode").val());
    $("#shippingCountry").val($("#Country").val());
    $("#shippingState").val($("#State").val());
    $("#shippingCity").val($("#City").val());
    $("#ShippingPhone").val($("#BillingPhone").val());

    var countryID=$("#hdnBillingCountryID").val();
    var stateId=$("#hdnBillingStateID").val();
    var cityID=$("#hdnBillingCityID").val();
    var PinID = $("#hdnBillingPinID").val();

    $("#hdnServiceCountryID").val(countryID);
    $("#hdnServiceStateID").val(stateId);
    $("#hdnServiceCityID").val(cityID);
    $("#hdnServicePinID").val(PinID);

}

function VendorbillingCopyToshipping() {

    $("#VenshippingAddress1").val($("#VenBillingAddress1").val());
    $("#VenshippingAddress2").val($("#VenBillingAddress2").val());
    $("#VenshippingpinCode").val($("#VenpinCode").val());
    $("#VenshippingCountry").val($("#VenCountry").val());
    $("#VenshippingState").val($("#VenState").val());
    $("#VenshippingCity").val($("#VenCity").val());
    $("#VenShippingPhone").val($("#VenBillingPhone").val());

}

function shippingCopyTobilling() {
    $("#BillingAddress1").val($("#shippingAddress1").val());
    $("#BillingAddress2").val($("#shippingAddress2").val());
    $("#pinCode").val($("#shippingpinCode").val());
    $("#Country").val($("#shippingCountry").val());
    $("#State").val($("#shippingState").val());
    $("#City").val($("#shippingCity").val());
    $("#BillingPhone").val($("#ShippingPhone").val());
}

function VendorshippingCopyTobilling() {
    $("#VenBillingAddress1").val($("#VenshippingAddress1").val());
    $("#VenBillingAddress2").val($("#VenshippingAddress2").val());
    $("#VenpinCode").val($("#VenshippingpinCode").val());
    $("#VenCountry").val($("#VenshippingCountry").val());
    $("#VenState").val($("#VenshippingState").val());
    $("#VenCity").val($("#VenshippingCity").val());
    $("#VenBillingPhone").val($("#VenShippingPhone").val());
}

function ClearShipping() {
    $("#shippingAddress1").val('');
    $("#shippingAddress2").val('');
    $("#shippingpinCode").val('');
    $("#shippingCountry").val('');
    $("#shippingState").val('');
    $("#shippingCity").val('');
    $("#ShippingPhone").val('');

}

function VendorClearShipping() {
    $("#VenshippingAddress1").val('');
    $("#VenshippingAddress2").val('');
    $("#VenshippingpinCode").val('');
    $("#VenshippingCountry").val('');
    $("#VenshippingState").val('');
    $("#VenshippingCity").val('');
    $("#VenShippingPhone").val('');

}

function ClearBilling() {
    $("#BillingAddress1").val('');
    $("#BillingAddress2").val('');
    $("#pinCode").val('');
    $("#Country").val('');
    $("#State").val('');
    $("#City").val('');
    $("#BillingPhone").val('');
}
function VendorClearBilling() {
    $("#VenBillingAddress1").val('');
    $("#VenBillingAddress2").val('');
    $("#VenpinCode").val('');
    $("#VenCountry").val('');
    $("#VenState").val('');
    $("#VenCity").val('');
    $("#VenBillingPhone").val('');
}
function validGstin() {
    var GSTIN1 = $("#GSTIN1").val().trim();
    var GSTIN2 = $("#GSTIN2").val().trim();
    var GSTIN3 = $("#GSTIN3").val().trim();

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

function validVendorGstin() {
    var GSTIN1 = $("#VenGSTIN1").val().trim();
    var GSTIN2 = $("#VenGSTIN2").val().trim();
    var GSTIN3 = $("#VenGSTIN3").val().trim();

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


$(document).ready(function () {
    document.getElementById("uniqueId").disabled = true;
    $("#uniqueId").focus();
    $('#ddlSegment').change(function () {
        $('#Name').val("");
        var SegmentNo = $("#ddlSegment").val();
        var CustomerID = $("#hdnCustomerID").val();
        if (SegmentNo != "") {

            $.ajax({
                url: "DocumentSegmentAdd.aspx/GetMaxLenghtSegment",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ SegmentID: SegmentNo, CustomerID: CustomerID }),
                datatype: "JSON",
                type: "POST",
                success: function (data) {
                    var grpdetl = data.d;
                    document.getElementById("uniqueId").maxLength = grpdetl;
                    document.getElementById("uniqueId").disabled = false;
                },
                error: function (data) {

                    jAlert("Please try again later");
                }
            });
            
            $.ajax({
                url: "DocumentSegmentAdd.aspx/PopulateParentSegment",
                //url: "/OMS/management/Master/DocumentSegmentAdd.aspx/PopulateParentSegment",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ SegmentID: SegmentNo, CustomerID: CustomerID }),
                datatype: "JSON",
                type: "POST",
                success: function (data) {

                    $("#ddlParentSegment").empty();
                    var grpdetl = data.d;

                    var opts = "";

                    for (i in grpdetl)
                        opts += "<option value='" + grpdetl[i].ID + "'>" + grpdetl[i].Segment + "</option>";

                    $("#ddlParentSegment").empty().append(opts);

                },
                error: function (data) {

                    jAlert("Please try again later");
                }
            });
          
        }
    });

    var urlKeys = getUrlVars();
    if (urlKeys.type == 'req') {
        document.getElementById("btnSaveCust").disabled = true;
    }

});

function getUrlVars() {
    var vars = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }
    return vars;
}


function BillingCheckChange() {
    if (document.getElementById('fancy-checkbox-success').checked) {
        document.getElementById('fancy-checkbox-success').checked = false;
        ClearShipping();
    }
    else {
        document.getElementById('fancy-checkbox-success').checked = true;
        billingCopyToshipping();
    }

}


function VendorBillingCheckChange() {
    if (document.getElementById('fancy-checkbox-success').checked) {
        document.getElementById('fancy-checkbox-success').checked = false;
        VendorClearShipping();
    }
    else {
        document.getElementById('fancy-checkbox-success').checked = true;
        VendorbillingCopyToshipping();
    }

}


function ShippingCheckChange() {
    if (document.getElementById('fancy-checkbox-successShipping').checked) {
        document.getElementById('fancy-checkbox-successShipping').checked = false;
        ClearBilling();
    }
    else {
        document.getElementById('fancy-checkbox-successShipping').checked = true;
        shippingCopyTobilling();
    }

}


function VendorShippingCheckChange() {
    if (document.getElementById('fancy-checkbox-successShipping').checked) {
        document.getElementById('fancy-checkbox-successShipping').checked = false;
        VendorClearBilling();
    }
    else {
        document.getElementById('fancy-checkbox-successShipping').checked = true;
        VendorshippingCopyTobilling();
    }

}

$(document).ready(function () {
    
});


var dataval;
var datavalVendor;
$(document).ready(function () {
    
    var mode = $('#hdAddEdit').val();

    var CustomerID = $("#hdnCustomerID").val();
    $.ajax({
        url: "DocumentSegmentAdd.aspx/PopulateSEgment",
       // url: "/OMS/management/Master/DocumentSegmentAdd.aspx/PopulateSEgment",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ CustomerID: CustomerID }),
        datatype: "JSON",
        type: "POST",
        async: false,
        success: function (data) {

            $("#ddlSegment").empty();            
            var grpdetl = data.d;          

            var opts = "";
         
            for (i in grpdetl)               
                opts += "<option value='" + grpdetl[i].ID + "'>" + grpdetl[i].Segment + "</option>";

            $("#ddlSegment").empty().append(opts);
            
        },
        error: function (data) {

            jAlert("Please try again later");
        }
    });


    

    
    if(mode=="Edit")
    {
        $('#uniqueId').prop('disabled', true);
        var DocumentSegmentId = $('#hdDocumentSegmentId').val();
        $.ajax({
            url: "DocumentSegmentAdd.aspx/PopulateEditDate",
            // url: "/OMS/management/Master/DocumentSegmentAdd.aspx/PopulateSEgment",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ DocumentSegmentId: DocumentSegmentId }),
            datatype: "JSON",
            type: "POST",
            async: false,
            success: function (data) {

                //$("#ddlSegment").empty();
                var grpdetl = data.d;

                

                $("#ddlSegment").val(grpdetl[0].Segment_ID);

                if (grpdetl[0].Segment_ID != "") {
                    $.ajax({
                        url: "DocumentSegmentAdd.aspx/PopulateParentSegment",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify({ SegmentID: grpdetl[0].Segment_ID, CustomerID: CustomerID }),
                        datatype: "JSON",
                        type: "POST",
                        async: false,
                        success: function (data) {

                            $("#ddlParentSegment").empty();
                            var grpdetl = data.d;

                            var opts = "";

                            for (i in grpdetl)
                                opts += "<option value='" + grpdetl[i].ID + "'>" + grpdetl[i].Segment + "</option>";

                            $("#ddlParentSegment").empty().append(opts);

                        },
                        error: function (data) {

                            jAlert("Please try again later");
                        }
                    });
                }
                $("#ddlParentSegment").val(grpdetl[0].ParentSegment_ID);

                $("#uniqueId").val(grpdetl[0].Code);
                $("#Name").val(grpdetl[0].Name);
                $("#PANNum").val(grpdetl[0].PanNo);

                $("#BillingAddress1").val(grpdetl[0].Address1);
                $("#BillingAddress2").val(grpdetl[0].Address2);
                $("#BillingPhone").val(grpdetl[0].BillPhoneNo);
                $("#pinCode").val(grpdetl[0].BillPincode);
                $("#Country").val(grpdetl[0].billCountryName);
                $("#State").val(grpdetl[0].BillstateName);
                $("#City").val(grpdetl[0].billcity_name);
                

                $("#contactperson").val(grpdetl[0].ContactName);
                $("#shippingAddress1").val(grpdetl[0].shippingAddress1);
                $("#shippingpinCode").val(grpdetl[0].SerPincode);
                $("#ShippingPhone").val(grpdetl[0].ServicePhoneNo);
                $("#shippingState").val(grpdetl[0].SerstateName);
                $("#shippingCountry").val(grpdetl[0].serCountryName);
                $("#shippingCity").val(grpdetl[0].sercity_name);


                //$("#GSTIN1").val(grpdetl.);
                //$("#GSTIN2").val(grpdetl.);
                //$("#GSTIN3").val(grpdetl.);
                var GSTIN = grpdetl[0].GSTIN;
                var GSTIN1 = GSTIN.substring(0, 2);
                var GSTIN2 = GSTIN.substring(2, 12);
                var GSTIN3 = GSTIN.substring(12, 15);


                $("#GSTIN1").val(GSTIN1);
                $("#GSTIN2").val(GSTIN2);
                $("#GSTIN3").val(GSTIN3);

                $("#hdnBillingCountryID").val(grpdetl[0].CountryId);
                $("#hdnBillingStateID").val(grpdetl[0].StateID);
                $("#hdnBillingCityID").val(grpdetl[0].CityID);
                $("#hdnBillingPinID").val(grpdetl[0].PinCode);

                $("#hdnServiceCountryID").val(grpdetl[0].ServiceCountryID);
                $("#hdnServiceStateID").val(grpdetl[0].ServiceStateID);
                $("#hdnServiceCityID").val(grpdetl[0].ServiceCityID);
                $("#hdnServicePinID").val(grpdetl[0].ServicePinID);

                $("#BillingLatitude").val(grpdetl[0].BillingLatitude);
                $("#BillingLongitude").val(grpdetl[0].BillingLongitude);
                $("#ServiceLatitude").val(grpdetl[0].ServiceLatitude);
                $("#ServiceLongitude").val(grpdetl[0].ServiceLongitude);
                $("#TreatmentArea").val(grpdetl[0].TreatmentArea);


                $("#ddlServiceBranch").val(grpdetl[0].ServiceBranch);
                
            },
            error: function (data) {

                jAlert("Please try again later");
            }
        });
    }   

});

function ChangeCustomer() {
    $.ajax({
        url: "/OMS/management/Master/View/Services/ViewCustomerService.asmx/getGroupmasterDetails",
        contentType: "application/json; charset=utf-8",
        datatype: "JSON",
        type: "POST",
        success: function (data) {

            $("#GroupCust").empty();
            var grpdetl = data.d;

            debugger;

            var opts = "";
            for (i in grpdetl)
                opts += "<option value='" + grpdetl[i].Id + "'>" + grpdetl[i].Name + "</option>";

            $("#GroupCust").empty().append(opts);
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
}
