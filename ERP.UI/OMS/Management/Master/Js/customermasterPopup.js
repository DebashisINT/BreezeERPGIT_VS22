

function SaveCustomer() {
   
    $("#uniqueId").removeClass('alert-danger');
    $("#Name").removeClass('alert-danger');
    $("#BillingAddress1").removeClass('alert-danger');
    $("#pinCode").removeClass('alert-danger');
    $("#shippingAddress1").removeClass('alert-danger');
    $("#shippingpinCode").removeClass('alert-danger');

    $("#GSTIN1").removeClass('alert-danger');
    $("#GSTIN2").removeClass('alert-danger');
    $("#GSTIN3").removeClass('alert-danger');

    var uniqueID = $("#uniqueId").val();
    var Name = $("#Name").val();
    var BillingAddress1 = $("#BillingAddress1").val();
    var BillingAddress2 = $("#BillingAddress2").val();
    var pinCode = $("#pinCode").val();
    var Country = $("#Country").val();

    var shippingAddress1 = $("#shippingAddress1").val();
    var shippingAddress2 = $("#shippingAddress2").val();
    var shippingpinCode = $("#shippingpinCode").val();
    var shippingCountry = $("#shippingCountry").val();

   

    var BillingPhone = $("#BillingPhone").val().trim();
    var ShippingPhone = $("#ShippingPhone").val().trim();
    var contactperson = $("#contactperson").val().trim();

    var GrpCust = $("#GroupCust option:selected").val();
    if (uniqueID.trim() == "") {
        $("#uniqueId").addClass('alert-danger');
        $("#uniqueId").focus();
        return;
    } else if (Name.trim() == "") {
        $("#Name").addClass('alert-danger');
        $("#Name").focus();
        return;
    }




    var IdType = document.getElementById("IdType");
    var IdTypeval = IdType.options[IdType.selectedIndex].value;

    //alert(IdTypeval);

        if (IdTypeval == 1) {
            if (isNaN(uniqueID)) {
                alert("Please type valid Phone No.");
                return;
            }
        } else if (IdTypeval == 2) {

            var panPat = /^([a-zA-Z]{5})(\d{4})([a-zA-Z]{1})$/;
            var code = /([C,P,H,F,A,T,B,L,J,G])/;
            var code_chk = uniqueID.substring(3, 4);
            if (uniqueID.search(panPat) == -1) {
                alert("Please type valid PAN No.");
                return;
            }
            if (code.test(code_chk) == false) {
                alert("Please type valid PAN No.");
                return;
            }

        } else if (IdTypeval == 3) {
            if (isNaN(uniqueID) || uniqueID.length != 12) {
                alert("Please type valid Aadhar No.");
                return;
            }
        }


        var GSTIN1 = $("#GSTIN1").val().trim();
        var GSTIN2 = $("#GSTIN2").val().trim();
        var GSTIN3 = $("#GSTIN3").val().trim();

        var GSTIN = GSTIN1 + GSTIN2 + GSTIN3;
        //-----------------GSTIN Validation-------------

             if (GSTIN.length > 0) {
                 if (GSTIN1.length != 2 || GSTIN2.length != 10 || GSTIN3.length != 3) {
                     alert('Please enter valid GSTIN.');
                     return;
                 }
                 
                 var panPat = /^([a-zA-Z]{5})(\d{4})([a-zA-Z]{1})$/;
                 var code = /([C,P,H,F,A,T,B,L,J,G])/;
                 var code_chk = GSTIN2.substring(3, 4);
                 if (GSTIN2.search(panPat) == -1) {
                     alert('Please enter valid GSTIN.');
                     return;
                 }
                 else if (code.test(code_chk) == false) {
                     alert('Please enter valid GSTIN.');
                     return;
                 }              

            }
          
        //-----------------GSTIN Validation END---------



     if (BillingAddress1.trim() == "") {
        $("#BillingAddress1").addClass('alert-danger');
        $("#BillingAddress1").focus();
        return;
    } else if (pinCode.trim() == "") {
        $("#pinCode").addClass('alert-danger');
        $("#pinCode").focus();
        return;
    } else if (Country.trim() == "") {
        $("#pinCode").addClass('alert-danger');
        $("#pinCode").focus();
        return;
    }

    else if (shippingAddress1.trim() == "") {
        $("#shippingAddress1").addClass('alert-danger');
        $("#shippingAddress1").focus();
        return;
    } else if (shippingpinCode.trim() == "") {
        $("#shippingpinCode").addClass('alert-danger');
        $("#shippingpinCode").focus();
        return;
    }
    else if (shippingCountry.trim() == "") {
        $("#shippingpinCode").addClass('alert-danger');
        $("#shippingpinCode").focus();
        return;
    } else if (!validGstin()) {
        $("#GSTIN1").addClass('alert-danger');
        $("#GSTIN2").addClass('alert-danger');
        $("#GSTIN3").addClass('alert-danger');

        return;
    }

    var CustomerDetails = {}

    //var 1.2
    CustomerDetails.UniqueID = uniqueID;
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
    CustomerDetails.GrpCust = GrpCust;
    //data: "{ UniqueID: '" + uniqueID + "',Name: '" + Name + "',BillingAddress1: '" + BillingAddress1 + "',BillingAddress2: '" + BillingAddress2 + "',BillingPin: '" + pinCode + "',shippingAddress1: '" + shippingAddress1 + "',shippingAddress2: '" + shippingAddress2 + "',shippingPin: '" + shippingpinCode + "', GSTIN:'" + GSTIN + "',BillingPhone:'" + BillingPhone + "',ShippingPhone:'" + ShippingPhone + "',contactperson:'" + contactperson + "' }",

    $.ajax({
        type: "POST",
        url: "Customer_general.aspx/SaveCustomerMaster",
        data: JSON.stringify(CustomerDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: "true",
        cache: "false",
        success: function (msg) {
            var obj = msg.d;
           
            if (obj.status == "Ok") {

                parent.ParentCustomerOnClose(obj.InternalId, $("#Name").val(), $("#uniqueId").val(), obj.BillingStateText, obj.BillingStateCode, obj.ShippingStateText, obj.ShippingStateCode);

            }
            else if (obj.status == "DuplicateGSTIN")
            {
                alert('Duplicate GSTIN. ');
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

function SaveVendor() {
    
    $("#uniqueId").removeClass('alert-danger');
    $("#Name").removeClass('alert-danger');
    $("#BillingAddress1").removeClass('alert-danger');
    $("#pinCode").removeClass('alert-danger');
    $("#shippingAddress1").removeClass('alert-danger');
    $("#shippingpinCode").removeClass('alert-danger');

    $("#GSTIN1").removeClass('alert-danger');
    $("#GSTIN2").removeClass('alert-danger');
    $("#GSTIN3").removeClass('alert-danger');

    var uniqueID = $("#uniqueId").val();
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

    if (uniqueID.trim() == "" || isNaN(uniqueID)) {
        $("#uniqueId").addClass('alert-danger');
        $("#uniqueId").focus();
        return;
    } else if (Name.trim() == "") {
        $("#Name").addClass('alert-danger');
        $("#Name").focus();
        return;
    }


    else if (BillingAddress1.trim() == "") {
        $("#BillingAddress1").addClass('alert-danger');
        $("#BillingAddress1").focus();
        return;
    } else if (pinCode.trim() == "") {
        $("#pinCode").addClass('alert-danger');
        $("#pinCode").focus();
        return;
    } else if (Country.trim() == "") {
        $("#pinCode").addClass('alert-danger');
        $("#pinCode").focus();
        return;
    }

    else if (shippingAddress1.trim() == "") {
        $("#shippingAddress1").addClass('alert-danger');
        $("#shippingAddress1").focus();
        return;
    } else if (shippingpinCode.trim() == "") {
        $("#shippingpinCode").addClass('alert-danger');
        $("#shippingpinCode").focus();
        return;
    }
    else if (shippingCountry.trim() == "") {
        $("#shippingpinCode").addClass('alert-danger');
        $("#shippingpinCode").focus();
        return;
    } else if (!validGstin()) {
        $("#GSTIN1").addClass('alert-danger');
        $("#GSTIN2").addClass('alert-danger');
        $("#GSTIN3").addClass('alert-danger');

        return;
    }

    var CustomerDetails = {}

    //var 1.2
    CustomerDetails.UniqueID = uniqueID;
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

    //data: "{ UniqueID: '" + uniqueID + "',Name: '" + Name + "',BillingAddress1: '" + BillingAddress1 + "',BillingAddress2: '" + BillingAddress2 + "',BillingPin: '" + pinCode + "',shippingAddress1: '" + shippingAddress1 + "',shippingAddress2: '" + shippingAddress2 + "',shippingPin: '" + shippingpinCode + "', GSTIN:'" + GSTIN + "',BillingPhone:'" + BillingPhone + "',ShippingPhone:'" + ShippingPhone + "',contactperson:'" + contactperson + "' }",

    $.ajax({
        type: "POST",
        url: "Customer_general.aspx/SaveVendorMaster",
        data: JSON.stringify(CustomerDetails),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: "true",
        cache: "false",
        success: function (msg) {
            var obj = msg.d;
            if (obj.status == "Ok") {

                parent.ParentCustomerOnClose(obj.InternalId, $("#Name").val(), $("#uniqueId").val(), obj.BillingStateText, obj.BillingStateCode, obj.ShippingStateText, obj.ShippingStateCode);

            } else {
                console.log(obj.Msg);
            }
        },
        Error: function (x, e) {
            // On Error
        }
    });
}

function PinChange() {
    var pincode = $("#pinCode").val();

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
                $("#Country").val(obj.Country);
                $("#State").val(obj.state);
                $("#City").val(obj.city);
                $("#pinCode").removeClass('alert-danger');
            } else {
                $("#Country").val('');
                $("#State").val('');
                $("#City").val('');
                $("#pinCode").addClass('alert-danger');
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


function CheckUnique() {
    var uniqueId = $("#uniqueId").val();
    if (uniqueId.trim() != '') {
        $.ajax({
            type: "POST",
            url: "Customer_general.aspx/CheckuniqueId",
            data: "{ uniqueId: '" + uniqueId + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: "true",
            cache: "false",
            success: function (msg) {
                debugger;
                var obj = msg.d;
                if (obj.IsPresent) {
                    $("#uniqueId").addClass('alert-danger');
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

function ClearShipping() {
    $("#shippingAddress1").val('');
    $("#shippingAddress2").val('');
    $("#shippingpinCode").val('');
    $("#shippingCountry").val('');
    $("#shippingState").val('');
    $("#shippingCity").val('');
    $("#ShippingPhone").val('');

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


$(document).ready(function () {
    $("#uniqueId").focus();
});



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


$(document).ready(function () {
    $.ajax({
        url: "View/Services/ViewCustomerService.asmx/getGroupmasterDetails",
        contentType: "application/json; charset=utf-8",
        datatype: "JSON",
        type: "POST",
        success: function (data) {

            $("#GroupCust").empty();
            var grpdetl = data.d;

            debugger;

            var opts = "";
            opts = "<option value='0'>Select</option>";
            for (i in grpdetl)
                //if (opts == "") {
                //    opts = "<option value='0'>Select</option>"
                //}
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
});